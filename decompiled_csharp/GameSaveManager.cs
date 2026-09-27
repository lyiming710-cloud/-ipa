using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using Cysharp.Text;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using Microsoft.Extensions.Logging;
using PVZHE.ModEditor.ModSystem;
using ZLogger;

[GlobalClass]
[ScriptPath("res://Core/GameSaveManager/GameSaveManager.cs")]
public class GameSaveManager : Node
{
	internal sealed class ProgressRecoverySession
	{
		internal string PrimaryPath;

		internal string BackupPath;

		internal ulong ControlId;

		internal bool UsedBackup;

		internal bool BackupAvailable;

		internal string BackupError = "";

		internal bool AwaitingDecision = true;

		internal bool Discarding;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ShouldDeferStartupDataLoad = "ShouldDeferStartupDataLoad";

		public static readonly StringName IsLoadingScenePath = "IsLoadingScenePath";

		public static readonly StringName InitializeStorageAndLoad = "InitializeStorageAndLoad";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName Check = "Check";

		public static readonly StringName CheckCondition = "CheckCondition";

		public static readonly StringName ApplySave = "ApplySave";

		public static readonly StringName Save = "Save";

		public static readonly StringName SaveGameConfig = "SaveGameConfig";

		public static readonly StringName SaveResourceAtomically = "SaveResourceAtomically";

		public static readonly StringName ScheduleSave = "ScheduleSave";

		public static readonly StringName DeferredSave = "DeferredSave";

		public static readonly StringName Load = "Load";

		public static readonly StringName SyncCoinBankFromSave = "SyncCoinBankFromSave";

		public static readonly StringName SaveCoinBankIfReady = "SaveCoinBankIfReady";

		public static readonly StringName SyncCoinBankForExport = "SyncCoinBankForExport";

		public static readonly StringName EnsureLoaded = "EnsureLoaded";

		public static readonly StringName EnsureUser = "EnsureUser";

		public static readonly StringName GetUserCurrent = "GetUserCurrent";

		public static readonly StringName SetUserCurrent = "SetUserCurrent";

		public static readonly StringName GetUserList = "GetUserList";

		public static readonly StringName HasUser = "HasUser";

		public static readonly StringName AddUser = "AddUser";

		public static readonly StringName RenameUser = "RenameUser";

		public static readonly StringName DeleteUser = "DeleteUser";

		public static readonly StringName GetUserDictionary = "GetUserDictionary";

		public static readonly StringName GetCategoryDictionary = "GetCategoryDictionary";

		public static readonly StringName GetCategoryValue = "GetCategoryValue";

		public static readonly StringName SetCategoryValue = "SetCategoryValue";

		public static readonly StringName GetTowerDefensePacketDictionary = "GetTowerDefensePacketDictionary";

		public static readonly StringName GetTowerDefensePacketValue = "GetTowerDefensePacketValue";

		public static readonly StringName SetTowerDefensePacketValue = "SetTowerDefensePacketValue";

		public static readonly StringName CreateTowerDefensePacketDefaultValue = "CreateTowerDefensePacketDefaultValue";

		public static readonly StringName GetFeatureDictionary = "GetFeatureDictionary";

		public static readonly StringName GetFeatureValue = "GetFeatureValue";

		public static readonly StringName HasFeatureDefinition = "HasFeatureDefinition";

		public static readonly StringName SetFeatureValue = "SetFeatureValue";

		public static readonly StringName GetTutorialDictionary = "GetTutorialDictionary";

		public static readonly StringName GetTutorialValue = "GetTutorialValue";

		public static readonly StringName SetTutorialValue = "SetTutorialValue";

		public static readonly StringName GetLevelDictionary = "GetLevelDictionary";

		public static readonly StringName GetLevelValue = "GetLevelValue";

		public static readonly StringName SetLevelValue = "SetLevelValue";

		public static readonly StringName GetKeyDictionary = "GetKeyDictionary";

		public static readonly StringName GetKeyValue = "GetKeyValue";

		public static readonly StringName SetKeyValue = "SetKeyValue";

		public static readonly StringName GetConfigDictionary = "GetConfigDictionary";

		public static readonly StringName GetConfigValue = "GetConfigValue";

		public static readonly StringName SetConfigValue = "SetConfigValue";

		public static readonly StringName GetDailyLevel = "GetDailyLevel";

		public static readonly StringName SaveDailyLevel = "SaveDailyLevel";

		public static readonly StringName GetOnlineLevel = "GetOnlineLevel";

		public static readonly StringName SaveOnlineLevel = "SaveOnlineLevel";

		public static readonly StringName CanUseLevelProgressInCurrentMode = "CanUseLevelProgressInCurrentMode";

		public static readonly StringName _GetUserCurrentSafe = "_GetUserCurrentSafe";

		public static readonly StringName RefreshCrystalNum = "RefreshCrystalNum";

		public static readonly StringName ReplaceMainSave = "ReplaceMainSave";

		public static readonly StringName HasUsableSaveData = "HasUsableSaveData";

		public static readonly StringName NormalizeImportedSave = "NormalizeImportedSave";

		public static readonly StringName AcceptProgressRecovery = "AcceptProgressRecovery";

		public static readonly StringName DiscardProgressRecovery = "DiscardProgressRecovery";

		public static readonly StringName EndProgressRecovery = "EndProgressRecovery";

		public static readonly StringName ProgressBackupPath = "ProgressBackupPath";

		public static readonly StringName CanWriteRecoveredProgress = "CanWriteRecoveredProgress";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName savePath = "savePath";

		public static readonly StringName configPath = "configPath";

		public static readonly StringName dailyLevelPath = "dailyLevelPath";

		public static readonly StringName onlineLevelPath = "onlineLevelPath";

		public static readonly StringName config = "config";

		public static readonly StringName gameConfig = "gameConfig";

		public static readonly StringName _saveLoaded = "_saveLoaded";

		public static readonly StringName _storageInitialized = "_storageInitialized";

		public static readonly StringName _coinBankSyncedFromSave = "_coinBankSyncedFromSave";

		public static readonly StringName _mainSaveWriteBlocked = "_mainSaveWriteBlocked";

		public static readonly StringName _configSaveWriteBlocked = "_configSaveWriteBlocked";

		public static readonly StringName saveScheduled = "saveScheduled";

		public static readonly StringName configDirty = "configDirty";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private static readonly ILogger _logger = Log.CreateLogger<GameSaveManager>();

	private const string LoadingScenePath = "res://Scene/Loading/Loading.tscn";

	private static Json _checkInit;

	private static Json _towerDefensePacketInit;

	private static Json _featureInit;

	private static Json _tutorialInit;

	private static Json _levelInit;

	private static Json _keyInit;

	private static Json _configInit;

	private static Dictionary _towerDefensePacketInitDict;

	private static Dictionary _featureInitDict;

	private static Dictionary _tutorialInitDict;

	private static Dictionary _levelInitDict;

	private static Dictionary _keyInitDict;

	private static Dictionary _configInitDict;

	private static readonly HashSet<string> _reportedMissingTowerDefensePacketInitKeys = new HashSet<string>(StringComparer.Ordinal);

	private const string PATH = "user://Csharp/save.res";

	private const string PATHCONFIG = "user://Csharp/config.res";

	private const string PATHDAILYLEVEL = "user://Csharp/DailyLevel";

	private const string PATHONLINELEVEL = "user://Csharp/OnlineLevel";

	private const string PathDebug = "user://Csharp/Debug/save.res";

	private const string PathConfigDebug = "user://Csharp/Debug/config.res";

	private const string PathDailyLevelDebug = "user://Csharp/Debug/DailyLevel";

	private const string PathOnlineLevelDebug = "user://Csharp/Debug/OnlineLevel";

	[Export(PropertyHint.None, "")]
	public GameSaveConfigCSharp config;

	[Export(PropertyHint.None, "")]
	public GameConfigSaveConfigCSharp gameConfig;

	private bool _saveLoaded;

	private bool _storageInitialized;

	private bool _coinBankSyncedFromSave;

	private bool _mainSaveWriteBlocked;

	private bool _configSaveWriteBlocked;

	public bool saveScheduled;

	public bool configDirty;

	public static GameSaveManager Instance;

	private static readonly Regex _safeCharRegex = new Regex("[^\\w\\-\\.]", RegexOptions.Compiled);

	private ProgressRecoverySession _progressRecovery;

	private static Json CheckInit => _checkInit ?? (_checkInit = GD.Load<Json>("res://Asset/Config/Check/CheckInit.json"));

	public static Json TOWER_DEFENSE_PACKET_INIT => _towerDefensePacketInit ?? (_towerDefensePacketInit = GD.Load<Json>("res://Asset/Config/Save/TowerDefensePacketInit.json"));

	public static Json FEATURE_INIT => _featureInit ?? (_featureInit = GD.Load<Json>("res://Asset/Config/Save/FeatureInit.json"));

	public static Json TUTORIAL_INIT => _tutorialInit ?? (_tutorialInit = GD.Load<Json>("res://Asset/Config/Save/TutorialInit.json"));

	public static Json LEVEL_INIT => _levelInit ?? (_levelInit = GD.Load<Json>("res://Asset/Config/Save/LevelInit.json"));

	public static Json KEY_INIT => _keyInit ?? (_keyInit = GD.Load<Json>("res://Asset/Config/Save/KeyInit.json"));

	public static Json CONFIG_INIT => _configInit ?? (_configInit = GD.Load<Json>("res://Asset/Config/Save/ConfigInit.json"));

	private static Dictionary TOWER_DEFENSE_PACKET_INIT_DICT => _towerDefensePacketInitDict ?? (_towerDefensePacketInitDict = TOWER_DEFENSE_PACKET_INIT.Data.AsGodotDictionary());

	private static Dictionary FEATURE_INIT_DICT => _featureInitDict ?? (_featureInitDict = FEATURE_INIT.Data.AsGodotDictionary());

	private static Dictionary TUTORIAL_INIT_DICT => _tutorialInitDict ?? (_tutorialInitDict = TUTORIAL_INIT.Data.AsGodotDictionary());

	private static Dictionary LEVEL_INIT_DICT => _levelInitDict ?? (_levelInitDict = LEVEL_INIT.Data.AsGodotDictionary());

	private static Dictionary KEY_INIT_DICT => _keyInitDict ?? (_keyInitDict = KEY_INIT.Data.AsGodotDictionary());

	private static Dictionary CONFIG_INIT_DICT => _configInitDict ?? (_configInitDict = CONFIG_INIT.Data.AsGodotDictionary());

	public string savePath
	{
		get
		{
			if (Global.Instance == null || !Global.Instance.debug)
			{
				return "user://Csharp/save.res";
			}
			return "user://Csharp/Debug/save.res";
		}
	}

	public string configPath
	{
		get
		{
			if (Global.Instance == null || !Global.Instance.debug)
			{
				return "user://Csharp/config.res";
			}
			return "user://Csharp/Debug/config.res";
		}
	}

	public string dailyLevelPath
	{
		get
		{
			if (Global.Instance == null || !Global.Instance.debug)
			{
				return "user://Csharp/DailyLevel";
			}
			return "user://Csharp/Debug/DailyLevel";
		}
	}

	public string onlineLevelPath
	{
		get
		{
			if (Global.Instance == null || !Global.Instance.debug)
			{
				return "user://Csharp/OnlineLevel";
			}
			return "user://Csharp/Debug/OnlineLevel";
		}
	}

	public event Action<string, Variant> ConfigValueChanged;

	public event Action<string, int, int> FeatureValueChanged;

	public event Action<string> UserChanged;

	public override void _Ready()
	{
		Instance = this;
		SetProcess(enable: false);
		config = new GameSaveConfigCSharp();
		gameConfig = new GameConfigSaveConfigCSharp();
		if (!ShouldDeferStartupDataLoad())
		{
			InitializeStorageAndLoad();
		}
	}

	private static bool ShouldDeferStartupDataLoad()
	{
		string[] cmdlineArgs = OS.GetCmdlineArgs();
		for (int i = 0; i < cmdlineArgs.Length; i++)
		{
			string text = cmdlineArgs[i]?.Replace('\\', '/');
			if (!string.IsNullOrEmpty(text) && text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) && text.EndsWith(".tscn", StringComparison.OrdinalIgnoreCase))
			{
				return IsLoadingScenePath(text);
			}
		}
		return IsLoadingScenePath(ProjectSettings.GetSetting("application/run/main_scene", "").AsString());
	}

	private static bool IsLoadingScenePath(string scenePath)
	{
		if (string.IsNullOrWhiteSpace(scenePath))
		{
			return false;
		}
		string text = scenePath.Replace('\\', '/');
		if (text.StartsWith("uid://", StringComparison.OrdinalIgnoreCase))
		{
			long num = ResourceUid.TextToId(text);
			if (num != -1 && ResourceUid.HasId(num))
			{
				text = ResourceUid.GetIdPath(num);
			}
		}
		return string.Equals(text, "res://Scene/Loading/Loading.tscn", StringComparison.OrdinalIgnoreCase);
	}

	private void InitializeStorageAndLoad()
	{
		if (_storageInitialized)
		{
			return;
		}
		_storageInitialized = true;
		if (!DirAccess.DirExistsAbsolute(dailyLevelPath))
		{
			DirAccess.MakeDirRecursiveAbsolute(dailyLevelPath);
		}
		if (!DirAccess.DirExistsAbsolute(onlineLevelPath))
		{
			DirAccess.MakeDirRecursiveAbsolute(onlineLevelPath);
		}
		if (!Godot.FileAccess.FileExists(configPath))
		{
			gameConfig = new GameConfigSaveConfigCSharp();
		}
		else
		{
			ZLoggerErrorInterpolatedStringHandler message;
			bool enabled;
			try
			{
				if (ResourceLoader.Load(configPath, "", ResourceLoader.CacheMode.Ignore) is GameConfigSaveConfigCSharp gameConfigSaveConfigCSharp)
				{
					gameConfig = gameConfigSaveConfigCSharp;
				}
				else
				{
					_configSaveWriteBlocked = true;
					gameConfig = new GameConfigSaveConfigCSharp();
					ILogger logger = _logger;
					ILogger logger2 = logger;
					message = new ZLoggerErrorInterpolatedStringHandler(27, 1, logger, out enabled);
					if (enabled)
					{
						message.AppendLiteral("配置存档类型无效或加载失败，已保留原文件并阻止覆盖: ");
						message.AppendFormatted(configPath, 0, null, "configPath");
					}
					logger2.ZLogError(ref message);
				}
			}
			catch (Exception ex)
			{
				_configSaveWriteBlocked = true;
				gameConfig = new GameConfigSaveConfigCSharp();
				ILogger logger = _logger;
				ILogger logger3 = logger;
				message = new ZLoggerErrorInterpolatedStringHandler(22, 1, logger, out enabled);
				if (enabled)
				{
					message.AppendLiteral("配置存档加载异常，已保留原文件并阻止覆盖: ");
					message.AppendFormatted(ex.Message, 0, null, "e.Message");
				}
				logger3.ZLogError(ref message);
			}
		}
		if (Global.Instance != null && !Global.Instance.isMobile)
		{
			if (GetConfigValue("FullScreen").AsBool())
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			}
			else
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			}
		}
		if (Global.Instance != null)
		{
			Global.Instance.animeFrameRate = GetConfigValue("AnimeFrameRate").AsDouble();
			Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackendPolicy.Normalize(GetConfigValue("AdobeAnimateRenderBackend").AsInt32());
		}
		if (AudioManager.Instance != null)
		{
			AudioManager.Instance.VolumSet(AudioManagerEnum.TYPE.MUSIC, GetConfigValue("MusicVolum").AsDouble());
			AudioManager.Instance.VolumSet(AudioManagerEnum.TYPE.SFX, GetConfigValue("SfxVolum").AsDouble());
		}
		Load();
	}

	public override void _Process(double delta)
	{
		if (configDirty)
		{
			configDirty = false;
			SaveGameConfig();
		}
		if (!configDirty)
		{
			SetProcess(enable: false);
		}
	}

	public void Check()
	{
		Json checkInit = CheckInit;
		if (checkInit == null)
		{
			return;
		}
		foreach (Variant value in checkInit.Data.AsGodotDictionary().Values)
		{
			Dictionary dictionary = value.AsGodotDictionary();
			if (CheckCondition(dictionary["CheckType"].AsString(), dictionary["CheckKey"].AsString()))
			{
				ApplySave(dictionary["SaveType"].AsString(), dictionary["SaveKey"].AsString());
			}
		}
	}

	public bool CheckCondition(string type, string key)
	{
		return type switch
		{
			"packet" => GetTowerDefensePacketValue(key)["Unlock"].AsBool(), 
			"Feature" => GetFeatureValue(key) > 0, 
			"Tutorial" => GetTutorialValue(key), 
			"Level" => GetLevelValue(key)["Key"].AsGodotDictionary()["Finish"].AsInt32() > 0, 
			"Key" => GetKeyValue(key).AsBool(), 
			"config" => GetConfigValue(key).AsBool(), 
			_ => false, 
		};
	}

	public void ApplySave(string type, string key)
	{
		switch (type)
		{
		case "packet":
		{
			Dictionary towerDefensePacketValue = GetTowerDefensePacketValue(key);
			if (!towerDefensePacketValue["Unlock"].AsBool())
			{
				towerDefensePacketValue["Unlock"] = true;
			}
			SetTowerDefensePacketValue(key, towerDefensePacketValue);
			break;
		}
		case "Feature":
			SetFeatureValue(key, true);
			break;
		case "Tutorial":
			SetTutorialValue(key, value: true);
			break;
		case "Level":
		{
			Dictionary levelValue = GetLevelValue(key);
			if (levelValue["Key"].AsGodotDictionary()["Finish"].AsInt32() <= 0)
			{
				Dictionary dictionary = levelValue["Key"].AsGodotDictionary();
				dictionary["Finish"] = dictionary["Finish"].AsInt32() + 1;
			}
			SetLevelValue(key, levelValue);
			break;
		}
		case "Key":
			SetKeyValue(key, true);
			break;
		case "config":
			SetConfigValue(key, true);
			break;
		}
	}

	public void Save()
	{
		if (!_storageInitialized)
		{
			EnsureLoaded();
		}
		if (config == null)
		{
			Load();
		}
		if (GetUserCurrent() != "")
		{
			Check();
		}
		SaveCoinBankIfReady();
		Error error = (_mainSaveWriteBlocked ? Error.Unauthorized : SaveResourceAtomically(config, savePath));
		bool enabled;
		if (error != Error.Ok)
		{
			ILogger logger = _logger;
			ILogger logger2 = logger;
			ZLoggerErrorInterpolatedStringHandler message = new ZLoggerErrorInterpolatedStringHandler(15, 2, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("主存档保存失败: ");
				message.AppendFormatted(error, 0, null, "err");
				message.AppendLiteral(" path=");
				message.AppendFormatted(savePath, 0, null, "savePath");
			}
			logger2.ZLogError(ref message);
		}
		else
		{
			ILogger logger = _logger;
			ILogger logger3 = logger;
			ZLoggerInformationInterpolatedStringHandler message2 = new ZLoggerInformationInterpolatedStringHandler(8, 1, logger, out enabled);
			if (enabled)
			{
				message2.AppendLiteral("主存档已保存到 ");
				message2.AppendFormatted(savePath, 0, null, "savePath");
			}
			logger3.ZLogInformation(ref message2);
		}
		SaveGameConfig();
	}

	public void SaveGameConfig()
	{
		Error error = (_configSaveWriteBlocked ? Error.Unauthorized : SaveResourceAtomically(gameConfig, configPath));
		if (error != Error.Ok)
		{
			ILogger logger = _logger;
			ZLoggerErrorInterpolatedStringHandler message = new ZLoggerErrorInterpolatedStringHandler(16, 2, logger, out var enabled);
			if (enabled)
			{
				message.AppendLiteral("配置存档保存失败: ");
				message.AppendFormatted(error, 0, null, "err");
				message.AppendLiteral(" path=");
				message.AppendFormatted(configPath, 0, null, "configPath");
			}
			logger.ZLogError(ref message);
		}
	}

	internal Error SaveResourceAtomically(Resource resource, string targetPath)
	{
		if (resource == null || string.IsNullOrWhiteSpace(targetPath))
		{
			return Error.InvalidParameter;
		}
		string baseDir = targetPath.GetBaseDir();
		if (!DirAccess.DirExistsAbsolute(baseDir))
		{
			Error error = DirAccess.MakeDirRecursiveAbsolute(baseDir);
			if (error != Error.Ok)
			{
				return error;
			}
		}
		string extension = targetPath.GetExtension();
		string text = targetPath.TrimSuffix("." + extension) + ".tmp." + extension;
		string text2 = ProjectSettings.GlobalizePath(text);
		string destFileName = ProjectSettings.GlobalizePath(targetPath);
		bool enabled;
		try
		{
			if (File.Exists(text2))
			{
				File.Delete(text2);
			}
			Error error2 = ResourceSaver.Save(resource, text, ResourceSaver.SaverFlags.None);
			if (error2 != Error.Ok)
			{
				return error2;
			}
			File.Move(text2, destFileName, overwrite: true);
			return Error.Ok;
		}
		catch (Exception ex)
		{
			ILogger logger = _logger;
			ILogger logger2 = logger;
			ZLoggerErrorInterpolatedStringHandler message = new ZLoggerErrorInterpolatedStringHandler(16, 2, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("原子存档替换失败: ");
				message.AppendFormatted(ex.Message, 0, null, "exception.Message");
				message.AppendLiteral(" path=");
				message.AppendFormatted(targetPath, 0, null, "targetPath");
			}
			logger2.ZLogError(ref message);
			return Error.CantCreate;
		}
		finally
		{
			try
			{
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
			}
			catch (Exception ex2)
			{
				ILogger logger = _logger;
				ILogger logger3 = logger;
				ZLoggerWarningInterpolatedStringHandler message2 = new ZLoggerWarningInterpolatedStringHandler(16, 2, logger, out enabled);
				if (enabled)
				{
					message2.AppendLiteral("临时存档清理失败: ");
					message2.AppendFormatted(ex2.Message, 0, null, "exception.Message");
					message2.AppendLiteral(" path=");
					message2.AppendFormatted(text, 0, null, "temporaryPath");
				}
				logger3.ZLogWarning(ref message2);
			}
		}
	}

	public void ScheduleSave()
	{
		if (!saveScheduled)
		{
			saveScheduled = true;
			CallDeferred(MethodName.DeferredSave);
		}
	}

	public void DeferredSave()
	{
		saveScheduled = false;
		Save();
	}

	public void Load()
	{
		_coinBankSyncedFromSave = false;
		ZLoggerErrorInterpolatedStringHandler message;
		bool enabled;
		try
		{
			if (!Godot.FileAccess.FileExists(savePath))
			{
				config = new GameSaveConfigCSharp();
				Save();
			}
			else if (ResourceLoader.Load(savePath, "", ResourceLoader.CacheMode.Ignore) is GameSaveConfigCSharp gameSaveConfigCSharp)
			{
				config = gameSaveConfigCSharp;
			}
			else
			{
				_mainSaveWriteBlocked = true;
				config = new GameSaveConfigCSharp();
				ILogger logger = _logger;
				ILogger logger2 = logger;
				message = new ZLoggerErrorInterpolatedStringHandler(26, 1, logger, out enabled);
				if (enabled)
				{
					message.AppendLiteral("主存档类型无效或加载失败，已保留原文件并阻止覆盖: ");
					message.AppendFormatted(savePath, 0, null, "savePath");
				}
				logger2.ZLogError(ref message);
			}
		}
		catch (Exception ex)
		{
			_mainSaveWriteBlocked = true;
			config = new GameSaveConfigCSharp();
			ILogger logger = _logger;
			ILogger logger3 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(22, 2, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("主存档加载异常，已保留原文件并阻止覆盖: ");
				message.AppendFormatted(ex.Message, 0, null, "e.Message");
				message.AppendLiteral("\n");
				message.AppendFormatted(ex.StackTrace, 0, null, "e.StackTrace");
			}
			logger3.ZLogError(ref message);
		}
		SyncCoinBankFromSave();
		if (config.userCurrent != "" && GetKeyValue("CrystalNum").AsInt32() < 0)
		{
			RefreshCrystalNum();
		}
		if (GetUserCurrent() != "")
		{
			Check();
		}
		_saveLoaded = true;
		UserChanged?.Invoke(GetUserCurrent());
	}

	public void SyncCoinBankFromSave()
	{
		if (config != null && !(config.userCurrent == "") && TowerDefenseManager.Instance != null && TowerDefenseManager.Instance.coinBank != null)
		{
			TowerDefenseManager.Instance.coinBank.num = GetKeyValue("CoinNum").AsInt64();
			_coinBankSyncedFromSave = true;
		}
	}

	private void SaveCoinBankIfReady()
	{
		if (!(GetUserCurrent() == "") && TowerDefenseManager.Instance != null && TowerDefenseManager.Instance.coinBank != null)
		{
			if (!_coinBankSyncedFromSave)
			{
				SyncCoinBankFromSave();
			}
			if (_coinBankSyncedFromSave)
			{
				SetKeyValue("CoinNum", TowerDefenseManager.Instance.coinBank.num);
			}
		}
	}

	public void SyncCoinBankForExport()
	{
		if (!_storageInitialized)
		{
			EnsureLoaded();
		}
		if (config == null)
		{
			Load();
		}
		SaveCoinBankIfReady();
	}

	public void EnsureLoaded()
	{
		if (!_saveLoaded)
		{
			InitializeStorageAndLoad();
		}
	}

	public string EnsureUser()
	{
		return GetUserCurrent();
	}

	public string GetUserCurrent()
	{
		if (config != null)
		{
			return config.userCurrent;
		}
		return "";
	}

	public void SetUserCurrent(string user)
	{
		string userCurrent = GetUserCurrent();
		if (!config.userList.Contains(user))
		{
			AddUser(user);
			Save();
		}
		config.userCurrent = user;
		_coinBankSyncedFromSave = false;
		SyncCoinBankFromSave();
		if (GetKeyValue("CrystalNum").AsInt32() < 0)
		{
			RefreshCrystalNum();
		}
		Save();
		if (userCurrent != user)
		{
			UserChanged?.Invoke(user);
		}
	}

	public Array<string> GetUserList()
	{
		return config.userList;
	}

	public bool HasUser(string user)
	{
		return config.userList.Contains(user);
	}

	public void AddUser(string user)
	{
		if (!HasUser(user))
		{
			config.InitUser(user);
		}
		Save();
	}

	public void RenameUser(string user, string newName)
	{
		if (HasUser(user))
		{
			config.RenameUser(user, newName);
		}
	}

	public void DeleteUser(string user)
	{
		if (HasUser(user))
		{
			config.DeleteUser(user);
			if (config.userList.Count > 0)
			{
				SetUserCurrent(config.userList[0]);
			}
			else
			{
				SetUserCurrent("");
			}
		}
		Save();
	}

	public Dictionary GetUserDictionary(string user)
	{
		if (!config.userList.Contains(user))
		{
			return new Dictionary();
		}
		return config.saveDictionary[user].AsGodotDictionary();
	}

	public Dictionary GetCategoryDictionary(string category)
	{
		string userCurrent = GetUserCurrent();
		if (userCurrent == "")
		{
			return new Dictionary();
		}
		Dictionary dictionary = config.saveDictionary[userCurrent].AsGodotDictionary();
		if (!dictionary.ContainsKey(category))
		{
			config.Call(ZString.Concat(category, "DictionaryInit"), dictionary);
		}
		return dictionary[category].AsGodotDictionary();
	}

	public Variant GetCategoryValue(string category, string key, Dictionary initData)
	{
		if (EnsureUser() == "")
		{
			return false;
		}
		Dictionary categoryDictionary = GetCategoryDictionary(category);
		if (!categoryDictionary.ContainsKey(key))
		{
			categoryDictionary[key] = initData[key];
		}
		return categoryDictionary[key];
	}

	public void SetCategoryValue(string category, string key, Variant value, Dictionary initData)
	{
		if (!(EnsureUser() == ""))
		{
			Dictionary categoryDictionary = GetCategoryDictionary(category);
			if (!categoryDictionary.ContainsKey(key))
			{
				categoryDictionary[key] = initData[key];
			}
			categoryDictionary[key] = value;
		}
	}

	public Dictionary GetTowerDefensePacketDictionary()
	{
		return GetCategoryDictionary("TowerDefensePacket");
	}

	public Dictionary GetTowerDefensePacketValue(string key)
	{
		if (EnsureUser() == "")
		{
			return new Dictionary();
		}
		Dictionary towerDefensePacketDictionary = GetTowerDefensePacketDictionary();
		if (!towerDefensePacketDictionary.ContainsKey(key))
		{
			towerDefensePacketDictionary[key] = CreateTowerDefensePacketDefaultValue(key);
		}
		return towerDefensePacketDictionary[key].AsGodotDictionary();
	}

	public void SetTowerDefensePacketValue(string key, Dictionary value)
	{
		if (!(EnsureUser() == ""))
		{
			Dictionary towerDefensePacketDictionary = GetTowerDefensePacketDictionary();
			if (!towerDefensePacketDictionary.ContainsKey(key))
			{
				towerDefensePacketDictionary[key] = CreateTowerDefensePacketDefaultValue(key);
			}
			towerDefensePacketDictionary[key] = value;
		}
	}

	private Dictionary CreateTowerDefensePacketDefaultValue(string key)
	{
		bool unlock = TOWER_DEFENSE_PACKET_INIT_DICT.TryGetValue(key, out var value) && value.AsBool();
		if (!TOWER_DEFENSE_PACKET_INIT_DICT.ContainsKey(key) && _reportedMissingTowerDefensePacketInitKeys.Add(key ?? ""))
		{
			ILogger logger = _logger;
			ZLoggerWarningInterpolatedStringHandler message = new ZLoggerWarningInterpolatedStringHandler(85, 1, logger, out var enabled);
			if (enabled)
			{
				message.AppendLiteral("Tower-defense packet save key is absent from the init data; ");
				message.AppendLiteral("defaulting to locked: '");
				message.AppendFormatted(key ?? "<null>", 0, null, "key ?? \"<null>\"");
				message.AppendLiteral("'.");
			}
			logger.ZLogWarning(ref message);
		}
		return config.TowerDefensePacketDictionaryInitData(unlock);
	}

	public Dictionary GetFeatureDictionary()
	{
		return GetCategoryDictionary("Feature");
	}

	public int GetFeatureValue(string key)
	{
		Dictionary fEATURE_INIT_DICT = FEATURE_INIT_DICT;
		return GetCategoryValue("Feature", key, fEATURE_INIT_DICT).AsInt32();
	}

	public bool HasFeatureDefinition(string key)
	{
		if (!string.IsNullOrWhiteSpace(key))
		{
			return FEATURE_INIT_DICT.ContainsKey(key);
		}
		return false;
	}

	public void SetFeatureValue(string key, Variant value)
	{
		int featureValue = GetFeatureValue(key);
		Dictionary fEATURE_INIT_DICT = FEATURE_INIT_DICT;
		SetCategoryValue("Feature", key, value, fEATURE_INIT_DICT);
		int featureValue2 = GetFeatureValue(key);
		if (featureValue == featureValue2)
		{
			return;
		}
		try
		{
			FeatureValueChanged?.Invoke(key, featureValue, featureValue2);
		}
		catch (Exception value2)
		{
			GD.PushError($"FeatureValueChanged handler failed for '{key}': {value2}");
		}
	}

	public Dictionary GetTutorialDictionary()
	{
		return GetCategoryDictionary("Tutorial");
	}

	public bool GetTutorialValue(string key)
	{
		Dictionary tUTORIAL_INIT_DICT = TUTORIAL_INIT_DICT;
		return GetCategoryValue("Tutorial", key, tUTORIAL_INIT_DICT).AsBool();
	}

	public void SetTutorialValue(string key, bool value)
	{
		Dictionary tUTORIAL_INIT_DICT = TUTORIAL_INIT_DICT;
		SetCategoryValue("Tutorial", key, value, tUTORIAL_INIT_DICT);
	}

	public Dictionary GetLevelDictionary()
	{
		return GetCategoryDictionary("Level");
	}

	public Dictionary GetLevelValue(string key)
	{
		if (EnsureUser() == "" || key == "")
		{
			return new Dictionary();
		}
		Dictionary levelDictionary = GetLevelDictionary();
		if (!levelDictionary.ContainsKey(key))
		{
			Dictionary lEVEL_INIT_DICT = LEVEL_INIT_DICT;
			if (lEVEL_INIT_DICT.ContainsKey(key))
			{
				levelDictionary[key] = lEVEL_INIT_DICT[key];
			}
			else
			{
				levelDictionary[key] = new Dictionary();
			}
		}
		return levelDictionary[key].AsGodotDictionary();
	}

	public void SetLevelValue(string key, Dictionary value)
	{
		if (EnsureUser() == "")
		{
			return;
		}
		Dictionary levelDictionary = GetLevelDictionary();
		if (!levelDictionary.ContainsKey(key))
		{
			Dictionary lEVEL_INIT_DICT = LEVEL_INIT_DICT;
			if (lEVEL_INIT_DICT.ContainsKey(key))
			{
				levelDictionary[key] = lEVEL_INIT_DICT[key];
			}
			else
			{
				levelDictionary[key] = new Dictionary();
			}
		}
		levelDictionary[key] = value;
	}

	public Dictionary GetKeyDictionary()
	{
		return GetCategoryDictionary("Key");
	}

	public Variant GetKeyValue(string key)
	{
		Dictionary kEY_INIT_DICT = KEY_INIT_DICT;
		return GetCategoryValue("Key", key, kEY_INIT_DICT);
	}

	public void SetKeyValue(string key, Variant value)
	{
		Dictionary kEY_INIT_DICT = KEY_INIT_DICT;
		SetCategoryValue("Key", key, value, kEY_INIT_DICT);
	}

	public Dictionary GetConfigDictionary()
	{
		if (gameConfig.saveDictionary == null || gameConfig.saveDictionary.Count == 0)
		{
			gameConfig.Init();
		}
		return gameConfig.saveDictionary;
	}

	public Variant GetConfigValue(string key)
	{
		Dictionary configDictionary = GetConfigDictionary();
		if (!configDictionary.ContainsKey(key))
		{
			Dictionary cONFIG_INIT_DICT = CONFIG_INIT_DICT;
			configDictionary[key] = cONFIG_INIT_DICT[key];
		}
		return configDictionary[key];
	}

	public void SetConfigValue(string key, Variant value)
	{
		Dictionary configDictionary = GetConfigDictionary();
		if (!configDictionary.ContainsKey(key))
		{
			Dictionary cONFIG_INIT_DICT = CONFIG_INIT_DICT;
			configDictionary[key] = cONFIG_INIT_DICT[key];
		}
		if (configDictionary[key].Equals(value))
		{
			return;
		}
		configDictionary[key] = value;
		if (!configDirty)
		{
			configDirty = true;
			SetProcess(enable: true);
		}
		try
		{
			ConfigValueChanged?.Invoke(key, value);
		}
		catch (Exception value2)
		{
			GD.PushError($"ConfigValueChanged handler failed for '{key}': {value2}");
		}
	}

	public Json GetDailyLevel(string levelName)
	{
		string path = ZString.Concat(dailyLevelPath, "/", levelName, ".json");
		if (Godot.FileAccess.FileExists(path))
		{
			Json json = GD.Load<Json>(path);
			if (json != null && json.Data.VariantType != Variant.Type.Nil)
			{
				return json;
			}
		}
		return null;
	}

	public void SaveDailyLevel(string levelName, Json json)
	{
		Godot.FileAccess fileAccess = Godot.FileAccess.Open(ZString.Concat(dailyLevelPath, "/", levelName, ".json"), Godot.FileAccess.ModeFlags.Write);
		if (fileAccess != null)
		{
			fileAccess.StoreString(Json.Stringify(json.Data, "\t"));
			fileAccess.Close();
		}
	}

	public Json GetOnlineLevel(string levelName)
	{
		string path = ZString.Concat(onlineLevelPath, "/", levelName, ".json");
		if (Godot.FileAccess.FileExists(path))
		{
			Json json = GD.Load<Json>(path);
			if (json != null && json.Data.VariantType != Variant.Type.Nil)
			{
				return json;
			}
		}
		return null;
	}

	public void SaveOnlineLevel(string levelName, Json json)
	{
		Godot.FileAccess fileAccess = Godot.FileAccess.Open(ZString.Concat(onlineLevelPath, "/", levelName, ".json"), Godot.FileAccess.ModeFlags.Write);
		if (fileAccess != null)
		{
			fileAccess.StoreString(json.GetParsedText());
			fileAccess.Close();
		}
	}

	public bool CanUseLevelProgressInCurrentMode()
	{
		if (Global.Instance == null)
		{
			return true;
		}
		if (Global.Instance.enterLevelMode != "DiyLevel" && Global.Instance.enterLevelMode != "LoadLevel" && Global.Instance.enterLevelMode != "OnlineLevel")
		{
			return Global.Instance.enterLevelMode != "DailyLevel";
		}
		return false;
	}

	public void SaveLevelProgress(string levelName, XWModLevelIdentity identity = null)
	{
		ZLoggerInformationInterpolatedStringHandler message;
		bool enabled;
		if (string.IsNullOrEmpty(levelName) || GetUserCurrent() == "")
		{
			ILogger logger = _logger;
			ILogger logger2 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(51, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Skip level progress save: empty user or level name.");
			}
			logger2.ZLogInformation(ref message);
			return;
		}
		if (TowerDefenseManager.Instance == null || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentControl))
		{
			ILogger logger = _logger;
			ILogger logger3 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(50, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Skip level progress save: no active level control.");
			}
			logger3.ZLogInformation(ref message);
			return;
		}
		TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
		string reason;
		if (!currentControl.isGameRunning || currentControl.isGameFail)
		{
			ILogger logger = _logger;
			ILogger logger4 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(64, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Skip level progress save: game is not running or already failed.");
			}
			logger4.ZLogInformation(ref message);
		}
		else if (GodotObject.IsInstanceValid(currentControl.levelControl) && currentControl.levelControl.awardCreate)
		{
			ILogger logger = _logger;
			ILogger logger5 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(50, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Skip level progress save: level already completed.");
			}
			logger5.ZLogInformation(ref message);
		}
		else if (Global.IsMultiplayerMode)
		{
			ILogger logger = _logger;
			ILogger logger6 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(43, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Skip level progress save: multiplayer mode.");
			}
			logger6.ZLogInformation(ref message);
		}
		else if (!CanUseLevelProgressInCurrentMode())
		{
			ILogger logger = _logger;
			ILogger logger7 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(47, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Skip level progress save: external level mode ");
				message.AppendFormatted(Global.Instance?.enterLevelMode ?? "", 0, null, "Global.Instance?.enterLevelMode ?? \"\"");
				message.AppendLiteral(".");
			}
			logger7.ZLogInformation(ref message);
		}
		else if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentControl) || !TowerDefenseManager.Instance.currentControl.isGameRunning)
		{
			ILogger logger = _logger;
			ILogger logger8 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(12, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("游戏未在运行中，跳过保存");
			}
			logger8.ZLogInformation(ref message);
		}
		else if (TowerDefenseManager.GetGameMethod() == TowerDefenseEnum.LEVEL_FINISH_METHOD.QUIZ)
		{
			ILogger logger = _logger;
			ILogger logger9 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(14, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("QUIZ 模式不保存关卡进度");
			}
			logger9.ZLogInformation(ref message);
		}
		else if (!currentControl.CanCreateProgressSave(out reason))
		{
			ILogger logger = _logger;
			ILogger logger10 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(54, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Skip level progress save: unsafe battle checkpoint (");
				message.AppendFormatted(reason, 0, null, "unsafeReason");
				message.AppendLiteral(").");
			}
			logger10.ZLogInformation(ref message);
		}
		else
		{
			if (identity != null && identity.LevelSaveKey != levelName)
			{
				return;
			}
			string levelProgressPath = GetLevelProgressPath(levelName, identity);
			if (!CanWriteRecoveredProgress(levelProgressPath, currentControl))
			{
				return;
			}
			DirAccess.MakeDirRecursiveAbsolute(levelProgressPath.GetBaseDir());
			TowerDefenseLevelSaveConfigCSharp towerDefenseLevelSaveConfigCSharp = new TowerDefenseLevelSaveConfigCSharp();
			towerDefenseLevelSaveConfigCSharp.Save();
			if (identity != null)
			{
				towerDefenseLevelSaveConfigCSharp.SetMeta("mod_level_identity", identity.ToDictionary());
			}
			Error error = SaveResourceAtomically(towerDefenseLevelSaveConfigCSharp, levelProgressPath);
			if (error != Error.Ok)
			{
				ILogger logger = _logger;
				ILogger logger11 = logger;
				ZLoggerErrorInterpolatedStringHandler message2 = new ZLoggerErrorInterpolatedStringHandler(34, 2, logger, out enabled);
				if (enabled)
				{
					message2.AppendLiteral("Level progress save failed: ");
					message2.AppendFormatted(error, 0, null, "err");
					message2.AppendLiteral(" path=");
					message2.AppendFormatted(levelProgressPath, 0, null, "filePath");
				}
				logger11.ZLogError(ref message2);
			}
		}
	}

	public bool LoadLevelProgress(string levelName, XWModLevelIdentity identity = null)
	{
		if (!TryGetLoadableLevelProgress(levelName, out var saveConfig, out var _, identity))
		{
			return false;
		}
		bool flag = saveConfig.Load();
		if (flag)
		{
			AcceptProgressRecovery(TowerDefenseManager.Instance.currentControl);
		}
		return flag;
	}

	public bool TryGetLoadableLevelProgress(string levelName, out TowerDefenseLevelSaveConfigCSharp saveConfig, out string reason, XWModLevelIdentity identity = null)
	{
		return PrepareProgressRecovery(levelName, identity, out saveConfig, out reason);
	}

	private string GetLevelProgressPath(string levelName, XWModLevelIdentity identity)
	{
		if (!(identity == null))
		{
			return XWModPlayerProgressService.ProgressPath(identity);
		}
		return $"user://Csharp/Progress/{_GetUserCurrentSafe()}/{levelName}.tres";
	}

	public TowerDefenseLevelSaveConfigCSharp GetLevelProgress(string levelName, XWModLevelIdentity identity = null)
	{
		if (string.IsNullOrEmpty(levelName) || GetUserCurrent() == "")
		{
			return null;
		}
		if (identity != null && identity.LevelSaveKey != levelName)
		{
			return null;
		}
		if (!TryReadProgressFile(GetLevelProgressPath(levelName, identity), out var save, out var _))
		{
			return null;
		}
		return save;
	}

	public void DeleteLevelProgress(string levelName, XWModLevelIdentity identity = null)
	{
		if (string.IsNullOrEmpty(levelName) || GetUserCurrent() == "" || (identity != null && identity.LevelSaveKey != levelName))
		{
			return;
		}
		string levelProgressPath = GetLevelProgressPath(levelName, identity);
		ProgressRecoverySession progressRecovery = GetProgressRecovery(TowerDefenseManager.Instance?.currentControl);
		if (progressRecovery?.PrimaryPath == levelProgressPath && (progressRecovery.AwaitingDecision || progressRecovery.Discarding))
		{
			return;
		}
		try
		{
			File.Delete(ProjectSettings.GlobalizePath(ProgressBackupPath(levelProgressPath)));
			File.Delete(ProjectSettings.GlobalizePath(levelProgressPath));
		}
		catch (Exception ex)
		{
			GD.PushWarning("[ProgressLoad] 进度清理失败：" + ex.Message);
		}
	}

	public bool HasLevelProgress(string levelName, XWModLevelIdentity identity = null)
	{
		if (string.IsNullOrEmpty(levelName) || GetUserCurrent() == "" || (identity != null && identity.LevelSaveKey != levelName))
		{
			return false;
		}
		string levelProgressPath = GetLevelProgressPath(levelName, identity);
		if (!Godot.FileAccess.FileExists(levelProgressPath))
		{
			return Godot.FileAccess.FileExists(ProgressBackupPath(levelProgressPath));
		}
		return true;
	}

	private string _GetUserCurrentSafe()
	{
		return _safeCharRegex.Replace(GetUserCurrent(), "_");
	}

	public void RefreshCrystalNum()
	{
		Dictionary levelDictionary = GetLevelDictionary();
		int num = 0;
		foreach (Variant key in levelDictionary.Keys)
		{
			if (key.AsString().StartsWith("OnlineLevel"))
			{
				Dictionary dictionary = levelDictionary[key].AsGodotDictionary();
				Dictionary dictionary2 = (dictionary.ContainsKey("Key") ? dictionary["Key"].AsGodotDictionary() : new Dictionary());
				if (dictionary2.ContainsKey("Finish") && dictionary2["Finish"].AsInt32() > 0)
				{
					num++;
				}
			}
		}
		SetKeyValue("CrystalNum", num);
		Save();
	}

	public bool TryConvertSaveResource(Resource resource, out GameSaveConfigCSharp saveConfig)
	{
		saveConfig = null;
		if (!GodotObject.IsInstanceValid(resource))
		{
			return false;
		}
		if (resource is GameSaveConfigCSharp gameSaveConfigCSharp)
		{
			NormalizeImportedSave(gameSaveConfigCSharp);
			if (!HasUsableSaveData(gameSaveConfigCSharp))
			{
				return false;
			}
			saveConfig = gameSaveConfigCSharp;
			return true;
		}
		return false;
	}

	public bool ReplaceMainSave(GameSaveConfigCSharp saveConfig)
	{
		if (!HasUsableSaveData(saveConfig))
		{
			return false;
		}
		GameSaveConfigCSharp gameSaveConfigCSharp = config;
		bool mainSaveWriteBlocked = _mainSaveWriteBlocked;
		NormalizeImportedSave(saveConfig);
		config = saveConfig;
		_mainSaveWriteBlocked = false;
		Error error = SaveResourceAtomically(config, savePath);
		if (error == Error.Ok)
		{
			_coinBankSyncedFromSave = false;
			return true;
		}
		config = gameSaveConfigCSharp;
		_mainSaveWriteBlocked = mainSaveWriteBlocked;
		ILogger logger = _logger;
		ZLoggerErrorInterpolatedStringHandler message = new ZLoggerErrorInterpolatedStringHandler(16, 2, logger, out var enabled);
		if (enabled)
		{
			message.AppendLiteral("导入存档替换失败: ");
			message.AppendFormatted(error, 0, null, "saveError");
			message.AppendLiteral(" path=");
			message.AppendFormatted(savePath, 0, null, "savePath");
		}
		logger.ZLogError(ref message);
		return false;
	}

	private static bool HasUsableSaveData(GameSaveConfigCSharp saveConfig)
	{
		if (saveConfig != null && saveConfig.userList != null && saveConfig.saveDictionary != null && saveConfig.userList.Count > 0)
		{
			return saveConfig.saveDictionary.Count > 0;
		}
		return false;
	}

	private static void NormalizeImportedSave(GameSaveConfigCSharp saveConfig)
	{
		if (saveConfig.userList == null)
		{
			saveConfig.userList = new Array<string>();
		}
		if (saveConfig.saveDictionary == null)
		{
			saveConfig.saveDictionary = new Dictionary();
		}
		foreach (Variant key in saveConfig.saveDictionary.Keys)
		{
			string text = key.AsString();
			if (!string.IsNullOrEmpty(text) && !saveConfig.userList.Contains(text))
			{
				saveConfig.userList.Add(text);
			}
		}
		if (string.IsNullOrEmpty(saveConfig.userCurrent) && saveConfig.userList.Count > 0)
		{
			saveConfig.userCurrent = saveConfig.userList[0];
		}
		if (!string.IsNullOrEmpty(saveConfig.userCurrent) && !saveConfig.userList.Contains(saveConfig.userCurrent))
		{
			saveConfig.userList.Add(saveConfig.userCurrent);
		}
	}

	internal ProgressRecoverySession GetProgressRecovery(TowerDefenseControlNew control)
	{
		if (!GodotObject.IsInstanceValid(control) || _progressRecovery?.ControlId != control.GetInstanceId())
		{
			return null;
		}
		return _progressRecovery;
	}

	internal void AcceptProgressRecovery(TowerDefenseControlNew control)
	{
		ProgressRecoverySession progressRecovery = GetProgressRecovery(control);
		if (progressRecovery != null)
		{
			progressRecovery.AwaitingDecision = false;
		}
	}

	internal void DiscardProgressRecovery(TowerDefenseControlNew control)
	{
		ProgressRecoverySession progressRecovery = GetProgressRecovery(control);
		if (progressRecovery != null)
		{
			progressRecovery.Discarding = true;
		}
	}

	internal void EndProgressRecovery(TowerDefenseControlNew control)
	{
		if (GetProgressRecovery(control) != null)
		{
			_progressRecovery = null;
		}
	}

	internal static string ProgressBackupPath(string primary)
	{
		return primary.TrimSuffix(".tres") + ".pre-relaxed.tres";
	}

	private static bool TryReadProgressFile(string path, out TowerDefenseLevelSaveConfigCSharp save, out string reason)
	{
		save = null;
		reason = "关卡进度文件不存在。";
		if (!Godot.FileAccess.FileExists(path))
		{
			return false;
		}
		try
		{
			save = ResourceLoader.Load(path, "", ResourceLoader.CacheMode.Ignore) as TowerDefenseLevelSaveConfigCSharp;
			reason = ((save == null) ? "无法读取关卡进度资源。" : "");
			return GodotObject.IsInstanceValid(save);
		}
		catch (Exception ex)
		{
			reason = "读取关卡进度失败：" + ex.Message;
			return false;
		}
	}

	private static bool CopyProgressBytesAtomically(string source, string destination, bool overwrite, out string reason)
	{
		string text = null;
		try
		{
			string text2 = ProjectSettings.GlobalizePath(destination);
			Directory.CreateDirectory(Path.GetDirectoryName(text2));
			text = text2 + "." + Guid.NewGuid().ToString("N") + ".tmp";
			File.Copy(ProjectSettings.GlobalizePath(source), text, overwrite: false);
			File.Move(text, text2, overwrite);
			reason = "";
			return true;
		}
		catch (Exception ex)
		{
			reason = ex.Message;
			return false;
		}
		finally
		{
			if (text != null)
			{
				try
				{
					File.Delete(text);
				}
				catch (Exception ex2)
				{
					GD.PushWarning("[ProgressBackup] 临时文件清理失败：" + ex2.Message);
				}
			}
		}
	}

	private static bool EnsureProgressBackup(ProgressRecoverySession session)
	{
		if (Godot.FileAccess.FileExists(session.BackupPath))
		{
			session.BackupAvailable = TryReadProgressFile(session.BackupPath, out var _, out session.BackupError);
		}
		else
		{
			session.BackupAvailable = CopyProgressBytesAtomically(session.PrimaryPath, session.BackupPath, overwrite: false, out session.BackupError);
		}
		return session.BackupAvailable;
	}

	private bool CanWriteRecoveredProgress(string path, TowerDefenseControlNew control)
	{
		ProgressRecoverySession progressRecovery = GetProgressRecovery(control);
		if (progressRecovery == null || progressRecovery.PrimaryPath != path)
		{
			return true;
		}
		if (progressRecovery.AwaitingDecision || progressRecovery.Discarding)
		{
			return false;
		}
		if (!progressRecovery.BackupAvailable && !EnsureProgressBackup(progressRecovery))
		{
			GD.PushWarning("[ProgressBackup] 原档备份尚未完成，未覆盖关卡进度：" + progressRecovery.BackupError);
			return false;
		}
		return true;
	}

	internal bool TryRestoreProgressBackup(TowerDefenseControlNew control, out string reason)
	{
		ProgressRecoverySession progressRecovery = GetProgressRecovery(control);
		if (progressRecovery == null)
		{
			reason = "当前关卡没有可恢复的备份记录。";
			return false;
		}
		progressRecovery.AwaitingDecision = true;
		if (!TryReadProgressFile(progressRecovery.BackupPath, out var _, out reason))
		{
			progressRecovery.BackupAvailable = false;
			progressRecovery.BackupError = reason;
			return false;
		}
		if (!CopyProgressBytesAtomically(progressRecovery.BackupPath, progressRecovery.PrimaryPath, overwrite: true, out reason))
		{
			return false;
		}
		progressRecovery.Discarding = true;
		return true;
	}

	private bool PrepareProgressRecovery(string levelName, XWModLevelIdentity identity, out TowerDefenseLevelSaveConfigCSharp save, out string reason)
	{
		save = null;
		reason = "当前用户或关卡不可用。";
		if (string.IsNullOrEmpty(levelName) || GetUserCurrent() == "" || (identity != null && identity.LevelSaveKey != levelName))
		{
			return false;
		}
		string levelProgressPath = GetLevelProgressPath(levelName, identity);
		ProgressRecoverySession progressRecoverySession = new ProgressRecoverySession
		{
			PrimaryPath = levelProgressPath,
			BackupPath = ProgressBackupPath(levelProgressPath),
			ControlId = (TowerDefenseManager.Instance?.currentControl?.GetInstanceId()).GetValueOrDefault()
		};
		if (!TryReadProgressFile(levelProgressPath, out save, out var reason2))
		{
			if (!TryReadProgressFile(progressRecoverySession.BackupPath, out save, out var reason3))
			{
				reason = reason2 + " 备份：" + reason3;
				return false;
			}
			progressRecoverySession.UsedBackup = true;
			progressRecoverySession.BackupAvailable = true;
		}
		if (!save.CanLoad(out reason))
		{
			return false;
		}
		if (!progressRecoverySession.UsedBackup && !EnsureProgressBackup(progressRecoverySession))
		{
			GD.PushWarning("[ProgressBackup] 继续读取，但暂不覆盖原档：" + progressRecoverySession.BackupError);
		}
		_progressRecovery = progressRecoverySession;
		return true;
	}

	public GameSaveManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/GameSaveManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(65)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldDeferStartupDataLoad, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.IsLoadingScenePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "scenePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeStorageAndLoad, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckCondition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Save, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveGameConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveResourceAtomically, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "targetPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScheduleSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeferredSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Load, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncCoinBankFromSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveCoinBankIfReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncCoinBankForExport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureLoaded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureUser, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetUserCurrent, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetUserCurrent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetUserList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasUser, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddUser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenameUser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DeleteUser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetUserDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCategoryDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCategoryValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "initData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCategoryValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "initData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTowerDefensePacketDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetTowerDefensePacketValue, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetTowerDefensePacketValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateTowerDefensePacketDefaultValue, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFeatureDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFeatureValue, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasFeatureDefinition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFeatureValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTutorialDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetTutorialValue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetTutorialValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLevelDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLevelValue, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetLevelValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetKeyDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetKeyValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetKeyValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetConfigDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetConfigValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetConfigValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDailyLevel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveDailyLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "json", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetOnlineLevel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveOnlineLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "json", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanUseLevelProgressInCurrentMode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetUserCurrentSafe, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCrystalNum, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReplaceMainSave, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "saveConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasUsableSaveData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "saveConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeImportedSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "saveConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AcceptProgressRecovery, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.DiscardProgressRecovery, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.EndProgressRecovery, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ProgressBackupPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "primary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanWriteRecoveredProgress, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.ShouldDeferStartupDataLoad && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldDeferStartupDataLoad());
			return true;
		}
		if (method == MethodName.IsLoadingScenePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLoadingScenePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.InitializeStorageAndLoad && args.Count == 0)
		{
			InitializeStorageAndLoad();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 0)
		{
			Check();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckCondition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckCondition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplySave && args.Count == 2)
		{
			ApplySave(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Save && args.Count == 0)
		{
			Save();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveGameConfig && args.Count == 0)
		{
			SaveGameConfig();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveResourceAtomically && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Error>(SaveResourceAtomically(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ScheduleSave && args.Count == 0)
		{
			ScheduleSave();
			ret = default;
			return true;
		}
		if (method == MethodName.DeferredSave && args.Count == 0)
		{
			DeferredSave();
			ret = default;
			return true;
		}
		if (method == MethodName.Load && args.Count == 0)
		{
			Load();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncCoinBankFromSave && args.Count == 0)
		{
			SyncCoinBankFromSave();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveCoinBankIfReady && args.Count == 0)
		{
			SaveCoinBankIfReady();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncCoinBankForExport && args.Count == 0)
		{
			SyncCoinBankForExport();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureLoaded && args.Count == 0)
		{
			EnsureLoaded();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureUser && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(EnsureUser());
			return true;
		}
		if (method == MethodName.GetUserCurrent && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetUserCurrent());
			return true;
		}
		if (method == MethodName.SetUserCurrent && args.Count == 1)
		{
			SetUserCurrent(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetUserList && args.Count == 0)
		{
			Array<string> userList = GetUserList();
			ret = VariantUtils.CreateFromArray(userList);
			return true;
		}
		if (method == MethodName.HasUser && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUser(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AddUser && args.Count == 1)
		{
			AddUser(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenameUser && args.Count == 2)
		{
			RenameUser(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteUser && args.Count == 1)
		{
			DeleteUser(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetUserDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetUserDictionary(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCategoryDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetCategoryDictionary(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCategoryValue && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetCategoryValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Dictionary>(in args[2])));
			return true;
		}
		if (method == MethodName.SetCategoryValue && args.Count == 4)
		{
			SetCategoryValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]), VariantUtils.ConvertTo<Dictionary>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetTowerDefensePacketDictionary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetTowerDefensePacketDictionary());
			return true;
		}
		if (method == MethodName.GetTowerDefensePacketValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetTowerDefensePacketValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetTowerDefensePacketValue && args.Count == 2)
		{
			SetTowerDefensePacketValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateTowerDefensePacketDefaultValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateTowerDefensePacketDefaultValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFeatureDictionary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetFeatureDictionary());
			return true;
		}
		if (method == MethodName.GetFeatureValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetFeatureValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasFeatureDefinition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFeatureDefinition(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetFeatureValue && args.Count == 2)
		{
			SetFeatureValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetTutorialDictionary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetTutorialDictionary());
			return true;
		}
		if (method == MethodName.GetTutorialValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(GetTutorialValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetTutorialValue && args.Count == 2)
		{
			SetTutorialValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetLevelDictionary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetLevelDictionary());
			return true;
		}
		if (method == MethodName.GetLevelValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetLevelValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetLevelValue && args.Count == 2)
		{
			SetLevelValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetKeyDictionary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetKeyDictionary());
			return true;
		}
		if (method == MethodName.GetKeyValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetKeyValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetKeyValue && args.Count == 2)
		{
			SetKeyValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetConfigDictionary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetConfigDictionary());
			return true;
		}
		if (method == MethodName.GetConfigValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetConfigValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetConfigValue && args.Count == 2)
		{
			SetConfigValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDailyLevel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Json>(GetDailyLevel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SaveDailyLevel && args.Count == 2)
		{
			SaveDailyLevel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Json>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetOnlineLevel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Json>(GetOnlineLevel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SaveOnlineLevel && args.Count == 2)
		{
			SaveOnlineLevel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Json>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanUseLevelProgressInCurrentMode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanUseLevelProgressInCurrentMode());
			return true;
		}
		if (method == MethodName._GetUserCurrentSafe && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetUserCurrentSafe());
			return true;
		}
		if (method == MethodName.RefreshCrystalNum && args.Count == 0)
		{
			RefreshCrystalNum();
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceMainSave && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReplaceMainSave(VariantUtils.ConvertTo<GameSaveConfigCSharp>(in args[0])));
			return true;
		}
		if (method == MethodName.HasUsableSaveData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUsableSaveData(VariantUtils.ConvertTo<GameSaveConfigCSharp>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeImportedSave && args.Count == 1)
		{
			NormalizeImportedSave(VariantUtils.ConvertTo<GameSaveConfigCSharp>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AcceptProgressRecovery && args.Count == 1)
		{
			AcceptProgressRecovery(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DiscardProgressRecovery && args.Count == 1)
		{
			DiscardProgressRecovery(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EndProgressRecovery && args.Count == 1)
		{
			EndProgressRecovery(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProgressBackupPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ProgressBackupPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanWriteRecoveredProgress && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanWriteRecoveredProgress(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ShouldDeferStartupDataLoad && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldDeferStartupDataLoad());
			return true;
		}
		if (method == MethodName.IsLoadingScenePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLoadingScenePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasUsableSaveData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUsableSaveData(VariantUtils.ConvertTo<GameSaveConfigCSharp>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeImportedSave && args.Count == 1)
		{
			NormalizeImportedSave(VariantUtils.ConvertTo<GameSaveConfigCSharp>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProgressBackupPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ProgressBackupPath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.ShouldDeferStartupDataLoad)
		{
			return true;
		}
		if (method == MethodName.IsLoadingScenePath)
		{
			return true;
		}
		if (method == MethodName.InitializeStorageAndLoad)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.CheckCondition)
		{
			return true;
		}
		if (method == MethodName.ApplySave)
		{
			return true;
		}
		if (method == MethodName.Save)
		{
			return true;
		}
		if (method == MethodName.SaveGameConfig)
		{
			return true;
		}
		if (method == MethodName.SaveResourceAtomically)
		{
			return true;
		}
		if (method == MethodName.ScheduleSave)
		{
			return true;
		}
		if (method == MethodName.DeferredSave)
		{
			return true;
		}
		if (method == MethodName.Load)
		{
			return true;
		}
		if (method == MethodName.SyncCoinBankFromSave)
		{
			return true;
		}
		if (method == MethodName.SaveCoinBankIfReady)
		{
			return true;
		}
		if (method == MethodName.SyncCoinBankForExport)
		{
			return true;
		}
		if (method == MethodName.EnsureLoaded)
		{
			return true;
		}
		if (method == MethodName.EnsureUser)
		{
			return true;
		}
		if (method == MethodName.GetUserCurrent)
		{
			return true;
		}
		if (method == MethodName.SetUserCurrent)
		{
			return true;
		}
		if (method == MethodName.GetUserList)
		{
			return true;
		}
		if (method == MethodName.HasUser)
		{
			return true;
		}
		if (method == MethodName.AddUser)
		{
			return true;
		}
		if (method == MethodName.RenameUser)
		{
			return true;
		}
		if (method == MethodName.DeleteUser)
		{
			return true;
		}
		if (method == MethodName.GetUserDictionary)
		{
			return true;
		}
		if (method == MethodName.GetCategoryDictionary)
		{
			return true;
		}
		if (method == MethodName.GetCategoryValue)
		{
			return true;
		}
		if (method == MethodName.SetCategoryValue)
		{
			return true;
		}
		if (method == MethodName.GetTowerDefensePacketDictionary)
		{
			return true;
		}
		if (method == MethodName.GetTowerDefensePacketValue)
		{
			return true;
		}
		if (method == MethodName.SetTowerDefensePacketValue)
		{
			return true;
		}
		if (method == MethodName.CreateTowerDefensePacketDefaultValue)
		{
			return true;
		}
		if (method == MethodName.GetFeatureDictionary)
		{
			return true;
		}
		if (method == MethodName.GetFeatureValue)
		{
			return true;
		}
		if (method == MethodName.HasFeatureDefinition)
		{
			return true;
		}
		if (method == MethodName.SetFeatureValue)
		{
			return true;
		}
		if (method == MethodName.GetTutorialDictionary)
		{
			return true;
		}
		if (method == MethodName.GetTutorialValue)
		{
			return true;
		}
		if (method == MethodName.SetTutorialValue)
		{
			return true;
		}
		if (method == MethodName.GetLevelDictionary)
		{
			return true;
		}
		if (method == MethodName.GetLevelValue)
		{
			return true;
		}
		if (method == MethodName.SetLevelValue)
		{
			return true;
		}
		if (method == MethodName.GetKeyDictionary)
		{
			return true;
		}
		if (method == MethodName.GetKeyValue)
		{
			return true;
		}
		if (method == MethodName.SetKeyValue)
		{
			return true;
		}
		if (method == MethodName.GetConfigDictionary)
		{
			return true;
		}
		if (method == MethodName.GetConfigValue)
		{
			return true;
		}
		if (method == MethodName.SetConfigValue)
		{
			return true;
		}
		if (method == MethodName.GetDailyLevel)
		{
			return true;
		}
		if (method == MethodName.SaveDailyLevel)
		{
			return true;
		}
		if (method == MethodName.GetOnlineLevel)
		{
			return true;
		}
		if (method == MethodName.SaveOnlineLevel)
		{
			return true;
		}
		if (method == MethodName.CanUseLevelProgressInCurrentMode)
		{
			return true;
		}
		if (method == MethodName._GetUserCurrentSafe)
		{
			return true;
		}
		if (method == MethodName.RefreshCrystalNum)
		{
			return true;
		}
		if (method == MethodName.ReplaceMainSave)
		{
			return true;
		}
		if (method == MethodName.HasUsableSaveData)
		{
			return true;
		}
		if (method == MethodName.NormalizeImportedSave)
		{
			return true;
		}
		if (method == MethodName.AcceptProgressRecovery)
		{
			return true;
		}
		if (method == MethodName.DiscardProgressRecovery)
		{
			return true;
		}
		if (method == MethodName.EndProgressRecovery)
		{
			return true;
		}
		if (method == MethodName.ProgressBackupPath)
		{
			return true;
		}
		if (method == MethodName.CanWriteRecoveredProgress)
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
			config = VariantUtils.ConvertTo<GameSaveConfigCSharp>(in value);
			return true;
		}
		if (name == PropertyName.gameConfig)
		{
			gameConfig = VariantUtils.ConvertTo<GameConfigSaveConfigCSharp>(in value);
			return true;
		}
		if (name == PropertyName._saveLoaded)
		{
			_saveLoaded = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._storageInitialized)
		{
			_storageInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._coinBankSyncedFromSave)
		{
			_coinBankSyncedFromSave = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mainSaveWriteBlocked)
		{
			_mainSaveWriteBlocked = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._configSaveWriteBlocked)
		{
			_configSaveWriteBlocked = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.saveScheduled)
		{
			saveScheduled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.configDirty)
		{
			configDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.savePath)
		{
			from = savePath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.configPath)
		{
			from = configPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dailyLevelPath)
		{
			from = dailyLevelPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.onlineLevelPath)
		{
			from = onlineLevelPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.gameConfig)
		{
			value = VariantUtils.CreateFrom(in gameConfig);
			return true;
		}
		if (name == PropertyName._saveLoaded)
		{
			value = VariantUtils.CreateFrom(in _saveLoaded);
			return true;
		}
		if (name == PropertyName._storageInitialized)
		{
			value = VariantUtils.CreateFrom(in _storageInitialized);
			return true;
		}
		if (name == PropertyName._coinBankSyncedFromSave)
		{
			value = VariantUtils.CreateFrom(in _coinBankSyncedFromSave);
			return true;
		}
		if (name == PropertyName._mainSaveWriteBlocked)
		{
			value = VariantUtils.CreateFrom(in _mainSaveWriteBlocked);
			return true;
		}
		if (name == PropertyName._configSaveWriteBlocked)
		{
			value = VariantUtils.CreateFrom(in _configSaveWriteBlocked);
			return true;
		}
		if (name == PropertyName.saveScheduled)
		{
			value = VariantUtils.CreateFrom(in saveScheduled);
			return true;
		}
		if (name == PropertyName.configDirty)
		{
			value = VariantUtils.CreateFrom(in configDirty);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.ResourceType, "GameSaveConfigCSharp", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.gameConfig, PropertyHint.ResourceType, "GameConfigSaveConfigCSharp", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._saveLoaded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._storageInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._coinBankSyncedFromSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mainSaveWriteBlocked, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._configSaveWriteBlocked, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.saveScheduled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.configDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.savePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.configPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.dailyLevelPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.onlineLevelPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.gameConfig, Variant.From(in gameConfig));
		info.AddProperty(PropertyName._saveLoaded, Variant.From(in _saveLoaded));
		info.AddProperty(PropertyName._storageInitialized, Variant.From(in _storageInitialized));
		info.AddProperty(PropertyName._coinBankSyncedFromSave, Variant.From(in _coinBankSyncedFromSave));
		info.AddProperty(PropertyName._mainSaveWriteBlocked, Variant.From(in _mainSaveWriteBlocked));
		info.AddProperty(PropertyName._configSaveWriteBlocked, Variant.From(in _configSaveWriteBlocked));
		info.AddProperty(PropertyName.saveScheduled, Variant.From(in saveScheduled));
		info.AddProperty(PropertyName.configDirty, Variant.From(in configDirty));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.config, out var value))
		{
			config = value.As<GameSaveConfigCSharp>();
		}
		if (info.TryGetProperty(PropertyName.gameConfig, out var value2))
		{
			gameConfig = value2.As<GameConfigSaveConfigCSharp>();
		}
		if (info.TryGetProperty(PropertyName._saveLoaded, out var value3))
		{
			_saveLoaded = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._storageInitialized, out var value4))
		{
			_storageInitialized = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._coinBankSyncedFromSave, out var value5))
		{
			_coinBankSyncedFromSave = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mainSaveWriteBlocked, out var value6))
		{
			_mainSaveWriteBlocked = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._configSaveWriteBlocked, out var value7))
		{
			_configSaveWriteBlocked = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.saveScheduled, out var value8))
		{
			saveScheduled = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.configDirty, out var value9))
		{
			configDirty = value9.As<bool>();
		}
	}
}
