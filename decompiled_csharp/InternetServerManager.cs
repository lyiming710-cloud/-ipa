using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using Microsoft.Extensions.Logging;
using ZLogger;

[GlobalClass]
[ScriptPath("res://Core/InternetServerManager/InternetServerManager.cs")]
public class InternetServerManager : Node2D
{
	public delegate void OnlineLevelGetEventHandler(Dictionary data);

	public delegate void WorkshopTagsGetEventHandler(Godot.Collections.Array data);

	public delegate void ShareLevelSuccessEventHandler(string code, long expireAt, long expireSeconds);

	public delegate void ShareLevelFailedEventHandler(string message);

	public delegate void GetSharedLevelSuccessEventHandler(byte[] data);

	public delegate void GetSharedLevelFailedEventHandler(string message);

	public delegate void DailyLevelMonthLoadedEventHandler(int year, int month);

	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateHttpRequest = "CreateHttpRequest";

		public static readonly StringName VersionHTTPRequestCompleted = "VersionHTTPRequestCompleted";

		public static readonly StringName DailyLevelMonthNeedRefresh = "DailyLevelMonthNeedRefresh";

		public static readonly StringName IsDailyLevelRequestPending = "IsDailyLevelRequestPending";

		public static readonly StringName GetDailyLevel = "GetDailyLevel";

		public static readonly StringName DailyLevelHTTPRequestCompleted = "DailyLevelHTTPRequestCompleted";

		public static readonly StringName GetOnlineLevelPage = "GetOnlineLevelPage";

		public static readonly StringName BuildOnlineLevelPageUrl = "BuildOnlineLevelPageUrl";

		public static readonly StringName OnlineLevelHTTPRequestCompleted = "OnlineLevelHTTPRequestCompleted";

		public static readonly StringName GetWorkshopTags = "GetWorkshopTags";

		public static readonly StringName WorkshopTagsHTTPRequestCompleted = "WorkshopTagsHTTPRequestCompleted";

		public static readonly StringName OnlineLevelPost = "OnlineLevelPost";

		public static readonly StringName ShareFile = "ShareFile";

		public static readonly StringName ShareLevel = "ShareLevel";

		public static readonly StringName GetSharedFile = "GetSharedFile";

		public static readonly StringName ExportFileHTTPRequestCompleted = "ExportFileHTTPRequestCompleted";

		public static readonly StringName LoadFileHTTPRequestCompleted = "LoadFileHTTPRequestCompleted";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName versionGetOver = "versionGetOver";

		public static readonly StringName newVersion = "newVersion";

		public static readonly StringName versionMessage = "versionMessage";

		public static readonly StringName dailyLevelGetOver = "dailyLevelGetOver";

		public static readonly StringName dailyLevelLoadedMonths = "dailyLevelLoadedMonths";

		public static readonly StringName dailyLevelRequestYear = "dailyLevelRequestYear";

		public static readonly StringName dailyLevelRequestMonth = "dailyLevelRequestMonth";

		public static readonly StringName versionHttpRequest = "versionHttpRequest";

		public static readonly StringName dailyLevelHTTPRequest = "dailyLevelHTTPRequest";

		public static readonly StringName onlineLevelHTTPRequest = "onlineLevelHTTPRequest";

		public static readonly StringName _onlineLevelStatistics = "_onlineLevelStatistics";

		public static readonly StringName workshopTagsHTTPRequest = "workshopTagsHTTPRequest";

		public static readonly StringName exportFileHTTPRequest = "exportFileHTTPRequest";

		public static readonly StringName loadFileHTTPRequest = "loadFileHTTPRequest";

		public static readonly StringName dailyLevelRequesting = "dailyLevelRequesting";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static readonly ILogger _logger = Log.CreateLogger<InternetServerManager>();

	public NativeHttpRequest versionHttpRequest;

	public NativeHttpRequest dailyLevelHTTPRequest;

	public NativeHttpRequest onlineLevelHTTPRequest;

	private OnlineLevelStatisticsReporter _onlineLevelStatistics;

	public NativeHttpRequest workshopTagsHTTPRequest;

	public NativeHttpRequest exportFileHTTPRequest;

	public NativeHttpRequest loadFileHTTPRequest;

	private bool dailyLevelRequesting;

	public bool versionGetOver { get; set; }

	public string newVersion { get; set; } = "";

	public string versionMessage { get; set; } = "";

	public bool dailyLevelGetOver { get; set; }

	public Dictionary dailyLevelLoadedMonths { get; set; } = new Dictionary();

	public int dailyLevelRequestYear { get; set; }

	public int dailyLevelRequestMonth { get; set; }

	public static InternetServerManager Instance { get; private set; }

	public event OnlineLevelGetEventHandler OnOnlineLevelGet;

	public event WorkshopTagsGetEventHandler OnWorkshopTagsGet;

	public event ShareLevelSuccessEventHandler OnShareLevelSuccess;

	public event ShareLevelFailedEventHandler OnShareLevelFailed;

	public event GetSharedLevelSuccessEventHandler OnGetSharedLevelSuccess;

	public event GetSharedLevelFailedEventHandler OnGetSharedLevelFailed;

	public event DailyLevelMonthLoadedEventHandler OnDailyLevelMonthLoaded;

	public override void _Ready()
	{
		Instance = this;
		versionHttpRequest = CreateHttpRequest();
		dailyLevelHTTPRequest = CreateHttpRequest();
		onlineLevelHTTPRequest = CreateHttpRequest();
		_onlineLevelStatistics = new OnlineLevelStatisticsReporter();
		AddChild(_onlineLevelStatistics, forceReadableName: false, InternalMode.Disabled);
		workshopTagsHTTPRequest = CreateHttpRequest();
		exportFileHTTPRequest = CreateHttpRequest();
		loadFileHTTPRequest = CreateHttpRequest();
		versionHttpRequest.RequestCompleted += VersionHTTPRequestCompleted;
		dailyLevelHTTPRequest.RequestCompleted += DailyLevelHTTPRequestCompleted;
		onlineLevelHTTPRequest.RequestCompleted += OnlineLevelHTTPRequestCompleted;
		workshopTagsHTTPRequest.RequestCompleted += WorkshopTagsHTTPRequestCompleted;
		exportFileHTTPRequest.RequestCompleted += ExportFileHTTPRequestCompleted;
		loadFileHTTPRequest.RequestCompleted += LoadFileHTTPRequestCompleted;
		versionHttpRequest.Request("https://api.pvzhe.com/new_version", Global.Instance.header);
	}

	private NativeHttpRequest CreateHttpRequest()
	{
		NativeHttpRequest nativeHttpRequest = new NativeHttpRequest();
		AddChild(nativeHttpRequest, forceReadableName: false, InternalMode.Disabled);
		return nativeHttpRequest;
	}

	public void VersionHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		ZLoggerInformationInterpolatedStringHandler message;
		bool enabled;
		if (result != 0L)
		{
			ILogger logger = _logger;
			ILogger logger2 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(8, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("版本检查失败: ");
				message.AppendFormatted(Global.Instance.GetHTTPRequestErrorMessage(result), 0, null, "Global.Instance.GetHTTPRequestErrorMessage(result)");
			}
			logger2.ZLogInformation(ref message);
			return;
		}
		JsonDocument jsonDocument;
		try
		{
			jsonDocument = JsonDocument.Parse(body);
		}
		catch (JsonException)
		{
			return;
		}
		using (jsonDocument)
		{
			JsonElement rootElement = jsonDocument.RootElement;
			if (rootElement.ValueKind != JsonValueKind.Object)
			{
				return;
			}
			string propertyName = (Global.IsMobile ? "android" : "pc");
			if (!rootElement.TryGetProperty(propertyName, out var value))
			{
				return;
			}
			versionGetOver = true;
			List<int> list = new List<int>(4);
			if (value.TryGetProperty("version", out var value2) && value2.ValueKind == JsonValueKind.Array)
			{
				foreach (JsonElement item in value2.EnumerateArray())
				{
					list.Add(item.GetInt32());
				}
			}
			else
			{
				list.AddRange(new int[4] { -1, -1, -1, -1 });
			}
			string text = ((value.TryGetProperty("base64", out var value3) && value3.ValueKind == JsonValueKind.String) ? (value3.GetString() ?? "error") : "error");
			Global.Instance.uri = ((value.TryGetProperty("url", out var value4) && value4.ValueKind == JsonValueKind.String) ? (value4.GetString() ?? "error") : "error");
			versionMessage = ((value.TryGetProperty("message", out var value5) && value5.ValueKind == JsonValueKind.String) ? (value5.GetString() ?? "") : "");
			newVersion = string.Join(".", list);
			ILogger logger = _logger;
			ILogger logger3 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(12, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("newVersion: ");
				message.AppendFormatted(newVersion, 0, null, "newVersion");
			}
			logger3.ZLogInformation(ref message);
			logger = _logger;
			ILogger logger4 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(5, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("uri: ");
				message.AppendFormatted(Global.Instance.uri, 0, null, "Global.Instance.uri");
			}
			logger4.ZLogInformation(ref message);
			logger = _logger;
			ILogger logger5 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(8, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("base64: ");
				message.AppendFormatted(text, 0, null, "base64");
			}
			logger5.ZLogInformation(ref message);
			if (!(text != "error"))
			{
				return;
			}
			string text2 = Marshalls.Base64ToUtf8(text);
			logger = _logger;
			ILogger logger6 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(13, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("checkString: ");
				message.AppendFormatted(text2, 0, null, "checkString");
			}
			logger6.ZLogInformation(ref message);
			if (!(text2 == newVersion + Global.Instance.uri))
			{
				return;
			}
			Global.Instance.newVersion = newVersion;
			bool hasNewVersion = false;
			string[] array = Global.Instance.version.Split(".");
			for (int i = 0; i < list.Count; i++)
			{
				if (int.Parse(array[i]) < list[i])
				{
					hasNewVersion = true;
					break;
				}
				if (int.Parse(array[i]) > list[i])
				{
					break;
				}
			}
			Global.Instance.hasNewVersion = hasNewVersion;
		}
	}

	public bool DailyLevelMonthNeedRefresh(int year, int month)
	{
		string text = $"{year:D}-{month:D2}";
		if (!dailyLevelLoadedMonths.ContainsKey(text))
		{
			return true;
		}
		Dictionary datetimeDictFromSystem = Time.GetDatetimeDictFromSystem();
		if ((int)(long)datetimeDictFromSystem["year"] == year && (int)(long)datetimeDictFromSystem["month"] == month)
		{
			string text2 = string.Format("{0}-{1:D2}-{2:D2}", (int)(long)datetimeDictFromSystem["year"], (int)(long)datetimeDictFromSystem["month"], (int)(long)datetimeDictFromSystem["day"]);
			System.Collections.Generic.Dictionary<string, Variant> dAILY_LEVEL_DATA = ResourceManager.Instance.DAILY_LEVEL_DATA;
			if (dAILY_LEVEL_DATA == null || dAILY_LEVEL_DATA.Count == 0 || !dAILY_LEVEL_DATA.ContainsKey("LevelDateMap"))
			{
				return true;
			}
			if (!((Dictionary)dAILY_LEVEL_DATA["LevelDateMap"]).ContainsKey(text2))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsDailyLevelRequestPending(int year, int month)
	{
		if (dailyLevelRequesting && dailyLevelRequestYear == year)
		{
			return dailyLevelRequestMonth == month;
		}
		return false;
	}

	public void GetDailyLevel(int year = 0, int month = 0, bool forceRefresh = false)
	{
		if (year == 0 || month == 0)
		{
			Dictionary datetimeDictFromSystem = Time.GetDatetimeDictFromSystem();
			year = (int)(long)datetimeDictFromSystem["year"];
			month = (int)(long)datetimeDictFromSystem["month"];
		}
		string text = $"{year:D}-{month:D2}";
		if (!forceRefresh && dailyLevelLoadedMonths.ContainsKey(text))
		{
			OnDailyLevelMonthLoaded?.Invoke(year, month);
		}
		else if (!dailyLevelRequesting || dailyLevelRequestYear != year || dailyLevelRequestMonth != month)
		{
			dailyLevelRequestYear = year;
			dailyLevelRequestMonth = month;
			dailyLevelRequesting = true;
			dailyLevelHTTPRequest.CancelRequest();
			dailyLevelHTTPRequest.Request($"https://api.pvzhe.com/get_daily_levels?year={year}&month={month}", Global.Instance.header);
		}
	}

	public void DailyLevelHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != 0L)
		{
			ILogger logger = _logger;
			ZLoggerInformationInterpolatedStringHandler message = new ZLoggerInformationInterpolatedStringHandler(10, 1, logger, out var enabled);
			if (enabled)
			{
				message.AppendLiteral("每日关卡获取失败: ");
				message.AppendFormatted(Global.Instance.GetHTTPRequestErrorMessage(result), 0, null, "Global.Instance.GetHTTPRequestErrorMessage(result)");
			}
			logger.ZLogInformation(ref message);
			dailyLevelRequesting = false;
			return;
		}
		Json json = new Json();
		json.Parse(body.GetStringFromUtf8());
		if (json.Data.VariantType != Variant.Type.Nil)
		{
			Dictionary dictionary = (Dictionary)json.Data;
			if (ResourceManager.Instance.DAILY_LEVEL_DATA.Count == 0)
			{
				foreach (Variant key in dictionary.Keys)
				{
					string text = (string)key;
					ResourceManager.Instance.DAILY_LEVEL_DATA[text] = dictionary[text];
				}
			}
			else
			{
				if (dictionary.ContainsKey("LevelDateMap"))
				{
					Dictionary dictionary2 = (Dictionary)dictionary["LevelDateMap"];
					Dictionary dictionary3 = (Dictionary)ResourceManager.Instance.DAILY_LEVEL_DATA["LevelDateMap"];
					foreach (Variant key2 in dictionary2.Keys)
					{
						string text2 = (string)key2;
						dictionary3[text2] = dictionary2[text2];
					}
				}
				if (dictionary.ContainsKey("LevelMeta"))
				{
					Dictionary dictionary4 = (Dictionary)dictionary["LevelMeta"];
					if (!ResourceManager.Instance.DAILY_LEVEL_DATA.ContainsKey("LevelMeta") || ResourceManager.Instance.DAILY_LEVEL_DATA["LevelMeta"].VariantType != Variant.Type.Dictionary)
					{
						ResourceManager.Instance.DAILY_LEVEL_DATA["LevelMeta"] = new Dictionary();
					}
					Dictionary dictionary5 = (Dictionary)ResourceManager.Instance.DAILY_LEVEL_DATA["LevelMeta"];
					foreach (Variant key3 in dictionary4.Keys)
					{
						string text3 = (string)key3;
						dictionary5[text3] = dictionary4[text3];
					}
				}
				if (dictionary.ContainsKey("LevelDateMeta"))
				{
					Dictionary dictionary6 = (Dictionary)dictionary["LevelDateMeta"];
					if (!ResourceManager.Instance.DAILY_LEVEL_DATA.ContainsKey("LevelDateMeta") || ResourceManager.Instance.DAILY_LEVEL_DATA["LevelDateMeta"].VariantType != Variant.Type.Dictionary)
					{
						ResourceManager.Instance.DAILY_LEVEL_DATA["LevelDateMeta"] = new Dictionary();
					}
					Dictionary dictionary7 = (Dictionary)ResourceManager.Instance.DAILY_LEVEL_DATA["LevelDateMeta"];
					foreach (Variant key4 in dictionary6.Keys)
					{
						string text4 = (string)key4;
						dictionary7[text4] = dictionary6[text4];
					}
				}
			}
			string text5 = $"{dailyLevelRequestYear:D}-{dailyLevelRequestMonth:D2}";
			dailyLevelLoadedMonths[text5] = true;
			dailyLevelGetOver = true;
			dailyLevelRequesting = false;
			OnDailyLevelMonthLoaded?.Invoke(dailyLevelRequestYear, dailyLevelRequestMonth);
		}
		else
		{
			dailyLevelRequesting = false;
		}
	}

	public void GetOnlineLevelPage(int pageIndex = 1, string suffix = "", string suffix2 = "", string search = "", string tags = "", string authorUid = "")
	{
		onlineLevelHTTPRequest.CancelRequest();
		onlineLevelHTTPRequest.Request(BuildOnlineLevelPageUrl(pageIndex, suffix, suffix2, search, tags, authorUid), Global.Instance.header);
	}

	internal static string BuildOnlineLevelPageUrl(int pageIndex = 1, string suffix = "", string suffix2 = "", string search = "", string tags = "", string authorUid = "")
	{
		pageIndex = Mathf.Clamp(pageIndex, 1, 100000);
		string text = $"https://api.pvzhe.com/workshop/levels?page={pageIndex}{suffix}{suffix2}";
		if (search != "")
		{
			text = text + "&search=" + Uri.EscapeDataString(search);
		}
		if (tags != "")
		{
			text = text + "&tags=" + Uri.EscapeDataString(tags);
		}
		if (!string.IsNullOrEmpty(authorUid))
		{
			text = text + "&authorUid=" + Uri.EscapeDataString(authorUid);
		}
		return text;
	}

	public void OnlineLevelHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != 0L || responseCode != 200)
		{
			OnOnlineLevelGet?.Invoke(new Dictionary());
			return;
		}
		Json json = new Json();
		if (json.Parse(body.GetStringFromUtf8()) == Error.Ok && json.Data.VariantType == Variant.Type.Dictionary)
		{
			OnOnlineLevelGet?.Invoke((Dictionary)json.Data);
		}
		else
		{
			OnOnlineLevelGet?.Invoke(new Dictionary());
		}
	}

	public void GetWorkshopTags()
	{
		workshopTagsHTTPRequest.CancelRequest();
		workshopTagsHTTPRequest.Request("https://api.pvzhe.com/workshop/tags", Global.Instance.header);
	}

	public void WorkshopTagsHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != 0L)
		{
			OnWorkshopTagsGet?.Invoke(new Godot.Collections.Array());
			return;
		}
		Json json = new Json();
		json.Parse(body.GetStringFromUtf8());
		if (json.Data.VariantType != Variant.Type.Nil && json.Data.VariantType == Variant.Type.Array)
		{
			OnWorkshopTagsGet?.Invoke((Godot.Collections.Array)json.Data);
		}
		else
		{
			OnWorkshopTagsGet?.Invoke(new Godot.Collections.Array());
		}
	}

	public void OnlineLevelPost(string levelId, string suffix = "abandon")
	{
		_onlineLevelStatistics.Enqueue(levelId, suffix, Global.Instance.header);
	}

	public void ShareFile(byte[] data)
	{
		exportFileHTTPRequest.CancelRequest();
		List<string> list = new List<string>();
		string[] header = Global.Instance.header;
		foreach (string item in header)
		{
			list.Add(item);
		}
		list.Add("Content-Type: application/octet-stream");
		byte[] body = data.Compress(FileAccess.CompressionMode.GZip);
		exportFileHTTPRequest.RequestRaw("https://api.pvzhe.com/save_share", list.ToArray(), HttpMethod.Post, body);
	}

	public void ShareLevel(byte[] data)
	{
		exportFileHTTPRequest.CancelRequest();
		List<string> list = new List<string>();
		string[] header = Global.Instance.header;
		foreach (string item in header)
		{
			list.Add(item);
		}
		list.Add("Content-Type: application/octet-stream");
		byte[] body = data.Compress(FileAccess.CompressionMode.GZip);
		exportFileHTTPRequest.RequestRaw("https://api.pvzhe.com/save_share", list.ToArray(), HttpMethod.Post, body);
	}

	public void GetSharedFile(string code)
	{
		loadFileHTTPRequest.CancelRequest();
		loadFileHTTPRequest.Request("https://api.pvzhe.com/save_share?code=" + code, Global.Instance.header);
	}

	public void ExportFileHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != 0L)
		{
			OnShareLevelFailed?.Invoke(Global.Instance.GetHTTPRequestErrorMessage(result));
			return;
		}
		JsonDocument jsonDocument = null;
		try
		{
			jsonDocument = JsonDocument.Parse(body);
		}
		catch (JsonException)
		{
			OnShareLevelFailed?.Invoke($"请求失败，状态码: {responseCode}");
			return;
		}
		using (jsonDocument)
		{
			JsonElement rootElement = jsonDocument.RootElement;
			if (rootElement.ValueKind != JsonValueKind.Object)
			{
				OnShareLevelFailed?.Invoke($"请求失败，状态码: {responseCode}");
			}
			else if (responseCode == 200)
			{
				string code = ((rootElement.TryGetProperty("code", out var value) && value.ValueKind == JsonValueKind.String) ? (value.GetString() ?? "") : "");
				long expireAt = ((rootElement.TryGetProperty("expireAt", out var value2) && value2.ValueKind == JsonValueKind.Number) ? value2.GetInt64() : 0);
				long expireSeconds = ((rootElement.TryGetProperty("expireSeconds", out var value3) && value3.ValueKind == JsonValueKind.Number) ? value3.GetInt64() : 0);
				OnShareLevelSuccess?.Invoke(code, expireAt, expireSeconds);
			}
			else
			{
				string message = ((rootElement.TryGetProperty("message", out var value4) && value4.ValueKind == JsonValueKind.String) ? (value4.GetString() ?? "未知错误") : "未知错误");
				OnShareLevelFailed?.Invoke(message);
			}
		}
	}

	public void LoadFileHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != 0L)
		{
			OnGetSharedLevelFailed?.Invoke(Global.Instance.GetHTTPRequestErrorMessage(result));
			return;
		}
		if (responseCode == 200)
		{
			OnGetSharedLevelSuccess?.Invoke(body);
			return;
		}
		string message = "未知错误";
		try
		{
			using JsonDocument jsonDocument = JsonDocument.Parse(body);
			JsonElement rootElement = jsonDocument.RootElement;
			if (rootElement.ValueKind == JsonValueKind.Object && rootElement.TryGetProperty("message", out var value) && value.ValueKind == JsonValueKind.String)
			{
				message = value.GetString() ?? "未知错误";
			}
		}
		catch (JsonException)
		{
		}
		OnGetSharedLevelFailed?.Invoke(message);
	}

	public InternetServerManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/InternetServerManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateHttpRequest, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VersionHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DailyLevelMonthNeedRefresh, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "year", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "month", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsDailyLevelRequestPending, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "year", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "month", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDailyLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "year", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "month", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "forceRefresh", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DailyLevelHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetOnlineLevelPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "pageIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "suffix", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "suffix2", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "search", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "authorUid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildOnlineLevelPageUrl, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "pageIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "suffix", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "suffix2", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "search", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "authorUid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnlineLevelHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetWorkshopTags, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WorkshopTagsHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnlineLevelPost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "suffix", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShareFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedByteArray, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShareLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedByteArray, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSharedFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "code", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportFileHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadFileHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateHttpRequest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<NativeHttpRequest>(CreateHttpRequest());
			return true;
		}
		if (method == MethodName.VersionHTTPRequestCompleted && args.Count == 4)
		{
			VersionHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DailyLevelMonthNeedRefresh && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(DailyLevelMonthNeedRefresh(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.IsDailyLevelRequestPending && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDailyLevelRequestPending(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetDailyLevel && args.Count == 3)
		{
			GetDailyLevel(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DailyLevelHTTPRequestCompleted && args.Count == 4)
		{
			DailyLevelHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetOnlineLevelPage && args.Count == 6)
		{
			GetOnlineLevelPage(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildOnlineLevelPageUrl && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<string>(BuildOnlineLevelPageUrl(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<string>(in args[5])));
			return true;
		}
		if (method == MethodName.OnlineLevelHTTPRequestCompleted && args.Count == 4)
		{
			OnlineLevelHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetWorkshopTags && args.Count == 0)
		{
			GetWorkshopTags();
			ret = default;
			return true;
		}
		if (method == MethodName.WorkshopTagsHTTPRequestCompleted && args.Count == 4)
		{
			WorkshopTagsHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnlineLevelPost && args.Count == 2)
		{
			OnlineLevelPost(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShareFile && args.Count == 1)
		{
			ShareFile(VariantUtils.ConvertTo<byte[]>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShareLevel && args.Count == 1)
		{
			ShareLevel(VariantUtils.ConvertTo<byte[]>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSharedFile && args.Count == 1)
		{
			GetSharedFile(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportFileHTTPRequestCompleted && args.Count == 4)
		{
			ExportFileHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadFileHTTPRequestCompleted && args.Count == 4)
		{
			LoadFileHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BuildOnlineLevelPageUrl && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<string>(BuildOnlineLevelPageUrl(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<string>(in args[5])));
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
		if (method == MethodName.CreateHttpRequest)
		{
			return true;
		}
		if (method == MethodName.VersionHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.DailyLevelMonthNeedRefresh)
		{
			return true;
		}
		if (method == MethodName.IsDailyLevelRequestPending)
		{
			return true;
		}
		if (method == MethodName.GetDailyLevel)
		{
			return true;
		}
		if (method == MethodName.DailyLevelHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.GetOnlineLevelPage)
		{
			return true;
		}
		if (method == MethodName.BuildOnlineLevelPageUrl)
		{
			return true;
		}
		if (method == MethodName.OnlineLevelHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.GetWorkshopTags)
		{
			return true;
		}
		if (method == MethodName.WorkshopTagsHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.OnlineLevelPost)
		{
			return true;
		}
		if (method == MethodName.ShareFile)
		{
			return true;
		}
		if (method == MethodName.ShareLevel)
		{
			return true;
		}
		if (method == MethodName.GetSharedFile)
		{
			return true;
		}
		if (method == MethodName.ExportFileHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.LoadFileHTTPRequestCompleted)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.versionGetOver)
		{
			versionGetOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.newVersion)
		{
			newVersion = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.versionMessage)
		{
			versionMessage = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.dailyLevelGetOver)
		{
			dailyLevelGetOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.dailyLevelLoadedMonths)
		{
			dailyLevelLoadedMonths = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.dailyLevelRequestYear)
		{
			dailyLevelRequestYear = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.dailyLevelRequestMonth)
		{
			dailyLevelRequestMonth = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.versionHttpRequest)
		{
			versionHttpRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.dailyLevelHTTPRequest)
		{
			dailyLevelHTTPRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.onlineLevelHTTPRequest)
		{
			onlineLevelHTTPRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName._onlineLevelStatistics)
		{
			_onlineLevelStatistics = VariantUtils.ConvertTo<OnlineLevelStatisticsReporter>(in value);
			return true;
		}
		if (name == PropertyName.workshopTagsHTTPRequest)
		{
			workshopTagsHTTPRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.exportFileHTTPRequest)
		{
			exportFileHTTPRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.loadFileHTTPRequest)
		{
			loadFileHTTPRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.dailyLevelRequesting)
		{
			dailyLevelRequesting = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.versionGetOver)
		{
			from = versionGetOver;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		string from2;
		if (name == PropertyName.newVersion)
		{
			from2 = newVersion;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.versionMessage)
		{
			from2 = versionMessage;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.dailyLevelGetOver)
		{
			from = dailyLevelGetOver;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dailyLevelLoadedMonths)
		{
			value = VariantUtils.CreateFrom<Dictionary>(dailyLevelLoadedMonths);
			return true;
		}
		int from3;
		if (name == PropertyName.dailyLevelRequestYear)
		{
			from3 = dailyLevelRequestYear;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.dailyLevelRequestMonth)
		{
			from3 = dailyLevelRequestMonth;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.versionHttpRequest)
		{
			value = VariantUtils.CreateFrom(in versionHttpRequest);
			return true;
		}
		if (name == PropertyName.dailyLevelHTTPRequest)
		{
			value = VariantUtils.CreateFrom(in dailyLevelHTTPRequest);
			return true;
		}
		if (name == PropertyName.onlineLevelHTTPRequest)
		{
			value = VariantUtils.CreateFrom(in onlineLevelHTTPRequest);
			return true;
		}
		if (name == PropertyName._onlineLevelStatistics)
		{
			value = VariantUtils.CreateFrom(in _onlineLevelStatistics);
			return true;
		}
		if (name == PropertyName.workshopTagsHTTPRequest)
		{
			value = VariantUtils.CreateFrom(in workshopTagsHTTPRequest);
			return true;
		}
		if (name == PropertyName.exportFileHTTPRequest)
		{
			value = VariantUtils.CreateFrom(in exportFileHTTPRequest);
			return true;
		}
		if (name == PropertyName.loadFileHTTPRequest)
		{
			value = VariantUtils.CreateFrom(in loadFileHTTPRequest);
			return true;
		}
		if (name == PropertyName.dailyLevelRequesting)
		{
			value = VariantUtils.CreateFrom(in dailyLevelRequesting);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.versionHttpRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.dailyLevelHTTPRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.onlineLevelHTTPRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._onlineLevelStatistics, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.workshopTagsHTTPRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.exportFileHTTPRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.loadFileHTTPRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.versionGetOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.newVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.versionMessage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.dailyLevelGetOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.dailyLevelLoadedMonths, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.dailyLevelRequestYear, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.dailyLevelRequestMonth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.dailyLevelRequesting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.versionGetOver, Variant.From<bool>(versionGetOver));
		info.AddProperty(PropertyName.newVersion, Variant.From<string>(newVersion));
		info.AddProperty(PropertyName.versionMessage, Variant.From<string>(versionMessage));
		info.AddProperty(PropertyName.dailyLevelGetOver, Variant.From<bool>(dailyLevelGetOver));
		info.AddProperty(PropertyName.dailyLevelLoadedMonths, Variant.From<Dictionary>(dailyLevelLoadedMonths));
		info.AddProperty(PropertyName.dailyLevelRequestYear, Variant.From<int>(dailyLevelRequestYear));
		info.AddProperty(PropertyName.dailyLevelRequestMonth, Variant.From<int>(dailyLevelRequestMonth));
		info.AddProperty(PropertyName.versionHttpRequest, Variant.From(in versionHttpRequest));
		info.AddProperty(PropertyName.dailyLevelHTTPRequest, Variant.From(in dailyLevelHTTPRequest));
		info.AddProperty(PropertyName.onlineLevelHTTPRequest, Variant.From(in onlineLevelHTTPRequest));
		info.AddProperty(PropertyName._onlineLevelStatistics, Variant.From(in _onlineLevelStatistics));
		info.AddProperty(PropertyName.workshopTagsHTTPRequest, Variant.From(in workshopTagsHTTPRequest));
		info.AddProperty(PropertyName.exportFileHTTPRequest, Variant.From(in exportFileHTTPRequest));
		info.AddProperty(PropertyName.loadFileHTTPRequest, Variant.From(in loadFileHTTPRequest));
		info.AddProperty(PropertyName.dailyLevelRequesting, Variant.From(in dailyLevelRequesting));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.versionGetOver, out var value))
		{
			versionGetOver = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.newVersion, out var value2))
		{
			newVersion = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.versionMessage, out var value3))
		{
			versionMessage = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.dailyLevelGetOver, out var value4))
		{
			dailyLevelGetOver = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dailyLevelLoadedMonths, out var value5))
		{
			dailyLevelLoadedMonths = value5.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.dailyLevelRequestYear, out var value6))
		{
			dailyLevelRequestYear = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.dailyLevelRequestMonth, out var value7))
		{
			dailyLevelRequestMonth = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.versionHttpRequest, out var value8))
		{
			versionHttpRequest = value8.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.dailyLevelHTTPRequest, out var value9))
		{
			dailyLevelHTTPRequest = value9.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.onlineLevelHTTPRequest, out var value10))
		{
			onlineLevelHTTPRequest = value10.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName._onlineLevelStatistics, out var value11))
		{
			_onlineLevelStatistics = value11.As<OnlineLevelStatisticsReporter>();
		}
		if (info.TryGetProperty(PropertyName.workshopTagsHTTPRequest, out var value12))
		{
			workshopTagsHTTPRequest = value12.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.exportFileHTTPRequest, out var value13))
		{
			exportFileHTTPRequest = value13.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.loadFileHTTPRequest, out var value14))
		{
			loadFileHTTPRequest = value14.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.dailyLevelRequesting, out var value15))
		{
			dailyLevelRequesting = value15.As<bool>();
		}
	}
}
