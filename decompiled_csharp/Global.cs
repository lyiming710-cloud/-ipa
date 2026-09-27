using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using Microsoft.Extensions.Logging;
using ZLogger;

[GlobalClass]
[ScriptPath("res://Core/Global/Global.cs")]
public class Global : Node
{
	public delegate void AnimeFrameRateChangeEventHandler();

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName GetFrameRateOptions = "GetFrameRateOptions";

		public static readonly StringName ApplyEffectiveFrameRate = "ApplyEffectiveFrameRate";

		public static readonly StringName GetHTTPRequestErrorMessage = "GetHTTPRequestErrorMessage";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ApplyAndroidImmersiveMode = "ApplyAndroidImmersiveMode";

		public static readonly StringName FreshAnimeData = "FreshAnimeData";

		public static readonly StringName FreshAnimeDataOpenDir = "FreshAnimeDataOpenDir";

		public static readonly StringName FreshLevelData = "FreshLevelData";

		public static readonly StringName FreshLevelDataOpenDir = "FreshLevelDataOpenDir";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName version = "version";

		public static readonly StringName header = "header";

		public static readonly StringName isMobile = "isMobile";

		public static readonly StringName debug = "debug";

		public static readonly StringName animeFrameRate = "animeFrameRate";

		public static readonly StringName effectiveAnimeFrameRate = "effectiveAnimeFrameRate";

		public static readonly StringName trueAnimeFrameRate = "trueAnimeFrameRate";

		public static readonly StringName adobeAnimateRenderBackend = "adobeAnimateRenderBackend";

		public static readonly StringName timeScale = "timeScale";

		public static readonly StringName enterLevelMode = "enterLevelMode";

		public static readonly StringName enterLevelIsBattle = "enterLevelIsBattle";

		public static readonly StringName enterLevelIsBattleFinish = "enterLevelIsBattleFinish";

		public static readonly StringName enterLevelId = "enterLevelId";

		public static readonly StringName enterTryLevelGroup = "enterTryLevelGroup";

		public static readonly StringName enterQuizMap = "enterQuizMap";

		public static readonly StringName currentLevelChoose = "currentLevelChoose";

		public static readonly StringName currentChapterId = "currentChapterId";

		public static readonly StringName currentLevelId = "currentLevelId";

		public static readonly StringName currentAwardMode = "currentAwardMode";

		public static readonly StringName mainMenuShowMoreModes = "mainMenuShowMoreModes";

		public static readonly StringName currentAwardType = "currentAwardType";

		public static readonly StringName currentAwardValue = "currentAwardValue";

		public static readonly StringName currentDiyLevelUid = "currentDiyLevelUid";

		public static readonly StringName isMultiplayerMode = "isMultiplayerMode";

		public static readonly StringName isMultiplayerHost = "isMultiplayerHost";

		public static readonly StringName newVersion = "newVersion";

		public static readonly StringName hasNewVersion = "hasNewVersion";

		public static readonly StringName uri = "uri";

		public static readonly StringName newVersionSkip = "newVersionSkip";

		public static readonly StringName maxFps = "maxFps";

		public static readonly StringName isEditor = "isEditor";

		public static readonly StringName _frameRateService = "_frameRateService";

		public static readonly StringName _animeFrameRate = "_animeFrameRate";

		public static readonly StringName _adobeAnimateRenderBackend = "_adobeAnimateRenderBackend";

		public static readonly StringName _timeScale = "_timeScale";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private static readonly ILogger _logger = Log.CreateLogger<Global>();

	private readonly StringBuilder _pathBuilder = new StringBuilder(256);

	[ExportGroup("Base", "")]
	private DeviceFrameRateService _frameRateService;

	private double _animeFrameRate = 30.0;

	private AdobeAnimateRenderBackend _adobeAnimateRenderBackend;

	private double _timeScale = 1.0;

	public static Global Instance { get; private set; }

	public string version { get; set; } = "0.29.0.0";

	public string[] header { get; set; } = Array.Empty<string>();

	public bool isMobile { get; set; }

	[Export(PropertyHint.None, "")]
	public bool debug { get; set; }

	[Export(PropertyHint.None, "")]
	public double animeFrameRate
	{
		get
		{
			return _animeFrameRate;
		}
		set
		{
			_animeFrameRate = FrameRatePolicy.NormalizeRefreshRate(value);
			if (_frameRateService != null)
			{
				_frameRateService.SetPreferredFrameRate(_animeFrameRate);
			}
			else
			{
				ApplyEffectiveFrameRate((int)_animeFrameRate);
			}
		}
	}

	public int effectiveAnimeFrameRate { get; private set; } = 30;

	public double trueAnimeFrameRate => (double)effectiveAnimeFrameRate / timeScale;

	public AdobeAnimateRenderBackend adobeAnimateRenderBackend
	{
		get
		{
			return _adobeAnimateRenderBackend;
		}
		set
		{
			AdobeAnimateRenderBackend adobeAnimateRenderBackend = AdobeAnimateRenderBackendPolicy.Normalize((int)value);
			if (_adobeAnimateRenderBackend != adobeAnimateRenderBackend)
			{
				_adobeAnimateRenderBackend = adobeAnimateRenderBackend;
				OnAdobeAnimateRenderBackendChanged?.Invoke(adobeAnimateRenderBackend);
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public double timeScale
	{
		get
		{
			return _timeScale;
		}
		set
		{
			_timeScale = value;
			Engine.TimeScale = _timeScale;
			OnAnimeFrameRateChange?.Invoke();
		}
	}

	public string enterLevelMode { get; set; } = "LevelChoose";

	public bool enterLevelIsBattle { get; set; }

	public bool enterLevelIsBattleFinish { get; set; }

	public string enterLevelId { get; set; } = "-1";

	public string enterTryLevelGroup { get; set; } = "";

	public string enterQuizMap { get; set; } = "Frontlawn";

	public string currentLevelChoose { get; set; } = "Adventure";

	public int currentChapterId { get; set; } = -1;

	public int currentLevelId { get; set; } = -1;

	public bool currentAwardMode { get; set; }

	public bool mainMenuShowMoreModes { get; set; }

	public int currentAwardType { get; set; }

	public string currentAwardValue { get; set; } = "";

	public string currentDiyLevelUid { get; set; } = "";

	public bool isMultiplayerMode { get; set; }

	public bool isMultiplayerHost { get; set; }

	public string newVersion { get; set; } = "";

	public bool hasNewVersion { get; set; }

	public string uri { get; set; } = "";

	public bool newVersionSkip { get; set; }

	public int maxFps { get; set; } = 60;

	public bool isEditor { get; set; }

	public static bool IsMultiplayerMode => Instance?.isMultiplayerMode ?? false;

	public static bool IsMultiplayerHost => Instance?.isMultiplayerHost ?? false;

	public static string Version => Instance?.version ?? "";

	public static string[] Header => Instance?.header ?? Array.Empty<string>();

	public static bool IsMobile => Instance?.isMobile ?? false;

	public static bool IsEditor => Instance?.isEditor ?? false;

	public static double TimeScale
	{
		get
		{
			return Instance?.timeScale ?? 1.0;
		}
		set
		{
			if (Instance != null)
			{
				Instance.timeScale = value;
			}
		}
	}

	public event AnimeFrameRateChangeEventHandler OnAnimeFrameRateChange;

	public event Action OnFrameRateOptionsChanged;

	public event Action<AdobeAnimateRenderBackend> OnAdobeAnimateRenderBackendChanged;

	public int[] GetFrameRateOptions()
	{
		return _frameRateService?.GetFrameRateOptions() ?? FrameRatePolicy.BuildOptions(Math.Max(1, maxFps));
	}

	private void ApplyEffectiveFrameRate(int effectiveFrameRate)
	{
		effectiveAnimeFrameRate = Math.Max(1, effectiveFrameRate);
		Engine.MaxFps = effectiveAnimeFrameRate;
		Engine.PhysicsTicksPerSecond = Mathf.Min(60, effectiveFrameRate);
		OnAnimeFrameRateChange?.Invoke();
	}

	public string GetHTTPRequestErrorMessage(long result)
	{
		if ((ulong)result <= 8uL)
		{
			switch ((int)result)
			{
			case 0:
				return "";
			case 1:
				return "无法连接到服务器";
			case 2:
				return "无法解析服务器地址";
			case 3:
				return "连接错误";
			case 4:
				return "TLS握手失败，请检查网络连接";
			case 5:
				return "服务器无响应";
			case 6:
				return "请求失败";
			case 7:
				return "重定向次数过多";
			case 8:
				return "服务器返回的数据超过安全上限";
			}
		}
		return $"未知网络错误({result})";
	}

	public override void _Ready()
	{
		StartupLoadDiagnostics.Mark("global.ready.begin");
		Instance = this;
		StartupLoadDiagnostics.Mark("global.log.begin");
		Log.Init(debug);
		StartupLoadDiagnostics.Mark("global.log.end");
		header = new string[1] { "X-PVZHE-Client-Version:" + version };
		GetWindow().CloseRequested += () =>
		{
			if (!Engine.IsEditorHint())
			{
				if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentControl) && !isMultiplayerMode)
				{
					GameSaveManager.Instance.SaveLevelProgress(TowerDefenseManager.Instance.currentControl.levelConfig.name, TowerDefenseManager.Instance.currentControl.ModLevelIdentity);
				}
				GameSaveManager.Instance.Save();
			}
		};
		string name = OS.GetName();
		isMobile = name == "Android" || name == "iOS";
		StartupLoadDiagnostics.Mark("global.locale.begin");
		TranslationServer.SetLocale("zh");
		StartupLoadDiagnostics.Mark("global.locale.end");
		StartupLoadDiagnostics.Mark("global.frame_rate.begin");
		_frameRateService = new DeviceFrameRateService();
		_frameRateService.Initialize(_animeFrameRate);
		_frameRateService.EffectiveFrameRateChanged += ApplyEffectiveFrameRate;
		_frameRateService.FrameRateOptionsChanged += () =>
		{
			maxFps = _frameRateService.DeviceFrameRateLimit;
			OnFrameRateOptionsChanged?.Invoke();
		};
		AddChild(_frameRateService, forceReadableName: false, InternalMode.Disabled);
		StartupLoadDiagnostics.Mark("global.frame_rate.end");
		if (isMobile)
		{
			OS.RequestPermissions();
			if (name == "Android")
			{
				CallDeferred(MethodName.ApplyAndroidImmersiveMode);
			}
		}
		StartupLoadDiagnostics.Mark("global.ready.end");
	}

	private void ApplyAndroidImmersiveMode()
	{
		Window window = GetWindow();
		if (window != null)
		{
			window.Mode = Window.ModeEnum.ExclusiveFullscreen;
			window.Borderless = true;
			int windowId = DisplayServer.WindowGetCurrentScreen();
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen, windowId);
			DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, enabled: true, windowId);
		}
	}

	public void FreshAnimeData()
	{
		new GodotThread().Start(Callable.From(() =>
		{
			FreshAnimeDataOpenDir("res://");
		}), GodotThread.Priority.Normal);
	}

	public void FreshAnimeDataOpenDir(string path)
	{
		DirAccess dirAccess = DirAccess.Open(path);
		ZLoggerInformationInterpolatedStringHandler message;
		bool enabled;
		if (dirAccess != null)
		{
			dirAccess.ListDirBegin();
			string next = dirAccess.GetNext();
			while (next != "")
			{
				_pathBuilder.Clear();
				_pathBuilder.Append(path).Append("//").Append(next);
				string text = _pathBuilder.ToString();
				if (dirAccess.CurrentIsDir())
				{
					ILogger logger = _logger;
					ILogger logger2 = logger;
					message = new ZLoggerInformationInterpolatedStringHandler(5, 1, logger, out enabled);
					if (enabled)
					{
						message.AppendLiteral("发现文件夹");
						message.AppendFormatted(text, 0, null, "fullPath");
					}
					logger2.ZLogInformation(ref message);
					FreshAnimeDataOpenDir(text);
				}
				else
				{
					ILogger logger = _logger;
					ILogger logger3 = logger;
					message = new ZLoggerInformationInterpolatedStringHandler(4, 1, logger, out enabled);
					if (enabled)
					{
						message.AppendLiteral("发现文件");
						message.AppendFormatted(text, 0, null, "fullPath");
					}
					logger3.ZLogInformation(ref message);
					if (next.GetExtension() == "tres" && GD.Load(text) is AdobeAnimateData adobeAnimateData)
					{
						adobeAnimateData.Init();
						ResourceSaver.Save(adobeAnimateData, text, ResourceSaver.SaverFlags.Compress);
					}
				}
				next = dirAccess.GetNext();
			}
		}
		else
		{
			ILogger logger = _logger;
			ILogger logger4 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(10, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("尝试访问路径时出错。");
			}
			logger4.ZLogInformation(ref message);
		}
	}

	public void FreshLevelData()
	{
		new GodotThread().Start(Callable.From(() =>
		{
			FreshLevelDataOpenDir("res://");
		}), GodotThread.Priority.Normal);
	}

	public void FreshLevelDataOpenDir(string path)
	{
		DirAccess dirAccess = DirAccess.Open(path);
		ZLoggerInformationInterpolatedStringHandler message;
		bool enabled;
		if (dirAccess != null)
		{
			dirAccess.ListDirBegin();
			string next = dirAccess.GetNext();
			while (next != "")
			{
				_pathBuilder.Clear();
				_pathBuilder.Append(path).Append("//").Append(next);
				string text = _pathBuilder.ToString();
				if (dirAccess.CurrentIsDir())
				{
					ILogger logger = _logger;
					ILogger logger2 = logger;
					message = new ZLoggerInformationInterpolatedStringHandler(5, 1, logger, out enabled);
					if (enabled)
					{
						message.AppendLiteral("发现文件夹");
						message.AppendFormatted(text, 0, null, "fullPath");
					}
					logger2.ZLogInformation(ref message);
					FreshLevelDataOpenDir(text);
				}
				else
				{
					ILogger logger = _logger;
					ILogger logger3 = logger;
					message = new ZLoggerInformationInterpolatedStringHandler(4, 1, logger, out enabled);
					if (enabled)
					{
						message.AppendLiteral("发现文件");
						message.AppendFormatted(text, 0, null, "fullPath");
					}
					logger3.ZLogInformation(ref message);
					if (next.GetExtension() == "tres" && GD.Load(text) is TowerDefenseLevelConfig towerDefenseLevelConfig)
					{
						towerDefenseLevelConfig.Clear();
						ResourceSaver.Save(towerDefenseLevelConfig, "", ResourceSaver.SaverFlags.None);
					}
				}
				next = dirAccess.GetNext();
			}
		}
		else
		{
			ILogger logger = _logger;
			ILogger logger4 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(10, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("尝试访问路径时出错。");
			}
			logger4.ZLogInformation(ref message);
		}
	}

	public Global()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/Global");
		StartupLoadDiagnostics.ObserveAutoloadTree();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.GetFrameRateOptions, new PropertyInfo(Variant.Type.PackedInt32Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyEffectiveFrameRate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "effectiveFrameRate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetHTTPRequestErrorMessage, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyAndroidImmersiveMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FreshAnimeData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FreshAnimeDataOpenDir, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FreshLevelData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FreshLevelDataOpenDir, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetFrameRateOptions && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int[]>(GetFrameRateOptions());
			return true;
		}
		if (method == MethodName.ApplyEffectiveFrameRate && args.Count == 1)
		{
			ApplyEffectiveFrameRate(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetHTTPRequestErrorMessage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetHTTPRequestErrorMessage(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyAndroidImmersiveMode && args.Count == 0)
		{
			ApplyAndroidImmersiveMode();
			ret = default;
			return true;
		}
		if (method == MethodName.FreshAnimeData && args.Count == 0)
		{
			FreshAnimeData();
			ret = default;
			return true;
		}
		if (method == MethodName.FreshAnimeDataOpenDir && args.Count == 1)
		{
			FreshAnimeDataOpenDir(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreshLevelData && args.Count == 0)
		{
			FreshLevelData();
			ret = default;
			return true;
		}
		if (method == MethodName.FreshLevelDataOpenDir && args.Count == 1)
		{
			FreshLevelDataOpenDir(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetFrameRateOptions)
		{
			return true;
		}
		if (method == MethodName.ApplyEffectiveFrameRate)
		{
			return true;
		}
		if (method == MethodName.GetHTTPRequestErrorMessage)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.ApplyAndroidImmersiveMode)
		{
			return true;
		}
		if (method == MethodName.FreshAnimeData)
		{
			return true;
		}
		if (method == MethodName.FreshAnimeDataOpenDir)
		{
			return true;
		}
		if (method == MethodName.FreshLevelData)
		{
			return true;
		}
		if (method == MethodName.FreshLevelDataOpenDir)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.version)
		{
			version = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.header)
		{
			header = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName.isMobile)
		{
			isMobile = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.debug)
		{
			debug = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.animeFrameRate)
		{
			animeFrameRate = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.effectiveAnimeFrameRate)
		{
			effectiveAnimeFrameRate = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.adobeAnimateRenderBackend)
		{
			adobeAnimateRenderBackend = VariantUtils.ConvertTo<AdobeAnimateRenderBackend>(in value);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			timeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.enterLevelMode)
		{
			enterLevelMode = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.enterLevelIsBattle)
		{
			enterLevelIsBattle = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.enterLevelIsBattleFinish)
		{
			enterLevelIsBattleFinish = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.enterLevelId)
		{
			enterLevelId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.enterTryLevelGroup)
		{
			enterTryLevelGroup = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.enterQuizMap)
		{
			enterQuizMap = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentLevelChoose)
		{
			currentLevelChoose = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentChapterId)
		{
			currentChapterId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentLevelId)
		{
			currentLevelId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentAwardMode)
		{
			currentAwardMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mainMenuShowMoreModes)
		{
			mainMenuShowMoreModes = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.currentAwardType)
		{
			currentAwardType = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentAwardValue)
		{
			currentAwardValue = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentDiyLevelUid)
		{
			currentDiyLevelUid = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.isMultiplayerMode)
		{
			isMultiplayerMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isMultiplayerHost)
		{
			isMultiplayerHost = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.newVersion)
		{
			newVersion = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.hasNewVersion)
		{
			hasNewVersion = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.uri)
		{
			uri = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.newVersionSkip)
		{
			newVersionSkip = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.maxFps)
		{
			maxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.isEditor)
		{
			isEditor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._frameRateService)
		{
			_frameRateService = VariantUtils.ConvertTo<DeviceFrameRateService>(in value);
			return true;
		}
		if (name == PropertyName._animeFrameRate)
		{
			_animeFrameRate = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._adobeAnimateRenderBackend)
		{
			_adobeAnimateRenderBackend = VariantUtils.ConvertTo<AdobeAnimateRenderBackend>(in value);
			return true;
		}
		if (name == PropertyName._timeScale)
		{
			_timeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.version)
		{
			from = version;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.header)
		{
			value = VariantUtils.CreateFrom<string[]>(header);
			return true;
		}
		bool from2;
		if (name == PropertyName.isMobile)
		{
			from2 = isMobile;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.debug)
		{
			from2 = debug;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		double from3;
		if (name == PropertyName.animeFrameRate)
		{
			from3 = animeFrameRate;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		int from4;
		if (name == PropertyName.effectiveAnimeFrameRate)
		{
			from4 = effectiveAnimeFrameRate;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.trueAnimeFrameRate)
		{
			from3 = trueAnimeFrameRate;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.adobeAnimateRenderBackend)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateRenderBackend>(adobeAnimateRenderBackend);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			from3 = timeScale;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.enterLevelMode)
		{
			from = enterLevelMode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.enterLevelIsBattle)
		{
			from2 = enterLevelIsBattle;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.enterLevelIsBattleFinish)
		{
			from2 = enterLevelIsBattleFinish;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.enterLevelId)
		{
			from = enterLevelId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.enterTryLevelGroup)
		{
			from = enterTryLevelGroup;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.enterQuizMap)
		{
			from = enterQuizMap;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.currentLevelChoose)
		{
			from = currentLevelChoose;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.currentChapterId)
		{
			from4 = currentChapterId;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.currentLevelId)
		{
			from4 = currentLevelId;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.currentAwardMode)
		{
			from2 = currentAwardMode;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.mainMenuShowMoreModes)
		{
			from2 = mainMenuShowMoreModes;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.currentAwardType)
		{
			from4 = currentAwardType;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.currentAwardValue)
		{
			from = currentAwardValue;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.currentDiyLevelUid)
		{
			from = currentDiyLevelUid;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.isMultiplayerMode)
		{
			from2 = isMultiplayerMode;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.isMultiplayerHost)
		{
			from2 = isMultiplayerHost;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.newVersion)
		{
			from = newVersion;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.hasNewVersion)
		{
			from2 = hasNewVersion;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.uri)
		{
			from = uri;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.newVersionSkip)
		{
			from2 = newVersionSkip;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.maxFps)
		{
			from4 = maxFps;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.isEditor)
		{
			from2 = isEditor;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._frameRateService)
		{
			value = VariantUtils.CreateFrom(in _frameRateService);
			return true;
		}
		if (name == PropertyName._animeFrameRate)
		{
			value = VariantUtils.CreateFrom(in _animeFrameRate);
			return true;
		}
		if (name == PropertyName._adobeAnimateRenderBackend)
		{
			value = VariantUtils.CreateFrom(in _adobeAnimateRenderBackend);
			return true;
		}
		if (name == PropertyName._timeScale)
		{
			value = VariantUtils.CreateFrom(in _timeScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.version, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName.header, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isMobile, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.debug, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Base", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._frameRateService, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._animeFrameRate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.animeFrameRate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.effectiveAnimeFrameRate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.trueAnimeFrameRate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._adobeAnimateRenderBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.adobeAnimateRenderBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._timeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.enterLevelMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enterLevelIsBattle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enterLevelIsBattleFinish, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.enterLevelId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.enterTryLevelGroup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.enterQuizMap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentLevelChoose, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentChapterId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentLevelId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.currentAwardMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mainMenuShowMoreModes, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentAwardType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentAwardValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentDiyLevelUid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isMultiplayerMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isMultiplayerHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.newVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasNewVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.uri, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.newVersionSkip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxFps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.version, Variant.From<string>(version));
		info.AddProperty(PropertyName.header, Variant.From<string[]>(header));
		info.AddProperty(PropertyName.isMobile, Variant.From<bool>(isMobile));
		info.AddProperty(PropertyName.debug, Variant.From<bool>(debug));
		info.AddProperty(PropertyName.animeFrameRate, Variant.From<double>(animeFrameRate));
		info.AddProperty(PropertyName.effectiveAnimeFrameRate, Variant.From<int>(effectiveAnimeFrameRate));
		info.AddProperty(PropertyName.adobeAnimateRenderBackend, Variant.From<AdobeAnimateRenderBackend>(adobeAnimateRenderBackend));
		info.AddProperty(PropertyName.timeScale, Variant.From<double>(timeScale));
		info.AddProperty(PropertyName.enterLevelMode, Variant.From<string>(enterLevelMode));
		info.AddProperty(PropertyName.enterLevelIsBattle, Variant.From<bool>(enterLevelIsBattle));
		info.AddProperty(PropertyName.enterLevelIsBattleFinish, Variant.From<bool>(enterLevelIsBattleFinish));
		info.AddProperty(PropertyName.enterLevelId, Variant.From<string>(enterLevelId));
		info.AddProperty(PropertyName.enterTryLevelGroup, Variant.From<string>(enterTryLevelGroup));
		info.AddProperty(PropertyName.enterQuizMap, Variant.From<string>(enterQuizMap));
		info.AddProperty(PropertyName.currentLevelChoose, Variant.From<string>(currentLevelChoose));
		info.AddProperty(PropertyName.currentChapterId, Variant.From<int>(currentChapterId));
		info.AddProperty(PropertyName.currentLevelId, Variant.From<int>(currentLevelId));
		info.AddProperty(PropertyName.currentAwardMode, Variant.From<bool>(currentAwardMode));
		info.AddProperty(PropertyName.mainMenuShowMoreModes, Variant.From<bool>(mainMenuShowMoreModes));
		info.AddProperty(PropertyName.currentAwardType, Variant.From<int>(currentAwardType));
		info.AddProperty(PropertyName.currentAwardValue, Variant.From<string>(currentAwardValue));
		info.AddProperty(PropertyName.currentDiyLevelUid, Variant.From<string>(currentDiyLevelUid));
		info.AddProperty(PropertyName.isMultiplayerMode, Variant.From<bool>(isMultiplayerMode));
		info.AddProperty(PropertyName.isMultiplayerHost, Variant.From<bool>(isMultiplayerHost));
		info.AddProperty(PropertyName.newVersion, Variant.From<string>(newVersion));
		info.AddProperty(PropertyName.hasNewVersion, Variant.From<bool>(hasNewVersion));
		info.AddProperty(PropertyName.uri, Variant.From<string>(uri));
		info.AddProperty(PropertyName.newVersionSkip, Variant.From<bool>(newVersionSkip));
		info.AddProperty(PropertyName.maxFps, Variant.From<int>(maxFps));
		info.AddProperty(PropertyName.isEditor, Variant.From<bool>(isEditor));
		info.AddProperty(PropertyName._frameRateService, Variant.From(in _frameRateService));
		info.AddProperty(PropertyName._animeFrameRate, Variant.From(in _animeFrameRate));
		info.AddProperty(PropertyName._adobeAnimateRenderBackend, Variant.From(in _adobeAnimateRenderBackend));
		info.AddProperty(PropertyName._timeScale, Variant.From(in _timeScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.version, out var value))
		{
			version = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.header, out var value2))
		{
			header = value2.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName.isMobile, out var value3))
		{
			isMobile = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.debug, out var value4))
		{
			debug = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.animeFrameRate, out var value5))
		{
			animeFrameRate = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.effectiveAnimeFrameRate, out var value6))
		{
			effectiveAnimeFrameRate = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.adobeAnimateRenderBackend, out var value7))
		{
			adobeAnimateRenderBackend = value7.As<AdobeAnimateRenderBackend>();
		}
		if (info.TryGetProperty(PropertyName.timeScale, out var value8))
		{
			timeScale = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.enterLevelMode, out var value9))
		{
			enterLevelMode = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.enterLevelIsBattle, out var value10))
		{
			enterLevelIsBattle = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.enterLevelIsBattleFinish, out var value11))
		{
			enterLevelIsBattleFinish = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.enterLevelId, out var value12))
		{
			enterLevelId = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.enterTryLevelGroup, out var value13))
		{
			enterTryLevelGroup = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName.enterQuizMap, out var value14))
		{
			enterQuizMap = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentLevelChoose, out var value15))
		{
			currentLevelChoose = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentChapterId, out var value16))
		{
			currentChapterId = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentLevelId, out var value17))
		{
			currentLevelId = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentAwardMode, out var value18))
		{
			currentAwardMode = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mainMenuShowMoreModes, out var value19))
		{
			mainMenuShowMoreModes = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.currentAwardType, out var value20))
		{
			currentAwardType = value20.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentAwardValue, out var value21))
		{
			currentAwardValue = value21.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentDiyLevelUid, out var value22))
		{
			currentDiyLevelUid = value22.As<string>();
		}
		if (info.TryGetProperty(PropertyName.isMultiplayerMode, out var value23))
		{
			isMultiplayerMode = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isMultiplayerHost, out var value24))
		{
			isMultiplayerHost = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.newVersion, out var value25))
		{
			newVersion = value25.As<string>();
		}
		if (info.TryGetProperty(PropertyName.hasNewVersion, out var value26))
		{
			hasNewVersion = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.uri, out var value27))
		{
			uri = value27.As<string>();
		}
		if (info.TryGetProperty(PropertyName.newVersionSkip, out var value28))
		{
			newVersionSkip = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.maxFps, out var value29))
		{
			maxFps = value29.As<int>();
		}
		if (info.TryGetProperty(PropertyName.isEditor, out var value30))
		{
			isEditor = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._frameRateService, out var value31))
		{
			_frameRateService = value31.As<DeviceFrameRateService>();
		}
		if (info.TryGetProperty(PropertyName._animeFrameRate, out var value32))
		{
			_animeFrameRate = value32.As<double>();
		}
		if (info.TryGetProperty(PropertyName._adobeAnimateRenderBackend, out var value33))
		{
			_adobeAnimateRenderBackend = value33.As<AdobeAnimateRenderBackend>();
		}
		if (info.TryGetProperty(PropertyName._timeScale, out var value34))
		{
			_timeScale = value34.As<double>();
		}
	}
}
