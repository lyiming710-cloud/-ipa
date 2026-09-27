using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/BGM/TowerDefenseBattleFeatureBGM.cs")]
public class TowerDefenseBattleFeatureBGM : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName Process = "Process";

		public static readonly StringName PlayEntryBGM = "PlayEntryBGM";

		public static readonly StringName StopBGM = "StopBGM";

		public new static readonly StringName GameFail = "GameFail";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName backgroundAudio = "backgroundAudio";

		public static readonly StringName backgroundDrumsAudio = "backgroundDrumsAudio";

		public static readonly StringName backgroundMusicConfig = "backgroundMusicConfig";

		public static readonly StringName _drumsCheckTimer = "_drumsCheckTimer";

		public static readonly StringName _drumsTargetVolume = "_drumsTargetVolume";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	public AudioStreamPlayerMember backgroundAudio;

	public AudioStreamPlayerMember backgroundDrumsAudio;

	public TowerDefenseBackgroundMusicConfig backgroundMusicConfig;

	private double _drumsCheckTimer;

	private float _drumsTargetVolume;

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		string text = data.GetValueOrDefault("BackgroundMusic", data.GetValueOrDefault("BGMName", "")).AsString();
		if (text != "")
		{
			backgroundMusicConfig = TowerDefenseManager.Instance.GetbackgroundMusicConfig(text);
		}
	}

	public override void Process(double delta)
	{
		if (!GodotObject.IsInstanceValid(backgroundDrumsAudio))
		{
			return;
		}
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(instance.characterRegistry))
		{
			_drumsCheckTimer -= Math.Max(0.0, delta);
			if (_drumsCheckTimer <= 0.0)
			{
				int num = Math.Max(0, backgroundMusicConfig?.drumsZombieThreshold ?? 10);
				_drumsTargetVolume = ((instance.characterRegistry.GetZombieCount() >= num) ? 1f : 0f);
				_drumsCheckTimer = Mathf.Max(0.05f, backgroundMusicConfig?.drumsCheckInterval ?? 0.25f);
			}
			float num2 = (float)backgroundDrumsAudio.volumeScale;
			if (Mathf.Abs(num2 - _drumsTargetVolume) <= 0.001f)
			{
				backgroundDrumsAudio.volumeScale = _drumsTargetVolume;
				return;
			}
			float num3 = Mathf.Max(0.01f, backgroundMusicConfig?.drumsFadeSpeed ?? 1f);
			float weight = 1f - Mathf.Exp((0f - num3) * (float)Math.Max(0.0, delta));
			backgroundDrumsAudio.volumeScale = Mathf.Lerp(num2, _drumsTargetVolume, weight);
		}
	}

	public override Task GameEntry()
	{
		PlayEntryBGM();
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		StopBGM();
		_drumsCheckTimer = 0.0;
		_drumsTargetVolume = 0f;
		if (GodotObject.IsInstanceValid(backgroundMusicConfig))
		{
			if (!string.IsNullOrWhiteSpace(backgroundMusicConfig.flag1))
			{
				backgroundAudio = AudioManager.Instance.AudioPlay(backgroundMusicConfig.flag1, AudioManagerEnum.TYPE.MUSIC, 0.0, once: false);
			}
			if (!string.IsNullOrWhiteSpace(backgroundMusicConfig.drums))
			{
				backgroundDrumsAudio = AudioManager.Instance.AudioPlay(backgroundMusicConfig.drums, AudioManagerEnum.TYPE.MUSIC, 0.0, once: false);
				if (GodotObject.IsInstanceValid(backgroundDrumsAudio))
				{
					backgroundDrumsAudio.volumeScale = 0.0;
				}
			}
		}
		return Task.CompletedTask;
	}

	public override Task GameReady()
	{
		StopBGM();
		return Task.CompletedTask;
	}

	public void PlayEntryBGM()
	{
		if (GodotObject.IsInstanceValid(backgroundMusicConfig))
		{
			if (GodotObject.IsInstanceValid(backgroundAudio))
			{
				backgroundAudio.Stop();
			}
			if (GodotObject.IsInstanceValid(backgroundDrumsAudio))
			{
				backgroundDrumsAudio.Stop();
			}
			if (!string.IsNullOrWhiteSpace(backgroundMusicConfig.entry))
			{
				backgroundAudio = AudioManager.Instance.AudioPlay(backgroundMusicConfig.entry, AudioManagerEnum.TYPE.MUSIC, 0.0, once: false);
			}
		}
	}

	public void StopBGM()
	{
		if (GodotObject.IsInstanceValid(backgroundAudio))
		{
			backgroundAudio.Stop();
		}
		if (GodotObject.IsInstanceValid(backgroundDrumsAudio))
		{
			backgroundDrumsAudio.Stop();
		}
	}

	public override void GameFail()
	{
		StopBGM();
	}

	public override void Destroy()
	{
		StopBGM();
		backgroundAudio = null;
		backgroundDrumsAudio = null;
		backgroundMusicConfig = null;
		_drumsCheckTimer = 0.0;
		_drumsTargetVolume = 0f;
		base.Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayEntryBGM, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopBGM, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayEntryBGM && args.Count == 0)
		{
			PlayEntryBGM();
			ret = default;
			return true;
		}
		if (method == MethodName.StopBGM && args.Count == 0)
		{
			StopBGM();
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 0)
		{
			GameFail();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Process)
		{
			return true;
		}
		if (method == MethodName.PlayEntryBGM)
		{
			return true;
		}
		if (method == MethodName.StopBGM)
		{
			return true;
		}
		if (method == MethodName.GameFail)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.backgroundAudio)
		{
			backgroundAudio = VariantUtils.ConvertTo<AudioStreamPlayerMember>(in value);
			return true;
		}
		if (name == PropertyName.backgroundDrumsAudio)
		{
			backgroundDrumsAudio = VariantUtils.ConvertTo<AudioStreamPlayerMember>(in value);
			return true;
		}
		if (name == PropertyName.backgroundMusicConfig)
		{
			backgroundMusicConfig = VariantUtils.ConvertTo<TowerDefenseBackgroundMusicConfig>(in value);
			return true;
		}
		if (name == PropertyName._drumsCheckTimer)
		{
			_drumsCheckTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._drumsTargetVolume)
		{
			_drumsTargetVolume = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.backgroundAudio)
		{
			value = VariantUtils.CreateFrom(in backgroundAudio);
			return true;
		}
		if (name == PropertyName.backgroundDrumsAudio)
		{
			value = VariantUtils.CreateFrom(in backgroundDrumsAudio);
			return true;
		}
		if (name == PropertyName.backgroundMusicConfig)
		{
			value = VariantUtils.CreateFrom(in backgroundMusicConfig);
			return true;
		}
		if (name == PropertyName._drumsCheckTimer)
		{
			value = VariantUtils.CreateFrom(in _drumsCheckTimer);
			return true;
		}
		if (name == PropertyName._drumsTargetVolume)
		{
			value = VariantUtils.CreateFrom(in _drumsTargetVolume);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.backgroundAudio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.backgroundDrumsAudio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.backgroundMusicConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._drumsCheckTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._drumsTargetVolume, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.backgroundAudio, Variant.From(in backgroundAudio));
		info.AddProperty(PropertyName.backgroundDrumsAudio, Variant.From(in backgroundDrumsAudio));
		info.AddProperty(PropertyName.backgroundMusicConfig, Variant.From(in backgroundMusicConfig));
		info.AddProperty(PropertyName._drumsCheckTimer, Variant.From(in _drumsCheckTimer));
		info.AddProperty(PropertyName._drumsTargetVolume, Variant.From(in _drumsTargetVolume));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.backgroundAudio, out var value))
		{
			backgroundAudio = value.As<AudioStreamPlayerMember>();
		}
		if (info.TryGetProperty(PropertyName.backgroundDrumsAudio, out var value2))
		{
			backgroundDrumsAudio = value2.As<AudioStreamPlayerMember>();
		}
		if (info.TryGetProperty(PropertyName.backgroundMusicConfig, out var value3))
		{
			backgroundMusicConfig = value3.As<TowerDefenseBackgroundMusicConfig>();
		}
		if (info.TryGetProperty(PropertyName._drumsCheckTimer, out var value4))
		{
			_drumsCheckTimer = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._drumsTargetVolume, out var value5))
		{
			_drumsTargetVolume = value5.As<float>();
		}
	}
}
