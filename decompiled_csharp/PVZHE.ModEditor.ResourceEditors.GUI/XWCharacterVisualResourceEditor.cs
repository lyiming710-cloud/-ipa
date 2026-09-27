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

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterVisualResourceEditor.cs")]
public class XWCharacterVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName OnEmbeddedInspectorPropertyChanged = "OnEmbeddedInspectorPropertyChanged";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public static readonly StringName RenderCharacterOverrideEditor = "RenderCharacterOverrideEditor";

		public static readonly StringName PrepareCharacterOverrideSession = "PrepareCharacterOverrideSession";

		public static readonly StringName BindCharacterOverrideScene = "BindCharacterOverrideScene";

		public static readonly StringName OpenOverrideBaseCharacterPicker = "OpenOverrideBaseCharacterPicker";

		public static readonly StringName UpdateCharacterOverrideContext = "UpdateCharacterOverrideContext";

		public static readonly StringName RebuildCharacterOverridePreview = "RebuildCharacterOverridePreview";

		public static readonly StringName BuildCharacterOverridePreviewNode = "BuildCharacterOverridePreviewNode";

		public static readonly StringName InstantiateCharacterPreviewScene = "InstantiateCharacterPreviewScene";

		public static readonly StringName ApplySafeCharacterOverridePreview = "ApplySafeCharacterOverridePreview";

		public static readonly StringName ApplySafePropertyChangePreview = "ApplySafePropertyChangePreview";

		public static readonly StringName IsSafeCharacterPreviewProperty = "IsSafeCharacterPreviewProperty";

		public static readonly StringName StableRangeMidpoint = "StableRangeMidpoint";

		public static readonly StringName PopulateCharacterOverrideControls = "PopulateCharacterOverrideControls";

		public static readonly StringName BindCharacterOverrideEditing = "BindCharacterOverrideEditing";

		public static readonly StringName OnCharacterOverrideVisualPropertyEdited = "OnCharacterOverrideVisualPropertyEdited";

		public static readonly StringName SelectOverridePropertyChange = "SelectOverridePropertyChange";

		public static readonly StringName GetSelectedCharacterOverridePropertyChange = "GetSelectedCharacterOverridePropertyChange";

		public static readonly StringName AddCharacterOverridePropertyChange = "AddCharacterOverridePropertyChange";

		public static readonly StringName DuplicateSelectedCharacterOverridePropertyChange = "DuplicateSelectedCharacterOverridePropertyChange";

		public static readonly StringName RemoveSelectedCharacterOverridePropertyChange = "RemoveSelectedCharacterOverridePropertyChange";

		public static readonly StringName MoveSelectedCharacterOverridePropertyChange = "MoveSelectedCharacterOverridePropertyChange";

		public static readonly StringName UpdateCharacterOverridePropertyButtons = "UpdateCharacterOverridePropertyButtons";

		public static readonly StringName RefreshCharacterOverridePropertyList = "RefreshCharacterOverridePropertyList";

		public static readonly StringName FormatPropertyChange = "FormatPropertyChange";

		public static readonly StringName OpenOverridePropertyChange = "OpenOverridePropertyChange";

		public static readonly StringName RefreshCharacterOverrideWorkbench = "RefreshCharacterOverrideWorkbench";

		public static readonly StringName RefreshCharacterOverrideSummary = "RefreshCharacterOverrideSummary";

		public static readonly StringName FormatOverrideNumber = "FormatOverrideNumber";

		public static readonly StringName FormatRange = "FormatRange";

		public static readonly StringName OnCharacterOverridePreviewGuiInput = "OnCharacterOverridePreviewGuiInput";

		public static readonly StringName ResetCharacterOverridePreviewView = "ResetCharacterOverridePreviewView";

		public static readonly StringName UpdateCharacterOverridePreviewTransform = "UpdateCharacterOverridePreviewTransform";

		public static readonly StringName UpdateCharacterOverrideStageLayout = "UpdateCharacterOverrideStageLayout";

		public static readonly StringName SetPreviewRootProcessing = "SetPreviewRootProcessing";

		public static readonly StringName ClearPreviewRoot = "ClearPreviewRoot";

		public static readonly StringName DisposeCharacterOverrideEditor = "DisposeCharacterOverrideEditor";

		public static readonly StringName RenderCharacterEditor = "RenderCharacterEditor";

		public static readonly StringName BindCharacterPreview = "BindCharacterPreview";

		public static readonly StringName BindCharacterBasicFields = "BindCharacterBasicFields";

		public static readonly StringName BindComponentPanel = "BindComponentPanel";

		public static readonly StringName BindOverrideDiffPanel = "BindOverrideDiffPanel";

		public static readonly StringName BindTypedCharacterFields = "BindTypedCharacterFields";

		public static readonly StringName SetTypedCharacterFieldVisibility = "SetTypedCharacterFieldVisibility";

		public static readonly StringName RebuildCharacterPreview = "RebuildCharacterPreview";

		public static readonly StringName BuildRuntimeCharacterPreviewScene = "BuildRuntimeCharacterPreviewScene";

		public static readonly StringName HasExternalCSharpSceneScript = "HasExternalCSharpSceneScript";

		public static readonly StringName BuildSpriteFallbackPreview = "BuildSpriteFallbackPreview";

		public static readonly StringName FindRuntimeCharacter = "FindRuntimeCharacter";

		public static readonly StringName InstantiateCharacterSpriteFallback = "InstantiateCharacterSpriteFallback";

		public static readonly StringName DisableCharacterFallbackGameplayProcessing = "DisableCharacterFallbackGameplayProcessing";

		public static readonly StringName OnPreviewHealthChanged = "OnPreviewHealthChanged";

		public static readonly StringName ApplyCharacterPreviewState = "ApplyCharacterPreviewState";

		public static readonly StringName OnCharacterPreviewGuiInput = "OnCharacterPreviewGuiInput";

		public static readonly StringName ResetCharacterPreviewView = "ResetCharacterPreviewView";

		public static readonly StringName UpdateCharacterPreviewTransform = "UpdateCharacterPreviewTransform";

		public static readonly StringName UpdateCharacterPreviewStageLayout = "UpdateCharacterPreviewStageLayout";

		public static readonly StringName FindCharacterPackagePreviewScene = "FindCharacterPackagePreviewScene";

		public static readonly StringName IsCharacterPreviewSpriteScenePath = "IsCharacterPreviewSpriteScenePath";

		public static readonly StringName GetCharacterPackageDirectoryForPreview = "GetCharacterPackageDirectoryForPreview";

		public static readonly StringName ResolveRelativeCharacterPath = "ResolveRelativeCharacterPath";

		public static readonly StringName ToAbsoluteCharacterPreviewFilePath = "ToAbsoluteCharacterPreviewFilePath";

		public static readonly StringName NormalizeCharacterPreviewPath = "NormalizeCharacterPreviewPath";

		public static readonly StringName OnCharacterFieldChanged = "OnCharacterFieldChanged";

		public static readonly StringName OnCharacterDiscreteFieldChanged = "OnCharacterDiscreteFieldChanged";

		public static readonly StringName ApplyCharacterDiscreteFieldFromHistory = "ApplyCharacterDiscreteFieldFromHistory";

		public static readonly StringName RefreshCharacterDiscreteSurfaceFromHistory = "RefreshCharacterDiscreteSurfaceFromHistory";

		public static readonly StringName SetResourceProperty = "SetResourceProperty";

		public static readonly StringName SavePendingCharacterResource = "SavePendingCharacterResource";

		public static readonly StringName SaveCharacterResource = "SaveCharacterResource";

		public static readonly StringName UpdateCharacterPreview = "UpdateCharacterPreview";

		public static readonly StringName RefreshComponentPanel = "RefreshComponentPanel";

		public static readonly StringName RefreshOverrideDiffPanel = "RefreshOverrideDiffPanel";

		public static readonly StringName AppendDiffRow = "AppendDiffRow";

		public static readonly StringName AddCharacterSummaryRows = "AddCharacterSummaryRows";

		public static readonly StringName MountSmallSegmentedOption = "MountSmallSegmentedOption";

		public static readonly StringName DisposeCharacterVisualChoices = "DisposeCharacterVisualChoices";

		public static readonly StringName BindCharacterKeyCatalog = "BindCharacterKeyCatalog";

		public static readonly StringName BindCharacterResourceCatalog = "BindCharacterResourceCatalog";

		public static readonly StringName EnsureCharacterCatalogPicker = "EnsureCharacterCatalogPicker";

		public static readonly StringName PopulateVariantList = "PopulateVariantList";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";

		public static readonly StringName BuildNameText = "BuildNameText";

		public static readonly StringName BuildCategoryText = "BuildCategoryText";

		public static readonly StringName BuildPacketText = "BuildPacketText";

		public static readonly StringName GetCharacterCategory = "GetCharacterCategory";

		public static readonly StringName GetCategoryColor = "GetCategoryColor";

		public static readonly StringName SetFlexibleProperty = "SetFlexibleProperty";

		public static readonly StringName ReadVariant = "ReadVariant";

		public static readonly StringName ReadString = "ReadString";

		public static readonly StringName ReadDouble = "ReadDouble";

		public static readonly StringName ReadResource = "ReadResource";

		public static readonly StringName ReadArray = "ReadArray";

		public static readonly StringName HasResourceProperty = "HasResourceProperty";

		public static readonly StringName GetModMetaName = "GetModMetaName";

		public static readonly StringName FormatVariant = "FormatVariant";

		public static readonly StringName FormatResource = "FormatResource";

		public static readonly StringName FormatPath = "FormatPath";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingCharacter = "_editingCharacter";

		public static readonly StringName _characterViewport = "_characterViewport";

		public static readonly StringName _characterViewportContainer = "_characterViewportContainer";

		public static readonly StringName _characterPreviewBackground = "_characterPreviewBackground";

		public static readonly StringName _characterGroundShade = "_characterGroundShade";

		public static readonly StringName _characterPreviewRoot = "_characterPreviewRoot";

		public static readonly StringName _runtimeCharacterPreview = "_runtimeCharacterPreview";

		public static readonly StringName _characterPreviewUsesExternalScriptFallback = "_characterPreviewUsesExternalScriptFallback";

		public static readonly StringName _previewStatusLabel = "_previewStatusLabel";

		public static readonly StringName _previewHealthSlider = "_previewHealthSlider";

		public static readonly StringName _previewHealthValueLabel = "_previewHealthValueLabel";

		public static readonly StringName _draggingCharacterPreview = "_draggingCharacterPreview";

		public static readonly StringName _characterPreviewPan = "_characterPreviewPan";

		public static readonly StringName _characterPreviewZoom = "_characterPreviewZoom";

		public static readonly StringName _previewNameLabel = "_previewNameLabel";

		public static readonly StringName _previewCategoryLabel = "_previewCategoryLabel";

		public static readonly StringName _previewModeLabel = "_previewModeLabel";

		public static readonly StringName _previewHitpointsLabel = "_previewHitpointsLabel";

		public static readonly StringName _previewPacketLabel = "_previewPacketLabel";

		public static readonly StringName _previewResourceLabel = "_previewResourceLabel";

		public static readonly StringName _componentListView = "_componentListView";

		public static readonly StringName _componentPatchListView = "_componentPatchListView";

		public static readonly StringName _spawnEventListView = "_spawnEventListView";

		public static readonly StringName _dieEventListView = "_dieEventListView";

		public static readonly StringName _propertyChangeListView = "_propertyChangeListView";

		public static readonly StringName _overrideDiffTree = "_overrideDiffTree";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _characterSaveTimer = "_characterSaveTimer";

		public static readonly StringName _editingCharacterOverride = "_editingCharacterOverride";

		public static readonly StringName _editingPropertyChange = "_editingPropertyChange";

		public static readonly StringName _editingOverrideResource = "_editingOverrideResource";

		public static readonly StringName _overrideBaseCharacter = "_overrideBaseCharacter";

		public static readonly StringName _overrideBasePacket = "_overrideBasePacket";

		public static readonly StringName _overrideRuntimeScene = "_overrideRuntimeScene";

		public static readonly StringName _overrideSpriteScene = "_overrideSpriteScene";

		public static readonly StringName _characterOverrideWorkbench = "_characterOverrideWorkbench";

		public static readonly StringName _overrideViewport = "_overrideViewport";

		public static readonly StringName _overrideViewportContainer = "_overrideViewportContainer";

		public static readonly StringName _overridePreviewBackground = "_overridePreviewBackground";

		public static readonly StringName _overrideGroundShade = "_overrideGroundShade";

		public static readonly StringName _overridePreviewCameraRoot = "_overridePreviewCameraRoot";

		public static readonly StringName _overrideBaseCharacterRoot = "_overrideBaseCharacterRoot";

		public static readonly StringName _overriddenCharacterRoot = "_overriddenCharacterRoot";

		public static readonly StringName _overrideBaseRuntimeCharacter = "_overrideBaseRuntimeCharacter";

		public static readonly StringName _overriddenRuntimeCharacter = "_overriddenRuntimeCharacter";

		public static readonly StringName _overrideContextLabel = "_overrideContextLabel";

		public static readonly StringName _overrideSourceLabel = "_overrideSourceLabel";

		public static readonly StringName _overridePreviewStatusLabel = "_overridePreviewStatusLabel";

		public static readonly StringName _overrideZoomLabel = "_overrideZoomLabel";

		public static readonly StringName _overrideEffectSummaryLabel = "_overrideEffectSummaryLabel";

		public static readonly StringName _overrideSafetySummaryLabel = "_overrideSafetySummaryLabel";

		public static readonly StringName _overrideSaveStateLabel = "_overrideSaveStateLabel";

		public static readonly StringName _overridePropertyChangeList = "_overridePropertyChangeList";

		public static readonly StringName _selectOverrideBaseCharacterButton = "_selectOverrideBaseCharacterButton";

		public static readonly StringName _overrideInvisibleCheck = "_overrideInvisibleCheck";

		public static readonly StringName _overrideScaleSpinBox = "_overrideScaleSpinBox";

		public static readonly StringName _overrideHitpointScaleSpinBox = "_overrideHitpointScaleSpinBox";

		public static readonly StringName _overrideWalkSpeedMinSpinBox = "_overrideWalkSpeedMinSpinBox";

		public static readonly StringName _overrideWalkSpeedMaxSpinBox = "_overrideWalkSpeedMaxSpinBox";

		public static readonly StringName _overrideAnimeSpeedMinSpinBox = "_overrideAnimeSpeedMinSpinBox";

		public static readonly StringName _overrideAnimeSpeedMaxSpinBox = "_overrideAnimeSpeedMaxSpinBox";

		public static readonly StringName _overrideCanMowerMoveCheck = "_overrideCanMowerMoveCheck";

		public static readonly StringName _addOverridePropertyChangeButton = "_addOverridePropertyChangeButton";

		public static readonly StringName _duplicateOverridePropertyChangeButton = "_duplicateOverridePropertyChangeButton";

		public static readonly StringName _moveOverridePropertyChangeUpButton = "_moveOverridePropertyChangeUpButton";

		public static readonly StringName _moveOverridePropertyChangeDownButton = "_moveOverridePropertyChangeDownButton";

		public static readonly StringName _removeOverridePropertyChangeButton = "_removeOverridePropertyChangeButton";

		public static readonly StringName _overridePropertyNameLineEdit = "_overridePropertyNameLineEdit";

		public static readonly StringName _overridePropertyValueLineEdit = "_overridePropertyValueLineEdit";

		public static readonly StringName _overridePropertyValueTypeLabel = "_overridePropertyValueTypeLabel";

		public static readonly StringName _draggingOverridePreview = "_draggingOverridePreview";

		public static readonly StringName _overridePreviewPan = "_overridePreviewPan";

		public static readonly StringName _overridePreviewZoom = "_overridePreviewZoom";

		public static readonly StringName _overrideSelectedPacketName = "_overrideSelectedPacketName";

		public static readonly StringName _overrideResolvedPacketName = "_overrideResolvedPacketName";

		public static readonly StringName _overridePreviewResourceId = "_overridePreviewResourceId";

		public static readonly StringName _selectedOverridePropertyChangeIndex = "_selectedOverridePropertyChangeIndex";

		public static readonly StringName _overrideBaseCharacterPicker = "_overrideBaseCharacterPicker";

		public static readonly StringName _characterCatalogPicker = "_characterCatalogPicker";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string EditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterVisualEditorLayout.tscn";

	private const string CharacterBasicFieldsScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterBasicFields.tscn";

	private const string CharacterOverrideEditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterOverrideEditorLayout.tscn";

	private const string CharacterRegistryPath = "res://Asset/Config/Character/CharacterResource.json";

	private static PackedScene _editorLayoutScene;

	private static PackedScene _characterBasicFieldsScene;

	private static PackedScene _characterOverrideEditorLayoutScene;

	private const string PropertyCategory = "category";

	private const string PropertyCreationMode = "creationMode";

	private const string PropertyBaseCharacterKey = "baseCharacterKey";

	private const string PropertyTargetCharacterKey = "targetCharacterKey";

	private const string PropertySceneOverride = "sceneOverride";

	private const string PropertySpriteOverride = "spriteOverride";

	private const string PropertyPacketPatch = "packetPatch";

	private const string PropertyCharacterConfigPatch = "characterConfigPatch";

	private const string PropertyComponentPatch = "componentPatch";

	private const string PropertyComponentList = "componentList";

	private const string PropertySpawnEvent = "spawnEvent";

	private const string PropertyDieEvent = "dieEvent";

	private const string PropertyPropertyChange = "propertyChange";

	private static readonly string[] CharacterCategories = new string[8] { "植物", "僵尸", "道具", "花瓶", "小推车", "物品", "墓碑", "弹坑" };

	private static readonly string[] CreationModes = new string[3] { "空白创建", "继承内置", "覆盖内置" };

	private static readonly HashSet<string> CharacterInspectorEditableProperties = new HashSet<string>
	{
		"resource_name", "resource_local_to_scene", "name", "hitpointsNearDeath", "hitpoints", "explosionHurt", "smashHurt", "dragHurt", "spikeHurt", "biteHurt",
		"canDragIntoWater", "canImitate", "canCopy", "warnningLineFliter", "sleepTime", "height", "damagePointData", "armorData", "customData", "ashScene",
		"homeWorld", "costRise", "cost", "costNight", "costMultiple", "packetCooldown", "startingCooldown", "plantCoverAll", "plantCoverSelf", "plantCover",
		"plantCoverRecycle", "plantCanHasSurround", "plantSurroundCanPlantWater", "plantSurroundCanHasSlot", "plantGridType", "plantGridOverrideType", "Flag/Collision", "Flag/Mask", "Flag/UnUseBuff", "Flag/PhysiqueType",
		"Flag/Elemet", "canUsePlantfood", "extendGrid", "extendCoverDictionary", "izm2Fliter", "physique", "attack", "smashAttack", "impactAudio", "preview",
		"weight", "wavePointCost", "canSpawnPlantfood", "excludeLineGridType", "spawnLineNeed", "type", "isLadder", "isShield", "mowerConfig", "category",
		"creationMode", "baseCharacterKey", "targetCharacterKey", "sceneOverride", "spriteOverride", "packetPatch", "characterConfigPatch", "componentPatch", "componentList", "spawnEvent",
		"dieEvent", "propertyChange", "invisible", "scale", "hitpointScale", "walkSpeedScale", "animeSpeedScale", "armor", "canMowerMove", "propertyName",
		"value"
	};

	private TowerDefenseCharacterConfig _editingCharacter;

	private SubViewport _characterViewport;

	private SubViewportContainer _characterViewportContainer;

	private TextureRect _characterPreviewBackground;

	private ColorRect _characterGroundShade;

	private Node2D _characterPreviewRoot;

	private TowerDefenseCharacter _runtimeCharacterPreview;

	private bool _characterPreviewUsesExternalScriptFallback;

	private Label _previewStatusLabel;

	private HSlider _previewHealthSlider;

	private Label _previewHealthValueLabel;

	private bool _draggingCharacterPreview;

	private Vector2 _characterPreviewPan;

	private float _characterPreviewZoom = 1f;

	private Label _previewNameLabel;

	private Label _previewCategoryLabel;

	private Label _previewModeLabel;

	private Label _previewHitpointsLabel;

	private Label _previewPacketLabel;

	private Label _previewResourceLabel;

	private ItemList _componentListView;

	private ItemList _componentPatchListView;

	private ItemList _spawnEventListView;

	private ItemList _dieEventListView;

	private ItemList _propertyChangeListView;

	private Tree _overrideDiffTree;

	private bool _updatingControls;

	private Timer _characterSaveTimer;

	private TowerDefenseCharacterOverride _editingCharacterOverride;

	private TowerDefenseCharacterPropertyChangeConfig _editingPropertyChange;

	private Resource _editingOverrideResource;

	private TowerDefenseCharacterConfig _overrideBaseCharacter;

	private TowerDefensePacketConfig _overrideBasePacket;

	private PackedScene _overrideRuntimeScene;

	private PackedScene _overrideSpriteScene;

	private PanelContainer _characterOverrideWorkbench;

	private SubViewport _overrideViewport;

	private SubViewportContainer _overrideViewportContainer;

	private TextureRect _overridePreviewBackground;

	private ColorRect _overrideGroundShade;

	private Node2D _overridePreviewCameraRoot;

	private Node2D _overrideBaseCharacterRoot;

	private Node2D _overriddenCharacterRoot;

	private TowerDefenseCharacter _overrideBaseRuntimeCharacter;

	private TowerDefenseCharacter _overriddenRuntimeCharacter;

	private Label _overrideContextLabel;

	private Label _overrideSourceLabel;

	private Label _overridePreviewStatusLabel;

	private Label _overrideZoomLabel;

	private Label _overrideEffectSummaryLabel;

	private Label _overrideSafetySummaryLabel;

	private Label _overrideSaveStateLabel;

	private ItemList _overridePropertyChangeList;

	private Button _selectOverrideBaseCharacterButton;

	private CheckButton _overrideInvisibleCheck;

	private SpinBox _overrideScaleSpinBox;

	private SpinBox _overrideHitpointScaleSpinBox;

	private SpinBox _overrideWalkSpeedMinSpinBox;

	private SpinBox _overrideWalkSpeedMaxSpinBox;

	private SpinBox _overrideAnimeSpeedMinSpinBox;

	private SpinBox _overrideAnimeSpeedMaxSpinBox;

	private CheckButton _overrideCanMowerMoveCheck;

	private Button _addOverridePropertyChangeButton;

	private Button _duplicateOverridePropertyChangeButton;

	private Button _moveOverridePropertyChangeUpButton;

	private Button _moveOverridePropertyChangeDownButton;

	private Button _removeOverridePropertyChangeButton;

	private LineEdit _overridePropertyNameLineEdit;

	private LineEdit _overridePropertyValueLineEdit;

	private Label _overridePropertyValueTypeLabel;

	private bool _draggingOverridePreview;

	private Vector2 _overridePreviewPan;

	private float _overridePreviewZoom = 1f;

	private string _overrideSelectedPacketName = "";

	private string _overrideResolvedPacketName = "";

	private ulong _overridePreviewResourceId;

	private int _selectedOverridePropertyChangeIndex = -1;

	private XWGameplayResourcePickerWindow _overrideBaseCharacterPicker;

	private XWGameplayResourcePickerWindow _characterCatalogPicker;

	private readonly List<XWVisualSegmentedOption> _characterSegmentedOptions = new List<XWVisualSegmentedOption>();

	private XWVisualPropertyBinding _overridePropertyBinding;

	public override void _Ready()
	{
		base._Ready();
		_characterSaveTimer = new Timer
		{
			WaitTime = 0.35,
			OneShot = true
		};
		_characterSaveTimer.Timeout += SavePendingCharacterResource;
		AddChild(_characterSaveTimer, forceReadableName: false, InternalMode.Disabled);
		EnableVisibilityGatedProcessing();
		RequestVisibilityGatedProcessing(requested: false);
	}

	public override void _ExitTree()
	{
		DisposeCharacterVisualChoices();
		if (GodotObject.IsInstanceValid(_characterSaveTimer))
		{
			_characterSaveTimer.Stop();
		}
		DisposeCharacterOverrideEditor(disposePicker: true);
		_editingCharacter = null;
		_runtimeCharacterPreview = null;
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeCharacterVisualChoices();
		DisposeCharacterOverrideEditor(disposePicker: false);
		_editingCharacter = null;
		_runtimeCharacterPreview = null;
		Resource currentResource = CurrentResource;
		if (!(currentResource is TowerDefenseCharacterConfig towerDefenseCharacterConfig))
		{
			if (!(currentResource is TowerDefenseCharacterOverride towerDefenseCharacterOverride))
			{
				if (currentResource is TowerDefenseCharacterPropertyChangeConfig towerDefenseCharacterPropertyChangeConfig)
				{
					_editingCharacter = null;
					_editingPropertyChange = towerDefenseCharacterPropertyChangeConfig;
					_editingOverrideResource = towerDefenseCharacterPropertyChangeConfig;
					RenderCharacterOverrideEditor(null, towerDefenseCharacterPropertyChangeConfig);
				}
			}
			else
			{
				_editingCharacter = null;
				_editingCharacterOverride = towerDefenseCharacterOverride;
				_editingOverrideResource = towerDefenseCharacterOverride;
				RenderCharacterOverrideEditor(towerDefenseCharacterOverride, null);
			}
		}
		else
		{
			_editingCharacter = towerDefenseCharacterConfig;
			RenderCharacterEditor(towerDefenseCharacterConfig);
		}
	}

	protected override HashSet<string> GetEmbeddedInspectorAllowedProperties(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if ((resource is TowerDefenseCharacterConfig || resource is TowerDefenseCharacterOverride || resource is TowerDefenseCharacterPropertyChangeConfig) ? true : false)
		{
			return CharacterInspectorEditableProperties;
		}
		return base.GetEmbeddedInspectorAllowedProperties(resource, path, descriptor);
	}

	protected override void OnEmbeddedInspectorPropertyChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		if (!(obj is TowerDefenseCharacterConfig editingCharacter))
		{
			if (!(obj is TowerDefenseCharacterOverride editingCharacterOverride))
			{
				if (obj is TowerDefenseCharacterPropertyChangeConfig editingPropertyChange)
				{
					_editingPropertyChange = editingPropertyChange;
					RefreshCharacterOverrideWorkbench(rebuildPreview: true);
				}
			}
			else
			{
				_editingCharacterOverride = editingCharacterOverride;
				RefreshCharacterOverrideWorkbench(rebuildPreview: true);
			}
		}
		else
		{
			_editingCharacter = editingCharacter;
			UpdateCharacterPreview();
			RefreshComponentPanel();
			RefreshOverrideDiffPanel();
		}
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		SetPreviewRootProcessing(_characterPreviewRoot, visible);
		SetPreviewRootProcessing(_overrideBaseCharacterRoot, visible);
		SetPreviewRootProcessing(_overriddenCharacterRoot, visible);
	}

	private void RenderCharacterOverrideEditor(TowerDefenseCharacterOverride characterOverride, TowerDefenseCharacterPropertyChangeConfig propertyChange)
	{
		if (CanvasGrid != null && GodotObject.IsInstanceValid(_editingOverrideResource))
		{
			PrepareCharacterOverrideSession();
			CanvasGrid.Columns = 1;
			if (_characterOverrideEditorLayoutScene == null)
			{
				_characterOverrideEditorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterOverrideEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_characterOverrideWorkbench = _characterOverrideEditorLayoutScene?.Instantiate<PanelContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(_characterOverrideWorkbench))
			{
				CanvasGrid.AddChild(_characterOverrideWorkbench, forceReadableName: false, InternalMode.Disabled);
				BindCharacterOverrideScene(_characterOverrideWorkbench);
				_overrideBaseCharacter = ResolveOverrideBaseCharacter(out _overrideBasePacket, out var packetName, out var sourceDescription);
				UpdateCharacterOverrideContext(packetName, sourceDescription);
				PopulateCharacterOverrideControls();
				BindCharacterOverrideEditing();
				RebuildCharacterOverridePreview();
				CallDeferred("UpdateCharacterOverrideStageLayout");
			}
		}
	}

	private void PrepareCharacterOverrideSession()
	{
		ulong num = (GodotObject.IsInstanceValid(CurrentResource) ? CurrentResource.GetInstanceId() : 0);
		if (_overridePreviewResourceId != num)
		{
			_overridePreviewResourceId = num;
			_overrideSelectedPacketName = "";
			_overrideResolvedPacketName = "";
			_overridePreviewPan = Vector2.Zero;
			_overridePreviewZoom = 1f;
		}
	}

	private void BindCharacterOverrideScene(PanelContainer root)
	{
		_overrideViewport = root.GetNode<SubViewport>("%OverrideViewport");
		_overrideViewportContainer = root.GetNode<SubViewportContainer>("%ViewportContainer");
		_overridePreviewBackground = root.GetNode<TextureRect>("%GameBackground");
		_overrideGroundShade = root.GetNode<ColorRect>("%GroundShade");
		_overridePreviewCameraRoot = root.GetNode<Node2D>("%PreviewCameraRoot");
		_overrideBaseCharacterRoot = root.GetNode<Node2D>("%BaseCharacterRoot");
		_overriddenCharacterRoot = root.GetNode<Node2D>("%OverriddenCharacterRoot");
		_overrideContextLabel = root.GetNode<Label>("%OverrideContextLabel");
		_overrideSourceLabel = root.GetNode<Label>("%OverrideSourceLabel");
		_overridePreviewStatusLabel = root.GetNode<Label>("%PreviewStatusLabel");
		_overrideZoomLabel = root.GetNode<Label>("%ZoomLabel");
		_overrideEffectSummaryLabel = root.GetNode<Label>("%EffectSummaryLabel");
		_overrideSafetySummaryLabel = root.GetNode<Label>("%SafetySummaryLabel");
		_overrideSaveStateLabel = root.GetNode<Label>("%SaveStateLabel");
		_overridePropertyChangeList = root.GetNode<ItemList>("%PropertyChangeList");
		_selectOverrideBaseCharacterButton = root.GetNode<Button>("%SelectBaseCharacterButton");
		_overrideInvisibleCheck = root.GetNode<CheckButton>("%InvisibleCheck");
		_overrideScaleSpinBox = root.GetNode<SpinBox>("%ScaleSpinBox");
		_overrideHitpointScaleSpinBox = root.GetNode<SpinBox>("%HitpointScaleSpinBox");
		_overrideWalkSpeedMinSpinBox = root.GetNode<SpinBox>("%WalkSpeedMinSpinBox");
		_overrideWalkSpeedMaxSpinBox = root.GetNode<SpinBox>("%WalkSpeedMaxSpinBox");
		_overrideAnimeSpeedMinSpinBox = root.GetNode<SpinBox>("%AnimeSpeedMinSpinBox");
		_overrideAnimeSpeedMaxSpinBox = root.GetNode<SpinBox>("%AnimeSpeedMaxSpinBox");
		_overrideCanMowerMoveCheck = root.GetNode<CheckButton>("%CanMowerMoveCheck");
		_addOverridePropertyChangeButton = root.GetNode<Button>("%AddPropertyChangeButton");
		_duplicateOverridePropertyChangeButton = root.GetNode<Button>("%DuplicatePropertyChangeButton");
		_moveOverridePropertyChangeUpButton = root.GetNode<Button>("%MovePropertyChangeUpButton");
		_moveOverridePropertyChangeDownButton = root.GetNode<Button>("%MovePropertyChangeDownButton");
		_removeOverridePropertyChangeButton = root.GetNode<Button>("%RemovePropertyChangeButton");
		_overridePropertyNameLineEdit = root.GetNode<LineEdit>("%PropertyNameLineEdit");
		_overridePropertyValueLineEdit = root.GetNode<LineEdit>("%PropertyValueLineEdit");
		_overridePropertyValueTypeLabel = root.GetNode<Label>("%PropertyValueTypeLabel");
		_overrideViewportContainer.GuiInput += OnCharacterOverridePreviewGuiInput;
		_overrideViewportContainer.Resized += UpdateCharacterOverrideStageLayout;
		root.GetNode<Button>("%ResetViewButton").Pressed += ResetCharacterOverridePreviewView;
		_selectOverrideBaseCharacterButton.Pressed += OpenOverrideBaseCharacterPicker;
		_overridePropertyChangeList.ItemActivated += OpenOverridePropertyChange;
		_overridePropertyChangeList.ItemSelected += SelectOverridePropertyChange;
		_addOverridePropertyChangeButton.Pressed += AddCharacterOverridePropertyChange;
		_duplicateOverridePropertyChangeButton.Pressed += DuplicateSelectedCharacterOverridePropertyChange;
		_moveOverridePropertyChangeUpButton.Pressed += () =>
		{
			MoveSelectedCharacterOverridePropertyChange(-1);
		};
		_moveOverridePropertyChangeDownButton.Pressed += () =>
		{
			MoveSelectedCharacterOverridePropertyChange(1);
		};
		_removeOverridePropertyChangeButton.Pressed += RemoveSelectedCharacterOverridePropertyChange;
	}

	private TowerDefenseCharacterConfig ResolveOverrideBaseCharacter(out TowerDefensePacketConfig packet, out string packetName, out string sourceDescription)
	{
		packet = null;
		packetName = "";
		sourceDescription = "未选择基础角色";
		_overrideRuntimeScene = null;
		_overrideSpriteScene = null;
		if (CurrentEditContext?.OwnerResource is TowerDefenseLevelPreSpawnConfig towerDefenseLevelPreSpawnConfig && !string.IsNullOrWhiteSpace(towerDefenseLevelPreSpawnConfig.packetName))
		{
			packetName = towerDefenseLevelPreSpawnConfig.packetName;
			sourceDescription = $"关卡预生成角色 · 格子 {towerDefenseLevelPreSpawnConfig.gridPos.X + 1},{towerDefenseLevelPreSpawnConfig.gridPos.Y + 1}";
		}
		else if (!string.IsNullOrWhiteSpace(_overrideSelectedPacketName))
		{
			packetName = _overrideSelectedPacketName;
			sourceDescription = "工作台临时预览选择（不会写入覆盖资源）";
		}
		else if (!string.IsNullOrWhiteSpace(CurrentEditContext?.PreviewKey))
		{
			packetName = CurrentEditContext.PreviewKey;
			sourceDescription = "来自上级角色覆盖工作台的预览上下文";
		}
		if (string.IsNullOrWhiteSpace(packetName))
		{
			_overrideResolvedPacketName = "";
			return null;
		}
		_overrideResolvedPacketName = packetName;
		if (ResourceManager.Instance != null)
		{
			Variant packet2 = ResourceManager.Instance.GetPacket(packetName);
			if (packet2.VariantType == Variant.Type.Object && packet2.AsGodotObject() is TowerDefensePacketConfig towerDefensePacketConfig && GodotObject.IsInstanceValid(towerDefensePacketConfig))
			{
				packet = towerDefensePacketConfig;
				if (GodotObject.IsInstanceValid(towerDefensePacketConfig.characterConfig))
				{
					_overrideRuntimeScene = ResourceManager.Instance.GetCharacterScene(towerDefensePacketConfig.characterConfig.name);
				}
			}
		}
		if (!GodotObject.IsInstanceValid(packet) && !TryLoadPacketFromCharacterRegistry(packetName, out packet, out _overrideRuntimeScene, out _overrideSpriteScene))
		{
			sourceDescription += " · 未在角色注册表中找到";
			return null;
		}
		if (!GodotObject.IsInstanceValid(_overrideRuntimeScene) && GodotObject.IsInstanceValid(packet?.characterConfig) && ResourceManager.Instance != null)
		{
			_overrideRuntimeScene = ResourceManager.Instance.GetCharacterScene(packet.characterConfig.name);
		}
		if (!GodotObject.IsInstanceValid(packet?.characterConfig))
		{
			return null;
		}
		return packet.characterConfig;
	}

	private static bool TryLoadPacketFromCharacterRegistry(string packetName, out TowerDefensePacketConfig packet, out PackedScene runtimeScene, out PackedScene spriteScene)
	{
		packet = null;
		runtimeScene = null;
		spriteScene = null;
		if (string.IsNullOrWhiteSpace(packetName))
		{
			return false;
		}
		Json json = ResourceLoader.Load<Json>("res://Asset/Config/Character/CharacterResource.json", "", ResourceLoader.CacheMode.Reuse);
		if (!GodotObject.IsInstanceValid(json) || json.Data.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		Dictionary dictionary = json.Data.AsGodotDictionary();
		foreach (Variant key2 in dictionary.Keys)
		{
			if (dictionary[key2].VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary2 = dictionary[key2].AsGodotDictionary();
			Variant valueOrDefault = dictionary2.GetValueOrDefault("Packet");
			if (valueOrDefault.VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary3 = valueOrDefault.AsGodotDictionary();
			Variant key = default;
			bool flag = false;
			foreach (Variant key3 in dictionary3.Keys)
			{
				if (string.Equals(key3.AsString(), packetName, StringComparison.OrdinalIgnoreCase))
				{
					key = key3;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				string path = dictionary3[key].AsString();
				packet = LoadRegistryResource<TowerDefensePacketConfig>(path);
				runtimeScene = LoadRegistryResource<PackedScene>(dictionary2.GetValueOrDefault("Scene", "").AsString());
				spriteScene = LoadRegistryResource<PackedScene>(dictionary2.GetValueOrDefault("Sprite", "").AsString());
				return GodotObject.IsInstanceValid(packet);
			}
		}
		return false;
	}

	private static T LoadRegistryResource<T>(string path) where T : Resource
	{
		if (string.IsNullOrWhiteSpace(path) || !ResourceLoader.Exists(path))
		{
			return null;
		}
		return ResourceLoader.Load<T>(path, "", ResourceLoader.CacheMode.Reuse);
	}

	private void OpenOverrideBaseCharacterPicker()
	{
		if (!GodotObject.IsInstanceValid(_overrideBaseCharacterPicker))
		{
			_overrideBaseCharacterPicker = XWGameplayResourcePickerWindow.Create();
			if (!GodotObject.IsInstanceValid(_overrideBaseCharacterPicker))
			{
				return;
			}
			AddChild(_overrideBaseCharacterPicker, forceReadableName: false, InternalMode.Disabled);
		}
		_overrideBaseCharacterPicker.Open(XWGameplayResourceKind.Card, _overrideSelectedPacketName, (XWGameplayResourceChoice choice) =>
		{
			if (!(choice == null) && !string.IsNullOrWhiteSpace(choice.Key))
			{
				_overrideSelectedPacketName = choice.Key;
				_overrideBaseCharacter = ResolveOverrideBaseCharacter(out _overrideBasePacket, out var packetName, out var sourceDescription);
				UpdateCharacterOverrideContext(packetName, sourceDescription);
				RebuildCharacterOverridePreview();
			}
		});
	}

	private void UpdateCharacterOverrideContext(string packetName, string sourceDescription)
	{
		bool flag = GodotObject.IsInstanceValid(_overrideBaseCharacter);
		if (GodotObject.IsInstanceValid(_overrideContextLabel))
		{
			_overrideContextLabel.Text = (flag ? ("基础角色：" + _overrideBaseCharacter.name + " · 卡片 " + packetName) : "尚未解析基础角色 · 请从关卡进入或手动选择");
		}
		if (GodotObject.IsInstanceValid(_overrideSourceLabel))
		{
			_overrideSourceLabel.Text = "来源：" + sourceDescription;
		}
		if (GodotObject.IsInstanceValid(_selectOverrideBaseCharacterButton))
		{
			_selectOverrideBaseCharacterButton.Text = (flag ? "更换预览角色" : "选择基础角色");
		}
	}

	private void RebuildCharacterOverridePreview()
	{
		ClearPreviewRoot(_overrideBaseCharacterRoot);
		ClearPreviewRoot(_overriddenCharacterRoot);
		_overrideBaseRuntimeCharacter = null;
		_overriddenRuntimeCharacter = null;
		if (!GodotObject.IsInstanceValid(_overrideBaseCharacter) || !GodotObject.IsInstanceValid(_overrideBaseCharacterRoot) || !GodotObject.IsInstanceValid(_overriddenCharacterRoot))
		{
			if (GodotObject.IsInstanceValid(_overridePreviewStatusLabel))
			{
				_overridePreviewStatusLabel.Visible = true;
				_overridePreviewStatusLabel.Text = "从关卡的预生成角色打开可自动带入模型，独立资源请先选择基础角色。";
			}
			RefreshCharacterOverrideSummary();
			return;
		}
		Node node = BuildCharacterOverridePreviewNode();
		Node node2 = BuildCharacterOverridePreviewNode();
		if (node != null)
		{
			node.Name = "OriginalCharacterPreview";
			_overrideBaseCharacterRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			_overrideBaseRuntimeCharacter = FindRuntimeCharacter(node);
		}
		if (node2 != null)
		{
			node2.Name = "OverriddenCharacterPreview";
			_overriddenCharacterRoot.AddChild(node2, forceReadableName: false, InternalMode.Disabled);
			_overriddenRuntimeCharacter = FindRuntimeCharacter(node2);
			ApplySafeCharacterOverridePreview();
		}
		bool flag = node != null && node2 != null;
		if (GodotObject.IsInstanceValid(_overridePreviewStatusLabel))
		{
			_overridePreviewStatusLabel.Visible = !flag;
			_overridePreviewStatusLabel.Text = (flag ? "" : "完整角色场景不可用，当前使用安全动画降级预览。");
		}
		SetPreviewRootProcessing(_overrideBaseCharacterRoot, IsVisibleInTree());
		SetPreviewRootProcessing(_overriddenCharacterRoot, IsVisibleInTree());
		RefreshCharacterOverrideSummary();
	}

	private Node BuildCharacterOverridePreviewNode()
	{
		Node node = InstantiateCharacterPreviewScene(_overrideRuntimeScene, _overrideBaseCharacter);
		if (node != null)
		{
			return node;
		}
		if (GodotObject.IsInstanceValid(_overrideSpriteScene))
		{
			return InstantiateCharacterSpriteFallback(_overrideSpriteScene);
		}
		return BuildSpriteFallbackPreview(_overrideBaseCharacter);
	}

	private static Node InstantiateCharacterPreviewScene(PackedScene scene, TowerDefenseCharacterConfig character)
	{
		if (!GodotObject.IsInstanceValid(scene) || !GodotObject.IsInstanceValid(character))
		{
			return null;
		}
		try
		{
			Node node = scene.Instantiate(PackedScene.GenEditState.Disabled);
			XWRuntimePreviewSceneAdapter.PrepareAnimationSprites(node);
			TowerDefenseCharacter towerDefenseCharacter = FindRuntimeCharacter(node);
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				node.Free();
				return null;
			}
			towerDefenseCharacter.inGame = false;
			towerDefenseCharacter.editorPreviewMode = true;
			towerDefenseCharacter.config = character;
			return node;
		}
		catch (Exception ex)
		{
			GD.PushWarning("Character override runtime preview failed: " + ex.Message);
			return null;
		}
	}

	private void ApplySafeCharacterOverridePreview()
	{
		if (!GodotObject.IsInstanceValid(_overriddenRuntimeCharacter))
		{
			return;
		}
		if (GodotObject.IsInstanceValid(_editingCharacterOverride))
		{
			if (_editingCharacterOverride.scale >= 0.0 && GodotObject.IsInstanceValid(_overriddenRuntimeCharacter.transformPoint))
			{
				_overriddenRuntimeCharacter.transformPoint.Scale = Vector2.One * (float)_editingCharacterOverride.scale;
			}
			if (_editingCharacterOverride.animeSpeedScale != new Vector2(-1f, -1f))
			{
				_overriddenRuntimeCharacter.timeScale = StableRangeMidpoint(_editingCharacterOverride.animeSpeedScale);
			}
			_overriddenRuntimeCharacter.Modulate = (_editingCharacterOverride.invisible ? new Color(0.75f, 0.9f, 1f, 0.28f) : Colors.White);
			if (_editingCharacterOverride.propertyChange == null)
			{
				return;
			}
			{
				foreach (TowerDefenseCharacterPropertyChangeConfig item in _editingCharacterOverride.propertyChange)
				{
					ApplySafePropertyChangePreview(item);
				}
				return;
			}
		}
		if (GodotObject.IsInstanceValid(_editingPropertyChange))
		{
			ApplySafePropertyChangePreview(_editingPropertyChange);
		}
	}

	private void ApplySafePropertyChangePreview(TowerDefenseCharacterPropertyChangeConfig change)
	{
		if (!GodotObject.IsInstanceValid(change) || !GodotObject.IsInstanceValid(_overriddenRuntimeCharacter) || string.IsNullOrWhiteSpace(change.propertyName) || !IsSafeCharacterPreviewProperty(change.propertyName))
		{
			return;
		}
		foreach (Dictionary property in _overriddenRuntimeCharacter.GetPropertyList())
		{
			if (string.Equals(property.GetValueOrDefault("name", "").AsString(), change.propertyName, StringComparison.Ordinal))
			{
				if (TryConvertSafeCharacterPreviewValue(property, change.value, out var previewValue))
				{
					_overriddenRuntimeCharacter.Set(change.propertyName, previewValue);
				}
				break;
			}
		}
	}

	private static bool TryConvertSafeCharacterPreviewValue(Dictionary property, Variant value, out Variant previewValue)
	{
		previewValue = default;
		Variant valueOrDefault = property.GetValueOrDefault("type");
		if (valueOrDefault.VariantType != Variant.Type.Int || value.VariantType == Variant.Type.Nil)
		{
			return false;
		}
		Variant.Type type = (Variant.Type)valueOrDefault.AsInt32();
		if (type == value.VariantType)
		{
			previewValue = value;
			return true;
		}
		if (type == Variant.Type.Float && value.VariantType == Variant.Type.Int)
		{
			previewValue = Variant.From<double>(value.AsDouble());
			return true;
		}
		if (type == Variant.Type.Int && value.VariantType == Variant.Type.Float)
		{
			previewValue = Variant.From<long>((long)Math.Round(value.AsDouble()));
			return true;
		}
		if (type == Variant.Type.String && value.VariantType == Variant.Type.StringName)
		{
			previewValue = Variant.From<string>(value.AsString());
			return true;
		}
		if (type == Variant.Type.StringName && value.VariantType == Variant.Type.String)
		{
			previewValue = Variant.From<StringName>(new StringName(value.AsString()));
			return true;
		}
		return false;
	}

	private static bool IsSafeCharacterPreviewProperty(string propertyName)
	{
		if (propertyName != null)
		{
			int length = propertyName.Length;
			if (length <= 9)
			{
				if (length != 4)
				{
					if (length == 9)
					{
						char c = propertyName[0];
						if (c != 'h')
						{
							if (c != 'i')
							{
								if (c == 't' && propertyName == "timeScale")
								{
									goto IL_00c6;
								}
							}
							else if (propertyName == "invisible")
							{
								goto IL_00c6;
							}
						}
						else if (propertyName == "hitpoints")
						{
							goto IL_00c6;
						}
					}
				}
				else if (propertyName == "cost")
				{
					goto IL_00c6;
				}
			}
			else if (length != 12)
			{
				if (length == 28 && propertyName == "previewDamagePointPersontage")
				{
					goto IL_00c6;
				}
			}
			else
			{
				char c = propertyName[0];
				if (c != 'c')
				{
					if (c == 'h' && propertyName == "hitpointsMax")
					{
						goto IL_00c6;
					}
				}
				else if (propertyName == "canMowerMove")
				{
					goto IL_00c6;
				}
			}
		}
		return false;
		IL_00c6:
		return true;
	}

	private static float StableRangeMidpoint(Vector2 range)
	{
		return (range.X + range.Y) * 0.5f;
	}

	private void PopulateCharacterOverrideControls()
	{
		bool flag = GodotObject.IsInstanceValid(_editingCharacterOverride);
		_updatingControls = true;
		try
		{
			_overrideInvisibleCheck.ButtonPressed = flag && _editingCharacterOverride.invisible;
			_overrideScaleSpinBox.Value = (flag ? _editingCharacterOverride.scale : (-1.0));
			_overrideHitpointScaleSpinBox.Value = (flag ? _editingCharacterOverride.hitpointScale : (-1.0));
			_overrideWalkSpeedMinSpinBox.Value = (flag ? ((double)_editingCharacterOverride.walkSpeedScale.X) : (-1.0));
			_overrideWalkSpeedMaxSpinBox.Value = (flag ? ((double)_editingCharacterOverride.walkSpeedScale.Y) : (-1.0));
			_overrideAnimeSpeedMinSpinBox.Value = (flag ? ((double)_editingCharacterOverride.animeSpeedScale.X) : (-1.0));
			_overrideAnimeSpeedMaxSpinBox.Value = (flag ? ((double)_editingCharacterOverride.animeSpeedScale.Y) : (-1.0));
			_overrideCanMowerMoveCheck.ButtonPressed = flag && _editingCharacterOverride.canMowerMove;
		}
		finally
		{
			_updatingControls = false;
		}
		_overrideInvisibleCheck.Disabled = !flag;
		_overrideScaleSpinBox.Editable = flag;
		_overrideHitpointScaleSpinBox.Editable = flag;
		_overrideWalkSpeedMinSpinBox.Editable = flag;
		_overrideWalkSpeedMaxSpinBox.Editable = flag;
		_overrideAnimeSpeedMinSpinBox.Editable = flag;
		_overrideAnimeSpeedMaxSpinBox.Editable = flag;
		_overrideCanMowerMoveCheck.Disabled = !flag;
		_addOverridePropertyChangeButton.Disabled = !flag;
		_duplicateOverridePropertyChangeButton.Disabled = !flag;
		_removeOverridePropertyChangeButton.Disabled = !flag;
		RefreshCharacterOverridePropertyList();
		RefreshCharacterOverrideSummary();
	}

	private void BindCharacterOverrideEditing()
	{
		_overridePropertyBinding?.Dispose();
		_overridePropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnCharacterOverrideVisualPropertyEdited);
		if (GodotObject.IsInstanceValid(_editingCharacterOverride))
		{
			_overridePropertyBinding.BindToggle(_overrideInvisibleCheck, _editingCharacterOverride, "invisible", null);
			_overridePropertyBinding.BindNumber(_overrideScaleSpinBox, _editingCharacterOverride, "scale", null);
			_overridePropertyBinding.BindNumber(_overrideHitpointScaleSpinBox, _editingCharacterOverride, "hitpointScale", null);
			_overridePropertyBinding.BindVector2Range(_overrideWalkSpeedMinSpinBox, _overrideWalkSpeedMaxSpinBox, _editingCharacterOverride, "walkSpeedScale", null);
			_overridePropertyBinding.BindVector2Range(_overrideAnimeSpeedMinSpinBox, _overrideAnimeSpeedMaxSpinBox, _editingCharacterOverride, "animeSpeedScale", null);
			_overridePropertyBinding.BindToggle(_overrideCanMowerMoveCheck, _editingCharacterOverride, "canMowerMove", null);
		}
		TowerDefenseCharacterPropertyChangeConfig selectedCharacterOverridePropertyChange = GetSelectedCharacterOverridePropertyChange();
		bool flag = GodotObject.IsInstanceValid(selectedCharacterOverridePropertyChange);
		_overridePropertyNameLineEdit.Editable = flag;
		_overridePropertyValueLineEdit.Editable = flag;
		if (flag)
		{
			_overridePropertyBinding.BindText(_overridePropertyNameLineEdit, selectedCharacterOverridePropertyChange, "propertyName", null);
			_overridePropertyBinding.BindVariantText(_overridePropertyValueLineEdit, _overridePropertyValueTypeLabel, selectedCharacterOverridePropertyChange, "value", null);
		}
		else
		{
			_overridePropertyNameLineEdit.Text = "";
			_overridePropertyValueLineEdit.Text = "";
			_overridePropertyValueTypeLabel.Text = "类型：—";
		}
		UpdateCharacterOverridePropertyButtons();
	}

	private void OnCharacterOverrideVisualPropertyEdited(bool committed)
	{
		NotifyCurrentResourceEdited();
		RefreshCharacterOverridePropertyList();
		RefreshCharacterOverrideSummary();
		RebuildCharacterOverridePreview();
		if (GodotObject.IsInstanceValid(_overrideSaveStateLabel))
		{
			_overrideSaveStateLabel.Text = (committed ? "未保存 · 已加入撤销记录" : "未保存 · 正在实时预览");
		}
	}

	private void SelectOverridePropertyChange(long index)
	{
		_selectedOverridePropertyChangeIndex = (int)index;
		BindCharacterOverrideEditing();
	}

	private TowerDefenseCharacterPropertyChangeConfig GetSelectedCharacterOverridePropertyChange()
	{
		if (GodotObject.IsInstanceValid(_editingPropertyChange))
		{
			return _editingPropertyChange;
		}
		if (!GodotObject.IsInstanceValid(_editingCharacterOverride) || _editingCharacterOverride.propertyChange == null || _selectedOverridePropertyChangeIndex < 0 || _selectedOverridePropertyChangeIndex >= _editingCharacterOverride.propertyChange.Count)
		{
			return null;
		}
		return _editingCharacterOverride.propertyChange[_selectedOverridePropertyChangeIndex];
	}

	private void AddCharacterOverridePropertyChange()
	{
		if (GodotObject.IsInstanceValid(_editingCharacterOverride) && _overridePropertyBinding != null)
		{
			TowerDefenseCharacterOverride editingCharacterOverride = _editingCharacterOverride;
			if (editingCharacterOverride.propertyChange == null)
			{
				editingCharacterOverride.propertyChange = new Array<TowerDefenseCharacterPropertyChangeConfig>();
			}
			Array<TowerDefenseCharacterPropertyChangeConfig> array = new Array<TowerDefenseCharacterPropertyChangeConfig>(_editingCharacterOverride.propertyChange);
			TowerDefenseCharacterPropertyChangeConfig item = new TowerDefenseCharacterPropertyChangeConfig
			{
				ResourceName = $"PropertyChange_{array.Count + 1}",
				ResourceLocalToScene = true,
				propertyName = "timeScale",
				value = 1.0
			};
			array.Add(item);
			_selectedOverridePropertyChangeIndex = array.Count - 1;
			_overridePropertyBinding.SetValue(_editingCharacterOverride, "propertyChange", array, "新增角色属性变化");
			RefreshCharacterOverridePropertyList();
			BindCharacterOverrideEditing();
		}
	}

	private void DuplicateSelectedCharacterOverridePropertyChange()
	{
		if (GodotObject.IsInstanceValid(_editingCharacterOverride) && _editingCharacterOverride.propertyChange != null && _overridePropertyBinding != null)
		{
			TowerDefenseCharacterPropertyChangeConfig selectedCharacterOverridePropertyChange = GetSelectedCharacterOverridePropertyChange();
			if (GodotObject.IsInstanceValid(selectedCharacterOverridePropertyChange))
			{
				TowerDefenseCharacterPropertyChangeConfig towerDefenseCharacterPropertyChangeConfig = (selectedCharacterOverridePropertyChange.Duplicate(deep: true) as TowerDefenseCharacterPropertyChangeConfig) ?? new TowerDefenseCharacterPropertyChangeConfig
				{
					propertyName = selectedCharacterOverridePropertyChange.propertyName,
					value = selectedCharacterOverridePropertyChange.value
				};
				towerDefenseCharacterPropertyChangeConfig.ResourceLocalToScene = true;
				towerDefenseCharacterPropertyChangeConfig.ResourceName = (string.IsNullOrWhiteSpace(selectedCharacterOverridePropertyChange.ResourceName) ? $"PropertyChange_{_editingCharacterOverride.propertyChange.Count + 1}" : (selectedCharacterOverridePropertyChange.ResourceName + "_Copy"));
				Array<TowerDefenseCharacterPropertyChangeConfig> array = new Array<TowerDefenseCharacterPropertyChangeConfig>(_editingCharacterOverride.propertyChange);
				int num = Mathf.Clamp(_selectedOverridePropertyChangeIndex + 1, 0, array.Count);
				array.Insert(num, towerDefenseCharacterPropertyChangeConfig);
				_selectedOverridePropertyChangeIndex = num;
				_overridePropertyBinding.SetValue(_editingCharacterOverride, "propertyChange", array, "复制角色属性变化");
				RefreshCharacterOverridePropertyList();
				BindCharacterOverrideEditing();
			}
		}
	}

	private void RemoveSelectedCharacterOverridePropertyChange()
	{
		if (GodotObject.IsInstanceValid(_editingCharacterOverride) && _editingCharacterOverride.propertyChange != null && _overridePropertyBinding != null && _selectedOverridePropertyChangeIndex >= 0 && _selectedOverridePropertyChangeIndex < _editingCharacterOverride.propertyChange.Count)
		{
			Array<TowerDefenseCharacterPropertyChangeConfig> array = new Array<TowerDefenseCharacterPropertyChangeConfig>(_editingCharacterOverride.propertyChange);
			array.RemoveAt(_selectedOverridePropertyChangeIndex);
			_selectedOverridePropertyChangeIndex = ((array.Count == 0) ? (-1) : Mathf.Min(_selectedOverridePropertyChangeIndex, array.Count - 1));
			_overridePropertyBinding.SetValue(_editingCharacterOverride, "propertyChange", array, "删除角色属性变化");
			RefreshCharacterOverridePropertyList();
			BindCharacterOverrideEditing();
		}
	}

	private void MoveSelectedCharacterOverridePropertyChange(int direction)
	{
		if (GodotObject.IsInstanceValid(_editingCharacterOverride) && _editingCharacterOverride.propertyChange != null && _overridePropertyBinding != null && _selectedOverridePropertyChangeIndex >= 0)
		{
			int num = _selectedOverridePropertyChangeIndex + Math.Sign(direction);
			if (num >= 0 && num < _editingCharacterOverride.propertyChange.Count)
			{
				Array<TowerDefenseCharacterPropertyChangeConfig> array = new Array<TowerDefenseCharacterPropertyChangeConfig>(_editingCharacterOverride.propertyChange);
				TowerDefenseCharacterPropertyChangeConfig item = array[_selectedOverridePropertyChangeIndex];
				array.RemoveAt(_selectedOverridePropertyChangeIndex);
				array.Insert(num, item);
				_selectedOverridePropertyChangeIndex = num;
				_overridePropertyBinding.SetValue(_editingCharacterOverride, "propertyChange", array, (direction < 0) ? "上移角色属性变化" : "下移角色属性变化");
				RefreshCharacterOverridePropertyList();
				BindCharacterOverrideEditing();
			}
		}
	}

	private void UpdateCharacterOverridePropertyButtons()
	{
		bool flag = GodotObject.IsInstanceValid(_editingCharacterOverride);
		int valueOrDefault = (_editingCharacterOverride?.propertyChange?.Count).GetValueOrDefault();
		bool flag2 = flag && _selectedOverridePropertyChangeIndex >= 0 && _selectedOverridePropertyChangeIndex < valueOrDefault;
		if (GodotObject.IsInstanceValid(_addOverridePropertyChangeButton))
		{
			_addOverridePropertyChangeButton.Disabled = !flag;
		}
		if (GodotObject.IsInstanceValid(_duplicateOverridePropertyChangeButton))
		{
			_duplicateOverridePropertyChangeButton.Disabled = !flag2;
		}
		if (GodotObject.IsInstanceValid(_removeOverridePropertyChangeButton))
		{
			_removeOverridePropertyChangeButton.Disabled = !flag2;
		}
		if (GodotObject.IsInstanceValid(_moveOverridePropertyChangeUpButton))
		{
			_moveOverridePropertyChangeUpButton.Disabled = !flag2 || _selectedOverridePropertyChangeIndex == 0;
		}
		if (GodotObject.IsInstanceValid(_moveOverridePropertyChangeDownButton))
		{
			_moveOverridePropertyChangeDownButton.Disabled = !flag2 || _selectedOverridePropertyChangeIndex >= valueOrDefault - 1;
		}
	}

	private void RefreshCharacterOverridePropertyList()
	{
		if (!GodotObject.IsInstanceValid(_overridePropertyChangeList))
		{
			return;
		}
		_overridePropertyChangeList.Clear();
		if (GodotObject.IsInstanceValid(_editingCharacterOverride) && _editingCharacterOverride.propertyChange != null)
		{
			for (int i = 0; i < _editingCharacterOverride.propertyChange.Count; i++)
			{
				TowerDefenseCharacterPropertyChangeConfig change = _editingCharacterOverride.propertyChange[i];
				_overridePropertyChangeList.AddItem(FormatPropertyChange(change, i));
			}
		}
		else if (GodotObject.IsInstanceValid(_editingPropertyChange))
		{
			_overridePropertyChangeList.AddItem(FormatPropertyChange(_editingPropertyChange, 0));
			_selectedOverridePropertyChangeIndex = 0;
		}
		if (_overridePropertyChangeList.ItemCount > 0)
		{
			_selectedOverridePropertyChangeIndex = Mathf.Clamp((_selectedOverridePropertyChangeIndex >= 0) ? _selectedOverridePropertyChangeIndex : 0, 0, _overridePropertyChangeList.ItemCount - 1);
			_overridePropertyChangeList.Select(_selectedOverridePropertyChangeIndex);
		}
		else
		{
			_selectedOverridePropertyChangeIndex = -1;
		}
		UpdateCharacterOverridePropertyButtons();
	}

	private static string FormatPropertyChange(TowerDefenseCharacterPropertyChangeConfig change, int index)
	{
		if (GodotObject.IsInstanceValid(change))
		{
			string value = (string.IsNullOrWhiteSpace(change.propertyName) ? "<未选择属性>" : change.propertyName);
			return $"{index + 1}. {value}  →  {change.value}";
		}
		return $"{index + 1}. <空属性变化>";
	}

	private void OpenOverridePropertyChange(long index)
	{
		TowerDefenseCharacterPropertyChangeConfig towerDefenseCharacterPropertyChangeConfig = null;
		if (GodotObject.IsInstanceValid(_editingCharacterOverride) && _editingCharacterOverride.propertyChange != null && index >= 0 && index < _editingCharacterOverride.propertyChange.Count)
		{
			towerDefenseCharacterPropertyChangeConfig = _editingCharacterOverride.propertyChange[(int)index];
		}
		else if (GodotObject.IsInstanceValid(_editingPropertyChange) && index == 0L)
		{
			towerDefenseCharacterPropertyChangeConfig = _editingPropertyChange;
		}
		if (GodotObject.IsInstanceValid(towerDefenseCharacterPropertyChangeConfig) && towerDefenseCharacterPropertyChangeConfig != CurrentResource)
		{
			XWResourceEditContext context = XWResourceEditContext.ForProperty(towerDefenseCharacterPropertyChangeConfig, _editingCharacterOverride, towerDefenseCharacterPropertyChangeConfig.ResourcePath, CurrentResourcePath, "propertyChange", (int)index, "character_editor", CurrentEditContext?.IsBuiltInSource ?? false, _overrideResolvedPacketName);
			XWEditorInterface.Instance?.EditResource(towerDefenseCharacterPropertyChangeConfig, context);
		}
	}

	private void RefreshCharacterOverrideWorkbench(bool rebuildPreview)
	{
		if (GodotObject.IsInstanceValid(_characterOverrideWorkbench))
		{
			_overridePropertyBinding?.Dispose();
			_overridePropertyBinding = null;
			PopulateCharacterOverrideControls();
			BindCharacterOverrideEditing();
			if (rebuildPreview)
			{
				RebuildCharacterOverridePreview();
			}
		}
	}

	private void RefreshCharacterOverrideSummary()
	{
		if (GodotObject.IsInstanceValid(_overrideEffectSummaryLabel))
		{
			_overrideEffectSummaryLabel.Text = (GodotObject.IsInstanceValid(_editingCharacterOverride) ? $"覆盖效果：模型 ×{FormatOverrideNumber(_editingCharacterOverride.scale)} · 生命 ×{FormatOverrideNumber(_editingCharacterOverride.hitpointScale)} · 动画 {FormatRange(_editingCharacterOverride.animeSpeedScale)}" : ("单条属性：" + (_editingPropertyChange?.propertyName ?? "<未设置>") + " → " + (_editingPropertyChange?.value.ToString() ?? "<空>")));
		}
		if (GodotObject.IsInstanceValid(_overrideSafetySummaryLabel))
		{
			int valueOrDefault = (_editingCharacterOverride?.armor?.Count).GetValueOrDefault();
			int valueOrDefault2 = (_editingCharacterOverride?.spawnEvent?.Count).GetValueOrDefault();
			int valueOrDefault3 = (_editingCharacterOverride?.dieEvent?.Count).GetValueOrDefault();
			_overrideSafetySummaryLabel.Text = $"安全预览：护甲 {valueOrDefault} · 生成事件 {valueOrDefault2} · 死亡事件 {valueOrDefault3}（事件不会在编辑器执行）";
		}
		if (GodotObject.IsInstanceValid(_overrideSaveStateLabel))
		{
			_overrideSaveStateLabel.Text = (GodotObject.IsInstanceValid(_overrideBaseCharacter) ? "游戏内对照已就绪" : "等待选择基础角色");
		}
	}

	private static string FormatOverrideNumber(double value)
	{
		if (!(value < 0.0))
		{
			return value.ToString("0.##");
		}
		return "沿用";
	}

	private static string FormatRange(Vector2 value)
	{
		if (!(value == new Vector2(-1f, -1f)))
		{
			return $"{value.X:0.##}–{value.Y:0.##}×";
		}
		return "沿用";
	}

	private void OnCharacterOverridePreviewGuiInput(InputEvent inputEvent)
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
			_overridePreviewZoom = Mathf.Clamp(_overridePreviewZoom * ((inputEventMouseButton.ButtonIndex == MouseButton.WheelUp) ? 1.1f : 0.9f), 0.35f, 3f);
			UpdateCharacterOverridePreviewTransform();
			_overrideViewportContainer.AcceptEvent();
		}
		else if (inputEvent is InputEventMouseButton inputEventMouseButton2 && inputEventMouseButton2.ButtonIndex == MouseButton.Left)
		{
			_draggingOverridePreview = inputEventMouseButton2.Pressed;
			_overrideViewportContainer.AcceptEvent();
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _draggingOverridePreview)
		{
			_overridePreviewPan += inputEventMouseMotion.Relative;
			UpdateCharacterOverridePreviewTransform();
			_overrideViewportContainer.AcceptEvent();
		}
	}

	private void ResetCharacterOverridePreviewView()
	{
		_overridePreviewPan = Vector2.Zero;
		_overridePreviewZoom = 1f;
		UpdateCharacterOverridePreviewTransform();
	}

	private void UpdateCharacterOverridePreviewTransform()
	{
		if (GodotObject.IsInstanceValid(_overridePreviewCameraRoot))
		{
			_overridePreviewCameraRoot.Position = _overridePreviewPan;
			_overridePreviewCameraRoot.Scale = Vector2.One * _overridePreviewZoom;
			if (GodotObject.IsInstanceValid(_overrideZoomLabel))
			{
				_overrideZoomLabel.Text = $"{_overridePreviewZoom:P0}";
			}
		}
	}

	private void UpdateCharacterOverrideStageLayout()
	{
		if (GodotObject.IsInstanceValid(_overrideViewport))
		{
			Vector2 size = new Vector2(_overrideViewport.Size.X, _overrideViewport.Size.Y);
			if (size.X <= 1f || size.Y <= 1f)
			{
				size = new Vector2(900f, 350f);
			}
			if (GodotObject.IsInstanceValid(_overridePreviewBackground))
			{
				_overridePreviewBackground.Size = size;
			}
			if (GodotObject.IsInstanceValid(_overrideGroundShade))
			{
				float num = size.Y * 0.73f;
				_overrideGroundShade.Position = new Vector2(0f, num);
				_overrideGroundShade.Size = new Vector2(size.X, size.Y - num);
			}
			if (GodotObject.IsInstanceValid(_overrideBaseCharacterRoot))
			{
				_overrideBaseCharacterRoot.Position = new Vector2(size.X * 0.25f, size.Y * 0.77f);
			}
			if (GodotObject.IsInstanceValid(_overriddenCharacterRoot))
			{
				_overriddenCharacterRoot.Position = new Vector2(size.X * 0.75f, size.Y * 0.77f);
			}
			UpdateCharacterOverridePreviewTransform();
		}
	}

	private static void SetPreviewRootProcessing(Node root, bool enabled)
	{
		if (GodotObject.IsInstanceValid(root))
		{
			root.ProcessMode = (ProcessModeEnum)(enabled ? 0 : 4);
		}
	}

	private static void ClearPreviewRoot(Node root)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return;
		}
		foreach (Node child in root.GetChildren())
		{
			root.RemoveChild(child);
			child.Free();
		}
	}

	private void DisposeCharacterOverrideEditor(bool disposePicker)
	{
		_overridePropertyBinding?.Dispose();
		_overridePropertyBinding = null;
		ClearPreviewRoot(_overrideBaseCharacterRoot);
		ClearPreviewRoot(_overriddenCharacterRoot);
		_overrideBaseRuntimeCharacter = null;
		_overriddenRuntimeCharacter = null;
		_overrideBaseCharacter = null;
		_overrideBasePacket = null;
		_overrideRuntimeScene = null;
		_overrideSpriteScene = null;
		_overrideResolvedPacketName = "";
		_characterOverrideWorkbench = null;
		_overrideViewport = null;
		_overrideViewportContainer = null;
		_overridePreviewBackground = null;
		_overrideGroundShade = null;
		_overridePreviewCameraRoot = null;
		_overrideBaseCharacterRoot = null;
		_overriddenCharacterRoot = null;
		_overrideContextLabel = null;
		_overrideSourceLabel = null;
		_overridePreviewStatusLabel = null;
		_overrideZoomLabel = null;
		_overrideEffectSummaryLabel = null;
		_overrideSafetySummaryLabel = null;
		_overrideSaveStateLabel = null;
		_overridePropertyChangeList = null;
		_selectOverrideBaseCharacterButton = null;
		_overrideInvisibleCheck = null;
		_overrideScaleSpinBox = null;
		_overrideHitpointScaleSpinBox = null;
		_overrideWalkSpeedMinSpinBox = null;
		_overrideWalkSpeedMaxSpinBox = null;
		_overrideAnimeSpeedMinSpinBox = null;
		_overrideAnimeSpeedMaxSpinBox = null;
		_overrideCanMowerMoveCheck = null;
		_addOverridePropertyChangeButton = null;
		_duplicateOverridePropertyChangeButton = null;
		_moveOverridePropertyChangeUpButton = null;
		_moveOverridePropertyChangeDownButton = null;
		_removeOverridePropertyChangeButton = null;
		_overridePropertyNameLineEdit = null;
		_overridePropertyValueLineEdit = null;
		_overridePropertyValueTypeLabel = null;
		_editingCharacterOverride = null;
		_editingPropertyChange = null;
		_editingOverrideResource = null;
		_draggingOverridePreview = false;
		_selectedOverridePropertyChangeIndex = -1;
		if (GodotObject.IsInstanceValid(_overrideBaseCharacterPicker))
		{
			_overrideBaseCharacterPicker.Dismiss();
			if (disposePicker)
			{
				_overrideBaseCharacterPicker.Free();
				_overrideBaseCharacterPicker = null;
			}
		}
	}

	private void RenderCharacterEditor(TowerDefenseCharacterConfig character)
	{
		if (CanvasGrid == null)
		{
			return;
		}
		CanvasGrid.Columns = 1;
		if (_editorLayoutScene == null)
		{
			_editorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _editorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(vBoxContainer))
		{
			CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
			BindCharacterPreview(vBoxContainer, character);
			if (_characterBasicFieldsScene == null)
			{
				_characterBasicFieldsScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterBasicFields.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer2 = _characterBasicFieldsScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer2))
			{
				vBoxContainer.GetNode<VBoxContainer>("%BasicHost").AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
				BindCharacterBasicFields(vBoxContainer2, character);
			}
			BindComponentPanel(vBoxContainer, character);
			BindOverrideDiffPanel(vBoxContainer, character);
			AddCharacterSummaryRows(character);
		}
	}

	private void BindCharacterPreview(VBoxContainer root, TowerDefenseCharacterConfig character)
	{
		_previewNameLabel = root.GetNode<Label>("%PreviewNameLabel");
		_previewCategoryLabel = root.GetNode<Label>("%PreviewCategoryLabel");
		_characterViewport = root.GetNode<SubViewport>("%CharacterViewport");
		_characterViewportContainer = root.GetNode<SubViewportContainer>("%ViewportContainer");
		_characterPreviewBackground = root.GetNode<TextureRect>("%GameBackground");
		_characterGroundShade = root.GetNode<ColorRect>("%GroundShade");
		_characterPreviewRoot = root.GetNode<Node2D>("%PreviewRoot");
		_previewStatusLabel = root.GetNode<Label>("%PreviewStatusLabel");
		_previewHealthSlider = root.GetNode<HSlider>("%HealthSlider");
		_previewHealthValueLabel = root.GetNode<Label>("%HealthValueLabel");
		_previewHitpointsLabel = root.GetNode<Label>("%HitpointsLabel");
		_previewPacketLabel = root.GetNode<Label>("%PacketLabel");
		_previewModeLabel = root.GetNode<Label>("%ModeLabel");
		_previewResourceLabel = root.GetNode<Label>("%ResourceLabel");
		_characterViewportContainer.GuiInput += OnCharacterPreviewGuiInput;
		_characterViewportContainer.Resized += UpdateCharacterPreviewStageLayout;
		_previewHealthSlider.ValueChanged += OnPreviewHealthChanged;
		root.GetNode<Button>("%ResetViewButton").Pressed += ResetCharacterPreviewView;
		RebuildCharacterPreview();
		UpdateCharacterPreview();
		CallDeferred("UpdateCharacterPreviewStageLayout");
	}

	private void BindCharacterBasicFields(VBoxContainer root, TowerDefenseCharacterConfig character)
	{
		if (!GodotObject.IsInstanceValid(root) || character == null)
		{
			return;
		}
		_updatingControls = true;
		try
		{
			BindLineEdit(root.GetNode<LineEdit>("%CharacterNameLineEdit"), character.name, (string text) =>
			{
				OnCharacterFieldChanged("name", Variant.From(in text));
			});
			OptionButton node = root.GetNode<OptionButton>("%CharacterCategoryOption");
			BindStringOption(node, ReadString(character, "category", GetCharacterCategory(character)), CharacterCategories, (string value) =>
			{
				OnCharacterDiscreteFieldChanged("category", Variant.From(in value), refreshPreview: false);
			});
			MountSmallSegmentedOption(node, root.GetNode<HFlowContainer>("%CharacterCategorySegments"));
			OptionButton node2 = root.GetNode<OptionButton>("%CreationModeOption");
			BindStringOption(node2, ReadString(character, "creationMode", "空白创建"), CreationModes, (string value) =>
			{
				OnCharacterDiscreteFieldChanged("creationMode", Variant.From(in value), refreshPreview: false);
			});
			MountSmallSegmentedOption(node2, root.GetNode<HFlowContainer>("%CreationModeSegments"));
			BindLineEdit(root.GetNode<LineEdit>("%BaseCharacterKeyLineEdit"), ReadString(character, "baseCharacterKey"), (string text) =>
			{
				OnCharacterFieldChanged("baseCharacterKey", Variant.From(in text), refreshPreview: false);
			});
			BindLineEdit(root.GetNode<LineEdit>("%TargetCharacterKeyLineEdit"), ReadString(character, "targetCharacterKey"), (string text) =>
			{
				OnCharacterFieldChanged("targetCharacterKey", Variant.From(in text), refreshPreview: false);
			});
			BindCharacterKeyCatalog(root, "%BaseCharacterKeyLineEdit", "%BaseCharacterSelectButton", "baseCharacterKey");
			BindCharacterKeyCatalog(root, "%TargetCharacterKeyLineEdit", "%TargetCharacterSelectButton", "targetCharacterKey");
			BindResourcePicker(root.GetNode<XWResourcePicker>("%SceneOverridePicker"), "PackedScene", ReadResource(character, "sceneOverride"), (Resource resource) =>
			{
				OnCharacterDiscreteFieldChanged("sceneOverride", Variant.From(in resource), refreshPreview: true, rebuildPreview: true);
			});
			BindResourcePicker(root.GetNode<XWResourcePicker>("%SpriteOverridePicker"), "Texture2D", ReadResource(character, "spriteOverride"), (Resource resource) =>
			{
				OnCharacterDiscreteFieldChanged("spriteOverride", Variant.From(in resource), refreshPreview: true, rebuildPreview: true);
			});
			BindResourcePicker(root.GetNode<XWResourcePicker>("%PacketPatchPicker"), "Resource", ReadResource(character, "packetPatch"), (Resource resource) =>
			{
				OnCharacterDiscreteFieldChanged("packetPatch", Variant.From(in resource), refreshPreview: false);
			});
			BindResourcePicker(root.GetNode<XWResourcePicker>("%CharacterConfigPatchPicker"), "Resource", ReadResource(character, "characterConfigPatch"), (Resource resource) =>
			{
				OnCharacterDiscreteFieldChanged("characterConfigPatch", Variant.From(in resource), refreshPreview: false);
			});
			BindCharacterResourceCatalog(root, "%SceneOverrideCatalogButton", "sceneOverride", "PackedScene", "角色场景", "res://addons/ModEditor/Icons/PackedScene.svg", rebuildPreview: true);
			BindCharacterResourceCatalog(root, "%SpriteOverrideCatalogButton", "spriteOverride", "Texture2D", "预览贴图", "res://addons/ModEditor/Icons/ClassIcon/Image.svg", rebuildPreview: true);
			BindCharacterResourceCatalog(root, "%PacketPatchCatalogButton", "packetPatch", "Resource", "卡牌补丁", "res://addons/ModEditor/Icons/ResourceCard.svg", rebuildPreview: false);
			BindCharacterResourceCatalog(root, "%CharacterConfigPatchCatalogButton", "characterConfigPatch", "Resource", "角色配置补丁", "res://addons/ModEditor/Icons/ResourceCharacter.svg", rebuildPreview: false);
			BindSpinBox(root.GetNode<SpinBox>("%HitpointsSpinBox"), character.hitpoints, (double value) =>
			{
				OnCharacterFieldChanged("hitpoints", Variant.From(in value));
			});
			BindSpinBox(root.GetNode<SpinBox>("%NearDeathSpinBox"), character.hitpointsNearDeath, (double value) =>
			{
				OnCharacterFieldChanged("hitpointsNearDeath", Variant.From(in value));
			});
			BindSpinBox(root.GetNode<SpinBox>("%CostSpinBox"), character.cost, (double value) =>
			{
				OnCharacterFieldChanged("cost", Variant.From<int>((int)Math.Round(value)));
			});
			BindSpinBox(root.GetNode<SpinBox>("%CooldownSpinBox"), character.packetCooldown, (double value) =>
			{
				OnCharacterFieldChanged("packetCooldown", Variant.From(in value));
			});
			BindSpinBox(root.GetNode<SpinBox>("%StartingCooldownSpinBox"), character.startingCooldown, (double value) =>
			{
				OnCharacterFieldChanged("startingCooldown", Variant.From(in value));
			});
			BindToggle(root.GetNode<BaseButton>("%CanImitateCheck"), character.canImitate, (bool value) =>
			{
				OnCharacterFieldChanged("canImitate", Variant.From(in value));
			});
			BindToggle(root.GetNode<BaseButton>("%CanCopyCheck"), character.canCopy, (bool value) =>
			{
				OnCharacterFieldChanged("canCopy", Variant.From(in value));
			});
			BindToggle(root.GetNode<BaseButton>("%CanDragIntoWaterCheck"), character.canDragIntoWater, (bool value) =>
			{
				OnCharacterFieldChanged("canDragIntoWater", Variant.From(in value));
			});
			BindTypedCharacterFields(root, character);
			SetTypedCharacterFieldVisibility(root, character);
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void BindComponentPanel(VBoxContainer root, TowerDefenseCharacterConfig character)
	{
		_componentListView = root.GetNode<ItemList>("%ComponentList");
		_componentPatchListView = root.GetNode<ItemList>("%ComponentPatchList");
		_spawnEventListView = root.GetNode<ItemList>("%SpawnEventList");
		_dieEventListView = root.GetNode<ItemList>("%DieEventList");
		_propertyChangeListView = root.GetNode<ItemList>("%PropertyChangeList");
		RefreshComponentPanel(character);
	}

	private void BindOverrideDiffPanel(VBoxContainer root, TowerDefenseCharacterConfig character)
	{
		_overrideDiffTree = root.GetNode<Tree>("%OverrideDiffTree");
		_overrideDiffTree.SetColumnTitle(0, "属性");
		_overrideDiffTree.SetColumnTitle(1, "原版值");
		_overrideDiffTree.SetColumnTitle(2, "Mod 值");
		_overrideDiffTree.SetColumnTitle(3, "覆盖");
		RefreshOverrideDiffPanel(character);
	}

	private void BindTypedCharacterFields(VBoxContainer root, TowerDefenseCharacterConfig character)
	{
		if (!(character is TowerDefensePlantConfig towerDefensePlantConfig))
		{
			if (!(character is TowerDefenseZombieConfig towerDefenseZombieConfig))
			{
				if (!(character is TowerDefenseMowerConfig towerDefenseMowerConfig))
				{
					if (!(character is TowerDefenseVaseConfig towerDefenseVaseConfig))
					{
						if (character is TowerDefenseItemConfig towerDefenseItemConfig)
						{
							BindToggle(root.GetNode<BaseButton>("%IsLadderCheck"), towerDefenseItemConfig.isLadder, (bool value) =>
							{
								OnCharacterFieldChanged("isLadder", Variant.From(in value));
							});
							BindToggle(root.GetNode<BaseButton>("%IsShieldCheck"), towerDefenseItemConfig.isShield, (bool value) =>
							{
								OnCharacterFieldChanged("isShield", Variant.From(in value));
							});
						}
					}
					else
					{
						OptionButton node = root.GetNode<OptionButton>("%VaseTypeOption");
						BindEnumOption(node, towerDefenseVaseConfig.type, (TowerDefenseEnum.VASE_TYPE value) =>
						{
							int from = (int)value;
							OnCharacterDiscreteFieldChanged("type", Variant.From(in from));
						});
						MountSmallSegmentedOption(node, root.GetNode<HFlowContainer>("%VaseTypeSegments"));
					}
				}
				else
				{
					BindResourcePicker(root.GetNode<XWResourcePicker>("%MowerConfigPicker"), "Resource", towerDefenseMowerConfig.mowerConfig, (Resource resource) =>
					{
						OnCharacterDiscreteFieldChanged("mowerConfig", Variant.From(in resource), refreshPreview: false);
					});
				}
			}
			else
			{
				BindSpinBox(root.GetNode<SpinBox>("%AttackSpinBox"), towerDefenseZombieConfig.attack, (double value) =>
				{
					OnCharacterFieldChanged("attack", Variant.From(in value));
				});
				BindSpinBox(root.GetNode<SpinBox>("%SmashAttackSpinBox"), towerDefenseZombieConfig.smashAttack, (double value) =>
				{
					OnCharacterFieldChanged("smashAttack", Variant.From(in value));
				});
				BindSpinBox(root.GetNode<SpinBox>("%WeightSpinBox"), towerDefenseZombieConfig.weight, (double value) =>
				{
					OnCharacterFieldChanged("weight", Variant.From<int>((int)Math.Round(value)));
				});
				BindSpinBox(root.GetNode<SpinBox>("%WavePointCostSpinBox"), towerDefenseZombieConfig.wavePointCost, (double value) =>
				{
					OnCharacterFieldChanged("wavePointCost", Variant.From<int>((int)Math.Round(value)));
				});
				BindToggle(root.GetNode<BaseButton>("%ZombiePreviewCheck"), towerDefenseZombieConfig.preview, (bool value) =>
				{
					OnCharacterFieldChanged("preview", Variant.From(in value));
				});
			}
		}
		else
		{
			BindToggle(root.GetNode<BaseButton>("%CanUsePlantfoodCheck"), towerDefensePlantConfig.canUsePlantfood, (bool value) =>
			{
				OnCharacterFieldChanged("canUsePlantfood", Variant.From(in value));
			});
			BindToggle(root.GetNode<BaseButton>("%Izn2FilterCheck"), towerDefensePlantConfig.izm2Fliter, (bool value) =>
			{
				OnCharacterFieldChanged("izm2Fliter", Variant.From(in value));
			});
			BindToggle(root.GetNode<BaseButton>("%PlantCoverAllCheck"), towerDefensePlantConfig.plantCoverAll, (bool value) =>
			{
				OnCharacterFieldChanged("plantCoverAll", Variant.From(in value));
			});
			BindToggle(root.GetNode<BaseButton>("%PlantCoverSelfCheck"), towerDefensePlantConfig.plantCoverSelf, (bool value) =>
			{
				OnCharacterFieldChanged("plantCoverSelf", Variant.From(in value));
			});
		}
	}

	private static void SetTypedCharacterFieldVisibility(VBoxContainer root, TowerDefenseCharacterConfig character)
	{
		root.GetNode<PanelContainer>("%PlantFields").Visible = character is TowerDefensePlantConfig;
		root.GetNode<PanelContainer>("%ZombieFields").Visible = character is TowerDefenseZombieConfig;
		root.GetNode<PanelContainer>("%MowerFields").Visible = character is TowerDefenseMowerConfig;
		root.GetNode<PanelContainer>("%VaseFields").Visible = character is TowerDefenseVaseConfig;
		root.GetNode<PanelContainer>("%ItemFields").Visible = character is TowerDefenseItemConfig;
	}

	private void RebuildCharacterPreview()
	{
		if (!GodotObject.IsInstanceValid(_characterPreviewRoot) || _editingCharacter == null)
		{
			return;
		}
		foreach (Node child in _characterPreviewRoot.GetChildren())
		{
			child.QueueFree();
		}
		_runtimeCharacterPreview = null;
		_characterPreviewUsesExternalScriptFallback = false;
		Node node = BuildRuntimeCharacterPreviewScene(_editingCharacter);
		if (node != null)
		{
			node.Name = "ConfiguredCharacterGamePreview";
			_characterPreviewRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			_runtimeCharacterPreview = FindRuntimeCharacter(node);
			ApplyCharacterPreviewState();
			if (GodotObject.IsInstanceValid(_previewStatusLabel))
			{
				_previewStatusLabel.Text = "完整游戏角色场景 · 拖拽移动 · 滚轮缩放";
			}
			return;
		}
		Node node2 = BuildSpriteFallbackPreview(_editingCharacter);
		if (node2 != null)
		{
			node2.Name = "CharacterSpriteFallbackPreview";
			_characterPreviewRoot.AddChild(node2, forceReadableName: false, InternalMode.Disabled);
			if (GodotObject.IsInstanceValid(_previewStatusLabel))
			{
				_previewStatusLabel.Text = (_characterPreviewUsesExternalScriptFallback ? "外部 C# 尚未注册到 Godot，当前使用同包游戏动画安全预览" : "未找到完整角色场景，当前为动画资源降级预览");
			}
			return;
		}
		ColorRect node3 = new ColorRect
		{
			Name = "CharacterFallbackPreview",
			Color = GetCategoryColor(GetCharacterCategory(_editingCharacter)),
			CustomMinimumSize = new Vector2(96f, 132f),
			Position = new Vector2(-48f, -106f)
		};
		_characterPreviewRoot.AddChild(node3, forceReadableName: false, InternalMode.Disabled);
		_characterPreviewRoot.AddChild(new Label
		{
			Name = "CharacterFallbackLabel",
			Text = EmptyToPlaceholder(_editingCharacter.name),
			Position = new Vector2(-80f, 34f),
			HorizontalAlignment = HorizontalAlignment.Center,
			CustomMinimumSize = new Vector2(160f, 24f)
		}, forceReadableName: false, InternalMode.Disabled);
		if (GodotObject.IsInstanceValid(_previewStatusLabel))
		{
			_previewStatusLabel.Text = "未找到角色场景或动画资源";
		}
	}

	private Node BuildRuntimeCharacterPreviewScene(TowerDefenseCharacterConfig character)
	{
		if (character == null)
		{
			return null;
		}
		if (!TryResolveCharacterRuntimeScene(character, out var characterScene))
		{
			return null;
		}
		if (HasExternalCSharpSceneScript(characterScene.ResourcePath))
		{
			_characterPreviewUsesExternalScriptFallback = true;
			return null;
		}
		try
		{
			Node node = characterScene.Instantiate(PackedScene.GenEditState.Disabled);
			XWRuntimePreviewSceneAdapter.PrepareAnimationSprites(node);
			TowerDefenseCharacter towerDefenseCharacter = FindRuntimeCharacter(node);
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				node.QueueFree();
				return null;
			}
			towerDefenseCharacter.inGame = false;
			towerDefenseCharacter.editorPreviewMode = true;
			towerDefenseCharacter.config = character;
			return node;
		}
		catch (Exception ex)
		{
			GD.PushWarning("Character editor runtime scene preview failed: " + ex.Message);
			return null;
		}
	}

	private static bool HasExternalCSharpSceneScript(string scenePath)
	{
		string text = ToAbsoluteCharacterPreviewFilePath(scenePath);
		if (string.IsNullOrWhiteSpace(text) || !File.Exists(text) || !string.Equals(Path.GetExtension(text), ".tscn", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		foreach (string item in File.ReadLines(text))
		{
			string text2 = item.Trim();
			if (!text2.StartsWith("[ext_resource", StringComparison.OrdinalIgnoreCase) || !text2.Contains("type=\"Script\"", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			int num = text2.IndexOf("path=\"", StringComparison.OrdinalIgnoreCase);
			if (num < 0)
			{
				continue;
			}
			num += "path=\"".Length;
			int num2 = text2.IndexOf('"', num);
			if (num2 > num)
			{
				string text3 = text2.Substring(num, num2 - num).Trim();
				if (text3.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) && !text3.StartsWith("res://", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		return false;
	}

	private Node BuildSpriteFallbackPreview(TowerDefenseCharacterConfig character)
	{
		if (TryResolveCharacterPreviewSpriteScene(character, out var spriteScene))
		{
			return InstantiateCharacterSpriteFallback(spriteScene);
		}
		if (ReadResource(character, "spriteOverride") is Texture2D texture)
		{
			return new Sprite2D
			{
				Texture = texture,
				Centered = true
			};
		}
		return null;
	}

	private bool TryResolveCharacterRuntimeScene(TowerDefenseCharacterConfig character, out PackedScene characterScene)
	{
		characterScene = ReadResource(character, "sceneOverride") as PackedScene;
		if (GodotObject.IsInstanceValid(characterScene))
		{
			return true;
		}
		if (!string.IsNullOrWhiteSpace(character?.name) && ResourceManager.Instance != null)
		{
			characterScene = ResourceManager.Instance.GetCharacterScene(character.name);
			if (GodotObject.IsInstanceValid(characterScene))
			{
				return true;
			}
		}
		string[] array = new string[2]
		{
			CurrentResourcePath,
			character?.ResourcePath
		};
		for (int i = 0; i < array.Length; i++)
		{
			string characterPackageDirectoryForPreview = GetCharacterPackageDirectoryForPreview(array[i]);
			if (string.IsNullOrWhiteSpace(characterPackageDirectoryForPreview))
			{
				continue;
			}
			foreach (string characterRuntimeSceneCandidate in GetCharacterRuntimeSceneCandidates(NormalizeCharacterPreviewPath(characterPackageDirectoryForPreview.PathJoin("Scene")), character?.name))
			{
				if (ResourceLoader.Exists(characterRuntimeSceneCandidate))
				{
					characterScene = ResourceLoader.Load<PackedScene>(characterRuntimeSceneCandidate, null, ResourceLoader.CacheMode.Reuse);
					if (GodotObject.IsInstanceValid(characterScene))
					{
						return true;
					}
				}
			}
		}
		characterScene = null;
		return false;
	}

	private static IEnumerable<string> GetCharacterRuntimeSceneCandidates(string sceneDir, string characterName)
	{
		if (string.IsNullOrWhiteSpace(sceneDir) || !DirAccess.DirExistsAbsolute(sceneDir))
		{
			yield break;
		}
		if (!string.IsNullOrWhiteSpace(characterName))
		{
			yield return NormalizeCharacterPreviewPath(sceneDir.PathJoin(characterName + ".tscn"));
			yield return NormalizeCharacterPreviewPath(sceneDir.PathJoin("TowerDefense" + characterName + ".tscn"));
		}
		string[] filesAt = DirAccess.GetFilesAt(sceneDir);
		foreach (string text in filesAt)
		{
			if (string.Equals(text.GetExtension(), "tscn", StringComparison.OrdinalIgnoreCase))
			{
				yield return NormalizeCharacterPreviewPath(sceneDir.PathJoin(text));
			}
		}
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

	private bool TryResolveCharacterPreviewSpriteScene(TowerDefenseCharacterConfig character, out PackedScene spriteScene)
	{
		spriteScene = null;
		if (TryResolveExplicitCharacterSpritePreviewScene(character, out spriteScene))
		{
			return true;
		}
		if (ReadResource(character, "sceneOverride") is PackedScene packedScene && TryResolveCharacterSceneMetadataSpriteScene(packedScene.ResourcePath, out spriteScene))
		{
			return true;
		}
		string text = FindCharacterPackagePreviewScene(character);
		if (!string.IsNullOrWhiteSpace(text) && ResourceLoader.Exists(text))
		{
			spriteScene = ResourceLoader.Load<PackedScene>(text, null, ResourceLoader.CacheMode.Reuse);
			return GodotObject.IsInstanceValid(spriteScene);
		}
		return false;
	}

	private bool TryResolveExplicitCharacterSpritePreviewScene(TowerDefenseCharacterConfig character, out PackedScene spriteScene)
	{
		spriteScene = null;
		if (!TryGetPackedScenePath(ReadResource(character, "spriteOverride"), out var path))
		{
			return false;
		}
		if (!IsCharacterPreviewSpriteScenePath(path))
		{
			GD.PushWarning("Character editor ignored non-sprite preview scene: " + path);
			return false;
		}
		if (!ResourceLoader.Exists(path))
		{
			return false;
		}
		spriteScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Reuse);
		return GodotObject.IsInstanceValid(spriteScene);
	}

	private Node InstantiateCharacterSpriteFallback(PackedScene spriteScene)
	{
		if (!GodotObject.IsInstanceValid(spriteScene))
		{
			return null;
		}
		try
		{
			Node node = spriteScene.Instantiate(PackedScene.GenEditState.Disabled);
			DisableCharacterFallbackGameplayProcessing(node);
			return node;
		}
		catch (Exception ex)
		{
			GD.PushWarning("Character editor safe sprite preview failed: " + ex.Message);
			return null;
		}
	}

	private static void DisableCharacterFallbackGameplayProcessing(Node node)
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
			DisableCharacterFallbackGameplayProcessing(child);
		}
	}

	private void OnPreviewHealthChanged(double value)
	{
		ApplyCharacterPreviewState();
	}

	private void ApplyCharacterPreviewState()
	{
		double num = (GodotObject.IsInstanceValid(_previewHealthSlider) ? Mathf.Clamp(_previewHealthSlider.Value / 100.0, 0.0, 1.0) : 1.0);
		if (GodotObject.IsInstanceValid(_previewHealthValueLabel))
		{
			_previewHealthValueLabel.Text = $"{num:P0}";
		}
		if (GodotObject.IsInstanceValid(_runtimeCharacterPreview))
		{
			_runtimeCharacterPreview.previewDamagePointPersontage = num;
		}
	}

	private void OnCharacterPreviewGuiInput(InputEvent inputEvent)
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
			_characterPreviewZoom = Mathf.Clamp(_characterPreviewZoom * ((inputEventMouseButton.ButtonIndex == MouseButton.WheelUp) ? 1.1f : 0.9f), 0.35f, 3f);
			UpdateCharacterPreviewTransform();
			_characterViewportContainer.AcceptEvent();
		}
		else if (inputEvent is InputEventMouseButton inputEventMouseButton2 && inputEventMouseButton2.ButtonIndex == MouseButton.Left)
		{
			_draggingCharacterPreview = inputEventMouseButton2.Pressed;
			_characterViewportContainer.AcceptEvent();
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _draggingCharacterPreview)
		{
			_characterPreviewPan += inputEventMouseMotion.Relative;
			UpdateCharacterPreviewTransform();
			_characterViewportContainer.AcceptEvent();
		}
	}

	private void ResetCharacterPreviewView()
	{
		_characterPreviewPan = Vector2.Zero;
		_characterPreviewZoom = 1f;
		UpdateCharacterPreviewTransform();
	}

	private void UpdateCharacterPreviewTransform()
	{
		if (GodotObject.IsInstanceValid(_characterPreviewRoot))
		{
			Vector2 vector = (GodotObject.IsInstanceValid(_characterViewport) ? new Vector2(_characterViewport.Size.X, _characterViewport.Size.Y) : new Vector2(720f, 340f));
			_characterPreviewRoot.Position = new Vector2(vector.X * 0.5f, vector.Y * 0.62f) + _characterPreviewPan;
			_characterPreviewRoot.Scale = Vector2.One * _characterPreviewZoom;
		}
	}

	private void UpdateCharacterPreviewStageLayout()
	{
		if (GodotObject.IsInstanceValid(_characterViewport))
		{
			Vector2 size = new Vector2(_characterViewport.Size.X, _characterViewport.Size.Y);
			if (size.X <= 1f || size.Y <= 1f)
			{
				size = new Vector2(720f, 340f);
			}
			if (GodotObject.IsInstanceValid(_characterPreviewBackground))
			{
				_characterPreviewBackground.Size = size;
			}
			if (GodotObject.IsInstanceValid(_characterGroundShade))
			{
				float num = size.Y * 0.72f;
				_characterGroundShade.Position = new Vector2(0f, num);
				_characterGroundShade.Size = new Vector2(size.X, size.Y - num);
			}
			UpdateCharacterPreviewTransform();
		}
	}

	private bool TryResolveCharacterSceneMetadataSpriteScene(string scenePath, out PackedScene spriteScene)
	{
		spriteScene = null;
		scenePath = NormalizeCharacterPreviewPath(scenePath);
		if (string.IsNullOrWhiteSpace(scenePath))
		{
			return false;
		}
		if (!TryReadCharacterSceneMetadataSpritePath(scenePath, out var spritePath))
		{
			return false;
		}
		spritePath = ResolveRelativeCharacterPath(scenePath, spritePath);
		if (string.IsNullOrWhiteSpace(spritePath) || !IsCharacterPreviewSpriteScenePath(spritePath) || !ResourceLoader.Exists(spritePath))
		{
			return false;
		}
		spriteScene = ResourceLoader.Load<PackedScene>(spritePath, null, ResourceLoader.CacheMode.Reuse);
		return GodotObject.IsInstanceValid(spriteScene);
	}

	private static bool TryReadCharacterSceneMetadataSpritePath(string scenePath, out string spritePath)
	{
		spritePath = "";
		string text = ToAbsoluteCharacterPreviewFilePath(scenePath);
		if (string.IsNullOrWhiteSpace(text) || !File.Exists(text))
		{
			return false;
		}
		foreach (string item in File.ReadLines(text))
		{
			int num = item.IndexOf("metadata/mod_character_sprite_scene", StringComparison.OrdinalIgnoreCase);
			if (num >= 0)
			{
				int num2 = item.IndexOf('"', num + "metadata/mod_character_sprite_scene".Length);
				int num3 = ((num2 < 0) ? (-1) : item.IndexOf('"', num2 + 1));
				if (num2 >= 0 && num3 > num2)
				{
					spritePath = item.Substring(num2 + 1, num3 - num2 - 1);
					return !string.IsNullOrWhiteSpace(spritePath);
				}
			}
		}
		return false;
	}

	private string FindCharacterPackagePreviewScene(TowerDefenseCharacterConfig character)
	{
		string[] array = new string[3]
		{
			CurrentResourcePath,
			character?.ResourcePath,
			(ReadResource(character, "sceneOverride") as PackedScene)?.ResourcePath
		};
		for (int i = 0; i < array.Length; i++)
		{
			string characterPackageDirectoryForPreview = GetCharacterPackageDirectoryForPreview(array[i]);
			if (string.IsNullOrWhiteSpace(characterPackageDirectoryForPreview))
			{
				continue;
			}
			string file = characterPackageDirectoryForPreview.GetFile();
			string[] array2 = new string[2]
			{
				character?.name,
				file
			};
			foreach (string text in array2)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					string text2 = NormalizeCharacterPreviewPath(characterPackageDirectoryForPreview.PathJoin("Sprite").PathJoin(text + ".tscn"));
					if (ResourceLoader.Exists(text2))
					{
						return text2;
					}
				}
			}
		}
		return "";
	}

	private static bool TryGetPackedScenePath(Resource resource, out string path)
	{
		path = "";
		if (!(resource is PackedScene packedScene) || !GodotObject.IsInstanceValid(packedScene))
		{
			return false;
		}
		path = NormalizeCharacterPreviewPath(packedScene.ResourcePath);
		return !string.IsNullOrWhiteSpace(path);
	}

	private static bool IsCharacterPreviewSpriteScenePath(string path)
	{
		path = NormalizeCharacterPreviewPath(path);
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		if (path.Contains("/Resources/Characters/", StringComparison.OrdinalIgnoreCase) && path.Contains("/Sprite/", StringComparison.OrdinalIgnoreCase))
		{
			return string.Equals(path.GetExtension(), "tscn", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static string GetCharacterPackageDirectoryForPreview(string ownerPath)
	{
		ownerPath = NormalizeCharacterPreviewPath(ownerPath);
		if (string.IsNullOrWhiteSpace(ownerPath))
		{
			return "";
		}
		string baseDir = ownerPath.GetBaseDir();
		string file = baseDir.GetFile();
		if (string.Equals(file, "Config", StringComparison.OrdinalIgnoreCase) || string.Equals(file, "Scene", StringComparison.OrdinalIgnoreCase) || string.Equals(file, "Script", StringComparison.OrdinalIgnoreCase) || string.Equals(file, "Sprite", StringComparison.OrdinalIgnoreCase))
		{
			return baseDir.GetBaseDir();
		}
		if (ownerPath.EndsWith("/Config", StringComparison.OrdinalIgnoreCase) || ownerPath.EndsWith("/Scene", StringComparison.OrdinalIgnoreCase) || ownerPath.EndsWith("/Script", StringComparison.OrdinalIgnoreCase) || ownerPath.EndsWith("/Sprite", StringComparison.OrdinalIgnoreCase))
		{
			return ownerPath.GetBaseDir();
		}
		return baseDir;
	}

	private static string ResolveRelativeCharacterPath(string ownerPath, string path)
	{
		path = NormalizeCharacterPreviewPath(path);
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("user://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("uid://", StringComparison.OrdinalIgnoreCase) || Path.IsPathRooted(path))
		{
			return path;
		}
		ownerPath = NormalizeCharacterPreviewPath(ownerPath);
		if (string.IsNullOrWhiteSpace(ownerPath))
		{
			return path;
		}
		if (Path.IsPathRooted(ownerPath))
		{
			try
			{
				return NormalizeCharacterPreviewPath(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(ownerPath) ?? "", path)));
			}
			catch
			{
				return NormalizeCharacterPreviewPath(ownerPath.GetBaseDir().PathJoin(path));
			}
		}
		return NormalizeCharacterPreviewPath(ownerPath.GetBaseDir().PathJoin(path));
	}

	private static string ToAbsoluteCharacterPreviewFilePath(string path)
	{
		path = NormalizeCharacterPreviewPath(path);
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			return ProjectSettings.GlobalizePath(path);
		}
		return path;
	}

	private static string NormalizeCharacterPreviewPath(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return path.Trim().Replace('\\', '/');
		}
		return "";
	}

	private void OnCharacterFieldChanged(string propertyName, Variant value, bool refreshPreview = true, bool rebuildPreview = false)
	{
		if (_updatingControls || !(CurrentResource is TowerDefenseCharacterConfig resource) || string.IsNullOrWhiteSpace(propertyName))
		{
			return;
		}
		Variant variant = ReadVariant(resource, propertyName);
		if (!variant.Equals(value))
		{
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager == null)
			{
				ApplyCharacterDiscreteFieldFromHistory(propertyName, value, refreshPreview, rebuildPreview);
				return;
			}
			xWUndoRedoManager.CreateAction("修改角色 " + propertyName, mergeMode: true);
			xWUndoRedoManager.AddDoMethod(this, "ApplyCharacterDiscreteFieldFromHistory", propertyName, value, refreshPreview, rebuildPreview);
			xWUndoRedoManager.AddDoMethod(this, "RefreshCharacterDiscreteSurfaceFromHistory");
			xWUndoRedoManager.AddUndoMethod(this, "ApplyCharacterDiscreteFieldFromHistory", propertyName, variant, refreshPreview, rebuildPreview);
			xWUndoRedoManager.AddUndoMethod(this, "RefreshCharacterDiscreteSurfaceFromHistory");
			xWUndoRedoManager.CommitAction();
		}
	}

	private void OnCharacterDiscreteFieldChanged(string propertyName, Variant value, bool refreshPreview = true, bool rebuildPreview = false)
	{
		if (_updatingControls || !(CurrentResource is TowerDefenseCharacterConfig resource) || string.IsNullOrWhiteSpace(propertyName))
		{
			return;
		}
		Variant variant = ReadVariant(resource, propertyName);
		if (!variant.Equals(value))
		{
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager == null)
			{
				ApplyCharacterDiscreteFieldFromHistory(propertyName, value, refreshPreview, rebuildPreview);
				return;
			}
			xWUndoRedoManager.CreateAction("修改角色 " + propertyName);
			xWUndoRedoManager.AddDoMethod(this, "ApplyCharacterDiscreteFieldFromHistory", propertyName, value, refreshPreview, rebuildPreview);
			xWUndoRedoManager.AddDoMethod(this, "RefreshCharacterDiscreteSurfaceFromHistory");
			xWUndoRedoManager.AddUndoMethod(this, "ApplyCharacterDiscreteFieldFromHistory", propertyName, variant, refreshPreview, rebuildPreview);
			xWUndoRedoManager.AddUndoMethod(this, "RefreshCharacterDiscreteSurfaceFromHistory");
			xWUndoRedoManager.CommitAction();
		}
	}

	public void ApplyCharacterDiscreteFieldFromHistory(string propertyName, Variant value, bool refreshPreview, bool rebuildPreview)
	{
		SetResourceProperty(propertyName, value);
		UpdateCharacterPreview();
		RefreshComponentPanel();
		RefreshOverrideDiffPanel();
		if (rebuildPreview)
		{
			RebuildCharacterPreview();
			return;
		}
		bool flag = refreshPreview;
		if (flag)
		{
			bool flag2 = ((propertyName == "category" || propertyName == "name") ? true : false);
			flag = flag2;
		}
		if (flag)
		{
			RebuildCharacterPreview();
		}
	}

	public void RefreshCharacterDiscreteSurfaceFromHistory()
	{
		if (GodotObject.IsInstanceValid(CurrentResource) && CurrentDescriptor != null)
		{
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()))
			{
				LoadResource(CurrentResource, CurrentResourcePath, CurrentDescriptor, CurrentEditContext);
			}
		}
	}

	private void SetResourceProperty(string propertyName, Variant value)
	{
		if (!(CurrentResource is TowerDefenseCharacterConfig towerDefenseCharacterConfig) || string.IsNullOrWhiteSpace(propertyName))
		{
			return;
		}
		if (!(propertyName == "mowerConfig"))
		{
			if (!(propertyName == "type") || !(towerDefenseCharacterConfig is TowerDefenseVaseConfig towerDefenseVaseConfig))
			{
				goto IL_006d;
			}
			towerDefenseVaseConfig.type = (TowerDefenseEnum.VASE_TYPE)value.AsInt32();
		}
		else
		{
			if (!(towerDefenseCharacterConfig is TowerDefenseMowerConfig towerDefenseMowerConfig))
			{
				goto IL_006d;
			}
			towerDefenseMowerConfig.mowerConfig = value.AsGodotObject() as MowerConfig;
		}
		goto IL_0075;
		IL_006d:
		SetFlexibleProperty(towerDefenseCharacterConfig, propertyName, value);
		goto IL_0075;
		IL_0075:
		towerDefenseCharacterConfig.EmitChanged();
		towerDefenseCharacterConfig.NotifyPropertyListChanged();
		if (GodotObject.IsInstanceValid(_characterSaveTimer))
		{
			_characterSaveTimer.Start();
		}
	}

	private void SavePendingCharacterResource()
	{
		if (GodotObject.IsInstanceValid(_editingCharacter))
		{
			SaveCharacterResource(_editingCharacter);
		}
	}

	private void SaveCharacterResource(TowerDefenseCharacterConfig character)
	{
		string currentResourcePath = CurrentResourcePath;
		if (string.IsNullOrWhiteSpace(currentResourcePath) || !GodotObject.IsInstanceValid(character))
		{
			return;
		}
		Error error = ResourceSaver.Save(character, currentResourcePath, ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			GD.PushWarning($"Character editor save failed: {error} {currentResourcePath}");
		}
		else
		{
			string text = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
			if (!string.IsNullOrWhiteSpace(text))
			{
				XWModProjectLayout.MakeSavedTextResourceReferencesPortable(currentResourcePath, text);
				XWModManifestSyncService.RegisterPath(text, currentResourcePath);
			}
		}
	}

	private void UpdateCharacterPreview()
	{
		if (_editingCharacter == null)
		{
			return;
		}
		_updatingControls = true;
		try
		{
			if (GodotObject.IsInstanceValid(_previewNameLabel))
			{
				_previewNameLabel.Text = BuildNameText(_editingCharacter);
			}
			if (GodotObject.IsInstanceValid(_previewCategoryLabel))
			{
				_previewCategoryLabel.Text = BuildCategoryText(_editingCharacter);
			}
			if (GodotObject.IsInstanceValid(_previewHitpointsLabel))
			{
				_previewHitpointsLabel.Text = $"生命: {_editingCharacter.hitpoints:0.##}";
			}
			if (GodotObject.IsInstanceValid(_previewPacketLabel))
			{
				_previewPacketLabel.Text = "卡片: " + BuildPacketText(_editingCharacter);
			}
			if (GodotObject.IsInstanceValid(_previewModeLabel))
			{
				_previewModeLabel.Text = "创建: " + ReadString(_editingCharacter, "creationMode", "空白创建");
			}
			if (GodotObject.IsInstanceValid(_previewResourceLabel))
			{
				_previewResourceLabel.Text = "资源: " + FormatPath(CurrentResourcePath);
			}
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void RefreshComponentPanel()
	{
		if (_editingCharacter != null)
		{
			RefreshComponentPanel(_editingCharacter);
		}
	}

	private void RefreshComponentPanel(TowerDefenseCharacterConfig character)
	{
		PopulateVariantList(_componentListView, ReadArray(character, "componentList"));
		PopulateVariantList(_componentPatchListView, ReadArray(character, "componentPatch"));
		PopulateVariantList(_spawnEventListView, ReadArray(character, "spawnEvent"));
		PopulateVariantList(_dieEventListView, ReadArray(character, "dieEvent"));
		PopulateVariantList(_propertyChangeListView, ReadArray(character, "propertyChange"));
	}

	private void RefreshOverrideDiffPanel()
	{
		if (_editingCharacter != null)
		{
			RefreshOverrideDiffPanel(_editingCharacter);
		}
	}

	private void RefreshOverrideDiffPanel(TowerDefenseCharacterConfig character)
	{
		if (GodotObject.IsInstanceValid(_overrideDiffTree))
		{
			_overrideDiffTree.Clear();
			TreeItem root = _overrideDiffTree.CreateItem();
			AppendDiffRow(root, "baseCharacterKey", ReadString(character, "baseCharacterKey"), originalFromKey: true);
			AppendDiffRow(root, "targetCharacterKey", ReadString(character, "targetCharacterKey"), originalFromKey: true);
			AppendDiffRow(root, "sceneOverride", FormatResource(ReadResource(character, "sceneOverride")), originalFromKey: false);
			AppendDiffRow(root, "spriteOverride", FormatResource(ReadResource(character, "spriteOverride")), originalFromKey: false);
			AppendDiffRow(root, "packetPatch", FormatResource(ReadResource(character, "packetPatch")), originalFromKey: false);
			AppendDiffRow(root, "characterConfigPatch", FormatResource(ReadResource(character, "characterConfigPatch")), originalFromKey: false);
			AppendDiffRow(root, "componentPatch", $"Array({ReadArray(character, "componentPatch").Count})", originalFromKey: false);
			AppendDiffRow(root, "propertyChange", $"Array({ReadArray(character, "propertyChange").Count})", originalFromKey: false);
		}
	}

	private void AppendDiffRow(TreeItem root, string propertyName, string modValue, bool originalFromKey)
	{
		TreeItem treeItem = _overrideDiffTree.CreateItem(root);
		string text = (originalFromKey ? EmptyToPlaceholder(ReadString(_editingCharacter, "baseCharacterKey")) : "<原版>");
		bool flag = !string.IsNullOrWhiteSpace(modValue) && modValue != EmptyToPlaceholder("");
		treeItem.SetText(0, propertyName);
		treeItem.SetText(1, text);
		treeItem.SetText(2, EmptyToPlaceholder(modValue));
		treeItem.SetText(3, flag ? "是" : "否");
	}

	private void AddCharacterSummaryRows(TowerDefenseCharacterConfig character)
	{
		AddItemIfMissing(PreviewList, BuildNameText(character));
		AddItemIfMissing(PreviewList, BuildCategoryText(character));
		AddItemIfMissing(PreviewList, $"生命/费用: {character.hitpoints:0.##} / {character.cost}");
		AddItemIfMissing(TimelineList, "创建方式 creationMode -> " + ReadString(character, "creationMode", "空白创建"));
		AddItemIfMissing(TimelineList, $"组件 componentList -> {ReadArray(character, "componentList").Count}");
		AddItemIfMissing(GraphList, "角色 -> sceneOverride -> " + FormatResource(ReadResource(character, "sceneOverride")));
		AddItemIfMissing(GraphList, "角色 -> spriteOverride -> " + FormatResource(ReadResource(character, "spriteOverride")));
		AddItemIfMissing(GraphList, $"角色 -> componentPatch -> {ReadArray(character, "componentPatch").Count}");
		AddItemIfMissing(ReferenceList, "baseCharacterKey -> " + ReadString(character, "baseCharacterKey"));
		AddItemIfMissing(ReferenceList, "targetCharacterKey -> " + ReadString(character, "targetCharacterKey"));
		AddItemIfMissing(ReferenceList, "packetPatch -> " + FormatResource(ReadResource(character, "packetPatch")));
		AddItemIfMissing(ReferenceList, "characterConfigPatch -> " + FormatResource(ReadResource(character, "characterConfigPatch")));
	}

	private void BindLineEdit(LineEdit edit, string value, Action<string> changed)
	{
		edit.Text = value ?? "";
		edit.TextChanged += (string text) =>
		{
			if (!_updatingControls)
			{
				changed?.Invoke(text);
			}
		};
	}

	private void MountSmallSegmentedOption(OptionButton option, HFlowContainer host)
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
			_characterSegmentedOptions.Add(xWVisualSegmentedOption);
		}
	}

	private void DisposeCharacterVisualChoices()
	{
		foreach (XWVisualSegmentedOption characterSegmentedOption in _characterSegmentedOptions)
		{
			characterSegmentedOption?.Dispose();
		}
		_characterSegmentedOptions.Clear();
	}

	private void BindCharacterKeyCatalog(VBoxContainer root, NodePath editPath, NodePath buttonPath, string propertyName)
	{
		LineEdit edit = root.GetNode<LineEdit>(editPath);
		root.GetNode<Button>(buttonPath).Pressed += () =>
		{
			EnsureCharacterCatalogPicker();
			_characterCatalogPicker?.Open(XWGameplayResourceKind.Character, edit.Text, (XWGameplayResourceChoice choice) =>
			{
				if (!(choice == null))
				{
					_updatingControls = true;
					edit.Text = choice.Key;
					_updatingControls = false;
					OnCharacterDiscreteFieldChanged(propertyName, Variant.From<string>(choice.Key), refreshPreview: false);
				}
			}, null, lockKind: true, "角色图鉴");
		};
	}

	private void BindCharacterResourceCatalog(VBoxContainer root, NodePath buttonPath, string propertyName, string className, string displayName, string iconPath, bool rebuildPreview)
	{
		string text = propertyName switch
		{
			"sceneOverride" => "%SceneOverridePicker", 
			"spriteOverride" => "%SpriteOverridePicker", 
			"packetPatch" => "%PacketPatchPicker", 
			"characterConfigPatch" => "%CharacterConfigPatchPicker", 
			_ => string.Empty, 
		};
		XWResourcePicker surfacePicker = (string.IsNullOrWhiteSpace(text) ? null : root.GetNodeOrNull<XWResourcePicker>(text));
		root.GetNode<Button>(buttonPath).Pressed += () =>
		{
			EnsureCharacterCatalogPicker();
			Resource resource = ReadResource(_editingCharacter, propertyName);
			_characterCatalogPicker?.OpenResourceLibrary(className, displayName, resource?.ResourcePath ?? string.Empty, XWFileSystem.GetSingleton()?.ProjectFolderPath ?? string.Empty, new string[1] { className }, new string[3] { "Resources", "Scenes", "Asset" }, iconPath, (XWGameplayResourceChoice choice) =>
			{
				Resource from = ResourceLoader.Load<Resource>(choice?.ResourcePath ?? string.Empty, null, ResourceLoader.CacheMode.Reuse);
				surfacePicker?.SetEditedResource(from);
				OnCharacterDiscreteFieldChanged(propertyName, Variant.From(in from), refreshPreview: true, rebuildPreview);
			});
		};
	}

	private void EnsureCharacterCatalogPicker()
	{
		if (!GodotObject.IsInstanceValid(_characterCatalogPicker))
		{
			_characterCatalogPicker = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_characterCatalogPicker))
			{
				AddChild(_characterCatalogPicker, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void BindSpinBox(SpinBox spin, double value, Action<double> changed)
	{
		spin.Value = value;
		spin.ValueChanged += (double newValue) =>
		{
			if (!_updatingControls)
			{
				changed?.Invoke(newValue);
			}
		};
	}

	private void BindToggle(BaseButton button, bool value, Action<bool> changed)
	{
		button.ButtonPressed = value;
		button.Toggled += (bool newValue) =>
		{
			if (!_updatingControls)
			{
				changed?.Invoke(newValue);
			}
		};
	}

	private void BindStringOption(OptionButton option, string value, string[] values, Action<string> changed)
	{
		option.Clear();
		for (int i = 0; i < values.Length; i++)
		{
			option.AddItem(values[i], i);
			if (string.Equals(values[i], value, StringComparison.OrdinalIgnoreCase))
			{
				option.Select(i);
			}
		}
		option.ItemSelected += (long index) =>
		{
			if (!_updatingControls)
			{
				changed?.Invoke(option.GetItemText((int)index));
			}
		};
	}

	private void BindEnumOption<T>(OptionButton option, T value, Action<T> changed) where T : struct, Enum
	{
		option.Clear();
		T[] values = Enum.GetValues<T>();
		for (int i = 0; i < values.Length; i++)
		{
			T val = values[i];
			int id = Convert.ToInt32(val);
			option.AddItem(val.ToString(), id);
			if (EqualityComparer<T>.Default.Equals(val, value))
			{
				option.Select(option.ItemCount - 1);
			}
		}
		option.ItemSelected += (long index) =>
		{
			if (!_updatingControls)
			{
				int itemId = option.GetItemId((int)index);
				changed?.Invoke((T)Enum.ToObject(typeof(T), itemId));
			}
		};
	}

	private void BindResourcePicker(XWResourcePicker picker, string baseType, Resource value, Action<Resource> changed)
	{
		picker.Setup(baseType);
		picker.SetEditedResource(value);
		picker.ResourceChanged += (Resource resource) =>
		{
			if (!_updatingControls)
			{
				changed?.Invoke(resource);
			}
		};
	}

	private static void PopulateVariantList(ItemList list, Godot.Collections.Array array)
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
			list.AddItem($"{i}: {FormatVariant(array[i])}");
		}
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

	private static string BuildNameText(TowerDefenseCharacterConfig character)
	{
		return "角色: " + EmptyToPlaceholder(character?.name);
	}

	private static string BuildCategoryText(TowerDefenseCharacterConfig character)
	{
		return "分类: " + GetCharacterCategory(character);
	}

	private static string BuildPacketText(TowerDefenseCharacterConfig character)
	{
		if (character == null)
		{
			return "未配置";
		}
		return $"cost={character.cost}, cooldown={character.packetCooldown:0.###}";
	}

	private static string GetCharacterCategory(TowerDefenseCharacterConfig character)
	{
		if (!(character is TowerDefensePlantConfig))
		{
			if (!(character is TowerDefenseZombieConfig))
			{
				if (!(character is TowerDefenseVaseConfig))
				{
					if (!(character is TowerDefenseMowerConfig))
					{
						if (!(character is TowerDefenseItemConfig))
						{
							if (!(character is TowerDefenseGravestoneConfig))
							{
								if (character is TowerDefenseCraterConfig)
								{
									return "弹坑";
								}
								return "道具";
							}
							return "墓碑";
						}
						return "物品";
					}
					return "小推车";
				}
				return "花瓶";
			}
			return "僵尸";
		}
		return "植物";
	}

	private static Color GetCategoryColor(string category)
	{
		return category switch
		{
			"植物" => new Color(0.24f, 0.55f, 0.24f, 0.95f), 
			"僵尸" => new Color(0.42f, 0.46f, 0.52f, 0.95f), 
			"花瓶" => new Color(0.48f, 0.36f, 0.28f, 0.95f), 
			"小推车" => new Color(0.66f, 0.34f, 0.28f, 0.95f), 
			"墓碑" => new Color(0.32f, 0.32f, 0.36f, 0.95f), 
			"弹坑" => new Color(0.2f, 0.18f, 0.16f, 0.95f), 
			_ => new Color(0.32f, 0.4f, 0.58f, 0.95f), 
		};
	}

	private static void SetFlexibleProperty(Resource resource, string propertyName, Variant value)
	{
		if (GodotObject.IsInstanceValid(resource) && !string.IsNullOrWhiteSpace(propertyName))
		{
			if (HasResourceProperty(resource, propertyName))
			{
				resource.Set(propertyName, value);
			}
			else
			{
				resource.SetMeta(GetModMetaName(propertyName), value);
			}
		}
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
			string modMetaName = GetModMetaName(propertyName);
			if (resource.HasMeta(modMetaName))
			{
				return resource.GetMeta(modMetaName);
			}
		}
		catch
		{
		}
		return default;
	}

	private static string ReadString(Resource resource, string propertyName, string fallback = "")
	{
		Variant variant = ReadVariant(resource, propertyName);
		if (variant.VariantType == Variant.Type.Nil)
		{
			return fallback;
		}
		string text = variant.AsString();
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return fallback;
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

	private static Resource ReadResource(Resource resource, string propertyName)
	{
		Variant variant = ReadVariant(resource, propertyName);
		if (variant.VariantType != Variant.Type.Object)
		{
			return null;
		}
		return variant.AsGodotObject() as Resource;
	}

	private static Godot.Collections.Array ReadArray(Resource resource, string propertyName)
	{
		Variant variant = ReadVariant(resource, propertyName);
		if (variant.VariantType != Variant.Type.Array)
		{
			return new Godot.Collections.Array();
		}
		return variant.AsGodotArray();
	}

	private static bool HasResourceProperty(Resource resource, string propertyName)
	{
		if (!GodotObject.IsInstanceValid(resource) || string.IsNullOrWhiteSpace(propertyName))
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

	private static string GetModMetaName(string propertyName)
	{
		return "mod_" + propertyName;
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
			goto IL_015e;
		}
		switch ((int)num)
		{
		case 0:
		case 1:
			break;
		case 7:
			return $"Array({value.AsGodotArray().Count})";
		case 6:
			return $"Dictionary({value.AsGodotDictionary().Count})";
		case 3:
			return FormatResource(value.AsGodotObject() as Resource);
		default:
			goto IL_015e;
		}
		goto IL_00bb;
		IL_015e:
		return value.ToString();
		IL_00bb:
		return EmptyToPlaceholder(value.AsString());
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

	private static string FormatPath(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return path.GetFile();
		}
		return "未保存";
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
		return new List<MethodInfo>(104)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEmbeddedInspectorPropertyChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenderCharacterOverrideEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "characterOverride", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "propertyChange", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareCharacterOverrideSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindCharacterOverrideScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenOverrideBaseCharacterPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCharacterOverrideContext, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sourceDescription", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildCharacterOverridePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildCharacterOverridePreviewNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InstantiateCharacterPreviewScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySafeCharacterOverridePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySafePropertyChangePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "change", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsSafeCharacterPreviewProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StableRangeMidpoint, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "range", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateCharacterOverrideControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindCharacterOverrideEditing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCharacterOverrideVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectOverridePropertyChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedCharacterOverridePropertyChange, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddCharacterOverridePropertyChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DuplicateSelectedCharacterOverridePropertyChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedCharacterOverridePropertyChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedCharacterOverridePropertyChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCharacterOverridePropertyButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCharacterOverridePropertyList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatPropertyChange, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "change", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenOverridePropertyChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCharacterOverrideWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "rebuildPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCharacterOverrideSummary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatOverrideNumber, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatRange, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCharacterOverridePreviewGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResetCharacterOverridePreviewView, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCharacterOverridePreviewTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCharacterOverrideStageLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPreviewRootProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearPreviewRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeCharacterOverrideEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "disposePicker", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenderCharacterEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindCharacterPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindCharacterBasicFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindComponentPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindOverrideDiffPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindTypedCharacterFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetTypedCharacterFieldVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildCharacterPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildRuntimeCharacterPreviewScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasExternalCSharpSceneScript, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "scenePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSpriteFallbackPreview, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindRuntimeCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.InstantiateCharacterSpriteFallback, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spriteScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisableCharacterFallbackGameplayProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnPreviewHealthChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCharacterPreviewState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCharacterPreviewGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResetCharacterPreviewView, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCharacterPreviewTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCharacterPreviewStageLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindCharacterPackagePreviewScene, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsCharacterPreviewSpriteScenePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterPackageDirectoryForPreview, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "ownerPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveRelativeCharacterPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "ownerPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToAbsoluteCharacterPreviewFilePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeCharacterPreviewPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCharacterFieldChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Bool, "refreshPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "rebuildPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCharacterDiscreteFieldChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Bool, "refreshPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "rebuildPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCharacterDiscreteFieldFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Bool, "refreshPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "rebuildPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCharacterDiscreteSurfaceFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetResourceProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SavePendingCharacterResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveCharacterResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCharacterPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshComponentPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshComponentPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshOverrideDiffPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshOverrideDiffPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AppendDiffRow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "modValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "originalFromKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddCharacterSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.MountSmallSegmentedOption, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeCharacterVisualChoices, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindCharacterKeyCatalog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.NodePath, "editPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.NodePath, "buttonPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindCharacterResourceCatalog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.NodePath, "buttonPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "displayName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "rebuildPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCharacterCatalogPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateVariantList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.Array, "array", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddItemIfMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildNameText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCategoryText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPacketText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterCategory, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCategoryColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFlexibleProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadVariant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadDouble, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadResource, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasResourceProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetModMetaName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVariant, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatResource, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OnEmbeddedInspectorPropertyChanged && args.Count == 4)
		{
			OnEmbeddedInspectorPropertyChanged(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged && args.Count == 1)
		{
			OnVisualEditorVisibilityChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderCharacterOverrideEditor && args.Count == 2)
		{
			RenderCharacterOverrideEditor(VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterPropertyChangeConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareCharacterOverrideSession && args.Count == 0)
		{
			PrepareCharacterOverrideSession();
			ret = default;
			return true;
		}
		if (method == MethodName.BindCharacterOverrideScene && args.Count == 1)
		{
			BindCharacterOverrideScene(VariantUtils.ConvertTo<PanelContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenOverrideBaseCharacterPicker && args.Count == 0)
		{
			OpenOverrideBaseCharacterPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCharacterOverrideContext && args.Count == 2)
		{
			UpdateCharacterOverrideContext(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildCharacterOverridePreview && args.Count == 0)
		{
			RebuildCharacterOverridePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildCharacterOverridePreviewNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(BuildCharacterOverridePreviewNode());
			return true;
		}
		if (method == MethodName.InstantiateCharacterPreviewScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Node>(InstantiateCharacterPreviewScene(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplySafeCharacterOverridePreview && args.Count == 0)
		{
			ApplySafeCharacterOverridePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySafePropertyChangePreview && args.Count == 1)
		{
			ApplySafePropertyChangePreview(VariantUtils.ConvertTo<TowerDefenseCharacterPropertyChangeConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSafeCharacterPreviewProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSafeCharacterPreviewProperty(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.StableRangeMidpoint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(StableRangeMidpoint(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.PopulateCharacterOverrideControls && args.Count == 0)
		{
			PopulateCharacterOverrideControls();
			ret = default;
			return true;
		}
		if (method == MethodName.BindCharacterOverrideEditing && args.Count == 0)
		{
			BindCharacterOverrideEditing();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCharacterOverrideVisualPropertyEdited && args.Count == 1)
		{
			OnCharacterOverrideVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectOverridePropertyChange && args.Count == 1)
		{
			SelectOverridePropertyChange(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedCharacterOverridePropertyChange && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacterPropertyChangeConfig>(GetSelectedCharacterOverridePropertyChange());
			return true;
		}
		if (method == MethodName.AddCharacterOverridePropertyChange && args.Count == 0)
		{
			AddCharacterOverridePropertyChange();
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicateSelectedCharacterOverridePropertyChange && args.Count == 0)
		{
			DuplicateSelectedCharacterOverridePropertyChange();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedCharacterOverridePropertyChange && args.Count == 0)
		{
			RemoveSelectedCharacterOverridePropertyChange();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedCharacterOverridePropertyChange && args.Count == 1)
		{
			MoveSelectedCharacterOverridePropertyChange(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCharacterOverridePropertyButtons && args.Count == 0)
		{
			UpdateCharacterOverridePropertyButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCharacterOverridePropertyList && args.Count == 0)
		{
			RefreshCharacterOverridePropertyList();
			ret = default;
			return true;
		}
		if (method == MethodName.FormatPropertyChange && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatPropertyChange(VariantUtils.ConvertTo<TowerDefenseCharacterPropertyChangeConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.OpenOverridePropertyChange && args.Count == 1)
		{
			OpenOverridePropertyChange(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCharacterOverrideWorkbench && args.Count == 1)
		{
			RefreshCharacterOverrideWorkbench(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCharacterOverrideSummary && args.Count == 0)
		{
			RefreshCharacterOverrideSummary();
			ret = default;
			return true;
		}
		if (method == MethodName.FormatOverrideNumber && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatOverrideNumber(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatRange && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatRange(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.OnCharacterOverridePreviewGuiInput && args.Count == 1)
		{
			OnCharacterOverridePreviewGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCharacterOverridePreviewView && args.Count == 0)
		{
			ResetCharacterOverridePreviewView();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCharacterOverridePreviewTransform && args.Count == 0)
		{
			UpdateCharacterOverridePreviewTransform();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCharacterOverrideStageLayout && args.Count == 0)
		{
			UpdateCharacterOverrideStageLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPreviewRootProcessing && args.Count == 2)
		{
			SetPreviewRootProcessing(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPreviewRoot && args.Count == 1)
		{
			ClearPreviewRoot(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeCharacterOverrideEditor && args.Count == 1)
		{
			DisposeCharacterOverrideEditor(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderCharacterEditor && args.Count == 1)
		{
			RenderCharacterEditor(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCharacterPreview && args.Count == 2)
		{
			BindCharacterPreview(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCharacterBasicFields && args.Count == 2)
		{
			BindCharacterBasicFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindComponentPanel && args.Count == 2)
		{
			BindComponentPanel(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindOverrideDiffPanel && args.Count == 2)
		{
			BindOverrideDiffPanel(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindTypedCharacterFields && args.Count == 2)
		{
			BindTypedCharacterFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTypedCharacterFieldVisibility && args.Count == 2)
		{
			SetTypedCharacterFieldVisibility(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildCharacterPreview && args.Count == 0)
		{
			RebuildCharacterPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRuntimeCharacterPreviewScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(BuildRuntimeCharacterPreviewScene(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.HasExternalCSharpSceneScript && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasExternalCSharpSceneScript(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildSpriteFallbackPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(BuildSpriteFallbackPreview(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FindRuntimeCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindRuntimeCharacter(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.InstantiateCharacterSpriteFallback && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(InstantiateCharacterSpriteFallback(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.DisableCharacterFallbackGameplayProcessing && args.Count == 1)
		{
			DisableCharacterFallbackGameplayProcessing(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPreviewHealthChanged && args.Count == 1)
		{
			OnPreviewHealthChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCharacterPreviewState && args.Count == 0)
		{
			ApplyCharacterPreviewState();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCharacterPreviewGuiInput && args.Count == 1)
		{
			OnCharacterPreviewGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCharacterPreviewView && args.Count == 0)
		{
			ResetCharacterPreviewView();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCharacterPreviewTransform && args.Count == 0)
		{
			UpdateCharacterPreviewTransform();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCharacterPreviewStageLayout && args.Count == 0)
		{
			UpdateCharacterPreviewStageLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.FindCharacterPackagePreviewScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FindCharacterPackagePreviewScene(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCharacterPreviewSpriteScenePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCharacterPreviewSpriteScenePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterPackageDirectoryForPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterPackageDirectoryForPreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveRelativeCharacterPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveRelativeCharacterPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ToAbsoluteCharacterPreviewFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ToAbsoluteCharacterPreviewFilePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeCharacterPreviewPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeCharacterPreviewPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnCharacterFieldChanged && args.Count == 4)
		{
			OnCharacterFieldChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCharacterDiscreteFieldChanged && args.Count == 4)
		{
			OnCharacterDiscreteFieldChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCharacterDiscreteFieldFromHistory && args.Count == 4)
		{
			ApplyCharacterDiscreteFieldFromHistory(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCharacterDiscreteSurfaceFromHistory && args.Count == 0)
		{
			RefreshCharacterDiscreteSurfaceFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.SetResourceProperty && args.Count == 2)
		{
			SetResourceProperty(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SavePendingCharacterResource && args.Count == 0)
		{
			SavePendingCharacterResource();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveCharacterResource && args.Count == 1)
		{
			SaveCharacterResource(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCharacterPreview && args.Count == 0)
		{
			UpdateCharacterPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshComponentPanel && args.Count == 0)
		{
			RefreshComponentPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshComponentPanel && args.Count == 1)
		{
			RefreshComponentPanel(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshOverrideDiffPanel && args.Count == 0)
		{
			RefreshOverrideDiffPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshOverrideDiffPanel && args.Count == 1)
		{
			RefreshOverrideDiffPanel(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AppendDiffRow && args.Count == 4)
		{
			AppendDiffRow(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddCharacterSummaryRows && args.Count == 1)
		{
			AddCharacterSummaryRows(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MountSmallSegmentedOption && args.Count == 2)
		{
			MountSmallSegmentedOption(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<HFlowContainer>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeCharacterVisualChoices && args.Count == 0)
		{
			DisposeCharacterVisualChoices();
			ret = default;
			return true;
		}
		if (method == MethodName.BindCharacterKeyCatalog && args.Count == 4)
		{
			BindCharacterKeyCatalog(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<NodePath>(in args[1]), VariantUtils.ConvertTo<NodePath>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCharacterResourceCatalog && args.Count == 7)
		{
			BindCharacterResourceCatalog(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<NodePath>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCharacterCatalogPicker && args.Count == 0)
		{
			EnsureCharacterCatalogPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateVariantList && args.Count == 2)
		{
			PopulateVariantList(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildNameText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildNameText(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCategoryText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCategoryText(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPacketText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPacketText(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterCategory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterCategory(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCategoryColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCategoryColor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetFlexibleProperty && args.Count == 3)
		{
			SetFlexibleProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadVariant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadVariant(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadString && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(ReadString(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadDouble && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadDouble(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadResource && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Resource>(ReadResource(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadArray && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(ReadArray(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.HasResourceProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasResourceProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetModMetaName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetModMetaName(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.FormatPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatPath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.InstantiateCharacterPreviewScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Node>(InstantiateCharacterPreviewScene(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.IsSafeCharacterPreviewProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSafeCharacterPreviewProperty(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.StableRangeMidpoint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(StableRangeMidpoint(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatPropertyChange && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatPropertyChange(VariantUtils.ConvertTo<TowerDefenseCharacterPropertyChangeConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.FormatOverrideNumber && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatOverrideNumber(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatRange && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatRange(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPreviewRootProcessing && args.Count == 2)
		{
			SetPreviewRootProcessing(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPreviewRoot && args.Count == 1)
		{
			ClearPreviewRoot(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTypedCharacterFieldVisibility && args.Count == 2)
		{
			SetTypedCharacterFieldVisibility(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasExternalCSharpSceneScript && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasExternalCSharpSceneScript(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindRuntimeCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindRuntimeCharacter(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.DisableCharacterFallbackGameplayProcessing && args.Count == 1)
		{
			DisableCharacterFallbackGameplayProcessing(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsCharacterPreviewSpriteScenePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCharacterPreviewSpriteScenePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterPackageDirectoryForPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterPackageDirectoryForPreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveRelativeCharacterPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveRelativeCharacterPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ToAbsoluteCharacterPreviewFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ToAbsoluteCharacterPreviewFilePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeCharacterPreviewPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeCharacterPreviewPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.PopulateVariantList && args.Count == 2)
		{
			PopulateVariantList(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildNameText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildNameText(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCategoryText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCategoryText(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPacketText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPacketText(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterCategory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterCategory(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCategoryColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCategoryColor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetFlexibleProperty && args.Count == 3)
		{
			SetFlexibleProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadVariant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadVariant(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadString && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(ReadString(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadDouble && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadDouble(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadResource && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Resource>(ReadResource(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadArray && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(ReadArray(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.HasResourceProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasResourceProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetModMetaName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetModMetaName(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.FormatPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatPath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.OnEmbeddedInspectorPropertyChanged)
		{
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.RenderCharacterOverrideEditor)
		{
			return true;
		}
		if (method == MethodName.PrepareCharacterOverrideSession)
		{
			return true;
		}
		if (method == MethodName.BindCharacterOverrideScene)
		{
			return true;
		}
		if (method == MethodName.OpenOverrideBaseCharacterPicker)
		{
			return true;
		}
		if (method == MethodName.UpdateCharacterOverrideContext)
		{
			return true;
		}
		if (method == MethodName.RebuildCharacterOverridePreview)
		{
			return true;
		}
		if (method == MethodName.BuildCharacterOverridePreviewNode)
		{
			return true;
		}
		if (method == MethodName.InstantiateCharacterPreviewScene)
		{
			return true;
		}
		if (method == MethodName.ApplySafeCharacterOverridePreview)
		{
			return true;
		}
		if (method == MethodName.ApplySafePropertyChangePreview)
		{
			return true;
		}
		if (method == MethodName.IsSafeCharacterPreviewProperty)
		{
			return true;
		}
		if (method == MethodName.StableRangeMidpoint)
		{
			return true;
		}
		if (method == MethodName.PopulateCharacterOverrideControls)
		{
			return true;
		}
		if (method == MethodName.BindCharacterOverrideEditing)
		{
			return true;
		}
		if (method == MethodName.OnCharacterOverrideVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.SelectOverridePropertyChange)
		{
			return true;
		}
		if (method == MethodName.GetSelectedCharacterOverridePropertyChange)
		{
			return true;
		}
		if (method == MethodName.AddCharacterOverridePropertyChange)
		{
			return true;
		}
		if (method == MethodName.DuplicateSelectedCharacterOverridePropertyChange)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedCharacterOverridePropertyChange)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedCharacterOverridePropertyChange)
		{
			return true;
		}
		if (method == MethodName.UpdateCharacterOverridePropertyButtons)
		{
			return true;
		}
		if (method == MethodName.RefreshCharacterOverridePropertyList)
		{
			return true;
		}
		if (method == MethodName.FormatPropertyChange)
		{
			return true;
		}
		if (method == MethodName.OpenOverridePropertyChange)
		{
			return true;
		}
		if (method == MethodName.RefreshCharacterOverrideWorkbench)
		{
			return true;
		}
		if (method == MethodName.RefreshCharacterOverrideSummary)
		{
			return true;
		}
		if (method == MethodName.FormatOverrideNumber)
		{
			return true;
		}
		if (method == MethodName.FormatRange)
		{
			return true;
		}
		if (method == MethodName.OnCharacterOverridePreviewGuiInput)
		{
			return true;
		}
		if (method == MethodName.ResetCharacterOverridePreviewView)
		{
			return true;
		}
		if (method == MethodName.UpdateCharacterOverridePreviewTransform)
		{
			return true;
		}
		if (method == MethodName.UpdateCharacterOverrideStageLayout)
		{
			return true;
		}
		if (method == MethodName.SetPreviewRootProcessing)
		{
			return true;
		}
		if (method == MethodName.ClearPreviewRoot)
		{
			return true;
		}
		if (method == MethodName.DisposeCharacterOverrideEditor)
		{
			return true;
		}
		if (method == MethodName.RenderCharacterEditor)
		{
			return true;
		}
		if (method == MethodName.BindCharacterPreview)
		{
			return true;
		}
		if (method == MethodName.BindCharacterBasicFields)
		{
			return true;
		}
		if (method == MethodName.BindComponentPanel)
		{
			return true;
		}
		if (method == MethodName.BindOverrideDiffPanel)
		{
			return true;
		}
		if (method == MethodName.BindTypedCharacterFields)
		{
			return true;
		}
		if (method == MethodName.SetTypedCharacterFieldVisibility)
		{
			return true;
		}
		if (method == MethodName.RebuildCharacterPreview)
		{
			return true;
		}
		if (method == MethodName.BuildRuntimeCharacterPreviewScene)
		{
			return true;
		}
		if (method == MethodName.HasExternalCSharpSceneScript)
		{
			return true;
		}
		if (method == MethodName.BuildSpriteFallbackPreview)
		{
			return true;
		}
		if (method == MethodName.FindRuntimeCharacter)
		{
			return true;
		}
		if (method == MethodName.InstantiateCharacterSpriteFallback)
		{
			return true;
		}
		if (method == MethodName.DisableCharacterFallbackGameplayProcessing)
		{
			return true;
		}
		if (method == MethodName.OnPreviewHealthChanged)
		{
			return true;
		}
		if (method == MethodName.ApplyCharacterPreviewState)
		{
			return true;
		}
		if (method == MethodName.OnCharacterPreviewGuiInput)
		{
			return true;
		}
		if (method == MethodName.ResetCharacterPreviewView)
		{
			return true;
		}
		if (method == MethodName.UpdateCharacterPreviewTransform)
		{
			return true;
		}
		if (method == MethodName.UpdateCharacterPreviewStageLayout)
		{
			return true;
		}
		if (method == MethodName.FindCharacterPackagePreviewScene)
		{
			return true;
		}
		if (method == MethodName.IsCharacterPreviewSpriteScenePath)
		{
			return true;
		}
		if (method == MethodName.GetCharacterPackageDirectoryForPreview)
		{
			return true;
		}
		if (method == MethodName.ResolveRelativeCharacterPath)
		{
			return true;
		}
		if (method == MethodName.ToAbsoluteCharacterPreviewFilePath)
		{
			return true;
		}
		if (method == MethodName.NormalizeCharacterPreviewPath)
		{
			return true;
		}
		if (method == MethodName.OnCharacterFieldChanged)
		{
			return true;
		}
		if (method == MethodName.OnCharacterDiscreteFieldChanged)
		{
			return true;
		}
		if (method == MethodName.ApplyCharacterDiscreteFieldFromHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshCharacterDiscreteSurfaceFromHistory)
		{
			return true;
		}
		if (method == MethodName.SetResourceProperty)
		{
			return true;
		}
		if (method == MethodName.SavePendingCharacterResource)
		{
			return true;
		}
		if (method == MethodName.SaveCharacterResource)
		{
			return true;
		}
		if (method == MethodName.UpdateCharacterPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshComponentPanel)
		{
			return true;
		}
		if (method == MethodName.RefreshOverrideDiffPanel)
		{
			return true;
		}
		if (method == MethodName.AppendDiffRow)
		{
			return true;
		}
		if (method == MethodName.AddCharacterSummaryRows)
		{
			return true;
		}
		if (method == MethodName.MountSmallSegmentedOption)
		{
			return true;
		}
		if (method == MethodName.DisposeCharacterVisualChoices)
		{
			return true;
		}
		if (method == MethodName.BindCharacterKeyCatalog)
		{
			return true;
		}
		if (method == MethodName.BindCharacterResourceCatalog)
		{
			return true;
		}
		if (method == MethodName.EnsureCharacterCatalogPicker)
		{
			return true;
		}
		if (method == MethodName.PopulateVariantList)
		{
			return true;
		}
		if (method == MethodName.AddItemIfMissing)
		{
			return true;
		}
		if (method == MethodName.BuildNameText)
		{
			return true;
		}
		if (method == MethodName.BuildCategoryText)
		{
			return true;
		}
		if (method == MethodName.BuildPacketText)
		{
			return true;
		}
		if (method == MethodName.GetCharacterCategory)
		{
			return true;
		}
		if (method == MethodName.GetCategoryColor)
		{
			return true;
		}
		if (method == MethodName.SetFlexibleProperty)
		{
			return true;
		}
		if (method == MethodName.ReadVariant)
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
		if (method == MethodName.ReadResource)
		{
			return true;
		}
		if (method == MethodName.ReadArray)
		{
			return true;
		}
		if (method == MethodName.HasResourceProperty)
		{
			return true;
		}
		if (method == MethodName.GetModMetaName)
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
		if (method == MethodName.FormatPath)
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
		if (name == PropertyName._editingCharacter)
		{
			_editingCharacter = VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in value);
			return true;
		}
		if (name == PropertyName._characterViewport)
		{
			_characterViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._characterViewportContainer)
		{
			_characterViewportContainer = VariantUtils.ConvertTo<SubViewportContainer>(in value);
			return true;
		}
		if (name == PropertyName._characterPreviewBackground)
		{
			_characterPreviewBackground = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._characterGroundShade)
		{
			_characterGroundShade = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName._characterPreviewRoot)
		{
			_characterPreviewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._runtimeCharacterPreview)
		{
			_runtimeCharacterPreview = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._characterPreviewUsesExternalScriptFallback)
		{
			_characterPreviewUsesExternalScriptFallback = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._previewStatusLabel)
		{
			_previewStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewHealthSlider)
		{
			_previewHealthSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._previewHealthValueLabel)
		{
			_previewHealthValueLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._draggingCharacterPreview)
		{
			_draggingCharacterPreview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._characterPreviewPan)
		{
			_characterPreviewPan = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._characterPreviewZoom)
		{
			_characterPreviewZoom = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._previewNameLabel)
		{
			_previewNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewCategoryLabel)
		{
			_previewCategoryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewModeLabel)
		{
			_previewModeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewHitpointsLabel)
		{
			_previewHitpointsLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewPacketLabel)
		{
			_previewPacketLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewResourceLabel)
		{
			_previewResourceLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._componentListView)
		{
			_componentListView = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._componentPatchListView)
		{
			_componentPatchListView = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._spawnEventListView)
		{
			_spawnEventListView = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._dieEventListView)
		{
			_dieEventListView = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._propertyChangeListView)
		{
			_propertyChangeListView = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._overrideDiffTree)
		{
			_overrideDiffTree = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._characterSaveTimer)
		{
			_characterSaveTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._editingCharacterOverride)
		{
			_editingCharacterOverride = VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in value);
			return true;
		}
		if (name == PropertyName._editingPropertyChange)
		{
			_editingPropertyChange = VariantUtils.ConvertTo<TowerDefenseCharacterPropertyChangeConfig>(in value);
			return true;
		}
		if (name == PropertyName._editingOverrideResource)
		{
			_editingOverrideResource = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._overrideBaseCharacter)
		{
			_overrideBaseCharacter = VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in value);
			return true;
		}
		if (name == PropertyName._overrideBasePacket)
		{
			_overrideBasePacket = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName._overrideRuntimeScene)
		{
			_overrideRuntimeScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName._overrideSpriteScene)
		{
			_overrideSpriteScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName._characterOverrideWorkbench)
		{
			_characterOverrideWorkbench = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._overrideViewport)
		{
			_overrideViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._overrideViewportContainer)
		{
			_overrideViewportContainer = VariantUtils.ConvertTo<SubViewportContainer>(in value);
			return true;
		}
		if (name == PropertyName._overridePreviewBackground)
		{
			_overridePreviewBackground = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._overrideGroundShade)
		{
			_overrideGroundShade = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName._overridePreviewCameraRoot)
		{
			_overridePreviewCameraRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._overrideBaseCharacterRoot)
		{
			_overrideBaseCharacterRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._overriddenCharacterRoot)
		{
			_overriddenCharacterRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._overrideBaseRuntimeCharacter)
		{
			_overrideBaseRuntimeCharacter = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._overriddenRuntimeCharacter)
		{
			_overriddenRuntimeCharacter = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._overrideContextLabel)
		{
			_overrideContextLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._overrideSourceLabel)
		{
			_overrideSourceLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._overridePreviewStatusLabel)
		{
			_overridePreviewStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._overrideZoomLabel)
		{
			_overrideZoomLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._overrideEffectSummaryLabel)
		{
			_overrideEffectSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._overrideSafetySummaryLabel)
		{
			_overrideSafetySummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._overrideSaveStateLabel)
		{
			_overrideSaveStateLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._overridePropertyChangeList)
		{
			_overridePropertyChangeList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._selectOverrideBaseCharacterButton)
		{
			_selectOverrideBaseCharacterButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._overrideInvisibleCheck)
		{
			_overrideInvisibleCheck = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._overrideScaleSpinBox)
		{
			_overrideScaleSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._overrideHitpointScaleSpinBox)
		{
			_overrideHitpointScaleSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._overrideWalkSpeedMinSpinBox)
		{
			_overrideWalkSpeedMinSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._overrideWalkSpeedMaxSpinBox)
		{
			_overrideWalkSpeedMaxSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._overrideAnimeSpeedMinSpinBox)
		{
			_overrideAnimeSpeedMinSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._overrideAnimeSpeedMaxSpinBox)
		{
			_overrideAnimeSpeedMaxSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._overrideCanMowerMoveCheck)
		{
			_overrideCanMowerMoveCheck = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._addOverridePropertyChangeButton)
		{
			_addOverridePropertyChangeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._duplicateOverridePropertyChangeButton)
		{
			_duplicateOverridePropertyChangeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveOverridePropertyChangeUpButton)
		{
			_moveOverridePropertyChangeUpButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveOverridePropertyChangeDownButton)
		{
			_moveOverridePropertyChangeDownButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._removeOverridePropertyChangeButton)
		{
			_removeOverridePropertyChangeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._overridePropertyNameLineEdit)
		{
			_overridePropertyNameLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._overridePropertyValueLineEdit)
		{
			_overridePropertyValueLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._overridePropertyValueTypeLabel)
		{
			_overridePropertyValueTypeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._draggingOverridePreview)
		{
			_draggingOverridePreview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._overridePreviewPan)
		{
			_overridePreviewPan = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._overridePreviewZoom)
		{
			_overridePreviewZoom = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._overrideSelectedPacketName)
		{
			_overrideSelectedPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._overrideResolvedPacketName)
		{
			_overrideResolvedPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._overridePreviewResourceId)
		{
			_overridePreviewResourceId = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._selectedOverridePropertyChangeIndex)
		{
			_selectedOverridePropertyChangeIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._overrideBaseCharacterPicker)
		{
			_overrideBaseCharacterPicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._characterCatalogPicker)
		{
			_characterCatalogPicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editingCharacter)
		{
			value = VariantUtils.CreateFrom(in _editingCharacter);
			return true;
		}
		if (name == PropertyName._characterViewport)
		{
			value = VariantUtils.CreateFrom(in _characterViewport);
			return true;
		}
		if (name == PropertyName._characterViewportContainer)
		{
			value = VariantUtils.CreateFrom(in _characterViewportContainer);
			return true;
		}
		if (name == PropertyName._characterPreviewBackground)
		{
			value = VariantUtils.CreateFrom(in _characterPreviewBackground);
			return true;
		}
		if (name == PropertyName._characterGroundShade)
		{
			value = VariantUtils.CreateFrom(in _characterGroundShade);
			return true;
		}
		if (name == PropertyName._characterPreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _characterPreviewRoot);
			return true;
		}
		if (name == PropertyName._runtimeCharacterPreview)
		{
			value = VariantUtils.CreateFrom(in _runtimeCharacterPreview);
			return true;
		}
		if (name == PropertyName._characterPreviewUsesExternalScriptFallback)
		{
			value = VariantUtils.CreateFrom(in _characterPreviewUsesExternalScriptFallback);
			return true;
		}
		if (name == PropertyName._previewStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _previewStatusLabel);
			return true;
		}
		if (name == PropertyName._previewHealthSlider)
		{
			value = VariantUtils.CreateFrom(in _previewHealthSlider);
			return true;
		}
		if (name == PropertyName._previewHealthValueLabel)
		{
			value = VariantUtils.CreateFrom(in _previewHealthValueLabel);
			return true;
		}
		if (name == PropertyName._draggingCharacterPreview)
		{
			value = VariantUtils.CreateFrom(in _draggingCharacterPreview);
			return true;
		}
		if (name == PropertyName._characterPreviewPan)
		{
			value = VariantUtils.CreateFrom(in _characterPreviewPan);
			return true;
		}
		if (name == PropertyName._characterPreviewZoom)
		{
			value = VariantUtils.CreateFrom(in _characterPreviewZoom);
			return true;
		}
		if (name == PropertyName._previewNameLabel)
		{
			value = VariantUtils.CreateFrom(in _previewNameLabel);
			return true;
		}
		if (name == PropertyName._previewCategoryLabel)
		{
			value = VariantUtils.CreateFrom(in _previewCategoryLabel);
			return true;
		}
		if (name == PropertyName._previewModeLabel)
		{
			value = VariantUtils.CreateFrom(in _previewModeLabel);
			return true;
		}
		if (name == PropertyName._previewHitpointsLabel)
		{
			value = VariantUtils.CreateFrom(in _previewHitpointsLabel);
			return true;
		}
		if (name == PropertyName._previewPacketLabel)
		{
			value = VariantUtils.CreateFrom(in _previewPacketLabel);
			return true;
		}
		if (name == PropertyName._previewResourceLabel)
		{
			value = VariantUtils.CreateFrom(in _previewResourceLabel);
			return true;
		}
		if (name == PropertyName._componentListView)
		{
			value = VariantUtils.CreateFrom(in _componentListView);
			return true;
		}
		if (name == PropertyName._componentPatchListView)
		{
			value = VariantUtils.CreateFrom(in _componentPatchListView);
			return true;
		}
		if (name == PropertyName._spawnEventListView)
		{
			value = VariantUtils.CreateFrom(in _spawnEventListView);
			return true;
		}
		if (name == PropertyName._dieEventListView)
		{
			value = VariantUtils.CreateFrom(in _dieEventListView);
			return true;
		}
		if (name == PropertyName._propertyChangeListView)
		{
			value = VariantUtils.CreateFrom(in _propertyChangeListView);
			return true;
		}
		if (name == PropertyName._overrideDiffTree)
		{
			value = VariantUtils.CreateFrom(in _overrideDiffTree);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._characterSaveTimer)
		{
			value = VariantUtils.CreateFrom(in _characterSaveTimer);
			return true;
		}
		if (name == PropertyName._editingCharacterOverride)
		{
			value = VariantUtils.CreateFrom(in _editingCharacterOverride);
			return true;
		}
		if (name == PropertyName._editingPropertyChange)
		{
			value = VariantUtils.CreateFrom(in _editingPropertyChange);
			return true;
		}
		if (name == PropertyName._editingOverrideResource)
		{
			value = VariantUtils.CreateFrom(in _editingOverrideResource);
			return true;
		}
		if (name == PropertyName._overrideBaseCharacter)
		{
			value = VariantUtils.CreateFrom(in _overrideBaseCharacter);
			return true;
		}
		if (name == PropertyName._overrideBasePacket)
		{
			value = VariantUtils.CreateFrom(in _overrideBasePacket);
			return true;
		}
		if (name == PropertyName._overrideRuntimeScene)
		{
			value = VariantUtils.CreateFrom(in _overrideRuntimeScene);
			return true;
		}
		if (name == PropertyName._overrideSpriteScene)
		{
			value = VariantUtils.CreateFrom(in _overrideSpriteScene);
			return true;
		}
		if (name == PropertyName._characterOverrideWorkbench)
		{
			value = VariantUtils.CreateFrom(in _characterOverrideWorkbench);
			return true;
		}
		if (name == PropertyName._overrideViewport)
		{
			value = VariantUtils.CreateFrom(in _overrideViewport);
			return true;
		}
		if (name == PropertyName._overrideViewportContainer)
		{
			value = VariantUtils.CreateFrom(in _overrideViewportContainer);
			return true;
		}
		if (name == PropertyName._overridePreviewBackground)
		{
			value = VariantUtils.CreateFrom(in _overridePreviewBackground);
			return true;
		}
		if (name == PropertyName._overrideGroundShade)
		{
			value = VariantUtils.CreateFrom(in _overrideGroundShade);
			return true;
		}
		if (name == PropertyName._overridePreviewCameraRoot)
		{
			value = VariantUtils.CreateFrom(in _overridePreviewCameraRoot);
			return true;
		}
		if (name == PropertyName._overrideBaseCharacterRoot)
		{
			value = VariantUtils.CreateFrom(in _overrideBaseCharacterRoot);
			return true;
		}
		if (name == PropertyName._overriddenCharacterRoot)
		{
			value = VariantUtils.CreateFrom(in _overriddenCharacterRoot);
			return true;
		}
		if (name == PropertyName._overrideBaseRuntimeCharacter)
		{
			value = VariantUtils.CreateFrom(in _overrideBaseRuntimeCharacter);
			return true;
		}
		if (name == PropertyName._overriddenRuntimeCharacter)
		{
			value = VariantUtils.CreateFrom(in _overriddenRuntimeCharacter);
			return true;
		}
		if (name == PropertyName._overrideContextLabel)
		{
			value = VariantUtils.CreateFrom(in _overrideContextLabel);
			return true;
		}
		if (name == PropertyName._overrideSourceLabel)
		{
			value = VariantUtils.CreateFrom(in _overrideSourceLabel);
			return true;
		}
		if (name == PropertyName._overridePreviewStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _overridePreviewStatusLabel);
			return true;
		}
		if (name == PropertyName._overrideZoomLabel)
		{
			value = VariantUtils.CreateFrom(in _overrideZoomLabel);
			return true;
		}
		if (name == PropertyName._overrideEffectSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _overrideEffectSummaryLabel);
			return true;
		}
		if (name == PropertyName._overrideSafetySummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _overrideSafetySummaryLabel);
			return true;
		}
		if (name == PropertyName._overrideSaveStateLabel)
		{
			value = VariantUtils.CreateFrom(in _overrideSaveStateLabel);
			return true;
		}
		if (name == PropertyName._overridePropertyChangeList)
		{
			value = VariantUtils.CreateFrom(in _overridePropertyChangeList);
			return true;
		}
		if (name == PropertyName._selectOverrideBaseCharacterButton)
		{
			value = VariantUtils.CreateFrom(in _selectOverrideBaseCharacterButton);
			return true;
		}
		if (name == PropertyName._overrideInvisibleCheck)
		{
			value = VariantUtils.CreateFrom(in _overrideInvisibleCheck);
			return true;
		}
		if (name == PropertyName._overrideScaleSpinBox)
		{
			value = VariantUtils.CreateFrom(in _overrideScaleSpinBox);
			return true;
		}
		if (name == PropertyName._overrideHitpointScaleSpinBox)
		{
			value = VariantUtils.CreateFrom(in _overrideHitpointScaleSpinBox);
			return true;
		}
		if (name == PropertyName._overrideWalkSpeedMinSpinBox)
		{
			value = VariantUtils.CreateFrom(in _overrideWalkSpeedMinSpinBox);
			return true;
		}
		if (name == PropertyName._overrideWalkSpeedMaxSpinBox)
		{
			value = VariantUtils.CreateFrom(in _overrideWalkSpeedMaxSpinBox);
			return true;
		}
		if (name == PropertyName._overrideAnimeSpeedMinSpinBox)
		{
			value = VariantUtils.CreateFrom(in _overrideAnimeSpeedMinSpinBox);
			return true;
		}
		if (name == PropertyName._overrideAnimeSpeedMaxSpinBox)
		{
			value = VariantUtils.CreateFrom(in _overrideAnimeSpeedMaxSpinBox);
			return true;
		}
		if (name == PropertyName._overrideCanMowerMoveCheck)
		{
			value = VariantUtils.CreateFrom(in _overrideCanMowerMoveCheck);
			return true;
		}
		if (name == PropertyName._addOverridePropertyChangeButton)
		{
			value = VariantUtils.CreateFrom(in _addOverridePropertyChangeButton);
			return true;
		}
		if (name == PropertyName._duplicateOverridePropertyChangeButton)
		{
			value = VariantUtils.CreateFrom(in _duplicateOverridePropertyChangeButton);
			return true;
		}
		if (name == PropertyName._moveOverridePropertyChangeUpButton)
		{
			value = VariantUtils.CreateFrom(in _moveOverridePropertyChangeUpButton);
			return true;
		}
		if (name == PropertyName._moveOverridePropertyChangeDownButton)
		{
			value = VariantUtils.CreateFrom(in _moveOverridePropertyChangeDownButton);
			return true;
		}
		if (name == PropertyName._removeOverridePropertyChangeButton)
		{
			value = VariantUtils.CreateFrom(in _removeOverridePropertyChangeButton);
			return true;
		}
		if (name == PropertyName._overridePropertyNameLineEdit)
		{
			value = VariantUtils.CreateFrom(in _overridePropertyNameLineEdit);
			return true;
		}
		if (name == PropertyName._overridePropertyValueLineEdit)
		{
			value = VariantUtils.CreateFrom(in _overridePropertyValueLineEdit);
			return true;
		}
		if (name == PropertyName._overridePropertyValueTypeLabel)
		{
			value = VariantUtils.CreateFrom(in _overridePropertyValueTypeLabel);
			return true;
		}
		if (name == PropertyName._draggingOverridePreview)
		{
			value = VariantUtils.CreateFrom(in _draggingOverridePreview);
			return true;
		}
		if (name == PropertyName._overridePreviewPan)
		{
			value = VariantUtils.CreateFrom(in _overridePreviewPan);
			return true;
		}
		if (name == PropertyName._overridePreviewZoom)
		{
			value = VariantUtils.CreateFrom(in _overridePreviewZoom);
			return true;
		}
		if (name == PropertyName._overrideSelectedPacketName)
		{
			value = VariantUtils.CreateFrom(in _overrideSelectedPacketName);
			return true;
		}
		if (name == PropertyName._overrideResolvedPacketName)
		{
			value = VariantUtils.CreateFrom(in _overrideResolvedPacketName);
			return true;
		}
		if (name == PropertyName._overridePreviewResourceId)
		{
			value = VariantUtils.CreateFrom(in _overridePreviewResourceId);
			return true;
		}
		if (name == PropertyName._selectedOverridePropertyChangeIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedOverridePropertyChangeIndex);
			return true;
		}
		if (name == PropertyName._overrideBaseCharacterPicker)
		{
			value = VariantUtils.CreateFrom(in _overrideBaseCharacterPicker);
			return true;
		}
		if (name == PropertyName._characterCatalogPicker)
		{
			value = VariantUtils.CreateFrom(in _characterCatalogPicker);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterViewportContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterPreviewBackground, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterGroundShade, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterPreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeCharacterPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._characterPreviewUsesExternalScriptFallback, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewHealthSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewHealthValueLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._draggingCharacterPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._characterPreviewPan, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._characterPreviewZoom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewCategoryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewModeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewHitpointsLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewPacketLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewResourceLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._componentListView, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._componentPatchListView, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spawnEventListView, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._dieEventListView, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._propertyChangeListView, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideDiffTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterSaveTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingCharacterOverride, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingPropertyChange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingOverrideResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideBaseCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideBasePacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideRuntimeScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideSpriteScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterOverrideWorkbench, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideViewportContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overridePreviewBackground, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideGroundShade, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overridePreviewCameraRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideBaseCharacterRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overriddenCharacterRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideBaseRuntimeCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overriddenRuntimeCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideContextLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideSourceLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overridePreviewStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideZoomLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideEffectSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideSafetySummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideSaveStateLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overridePropertyChangeList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectOverrideBaseCharacterButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideInvisibleCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideScaleSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideHitpointScaleSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideWalkSpeedMinSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideWalkSpeedMaxSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideAnimeSpeedMinSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideAnimeSpeedMaxSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideCanMowerMoveCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addOverridePropertyChangeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._duplicateOverridePropertyChangeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveOverridePropertyChangeUpButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveOverridePropertyChangeDownButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._removeOverridePropertyChangeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overridePropertyNameLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overridePropertyValueLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overridePropertyValueTypeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._draggingOverridePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._overridePreviewPan, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._overridePreviewZoom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._overrideSelectedPacketName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._overrideResolvedPacketName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._overridePreviewResourceId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedOverridePropertyChangeIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideBaseCharacterPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterCatalogPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingCharacter, Variant.From(in _editingCharacter));
		info.AddProperty(PropertyName._characterViewport, Variant.From(in _characterViewport));
		info.AddProperty(PropertyName._characterViewportContainer, Variant.From(in _characterViewportContainer));
		info.AddProperty(PropertyName._characterPreviewBackground, Variant.From(in _characterPreviewBackground));
		info.AddProperty(PropertyName._characterGroundShade, Variant.From(in _characterGroundShade));
		info.AddProperty(PropertyName._characterPreviewRoot, Variant.From(in _characterPreviewRoot));
		info.AddProperty(PropertyName._runtimeCharacterPreview, Variant.From(in _runtimeCharacterPreview));
		info.AddProperty(PropertyName._characterPreviewUsesExternalScriptFallback, Variant.From(in _characterPreviewUsesExternalScriptFallback));
		info.AddProperty(PropertyName._previewStatusLabel, Variant.From(in _previewStatusLabel));
		info.AddProperty(PropertyName._previewHealthSlider, Variant.From(in _previewHealthSlider));
		info.AddProperty(PropertyName._previewHealthValueLabel, Variant.From(in _previewHealthValueLabel));
		info.AddProperty(PropertyName._draggingCharacterPreview, Variant.From(in _draggingCharacterPreview));
		info.AddProperty(PropertyName._characterPreviewPan, Variant.From(in _characterPreviewPan));
		info.AddProperty(PropertyName._characterPreviewZoom, Variant.From(in _characterPreviewZoom));
		info.AddProperty(PropertyName._previewNameLabel, Variant.From(in _previewNameLabel));
		info.AddProperty(PropertyName._previewCategoryLabel, Variant.From(in _previewCategoryLabel));
		info.AddProperty(PropertyName._previewModeLabel, Variant.From(in _previewModeLabel));
		info.AddProperty(PropertyName._previewHitpointsLabel, Variant.From(in _previewHitpointsLabel));
		info.AddProperty(PropertyName._previewPacketLabel, Variant.From(in _previewPacketLabel));
		info.AddProperty(PropertyName._previewResourceLabel, Variant.From(in _previewResourceLabel));
		info.AddProperty(PropertyName._componentListView, Variant.From(in _componentListView));
		info.AddProperty(PropertyName._componentPatchListView, Variant.From(in _componentPatchListView));
		info.AddProperty(PropertyName._spawnEventListView, Variant.From(in _spawnEventListView));
		info.AddProperty(PropertyName._dieEventListView, Variant.From(in _dieEventListView));
		info.AddProperty(PropertyName._propertyChangeListView, Variant.From(in _propertyChangeListView));
		info.AddProperty(PropertyName._overrideDiffTree, Variant.From(in _overrideDiffTree));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._characterSaveTimer, Variant.From(in _characterSaveTimer));
		info.AddProperty(PropertyName._editingCharacterOverride, Variant.From(in _editingCharacterOverride));
		info.AddProperty(PropertyName._editingPropertyChange, Variant.From(in _editingPropertyChange));
		info.AddProperty(PropertyName._editingOverrideResource, Variant.From(in _editingOverrideResource));
		info.AddProperty(PropertyName._overrideBaseCharacter, Variant.From(in _overrideBaseCharacter));
		info.AddProperty(PropertyName._overrideBasePacket, Variant.From(in _overrideBasePacket));
		info.AddProperty(PropertyName._overrideRuntimeScene, Variant.From(in _overrideRuntimeScene));
		info.AddProperty(PropertyName._overrideSpriteScene, Variant.From(in _overrideSpriteScene));
		info.AddProperty(PropertyName._characterOverrideWorkbench, Variant.From(in _characterOverrideWorkbench));
		info.AddProperty(PropertyName._overrideViewport, Variant.From(in _overrideViewport));
		info.AddProperty(PropertyName._overrideViewportContainer, Variant.From(in _overrideViewportContainer));
		info.AddProperty(PropertyName._overridePreviewBackground, Variant.From(in _overridePreviewBackground));
		info.AddProperty(PropertyName._overrideGroundShade, Variant.From(in _overrideGroundShade));
		info.AddProperty(PropertyName._overridePreviewCameraRoot, Variant.From(in _overridePreviewCameraRoot));
		info.AddProperty(PropertyName._overrideBaseCharacterRoot, Variant.From(in _overrideBaseCharacterRoot));
		info.AddProperty(PropertyName._overriddenCharacterRoot, Variant.From(in _overriddenCharacterRoot));
		info.AddProperty(PropertyName._overrideBaseRuntimeCharacter, Variant.From(in _overrideBaseRuntimeCharacter));
		info.AddProperty(PropertyName._overriddenRuntimeCharacter, Variant.From(in _overriddenRuntimeCharacter));
		info.AddProperty(PropertyName._overrideContextLabel, Variant.From(in _overrideContextLabel));
		info.AddProperty(PropertyName._overrideSourceLabel, Variant.From(in _overrideSourceLabel));
		info.AddProperty(PropertyName._overridePreviewStatusLabel, Variant.From(in _overridePreviewStatusLabel));
		info.AddProperty(PropertyName._overrideZoomLabel, Variant.From(in _overrideZoomLabel));
		info.AddProperty(PropertyName._overrideEffectSummaryLabel, Variant.From(in _overrideEffectSummaryLabel));
		info.AddProperty(PropertyName._overrideSafetySummaryLabel, Variant.From(in _overrideSafetySummaryLabel));
		info.AddProperty(PropertyName._overrideSaveStateLabel, Variant.From(in _overrideSaveStateLabel));
		info.AddProperty(PropertyName._overridePropertyChangeList, Variant.From(in _overridePropertyChangeList));
		info.AddProperty(PropertyName._selectOverrideBaseCharacterButton, Variant.From(in _selectOverrideBaseCharacterButton));
		info.AddProperty(PropertyName._overrideInvisibleCheck, Variant.From(in _overrideInvisibleCheck));
		info.AddProperty(PropertyName._overrideScaleSpinBox, Variant.From(in _overrideScaleSpinBox));
		info.AddProperty(PropertyName._overrideHitpointScaleSpinBox, Variant.From(in _overrideHitpointScaleSpinBox));
		info.AddProperty(PropertyName._overrideWalkSpeedMinSpinBox, Variant.From(in _overrideWalkSpeedMinSpinBox));
		info.AddProperty(PropertyName._overrideWalkSpeedMaxSpinBox, Variant.From(in _overrideWalkSpeedMaxSpinBox));
		info.AddProperty(PropertyName._overrideAnimeSpeedMinSpinBox, Variant.From(in _overrideAnimeSpeedMinSpinBox));
		info.AddProperty(PropertyName._overrideAnimeSpeedMaxSpinBox, Variant.From(in _overrideAnimeSpeedMaxSpinBox));
		info.AddProperty(PropertyName._overrideCanMowerMoveCheck, Variant.From(in _overrideCanMowerMoveCheck));
		info.AddProperty(PropertyName._addOverridePropertyChangeButton, Variant.From(in _addOverridePropertyChangeButton));
		info.AddProperty(PropertyName._duplicateOverridePropertyChangeButton, Variant.From(in _duplicateOverridePropertyChangeButton));
		info.AddProperty(PropertyName._moveOverridePropertyChangeUpButton, Variant.From(in _moveOverridePropertyChangeUpButton));
		info.AddProperty(PropertyName._moveOverridePropertyChangeDownButton, Variant.From(in _moveOverridePropertyChangeDownButton));
		info.AddProperty(PropertyName._removeOverridePropertyChangeButton, Variant.From(in _removeOverridePropertyChangeButton));
		info.AddProperty(PropertyName._overridePropertyNameLineEdit, Variant.From(in _overridePropertyNameLineEdit));
		info.AddProperty(PropertyName._overridePropertyValueLineEdit, Variant.From(in _overridePropertyValueLineEdit));
		info.AddProperty(PropertyName._overridePropertyValueTypeLabel, Variant.From(in _overridePropertyValueTypeLabel));
		info.AddProperty(PropertyName._draggingOverridePreview, Variant.From(in _draggingOverridePreview));
		info.AddProperty(PropertyName._overridePreviewPan, Variant.From(in _overridePreviewPan));
		info.AddProperty(PropertyName._overridePreviewZoom, Variant.From(in _overridePreviewZoom));
		info.AddProperty(PropertyName._overrideSelectedPacketName, Variant.From(in _overrideSelectedPacketName));
		info.AddProperty(PropertyName._overrideResolvedPacketName, Variant.From(in _overrideResolvedPacketName));
		info.AddProperty(PropertyName._overridePreviewResourceId, Variant.From(in _overridePreviewResourceId));
		info.AddProperty(PropertyName._selectedOverridePropertyChangeIndex, Variant.From(in _selectedOverridePropertyChangeIndex));
		info.AddProperty(PropertyName._overrideBaseCharacterPicker, Variant.From(in _overrideBaseCharacterPicker));
		info.AddProperty(PropertyName._characterCatalogPicker, Variant.From(in _characterCatalogPicker));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingCharacter, out var value))
		{
			_editingCharacter = value.As<TowerDefenseCharacterConfig>();
		}
		if (info.TryGetProperty(PropertyName._characterViewport, out var value2))
		{
			_characterViewport = value2.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._characterViewportContainer, out var value3))
		{
			_characterViewportContainer = value3.As<SubViewportContainer>();
		}
		if (info.TryGetProperty(PropertyName._characterPreviewBackground, out var value4))
		{
			_characterPreviewBackground = value4.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._characterGroundShade, out var value5))
		{
			_characterGroundShade = value5.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName._characterPreviewRoot, out var value6))
		{
			_characterPreviewRoot = value6.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._runtimeCharacterPreview, out var value7))
		{
			_runtimeCharacterPreview = value7.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._characterPreviewUsesExternalScriptFallback, out var value8))
		{
			_characterPreviewUsesExternalScriptFallback = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._previewStatusLabel, out var value9))
		{
			_previewStatusLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewHealthSlider, out var value10))
		{
			_previewHealthSlider = value10.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._previewHealthValueLabel, out var value11))
		{
			_previewHealthValueLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._draggingCharacterPreview, out var value12))
		{
			_draggingCharacterPreview = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._characterPreviewPan, out var value13))
		{
			_characterPreviewPan = value13.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._characterPreviewZoom, out var value14))
		{
			_characterPreviewZoom = value14.As<float>();
		}
		if (info.TryGetProperty(PropertyName._previewNameLabel, out var value15))
		{
			_previewNameLabel = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewCategoryLabel, out var value16))
		{
			_previewCategoryLabel = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewModeLabel, out var value17))
		{
			_previewModeLabel = value17.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewHitpointsLabel, out var value18))
		{
			_previewHitpointsLabel = value18.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewPacketLabel, out var value19))
		{
			_previewPacketLabel = value19.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewResourceLabel, out var value20))
		{
			_previewResourceLabel = value20.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._componentListView, out var value21))
		{
			_componentListView = value21.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._componentPatchListView, out var value22))
		{
			_componentPatchListView = value22.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._spawnEventListView, out var value23))
		{
			_spawnEventListView = value23.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._dieEventListView, out var value24))
		{
			_dieEventListView = value24.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._propertyChangeListView, out var value25))
		{
			_propertyChangeListView = value25.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._overrideDiffTree, out var value26))
		{
			_overrideDiffTree = value26.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value27))
		{
			_updatingControls = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._characterSaveTimer, out var value28))
		{
			_characterSaveTimer = value28.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._editingCharacterOverride, out var value29))
		{
			_editingCharacterOverride = value29.As<TowerDefenseCharacterOverride>();
		}
		if (info.TryGetProperty(PropertyName._editingPropertyChange, out var value30))
		{
			_editingPropertyChange = value30.As<TowerDefenseCharacterPropertyChangeConfig>();
		}
		if (info.TryGetProperty(PropertyName._editingOverrideResource, out var value31))
		{
			_editingOverrideResource = value31.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._overrideBaseCharacter, out var value32))
		{
			_overrideBaseCharacter = value32.As<TowerDefenseCharacterConfig>();
		}
		if (info.TryGetProperty(PropertyName._overrideBasePacket, out var value33))
		{
			_overrideBasePacket = value33.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName._overrideRuntimeScene, out var value34))
		{
			_overrideRuntimeScene = value34.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName._overrideSpriteScene, out var value35))
		{
			_overrideSpriteScene = value35.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName._characterOverrideWorkbench, out var value36))
		{
			_characterOverrideWorkbench = value36.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._overrideViewport, out var value37))
		{
			_overrideViewport = value37.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._overrideViewportContainer, out var value38))
		{
			_overrideViewportContainer = value38.As<SubViewportContainer>();
		}
		if (info.TryGetProperty(PropertyName._overridePreviewBackground, out var value39))
		{
			_overridePreviewBackground = value39.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._overrideGroundShade, out var value40))
		{
			_overrideGroundShade = value40.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName._overridePreviewCameraRoot, out var value41))
		{
			_overridePreviewCameraRoot = value41.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._overrideBaseCharacterRoot, out var value42))
		{
			_overrideBaseCharacterRoot = value42.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._overriddenCharacterRoot, out var value43))
		{
			_overriddenCharacterRoot = value43.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._overrideBaseRuntimeCharacter, out var value44))
		{
			_overrideBaseRuntimeCharacter = value44.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._overriddenRuntimeCharacter, out var value45))
		{
			_overriddenRuntimeCharacter = value45.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._overrideContextLabel, out var value46))
		{
			_overrideContextLabel = value46.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._overrideSourceLabel, out var value47))
		{
			_overrideSourceLabel = value47.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._overridePreviewStatusLabel, out var value48))
		{
			_overridePreviewStatusLabel = value48.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._overrideZoomLabel, out var value49))
		{
			_overrideZoomLabel = value49.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._overrideEffectSummaryLabel, out var value50))
		{
			_overrideEffectSummaryLabel = value50.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._overrideSafetySummaryLabel, out var value51))
		{
			_overrideSafetySummaryLabel = value51.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._overrideSaveStateLabel, out var value52))
		{
			_overrideSaveStateLabel = value52.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._overridePropertyChangeList, out var value53))
		{
			_overridePropertyChangeList = value53.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._selectOverrideBaseCharacterButton, out var value54))
		{
			_selectOverrideBaseCharacterButton = value54.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._overrideInvisibleCheck, out var value55))
		{
			_overrideInvisibleCheck = value55.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._overrideScaleSpinBox, out var value56))
		{
			_overrideScaleSpinBox = value56.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._overrideHitpointScaleSpinBox, out var value57))
		{
			_overrideHitpointScaleSpinBox = value57.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._overrideWalkSpeedMinSpinBox, out var value58))
		{
			_overrideWalkSpeedMinSpinBox = value58.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._overrideWalkSpeedMaxSpinBox, out var value59))
		{
			_overrideWalkSpeedMaxSpinBox = value59.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._overrideAnimeSpeedMinSpinBox, out var value60))
		{
			_overrideAnimeSpeedMinSpinBox = value60.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._overrideAnimeSpeedMaxSpinBox, out var value61))
		{
			_overrideAnimeSpeedMaxSpinBox = value61.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._overrideCanMowerMoveCheck, out var value62))
		{
			_overrideCanMowerMoveCheck = value62.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._addOverridePropertyChangeButton, out var value63))
		{
			_addOverridePropertyChangeButton = value63.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._duplicateOverridePropertyChangeButton, out var value64))
		{
			_duplicateOverridePropertyChangeButton = value64.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveOverridePropertyChangeUpButton, out var value65))
		{
			_moveOverridePropertyChangeUpButton = value65.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveOverridePropertyChangeDownButton, out var value66))
		{
			_moveOverridePropertyChangeDownButton = value66.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._removeOverridePropertyChangeButton, out var value67))
		{
			_removeOverridePropertyChangeButton = value67.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._overridePropertyNameLineEdit, out var value68))
		{
			_overridePropertyNameLineEdit = value68.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._overridePropertyValueLineEdit, out var value69))
		{
			_overridePropertyValueLineEdit = value69.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._overridePropertyValueTypeLabel, out var value70))
		{
			_overridePropertyValueTypeLabel = value70.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._draggingOverridePreview, out var value71))
		{
			_draggingOverridePreview = value71.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._overridePreviewPan, out var value72))
		{
			_overridePreviewPan = value72.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._overridePreviewZoom, out var value73))
		{
			_overridePreviewZoom = value73.As<float>();
		}
		if (info.TryGetProperty(PropertyName._overrideSelectedPacketName, out var value74))
		{
			_overrideSelectedPacketName = value74.As<string>();
		}
		if (info.TryGetProperty(PropertyName._overrideResolvedPacketName, out var value75))
		{
			_overrideResolvedPacketName = value75.As<string>();
		}
		if (info.TryGetProperty(PropertyName._overridePreviewResourceId, out var value76))
		{
			_overridePreviewResourceId = value76.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._selectedOverridePropertyChangeIndex, out var value77))
		{
			_selectedOverridePropertyChangeIndex = value77.As<int>();
		}
		if (info.TryGetProperty(PropertyName._overrideBaseCharacterPicker, out var value78))
		{
			_overrideBaseCharacterPicker = value78.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._characterCatalogPicker, out var value79))
		{
			_characterCatalogPicker = value79.As<XWGameplayResourcePickerWindow>();
		}
	}
}
