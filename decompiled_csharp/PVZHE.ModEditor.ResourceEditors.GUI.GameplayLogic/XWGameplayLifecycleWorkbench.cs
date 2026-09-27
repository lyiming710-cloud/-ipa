using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/GameplayLogic/XWGameplayLifecycleWorkbench.cs")]
public class XWGameplayLifecycleWorkbench : PanelContainer
{
	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Bind = "Bind";

		public static readonly StringName Unbind = "Unbind";

		public static readonly StringName TogglePlayback = "TogglePlayback";

		public static readonly StringName StepForward = "StepForward";

		public static readonly StringName ResetPreview = "ResetPreview";

		public static readonly StringName ConnectSignals = "ConnectSignals";

		public static readonly StringName DisconnectSignals = "DisconnectSignals";

		public static readonly StringName GoReady = "GoReady";

		public static readonly StringName GoBattle = "GoBattle";

		public static readonly StringName GoSettle = "GoSettle";

		public static readonly StringName OnScrubberChanged = "OnScrubberChanged";

		public static readonly StringName OnPreviewTick = "OnPreviewTick";

		public static readonly StringName OnVisibilityChanged = "OnVisibilityChanged";

		public static readonly StringName StopPlayback = "StopPlayback";

		public static readonly StringName SetPreviewTime = "SetPreviewTime";

		public static readonly StringName UpdateLabels = "UpdateLabels";

		public static readonly StringName UpdatePlayButton = "UpdatePlayButton";

		public static readonly StringName RebuildHistory = "RebuildHistory";

		public static readonly StringName FormatTime = "FormatTime";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName PreviewTime = "PreviewTime";

		public static readonly StringName IsPlaying = "IsPlaying";

		public static readonly StringName PhaseName = "PhaseName";

		public static readonly StringName _resource = "_resource";

		public static readonly StringName _readyButton = "_readyButton";

		public static readonly StringName _battleButton = "_battleButton";

		public static readonly StringName _settleButton = "_settleButton";

		public static readonly StringName _playPauseButton = "_playPauseButton";

		public static readonly StringName _stepButton = "_stepButton";

		public static readonly StringName _resetButton = "_resetButton";

		public static readonly StringName _phaseBadge = "_phaseBadge";

		public static readonly StringName _timeLabel = "_timeLabel";

		public static readonly StringName _summaryLabel = "_summaryLabel";

		public static readonly StringName _scrubber = "_scrubber";

		public static readonly StringName _historyRoot = "_historyRoot";

		public static readonly StringName _canvas = "_canvas";

		public static readonly StringName _timer = "_timer";

		public static readonly StringName _previewTime = "_previewTime";

		public static readonly StringName _syncingScrubber = "_syncingScrubber";

		public static readonly StringName _signalsConnected = "_signalsConnected";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private const double TickSeconds = 0.1;

	private XWGameplayLifecyclePlan.Snapshot _snapshot;

	private Resource _resource;

	private Button _readyButton;

	private Button _battleButton;

	private Button _settleButton;

	private Button _playPauseButton;

	private Button _stepButton;

	private Button _resetButton;

	private Label _phaseBadge;

	private Label _timeLabel;

	private Label _summaryLabel;

	private HSlider _scrubber;

	private VBoxContainer _historyRoot;

	private XWGameplayLifecycleTimelineCanvas _canvas;

	private Timer _timer;

	private double _previewTime;

	private bool _syncingScrubber;

	private bool _signalsConnected;

	public double PreviewTime => _previewTime;

	public bool IsPlaying
	{
		get
		{
			if (GodotObject.IsInstanceValid(_timer))
			{
				return !_timer.IsStopped();
			}
			return false;
		}
	}

	public string PhaseName => XWGameplayLifecyclePlan.PhaseAt(_snapshot, _previewTime).ToString();

	public override void _Ready()
	{
		_readyButton = GetNode<Button>("%LifecycleReadyButton");
		_battleButton = GetNode<Button>("%LifecycleBattleButton");
		_settleButton = GetNode<Button>("%LifecycleSettleButton");
		_playPauseButton = GetNode<Button>("%PlayPauseButton");
		_stepButton = GetNode<Button>("%StepButton");
		_resetButton = GetNode<Button>("%ResetButton");
		_phaseBadge = GetNode<Label>("%LifecyclePhaseBadge");
		_timeLabel = GetNode<Label>("%LifecycleTimeLabel");
		_summaryLabel = GetNode<Label>("%LifecycleSummaryLabel");
		_scrubber = GetNode<HSlider>("%LifecycleScrubber");
		_historyRoot = GetNode<VBoxContainer>("%LifecycleHistoryRoot");
		_canvas = GetNode<XWGameplayLifecycleTimelineCanvas>("%LifecycleTimelineCanvas");
		_timer = GetNode<Timer>("%LifecyclePreviewTimer");
		ConnectSignals();
		SetProcess(enable: false);
		Bind(null);
	}

	public override void _ExitTree()
	{
		StopPlayback();
		DisconnectSignals();
		_resource = null;
		_snapshot = null;
	}

	public void Bind(Resource resource)
	{
		StopPlayback();
		_resource = (GodotObject.IsInstanceValid(resource) ? resource : null);
		_snapshot = XWGameplayLifecyclePlan.Build(_resource);
		if (GodotObject.IsInstanceValid(_summaryLabel))
		{
			_summaryLabel.Text = _snapshot.Summary;
		}
		if (GodotObject.IsInstanceValid(_scrubber))
		{
			_scrubber.MaxValue = Math.Max(0.01, _snapshot.TotalDuration);
		}
		SetPreviewTime(0.0);
	}

	public void Unbind()
	{
		StopPlayback();
		_resource = null;
		_snapshot = XWGameplayLifecyclePlan.Build(null);
		SetPreviewTime(0.0);
	}

	public void TogglePlayback()
	{
		if (!GodotObject.IsInstanceValid(_timer) || _snapshot == null || !IsVisibleInTree())
		{
			return;
		}
		if (IsPlaying)
		{
			StopPlayback();
			return;
		}
		if (_previewTime >= _snapshot.TotalDuration)
		{
			SetPreviewTime(0.0);
		}
		_timer.Start();
		UpdatePlayButton();
	}

	public void StepForward()
	{
		StopPlayback();
		if (_snapshot == null)
		{
			return;
		}
		double previewTime = Math.Min(_snapshot.TotalDuration, _previewTime + 1.0);
		foreach (XWGameplayLifecyclePlan.Entry entry in _snapshot.Entries)
		{
			if (entry.Time > _previewTime + 0.001)
			{
				previewTime = entry.Time;
				break;
			}
		}
		SetPreviewTime(previewTime);
	}

	public void ResetPreview()
	{
		StopPlayback();
		SetPreviewTime(0.0);
	}

	private void ConnectSignals()
	{
		if (!_signalsConnected)
		{
			_readyButton.Pressed += GoReady;
			_battleButton.Pressed += GoBattle;
			_settleButton.Pressed += GoSettle;
			_playPauseButton.Pressed += TogglePlayback;
			_stepButton.Pressed += StepForward;
			_resetButton.Pressed += ResetPreview;
			_scrubber.ValueChanged += OnScrubberChanged;
			_timer.Timeout += OnPreviewTick;
			VisibilityChanged += OnVisibilityChanged;
			_signalsConnected = true;
		}
	}

	private void DisconnectSignals()
	{
		if (_signalsConnected)
		{
			if (GodotObject.IsInstanceValid(_readyButton))
			{
				_readyButton.Pressed -= GoReady;
			}
			if (GodotObject.IsInstanceValid(_battleButton))
			{
				_battleButton.Pressed -= GoBattle;
			}
			if (GodotObject.IsInstanceValid(_settleButton))
			{
				_settleButton.Pressed -= GoSettle;
			}
			if (GodotObject.IsInstanceValid(_playPauseButton))
			{
				_playPauseButton.Pressed -= TogglePlayback;
			}
			if (GodotObject.IsInstanceValid(_stepButton))
			{
				_stepButton.Pressed -= StepForward;
			}
			if (GodotObject.IsInstanceValid(_resetButton))
			{
				_resetButton.Pressed -= ResetPreview;
			}
			if (GodotObject.IsInstanceValid(_scrubber))
			{
				_scrubber.ValueChanged -= OnScrubberChanged;
			}
			if (GodotObject.IsInstanceValid(_timer))
			{
				_timer.Timeout -= OnPreviewTick;
			}
			VisibilityChanged -= OnVisibilityChanged;
			_signalsConnected = false;
		}
	}

	private void GoReady()
	{
		StopPlayback();
		SetPreviewTime(0.0);
	}

	private void GoBattle()
	{
		StopPlayback();
		SetPreviewTime(_snapshot?.BattleStart ?? 0.0);
	}

	private void GoSettle()
	{
		StopPlayback();
		SetPreviewTime(_snapshot?.SettleStart ?? 0.0);
	}

	private void OnScrubberChanged(double value)
	{
		if (!_syncingScrubber)
		{
			StopPlayback();
			SetPreviewTime(value);
		}
	}

	private void OnPreviewTick()
	{
		if (!IsVisibleInTree() || _snapshot == null)
		{
			StopPlayback();
			return;
		}
		double num = Math.Min(_snapshot.TotalDuration, _previewTime + 0.1);
		SetPreviewTime(num);
		if (num >= _snapshot.TotalDuration)
		{
			StopPlayback();
		}
	}

	private void OnVisibilityChanged()
	{
		if (!IsVisibleInTree())
		{
			StopPlayback();
		}
	}

	private void StopPlayback()
	{
		if (GodotObject.IsInstanceValid(_timer))
		{
			_timer.Stop();
		}
		UpdatePlayButton();
	}

	private void SetPreviewTime(double value)
	{
		if (_snapshot != null)
		{
			_previewTime = Math.Clamp(double.IsFinite(value) ? value : 0.0, 0.0, _snapshot.TotalDuration);
			_syncingScrubber = true;
			if (GodotObject.IsInstanceValid(_scrubber))
			{
				_scrubber.Value = _previewTime;
			}
			_syncingScrubber = false;
			UpdateLabels();
			_canvas?.SetPlan(_snapshot, _previewTime);
			RebuildHistory();
		}
	}

	private void UpdateLabels()
	{
		XWGameplayLifecyclePlan.Phase phase = XWGameplayLifecyclePlan.PhaseAt(_snapshot, _previewTime);
		if (GodotObject.IsInstanceValid(_readyButton))
		{
			_readyButton.ButtonPressed = phase == XWGameplayLifecyclePlan.Phase.Ready;
		}
		if (GodotObject.IsInstanceValid(_battleButton))
		{
			_battleButton.ButtonPressed = phase == XWGameplayLifecyclePlan.Phase.Battle;
		}
		if (GodotObject.IsInstanceValid(_settleButton))
		{
			_settleButton.ButtonPressed = phase == XWGameplayLifecyclePlan.Phase.Settle;
		}
		if (GodotObject.IsInstanceValid(_phaseBadge))
		{
			_phaseBadge.Text = phase.ToString().ToUpperInvariant();
			Label phaseBadge = _phaseBadge;
			StringName name = "font_color";
			phaseBadge.AddThemeColorOverride(name, phase switch
			{
				XWGameplayLifecyclePlan.Phase.Ready => new Color("8ed4ff"), 
				XWGameplayLifecyclePlan.Phase.Battle => new Color("91eda2"), 
				_ => new Color("ffc17d"), 
			});
		}
		if (GodotObject.IsInstanceValid(_timeLabel))
		{
			_timeLabel.Text = FormatTime(_previewTime) + " / " + FormatTime(_snapshot.TotalDuration);
		}
	}

	private void UpdatePlayButton()
	{
		if (GodotObject.IsInstanceValid(_playPauseButton))
		{
			_playPauseButton.Text = (IsPlaying ? "Ⅱ  暂停" : "▶  播放");
		}
	}

	private void RebuildHistory()
	{
		if (!GodotObject.IsInstanceValid(_historyRoot) || _snapshot == null)
		{
			return;
		}
		foreach (Node child in _historyRoot.GetChildren())
		{
			_historyRoot.RemoveChild(child);
			child.QueueFree();
		}
		int num = 0;
		foreach (XWGameplayLifecyclePlan.Entry entry in _snapshot.Entries)
		{
			if (!(entry.Time > _previewTime + 0.001) && num++ < 32)
			{
				Label label = new Label
				{
					Text = $"{FormatTime(entry.Time)}  {entry.Title}\n{entry.Detail}",
					AutowrapMode = TextServer.AutowrapMode.WordSmart,
					TooltipText = $"{entry.Phase} · 沙盘记录，不是运行时事件"
				};
				Label label2 = label;
				StringName name = "font_color";
				label2.AddThemeColorOverride(name, entry.Phase switch
				{
					XWGameplayLifecyclePlan.Phase.Ready => new Color("9ed8f5"), 
					XWGameplayLifecyclePlan.Phase.Battle => new Color("a6e8ae"), 
					_ => new Color("f4c18c"), 
				});
				_historyRoot.AddChild(label, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private static string FormatTime(double seconds)
	{
		int num = Math.Max(0, (int)Math.Round(seconds));
		return $"{num / 60:00}:{num % 60:00}";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Unbind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TogglePlayback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StepForward, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GoReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GoBattle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GoSettle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnScrubberChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPreviewTick, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopPlayback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPreviewTime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateLabels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePlayButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatTime, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "seconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Bind && args.Count == 1)
		{
			Bind(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Unbind && args.Count == 0)
		{
			Unbind();
			ret = default;
			return true;
		}
		if (method == MethodName.TogglePlayback && args.Count == 0)
		{
			TogglePlayback();
			ret = default;
			return true;
		}
		if (method == MethodName.StepForward && args.Count == 0)
		{
			StepForward();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetPreview && args.Count == 0)
		{
			ResetPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectSignals && args.Count == 0)
		{
			ConnectSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectSignals && args.Count == 0)
		{
			DisconnectSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.GoReady && args.Count == 0)
		{
			GoReady();
			ret = default;
			return true;
		}
		if (method == MethodName.GoBattle && args.Count == 0)
		{
			GoBattle();
			ret = default;
			return true;
		}
		if (method == MethodName.GoSettle && args.Count == 0)
		{
			GoSettle();
			ret = default;
			return true;
		}
		if (method == MethodName.OnScrubberChanged && args.Count == 1)
		{
			OnScrubberChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPreviewTick && args.Count == 0)
		{
			OnPreviewTick();
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisibilityChanged && args.Count == 0)
		{
			OnVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.StopPlayback && args.Count == 0)
		{
			StopPlayback();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPreviewTime && args.Count == 1)
		{
			SetPreviewTime(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateLabels && args.Count == 0)
		{
			UpdateLabels();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePlayButton && args.Count == 0)
		{
			UpdatePlayButton();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildHistory && args.Count == 0)
		{
			RebuildHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.FormatTime && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatTime(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FormatTime && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatTime(VariantUtils.ConvertTo<double>(in args[0])));
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
		if (method == MethodName.Bind)
		{
			return true;
		}
		if (method == MethodName.Unbind)
		{
			return true;
		}
		if (method == MethodName.TogglePlayback)
		{
			return true;
		}
		if (method == MethodName.StepForward)
		{
			return true;
		}
		if (method == MethodName.ResetPreview)
		{
			return true;
		}
		if (method == MethodName.ConnectSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectSignals)
		{
			return true;
		}
		if (method == MethodName.GoReady)
		{
			return true;
		}
		if (method == MethodName.GoBattle)
		{
			return true;
		}
		if (method == MethodName.GoSettle)
		{
			return true;
		}
		if (method == MethodName.OnScrubberChanged)
		{
			return true;
		}
		if (method == MethodName.OnPreviewTick)
		{
			return true;
		}
		if (method == MethodName.OnVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.StopPlayback)
		{
			return true;
		}
		if (method == MethodName.SetPreviewTime)
		{
			return true;
		}
		if (method == MethodName.UpdateLabels)
		{
			return true;
		}
		if (method == MethodName.UpdatePlayButton)
		{
			return true;
		}
		if (method == MethodName.RebuildHistory)
		{
			return true;
		}
		if (method == MethodName.FormatTime)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._resource)
		{
			_resource = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._readyButton)
		{
			_readyButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._battleButton)
		{
			_battleButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._settleButton)
		{
			_settleButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._playPauseButton)
		{
			_playPauseButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stepButton)
		{
			_stepButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._resetButton)
		{
			_resetButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._phaseBadge)
		{
			_phaseBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._timeLabel)
		{
			_timeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			_summaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._scrubber)
		{
			_scrubber = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._historyRoot)
		{
			_historyRoot = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._canvas)
		{
			_canvas = VariantUtils.ConvertTo<XWGameplayLifecycleTimelineCanvas>(in value);
			return true;
		}
		if (name == PropertyName._timer)
		{
			_timer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._previewTime)
		{
			_previewTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._syncingScrubber)
		{
			_syncingScrubber = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._signalsConnected)
		{
			_signalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.PreviewTime)
		{
			value = VariantUtils.CreateFrom<double>(PreviewTime);
			return true;
		}
		if (name == PropertyName.IsPlaying)
		{
			value = VariantUtils.CreateFrom<bool>(IsPlaying);
			return true;
		}
		if (name == PropertyName.PhaseName)
		{
			value = VariantUtils.CreateFrom<string>(PhaseName);
			return true;
		}
		if (name == PropertyName._resource)
		{
			value = VariantUtils.CreateFrom(in _resource);
			return true;
		}
		if (name == PropertyName._readyButton)
		{
			value = VariantUtils.CreateFrom(in _readyButton);
			return true;
		}
		if (name == PropertyName._battleButton)
		{
			value = VariantUtils.CreateFrom(in _battleButton);
			return true;
		}
		if (name == PropertyName._settleButton)
		{
			value = VariantUtils.CreateFrom(in _settleButton);
			return true;
		}
		if (name == PropertyName._playPauseButton)
		{
			value = VariantUtils.CreateFrom(in _playPauseButton);
			return true;
		}
		if (name == PropertyName._stepButton)
		{
			value = VariantUtils.CreateFrom(in _stepButton);
			return true;
		}
		if (name == PropertyName._resetButton)
		{
			value = VariantUtils.CreateFrom(in _resetButton);
			return true;
		}
		if (name == PropertyName._phaseBadge)
		{
			value = VariantUtils.CreateFrom(in _phaseBadge);
			return true;
		}
		if (name == PropertyName._timeLabel)
		{
			value = VariantUtils.CreateFrom(in _timeLabel);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			value = VariantUtils.CreateFrom(in _summaryLabel);
			return true;
		}
		if (name == PropertyName._scrubber)
		{
			value = VariantUtils.CreateFrom(in _scrubber);
			return true;
		}
		if (name == PropertyName._historyRoot)
		{
			value = VariantUtils.CreateFrom(in _historyRoot);
			return true;
		}
		if (name == PropertyName._canvas)
		{
			value = VariantUtils.CreateFrom(in _canvas);
			return true;
		}
		if (name == PropertyName._timer)
		{
			value = VariantUtils.CreateFrom(in _timer);
			return true;
		}
		if (name == PropertyName._previewTime)
		{
			value = VariantUtils.CreateFrom(in _previewTime);
			return true;
		}
		if (name == PropertyName._syncingScrubber)
		{
			value = VariantUtils.CreateFrom(in _syncingScrubber);
			return true;
		}
		if (name == PropertyName._signalsConnected)
		{
			value = VariantUtils.CreateFrom(in _signalsConnected);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._resource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._readyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._battleButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._settleButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._playPauseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stepButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resetButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._phaseBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._timeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._summaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scrubber, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._historyRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._canvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._previewTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._syncingScrubber, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._signalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.PreviewTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.PhaseName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._resource, Variant.From(in _resource));
		info.AddProperty(PropertyName._readyButton, Variant.From(in _readyButton));
		info.AddProperty(PropertyName._battleButton, Variant.From(in _battleButton));
		info.AddProperty(PropertyName._settleButton, Variant.From(in _settleButton));
		info.AddProperty(PropertyName._playPauseButton, Variant.From(in _playPauseButton));
		info.AddProperty(PropertyName._stepButton, Variant.From(in _stepButton));
		info.AddProperty(PropertyName._resetButton, Variant.From(in _resetButton));
		info.AddProperty(PropertyName._phaseBadge, Variant.From(in _phaseBadge));
		info.AddProperty(PropertyName._timeLabel, Variant.From(in _timeLabel));
		info.AddProperty(PropertyName._summaryLabel, Variant.From(in _summaryLabel));
		info.AddProperty(PropertyName._scrubber, Variant.From(in _scrubber));
		info.AddProperty(PropertyName._historyRoot, Variant.From(in _historyRoot));
		info.AddProperty(PropertyName._canvas, Variant.From(in _canvas));
		info.AddProperty(PropertyName._timer, Variant.From(in _timer));
		info.AddProperty(PropertyName._previewTime, Variant.From(in _previewTime));
		info.AddProperty(PropertyName._syncingScrubber, Variant.From(in _syncingScrubber));
		info.AddProperty(PropertyName._signalsConnected, Variant.From(in _signalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._resource, out var value))
		{
			_resource = value.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._readyButton, out var value2))
		{
			_readyButton = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._battleButton, out var value3))
		{
			_battleButton = value3.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._settleButton, out var value4))
		{
			_settleButton = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._playPauseButton, out var value5))
		{
			_playPauseButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stepButton, out var value6))
		{
			_stepButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._resetButton, out var value7))
		{
			_resetButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._phaseBadge, out var value8))
		{
			_phaseBadge = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._timeLabel, out var value9))
		{
			_timeLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._summaryLabel, out var value10))
		{
			_summaryLabel = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._scrubber, out var value11))
		{
			_scrubber = value11.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._historyRoot, out var value12))
		{
			_historyRoot = value12.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._canvas, out var value13))
		{
			_canvas = value13.As<XWGameplayLifecycleTimelineCanvas>();
		}
		if (info.TryGetProperty(PropertyName._timer, out var value14))
		{
			_timer = value14.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._previewTime, out var value15))
		{
			_previewTime = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName._syncingScrubber, out var value16))
		{
			_syncingScrubber = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._signalsConnected, out var value17))
		{
			_signalsConnected = value17.As<bool>();
		}
	}
}
