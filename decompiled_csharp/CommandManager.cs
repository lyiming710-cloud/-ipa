using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Core/CommandManager/CommandManager.cs")]
public class CommandManager : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName Init = "Init";

		public static readonly StringName OpenButtonToggled = "OpenButtonToggled";

		public static readonly StringName SetCommandLayerVisible = "SetCommandLayerVisible";

		public static readonly StringName _on_speed_reset_button_pressed = "_on_speed_reset_button_pressed";

		public static readonly StringName SetSunValue = "SetSunValue";

		public static readonly StringName SetCoinValue = "SetCoinValue";

		public static readonly StringName SetCrystalValue = "SetCrystalValue";

		public static readonly StringName TestLevelButtonPressed = "TestLevelButtonPressed";

		public static readonly StringName LoadLevelButtonPressed = "LoadLevelButtonPressed";

		public static readonly StringName LoadSelectedLevelFile = "LoadSelectedLevelFile";

		public static readonly StringName StoreSelectedLevelBytes = "StoreSelectedLevelBytes";

		public static readonly StringName EnsureImportLevelDirectory = "EnsureImportLevelDirectory";

		public static readonly StringName EnterLoadedLevel = "EnterLoadedLevel";

		public static readonly StringName BroadcastInvalidLevelFile = "BroadcastInvalidLevelFile";

		public static readonly StringName SkipToWave = "SkipToWave";

		public static readonly StringName SkipToFinalWave = "SkipToFinalWave";

		public static readonly StringName SkipWaveWait = "SkipWaveWait";

		public static readonly StringName KillAllZombies = "KillAllZombies";

		public static readonly StringName InstantWin = "InstantWin";

		public static readonly StringName RestoreAllMowers = "RestoreAllMowers";

		public static readonly StringName RemoveAllMowers = "RemoveAllMowers";

		public static readonly StringName ResetAllBrains = "ResetAllBrains";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName debugCoinMax = "debugCoinMax";

		public static readonly StringName _debugStatsRefreshRemaining = "_debugStatsRefreshRemaining";

		public static readonly StringName _openButton = "_openButton";

		public static readonly StringName _guiLayer = "_guiLayer";

		public static readonly StringName _openAllLevelCheckBox = "_openAllLevelCheckBox";

		public static readonly StringName _coinMaxCheckBox = "_coinMaxCheckBox";

		public static readonly StringName _sunMaxCheckBox = "_sunMaxCheckBox";

		public static readonly StringName _packetSelectCheckBox = "_packetSelectCheckBox";

		public static readonly StringName _packetOpenAllCheckBox = "_packetOpenAllCheckBox";

		public static readonly StringName _packetColdDownCheckBox = "_packetColdDownCheckBox";

		public static readonly StringName _openAllCustomCheckBox = "_openAllCustomCheckBox";

		public static readonly StringName _openGloveCheckBox = "_openGloveCheckBox";

		public static readonly StringName _unlimitedFireCheckBox = "_unlimitedFireCheckBox";

		public static readonly StringName _plantInvincibleCheckBox = "_plantInvincibleCheckBox";

		public static readonly StringName _noLoseCheckBox = "_noLoseCheckBox";

		public static readonly StringName _sunSpinBox = "_sunSpinBox";

		public static readonly StringName _coinSpinBox = "_coinSpinBox";

		public static readonly StringName _crystalSpinBox = "_crystalSpinBox";

		public static readonly StringName _wavePausedCheckBox = "_wavePausedCheckBox";

		public static readonly StringName _noZombieSpawnCheckBox = "_noZombieSpawnCheckBox";

		public static readonly StringName _waveSpinBox = "_waveSpinBox";

		public static readonly StringName _brainInvincibleCheckBox = "_brainInvincibleCheckBox";

		public static readonly StringName _gameSpeedLabel = "_gameSpeedLabel";

		public static readonly StringName _gameSpeedSlider = "_gameSpeedSlider";

		public static readonly StringName _fpsLabel = "_fpsLabel";

		public static readonly StringName _waveInfoLabel = "_waveInfoLabel";

		public static readonly StringName _characterCountLabel = "_characterCountLabel";

		public static readonly StringName _bulletCountLabel = "_bulletCountLabel";

		public static readonly StringName debug = "debug";

		public static readonly StringName debugUnlimitedFire = "debugUnlimitedFire";

		public static readonly StringName _debugCoinMax = "_debugCoinMax";

		public static readonly StringName debugOpenAllLevel = "debugOpenAllLevel";

		public static readonly StringName debugSunMax = "debugSunMax";

		public static readonly StringName debugPacketSelect = "debugPacketSelect";

		public static readonly StringName debugPacketOpenAll = "debugPacketOpenAll";

		public static readonly StringName debugPacketColdDown = "debugPacketColdDown";

		public static readonly StringName debugOpenAllCustom = "debugOpenAllCustom";

		public static readonly StringName debugOpenGlove = "debugOpenGlove";

		public static readonly StringName debugPlantInvincible = "debugPlantInvincible";

		public static readonly StringName debugNoLose = "debugNoLose";

		public static readonly StringName debugWavePaused = "debugWavePaused";

		public static readonly StringName debugNoZombieSpawn = "debugNoZombieSpawn";

		public static readonly StringName debugBrainInvincible = "debugBrainInvincible";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const string ImportLevelJsonTempPath = "user://Csharp/import_level.json";

	private const string ImportLevelTresTempPath = "user://Csharp/import_level.tres";

	private const double DebugStatsRefreshInterval = 0.2;

	private double _debugStatsRefreshRemaining;

	private MainButton _openButton;

	private CanvasLayer _guiLayer;

	private CheckBox _openAllLevelCheckBox;

	private CheckBox _coinMaxCheckBox;

	private CheckBox _sunMaxCheckBox;

	private CheckBox _packetSelectCheckBox;

	private CheckBox _packetOpenAllCheckBox;

	private CheckBox _packetColdDownCheckBox;

	private CheckBox _openAllCustomCheckBox;

	private CheckBox _openGloveCheckBox;

	private CheckBox _unlimitedFireCheckBox;

	private CheckBox _plantInvincibleCheckBox;

	private CheckBox _noLoseCheckBox;

	private SpinBox _sunSpinBox;

	private SpinBox _coinSpinBox;

	private SpinBox _crystalSpinBox;

	private CheckBox _wavePausedCheckBox;

	private CheckBox _noZombieSpawnCheckBox;

	private SpinBox _waveSpinBox;

	private CheckBox _brainInvincibleCheckBox;

	private Label _gameSpeedLabel;

	private HSlider _gameSpeedSlider;

	private Label _fpsLabel;

	private Label _waveInfoLabel;

	private Label _characterCountLabel;

	private Label _bulletCountLabel;

	[Export(PropertyHint.None, "")]
	public bool debug;

	[Export(PropertyHint.None, "")]
	public bool debugUnlimitedFire;

	private bool _debugCoinMax;

	[Export(PropertyHint.None, "")]
	public bool debugOpenAllLevel;

	[Export(PropertyHint.None, "")]
	public bool debugSunMax;

	[Export(PropertyHint.None, "")]
	public bool debugPacketSelect;

	[Export(PropertyHint.None, "")]
	public bool debugPacketOpenAll;

	[Export(PropertyHint.None, "")]
	public bool debugPacketColdDown;

	[Export(PropertyHint.None, "")]
	public bool debugOpenAllCustom;

	[Export(PropertyHint.None, "")]
	public bool debugOpenGlove;

	[Export(PropertyHint.None, "")]
	public bool debugPlantInvincible;

	[Export(PropertyHint.None, "")]
	public bool debugNoLose;

	[Export(PropertyHint.None, "")]
	public bool debugWavePaused;

	[Export(PropertyHint.None, "")]
	public bool debugNoZombieSpawn;

	[Export(PropertyHint.None, "")]
	public bool debugBrainInvincible;

	public static CommandManager Instance;

	[Export(PropertyHint.None, "")]
	public bool debugCoinMax
	{
		get
		{
			return _debugCoinMax;
		}
		set
		{
			_debugCoinMax = value;
			if (value && GodotObject.IsInstanceValid(TowerDefenseManager.Instance?.coinBank))
			{
				TowerDefenseManager.Instance.coinBank.SetNum(999999999L);
			}
		}
	}

	public override void _Ready()
	{
		Instance = this;
		if (debugCoinMax && GodotObject.IsInstanceValid(TowerDefenseManager.Instance?.coinBank))
		{
			TowerDefenseManager.Instance.coinBank.SetNum(999999999L);
		}
		_openButton = GetNode<MainButton>("%OpenButton");
		_guiLayer = GetNode<CanvasLayer>("GUILayer");
		_openAllLevelCheckBox = GetNode<CheckBox>("%OpenAllLevelCheckBox");
		_coinMaxCheckBox = GetNode<CheckBox>("%CoinMaxCheckBox");
		_sunMaxCheckBox = GetNode<CheckBox>("%SunMaxCheckBox");
		_packetSelectCheckBox = GetNode<CheckBox>("%PacketSelectCheckBox");
		_packetOpenAllCheckBox = GetNode<CheckBox>("%PacketOpenAllCheckBox");
		_packetColdDownCheckBox = GetNode<CheckBox>("%PacketColdDownCheckBox");
		_openAllCustomCheckBox = GetNode<CheckBox>("%OpenAllCustomCheckBox");
		_openGloveCheckBox = GetNode<CheckBox>("%OpenGloveCheckBox");
		_unlimitedFireCheckBox = GetNode<CheckBox>("%UnlimitedFireCheckBox");
		_plantInvincibleCheckBox = GetNode<CheckBox>("%PlantInvincibleCheckBox");
		_noLoseCheckBox = GetNode<CheckBox>("%NoLoseCheckBox");
		_sunSpinBox = GetNode<SpinBox>("%SunSpinBox");
		_coinSpinBox = GetNode<SpinBox>("%CoinSpinBox");
		_crystalSpinBox = GetNode<SpinBox>("%CrystalSpinBox");
		_wavePausedCheckBox = GetNode<CheckBox>("%WavePausedCheckBox");
		_noZombieSpawnCheckBox = GetNode<CheckBox>("%NoZombieSpawnCheckBox");
		_waveSpinBox = GetNode<SpinBox>("%WaveSpinBox");
		_brainInvincibleCheckBox = GetNode<CheckBox>("%BrainInvincibleCheckBox");
		_gameSpeedLabel = GetNode<Label>("%GameSpeedLabel");
		_gameSpeedSlider = GetNode<HSlider>("%GameSpeedSlider");
		_fpsLabel = GetNode<Label>("%FpsLabel");
		_waveInfoLabel = GetNode<Label>("%WaveInfoLabel");
		_characterCountLabel = GetNode<Label>("%CharacterCountLabel");
		_bulletCountLabel = GetNode<Label>("%BulletCountLabel");
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/游戏内/VBox/SunHBox/SunSetButton").Pressed += SetSunValue;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/游戏内/VBox/CoinHBox/CoinSetButton").Pressed += SetCoinValue;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/游戏内/VBox/CrystalHBox/CrystalSetButton").Pressed += SetCrystalValue;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/战斗/VBox/WaveHBox/WaveButton").Pressed += SkipToWave;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/战斗/VBox/SkipWaitButton").Pressed += SkipWaveWait;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/战斗/VBox/FinalWaveButton").Pressed += SkipToFinalWave;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/战斗/VBox/KillAllButton").Pressed += KillAllZombies;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/战斗/VBox/InstantWinButton").Pressed += InstantWin;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/场景/VBox/RestoreMowerButton").Pressed += RestoreAllMowers;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/场景/VBox/RemoveMowerButton").Pressed += RemoveAllMowers;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/场景/VBox/ResetBrainButton").Pressed += ResetAllBrains;
		GetNode<BaseButton>("GUILayer/CommandContainer/TabContainer/速度/VBox/SpeedResetButton").Pressed += _on_speed_reset_button_pressed;
		GetNode<BaseButton>("GUILayer/TestLevelButton").Pressed += TestLevelButtonPressed;
		GetNode<BaseButton>("GUILayer/LoadLevelButton").Pressed += LoadLevelButtonPressed;
		GetNode<BaseButton>("%OpenButton").Toggled += OpenButtonToggled;
		if (!debug)
		{
			_openButton.Visible = false;
			ProcessMode = ProcessModeEnum.Disabled;
		}
		else
		{
			Init();
			SetProcess(_guiLayer.Visible);
		}
	}

	public override void _Input(InputEvent _event)
	{
		if (debug && Input.IsActionJustPressed("Command"))
		{
			GrabFocus();
			SetCommandLayerVisible(!_guiLayer.Visible);
		}
	}

	public override void _Process(double delta)
	{
		if (!debug || !_guiLayer.Visible)
		{
			return;
		}
		_debugStatsRefreshRemaining -= delta;
		if (_debugStatsRefreshRemaining > 0.0)
		{
			return;
		}
		_debugStatsRefreshRemaining = 0.2;
		_fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
		if (GodotObject.IsInstanceValid(TowerDefenseBattleFeatureWave.Instance))
		{
			TowerDefenseBattleFeatureWave instance = TowerDefenseBattleFeatureWave.Instance;
			int num = ((instance.config != null) ? instance.config.wave.Count : 0);
			_waveInfoLabel.Text = $"波次: {instance.currentWave}/{num}\n血量: {instance.currentHpPoint:F0}/{instance.currentHpPointTotal:F0}";
			if (_waveSpinBox.MaxValue != (double)num)
			{
				_waveSpinBox.MaxValue = Mathf.Max(1, num);
			}
		}
		TowerDefenseBattleCharacterRegistry towerDefenseBattleCharacterRegistry = ((TowerDefenseManager.Instance != null) ? TowerDefenseManager.Instance.characterRegistry : null);
		if (GodotObject.IsInstanceValid(towerDefenseBattleCharacterRegistry))
		{
			int num2 = 0;
			int num3 = 0;
			List<TowerDefenseCharacter> cleanCharactersList = towerDefenseBattleCharacterRegistry.GetCleanCharactersList();
			for (int i = 0; i < cleanCharactersList.Count; i++)
			{
				if (cleanCharactersList[i] is TowerDefenseZombie)
				{
					num3++;
				}
				else if (cleanCharactersList[i] is TowerDefensePlant)
				{
					num2++;
				}
			}
			_characterCountLabel.Text = $"植物: {num2}  僵尸: {num3}  总数: {num2 + num3}";
		}
		int num4 = (GodotObject.IsInstanceValid(BulletField.Instance) ? BulletField.Instance.ActiveCount : 0);
		int nodeCountInGroup = GetTree().GetNodeCountInGroup("Projectile");
		_bulletCountLabel.Text = $"子弹: {num4 + nodeCountInGroup}  (场内:{nodeCountInGroup} 缓存场:{num4})";
	}

	public void Init()
	{
		_openAllLevelCheckBox.ButtonPressed = debugOpenAllLevel;
		_openAllLevelCheckBox.Toggled += (bool toggle) =>
		{
			debugOpenAllLevel = toggle;
		};
		_coinMaxCheckBox.ButtonPressed = debugCoinMax;
		_coinMaxCheckBox.Toggled += (bool toggle) =>
		{
			debugCoinMax = toggle;
		};
		_sunMaxCheckBox.ButtonPressed = debugSunMax;
		_sunMaxCheckBox.Toggled += (bool toggle) =>
		{
			debugSunMax = toggle;
		};
		_packetSelectCheckBox.ButtonPressed = debugPacketSelect;
		_packetSelectCheckBox.Toggled += (bool toggle) =>
		{
			debugPacketSelect = toggle;
		};
		_packetOpenAllCheckBox.ButtonPressed = debugPacketOpenAll;
		_packetOpenAllCheckBox.Toggled += (bool toggle) =>
		{
			debugPacketOpenAll = toggle;
		};
		_packetColdDownCheckBox.ButtonPressed = debugPacketColdDown;
		_packetColdDownCheckBox.Toggled += (bool toggle) =>
		{
			debugPacketColdDown = toggle;
		};
		_openAllCustomCheckBox.ButtonPressed = debugOpenAllCustom;
		_openAllCustomCheckBox.Toggled += (bool toggle) =>
		{
			debugOpenAllCustom = toggle;
		};
		_openGloveCheckBox.ButtonPressed = GameSaveManager.Instance.GetFeatureValue("Glove") > 0;
		_openGloveCheckBox.Toggled += (bool toggle) =>
		{
			Instance.debugOpenGlove = toggle;
		};
		_unlimitedFireCheckBox.ButtonPressed = debugUnlimitedFire;
		_unlimitedFireCheckBox.Toggled += (bool toggle) =>
		{
			debugUnlimitedFire = toggle;
		};
		_plantInvincibleCheckBox.ButtonPressed = debugPlantInvincible;
		_plantInvincibleCheckBox.Toggled += (bool toggle) =>
		{
			debugPlantInvincible = toggle;
		};
		_noLoseCheckBox.ButtonPressed = debugNoLose;
		_noLoseCheckBox.Toggled += (bool toggle) =>
		{
			debugNoLose = toggle;
		};
		_wavePausedCheckBox.ButtonPressed = debugWavePaused;
		_wavePausedCheckBox.Toggled += (bool toggle) =>
		{
			debugWavePaused = toggle;
		};
		_noZombieSpawnCheckBox.ButtonPressed = debugNoZombieSpawn;
		_noZombieSpawnCheckBox.Toggled += (bool toggle) =>
		{
			debugNoZombieSpawn = toggle;
		};
		_brainInvincibleCheckBox.ButtonPressed = debugBrainInvincible;
		_brainInvincibleCheckBox.Toggled += (bool toggle) =>
		{
			debugBrainInvincible = toggle;
		};
		_gameSpeedSlider.ValueChanged += (double value) =>
		{
			Global.TimeScale = value;
			_gameSpeedLabel.Text = $"游戏速度:{value:F1}x";
		};
	}

	public void OpenButtonToggled(bool toggledOn)
	{
		SetCommandLayerVisible(toggledOn);
	}

	private void SetCommandLayerVisible(bool visible)
	{
		_guiLayer.Visible = visible;
		GetTree().Paused = visible;
		_debugStatsRefreshRemaining = 0.0;
		SetProcess(debug & visible);
	}

	public void _on_speed_reset_button_pressed()
	{
		_gameSpeedSlider.Value = 1.0;
	}

	public void SetSunValue()
	{
		TowerDefenseManager.Instance.SetSun((int)_sunSpinBox.Value);
	}

	public void SetCoinValue()
	{
		TowerDefenseManager.Instance.coinBank.num = (long)_coinSpinBox.Value;
	}

	public void SetCrystalValue()
	{
		GameSaveManager.Instance.SetKeyValue("CrystalNum", (int)_crystalSpinBox.Value);
		GameSaveManager.Instance.Save();
	}

	public void TestLevelButtonPressed()
	{
		TowerDefenseManager.Instance.currentLevelConfig = GD.Load<TowerDefenseLevelConfig>("uid://bfl6f5wb3lu7m");
		_guiLayer.Visible = !_guiLayer.Visible;
		GetTree().Paused = _guiLayer.Visible;
		Global.Instance.enterLevelMode = "LevelChoose";
		SceneManager.Instance.ChangeScene("TowerDefense");
	}

	public void LoadLevelButtonPressed()
	{
		DisplayServer.FileDialogShow("打开关卡文件", "", "", showHidden: false, DisplayServer.FileDialogMode.OpenFile, new string[1] { "*.json,*.tres" }, Callable.From((long status, string[] selectedPaths, int selectedFilterIndex) =>
		{
			if (selectedPaths.Length != 0)
			{
				LoadSelectedLevelFile(selectedPaths[0]);
			}
		}));
	}

	private void LoadSelectedLevelFile(string selectedPath)
	{
		if (string.IsNullOrEmpty(selectedPath))
		{
			return;
		}
		byte[] fileAsBytes = FileAccess.GetFileAsBytes(selectedPath);
		if (fileAsBytes == null || fileAsBytes.Length == 0)
		{
			BroadcastInvalidLevelFile();
			return;
		}
		string text = selectedPath.GetExtension().ToLowerInvariant();
		TowerDefenseLevelConfig config3;
		if (!(text == "json"))
		{
			TowerDefenseLevelConfig config2;
			if (text == "tres")
			{
				if (TryLoadTresLevel(fileAsBytes, out var config))
				{
					EnterLoadedLevel(config);
					return;
				}
			}
			else if (TryLoadJsonLevel(fileAsBytes, out config2) || TryLoadTresLevel(fileAsBytes, out config2))
			{
				EnterLoadedLevel(config2);
				return;
			}
		}
		else if (TryLoadJsonLevel(fileAsBytes, out config3))
		{
			EnterLoadedLevel(config3);
			return;
		}
		BroadcastInvalidLevelFile();
	}

	private static bool TryLoadJsonLevel(byte[] bytes, out TowerDefenseLevelConfig config)
	{
		config = null;
		if (!StoreSelectedLevelBytes(bytes, "user://Csharp/import_level.json"))
		{
			return false;
		}
		try
		{
			Json json = new Json();
			if (json.Parse(bytes.GetStringFromUtf8()) != Error.Ok)
			{
				return false;
			}
			TowerDefenseLevelConfig towerDefenseLevelConfig = new TowerDefenseLevelConfig
			{
				data = json
			};
			towerDefenseLevelConfig.Init();
			config = (TowerDefenseLevelConfig)towerDefenseLevelConfig.DuplicateDeep(Resource.DeepDuplicateMode.Internal);
			return config != null;
		}
		catch
		{
			config = null;
			return false;
		}
	}

	private static bool TryLoadTresLevel(byte[] bytes, out TowerDefenseLevelConfig config)
	{
		config = null;
		if (!StoreSelectedLevelBytes(bytes, "user://Csharp/import_level.tres"))
		{
			return false;
		}
		ResourceTextScriptMigration.MigrateLevelEditorResourceForLoadIfNeeded("user://Csharp/import_level.tres");
		TowerDefenseLevelConfig towerDefenseLevelConfig = ResourceLoader.Load<TowerDefenseLevelConfig>("user://Csharp/import_level.tres", "", ResourceLoader.CacheMode.Ignore);
		if (towerDefenseLevelConfig == null)
		{
			return false;
		}
		config = (TowerDefenseLevelConfig)towerDefenseLevelConfig.DuplicateDeep(Resource.DeepDuplicateMode.Internal);
		return config != null;
	}

	private static bool StoreSelectedLevelBytes(byte[] bytes, string tempPath)
	{
		if (bytes == null || bytes.Length == 0 || string.IsNullOrEmpty(tempPath))
		{
			return false;
		}
		EnsureImportLevelDirectory();
		FileAccess fileAccess = FileAccess.Open(tempPath, FileAccess.ModeFlags.Write);
		if (fileAccess == null)
		{
			return false;
		}
		fileAccess.StoreBuffer(bytes);
		fileAccess.Close();
		return true;
	}

	private static void EnsureImportLevelDirectory()
	{
		if (!DirAccess.DirExistsAbsolute("user://Csharp"))
		{
			DirAccess.MakeDirRecursiveAbsolute("user://Csharp");
		}
	}

	private void EnterLoadedLevel(TowerDefenseLevelConfig config)
	{
		TowerDefenseManager.Instance.currentLevelConfig = config;
		_guiLayer.Visible = false;
		GetTree().Paused = _guiLayer.Visible;
		Global.Instance.enterLevelMode = "LoadLevel";
		SceneManager.Instance.ChangeScene("TowerDefense");
		Global.Instance.isEditor = false;
	}

	private static void BroadcastInvalidLevelFile()
	{
		BroadCastManager.Instance.BroadCastFloatCreate("不是关卡文件", Colors.Red);
	}

	public async void SkipToWave()
	{
		TowerDefenseBattleFeatureWave wave = TowerDefenseBattleFeatureWave.Instance;
		if (GodotObject.IsInstanceValid(wave))
		{
			int targetWave = (int)_waveSpinBox.Value;
			while (wave.currentWave < targetWave && !wave.waveFinal)
			{
				wave.timer = wave.nextWaveTime;
				wave.awaitSpawn = false;
				await ToSignal(GetTree().CreateTimer(0.2, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			}
		}
	}

	public async void SkipToFinalWave()
	{
		TowerDefenseBattleFeatureWave wave = TowerDefenseBattleFeatureWave.Instance;
		if (GodotObject.IsInstanceValid(wave))
		{
			if (!wave.waveStart)
			{
				wave.waveStart = true;
			}
			while (!wave.waveFinal)
			{
				wave.timer = wave.nextWaveTime;
				wave.awaitSpawn = false;
				await ToSignal(GetTree().CreateTimer(0.2, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			}
		}
	}

	public void SkipWaveWait()
	{
		TowerDefenseBattleFeatureWave instance = TowerDefenseBattleFeatureWave.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			instance.timer = instance.nextWaveTime;
			instance.awaitSpawn = false;
		}
	}

	public void KillAllZombies()
	{
		foreach (Node item in GetTree().GetNodesInGroup("Zombie"))
		{
			if (GodotObject.IsInstanceValid(item) && item is TowerDefenseZombie towerDefenseZombie && !towerDefenseZombie.instance.die)
			{
				towerDefenseZombie.Hurt(1000000000000.0);
			}
		}
	}

	public void InstantWin()
	{
		TowerDefenseBattleFeatureWave instance = TowerDefenseBattleFeatureWave.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			KillAllZombies();
			instance.waveStart = true;
			instance.waveFinal = true;
			instance.EmitFinal();
			instance.awaitSpawn = false;
		}
	}

	public void RestoreAllMowers()
	{
		if (TowerDefenseManager.Instance == null)
		{
			return;
		}
		TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
		if (currentControl == null || !(currentControl.GetFeature("Mower") is TowerDefenseBattleFeatureMower towerDefenseBattleFeatureMower))
		{
			return;
		}
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (mapFeature == null)
		{
			return;
		}
		for (int i = 1; i <= mapFeature.config.gridNum.Y; i++)
		{
			if (TowerDefenseManager.Instance.GetMapLineUse(i) && !GodotObject.IsInstanceValid(towerDefenseBattleFeatureMower.mowerLine[i]))
			{
				towerDefenseBattleFeatureMower.CreateMower(i);
			}
		}
	}

	public void RemoveAllMowers()
	{
		if (TowerDefenseManager.Instance == null)
		{
			return;
		}
		TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
		if (currentControl == null || !(currentControl.GetFeature("Mower") is TowerDefenseBattleFeatureMower towerDefenseBattleFeatureMower))
		{
			return;
		}
		for (int i = 0; i < towerDefenseBattleFeatureMower.mowerLine.Count; i++)
		{
			if (GodotObject.IsInstanceValid(towerDefenseBattleFeatureMower.mowerLine[i]))
			{
				towerDefenseBattleFeatureMower.mowerLine[i].QueueFree();
				towerDefenseBattleFeatureMower.mowerLine[i] = null;
			}
		}
	}

	public void ResetAllBrains()
	{
		if (TowerDefenseManager.Instance == null)
		{
			return;
		}
		TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
		if (currentControl == null || !(currentControl.GetFeature("Brain") is TowerDefenseBattleFeatureBrain towerDefenseBattleFeatureBrain))
		{
			return;
		}
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (mapFeature == null)
		{
			return;
		}
		for (int i = 0; i < towerDefenseBattleFeatureBrain.brainLine.Count; i++)
		{
			if (GodotObject.IsInstanceValid(towerDefenseBattleFeatureBrain.brainLine[i]))
			{
				towerDefenseBattleFeatureBrain.brainLine[i].QueueFree();
				towerDefenseBattleFeatureBrain.brainLine[i] = null;
			}
		}
		for (int j = 1; j <= mapFeature.config.gridNum.Y; j++)
		{
			if (TowerDefenseManager.Instance.GetMapLineUse(j))
			{
				towerDefenseBattleFeatureBrain.CreateBrain(j);
			}
		}
	}

	public CommandManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/CommandManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(25)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenButtonToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCommandLayerVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._on_speed_reset_button_pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetSunValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCoinValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCrystalValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TestLevelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadLevelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadSelectedLevelFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "selectedPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StoreSelectedLevelBytes, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedByteArray, "bytes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tempPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureImportLevelDirectory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.EnterLoadedLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BroadcastInvalidLevelFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.SkipToWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SkipToFinalWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SkipWaveWait, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.KillAllZombies, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InstantWin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreAllMowers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveAllMowers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetAllBrains, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenButtonToggled && args.Count == 1)
		{
			OpenButtonToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCommandLayerVisible && args.Count == 1)
		{
			SetCommandLayerVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._on_speed_reset_button_pressed && args.Count == 0)
		{
			_on_speed_reset_button_pressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SetSunValue && args.Count == 0)
		{
			SetSunValue();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCoinValue && args.Count == 0)
		{
			SetCoinValue();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCrystalValue && args.Count == 0)
		{
			SetCrystalValue();
			ret = default;
			return true;
		}
		if (method == MethodName.TestLevelButtonPressed && args.Count == 0)
		{
			TestLevelButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadLevelButtonPressed && args.Count == 0)
		{
			LoadLevelButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadSelectedLevelFile && args.Count == 1)
		{
			LoadSelectedLevelFile(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StoreSelectedLevelBytes && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(StoreSelectedLevelBytes(VariantUtils.ConvertTo<byte[]>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.EnsureImportLevelDirectory && args.Count == 0)
		{
			EnsureImportLevelDirectory();
			ret = default;
			return true;
		}
		if (method == MethodName.EnterLoadedLevel && args.Count == 1)
		{
			EnterLoadedLevel(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BroadcastInvalidLevelFile && args.Count == 0)
		{
			BroadcastInvalidLevelFile();
			ret = default;
			return true;
		}
		if (method == MethodName.SkipToWave && args.Count == 0)
		{
			SkipToWave();
			ret = default;
			return true;
		}
		if (method == MethodName.SkipToFinalWave && args.Count == 0)
		{
			SkipToFinalWave();
			ret = default;
			return true;
		}
		if (method == MethodName.SkipWaveWait && args.Count == 0)
		{
			SkipWaveWait();
			ret = default;
			return true;
		}
		if (method == MethodName.KillAllZombies && args.Count == 0)
		{
			KillAllZombies();
			ret = default;
			return true;
		}
		if (method == MethodName.InstantWin && args.Count == 0)
		{
			InstantWin();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreAllMowers && args.Count == 0)
		{
			RestoreAllMowers();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveAllMowers && args.Count == 0)
		{
			RemoveAllMowers();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetAllBrains && args.Count == 0)
		{
			ResetAllBrains();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.StoreSelectedLevelBytes && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(StoreSelectedLevelBytes(VariantUtils.ConvertTo<byte[]>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.EnsureImportLevelDirectory && args.Count == 0)
		{
			EnsureImportLevelDirectory();
			ret = default;
			return true;
		}
		if (method == MethodName.BroadcastInvalidLevelFile && args.Count == 0)
		{
			BroadcastInvalidLevelFile();
			ret = default;
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
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.OpenButtonToggled)
		{
			return true;
		}
		if (method == MethodName.SetCommandLayerVisible)
		{
			return true;
		}
		if (method == MethodName._on_speed_reset_button_pressed)
		{
			return true;
		}
		if (method == MethodName.SetSunValue)
		{
			return true;
		}
		if (method == MethodName.SetCoinValue)
		{
			return true;
		}
		if (method == MethodName.SetCrystalValue)
		{
			return true;
		}
		if (method == MethodName.TestLevelButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LoadLevelButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LoadSelectedLevelFile)
		{
			return true;
		}
		if (method == MethodName.StoreSelectedLevelBytes)
		{
			return true;
		}
		if (method == MethodName.EnsureImportLevelDirectory)
		{
			return true;
		}
		if (method == MethodName.EnterLoadedLevel)
		{
			return true;
		}
		if (method == MethodName.BroadcastInvalidLevelFile)
		{
			return true;
		}
		if (method == MethodName.SkipToWave)
		{
			return true;
		}
		if (method == MethodName.SkipToFinalWave)
		{
			return true;
		}
		if (method == MethodName.SkipWaveWait)
		{
			return true;
		}
		if (method == MethodName.KillAllZombies)
		{
			return true;
		}
		if (method == MethodName.InstantWin)
		{
			return true;
		}
		if (method == MethodName.RestoreAllMowers)
		{
			return true;
		}
		if (method == MethodName.RemoveAllMowers)
		{
			return true;
		}
		if (method == MethodName.ResetAllBrains)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.debugCoinMax)
		{
			debugCoinMax = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._debugStatsRefreshRemaining)
		{
			_debugStatsRefreshRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._openButton)
		{
			_openButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._guiLayer)
		{
			_guiLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName._openAllLevelCheckBox)
		{
			_openAllLevelCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._coinMaxCheckBox)
		{
			_coinMaxCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._sunMaxCheckBox)
		{
			_sunMaxCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._packetSelectCheckBox)
		{
			_packetSelectCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._packetOpenAllCheckBox)
		{
			_packetOpenAllCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._packetColdDownCheckBox)
		{
			_packetColdDownCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._openAllCustomCheckBox)
		{
			_openAllCustomCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._openGloveCheckBox)
		{
			_openGloveCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._unlimitedFireCheckBox)
		{
			_unlimitedFireCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._plantInvincibleCheckBox)
		{
			_plantInvincibleCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._noLoseCheckBox)
		{
			_noLoseCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._sunSpinBox)
		{
			_sunSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._coinSpinBox)
		{
			_coinSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._crystalSpinBox)
		{
			_crystalSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._wavePausedCheckBox)
		{
			_wavePausedCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._noZombieSpawnCheckBox)
		{
			_noZombieSpawnCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._waveSpinBox)
		{
			_waveSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._brainInvincibleCheckBox)
		{
			_brainInvincibleCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._gameSpeedLabel)
		{
			_gameSpeedLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._gameSpeedSlider)
		{
			_gameSpeedSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._fpsLabel)
		{
			_fpsLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._waveInfoLabel)
		{
			_waveInfoLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._characterCountLabel)
		{
			_characterCountLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._bulletCountLabel)
		{
			_bulletCountLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.debug)
		{
			debug = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugUnlimitedFire)
		{
			debugUnlimitedFire = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._debugCoinMax)
		{
			_debugCoinMax = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugOpenAllLevel)
		{
			debugOpenAllLevel = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugSunMax)
		{
			debugSunMax = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugPacketSelect)
		{
			debugPacketSelect = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugPacketOpenAll)
		{
			debugPacketOpenAll = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugPacketColdDown)
		{
			debugPacketColdDown = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugOpenAllCustom)
		{
			debugOpenAllCustom = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugOpenGlove)
		{
			debugOpenGlove = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugPlantInvincible)
		{
			debugPlantInvincible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugNoLose)
		{
			debugNoLose = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugWavePaused)
		{
			debugWavePaused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugNoZombieSpawn)
		{
			debugNoZombieSpawn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debugBrainInvincible)
		{
			debugBrainInvincible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.debugCoinMax)
		{
			value = VariantUtils.CreateFrom<bool>(debugCoinMax);
			return true;
		}
		if (name == PropertyName._debugStatsRefreshRemaining)
		{
			value = VariantUtils.CreateFrom(in _debugStatsRefreshRemaining);
			return true;
		}
		if (name == PropertyName._openButton)
		{
			value = VariantUtils.CreateFrom(in _openButton);
			return true;
		}
		if (name == PropertyName._guiLayer)
		{
			value = VariantUtils.CreateFrom(in _guiLayer);
			return true;
		}
		if (name == PropertyName._openAllLevelCheckBox)
		{
			value = VariantUtils.CreateFrom(in _openAllLevelCheckBox);
			return true;
		}
		if (name == PropertyName._coinMaxCheckBox)
		{
			value = VariantUtils.CreateFrom(in _coinMaxCheckBox);
			return true;
		}
		if (name == PropertyName._sunMaxCheckBox)
		{
			value = VariantUtils.CreateFrom(in _sunMaxCheckBox);
			return true;
		}
		if (name == PropertyName._packetSelectCheckBox)
		{
			value = VariantUtils.CreateFrom(in _packetSelectCheckBox);
			return true;
		}
		if (name == PropertyName._packetOpenAllCheckBox)
		{
			value = VariantUtils.CreateFrom(in _packetOpenAllCheckBox);
			return true;
		}
		if (name == PropertyName._packetColdDownCheckBox)
		{
			value = VariantUtils.CreateFrom(in _packetColdDownCheckBox);
			return true;
		}
		if (name == PropertyName._openAllCustomCheckBox)
		{
			value = VariantUtils.CreateFrom(in _openAllCustomCheckBox);
			return true;
		}
		if (name == PropertyName._openGloveCheckBox)
		{
			value = VariantUtils.CreateFrom(in _openGloveCheckBox);
			return true;
		}
		if (name == PropertyName._unlimitedFireCheckBox)
		{
			value = VariantUtils.CreateFrom(in _unlimitedFireCheckBox);
			return true;
		}
		if (name == PropertyName._plantInvincibleCheckBox)
		{
			value = VariantUtils.CreateFrom(in _plantInvincibleCheckBox);
			return true;
		}
		if (name == PropertyName._noLoseCheckBox)
		{
			value = VariantUtils.CreateFrom(in _noLoseCheckBox);
			return true;
		}
		if (name == PropertyName._sunSpinBox)
		{
			value = VariantUtils.CreateFrom(in _sunSpinBox);
			return true;
		}
		if (name == PropertyName._coinSpinBox)
		{
			value = VariantUtils.CreateFrom(in _coinSpinBox);
			return true;
		}
		if (name == PropertyName._crystalSpinBox)
		{
			value = VariantUtils.CreateFrom(in _crystalSpinBox);
			return true;
		}
		if (name == PropertyName._wavePausedCheckBox)
		{
			value = VariantUtils.CreateFrom(in _wavePausedCheckBox);
			return true;
		}
		if (name == PropertyName._noZombieSpawnCheckBox)
		{
			value = VariantUtils.CreateFrom(in _noZombieSpawnCheckBox);
			return true;
		}
		if (name == PropertyName._waveSpinBox)
		{
			value = VariantUtils.CreateFrom(in _waveSpinBox);
			return true;
		}
		if (name == PropertyName._brainInvincibleCheckBox)
		{
			value = VariantUtils.CreateFrom(in _brainInvincibleCheckBox);
			return true;
		}
		if (name == PropertyName._gameSpeedLabel)
		{
			value = VariantUtils.CreateFrom(in _gameSpeedLabel);
			return true;
		}
		if (name == PropertyName._gameSpeedSlider)
		{
			value = VariantUtils.CreateFrom(in _gameSpeedSlider);
			return true;
		}
		if (name == PropertyName._fpsLabel)
		{
			value = VariantUtils.CreateFrom(in _fpsLabel);
			return true;
		}
		if (name == PropertyName._waveInfoLabel)
		{
			value = VariantUtils.CreateFrom(in _waveInfoLabel);
			return true;
		}
		if (name == PropertyName._characterCountLabel)
		{
			value = VariantUtils.CreateFrom(in _characterCountLabel);
			return true;
		}
		if (name == PropertyName._bulletCountLabel)
		{
			value = VariantUtils.CreateFrom(in _bulletCountLabel);
			return true;
		}
		if (name == PropertyName.debug)
		{
			value = VariantUtils.CreateFrom(in debug);
			return true;
		}
		if (name == PropertyName.debugUnlimitedFire)
		{
			value = VariantUtils.CreateFrom(in debugUnlimitedFire);
			return true;
		}
		if (name == PropertyName._debugCoinMax)
		{
			value = VariantUtils.CreateFrom(in _debugCoinMax);
			return true;
		}
		if (name == PropertyName.debugOpenAllLevel)
		{
			value = VariantUtils.CreateFrom(in debugOpenAllLevel);
			return true;
		}
		if (name == PropertyName.debugSunMax)
		{
			value = VariantUtils.CreateFrom(in debugSunMax);
			return true;
		}
		if (name == PropertyName.debugPacketSelect)
		{
			value = VariantUtils.CreateFrom(in debugPacketSelect);
			return true;
		}
		if (name == PropertyName.debugPacketOpenAll)
		{
			value = VariantUtils.CreateFrom(in debugPacketOpenAll);
			return true;
		}
		if (name == PropertyName.debugPacketColdDown)
		{
			value = VariantUtils.CreateFrom(in debugPacketColdDown);
			return true;
		}
		if (name == PropertyName.debugOpenAllCustom)
		{
			value = VariantUtils.CreateFrom(in debugOpenAllCustom);
			return true;
		}
		if (name == PropertyName.debugOpenGlove)
		{
			value = VariantUtils.CreateFrom(in debugOpenGlove);
			return true;
		}
		if (name == PropertyName.debugPlantInvincible)
		{
			value = VariantUtils.CreateFrom(in debugPlantInvincible);
			return true;
		}
		if (name == PropertyName.debugNoLose)
		{
			value = VariantUtils.CreateFrom(in debugNoLose);
			return true;
		}
		if (name == PropertyName.debugWavePaused)
		{
			value = VariantUtils.CreateFrom(in debugWavePaused);
			return true;
		}
		if (name == PropertyName.debugNoZombieSpawn)
		{
			value = VariantUtils.CreateFrom(in debugNoZombieSpawn);
			return true;
		}
		if (name == PropertyName.debugBrainInvincible)
		{
			value = VariantUtils.CreateFrom(in debugBrainInvincible);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._debugStatsRefreshRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._guiLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openAllLevelCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._coinMaxCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunMaxCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetSelectCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetOpenAllCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetColdDownCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openAllCustomCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openGloveCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlimitedFireCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._plantInvincibleCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._noLoseCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._coinSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._crystalSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._wavePausedCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._noZombieSpawnCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._waveSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._brainInvincibleCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gameSpeedLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gameSpeedSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fpsLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._waveInfoLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterCountLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bulletCountLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debug, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugUnlimitedFire, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._debugCoinMax, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugCoinMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugOpenAllLevel, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugSunMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugPacketSelect, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugPacketOpenAll, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugPacketColdDown, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugOpenAllCustom, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugOpenGlove, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugPlantInvincible, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugNoLose, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugWavePaused, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugNoZombieSpawn, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debugBrainInvincible, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.debugCoinMax, Variant.From<bool>(debugCoinMax));
		info.AddProperty(PropertyName._debugStatsRefreshRemaining, Variant.From(in _debugStatsRefreshRemaining));
		info.AddProperty(PropertyName._openButton, Variant.From(in _openButton));
		info.AddProperty(PropertyName._guiLayer, Variant.From(in _guiLayer));
		info.AddProperty(PropertyName._openAllLevelCheckBox, Variant.From(in _openAllLevelCheckBox));
		info.AddProperty(PropertyName._coinMaxCheckBox, Variant.From(in _coinMaxCheckBox));
		info.AddProperty(PropertyName._sunMaxCheckBox, Variant.From(in _sunMaxCheckBox));
		info.AddProperty(PropertyName._packetSelectCheckBox, Variant.From(in _packetSelectCheckBox));
		info.AddProperty(PropertyName._packetOpenAllCheckBox, Variant.From(in _packetOpenAllCheckBox));
		info.AddProperty(PropertyName._packetColdDownCheckBox, Variant.From(in _packetColdDownCheckBox));
		info.AddProperty(PropertyName._openAllCustomCheckBox, Variant.From(in _openAllCustomCheckBox));
		info.AddProperty(PropertyName._openGloveCheckBox, Variant.From(in _openGloveCheckBox));
		info.AddProperty(PropertyName._unlimitedFireCheckBox, Variant.From(in _unlimitedFireCheckBox));
		info.AddProperty(PropertyName._plantInvincibleCheckBox, Variant.From(in _plantInvincibleCheckBox));
		info.AddProperty(PropertyName._noLoseCheckBox, Variant.From(in _noLoseCheckBox));
		info.AddProperty(PropertyName._sunSpinBox, Variant.From(in _sunSpinBox));
		info.AddProperty(PropertyName._coinSpinBox, Variant.From(in _coinSpinBox));
		info.AddProperty(PropertyName._crystalSpinBox, Variant.From(in _crystalSpinBox));
		info.AddProperty(PropertyName._wavePausedCheckBox, Variant.From(in _wavePausedCheckBox));
		info.AddProperty(PropertyName._noZombieSpawnCheckBox, Variant.From(in _noZombieSpawnCheckBox));
		info.AddProperty(PropertyName._waveSpinBox, Variant.From(in _waveSpinBox));
		info.AddProperty(PropertyName._brainInvincibleCheckBox, Variant.From(in _brainInvincibleCheckBox));
		info.AddProperty(PropertyName._gameSpeedLabel, Variant.From(in _gameSpeedLabel));
		info.AddProperty(PropertyName._gameSpeedSlider, Variant.From(in _gameSpeedSlider));
		info.AddProperty(PropertyName._fpsLabel, Variant.From(in _fpsLabel));
		info.AddProperty(PropertyName._waveInfoLabel, Variant.From(in _waveInfoLabel));
		info.AddProperty(PropertyName._characterCountLabel, Variant.From(in _characterCountLabel));
		info.AddProperty(PropertyName._bulletCountLabel, Variant.From(in _bulletCountLabel));
		info.AddProperty(PropertyName.debug, Variant.From(in debug));
		info.AddProperty(PropertyName.debugUnlimitedFire, Variant.From(in debugUnlimitedFire));
		info.AddProperty(PropertyName._debugCoinMax, Variant.From(in _debugCoinMax));
		info.AddProperty(PropertyName.debugOpenAllLevel, Variant.From(in debugOpenAllLevel));
		info.AddProperty(PropertyName.debugSunMax, Variant.From(in debugSunMax));
		info.AddProperty(PropertyName.debugPacketSelect, Variant.From(in debugPacketSelect));
		info.AddProperty(PropertyName.debugPacketOpenAll, Variant.From(in debugPacketOpenAll));
		info.AddProperty(PropertyName.debugPacketColdDown, Variant.From(in debugPacketColdDown));
		info.AddProperty(PropertyName.debugOpenAllCustom, Variant.From(in debugOpenAllCustom));
		info.AddProperty(PropertyName.debugOpenGlove, Variant.From(in debugOpenGlove));
		info.AddProperty(PropertyName.debugPlantInvincible, Variant.From(in debugPlantInvincible));
		info.AddProperty(PropertyName.debugNoLose, Variant.From(in debugNoLose));
		info.AddProperty(PropertyName.debugWavePaused, Variant.From(in debugWavePaused));
		info.AddProperty(PropertyName.debugNoZombieSpawn, Variant.From(in debugNoZombieSpawn));
		info.AddProperty(PropertyName.debugBrainInvincible, Variant.From(in debugBrainInvincible));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.debugCoinMax, out var value))
		{
			debugCoinMax = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._debugStatsRefreshRemaining, out var value2))
		{
			_debugStatsRefreshRemaining = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._openButton, out var value3))
		{
			_openButton = value3.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._guiLayer, out var value4))
		{
			_guiLayer = value4.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName._openAllLevelCheckBox, out var value5))
		{
			_openAllLevelCheckBox = value5.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._coinMaxCheckBox, out var value6))
		{
			_coinMaxCheckBox = value6.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._sunMaxCheckBox, out var value7))
		{
			_sunMaxCheckBox = value7.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._packetSelectCheckBox, out var value8))
		{
			_packetSelectCheckBox = value8.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._packetOpenAllCheckBox, out var value9))
		{
			_packetOpenAllCheckBox = value9.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._packetColdDownCheckBox, out var value10))
		{
			_packetColdDownCheckBox = value10.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._openAllCustomCheckBox, out var value11))
		{
			_openAllCustomCheckBox = value11.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._openGloveCheckBox, out var value12))
		{
			_openGloveCheckBox = value12.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._unlimitedFireCheckBox, out var value13))
		{
			_unlimitedFireCheckBox = value13.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._plantInvincibleCheckBox, out var value14))
		{
			_plantInvincibleCheckBox = value14.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._noLoseCheckBox, out var value15))
		{
			_noLoseCheckBox = value15.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._sunSpinBox, out var value16))
		{
			_sunSpinBox = value16.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._coinSpinBox, out var value17))
		{
			_coinSpinBox = value17.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._crystalSpinBox, out var value18))
		{
			_crystalSpinBox = value18.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._wavePausedCheckBox, out var value19))
		{
			_wavePausedCheckBox = value19.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._noZombieSpawnCheckBox, out var value20))
		{
			_noZombieSpawnCheckBox = value20.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._waveSpinBox, out var value21))
		{
			_waveSpinBox = value21.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._brainInvincibleCheckBox, out var value22))
		{
			_brainInvincibleCheckBox = value22.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._gameSpeedLabel, out var value23))
		{
			_gameSpeedLabel = value23.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._gameSpeedSlider, out var value24))
		{
			_gameSpeedSlider = value24.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._fpsLabel, out var value25))
		{
			_fpsLabel = value25.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._waveInfoLabel, out var value26))
		{
			_waveInfoLabel = value26.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._characterCountLabel, out var value27))
		{
			_characterCountLabel = value27.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._bulletCountLabel, out var value28))
		{
			_bulletCountLabel = value28.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.debug, out var value29))
		{
			debug = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugUnlimitedFire, out var value30))
		{
			debugUnlimitedFire = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._debugCoinMax, out var value31))
		{
			_debugCoinMax = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugOpenAllLevel, out var value32))
		{
			debugOpenAllLevel = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugSunMax, out var value33))
		{
			debugSunMax = value33.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugPacketSelect, out var value34))
		{
			debugPacketSelect = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugPacketOpenAll, out var value35))
		{
			debugPacketOpenAll = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugPacketColdDown, out var value36))
		{
			debugPacketColdDown = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugOpenAllCustom, out var value37))
		{
			debugOpenAllCustom = value37.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugOpenGlove, out var value38))
		{
			debugOpenGlove = value38.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugPlantInvincible, out var value39))
		{
			debugPlantInvincible = value39.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugNoLose, out var value40))
		{
			debugNoLose = value40.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugWavePaused, out var value41))
		{
			debugWavePaused = value41.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugNoZombieSpawn, out var value42))
		{
			debugNoZombieSpawn = value42.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debugBrainInvincible, out var value43))
		{
			debugBrainInvincible = value43.As<bool>();
		}
	}
}
