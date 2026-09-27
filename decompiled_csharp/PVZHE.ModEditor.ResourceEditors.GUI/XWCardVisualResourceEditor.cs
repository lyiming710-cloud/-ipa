using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCardVisualResourceEditor.cs")]
public class XWCardVisualResourceEditor : XWGenericVisualResourceEditor
{
	private enum CardCharacterSource
	{
		Mod,
		BuiltIn
	}

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public static readonly StringName RenderCardPreview = "RenderCardPreview";

		public static readonly StringName BindCardPreviewSurface = "BindCardPreviewSurface";

		public static readonly StringName InitializeCardRuntimePreview = "InitializeCardRuntimePreview";

		public static readonly StringName BindCardInspectorSurface = "BindCardInspectorSurface";

		public static readonly StringName BindUnlockAndArmorEditors = "BindUnlockAndArmorEditors";

		public static readonly StringName GetSelectedUnlockIndex = "GetSelectedUnlockIndex";

		public static readonly StringName AddUnlockCondition = "AddUnlockCondition";

		public static readonly StringName ReplaceSelectedUnlockCondition = "ReplaceSelectedUnlockCondition";

		public static readonly StringName RemoveSelectedUnlockCondition = "RemoveSelectedUnlockCondition";

		public static readonly StringName MoveSelectedUnlockCondition = "MoveSelectedUnlockCondition";

		public static readonly StringName ReplaceCardUnlockConditions = "ReplaceCardUnlockConditions";

		public static readonly StringName AddArmor = "AddArmor";

		public static readonly StringName ReplaceSelectedArmor = "ReplaceSelectedArmor";

		public static readonly StringName RemoveSelectedArmor = "RemoveSelectedArmor";

		public static readonly StringName MoveSelectedArmor = "MoveSelectedArmor";

		public static readonly StringName ReplaceCardArmor = "ReplaceCardArmor";

		public static readonly StringName RefreshArmorList = "RefreshArmorList";

		public static readonly StringName BindCardBehaviorSurface = "BindCardBehaviorSurface";

		public static readonly StringName ShowRegistryBehaviorPicker = "ShowRegistryBehaviorPicker";

		public static readonly StringName BuildRegistryBehaviorPicker = "BuildRegistryBehaviorPicker";

		public static readonly StringName AddRegistryBehaviorFromPicker = "AddRegistryBehaviorFromPicker";

		public static readonly StringName AddCustomBehaviorId = "AddCustomBehaviorId";

		public static readonly StringName AddRegistryBehaviorId = "AddRegistryBehaviorId";

		public static readonly StringName RemoveSelectedRegistryBehavior = "RemoveSelectedRegistryBehavior";

		public static readonly StringName MoveSelectedRegistryBehavior = "MoveSelectedRegistryBehavior";

		public static readonly StringName ReplaceRegistryBehaviorIds = "ReplaceRegistryBehaviorIds";

		public static readonly StringName GetSelectedRegistryBehaviorIndex = "GetSelectedRegistryBehaviorIndex";

		public static readonly StringName OpenSelectedRegistryBehavior = "OpenSelectedRegistryBehavior";

		public static readonly StringName ShowInlineBehaviorSelector = "ShowInlineBehaviorSelector";

		public static readonly StringName RemoveSelectedInlineBehavior = "RemoveSelectedInlineBehavior";

		public static readonly StringName MoveSelectedInlineBehavior = "MoveSelectedInlineBehavior";

		public static readonly StringName ReplaceInlineBehaviors = "ReplaceInlineBehaviors";

		public static readonly StringName GetSelectedInlineBehaviorIndex = "GetSelectedInlineBehaviorIndex";

		public static readonly StringName OpenSelectedInlineBehavior = "OpenSelectedInlineBehavior";

		public static readonly StringName OpenInlineBehavior = "OpenInlineBehavior";

		public static readonly StringName RefreshBehaviorRegistryFromUser = "RefreshBehaviorRegistryFromUser";

		public static readonly StringName RefreshCardBehaviorLists = "RefreshCardBehaviorLists";

		public static readonly StringName BindCardEventSurface = "BindCardEventSurface";

		public static readonly StringName EditSelectedUnlockCondition = "EditSelectedUnlockCondition";

		public static readonly StringName ShowPacketEventSelector = "ShowPacketEventSelector";

		public static readonly StringName EnsurePacketEventSelectorWindow = "EnsurePacketEventSelectorWindow";

		public static readonly StringName AddPacketEventToTarget = "AddPacketEventToTarget";

		public static readonly StringName RemoveSelectedPacketEvent = "RemoveSelectedPacketEvent";

		public static readonly StringName MoveSelectedPacketEvent = "MoveSelectedPacketEvent";

		public static readonly StringName ReplacePacketEvents = "ReplacePacketEvents";

		public static readonly StringName EditSelectedPacketEvent = "EditSelectedPacketEvent";

		public static readonly StringName OpenPacketEventResource = "OpenPacketEventResource";

		public static readonly StringName GetPacketEventArray = "GetPacketEventArray";

		public static readonly StringName GetSelectedPacketEventIndex = "GetSelectedPacketEventIndex";

		public static readonly StringName OnCardPropertyEdited = "OnCardPropertyEdited";

		public static readonly StringName RefreshCardEditorFromHistory = "RefreshCardEditorFromHistory";

		public static readonly StringName SyncCardVector2HistoryControls = "SyncCardVector2HistoryControls";

		public static readonly StringName SyncCardVector2HistoryControl = "SyncCardVector2HistoryControl";

		public static readonly StringName SaveCardResource = "SaveCardResource";

		public static readonly StringName UpdateCardPreview = "UpdateCardPreview";

		public static readonly StringName CreatePacketOverride = "CreatePacketOverride";

		public static readonly StringName OpenPacketOverride = "OpenPacketOverride";

		public static readonly StringName ClearPacketOverride = "ClearPacketOverride";

		public static readonly StringName SetPacketOverride = "SetPacketOverride";

		public static readonly StringName UpdatePacketOverrideEntry = "UpdatePacketOverrideEntry";

		public static readonly StringName RefreshCardLists = "RefreshCardLists";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName BuildRuntimeStateCards = "BuildRuntimeStateCards";

		public static readonly StringName SetCardRuntimePreviewState = "SetCardRuntimePreviewState";

		public static readonly StringName BuildPacketTypeCards = "BuildPacketTypeCards";

		public static readonly StringName BuildSpawnMethodCards = "BuildSpawnMethodCards";

		public static readonly StringName ClearVisualCards = "ClearVisualCards";

		public static readonly StringName CreateVisualChoiceCard = "CreateVisualChoiceCard";

		public static readonly StringName SelectVisualCard = "SelectVisualCard";

		public static readonly StringName BindCardVector2Field = "BindCardVector2Field";

		public static readonly StringName RefreshCharacterBindingSelector = "RefreshCharacterBindingSelector";

		public static readonly StringName ShowCharacterSelectorWindow = "ShowCharacterSelectorWindow";

		public static readonly StringName EnsureCharacterResourcePicker = "EnsureCharacterResourcePicker";

		public static readonly StringName ClearCharacterBinding = "ClearCharacterBinding";

		public static readonly StringName SetCharacterBindingResource = "SetCharacterBindingResource";

		public static readonly StringName LoadCharacterConfig = "LoadCharacterConfig";

		public static readonly StringName IsCharacterConfigResourcePath = "IsCharacterConfigResourcePath";

		public static readonly StringName UpdateCharacterSourceLabel = "UpdateCharacterSourceLabel";

		public static readonly StringName GetCharacterSourceTag = "GetCharacterSourceTag";

		public static readonly StringName ResolveCharacterSource = "ResolveCharacterSource";

		public static readonly StringName IsBuiltInCharacterPath = "IsBuiltInCharacterPath";

		public static readonly StringName NormalizeResourcePath = "NormalizeResourcePath";

		public static readonly StringName GetCharacterKey = "GetCharacterKey";

		public static readonly StringName BuildCharacterDisplayName = "BuildCharacterDisplayName";

		public static readonly StringName CreatePanelStyle = "CreatePanelStyle";

		public static readonly StringName RefreshPacketShowPreview = "RefreshPacketShowPreview";

		public static readonly StringName ConfigurePacketShowPreview = "ConfigurePacketShowPreview";

		public static readonly StringName ApplyPacketShowPreviewIncremental = "ApplyPacketShowPreviewIncremental";

		public static readonly StringName RefreshPacketShowAnimationInPlace = "RefreshPacketShowAnimationInPlace";

		public static readonly StringName BuildPacketShowSpriteFingerprint = "BuildPacketShowSpriteFingerprint";

		public static readonly StringName BuildPacketShowSaveKeyFingerprint = "BuildPacketShowSaveKeyFingerprint";

		public static readonly StringName ResolveConfiguredPacketCooldown = "ResolveConfiguredPacketCooldown";

		public static readonly StringName ApplyPacketShowPreviewLayout = "ApplyPacketShowPreviewLayout";

		public static readonly StringName EnsurePacketPreviewCharacterSpriteRegistered = "EnsurePacketPreviewCharacterSpriteRegistered";

		public static readonly StringName IsAdobeAnimateSpriteScene = "IsAdobeAnimateSpriteScene";

		public static readonly StringName PositionPacketShowPreview = "PositionPacketShowPreview";

		public static readonly StringName OnCardInteractionGuiInput = "OnCardInteractionGuiInput";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName CanvasPointToControlLocal = "CanvasPointToControlLocal";

		public static readonly StringName IsCanvasPointInsideControl = "IsCanvasPointInsideControl";

		public static readonly StringName BeginCardPointerSession = "BeginCardPointerSession";

		public static readonly StringName HandleCardPointerMotion = "HandleCardPointerMotion";

		public static readonly StringName StartCardDragSession = "StartCardDragSession";

		public static readonly StringName CompleteCardPointerSession = "CompleteCardPointerSession";

		public static readonly StringName ClearCardPointerSession = "ClearCardPointerSession";

		public static readonly StringName EnsureCardDragGhost = "EnsureCardDragGhost";

		public static readonly StringName DisposeCardDragGhost = "DisposeCardDragGhost";

		public static readonly StringName UpdateCardDragTarget = "UpdateCardDragTarget";

		public static readonly StringName ShowCardDragTarget = "ShowCardDragTarget";

		public static readonly StringName HideCardDragTarget = "HideCardDragTarget";

		public static readonly StringName PlaceCardPreviewAtCell = "PlaceCardPreviewAtCell";

		public static readonly StringName CancelCardDragSession = "CancelCardDragSession";

		public static readonly StringName FinishCardDragVisuals = "FinishCardDragVisuals";

		public static readonly StringName ResetCardDragPreviewState = "ResetCardDragPreviewState";

		public static readonly StringName ShowCardPlacementMessage = "ShowCardPlacementMessage";

		public static readonly StringName OnCardBattleDropSurfaceResized = "OnCardBattleDropSurfaceResized";

		public static readonly StringName DrawCardBattleDropSurface = "DrawCardBattleDropSurface";

		public static readonly StringName QueueCardBattleDropSurfaceRedraw = "QueueCardBattleDropSurfaceRedraw";

		public static readonly StringName GetCardBattleGridRect = "GetCardBattleGridRect";

		public static readonly StringName GetCardBattleCellSize = "GetCardBattleCellSize";

		public static readonly StringName GetCardBattleCellRect = "GetCardBattleCellRect";

		public static readonly StringName GetCardBattleCellCenter = "GetCardBattleCellCenter";

		public static readonly StringName IsCardBattleCell = "IsCardBattleCell";

		public static readonly StringName PreviewCharacterAtCardCell = "PreviewCharacterAtCardCell";

		public static readonly StringName RestoreCardCharacterPreviewPosition = "RestoreCardCharacterPreviewPosition";

		public static readonly StringName UpdateCardBattleViewportLayout = "UpdateCardBattleViewportLayout";

		public static readonly StringName PreviewCardPlantAndCooldown = "PreviewCardPlantAndCooldown";

		public static readonly StringName ResetCardRuntimePreview = "ResetCardRuntimePreview";

		public static readonly StringName StartConfiguredCardCooldown = "StartConfiguredCardCooldown";

		public static readonly StringName ToggleCardRuntimeClock = "ToggleCardRuntimeClock";

		public static readonly StringName StopCardRuntimeClock = "StopCardRuntimeClock";

		public static readonly StringName EnsureCardRuntimeCooldownDuration = "EnsureCardRuntimeCooldownDuration";

		public static readonly StringName ResolveCardAvailabilityState = "ResolveCardAvailabilityState";

		public static readonly StringName ToggleCardRuntimeSelection = "ToggleCardRuntimeSelection";

		public static readonly StringName OnCardRuntimeMouseEntered = "OnCardRuntimeMouseEntered";

		public static readonly StringName OnCardRuntimeMouseExited = "OnCardRuntimeMouseExited";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName ApplyCardRuntimePreviewState = "ApplyCardRuntimePreviewState";

		public static readonly StringName RefreshCardRuntimeReadouts = "RefreshCardRuntimeReadouts";

		public static readonly StringName UpdateCardRuntimeCooldownVisual = "UpdateCardRuntimeCooldownVisual";

		public static readonly StringName RebuildCardCharacterPreview = "RebuildCardCharacterPreview";

		public static readonly StringName DisposeCardCharacterPreview = "DisposeCardCharacterPreview";

		public static readonly StringName BuildCardArmorFingerprint = "BuildCardArmorFingerprint";

		public static readonly StringName FindCardRuntimeCharacter = "FindCardRuntimeCharacter";

		public static readonly StringName SetCardCharacterMissing = "SetCardCharacterMissing";

		public static readonly StringName SetPacketShowPreviewMissing = "SetPacketShowPreviewMissing";

		public static readonly StringName GetEffectiveCostText = "GetEffectiveCostText";

		public static readonly StringName GetEffectiveCost = "GetEffectiveCost";

		public static readonly StringName GetEffectiveCooldownText = "GetEffectiveCooldownText";

		public static readonly StringName BuildCardValueOverrideSummary = "BuildCardValueOverrideSummary";

		public static readonly StringName GetPreviewValueSourceLabel = "GetPreviewValueSourceLabel";

		public static readonly StringName GetPacketTypeColor = "GetPacketTypeColor";

		public static readonly StringName GetPacketTexturePath = "GetPacketTexturePath";

		public static readonly StringName GetPacketTypeDisplayName = "GetPacketTypeDisplayName";

		public static readonly StringName FormatResource = "FormatResource";

		public static readonly StringName FormatCharacterBinding = "FormatCharacterBinding";

		public static readonly StringName FormatVariant = "FormatVariant";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName CardRuntimeReadoutRefreshCount = "CardRuntimeReadoutRefreshCount";

		public static readonly StringName CardCharacterPreviewBuildCount = "CardCharacterPreviewBuildCount";

		public static readonly StringName CardPacketShowPreviewBuildCount = "CardPacketShowPreviewBuildCount";

		public static readonly StringName CardPacketShowSpriteRebuildCount = "CardPacketShowSpriteRebuildCount";

		public static readonly StringName IsCardDragActive = "IsCardDragActive";

		public static readonly StringName IsCardDragTargetValid = "IsCardDragTargetValid";

		public static readonly StringName HasCardDragGhost = "HasCardDragGhost";

		public static readonly StringName CardDragPreviewCell = "CardDragPreviewCell";

		public static readonly StringName CardPlacedPreviewCell = "CardPlacedPreviewCell";

		public static readonly StringName CardDragPreviewPlacementCount = "CardDragPreviewPlacementCount";

		public static readonly StringName CardDragRejectedCount = "CardDragRejectedCount";

		public static readonly StringName CardDragCancelCount = "CardDragCancelCount";

		public static readonly StringName CardDragTargetColor = "CardDragTargetColor";

		public static readonly StringName RuntimeStateVisualCardCount = "RuntimeStateVisualCardCount";

		public static readonly StringName IsCardPreviewRendering = "IsCardPreviewRendering";

		public static readonly StringName _editingPacket = "_editingPacket";

		public static readonly StringName _previewNameLabel = "_previewNameLabel";

		public static readonly StringName _inlineSaveKeyLineEdit = "_inlineSaveKeyLineEdit";

		public static readonly StringName _inlineAnimationLineEdit = "_inlineAnimationLineEdit";

		public static readonly StringName _inlineCostSpinBox = "_inlineCostSpinBox";

		public static readonly StringName _inlineCooldownSpinBox = "_inlineCooldownSpinBox";

		public static readonly StringName _inlineStartingCooldownSpinBox = "_inlineStartingCooldownSpinBox";

		public static readonly StringName _previewKeyLabel = "_previewKeyLabel";

		public static readonly StringName _previewCostLabel = "_previewCostLabel";

		public static readonly StringName _previewCooldownLabel = "_previewCooldownLabel";

		public static readonly StringName _previewTypeLabel = "_previewTypeLabel";

		public static readonly StringName _previewCharacterLabel = "_previewCharacterLabel";

		public static readonly StringName _previewAnimationLabel = "_previewAnimationLabel";

		public static readonly StringName _valueOverrideBadge = "_valueOverrideBadge";

		public static readonly StringName _battleStatusLabel = "_battleStatusLabel";

		public static readonly StringName _cooldownPercentLabel = "_cooldownPercentLabel";

		public static readonly StringName _runtimeClockLabel = "_runtimeClockLabel";

		public static readonly StringName _runtimeStateCards = "_runtimeStateCards";

		public static readonly StringName _cooldownProgressSlider = "_cooldownProgressSlider";

		public static readonly StringName _previewSunSpinBox = "_previewSunSpinBox";

		public static readonly StringName _mobilePreviewCheck = "_mobilePreviewCheck";

		public static readonly StringName _cardInteractionButton = "_cardInteractionButton";

		public static readonly StringName _pauseRuntimeButton = "_pauseRuntimeButton";

		public static readonly StringName _cardFrame = "_cardFrame";

		public static readonly StringName _cardPacketShowPreviewRoot = "_cardPacketShowPreviewRoot";

		public static readonly StringName _cardPacketShowPreview = "_cardPacketShowPreview";

		public static readonly StringName _cardPacketShowSpriteFingerprint = "_cardPacketShowSpriteFingerprint";

		public static readonly StringName _cardPacketShowAnimationFingerprint = "_cardPacketShowAnimationFingerprint";

		public static readonly StringName _cardPacketShowSaveKeyFingerprint = "_cardPacketShowSaveKeyFingerprint";

		public static readonly StringName _cardPacketShowPreviewBuildCount = "_cardPacketShowPreviewBuildCount";

		public static readonly StringName _cardPacketShowSpriteRebuildCount = "_cardPacketShowSpriteRebuildCount";

		public static readonly StringName _cardPreviewRefreshCommitted = "_cardPreviewRefreshCommitted";

		public static readonly StringName _cardCharacterViewportContainer = "_cardCharacterViewportContainer";

		public static readonly StringName _cardCharacterViewport = "_cardCharacterViewport";

		public static readonly StringName _cardGameBackground = "_cardGameBackground";

		public static readonly StringName _cardBattleTint = "_cardBattleTint";

		public static readonly StringName _cardCharacterPreviewRoot = "_cardCharacterPreviewRoot";

		public static readonly StringName _cardBattleDropSurface = "_cardBattleDropSurface";

		public static readonly StringName _cardDragTargetHighlight = "_cardDragTargetHighlight";

		public static readonly StringName _cardDragPacketGhost = "_cardDragPacketGhost";

		public static readonly StringName _cardCharacterPreview = "_cardCharacterPreview";

		public static readonly StringName _cardCharacterMissingLabel = "_cardCharacterMissingLabel";

		public static readonly StringName _cardImageMissingLabel = "_cardImageMissingLabel";

		public static readonly StringName _eventSummaryLabel = "_eventSummaryLabel";

		public static readonly StringName _behaviorSummaryLabel = "_behaviorSummaryLabel";

		public static readonly StringName _registryBehaviorStatusLabel = "_registryBehaviorStatusLabel";

		public static readonly StringName _registryBehaviorList = "_registryBehaviorList";

		public static readonly StringName _inlineBehaviorList = "_inlineBehaviorList";

		public static readonly StringName _customBehaviorIdEdit = "_customBehaviorIdEdit";

		public static readonly StringName _registryBehaviorPicker = "_registryBehaviorPicker";

		public static readonly StringName _pressedActionsList = "_pressedActionsList";

		public static readonly StringName _useSucceededActionsList = "_useSucceededActionsList";

		public static readonly StringName _unlockConditionList = "_unlockConditionList";

		public static readonly StringName _armorList = "_armorList";

		public static readonly StringName _unlockConditionPicker = "_unlockConditionPicker";

		public static readonly StringName _characterBindingButton = "_characterBindingButton";

		public static readonly StringName _characterSourceLabel = "_characterSourceLabel";

		public static readonly StringName _overrideStatus = "_overrideStatus";

		public static readonly StringName _createOverrideButton = "_createOverrideButton";

		public static readonly StringName _openOverrideButton = "_openOverrideButton";

		public static readonly StringName _clearOverrideButton = "_clearOverrideButton";

		public static readonly StringName _characterResourcePicker = "_characterResourcePicker";

		public static readonly StringName _packetEventSelectorWindow = "_packetEventSelectorWindow";

		public static readonly StringName _packetEventTargetProperty = "_packetEventTargetProperty";

		public static readonly StringName _behaviorRegistryRevision = "_behaviorRegistryRevision";

		public static readonly StringName _behaviorRegistryPollAccumulator = "_behaviorRegistryPollAccumulator";

		public static readonly StringName _selectedRegistryBehaviorIndex = "_selectedRegistryBehaviorIndex";

		public static readonly StringName _selectedInlineBehaviorIndex = "_selectedInlineBehaviorIndex";

		public static readonly StringName _selectedEventPressIndex = "_selectedEventPressIndex";

		public static readonly StringName _selectedEventPlantIndex = "_selectedEventPlantIndex";

		public static readonly StringName _selectedUnlockIndex = "_selectedUnlockIndex";

		public static readonly StringName _selectedArmorIndex = "_selectedArmorIndex";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _cardRuntimePreviewState = "_cardRuntimePreviewState";

		public static readonly StringName _cardRuntimeCooldownProgress = "_cardRuntimeCooldownProgress";

		public static readonly StringName _cardRuntimeCooldownDuration = "_cardRuntimeCooldownDuration";

		public static readonly StringName _cardRuntimeCooldownRemaining = "_cardRuntimeCooldownRemaining";

		public static readonly StringName _cardRuntimeCooldownRunning = "_cardRuntimeCooldownRunning";

		public static readonly StringName _cardRuntimeHovered = "_cardRuntimeHovered";

		public static readonly StringName _cardRuntimeMobile = "_cardRuntimeMobile";

		public static readonly StringName _cardRuntimeReadoutAccumulator = "_cardRuntimeReadoutAccumulator";

		public static readonly StringName _cardRuntimeReadoutRefreshCount = "_cardRuntimeReadoutRefreshCount";

		public static readonly StringName _cardCharacterPreviewConfig = "_cardCharacterPreviewConfig";

		public static readonly StringName _cardCharacterPreviewArmorFingerprint = "_cardCharacterPreviewArmorFingerprint";

		public static readonly StringName _cardCharacterPreviewInitialized = "_cardCharacterPreviewInitialized";

		public static readonly StringName _cardCharacterPreviewBuildCount = "_cardCharacterPreviewBuildCount";

		public static readonly StringName _cardDragCandidate = "_cardDragCandidate";

		public static readonly StringName _cardDragActive = "_cardDragActive";

		public static readonly StringName _cardDragTouch = "_cardDragTouch";

		public static readonly StringName _cardDragTouchIndex = "_cardDragTouchIndex";

		public static readonly StringName _cardDragStartGlobalPosition = "_cardDragStartGlobalPosition";

		public static readonly StringName _cardDragPointerGlobalPosition = "_cardDragPointerGlobalPosition";

		public static readonly StringName _cardDragPreviewCell = "_cardDragPreviewCell";

		public static readonly StringName _cardPlacedPreviewCell = "_cardPlacedPreviewCell";

		public static readonly StringName _cardDragTargetValid = "_cardDragTargetValid";

		public static readonly StringName _cardHasPlacedPreview = "_cardHasPlacedPreview";

		public static readonly StringName _cardDragPreviewPlacementCount = "_cardDragPreviewPlacementCount";

		public static readonly StringName _cardDragRejectedCount = "_cardDragRejectedCount";

		public static readonly StringName _cardDragCancelCount = "_cardDragCancelCount";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string EditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCardVisualEditorLayout.tscn";

	private const string CardInspectorScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCardInspectorSurface.tscn";

	private const string PacketEventSelectorScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketEventSelectorWindow.tscn";

	private const string VisualChoiceCardScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn";

	private const string BehaviorIconPath = "res://addons/ModEditor/Icons/GraphEdit.svg";

	private const string BehaviorWarningIconPath = "res://addons/ModEditor/Icons/NodeWarning.svg";

	private static PackedScene _editorLayoutScene;

	private static PackedScene _cardInspectorScene;

	private static PackedScene _packetEventSelectorScene;

	private static PackedScene _visualChoiceCardScene;

	private static Texture2D _behaviorIcon;

	private static Texture2D _behaviorWarningIcon;

	private TowerDefensePacketConfig _editingPacket;

	private XWVisualPropertyBinding _cardPropertyBinding;

	private LineEdit _previewNameLabel;

	private LineEdit _inlineSaveKeyLineEdit;

	private LineEdit _inlineAnimationLineEdit;

	private SpinBox _inlineCostSpinBox;

	private SpinBox _inlineCooldownSpinBox;

	private SpinBox _inlineStartingCooldownSpinBox;

	private Label _previewKeyLabel;

	private Label _previewCostLabel;

	private Label _previewCooldownLabel;

	private Label _previewTypeLabel;

	private Label _previewCharacterLabel;

	private Label _previewAnimationLabel;

	private Label _valueOverrideBadge;

	private Label _battleStatusLabel;

	private Label _cooldownPercentLabel;

	private Label _runtimeClockLabel;

	private HFlowContainer _runtimeStateCards;

	private HSlider _cooldownProgressSlider;

	private SpinBox _previewSunSpinBox;

	private CheckButton _mobilePreviewCheck;

	private Button _cardInteractionButton;

	private Button _pauseRuntimeButton;

	private PanelContainer _cardFrame;

	private Control _cardPacketShowPreviewRoot;

	private TowerDefenseInGamePacketShow _cardPacketShowPreview;

	private string _cardPacketShowSpriteFingerprint = "";

	private string _cardPacketShowAnimationFingerprint = "";

	private string _cardPacketShowSaveKeyFingerprint = "";

	private int _cardPacketShowPreviewBuildCount;

	private int _cardPacketShowSpriteRebuildCount;

	private bool _cardPreviewRefreshCommitted = true;

	private SubViewportContainer _cardCharacterViewportContainer;

	private SubViewport _cardCharacterViewport;

	private TextureRect _cardGameBackground;

	private ColorRect _cardBattleTint;

	private Node2D _cardCharacterPreviewRoot;

	private Control _cardBattleDropSurface;

	private ColorRect _cardDragTargetHighlight;

	private TowerDefenseInGamePacketShow _cardDragPacketGhost;

	private TowerDefenseCharacter _cardCharacterPreview;

	private XWGameplayLogicPreviewSafety _cardCharacterPreviewSafety;

	private Label _cardCharacterMissingLabel;

	private Label _cardImageMissingLabel;

	private Label _eventSummaryLabel;

	private Label _behaviorSummaryLabel;

	private Label _registryBehaviorStatusLabel;

	private ItemList _registryBehaviorList;

	private ItemList _inlineBehaviorList;

	private LineEdit _customBehaviorIdEdit;

	private PopupMenu _registryBehaviorPicker;

	private ItemList _pressedActionsList;

	private ItemList _useSucceededActionsList;

	private ItemList _unlockConditionList;

	private ItemList _armorList;

	private XWResourcePicker _unlockConditionPicker;

	private Button _characterBindingButton;

	private Label _characterSourceLabel;

	private Label _overrideStatus;

	private Button _createOverrideButton;

	private Button _openOverrideButton;

	private Button _clearOverrideButton;

	private XWGameplayResourcePickerWindow _characterResourcePicker;

	private XWPacketEventSelectorWindow _packetEventSelectorWindow;

	private string _packetEventTargetProperty = "pressedActions";

	private readonly List<StringName> _registryBehaviorPickerIds = new List<StringName>();

	private ulong _behaviorRegistryRevision;

	private double _behaviorRegistryPollAccumulator;

	private int _selectedRegistryBehaviorIndex = -1;

	private int _selectedInlineBehaviorIndex = -1;

	private int _selectedEventPressIndex = -1;

	private int _selectedEventPlantIndex = -1;

	private int _selectedUnlockIndex = -1;

	private int _selectedArmorIndex = -1;

	private bool _updatingControls;

	private int _cardRuntimePreviewState;

	private float _cardRuntimeCooldownProgress;

	private double _cardRuntimeCooldownDuration;

	private double _cardRuntimeCooldownRemaining;

	private bool _cardRuntimeCooldownRunning;

	private bool _cardRuntimeHovered;

	private bool _cardRuntimeMobile;

	private double _cardRuntimeReadoutAccumulator;

	private int _cardRuntimeReadoutRefreshCount;

	private TowerDefenseCharacterConfig _cardCharacterPreviewConfig;

	private string _cardCharacterPreviewArmorFingerprint = "";

	private bool _cardCharacterPreviewInitialized;

	private int _cardCharacterPreviewBuildCount;

	private bool _cardDragCandidate;

	private bool _cardDragActive;

	private bool _cardDragTouch;

	private int _cardDragTouchIndex = -1;

	private Vector2 _cardDragStartGlobalPosition;

	private Vector2 _cardDragPointerGlobalPosition;

	private Vector2I _cardDragPreviewCell = new Vector2I(-1, -1);

	private Vector2I _cardPlacedPreviewCell = new Vector2I(-1, -1);

	private bool _cardDragTargetValid;

	private bool _cardHasPlacedPreview;

	private int _cardDragPreviewPlacementCount;

	private int _cardDragRejectedCount;

	private int _cardDragCancelCount;

	private const double CardRuntimeReadoutInterval = 0.1;

	private const float CardDragStartDistance = 10f;

	private const int CardBattleColumns = 9;

	private const int CardBattleRows = 5;

	private static readonly Color CardDragValidColor = new Color(0.32f, 1f, 0.38f, 0.34f);

	private static readonly Color CardDragInvalidColor = new Color(1f, 0.24f, 0.18f, 0.38f);

	private const string BuiltInCharacterRootPath = "Asset/Anime/Character";

	public int CardRuntimeReadoutRefreshCount => _cardRuntimeReadoutRefreshCount;

	public int CardCharacterPreviewBuildCount => _cardCharacterPreviewBuildCount;

	public int CardPacketShowPreviewBuildCount => _cardPacketShowPreviewBuildCount;

	public int CardPacketShowSpriteRebuildCount => _cardPacketShowSpriteRebuildCount;

	public bool IsCardDragActive => _cardDragActive;

	public bool IsCardDragTargetValid
	{
		get
		{
			if (_cardDragActive)
			{
				return _cardDragTargetValid;
			}
			return false;
		}
	}

	public bool HasCardDragGhost
	{
		get
		{
			if (_cardDragActive && GodotObject.IsInstanceValid(_cardDragPacketGhost))
			{
				return _cardDragPacketGhost.Visible;
			}
			return false;
		}
	}

	public Vector2I CardDragPreviewCell => _cardDragPreviewCell;

	public Vector2I CardPlacedPreviewCell => _cardPlacedPreviewCell;

	public int CardDragPreviewPlacementCount => _cardDragPreviewPlacementCount;

	public int CardDragRejectedCount => _cardDragRejectedCount;

	public int CardDragCancelCount => _cardDragCancelCount;

	public Color CardDragTargetColor
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_cardDragTargetHighlight))
			{
				return Colors.Transparent;
			}
			return _cardDragTargetHighlight.Color;
		}
	}

	public int RuntimeStateVisualCardCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_runtimeStateCards))
			{
				return 0;
			}
			return _runtimeStateCards.GetChildCount();
		}
	}

	public bool IsCardPreviewRendering
	{
		get
		{
			if (GodotObject.IsInstanceValid(_cardCharacterViewport))
			{
				return _cardCharacterViewport.RenderTargetUpdateMode != SubViewport.UpdateMode.Disabled;
			}
			return false;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		EnableVisibilityGatedProcessing();
		RequestVisibilityGatedProcessing(requested: false);
		SetProcessInput(IsVisibleInTree());
	}

	public override void _ExitTree()
	{
		CancelCardDragSession(countCancellation: false, "编辑器已关闭");
		SetProcessInput(enable: false);
		DisposeCardDragGhost();
		StopCardRuntimeClock();
		DisposeCardCharacterPreview();
		_cardCharacterPreviewBuildCount = 0;
		_cardPropertyBinding?.Dispose();
		_cardPropertyBinding = null;
		_registryBehaviorPickerIds.Clear();
		_behaviorRegistryRevision = 0uL;
		_behaviorRegistryPollAccumulator = 0.0;
		_cardPacketShowPreview = null;
		_cardPacketShowSpriteFingerprint = "";
		_cardPacketShowAnimationFingerprint = "";
		_cardPacketShowSaveKeyFingerprint = "";
		_cardPacketShowPreviewBuildCount = 0;
		_cardPacketShowSpriteRebuildCount = 0;
		_cardPreviewRefreshCommitted = true;
		_editingPacket = null;
		base._ExitTree();
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		base.OnVisualEditorVisibilityChanged(visible);
		if (!visible)
		{
			CancelCardDragSession(_cardDragCandidate || _cardDragActive, "已切换页面，拖拽预览已取消");
		}
		SetProcessInput(visible);
		if (GodotObject.IsInstanceValid(_cardCharacterViewport))
		{
			_cardCharacterViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode)(visible ? 4 : 0);
		}
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		CancelCardDragSession(countCancellation: false, "");
		DisposeCardDragGhost();
		StopCardRuntimeClock();
		DisposeCardCharacterPreview();
		_cardPropertyBinding?.Dispose();
		_cardPropertyBinding = null;
		_registryBehaviorPickerIds.Clear();
		_behaviorRegistryRevision = 0uL;
		_behaviorRegistryPollAccumulator = 0.0;
		_cardPacketShowPreview = null;
		_cardPacketShowSpriteFingerprint = "";
		_cardPacketShowAnimationFingerprint = "";
		_cardPacketShowSaveKeyFingerprint = "";
		_cardPacketShowPreviewBuildCount = 0;
		_cardPacketShowSpriteRebuildCount = 0;
		_cardPreviewRefreshCommitted = true;
		_editingPacket = null;
		if (CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig)
		{
			_editingPacket = towerDefensePacketConfig;
			_cardPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnCardPropertyEdited);
			RenderCardPreview(towerDefensePacketConfig);
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (resource?.GetType() != typeof(TowerDefensePacketConfig))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private void RenderCardPreview(TowerDefensePacketConfig packet)
	{
		if (CanvasGrid == null)
		{
			return;
		}
		CanvasGrid.Columns = 1;
		if (_editorLayoutScene == null)
		{
			_editorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCardVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _editorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(vBoxContainer))
		{
			CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
			BindCardPreviewSurface(vBoxContainer, packet);
			if (_cardInspectorScene == null)
			{
				_cardInspectorScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCardInspectorSurface.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer2 = _cardInspectorScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer2))
			{
				vBoxContainer.GetNode<VBoxContainer>("%InspectorHost").AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
				BindCardInspectorSurface(vBoxContainer2, packet);
			}
			BindCardBehaviorSurface(vBoxContainer, packet);
			BindCardEventSurface(vBoxContainer, packet);
			AddSummaryRows(packet);
		}
	}

	private void BindCardPreviewSurface(VBoxContainer root, TowerDefensePacketConfig packet)
	{
		_cardFrame = root.GetNode<PanelContainer>("%CardFrame");
		_previewCostLabel = root.GetNode<Label>("%CostLabel");
		_previewNameLabel = root.GetNode<LineEdit>("%CardNameLabel");
		_cardPropertyBinding.BindText(_previewNameLabel, packet, "name", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
		_inlineSaveKeyLineEdit = root.GetNode<LineEdit>("%InlineSaveKeyLineEdit");
		_cardPropertyBinding.BindText(_inlineSaveKeyLineEdit, packet, "saveKey", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
		_inlineCostSpinBox = root.GetNode<SpinBox>("%InlineCostSpinBox");
		_cardPropertyBinding.BindNumber(_inlineCostSpinBox, packet, "overrideCost", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
		_inlineCooldownSpinBox = root.GetNode<SpinBox>("%InlineCooldownSpinBox");
		_cardPropertyBinding.BindNumber(_inlineCooldownSpinBox, packet, "overridePacketCooldown", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
		_inlineStartingCooldownSpinBox = root.GetNode<SpinBox>("%InlineStartingCooldownSpinBox");
		_cardPropertyBinding.BindNumber(_inlineStartingCooldownSpinBox, packet, "overrideStartingCooldown", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
		_inlineAnimationLineEdit = root.GetNode<LineEdit>("%InlineAnimationLineEdit");
		_cardPropertyBinding.BindText(_inlineAnimationLineEdit, packet, "packetAnimeClip", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
		root.GetNode<Button>("%InlineCharacterBindingButton").Pressed += ShowCharacterSelectorWindow;
		_cardPacketShowPreviewRoot = root.GetNode<Control>("%PacketShowRoot");
		_cardPacketShowPreviewRoot.Resized += PositionPacketShowPreview;
		_cardInteractionButton = root.GetNode<Button>("%PacketInteractionButton");
		_cardInteractionButton.MouseEntered += OnCardRuntimeMouseEntered;
		_cardInteractionButton.MouseExited += OnCardRuntimeMouseExited;
		_cardInteractionButton.Pressed += ToggleCardRuntimeSelection;
		_cardInteractionButton.GuiInput += OnCardInteractionGuiInput;
		_cardCharacterViewportContainer = root.GetNode<SubViewportContainer>("%CharacterViewportContainer");
		_cardCharacterViewport = root.GetNode<SubViewport>("%CharacterViewport");
		_cardCharacterViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode)(IsVisibleInTree() ? 4 : 0);
		_cardGameBackground = root.GetNode<TextureRect>("%GameBackground");
		_cardBattleTint = root.GetNode<ColorRect>("%BattleTint");
		_cardCharacterPreviewRoot = root.GetNode<Node2D>("%CharacterPreviewRoot");
		_cardBattleDropSurface = root.GetNode<Control>("%CardBattleDropSurface");
		_cardBattleDropSurface.Draw += DrawCardBattleDropSurface;
		_cardBattleDropSurface.Resized += OnCardBattleDropSurfaceResized;
		_cardDragTargetHighlight = root.GetNode<ColorRect>("%CardDragTargetHighlight");
		_cardCharacterViewportContainer.Resized += UpdateCardBattleViewportLayout;
		_cardCharacterMissingLabel = root.GetNode<Label>("%CharacterMissingLabel");
		root.GetNode<Button>("%PreviewPlantButton").Pressed += PreviewCardPlantAndCooldown;
		root.GetNode<Button>("%ResetRuntimeButton").Pressed += ResetCardRuntimePreview;
		_cardImageMissingLabel = root.GetNode<Label>("%ImageMissingLabel");
		_battleStatusLabel = root.GetNode<Label>("%BattleStatusLabel");
		_cooldownPercentLabel = root.GetNode<Label>("%CooldownPercentLabel");
		_runtimeClockLabel = root.GetNode<Label>("%RuntimeClockLabel");
		_runtimeStateCards = root.GetNode<HFlowContainer>("%RuntimeStateCards");
		BuildRuntimeStateCards();
		_cooldownProgressSlider = root.GetNode<HSlider>("%CooldownProgressSlider");
		_cooldownProgressSlider.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				_cardRuntimeCooldownProgress = (float)value;
				EnsureCardRuntimeCooldownDuration();
				_cardRuntimeCooldownRemaining = _cardRuntimeCooldownDuration * (double)_cardRuntimeCooldownProgress;
				if (_cardRuntimeCooldownProgress > 0.001f)
				{
					_cardRuntimePreviewState = 2;
				}
				else if (_cardRuntimePreviewState == 2)
				{
					StopCardRuntimeClock();
					_cardRuntimePreviewState = ResolveCardAvailabilityState();
				}
				ApplyCardRuntimePreviewState();
			}
		};
		_previewSunSpinBox = root.GetNode<SpinBox>("%PreviewSunSpinBox");
		_previewSunSpinBox.ValueChanged += (double _) =>
		{
			if (!_updatingControls)
			{
				if (_cardRuntimePreviewState != 2 && _cardRuntimePreviewState != 4)
				{
					_cardRuntimePreviewState = ResolveCardAvailabilityState();
				}
				ApplyCardRuntimePreviewState();
			}
		};
		_mobilePreviewCheck = root.GetNode<CheckButton>("%MobilePreviewCheck");
		_mobilePreviewCheck.Toggled += (bool enabled) =>
		{
			if (!_updatingControls)
			{
				_cardRuntimeMobile = enabled;
				RefreshPacketShowPreview(_editingPacket);
			}
		};
		_pauseRuntimeButton = root.GetNode<Button>("%PauseRuntimeButton");
		_pauseRuntimeButton.Pressed += ToggleCardRuntimeClock;
		_previewKeyLabel = root.GetNode<Label>("%KeyLabel");
		_previewTypeLabel = root.GetNode<Label>("%TypeLabel");
		_previewCooldownLabel = root.GetNode<Label>("%CooldownLabel");
		_previewCharacterLabel = root.GetNode<Label>("%CharacterLabel");
		_previewAnimationLabel = root.GetNode<Label>("%AnimationLabel");
		_valueOverrideBadge = root.GetNode<Label>("%ValueOverrideBadge");
		InitializeCardRuntimePreview();
		CallDeferred("UpdateCardBattleViewportLayout");
	}

	private void InitializeCardRuntimePreview()
	{
		ResetCardDragPreviewState();
		_cardRuntimePreviewState = 0;
		_cardRuntimeCooldownProgress = 0f;
		_cardRuntimeCooldownDuration = 0.0;
		_cardRuntimeCooldownRemaining = 0.0;
		_cardRuntimeCooldownRunning = false;
		_cardRuntimeHovered = false;
		_cardRuntimeReadoutAccumulator = 0.0;
		_cardRuntimeReadoutRefreshCount = 0;
		RequestVisibilityGatedProcessing(requested: false);
		bool updatingControls = _updatingControls;
		_updatingControls = true;
		try
		{
			if (GodotObject.IsInstanceValid(_previewSunSpinBox))
			{
				_previewSunSpinBox.Value = Math.Max(500L, GetEffectiveCost(_editingPacket));
			}
		}
		finally
		{
			_updatingControls = updatingControls;
		}
		UpdateCardPreview();
		ApplyCardRuntimePreviewState();
	}

	private void BindCardInspectorSurface(VBoxContainer root, TowerDefensePacketConfig packet)
	{
		if (!GodotObject.IsInstanceValid(root) || packet == null)
		{
			return;
		}
		bool updatingControls = _updatingControls;
		_updatingControls = true;
		try
		{
			_cardPropertyBinding.BindText(root.GetNode<LineEdit>("%ResourceNameEdit"), packet, "resource_name", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindToggle(root.GetNode<CheckButton>("%LocalToSceneCheck"), packet, "resource_local_to_scene", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindText(root.GetNode<LineEdit>("%SaveKeyLineEdit"), packet, "saveKey", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindText(root.GetNode<LineEdit>("%NameLineEdit"), packet, "name", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			BuildPacketTypeCards(root.GetNode<HFlowContainer>("%TypeCards"), packet.type);
			_cardPropertyBinding.BindToggle(root.GetNode<CheckButton>("%PacketFlipCheck"), packet, "packetFlip", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_characterBindingButton = root.GetNode<Button>("%CharacterBindingWindowButton");
			_characterBindingButton.Pressed += ShowCharacterSelectorWindow;
			root.GetNode<Button>("%ClearCharacterBindingButton").Pressed += ClearCharacterBinding;
			_characterSourceLabel = root.GetNode<Label>("%CharacterSourceLabel");
			_cardPropertyBinding.BindNumber(root.GetNode<SpinBox>("%CostSpinBox"), packet, "overrideCost", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindNumber(root.GetNode<SpinBox>("%CooldownSpinBox"), packet, "overridePacketCooldown", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindNumber(root.GetNode<SpinBox>("%StartingCooldownSpinBox"), packet, "overrideStartingCooldown", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindToggle(root.GetNode<CheckButton>("%CanChangeCostCheck"), packet, "canChangeCost", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			BuildSpawnMethodCards(root.GetNode<HFlowContainer>("%SpawnMethodCards"), packet);
			_cardPropertyBinding.BindToggle(root.GetNode<CheckButton>("%OverrideHypnosesCheck"), packet, "overrideHypnoses", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindNumber(root.GetNode<SpinBox>("%OverrideCostRiseSpinBox"), packet, "overrideCostRise", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindNumber(root.GetNode<SpinBox>("%OverrideWeightSpinBox"), packet, "overrideWeight", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindNumber(root.GetNode<SpinBox>("%OverrideWavePointCostSpinBox"), packet, "overrideWavePointCost", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindToggle(root.GetNode<CheckButton>("%PlantUseCellCheck"), packet, "plantUseCell", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindToggle(root.GetNode<CheckButton>("%IzmPlantAllCellCheck"), packet, "izmPlantAllCell", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindToggle(root.GetNode<CheckButton>("%IzmPlantLeftCheck"), packet, "izmPlantLeft", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindToggle(root.GetNode<CheckButton>("%DisableWhenSunNegativeCheck"), packet, "disableWhenSunNegative", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindToggle(root.GetNode<CheckButton>("%CanPlaceOnZombieCheck"), packet, "canPlaceOnZombie", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_overrideStatus = root.GetNode<Label>("%OverrideStatus");
			_createOverrideButton = root.GetNode<Button>("%CreateOverrideButton");
			_openOverrideButton = root.GetNode<Button>("%OpenOverrideButton");
			_clearOverrideButton = root.GetNode<Button>("%ClearOverrideButton");
			_createOverrideButton.Pressed += CreatePacketOverride;
			_openOverrideButton.Pressed += OpenPacketOverride;
			_clearOverrideButton.Pressed += ClearPacketOverride;
			UpdatePacketOverrideEntry();
			_cardPropertyBinding.BindText(root.GetNode<LineEdit>("%PacketAnimeClipLineEdit"), packet, "packetAnimeClip", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			BindCardVector2Field(root.GetNode<SpinBox>("%PacketAnimeOffsetX"), root.GetNode<SpinBox>("%PacketAnimeOffsetY"), packet, "packetAnimeOffset");
			BindCardVector2Field(root.GetNode<SpinBox>("%PacketAnimeScaleX"), root.GetNode<SpinBox>("%PacketAnimeScaleY"), packet, "packetAnimeScale");
			BindCardVector2Field(root.GetNode<SpinBox>("%HandbookPacketAnimeOffsetX"), root.GetNode<SpinBox>("%HandbookPacketAnimeOffsetY"), packet, "handbookPacketAnimeOffset");
			_cardPropertyBinding.BindText(root.GetNode<TextEdit>("%DescribeTextEdit"), packet, "describe", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindText(root.GetNode<TextEdit>("%PlantFoodTextEdit"), packet, "plantfood", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindText(root.GetNode<TextEdit>("%HandbookDescribeTextEdit"), packet, "handbookDescribe", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			_cardPropertyBinding.BindText(root.GetNode<TextEdit>("%HandbookStoryTextEdit"), packet, "handbookStory", UpdateCardPreview, this, "RefreshCardEditorFromHistory");
			BindUnlockAndArmorEditors(root, packet);
			RefreshCharacterBindingSelector(packet.characterConfig);
		}
		finally
		{
			_updatingControls = updatingControls;
		}
	}

	private void BindUnlockAndArmorEditors(VBoxContainer root, TowerDefensePacketConfig packet)
	{
		HBoxContainer node = root.GetNode<HBoxContainer>("%UnlockPickerHost");
		_unlockConditionPicker = XWResourcePicker.Create();
		_unlockConditionPicker.Name = "UnlockConditionPicker";
		_unlockConditionPicker.Setup("UnlockConditionBaseConfig");
		_unlockConditionPicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		node.AddChild(_unlockConditionPicker, forceReadableName: false, InternalMode.Disabled);
		root.GetNode<Button>("%AddUnlockButton").Pressed += AddUnlockCondition;
		root.GetNode<Button>("%ReplaceUnlockButton").Pressed += ReplaceSelectedUnlockCondition;
		root.GetNode<Button>("%RemoveUnlockButton").Pressed += RemoveSelectedUnlockCondition;
		root.GetNode<Button>("%OpenUnlockButton").Pressed += () =>
		{
			EditSelectedUnlockCondition(GetSelectedUnlockIndex());
		};
		root.GetNode<Button>("%MoveUnlockUpButton").Pressed += () =>
		{
			MoveSelectedUnlockCondition(-1);
		};
		root.GetNode<Button>("%MoveUnlockDownButton").Pressed += () =>
		{
			MoveSelectedUnlockCondition(1);
		};
		_armorList = root.GetNode<ItemList>("%ArmorList");
		LineEdit armorValueEdit = root.GetNode<LineEdit>("%ArmorValueEdit");
		_armorList.ItemSelected += (long index) =>
		{
			_selectedArmorIndex = (int)index;
			if (_editingPacket?.initArmor != null && _selectedArmorIndex >= 0 && _selectedArmorIndex < _editingPacket.initArmor.Count)
			{
				armorValueEdit.Text = _editingPacket.initArmor[_selectedArmorIndex];
			}
		};
		root.GetNode<Button>("%AddArmorButton").Pressed += () =>
		{
			AddArmor(armorValueEdit.Text);
		};
		root.GetNode<Button>("%ReplaceArmorButton").Pressed += () =>
		{
			ReplaceSelectedArmor(armorValueEdit.Text);
		};
		root.GetNode<Button>("%RemoveArmorButton").Pressed += RemoveSelectedArmor;
		root.GetNode<Button>("%MoveArmorUpButton").Pressed += () =>
		{
			MoveSelectedArmor(-1);
		};
		root.GetNode<Button>("%MoveArmorDownButton").Pressed += () =>
		{
			MoveSelectedArmor(1);
		};
		armorValueEdit.TextSubmitted += (string _) =>
		{
			AddArmor(armorValueEdit.Text);
		};
		RefreshArmorList(packet);
	}

	private int GetSelectedUnlockIndex()
	{
		if (GodotObject.IsInstanceValid(_unlockConditionList))
		{
			int[] selectedItems = _unlockConditionList.GetSelectedItems();
			if (selectedItems.Length != 0)
			{
				return selectedItems[0];
			}
		}
		return _selectedUnlockIndex;
	}

	private void AddUnlockCondition()
	{
		if (CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig && _unlockConditionPicker?.EditedResource is UnlockConditionBaseConfig unlockConditionBaseConfig && GodotObject.IsInstanceValid(unlockConditionBaseConfig))
		{
			Array<UnlockConditionBaseConfig> array = ((towerDefensePacketConfig.unlockCheckList == null) ? new Array<UnlockConditionBaseConfig>() : new Array<UnlockConditionBaseConfig>(towerDefensePacketConfig.unlockCheckList));
			array.Add(unlockConditionBaseConfig);
			_selectedUnlockIndex = array.Count - 1;
			ReplaceCardUnlockConditions(array, "添加卡牌解锁条件");
		}
	}

	private void ReplaceSelectedUnlockCondition()
	{
		if (CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig && _unlockConditionPicker?.EditedResource is UnlockConditionBaseConfig unlockConditionBaseConfig && GodotObject.IsInstanceValid(unlockConditionBaseConfig) && towerDefensePacketConfig.unlockCheckList != null)
		{
			int selectedUnlockIndex = GetSelectedUnlockIndex();
			if (selectedUnlockIndex >= 0 && selectedUnlockIndex < towerDefensePacketConfig.unlockCheckList.Count)
			{
				Array<UnlockConditionBaseConfig> array = new Array<UnlockConditionBaseConfig>(towerDefensePacketConfig.unlockCheckList);
				array[selectedUnlockIndex] = unlockConditionBaseConfig;
				_selectedUnlockIndex = selectedUnlockIndex;
				ReplaceCardUnlockConditions(array, "替换卡牌解锁条件");
			}
		}
	}

	private void RemoveSelectedUnlockCondition()
	{
		if (_editingPacket?.unlockCheckList != null)
		{
			int selectedUnlockIndex = GetSelectedUnlockIndex();
			if (selectedUnlockIndex >= 0 && selectedUnlockIndex < _editingPacket.unlockCheckList.Count)
			{
				Array<UnlockConditionBaseConfig> array = new Array<UnlockConditionBaseConfig>(_editingPacket.unlockCheckList);
				array.RemoveAt(selectedUnlockIndex);
				_selectedUnlockIndex = ((array.Count == 0) ? (-1) : Math.Min(selectedUnlockIndex, array.Count - 1));
				ReplaceCardUnlockConditions(array, "删除卡牌解锁条件");
			}
		}
	}

	private void MoveSelectedUnlockCondition(int direction)
	{
		if (_editingPacket?.unlockCheckList != null)
		{
			int selectedUnlockIndex = GetSelectedUnlockIndex();
			int num = selectedUnlockIndex + Math.Sign(direction);
			if (selectedUnlockIndex >= 0 && selectedUnlockIndex < _editingPacket.unlockCheckList.Count && num >= 0 && num < _editingPacket.unlockCheckList.Count)
			{
				Array<UnlockConditionBaseConfig> array = new Array<UnlockConditionBaseConfig>(_editingPacket.unlockCheckList);
				UnlockConditionBaseConfig value = array[selectedUnlockIndex];
				array[selectedUnlockIndex] = array[num];
				array[num] = value;
				_selectedUnlockIndex = num;
				ReplaceCardUnlockConditions(array, (direction < 0) ? "上移卡牌解锁条件" : "下移卡牌解锁条件");
			}
		}
	}

	private void ReplaceCardUnlockConditions(Array<UnlockConditionBaseConfig> next, string actionName)
	{
		if (_cardPropertyBinding != null && CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig)
		{
			_cardPropertyBinding.SetValue(towerDefensePacketConfig, "unlockCheckList", next, actionName, this, "RefreshCardEditorFromHistory");
			towerDefensePacketConfig.NotifyPropertyListChanged();
			RefreshCardLists(towerDefensePacketConfig);
		}
	}

	private void AddArmor(string armor)
	{
		if (!string.IsNullOrWhiteSpace(armor) && CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig)
		{
			Array<string> array = ((towerDefensePacketConfig.initArmor == null) ? new Array<string>() : new Array<string>(towerDefensePacketConfig.initArmor));
			array.Add(armor.Trim());
			_selectedArmorIndex = array.Count - 1;
			ReplaceCardArmor(array, "添加初始护甲");
		}
	}

	private void ReplaceSelectedArmor(string armor)
	{
		if (!string.IsNullOrWhiteSpace(armor) && _editingPacket?.initArmor != null && _selectedArmorIndex >= 0 && _selectedArmorIndex < _editingPacket.initArmor.Count)
		{
			Array<string> array = new Array<string>(_editingPacket.initArmor);
			array[_selectedArmorIndex] = armor.Trim();
			ReplaceCardArmor(array, "替换初始护甲");
		}
	}

	private void RemoveSelectedArmor()
	{
		if (_editingPacket?.initArmor != null && _selectedArmorIndex >= 0 && _selectedArmorIndex < _editingPacket.initArmor.Count)
		{
			Array<string> array = new Array<string>(_editingPacket.initArmor);
			array.RemoveAt(_selectedArmorIndex);
			_selectedArmorIndex = ((array.Count == 0) ? (-1) : Math.Min(_selectedArmorIndex, array.Count - 1));
			ReplaceCardArmor(array, "删除初始护甲");
		}
	}

	private void MoveSelectedArmor(int direction)
	{
		if (_editingPacket?.initArmor != null && _selectedArmorIndex >= 0 && _selectedArmorIndex < _editingPacket.initArmor.Count)
		{
			int num = _selectedArmorIndex + Math.Sign(direction);
			if (num >= 0 && num < _editingPacket.initArmor.Count)
			{
				Array<string> array = new Array<string>(_editingPacket.initArmor);
				string value = array[_selectedArmorIndex];
				array[_selectedArmorIndex] = array[num];
				array[num] = value;
				_selectedArmorIndex = num;
				ReplaceCardArmor(array, (direction < 0) ? "上移初始护甲" : "下移初始护甲");
			}
		}
	}

	private void ReplaceCardArmor(Array<string> next, string actionName)
	{
		if (_cardPropertyBinding != null && CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig)
		{
			_cardPropertyBinding.SetValue(towerDefensePacketConfig, "initArmor", next, actionName, this, "RefreshCardEditorFromHistory");
			towerDefensePacketConfig.NotifyPropertyListChanged();
			RefreshArmorList(towerDefensePacketConfig);
			UpdateCardPreview();
		}
	}

	private void RefreshArmorList(TowerDefensePacketConfig packet)
	{
		if (!GodotObject.IsInstanceValid(_armorList))
		{
			return;
		}
		_armorList.Clear();
		if (packet?.initArmor != null)
		{
			for (int i = 0; i < packet.initArmor.Count; i++)
			{
				_armorList.AddItem($"{i + 1}. {packet.initArmor[i]}");
			}
			if (_selectedArmorIndex >= 0 && _selectedArmorIndex < _armorList.ItemCount)
			{
				_armorList.Select(_selectedArmorIndex);
			}
		}
	}

	private void BindCardBehaviorSurface(VBoxContainer root, TowerDefensePacketConfig packet)
	{
		_behaviorSummaryLabel = root.GetNode<Label>("%BehaviorSummaryLabel");
		_registryBehaviorStatusLabel = root.GetNode<Label>("%RegistryBehaviorStatusLabel");
		_registryBehaviorList = root.GetNode<ItemList>("%RegistryBehaviorList");
		_inlineBehaviorList = root.GetNode<ItemList>("%InlineBehaviorList");
		_customBehaviorIdEdit = root.GetNode<LineEdit>("%CustomBehaviorIdEdit");
		_registryBehaviorPicker = root.GetNode<PopupMenu>("%RegistryBehaviorPicker");
		root.GetNode<Button>("%PickRegistryBehaviorButton").Pressed += ShowRegistryBehaviorPicker;
		root.GetNode<Button>("%AddCustomBehaviorIdButton").Pressed += AddCustomBehaviorId;
		root.GetNode<Button>("%OpenRegistryBehaviorButton").Pressed += OpenSelectedRegistryBehavior;
		root.GetNode<Button>("%RemoveRegistryBehaviorButton").Pressed += RemoveSelectedRegistryBehavior;
		root.GetNode<Button>("%MoveRegistryBehaviorUpButton").Pressed += () =>
		{
			MoveSelectedRegistryBehavior(-1);
		};
		root.GetNode<Button>("%MoveRegistryBehaviorDownButton").Pressed += () =>
		{
			MoveSelectedRegistryBehavior(1);
		};
		root.GetNode<Button>("%RefreshRegistryBehaviorButton").Pressed += RefreshBehaviorRegistryFromUser;
		root.GetNode<Button>("%AddInlineBehaviorButton").Pressed += ShowInlineBehaviorSelector;
		root.GetNode<Button>("%OpenInlineBehaviorButton").Pressed += OpenSelectedInlineBehavior;
		root.GetNode<Button>("%RemoveInlineBehaviorButton").Pressed += RemoveSelectedInlineBehavior;
		root.GetNode<Button>("%MoveInlineBehaviorUpButton").Pressed += () =>
		{
			MoveSelectedInlineBehavior(-1);
		};
		root.GetNode<Button>("%MoveInlineBehaviorDownButton").Pressed += () =>
		{
			MoveSelectedInlineBehavior(1);
		};
		_registryBehaviorPicker.IndexPressed += AddRegistryBehaviorFromPicker;
		_customBehaviorIdEdit.TextSubmitted += (string _) =>
		{
			AddCustomBehaviorId();
		};
		_registryBehaviorList.ItemSelected += (long index) =>
		{
			_selectedRegistryBehaviorIndex = (int)index;
		};
		_registryBehaviorList.ItemActivated += (long index) =>
		{
			_selectedRegistryBehaviorIndex = (int)index;
			OpenSelectedRegistryBehavior();
		};
		_inlineBehaviorList.ItemSelected += (long index) =>
		{
			_selectedInlineBehaviorIndex = (int)index;
		};
		_inlineBehaviorList.ItemActivated += (long index) =>
		{
			_selectedInlineBehaviorIndex = (int)index;
			OpenSelectedInlineBehavior();
		};
		_behaviorRegistryRevision = TowerDefenseBehaviorRegistry.Revision;
		RefreshCardBehaviorLists(packet);
	}

	private void ShowRegistryBehaviorPicker()
	{
		if (GodotObject.IsInstanceValid(_registryBehaviorPicker))
		{
			BuildRegistryBehaviorPicker();
			_registryBehaviorPicker.Position = DisplayServer.MouseGetPosition();
			_registryBehaviorPicker.Popup();
		}
	}

	private void BuildRegistryBehaviorPicker()
	{
		_registryBehaviorPicker.Clear();
		_registryBehaviorPickerIds.Clear();
		Array<StringName> behaviorNames = TowerDefenseBehaviorRegistry.GetBehaviorNames();
		List<StringName> list = new List<StringName>();
		if (behaviorNames != null)
		{
			foreach (StringName item in behaviorNames)
			{
				list.Add(item);
			}
		}
		list.Sort((StringName left, StringName right) => string.Compare(left.ToString(), right.ToString(), StringComparison.OrdinalIgnoreCase));
		int num = 0;
		foreach (StringName item2 in list)
		{
			Resource behavior = TowerDefenseBehaviorRegistry.GetBehavior(item2);
			bool flag = behavior is CardBehaviorDefinition;
			string value = behavior?.GetType().Name ?? "资源缺失";
			int itemCount = _registryBehaviorPicker.ItemCount;
			_registryBehaviorPicker.AddItem(flag ? $"◆ {item2}  ·  {value}" : $"⚠ {item2}  ·  非卡牌行为 {value}", -1, Key.None);
			_registryBehaviorPickerIds.Add(item2);
			_registryBehaviorPicker.SetItemDisabled(itemCount, !flag);
			if (flag)
			{
				num++;
			}
		}
		if (_registryBehaviorPicker.ItemCount == 0)
		{
			_registryBehaviorPicker.AddItem("当前行为库为空，可在下方直接输入 Mod 行为 ID", -1, Key.None);
			_registryBehaviorPicker.SetItemDisabled(0, disabled: true);
			_registryBehaviorPickerIds.Add(null);
		}
		if (GodotObject.IsInstanceValid(_registryBehaviorStatusLabel))
		{
			_registryBehaviorStatusLabel.Text = $"行为库版本 {_behaviorRegistryRevision} · 可用卡牌拼图 {num} · 其他类型 {list.Count - num}";
		}
	}

	private void AddRegistryBehaviorFromPicker(long index)
	{
		int num = (int)index;
		if (num >= 0 && num < _registryBehaviorPickerIds.Count)
		{
			StringName stringName = _registryBehaviorPickerIds[num];
			if (!(stringName == null) && !stringName.IsEmpty)
			{
				AddRegistryBehaviorId(stringName, "添加注册行为拼图");
			}
		}
	}

	private void AddCustomBehaviorId()
	{
		if (GodotObject.IsInstanceValid(_customBehaviorIdEdit))
		{
			string text = _customBehaviorIdEdit.Text.Trim();
			if (!string.IsNullOrWhiteSpace(text))
			{
				AddRegistryBehaviorId(new StringName(text), "添加自定义行为 ID");
				_customBehaviorIdEdit.Text = "";
			}
		}
	}

	private void AddRegistryBehaviorId(StringName id, string actionName)
	{
		if (_editingPacket != null && !(id == null) && !id.IsEmpty)
		{
			Array<StringName> array = ((_editingPacket.behaviorIds == null) ? new Array<StringName>() : new Array<StringName>(_editingPacket.behaviorIds));
			array.Add(id);
			_selectedRegistryBehaviorIndex = array.Count - 1;
			ReplaceRegistryBehaviorIds(array, actionName);
		}
	}

	private void RemoveSelectedRegistryBehavior()
	{
		if (_editingPacket?.behaviorIds != null)
		{
			int selectedRegistryBehaviorIndex = GetSelectedRegistryBehaviorIndex();
			if (selectedRegistryBehaviorIndex >= 0 && selectedRegistryBehaviorIndex < _editingPacket.behaviorIds.Count)
			{
				Array<StringName> array = new Array<StringName>(_editingPacket.behaviorIds);
				array.RemoveAt(selectedRegistryBehaviorIndex);
				_selectedRegistryBehaviorIndex = Math.Min(selectedRegistryBehaviorIndex, array.Count - 1);
				ReplaceRegistryBehaviorIds(array, "移除注册行为拼图");
			}
		}
	}

	private void MoveSelectedRegistryBehavior(int direction)
	{
		if (_editingPacket?.behaviorIds != null)
		{
			int selectedRegistryBehaviorIndex = GetSelectedRegistryBehaviorIndex();
			int num = selectedRegistryBehaviorIndex + Math.Sign(direction);
			if (selectedRegistryBehaviorIndex >= 0 && selectedRegistryBehaviorIndex < _editingPacket.behaviorIds.Count && num >= 0 && num < _editingPacket.behaviorIds.Count)
			{
				Array<StringName> array = new Array<StringName>(_editingPacket.behaviorIds);
				StringName value = array[selectedRegistryBehaviorIndex];
				array[selectedRegistryBehaviorIndex] = array[num];
				array[num] = value;
				_selectedRegistryBehaviorIndex = num;
				ReplaceRegistryBehaviorIds(array, (direction < 0) ? "提前注册行为拼图" : "延后注册行为拼图");
			}
		}
	}

	private void ReplaceRegistryBehaviorIds(Array<StringName> next, string actionName)
	{
		if (_cardPropertyBinding != null && CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig)
		{
			_cardPropertyBinding.SetValue(towerDefensePacketConfig, "behaviorIds", next, actionName, this, "RefreshCardEditorFromHistory");
			towerDefensePacketConfig.NotifyPropertyListChanged();
			RefreshCardBehaviorLists(towerDefensePacketConfig);
			UpdateCardPreview();
		}
	}

	private int GetSelectedRegistryBehaviorIndex()
	{
		if (GodotObject.IsInstanceValid(_registryBehaviorList))
		{
			int[] selectedItems = _registryBehaviorList.GetSelectedItems();
			if (selectedItems.Length != 0)
			{
				return selectedItems[0];
			}
		}
		return _selectedRegistryBehaviorIndex;
	}

	private void OpenSelectedRegistryBehavior()
	{
		if (_editingPacket?.behaviorIds == null)
		{
			return;
		}
		int selectedRegistryBehaviorIndex = GetSelectedRegistryBehaviorIndex();
		if (selectedRegistryBehaviorIndex >= 0 && selectedRegistryBehaviorIndex < _editingPacket.behaviorIds.Count)
		{
			Resource behavior = TowerDefenseBehaviorRegistry.GetBehavior(_editingPacket.behaviorIds[selectedRegistryBehaviorIndex]);
			if (GodotObject.IsInstanceValid(behavior))
			{
				XWEditorInterface.Instance?.EditResource(behavior);
			}
			else
			{
				XWEditorInterface.Instance?.ShowToast("该行为 ID 尚未由游戏或 Mod 注册。", 2);
			}
		}
	}

	private void ShowInlineBehaviorSelector()
	{
		EnsurePacketEventSelectorWindow();
		if (GodotObject.IsInstanceValid(_packetEventSelectorWindow))
		{
			_packetEventSelectorWindow.ShowFor("新建卡牌行为拼图", "只显示 CardBehaviorDefinition；创建后可在主面板直接编辑全部属性", typeof(CardBehaviorDefinition), OnInlineBehaviorTypeSelected);
		}
	}

	private void OnInlineBehaviorTypeSelected(Type runtimeType)
	{
		CardBehaviorDefinition cardBehaviorDefinition = CreateInlineBehaviorInstance(runtimeType);
		if (GodotObject.IsInstanceValid(cardBehaviorDefinition) && _editingPacket != null)
		{
			Array<CardBehaviorDefinition> array = ((_editingPacket.behaviors == null) ? new Array<CardBehaviorDefinition>() : new Array<CardBehaviorDefinition>(_editingPacket.behaviors));
			array.Add(cardBehaviorDefinition);
			_selectedInlineBehaviorIndex = array.Count - 1;
			ReplaceInlineBehaviors(array, "添加内联行为拼图");
			OpenInlineBehavior(array.Count - 1, cardBehaviorDefinition);
		}
	}

	[UnconditionalSuppressMessage("Trimming", "IL2067", Justification = "The Mod editor creates an authored card behavior selected from loaded game and Mod assemblies.")]
	[UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "The selector already restricts choices to public parameterless authored behavior types.")]
	private static CardBehaviorDefinition CreateInlineBehaviorInstance(Type type)
	{
		if (type == null || type.IsAbstract || type.ContainsGenericParameters || !typeof(CardBehaviorDefinition).IsAssignableFrom(type) || type.GetConstructor(Type.EmptyTypes) == null)
		{
			return null;
		}
		try
		{
			return Activator.CreateInstance(type) as CardBehaviorDefinition;
		}
		catch (Exception ex)
		{
			GD.PushWarning("Card behavior authoring create failed: " + type.FullName + " " + ex.Message);
			return null;
		}
	}

	private void RemoveSelectedInlineBehavior()
	{
		if (_editingPacket?.behaviors != null)
		{
			int selectedInlineBehaviorIndex = GetSelectedInlineBehaviorIndex();
			if (selectedInlineBehaviorIndex >= 0 && selectedInlineBehaviorIndex < _editingPacket.behaviors.Count)
			{
				Array<CardBehaviorDefinition> array = new Array<CardBehaviorDefinition>(_editingPacket.behaviors);
				array.RemoveAt(selectedInlineBehaviorIndex);
				_selectedInlineBehaviorIndex = Math.Min(selectedInlineBehaviorIndex, array.Count - 1);
				ReplaceInlineBehaviors(array, "移除内联行为拼图");
			}
		}
	}

	private void MoveSelectedInlineBehavior(int direction)
	{
		if (_editingPacket?.behaviors != null)
		{
			int selectedInlineBehaviorIndex = GetSelectedInlineBehaviorIndex();
			int num = selectedInlineBehaviorIndex + Math.Sign(direction);
			if (selectedInlineBehaviorIndex >= 0 && selectedInlineBehaviorIndex < _editingPacket.behaviors.Count && num >= 0 && num < _editingPacket.behaviors.Count)
			{
				Array<CardBehaviorDefinition> array = new Array<CardBehaviorDefinition>(_editingPacket.behaviors);
				CardBehaviorDefinition value = array[selectedInlineBehaviorIndex];
				array[selectedInlineBehaviorIndex] = array[num];
				array[num] = value;
				_selectedInlineBehaviorIndex = num;
				ReplaceInlineBehaviors(array, (direction < 0) ? "提前内联行为拼图" : "延后内联行为拼图");
			}
		}
	}

	private void ReplaceInlineBehaviors(Array<CardBehaviorDefinition> next, string actionName)
	{
		if (_cardPropertyBinding != null && CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig)
		{
			_cardPropertyBinding.SetValue(towerDefensePacketConfig, "behaviors", next, actionName, this, "RefreshCardEditorFromHistory");
			towerDefensePacketConfig.NotifyPropertyListChanged();
			RefreshCardBehaviorLists(towerDefensePacketConfig);
			UpdateCardPreview();
		}
	}

	private int GetSelectedInlineBehaviorIndex()
	{
		if (GodotObject.IsInstanceValid(_inlineBehaviorList))
		{
			int[] selectedItems = _inlineBehaviorList.GetSelectedItems();
			if (selectedItems.Length != 0)
			{
				return selectedItems[0];
			}
		}
		return _selectedInlineBehaviorIndex;
	}

	private void OpenSelectedInlineBehavior()
	{
		if (_editingPacket?.behaviors == null)
		{
			return;
		}
		int selectedInlineBehaviorIndex = GetSelectedInlineBehaviorIndex();
		if (selectedInlineBehaviorIndex >= 0 && selectedInlineBehaviorIndex < _editingPacket.behaviors.Count)
		{
			CardBehaviorDefinition cardBehaviorDefinition = _editingPacket.behaviors[selectedInlineBehaviorIndex];
			if (GodotObject.IsInstanceValid(cardBehaviorDefinition))
			{
				OpenInlineBehavior(selectedInlineBehaviorIndex, cardBehaviorDefinition);
			}
		}
	}

	private void OpenInlineBehavior(int index, CardBehaviorDefinition behavior)
	{
		XWResourceEditContext context = XWResourceEditContext.ForProperty(behavior, _editingPacket, behavior.ResourcePath, CurrentResourcePath, "behaviors", index, "card_behavior_editor", CurrentEditContext?.IsBuiltInSource ?? false);
		XWEditorInterface.Instance?.EditResource(behavior, context);
	}

	private void RefreshBehaviorRegistryFromUser()
	{
		_behaviorRegistryRevision = TowerDefenseBehaviorRegistry.Revision;
		BuildRegistryBehaviorPicker();
		RefreshCardBehaviorLists(_editingPacket);
		XWEditorInterface.Instance?.ShowToast("卡牌行为库已刷新。");
	}

	private void RefreshCardBehaviorLists(TowerDefensePacketConfig packet)
	{
		if (packet == null)
		{
			return;
		}
		int value = packet.behaviorIds?.Count ?? 0;
		int value2 = packet.behaviors?.Count ?? 0;
		if (GodotObject.IsInstanceValid(_behaviorSummaryLabel))
		{
			_behaviorSummaryLabel.Text = $"注册行为 {value} · 内联行为 {value2} · 费用规则 {packet.changeCostList?.Count ?? 0}";
		}
		if (_behaviorIcon == null)
		{
			_behaviorIcon = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/GraphEdit.svg", null, ResourceLoader.CacheMode.Reuse);
		}
		if (_behaviorWarningIcon == null)
		{
			_behaviorWarningIcon = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/NodeWarning.svg", null, ResourceLoader.CacheMode.Reuse);
		}
		if (GodotObject.IsInstanceValid(_registryBehaviorList))
		{
			_registryBehaviorList.Clear();
			int num = 0;
			int num2 = 0;
			if (packet.behaviorIds != null)
			{
				for (int i = 0; i < packet.behaviorIds.Count; i++)
				{
					StringName stringName = packet.behaviorIds[i];
					Resource behavior = TowerDefenseBehaviorRegistry.GetBehavior(stringName);
					bool flag = !GodotObject.IsInstanceValid(behavior);
					bool flag2 = !flag && !(behavior is CardBehaviorDefinition);
					string value3;
					if (flag)
					{
						value3 = "断开";
					}
					else
					{
						value3 = (flag2 ? "类型错误" : "已连接");
					}
					string text = behavior?.GetType().Name ?? "等待 Mod 注册";
					_registryBehaviorList.AddItem($"{i + 1}. ◆ {stringName}  [{value3}]  {text}");
					_registryBehaviorList.SetItemIcon(i, (flag | flag2) ? _behaviorWarningIcon : _behaviorIcon);
					ItemList registryBehaviorList = _registryBehaviorList;
					int idx = i;
					Color customFgColor;
					if (flag)
					{
						customFgColor = new Color(1f, 0.42f, 0.38f);
					}
					else
					{
						customFgColor = (flag2 ? new Color(1f, 0.68f, 0.25f) : new Color(0.68f, 1f, 0.58f));
					}
					registryBehaviorList.SetItemCustomFgColor(idx, customFgColor);
					ItemList registryBehaviorList2 = _registryBehaviorList;
					int idx2 = i;
					string tooltip;
					if (flag)
					{
						tooltip = $"未找到行为 ID“{stringName}”。保存是安全的；加载对应 Mod 后会自动连接。";
					}
					else
					{
						tooltip = (flag2 ? $"“{stringName}”注册为 {text}，不是卡牌行为，运行时会跳过。" : ("双击打开 " + text + " 的直接属性编辑界面。"));
					}
					registryBehaviorList2.SetItemTooltip(idx2, tooltip);
					if (flag)
					{
						num++;
					}
					else if (flag2)
					{
						num2++;
					}
				}
			}
			if (_selectedRegistryBehaviorIndex >= 0 && _selectedRegistryBehaviorIndex < _registryBehaviorList.ItemCount)
			{
				_registryBehaviorList.Select(_selectedRegistryBehaviorIndex);
			}
			if (GodotObject.IsInstanceValid(_registryBehaviorStatusLabel))
			{
				_registryBehaviorStatusLabel.Text = ((num == 0 && num2 == 0) ? $"行为库版本 {_behaviorRegistryRevision} · 全部拼图连接正常" : $"行为库版本 {_behaviorRegistryRevision} · 断开 {num} · 类型错误 {num2}");
			}
		}
		if (!GodotObject.IsInstanceValid(_inlineBehaviorList))
		{
			return;
		}
		_inlineBehaviorList.Clear();
		if (packet.behaviors != null)
		{
			for (int j = 0; j < packet.behaviors.Count; j++)
			{
				CardBehaviorDefinition cardBehaviorDefinition = packet.behaviors[j];
				bool flag3 = GodotObject.IsInstanceValid(cardBehaviorDefinition);
				string value4 = (flag3 ? cardBehaviorDefinition.GetDiagnosticName() : "空拼图");
				string value5 = (flag3 ? cardBehaviorDefinition.GetType().Name : "资源缺失");
				string value6 = ((flag3 && cardBehaviorDefinition.InitiallyEnabled) ? "启用" : "停用");
				_inlineBehaviorList.AddItem($"{j + 1}. ◆ {value4}  [{value6}]  {value5}");
				_inlineBehaviorList.SetItemIcon(j, flag3 ? _behaviorIcon : _behaviorWarningIcon);
				_inlineBehaviorList.SetItemCustomFgColor(j, (flag3 && cardBehaviorDefinition.InitiallyEnabled) ? new Color(0.58f, 0.9f, 1f) : new Color(0.72f, 0.72f, 0.68f));
				_inlineBehaviorList.SetItemTooltip(j, flag3 ? "双击进入直接属性界面；全部导出属性都可在主面板编辑。" : "该数组位置没有有效行为资源。");
			}
		}
		if (_selectedInlineBehaviorIndex >= 0 && _selectedInlineBehaviorIndex < _inlineBehaviorList.ItemCount)
		{
			_inlineBehaviorList.Select(_selectedInlineBehaviorIndex);
		}
	}

	private void BindCardEventSurface(VBoxContainer root, TowerDefensePacketConfig packet)
	{
		_eventSummaryLabel = root.GetNode<Label>("%EventSummaryLabel");
		root.GetNode<Button>("%AddPressButton").Pressed += () =>
		{
			ShowPacketEventSelector("pressedActions");
		};
		root.GetNode<Button>("%RemovePressButton").Pressed += () =>
		{
			RemoveSelectedPacketEvent("pressedActions");
		};
		root.GetNode<Button>("%AddPlantButton").Pressed += () =>
		{
			ShowPacketEventSelector("useSucceededActions");
		};
		root.GetNode<Button>("%RemovePlantButton").Pressed += () =>
		{
			RemoveSelectedPacketEvent("useSucceededActions");
		};
		root.GetNode<Button>("%MovePressUpButton").Pressed += () =>
		{
			MoveSelectedPacketEvent("pressedActions", -1);
		};
		root.GetNode<Button>("%MovePressDownButton").Pressed += () =>
		{
			MoveSelectedPacketEvent("pressedActions", 1);
		};
		root.GetNode<Button>("%MovePlantUpButton").Pressed += () =>
		{
			MoveSelectedPacketEvent("useSucceededActions", -1);
		};
		root.GetNode<Button>("%MovePlantDownButton").Pressed += () =>
		{
			MoveSelectedPacketEvent("useSucceededActions", 1);
		};
		root.GetNode<Button>("%OpenPressButton").Pressed += () =>
		{
			EditSelectedPacketEvent("pressedActions", GetSelectedPacketEventIndex("pressedActions"));
		};
		root.GetNode<Button>("%OpenPlantButton").Pressed += () =>
		{
			EditSelectedPacketEvent("useSucceededActions", GetSelectedPacketEventIndex("useSucceededActions"));
		};
		_pressedActionsList = root.GetNode<ItemList>("%EventPressList");
		_useSucceededActionsList = root.GetNode<ItemList>("%EventPlantList");
		_unlockConditionList = root.GetNode<ItemList>("%UnlockConditionList");
		_pressedActionsList.ItemSelected += (long index) =>
		{
			_selectedEventPressIndex = (int)index;
		};
		_useSucceededActionsList.ItemSelected += (long index) =>
		{
			_selectedEventPlantIndex = (int)index;
		};
		_pressedActionsList.ItemActivated += (long index) =>
		{
			EditSelectedPacketEvent("pressedActions", (int)index);
		};
		_useSucceededActionsList.ItemActivated += (long index) =>
		{
			EditSelectedPacketEvent("useSucceededActions", (int)index);
		};
		_unlockConditionList.ItemActivated += EditSelectedUnlockCondition;
		_unlockConditionList.ItemSelected += (long index) =>
		{
			_selectedUnlockIndex = (int)index;
			if (_editingPacket?.unlockCheckList != null && _selectedUnlockIndex >= 0 && _selectedUnlockIndex < _editingPacket.unlockCheckList.Count)
			{
				_unlockConditionPicker?.SetEditedResource(_editingPacket.unlockCheckList[_selectedUnlockIndex]);
			}
		};
		RefreshCardLists(packet);
	}

	private void EditSelectedUnlockCondition(long index)
	{
		if (GodotObject.IsInstanceValid(_editingPacket) && _editingPacket.unlockCheckList != null && index >= 0 && index < _editingPacket.unlockCheckList.Count)
		{
			UnlockConditionBaseConfig unlockConditionBaseConfig = _editingPacket.unlockCheckList[(int)index];
			if (GodotObject.IsInstanceValid(unlockConditionBaseConfig))
			{
				XWResourceEditContext context = XWResourceEditContext.ForProperty(unlockConditionBaseConfig, _editingPacket, unlockConditionBaseConfig.ResourcePath, CurrentResourcePath, "unlockCheckList", (int)index, "card_editor", CurrentEditContext?.IsBuiltInSource ?? XWResourceEditContext.IsBuiltInPath(CurrentResourcePath));
				XWEditorInterface.Instance?.EditResource(unlockConditionBaseConfig, context);
			}
		}
	}

	private void ShowPacketEventSelector(string targetProperty)
	{
		_packetEventTargetProperty = ((targetProperty == "useSucceededActions") ? "useSucceededActions" : "pressedActions");
		EnsurePacketEventSelectorWindow();
		if (GodotObject.IsInstanceValid(_packetEventSelectorWindow))
		{
			string title = ((_packetEventTargetProperty == "useSucceededActions") ? "种植事件拼图" : "按下事件拼图");
			string subtitle = ((_packetEventTargetProperty == "useSucceededActions") ? "完成种植时按顺序执行这些事件" : "拿起或按下卡牌时按顺序执行这些事件");
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
		CardActionBehaviorDefinition packetEvent = CreatePacketEventInstance(runtimeType);
		AddPacketEventToTarget(_packetEventTargetProperty, packetEvent);
	}

	[UnconditionalSuppressMessage("Trimming", "IL2067", Justification = "Packet event selector creates editor-only instances from loaded runtime event types discovered for Mod authoring.")]
	private static CardActionBehaviorDefinition CreatePacketEventInstance(Type type)
	{
		if (type == null || !typeof(CardActionBehaviorDefinition).IsAssignableFrom(type))
		{
			return null;
		}
		try
		{
			return Activator.CreateInstance(type) as CardActionBehaviorDefinition;
		}
		catch (Exception ex)
		{
			GD.PushWarning("Card packet event create failed: " + type.FullName + " " + ex.Message);
			return null;
		}
	}

	private static string GetPacketEventName(CardActionBehaviorDefinition packetEvent, Type type)
	{
		if (GodotObject.IsInstanceValid(packetEvent))
		{
			try
			{
				Dictionary dictionary = packetEvent.ExportConfiguration();
				if (dictionary != null && dictionary.ContainsKey("EventName"))
				{
					string text = dictionary["EventName"].AsString();
					if (!string.IsNullOrWhiteSpace(text))
					{
						return text;
					}
				}
			}
			catch
			{
			}
		}
		string text2 = type?.Name ?? "PacketEvent";
		if (!text2.StartsWith("CardActionBehavior", StringComparison.Ordinal))
		{
			return text2;
		}
		string text3 = text2;
		int length = "CardActionBehavior".Length;
		return text3.Substring(length, text3.Length - length);
	}

	private void AddPacketEventToTarget(string propertyName, CardActionBehaviorDefinition packetEvent)
	{
		if (_editingPacket != null && GodotObject.IsInstanceValid(packetEvent))
		{
			Array<CardActionBehaviorDefinition> packetEventArray = GetPacketEventArray(propertyName);
			Array<CardActionBehaviorDefinition> array = ((packetEventArray == null) ? new Array<CardActionBehaviorDefinition>() : new Array<CardActionBehaviorDefinition>(packetEventArray));
			array.Add(packetEvent);
			if (propertyName == "useSucceededActions")
			{
				_selectedEventPlantIndex = array.Count - 1;
			}
			else
			{
				_selectedEventPressIndex = array.Count - 1;
			}
			ReplacePacketEvents(propertyName, array, "添加卡牌事件 " + propertyName);
			OpenPacketEventResource(propertyName, array.Count - 1, packetEvent);
			XWEditorInterface.Instance?.AddOutputMessage("卡牌编辑器: 添加事件 " + GetPacketEventName(packetEvent, packetEvent.GetType()) + " -> " + propertyName);
		}
	}

	private void RemoveSelectedPacketEvent(string propertyName)
	{
		if (_editingPacket == null)
		{
			return;
		}
		Array<CardActionBehaviorDefinition> packetEventArray = GetPacketEventArray(propertyName);
		int selectedPacketEventIndex = GetSelectedPacketEventIndex(propertyName);
		if (packetEventArray != null && selectedPacketEventIndex >= 0 && selectedPacketEventIndex < packetEventArray.Count)
		{
			Array<CardActionBehaviorDefinition> array = new Array<CardActionBehaviorDefinition>(packetEventArray);
			array.RemoveAt(selectedPacketEventIndex);
			if (propertyName == "useSucceededActions")
			{
				_selectedEventPlantIndex = Math.Min(selectedPacketEventIndex, array.Count - 1);
			}
			else
			{
				_selectedEventPressIndex = Math.Min(selectedPacketEventIndex, array.Count - 1);
			}
			ReplacePacketEvents(propertyName, array, "删除卡牌事件 " + propertyName);
			XWEditorInterface.Instance?.AddOutputMessage($"卡牌编辑器: 删除事件 {propertyName}[{selectedPacketEventIndex}]");
		}
	}

	private void MoveSelectedPacketEvent(string propertyName, int direction)
	{
		Array<CardActionBehaviorDefinition> packetEventArray = GetPacketEventArray(propertyName);
		int selectedPacketEventIndex = GetSelectedPacketEventIndex(propertyName);
		if (packetEventArray == null || selectedPacketEventIndex < 0 || selectedPacketEventIndex >= packetEventArray.Count)
		{
			return;
		}
		int num = selectedPacketEventIndex + Math.Sign(direction);
		if (num >= 0 && num < packetEventArray.Count)
		{
			Array<CardActionBehaviorDefinition> array = new Array<CardActionBehaviorDefinition>(packetEventArray);
			CardActionBehaviorDefinition value = array[selectedPacketEventIndex];
			array[selectedPacketEventIndex] = array[num];
			array[num] = value;
			if (propertyName == "useSucceededActions")
			{
				_selectedEventPlantIndex = num;
			}
			else
			{
				_selectedEventPressIndex = num;
			}
			ReplacePacketEvents(propertyName, array, (direction < 0) ? "上移卡牌事件" : "下移卡牌事件");
		}
	}

	private void ReplacePacketEvents(string propertyName, Array<CardActionBehaviorDefinition> next, string actionName)
	{
		if (_cardPropertyBinding != null && CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig)
		{
			_cardPropertyBinding.SetValue(towerDefensePacketConfig, propertyName, next, actionName, this, "RefreshCardEditorFromHistory");
			towerDefensePacketConfig.NotifyPropertyListChanged();
			RefreshCardLists(towerDefensePacketConfig);
			UpdateCardPreview();
		}
	}

	private void EditSelectedPacketEvent(string propertyName, int index)
	{
		Array<CardActionBehaviorDefinition> packetEventArray = GetPacketEventArray(propertyName);
		if (packetEventArray != null && index >= 0 && index < packetEventArray.Count)
		{
			CardActionBehaviorDefinition cardActionBehaviorDefinition = packetEventArray[index];
			if (GodotObject.IsInstanceValid(cardActionBehaviorDefinition))
			{
				OpenPacketEventResource(propertyName, index, cardActionBehaviorDefinition);
			}
		}
	}

	private void OpenPacketEventResource(string propertyName, int index, CardActionBehaviorDefinition packetEvent)
	{
		if (GodotObject.IsInstanceValid(_editingPacket) && GodotObject.IsInstanceValid(packetEvent))
		{
			XWResourceEditContext context = XWResourceEditContext.ForProperty(packetEvent, _editingPacket, packetEvent.ResourcePath, CurrentResourcePath, propertyName, index, "card_editor", CurrentEditContext?.IsBuiltInSource ?? false);
			XWEditorInterface.Instance?.EditResource(packetEvent, context);
		}
	}

	private Array<CardActionBehaviorDefinition> GetPacketEventArray(string propertyName)
	{
		if (_editingPacket == null)
		{
			return null;
		}
		if (propertyName == "useSucceededActions")
		{
			return _editingPacket.useSucceededActions;
		}
		return _editingPacket.pressedActions;
	}

	private int GetSelectedPacketEventIndex(string propertyName)
	{
		if (propertyName == "useSucceededActions")
		{
			if (GodotObject.IsInstanceValid(_useSucceededActionsList))
			{
				int[] selectedItems = _useSucceededActionsList.GetSelectedItems();
				if (selectedItems.Length != 0)
				{
					return selectedItems[0];
				}
			}
			return _selectedEventPlantIndex;
		}
		if (GodotObject.IsInstanceValid(_pressedActionsList))
		{
			int[] selectedItems2 = _pressedActionsList.GetSelectedItems();
			if (selectedItems2.Length != 0)
			{
				return selectedItems2[0];
			}
		}
		return _selectedEventPressIndex;
	}

	private void OnCardPropertyEdited(bool committed)
	{
		if (CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig && towerDefensePacketConfig == _editingPacket)
		{
			_cardPreviewRefreshCommitted = committed;
			towerDefensePacketConfig.EmitChanged();
			if (committed)
			{
				NotifyCurrentResourceEdited();
				SaveCardResource(towerDefensePacketConfig);
			}
			else
			{
				MarkCurrentResourceDirty();
			}
		}
	}

	public void RefreshCardEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()) && CurrentResource is TowerDefensePacketConfig towerDefensePacketConfig)
		{
			_cardPreviewRefreshCommitted = true;
			_editingPacket = towerDefensePacketConfig;
			_cardPropertyBinding?.RefreshBoundControls();
			SyncCardVector2HistoryControls(towerDefensePacketConfig);
			Control host = CanvasGrid?.FindChild("TypeCards", recursive: true, owned: false) as Control;
			int type = (int)towerDefensePacketConfig.type;
			SelectVisualCard(host, type.ToString());
			SelectVisualCard(CanvasGrid?.FindChild("SpawnMethodCards", recursive: true, owned: false) as Control, towerDefensePacketConfig.spawnMethod ?? "");
			RefreshCardLists(towerDefensePacketConfig);
			RefreshCharacterBindingSelector(towerDefensePacketConfig.characterConfig);
			UpdatePacketOverrideEntry();
			UpdateCardPreview();
			SaveCardResource(towerDefensePacketConfig);
		}
	}

	private void SyncCardVector2HistoryControls(TowerDefensePacketConfig packet)
	{
		if (CanvasGrid != null && GodotObject.IsInstanceValid(packet))
		{
			SyncCardVector2HistoryControl(packet, "packetAnimeOffset", "PacketAnimeOffsetX", "PacketAnimeOffsetY");
			SyncCardVector2HistoryControl(packet, "packetAnimeScale", "PacketAnimeScaleX", "PacketAnimeScaleY");
			SyncCardVector2HistoryControl(packet, "handbookPacketAnimeOffset", "HandbookPacketAnimeOffsetX", "HandbookPacketAnimeOffsetY");
		}
	}

	private void SyncCardVector2HistoryControl(TowerDefensePacketConfig packet, StringName property, string xControlName, string yControlName)
	{
		SpinBox spinBox = CanvasGrid?.FindChild(xControlName, recursive: true, owned: false) as SpinBox;
		SpinBox spinBox2 = CanvasGrid?.FindChild(yControlName, recursive: true, owned: false) as SpinBox;
		if (GodotObject.IsInstanceValid(spinBox) && GodotObject.IsInstanceValid(spinBox2))
		{
			Vector2 vector = packet.Get(property).AsVector2();
			spinBox.SetValueNoSignal(vector.X);
			spinBox2.SetValueNoSignal(vector.Y);
		}
	}

	private void SaveCardResource(TowerDefensePacketConfig packet)
	{
		string currentResourcePath = CurrentResourcePath;
		if (string.IsNullOrWhiteSpace(currentResourcePath) || !GodotObject.IsInstanceValid(packet))
		{
			return;
		}
		Error error = ResourceSaver.Save(packet, currentResourcePath, ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			GD.PushWarning($"Card editor save failed: {error} {currentResourcePath}");
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

	private void UpdateCardPreview()
	{
		if (_editingPacket == null)
		{
			return;
		}
		_updatingControls = true;
		try
		{
			if (GodotObject.IsInstanceValid(_cardFrame))
			{
				_cardFrame.AddThemeStyleboxOverride("panel", CreatePanelStyle(GetPacketTypeColor(_editingPacket.type), 2, 6));
			}
			if (GodotObject.IsInstanceValid(_previewNameLabel))
			{
				_previewNameLabel.Text = EmptyToPlaceholder(_editingPacket.name);
			}
			if (GodotObject.IsInstanceValid(_previewKeyLabel))
			{
				_previewKeyLabel.Text = "资源键：" + EmptyToPlaceholder(_editingPacket.saveKey);
			}
			if (GodotObject.IsInstanceValid(_previewCostLabel))
			{
				_previewCostLabel.Text = "☀";
			}
			if (GodotObject.IsInstanceValid(_inlineCostSpinBox))
			{
				_inlineCostSpinBox.Value = _editingPacket.overrideCost;
			}
			if (GodotObject.IsInstanceValid(_inlineSaveKeyLineEdit))
			{
				_inlineSaveKeyLineEdit.Text = _editingPacket.saveKey ?? "";
			}
			if (GodotObject.IsInstanceValid(_inlineCooldownSpinBox))
			{
				_inlineCooldownSpinBox.Value = _editingPacket.overridePacketCooldown;
			}
			if (GodotObject.IsInstanceValid(_inlineStartingCooldownSpinBox))
			{
				_inlineStartingCooldownSpinBox.Value = _editingPacket.overrideStartingCooldown;
			}
			if (GodotObject.IsInstanceValid(_inlineAnimationLineEdit))
			{
				_inlineAnimationLineEdit.Text = _editingPacket.packetAnimeClip ?? "";
			}
			if (GodotObject.IsInstanceValid(_previewCooldownLabel))
			{
				_previewCooldownLabel.Text = "冷却：" + GetEffectiveCooldownText(_editingPacket);
			}
			if (GodotObject.IsInstanceValid(_previewTypeLabel))
			{
				_previewTypeLabel.Text = "卡面：" + GetPacketTypeDisplayName(_editingPacket.type);
			}
			if (GodotObject.IsInstanceValid(_previewCharacterLabel))
			{
				_previewCharacterLabel.Text = "角色：" + FormatCharacterBinding(_editingPacket.characterConfig);
			}
			if (GodotObject.IsInstanceValid(_previewAnimationLabel))
			{
				_previewAnimationLabel.Text = "动画片段：" + EmptyToPlaceholder(_editingPacket.packetAnimeClip);
			}
			if (GodotObject.IsInstanceValid(_valueOverrideBadge))
			{
				_valueOverrideBadge.Text = BuildCardValueOverrideSummary(_editingPacket);
			}
			RefreshPacketShowPreview(_editingPacket);
			RebuildCardCharacterPreview();
			RefreshCharacterBindingSelector(_editingPacket.characterConfig);
			UpdatePacketOverrideEntry();
		}
		finally
		{
			_updatingControls = false;
			_cardPreviewRefreshCommitted = true;
		}
	}

	private void CreatePacketOverride()
	{
		if (GodotObject.IsInstanceValid(_editingPacket))
		{
			SetPacketOverride(new TowerDefensePacketOverride(), "创建卡牌覆盖规则");
			UpdatePacketOverrideEntry();
			OpenPacketOverride();
		}
	}

	private void OpenPacketOverride()
	{
		if (GodotObject.IsInstanceValid(_editingPacket) && GodotObject.IsInstanceValid(_editingPacket._override))
		{
			XWResourceEditContext context = XWResourceEditContext.ForProperty(_editingPacket._override, _editingPacket, _editingPacket._override.ResourcePath, CurrentResourcePath, "_override", -1, "card_editor", CurrentEditContext?.IsBuiltInSource ?? XWResourceEditContext.IsBuiltInPath(CurrentResourcePath));
			XWEditorInterface.Instance?.EditResource(_editingPacket._override, context);
		}
	}

	private void ClearPacketOverride()
	{
		if (GodotObject.IsInstanceValid(_editingPacket))
		{
			SetPacketOverride(null, "清除卡牌覆盖规则");
			UpdateCardPreview();
			UpdatePacketOverrideEntry();
		}
	}

	private void SetPacketOverride(TowerDefensePacketOverride value, string actionName)
	{
		if (GodotObject.IsInstanceValid(_editingPacket) && _editingPacket._override != value)
		{
			TowerDefensePacketOverride from = _editingPacket._override;
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager == null)
			{
				_editingPacket._override = value;
			}
			else
			{
				xWUndoRedoManager.CreateAction(actionName);
				xWUndoRedoManager.AddDoProperty(_editingPacket, "_override", Variant.From(in value));
				xWUndoRedoManager.AddUndoProperty(_editingPacket, "_override", Variant.From(in from));
				xWUndoRedoManager.AddDoMethod(this, "RefreshCardEditorFromHistory");
				xWUndoRedoManager.AddUndoMethod(this, "RefreshCardEditorFromHistory");
				xWUndoRedoManager.CommitAction();
			}
			_editingPacket.NotifyPropertyListChanged();
			NotifyCurrentResourceEdited();
			SaveCardResource(_editingPacket);
		}
	}

	private void UpdatePacketOverrideEntry()
	{
		bool flag = GodotObject.IsInstanceValid(_editingPacket) && GodotObject.IsInstanceValid(_editingPacket._override);
		if (GodotObject.IsInstanceValid(_overrideStatus))
		{
			_overrideStatus.Text = (flag ? $"已配置：{_editingPacket._override.type}，费用 {_editingPacket._override.cost}，冷却 {_editingPacket._override.packetCooldown:0.##}" : "当前卡牌未配置覆盖规则；创建后可比较原始卡牌与覆盖结果。");
		}
		if (GodotObject.IsInstanceValid(_createOverrideButton))
		{
			_createOverrideButton.Disabled = flag;
		}
		if (GodotObject.IsInstanceValid(_openOverrideButton))
		{
			_openOverrideButton.Disabled = !flag;
		}
		if (GodotObject.IsInstanceValid(_clearOverrideButton))
		{
			_clearOverrideButton.Disabled = !flag;
		}
	}

	private void RefreshCardLists()
	{
		if (_editingPacket != null)
		{
			RefreshCardLists(_editingPacket);
		}
	}

	private void RefreshCardLists(TowerDefensePacketConfig packet)
	{
		if (packet != null)
		{
			if (GodotObject.IsInstanceValid(_eventSummaryLabel))
			{
				_eventSummaryLabel.Text = $"事件与解锁 · 拿起 {packet.pressedActions?.Count ?? 0} · 种植 {packet.useSucceededActions?.Count ?? 0} · 解锁条件 {packet.unlockCheckList?.Count ?? 0}";
			}
			PopulateResourceList(_pressedActionsList, packet.pressedActions);
			PopulateResourceList(_useSucceededActionsList, packet.useSucceededActions);
			PopulateResourceList(_unlockConditionList, packet.unlockCheckList);
			if (GodotObject.IsInstanceValid(_pressedActionsList) && _selectedEventPressIndex >= 0 && _selectedEventPressIndex < _pressedActionsList.ItemCount)
			{
				_pressedActionsList.Select(_selectedEventPressIndex);
			}
			if (GodotObject.IsInstanceValid(_useSucceededActionsList) && _selectedEventPlantIndex >= 0 && _selectedEventPlantIndex < _useSucceededActionsList.ItemCount)
			{
				_useSucceededActionsList.Select(_selectedEventPlantIndex);
			}
			if (GodotObject.IsInstanceValid(_unlockConditionList) && _selectedUnlockIndex >= 0 && _selectedUnlockIndex < _unlockConditionList.ItemCount)
			{
				_unlockConditionList.Select(_selectedUnlockIndex);
			}
			RefreshCardBehaviorLists(packet);
			RefreshArmorList(packet);
		}
	}

	private void AddSummaryRows(TowerDefensePacketConfig packet)
	{
		AddItemIfMissing(PreviewList, "卡片: " + EmptyToPlaceholder(packet.name));
		AddItemIfMissing(PreviewList, "费用/冷却: " + GetEffectiveCostText(packet) + " / " + GetEffectiveCooldownText(packet));
		AddItemIfMissing(TimelineList, "卡片 key: " + EmptyToPlaceholder(packet.saveKey));
		AddItemIfMissing(TimelineList, $"事件: 拿起 {packet.pressedActions?.Count ?? 0}，种植 {packet.useSucceededActions?.Count ?? 0}");
		AddItemIfMissing(GraphList, $"行为顺序: 注册 {packet.behaviorIds?.Count ?? 0} → 内联 {packet.behaviors?.Count ?? 0} → 费用规则 {packet.changeCostList?.Count ?? 0}");
		AddItemIfMissing(GraphList, "卡片 -> characterConfig -> " + FormatCharacterBinding(packet.characterConfig));
		AddItemIfMissing(GraphList, "卡片 -> packetAnimeClip -> " + EmptyToPlaceholder(packet.packetAnimeClip));
		AddItemIfMissing(ReferenceList, "绑定角色 characterConfig -> " + FormatCharacterBinding(packet.characterConfig));
		AddItemIfMissing(ReferenceList, "卡牌动画 packetAnimeClip -> " + EmptyToPlaceholder(packet.packetAnimeClip));
		AddItemIfMissing(ReferenceList, $"解锁条件 unlockCheckList -> {packet.unlockCheckList?.Count ?? 0}");
	}

	private void BuildRuntimeStateCards()
	{
		if (!GodotObject.IsInstanceValid(_runtimeStateCards))
		{
			return;
		}
		ClearVisualCards(_runtimeStateCards);
		(string, string, string)[] array = new (string, string, string)[5]
		{
			("可种植", "准备", "res://addons/ModEditor/Icons/ResourceShovel.svg"),
			("已选中", "手持", "res://addons/ModEditor/Icons/Pin.svg"),
			("冷却中", "计时", "res://addons/ModEditor/Icons/Progress1.svg"),
			("阳光不足", "灰暗", "res://addons/ModEditor/Icons/NodeWarning.svg"),
			("已锁定", "禁用", "res://addons/ModEditor/Icons/Lock.svg")
		};
		for (int i = 0; i < array.Length; i++)
		{
			int selectedState = i;
			XWGameVisualChoiceCard xWGameVisualChoiceCard = CreateVisualChoiceCard(i.ToString(), array[i].Item1, array[i].Item2, array[i].Item3, i == _cardRuntimePreviewState, new Vector2(124f, 58f));
			if (GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
			{
				xWGameVisualChoiceCard.Pressed += () =>
				{
					SetCardRuntimePreviewState(selectedState);
				};
				_runtimeStateCards.AddChild(xWGameVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void SetCardRuntimePreviewState(int state)
	{
		if (!_updatingControls)
		{
			_cardRuntimePreviewState = Math.Clamp(state, 0, 4);
			if (_cardRuntimePreviewState == 2)
			{
				EnsureCardRuntimeCooldownDuration();
				_cardRuntimeCooldownRemaining = _cardRuntimeCooldownDuration * (double)Math.Max(0.001f, _cardRuntimeCooldownProgress);
			}
			else
			{
				StopCardRuntimeClock();
			}
			ApplyCardRuntimePreviewState();
		}
	}

	private void BuildPacketTypeCards(HFlowContainer host, TowerDefenseEnum.PACKET_TYPE value)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return;
		}
		ClearVisualCards(host);
		TowerDefenseEnum.PACKET_TYPE[] values = Enum.GetValues<TowerDefenseEnum.PACKET_TYPE>();
		for (int i = 0; i < values.Length; i++)
		{
			TowerDefenseEnum.PACKET_TYPE pACKET_TYPE = values[i];
			TowerDefenseEnum.PACKET_TYPE selectedType = pACKET_TYPE;
			int num = (int)pACKET_TYPE;
			XWGameVisualChoiceCard xWGameVisualChoiceCard = CreateVisualChoiceCard(num.ToString(), GetPacketTypeDisplayName(pACKET_TYPE), pACKET_TYPE.ToString(), GetPacketTexturePath(pACKET_TYPE), pACKET_TYPE == value, new Vector2(138f, 66f));
			if (!GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
			{
				continue;
			}
			xWGameVisualChoiceCard.Pressed += () =>
			{
				if (!_updatingControls)
				{
					_cardPropertyBinding?.SetValue(_editingPacket, "type", (int)selectedType, "修改卡牌品质", this, "RefreshCardEditorFromHistory");
					HFlowContainer host2 = host;
					int num2 = (int)selectedType;
					SelectVisualCard(host2, num2.ToString());
					UpdateCardPreview();
				}
			};
			host.AddChild(xWGameVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void BuildSpawnMethodCards(HFlowContainer host, TowerDefensePacketConfig packet)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return;
		}
		ClearVisualCards(host);
		(string, string, string, string)[] array = new (string, string, string, string)[3]
		{
			("Noone", "原地出现", "不播放入场位移", "res://addons/ModEditor/Icons/Pin.svg"),
			("Rise", "破土上升", "从地面升起", "res://addons/ModEditor/Icons/GuiArrowUp.svg"),
			("FlyDown", "空投落地", "从画面上方降落", "res://addons/ModEditor/Icons/MoveDown.svg")
		};
		for (int i = 0; i < array.Length; i++)
		{
			(string, string, string, string) tuple = array[i];
			string key = tuple.Item1;
			XWGameVisualChoiceCard xWGameVisualChoiceCard = CreateVisualChoiceCard(key, tuple.Item2, tuple.Item3, tuple.Item4, string.Equals(packet.spawnMethod, key, StringComparison.OrdinalIgnoreCase), new Vector2(160f, 66f));
			if (!GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
			{
				continue;
			}
			xWGameVisualChoiceCard.Pressed += () =>
			{
				if (!_updatingControls)
				{
					_cardPropertyBinding?.SetValue(packet, "spawnMethod", key, "修改卡牌出场方式", this, "RefreshCardEditorFromHistory");
					SelectVisualCard(host, key);
					UpdateCardPreview();
				}
			};
			host.AddChild(xWGameVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private static void ClearVisualCards(Control host)
	{
		foreach (Node child in host.GetChildren())
		{
			host.RemoveChild(child);
			child.QueueFree();
		}
	}

	private XWGameVisualChoiceCard CreateVisualChoiceCard(string key, string title, string detail, string iconPath, bool selected, Vector2 minimumSize)
	{
		if (_visualChoiceCardScene == null)
		{
			_visualChoiceCardScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		XWGameVisualChoiceCard xWGameVisualChoiceCard = _visualChoiceCardScene?.Instantiate<XWGameVisualChoiceCard>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
		{
			return null;
		}
		xWGameVisualChoiceCard.CustomMinimumSize = minimumSize;
		xWGameVisualChoiceCard.Configure(key, title, detail, ResourceLoader.Load<Texture2D>(iconPath, null, ResourceLoader.CacheMode.Reuse), selected);
		return xWGameVisualChoiceCard;
	}

	private static void SelectVisualCard(Control host, string selectedKey)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return;
		}
		foreach (Node child in host.GetChildren())
		{
			if (child is XWGameVisualChoiceCard xWGameVisualChoiceCard)
			{
				xWGameVisualChoiceCard.SetSelected(xWGameVisualChoiceCard.ChoiceKey == selectedKey);
			}
		}
	}

	private void BindCardVector2Field(SpinBox x, SpinBox y, TowerDefensePacketConfig packet, StringName property)
	{
		Vector2 vector = packet.Get(property).AsVector2();
		x.Value = vector.X;
		y.Value = vector.Y;
		Action value = () =>
		{
			_cardPropertyBinding?.BeginEdit(packet, property);
		};
		Godot.Range.ValueChangedEventHandler value2 = (double _) =>
		{
			if (!_updatingControls)
			{
				_cardPropertyBinding?.PreviewValue(packet, property, ReadValue());
				UpdateCardPreview();
			}
		};
		Action value3 = () =>
		{
			if (!_updatingControls)
			{
				_cardPropertyBinding?.CommitEdit(packet, property, ReadValue(), $"修改 {property}", this, "RefreshCardEditorFromHistory");
				UpdateCardPreview();
			}
		};
		x.FocusEntered += value;
		y.FocusEntered += value;
		x.ValueChanged += value2;
		y.ValueChanged += value2;
		x.FocusExited += value3;
		y.FocusExited += value3;
		Variant ReadValue()
		{
			return Variant.From<Vector2>(new Vector2((float)x.Value, (float)y.Value));
		}
	}

	private void RefreshCharacterBindingSelector(TowerDefenseCharacterConfig current)
	{
		if (!GodotObject.IsInstanceValid(_characterBindingButton) && !GodotObject.IsInstanceValid(_characterSourceLabel))
		{
			return;
		}
		if (GodotObject.IsInstanceValid(_characterBindingButton))
		{
			if (!GodotObject.IsInstanceValid(current))
			{
				_characterBindingButton.Text = "选择角色...";
			}
			else
			{
				CardCharacterSource source = ResolveCharacterSource(current.ResourcePath);
				string characterKey = GetCharacterKey(current, current.ResourcePath);
				_characterBindingButton.Text = "更换: " + GetCharacterSourceTag(source) + " " + BuildCharacterDisplayName(characterKey, current.ResourcePath);
			}
		}
		UpdateCharacterSourceLabel(current);
	}

	private void ShowCharacterSelectorWindow()
	{
		EnsureCharacterResourcePicker();
		if (GodotObject.IsInstanceValid(_characterResourcePicker))
		{
			TowerDefenseCharacterConfig towerDefenseCharacterConfig = _editingPacket?.characterConfig;
			string currentPath = (GodotObject.IsInstanceValid(towerDefenseCharacterConfig) ? towerDefenseCharacterConfig.ResourcePath : "");
			string projectPath = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
			_characterResourcePicker.OpenResourceLibrary("Character", "角色", currentPath, projectPath, new string[1] { "TowerDefenseCharacterConfig" }, new string[1] { "Asset/Anime/Character" }, "res://addons/ModEditor/Icons/ResourceCharacter.svg", ApplySelectedCharacterChoice);
		}
	}

	private void EnsureCharacterResourcePicker()
	{
		if (!GodotObject.IsInstanceValid(_characterResourcePicker))
		{
			_characterResourcePicker = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_characterResourcePicker))
			{
				AddChild(_characterResourcePicker, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void ApplySelectedCharacterChoice(XWGameplayResourceChoice choice)
	{
		if (!_updatingControls && !(choice == null))
		{
			TowerDefenseCharacterConfig towerDefenseCharacterConfig = LoadCharacterConfig(choice.ResourcePath);
			if (!GodotObject.IsInstanceValid(towerDefenseCharacterConfig))
			{
				GD.PushWarning("Card character binding failed: " + choice.ResourcePath);
				return;
			}
			SetCharacterBindingResource(towerDefenseCharacterConfig);
			UpdateCharacterSourceLabel(towerDefenseCharacterConfig);
		}
	}

	private void ClearCharacterBinding()
	{
		SetCharacterBindingResource(null);
		UpdateCharacterSourceLabel(null);
	}

	private void SetCharacterBindingResource(TowerDefenseCharacterConfig character)
	{
		if (_editingPacket != null)
		{
			_cardPropertyBinding?.SetValue(_editingPacket, "characterConfig", Variant.From(in character), "更换卡牌角色绑定", this, "RefreshCardEditorFromHistory");
			_editingPacket.NotifyPropertyListChanged();
			UpdateCardPreview();
			XWEditorInterface.Instance?.AddOutputMessage("卡片编辑器: 绑定角色 " + FormatCharacterBinding(character));
		}
	}

	private static TowerDefenseCharacterConfig LoadCharacterConfig(string resourcePath)
	{
		if (string.IsNullOrWhiteSpace(resourcePath))
		{
			return null;
		}
		if (!IsCharacterConfigResourcePath(resourcePath))
		{
			return null;
		}
		try
		{
			return (ResourceLoader.Load<Resource>(resourcePath, "", ResourceLoader.CacheMode.Ignore) is TowerDefenseCharacterConfig towerDefenseCharacterConfig) ? towerDefenseCharacterConfig : null;
		}
		catch (Exception ex)
		{
			GD.PushWarning("Card character option load failed: " + resourcePath + " " + ex.Message);
			return null;
		}
	}

	private static bool IsCharacterConfigResourcePath(string resourcePath)
	{
		string text = NormalizeResourcePath(resourcePath);
		if (string.IsNullOrWhiteSpace(text))
		{
			return false;
		}
		string text2 = text.GetExtension().ToLowerInvariant();
		if (!(text2 == "tres") && !(text2 == "res"))
		{
			return false;
		}
		if (!text.Contains("/Config/", StringComparison.OrdinalIgnoreCase) && !text.EndsWith("Config.tres", StringComparison.OrdinalIgnoreCase) && !text.EndsWith("Config.res", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return true;
	}

	private void UpdateCharacterSourceLabel(TowerDefenseCharacterConfig current)
	{
		if (GodotObject.IsInstanceValid(_characterSourceLabel))
		{
			if (!GodotObject.IsInstanceValid(current))
			{
				_characterSourceLabel.Text = "来源: 未绑定";
				return;
			}
			CardCharacterSource cardCharacterSource = ResolveCharacterSource(current.ResourcePath);
			_characterSourceLabel.Text = ((cardCharacterSource == CardCharacterSource.Mod) ? ("来源: Mod 角色  " + FormatCharacterBinding(current)) : ("来源: 游戏内置角色  " + FormatCharacterBinding(current)));
		}
	}

	private static string GetCharacterSourceTag(CardCharacterSource source)
	{
		return source switch
		{
			CardCharacterSource.Mod => "[Mod]", 
			CardCharacterSource.BuiltIn => "[内置]", 
			_ => "[Mod]", 
		};
	}

	private static CardCharacterSource ResolveCharacterSource(string resourcePath)
	{
		if (IsBuiltInCharacterPath(resourcePath))
		{
			return CardCharacterSource.BuiltIn;
		}
		return CardCharacterSource.Mod;
	}

	private static bool IsBuiltInCharacterPath(string resourcePath)
	{
		string text = NormalizeResourcePath(resourcePath);
		if (text.StartsWith("res://Asset/Anime/Character", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		string text2 = NormalizeResourcePath(ProjectSettings.GlobalizePath("res://Asset/Anime/Character")).TrimEnd('/');
		if (!string.IsNullOrWhiteSpace(text2))
		{
			return text.StartsWith(text2 + "/", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static string NormalizeResourcePath(string path)
	{
		return (path ?? "").Replace('\\', '/').Trim();
	}

	private static string GetCharacterKey(TowerDefenseCharacterConfig character, string resourcePath)
	{
		if (GodotObject.IsInstanceValid(character) && !string.IsNullOrWhiteSpace(character.name))
		{
			return character.name;
		}
		if (GodotObject.IsInstanceValid(character) && !string.IsNullOrWhiteSpace(character.ResourceName))
		{
			return character.ResourceName;
		}
		if (!string.IsNullOrWhiteSpace(resourcePath))
		{
			return resourcePath.GetFile().GetBaseName();
		}
		return "未命名角色";
	}

	private static string BuildCharacterDisplayName(string key, string resourcePath)
	{
		string text = (string.IsNullOrWhiteSpace(resourcePath) ? "" : resourcePath.GetFile());
		if (string.IsNullOrWhiteSpace(text) || string.Equals(text.GetBaseName(), key, StringComparison.OrdinalIgnoreCase))
		{
			return EmptyToPlaceholder(key);
		}
		return EmptyToPlaceholder(key) + " (" + text + ")";
	}

	private static void PopulateResourceList<[MustBeVariant] T>(ItemList list, Array<T> array)
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

	private void RefreshPacketShowPreview(TowerDefensePacketConfig packet)
	{
		if (!GodotObject.IsInstanceValid(_cardPacketShowPreviewRoot))
		{
			return;
		}
		if (packet == null || !GodotObject.IsInstanceValid(packet.characterConfig))
		{
			if (GodotObject.IsInstanceValid(_cardPacketShowPreview))
			{
				_cardPacketShowPreview.QueueFree();
				_cardPacketShowPreview = null;
			}
			_cardPacketShowSpriteFingerprint = "";
			_cardPacketShowAnimationFingerprint = "";
			_cardPacketShowSaveKeyFingerprint = "";
			SetPacketShowPreviewMissing(missing: true);
			return;
		}
		if (GodotObject.IsInstanceValid(_cardPacketShowPreview))
		{
			ConfigurePacketShowPreview(_cardPacketShowPreview, packet, forceSpriteRebuild: false, trackMainPreview: true);
			SetPacketShowPreviewMissing(missing: false);
			PositionPacketShowPreview();
			ApplyCardRuntimePreviewState();
			return;
		}
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = null;
		try
		{
			towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
			if (!GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				SetPacketShowPreviewMissing(missing: true);
				return;
			}
			towerDefenseInGamePacketShow.Name = "CardPacketShowPreview";
			towerDefenseInGamePacketShow.onlyDraw = true;
			towerDefenseInGamePacketShow.setPcLayout = !_cardRuntimeMobile;
			towerDefenseInGamePacketShow.setMobileLayout = _cardRuntimeMobile;
			towerDefenseInGamePacketShow.showLove = false;
			towerDefenseInGamePacketShow.MouseFilter = MouseFilterEnum.Ignore;
			towerDefenseInGamePacketShow.ProcessMode = ProcessModeEnum.Inherit;
			_cardPacketShowPreviewRoot.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
			ConfigurePacketShowPreview(towerDefenseInGamePacketShow, packet, forceSpriteRebuild: true, trackMainPreview: true);
			_cardPacketShowPreview = towerDefenseInGamePacketShow;
			_cardPacketShowPreviewBuildCount++;
			SetPacketShowPreviewMissing(missing: false);
			PositionPacketShowPreview();
			ApplyCardRuntimePreviewState();
		}
		catch (Exception ex)
		{
			if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				towerDefenseInGamePacketShow.QueueFree();
			}
			_cardPacketShowPreview = null;
			SetPacketShowPreviewMissing(missing: true);
			GD.PushWarning("Card PacketShow preview failed: " + ex.Message);
		}
	}

	private void ConfigurePacketShowPreview(TowerDefenseInGamePacketShow packetShow, TowerDefensePacketConfig packet, bool forceSpriteRebuild = false, bool trackMainPreview = false)
	{
		if (!GodotObject.IsInstanceValid(packetShow) || packet == null)
		{
			return;
		}
		XWCardBehaviorPreviewSnapshot xWCardBehaviorPreviewSnapshot = XWCardBehaviorPreviewSafety.Capture(packet);
		packetShow.onlyDraw = true;
		ApplyPacketShowPreviewLayout(packetShow);
		packetShow.showLove = false;
		packetShow.select = false;
		packetShow.alive = true;
		packetShow.@lock = false;
		packetShow.MouseFilter = MouseFilterEnum.Ignore;
		string text = BuildPacketShowSaveKeyFingerprint(packet);
		string text2 = BuildPacketShowSpriteFingerprint(packet);
		string text3 = packet.packetAnimeClip ?? "";
		bool flag = trackMainPreview && !string.Equals(text, _cardPacketShowSaveKeyFingerprint, StringComparison.Ordinal);
		bool flag2 = trackMainPreview && !string.Equals(text2, _cardPacketShowSpriteFingerprint, StringComparison.Ordinal);
		bool flag3 = forceSpriteRebuild || !GodotObject.IsInstanceValid(packetShow.sprite) || (flag2 && (!flag || _cardPreviewRefreshCommitted)) || (flag && _cardPreviewRefreshCommitted);
		bool animationChanged = (!flag3 & trackMainPreview) && !string.Equals(text3, _cardPacketShowAnimationFingerprint, StringComparison.Ordinal);
		if (flag3)
		{
			EnsurePacketPreviewCharacterSpriteRegistered(packet);
			text2 = BuildPacketShowSpriteFingerprint(packet);
			ulong num = (GodotObject.IsInstanceValid(packetShow.sprite) ? packetShow.sprite.GetInstanceId() : 0);
			packetShow.InitVisualPreview(packet, xWCardBehaviorPreviewSnapshot.Cost, xWCardBehaviorPreviewSnapshot.CostRise, xWCardBehaviorPreviewSnapshot.CostMultiple, xWCardBehaviorPreviewSnapshot.Cooldown);
			if (trackMainPreview && GodotObject.IsInstanceValid(packetShow.sprite) && packetShow.sprite.GetInstanceId() != num)
			{
				_cardPacketShowSpriteRebuildCount++;
			}
			if (trackMainPreview)
			{
				DisposeCardDragGhost();
			}
		}
		else
		{
			ApplyPacketShowPreviewIncremental(packetShow, packet, animationChanged);
		}
		if (trackMainPreview)
		{
			if (flag3 || !flag || _cardPreviewRefreshCommitted)
			{
				_cardPacketShowSpriteFingerprint = text2;
			}
			_cardPacketShowAnimationFingerprint = text3;
			if (flag3 || _cardPreviewRefreshCommitted)
			{
				_cardPacketShowSaveKeyFingerprint = text;
			}
		}
		packetShow.onlyDraw = true;
	}

	private void ApplyPacketShowPreviewIncremental(TowerDefenseInGamePacketShow packetShow, TowerDefensePacketConfig packet, bool animationChanged)
	{
		if (!GodotObject.IsInstanceValid(packetShow) || packet == null)
		{
			return;
		}
		XWCardBehaviorPreviewSnapshot xWCardBehaviorPreviewSnapshot = XWCardBehaviorPreviewSafety.Capture(packet);
		packetShow.config = packet;
		packetShow.riseCost = xWCardBehaviorPreviewSnapshot.CostRise;
		packetShow.costMultiple = xWCardBehaviorPreviewSnapshot.CostMultiple;
		packetShow.baseItemCost = xWCardBehaviorPreviewSnapshot.Cost;
		packetShow.itemCost = packetShow.baseItemCost;
		packetShow.coldDown = xWCardBehaviorPreviewSnapshot.Cooldown;
		packetShow.InvalidateRuntimeState();
		packetShow.UpdateBackgroundTexture();
		packetShow.UpdateSpriteLayout();
		if (GodotObject.IsInstanceValid(packetShow.coldDownProgressBar))
		{
			packetShow.coldDownProgressBar.MaxValue = Math.Max(0.001, packetShow.coldDown);
		}
		if (GodotObject.IsInstanceValid(packetShow.sprite))
		{
			if (animationChanged)
			{
				RefreshPacketShowAnimationInPlace(packetShow, packet.packetAnimeClip ?? "");
			}
			else
			{
				packetShow.sprite.Visible = true;
			}
		}
		if (GodotObject.IsInstanceValid(packetShow.previewClip))
		{
			packetShow.previewClip.Visible = GodotObject.IsInstanceValid(packetShow.sprite);
		}
		if (GodotObject.IsInstanceValid(packetShow.previewSpriteNode))
		{
			packetShow.previewSpriteNode.Visible = GodotObject.IsInstanceValid(packetShow.sprite);
		}
		packetShow.ColorSet();
	}

	private void RefreshPacketShowAnimationInPlace(TowerDefenseInGamePacketShow packetShow, string clip)
	{
		if (GodotObject.IsInstanceValid(packetShow?.sprite))
		{
			if (_cardRuntimeHovered)
			{
				packetShow.OnMouseEntered();
				return;
			}
			packetShow.sprite.SetAnimation(clip);
			packetShow.RefreshPreview();
		}
	}

	private static string BuildPacketShowSpriteFingerprint(TowerDefensePacketConfig packet)
	{
		if (packet == null || !GodotObject.IsInstanceValid(packet.characterConfig))
		{
			return "";
		}
		TowerDefenseCharacterConfig characterConfig = packet.characterConfig;
		ulong instanceId = characterConfig.GetInstanceId();
		ulong num = (GodotObject.IsInstanceValid(characterConfig.armorData) ? characterConfig.armorData.GetInstanceId() : 0);
		ulong num2 = (GodotObject.IsInstanceValid(characterConfig.customData) ? characterConfig.customData.GetInstanceId() : 0);
		PackedScene packetSpriteScene = TowerDefenseManager.GetPacketSpriteScene(packet);
		ulong num3 = (GodotObject.IsInstanceValid(packetSpriteScene) ? packetSpriteScene.GetInstanceId() : 0);
		_003C_003Ey__InlineArray9<object> buffer = default;
		buffer[0] = instanceId;
		buffer[1] = characterConfig.ResourcePath ?? "";
		buffer[2] = characterConfig.name ?? "";
		buffer[3] = num;
		buffer[4] = num2;
		buffer[5] = num3;
		buffer[6] = packetSpriteScene?.ResourcePath ?? "";
		buffer[7] = XWCardBehaviorPreviewSafety.ResolveBaseOverrideHypnoses(packet);
		buffer[8] = BuildCardArmorFingerprint(packet.initArmor);
		return string.Join('\u001e', (ReadOnlySpan<object?>)buffer);
	}

	private static string BuildPacketShowSaveKeyFingerprint(TowerDefensePacketConfig packet)
	{
		return packet?.saveKey ?? "";
	}

	private static double ResolveConfiguredPacketCooldown(TowerDefensePacketConfig packet)
	{
		double val = XWCardBehaviorPreviewSafety.ResolveBaseOverrideCooldown(packet, out var _);
		return Math.Max(0.0, val);
	}

	private void ApplyPacketShowPreviewLayout(TowerDefenseInGamePacketShow packetShow)
	{
		if (GodotObject.IsInstanceValid(packetShow))
		{
			packetShow.setPcLayout = !_cardRuntimeMobile;
			packetShow.setMobileLayout = _cardRuntimeMobile;
			packetShow.isMobile = _cardRuntimeMobile;
			if (_cardRuntimeMobile)
			{
				packetShow.MobilePreset();
			}
			else
			{
				packetShow.SetPcPreset();
			}
		}
	}

	private static bool EnsurePacketPreviewCharacterSpriteRegistered(TowerDefensePacketConfig packet)
	{
		TowerDefenseCharacterConfig towerDefenseCharacterConfig = packet?.characterConfig;
		if (!GodotObject.IsInstanceValid(towerDefenseCharacterConfig) || string.IsNullOrWhiteSpace(towerDefenseCharacterConfig.name))
		{
			return false;
		}
		if (ResourceManager.Instance == null)
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(TowerDefenseManager.GetPacketSpriteScene(packet)))
		{
			return true;
		}
		if (!TryResolveCardSpriteScene(packet, out var spriteScene))
		{
			return false;
		}
		XWModRuntimeRegistry.Register("ModEditorPreview", "CharacterSprite", towerDefenseCharacterConfig.name, Variant.From(in spriteScene), allowOverride: true);
		if (!string.IsNullOrWhiteSpace(packet.saveKey) && !string.Equals(packet.saveKey, towerDefenseCharacterConfig.name, StringComparison.OrdinalIgnoreCase))
		{
			XWModRuntimeRegistry.Register("ModEditorPreview", "CharacterSprite", packet.saveKey, Variant.From(in spriteScene), allowOverride: true);
		}
		return GodotObject.IsInstanceValid(TowerDefenseManager.GetPacketSpriteScene(packet));
	}

	private static bool TryResolveCardSpriteScene(TowerDefensePacketConfig packet, out PackedScene spriteScene)
	{
		spriteScene = TowerDefenseManager.GetPacketSpriteScene(packet);
		if (GodotObject.IsInstanceValid(spriteScene))
		{
			return true;
		}
		if (!GodotObject.IsInstanceValid(packet?.characterConfig))
		{
			return false;
		}
		List<string> list = new List<string>();
		CollectCardSpriteSceneCandidates(packet, list);
		foreach (string item in list)
		{
			if (ResourceLoader.Exists(item))
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>(item, null, ResourceLoader.CacheMode.Reuse);
				if (IsAdobeAnimateSpriteScene(packedScene))
				{
					spriteScene = packedScene;
					return true;
				}
			}
		}
		return false;
	}

	private static void CollectCardSpriteSceneCandidates(TowerDefensePacketConfig packet, List<string> candidates)
	{
		if (packet != null && candidates != null)
		{
			string characterName = packet.characterConfig?.name ?? "";
			CollectCardSpriteSceneCandidates(packet.characterConfig?.ResourcePath, characterName, candidates);
			CollectCardSpriteSceneCandidates(packet.ResourcePath, characterName, candidates);
		}
	}

	private static void CollectCardSpriteSceneCandidates(string resourcePath, string characterName, List<string> candidates)
	{
		string text = NormalizeResourcePath(resourcePath);
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		string baseDir = text.GetBaseDir();
		string file = baseDir.GetFile();
		string text2 = ((file.Equals("Config", StringComparison.OrdinalIgnoreCase) || file.Equals("Packet", StringComparison.OrdinalIgnoreCase)) ? baseDir.GetBaseDir() : baseDir);
		if (!string.IsNullOrWhiteSpace(text2))
		{
			if (!string.IsNullOrWhiteSpace(characterName))
			{
				AddCardSpriteSceneCandidate(candidates, text2.PathJoin("Sprite").PathJoin(characterName + ".tscn"));
				AddCardSpriteSceneCandidate(candidates, text2.PathJoin(characterName + ".tscn"));
			}
			AddCardSpriteSceneCandidate(candidates, text2.PathJoin(text2.GetFile() + ".tscn"));
		}
	}

	private static void AddCardSpriteSceneCandidate(List<string> candidates, string candidate)
	{
		candidate = NormalizeResourcePath(candidate);
		if (!string.IsNullOrWhiteSpace(candidate) && !candidates.Contains(candidate))
		{
			candidates.Add(candidate);
		}
	}

	private static bool IsAdobeAnimateSpriteScene(PackedScene scene)
	{
		if (!GodotObject.IsInstanceValid(scene))
		{
			return false;
		}
		Node node = null;
		try
		{
			node = scene.Instantiate(PackedScene.GenEditState.Disabled);
			return node is AdobeAnimateSprite;
		}
		catch
		{
			return false;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(node))
			{
				node.Free();
			}
		}
	}

	private void PositionPacketShowPreview()
	{
		if (GodotObject.IsInstanceValid(_cardPacketShowPreviewRoot) && GodotObject.IsInstanceValid(_cardPacketShowPreview))
		{
			Vector2 vector = _cardPacketShowPreviewRoot.Size;
			if (vector.X <= 1f || vector.Y <= 1f)
			{
				vector = _cardPacketShowPreviewRoot.CustomMinimumSize;
			}
			_cardPacketShowPreview.Position = new Vector2(vector.X * 0.5f, vector.Y * 0.5f);
			_cardPacketShowPreview.Scale = Vector2.One * (_cardRuntimeMobile ? 1.25f : 1.35f);
		}
	}

	private void OnCardInteractionGuiInput(InputEvent inputEvent)
	{
		if (!GodotObject.IsInstanceValid(_cardInteractionButton) || !IsVisibleInTree())
		{
			return;
		}
		if (inputEvent is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			Vector2 globalPosition = inputEventMouseButton.GlobalPosition;
			if (inputEventMouseButton.Pressed)
			{
				BeginCardPointerSession(globalPosition, touch: false, -1);
			}
			else
			{
				CompleteCardPointerSession(globalPosition, touch: false, -1);
			}
			_cardInteractionButton.AcceptEvent();
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && !_cardDragTouch)
		{
			HandleCardPointerMotion(inputEventMouseMotion.GlobalPosition, touch: false, -1);
			if (_cardDragCandidate || _cardDragActive)
			{
				_cardInteractionButton.AcceptEvent();
			}
		}
		else if (inputEvent is InputEventScreenTouch { Position: var position } inputEventScreenTouch)
		{
			if (inputEventScreenTouch.Pressed)
			{
				BeginCardPointerSession(position, touch: true, inputEventScreenTouch.Index);
			}
			else
			{
				CompleteCardPointerSession(position, touch: true, inputEventScreenTouch.Index);
			}
			_cardInteractionButton.AcceptEvent();
		}
		else if (inputEvent is InputEventScreenDrag inputEventScreenDrag)
		{
			HandleCardPointerMotion(inputEventScreenDrag.Position, touch: true, inputEventScreenDrag.Index);
			if (_cardDragCandidate || _cardDragActive)
			{
				_cardInteractionButton.AcceptEvent();
			}
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!_cardDragCandidate && !_cardDragActive)
		{
			if (IsVisibleInTree() && GodotObject.IsInstanceValid(_cardInteractionButton))
			{
				if (inputEvent is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left && inputEventMouseButton.Pressed && IsCanvasPointInsideControl(_cardInteractionButton, inputEventMouseButton.GlobalPosition))
				{
					BeginCardPointerSession(inputEventMouseButton.GlobalPosition, touch: false, -1);
				}
				else if (inputEvent is InputEventScreenTouch { Pressed: not false } inputEventScreenTouch && IsCanvasPointInsideControl(_cardInteractionButton, inputEventScreenTouch.Position))
				{
					BeginCardPointerSession(inputEventScreenTouch.Position, touch: true, inputEventScreenTouch.Index);
				}
			}
			return;
		}
		if (!IsVisibleInTree())
		{
			CancelCardDragSession(countCancellation: true, "页面已隐藏，拖拽预览已取消");
			return;
		}
		bool flag = false;
		if (inputEvent is InputEventMouseMotion inputEventMouseMotion && !_cardDragTouch)
		{
			HandleCardPointerMotion(inputEventMouseMotion.GlobalPosition, touch: false, -1);
			flag = true;
		}
		else if (inputEvent is InputEventMouseButton inputEventMouseButton2 && inputEventMouseButton2.ButtonIndex == MouseButton.Left && !inputEventMouseButton2.Pressed && !_cardDragTouch)
		{
			flag = _cardDragActive;
			CompleteCardPointerSession(inputEventMouseButton2.GlobalPosition, touch: false, -1);
		}
		else if (inputEvent is InputEventScreenDrag inputEventScreenDrag && _cardDragTouch && inputEventScreenDrag.Index == _cardDragTouchIndex)
		{
			HandleCardPointerMotion(inputEventScreenDrag.Position, touch: true, inputEventScreenDrag.Index);
			flag = true;
		}
		else if (inputEvent is InputEventScreenTouch { Pressed: false } inputEventScreenTouch2 && _cardDragTouch && inputEventScreenTouch2.Index == _cardDragTouchIndex)
		{
			flag = _cardDragActive;
			CompleteCardPointerSession(inputEventScreenTouch2.Position, touch: true, inputEventScreenTouch2.Index);
		}
		else if (inputEvent is InputEventKey { Pressed: not false } inputEventKey && inputEventKey.Keycode == Key.Escape)
		{
			CancelCardDragSession(countCancellation: true, "已取消卡牌拖拽");
			flag = true;
		}
		if (flag)
		{
			GetViewport()?.SetInputAsHandled();
		}
	}

	private static Vector2 CanvasPointToControlLocal(Control control, Vector2 canvasPosition)
	{
		if (!GodotObject.IsInstanceValid(control))
		{
			return canvasPosition;
		}
		return control.GetGlobalTransformWithCanvas().AffineInverse() * canvasPosition;
	}

	private static bool IsCanvasPointInsideControl(Control control, Vector2 canvasPosition)
	{
		if (!GodotObject.IsInstanceValid(control))
		{
			return false;
		}
		Vector2 point = CanvasPointToControlLocal(control, canvasPosition);
		return new Rect2(Vector2.Zero, control.Size).HasPoint(point);
	}

	private void BeginCardPointerSession(Vector2 globalPosition, bool touch, int touchIndex)
	{
		if (!_cardDragCandidate && !_cardDragActive)
		{
			if (TryGetCardPlacementBlockReason(out var reason))
			{
				_cardDragRejectedCount++;
				ShowCardPlacementMessage("无法拿起卡牌 · " + reason, valid: false);
				return;
			}
			_cardDragCandidate = true;
			_cardDragTouch = touch;
			_cardDragTouchIndex = (touch ? touchIndex : (-1));
			_cardDragStartGlobalPosition = globalPosition;
			_cardDragPointerGlobalPosition = globalPosition;
			SetProcessInput(enable: true);
		}
	}

	private void HandleCardPointerMotion(Vector2 globalPosition, bool touch, int touchIndex)
	{
		if ((_cardDragCandidate || _cardDragActive) && touch == _cardDragTouch && (!touch || touchIndex == _cardDragTouchIndex))
		{
			_cardDragPointerGlobalPosition = globalPosition;
			if (!_cardDragActive && globalPosition.DistanceSquaredTo(_cardDragStartGlobalPosition) >= 100f)
			{
				StartCardDragSession();
			}
			if (_cardDragActive)
			{
				UpdateCardDragTarget(globalPosition);
			}
		}
	}

	private void StartCardDragSession()
	{
		if (!_cardDragCandidate || _cardDragActive)
		{
			return;
		}
		if (TryGetCardPlacementBlockReason(out var reason))
		{
			_cardDragRejectedCount++;
			CancelCardDragSession(countCancellation: false, "无法拖拽 · " + reason);
			return;
		}
		_cardDragActive = true;
		EnsureCardDragGhost();
		if (GodotObject.IsInstanceValid(_cardDragPacketGhost))
		{
			_cardDragPacketGhost.Show();
		}
		_cardRuntimePreviewState = 1;
		ApplyCardRuntimePreviewState();
		UpdateCardDragTarget(_cardDragPointerGlobalPosition);
	}

	private void CompleteCardPointerSession(Vector2 globalPosition, bool touch, int touchIndex)
	{
		if ((!_cardDragCandidate && !_cardDragActive) || touch != _cardDragTouch || (touch && touchIndex != _cardDragTouchIndex))
		{
			return;
		}
		if (!_cardDragActive)
		{
			ClearCardPointerSession();
			return;
		}
		UpdateCardDragTarget(globalPosition);
		if (_cardDragTargetValid && IsCardBattleCell(_cardDragPreviewCell))
		{
			PlaceCardPreviewAtCell(_cardDragPreviewCell);
			return;
		}
		bool flag = IsCanvasPointInsideControl(_cardBattleDropSurface, globalPosition);
		_cardDragRejectedCount++;
		CancelCardDragSession(!flag, flag ? "落点无效 · 未扣除阳光，也未进入冷却" : "已在草坪外取消 · 未扣除阳光，也未进入冷却");
	}

	private void ClearCardPointerSession()
	{
		_cardDragCandidate = false;
		_cardDragTouch = false;
		_cardDragTouchIndex = -1;
		SetProcessInput(IsVisibleInTree());
	}

	private void EnsureCardDragGhost()
	{
		if (GodotObject.IsInstanceValid(_cardDragPacketGhost) || !GodotObject.IsInstanceValid(_cardBattleDropSurface) || !GodotObject.IsInstanceValid(_editingPacket) || !GodotObject.IsInstanceValid(_editingPacket.characterConfig))
		{
			return;
		}
		try
		{
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
			if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				towerDefenseInGamePacketShow.Name = "CardDragPacketGhost";
				towerDefenseInGamePacketShow.ZIndex = 220;
				towerDefenseInGamePacketShow.MouseFilter = MouseFilterEnum.Ignore;
				towerDefenseInGamePacketShow.Modulate = new Color(1f, 1f, 1f, 0.86f);
				towerDefenseInGamePacketShow.onlyDraw = true;
				_cardBattleDropSurface.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
				ConfigurePacketShowPreview(towerDefenseInGamePacketShow, _editingPacket, forceSpriteRebuild: true);
				towerDefenseInGamePacketShow.ProcessMode = ProcessModeEnum.Disabled;
				towerDefenseInGamePacketShow.Scale = Vector2.One * (_cardRuntimeMobile ? 1.05f : 1.15f);
				towerDefenseInGamePacketShow.Hide();
				_cardDragPacketGhost = towerDefenseInGamePacketShow;
			}
		}
		catch (Exception ex)
		{
			GD.PushWarning("Card drag ghost preview failed: " + ex.Message);
			DisposeCardDragGhost();
		}
	}

	private void DisposeCardDragGhost()
	{
		if (GodotObject.IsInstanceValid(_cardDragPacketGhost) && !_cardDragPacketGhost.IsQueuedForDeletion())
		{
			_cardDragPacketGhost.QueueFree();
		}
		_cardDragPacketGhost = null;
	}

	private void UpdateCardDragTarget(Vector2 globalPosition)
	{
		if (_cardDragActive && GodotObject.IsInstanceValid(_cardBattleDropSurface))
		{
			Vector2 vector = CanvasPointToControlLocal(_cardBattleDropSurface, globalPosition);
			Rect2 rect = new Rect2(Vector2.Zero, _cardBattleDropSurface.Size);
			if (GodotObject.IsInstanceValid(_cardDragPacketGhost))
			{
				_cardDragPacketGhost.Position = vector + new Vector2(18f, 8f);
			}
			bool flag = rect.HasPoint(vector);
			bool flag2 = TryGetCardBattleCell(vector, out var cell);
			_cardDragPreviewCell = (flag2 ? cell : new Vector2I(-1, -1));
			_cardDragTargetValid = flag2 && !TryGetCardPlacementBlockReason(out var _) && (!_cardHasPlacedPreview || cell != _cardPlacedPreviewCell);
			if (flag2)
			{
				Rect2 rect2 = GetCardBattleCellRect(_cardBattleDropSurface.Size, cell).Grow(-2f);
				ShowCardDragTarget(rect2, _cardDragTargetValid);
				PreviewCharacterAtCardCell(cell, _cardDragTargetValid);
			}
			else if (flag)
			{
				Vector2 vector2 = GetCardBattleCellSize(_cardBattleDropSurface.Size) * 0.72f;
				Vector2 position = vector - vector2 * 0.5f;
				position.X = Mathf.Clamp(position.X, 0f, Math.Max(0f, _cardBattleDropSurface.Size.X - vector2.X));
				position.Y = Mathf.Clamp(position.Y, 0f, Math.Max(0f, _cardBattleDropSurface.Size.Y - vector2.Y));
				ShowCardDragTarget(new Rect2(position, vector2), valid: false);
				RestoreCardCharacterPreviewPosition();
			}
			else
			{
				HideCardDragTarget();
				RestoreCardCharacterPreviewPosition();
			}
			if (GodotObject.IsInstanceValid(_cardBattleTint))
			{
				_cardBattleTint.Color = (_cardDragTargetValid ? new Color(0.04f, 0.22f, 0.04f, 0.16f) : new Color(0.28f, 0.035f, 0.025f, flag ? 0.23f : 0.12f));
			}
			XWCardVisualResourceEditor xWCardVisualResourceEditor = this;
			string message;
			if (_cardDragTargetValid)
			{
				message = $"可种植 · 第 {_cardDragPreviewCell.X} 列 / 第 {_cardDragPreviewCell.Y} 行";
			}
			else
			{
				message = (flag ? "红色区域不可种植 · 请移到草坪格子" : "拖到草坪内的绿色格子后松开");
			}
			xWCardVisualResourceEditor.ShowCardPlacementMessage(message, _cardDragTargetValid);
		}
	}

	private void ShowCardDragTarget(Rect2 rect, bool valid)
	{
		if (GodotObject.IsInstanceValid(_cardDragTargetHighlight))
		{
			_cardDragTargetHighlight.Position = rect.Position;
			_cardDragTargetHighlight.Size = rect.Size;
			_cardDragTargetHighlight.Color = (valid ? CardDragValidColor : CardDragInvalidColor);
			_cardDragTargetHighlight.Show();
		}
	}

	private void HideCardDragTarget()
	{
		if (GodotObject.IsInstanceValid(_cardDragTargetHighlight))
		{
			_cardDragTargetHighlight.Hide();
		}
	}

	private bool TryGetCardPlacementBlockReason(out string reason)
	{
		reason = "";
		if (!GodotObject.IsInstanceValid(_editingPacket) || !GodotObject.IsInstanceValid(_editingPacket.characterConfig))
		{
			reason = "尚未绑定可预览角色";
			return true;
		}
		int cardRuntimePreviewState = _cardRuntimePreviewState;
		if ((uint)(cardRuntimePreviewState - 2) <= 2u)
		{
			reason = _cardRuntimePreviewState switch
			{
				2 => "卡牌仍在冷却", 
				3 => "阳光不足", 
				_ => "卡牌尚未解锁", 
			};
			return true;
		}
		if (_cardRuntimeCooldownRemaining > 0.001)
		{
			reason = "卡牌仍在冷却";
			return true;
		}
		long effectiveCost = GetEffectiveCost(_editingPacket);
		if (effectiveCost < 0)
		{
			reason = "卡牌费用不可用";
			return true;
		}
		double num = _previewSunSpinBox?.Value ?? 0.0;
		if (num < (double)effectiveCost)
		{
			reason = $"阳光不足（{Mathf.RoundToInt((float)num)} / {effectiveCost}）";
			return true;
		}
		return false;
	}

	private void PlaceCardPreviewAtCell(Vector2I cell)
	{
		string reason = (IsCardBattleCell(cell) ? "" : "目标不在草坪格子内");
		if (!IsCardBattleCell(cell) || TryGetCardPlacementBlockReason(out reason))
		{
			_cardDragRejectedCount++;
			bool flag = _cardDragCandidate || _cardDragActive;
			CancelCardDragSession(flag, "放置失败 · " + reason);
			if (!flag)
			{
				ShowCardPlacementMessage("放置失败 · " + reason, valid: false);
			}
			return;
		}
		_cardHasPlacedPreview = true;
		_cardPlacedPreviewCell = cell;
		_cardDragPreviewPlacementCount++;
		RebuildCardCharacterPreview(force: true);
		RestoreCardCharacterPreviewPosition();
		long num = Math.Max(0L, GetEffectiveCost(_editingPacket));
		if (GodotObject.IsInstanceValid(_previewSunSpinBox) && num > 0)
		{
			bool updatingControls = _updatingControls;
			_updatingControls = true;
			try
			{
				_previewSunSpinBox.SetValueNoSignal(Math.Max(0.0, _previewSunSpinBox.Value - (double)num));
			}
			finally
			{
				_updatingControls = updatingControls;
			}
		}
		FinishCardDragVisuals();
		StartConfiguredCardCooldown();
		ShowCardPlacementMessage($"种植完成 · 第 {cell.X} 列 / 第 {cell.Y} 行 · 消耗 {num} 阳光", valid: true);
		QueueCardBattleDropSurfaceRedraw();
	}

	private void CancelCardDragSession(bool countCancellation, string message)
	{
		bool flag = _cardDragCandidate || _cardDragActive;
		if (countCancellation & flag)
		{
			_cardDragCancelCount++;
		}
		FinishCardDragVisuals();
		if (_cardRuntimePreviewState == 1)
		{
			_cardRuntimePreviewState = ResolveCardAvailabilityState();
			ApplyCardRuntimePreviewState();
		}
		if (!string.IsNullOrWhiteSpace(message) & flag)
		{
			ShowCardPlacementMessage(message, valid: false);
		}
	}

	private void FinishCardDragVisuals()
	{
		_cardDragActive = false;
		_cardDragTargetValid = false;
		_cardDragPreviewCell = new Vector2I(-1, -1);
		ClearCardPointerSession();
		if (GodotObject.IsInstanceValid(_cardDragPacketGhost))
		{
			_cardDragPacketGhost.Hide();
		}
		HideCardDragTarget();
		if (GodotObject.IsInstanceValid(_cardBattleTint))
		{
			_cardBattleTint.Color = new Color(0.04f, 0.12f, 0.035f, 0.12f);
		}
		RestoreCardCharacterPreviewPosition();
		QueueCardBattleDropSurfaceRedraw();
	}

	private void ResetCardDragPreviewState()
	{
		_cardHasPlacedPreview = false;
		_cardPlacedPreviewCell = new Vector2I(-1, -1);
		FinishCardDragVisuals();
		_cardDragPreviewPlacementCount = 0;
		_cardDragRejectedCount = 0;
		_cardDragCancelCount = 0;
	}

	private void ShowCardPlacementMessage(string message, bool valid)
	{
		if (GodotObject.IsInstanceValid(_battleStatusLabel))
		{
			_battleStatusLabel.Text = message;
			_battleStatusLabel.AddThemeColorOverride("font_color", valid ? new Color(0.72f, 1f, 0.58f) : new Color(1f, 0.64f, 0.48f));
		}
	}

	private void OnCardBattleDropSurfaceResized()
	{
		QueueCardBattleDropSurfaceRedraw();
		if (_cardDragActive)
		{
			UpdateCardDragTarget(_cardDragPointerGlobalPosition);
		}
	}

	private void DrawCardBattleDropSurface()
	{
		if (!GodotObject.IsInstanceValid(_cardBattleDropSurface))
		{
			return;
		}
		Rect2 cardBattleGridRect = GetCardBattleGridRect(_cardBattleDropSurface.Size);
		Vector2 cardBattleCellSize = GetCardBattleCellSize(_cardBattleDropSurface.Size);
		_cardBattleDropSurface.DrawRect(cardBattleGridRect, new Color(0.12f, 0.36f, 0.08f, 0.1f));
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 9; j++)
			{
				Rect2 rect = new Rect2(cardBattleGridRect.Position + new Vector2((float)j * cardBattleCellSize.X, (float)i * cardBattleCellSize.Y), cardBattleCellSize);
				Color color = (((j + i) % 2 == 0) ? new Color(0.5f, 0.84f, 0.28f, 0.055f) : new Color(0.2f, 0.58f, 0.14f, 0.035f));
				_cardBattleDropSurface.DrawRect(rect, color);
				_cardBattleDropSurface.DrawRect(rect, new Color(0.82f, 1f, 0.62f, 0.22f), filled: false, 1f);
			}
		}
		_cardBattleDropSurface.DrawRect(cardBattleGridRect, new Color(0.8f, 1f, 0.54f, 0.5f), filled: false, 2f);
		if (_cardHasPlacedPreview && IsCardBattleCell(_cardPlacedPreviewCell))
		{
			Rect2 rect2 = GetCardBattleCellRect(_cardBattleDropSurface.Size, _cardPlacedPreviewCell).Grow(-3f);
			_cardBattleDropSurface.DrawRect(rect2, new Color(1f, 0.72f, 0.22f, 0.18f));
			_cardBattleDropSurface.DrawRect(rect2, new Color(1f, 0.82f, 0.32f, 0.78f), filled: false, 2f);
		}
	}

	private void QueueCardBattleDropSurfaceRedraw()
	{
		if (GodotObject.IsInstanceValid(_cardBattleDropSurface))
		{
			_cardBattleDropSurface.QueueRedraw();
		}
	}

	private bool TryGetCardBattleCell(Vector2 localPosition, out Vector2I cell)
	{
		cell = new Vector2I(-1, -1);
		if (!GodotObject.IsInstanceValid(_cardBattleDropSurface))
		{
			return false;
		}
		Rect2 cardBattleGridRect = GetCardBattleGridRect(_cardBattleDropSurface.Size);
		if (!cardBattleGridRect.HasPoint(localPosition))
		{
			return false;
		}
		Vector2 cardBattleCellSize = GetCardBattleCellSize(_cardBattleDropSurface.Size);
		if (cardBattleCellSize.X <= 0f || cardBattleCellSize.Y <= 0f)
		{
			return false;
		}
		Vector2 vector = localPosition - cardBattleGridRect.Position;
		cell = new Vector2I(Math.Clamp(Mathf.FloorToInt(vector.X / cardBattleCellSize.X) + 1, 1, 9), Math.Clamp(Mathf.FloorToInt(vector.Y / cardBattleCellSize.Y) + 1, 1, 5));
		return true;
	}

	private static Rect2 GetCardBattleGridRect(Vector2 size)
	{
		return new Rect2(size * new Vector2(0.16f, 0.19f), size * new Vector2(0.72f, 0.65f));
	}

	private static Vector2 GetCardBattleCellSize(Vector2 size)
	{
		Rect2 cardBattleGridRect = GetCardBattleGridRect(size);
		return new Vector2(cardBattleGridRect.Size.X / 9f, cardBattleGridRect.Size.Y / 5f);
	}

	private static Rect2 GetCardBattleCellRect(Vector2 size, Vector2I cell)
	{
		Rect2 cardBattleGridRect = GetCardBattleGridRect(size);
		Vector2 cardBattleCellSize = GetCardBattleCellSize(size);
		return new Rect2(cardBattleGridRect.Position + new Vector2((float)(cell.X - 1) * cardBattleCellSize.X, (float)(cell.Y - 1) * cardBattleCellSize.Y), cardBattleCellSize);
	}

	private static Vector2 GetCardBattleCellCenter(Vector2 size, Vector2I cell)
	{
		Rect2 cardBattleCellRect = GetCardBattleCellRect(size, cell);
		return cardBattleCellRect.Position + new Vector2(cardBattleCellRect.Size.X * 0.5f, cardBattleCellRect.Size.Y * 0.72f);
	}

	private static bool IsCardBattleCell(Vector2I cell)
	{
		if (cell.X >= 1 && cell.X <= 9 && cell.Y >= 1)
		{
			return cell.Y <= 5;
		}
		return false;
	}

	private void PreviewCharacterAtCardCell(Vector2I cell, bool valid)
	{
		if (GodotObject.IsInstanceValid(_cardCharacterPreviewRoot) && GodotObject.IsInstanceValid(_cardCharacterViewport) && IsCardBattleCell(cell))
		{
			Vector2 size = new Vector2(_cardCharacterViewport.Size.X, _cardCharacterViewport.Size.Y);
			_cardCharacterPreviewRoot.Position = GetCardBattleCellCenter(size, cell);
			_cardCharacterPreviewRoot.Modulate = (valid ? new Color(0.74f, 1f, 0.72f, 0.86f) : new Color(1f, 0.42f, 0.38f, 0.68f));
		}
	}

	private void RestoreCardCharacterPreviewPosition()
	{
		if (GodotObject.IsInstanceValid(_cardCharacterPreviewRoot))
		{
			Vector2 size = (GodotObject.IsInstanceValid(_cardCharacterViewport) ? new Vector2(_cardCharacterViewport.Size.X, _cardCharacterViewport.Size.Y) : new Vector2(720f, 365f));
			if (_cardHasPlacedPreview && IsCardBattleCell(_cardPlacedPreviewCell))
			{
				_cardCharacterPreviewRoot.Position = GetCardBattleCellCenter(size, _cardPlacedPreviewCell);
			}
			else
			{
				_cardCharacterPreviewRoot.Position = new Vector2(size.X * 0.5f, size.Y * 0.6f);
			}
			_cardCharacterPreviewRoot.Modulate = Colors.White;
		}
	}

	private void UpdateCardBattleViewportLayout()
	{
		if (GodotObject.IsInstanceValid(_cardCharacterViewport))
		{
			Vector2 size = new Vector2(_cardCharacterViewport.Size.X, _cardCharacterViewport.Size.Y);
			if (size.X <= 1f || size.Y <= 1f)
			{
				size = new Vector2(720f, 365f);
			}
			if (GodotObject.IsInstanceValid(_cardGameBackground))
			{
				_cardGameBackground.Size = size;
			}
			if (GodotObject.IsInstanceValid(_cardBattleTint))
			{
				_cardBattleTint.Size = size;
			}
			RestoreCardCharacterPreviewPosition();
			QueueCardBattleDropSurfaceRedraw();
		}
	}

	private void PreviewCardPlantAndCooldown()
	{
		Vector2I cell = ((_cardHasPlacedPreview && IsCardBattleCell(_cardPlacedPreviewCell)) ? _cardPlacedPreviewCell : new Vector2I(5, 3));
		PlaceCardPreviewAtCell(cell);
	}

	private void ResetCardRuntimePreview()
	{
		CancelCardDragSession(countCancellation: false, "");
		StopCardRuntimeClock();
		_cardRuntimePreviewState = 0;
		_cardRuntimeCooldownProgress = 0f;
		_cardRuntimeCooldownDuration = 0.0;
		_cardRuntimeCooldownRemaining = 0.0;
		if (GodotObject.IsInstanceValid(_previewSunSpinBox))
		{
			_previewSunSpinBox.Value = Math.Max(500L, GetEffectiveCost(_editingPacket));
		}
		_cardHasPlacedPreview = false;
		_cardPlacedPreviewCell = new Vector2I(-1, -1);
		RebuildCardCharacterPreview(force: true);
		RestoreCardCharacterPreviewPosition();
		QueueCardBattleDropSurfaceRedraw();
		ApplyCardRuntimePreviewState();
	}

	private void StartConfiguredCardCooldown()
	{
		EnsureCardRuntimeCooldownDuration(forceRefresh: true);
		_cardRuntimeCooldownRemaining = _cardRuntimeCooldownDuration;
		_cardRuntimeCooldownProgress = ((_cardRuntimeCooldownDuration > 0.0) ? 1f : 0f);
		_cardRuntimePreviewState = ((_cardRuntimeCooldownDuration > 0.0) ? 2 : ResolveCardAvailabilityState());
		_cardRuntimeCooldownRunning = _cardRuntimeCooldownDuration > 0.0;
		RequestVisibilityGatedProcessing(_cardRuntimeCooldownRunning);
		ApplyCardRuntimePreviewState();
	}

	private void ToggleCardRuntimeClock()
	{
		if (_cardRuntimeCooldownRemaining <= 0.0)
		{
			StartConfiguredCardCooldown();
			return;
		}
		_cardRuntimeCooldownRunning = !_cardRuntimeCooldownRunning;
		RequestVisibilityGatedProcessing(_cardRuntimeCooldownRunning);
		ApplyCardRuntimePreviewState();
	}

	private void StopCardRuntimeClock()
	{
		_cardRuntimeCooldownRunning = false;
		RequestVisibilityGatedProcessing(requested: false);
	}

	private void EnsureCardRuntimeCooldownDuration(bool forceRefresh = false)
	{
		if (forceRefresh || !(_cardRuntimeCooldownDuration > 0.0))
		{
			double num = (GodotObject.IsInstanceValid(_cardPacketShowPreview) ? _cardPacketShowPreview.coldDown : 0.0);
			if (num <= 0.0 && double.TryParse(GetEffectiveCooldownText(_editingPacket), out var result))
			{
				num = result;
			}
			_cardRuntimeCooldownDuration = Math.Max(0.0, num);
		}
	}

	private int ResolveCardAvailabilityState()
	{
		long effectiveCost = GetEffectiveCost(_editingPacket);
		double num = _previewSunSpinBox?.Value ?? 0.0;
		TowerDefensePacketConfig editingPacket = _editingPacket;
		if (editingPacket != null && editingPacket.disableWhenSunNegative && num < 0.0)
		{
			return 3;
		}
		if (effectiveCost < 0 || !(num < (double)effectiveCost))
		{
			return 0;
		}
		return 3;
	}

	private void ToggleCardRuntimeSelection()
	{
		int cardRuntimePreviewState = _cardRuntimePreviewState;
		if ((uint)(cardRuntimePreviewState - 2) <= 2u)
		{
			ApplyCardRuntimePreviewState();
			return;
		}
		_cardRuntimePreviewState = ((_cardRuntimePreviewState != 1) ? 1 : 0);
		ApplyCardRuntimePreviewState();
	}

	private void OnCardRuntimeMouseEntered()
	{
		_cardRuntimeHovered = true;
		_cardPacketShowPreview?.OnMouseEntered();
		ApplyCardRuntimePreviewState();
	}

	private void OnCardRuntimeMouseExited()
	{
		_cardRuntimeHovered = false;
		_cardPacketShowPreview?.OnMouseExited();
		ApplyCardRuntimePreviewState();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		_behaviorRegistryPollAccumulator += Math.Max(0.0, delta);
		if (_behaviorRegistryPollAccumulator >= 0.25)
		{
			_behaviorRegistryPollAccumulator %= 0.25;
			ulong revision = TowerDefenseBehaviorRegistry.Revision;
			if (revision != _behaviorRegistryRevision)
			{
				_behaviorRegistryRevision = revision;
				BuildRegistryBehaviorPicker();
				RefreshCardBehaviorLists(_editingPacket);
			}
		}
		if (!_cardRuntimeCooldownRunning || _cardRuntimeCooldownRemaining <= 0.0)
		{
			return;
		}
		_cardRuntimeCooldownRemaining = Math.Max(0.0, _cardRuntimeCooldownRemaining - delta);
		_cardRuntimeCooldownProgress = ((_cardRuntimeCooldownDuration > 0.0) ? ((float)(_cardRuntimeCooldownRemaining / _cardRuntimeCooldownDuration)) : 0f);
		UpdateCardRuntimeCooldownVisual();
		if (_cardRuntimeCooldownRemaining <= 0.0)
		{
			StopCardRuntimeClock();
			_cardRuntimePreviewState = ResolveCardAvailabilityState();
			ApplyCardRuntimePreviewState();
			return;
		}
		_cardRuntimeReadoutAccumulator += Math.Max(0.0, delta);
		if (_cardRuntimeReadoutAccumulator >= 0.1)
		{
			_cardRuntimeReadoutAccumulator %= 0.1;
			RefreshCardRuntimeReadouts();
		}
	}

	private void ApplyCardRuntimePreviewState()
	{
		RefreshCardRuntimeReadouts();
		UpdateCardRuntimeCooldownVisual();
		if (GodotObject.IsInstanceValid(_cardPacketShowPreview))
		{
			_cardPacketShowPreview.select = _cardRuntimePreviewState == 1;
			_cardPacketShowPreview.@lock = _cardRuntimePreviewState == 4;
			_cardPacketShowPreview.openShadow = _cardRuntimePreviewState == 3;
			_cardPacketShowPreview.coldDownOpen = _cardRuntimePreviewState == 2;
			TowerDefenseInGamePacketShow cardPacketShowPreview = _cardPacketShowPreview;
			int cardRuntimePreviewState = _cardRuntimePreviewState;
			bool alive = (uint)cardRuntimePreviewState <= 1u;
			cardPacketShowPreview.alive = alive;
			_cardPacketShowPreview.ColorSet();
		}
	}

	private void RefreshCardRuntimeReadouts()
	{
		_cardRuntimeReadoutRefreshCount++;
		bool updatingControls = _updatingControls;
		_updatingControls = true;
		try
		{
			SelectVisualCard(_runtimeStateCards, _cardRuntimePreviewState.ToString());
			if (GodotObject.IsInstanceValid(_cooldownProgressSlider))
			{
				_cooldownProgressSlider.Value = _cardRuntimeCooldownProgress;
			}
			if (GodotObject.IsInstanceValid(_cooldownPercentLabel))
			{
				_cooldownPercentLabel.Text = $"{Mathf.RoundToInt(_cardRuntimeCooldownProgress * 100f)}%";
			}
			if (GodotObject.IsInstanceValid(_runtimeClockLabel))
			{
				_runtimeClockLabel.Text = ((_cardRuntimeCooldownDuration <= 0.0) ? "未配置冷却" : $"{_cardRuntimeCooldownRemaining:0.00} / {_cardRuntimeCooldownDuration:0.00} 秒");
			}
			if (GodotObject.IsInstanceValid(_pauseRuntimeButton))
			{
				_pauseRuntimeButton.Text = (_cardRuntimeCooldownRunning ? "暂停冷却" : "继续冷却");
			}
		}
		finally
		{
			_updatingControls = updatingControls;
		}
		if (GodotObject.IsInstanceValid(_battleStatusLabel))
		{
			_battleStatusLabel.AddThemeColorOverride("font_color", new Color(0.94f, 1f, 0.78f));
			Label battleStatusLabel = _battleStatusLabel;
			battleStatusLabel.Text = _cardRuntimePreviewState switch
			{
				1 => "卡片已拿起 · 再次点击可放回", 
				2 => $"种植完成 · {(_cardRuntimeCooldownRunning ? "实时冷却" : "冷却已暂停")} {_cardRuntimeCooldownRemaining:0.00} 秒", 
				3 => $"阳光不足 · 当前 {Mathf.RoundToInt((float)(_previewSunSpinBox?.Value ?? 0.0))} / 需要 {GetEffectiveCost(_editingPacket)}", 
				4 => "卡片尚未解锁", 
				_ => _cardRuntimeHovered ? "悬停预览 · 正在播放卡片动画" : "准备种植 · 点击卡片拿起", 
			};
		}
	}

	private void UpdateCardRuntimeCooldownVisual()
	{
		if (GodotObject.IsInstanceValid(_cardPacketShowPreview))
		{
			_cardPacketShowPreview.coldDown = _cardRuntimeCooldownDuration;
			_cardPacketShowPreview.coldDownTimer = _cardRuntimeCooldownRemaining;
			if (GodotObject.IsInstanceValid(_cardPacketShowPreview.coldDownProgressBar))
			{
				_cardPacketShowPreview.coldDownProgressBar.MaxValue = Math.Max(0.001, _cardRuntimeCooldownDuration);
				_cardPacketShowPreview.coldDownProgressBar.Value = _cardRuntimeCooldownRemaining;
				_cardPacketShowPreview.coldDownProgressBar.Visible = _cardRuntimePreviewState == 2;
			}
		}
	}

	private void RebuildCardCharacterPreview(bool force = false)
	{
		if (!GodotObject.IsInstanceValid(_cardCharacterPreviewRoot))
		{
			return;
		}
		TowerDefenseCharacterConfig towerDefenseCharacterConfig = _editingPacket?.characterConfig;
		string text = BuildCardArmorFingerprint(_editingPacket?.initArmor);
		if (!force && _cardCharacterPreviewInitialized && towerDefenseCharacterConfig == _cardCharacterPreviewConfig && string.Equals(text, _cardCharacterPreviewArmorFingerprint, StringComparison.Ordinal))
		{
			return;
		}
		DisposeCardCharacterPreview();
		_cardCharacterPreviewInitialized = true;
		_cardCharacterPreviewConfig = towerDefenseCharacterConfig;
		_cardCharacterPreviewArmorFingerprint = text;
		foreach (Node child in _cardCharacterPreviewRoot.GetChildren())
		{
			if (!child.IsQueuedForDeletion())
			{
				child.QueueFree();
			}
		}
		_cardCharacterPreview = null;
		_cardCharacterPreviewSafety = new XWGameplayLogicPreviewSafety();
		if (!GodotObject.IsInstanceValid(towerDefenseCharacterConfig) || !TryResolveCardCharacterScene(towerDefenseCharacterConfig, out var scene))
		{
			SetCardCharacterMissing(missing: true, GodotObject.IsInstanceValid(towerDefenseCharacterConfig) ? "未找到绑定角色的完整场景" : "请先为卡片绑定角色");
			return;
		}
		try
		{
			_cardCharacterPreviewBuildCount++;
			Node node = scene.Instantiate(PackedScene.GenEditState.Disabled);
			XWRuntimePreviewSceneAdapter.PrepareAnimationSprites(node);
			_cardCharacterPreview = FindCardRuntimeCharacter(node);
			if (!GodotObject.IsInstanceValid(_cardCharacterPreview))
			{
				node.Free();
				SetCardCharacterMissing(missing: true, "角色场景缺少 TowerDefenseCharacter");
				return;
			}
			_cardCharacterPreview.inGame = false;
			_cardCharacterPreview.editorPreviewMode = true;
			_cardCharacterPreview.config = towerDefenseCharacterConfig;
			if (_editingPacket.initArmor != null)
			{
				_cardCharacterPreview.currentArmor = new Array<string>(_editingPacket.initArmor);
			}
			_cardCharacterPreviewSafety.PrepareCharacter(node);
			_cardCharacterPreviewSafety.TrackPreviewRoot(node);
			_cardCharacterPreviewRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			SetCardCharacterMissing(missing: false, "");
		}
		catch (Exception ex)
		{
			_cardCharacterPreview = null;
			SetCardCharacterMissing(missing: true, "角色预览失败：" + ex.Message);
		}
	}

	private void DisposeCardCharacterPreview()
	{
		_cardCharacterPreviewSafety?.Dispose();
		_cardCharacterPreviewSafety = null;
		_cardCharacterPreview = null;
		_cardCharacterPreviewInitialized = false;
		_cardCharacterPreviewConfig = null;
		_cardCharacterPreviewArmorFingerprint = "";
	}

	private static string BuildCardArmorFingerprint(Array<string> armor)
	{
		if (armor == null || armor.Count == 0)
		{
			return "";
		}
		return string.Join('\u001f', armor);
	}

	private static bool TryResolveCardCharacterScene(TowerDefenseCharacterConfig characterConfig, out PackedScene scene)
	{
		scene = null;
		if (!GodotObject.IsInstanceValid(characterConfig))
		{
			return false;
		}
		if (!string.IsNullOrWhiteSpace(characterConfig.name) && ResourceManager.Instance != null)
		{
			scene = ResourceManager.Instance.GetCharacterScene(characterConfig.name);
			if (GodotObject.IsInstanceValid(scene))
			{
				return true;
			}
		}
		string baseDir = NormalizeResourcePath(characterConfig.ResourcePath).GetBaseDir();
		string text = (string.Equals(baseDir.GetFile(), "Config", StringComparison.OrdinalIgnoreCase) ? baseDir.GetBaseDir() : baseDir).PathJoin("Scene");
		if (!DirAccess.DirExistsAbsolute(text))
		{
			return false;
		}
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

	private static TowerDefenseCharacter FindCardRuntimeCharacter(Node node)
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
			TowerDefenseCharacter towerDefenseCharacter = FindCardRuntimeCharacter(child);
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				return towerDefenseCharacter;
			}
		}
		return null;
	}

	private void SetCardCharacterMissing(bool missing, string message)
	{
		if (GodotObject.IsInstanceValid(_cardCharacterMissingLabel))
		{
			_cardCharacterMissingLabel.Visible = missing;
			_cardCharacterMissingLabel.Text = message;
		}
	}

	private void SetPacketShowPreviewMissing(bool missing)
	{
		if (GodotObject.IsInstanceValid(_cardImageMissingLabel))
		{
			_cardImageMissingLabel.Visible = missing;
		}
	}

	private static string GetEffectiveCostText(TowerDefensePacketConfig packet)
	{
		long effectiveCost = GetEffectiveCost(packet);
		if (effectiveCost >= 0)
		{
			return effectiveCost.ToString();
		}
		return "-";
	}

	private static long GetEffectiveCost(TowerDefensePacketConfig packet)
	{
		XWCardPreviewValueSource source;
		return XWCardBehaviorPreviewSafety.ResolveBaseOverrideCost(packet, out source);
	}

	private static string GetEffectiveCooldownText(TowerDefensePacketConfig packet)
	{
		double num = XWCardBehaviorPreviewSafety.ResolveBaseOverrideCooldown(packet, out var _);
		if (!(num < 0.0))
		{
			return num.ToString("0.###");
		}
		return "-";
	}

	private static string BuildCardValueOverrideSummary(TowerDefensePacketConfig packet)
	{
		if (packet == null)
		{
			return "数值覆盖：未加载";
		}
		XWCardBehaviorPreviewSnapshot xWCardBehaviorPreviewSnapshot = XWCardBehaviorPreviewSafety.Capture(packet);
		string previewValueSourceLabel = GetPreviewValueSourceLabel(xWCardBehaviorPreviewSnapshot.CostSource);
		string previewValueSourceLabel2 = GetPreviewValueSourceLabel(xWCardBehaviorPreviewSnapshot.CooldownSource);
		string previewValueSourceLabel3 = GetPreviewValueSourceLabel(xWCardBehaviorPreviewSnapshot.StartingCooldownSource);
		string value = ((xWCardBehaviorPreviewSnapshot.StartingCooldown < 0.0) ? "-" : xWCardBehaviorPreviewSnapshot.StartingCooldown.ToString("0.###"));
		return $"数值覆盖 · 费用 {previewValueSourceLabel} {GetEffectiveCostText(packet)} · 冷却 {previewValueSourceLabel2} {GetEffectiveCooldownText(packet)} 秒 · 初始 {previewValueSourceLabel3} {value} 秒";
	}

	private static string GetPreviewValueSourceLabel(XWCardPreviewValueSource source)
	{
		return source switch
		{
			XWCardPreviewValueSource.CharacterBase => "继承角色", 
			XWCardPreviewValueSource.PacketOverride => "卡牌覆盖", 
			XWCardPreviewValueSource.OverrideResource => "规则覆盖", 
			_ => "未配置", 
		};
	}

	private static bool TryGetInt(Resource resource, string propertyName, out int value)
	{
		value = 0;
		if (!GodotObject.IsInstanceValid(resource))
		{
			return false;
		}
		try
		{
			Variant variant = resource.Get(propertyName);
			if (variant.VariantType == Variant.Type.Int)
			{
				value = variant.AsInt32();
				return true;
			}
		}
		catch
		{
		}
		return false;
	}

	private static bool TryGetDouble(Resource resource, string propertyName, out double value)
	{
		value = 0.0;
		if (!GodotObject.IsInstanceValid(resource))
		{
			return false;
		}
		try
		{
			Variant variant = resource.Get(propertyName);
			if (variant.VariantType == Variant.Type.Float || variant.VariantType == Variant.Type.Int)
			{
				value = variant.AsDouble();
				return true;
			}
		}
		catch
		{
		}
		return false;
	}

	private static Color GetPacketTypeColor(TowerDefenseEnum.PACKET_TYPE type)
	{
		return type switch
		{
			TowerDefenseEnum.PACKET_TYPE.GOLD => new Color(0.82f, 0.62f, 0.2f), 
			TowerDefenseEnum.PACKET_TYPE.DIAMOND => new Color(0.25f, 0.6f, 0.86f), 
			TowerDefenseEnum.PACKET_TYPE.COLOUR => new Color(0.55f, 0.4f, 0.92f), 
			TowerDefenseEnum.PACKET_TYPE.STAR => new Color(0.82f, 0.46f, 0.18f), 
			TowerDefenseEnum.PACKET_TYPE.ORIGINAL => new Color(0.3f, 0.5f, 0.3f), 
			TowerDefenseEnum.PACKET_TYPE.ZOMBIE => new Color(0.4f, 0.42f, 0.46f), 
			TowerDefenseEnum.PACKET_TYPE.COVER => new Color(0.3f, 0.3f, 0.36f), 
			TowerDefenseEnum.PACKET_TYPE.GRAY => new Color(0.28f, 0.28f, 0.28f), 
			_ => new Color(0.16f, 0.18f, 0.2f), 
		};
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
			TowerDefenseEnum.PACKET_TYPE.NOONE => "无卡面", 
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

	private static string FormatCharacterBinding(TowerDefenseCharacterConfig character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return "未配置";
		}
		return GetCharacterSourceTag(ResolveCharacterSource(character.ResourcePath)) + " " + FormatResource(character);
	}

	private static string FormatVariant(Variant value)
	{
		switch (value.VariantType)
		{
		case Variant.Type.Nil:
			return "空";
		case Variant.Type.Object:
			return (value.AsGodotObject() is Resource resource) ? FormatResource(resource) : (value.AsGodotObject()?.GetType().Name ?? "空");
		case Variant.Type.String:
		case Variant.Type.StringName:
			return EmptyToPlaceholder(value.AsString());
		default:
			return value.ToString();
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
		return new List<MethodInfo>(163)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenderCardPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindCardPreviewSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeCardRuntimePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindCardInspectorSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindUnlockAndArmorEditors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedUnlockIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReplaceSelectedUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceCardUnlockConditions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceSelectedArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelectedArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceCardArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshArmorList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindCardBehaviorSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowRegistryBehaviorPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildRegistryBehaviorPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddRegistryBehaviorFromPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddCustomBehaviorId, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddRegistryBehaviorId, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelectedRegistryBehavior, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedRegistryBehavior, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceRegistryBehaviorIds, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedRegistryBehaviorIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenSelectedRegistryBehavior, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowInlineBehaviorSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedInlineBehavior, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedInlineBehavior, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceInlineBehaviors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedInlineBehaviorIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenSelectedInlineBehavior, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenInlineBehavior, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "behavior", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshBehaviorRegistryFromUser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCardBehaviorLists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindCardEventSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EditSelectedUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowPacketEventSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "targetProperty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsurePacketEventSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddPacketEventToTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "packetEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelectedPacketEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveSelectedPacketEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplacePacketEvents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EditSelectedPacketEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenPacketEventResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "packetEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketEventArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedPacketEventIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCardPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCardEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncCardVector2HistoryControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncCardVector2HistoryControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "xControlName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "yControlName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveCardResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCardPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreatePacketOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenPacketOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearPacketOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPacketOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "value", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePacketOverrideEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCardLists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCardLists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildRuntimeStateCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCardRuntimePreviewState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPacketTypeCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false),
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSpawnMethodCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearVisualCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateVisualChoiceCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "detail", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "minimumSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectVisualCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "selectedKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindCardVector2Field, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "x", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "y", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCharacterBindingSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "current", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowCharacterSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureCharacterResourcePicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearCharacterBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCharacterBindingResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadCharacterConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsCharacterConfigResourcePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCharacterSourceLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "current", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterSourceTag, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveCharacterSource, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsBuiltInCharacterPath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCharacterDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePanelStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "borderWidth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "cornerRadius", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPacketShowPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigurePacketShowPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetShow", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "forceSpriteRebuild", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "trackMainPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPacketShowPreviewIncremental, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetShow", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "animationChanged", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPacketShowAnimationInPlace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetShow", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPacketShowSpriteFingerprint, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPacketShowSaveKeyFingerprint, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveConfiguredPacketCooldown, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPacketShowPreviewLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetShow", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsurePacketPreviewCharacterSpriteRegistered, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsAdobeAnimateSpriteScene, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.PositionPacketShowPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCardInteractionGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanvasPointToControlLocal, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "canvasPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsCanvasPointInsideControl, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "canvasPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginCardPointerSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "touch", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "touchIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HandleCardPointerMotion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "touch", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "touchIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartCardDragSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteCardPointerSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "touch", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "touchIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearCardPointerSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureCardDragGhost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeCardDragGhost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCardDragTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowCardDragTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "valid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HideCardDragTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlaceCardPreviewAtCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelCardDragSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "countCancellation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishCardDragVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetCardDragPreviewState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowCardPlacementMessage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "valid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCardBattleDropSurfaceResized, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawCardBattleDropSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueCardBattleDropSurfaceRedraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCardBattleGridRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCardBattleCellSize, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCardBattleCellRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCardBattleCellCenter, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsCardBattleCell, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PreviewCharacterAtCardCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "valid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreCardCharacterPreviewPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCardBattleViewportLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewCardPlantAndCooldown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetCardRuntimePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartConfiguredCardCooldown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleCardRuntimeClock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopCardRuntimeClock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureCardRuntimeCooldownDuration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "forceRefresh", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveCardAvailabilityState, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleCardRuntimeSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCardRuntimeMouseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCardRuntimeMouseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCardRuntimePreviewState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCardRuntimeReadouts, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCardRuntimeCooldownVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildCardCharacterPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "force", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeCardCharacterPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildCardArmorFingerprint, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "armor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindCardRuntimeCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetCardCharacterMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "missing", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPacketShowPreviewMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "missing", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetEffectiveCostText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetEffectiveCost, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetEffectiveCooldownText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCardValueOverrideSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetPreviewValueSourceLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketTypeColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketTexturePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketTypeDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatResource, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatCharacterBinding, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVariant, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddItemIfMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
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
		if (method == MethodName.RenderCardPreview && args.Count == 1)
		{
			RenderCardPreview(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCardPreviewSurface && args.Count == 2)
		{
			BindCardPreviewSurface(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeCardRuntimePreview && args.Count == 0)
		{
			InitializeCardRuntimePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.BindCardInspectorSurface && args.Count == 2)
		{
			BindCardInspectorSurface(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindUnlockAndArmorEditors && args.Count == 2)
		{
			BindUnlockAndArmorEditors(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedUnlockIndex && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetSelectedUnlockIndex());
			return true;
		}
		if (method == MethodName.AddUnlockCondition && args.Count == 0)
		{
			AddUnlockCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceSelectedUnlockCondition && args.Count == 0)
		{
			ReplaceSelectedUnlockCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedUnlockCondition && args.Count == 0)
		{
			RemoveSelectedUnlockCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedUnlockCondition && args.Count == 1)
		{
			MoveSelectedUnlockCondition(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceCardUnlockConditions && args.Count == 2)
		{
			ReplaceCardUnlockConditions(VariantUtils.ConvertToArray<UnlockConditionBaseConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddArmor && args.Count == 1)
		{
			AddArmor(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceSelectedArmor && args.Count == 1)
		{
			ReplaceSelectedArmor(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedArmor && args.Count == 0)
		{
			RemoveSelectedArmor();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedArmor && args.Count == 1)
		{
			MoveSelectedArmor(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceCardArmor && args.Count == 2)
		{
			ReplaceCardArmor(VariantUtils.ConvertToArray<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshArmorList && args.Count == 1)
		{
			RefreshArmorList(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCardBehaviorSurface && args.Count == 2)
		{
			BindCardBehaviorSurface(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowRegistryBehaviorPicker && args.Count == 0)
		{
			ShowRegistryBehaviorPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRegistryBehaviorPicker && args.Count == 0)
		{
			BuildRegistryBehaviorPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.AddRegistryBehaviorFromPicker && args.Count == 1)
		{
			AddRegistryBehaviorFromPicker(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddCustomBehaviorId && args.Count == 0)
		{
			AddCustomBehaviorId();
			ret = default;
			return true;
		}
		if (method == MethodName.AddRegistryBehaviorId && args.Count == 2)
		{
			AddRegistryBehaviorId(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedRegistryBehavior && args.Count == 0)
		{
			RemoveSelectedRegistryBehavior();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedRegistryBehavior && args.Count == 1)
		{
			MoveSelectedRegistryBehavior(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceRegistryBehaviorIds && args.Count == 2)
		{
			ReplaceRegistryBehaviorIds(VariantUtils.ConvertToArray<StringName>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedRegistryBehaviorIndex && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetSelectedRegistryBehaviorIndex());
			return true;
		}
		if (method == MethodName.OpenSelectedRegistryBehavior && args.Count == 0)
		{
			OpenSelectedRegistryBehavior();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowInlineBehaviorSelector && args.Count == 0)
		{
			ShowInlineBehaviorSelector();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedInlineBehavior && args.Count == 0)
		{
			RemoveSelectedInlineBehavior();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedInlineBehavior && args.Count == 1)
		{
			MoveSelectedInlineBehavior(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceInlineBehaviors && args.Count == 2)
		{
			ReplaceInlineBehaviors(VariantUtils.ConvertToArray<CardBehaviorDefinition>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedInlineBehaviorIndex && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetSelectedInlineBehaviorIndex());
			return true;
		}
		if (method == MethodName.OpenSelectedInlineBehavior && args.Count == 0)
		{
			OpenSelectedInlineBehavior();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenInlineBehavior && args.Count == 2)
		{
			OpenInlineBehavior(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<CardBehaviorDefinition>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshBehaviorRegistryFromUser && args.Count == 0)
		{
			RefreshBehaviorRegistryFromUser();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCardBehaviorLists && args.Count == 1)
		{
			RefreshCardBehaviorLists(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCardEventSurface && args.Count == 2)
		{
			BindCardEventSurface(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EditSelectedUnlockCondition && args.Count == 1)
		{
			EditSelectedUnlockCondition(VariantUtils.ConvertTo<long>(in args[0]));
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
		if (method == MethodName.AddPacketEventToTarget && args.Count == 2)
		{
			AddPacketEventToTarget(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<CardActionBehaviorDefinition>(in args[1]));
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
		if (method == MethodName.ReplacePacketEvents && args.Count == 3)
		{
			ReplacePacketEvents(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertToArray<CardActionBehaviorDefinition>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EditSelectedPacketEvent && args.Count == 2)
		{
			EditSelectedPacketEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenPacketEventResource && args.Count == 3)
		{
			OpenPacketEventResource(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<CardActionBehaviorDefinition>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPacketEventArray && args.Count == 1)
		{
			Array<CardActionBehaviorDefinition> packetEventArray = GetPacketEventArray(VariantUtils.ConvertTo<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(packetEventArray);
			return true;
		}
		if (method == MethodName.GetSelectedPacketEventIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetSelectedPacketEventIndex(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnCardPropertyEdited && args.Count == 1)
		{
			OnCardPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCardEditorFromHistory && args.Count == 0)
		{
			RefreshCardEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncCardVector2HistoryControls && args.Count == 1)
		{
			SyncCardVector2HistoryControls(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncCardVector2HistoryControl && args.Count == 4)
		{
			SyncCardVector2HistoryControl(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveCardResource && args.Count == 1)
		{
			SaveCardResource(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCardPreview && args.Count == 0)
		{
			UpdateCardPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePacketOverride && args.Count == 0)
		{
			CreatePacketOverride();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenPacketOverride && args.Count == 0)
		{
			OpenPacketOverride();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPacketOverride && args.Count == 0)
		{
			ClearPacketOverride();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPacketOverride && args.Count == 2)
		{
			SetPacketOverride(VariantUtils.ConvertTo<TowerDefensePacketOverride>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePacketOverrideEntry && args.Count == 0)
		{
			UpdatePacketOverrideEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCardLists && args.Count == 0)
		{
			RefreshCardLists();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCardLists && args.Count == 1)
		{
			RefreshCardLists(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 1)
		{
			AddSummaryRows(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRuntimeStateCards && args.Count == 0)
		{
			BuildRuntimeStateCards();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCardRuntimePreviewState && args.Count == 1)
		{
			SetCardRuntimePreviewState(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildPacketTypeCards && args.Count == 2)
		{
			BuildPacketTypeCards(VariantUtils.ConvertTo<HFlowContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSpawnMethodCards && args.Count == 2)
		{
			BuildSpawnMethodCards(VariantUtils.ConvertTo<HFlowContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearVisualCards && args.Count == 1)
		{
			ClearVisualCards(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateVisualChoiceCard && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<XWGameVisualChoiceCard>(CreateVisualChoiceCard(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<Vector2>(in args[5])));
			return true;
		}
		if (method == MethodName.SelectVisualCard && args.Count == 2)
		{
			SelectVisualCard(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCardVector2Field && args.Count == 4)
		{
			BindCardVector2Field(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[2]), VariantUtils.ConvertTo<StringName>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCharacterBindingSelector && args.Count == 1)
		{
			RefreshCharacterBindingSelector(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCharacterSelectorWindow && args.Count == 0)
		{
			ShowCharacterSelectorWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCharacterResourcePicker && args.Count == 0)
		{
			EnsureCharacterResourcePicker();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearCharacterBinding && args.Count == 0)
		{
			ClearCharacterBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCharacterBindingResource && args.Count == 1)
		{
			SetCharacterBindingResource(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadCharacterConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacterConfig>(LoadCharacterConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCharacterConfigResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCharacterConfigResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateCharacterSourceLabel && args.Count == 1)
		{
			UpdateCharacterSourceLabel(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCharacterSourceTag && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterSourceTag(VariantUtils.ConvertTo<CardCharacterSource>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveCharacterSource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CardCharacterSource>(ResolveCharacterSource(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsBuiltInCharacterPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBuiltInCharacterPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterKey(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildCharacterDisplayName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCharacterDisplayName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreatePanelStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreatePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.RefreshPacketShowPreview && args.Count == 1)
		{
			RefreshPacketShowPreview(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigurePacketShowPreview && args.Count == 4)
		{
			ConfigurePacketShowPreview(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPacketShowPreviewIncremental && args.Count == 3)
		{
			ApplyPacketShowPreviewIncremental(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketShowAnimationInPlace && args.Count == 2)
		{
			RefreshPacketShowAnimationInPlace(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildPacketShowSpriteFingerprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPacketShowSpriteFingerprint(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPacketShowSaveKeyFingerprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPacketShowSaveKeyFingerprint(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveConfiguredPacketCooldown && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ResolveConfiguredPacketCooldown(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyPacketShowPreviewLayout && args.Count == 1)
		{
			ApplyPacketShowPreviewLayout(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsurePacketPreviewCharacterSpriteRegistered && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsurePacketPreviewCharacterSpriteRegistered(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAdobeAnimateSpriteScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAdobeAnimateSpriteScene(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.PositionPacketShowPreview && args.Count == 0)
		{
			PositionPacketShowPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCardInteractionGuiInput && args.Count == 1)
		{
			OnCardInteractionGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanvasPointToControlLocal && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(CanvasPointToControlLocal(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.IsCanvasPointInsideControl && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCanvasPointInsideControl(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.BeginCardPointerSession && args.Count == 3)
		{
			BeginCardPointerSession(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleCardPointerMotion && args.Count == 3)
		{
			HandleCardPointerMotion(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartCardDragSession && args.Count == 0)
		{
			StartCardDragSession();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteCardPointerSession && args.Count == 3)
		{
			CompleteCardPointerSession(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearCardPointerSession && args.Count == 0)
		{
			ClearCardPointerSession();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCardDragGhost && args.Count == 0)
		{
			EnsureCardDragGhost();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeCardDragGhost && args.Count == 0)
		{
			DisposeCardDragGhost();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCardDragTarget && args.Count == 1)
		{
			UpdateCardDragTarget(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCardDragTarget && args.Count == 2)
		{
			ShowCardDragTarget(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HideCardDragTarget && args.Count == 0)
		{
			HideCardDragTarget();
			ret = default;
			return true;
		}
		if (method == MethodName.PlaceCardPreviewAtCell && args.Count == 1)
		{
			PlaceCardPreviewAtCell(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CancelCardDragSession && args.Count == 2)
		{
			CancelCardDragSession(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishCardDragVisuals && args.Count == 0)
		{
			FinishCardDragVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCardDragPreviewState && args.Count == 0)
		{
			ResetCardDragPreviewState();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCardPlacementMessage && args.Count == 2)
		{
			ShowCardPlacementMessage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCardBattleDropSurfaceResized && args.Count == 0)
		{
			OnCardBattleDropSurfaceResized();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawCardBattleDropSurface && args.Count == 0)
		{
			DrawCardBattleDropSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueCardBattleDropSurfaceRedraw && args.Count == 0)
		{
			QueueCardBattleDropSurfaceRedraw();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCardBattleGridRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetCardBattleGridRect(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCardBattleCellSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCardBattleCellSize(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCardBattleCellRect && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetCardBattleCellRect(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCardBattleCellCenter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCardBattleCellCenter(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.IsCardBattleCell && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCardBattleCell(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.PreviewCharacterAtCardCell && args.Count == 2)
		{
			PreviewCharacterAtCardCell(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreCardCharacterPreviewPosition && args.Count == 0)
		{
			RestoreCardCharacterPreviewPosition();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCardBattleViewportLayout && args.Count == 0)
		{
			UpdateCardBattleViewportLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewCardPlantAndCooldown && args.Count == 0)
		{
			PreviewCardPlantAndCooldown();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCardRuntimePreview && args.Count == 0)
		{
			ResetCardRuntimePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.StartConfiguredCardCooldown && args.Count == 0)
		{
			StartConfiguredCardCooldown();
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleCardRuntimeClock && args.Count == 0)
		{
			ToggleCardRuntimeClock();
			ret = default;
			return true;
		}
		if (method == MethodName.StopCardRuntimeClock && args.Count == 0)
		{
			StopCardRuntimeClock();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCardRuntimeCooldownDuration && args.Count == 1)
		{
			EnsureCardRuntimeCooldownDuration(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveCardAvailabilityState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveCardAvailabilityState());
			return true;
		}
		if (method == MethodName.ToggleCardRuntimeSelection && args.Count == 0)
		{
			ToggleCardRuntimeSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCardRuntimeMouseEntered && args.Count == 0)
		{
			OnCardRuntimeMouseEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCardRuntimeMouseExited && args.Count == 0)
		{
			OnCardRuntimeMouseExited();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCardRuntimePreviewState && args.Count == 0)
		{
			ApplyCardRuntimePreviewState();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCardRuntimeReadouts && args.Count == 0)
		{
			RefreshCardRuntimeReadouts();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCardRuntimeCooldownVisual && args.Count == 0)
		{
			UpdateCardRuntimeCooldownVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildCardCharacterPreview && args.Count == 1)
		{
			RebuildCardCharacterPreview(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeCardCharacterPreview && args.Count == 0)
		{
			DisposeCardCharacterPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildCardArmorFingerprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCardArmorFingerprint(VariantUtils.ConvertToArray<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindCardRuntimeCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindCardRuntimeCharacter(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SetCardCharacterMissing && args.Count == 2)
		{
			SetCardCharacterMissing(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPacketShowPreviewMissing && args.Count == 1)
		{
			SetPacketShowPreviewMissing(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetEffectiveCostText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetEffectiveCostText(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetEffectiveCost && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<long>(GetEffectiveCost(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetEffectiveCooldownText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetEffectiveCooldownText(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCardValueOverrideSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCardValueOverrideSummary(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPreviewValueSourceLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPreviewValueSourceLabel(VariantUtils.ConvertTo<XWCardPreviewValueSource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketTypeColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetPacketTypeColor(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
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
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatCharacterBinding && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatCharacterBinding(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
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
		if (method == MethodName.ClearVisualCards && args.Count == 1)
		{
			ClearVisualCards(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectVisualCard && args.Count == 2)
		{
			SelectVisualCard(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadCharacterConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacterConfig>(LoadCharacterConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCharacterConfigResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCharacterConfigResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterSourceTag && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterSourceTag(VariantUtils.ConvertTo<CardCharacterSource>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveCharacterSource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CardCharacterSource>(ResolveCharacterSource(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsBuiltInCharacterPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBuiltInCharacterPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterKey(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildCharacterDisplayName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCharacterDisplayName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreatePanelStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreatePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.BuildPacketShowSpriteFingerprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPacketShowSpriteFingerprint(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPacketShowSaveKeyFingerprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPacketShowSaveKeyFingerprint(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveConfiguredPacketCooldown && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ResolveConfiguredPacketCooldown(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.EnsurePacketPreviewCharacterSpriteRegistered && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsurePacketPreviewCharacterSpriteRegistered(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAdobeAnimateSpriteScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAdobeAnimateSpriteScene(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.CanvasPointToControlLocal && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(CanvasPointToControlLocal(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.IsCanvasPointInsideControl && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCanvasPointInsideControl(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCardBattleGridRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetCardBattleGridRect(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCardBattleCellSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCardBattleCellSize(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCardBattleCellRect && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetCardBattleCellRect(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCardBattleCellCenter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCardBattleCellCenter(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.IsCardBattleCell && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCardBattleCell(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCardArmorFingerprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCardArmorFingerprint(VariantUtils.ConvertToArray<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindCardRuntimeCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindCardRuntimeCharacter(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetEffectiveCostText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetEffectiveCostText(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetEffectiveCost && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<long>(GetEffectiveCost(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetEffectiveCooldownText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetEffectiveCooldownText(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCardValueOverrideSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCardValueOverrideSummary(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPreviewValueSourceLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPreviewValueSourceLabel(VariantUtils.ConvertTo<XWCardPreviewValueSource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketTypeColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetPacketTypeColor(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
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
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatCharacterBinding && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatCharacterBinding(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
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
		if (method == MethodName.RenderCardPreview)
		{
			return true;
		}
		if (method == MethodName.BindCardPreviewSurface)
		{
			return true;
		}
		if (method == MethodName.InitializeCardRuntimePreview)
		{
			return true;
		}
		if (method == MethodName.BindCardInspectorSurface)
		{
			return true;
		}
		if (method == MethodName.BindUnlockAndArmorEditors)
		{
			return true;
		}
		if (method == MethodName.GetSelectedUnlockIndex)
		{
			return true;
		}
		if (method == MethodName.AddUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.ReplaceSelectedUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.ReplaceCardUnlockConditions)
		{
			return true;
		}
		if (method == MethodName.AddArmor)
		{
			return true;
		}
		if (method == MethodName.ReplaceSelectedArmor)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedArmor)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedArmor)
		{
			return true;
		}
		if (method == MethodName.ReplaceCardArmor)
		{
			return true;
		}
		if (method == MethodName.RefreshArmorList)
		{
			return true;
		}
		if (method == MethodName.BindCardBehaviorSurface)
		{
			return true;
		}
		if (method == MethodName.ShowRegistryBehaviorPicker)
		{
			return true;
		}
		if (method == MethodName.BuildRegistryBehaviorPicker)
		{
			return true;
		}
		if (method == MethodName.AddRegistryBehaviorFromPicker)
		{
			return true;
		}
		if (method == MethodName.AddCustomBehaviorId)
		{
			return true;
		}
		if (method == MethodName.AddRegistryBehaviorId)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedRegistryBehavior)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedRegistryBehavior)
		{
			return true;
		}
		if (method == MethodName.ReplaceRegistryBehaviorIds)
		{
			return true;
		}
		if (method == MethodName.GetSelectedRegistryBehaviorIndex)
		{
			return true;
		}
		if (method == MethodName.OpenSelectedRegistryBehavior)
		{
			return true;
		}
		if (method == MethodName.ShowInlineBehaviorSelector)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedInlineBehavior)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedInlineBehavior)
		{
			return true;
		}
		if (method == MethodName.ReplaceInlineBehaviors)
		{
			return true;
		}
		if (method == MethodName.GetSelectedInlineBehaviorIndex)
		{
			return true;
		}
		if (method == MethodName.OpenSelectedInlineBehavior)
		{
			return true;
		}
		if (method == MethodName.OpenInlineBehavior)
		{
			return true;
		}
		if (method == MethodName.RefreshBehaviorRegistryFromUser)
		{
			return true;
		}
		if (method == MethodName.RefreshCardBehaviorLists)
		{
			return true;
		}
		if (method == MethodName.BindCardEventSurface)
		{
			return true;
		}
		if (method == MethodName.EditSelectedUnlockCondition)
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
		if (method == MethodName.AddPacketEventToTarget)
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
		if (method == MethodName.ReplacePacketEvents)
		{
			return true;
		}
		if (method == MethodName.EditSelectedPacketEvent)
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
		if (method == MethodName.GetSelectedPacketEventIndex)
		{
			return true;
		}
		if (method == MethodName.OnCardPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshCardEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.SyncCardVector2HistoryControls)
		{
			return true;
		}
		if (method == MethodName.SyncCardVector2HistoryControl)
		{
			return true;
		}
		if (method == MethodName.SaveCardResource)
		{
			return true;
		}
		if (method == MethodName.UpdateCardPreview)
		{
			return true;
		}
		if (method == MethodName.CreatePacketOverride)
		{
			return true;
		}
		if (method == MethodName.OpenPacketOverride)
		{
			return true;
		}
		if (method == MethodName.ClearPacketOverride)
		{
			return true;
		}
		if (method == MethodName.SetPacketOverride)
		{
			return true;
		}
		if (method == MethodName.UpdatePacketOverrideEntry)
		{
			return true;
		}
		if (method == MethodName.RefreshCardLists)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.BuildRuntimeStateCards)
		{
			return true;
		}
		if (method == MethodName.SetCardRuntimePreviewState)
		{
			return true;
		}
		if (method == MethodName.BuildPacketTypeCards)
		{
			return true;
		}
		if (method == MethodName.BuildSpawnMethodCards)
		{
			return true;
		}
		if (method == MethodName.ClearVisualCards)
		{
			return true;
		}
		if (method == MethodName.CreateVisualChoiceCard)
		{
			return true;
		}
		if (method == MethodName.SelectVisualCard)
		{
			return true;
		}
		if (method == MethodName.BindCardVector2Field)
		{
			return true;
		}
		if (method == MethodName.RefreshCharacterBindingSelector)
		{
			return true;
		}
		if (method == MethodName.ShowCharacterSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.EnsureCharacterResourcePicker)
		{
			return true;
		}
		if (method == MethodName.ClearCharacterBinding)
		{
			return true;
		}
		if (method == MethodName.SetCharacterBindingResource)
		{
			return true;
		}
		if (method == MethodName.LoadCharacterConfig)
		{
			return true;
		}
		if (method == MethodName.IsCharacterConfigResourcePath)
		{
			return true;
		}
		if (method == MethodName.UpdateCharacterSourceLabel)
		{
			return true;
		}
		if (method == MethodName.GetCharacterSourceTag)
		{
			return true;
		}
		if (method == MethodName.ResolveCharacterSource)
		{
			return true;
		}
		if (method == MethodName.IsBuiltInCharacterPath)
		{
			return true;
		}
		if (method == MethodName.NormalizeResourcePath)
		{
			return true;
		}
		if (method == MethodName.GetCharacterKey)
		{
			return true;
		}
		if (method == MethodName.BuildCharacterDisplayName)
		{
			return true;
		}
		if (method == MethodName.CreatePanelStyle)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketShowPreview)
		{
			return true;
		}
		if (method == MethodName.ConfigurePacketShowPreview)
		{
			return true;
		}
		if (method == MethodName.ApplyPacketShowPreviewIncremental)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketShowAnimationInPlace)
		{
			return true;
		}
		if (method == MethodName.BuildPacketShowSpriteFingerprint)
		{
			return true;
		}
		if (method == MethodName.BuildPacketShowSaveKeyFingerprint)
		{
			return true;
		}
		if (method == MethodName.ResolveConfiguredPacketCooldown)
		{
			return true;
		}
		if (method == MethodName.ApplyPacketShowPreviewLayout)
		{
			return true;
		}
		if (method == MethodName.EnsurePacketPreviewCharacterSpriteRegistered)
		{
			return true;
		}
		if (method == MethodName.IsAdobeAnimateSpriteScene)
		{
			return true;
		}
		if (method == MethodName.PositionPacketShowPreview)
		{
			return true;
		}
		if (method == MethodName.OnCardInteractionGuiInput)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.CanvasPointToControlLocal)
		{
			return true;
		}
		if (method == MethodName.IsCanvasPointInsideControl)
		{
			return true;
		}
		if (method == MethodName.BeginCardPointerSession)
		{
			return true;
		}
		if (method == MethodName.HandleCardPointerMotion)
		{
			return true;
		}
		if (method == MethodName.StartCardDragSession)
		{
			return true;
		}
		if (method == MethodName.CompleteCardPointerSession)
		{
			return true;
		}
		if (method == MethodName.ClearCardPointerSession)
		{
			return true;
		}
		if (method == MethodName.EnsureCardDragGhost)
		{
			return true;
		}
		if (method == MethodName.DisposeCardDragGhost)
		{
			return true;
		}
		if (method == MethodName.UpdateCardDragTarget)
		{
			return true;
		}
		if (method == MethodName.ShowCardDragTarget)
		{
			return true;
		}
		if (method == MethodName.HideCardDragTarget)
		{
			return true;
		}
		if (method == MethodName.PlaceCardPreviewAtCell)
		{
			return true;
		}
		if (method == MethodName.CancelCardDragSession)
		{
			return true;
		}
		if (method == MethodName.FinishCardDragVisuals)
		{
			return true;
		}
		if (method == MethodName.ResetCardDragPreviewState)
		{
			return true;
		}
		if (method == MethodName.ShowCardPlacementMessage)
		{
			return true;
		}
		if (method == MethodName.OnCardBattleDropSurfaceResized)
		{
			return true;
		}
		if (method == MethodName.DrawCardBattleDropSurface)
		{
			return true;
		}
		if (method == MethodName.QueueCardBattleDropSurfaceRedraw)
		{
			return true;
		}
		if (method == MethodName.GetCardBattleGridRect)
		{
			return true;
		}
		if (method == MethodName.GetCardBattleCellSize)
		{
			return true;
		}
		if (method == MethodName.GetCardBattleCellRect)
		{
			return true;
		}
		if (method == MethodName.GetCardBattleCellCenter)
		{
			return true;
		}
		if (method == MethodName.IsCardBattleCell)
		{
			return true;
		}
		if (method == MethodName.PreviewCharacterAtCardCell)
		{
			return true;
		}
		if (method == MethodName.RestoreCardCharacterPreviewPosition)
		{
			return true;
		}
		if (method == MethodName.UpdateCardBattleViewportLayout)
		{
			return true;
		}
		if (method == MethodName.PreviewCardPlantAndCooldown)
		{
			return true;
		}
		if (method == MethodName.ResetCardRuntimePreview)
		{
			return true;
		}
		if (method == MethodName.StartConfiguredCardCooldown)
		{
			return true;
		}
		if (method == MethodName.ToggleCardRuntimeClock)
		{
			return true;
		}
		if (method == MethodName.StopCardRuntimeClock)
		{
			return true;
		}
		if (method == MethodName.EnsureCardRuntimeCooldownDuration)
		{
			return true;
		}
		if (method == MethodName.ResolveCardAvailabilityState)
		{
			return true;
		}
		if (method == MethodName.ToggleCardRuntimeSelection)
		{
			return true;
		}
		if (method == MethodName.OnCardRuntimeMouseEntered)
		{
			return true;
		}
		if (method == MethodName.OnCardRuntimeMouseExited)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.ApplyCardRuntimePreviewState)
		{
			return true;
		}
		if (method == MethodName.RefreshCardRuntimeReadouts)
		{
			return true;
		}
		if (method == MethodName.UpdateCardRuntimeCooldownVisual)
		{
			return true;
		}
		if (method == MethodName.RebuildCardCharacterPreview)
		{
			return true;
		}
		if (method == MethodName.DisposeCardCharacterPreview)
		{
			return true;
		}
		if (method == MethodName.BuildCardArmorFingerprint)
		{
			return true;
		}
		if (method == MethodName.FindCardRuntimeCharacter)
		{
			return true;
		}
		if (method == MethodName.SetCardCharacterMissing)
		{
			return true;
		}
		if (method == MethodName.SetPacketShowPreviewMissing)
		{
			return true;
		}
		if (method == MethodName.GetEffectiveCostText)
		{
			return true;
		}
		if (method == MethodName.GetEffectiveCost)
		{
			return true;
		}
		if (method == MethodName.GetEffectiveCooldownText)
		{
			return true;
		}
		if (method == MethodName.BuildCardValueOverrideSummary)
		{
			return true;
		}
		if (method == MethodName.GetPreviewValueSourceLabel)
		{
			return true;
		}
		if (method == MethodName.GetPacketTypeColor)
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
		if (method == MethodName.FormatResource)
		{
			return true;
		}
		if (method == MethodName.FormatCharacterBinding)
		{
			return true;
		}
		if (method == MethodName.FormatVariant)
		{
			return true;
		}
		if (method == MethodName.AddItemIfMissing)
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
		if (name == PropertyName._editingPacket)
		{
			_editingPacket = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName._previewNameLabel)
		{
			_previewNameLabel = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._inlineSaveKeyLineEdit)
		{
			_inlineSaveKeyLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._inlineAnimationLineEdit)
		{
			_inlineAnimationLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._inlineCostSpinBox)
		{
			_inlineCostSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._inlineCooldownSpinBox)
		{
			_inlineCooldownSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._inlineStartingCooldownSpinBox)
		{
			_inlineStartingCooldownSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._previewKeyLabel)
		{
			_previewKeyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewCostLabel)
		{
			_previewCostLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewCooldownLabel)
		{
			_previewCooldownLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewTypeLabel)
		{
			_previewTypeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewCharacterLabel)
		{
			_previewCharacterLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewAnimationLabel)
		{
			_previewAnimationLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._valueOverrideBadge)
		{
			_valueOverrideBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._battleStatusLabel)
		{
			_battleStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._cooldownPercentLabel)
		{
			_cooldownPercentLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._runtimeClockLabel)
		{
			_runtimeClockLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._runtimeStateCards)
		{
			_runtimeStateCards = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._cooldownProgressSlider)
		{
			_cooldownProgressSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._previewSunSpinBox)
		{
			_previewSunSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._mobilePreviewCheck)
		{
			_mobilePreviewCheck = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._cardInteractionButton)
		{
			_cardInteractionButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._pauseRuntimeButton)
		{
			_pauseRuntimeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._cardFrame)
		{
			_cardFrame = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._cardPacketShowPreviewRoot)
		{
			_cardPacketShowPreviewRoot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._cardPacketShowPreview)
		{
			_cardPacketShowPreview = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName._cardPacketShowSpriteFingerprint)
		{
			_cardPacketShowSpriteFingerprint = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._cardPacketShowAnimationFingerprint)
		{
			_cardPacketShowAnimationFingerprint = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._cardPacketShowSaveKeyFingerprint)
		{
			_cardPacketShowSaveKeyFingerprint = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._cardPacketShowPreviewBuildCount)
		{
			_cardPacketShowPreviewBuildCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cardPacketShowSpriteRebuildCount)
		{
			_cardPacketShowSpriteRebuildCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cardPreviewRefreshCommitted)
		{
			_cardPreviewRefreshCommitted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardCharacterViewportContainer)
		{
			_cardCharacterViewportContainer = VariantUtils.ConvertTo<SubViewportContainer>(in value);
			return true;
		}
		if (name == PropertyName._cardCharacterViewport)
		{
			_cardCharacterViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._cardGameBackground)
		{
			_cardGameBackground = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._cardBattleTint)
		{
			_cardBattleTint = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName._cardCharacterPreviewRoot)
		{
			_cardCharacterPreviewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._cardBattleDropSurface)
		{
			_cardBattleDropSurface = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._cardDragTargetHighlight)
		{
			_cardDragTargetHighlight = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName._cardDragPacketGhost)
		{
			_cardDragPacketGhost = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName._cardCharacterPreview)
		{
			_cardCharacterPreview = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._cardCharacterMissingLabel)
		{
			_cardCharacterMissingLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._cardImageMissingLabel)
		{
			_cardImageMissingLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._eventSummaryLabel)
		{
			_eventSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._behaviorSummaryLabel)
		{
			_behaviorSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._registryBehaviorStatusLabel)
		{
			_registryBehaviorStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._registryBehaviorList)
		{
			_registryBehaviorList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._inlineBehaviorList)
		{
			_inlineBehaviorList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._customBehaviorIdEdit)
		{
			_customBehaviorIdEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._registryBehaviorPicker)
		{
			_registryBehaviorPicker = VariantUtils.ConvertTo<PopupMenu>(in value);
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
		if (name == PropertyName._unlockConditionList)
		{
			_unlockConditionList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._armorList)
		{
			_armorList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._unlockConditionPicker)
		{
			_unlockConditionPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._characterBindingButton)
		{
			_characterBindingButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._characterSourceLabel)
		{
			_characterSourceLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._overrideStatus)
		{
			_overrideStatus = VariantUtils.ConvertTo<Label>(in value);
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
		if (name == PropertyName._characterResourcePicker)
		{
			_characterResourcePicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
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
		if (name == PropertyName._behaviorRegistryRevision)
		{
			_behaviorRegistryRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._behaviorRegistryPollAccumulator)
		{
			_behaviorRegistryPollAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._selectedRegistryBehaviorIndex)
		{
			_selectedRegistryBehaviorIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedInlineBehaviorIndex)
		{
			_selectedInlineBehaviorIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedEventPressIndex)
		{
			_selectedEventPressIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedEventPlantIndex)
		{
			_selectedEventPlantIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedUnlockIndex)
		{
			_selectedUnlockIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedArmorIndex)
		{
			_selectedArmorIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardRuntimePreviewState)
		{
			_cardRuntimePreviewState = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cardRuntimeCooldownProgress)
		{
			_cardRuntimeCooldownProgress = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._cardRuntimeCooldownDuration)
		{
			_cardRuntimeCooldownDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._cardRuntimeCooldownRemaining)
		{
			_cardRuntimeCooldownRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._cardRuntimeCooldownRunning)
		{
			_cardRuntimeCooldownRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardRuntimeHovered)
		{
			_cardRuntimeHovered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardRuntimeMobile)
		{
			_cardRuntimeMobile = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardRuntimeReadoutAccumulator)
		{
			_cardRuntimeReadoutAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._cardRuntimeReadoutRefreshCount)
		{
			_cardRuntimeReadoutRefreshCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cardCharacterPreviewConfig)
		{
			_cardCharacterPreviewConfig = VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in value);
			return true;
		}
		if (name == PropertyName._cardCharacterPreviewArmorFingerprint)
		{
			_cardCharacterPreviewArmorFingerprint = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._cardCharacterPreviewInitialized)
		{
			_cardCharacterPreviewInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardCharacterPreviewBuildCount)
		{
			_cardCharacterPreviewBuildCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cardDragCandidate)
		{
			_cardDragCandidate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardDragActive)
		{
			_cardDragActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardDragTouch)
		{
			_cardDragTouch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardDragTouchIndex)
		{
			_cardDragTouchIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cardDragStartGlobalPosition)
		{
			_cardDragStartGlobalPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._cardDragPointerGlobalPosition)
		{
			_cardDragPointerGlobalPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._cardDragPreviewCell)
		{
			_cardDragPreviewCell = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._cardPlacedPreviewCell)
		{
			_cardPlacedPreviewCell = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._cardDragTargetValid)
		{
			_cardDragTargetValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardHasPlacedPreview)
		{
			_cardHasPlacedPreview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cardDragPreviewPlacementCount)
		{
			_cardDragPreviewPlacementCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cardDragRejectedCount)
		{
			_cardDragRejectedCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cardDragCancelCount)
		{
			_cardDragCancelCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.CardRuntimeReadoutRefreshCount)
		{
			from = CardRuntimeReadoutRefreshCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CardCharacterPreviewBuildCount)
		{
			from = CardCharacterPreviewBuildCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CardPacketShowPreviewBuildCount)
		{
			from = CardPacketShowPreviewBuildCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CardPacketShowSpriteRebuildCount)
		{
			from = CardPacketShowSpriteRebuildCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.IsCardDragActive)
		{
			from2 = IsCardDragActive;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsCardDragTargetValid)
		{
			from2 = IsCardDragTargetValid;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.HasCardDragGhost)
		{
			from2 = HasCardDragGhost;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		Vector2I from3;
		if (name == PropertyName.CardDragPreviewCell)
		{
			from3 = CardDragPreviewCell;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CardPlacedPreviewCell)
		{
			from3 = CardPlacedPreviewCell;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CardDragPreviewPlacementCount)
		{
			from = CardDragPreviewPlacementCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CardDragRejectedCount)
		{
			from = CardDragRejectedCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CardDragCancelCount)
		{
			from = CardDragCancelCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CardDragTargetColor)
		{
			value = VariantUtils.CreateFrom<Color>(CardDragTargetColor);
			return true;
		}
		if (name == PropertyName.RuntimeStateVisualCardCount)
		{
			from = RuntimeStateVisualCardCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsCardPreviewRendering)
		{
			from2 = IsCardPreviewRendering;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._editingPacket)
		{
			value = VariantUtils.CreateFrom(in _editingPacket);
			return true;
		}
		if (name == PropertyName._previewNameLabel)
		{
			value = VariantUtils.CreateFrom(in _previewNameLabel);
			return true;
		}
		if (name == PropertyName._inlineSaveKeyLineEdit)
		{
			value = VariantUtils.CreateFrom(in _inlineSaveKeyLineEdit);
			return true;
		}
		if (name == PropertyName._inlineAnimationLineEdit)
		{
			value = VariantUtils.CreateFrom(in _inlineAnimationLineEdit);
			return true;
		}
		if (name == PropertyName._inlineCostSpinBox)
		{
			value = VariantUtils.CreateFrom(in _inlineCostSpinBox);
			return true;
		}
		if (name == PropertyName._inlineCooldownSpinBox)
		{
			value = VariantUtils.CreateFrom(in _inlineCooldownSpinBox);
			return true;
		}
		if (name == PropertyName._inlineStartingCooldownSpinBox)
		{
			value = VariantUtils.CreateFrom(in _inlineStartingCooldownSpinBox);
			return true;
		}
		if (name == PropertyName._previewKeyLabel)
		{
			value = VariantUtils.CreateFrom(in _previewKeyLabel);
			return true;
		}
		if (name == PropertyName._previewCostLabel)
		{
			value = VariantUtils.CreateFrom(in _previewCostLabel);
			return true;
		}
		if (name == PropertyName._previewCooldownLabel)
		{
			value = VariantUtils.CreateFrom(in _previewCooldownLabel);
			return true;
		}
		if (name == PropertyName._previewTypeLabel)
		{
			value = VariantUtils.CreateFrom(in _previewTypeLabel);
			return true;
		}
		if (name == PropertyName._previewCharacterLabel)
		{
			value = VariantUtils.CreateFrom(in _previewCharacterLabel);
			return true;
		}
		if (name == PropertyName._previewAnimationLabel)
		{
			value = VariantUtils.CreateFrom(in _previewAnimationLabel);
			return true;
		}
		if (name == PropertyName._valueOverrideBadge)
		{
			value = VariantUtils.CreateFrom(in _valueOverrideBadge);
			return true;
		}
		if (name == PropertyName._battleStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _battleStatusLabel);
			return true;
		}
		if (name == PropertyName._cooldownPercentLabel)
		{
			value = VariantUtils.CreateFrom(in _cooldownPercentLabel);
			return true;
		}
		if (name == PropertyName._runtimeClockLabel)
		{
			value = VariantUtils.CreateFrom(in _runtimeClockLabel);
			return true;
		}
		if (name == PropertyName._runtimeStateCards)
		{
			value = VariantUtils.CreateFrom(in _runtimeStateCards);
			return true;
		}
		if (name == PropertyName._cooldownProgressSlider)
		{
			value = VariantUtils.CreateFrom(in _cooldownProgressSlider);
			return true;
		}
		if (name == PropertyName._previewSunSpinBox)
		{
			value = VariantUtils.CreateFrom(in _previewSunSpinBox);
			return true;
		}
		if (name == PropertyName._mobilePreviewCheck)
		{
			value = VariantUtils.CreateFrom(in _mobilePreviewCheck);
			return true;
		}
		if (name == PropertyName._cardInteractionButton)
		{
			value = VariantUtils.CreateFrom(in _cardInteractionButton);
			return true;
		}
		if (name == PropertyName._pauseRuntimeButton)
		{
			value = VariantUtils.CreateFrom(in _pauseRuntimeButton);
			return true;
		}
		if (name == PropertyName._cardFrame)
		{
			value = VariantUtils.CreateFrom(in _cardFrame);
			return true;
		}
		if (name == PropertyName._cardPacketShowPreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _cardPacketShowPreviewRoot);
			return true;
		}
		if (name == PropertyName._cardPacketShowPreview)
		{
			value = VariantUtils.CreateFrom(in _cardPacketShowPreview);
			return true;
		}
		if (name == PropertyName._cardPacketShowSpriteFingerprint)
		{
			value = VariantUtils.CreateFrom(in _cardPacketShowSpriteFingerprint);
			return true;
		}
		if (name == PropertyName._cardPacketShowAnimationFingerprint)
		{
			value = VariantUtils.CreateFrom(in _cardPacketShowAnimationFingerprint);
			return true;
		}
		if (name == PropertyName._cardPacketShowSaveKeyFingerprint)
		{
			value = VariantUtils.CreateFrom(in _cardPacketShowSaveKeyFingerprint);
			return true;
		}
		if (name == PropertyName._cardPacketShowPreviewBuildCount)
		{
			value = VariantUtils.CreateFrom(in _cardPacketShowPreviewBuildCount);
			return true;
		}
		if (name == PropertyName._cardPacketShowSpriteRebuildCount)
		{
			value = VariantUtils.CreateFrom(in _cardPacketShowSpriteRebuildCount);
			return true;
		}
		if (name == PropertyName._cardPreviewRefreshCommitted)
		{
			value = VariantUtils.CreateFrom(in _cardPreviewRefreshCommitted);
			return true;
		}
		if (name == PropertyName._cardCharacterViewportContainer)
		{
			value = VariantUtils.CreateFrom(in _cardCharacterViewportContainer);
			return true;
		}
		if (name == PropertyName._cardCharacterViewport)
		{
			value = VariantUtils.CreateFrom(in _cardCharacterViewport);
			return true;
		}
		if (name == PropertyName._cardGameBackground)
		{
			value = VariantUtils.CreateFrom(in _cardGameBackground);
			return true;
		}
		if (name == PropertyName._cardBattleTint)
		{
			value = VariantUtils.CreateFrom(in _cardBattleTint);
			return true;
		}
		if (name == PropertyName._cardCharacterPreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _cardCharacterPreviewRoot);
			return true;
		}
		if (name == PropertyName._cardBattleDropSurface)
		{
			value = VariantUtils.CreateFrom(in _cardBattleDropSurface);
			return true;
		}
		if (name == PropertyName._cardDragTargetHighlight)
		{
			value = VariantUtils.CreateFrom(in _cardDragTargetHighlight);
			return true;
		}
		if (name == PropertyName._cardDragPacketGhost)
		{
			value = VariantUtils.CreateFrom(in _cardDragPacketGhost);
			return true;
		}
		if (name == PropertyName._cardCharacterPreview)
		{
			value = VariantUtils.CreateFrom(in _cardCharacterPreview);
			return true;
		}
		if (name == PropertyName._cardCharacterMissingLabel)
		{
			value = VariantUtils.CreateFrom(in _cardCharacterMissingLabel);
			return true;
		}
		if (name == PropertyName._cardImageMissingLabel)
		{
			value = VariantUtils.CreateFrom(in _cardImageMissingLabel);
			return true;
		}
		if (name == PropertyName._eventSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _eventSummaryLabel);
			return true;
		}
		if (name == PropertyName._behaviorSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _behaviorSummaryLabel);
			return true;
		}
		if (name == PropertyName._registryBehaviorStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _registryBehaviorStatusLabel);
			return true;
		}
		if (name == PropertyName._registryBehaviorList)
		{
			value = VariantUtils.CreateFrom(in _registryBehaviorList);
			return true;
		}
		if (name == PropertyName._inlineBehaviorList)
		{
			value = VariantUtils.CreateFrom(in _inlineBehaviorList);
			return true;
		}
		if (name == PropertyName._customBehaviorIdEdit)
		{
			value = VariantUtils.CreateFrom(in _customBehaviorIdEdit);
			return true;
		}
		if (name == PropertyName._registryBehaviorPicker)
		{
			value = VariantUtils.CreateFrom(in _registryBehaviorPicker);
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
		if (name == PropertyName._unlockConditionList)
		{
			value = VariantUtils.CreateFrom(in _unlockConditionList);
			return true;
		}
		if (name == PropertyName._armorList)
		{
			value = VariantUtils.CreateFrom(in _armorList);
			return true;
		}
		if (name == PropertyName._unlockConditionPicker)
		{
			value = VariantUtils.CreateFrom(in _unlockConditionPicker);
			return true;
		}
		if (name == PropertyName._characterBindingButton)
		{
			value = VariantUtils.CreateFrom(in _characterBindingButton);
			return true;
		}
		if (name == PropertyName._characterSourceLabel)
		{
			value = VariantUtils.CreateFrom(in _characterSourceLabel);
			return true;
		}
		if (name == PropertyName._overrideStatus)
		{
			value = VariantUtils.CreateFrom(in _overrideStatus);
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
		if (name == PropertyName._characterResourcePicker)
		{
			value = VariantUtils.CreateFrom(in _characterResourcePicker);
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
		if (name == PropertyName._behaviorRegistryRevision)
		{
			value = VariantUtils.CreateFrom(in _behaviorRegistryRevision);
			return true;
		}
		if (name == PropertyName._behaviorRegistryPollAccumulator)
		{
			value = VariantUtils.CreateFrom(in _behaviorRegistryPollAccumulator);
			return true;
		}
		if (name == PropertyName._selectedRegistryBehaviorIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedRegistryBehaviorIndex);
			return true;
		}
		if (name == PropertyName._selectedInlineBehaviorIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedInlineBehaviorIndex);
			return true;
		}
		if (name == PropertyName._selectedEventPressIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedEventPressIndex);
			return true;
		}
		if (name == PropertyName._selectedEventPlantIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedEventPlantIndex);
			return true;
		}
		if (name == PropertyName._selectedUnlockIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedUnlockIndex);
			return true;
		}
		if (name == PropertyName._selectedArmorIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedArmorIndex);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._cardRuntimePreviewState)
		{
			value = VariantUtils.CreateFrom(in _cardRuntimePreviewState);
			return true;
		}
		if (name == PropertyName._cardRuntimeCooldownProgress)
		{
			value = VariantUtils.CreateFrom(in _cardRuntimeCooldownProgress);
			return true;
		}
		if (name == PropertyName._cardRuntimeCooldownDuration)
		{
			value = VariantUtils.CreateFrom(in _cardRuntimeCooldownDuration);
			return true;
		}
		if (name == PropertyName._cardRuntimeCooldownRemaining)
		{
			value = VariantUtils.CreateFrom(in _cardRuntimeCooldownRemaining);
			return true;
		}
		if (name == PropertyName._cardRuntimeCooldownRunning)
		{
			value = VariantUtils.CreateFrom(in _cardRuntimeCooldownRunning);
			return true;
		}
		if (name == PropertyName._cardRuntimeHovered)
		{
			value = VariantUtils.CreateFrom(in _cardRuntimeHovered);
			return true;
		}
		if (name == PropertyName._cardRuntimeMobile)
		{
			value = VariantUtils.CreateFrom(in _cardRuntimeMobile);
			return true;
		}
		if (name == PropertyName._cardRuntimeReadoutAccumulator)
		{
			value = VariantUtils.CreateFrom(in _cardRuntimeReadoutAccumulator);
			return true;
		}
		if (name == PropertyName._cardRuntimeReadoutRefreshCount)
		{
			value = VariantUtils.CreateFrom(in _cardRuntimeReadoutRefreshCount);
			return true;
		}
		if (name == PropertyName._cardCharacterPreviewConfig)
		{
			value = VariantUtils.CreateFrom(in _cardCharacterPreviewConfig);
			return true;
		}
		if (name == PropertyName._cardCharacterPreviewArmorFingerprint)
		{
			value = VariantUtils.CreateFrom(in _cardCharacterPreviewArmorFingerprint);
			return true;
		}
		if (name == PropertyName._cardCharacterPreviewInitialized)
		{
			value = VariantUtils.CreateFrom(in _cardCharacterPreviewInitialized);
			return true;
		}
		if (name == PropertyName._cardCharacterPreviewBuildCount)
		{
			value = VariantUtils.CreateFrom(in _cardCharacterPreviewBuildCount);
			return true;
		}
		if (name == PropertyName._cardDragCandidate)
		{
			value = VariantUtils.CreateFrom(in _cardDragCandidate);
			return true;
		}
		if (name == PropertyName._cardDragActive)
		{
			value = VariantUtils.CreateFrom(in _cardDragActive);
			return true;
		}
		if (name == PropertyName._cardDragTouch)
		{
			value = VariantUtils.CreateFrom(in _cardDragTouch);
			return true;
		}
		if (name == PropertyName._cardDragTouchIndex)
		{
			value = VariantUtils.CreateFrom(in _cardDragTouchIndex);
			return true;
		}
		if (name == PropertyName._cardDragStartGlobalPosition)
		{
			value = VariantUtils.CreateFrom(in _cardDragStartGlobalPosition);
			return true;
		}
		if (name == PropertyName._cardDragPointerGlobalPosition)
		{
			value = VariantUtils.CreateFrom(in _cardDragPointerGlobalPosition);
			return true;
		}
		if (name == PropertyName._cardDragPreviewCell)
		{
			value = VariantUtils.CreateFrom(in _cardDragPreviewCell);
			return true;
		}
		if (name == PropertyName._cardPlacedPreviewCell)
		{
			value = VariantUtils.CreateFrom(in _cardPlacedPreviewCell);
			return true;
		}
		if (name == PropertyName._cardDragTargetValid)
		{
			value = VariantUtils.CreateFrom(in _cardDragTargetValid);
			return true;
		}
		if (name == PropertyName._cardHasPlacedPreview)
		{
			value = VariantUtils.CreateFrom(in _cardHasPlacedPreview);
			return true;
		}
		if (name == PropertyName._cardDragPreviewPlacementCount)
		{
			value = VariantUtils.CreateFrom(in _cardDragPreviewPlacementCount);
			return true;
		}
		if (name == PropertyName._cardDragRejectedCount)
		{
			value = VariantUtils.CreateFrom(in _cardDragRejectedCount);
			return true;
		}
		if (name == PropertyName._cardDragCancelCount)
		{
			value = VariantUtils.CreateFrom(in _cardDragCancelCount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineSaveKeyLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineAnimationLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineCostSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineCooldownSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineStartingCooldownSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewKeyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewCostLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewCooldownLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewTypeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewCharacterLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewAnimationLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._valueOverrideBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._battleStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cooldownPercentLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeClockLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeStateCards, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cooldownProgressSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewSunSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mobilePreviewCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardInteractionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pauseRuntimeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardPacketShowPreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardPacketShowPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._cardPacketShowSpriteFingerprint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._cardPacketShowAnimationFingerprint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._cardPacketShowSaveKeyFingerprint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cardPacketShowPreviewBuildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cardPacketShowSpriteRebuildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cardPreviewRefreshCommitted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardCharacterViewportContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardCharacterViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardGameBackground, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardBattleTint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardCharacterPreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardBattleDropSurface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardDragTargetHighlight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardDragPacketGhost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardCharacterPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardCharacterMissingLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardImageMissingLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._behaviorSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._registryBehaviorStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._registryBehaviorList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineBehaviorList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._customBehaviorIdEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._registryBehaviorPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pressedActionsList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._useSucceededActionsList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockConditionList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._armorList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockConditionPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterBindingButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterSourceLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._createOverrideButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openOverrideButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clearOverrideButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterResourcePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetEventSelectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._packetEventTargetProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._behaviorRegistryRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._behaviorRegistryPollAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedRegistryBehaviorIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedInlineBehaviorIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedEventPressIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedEventPlantIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedUnlockIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedArmorIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cardRuntimePreviewState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cardRuntimeCooldownProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cardRuntimeCooldownDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cardRuntimeCooldownRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cardRuntimeCooldownRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cardRuntimeHovered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cardRuntimeMobile, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cardRuntimeReadoutAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cardRuntimeReadoutRefreshCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardCharacterPreviewConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._cardCharacterPreviewArmorFingerprint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cardCharacterPreviewInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cardCharacterPreviewBuildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cardDragCandidate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cardDragActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cardDragTouch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cardDragTouchIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._cardDragStartGlobalPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._cardDragPointerGlobalPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._cardDragPreviewCell, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._cardPlacedPreviewCell, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cardDragTargetValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cardHasPlacedPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cardDragPreviewPlacementCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cardDragRejectedCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cardDragCancelCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CardRuntimeReadoutRefreshCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CardCharacterPreviewBuildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CardPacketShowPreviewBuildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CardPacketShowSpriteRebuildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCardDragActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCardDragTargetValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasCardDragGhost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.CardDragPreviewCell, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.CardPlacedPreviewCell, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CardDragPreviewPlacementCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CardDragRejectedCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CardDragCancelCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.CardDragTargetColor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RuntimeStateVisualCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCardPreviewRendering, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingPacket, Variant.From(in _editingPacket));
		info.AddProperty(PropertyName._previewNameLabel, Variant.From(in _previewNameLabel));
		info.AddProperty(PropertyName._inlineSaveKeyLineEdit, Variant.From(in _inlineSaveKeyLineEdit));
		info.AddProperty(PropertyName._inlineAnimationLineEdit, Variant.From(in _inlineAnimationLineEdit));
		info.AddProperty(PropertyName._inlineCostSpinBox, Variant.From(in _inlineCostSpinBox));
		info.AddProperty(PropertyName._inlineCooldownSpinBox, Variant.From(in _inlineCooldownSpinBox));
		info.AddProperty(PropertyName._inlineStartingCooldownSpinBox, Variant.From(in _inlineStartingCooldownSpinBox));
		info.AddProperty(PropertyName._previewKeyLabel, Variant.From(in _previewKeyLabel));
		info.AddProperty(PropertyName._previewCostLabel, Variant.From(in _previewCostLabel));
		info.AddProperty(PropertyName._previewCooldownLabel, Variant.From(in _previewCooldownLabel));
		info.AddProperty(PropertyName._previewTypeLabel, Variant.From(in _previewTypeLabel));
		info.AddProperty(PropertyName._previewCharacterLabel, Variant.From(in _previewCharacterLabel));
		info.AddProperty(PropertyName._previewAnimationLabel, Variant.From(in _previewAnimationLabel));
		info.AddProperty(PropertyName._valueOverrideBadge, Variant.From(in _valueOverrideBadge));
		info.AddProperty(PropertyName._battleStatusLabel, Variant.From(in _battleStatusLabel));
		info.AddProperty(PropertyName._cooldownPercentLabel, Variant.From(in _cooldownPercentLabel));
		info.AddProperty(PropertyName._runtimeClockLabel, Variant.From(in _runtimeClockLabel));
		info.AddProperty(PropertyName._runtimeStateCards, Variant.From(in _runtimeStateCards));
		info.AddProperty(PropertyName._cooldownProgressSlider, Variant.From(in _cooldownProgressSlider));
		info.AddProperty(PropertyName._previewSunSpinBox, Variant.From(in _previewSunSpinBox));
		info.AddProperty(PropertyName._mobilePreviewCheck, Variant.From(in _mobilePreviewCheck));
		info.AddProperty(PropertyName._cardInteractionButton, Variant.From(in _cardInteractionButton));
		info.AddProperty(PropertyName._pauseRuntimeButton, Variant.From(in _pauseRuntimeButton));
		info.AddProperty(PropertyName._cardFrame, Variant.From(in _cardFrame));
		info.AddProperty(PropertyName._cardPacketShowPreviewRoot, Variant.From(in _cardPacketShowPreviewRoot));
		info.AddProperty(PropertyName._cardPacketShowPreview, Variant.From(in _cardPacketShowPreview));
		info.AddProperty(PropertyName._cardPacketShowSpriteFingerprint, Variant.From(in _cardPacketShowSpriteFingerprint));
		info.AddProperty(PropertyName._cardPacketShowAnimationFingerprint, Variant.From(in _cardPacketShowAnimationFingerprint));
		info.AddProperty(PropertyName._cardPacketShowSaveKeyFingerprint, Variant.From(in _cardPacketShowSaveKeyFingerprint));
		info.AddProperty(PropertyName._cardPacketShowPreviewBuildCount, Variant.From(in _cardPacketShowPreviewBuildCount));
		info.AddProperty(PropertyName._cardPacketShowSpriteRebuildCount, Variant.From(in _cardPacketShowSpriteRebuildCount));
		info.AddProperty(PropertyName._cardPreviewRefreshCommitted, Variant.From(in _cardPreviewRefreshCommitted));
		info.AddProperty(PropertyName._cardCharacterViewportContainer, Variant.From(in _cardCharacterViewportContainer));
		info.AddProperty(PropertyName._cardCharacterViewport, Variant.From(in _cardCharacterViewport));
		info.AddProperty(PropertyName._cardGameBackground, Variant.From(in _cardGameBackground));
		info.AddProperty(PropertyName._cardBattleTint, Variant.From(in _cardBattleTint));
		info.AddProperty(PropertyName._cardCharacterPreviewRoot, Variant.From(in _cardCharacterPreviewRoot));
		info.AddProperty(PropertyName._cardBattleDropSurface, Variant.From(in _cardBattleDropSurface));
		info.AddProperty(PropertyName._cardDragTargetHighlight, Variant.From(in _cardDragTargetHighlight));
		info.AddProperty(PropertyName._cardDragPacketGhost, Variant.From(in _cardDragPacketGhost));
		info.AddProperty(PropertyName._cardCharacterPreview, Variant.From(in _cardCharacterPreview));
		info.AddProperty(PropertyName._cardCharacterMissingLabel, Variant.From(in _cardCharacterMissingLabel));
		info.AddProperty(PropertyName._cardImageMissingLabel, Variant.From(in _cardImageMissingLabel));
		info.AddProperty(PropertyName._eventSummaryLabel, Variant.From(in _eventSummaryLabel));
		info.AddProperty(PropertyName._behaviorSummaryLabel, Variant.From(in _behaviorSummaryLabel));
		info.AddProperty(PropertyName._registryBehaviorStatusLabel, Variant.From(in _registryBehaviorStatusLabel));
		info.AddProperty(PropertyName._registryBehaviorList, Variant.From(in _registryBehaviorList));
		info.AddProperty(PropertyName._inlineBehaviorList, Variant.From(in _inlineBehaviorList));
		info.AddProperty(PropertyName._customBehaviorIdEdit, Variant.From(in _customBehaviorIdEdit));
		info.AddProperty(PropertyName._registryBehaviorPicker, Variant.From(in _registryBehaviorPicker));
		info.AddProperty(PropertyName._pressedActionsList, Variant.From(in _pressedActionsList));
		info.AddProperty(PropertyName._useSucceededActionsList, Variant.From(in _useSucceededActionsList));
		info.AddProperty(PropertyName._unlockConditionList, Variant.From(in _unlockConditionList));
		info.AddProperty(PropertyName._armorList, Variant.From(in _armorList));
		info.AddProperty(PropertyName._unlockConditionPicker, Variant.From(in _unlockConditionPicker));
		info.AddProperty(PropertyName._characterBindingButton, Variant.From(in _characterBindingButton));
		info.AddProperty(PropertyName._characterSourceLabel, Variant.From(in _characterSourceLabel));
		info.AddProperty(PropertyName._overrideStatus, Variant.From(in _overrideStatus));
		info.AddProperty(PropertyName._createOverrideButton, Variant.From(in _createOverrideButton));
		info.AddProperty(PropertyName._openOverrideButton, Variant.From(in _openOverrideButton));
		info.AddProperty(PropertyName._clearOverrideButton, Variant.From(in _clearOverrideButton));
		info.AddProperty(PropertyName._characterResourcePicker, Variant.From(in _characterResourcePicker));
		info.AddProperty(PropertyName._packetEventSelectorWindow, Variant.From(in _packetEventSelectorWindow));
		info.AddProperty(PropertyName._packetEventTargetProperty, Variant.From(in _packetEventTargetProperty));
		info.AddProperty(PropertyName._behaviorRegistryRevision, Variant.From(in _behaviorRegistryRevision));
		info.AddProperty(PropertyName._behaviorRegistryPollAccumulator, Variant.From(in _behaviorRegistryPollAccumulator));
		info.AddProperty(PropertyName._selectedRegistryBehaviorIndex, Variant.From(in _selectedRegistryBehaviorIndex));
		info.AddProperty(PropertyName._selectedInlineBehaviorIndex, Variant.From(in _selectedInlineBehaviorIndex));
		info.AddProperty(PropertyName._selectedEventPressIndex, Variant.From(in _selectedEventPressIndex));
		info.AddProperty(PropertyName._selectedEventPlantIndex, Variant.From(in _selectedEventPlantIndex));
		info.AddProperty(PropertyName._selectedUnlockIndex, Variant.From(in _selectedUnlockIndex));
		info.AddProperty(PropertyName._selectedArmorIndex, Variant.From(in _selectedArmorIndex));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._cardRuntimePreviewState, Variant.From(in _cardRuntimePreviewState));
		info.AddProperty(PropertyName._cardRuntimeCooldownProgress, Variant.From(in _cardRuntimeCooldownProgress));
		info.AddProperty(PropertyName._cardRuntimeCooldownDuration, Variant.From(in _cardRuntimeCooldownDuration));
		info.AddProperty(PropertyName._cardRuntimeCooldownRemaining, Variant.From(in _cardRuntimeCooldownRemaining));
		info.AddProperty(PropertyName._cardRuntimeCooldownRunning, Variant.From(in _cardRuntimeCooldownRunning));
		info.AddProperty(PropertyName._cardRuntimeHovered, Variant.From(in _cardRuntimeHovered));
		info.AddProperty(PropertyName._cardRuntimeMobile, Variant.From(in _cardRuntimeMobile));
		info.AddProperty(PropertyName._cardRuntimeReadoutAccumulator, Variant.From(in _cardRuntimeReadoutAccumulator));
		info.AddProperty(PropertyName._cardRuntimeReadoutRefreshCount, Variant.From(in _cardRuntimeReadoutRefreshCount));
		info.AddProperty(PropertyName._cardCharacterPreviewConfig, Variant.From(in _cardCharacterPreviewConfig));
		info.AddProperty(PropertyName._cardCharacterPreviewArmorFingerprint, Variant.From(in _cardCharacterPreviewArmorFingerprint));
		info.AddProperty(PropertyName._cardCharacterPreviewInitialized, Variant.From(in _cardCharacterPreviewInitialized));
		info.AddProperty(PropertyName._cardCharacterPreviewBuildCount, Variant.From(in _cardCharacterPreviewBuildCount));
		info.AddProperty(PropertyName._cardDragCandidate, Variant.From(in _cardDragCandidate));
		info.AddProperty(PropertyName._cardDragActive, Variant.From(in _cardDragActive));
		info.AddProperty(PropertyName._cardDragTouch, Variant.From(in _cardDragTouch));
		info.AddProperty(PropertyName._cardDragTouchIndex, Variant.From(in _cardDragTouchIndex));
		info.AddProperty(PropertyName._cardDragStartGlobalPosition, Variant.From(in _cardDragStartGlobalPosition));
		info.AddProperty(PropertyName._cardDragPointerGlobalPosition, Variant.From(in _cardDragPointerGlobalPosition));
		info.AddProperty(PropertyName._cardDragPreviewCell, Variant.From(in _cardDragPreviewCell));
		info.AddProperty(PropertyName._cardPlacedPreviewCell, Variant.From(in _cardPlacedPreviewCell));
		info.AddProperty(PropertyName._cardDragTargetValid, Variant.From(in _cardDragTargetValid));
		info.AddProperty(PropertyName._cardHasPlacedPreview, Variant.From(in _cardHasPlacedPreview));
		info.AddProperty(PropertyName._cardDragPreviewPlacementCount, Variant.From(in _cardDragPreviewPlacementCount));
		info.AddProperty(PropertyName._cardDragRejectedCount, Variant.From(in _cardDragRejectedCount));
		info.AddProperty(PropertyName._cardDragCancelCount, Variant.From(in _cardDragCancelCount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingPacket, out var value))
		{
			_editingPacket = value.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName._previewNameLabel, out var value2))
		{
			_previewNameLabel = value2.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._inlineSaveKeyLineEdit, out var value3))
		{
			_inlineSaveKeyLineEdit = value3.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._inlineAnimationLineEdit, out var value4))
		{
			_inlineAnimationLineEdit = value4.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._inlineCostSpinBox, out var value5))
		{
			_inlineCostSpinBox = value5.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._inlineCooldownSpinBox, out var value6))
		{
			_inlineCooldownSpinBox = value6.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._inlineStartingCooldownSpinBox, out var value7))
		{
			_inlineStartingCooldownSpinBox = value7.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._previewKeyLabel, out var value8))
		{
			_previewKeyLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewCostLabel, out var value9))
		{
			_previewCostLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewCooldownLabel, out var value10))
		{
			_previewCooldownLabel = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewTypeLabel, out var value11))
		{
			_previewTypeLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewCharacterLabel, out var value12))
		{
			_previewCharacterLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewAnimationLabel, out var value13))
		{
			_previewAnimationLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._valueOverrideBadge, out var value14))
		{
			_valueOverrideBadge = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._battleStatusLabel, out var value15))
		{
			_battleStatusLabel = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._cooldownPercentLabel, out var value16))
		{
			_cooldownPercentLabel = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._runtimeClockLabel, out var value17))
		{
			_runtimeClockLabel = value17.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._runtimeStateCards, out var value18))
		{
			_runtimeStateCards = value18.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._cooldownProgressSlider, out var value19))
		{
			_cooldownProgressSlider = value19.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._previewSunSpinBox, out var value20))
		{
			_previewSunSpinBox = value20.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._mobilePreviewCheck, out var value21))
		{
			_mobilePreviewCheck = value21.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._cardInteractionButton, out var value22))
		{
			_cardInteractionButton = value22.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pauseRuntimeButton, out var value23))
		{
			_pauseRuntimeButton = value23.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._cardFrame, out var value24))
		{
			_cardFrame = value24.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._cardPacketShowPreviewRoot, out var value25))
		{
			_cardPacketShowPreviewRoot = value25.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._cardPacketShowPreview, out var value26))
		{
			_cardPacketShowPreview = value26.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._cardPacketShowSpriteFingerprint, out var value27))
		{
			_cardPacketShowSpriteFingerprint = value27.As<string>();
		}
		if (info.TryGetProperty(PropertyName._cardPacketShowAnimationFingerprint, out var value28))
		{
			_cardPacketShowAnimationFingerprint = value28.As<string>();
		}
		if (info.TryGetProperty(PropertyName._cardPacketShowSaveKeyFingerprint, out var value29))
		{
			_cardPacketShowSaveKeyFingerprint = value29.As<string>();
		}
		if (info.TryGetProperty(PropertyName._cardPacketShowPreviewBuildCount, out var value30))
		{
			_cardPacketShowPreviewBuildCount = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cardPacketShowSpriteRebuildCount, out var value31))
		{
			_cardPacketShowSpriteRebuildCount = value31.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cardPreviewRefreshCommitted, out var value32))
		{
			_cardPreviewRefreshCommitted = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardCharacterViewportContainer, out var value33))
		{
			_cardCharacterViewportContainer = value33.As<SubViewportContainer>();
		}
		if (info.TryGetProperty(PropertyName._cardCharacterViewport, out var value34))
		{
			_cardCharacterViewport = value34.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._cardGameBackground, out var value35))
		{
			_cardGameBackground = value35.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._cardBattleTint, out var value36))
		{
			_cardBattleTint = value36.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName._cardCharacterPreviewRoot, out var value37))
		{
			_cardCharacterPreviewRoot = value37.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._cardBattleDropSurface, out var value38))
		{
			_cardBattleDropSurface = value38.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._cardDragTargetHighlight, out var value39))
		{
			_cardDragTargetHighlight = value39.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName._cardDragPacketGhost, out var value40))
		{
			_cardDragPacketGhost = value40.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._cardCharacterPreview, out var value41))
		{
			_cardCharacterPreview = value41.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._cardCharacterMissingLabel, out var value42))
		{
			_cardCharacterMissingLabel = value42.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._cardImageMissingLabel, out var value43))
		{
			_cardImageMissingLabel = value43.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._eventSummaryLabel, out var value44))
		{
			_eventSummaryLabel = value44.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._behaviorSummaryLabel, out var value45))
		{
			_behaviorSummaryLabel = value45.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._registryBehaviorStatusLabel, out var value46))
		{
			_registryBehaviorStatusLabel = value46.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._registryBehaviorList, out var value47))
		{
			_registryBehaviorList = value47.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._inlineBehaviorList, out var value48))
		{
			_inlineBehaviorList = value48.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._customBehaviorIdEdit, out var value49))
		{
			_customBehaviorIdEdit = value49.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._registryBehaviorPicker, out var value50))
		{
			_registryBehaviorPicker = value50.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._pressedActionsList, out var value51))
		{
			_pressedActionsList = value51.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._useSucceededActionsList, out var value52))
		{
			_useSucceededActionsList = value52.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._unlockConditionList, out var value53))
		{
			_unlockConditionList = value53.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._armorList, out var value54))
		{
			_armorList = value54.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._unlockConditionPicker, out var value55))
		{
			_unlockConditionPicker = value55.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._characterBindingButton, out var value56))
		{
			_characterBindingButton = value56.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._characterSourceLabel, out var value57))
		{
			_characterSourceLabel = value57.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._overrideStatus, out var value58))
		{
			_overrideStatus = value58.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._createOverrideButton, out var value59))
		{
			_createOverrideButton = value59.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._openOverrideButton, out var value60))
		{
			_openOverrideButton = value60.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._clearOverrideButton, out var value61))
		{
			_clearOverrideButton = value61.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._characterResourcePicker, out var value62))
		{
			_characterResourcePicker = value62.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._packetEventSelectorWindow, out var value63))
		{
			_packetEventSelectorWindow = value63.As<XWPacketEventSelectorWindow>();
		}
		if (info.TryGetProperty(PropertyName._packetEventTargetProperty, out var value64))
		{
			_packetEventTargetProperty = value64.As<string>();
		}
		if (info.TryGetProperty(PropertyName._behaviorRegistryRevision, out var value65))
		{
			_behaviorRegistryRevision = value65.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._behaviorRegistryPollAccumulator, out var value66))
		{
			_behaviorRegistryPollAccumulator = value66.As<double>();
		}
		if (info.TryGetProperty(PropertyName._selectedRegistryBehaviorIndex, out var value67))
		{
			_selectedRegistryBehaviorIndex = value67.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedInlineBehaviorIndex, out var value68))
		{
			_selectedInlineBehaviorIndex = value68.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedEventPressIndex, out var value69))
		{
			_selectedEventPressIndex = value69.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedEventPlantIndex, out var value70))
		{
			_selectedEventPlantIndex = value70.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedUnlockIndex, out var value71))
		{
			_selectedUnlockIndex = value71.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedArmorIndex, out var value72))
		{
			_selectedArmorIndex = value72.As<int>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value73))
		{
			_updatingControls = value73.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardRuntimePreviewState, out var value74))
		{
			_cardRuntimePreviewState = value74.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cardRuntimeCooldownProgress, out var value75))
		{
			_cardRuntimeCooldownProgress = value75.As<float>();
		}
		if (info.TryGetProperty(PropertyName._cardRuntimeCooldownDuration, out var value76))
		{
			_cardRuntimeCooldownDuration = value76.As<double>();
		}
		if (info.TryGetProperty(PropertyName._cardRuntimeCooldownRemaining, out var value77))
		{
			_cardRuntimeCooldownRemaining = value77.As<double>();
		}
		if (info.TryGetProperty(PropertyName._cardRuntimeCooldownRunning, out var value78))
		{
			_cardRuntimeCooldownRunning = value78.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardRuntimeHovered, out var value79))
		{
			_cardRuntimeHovered = value79.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardRuntimeMobile, out var value80))
		{
			_cardRuntimeMobile = value80.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardRuntimeReadoutAccumulator, out var value81))
		{
			_cardRuntimeReadoutAccumulator = value81.As<double>();
		}
		if (info.TryGetProperty(PropertyName._cardRuntimeReadoutRefreshCount, out var value82))
		{
			_cardRuntimeReadoutRefreshCount = value82.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cardCharacterPreviewConfig, out var value83))
		{
			_cardCharacterPreviewConfig = value83.As<TowerDefenseCharacterConfig>();
		}
		if (info.TryGetProperty(PropertyName._cardCharacterPreviewArmorFingerprint, out var value84))
		{
			_cardCharacterPreviewArmorFingerprint = value84.As<string>();
		}
		if (info.TryGetProperty(PropertyName._cardCharacterPreviewInitialized, out var value85))
		{
			_cardCharacterPreviewInitialized = value85.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardCharacterPreviewBuildCount, out var value86))
		{
			_cardCharacterPreviewBuildCount = value86.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cardDragCandidate, out var value87))
		{
			_cardDragCandidate = value87.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardDragActive, out var value88))
		{
			_cardDragActive = value88.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardDragTouch, out var value89))
		{
			_cardDragTouch = value89.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardDragTouchIndex, out var value90))
		{
			_cardDragTouchIndex = value90.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cardDragStartGlobalPosition, out var value91))
		{
			_cardDragStartGlobalPosition = value91.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._cardDragPointerGlobalPosition, out var value92))
		{
			_cardDragPointerGlobalPosition = value92.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._cardDragPreviewCell, out var value93))
		{
			_cardDragPreviewCell = value93.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._cardPlacedPreviewCell, out var value94))
		{
			_cardPlacedPreviewCell = value94.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._cardDragTargetValid, out var value95))
		{
			_cardDragTargetValid = value95.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardHasPlacedPreview, out var value96))
		{
			_cardHasPlacedPreview = value96.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cardDragPreviewPlacementCount, out var value97))
		{
			_cardDragPreviewPlacementCount = value97.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cardDragRejectedCount, out var value98))
		{
			_cardDragRejectedCount = value98.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cardDragCancelCount, out var value99))
		{
			_cardDragCancelCount = value99.As<int>();
		}
	}
}
