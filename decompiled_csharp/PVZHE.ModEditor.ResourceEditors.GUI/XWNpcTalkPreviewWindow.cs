using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWNpcTalkPreviewWindow.cs")]
public class XWNpcTalkPreviewWindow : Window
{
	public new class MethodName : Window.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _UnhandledKeyInput = "_UnhandledKeyInput";

		public static readonly StringName Preview = "Preview";

		public static readonly StringName RefreshConfig = "RefreshConfig";

		public static readonly StringName BindInterface = "BindInterface";

		public static readonly StringName RefreshFromConfig = "RefreshFromConfig";

		public static readonly StringName ShowCurrent = "ShowCurrent";

		public static readonly StringName EnsureNpc = "EnsureNpc";

		public static readonly StringName ApplyTalkToNpc = "ApplyTalkToNpc";

		public static readonly StringName PlayTalkAudio = "PlayTalkAudio";

		public static readonly StringName ShowPrevious = "ShowPrevious";

		public static readonly StringName ShowNext = "ShowNext";

		public static readonly StringName ToggleAutoPlay = "ToggleAutoPlay";

		public static readonly StringName ScheduleAutoAdvance = "ScheduleAutoAdvance";

		public static readonly StringName AdvanceAutoPlay = "AdvanceAutoPlay";

		public static readonly StringName StopAutoPlay = "StopAutoPlay";

		public static readonly StringName ClearNpc = "ClearNpc";

		public static readonly StringName ClosePreview = "ClosePreview";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : Window.PropertyName
	{
		public static readonly StringName ResponsivePreviewHost = "ResponsivePreviewHost";

		public static readonly StringName _config = "_config";

		public static readonly StringName _index = "_index";

		public static readonly StringName _autoPlaying = "_autoPlaying";

		public static readonly StringName _runtimeControl = "_runtimeControl";

		public static readonly StringName _npc = "_npc";

		public static readonly StringName _npcKey = "_npcKey";

		public static readonly StringName _titleLabel = "_titleLabel";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _emptyLabel = "_emptyLabel";

		public static readonly StringName _previousButton = "_previousButton";

		public static readonly StringName _nextButton = "_nextButton";

		public static readonly StringName _autoButton = "_autoButton";

		public static readonly StringName _autoTimer = "_autoTimer";

		public static readonly StringName _directAudioPlayer = "_directAudioPlayer";

		public static readonly StringName _previewHost = "_previewHost";
	}

	public new class SignalName : Window.SignalName
	{
	}

	private const string CrazyDaveScenePath = "res://Prefab/Npc/CrazyDave/NpcCrazyDave.tscn";

	private const string WeiWeiMiScenePath = "res://Prefab/Npc/WeiWeiMi/WeiWeiMi.tscn";

	private NpcTalkConfig _config;

	private int _index;

	private bool _autoPlaying;

	private NpcTalkControl _runtimeControl;

	private NpcBase _npc;

	private string _npcKey = "";

	private Label _titleLabel;

	private Label _statusLabel;

	private Label _emptyLabel;

	private Button _previousButton;

	private Button _nextButton;

	private Button _autoButton;

	private Timer _autoTimer;

	private AudioStreamPlayer _directAudioPlayer;

	private XWAspectScaledPreviewHost _previewHost;

	public XWAspectScaledPreviewHost ResponsivePreviewHost => _previewHost;

	public override void _Ready()
	{
		CloseRequested += ClosePreview;
		BindInterface();
		RefreshFromConfig();
	}

	public override void _UnhandledKeyInput(InputEvent inputEvent)
	{
		if (inputEvent is InputEventKey { Pressed: not false, Echo: false, Keycode: var keycode } inputEventKey)
		{
			if ((keycode == Key.Space || keycode == Key.Enter || keycode == Key.Right) ? true : false)
			{
				ShowNext();
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.Left)
			{
				ShowPrevious();
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.Escape)
			{
				ClosePreview();
				GetViewport().SetInputAsHandled();
			}
		}
	}

	public void Preview(NpcTalkConfig config, int startIndex = 0)
	{
		_config = config;
		_index = Mathf.Max(0, startIndex);
		if (IsNodeReady())
		{
			RefreshFromConfig();
			PopupCenteredClamped(new Vector2I(1000, 680), 0.92f);
			GrabFocus();
		}
	}

	public void RefreshConfig(NpcTalkConfig config)
	{
		_config = config;
		if (IsNodeReady())
		{
			RefreshFromConfig();
		}
	}

	private void BindInterface()
	{
		_previewHost = GetNode<XWAspectScaledPreviewHost>("%GameCanvasCenter");
		_titleLabel = GetNode<Label>("Layout/Header/Title");
		_statusLabel = GetNode<Label>("Layout/Status");
		_previousButton = GetNode<Button>("Layout/Controls/PreviousButton");
		_nextButton = GetNode<Button>("Layout/Controls/NextButton");
		_autoButton = GetNode<Button>("Layout/Controls/AutoButton");
		Button node = GetNode<Button>("Layout/Controls/ReplayButton");
		SubViewportContainer node2 = GetNode<SubViewportContainer>("Layout/GameCanvasCenter/ViewportContainer");
		_runtimeControl = GetNode<NpcTalkControl>("%RuntimeNpcTalkControl");
		_emptyLabel = GetNode<Label>("Layout/GameCanvasCenter/ViewportContainer/Viewport/EmptyLabel");
		_autoTimer = GetNode<Timer>("AutoTimer");
		_directAudioPlayer = GetNode<AudioStreamPlayer>("DirectAudioPlayer");
		node2.GuiInput += (InputEvent inputEvent) =>
		{
			if (inputEvent is InputEventMouseButton { Pressed: not false } inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
			{
				ShowNext();
			}
		};
		_previousButton.Pressed += ShowPrevious;
		_nextButton.Pressed += ShowNext;
		_autoButton.Pressed += ToggleAutoPlay;
		node.Pressed += ShowCurrent;
		_autoTimer.Timeout += AdvanceAutoPlay;
	}

	private void RefreshFromConfig()
	{
		int valueOrDefault = (_config?.talkList?.Count).GetValueOrDefault();
		if (valueOrDefault == 0)
		{
			_index = 0;
			_emptyLabel.Visible = true;
			_statusLabel.Text = "对话列表为空";
			_previousButton.Disabled = true;
			_nextButton.Disabled = true;
			_autoButton.Disabled = true;
			ClearNpc();
		}
		else
		{
			_index = Mathf.Clamp(_index, 0, valueOrDefault - 1);
			_emptyLabel.Visible = false;
			_previousButton.Disabled = _index <= 0;
			_nextButton.Disabled = _index >= valueOrDefault - 1;
			_autoButton.Disabled = false;
			_titleLabel.Text = "NPC 对话运行预览  ·  " + EmptyToPlaceholder(_config.saveKey);
			ShowCurrent();
		}
	}

	private void ShowCurrent()
	{
		int valueOrDefault = (_config?.talkList?.Count).GetValueOrDefault();
		if (valueOrDefault != 0 && _index >= 0 && _index < valueOrDefault)
		{
			NpcTalkBaseConfig npcTalkBaseConfig = _config.talkList[_index];
			if (!GodotObject.IsInstanceValid(npcTalkBaseConfig))
			{
				_statusLabel.Text = $"{_index + 1} / {valueOrDefault}    空对话条目";
				ScheduleAutoAdvance();
				return;
			}
			EnsureNpc(npcTalkBaseConfig.npc);
			ApplyTalkToNpc(npcTalkBaseConfig);
			_statusLabel.Text = $"{_index + 1} / {valueOrDefault}    NPC: {EmptyToPlaceholder(npcTalkBaseConfig.npc)}    动画: {EmptyToPlaceholder(npcTalkBaseConfig.anime)}    音频: {EmptyToPlaceholder(npcTalkBaseConfig.audio)}";
			_previousButton.Disabled = _index <= 0;
			_nextButton.Disabled = _index >= valueOrDefault - 1;
			ScheduleAutoAdvance();
		}
	}

	private void EnsureNpc(string npcKey)
	{
		npcKey = (string.IsNullOrWhiteSpace(npcKey) ? "CrazyDave" : npcKey);
		if (GodotObject.IsInstanceValid(_npc) && _npcKey == npcKey)
		{
			return;
		}
		ClearNpc();
		string path = ((!(npcKey == "WeiWeiMi")) ? "res://Prefab/Npc/CrazyDave/NpcCrazyDave.tscn" : "res://Prefab/Npc/WeiWeiMi/WeiWeiMi.tscn");
		_npc = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Reuse)?.Instantiate<NpcBase>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(_npc))
		{
			_npcKey = npcKey;
			_npc.Name = "PreviewNpc";
			_npc.Position = Vector2.Zero;
			_npc.Scale = Vector2.One;
			ColorRect nodeOrNull = _npc.GetNodeOrNull<ColorRect>("CanvasLayer/ColorRect");
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				nodeOrNull.MouseFilter = Control.MouseFilterEnum.Ignore;
			}
			_runtimeControl.AddNpc(_npc);
		}
	}

	private void ApplyTalkToNpc(NpcTalkBaseConfig talk)
	{
		if (GodotObject.IsInstanceValid(_npc))
		{
			Label nodeOrNull = _npc.GetNodeOrNull<Label>("%TalkLabel");
			TextureRect nodeOrNull2 = _npc.GetNodeOrNull<TextureRect>("%TalkBubble");
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				nodeOrNull.Text = talk.text ?? "";
			}
			if (GodotObject.IsInstanceValid(nodeOrNull2))
			{
				nodeOrNull2.Visible = true;
				nodeOrNull2.Scale = Vector2.One;
			}
			if (GodotObject.IsInstanceValid(_npc.sprite) && !string.IsNullOrWhiteSpace(talk.anime))
			{
				_npc.sprite.SetAnimation(talk.anime);
			}
			if (talk is NpcTalkHandConfig hand)
			{
				_npc.Hand(hand);
			}
			PlayTalkAudio(talk.audio);
		}
	}

	private void PlayTalkAudio(string audioKey)
	{
		_directAudioPlayer.Stop();
		if (!string.IsNullOrWhiteSpace(audioKey))
		{
			if (GodotObject.IsInstanceValid(AudioManager.Instance))
			{
				AudioManager.Instance.AudioPlay(audioKey, AudioManagerEnum.TYPE.SFX, 0.0, once: true, pauseAlive: true);
			}
			else if (ResourceLoader.Exists(audioKey))
			{
				_directAudioPlayer.Stream = ResourceLoader.Load<AudioStream>(audioKey, null, ResourceLoader.CacheMode.Reuse);
				_directAudioPlayer.Play();
			}
		}
	}

	private void ShowPrevious()
	{
		if (_index > 0)
		{
			_index--;
			ShowCurrent();
		}
	}

	private void ShowNext()
	{
		int valueOrDefault = (_config?.talkList?.Count).GetValueOrDefault();
		if (_index + 1 >= valueOrDefault)
		{
			StopAutoPlay();
			return;
		}
		_index++;
		ShowCurrent();
	}

	private void ToggleAutoPlay()
	{
		if (_autoPlaying)
		{
			StopAutoPlay();
			return;
		}
		_autoPlaying = true;
		_index = 0;
		_autoButton.Text = "暂停播放";
		ShowCurrent();
	}

	private void ScheduleAutoAdvance()
	{
		if (_autoPlaying)
		{
			double num = 3.2;
			if (GodotObject.IsInstanceValid(_directAudioPlayer.Stream))
			{
				num = Mathf.Max(num, _directAudioPlayer.Stream.GetLength() + 0.35);
			}
			_autoTimer.Start(num);
		}
	}

	private void AdvanceAutoPlay()
	{
		if (_autoPlaying)
		{
			ShowNext();
		}
	}

	private void StopAutoPlay()
	{
		_autoPlaying = false;
		_autoTimer.Stop();
		_autoButton.Text = "播放整段";
	}

	private void ClearNpc()
	{
		if (GodotObject.IsInstanceValid(_npc))
		{
			_npc.GetParent()?.RemoveChild(_npc);
			_npc.QueueFree();
		}
		_npc = null;
		_npcKey = "";
	}

	private void ClosePreview()
	{
		StopAutoPlay();
		_directAudioPlayer?.Stop();
		Hide();
	}

	private static string EmptyToPlaceholder(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return "未设置";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._UnhandledKeyInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.Preview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "startIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindInterface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshFromConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowCurrent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureNpc, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "npcKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyTalkToNpc, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlayTalkAudio, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "audioKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowPrevious, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowNext, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleAutoPlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleAutoAdvance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdvanceAutoPlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopAutoPlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearNpc, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClosePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName._UnhandledKeyInput && args.Count == 1)
		{
			_UnhandledKeyInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Preview && args.Count == 2)
		{
			Preview(VariantUtils.ConvertTo<NpcTalkConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshConfig && args.Count == 1)
		{
			RefreshConfig(VariantUtils.ConvertTo<NpcTalkConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindInterface && args.Count == 0)
		{
			BindInterface();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFromConfig && args.Count == 0)
		{
			RefreshFromConfig();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCurrent && args.Count == 0)
		{
			ShowCurrent();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureNpc && args.Count == 1)
		{
			EnsureNpc(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyTalkToNpc && args.Count == 1)
		{
			ApplyTalkToNpc(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayTalkAudio && args.Count == 1)
		{
			PlayTalkAudio(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowPrevious && args.Count == 0)
		{
			ShowPrevious();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowNext && args.Count == 0)
		{
			ShowNext();
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleAutoPlay && args.Count == 0)
		{
			ToggleAutoPlay();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleAutoAdvance && args.Count == 0)
		{
			ScheduleAutoAdvance();
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceAutoPlay && args.Count == 0)
		{
			AdvanceAutoPlay();
			ret = default;
			return true;
		}
		if (method == MethodName.StopAutoPlay && args.Count == 0)
		{
			StopAutoPlay();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearNpc && args.Count == 0)
		{
			ClearNpc();
			ret = default;
			return true;
		}
		if (method == MethodName.ClosePreview && args.Count == 0)
		{
			ClosePreview();
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
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName._UnhandledKeyInput)
		{
			return true;
		}
		if (method == MethodName.Preview)
		{
			return true;
		}
		if (method == MethodName.RefreshConfig)
		{
			return true;
		}
		if (method == MethodName.BindInterface)
		{
			return true;
		}
		if (method == MethodName.RefreshFromConfig)
		{
			return true;
		}
		if (method == MethodName.ShowCurrent)
		{
			return true;
		}
		if (method == MethodName.EnsureNpc)
		{
			return true;
		}
		if (method == MethodName.ApplyTalkToNpc)
		{
			return true;
		}
		if (method == MethodName.PlayTalkAudio)
		{
			return true;
		}
		if (method == MethodName.ShowPrevious)
		{
			return true;
		}
		if (method == MethodName.ShowNext)
		{
			return true;
		}
		if (method == MethodName.ToggleAutoPlay)
		{
			return true;
		}
		if (method == MethodName.ScheduleAutoAdvance)
		{
			return true;
		}
		if (method == MethodName.AdvanceAutoPlay)
		{
			return true;
		}
		if (method == MethodName.StopAutoPlay)
		{
			return true;
		}
		if (method == MethodName.ClearNpc)
		{
			return true;
		}
		if (method == MethodName.ClosePreview)
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
		if (name == PropertyName._config)
		{
			_config = VariantUtils.ConvertTo<NpcTalkConfig>(in value);
			return true;
		}
		if (name == PropertyName._index)
		{
			_index = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._autoPlaying)
		{
			_autoPlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeControl)
		{
			_runtimeControl = VariantUtils.ConvertTo<NpcTalkControl>(in value);
			return true;
		}
		if (name == PropertyName._npc)
		{
			_npc = VariantUtils.ConvertTo<NpcBase>(in value);
			return true;
		}
		if (name == PropertyName._npcKey)
		{
			_npcKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			_titleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._emptyLabel)
		{
			_emptyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previousButton)
		{
			_previousButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._nextButton)
		{
			_nextButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._autoButton)
		{
			_autoButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._autoTimer)
		{
			_autoTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._directAudioPlayer)
		{
			_directAudioPlayer = VariantUtils.ConvertTo<AudioStreamPlayer>(in value);
			return true;
		}
		if (name == PropertyName._previewHost)
		{
			_previewHost = VariantUtils.ConvertTo<XWAspectScaledPreviewHost>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ResponsivePreviewHost)
		{
			value = VariantUtils.CreateFrom<XWAspectScaledPreviewHost>(ResponsivePreviewHost);
			return true;
		}
		if (name == PropertyName._config)
		{
			value = VariantUtils.CreateFrom(in _config);
			return true;
		}
		if (name == PropertyName._index)
		{
			value = VariantUtils.CreateFrom(in _index);
			return true;
		}
		if (name == PropertyName._autoPlaying)
		{
			value = VariantUtils.CreateFrom(in _autoPlaying);
			return true;
		}
		if (name == PropertyName._runtimeControl)
		{
			value = VariantUtils.CreateFrom(in _runtimeControl);
			return true;
		}
		if (name == PropertyName._npc)
		{
			value = VariantUtils.CreateFrom(in _npc);
			return true;
		}
		if (name == PropertyName._npcKey)
		{
			value = VariantUtils.CreateFrom(in _npcKey);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			value = VariantUtils.CreateFrom(in _titleLabel);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._emptyLabel)
		{
			value = VariantUtils.CreateFrom(in _emptyLabel);
			return true;
		}
		if (name == PropertyName._previousButton)
		{
			value = VariantUtils.CreateFrom(in _previousButton);
			return true;
		}
		if (name == PropertyName._nextButton)
		{
			value = VariantUtils.CreateFrom(in _nextButton);
			return true;
		}
		if (name == PropertyName._autoButton)
		{
			value = VariantUtils.CreateFrom(in _autoButton);
			return true;
		}
		if (name == PropertyName._autoTimer)
		{
			value = VariantUtils.CreateFrom(in _autoTimer);
			return true;
		}
		if (name == PropertyName._directAudioPlayer)
		{
			value = VariantUtils.CreateFrom(in _directAudioPlayer);
			return true;
		}
		if (name == PropertyName._previewHost)
		{
			value = VariantUtils.CreateFrom(in _previewHost);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._index, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._autoPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._npc, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._npcKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._titleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._autoButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._autoTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._directAudioPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ResponsivePreviewHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._config, Variant.From(in _config));
		info.AddProperty(PropertyName._index, Variant.From(in _index));
		info.AddProperty(PropertyName._autoPlaying, Variant.From(in _autoPlaying));
		info.AddProperty(PropertyName._runtimeControl, Variant.From(in _runtimeControl));
		info.AddProperty(PropertyName._npc, Variant.From(in _npc));
		info.AddProperty(PropertyName._npcKey, Variant.From(in _npcKey));
		info.AddProperty(PropertyName._titleLabel, Variant.From(in _titleLabel));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._emptyLabel, Variant.From(in _emptyLabel));
		info.AddProperty(PropertyName._previousButton, Variant.From(in _previousButton));
		info.AddProperty(PropertyName._nextButton, Variant.From(in _nextButton));
		info.AddProperty(PropertyName._autoButton, Variant.From(in _autoButton));
		info.AddProperty(PropertyName._autoTimer, Variant.From(in _autoTimer));
		info.AddProperty(PropertyName._directAudioPlayer, Variant.From(in _directAudioPlayer));
		info.AddProperty(PropertyName._previewHost, Variant.From(in _previewHost));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._config, out var value))
		{
			_config = value.As<NpcTalkConfig>();
		}
		if (info.TryGetProperty(PropertyName._index, out var value2))
		{
			_index = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._autoPlaying, out var value3))
		{
			_autoPlaying = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeControl, out var value4))
		{
			_runtimeControl = value4.As<NpcTalkControl>();
		}
		if (info.TryGetProperty(PropertyName._npc, out var value5))
		{
			_npc = value5.As<NpcBase>();
		}
		if (info.TryGetProperty(PropertyName._npcKey, out var value6))
		{
			_npcKey = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName._titleLabel, out var value7))
		{
			_titleLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value8))
		{
			_statusLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._emptyLabel, out var value9))
		{
			_emptyLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previousButton, out var value10))
		{
			_previousButton = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nextButton, out var value11))
		{
			_nextButton = value11.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._autoButton, out var value12))
		{
			_autoButton = value12.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._autoTimer, out var value13))
		{
			_autoTimer = value13.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._directAudioPlayer, out var value14))
		{
			_directAudioPlayer = value14.As<AudioStreamPlayer>();
		}
		if (info.TryGetProperty(PropertyName._previewHost, out var value15))
		{
			_previewHost = value15.As<XWAspectScaledPreviewHost>();
		}
	}
}
