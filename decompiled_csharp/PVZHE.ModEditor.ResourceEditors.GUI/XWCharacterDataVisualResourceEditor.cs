using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterDataVisualResourceEditor.cs")]
public class XWCharacterDataVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName HasCompleteCharacterDataVisualCoverage = "HasCompleteCharacterDataVisualCoverage";

		public static readonly StringName ResetCharacterDataBindings = "ResetCharacterDataBindings";

		public static readonly StringName DisposeCharacterDataBindings = "DisposeCharacterDataBindings";

		public static readonly StringName ReleaseCharacterDataPreviewBindings = "ReleaseCharacterDataPreviewBindings";

		public static readonly StringName BindCharacterDataMetadata = "BindCharacterDataMetadata";

		public static readonly StringName OnCharacterDataPropertyEdited = "OnCharacterDataPropertyEdited";

		public static readonly StringName LightweightCharacterDataPreview = "LightweightCharacterDataPreview";

		public static readonly StringName RenderCharacterDataItemEditor = "RenderCharacterDataItemEditor";

		public static readonly StringName RenderCharacterDataEditor = "RenderCharacterDataEditor";

		public static readonly StringName CreateCharacterDataVisualPreview = "CreateCharacterDataVisualPreview";

		public static readonly StringName BindCharacterDataVisualPreview = "BindCharacterDataVisualPreview";

		public static readonly StringName RenderArmorDataEditor = "RenderArmorDataEditor";

		public static readonly StringName RenderCustomDataEditor = "RenderCustomDataEditor";

		public static readonly StringName RenderDamagePointDataEditor = "RenderDamagePointDataEditor";

		public static readonly StringName RefreshEntryList = "RefreshEntryList";

		public static readonly StringName SelectEntry = "SelectEntry";

		public static readonly StringName RenderSelectedEntryEditor = "RenderSelectedEntryEditor";

		public static readonly StringName CreateArmorSlotEditor = "CreateArmorSlotEditor";

		public static readonly StringName CreateCustomConfigEditor = "CreateCustomConfigEditor";

		public static readonly StringName CreateCustomTextureArrayEditor = "CreateCustomTextureArrayEditor";

		public static readonly StringName AddCustomTexture = "AddCustomTexture";

		public static readonly StringName RemoveSelectedCustomTexture = "RemoveSelectedCustomTexture";

		public static readonly StringName MoveSelectedCustomTexture = "MoveSelectedCustomTexture";

		public static readonly StringName SetCustomTexture = "SetCustomTexture";

		public static readonly StringName CopyTexturePaths = "CopyTexturePaths";

		public static readonly StringName CreateDamagePointConfigEditor = "CreateDamagePointConfigEditor";

		public static readonly StringName CreateArmorTypeEditor = "CreateArmorTypeEditor";

		public static readonly StringName CreateArmorStageCard = "CreateArmorStageCard";

		public static readonly StringName AddArmorStage = "AddArmorStage";

		public static readonly StringName RemoveArmorStage = "RemoveArmorStage";

		public static readonly StringName MoveArmorStage = "MoveArmorStage";

		public static readonly StringName PreviewArmorStageThreshold = "PreviewArmorStageThreshold";

		public static readonly StringName CommitArmorStageThreshold = "CommitArmorStageThreshold";

		public static readonly StringName SetArmorStageTexture = "SetArmorStageTexture";

		public static readonly StringName SetArmorStageArrays = "SetArmorStageArrays";

		public static readonly StringName CopyThresholds = "CopyThresholds";

		public static readonly StringName ToggleArmorMethodFlag = "ToggleArmorMethodFlag";

		public static readonly StringName ShowAudioSelector = "ShowAudioSelector";

		public static readonly StringName AddResourceMetadataFields = "AddResourceMetadataFields";

		public static readonly StringName AddArmorSlot = "AddArmorSlot";

		public static readonly StringName RemoveSelectedArmorSlot = "RemoveSelectedArmorSlot";

		public static readonly StringName AddCustomConfig = "AddCustomConfig";

		public static readonly StringName RemoveSelectedCustomConfig = "RemoveSelectedCustomConfig";

		public static readonly StringName AddDamagePoint = "AddDamagePoint";

		public static readonly StringName RemoveSelectedDamagePoint = "RemoveSelectedDamagePoint";

		public static readonly StringName MoveSelectedAggregateEntry = "MoveSelectedAggregateEntry";

		public static readonly StringName CopyArmorSlots = "CopyArmorSlots";

		public static readonly StringName CopyCustomConfigs = "CopyCustomConfigs";

		public static readonly StringName CopyDamagePoints = "CopyDamagePoints";

		public static readonly StringName SetEntryProperty = "SetEntryProperty";

		public static readonly StringName NormalizeCharacterData = "NormalizeCharacterData";

		public static readonly StringName EnsureArmorList = "EnsureArmorList";

		public static readonly StringName EnsureCustomList = "EnsureCustomList";

		public static readonly StringName EnsureDamagePointList = "EnsureDamagePointList";

		public static readonly StringName ClampSelectedIndex = "ClampSelectedIndex";

		public static readonly StringName GetEntryCount = "GetEntryCount";

		public static readonly StringName GetArmorSlot = "GetArmorSlot";

		public static readonly StringName GetCustomConfig = "GetCustomConfig";

		public static readonly StringName GetDamagePointConfig = "GetDamagePointConfig";

		public static readonly StringName BuildEntryTitle = "BuildEntryTitle";

		public static readonly StringName UpdateVisualPreview = "UpdateVisualPreview";

		public static readonly StringName RebuildRuntimeCharacterPreview = "RebuildRuntimeCharacterPreview";

		public static readonly StringName ApplyRuntimeCharacterDataPreview = "ApplyRuntimeCharacterDataPreview";

		public static readonly StringName ToggleDamagePreview = "ToggleDamagePreview";

		public static readonly StringName ResetDamagePreview = "ResetDamagePreview";

		public static readonly StringName SetDamagePreviewPlaying = "SetDamagePreviewPlaying";

		public static readonly StringName ApplyPreviewHealthRatio = "ApplyPreviewHealthRatio";

		public static readonly StringName UpdateDamagePreviewControls = "UpdateDamagePreviewControls";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public static readonly StringName ApplyCharacterDataPreviewVisibility = "ApplyCharacterDataPreviewVisibility";

		public static readonly StringName GetPreviewArmorName = "GetPreviewArmorName";

		public static readonly StringName GetPreviewCustomName = "GetPreviewCustomName";

		public static readonly StringName FindOwningCharacterConfig = "FindOwningCharacterConfig";

		public static readonly StringName FindCharacterPackageRoot = "FindCharacterPackageRoot";

		public static readonly StringName CharacterConfigReferencesData = "CharacterConfigReferencesData";

		public static readonly StringName SameResource = "SameResource";

		public static readonly StringName FindRuntimeCharacter = "FindRuntimeCharacter";

		public static readonly StringName SetRuntimePreviewMissing = "SetRuntimePreviewMissing";

		public static readonly StringName OnRuntimePreviewGuiInput = "OnRuntimePreviewGuiInput";

		public static readonly StringName UpdateRuntimePreviewTransform = "UpdateRuntimePreviewTransform";

		public static readonly StringName NormalizePreviewPath = "NormalizePreviewPath";

		public static readonly StringName DrawCharacterDataPreview = "DrawCharacterDataPreview";

		public static readonly StringName DrawDamagePointConfigPreview = "DrawDamagePointConfigPreview";

		public static readonly StringName DrawArmorSlotConfigPreview = "DrawArmorSlotConfigPreview";

		public static readonly StringName DrawCustomConfigPreview = "DrawCustomConfigPreview";

		public static readonly StringName DrawArmorTypePreview = "DrawArmorTypePreview";

		public static readonly StringName DrawArmorPreview = "DrawArmorPreview";

		public static readonly StringName DrawDamagePointPreview = "DrawDamagePointPreview";

		public static readonly StringName DrawCustomPreview = "DrawCustomPreview";

		public static readonly StringName DrawHealthBar = "DrawHealthBar";

		public static readonly StringName BuildArmorPreviewStatus = "BuildArmorPreviewStatus";

		public static readonly StringName BuildDamagePreviewStatus = "BuildDamagePreviewStatus";

		public static readonly StringName BuildCustomPreviewStatus = "BuildCustomPreviewStatus";

		public static readonly StringName BuildDamagePointConfigPreviewStatus = "BuildDamagePointConfigPreviewStatus";

		public static readonly StringName BuildArmorSlotPreviewStatus = "BuildArmorSlotPreviewStatus";

		public static readonly StringName BuildCustomConfigPreviewStatus = "BuildCustomConfigPreviewStatus";

		public static readonly StringName BuildArmorTypePreviewStatus = "BuildArmorTypePreviewStatus";

		public static readonly StringName IsAggregateCharacterData = "IsAggregateCharacterData";

		public static readonly StringName IsSupportedCharacterDataResource = "IsSupportedCharacterDataResource";

		public static readonly StringName UpdateSummary = "UpdateSummary";

		public static readonly StringName BuildDataSummary = "BuildDataSummary";

		public static readonly StringName BuildCacheSummary = "BuildCacheSummary";

		public static readonly StringName AddBoundTextEdit = "AddBoundTextEdit";

		public static readonly StringName AddBoundSpinBox = "AddBoundSpinBox";

		public static readonly StringName AddBoundVector2Field = "AddBoundVector2Field";

		public static readonly StringName RefreshCharacterDataEditorFromHistory = "RefreshCharacterDataEditorFromHistory";

		public static readonly StringName CreateVectorSpinBox = "CreateVectorSpinBox";

		public static readonly StringName LoadTexturePreview = "LoadTexturePreview";

		public static readonly StringName WrapField = "WrapField";

		public static readonly StringName CreateSectionTitle = "CreateSectionTitle";

		public static readonly StringName CreateEmptyEntryLabel = "CreateEmptyEntryLabel";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName RuntimeCharacterPreviewBuildCount = "RuntimeCharacterPreviewBuildCount";

		public static readonly StringName RuntimeCharacterPreviewApplyCount = "RuntimeCharacterPreviewApplyCount";

		public static readonly StringName DamagePreviewProcessTickCount = "DamagePreviewProcessTickCount";

		public static readonly StringName IsDamagePreviewPlaying = "IsDamagePreviewPlaying";

		public static readonly StringName PreviewHealthRatio = "PreviewHealthRatio";

		public static readonly StringName CurrentRuntimeCharacterPreview = "CurrentRuntimeCharacterPreview";

		public static readonly StringName IsCharacterDataPreviewRendering = "IsCharacterDataPreviewRendering";

		public static readonly StringName _editingData = "_editingData";

		public static readonly StringName _selectedIndex = "_selectedIndex";

		public static readonly StringName _entryList = "_entryList";

		public static readonly StringName _entryEditorHost = "_entryEditorHost";

		public static readonly StringName _summaryLabel = "_summaryLabel";

		public static readonly StringName _cacheSummaryLabel = "_cacheSummaryLabel";

		public static readonly StringName _previewViewportContainer = "_previewViewportContainer";

		public static readonly StringName _previewRoot = "_previewRoot";

		public static readonly StringName _previewMissingLabel = "_previewMissingLabel";

		public static readonly StringName _configurationPreviewCanvas = "_configurationPreviewCanvas";

		public static readonly StringName _fallbackRibbon = "_fallbackRibbon";

		public static readonly StringName _fallbackRibbonLabel = "_fallbackRibbonLabel";

		public static readonly StringName _previewHealthValueLabel = "_previewHealthValueLabel";

		public static readonly StringName _previewPlaybackStateLabel = "_previewPlaybackStateLabel";

		public static readonly StringName _runtimeCharacterPreview = "_runtimeCharacterPreview";

		public static readonly StringName _previewOwnerConfig = "_previewOwnerConfig";

		public static readonly StringName _previewHealthSlider = "_previewHealthSlider";

		public static readonly StringName _previewStatusLabel = "_previewStatusLabel";

		public static readonly StringName _previewPlayPauseButton = "_previewPlayPauseButton";

		public static readonly StringName _previewResetButton = "_previewResetButton";

		public static readonly StringName _previewViewport = "_previewViewport";

		public static readonly StringName _previewHealthRatio = "_previewHealthRatio";

		public static readonly StringName _damagePreviewElapsed = "_damagePreviewElapsed";

		public static readonly StringName _damagePreviewPlaying = "_damagePreviewPlaying";

		public static readonly StringName _draggingPreview = "_draggingPreview";

		public static readonly StringName _previewPan = "_previewPan";

		public static readonly StringName _previewZoom = "_previewZoom";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _audioPicker = "_audioPicker";

		public static readonly StringName _audioTargetResource = "_audioTargetResource";

		public static readonly StringName _audioTargetProperty = "_audioTargetProperty";

		public static readonly StringName _selectedCustomTextureIndex = "_selectedCustomTextureIndex";

		public static readonly StringName _selectedArmorStageIndex = "_selectedArmorStageIndex";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string EditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterDataEditorLayout.tscn";

	private const string VisualPreviewScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterDataVisualPreview.tscn";

	private const double DamagePreviewDurationSeconds = 6.0;

	private static PackedScene _editorLayoutScene;

	private static PackedScene _visualPreviewScene;

	private static readonly TowerDefenseEnum.ARMOR_METHOD_FLAGS[] ArmorMethodFlagValues = Enum.GetValues<TowerDefenseEnum.ARMOR_METHOD_FLAGS>();

	private static readonly string[] DerivedCacheProperties = new string[10] { "armorDictionary", "fliterAllDictionary", "fliterOpenDictionary", "fliterCloseDictionary", "customDictionary", "fliterOpenAll", "fliterCloseAll", "damagePointDictionary", "fliterOpenAll", "fliterCloseAll" };

	private Resource _editingData;

	private int _selectedIndex = -1;

	private ItemList _entryList;

	private VBoxContainer _entryEditorHost;

	private Label _summaryLabel;

	private Label _cacheSummaryLabel;

	private SubViewportContainer _previewViewportContainer;

	private Node2D _previewRoot;

	private Label _previewMissingLabel;

	private Control _configurationPreviewCanvas;

	private Control _fallbackRibbon;

	private Label _fallbackRibbonLabel;

	private Label _previewHealthValueLabel;

	private Label _previewPlaybackStateLabel;

	private TowerDefenseCharacter _runtimeCharacterPreview;

	private TowerDefenseCharacterConfig _previewOwnerConfig;

	private HSlider _previewHealthSlider;

	private Label _previewStatusLabel;

	private Button _previewPlayPauseButton;

	private Button _previewResetButton;

	private SubViewport _previewViewport;

	private double _previewHealthRatio = 1.0;

	private double _damagePreviewElapsed;

	private bool _damagePreviewPlaying;

	private bool _draggingPreview;

	private Vector2 _previewPan;

	private float _previewZoom = 1f;

	private bool _updatingControls;

	private XWVisualPropertyBinding _metadataBinding;

	private XWVisualPropertyBinding _entryBinding;

	private XWGameplayResourcePickerWindow _audioPicker;

	private Resource _audioTargetResource;

	private string _audioTargetProperty = "";

	private int _selectedCustomTextureIndex = -1;

	private int _selectedArmorStageIndex = -1;

	public int RuntimeCharacterPreviewBuildCount { get; private set; }

	public int RuntimeCharacterPreviewApplyCount { get; private set; }

	public int DamagePreviewProcessTickCount { get; private set; }

	public bool IsDamagePreviewPlaying => _damagePreviewPlaying;

	public double PreviewHealthRatio => _previewHealthRatio;

	public TowerDefenseCharacter CurrentRuntimeCharacterPreview => _runtimeCharacterPreview;

	public bool IsCharacterDataPreviewRendering
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
		base._Ready();
	}

	public override void _Process(double delta)
	{
		if (_damagePreviewPlaying)
		{
			DamagePreviewProcessTickCount++;
			_damagePreviewElapsed = Math.Min(6.0, _damagePreviewElapsed + Math.Max(0.0, delta));
			_previewHealthRatio = Mathf.Clamp(1.0 - _damagePreviewElapsed / 6.0, 0.0, 1.0);
			ApplyPreviewHealthRatio();
			if (_damagePreviewElapsed >= 6.0)
			{
				SetDamagePreviewPlaying(playing: false, "受击演示完成 · 点击重播");
			}
		}
	}

	public override void _ExitTree()
	{
		DisposeCharacterDataBindings();
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		if (!IsSupportedCharacterDataResource(CurrentResource))
		{
			DisposeCharacterDataBindings();
			return;
		}
		_editingData = CurrentResource;
		ResetCharacterDataBindings();
		if (IsAggregateCharacterData(CurrentResource))
		{
			RenderCharacterDataEditor(CurrentResource);
		}
		else
		{
			RenderCharacterDataItemEditor(CurrentResource);
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompleteCharacterDataVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompleteCharacterDataVisualCoverage(Resource resource)
	{
		Type type = resource?.GetType();
		if (!(type == typeof(CharacterArmorData)) && !(type == typeof(CharacterCustomData)) && !(type == typeof(CharacterDamagePointData)) && !(type == typeof(ArmorSlotConfig)) && !(type == typeof(CharacterCustomConfig)) && !(type == typeof(CharacterDamagePointConfig)))
		{
			return type == typeof(TowerDefenseArmorTypeData);
		}
		return true;
	}

	private void ResetCharacterDataBindings()
	{
		ReleaseCharacterDataPreviewBindings();
		_metadataBinding?.Dispose();
		_entryBinding?.Dispose();
		_metadataBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnCharacterDataPropertyEdited);
		_entryBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnCharacterDataPropertyEdited);
		_entryList = null;
		_entryEditorHost = null;
	}

	private void DisposeCharacterDataBindings()
	{
		ReleaseCharacterDataPreviewBindings();
		_metadataBinding?.Dispose();
		_entryBinding?.Dispose();
		_metadataBinding = null;
		_entryBinding = null;
		_editingData = null;
		_audioTargetResource = null;
		_audioTargetProperty = "";
		_entryList = null;
		_entryEditorHost = null;
	}

	private void ReleaseCharacterDataPreviewBindings()
	{
		SetDamagePreviewPlaying(playing: false, "");
		if (GodotObject.IsInstanceValid(_previewViewport))
		{
			_previewViewport.RenderTargetUpdateMode = SubViewport.UpdateMode.Disabled;
		}
		_runtimeCharacterPreview = null;
		_previewOwnerConfig = null;
		_previewViewportContainer = null;
		_previewViewport = null;
		_previewRoot = null;
		_previewMissingLabel = null;
		_configurationPreviewCanvas = null;
		_fallbackRibbon = null;
		_fallbackRibbonLabel = null;
		_previewHealthValueLabel = null;
		_previewHealthSlider = null;
		_previewStatusLabel = null;
		_previewPlayPauseButton = null;
		_previewResetButton = null;
		_previewPlaybackStateLabel = null;
	}

	private void BindCharacterDataMetadata(VBoxContainer root, Resource resource)
	{
		if (GodotObject.IsInstanceValid(root) && GodotObject.IsInstanceValid(resource))
		{
			LineEdit nodeOrNull = root.GetNodeOrNull<LineEdit>("%ResourceNameEdit");
			CheckButton nodeOrNull2 = root.GetNodeOrNull<CheckButton>("%LocalToSceneCheck");
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				_metadataBinding.BindText(nodeOrNull, resource, "resource_name", LightweightCharacterDataPreview, this, "RefreshCharacterDataEditorFromHistory");
			}
			if (GodotObject.IsInstanceValid(nodeOrNull2))
			{
				_metadataBinding.BindToggle(nodeOrNull2, resource, "resource_local_to_scene", LightweightCharacterDataPreview, this, "RefreshCharacterDataEditorFromHistory");
			}
		}
	}

	private void OnCharacterDataPropertyEdited(bool committed)
	{
		if (committed)
		{
			NormalizeCharacterData(_editingData);
			RefreshEntryList();
			if (IsAggregateCharacterData(_editingData))
			{
				RenderSelectedEntryEditor();
			}
			UpdateSummary();
		}
		else
		{
			LightweightCharacterDataPreview();
		}
		NotifyCurrentResourceEdited();
	}

	private void LightweightCharacterDataPreview()
	{
		if (GodotObject.IsInstanceValid(_summaryLabel))
		{
			_summaryLabel.Text = BuildDataSummary(_editingData);
		}
		UpdateVisualPreview();
	}

	private void RenderCharacterDataItemEditor(VBoxContainer host, Resource resource)
	{
		if (!GodotObject.IsInstanceValid(host) || !GodotObject.IsInstanceValid(resource))
		{
			return;
		}
		if (!(resource is ArmorSlotConfig slot))
		{
			if (!(resource is CharacterCustomConfig config))
			{
				if (!(resource is CharacterDamagePointConfig config2))
				{
					if (resource is TowerDefenseArmorTypeData data)
					{
						CreateArmorTypeEditor(host, data);
					}
				}
				else
				{
					CreateDamagePointConfigEditor(host, resource, config2);
				}
			}
			else
			{
				CreateCustomConfigEditor(host, resource, config);
			}
		}
		else
		{
			CreateArmorSlotEditor(host, resource, slot);
		}
	}

	private void RenderCharacterDataItemEditor(Resource resource)
	{
		if (CanvasGrid == null || !GodotObject.IsInstanceValid(resource))
		{
			return;
		}
		CanvasGrid.Columns = 1;
		if (_editorLayoutScene == null)
		{
			_editorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterDataEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _editorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(vBoxContainer))
		{
			return;
		}
		CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		_summaryLabel = vBoxContainer.GetNodeOrNull<Label>("%SummaryLabel");
		_cacheSummaryLabel = vBoxContainer.GetNodeOrNull<Label>("%CacheSummaryLabel");
		BindCharacterDataMetadata(vBoxContainer, resource);
		Control nodeOrNull = vBoxContainer.GetNodeOrNull<Control>("%VisualPreview");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			BindCharacterDataVisualPreview(nodeOrNull, resource);
		}
		else
		{
			nodeOrNull = CreateCharacterDataVisualPreview(resource);
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				vBoxContainer.AddChild(nodeOrNull, forceReadableName: false, InternalMode.Disabled);
			}
		}
		Control nodeOrNull2 = vBoxContainer.GetNodeOrNull<Control>("%EditorSplit");
		if (GodotObject.IsInstanceValid(nodeOrNull2))
		{
			nodeOrNull2.Visible = false;
		}
		_entryEditorHost = vBoxContainer.GetNodeOrNull<VBoxContainer>("%StandaloneEditorHost") ?? vBoxContainer.GetNodeOrNull<VBoxContainer>("%EntryEditorHost");
		RenderCharacterDataItemEditor(_entryEditorHost, resource);
		PreviewList?.AddItem("资源类型: " + resource.GetType().Name);
		PreviewList?.AddItem("所有可配置属性都在当前游戏化界面直接编辑。");
		GraphList?.AddItem(resource.GetType().Name + " -> 运行状态模拟 -> 角色表现");
		UpdateSummary();
		UpdateVisualPreview();
	}

	private void RenderCharacterDataEditor(Resource resource)
	{
		if (CanvasGrid == null || resource == null)
		{
			return;
		}
		NormalizeCharacterData(resource);
		ClampSelectedIndex(resource);
		CanvasGrid.Columns = 1;
		if (_editorLayoutScene == null)
		{
			_editorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterDataEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _editorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(vBoxContainer))
		{
			return;
		}
		CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		_summaryLabel = vBoxContainer.GetNode<Label>("%SummaryLabel");
		_cacheSummaryLabel = vBoxContainer.GetNode<Label>("%CacheSummaryLabel");
		BindCharacterDataMetadata(vBoxContainer, resource);
		BindCharacterDataVisualPreview(vBoxContainer.GetNode<Control>("%VisualPreview"), resource);
		if (!(resource is CharacterArmorData armorData))
		{
			if (!(resource is CharacterCustomData customData))
			{
				if (resource is CharacterDamagePointData damagePointData)
				{
					RenderDamagePointDataEditor(vBoxContainer, damagePointData);
				}
			}
			else
			{
				RenderCustomDataEditor(vBoxContainer, customData);
			}
		}
		else
		{
			RenderArmorDataEditor(vBoxContainer, armorData);
		}
		RefreshEntryList();
		RenderSelectedEntryEditor();
		UpdateSummary();
		UpdateVisualPreview();
	}

	private Control CreateCharacterDataVisualPreview(Resource resource)
	{
		if (_visualPreviewScene == null)
		{
			_visualPreviewScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterDataVisualPreview.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		if (!GodotObject.IsInstanceValid(_visualPreviewScene))
		{
			return null;
		}
		Control control = _visualPreviewScene.Instantiate<Control>(PackedScene.GenEditState.Disabled);
		BindCharacterDataVisualPreview(control, resource);
		return control;
	}

	private void BindCharacterDataVisualPreview(Control panel, Resource resource)
	{
		_previewStatusLabel = panel.GetNode<Label>("Layout/Header/Status");
		_previewHealthSlider = panel.GetNode<HSlider>("Layout/HealthRow/Slider");
		_previewViewportContainer = panel.GetNode<SubViewportContainer>("%ViewportContainer");
		_previewViewport = panel.GetNode<SubViewport>("%CharacterViewport");
		_previewRoot = panel.GetNode<Node2D>("%PreviewRoot");
		_previewMissingLabel = panel.GetNode<Label>("%MissingLabel");
		_fallbackRibbon = panel.GetNodeOrNull<Control>("%FallbackRibbon");
		_fallbackRibbonLabel = panel.GetNodeOrNull<Label>("%FallbackRibbonLabel");
		_previewHealthValueLabel = panel.GetNodeOrNull<Label>("%HealthValueLabel");
		_previewPlaybackStateLabel = panel.GetNodeOrNull<Label>("%PlaybackStateLabel");
		_previewPlayPauseButton = panel.GetNodeOrNull<Button>("%DamagePlayPauseButton");
		_previewResetButton = panel.GetNodeOrNull<Button>("%DamageResetButton");
		if (_previewRoot.GetParent() is Control control)
		{
			_configurationPreviewCanvas = new Control
			{
				Name = "CharacterDataConfigurationPreview",
				Position = Vector2.Zero,
				Size = new Vector2(900f, 300f),
				MouseFilter = MouseFilterEnum.Ignore,
				Visible = false
			};
			_configurationPreviewCanvas.Draw += () =>
			{
				DrawCharacterDataPreview(_configurationPreviewCanvas);
			};
			control.AddChild(_configurationPreviewCanvas, forceReadableName: false, InternalMode.Disabled);
		}
		_previewHealthSlider.Value = _previewHealthRatio * 100.0;
		_previewHealthSlider.Editable = !(resource is CharacterCustomData) && !(resource is CharacterCustomConfig);
		_previewHealthSlider.ValueChanged += (double value) =>
		{
			_previewHealthRatio = Mathf.Clamp(value / 100.0, 0.0, 1.0);
			_damagePreviewElapsed = (1.0 - _previewHealthRatio) * 6.0;
			if (GodotObject.IsInstanceValid(_previewHealthValueLabel))
			{
				_previewHealthValueLabel.Text = $"{_previewHealthRatio:P0}";
			}
			UpdateVisualPreview();
		};
		if (GodotObject.IsInstanceValid(_previewPlayPauseButton))
		{
			_previewPlayPauseButton.Pressed += ToggleDamagePreview;
		}
		if (GodotObject.IsInstanceValid(_previewResetButton))
		{
			_previewResetButton.Pressed += ResetDamagePreview;
		}
		_previewViewportContainer.GuiInput += OnRuntimePreviewGuiInput;
		ApplyCharacterDataPreviewVisibility(IsVisibleInTree());
		UpdateDamagePreviewControls("拖动生命值，或播放一次完整受击过程");
		RebuildRuntimeCharacterPreview(resource);
	}

	private void RenderArmorDataEditor(VBoxContainer root, CharacterArmorData armorData)
	{
		root.TooltipText = $"armorList: {armorData.armorList?.Count ?? 0}";
		BindSplitEditorPanel(root, "护甲槽", "护甲槽决定动画替换方式、挂点、偏移和受伤阶段触发阈值。", () =>
		{
			AddArmorSlot(armorData);
		}, RemoveSelectedArmorSlot);
	}

	private void RenderCustomDataEditor(VBoxContainer root, CharacterCustomData customData)
	{
		root.TooltipText = $"customList: {customData.customList?.Count ?? 0}";
		BindSplitEditorPanel(root, "自定义外观", "自定义外观用于解锁皮肤、切换动画滤镜，以及按受伤阶段替换贴图。", () =>
		{
			AddCustomConfig(customData);
		}, RemoveSelectedCustomConfig);
	}

	private void RenderDamagePointDataEditor(VBoxContainer root, CharacterDamagePointData damagePointData)
	{
		root.TooltipText = $"damagePointList: {damagePointData.damagePointList?.Count ?? 0}";
		BindSplitEditorPanel(root, "受伤点", "受伤点按生命百分比从高到低触发，可切换动画滤镜、替换部件和播放受伤特效。", () =>
		{
			AddDamagePoint(damagePointData);
		}, RemoveSelectedDamagePoint);
	}

	private void BindSplitEditorPanel(VBoxContainer root, string title, string description, Action addPressed, Action removePressed)
	{
		root.GetNode<Label>("%EntryTitle").Text = title;
		root.GetNode<Label>("%EntryDescription").Text = description;
		root.GetNode<Button>("%AddButton").Pressed += () =>
		{
			addPressed?.Invoke();
		};
		root.GetNode<Button>("%RemoveButton").Pressed += () =>
		{
			removePressed?.Invoke();
		};
		if (root.GetNode<Button>("%AddButton").GetParent() is Container container)
		{
			container.AddChild(CreateToolbarButton("↑", () =>
			{
				MoveSelectedAggregateEntry(-1);
			}), forceReadableName: false, InternalMode.Disabled);
			container.AddChild(CreateToolbarButton("↓", () =>
			{
				MoveSelectedAggregateEntry(1);
			}), forceReadableName: false, InternalMode.Disabled);
		}
		_entryList = root.GetNode<ItemList>("%EntryList");
		_entryList.ItemSelected += (long index) =>
		{
			SelectEntry((int)index);
		};
		_entryEditorHost = root.GetNodeOrNull<VBoxContainer>("%EntryEditorContentHost") ?? root.GetNode<VBoxContainer>("%EntryEditorHost");
	}

	private void RefreshEntryList()
	{
		if (!GodotObject.IsInstanceValid(_entryList))
		{
			return;
		}
		_updatingControls = true;
		try
		{
			_entryList.Clear();
			int entryCount = GetEntryCount(_editingData);
			if (entryCount == 0)
			{
				_entryList.AddItem("未配置");
				return;
			}
			for (int i = 0; i < entryCount; i++)
			{
				_entryList.AddItem(BuildEntryTitle(_editingData, i));
			}
			ClampSelectedIndex(_editingData);
			if (_selectedIndex >= 0 && _selectedIndex < _entryList.ItemCount)
			{
				_entryList.Select(_selectedIndex);
			}
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void SelectEntry(int index)
	{
		if (!_updatingControls)
		{
			if (GetEntryCount(_editingData) == 0)
			{
				_selectedIndex = -1;
				RenderSelectedEntryEditor();
				UpdateVisualPreview();
			}
			else
			{
				_selectedIndex = index;
				ClampSelectedIndex(_editingData);
				RenderSelectedEntryEditor();
				UpdateVisualPreview();
			}
		}
	}

	private void RenderSelectedEntryEditor()
	{
		if (!GodotObject.IsInstanceValid(_entryEditorHost))
		{
			return;
		}
		_entryBinding?.Dispose();
		_entryBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnCharacterDataPropertyEdited);
		foreach (Node child in _entryEditorHost.GetChildren())
		{
			child.QueueFree();
		}
		if (_editingData is CharacterArmorData characterArmorData)
		{
			ArmorSlotConfig armorSlot = GetArmorSlot(characterArmorData);
			if (armorSlot == null)
			{
				_entryEditorHost.AddChild(CreateEmptyEntryLabel("还没有护甲槽。点击左侧添加后编辑。"), forceReadableName: false, InternalMode.Disabled);
			}
			else
			{
				CreateArmorSlotEditor(_entryEditorHost, characterArmorData, armorSlot);
			}
		}
		else if (_editingData is CharacterCustomData characterCustomData)
		{
			CharacterCustomConfig customConfig = GetCustomConfig(characterCustomData);
			if (customConfig == null)
			{
				_entryEditorHost.AddChild(CreateEmptyEntryLabel("还没有自定义外观。点击左侧添加后编辑。"), forceReadableName: false, InternalMode.Disabled);
			}
			else
			{
				CreateCustomConfigEditor(_entryEditorHost, characterCustomData, customConfig);
			}
		}
		else if (_editingData is CharacterDamagePointData characterDamagePointData)
		{
			CharacterDamagePointConfig damagePointConfig = GetDamagePointConfig(characterDamagePointData);
			if (damagePointConfig == null)
			{
				_entryEditorHost.AddChild(CreateEmptyEntryLabel("还没有受伤点。点击左侧添加后编辑。"), forceReadableName: false, InternalMode.Disabled);
			}
			else
			{
				CreateDamagePointConfigEditor(_entryEditorHost, characterDamagePointData, damagePointConfig);
			}
		}
	}

	private void CreateArmorSlotEditor(VBoxContainer host, Resource owner, ArmorSlotConfig slot)
	{
		host.AddChild(CreateSectionTitle($"护甲槽 {_selectedIndex + 1}"), forceReadableName: false, InternalMode.Disabled);
		AddResourceMetadataFields(host, slot);
		host.AddChild(AddBoundLineEdit("ArmorNameLineEdit", "armorName", slot, "armorName"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddOptionField("ArmorReplaceMethodOption", "replaceMethod", slot.replaceMethod, new string[2] { "Media", "Sprite" }, (string value) =>
		{
			SetEntryProperty(owner, slot, "replaceMethod", Variant.From(in value));
		}), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundLineEdit("ArmorReplaceMediaLineEdit", "replaceMediaName", slot, "replaceMediaName", (string value) => Variant.From<StringName>(new StringName(value))), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundLineEdit("ArmorSlotPathLineEdit", "slotPath", slot, "slotPath", (string value) => Variant.From<NodePath>(new NodePath(value))), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundVector2Field("ArmorOffset", "offset", slot, "offset"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundSpinBox("ArmorRotationSpinBox", "rotation", slot, "rotation", -3600.0, 3600.0, 0.1), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundVector2Field("ArmorScale", "scale", slot, "scale"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundSpinBox("ArmorDamagePointSpinBox", "damagePoint", slot, "damagePoint", -1.0, 1.0, 0.01), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundTextEdit("ArmorOpenFilterText", "openFliter", slot, "openFliter"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundTextEdit("ArmorCloseFilterText", "closeFliter", slot, "closeFliter"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundTextEdit("ArmorDestroyFilterText", "destroyFliter", slot, "destroyFliter"), forceReadableName: false, InternalMode.Disabled);
	}

	private void CreateCustomConfigEditor(VBoxContainer host, Resource owner, CharacterCustomConfig config)
	{
		host.AddChild(CreateSectionTitle($"自定义外观 {_selectedIndex + 1}"), forceReadableName: false, InternalMode.Disabled);
		AddResourceMetadataFields(host, config);
		host.AddChild(AddBoundLineEdit("CustomOpenKeyLineEdit", "openKey", config, "openKey"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundLineEdit("CustomNameLineEdit", "customName", config, "customName"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddOptionField("CustomTypeOption", "type", config.type, new string[2] { "White", "Gold" }, (string value) =>
		{
			SetEntryProperty(owner, config, "type", Variant.From(in value));
		}), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundLineEdit("CustomHandbookNameLineEdit", "customHandbookName", config, "customHandbookName"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundLineEdit("CustomHandbookAccessLineEdit", "customHandbookAccess", config, "customHandbookAccess"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundTextEdit("CustomHandbookStoryText", "customHandbookStory", config, "customHandbookStory"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundTextEdit("CustomFilterOpenText", "animeFliterOpen", config, "animeFliterOpen"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundTextEdit("CustomFilterCloseText", "animeFliterClose", config, "animeFliterClose"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundLineEdit("CustomDamageMediaLineEdit", "damagePointChangeMediaName", config, "damagePointChangeMediaName"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(CreateCustomTextureArrayEditor(config), forceReadableName: false, InternalMode.Disabled);
	}

	private Control CreateCustomTextureArrayEditor(CharacterCustomConfig config)
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Name = "CustomTextureArrayEditor",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddChild(new Label
		{
			Text = "damagePointChangeMediaTexturePaths · 受伤阶段贴图",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateToolbarButton("＋", () =>
		{
			AddCustomTexture(config);
		}), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateToolbarButton("－", () =>
		{
			RemoveSelectedCustomTexture(config);
		}), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateToolbarButton("←", () =>
		{
			MoveSelectedCustomTexture(config, -1);
		}), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateToolbarButton("→", () =>
		{
			MoveSelectedCustomTexture(config, 1);
		}), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer2 = new HBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		Array<string> array = config.damagePointChangeMediaTexturePaths ?? new Array<string>();
		if (array.Count == 0)
		{
			hBoxContainer2.AddChild(CreateEmptyEntryLabel("点击＋添加第一张阶段贴图"), forceReadableName: false, InternalMode.Disabled);
		}
		for (int num = 0; num < array.Count; num++)
		{
			int index = num;
			VBoxContainer vBoxContainer2 = new VBoxContainer
			{
				CustomMinimumSize = new Vector2(142f, 96f)
			};
			Button button = new Button
			{
				Text = $"阶段 {num + 1}",
				ToggleMode = true,
				ButtonPressed = (num == _selectedCustomTextureIndex)
			};
			button.Pressed += () =>
			{
				_selectedCustomTextureIndex = index;
				RenderSelectedEntryEditor();
			};
			vBoxContainer2.AddChild(button, forceReadableName: false, InternalMode.Disabled);
			XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
			xWResourcePicker.Name = $"CustomTexturePicker{num}";
			xWResourcePicker.Setup("Texture2D");
			xWResourcePicker.SetEditedResource(LoadTexturePreview(array[num]));
			xWResourcePicker.ResourceChanged += (Resource resource) =>
			{
				SetCustomTexture(config, index, resource?.ResourcePath ?? string.Empty);
			};
			vBoxContainer2.AddChild(xWResourcePicker, forceReadableName: false, InternalMode.Disabled);
			hBoxContainer2.AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		}
		vBoxContainer.AddChild(hBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private void AddCustomTexture(CharacterCustomConfig config)
	{
		Array<string> array = CopyTexturePaths(config?.damagePointChangeMediaTexturePaths);
		array.Add(string.Empty);
		_selectedCustomTextureIndex = array.Count - 1;
		_entryBinding.SetValue(config, "damagePointChangeMediaTexturePaths", array, "添加外观阶段贴图", this, "RefreshCharacterDataEditorFromHistory");
	}

	private void RemoveSelectedCustomTexture(CharacterCustomConfig config)
	{
		Array<string> array = CopyTexturePaths(config?.damagePointChangeMediaTexturePaths);
		if (_selectedCustomTextureIndex >= 0 && _selectedCustomTextureIndex < array.Count)
		{
			array.RemoveAt(_selectedCustomTextureIndex);
			_selectedCustomTextureIndex = Mathf.Min(_selectedCustomTextureIndex, array.Count - 1);
			_entryBinding.SetValue(config, "damagePointChangeMediaTexturePaths", array, "删除外观阶段贴图", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private void MoveSelectedCustomTexture(CharacterCustomConfig config, int direction)
	{
		Array<string> array = CopyTexturePaths(config?.damagePointChangeMediaTexturePaths);
		int num = Mathf.Clamp(_selectedCustomTextureIndex + direction, 0, array.Count - 1);
		if (_selectedCustomTextureIndex >= 0 && num != _selectedCustomTextureIndex)
		{
			string item = array[_selectedCustomTextureIndex];
			array.RemoveAt(_selectedCustomTextureIndex);
			array.Insert(num, item);
			_selectedCustomTextureIndex = num;
			_entryBinding.SetValue(config, "damagePointChangeMediaTexturePaths", array, "调整外观阶段贴图顺序", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private void SetCustomTexture(CharacterCustomConfig config, int index, string texturePath)
	{
		Array<string> array = CopyTexturePaths(config?.damagePointChangeMediaTexturePaths);
		if (index >= 0 && index < array.Count)
		{
			array[index] = texturePath ?? string.Empty;
			_entryBinding.SetValue(config, "damagePointChangeMediaTexturePaths", array, "更换外观阶段贴图", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private static Array<string> CopyTexturePaths(Array<string> source)
	{
		if (source != null)
		{
			return new Array<string>(source);
		}
		return new Array<string>();
	}

	private void CreateDamagePointConfigEditor(VBoxContainer host, Resource owner, CharacterDamagePointConfig config)
	{
		host.AddChild(CreateSectionTitle($"受伤点 {_selectedIndex + 1}"), forceReadableName: false, InternalMode.Disabled);
		AddResourceMetadataFields(host, config);
		host.AddChild(AddBoundLineEdit("DamagePointNameLineEdit", "damagePointName", config, "damagePointName"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundSpinBox("DamagePercentageSpinBox", "damagePersontage", config, "damagePersontage", 0.0, 1.0, 0.01), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddResourcePicker("DamageEffectPicker", "animeEffect", "PackedScene", config.animeEffect, (Resource value) =>
		{
			SetEntryProperty(owner, config, "animeEffect", Variant.From(in value));
		}), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundVector2Field("DamageEffectOffset", "animeEffectOffset", config, "animeEffectOffset"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoolField("DamageDropCheck", "isDrop", config.isDrop, (bool value) =>
		{
			SetEntryProperty(owner, config, "isDrop", Variant.From(in value));
		}), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundTextEdit("DamageFilterOpenText", "animeFliterOpen", config, "animeFliterOpen"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundTextEdit("DamageFilterCloseText", "animeFliterClose", config, "animeFliterClose"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundLineEdit("DamageReplaceMediaLineEdit", "replaceMediaName", config, "replaceMediaName", (string value) => Variant.From<StringName>(new StringName(value))), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddResourcePicker("DamageReplaceTexturePicker", "replaceMediaTexturePath", "Texture2D", LoadTexturePreview(config.replaceMediaTexturePath), (Resource value) =>
		{
			XWCharacterDataVisualResourceEditor xWCharacterDataVisualResourceEditor = this;
			Resource owner2 = owner;
			CharacterDamagePointConfig entry = config;
			string from = value?.ResourcePath ?? string.Empty;
			xWCharacterDataVisualResourceEditor.SetEntryProperty(owner2, entry, "replaceMediaTexturePath", Variant.From(in from));
		}), forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hBoxContainer.AddChild(AddBoundLineEdit("DamageAudioLineEdit", "damageAudio", config, "damageAudio"), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateToolbarButton("游戏音频图鉴", () =>
		{
			_audioTargetResource = config;
			ShowAudioSelector("damageAudio");
		}), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
	}

	private void CreateArmorTypeEditor(VBoxContainer host, TowerDefenseArmorTypeData data)
	{
		if (!GodotObject.IsInstanceValid(host) || !GodotObject.IsInstanceValid(data))
		{
			return;
		}
		host.AddChild(CreateSectionTitle("护甲类型 · 阶段与受击行为"), forceReadableName: false, InternalMode.Disabled);
		AddResourceMetadataFields(host, data);
		host.AddChild(AddBoundLineEdit("ArmorTypeNameLineEdit", "armorName", data, "armorName"), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundSpinBox("ArmorTypeDamagePoint", "damagePoint", data, "damagePoint", 0.0, 100000000.0, 1.0), forceReadableName: false, InternalMode.Disabled);
		OptionButton optionButton = new OptionButton
		{
			Name = "ArmorHeightOption",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		TowerDefenseEnum.CHARACTER_HEIGHT[] values = Enum.GetValues<TowerDefenseEnum.CHARACTER_HEIGHT>();
		for (int i = 0; i < values.Length; i++)
		{
			TowerDefenseEnum.CHARACTER_HEIGHT id = values[i];
			optionButton.AddItem(id.ToString(), (int)id);
		}
		optionButton.Select((int)data.height);
		optionButton.ItemSelected += (long index) =>
		{
			SetEntryProperty(data, data, "height", Variant.From<int>((int)index));
		};
		host.AddChild(WrapField("height", optionButton), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(CreateArmorStageCard(data), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundSpinBox("ArmorLimitMaxHit", "limitMaxHit", data, "limitMaxHit", -1.0, 100000000.0, 1.0), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(AddBoundSpinBox("ArmorExplodePercentage", "explodePersontage", data, "explodePersontage", 0.0, 10.0, 0.01), forceReadableName: false, InternalMode.Disabled);
		FlowContainer flowContainer = new FlowContainer
		{
			Name = "ArmorMethodFlags",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		flowContainer.AddChild(new Label
		{
			Text = "ARMOR_METHOD_FLAGS",
			CustomMinimumSize = new Vector2(168f, 0f)
		}, forceReadableName: false, InternalMode.Disabled);
		TowerDefenseEnum.ARMOR_METHOD_FLAGS[] armorMethodFlagValues = ArmorMethodFlagValues;
		for (int i = 0; i < armorMethodFlagValues.Length; i++)
		{
			TowerDefenseEnum.ARMOR_METHOD_FLAGS flag = armorMethodFlagValues[i];
			CheckButton checkButton = new CheckButton
			{
				Text = flag.ToString(),
				ButtonPressed = (((uint)data.armorMethodFlags & (uint)flag) != 0)
			};
			checkButton.Toggled += (bool enabled) =>
			{
				ToggleArmorMethodFlag(data, flag, enabled);
			};
			flowContainer.AddChild(checkButton, forceReadableName: false, InternalMode.Disabled);
		}
		host.AddChild(flowContainer, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hBoxContainer.AddChild(CreateToolbarButton("受击音效 · " + EmptyToPlaceholder(data.impactAudio), () =>
		{
			_audioTargetResource = data;
			ShowAudioSelector("impactAudio");
		}), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateToolbarButton("破坏音效 · " + EmptyToPlaceholder(data.damageAudio), () =>
		{
			_audioTargetResource = data;
			ShowAudioSelector("damageAudio");
		}), forceReadableName: false, InternalMode.Disabled);
		host.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
	}

	private Control CreateArmorStageCard(TowerDefenseArmorTypeData data)
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Name = "ArmorStageCards",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddChild(new Label
		{
			Text = "stagePersontage / stageAnimeTexturePaths",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateToolbarButton("＋", () =>
		{
			AddArmorStage(data);
		}), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateToolbarButton("－", () =>
		{
			RemoveArmorStage(data);
		}), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateToolbarButton("←", () =>
		{
			MoveArmorStage(data, -1);
		}), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateToolbarButton("→", () =>
		{
			MoveArmorStage(data, 1);
		}), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
		xWResourcePicker.Name = "ArmorBaseTexturePicker";
		xWResourcePicker.Setup("Texture2D");
		xWResourcePicker.SetEditedResource((data.stageAnimeTexturePaths != null && data.stageAnimeTexturePaths.Count > 0) ? LoadTexturePreview(data.stageAnimeTexturePaths[0]) : null);
		xWResourcePicker.ResourceChanged += (Resource resource) =>
		{
			SetArmorStageTexture(data, 0, resource?.ResourcePath ?? string.Empty);
		};
		vBoxContainer.AddChild(WrapField("初始外观", xWResourcePicker), forceReadableName: false, InternalMode.Disabled);
		int num = data.stagePersontage?.Count ?? 0;
		for (int num2 = 0; num2 < num; num2++)
		{
			int index = num2;
			HBoxContainer hBoxContainer2 = new HBoxContainer
			{
				CustomMinimumSize = new Vector2(0f, 42f),
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			Button button = new Button
			{
				Text = $"阶段 {num2 + 1}",
				ToggleMode = true,
				ButtonPressed = (num2 == _selectedArmorStageIndex)
			};
			button.Pressed += () =>
			{
				_selectedArmorStageIndex = index;
				RefreshCharacterDataEditorFromHistory();
			};
			hBoxContainer2.AddChild(button, forceReadableName: false, InternalMode.Disabled);
			SpinBox threshold = new SpinBox
			{
				MinValue = 0.0,
				MaxValue = 1.0,
				Step = 0.01,
				Value = data.stagePersontage[num2],
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			threshold.FocusEntered += () =>
			{
				_entryBinding.BeginEdit(data, "stagePersontage");
			};
			threshold.ValueChanged += (double value) =>
			{
				PreviewArmorStageThreshold(data, index, value);
			};
			threshold.FocusExited += () =>
			{
				CommitArmorStageThreshold(data, index, threshold.Value);
			};
			hBoxContainer2.AddChild(threshold, forceReadableName: false, InternalMode.Disabled);
			XWResourcePicker xWResourcePicker2 = XWResourcePicker.Create();
			xWResourcePicker2.Setup("Texture2D");
			int textureIndex = num2 + 1;
			xWResourcePicker2.SetEditedResource((data.stageAnimeTexturePaths != null && textureIndex < data.stageAnimeTexturePaths.Count) ? LoadTexturePreview(data.stageAnimeTexturePaths[textureIndex]) : null);
			xWResourcePicker2.ResourceChanged += (Resource resource) =>
			{
				SetArmorStageTexture(data, textureIndex, resource?.ResourcePath ?? string.Empty);
			};
			hBoxContainer2.AddChild(xWResourcePicker2, forceReadableName: false, InternalMode.Disabled);
			vBoxContainer.AddChild(hBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		}
		return vBoxContainer;
	}

	private void AddArmorStage(TowerDefenseArmorTypeData data)
	{
		Array<double> array = CopyThresholds(data?.stagePersontage);
		Array<string> array2 = CopyTexturePaths(data?.stageAnimeTexturePaths);
		array.Add(0.5);
		array2.Add(string.Empty);
		_selectedArmorStageIndex = array.Count - 1;
		StringName refreshMethod = "RefreshCharacterDataEditorFromHistory";
		SetArmorStageArrays(data, array, array2, "添加护甲阶段", refreshMethod);
	}

	private void RemoveArmorStage(TowerDefenseArmorTypeData data)
	{
		Array<double> array = CopyThresholds(data?.stagePersontage);
		Array<string> array2 = CopyTexturePaths(data?.stageAnimeTexturePaths);
		if (_selectedArmorStageIndex >= 0 && _selectedArmorStageIndex < array.Count)
		{
			array.RemoveAt(_selectedArmorStageIndex);
			int num = _selectedArmorStageIndex + 1;
			if (num < array2.Count)
			{
				array2.RemoveAt(num);
			}
			_selectedArmorStageIndex = Mathf.Min(_selectedArmorStageIndex, array.Count - 1);
			StringName refreshMethod = "RefreshCharacterDataEditorFromHistory";
			SetArmorStageArrays(data, array, array2, "删除护甲阶段", refreshMethod);
		}
	}

	private void MoveArmorStage(TowerDefenseArmorTypeData data, int direction)
	{
		Array<double> array = CopyThresholds(data?.stagePersontage);
		Array<string> array2 = CopyTexturePaths(data?.stageAnimeTexturePaths);
		int num = Mathf.Clamp(_selectedArmorStageIndex + direction, 0, array.Count - 1);
		if (_selectedArmorStageIndex >= 0 && num != _selectedArmorStageIndex)
		{
			double item = array[_selectedArmorStageIndex];
			array.RemoveAt(_selectedArmorStageIndex);
			array.Insert(num, item);
			int num2 = _selectedArmorStageIndex + 1;
			int a = num + 1;
			if (num2 < array2.Count)
			{
				string item2 = array2[num2];
				array2.RemoveAt(num2);
				array2.Insert(Mathf.Min(a, array2.Count), item2);
			}
			_selectedArmorStageIndex = num;
			StringName refreshMethod = "RefreshCharacterDataEditorFromHistory";
			SetArmorStageArrays(data, array, array2, "调整护甲阶段顺序", refreshMethod);
		}
	}

	private void PreviewArmorStageThreshold(TowerDefenseArmorTypeData data, int index, double value)
	{
		Array<double> array = CopyThresholds(data?.stagePersontage);
		if (index >= 0 && index < array.Count)
		{
			array[index] = value;
			_entryBinding.PreviewValue(data, "stagePersontage", array);
			LightweightCharacterDataPreview();
		}
	}

	private void CommitArmorStageThreshold(TowerDefenseArmorTypeData data, int index, double value)
	{
		Array<double> array = CopyThresholds(data?.stagePersontage);
		if (index >= 0 && index < array.Count)
		{
			array[index] = value;
			_entryBinding.CommitEdit(data, "stagePersontage", array, "修改护甲阶段阈值", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private void SetArmorStageTexture(TowerDefenseArmorTypeData data, int index, string texturePath)
	{
		Array<string> array = CopyTexturePaths(data?.stageAnimeTexturePaths);
		while (array.Count <= index)
		{
			array.Add(string.Empty);
		}
		array[index] = texturePath ?? string.Empty;
		_entryBinding.SetValue(data, "stageAnimeTexturePaths", array, "更换护甲阶段贴图", this, "RefreshCharacterDataEditorFromHistory");
	}

	private void SetArmorStageArrays(TowerDefenseArmorTypeData data, Array<double> stagePersontage, Array<string> stageAnimeTexturePaths, string actionName, StringName refreshMethod)
	{
		if (GodotObject.IsInstanceValid(data))
		{
			Array<double> array = CopyThresholds(data.stagePersontage);
			Array<string> array2 = CopyTexturePaths(data.stageAnimeTexturePaths);
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager == null)
			{
				data.stagePersontage = stagePersontage;
				data.stageAnimeTexturePaths = stageAnimeTexturePaths;
				OnCharacterDataPropertyEdited(committed: true);
				return;
			}
			xWUndoRedoManager.CreateAction(actionName);
			xWUndoRedoManager.AddDoProperty(data, "stagePersontage", stagePersontage);
			xWUndoRedoManager.AddDoProperty(data, "stageAnimeTexturePaths", stageAnimeTexturePaths);
			xWUndoRedoManager.AddUndoProperty(data, "stagePersontage", array);
			xWUndoRedoManager.AddUndoProperty(data, "stageAnimeTexturePaths", array2);
			xWUndoRedoManager.AddDoMethod(this, refreshMethod.ToString());
			xWUndoRedoManager.AddUndoMethod(this, refreshMethod.ToString());
			xWUndoRedoManager.CommitAction();
			OnCharacterDataPropertyEdited(committed: true);
		}
	}

	private static Array<double> CopyThresholds(Array<double> source)
	{
		if (source != null)
		{
			return new Array<double>(source);
		}
		return new Array<double>();
	}

	private void ToggleArmorMethodFlag(TowerDefenseArmorTypeData data, TowerDefenseEnum.ARMOR_METHOD_FLAGS flag, bool enabled)
	{
		SetEntryProperty(data, data, "armorMethodFlags", Variant.From<int>(enabled ? (data.armorMethodFlags | (int)flag) : (data.armorMethodFlags & (int)(~flag))));
	}

	private void ShowAudioSelector(string propertyName)
	{
		if (!GodotObject.IsInstanceValid(_audioTargetResource))
		{
			return;
		}
		_audioTargetProperty = ((propertyName == "damageAudio") ? "damageAudio" : "impactAudio");
		if (_audioPicker == null)
		{
			_audioPicker = XWGameplayResourcePickerWindow.Create();
		}
		if (GodotObject.IsInstanceValid(_audioPicker))
		{
			if (_audioPicker.GetParent() == null)
			{
				AddChild(_audioPicker, forceReadableName: false, InternalMode.Disabled);
			}
			string currentKey = _audioTargetResource.Get(_audioTargetProperty).AsString();
			_audioPicker.Open(XWGameplayResourceKind.Audio, currentKey, ApplyArmorAudioChoice, (XWGameplayResourceChoice choice) => choice.Kind == XWGameplayResourceKind.Audio, lockKind: true, "选择游戏音效");
		}
	}

	private void ApplyArmorAudioChoice(XWGameplayResourceChoice choice)
	{
		if (!(choice == null) && choice.Kind == XWGameplayResourceKind.Audio && GodotObject.IsInstanceValid(_audioTargetResource))
		{
			SetEntryProperty(_audioTargetResource, _audioTargetResource, _audioTargetProperty, Variant.From<string>(choice.Key));
		}
	}

	private void AddResourceMetadataFields(VBoxContainer host, Resource resource)
	{
		host.AddChild(AddBoundLineEdit(resource.GetType().Name + "ResourceName", "resource_name", resource, "resource_name"), forceReadableName: false, InternalMode.Disabled);
		CheckButton checkButton = new CheckButton
		{
			Text = "resource_local_to_scene",
			ButtonPressed = resource.ResourceLocalToScene
		};
		_entryBinding.BindToggle(checkButton, resource, "resource_local_to_scene", LightweightCharacterDataPreview, this, "RefreshCharacterDataEditorFromHistory");
		host.AddChild(WrapField("resource_local_to_scene", checkButton), forceReadableName: false, InternalMode.Disabled);
	}

	private void AddArmorSlot(CharacterArmorData data)
	{
		if (data != null)
		{
			EnsureArmorList(data);
			ArmorSlotConfig item = new ArmorSlotConfig
			{
				ResourceName = $"ArmorSlot{data.armorList.Count + 1}",
				armorName = $"Armor{data.armorList.Count + 1}",
				replaceMethod = "Media",
				scale = Vector2.One,
				damagePoint = -1.0
			};
			Array<ArmorSlotConfig> array = CopyArmorSlots(data.armorList);
			array.Add(item);
			_selectedIndex = array.Count - 1;
			_entryBinding.SetValue(data, "armorList", array, "添加护甲槽", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private void RemoveSelectedArmorSlot()
	{
		if (_editingData is CharacterArmorData { armorList: not null } characterArmorData && _selectedIndex >= 0 && _selectedIndex < characterArmorData.armorList.Count)
		{
			Array<ArmorSlotConfig> array = CopyArmorSlots(characterArmorData.armorList);
			array.RemoveAt(_selectedIndex);
			_selectedIndex = Mathf.Min(_selectedIndex, array.Count - 1);
			_entryBinding.SetValue(characterArmorData, "armorList", array, "删除护甲槽", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private void AddCustomConfig(CharacterCustomData data)
	{
		if (data != null)
		{
			EnsureCustomList(data);
			CharacterCustomConfig item = new CharacterCustomConfig
			{
				ResourceName = $"CustomConfig{data.customList.Count + 1}",
				openKey = $"Custom{data.customList.Count + 1}",
				customName = $"Custom{data.customList.Count + 1}",
				type = "White"
			};
			Array<CharacterCustomConfig> array = CopyCustomConfigs(data.customList);
			array.Add(item);
			_selectedIndex = array.Count - 1;
			_entryBinding.SetValue(data, "customList", array, "添加自定义外观", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private void RemoveSelectedCustomConfig()
	{
		if (_editingData is CharacterCustomData { customList: not null } characterCustomData && _selectedIndex >= 0 && _selectedIndex < characterCustomData.customList.Count)
		{
			Array<CharacterCustomConfig> array = CopyCustomConfigs(characterCustomData.customList);
			array.RemoveAt(_selectedIndex);
			_selectedIndex = Mathf.Min(_selectedIndex, array.Count - 1);
			_entryBinding.SetValue(characterCustomData, "customList", array, "删除自定义外观", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private void AddDamagePoint(CharacterDamagePointData data)
	{
		if (data != null)
		{
			EnsureDamagePointList(data);
			CharacterDamagePointConfig item = new CharacterDamagePointConfig
			{
				ResourceName = $"DamagePoint{data.damagePointList.Count + 1}",
				damagePointName = $"DamagePoint{data.damagePointList.Count + 1}",
				damagePersontage = 0.5,
				isDrop = true
			};
			Array<CharacterDamagePointConfig> array = CopyDamagePoints(data.damagePointList);
			array.Add(item);
			_selectedIndex = array.Count - 1;
			_entryBinding.SetValue(data, "damagePointList", array, "添加受伤点", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private void RemoveSelectedDamagePoint()
	{
		if (_editingData is CharacterDamagePointData { damagePointList: not null } characterDamagePointData && _selectedIndex >= 0 && _selectedIndex < characterDamagePointData.damagePointList.Count)
		{
			Array<CharacterDamagePointConfig> array = CopyDamagePoints(characterDamagePointData.damagePointList);
			array.RemoveAt(_selectedIndex);
			_selectedIndex = Mathf.Min(_selectedIndex, array.Count - 1);
			_entryBinding.SetValue(characterDamagePointData, "damagePointList", array, "删除受伤点", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private void MoveSelectedAggregateEntry(int direction)
	{
		if (_selectedIndex < 0 || direction == 0)
		{
			return;
		}
		int num = Mathf.Clamp(_selectedIndex + direction, 0, GetEntryCount(_editingData) - 1);
		if (num == _selectedIndex)
		{
			return;
		}
		Resource editingData = _editingData;
		if (!(editingData is CharacterArmorData characterArmorData))
		{
			if (!(editingData is CharacterCustomData characterCustomData))
			{
				if (editingData is CharacterDamagePointData characterDamagePointData)
				{
					Array<CharacterDamagePointConfig> array = CopyDamagePoints(characterDamagePointData.damagePointList);
					CharacterDamagePointConfig item = array[_selectedIndex];
					array.RemoveAt(_selectedIndex);
					array.Insert(num, item);
					_selectedIndex = num;
					_entryBinding.SetValue(characterDamagePointData, "damagePointList", array, "调整受伤点顺序", this, "RefreshCharacterDataEditorFromHistory");
				}
			}
			else
			{
				Array<CharacterCustomConfig> array2 = CopyCustomConfigs(characterCustomData.customList);
				CharacterCustomConfig item2 = array2[_selectedIndex];
				array2.RemoveAt(_selectedIndex);
				array2.Insert(num, item2);
				_selectedIndex = num;
				_entryBinding.SetValue(characterCustomData, "customList", array2, "调整外观顺序", this, "RefreshCharacterDataEditorFromHistory");
			}
		}
		else
		{
			Array<ArmorSlotConfig> array3 = CopyArmorSlots(characterArmorData.armorList);
			ArmorSlotConfig item3 = array3[_selectedIndex];
			array3.RemoveAt(_selectedIndex);
			array3.Insert(num, item3);
			_selectedIndex = num;
			_entryBinding.SetValue(characterArmorData, "armorList", array3, "调整护甲槽顺序", this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private static Array<ArmorSlotConfig> CopyArmorSlots(Array<ArmorSlotConfig> source)
	{
		if (source != null)
		{
			return new Array<ArmorSlotConfig>(source);
		}
		return new Array<ArmorSlotConfig>();
	}

	private static Array<CharacterCustomConfig> CopyCustomConfigs(Array<CharacterCustomConfig> source)
	{
		if (source != null)
		{
			return new Array<CharacterCustomConfig>(source);
		}
		return new Array<CharacterCustomConfig>();
	}

	private static Array<CharacterDamagePointConfig> CopyDamagePoints(Array<CharacterDamagePointConfig> source)
	{
		if (source != null)
		{
			return new Array<CharacterDamagePointConfig>(source);
		}
		return new Array<CharacterDamagePointConfig>();
	}

	private void SetEntryProperty(Resource owner, Resource entry, string propertyName, Variant value)
	{
		if (!_updatingControls && GodotObject.IsInstanceValid(owner) && GodotObject.IsInstanceValid(entry) && !string.IsNullOrWhiteSpace(propertyName))
		{
			_entryBinding.SetValue(entry, propertyName, value, "修改 " + propertyName, this, "RefreshCharacterDataEditorFromHistory");
		}
	}

	private static void NormalizeCharacterData(Resource resource)
	{
		if (!(resource is CharacterArmorData characterArmorData))
		{
			if (!(resource is CharacterCustomData characterCustomData))
			{
				if (resource is CharacterDamagePointData characterDamagePointData)
				{
					EnsureDamagePointList(characterDamagePointData);
					characterDamagePointData.Refresh();
				}
			}
			else
			{
				EnsureCustomList(characterCustomData);
				characterCustomData.Init();
			}
		}
		else
		{
			EnsureArmorList(characterArmorData);
			characterArmorData.Init();
		}
	}

	private static void EnsureArmorList(CharacterArmorData data)
	{
		if (data.armorList == null)
		{
			data.armorList = new Array<ArmorSlotConfig>();
		}
	}

	private static void EnsureCustomList(CharacterCustomData data)
	{
		if (data.customList == null)
		{
			data.customList = new Array<CharacterCustomConfig>();
		}
	}

	private static void EnsureDamagePointList(CharacterDamagePointData data)
	{
		if (data.damagePointList == null)
		{
			data.damagePointList = new Array<CharacterDamagePointConfig>();
		}
	}

	private void ClampSelectedIndex(Resource resource)
	{
		int entryCount = GetEntryCount(resource);
		if (entryCount <= 0)
		{
			_selectedIndex = -1;
			return;
		}
		if (_selectedIndex < 0)
		{
			_selectedIndex = 0;
		}
		_selectedIndex = Mathf.Clamp(_selectedIndex, 0, entryCount - 1);
	}

	private static int GetEntryCount(Resource resource)
	{
		if (!(resource is CharacterArmorData { armorList: var armorList }))
		{
			if (!(resource is CharacterCustomData { customList: var customList }))
			{
				if (resource is CharacterDamagePointData { damagePointList: var damagePointList })
				{
					return damagePointList?.Count ?? 0;
				}
				return 0;
			}
			return customList?.Count ?? 0;
		}
		return armorList?.Count ?? 0;
	}

	private ArmorSlotConfig GetArmorSlot(CharacterArmorData data)
	{
		if (data?.armorList == null || _selectedIndex < 0 || _selectedIndex >= data.armorList.Count)
		{
			return null;
		}
		return data.armorList[_selectedIndex];
	}

	private CharacterCustomConfig GetCustomConfig(CharacterCustomData data)
	{
		if (data?.customList == null || _selectedIndex < 0 || _selectedIndex >= data.customList.Count)
		{
			return null;
		}
		return data.customList[_selectedIndex];
	}

	private CharacterDamagePointConfig GetDamagePointConfig(CharacterDamagePointData data)
	{
		if (data?.damagePointList == null || _selectedIndex < 0 || _selectedIndex >= data.damagePointList.Count)
		{
			return null;
		}
		return data.damagePointList[_selectedIndex];
	}

	private static string BuildEntryTitle(Resource resource, int index)
	{
		if (!(resource is CharacterArmorData characterArmorData))
		{
			if (!(resource is CharacterCustomData characterCustomData))
			{
				if (resource is CharacterDamagePointData characterDamagePointData && index >= 0 && index < characterDamagePointData.damagePointList.Count)
				{
					return $"{index + 1}. {EmptyToPlaceholder(characterDamagePointData.damagePointList[index]?.damagePointName)}  hp={characterDamagePointData.damagePointList[index]?.damagePersontage:0.##}";
				}
			}
			else if (index >= 0 && index < characterCustomData.customList.Count)
			{
				return $"{index + 1}. {EmptyToPlaceholder(characterCustomData.customList[index]?.customName)}  type={EmptyToPlaceholder(characterCustomData.customList[index]?.type)}";
			}
		}
		else if (index >= 0 && index < characterArmorData.armorList.Count)
		{
			return $"{index + 1}. {EmptyToPlaceholder(characterArmorData.armorList[index]?.armorName)}  damage={characterArmorData.armorList[index]?.damagePoint:0.##}";
		}
		return $"{index + 1}";
	}

	private void UpdateVisualPreview()
	{
		if (GodotObject.IsInstanceValid(_previewHealthSlider))
		{
			Resource editingData = _editingData;
			bool flag = ((editingData is CharacterCustomData || editingData is CharacterCustomConfig) ? true : false);
			bool flag2 = flag;
			_previewHealthSlider.Editable = !flag2;
			_previewHealthSlider.TooltipText = (flag2 ? "自定义外观不依赖生命值，选择左侧条目查看槽位效果。" : "拖动生命值，预览护甲或受伤阶段在运行时的触发状态。");
			if (GodotObject.IsInstanceValid(_previewPlayPauseButton))
			{
				_previewPlayPauseButton.Disabled = flag2;
				_previewPlayPauseButton.TooltipText = (flag2 ? "自定义外观不依赖生命值，无需播放受击演示。" : "在真实角色预览中连续播放从满生命到零生命的受击阶段。");
			}
		}
		if (GodotObject.IsInstanceValid(_previewStatusLabel))
		{
			Label previewStatusLabel = _previewStatusLabel;
			Resource editingData = _editingData;
			string text;
			if (editingData is CharacterArmorData data)
			{
				text = BuildArmorPreviewStatus(data);
			}
			else if (editingData is CharacterCustomData data2)
			{
				text = BuildCustomPreviewStatus(data2);
			}
			else if (editingData is CharacterDamagePointData data3)
			{
				text = BuildDamagePreviewStatus(data3);
			}
			else if (editingData is ArmorSlotConfig config)
			{
				text = BuildArmorSlotPreviewStatus(config);
			}
			else if (editingData is CharacterCustomConfig config2)
			{
				text = BuildCustomConfigPreviewStatus(config2);
			}
			else if (editingData is CharacterDamagePointConfig config3)
			{
				text = BuildDamagePointConfigPreviewStatus(config3);
			}
			else
			{
				text = ((!(editingData is TowerDefenseArmorTypeData data4)) ? "" : BuildArmorTypePreviewStatus(data4));
			}
			previewStatusLabel.Text = text;
		}
		ApplyRuntimeCharacterDataPreview();
		_configurationPreviewCanvas?.QueueRedraw();
	}

	private void RebuildRuntimeCharacterPreview(Resource data)
	{
		if (!GodotObject.IsInstanceValid(_previewRoot))
		{
			return;
		}
		RuntimeCharacterPreviewBuildCount++;
		foreach (Node child in _previewRoot.GetChildren())
		{
			child.QueueFree();
		}
		_runtimeCharacterPreview = null;
		_previewOwnerConfig = FindOwningCharacterConfig(data);
		if (!GodotObject.IsInstanceValid(_previewOwnerConfig) || !TryResolveOwningCharacterScene(_previewOwnerConfig, out var scene))
		{
			SetRuntimePreviewMissing(missing: true, "未找到引用该数据的完整角色场景。请把数据放入角色包并在角色配置中引用它。");
			return;
		}
		try
		{
			Node node = scene.Instantiate(PackedScene.GenEditState.Disabled);
			_runtimeCharacterPreview = FindRuntimeCharacter(node);
			if (!GodotObject.IsInstanceValid(_runtimeCharacterPreview))
			{
				node.Free();
				SetRuntimePreviewMissing(missing: true, "角色场景没有 TowerDefenseCharacter 根节点。");
				return;
			}
			_runtimeCharacterPreview.inGame = false;
			_runtimeCharacterPreview.editorPreviewMode = true;
			_runtimeCharacterPreview.config = _previewOwnerConfig;
			_previewRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			SetRuntimePreviewMissing(missing: false, "");
			ApplyRuntimeCharacterDataPreview();
		}
		catch (Exception ex)
		{
			_runtimeCharacterPreview = null;
			SetRuntimePreviewMissing(missing: true, "完整角色场景加载失败：" + ex.Message);
		}
	}

	private void ApplyRuntimeCharacterDataPreview()
	{
		if (GodotObject.IsInstanceValid(_runtimeCharacterPreview))
		{
			RuntimeCharacterPreviewApplyCount++;
			_runtimeCharacterPreview.previewDamagePointPersontage = _previewHealthRatio;
			string previewArmorName = GetPreviewArmorName();
			_runtimeCharacterPreview.currentArmor = (string.IsNullOrWhiteSpace(previewArmorName) ? new Array<string>() : new Array<string> { previewArmorName });
			string previewCustomName = GetPreviewCustomName();
			_runtimeCharacterPreview.currentCustom = (string.IsNullOrWhiteSpace(previewCustomName) ? new Array<string>() : new Array<string> { previewCustomName });
		}
	}

	private void ToggleDamagePreview()
	{
		if (_damagePreviewPlaying)
		{
			SetDamagePreviewPlaying(playing: false, "演示已暂停 · 点击继续");
			return;
		}
		if (_previewHealthRatio <= 0.001)
		{
			ResetDamagePreview();
		}
		_damagePreviewElapsed = (1.0 - _previewHealthRatio) * 6.0;
		SetDamagePreviewPlaying(playing: true, "正在播放真实角色受击阶段");
	}

	private void ResetDamagePreview()
	{
		_damagePreviewElapsed = 0.0;
		_previewHealthRatio = 1.0;
		ApplyPreviewHealthRatio();
		SetDamagePreviewPlaying(playing: false, "已恢复满生命 · 可重新播放");
	}

	private void SetDamagePreviewPlaying(bool playing, string stateText)
	{
		_damagePreviewPlaying = playing && GodotObject.IsInstanceValid(_previewHealthSlider) && _previewHealthSlider.Editable;
		RequestVisibilityGatedProcessing(_damagePreviewPlaying);
		UpdateDamagePreviewControls(stateText);
	}

	private void ApplyPreviewHealthRatio()
	{
		if (GodotObject.IsInstanceValid(_previewHealthSlider))
		{
			_previewHealthSlider.SetValueNoSignal(_previewHealthRatio * 100.0);
		}
		if (GodotObject.IsInstanceValid(_previewHealthValueLabel))
		{
			_previewHealthValueLabel.Text = $"{_previewHealthRatio:P0}";
		}
		UpdateVisualPreview();
	}

	private void UpdateDamagePreviewControls(string stateText)
	{
		if (GodotObject.IsInstanceValid(_previewPlayPauseButton))
		{
			_previewPlayPauseButton.Text = (_damagePreviewPlaying ? "⏸ 暂停演示" : "▶ 播放受击");
		}
		if (GodotObject.IsInstanceValid(_previewPlaybackStateLabel))
		{
			_previewPlaybackStateLabel.Text = stateText ?? "";
			_previewPlaybackStateLabel.Modulate = (_damagePreviewPlaying ? new Color(1f, 0.79f, 0.32f) : new Color(0.64f, 0.78f, 0.71f));
		}
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		base.OnVisualEditorVisibilityChanged(visible);
		ApplyCharacterDataPreviewVisibility(visible);
	}

	private void ApplyCharacterDataPreviewVisibility(bool visible)
	{
		if (GodotObject.IsInstanceValid(_previewViewport))
		{
			_previewViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode)(visible ? 4 : 0);
		}
		if (GodotObject.IsInstanceValid(_previewRoot))
		{
			_previewRoot.ProcessMode = (ProcessModeEnum)(visible ? 0 : 4);
		}
	}

	private string GetPreviewArmorName()
	{
		if (_editingData is ArmorSlotConfig armorSlotConfig)
		{
			return armorSlotConfig.armorName;
		}
		if (_editingData is CharacterArmorData { armorList: not null } characterArmorData && characterArmorData.armorList.Count > 0)
		{
			int index = Mathf.Clamp(_selectedIndex, 0, characterArmorData.armorList.Count - 1);
			return characterArmorData.armorList[index]?.armorName ?? "";
		}
		return "";
	}

	private string GetPreviewCustomName()
	{
		if (_editingData is CharacterCustomConfig characterCustomConfig)
		{
			return characterCustomConfig.customName;
		}
		if (_editingData is CharacterCustomData { customList: not null } characterCustomData && characterCustomData.customList.Count > 0)
		{
			int index = Mathf.Clamp(_selectedIndex, 0, characterCustomData.customList.Count - 1);
			return characterCustomData.customList[index]?.customName ?? "";
		}
		return "";
	}

	private TowerDefenseCharacterConfig FindOwningCharacterConfig(Resource data)
	{
		string text = FindCharacterPackageRoot(CurrentResourcePath, data?.ResourcePath);
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		string text2 = text.PathJoin("Config");
		string[] filesAt = DirAccess.GetFilesAt(text2);
		foreach (string text3 in filesAt)
		{
			if (string.Equals(text3.GetExtension(), "tres", StringComparison.OrdinalIgnoreCase))
			{
				TowerDefenseCharacterConfig towerDefenseCharacterConfig = ResourceLoader.Load<TowerDefenseCharacterConfig>(text2.PathJoin(text3), null, ResourceLoader.CacheMode.Reuse);
				if (GodotObject.IsInstanceValid(towerDefenseCharacterConfig) && CharacterConfigReferencesData(towerDefenseCharacterConfig, data))
				{
					return towerDefenseCharacterConfig;
				}
			}
		}
		return null;
	}

	private static string FindCharacterPackageRoot(params string[] paths)
	{
		for (int i = 0; i < paths.Length; i++)
		{
			string text = NormalizePreviewPath(paths[i]).GetBaseDir();
			for (int j = 0; j < 6; j++)
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					break;
				}
				if (DirAccess.DirExistsAbsolute(text.PathJoin("Config")) && DirAccess.DirExistsAbsolute(text.PathJoin("Scene")))
				{
					return text;
				}
				string baseDir = text.GetBaseDir();
				if (baseDir == text)
				{
					break;
				}
				text = baseDir;
			}
		}
		return "";
	}

	private static bool CharacterConfigReferencesData(TowerDefenseCharacterConfig config, Resource data)
	{
		if (config == null || data == null)
		{
			return false;
		}
		if (SameResource(config.armorData, data) || SameResource(config.customData, data) || SameResource(config.damagePointData, data))
		{
			return true;
		}
		if (data is ArmorSlotConfig right && config.armorData?.armorList != null)
		{
			foreach (ArmorSlotConfig armor in config.armorData.armorList)
			{
				if (SameResource(armor, right))
				{
					return true;
				}
			}
		}
		if (data is CharacterCustomConfig right2 && config.customData?.customList != null)
		{
			foreach (CharacterCustomConfig custom in config.customData.customList)
			{
				if (SameResource(custom, right2))
				{
					return true;
				}
			}
		}
		if (data is CharacterDamagePointConfig right3 && config.damagePointData?.damagePointList != null)
		{
			foreach (CharacterDamagePointConfig damagePoint in config.damagePointData.damagePointList)
			{
				if (SameResource(damagePoint, right3))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool SameResource(Resource left, Resource right)
	{
		if (left == right)
		{
			return true;
		}
		if (GodotObject.IsInstanceValid(left) && GodotObject.IsInstanceValid(right) && !string.IsNullOrWhiteSpace(left.ResourcePath))
		{
			return string.Equals(NormalizePreviewPath(left.ResourcePath), NormalizePreviewPath(right.ResourcePath), StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static bool TryResolveOwningCharacterScene(TowerDefenseCharacterConfig config, out PackedScene scene)
	{
		scene = null;
		if (GodotObject.IsInstanceValid(config) && !string.IsNullOrWhiteSpace(config.name) && ResourceManager.Instance != null)
		{
			scene = ResourceManager.Instance.GetCharacterScene(config.name);
			if (GodotObject.IsInstanceValid(scene))
			{
				return true;
			}
		}
		string text = FindCharacterPackageRoot(config?.ResourcePath).PathJoin("Scene");
		string[] filesAt = DirAccess.GetFilesAt(text);
		foreach (string text2 in filesAt)
		{
			if (string.Equals(text2.GetExtension(), "tscn", StringComparison.OrdinalIgnoreCase))
			{
				scene = ResourceLoader.Load<PackedScene>(text.PathJoin(text2), null, ResourceLoader.CacheMode.Reuse);
				if (GodotObject.IsInstanceValid(scene))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static TowerDefenseCharacter FindRuntimeCharacter(Node node)
	{
		if (node is TowerDefenseCharacter result)
		{
			return result;
		}
		if (!GodotObject.IsInstanceValid(node))
		{
			return null;
		}
		foreach (Node child in node.GetChildren())
		{
			TowerDefenseCharacter towerDefenseCharacter = FindRuntimeCharacter(child);
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				return towerDefenseCharacter;
			}
		}
		return null;
	}

	private void SetRuntimePreviewMissing(bool missing, string message)
	{
		if (GodotObject.IsInstanceValid(_configurationPreviewCanvas))
		{
			_configurationPreviewCanvas.Visible = missing;
			_configurationPreviewCanvas.QueueRedraw();
		}
		if (GodotObject.IsInstanceValid(_fallbackRibbon))
		{
			_fallbackRibbon.Visible = missing;
		}
		if (GodotObject.IsInstanceValid(_fallbackRibbonLabel))
		{
			_fallbackRibbonLabel.Text = (missing ? message : "");
		}
		if (GodotObject.IsInstanceValid(_previewMissingLabel))
		{
			_previewMissingLabel.Visible = missing && !GodotObject.IsInstanceValid(_configurationPreviewCanvas);
			_previewMissingLabel.Text = message;
		}
	}

	private void OnRuntimePreviewGuiInput(InputEvent inputEvent)
	{
		InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
		bool flag = inputEventMouseButton?.Pressed ?? false;
		if (flag)
		{
			MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
			bool flag2 = (((ulong)(buttonIndex - 4) <= 1uL) ? true : false);
			flag = flag2;
		}
		if (flag)
		{
			_previewZoom = Mathf.Clamp(_previewZoom * ((inputEventMouseButton.ButtonIndex == MouseButton.WheelUp) ? 1.1f : 0.9f), 0.35f, 3f);
			UpdateRuntimePreviewTransform();
			_previewViewportContainer.AcceptEvent();
		}
		else if (inputEvent is InputEventMouseButton inputEventMouseButton2 && inputEventMouseButton2.ButtonIndex == MouseButton.Left)
		{
			_draggingPreview = inputEventMouseButton2.Pressed;
			_previewViewportContainer.AcceptEvent();
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _draggingPreview)
		{
			_previewPan += inputEventMouseMotion.Relative;
			UpdateRuntimePreviewTransform();
			_previewViewportContainer.AcceptEvent();
		}
	}

	private void UpdateRuntimePreviewTransform()
	{
		if (GodotObject.IsInstanceValid(_previewRoot))
		{
			_previewRoot.Position = new Vector2(450f, 228f) + _previewPan;
			_previewRoot.Scale = Vector2.One * _previewZoom;
		}
	}

	private static string NormalizePreviewPath(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return path.Replace('\\', '/').Trim();
		}
		return "";
	}

	private void DrawCharacterDataPreview(Control canvas)
	{
		if (!GodotObject.IsInstanceValid(canvas))
		{
			return;
		}
		canvas.DrawRect(new Rect2(Vector2.Zero, canvas.Size), new Color(0.042f, 0.048f, 0.058f));
		Resource editingData = _editingData;
		if (!(editingData is CharacterArmorData data))
		{
			if (!(editingData is CharacterCustomData data2))
			{
				if (!(editingData is CharacterDamagePointData data3))
				{
					if (!(editingData is ArmorSlotConfig config))
					{
						if (!(editingData is CharacterCustomConfig config2))
						{
							if (!(editingData is CharacterDamagePointConfig config3))
							{
								if (editingData is TowerDefenseArmorTypeData data4)
								{
									DrawArmorTypePreview(canvas, data4);
								}
							}
							else
							{
								DrawDamagePointConfigPreview(canvas, config3);
							}
						}
						else
						{
							DrawCustomConfigPreview(canvas, config2);
						}
					}
					else
					{
						DrawArmorSlotConfigPreview(canvas, config);
					}
				}
				else
				{
					DrawDamagePointPreview(canvas, data3);
				}
			}
			else
			{
				DrawCustomPreview(canvas, data2);
			}
		}
		else
		{
			DrawArmorPreview(canvas, data);
		}
	}

	private void DrawDamagePointConfigPreview(Control canvas, CharacterDamagePointConfig config)
	{
		float num = Mathf.Max(180f, canvas.Size.X - 72f);
		DrawHealthBar(canvas, "受伤点", 36f, 18f, num);
		float num2 = Mathf.Clamp((float)config.damagePersontage, 0f, 1f);
		float num3 = 124f + Mathf.Max(30f, num - 88f) * num2;
		bool flag = _previewHealthRatio <= (double)num2;
		canvas.DrawLine(new Vector2(num3, 43f), new Vector2(num3, 86f), new Color(1f, 0.62f, 0.34f), 3f);
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(num3 - 30f, 101f), $"阈值 {num2:P0}", HorizontalAlignment.Center, 60f, 12, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		Rect2 rect = new Rect2(36f, 112f, num, 50f);
		canvas.DrawRect(rect, flag ? new Color(0.38f, 0.16f, 0.13f) : new Color(0.13f, 0.21f, 0.26f));
		canvas.DrawRect(rect, flag ? new Color(1f, 0.58f, 0.38f) : new Color(0.36f, 0.58f, 0.7f), filled: false, 2f);
		Texture2D texture2D = LoadTexturePreview(config.replaceMediaTexturePath);
		if (GodotObject.IsInstanceValid(texture2D))
		{
			canvas.DrawTextureRect(texture2D, new Rect2(43f, 118f, 38f, 38f), tile: false);
		}
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(89f, 134f), EmptyToPlaceholder(config.damagePointName), HorizontalAlignment.Left, num - 62f, 13, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(89f, 154f), flag ? "运行状态：已触发" : "运行状态：等待触发", HorizontalAlignment.Left, num - 62f, 11, flag ? new Color(1f, 0.69f, 0.52f) : new Color(0.67f, 0.82f, 0.91f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
	}

	private void DrawArmorSlotConfigPreview(Control canvas, ArmorSlotConfig config)
	{
		DrawHealthBar(canvas, "护甲槽", 24f, 14f, canvas.Size.X - 48f);
		bool flag = config.damagePoint < 0.0 || _previewHealthRatio > config.damagePoint;
		Vector2 vector = new Vector2(canvas.Size.X * 0.33f, 116f);
		canvas.DrawCircle(vector - new Vector2(0f, 28f), 22f, new Color(0.35f, 0.39f, 0.45f));
		canvas.DrawRect(new Rect2(vector - new Vector2(26f, 6f), new Vector2(52f, 58f)), new Color(0.27f, 0.31f, 0.37f));
		Vector2 position = vector + config.offset * 0.25f;
		canvas.DrawSetTransform(position, (float)config.rotation, config.scale);
		canvas.DrawRect(new Rect2(new Vector2(-34f, -28f), new Vector2(68f, 56f)), flag ? new Color(0.28f, 0.58f, 0.76f) : new Color(0.3f, 0.19f, 0.18f));
		canvas.DrawRect(new Rect2(new Vector2(-34f, -28f), new Vector2(68f, 56f)), flag ? new Color(0.65f, 0.88f, 1f) : new Color(0.72f, 0.43f, 0.38f), filled: false, 2f);
		canvas.DrawSetTransform(Vector2.Zero, 0f, Vector2.One);
		float x = canvas.Size.X * 0.55f;
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(x, 82f), EmptyToPlaceholder(config.armorName), HorizontalAlignment.Left, -1f, 16, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(x, 108f), "替换方式: " + EmptyToPlaceholder(config.replaceMethod), HorizontalAlignment.Left, -1f, 12, new Color(0.72f, 0.83f, 0.92f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(x, 132f), $"槽位: {config.slotPath}", HorizontalAlignment.Left, -1f, 12, new Color(0.72f, 0.83f, 0.92f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(x, 156f), flag ? "护甲显示" : "已达到移除阈值", HorizontalAlignment.Left, -1f, 12, flag ? new Color(0.55f, 0.92f, 0.66f) : new Color(1f, 0.62f, 0.52f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
	}

	private void DrawCustomConfigPreview(Control canvas, CharacterCustomConfig config)
	{
		bool flag = string.Equals(config.type, "Gold", StringComparison.OrdinalIgnoreCase);
		Rect2 rect = new Rect2(26f, 24f, canvas.Size.X - 52f, canvas.Size.Y - 42f);
		canvas.DrawRect(rect, flag ? new Color(0.34f, 0.27f, 0.08f) : new Color(0.22f, 0.24f, 0.29f));
		canvas.DrawRect(rect, flag ? new Color(0.96f, 0.76f, 0.24f) : new Color(0.73f, 0.78f, 0.84f), filled: false, 2f);
		Texture2D texture2D = ((config.damagePointChangeMediaTexturePaths != null && config.damagePointChangeMediaTexturePaths.Count > 0) ? LoadTexturePreview(config.damagePointChangeMediaTexturePaths[0]) : null);
		if (GodotObject.IsInstanceValid(texture2D))
		{
			canvas.DrawTextureRect(texture2D, new Rect2(rect.End.X - 122f, rect.Position.Y + 12f, 104f, 104f), tile: false);
		}
		canvas.DrawString(GetThemeDefaultFont(), rect.Position + new Vector2(16f, 32f), EmptyToPlaceholder(config.customName), HorizontalAlignment.Left, rect.Size.X - 150f, 18, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		canvas.DrawString(GetThemeDefaultFont(), rect.Position + new Vector2(16f, 60f), "品质: " + EmptyToPlaceholder(config.type), HorizontalAlignment.Left, rect.Size.X - 150f, 13, flag ? new Color(1f, 0.82f, 0.35f) : new Color(0.8f, 0.84f, 0.9f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		canvas.DrawString(GetThemeDefaultFont(), rect.Position + new Vector2(16f, 86f), "解锁键: " + EmptyToPlaceholder(config.openKey), HorizontalAlignment.Left, rect.Size.X - 150f, 12, new Color(0.72f, 0.82f, 0.91f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		canvas.DrawString(GetThemeDefaultFont(), rect.Position + new Vector2(16f, 112f), $"伤害阶段贴图: {config.damagePointChangeMediaTexturePaths?.Count ?? 0}", HorizontalAlignment.Left, rect.Size.X - 150f, 12, new Color(0.72f, 0.82f, 0.91f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
	}

	private void DrawArmorTypePreview(Control canvas, TowerDefenseArmorTypeData data)
	{
		float num = Mathf.Max(200f, canvas.Size.X - 60f);
		DrawHealthBar(canvas, "护甲耐久", 30f, 14f, num);
		int num2 = data.stagePersontage?.Count ?? 0;
		for (int i = 0; i < num2; i++)
		{
			float num3 = Mathf.Clamp((float)data.stagePersontage[i], 0f, 1f);
			float num4 = 118f + Mathf.Max(30f, num - 88f) * num3;
			canvas.DrawLine(new Vector2(num4, 42f), new Vector2(num4, 68f), new Color(0.98f, 0.67f, 0.3f), 2f);
			canvas.DrawString(GetThemeDefaultFont(), new Vector2(num4 - 22f, 82f), num3.ToString("P0"), HorizontalAlignment.Center, 44f, 10, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}
		int num5 = data.stageAnimeTexturePaths?.Count ?? 0;
		float num6 = Mathf.Clamp((num - 8f * (float)Mathf.Max(0, num5 - 1)) / (float)Mathf.Max(1, num5), 82f, 150f);
		for (int j = 0; j < num5; j++)
		{
			float num7 = 30f + (float)j * (num6 + 8f);
			Rect2 rect = new Rect2(num7, 94f, num6, 68f);
			canvas.DrawRect(rect, new Color(0.13f, 0.19f, 0.25f));
			canvas.DrawRect(rect, new Color(0.36f, 0.52f, 0.64f), filled: false, 1f);
			Texture2D texture2D = LoadTexturePreview(data.stageAnimeTexturePaths[j]);
			if (GodotObject.IsInstanceValid(texture2D))
			{
				canvas.DrawTextureRect(texture2D, new Rect2(num7 + 5f, 99f, 48f, 48f), tile: false);
			}
			canvas.DrawString(GetThemeDefaultFont(), new Vector2(num7 + 58f, 124f), $"阶段 {j + 1}", HorizontalAlignment.Left, num6 - 62f, 12, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}
	}

	private void DrawArmorPreview(Control canvas, CharacterArmorData data)
	{
		DrawHealthBar(canvas, "角色生命", 18f, 14f, canvas.Size.X - 36f);
		Vector2 vector = new Vector2(76f, 105f);
		canvas.DrawCircle(vector - new Vector2(0f, 31f), 22f, new Color(0.34f, 0.39f, 0.45f));
		canvas.DrawRect(new Rect2(vector - new Vector2(25f, 10f), new Vector2(50f, 65f)), new Color(0.27f, 0.31f, 0.37f));
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(36f, 168f), "角色挂点", HorizontalAlignment.Center, 80f, 12, new Color(0.72f, 0.76f, 0.82f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		int valueOrDefault = (data?.armorList?.Count).GetValueOrDefault();
		if (valueOrDefault == 0)
		{
			canvas.DrawString(GetThemeDefaultFont(), new Vector2(150f, 105f), "暂无护甲槽", HorizontalAlignment.Left, -1f, 14, new Color(0.72f, 0.74f, 0.78f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			return;
		}
		float num = 138f;
		float num2 = 8f;
		float num3 = Mathf.Clamp((Mathf.Max(120f, canvas.Size.X - num - 16f) - num2 * (float)(valueOrDefault - 1)) / (float)valueOrDefault, 88f, 170f);
		for (int i = 0; i < valueOrDefault; i++)
		{
			ArmorSlotConfig armorSlotConfig = data.armorList[i];
			if (armorSlotConfig != null)
			{
				float num4 = num + (float)i * (num3 + num2);
				Rect2 rect = new Rect2(num4, 54f, num3, 106f);
				bool flag = i == _selectedIndex;
				bool flag2 = armorSlotConfig.damagePoint >= 0.0 && _previewHealthRatio <= armorSlotConfig.damagePoint;
				Color color = (flag2 ? new Color(0.31f, 0.16f, 0.15f) : new Color(0.12f, 0.22f, 0.29f));
				canvas.DrawRect(rect, color);
				canvas.DrawRect(rect, flag ? new Color(0.49f, 0.79f, 1f) : new Color(0.3f, 0.39f, 0.47f), filled: false, flag ? 3f : 1f);
				Texture2D texture2D = null;
				TowerDefenseArmorTypeData typeData = data.GetTypeData(armorSlotConfig.armorName);
				if (typeData?.stageAnimeTexturePaths != null && typeData.stageAnimeTexturePaths.Count > 0)
				{
					texture2D = LoadTexturePreview(typeData.stageAnimeTexturePaths[0]);
				}
				if (GodotObject.IsInstanceValid(texture2D))
				{
					canvas.DrawTextureRect(texture2D, new Rect2(num4 + 8f, 62f, Mathf.Min(42f, num3 - 16f), 42f), tile: false);
				}
				else
				{
					canvas.DrawCircle(new Vector2(num4 + 29f, 83f), 17f, new Color(0.39f, 0.48f, 0.57f));
				}
				canvas.DrawString(GetThemeDefaultFont(), new Vector2(num4 + 8f, 119f), $"{i + 1}. {EmptyToPlaceholder(armorSlotConfig.armorName)}", HorizontalAlignment.Left, num3 - 16f, 12, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				string text;
				if (armorSlotConfig.damagePoint < 0.0)
				{
					text = "常驻";
				}
				else
				{
					text = (flag2 ? "已达阈值" : $"阈值 {armorSlotConfig.damagePoint:P0}");
				}
				canvas.DrawString(GetThemeDefaultFont(), new Vector2(num4 + 8f, 143f), text, HorizontalAlignment.Left, num3 - 16f, 12, flag2 ? new Color(1f, 0.61f, 0.52f) : new Color(0.68f, 0.83f, 0.94f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			}
		}
	}

	private void DrawDamagePointPreview(Control canvas, CharacterDamagePointData data)
	{
		float num = Mathf.Max(120f, canvas.Size.X - 84f);
		DrawHealthBar(canvas, "生命阶段", 42f, 18f, num);
		int valueOrDefault = (data?.damagePointList?.Count).GetValueOrDefault();
		if (valueOrDefault == 0)
		{
			canvas.DrawString(GetThemeDefaultFont(), new Vector2(42f, 108f), "暂无受伤点", HorizontalAlignment.Left, -1f, 14, new Color(0.72f, 0.74f, 0.78f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			return;
		}
		float num2 = Mathf.Clamp((num - 8f * (float)(valueOrDefault - 1)) / (float)valueOrDefault, 92f, 180f);
		for (int i = 0; i < valueOrDefault; i++)
		{
			CharacterDamagePointConfig characterDamagePointConfig = data.damagePointList[i];
			if (characterDamagePointConfig != null)
			{
				float num3 = Mathf.Clamp((float)characterDamagePointConfig.damagePersontage, 0f, 1f);
				float num4 = 42f + num * num3;
				bool flag = _previewHealthRatio <= (double)num3;
				bool flag2 = i == _selectedIndex;
				canvas.DrawLine(new Vector2(num4, 48f), new Vector2(num4, 72f), flag2 ? new Color(0.54f, 0.83f, 1f) : new Color(0.84f, 0.57f, 0.35f), flag2 ? 3f : 2f);
				canvas.DrawString(GetThemeDefaultFont(), new Vector2(num4 - 24f, 86f), $"{num3:P0}", HorizontalAlignment.Center, 48f, 11, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				float num5 = 42f + (float)i * (num2 + 8f);
				Rect2 rect = new Rect2(num5, 98f, num2, 65f);
				canvas.DrawRect(rect, flag ? new Color(0.38f, 0.17f, 0.13f) : new Color(0.13f, 0.19f, 0.24f));
				canvas.DrawRect(rect, flag2 ? new Color(0.52f, 0.82f, 1f) : new Color(0.34f, 0.38f, 0.43f), filled: false, flag2 ? 3f : 1f);
				Texture2D texture2D = LoadTexturePreview(characterDamagePointConfig.replaceMediaTexturePath);
				if (GodotObject.IsInstanceValid(texture2D))
				{
					canvas.DrawTextureRect(texture2D, new Rect2(num5 + 6f, 104f, 38f, 38f), tile: false);
				}
				canvas.DrawString(GetThemeDefaultFont(), new Vector2(num5 + 8f, 124f), $"{i + 1}. {EmptyToPlaceholder(characterDamagePointConfig.damagePointName)}", HorizontalAlignment.Left, num2 - 16f, 12, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				canvas.DrawString(GetThemeDefaultFont(), new Vector2(num5 + 8f, 148f), flag ? "已触发" : "未触发", HorizontalAlignment.Left, num2 - 16f, 11, flag ? new Color(1f, 0.65f, 0.48f) : new Color(0.63f, 0.8f, 0.91f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			}
		}
	}

	private void DrawCustomPreview(Control canvas, CharacterCustomData data)
	{
		int valueOrDefault = (data?.customList?.Count).GetValueOrDefault();
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(18f, 28f), "外观与解锁槽位", HorizontalAlignment.Left, -1f, 14, new Color(0.82f, 0.86f, 0.92f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		if (valueOrDefault == 0)
		{
			canvas.DrawString(GetThemeDefaultFont(), new Vector2(18f, 92f), "暂无自定义外观", HorizontalAlignment.Left, -1f, 14, new Color(0.72f, 0.74f, 0.78f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			return;
		}
		float num = 10f;
		float num2 = Mathf.Clamp((Mathf.Max(120f, canvas.Size.X - 36f) - num * (float)(valueOrDefault - 1)) / (float)valueOrDefault, 110f, 210f);
		for (int i = 0; i < valueOrDefault; i++)
		{
			CharacterCustomConfig characterCustomConfig = data.customList[i];
			if (characterCustomConfig != null)
			{
				float num3 = 18f + (float)i * (num2 + num);
				Rect2 rect = new Rect2(num3, 42f, num2, 116f);
				bool flag = i == _selectedIndex;
				bool flag2 = string.Equals(characterCustomConfig.type, "Gold", StringComparison.OrdinalIgnoreCase);
				Color color = (flag2 ? new Color(0.34f, 0.27f, 0.08f) : new Color(0.23f, 0.25f, 0.29f));
				Color color2;
				if (flag)
				{
					color2 = new Color(0.51f, 0.84f, 1f);
				}
				else
				{
					color2 = (flag2 ? new Color(0.95f, 0.75f, 0.24f) : new Color(0.72f, 0.76f, 0.82f));
				}
				canvas.DrawRect(rect, color);
				canvas.DrawRect(rect, color2, filled: false, flag ? 3f : 1f);
				Texture2D texture2D = ((characterCustomConfig.damagePointChangeMediaTexturePaths != null && characterCustomConfig.damagePointChangeMediaTexturePaths.Count > 0) ? LoadTexturePreview(characterCustomConfig.damagePointChangeMediaTexturePaths[0]) : null);
				if (GodotObject.IsInstanceValid(texture2D))
				{
					canvas.DrawTextureRect(texture2D, new Rect2(num3 + num2 - 50f, 50f, 42f, 42f), tile: false);
				}
				canvas.DrawString(GetThemeDefaultFont(), new Vector2(num3 + 9f, 67f), $"{i + 1}. {EmptyToPlaceholder(characterCustomConfig.customName)}", HorizontalAlignment.Left, num2 - 62f, 13, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				canvas.DrawString(GetThemeDefaultFont(), new Vector2(num3 + 9f, 91f), "类型: " + EmptyToPlaceholder(characterCustomConfig.type), HorizontalAlignment.Left, num2 - 18f, 11, color2, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				canvas.DrawString(GetThemeDefaultFont(), new Vector2(num3 + 9f, 113f), "解锁键: " + EmptyToPlaceholder(characterCustomConfig.openKey), HorizontalAlignment.Left, num2 - 18f, 11, new Color(0.76f, 0.8f, 0.86f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				canvas.DrawString(GetThemeDefaultFont(), new Vector2(num3 + 9f, 137f), $"伤害贴图: {characterCustomConfig.damagePointChangeMediaTexturePaths?.Count ?? 0}", HorizontalAlignment.Left, num2 - 18f, 11, new Color(0.64f, 0.82f, 0.91f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			}
		}
	}

	private void DrawHealthBar(Control canvas, string title, float x, float y, float width)
	{
		float num = Mathf.Max(80f, width);
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(x, y + 14f), title, HorizontalAlignment.Left, 88f, 12, new Color(0.8f, 0.84f, 0.9f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		Rect2 rect = new Rect2(x + 88f, y + 2f, Mathf.Max(30f, num - 88f), 15f);
		canvas.DrawRect(rect, new Color(0.16f, 0.1f, 0.1f));
		canvas.DrawRect(new Rect2(rect.Position, new Vector2(rect.Size.X * (float)_previewHealthRatio, rect.Size.Y)), new Color(0.28f, 0.72f, 0.35f));
		canvas.DrawRect(rect, new Color(0.58f, 0.64f, 0.7f), filled: false, 1f);
		canvas.DrawString(GetThemeDefaultFont(), new Vector2(rect.End.X - 50f, y + 15f), $"{_previewHealthRatio:P0}", HorizontalAlignment.Right, 46f, 11, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
	}

	private string BuildArmorPreviewStatus(CharacterArmorData data)
	{
		int num = 0;
		if (data?.armorList != null)
		{
			foreach (ArmorSlotConfig armor in data.armorList)
			{
				if (armor != null && armor.damagePoint >= 0.0 && _previewHealthRatio <= armor.damagePoint)
				{
					num++;
				}
			}
		}
		return $"生命 {_previewHealthRatio:P0} · 达到 {num} 个护甲阈值";
	}

	private string BuildDamagePreviewStatus(CharacterDamagePointData data)
	{
		int num = 0;
		if (data?.damagePointList != null)
		{
			foreach (CharacterDamagePointConfig damagePoint in data.damagePointList)
			{
				if (damagePoint != null && _previewHealthRatio <= damagePoint.damagePersontage)
				{
					num++;
				}
			}
		}
		return $"生命 {_previewHealthRatio:P0} · 已触发 {num} 个阶段";
	}

	private string BuildCustomPreviewStatus(CharacterCustomData data)
	{
		CharacterCustomConfig customConfig = GetCustomConfig(data);
		if (customConfig != null)
		{
			return "当前: " + EmptyToPlaceholder(customConfig.customName) + " · " + EmptyToPlaceholder(customConfig.type);
		}
		return $"共 {(data?.customList?.Count).GetValueOrDefault()} 个外观槽";
	}

	private string BuildDamagePointConfigPreviewStatus(CharacterDamagePointConfig config)
	{
		bool flag = _previewHealthRatio <= config.damagePersontage;
		return $"生命 {_previewHealthRatio:P0} · {(flag ? "已触发" : "未触发")} · 音效 {EmptyToPlaceholder(config.damageAudio)}";
	}

	private string BuildArmorSlotPreviewStatus(ArmorSlotConfig config)
	{
		bool flag = config.damagePoint < 0.0 || _previewHealthRatio > config.damagePoint;
		return $"生命 {_previewHealthRatio:P0} · {(flag ? "护甲显示" : "护甲移除")} · {EmptyToPlaceholder(config.replaceMethod)}";
	}

	private static string BuildCustomConfigPreviewStatus(CharacterCustomConfig config)
	{
		return $"{EmptyToPlaceholder(config.customName)} · {EmptyToPlaceholder(config.type)} · {config.damagePointChangeMediaTexturePaths?.Count ?? 0} 张阶段贴图";
	}

	private string BuildArmorTypePreviewStatus(TowerDefenseArmorTypeData data)
	{
		int num = 0;
		if (data.stagePersontage != null)
		{
			foreach (double item in data.stagePersontage)
			{
				if (_previewHealthRatio <= item)
				{
					num++;
				}
			}
		}
		return $"耐久 {data.damagePoint:0.##} · 生命 {_previewHealthRatio:P0} · 阶段 {num}/{data.stagePersontage?.Count ?? 0}";
	}

	private static bool IsAggregateCharacterData(Resource resource)
	{
		if (resource is CharacterArmorData || resource is CharacterCustomData || resource is CharacterDamagePointData)
		{
			return true;
		}
		return false;
	}

	private static bool IsSupportedCharacterDataResource(Resource resource)
	{
		bool flag = IsAggregateCharacterData(resource);
		if (!flag)
		{
			bool flag2 = ((resource is ArmorSlotConfig || resource is CharacterCustomConfig || resource is CharacterDamagePointConfig || resource is TowerDefenseArmorTypeData) ? true : false);
			flag = flag2;
		}
		return flag;
	}

	private void UpdateSummary()
	{
		if (GodotObject.IsInstanceValid(_summaryLabel))
		{
			_summaryLabel.Text = BuildDataSummary(_editingData);
		}
		if (GodotObject.IsInstanceValid(_cacheSummaryLabel))
		{
			_cacheSummaryLabel.Text = BuildCacheSummary(_editingData);
		}
		UpdateVisualPreview();
	}

	private static string BuildDataSummary(Resource resource)
	{
		if (!(resource is CharacterArmorData characterArmorData))
		{
			if (!(resource is CharacterCustomData characterCustomData))
			{
				if (resource is CharacterDamagePointData characterDamagePointData)
				{
					return $"受伤点数据: {characterDamagePointData.damagePointList?.Count ?? 0} 个受伤阶段";
				}
				return "角色数据";
			}
			return $"自定义外观数据: {characterCustomData.customList?.Count ?? 0} 个外观";
		}
		return $"护甲数据: {characterArmorData.armorList?.Count ?? 0} 个护甲槽";
	}

	private static string BuildCacheSummary(Resource resource)
	{
		if (!(resource is CharacterArmorData characterArmorData))
		{
			if (!(resource is CharacterCustomData characterCustomData))
			{
				if (resource is CharacterDamagePointData characterDamagePointData)
				{
					return $"派生缓存: damagePointDictionary={characterDamagePointData.damagePointDictionary?.Count ?? 0}, open={characterDamagePointData.fliterOpenAll?.Count ?? 0}, close={characterDamagePointData.fliterCloseAll?.Count ?? 0}";
				}
				return "";
			}
			return $"派生缓存: customDictionary={characterCustomData.customDictionary?.Count ?? 0}, open={characterCustomData.fliterOpenAll?.Count ?? 0}, close={characterCustomData.fliterCloseAll?.Count ?? 0}";
		}
		return $"派生缓存: armorDictionary={characterArmorData.armorDictionary?.Count ?? 0}, all/open/close filter={characterArmorData.fliterAllDictionary?.Count ?? 0}/{characterArmorData.fliterOpenDictionary?.Count ?? 0}/{characterArmorData.fliterCloseDictionary?.Count ?? 0}";
	}

	private Control AddBoundLineEdit(string nodeName, string label, Resource resource, StringName property, Func<string, Variant> convert = null)
	{
		LineEdit edit = new LineEdit
		{
			Name = nodeName,
			Text = resource.Get(property).AsString(),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		bool submittedSinceLastPreview = false;
		edit.FocusEntered += () =>
		{
			submittedSinceLastPreview = false;
			_entryBinding.BeginEdit(resource, property);
		};
		edit.TextChanged += (string text) =>
		{
			submittedSinceLastPreview = false;
			_entryBinding.PreviewValue(resource, property, ReadValue(text));
			LightweightCharacterDataPreview();
		};
		edit.TextSubmitted += (string text) =>
		{
			_entryBinding.CommitEdit(resource, property, ReadValue(text), $"修改 {property}", this, "RefreshCharacterDataEditorFromHistory");
			submittedSinceLastPreview = true;
		};
		edit.FocusExited += () =>
		{
			if (submittedSinceLastPreview)
			{
				submittedSinceLastPreview = false;
			}
			else
			{
				_entryBinding.CommitEdit(resource, property, ReadValue(edit.Text), $"修改 {property}", this, "RefreshCharacterDataEditorFromHistory");
			}
		};
		return WrapField(label, edit);
		Variant ReadValue(string text)
		{
			Func<string, Variant> func = convert;
			if (func == null)
			{
				string from = text ?? "";
				return Variant.From(in from);
			}
			return func(text);
		}
	}

	private Control AddBoundTextEdit(string nodeName, string label, Resource resource, StringName property)
	{
		TextEdit edit = new TextEdit
		{
			Name = nodeName,
			Text = resource.Get(property).AsString(),
			CustomMinimumSize = new Vector2(0f, 78f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			WrapMode = TextEdit.LineWrappingMode.Boundary
		};
		edit.FocusEntered += () =>
		{
			_entryBinding.BeginEdit(resource, property);
		};
		edit.TextChanged += () =>
		{
			_entryBinding.PreviewValue(resource, property, edit.Text);
			LightweightCharacterDataPreview();
		};
		edit.FocusExited += () =>
		{
			_entryBinding.CommitEdit(resource, property, edit.Text, $"修改 {property}", this, "RefreshCharacterDataEditorFromHistory");
		};
		return WrapField(label, edit);
	}

	private Control AddBoundSpinBox(string nodeName, string label, Resource resource, StringName property, double min, double max, double step)
	{
		SpinBox spinBox = new SpinBox
		{
			Name = nodeName,
			MinValue = min,
			MaxValue = max,
			Step = step,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			CustomMinimumSize = new Vector2(120f, 0f)
		};
		spinBox.GetLineEdit().Alignment = HorizontalAlignment.Right;
		_entryBinding.BindNumber(spinBox, resource, property, LightweightCharacterDataPreview, this, "RefreshCharacterDataEditorFromHistory");
		return WrapField(label, spinBox);
	}

	private Control AddBoundVector2Field(string nodeName, string label, Resource resource, StringName property)
	{
		Vector2 vector = resource.Get(property).AsVector2();
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			Name = nodeName,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		SpinBox x = CreateVectorSpinBox("X", vector.X);
		SpinBox y = CreateVectorSpinBox("Y", vector.Y);
		Action value = () =>
		{
			_entryBinding.BeginEdit(resource, property);
		};
		Action preview = () =>
		{
			_entryBinding.PreviewValue(resource, property, ReadValue());
			LightweightCharacterDataPreview();
		};
		Action value2 = () =>
		{
			_entryBinding.CommitEdit(resource, property, ReadValue(), $"修改 {property}", this, "RefreshCharacterDataEditorFromHistory");
		};
		x.FocusEntered += value;
		y.FocusEntered += value;
		x.ValueChanged += (double _) =>
		{
			preview();
		};
		y.ValueChanged += (double _) =>
		{
			preview();
		};
		x.FocusExited += value2;
		y.FocusExited += value2;
		hBoxContainer.AddChild(x, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(y, forceReadableName: false, InternalMode.Disabled);
		return WrapField(label, hBoxContainer);
		Variant ReadValue()
		{
			return Variant.From<Vector2>(new Vector2((float)x.Value, (float)y.Value));
		}
	}

	public void RefreshCharacterDataEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null || (!xWUndoRedoManager.IsUndoing() && !xWUndoRedoManager.IsRedoing()) || CanvasGrid == null)
		{
			return;
		}
		_metadataBinding?.Dispose();
		_entryBinding?.Dispose();
		NormalizeCharacterData(_editingData);
		ClampSelectedIndex(_editingData);
		foreach (Node child in CanvasGrid.GetChildren())
		{
			CanvasGrid.RemoveChild(child);
			child.QueueFree();
		}
		RenderCustomVisualPreset(null);
	}

	private static Control AddLineEdit(string nodeName, string label, string value, Action<string> changed)
	{
		LineEdit lineEdit = new LineEdit
		{
			Name = nodeName,
			Text = (value ?? ""),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		lineEdit.TextChanged += (string text) =>
		{
			changed?.Invoke(text);
		};
		return WrapField(label, lineEdit);
	}

	private static Control AddTextEdit(string nodeName, string label, string value, Action<string> changed)
	{
		TextEdit edit = new TextEdit
		{
			Name = nodeName,
			Text = (value ?? ""),
			CustomMinimumSize = new Vector2(0f, 78f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			WrapMode = TextEdit.LineWrappingMode.Boundary
		};
		edit.TextChanged += () =>
		{
			changed?.Invoke(edit.Text);
		};
		return WrapField(label, edit);
	}

	private static Control AddSpinBox(string nodeName, string label, double value, double min, double max, double step, Action<double> changed)
	{
		SpinBox spinBox = new SpinBox
		{
			Name = nodeName,
			MinValue = min,
			MaxValue = max,
			Step = step,
			Value = value,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			CustomMinimumSize = new Vector2(120f, 0f)
		};
		spinBox.GetLineEdit().Alignment = HorizontalAlignment.Right;
		spinBox.ValueChanged += (double newValue) =>
		{
			changed?.Invoke(newValue);
		};
		return WrapField(label, spinBox);
	}

	private static Control AddBoolField(string nodeName, string label, bool value, Action<bool> changed)
	{
		CheckBox checkBox = new CheckBox
		{
			Name = nodeName,
			Text = "启用",
			ButtonPressed = value,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		checkBox.Toggled += (bool newValue) =>
		{
			changed?.Invoke(newValue);
		};
		return WrapField(label, checkBox);
	}

	private static Control AddVector2Field(string nodeName, string label, Vector2 value, Action<Vector2> changed)
	{
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			Name = nodeName,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hBoxContainer.AddThemeConstantOverride("separation", 4);
		SpinBox x = CreateVectorSpinBox("X", value.X);
		SpinBox y = CreateVectorSpinBox("Y", value.Y);
		x.ValueChanged += (double _) =>
		{
			changed?.Invoke(new Vector2((float)x.Value, (float)y.Value));
		};
		y.ValueChanged += (double _) =>
		{
			changed?.Invoke(new Vector2((float)x.Value, (float)y.Value));
		};
		hBoxContainer.AddChild(x, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(y, forceReadableName: false, InternalMode.Disabled);
		return WrapField(label, hBoxContainer);
	}

	private static SpinBox CreateVectorSpinBox(string name, float value)
	{
		SpinBox spinBox = new SpinBox();
		spinBox.Name = name;
		spinBox.MinValue = -99999.0;
		spinBox.MaxValue = 99999.0;
		spinBox.Step = 0.1;
		spinBox.Value = value;
		spinBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		spinBox.GetLineEdit().Alignment = HorizontalAlignment.Right;
		return spinBox;
	}

	private static Control AddOptionField(string nodeName, string label, string value, string[] options, Action<string> changed)
	{
		OptionButton option = new OptionButton
		{
			Name = nodeName,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		for (int i = 0; i < options.Length; i++)
		{
			option.AddItem(options[i], i);
			if (string.Equals(options[i], value, StringComparison.OrdinalIgnoreCase))
			{
				option.Select(i);
			}
		}
		option.ItemSelected += (long index) =>
		{
			changed?.Invoke(option.GetItemText((int)index));
		};
		return WrapField(label, option);
	}

	private static Control AddResourcePicker(string nodeName, string label, string baseType, Resource value, Action<Resource> changed)
	{
		XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
		xWResourcePicker.Name = nodeName;
		xWResourcePicker.Setup(baseType);
		xWResourcePicker.SetEditedResource(value);
		xWResourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		xWResourcePicker.ResourceChanged += (Resource resource) =>
		{
			changed?.Invoke(resource);
		};
		return WrapField(label, xWResourcePicker);
	}

	private static Texture2D LoadTexturePreview(string path)
	{
		if (string.IsNullOrWhiteSpace(path) || !ResourceLoader.Exists(path, "Texture2D"))
		{
			return null;
		}
		return ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse);
	}

	private static Control WrapField(string label, Control editor)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddThemeConstantOverride("separation", 8);
		hBoxContainer.AddChild(new Label
		{
			Text = label,
			CustomMinimumSize = new Vector2(168f, 0f),
			VerticalAlignment = VerticalAlignment.Center,
			ClipText = true
		}, forceReadableName: false, InternalMode.Disabled);
		editor.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddChild(editor, forceReadableName: false, InternalMode.Disabled);
		return hBoxContainer;
	}

	private static Label CreateSectionTitle(string text)
	{
		return new Label
		{
			Text = text,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
	}

	private static Label CreateEmptyEntryLabel(string text)
	{
		return new Label
		{
			Text = text,
			AutowrapMode = TextServer.AutowrapMode.WordSmart,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
	}

	private static Button CreateToolbarButton(string text, Action pressed)
	{
		Button button = new Button();
		button.Text = text;
		button.CustomMinimumSize = new Vector2(54f, 28f);
		button.Pressed += () =>
		{
			pressed?.Invoke();
		};
		return button;
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
		return new List<MethodInfo>(116)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCompleteCharacterDataVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResetCharacterDataBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeCharacterDataBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseCharacterDataPreviewBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindCharacterDataMetadata, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnCharacterDataPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LightweightCharacterDataPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderCharacterDataItemEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderCharacterDataItemEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderCharacterDataEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateCharacterDataVisualPreview, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindCharacterDataVisualPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "panel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderArmorDataEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "armorData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderCustomDataEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "customData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderDamagePointDataEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "damagePointData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshEntryList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenderSelectedEntryEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateArmorSlotEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateCustomConfigEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateCustomTextureArrayEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddCustomTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelectedCustomTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.MoveSelectedCustomTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCustomTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "texturePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyTexturePaths, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateDamagePointConfigEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateArmorTypeEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateArmorStageCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddArmorStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveArmorStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.MoveArmorStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PreviewArmorStageThreshold, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitArmorStageThreshold, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetArmorStageTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "texturePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetArmorStageArrays, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Array, "stagePersontage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "stageAnimeTexturePaths", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "refreshMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyThresholds, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToggleArmorMethodFlag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "flag", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowAudioSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddResourceMetadataFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddArmorSlot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelectedArmorSlot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddCustomConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelectedCustomConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddDamagePoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelectedDamagePoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedAggregateEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyArmorSlots, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyCustomConfigs, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyDamagePoints, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetEntryProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "entry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeCharacterData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureArmorList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCustomList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureDamagePointList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClampSelectedIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetEntryCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetArmorSlot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCustomConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetDamagePointConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildEntryTitle, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateVisualPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildRuntimeCharacterPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRuntimeCharacterDataPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleDamagePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetDamagePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetDamagePreviewPlaying, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "playing", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "stateText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPreviewHealthRatio, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateDamagePreviewControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stateText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCharacterDataPreviewVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPreviewArmorName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPreviewCustomName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindOwningCharacterConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindCharacterPackageRoot, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedStringArray, "paths", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CharacterConfigReferencesData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SameResource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "left", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "right", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindRuntimeCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimePreviewMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "missing", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnRuntimePreviewGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateRuntimePreviewTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizePreviewPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawCharacterDataPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvas", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawDamagePointConfigPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvas", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawArmorSlotConfigPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvas", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawCustomConfigPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvas", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawArmorTypePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvas", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawArmorPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvas", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawDamagePointPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvas", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawCustomPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvas", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawHealthBar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvas", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "x", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "y", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildArmorPreviewStatus, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildDamagePreviewStatus, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCustomPreviewStatus, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildDamagePointConfigPreviewStatus, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildArmorSlotPreviewStatus, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCustomConfigPreviewStatus, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildArmorTypePreviewStatus, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsAggregateCharacterData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsSupportedCharacterDataResource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateSummary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildDataSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCacheSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddBoundTextEdit, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddBoundSpinBox, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "min", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "max", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddBoundVector2Field, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCharacterDataEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateVectorSpinBox, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadTexturePreview, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WrapField, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSectionTitle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEmptyEntryLabel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.HasCompleteCharacterDataVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteCharacterDataVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.ResetCharacterDataBindings && args.Count == 0)
		{
			ResetCharacterDataBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeCharacterDataBindings && args.Count == 0)
		{
			DisposeCharacterDataBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseCharacterDataPreviewBindings && args.Count == 0)
		{
			ReleaseCharacterDataPreviewBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.BindCharacterDataMetadata && args.Count == 2)
		{
			BindCharacterDataMetadata(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCharacterDataPropertyEdited && args.Count == 1)
		{
			OnCharacterDataPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LightweightCharacterDataPreview && args.Count == 0)
		{
			LightweightCharacterDataPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderCharacterDataItemEditor && args.Count == 2)
		{
			RenderCharacterDataItemEditor(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderCharacterDataItemEditor && args.Count == 1)
		{
			RenderCharacterDataItemEditor(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderCharacterDataEditor && args.Count == 1)
		{
			RenderCharacterDataEditor(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCharacterDataVisualPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateCharacterDataVisualPreview(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BindCharacterDataVisualPreview && args.Count == 2)
		{
			BindCharacterDataVisualPreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderArmorDataEditor && args.Count == 2)
		{
			RenderArmorDataEditor(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<CharacterArmorData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderCustomDataEditor && args.Count == 2)
		{
			RenderCustomDataEditor(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<CharacterCustomData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderDamagePointDataEditor && args.Count == 2)
		{
			RenderDamagePointDataEditor(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<CharacterDamagePointData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEntryList && args.Count == 0)
		{
			RefreshEntryList();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectEntry && args.Count == 1)
		{
			SelectEntry(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderSelectedEntryEditor && args.Count == 0)
		{
			RenderSelectedEntryEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateArmorSlotEditor && args.Count == 3)
		{
			CreateArmorSlotEditor(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<ArmorSlotConfig>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCustomConfigEditor && args.Count == 3)
		{
			CreateCustomConfigEditor(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<CharacterCustomConfig>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCustomTextureArrayEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateCustomTextureArrayEditor(VariantUtils.ConvertTo<CharacterCustomConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.AddCustomTexture && args.Count == 1)
		{
			AddCustomTexture(VariantUtils.ConvertTo<CharacterCustomConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedCustomTexture && args.Count == 1)
		{
			RemoveSelectedCustomTexture(VariantUtils.ConvertTo<CharacterCustomConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedCustomTexture && args.Count == 2)
		{
			MoveSelectedCustomTexture(VariantUtils.ConvertTo<CharacterCustomConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCustomTexture && args.Count == 3)
		{
			SetCustomTexture(VariantUtils.ConvertTo<CharacterCustomConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CopyTexturePaths && args.Count == 1)
		{
			Array<string> array = CopyTexturePaths(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CreateDamagePointConfigEditor && args.Count == 3)
		{
			CreateDamagePointConfigEditor(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<CharacterDamagePointConfig>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateArmorTypeEditor && args.Count == 2)
		{
			CreateArmorTypeEditor(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateArmorStageCard && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateArmorStageCard(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0])));
			return true;
		}
		if (method == MethodName.AddArmorStage && args.Count == 1)
		{
			AddArmorStage(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveArmorStage && args.Count == 1)
		{
			RemoveArmorStage(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveArmorStage && args.Count == 2)
		{
			MoveArmorStage(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewArmorStageThreshold && args.Count == 3)
		{
			PreviewArmorStageThreshold(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitArmorStageThreshold && args.Count == 3)
		{
			CommitArmorStageThreshold(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetArmorStageTexture && args.Count == 3)
		{
			SetArmorStageTexture(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetArmorStageArrays && args.Count == 5)
		{
			SetArmorStageArrays(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0]), VariantUtils.ConvertToArray<double>(in args[1]), VariantUtils.ConvertToArray<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<StringName>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.CopyThresholds && args.Count == 1)
		{
			Array<double> array2 = CopyThresholds(VariantUtils.ConvertToArray<double>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.ToggleArmorMethodFlag && args.Count == 3)
		{
			ToggleArmorMethodFlag(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.ARMOR_METHOD_FLAGS>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowAudioSelector && args.Count == 1)
		{
			ShowAudioSelector(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddResourceMetadataFields && args.Count == 2)
		{
			AddResourceMetadataFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddArmorSlot && args.Count == 1)
		{
			AddArmorSlot(VariantUtils.ConvertTo<CharacterArmorData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedArmorSlot && args.Count == 0)
		{
			RemoveSelectedArmorSlot();
			ret = default;
			return true;
		}
		if (method == MethodName.AddCustomConfig && args.Count == 1)
		{
			AddCustomConfig(VariantUtils.ConvertTo<CharacterCustomData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedCustomConfig && args.Count == 0)
		{
			RemoveSelectedCustomConfig();
			ret = default;
			return true;
		}
		if (method == MethodName.AddDamagePoint && args.Count == 1)
		{
			AddDamagePoint(VariantUtils.ConvertTo<CharacterDamagePointData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedDamagePoint && args.Count == 0)
		{
			RemoveSelectedDamagePoint();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedAggregateEntry && args.Count == 1)
		{
			MoveSelectedAggregateEntry(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CopyArmorSlots && args.Count == 1)
		{
			Array<ArmorSlotConfig> array3 = CopyArmorSlots(VariantUtils.ConvertToArray<ArmorSlotConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.CopyCustomConfigs && args.Count == 1)
		{
			Array<CharacterCustomConfig> array4 = CopyCustomConfigs(VariantUtils.ConvertToArray<CharacterCustomConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array4);
			return true;
		}
		if (method == MethodName.CopyDamagePoints && args.Count == 1)
		{
			Array<CharacterDamagePointConfig> array5 = CopyDamagePoints(VariantUtils.ConvertToArray<CharacterDamagePointConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array5);
			return true;
		}
		if (method == MethodName.SetEntryProperty && args.Count == 4)
		{
			SetEntryProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeCharacterData && args.Count == 1)
		{
			NormalizeCharacterData(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureArmorList && args.Count == 1)
		{
			EnsureArmorList(VariantUtils.ConvertTo<CharacterArmorData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCustomList && args.Count == 1)
		{
			EnsureCustomList(VariantUtils.ConvertTo<CharacterCustomData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureDamagePointList && args.Count == 1)
		{
			EnsureDamagePointList(VariantUtils.ConvertTo<CharacterDamagePointData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClampSelectedIndex && args.Count == 1)
		{
			ClampSelectedIndex(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetEntryCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetEntryCount(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetArmorSlot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ArmorSlotConfig>(GetArmorSlot(VariantUtils.ConvertTo<CharacterArmorData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCustomConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CharacterCustomConfig>(GetCustomConfig(VariantUtils.ConvertTo<CharacterCustomData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDamagePointConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CharacterDamagePointConfig>(GetDamagePointConfig(VariantUtils.ConvertTo<CharacterDamagePointData>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildEntryTitle && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildEntryTitle(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.UpdateVisualPreview && args.Count == 0)
		{
			UpdateVisualPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildRuntimeCharacterPreview && args.Count == 1)
		{
			RebuildRuntimeCharacterPreview(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRuntimeCharacterDataPreview && args.Count == 0)
		{
			ApplyRuntimeCharacterDataPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleDamagePreview && args.Count == 0)
		{
			ToggleDamagePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetDamagePreview && args.Count == 0)
		{
			ResetDamagePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.SetDamagePreviewPlaying && args.Count == 2)
		{
			SetDamagePreviewPlaying(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPreviewHealthRatio && args.Count == 0)
		{
			ApplyPreviewHealthRatio();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDamagePreviewControls && args.Count == 1)
		{
			UpdateDamagePreviewControls(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged && args.Count == 1)
		{
			OnVisualEditorVisibilityChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCharacterDataPreviewVisibility && args.Count == 1)
		{
			ApplyCharacterDataPreviewVisibility(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPreviewArmorName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetPreviewArmorName());
			return true;
		}
		if (method == MethodName.GetPreviewCustomName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetPreviewCustomName());
			return true;
		}
		if (method == MethodName.FindOwningCharacterConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacterConfig>(FindOwningCharacterConfig(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.FindCharacterPackageRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FindCharacterPackageRoot(VariantUtils.ConvertTo<string[]>(in args[0])));
			return true;
		}
		if (method == MethodName.CharacterConfigReferencesData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CharacterConfigReferencesData(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.SameResource && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SameResource(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.FindRuntimeCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindRuntimeCharacter(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SetRuntimePreviewMissing && args.Count == 2)
		{
			SetRuntimePreviewMissing(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnRuntimePreviewGuiInput && args.Count == 1)
		{
			OnRuntimePreviewGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRuntimePreviewTransform && args.Count == 0)
		{
			UpdateRuntimePreviewTransform();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizePreviewPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizePreviewPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DrawCharacterDataPreview && args.Count == 1)
		{
			DrawCharacterDataPreview(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawDamagePointConfigPreview && args.Count == 2)
		{
			DrawDamagePointConfigPreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<CharacterDamagePointConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawArmorSlotConfigPreview && args.Count == 2)
		{
			DrawArmorSlotConfigPreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<ArmorSlotConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawCustomConfigPreview && args.Count == 2)
		{
			DrawCustomConfigPreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<CharacterCustomConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawArmorTypePreview && args.Count == 2)
		{
			DrawArmorTypePreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawArmorPreview && args.Count == 2)
		{
			DrawArmorPreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<CharacterArmorData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawDamagePointPreview && args.Count == 2)
		{
			DrawDamagePointPreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<CharacterDamagePointData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawCustomPreview && args.Count == 2)
		{
			DrawCustomPreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<CharacterCustomData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawHealthBar && args.Count == 5)
		{
			DrawHealthBar(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildArmorPreviewStatus && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildArmorPreviewStatus(VariantUtils.ConvertTo<CharacterArmorData>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildDamagePreviewStatus && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildDamagePreviewStatus(VariantUtils.ConvertTo<CharacterDamagePointData>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCustomPreviewStatus && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCustomPreviewStatus(VariantUtils.ConvertTo<CharacterCustomData>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildDamagePointConfigPreviewStatus && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildDamagePointConfigPreviewStatus(VariantUtils.ConvertTo<CharacterDamagePointConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildArmorSlotPreviewStatus && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildArmorSlotPreviewStatus(VariantUtils.ConvertTo<ArmorSlotConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCustomConfigPreviewStatus && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCustomConfigPreviewStatus(VariantUtils.ConvertTo<CharacterCustomConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildArmorTypePreviewStatus && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildArmorTypePreviewStatus(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAggregateCharacterData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAggregateCharacterData(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSupportedCharacterDataResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSupportedCharacterDataResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateSummary && args.Count == 0)
		{
			UpdateSummary();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildDataSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildDataSummary(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCacheSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCacheSummary(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.AddBoundTextEdit && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Control>(AddBoundTextEdit(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Resource>(in args[2]), VariantUtils.ConvertTo<StringName>(in args[3])));
			return true;
		}
		if (method == MethodName.AddBoundSpinBox && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<Control>(AddBoundSpinBox(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Resource>(in args[2]), VariantUtils.ConvertTo<StringName>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6])));
			return true;
		}
		if (method == MethodName.AddBoundVector2Field && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Control>(AddBoundVector2Field(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Resource>(in args[2]), VariantUtils.ConvertTo<StringName>(in args[3])));
			return true;
		}
		if (method == MethodName.RefreshCharacterDataEditorFromHistory && args.Count == 0)
		{
			RefreshCharacterDataEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateVectorSpinBox && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateVectorSpinBox(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.LoadTexturePreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadTexturePreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.WrapField && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(WrapField(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateSectionTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateSectionTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateEmptyEntryLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateEmptyEntryLabel(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.HasCompleteCharacterDataVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteCharacterDataVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.CopyTexturePaths && args.Count == 1)
		{
			Array<string> array = CopyTexturePaths(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CopyThresholds && args.Count == 1)
		{
			Array<double> array2 = CopyThresholds(VariantUtils.ConvertToArray<double>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.CopyArmorSlots && args.Count == 1)
		{
			Array<ArmorSlotConfig> array3 = CopyArmorSlots(VariantUtils.ConvertToArray<ArmorSlotConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.CopyCustomConfigs && args.Count == 1)
		{
			Array<CharacterCustomConfig> array4 = CopyCustomConfigs(VariantUtils.ConvertToArray<CharacterCustomConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array4);
			return true;
		}
		if (method == MethodName.CopyDamagePoints && args.Count == 1)
		{
			Array<CharacterDamagePointConfig> array5 = CopyDamagePoints(VariantUtils.ConvertToArray<CharacterDamagePointConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array5);
			return true;
		}
		if (method == MethodName.NormalizeCharacterData && args.Count == 1)
		{
			NormalizeCharacterData(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureArmorList && args.Count == 1)
		{
			EnsureArmorList(VariantUtils.ConvertTo<CharacterArmorData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCustomList && args.Count == 1)
		{
			EnsureCustomList(VariantUtils.ConvertTo<CharacterCustomData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureDamagePointList && args.Count == 1)
		{
			EnsureDamagePointList(VariantUtils.ConvertTo<CharacterDamagePointData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetEntryCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetEntryCount(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildEntryTitle && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildEntryTitle(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.FindCharacterPackageRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FindCharacterPackageRoot(VariantUtils.ConvertTo<string[]>(in args[0])));
			return true;
		}
		if (method == MethodName.CharacterConfigReferencesData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CharacterConfigReferencesData(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.SameResource && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SameResource(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.FindRuntimeCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindRuntimeCharacter(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizePreviewPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizePreviewPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCustomConfigPreviewStatus && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCustomConfigPreviewStatus(VariantUtils.ConvertTo<CharacterCustomConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAggregateCharacterData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAggregateCharacterData(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSupportedCharacterDataResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSupportedCharacterDataResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildDataSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildDataSummary(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCacheSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCacheSummary(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateVectorSpinBox && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateVectorSpinBox(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.LoadTexturePreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadTexturePreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.WrapField && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(WrapField(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateSectionTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateSectionTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateEmptyEntryLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateEmptyEntryLabel(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.HasCompleteCharacterDataVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.ResetCharacterDataBindings)
		{
			return true;
		}
		if (method == MethodName.DisposeCharacterDataBindings)
		{
			return true;
		}
		if (method == MethodName.ReleaseCharacterDataPreviewBindings)
		{
			return true;
		}
		if (method == MethodName.BindCharacterDataMetadata)
		{
			return true;
		}
		if (method == MethodName.OnCharacterDataPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.LightweightCharacterDataPreview)
		{
			return true;
		}
		if (method == MethodName.RenderCharacterDataItemEditor)
		{
			return true;
		}
		if (method == MethodName.RenderCharacterDataEditor)
		{
			return true;
		}
		if (method == MethodName.CreateCharacterDataVisualPreview)
		{
			return true;
		}
		if (method == MethodName.BindCharacterDataVisualPreview)
		{
			return true;
		}
		if (method == MethodName.RenderArmorDataEditor)
		{
			return true;
		}
		if (method == MethodName.RenderCustomDataEditor)
		{
			return true;
		}
		if (method == MethodName.RenderDamagePointDataEditor)
		{
			return true;
		}
		if (method == MethodName.RefreshEntryList)
		{
			return true;
		}
		if (method == MethodName.SelectEntry)
		{
			return true;
		}
		if (method == MethodName.RenderSelectedEntryEditor)
		{
			return true;
		}
		if (method == MethodName.CreateArmorSlotEditor)
		{
			return true;
		}
		if (method == MethodName.CreateCustomConfigEditor)
		{
			return true;
		}
		if (method == MethodName.CreateCustomTextureArrayEditor)
		{
			return true;
		}
		if (method == MethodName.AddCustomTexture)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedCustomTexture)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedCustomTexture)
		{
			return true;
		}
		if (method == MethodName.SetCustomTexture)
		{
			return true;
		}
		if (method == MethodName.CopyTexturePaths)
		{
			return true;
		}
		if (method == MethodName.CreateDamagePointConfigEditor)
		{
			return true;
		}
		if (method == MethodName.CreateArmorTypeEditor)
		{
			return true;
		}
		if (method == MethodName.CreateArmorStageCard)
		{
			return true;
		}
		if (method == MethodName.AddArmorStage)
		{
			return true;
		}
		if (method == MethodName.RemoveArmorStage)
		{
			return true;
		}
		if (method == MethodName.MoveArmorStage)
		{
			return true;
		}
		if (method == MethodName.PreviewArmorStageThreshold)
		{
			return true;
		}
		if (method == MethodName.CommitArmorStageThreshold)
		{
			return true;
		}
		if (method == MethodName.SetArmorStageTexture)
		{
			return true;
		}
		if (method == MethodName.SetArmorStageArrays)
		{
			return true;
		}
		if (method == MethodName.CopyThresholds)
		{
			return true;
		}
		if (method == MethodName.ToggleArmorMethodFlag)
		{
			return true;
		}
		if (method == MethodName.ShowAudioSelector)
		{
			return true;
		}
		if (method == MethodName.AddResourceMetadataFields)
		{
			return true;
		}
		if (method == MethodName.AddArmorSlot)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedArmorSlot)
		{
			return true;
		}
		if (method == MethodName.AddCustomConfig)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedCustomConfig)
		{
			return true;
		}
		if (method == MethodName.AddDamagePoint)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedDamagePoint)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedAggregateEntry)
		{
			return true;
		}
		if (method == MethodName.CopyArmorSlots)
		{
			return true;
		}
		if (method == MethodName.CopyCustomConfigs)
		{
			return true;
		}
		if (method == MethodName.CopyDamagePoints)
		{
			return true;
		}
		if (method == MethodName.SetEntryProperty)
		{
			return true;
		}
		if (method == MethodName.NormalizeCharacterData)
		{
			return true;
		}
		if (method == MethodName.EnsureArmorList)
		{
			return true;
		}
		if (method == MethodName.EnsureCustomList)
		{
			return true;
		}
		if (method == MethodName.EnsureDamagePointList)
		{
			return true;
		}
		if (method == MethodName.ClampSelectedIndex)
		{
			return true;
		}
		if (method == MethodName.GetEntryCount)
		{
			return true;
		}
		if (method == MethodName.GetArmorSlot)
		{
			return true;
		}
		if (method == MethodName.GetCustomConfig)
		{
			return true;
		}
		if (method == MethodName.GetDamagePointConfig)
		{
			return true;
		}
		if (method == MethodName.BuildEntryTitle)
		{
			return true;
		}
		if (method == MethodName.UpdateVisualPreview)
		{
			return true;
		}
		if (method == MethodName.RebuildRuntimeCharacterPreview)
		{
			return true;
		}
		if (method == MethodName.ApplyRuntimeCharacterDataPreview)
		{
			return true;
		}
		if (method == MethodName.ToggleDamagePreview)
		{
			return true;
		}
		if (method == MethodName.ResetDamagePreview)
		{
			return true;
		}
		if (method == MethodName.SetDamagePreviewPlaying)
		{
			return true;
		}
		if (method == MethodName.ApplyPreviewHealthRatio)
		{
			return true;
		}
		if (method == MethodName.UpdateDamagePreviewControls)
		{
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.ApplyCharacterDataPreviewVisibility)
		{
			return true;
		}
		if (method == MethodName.GetPreviewArmorName)
		{
			return true;
		}
		if (method == MethodName.GetPreviewCustomName)
		{
			return true;
		}
		if (method == MethodName.FindOwningCharacterConfig)
		{
			return true;
		}
		if (method == MethodName.FindCharacterPackageRoot)
		{
			return true;
		}
		if (method == MethodName.CharacterConfigReferencesData)
		{
			return true;
		}
		if (method == MethodName.SameResource)
		{
			return true;
		}
		if (method == MethodName.FindRuntimeCharacter)
		{
			return true;
		}
		if (method == MethodName.SetRuntimePreviewMissing)
		{
			return true;
		}
		if (method == MethodName.OnRuntimePreviewGuiInput)
		{
			return true;
		}
		if (method == MethodName.UpdateRuntimePreviewTransform)
		{
			return true;
		}
		if (method == MethodName.NormalizePreviewPath)
		{
			return true;
		}
		if (method == MethodName.DrawCharacterDataPreview)
		{
			return true;
		}
		if (method == MethodName.DrawDamagePointConfigPreview)
		{
			return true;
		}
		if (method == MethodName.DrawArmorSlotConfigPreview)
		{
			return true;
		}
		if (method == MethodName.DrawCustomConfigPreview)
		{
			return true;
		}
		if (method == MethodName.DrawArmorTypePreview)
		{
			return true;
		}
		if (method == MethodName.DrawArmorPreview)
		{
			return true;
		}
		if (method == MethodName.DrawDamagePointPreview)
		{
			return true;
		}
		if (method == MethodName.DrawCustomPreview)
		{
			return true;
		}
		if (method == MethodName.DrawHealthBar)
		{
			return true;
		}
		if (method == MethodName.BuildArmorPreviewStatus)
		{
			return true;
		}
		if (method == MethodName.BuildDamagePreviewStatus)
		{
			return true;
		}
		if (method == MethodName.BuildCustomPreviewStatus)
		{
			return true;
		}
		if (method == MethodName.BuildDamagePointConfigPreviewStatus)
		{
			return true;
		}
		if (method == MethodName.BuildArmorSlotPreviewStatus)
		{
			return true;
		}
		if (method == MethodName.BuildCustomConfigPreviewStatus)
		{
			return true;
		}
		if (method == MethodName.BuildArmorTypePreviewStatus)
		{
			return true;
		}
		if (method == MethodName.IsAggregateCharacterData)
		{
			return true;
		}
		if (method == MethodName.IsSupportedCharacterDataResource)
		{
			return true;
		}
		if (method == MethodName.UpdateSummary)
		{
			return true;
		}
		if (method == MethodName.BuildDataSummary)
		{
			return true;
		}
		if (method == MethodName.BuildCacheSummary)
		{
			return true;
		}
		if (method == MethodName.AddBoundTextEdit)
		{
			return true;
		}
		if (method == MethodName.AddBoundSpinBox)
		{
			return true;
		}
		if (method == MethodName.AddBoundVector2Field)
		{
			return true;
		}
		if (method == MethodName.RefreshCharacterDataEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.CreateVectorSpinBox)
		{
			return true;
		}
		if (method == MethodName.LoadTexturePreview)
		{
			return true;
		}
		if (method == MethodName.WrapField)
		{
			return true;
		}
		if (method == MethodName.CreateSectionTitle)
		{
			return true;
		}
		if (method == MethodName.CreateEmptyEntryLabel)
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
		if (name == PropertyName.RuntimeCharacterPreviewBuildCount)
		{
			RuntimeCharacterPreviewBuildCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeCharacterPreviewApplyCount)
		{
			RuntimeCharacterPreviewApplyCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.DamagePreviewProcessTickCount)
		{
			DamagePreviewProcessTickCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._editingData)
		{
			_editingData = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._selectedIndex)
		{
			_selectedIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._entryList)
		{
			_entryList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._entryEditorHost)
		{
			_entryEditorHost = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			_summaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._cacheSummaryLabel)
		{
			_cacheSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewViewportContainer)
		{
			_previewViewportContainer = VariantUtils.ConvertTo<SubViewportContainer>(in value);
			return true;
		}
		if (name == PropertyName._previewRoot)
		{
			_previewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._previewMissingLabel)
		{
			_previewMissingLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._configurationPreviewCanvas)
		{
			_configurationPreviewCanvas = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._fallbackRibbon)
		{
			_fallbackRibbon = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._fallbackRibbonLabel)
		{
			_fallbackRibbonLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewHealthValueLabel)
		{
			_previewHealthValueLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewPlaybackStateLabel)
		{
			_previewPlaybackStateLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._runtimeCharacterPreview)
		{
			_runtimeCharacterPreview = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._previewOwnerConfig)
		{
			_previewOwnerConfig = VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in value);
			return true;
		}
		if (name == PropertyName._previewHealthSlider)
		{
			_previewHealthSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._previewStatusLabel)
		{
			_previewStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewPlayPauseButton)
		{
			_previewPlayPauseButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._previewResetButton)
		{
			_previewResetButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._previewViewport)
		{
			_previewViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._previewHealthRatio)
		{
			_previewHealthRatio = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._damagePreviewElapsed)
		{
			_damagePreviewElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._damagePreviewPlaying)
		{
			_damagePreviewPlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._draggingPreview)
		{
			_draggingPreview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._previewPan)
		{
			_previewPan = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._previewZoom)
		{
			_previewZoom = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._audioPicker)
		{
			_audioPicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._audioTargetResource)
		{
			_audioTargetResource = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._audioTargetProperty)
		{
			_audioTargetProperty = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._selectedCustomTextureIndex)
		{
			_selectedCustomTextureIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedArmorStageIndex)
		{
			_selectedArmorStageIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.RuntimeCharacterPreviewBuildCount)
		{
			from = RuntimeCharacterPreviewBuildCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RuntimeCharacterPreviewApplyCount)
		{
			from = RuntimeCharacterPreviewApplyCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DamagePreviewProcessTickCount)
		{
			from = DamagePreviewProcessTickCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.IsDamagePreviewPlaying)
		{
			from2 = IsDamagePreviewPlaying;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PreviewHealthRatio)
		{
			value = VariantUtils.CreateFrom<double>(PreviewHealthRatio);
			return true;
		}
		if (name == PropertyName.CurrentRuntimeCharacterPreview)
		{
			value = VariantUtils.CreateFrom<TowerDefenseCharacter>(CurrentRuntimeCharacterPreview);
			return true;
		}
		if (name == PropertyName.IsCharacterDataPreviewRendering)
		{
			from2 = IsCharacterDataPreviewRendering;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._editingData)
		{
			value = VariantUtils.CreateFrom(in _editingData);
			return true;
		}
		if (name == PropertyName._selectedIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedIndex);
			return true;
		}
		if (name == PropertyName._entryList)
		{
			value = VariantUtils.CreateFrom(in _entryList);
			return true;
		}
		if (name == PropertyName._entryEditorHost)
		{
			value = VariantUtils.CreateFrom(in _entryEditorHost);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			value = VariantUtils.CreateFrom(in _summaryLabel);
			return true;
		}
		if (name == PropertyName._cacheSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _cacheSummaryLabel);
			return true;
		}
		if (name == PropertyName._previewViewportContainer)
		{
			value = VariantUtils.CreateFrom(in _previewViewportContainer);
			return true;
		}
		if (name == PropertyName._previewRoot)
		{
			value = VariantUtils.CreateFrom(in _previewRoot);
			return true;
		}
		if (name == PropertyName._previewMissingLabel)
		{
			value = VariantUtils.CreateFrom(in _previewMissingLabel);
			return true;
		}
		if (name == PropertyName._configurationPreviewCanvas)
		{
			value = VariantUtils.CreateFrom(in _configurationPreviewCanvas);
			return true;
		}
		if (name == PropertyName._fallbackRibbon)
		{
			value = VariantUtils.CreateFrom(in _fallbackRibbon);
			return true;
		}
		if (name == PropertyName._fallbackRibbonLabel)
		{
			value = VariantUtils.CreateFrom(in _fallbackRibbonLabel);
			return true;
		}
		if (name == PropertyName._previewHealthValueLabel)
		{
			value = VariantUtils.CreateFrom(in _previewHealthValueLabel);
			return true;
		}
		if (name == PropertyName._previewPlaybackStateLabel)
		{
			value = VariantUtils.CreateFrom(in _previewPlaybackStateLabel);
			return true;
		}
		if (name == PropertyName._runtimeCharacterPreview)
		{
			value = VariantUtils.CreateFrom(in _runtimeCharacterPreview);
			return true;
		}
		if (name == PropertyName._previewOwnerConfig)
		{
			value = VariantUtils.CreateFrom(in _previewOwnerConfig);
			return true;
		}
		if (name == PropertyName._previewHealthSlider)
		{
			value = VariantUtils.CreateFrom(in _previewHealthSlider);
			return true;
		}
		if (name == PropertyName._previewStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _previewStatusLabel);
			return true;
		}
		if (name == PropertyName._previewPlayPauseButton)
		{
			value = VariantUtils.CreateFrom(in _previewPlayPauseButton);
			return true;
		}
		if (name == PropertyName._previewResetButton)
		{
			value = VariantUtils.CreateFrom(in _previewResetButton);
			return true;
		}
		if (name == PropertyName._previewViewport)
		{
			value = VariantUtils.CreateFrom(in _previewViewport);
			return true;
		}
		if (name == PropertyName._previewHealthRatio)
		{
			value = VariantUtils.CreateFrom(in _previewHealthRatio);
			return true;
		}
		if (name == PropertyName._damagePreviewElapsed)
		{
			value = VariantUtils.CreateFrom(in _damagePreviewElapsed);
			return true;
		}
		if (name == PropertyName._damagePreviewPlaying)
		{
			value = VariantUtils.CreateFrom(in _damagePreviewPlaying);
			return true;
		}
		if (name == PropertyName._draggingPreview)
		{
			value = VariantUtils.CreateFrom(in _draggingPreview);
			return true;
		}
		if (name == PropertyName._previewPan)
		{
			value = VariantUtils.CreateFrom(in _previewPan);
			return true;
		}
		if (name == PropertyName._previewZoom)
		{
			value = VariantUtils.CreateFrom(in _previewZoom);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._audioPicker)
		{
			value = VariantUtils.CreateFrom(in _audioPicker);
			return true;
		}
		if (name == PropertyName._audioTargetResource)
		{
			value = VariantUtils.CreateFrom(in _audioTargetResource);
			return true;
		}
		if (name == PropertyName._audioTargetProperty)
		{
			value = VariantUtils.CreateFrom(in _audioTargetProperty);
			return true;
		}
		if (name == PropertyName._selectedCustomTextureIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedCustomTextureIndex);
			return true;
		}
		if (name == PropertyName._selectedArmorStageIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedArmorStageIndex);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._entryList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._entryEditorHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._summaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cacheSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewViewportContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewMissingLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._configurationPreviewCanvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fallbackRibbon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fallbackRibbonLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewHealthValueLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewPlaybackStateLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeCharacterPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewOwnerConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewHealthSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewPlayPauseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewResetButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._previewHealthRatio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._damagePreviewElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._damagePreviewPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._draggingPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previewPan, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._previewZoom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioTargetResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._audioTargetProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedCustomTextureIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedArmorStageIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RuntimeCharacterPreviewBuildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RuntimeCharacterPreviewApplyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DamagePreviewProcessTickCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsDamagePreviewPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.PreviewHealthRatio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentRuntimeCharacterPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCharacterDataPreviewRendering, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.RuntimeCharacterPreviewBuildCount, Variant.From<int>(RuntimeCharacterPreviewBuildCount));
		info.AddProperty(PropertyName.RuntimeCharacterPreviewApplyCount, Variant.From<int>(RuntimeCharacterPreviewApplyCount));
		info.AddProperty(PropertyName.DamagePreviewProcessTickCount, Variant.From<int>(DamagePreviewProcessTickCount));
		info.AddProperty(PropertyName._editingData, Variant.From(in _editingData));
		info.AddProperty(PropertyName._selectedIndex, Variant.From(in _selectedIndex));
		info.AddProperty(PropertyName._entryList, Variant.From(in _entryList));
		info.AddProperty(PropertyName._entryEditorHost, Variant.From(in _entryEditorHost));
		info.AddProperty(PropertyName._summaryLabel, Variant.From(in _summaryLabel));
		info.AddProperty(PropertyName._cacheSummaryLabel, Variant.From(in _cacheSummaryLabel));
		info.AddProperty(PropertyName._previewViewportContainer, Variant.From(in _previewViewportContainer));
		info.AddProperty(PropertyName._previewRoot, Variant.From(in _previewRoot));
		info.AddProperty(PropertyName._previewMissingLabel, Variant.From(in _previewMissingLabel));
		info.AddProperty(PropertyName._configurationPreviewCanvas, Variant.From(in _configurationPreviewCanvas));
		info.AddProperty(PropertyName._fallbackRibbon, Variant.From(in _fallbackRibbon));
		info.AddProperty(PropertyName._fallbackRibbonLabel, Variant.From(in _fallbackRibbonLabel));
		info.AddProperty(PropertyName._previewHealthValueLabel, Variant.From(in _previewHealthValueLabel));
		info.AddProperty(PropertyName._previewPlaybackStateLabel, Variant.From(in _previewPlaybackStateLabel));
		info.AddProperty(PropertyName._runtimeCharacterPreview, Variant.From(in _runtimeCharacterPreview));
		info.AddProperty(PropertyName._previewOwnerConfig, Variant.From(in _previewOwnerConfig));
		info.AddProperty(PropertyName._previewHealthSlider, Variant.From(in _previewHealthSlider));
		info.AddProperty(PropertyName._previewStatusLabel, Variant.From(in _previewStatusLabel));
		info.AddProperty(PropertyName._previewPlayPauseButton, Variant.From(in _previewPlayPauseButton));
		info.AddProperty(PropertyName._previewResetButton, Variant.From(in _previewResetButton));
		info.AddProperty(PropertyName._previewViewport, Variant.From(in _previewViewport));
		info.AddProperty(PropertyName._previewHealthRatio, Variant.From(in _previewHealthRatio));
		info.AddProperty(PropertyName._damagePreviewElapsed, Variant.From(in _damagePreviewElapsed));
		info.AddProperty(PropertyName._damagePreviewPlaying, Variant.From(in _damagePreviewPlaying));
		info.AddProperty(PropertyName._draggingPreview, Variant.From(in _draggingPreview));
		info.AddProperty(PropertyName._previewPan, Variant.From(in _previewPan));
		info.AddProperty(PropertyName._previewZoom, Variant.From(in _previewZoom));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._audioPicker, Variant.From(in _audioPicker));
		info.AddProperty(PropertyName._audioTargetResource, Variant.From(in _audioTargetResource));
		info.AddProperty(PropertyName._audioTargetProperty, Variant.From(in _audioTargetProperty));
		info.AddProperty(PropertyName._selectedCustomTextureIndex, Variant.From(in _selectedCustomTextureIndex));
		info.AddProperty(PropertyName._selectedArmorStageIndex, Variant.From(in _selectedArmorStageIndex));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.RuntimeCharacterPreviewBuildCount, out var value))
		{
			RuntimeCharacterPreviewBuildCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeCharacterPreviewApplyCount, out var value2))
		{
			RuntimeCharacterPreviewApplyCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.DamagePreviewProcessTickCount, out var value3))
		{
			DamagePreviewProcessTickCount = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._editingData, out var value4))
		{
			_editingData = value4.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._selectedIndex, out var value5))
		{
			_selectedIndex = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._entryList, out var value6))
		{
			_entryList = value6.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._entryEditorHost, out var value7))
		{
			_entryEditorHost = value7.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._summaryLabel, out var value8))
		{
			_summaryLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._cacheSummaryLabel, out var value9))
		{
			_cacheSummaryLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewViewportContainer, out var value10))
		{
			_previewViewportContainer = value10.As<SubViewportContainer>();
		}
		if (info.TryGetProperty(PropertyName._previewRoot, out var value11))
		{
			_previewRoot = value11.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._previewMissingLabel, out var value12))
		{
			_previewMissingLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._configurationPreviewCanvas, out var value13))
		{
			_configurationPreviewCanvas = value13.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._fallbackRibbon, out var value14))
		{
			_fallbackRibbon = value14.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._fallbackRibbonLabel, out var value15))
		{
			_fallbackRibbonLabel = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewHealthValueLabel, out var value16))
		{
			_previewHealthValueLabel = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewPlaybackStateLabel, out var value17))
		{
			_previewPlaybackStateLabel = value17.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._runtimeCharacterPreview, out var value18))
		{
			_runtimeCharacterPreview = value18.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._previewOwnerConfig, out var value19))
		{
			_previewOwnerConfig = value19.As<TowerDefenseCharacterConfig>();
		}
		if (info.TryGetProperty(PropertyName._previewHealthSlider, out var value20))
		{
			_previewHealthSlider = value20.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._previewStatusLabel, out var value21))
		{
			_previewStatusLabel = value21.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewPlayPauseButton, out var value22))
		{
			_previewPlayPauseButton = value22.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._previewResetButton, out var value23))
		{
			_previewResetButton = value23.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._previewViewport, out var value24))
		{
			_previewViewport = value24.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._previewHealthRatio, out var value25))
		{
			_previewHealthRatio = value25.As<double>();
		}
		if (info.TryGetProperty(PropertyName._damagePreviewElapsed, out var value26))
		{
			_damagePreviewElapsed = value26.As<double>();
		}
		if (info.TryGetProperty(PropertyName._damagePreviewPlaying, out var value27))
		{
			_damagePreviewPlaying = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._draggingPreview, out var value28))
		{
			_draggingPreview = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._previewPan, out var value29))
		{
			_previewPan = value29.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._previewZoom, out var value30))
		{
			_previewZoom = value30.As<float>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value31))
		{
			_updatingControls = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._audioPicker, out var value32))
		{
			_audioPicker = value32.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._audioTargetResource, out var value33))
		{
			_audioTargetResource = value33.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._audioTargetProperty, out var value34))
		{
			_audioTargetProperty = value34.As<string>();
		}
		if (info.TryGetProperty(PropertyName._selectedCustomTextureIndex, out var value35))
		{
			_selectedCustomTextureIndex = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedArmorStageIndex, out var value36))
		{
			_selectedArmorStageIndex = value36.As<int>();
		}
	}
}
