using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Progress/TowerDefenseBattleFeatureProgress.cs")]
public class TowerDefenseBattleFeatureProgress : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName ReadConfiguration = "ReadConfiguration";

		public static readonly StringName ResolveConfiguredText = "ResolveConfiguredText";

		public static readonly StringName ApplyConfiguredPresentation = "ApplyConfiguredPresentation";

		public static readonly StringName FormatRoundText = "FormatRoundText";

		public static readonly StringName ConfigureAutoProgressResponse = "ConfigureAutoProgressResponse";

		public static readonly StringName ProgressInit = "ProgressInit";

		public static readonly StringName ConfigureWaveProgress = "ConfigureWaveProgress";

		public static readonly StringName ProgressRefresh = "ProgressRefresh";

		public static readonly StringName SetupUI = "SetupUI";

		public static readonly StringName SetDifficultModulate = "SetDifficultModulate";

		public static readonly StringName SetDifficultText = "SetDifficultText";

		public static readonly StringName SetLevelName = "SetLevelName";

		public static readonly StringName SetSurvivalText = "SetSurvivalText";

		public static readonly StringName SetDifficultVisible = "SetDifficultVisible";

		public static readonly StringName HasSingleConfiguredDifficulty = "HasSingleConfiguredDifficulty";

		public static readonly StringName SetSurvivalVisible = "SetSurvivalVisible";

		public static readonly StringName SetLevelNameVisible = "SetLevelNameVisible";

		public static readonly StringName SetProgressMeterVisible = "SetProgressMeterVisible";

		public static readonly StringName SetProgressMeterHideItem = "SetProgressMeterHideItem";

		public static readonly StringName SetProgressMeterWaveCurrent = "SetProgressMeterWaveCurrent";

		public static readonly StringName SetProgressMeterMaxValue = "SetProgressMeterMaxValue";

		public static readonly StringName SetProgressMeterWaveNum = "SetProgressMeterWaveNum";

		public static readonly StringName SetProgressMeterPreviewWave = "SetProgressMeterPreviewWave";

		public static readonly StringName SetProgressMeterValue = "SetProgressMeterValue";

		public static readonly StringName SetProgressMeterManualProgress = "SetProgressMeterManualProgress";

		public static readonly StringName ReleaseCustomProgress = "ReleaseCustomProgress";

		public static readonly StringName SetProgressMeterText = "SetProgressMeterText";

		public static readonly StringName SetProgressMeterTextVisible = "SetProgressMeterTextVisible";

		public static readonly StringName IncrementPreviewWave = "IncrementPreviewWave";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public static readonly StringName SerializeState = "SerializeState";

		public static readonly StringName DeserializeState = "DeserializeState";

		public static readonly StringName IsProgressReady = "IsProgressReady";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName progressManager = "progressManager";

		public static readonly StringName config = "config";

		public static readonly StringName _configuredLevelName = "_configuredLevelName";

		public static readonly StringName _configuredDifficultText = "_configuredDifficultText";

		public static readonly StringName _configuredSurvivalText = "_configuredSurvivalText";

		public static readonly StringName _configuredProgressText = "_configuredProgressText";

		public static readonly StringName _hasConfiguredProgressText = "_hasConfiguredProgressText";

		public static readonly StringName _manualProgress = "_manualProgress";

		public static readonly StringName _hasHideProgressItems = "_hasHideProgressItems";

		public static readonly StringName _hideProgressItems = "_hideProgressItems";

		public static readonly StringName _initialProgressValue = "_initialProgressValue";

		public static readonly StringName _initialProgressMax = "_initialProgressMax";

		public static readonly StringName _hasDifficultVisibility = "_hasDifficultVisibility";

		public static readonly StringName _difficultVisible = "_difficultVisible";

		public static readonly StringName _singleDifficultyLevel = "_singleDifficultyLevel";

		public static readonly StringName _hasLevelNameVisibility = "_hasLevelNameVisibility";

		public static readonly StringName _levelNameVisible = "_levelNameVisible";

		public static readonly StringName _hasSurvivalVisibility = "_hasSurvivalVisibility";

		public static readonly StringName _survivalVisible = "_survivalVisible";

		public static readonly StringName _hasProgressVisibility = "_hasProgressVisibility";

		public static readonly StringName _progressVisible = "_progressVisible";

		public static readonly StringName _hasProgressTextVisibility = "_hasProgressTextVisibility";

		public static readonly StringName _progressTextVisible = "_progressTextVisible";

		public static readonly StringName _progressTextSource = "_progressTextSource";

		public static readonly StringName _translateProgressTextSource = "_translateProgressTextSource";

		public static readonly StringName _runtimeCustomProgress = "_runtimeCustomProgress";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _towerDefenseProgressManager;

	public TowerDefenseProgressManager progressManager;

	public TowerDefenseBattleFeatureProgressConfig config;

	private string _configuredLevelName = "";

	private string _configuredDifficultText = "";

	private string _configuredSurvivalText = "";

	private string _configuredProgressText = "{value}/{max}";

	private bool _hasConfiguredProgressText;

	private bool _manualProgress;

	private bool _hasHideProgressItems;

	private bool _hideProgressItems;

	private double _initialProgressValue;

	private double _initialProgressMax = 1.0;

	private bool _hasDifficultVisibility;

	private bool _difficultVisible;

	private bool _singleDifficultyLevel;

	private bool _hasLevelNameVisibility;

	private bool _levelNameVisible;

	private bool _hasSurvivalVisibility;

	private bool _survivalVisible;

	private bool _hasProgressVisibility;

	private bool _progressVisible;

	private bool _hasProgressTextVisibility;

	private bool _progressTextVisible;

	private string _progressTextSource = "{value}/{max}";

	private bool _translateProgressTextSource;

	private bool _runtimeCustomProgress;

	private static PackedScene TOWER_DEFENSE_PROGRESS_MANAGER => _towerDefenseProgressManager ?? (_towerDefenseProgressManager = GD.Load<PackedScene>("uid://dlgafsqc16cbw"));

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		progressManager = TOWER_DEFENSE_PROGRESS_MANAGER.Instantiate<TowerDefenseProgressManager>(PackedScene.GenEditState.Disabled);
		progressManager.progressFeature = this;
		control.AddUI(progressManager, 3);
		ReadConfiguration();
		_singleDifficultyLevel = HasSingleConfiguredDifficulty();
		ApplyConfiguredPresentation();
	}

	private void ReadConfiguration()
	{
		config = new TowerDefenseBattleFeatureProgressConfig();
		config.Init(data);
		_configuredLevelName = ResolveConfiguredText(config.levelName);
		_configuredDifficultText = ResolveConfiguredText(config.difficultyText);
		_configuredSurvivalText = ResolveConfiguredText(config.survivalText);
		_hasConfiguredProgressText = config.HasProgressText;
		_progressTextSource = config.progressText;
		_translateProgressTextSource = config.HasProgressText;
		_configuredProgressText = ResolveConfiguredText(config.progressText);
		_manualProgress = config.mode == TowerDefenseProgressMode.Manual;
		_hasHideProgressItems = config.HasHideProgressItems;
		_hideProgressItems = config.HideProgressItems;
		_initialProgressValue = config.progressValue;
		_initialProgressMax = config.progressMax;
		_hasDifficultVisibility = TowerDefenseBattleFeatureProgressConfig.TryGetVisibility(config.difficultyVisibility, out _difficultVisible);
		_hasLevelNameVisibility = TowerDefenseBattleFeatureProgressConfig.TryGetVisibility(config.levelNameVisibility, out _levelNameVisible);
		_hasSurvivalVisibility = TowerDefenseBattleFeatureProgressConfig.TryGetVisibility(config.survivalVisibility, out _survivalVisible);
		_hasProgressVisibility = TowerDefenseBattleFeatureProgressConfig.TryGetVisibility(config.progressVisibility, out _progressVisible);
		_hasProgressTextVisibility = TowerDefenseBattleFeatureProgressConfig.TryGetVisibility(config.progressTextVisibility, out _progressTextVisible);
		if (!_hasProgressTextVisibility && (_hasConfiguredProgressText || _manualProgress))
		{
			_hasProgressTextVisibility = true;
			_progressTextVisible = true;
		}
	}

	private string ResolveConfiguredText(string value)
	{
		if (!(value == ""))
		{
			return Tr(value);
		}
		return "";
	}

	private void ApplyConfiguredPresentation()
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			if (_configuredLevelName != "")
			{
				progressManager.levelNameLabel.Text = _configuredLevelName;
			}
			if (_configuredDifficultText != "")
			{
				progressManager.difficultLabel.Text = _configuredDifficultText;
			}
			if (_configuredSurvivalText != "")
			{
				progressManager.survivalLabel.Text = FormatRoundText(_configuredSurvivalText, 0);
			}
			if (_manualProgress && !progressManager.progressMeter.manualProgressValue)
			{
				progressManager.progressMeter.SetManualProgress(_initialProgressValue, _initialProgressMax, _configuredProgressText);
			}
			else if (_hasConfiguredProgressText)
			{
				progressManager.progressMeter.SetProgressText(_configuredProgressText);
			}
			if (_hasHideProgressItems)
			{
				progressManager.progressMeter.SetHideItem(_hideProgressItems);
			}
			if (_hasDifficultVisibility || _singleDifficultyLevel)
			{
				SetDifficultVisible(progressManager.difficultLabel.Visible);
			}
			if (_hasLevelNameVisibility)
			{
				progressManager.levelNameLabel.Visible = _levelNameVisible;
			}
			if (_hasSurvivalVisibility)
			{
				progressManager.survivalLabel.Visible = _survivalVisible;
			}
			if (_hasProgressVisibility)
			{
				progressManager.progressMeter.Visible = _progressVisible;
			}
			if (_hasProgressTextVisibility)
			{
				progressManager.progressMeter.SetProgressTextVisible(_progressTextVisible);
			}
		}
	}

	private static string FormatRoundText(string template, int round)
	{
		return (template ?? "").Replace("{round}", round.ToString()).Replace("%d", round.ToString());
	}

	public override Task GameReady()
	{
		ConfigureAutoProgressResponse();
		ApplyConfiguredPresentation();
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		if (!GodotObject.IsInstanceValid(progressManager))
		{
			return Task.CompletedTask;
		}
		ConfigureAutoProgressResponse();
		if (GetProcess() is TowerDefenseBattleProcessIZM)
		{
			progressManager.progressMeter.SetHideItem(hide: true);
			progressManager.progressMeter.Visible = true;
		}
		ApplyConfiguredPresentation();
		return Task.CompletedTask;
	}

	private void ConfigureAutoProgressResponse()
	{
		if (GodotObject.IsInstanceValid(progressManager) && GodotObject.IsInstanceValid(progressManager.progressMeter))
		{
			double autoProgressResponse;
			if (config != null && config.autoProgressResponse >= 0.0)
			{
				autoProgressResponse = config.autoProgressResponse;
			}
			else
			{
				autoProgressResponse = ((GetProcess() is TowerDefenseBattleProcessIZM) ? 2.0 : 0.1);
			}
			progressManager.progressMeter.SetAutoProgressResponse(autoProgressResponse);
		}
	}

	public void ProgressInit(int waveNum, int flagWaveInterval)
	{
		ConfigureWaveProgress(waveNum, flagWaveInterval, 0);
	}

	public void ConfigureWaveProgress(int waveNum, int flagWaveInterval, int currentWave)
	{
		if (GodotObject.IsInstanceValid(progressManager) && GodotObject.IsInstanceValid(progressManager.progressMeter) && !progressManager.progressMeter.manualProgressValue)
		{
			if (_manualProgress)
			{
				progressManager.progressMeter.SetManualProgress(_initialProgressValue, _initialProgressMax, _configuredProgressText);
				return;
			}
			progressManager.progressMeter.SetManualProgressEnabled(enabled: false);
			progressManager.progressMeter.Init(waveNum, flagWaveInterval);
			progressManager.progressMeter.SetWaveCurrent(currentWave);
		}
	}

	public void ProgressRefresh(bool isSurvival = false, int survivalRoundNum = 0)
	{
		if (!GodotObject.IsInstanceValid(progressManager))
		{
			return;
		}
		if (progressManager.progressMeter.manualProgressValue || _manualProgress)
		{
			if (!progressManager.progressMeter.manualProgressValue)
			{
				progressManager.progressMeter.SetManualProgress(_initialProgressValue, _initialProgressMax, _configuredProgressText);
			}
			SetProgressMeterVisible(visible: true);
			return;
		}
		progressManager.progressMeter.SetManualProgressEnabled(enabled: false);
		progressManager.progressMeter.Visible = false;
		progressManager.progressMeter.SetWaveCurrent(0);
		progressManager.progressBar.Value = 0.0;
		if (isSurvival)
		{
			SetSurvivalText(survivalRoundNum);
		}
	}

	public void SetupUI(string difficult, string levelName, bool isSurvival = false, int survivalRoundNum = 0)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.SetupUI(difficult, levelName, isSurvival, survivalRoundNum);
			if (_configuredLevelName != "")
			{
				progressManager.levelNameLabel.Text = _configuredLevelName;
			}
			if (_configuredDifficultText != "")
			{
				progressManager.difficultLabel.Text = _configuredDifficultText;
			}
			if (_configuredSurvivalText != "")
			{
				progressManager.survivalLabel.Text = FormatRoundText(_configuredSurvivalText, survivalRoundNum);
			}
		}
	}

	public void SetDifficultModulate(string difficult)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			switch (difficult)
			{
			case "Normal":
				progressManager.difficultLabel.Modulate = new Color(progressManager.difficultLabel.Modulate.R, 1f, progressManager.difficultLabel.Modulate.B, progressManager.difficultLabel.Modulate.A);
				break;
			case "Difficult":
				progressManager.difficultLabel.Modulate = new Color(progressManager.difficultLabel.Modulate.R, 0f, progressManager.difficultLabel.Modulate.B, progressManager.difficultLabel.Modulate.A);
				break;
			case "Ultimate":
				progressManager.difficultLabel.Modulate = new Color(progressManager.difficultLabel.Modulate.R, 0f, progressManager.difficultLabel.Modulate.B, progressManager.difficultLabel.Modulate.A);
				break;
			}
		}
	}

	public void SetDifficultText(string difficultString)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.difficultLabel.Text = ((_configuredDifficultText != "") ? _configuredDifficultText.Replace("{difficulty}", difficultString).Replace("%s", difficultString) : Tr("INGAME_DIFFICULT").Replace("%s", difficultString));
		}
	}

	public void SetLevelName(string levelName)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.levelNameLabel.Text = ((_configuredLevelName != "") ? _configuredLevelName : levelName);
		}
	}

	public void SetSurvivalText(int survivalRoundNum)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.survivalLabel.Text = ((_configuredSurvivalText != "") ? FormatRoundText(_configuredSurvivalText, survivalRoundNum) : Tr("PLAYERS_SURVIVAL_LEVEL_DESCRIBE").Replace("%d", survivalRoundNum.ToString()));
		}
	}

	public void SetDifficultVisible(bool visible)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.difficultLabel.Visible = (_hasDifficultVisibility ? _difficultVisible : (visible && !_singleDifficultyLevel));
		}
	}

	private static bool HasSingleConfiguredDifficulty()
	{
		TowerDefenseLevelBaseConfig towerDefenseLevelBaseConfig = TowerDefenseManager.Instance?.currentLevelConfig;
		if (!GodotObject.IsInstanceValid(towerDefenseLevelBaseConfig) || string.IsNullOrEmpty(towerDefenseLevelBaseConfig.name) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
		{
			return false;
		}
		foreach (Dictionary value3 in ResourceManager.Instance.LEVELS.Values)
		{
			if (!value3.TryGetValue("Chapter", out var value))
			{
				continue;
			}
			foreach (Variant item in value.AsGodotArray())
			{
				if (item.AsGodotDictionary().TryGetValue("Level", out var value2))
				{
					bool? flag = FindSingleDifficultyLevel(value2.AsGodotArray(), towerDefenseLevelBaseConfig.name);
					if (flag.HasValue)
					{
						return flag.Value;
					}
				}
			}
		}
		if (Global.Instance?.currentLevelChoose == "TryLevel")
		{
			foreach (Variant value4 in GD.Load<Json>("res://Asset/Config/Level/TryLevelResource.json").Data.AsGodotDictionary().Values)
			{
				bool? flag2 = FindSingleDifficultyLevel(value4.AsGodotArray(), towerDefenseLevelBaseConfig.name);
				if (flag2.HasValue)
				{
					return flag2.Value;
				}
			}
		}
		return false;
	}

	private static bool? FindSingleDifficultyLevel(Array levels, string levelName)
	{
		foreach (Variant level in levels)
		{
			Dictionary dictionary = level.AsGodotDictionary();
			if (!(dictionary.GetValueOrDefault("SaveKey", "").AsString() != levelName) && dictionary.TryGetValue("Level", out var value))
			{
				Dictionary dictionary2 = value.AsGodotDictionary();
				int num = 0;
				if (!string.IsNullOrEmpty(dictionary2.GetValueOrDefault("Normal", "").AsString()))
				{
					num++;
				}
				if (!string.IsNullOrEmpty(dictionary2.GetValueOrDefault("Difficult", "").AsString()))
				{
					num++;
				}
				if (!string.IsNullOrEmpty(dictionary2.GetValueOrDefault("Ultimate", "").AsString()))
				{
					num++;
				}
				return num == 1;
			}
		}
		return null;
	}

	public void SetSurvivalVisible(bool visible)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.survivalLabel.Visible = (_hasSurvivalVisibility ? _survivalVisible : visible);
		}
	}

	public void SetLevelNameVisible(bool visible)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.levelNameLabel.Visible = (_hasLevelNameVisibility ? _levelNameVisible : visible);
		}
	}

	public void SetProgressMeterVisible(bool visible)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.progressMeter.Visible = (_hasProgressVisibility ? _progressVisible : visible);
		}
	}

	public void SetProgressMeterHideItem(bool hide)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.progressMeter.SetHideItem(_hasHideProgressItems ? _hideProgressItems : hide);
		}
	}

	public void SetProgressMeterWaveCurrent(int waveCurrent)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.progressMeter.SetWaveCurrent(waveCurrent);
		}
	}

	public void SetProgressMeterMaxValue(double maxValue)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.progressMeter.SetProgressMaxValue(maxValue);
		}
	}

	public void SetProgressMeterWaveNum(int waveNum)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.progressMeter.waveNum = waveNum;
		}
	}

	public void SetProgressMeterPreviewWave(int previewWave)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.progressMeter.previewWave = previewWave;
		}
	}

	public void SetProgressMeterValue(double value)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.progressMeter.SetProgressValue(value);
		}
	}

	public void SetProgressMeterManualProgress(double value, double maxValue, string text)
	{
		SetCustomProgress(value, maxValue, text, null, null);
	}

	public void SetCustomProgress(double value, double maxValue, string text, bool? visible = true, bool? hideItems = true)
	{
		if (GodotObject.IsInstanceValid(progressManager) && GodotObject.IsInstanceValid(progressManager.progressMeter))
		{
			_runtimeCustomProgress = true;
			_progressTextSource = text ?? "";
			_translateProgressTextSource = false;
			progressManager.progressMeter.SetManualProgress(value, maxValue, text);
			if (visible.HasValue)
			{
				progressManager.progressMeter.Visible = visible.Value;
			}
			if (hideItems.HasValue)
			{
				progressManager.progressMeter.SetHideItem(hideItems.Value);
			}
		}
	}

	public void ReleaseCustomProgress()
	{
		if (!_runtimeCustomProgress || !GodotObject.IsInstanceValid(progressManager) || !GodotObject.IsInstanceValid(progressManager.progressMeter))
		{
			return;
		}
		_runtimeCustomProgress = false;
		if (_manualProgress)
		{
			_progressTextSource = config?.progressText ?? "{value}/{max}";
			_translateProgressTextSource = config?.HasProgressText ?? false;
			progressManager.progressMeter.SetManualProgress(_initialProgressValue, _initialProgressMax, _configuredProgressText);
			return;
		}
		progressManager.progressMeter.SetManualProgressEnabled(enabled: false);
		if (_hasHideProgressItems)
		{
			progressManager.progressMeter.SetHideItem(_hideProgressItems);
		}
		else
		{
			progressManager.progressMeter.SetHideItem(hide: false);
		}
		ConfigureAutoProgressResponse();
	}

	public void SetProgressMeterText(string text)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			_progressTextSource = text ?? "";
			_translateProgressTextSource = false;
			progressManager.progressMeter.SetProgressText(text);
			progressManager.progressMeter.SetProgressTextVisible(visible: true);
		}
	}

	public void SetProgressMeterTextVisible(bool visible)
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.progressMeter.SetProgressTextVisible(visible);
		}
	}

	public void IncrementPreviewWave()
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.progressMeter.previewWave++;
		}
	}

	public override Dictionary SaveFeature()
	{
		return SerializeState(includeLocalizedText: true);
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		DeserializeState(_data);
	}

	public override Dictionary SyncSerialize()
	{
		return SerializeState(includeLocalizedText: false);
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		DeserializeState(_data);
	}

	private Dictionary SerializeState(bool includeLocalizedText)
	{
		Dictionary dictionary = new Dictionary();
		if (!IsProgressReady())
		{
			return dictionary;
		}
		if (includeLocalizedText)
		{
			dictionary["level_name"] = progressManager.levelNameLabel.Text;
			dictionary["difficult_text"] = progressManager.difficultLabel.Text;
			dictionary["survival_text"] = progressManager.survivalLabel.Text;
		}
		bool manualProgressValue = progressManager.progressMeter.manualProgressValue;
		if (includeLocalizedText | manualProgressValue)
		{
			dictionary["progress_value"] = progressManager.progressBar.Value;
		}
		dictionary["progress_max"] = progressManager.progressBar.MaxValue;
		dictionary["manual_progress"] = manualProgressValue;
		dictionary["runtime_custom_progress"] = _runtimeCustomProgress;
		dictionary["progress_text_source"] = _progressTextSource;
		dictionary["translate_progress_text"] = _translateProgressTextSource;
		if (includeLocalizedText)
		{
			dictionary["progress_text"] = progressManager.progressMeter.ManualProgressTextTemplate;
		}
		dictionary["show_progress_text"] = progressManager.progressMeter.showProgressText;
		dictionary["wave_num"] = progressManager.progressMeter.waveNum;
		dictionary["wave_interval"] = progressManager.progressMeter.waveInterval;
		dictionary["preview_wave"] = progressManager.progressMeter.previewWave;
		dictionary["hide_progress_items"] = progressManager.progressMeter.hideItem;
		dictionary["show_difficult"] = progressManager.difficultLabel.Visible;
		dictionary["show_level_name"] = progressManager.levelNameLabel.Visible;
		dictionary["show_survival"] = progressManager.survivalLabel.Visible;
		dictionary["show_progress"] = progressManager.progressMeter.Visible;
		return dictionary;
	}

	private void DeserializeState(Dictionary state)
	{
		if (!IsProgressReady())
		{
			return;
		}
		if (state.ContainsKey("level_name"))
		{
			progressManager.levelNameLabel.Text = state["level_name"].AsString();
		}
		if (state.ContainsKey("difficult_text"))
		{
			progressManager.difficultLabel.Text = state["difficult_text"].AsString();
		}
		if (state.ContainsKey("survival_text"))
		{
			progressManager.survivalLabel.Text = state["survival_text"].AsString();
		}
		progressManager.progressMeter.waveNum = state.GetValueOrDefault("wave_num", progressManager.progressMeter.waveNum).AsInt32();
		progressManager.progressMeter.waveInterval = state.GetValueOrDefault("wave_interval", progressManager.progressMeter.waveInterval).AsInt32();
		progressManager.progressMeter.previewWave = state.GetValueOrDefault("preview_wave", progressManager.progressMeter.previewWave).AsInt32();
		progressManager.progressMeter.SetHideItem(state.GetValueOrDefault("hide_progress_items", progressManager.progressMeter.hideItem).AsBool());
		double num = state.GetValueOrDefault("progress_value", progressManager.progressBar.Value).AsDouble();
		double num2 = state.GetValueOrDefault("progress_max", progressManager.progressBar.MaxValue).AsDouble();
		_progressTextSource = state.GetValueOrDefault("progress_text_source", state.GetValueOrDefault("progress_text", _progressTextSource)).AsString();
		_translateProgressTextSource = state.GetValueOrDefault("translate_progress_text", false).AsBool();
		_runtimeCustomProgress = state.GetValueOrDefault("runtime_custom_progress", false).AsBool();
		string text = (_translateProgressTextSource ? Tr(_progressTextSource) : _progressTextSource);
		if (state.GetValueOrDefault("manual_progress", progressManager.progressMeter.manualProgressValue).AsBool())
		{
			progressManager.progressMeter.SetManualProgress(num, num2, text);
		}
		else
		{
			progressManager.progressMeter.SetManualProgressEnabled(enabled: false);
			if (state.ContainsKey("progress_text_source") || state.ContainsKey("progress_text"))
			{
				progressManager.progressMeter.SetProgressText(text);
			}
			progressManager.progressMeter.SetProgressMaxValue(num2);
			if (state.ContainsKey("progress_value"))
			{
				progressManager.progressMeter.SetProgressValue(num);
			}
		}
		progressManager.progressMeter.SetProgressTextVisible(state.GetValueOrDefault("show_progress_text", progressManager.progressMeter.showProgressText).AsBool());
		SetDifficultVisible(state.GetValueOrDefault("show_difficult", progressManager.difficultLabel.Visible).AsBool());
		SetLevelNameVisible(state.GetValueOrDefault("show_level_name", progressManager.levelNameLabel.Visible).AsBool());
		SetSurvivalVisible(state.GetValueOrDefault("show_survival", progressManager.survivalLabel.Visible).AsBool());
		SetProgressMeterVisible(state.GetValueOrDefault("show_progress", progressManager.progressMeter.Visible).AsBool());
	}

	private bool IsProgressReady()
	{
		if (GodotObject.IsInstanceValid(progressManager) && GodotObject.IsInstanceValid(progressManager.progressMeter))
		{
			return GodotObject.IsInstanceValid(progressManager.progressBar);
		}
		return false;
	}

	public override void Destroy()
	{
		if (GodotObject.IsInstanceValid(progressManager))
		{
			progressManager.progressFeature = null;
			progressManager.QueueFree();
		}
		progressManager = null;
		base.Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(38)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadConfiguration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveConfiguredText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyConfiguredPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatRoundText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "template", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "round", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureAutoProgressResponse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProgressInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "flagWaveInterval", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureWaveProgress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "flagWaveInterval", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "currentWave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProgressRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "isSurvival", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "survivalRoundNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetupUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "difficult", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "levelName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isSurvival", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "survivalRoundNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetDifficultModulate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "difficult", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetDifficultText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "difficultString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetLevelName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSurvivalText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "survivalRoundNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetDifficultVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasSingleConfiguredDifficulty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.SetSurvivalVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetLevelNameVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressMeterVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressMeterHideItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "hide", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressMeterWaveCurrent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveCurrent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressMeterMaxValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "maxValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressMeterWaveNum, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressMeterPreviewWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "previewWave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressMeterValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressMeterManualProgress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maxValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseCustomProgress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetProgressMeterText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressMeterTextVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IncrementPreviewWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SerializeState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "includeLocalizedText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DeserializeState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsProgressReady, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ReadConfiguration && args.Count == 0)
		{
			ReadConfiguration();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveConfiguredText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveConfiguredText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyConfiguredPresentation && args.Count == 0)
		{
			ApplyConfiguredPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.FormatRoundText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatRoundText(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ConfigureAutoProgressResponse && args.Count == 0)
		{
			ConfigureAutoProgressResponse();
			ret = default;
			return true;
		}
		if (method == MethodName.ProgressInit && args.Count == 2)
		{
			ProgressInit(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureWaveProgress && args.Count == 3)
		{
			ConfigureWaveProgress(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProgressRefresh && args.Count == 2)
		{
			ProgressRefresh(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetupUI && args.Count == 4)
		{
			SetupUI(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDifficultModulate && args.Count == 1)
		{
			SetDifficultModulate(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDifficultText && args.Count == 1)
		{
			SetDifficultText(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLevelName && args.Count == 1)
		{
			SetLevelName(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSurvivalText && args.Count == 1)
		{
			SetSurvivalText(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDifficultVisible && args.Count == 1)
		{
			SetDifficultVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasSingleConfiguredDifficulty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSingleConfiguredDifficulty());
			return true;
		}
		if (method == MethodName.SetSurvivalVisible && args.Count == 1)
		{
			SetSurvivalVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLevelNameVisible && args.Count == 1)
		{
			SetLevelNameVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMeterVisible && args.Count == 1)
		{
			SetProgressMeterVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMeterHideItem && args.Count == 1)
		{
			SetProgressMeterHideItem(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMeterWaveCurrent && args.Count == 1)
		{
			SetProgressMeterWaveCurrent(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMeterMaxValue && args.Count == 1)
		{
			SetProgressMeterMaxValue(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMeterWaveNum && args.Count == 1)
		{
			SetProgressMeterWaveNum(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMeterPreviewWave && args.Count == 1)
		{
			SetProgressMeterPreviewWave(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMeterValue && args.Count == 1)
		{
			SetProgressMeterValue(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMeterManualProgress && args.Count == 3)
		{
			SetProgressMeterManualProgress(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseCustomProgress && args.Count == 0)
		{
			ReleaseCustomProgress();
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMeterText && args.Count == 1)
		{
			SetProgressMeterText(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMeterTextVisible && args.Count == 1)
		{
			SetProgressMeterTextVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IncrementPreviewWave && args.Count == 0)
		{
			IncrementPreviewWave();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveFeature());
			return true;
		}
		if (method == MethodName.LoadFeature && args.Count == 2)
		{
			LoadFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncSerialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SyncSerialize());
			return true;
		}
		if (method == MethodName.SyncDeserialize && args.Count == 1)
		{
			SyncDeserialize(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SerializeState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SerializeState(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.DeserializeState && args.Count == 1)
		{
			DeserializeState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsProgressReady && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProgressReady());
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
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FormatRoundText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatRoundText(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.HasSingleConfiguredDifficulty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSingleConfiguredDifficulty());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.ReadConfiguration)
		{
			return true;
		}
		if (method == MethodName.ResolveConfiguredText)
		{
			return true;
		}
		if (method == MethodName.ApplyConfiguredPresentation)
		{
			return true;
		}
		if (method == MethodName.FormatRoundText)
		{
			return true;
		}
		if (method == MethodName.ConfigureAutoProgressResponse)
		{
			return true;
		}
		if (method == MethodName.ProgressInit)
		{
			return true;
		}
		if (method == MethodName.ConfigureWaveProgress)
		{
			return true;
		}
		if (method == MethodName.ProgressRefresh)
		{
			return true;
		}
		if (method == MethodName.SetupUI)
		{
			return true;
		}
		if (method == MethodName.SetDifficultModulate)
		{
			return true;
		}
		if (method == MethodName.SetDifficultText)
		{
			return true;
		}
		if (method == MethodName.SetLevelName)
		{
			return true;
		}
		if (method == MethodName.SetSurvivalText)
		{
			return true;
		}
		if (method == MethodName.SetDifficultVisible)
		{
			return true;
		}
		if (method == MethodName.HasSingleConfiguredDifficulty)
		{
			return true;
		}
		if (method == MethodName.SetSurvivalVisible)
		{
			return true;
		}
		if (method == MethodName.SetLevelNameVisible)
		{
			return true;
		}
		if (method == MethodName.SetProgressMeterVisible)
		{
			return true;
		}
		if (method == MethodName.SetProgressMeterHideItem)
		{
			return true;
		}
		if (method == MethodName.SetProgressMeterWaveCurrent)
		{
			return true;
		}
		if (method == MethodName.SetProgressMeterMaxValue)
		{
			return true;
		}
		if (method == MethodName.SetProgressMeterWaveNum)
		{
			return true;
		}
		if (method == MethodName.SetProgressMeterPreviewWave)
		{
			return true;
		}
		if (method == MethodName.SetProgressMeterValue)
		{
			return true;
		}
		if (method == MethodName.SetProgressMeterManualProgress)
		{
			return true;
		}
		if (method == MethodName.ReleaseCustomProgress)
		{
			return true;
		}
		if (method == MethodName.SetProgressMeterText)
		{
			return true;
		}
		if (method == MethodName.SetProgressMeterTextVisible)
		{
			return true;
		}
		if (method == MethodName.IncrementPreviewWave)
		{
			return true;
		}
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		if (method == MethodName.SerializeState)
		{
			return true;
		}
		if (method == MethodName.DeserializeState)
		{
			return true;
		}
		if (method == MethodName.IsProgressReady)
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
		if (name == PropertyName.progressManager)
		{
			progressManager = VariantUtils.ConvertTo<TowerDefenseProgressManager>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseBattleFeatureProgressConfig>(in value);
			return true;
		}
		if (name == PropertyName._configuredLevelName)
		{
			_configuredLevelName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._configuredDifficultText)
		{
			_configuredDifficultText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._configuredSurvivalText)
		{
			_configuredSurvivalText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._configuredProgressText)
		{
			_configuredProgressText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._hasConfiguredProgressText)
		{
			_hasConfiguredProgressText = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._manualProgress)
		{
			_manualProgress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasHideProgressItems)
		{
			_hasHideProgressItems = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hideProgressItems)
		{
			_hideProgressItems = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._initialProgressValue)
		{
			_initialProgressValue = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._initialProgressMax)
		{
			_initialProgressMax = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._hasDifficultVisibility)
		{
			_hasDifficultVisibility = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._difficultVisible)
		{
			_difficultVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._singleDifficultyLevel)
		{
			_singleDifficultyLevel = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasLevelNameVisibility)
		{
			_hasLevelNameVisibility = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._levelNameVisible)
		{
			_levelNameVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasSurvivalVisibility)
		{
			_hasSurvivalVisibility = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._survivalVisible)
		{
			_survivalVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasProgressVisibility)
		{
			_hasProgressVisibility = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._progressVisible)
		{
			_progressVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasProgressTextVisibility)
		{
			_hasProgressTextVisibility = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._progressTextVisible)
		{
			_progressTextVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._progressTextSource)
		{
			_progressTextSource = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._translateProgressTextSource)
		{
			_translateProgressTextSource = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeCustomProgress)
		{
			_runtimeCustomProgress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.progressManager)
		{
			value = VariantUtils.CreateFrom(in progressManager);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName._configuredLevelName)
		{
			value = VariantUtils.CreateFrom(in _configuredLevelName);
			return true;
		}
		if (name == PropertyName._configuredDifficultText)
		{
			value = VariantUtils.CreateFrom(in _configuredDifficultText);
			return true;
		}
		if (name == PropertyName._configuredSurvivalText)
		{
			value = VariantUtils.CreateFrom(in _configuredSurvivalText);
			return true;
		}
		if (name == PropertyName._configuredProgressText)
		{
			value = VariantUtils.CreateFrom(in _configuredProgressText);
			return true;
		}
		if (name == PropertyName._hasConfiguredProgressText)
		{
			value = VariantUtils.CreateFrom(in _hasConfiguredProgressText);
			return true;
		}
		if (name == PropertyName._manualProgress)
		{
			value = VariantUtils.CreateFrom(in _manualProgress);
			return true;
		}
		if (name == PropertyName._hasHideProgressItems)
		{
			value = VariantUtils.CreateFrom(in _hasHideProgressItems);
			return true;
		}
		if (name == PropertyName._hideProgressItems)
		{
			value = VariantUtils.CreateFrom(in _hideProgressItems);
			return true;
		}
		if (name == PropertyName._initialProgressValue)
		{
			value = VariantUtils.CreateFrom(in _initialProgressValue);
			return true;
		}
		if (name == PropertyName._initialProgressMax)
		{
			value = VariantUtils.CreateFrom(in _initialProgressMax);
			return true;
		}
		if (name == PropertyName._hasDifficultVisibility)
		{
			value = VariantUtils.CreateFrom(in _hasDifficultVisibility);
			return true;
		}
		if (name == PropertyName._difficultVisible)
		{
			value = VariantUtils.CreateFrom(in _difficultVisible);
			return true;
		}
		if (name == PropertyName._singleDifficultyLevel)
		{
			value = VariantUtils.CreateFrom(in _singleDifficultyLevel);
			return true;
		}
		if (name == PropertyName._hasLevelNameVisibility)
		{
			value = VariantUtils.CreateFrom(in _hasLevelNameVisibility);
			return true;
		}
		if (name == PropertyName._levelNameVisible)
		{
			value = VariantUtils.CreateFrom(in _levelNameVisible);
			return true;
		}
		if (name == PropertyName._hasSurvivalVisibility)
		{
			value = VariantUtils.CreateFrom(in _hasSurvivalVisibility);
			return true;
		}
		if (name == PropertyName._survivalVisible)
		{
			value = VariantUtils.CreateFrom(in _survivalVisible);
			return true;
		}
		if (name == PropertyName._hasProgressVisibility)
		{
			value = VariantUtils.CreateFrom(in _hasProgressVisibility);
			return true;
		}
		if (name == PropertyName._progressVisible)
		{
			value = VariantUtils.CreateFrom(in _progressVisible);
			return true;
		}
		if (name == PropertyName._hasProgressTextVisibility)
		{
			value = VariantUtils.CreateFrom(in _hasProgressTextVisibility);
			return true;
		}
		if (name == PropertyName._progressTextVisible)
		{
			value = VariantUtils.CreateFrom(in _progressTextVisible);
			return true;
		}
		if (name == PropertyName._progressTextSource)
		{
			value = VariantUtils.CreateFrom(in _progressTextSource);
			return true;
		}
		if (name == PropertyName._translateProgressTextSource)
		{
			value = VariantUtils.CreateFrom(in _translateProgressTextSource);
			return true;
		}
		if (name == PropertyName._runtimeCustomProgress)
		{
			value = VariantUtils.CreateFrom(in _runtimeCustomProgress);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.progressManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._configuredLevelName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._configuredDifficultText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._configuredSurvivalText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._configuredProgressText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasConfiguredProgressText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._manualProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasHideProgressItems, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hideProgressItems, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._initialProgressValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._initialProgressMax, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasDifficultVisibility, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._difficultVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._singleDifficultyLevel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasLevelNameVisibility, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._levelNameVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasSurvivalVisibility, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._survivalVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasProgressVisibility, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._progressVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasProgressTextVisibility, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._progressTextVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._progressTextSource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._translateProgressTextSource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeCustomProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.progressManager, Variant.From(in progressManager));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName._configuredLevelName, Variant.From(in _configuredLevelName));
		info.AddProperty(PropertyName._configuredDifficultText, Variant.From(in _configuredDifficultText));
		info.AddProperty(PropertyName._configuredSurvivalText, Variant.From(in _configuredSurvivalText));
		info.AddProperty(PropertyName._configuredProgressText, Variant.From(in _configuredProgressText));
		info.AddProperty(PropertyName._hasConfiguredProgressText, Variant.From(in _hasConfiguredProgressText));
		info.AddProperty(PropertyName._manualProgress, Variant.From(in _manualProgress));
		info.AddProperty(PropertyName._hasHideProgressItems, Variant.From(in _hasHideProgressItems));
		info.AddProperty(PropertyName._hideProgressItems, Variant.From(in _hideProgressItems));
		info.AddProperty(PropertyName._initialProgressValue, Variant.From(in _initialProgressValue));
		info.AddProperty(PropertyName._initialProgressMax, Variant.From(in _initialProgressMax));
		info.AddProperty(PropertyName._hasDifficultVisibility, Variant.From(in _hasDifficultVisibility));
		info.AddProperty(PropertyName._difficultVisible, Variant.From(in _difficultVisible));
		info.AddProperty(PropertyName._singleDifficultyLevel, Variant.From(in _singleDifficultyLevel));
		info.AddProperty(PropertyName._hasLevelNameVisibility, Variant.From(in _hasLevelNameVisibility));
		info.AddProperty(PropertyName._levelNameVisible, Variant.From(in _levelNameVisible));
		info.AddProperty(PropertyName._hasSurvivalVisibility, Variant.From(in _hasSurvivalVisibility));
		info.AddProperty(PropertyName._survivalVisible, Variant.From(in _survivalVisible));
		info.AddProperty(PropertyName._hasProgressVisibility, Variant.From(in _hasProgressVisibility));
		info.AddProperty(PropertyName._progressVisible, Variant.From(in _progressVisible));
		info.AddProperty(PropertyName._hasProgressTextVisibility, Variant.From(in _hasProgressTextVisibility));
		info.AddProperty(PropertyName._progressTextVisible, Variant.From(in _progressTextVisible));
		info.AddProperty(PropertyName._progressTextSource, Variant.From(in _progressTextSource));
		info.AddProperty(PropertyName._translateProgressTextSource, Variant.From(in _translateProgressTextSource));
		info.AddProperty(PropertyName._runtimeCustomProgress, Variant.From(in _runtimeCustomProgress));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.progressManager, out var value))
		{
			progressManager = value.As<TowerDefenseProgressManager>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseBattleFeatureProgressConfig>();
		}
		if (info.TryGetProperty(PropertyName._configuredLevelName, out var value3))
		{
			_configuredLevelName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._configuredDifficultText, out var value4))
		{
			_configuredDifficultText = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName._configuredSurvivalText, out var value5))
		{
			_configuredSurvivalText = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName._configuredProgressText, out var value6))
		{
			_configuredProgressText = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName._hasConfiguredProgressText, out var value7))
		{
			_hasConfiguredProgressText = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._manualProgress, out var value8))
		{
			_manualProgress = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasHideProgressItems, out var value9))
		{
			_hasHideProgressItems = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hideProgressItems, out var value10))
		{
			_hideProgressItems = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._initialProgressValue, out var value11))
		{
			_initialProgressValue = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName._initialProgressMax, out var value12))
		{
			_initialProgressMax = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hasDifficultVisibility, out var value13))
		{
			_hasDifficultVisibility = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._difficultVisible, out var value14))
		{
			_difficultVisible = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._singleDifficultyLevel, out var value15))
		{
			_singleDifficultyLevel = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasLevelNameVisibility, out var value16))
		{
			_hasLevelNameVisibility = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._levelNameVisible, out var value17))
		{
			_levelNameVisible = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasSurvivalVisibility, out var value18))
		{
			_hasSurvivalVisibility = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._survivalVisible, out var value19))
		{
			_survivalVisible = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasProgressVisibility, out var value20))
		{
			_hasProgressVisibility = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._progressVisible, out var value21))
		{
			_progressVisible = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasProgressTextVisibility, out var value22))
		{
			_hasProgressTextVisibility = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._progressTextVisible, out var value23))
		{
			_progressTextVisible = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._progressTextSource, out var value24))
		{
			_progressTextSource = value24.As<string>();
		}
		if (info.TryGetProperty(PropertyName._translateProgressTextSource, out var value25))
		{
			_translateProgressTextSource = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeCustomProgress, out var value26))
		{
			_runtimeCustomProgress = value26.As<bool>();
		}
	}
}
