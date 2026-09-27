using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Scene/TowerDefesne/TowerDefenseControl.cs")]
public class TowerDefenseControl : Node
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Init = "Init";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ButtonPauseToggled = "ButtonPauseToggled";

		public static readonly StringName CheckBox2XToggled = "CheckBox2XToggled";

		public static readonly StringName OptionButtonPressed = "OptionButtonPressed";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName buttonPause = "buttonPause";

		public static readonly StringName checkBox2X = "checkBox2X";

		public static readonly StringName optionButton = "optionButton";

		public static readonly StringName levelConfig = "levelConfig";

		public static readonly StringName hasProgress = "hasProgress";
	}

	public new class SignalName : Node.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public MainButton buttonPause;

	[Export(PropertyHint.None, "")]
	public CheckBox checkBox2X;

	[Export(PropertyHint.None, "")]
	public SpriteBrightButton optionButton;

	public TowerDefenseLevelBaseConfig levelConfig;

	public bool hasProgress;

	public virtual void Init(TowerDefenseLevelBaseConfig _levelConfig)
	{
		levelConfig = _levelConfig;
		levelConfig.Init();
		TowerDefenseManager.Instance.currentLevelConfig = levelConfig;
		if (this is TowerDefenseControlNew currentControl)
		{
			TowerDefenseManager.Instance.currentControl = currentControl;
		}
	}

	public override void _Ready()
	{
		if (buttonPause != null)
		{
			buttonPause.Visible = false;
			buttonPause.Toggled += ButtonPauseToggled;
		}
		if (checkBox2X != null)
		{
			checkBox2X.Visible = false;
			checkBox2X.Toggled += CheckBox2XToggled;
		}
		if (optionButton != null)
		{
			optionButton.Visible = false;
			optionButton.OnPressed += OptionButtonPressed;
		}
	}

	public void ButtonPauseToggled(bool toggled)
	{
		if (!toggled)
		{
			return;
		}
		AudioManager.Instance.AudioPlay("Pause", AudioManagerEnum.TYPE.SFX, 0.0, once: true, pauseAlive: true);
		DialogManager.Instance.DialogCreate("BattlePause").OnClose += () =>
		{
			if (GodotObject.IsInstanceValid(buttonPause))
			{
				buttonPause.ButtonPressed = false;
			}
			if (Global.IsMultiplayerMode && MultiPlayerManager.Instance.IsConnect())
			{
				if (this is TowerDefenseControlNew towerDefenseControlNew)
				{
					towerDefenseControlNew.ApplyNetworkPauseFromSession(paused: false);
				}
				MultiPlayerManager.Instance.SendResume();
			}
		};
	}

	public void CheckBox2XToggled(bool toggled)
	{
		if (toggled)
		{
			AudioManager.Instance.AudioPlay("2XSpeedOn");
			if (levelConfig is TowerDefenseLevelConfig { finishMethod: not TowerDefenseEnum.LEVEL_FINISH_METHOD.QUIZ } towerDefenseLevelConfig)
			{
				Global.TimeScale = towerDefenseLevelConfig.baseTimeScale * 1.5;
			}
			else if (levelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig2)
			{
				Global.TimeScale = towerDefenseLevelConfig2.baseTimeScale * 3.0;
			}
		}
		else
		{
			AudioManager.Instance.AudioPlay("2XSpeedDown");
			Global.TimeScale = ((levelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig3) ? towerDefenseLevelConfig3.baseTimeScale : 1.0);
		}
	}

	public void OptionButtonPressed()
	{
		DialogManager.Instance.DialogCreate("BattleOption");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_levelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ButtonPauseToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckBox2XToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OptionButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseLevelBaseConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.ButtonPauseToggled && args.Count == 1)
		{
			ButtonPauseToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckBox2XToggled && args.Count == 1)
		{
			CheckBox2XToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OptionButtonPressed && args.Count == 0)
		{
			OptionButtonPressed();
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.ButtonPauseToggled)
		{
			return true;
		}
		if (method == MethodName.CheckBox2XToggled)
		{
			return true;
		}
		if (method == MethodName.OptionButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.buttonPause)
		{
			buttonPause = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.checkBox2X)
		{
			checkBox2X = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.optionButton)
		{
			optionButton = VariantUtils.ConvertTo<SpriteBrightButton>(in value);
			return true;
		}
		if (name == PropertyName.levelConfig)
		{
			levelConfig = VariantUtils.ConvertTo<TowerDefenseLevelBaseConfig>(in value);
			return true;
		}
		if (name == PropertyName.hasProgress)
		{
			hasProgress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.buttonPause)
		{
			value = VariantUtils.CreateFrom(in buttonPause);
			return true;
		}
		if (name == PropertyName.checkBox2X)
		{
			value = VariantUtils.CreateFrom(in checkBox2X);
			return true;
		}
		if (name == PropertyName.optionButton)
		{
			value = VariantUtils.CreateFrom(in optionButton);
			return true;
		}
		if (name == PropertyName.levelConfig)
		{
			value = VariantUtils.CreateFrom(in levelConfig);
			return true;
		}
		if (name == PropertyName.hasProgress)
		{
			value = VariantUtils.CreateFrom(in hasProgress);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.buttonPause, PropertyHint.NodeType, "MainButton", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.checkBox2X, PropertyHint.NodeType, "CheckBox", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.optionButton, PropertyHint.NodeType, "SpriteBrightButton", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.buttonPause, Variant.From(in buttonPause));
		info.AddProperty(PropertyName.checkBox2X, Variant.From(in checkBox2X));
		info.AddProperty(PropertyName.optionButton, Variant.From(in optionButton));
		info.AddProperty(PropertyName.levelConfig, Variant.From(in levelConfig));
		info.AddProperty(PropertyName.hasProgress, Variant.From(in hasProgress));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.buttonPause, out var value))
		{
			buttonPause = value.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.checkBox2X, out var value2))
		{
			checkBox2X = value2.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.optionButton, out var value3))
		{
			optionButton = value3.As<SpriteBrightButton>();
		}
		if (info.TryGetProperty(PropertyName.levelConfig, out var value4))
		{
			levelConfig = value4.As<TowerDefenseLevelBaseConfig>();
		}
		if (info.TryGetProperty(PropertyName.hasProgress, out var value5))
		{
			hasProgress = value5.As<bool>();
		}
	}
}
