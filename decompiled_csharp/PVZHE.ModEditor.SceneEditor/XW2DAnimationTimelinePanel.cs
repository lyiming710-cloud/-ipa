using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.GUI;
using PVZHE.ModEditor.ResourceEditors.GUI;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/2D/Animation/XW2DAnimationTimelinePanel.cs")]
public class XW2DAnimationTimelinePanel : PanelContainer
{
	private readonly struct PropertyChoice(Node node, string property, string label, Variant value)
	{
		public readonly Node Node = node;

		public readonly string Property = property;

		public readonly string Label = label;

		public readonly Variant Value = value;
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName BindEditor = "BindEditor";

		public static readonly StringName BindScene = "BindScene";

		public static readonly StringName CaptureState = "CaptureState";

		public static readonly StringName RefreshSelection = "RefreshSelection";

		public static readonly StringName SuspendForSave = "SuspendForSave";

		public static readonly StringName ResumeAfterSave = "ResumeAfterSave";

		public static readonly StringName RefreshFromSceneHistory = "RefreshFromSceneHistory";

		public static readonly StringName ClearSurface = "ClearSurface";

		public static readonly StringName CreateAnimation = "CreateAnimation";

		public static readonly StringName SelectAnimation = "SelectAnimation";

		public static readonly StringName SelectKey = "SelectKey";

		public static readonly StringName SeekPreview = "SeekPreview";

		public static readonly StringName PlayPreview = "PlayPreview";

		public static readonly StringName PausePreview = "PausePreview";

		public static readonly StringName AddValueTrack = "AddValueTrack";

		public static readonly StringName SetSelectedTrackInterpolation = "SetSelectedTrackInterpolation";

		public static readonly StringName SetSelectedValueUpdateMode = "SetSelectedValueUpdateMode";

		public static readonly StringName SetSelectedTrackInterpolationLoopWrap = "SetSelectedTrackInterpolationLoopWrap";

		public static readonly StringName SetSelectedKeyTransition = "SetSelectedKeyTransition";

		public static readonly StringName InsertCurrentKey = "InsertCurrentKey";

		public static readonly StringName MoveSelectedKey = "MoveSelectedKey";

		public static readonly StringName DeleteSelectedKey = "DeleteSelectedKey";

		public static readonly StringName BuildInterface = "BuildInterface";

		public static readonly StringName RefreshWorkbenchLayout = "RefreshWorkbenchLayout";

		public static readonly StringName BuildMotionCurveCard = "BuildMotionCurveCard";

		public static readonly StringName AddEasePreset = "AddEasePreset";

		public static readonly StringName OnTrackInterpolationSelected = "OnTrackInterpolationSelected";

		public static readonly StringName OnValueUpdateModeSelected = "OnValueUpdateModeSelected";

		public static readonly StringName OnEaseTransitionPreviewChanged = "OnEaseTransitionPreviewChanged";

		public static readonly StringName OnEaseTransitionCommitRequested = "OnEaseTransitionCommitRequested";

		public static readonly StringName BeginTransitionSpinEdit = "BeginTransitionSpinEdit";

		public static readonly StringName OnTransitionSpinValueChanged = "OnTransitionSpinValueChanged";

		public static readonly StringName CommitTransitionSpinEdit = "CommitTransitionSpinEdit";

		public static readonly StringName CommitPendingTransitionInput = "CommitPendingTransitionInput";

		public static readonly StringName RefreshMotionCurveEditor = "RefreshMotionCurveEditor";

		public static readonly StringName StyleMotionSegments = "StyleMotionSegments";

		public static readonly StringName MakeSmallHeading = "MakeSmallHeading";

		public static readonly StringName SelectOptionById = "SelectOptionById";

		public static readonly StringName LoadClassIcon = "LoadClassIcon";

		public static readonly StringName BuildDialogs = "BuildDialogs";

		public static readonly StringName RefreshSources = "RefreshSources";

		public static readonly StringName RefreshLibraries = "RefreshLibraries";

		public static readonly StringName RefreshAnimations = "RefreshAnimations";

		public static readonly StringName BindAnimation = "BindAnimation";

		public static readonly StringName BindSafePreview = "BindSafePreview";

		public static readonly StringName RefreshTrackList = "RefreshTrackList";

		public static readonly StringName RefreshTrackSelection = "RefreshTrackSelection";

		public static readonly StringName RefreshValueEditor = "RefreshValueEditor";

		public static readonly StringName CreateVariantEditor = "CreateVariantEditor";

		public static readonly StringName SetSelectedKeyValue = "SetSelectedKeyValue";

		public static readonly StringName ApplyAnimationSnapshot = "ApplyAnimationSnapshot";

		public static readonly StringName CommitAnimationSlot = "CommitAnimationSlot";

		public static readonly StringName SetAnimationSlot = "SetAnimationSlot";

		public static readonly StringName DuplicateCurrentAnimation = "DuplicateCurrentAnimation";

		public static readonly StringName DeleteCurrentAnimation = "DeleteCurrentAnimation";

		public static readonly StringName RenameCurrentAnimation = "RenameCurrentAnimation";

		public static readonly StringName RenameAnimationSlot = "RenameAnimationSlot";

		public static readonly StringName CopyCurrentAnimationToScene = "CopyCurrentAnimationToScene";

		public static readonly StringName SetLocalAnimationCopy = "SetLocalAnimationCopy";

		public static readonly StringName DeleteSelectedTrack = "DeleteSelectedTrack";

		public static readonly StringName MoveSelectedTrack = "MoveSelectedTrack";

		public static readonly StringName ToggleSelectedTrack = "ToggleSelectedTrack";

		public static readonly StringName DuplicateSelectedKey = "DuplicateSelectedKey";

		public static readonly StringName OnTimelinePlayheadChanged = "OnTimelinePlayheadChanged";

		public static readonly StringName OnTimelineKeySelected = "OnTimelineKeySelected";

		public static readonly StringName OnTimelineKeyMoveRequested = "OnTimelineKeyMoveRequested";

		public static readonly StringName OnTrackListSelected = "OnTrackListSelected";

		public static readonly StringName TogglePlayback = "TogglePlayback";

		public static readonly StringName StopPreview = "StopPreview";

		public static readonly StringName PauseForMutation = "PauseForMutation";

		public static readonly StringName JumpToKey = "JumpToKey";

		public static readonly StringName OnLoopToggled = "OnLoopToggled";

		public static readonly StringName OnSpeedChanged = "OnSpeedChanged";

		public static readonly StringName OnLengthChanged = "OnLengthChanged";

		public static readonly StringName OnStepChanged = "OnStepChanged";

		public static readonly StringName OnPlayerSelected = "OnPlayerSelected";

		public static readonly StringName OnLibrarySelected = "OnLibrarySelected";

		public static readonly StringName OnAnimationSelected = "OnAnimationSelected";

		public static readonly StringName ApplyPendingState = "ApplyPendingState";

		public static readonly StringName UpdateSourceInformation = "UpdateSourceInformation";

		public static readonly StringName UpdateAnimationTreeLink = "UpdateAnimationTreeLink";

		public static readonly StringName UpdateEditingState = "UpdateEditingState";

		public static readonly StringName UpdatePlaybackButton = "UpdatePlaybackButton";

		public static readonly StringName UpdateProcessing = "UpdateProcessing";

		public static readonly StringName UpdateTimeLabel = "UpdateTimeLabel";

		public static readonly StringName EnsureEditable = "EnsureEditable";

		public static readonly StringName IsCurrentAnimationReadOnly = "IsCurrentAnimationReadOnly";

		public static readonly StringName IsLibraryExternal = "IsLibraryExternal";

		public static readonly StringName IsResourceExternal = "IsResourceExternal";

		public static readonly StringName IsEditableValueTrack = "IsEditableValueTrack";

		public static readonly StringName ClampTime = "ClampTime";

		public static readonly StringName GetStep = "GetStep";

		public static readonly StringName GetSceneHistoryId = "GetSceneHistoryId";

		public static readonly StringName GetSelectedLibraryName = "GetSelectedLibraryName";

		public static readonly StringName GetSelectedAnimationName = "GetSelectedAnimationName";

		public static readonly StringName BuildUniqueAnimationName = "BuildUniqueAnimationName";

		public static readonly StringName BuildUniqueAnimationNameAcrossLibrary = "BuildUniqueAnimationNameAcrossLibrary";

		public static readonly StringName FindWritableLibraryName = "FindWritableLibraryName";

		public static readonly StringName OpenNameDialog = "OpenNameDialog";

		public static readonly StringName OnNameDialogConfirmed = "OnNameDialogConfirmed";

		public static readonly StringName ConfirmDeleteAnimation = "ConfirmDeleteAnimation";

		public static readonly StringName Fail = "Fail";

		public static readonly StringName SetStatus = "SetStatus";

		public static readonly StringName AddPropertyChoices = "AddPropertyChoices";

		public static readonly StringName IsNodeInsideScene = "IsNodeInsideScene";

		public static readonly StringName ResolveAnimationRoot = "ResolveAnimationRoot";

		public static readonly StringName DuplicateAnimation = "DuplicateAnimation";

		public static readonly StringName ReplaceAnimationDirect = "ReplaceAnimationDirect";

		public static readonly StringName FindKeyAtTime = "FindKeyAtTime";

		public static readonly StringName IsPreviewUnsafeTrack = "IsPreviewUnsafeTrack";

		public static readonly StringName IsSupportedEditableVariant = "IsSupportedEditableVariant";

		public static readonly StringName GetTrackTypeLabel = "GetTrackTypeLabel";

		public static readonly StringName IsRotationTrack = "IsRotationTrack";

		public static readonly StringName IsAngleInterpolation = "IsAngleInterpolation";

		public static readonly StringName GetInterpolationLabel = "GetInterpolationLabel";

		public static readonly StringName GetUpdateModeLabel = "GetUpdateModeLabel";

		public static readonly StringName NormalizeTransition = "NormalizeTransition";

		public static readonly StringName GetVariantTypeLabel = "GetVariantTypeLabel";

		public static readonly StringName FormatVariant = "FormatVariant";

		public static readonly StringName ReadStateString = "ReadStateString";

		public static readonly StringName ReadStateDouble = "ReadStateDouble";

		public static readonly StringName ReadStateBool = "ReadStateBool";

		public static readonly StringName MakeSection = "MakeSection";

		public static readonly StringName MakeStandaloneSection = "MakeStandaloneSection";

		public static readonly StringName Labeled = "Labeled";

		public static readonly StringName MakeOption = "MakeOption";

		public static readonly StringName MakeButton = "MakeButton";

		public static readonly StringName MakeIconBadge = "MakeIconBadge";

		public static readonly StringName MakeHintLabel = "MakeHintLabel";

		public static readonly StringName MakeSpin = "MakeSpin";

		public static readonly StringName MakePanelStyle = "MakePanelStyle";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName CurrentPlayer = "CurrentPlayer";

		public static readonly StringName CurrentLibrary = "CurrentLibrary";

		public static readonly StringName CurrentAnimation = "CurrentAnimation";

		public static readonly StringName CurrentAnimationName = "CurrentAnimationName";

		public static readonly StringName CurrentLibraryName = "CurrentLibraryName";

		public static readonly StringName SelectedTrack = "SelectedTrack";

		public static readonly StringName SelectedKeyTime = "SelectedKeyTime";

		public static readonly StringName TrackCount = "TrackCount";

		public static readonly StringName SelectedTrackKeyCount = "SelectedTrackKeyCount";

		public static readonly StringName CurrentPlayhead = "CurrentPlayhead";

		public static readonly StringName PreviewUnsafeTrackCount = "PreviewUnsafeTrackCount";

		public static readonly StringName PreviewBaselineCount = "PreviewBaselineCount";

		public static readonly StringName IsPreviewPlaying = "IsPreviewPlaying";

		public static readonly StringName PreviewPosition = "PreviewPosition";

		public static readonly StringName IsReadOnly = "IsReadOnly";

		public static readonly StringName EaseCurveCanvas = "EaseCurveCanvas";

		public static readonly StringName SelectedTrackInterpolation = "SelectedTrackInterpolation";

		public static readonly StringName SelectedValueUpdateMode = "SelectedValueUpdateMode";

		public static readonly StringName SelectedTrackInterpolationLoopWrap = "SelectedTrackInterpolationLoopWrap";

		public static readonly StringName SelectedKeyTransition = "SelectedKeyTransition";

		public static readonly StringName _editor = "_editor";

		public static readonly StringName _sceneRoot = "_sceneRoot";

		public static readonly StringName _player = "_player";

		public static readonly StringName _library = "_library";

		public static readonly StringName _animation = "_animation";

		public static readonly StringName _timeline = "_timeline";

		public static readonly StringName _preview = "_preview";

		public static readonly StringName _playerOption = "_playerOption";

		public static readonly StringName _libraryOption = "_libraryOption";

		public static readonly StringName _animationOption = "_animationOption";

		public static readonly StringName _treeLinkLabel = "_treeLinkLabel";

		public static readonly StringName _readonlyLabel = "_readonlyLabel";

		public static readonly StringName _previewSafetyLabel = "_previewSafetyLabel";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _copyLocalButton = "_copyLocalButton";

		public static readonly StringName _playPauseButton = "_playPauseButton";

		public static readonly StringName _loopButton = "_loopButton";

		public static readonly StringName _speedSpin = "_speedSpin";

		public static readonly StringName _lengthSpin = "_lengthSpin";

		public static readonly StringName _stepSpin = "_stepSpin";

		public static readonly StringName _zoomSpin = "_zoomSpin";

		public static readonly StringName _timeLabel = "_timeLabel";

		public static readonly StringName _propertyPalette = "_propertyPalette";

		public static readonly StringName _trackList = "_trackList";

		public static readonly StringName _selectedTrackLabel = "_selectedTrackLabel";

		public static readonly StringName _trackEnabledCheck = "_trackEnabledCheck";

		public static readonly StringName _deleteTrackButton = "_deleteTrackButton";

		public static readonly StringName _moveTrackUpButton = "_moveTrackUpButton";

		public static readonly StringName _moveTrackDownButton = "_moveTrackDownButton";

		public static readonly StringName _insertKeyButton = "_insertKeyButton";

		public static readonly StringName _deleteKeyButton = "_deleteKeyButton";

		public static readonly StringName _duplicateKeyButton = "_duplicateKeyButton";

		public static readonly StringName _moveKeyEarlierButton = "_moveKeyEarlierButton";

		public static readonly StringName _moveKeyLaterButton = "_moveKeyLaterButton";

		public static readonly StringName _selectedKeyLabel = "_selectedKeyLabel";

		public static readonly StringName _valueEditorHost = "_valueEditorHost";

		public static readonly StringName _trackInterpolationOption = "_trackInterpolationOption";

		public static readonly StringName _valueUpdateModeOption = "_valueUpdateModeOption";

		public static readonly StringName _interpolationLoopWrapCheck = "_interpolationLoopWrapCheck";

		public static readonly StringName _trackInterpolationVisualHost = "_trackInterpolationVisualHost";

		public static readonly StringName _valueUpdateModeVisualHost = "_valueUpdateModeVisualHost";

		public static readonly StringName _easePresetShelf = "_easePresetShelf";

		public static readonly StringName _easeCurveCanvas = "_easeCurveCanvas";

		public static readonly StringName _keyTransitionSpin = "_keyTransitionSpin";

		public static readonly StringName _motionCurveStatusLabel = "_motionCurveStatusLabel";

		public static readonly StringName _workbenchScroll = "_workbenchScroll";

		public static readonly StringName _workbenchRoot = "_workbenchRoot";

		public static readonly StringName _deleteAnimationDialog = "_deleteAnimationDialog";

		public static readonly StringName _nameDialog = "_nameDialog";

		public static readonly StringName _nameEdit = "_nameEdit";

		public static readonly StringName _updating = "_updating";

		public static readonly StringName _suspendedForSave = "_suspendedForSave";

		public static readonly StringName _resumePlayingAfterSave = "_resumePlayingAfterSave";

		public static readonly StringName _resumePositionAfterSave = "_resumePositionAfterSave";

		public static readonly StringName _transitionSpinEditing = "_transitionSpinEditing";

		public static readonly StringName _transitionSpinPending = "_transitionSpinPending";

		public static readonly StringName _transitionSpinStart = "_transitionSpinStart";

		public static readonly StringName _selectedTrack = "_selectedTrack";

		public static readonly StringName _selectedKeyTime = "_selectedKeyTime";

		public static readonly StringName _pendingNameOperation = "_pendingNameOperation";

		public static readonly StringName _pendingBindState = "_pendingBindState";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private XW2DSceneEditor _editor;

	private Node _sceneRoot;

	private AnimationPlayer _player;

	private AnimationLibrary _library;

	private Animation _animation;

	private XW2DAnimationTimelineCanvas _timeline;

	private XW2DAnimationSafePreview _preview;

	private OptionButton _playerOption;

	private OptionButton _libraryOption;

	private OptionButton _animationOption;

	private Label _treeLinkLabel;

	private Label _readonlyLabel;

	private Label _previewSafetyLabel;

	private Label _statusLabel;

	private Button _copyLocalButton;

	private Button _playPauseButton;

	private CheckButton _loopButton;

	private SpinBox _speedSpin;

	private SpinBox _lengthSpin;

	private SpinBox _stepSpin;

	private SpinBox _zoomSpin;

	private Label _timeLabel;

	private ItemList _propertyPalette;

	private ItemList _trackList;

	private Label _selectedTrackLabel;

	private CheckButton _trackEnabledCheck;

	private Button _deleteTrackButton;

	private Button _moveTrackUpButton;

	private Button _moveTrackDownButton;

	private Button _insertKeyButton;

	private Button _deleteKeyButton;

	private Button _duplicateKeyButton;

	private Button _moveKeyEarlierButton;

	private Button _moveKeyLaterButton;

	private Label _selectedKeyLabel;

	private VBoxContainer _valueEditorHost;

	private OptionButton _trackInterpolationOption;

	private OptionButton _valueUpdateModeOption;

	private CheckButton _interpolationLoopWrapCheck;

	private XWVisualSegmentedOption _trackInterpolationSegments;

	private XWVisualSegmentedOption _valueUpdateModeSegments;

	private HFlowContainer _trackInterpolationVisualHost;

	private HFlowContainer _valueUpdateModeVisualHost;

	private HFlowContainer _easePresetShelf;

	private XW2DAnimationEaseCurveCanvas _easeCurveCanvas;

	private SpinBox _keyTransitionSpin;

	private Label _motionCurveStatusLabel;

	private ScrollContainer _workbenchScroll;

	private VBoxContainer _workbenchRoot;

	private ConfirmationDialog _deleteAnimationDialog;

	private AcceptDialog _nameDialog;

	private LineEdit _nameEdit;

	private readonly List<AnimationPlayer> _players = new List<AnimationPlayer>();

	private readonly List<StringName> _libraries = new List<StringName>();

	private readonly List<StringName> _animations = new List<StringName>();

	private readonly List<PropertyChoice> _properties = new List<PropertyChoice>();

	private bool _updating;

	private bool _suspendedForSave;

	private bool _resumePlayingAfterSave;

	private double _resumePositionAfterSave;

	private bool _transitionSpinEditing;

	private bool _transitionSpinPending;

	private float _transitionSpinStart = 1f;

	private int _selectedTrack = -1;

	private double _selectedKeyTime = -1.0;

	private string _pendingNameOperation = "";

	private Dictionary _pendingBindState;

	public AnimationPlayer CurrentPlayer => _player;

	public AnimationLibrary CurrentLibrary => _library;

	public Animation CurrentAnimation => _animation;

	public string CurrentAnimationName => GetSelectedAnimationName();

	public string CurrentLibraryName => GetSelectedLibraryName();

	public int SelectedTrack => _selectedTrack;

	public double SelectedKeyTime => _selectedKeyTime;

	public int TrackCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_animation))
			{
				return 0;
			}
			return _animation.GetTrackCount();
		}
	}

	public int SelectedTrackKeyCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_animation) || _selectedTrack < 0 || _selectedTrack >= _animation.GetTrackCount())
			{
				return 0;
			}
			return _animation.TrackGetKeyCount(_selectedTrack);
		}
	}

	public double CurrentPlayhead => _timeline?.Playhead ?? 0.0;

	public int PreviewUnsafeTrackCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_preview))
			{
				return 0;
			}
			return _preview.UnsafeTrackCount;
		}
	}

	public int PreviewBaselineCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_preview))
			{
				return 0;
			}
			return _preview.BaselineCount;
		}
	}

	public bool IsPreviewPlaying
	{
		get
		{
			if (GodotObject.IsInstanceValid(_preview))
			{
				return _preview.IsPlaying;
			}
			return false;
		}
	}

	public double PreviewPosition
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_preview))
			{
				return 0.0;
			}
			return _preview.CurrentPosition;
		}
	}

	public bool IsReadOnly => IsCurrentAnimationReadOnly();

	public XW2DAnimationEaseCurveCanvas EaseCurveCanvas => _easeCurveCanvas;

	public Animation.InterpolationType SelectedTrackInterpolation
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_animation) || _selectedTrack < 0 || _selectedTrack >= _animation.GetTrackCount())
			{
				return Animation.InterpolationType.Linear;
			}
			return _animation.TrackGetInterpolationType(_selectedTrack);
		}
	}

	public Animation.UpdateMode SelectedValueUpdateMode
	{
		get
		{
			if (!IsEditableValueTrack(_selectedTrack))
			{
				return Animation.UpdateMode.Continuous;
			}
			return _animation.ValueTrackGetUpdateMode(_selectedTrack);
		}
	}

	public bool SelectedTrackInterpolationLoopWrap
	{
		get
		{
			if (GodotObject.IsInstanceValid(_animation) && _selectedTrack >= 0 && _selectedTrack < _animation.GetTrackCount())
			{
				return _animation.TrackGetInterpolationLoopWrap(_selectedTrack);
			}
			return false;
		}
	}

	public float SelectedKeyTransition
	{
		get
		{
			if (!TryGetSelectedKey(out var track, out var key))
			{
				return 1f;
			}
			return _animation.TrackGetKeyTransition(track, key);
		}
	}

	public override void _Ready()
	{
		BuildInterface();
		VisibilityChanged += UpdateProcessing;
		SetProcess(enable: false);
		CallDeferred("RefreshWorkbenchLayout");
	}

	public override void _ExitTree()
	{
		_trackInterpolationSegments?.Dispose();
		_valueUpdateModeSegments?.Dispose();
		if (GodotObject.IsInstanceValid(_preview))
		{
			_preview.Clear();
		}
	}

	public override void _Process(double delta)
	{
		if (!Visible || !IsVisibleInTree() || !GodotObject.IsInstanceValid(_preview))
		{
			SetProcess(enable: false);
		}
		else if (!_preview.IsPlaying)
		{
			SetProcess(enable: false);
			UpdatePlaybackButton();
		}
		else
		{
			double currentPosition = _preview.CurrentPosition;
			_timeline?.SetPlayhead(currentPosition);
			UpdateTimeLabel(currentPosition);
		}
	}

	public void BindEditor(XW2DSceneEditor editor)
	{
		_editor = editor;
	}

	public void BindScene(Node sceneRoot, Dictionary state)
	{
		if (_sceneRoot == sceneRoot && sceneRoot != null)
		{
			_pendingBindState = state?.Duplicate(deep: true) ?? new Dictionary();
			RefreshSources();
			return;
		}
		StopPreview();
		_sceneRoot = sceneRoot;
		_pendingBindState = state?.Duplicate(deep: true) ?? new Dictionary();
		RefreshSources();
		RefreshSelection();
	}

	public Dictionary CaptureState()
	{
		return new Dictionary
		{
			["player_path"] = (IsNodeInsideScene(_player) ? _sceneRoot.GetPathTo(_player).ToString() : ""),
			["library"] = GetSelectedLibraryName(),
			["animation"] = GetSelectedAnimationName(),
			["playhead"] = (GodotObject.IsInstanceValid(_preview) ? _preview.CurrentPosition : (_timeline?.Playhead ?? 0.0)),
			["speed"] = (GodotObject.IsInstanceValid(_speedSpin) ? _speedSpin.Value : 1.0),
			["loop"] = GodotObject.IsInstanceValid(_loopButton) && _loopButton.ButtonPressed,
			["zoom"] = (GodotObject.IsInstanceValid(_zoomSpin) ? _zoomSpin.Value : 120.0)
		};
	}

	public void RefreshSelection()
	{
		_properties.Clear();
		if (!GodotObject.IsInstanceValid(_propertyPalette))
		{
			return;
		}
		_propertyPalette.Clear();
		Node node = XWEditorInterface.Instance?.GetSceneTreeDock()?.GetSelectedNode();
		if (!IsNodeInsideScene(node))
		{
			node = _sceneRoot;
		}
		if (!GodotObject.IsInstanceValid(node))
		{
			_propertyPalette.AddItem("先在画布或场景树选择节点");
			_propertyPalette.SetItemDisabled(0, disabled: true);
			return;
		}
		AddPropertyChoices(node);
		if (_properties.Count == 0)
		{
			_propertyPalette.AddItem($"{node.Name} 没有可直接制作动画的常用属性");
			_propertyPalette.SetItemDisabled(0, disabled: true);
			return;
		}
		foreach (PropertyChoice property in _properties)
		{
			_propertyPalette.AddItem(property.Label);
		}
		_propertyPalette.Select(0);
	}

	public bool SuspendForSave()
	{
		if (_suspendedForSave)
		{
			return GodotObject.IsInstanceValid(_animation);
		}
		CommitPendingTransitionInput();
		_suspendedForSave = true;
		_resumePlayingAfterSave = GodotObject.IsInstanceValid(_preview) && _preview.IsPlaying;
		_resumePositionAfterSave = (GodotObject.IsInstanceValid(_preview) ? _preview.CurrentPosition : (_timeline?.Playhead ?? 0.0));
		_preview?.Stop();
		SetProcess(enable: false);
		SetStatus("已暂停安全预览，正在保存场景。");
		return GodotObject.IsInstanceValid(_animation);
	}

	public void ResumeAfterSave()
	{
		if (!_suspendedForSave)
		{
			return;
		}
		_suspendedForSave = false;
		if (GodotObject.IsInstanceValid(_animation))
		{
			BindSafePreview();
			double num = ClampTime(_resumePositionAfterSave);
			_preview.Seek(num);
			_timeline.SetPlayhead(num);
			if (_resumePlayingAfterSave)
			{
				_preview.Play();
				SetProcess(IsVisibleInTree());
			}
		}
		_resumePlayingAfterSave = false;
		_resumePositionAfterSave = 0.0;
		UpdatePlaybackButton();
		SetStatus("保存完成；动画工作台已恢复。");
	}

	public void RefreshFromSceneHistory()
	{
		string selectedAnimationName = GetSelectedAnimationName();
		double time = _timeline?.Playhead ?? 0.0;
		PauseForMutation();
		RefreshLibraries();
		if (!string.IsNullOrWhiteSpace(selectedAnimationName))
		{
			RefreshAnimations(selectedAnimationName);
		}
		time = ClampTime(time);
		_timeline?.SetPlayhead(time);
		_preview?.Seek(time);
		RefreshSelection();
		_editor?.QueueViewportRedraw();
	}

	public void ClearSurface()
	{
		StopPreview();
		_preview?.Clear();
		_sceneRoot = null;
		_player = null;
		_library = null;
		_animation = null;
		_players.Clear();
		_libraries.Clear();
		_animations.Clear();
		_properties.Clear();
		_playerOption?.Clear();
		_libraryOption?.Clear();
		_animationOption?.Clear();
		_trackList?.Clear();
		_propertyPalette?.Clear();
		_timeline?.BindAnimation(null);
		RefreshValueEditor();
		UpdateEditingState();
		SetStatus("打开 2D 场景后可编辑 AnimationPlayer 动画。");
	}

	public bool CreateAnimation(string requestedName = "")
	{
		if (!GodotObject.IsInstanceValid(_player))
		{
			return Fail("当前场景没有可用的动画播放器。");
		}
		if (!GodotObject.IsInstanceValid(_library))
		{
			return Fail("请先选择一个动画库。");
		}
		if (IsLibraryExternal(_library))
		{
			return Fail("外部动画库只读；请先复制片段到当前场景。");
		}
		string text = BuildUniqueAnimationName(string.IsNullOrWhiteSpace(requestedName) ? "新动画" : requestedName.Trim());
		Animation snapshot = new Animation
		{
			Length = 1.0,
			Step = 1f / 30f,
			ResourceLocalToScene = true
		};
		return CommitAnimationSlot("新建动画片段", _library, text, snapshot, add: true, text);
	}

	public bool SelectAnimation(string playerPath, string libraryName, string animationName)
	{
		int num = -1;
		for (int i = 0; i < _players.Count; i++)
		{
			if (string.Equals(IsNodeInsideScene(_players[i]) ? _sceneRoot.GetPathTo(_players[i]).ToString() : ((string?)_players[i].Name), playerPath, StringComparison.Ordinal) || string.Equals(_players[i].Name, playerPath, StringComparison.Ordinal))
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return Fail("找不到动画播放器：" + playerPath);
		}
		_updating = true;
		_playerOption.Select(num);
		_player = _players[num];
		_pendingBindState = new Dictionary
		{
			["library"] = libraryName ?? "",
			["animation"] = animationName ?? ""
		};
		RefreshLibraries();
		_updating = false;
		ApplyPendingState();
		if (GodotObject.IsInstanceValid(_animation))
		{
			return string.Equals(GetSelectedAnimationName(), animationName, StringComparison.Ordinal);
		}
		return false;
	}

	public bool SelectKey(int trackIndex, double keyTime)
	{
		if (!GodotObject.IsInstanceValid(_animation) || trackIndex < 0 || trackIndex >= _animation.GetTrackCount() || FindKeyAtTime(_animation, trackIndex, keyTime) < 0)
		{
			return Fail("找不到指定关键帧。");
		}
		_selectedTrack = trackIndex;
		_selectedKeyTime = keyTime;
		_timeline.SelectKey(trackIndex, keyTime);
		if (_trackList.ItemCount > trackIndex)
		{
			_trackList.Select(trackIndex);
		}
		RefreshTrackSelection();
		return true;
	}

	public void SeekPreview(double position)
	{
		OnTimelinePlayheadChanged(position);
	}

	public void PlayPreview()
	{
		if (GodotObject.IsInstanceValid(_animation) && !_suspendedForSave)
		{
			if (!_preview.IsPlaying)
			{
				_preview.Play();
				SetProcess(IsVisibleInTree());
			}
			UpdatePlaybackButton();
		}
	}

	public void PausePreview()
	{
		_preview?.Pause();
		SetProcess(enable: false);
		UpdatePlaybackButton();
	}

	public bool AddValueTrack()
	{
		int[] array = _propertyPalette?.GetSelectedItems();
		int num = ((array != null && array.Length > 0) ? array[0] : (-1));
		if (num < 0 || num >= _properties.Count)
		{
			return Fail("请先从属性图库选择一个节点属性。");
		}
		PropertyChoice propertyChoice = _properties[num];
		return AddValueTrack(propertyChoice.Node, propertyChoice.Property);
	}

	public bool AddValueTrack(Node node, string propertyName)
	{
		if (!IsNodeInsideScene(node) || string.IsNullOrWhiteSpace(propertyName))
		{
			return Fail("属性目标不在当前场景中。");
		}
		Node node2 = ResolveAnimationRoot(_player);
		if (!GodotObject.IsInstanceValid(node2))
		{
			return Fail("动画播放器的根节点路径无效。");
		}
		string text = ((node2 == node) ? "." : node2.GetPathTo(node).ToString());
		return AddValueTrack(text + ":" + propertyName);
	}

	public bool AddValueTrack(string propertyPath)
	{
		if (!EnsureEditable("新增属性轨道"))
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(propertyPath) || !propertyPath.Contains(':'))
		{
			return Fail("轨道路径应为“节点路径:属性”。");
		}
		return CommitAnimationEdit("新增属性轨道", (Animation animation) =>
		{
			int num = animation.AddTrack(Animation.TrackType.Value);
			animation.TrackSetPath(num, new NodePath(propertyPath));
			animation.ValueTrackSetUpdateMode(num, Animation.UpdateMode.Continuous);
			_selectedTrack = num;
			_selectedKeyTime = -1.0;
			return true;
		});
	}

	public bool SetSelectedTrackInterpolation(Animation.InterpolationType interpolation)
	{
		if (!EnsureEditable("切换轨道插值") || !GodotObject.IsInstanceValid(_animation) || _selectedTrack < 0 || _selectedTrack >= _animation.GetTrackCount())
		{
			return Fail("请先选择可编辑的动画轨道。");
		}
		int track = _selectedTrack;
		if (IsAngleInterpolation(interpolation) && !IsRotationTrack(track))
		{
			return Fail("角度插值只适用于 rotation 旋转属性轨道。");
		}
		if (_animation.TrackGetInterpolationType(track) == interpolation)
		{
			return true;
		}
		return CommitAnimationEdit("切换轨道插值为" + GetInterpolationLabel(interpolation), (Animation animation) =>
		{
			animation.TrackSetInterpolationType(track, interpolation);
			_selectedTrack = track;
			return true;
		});
	}

	public bool SetSelectedValueUpdateMode(Animation.UpdateMode updateMode)
	{
		if (!EnsureEditable("切换属性轨更新方式") || !IsEditableValueTrack(_selectedTrack))
		{
			return Fail("更新方式只适用于可编辑的 Value 属性轨道。");
		}
		int track = _selectedTrack;
		if (_animation.ValueTrackGetUpdateMode(track) == updateMode)
		{
			return true;
		}
		return CommitAnimationEdit("切换属性轨为" + GetUpdateModeLabel(updateMode) + "更新", (Animation animation) =>
		{
			animation.ValueTrackSetUpdateMode(track, updateMode);
			_selectedTrack = track;
			return true;
		});
	}

	public bool SetSelectedTrackInterpolationLoopWrap(bool enabled)
	{
		if (!EnsureEditable("切换循环插值衔接") || !GodotObject.IsInstanceValid(_animation) || _selectedTrack < 0 || _selectedTrack >= _animation.GetTrackCount())
		{
			return Fail("请先选择可编辑的动画轨道。");
		}
		int track = _selectedTrack;
		if (_animation.TrackGetInterpolationLoopWrap(track) == enabled)
		{
			return true;
		}
		return CommitAnimationEdit(enabled ? "开启循环插值衔接" : "关闭循环插值衔接", (Animation animation) =>
		{
			animation.TrackSetInterpolationLoopWrap(track, enabled);
			_selectedTrack = track;
			return true;
		});
	}

	public bool SetSelectedKeyTransition(float transition)
	{
		if (!TryGetSelectedKey(out var track, out var key))
		{
			return Fail("请先选择要编辑缓动的关键帧。");
		}
		float normalized = NormalizeTransition(transition);
		if (Mathf.IsEqualApprox(_animation.TrackGetKeyTransition(track, key), normalized))
		{
			return true;
		}
		double keyTime = _animation.TrackGetKeyTime(track, key);
		return CommitAnimationEdit("调整关键帧运动曲线", (Animation animation) =>
		{
			int num = FindKeyAtTime(animation, track, keyTime);
			if (num < 0)
			{
				return false;
			}
			animation.TrackSetKeyTransition(track, num, normalized);
			_selectedTrack = track;
			_selectedKeyTime = keyTime;
			return true;
		});
	}

	public bool InsertCurrentKey()
	{
		if (!TryReadCurrentTrackValue(out var value))
		{
			return false;
		}
		return InsertCurrentKey(value);
	}

	public bool InsertCurrentKey(Variant value)
	{
		if (!EnsureEditable("插入关键帧") || !IsEditableValueTrack(_selectedTrack))
		{
			return Fail("只能给属性值轨道插入关键帧。");
		}
		double time = ClampTime(_timeline?.Playhead ?? 0.0);
		int track = _selectedTrack;
		return CommitAnimationEdit("在播放头写入当前值", (Animation animation) =>
		{
			animation.TrackInsertKey(track, time, value);
			_selectedTrack = track;
			_selectedKeyTime = time;
			return true;
		});
	}

	public bool MoveSelectedKey(double newTime)
	{
		if (!TryGetSelectedKey(out var track, out var key))
		{
			return false;
		}
		double sourceTime = _animation.TrackGetKeyTime(track, key);
		double targetTime = ClampTime(newTime);
		if (Mathf.IsEqualApprox(sourceTime, targetTime))
		{
			return true;
		}
		return CommitAnimationEdit("移动关键帧", (Animation animation) =>
		{
			int num = FindKeyAtTime(animation, track, sourceTime);
			if (num < 0)
			{
				return false;
			}
			Variant key2 = animation.TrackGetKeyValue(track, num);
			float transition = animation.TrackGetKeyTransition(track, num);
			animation.TrackRemoveKey(track, num);
			int keyIdx = animation.TrackInsertKey(track, targetTime, key2, transition);
			_selectedTrack = track;
			_selectedKeyTime = animation.TrackGetKeyTime(track, keyIdx);
			return true;
		});
	}

	public bool DeleteSelectedKey()
	{
		if (!TryGetSelectedKey(out var track, out var key))
		{
			return false;
		}
		double time = _animation.TrackGetKeyTime(track, key);
		return CommitAnimationEdit("删除关键帧", (Animation animation) =>
		{
			int num = FindKeyAtTime(animation, track, time);
			if (num < 0)
			{
				return false;
			}
			animation.TrackRemoveKey(track, num);
			_selectedTrack = track;
			_selectedKeyTime = -1.0;
			return true;
		});
	}

	private void BuildInterface()
	{
		CustomMinimumSize = new Vector2(420f, 260f);
		AddThemeStyleboxOverride("panel", MakePanelStyle(new Color("#101923"), new Color("#36506a"), 10, 1));
		_workbenchScroll = new ScrollContainer
		{
			Name = "AnimationWorkbenchScroll",
			CustomMinimumSize = new Vector2(0f, 220f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill,
			HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
			VerticalScrollMode = ScrollContainer.ScrollMode.Auto
		};
		AddChild(_workbenchScroll, forceReadableName: false, InternalMode.Disabled);
		_workbenchRoot = new VBoxContainer
		{
			Name = "AnimationWorkbench",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_workbenchRoot.AddThemeConstantOverride("separation", 8);
		_workbenchScroll.AddChild(_workbenchRoot, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer workbenchRoot = _workbenchRoot;
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddChild(MakeIconBadge("◆", new Color("#67d9ff")), forceReadableName: false, InternalMode.Disabled);
		Label label = new Label
		{
			Text = "2D 动画工作台",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		label.AddThemeFontSizeOverride("font_size", 18);
		label.AddThemeColorOverride("font_color", new Color("#f5dc91"));
		hBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		Button button = MakeButton("刷新节点", "重新扫描 AnimationPlayer 与所选节点");
		button.Pressed += () =>
		{
			RefreshSources();
			RefreshSelection();
		};
		hBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		workbenchRoot.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer = MakeSection(workbenchRoot, "播放器 · 动画库 · 片段", "场景内资源直接编辑，外部与共享资源保持只读");
		HBoxContainer hBoxContainer2 = new HBoxContainer();
		hBoxContainer2.AddThemeConstantOverride("separation", 6);
		_playerOption = MakeOption("选择 AnimationPlayer");
		_libraryOption = MakeOption("选择动画库");
		_animationOption = MakeOption("选择动画片段");
		_playerOption.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		_libraryOption.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		_animationOption.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer2.AddChild(Labeled("播放器", _playerOption), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer2.AddChild(Labeled("动画库", _libraryOption), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer2.AddChild(Labeled("片段", _animationOption), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(hBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		_playerOption.ItemSelected += OnPlayerSelected;
		_libraryOption.ItemSelected += OnLibrarySelected;
		_animationOption.ItemSelected += OnAnimationSelected;
		HBoxContainer hBoxContainer3 = new HBoxContainer();
		Button button2 = MakeButton("＋ 新建", "在当前动画库新建片段");
		Button button3 = MakeButton("▣ 复制", "复制当前片段为本地可编辑资源");
		Button button4 = MakeButton("✎ 改名", "重命名当前片段");
		Button button5 = MakeButton("× 删除", "删除当前片段");
		_copyLocalButton = MakeButton("⇩ 复制到当前场景", "外部或共享动画先复制为场景本地片段");
		button2.Pressed += () =>
		{
			OpenNameDialog("create", "新建动画片段", "新动画");
		};
		button3.Pressed += DuplicateCurrentAnimation;
		button4.Pressed += () =>
		{
			OpenNameDialog("rename", "重命名动画片段", GetSelectedAnimationName());
		};
		button5.Pressed += ConfirmDeleteAnimation;
		_copyLocalButton.Pressed += CopyCurrentAnimationToScene;
		Button[] array = new Button[5] { button2, button3, button4, button5, _copyLocalButton };
		foreach (Button node in array)
		{
			hBoxContainer3.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		}
		vBoxContainer.AddChild(hBoxContainer3, forceReadableName: false, InternalMode.Disabled);
		_treeLinkLabel = MakeHintLabel("");
		_readonlyLabel = MakeHintLabel("");
		vBoxContainer.AddChild(_treeLinkLabel, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(_readonlyLabel, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer2 = MakeSection(workbenchRoot, "安全片段预览", "只复制安全属性轨道；方法、音频与嵌套动画轨不会执行");
		HBoxContainer hBoxContainer4 = new HBoxContainer();
		Button button6 = MakeButton("◀◆", "跳到前一个关键帧");
		_playPauseButton = MakeButton("▶ 播放", "在隔离播放器中播放安全轨道");
		Button button7 = MakeButton("■ 停止", "停止并回到片段起点");
		Button button8 = MakeButton("◆▶", "跳到后一个关键帧");
		_loopButton = new CheckButton
		{
			Text = "循环",
			ButtonPressed = false
		};
		_speedSpin = MakeSpin(0.1, 4.0, 0.1, 1.0, "0.0×");
		_lengthSpin = MakeSpin(0.01, 3600.0, 0.01, 1.0, "0.00 秒");
		_stepSpin = MakeSpin(0.001, 1.0, 0.001, 1.0 / 30.0, "0.000 秒");
		_zoomSpin = MakeSpin(24.0, 480.0, 8.0, 120.0, "0 像素/秒");
		_timeLabel = new Label
		{
			Text = "0.00 / 0.00 秒",
			CustomMinimumSize = new Vector2(130f, 0f)
		};
		hBoxContainer4.AddChild(button6, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(_playPauseButton, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(button7, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(button8, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(_loopButton, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(Labeled("速度", _speedSpin), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(Labeled("长度", _lengthSpin), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(Labeled("步长", _stepSpin), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(Labeled("缩放", _zoomSpin), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(_timeLabel, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(hBoxContainer4, forceReadableName: false, InternalMode.Disabled);
		_timeline = new XW2DAnimationTimelineCanvas
		{
			Name = "TimelineCanvas",
			CustomMinimumSize = new Vector2(0f, 190f),
			SizeFlagsVertical = SizeFlags.Fill,
			MouseFilter = MouseFilterEnum.Stop
		};
		vBoxContainer2.AddChild(_timeline, forceReadableName: false, InternalMode.Disabled);
		_preview = new XW2DAnimationSafePreview
		{
			Name = "SafePreview"
		};
		AddChild(_preview, forceReadableName: false, InternalMode.Disabled);
		_previewSafetyLabel = MakeHintLabel("预览尚未绑定。源 AnimationPlayer 不会被播放。");
		vBoxContainer2.AddChild(_previewSafetyLabel, forceReadableName: false, InternalMode.Disabled);
		_playPauseButton.Pressed += TogglePlayback;
		button7.Pressed += StopPreview;
		button6.Pressed += () =>
		{
			JumpToKey(-1);
		};
		button8.Pressed += () =>
		{
			JumpToKey(1);
		};
		_loopButton.Toggled += OnLoopToggled;
		_speedSpin.ValueChanged += OnSpeedChanged;
		_lengthSpin.ValueChanged += OnLengthChanged;
		_stepSpin.ValueChanged += OnStepChanged;
		_zoomSpin.ValueChanged += (double value) =>
		{
			_timeline.SetTimeScale(value);
		};
		_timeline.PlayheadChanged += OnTimelinePlayheadChanged;
		_timeline.KeySelected += OnTimelineKeySelected;
		_timeline.KeyMoveRequested += OnTimelineKeyMoveRequested;
		HSplitContainer hSplitContainer = new HSplitContainer();
		hSplitContainer.CustomMinimumSize = new Vector2(0f, 500f);
		hSplitContainer.SizeFlagsVertical = SizeFlags.Fill;
		hSplitContainer.SplitOffsets = new int[1];
		HSplitContainer hSplitContainer2 = hSplitContainer;
		workbenchRoot.AddChild(hSplitContainer2, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer3 = MakeStandaloneSection("所选节点属性图库", "选择可视属性后，一键生成属性轨道");
		vBoxContainer3.SizeFlagsHorizontal = SizeFlags.Fill;
		hSplitContainer2.AddChild(vBoxContainer3, forceReadableName: false, InternalMode.Disabled);
		_propertyPalette = new ItemList
		{
			Name = "PropertyPalette",
			CustomMinimumSize = new Vector2(280f, 160f),
			SizeFlagsVertical = SizeFlags.ExpandFill,
			AllowReselect = true
		};
		vBoxContainer3.AddChild(_propertyPalette, forceReadableName: false, InternalMode.Disabled);
		Button button9 = MakeButton("＋ 当前属性生成轨道", "不打开检查器，直接将所选属性加入时间轴");
		button9.Pressed += () =>
		{
			AddValueTrack();
		};
		vBoxContainer3.AddChild(button9, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer4 = MakeStandaloneSection("轨道与关键帧", "轨道按执行顺序排列；菱形可拖动换时刻");
		vBoxContainer4.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hSplitContainer2.AddChild(vBoxContainer4, forceReadableName: false, InternalMode.Disabled);
		hSplitContainer = new HSplitContainer();
		hSplitContainer.CustomMinimumSize = new Vector2(0f, 180f);
		hSplitContainer.SizeFlagsVertical = SizeFlags.Fill;
		hSplitContainer.SplitOffsets = new int[1];
		HSplitContainer hSplitContainer3 = hSplitContainer;
		vBoxContainer4.AddChild(hSplitContainer3, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer5 = new VBoxContainer
		{
			CustomMinimumSize = new Vector2(300f, 0f),
			SizeFlagsHorizontal = SizeFlags.Fill
		};
		hSplitContainer3.AddChild(vBoxContainer5, forceReadableName: false, InternalMode.Disabled);
		_trackList = new ItemList
		{
			Name = "TrackList",
			CustomMinimumSize = new Vector2(300f, 150f),
			SizeFlagsVertical = SizeFlags.ExpandFill,
			AllowReselect = true
		};
		_trackList.ItemSelected += OnTrackListSelected;
		vBoxContainer5.AddChild(_trackList, forceReadableName: false, InternalMode.Disabled);
		_selectedTrackLabel = MakeHintLabel("未选择轨道");
		vBoxContainer5.AddChild(_selectedTrackLabel, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer5 = new HBoxContainer();
		_trackEnabledCheck = new CheckButton
		{
			Text = "启用轨道"
		};
		_deleteTrackButton = MakeButton("删除", "删除整条轨道");
		_moveTrackUpButton = MakeButton("↑", "轨道上移");
		_moveTrackDownButton = MakeButton("↓", "轨道下移");
		_trackEnabledCheck.Toggled += ToggleSelectedTrack;
		_deleteTrackButton.Pressed += DeleteSelectedTrack;
		_moveTrackUpButton.Pressed += () =>
		{
			MoveSelectedTrack(-1);
		};
		_moveTrackDownButton.Pressed += () =>
		{
			MoveSelectedTrack(1);
		};
		hBoxContainer5.AddChild(_trackEnabledCheck, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer5.AddChild(_moveTrackUpButton, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer5.AddChild(_moveTrackDownButton, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer5.AddChild(_deleteTrackButton, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer5.AddChild(hBoxContainer5, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer6 = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hSplitContainer3.AddChild(vBoxContainer6, forceReadableName: false, InternalMode.Disabled);
		_selectedKeyLabel = MakeHintLabel("选择时间轴上的菱形关键帧");
		vBoxContainer6.AddChild(_selectedKeyLabel, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer6 = new HBoxContainer();
		_insertKeyButton = MakeButton("◆ 写入当前值", "读取场景节点当前属性并写入播放头");
		_duplicateKeyButton = MakeButton("▣ 复制帧", "复制到下一步长");
		_deleteKeyButton = MakeButton("× 删除帧", "删除选中关键帧");
		_moveKeyEarlierButton = MakeButton("← 前移", "前移一个动画步长");
		_moveKeyLaterButton = MakeButton("后移 →", "后移一个动画步长");
		_insertKeyButton.Pressed += () =>
		{
			InsertCurrentKey();
		};
		_duplicateKeyButton.Pressed += DuplicateSelectedKey;
		_deleteKeyButton.Pressed += () =>
		{
			DeleteSelectedKey();
		};
		_moveKeyEarlierButton.Pressed += () =>
		{
			MoveSelectedKey(((_selectedKeyTime < 0.0) ? 0.0 : _selectedKeyTime) - GetStep());
		};
		_moveKeyLaterButton.Pressed += () =>
		{
			MoveSelectedKey(((_selectedKeyTime < 0.0) ? 0.0 : _selectedKeyTime) + GetStep());
		};
		array = new Button[5] { _insertKeyButton, _duplicateKeyButton, _deleteKeyButton, _moveKeyEarlierButton, _moveKeyLaterButton };
		foreach (Button node2 in array)
		{
			hBoxContainer6.AddChild(node2, forceReadableName: false, InternalMode.Disabled);
		}
		vBoxContainer6.AddChild(hBoxContainer6, forceReadableName: false, InternalMode.Disabled);
		_valueEditorHost = new VBoxContainer
		{
			Name = "DirectKeyValueEditor",
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		vBoxContainer6.AddChild(_valueEditorHost, forceReadableName: false, InternalMode.Disabled);
		workbenchRoot.AddChild(BuildMotionCurveCard(), forceReadableName: false, InternalMode.Disabled);
		_statusLabel = MakeHintLabel("打开包含 AnimationPlayer 的 2D 场景即可开始。");
		_statusLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
		workbenchRoot.AddChild(_statusLabel, forceReadableName: false, InternalMode.Disabled);
		BuildDialogs();
		UpdateEditingState();
	}

	private void RefreshWorkbenchLayout()
	{
		if (GodotObject.IsInstanceValid(_workbenchScroll) && GodotObject.IsInstanceValid(_workbenchRoot))
		{
			_workbenchRoot.UpdateMinimumSize();
			_workbenchRoot.QueueSort();
			_workbenchScroll.UpdateMinimumSize();
			_workbenchScroll.QueueSort();
		}
	}

	private Control BuildMotionCurveCard()
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.Name = "MotionCurveCard";
		panelContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		panelContainer.AddThemeStyleboxOverride("panel", MakePanelStyle(ModEditorTheme.GrassColor.Darkened(0.42f), ModEditorTheme.LeafColor.Darkened(0.18f), 10, 1));
		XWUiMotion.BindPanel(panelContainer);
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.AddThemeConstantOverride("separation", 7);
		panelContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddThemeConstantOverride("separation", 8);
		hBoxContainer.AddChild(new TextureRect
		{
			Texture = LoadClassIcon("CurveEdit"),
			CustomMinimumSize = new Vector2(28f, 28f),
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
		}, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer2 = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		Label label = new Label
		{
			Text = "运动曲线"
		};
		label.AddThemeFontSizeOverride("font_size", 15);
		label.AddThemeColorOverride("font_color", ModEditorTheme.SunGoldColor.Lightened(0.16f));
		vBoxContainer2.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		_motionCurveStatusLabel = MakeHintLabel("选择 Value 轨和关键帧后，可直接配置插值、更新方式与缓动。");
		_motionCurveStatusLabel.Name = "MotionCurveStatus";
		_motionCurveStatusLabel.Modulate = ModEditorTheme.DimTextColor.Lightened(0.16f);
		vBoxContainer2.AddChild(_motionCurveStatusLabel, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(MakeSmallHeading("轨道插值"), forceReadableName: false, InternalMode.Disabled);
		HFlowContainer hFlowContainer = new HFlowContainer
		{
			Name = "TrackInterpolationSegments",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hFlowContainer.AddThemeConstantOverride("h_separation", 5);
		hFlowContainer.AddThemeConstantOverride("v_separation", 5);
		vBoxContainer.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
		_trackInterpolationOption = new OptionButton
		{
			Name = "TrackInterpolationOption",
			TooltipText = "选择当前轨道在关键帧之间的插值方式"
		};
		AddOptionItem(_trackInterpolationOption, "保持", Animation.InterpolationType.Nearest);
		AddOptionItem(_trackInterpolationOption, "线性", Animation.InterpolationType.Linear);
		AddOptionItem(_trackInterpolationOption, "三次平滑", Animation.InterpolationType.Cubic);
		AddOptionItem(_trackInterpolationOption, "角度线性", Animation.InterpolationType.LinearAngle);
		AddOptionItem(_trackInterpolationOption, "角度平滑", Animation.InterpolationType.CubicAngle);
		_trackInterpolationOption.ItemSelected += OnTrackInterpolationSelected;
		hFlowContainer.AddChild(_trackInterpolationOption, forceReadableName: false, InternalMode.Disabled);
		_trackInterpolationVisualHost = new HFlowContainer
		{
			Name = "TrackInterpolationVisualOptions",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hFlowContainer.AddChild(_trackInterpolationVisualHost, forceReadableName: false, InternalMode.Disabled);
		_trackInterpolationSegments = new XWVisualSegmentedOption(_trackInterpolationOption, _trackInterpolationVisualHost, (int index) => LoadClassIcon(index switch
		{
			0 => "InterpRaw", 
			1 => "InterpLinear", 
			2 => "InterpCubic", 
			3 => "InterpLinearAngle", 
			_ => "InterpCubicAngle", 
		}));
		_trackInterpolationSegments.Rebuild();
		StyleMotionSegments(_trackInterpolationVisualHost);
		_interpolationLoopWrapCheck = new CheckButton
		{
			Name = "InterpolationLoopWrapCheck",
			Text = "↻ 循环衔接",
			TooltipText = "开启后，循环动画会让末帧与首帧参与连续插值"
		};
		_interpolationLoopWrapCheck.Toggled += (bool enabled) =>
		{
			if (!_updating)
			{
				SetSelectedTrackInterpolationLoopWrap(enabled);
			}
		};
		XWUiMotion.BindButton(_interpolationLoopWrapCheck, XWUiMotion.MotionRole.Card);
		vBoxContainer.AddChild(_interpolationLoopWrapCheck, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(MakeSmallHeading("Value 轨更新方式"), forceReadableName: false, InternalMode.Disabled);
		HFlowContainer hFlowContainer2 = new HFlowContainer
		{
			Name = "ValueUpdateModeSegments",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hFlowContainer2.AddThemeConstantOverride("h_separation", 5);
		hFlowContainer2.AddThemeConstantOverride("v_separation", 5);
		vBoxContainer.AddChild(hFlowContainer2, forceReadableName: false, InternalMode.Disabled);
		_valueUpdateModeOption = new OptionButton
		{
			Name = "ValueUpdateModeOption",
			TooltipText = "选择属性值连续变化、到帧跳变或从当前对象捕获"
		};
		AddOptionItem(_valueUpdateModeOption, "连续", Animation.UpdateMode.Continuous);
		AddOptionItem(_valueUpdateModeOption, "离散", Animation.UpdateMode.Discrete);
		AddOptionItem(_valueUpdateModeOption, "捕获", Animation.UpdateMode.Capture);
		_valueUpdateModeOption.ItemSelected += OnValueUpdateModeSelected;
		hFlowContainer2.AddChild(_valueUpdateModeOption, forceReadableName: false, InternalMode.Disabled);
		_valueUpdateModeVisualHost = new HFlowContainer
		{
			Name = "ValueUpdateModeVisualOptions",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hFlowContainer2.AddChild(_valueUpdateModeVisualHost, forceReadableName: false, InternalMode.Disabled);
		_valueUpdateModeSegments = new XWVisualSegmentedOption(_valueUpdateModeOption, _valueUpdateModeVisualHost, (int index) => LoadClassIcon(index switch
		{
			0 => "TrackContinuous", 
			1 => "TrackDiscrete", 
			_ => "TrackCapture", 
		}));
		_valueUpdateModeSegments.Rebuild();
		StyleMotionSegments(_valueUpdateModeVisualHost);
		vBoxContainer.AddChild(MakeSmallHeading("关键帧缓动"), forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer3 = new VBoxContainer
		{
			Name = "KeyTransitionCurveBody",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer3.AddThemeConstantOverride("separation", 8);
		vBoxContainer.AddChild(vBoxContainer3, forceReadableName: false, InternalMode.Disabled);
		_easeCurveCanvas = new XW2DAnimationEaseCurveCanvas
		{
			Name = "KeyTransitionCurve",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_easeCurveCanvas.TransitionPreviewChanged += OnEaseTransitionPreviewChanged;
		_easeCurveCanvas.TransitionCommitRequested += OnEaseTransitionCommitRequested;
		vBoxContainer3.AddChild(_easeCurveCanvas, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer4 = new VBoxContainer
		{
			CustomMinimumSize = new Vector2(250f, 0f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer3.AddChild(vBoxContainer4, forceReadableName: false, InternalMode.Disabled);
		_keyTransitionSpin = MakeSpin(-1000000.0, 1000000.0, 0.01, 1.0, "");
		_keyTransitionSpin.Name = "KeyTransitionSpin";
		_keyTransitionSpin.TooltipText = "输入 Godot transition 精确曲率；失去焦点时只提交一次";
		_keyTransitionSpin.ValueChanged += OnTransitionSpinValueChanged;
		LineEdit lineEdit = _keyTransitionSpin.GetLineEdit();
		lineEdit.FocusEntered += BeginTransitionSpinEdit;
		lineEdit.FocusExited += CommitTransitionSpinEdit;
		lineEdit.TextSubmitted += (string _) =>
		{
			CommitTransitionSpinEdit();
		};
		vBoxContainer4.AddChild(Labeled("精确曲率", _keyTransitionSpin), forceReadableName: false, InternalMode.Disabled);
		_easePresetShelf = new HFlowContainer
		{
			Name = "EasePresetShelf",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_easePresetShelf.AddThemeConstantOverride("h_separation", 5);
		_easePresetShelf.AddThemeConstantOverride("v_separation", 5);
		vBoxContainer4.AddChild(_easePresetShelf, forceReadableName: false, InternalMode.Disabled);
		AddEasePreset("EaseLinearPreset", "匀速", "CurveLinear", 1f);
		AddEasePreset("EaseInPreset", "缓入", "CurveIn", 2f);
		AddEasePreset("EaseOutPreset", "缓出", "CurveOut", 0.5f);
		AddEasePreset("EaseInOutPreset", "缓入缓出", "CurveInOut", -2f);
		return panelContainer;
	}

	private void AddEasePreset(string nodeName, string label, string iconName, float transition)
	{
		Button button = new Button
		{
			Name = nodeName,
			Text = label,
			TooltipText = $"{label} · transition {transition:0.###}",
			Icon = LoadClassIcon(iconName),
			ExpandIcon = false,
			ThemeTypeVariation = "GameNavCard",
			CustomMinimumSize = new Vector2(112f, 42f)
		};
		button.AddThemeConstantOverride("icon_max_width", 24);
		button.Pressed += () =>
		{
			SetSelectedKeyTransition(transition);
		};
		_easePresetShelf.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		XWUiMotion.BindButton(button, XWUiMotion.MotionRole.Card);
	}

	private void OnTrackInterpolationSelected(long index)
	{
		if (!_updating && GodotObject.IsInstanceValid(_trackInterpolationOption) && index >= 0 && index < _trackInterpolationOption.ItemCount)
		{
			int itemId = _trackInterpolationOption.GetItemId((int)index);
			SetSelectedTrackInterpolation((Animation.InterpolationType)itemId);
		}
	}

	private void OnValueUpdateModeSelected(long index)
	{
		if (!_updating && GodotObject.IsInstanceValid(_valueUpdateModeOption) && index >= 0 && index < _valueUpdateModeOption.ItemCount)
		{
			int itemId = _valueUpdateModeOption.GetItemId((int)index);
			SetSelectedValueUpdateMode((Animation.UpdateMode)itemId);
		}
	}

	private void OnEaseTransitionPreviewChanged(float transition)
	{
		if (GodotObject.IsInstanceValid(_keyTransitionSpin))
		{
			_updating = true;
			_keyTransitionSpin.Value = transition;
			_updating = false;
		}
		if (GodotObject.IsInstanceValid(_motionCurveStatusLabel))
		{
			_motionCurveStatusLabel.Text = $"正在预览曲率 {transition:0.###}；松开后只写入一次撤销历史。";
		}
	}

	private void OnEaseTransitionCommitRequested(float _, float transition)
	{
		if (!SetSelectedKeyTransition(transition) && GodotObject.IsInstanceValid(_easeCurveCanvas))
		{
			_easeCurveCanvas.SetTransition(SelectedKeyTransition);
		}
	}

	private void BeginTransitionSpinEdit()
	{
		if (!_updating && TryGetSelectedKey(out var _, out var _))
		{
			_transitionSpinEditing = true;
			_transitionSpinPending = false;
			_transitionSpinStart = SelectedKeyTransition;
		}
	}

	private void OnTransitionSpinValueChanged(double value)
	{
		if (!_updating && GodotObject.IsInstanceValid(_easeCurveCanvas))
		{
			float num = NormalizeTransition((float)value);
			_easeCurveCanvas.SetTransition(num);
			_transitionSpinPending = true;
			if (!_transitionSpinEditing && GodotObject.IsInstanceValid(_keyTransitionSpin) && !_keyTransitionSpin.GetLineEdit().HasFocus())
			{
				_transitionSpinPending = false;
				SetSelectedKeyTransition(num);
			}
		}
	}

	private void CommitTransitionSpinEdit()
	{
		if (!_transitionSpinPending)
		{
			_transitionSpinEditing = false;
			return;
		}
		float num = (GodotObject.IsInstanceValid(_keyTransitionSpin) ? NormalizeTransition((float)_keyTransitionSpin.Value) : _transitionSpinStart);
		_transitionSpinEditing = false;
		_transitionSpinPending = false;
		if (!Mathf.IsEqualApprox(_transitionSpinStart, num))
		{
			SetSelectedKeyTransition(num);
		}
	}

	private void CommitPendingTransitionInput()
	{
		if (GodotObject.IsInstanceValid(_keyTransitionSpin))
		{
			LineEdit lineEdit = _keyTransitionSpin.GetLineEdit();
			if (GodotObject.IsInstanceValid(lineEdit) && lineEdit.HasFocus())
			{
				_keyTransitionSpin.Apply();
			}
			CommitTransitionSpinEdit();
		}
	}

	private void RefreshMotionCurveEditor()
	{
		if (!GodotObject.IsInstanceValid(_trackInterpolationOption) || !GodotObject.IsInstanceValid(_valueUpdateModeOption) || !GodotObject.IsInstanceValid(_easeCurveCanvas))
		{
			return;
		}
		bool flag = GodotObject.IsInstanceValid(_animation) && _selectedTrack >= 0 && _selectedTrack < _animation.GetTrackCount();
		bool flag2 = flag && IsEditableValueTrack(_selectedTrack);
		bool flag3 = flag2 && !IsCurrentAnimationReadOnly();
		int track = -1;
		int key = -1;
		bool flag4 = flag2 && TryGetSelectedKey(out track, out key);
		bool flag5 = flag2 && IsRotationTrack(_selectedTrack);
		Animation.InterpolationType interpolationType = (flag ? _animation.TrackGetInterpolationType(_selectedTrack) : Animation.InterpolationType.Linear);
		Animation.UpdateMode updateMode = (flag2 ? _animation.ValueTrackGetUpdateMode(_selectedTrack) : Animation.UpdateMode.Continuous);
		_updating = true;
		_trackInterpolationOption.Disabled = !flag3;
		_valueUpdateModeOption.Disabled = !flag3;
		SelectOptionById(_trackInterpolationOption, (int)interpolationType);
		SelectOptionById(_valueUpdateModeOption, (int)updateMode);
		_trackInterpolationSegments?.RefreshSelection();
		_valueUpdateModeSegments?.RefreshSelection();
		if (GodotObject.IsInstanceValid(_interpolationLoopWrapCheck))
		{
			_interpolationLoopWrapCheck.Disabled = !flag3;
			_interpolationLoopWrapCheck.SetPressedNoSignal(flag && _animation.TrackGetInterpolationLoopWrap(_selectedTrack));
		}
		_updating = false;
		if (GodotObject.IsInstanceValid(_trackInterpolationVisualHost))
		{
			int num = 0;
			foreach (Node child in _trackInterpolationVisualHost.GetChildren())
			{
				if (child is Button button)
				{
					bool flag6 = num >= 3;
					button.Visible = (!flag6 | flag5) || IsAngleInterpolation(interpolationType);
					button.Disabled = !flag3 || (flag6 && !flag5);
				}
				num++;
			}
		}
		if (GodotObject.IsInstanceValid(_valueUpdateModeVisualHost))
		{
			foreach (Node child2 in _valueUpdateModeVisualHost.GetChildren())
			{
				if (child2 is Button button2)
				{
					button2.Disabled = !flag3;
				}
			}
		}
		float num2 = (flag4 ? _animation.TrackGetKeyTransition(track, key) : 1f);
		_updating = true;
		if (GodotObject.IsInstanceValid(_keyTransitionSpin))
		{
			_keyTransitionSpin.Editable = flag3 & flag4;
			_keyTransitionSpin.Value = num2;
		}
		_updating = false;
		_easeCurveCanvas.SetTransition(num2);
		_easeCurveCanvas.SetEditable(flag3 & flag4);
		if (GodotObject.IsInstanceValid(_easePresetShelf))
		{
			foreach (Node child3 in _easePresetShelf.GetChildren())
			{
				if (child3 is Button button3)
				{
					button3.Disabled = !flag3 || !flag4;
				}
			}
		}
		if (!flag)
		{
			_motionCurveStatusLabel.Text = "选择轨道后，这里会显示可视插值与循环衔接。";
			return;
		}
		if (!flag2)
		{
			_motionCurveStatusLabel.Text = "当前轨道仅显示；运动曲线直接编辑适用于 Value 属性轨。";
			return;
		}
		if (!flag4)
		{
			_motionCurveStatusLabel.Text = "轨道模式可编辑；选择一个菱形关键帧即可调整缓动曲线。";
			return;
		}
		if (updateMode == Animation.UpdateMode.Capture)
		{
			_motionCurveStatusLabel.Text = "捕获模式已开启；运行时会从对象当前值衔接，安全预览显示已保存关键帧部分。";
			return;
		}
		_motionCurveStatusLabel.Text = $"{GetInterpolationLabel(interpolationType)} · {GetUpdateModeLabel(updateMode)} · 曲率 {num2:0.###}";
	}

	private static void StyleMotionSegments(HFlowContainer host)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return;
		}
		foreach (Node child in host.GetChildren())
		{
			if (child is Button button)
			{
				button.ThemeTypeVariation = "GameNavCard";
				button.CustomMinimumSize = new Vector2(110f, 40f);
				button.ExpandIcon = false;
				button.AddThemeConstantOverride("icon_max_width", 24);
				XWUiMotion.BindButton(button, XWUiMotion.MotionRole.Card);
			}
		}
	}

	private static Label MakeSmallHeading(string text)
	{
		Label label = new Label();
		label.Text = text;
		label.AddThemeFontSizeOverride("font_size", 11);
		label.AddThemeColorOverride("font_color", ModEditorTheme.LeafColor.Lightened(0.42f));
		return label;
	}

	private static void AddOptionItem<TEnum>(OptionButton option, string label, TEnum value) where TEnum : struct, Enum
	{
		option.AddItem(label, Convert.ToInt32(value));
	}

	private static void SelectOptionById(OptionButton option, int id)
	{
		if (!GodotObject.IsInstanceValid(option))
		{
			return;
		}
		for (int i = 0; i < option.ItemCount; i++)
		{
			if (option.GetItemId(i) == id)
			{
				option.Select(i);
				break;
			}
		}
	}

	private static Texture2D LoadClassIcon(string iconName)
	{
		return ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ClassIcon/" + iconName + ".svg", null, ResourceLoader.CacheMode.Reuse);
	}

	private void BuildDialogs()
	{
		_nameDialog = new AcceptDialog
		{
			Title = "动画片段名称",
			DialogText = "输入中文或英文片段名称：",
			OkButtonText = "确定",
			MinSize = new Vector2I(420, 150)
		};
		_nameEdit = new LineEdit
		{
			PlaceholderText = "例如：待机、攻击、胜利"
		};
		_nameDialog.AddChild(_nameEdit, forceReadableName: false, InternalMode.Disabled);
		_nameDialog.Confirmed += OnNameDialogConfirmed;
		AddChild(_nameDialog, forceReadableName: false, InternalMode.Disabled);
		_deleteAnimationDialog = new ConfirmationDialog
		{
			Title = "删除动画片段",
			DialogText = "确定删除当前动画片段？此操作可以撤销。",
			OkButtonText = "删除"
		};
		_deleteAnimationDialog.Confirmed += DeleteCurrentAnimation;
		AddChild(_deleteAnimationDialog, forceReadableName: false, InternalMode.Disabled);
	}

	private void RefreshSources()
	{
		_updating = true;
		string b = ReadStateString(_pendingBindState, "player_path");
		_players.Clear();
		_playerOption.Clear();
		CollectAnimationPlayers(_sceneRoot, _players);
		for (int i = 0; i < _players.Count; i++)
		{
			AnimationPlayer animationPlayer = _players[i];
			string text = (IsNodeInsideScene(animationPlayer) ? _sceneRoot.GetPathTo(animationPlayer).ToString() : ((string?)animationPlayer.Name));
			_playerOption.AddItem("▶ " + text);
			if (string.Equals(text, b, StringComparison.Ordinal))
			{
				_playerOption.Select(i);
			}
		}
		if (_players.Count == 0)
		{
			_playerOption.AddItem("场景中没有 AnimationPlayer");
			_playerOption.SetItemDisabled(0, disabled: true);
			_player = null;
			RefreshLibraries();
			_updating = false;
			UpdateEditingState();
			SetStatus("没有找到动画播放器；可在场景树创建 AnimationPlayer 后刷新。", error: true);
		}
		else
		{
			int num = Math.Clamp(_playerOption.Selected, 0, _players.Count - 1);
			_playerOption.Select(num);
			_player = _players[num];
			RefreshLibraries();
			_updating = false;
			ApplyPendingState();
		}
	}

	private void RefreshLibraries()
	{
		string text = ReadStateString(_pendingBindState, "library");
		if (string.IsNullOrEmpty(text))
		{
			text = GetSelectedLibraryName();
		}
		_libraries.Clear();
		_libraryOption.Clear();
		if (GodotObject.IsInstanceValid(_player))
		{
			foreach (StringName animationLibrary in _player.GetAnimationLibraryList())
			{
				int itemCount = _libraryOption.ItemCount;
				_libraries.Add(animationLibrary);
				string text2 = (string.IsNullOrEmpty(animationLibrary.ToString()) ? "默认库" : animationLibrary.ToString());
				_libraryOption.AddItem("▤ " + text2);
				if (string.Equals(animationLibrary.ToString(), text, StringComparison.Ordinal))
				{
					_libraryOption.Select(itemCount);
				}
			}
		}
		if (_libraries.Count == 0)
		{
			_libraryOption.AddItem("没有动画库");
			_libraryOption.SetItemDisabled(0, disabled: true);
			_library = null;
		}
		else
		{
			int num = Math.Clamp(_libraryOption.Selected, 0, _libraries.Count - 1);
			_libraryOption.Select(num);
			_library = _player.GetAnimationLibrary(_libraries[num]);
		}
		RefreshAnimations();
	}

	private void RefreshAnimations(string requestedName = "")
	{
		string text = (string.IsNullOrWhiteSpace(requestedName) ? ReadStateString(_pendingBindState, "animation") : requestedName);
		if (string.IsNullOrEmpty(text))
		{
			text = GetSelectedAnimationName();
		}
		_animations.Clear();
		_animationOption.Clear();
		if (GodotObject.IsInstanceValid(_library))
		{
			foreach (StringName animation in _library.GetAnimationList())
			{
				int itemCount = _animationOption.ItemCount;
				_animations.Add(animation);
				_animationOption.AddItem($"◆ {animation}");
				if (string.Equals(animation.ToString(), text, StringComparison.Ordinal))
				{
					_animationOption.Select(itemCount);
				}
			}
		}
		if (_animations.Count == 0)
		{
			_animationOption.AddItem("动画库为空");
			_animationOption.SetItemDisabled(0, disabled: true);
			BindAnimation(null);
		}
		else
		{
			int num = Math.Clamp(_animationOption.Selected, 0, _animations.Count - 1);
			_animationOption.Select(num);
			BindAnimation(_library.GetAnimation(_animations[num]));
		}
	}

	private void BindAnimation(Animation animation)
	{
		StopPreview();
		_animation = animation;
		_selectedTrack = -1;
		_selectedKeyTime = -1.0;
		_timeline.BindAnimation(animation);
		RefreshTrackList();
		_updating = true;
		if (GodotObject.IsInstanceValid(animation))
		{
			_lengthSpin.Value = Math.Max(0.01, animation.Length);
			_stepSpin.Value = Math.Max(0.001, animation.Step);
			_loopButton.ButtonPressed = animation.LoopMode != Animation.LoopModeEnum.None;
			BindSafePreview();
		}
		else
		{
			_lengthSpin.Value = 1.0;
			_stepSpin.Value = 1.0 / 30.0;
			_loopButton.ButtonPressed = false;
			_preview.Clear();
		}
		_updating = false;
		UpdateTimeLabel(0.0);
		UpdateSourceInformation();
		UpdateEditingState();
	}

	private void BindSafePreview()
	{
		if (!GodotObject.IsInstanceValid(_player) || !GodotObject.IsInstanceValid(_animation))
		{
			_preview.Clear();
			return;
		}
		bool flag = _preview.Bind(_player, _animation, new StringName("__编辑预览_" + GetSelectedAnimationName()));
		_preview.SpeedScale = _speedSpin?.Value ?? 1.0;
		_previewSafetyLabel.Text = (flag ? $"隔离预览已建立：{_preview.BaselineCount} 个安全初始值，跳过 {_preview.UnsafeTrackCount} 条方法/音频/嵌套或不安全轨道。" : "无法建立安全预览；源播放器仍保持停止，不会执行动画事件。");
		_previewSafetyLabel.Modulate = (flag ? new Color("#9ee6a7") : new Color("#ff8d75"));
	}

	private void RefreshTrackList()
	{
		_updating = true;
		_trackList.Clear();
		if (!GodotObject.IsInstanceValid(_animation))
		{
			_updating = false;
			RefreshTrackSelection();
			return;
		}
		for (int i = 0; i < _animation.GetTrackCount(); i++)
		{
			Animation.TrackType type = _animation.TrackGetType(i);
			bool flag = IsPreviewUnsafeTrack(type);
			string value = (_animation.TrackIsEnabled(i) ? "●" : "○");
			string value2 = (flag ? "  ⛔ 仅显示" : "");
			_trackList.AddItem($"{value} {GetTrackTypeLabel(type)}  {_animation.TrackGetPath(i)}{value2}");
			_trackList.SetItemMetadata(i, i);
		}
		if (_selectedTrack >= 0 && _selectedTrack < _animation.GetTrackCount())
		{
			_trackList.Select(_selectedTrack);
		}
		else if (_animation.GetTrackCount() > 0)
		{
			_selectedTrack = 0;
			_trackList.Select(0);
		}
		_updating = false;
		RefreshTrackSelection();
	}

	private void RefreshTrackSelection()
	{
		int num;
		int num2;
		if (GodotObject.IsInstanceValid(_animation) && _selectedTrack >= 0)
		{
			num = ((_selectedTrack < _animation.GetTrackCount()) ? 1 : 0);
			if (num != 0)
			{
				num2 = (IsPreviewUnsafeTrack(_animation.TrackGetType(_selectedTrack)) ? 1 : 0);
				goto IL_0048;
			}
		}
		else
		{
			num = 0;
		}
		num2 = 0;
		goto IL_0048;
		IL_0048:
		bool flag = (byte)num2 != 0;
		_updating = true;
		if (num != 0)
		{
			Animation.TrackType type = _animation.TrackGetType(_selectedTrack);
			_selectedTrackLabel.Text = $"轨道 {_selectedTrack + 1} · {GetTrackTypeLabel(type)} · {_animation.TrackGetPath(_selectedTrack)}";
			_trackEnabledCheck.ButtonPressed = _animation.TrackIsEnabled(_selectedTrack);
		}
		else
		{
			_selectedTrackLabel.Text = "未选择轨道";
			_trackEnabledCheck.ButtonPressed = false;
		}
		_updating = false;
		RefreshValueEditor();
		RefreshMotionCurveEditor();
		UpdateEditingState();
		if (flag)
		{
			_selectedTrackLabel.Text += " · 仅显示（安全预览不执行）";
		}
	}

	private void RefreshValueEditor()
	{
		if (!GodotObject.IsInstanceValid(_valueEditorHost))
		{
			return;
		}
		foreach (Node child in _valueEditorHost.GetChildren())
		{
			child.QueueFree();
		}
		if (!TryGetSelectedKey(out var track, out var key))
		{
			_valueEditorHost.AddChild(MakeHintLabel("选择属性值轨道上的关键帧，可直接编辑布尔、整数、小数、文本、坐标与颜色。"), forceReadableName: false, InternalMode.Disabled);
			_selectedKeyLabel.Text = "选择时间轴上的菱形关键帧";
			return;
		}
		Variant value = _animation.TrackGetKeyValue(track, key);
		_selectedKeyLabel.Text = $"关键帧 {_selectedKeyTime:0.###} 秒 · {GetVariantTypeLabel(value.VariantType)}";
		Control control = CreateVariantEditor(value);
		if (control == null)
		{
			_valueEditorHost.AddChild(MakeHintLabel(GetVariantTypeLabel(value.VariantType) + " 当前仅显示。方法、音频、嵌套动画和复杂资源不会在编辑预览中执行。"), forceReadableName: false, InternalMode.Disabled);
		}
		else
		{
			_valueEditorHost.AddChild(control, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private Control CreateVariantEditor(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		Variant.Type num = variantType - 1;
		if ((ulong)num <= 4uL)
		{
			switch ((int)num)
			{
			case 0:
			{
				CheckButton checkButton = new CheckButton
				{
					Text = "开启",
					ButtonPressed = value.AsBool()
				};
				checkButton.Toggled += (bool next) =>
				{
					SetSelectedKeyValue(Variant.From(in next));
				};
				return Labeled("布尔开关", checkButton);
			}
			case 1:
			{
				SpinBox spinBox2 = MakeSpin(-1000000.0, 1000000.0, 1.0, value.AsInt64(), "0");
				spinBox2.ValueChanged += (double next) =>
				{
					if (!_updating)
					{
						SetSelectedKeyValue(Variant.From<long>((long)Math.Round(next)));
					}
				};
				return Labeled("整数值", spinBox2);
			}
			case 2:
			{
				SpinBox spinBox = MakeSpin(-1000000.0, 1000000.0, 0.01, value.AsDouble(), "0.###");
				spinBox.ValueChanged += (double next) =>
				{
					if (!_updating)
					{
						SetSelectedKeyValue(Variant.From(in next));
					}
				};
				return Labeled("小数值", spinBox);
			}
			case 3:
			{
				LineEdit text = new LineEdit
				{
					Text = value.AsString(),
					SizeFlagsHorizontal = SizeFlags.ExpandFill
				};
				text.TextSubmitted += (string next) =>
				{
					SetSelectedKeyValue(Variant.From(in next));
				};
				text.FocusExited += () =>
				{
					SetSelectedKeyValue(Variant.From<string>(text.Text));
				};
				return Labeled("文本内容", text);
			}
			case 4:
			{
				Vector2 vector = value.AsVector2();
				HBoxContainer hBoxContainer = new HBoxContainer();
				SpinBox x = MakeSpin(-1000000.0, 1000000.0, 0.1, vector.X, "X 0.##");
				SpinBox y = MakeSpin(-1000000.0, 1000000.0, 0.1, vector.Y, "Y 0.##");
				x.ValueChanged += (double next) =>
				{
					if (!_updating)
					{
						SetSelectedKeyValue(Variant.From<Vector2>(new Vector2((float)next, (float)y.Value)));
					}
				};
				y.ValueChanged += (double next) =>
				{
					if (!_updating)
					{
						SetSelectedKeyValue(Variant.From<Vector2>(new Vector2((float)x.Value, (float)next)));
					}
				};
				hBoxContainer.AddChild(x, forceReadableName: false, InternalMode.Disabled);
				hBoxContainer.AddChild(y, forceReadableName: false, InternalMode.Disabled);
				return Labeled("二维坐标", hBoxContainer);
			}
			}
		}
		if (variantType == Variant.Type.Color)
		{
			ColorPickerButton colorPickerButton = new ColorPickerButton
			{
				Color = value.AsColor(),
				CustomMinimumSize = new Vector2(160f, 36f),
				EditAlpha = true
			};
			colorPickerButton.ColorChanged += (Color next) =>
			{
				if (!_updating)
				{
					SetSelectedKeyValue(Variant.From(in next));
				}
			};
			return Labeled("颜色", colorPickerButton);
		}
		return null;
	}

	private void SetSelectedKeyValue(Variant value)
	{
		if (_updating || !TryGetSelectedKey(out var track, out var key))
		{
			return;
		}
		double time = _animation.TrackGetKeyTime(track, key);
		CommitAnimationEdit("直接编辑关键帧值", (Animation animation) =>
		{
			int num = FindKeyAtTime(animation, track, time);
			if (num < 0)
			{
				return false;
			}
			animation.TrackSetKeyValue(track, num, value);
			_selectedTrack = track;
			_selectedKeyTime = time;
			return true;
		});
	}

	private bool CommitAnimationEdit(string actionName, Func<Animation, bool> mutation)
	{
		if (!EnsureEditable(actionName) || mutation == null)
		{
			return false;
		}
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null)
		{
			return Fail("场景撤销历史不可用，已取消修改。");
		}
		PauseForMutation();
		Animation from = DuplicateAnimation(_animation);
		string from2 = GetSelectedAnimationName();
		int selectedTrack = _selectedTrack;
		double selectedKeyTime = _selectedKeyTime;
		if (from == null || !mutation(_animation))
		{
			if (from != null)
			{
				ReplaceAnimationDirect(_library, from2, from);
			}
			RefreshAnimations(from2);
			return Fail("动画修改没有完成。");
		}
		Animation from3 = DuplicateAnimation(_animation);
		if (from3 == null)
		{
			ReplaceAnimationDirect(_library, from2, from);
			RefreshAnimations(from2);
			return Fail("无法创建动画撤销快照。");
		}
		int selectedTrack2 = _selectedTrack;
		double selectedKeyTime2 = _selectedKeyTime;
		ReplaceAnimationDirect(_library, from2, from);
		xWUndoRedoManager.CreateAction(actionName, mergeMode: false, GetSceneHistoryId());
		xWUndoRedoManager.AddDoMethod(this, "ApplyAnimationSnapshot", Variant.From(in _library), Variant.From(in from2), Variant.From(in from3), selectedTrack2, selectedKeyTime2);
		xWUndoRedoManager.AddUndoMethod(this, "ApplyAnimationSnapshot", Variant.From(in _library), Variant.From(in from2), Variant.From(in from), selectedTrack, selectedKeyTime);
		xWUndoRedoManager.CommitAction();
		SetStatus(actionName + "；可使用场景撤销/重做。");
		return true;
	}

	public void ApplyAnimationSnapshot(AnimationLibrary library, string animationName, Animation snapshot, int selectedTrack, double selectedKeyTime)
	{
		if (!GodotObject.IsInstanceValid(library) || !GodotObject.IsInstanceValid(snapshot))
		{
			return;
		}
		ReplaceAnimationDirect(library, animationName, snapshot);
		if (_library == library)
		{
			double time = _timeline?.Playhead ?? 0.0;
			RefreshAnimations(animationName);
			_selectedTrack = (GodotObject.IsInstanceValid(_animation) ? Math.Clamp(selectedTrack, -1, _animation.GetTrackCount() - 1) : (-1));
			_selectedKeyTime = ((_selectedTrack >= 0 && selectedKeyTime >= 0.0 && FindKeyAtTime(_animation, _selectedTrack, selectedKeyTime) >= 0) ? selectedKeyTime : (-1.0));
			if (_selectedTrack >= 0 && _trackList.ItemCount > _selectedTrack)
			{
				_trackList.Select(_selectedTrack);
			}
			if (_selectedTrack >= 0 && _selectedKeyTime >= 0.0)
			{
				_timeline.SelectKey(_selectedTrack, _selectedKeyTime);
			}
			RefreshTrackSelection();
			OnTimelinePlayheadChanged(time);
			_editor?.QueueViewportRedraw();
		}
	}

	private bool CommitAnimationSlot(string actionName, AnimationLibrary library, string animationName, Animation snapshot, bool add, string selectionAfter)
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null || !GodotObject.IsInstanceValid(library) || !GodotObject.IsInstanceValid(snapshot))
		{
			return Fail("场景撤销历史或动画库不可用。");
		}
		PauseForMutation();
		xWUndoRedoManager.CreateAction(actionName, mergeMode: false, GetSceneHistoryId());
		if (add)
		{
			xWUndoRedoManager.AddDoMethod(this, "SetAnimationSlot", Variant.From(in library), Variant.From(in animationName), Variant.From(in snapshot), true, Variant.From(in selectionAfter));
			xWUndoRedoManager.AddUndoMethod(this, "SetAnimationSlot", Variant.From(in library), Variant.From(in animationName), Variant.From(in snapshot), false, Variant.From<string>(GetSelectedAnimationName()));
		}
		else
		{
			xWUndoRedoManager.AddDoMethod(this, "SetAnimationSlot", Variant.From(in library), Variant.From(in animationName), Variant.From(in snapshot), false, Variant.From(in selectionAfter));
			xWUndoRedoManager.AddUndoMethod(this, "SetAnimationSlot", Variant.From(in library), Variant.From(in animationName), Variant.From(in snapshot), true, Variant.From(in animationName));
		}
		xWUndoRedoManager.CommitAction();
		SetStatus(actionName + "；可使用场景撤销/重做。");
		return true;
	}

	public void SetAnimationSlot(AnimationLibrary library, string animationName, Animation snapshot, bool present, string requestedSelection)
	{
		if (GodotObject.IsInstanceValid(library))
		{
			if (library.HasAnimation(animationName))
			{
				library.RemoveAnimation(animationName);
			}
			if (present && GodotObject.IsInstanceValid(snapshot))
			{
				library.AddAnimation(animationName, DuplicateAnimation(snapshot));
			}
			if (_library == library)
			{
				RefreshAnimations(requestedSelection);
			}
			_editor?.QueueViewportRedraw();
		}
	}

	private void DuplicateCurrentAnimation()
	{
		if (GodotObject.IsInstanceValid(_animation) && GodotObject.IsInstanceValid(_library))
		{
			if (IsLibraryExternal(_library))
			{
				CopyCurrentAnimationToScene();
				return;
			}
			string text = BuildUniqueAnimationName(GetSelectedAnimationName() + "_副本");
			Animation animation = DuplicateAnimation(_animation);
			animation.ResourceLocalToScene = true;
			CommitAnimationSlot("复制动画片段", _library, text, animation, add: true, text);
		}
	}

	private void DeleteCurrentAnimation()
	{
		if (!EnsureEditable("删除动画片段") || !GodotObject.IsInstanceValid(_library))
		{
			return;
		}
		string selectedAnimationName = GetSelectedAnimationName();
		Animation snapshot = DuplicateAnimation(_animation);
		string selectionAfter = "";
		foreach (StringName animation in _library.GetAnimationList())
		{
			if (!string.Equals(animation.ToString(), selectedAnimationName, StringComparison.Ordinal))
			{
				selectionAfter = animation.ToString();
				break;
			}
		}
		CommitAnimationSlot("删除动画片段", _library, selectedAnimationName, snapshot, add: false, selectionAfter);
	}

	private void RenameCurrentAnimation(string requestedName)
	{
		if (!EnsureEditable("重命名动画片段") || !GodotObject.IsInstanceValid(_library))
		{
			return;
		}
		string from = GetSelectedAnimationName();
		string from2 = requestedName?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(from2))
		{
			Fail("动画名称不能为空。");
		}
		else
		{
			if (string.Equals(from, from2, StringComparison.Ordinal))
			{
				return;
			}
			if (_library.HasAnimation(from2))
			{
				Fail("动画片段“" + from2 + "”已存在。");
				return;
			}
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager == null)
			{
				Fail("场景撤销历史不可用。");
				return;
			}
			PauseForMutation();
			xWUndoRedoManager.CreateAction("重命名动画片段", mergeMode: false, GetSceneHistoryId());
			xWUndoRedoManager.AddDoMethod(this, "RenameAnimationSlot", Variant.From(in _library), Variant.From(in from), Variant.From(in from2));
			xWUndoRedoManager.AddUndoMethod(this, "RenameAnimationSlot", Variant.From(in _library), Variant.From(in from2), Variant.From(in from));
			xWUndoRedoManager.CommitAction();
		}
	}

	public void RenameAnimationSlot(AnimationLibrary library, string from, string to)
	{
		if (GodotObject.IsInstanceValid(library) && library.HasAnimation(from) && !library.HasAnimation(to))
		{
			library.RenameAnimation(from, to);
			if (_library == library)
			{
				RefreshAnimations(to);
			}
			_editor?.QueueViewportRedraw();
		}
	}

	private void CopyCurrentAnimationToScene()
	{
		if (GodotObject.IsInstanceValid(_player) && GodotObject.IsInstanceValid(_animation))
		{
			PauseForMutation();
			string from = FindWritableLibraryName();
			string from2 = BuildUniqueAnimationNameAcrossLibrary(from, GetSelectedAnimationName() + "_本地");
			Animation from3 = DuplicateAnimation(_animation);
			from3.ResourcePath = "";
			from3.ResourceLocalToScene = true;
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager == null)
			{
				Fail("场景撤销历史不可用。");
				return;
			}
			bool flag = !_player.HasAnimationLibrary(from);
			xWUndoRedoManager.CreateAction("复制动画到当前场景", mergeMode: false, GetSceneHistoryId());
			xWUndoRedoManager.AddDoMethod(this, "SetLocalAnimationCopy", Variant.From(in _player), Variant.From(in from), Variant.From(in from2), Variant.From(in from3), true, flag);
			xWUndoRedoManager.AddUndoMethod(this, "SetLocalAnimationCopy", Variant.From(in _player), Variant.From(in from), Variant.From(in from2), Variant.From(in from3), false, flag);
			xWUndoRedoManager.CommitAction();
		}
	}

	public void SetLocalAnimationCopy(AnimationPlayer player, string libraryName, string animationName, Animation snapshot, bool present, bool removeEmptyLibrary)
	{
		if (!GodotObject.IsInstanceValid(player))
		{
			return;
		}
		AnimationLibrary animationLibrary = (player.HasAnimationLibrary(libraryName) ? player.GetAnimationLibrary(libraryName) : null);
		if (present)
		{
			if (!GodotObject.IsInstanceValid(animationLibrary))
			{
				animationLibrary = new AnimationLibrary
				{
					ResourceLocalToScene = true
				};
				player.AddAnimationLibrary(libraryName, animationLibrary);
			}
			if (animationLibrary.HasAnimation(animationName))
			{
				animationLibrary.RemoveAnimation(animationName);
			}
			animationLibrary.AddAnimation(animationName, DuplicateAnimation(snapshot));
		}
		else if (GodotObject.IsInstanceValid(animationLibrary))
		{
			if (animationLibrary.HasAnimation(animationName))
			{
				animationLibrary.RemoveAnimation(animationName);
			}
			if (removeEmptyLibrary && animationLibrary.GetAnimationList().Count == 0)
			{
				player.RemoveAnimationLibrary(libraryName);
			}
		}
		if (_player == player)
		{
			_pendingBindState = new Dictionary
			{
				["library"] = libraryName,
				["animation"] = (present ? animationName : "")
			};
			RefreshLibraries();
			ApplyPendingState();
		}
		_editor?.QueueViewportRedraw();
	}

	private void DeleteSelectedTrack()
	{
		if (EnsureEditable("删除轨道") && _selectedTrack >= 0 && _selectedTrack < _animation.GetTrackCount())
		{
			int track = _selectedTrack;
			CommitAnimationEdit("删除动画轨道", (Animation animation) =>
			{
				animation.RemoveTrack(track);
				_selectedTrack = Math.Min(track, animation.GetTrackCount() - 1);
				_selectedKeyTime = -1.0;
				return true;
			});
		}
	}

	private void MoveSelectedTrack(int direction)
	{
		if (!EnsureEditable("移动轨道") || _selectedTrack < 0 || _selectedTrack >= _animation.GetTrackCount())
		{
			return;
		}
		int source = _selectedTrack;
		int target = Math.Clamp(source + Math.Sign(direction), 0, _animation.GetTrackCount() - 1);
		if (source != target)
		{
			CommitAnimationEdit("调整动画轨道顺序", (Animation animation) =>
			{
				animation.TrackMoveTo(source, target);
				_selectedTrack = target;
				return true;
			});
		}
	}

	private void ToggleSelectedTrack(bool enabled)
	{
		if (!_updating && EnsureEditable("启停轨道") && _selectedTrack >= 0 && _selectedTrack < _animation.GetTrackCount())
		{
			int track = _selectedTrack;
			CommitAnimationEdit(enabled ? "启用动画轨道" : "停用动画轨道", (Animation animation) =>
			{
				animation.TrackSetEnabled(track, enabled);
				_selectedTrack = track;
				return true;
			});
		}
	}

	private void DuplicateSelectedKey()
	{
		if (TryGetSelectedKey(out var track, out var key))
		{
			double num = _animation.TrackGetKeyTime(track, key);
			double target = ClampTime(num + GetStep());
			if (Mathf.IsEqualApprox(num, target))
			{
				target = ClampTime(num - GetStep());
			}
			Variant value = _animation.TrackGetKeyValue(track, key);
			float transition = _animation.TrackGetKeyTransition(track, key);
			CommitAnimationEdit("复制关键帧", (Animation animation) =>
			{
				int keyIdx = animation.TrackInsertKey(track, target, value, transition);
				_selectedTrack = track;
				_selectedKeyTime = animation.TrackGetKeyTime(track, keyIdx);
				return true;
			});
		}
	}

	private void OnTimelinePlayheadChanged(double time)
	{
		if (GodotObject.IsInstanceValid(_animation))
		{
			double num = ClampTime(time);
			_preview?.Seek(num);
			_timeline.SetPlayhead(num);
			UpdateTimeLabel(num);
		}
	}

	private void OnTimelineKeySelected(int track, double time)
	{
		_selectedTrack = track;
		_selectedKeyTime = time;
		if (_trackList.ItemCount > track)
		{
			_trackList.Select(track);
		}
		_preview?.Seek(ClampTime(time));
		RefreshTrackSelection();
	}

	private void OnTimelineKeyMoveRequested(int track, double oldTime, double newTime)
	{
		_selectedTrack = track;
		_selectedKeyTime = oldTime;
		MoveSelectedKey(newTime);
	}

	private void OnTrackListSelected(long index)
	{
		if (!_updating && index >= 0 && index < _trackList.ItemCount)
		{
			_selectedTrack = _trackList.GetItemMetadata((int)index).AsInt32();
			_selectedKeyTime = -1.0;
			RefreshTrackSelection();
		}
	}

	private void TogglePlayback()
	{
		if (!GodotObject.IsInstanceValid(_preview) || !GodotObject.IsInstanceValid(_animation) || _suspendedForSave)
		{
			return;
		}
		if (_preview.IsPlaying)
		{
			_preview.Pause();
			SetProcess(enable: false);
		}
		else
		{
			if (_preview.CurrentPosition >= _animation.Length - 0.0001)
			{
				_preview.Seek(0.0);
			}
			_preview.Play();
			SetProcess(IsVisibleInTree());
		}
		UpdatePlaybackButton();
	}

	public void StopPreview()
	{
		if (GodotObject.IsInstanceValid(_preview))
		{
			_preview.Stop();
		}
		SetProcess(enable: false);
		_timeline?.SetPlayhead(0.0);
		UpdateTimeLabel(0.0);
		UpdatePlaybackButton();
	}

	private void PauseForMutation()
	{
		if (GodotObject.IsInstanceValid(_preview) && _preview.IsPlaying)
		{
			_preview.Pause();
		}
		SetProcess(enable: false);
		UpdatePlaybackButton();
	}

	private void JumpToKey(int direction)
	{
		if (!GodotObject.IsInstanceValid(_animation))
		{
			return;
		}
		double playhead = _timeline.Playhead;
		double num = ((direction < 0) ? (-1.0 / 0.0) : (1.0 / 0.0));
		for (int i = 0; i < _animation.GetTrackCount(); i++)
		{
			for (int j = 0; j < _animation.TrackGetKeyCount(i); j++)
			{
				double num2 = _animation.TrackGetKeyTime(i, j);
				if (direction < 0 && num2 < playhead - 0.0001 && num2 > num)
				{
					num = num2;
				}
				if (direction > 0 && num2 > playhead + 0.0001 && num2 < num)
				{
					num = num2;
				}
			}
		}
		if (double.IsInfinity(num))
		{
			num = ((direction < 0) ? 0.0 : _animation.Length);
		}
		OnTimelinePlayheadChanged(num);
	}

	private void OnLoopToggled(bool enabled)
	{
		if (!_updating && EnsureEditable("设置动画循环"))
		{
			CommitAnimationEdit(enabled ? "开启动画循环" : "关闭动画循环", (Animation animation) =>
			{
				animation.LoopMode = (Animation.LoopModeEnum)(enabled ? 1 : 0);
				return true;
			});
		}
	}

	private void OnSpeedChanged(double value)
	{
		if (!_updating && GodotObject.IsInstanceValid(_preview))
		{
			_preview.SpeedScale = value;
			SetStatus($"安全预览速度：{value:0.0}×。");
		}
	}

	private void OnLengthChanged(double value)
	{
		if (!_updating && EnsureEditable("调整动画长度"))
		{
			CommitAnimationEdit("调整动画长度", (Animation animation) =>
			{
				animation.Length = Math.Max(0.01, value);
				return true;
			});
		}
	}

	private void OnStepChanged(double value)
	{
		if (!_updating && EnsureEditable("调整动画步长"))
		{
			CommitAnimationEdit("调整动画步长", (Animation animation) =>
			{
				animation.Step = (float)Math.Max(0.001, value);
				return true;
			});
		}
	}

	private void OnPlayerSelected(long index)
	{
		if (!_updating && index >= 0 && index < _players.Count)
		{
			_player = _players[(int)index];
			_pendingBindState = new Dictionary();
			RefreshLibraries();
			UpdateAnimationTreeLink();
		}
	}

	private void OnLibrarySelected(long index)
	{
		if (!_updating && GodotObject.IsInstanceValid(_player) && index >= 0 && index < _libraries.Count)
		{
			_library = _player.GetAnimationLibrary(_libraries[(int)index]);
			_pendingBindState = new Dictionary();
			RefreshAnimations();
		}
	}

	private void OnAnimationSelected(long index)
	{
		if (!_updating && GodotObject.IsInstanceValid(_library) && index >= 0 && index < _animations.Count)
		{
			_pendingBindState = new Dictionary();
			BindAnimation(_library.GetAnimation(_animations[(int)index]));
		}
	}

	private void ApplyPendingState()
	{
		Dictionary pendingBindState = _pendingBindState;
		if (pendingBindState != null && pendingBindState.Count != 0)
		{
			_updating = true;
			SelectOptionByString(_libraryOption, _libraries, ReadStateString(pendingBindState, "library"));
			if (_libraryOption.Selected >= 0 && _libraryOption.Selected < _libraries.Count)
			{
				_library = _player.GetAnimationLibrary(_libraries[_libraryOption.Selected]);
				RefreshAnimations(ReadStateString(pendingBindState, "animation"));
			}
			double value = ReadStateDouble(pendingBindState, "speed", 1.0);
			double value2 = ReadStateDouble(pendingBindState, "zoom", 120.0);
			_speedSpin.Value = Math.Clamp(value, 0.1, 4.0);
			_zoomSpin.Value = Math.Clamp(value2, 24.0, 480.0);
			_timeline.SetTimeScale(_zoomSpin.Value);
			_preview.SpeedScale = _speedSpin.Value;
			bool buttonPressed = ReadStateBool(pendingBindState, "loop", GodotObject.IsInstanceValid(_animation) && _animation.LoopMode != Animation.LoopModeEnum.None);
			_loopButton.ButtonPressed = buttonPressed;
			double num = ClampTime(ReadStateDouble(pendingBindState, "playhead", 0.0));
			_timeline.SetPlayhead(num);
			_preview.Seek(num);
			_updating = false;
			_pendingBindState = null;
			UpdateTimeLabel(num);
		}
	}

	private void UpdateSourceInformation()
	{
		UpdateAnimationTreeLink();
		bool flag = IsCurrentAnimationReadOnly();
		if (!GodotObject.IsInstanceValid(_animation))
		{
			_readonlyLabel.Text = "动画库中还没有片段。";
		}
		else if (flag)
		{
			_readonlyLabel.Text = "\ud83d\udd12 外部/共享动画只读：不会直接改写原资源。点击“复制到当前场景”后可自由编辑。";
		}
		else
		{
			_readonlyLabel.Text = "✦ 当前场景本地动画：所有修改写入独立场景撤销历史。";
		}
		_readonlyLabel.Modulate = (flag ? new Color("#ffcf76") : new Color("#9ee6a7"));
		_copyLocalButton.Visible = GodotObject.IsInstanceValid(_animation) & flag;
	}

	private void UpdateAnimationTreeLink()
	{
		if (!GodotObject.IsInstanceValid(_sceneRoot) || !GodotObject.IsInstanceValid(_player))
		{
			_treeLinkLabel.Text = "未发现 AnimationTree 关联。";
			return;
		}
		List<string> list = new List<string>();
		CollectAnimationTrees(_sceneRoot, _player, list);
		_treeLinkLabel.Text = ((list.Count == 0) ? "未发现 AnimationTree 关联；当前直接编辑 AnimationPlayer 片段。" : ("检测到 AnimationTree：" + string.Join("、", list) + "；已解析其 AnimPlayer 关联，仅预览底层安全属性轨道。"));
	}

	private void UpdateEditingState()
	{
		bool flag = GodotObject.IsInstanceValid(_animation);
		bool flag2 = flag && !IsCurrentAnimationReadOnly();
		bool flag3 = flag && IsEditableValueTrack(_selectedTrack);
		bool flag4 = flag && _selectedTrack >= 0 && _selectedTrack < _animation.GetTrackCount() && IsPreviewUnsafeTrack(_animation.TrackGetType(_selectedTrack));
		bool flag5 = TryGetSelectedKey(out var _, out var _);
		Button[] array = new Button[3] { _deleteTrackButton, _moveTrackUpButton, _moveTrackDownButton };
		foreach (Button button in array)
		{
			if (GodotObject.IsInstanceValid(button))
			{
				button.Disabled = (!flag2 || _selectedTrack < 0) | flag4;
			}
		}
		if (GodotObject.IsInstanceValid(_trackEnabledCheck))
		{
			_trackEnabledCheck.Disabled = (!flag2 || _selectedTrack < 0) | flag4;
		}
		if (GodotObject.IsInstanceValid(_insertKeyButton))
		{
			_insertKeyButton.Disabled = !flag2 || !flag3;
		}
		array = new Button[4] { _deleteKeyButton, _duplicateKeyButton, _moveKeyEarlierButton, _moveKeyLaterButton };
		foreach (Button button2 in array)
		{
			if (GodotObject.IsInstanceValid(button2))
			{
				button2.Disabled = !flag2 || !flag5 || !flag3;
			}
		}
		if (GodotObject.IsInstanceValid(_lengthSpin))
		{
			_lengthSpin.Editable = flag2;
		}
		if (GodotObject.IsInstanceValid(_stepSpin))
		{
			_stepSpin.Editable = flag2;
		}
		if (GodotObject.IsInstanceValid(_playPauseButton))
		{
			_playPauseButton.Disabled = !flag;
		}
	}

	private void UpdatePlaybackButton()
	{
		if (GodotObject.IsInstanceValid(_playPauseButton))
		{
			_playPauseButton.Text = ((GodotObject.IsInstanceValid(_preview) && _preview.IsPlaying) ? "Ⅱ 暂停" : "▶ 播放");
		}
	}

	private void UpdateProcessing()
	{
		bool flag = IsVisibleInTree();
		if (!flag)
		{
			_easeCurveCanvas?.CancelPendingDrag();
			if (GodotObject.IsInstanceValid(_preview) && _preview.IsPlaying)
			{
				_preview.Pause();
			}
		}
		if (flag)
		{
			RefreshMotionCurveEditor();
		}
		SetProcess(flag && GodotObject.IsInstanceValid(_preview) && _preview.IsPlaying);
		UpdatePlaybackButton();
	}

	private void UpdateTimeLabel(double time)
	{
		if (GodotObject.IsInstanceValid(_timeLabel))
		{
			_timeLabel.Text = $"{time:0.00} / {(GodotObject.IsInstanceValid(_animation) ? _animation.Length : 0.0):0.00} 秒";
		}
	}

	private bool EnsureEditable(string operation)
	{
		if (!GodotObject.IsInstanceValid(_animation))
		{
			return Fail("无法" + operation + "：请先选择动画片段。");
		}
		if (IsCurrentAnimationReadOnly())
		{
			return Fail("无法" + operation + "：外部/共享动画只读，请先复制到当前场景。");
		}
		return true;
	}

	private bool IsCurrentAnimationReadOnly()
	{
		if (!GodotObject.IsInstanceValid(_animation) || !GodotObject.IsInstanceValid(_library))
		{
			return true;
		}
		if (!IsLibraryExternal(_library))
		{
			return IsResourceExternal(_animation);
		}
		return true;
	}

	private bool IsLibraryExternal(AnimationLibrary library)
	{
		return IsResourceExternal(library);
	}

	private bool IsResourceExternal(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource) || string.IsNullOrWhiteSpace(resource.ResourcePath))
		{
			return false;
		}
		string text = _sceneRoot?.SceneFilePath ?? "";
		if (!string.IsNullOrWhiteSpace(text) && resource.ResourcePath.StartsWith(text + "::", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return !resource.ResourcePath.Contains("::", StringComparison.Ordinal);
	}

	private bool TryReadCurrentTrackValue(out Variant value)
	{
		value = default;
		if (!GodotObject.IsInstanceValid(_animation) || !GodotObject.IsInstanceValid(_player) || !IsEditableValueTrack(_selectedTrack))
		{
			return Fail("请选择一条属性值轨道。");
		}
		Node node = ResolveAnimationRoot(_player);
		string text = _animation.TrackGetPath(_selectedTrack).ToString();
		int num = text.LastIndexOf(':');
		if (num < 0)
		{
			return Fail("轨道没有可读取的属性路径。");
		}
		string text2 = text.Substring(0, num);
		string text3 = text;
		int num2 = num + 1;
		string text4 = text3.Substring(num2, text3.Length - num2);
		bool flag = (((text2 != null && text2.Length == 0) || text2 == ".") ? true : false);
		Node node2 = (flag ? node : node?.GetNodeOrNull(new NodePath(text2)));
		if (!GodotObject.IsInstanceValid(node2))
		{
			return Fail("找不到轨道目标节点：" + text2);
		}
		value = node2.Get(text4);
		if (!IsSupportedEditableVariant(value.VariantType))
		{
			return Fail("当前属性类型 " + GetVariantTypeLabel(value.VariantType) + " 仅显示，不能直接写入。");
		}
		return true;
	}

	private bool TryGetSelectedKey(out int track, out int key)
	{
		track = _selectedTrack;
		key = -1;
		if (!GodotObject.IsInstanceValid(_animation) || track < 0 || track >= _animation.GetTrackCount() || _selectedKeyTime < -0.0001)
		{
			return false;
		}
		key = FindKeyAtTime(_animation, track, _selectedKeyTime);
		return key >= 0;
	}

	private bool IsEditableValueTrack(int track)
	{
		if (GodotObject.IsInstanceValid(_animation) && track >= 0 && track < _animation.GetTrackCount())
		{
			return _animation.TrackGetType(track) == Animation.TrackType.Value;
		}
		return false;
	}

	private double ClampTime(double time)
	{
		return Math.Clamp(time, 0.0, GodotObject.IsInstanceValid(_animation) ? Math.Max(0.0, _animation.Length) : 0.0);
	}

	private double GetStep()
	{
		if (!GodotObject.IsInstanceValid(_animation))
		{
			return 1.0 / 30.0;
		}
		return Math.Max(0.001, _animation.Step);
	}

	private int GetSceneHistoryId()
	{
		if (!GodotObject.IsInstanceValid(_editor))
		{
			return XWEditorInterface.Instance?.GetCurrentSceneHistoryId() ?? 1;
		}
		return _editor.GetCurrentSceneHistoryId();
	}

	private string GetSelectedLibraryName()
	{
		int num = _libraryOption?.Selected ?? (-1);
		if (num < 0 || num >= _libraries.Count)
		{
			return "";
		}
		return _libraries[num].ToString();
	}

	private string GetSelectedAnimationName()
	{
		int num = _animationOption?.Selected ?? (-1);
		if (num < 0 || num >= _animations.Count)
		{
			return "";
		}
		return _animations[num].ToString();
	}

	private string BuildUniqueAnimationName(string baseName)
	{
		if (!GodotObject.IsInstanceValid(_library))
		{
			return baseName;
		}
		string text = baseName;
		int num = 2;
		while (_library.HasAnimation(text))
		{
			text = $"{baseName}_{num++}";
		}
		return text;
	}

	private string BuildUniqueAnimationNameAcrossLibrary(string libraryName, string baseName)
	{
		if (!GodotObject.IsInstanceValid(_player) || !_player.HasAnimationLibrary(libraryName))
		{
			return baseName;
		}
		AnimationLibrary animationLibrary = _player.GetAnimationLibrary(libraryName);
		string text = baseName;
		int num = 2;
		while (animationLibrary.HasAnimation(text))
		{
			text = $"{baseName}_{num++}";
		}
		return text;
	}

	private string FindWritableLibraryName()
	{
		if (GodotObject.IsInstanceValid(_player))
		{
			foreach (StringName animationLibrary2 in _player.GetAnimationLibraryList())
			{
				AnimationLibrary animationLibrary = _player.GetAnimationLibrary(animationLibrary2);
				if (!IsLibraryExternal(animationLibrary))
				{
					return animationLibrary2.ToString();
				}
			}
		}
		return "场景动画";
	}

	private void OpenNameDialog(string operation, string title, string initial)
	{
		_pendingNameOperation = operation;
		_nameDialog.Title = title;
		_nameEdit.Text = initial ?? "";
		_nameDialog.PopupCentered();
		_nameEdit.GrabFocus();
		_nameEdit.SelectAll();
	}

	private void OnNameDialogConfirmed()
	{
		string text = _nameEdit.Text;
		string pendingNameOperation = _pendingNameOperation;
		if (!(pendingNameOperation == "create"))
		{
			if (pendingNameOperation == "rename")
			{
				RenameCurrentAnimation(text);
			}
		}
		else
		{
			CreateAnimation(text);
		}
		_pendingNameOperation = "";
	}

	private void ConfirmDeleteAnimation()
	{
		if (GodotObject.IsInstanceValid(_animation))
		{
			_deleteAnimationDialog.DialogText = "确定删除动画片段“" + GetSelectedAnimationName() + "”？此操作可以撤销。";
			_deleteAnimationDialog.PopupCentered();
		}
	}

	private bool Fail(string message)
	{
		SetStatus(message, error: true);
		return false;
	}

	private void SetStatus(string message, bool error = false)
	{
		if (GodotObject.IsInstanceValid(_statusLabel))
		{
			_statusLabel.Text = message ?? "";
			_statusLabel.Modulate = (error ? new Color("#ff8876") : new Color("#a8d8f0"));
		}
	}

	private void AddPropertyChoices(Node node)
	{
		HashSet<string> names = new HashSet<string>(StringComparer.Ordinal);
		if (node is Node2D)
		{
			AddPropertyChoice(node, "position", "◇ 位置", names);
			AddPropertyChoice(node, "rotation", "↻ 旋转", names);
			AddPropertyChoice(node, "scale", "↔ 缩放", names);
			AddPropertyChoice(node, "skew", "⟋ 倾斜", names);
		}
		if (node is Control)
		{
			AddPropertyChoice(node, "position", "◇ UI 位置", names);
			AddPropertyChoice(node, "size", "▭ UI 尺寸", names);
			AddPropertyChoice(node, "pivot_offset", "⊹ 旋转中心", names);
			AddPropertyChoice(node, "rotation", "↻ UI 旋转", names);
			AddPropertyChoice(node, "scale", "↔ UI 缩放", names);
		}
		if (node is CanvasItem)
		{
			AddPropertyChoice(node, "visible", "◉ 可见性", names);
			AddPropertyChoice(node, "modulate", "▧ 整体颜色", names);
			AddPropertyChoice(node, "self_modulate", "▧ 自身颜色", names);
		}
		if (node is Sprite2D)
		{
			AddPropertyChoice(node, "frame", "▣ 图集帧", names);
			AddPropertyChoice(node, "flip_h", "⇋ 水平翻转", names);
			AddPropertyChoice(node, "flip_v", "⇅ 垂直翻转", names);
		}
		if ((node is Label || node is RichTextLabel || node is LineEdit || node is TextEdit) ? true : false)
		{
			AddPropertyChoice(node, "text", "文 文本内容", names);
		}
		if (node is Godot.Range)
		{
			AddPropertyChoice(node, "value", "◫ 数值", names);
		}
		if (node is AnimatedSprite2D)
		{
			AddPropertyChoice(node, "frame", "▣ 动画帧编号", names);
		}
	}

	private void AddPropertyChoice(Node node, string property, string label, HashSet<string> names)
	{
		if (names.Add(property))
		{
			Variant value = node.Get(property);
			if (IsSupportedEditableVariant(value.VariantType))
			{
				_properties.Add(new PropertyChoice(node, property, $"{label}  ·  {node.Name}  ·  {FormatVariant(value)}", value));
			}
		}
	}

	private bool IsNodeInsideScene(Node node)
	{
		if (GodotObject.IsInstanceValid(node) && GodotObject.IsInstanceValid(_sceneRoot))
		{
			if (node != _sceneRoot)
			{
				return _sceneRoot.IsAncestorOf(node);
			}
			return true;
		}
		return false;
	}

	private static Node ResolveAnimationRoot(AnimationPlayer player)
	{
		if (!GodotObject.IsInstanceValid(player))
		{
			return null;
		}
		return player.GetNodeOrNull(player.RootNode) ?? player;
	}

	private static Animation DuplicateAnimation(Animation source)
	{
		if (!GodotObject.IsInstanceValid(source))
		{
			return null;
		}
		return source.Duplicate(deep: true) as Animation;
	}

	private static void ReplaceAnimationDirect(AnimationLibrary library, string name, Animation source)
	{
		if (GodotObject.IsInstanceValid(library) && GodotObject.IsInstanceValid(source))
		{
			if (library.HasAnimation(name))
			{
				library.RemoveAnimation(name);
			}
			library.AddAnimation(name, DuplicateAnimation(source));
		}
	}

	private static int FindKeyAtTime(Animation animation, int track, double time)
	{
		if (!GodotObject.IsInstanceValid(animation) || track < 0 || track >= animation.GetTrackCount())
		{
			return -1;
		}
		for (int i = 0; i < animation.TrackGetKeyCount(track); i++)
		{
			if (Math.Abs(animation.TrackGetKeyTime(track, i) - time) <= 0.0001)
			{
				return i;
			}
		}
		return -1;
	}

	private static bool IsPreviewUnsafeTrack(Animation.TrackType type)
	{
		if (type == Animation.TrackType.Method || (ulong)(type - 7) <= 1uL)
		{
			return true;
		}
		return false;
	}

	private static bool IsSupportedEditableVariant(Variant.Type type)
	{
		if ((ulong)(type - 1) <= 4uL || type == Variant.Type.Color)
		{
			return true;
		}
		return false;
	}

	private static string GetTrackTypeLabel(Animation.TrackType type)
	{
		Animation.TrackType trackType = type;
		if ((ulong)trackType <= 8uL)
		{
			switch ((int)trackType)
			{
			case 0:
				return "属性";
			case 5:
				return "方法";
			case 7:
				return "音频";
			case 8:
				return "嵌套动画";
			case 6:
				return "曲线";
			case 1:
				return "3D 位置";
			case 2:
				return "3D 旋转";
			case 3:
				return "3D 缩放";
			case 4:
				return "混合形状";
			}
		}
		return type.ToString();
	}

	private bool IsRotationTrack(int track)
	{
		if (!GodotObject.IsInstanceValid(_animation) || track < 0 || track >= _animation.GetTrackCount())
		{
			return false;
		}
		return _animation.TrackGetPath(track).ToString().EndsWith(":rotation", StringComparison.OrdinalIgnoreCase);
	}

	private static bool IsAngleInterpolation(Animation.InterpolationType interpolation)
	{
		if ((ulong)(interpolation - 3) <= 1uL)
		{
			return true;
		}
		return false;
	}

	private static string GetInterpolationLabel(Animation.InterpolationType interpolation)
	{
		Animation.InterpolationType interpolationType = interpolation;
		if ((ulong)interpolationType <= 4uL)
		{
			switch ((int)interpolationType)
			{
			case 0:
				return "保持";
			case 1:
				return "线性";
			case 2:
				return "三次平滑";
			case 3:
				return "角度线性";
			case 4:
				return "角度平滑";
			}
		}
		return interpolation.ToString();
	}

	private static string GetUpdateModeLabel(Animation.UpdateMode updateMode)
	{
		Animation.UpdateMode updateMode2 = updateMode;
		if ((ulong)updateMode2 <= 2uL)
		{
			switch ((int)updateMode2)
			{
			case 0:
				return "连续";
			case 1:
				return "离散";
			case 2:
				return "捕获";
			}
		}
		return updateMode.ToString();
	}

	private static float NormalizeTransition(float transition)
	{
		if (!float.IsFinite(transition))
		{
			return 1f;
		}
		return Math.Clamp(transition, -1000000f, 1000000f);
	}

	private static string GetVariantTypeLabel(Variant.Type type)
	{
		Variant.Type num = type - 1;
		if ((ulong)num <= 4uL)
		{
			switch ((int)num)
			{
			case 0:
				return "开关";
			case 1:
				return "整数";
			case 2:
				return "小数";
			case 3:
				return "文本";
			case 4:
				return "二维坐标";
			}
		}
		if (type == Variant.Type.Color)
		{
			return "颜色";
		}
		return type.ToString();
	}

	private static string FormatVariant(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		Variant.Type num = variantType - 1;
		if ((ulong)num <= 4uL)
		{
			switch ((int)num)
			{
			case 0:
				return value.AsBool() ? "开启" : "关闭";
			case 1:
				return value.AsInt64().ToString();
			case 2:
				return value.AsDouble().ToString("0.###");
			case 3:
				return value.AsString();
			case 4:
				return $"({value.AsVector2().X:0.#}, {value.AsVector2().Y:0.#})";
			}
		}
		if (variantType == Variant.Type.Color)
		{
			return "#" + value.AsColor().ToHtml();
		}
		return value.ToString();
	}

	private static void CollectAnimationPlayers(Node node, List<AnimationPlayer> output)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AnimationPlayer item)
		{
			output.Add(item);
		}
		foreach (Node child in node.GetChildren())
		{
			CollectAnimationPlayers(child, output);
		}
	}

	private static void CollectAnimationTrees(Node node, AnimationPlayer player, List<string> output)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AnimationTree animationTree && animationTree.GetNodeOrNull(animationTree.AnimPlayer) == player)
		{
			output.Add(animationTree.Name);
		}
		foreach (Node child in node.GetChildren())
		{
			CollectAnimationTrees(child, player, output);
		}
	}

	private static void SelectOptionByString(OptionButton option, List<StringName> values, string requested)
	{
		if (option == null || string.IsNullOrEmpty(requested))
		{
			return;
		}
		for (int i = 0; i < values.Count; i++)
		{
			if (string.Equals(values[i].ToString(), requested, StringComparison.Ordinal))
			{
				option.Select(i);
				break;
			}
		}
	}

	private static string ReadStateString(Dictionary state, string key)
	{
		if (state == null || !state.ContainsKey(key))
		{
			return "";
		}
		return state[key].AsString();
	}

	private static double ReadStateDouble(Dictionary state, string key, double fallback)
	{
		if (state == null || !state.ContainsKey(key))
		{
			return fallback;
		}
		return state[key].AsDouble();
	}

	private static bool ReadStateBool(Dictionary state, string key, bool fallback)
	{
		if (state == null || !state.ContainsKey(key))
		{
			return fallback;
		}
		return state[key].AsBool();
	}

	private static VBoxContainer MakeSection(VBoxContainer parent, string title, string subtitle)
	{
		VBoxContainer vBoxContainer = MakeStandaloneSection(title, subtitle);
		parent.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private static VBoxContainer MakeStandaloneSection(string title, string subtitle)
	{
		PanelContainer panelContainer = new PanelContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		panelContainer.AddThemeStyleboxOverride("panel", MakePanelStyle(new Color("#172534"), new Color("#2c4962"), 8, 1));
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.AddThemeConstantOverride("separation", 5);
		panelContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		Label label = new Label
		{
			Text = title
		};
		label.AddThemeFontSizeOverride("font_size", 15);
		label.AddThemeColorOverride("font_color", new Color("#f1d487"));
		vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		if (!string.IsNullOrWhiteSpace(subtitle))
		{
			Label label2 = MakeHintLabel(subtitle);
			label2.Modulate = new Color("#8eacc1");
			vBoxContainer.AddChild(label2, forceReadableName: false, InternalMode.Disabled);
		}
		VBoxContainer vBoxContainer2 = new VBoxContainer();
		vBoxContainer2.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer2;
	}

	private static Control Labeled(string text, Control control)
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		Label label = new Label
		{
			Text = text
		};
		label.AddThemeFontSizeOverride("font_size", 11);
		label.AddThemeColorOverride("font_color", new Color("#8fabc0"));
		vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(control, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private static OptionButton MakeOption(string tooltip)
	{
		return new OptionButton
		{
			TooltipText = tooltip,
			FitToLongestItem = false,
			CustomMinimumSize = new Vector2(150f, 34f)
		};
	}

	private static Button MakeButton(string text, string tooltip)
	{
		Button button = new Button();
		button.Text = text;
		button.TooltipText = tooltip;
		button.CustomMinimumSize = new Vector2(38f, 34f);
		button.AddThemeStyleboxOverride("normal", MakePanelStyle(new Color("#20364a"), new Color("#426985"), 6, 1));
		button.AddThemeStyleboxOverride("hover", MakePanelStyle(new Color("#2c4d65"), new Color("#68b9d8"), 6, 1));
		button.AddThemeStyleboxOverride("pressed", MakePanelStyle(new Color("#132534"), new Color("#f0c86f"), 6, 1));
		return button;
	}

	private static Label MakeIconBadge(string text, Color color)
	{
		Label label = new Label();
		label.Text = text;
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.VerticalAlignment = VerticalAlignment.Center;
		label.CustomMinimumSize = new Vector2(34f, 34f);
		label.AddThemeFontSizeOverride("font_size", 20);
		label.AddThemeColorOverride("font_color", color);
		label.AddThemeStyleboxOverride("normal", MakePanelStyle(new Color("#172c3b"), color.Darkened(0.35f), 17, 1));
		return label;
	}

	private static Label MakeHintLabel(string text)
	{
		return new Label
		{
			Text = text,
			AutowrapMode = TextServer.AutowrapMode.WordSmart,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
	}

	private static SpinBox MakeSpin(double min, double max, double step, double value, string suffix)
	{
		return new SpinBox
		{
			MinValue = min,
			MaxValue = max,
			Step = step,
			Value = value,
			Suffix = suffix,
			AllowGreater = false,
			AllowLesser = false,
			CustomMinimumSize = new Vector2(105f, 34f)
		};
	}

	private static StyleBoxFlat MakePanelStyle(Color background, Color border, int radius, int width)
	{
		return new StyleBoxFlat
		{
			BgColor = background,
			BorderColor = border,
			BorderWidthLeft = width,
			BorderWidthTop = width,
			BorderWidthRight = width,
			BorderWidthBottom = width,
			CornerRadiusTopLeft = radius,
			CornerRadiusTopRight = radius,
			CornerRadiusBottomLeft = radius,
			CornerRadiusBottomRight = radius,
			ContentMarginLeft = 8f,
			ContentMarginTop = 6f,
			ContentMarginRight = 8f,
			ContentMarginBottom = 6f
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(137)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sceneRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SuspendForSave, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResumeAfterSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshFromSceneHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateAnimation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "requestedName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectAnimation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "playerPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "libraryName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "animationName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectKey, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "trackIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "keyTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SeekPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PausePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddValueTrack, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddValueTrack, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddValueTrack, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelectedTrackInterpolation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "interpolation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelectedValueUpdateMode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "updateMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelectedTrackInterpolationLoopWrap, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelectedKeyTransition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InsertCurrentKey, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InsertCurrentKey, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveSelectedKey, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "newTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DeleteSelectedKey, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildInterface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshWorkbenchLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildMotionCurveCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddEasePreset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTrackInterpolationSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnValueUpdateModeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnEaseTransitionPreviewChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnEaseTransitionCommitRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginTransitionSpinEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTransitionSpinValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitTransitionSpinEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitPendingTransitionInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshMotionCurveEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StyleMotionSegments, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.MakeSmallHeading, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectOptionById, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadClassIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "iconName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildDialogs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshSources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshLibraries, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshAnimations, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "requestedName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindSafePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshTrackList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshTrackSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshValueEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateVariantEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelectedKeyValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyAnimationSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "library", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AnimationLibrary"), exported: false),
				new PropertyInfo(Variant.Type.String, "animationName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false),
				new PropertyInfo(Variant.Type.Int, "selectedTrack", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "selectedKeyTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitAnimationSlot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "library", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AnimationLibrary"), exported: false),
				new PropertyInfo(Variant.Type.String, "animationName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "add", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "selectionAfter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnimationSlot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "library", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AnimationLibrary"), exported: false),
				new PropertyInfo(Variant.Type.String, "animationName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "present", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "requestedSelection", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DuplicateCurrentAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeleteCurrentAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenameCurrentAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "requestedName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenameAnimationSlot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "library", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AnimationLibrary"), exported: false),
				new PropertyInfo(Variant.Type.String, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyCurrentAnimationToScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetLocalAnimationCopy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "player", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AnimationPlayer"), exported: false),
				new PropertyInfo(Variant.Type.String, "libraryName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "animationName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "present", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "removeEmptyLibrary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DeleteSelectedTrack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedTrack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToggleSelectedTrack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DuplicateSelectedKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTimelinePlayheadChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTimelineKeySelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "track", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTimelineKeyMoveRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "track", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "oldTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "newTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTrackListSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TogglePlayback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PauseForMutation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpToKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnLoopToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSpeedChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnLengthChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnStepChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPlayerSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnLibrarySelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPendingState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSourceInformation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateAnimationTreeLink, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateEditingState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePlaybackButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateTimeLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureEditable, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "operation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsCurrentAnimationReadOnly, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsLibraryExternal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "library", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AnimationLibrary"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsResourceExternal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsEditableValueTrack, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "track", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClampTime, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetStep, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSceneHistoryId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedLibraryName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedAnimationName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildUniqueAnimationName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "baseName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildUniqueAnimationNameAcrossLibrary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "libraryName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "baseName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindWritableLibraryName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenNameDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "operation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "initial", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnNameDialogConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfirmDeleteAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Fail, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddPropertyChoices, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsNodeInsideScene, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveAnimationRoot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "player", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AnimationPlayer"), exported: false)
			}, null),
			new MethodInfo(MethodName.DuplicateAnimation, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceAnimationDirect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "library", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AnimationLibrary"), exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindKeyAtTime, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false),
				new PropertyInfo(Variant.Type.Int, "track", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPreviewUnsafeTrack, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSupportedEditableVariant, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTrackTypeLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRotationTrack, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "track", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsAngleInterpolation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "interpolation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetInterpolationLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "interpolation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetUpdateModeLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "updateMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeTransition, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetVariantTypeLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVariant, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadStateString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadStateDouble, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadStateBool, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeSection, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subtitle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeStandaloneSection, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subtitle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Labeled, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.MakeOption, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeIconBadge, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeHintLabel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeSpin, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "min", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "max", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "suffix", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakePanelStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "background", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "border", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "radius", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindEditor && args.Count == 1)
		{
			BindEditor(VariantUtils.ConvertTo<XW2DSceneEditor>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindScene && args.Count == 2)
		{
			BindScene(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CaptureState());
			return true;
		}
		if (method == MethodName.RefreshSelection && args.Count == 0)
		{
			RefreshSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.SuspendForSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(SuspendForSave());
			return true;
		}
		if (method == MethodName.ResumeAfterSave && args.Count == 0)
		{
			ResumeAfterSave();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFromSceneHistory && args.Count == 0)
		{
			RefreshFromSceneHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearSurface && args.Count == 0)
		{
			ClearSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateAnimation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CreateAnimation(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectAnimation && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(SelectAnimation(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.SelectKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SelectKey(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.SeekPreview && args.Count == 1)
		{
			SeekPreview(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayPreview && args.Count == 0)
		{
			PlayPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.PausePreview && args.Count == 0)
		{
			PausePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.AddValueTrack && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(AddValueTrack());
			return true;
		}
		if (method == MethodName.AddValueTrack && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AddValueTrack(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AddValueTrack && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(AddValueTrack(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSelectedTrackInterpolation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetSelectedTrackInterpolation(VariantUtils.ConvertTo<Animation.InterpolationType>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSelectedValueUpdateMode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetSelectedValueUpdateMode(VariantUtils.ConvertTo<Animation.UpdateMode>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSelectedTrackInterpolationLoopWrap && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetSelectedTrackInterpolationLoopWrap(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSelectedKeyTransition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetSelectedKeyTransition(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.InsertCurrentKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(InsertCurrentKey());
			return true;
		}
		if (method == MethodName.InsertCurrentKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(InsertCurrentKey(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.MoveSelectedKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MoveSelectedKey(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.DeleteSelectedKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(DeleteSelectedKey());
			return true;
		}
		if (method == MethodName.BuildInterface && args.Count == 0)
		{
			BuildInterface();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshWorkbenchLayout && args.Count == 0)
		{
			RefreshWorkbenchLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildMotionCurveCard && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(BuildMotionCurveCard());
			return true;
		}
		if (method == MethodName.AddEasePreset && args.Count == 4)
		{
			AddEasePreset(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTrackInterpolationSelected && args.Count == 1)
		{
			OnTrackInterpolationSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnValueUpdateModeSelected && args.Count == 1)
		{
			OnValueUpdateModeSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnEaseTransitionPreviewChanged && args.Count == 1)
		{
			OnEaseTransitionPreviewChanged(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnEaseTransitionCommitRequested && args.Count == 2)
		{
			OnEaseTransitionCommitRequested(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginTransitionSpinEdit && args.Count == 0)
		{
			BeginTransitionSpinEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTransitionSpinValueChanged && args.Count == 1)
		{
			OnTransitionSpinValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitTransitionSpinEdit && args.Count == 0)
		{
			CommitTransitionSpinEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.CommitPendingTransitionInput && args.Count == 0)
		{
			CommitPendingTransitionInput();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMotionCurveEditor && args.Count == 0)
		{
			RefreshMotionCurveEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.StyleMotionSegments && args.Count == 1)
		{
			StyleMotionSegments(VariantUtils.ConvertTo<HFlowContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MakeSmallHeading && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(MakeSmallHeading(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectOptionById && args.Count == 2)
		{
			SelectOptionById(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadClassIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadClassIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildDialogs && args.Count == 0)
		{
			BuildDialogs();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSources && args.Count == 0)
		{
			RefreshSources();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshLibraries && args.Count == 0)
		{
			RefreshLibraries();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAnimations && args.Count == 1)
		{
			RefreshAnimations(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindAnimation && args.Count == 1)
		{
			BindAnimation(VariantUtils.ConvertTo<Animation>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindSafePreview && args.Count == 0)
		{
			BindSafePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshTrackList && args.Count == 0)
		{
			RefreshTrackList();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshTrackSelection && args.Count == 0)
		{
			RefreshTrackSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshValueEditor && args.Count == 0)
		{
			RefreshValueEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateVariantEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateVariantEditor(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSelectedKeyValue && args.Count == 1)
		{
			SetSelectedKeyValue(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyAnimationSnapshot && args.Count == 5)
		{
			ApplyAnimationSnapshot(VariantUtils.ConvertTo<AnimationLibrary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Animation>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitAnimationSlot && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitAnimationSlot(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<AnimationLibrary>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Animation>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<string>(in args[5])));
			return true;
		}
		if (method == MethodName.SetAnimationSlot && args.Count == 5)
		{
			SetAnimationSlot(VariantUtils.ConvertTo<AnimationLibrary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Animation>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicateCurrentAnimation && args.Count == 0)
		{
			DuplicateCurrentAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteCurrentAnimation && args.Count == 0)
		{
			DeleteCurrentAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName.RenameCurrentAnimation && args.Count == 1)
		{
			RenameCurrentAnimation(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenameAnimationSlot && args.Count == 3)
		{
			RenameAnimationSlot(VariantUtils.ConvertTo<AnimationLibrary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CopyCurrentAnimationToScene && args.Count == 0)
		{
			CopyCurrentAnimationToScene();
			ret = default;
			return true;
		}
		if (method == MethodName.SetLocalAnimationCopy && args.Count == 6)
		{
			SetLocalAnimationCopy(VariantUtils.ConvertTo<AnimationPlayer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Animation>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteSelectedTrack && args.Count == 0)
		{
			DeleteSelectedTrack();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedTrack && args.Count == 1)
		{
			MoveSelectedTrack(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleSelectedTrack && args.Count == 1)
		{
			ToggleSelectedTrack(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicateSelectedKey && args.Count == 0)
		{
			DuplicateSelectedKey();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTimelinePlayheadChanged && args.Count == 1)
		{
			OnTimelinePlayheadChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTimelineKeySelected && args.Count == 2)
		{
			OnTimelineKeySelected(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTimelineKeyMoveRequested && args.Count == 3)
		{
			OnTimelineKeyMoveRequested(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTrackListSelected && args.Count == 1)
		{
			OnTrackListSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TogglePlayback && args.Count == 0)
		{
			TogglePlayback();
			ret = default;
			return true;
		}
		if (method == MethodName.StopPreview && args.Count == 0)
		{
			StopPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.PauseForMutation && args.Count == 0)
		{
			PauseForMutation();
			ret = default;
			return true;
		}
		if (method == MethodName.JumpToKey && args.Count == 1)
		{
			JumpToKey(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnLoopToggled && args.Count == 1)
		{
			OnLoopToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSpeedChanged && args.Count == 1)
		{
			OnSpeedChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnLengthChanged && args.Count == 1)
		{
			OnLengthChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnStepChanged && args.Count == 1)
		{
			OnStepChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPlayerSelected && args.Count == 1)
		{
			OnPlayerSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnLibrarySelected && args.Count == 1)
		{
			OnLibrarySelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationSelected && args.Count == 1)
		{
			OnAnimationSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingState && args.Count == 0)
		{
			ApplyPendingState();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSourceInformation && args.Count == 0)
		{
			UpdateSourceInformation();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateAnimationTreeLink && args.Count == 0)
		{
			UpdateAnimationTreeLink();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateEditingState && args.Count == 0)
		{
			UpdateEditingState();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePlaybackButton && args.Count == 0)
		{
			UpdatePlaybackButton();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateProcessing && args.Count == 0)
		{
			UpdateProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateTimeLabel && args.Count == 1)
		{
			UpdateTimeLabel(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureEditable && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureEditable(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCurrentAnimationReadOnly && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCurrentAnimationReadOnly());
			return true;
		}
		if (method == MethodName.IsLibraryExternal && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLibraryExternal(VariantUtils.ConvertTo<AnimationLibrary>(in args[0])));
			return true;
		}
		if (method == MethodName.IsResourceExternal && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsResourceExternal(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.IsEditableValueTrack && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEditableValueTrack(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ClampTime && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ClampTime(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.GetStep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetStep());
			return true;
		}
		if (method == MethodName.GetSceneHistoryId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetSceneHistoryId());
			return true;
		}
		if (method == MethodName.GetSelectedLibraryName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetSelectedLibraryName());
			return true;
		}
		if (method == MethodName.GetSelectedAnimationName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetSelectedAnimationName());
			return true;
		}
		if (method == MethodName.BuildUniqueAnimationName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildUniqueAnimationName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildUniqueAnimationNameAcrossLibrary && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildUniqueAnimationNameAcrossLibrary(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindWritableLibraryName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(FindWritableLibraryName());
			return true;
		}
		if (method == MethodName.OpenNameDialog && args.Count == 3)
		{
			OpenNameDialog(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnNameDialogConfirmed && args.Count == 0)
		{
			OnNameDialogConfirmed();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfirmDeleteAnimation && args.Count == 0)
		{
			ConfirmDeleteAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName.Fail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Fail(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetStatus && args.Count == 2)
		{
			SetStatus(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddPropertyChoices && args.Count == 1)
		{
			AddPropertyChoices(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsNodeInsideScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNodeInsideScene(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveAnimationRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(ResolveAnimationRoot(VariantUtils.ConvertTo<AnimationPlayer>(in args[0])));
			return true;
		}
		if (method == MethodName.DuplicateAnimation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Animation>(DuplicateAnimation(VariantUtils.ConvertTo<Animation>(in args[0])));
			return true;
		}
		if (method == MethodName.ReplaceAnimationDirect && args.Count == 3)
		{
			ReplaceAnimationDirect(VariantUtils.ConvertTo<AnimationLibrary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Animation>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindKeyAtTime && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<int>(FindKeyAtTime(VariantUtils.ConvertTo<Animation>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.IsPreviewUnsafeTrack && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPreviewUnsafeTrack(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSupportedEditableVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSupportedEditableVariant(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTrackTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetTrackTypeLabel(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.IsRotationTrack && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRotationTrack(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAngleInterpolation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAngleInterpolation(VariantUtils.ConvertTo<Animation.InterpolationType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetInterpolationLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetInterpolationLabel(VariantUtils.ConvertTo<Animation.InterpolationType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetUpdateModeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetUpdateModeLabel(VariantUtils.ConvertTo<Animation.UpdateMode>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeTransition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(NormalizeTransition(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.GetVariantTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetVariantTypeLabel(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadStateString && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadStateString(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadStateDouble && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadStateDouble(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadStateBool && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadStateBool(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.MakeSection && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeSection(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.MakeStandaloneSection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeStandaloneSection(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.Labeled && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(Labeled(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeOption && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<OptionButton>(MakeOption(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MakeButton && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(MakeButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeIconBadge && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Label>(MakeIconBadge(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeHintLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(MakeHintLabel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MakeSpin && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(MakeSpin(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<string>(in args[4])));
			return true;
		}
		if (method == MethodName.MakePanelStyle && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(MakePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.StyleMotionSegments && args.Count == 1)
		{
			StyleMotionSegments(VariantUtils.ConvertTo<HFlowContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MakeSmallHeading && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(MakeSmallHeading(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectOptionById && args.Count == 2)
		{
			SelectOptionById(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadClassIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadClassIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveAnimationRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(ResolveAnimationRoot(VariantUtils.ConvertTo<AnimationPlayer>(in args[0])));
			return true;
		}
		if (method == MethodName.DuplicateAnimation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Animation>(DuplicateAnimation(VariantUtils.ConvertTo<Animation>(in args[0])));
			return true;
		}
		if (method == MethodName.ReplaceAnimationDirect && args.Count == 3)
		{
			ReplaceAnimationDirect(VariantUtils.ConvertTo<AnimationLibrary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Animation>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindKeyAtTime && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<int>(FindKeyAtTime(VariantUtils.ConvertTo<Animation>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.IsPreviewUnsafeTrack && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPreviewUnsafeTrack(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSupportedEditableVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSupportedEditableVariant(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTrackTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetTrackTypeLabel(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAngleInterpolation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAngleInterpolation(VariantUtils.ConvertTo<Animation.InterpolationType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetInterpolationLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetInterpolationLabel(VariantUtils.ConvertTo<Animation.InterpolationType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetUpdateModeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetUpdateModeLabel(VariantUtils.ConvertTo<Animation.UpdateMode>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeTransition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(NormalizeTransition(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.GetVariantTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetVariantTypeLabel(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadStateString && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadStateString(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadStateDouble && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadStateDouble(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadStateBool && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadStateBool(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.MakeSection && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeSection(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.MakeStandaloneSection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeStandaloneSection(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.Labeled && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(Labeled(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeOption && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<OptionButton>(MakeOption(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MakeButton && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(MakeButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeIconBadge && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Label>(MakeIconBadge(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeHintLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(MakeHintLabel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MakeSpin && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(MakeSpin(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<string>(in args[4])));
			return true;
		}
		if (method == MethodName.MakePanelStyle && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(MakePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.BindEditor)
		{
			return true;
		}
		if (method == MethodName.BindScene)
		{
			return true;
		}
		if (method == MethodName.CaptureState)
		{
			return true;
		}
		if (method == MethodName.RefreshSelection)
		{
			return true;
		}
		if (method == MethodName.SuspendForSave)
		{
			return true;
		}
		if (method == MethodName.ResumeAfterSave)
		{
			return true;
		}
		if (method == MethodName.RefreshFromSceneHistory)
		{
			return true;
		}
		if (method == MethodName.ClearSurface)
		{
			return true;
		}
		if (method == MethodName.CreateAnimation)
		{
			return true;
		}
		if (method == MethodName.SelectAnimation)
		{
			return true;
		}
		if (method == MethodName.SelectKey)
		{
			return true;
		}
		if (method == MethodName.SeekPreview)
		{
			return true;
		}
		if (method == MethodName.PlayPreview)
		{
			return true;
		}
		if (method == MethodName.PausePreview)
		{
			return true;
		}
		if (method == MethodName.AddValueTrack)
		{
			return true;
		}
		if (method == MethodName.SetSelectedTrackInterpolation)
		{
			return true;
		}
		if (method == MethodName.SetSelectedValueUpdateMode)
		{
			return true;
		}
		if (method == MethodName.SetSelectedTrackInterpolationLoopWrap)
		{
			return true;
		}
		if (method == MethodName.SetSelectedKeyTransition)
		{
			return true;
		}
		if (method == MethodName.InsertCurrentKey)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedKey)
		{
			return true;
		}
		if (method == MethodName.DeleteSelectedKey)
		{
			return true;
		}
		if (method == MethodName.BuildInterface)
		{
			return true;
		}
		if (method == MethodName.RefreshWorkbenchLayout)
		{
			return true;
		}
		if (method == MethodName.BuildMotionCurveCard)
		{
			return true;
		}
		if (method == MethodName.AddEasePreset)
		{
			return true;
		}
		if (method == MethodName.OnTrackInterpolationSelected)
		{
			return true;
		}
		if (method == MethodName.OnValueUpdateModeSelected)
		{
			return true;
		}
		if (method == MethodName.OnEaseTransitionPreviewChanged)
		{
			return true;
		}
		if (method == MethodName.OnEaseTransitionCommitRequested)
		{
			return true;
		}
		if (method == MethodName.BeginTransitionSpinEdit)
		{
			return true;
		}
		if (method == MethodName.OnTransitionSpinValueChanged)
		{
			return true;
		}
		if (method == MethodName.CommitTransitionSpinEdit)
		{
			return true;
		}
		if (method == MethodName.CommitPendingTransitionInput)
		{
			return true;
		}
		if (method == MethodName.RefreshMotionCurveEditor)
		{
			return true;
		}
		if (method == MethodName.StyleMotionSegments)
		{
			return true;
		}
		if (method == MethodName.MakeSmallHeading)
		{
			return true;
		}
		if (method == MethodName.SelectOptionById)
		{
			return true;
		}
		if (method == MethodName.LoadClassIcon)
		{
			return true;
		}
		if (method == MethodName.BuildDialogs)
		{
			return true;
		}
		if (method == MethodName.RefreshSources)
		{
			return true;
		}
		if (method == MethodName.RefreshLibraries)
		{
			return true;
		}
		if (method == MethodName.RefreshAnimations)
		{
			return true;
		}
		if (method == MethodName.BindAnimation)
		{
			return true;
		}
		if (method == MethodName.BindSafePreview)
		{
			return true;
		}
		if (method == MethodName.RefreshTrackList)
		{
			return true;
		}
		if (method == MethodName.RefreshTrackSelection)
		{
			return true;
		}
		if (method == MethodName.RefreshValueEditor)
		{
			return true;
		}
		if (method == MethodName.CreateVariantEditor)
		{
			return true;
		}
		if (method == MethodName.SetSelectedKeyValue)
		{
			return true;
		}
		if (method == MethodName.ApplyAnimationSnapshot)
		{
			return true;
		}
		if (method == MethodName.CommitAnimationSlot)
		{
			return true;
		}
		if (method == MethodName.SetAnimationSlot)
		{
			return true;
		}
		if (method == MethodName.DuplicateCurrentAnimation)
		{
			return true;
		}
		if (method == MethodName.DeleteCurrentAnimation)
		{
			return true;
		}
		if (method == MethodName.RenameCurrentAnimation)
		{
			return true;
		}
		if (method == MethodName.RenameAnimationSlot)
		{
			return true;
		}
		if (method == MethodName.CopyCurrentAnimationToScene)
		{
			return true;
		}
		if (method == MethodName.SetLocalAnimationCopy)
		{
			return true;
		}
		if (method == MethodName.DeleteSelectedTrack)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedTrack)
		{
			return true;
		}
		if (method == MethodName.ToggleSelectedTrack)
		{
			return true;
		}
		if (method == MethodName.DuplicateSelectedKey)
		{
			return true;
		}
		if (method == MethodName.OnTimelinePlayheadChanged)
		{
			return true;
		}
		if (method == MethodName.OnTimelineKeySelected)
		{
			return true;
		}
		if (method == MethodName.OnTimelineKeyMoveRequested)
		{
			return true;
		}
		if (method == MethodName.OnTrackListSelected)
		{
			return true;
		}
		if (method == MethodName.TogglePlayback)
		{
			return true;
		}
		if (method == MethodName.StopPreview)
		{
			return true;
		}
		if (method == MethodName.PauseForMutation)
		{
			return true;
		}
		if (method == MethodName.JumpToKey)
		{
			return true;
		}
		if (method == MethodName.OnLoopToggled)
		{
			return true;
		}
		if (method == MethodName.OnSpeedChanged)
		{
			return true;
		}
		if (method == MethodName.OnLengthChanged)
		{
			return true;
		}
		if (method == MethodName.OnStepChanged)
		{
			return true;
		}
		if (method == MethodName.OnPlayerSelected)
		{
			return true;
		}
		if (method == MethodName.OnLibrarySelected)
		{
			return true;
		}
		if (method == MethodName.OnAnimationSelected)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingState)
		{
			return true;
		}
		if (method == MethodName.UpdateSourceInformation)
		{
			return true;
		}
		if (method == MethodName.UpdateAnimationTreeLink)
		{
			return true;
		}
		if (method == MethodName.UpdateEditingState)
		{
			return true;
		}
		if (method == MethodName.UpdatePlaybackButton)
		{
			return true;
		}
		if (method == MethodName.UpdateProcessing)
		{
			return true;
		}
		if (method == MethodName.UpdateTimeLabel)
		{
			return true;
		}
		if (method == MethodName.EnsureEditable)
		{
			return true;
		}
		if (method == MethodName.IsCurrentAnimationReadOnly)
		{
			return true;
		}
		if (method == MethodName.IsLibraryExternal)
		{
			return true;
		}
		if (method == MethodName.IsResourceExternal)
		{
			return true;
		}
		if (method == MethodName.IsEditableValueTrack)
		{
			return true;
		}
		if (method == MethodName.ClampTime)
		{
			return true;
		}
		if (method == MethodName.GetStep)
		{
			return true;
		}
		if (method == MethodName.GetSceneHistoryId)
		{
			return true;
		}
		if (method == MethodName.GetSelectedLibraryName)
		{
			return true;
		}
		if (method == MethodName.GetSelectedAnimationName)
		{
			return true;
		}
		if (method == MethodName.BuildUniqueAnimationName)
		{
			return true;
		}
		if (method == MethodName.BuildUniqueAnimationNameAcrossLibrary)
		{
			return true;
		}
		if (method == MethodName.FindWritableLibraryName)
		{
			return true;
		}
		if (method == MethodName.OpenNameDialog)
		{
			return true;
		}
		if (method == MethodName.OnNameDialogConfirmed)
		{
			return true;
		}
		if (method == MethodName.ConfirmDeleteAnimation)
		{
			return true;
		}
		if (method == MethodName.Fail)
		{
			return true;
		}
		if (method == MethodName.SetStatus)
		{
			return true;
		}
		if (method == MethodName.AddPropertyChoices)
		{
			return true;
		}
		if (method == MethodName.IsNodeInsideScene)
		{
			return true;
		}
		if (method == MethodName.ResolveAnimationRoot)
		{
			return true;
		}
		if (method == MethodName.DuplicateAnimation)
		{
			return true;
		}
		if (method == MethodName.ReplaceAnimationDirect)
		{
			return true;
		}
		if (method == MethodName.FindKeyAtTime)
		{
			return true;
		}
		if (method == MethodName.IsPreviewUnsafeTrack)
		{
			return true;
		}
		if (method == MethodName.IsSupportedEditableVariant)
		{
			return true;
		}
		if (method == MethodName.GetTrackTypeLabel)
		{
			return true;
		}
		if (method == MethodName.IsRotationTrack)
		{
			return true;
		}
		if (method == MethodName.IsAngleInterpolation)
		{
			return true;
		}
		if (method == MethodName.GetInterpolationLabel)
		{
			return true;
		}
		if (method == MethodName.GetUpdateModeLabel)
		{
			return true;
		}
		if (method == MethodName.NormalizeTransition)
		{
			return true;
		}
		if (method == MethodName.GetVariantTypeLabel)
		{
			return true;
		}
		if (method == MethodName.FormatVariant)
		{
			return true;
		}
		if (method == MethodName.ReadStateString)
		{
			return true;
		}
		if (method == MethodName.ReadStateDouble)
		{
			return true;
		}
		if (method == MethodName.ReadStateBool)
		{
			return true;
		}
		if (method == MethodName.MakeSection)
		{
			return true;
		}
		if (method == MethodName.MakeStandaloneSection)
		{
			return true;
		}
		if (method == MethodName.Labeled)
		{
			return true;
		}
		if (method == MethodName.MakeOption)
		{
			return true;
		}
		if (method == MethodName.MakeButton)
		{
			return true;
		}
		if (method == MethodName.MakeIconBadge)
		{
			return true;
		}
		if (method == MethodName.MakeHintLabel)
		{
			return true;
		}
		if (method == MethodName.MakeSpin)
		{
			return true;
		}
		if (method == MethodName.MakePanelStyle)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XW2DSceneEditor>(in value);
			return true;
		}
		if (name == PropertyName._sceneRoot)
		{
			_sceneRoot = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._player)
		{
			_player = VariantUtils.ConvertTo<AnimationPlayer>(in value);
			return true;
		}
		if (name == PropertyName._library)
		{
			_library = VariantUtils.ConvertTo<AnimationLibrary>(in value);
			return true;
		}
		if (name == PropertyName._animation)
		{
			_animation = VariantUtils.ConvertTo<Animation>(in value);
			return true;
		}
		if (name == PropertyName._timeline)
		{
			_timeline = VariantUtils.ConvertTo<XW2DAnimationTimelineCanvas>(in value);
			return true;
		}
		if (name == PropertyName._preview)
		{
			_preview = VariantUtils.ConvertTo<XW2DAnimationSafePreview>(in value);
			return true;
		}
		if (name == PropertyName._playerOption)
		{
			_playerOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._libraryOption)
		{
			_libraryOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._animationOption)
		{
			_animationOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._treeLinkLabel)
		{
			_treeLinkLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._readonlyLabel)
		{
			_readonlyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewSafetyLabel)
		{
			_previewSafetyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._copyLocalButton)
		{
			_copyLocalButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._playPauseButton)
		{
			_playPauseButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._loopButton)
		{
			_loopButton = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._speedSpin)
		{
			_speedSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._lengthSpin)
		{
			_lengthSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._stepSpin)
		{
			_stepSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._zoomSpin)
		{
			_zoomSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._timeLabel)
		{
			_timeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._propertyPalette)
		{
			_propertyPalette = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._trackList)
		{
			_trackList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._selectedTrackLabel)
		{
			_selectedTrackLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._trackEnabledCheck)
		{
			_trackEnabledCheck = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._deleteTrackButton)
		{
			_deleteTrackButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveTrackUpButton)
		{
			_moveTrackUpButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveTrackDownButton)
		{
			_moveTrackDownButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._insertKeyButton)
		{
			_insertKeyButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._deleteKeyButton)
		{
			_deleteKeyButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._duplicateKeyButton)
		{
			_duplicateKeyButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveKeyEarlierButton)
		{
			_moveKeyEarlierButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveKeyLaterButton)
		{
			_moveKeyLaterButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._selectedKeyLabel)
		{
			_selectedKeyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._valueEditorHost)
		{
			_valueEditorHost = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._trackInterpolationOption)
		{
			_trackInterpolationOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._valueUpdateModeOption)
		{
			_valueUpdateModeOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._interpolationLoopWrapCheck)
		{
			_interpolationLoopWrapCheck = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._trackInterpolationVisualHost)
		{
			_trackInterpolationVisualHost = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._valueUpdateModeVisualHost)
		{
			_valueUpdateModeVisualHost = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._easePresetShelf)
		{
			_easePresetShelf = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._easeCurveCanvas)
		{
			_easeCurveCanvas = VariantUtils.ConvertTo<XW2DAnimationEaseCurveCanvas>(in value);
			return true;
		}
		if (name == PropertyName._keyTransitionSpin)
		{
			_keyTransitionSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._motionCurveStatusLabel)
		{
			_motionCurveStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._workbenchScroll)
		{
			_workbenchScroll = VariantUtils.ConvertTo<ScrollContainer>(in value);
			return true;
		}
		if (name == PropertyName._workbenchRoot)
		{
			_workbenchRoot = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._deleteAnimationDialog)
		{
			_deleteAnimationDialog = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._nameDialog)
		{
			_nameDialog = VariantUtils.ConvertTo<AcceptDialog>(in value);
			return true;
		}
		if (name == PropertyName._nameEdit)
		{
			_nameEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._updating)
		{
			_updating = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._suspendedForSave)
		{
			_suspendedForSave = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._resumePlayingAfterSave)
		{
			_resumePlayingAfterSave = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._resumePositionAfterSave)
		{
			_resumePositionAfterSave = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._transitionSpinEditing)
		{
			_transitionSpinEditing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._transitionSpinPending)
		{
			_transitionSpinPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._transitionSpinStart)
		{
			_transitionSpinStart = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._selectedTrack)
		{
			_selectedTrack = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedKeyTime)
		{
			_selectedKeyTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pendingNameOperation)
		{
			_pendingNameOperation = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pendingBindState)
		{
			_pendingBindState = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.CurrentPlayer)
		{
			value = VariantUtils.CreateFrom<AnimationPlayer>(CurrentPlayer);
			return true;
		}
		if (name == PropertyName.CurrentLibrary)
		{
			value = VariantUtils.CreateFrom<AnimationLibrary>(CurrentLibrary);
			return true;
		}
		if (name == PropertyName.CurrentAnimation)
		{
			value = VariantUtils.CreateFrom<Animation>(CurrentAnimation);
			return true;
		}
		string from;
		if (name == PropertyName.CurrentAnimationName)
		{
			from = CurrentAnimationName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CurrentLibraryName)
		{
			from = CurrentLibraryName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.SelectedTrack)
		{
			from2 = SelectedTrack;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		double from3;
		if (name == PropertyName.SelectedKeyTime)
		{
			from3 = SelectedKeyTime;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.TrackCount)
		{
			from2 = TrackCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SelectedTrackKeyCount)
		{
			from2 = SelectedTrackKeyCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CurrentPlayhead)
		{
			from3 = CurrentPlayhead;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.PreviewUnsafeTrackCount)
		{
			from2 = PreviewUnsafeTrackCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PreviewBaselineCount)
		{
			from2 = PreviewBaselineCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		bool from4;
		if (name == PropertyName.IsPreviewPlaying)
		{
			from4 = IsPreviewPlaying;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.PreviewPosition)
		{
			from3 = PreviewPosition;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.IsReadOnly)
		{
			from4 = IsReadOnly;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.EaseCurveCanvas)
		{
			value = VariantUtils.CreateFrom<XW2DAnimationEaseCurveCanvas>(EaseCurveCanvas);
			return true;
		}
		if (name == PropertyName.SelectedTrackInterpolation)
		{
			value = VariantUtils.CreateFrom<Animation.InterpolationType>(SelectedTrackInterpolation);
			return true;
		}
		if (name == PropertyName.SelectedValueUpdateMode)
		{
			value = VariantUtils.CreateFrom<Animation.UpdateMode>(SelectedValueUpdateMode);
			return true;
		}
		if (name == PropertyName.SelectedTrackInterpolationLoopWrap)
		{
			from4 = SelectedTrackInterpolationLoopWrap;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.SelectedKeyTransition)
		{
			value = VariantUtils.CreateFrom<float>(SelectedKeyTransition);
			return true;
		}
		if (name == PropertyName._editor)
		{
			value = VariantUtils.CreateFrom(in _editor);
			return true;
		}
		if (name == PropertyName._sceneRoot)
		{
			value = VariantUtils.CreateFrom(in _sceneRoot);
			return true;
		}
		if (name == PropertyName._player)
		{
			value = VariantUtils.CreateFrom(in _player);
			return true;
		}
		if (name == PropertyName._library)
		{
			value = VariantUtils.CreateFrom(in _library);
			return true;
		}
		if (name == PropertyName._animation)
		{
			value = VariantUtils.CreateFrom(in _animation);
			return true;
		}
		if (name == PropertyName._timeline)
		{
			value = VariantUtils.CreateFrom(in _timeline);
			return true;
		}
		if (name == PropertyName._preview)
		{
			value = VariantUtils.CreateFrom(in _preview);
			return true;
		}
		if (name == PropertyName._playerOption)
		{
			value = VariantUtils.CreateFrom(in _playerOption);
			return true;
		}
		if (name == PropertyName._libraryOption)
		{
			value = VariantUtils.CreateFrom(in _libraryOption);
			return true;
		}
		if (name == PropertyName._animationOption)
		{
			value = VariantUtils.CreateFrom(in _animationOption);
			return true;
		}
		if (name == PropertyName._treeLinkLabel)
		{
			value = VariantUtils.CreateFrom(in _treeLinkLabel);
			return true;
		}
		if (name == PropertyName._readonlyLabel)
		{
			value = VariantUtils.CreateFrom(in _readonlyLabel);
			return true;
		}
		if (name == PropertyName._previewSafetyLabel)
		{
			value = VariantUtils.CreateFrom(in _previewSafetyLabel);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._copyLocalButton)
		{
			value = VariantUtils.CreateFrom(in _copyLocalButton);
			return true;
		}
		if (name == PropertyName._playPauseButton)
		{
			value = VariantUtils.CreateFrom(in _playPauseButton);
			return true;
		}
		if (name == PropertyName._loopButton)
		{
			value = VariantUtils.CreateFrom(in _loopButton);
			return true;
		}
		if (name == PropertyName._speedSpin)
		{
			value = VariantUtils.CreateFrom(in _speedSpin);
			return true;
		}
		if (name == PropertyName._lengthSpin)
		{
			value = VariantUtils.CreateFrom(in _lengthSpin);
			return true;
		}
		if (name == PropertyName._stepSpin)
		{
			value = VariantUtils.CreateFrom(in _stepSpin);
			return true;
		}
		if (name == PropertyName._zoomSpin)
		{
			value = VariantUtils.CreateFrom(in _zoomSpin);
			return true;
		}
		if (name == PropertyName._timeLabel)
		{
			value = VariantUtils.CreateFrom(in _timeLabel);
			return true;
		}
		if (name == PropertyName._propertyPalette)
		{
			value = VariantUtils.CreateFrom(in _propertyPalette);
			return true;
		}
		if (name == PropertyName._trackList)
		{
			value = VariantUtils.CreateFrom(in _trackList);
			return true;
		}
		if (name == PropertyName._selectedTrackLabel)
		{
			value = VariantUtils.CreateFrom(in _selectedTrackLabel);
			return true;
		}
		if (name == PropertyName._trackEnabledCheck)
		{
			value = VariantUtils.CreateFrom(in _trackEnabledCheck);
			return true;
		}
		if (name == PropertyName._deleteTrackButton)
		{
			value = VariantUtils.CreateFrom(in _deleteTrackButton);
			return true;
		}
		if (name == PropertyName._moveTrackUpButton)
		{
			value = VariantUtils.CreateFrom(in _moveTrackUpButton);
			return true;
		}
		if (name == PropertyName._moveTrackDownButton)
		{
			value = VariantUtils.CreateFrom(in _moveTrackDownButton);
			return true;
		}
		if (name == PropertyName._insertKeyButton)
		{
			value = VariantUtils.CreateFrom(in _insertKeyButton);
			return true;
		}
		if (name == PropertyName._deleteKeyButton)
		{
			value = VariantUtils.CreateFrom(in _deleteKeyButton);
			return true;
		}
		if (name == PropertyName._duplicateKeyButton)
		{
			value = VariantUtils.CreateFrom(in _duplicateKeyButton);
			return true;
		}
		if (name == PropertyName._moveKeyEarlierButton)
		{
			value = VariantUtils.CreateFrom(in _moveKeyEarlierButton);
			return true;
		}
		if (name == PropertyName._moveKeyLaterButton)
		{
			value = VariantUtils.CreateFrom(in _moveKeyLaterButton);
			return true;
		}
		if (name == PropertyName._selectedKeyLabel)
		{
			value = VariantUtils.CreateFrom(in _selectedKeyLabel);
			return true;
		}
		if (name == PropertyName._valueEditorHost)
		{
			value = VariantUtils.CreateFrom(in _valueEditorHost);
			return true;
		}
		if (name == PropertyName._trackInterpolationOption)
		{
			value = VariantUtils.CreateFrom(in _trackInterpolationOption);
			return true;
		}
		if (name == PropertyName._valueUpdateModeOption)
		{
			value = VariantUtils.CreateFrom(in _valueUpdateModeOption);
			return true;
		}
		if (name == PropertyName._interpolationLoopWrapCheck)
		{
			value = VariantUtils.CreateFrom(in _interpolationLoopWrapCheck);
			return true;
		}
		if (name == PropertyName._trackInterpolationVisualHost)
		{
			value = VariantUtils.CreateFrom(in _trackInterpolationVisualHost);
			return true;
		}
		if (name == PropertyName._valueUpdateModeVisualHost)
		{
			value = VariantUtils.CreateFrom(in _valueUpdateModeVisualHost);
			return true;
		}
		if (name == PropertyName._easePresetShelf)
		{
			value = VariantUtils.CreateFrom(in _easePresetShelf);
			return true;
		}
		if (name == PropertyName._easeCurveCanvas)
		{
			value = VariantUtils.CreateFrom(in _easeCurveCanvas);
			return true;
		}
		if (name == PropertyName._keyTransitionSpin)
		{
			value = VariantUtils.CreateFrom(in _keyTransitionSpin);
			return true;
		}
		if (name == PropertyName._motionCurveStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _motionCurveStatusLabel);
			return true;
		}
		if (name == PropertyName._workbenchScroll)
		{
			value = VariantUtils.CreateFrom(in _workbenchScroll);
			return true;
		}
		if (name == PropertyName._workbenchRoot)
		{
			value = VariantUtils.CreateFrom(in _workbenchRoot);
			return true;
		}
		if (name == PropertyName._deleteAnimationDialog)
		{
			value = VariantUtils.CreateFrom(in _deleteAnimationDialog);
			return true;
		}
		if (name == PropertyName._nameDialog)
		{
			value = VariantUtils.CreateFrom(in _nameDialog);
			return true;
		}
		if (name == PropertyName._nameEdit)
		{
			value = VariantUtils.CreateFrom(in _nameEdit);
			return true;
		}
		if (name == PropertyName._updating)
		{
			value = VariantUtils.CreateFrom(in _updating);
			return true;
		}
		if (name == PropertyName._suspendedForSave)
		{
			value = VariantUtils.CreateFrom(in _suspendedForSave);
			return true;
		}
		if (name == PropertyName._resumePlayingAfterSave)
		{
			value = VariantUtils.CreateFrom(in _resumePlayingAfterSave);
			return true;
		}
		if (name == PropertyName._resumePositionAfterSave)
		{
			value = VariantUtils.CreateFrom(in _resumePositionAfterSave);
			return true;
		}
		if (name == PropertyName._transitionSpinEditing)
		{
			value = VariantUtils.CreateFrom(in _transitionSpinEditing);
			return true;
		}
		if (name == PropertyName._transitionSpinPending)
		{
			value = VariantUtils.CreateFrom(in _transitionSpinPending);
			return true;
		}
		if (name == PropertyName._transitionSpinStart)
		{
			value = VariantUtils.CreateFrom(in _transitionSpinStart);
			return true;
		}
		if (name == PropertyName._selectedTrack)
		{
			value = VariantUtils.CreateFrom(in _selectedTrack);
			return true;
		}
		if (name == PropertyName._selectedKeyTime)
		{
			value = VariantUtils.CreateFrom(in _selectedKeyTime);
			return true;
		}
		if (name == PropertyName._pendingNameOperation)
		{
			value = VariantUtils.CreateFrom(in _pendingNameOperation);
			return true;
		}
		if (name == PropertyName._pendingBindState)
		{
			value = VariantUtils.CreateFrom(in _pendingBindState);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sceneRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._player, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._library, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._timeline, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._preview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._playerOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._libraryOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._treeLinkLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._readonlyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewSafetyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._copyLocalButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._playPauseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loopButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._speedSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lengthSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stepSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zoomSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._timeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._propertyPalette, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._trackList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedTrackLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._trackEnabledCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._deleteTrackButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveTrackUpButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveTrackDownButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._insertKeyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._deleteKeyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._duplicateKeyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveKeyEarlierButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveKeyLaterButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedKeyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._valueEditorHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._trackInterpolationOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._valueUpdateModeOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._interpolationLoopWrapCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._trackInterpolationVisualHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._valueUpdateModeVisualHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._easePresetShelf, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._easeCurveCanvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._keyTransitionSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._motionCurveStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._workbenchScroll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._workbenchRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._deleteAnimationDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nameDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._suspendedForSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._resumePlayingAfterSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._resumePositionAfterSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._transitionSpinEditing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._transitionSpinPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._transitionSpinStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedTrack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._selectedKeyTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingNameOperation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._pendingBindState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentLibrary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentAnimation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.CurrentAnimationName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.CurrentLibraryName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedTrack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.SelectedKeyTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TrackCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedTrackKeyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.CurrentPlayhead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PreviewUnsafeTrackCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PreviewBaselineCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPreviewPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.PreviewPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsReadOnly, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.EaseCurveCanvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedTrackInterpolation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedValueUpdateMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.SelectedTrackInterpolationLoopWrap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.SelectedKeyTransition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._sceneRoot, Variant.From(in _sceneRoot));
		info.AddProperty(PropertyName._player, Variant.From(in _player));
		info.AddProperty(PropertyName._library, Variant.From(in _library));
		info.AddProperty(PropertyName._animation, Variant.From(in _animation));
		info.AddProperty(PropertyName._timeline, Variant.From(in _timeline));
		info.AddProperty(PropertyName._preview, Variant.From(in _preview));
		info.AddProperty(PropertyName._playerOption, Variant.From(in _playerOption));
		info.AddProperty(PropertyName._libraryOption, Variant.From(in _libraryOption));
		info.AddProperty(PropertyName._animationOption, Variant.From(in _animationOption));
		info.AddProperty(PropertyName._treeLinkLabel, Variant.From(in _treeLinkLabel));
		info.AddProperty(PropertyName._readonlyLabel, Variant.From(in _readonlyLabel));
		info.AddProperty(PropertyName._previewSafetyLabel, Variant.From(in _previewSafetyLabel));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._copyLocalButton, Variant.From(in _copyLocalButton));
		info.AddProperty(PropertyName._playPauseButton, Variant.From(in _playPauseButton));
		info.AddProperty(PropertyName._loopButton, Variant.From(in _loopButton));
		info.AddProperty(PropertyName._speedSpin, Variant.From(in _speedSpin));
		info.AddProperty(PropertyName._lengthSpin, Variant.From(in _lengthSpin));
		info.AddProperty(PropertyName._stepSpin, Variant.From(in _stepSpin));
		info.AddProperty(PropertyName._zoomSpin, Variant.From(in _zoomSpin));
		info.AddProperty(PropertyName._timeLabel, Variant.From(in _timeLabel));
		info.AddProperty(PropertyName._propertyPalette, Variant.From(in _propertyPalette));
		info.AddProperty(PropertyName._trackList, Variant.From(in _trackList));
		info.AddProperty(PropertyName._selectedTrackLabel, Variant.From(in _selectedTrackLabel));
		info.AddProperty(PropertyName._trackEnabledCheck, Variant.From(in _trackEnabledCheck));
		info.AddProperty(PropertyName._deleteTrackButton, Variant.From(in _deleteTrackButton));
		info.AddProperty(PropertyName._moveTrackUpButton, Variant.From(in _moveTrackUpButton));
		info.AddProperty(PropertyName._moveTrackDownButton, Variant.From(in _moveTrackDownButton));
		info.AddProperty(PropertyName._insertKeyButton, Variant.From(in _insertKeyButton));
		info.AddProperty(PropertyName._deleteKeyButton, Variant.From(in _deleteKeyButton));
		info.AddProperty(PropertyName._duplicateKeyButton, Variant.From(in _duplicateKeyButton));
		info.AddProperty(PropertyName._moveKeyEarlierButton, Variant.From(in _moveKeyEarlierButton));
		info.AddProperty(PropertyName._moveKeyLaterButton, Variant.From(in _moveKeyLaterButton));
		info.AddProperty(PropertyName._selectedKeyLabel, Variant.From(in _selectedKeyLabel));
		info.AddProperty(PropertyName._valueEditorHost, Variant.From(in _valueEditorHost));
		info.AddProperty(PropertyName._trackInterpolationOption, Variant.From(in _trackInterpolationOption));
		info.AddProperty(PropertyName._valueUpdateModeOption, Variant.From(in _valueUpdateModeOption));
		info.AddProperty(PropertyName._interpolationLoopWrapCheck, Variant.From(in _interpolationLoopWrapCheck));
		info.AddProperty(PropertyName._trackInterpolationVisualHost, Variant.From(in _trackInterpolationVisualHost));
		info.AddProperty(PropertyName._valueUpdateModeVisualHost, Variant.From(in _valueUpdateModeVisualHost));
		info.AddProperty(PropertyName._easePresetShelf, Variant.From(in _easePresetShelf));
		info.AddProperty(PropertyName._easeCurveCanvas, Variant.From(in _easeCurveCanvas));
		info.AddProperty(PropertyName._keyTransitionSpin, Variant.From(in _keyTransitionSpin));
		info.AddProperty(PropertyName._motionCurveStatusLabel, Variant.From(in _motionCurveStatusLabel));
		info.AddProperty(PropertyName._workbenchScroll, Variant.From(in _workbenchScroll));
		info.AddProperty(PropertyName._workbenchRoot, Variant.From(in _workbenchRoot));
		info.AddProperty(PropertyName._deleteAnimationDialog, Variant.From(in _deleteAnimationDialog));
		info.AddProperty(PropertyName._nameDialog, Variant.From(in _nameDialog));
		info.AddProperty(PropertyName._nameEdit, Variant.From(in _nameEdit));
		info.AddProperty(PropertyName._updating, Variant.From(in _updating));
		info.AddProperty(PropertyName._suspendedForSave, Variant.From(in _suspendedForSave));
		info.AddProperty(PropertyName._resumePlayingAfterSave, Variant.From(in _resumePlayingAfterSave));
		info.AddProperty(PropertyName._resumePositionAfterSave, Variant.From(in _resumePositionAfterSave));
		info.AddProperty(PropertyName._transitionSpinEditing, Variant.From(in _transitionSpinEditing));
		info.AddProperty(PropertyName._transitionSpinPending, Variant.From(in _transitionSpinPending));
		info.AddProperty(PropertyName._transitionSpinStart, Variant.From(in _transitionSpinStart));
		info.AddProperty(PropertyName._selectedTrack, Variant.From(in _selectedTrack));
		info.AddProperty(PropertyName._selectedKeyTime, Variant.From(in _selectedKeyTime));
		info.AddProperty(PropertyName._pendingNameOperation, Variant.From(in _pendingNameOperation));
		info.AddProperty(PropertyName._pendingBindState, Variant.From(in _pendingBindState));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XW2DSceneEditor>();
		}
		if (info.TryGetProperty(PropertyName._sceneRoot, out var value2))
		{
			_sceneRoot = value2.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._player, out var value3))
		{
			_player = value3.As<AnimationPlayer>();
		}
		if (info.TryGetProperty(PropertyName._library, out var value4))
		{
			_library = value4.As<AnimationLibrary>();
		}
		if (info.TryGetProperty(PropertyName._animation, out var value5))
		{
			_animation = value5.As<Animation>();
		}
		if (info.TryGetProperty(PropertyName._timeline, out var value6))
		{
			_timeline = value6.As<XW2DAnimationTimelineCanvas>();
		}
		if (info.TryGetProperty(PropertyName._preview, out var value7))
		{
			_preview = value7.As<XW2DAnimationSafePreview>();
		}
		if (info.TryGetProperty(PropertyName._playerOption, out var value8))
		{
			_playerOption = value8.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._libraryOption, out var value9))
		{
			_libraryOption = value9.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._animationOption, out var value10))
		{
			_animationOption = value10.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._treeLinkLabel, out var value11))
		{
			_treeLinkLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._readonlyLabel, out var value12))
		{
			_readonlyLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewSafetyLabel, out var value13))
		{
			_previewSafetyLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value14))
		{
			_statusLabel = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._copyLocalButton, out var value15))
		{
			_copyLocalButton = value15.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._playPauseButton, out var value16))
		{
			_playPauseButton = value16.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._loopButton, out var value17))
		{
			_loopButton = value17.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._speedSpin, out var value18))
		{
			_speedSpin = value18.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._lengthSpin, out var value19))
		{
			_lengthSpin = value19.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._stepSpin, out var value20))
		{
			_stepSpin = value20.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._zoomSpin, out var value21))
		{
			_zoomSpin = value21.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._timeLabel, out var value22))
		{
			_timeLabel = value22.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._propertyPalette, out var value23))
		{
			_propertyPalette = value23.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._trackList, out var value24))
		{
			_trackList = value24.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._selectedTrackLabel, out var value25))
		{
			_selectedTrackLabel = value25.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._trackEnabledCheck, out var value26))
		{
			_trackEnabledCheck = value26.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._deleteTrackButton, out var value27))
		{
			_deleteTrackButton = value27.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveTrackUpButton, out var value28))
		{
			_moveTrackUpButton = value28.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveTrackDownButton, out var value29))
		{
			_moveTrackDownButton = value29.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._insertKeyButton, out var value30))
		{
			_insertKeyButton = value30.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._deleteKeyButton, out var value31))
		{
			_deleteKeyButton = value31.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._duplicateKeyButton, out var value32))
		{
			_duplicateKeyButton = value32.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveKeyEarlierButton, out var value33))
		{
			_moveKeyEarlierButton = value33.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveKeyLaterButton, out var value34))
		{
			_moveKeyLaterButton = value34.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._selectedKeyLabel, out var value35))
		{
			_selectedKeyLabel = value35.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._valueEditorHost, out var value36))
		{
			_valueEditorHost = value36.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._trackInterpolationOption, out var value37))
		{
			_trackInterpolationOption = value37.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._valueUpdateModeOption, out var value38))
		{
			_valueUpdateModeOption = value38.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._interpolationLoopWrapCheck, out var value39))
		{
			_interpolationLoopWrapCheck = value39.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._trackInterpolationVisualHost, out var value40))
		{
			_trackInterpolationVisualHost = value40.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._valueUpdateModeVisualHost, out var value41))
		{
			_valueUpdateModeVisualHost = value41.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._easePresetShelf, out var value42))
		{
			_easePresetShelf = value42.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._easeCurveCanvas, out var value43))
		{
			_easeCurveCanvas = value43.As<XW2DAnimationEaseCurveCanvas>();
		}
		if (info.TryGetProperty(PropertyName._keyTransitionSpin, out var value44))
		{
			_keyTransitionSpin = value44.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._motionCurveStatusLabel, out var value45))
		{
			_motionCurveStatusLabel = value45.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._workbenchScroll, out var value46))
		{
			_workbenchScroll = value46.As<ScrollContainer>();
		}
		if (info.TryGetProperty(PropertyName._workbenchRoot, out var value47))
		{
			_workbenchRoot = value47.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._deleteAnimationDialog, out var value48))
		{
			_deleteAnimationDialog = value48.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._nameDialog, out var value49))
		{
			_nameDialog = value49.As<AcceptDialog>();
		}
		if (info.TryGetProperty(PropertyName._nameEdit, out var value50))
		{
			_nameEdit = value50.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._updating, out var value51))
		{
			_updating = value51.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._suspendedForSave, out var value52))
		{
			_suspendedForSave = value52.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._resumePlayingAfterSave, out var value53))
		{
			_resumePlayingAfterSave = value53.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._resumePositionAfterSave, out var value54))
		{
			_resumePositionAfterSave = value54.As<double>();
		}
		if (info.TryGetProperty(PropertyName._transitionSpinEditing, out var value55))
		{
			_transitionSpinEditing = value55.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._transitionSpinPending, out var value56))
		{
			_transitionSpinPending = value56.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._transitionSpinStart, out var value57))
		{
			_transitionSpinStart = value57.As<float>();
		}
		if (info.TryGetProperty(PropertyName._selectedTrack, out var value58))
		{
			_selectedTrack = value58.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedKeyTime, out var value59))
		{
			_selectedKeyTime = value59.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pendingNameOperation, out var value60))
		{
			_pendingNameOperation = value60.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingBindState, out var value61))
		{
			_pendingBindState = value61.As<Dictionary>();
		}
	}
}
