using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.AdobeAnimateEditor.Inspector;

[Tool]
[ScriptPath("res://addons/AdobeAnimateEditor/Inspector/AdobeAnimateInspectorPreview.cs")]
public class AdobeAnimateInspectorPreview : PanelContainer
{
	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _Notification = "_Notification";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName EditAnimation = "EditAnimation";

		public static readonly StringName RefreshAnimation = "RefreshAnimation";

		public static readonly StringName RefreshClipOptions = "RefreshClipOptions";

		public static readonly StringName SelectClipByName = "SelectClipByName";

		public static readonly StringName ClearPreview = "ClearPreview";

		public static readonly StringName StartAnimationHydration = "StartAnimationHydration";

		public static readonly StringName PollAnimationHydration = "PollAnimationHydration";

		public static readonly StringName CancelAnimationHydration = "CancelAnimationHydration";

		public static readonly StringName ClearRenderedPreview = "ClearRenderedPreview";

		public static readonly StringName ShowHydratedAnimation = "ShowHydratedAnimation";

		public static readonly StringName CreateSprite = "CreateSprite";

		public static readonly StringName BuildClipOptions = "BuildClipOptions";

		public static readonly StringName SelectClip = "SelectClip";

		public static readonly StringName TogglePlayback = "TogglePlayback";

		public static readonly StringName PreviousFrame = "PreviousFrame";

		public static readonly StringName UpdateVisibilityPlaybackState = "UpdateVisibilityPlaybackState";

		public static readonly StringName RefreshProcessEligibility = "RefreshProcessEligibility";

		public static readonly StringName NextFrame = "NextFrame";

		public static readonly StringName StepFrame = "StepFrame";

		public static readonly StringName SetFrameFromSlider = "SetFrameFromSlider";

		public static readonly StringName SetFrame = "SetFrame";

		public static readonly StringName ConfigureFrameRange = "ConfigureFrameRange";

		public static readonly StringName UpdateFrameControls = "UpdateFrameControls";

		public static readonly StringName PublishPreviewState = "PublishPreviewState";

		public static readonly StringName GetStartFrame = "GetStartFrame";

		public static readonly StringName GetEndFrame = "GetEndFrame";

		public static readonly StringName GetPlayableEndFrame = "GetPlayableEndFrame";

		public static readonly StringName SelectZoom = "SelectZoom";

		public static readonly StringName FitToPanel = "FitToPanel";

		public static readonly StringName OnPreviewResized = "OnPreviewResized";

		public static readonly StringName UpdatePreviewViewportLayout = "UpdatePreviewViewportLayout";

		public static readonly StringName OnPreviewGuiInput = "OnPreviewGuiInput";

		public static readonly StringName SetPreviewZoom = "SetPreviewZoom";

		public static readonly StringName UpdatePreviewTransform = "UpdatePreviewTransform";

		public static readonly StringName GetViewportCenter = "GetViewportCenter";

		public static readonly StringName ToViewportPosition = "ToViewportPosition";

		public static readonly StringName ToViewportDelta = "ToViewportDelta";

		public static readonly StringName SelectClosestZoomPreset = "SelectClosestZoomPreset";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName ControlRefreshCount = "ControlRefreshCount";

		public static readonly StringName CurrentFrame = "CurrentFrame";

		public static readonly StringName IsPlaying = "IsPlaying";

		public static readonly StringName SelectedClipName = "SelectedClipName";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _clipOption = "_clipOption";

		public static readonly StringName _playButton = "_playButton";

		public static readonly StringName _frameSlider = "_frameSlider";

		public static readonly StringName _frameLabel = "_frameLabel";

		public static readonly StringName _zoomOption = "_zoomOption";

		public static readonly StringName _viewportContainer = "_viewportContainer";

		public static readonly StringName _previewViewport = "_previewViewport";

		public static readonly StringName _background = "_background";

		public static readonly StringName _horizontalAxis = "_horizontalAxis";

		public static readonly StringName _verticalAxis = "_verticalAxis";

		public static readonly StringName _previewRoot = "_previewRoot";

		public static readonly StringName _cpuPreviewCanvas = "_cpuPreviewCanvas";

		public static readonly StringName _animation = "_animation";

		public static readonly StringName _sprite = "_sprite";

		public static readonly StringName _hydrationTarget = "_hydrationTarget";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _draggingPreview = "_draggingPreview";

		public static readonly StringName _resumePlaybackWhenVisible = "_resumePlaybackWhenVisible";

		public static readonly StringName _pausedForVisibility = "_pausedForVisibility";

		public static readonly StringName _previewPan = "_previewPan";

		public static readonly StringName _previewZoom = "_previewZoom";

		public static readonly StringName _lastControlFrame = "_lastControlFrame";

		public static readonly StringName _lastControlPaused = "_lastControlPaused";

		public static readonly StringName _lastPublishedFrame = "_lastPublishedFrame";

		public static readonly StringName _lastPublishedStart = "_lastPublishedStart";

		public static readonly StringName _lastPublishedEnd = "_lastPublishedEnd";

		public static readonly StringName _lastPublishedClip = "_lastPublishedClip";

		public static readonly StringName _lastPublishedPlaying = "_lastPublishedPlaying";

		public static readonly StringName _controlRefreshCount = "_controlRefreshCount";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private readonly List<string> _clipNames = new List<string>();

	private Label _statusLabel;

	private OptionButton _clipOption;

	private Button _playButton;

	private HSlider _frameSlider;

	private Label _frameLabel;

	private OptionButton _zoomOption;

	private SubViewportContainer _viewportContainer;

	private SubViewport _previewViewport;

	private ColorRect _background;

	private ColorRect _horizontalAxis;

	private ColorRect _verticalAxis;

	private Node2D _previewRoot;

	private AdobeAnimateCpuPreviewCanvas _cpuPreviewCanvas;

	private AdobeAnimateData _animation;

	private AdobeAnimateSprite _sprite;

	private AdobeAnimateData _hydrationTarget;

	private CancellationTokenSource _hydrationCancellation;

	private Task<AdobeAnimateData.EditorDatHydrationResult> _hydrationTask;

	private bool _updatingControls;

	private bool _draggingPreview;

	private bool _resumePlaybackWhenVisible;

	private bool _pausedForVisibility;

	private Vector2 _previewPan;

	private float _previewZoom = 1f;

	private int _lastControlFrame = -2147483648;

	private bool _lastControlPaused = true;

	private int _lastPublishedFrame = -2147483648;

	private int _lastPublishedStart = -2147483648;

	private int _lastPublishedEnd = -2147483648;

	private string _lastPublishedClip = "";

	private bool _lastPublishedPlaying;

	private int _controlRefreshCount;

	public int ControlRefreshCount => _controlRefreshCount;

	public int CurrentFrame
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_sprite))
			{
				return 0;
			}
			return _sprite.frameIndex;
		}
	}

	public bool IsPlaying
	{
		get
		{
			if (GodotObject.IsInstanceValid(_sprite) && !_sprite.pause)
			{
				return IsVisibleInTree();
			}
			return false;
		}
	}

	public string SelectedClipName
	{
		get
		{
			if (_clipOption == null || _clipOption.Selected < 0 || _clipOption.Selected >= _clipNames.Count)
			{
				return "";
			}
			return _clipNames[_clipOption.Selected];
		}
	}

	public event Action<int, int, int, string, bool> PreviewStateChanged;

	public override void _Ready()
	{
		_statusLabel = GetNode<Label>("%StatusLabel");
		_clipOption = GetNode<OptionButton>("%ClipOption");
		_playButton = GetNode<Button>("%PlayButton");
		_frameSlider = GetNode<HSlider>("%FrameSlider");
		_frameLabel = GetNode<Label>("%FrameLabel");
		_zoomOption = GetNode<OptionButton>("%ZoomOption");
		_viewportContainer = GetNode<SubViewportContainer>("%PreviewViewportContainer");
		_previewViewport = GetNode<SubViewport>("%PreviewViewport");
		_background = GetNode<ColorRect>("%Background");
		_horizontalAxis = GetNode<ColorRect>("%HorizontalAxis");
		_verticalAxis = GetNode<ColorRect>("%VerticalAxis");
		_previewRoot = GetNode<Node2D>("%PreviewRoot");
		_cpuPreviewCanvas = GetNode<AdobeAnimateCpuPreviewCanvas>("%CpuPreviewCanvas");
		GetNode<Button>("%PreviousButton").Connect(BaseButton.SignalName.Pressed, new Callable(this, "PreviousFrame"));
		GetNode<Button>("%NextButton").Connect(BaseButton.SignalName.Pressed, new Callable(this, "NextFrame"));
		GetNode<Button>("%FitButton").Connect(BaseButton.SignalName.Pressed, new Callable(this, "FitToPanel"));
		_playButton.Connect(BaseButton.SignalName.Pressed, new Callable(this, "TogglePlayback"));
		_clipOption.Connect(OptionButton.SignalName.ItemSelected, new Callable(this, "SelectClip"));
		_frameSlider.Connect(Godot.Range.SignalName.ValueChanged, new Callable(this, "SetFrameFromSlider"));
		_zoomOption.Connect(OptionButton.SignalName.ItemSelected, new Callable(this, "SelectZoom"));
		_viewportContainer.Connect(Control.SignalName.GuiInput, new Callable(this, "OnPreviewGuiInput"));
		_viewportContainer.Connect(Control.SignalName.Resized, new Callable(this, "OnPreviewResized"));
		UpdatePreviewViewportLayout();
		SetProcess(enable: false);
		if (GodotObject.IsInstanceValid(_animation))
		{
			EditAnimation(_animation);
		}
		else
		{
			RefreshProcessEligibility();
		}
	}

	public override void _Process(double delta)
	{
		PollAnimationHydration();
		UpdateVisibilityPlaybackState();
		if (GodotObject.IsInstanceValid(_sprite) && IsVisibleInTree())
		{
			UpdateFrameControls(_sprite.frameIndex);
		}
		RefreshProcessEligibility();
	}

	public override void _Notification(int what)
	{
		base._Notification(what);
		if ((long)what == 31 && IsNodeReady())
		{
			UpdateVisibilityPlaybackState();
			RefreshProcessEligibility();
		}
	}

	public override void _ExitTree()
	{
		CancelAnimationHydration();
		base._ExitTree();
	}

	public void EditAnimation(AdobeAnimateData animation)
	{
		_animation = animation;
		if (IsNodeReady())
		{
			if (!GodotObject.IsInstanceValid(_animation))
			{
				ClearPreview();
			}
			else if (!_animation.HasPackedRuntimeData())
			{
				ClearRenderedPreview();
				StartAnimationHydration();
			}
			else
			{
				ShowHydratedAnimation();
			}
		}
	}

	public void RefreshAnimation()
	{
		if (GodotObject.IsInstanceValid(_animation))
		{
			EditAnimation(_animation);
		}
	}

	public bool RefreshClipOptions(string preferredClipName = "")
	{
		if (!GodotObject.IsInstanceValid(_animation) || !GodotObject.IsInstanceValid(_sprite))
		{
			return false;
		}
		BuildClipOptions(preferredClipName);
		if (!string.IsNullOrEmpty(preferredClipName))
		{
			return string.Equals(SelectedClipName, preferredClipName, StringComparison.Ordinal);
		}
		return true;
	}

	public bool SelectClipByName(string clipName)
	{
		if (!GodotObject.IsInstanceValid(_sprite))
		{
			return false;
		}
		if (clipName == null)
		{
			clipName = "";
		}
		for (int i = 0; i < _clipNames.Count; i++)
		{
			if (string.Equals(_clipNames[i], clipName, StringComparison.Ordinal))
			{
				_clipOption.Select(i);
				SelectClip(i);
				return true;
			}
		}
		return false;
	}

	public void ClearPreview()
	{
		CancelAnimationHydration();
		ClearRenderedPreview();
		_animation = null;
		_clipNames.Clear();
		if (GodotObject.IsInstanceValid(_clipOption))
		{
			_clipOption.Clear();
		}
		if (GodotObject.IsInstanceValid(_statusLabel))
		{
			_statusLabel.Text = "0 帧";
		}
	}

	private void StartAnimationHydration()
	{
		if (GodotObject.IsInstanceValid(_animation) && (_hydrationTask == null || _hydrationTask.IsCompleted || _hydrationTarget != _animation))
		{
			CancelAnimationHydration();
			_hydrationTarget = _animation;
			_hydrationCancellation = new CancellationTokenSource();
			_hydrationTask = _animation.BuildEditorDatHydrationAsync(_animation.GetResolvedAnimeFilePath(), _hydrationCancellation.Token);
			_statusLabel.Text = "正在后台读取完整动画...";
			RefreshProcessEligibility();
		}
	}

	private void PollAnimationHydration()
	{
		if (_hydrationTask == null || !_hydrationTask.IsCompleted)
		{
			return;
		}
		AdobeAnimateData hydrationTarget = _hydrationTarget;
		Task<AdobeAnimateData.EditorDatHydrationResult> hydrationTask = _hydrationTask;
		_hydrationTarget = null;
		_hydrationTask = null;
		_hydrationCancellation?.Dispose();
		_hydrationCancellation = null;
		if (hydrationTask.IsCanceled)
		{
			RefreshProcessEligibility();
			return;
		}
		if (hydrationTask.IsFaulted)
		{
			_statusLabel.Text = "DAT 读取失败：" + hydrationTask.Exception?.GetBaseException().Message;
			RefreshProcessEligibility();
			return;
		}
		if (!GodotObject.IsInstanceValid(_animation) || hydrationTarget != _animation)
		{
			RefreshProcessEligibility();
			return;
		}
		AdobeAnimateData.EditorDatHydrationResult result = hydrationTask.GetAwaiter().GetResult();
		if (!result.IsSuccess || !_animation.ApplyEditorDatHydration(result))
		{
			_statusLabel.Text = (string.IsNullOrWhiteSpace(result.ErrorMessage) ? "DAT 数据不可用" : ("DAT 读取失败：" + result.ErrorMessage));
			RefreshProcessEligibility();
		}
		else
		{
			ShowHydratedAnimation();
		}
	}

	private void CancelAnimationHydration()
	{
		if (_hydrationCancellation != null)
		{
			_hydrationCancellation.Cancel();
			_hydrationCancellation.Dispose();
		}
		_hydrationCancellation = null;
		_hydrationTask = null;
		_hydrationTarget = null;
		RefreshProcessEligibility();
	}

	private void ClearRenderedPreview()
	{
		if (GodotObject.IsInstanceValid(_cpuPreviewCanvas))
		{
			_cpuPreviewCanvas.BindSource(null);
		}
		if (GodotObject.IsInstanceValid(_sprite))
		{
			_sprite.QueueFree();
		}
		_sprite = null;
		_resumePlaybackWhenVisible = false;
		_pausedForVisibility = false;
		_lastControlFrame = -2147483648;
		_lastPublishedFrame = -2147483648;
		RefreshProcessEligibility();
	}

	private void ShowHydratedAnimation()
	{
		CreateSprite();
		BuildClipOptions();
		ConfigureFrameRange();
		CallDeferred("FitToPanel");
		_statusLabel.Text = $"{_animation.frameMax} 帧 · {_animation.frameRate:0.##} fps";
		RefreshProcessEligibility();
	}

	private void CreateSprite()
	{
		if (GodotObject.IsInstanceValid(_sprite))
		{
			_sprite.QueueFree();
		}
		_sprite = new AdobeAnimateSprite
		{
			Name = "InspectorAnimationSprite",
			flashAnimeData = _animation,
			preview = true,
			forceLocalRender = true,
			suppressEditorRenderSubmission = true,
			loop = true,
			skipLastFrame = false,
			pause = false,
			keepRenderSubmittedWhenPaused = true,
			refreshEveryFrame = true
		};
		_previewRoot.AddChild(_sprite, forceReadableName: false, InternalMode.Disabled);
		_cpuPreviewCanvas.BindSource(_sprite);
		_resumePlaybackWhenVisible = false;
		_pausedForVisibility = false;
		UpdateVisibilityPlaybackState();
		_previewPan = Vector2.Zero;
		_previewZoom = 1f;
		UpdatePreviewTransform();
	}

	private void BuildClipOptions(string preferredClipName = "")
	{
		_clipNames.Clear();
		_clipOption.Clear();
		_clipOption.AddItem("全部帧");
		_clipNames.Add("");
		if (_animation.clips != null)
		{
			foreach (Variant key in _animation.clips.Keys)
			{
				string text = key.AsString();
				Vector2I clip = _animation.GetClip(text);
				_clipOption.AddItem($"{text} ({clip.X}-{GetPlayableEndFrame(clip)})");
				_clipNames.Add(text);
			}
		}
		int num = 0;
		if (!string.IsNullOrEmpty(preferredClipName))
		{
			for (int i = 1; i < _clipNames.Count; i++)
			{
				if (string.Equals(_clipNames[i], preferredClipName, StringComparison.Ordinal))
				{
					num = i;
					break;
				}
			}
		}
		_clipOption.Select(num);
		SelectClip(num);
	}

	public void SelectClip(long index)
	{
		if (GodotObject.IsInstanceValid(_sprite) && index >= 0 && index < _clipNames.Count)
		{
			bool pause = _sprite.pause;
			string text = _clipNames[(int)index];
			if (string.IsNullOrEmpty(text))
			{
				_sprite.clipRange = new Vector2I(0, Math.Max(0, _animation.frameMax));
				_sprite.frameIndex = 0;
				_sprite.elapsedTimer = 0.0;
				_sprite.QueueRedraw();
			}
			else
			{
				_sprite.SetClip(text);
			}
			_sprite.pause = pause;
			ConfigureFrameRange();
		}
	}

	public void TogglePlayback()
	{
		if (GodotObject.IsInstanceValid(_sprite))
		{
			_sprite.pause = !_sprite.pause;
			UpdateFrameControls(_sprite.frameIndex);
			RefreshProcessEligibility();
		}
	}

	public void PreviousFrame()
	{
		StepFrame(-1);
	}

	private void UpdateVisibilityPlaybackState()
	{
		if (GodotObject.IsInstanceValid(_cpuPreviewCanvas))
		{
			_cpuPreviewCanvas.SetPreviewActive(IsVisibleInTree());
		}
		if (!GodotObject.IsInstanceValid(_sprite))
		{
			RefreshProcessEligibility();
		}
		else if (!IsVisibleInTree())
		{
			if (_pausedForVisibility)
			{
				RefreshProcessEligibility();
				return;
			}
			_resumePlaybackWhenVisible = !_sprite.pause;
			_pausedForVisibility = true;
			if (_resumePlaybackWhenVisible)
			{
				_sprite.pause = true;
			}
			RefreshProcessEligibility();
		}
		else if (!_pausedForVisibility)
		{
			RefreshProcessEligibility();
		}
		else
		{
			_pausedForVisibility = false;
			if (_resumePlaybackWhenVisible)
			{
				_sprite.pause = false;
			}
			_resumePlaybackWhenVisible = false;
			UpdateFrameControls(_sprite.frameIndex);
			RefreshProcessEligibility();
		}
	}

	private void RefreshProcessEligibility()
	{
		if (IsInsideTree())
		{
			bool flag = _hydrationTask != null && !_hydrationTask.IsCompleted;
			bool flag2 = IsVisibleInTree() && GodotObject.IsInstanceValid(_sprite) && !_sprite.pause;
			SetProcess(flag | flag2);
		}
	}

	public void NextFrame()
	{
		StepFrame(1);
	}

	private void StepFrame(int direction)
	{
		if (GodotObject.IsInstanceValid(_sprite))
		{
			_sprite.pause = true;
			SetFrame(Mathf.Clamp(_sprite.frameIndex + direction, GetStartFrame(), GetEndFrame()));
			RefreshProcessEligibility();
		}
	}

	public void SetFrameFromSlider(double value)
	{
		if (!_updatingControls && GodotObject.IsInstanceValid(_sprite))
		{
			int num = Mathf.Clamp(Mathf.RoundToInt(value), GetStartFrame(), GetEndFrame());
			_sprite.pause = true;
			if (_sprite.frameIndex != num)
			{
				SetFrame(num);
			}
			else
			{
				UpdateFrameControls(num);
			}
			RefreshProcessEligibility();
		}
	}

	private void SetFrame(int frame)
	{
		_sprite.frameIndex = Mathf.Clamp(frame, GetStartFrame(), GetEndFrame());
		_sprite.elapsedTimer = 0.0;
		_sprite.QueueRedraw();
		UpdateFrameControls(_sprite.frameIndex);
	}

	private void ConfigureFrameRange()
	{
		if (GodotObject.IsInstanceValid(_sprite))
		{
			_updatingControls = true;
			_frameSlider.MinValue = GetStartFrame();
			_frameSlider.MaxValue = GetEndFrame();
			_updatingControls = false;
			UpdateFrameControls(_sprite.frameIndex);
		}
	}

	private void UpdateFrameControls(int frame)
	{
		if (!GodotObject.IsInstanceValid(_sprite))
		{
			return;
		}
		bool pause = _sprite.pause;
		if (_lastControlFrame != frame || _lastControlPaused != pause)
		{
			_updatingControls = true;
			if (_lastControlFrame != frame)
			{
				_frameSlider.Value = frame;
				_frameLabel.Text = $"{frame} / {GetEndFrame()}";
			}
			if (_lastControlPaused != pause)
			{
				_playButton.Text = (pause ? "播放" : "暂停");
			}
			_updatingControls = false;
			_lastControlFrame = frame;
			_lastControlPaused = pause;
			_controlRefreshCount++;
		}
		PublishPreviewState(frame);
	}

	private void PublishPreviewState(int frame)
	{
		int startFrame = GetStartFrame();
		int endFrame = GetEndFrame();
		string selectedClipName = SelectedClipName;
		bool isPlaying = IsPlaying;
		if (_lastPublishedFrame != frame || _lastPublishedStart != startFrame || _lastPublishedEnd != endFrame || !string.Equals(_lastPublishedClip, selectedClipName, StringComparison.Ordinal) || _lastPublishedPlaying != isPlaying)
		{
			_lastPublishedFrame = frame;
			_lastPublishedStart = startFrame;
			_lastPublishedEnd = endFrame;
			_lastPublishedClip = selectedClipName;
			_lastPublishedPlaying = isPlaying;
			PreviewStateChanged?.Invoke(frame, startFrame, endFrame, selectedClipName, isPlaying);
		}
	}

	private int GetStartFrame()
	{
		if (!GodotObject.IsInstanceValid(_sprite))
		{
			return 0;
		}
		return Math.Max(0, _sprite.clipRange.X);
	}

	private int GetEndFrame()
	{
		if (!GodotObject.IsInstanceValid(_sprite))
		{
			return 0;
		}
		return Math.Max(GetStartFrame(), Math.Min(Math.Max(0, _animation.frameMax - 1), GetPlayableEndFrame(_sprite.clipRange)));
	}

	private static int GetPlayableEndFrame(Vector2I range)
	{
		return Math.Max(range.X, range.Y - 1);
	}

	public void SelectZoom(long index)
	{
		if (GodotObject.IsInstanceValid(_sprite) && index >= 0 && index < _zoomOption.ItemCount)
		{
			int itemId = _zoomOption.GetItemId((int)index);
			if (itemId <= 0)
			{
				FitToPanel();
			}
			else
			{
				SetPreviewZoom((float)itemId / 100f, GetViewportCenter());
			}
		}
	}

	public void FitToPanel()
	{
		if (!GodotObject.IsInstanceValid(_sprite) || !GodotObject.IsInstanceValid(_previewViewport))
		{
			return;
		}
		Rect2 rect = ((!GodotObject.IsInstanceValid(_cpuPreviewCanvas) || !_cpuPreviewCanvas.TryGetLocalRenderBounds(out var bounds)) ? (AdobeAnimateDefinitionCache.GetOrBuild(_animation)?.LocalBounds ?? default(Rect2)) : bounds);
		if (rect.Size.X <= 0.001f || rect.Size.Y <= 0.001f)
		{
			_previewZoom = 1f;
			_previewPan = Vector2.Zero;
			UpdatePreviewTransform();
			return;
		}
		Vector2 vector = new Vector2(_previewViewport.Size.X, _previewViewport.Size.Y) - Vector2.One * 24f;
		float value = Mathf.Min(vector.X / rect.Size.X, vector.Y / rect.Size.Y);
		_previewZoom = Mathf.Clamp(value, 0.05f, 16f);
		_previewPan = -rect.GetCenter() * _previewZoom;
		UpdatePreviewTransform();
		if (_zoomOption.ItemCount > 0)
		{
			_zoomOption.Select(0);
		}
	}

	public void OnPreviewResized()
	{
		UpdatePreviewViewportLayout();
		if (_zoomOption.Selected == 0)
		{
			FitToPanel();
		}
		else
		{
			UpdatePreviewTransform();
		}
	}

	private void UpdatePreviewViewportLayout()
	{
		if (GodotObject.IsInstanceValid(_viewportContainer) && GodotObject.IsInstanceValid(_previewViewport))
		{
			Vector2 size = _viewportContainer.Size;
			if (!(size.X < 2f) && !(size.Y < 2f))
			{
				Vector2I size2 = new Vector2I(Math.Max(2, Mathf.RoundToInt(size.X)), Math.Max(2, Mathf.RoundToInt(size.Y)));
				_previewViewport.Size = size2;
				Vector2 vector = new Vector2((float)size2.X * 0.5f, (float)size2.Y * 0.5f);
				_background.Size = new Vector2(size2.X, size2.Y);
				_horizontalAxis.Position = new Vector2(0f, vector.Y - 1f);
				_horizontalAxis.Size = new Vector2(size2.X, 2f);
				_verticalAxis.Position = new Vector2(vector.X - 1f, 0f);
				_verticalAxis.Size = new Vector2(2f, size2.Y);
			}
		}
	}

	public void OnPreviewGuiInput(InputEvent inputEvent)
	{
		if (!GodotObject.IsInstanceValid(_sprite))
		{
			return;
		}
		if (inputEvent is InputEventMouseButton { ButtonIndex: var buttonIndex } inputEventMouseButton)
		{
			bool flag = (((ulong)(buttonIndex - 4) <= 1uL) ? true : false);
			if (flag && inputEventMouseButton.Pressed)
			{
				float num = ((inputEventMouseButton.ButtonIndex == MouseButton.WheelUp) ? 1.12f : (25f / 28f));
				SetPreviewZoom(_previewZoom * num, ToViewportPosition(inputEventMouseButton.Position));
				SelectClosestZoomPreset();
				_viewportContainer.AcceptEvent();
				return;
			}
			MouseButton buttonIndex2 = inputEventMouseButton.ButtonIndex;
			if ((buttonIndex2 == MouseButton.Left || buttonIndex2 == MouseButton.Middle) ? true : false)
			{
				_draggingPreview = inputEventMouseButton.Pressed;
				_viewportContainer.MouseDefaultCursorShape = (CursorShape)(_draggingPreview ? 6 : 13);
				_viewportContainer.AcceptEvent();
				return;
			}
			if (inputEventMouseButton.ButtonIndex == MouseButton.Right && inputEventMouseButton.Pressed)
			{
				FitToPanel();
				_viewportContainer.AcceptEvent();
			}
		}
		if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _draggingPreview)
		{
			_previewPan += ToViewportDelta(inputEventMouseMotion.Relative);
			UpdatePreviewTransform();
			_viewportContainer.AcceptEvent();
		}
	}

	private void SetPreviewZoom(float zoom, Vector2 pivot)
	{
		float num = Mathf.Clamp(zoom, 0.05f, 16f);
		if (!Mathf.IsEqualApprox(num, _previewZoom))
		{
			Vector2 vector = GetViewportCenter() + _previewPan;
			Vector2 vector2 = (pivot - vector) / _previewZoom;
			_previewZoom = num;
			_previewPan = pivot - vector2 * _previewZoom - GetViewportCenter();
			UpdatePreviewTransform();
		}
	}

	private void UpdatePreviewTransform()
	{
		if (GodotObject.IsInstanceValid(_previewRoot) && GodotObject.IsInstanceValid(_sprite))
		{
			_previewRoot.Position = GetViewportCenter() + _previewPan;
			_sprite.Scale = Vector2.One * _previewZoom;
		}
	}

	private Vector2 GetViewportCenter()
	{
		if (!GodotObject.IsInstanceValid(_previewViewport))
		{
			return Vector2.Zero;
		}
		return new Vector2((float)_previewViewport.Size.X * 0.5f, (float)_previewViewport.Size.Y * 0.5f);
	}

	private Vector2 ToViewportPosition(Vector2 controlPosition)
	{
		if (_viewportContainer.Size.X <= 0.001f || _viewportContainer.Size.Y <= 0.001f)
		{
			return controlPosition;
		}
		return controlPosition * new Vector2((float)_previewViewport.Size.X / _viewportContainer.Size.X, (float)_previewViewport.Size.Y / _viewportContainer.Size.Y);
	}

	private Vector2 ToViewportDelta(Vector2 controlDelta)
	{
		if (_viewportContainer.Size.X <= 0.001f || _viewportContainer.Size.Y <= 0.001f)
		{
			return controlDelta;
		}
		return controlDelta * new Vector2((float)_previewViewport.Size.X / _viewportContainer.Size.X, (float)_previewViewport.Size.Y / _viewportContainer.Size.Y);
	}

	private void SelectClosestZoomPreset()
	{
		if (_zoomOption.ItemCount <= 1)
		{
			return;
		}
		int idx = 1;
		float num = 3.4028235E+38f;
		for (int i = 1; i < _zoomOption.ItemCount; i++)
		{
			float num2 = Mathf.Abs((float)_zoomOption.GetItemId(i) / 100f - _previewZoom);
			if (num2 < num)
			{
				idx = i;
				num = num2;
			}
		}
		_zoomOption.Select(idx);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(42)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EditAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshClipOptions, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "preferredClipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectClipByName, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartAnimationHydration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PollAnimationHydration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelAnimationHydration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearRenderedPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowHydratedAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildClipOptions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "preferredClipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TogglePlayback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviousFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateVisibilityPlaybackState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshProcessEligibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StepFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFrameFromSlider, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureFrameRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateFrameControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PublishPreviewState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetStartFrame, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEndFrame, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPlayableEndFrame, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "range", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectZoom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FitToPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPreviewResized, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePreviewViewportLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPreviewGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetPreviewZoom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "zoom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pivot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePreviewTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetViewportCenter, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToViewportPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "controlPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToViewportDelta, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "controlDelta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectClosestZoomPreset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.EditAnimation && args.Count == 1)
		{
			EditAnimation(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAnimation && args.Count == 0)
		{
			RefreshAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshClipOptions && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RefreshClipOptions(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectClipByName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SelectClipByName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearPreview && args.Count == 0)
		{
			ClearPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.StartAnimationHydration && args.Count == 0)
		{
			StartAnimationHydration();
			ret = default;
			return true;
		}
		if (method == MethodName.PollAnimationHydration && args.Count == 0)
		{
			PollAnimationHydration();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelAnimationHydration && args.Count == 0)
		{
			CancelAnimationHydration();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearRenderedPreview && args.Count == 0)
		{
			ClearRenderedPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowHydratedAnimation && args.Count == 0)
		{
			ShowHydratedAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSprite && args.Count == 0)
		{
			CreateSprite();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildClipOptions && args.Count == 1)
		{
			BuildClipOptions(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectClip && args.Count == 1)
		{
			SelectClip(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TogglePlayback && args.Count == 0)
		{
			TogglePlayback();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviousFrame && args.Count == 0)
		{
			PreviousFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateVisibilityPlaybackState && args.Count == 0)
		{
			UpdateVisibilityPlaybackState();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshProcessEligibility && args.Count == 0)
		{
			RefreshProcessEligibility();
			ret = default;
			return true;
		}
		if (method == MethodName.NextFrame && args.Count == 0)
		{
			NextFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.StepFrame && args.Count == 1)
		{
			StepFrame(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFrameFromSlider && args.Count == 1)
		{
			SetFrameFromSlider(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFrame && args.Count == 1)
		{
			SetFrame(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureFrameRange && args.Count == 0)
		{
			ConfigureFrameRange();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateFrameControls && args.Count == 1)
		{
			UpdateFrameControls(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PublishPreviewState && args.Count == 1)
		{
			PublishPreviewState(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetStartFrame && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetStartFrame());
			return true;
		}
		if (method == MethodName.GetEndFrame && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEndFrame());
			return true;
		}
		if (method == MethodName.GetPlayableEndFrame && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetPlayableEndFrame(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectZoom && args.Count == 1)
		{
			SelectZoom(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FitToPanel && args.Count == 0)
		{
			FitToPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPreviewResized && args.Count == 0)
		{
			OnPreviewResized();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreviewViewportLayout && args.Count == 0)
		{
			UpdatePreviewViewportLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPreviewGuiInput && args.Count == 1)
		{
			OnPreviewGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPreviewZoom && args.Count == 2)
		{
			SetPreviewZoom(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreviewTransform && args.Count == 0)
		{
			UpdatePreviewTransform();
			ret = default;
			return true;
		}
		if (method == MethodName.GetViewportCenter && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetViewportCenter());
			return true;
		}
		if (method == MethodName.ToViewportPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ToViewportPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.ToViewportDelta && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ToViewportDelta(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectClosestZoomPreset && args.Count == 0)
		{
			SelectClosestZoomPreset();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetPlayableEndFrame && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetPlayableEndFrame(VariantUtils.ConvertTo<Vector2I>(in args[0])));
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
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.EditAnimation)
		{
			return true;
		}
		if (method == MethodName.RefreshAnimation)
		{
			return true;
		}
		if (method == MethodName.RefreshClipOptions)
		{
			return true;
		}
		if (method == MethodName.SelectClipByName)
		{
			return true;
		}
		if (method == MethodName.ClearPreview)
		{
			return true;
		}
		if (method == MethodName.StartAnimationHydration)
		{
			return true;
		}
		if (method == MethodName.PollAnimationHydration)
		{
			return true;
		}
		if (method == MethodName.CancelAnimationHydration)
		{
			return true;
		}
		if (method == MethodName.ClearRenderedPreview)
		{
			return true;
		}
		if (method == MethodName.ShowHydratedAnimation)
		{
			return true;
		}
		if (method == MethodName.CreateSprite)
		{
			return true;
		}
		if (method == MethodName.BuildClipOptions)
		{
			return true;
		}
		if (method == MethodName.SelectClip)
		{
			return true;
		}
		if (method == MethodName.TogglePlayback)
		{
			return true;
		}
		if (method == MethodName.PreviousFrame)
		{
			return true;
		}
		if (method == MethodName.UpdateVisibilityPlaybackState)
		{
			return true;
		}
		if (method == MethodName.RefreshProcessEligibility)
		{
			return true;
		}
		if (method == MethodName.NextFrame)
		{
			return true;
		}
		if (method == MethodName.StepFrame)
		{
			return true;
		}
		if (method == MethodName.SetFrameFromSlider)
		{
			return true;
		}
		if (method == MethodName.SetFrame)
		{
			return true;
		}
		if (method == MethodName.ConfigureFrameRange)
		{
			return true;
		}
		if (method == MethodName.UpdateFrameControls)
		{
			return true;
		}
		if (method == MethodName.PublishPreviewState)
		{
			return true;
		}
		if (method == MethodName.GetStartFrame)
		{
			return true;
		}
		if (method == MethodName.GetEndFrame)
		{
			return true;
		}
		if (method == MethodName.GetPlayableEndFrame)
		{
			return true;
		}
		if (method == MethodName.SelectZoom)
		{
			return true;
		}
		if (method == MethodName.FitToPanel)
		{
			return true;
		}
		if (method == MethodName.OnPreviewResized)
		{
			return true;
		}
		if (method == MethodName.UpdatePreviewViewportLayout)
		{
			return true;
		}
		if (method == MethodName.OnPreviewGuiInput)
		{
			return true;
		}
		if (method == MethodName.SetPreviewZoom)
		{
			return true;
		}
		if (method == MethodName.UpdatePreviewTransform)
		{
			return true;
		}
		if (method == MethodName.GetViewportCenter)
		{
			return true;
		}
		if (method == MethodName.ToViewportPosition)
		{
			return true;
		}
		if (method == MethodName.ToViewportDelta)
		{
			return true;
		}
		if (method == MethodName.SelectClosestZoomPreset)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._clipOption)
		{
			_clipOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._playButton)
		{
			_playButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._frameSlider)
		{
			_frameSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._frameLabel)
		{
			_frameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._zoomOption)
		{
			_zoomOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._viewportContainer)
		{
			_viewportContainer = VariantUtils.ConvertTo<SubViewportContainer>(in value);
			return true;
		}
		if (name == PropertyName._previewViewport)
		{
			_previewViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._background)
		{
			_background = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName._horizontalAxis)
		{
			_horizontalAxis = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName._verticalAxis)
		{
			_verticalAxis = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName._previewRoot)
		{
			_previewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._cpuPreviewCanvas)
		{
			_cpuPreviewCanvas = VariantUtils.ConvertTo<AdobeAnimateCpuPreviewCanvas>(in value);
			return true;
		}
		if (name == PropertyName._animation)
		{
			_animation = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			_sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._hydrationTarget)
		{
			_hydrationTarget = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._draggingPreview)
		{
			_draggingPreview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._resumePlaybackWhenVisible)
		{
			_resumePlaybackWhenVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pausedForVisibility)
		{
			_pausedForVisibility = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._lastControlFrame)
		{
			_lastControlFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastControlPaused)
		{
			_lastControlPaused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lastPublishedFrame)
		{
			_lastPublishedFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastPublishedStart)
		{
			_lastPublishedStart = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastPublishedEnd)
		{
			_lastPublishedEnd = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastPublishedClip)
		{
			_lastPublishedClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._lastPublishedPlaying)
		{
			_lastPublishedPlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._controlRefreshCount)
		{
			_controlRefreshCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.ControlRefreshCount)
		{
			from = ControlRefreshCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CurrentFrame)
		{
			from = CurrentFrame;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsPlaying)
		{
			value = VariantUtils.CreateFrom<bool>(IsPlaying);
			return true;
		}
		if (name == PropertyName.SelectedClipName)
		{
			value = VariantUtils.CreateFrom<string>(SelectedClipName);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._clipOption)
		{
			value = VariantUtils.CreateFrom(in _clipOption);
			return true;
		}
		if (name == PropertyName._playButton)
		{
			value = VariantUtils.CreateFrom(in _playButton);
			return true;
		}
		if (name == PropertyName._frameSlider)
		{
			value = VariantUtils.CreateFrom(in _frameSlider);
			return true;
		}
		if (name == PropertyName._frameLabel)
		{
			value = VariantUtils.CreateFrom(in _frameLabel);
			return true;
		}
		if (name == PropertyName._zoomOption)
		{
			value = VariantUtils.CreateFrom(in _zoomOption);
			return true;
		}
		if (name == PropertyName._viewportContainer)
		{
			value = VariantUtils.CreateFrom(in _viewportContainer);
			return true;
		}
		if (name == PropertyName._previewViewport)
		{
			value = VariantUtils.CreateFrom(in _previewViewport);
			return true;
		}
		if (name == PropertyName._background)
		{
			value = VariantUtils.CreateFrom(in _background);
			return true;
		}
		if (name == PropertyName._horizontalAxis)
		{
			value = VariantUtils.CreateFrom(in _horizontalAxis);
			return true;
		}
		if (name == PropertyName._verticalAxis)
		{
			value = VariantUtils.CreateFrom(in _verticalAxis);
			return true;
		}
		if (name == PropertyName._previewRoot)
		{
			value = VariantUtils.CreateFrom(in _previewRoot);
			return true;
		}
		if (name == PropertyName._cpuPreviewCanvas)
		{
			value = VariantUtils.CreateFrom(in _cpuPreviewCanvas);
			return true;
		}
		if (name == PropertyName._animation)
		{
			value = VariantUtils.CreateFrom(in _animation);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			value = VariantUtils.CreateFrom(in _sprite);
			return true;
		}
		if (name == PropertyName._hydrationTarget)
		{
			value = VariantUtils.CreateFrom(in _hydrationTarget);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._draggingPreview)
		{
			value = VariantUtils.CreateFrom(in _draggingPreview);
			return true;
		}
		if (name == PropertyName._resumePlaybackWhenVisible)
		{
			value = VariantUtils.CreateFrom(in _resumePlaybackWhenVisible);
			return true;
		}
		if (name == PropertyName._pausedForVisibility)
		{
			value = VariantUtils.CreateFrom(in _pausedForVisibility);
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
		if (name == PropertyName._lastControlFrame)
		{
			value = VariantUtils.CreateFrom(in _lastControlFrame);
			return true;
		}
		if (name == PropertyName._lastControlPaused)
		{
			value = VariantUtils.CreateFrom(in _lastControlPaused);
			return true;
		}
		if (name == PropertyName._lastPublishedFrame)
		{
			value = VariantUtils.CreateFrom(in _lastPublishedFrame);
			return true;
		}
		if (name == PropertyName._lastPublishedStart)
		{
			value = VariantUtils.CreateFrom(in _lastPublishedStart);
			return true;
		}
		if (name == PropertyName._lastPublishedEnd)
		{
			value = VariantUtils.CreateFrom(in _lastPublishedEnd);
			return true;
		}
		if (name == PropertyName._lastPublishedClip)
		{
			value = VariantUtils.CreateFrom(in _lastPublishedClip);
			return true;
		}
		if (name == PropertyName._lastPublishedPlaying)
		{
			value = VariantUtils.CreateFrom(in _lastPublishedPlaying);
			return true;
		}
		if (name == PropertyName._controlRefreshCount)
		{
			value = VariantUtils.CreateFrom(in _controlRefreshCount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clipOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._playButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._frameSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._frameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zoomOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._viewportContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._background, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._horizontalAxis, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._verticalAxis, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cpuPreviewCanvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hydrationTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._draggingPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._resumePlaybackWhenVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pausedForVisibility, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previewPan, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._previewZoom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastControlFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._lastControlPaused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastPublishedFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastPublishedStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastPublishedEnd, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._lastPublishedClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._lastPublishedPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._controlRefreshCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ControlRefreshCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CurrentFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.SelectedClipName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._clipOption, Variant.From(in _clipOption));
		info.AddProperty(PropertyName._playButton, Variant.From(in _playButton));
		info.AddProperty(PropertyName._frameSlider, Variant.From(in _frameSlider));
		info.AddProperty(PropertyName._frameLabel, Variant.From(in _frameLabel));
		info.AddProperty(PropertyName._zoomOption, Variant.From(in _zoomOption));
		info.AddProperty(PropertyName._viewportContainer, Variant.From(in _viewportContainer));
		info.AddProperty(PropertyName._previewViewport, Variant.From(in _previewViewport));
		info.AddProperty(PropertyName._background, Variant.From(in _background));
		info.AddProperty(PropertyName._horizontalAxis, Variant.From(in _horizontalAxis));
		info.AddProperty(PropertyName._verticalAxis, Variant.From(in _verticalAxis));
		info.AddProperty(PropertyName._previewRoot, Variant.From(in _previewRoot));
		info.AddProperty(PropertyName._cpuPreviewCanvas, Variant.From(in _cpuPreviewCanvas));
		info.AddProperty(PropertyName._animation, Variant.From(in _animation));
		info.AddProperty(PropertyName._sprite, Variant.From(in _sprite));
		info.AddProperty(PropertyName._hydrationTarget, Variant.From(in _hydrationTarget));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._draggingPreview, Variant.From(in _draggingPreview));
		info.AddProperty(PropertyName._resumePlaybackWhenVisible, Variant.From(in _resumePlaybackWhenVisible));
		info.AddProperty(PropertyName._pausedForVisibility, Variant.From(in _pausedForVisibility));
		info.AddProperty(PropertyName._previewPan, Variant.From(in _previewPan));
		info.AddProperty(PropertyName._previewZoom, Variant.From(in _previewZoom));
		info.AddProperty(PropertyName._lastControlFrame, Variant.From(in _lastControlFrame));
		info.AddProperty(PropertyName._lastControlPaused, Variant.From(in _lastControlPaused));
		info.AddProperty(PropertyName._lastPublishedFrame, Variant.From(in _lastPublishedFrame));
		info.AddProperty(PropertyName._lastPublishedStart, Variant.From(in _lastPublishedStart));
		info.AddProperty(PropertyName._lastPublishedEnd, Variant.From(in _lastPublishedEnd));
		info.AddProperty(PropertyName._lastPublishedClip, Variant.From(in _lastPublishedClip));
		info.AddProperty(PropertyName._lastPublishedPlaying, Variant.From(in _lastPublishedPlaying));
		info.AddProperty(PropertyName._controlRefreshCount, Variant.From(in _controlRefreshCount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._statusLabel, out var value))
		{
			_statusLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._clipOption, out var value2))
		{
			_clipOption = value2.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._playButton, out var value3))
		{
			_playButton = value3.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._frameSlider, out var value4))
		{
			_frameSlider = value4.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._frameLabel, out var value5))
		{
			_frameLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._zoomOption, out var value6))
		{
			_zoomOption = value6.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._viewportContainer, out var value7))
		{
			_viewportContainer = value7.As<SubViewportContainer>();
		}
		if (info.TryGetProperty(PropertyName._previewViewport, out var value8))
		{
			_previewViewport = value8.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._background, out var value9))
		{
			_background = value9.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName._horizontalAxis, out var value10))
		{
			_horizontalAxis = value10.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName._verticalAxis, out var value11))
		{
			_verticalAxis = value11.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName._previewRoot, out var value12))
		{
			_previewRoot = value12.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._cpuPreviewCanvas, out var value13))
		{
			_cpuPreviewCanvas = value13.As<AdobeAnimateCpuPreviewCanvas>();
		}
		if (info.TryGetProperty(PropertyName._animation, out var value14))
		{
			_animation = value14.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._sprite, out var value15))
		{
			_sprite = value15.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._hydrationTarget, out var value16))
		{
			_hydrationTarget = value16.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value17))
		{
			_updatingControls = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._draggingPreview, out var value18))
		{
			_draggingPreview = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._resumePlaybackWhenVisible, out var value19))
		{
			_resumePlaybackWhenVisible = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pausedForVisibility, out var value20))
		{
			_pausedForVisibility = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._previewPan, out var value21))
		{
			_previewPan = value21.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._previewZoom, out var value22))
		{
			_previewZoom = value22.As<float>();
		}
		if (info.TryGetProperty(PropertyName._lastControlFrame, out var value23))
		{
			_lastControlFrame = value23.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastControlPaused, out var value24))
		{
			_lastControlPaused = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lastPublishedFrame, out var value25))
		{
			_lastPublishedFrame = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastPublishedStart, out var value26))
		{
			_lastPublishedStart = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastPublishedEnd, out var value27))
		{
			_lastPublishedEnd = value27.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastPublishedClip, out var value28))
		{
			_lastPublishedClip = value28.As<string>();
		}
		if (info.TryGetProperty(PropertyName._lastPublishedPlaying, out var value29))
		{
			_lastPublishedPlaying = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._controlRefreshCount, out var value30))
		{
			_controlRefreshCount = value30.As<int>();
		}
	}
}
