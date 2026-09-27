using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/2D/Animation/XW2DAnimationTimelineCanvas.cs")]
public class XW2DAnimationTimelineCanvas : Control
{
	private sealed class TrackRecord
	{
		public int TrackIndex;

		public Animation.TrackType Type;

		public bool Enabled;

		public string Path = "";

		public string TypeLabel = "";

		public Color Color = Colors.White;

		public Animation.InterpolationType Interpolation;

		public Animation.UpdateMode UpdateMode;

		public bool InterpolationLoopWrap;

		public double[] KeyTimes = Array.Empty<double>();

		public float[] KeyTransitions = Array.Empty<float>();
	}

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName BindAnimation = "BindAnimation";

		public static readonly StringName SetPlayhead = "SetPlayhead";

		public static readonly StringName SetTimeScale = "SetTimeScale";

		public static readonly StringName SelectKey = "SelectKey";

		public new static readonly StringName _GuiInput = "_GuiInput";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName DrawTimeRulerAndGrid = "DrawTimeRulerAndGrid";

		public static readonly StringName DrawTracksAndKeys = "DrawTracksAndKeys";

		public static readonly StringName DrawPlayhead = "DrawPlayhead";

		public static readonly StringName DrawFixedTrackRail = "DrawFixedTrackRail";

		public static readonly StringName HandleWheel = "HandleWheel";

		public static readonly StringName BeginLeftPointerOperation = "BeginLeftPointerOperation";

		public static readonly StringName FinishLeftPointerOperation = "FinishLeftPointerOperation";

		public static readonly StringName SetPlayheadInternal = "SetPlayheadInternal";

		public static readonly StringName OnAnimationChanged = "OnAnimationChanged";

		public static readonly StringName MarkTimelineIndexDirty = "MarkTimelineIndexDirty";

		public static readonly StringName EnsureTimelineIndex = "EnsureTimelineIndex";

		public static readonly StringName RebuildTimelineIndex = "RebuildTimelineIndex";

		public static readonly StringName DisconnectAnimation = "DisconnectAnimation";

		public static readonly StringName ClampViewport = "ClampViewport";

		public static readonly StringName GetTimelineEnd = "GetTimelineEnd";

		public static readonly StringName TrackRowY = "TrackRowY";

		public static readonly StringName TimeToX = "TimeToX";

		public static readonly StringName XToTime = "XToTime";

		public static readonly StringName FindTrackRow = "FindTrackRow";

		public static readonly StringName CancelPointerOperation = "CancelPointerOperation";

		public static readonly StringName ShouldVisitTimeline = "ShouldVisitTimeline";

		public static readonly StringName QueueTimelineRedraw = "QueueTimelineRedraw";

		public static readonly StringName LowerBound = "LowerBound";

		public static readonly StringName FindNearestKey = "FindNearestKey";

		public static readonly StringName ChooseMajorTickStep = "ChooseMajorTickStep";

		public static readonly StringName FormatSeconds = "FormatSeconds";

		public static readonly StringName EllipsizeTrackPath = "EllipsizeTrackPath";

		public static readonly StringName GetTrackTypeLabel = "GetTrackTypeLabel";

		public static readonly StringName GetTrackTypeColor = "GetTrackTypeColor";

		public static readonly StringName GetInterpolationBadge = "GetInterpolationBadge";

		public static readonly StringName GetUpdateModeBadge = "GetUpdateModeBadge";

		public static readonly StringName DrawDiamond = "DrawDiamond";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName BoundAnimation = "BoundAnimation";

		public static readonly StringName Playhead = "Playhead";

		public static readonly StringName TimeScale = "TimeScale";

		public static readonly StringName VisibleTimeStart = "VisibleTimeStart";

		public static readonly StringName SelectedTrack = "SelectedTrack";

		public static readonly StringName SelectedKeyTime = "SelectedKeyTime";

		public static readonly StringName LastDrawVisitedTrackCount = "LastDrawVisitedTrackCount";

		public static readonly StringName LastDrawVisitedKeyCount = "LastDrawVisitedKeyCount";

		public static readonly StringName TimelineIndexRebuildCount = "TimelineIndexRebuildCount";

		public static readonly StringName _animation = "_animation";

		public static readonly StringName _timelineIndexDirty = "_timelineIndexDirty";

		public static readonly StringName _playhead = "_playhead";

		public static readonly StringName _timeScale = "_timeScale";

		public static readonly StringName _timeWindowStart = "_timeWindowStart";

		public static readonly StringName _trackScroll = "_trackScroll";

		public static readonly StringName _selectedTrack = "_selectedTrack";

		public static readonly StringName _selectedKeyTime = "_selectedKeyTime";

		public static readonly StringName _scrubbing = "_scrubbing";

		public static readonly StringName _draggingKey = "_draggingKey";

		public static readonly StringName _dragMoved = "_dragMoved";

		public static readonly StringName _dragTrack = "_dragTrack";

		public static readonly StringName _dragOriginTime = "_dragOriginTime";

		public static readonly StringName _dragTargetTime = "_dragTargetTime";

		public static readonly StringName _panningTime = "_panningTime";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const float RulerHeight = 34f;

	private const float TrackRailWidth = 238f;

	private const float TrackRowHeight = 42f;

	private const float KeyRadius = 6.5f;

	private const float KeyHitRadius = 11f;

	private const double MinimumTimeScale = 24.0;

	private const double MaximumTimeScale = 960.0;

	private const double DefaultTimeScale = 96.0;

	private const double TimeEpsilon = 1E-06;

	private readonly List<TrackRecord> _tracks = new List<TrackRecord>();

	private readonly Vector2[][] _transitionPointBuffers = new Vector2[19][];

	private Animation _animation;

	private Action _animationChangedAction;

	private bool _timelineIndexDirty;

	private double _playhead;

	private double _timeScale = 96.0;

	private double _timeWindowStart;

	private float _trackScroll;

	private int _selectedTrack = -1;

	private double _selectedKeyTime = -1.0;

	private bool _scrubbing;

	private bool _draggingKey;

	private bool _dragMoved;

	private int _dragTrack = -1;

	private double _dragOriginTime = -1.0;

	private double _dragTargetTime = -1.0;

	private bool _panningTime;

	public Animation BoundAnimation => _animation;

	public double Playhead => _playhead;

	public double TimeScale => _timeScale;

	public double VisibleTimeStart => _timeWindowStart;

	public int SelectedTrack => _selectedTrack;

	public double SelectedKeyTime => _selectedKeyTime;

	public int LastDrawVisitedTrackCount { get; private set; }

	public int LastDrawVisitedKeyCount { get; private set; }

	public int TimelineIndexRebuildCount { get; private set; }

	public event Action<double> PlayheadChanged;

	public event Action<int, double> KeySelected;

	public event Action<int, double, double> KeyMoveRequested;

	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Stop;
		FocusMode = FocusModeEnum.All;
		ClipContents = true;
		CustomMinimumSize = new Vector2(420f, 220f);
		if (_timelineIndexDirty && ShouldVisitTimeline())
		{
			EnsureTimelineIndex();
		}
		QueueTimelineRedraw();
	}

	public override void _Notification(int what)
	{
		base._Notification(what);
		if ((long)what == 31 && IsNodeReady())
		{
			if (ShouldVisitTimeline())
			{
				EnsureTimelineIndex();
				QueueTimelineRedraw();
			}
			else
			{
				CancelPointerOperation();
				LastDrawVisitedTrackCount = 0;
				LastDrawVisitedKeyCount = 0;
			}
		}
		else if ((long)what == 40 && IsNodeReady())
		{
			ClampViewport();
			QueueTimelineRedraw();
		}
		else if ((long)what == 1)
		{
			DisconnectAnimation();
		}
	}

	public void BindAnimation(Animation animation)
	{
		if (_animation == animation)
		{
			MarkTimelineIndexDirty();
			return;
		}
		DisconnectAnimation();
		_animation = animation;
		if (GodotObject.IsInstanceValid(_animation))
		{
			if (_animationChangedAction == null)
			{
				_animationChangedAction = OnAnimationChanged;
			}
			_animation.Changed += _animationChangedAction;
		}
		_selectedTrack = -1;
		_selectedKeyTime = -1.0;
		_playhead = 0.0;
		_timeWindowStart = 0.0;
		_trackScroll = 0f;
		MarkTimelineIndexDirty();
	}

	public void SetPlayhead(double time)
	{
		SetPlayheadInternal(time, notify: false);
	}

	public void SetTimeScale(double pixelsPerSecond)
	{
		double num = Math.Clamp(pixelsPerSecond, 24.0, 960.0);
		if (!(Math.Abs(num - _timeScale) <= 1E-06))
		{
			_timeScale = num;
			ClampViewport();
			QueueTimelineRedraw();
		}
	}

	public bool SelectKey(int trackIndex, double time)
	{
		EnsureTimelineIndex();
		TrackRecord trackRecord = FindTrack(trackIndex);
		if (trackRecord == null || FindNearestKey(trackRecord.KeyTimes, time, 0.0005) < 0)
		{
			return false;
		}
		_selectedTrack = trackIndex;
		_selectedKeyTime = time;
		QueueTimelineRedraw();
		return true;
	}

	public override void _GuiInput(InputEvent inputEvent)
	{
		if (!ShouldVisitTimeline())
		{
			return;
		}
		EnsureTimelineIndex();
		if (inputEvent is InputEventMouseButton inputEventMouseButton)
		{
			if (HandleWheel(inputEventMouseButton))
			{
				return;
			}
			if (inputEventMouseButton.ButtonIndex == MouseButton.Middle)
			{
				_panningTime = inputEventMouseButton.Pressed;
				if (inputEventMouseButton.Pressed)
				{
					GrabFocus();
				}
				AcceptEvent();
			}
			else if (inputEventMouseButton.ButtonIndex == MouseButton.Left)
			{
				if (inputEventMouseButton.Pressed)
				{
					BeginLeftPointerOperation(inputEventMouseButton.Position);
				}
				else
				{
					FinishLeftPointerOperation();
				}
				AcceptEvent();
			}
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion)
		{
			if (_panningTime)
			{
				_timeWindowStart = Math.Max(0.0, _timeWindowStart - (double)inputEventMouseMotion.Relative.X / _timeScale);
				ClampViewport();
				QueueTimelineRedraw();
				AcceptEvent();
			}
			else if (_draggingKey)
			{
				double val = XToTime(inputEventMouseMotion.Position.X);
				_dragTargetTime = Math.Max(0.0, val);
				_dragMoved |= Math.Abs(_dragTargetTime - _dragOriginTime) > 1E-06;
				QueueTimelineRedraw();
				AcceptEvent();
			}
			else if (_scrubbing)
			{
				SetPlayheadInternal(XToTime(inputEventMouseMotion.Position.X), notify: true);
				AcceptEvent();
			}
		}
	}

	public override void _Draw()
	{
		if (!ShouldVisitTimeline())
		{
			LastDrawVisitedTrackCount = 0;
			LastDrawVisitedKeyCount = 0;
			return;
		}
		EnsureTimelineIndex();
		LastDrawVisitedTrackCount = 0;
		LastDrawVisitedKeyCount = 0;
		DrawRect(new Rect2(Vector2.Zero, Size), new Color("111821"));
		DrawRect(new Rect2(238f, 0f, Math.Max(0f, Size.X - 238f), 34f), new Color("1b2930"));
		DrawTimeRulerAndGrid();
		DrawTracksAndKeys();
		DrawPlayhead();
		DrawFixedTrackRail();
		DrawRect(new Rect2(Vector2.Zero, Size), new Color("52705e"), filled: false, 1f);
	}

	private void DrawTimeRulerAndGrid()
	{
		if (Size.X <= 238f)
		{
			return;
		}
		double num = ChooseMajorTickStep(_timeScale);
		double num2 = num / 5.0;
		double num3 = XToTime(Size.X);
		double num4 = Math.Ceiling(_timeWindowStart / num2) * num2;
		Font themeDefaultFont = GetThemeDefaultFont();
		int fontSize = Math.Max(10, GetThemeDefaultFontSize() - 1);
		for (double num5 = num4; num5 <= num3 + 1E-06; num5 += num2)
		{
			float num6 = TimeToX(num5);
			double num7 = num5 / num;
			bool flag = Math.Abs(num7 - Math.Round(num7)) < 0.0001;
			Color color = (flag ? new Color("49605b99") : new Color("31443f66"));
			DrawLine(new Vector2(num6, flag ? 18f : 26f), new Vector2(num6, Size.Y), color, flag ? 1.2f : 1f);
			if (flag)
			{
				DrawString(themeDefaultFont, new Vector2(num6 + 4f, 17f), FormatSeconds(num5), HorizontalAlignment.Left, 82f, fontSize, new Color("c7d8c9"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			}
		}
		DrawLine(new Vector2(238f, 34f), new Vector2(Size.X, 34f), new Color("52665f"));
	}

	private void DrawTracksAndKeys()
	{
		if (_tracks.Count == 0)
		{
			DrawString(GetThemeDefaultFont(), new Vector2(262f, 76f), (_animation == null) ? "尚未绑定动画" : "动画中没有可显示的轨道", HorizontalAlignment.Left, Math.Max(1f, Size.X - 238f - 32f), GetThemeDefaultFontSize(), new Color("8fa39a"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			return;
		}
		GetVisibleRowRange(out var firstRow, out var lastRow);
		double timeWindowStart = _timeWindowStart;
		double num = XToTime(Size.X);
		for (int i = firstRow; i <= lastRow; i++)
		{
			TrackRecord trackRecord = _tracks[i];
			LastDrawVisitedTrackCount++;
			float num2 = TrackRowY(i);
			Color color = ((i % 2 == 0) ? new Color("172229") : new Color("141e25"));
			if (!trackRecord.Enabled)
			{
				color = color.Darkened(0.18f);
			}
			DrawRect(new Rect2(238f, num2, Math.Max(0f, Size.X - 238f), 42f), color);
			DrawLine(new Vector2(238f, num2 + 42f), new Vector2(Size.X, num2 + 42f), new Color("31423e99"));
			DrawTransitionSegments(trackRecord, num2 + 21f, timeWindowStart, num);
			for (int j = LowerBound(trackRecord.KeyTimes, timeWindowStart); j < trackRecord.KeyTimes.Length; j++)
			{
				double num3 = trackRecord.KeyTimes[j];
				if (num3 > num + 1E-06)
				{
					break;
				}
				LastDrawVisitedKeyCount++;
				DrawKey(trackRecord, num3, num2 + 21f);
			}
		}
		if (_draggingKey && _dragMoved && _dragTrack >= 0)
		{
			int num4 = FindTrackRow(_dragTrack);
			if (num4 >= firstRow && num4 <= lastRow)
			{
				Vector2 center = new Vector2(TimeToX(_dragTargetTime), TrackRowY(num4) + 21f);
				DrawDiamond(center, 8.5f, new Color("ffd66b66"), new Color("fff0ab"), 2f);
			}
		}
	}

	private void DrawKey(TrackRecord track, double time, float centerY)
	{
		float x = TimeToX(time);
		bool flag = track.TrackIndex == _selectedTrack && Math.Abs(time - _selectedKeyTime) <= 0.0005;
		Color fill = (track.Enabled ? track.Color : new Color("66716c"));
		if (flag)
		{
			fill = new Color("ffd55e");
		}
		DrawDiamond(new Vector2(x, centerY), 6.5f, fill, flag ? Colors.White : fill.Lightened(0.28f), flag ? 2f : 1.2f);
	}

	private void DrawTransitionSegments(TrackRecord track, float centerY, double visibleStart, double visibleEnd)
	{
		if (track.KeyTimes.Length < 2)
		{
			return;
		}
		int num = Math.Max(0, LowerBound(track.KeyTimes, visibleStart) - 1);
		Color color = (track.Enabled ? new Color(track.Color, 0.72f).Darkened(0.12f) : new Color("59635e88"));
		for (int i = num; i < track.KeyTimes.Length - 1; i++)
		{
			double num2 = track.KeyTimes[i];
			double num3 = track.KeyTimes[i + 1];
			if (num2 > visibleEnd + 1E-06)
			{
				break;
			}
			if (num3 < visibleStart - 1E-06)
			{
				continue;
			}
			float num4 = TimeToX(num2);
			float num5 = TimeToX(num3);
			if (num5 - num4 < 2f)
			{
				continue;
			}
			if (track.UpdateMode == Animation.UpdateMode.Discrete || track.Interpolation == Animation.InterpolationType.Nearest)
			{
				DrawLine(new Vector2(num4, centerY + 6f), new Vector2(num5, centerY + 6f), color, 1.6f, antialiased: true);
				DrawLine(new Vector2(num5, centerY + 6f), new Vector2(num5, centerY - 6f), color, 1.6f, antialiased: true);
				continue;
			}
			int num6 = Math.Clamp(Mathf.CeilToInt((num5 - num4) / 14f), 4, 18);
			Vector2[][] transitionPointBuffers = _transitionPointBuffers;
			int num7 = num6;
			Vector2[] array = transitionPointBuffers[num7] ?? (transitionPointBuffers[num7] = new Vector2[num6 + 1]);
			float curve = ((i < track.KeyTransitions.Length) ? track.KeyTransitions[i] : 1f);
			for (int j = 0; j <= num6; j++)
			{
				float num8 = (float)j / (float)num6;
				float num9 = Mathf.Ease(num8, curve);
				if (!float.IsFinite(num9))
				{
					num9 = num8;
				}
				array[j] = new Vector2(Mathf.Lerp(num4, num5, num8), Mathf.Lerp(centerY + 6f, centerY - 6f, Mathf.Clamp(num9, 0f, 1f)));
			}
			DrawPolyline(array, color, 1.6f, antialiased: true);
		}
	}

	private void DrawPlayhead()
	{
		float num = TimeToX(_playhead);
		if (!(num < 237f) && !(num > Size.X + 1f))
		{
			Color color = new Color("ff7958");
			DrawLine(new Vector2(num, 0f), new Vector2(num, Size.Y), color, 2f);
			DrawColoredPolygon(new Vector2[3]
			{
				new Vector2(num - 6f, 0f),
				new Vector2(num + 6f, 0f),
				new Vector2(num, 9f)
			}, color);
		}
	}

	private void DrawFixedTrackRail()
	{
		Font themeDefaultFont = GetThemeDefaultFont();
		int num = Math.Max(11, GetThemeDefaultFontSize());
		DrawRect(new Rect2(0f, 0f, 238f, Size.Y), new Color("17241f"));
		DrawRect(new Rect2(0f, 0f, 238f, 34f), new Color("21352a"));
		DrawString(themeDefaultFont, new Vector2(12f, 22f), "轨道 / 类型与状态", HorizontalAlignment.Left, 218f, num, new Color("d3e8c9"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		if (_tracks.Count == 0)
		{
			DrawLine(new Vector2(238f, 0f), new Vector2(238f, Size.Y), new Color("607468"));
			return;
		}
		GetVisibleRowRange(out var firstRow, out var lastRow);
		for (int i = firstRow; i <= lastRow; i++)
		{
			TrackRecord trackRecord = _tracks[i];
			float num2 = TrackRowY(i);
			Color color = ((i % 2 == 0) ? new Color("1c2d25") : new Color("192820"));
			DrawRect(new Rect2(0f, num2, 238f, 42f), color);
			DrawCircle(new Vector2(13f, num2 + 13f), 5f, trackRecord.Enabled ? new Color("71d47c") : new Color("6f7873"));
			DrawString(themeDefaultFont, new Vector2(25f, num2 + 17f), EllipsizeTrackPath(trackRecord.Path), HorizontalAlignment.Left, 207f, num, new Color("e0e9df"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			string value = (trackRecord.Enabled ? "启用" : "停用");
			Color value2 = (trackRecord.Enabled ? trackRecord.Color.Lightened(0.22f) : new Color("89938e"));
			string interpolationBadge = GetInterpolationBadge(trackRecord.Interpolation);
			string value3 = ((trackRecord.Type == Animation.TrackType.Value) ? (" · " + GetUpdateModeBadge(trackRecord.UpdateMode)) : "");
			string value4 = (trackRecord.InterpolationLoopWrap ? " · ↻" : "");
			DrawString(themeDefaultFont, new Vector2(25f, num2 + 35f), $"{trackRecord.TypeLabel} · {value} · {interpolationBadge}{value3}{value4}", HorizontalAlignment.Left, 207f, Math.Max(10, num - 2), value2, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			DrawLine(new Vector2(0f, num2 + 42f), new Vector2(238f, num2 + 42f), new Color("3b5045"));
		}
		DrawLine(new Vector2(238f, 0f), new Vector2(238f, Size.Y), new Color("607468"), 1.5f);
	}

	private bool HandleWheel(InputEventMouseButton button)
	{
		if (!button.Pressed || (button.ButtonIndex != MouseButton.WheelUp && button.ButtonIndex != MouseButton.WheelDown))
		{
			return false;
		}
		double num = ((button.ButtonIndex == MouseButton.WheelUp) ? 1.0 : (-1.0));
		double num2 = Math.Max(1.0, button.Factor);
		if (button.CtrlPressed)
		{
			double num3 = XToTime(button.Position.X);
			double num4 = Math.Pow(1.14, num * num2);
			double num5 = Math.Clamp(_timeScale * num4, 24.0, 960.0);
			if (button.Position.X >= 238f)
			{
				_timeWindowStart = Math.Max(0.0, num3 - (double)(button.Position.X - 238f) / num5);
			}
			_timeScale = num5;
		}
		else if (button.ShiftPressed)
		{
			_timeWindowStart = Math.Max(0.0, _timeWindowStart - num * num2 * 96.0 / _timeScale);
		}
		else
		{
			_trackScroll -= (float)(num * num2 * 42.0 * 2.0);
		}
		ClampViewport();
		QueueTimelineRedraw();
		AcceptEvent();
		return true;
	}

	private void BeginLeftPointerOperation(Vector2 position)
	{
		GrabFocus();
		if (!(position.X < 238f))
		{
			if (TryHitKey(position, out var track, out var keyTime))
			{
				_selectedTrack = track.TrackIndex;
				_selectedKeyTime = keyTime;
				_playhead = keyTime;
				_draggingKey = true;
				_dragMoved = false;
				_dragTrack = track.TrackIndex;
				_dragOriginTime = keyTime;
				_dragTargetTime = keyTime;
				KeySelected?.Invoke(track.TrackIndex, keyTime);
				PlayheadChanged?.Invoke(keyTime);
				QueueTimelineRedraw();
			}
			else
			{
				_scrubbing = true;
				SetPlayheadInternal(XToTime(position.X), notify: true);
			}
		}
	}

	private void FinishLeftPointerOperation()
	{
		if (_draggingKey && _dragMoved && _dragTrack >= 0 && Math.Abs(_dragTargetTime - _dragOriginTime) > 1E-06)
		{
			KeyMoveRequested?.Invoke(_dragTrack, _dragOriginTime, _dragTargetTime);
		}
		CancelPointerOperation();
		QueueTimelineRedraw();
	}

	private bool TryHitKey(Vector2 position, out TrackRecord track, out double keyTime)
	{
		track = null;
		keyTime = -1.0;
		int num = Mathf.FloorToInt((position.Y - 34f + _trackScroll) / 42f);
		if (num < 0 || num >= _tracks.Count)
		{
			return false;
		}
		float num2 = TrackRowY(num);
		if (position.Y < num2 || position.Y > num2 + 42f)
		{
			return false;
		}
		TrackRecord trackRecord = _tracks[num];
		double target = XToTime(position.X);
		double tolerance = 11.0 / _timeScale;
		int num3 = FindNearestKey(trackRecord.KeyTimes, target, tolerance);
		if (num3 < 0)
		{
			return false;
		}
		track = trackRecord;
		keyTime = trackRecord.KeyTimes[num3];
		return true;
	}

	private void SetPlayheadInternal(double time, bool notify)
	{
		double num = Math.Max(0.0, time);
		if (!(Math.Abs(num - _playhead) <= 1E-06))
		{
			_playhead = num;
			QueueTimelineRedraw();
			if (notify)
			{
				PlayheadChanged?.Invoke(_playhead);
			}
		}
	}

	private void OnAnimationChanged()
	{
		MarkTimelineIndexDirty();
	}

	private void MarkTimelineIndexDirty()
	{
		_timelineIndexDirty = true;
		if (ShouldVisitTimeline())
		{
			EnsureTimelineIndex();
		}
		QueueTimelineRedraw();
	}

	private void EnsureTimelineIndex()
	{
		if (_timelineIndexDirty)
		{
			_timelineIndexDirty = false;
			RebuildTimelineIndex();
		}
	}

	private void RebuildTimelineIndex()
	{
		_tracks.Clear();
		TimelineIndexRebuildCount++;
		if (!GodotObject.IsInstanceValid(_animation))
		{
			ClampViewport();
			return;
		}
		int trackCount = _animation.GetTrackCount();
		for (int i = 0; i < trackCount; i++)
		{
			try
			{
				Animation.TrackType trackType = _animation.TrackGetType(i);
				int val = _animation.TrackGetKeyCount(i);
				double[] array = new double[Math.Max(0, val)];
				float[] array2 = new float[Math.Max(0, val)];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = _animation.TrackGetKeyTime(i, j);
					array2[j] = _animation.TrackGetKeyTransition(i, j);
				}
				_tracks.Add(new TrackRecord
				{
					TrackIndex = i,
					Type = trackType,
					Enabled = _animation.TrackIsEnabled(i),
					Path = _animation.TrackGetPath(i).ToString(),
					TypeLabel = GetTrackTypeLabel(trackType),
					Color = GetTrackTypeColor(trackType),
					Interpolation = _animation.TrackGetInterpolationType(i),
					UpdateMode = ((trackType == Animation.TrackType.Value) ? _animation.ValueTrackGetUpdateMode(i) : Animation.UpdateMode.Continuous),
					InterpolationLoopWrap = _animation.TrackGetInterpolationLoopWrap(i),
					KeyTimes = array,
					KeyTransitions = array2
				});
			}
			catch (Exception ex)
			{
				GD.PushWarning($"2D 动画时间轴跳过无法读取的轨道 {i}: {ex.Message}");
			}
		}
		ClampViewport();
	}

	private void DisconnectAnimation()
	{
		if (GodotObject.IsInstanceValid(_animation) && _animationChangedAction != null)
		{
			_animation.Changed -= _animationChangedAction;
		}
	}

	private void ClampViewport()
	{
		float num = (float)_tracks.Count * 42f;
		float num2 = Math.Max(0f, Size.Y - 34f);
		_trackScroll = Math.Clamp(_trackScroll, 0f, Math.Max(0f, num - num2));
		double timelineEnd = GetTimelineEnd();
		double num3 = Math.Max(0.0, Size.X - 238f) / _timeScale;
		_timeWindowStart = Math.Clamp(_timeWindowStart, 0.0, Math.Max(0.0, timelineEnd - num3 * 0.1));
	}

	private double GetTimelineEnd()
	{
		double num = (GodotObject.IsInstanceValid(_animation) ? Math.Max(0.0, _animation.Length) : 0.0);
		foreach (TrackRecord track in _tracks)
		{
			if (track.KeyTimes.Length != 0)
			{
				num = Math.Max(num, track.KeyTimes[^1]);
			}
		}
		return Math.Max(1.0, num);
	}

	private void GetVisibleRowRange(out int firstRow, out int lastRow)
	{
		if (_tracks.Count == 0 || Size.Y <= 34f)
		{
			firstRow = 0;
			lastRow = -1;
		}
		else
		{
			firstRow = Math.Clamp(Mathf.FloorToInt(_trackScroll / 42f), 0, _tracks.Count - 1);
			int num = Mathf.CeilToInt((Size.Y - 34f) / 42f) + 1;
			lastRow = Math.Min(_tracks.Count - 1, firstRow + num);
		}
	}

	private float TrackRowY(int row)
	{
		return 34f + (float)row * 42f - _trackScroll;
	}

	private float TimeToX(double time)
	{
		return 238f + (float)((time - _timeWindowStart) * _timeScale);
	}

	private double XToTime(float x)
	{
		return Math.Max(0.0, _timeWindowStart + (double)(x - 238f) / _timeScale);
	}

	private TrackRecord FindTrack(int trackIndex)
	{
		foreach (TrackRecord track in _tracks)
		{
			if (track.TrackIndex == trackIndex)
			{
				return track;
			}
		}
		return null;
	}

	private int FindTrackRow(int trackIndex)
	{
		for (int i = 0; i < _tracks.Count; i++)
		{
			if (_tracks[i].TrackIndex == trackIndex)
			{
				return i;
			}
		}
		return -1;
	}

	private void CancelPointerOperation()
	{
		_scrubbing = false;
		_draggingKey = false;
		_dragMoved = false;
		_dragTrack = -1;
		_dragOriginTime = -1.0;
		_dragTargetTime = -1.0;
		_panningTime = false;
	}

	private bool ShouldVisitTimeline()
	{
		if (Visible)
		{
			if (IsInsideTree())
			{
				return IsVisibleInTree();
			}
			return true;
		}
		return false;
	}

	private void QueueTimelineRedraw()
	{
		if (IsInsideTree() && IsVisibleInTree())
		{
			QueueRedraw();
		}
	}

	private static int LowerBound(double[] values, double target)
	{
		int num = 0;
		int num2 = values.Length;
		while (num < num2)
		{
			int num3 = num + (num2 - num) / 2;
			if (values[num3] < target)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		return num;
	}

	private static int FindNearestKey(double[] values, double target, double tolerance)
	{
		int num = LowerBound(values, target);
		int result = -1;
		double num2 = tolerance + 1E-06;
		if (num < values.Length)
		{
			double num3 = Math.Abs(values[num] - target);
			if (num3 <= num2)
			{
				result = num;
				num2 = num3;
			}
		}
		if (num > 0 && Math.Abs(values[num - 1] - target) <= num2)
		{
			result = num - 1;
		}
		return result;
	}

	private static double ChooseMajorTickStep(double pixelsPerSecond)
	{
		double[] array = new double[10] { 0.05, 0.1, 0.2, 0.5, 1.0, 2.0, 5.0, 10.0, 20.0, 60.0 };
		double[] array2 = array;
		foreach (double num in array2)
		{
			if (num * pixelsPerSecond >= 72.0)
			{
				return num;
			}
		}
		return array[^1];
	}

	private static string FormatSeconds(double time)
	{
		if (!(time >= 10.0) && !(Math.Abs(time - Math.Round(time)) < 0.0001))
		{
			return $"{time:0.##}秒";
		}
		return $"{time:0}秒";
	}

	private static string EllipsizeTrackPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "未命名轨道";
		}
		if (path.Length > 27)
		{
			int length = path.Length;
			int num = length - 26;
			return "…" + path.Substring(num, length - num);
		}
		return path;
	}

	private static string GetTrackTypeLabel(Animation.TrackType type)
	{
		Animation.TrackType trackType = type;
		if ((ulong)trackType <= 8uL)
		{
			switch ((int)trackType)
			{
			case 0:
				return "数值 Value";
			case 6:
				return "曲线 Bezier";
			case 5:
				return "方法 Method";
			case 7:
				return "音频 Audio";
			case 8:
				return "动画 Animation";
			case 1:
				return "位置 Position3D";
			case 2:
				return "旋转 Rotation3D";
			case 3:
				return "缩放 Scale3D";
			case 4:
				return "混合形状 BlendShape";
			}
		}
		return type.ToString();
	}

	private static Color GetTrackTypeColor(Animation.TrackType type)
	{
		if ((ulong)type <= 8uL)
		{
			switch ((int)type)
			{
			case 0:
				return new Color("6fc3ff");
			case 6:
				return new Color("b68cff");
			case 5:
				return new Color("ffad63");
			case 7:
				return new Color("55d6af");
			case 8:
				return new Color("ff789e");
			case 1:
				return new Color("69c5df");
			case 2:
				return new Color("df9ddd");
			case 3:
				return new Color("98d36e");
			case 4:
				return new Color("e6ca66");
			}
		}
		return new Color("aab9b0");
	}

	private static string GetInterpolationBadge(Animation.InterpolationType interpolation)
	{
		Animation.InterpolationType interpolationType = interpolation;
		if ((ulong)interpolationType <= 4uL)
		{
			switch ((int)interpolationType)
			{
			case 0:
				return "▥保持";
			case 1:
				return "／线性";
			case 2:
				return "∿平滑";
			case 3:
				return "↻／";
			case 4:
				return "↻∿";
			}
		}
		return interpolation.ToString();
	}

	private static string GetUpdateModeBadge(Animation.UpdateMode updateMode)
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

	private void DrawDiamond(Vector2 center, float radius, Color fill, Color outline, float outlineWidth)
	{
		Vector2[] array = new Vector2[4]
		{
			new Vector2(center.X, center.Y - radius),
			new Vector2(center.X + radius, center.Y),
			new Vector2(center.X, center.Y + radius),
			new Vector2(center.X - radius, center.Y)
		};
		DrawColoredPolygon(array, fill);
		DrawPolyline(new Vector2[5]
		{
			array[0],
			array[1],
			array[2],
			array[3],
			array[0]
		}, outline, outlineWidth, antialiased: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(40)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetPlayhead, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetTimeScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "pixelsPerSecond", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectKey, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "trackIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawTimeRulerAndGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawTracksAndKeys, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawPlayhead, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawFixedTrackRail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleWheel, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseButton"), exported: false)
			}, null),
			new MethodInfo(MethodName.BeginLeftPointerOperation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishLeftPointerOperation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPlayheadInternal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "notify", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAnimationChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkTimelineIndexDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureTimelineIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildTimelineIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClampViewport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetTimelineEnd, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrackRowY, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "row", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TimeToX, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.XToTime, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "x", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindTrackRow, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "trackIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelPointerOperation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldVisitTimeline, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueTimelineRedraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LowerBound, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedFloat64Array, "values", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindNearestKey, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedFloat64Array, "values", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "tolerance", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChooseMajorTickStep, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "pixelsPerSecond", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatSeconds, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EllipsizeTrackPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTrackTypeLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTrackTypeColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetInterpolationBadge, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "interpolation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetUpdateModeBadge, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "updateMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawDiamond, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "center", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "radius", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "fill", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "outline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "outlineWidth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindAnimation && args.Count == 1)
		{
			BindAnimation(VariantUtils.ConvertTo<Animation>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPlayhead && args.Count == 1)
		{
			SetPlayhead(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTimeScale && args.Count == 1)
		{
			SetTimeScale(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SelectKey(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName._GuiInput && args.Count == 1)
		{
			_GuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawTimeRulerAndGrid && args.Count == 0)
		{
			DrawTimeRulerAndGrid();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawTracksAndKeys && args.Count == 0)
		{
			DrawTracksAndKeys();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawPlayhead && args.Count == 0)
		{
			DrawPlayhead();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawFixedTrackRail && args.Count == 0)
		{
			DrawFixedTrackRail();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleWheel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HandleWheel(VariantUtils.ConvertTo<InputEventMouseButton>(in args[0])));
			return true;
		}
		if (method == MethodName.BeginLeftPointerOperation && args.Count == 1)
		{
			BeginLeftPointerOperation(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishLeftPointerOperation && args.Count == 0)
		{
			FinishLeftPointerOperation();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPlayheadInternal && args.Count == 2)
		{
			SetPlayheadInternal(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationChanged && args.Count == 0)
		{
			OnAnimationChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkTimelineIndexDirty && args.Count == 0)
		{
			MarkTimelineIndexDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureTimelineIndex && args.Count == 0)
		{
			EnsureTimelineIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildTimelineIndex && args.Count == 0)
		{
			RebuildTimelineIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectAnimation && args.Count == 0)
		{
			DisconnectAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName.ClampViewport && args.Count == 0)
		{
			ClampViewport();
			ret = default;
			return true;
		}
		if (method == MethodName.GetTimelineEnd && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetTimelineEnd());
			return true;
		}
		if (method == MethodName.TrackRowY && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(TrackRowY(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.TimeToX && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(TimeToX(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.XToTime && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(XToTime(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.FindTrackRow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindTrackRow(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CancelPointerOperation && args.Count == 0)
		{
			CancelPointerOperation();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldVisitTimeline && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldVisitTimeline());
			return true;
		}
		if (method == MethodName.QueueTimelineRedraw && args.Count == 0)
		{
			QueueTimelineRedraw();
			ret = default;
			return true;
		}
		if (method == MethodName.LowerBound && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(LowerBound(VariantUtils.ConvertTo<double[]>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.FindNearestKey && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<int>(FindNearestKey(VariantUtils.ConvertTo<double[]>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ChooseMajorTickStep && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ChooseMajorTickStep(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatSeconds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatSeconds(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.EllipsizeTrackPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EllipsizeTrackPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTrackTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetTrackTypeLabel(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTrackTypeColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetTrackTypeColor(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetInterpolationBadge && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetInterpolationBadge(VariantUtils.ConvertTo<Animation.InterpolationType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetUpdateModeBadge && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetUpdateModeBadge(VariantUtils.ConvertTo<Animation.UpdateMode>(in args[0])));
			return true;
		}
		if (method == MethodName.DrawDiamond && args.Count == 5)
		{
			DrawDiamond(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LowerBound && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(LowerBound(VariantUtils.ConvertTo<double[]>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.FindNearestKey && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<int>(FindNearestKey(VariantUtils.ConvertTo<double[]>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ChooseMajorTickStep && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ChooseMajorTickStep(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatSeconds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatSeconds(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.EllipsizeTrackPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EllipsizeTrackPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTrackTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetTrackTypeLabel(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTrackTypeColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetTrackTypeColor(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetInterpolationBadge && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetInterpolationBadge(VariantUtils.ConvertTo<Animation.InterpolationType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetUpdateModeBadge && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetUpdateModeBadge(VariantUtils.ConvertTo<Animation.UpdateMode>(in args[0])));
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
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.BindAnimation)
		{
			return true;
		}
		if (method == MethodName.SetPlayhead)
		{
			return true;
		}
		if (method == MethodName.SetTimeScale)
		{
			return true;
		}
		if (method == MethodName.SelectKey)
		{
			return true;
		}
		if (method == MethodName._GuiInput)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.DrawTimeRulerAndGrid)
		{
			return true;
		}
		if (method == MethodName.DrawTracksAndKeys)
		{
			return true;
		}
		if (method == MethodName.DrawPlayhead)
		{
			return true;
		}
		if (method == MethodName.DrawFixedTrackRail)
		{
			return true;
		}
		if (method == MethodName.HandleWheel)
		{
			return true;
		}
		if (method == MethodName.BeginLeftPointerOperation)
		{
			return true;
		}
		if (method == MethodName.FinishLeftPointerOperation)
		{
			return true;
		}
		if (method == MethodName.SetPlayheadInternal)
		{
			return true;
		}
		if (method == MethodName.OnAnimationChanged)
		{
			return true;
		}
		if (method == MethodName.MarkTimelineIndexDirty)
		{
			return true;
		}
		if (method == MethodName.EnsureTimelineIndex)
		{
			return true;
		}
		if (method == MethodName.RebuildTimelineIndex)
		{
			return true;
		}
		if (method == MethodName.DisconnectAnimation)
		{
			return true;
		}
		if (method == MethodName.ClampViewport)
		{
			return true;
		}
		if (method == MethodName.GetTimelineEnd)
		{
			return true;
		}
		if (method == MethodName.TrackRowY)
		{
			return true;
		}
		if (method == MethodName.TimeToX)
		{
			return true;
		}
		if (method == MethodName.XToTime)
		{
			return true;
		}
		if (method == MethodName.FindTrackRow)
		{
			return true;
		}
		if (method == MethodName.CancelPointerOperation)
		{
			return true;
		}
		if (method == MethodName.ShouldVisitTimeline)
		{
			return true;
		}
		if (method == MethodName.QueueTimelineRedraw)
		{
			return true;
		}
		if (method == MethodName.LowerBound)
		{
			return true;
		}
		if (method == MethodName.FindNearestKey)
		{
			return true;
		}
		if (method == MethodName.ChooseMajorTickStep)
		{
			return true;
		}
		if (method == MethodName.FormatSeconds)
		{
			return true;
		}
		if (method == MethodName.EllipsizeTrackPath)
		{
			return true;
		}
		if (method == MethodName.GetTrackTypeLabel)
		{
			return true;
		}
		if (method == MethodName.GetTrackTypeColor)
		{
			return true;
		}
		if (method == MethodName.GetInterpolationBadge)
		{
			return true;
		}
		if (method == MethodName.GetUpdateModeBadge)
		{
			return true;
		}
		if (method == MethodName.DrawDiamond)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.LastDrawVisitedTrackCount)
		{
			LastDrawVisitedTrackCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.LastDrawVisitedKeyCount)
		{
			LastDrawVisitedKeyCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.TimelineIndexRebuildCount)
		{
			TimelineIndexRebuildCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animation)
		{
			_animation = VariantUtils.ConvertTo<Animation>(in value);
			return true;
		}
		if (name == PropertyName._timelineIndexDirty)
		{
			_timelineIndexDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._playhead)
		{
			_playhead = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._timeScale)
		{
			_timeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._timeWindowStart)
		{
			_timeWindowStart = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._trackScroll)
		{
			_trackScroll = VariantUtils.ConvertTo<float>(in value);
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
		if (name == PropertyName._scrubbing)
		{
			_scrubbing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._draggingKey)
		{
			_draggingKey = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragMoved)
		{
			_dragMoved = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragTrack)
		{
			_dragTrack = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._dragOriginTime)
		{
			_dragOriginTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._dragTargetTime)
		{
			_dragTargetTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._panningTime)
		{
			_panningTime = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.BoundAnimation)
		{
			value = VariantUtils.CreateFrom<Animation>(BoundAnimation);
			return true;
		}
		double from;
		if (name == PropertyName.Playhead)
		{
			from = Playhead;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TimeScale)
		{
			from = TimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.VisibleTimeStart)
		{
			from = VisibleTimeStart;
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
		if (name == PropertyName.SelectedKeyTime)
		{
			from = SelectedKeyTime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.LastDrawVisitedTrackCount)
		{
			from2 = LastDrawVisitedTrackCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.LastDrawVisitedKeyCount)
		{
			from2 = LastDrawVisitedKeyCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.TimelineIndexRebuildCount)
		{
			from2 = TimelineIndexRebuildCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._animation)
		{
			value = VariantUtils.CreateFrom(in _animation);
			return true;
		}
		if (name == PropertyName._timelineIndexDirty)
		{
			value = VariantUtils.CreateFrom(in _timelineIndexDirty);
			return true;
		}
		if (name == PropertyName._playhead)
		{
			value = VariantUtils.CreateFrom(in _playhead);
			return true;
		}
		if (name == PropertyName._timeScale)
		{
			value = VariantUtils.CreateFrom(in _timeScale);
			return true;
		}
		if (name == PropertyName._timeWindowStart)
		{
			value = VariantUtils.CreateFrom(in _timeWindowStart);
			return true;
		}
		if (name == PropertyName._trackScroll)
		{
			value = VariantUtils.CreateFrom(in _trackScroll);
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
		if (name == PropertyName._scrubbing)
		{
			value = VariantUtils.CreateFrom(in _scrubbing);
			return true;
		}
		if (name == PropertyName._draggingKey)
		{
			value = VariantUtils.CreateFrom(in _draggingKey);
			return true;
		}
		if (name == PropertyName._dragMoved)
		{
			value = VariantUtils.CreateFrom(in _dragMoved);
			return true;
		}
		if (name == PropertyName._dragTrack)
		{
			value = VariantUtils.CreateFrom(in _dragTrack);
			return true;
		}
		if (name == PropertyName._dragOriginTime)
		{
			value = VariantUtils.CreateFrom(in _dragOriginTime);
			return true;
		}
		if (name == PropertyName._dragTargetTime)
		{
			value = VariantUtils.CreateFrom(in _dragTargetTime);
			return true;
		}
		if (name == PropertyName._panningTime)
		{
			value = VariantUtils.CreateFrom(in _panningTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._animation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._timelineIndexDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._playhead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._timeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._timeWindowStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._trackScroll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedTrack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._selectedKeyTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._scrubbing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._draggingKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dragMoved, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._dragTrack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._dragOriginTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._dragTargetTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._panningTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.BoundAnimation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.Playhead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.TimeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.VisibleTimeStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedTrack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.SelectedKeyTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LastDrawVisitedTrackCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LastDrawVisitedKeyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TimelineIndexRebuildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.LastDrawVisitedTrackCount, Variant.From<int>(LastDrawVisitedTrackCount));
		info.AddProperty(PropertyName.LastDrawVisitedKeyCount, Variant.From<int>(LastDrawVisitedKeyCount));
		info.AddProperty(PropertyName.TimelineIndexRebuildCount, Variant.From<int>(TimelineIndexRebuildCount));
		info.AddProperty(PropertyName._animation, Variant.From(in _animation));
		info.AddProperty(PropertyName._timelineIndexDirty, Variant.From(in _timelineIndexDirty));
		info.AddProperty(PropertyName._playhead, Variant.From(in _playhead));
		info.AddProperty(PropertyName._timeScale, Variant.From(in _timeScale));
		info.AddProperty(PropertyName._timeWindowStart, Variant.From(in _timeWindowStart));
		info.AddProperty(PropertyName._trackScroll, Variant.From(in _trackScroll));
		info.AddProperty(PropertyName._selectedTrack, Variant.From(in _selectedTrack));
		info.AddProperty(PropertyName._selectedKeyTime, Variant.From(in _selectedKeyTime));
		info.AddProperty(PropertyName._scrubbing, Variant.From(in _scrubbing));
		info.AddProperty(PropertyName._draggingKey, Variant.From(in _draggingKey));
		info.AddProperty(PropertyName._dragMoved, Variant.From(in _dragMoved));
		info.AddProperty(PropertyName._dragTrack, Variant.From(in _dragTrack));
		info.AddProperty(PropertyName._dragOriginTime, Variant.From(in _dragOriginTime));
		info.AddProperty(PropertyName._dragTargetTime, Variant.From(in _dragTargetTime));
		info.AddProperty(PropertyName._panningTime, Variant.From(in _panningTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.LastDrawVisitedTrackCount, out var value))
		{
			LastDrawVisitedTrackCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.LastDrawVisitedKeyCount, out var value2))
		{
			LastDrawVisitedKeyCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.TimelineIndexRebuildCount, out var value3))
		{
			TimelineIndexRebuildCount = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animation, out var value4))
		{
			_animation = value4.As<Animation>();
		}
		if (info.TryGetProperty(PropertyName._timelineIndexDirty, out var value5))
		{
			_timelineIndexDirty = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._playhead, out var value6))
		{
			_playhead = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._timeScale, out var value7))
		{
			_timeScale = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._timeWindowStart, out var value8))
		{
			_timeWindowStart = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._trackScroll, out var value9))
		{
			_trackScroll = value9.As<float>();
		}
		if (info.TryGetProperty(PropertyName._selectedTrack, out var value10))
		{
			_selectedTrack = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedKeyTime, out var value11))
		{
			_selectedKeyTime = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName._scrubbing, out var value12))
		{
			_scrubbing = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._draggingKey, out var value13))
		{
			_draggingKey = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragMoved, out var value14))
		{
			_dragMoved = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragTrack, out var value15))
		{
			_dragTrack = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName._dragOriginTime, out var value16))
		{
			_dragOriginTime = value16.As<double>();
		}
		if (info.TryGetProperty(PropertyName._dragTargetTime, out var value17))
		{
			_dragTargetTime = value17.As<double>();
		}
		if (info.TryGetProperty(PropertyName._panningTime, out var value18))
		{
			_panningTime = value18.As<bool>();
		}
	}
}
