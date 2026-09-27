using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/MenuDialog/MenuDialogBase.cs")]
public class MenuDialogBase : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RefreshFrameRateOptions = "RefreshFrameRateOptions";

		public static readonly StringName FindFrameRateIndex = "FindFrameRateIndex";

		public static readonly StringName AnimeFrameRateSliderValueChanged = "AnimeFrameRateSliderValueChanged";

		public static readonly StringName AnimeFrameRateSliderDragEnded = "AnimeFrameRateSliderDragEnded";

		public static readonly StringName UpdateFrameRateLabel = "UpdateFrameRateLabel";

		public static readonly StringName GpuOptimizationCheckBoxPressed = "GpuOptimizationCheckBoxPressed";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName SliderPressed = "SliderPressed";

		public static readonly StringName SfxSliderRelease = "SfxSliderRelease";

		public static readonly StringName MusicSliderRelease = "MusicSliderRelease";

		public static readonly StringName MusicValueChanged = "MusicValueChanged";

		public static readonly StringName SFXValueChanged = "SFXValueChanged";

		public static readonly StringName FullScreenCheckBoxPressed = "FullScreenCheckBoxPressed";

		public static readonly StringName MobilePresetCheckBoxPressed = "MobilePresetCheckBoxPressed";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName fullScreenCheckBox = "fullScreenCheckBox";

		public static readonly StringName mobilePresetCheckBox = "mobilePresetCheckBox";

		public static readonly StringName musicSlider = "musicSlider";

		public static readonly StringName sfxSlider = "sfxSlider";

		public static readonly StringName backButton = "backButton";

		public static readonly StringName animeFrameRateLabel = "animeFrameRateLabel";

		public static readonly StringName animeFrameRateSlider = "animeFrameRateSlider";

		public static readonly StringName gpuOptimizationCheckBox = "gpuOptimizationCheckBox";

		public static readonly StringName fullScreenLabel = "fullScreenLabel";

		public static readonly StringName mobilePresetLabel = "mobilePresetLabel";

		public static readonly StringName _frameRateOptions = "_frameRateOptions";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	private CheckBox fullScreenCheckBox;

	private CheckBox mobilePresetCheckBox;

	private HSlider musicSlider;

	private HSlider sfxSlider;

	private TextureButton backButton;

	private Label animeFrameRateLabel;

	private HSlider animeFrameRateSlider;

	private CheckBox gpuOptimizationCheckBox;

	private Label fullScreenLabel;

	private Label mobilePresetLabel;

	private int[] _frameRateOptions = Array.Empty<int>();

	public override void _Ready()
	{
		fullScreenCheckBox = GetNode<CheckBox>("%FullScreenCheckBox");
		mobilePresetCheckBox = GetNode<CheckBox>("%MobilePresetCheckBox");
		musicSlider = GetNode<HSlider>("%MusicHSlider");
		sfxSlider = GetNode<HSlider>("%SfxHSlider");
		backButton = GetNode<TextureButton>("%BackButton");
		animeFrameRateLabel = GetNode<Label>("%AnimeFrameRateLabel");
		animeFrameRateSlider = GetNode<HSlider>("%AnimeFrameRateSlider");
		gpuOptimizationCheckBox = GetNode<CheckBox>("%GpuOptimizationCheckBox");
		fullScreenLabel = GetNode<Label>("%FullScreenLabel");
		mobilePresetLabel = GetNode<Label>("%MobilePresetLabel");
		GetNode<CheckBox>("%FullScreenCheckBox").Pressed += FullScreenCheckBoxPressed;
		GetNode<CheckBox>("%MobilePresetCheckBox").Pressed += MobilePresetCheckBoxPressed;
		gpuOptimizationCheckBox.Pressed += GpuOptimizationCheckBoxPressed;
		GetNode<HSlider>("%MusicHSlider").DragEnded += MusicSliderRelease;
		GetNode<HSlider>("%MusicHSlider").DragStarted += SliderPressed;
		GetNode<HSlider>("%MusicHSlider").ValueChanged += MusicValueChanged;
		GetNode<HSlider>("%SfxHSlider").DragEnded += SfxSliderRelease;
		GetNode<HSlider>("%SfxHSlider").DragStarted += SliderPressed;
		GetNode<HSlider>("%SfxHSlider").ValueChanged += SFXValueChanged;
		GetNode<BaseButton>("%BackButton").Pressed += CloseDialog;
		base._Ready();
		Global.Instance.OnFrameRateOptionsChanged += RefreshFrameRateOptions;
		animeFrameRateSlider.DragEnded += AnimeFrameRateSliderDragEnded;
		animeFrameRateSlider.ValueChanged += AnimeFrameRateSliderValueChanged;
		RefreshFrameRateOptions();
		if (Global.Instance.isMobile)
		{
			fullScreenLabel.Visible = false;
		}
		AudioManager.Instance.AudioPlay("GraveButtonPress");
		musicSlider.CallDeferred("set_value", AudioManager.Instance.VolumGet(AudioManagerEnum.TYPE.MUSIC));
		sfxSlider.CallDeferred("set_value", AudioManager.Instance.VolumGet());
		fullScreenCheckBox.ButtonPressed = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;
		mobilePresetCheckBox.ButtonPressed = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
		gpuOptimizationCheckBox.ButtonPressed = Global.Instance.adobeAnimateRenderBackend == AdobeAnimateRenderBackend.GpuCrowd;
	}

	private void RefreshFrameRateOptions()
	{
		_frameRateOptions = Global.Instance.GetFrameRateOptions();
		if (_frameRateOptions.Length == 0)
		{
			_frameRateOptions = new int[1] { 60 };
		}
		animeFrameRateSlider.MaxValue = Math.Max(0, _frameRateOptions.Length - 1);
		animeFrameRateSlider.Value = FindFrameRateIndex(Global.Instance.animeFrameRate);
		UpdateFrameRateLabel();
	}

	private int FindFrameRateIndex(double value)
	{
		int num = 0;
		int num2 = _frameRateOptions.Length;
		while (num < num2)
		{
			int num3 = (num + num2) / 2;
			if ((double)_frameRateOptions[num3] < value)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		return Mathf.Clamp(num, 0, _frameRateOptions.Length - 1);
	}

	private void AnimeFrameRateSliderValueChanged(double value)
	{
		int num = Mathf.Clamp((int)Math.Round(value), 0, _frameRateOptions.Length - 1);
		animeFrameRateLabel.Text = $"动画帧率:{_frameRateOptions[num]}";
	}

	private void AnimeFrameRateSliderDragEnded(bool valueChanged)
	{
		if (valueChanged)
		{
			int num = Mathf.Clamp((int)animeFrameRateSlider.Value, 0, _frameRateOptions.Length - 1);
			Global.Instance.animeFrameRate = _frameRateOptions[num];
			GameSaveManager.Instance.SetConfigValue("AnimeFrameRate", Global.Instance.animeFrameRate);
			UpdateFrameRateLabel();
		}
	}

	private void UpdateFrameRateLabel()
	{
		int num = FrameRatePolicy.NormalizeRefreshRate(Global.Instance.animeFrameRate);
		int effectiveAnimeFrameRate = Global.Instance.effectiveAnimeFrameRate;
		animeFrameRateLabel.Text = ((num == effectiveAnimeFrameRate) ? $"动画帧率:{num}" : $"动画帧率:{num}（当前{effectiveAnimeFrameRate}）");
	}

	private void GpuOptimizationCheckBoxPressed()
	{
		AdobeAnimateRenderBackend adobeAnimateRenderBackend = ((!gpuOptimizationCheckBox.ButtonPressed) ? AdobeAnimateRenderBackend.CpuPose : AdobeAnimateRenderBackend.GpuCrowd);
		Global.Instance.adobeAnimateRenderBackend = adobeAnimateRenderBackend;
		GameSaveManager.Instance.SetConfigValue("AdobeAnimateRenderBackend", (int)adobeAnimateRenderBackend);
		AudioManager.Instance.AudioPlay("ButtonPress");
	}

	public override void _ExitTree()
	{
		if (Global.Instance != null)
		{
			Global.Instance.OnFrameRateOptionsChanged -= RefreshFrameRateOptions;
		}
	}

	public void SliderPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
	}

	public void SfxSliderRelease(bool valueChanged)
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		if (valueChanged)
		{
			GameSaveManager.Instance.SetConfigValue("SfxVolum", sfxSlider.Value);
		}
	}

	public void MusicSliderRelease(bool valueChanged)
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		if (valueChanged)
		{
			GameSaveManager.Instance.SetConfigValue("MusicVolum", musicSlider.Value);
		}
	}

	public void MusicValueChanged(double value)
	{
		AudioManager.Instance.VolumSet(AudioManagerEnum.TYPE.MUSIC, value);
	}

	public void SFXValueChanged(double value)
	{
		AudioManager.Instance.VolumSet(AudioManagerEnum.TYPE.SFX, value);
	}

	public void FullScreenCheckBoxPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		if (fullScreenCheckBox.ButtonPressed)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
		GameSaveManager.Instance.SetConfigValue("FullScreen", fullScreenCheckBox.ButtonPressed);
	}

	public void MobilePresetCheckBoxPressed()
	{
		BattleEventBus.Instance.EmitUiSwitched(mobilePresetCheckBox.ButtonPressed);
		AudioManager.Instance.AudioPlay("ButtonPress");
		GameSaveManager.Instance.SetConfigValue("MobilePreset", mobilePresetCheckBox.ButtonPressed);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshFrameRateOptions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindFrameRateIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeFrameRateSliderValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeFrameRateSliderDragEnded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "valueChanged", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateFrameRateLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GpuOptimizationCheckBoxPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SliderPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SfxSliderRelease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "valueChanged", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MusicSliderRelease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "valueChanged", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MusicValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SFXValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FullScreenCheckBoxPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MobilePresetCheckBoxPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.RefreshFrameRateOptions && args.Count == 0)
		{
			RefreshFrameRateOptions();
			ret = default;
			return true;
		}
		if (method == MethodName.FindFrameRateIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindFrameRateIndex(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.AnimeFrameRateSliderValueChanged && args.Count == 1)
		{
			AnimeFrameRateSliderValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeFrameRateSliderDragEnded && args.Count == 1)
		{
			AnimeFrameRateSliderDragEnded(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateFrameRateLabel && args.Count == 0)
		{
			UpdateFrameRateLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.GpuOptimizationCheckBoxPressed && args.Count == 0)
		{
			GpuOptimizationCheckBoxPressed();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.SliderPressed && args.Count == 0)
		{
			SliderPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SfxSliderRelease && args.Count == 1)
		{
			SfxSliderRelease(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MusicSliderRelease && args.Count == 1)
		{
			MusicSliderRelease(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MusicValueChanged && args.Count == 1)
		{
			MusicValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SFXValueChanged && args.Count == 1)
		{
			SFXValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FullScreenCheckBoxPressed && args.Count == 0)
		{
			FullScreenCheckBoxPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.MobilePresetCheckBoxPressed && args.Count == 0)
		{
			MobilePresetCheckBoxPressed();
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
		if (method == MethodName.RefreshFrameRateOptions)
		{
			return true;
		}
		if (method == MethodName.FindFrameRateIndex)
		{
			return true;
		}
		if (method == MethodName.AnimeFrameRateSliderValueChanged)
		{
			return true;
		}
		if (method == MethodName.AnimeFrameRateSliderDragEnded)
		{
			return true;
		}
		if (method == MethodName.UpdateFrameRateLabel)
		{
			return true;
		}
		if (method == MethodName.GpuOptimizationCheckBoxPressed)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.SliderPressed)
		{
			return true;
		}
		if (method == MethodName.SfxSliderRelease)
		{
			return true;
		}
		if (method == MethodName.MusicSliderRelease)
		{
			return true;
		}
		if (method == MethodName.MusicValueChanged)
		{
			return true;
		}
		if (method == MethodName.SFXValueChanged)
		{
			return true;
		}
		if (method == MethodName.FullScreenCheckBoxPressed)
		{
			return true;
		}
		if (method == MethodName.MobilePresetCheckBoxPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.fullScreenCheckBox)
		{
			fullScreenCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.mobilePresetCheckBox)
		{
			mobilePresetCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.musicSlider)
		{
			musicSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName.sfxSlider)
		{
			sfxSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName.backButton)
		{
			backButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.animeFrameRateLabel)
		{
			animeFrameRateLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.animeFrameRateSlider)
		{
			animeFrameRateSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName.gpuOptimizationCheckBox)
		{
			gpuOptimizationCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.fullScreenLabel)
		{
			fullScreenLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.mobilePresetLabel)
		{
			mobilePresetLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._frameRateOptions)
		{
			_frameRateOptions = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.fullScreenCheckBox)
		{
			value = VariantUtils.CreateFrom(in fullScreenCheckBox);
			return true;
		}
		if (name == PropertyName.mobilePresetCheckBox)
		{
			value = VariantUtils.CreateFrom(in mobilePresetCheckBox);
			return true;
		}
		if (name == PropertyName.musicSlider)
		{
			value = VariantUtils.CreateFrom(in musicSlider);
			return true;
		}
		if (name == PropertyName.sfxSlider)
		{
			value = VariantUtils.CreateFrom(in sfxSlider);
			return true;
		}
		if (name == PropertyName.backButton)
		{
			value = VariantUtils.CreateFrom(in backButton);
			return true;
		}
		if (name == PropertyName.animeFrameRateLabel)
		{
			value = VariantUtils.CreateFrom(in animeFrameRateLabel);
			return true;
		}
		if (name == PropertyName.animeFrameRateSlider)
		{
			value = VariantUtils.CreateFrom(in animeFrameRateSlider);
			return true;
		}
		if (name == PropertyName.gpuOptimizationCheckBox)
		{
			value = VariantUtils.CreateFrom(in gpuOptimizationCheckBox);
			return true;
		}
		if (name == PropertyName.fullScreenLabel)
		{
			value = VariantUtils.CreateFrom(in fullScreenLabel);
			return true;
		}
		if (name == PropertyName.mobilePresetLabel)
		{
			value = VariantUtils.CreateFrom(in mobilePresetLabel);
			return true;
		}
		if (name == PropertyName._frameRateOptions)
		{
			value = VariantUtils.CreateFrom(in _frameRateOptions);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.fullScreenCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobilePresetCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.musicSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sfxSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.backButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.animeFrameRateLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.animeFrameRateSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.gpuOptimizationCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.fullScreenLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobilePresetLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._frameRateOptions, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fullScreenCheckBox, Variant.From(in fullScreenCheckBox));
		info.AddProperty(PropertyName.mobilePresetCheckBox, Variant.From(in mobilePresetCheckBox));
		info.AddProperty(PropertyName.musicSlider, Variant.From(in musicSlider));
		info.AddProperty(PropertyName.sfxSlider, Variant.From(in sfxSlider));
		info.AddProperty(PropertyName.backButton, Variant.From(in backButton));
		info.AddProperty(PropertyName.animeFrameRateLabel, Variant.From(in animeFrameRateLabel));
		info.AddProperty(PropertyName.animeFrameRateSlider, Variant.From(in animeFrameRateSlider));
		info.AddProperty(PropertyName.gpuOptimizationCheckBox, Variant.From(in gpuOptimizationCheckBox));
		info.AddProperty(PropertyName.fullScreenLabel, Variant.From(in fullScreenLabel));
		info.AddProperty(PropertyName.mobilePresetLabel, Variant.From(in mobilePresetLabel));
		info.AddProperty(PropertyName._frameRateOptions, Variant.From(in _frameRateOptions));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fullScreenCheckBox, out var value))
		{
			fullScreenCheckBox = value.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.mobilePresetCheckBox, out var value2))
		{
			mobilePresetCheckBox = value2.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.musicSlider, out var value3))
		{
			musicSlider = value3.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName.sfxSlider, out var value4))
		{
			sfxSlider = value4.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName.backButton, out var value5))
		{
			backButton = value5.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.animeFrameRateLabel, out var value6))
		{
			animeFrameRateLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.animeFrameRateSlider, out var value7))
		{
			animeFrameRateSlider = value7.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName.gpuOptimizationCheckBox, out var value8))
		{
			gpuOptimizationCheckBox = value8.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.fullScreenLabel, out var value9))
		{
			fullScreenLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.mobilePresetLabel, out var value10))
		{
			mobilePresetLabel = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._frameRateOptions, out var value11))
		{
			_frameRateOptions = value11.As<int[]>();
		}
	}
}
