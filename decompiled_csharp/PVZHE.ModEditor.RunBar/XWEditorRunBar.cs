using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.RunBar;

[ScriptPath("res://addons/ModEditor/RunBar/XWEditorRunBar.cs")]
public class XWEditorRunBar : HBoxContainer
{
	public enum RunMode
	{
		Stopped,
		RunMain,
		RunCurrent,
		RunCustom
	}

	[Signal]
	public delegate void PlayPressedEventHandler();

	[Signal]
	public delegate void PlayScenePressedEventHandler();

	[Signal]
	public delegate void StopPressedEventHandler();

	[Signal]
	public delegate void PauseToggledEventHandler(bool isPaused);

	[Signal]
	public delegate void PlayCustomPressedEventHandler();

	[Signal]
	public delegate void ProfilerIndicatorPressedEventHandler();

	[Signal]
	public delegate void RunStatusChangedEventHandler(bool running, bool paused, int mode);

	public new class MethodName : HBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitRemoteMenu = "InitRemoteMenu";

		public static readonly StringName OnPlayPressed = "OnPlayPressed";

		public static readonly StringName OnPlayScenePressed = "OnPlayScenePressed";

		public static readonly StringName OnPlayCustomPressed = "OnPlayCustomPressed";

		public static readonly StringName OnPauseToggled = "OnPauseToggled";

		public static readonly StringName SetRunning = "SetRunning";

		public static readonly StringName SetRunMode = "SetRunMode";

		public static readonly StringName SetPaused = "SetPaused";

		public static readonly StringName IsRunning = "IsRunning";

		public static readonly StringName IsPaused = "IsPaused";

		public static readonly StringName GetRunMode = "GetRunMode";

		public static readonly StringName ShowProfilerIndicator = "ShowProfilerIndicator";

		public static readonly StringName SetProfilerText = "SetProfilerText";

		public static readonly StringName UpdateUI = "UpdateUI";
	}

	public new class PropertyName : HBoxContainer.PropertyName
	{
		public static readonly StringName _runMode = "_runMode";

		public static readonly StringName _isPaused = "_isPaused";

		public static readonly StringName _updatingUi = "_updatingUi";

		public static readonly StringName _profilerIndicator = "_profilerIndicator";

		public static readonly StringName _playBtn = "_playBtn";

		public static readonly StringName _pauseBtn = "_pauseBtn";

		public static readonly StringName _stopBtn = "_stopBtn";

		public static readonly StringName _remoteBtn = "_remoteBtn";

		public static readonly StringName _playSceneBtn = "_playSceneBtn";

		public static readonly StringName _playCustomBtn = "_playCustomBtn";
	}

	public new class SignalName : HBoxContainer.SignalName
	{
		public static readonly StringName PlayPressed = "PlayPressed";

		public static readonly StringName PlayScenePressed = "PlayScenePressed";

		public static readonly StringName StopPressed = "StopPressed";

		public static readonly StringName PauseToggled = "PauseToggled";

		public static readonly StringName PlayCustomPressed = "PlayCustomPressed";

		public static readonly StringName ProfilerIndicatorPressed = "ProfilerIndicatorPressed";

		public static readonly StringName RunStatusChanged = "RunStatusChanged";
	}

	private RunMode _runMode;

	private bool _isPaused;

	private bool _updatingUi;

	private Button _profilerIndicator;

	private Button _playBtn;

	private Button _pauseBtn;

	private Button _stopBtn;

	private MenuButton _remoteBtn;

	private Button _playSceneBtn;

	private Button _playCustomBtn;

	private PlayPressedEventHandler backing_PlayPressed;

	private PlayScenePressedEventHandler backing_PlayScenePressed;

	private StopPressedEventHandler backing_StopPressed;

	private PauseToggledEventHandler backing_PauseToggled;

	private PlayCustomPressedEventHandler backing_PlayCustomPressed;

	private ProfilerIndicatorPressedEventHandler backing_ProfilerIndicatorPressed;

	private RunStatusChangedEventHandler backing_RunStatusChanged;

	public event PlayPressedEventHandler PlayPressed
	{
		add
		{
			backing_PlayPressed = (PlayPressedEventHandler)Delegate.Combine(backing_PlayPressed, value);
		}
		remove
		{
			backing_PlayPressed = (PlayPressedEventHandler)Delegate.Remove(backing_PlayPressed, value);
		}
	}

	public event PlayScenePressedEventHandler PlayScenePressed
	{
		add
		{
			backing_PlayScenePressed = (PlayScenePressedEventHandler)Delegate.Combine(backing_PlayScenePressed, value);
		}
		remove
		{
			backing_PlayScenePressed = (PlayScenePressedEventHandler)Delegate.Remove(backing_PlayScenePressed, value);
		}
	}

	public event StopPressedEventHandler StopPressed
	{
		add
		{
			backing_StopPressed = (StopPressedEventHandler)Delegate.Combine(backing_StopPressed, value);
		}
		remove
		{
			backing_StopPressed = (StopPressedEventHandler)Delegate.Remove(backing_StopPressed, value);
		}
	}

	public event PauseToggledEventHandler PauseToggled
	{
		add
		{
			backing_PauseToggled = (PauseToggledEventHandler)Delegate.Combine(backing_PauseToggled, value);
		}
		remove
		{
			backing_PauseToggled = (PauseToggledEventHandler)Delegate.Remove(backing_PauseToggled, value);
		}
	}

	public event PlayCustomPressedEventHandler PlayCustomPressed
	{
		add
		{
			backing_PlayCustomPressed = (PlayCustomPressedEventHandler)Delegate.Combine(backing_PlayCustomPressed, value);
		}
		remove
		{
			backing_PlayCustomPressed = (PlayCustomPressedEventHandler)Delegate.Remove(backing_PlayCustomPressed, value);
		}
	}

	public event ProfilerIndicatorPressedEventHandler ProfilerIndicatorPressed
	{
		add
		{
			backing_ProfilerIndicatorPressed = (ProfilerIndicatorPressedEventHandler)Delegate.Combine(backing_ProfilerIndicatorPressed, value);
		}
		remove
		{
			backing_ProfilerIndicatorPressed = (ProfilerIndicatorPressedEventHandler)Delegate.Remove(backing_ProfilerIndicatorPressed, value);
		}
	}

	public event RunStatusChangedEventHandler RunStatusChanged
	{
		add
		{
			backing_RunStatusChanged = (RunStatusChangedEventHandler)Delegate.Combine(backing_RunStatusChanged, value);
		}
		remove
		{
			backing_RunStatusChanged = (RunStatusChangedEventHandler)Delegate.Remove(backing_RunStatusChanged, value);
		}
	}

	public override void _Ready()
	{
		_profilerIndicator = GetNode<Button>("%ProfilerIndicator");
		_playBtn = GetNode<Button>("%PlayBtn");
		_pauseBtn = GetNode<Button>("%PauseBtn");
		_stopBtn = GetNode<Button>("%StopBtn");
		_remoteBtn = GetNode<MenuButton>("%RemoteBtn");
		_playSceneBtn = GetNode<Button>("%PlaySceneBtn");
		_playCustomBtn = GetNode<Button>("%PlayCustomBtn");
		InitRemoteMenu();
		_profilerIndicator.Pressed += () =>
		{
			EmitSignal(SignalName.ProfilerIndicatorPressed);
		};
		_playBtn.Toggled += OnPlayPressed;
		_playSceneBtn.Toggled += OnPlayScenePressed;
		_playCustomBtn.Toggled += OnPlayCustomPressed;
		_stopBtn.Pressed += () =>
		{
			EmitSignal(SignalName.StopPressed);
		};
		_pauseBtn.Toggled += OnPauseToggled;
		UpdateUI();
	}

	private void InitRemoteMenu()
	{
		PopupMenu popup = _remoteBtn.GetPopup();
		popup.Clear();
		popup.AddCheckItem("部署并远程调试", 0, Key.None);
		popup.AddSeparator();
		popup.AddItem("编辑远程调试设置...", 1, Key.None);
	}

	private void OnPlayPressed(bool pressed)
	{
		if (!_updatingUi && pressed)
		{
			EmitSignal(SignalName.PlayPressed);
		}
	}

	private void OnPlayScenePressed(bool pressed)
	{
		if (!_updatingUi && pressed)
		{
			EmitSignal(SignalName.PlayScenePressed);
		}
	}

	private void OnPlayCustomPressed(bool pressed)
	{
		if (!_updatingUi && pressed)
		{
			EmitSignal(SignalName.PlayCustomPressed);
		}
	}

	private void OnPauseToggled(bool pressed)
	{
		if (!_updatingUi)
		{
			EmitSignal(SignalName.PauseToggled, pressed);
		}
	}

	public void SetRunning(bool running)
	{
		if (running)
		{
			if (_runMode == RunMode.Stopped)
			{
				_runMode = RunMode.RunMain;
			}
		}
		else
		{
			_runMode = RunMode.Stopped;
			_isPaused = false;
		}
		UpdateUI();
	}

	public void SetRunMode(RunMode mode)
	{
		_runMode = mode;
		_isPaused = false;
		UpdateUI();
	}

	public void SetPaused(bool paused)
	{
		_isPaused = paused;
		UpdateUI();
	}

	public bool IsRunning()
	{
		return _runMode != RunMode.Stopped;
	}

	public bool IsPaused()
	{
		return _isPaused;
	}

	public RunMode GetRunMode()
	{
		return _runMode;
	}

	public void ShowProfilerIndicator(bool show)
	{
		if (GodotObject.IsInstanceValid(_profilerIndicator))
		{
			_profilerIndicator.Visible = show;
		}
	}

	public void SetProfilerText(string text)
	{
		if (GodotObject.IsInstanceValid(_profilerIndicator))
		{
			_profilerIndicator.Text = text;
		}
	}

	public void UpdateUI()
	{
		_updatingUi = true;
		bool flag = _runMode != RunMode.Stopped;
		_playBtn.SetPressedNoSignal(_runMode == RunMode.RunMain);
		_playSceneBtn.SetPressedNoSignal(_runMode == RunMode.RunCurrent);
		_playCustomBtn.SetPressedNoSignal(_runMode == RunMode.RunCustom);
		_stopBtn.Disabled = !flag;
		_pauseBtn.Disabled = !flag;
		_pauseBtn.SetPressedNoSignal(_isPaused);
		_updatingUi = false;
		EmitSignal(SignalName.RunStatusChanged, flag, _isPaused, (int)_runMode);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitRemoteMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPlayPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPlayScenePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPlayCustomPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPauseToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRunning, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "running", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRunMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPaused, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "paused", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRunning, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsPaused, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRunMode, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowProfilerIndicator, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "show", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProfilerText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.InitRemoteMenu && args.Count == 0)
		{
			InitRemoteMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPlayPressed && args.Count == 1)
		{
			OnPlayPressed(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPlayScenePressed && args.Count == 1)
		{
			OnPlayScenePressed(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPlayCustomPressed && args.Count == 1)
		{
			OnPlayCustomPressed(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPauseToggled && args.Count == 1)
		{
			OnPauseToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRunning && args.Count == 1)
		{
			SetRunning(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRunMode && args.Count == 1)
		{
			SetRunMode(VariantUtils.ConvertTo<RunMode>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPaused && args.Count == 1)
		{
			SetPaused(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsRunning && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRunning());
			return true;
		}
		if (method == MethodName.IsPaused && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPaused());
			return true;
		}
		if (method == MethodName.GetRunMode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<RunMode>(GetRunMode());
			return true;
		}
		if (method == MethodName.ShowProfilerIndicator && args.Count == 1)
		{
			ShowProfilerIndicator(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProfilerText && args.Count == 1)
		{
			SetProfilerText(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateUI && args.Count == 0)
		{
			UpdateUI();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.InitRemoteMenu)
		{
			return true;
		}
		if (method == MethodName.OnPlayPressed)
		{
			return true;
		}
		if (method == MethodName.OnPlayScenePressed)
		{
			return true;
		}
		if (method == MethodName.OnPlayCustomPressed)
		{
			return true;
		}
		if (method == MethodName.OnPauseToggled)
		{
			return true;
		}
		if (method == MethodName.SetRunning)
		{
			return true;
		}
		if (method == MethodName.SetRunMode)
		{
			return true;
		}
		if (method == MethodName.SetPaused)
		{
			return true;
		}
		if (method == MethodName.IsRunning)
		{
			return true;
		}
		if (method == MethodName.IsPaused)
		{
			return true;
		}
		if (method == MethodName.GetRunMode)
		{
			return true;
		}
		if (method == MethodName.ShowProfilerIndicator)
		{
			return true;
		}
		if (method == MethodName.SetProfilerText)
		{
			return true;
		}
		if (method == MethodName.UpdateUI)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._runMode)
		{
			_runMode = VariantUtils.ConvertTo<RunMode>(in value);
			return true;
		}
		if (name == PropertyName._isPaused)
		{
			_isPaused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._updatingUi)
		{
			_updatingUi = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._profilerIndicator)
		{
			_profilerIndicator = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._playBtn)
		{
			_playBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._pauseBtn)
		{
			_pauseBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stopBtn)
		{
			_stopBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._remoteBtn)
		{
			_remoteBtn = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._playSceneBtn)
		{
			_playSceneBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._playCustomBtn)
		{
			_playCustomBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._runMode)
		{
			value = VariantUtils.CreateFrom(in _runMode);
			return true;
		}
		if (name == PropertyName._isPaused)
		{
			value = VariantUtils.CreateFrom(in _isPaused);
			return true;
		}
		if (name == PropertyName._updatingUi)
		{
			value = VariantUtils.CreateFrom(in _updatingUi);
			return true;
		}
		if (name == PropertyName._profilerIndicator)
		{
			value = VariantUtils.CreateFrom(in _profilerIndicator);
			return true;
		}
		if (name == PropertyName._playBtn)
		{
			value = VariantUtils.CreateFrom(in _playBtn);
			return true;
		}
		if (name == PropertyName._pauseBtn)
		{
			value = VariantUtils.CreateFrom(in _pauseBtn);
			return true;
		}
		if (name == PropertyName._stopBtn)
		{
			value = VariantUtils.CreateFrom(in _stopBtn);
			return true;
		}
		if (name == PropertyName._remoteBtn)
		{
			value = VariantUtils.CreateFrom(in _remoteBtn);
			return true;
		}
		if (name == PropertyName._playSceneBtn)
		{
			value = VariantUtils.CreateFrom(in _playSceneBtn);
			return true;
		}
		if (name == PropertyName._playCustomBtn)
		{
			value = VariantUtils.CreateFrom(in _playCustomBtn);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._runMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isPaused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingUi, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._profilerIndicator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._playBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pauseBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stopBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._remoteBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._playSceneBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._playCustomBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._runMode, Variant.From(in _runMode));
		info.AddProperty(PropertyName._isPaused, Variant.From(in _isPaused));
		info.AddProperty(PropertyName._updatingUi, Variant.From(in _updatingUi));
		info.AddProperty(PropertyName._profilerIndicator, Variant.From(in _profilerIndicator));
		info.AddProperty(PropertyName._playBtn, Variant.From(in _playBtn));
		info.AddProperty(PropertyName._pauseBtn, Variant.From(in _pauseBtn));
		info.AddProperty(PropertyName._stopBtn, Variant.From(in _stopBtn));
		info.AddProperty(PropertyName._remoteBtn, Variant.From(in _remoteBtn));
		info.AddProperty(PropertyName._playSceneBtn, Variant.From(in _playSceneBtn));
		info.AddProperty(PropertyName._playCustomBtn, Variant.From(in _playCustomBtn));
		info.AddSignalEventDelegate(SignalName.PlayPressed, backing_PlayPressed);
		info.AddSignalEventDelegate(SignalName.PlayScenePressed, backing_PlayScenePressed);
		info.AddSignalEventDelegate(SignalName.StopPressed, backing_StopPressed);
		info.AddSignalEventDelegate(SignalName.PauseToggled, backing_PauseToggled);
		info.AddSignalEventDelegate(SignalName.PlayCustomPressed, backing_PlayCustomPressed);
		info.AddSignalEventDelegate(SignalName.ProfilerIndicatorPressed, backing_ProfilerIndicatorPressed);
		info.AddSignalEventDelegate(SignalName.RunStatusChanged, backing_RunStatusChanged);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._runMode, out var value))
		{
			_runMode = value.As<RunMode>();
		}
		if (info.TryGetProperty(PropertyName._isPaused, out var value2))
		{
			_isPaused = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._updatingUi, out var value3))
		{
			_updatingUi = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._profilerIndicator, out var value4))
		{
			_profilerIndicator = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._playBtn, out var value5))
		{
			_playBtn = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pauseBtn, out var value6))
		{
			_pauseBtn = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stopBtn, out var value7))
		{
			_stopBtn = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._remoteBtn, out var value8))
		{
			_remoteBtn = value8.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._playSceneBtn, out var value9))
		{
			_playSceneBtn = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._playCustomBtn, out var value10))
		{
			_playCustomBtn = value10.As<Button>();
		}
		if (info.TryGetSignalEventDelegate<PlayPressedEventHandler>(SignalName.PlayPressed, out var value11))
		{
			backing_PlayPressed = value11;
		}
		if (info.TryGetSignalEventDelegate<PlayScenePressedEventHandler>(SignalName.PlayScenePressed, out var value12))
		{
			backing_PlayScenePressed = value12;
		}
		if (info.TryGetSignalEventDelegate<StopPressedEventHandler>(SignalName.StopPressed, out var value13))
		{
			backing_StopPressed = value13;
		}
		if (info.TryGetSignalEventDelegate<PauseToggledEventHandler>(SignalName.PauseToggled, out var value14))
		{
			backing_PauseToggled = value14;
		}
		if (info.TryGetSignalEventDelegate<PlayCustomPressedEventHandler>(SignalName.PlayCustomPressed, out var value15))
		{
			backing_PlayCustomPressed = value15;
		}
		if (info.TryGetSignalEventDelegate<ProfilerIndicatorPressedEventHandler>(SignalName.ProfilerIndicatorPressed, out var value16))
		{
			backing_ProfilerIndicatorPressed = value16;
		}
		if (info.TryGetSignalEventDelegate<RunStatusChangedEventHandler>(SignalName.RunStatusChanged, out var value17))
		{
			backing_RunStatusChanged = value17;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(SignalName.PlayPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.PlayScenePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.StopPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.PauseToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "isPaused", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.PlayCustomPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ProfilerIndicatorPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.RunStatusChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "running", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "paused", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalPlayPressed()
	{
		EmitSignal(SignalName.PlayPressed, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalPlayScenePressed()
	{
		EmitSignal(SignalName.PlayScenePressed, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalStopPressed()
	{
		EmitSignal(SignalName.StopPressed, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalPauseToggled(bool isPaused)
	{
		EmitSignal(SignalName.PauseToggled, new ReadOnlySpan<Variant>((Variant)isPaused));
	}

	protected void EmitSignalPlayCustomPressed()
	{
		EmitSignal(SignalName.PlayCustomPressed, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalProfilerIndicatorPressed()
	{
		EmitSignal(SignalName.ProfilerIndicatorPressed, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalRunStatusChanged(bool running, bool paused, int mode)
	{
		StringName runStatusChanged = SignalName.RunStatusChanged;
		_003C_003Ey__InlineArray3<Variant> buffer = default;
		buffer[0] = running;
		buffer[1] = paused;
		buffer[2] = mode;
		EmitSignal(runStatusChanged, buffer);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.PlayPressed && args.Count == 0)
		{
			backing_PlayPressed?.Invoke();
		}
		else if (signal == SignalName.PlayScenePressed && args.Count == 0)
		{
			backing_PlayScenePressed?.Invoke();
		}
		else if (signal == SignalName.StopPressed && args.Count == 0)
		{
			backing_StopPressed?.Invoke();
		}
		else if (signal == SignalName.PauseToggled && args.Count == 1)
		{
			backing_PauseToggled?.Invoke(VariantUtils.ConvertTo<bool>(in args[0]));
		}
		else if (signal == SignalName.PlayCustomPressed && args.Count == 0)
		{
			backing_PlayCustomPressed?.Invoke();
		}
		else if (signal == SignalName.ProfilerIndicatorPressed && args.Count == 0)
		{
			backing_ProfilerIndicatorPressed?.Invoke();
		}
		else if (signal == SignalName.RunStatusChanged && args.Count == 3)
		{
			backing_RunStatusChanged?.Invoke(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.PlayPressed)
		{
			return true;
		}
		if (signal == SignalName.PlayScenePressed)
		{
			return true;
		}
		if (signal == SignalName.StopPressed)
		{
			return true;
		}
		if (signal == SignalName.PauseToggled)
		{
			return true;
		}
		if (signal == SignalName.PlayCustomPressed)
		{
			return true;
		}
		if (signal == SignalName.ProfilerIndicatorPressed)
		{
			return true;
		}
		if (signal == SignalName.RunStatusChanged)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
