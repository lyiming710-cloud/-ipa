using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAudioVisualResourceEditor.cs")]
public class XWAudioVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName RenderAudioPreview = "RenderAudioPreview";

		public static readonly StringName RenderAudioTimeline = "RenderAudioTimeline";

		public static readonly StringName RenderBgmBindingGraph = "RenderBgmBindingGraph";

		public static readonly StringName RenderBgmAudioSelector = "RenderBgmAudioSelector";

		public static readonly StringName BindBgmAudioField = "BindBgmAudioField";

		public static readonly StringName ConfigureBgmRuntimePreview = "ConfigureBgmRuntimePreview";

		public static readonly StringName PreviewBgmAudioKey = "PreviewBgmAudioKey";

		public static readonly StringName PlayBgmMainSegment = "PlayBgmMainSegment";

		public static readonly StringName StartBgmBattleMix = "StartBgmBattleMix";

		public static readonly StringName StopBgmRuntimePreview = "StopBgmRuntimePreview";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public static readonly StringName EnsureBgmPreviewPlayers = "EnsureBgmPreviewPlayers";

		public static readonly StringName UpdateBgmRuntimeStatus = "UpdateBgmRuntimeStatus";

		public static readonly StringName ShowBgmAudioSelector = "ShowBgmAudioSelector";

		public static readonly StringName EnsureBgmAudioSelectorWindow = "EnsureBgmAudioSelectorWindow";

		public static readonly StringName GetBgmAudioFieldValue = "GetBgmAudioFieldValue";

		public static readonly StringName SetBgmAudioField = "SetBgmAudioField";

		public static readonly StringName OnBgmPropertyEdited = "OnBgmPropertyEdited";

		public static readonly StringName RefreshBgmDirectPreview = "RefreshBgmDirectPreview";

		public static readonly StringName RefreshBgmFromHistory = "RefreshBgmFromHistory";

		public static readonly StringName ReloadBgmEditorAfterHistory = "ReloadBgmEditorAfterHistory";

		public static readonly StringName RenderAudioReferences = "RenderAudioReferences";

		public static readonly StringName PlayAudioPreview = "PlayAudioPreview";

		public static readonly StringName StopAudioPreview = "StopAudioPreview";

		public static readonly StringName EnsureAudioPreviewPlayer = "EnsureAudioPreviewPlayer";

		public static readonly StringName AddWaveformMarker = "AddWaveformMarker";

		public static readonly StringName AddTimelineRow = "AddTimelineRow";

		public static readonly StringName AddPreviewRow = "AddPreviewRow";

		public static readonly StringName AddGraphRow = "AddGraphRow";

		public static readonly StringName AddReferenceRow = "AddReferenceRow";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";

		public static readonly StringName ReadNamedValue = "ReadNamedValue";

		public static readonly StringName ReadBooleanProperty = "ReadBooleanProperty";

		public static readonly StringName ReadPropertyName = "ReadPropertyName";

		public static readonly StringName IsAudioLike = "IsAudioLike";

		public static readonly StringName IsReferenceLike = "IsReferenceLike";

		public static readonly StringName FormatVariant = "FormatVariant";

		public static readonly StringName FormatObject = "FormatObject";

		public static readonly StringName FormatTextValue = "FormatTextValue";

		public static readonly StringName FormatSeconds = "FormatSeconds";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _audioPreviewPlayer = "_audioPreviewPlayer";

		public static readonly StringName _bgmMainPreviewPlayer = "_bgmMainPreviewPlayer";

		public static readonly StringName _bgmDrumsPreviewPlayer = "_bgmDrumsPreviewPlayer";

		public static readonly StringName _bgmZombieCountSlider = "_bgmZombieCountSlider";

		public static readonly StringName _bgmRuntimeStatus = "_bgmRuntimeStatus";

		public static readonly StringName _bgmBattlePreviewActive = "_bgmBattlePreviewActive";

		public static readonly StringName _bgmAudioSelector = "_bgmAudioSelector";

		public static readonly StringName _bgmAudioSelectorField = "_bgmAudioSelectorField";

		public static readonly StringName _bgmRefreshQueued = "_bgmRefreshQueued";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string AudioStreamPreviewScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAudioStreamPreview.tscn";

	private const string BgmAudioBindingsPreviewScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWBgmAudioBindingsPreview.tscn";

	private static PackedScene _audioStreamPreviewScene;

	private static PackedScene _bgmAudioBindingsPreviewScene;

	private AudioStreamPlayer _audioPreviewPlayer;

	private AudioStreamPlayer _bgmMainPreviewPlayer;

	private AudioStreamPlayer _bgmDrumsPreviewPlayer;

	private HSlider _bgmZombieCountSlider;

	private Label _bgmRuntimeStatus;

	private bool _bgmBattlePreviewActive;

	private XWVisualPropertyBinding _bgmPropertyBinding;

	private XWGameplayResourcePickerWindow _bgmAudioSelector;

	private string _bgmAudioSelectorField = "";

	private bool _bgmRefreshQueued;

	public override void _Ready()
	{
		base._Ready();
		EnableVisibilityGatedProcessing();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		_bgmPropertyBinding?.Dispose();
		_bgmPropertyBinding = null;
		StopAudioPreview();
		StopBgmRuntimePreview();
		Resource currentResource = CurrentResource;
		if (GodotObject.IsInstanceValid(currentResource))
		{
			if (TryReadAudioStream(currentResource, out var stream))
			{
				RenderAudioPreview(stream);
				RenderAudioTimeline(stream);
			}
			if (currentResource is TowerDefenseBackgroundMusicConfig bgm)
			{
				RenderBgmAudioSelector(bgm);
				RenderBgmBindingGraph(bgm);
			}
			else
			{
				RenderBgmBindingGraph(currentResource);
			}
			RenderAudioReferences(currentResource, stream);
		}
	}

	public override void _ExitTree()
	{
		_bgmPropertyBinding?.Dispose();
		_bgmPropertyBinding = null;
		StopAudioPreview();
		StopBgmRuntimePreview();
		if (GodotObject.IsInstanceValid(_bgmAudioSelector))
		{
			_bgmAudioSelector.Dismiss();
		}
		if (GodotObject.IsInstanceValid(_audioPreviewPlayer))
		{
			_audioPreviewPlayer.QueueFree();
		}
		base._ExitTree();
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (resource?.GetType() != typeof(TowerDefenseBackgroundMusicConfig))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (_bgmBattlePreviewActive && CurrentResource is TowerDefenseBackgroundMusicConfig towerDefenseBackgroundMusicConfig && GodotObject.IsInstanceValid(_bgmDrumsPreviewPlayer))
		{
			float num = Math.Max(0, towerDefenseBackgroundMusicConfig.drumsZombieThreshold);
			float to = (((float)(_bgmZombieCountSlider?.Value ?? 0.0) >= num) ? 1f : 0f);
			float num2 = Mathf.DbToLinear(_bgmDrumsPreviewPlayer.VolumeDb);
			float num3 = Mathf.Max(0.01f, towerDefenseBackgroundMusicConfig.drumsFadeSpeed);
			float weight = 1f - Mathf.Exp((0f - num3) * (float)Math.Max(0.0, delta));
			float num4 = Mathf.Lerp(num2, to, weight);
			_bgmDrumsPreviewPlayer.VolumeDb = ((num4 <= 0.0001f) ? (-80f) : Mathf.LinearToDb(num4));
			UpdateBgmRuntimeStatus(towerDefenseBackgroundMusicConfig, num4);
		}
	}

	private void RenderAudioPreview(AudioStream stream)
	{
		EnsureAudioPreviewPlayer();
		AddPreviewRow("音频类型: " + stream.GetType().Name);
		AddPreviewRow("播放长度: " + FormatSeconds(stream.GetLength()));
		AddPreviewRow("循环: " + ReadBooleanProperty(stream, "loop", "Loop"));
		AddPreviewRow("音量: " + ReadNamedValue(stream, "volume", "VolumeDb", "volumeDb"));
		AddPreviewRow("淡入淡出: " + ReadNamedValue(stream, "fade", "fadeIn", "fadeOut"));
		if (CanvasGrid == null)
		{
			return;
		}
		CanvasGrid.Columns = 1;
		if (_audioStreamPreviewScene == null)
		{
			_audioStreamPreviewScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAudioStreamPreview.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _audioStreamPreviewScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(vBoxContainer))
		{
			CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
			vBoxContainer.GetNode<Button>("Controls/PlayButton").Pressed += PlayAudioPreview;
			vBoxContainer.GetNode<Button>("Controls/StopButton").Pressed += StopAudioPreview;
			vBoxContainer.GetNode<Label>("Controls/DurationLabel").Text = "时长 " + FormatSeconds(stream.GetLength());
			HBoxContainer node = vBoxContainer.GetNode<HBoxContainer>("Waveform");
			int num = 40;
			double num2 = Math.Max(stream.GetLength(), 1.0);
			for (int i = 0; i < num; i++)
			{
				float num3 = (float)i / (float)Math.Max(1, num - 1);
				float y = 12f + 52f * (0.35f + 0.65f * Mathf.Abs(Mathf.Sin(num3 * ((float)Math.PI * 2f) * 2.75f)));
				ColorRect node2 = new ColorRect
				{
					Color = new Color(0.32f, 0.62f, 0.95f, 0.85f),
					CustomMinimumSize = new Vector2(5f, y),
					SizeFlagsHorizontal = SizeFlags.ExpandFill,
					SizeFlagsVertical = SizeFlags.ShrinkEnd,
					TooltipText = (FormatSeconds(num2 * (double)num3) ?? "")
				};
				node.AddChild(node2, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void RenderAudioTimeline(AudioStream stream)
	{
		double num = Math.Max(0.0, stream.GetLength());
		AddTimelineRow("播放时间线: 0.00s -> " + FormatSeconds(num));
		AddTimelineRow("循环段: " + ReadBooleanProperty(stream, "loop", "Loop"));
		AddTimelineRow("淡入淡出: " + ReadNamedValue(stream, "fade", "fadeIn", "fadeOut"));
		AddTimelineRow("BGM 切换点: 由 BGM 配置的 entry / flag1 / drums / win 决定");
		AddWaveformMarker("起点", 0.0, num);
		AddWaveformMarker("中点", num * 0.5, num);
		AddWaveformMarker("终点", num, num);
	}

	private void RenderBgmBindingGraph(TowerDefenseBackgroundMusicConfig bgm)
	{
		AddGraphRow("BGM 段落绑定");
		AddGraphRow("entry -> " + FormatTextValue(bgm.entry));
		AddGraphRow("flag1 -> " + FormatTextValue(bgm.flag1));
		AddGraphRow("drums -> " + FormatTextValue(bgm.drums));
		AddGraphRow("win -> " + FormatTextValue(bgm.win));
		AddGraphRow("translate -> " + FormatTextValue(bgm.translate));
		AddPreviewRow("BGM 主段: " + FormatTextValue(bgm.entry));
		AddPreviewRow("BGM 鼓点: " + FormatTextValue(bgm.drums));
		AddReferenceRow("音频引用 entry -> " + FormatTextValue(bgm.entry));
		AddReferenceRow("音频引用 flag1 -> " + FormatTextValue(bgm.flag1));
		AddReferenceRow("音频引用 drums -> " + FormatTextValue(bgm.drums));
		AddReferenceRow("音频引用 win -> " + FormatTextValue(bgm.win));
	}

	private void RenderBgmAudioSelector(TowerDefenseBackgroundMusicConfig bgm)
	{
		if (CanvasGrid != null && GodotObject.IsInstanceValid(bgm))
		{
			CanvasGrid.Columns = 1;
			if (_bgmAudioBindingsPreviewScene == null)
			{
				_bgmAudioBindingsPreviewScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWBgmAudioBindingsPreview.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _bgmAudioBindingsPreviewScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				_bgmPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnBgmPropertyEdited);
				_bgmPropertyBinding.BindText(vBoxContainer.GetNode<LineEdit>("%ResourceNameEdit"), bgm, "resource_name", RefreshBgmDirectPreview, this, "RefreshBgmFromHistory");
				_bgmPropertyBinding.BindToggle(vBoxContainer.GetNode<CheckButton>("%LocalToSceneCheck"), bgm, "resource_local_to_scene", RefreshBgmDirectPreview, this, "RefreshBgmFromHistory");
				_bgmPropertyBinding.BindText(vBoxContainer.GetNode<LineEdit>("%TranslateEdit"), bgm, "translate", RefreshBgmDirectPreview, this, "RefreshBgmFromHistory");
				_bgmPropertyBinding.BindNumber(vBoxContainer.GetNode<SpinBox>("%DrumsZombieThreshold"), bgm, "drumsZombieThreshold", RefreshBgmDirectPreview, this, "RefreshBgmFromHistory");
				_bgmPropertyBinding.BindNumber(vBoxContainer.GetNode<SpinBox>("%DrumsFadeSpeed"), bgm, "drumsFadeSpeed", RefreshBgmDirectPreview, this, "RefreshBgmFromHistory");
				_bgmPropertyBinding.BindNumber(vBoxContainer.GetNode<SpinBox>("%DrumsCheckInterval"), bgm, "drumsCheckInterval", RefreshBgmDirectPreview, this, "RefreshBgmFromHistory");
				BindBgmAudioField(vBoxContainer, "EntryRow", "entry", bgm);
				BindBgmAudioField(vBoxContainer, "Flag1Row", "flag1", bgm);
				BindBgmAudioField(vBoxContainer, "DrumsRow", "drums", bgm);
				BindBgmAudioField(vBoxContainer, "WinRow", "win", bgm);
				ConfigureBgmRuntimePreview(vBoxContainer.GetNode<PanelContainer>("BgmRuntimeMixPreview"), bgm);
			}
		}
	}

	private void BindBgmAudioField(Node root, string rowPath, string fieldName, TowerDefenseBackgroundMusicConfig bgm)
	{
		LineEdit edit = root.GetNode<LineEdit>(rowPath + "/Value");
		_bgmPropertyBinding.BindText(edit, bgm, fieldName, RefreshBgmDirectPreview, this, "RefreshBgmFromHistory");
		root.GetNode<Button>(rowPath + "/PickButton").Pressed += () =>
		{
			ShowBgmAudioSelector(fieldName);
		};
		root.GetNode<Button>(rowPath + "/PreviewButton").Pressed += () =>
		{
			PreviewBgmAudioKey(edit.Text);
		};
	}

	private void ConfigureBgmRuntimePreview(PanelContainer panel, TowerDefenseBackgroundMusicConfig bgm)
	{
		EnsureBgmPreviewPlayers();
		if (GodotObject.IsInstanceValid(panel))
		{
			Button node = panel.GetNode<Button>("Layout/Controls/EntryButton");
			Button node2 = panel.GetNode<Button>("Layout/Controls/BattleButton");
			Button node3 = panel.GetNode<Button>("Layout/Controls/WinButton");
			Button node4 = panel.GetNode<Button>("Layout/Controls/StopButton");
			node.Pressed += () =>
			{
				PlayBgmMainSegment(bgm.entry, "entry");
			};
			node2.Pressed += () =>
			{
				StartBgmBattleMix(bgm);
			};
			node3.Pressed += () =>
			{
				PlayBgmMainSegment(bgm.win, "win");
			};
			node4.Pressed += StopBgmRuntimePreview;
			_bgmZombieCountSlider = panel.GetNode<HSlider>("Layout/ZombieRow/CountSlider");
			_bgmZombieCountSlider.MaxValue = Math.Max(30, bgm.drumsZombieThreshold * 2);
			_bgmZombieCountSlider.Value = 0.0;
			Label zombieValue = panel.GetNode<Label>("Layout/ZombieRow/CountValue");
			_bgmZombieCountSlider.ValueChanged += (double value) =>
			{
				zombieValue.Text = ((int)value).ToString();
			};
			_bgmRuntimeStatus = panel.GetNode<Label>("Layout/Status");
			_bgmRuntimeStatus.Text = $"鼓点触发阈值 {bgm.drumsZombieThreshold}，渐入速度 {bgm.drumsFadeSpeed:0.##}";
		}
	}

	private void PreviewBgmAudioKey(string audioKey)
	{
		if (!TryResolveAudioStream(audioKey, out var stream))
		{
			XWEditorInterface.Instance?.ShowToast("无法解析音频 key: " + FormatTextValue(audioKey));
			return;
		}
		EnsureBgmPreviewPlayers();
		StopBgmRuntimePreview();
		_bgmMainPreviewPlayer.Stream = stream;
		_bgmMainPreviewPlayer.Play();
		if (GodotObject.IsInstanceValid(_bgmRuntimeStatus))
		{
			_bgmRuntimeStatus.Text = "正在试听: " + audioKey + "    " + FormatSeconds(stream.GetLength());
		}
	}

	private void PlayBgmMainSegment(string audioKey, string segmentName)
	{
		if (!TryResolveAudioStream(audioKey, out var stream))
		{
			XWEditorInterface.Instance?.ShowToast("BGM " + segmentName + " 未绑定可用音频: " + FormatTextValue(audioKey));
			return;
		}
		EnsureBgmPreviewPlayers();
		StopBgmRuntimePreview();
		_bgmMainPreviewPlayer.Stream = stream;
		_bgmMainPreviewPlayer.Play();
		_bgmRuntimeStatus.Text = "正在播放 " + segmentName + ": " + audioKey;
	}

	private void StartBgmBattleMix(TowerDefenseBackgroundMusicConfig bgm)
	{
		if (!TryResolveAudioStream(bgm.flag1, out var stream))
		{
			XWEditorInterface.Instance?.ShowToast("战斗主轨 flag1 不可用: " + FormatTextValue(bgm.flag1));
			return;
		}
		EnsureBgmPreviewPlayers();
		StopBgmRuntimePreview();
		_bgmMainPreviewPlayer.Stream = stream;
		_bgmMainPreviewPlayer.Play();
		if (TryResolveAudioStream(bgm.drums, out var stream2))
		{
			_bgmDrumsPreviewPlayer.Stream = stream2;
			_bgmDrumsPreviewPlayer.VolumeDb = -80f;
			_bgmDrumsPreviewPlayer.Play();
		}
		_bgmBattlePreviewActive = true;
		RequestVisibilityGatedProcessing(requested: true);
		UpdateBgmRuntimeStatus(bgm, 0f);
	}

	private void StopBgmRuntimePreview()
	{
		_bgmBattlePreviewActive = false;
		RequestVisibilityGatedProcessing(requested: false);
		if (GodotObject.IsInstanceValid(_bgmMainPreviewPlayer))
		{
			_bgmMainPreviewPlayer.Stop();
			_bgmMainPreviewPlayer.Stream = null;
		}
		if (GodotObject.IsInstanceValid(_bgmDrumsPreviewPlayer))
		{
			_bgmDrumsPreviewPlayer.Stop();
			_bgmDrumsPreviewPlayer.Stream = null;
		}
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		if (!visible)
		{
			StopAudioPreview();
			StopBgmRuntimePreview();
		}
	}

	private void EnsureBgmPreviewPlayers()
	{
		if (!GodotObject.IsInstanceValid(_bgmMainPreviewPlayer))
		{
			_bgmMainPreviewPlayer = new AudioStreamPlayer
			{
				Bus = "Music"
			};
			AddChild(_bgmMainPreviewPlayer, forceReadableName: false, InternalMode.Disabled);
		}
		if (!GodotObject.IsInstanceValid(_bgmDrumsPreviewPlayer))
		{
			_bgmDrumsPreviewPlayer = new AudioStreamPlayer
			{
				Bus = "Music"
			};
			AddChild(_bgmDrumsPreviewPlayer, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private bool TryResolveAudioStream(string key, out AudioStream stream)
	{
		stream = null;
		if (string.IsNullOrWhiteSpace(key))
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(ResourceManager.Instance) && ResourceManager.Instance.AUDIOS.TryGetValue(key, out var value) && value is AudioStream audioStream)
		{
			stream = audioStream;
			return true;
		}
		EnsureBgmAudioSelectorWindow();
		if (!GodotObject.IsInstanceValid(_bgmAudioSelector))
		{
			return false;
		}
		stream = _bgmAudioSelector.LoadAudioStream(key);
		return GodotObject.IsInstanceValid(stream);
	}

	private void UpdateBgmRuntimeStatus(TowerDefenseBackgroundMusicConfig bgm, float drumsVolume)
	{
		if (GodotObject.IsInstanceValid(_bgmRuntimeStatus))
		{
			int value = (int)(_bgmZombieCountSlider?.Value ?? 0.0);
			_bgmRuntimeStatus.Text = $"战斗混音: {bgm.flag1} + {bgm.drums}    僵尸 {value}/{bgm.drumsZombieThreshold}    鼓点 {drumsVolume * 100f:0}%";
		}
	}

	private void ShowBgmAudioSelector(string fieldName)
	{
		EnsureBgmAudioSelectorWindow();
		if (GodotObject.IsInstanceValid(_bgmAudioSelector))
		{
			_bgmAudioSelectorField = fieldName ?? "";
			_bgmAudioSelector.Open(XWGameplayResourceKind.Audio, GetBgmAudioFieldValue(_bgmAudioSelectorField), SelectBgmAudioChoice, (XWGameplayResourceChoice choice) => choice.Kind == XWGameplayResourceKind.Audio, lockKind: true, "BGM 音频图鉴 · " + _bgmAudioSelectorField);
		}
	}

	private void EnsureBgmAudioSelectorWindow()
	{
		if (!GodotObject.IsInstanceValid(_bgmAudioSelector))
		{
			_bgmAudioSelector = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_bgmAudioSelector))
			{
				AddChild(_bgmAudioSelector, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void SelectBgmAudioChoice(XWGameplayResourceChoice choice)
	{
		if (!(choice == null) && choice.Kind == XWGameplayResourceKind.Audio)
		{
			string persistedAudioKey = XWGameplayResourcePickerWindow.GetPersistedAudioKey(choice);
			if (!string.IsNullOrWhiteSpace(persistedAudioKey))
			{
				SetBgmAudioField(_bgmAudioSelectorField, persistedAudioKey, refresh: true);
			}
		}
	}

	private string GetBgmAudioFieldValue(string fieldName)
	{
		if (!(CurrentResource is TowerDefenseBackgroundMusicConfig towerDefenseBackgroundMusicConfig))
		{
			return "";
		}
		return fieldName switch
		{
			"entry" => towerDefenseBackgroundMusicConfig.entry, 
			"flag1" => towerDefenseBackgroundMusicConfig.flag1, 
			"drums" => towerDefenseBackgroundMusicConfig.drums, 
			"win" => towerDefenseBackgroundMusicConfig.win, 
			_ => "", 
		};
	}

	private void SetBgmAudioField(string fieldName, string value, bool refresh)
	{
		if (!(CurrentResource is TowerDefenseBackgroundMusicConfig towerDefenseBackgroundMusicConfig) || string.IsNullOrWhiteSpace(fieldName))
		{
			return;
		}
		string text = value ?? "";
		bool flag;
		switch (fieldName)
		{
		case "entry":
		case "flag1":
		case "drums":
		case "win":
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			if (_bgmPropertyBinding != null)
			{
				_bgmPropertyBinding.SetValue(towerDefenseBackgroundMusicConfig, fieldName, text, "更换 BGM " + fieldName, this, "RefreshBgmFromHistory");
			}
			else
			{
				towerDefenseBackgroundMusicConfig.Set(fieldName, text);
				NotifyCurrentResourceEdited();
			}
			if (refresh)
			{
				RefreshBgmFromHistory();
			}
			else
			{
				RefreshBgmDirectPreview();
			}
		}
	}

	private void OnBgmPropertyEdited(bool committed)
	{
		NotifyCurrentResourceEdited();
		RefreshBgmDirectPreview();
	}

	private void RefreshBgmDirectPreview()
	{
		if (CurrentResource is TowerDefenseBackgroundMusicConfig towerDefenseBackgroundMusicConfig)
		{
			if (GodotObject.IsInstanceValid(_bgmZombieCountSlider))
			{
				_bgmZombieCountSlider.MaxValue = Math.Max(30, towerDefenseBackgroundMusicConfig.drumsZombieThreshold * 2);
			}
			UpdateBgmRuntimeStatus(towerDefenseBackgroundMusicConfig, GodotObject.IsInstanceValid(_bgmDrumsPreviewPlayer) ? Mathf.DbToLinear(_bgmDrumsPreviewPlayer.VolumeDb) : 0f);
		}
	}

	public void RefreshBgmFromHistory()
	{
		if (!_bgmRefreshQueued && CurrentResource is TowerDefenseBackgroundMusicConfig)
		{
			_bgmRefreshQueued = true;
			CallDeferred("ReloadBgmEditorAfterHistory");
		}
	}

	public void ReloadBgmEditorAfterHistory()
	{
		_bgmRefreshQueued = false;
		if (CurrentResource is TowerDefenseBackgroundMusicConfig)
		{
			LoadResource(CurrentResource, CurrentResourcePath, CurrentDescriptor, CurrentEditContext);
		}
	}

	private void RenderBgmBindingGraph(Resource resource)
	{
		List<(string, string)> list = ExtractAudioLikeProperties(resource);
		if (list.Count == 0)
		{
			return;
		}
		AddGraphRow("音频/BGM 属性绑定");
		foreach (var item in list)
		{
			AddGraphRow(item.Item1 + " -> " + item.Item2);
			if (IsReferenceLike(item.Item1))
			{
				AddReferenceRow("音频引用 " + item.Item1 + " -> " + item.Item2);
			}
		}
	}

	private void RenderAudioReferences(Resource resource, AudioStream stream)
	{
		if (!string.IsNullOrWhiteSpace(CurrentResourcePath))
		{
			AddReferenceRow("资源文件 -> " + CurrentResourcePath);
		}
		if (GodotObject.IsInstanceValid(stream))
		{
			string resourcePath = stream.ResourcePath;
			AddReferenceRow(string.IsNullOrWhiteSpace(resourcePath) ? "音频流 -> 内嵌资源" : ("音频流 -> " + resourcePath));
		}
		foreach (var item in ExtractAudioLikeProperties(resource))
		{
			if (IsReferenceLike(item.Name))
			{
				AddReferenceRow("属性引用 " + item.Name + " -> " + item.Value);
			}
		}
	}

	private void PlayAudioPreview()
	{
		if (TryReadAudioStream(CurrentResource, out var stream))
		{
			EnsureAudioPreviewPlayer();
			_audioPreviewPlayer.Stop();
			_audioPreviewPlayer.Stream = stream;
			_audioPreviewPlayer.Play();
		}
	}

	private void StopAudioPreview()
	{
		if (GodotObject.IsInstanceValid(_audioPreviewPlayer))
		{
			_audioPreviewPlayer.Stop();
			_audioPreviewPlayer.Stream = null;
		}
	}

	private void EnsureAudioPreviewPlayer()
	{
		if (!GodotObject.IsInstanceValid(_audioPreviewPlayer))
		{
			_audioPreviewPlayer = new AudioStreamPlayer();
			AddChild(_audioPreviewPlayer, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private bool TryReadAudioStream(Resource resource, out AudioStream stream)
	{
		stream = null;
		if (!GodotObject.IsInstanceValid(resource))
		{
			return false;
		}
		if (resource is AudioStream audioStream)
		{
			stream = audioStream;
			return true;
		}
		foreach (Dictionary property in resource.GetPropertyList())
		{
			string text = ReadPropertyName(property);
			if (!string.IsNullOrWhiteSpace(text) && IsAudioLike(text))
			{
				Variant variant;
				try
				{
					variant = resource.Get(text);
				}
				catch
				{
					continue;
				}
				if (variant.VariantType == Variant.Type.Object && variant.AsGodotObject() is AudioStream audioStream2)
				{
					stream = audioStream2;
					return true;
				}
			}
		}
		return false;
	}

	private List<(string Name, string Value)> ExtractAudioLikeProperties(Resource resource)
	{
		List<(string, string)> list = new List<(string, string)>();
		if (!GodotObject.IsInstanceValid(resource))
		{
			return list;
		}
		foreach (Dictionary property in resource.GetPropertyList())
		{
			string text = ReadPropertyName(property);
			if (!string.IsNullOrWhiteSpace(text) && IsAudioLike(text))
			{
				Variant value;
				try
				{
					value = resource.Get(text);
				}
				catch
				{
					continue;
				}
				list.Add((text, FormatVariant(value)));
			}
		}
		return list;
	}

	private void AddWaveformMarker(string label, double second, double totalLength)
	{
		double value = ((totalLength <= 0.0) ? 0.0 : Math.Clamp(second / totalLength * 100.0, 0.0, 100.0));
		AddTimelineRow($"{label}: {FormatSeconds(second)} ({value:0.#}%)");
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

	private static string ReadNamedValue(Resource resource, params string[] names)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "未设置";
		}
		foreach (string text in names)
		{
			try
			{
				Variant value = resource.Get(text);
				if (value.VariantType != Variant.Type.Nil)
				{
					return FormatVariant(value);
				}
			}
			catch
			{
			}
		}
		return "未设置";
	}

	private static string ReadBooleanProperty(Resource resource, params string[] names)
	{
		string text = ReadNamedValue(resource, names);
		if (text == "True")
		{
			return "启用";
		}
		if (text == "False")
		{
			return "关闭";
		}
		return text;
	}

	private static string ReadPropertyName(Dictionary property)
	{
		if (property == null || !property.ContainsKey("name"))
		{
			return "";
		}
		return property["name"].AsString();
	}

	private static bool IsAudioLike(string name)
	{
		string text = name.ToLowerInvariant();
		if (!text.Contains("audio") && !text.Contains("sound") && !text.Contains("sfx") && !text.Contains("bgm") && !text.Contains("music") && !text.Contains("stream") && !text.Contains("volume") && !text.Contains("loop") && !text.Contains("fade"))
		{
			switch (text)
			{
			default:
				return text == "win";
			case "entry":
			case "flag1":
			case "drums":
				break;
			}
		}
		return true;
	}

	private static bool IsReferenceLike(string name)
	{
		string text = name.ToLowerInvariant();
		if (!text.Contains("audio") && !text.Contains("sound") && !text.Contains("sfx") && !text.Contains("bgm") && !text.Contains("music") && !text.Contains("stream"))
		{
			switch (text)
			{
			default:
				return text == "win";
			case "entry":
			case "flag1":
			case "drums":
				break;
			}
		}
		return true;
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
				return value.AsBool() ? "True" : "False";
			case 2:
				return value.AsInt64().ToString();
			case 3:
				return value.AsDouble().ToString("0.###");
			case 4:
				goto IL_00b8;
			}
		}
		if (variantType != Variant.Type.StringName)
		{
			Variant.Type num = variantType - 24;
			if ((ulong)num <= 4uL)
			{
				switch ((int)num)
				{
				case 0:
					return FormatObject(value.AsGodotObject());
				case 4:
					return $"数组({value.AsGodotArray().Count})";
				case 3:
					return $"字典({value.AsGodotDictionary().Count})";
				}
			}
			return value.ToString();
		}
		goto IL_00b8;
		IL_00b8:
		return FormatTextValue(value.AsString());
	}

	private static string FormatObject(GodotObject obj)
	{
		if (!GodotObject.IsInstanceValid(obj))
		{
			return "空";
		}
		if (obj is Resource resource)
		{
			if (!string.IsNullOrWhiteSpace(resource.ResourcePath))
			{
				return resource.ResourcePath;
			}
			if (!string.IsNullOrWhiteSpace(resource.ResourceName))
			{
				return resource.ResourceName;
			}
		}
		return obj.GetType().Name;
	}

	private static string FormatTextValue(string text)
	{
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return "未设置";
	}

	private static string FormatSeconds(double seconds)
	{
		if (double.IsNaN(seconds) || double.IsInfinity(seconds) || seconds < 0.0)
		{
			return "未知";
		}
		return $"{seconds:0.00}s";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(43)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenderAudioPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "stream", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AudioStream"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderAudioTimeline, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "stream", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AudioStream"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderBgmBindingGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "bgm", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderBgmAudioSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "bgm", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindBgmAudioField, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "rowPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fieldName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "bgm", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureBgmRuntimePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "panel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "bgm", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PreviewBgmAudioKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "audioKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayBgmMainSegment, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "audioKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "segmentName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartBgmBattleMix, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "bgm", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.StopBgmRuntimePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureBgmPreviewPlayers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateBgmRuntimeStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "bgm", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "drumsVolume", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowBgmAudioSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fieldName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureBgmAudioSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetBgmAudioFieldValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fieldName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetBgmAudioField, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fieldName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "refresh", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnBgmPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshBgmDirectPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshBgmFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReloadBgmEditorAfterHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderAudioReferences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "stream", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AudioStream"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlayAudioPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopAudioPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureAudioPreviewPlayer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddWaveformMarker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "second", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "totalLength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.ReadNamedValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "names", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadBooleanProperty, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "names", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadPropertyName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsAudioLike, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsReferenceLike, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVariant, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatObject, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatTextValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatSeconds, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderAudioPreview && args.Count == 1)
		{
			RenderAudioPreview(VariantUtils.ConvertTo<AudioStream>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderAudioTimeline && args.Count == 1)
		{
			RenderAudioTimeline(VariantUtils.ConvertTo<AudioStream>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderBgmBindingGraph && args.Count == 1)
		{
			RenderBgmBindingGraph(VariantUtils.ConvertTo<TowerDefenseBackgroundMusicConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderBgmAudioSelector && args.Count == 1)
		{
			RenderBgmAudioSelector(VariantUtils.ConvertTo<TowerDefenseBackgroundMusicConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindBgmAudioField && args.Count == 4)
		{
			BindBgmAudioField(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<TowerDefenseBackgroundMusicConfig>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureBgmRuntimePreview && args.Count == 2)
		{
			ConfigureBgmRuntimePreview(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBackgroundMusicConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewBgmAudioKey && args.Count == 1)
		{
			PreviewBgmAudioKey(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayBgmMainSegment && args.Count == 2)
		{
			PlayBgmMainSegment(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartBgmBattleMix && args.Count == 1)
		{
			StartBgmBattleMix(VariantUtils.ConvertTo<TowerDefenseBackgroundMusicConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StopBgmRuntimePreview && args.Count == 0)
		{
			StopBgmRuntimePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged && args.Count == 1)
		{
			OnVisualEditorVisibilityChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureBgmPreviewPlayers && args.Count == 0)
		{
			EnsureBgmPreviewPlayers();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateBgmRuntimeStatus && args.Count == 2)
		{
			UpdateBgmRuntimeStatus(VariantUtils.ConvertTo<TowerDefenseBackgroundMusicConfig>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowBgmAudioSelector && args.Count == 1)
		{
			ShowBgmAudioSelector(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureBgmAudioSelectorWindow && args.Count == 0)
		{
			EnsureBgmAudioSelectorWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.GetBgmAudioFieldValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetBgmAudioFieldValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetBgmAudioField && args.Count == 3)
		{
			SetBgmAudioField(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnBgmPropertyEdited && args.Count == 1)
		{
			OnBgmPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshBgmDirectPreview && args.Count == 0)
		{
			RefreshBgmDirectPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshBgmFromHistory && args.Count == 0)
		{
			RefreshBgmFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.ReloadBgmEditorAfterHistory && args.Count == 0)
		{
			ReloadBgmEditorAfterHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderAudioReferences && args.Count == 2)
		{
			RenderAudioReferences(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<AudioStream>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayAudioPreview && args.Count == 0)
		{
			PlayAudioPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.StopAudioPreview && args.Count == 0)
		{
			StopAudioPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureAudioPreviewPlayer && args.Count == 0)
		{
			EnsureAudioPreviewPlayer();
			ret = default;
			return true;
		}
		if (method == MethodName.AddWaveformMarker && args.Count == 3)
		{
			AddWaveformMarker(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
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
		if (method == MethodName.ReadNamedValue && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadNamedValue(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadBooleanProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadBooleanProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadPropertyName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadPropertyName(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAudioLike && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAudioLike(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsReferenceLike && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsReferenceLike(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatObject && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatObject(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatTextValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatTextValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatSeconds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatSeconds(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadNamedValue && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadNamedValue(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadBooleanProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadBooleanProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadPropertyName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadPropertyName(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAudioLike && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAudioLike(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsReferenceLike && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsReferenceLike(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatObject && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatObject(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatTextValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatTextValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatSeconds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatSeconds(VariantUtils.ConvertTo<double>(in args[0])));
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
		if (method == MethodName.RenderAudioPreview)
		{
			return true;
		}
		if (method == MethodName.RenderAudioTimeline)
		{
			return true;
		}
		if (method == MethodName.RenderBgmBindingGraph)
		{
			return true;
		}
		if (method == MethodName.RenderBgmAudioSelector)
		{
			return true;
		}
		if (method == MethodName.BindBgmAudioField)
		{
			return true;
		}
		if (method == MethodName.ConfigureBgmRuntimePreview)
		{
			return true;
		}
		if (method == MethodName.PreviewBgmAudioKey)
		{
			return true;
		}
		if (method == MethodName.PlayBgmMainSegment)
		{
			return true;
		}
		if (method == MethodName.StartBgmBattleMix)
		{
			return true;
		}
		if (method == MethodName.StopBgmRuntimePreview)
		{
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.EnsureBgmPreviewPlayers)
		{
			return true;
		}
		if (method == MethodName.UpdateBgmRuntimeStatus)
		{
			return true;
		}
		if (method == MethodName.ShowBgmAudioSelector)
		{
			return true;
		}
		if (method == MethodName.EnsureBgmAudioSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.GetBgmAudioFieldValue)
		{
			return true;
		}
		if (method == MethodName.SetBgmAudioField)
		{
			return true;
		}
		if (method == MethodName.OnBgmPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshBgmDirectPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshBgmFromHistory)
		{
			return true;
		}
		if (method == MethodName.ReloadBgmEditorAfterHistory)
		{
			return true;
		}
		if (method == MethodName.RenderAudioReferences)
		{
			return true;
		}
		if (method == MethodName.PlayAudioPreview)
		{
			return true;
		}
		if (method == MethodName.StopAudioPreview)
		{
			return true;
		}
		if (method == MethodName.EnsureAudioPreviewPlayer)
		{
			return true;
		}
		if (method == MethodName.AddWaveformMarker)
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
		if (method == MethodName.ReadNamedValue)
		{
			return true;
		}
		if (method == MethodName.ReadBooleanProperty)
		{
			return true;
		}
		if (method == MethodName.ReadPropertyName)
		{
			return true;
		}
		if (method == MethodName.IsAudioLike)
		{
			return true;
		}
		if (method == MethodName.IsReferenceLike)
		{
			return true;
		}
		if (method == MethodName.FormatVariant)
		{
			return true;
		}
		if (method == MethodName.FormatObject)
		{
			return true;
		}
		if (method == MethodName.FormatTextValue)
		{
			return true;
		}
		if (method == MethodName.FormatSeconds)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._audioPreviewPlayer)
		{
			_audioPreviewPlayer = VariantUtils.ConvertTo<AudioStreamPlayer>(in value);
			return true;
		}
		if (name == PropertyName._bgmMainPreviewPlayer)
		{
			_bgmMainPreviewPlayer = VariantUtils.ConvertTo<AudioStreamPlayer>(in value);
			return true;
		}
		if (name == PropertyName._bgmDrumsPreviewPlayer)
		{
			_bgmDrumsPreviewPlayer = VariantUtils.ConvertTo<AudioStreamPlayer>(in value);
			return true;
		}
		if (name == PropertyName._bgmZombieCountSlider)
		{
			_bgmZombieCountSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._bgmRuntimeStatus)
		{
			_bgmRuntimeStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._bgmBattlePreviewActive)
		{
			_bgmBattlePreviewActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._bgmAudioSelector)
		{
			_bgmAudioSelector = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._bgmAudioSelectorField)
		{
			_bgmAudioSelectorField = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._bgmRefreshQueued)
		{
			_bgmRefreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._audioPreviewPlayer)
		{
			value = VariantUtils.CreateFrom(in _audioPreviewPlayer);
			return true;
		}
		if (name == PropertyName._bgmMainPreviewPlayer)
		{
			value = VariantUtils.CreateFrom(in _bgmMainPreviewPlayer);
			return true;
		}
		if (name == PropertyName._bgmDrumsPreviewPlayer)
		{
			value = VariantUtils.CreateFrom(in _bgmDrumsPreviewPlayer);
			return true;
		}
		if (name == PropertyName._bgmZombieCountSlider)
		{
			value = VariantUtils.CreateFrom(in _bgmZombieCountSlider);
			return true;
		}
		if (name == PropertyName._bgmRuntimeStatus)
		{
			value = VariantUtils.CreateFrom(in _bgmRuntimeStatus);
			return true;
		}
		if (name == PropertyName._bgmBattlePreviewActive)
		{
			value = VariantUtils.CreateFrom(in _bgmBattlePreviewActive);
			return true;
		}
		if (name == PropertyName._bgmAudioSelector)
		{
			value = VariantUtils.CreateFrom(in _bgmAudioSelector);
			return true;
		}
		if (name == PropertyName._bgmAudioSelectorField)
		{
			value = VariantUtils.CreateFrom(in _bgmAudioSelectorField);
			return true;
		}
		if (name == PropertyName._bgmRefreshQueued)
		{
			value = VariantUtils.CreateFrom(in _bgmRefreshQueued);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._audioPreviewPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bgmMainPreviewPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bgmDrumsPreviewPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bgmZombieCountSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bgmRuntimeStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._bgmBattlePreviewActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bgmAudioSelector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._bgmAudioSelectorField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._bgmRefreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._audioPreviewPlayer, Variant.From(in _audioPreviewPlayer));
		info.AddProperty(PropertyName._bgmMainPreviewPlayer, Variant.From(in _bgmMainPreviewPlayer));
		info.AddProperty(PropertyName._bgmDrumsPreviewPlayer, Variant.From(in _bgmDrumsPreviewPlayer));
		info.AddProperty(PropertyName._bgmZombieCountSlider, Variant.From(in _bgmZombieCountSlider));
		info.AddProperty(PropertyName._bgmRuntimeStatus, Variant.From(in _bgmRuntimeStatus));
		info.AddProperty(PropertyName._bgmBattlePreviewActive, Variant.From(in _bgmBattlePreviewActive));
		info.AddProperty(PropertyName._bgmAudioSelector, Variant.From(in _bgmAudioSelector));
		info.AddProperty(PropertyName._bgmAudioSelectorField, Variant.From(in _bgmAudioSelectorField));
		info.AddProperty(PropertyName._bgmRefreshQueued, Variant.From(in _bgmRefreshQueued));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._audioPreviewPlayer, out var value))
		{
			_audioPreviewPlayer = value.As<AudioStreamPlayer>();
		}
		if (info.TryGetProperty(PropertyName._bgmMainPreviewPlayer, out var value2))
		{
			_bgmMainPreviewPlayer = value2.As<AudioStreamPlayer>();
		}
		if (info.TryGetProperty(PropertyName._bgmDrumsPreviewPlayer, out var value3))
		{
			_bgmDrumsPreviewPlayer = value3.As<AudioStreamPlayer>();
		}
		if (info.TryGetProperty(PropertyName._bgmZombieCountSlider, out var value4))
		{
			_bgmZombieCountSlider = value4.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._bgmRuntimeStatus, out var value5))
		{
			_bgmRuntimeStatus = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._bgmBattlePreviewActive, out var value6))
		{
			_bgmBattlePreviewActive = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._bgmAudioSelector, out var value7))
		{
			_bgmAudioSelector = value7.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._bgmAudioSelectorField, out var value8))
		{
			_bgmAudioSelectorField = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName._bgmRefreshQueued, out var value9))
		{
			_bgmRefreshQueued = value9.As<bool>();
		}
	}
}
