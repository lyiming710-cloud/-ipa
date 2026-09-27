using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.AdobeAnimateEditor.Inspector;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationVisualResourceEditor.cs")]
public class XWAnimationVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName OnCurrentResourceSaved = "OnCurrentResourceSaved";

		public static readonly StringName BindAnimationWorkbench = "BindAnimationWorkbench";

		public static readonly StringName UnbindAnimationWorkbench = "UnbindAnimationWorkbench";

		public static readonly StringName OnAnimationWorkbenchTabChanged = "OnAnimationWorkbenchTabChanged";

		public static readonly StringName OnAnimationEditorVisibilityChanged = "OnAnimationEditorVisibilityChanged";

		public static readonly StringName UpdateAnimationProcessEligibility = "UpdateAnimationProcessEligibility";

		public static readonly StringName MountAnimationResponsiveWorkbench = "MountAnimationResponsiveWorkbench";

		public static readonly StringName ConfigureAnimationWorkbenchSurface = "ConfigureAnimationWorkbenchSurface";

		public static readonly StringName SetAnimationSurfaceVisible = "SetAnimationSurfaceVisible";

		public static readonly StringName ClearAnimationResponsiveWorkbench = "ClearAnimationResponsiveWorkbench";

		public static readonly StringName DisposeAnimationPropertyBinding = "DisposeAnimationPropertyBinding";

		public static readonly StringName RenderAnimationPreview = "RenderAnimationPreview";

		public static readonly StringName ReleaseClosedAnimationResource = "ReleaseClosedAnimationResource";

		public static readonly StringName OnAnimationPreviewStateChanged = "OnAnimationPreviewStateChanged";

		public static readonly StringName BindAnimationClipSurface = "BindAnimationClipSurface";

		public static readonly StringName DisposeAnimationClipSurface = "DisposeAnimationClipSurface";

		public static readonly StringName RefreshAnimationClipSurface = "RefreshAnimationClipSurface";

		public static readonly StringName OnAnimationClipSelected = "OnAnimationClipSelected";

		public static readonly StringName SelectAnimationClip = "SelectAnimationClip";

		public static readonly StringName SetAnimationClipRangeControls = "SetAnimationClipRangeControls";

		public static readonly StringName BeginAnimationClipRangeEdit = "BeginAnimationClipRangeEdit";

		public static readonly StringName BeginAnimationClipExactRangeEdit = "BeginAnimationClipExactRangeEdit";

		public static readonly StringName PreviewAnimationClipExactRange = "PreviewAnimationClipExactRange";

		public static readonly StringName PreviewAnimationClipRange = "PreviewAnimationClipRange";

		public static readonly StringName CommitAnimationClipExactRangeEdit = "CommitAnimationClipExactRangeEdit";

		public static readonly StringName CommitAnimationClipRange = "CommitAnimationClipRange";

		public static readonly StringName CommitAnimationClipRangeEdit = "CommitAnimationClipRangeEdit";

		public static readonly StringName CancelAnimationClipRangeEdit = "CancelAnimationClipRangeEdit";

		public static readonly StringName AddAnimationClip = "AddAnimationClip";

		public static readonly StringName DeleteSelectedAnimationClip = "DeleteSelectedAnimationClip";

		public static readonly StringName ShiftSelectedAnimationClipEarlier = "ShiftSelectedAnimationClipEarlier";

		public static readonly StringName ShiftSelectedAnimationClipLater = "ShiftSelectedAnimationClipLater";

		public static readonly StringName ShiftSelectedAnimationClip = "ShiftSelectedAnimationClip";

		public static readonly StringName OnAnimationClipNameSubmitted = "OnAnimationClipNameSubmitted";

		public static readonly StringName OnAnimationClipNameFocusExited = "OnAnimationClipNameFocusExited";

		public static readonly StringName RenameSelectedAnimationClip = "RenameSelectedAnimationClip";

		public static readonly StringName CommitAnimationClipDictionaryChange = "CommitAnimationClipDictionaryChange";

		public static readonly StringName RefreshAnimationClipEditorFromHistory = "RefreshAnimationClipEditorFromHistory";

		public static readonly StringName RefreshAnimationTimelineClipRows = "RefreshAnimationTimelineClipRows";

		public static readonly StringName UpdateAnimationClipListItem = "UpdateAnimationClipListItem";

		public static readonly StringName SetAnimationClipStatus = "SetAnimationClipStatus";

		public static readonly StringName FormatAnimationClipListItem = "FormatAnimationClipListItem";

		public static readonly StringName ClampAnimationClipRange = "ClampAnimationClipRange";

		public static readonly StringName HasAnimationClip = "HasAnimationClip";

		public static readonly StringName BuildUniqueAnimationClipName = "BuildUniqueAnimationClipName";

		public static readonly StringName AnimationClipDictionaryContains = "AnimationClipDictionaryContains";

		public static readonly StringName CloneAnimationClips = "CloneAnimationClips";

		public static readonly StringName AnimationClipDictionariesEqual = "AnimationClipDictionariesEqual";

		public static readonly StringName BindAnimationFrameSurface = "BindAnimationFrameSurface";

		public static readonly StringName BindAnimationFrameNumberControl = "BindAnimationFrameNumberControl";

		public static readonly StringName DisposeAnimationFrameSurface = "DisposeAnimationFrameSurface";

		public static readonly StringName UnbindAnimationFrameNumberControl = "UnbindAnimationFrameNumberControl";

		public static readonly StringName PopulateAnimationFrameOptions = "PopulateAnimationFrameOptions";

		public static readonly StringName OnAnimationKeyframeSelected = "OnAnimationKeyframeSelected";

		public static readonly StringName RefreshSelectedAnimationFrameControls = "RefreshSelectedAnimationFrameControls";

		public static readonly StringName SetAnimationFrameControlsEnabled = "SetAnimationFrameControlsEnabled";

		public static readonly StringName BeginAnimationFramePropertyEdit = "BeginAnimationFramePropertyEdit";

		public static readonly StringName PreviewSelectedAnimationFrameSlice = "PreviewSelectedAnimationFrameSlice";

		public static readonly StringName CommitAnimationFrameAlphaEdit = "CommitAnimationFrameAlphaEdit";

		public static readonly StringName CommitAnimationFramePropertyEdit = "CommitAnimationFramePropertyEdit";

		public static readonly StringName CancelAnimationFramePropertyEdit = "CancelAnimationFramePropertyEdit";

		public static readonly StringName OnAnimationFrameMediaSelected = "OnAnimationFrameMediaSelected";

		public static readonly StringName OnAnimationFrameLayerSelected = "OnAnimationFrameLayerSelected";

		public static readonly StringName OnAnimationVisualMediaSelected = "OnAnimationVisualMediaSelected";

		public static readonly StringName OnAnimationVisualLayerSelected = "OnAnimationVisualLayerSelected";

		public static readonly StringName AddAnimationKeyframeAtPlayhead = "AddAnimationKeyframeAtPlayhead";

		public static readonly StringName DuplicateSelectedAnimationKeyframe = "DuplicateSelectedAnimationKeyframe";

		public static readonly StringName DeleteSelectedAnimationKeyframe = "DeleteSelectedAnimationKeyframe";

		public static readonly StringName MoveSelectedAnimationKeyframeEarlier = "MoveSelectedAnimationKeyframeEarlier";

		public static readonly StringName MoveSelectedAnimationKeyframeLater = "MoveSelectedAnimationKeyframeLater";

		public static readonly StringName MoveSelectedAnimationKeyframeBy = "MoveSelectedAnimationKeyframeBy";

		public static readonly StringName OnAnimationKeyframeMoveRequested = "OnAnimationKeyframeMoveRequested";

		public static readonly StringName CommitAnimationFrameSnapshotChange = "CommitAnimationFrameSnapshotChange";

		public static readonly StringName RefreshAnimationFrameEditorFromHistory = "RefreshAnimationFrameEditorFromHistory";

		public static readonly StringName TrySelectAnimationSliceByKey = "TrySelectAnimationSliceByKey";

		public static readonly StringName RefreshAnimationRuntimeAtFrame = "RefreshAnimationRuntimeAtFrame";

		public static readonly StringName UpdateAnimationMediaThumbnail = "UpdateAnimationMediaThumbnail";

		public static readonly StringName CreateAnimationMediaThumbnail = "CreateAnimationMediaThumbnail";

		public static readonly StringName SetAnimationFrameStatus = "SetAnimationFrameStatus";

		public static readonly StringName GetSelectedOptionMetadata = "GetSelectedOptionMetadata";

		public static readonly StringName SelectOptionByMetadata = "SelectOptionByMetadata";

		public static readonly StringName GetAnimationNameById = "GetAnimationNameById";

		public static readonly StringName BindAnimationDirectEditSurface = "BindAnimationDirectEditSurface";

		public static readonly StringName BindAnimationFrameScaleControl = "BindAnimationFrameScaleControl";

		public static readonly StringName UnbindAnimationFrameScaleControl = "UnbindAnimationFrameScaleControl";

		public static readonly StringName BeginAnimationFrameScaleEdit = "BeginAnimationFrameScaleEdit";

		public static readonly StringName PreviewAnimationFrameScaleEdit = "PreviewAnimationFrameScaleEdit";

		public static readonly StringName CommitAnimationFrameScaleEdit = "CommitAnimationFrameScaleEdit";

		public static readonly StringName RequestAnimationFrameScaleConfirmation = "RequestAnimationFrameScaleConfirmation";

		public static readonly StringName StartAnimationFrameScaleHydration = "StartAnimationFrameScaleHydration";

		public static readonly StringName SyncAnimationFrameScaleControl = "SyncAnimationFrameScaleControl";

		public static readonly StringName BindAnimationReplacementSurface = "BindAnimationReplacementSurface";

		public static readonly StringName DisposeAnimationReplacementSurface = "DisposeAnimationReplacementSurface";

		public static readonly StringName EnsureAnimationReplacementPicker = "EnsureAnimationReplacementPicker";

		public static readonly StringName OpenAnimationReplacementSlotPicker = "OpenAnimationReplacementSlotPicker";

		public static readonly StringName OpenAnimationExtraTexturePicker = "OpenAnimationExtraTexturePicker";

		public static readonly StringName SelectAnimationExtraTexture = "SelectAnimationExtraTexture";

		public static readonly StringName LoadAnimationReplacementTexture = "LoadAnimationReplacementTexture";

		public static readonly StringName AddAnimationReplacementSlot = "AddAnimationReplacementSlot";

		public static readonly StringName RemoveAnimationReplacementSlot = "RemoveAnimationReplacementSlot";

		public static readonly StringName AddAnimationExtraTexture = "AddAnimationExtraTexture";

		public static readonly StringName ReplaceAnimationExtraTexture = "ReplaceAnimationExtraTexture";

		public static readonly StringName AddAnimationExtraTexturePath = "AddAnimationExtraTexturePath";

		public static readonly StringName ReplaceAnimationExtraTexturePath = "ReplaceAnimationExtraTexturePath";

		public static readonly StringName RemoveAnimationExtraTexture = "RemoveAnimationExtraTexture";

		public static readonly StringName CommitAnimationExtraTexturePaths = "CommitAnimationExtraTexturePaths";

		public static readonly StringName DuplicateAnimationExtraTexturePaths = "DuplicateAnimationExtraTexturePaths";

		public static readonly StringName RefreshAnimationReplacementWorkbenchFromHistory = "RefreshAnimationReplacementWorkbenchFromHistory";

		public static readonly StringName RebuildAnimationReplacementWorkbench = "RebuildAnimationReplacementWorkbench";

		public static readonly StringName AddAnimationReplacementSlotCard = "AddAnimationReplacementSlotCard";

		public static readonly StringName AddAnimationExtraTextureCard = "AddAnimationExtraTextureCard";

		public static readonly StringName AddAnimationReplacementEmptyCard = "AddAnimationReplacementEmptyCard";

		public static readonly StringName InstantiateAnimationReplacementCard = "InstantiateAnimationReplacementCard";

		public static readonly StringName ClearDynamicAnimationCards = "ClearDynamicAnimationCards";

		public static readonly StringName FormatAnimationTextureName = "FormatAnimationTextureName";

		public static readonly StringName ContainsAnimationReplacementSlot = "ContainsAnimationReplacementSlot";

		public static readonly StringName BuildAnimationMediaLayerSummary = "BuildAnimationMediaLayerSummary";

		public static readonly StringName RequestAnimationReplacementReload = "RequestAnimationReplacementReload";

		public static readonly StringName ReloadAnimationReplacementConfiguration = "ReloadAnimationReplacementConfiguration";

		public static readonly StringName SetAnimationReplacementStatus = "SetAnimationReplacementStatus";

		public static readonly StringName OnAnimationVisualPropertyEdited = "OnAnimationVisualPropertyEdited";

		public static readonly StringName OpenAnimationDatFileDialog = "OpenAnimationDatFileDialog";

		public static readonly StringName SelectAnimationDatFile = "SelectAnimationDatFile";

		public static readonly StringName ToStoredAnimationDatPath = "ToStoredAnimationDatPath";

		public static readonly StringName ReloadAnimationFromDirectSettings = "ReloadAnimationFromDirectSettings";

		public static readonly StringName RequestExplicitAnimationReimport = "RequestExplicitAnimationReimport";

		public static readonly StringName CancelPendingAnimationReimportConfirmation = "CancelPendingAnimationReimportConfirmation";

		public static readonly StringName ConfirmExplicitAnimationReimport = "ConfirmExplicitAnimationReimport";

		public static readonly StringName CancelExplicitAnimationReimport = "CancelExplicitAnimationReimport";

		public static readonly StringName RefreshRuntimeAnimationPreview = "RefreshRuntimeAnimationPreview";

		public static readonly StringName UpdateAnimationDirectSummary = "UpdateAnimationDirectSummary";

		public static readonly StringName FitRuntimePreviewAfterLayout = "FitRuntimePreviewAfterLayout";

		public static readonly StringName ClampRuntimePreviewViewportMinimum = "ClampRuntimePreviewViewportMinimum";

		public static readonly StringName StartAnimationResourceHydration = "StartAnimationResourceHydration";

		public static readonly StringName PollAnimationResourceHydration = "PollAnimationResourceHydration";

		public static readonly StringName AbortExplicitAnimationReimport = "AbortExplicitAnimationReimport";

		public static readonly StringName CancelAnimationResourceHydration = "CancelAnimationResourceHydration";

		public static readonly StringName GetAnimationFrameCount = "GetAnimationFrameCount";

		public static readonly StringName RenderAnimationTimeline = "RenderAnimationTimeline";

		public static readonly StringName GetAnimationLayerNames = "GetAnimationLayerNames";

		public static readonly StringName GetAnimationMediaNames = "GetAnimationMediaNames";

		public static readonly StringName RenderAnimationGraph = "RenderAnimationGraph";

		public static readonly StringName RenderAnimationReferences = "RenderAnimationReferences";

		public static readonly StringName ResolveCompanionDatPath = "ResolveCompanionDatPath";

		public static readonly StringName AddTimelineRow = "AddTimelineRow";

		public static readonly StringName AddPreviewRow = "AddPreviewRow";

		public static readonly StringName AddGraphRow = "AddGraphRow";

		public static readonly StringName AddReferenceRow = "AddReferenceRow";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";

		public static readonly StringName CountEventFrames = "CountEventFrames";

		public static readonly StringName TryGetSharedAnimationAtlasSize = "TryGetSharedAnimationAtlasSize";

		public static readonly StringName TryGetTextureSize = "TryGetTextureSize";

		public static readonly StringName FormatVariant = "FormatVariant";

		public static readonly StringName NormalizePath = "NormalizePath";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName AnimationStateUpdateCount = "AnimationStateUpdateCount";

		public static readonly StringName AnimationClipMutationCount = "AnimationClipMutationCount";

		public static readonly StringName AnimationFrameMutationCount = "AnimationFrameMutationCount";

		public static readonly StringName AnimationReplacementMutationCount = "AnimationReplacementMutationCount";

		public static readonly StringName AnimationReplacementSlotCardCount = "AnimationReplacementSlotCardCount";

		public static readonly StringName AnimationExtraTextureCardCount = "AnimationExtraTextureCardCount";

		public static readonly StringName SelectedAnimationClipName = "SelectedAnimationClipName";

		public static readonly StringName PackedFrameEditingSupported = "PackedFrameEditingSupported";

		public static readonly StringName SelectedAnimationFrame = "SelectedAnimationFrame";

		public static readonly StringName SelectedAnimationSliceKey = "SelectedAnimationSliceKey";

		public static readonly StringName AnimationWorkbenchCurrentTab = "AnimationWorkbenchCurrentTab";

		public static readonly StringName AnimationMediaVisualChoiceCount = "AnimationMediaVisualChoiceCount";

		public static readonly StringName AnimationLayerVisualChoiceCount = "AnimationLayerVisualChoiceCount";

		public static readonly StringName AnimationHighFrequencyActive = "AnimationHighFrequencyActive";

		public static readonly StringName AnimationHydrationPending = "AnimationHydrationPending";

		public static readonly StringName PendingAnimationFrameScale = "PendingAnimationFrameScale";

		public static readonly StringName RequestedAnimationHydrationFrameScale = "RequestedAnimationHydrationFrameScale";

		public static readonly StringName LastAnimationSavePersistedTicks = "LastAnimationSavePersistedTicks";

		public static readonly StringName _animationPreviewStatusLabel = "_animationPreviewStatusLabel";

		public static readonly StringName _animationDirectSummaryLabel = "_animationDirectSummaryLabel";

		public static readonly StringName _animationResourcePathLabel = "_animationResourcePathLabel";

		public static readonly StringName _animationRuntimeFrameLabel = "_animationRuntimeFrameLabel";

		public static readonly StringName _animationPlaybackStateLabel = "_animationPlaybackStateLabel";

		public static readonly StringName _animationClipList = "_animationClipList";

		public static readonly StringName _animationClipNameEdit = "_animationClipNameEdit";

		public static readonly StringName _animationClipStartSpinBox = "_animationClipStartSpinBox";

		public static readonly StringName _animationClipEndSpinBox = "_animationClipEndSpinBox";

		public static readonly StringName _animationClipRangeControl = "_animationClipRangeControl";

		public static readonly StringName _animationAddClipButton = "_animationAddClipButton";

		public static readonly StringName _animationDeleteClipButton = "_animationDeleteClipButton";

		public static readonly StringName _animationShiftClipEarlierButton = "_animationShiftClipEarlierButton";

		public static readonly StringName _animationShiftClipLaterButton = "_animationShiftClipLaterButton";

		public static readonly StringName _animationClipEditStatusLabel = "_animationClipEditStatusLabel";

		public static readonly StringName _animationFrameTimeline = "_animationFrameTimeline";

		public static readonly StringName _animationAddKeyframeButton = "_animationAddKeyframeButton";

		public static readonly StringName _animationDuplicateKeyframeButton = "_animationDuplicateKeyframeButton";

		public static readonly StringName _animationDeleteKeyframeButton = "_animationDeleteKeyframeButton";

		public static readonly StringName _animationMoveKeyframeEarlierButton = "_animationMoveKeyframeEarlierButton";

		public static readonly StringName _animationMoveKeyframeLaterButton = "_animationMoveKeyframeLaterButton";

		public static readonly StringName _animationSelectedKeyframeLabel = "_animationSelectedKeyframeLabel";

		public static readonly StringName _animationMediaOption = "_animationMediaOption";

		public static readonly StringName _animationLayerOption = "_animationLayerOption";

		public static readonly StringName _animationMediaThumbnail = "_animationMediaThumbnail";

		public static readonly StringName _animationPositionXSpinBox = "_animationPositionXSpinBox";

		public static readonly StringName _animationPositionYSpinBox = "_animationPositionYSpinBox";

		public static readonly StringName _animationRotationSpinBox = "_animationRotationSpinBox";

		public static readonly StringName _animationSkewSpinBox = "_animationSkewSpinBox";

		public static readonly StringName _animationScaleXSpinBox = "_animationScaleXSpinBox";

		public static readonly StringName _animationScaleYSpinBox = "_animationScaleYSpinBox";

		public static readonly StringName _animationAlphaSlider = "_animationAlphaSlider";

		public static readonly StringName _animationAlphaPercentLabel = "_animationAlphaPercentLabel";

		public static readonly StringName _animationKeyframeEditStatusLabel = "_animationKeyframeEditStatusLabel";

		public static readonly StringName _updatingAnimationFrameControls = "_updatingAnimationFrameControls";

		public static readonly StringName _animationFrameEditSnapshot = "_animationFrameEditSnapshot";

		public static readonly StringName _animationFrameEditTarget = "_animationFrameEditTarget";

		public static readonly StringName _animationFrameEditFrame = "_animationFrameEditFrame";

		public static readonly StringName _animationFrameEditSliceKey = "_animationFrameEditSliceKey";

		public static readonly StringName _animationFrameEditMutated = "_animationFrameEditMutated";

		public static readonly StringName _animationFrameMutationCount = "_animationFrameMutationCount";

		public static readonly StringName _animationDatFileButton = "_animationDatFileButton";

		public static readonly StringName _animationDatFileDialog = "_animationDatFileDialog";

		public static readonly StringName _animationReimportConfirmDialog = "_animationReimportConfirmDialog";

		public static readonly StringName _animationFrameScaleSpinBox = "_animationFrameScaleSpinBox";

		public static readonly StringName _updatingAnimationFrameScaleControl = "_updatingAnimationFrameScaleControl";

		public static readonly StringName _pendingAnimationReimportTarget = "_pendingAnimationReimportTarget";

		public static readonly StringName _pendingAnimationReimportPath = "_pendingAnimationReimportPath";

		public static readonly StringName _pendingAnimationFrameScale = "_pendingAnimationFrameScale";

		public static readonly StringName _animationExplicitReimportOldSnapshot = "_animationExplicitReimportOldSnapshot";

		public static readonly StringName _animationExplicitReimportFrame = "_animationExplicitReimportFrame";

		public static readonly StringName _animationExplicitReimportSliceKey = "_animationExplicitReimportSliceKey";

		public static readonly StringName _animationExplicitReimportActionName = "_animationExplicitReimportActionName";

		public static readonly StringName _animationExplicitReimportSuccessMessage = "_animationExplicitReimportSuccessMessage";

		public static readonly StringName _animationExplicitReimportRequiresRestore = "_animationExplicitReimportRequiresRestore";

		public static readonly StringName _runtimePreview = "_runtimePreview";

		public static readonly StringName _animationDatSelectionTarget = "_animationDatSelectionTarget";

		public static readonly StringName _animationHydrationPath = "_animationHydrationPath";

		public static readonly StringName _animationHydrationTarget = "_animationHydrationTarget";

		public static readonly StringName _animationHydrationRequestedFrameScale = "_animationHydrationRequestedFrameScale";

		public static readonly StringName _animationHydrationExpectedFrameScale = "_animationHydrationExpectedFrameScale";

		public static readonly StringName _animationHydrationExpectedAuthoringRevision = "_animationHydrationExpectedAuthoringRevision";

		public static readonly StringName _animationStateUpdateCount = "_animationStateUpdateCount";

		public static readonly StringName _selectedAnimationClipName = "_selectedAnimationClipName";

		public static readonly StringName _updatingAnimationClipControls = "_updatingAnimationClipControls";

		public static readonly StringName _animationClipEditSnapshot = "_animationClipEditSnapshot";

		public static readonly StringName _animationClipEditName = "_animationClipEditName";

		public static readonly StringName _animationClipEditTarget = "_animationClipEditTarget";

		public static readonly StringName _animationClipMutationCount = "_animationClipMutationCount";

		public static readonly StringName _editingAnimation = "_editingAnimation";

		public static readonly StringName _animationReplacementSlotCards = "_animationReplacementSlotCards";

		public static readonly StringName _animationExtraTextureCards = "_animationExtraTextureCards";

		public static readonly StringName _animationReplacementSummaryLabel = "_animationReplacementSummaryLabel";

		public static readonly StringName _animationReplacementStatusLabel = "_animationReplacementStatusLabel";

		public static readonly StringName _animationAddReplacementSlotButton = "_animationAddReplacementSlotButton";

		public static readonly StringName _animationAddExtraTextureButton = "_animationAddExtraTextureButton";

		public static readonly StringName _animationReplacementSaveButton = "_animationReplacementSaveButton";

		public static readonly StringName _animationReplacementReloadButton = "_animationReplacementReloadButton";

		public static readonly StringName _animationReplacementReloadConfirmDialog = "_animationReplacementReloadConfirmDialog";

		public static readonly StringName _animationReplacementPicker = "_animationReplacementPicker";

		public static readonly StringName _animationReplacementMutationCount = "_animationReplacementMutationCount";

		public static readonly StringName _animationReplacementSlotCardCount = "_animationReplacementSlotCardCount";

		public static readonly StringName _animationExtraTextureCardCount = "_animationExtraTextureCardCount";

		public static readonly StringName _animationWorkbench = "_animationWorkbench";

		public static readonly StringName _animationWorkbenchTabs = "_animationWorkbenchTabs";

		public static readonly StringName _animationChoiceSurface = "_animationChoiceSurface";

		public static readonly StringName _animationSourceHost = "_animationSourceHost";

		public static readonly StringName _animationSourceContainer = "_animationSourceContainer";

		public static readonly StringName _animationMediaChoiceGrid = "_animationMediaChoiceGrid";

		public static readonly StringName _animationLayerChoiceGrid = "_animationLayerChoiceGrid";

		public static readonly StringName _animationWorkbenchSignalsConnected = "_animationWorkbenchSignalsConnected";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string AnimationRuntimePreviewScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationRuntimePreview.tscn";

	private const string AnimationReplacementCardScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationReplacementCard.tscn";

	private static PackedScene _animationRuntimePreviewScene;

	private static PackedScene _animationReplacementCardScene;

	private static Texture2D _animationVisualFallbackIcon;

	private Label _animationPreviewStatusLabel;

	private Label _animationDirectSummaryLabel;

	private Label _animationResourcePathLabel;

	private Label _animationRuntimeFrameLabel;

	private Label _animationPlaybackStateLabel;

	private ItemList _animationClipList;

	private LineEdit _animationClipNameEdit;

	private SpinBox _animationClipStartSpinBox;

	private SpinBox _animationClipEndSpinBox;

	private XWPacketSpawnEntryRangeControl _animationClipRangeControl;

	private Button _animationAddClipButton;

	private Button _animationDeleteClipButton;

	private Button _animationShiftClipEarlierButton;

	private Button _animationShiftClipLaterButton;

	private Label _animationClipEditStatusLabel;

	private XWAnimationFrameTimeline _animationFrameTimeline;

	private Button _animationAddKeyframeButton;

	private Button _animationDuplicateKeyframeButton;

	private Button _animationDeleteKeyframeButton;

	private Button _animationMoveKeyframeEarlierButton;

	private Button _animationMoveKeyframeLaterButton;

	private Label _animationSelectedKeyframeLabel;

	private OptionButton _animationMediaOption;

	private OptionButton _animationLayerOption;

	private TextureRect _animationMediaThumbnail;

	private SpinBox _animationPositionXSpinBox;

	private SpinBox _animationPositionYSpinBox;

	private SpinBox _animationRotationSpinBox;

	private SpinBox _animationSkewSpinBox;

	private SpinBox _animationScaleXSpinBox;

	private SpinBox _animationScaleYSpinBox;

	private HSlider _animationAlphaSlider;

	private Label _animationAlphaPercentLabel;

	private Label _animationKeyframeEditStatusLabel;

	private bool _updatingAnimationFrameControls;

	private Dictionary _animationFrameEditSnapshot;

	private AdobeAnimateData _animationFrameEditTarget;

	private int _animationFrameEditFrame = -1;

	private int _animationFrameEditSliceKey = -1;

	private bool _animationFrameEditMutated;

	private int _animationFrameMutationCount;

	private Button _animationDatFileButton;

	private FileDialog _animationDatFileDialog;

	private ConfirmationDialog _animationReimportConfirmDialog;

	private SpinBox _animationFrameScaleSpinBox;

	private bool _updatingAnimationFrameScaleControl;

	private AdobeAnimateData _pendingAnimationReimportTarget;

	private string _pendingAnimationReimportPath = "";

	private int _pendingAnimationFrameScale = -1;

	private Dictionary _animationExplicitReimportOldSnapshot;

	private int _animationExplicitReimportFrame = -1;

	private int _animationExplicitReimportSliceKey = -1;

	private string _animationExplicitReimportActionName = "重新导入动画 DAT";

	private string _animationExplicitReimportSuccessMessage = "DAT 已在后台重新导入，可使用全局 Undo 无损恢复手工帧。";

	private bool _animationExplicitReimportRequiresRestore;

	private XWVisualPropertyBinding _animationPropertyBinding;

	private AdobeAnimateInspectorPreview _runtimePreview;

	private AdobeAnimateData _animationDatSelectionTarget;

	private string _animationHydrationPath = "";

	private AdobeAnimateData _animationHydrationTarget;

	private CancellationTokenSource _animationHydrationCancellation;

	private Task<AdobeAnimateData.EditorDatHydrationResult> _animationHydrationTask;

	private int _animationHydrationRequestedFrameScale = -1;

	private int _animationHydrationExpectedFrameScale = -1;

	private long _animationHydrationExpectedAuthoringRevision = -1L;

	private int _animationStateUpdateCount;

	private string _selectedAnimationClipName = "";

	private bool _updatingAnimationClipControls;

	private Dictionary _animationClipEditSnapshot;

	private string _animationClipEditName = "";

	private AdobeAnimateData _animationClipEditTarget;

	private int _animationClipMutationCount;

	private AdobeAnimateData _editingAnimation;

	private HFlowContainer _animationReplacementSlotCards;

	private HFlowContainer _animationExtraTextureCards;

	private Label _animationReplacementSummaryLabel;

	private Label _animationReplacementStatusLabel;

	private Button _animationAddReplacementSlotButton;

	private Button _animationAddExtraTextureButton;

	private Button _animationReplacementSaveButton;

	private Button _animationReplacementReloadButton;

	private ConfirmationDialog _animationReplacementReloadConfirmDialog;

	private XWGameplayResourcePickerWindow _animationReplacementPicker;

	private int _animationReplacementMutationCount;

	private int _animationReplacementSlotCardCount;

	private int _animationExtraTextureCardCount;

	private Control _animationWorkbench;

	private TabBar _animationWorkbenchTabs;

	private VBoxContainer _animationChoiceSurface;

	private VBoxContainer _animationSourceHost;

	private VBoxContainer _animationSourceContainer;

	private XWAnimationVisualChoiceGrid _animationMediaChoiceGrid;

	private XWAnimationVisualChoiceGrid _animationLayerChoiceGrid;

	private bool _animationWorkbenchSignalsConnected;

	public int AnimationStateUpdateCount => _animationStateUpdateCount;

	public int AnimationClipMutationCount => _animationClipMutationCount;

	public int AnimationFrameMutationCount => _animationFrameMutationCount;

	public int AnimationReplacementMutationCount => _animationReplacementMutationCount;

	public int AnimationReplacementSlotCardCount => _animationReplacementSlotCardCount;

	public int AnimationExtraTextureCardCount => _animationExtraTextureCardCount;

	public string SelectedAnimationClipName => _selectedAnimationClipName;

	public bool PackedFrameEditingSupported => true;

	public int SelectedAnimationFrame
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_animationFrameTimeline))
			{
				return -1;
			}
			return _animationFrameTimeline.SelectedFrame;
		}
	}

	public int SelectedAnimationSliceKey
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_animationFrameTimeline))
			{
				return -1;
			}
			return _animationFrameTimeline.SelectedSliceKey;
		}
	}

	public int AnimationWorkbenchCurrentTab
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_animationWorkbenchTabs))
			{
				return -1;
			}
			return _animationWorkbenchTabs.CurrentTab;
		}
	}

	public int AnimationMediaVisualChoiceCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_animationMediaChoiceGrid))
			{
				return 0;
			}
			return _animationMediaChoiceGrid.ChoiceCount;
		}
	}

	public int AnimationLayerVisualChoiceCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_animationLayerChoiceGrid))
			{
				return 0;
			}
			return _animationLayerChoiceGrid.ChoiceCount;
		}
	}

	public bool AnimationHighFrequencyActive
	{
		get
		{
			if (IsVisibleInTree() && GodotObject.IsInstanceValid(_runtimePreview))
			{
				return _runtimePreview.IsVisibleInTree();
			}
			return false;
		}
	}

	public bool AnimationHydrationPending
	{
		get
		{
			if (_animationHydrationTask != null)
			{
				return !_animationHydrationTask.IsCompleted;
			}
			return false;
		}
	}

	public int PendingAnimationFrameScale => _pendingAnimationFrameScale;

	public int RequestedAnimationHydrationFrameScale => _animationHydrationRequestedFrameScale;

	public ulong LastAnimationSavePersistedTicks { get; private set; }

	public override void _Ready()
	{
		SetProcess(enable: false);
		base._Ready();
		BindAnimationWorkbench();
	}

	public override void _Process(double delta)
	{
		if (IsVisibleInTree())
		{
			base._Process(delta);
			PollAnimationResourceHydration();
		}
	}

	public override void _ExitTree()
	{
		ReleaseClosedAnimationResource();
		UnbindAnimationWorkbench();
		base._ExitTree();
	}

	protected override void OnCurrentResourceSaved()
	{
		LastAnimationSavePersistedTicks = Time.GetTicksMsec();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		if (!(CurrentResource is AdobeAnimateData animation))
		{
			ReleaseClosedAnimationResource();
			return;
		}
		RenderAnimationPreview(animation);
		RenderAnimationTimeline(animation);
		RenderAnimationGraph(animation);
		RenderAnimationReferences(animation);
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!(resource is AdobeAnimateData))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	protected override bool ShouldBindDirectPropertySurface(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!(resource is AdobeAnimateData))
		{
			return base.ShouldBindDirectPropertySurface(resource, path, descriptor);
		}
		return false;
	}

	private void BindAnimationWorkbench()
	{
		_animationWorkbench = GetNodeOrNull<Control>("%AnimationWorkbench");
		_animationWorkbenchTabs = GetNodeOrNull<TabBar>("%AnimationWorkbenchTabs");
		_animationChoiceSurface = GetNodeOrNull<VBoxContainer>("%AnimationChoiceSurface");
		_animationSourceHost = GetNodeOrNull<VBoxContainer>("%AnimationSourceHost");
		_animationMediaChoiceGrid = GetNodeOrNull<XWAnimationVisualChoiceGrid>("%MediaChoiceGrid");
		_animationLayerChoiceGrid = GetNodeOrNull<XWAnimationVisualChoiceGrid>("%LayerChoiceGrid");
		if (GodotObject.IsInstanceValid(_animationWorkbenchTabs))
		{
			_animationWorkbenchTabs.TabChanged += OnAnimationWorkbenchTabChanged;
		}
		if (GodotObject.IsInstanceValid(_animationMediaChoiceGrid))
		{
			_animationMediaChoiceGrid.ChoiceSelected += OnAnimationVisualMediaSelected;
		}
		if (GodotObject.IsInstanceValid(_animationLayerChoiceGrid))
		{
			_animationLayerChoiceGrid.ChoiceSelected += OnAnimationVisualLayerSelected;
		}
		VisibilityChanged += OnAnimationEditorVisibilityChanged;
		_animationWorkbenchSignalsConnected = true;
	}

	private void UnbindAnimationWorkbench()
	{
		if (_animationWorkbenchSignalsConnected)
		{
			if (GodotObject.IsInstanceValid(_animationWorkbenchTabs))
			{
				_animationWorkbenchTabs.TabChanged -= OnAnimationWorkbenchTabChanged;
			}
			if (GodotObject.IsInstanceValid(_animationMediaChoiceGrid))
			{
				_animationMediaChoiceGrid.ChoiceSelected -= OnAnimationVisualMediaSelected;
			}
			if (GodotObject.IsInstanceValid(_animationLayerChoiceGrid))
			{
				_animationLayerChoiceGrid.ChoiceSelected -= OnAnimationVisualLayerSelected;
			}
			VisibilityChanged -= OnAnimationEditorVisibilityChanged;
			_animationWorkbenchSignalsConnected = false;
		}
	}

	private void OnAnimationWorkbenchTabChanged(long tab)
	{
		ConfigureAnimationWorkbenchSurface((int)tab);
		if (tab == 0L && GodotObject.IsInstanceValid(_runtimePreview))
		{
			FitRuntimePreviewAfterLayout(_runtimePreview);
		}
		UpdateAnimationProcessEligibility();
	}

	private void OnAnimationEditorVisibilityChanged()
	{
		if (!IsVisibleInTree())
		{
			if (GodotObject.IsInstanceValid(_pendingAnimationReimportTarget))
			{
				_animationReimportConfirmDialog?.Hide();
				CancelPendingAnimationReimportConfirmation();
			}
			CancelExplicitAnimationReimport(restoreOldSnapshot: true);
			CancelAnimationResourceHydration();
			UpdateAnimationProcessEligibility();
		}
		else
		{
			if (CurrentResource is AdobeAnimateData adobeAnimateData && !adobeAnimateData.HasPackedRuntimeData() && !AnimationHydrationPending)
			{
				StartAnimationResourceHydration();
			}
			UpdateAnimationProcessEligibility();
			if (GodotObject.IsInstanceValid(_animationWorkbenchTabs) && _animationWorkbenchTabs.CurrentTab == 0 && GodotObject.IsInstanceValid(_runtimePreview))
			{
				FitRuntimePreviewAfterLayout(_runtimePreview);
			}
		}
	}

	private void UpdateAnimationProcessEligibility()
	{
		bool flag = _animationHydrationTask != null;
		SetProcess(IsVisibleInTree() & flag);
	}

	private void MountAnimationResponsiveWorkbench(VBoxContainer source)
	{
		if (!GodotObject.IsInstanceValid(source) || !GodotObject.IsInstanceValid(_animationWorkbench) || !GodotObject.IsInstanceValid(_animationSourceHost))
		{
			return;
		}
		_animationWorkbench.Visible = true;
		if (GodotObject.IsInstanceValid(_animationWorkbenchTabs))
		{
			_animationWorkbenchTabs.CurrentTab = 0;
		}
		Control nodeOrNull = source.GetNodeOrNull<Control>("StageLayout/DirectSettingsPanel");
		Control nodeOrNull2 = source.GetNodeOrNull<Control>("FrameTimelinePanel/FrameTimelineMargin/FrameTimelineStack/FrameTimelineBody/KeyframePropertyPanel");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			nodeOrNull.Owner = null;
			nodeOrNull.Reparent(source, keepGlobalTransform: false);
			nodeOrNull.Owner = source;
		}
		if (GodotObject.IsInstanceValid(nodeOrNull2))
		{
			nodeOrNull2.Owner = null;
			nodeOrNull2.Reparent(source, keepGlobalTransform: false);
			nodeOrNull2.Owner = source;
		}
		_animationSourceHost.AddChild(source, forceReadableName: false, InternalMode.Disabled);
		_animationSourceContainer = source;
		source.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		SubViewportContainer nodeOrNull3 = source.GetNodeOrNull<SubViewportContainer>("%PreviewViewportContainer");
		if (GodotObject.IsInstanceValid(nodeOrNull3))
		{
			nodeOrNull3.Stretch = true;
			nodeOrNull3.StretchShrink = 2;
			nodeOrNull3.CustomMinimumSize = new Vector2(240f, 190f);
			SubViewport nodeOrNull4 = source.GetNodeOrNull<SubViewport>("%PreviewViewport");
			if (GodotObject.IsInstanceValid(nodeOrNull4))
			{
				nodeOrNull4.Size = new Vector2I(640, 360);
			}
		}
		Control nodeOrNull5 = source.GetNodeOrNull<Control>("FrameTimelinePanel/FrameTimelineMargin/FrameTimelineStack/FrameTimelineBody/FrameTimelineScroll");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			nodeOrNull.CustomMinimumSize = Vector2.Zero;
		}
		if (GodotObject.IsInstanceValid(nodeOrNull5))
		{
			nodeOrNull5.CustomMinimumSize = new Vector2(0f, 330f);
		}
		if (GodotObject.IsInstanceValid(nodeOrNull2))
		{
			nodeOrNull2.CustomMinimumSize = Vector2.Zero;
		}
		HSplitContainer nodeOrNull6 = source.GetNodeOrNull<HSplitContainer>("ClipEditorPanel/ClipEditorMargin/ClipEditor/Body");
		Control nodeOrNull7 = source.GetNodeOrNull<Control>("ClipEditorPanel/ClipEditorMargin/ClipEditor/Body/ClipList");
		Control nodeOrNull8 = source.GetNodeOrNull<Control>("ClipEditorPanel/ClipEditorMargin/ClipEditor/Body/Edit/ClipRangeControl");
		Control nodeOrNull9 = source.GetNodeOrNull<Control>("ClipEditorPanel/ClipEditorMargin/ClipEditor/Body/Edit/NameRow/ClipNameEdit");
		Control nodeOrNull10 = source.GetNodeOrNull<Control>("ClipEditorPanel/ClipEditorMargin/ClipEditor/Body/Edit/NameRow/ShiftEarlierButton");
		Control nodeOrNull11 = source.GetNodeOrNull<Control>("ClipEditorPanel/ClipEditorMargin/ClipEditor/Body/Edit/NameRow/ShiftLaterButton");
		if (GodotObject.IsInstanceValid(nodeOrNull6))
		{
			nodeOrNull6.SplitOffsets = new int[1] { 210 };
		}
		if (GodotObject.IsInstanceValid(nodeOrNull7))
		{
			nodeOrNull7.CustomMinimumSize = new Vector2(180f, 190f);
		}
		if (GodotObject.IsInstanceValid(nodeOrNull8))
		{
			nodeOrNull8.CustomMinimumSize = new Vector2(260f, 92f);
		}
		if (GodotObject.IsInstanceValid(nodeOrNull9))
		{
			nodeOrNull9.CustomMinimumSize = new Vector2(110f, 0f);
		}
		if (GodotObject.IsInstanceValid(nodeOrNull10))
		{
			nodeOrNull10.CustomMinimumSize = new Vector2(64f, 34f);
		}
		if (GodotObject.IsInstanceValid(nodeOrNull11))
		{
			nodeOrNull11.CustomMinimumSize = new Vector2(64f, 34f);
		}
		if (GodotObject.IsInstanceValid(_animationMediaOption))
		{
			_animationMediaOption.Visible = false;
		}
		if (GodotObject.IsInstanceValid(_animationLayerOption))
		{
			_animationLayerOption.Visible = false;
		}
		ConfigureAnimationWorkbenchSurface(0);
		UpdateAnimationProcessEligibility();
	}

	private void ConfigureAnimationWorkbenchSurface(int tab)
	{
		if (GodotObject.IsInstanceValid(_animationSourceContainer))
		{
			bool flag = tab == 0;
			bool flag2 = tab == 1;
			bool visible = tab == 2;
			bool visible2 = tab == 3;
			SetAnimationSurfaceVisible("TitleRow", visible: true);
			SetAnimationSurfaceVisible("StageLayout", flag | flag2);
			SetAnimationSurfaceVisible("StageLayout/PreviewHost", flag);
			SetAnimationSurfaceVisible("DirectSettingsPanel", flag2);
			SetAnimationSurfaceVisible("AtlasLabel", flag);
			SetAnimationSurfaceVisible("ReplacementLibraryPanel", tab == 5);
			SetAnimationSurfaceVisible("ClipEditorPanel", tab == 4);
			SetAnimationSurfaceVisible("FrameTimelinePanel", visible);
			SetAnimationSurfaceVisible("FrameTimelinePanel/FrameTimelineMargin/FrameTimelineStack/FrameTimelineHeader", visible);
			SetAnimationSurfaceVisible("FrameTimelinePanel/FrameTimelineMargin/FrameTimelineStack/FrameTimelineBody/FrameTimelineScroll", visible);
			SetAnimationSurfaceVisible("KeyframePropertyPanel", visible2);
			if (GodotObject.IsInstanceValid(_animationChoiceSurface))
			{
				_animationChoiceSurface.Visible = visible2;
			}
		}
	}

	private void SetAnimationSurfaceVisible(string path, bool visible)
	{
		Control control = _animationSourceContainer?.GetNodeOrNull<Control>(path);
		if (GodotObject.IsInstanceValid(control))
		{
			control.Visible = visible;
		}
	}

	private void ClearAnimationResponsiveWorkbench()
	{
		if (GodotObject.IsInstanceValid(_animationSourceContainer))
		{
			Node parent = _animationSourceContainer.GetParent();
			if (GodotObject.IsInstanceValid(parent))
			{
				parent.RemoveChild(_animationSourceContainer);
			}
			_animationSourceContainer.QueueFree();
		}
		_animationSourceContainer = null;
		_animationMediaChoiceGrid?.ClearChoices();
		_animationLayerChoiceGrid?.ClearChoices();
		if (GodotObject.IsInstanceValid(_animationChoiceSurface))
		{
			_animationChoiceSurface.Visible = false;
		}
		if (GodotObject.IsInstanceValid(_animationWorkbench))
		{
			_animationWorkbench.Visible = false;
		}
	}

	private void DisposeAnimationPropertyBinding()
	{
		CancelExplicitAnimationReimport(restoreOldSnapshot: true);
		UnbindAnimationFrameScaleControl();
		if (GodotObject.IsInstanceValid(_animationFrameTimeline))
		{
			_animationFrameTimeline.BindAnimation(null);
		}
		if (GodotObject.IsInstanceValid(_runtimePreview))
		{
			_runtimePreview.ClearPreview();
		}
		DisposeAnimationFrameSurface();
		DisposeAnimationClipSurface();
		DisposeAnimationReplacementSurface();
		if (GodotObject.IsInstanceValid(_runtimePreview))
		{
			_runtimePreview.PreviewStateChanged -= OnAnimationPreviewStateChanged;
		}
		_animationPropertyBinding?.Dispose();
		_animationPropertyBinding = null;
		ClearAnimationResponsiveWorkbench();
		_animationDatSelectionTarget = null;
		_runtimePreview = null;
		_pendingAnimationReimportTarget = null;
		_pendingAnimationReimportPath = "";
		_pendingAnimationFrameScale = -1;
		_animationReimportConfirmDialog = null;
	}

	private void RenderAnimationPreview(AdobeAnimateData animation)
	{
		AdobeAnimateData editingAnimation = _editingAnimation;
		DisposeAnimationPropertyBinding();
		if (GodotObject.IsInstanceValid(editingAnimation) && editingAnimation != animation)
		{
			AdobeAnimateDefinitionCache.Invalidate(editingAnimation);
		}
		_editingAnimation = animation;
		AddPreviewRow("动画文件: " + animation.animeFile);
		AddPreviewRow($"帧率: {animation.frameRate:0.##} fps");
		AddPreviewRow($"总帧数: {animation.frameMax}");
		AddPreviewRow($"图层数: {animation.layerDictionary?.Count ?? 0}");
		AddPreviewRow($"Clip 数: {animation.clips?.Count ?? 0}");
		AddPreviewRow($"事件帧数: {CountEventFrames(animation)}");
		bool flag = animation.HasPackedRuntimeData();
		bool flag2 = flag || !string.IsNullOrWhiteSpace(ResolveCompanionDatPath(animation));
		XWAnimationVisualResourceEditor xWAnimationVisualResourceEditor = this;
		string text;
		if (flag)
		{
			text = "已读取";
		}
		else
		{
			text = (flag2 ? "后台加载中" : "等待导入 DAT");
		}
		xWAnimationVisualResourceEditor.AddPreviewRow("完整数据: " + text);
		AddPreviewRow($"时间线帧数据: {0}");
		AddPreviewRow($"Packed 帧数据: {animation.frameOffsets?.Length ?? 0}");
		AddPreviewRow($"Packed 切片数据: {animation.sliceKeys?.Length ?? 0}");
		if (CanvasGrid == null)
		{
			return;
		}
		CanvasGrid.Columns = 1;
		if (_animationRuntimePreviewScene == null)
		{
			_animationRuntimePreviewScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationRuntimePreview.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _animationRuntimePreviewScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(vBoxContainer))
		{
			return;
		}
		MountAnimationResponsiveWorkbench(vBoxContainer);
		BindAnimationDirectEditSurface(vBoxContainer, animation);
		AdobeAnimateInspectorPreview adobeAnimateInspectorPreview = (_runtimePreview = vBoxContainer.GetNode<AdobeAnimateInspectorPreview>("%RuntimeInspectorPreview"));
		_animationRuntimeFrameLabel = vBoxContainer.GetNode<Label>("TitleRow/FrameLabel");
		_animationPlaybackStateLabel = vBoxContainer.GetNode<Label>("%PlaybackStateLabel");
		_animationStateUpdateCount = 0;
		adobeAnimateInspectorPreview.PreviewStateChanged += OnAnimationPreviewStateChanged;
		BindAnimationClipSurface(vBoxContainer, animation);
		BindAnimationFrameSurface(vBoxContainer, animation);
		PanelContainer node = vBoxContainer.GetNode<PanelContainer>("%LoadingPanel");
		_animationPreviewStatusLabel = vBoxContainer.GetNode<Label>("%LoadingLabel");
		vBoxContainer.GetNode<Label>("TitleRow/FrameLabel").Text = $"{GetAnimationFrameCount(animation)} 帧";
		if (!flag)
		{
			adobeAnimateInspectorPreview.Visible = false;
			node.Visible = true;
			if (flag2)
			{
				StartAnimationResourceHydration();
			}
			else if (GodotObject.IsInstanceValid(_animationPreviewStatusLabel))
			{
				_animationPreviewStatusLabel.Text = "尚未选择动画 DAT；可直接使用上方按钮导入。";
			}
		}
		else
		{
			node.Visible = false;
			adobeAnimateInspectorPreview.Visible = true;
			adobeAnimateInspectorPreview.EditAnimation(animation);
			FitRuntimePreviewAfterLayout(adobeAnimateInspectorPreview);
		}
		Vector2 vector = (flag ? TryGetSharedAnimationAtlasSize(animation) : Vector2.Zero);
		Label node2 = vBoxContainer.GetNode<Label>("AtlasLabel");
		string text2;
		if (vector == Vector2.Zero)
		{
			if (flag)
			{
				text2 = "未读取到内嵌图集";
			}
			else
			{
				text2 = (flag2 ? "正在后台读取动画资源..." : "等待导入动画 DAT");
			}
		}
		else
		{
			text2 = $"内嵌图集: {vector.X:0} x {vector.Y:0}";
		}
		node2.Text = text2;
		if (GodotObject.IsInstanceValid(_animationMediaOption))
		{
			_animationMediaOption.Visible = false;
		}
		if (GodotObject.IsInstanceValid(_animationLayerOption))
		{
			_animationLayerOption.Visible = false;
		}
		ConfigureAnimationWorkbenchSurface(0);
	}

	private void ReleaseClosedAnimationResource()
	{
		AdobeAnimateData editingAnimation = _editingAnimation;
		DisposeAnimationPropertyBinding();
		CancelAnimationResourceHydration();
		_editingAnimation = null;
		if (GodotObject.IsInstanceValid(editingAnimation))
		{
			AdobeAnimateDefinitionCache.Invalidate(editingAnimation);
		}
	}

	private void OnAnimationPreviewStateChanged(int frame, int startFrame, int endFrame, string clipName, bool playing)
	{
		if (AnimationHighFrequencyActive)
		{
			_animationStateUpdateCount++;
			if (GodotObject.IsInstanceValid(_animationFrameTimeline))
			{
				_animationFrameTimeline.SetPlayhead(frame);
			}
			if (GodotObject.IsInstanceValid(_animationRuntimeFrameLabel))
			{
				_animationRuntimeFrameLabel.Text = $"帧 {frame} / {endFrame}";
			}
			if (GodotObject.IsInstanceValid(_animationPlaybackStateLabel))
			{
				string value = (string.IsNullOrWhiteSpace(clipName) ? "全部动画" : clipName);
				_animationPlaybackStateLabel.Text = $"{(playing ? "播放中" : "已暂停")} · {value} · 范围 {startFrame}-{endFrame}";
				_animationPlaybackStateLabel.Modulate = (playing ? new Color(0.66f, 1f, 0.48f) : new Color(1f, 0.78f, 0.34f));
			}
		}
	}

	private void BindAnimationClipSurface(VBoxContainer container, AdobeAnimateData animation)
	{
		_animationClipList = container.GetNode<ItemList>("%ClipList");
		_animationClipNameEdit = container.GetNode<LineEdit>("%ClipNameEdit");
		_animationClipStartSpinBox = container.GetNode<SpinBox>("%ClipStartSpinBox");
		_animationClipEndSpinBox = container.GetNode<SpinBox>("%ClipEndSpinBox");
		_animationClipRangeControl = container.GetNode<XWPacketSpawnEntryRangeControl>("%ClipRangeControl");
		_animationAddClipButton = container.GetNode<Button>("%AddClipButton");
		_animationDeleteClipButton = container.GetNode<Button>("%DeleteClipButton");
		_animationShiftClipEarlierButton = container.GetNode<Button>("%ShiftEarlierButton");
		_animationShiftClipLaterButton = container.GetNode<Button>("%ShiftLaterButton");
		_animationClipEditStatusLabel = container.GetNode<Label>("%ClipEditStatusLabel");
		_animationClipMutationCount = 0;
		_animationClipList.ItemSelected += OnAnimationClipSelected;
		_animationClipNameEdit.TextSubmitted += OnAnimationClipNameSubmitted;
		_animationClipNameEdit.FocusExited += OnAnimationClipNameFocusExited;
		_animationClipStartSpinBox.FocusEntered += BeginAnimationClipExactRangeEdit;
		_animationClipStartSpinBox.ValueChanged += PreviewAnimationClipExactRange;
		_animationClipStartSpinBox.FocusExited += CommitAnimationClipExactRangeEdit;
		_animationClipEndSpinBox.FocusEntered += BeginAnimationClipExactRangeEdit;
		_animationClipEndSpinBox.ValueChanged += PreviewAnimationClipExactRange;
		_animationClipEndSpinBox.FocusExited += CommitAnimationClipExactRangeEdit;
		_animationClipRangeControl.DragStarted += BeginAnimationClipRangeEdit;
		_animationClipRangeControl.ValuesPreviewed += PreviewAnimationClipRange;
		_animationClipRangeControl.ValuesCommitted += CommitAnimationClipRange;
		_animationAddClipButton.Pressed += AddAnimationClip;
		_animationDeleteClipButton.Pressed += DeleteSelectedAnimationClip;
		_animationShiftClipEarlierButton.Pressed += ShiftSelectedAnimationClipEarlier;
		_animationShiftClipLaterButton.Pressed += ShiftSelectedAnimationClipLater;
		RefreshAnimationClipSurface(animation, "", refreshPreviewOptions: true);
	}

	private void DisposeAnimationClipSurface()
	{
		CancelAnimationClipRangeEdit();
		if (GodotObject.IsInstanceValid(_animationClipList))
		{
			_animationClipList.ItemSelected -= OnAnimationClipSelected;
		}
		if (GodotObject.IsInstanceValid(_animationClipNameEdit))
		{
			_animationClipNameEdit.TextSubmitted -= OnAnimationClipNameSubmitted;
			_animationClipNameEdit.FocusExited -= OnAnimationClipNameFocusExited;
		}
		if (GodotObject.IsInstanceValid(_animationClipStartSpinBox))
		{
			_animationClipStartSpinBox.FocusEntered -= BeginAnimationClipExactRangeEdit;
			_animationClipStartSpinBox.ValueChanged -= PreviewAnimationClipExactRange;
			_animationClipStartSpinBox.FocusExited -= CommitAnimationClipExactRangeEdit;
		}
		if (GodotObject.IsInstanceValid(_animationClipEndSpinBox))
		{
			_animationClipEndSpinBox.FocusEntered -= BeginAnimationClipExactRangeEdit;
			_animationClipEndSpinBox.ValueChanged -= PreviewAnimationClipExactRange;
			_animationClipEndSpinBox.FocusExited -= CommitAnimationClipExactRangeEdit;
		}
		if (GodotObject.IsInstanceValid(_animationClipRangeControl))
		{
			_animationClipRangeControl.DragStarted -= BeginAnimationClipRangeEdit;
			_animationClipRangeControl.ValuesPreviewed -= PreviewAnimationClipRange;
			_animationClipRangeControl.ValuesCommitted -= CommitAnimationClipRange;
		}
		if (GodotObject.IsInstanceValid(_animationAddClipButton))
		{
			_animationAddClipButton.Pressed -= AddAnimationClip;
		}
		if (GodotObject.IsInstanceValid(_animationDeleteClipButton))
		{
			_animationDeleteClipButton.Pressed -= DeleteSelectedAnimationClip;
		}
		if (GodotObject.IsInstanceValid(_animationShiftClipEarlierButton))
		{
			_animationShiftClipEarlierButton.Pressed -= ShiftSelectedAnimationClipEarlier;
		}
		if (GodotObject.IsInstanceValid(_animationShiftClipLaterButton))
		{
			_animationShiftClipLaterButton.Pressed -= ShiftSelectedAnimationClipLater;
		}
		_animationClipList = null;
		_animationClipNameEdit = null;
		_animationClipStartSpinBox = null;
		_animationClipEndSpinBox = null;
		_animationClipRangeControl = null;
		_animationAddClipButton = null;
		_animationDeleteClipButton = null;
		_animationShiftClipEarlierButton = null;
		_animationShiftClipLaterButton = null;
		_animationClipEditStatusLabel = null;
		_selectedAnimationClipName = "";
	}

	private void RefreshAnimationClipSurface(AdobeAnimateData animation, string requestedSelection, bool refreshPreviewOptions)
	{
		if (!GodotObject.IsInstanceValid(animation) || !GodotObject.IsInstanceValid(_animationClipList))
		{
			return;
		}
		int num = Math.Max(0, GetAnimationFrameCount(animation) - 1);
		_updatingAnimationClipControls = true;
		_animationClipStartSpinBox.MinValue = 0.0;
		_animationClipStartSpinBox.MaxValue = num;
		_animationClipEndSpinBox.MinValue = 0.0;
		_animationClipEndSpinBox.MaxValue = num;
		_animationClipRangeControl.SetTitle("Clip 可播放区间（结束帧为可见帧）");
		_animationClipRangeControl.Configure(0.0, num, 1.0, allowUnlimited: false);
		_animationClipList.Clear();
		string text;
		if (HasAnimationClip(animation, requestedSelection))
		{
			text = requestedSelection;
		}
		else
		{
			text = (HasAnimationClip(animation, _selectedAnimationClipName) ? _selectedAnimationClipName : "");
		}
		int num2 = -1;
		if (animation.clips != null)
		{
			foreach (Variant key in animation.clips.Keys)
			{
				string text2 = key.AsString();
				Vector2I range = ClampAnimationClipRange(animation, animation.GetClip(text2));
				int num3 = _animationClipList.AddItem(FormatAnimationClipListItem(text2, range));
				_animationClipList.SetItemMetadata(num3, text2);
				if (string.Equals(text, text2, StringComparison.Ordinal))
				{
					num2 = num3;
				}
			}
		}
		_updatingAnimationClipControls = false;
		if (refreshPreviewOptions && GodotObject.IsInstanceValid(_runtimePreview))
		{
			_runtimePreview.RefreshClipOptions(text);
		}
		if (num2 >= 0)
		{
			_animationClipList.Select(num2);
			SelectAnimationClip(text, !refreshPreviewOptions);
		}
		else
		{
			SelectAnimationClip("", !refreshPreviewOptions);
		}
	}

	private void OnAnimationClipSelected(long index)
	{
		if (!_updatingAnimationClipControls && GodotObject.IsInstanceValid(_animationClipList))
		{
			CommitAnimationClipRangeEdit();
			string clipName = ((index >= 0 && index < _animationClipList.ItemCount) ? _animationClipList.GetItemMetadata((int)index).AsString() : "");
			SelectAnimationClip(clipName, syncPreview: true);
		}
	}

	private void SelectAnimationClip(string clipName, bool syncPreview)
	{
		_selectedAnimationClipName = clipName ?? "";
		AdobeAnimateData adobeAnimateData = CurrentResource as AdobeAnimateData;
		bool flag = GodotObject.IsInstanceValid(adobeAnimateData) && HasAnimationClip(adobeAnimateData, _selectedAnimationClipName);
		_updatingAnimationClipControls = true;
		if (GodotObject.IsInstanceValid(_animationClipNameEdit))
		{
			_animationClipNameEdit.Editable = flag;
			_animationClipNameEdit.Text = (flag ? _selectedAnimationClipName : "");
		}
		if (flag)
		{
			Vector2I animationClipRangeControls = ClampAnimationClipRange(adobeAnimateData, adobeAnimateData.GetClip(_selectedAnimationClipName));
			SetAnimationClipRangeControls(animationClipRangeControls);
		}
		else
		{
			SetAnimationClipRangeControls(new Vector2I(0, 1));
		}
		_updatingAnimationClipControls = false;
		if (GodotObject.IsInstanceValid(_animationDeleteClipButton))
		{
			_animationDeleteClipButton.Disabled = !flag;
		}
		if (GodotObject.IsInstanceValid(_animationShiftClipEarlierButton))
		{
			_animationShiftClipEarlierButton.Disabled = !flag;
		}
		if (GodotObject.IsInstanceValid(_animationShiftClipLaterButton))
		{
			_animationShiftClipLaterButton.Disabled = !flag;
		}
		if (GodotObject.IsInstanceValid(_animationClipStartSpinBox))
		{
			_animationClipStartSpinBox.Editable = flag;
		}
		if (GodotObject.IsInstanceValid(_animationClipEndSpinBox))
		{
			_animationClipEndSpinBox.Editable = flag;
		}
		if (GodotObject.IsInstanceValid(_animationClipRangeControl))
		{
			_animationClipRangeControl.MouseFilter = (MouseFilterEnum)(flag ? 0 : 2);
			_animationClipRangeControl.Modulate = (flag ? Colors.White : new Color(0.48f, 0.52f, 0.48f, 0.7f));
		}
		if (syncPreview && GodotObject.IsInstanceValid(_runtimePreview))
		{
			_runtimePreview.SelectClipByName(flag ? _selectedAnimationClipName : "");
		}
		SetAnimationClipStatus(flag ? ("正在编辑 " + _selectedAnimationClipName + "：拖动两个彩色端点调整可播放范围。") : "当前没有 Clip；点击“创建 Clip”从当前预览帧建立。");
	}

	private void SetAnimationClipRangeControls(Vector2I storedRange)
	{
		int num = Math.Max(storedRange.X, storedRange.Y - 1);
		if (GodotObject.IsInstanceValid(_animationClipStartSpinBox))
		{
			_animationClipStartSpinBox.Value = storedRange.X;
		}
		if (GodotObject.IsInstanceValid(_animationClipEndSpinBox))
		{
			_animationClipEndSpinBox.Value = num;
		}
		if (GodotObject.IsInstanceValid(_animationClipRangeControl))
		{
			_animationClipRangeControl.SetValues(storedRange.X, num);
		}
	}

	private void BeginAnimationClipRangeEdit(bool minimumHandle)
	{
		BeginAnimationClipRangeEdit();
	}

	private void BeginAnimationClipExactRangeEdit()
	{
		BeginAnimationClipRangeEdit();
	}

	private void BeginAnimationClipRangeEdit()
	{
		if (_animationClipEditSnapshot == null && CurrentResource is AdobeAnimateData adobeAnimateData && HasAnimationClip(adobeAnimateData, _selectedAnimationClipName))
		{
			_animationClipEditSnapshot = CloneAnimationClips(adobeAnimateData.clips);
			_animationClipEditName = _selectedAnimationClipName;
			_animationClipEditTarget = adobeAnimateData;
		}
	}

	private void PreviewAnimationClipExactRange(double value)
	{
		if (!_updatingAnimationClipControls)
		{
			PreviewAnimationClipRange(_animationClipStartSpinBox.Value, _animationClipEndSpinBox.Value);
		}
	}

	private void PreviewAnimationClipRange(double minimum, double maximum)
	{
		if (!_updatingAnimationClipControls && CurrentResource is AdobeAnimateData adobeAnimateData && HasAnimationClip(adobeAnimateData, _selectedAnimationClipName))
		{
			BeginAnimationClipRangeEdit();
			Vector2I vector2I = ClampAnimationClipRange(adobeAnimateData, new Vector2I((int)Math.Round(minimum), (int)Math.Round(maximum) + 1));
			Dictionary dictionary = CloneAnimationClips(adobeAnimateData.clips);
			dictionary[_selectedAnimationClipName] = vector2I;
			adobeAnimateData.clips = dictionary;
			adobeAnimateData.EmitChanged();
			_updatingAnimationClipControls = true;
			SetAnimationClipRangeControls(vector2I);
			UpdateAnimationClipListItem(_selectedAnimationClipName, vector2I);
			_updatingAnimationClipControls = false;
			if (GodotObject.IsInstanceValid(_runtimePreview))
			{
				_runtimePreview.SelectClipByName(_selectedAnimationClipName);
			}
			SetAnimationClipStatus($"预览 {_selectedAnimationClipName}：{vector2I.X}-{vector2I.Y - 1}（松开后写入撤销历史）");
		}
	}

	private void CommitAnimationClipExactRangeEdit()
	{
		CommitAnimationClipRangeEdit();
	}

	private void CommitAnimationClipRange(double minimum, double maximum)
	{
		PreviewAnimationClipRange(minimum, maximum);
		CommitAnimationClipRangeEdit();
	}

	private void CommitAnimationClipRangeEdit()
	{
		AdobeAnimateData animationClipEditTarget = _animationClipEditTarget;
		if (_animationClipEditSnapshot == null || !GodotObject.IsInstanceValid(animationClipEditTarget) || CurrentResource != animationClipEditTarget)
		{
			CancelAnimationClipRangeEdit();
			return;
		}
		Dictionary animationClipEditSnapshot = _animationClipEditSnapshot;
		string animationClipEditName = _animationClipEditName;
		Dictionary nextClips = CloneAnimationClips(animationClipEditTarget.clips);
		_animationClipEditSnapshot = null;
		_animationClipEditName = "";
		_animationClipEditTarget = null;
		CommitAnimationClipDictionaryChange(animationClipEditSnapshot, nextClips, animationClipEditName, animationClipEditName, "调整动画 Clip 范围");
	}

	private void CancelAnimationClipRangeEdit()
	{
		if (_animationClipEditSnapshot != null && GodotObject.IsInstanceValid(_animationClipEditTarget))
		{
			_animationClipEditTarget.clips = CloneAnimationClips(_animationClipEditSnapshot);
			_animationClipEditTarget.EmitChanged();
		}
		_animationClipEditSnapshot = null;
		_animationClipEditName = "";
		_animationClipEditTarget = null;
	}

	private void AddAnimationClip()
	{
		CommitAnimationClipRangeEdit();
		if (CurrentResource is AdobeAnimateData adobeAnimateData)
		{
			Dictionary oldClips = CloneAnimationClips(adobeAnimateData.clips);
			Dictionary dictionary = CloneAnimationClips(adobeAnimateData.clips);
			string text = BuildUniqueAnimationClipName(dictionary, "clip");
			int num = Math.Max(1, GetAnimationFrameCount(adobeAnimateData));
			int num2 = Math.Clamp(GodotObject.IsInstanceValid(_runtimePreview) ? _runtimePreview.CurrentFrame : 0, 0, num - 1);
			dictionary[text] = new Vector2I(num2, Math.Min(num, num2 + 1));
			CommitAnimationClipDictionaryChange(oldClips, dictionary, text, _selectedAnimationClipName, "创建动画 Clip");
		}
	}

	private void DeleteSelectedAnimationClip()
	{
		CommitAnimationClipRangeEdit();
		if (!(CurrentResource is AdobeAnimateData adobeAnimateData) || !HasAnimationClip(adobeAnimateData, _selectedAnimationClipName))
		{
			return;
		}
		string selectedAnimationClipName = _selectedAnimationClipName;
		Dictionary oldClips = CloneAnimationClips(adobeAnimateData.clips);
		Dictionary dictionary = new Dictionary();
		string text = "";
		foreach (Variant key in adobeAnimateData.clips.Keys)
		{
			string text2 = key.AsString();
			if (!string.Equals(text2, selectedAnimationClipName, StringComparison.Ordinal))
			{
				dictionary[key] = adobeAnimateData.clips[key];
				if (string.IsNullOrEmpty(text))
				{
					text = text2;
				}
			}
		}
		CommitAnimationClipDictionaryChange(oldClips, dictionary, text, selectedAnimationClipName, "删除动画 Clip");
	}

	private void ShiftSelectedAnimationClipEarlier()
	{
		ShiftSelectedAnimationClip(-1);
	}

	private void ShiftSelectedAnimationClipLater()
	{
		ShiftSelectedAnimationClip(1);
	}

	private void ShiftSelectedAnimationClip(int direction)
	{
		CommitAnimationClipRangeEdit();
		if (CurrentResource is AdobeAnimateData adobeAnimateData && HasAnimationClip(adobeAnimateData, _selectedAnimationClipName))
		{
			Vector2I vector2I = ClampAnimationClipRange(adobeAnimateData, adobeAnimateData.GetClip(_selectedAnimationClipName));
			int num = Math.Max(1, GetAnimationFrameCount(adobeAnimateData));
			int num2 = Math.Max(1, vector2I.Y - vector2I.X);
			int num3 = Math.Clamp(vector2I.X + Math.Sign(direction), 0, Math.Max(0, num - num2));
			if (num3 != vector2I.X)
			{
				Dictionary oldClips = CloneAnimationClips(adobeAnimateData.clips);
				Dictionary dictionary = CloneAnimationClips(adobeAnimateData.clips);
				dictionary[_selectedAnimationClipName] = new Vector2I(num3, num3 + num2);
				CommitAnimationClipDictionaryChange(oldClips, dictionary, _selectedAnimationClipName, _selectedAnimationClipName, (direction < 0) ? "动画 Clip 前移 1 帧" : "动画 Clip 后移 1 帧");
			}
		}
	}

	private void OnAnimationClipNameSubmitted(string text)
	{
		RenameSelectedAnimationClip(text);
	}

	private void OnAnimationClipNameFocusExited()
	{
		if (!_updatingAnimationClipControls && GodotObject.IsInstanceValid(_animationClipNameEdit))
		{
			RenameSelectedAnimationClip(_animationClipNameEdit.Text);
		}
	}

	private void RenameSelectedAnimationClip(string requestedName)
	{
		CommitAnimationClipRangeEdit();
		if (!(CurrentResource is AdobeAnimateData adobeAnimateData) || !HasAnimationClip(adobeAnimateData, _selectedAnimationClipName))
		{
			return;
		}
		string selectedAnimationClipName = _selectedAnimationClipName;
		string text = (requestedName ?? "").Trim();
		if (string.Equals(selectedAnimationClipName, text, StringComparison.Ordinal))
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			SetAnimationClipStatus("Clip 名称不能为空。", error: true);
			return;
		}
		if (HasAnimationClip(adobeAnimateData, text))
		{
			SetAnimationClipStatus("Clip 名称“" + text + "”已存在。", error: true);
			return;
		}
		Dictionary oldClips = CloneAnimationClips(adobeAnimateData.clips);
		Dictionary dictionary = new Dictionary();
		foreach (Variant key in adobeAnimateData.clips.Keys)
		{
			if (string.Equals(key.AsString(), selectedAnimationClipName, StringComparison.Ordinal))
			{
				dictionary[text] = adobeAnimateData.clips[key];
			}
			else
			{
				dictionary[key] = adobeAnimateData.clips[key];
			}
		}
		CommitAnimationClipDictionaryChange(oldClips, dictionary, text, selectedAnimationClipName, "重命名动画 Clip");
	}

	private void CommitAnimationClipDictionaryChange(Dictionary oldClips, Dictionary nextClips, string doSelection, string undoSelection, string actionName)
	{
		AdobeAnimateData from = CurrentResource as AdobeAnimateData;
		if (from != null && !AnimationClipDictionariesEqual(oldClips, nextClips))
		{
			Dictionary from2 = CloneAnimationClips(nextClips);
			Dictionary from3 = CloneAnimationClips(oldClips);
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager == null)
			{
				from.clips = from2;
				RefreshAnimationClipEditorFromHistory(from, doSelection);
				return;
			}
			xWUndoRedoManager.CreateAction(actionName);
			xWUndoRedoManager.AddDoProperty(from, "clips", Variant.From(in from2));
			xWUndoRedoManager.AddUndoProperty(from, "clips", Variant.From(in from3));
			Variant[] array = new Variant[2]
			{
				Variant.From(in from),
				default
			};
			string from4 = doSelection ?? "";
			array[1] = Variant.From(in from4);
			xWUndoRedoManager.AddDoMethod(this, "RefreshAnimationClipEditorFromHistory", array);
			Variant[] array2 = new Variant[2]
			{
				Variant.From(in from),
				default
			};
			array2[1] = Variant.From<string>(undoSelection ?? "");
			xWUndoRedoManager.AddUndoMethod(this, "RefreshAnimationClipEditorFromHistory", array2);
			xWUndoRedoManager.CommitAction();
		}
	}

	public void RefreshAnimationClipEditorFromHistory(AdobeAnimateData animation, string requestedSelection)
	{
		if (GodotObject.IsInstanceValid(animation))
		{
			AdobeAnimateDefinitionCache.Invalidate(animation);
			animation.EmitChanged();
			if (CurrentResource == animation)
			{
				_animationClipMutationCount++;
				RefreshAnimationClipSurface(animation, requestedSelection ?? "", refreshPreviewOptions: true);
				RefreshAnimationTimelineClipRows(animation);
				UpdateAnimationDirectSummary();
				NotifyCurrentResourceEdited();
			}
		}
	}

	private void RefreshAnimationTimelineClipRows(AdobeAnimateData animation)
	{
		if (!GodotObject.IsInstanceValid(TimelineList))
		{
			return;
		}
		for (int num = TimelineList.ItemCount - 1; num >= 0; num--)
		{
			string itemText = TimelineList.GetItemText(num);
			if (itemText.StartsWith("Clip ", StringComparison.Ordinal) || itemText.StartsWith("Clip:", StringComparison.Ordinal))
			{
				TimelineList.RemoveItem(num);
			}
		}
		if (animation.clips == null || animation.clips.Count == 0)
		{
			AddTimelineRow("Clip: 未定义");
			return;
		}
		foreach (Variant key in animation.clips.Keys)
		{
			AddTimelineRow("Clip " + key.AsString() + ": " + FormatVariant(animation.clips[key]));
		}
	}

	private void UpdateAnimationClipListItem(string clipName, Vector2I range)
	{
		if (!GodotObject.IsInstanceValid(_animationClipList))
		{
			return;
		}
		for (int i = 0; i < _animationClipList.ItemCount; i++)
		{
			if (string.Equals(_animationClipList.GetItemMetadata(i).AsString(), clipName, StringComparison.Ordinal))
			{
				_animationClipList.SetItemText(i, FormatAnimationClipListItem(clipName, range));
				break;
			}
		}
	}

	private void SetAnimationClipStatus(string message, bool error = false)
	{
		if (GodotObject.IsInstanceValid(_animationClipEditStatusLabel))
		{
			_animationClipEditStatusLabel.Text = message ?? "";
			_animationClipEditStatusLabel.Modulate = (error ? new Color(1f, 0.42f, 0.34f) : new Color(0.68f, 0.88f, 0.52f));
		}
	}

	private static string FormatAnimationClipListItem(string clipName, Vector2I range)
	{
		return $"{clipName}    {range.X} ━ {Math.Max(range.X, range.Y - 1)}    ({Math.Max(1, range.Y - range.X)} 帧)";
	}

	private static Vector2I ClampAnimationClipRange(AdobeAnimateData animation, Vector2I range)
	{
		int num = Math.Max(1, GetAnimationFrameCount(animation));
		int num2 = Math.Clamp(range.X, 0, num - 1);
		int num3 = Math.Clamp(Math.Max(num2, range.Y - 1), num2, num - 1);
		return new Vector2I(num2, num3 + 1);
	}

	private static bool HasAnimationClip(AdobeAnimateData animation, string clipName)
	{
		if (animation?.clips == null || string.IsNullOrEmpty(clipName))
		{
			return false;
		}
		foreach (Variant key in animation.clips.Keys)
		{
			if (string.Equals(key.AsString(), clipName, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	private static string BuildUniqueAnimationClipName(Dictionary clips, string baseName)
	{
		baseName = (string.IsNullOrWhiteSpace(baseName) ? "clip" : baseName.Trim());
		string text = baseName;
		int num = 2;
		while (AnimationClipDictionaryContains(clips, text))
		{
			text = $"{baseName}_{num++}";
		}
		return text;
	}

	private static bool AnimationClipDictionaryContains(Dictionary clips, string clipName)
	{
		if (clips == null)
		{
			return false;
		}
		foreach (Variant key in clips.Keys)
		{
			if (string.Equals(key.AsString(), clipName, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	private static Dictionary CloneAnimationClips(Dictionary source)
	{
		Dictionary dictionary = new Dictionary();
		if (source == null)
		{
			return dictionary;
		}
		foreach (Variant key in source.Keys)
		{
			dictionary[key] = source[key];
		}
		return dictionary;
	}

	private static bool AnimationClipDictionariesEqual(Dictionary left, Dictionary right)
	{
		if (left == right)
		{
			return true;
		}
		if (left == null || right == null || left.Count != right.Count)
		{
			return false;
		}
		foreach (Variant key in left.Keys)
		{
			string text = key.AsString();
			if (!AnimationClipDictionaryContains(right, text))
			{
				return false;
			}
			Variant variant = default;
			foreach (Variant key2 in right.Keys)
			{
				if (string.Equals(key2.AsString(), text, StringComparison.Ordinal))
				{
					variant = right[key2];
					break;
				}
			}
			if (!left[key].Equals(variant))
			{
				return false;
			}
		}
		return true;
	}

	private void BindAnimationFrameSurface(VBoxContainer container, AdobeAnimateData animation)
	{
		_animationFrameTimeline = container.GetNode<XWAnimationFrameTimeline>("%FrameTimeline");
		_animationAddKeyframeButton = container.GetNode<Button>("%AddKeyframeButton");
		_animationDuplicateKeyframeButton = container.GetNode<Button>("%DuplicateKeyframeButton");
		_animationDeleteKeyframeButton = container.GetNode<Button>("%DeleteKeyframeButton");
		_animationMoveKeyframeEarlierButton = container.GetNode<Button>("%MoveKeyframeEarlierButton");
		_animationMoveKeyframeLaterButton = container.GetNode<Button>("%MoveKeyframeLaterButton");
		_animationSelectedKeyframeLabel = container.GetNode<Label>("%SelectedKeyframeLabel");
		_animationMediaOption = container.GetNode<OptionButton>("%MediaOption");
		_animationLayerOption = container.GetNode<OptionButton>("%LayerOption");
		_animationMediaThumbnail = container.GetNode<TextureRect>("%MediaThumbnail");
		_animationPositionXSpinBox = container.GetNode<SpinBox>("%PositionXSpinBox");
		_animationPositionYSpinBox = container.GetNode<SpinBox>("%PositionYSpinBox");
		_animationRotationSpinBox = container.GetNode<SpinBox>("%RotationSpinBox");
		_animationSkewSpinBox = container.GetNode<SpinBox>("%SkewSpinBox");
		_animationScaleXSpinBox = container.GetNode<SpinBox>("%ScaleXSpinBox");
		_animationScaleYSpinBox = container.GetNode<SpinBox>("%ScaleYSpinBox");
		_animationAlphaSlider = container.GetNode<HSlider>("%AlphaSlider");
		_animationAlphaPercentLabel = container.GetNode<Label>("%AlphaPercentLabel");
		_animationKeyframeEditStatusLabel = container.GetNode<Label>("%KeyframeEditStatusLabel");
		_animationFrameMutationCount = 0;
		_animationFrameTimeline.KeyframeSelected += OnAnimationKeyframeSelected;
		_animationFrameTimeline.KeyframeMoveRequested += OnAnimationKeyframeMoveRequested;
		_animationAddKeyframeButton.Pressed += AddAnimationKeyframeAtPlayhead;
		_animationDuplicateKeyframeButton.Pressed += DuplicateSelectedAnimationKeyframe;
		_animationDeleteKeyframeButton.Pressed += DeleteSelectedAnimationKeyframe;
		_animationMoveKeyframeEarlierButton.Pressed += MoveSelectedAnimationKeyframeEarlier;
		_animationMoveKeyframeLaterButton.Pressed += MoveSelectedAnimationKeyframeLater;
		_animationMediaOption.ItemSelected += OnAnimationFrameMediaSelected;
		_animationLayerOption.ItemSelected += OnAnimationFrameLayerSelected;
		BindAnimationFrameNumberControl(_animationPositionXSpinBox);
		BindAnimationFrameNumberControl(_animationPositionYSpinBox);
		BindAnimationFrameNumberControl(_animationRotationSpinBox);
		BindAnimationFrameNumberControl(_animationSkewSpinBox);
		BindAnimationFrameNumberControl(_animationScaleXSpinBox);
		BindAnimationFrameNumberControl(_animationScaleYSpinBox);
		_animationAlphaSlider.DragStarted += BeginAnimationFramePropertyEdit;
		_animationAlphaSlider.ValueChanged += PreviewSelectedAnimationFrameSlice;
		_animationAlphaSlider.DragEnded += CommitAnimationFrameAlphaEdit;
		PopulateAnimationFrameOptions(animation);
		_animationFrameTimeline.BindAnimation(animation);
		if (!_animationFrameTimeline.SelectFirstOccupiedCell())
		{
			SetAnimationFrameControlsEnabled(enabled: false);
		}
	}

	private void BindAnimationFrameNumberControl(SpinBox spinBox)
	{
		spinBox.FocusEntered += BeginAnimationFramePropertyEdit;
		spinBox.ValueChanged += PreviewSelectedAnimationFrameSlice;
		spinBox.FocusExited += CommitAnimationFramePropertyEdit;
	}

	private void DisposeAnimationFrameSurface()
	{
		CancelAnimationFramePropertyEdit();
		if (GodotObject.IsInstanceValid(_animationFrameTimeline))
		{
			_animationFrameTimeline.KeyframeSelected -= OnAnimationKeyframeSelected;
			_animationFrameTimeline.KeyframeMoveRequested -= OnAnimationKeyframeMoveRequested;
		}
		if (GodotObject.IsInstanceValid(_animationAddKeyframeButton))
		{
			_animationAddKeyframeButton.Pressed -= AddAnimationKeyframeAtPlayhead;
		}
		if (GodotObject.IsInstanceValid(_animationDuplicateKeyframeButton))
		{
			_animationDuplicateKeyframeButton.Pressed -= DuplicateSelectedAnimationKeyframe;
		}
		if (GodotObject.IsInstanceValid(_animationDeleteKeyframeButton))
		{
			_animationDeleteKeyframeButton.Pressed -= DeleteSelectedAnimationKeyframe;
		}
		if (GodotObject.IsInstanceValid(_animationMoveKeyframeEarlierButton))
		{
			_animationMoveKeyframeEarlierButton.Pressed -= MoveSelectedAnimationKeyframeEarlier;
		}
		if (GodotObject.IsInstanceValid(_animationMoveKeyframeLaterButton))
		{
			_animationMoveKeyframeLaterButton.Pressed -= MoveSelectedAnimationKeyframeLater;
		}
		if (GodotObject.IsInstanceValid(_animationMediaOption))
		{
			_animationMediaOption.ItemSelected -= OnAnimationFrameMediaSelected;
		}
		if (GodotObject.IsInstanceValid(_animationLayerOption))
		{
			_animationLayerOption.ItemSelected -= OnAnimationFrameLayerSelected;
		}
		UnbindAnimationFrameNumberControl(_animationPositionXSpinBox);
		UnbindAnimationFrameNumberControl(_animationPositionYSpinBox);
		UnbindAnimationFrameNumberControl(_animationRotationSpinBox);
		UnbindAnimationFrameNumberControl(_animationSkewSpinBox);
		UnbindAnimationFrameNumberControl(_animationScaleXSpinBox);
		UnbindAnimationFrameNumberControl(_animationScaleYSpinBox);
		if (GodotObject.IsInstanceValid(_animationAlphaSlider))
		{
			_animationAlphaSlider.DragStarted -= BeginAnimationFramePropertyEdit;
			_animationAlphaSlider.ValueChanged -= PreviewSelectedAnimationFrameSlice;
			_animationAlphaSlider.DragEnded -= CommitAnimationFrameAlphaEdit;
		}
		_animationFrameTimeline = null;
		_animationAddKeyframeButton = null;
		_animationDuplicateKeyframeButton = null;
		_animationDeleteKeyframeButton = null;
		_animationMoveKeyframeEarlierButton = null;
		_animationMoveKeyframeLaterButton = null;
		_animationSelectedKeyframeLabel = null;
		_animationMediaOption = null;
		_animationLayerOption = null;
		_animationMediaThumbnail = null;
		_animationPositionXSpinBox = null;
		_animationPositionYSpinBox = null;
		_animationRotationSpinBox = null;
		_animationSkewSpinBox = null;
		_animationScaleXSpinBox = null;
		_animationScaleYSpinBox = null;
		_animationAlphaSlider = null;
		_animationAlphaPercentLabel = null;
		_animationKeyframeEditStatusLabel = null;
	}

	private void UnbindAnimationFrameNumberControl(SpinBox spinBox)
	{
		if (GodotObject.IsInstanceValid(spinBox))
		{
			spinBox.FocusEntered -= BeginAnimationFramePropertyEdit;
			spinBox.ValueChanged -= PreviewSelectedAnimationFrameSlice;
			spinBox.FocusExited -= CommitAnimationFramePropertyEdit;
		}
	}

	private void PopulateAnimationFrameOptions(AdobeAnimateData animation)
	{
		_updatingAnimationFrameControls = true;
		_animationMediaOption.Clear();
		string[] animationMediaNames = GetAnimationMediaNames(animation);
		List<XWAnimationVisualChoice> list = new List<XWAnimationVisualChoice>(animationMediaNames.Length);
		if (_animationVisualFallbackIcon == null)
		{
			_animationVisualFallbackIcon = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/2D.svg", null, ResourceLoader.CacheMode.Reuse);
		}
		for (int i = 0; i < animationMediaNames.Length; i++)
		{
			int itemCount = _animationMediaOption.ItemCount;
			_animationMediaOption.AddItem(animationMediaNames[i]);
			_animationMediaOption.SetItemMetadata(itemCount, i);
			Texture2D icon = CreateAnimationMediaThumbnail(animation, i) ?? _animationVisualFallbackIcon;
			Color accent = Color.FromHsv(Mathf.PosMod((float)i * 0.113f + 0.08f, 1f), 0.5f, 0.94f);
			list.Add(new XWAnimationVisualChoice(i, animationMediaNames[i], $"媒体 #{i}", icon, accent));
		}
		_animationLayerOption.Clear();
		string[] animationLayerNames = GetAnimationLayerNames(animation);
		List<XWAnimationVisualChoice> list2 = new List<XWAnimationVisualChoice>(animationLayerNames.Length);
		for (int j = 0; j < animationLayerNames.Length; j++)
		{
			int itemCount2 = _animationLayerOption.ItemCount;
			_animationLayerOption.AddItem(animationLayerNames[j]);
			_animationLayerOption.SetItemMetadata(itemCount2, j);
			Color accent2 = Color.FromHsv(Mathf.PosMod((float)j * 0.137f + 0.23f, 1f), 0.52f, 0.94f);
			list2.Add(new XWAnimationVisualChoice(j, animationLayerNames[j], $"图层 #{j + 1}", _animationVisualFallbackIcon, accent2));
		}
		if (GodotObject.IsInstanceValid(_animationMediaChoiceGrid))
		{
			_animationMediaChoiceGrid.Configure("媒体缩略图", list, -1, enabled: false);
		}
		if (GodotObject.IsInstanceValid(_animationLayerChoiceGrid))
		{
			_animationLayerChoiceGrid.Configure("图层状态色", list2, -1, enabled: false);
		}
		_updatingAnimationFrameControls = false;
	}

	private void OnAnimationKeyframeSelected(int frame, int layer, int sliceKey)
	{
		CommitAnimationFramePropertyEdit();
		if (GodotObject.IsInstanceValid(_runtimePreview))
		{
			_runtimePreview.SetFrameFromSlider(frame);
		}
		RefreshSelectedAnimationFrameControls(frame, sliceKey);
	}

	private void RefreshSelectedAnimationFrameControls(int frame, int sliceKey)
	{
		string error = "";
		if (!(CurrentResource is AdobeAnimateData adobeAnimateData) || !adobeAnimateData.TryGetFrameSlice(frame, sliceKey, out var slice, out error))
		{
			SetAnimationFrameControlsEnabled(enabled: false);
			SetAnimationFrameStatus(string.IsNullOrWhiteSpace(error) ? "选中帧状态已不存在。" : error, error: true);
			return;
		}
		_updatingAnimationFrameControls = true;
		SetAnimationFrameControlsEnabled(enabled: true);
		string animationNameById = GetAnimationNameById(adobeAnimateData.layerDictionary, slice.LayerId, $"图层 {slice.LayerId + 1}");
		string animationNameById2 = GetAnimationNameById(adobeAnimateData.mediaDictionary, slice.MediaId, $"媒体 {slice.MediaId + 1}");
		_animationSelectedKeyframeLabel.Text = $"帧 {frame}  ·  {animationNameById}  ·  Slice #{slice.SliceKey}";
		SelectOptionByMetadata(_animationMediaOption, slice.MediaId);
		SelectOptionByMetadata(_animationLayerOption, slice.LayerId);
		_animationMediaChoiceGrid?.SelectChoice(slice.MediaId, emitSignal: false);
		_animationLayerChoiceGrid?.SelectChoice(slice.LayerId, emitSignal: false);
		Transform2D transform = slice.Transform;
		_animationPositionXSpinBox.Value = transform.Origin.X;
		_animationPositionYSpinBox.Value = transform.Origin.Y;
		_animationRotationSpinBox.Value = Mathf.RadToDeg(transform.Rotation);
		_animationSkewSpinBox.Value = Mathf.RadToDeg(transform.Skew);
		_animationScaleXSpinBox.Value = transform.Scale.X;
		_animationScaleYSpinBox.Value = transform.Scale.Y;
		_animationAlphaSlider.Value = slice.Alpha;
		_animationAlphaPercentLabel.Text = $"{Mathf.RoundToInt(slice.Alpha * 100f)}%";
		UpdateAnimationMediaThumbnail(adobeAnimateData, slice.MediaId);
		_updatingAnimationFrameControls = false;
		SetAnimationFrameStatus("正在直接编辑 " + animationNameById2 + "；拖动时间轴菱形可移帧。");
	}

	private void SetAnimationFrameControlsEnabled(bool enabled)
	{
		if (GodotObject.IsInstanceValid(_animationMediaOption))
		{
			_animationMediaOption.Disabled = !enabled;
		}
		if (GodotObject.IsInstanceValid(_animationLayerOption))
		{
			_animationLayerOption.Disabled = !enabled;
		}
		_animationMediaChoiceGrid?.SetChoicesEnabled(enabled);
		_animationLayerChoiceGrid?.SetChoicesEnabled(enabled);
		SpinBox[] array = new SpinBox[6] { _animationPositionXSpinBox, _animationPositionYSpinBox, _animationRotationSpinBox, _animationSkewSpinBox, _animationScaleXSpinBox, _animationScaleYSpinBox };
		foreach (SpinBox spinBox in array)
		{
			if (GodotObject.IsInstanceValid(spinBox))
			{
				spinBox.Editable = enabled;
			}
		}
		if (GodotObject.IsInstanceValid(_animationAlphaSlider))
		{
			_animationAlphaSlider.Editable = enabled;
		}
		if (GodotObject.IsInstanceValid(_animationDeleteKeyframeButton))
		{
			_animationDeleteKeyframeButton.Disabled = !enabled;
		}
		if (GodotObject.IsInstanceValid(_animationDuplicateKeyframeButton))
		{
			_animationDuplicateKeyframeButton.Disabled = !enabled;
		}
		if (GodotObject.IsInstanceValid(_animationMoveKeyframeEarlierButton))
		{
			_animationMoveKeyframeEarlierButton.Disabled = !enabled;
		}
		if (GodotObject.IsInstanceValid(_animationMoveKeyframeLaterButton))
		{
			_animationMoveKeyframeLaterButton.Disabled = !enabled;
		}
		if (!enabled && GodotObject.IsInstanceValid(_animationSelectedKeyframeLabel))
		{
			_animationSelectedKeyframeLabel.Text = "在时间轴中选择一个菱形帧状态";
		}
	}

	private void BeginAnimationFramePropertyEdit()
	{
		if (!_updatingAnimationFrameControls && _animationFrameEditSnapshot == null && CurrentResource is AdobeAnimateData adobeAnimateData && GodotObject.IsInstanceValid(_animationFrameTimeline) && _animationFrameTimeline.SelectedFrame >= 0 && _animationFrameTimeline.SelectedSliceKey >= 0)
		{
			_animationFrameEditSnapshot = adobeAnimateData.CaptureFrameEditSnapshot();
			_animationFrameEditTarget = adobeAnimateData;
			_animationFrameEditFrame = _animationFrameTimeline.SelectedFrame;
			_animationFrameEditSliceKey = _animationFrameTimeline.SelectedSliceKey;
			_animationFrameEditMutated = false;
		}
	}

	private void PreviewSelectedAnimationFrameSlice(double ignoredValue = 0.0)
	{
		if (_updatingAnimationFrameControls || !(CurrentResource is AdobeAnimateData adobeAnimateData) || !GodotObject.IsInstanceValid(_animationFrameTimeline) || _animationFrameTimeline.SelectedFrame < 0 || _animationFrameTimeline.SelectedSliceKey < 0)
		{
			return;
		}
		BeginAnimationFramePropertyEdit();
		int selectedFrame = _animationFrameTimeline.SelectedFrame;
		int selectedSliceKey = _animationFrameTimeline.SelectedSliceKey;
		if (!adobeAnimateData.TryGetFrameSlice(selectedFrame, selectedSliceKey, out var slice, out var error))
		{
			SetAnimationFrameStatus(error, error: true);
			return;
		}
		int selectedOptionMetadata = GetSelectedOptionMetadata(_animationMediaOption, slice.MediaId);
		int selectedOptionMetadata2 = GetSelectedOptionMetadata(_animationLayerOption, slice.LayerId);
		Transform2D transform = new Transform2D(Mathf.DegToRad((float)_animationRotationSpinBox.Value), new Vector2((float)_animationScaleXSpinBox.Value, (float)_animationScaleYSpinBox.Value), Mathf.DegToRad((float)_animationSkewSpinBox.Value), new Vector2((float)_animationPositionXSpinBox.Value, (float)_animationPositionYSpinBox.Value));
		float num = Math.Clamp((float)_animationAlphaSlider.Value, 0f, 1f);
		if (!adobeAnimateData.TryUpdateFrameSlice(selectedFrame, selectedSliceKey, selectedOptionMetadata, selectedOptionMetadata2, transform, num, out var error2))
		{
			SetAnimationFrameStatus(error2, error: true);
			return;
		}
		_animationFrameEditMutated = true;
		_animationAlphaPercentLabel.Text = $"{Mathf.RoundToInt(num * 100f)}%";
		UpdateAnimationMediaThumbnail(adobeAnimateData, selectedOptionMetadata);
		_animationFrameTimeline.RefreshFromAnimation();
		_animationFrameTimeline.SelectKeyframe(selectedFrame, selectedOptionMetadata2, selectedSliceKey, notify: false);
		RefreshAnimationRuntimeAtFrame(selectedFrame);
		SetAnimationFrameStatus($"正在预览帧 {selectedFrame} 的修改；离开控件后写入全局撤销历史。");
	}

	private void CommitAnimationFrameAlphaEdit(bool valueChanged)
	{
		CommitAnimationFramePropertyEdit();
	}

	private void CommitAnimationFramePropertyEdit()
	{
		AdobeAnimateData animationFrameEditTarget = _animationFrameEditTarget;
		if (_animationFrameEditSnapshot == null || !GodotObject.IsInstanceValid(animationFrameEditTarget))
		{
			CancelAnimationFramePropertyEdit();
			return;
		}
		Dictionary animationFrameEditSnapshot = _animationFrameEditSnapshot;
		int animationFrameEditFrame = _animationFrameEditFrame;
		int animationFrameEditSliceKey = _animationFrameEditSliceKey;
		bool animationFrameEditMutated = _animationFrameEditMutated;
		Dictionary nextSnapshot = (animationFrameEditMutated ? animationFrameEditTarget.CaptureFrameEditSnapshot() : null);
		_animationFrameEditSnapshot = null;
		_animationFrameEditTarget = null;
		_animationFrameEditFrame = -1;
		_animationFrameEditSliceKey = -1;
		_animationFrameEditMutated = false;
		if (animationFrameEditMutated)
		{
			if (!animationFrameEditTarget.TryRestoreFrameEditSnapshot(animationFrameEditSnapshot, out var error))
			{
				SetAnimationFrameStatus("无法建立撤销历史：" + error, error: true);
			}
			else
			{
				CommitAnimationFrameSnapshotChange(animationFrameEditTarget, animationFrameEditSnapshot, nextSnapshot, animationFrameEditFrame, animationFrameEditSliceKey, animationFrameEditFrame, animationFrameEditSliceKey, "编辑动画帧状态");
			}
		}
	}

	private void CancelAnimationFramePropertyEdit()
	{
		if (_animationFrameEditSnapshot != null && GodotObject.IsInstanceValid(_animationFrameEditTarget))
		{
			_animationFrameEditTarget.TryRestoreFrameEditSnapshot(_animationFrameEditSnapshot, out var _);
		}
		_animationFrameEditSnapshot = null;
		_animationFrameEditTarget = null;
		_animationFrameEditFrame = -1;
		_animationFrameEditSliceKey = -1;
		_animationFrameEditMutated = false;
	}

	private void OnAnimationFrameMediaSelected(long index)
	{
		if (!_updatingAnimationFrameControls)
		{
			BeginAnimationFramePropertyEdit();
			PreviewSelectedAnimationFrameSlice();
			CommitAnimationFramePropertyEdit();
		}
	}

	private void OnAnimationFrameLayerSelected(long index)
	{
		if (!_updatingAnimationFrameControls)
		{
			BeginAnimationFramePropertyEdit();
			PreviewSelectedAnimationFrameSlice();
			CommitAnimationFramePropertyEdit();
		}
	}

	private void OnAnimationVisualMediaSelected(int mediaId)
	{
		if (!_updatingAnimationFrameControls && GodotObject.IsInstanceValid(_animationMediaOption))
		{
			SelectOptionByMetadata(_animationMediaOption, mediaId);
			OnAnimationFrameMediaSelected(_animationMediaOption.Selected);
		}
	}

	private void OnAnimationVisualLayerSelected(int layerId)
	{
		if (!_updatingAnimationFrameControls && GodotObject.IsInstanceValid(_animationLayerOption))
		{
			SelectOptionByMetadata(_animationLayerOption, layerId);
			OnAnimationFrameLayerSelected(_animationLayerOption.Selected);
		}
	}

	private void AddAnimationKeyframeAtPlayhead()
	{
		if (!(CurrentResource is AdobeAnimateData adobeAnimateData))
		{
			return;
		}
		int targetFrame = Math.Clamp(GodotObject.IsInstanceValid(_runtimePreview) ? _runtimePreview.CurrentFrame : 0, 0, Math.Max(0, adobeAnimateData.EditableFrameCount - 1));
		int preferredSliceKey = (GodotObject.IsInstanceValid(_animationFrameTimeline) ? _animationFrameTimeline.SelectedSliceKey : (-1));
		if (!TryFindNearestAnimationFrameSlice(adobeAnimateData, targetFrame, preferredSliceKey, out var source))
		{
			SetAnimationFrameStatus("附近没有可复制的 Slice 状态。", error: true);
			return;
		}
		CommitAnimationFrameMutation(adobeAnimateData, "添加动画帧状态", targetFrame, source.SliceKey, (AdobeAnimateData resource) => (!resource.TryAddFrameSlice(targetFrame, source, out var error)) ? (Success: false, Error: error) : (Success: true, Error: ""));
	}

	private void DuplicateSelectedAnimationKeyframe()
	{
		if (!TryGetSelectedAnimationFrameSlice(out var animation, out var frame, out var slice))
		{
			return;
		}
		int targetFrame = Math.Min(animation.EditableFrameCount - 1, frame + 1);
		if (targetFrame == frame)
		{
			SetAnimationFrameStatus("已经是最后一帧。", error: true);
			return;
		}
		CommitAnimationFrameMutation(animation, "复制动画帧状态", targetFrame, slice.SliceKey, (AdobeAnimateData resource) => (!resource.TryCopyFrameSlice(frame, targetFrame, slice.SliceKey, out var error)) ? (Success: false, Error: error) : (Success: true, Error: ""));
	}

	private void DeleteSelectedAnimationKeyframe()
	{
		if (TryGetSelectedAnimationFrameSlice(out var animation, out var frame, out var slice))
		{
			CommitAnimationFrameMutation(animation, "删除动画帧状态", frame, -1, (AdobeAnimateData resource) => (!resource.TryRemoveFrameSlice(frame, slice.SliceKey, out var error)) ? (Success: false, Error: error) : (Success: true, Error: ""));
		}
	}

	private void MoveSelectedAnimationKeyframeEarlier()
	{
		MoveSelectedAnimationKeyframeBy(-1);
	}

	private void MoveSelectedAnimationKeyframeLater()
	{
		MoveSelectedAnimationKeyframeBy(1);
	}

	private void MoveSelectedAnimationKeyframeBy(int direction)
	{
		if (TryGetSelectedAnimationFrameSlice(out var animation, out var frame, out var slice))
		{
			int num = Math.Clamp(frame + Math.Sign(direction), 0, Math.Max(0, animation.EditableFrameCount - 1));
			if (num != frame)
			{
				MoveAnimationKeyframe(animation, frame, num, slice);
			}
		}
	}

	private void OnAnimationKeyframeMoveRequested(int sourceFrame, int destinationFrame, int layer, int sliceKey)
	{
		string error = "";
		if (!(CurrentResource is AdobeAnimateData adobeAnimateData) || !adobeAnimateData.TryGetFrameSlice(sourceFrame, sliceKey, out var slice, out error))
		{
			SetAnimationFrameStatus(error, error: true);
		}
		else
		{
			MoveAnimationKeyframe(adobeAnimateData, sourceFrame, destinationFrame, slice);
		}
	}

	private void MoveAnimationKeyframe(AdobeAnimateData animation, int sourceFrame, int destinationFrame, AdobeAnimateData.FrameSlice slice)
	{
		CommitAnimationFrameMutation(animation, "移动动画帧状态", destinationFrame, slice.SliceKey, (AdobeAnimateData resource) => (!resource.TryMoveFrameSlice(sourceFrame, destinationFrame, slice.SliceKey, out var error)) ? (Success: false, Error: error) : (Success: true, Error: ""));
	}

	private void CommitAnimationFrameMutation(AdobeAnimateData animation, string actionName, int doFrame, int doSliceKey, Func<AdobeAnimateData, (bool Success, string Error)> mutation)
	{
		CommitAnimationFramePropertyEdit();
		if (!GodotObject.IsInstanceValid(animation) || mutation == null)
		{
			return;
		}
		Dictionary dictionary = animation.CaptureFrameEditSnapshot();
		var (flag, text) = mutation(animation);
		if (!flag)
		{
			SetAnimationFrameStatus(string.IsNullOrWhiteSpace(text) ? "无法修改选中帧状态。" : text, error: true);
			return;
		}
		Dictionary nextSnapshot = animation.CaptureFrameEditSnapshot();
		if (!animation.TryRestoreFrameEditSnapshot(dictionary, out var error))
		{
			SetAnimationFrameStatus("无法建立撤销历史：" + error, error: true);
			return;
		}
		int undoFrame = (GodotObject.IsInstanceValid(_animationFrameTimeline) ? _animationFrameTimeline.SelectedFrame : doFrame);
		int undoSliceKey = (GodotObject.IsInstanceValid(_animationFrameTimeline) ? _animationFrameTimeline.SelectedSliceKey : doSliceKey);
		CommitAnimationFrameSnapshotChange(animation, dictionary, nextSnapshot, doFrame, doSliceKey, undoFrame, undoSliceKey, actionName);
	}

	private void CommitAnimationFrameSnapshotChange(AdobeAnimateData animation, Dictionary oldSnapshot, Dictionary nextSnapshot, int doFrame, int doSliceKey, int undoFrame, int undoSliceKey, string actionName)
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null)
		{
			animation.ApplyFrameEditSnapshot(nextSnapshot);
			RefreshAnimationFrameEditorFromHistory(animation, doFrame, doSliceKey);
			return;
		}
		xWUndoRedoManager.CreateAction(actionName);
		xWUndoRedoManager.AddDoMethod(animation, "ApplyFrameEditSnapshot", Variant.From(in nextSnapshot));
		xWUndoRedoManager.AddDoMethod(this, "RefreshAnimationFrameEditorFromHistory", Variant.From(in animation), doFrame, doSliceKey);
		xWUndoRedoManager.AddUndoMethod(this, "RefreshAnimationFrameEditorFromHistory", Variant.From(in animation), undoFrame, undoSliceKey);
		xWUndoRedoManager.AddUndoMethod(animation, "ApplyFrameEditSnapshot", Variant.From(in oldSnapshot));
		xWUndoRedoManager.CommitAction();
	}

	public void RefreshAnimationFrameEditorFromHistory(AdobeAnimateData animation, int frame, int sliceKey)
	{
		if (GodotObject.IsInstanceValid(animation) && CurrentResource == animation)
		{
			_animationFrameMutationCount++;
			SyncAnimationFrameScaleControl(animation);
			PopulateAnimationFrameOptions(animation);
			_animationFrameTimeline.RefreshFromAnimation();
			bool flag = frame >= 0 && sliceKey >= 0 && _animationFrameTimeline.SelectSliceKey(frame, sliceKey, notify: false);
			if (!flag)
			{
				flag = TrySelectAnimationSliceByKey(frame, sliceKey);
			}
			if (!flag)
			{
				flag = _animationFrameTimeline.SelectFirstOccupiedCell(notify: false);
			}
			if (flag)
			{
				RefreshSelectedAnimationFrameControls(_animationFrameTimeline.SelectedFrame, _animationFrameTimeline.SelectedSliceKey);
				RefreshAnimationRuntimeAtFrame(_animationFrameTimeline.SelectedFrame);
			}
			else
			{
				SetAnimationFrameControlsEnabled(enabled: false);
			}
			UpdateAnimationDirectSummary();
			NotifyCurrentResourceEdited();
		}
	}

	private bool TrySelectAnimationSliceByKey(int preferredFrame, int sliceKey)
	{
		if (!(CurrentResource is AdobeAnimateData adobeAnimateData) || sliceKey < 0)
		{
			return false;
		}
		for (int i = 0; i < adobeAnimateData.EditableFrameCount; i++)
		{
			int num = preferredFrame - i;
			if (num >= 0 && adobeAnimateData.TryGetFrameSlice(num, sliceKey, out var slice, out var error) && _animationFrameTimeline.SelectKeyframe(num, slice.LayerId, sliceKey, notify: false))
			{
				return true;
			}
			int num2 = preferredFrame + i;
			if (i > 0 && num2 < adobeAnimateData.EditableFrameCount && adobeAnimateData.TryGetFrameSlice(num2, sliceKey, out var slice2, out error) && _animationFrameTimeline.SelectKeyframe(num2, slice2.LayerId, sliceKey, notify: false))
			{
				return true;
			}
		}
		return false;
	}

	private bool TryGetSelectedAnimationFrameSlice(out AdobeAnimateData animation, out int frame, out AdobeAnimateData.FrameSlice slice)
	{
		animation = CurrentResource as AdobeAnimateData;
		frame = (GodotObject.IsInstanceValid(_animationFrameTimeline) ? _animationFrameTimeline.SelectedFrame : (-1));
		int num = (GodotObject.IsInstanceValid(_animationFrameTimeline) ? _animationFrameTimeline.SelectedSliceKey : (-1));
		slice = default;
		string error = "";
		if (!GodotObject.IsInstanceValid(animation) || frame < 0 || num < 0 || !animation.TryGetFrameSlice(frame, num, out slice, out error))
		{
			SetAnimationFrameStatus(string.IsNullOrWhiteSpace(error) ? "请先选择一个菱形帧状态。" : error, error: true);
			return false;
		}
		return true;
	}

	private static bool TryFindNearestAnimationFrameSlice(AdobeAnimateData animation, int targetFrame, int preferredSliceKey, out AdobeAnimateData.FrameSlice slice)
	{
		slice = default;
		if (!GodotObject.IsInstanceValid(animation) || animation.EditableFrameCount <= 0)
		{
			return false;
		}
		for (int i = 1; i < animation.EditableFrameCount; i++)
		{
			int[] array = new int[2]
			{
				targetFrame - i,
				targetFrame + i
			};
			foreach (int num in array)
			{
				if (num >= 0 && num < animation.EditableFrameCount)
				{
					if (preferredSliceKey >= 0 && animation.TryGetFrameSlice(num, preferredSliceKey, out slice, out var error))
					{
						return true;
					}
					if (preferredSliceKey < 0 && animation.TryGetFrameSlices(num, out var slices, out error) && slices.Length != 0)
					{
						slice = slices[0];
						return true;
					}
				}
			}
		}
		return false;
	}

	private void RefreshAnimationRuntimeAtFrame(int frame)
	{
		if (GodotObject.IsInstanceValid(_runtimePreview))
		{
			_runtimePreview.SetFrameFromSlider(frame);
		}
	}

	private void UpdateAnimationMediaThumbnail(AdobeAnimateData animation, int mediaId)
	{
		if (GodotObject.IsInstanceValid(_animationMediaThumbnail))
		{
			_animationMediaThumbnail.Texture = CreateAnimationMediaThumbnail(animation, mediaId);
		}
	}

	private static Texture2D CreateAnimationMediaThumbnail(AdobeAnimateData animation, int mediaId)
	{
		if (!TryGetAnimationDefinition(animation, out var definition) || !GodotObject.IsInstanceValid(definition.BaseAtlas) || definition.MediaRects == null || mediaId < 0 || mediaId >= definition.MediaRects.Length)
		{
			return null;
		}
		Rect2 region = definition.MediaRects[mediaId];
		if (region.Size.X <= 0f || region.Size.Y <= 0f)
		{
			return null;
		}
		return new AtlasTexture
		{
			Atlas = definition.BaseAtlas,
			Region = region
		};
	}

	private void SetAnimationFrameStatus(string message, bool error = false)
	{
		if (GodotObject.IsInstanceValid(_animationKeyframeEditStatusLabel))
		{
			_animationKeyframeEditStatusLabel.Text = message ?? "";
			_animationKeyframeEditStatusLabel.Modulate = (error ? new Color(1f, 0.42f, 0.34f) : new Color(0.68f, 0.88f, 0.52f));
		}
	}

	private static int GetSelectedOptionMetadata(OptionButton option, int fallback)
	{
		if (!GodotObject.IsInstanceValid(option) || option.Selected < 0 || option.Selected >= option.ItemCount)
		{
			return fallback;
		}
		return option.GetItemMetadata(option.Selected).AsInt32();
	}

	private static void SelectOptionByMetadata(OptionButton option, int requestedId)
	{
		if (!GodotObject.IsInstanceValid(option))
		{
			return;
		}
		for (int i = 0; i < option.ItemCount; i++)
		{
			if (option.GetItemMetadata(i).AsInt32() == requestedId)
			{
				option.Select(i);
				break;
			}
		}
	}

	private static string GetAnimationNameById(Dictionary dictionary, int requestedId, string fallback)
	{
		if (dictionary == null)
		{
			return fallback;
		}
		foreach (Variant key in dictionary.Keys)
		{
			if (dictionary[key].AsInt32() == requestedId)
			{
				return key.AsString();
			}
		}
		return fallback;
	}

	private void BindAnimationDirectEditSurface(VBoxContainer container, AdobeAnimateData animation)
	{
		LineEdit node = container.GetNode<LineEdit>("%ResourceNameEdit");
		SpinBox node2 = container.GetNode<SpinBox>("%FrameRateSpinBox");
		_animationFrameScaleSpinBox = container.GetNode<SpinBox>("%FrameScaleSpinBox");
		SpinBox node3 = container.GetNode<SpinBox>("%FrameMaxSpinBox");
		CheckButton node4 = container.GetNode<CheckButton>("%LocalToSceneCheck");
		Button node5 = container.GetNode<Button>("%ReloadButton");
		_animationResourcePathLabel = container.GetNode<Label>("%ResourcePathLabel");
		_animationDatFileButton = container.GetNode<Button>("%DatFileButton");
		_animationDirectSummaryLabel = container.GetNode<Label>("%DirectSummaryLabel");
		_animationDatFileDialog = container.GetNode<FileDialog>("%DatFileDialog");
		_animationReimportConfirmDialog = container.GetNode<ConfirmationDialog>("%ReimportConfirmDialog");
		_animationPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnAnimationVisualPropertyEdited);
		_animationPropertyBinding.BindText(node, animation, "resource_name", UpdateAnimationDirectSummary);
		_animationPropertyBinding.BindNumber(node2, animation, "frameRate", RefreshRuntimeAnimationPreview);
		BindAnimationFrameScaleControl(animation);
		_animationPropertyBinding.BindNumber(node3, animation, "frameMax", RefreshRuntimeAnimationPreview);
		_animationPropertyBinding.BindToggle(node4, animation, "resource_local_to_scene", UpdateAnimationDirectSummary);
		_animationDatFileButton.Pressed += () =>
		{
			OpenAnimationDatFileDialog(animation);
		};
		node5.Pressed += ReloadAnimationFromDirectSettings;
		_animationDatFileDialog.FileSelected += SelectAnimationDatFile;
		_animationReimportConfirmDialog.Confirmed += ConfirmExplicitAnimationReimport;
		_animationReimportConfirmDialog.Canceled += CancelPendingAnimationReimportConfirmation;
		BindAnimationReplacementSurface(container, animation);
		UpdateAnimationDirectSummary();
	}

	private void BindAnimationFrameScaleControl(AdobeAnimateData animation)
	{
		if (GodotObject.IsInstanceValid(_animationFrameScaleSpinBox) && GodotObject.IsInstanceValid(animation))
		{
			_updatingAnimationFrameScaleControl = true;
			_animationFrameScaleSpinBox.SetValueNoSignal(Math.Max(1, animation.frameScale));
			_updatingAnimationFrameScaleControl = false;
			_animationFrameScaleSpinBox.FocusEntered += BeginAnimationFrameScaleEdit;
			_animationFrameScaleSpinBox.ValueChanged += PreviewAnimationFrameScaleEdit;
			_animationFrameScaleSpinBox.FocusExited += CommitAnimationFrameScaleEdit;
		}
	}

	private void UnbindAnimationFrameScaleControl()
	{
		if (GodotObject.IsInstanceValid(_animationFrameScaleSpinBox))
		{
			_animationFrameScaleSpinBox.FocusEntered -= BeginAnimationFrameScaleEdit;
			_animationFrameScaleSpinBox.ValueChanged -= PreviewAnimationFrameScaleEdit;
			_animationFrameScaleSpinBox.FocusExited -= CommitAnimationFrameScaleEdit;
		}
		_animationFrameScaleSpinBox = null;
		_updatingAnimationFrameScaleControl = false;
	}

	private void BeginAnimationFrameScaleEdit()
	{
		if (!_updatingAnimationFrameScaleControl && CurrentResource is AdobeAnimateData adobeAnimateData)
		{
			SetAnimationFrameStatus($"当前帧缩放为 {Math.Max(1, adobeAnimateData.frameScale)}；修改后将在后台重建 DAT。");
		}
	}

	private void PreviewAnimationFrameScaleEdit(double value)
	{
		if (!_updatingAnimationFrameScaleControl && CurrentResource is AdobeAnimateData adobeAnimateData)
		{
			int num = Math.Max(1, (int)Math.Round(value));
			if (GodotObject.IsInstanceValid(_animationDirectSummaryLabel))
			{
				_animationDirectSummaryLabel.Text = ((num == adobeAnimateData.frameScale) ? $"{GetAnimationFrameCount(adobeAnimateData)} 帧 · {adobeAnimateData.layerDictionary?.Count ?? 0} 图层 · {adobeAnimateData.clips?.Count ?? 0} Clip · {adobeAnimateData.frameRate:0.##} FPS" : $"准备应用 {num}x 帧缩放 · 松开/失焦后在后台重建");
			}
		}
	}

	private void CommitAnimationFrameScaleEdit()
	{
		if (_updatingAnimationFrameScaleControl || !GodotObject.IsInstanceValid(_animationFrameScaleSpinBox) || !(CurrentResource is AdobeAnimateData adobeAnimateData))
		{
			return;
		}
		int num = Math.Max(1, (int)Math.Round(_animationFrameScaleSpinBox.Value));
		_updatingAnimationFrameScaleControl = true;
		_animationFrameScaleSpinBox.SetValueNoSignal(num);
		_updatingAnimationFrameScaleControl = false;
		if (num == Math.Max(1, adobeAnimateData.frameScale))
		{
			UpdateAnimationDirectSummary();
			return;
		}
		string text = ResolveCompanionDatPath(adobeAnimateData);
		if (string.IsNullOrWhiteSpace(text) || !Godot.FileAccess.FileExists(text))
		{
			SyncAnimationFrameScaleControl(adobeAnimateData);
			SetAnimationFrameStatus("无法修改帧缩放：未找到可用于重建的 DAT。", error: true);
		}
		else if (adobeAnimateData.HasAuthoredPackedFrames)
		{
			RequestAnimationFrameScaleConfirmation(adobeAnimateData, num);
		}
		else
		{
			StartAnimationFrameScaleHydration(adobeAnimateData, num);
		}
	}

	private void RequestAnimationFrameScaleConfirmation(AdobeAnimateData animation, int requestedScale)
	{
		if (!GodotObject.IsInstanceValid(animation) || animation != CurrentResource || !GodotObject.IsInstanceValid(_animationReimportConfirmDialog))
		{
			SyncAnimationFrameScaleControl(animation);
			return;
		}
		_pendingAnimationReimportTarget = animation;
		_pendingAnimationReimportPath = animation.animeFile ?? "";
		_pendingAnimationFrameScale = requestedScale;
		_animationReimportConfirmDialog.DialogText = $"当前动画含有手工帧编辑。\n\n帧缩放：{animation.frameScale}x → {requestedScale}x\n\n" + "继续会在后台从 DAT 重建帧、Clip 与事件；现有手工帧会先保存为完整快照，可由全局 Undo/Redo 无损恢复。";
		_animationReimportConfirmDialog.PopupCentered(new Vector2I(620, 280));
		SetAnimationFrameStatus("等待确认帧缩放；当前手工帧尚未改变。");
	}

	private void StartAnimationFrameScaleHydration(AdobeAnimateData animation, int requestedScale)
	{
		if (GodotObject.IsInstanceValid(animation) && animation == CurrentResource)
		{
			CancelExplicitAnimationReimport(restoreOldSnapshot: true);
			CancelAnimationResourceHydration();
			_animationExplicitReimportOldSnapshot = animation.CaptureFullFrameEditSnapshot();
			_animationExplicitReimportFrame = (GodotObject.IsInstanceValid(_animationFrameTimeline) ? _animationFrameTimeline.SelectedFrame : 0);
			_animationExplicitReimportSliceKey = (GodotObject.IsInstanceValid(_animationFrameTimeline) ? _animationFrameTimeline.SelectedSliceKey : (-1));
			_animationExplicitReimportActionName = "修改动画帧缩放";
			_animationExplicitReimportSuccessMessage = $"帧缩放已在后台重建为 {requestedScale}x，可使用全局 Undo 恢复。";
			_animationExplicitReimportRequiresRestore = false;
			StartAnimationResourceHydration(restart: true, requestedScale);
			SetAnimationFrameStatus($"正在后台重建 {requestedScale}x 帧缩放；主界面可继续操作。");
		}
	}

	private void SyncAnimationFrameScaleControl(AdobeAnimateData animation)
	{
		if (GodotObject.IsInstanceValid(animation) && GodotObject.IsInstanceValid(_animationFrameScaleSpinBox))
		{
			_updatingAnimationFrameScaleControl = true;
			_animationFrameScaleSpinBox.SetValueNoSignal(Math.Max(1, animation.frameScale));
			_updatingAnimationFrameScaleControl = false;
			UpdateAnimationDirectSummary();
		}
	}

	private void BindAnimationReplacementSurface(VBoxContainer container, AdobeAnimateData animation)
	{
		_animationReplacementSlotCards = container.GetNode<HFlowContainer>("%ReplacementSlotCards");
		_animationExtraTextureCards = container.GetNode<HFlowContainer>("%ExtraTextureCards");
		_animationReplacementSummaryLabel = container.GetNode<Label>("%ReplacementSummaryLabel");
		_animationReplacementStatusLabel = container.GetNode<Label>("%ReplacementStatusLabel");
		_animationAddReplacementSlotButton = container.GetNode<Button>("%AddReplacementSlotButton");
		_animationAddExtraTextureButton = container.GetNode<Button>("%AddExtraTextureButton");
		_animationReplacementSaveButton = container.GetNode<Button>("%ReplacementSaveButton");
		_animationReplacementReloadButton = container.GetNode<Button>("%ReplacementReloadButton");
		_animationReplacementReloadConfirmDialog = container.GetNode<ConfirmationDialog>("%ReplacementReloadConfirmDialog");
		_animationReplacementMutationCount = 0;
		_animationAddReplacementSlotButton.Pressed += OpenAnimationReplacementSlotPicker;
		_animationAddExtraTextureButton.Pressed += OpenAnimationExtraTexturePicker;
		_animationReplacementSaveButton.Pressed += SaveCurrentResource;
		_animationReplacementReloadButton.Pressed += RequestAnimationReplacementReload;
		_animationReplacementReloadConfirmDialog.Confirmed += ReloadAnimationReplacementConfiguration;
		RebuildAnimationReplacementWorkbench(animation);
	}

	private void DisposeAnimationReplacementSurface()
	{
		if (GodotObject.IsInstanceValid(_animationAddReplacementSlotButton))
		{
			_animationAddReplacementSlotButton.Pressed -= OpenAnimationReplacementSlotPicker;
		}
		if (GodotObject.IsInstanceValid(_animationAddExtraTextureButton))
		{
			_animationAddExtraTextureButton.Pressed -= OpenAnimationExtraTexturePicker;
		}
		if (GodotObject.IsInstanceValid(_animationReplacementSaveButton))
		{
			_animationReplacementSaveButton.Pressed -= SaveCurrentResource;
		}
		if (GodotObject.IsInstanceValid(_animationReplacementReloadButton))
		{
			_animationReplacementReloadButton.Pressed -= RequestAnimationReplacementReload;
		}
		if (GodotObject.IsInstanceValid(_animationReplacementReloadConfirmDialog))
		{
			_animationReplacementReloadConfirmDialog.Confirmed -= ReloadAnimationReplacementConfiguration;
		}
		if (GodotObject.IsInstanceValid(_animationReplacementPicker))
		{
			_animationReplacementPicker.Dismiss();
			_animationReplacementPicker.QueueFree();
		}
		_animationReplacementSlotCards = null;
		_animationExtraTextureCards = null;
		_animationReplacementSummaryLabel = null;
		_animationReplacementStatusLabel = null;
		_animationAddReplacementSlotButton = null;
		_animationAddExtraTextureButton = null;
		_animationReplacementSaveButton = null;
		_animationReplacementReloadButton = null;
		_animationReplacementReloadConfirmDialog = null;
		_animationReplacementPicker = null;
		_animationReplacementSlotCardCount = 0;
		_animationExtraTextureCardCount = 0;
	}

	private void EnsureAnimationReplacementPicker()
	{
		if (!GodotObject.IsInstanceValid(_animationReplacementPicker))
		{
			_animationReplacementPicker = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_animationReplacementPicker))
			{
				AddChild(_animationReplacementPicker, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void OpenAnimationReplacementSlotPicker()
	{
		if (!(CurrentResource is AdobeAnimateData adobeAnimateData))
		{
			return;
		}
		EnsureAnimationReplacementPicker();
		if (!GodotObject.IsInstanceValid(_animationReplacementPicker))
		{
			return;
		}
		List<XWGameplayResourceChoice> list = new List<XWGameplayResourceChoice>();
		if (adobeAnimateData.mediaDictionary != null)
		{
			List<(int, string)> list2 = new List<(int, string)>();
			foreach (Variant key in adobeAnimateData.mediaDictionary.Keys)
			{
				string text = key.AsString();
				if (!ContainsAnimationReplacementSlot(adobeAnimateData.replaceSlotDictionary, text))
				{
					list2.Add((adobeAnimateData.mediaDictionary[key].AsInt32(), text));
				}
			}
			list2.Sort(((int Id, string Name) left, (int Id, string Name) right) => left.Id.CompareTo(right.Id));
			foreach (var (num, text2) in list2)
			{
				list.Add(new XWGameplayResourceChoice(text2, text2, $"media://{num}", CreateAnimationMediaThumbnail(adobeAnimateData, num), XWGameplayResourceKind.Resource, IsModResource: false));
			}
		}
		if (list.Count == 0)
		{
			SetAnimationReplacementStatus("所有可用动画媒体都已建立替换槽位。", error: false);
			return;
		}
		_animationReplacementPicker.OpenChoices("动画媒体缩略图库", "按原始媒体画面选择需要开放给 Mod 的替换槽位", "", list, (XWGameplayResourceChoice choice) =>
		{
			AddAnimationReplacementSlot(choice.Key);
		}, "创建替换槽位");
	}

	private void OpenAnimationExtraTexturePicker()
	{
		OpenAnimationExtraTexturePicker(-1);
	}

	private void OpenAnimationExtraTexturePicker(int replacementIndex)
	{
		if (!(CurrentResource is AdobeAnimateData adobeAnimateData))
		{
			return;
		}
		EnsureAnimationReplacementPicker();
		if (GodotObject.IsInstanceValid(_animationReplacementPicker))
		{
			string currentPath = ((replacementIndex >= 0 && replacementIndex < (adobeAnimateData.extraMediaReplaceTexturePaths?.Count ?? 0)) ? adobeAnimateData.extraMediaReplaceTexturePaths[replacementIndex] : "");
			string text = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
			if (string.IsNullOrWhiteSpace(text) && XWEditorInterface.Instance?.GetEditorPanel() is ModEditorPanel modEditorPanel)
			{
				text = modEditorPanel.GetCurrentProject()?.ProjectPath ?? "";
			}
			_animationReplacementPicker.OpenResourceLibrary("Texture", "动画替换纹理", currentPath, text, new string[1] { "Texture2D" }, new string[2] { "Asset/Anime", "Asset/Texture" }, "res://addons/ModEditor/Icons/ResourceAnimation.svg", (XWGameplayResourceChoice choice) =>
			{
				SelectAnimationExtraTexture(choice.ResourcePath, replacementIndex);
			});
		}
	}

	private void SelectAnimationExtraTexture(string path, int replacementIndex)
	{
		if (!GodotObject.IsInstanceValid(LoadAnimationReplacementTexture(path)))
		{
			SetAnimationReplacementStatus("无法加载纹理：" + path, error: true);
		}
		else if (replacementIndex >= 0)
		{
			ReplaceAnimationExtraTexturePath(replacementIndex, path);
		}
		else
		{
			AddAnimationExtraTexturePath(path);
		}
	}

	private static Texture2D LoadAnimationReplacementTexture(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		if (ResourceLoader.Exists(path))
		{
			try
			{
				Texture2D texture2D = ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse);
				if (GodotObject.IsInstanceValid(texture2D))
				{
					return texture2D;
				}
			}
			catch (Exception ex)
			{
				GD.PushWarning($"Animation replacement texture load failed: {path} ({ex.Message})");
			}
		}
		string text = path.GetExtension().ToLowerInvariant();
		if (text != null)
		{
			int length = text.Length;
			if (length != 3)
			{
				if (length == 4)
				{
					char c = text[0];
					if (c != 'j')
					{
						if (c == 'w' && text == "webp")
						{
							goto IL_0166;
						}
					}
					else if (text == "jpeg")
					{
						goto IL_0166;
					}
				}
			}
			else
			{
				switch (text[0])
				{
				case 'p':
					break;
				case 'j':
					goto IL_010e;
				case 's':
					goto IL_011d;
				case 'b':
					goto IL_012c;
				case 't':
					goto IL_013b;
				default:
					goto IL_016b;
				}
				if (text == "png")
				{
					goto IL_0166;
				}
			}
		}
		goto IL_016b;
		IL_016e:
		bool flag;
		if (!flag)
		{
			return null;
		}
		string path2 = ((path.StartsWith("res://") || path.StartsWith("user://")) ? ProjectSettings.GlobalizePath(path) : path.Replace('\\', '/'));
		if (!Godot.FileAccess.FileExists(path2))
		{
			return null;
		}
		Image image = new Image();
		if (((text == "svg") ? image.LoadSvgFromBuffer(Godot.FileAccess.GetFileAsBytes(path2)) : image.Load(path2)) != Error.Ok || image.GetWidth() <= 0 || image.GetHeight() <= 0)
		{
			return null;
		}
		ImageTexture imageTexture = ImageTexture.CreateFromImage(image);
		imageTexture.ResourcePath = path;
		return imageTexture;
		IL_013b:
		if (text == "tga")
		{
			goto IL_0166;
		}
		goto IL_016b;
		IL_010e:
		if (text == "jpg")
		{
			goto IL_0166;
		}
		goto IL_016b;
		IL_011d:
		if (text == "svg")
		{
			goto IL_0166;
		}
		goto IL_016b;
		IL_016b:
		flag = false;
		goto IL_016e;
		IL_0166:
		flag = true;
		goto IL_016e;
		IL_012c:
		if (text == "bmp")
		{
			goto IL_0166;
		}
		goto IL_016b;
	}

	public bool AddAnimationReplacementSlot(string mediaName)
	{
		if (_animationPropertyBinding == null || !(CurrentResource is AdobeAnimateData adobeAnimateData) || string.IsNullOrWhiteSpace(mediaName) || adobeAnimateData.mediaDictionary == null || !TryGetAnimationDictionaryValue(adobeAnimateData.mediaDictionary, mediaName, out var value) || ContainsAnimationReplacementSlot(adobeAnimateData.replaceSlotDictionary, mediaName))
		{
			return false;
		}
		Dictionary from = adobeAnimateData.replaceSlotDictionary?.Duplicate(deep: true) ?? new Dictionary();
		from[mediaName] = value;
		_animationPropertyBinding.SetValue(adobeAnimateData, "replaceSlotDictionary", Variant.From(in from), "添加动画媒体替换槽位", this, "RefreshAnimationReplacementWorkbenchFromHistory");
		SetAnimationReplacementStatus("已开放媒体槽位：" + mediaName, error: false);
		return true;
	}

	public bool RemoveAnimationReplacementSlot(string mediaName)
	{
		if (_animationPropertyBinding == null || !(CurrentResource is AdobeAnimateData adobeAnimateData) || !TryFindAnimationDictionaryKey(adobeAnimateData.replaceSlotDictionary, mediaName, out var matchedKey))
		{
			return false;
		}
		Dictionary from = adobeAnimateData.replaceSlotDictionary.Duplicate(deep: true);
		from.Remove(matchedKey);
		_animationPropertyBinding.SetValue(adobeAnimateData, "replaceSlotDictionary", Variant.From(in from), "删除动画媒体替换槽位", this, "RefreshAnimationReplacementWorkbenchFromHistory");
		SetAnimationReplacementStatus("已移除媒体槽位：" + mediaName, error: false);
		return true;
	}

	public bool AddAnimationExtraTexture(Texture2D texture)
	{
		if (GodotObject.IsInstanceValid(texture))
		{
			return AddAnimationExtraTexturePath(texture.ResourcePath);
		}
		return false;
	}

	public bool ReplaceAnimationExtraTexture(int index, Texture2D texture)
	{
		if (GodotObject.IsInstanceValid(texture))
		{
			return ReplaceAnimationExtraTexturePath(index, texture.ResourcePath);
		}
		return false;
	}

	private bool AddAnimationExtraTexturePath(string texturePath)
	{
		if (_animationPropertyBinding == null || !(CurrentResource is AdobeAnimateData adobeAnimateData) || string.IsNullOrWhiteSpace(texturePath))
		{
			return false;
		}
		Array<string> array = DuplicateAnimationExtraTexturePaths(adobeAnimateData.extraMediaReplaceTexturePaths);
		array.Add(texturePath);
		CommitAnimationExtraTexturePaths(adobeAnimateData, array, "添加动画附加替换纹理");
		SetAnimationReplacementStatus("已加入替换纹理：" + texturePath.GetFile(), error: false);
		return true;
	}

	private bool ReplaceAnimationExtraTexturePath(int index, string texturePath)
	{
		if (_animationPropertyBinding == null || !(CurrentResource is AdobeAnimateData adobeAnimateData) || string.IsNullOrWhiteSpace(texturePath) || index < 0 || index >= (adobeAnimateData.extraMediaReplaceTexturePaths?.Count ?? 0))
		{
			return false;
		}
		Array<string> array = DuplicateAnimationExtraTexturePaths(adobeAnimateData.extraMediaReplaceTexturePaths);
		array[index] = texturePath;
		CommitAnimationExtraTexturePaths(adobeAnimateData, array, "更换动画附加替换纹理");
		SetAnimationReplacementStatus($"已更换第 {index + 1} 张替换纹理。", error: false);
		return true;
	}

	public bool RemoveAnimationExtraTexture(int index)
	{
		if (_animationPropertyBinding == null || !(CurrentResource is AdobeAnimateData adobeAnimateData) || index < 0 || index >= (adobeAnimateData.extraMediaReplaceTexturePaths?.Count ?? 0))
		{
			return false;
		}
		Array<string> array = DuplicateAnimationExtraTexturePaths(adobeAnimateData.extraMediaReplaceTexturePaths);
		array.RemoveAt(index);
		CommitAnimationExtraTexturePaths(adobeAnimateData, array, "删除动画附加替换纹理");
		SetAnimationReplacementStatus($"已移除第 {index + 1} 张替换纹理。", error: false);
		return true;
	}

	private void CommitAnimationExtraTexturePaths(AdobeAnimateData animation, Array<string> next, string actionName)
	{
		_animationPropertyBinding.SetValue(animation, "extraMediaReplaceTexturePaths", Variant.From(in next), actionName, this, "RefreshAnimationReplacementWorkbenchFromHistory");
	}

	private static Array<string> DuplicateAnimationExtraTexturePaths(Array<string> source)
	{
		if (source != null)
		{
			return source.Duplicate();
		}
		return new Array<string>();
	}

	public void RefreshAnimationReplacementWorkbenchFromHistory()
	{
		if (CurrentResource is AdobeAnimateData adobeAnimateData)
		{
			_animationReplacementMutationCount++;
			AdobeAnimateDefinitionCache.Invalidate(adobeAnimateData);
			adobeAnimateData.EmitChanged();
			RebuildAnimationReplacementWorkbench(adobeAnimateData);
			if (GodotObject.IsInstanceValid(_runtimePreview))
			{
				RefreshAnimationRuntimeAtFrame(_runtimePreview.CurrentFrame);
			}
		}
	}

	private void RebuildAnimationReplacementWorkbench(AdobeAnimateData animation)
	{
		if (!GodotObject.IsInstanceValid(animation) || !GodotObject.IsInstanceValid(_animationReplacementSlotCards) || !GodotObject.IsInstanceValid(_animationExtraTextureCards))
		{
			return;
		}
		ClearDynamicAnimationCards(_animationReplacementSlotCards);
		ClearDynamicAnimationCards(_animationExtraTextureCards);
		_animationReplacementSlotCardCount = animation.replaceSlotDictionary?.Count ?? 0;
		_animationExtraTextureCardCount = animation.extraMediaReplaceTexturePaths?.Count ?? 0;
		if (GodotObject.IsInstanceValid(_animationReplacementSummaryLabel))
		{
			_animationReplacementSummaryLabel.Text = $"{_animationReplacementSlotCardCount} 个媒体槽位 · {_animationExtraTextureCardCount} 张附加纹理 · {animation.mediaDictionary?.Count ?? 0} 个原始媒体";
		}
		if (_animationReplacementSlotCardCount == 0)
		{
			AddAnimationReplacementEmptyCard(_animationReplacementSlotCards, "＋", "选择原始媒体缩略图", "为图层开放可替换入口");
		}
		else
		{
			foreach (Variant key in animation.replaceSlotDictionary.Keys)
			{
				AddAnimationReplacementSlotCard(_animationReplacementSlotCards, animation, key.AsString());
			}
		}
		if (_animationExtraTextureCardCount == 0)
		{
			AddAnimationReplacementEmptyCard(_animationExtraTextureCards, "▧", "附加替换纹理库为空", "可选；支持内置资源与 Mod 图片");
			return;
		}
		for (int i = 0; i < animation.extraMediaReplaceTexturePaths.Count; i++)
		{
			AddAnimationExtraTextureCard(_animationExtraTextureCards, animation.extraMediaReplaceTexturePaths[i], i);
		}
	}

	private void AddAnimationReplacementSlotCard(Control host, AdobeAnimateData animation, string mediaName)
	{
		bool flag = TryGetAnimationDictionaryValue(animation.mediaDictionary, mediaName, out var value);
		int num = (flag ? value.AsInt32() : (-1));
		XWAnimationReplacementCard xWAnimationReplacementCard = InstantiateAnimationReplacementCard(host);
		if (GodotObject.IsInstanceValid(xWAnimationReplacementCard))
		{
			xWAnimationReplacementCard.Name = $"ReplacementSlotCard{Math.Max(0, num)}";
			xWAnimationReplacementCard.BindSlot(mediaName, num, CreateAnimationMediaThumbnail(animation, num), flag ? BuildAnimationMediaLayerSummary(animation, num) : "", flag, (string name) =>
			{
				RemoveAnimationReplacementSlot(name);
			});
		}
	}

	private void AddAnimationExtraTextureCard(Control host, string texturePath, int index)
	{
		XWAnimationReplacementCard xWAnimationReplacementCard = InstantiateAnimationReplacementCard(host);
		if (GodotObject.IsInstanceValid(xWAnimationReplacementCard))
		{
			xWAnimationReplacementCard.Name = $"ExtraTextureCard{index}";
			Texture2D texture = LoadAnimationReplacementTexture(texturePath);
			xWAnimationReplacementCard.BindTexture(texture, index, FormatAnimationTextureName(texture, texturePath, index), texturePath, OpenAnimationExtraTexturePicker, (int textureIndex) =>
			{
				RemoveAnimationExtraTexture(textureIndex);
			});
		}
	}

	private static void AddAnimationReplacementEmptyCard(Control host, string glyph, string title, string subtitle)
	{
		XWAnimationReplacementCard xWAnimationReplacementCard = InstantiateAnimationReplacementCard(host);
		if (GodotObject.IsInstanceValid(xWAnimationReplacementCard))
		{
			xWAnimationReplacementCard.BindEmpty(glyph, title, subtitle);
		}
	}

	private static XWAnimationReplacementCard InstantiateAnimationReplacementCard(Control host)
	{
		if (_animationReplacementCardScene == null)
		{
			_animationReplacementCardScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationReplacementCard.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		XWAnimationReplacementCard xWAnimationReplacementCard = _animationReplacementCardScene?.Instantiate<XWAnimationReplacementCard>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(xWAnimationReplacementCard) || !GodotObject.IsInstanceValid(host))
		{
			return null;
		}
		host.AddChild(xWAnimationReplacementCard, forceReadableName: false, InternalMode.Disabled);
		return xWAnimationReplacementCard;
	}

	private static void ClearDynamicAnimationCards(Control host)
	{
		foreach (Node child in host.GetChildren())
		{
			host.RemoveChild(child);
			child.QueueFree();
		}
	}

	private static string FormatAnimationTextureName(Texture2D texture, string texturePath, int index)
	{
		if (GodotObject.IsInstanceValid(texture) && !string.IsNullOrWhiteSpace(texture.ResourceName))
		{
			return texture.ResourceName;
		}
		if (!string.IsNullOrWhiteSpace(texturePath))
		{
			return texturePath.GetFile();
		}
		return $"无效纹理 {index + 1}";
	}

	private static bool ContainsAnimationReplacementSlot(Dictionary dictionary, string mediaName)
	{
		Variant matchedKey;
		return TryFindAnimationDictionaryKey(dictionary, mediaName, out matchedKey);
	}

	private static bool TryFindAnimationDictionaryKey(Dictionary dictionary, string requestedName, out Variant matchedKey)
	{
		matchedKey = default;
		if (dictionary == null)
		{
			return false;
		}
		foreach (Variant key in dictionary.Keys)
		{
			if (string.Equals(key.AsString(), requestedName, StringComparison.Ordinal))
			{
				matchedKey = key;
				return true;
			}
		}
		return false;
	}

	private static bool TryGetAnimationDictionaryValue(Dictionary dictionary, string requestedName, out Variant value)
	{
		value = default;
		if (!TryFindAnimationDictionaryKey(dictionary, requestedName, out var matchedKey))
		{
			return false;
		}
		value = dictionary[matchedKey];
		return true;
	}

	private static string BuildAnimationMediaLayerSummary(AdobeAnimateData animation, int mediaId)
	{
		if (animation?.sliceMediaIds == null || animation.sliceLayerIds == null)
		{
			return "尚未被关键帧使用";
		}
		int num = Math.Min(animation.sliceMediaIds.Length, animation.sliceLayerIds.Length);
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 0; i < num; i++)
		{
			if (animation.sliceMediaIds[i] == mediaId)
			{
				hashSet.Add(animation.sliceLayerIds[i]);
			}
		}
		if (hashSet.Count == 0)
		{
			return "尚未被关键帧使用";
		}
		List<string> list = new List<string>();
		foreach (int item in hashSet)
		{
			list.Add(GetAnimationNameById(animation.layerDictionary, item, $"图层 {item + 1}"));
			if (list.Count >= 3)
			{
				break;
			}
		}
		string text = ((hashSet.Count > list.Count) ? $" +{hashSet.Count - list.Count}" : "");
		return "图层：" + string.Join(" / ", list) + text;
	}

	private void RequestAnimationReplacementReload()
	{
		if (GodotObject.IsInstanceValid(_animationReplacementReloadConfirmDialog))
		{
			_animationReplacementReloadConfirmDialog.PopupCentered(new Vector2I(560, 220));
		}
	}

	public void ReloadAnimationReplacementConfiguration()
	{
		string currentResourcePath = CurrentResourcePath;
		if (string.IsNullOrWhiteSpace(currentResourcePath) || !ResourceLoader.Exists(currentResourcePath))
		{
			SetAnimationReplacementStatus("当前动画尚未保存，无法从磁盘重载。", error: true);
			return;
		}
		AdobeAnimateData adobeAnimateData = ResourceLoader.Load<AdobeAnimateData>(currentResourcePath, "", ResourceLoader.CacheMode.Replace);
		if (!GodotObject.IsInstanceValid(adobeAnimateData))
		{
			SetAnimationReplacementStatus("动画资源重载失败：" + currentResourcePath, error: true);
			return;
		}
		LoadResource(adobeAnimateData, currentResourcePath, CurrentDescriptor);
		SetAnimationReplacementStatus("已从磁盘重载动画与替换配置。", error: false);
	}

	private void SetAnimationReplacementStatus(string message, bool error)
	{
		if (GodotObject.IsInstanceValid(_animationReplacementStatusLabel))
		{
			_animationReplacementStatusLabel.Text = message ?? "";
			_animationReplacementStatusLabel.Modulate = (error ? new Color(1f, 0.42f, 0.34f) : new Color(0.66f, 0.9f, 0.53f));
		}
	}

	private void OnAnimationVisualPropertyEdited(bool committed)
	{
		if (GodotObject.IsInstanceValid(CurrentResource))
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
				return;
			}
			MarkCurrentResourceDirty();
			CurrentResource.EmitChanged();
		}
	}

	private void OpenAnimationDatFileDialog(AdobeAnimateData animation)
	{
		if (GodotObject.IsInstanceValid(_animationDatFileDialog) && animation == CurrentResource)
		{
			_animationDatSelectionTarget = animation;
			string text = ResolveCompanionDatPath(animation);
			if (!string.IsNullOrWhiteSpace(text))
			{
				string instance = (text.StartsWith("res://") ? NormalizePath(ProjectSettings.GlobalizePath(text)) : NormalizePath(text));
				_animationDatFileDialog.CurrentDir = instance.GetBaseDir();
				_animationDatFileDialog.CurrentFile = instance.GetFile();
			}
			_animationDatFileDialog.PopupCenteredClamped(new Vector2I(920, 620), 0.9f);
		}
	}

	private void SelectAnimationDatFile(string selectedPath)
	{
		if (_animationPropertyBinding == null)
		{
			return;
		}
		AdobeAnimateData animationDatSelectionTarget = _animationDatSelectionTarget;
		if (animationDatSelectionTarget != null && animationDatSelectionTarget == CurrentResource && !string.IsNullOrWhiteSpace(selectedPath))
		{
			string text = ToStoredAnimationDatPath(selectedPath);
			if (animationDatSelectionTarget.HasAuthoredPackedFrames)
			{
				RequestExplicitAnimationReimport(animationDatSelectionTarget, text);
				_animationDatSelectionTarget = null;
			}
			else
			{
				_animationPropertyBinding.SetValue(animationDatSelectionTarget, "animeFile", text, "选择动画 DAT", this, "ReloadAnimationFromDirectSettings");
				_animationDatSelectionTarget = null;
				UpdateAnimationDirectSummary();
			}
		}
	}

	private string ToStoredAnimationDatPath(string selectedPath)
	{
		string text = NormalizePath(selectedPath);
		string text2 = NormalizePath(CurrentResourcePath);
		if (text2.StartsWith("res://"))
		{
			text2 = NormalizePath(ProjectSettings.GlobalizePath(text2));
		}
		string baseDir = text2.GetBaseDir();
		if (!string.IsNullOrWhiteSpace(baseDir) && string.Equals(text.GetBaseDir(), baseDir, StringComparison.OrdinalIgnoreCase))
		{
			return text.GetFile();
		}
		string text3 = NormalizePath(ProjectSettings.GlobalizePath("res://")).TrimEnd('/');
		if (!string.IsNullOrWhiteSpace(text3) && text.StartsWith(text3 + "/", StringComparison.OrdinalIgnoreCase))
		{
			string text4 = text;
			int num = text3.Length + 1;
			return "res://" + text4.Substring(num, text4.Length - num);
		}
		return text;
	}

	public void ReloadAnimationFromDirectSettings()
	{
		if (!(CurrentResource is AdobeAnimateData adobeAnimateData))
		{
			return;
		}
		string text = ResolveCompanionDatPath(adobeAnimateData);
		if (string.IsNullOrWhiteSpace(text) || !Godot.FileAccess.FileExists(text))
		{
			if (GodotObject.IsInstanceValid(_animationPreviewStatusLabel))
			{
				_animationPreviewStatusLabel.Text = "找不到动画 DAT，请重新选择数据文件。";
			}
			UpdateAnimationDirectSummary();
		}
		else if (adobeAnimateData.HasAuthoredPackedFrames)
		{
			RequestExplicitAnimationReimport(adobeAnimateData, adobeAnimateData.animeFile);
		}
		else
		{
			StartAnimationResourceHydration(restart: true);
			UpdateAnimationDirectSummary();
		}
	}

	private void RequestExplicitAnimationReimport(AdobeAnimateData animation, string storedPath)
	{
		if (GodotObject.IsInstanceValid(animation) && animation == CurrentResource && GodotObject.IsInstanceValid(_animationReimportConfirmDialog))
		{
			_pendingAnimationReimportTarget = animation;
			_pendingAnimationReimportPath = storedPath ?? animation.animeFile ?? "";
			_pendingAnimationFrameScale = -1;
			_animationReimportConfirmDialog.DialogText = "当前动画含有手工帧编辑。\n\n来源：" + _pendingAnimationReimportPath + "\n\n重新导入会覆盖手工帧，但会先建立包含媒体、图层、Clip、事件与全部 Packed 数组的完整快照，可由全局 Undo/Redo 无损恢复。";
			_animationReimportConfirmDialog.PopupCentered(new Vector2I(620, 280));
		}
	}

	private void CancelPendingAnimationReimportConfirmation()
	{
		AdobeAnimateData pendingAnimationReimportTarget = _pendingAnimationReimportTarget;
		_pendingAnimationReimportTarget = null;
		_pendingAnimationReimportPath = "";
		_pendingAnimationFrameScale = -1;
		SyncAnimationFrameScaleControl(pendingAnimationReimportTarget);
		SetAnimationFrameStatus("已取消重新导入，手工帧编辑保持不变。");
	}

	private void ConfirmExplicitAnimationReimport()
	{
		AdobeAnimateData pendingAnimationReimportTarget = _pendingAnimationReimportTarget;
		string pendingAnimationReimportPath = _pendingAnimationReimportPath;
		int pendingAnimationFrameScale = _pendingAnimationFrameScale;
		_pendingAnimationReimportTarget = null;
		_pendingAnimationReimportPath = "";
		_pendingAnimationFrameScale = -1;
		if (GodotObject.IsInstanceValid(pendingAnimationReimportTarget) && pendingAnimationReimportTarget == CurrentResource)
		{
			if (pendingAnimationFrameScale > 0)
			{
				StartAnimationFrameScaleHydration(pendingAnimationReimportTarget, pendingAnimationFrameScale);
				return;
			}
			_animationExplicitReimportOldSnapshot = pendingAnimationReimportTarget.CaptureFullFrameEditSnapshot();
			_animationExplicitReimportFrame = (GodotObject.IsInstanceValid(_animationFrameTimeline) ? _animationFrameTimeline.SelectedFrame : 0);
			_animationExplicitReimportSliceKey = (GodotObject.IsInstanceValid(_animationFrameTimeline) ? _animationFrameTimeline.SelectedSliceKey : (-1));
			_animationExplicitReimportActionName = "重新导入动画 DAT";
			_animationExplicitReimportSuccessMessage = "DAT 已在后台重新导入，可使用全局 Undo 无损恢复手工帧。";
			_animationExplicitReimportRequiresRestore = true;
			pendingAnimationReimportTarget.animeFile = pendingAnimationReimportPath;
			StartAnimationResourceHydration(restart: true);
			SetAnimationFrameStatus("正在后台解析 DAT；当前帧编辑在导入成功并建立 Undo 前不会丢失。");
		}
	}

	private void CancelExplicitAnimationReimport(bool restoreOldSnapshot)
	{
		AdobeAnimateData adobeAnimateData = _animationHydrationTarget ?? _pendingAnimationReimportTarget;
		if (((_animationExplicitReimportOldSnapshot != null) & restoreOldSnapshot) && _animationExplicitReimportRequiresRestore && GodotObject.IsInstanceValid(adobeAnimateData))
		{
			adobeAnimateData.TryRestoreFrameEditSnapshot(_animationExplicitReimportOldSnapshot, out var _);
		}
		_animationExplicitReimportOldSnapshot = null;
		_animationExplicitReimportFrame = -1;
		_animationExplicitReimportSliceKey = -1;
		_animationExplicitReimportActionName = "重新导入动画 DAT";
		_animationExplicitReimportSuccessMessage = "DAT 已在后台重新导入，可使用全局 Undo 无损恢复手工帧。";
		_animationExplicitReimportRequiresRestore = false;
		if (restoreOldSnapshot)
		{
			SyncAnimationFrameScaleControl(adobeAnimateData);
		}
	}

	private void RefreshRuntimeAnimationPreview()
	{
		if (GodotObject.IsInstanceValid(_runtimePreview))
		{
			_runtimePreview.SetFrameFromSlider(_runtimePreview.CurrentFrame);
		}
		UpdateAnimationDirectSummary();
	}

	private void UpdateAnimationDirectSummary()
	{
		if (CurrentResource is AdobeAnimateData adobeAnimateData)
		{
			if (GodotObject.IsInstanceValid(_animationResourcePathLabel))
			{
				_animationResourcePathLabel.Text = (string.IsNullOrWhiteSpace(CurrentResourcePath) ? "未保存的动画资源" : NormalizePath(CurrentResourcePath));
				_animationResourcePathLabel.TooltipText = _animationResourcePathLabel.Text;
			}
			if (GodotObject.IsInstanceValid(_animationDatFileButton))
			{
				_animationDatFileButton.Text = (string.IsNullOrWhiteSpace(adobeAnimateData.animeFile) ? "选择 Adobe Animate DAT" : adobeAnimateData.animeFile.GetFile());
				_animationDatFileButton.TooltipText = ResolveCompanionDatPath(adobeAnimateData);
			}
			if (GodotObject.IsInstanceValid(_animationDirectSummaryLabel))
			{
				_animationDirectSummaryLabel.Text = $"{GetAnimationFrameCount(adobeAnimateData)} 帧 · {adobeAnimateData.layerDictionary?.Count ?? 0} 图层 · {adobeAnimateData.clips?.Count ?? 0} Clip · {adobeAnimateData.frameRate:0.##} FPS";
			}
		}
	}

	private async void FitRuntimePreviewAfterLayout(AdobeAnimateInspectorPreview runtimePreview)
	{
		for (int layoutPass = 0; layoutPass < 6; layoutPass++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (!GodotObject.IsInstanceValid(runtimePreview) || !runtimePreview.IsVisibleInTree())
			{
				break;
			}
			ClampRuntimePreviewViewportMinimum(runtimePreview);
			if (MainPanel is ScrollContainer scrollContainer)
			{
				scrollContainer.ScrollHorizontal = 0;
				scrollContainer.ScrollVertical = 0;
			}
			runtimePreview.FitToPanel();
		}
	}

	private static void ClampRuntimePreviewViewportMinimum(AdobeAnimateInspectorPreview runtimePreview)
	{
		SubViewport subViewport = runtimePreview?.GetNodeOrNull<SubViewport>("%PreviewViewport");
		if (GodotObject.IsInstanceValid(subViewport) && subViewport.Size.X > 640)
		{
			subViewport.Size = new Vector2I(640, Math.Clamp(subViewport.Size.Y, 190, 420));
		}
	}

	private void StartAnimationResourceHydration(bool restart = false, int requestedFrameScale = -1)
	{
		string text = NormalizePath(CurrentResourcePath);
		if (CurrentResource is AdobeAnimateData adobeAnimateData && !string.IsNullOrWhiteSpace(text))
		{
			switch (text.GetExtension().ToLowerInvariant())
			{
			case "tres":
			case "res":
			case "dat":
				if (string.IsNullOrWhiteSpace(ResolveCompanionDatPath(adobeAnimateData)))
				{
					break;
				}
				if ((restart || !adobeAnimateData.HasAuthoredPackedFrames) && (restart || _animationHydrationTask == null || _animationHydrationTask.IsCompleted || !(_animationHydrationPath == text) || _animationHydrationTarget != adobeAnimateData))
				{
					CancelAnimationResourceHydration();
					_animationHydrationPath = text;
					_animationHydrationTarget = adobeAnimateData;
					_animationHydrationRequestedFrameScale = ((requestedFrameScale > 0) ? requestedFrameScale : Math.Max(1, adobeAnimateData.frameScale));
					_animationHydrationExpectedFrameScale = Math.Max(1, adobeAnimateData.frameScale);
					_animationHydrationExpectedAuthoringRevision = adobeAnimateData.AuthoringRevision;
					_animationHydrationCancellation = new CancellationTokenSource();
					string sourceDatPath = ResolveCompanionDatPath(adobeAnimateData);
					_animationHydrationTask = adobeAnimateData.BuildEditorDatHydrationAsync(sourceDatPath, _animationHydrationRequestedFrameScale, _animationHydrationCancellation.Token);
					UpdateAnimationProcessEligibility();
					if (GodotObject.IsInstanceValid(_animationPreviewStatusLabel))
					{
						_animationPreviewStatusLabel.Text = "正在后台读取完整动画，编辑器仍可继续操作...";
					}
				}
				return;
			}
		}
		if (GodotObject.IsInstanceValid(_animationPreviewStatusLabel))
		{
			_animationPreviewStatusLabel.Text = "当前只有 DAT 元数据；请先导入为动画资源再预览。";
		}
	}

	private void PollAnimationResourceHydration()
	{
		if (_animationHydrationTask == null || !_animationHydrationTask.IsCompleted)
		{
			return;
		}
		string animationHydrationPath = _animationHydrationPath;
		AdobeAnimateData animationHydrationTarget = _animationHydrationTarget;
		Task<AdobeAnimateData.EditorDatHydrationResult> animationHydrationTask = _animationHydrationTask;
		int animationHydrationRequestedFrameScale = _animationHydrationRequestedFrameScale;
		int animationHydrationExpectedFrameScale = _animationHydrationExpectedFrameScale;
		long animationHydrationExpectedAuthoringRevision = _animationHydrationExpectedAuthoringRevision;
		bool flag = _animationExplicitReimportOldSnapshot != null && GodotObject.IsInstanceValid(animationHydrationTarget);
		_animationHydrationPath = "";
		_animationHydrationTarget = null;
		_animationHydrationTask = null;
		_animationHydrationRequestedFrameScale = -1;
		_animationHydrationExpectedFrameScale = -1;
		_animationHydrationExpectedAuthoringRevision = -1L;
		_animationHydrationCancellation?.Dispose();
		_animationHydrationCancellation = null;
		SetProcess(enable: false);
		if (animationHydrationTask.IsCanceled)
		{
			AbortExplicitAnimationReimport(animationHydrationTarget, "DAT 重新导入已取消。");
			return;
		}
		if (animationHydrationTask.IsFaulted)
		{
			AbortExplicitAnimationReimport(animationHydrationTarget, "DAT 重新导入失败：" + animationHydrationTask.Exception?.GetBaseException().Message);
			if (GodotObject.IsInstanceValid(_animationPreviewStatusLabel))
			{
				_animationPreviewStatusLabel.Text = "完整动画载入失败：" + animationHydrationTask.Exception?.GetBaseException().Message;
			}
			return;
		}
		if (!string.Equals(NormalizePath(CurrentResourcePath), animationHydrationPath, StringComparison.OrdinalIgnoreCase))
		{
			AbortExplicitAnimationReimport(animationHydrationTarget, "动画资源已切换，已恢复重新导入前的帧快照。");
			return;
		}
		if (!(CurrentResource is AdobeAnimateData adobeAnimateData) || adobeAnimateData != animationHydrationTarget)
		{
			AbortExplicitAnimationReimport(animationHydrationTarget, "动画资源已切换，已恢复重新导入前的帧快照。");
			if (GodotObject.IsInstanceValid(_animationPreviewStatusLabel))
			{
				_animationPreviewStatusLabel.Text = "动画资源已切换，已忽略过期的后台读取结果。";
			}
			return;
		}
		if (animationHydrationTarget.frameScale != animationHydrationExpectedFrameScale || animationHydrationTarget.AuthoringRevision != animationHydrationExpectedAuthoringRevision)
		{
			AbortExplicitAnimationReimport(animationHydrationTarget, "动画在后台重建期间又被修改，已忽略过期结果并保留当前内容。");
			return;
		}
		if (!flag && animationHydrationTarget.HasAuthoredPackedFrames)
		{
			if (GodotObject.IsInstanceValid(_animationPreviewStatusLabel))
			{
				_animationPreviewStatusLabel.Text = "后台 DAT 结果已忽略，手工帧编辑保持不变。";
			}
			return;
		}
		AdobeAnimateData.EditorDatHydrationResult result = animationHydrationTask.GetAwaiter().GetResult();
		if (flag && animationHydrationRequestedFrameScale > 0 && result.IsSuccess)
		{
			animationHydrationTarget.SetFrameScaleForDeferredHydration(animationHydrationRequestedFrameScale);
			_animationExplicitReimportRequiresRestore = true;
		}
		if (!result.IsSuccess || !animationHydrationTarget.ApplyEditorDatHydration(result))
		{
			AbortExplicitAnimationReimport(animationHydrationTarget, string.IsNullOrWhiteSpace(result.ErrorMessage) ? "完整动画载入失败，已恢复原帧快照。" : ("完整动画载入失败：" + result.ErrorMessage));
			if (GodotObject.IsInstanceValid(_animationPreviewStatusLabel))
			{
				_animationPreviewStatusLabel.Text = (string.IsNullOrWhiteSpace(result.ErrorMessage) ? "完整动画载入失败，请检查同名 DAT 文件。" : ("完整动画载入失败：" + result.ErrorMessage));
			}
		}
		else if (!animationHydrationTarget.HasPackedRuntimeData())
		{
			AbortExplicitAnimationReimport(animationHydrationTarget, "DAT 未生成可用的 Packed 动画数据，已恢复原帧快照。");
			if (GodotObject.IsInstanceValid(_animationPreviewStatusLabel))
			{
				_animationPreviewStatusLabel.Text = "DAT 未生成可用的 Packed 动画数据。";
			}
		}
		else if (flag)
		{
			animationHydrationTarget.ClearFrameAuthoringFlag();
			Dictionary animationExplicitReimportOldSnapshot = _animationExplicitReimportOldSnapshot;
			Dictionary nextSnapshot = animationHydrationTarget.CaptureFrameEditSnapshot();
			int animationExplicitReimportFrame = _animationExplicitReimportFrame;
			int animationExplicitReimportSliceKey = _animationExplicitReimportSliceKey;
			string animationExplicitReimportActionName = _animationExplicitReimportActionName;
			string animationExplicitReimportSuccessMessage = _animationExplicitReimportSuccessMessage;
			_animationExplicitReimportOldSnapshot = null;
			_animationExplicitReimportFrame = -1;
			_animationExplicitReimportSliceKey = -1;
			_animationExplicitReimportActionName = "重新导入动画 DAT";
			_animationExplicitReimportSuccessMessage = "DAT 已在后台重新导入，可使用全局 Undo 无损恢复手工帧。";
			_animationExplicitReimportRequiresRestore = false;
			if (!animationHydrationTarget.TryRestoreFrameEditSnapshot(animationExplicitReimportOldSnapshot, out var error))
			{
				SetAnimationFrameStatus("无法建立 DAT 重新导入撤销历史：" + error, error: true);
				return;
			}
			CommitAnimationFrameSnapshotChange(animationHydrationTarget, animationExplicitReimportOldSnapshot, nextSnapshot, Math.Clamp(animationExplicitReimportFrame, 0, Math.Max(0, animationHydrationTarget.EditableFrameCount - 1)), animationExplicitReimportSliceKey, animationExplicitReimportFrame, animationExplicitReimportSliceKey, animationExplicitReimportActionName);
			SyncAnimationFrameScaleControl(animationHydrationTarget);
			SetAnimationFrameStatus(animationExplicitReimportSuccessMessage);
		}
		else
		{
			LoadResource(animationHydrationTarget, animationHydrationPath, CurrentDescriptor);
		}
	}

	private void AbortExplicitAnimationReimport(AdobeAnimateData animation, string message)
	{
		if (_animationExplicitReimportOldSnapshot != null)
		{
			if (_animationExplicitReimportRequiresRestore && GodotObject.IsInstanceValid(animation))
			{
				animation.TryRestoreFrameEditSnapshot(_animationExplicitReimportOldSnapshot, out var _);
			}
			_animationExplicitReimportOldSnapshot = null;
			_animationExplicitReimportFrame = -1;
			_animationExplicitReimportSliceKey = -1;
			_animationExplicitReimportActionName = "重新导入动画 DAT";
			_animationExplicitReimportSuccessMessage = "DAT 已在后台重新导入，可使用全局 Undo 无损恢复手工帧。";
			_animationExplicitReimportRequiresRestore = false;
			SyncAnimationFrameScaleControl(animation);
			SetAnimationFrameStatus(message, error: true);
		}
	}

	private void CancelAnimationResourceHydration()
	{
		Task<AdobeAnimateData.EditorDatHydrationResult> animationHydrationTask = _animationHydrationTask;
		if (_animationHydrationCancellation != null)
		{
			_animationHydrationCancellation.Cancel();
			_animationHydrationCancellation.Dispose();
		}
		_animationHydrationCancellation = null;
		_animationHydrationTask = null;
		_animationHydrationTarget = null;
		_animationHydrationPath = "";
		_animationHydrationRequestedFrameScale = -1;
		_animationHydrationExpectedFrameScale = -1;
		_animationHydrationExpectedAuthoringRevision = -1L;
		animationHydrationTask?.ContinueWith((Task<AdobeAnimateData.EditorDatHydrationResult> task) => task.Exception, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
		SetProcess(enable: false);
	}

	private static int GetAnimationFrameCount(AdobeAnimateData animation)
	{
		if (animation == null)
		{
			return 0;
		}
		if (animation.frameMax > 0)
		{
			return animation.frameMax;
		}
		return animation.frameOffsets?.Length ?? 0;
	}

	private void RenderAnimationTimeline(AdobeAnimateData animation)
	{
		AddTimelineRow($"总帧数: {animation.frameMax}");
		AddTimelineRow($"帧率: {animation.frameRate:0.##} fps");
		AddTimelineRow($"帧缩放: {animation.frameScale}");
		if (animation.clips != null && animation.clips.Count > 0)
		{
			foreach (Variant key in animation.clips.Keys)
			{
				AddTimelineRow("Clip " + key.AsString() + ": " + FormatVariant(animation.clips[key]));
			}
		}
		else
		{
			AddTimelineRow("Clip: 未定义");
		}
		int num = 0;
		if (animation.events != null)
		{
			for (int i = 0; i < animation.events.Count; i++)
			{
				Godot.Collections.Array array = animation.events[i];
				if (array != null && array.Count != 0)
				{
					num++;
					AddTimelineRow($"事件帧 {i}: {array.Count} 个事件");
					if (num >= 32)
					{
						AddTimelineRow("还有更多事件帧...");
						break;
					}
				}
			}
		}
		if (num == 0)
		{
			AddTimelineRow("事件: 未定义");
		}
	}

	private static string[] GetAnimationLayerNames(AdobeAnimateData animation)
	{
		if (animation?.layerDictionary == null || animation.layerDictionary.Count == 0)
		{
			return System.Array.Empty<string>();
		}
		string[] array = new string[animation.layerDictionary.Count];
		foreach (Variant key in animation.layerDictionary.Keys)
		{
			int num = animation.layerDictionary[key].AsInt32();
			if (num >= 0 && num < array.Length)
			{
				array[num] = key.AsString();
			}
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (string.IsNullOrEmpty(array[i]))
			{
				array[i] = i.ToString();
			}
		}
		return array;
	}

	private static string[] GetAnimationMediaNames(AdobeAnimateData animation)
	{
		if (animation?.mediaDictionary == null || animation.mediaDictionary.Count == 0)
		{
			return System.Array.Empty<string>();
		}
		int num = -1;
		foreach (Variant key in animation.mediaDictionary.Keys)
		{
			num = Math.Max(num, animation.mediaDictionary[key].AsInt32());
		}
		if (num < 0)
		{
			return System.Array.Empty<string>();
		}
		string[] array = new string[num + 1];
		foreach (Variant key2 in animation.mediaDictionary.Keys)
		{
			int num2 = animation.mediaDictionary[key2].AsInt32();
			if ((uint)num2 < (uint)array.Length)
			{
				array[num2] = key2.AsString();
			}
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (string.IsNullOrWhiteSpace(array[i]))
			{
				array[i] = $"媒体 {i + 1}";
			}
		}
		return array;
	}

	private void RenderAnimationGraph(AdobeAnimateData animation)
	{
		string[] animationLayerNames = GetAnimationLayerNames(animation);
		if (animationLayerNames.Length != 0)
		{
			for (int i = 0; i < animationLayerNames.Length; i++)
			{
				AddGraphRow($"图层 {i + 1}: {animationLayerNames[i]}");
			}
		}
		else
		{
			AddGraphRow("图层: 未定义");
		}
		AddGraphRow($"媒体切片数: {animation.mediaRects?.Length ?? 0}");
		AddGraphRow($"Packed 帧数据: {animation.frameOffsets?.Length ?? 0}");
		AddGraphRow($"Packed 切片数据: {animation.sliceKeys?.Length ?? 0}");
		if (animation.mediaDictionary == null || animation.mediaDictionary.Count <= 0)
		{
			return;
		}
		int num = 0;
		foreach (Variant key in animation.mediaDictionary.Keys)
		{
			AddGraphRow("媒体 " + key.AsString() + " -> " + FormatVariant(animation.mediaDictionary[key]));
			num++;
			if (num >= 48)
			{
				AddGraphRow("还有更多媒体切片...");
				break;
			}
		}
	}

	private void RenderAnimationReferences(AdobeAnimateData animation)
	{
		string text = ResolveCompanionDatPath(animation);
		if (!string.IsNullOrWhiteSpace(text))
		{
			AddReferenceRow("DAT 子资源 -> " + text);
		}
		AdobeAnimateRuntimeDefinition definition;
		if (TryGetSharedAnimationAtlas(animation, out var texture, out var _))
		{
			Vector2 vector = TryGetTextureSize(texture);
			AddReferenceRow((vector == Vector2.Zero) ? "共享图集 -> runtime atlas" : $"共享图集 -> runtime atlas ({vector.X:0} x {vector.Y:0})");
		}
		else if (TryGetAnimationDefinition(animation, out definition) && GodotObject.IsInstanceValid(definition.AtlasTextureArray))
		{
			Vector2 atlasTextureArraySize = definition.AtlasTextureArraySize;
			AddReferenceRow($"共享图集数组 -> runtime atlas ({atlasTextureArraySize.X:0} x {atlasTextureArraySize.Y:0})");
		}
		AddReferenceRow("动画资源 -> " + CurrentResourcePath);
	}

	private string ResolveCompanionDatPath(AdobeAnimateData animation)
	{
		if (animation == null || string.IsNullOrWhiteSpace(animation.animeFile))
		{
			return "";
		}
		string text = NormalizePath(animation.animeFile);
		if (text.StartsWith("res://") || text.StartsWith("uid://") || Path.IsPathRooted(text))
		{
			return text;
		}
		string text2 = NormalizePath(CurrentResourcePath);
		if (string.IsNullOrWhiteSpace(text2))
		{
			return text;
		}
		if (text2.StartsWith("res://"))
		{
			return text2.GetBaseDir().PathJoin(text);
		}
		try
		{
			return NormalizePath(Path.Combine(Path.GetDirectoryName(text2) ?? "", text));
		}
		catch
		{
			return text;
		}
	}

	private void AddTimelineRow(string text)
	{
		AddItemIfMissing(TimelineList, text);
	}

	private void AddPreviewRow(string text)
	{
		AddItemIfMissing(PreviewList, text);
	}

	private void AddGraphRow(string text)
	{
		AddItemIfMissing(GraphList, text);
	}

	private void AddReferenceRow(string text)
	{
		AddItemIfMissing(ReferenceList, text);
	}

	private static void AddItemIfMissing(ItemList list, string text)
	{
		if (list == null || string.IsNullOrWhiteSpace(text))
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

	private static int CountEventFrames(AdobeAnimateData animation)
	{
		if (animation?.events == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < animation.events.Count; i++)
		{
			if (animation.events[i] != null && animation.events[i].Count > 0)
			{
				num++;
			}
		}
		return num;
	}

	private static bool TryGetSharedAnimationAtlas(AdobeAnimateData animation, out Texture2D texture, out Rect2[] mediaRects)
	{
		texture = null;
		mediaRects = System.Array.Empty<Rect2>();
		if (!TryGetAnimationDefinition(animation, out var definition) || !GodotObject.IsInstanceValid(definition.BaseAtlas))
		{
			return false;
		}
		texture = definition.BaseAtlas;
		mediaRects = definition.MediaRects ?? System.Array.Empty<Rect2>();
		return true;
	}

	private static bool TryGetAnimationDefinition(AdobeAnimateData animation, out AdobeAnimateRuntimeDefinition definition)
	{
		definition = null;
		if (animation == null || animation.frameMax <= 0)
		{
			return false;
		}
		definition = AdobeAnimateDefinitionCache.GetOrBuild(animation);
		if (definition != null)
		{
			if (!GodotObject.IsInstanceValid(definition.BaseAtlas) && (definition.AtlasPages == null || definition.AtlasPages.Length == 0))
			{
				return GodotObject.IsInstanceValid(definition.AtlasTextureArray);
			}
			return true;
		}
		return false;
	}

	private static Vector2 TryGetSharedAnimationAtlasSize(AdobeAnimateData animation)
	{
		if (!TryGetAnimationDefinition(animation, out var definition))
		{
			return Vector2.Zero;
		}
		if (definition.AtlasTextureArraySize.X > 0f && definition.AtlasTextureArraySize.Y > 0f)
		{
			return definition.AtlasTextureArraySize;
		}
		return TryGetTextureSize(definition.BaseAtlas);
	}

	private static Vector2 TryGetTextureSize(Texture2D texture)
	{
		if (!GodotObject.IsInstanceValid(texture))
		{
			return Vector2.Zero;
		}
		try
		{
			return texture.GetSize();
		}
		catch
		{
			return Vector2.Zero;
		}
	}

	private static string FormatVariant(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		if (variantType <= Variant.Type.StringName)
		{
			if ((ulong)variantType <= 6uL)
			{
				switch ((int)variantType)
				{
				case 0:
					return "空";
				case 6:
					return $"{value.AsVector2I().X} - {value.AsVector2I().Y}";
				case 5:
					return $"{value.AsVector2().X:0.###}, {value.AsVector2().Y:0.###}";
				case 2:
					return value.AsInt64().ToString();
				case 3:
					return value.AsDouble().ToString("0.###");
				case 4:
					goto IL_0133;
				case 1:
					goto IL_01bd;
				}
			}
			if (variantType == Variant.Type.StringName)
			{
				goto IL_0133;
			}
		}
		else
		{
			switch (variantType)
			{
			case Variant.Type.Array:
				return $"Array({value.AsGodotArray().Count})";
			case Variant.Type.Dictionary:
				return $"Dictionary({value.AsGodotDictionary().Count})";
			}
		}
		goto IL_01bd;
		IL_0133:
		return value.AsString();
		IL_01bd:
		return value.ToString();
	}

	private static string NormalizePath(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return path.Trim().Replace('\\', '/');
		}
		return "";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(159)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCurrentResourceSaved, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindAnimationWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnbindAnimationWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAnimationWorkbenchTabChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "tab", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateAnimationProcessEligibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MountAnimationResponsiveWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureAnimationWorkbenchSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "tab", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnimationSurfaceVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearAnimationResponsiveWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeAnimationPropertyBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderAnimationPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseClosedAnimationResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAnimationPreviewStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "startFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "endFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playing", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindAnimationClipSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeAnimationClipSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshAnimationClipSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "requestedSelection", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "refreshPreviewOptions", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationClipSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectAnimationClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "syncPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnimationClipRangeControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "storedRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginAnimationClipRangeEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "minimumHandle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginAnimationClipExactRangeEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginAnimationClipRangeEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewAnimationClipExactRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PreviewAnimationClipRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitAnimationClipExactRangeEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitAnimationClipRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitAnimationClipRangeEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelAnimationClipRangeEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddAnimationClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeleteSelectedAnimationClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShiftSelectedAnimationClipEarlier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShiftSelectedAnimationClipLater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShiftSelectedAnimationClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationClipNameSubmitted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationClipNameFocusExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenameSelectedAnimationClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "requestedName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitAnimationClipDictionaryChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "oldClips", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "nextClips", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "doSelection", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "undoSelection", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAnimationClipEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "requestedSelection", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAnimationTimelineClipRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateAnimationClipListItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "range", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnimationClipStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatAnimationClipListItem, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "range", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClampAnimationClipRange, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "range", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasAnimationClip, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildUniqueAnimationClipName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "clips", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "baseName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimationClipDictionaryContains, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "clips", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneAnimationClips, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimationClipDictionariesEqual, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindAnimationFrameSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindAnimationFrameNumberControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spinBox", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeAnimationFrameSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnbindAnimationFrameNumberControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spinBox", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateAnimationFrameOptions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationKeyframeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sliceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshSelectedAnimationFrameControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sliceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnimationFrameControlsEnabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginAnimationFramePropertyEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewSelectedAnimationFrameSlice, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "ignoredValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitAnimationFrameAlphaEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "valueChanged", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitAnimationFramePropertyEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelAnimationFramePropertyEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAnimationFrameMediaSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationFrameLayerSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationVisualMediaSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationVisualLayerSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddAnimationKeyframeAtPlayhead, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DuplicateSelectedAnimationKeyframe, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeleteSelectedAnimationKeyframe, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedAnimationKeyframeEarlier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedAnimationKeyframeLater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedAnimationKeyframeBy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationKeyframeMoveRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "sourceFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "destinationFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sliceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitAnimationFrameSnapshotChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "oldSnapshot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "nextSnapshot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "doFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "doSliceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "undoFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "undoSliceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAnimationFrameEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sliceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TrySelectAnimationSliceByKey, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "preferredFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sliceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAnimationRuntimeAtFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateAnimationMediaThumbnail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateAnimationMediaThumbnail, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnimationFrameStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedOptionMetadata, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Int, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectOptionByMetadata, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Int, "requestedId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAnimationNameById, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "requestedId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindAnimationDirectEditSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindAnimationFrameScaleControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UnbindAnimationFrameScaleControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginAnimationFrameScaleEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewAnimationFrameScaleEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitAnimationFrameScaleEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestAnimationFrameScaleConfirmation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "requestedScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartAnimationFrameScaleHydration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "requestedScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncAnimationFrameScaleControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindAnimationReplacementSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeAnimationReplacementSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureAnimationReplacementPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenAnimationReplacementSlotPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenAnimationExtraTexturePicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenAnimationExtraTexturePicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "replacementIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectAnimationExtraTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "replacementIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadAnimationReplacementTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddAnimationReplacementSlot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "mediaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveAnimationReplacementSlot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "mediaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddAnimationExtraTexture, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceAnimationExtraTexture, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddAnimationExtraTexturePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "texturePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceAnimationExtraTexturePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "texturePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveAnimationExtraTexture, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitAnimationExtraTexturePaths, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DuplicateAnimationExtraTexturePaths, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAnimationReplacementWorkbenchFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildAnimationReplacementWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddAnimationReplacementSlotCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "mediaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddAnimationExtraTextureCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "texturePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddAnimationReplacementEmptyCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "glyph", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subtitle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InstantiateAnimationReplacementCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearDynamicAnimationCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatAnimationTextureName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "texturePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ContainsAnimationReplacementSlot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "mediaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildAnimationMediaLayerSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RequestAnimationReplacementReload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReloadAnimationReplacementConfiguration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetAnimationReplacementStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenAnimationDatFileDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SelectAnimationDatFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "selectedPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToStoredAnimationDatPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "selectedPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReloadAnimationFromDirectSettings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestExplicitAnimationReimport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "storedPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelPendingAnimationReimportConfirmation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfirmExplicitAnimationReimport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelExplicitAnimationReimport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "restoreOldSnapshot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshRuntimeAnimationPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateAnimationDirectSummary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FitRuntimePreviewAfterLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "runtimePreview", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClampRuntimePreviewViewportMinimum, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "runtimePreview", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.StartAnimationResourceHydration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "restart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "requestedFrameScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PollAnimationResourceHydration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AbortExplicitAnimationReimport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelAnimationResourceHydration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetAnimationFrameCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderAnimationTimeline, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetAnimationLayerNames, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetAnimationMediaNames, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderAnimationGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderAnimationReferences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveCompanionDatPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddTimelineRow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddPreviewRow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddGraphRow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddReferenceRow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddItemIfMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountEventFrames, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryGetSharedAnimationAtlasSize, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryGetTextureSize, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVariant, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OnCurrentResourceSaved && args.Count == 0)
		{
			OnCurrentResourceSaved();
			ret = default;
			return true;
		}
		if (method == MethodName.BindAnimationWorkbench && args.Count == 0)
		{
			BindAnimationWorkbench();
			ret = default;
			return true;
		}
		if (method == MethodName.UnbindAnimationWorkbench && args.Count == 0)
		{
			UnbindAnimationWorkbench();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationWorkbenchTabChanged && args.Count == 1)
		{
			OnAnimationWorkbenchTabChanged(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationEditorVisibilityChanged && args.Count == 0)
		{
			OnAnimationEditorVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateAnimationProcessEligibility && args.Count == 0)
		{
			UpdateAnimationProcessEligibility();
			ret = default;
			return true;
		}
		if (method == MethodName.MountAnimationResponsiveWorkbench && args.Count == 1)
		{
			MountAnimationResponsiveWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureAnimationWorkbenchSurface && args.Count == 1)
		{
			ConfigureAnimationWorkbenchSurface(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnimationSurfaceVisible && args.Count == 2)
		{
			SetAnimationSurfaceVisible(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearAnimationResponsiveWorkbench && args.Count == 0)
		{
			ClearAnimationResponsiveWorkbench();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeAnimationPropertyBinding && args.Count == 0)
		{
			DisposeAnimationPropertyBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderAnimationPreview && args.Count == 1)
		{
			RenderAnimationPreview(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseClosedAnimationResource && args.Count == 0)
		{
			ReleaseClosedAnimationResource();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationPreviewStateChanged && args.Count == 5)
		{
			OnAnimationPreviewStateChanged(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindAnimationClipSurface && args.Count == 2)
		{
			BindAnimationClipSurface(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeAnimationClipSurface && args.Count == 0)
		{
			DisposeAnimationClipSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAnimationClipSurface && args.Count == 3)
		{
			RefreshAnimationClipSurface(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationClipSelected && args.Count == 1)
		{
			OnAnimationClipSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectAnimationClip && args.Count == 2)
		{
			SelectAnimationClip(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnimationClipRangeControls && args.Count == 1)
		{
			SetAnimationClipRangeControls(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginAnimationClipRangeEdit && args.Count == 1)
		{
			BeginAnimationClipRangeEdit(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginAnimationClipExactRangeEdit && args.Count == 0)
		{
			BeginAnimationClipExactRangeEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginAnimationClipRangeEdit && args.Count == 0)
		{
			BeginAnimationClipRangeEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewAnimationClipExactRange && args.Count == 1)
		{
			PreviewAnimationClipExactRange(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewAnimationClipRange && args.Count == 2)
		{
			PreviewAnimationClipRange(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitAnimationClipExactRangeEdit && args.Count == 0)
		{
			CommitAnimationClipExactRangeEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.CommitAnimationClipRange && args.Count == 2)
		{
			CommitAnimationClipRange(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitAnimationClipRangeEdit && args.Count == 0)
		{
			CommitAnimationClipRangeEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelAnimationClipRangeEdit && args.Count == 0)
		{
			CancelAnimationClipRangeEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.AddAnimationClip && args.Count == 0)
		{
			AddAnimationClip();
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteSelectedAnimationClip && args.Count == 0)
		{
			DeleteSelectedAnimationClip();
			ret = default;
			return true;
		}
		if (method == MethodName.ShiftSelectedAnimationClipEarlier && args.Count == 0)
		{
			ShiftSelectedAnimationClipEarlier();
			ret = default;
			return true;
		}
		if (method == MethodName.ShiftSelectedAnimationClipLater && args.Count == 0)
		{
			ShiftSelectedAnimationClipLater();
			ret = default;
			return true;
		}
		if (method == MethodName.ShiftSelectedAnimationClip && args.Count == 1)
		{
			ShiftSelectedAnimationClip(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationClipNameSubmitted && args.Count == 1)
		{
			OnAnimationClipNameSubmitted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationClipNameFocusExited && args.Count == 0)
		{
			OnAnimationClipNameFocusExited();
			ret = default;
			return true;
		}
		if (method == MethodName.RenameSelectedAnimationClip && args.Count == 1)
		{
			RenameSelectedAnimationClip(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitAnimationClipDictionaryChange && args.Count == 5)
		{
			CommitAnimationClipDictionaryChange(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAnimationClipEditorFromHistory && args.Count == 2)
		{
			RefreshAnimationClipEditorFromHistory(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAnimationTimelineClipRows && args.Count == 1)
		{
			RefreshAnimationTimelineClipRows(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateAnimationClipListItem && args.Count == 2)
		{
			UpdateAnimationClipListItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnimationClipStatus && args.Count == 2)
		{
			SetAnimationClipStatus(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FormatAnimationClipListItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatAnimationClipListItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.ClampAnimationClipRange && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ClampAnimationClipRange(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.HasAnimationClip && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasAnimationClip(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildUniqueAnimationClipName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildUniqueAnimationClipName(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AnimationClipDictionaryContains && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AnimationClipDictionaryContains(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CloneAnimationClips && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CloneAnimationClips(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.AnimationClipDictionariesEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AnimationClipDictionariesEqual(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1])));
			return true;
		}
		if (method == MethodName.BindAnimationFrameSurface && args.Count == 2)
		{
			BindAnimationFrameSurface(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindAnimationFrameNumberControl && args.Count == 1)
		{
			BindAnimationFrameNumberControl(VariantUtils.ConvertTo<SpinBox>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeAnimationFrameSurface && args.Count == 0)
		{
			DisposeAnimationFrameSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.UnbindAnimationFrameNumberControl && args.Count == 1)
		{
			UnbindAnimationFrameNumberControl(VariantUtils.ConvertTo<SpinBox>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateAnimationFrameOptions && args.Count == 1)
		{
			PopulateAnimationFrameOptions(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationKeyframeSelected && args.Count == 3)
		{
			OnAnimationKeyframeSelected(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSelectedAnimationFrameControls && args.Count == 2)
		{
			RefreshSelectedAnimationFrameControls(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnimationFrameControlsEnabled && args.Count == 1)
		{
			SetAnimationFrameControlsEnabled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginAnimationFramePropertyEdit && args.Count == 0)
		{
			BeginAnimationFramePropertyEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewSelectedAnimationFrameSlice && args.Count == 1)
		{
			PreviewSelectedAnimationFrameSlice(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitAnimationFrameAlphaEdit && args.Count == 1)
		{
			CommitAnimationFrameAlphaEdit(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitAnimationFramePropertyEdit && args.Count == 0)
		{
			CommitAnimationFramePropertyEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelAnimationFramePropertyEdit && args.Count == 0)
		{
			CancelAnimationFramePropertyEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationFrameMediaSelected && args.Count == 1)
		{
			OnAnimationFrameMediaSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationFrameLayerSelected && args.Count == 1)
		{
			OnAnimationFrameLayerSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationVisualMediaSelected && args.Count == 1)
		{
			OnAnimationVisualMediaSelected(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationVisualLayerSelected && args.Count == 1)
		{
			OnAnimationVisualLayerSelected(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddAnimationKeyframeAtPlayhead && args.Count == 0)
		{
			AddAnimationKeyframeAtPlayhead();
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicateSelectedAnimationKeyframe && args.Count == 0)
		{
			DuplicateSelectedAnimationKeyframe();
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteSelectedAnimationKeyframe && args.Count == 0)
		{
			DeleteSelectedAnimationKeyframe();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedAnimationKeyframeEarlier && args.Count == 0)
		{
			MoveSelectedAnimationKeyframeEarlier();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedAnimationKeyframeLater && args.Count == 0)
		{
			MoveSelectedAnimationKeyframeLater();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedAnimationKeyframeBy && args.Count == 1)
		{
			MoveSelectedAnimationKeyframeBy(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationKeyframeMoveRequested && args.Count == 4)
		{
			OnAnimationKeyframeMoveRequested(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitAnimationFrameSnapshotChange && args.Count == 8)
		{
			CommitAnimationFrameSnapshotChange(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]), VariantUtils.ConvertTo<Dictionary>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<string>(in args[7]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAnimationFrameEditorFromHistory && args.Count == 3)
		{
			RefreshAnimationFrameEditorFromHistory(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrySelectAnimationSliceByKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TrySelectAnimationSliceByKey(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.RefreshAnimationRuntimeAtFrame && args.Count == 1)
		{
			RefreshAnimationRuntimeAtFrame(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateAnimationMediaThumbnail && args.Count == 2)
		{
			UpdateAnimationMediaThumbnail(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateAnimationMediaThumbnail && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(CreateAnimationMediaThumbnail(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.SetAnimationFrameStatus && args.Count == 2)
		{
			SetAnimationFrameStatus(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedOptionMetadata && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetSelectedOptionMetadata(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.SelectOptionByMetadata && args.Count == 2)
		{
			SelectOptionByMetadata(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetAnimationNameById && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(GetAnimationNameById(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.BindAnimationDirectEditSurface && args.Count == 2)
		{
			BindAnimationDirectEditSurface(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindAnimationFrameScaleControl && args.Count == 1)
		{
			BindAnimationFrameScaleControl(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnbindAnimationFrameScaleControl && args.Count == 0)
		{
			UnbindAnimationFrameScaleControl();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginAnimationFrameScaleEdit && args.Count == 0)
		{
			BeginAnimationFrameScaleEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewAnimationFrameScaleEdit && args.Count == 1)
		{
			PreviewAnimationFrameScaleEdit(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitAnimationFrameScaleEdit && args.Count == 0)
		{
			CommitAnimationFrameScaleEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.RequestAnimationFrameScaleConfirmation && args.Count == 2)
		{
			RequestAnimationFrameScaleConfirmation(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartAnimationFrameScaleHydration && args.Count == 2)
		{
			StartAnimationFrameScaleHydration(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncAnimationFrameScaleControl && args.Count == 1)
		{
			SyncAnimationFrameScaleControl(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindAnimationReplacementSurface && args.Count == 2)
		{
			BindAnimationReplacementSurface(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeAnimationReplacementSurface && args.Count == 0)
		{
			DisposeAnimationReplacementSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureAnimationReplacementPicker && args.Count == 0)
		{
			EnsureAnimationReplacementPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenAnimationReplacementSlotPicker && args.Count == 0)
		{
			OpenAnimationReplacementSlotPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenAnimationExtraTexturePicker && args.Count == 0)
		{
			OpenAnimationExtraTexturePicker();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenAnimationExtraTexturePicker && args.Count == 1)
		{
			OpenAnimationExtraTexturePicker(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectAnimationExtraTexture && args.Count == 2)
		{
			SelectAnimationExtraTexture(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadAnimationReplacementTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadAnimationReplacementTexture(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AddAnimationReplacementSlot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(AddAnimationReplacementSlot(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveAnimationReplacementSlot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveAnimationReplacementSlot(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AddAnimationExtraTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(AddAnimationExtraTexture(VariantUtils.ConvertTo<Texture2D>(in args[0])));
			return true;
		}
		if (method == MethodName.ReplaceAnimationExtraTexture && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ReplaceAnimationExtraTexture(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1])));
			return true;
		}
		if (method == MethodName.AddAnimationExtraTexturePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(AddAnimationExtraTexturePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReplaceAnimationExtraTexturePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ReplaceAnimationExtraTexturePath(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.RemoveAnimationExtraTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveAnimationExtraTexture(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CommitAnimationExtraTexturePaths && args.Count == 3)
		{
			CommitAnimationExtraTexturePaths(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertToArray<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicateAnimationExtraTexturePaths && args.Count == 1)
		{
			Array<string> array = DuplicateAnimationExtraTexturePaths(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.RefreshAnimationReplacementWorkbenchFromHistory && args.Count == 0)
		{
			RefreshAnimationReplacementWorkbenchFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildAnimationReplacementWorkbench && args.Count == 1)
		{
			RebuildAnimationReplacementWorkbench(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddAnimationReplacementSlotCard && args.Count == 3)
		{
			AddAnimationReplacementSlotCard(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateData>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddAnimationExtraTextureCard && args.Count == 3)
		{
			AddAnimationExtraTextureCard(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddAnimationReplacementEmptyCard && args.Count == 4)
		{
			AddAnimationReplacementEmptyCard(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.InstantiateAnimationReplacementCard && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWAnimationReplacementCard>(InstantiateAnimationReplacementCard(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearDynamicAnimationCards && args.Count == 1)
		{
			ClearDynamicAnimationCards(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FormatAnimationTextureName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(FormatAnimationTextureName(VariantUtils.ConvertTo<Texture2D>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.ContainsAnimationReplacementSlot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsAnimationReplacementSlot(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildAnimationMediaLayerSummary && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildAnimationMediaLayerSummary(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.RequestAnimationReplacementReload && args.Count == 0)
		{
			RequestAnimationReplacementReload();
			ret = default;
			return true;
		}
		if (method == MethodName.ReloadAnimationReplacementConfiguration && args.Count == 0)
		{
			ReloadAnimationReplacementConfiguration();
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnimationReplacementStatus && args.Count == 2)
		{
			SetAnimationReplacementStatus(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationVisualPropertyEdited && args.Count == 1)
		{
			OnAnimationVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenAnimationDatFileDialog && args.Count == 1)
		{
			OpenAnimationDatFileDialog(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectAnimationDatFile && args.Count == 1)
		{
			SelectAnimationDatFile(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToStoredAnimationDatPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ToStoredAnimationDatPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReloadAnimationFromDirectSettings && args.Count == 0)
		{
			ReloadAnimationFromDirectSettings();
			ret = default;
			return true;
		}
		if (method == MethodName.RequestExplicitAnimationReimport && args.Count == 2)
		{
			RequestExplicitAnimationReimport(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CancelPendingAnimationReimportConfirmation && args.Count == 0)
		{
			CancelPendingAnimationReimportConfirmation();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfirmExplicitAnimationReimport && args.Count == 0)
		{
			ConfirmExplicitAnimationReimport();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelExplicitAnimationReimport && args.Count == 1)
		{
			CancelExplicitAnimationReimport(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRuntimeAnimationPreview && args.Count == 0)
		{
			RefreshRuntimeAnimationPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateAnimationDirectSummary && args.Count == 0)
		{
			UpdateAnimationDirectSummary();
			ret = default;
			return true;
		}
		if (method == MethodName.FitRuntimePreviewAfterLayout && args.Count == 1)
		{
			FitRuntimePreviewAfterLayout(VariantUtils.ConvertTo<AdobeAnimateInspectorPreview>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClampRuntimePreviewViewportMinimum && args.Count == 1)
		{
			ClampRuntimePreviewViewportMinimum(VariantUtils.ConvertTo<AdobeAnimateInspectorPreview>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartAnimationResourceHydration && args.Count == 2)
		{
			StartAnimationResourceHydration(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PollAnimationResourceHydration && args.Count == 0)
		{
			PollAnimationResourceHydration();
			ret = default;
			return true;
		}
		if (method == MethodName.AbortExplicitAnimationReimport && args.Count == 2)
		{
			AbortExplicitAnimationReimport(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CancelAnimationResourceHydration && args.Count == 0)
		{
			CancelAnimationResourceHydration();
			ret = default;
			return true;
		}
		if (method == MethodName.GetAnimationFrameCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetAnimationFrameCount(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.RenderAnimationTimeline && args.Count == 1)
		{
			RenderAnimationTimeline(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetAnimationLayerNames && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetAnimationLayerNames(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAnimationMediaNames && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetAnimationMediaNames(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.RenderAnimationGraph && args.Count == 1)
		{
			RenderAnimationGraph(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderAnimationReferences && args.Count == 1)
		{
			RenderAnimationReferences(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveCompanionDatPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveCompanionDatPath(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.AddTimelineRow && args.Count == 1)
		{
			AddTimelineRow(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddPreviewRow && args.Count == 1)
		{
			AddPreviewRow(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddGraphRow && args.Count == 1)
		{
			AddGraphRow(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddReferenceRow && args.Count == 1)
		{
			AddReferenceRow(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountEventFrames && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountEventFrames(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.TryGetSharedAnimationAtlasSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(TryGetSharedAnimationAtlasSize(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.TryGetTextureSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(TryGetTextureSize(VariantUtils.ConvertTo<Texture2D>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FormatAnimationClipListItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatAnimationClipListItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.ClampAnimationClipRange && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ClampAnimationClipRange(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.HasAnimationClip && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasAnimationClip(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildUniqueAnimationClipName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildUniqueAnimationClipName(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AnimationClipDictionaryContains && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AnimationClipDictionaryContains(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CloneAnimationClips && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CloneAnimationClips(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.AnimationClipDictionariesEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AnimationClipDictionariesEqual(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateAnimationMediaThumbnail && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(CreateAnimationMediaThumbnail(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetSelectedOptionMetadata && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetSelectedOptionMetadata(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.SelectOptionByMetadata && args.Count == 2)
		{
			SelectOptionByMetadata(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetAnimationNameById && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(GetAnimationNameById(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.LoadAnimationReplacementTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadAnimationReplacementTexture(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DuplicateAnimationExtraTexturePaths && args.Count == 1)
		{
			Array<string> array = DuplicateAnimationExtraTexturePaths(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.AddAnimationReplacementEmptyCard && args.Count == 4)
		{
			AddAnimationReplacementEmptyCard(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.InstantiateAnimationReplacementCard && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWAnimationReplacementCard>(InstantiateAnimationReplacementCard(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearDynamicAnimationCards && args.Count == 1)
		{
			ClearDynamicAnimationCards(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FormatAnimationTextureName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(FormatAnimationTextureName(VariantUtils.ConvertTo<Texture2D>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.ContainsAnimationReplacementSlot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsAnimationReplacementSlot(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildAnimationMediaLayerSummary && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildAnimationMediaLayerSummary(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ClampRuntimePreviewViewportMinimum && args.Count == 1)
		{
			ClampRuntimePreviewViewportMinimum(VariantUtils.ConvertTo<AdobeAnimateInspectorPreview>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetAnimationFrameCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetAnimationFrameCount(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAnimationLayerNames && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetAnimationLayerNames(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAnimationMediaNames && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetAnimationMediaNames(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountEventFrames && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountEventFrames(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.TryGetSharedAnimationAtlasSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(TryGetSharedAnimationAtlasSize(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.TryGetTextureSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(TryGetTextureSize(VariantUtils.ConvertTo<Texture2D>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizePath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.OnCurrentResourceSaved)
		{
			return true;
		}
		if (method == MethodName.BindAnimationWorkbench)
		{
			return true;
		}
		if (method == MethodName.UnbindAnimationWorkbench)
		{
			return true;
		}
		if (method == MethodName.OnAnimationWorkbenchTabChanged)
		{
			return true;
		}
		if (method == MethodName.OnAnimationEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.UpdateAnimationProcessEligibility)
		{
			return true;
		}
		if (method == MethodName.MountAnimationResponsiveWorkbench)
		{
			return true;
		}
		if (method == MethodName.ConfigureAnimationWorkbenchSurface)
		{
			return true;
		}
		if (method == MethodName.SetAnimationSurfaceVisible)
		{
			return true;
		}
		if (method == MethodName.ClearAnimationResponsiveWorkbench)
		{
			return true;
		}
		if (method == MethodName.DisposeAnimationPropertyBinding)
		{
			return true;
		}
		if (method == MethodName.RenderAnimationPreview)
		{
			return true;
		}
		if (method == MethodName.ReleaseClosedAnimationResource)
		{
			return true;
		}
		if (method == MethodName.OnAnimationPreviewStateChanged)
		{
			return true;
		}
		if (method == MethodName.BindAnimationClipSurface)
		{
			return true;
		}
		if (method == MethodName.DisposeAnimationClipSurface)
		{
			return true;
		}
		if (method == MethodName.RefreshAnimationClipSurface)
		{
			return true;
		}
		if (method == MethodName.OnAnimationClipSelected)
		{
			return true;
		}
		if (method == MethodName.SelectAnimationClip)
		{
			return true;
		}
		if (method == MethodName.SetAnimationClipRangeControls)
		{
			return true;
		}
		if (method == MethodName.BeginAnimationClipRangeEdit)
		{
			return true;
		}
		if (method == MethodName.BeginAnimationClipExactRangeEdit)
		{
			return true;
		}
		if (method == MethodName.PreviewAnimationClipExactRange)
		{
			return true;
		}
		if (method == MethodName.PreviewAnimationClipRange)
		{
			return true;
		}
		if (method == MethodName.CommitAnimationClipExactRangeEdit)
		{
			return true;
		}
		if (method == MethodName.CommitAnimationClipRange)
		{
			return true;
		}
		if (method == MethodName.CommitAnimationClipRangeEdit)
		{
			return true;
		}
		if (method == MethodName.CancelAnimationClipRangeEdit)
		{
			return true;
		}
		if (method == MethodName.AddAnimationClip)
		{
			return true;
		}
		if (method == MethodName.DeleteSelectedAnimationClip)
		{
			return true;
		}
		if (method == MethodName.ShiftSelectedAnimationClipEarlier)
		{
			return true;
		}
		if (method == MethodName.ShiftSelectedAnimationClipLater)
		{
			return true;
		}
		if (method == MethodName.ShiftSelectedAnimationClip)
		{
			return true;
		}
		if (method == MethodName.OnAnimationClipNameSubmitted)
		{
			return true;
		}
		if (method == MethodName.OnAnimationClipNameFocusExited)
		{
			return true;
		}
		if (method == MethodName.RenameSelectedAnimationClip)
		{
			return true;
		}
		if (method == MethodName.CommitAnimationClipDictionaryChange)
		{
			return true;
		}
		if (method == MethodName.RefreshAnimationClipEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshAnimationTimelineClipRows)
		{
			return true;
		}
		if (method == MethodName.UpdateAnimationClipListItem)
		{
			return true;
		}
		if (method == MethodName.SetAnimationClipStatus)
		{
			return true;
		}
		if (method == MethodName.FormatAnimationClipListItem)
		{
			return true;
		}
		if (method == MethodName.ClampAnimationClipRange)
		{
			return true;
		}
		if (method == MethodName.HasAnimationClip)
		{
			return true;
		}
		if (method == MethodName.BuildUniqueAnimationClipName)
		{
			return true;
		}
		if (method == MethodName.AnimationClipDictionaryContains)
		{
			return true;
		}
		if (method == MethodName.CloneAnimationClips)
		{
			return true;
		}
		if (method == MethodName.AnimationClipDictionariesEqual)
		{
			return true;
		}
		if (method == MethodName.BindAnimationFrameSurface)
		{
			return true;
		}
		if (method == MethodName.BindAnimationFrameNumberControl)
		{
			return true;
		}
		if (method == MethodName.DisposeAnimationFrameSurface)
		{
			return true;
		}
		if (method == MethodName.UnbindAnimationFrameNumberControl)
		{
			return true;
		}
		if (method == MethodName.PopulateAnimationFrameOptions)
		{
			return true;
		}
		if (method == MethodName.OnAnimationKeyframeSelected)
		{
			return true;
		}
		if (method == MethodName.RefreshSelectedAnimationFrameControls)
		{
			return true;
		}
		if (method == MethodName.SetAnimationFrameControlsEnabled)
		{
			return true;
		}
		if (method == MethodName.BeginAnimationFramePropertyEdit)
		{
			return true;
		}
		if (method == MethodName.PreviewSelectedAnimationFrameSlice)
		{
			return true;
		}
		if (method == MethodName.CommitAnimationFrameAlphaEdit)
		{
			return true;
		}
		if (method == MethodName.CommitAnimationFramePropertyEdit)
		{
			return true;
		}
		if (method == MethodName.CancelAnimationFramePropertyEdit)
		{
			return true;
		}
		if (method == MethodName.OnAnimationFrameMediaSelected)
		{
			return true;
		}
		if (method == MethodName.OnAnimationFrameLayerSelected)
		{
			return true;
		}
		if (method == MethodName.OnAnimationVisualMediaSelected)
		{
			return true;
		}
		if (method == MethodName.OnAnimationVisualLayerSelected)
		{
			return true;
		}
		if (method == MethodName.AddAnimationKeyframeAtPlayhead)
		{
			return true;
		}
		if (method == MethodName.DuplicateSelectedAnimationKeyframe)
		{
			return true;
		}
		if (method == MethodName.DeleteSelectedAnimationKeyframe)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedAnimationKeyframeEarlier)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedAnimationKeyframeLater)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedAnimationKeyframeBy)
		{
			return true;
		}
		if (method == MethodName.OnAnimationKeyframeMoveRequested)
		{
			return true;
		}
		if (method == MethodName.CommitAnimationFrameSnapshotChange)
		{
			return true;
		}
		if (method == MethodName.RefreshAnimationFrameEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.TrySelectAnimationSliceByKey)
		{
			return true;
		}
		if (method == MethodName.RefreshAnimationRuntimeAtFrame)
		{
			return true;
		}
		if (method == MethodName.UpdateAnimationMediaThumbnail)
		{
			return true;
		}
		if (method == MethodName.CreateAnimationMediaThumbnail)
		{
			return true;
		}
		if (method == MethodName.SetAnimationFrameStatus)
		{
			return true;
		}
		if (method == MethodName.GetSelectedOptionMetadata)
		{
			return true;
		}
		if (method == MethodName.SelectOptionByMetadata)
		{
			return true;
		}
		if (method == MethodName.GetAnimationNameById)
		{
			return true;
		}
		if (method == MethodName.BindAnimationDirectEditSurface)
		{
			return true;
		}
		if (method == MethodName.BindAnimationFrameScaleControl)
		{
			return true;
		}
		if (method == MethodName.UnbindAnimationFrameScaleControl)
		{
			return true;
		}
		if (method == MethodName.BeginAnimationFrameScaleEdit)
		{
			return true;
		}
		if (method == MethodName.PreviewAnimationFrameScaleEdit)
		{
			return true;
		}
		if (method == MethodName.CommitAnimationFrameScaleEdit)
		{
			return true;
		}
		if (method == MethodName.RequestAnimationFrameScaleConfirmation)
		{
			return true;
		}
		if (method == MethodName.StartAnimationFrameScaleHydration)
		{
			return true;
		}
		if (method == MethodName.SyncAnimationFrameScaleControl)
		{
			return true;
		}
		if (method == MethodName.BindAnimationReplacementSurface)
		{
			return true;
		}
		if (method == MethodName.DisposeAnimationReplacementSurface)
		{
			return true;
		}
		if (method == MethodName.EnsureAnimationReplacementPicker)
		{
			return true;
		}
		if (method == MethodName.OpenAnimationReplacementSlotPicker)
		{
			return true;
		}
		if (method == MethodName.OpenAnimationExtraTexturePicker)
		{
			return true;
		}
		if (method == MethodName.SelectAnimationExtraTexture)
		{
			return true;
		}
		if (method == MethodName.LoadAnimationReplacementTexture)
		{
			return true;
		}
		if (method == MethodName.AddAnimationReplacementSlot)
		{
			return true;
		}
		if (method == MethodName.RemoveAnimationReplacementSlot)
		{
			return true;
		}
		if (method == MethodName.AddAnimationExtraTexture)
		{
			return true;
		}
		if (method == MethodName.ReplaceAnimationExtraTexture)
		{
			return true;
		}
		if (method == MethodName.AddAnimationExtraTexturePath)
		{
			return true;
		}
		if (method == MethodName.ReplaceAnimationExtraTexturePath)
		{
			return true;
		}
		if (method == MethodName.RemoveAnimationExtraTexture)
		{
			return true;
		}
		if (method == MethodName.CommitAnimationExtraTexturePaths)
		{
			return true;
		}
		if (method == MethodName.DuplicateAnimationExtraTexturePaths)
		{
			return true;
		}
		if (method == MethodName.RefreshAnimationReplacementWorkbenchFromHistory)
		{
			return true;
		}
		if (method == MethodName.RebuildAnimationReplacementWorkbench)
		{
			return true;
		}
		if (method == MethodName.AddAnimationReplacementSlotCard)
		{
			return true;
		}
		if (method == MethodName.AddAnimationExtraTextureCard)
		{
			return true;
		}
		if (method == MethodName.AddAnimationReplacementEmptyCard)
		{
			return true;
		}
		if (method == MethodName.InstantiateAnimationReplacementCard)
		{
			return true;
		}
		if (method == MethodName.ClearDynamicAnimationCards)
		{
			return true;
		}
		if (method == MethodName.FormatAnimationTextureName)
		{
			return true;
		}
		if (method == MethodName.ContainsAnimationReplacementSlot)
		{
			return true;
		}
		if (method == MethodName.BuildAnimationMediaLayerSummary)
		{
			return true;
		}
		if (method == MethodName.RequestAnimationReplacementReload)
		{
			return true;
		}
		if (method == MethodName.ReloadAnimationReplacementConfiguration)
		{
			return true;
		}
		if (method == MethodName.SetAnimationReplacementStatus)
		{
			return true;
		}
		if (method == MethodName.OnAnimationVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.OpenAnimationDatFileDialog)
		{
			return true;
		}
		if (method == MethodName.SelectAnimationDatFile)
		{
			return true;
		}
		if (method == MethodName.ToStoredAnimationDatPath)
		{
			return true;
		}
		if (method == MethodName.ReloadAnimationFromDirectSettings)
		{
			return true;
		}
		if (method == MethodName.RequestExplicitAnimationReimport)
		{
			return true;
		}
		if (method == MethodName.CancelPendingAnimationReimportConfirmation)
		{
			return true;
		}
		if (method == MethodName.ConfirmExplicitAnimationReimport)
		{
			return true;
		}
		if (method == MethodName.CancelExplicitAnimationReimport)
		{
			return true;
		}
		if (method == MethodName.RefreshRuntimeAnimationPreview)
		{
			return true;
		}
		if (method == MethodName.UpdateAnimationDirectSummary)
		{
			return true;
		}
		if (method == MethodName.FitRuntimePreviewAfterLayout)
		{
			return true;
		}
		if (method == MethodName.ClampRuntimePreviewViewportMinimum)
		{
			return true;
		}
		if (method == MethodName.StartAnimationResourceHydration)
		{
			return true;
		}
		if (method == MethodName.PollAnimationResourceHydration)
		{
			return true;
		}
		if (method == MethodName.AbortExplicitAnimationReimport)
		{
			return true;
		}
		if (method == MethodName.CancelAnimationResourceHydration)
		{
			return true;
		}
		if (method == MethodName.GetAnimationFrameCount)
		{
			return true;
		}
		if (method == MethodName.RenderAnimationTimeline)
		{
			return true;
		}
		if (method == MethodName.GetAnimationLayerNames)
		{
			return true;
		}
		if (method == MethodName.GetAnimationMediaNames)
		{
			return true;
		}
		if (method == MethodName.RenderAnimationGraph)
		{
			return true;
		}
		if (method == MethodName.RenderAnimationReferences)
		{
			return true;
		}
		if (method == MethodName.ResolveCompanionDatPath)
		{
			return true;
		}
		if (method == MethodName.AddTimelineRow)
		{
			return true;
		}
		if (method == MethodName.AddPreviewRow)
		{
			return true;
		}
		if (method == MethodName.AddGraphRow)
		{
			return true;
		}
		if (method == MethodName.AddReferenceRow)
		{
			return true;
		}
		if (method == MethodName.AddItemIfMissing)
		{
			return true;
		}
		if (method == MethodName.CountEventFrames)
		{
			return true;
		}
		if (method == MethodName.TryGetSharedAnimationAtlasSize)
		{
			return true;
		}
		if (method == MethodName.TryGetTextureSize)
		{
			return true;
		}
		if (method == MethodName.FormatVariant)
		{
			return true;
		}
		if (method == MethodName.NormalizePath)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.LastAnimationSavePersistedTicks)
		{
			LastAnimationSavePersistedTicks = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._animationPreviewStatusLabel)
		{
			_animationPreviewStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationDirectSummaryLabel)
		{
			_animationDirectSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationResourcePathLabel)
		{
			_animationResourcePathLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationRuntimeFrameLabel)
		{
			_animationRuntimeFrameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationPlaybackStateLabel)
		{
			_animationPlaybackStateLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationClipList)
		{
			_animationClipList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._animationClipNameEdit)
		{
			_animationClipNameEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._animationClipStartSpinBox)
		{
			_animationClipStartSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._animationClipEndSpinBox)
		{
			_animationClipEndSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._animationClipRangeControl)
		{
			_animationClipRangeControl = VariantUtils.ConvertTo<XWPacketSpawnEntryRangeControl>(in value);
			return true;
		}
		if (name == PropertyName._animationAddClipButton)
		{
			_animationAddClipButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationDeleteClipButton)
		{
			_animationDeleteClipButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationShiftClipEarlierButton)
		{
			_animationShiftClipEarlierButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationShiftClipLaterButton)
		{
			_animationShiftClipLaterButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationClipEditStatusLabel)
		{
			_animationClipEditStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationFrameTimeline)
		{
			_animationFrameTimeline = VariantUtils.ConvertTo<XWAnimationFrameTimeline>(in value);
			return true;
		}
		if (name == PropertyName._animationAddKeyframeButton)
		{
			_animationAddKeyframeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationDuplicateKeyframeButton)
		{
			_animationDuplicateKeyframeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationDeleteKeyframeButton)
		{
			_animationDeleteKeyframeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationMoveKeyframeEarlierButton)
		{
			_animationMoveKeyframeEarlierButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationMoveKeyframeLaterButton)
		{
			_animationMoveKeyframeLaterButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationSelectedKeyframeLabel)
		{
			_animationSelectedKeyframeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationMediaOption)
		{
			_animationMediaOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._animationLayerOption)
		{
			_animationLayerOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._animationMediaThumbnail)
		{
			_animationMediaThumbnail = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._animationPositionXSpinBox)
		{
			_animationPositionXSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._animationPositionYSpinBox)
		{
			_animationPositionYSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._animationRotationSpinBox)
		{
			_animationRotationSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._animationSkewSpinBox)
		{
			_animationSkewSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._animationScaleXSpinBox)
		{
			_animationScaleXSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._animationScaleYSpinBox)
		{
			_animationScaleYSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._animationAlphaSlider)
		{
			_animationAlphaSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._animationAlphaPercentLabel)
		{
			_animationAlphaPercentLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationKeyframeEditStatusLabel)
		{
			_animationKeyframeEditStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._updatingAnimationFrameControls)
		{
			_updatingAnimationFrameControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._animationFrameEditSnapshot)
		{
			_animationFrameEditSnapshot = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._animationFrameEditTarget)
		{
			_animationFrameEditTarget = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._animationFrameEditFrame)
		{
			_animationFrameEditFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationFrameEditSliceKey)
		{
			_animationFrameEditSliceKey = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationFrameEditMutated)
		{
			_animationFrameEditMutated = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._animationFrameMutationCount)
		{
			_animationFrameMutationCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationDatFileButton)
		{
			_animationDatFileButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationDatFileDialog)
		{
			_animationDatFileDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._animationReimportConfirmDialog)
		{
			_animationReimportConfirmDialog = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._animationFrameScaleSpinBox)
		{
			_animationFrameScaleSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._updatingAnimationFrameScaleControl)
		{
			_updatingAnimationFrameScaleControl = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingAnimationReimportTarget)
		{
			_pendingAnimationReimportTarget = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._pendingAnimationReimportPath)
		{
			_pendingAnimationReimportPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pendingAnimationFrameScale)
		{
			_pendingAnimationFrameScale = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportOldSnapshot)
		{
			_animationExplicitReimportOldSnapshot = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportFrame)
		{
			_animationExplicitReimportFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportSliceKey)
		{
			_animationExplicitReimportSliceKey = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportActionName)
		{
			_animationExplicitReimportActionName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportSuccessMessage)
		{
			_animationExplicitReimportSuccessMessage = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportRequiresRestore)
		{
			_animationExplicitReimportRequiresRestore = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreview)
		{
			_runtimePreview = VariantUtils.ConvertTo<AdobeAnimateInspectorPreview>(in value);
			return true;
		}
		if (name == PropertyName._animationDatSelectionTarget)
		{
			_animationDatSelectionTarget = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._animationHydrationPath)
		{
			_animationHydrationPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._animationHydrationTarget)
		{
			_animationHydrationTarget = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._animationHydrationRequestedFrameScale)
		{
			_animationHydrationRequestedFrameScale = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationHydrationExpectedFrameScale)
		{
			_animationHydrationExpectedFrameScale = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationHydrationExpectedAuthoringRevision)
		{
			_animationHydrationExpectedAuthoringRevision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._animationStateUpdateCount)
		{
			_animationStateUpdateCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedAnimationClipName)
		{
			_selectedAnimationClipName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._updatingAnimationClipControls)
		{
			_updatingAnimationClipControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._animationClipEditSnapshot)
		{
			_animationClipEditSnapshot = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._animationClipEditName)
		{
			_animationClipEditName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._animationClipEditTarget)
		{
			_animationClipEditTarget = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._animationClipMutationCount)
		{
			_animationClipMutationCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._editingAnimation)
		{
			_editingAnimation = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._animationReplacementSlotCards)
		{
			_animationReplacementSlotCards = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._animationExtraTextureCards)
		{
			_animationExtraTextureCards = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._animationReplacementSummaryLabel)
		{
			_animationReplacementSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationReplacementStatusLabel)
		{
			_animationReplacementStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationAddReplacementSlotButton)
		{
			_animationAddReplacementSlotButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationAddExtraTextureButton)
		{
			_animationAddExtraTextureButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationReplacementSaveButton)
		{
			_animationReplacementSaveButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationReplacementReloadButton)
		{
			_animationReplacementReloadButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._animationReplacementReloadConfirmDialog)
		{
			_animationReplacementReloadConfirmDialog = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._animationReplacementPicker)
		{
			_animationReplacementPicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._animationReplacementMutationCount)
		{
			_animationReplacementMutationCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationReplacementSlotCardCount)
		{
			_animationReplacementSlotCardCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationExtraTextureCardCount)
		{
			_animationExtraTextureCardCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationWorkbench)
		{
			_animationWorkbench = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._animationWorkbenchTabs)
		{
			_animationWorkbenchTabs = VariantUtils.ConvertTo<TabBar>(in value);
			return true;
		}
		if (name == PropertyName._animationChoiceSurface)
		{
			_animationChoiceSurface = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._animationSourceHost)
		{
			_animationSourceHost = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._animationSourceContainer)
		{
			_animationSourceContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._animationMediaChoiceGrid)
		{
			_animationMediaChoiceGrid = VariantUtils.ConvertTo<XWAnimationVisualChoiceGrid>(in value);
			return true;
		}
		if (name == PropertyName._animationLayerChoiceGrid)
		{
			_animationLayerChoiceGrid = VariantUtils.ConvertTo<XWAnimationVisualChoiceGrid>(in value);
			return true;
		}
		if (name == PropertyName._animationWorkbenchSignalsConnected)
		{
			_animationWorkbenchSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.AnimationStateUpdateCount)
		{
			from = AnimationStateUpdateCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationClipMutationCount)
		{
			from = AnimationClipMutationCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationFrameMutationCount)
		{
			from = AnimationFrameMutationCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationReplacementMutationCount)
		{
			from = AnimationReplacementMutationCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationReplacementSlotCardCount)
		{
			from = AnimationReplacementSlotCardCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationExtraTextureCardCount)
		{
			from = AnimationExtraTextureCardCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SelectedAnimationClipName)
		{
			value = VariantUtils.CreateFrom<string>(SelectedAnimationClipName);
			return true;
		}
		bool from2;
		if (name == PropertyName.PackedFrameEditingSupported)
		{
			from2 = PackedFrameEditingSupported;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SelectedAnimationFrame)
		{
			from = SelectedAnimationFrame;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SelectedAnimationSliceKey)
		{
			from = SelectedAnimationSliceKey;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationWorkbenchCurrentTab)
		{
			from = AnimationWorkbenchCurrentTab;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationMediaVisualChoiceCount)
		{
			from = AnimationMediaVisualChoiceCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationLayerVisualChoiceCount)
		{
			from = AnimationLayerVisualChoiceCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationHighFrequencyActive)
		{
			from2 = AnimationHighFrequencyActive;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.AnimationHydrationPending)
		{
			from2 = AnimationHydrationPending;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PendingAnimationFrameScale)
		{
			from = PendingAnimationFrameScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RequestedAnimationHydrationFrameScale)
		{
			from = RequestedAnimationHydrationFrameScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.LastAnimationSavePersistedTicks)
		{
			value = VariantUtils.CreateFrom<ulong>(LastAnimationSavePersistedTicks);
			return true;
		}
		if (name == PropertyName._animationPreviewStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _animationPreviewStatusLabel);
			return true;
		}
		if (name == PropertyName._animationDirectSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _animationDirectSummaryLabel);
			return true;
		}
		if (name == PropertyName._animationResourcePathLabel)
		{
			value = VariantUtils.CreateFrom(in _animationResourcePathLabel);
			return true;
		}
		if (name == PropertyName._animationRuntimeFrameLabel)
		{
			value = VariantUtils.CreateFrom(in _animationRuntimeFrameLabel);
			return true;
		}
		if (name == PropertyName._animationPlaybackStateLabel)
		{
			value = VariantUtils.CreateFrom(in _animationPlaybackStateLabel);
			return true;
		}
		if (name == PropertyName._animationClipList)
		{
			value = VariantUtils.CreateFrom(in _animationClipList);
			return true;
		}
		if (name == PropertyName._animationClipNameEdit)
		{
			value = VariantUtils.CreateFrom(in _animationClipNameEdit);
			return true;
		}
		if (name == PropertyName._animationClipStartSpinBox)
		{
			value = VariantUtils.CreateFrom(in _animationClipStartSpinBox);
			return true;
		}
		if (name == PropertyName._animationClipEndSpinBox)
		{
			value = VariantUtils.CreateFrom(in _animationClipEndSpinBox);
			return true;
		}
		if (name == PropertyName._animationClipRangeControl)
		{
			value = VariantUtils.CreateFrom(in _animationClipRangeControl);
			return true;
		}
		if (name == PropertyName._animationAddClipButton)
		{
			value = VariantUtils.CreateFrom(in _animationAddClipButton);
			return true;
		}
		if (name == PropertyName._animationDeleteClipButton)
		{
			value = VariantUtils.CreateFrom(in _animationDeleteClipButton);
			return true;
		}
		if (name == PropertyName._animationShiftClipEarlierButton)
		{
			value = VariantUtils.CreateFrom(in _animationShiftClipEarlierButton);
			return true;
		}
		if (name == PropertyName._animationShiftClipLaterButton)
		{
			value = VariantUtils.CreateFrom(in _animationShiftClipLaterButton);
			return true;
		}
		if (name == PropertyName._animationClipEditStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _animationClipEditStatusLabel);
			return true;
		}
		if (name == PropertyName._animationFrameTimeline)
		{
			value = VariantUtils.CreateFrom(in _animationFrameTimeline);
			return true;
		}
		if (name == PropertyName._animationAddKeyframeButton)
		{
			value = VariantUtils.CreateFrom(in _animationAddKeyframeButton);
			return true;
		}
		if (name == PropertyName._animationDuplicateKeyframeButton)
		{
			value = VariantUtils.CreateFrom(in _animationDuplicateKeyframeButton);
			return true;
		}
		if (name == PropertyName._animationDeleteKeyframeButton)
		{
			value = VariantUtils.CreateFrom(in _animationDeleteKeyframeButton);
			return true;
		}
		if (name == PropertyName._animationMoveKeyframeEarlierButton)
		{
			value = VariantUtils.CreateFrom(in _animationMoveKeyframeEarlierButton);
			return true;
		}
		if (name == PropertyName._animationMoveKeyframeLaterButton)
		{
			value = VariantUtils.CreateFrom(in _animationMoveKeyframeLaterButton);
			return true;
		}
		if (name == PropertyName._animationSelectedKeyframeLabel)
		{
			value = VariantUtils.CreateFrom(in _animationSelectedKeyframeLabel);
			return true;
		}
		if (name == PropertyName._animationMediaOption)
		{
			value = VariantUtils.CreateFrom(in _animationMediaOption);
			return true;
		}
		if (name == PropertyName._animationLayerOption)
		{
			value = VariantUtils.CreateFrom(in _animationLayerOption);
			return true;
		}
		if (name == PropertyName._animationMediaThumbnail)
		{
			value = VariantUtils.CreateFrom(in _animationMediaThumbnail);
			return true;
		}
		if (name == PropertyName._animationPositionXSpinBox)
		{
			value = VariantUtils.CreateFrom(in _animationPositionXSpinBox);
			return true;
		}
		if (name == PropertyName._animationPositionYSpinBox)
		{
			value = VariantUtils.CreateFrom(in _animationPositionYSpinBox);
			return true;
		}
		if (name == PropertyName._animationRotationSpinBox)
		{
			value = VariantUtils.CreateFrom(in _animationRotationSpinBox);
			return true;
		}
		if (name == PropertyName._animationSkewSpinBox)
		{
			value = VariantUtils.CreateFrom(in _animationSkewSpinBox);
			return true;
		}
		if (name == PropertyName._animationScaleXSpinBox)
		{
			value = VariantUtils.CreateFrom(in _animationScaleXSpinBox);
			return true;
		}
		if (name == PropertyName._animationScaleYSpinBox)
		{
			value = VariantUtils.CreateFrom(in _animationScaleYSpinBox);
			return true;
		}
		if (name == PropertyName._animationAlphaSlider)
		{
			value = VariantUtils.CreateFrom(in _animationAlphaSlider);
			return true;
		}
		if (name == PropertyName._animationAlphaPercentLabel)
		{
			value = VariantUtils.CreateFrom(in _animationAlphaPercentLabel);
			return true;
		}
		if (name == PropertyName._animationKeyframeEditStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _animationKeyframeEditStatusLabel);
			return true;
		}
		if (name == PropertyName._updatingAnimationFrameControls)
		{
			value = VariantUtils.CreateFrom(in _updatingAnimationFrameControls);
			return true;
		}
		if (name == PropertyName._animationFrameEditSnapshot)
		{
			value = VariantUtils.CreateFrom(in _animationFrameEditSnapshot);
			return true;
		}
		if (name == PropertyName._animationFrameEditTarget)
		{
			value = VariantUtils.CreateFrom(in _animationFrameEditTarget);
			return true;
		}
		if (name == PropertyName._animationFrameEditFrame)
		{
			value = VariantUtils.CreateFrom(in _animationFrameEditFrame);
			return true;
		}
		if (name == PropertyName._animationFrameEditSliceKey)
		{
			value = VariantUtils.CreateFrom(in _animationFrameEditSliceKey);
			return true;
		}
		if (name == PropertyName._animationFrameEditMutated)
		{
			value = VariantUtils.CreateFrom(in _animationFrameEditMutated);
			return true;
		}
		if (name == PropertyName._animationFrameMutationCount)
		{
			value = VariantUtils.CreateFrom(in _animationFrameMutationCount);
			return true;
		}
		if (name == PropertyName._animationDatFileButton)
		{
			value = VariantUtils.CreateFrom(in _animationDatFileButton);
			return true;
		}
		if (name == PropertyName._animationDatFileDialog)
		{
			value = VariantUtils.CreateFrom(in _animationDatFileDialog);
			return true;
		}
		if (name == PropertyName._animationReimportConfirmDialog)
		{
			value = VariantUtils.CreateFrom(in _animationReimportConfirmDialog);
			return true;
		}
		if (name == PropertyName._animationFrameScaleSpinBox)
		{
			value = VariantUtils.CreateFrom(in _animationFrameScaleSpinBox);
			return true;
		}
		if (name == PropertyName._updatingAnimationFrameScaleControl)
		{
			value = VariantUtils.CreateFrom(in _updatingAnimationFrameScaleControl);
			return true;
		}
		if (name == PropertyName._pendingAnimationReimportTarget)
		{
			value = VariantUtils.CreateFrom(in _pendingAnimationReimportTarget);
			return true;
		}
		if (name == PropertyName._pendingAnimationReimportPath)
		{
			value = VariantUtils.CreateFrom(in _pendingAnimationReimportPath);
			return true;
		}
		if (name == PropertyName._pendingAnimationFrameScale)
		{
			value = VariantUtils.CreateFrom(in _pendingAnimationFrameScale);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportOldSnapshot)
		{
			value = VariantUtils.CreateFrom(in _animationExplicitReimportOldSnapshot);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportFrame)
		{
			value = VariantUtils.CreateFrom(in _animationExplicitReimportFrame);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportSliceKey)
		{
			value = VariantUtils.CreateFrom(in _animationExplicitReimportSliceKey);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportActionName)
		{
			value = VariantUtils.CreateFrom(in _animationExplicitReimportActionName);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportSuccessMessage)
		{
			value = VariantUtils.CreateFrom(in _animationExplicitReimportSuccessMessage);
			return true;
		}
		if (name == PropertyName._animationExplicitReimportRequiresRestore)
		{
			value = VariantUtils.CreateFrom(in _animationExplicitReimportRequiresRestore);
			return true;
		}
		if (name == PropertyName._runtimePreview)
		{
			value = VariantUtils.CreateFrom(in _runtimePreview);
			return true;
		}
		if (name == PropertyName._animationDatSelectionTarget)
		{
			value = VariantUtils.CreateFrom(in _animationDatSelectionTarget);
			return true;
		}
		if (name == PropertyName._animationHydrationPath)
		{
			value = VariantUtils.CreateFrom(in _animationHydrationPath);
			return true;
		}
		if (name == PropertyName._animationHydrationTarget)
		{
			value = VariantUtils.CreateFrom(in _animationHydrationTarget);
			return true;
		}
		if (name == PropertyName._animationHydrationRequestedFrameScale)
		{
			value = VariantUtils.CreateFrom(in _animationHydrationRequestedFrameScale);
			return true;
		}
		if (name == PropertyName._animationHydrationExpectedFrameScale)
		{
			value = VariantUtils.CreateFrom(in _animationHydrationExpectedFrameScale);
			return true;
		}
		if (name == PropertyName._animationHydrationExpectedAuthoringRevision)
		{
			value = VariantUtils.CreateFrom(in _animationHydrationExpectedAuthoringRevision);
			return true;
		}
		if (name == PropertyName._animationStateUpdateCount)
		{
			value = VariantUtils.CreateFrom(in _animationStateUpdateCount);
			return true;
		}
		if (name == PropertyName._selectedAnimationClipName)
		{
			value = VariantUtils.CreateFrom(in _selectedAnimationClipName);
			return true;
		}
		if (name == PropertyName._updatingAnimationClipControls)
		{
			value = VariantUtils.CreateFrom(in _updatingAnimationClipControls);
			return true;
		}
		if (name == PropertyName._animationClipEditSnapshot)
		{
			value = VariantUtils.CreateFrom(in _animationClipEditSnapshot);
			return true;
		}
		if (name == PropertyName._animationClipEditName)
		{
			value = VariantUtils.CreateFrom(in _animationClipEditName);
			return true;
		}
		if (name == PropertyName._animationClipEditTarget)
		{
			value = VariantUtils.CreateFrom(in _animationClipEditTarget);
			return true;
		}
		if (name == PropertyName._animationClipMutationCount)
		{
			value = VariantUtils.CreateFrom(in _animationClipMutationCount);
			return true;
		}
		if (name == PropertyName._editingAnimation)
		{
			value = VariantUtils.CreateFrom(in _editingAnimation);
			return true;
		}
		if (name == PropertyName._animationReplacementSlotCards)
		{
			value = VariantUtils.CreateFrom(in _animationReplacementSlotCards);
			return true;
		}
		if (name == PropertyName._animationExtraTextureCards)
		{
			value = VariantUtils.CreateFrom(in _animationExtraTextureCards);
			return true;
		}
		if (name == PropertyName._animationReplacementSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _animationReplacementSummaryLabel);
			return true;
		}
		if (name == PropertyName._animationReplacementStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _animationReplacementStatusLabel);
			return true;
		}
		if (name == PropertyName._animationAddReplacementSlotButton)
		{
			value = VariantUtils.CreateFrom(in _animationAddReplacementSlotButton);
			return true;
		}
		if (name == PropertyName._animationAddExtraTextureButton)
		{
			value = VariantUtils.CreateFrom(in _animationAddExtraTextureButton);
			return true;
		}
		if (name == PropertyName._animationReplacementSaveButton)
		{
			value = VariantUtils.CreateFrom(in _animationReplacementSaveButton);
			return true;
		}
		if (name == PropertyName._animationReplacementReloadButton)
		{
			value = VariantUtils.CreateFrom(in _animationReplacementReloadButton);
			return true;
		}
		if (name == PropertyName._animationReplacementReloadConfirmDialog)
		{
			value = VariantUtils.CreateFrom(in _animationReplacementReloadConfirmDialog);
			return true;
		}
		if (name == PropertyName._animationReplacementPicker)
		{
			value = VariantUtils.CreateFrom(in _animationReplacementPicker);
			return true;
		}
		if (name == PropertyName._animationReplacementMutationCount)
		{
			value = VariantUtils.CreateFrom(in _animationReplacementMutationCount);
			return true;
		}
		if (name == PropertyName._animationReplacementSlotCardCount)
		{
			value = VariantUtils.CreateFrom(in _animationReplacementSlotCardCount);
			return true;
		}
		if (name == PropertyName._animationExtraTextureCardCount)
		{
			value = VariantUtils.CreateFrom(in _animationExtraTextureCardCount);
			return true;
		}
		if (name == PropertyName._animationWorkbench)
		{
			value = VariantUtils.CreateFrom(in _animationWorkbench);
			return true;
		}
		if (name == PropertyName._animationWorkbenchTabs)
		{
			value = VariantUtils.CreateFrom(in _animationWorkbenchTabs);
			return true;
		}
		if (name == PropertyName._animationChoiceSurface)
		{
			value = VariantUtils.CreateFrom(in _animationChoiceSurface);
			return true;
		}
		if (name == PropertyName._animationSourceHost)
		{
			value = VariantUtils.CreateFrom(in _animationSourceHost);
			return true;
		}
		if (name == PropertyName._animationSourceContainer)
		{
			value = VariantUtils.CreateFrom(in _animationSourceContainer);
			return true;
		}
		if (name == PropertyName._animationMediaChoiceGrid)
		{
			value = VariantUtils.CreateFrom(in _animationMediaChoiceGrid);
			return true;
		}
		if (name == PropertyName._animationLayerChoiceGrid)
		{
			value = VariantUtils.CreateFrom(in _animationLayerChoiceGrid);
			return true;
		}
		if (name == PropertyName._animationWorkbenchSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _animationWorkbenchSignalsConnected);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._animationPreviewStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationDirectSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationResourcePathLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationRuntimeFrameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationPlaybackStateLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationClipList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationClipNameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationClipStartSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationClipEndSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationClipRangeControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationAddClipButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationDeleteClipButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationShiftClipEarlierButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationShiftClipLaterButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationClipEditStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationFrameTimeline, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationAddKeyframeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationDuplicateKeyframeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationDeleteKeyframeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationMoveKeyframeEarlierButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationMoveKeyframeLaterButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationSelectedKeyframeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationMediaOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationLayerOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationMediaThumbnail, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationPositionXSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationPositionYSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationRotationSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationSkewSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationScaleXSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationScaleYSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationAlphaSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationAlphaPercentLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationKeyframeEditStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingAnimationFrameControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._animationFrameEditSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationFrameEditTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationFrameEditFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationFrameEditSliceKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._animationFrameEditMutated, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationFrameMutationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationDatFileButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationDatFileDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationReimportConfirmDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationFrameScaleSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingAnimationFrameScaleControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingAnimationReimportTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingAnimationReimportPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pendingAnimationFrameScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._animationExplicitReimportOldSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationExplicitReimportFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationExplicitReimportSliceKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._animationExplicitReimportActionName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._animationExplicitReimportSuccessMessage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._animationExplicitReimportRequiresRestore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationDatSelectionTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._animationHydrationPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationHydrationTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationHydrationRequestedFrameScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationHydrationExpectedFrameScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationHydrationExpectedAuthoringRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationStateUpdateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._selectedAnimationClipName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingAnimationClipControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._animationClipEditSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._animationClipEditName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationClipEditTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationClipMutationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingAnimation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationReplacementSlotCards, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationExtraTextureCards, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationReplacementSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationReplacementStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationAddReplacementSlotButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationAddExtraTextureButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationReplacementSaveButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationReplacementReloadButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationReplacementReloadConfirmDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationReplacementPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationReplacementMutationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationReplacementSlotCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationExtraTextureCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationWorkbench, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationWorkbenchTabs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationChoiceSurface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationSourceHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationSourceContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationMediaChoiceGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationLayerChoiceGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._animationWorkbenchSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AnimationStateUpdateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AnimationClipMutationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AnimationFrameMutationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AnimationReplacementMutationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AnimationReplacementSlotCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AnimationExtraTextureCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.SelectedAnimationClipName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.PackedFrameEditingSupported, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedAnimationFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedAnimationSliceKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AnimationWorkbenchCurrentTab, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AnimationMediaVisualChoiceCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AnimationLayerVisualChoiceCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AnimationHighFrequencyActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AnimationHydrationPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PendingAnimationFrameScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RequestedAnimationHydrationFrameScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LastAnimationSavePersistedTicks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.LastAnimationSavePersistedTicks, Variant.From<ulong>(LastAnimationSavePersistedTicks));
		info.AddProperty(PropertyName._animationPreviewStatusLabel, Variant.From(in _animationPreviewStatusLabel));
		info.AddProperty(PropertyName._animationDirectSummaryLabel, Variant.From(in _animationDirectSummaryLabel));
		info.AddProperty(PropertyName._animationResourcePathLabel, Variant.From(in _animationResourcePathLabel));
		info.AddProperty(PropertyName._animationRuntimeFrameLabel, Variant.From(in _animationRuntimeFrameLabel));
		info.AddProperty(PropertyName._animationPlaybackStateLabel, Variant.From(in _animationPlaybackStateLabel));
		info.AddProperty(PropertyName._animationClipList, Variant.From(in _animationClipList));
		info.AddProperty(PropertyName._animationClipNameEdit, Variant.From(in _animationClipNameEdit));
		info.AddProperty(PropertyName._animationClipStartSpinBox, Variant.From(in _animationClipStartSpinBox));
		info.AddProperty(PropertyName._animationClipEndSpinBox, Variant.From(in _animationClipEndSpinBox));
		info.AddProperty(PropertyName._animationClipRangeControl, Variant.From(in _animationClipRangeControl));
		info.AddProperty(PropertyName._animationAddClipButton, Variant.From(in _animationAddClipButton));
		info.AddProperty(PropertyName._animationDeleteClipButton, Variant.From(in _animationDeleteClipButton));
		info.AddProperty(PropertyName._animationShiftClipEarlierButton, Variant.From(in _animationShiftClipEarlierButton));
		info.AddProperty(PropertyName._animationShiftClipLaterButton, Variant.From(in _animationShiftClipLaterButton));
		info.AddProperty(PropertyName._animationClipEditStatusLabel, Variant.From(in _animationClipEditStatusLabel));
		info.AddProperty(PropertyName._animationFrameTimeline, Variant.From(in _animationFrameTimeline));
		info.AddProperty(PropertyName._animationAddKeyframeButton, Variant.From(in _animationAddKeyframeButton));
		info.AddProperty(PropertyName._animationDuplicateKeyframeButton, Variant.From(in _animationDuplicateKeyframeButton));
		info.AddProperty(PropertyName._animationDeleteKeyframeButton, Variant.From(in _animationDeleteKeyframeButton));
		info.AddProperty(PropertyName._animationMoveKeyframeEarlierButton, Variant.From(in _animationMoveKeyframeEarlierButton));
		info.AddProperty(PropertyName._animationMoveKeyframeLaterButton, Variant.From(in _animationMoveKeyframeLaterButton));
		info.AddProperty(PropertyName._animationSelectedKeyframeLabel, Variant.From(in _animationSelectedKeyframeLabel));
		info.AddProperty(PropertyName._animationMediaOption, Variant.From(in _animationMediaOption));
		info.AddProperty(PropertyName._animationLayerOption, Variant.From(in _animationLayerOption));
		info.AddProperty(PropertyName._animationMediaThumbnail, Variant.From(in _animationMediaThumbnail));
		info.AddProperty(PropertyName._animationPositionXSpinBox, Variant.From(in _animationPositionXSpinBox));
		info.AddProperty(PropertyName._animationPositionYSpinBox, Variant.From(in _animationPositionYSpinBox));
		info.AddProperty(PropertyName._animationRotationSpinBox, Variant.From(in _animationRotationSpinBox));
		info.AddProperty(PropertyName._animationSkewSpinBox, Variant.From(in _animationSkewSpinBox));
		info.AddProperty(PropertyName._animationScaleXSpinBox, Variant.From(in _animationScaleXSpinBox));
		info.AddProperty(PropertyName._animationScaleYSpinBox, Variant.From(in _animationScaleYSpinBox));
		info.AddProperty(PropertyName._animationAlphaSlider, Variant.From(in _animationAlphaSlider));
		info.AddProperty(PropertyName._animationAlphaPercentLabel, Variant.From(in _animationAlphaPercentLabel));
		info.AddProperty(PropertyName._animationKeyframeEditStatusLabel, Variant.From(in _animationKeyframeEditStatusLabel));
		info.AddProperty(PropertyName._updatingAnimationFrameControls, Variant.From(in _updatingAnimationFrameControls));
		info.AddProperty(PropertyName._animationFrameEditSnapshot, Variant.From(in _animationFrameEditSnapshot));
		info.AddProperty(PropertyName._animationFrameEditTarget, Variant.From(in _animationFrameEditTarget));
		info.AddProperty(PropertyName._animationFrameEditFrame, Variant.From(in _animationFrameEditFrame));
		info.AddProperty(PropertyName._animationFrameEditSliceKey, Variant.From(in _animationFrameEditSliceKey));
		info.AddProperty(PropertyName._animationFrameEditMutated, Variant.From(in _animationFrameEditMutated));
		info.AddProperty(PropertyName._animationFrameMutationCount, Variant.From(in _animationFrameMutationCount));
		info.AddProperty(PropertyName._animationDatFileButton, Variant.From(in _animationDatFileButton));
		info.AddProperty(PropertyName._animationDatFileDialog, Variant.From(in _animationDatFileDialog));
		info.AddProperty(PropertyName._animationReimportConfirmDialog, Variant.From(in _animationReimportConfirmDialog));
		info.AddProperty(PropertyName._animationFrameScaleSpinBox, Variant.From(in _animationFrameScaleSpinBox));
		info.AddProperty(PropertyName._updatingAnimationFrameScaleControl, Variant.From(in _updatingAnimationFrameScaleControl));
		info.AddProperty(PropertyName._pendingAnimationReimportTarget, Variant.From(in _pendingAnimationReimportTarget));
		info.AddProperty(PropertyName._pendingAnimationReimportPath, Variant.From(in _pendingAnimationReimportPath));
		info.AddProperty(PropertyName._pendingAnimationFrameScale, Variant.From(in _pendingAnimationFrameScale));
		info.AddProperty(PropertyName._animationExplicitReimportOldSnapshot, Variant.From(in _animationExplicitReimportOldSnapshot));
		info.AddProperty(PropertyName._animationExplicitReimportFrame, Variant.From(in _animationExplicitReimportFrame));
		info.AddProperty(PropertyName._animationExplicitReimportSliceKey, Variant.From(in _animationExplicitReimportSliceKey));
		info.AddProperty(PropertyName._animationExplicitReimportActionName, Variant.From(in _animationExplicitReimportActionName));
		info.AddProperty(PropertyName._animationExplicitReimportSuccessMessage, Variant.From(in _animationExplicitReimportSuccessMessage));
		info.AddProperty(PropertyName._animationExplicitReimportRequiresRestore, Variant.From(in _animationExplicitReimportRequiresRestore));
		info.AddProperty(PropertyName._runtimePreview, Variant.From(in _runtimePreview));
		info.AddProperty(PropertyName._animationDatSelectionTarget, Variant.From(in _animationDatSelectionTarget));
		info.AddProperty(PropertyName._animationHydrationPath, Variant.From(in _animationHydrationPath));
		info.AddProperty(PropertyName._animationHydrationTarget, Variant.From(in _animationHydrationTarget));
		info.AddProperty(PropertyName._animationHydrationRequestedFrameScale, Variant.From(in _animationHydrationRequestedFrameScale));
		info.AddProperty(PropertyName._animationHydrationExpectedFrameScale, Variant.From(in _animationHydrationExpectedFrameScale));
		info.AddProperty(PropertyName._animationHydrationExpectedAuthoringRevision, Variant.From(in _animationHydrationExpectedAuthoringRevision));
		info.AddProperty(PropertyName._animationStateUpdateCount, Variant.From(in _animationStateUpdateCount));
		info.AddProperty(PropertyName._selectedAnimationClipName, Variant.From(in _selectedAnimationClipName));
		info.AddProperty(PropertyName._updatingAnimationClipControls, Variant.From(in _updatingAnimationClipControls));
		info.AddProperty(PropertyName._animationClipEditSnapshot, Variant.From(in _animationClipEditSnapshot));
		info.AddProperty(PropertyName._animationClipEditName, Variant.From(in _animationClipEditName));
		info.AddProperty(PropertyName._animationClipEditTarget, Variant.From(in _animationClipEditTarget));
		info.AddProperty(PropertyName._animationClipMutationCount, Variant.From(in _animationClipMutationCount));
		info.AddProperty(PropertyName._editingAnimation, Variant.From(in _editingAnimation));
		info.AddProperty(PropertyName._animationReplacementSlotCards, Variant.From(in _animationReplacementSlotCards));
		info.AddProperty(PropertyName._animationExtraTextureCards, Variant.From(in _animationExtraTextureCards));
		info.AddProperty(PropertyName._animationReplacementSummaryLabel, Variant.From(in _animationReplacementSummaryLabel));
		info.AddProperty(PropertyName._animationReplacementStatusLabel, Variant.From(in _animationReplacementStatusLabel));
		info.AddProperty(PropertyName._animationAddReplacementSlotButton, Variant.From(in _animationAddReplacementSlotButton));
		info.AddProperty(PropertyName._animationAddExtraTextureButton, Variant.From(in _animationAddExtraTextureButton));
		info.AddProperty(PropertyName._animationReplacementSaveButton, Variant.From(in _animationReplacementSaveButton));
		info.AddProperty(PropertyName._animationReplacementReloadButton, Variant.From(in _animationReplacementReloadButton));
		info.AddProperty(PropertyName._animationReplacementReloadConfirmDialog, Variant.From(in _animationReplacementReloadConfirmDialog));
		info.AddProperty(PropertyName._animationReplacementPicker, Variant.From(in _animationReplacementPicker));
		info.AddProperty(PropertyName._animationReplacementMutationCount, Variant.From(in _animationReplacementMutationCount));
		info.AddProperty(PropertyName._animationReplacementSlotCardCount, Variant.From(in _animationReplacementSlotCardCount));
		info.AddProperty(PropertyName._animationExtraTextureCardCount, Variant.From(in _animationExtraTextureCardCount));
		info.AddProperty(PropertyName._animationWorkbench, Variant.From(in _animationWorkbench));
		info.AddProperty(PropertyName._animationWorkbenchTabs, Variant.From(in _animationWorkbenchTabs));
		info.AddProperty(PropertyName._animationChoiceSurface, Variant.From(in _animationChoiceSurface));
		info.AddProperty(PropertyName._animationSourceHost, Variant.From(in _animationSourceHost));
		info.AddProperty(PropertyName._animationSourceContainer, Variant.From(in _animationSourceContainer));
		info.AddProperty(PropertyName._animationMediaChoiceGrid, Variant.From(in _animationMediaChoiceGrid));
		info.AddProperty(PropertyName._animationLayerChoiceGrid, Variant.From(in _animationLayerChoiceGrid));
		info.AddProperty(PropertyName._animationWorkbenchSignalsConnected, Variant.From(in _animationWorkbenchSignalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.LastAnimationSavePersistedTicks, out var value))
		{
			LastAnimationSavePersistedTicks = value.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._animationPreviewStatusLabel, out var value2))
		{
			_animationPreviewStatusLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationDirectSummaryLabel, out var value3))
		{
			_animationDirectSummaryLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationResourcePathLabel, out var value4))
		{
			_animationResourcePathLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationRuntimeFrameLabel, out var value5))
		{
			_animationRuntimeFrameLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationPlaybackStateLabel, out var value6))
		{
			_animationPlaybackStateLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationClipList, out var value7))
		{
			_animationClipList = value7.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._animationClipNameEdit, out var value8))
		{
			_animationClipNameEdit = value8.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._animationClipStartSpinBox, out var value9))
		{
			_animationClipStartSpinBox = value9.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._animationClipEndSpinBox, out var value10))
		{
			_animationClipEndSpinBox = value10.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._animationClipRangeControl, out var value11))
		{
			_animationClipRangeControl = value11.As<XWPacketSpawnEntryRangeControl>();
		}
		if (info.TryGetProperty(PropertyName._animationAddClipButton, out var value12))
		{
			_animationAddClipButton = value12.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationDeleteClipButton, out var value13))
		{
			_animationDeleteClipButton = value13.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationShiftClipEarlierButton, out var value14))
		{
			_animationShiftClipEarlierButton = value14.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationShiftClipLaterButton, out var value15))
		{
			_animationShiftClipLaterButton = value15.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationClipEditStatusLabel, out var value16))
		{
			_animationClipEditStatusLabel = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationFrameTimeline, out var value17))
		{
			_animationFrameTimeline = value17.As<XWAnimationFrameTimeline>();
		}
		if (info.TryGetProperty(PropertyName._animationAddKeyframeButton, out var value18))
		{
			_animationAddKeyframeButton = value18.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationDuplicateKeyframeButton, out var value19))
		{
			_animationDuplicateKeyframeButton = value19.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationDeleteKeyframeButton, out var value20))
		{
			_animationDeleteKeyframeButton = value20.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationMoveKeyframeEarlierButton, out var value21))
		{
			_animationMoveKeyframeEarlierButton = value21.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationMoveKeyframeLaterButton, out var value22))
		{
			_animationMoveKeyframeLaterButton = value22.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationSelectedKeyframeLabel, out var value23))
		{
			_animationSelectedKeyframeLabel = value23.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationMediaOption, out var value24))
		{
			_animationMediaOption = value24.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._animationLayerOption, out var value25))
		{
			_animationLayerOption = value25.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._animationMediaThumbnail, out var value26))
		{
			_animationMediaThumbnail = value26.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._animationPositionXSpinBox, out var value27))
		{
			_animationPositionXSpinBox = value27.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._animationPositionYSpinBox, out var value28))
		{
			_animationPositionYSpinBox = value28.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._animationRotationSpinBox, out var value29))
		{
			_animationRotationSpinBox = value29.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._animationSkewSpinBox, out var value30))
		{
			_animationSkewSpinBox = value30.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._animationScaleXSpinBox, out var value31))
		{
			_animationScaleXSpinBox = value31.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._animationScaleYSpinBox, out var value32))
		{
			_animationScaleYSpinBox = value32.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._animationAlphaSlider, out var value33))
		{
			_animationAlphaSlider = value33.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._animationAlphaPercentLabel, out var value34))
		{
			_animationAlphaPercentLabel = value34.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationKeyframeEditStatusLabel, out var value35))
		{
			_animationKeyframeEditStatusLabel = value35.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._updatingAnimationFrameControls, out var value36))
		{
			_updatingAnimationFrameControls = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._animationFrameEditSnapshot, out var value37))
		{
			_animationFrameEditSnapshot = value37.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._animationFrameEditTarget, out var value38))
		{
			_animationFrameEditTarget = value38.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._animationFrameEditFrame, out var value39))
		{
			_animationFrameEditFrame = value39.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationFrameEditSliceKey, out var value40))
		{
			_animationFrameEditSliceKey = value40.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationFrameEditMutated, out var value41))
		{
			_animationFrameEditMutated = value41.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._animationFrameMutationCount, out var value42))
		{
			_animationFrameMutationCount = value42.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationDatFileButton, out var value43))
		{
			_animationDatFileButton = value43.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationDatFileDialog, out var value44))
		{
			_animationDatFileDialog = value44.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._animationReimportConfirmDialog, out var value45))
		{
			_animationReimportConfirmDialog = value45.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._animationFrameScaleSpinBox, out var value46))
		{
			_animationFrameScaleSpinBox = value46.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._updatingAnimationFrameScaleControl, out var value47))
		{
			_updatingAnimationFrameScaleControl = value47.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingAnimationReimportTarget, out var value48))
		{
			_pendingAnimationReimportTarget = value48.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._pendingAnimationReimportPath, out var value49))
		{
			_pendingAnimationReimportPath = value49.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingAnimationFrameScale, out var value50))
		{
			_pendingAnimationFrameScale = value50.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationExplicitReimportOldSnapshot, out var value51))
		{
			_animationExplicitReimportOldSnapshot = value51.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._animationExplicitReimportFrame, out var value52))
		{
			_animationExplicitReimportFrame = value52.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationExplicitReimportSliceKey, out var value53))
		{
			_animationExplicitReimportSliceKey = value53.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationExplicitReimportActionName, out var value54))
		{
			_animationExplicitReimportActionName = value54.As<string>();
		}
		if (info.TryGetProperty(PropertyName._animationExplicitReimportSuccessMessage, out var value55))
		{
			_animationExplicitReimportSuccessMessage = value55.As<string>();
		}
		if (info.TryGetProperty(PropertyName._animationExplicitReimportRequiresRestore, out var value56))
		{
			_animationExplicitReimportRequiresRestore = value56.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreview, out var value57))
		{
			_runtimePreview = value57.As<AdobeAnimateInspectorPreview>();
		}
		if (info.TryGetProperty(PropertyName._animationDatSelectionTarget, out var value58))
		{
			_animationDatSelectionTarget = value58.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._animationHydrationPath, out var value59))
		{
			_animationHydrationPath = value59.As<string>();
		}
		if (info.TryGetProperty(PropertyName._animationHydrationTarget, out var value60))
		{
			_animationHydrationTarget = value60.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._animationHydrationRequestedFrameScale, out var value61))
		{
			_animationHydrationRequestedFrameScale = value61.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationHydrationExpectedFrameScale, out var value62))
		{
			_animationHydrationExpectedFrameScale = value62.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationHydrationExpectedAuthoringRevision, out var value63))
		{
			_animationHydrationExpectedAuthoringRevision = value63.As<long>();
		}
		if (info.TryGetProperty(PropertyName._animationStateUpdateCount, out var value64))
		{
			_animationStateUpdateCount = value64.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedAnimationClipName, out var value65))
		{
			_selectedAnimationClipName = value65.As<string>();
		}
		if (info.TryGetProperty(PropertyName._updatingAnimationClipControls, out var value66))
		{
			_updatingAnimationClipControls = value66.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._animationClipEditSnapshot, out var value67))
		{
			_animationClipEditSnapshot = value67.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._animationClipEditName, out var value68))
		{
			_animationClipEditName = value68.As<string>();
		}
		if (info.TryGetProperty(PropertyName._animationClipEditTarget, out var value69))
		{
			_animationClipEditTarget = value69.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._animationClipMutationCount, out var value70))
		{
			_animationClipMutationCount = value70.As<int>();
		}
		if (info.TryGetProperty(PropertyName._editingAnimation, out var value71))
		{
			_editingAnimation = value71.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._animationReplacementSlotCards, out var value72))
		{
			_animationReplacementSlotCards = value72.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._animationExtraTextureCards, out var value73))
		{
			_animationExtraTextureCards = value73.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._animationReplacementSummaryLabel, out var value74))
		{
			_animationReplacementSummaryLabel = value74.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationReplacementStatusLabel, out var value75))
		{
			_animationReplacementStatusLabel = value75.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationAddReplacementSlotButton, out var value76))
		{
			_animationAddReplacementSlotButton = value76.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationAddExtraTextureButton, out var value77))
		{
			_animationAddExtraTextureButton = value77.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationReplacementSaveButton, out var value78))
		{
			_animationReplacementSaveButton = value78.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationReplacementReloadButton, out var value79))
		{
			_animationReplacementReloadButton = value79.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._animationReplacementReloadConfirmDialog, out var value80))
		{
			_animationReplacementReloadConfirmDialog = value80.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._animationReplacementPicker, out var value81))
		{
			_animationReplacementPicker = value81.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._animationReplacementMutationCount, out var value82))
		{
			_animationReplacementMutationCount = value82.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationReplacementSlotCardCount, out var value83))
		{
			_animationReplacementSlotCardCount = value83.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationExtraTextureCardCount, out var value84))
		{
			_animationExtraTextureCardCount = value84.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationWorkbench, out var value85))
		{
			_animationWorkbench = value85.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._animationWorkbenchTabs, out var value86))
		{
			_animationWorkbenchTabs = value86.As<TabBar>();
		}
		if (info.TryGetProperty(PropertyName._animationChoiceSurface, out var value87))
		{
			_animationChoiceSurface = value87.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._animationSourceHost, out var value88))
		{
			_animationSourceHost = value88.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._animationSourceContainer, out var value89))
		{
			_animationSourceContainer = value89.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._animationMediaChoiceGrid, out var value90))
		{
			_animationMediaChoiceGrid = value90.As<XWAnimationVisualChoiceGrid>();
		}
		if (info.TryGetProperty(PropertyName._animationLayerChoiceGrid, out var value91))
		{
			_animationLayerChoiceGrid = value91.As<XWAnimationVisualChoiceGrid>();
		}
		if (info.TryGetProperty(PropertyName._animationWorkbenchSignalsConnected, out var value92))
		{
			_animationWorkbenchSignalsConnected = value92.As<bool>();
		}
	}
}
