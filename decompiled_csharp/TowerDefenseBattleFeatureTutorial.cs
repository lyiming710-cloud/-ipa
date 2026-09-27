using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Tutorial/TowerDefenseBattleFeatureTutorial.cs")]
public class TowerDefenseBattleFeatureTutorial : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName OnTutorialFinished = "OnTutorialFinished";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName config = "config";

		public static readonly StringName _tutorialActive = "_tutorialActive";

		public static readonly StringName _activeTutorialConfig = "_activeTutorialConfig";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	public TutorialConfig config;

	private bool _tutorialActive;

	private TutorialConfig _activeTutorialConfig;

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		if (!data.GetValueOrDefault("isCustom", false).AsBool())
		{
			string text = data.GetValueOrDefault("TutorialName", "").AsString();
			if (!(text != ""))
			{
				return;
			}
			TutorialConfig tutorial = TowerDefenseManager.GetTutorial(text);
			if (!GodotObject.IsInstanceValid(tutorial))
			{
				GD.PushWarning("[Tutorial] Tutorial '" + text + "' is unavailable.");
				return;
			}
			config = tutorial.Duplicate(deep: true) as TutorialConfig;
			if (GodotObject.IsInstanceValid(config))
			{
				config.Init();
			}
		}
		else
		{
			config = new TutorialConfig();
			config.Load(data);
		}
	}

	public override Task GameStart()
	{
		if (IsLifetimeActive && GodotObject.IsInstanceValid(config))
		{
			if (config.saveKey != "" && GameSaveManager.Instance.GetTutorialValue(config.saveKey))
			{
				return Task.CompletedTask;
			}
			if (!GodotObject.IsInstanceValid(TutorialManager.Instance))
			{
				return Task.CompletedTask;
			}
			_tutorialActive = true;
			_activeTutorialConfig = config;
			TutorialManager.Instance.OnTutorialFinish -= OnTutorialFinished;
			TutorialManager.Instance.OnTutorialFinish += OnTutorialFinished;
			TutorialManager.Instance.StartTutorial(config);
		}
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		return Task.CompletedTask;
	}

	private void OnTutorialFinished()
	{
		if (GodotObject.IsInstanceValid(TutorialManager.Instance))
		{
			TutorialManager.Instance.OnTutorialFinish -= OnTutorialFinished;
		}
		_tutorialActive = false;
		_activeTutorialConfig = null;
	}

	public override void Destroy()
	{
		base.Destroy();
		if (GodotObject.IsInstanceValid(TutorialManager.Instance))
		{
			TutorialManager.Instance.OnTutorialFinish -= OnTutorialFinished;
			if (_tutorialActive && TutorialManager.Instance.currentTutoroal == _activeTutorialConfig)
			{
				TutorialManager.Instance.TutorialClear();
			}
		}
		_tutorialActive = false;
		_activeTutorialConfig = null;
		config = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTutorialFinished, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.OnTutorialFinished && args.Count == 0)
		{
			OnTutorialFinished();
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
		if (method == MethodName.OnTutorialFinished)
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
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TutorialConfig>(in value);
			return true;
		}
		if (name == PropertyName._tutorialActive)
		{
			_tutorialActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._activeTutorialConfig)
		{
			_activeTutorialConfig = VariantUtils.ConvertTo<TutorialConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName._tutorialActive)
		{
			value = VariantUtils.CreateFrom(in _tutorialActive);
			return true;
		}
		if (name == PropertyName._activeTutorialConfig)
		{
			value = VariantUtils.CreateFrom(in _activeTutorialConfig);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._tutorialActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._activeTutorialConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName._tutorialActive, Variant.From(in _tutorialActive));
		info.AddProperty(PropertyName._activeTutorialConfig, Variant.From(in _activeTutorialConfig));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.config, out var value))
		{
			config = value.As<TutorialConfig>();
		}
		if (info.TryGetProperty(PropertyName._tutorialActive, out var value2))
		{
			_tutorialActive = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._activeTutorialConfig, out var value3))
		{
			_activeTutorialConfig = value3.As<TutorialConfig>();
		}
	}
}
