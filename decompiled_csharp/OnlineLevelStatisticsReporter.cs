using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using Microsoft.Extensions.Logging;
using ZLogger;

[ScriptPath("res://Core/InternetServerManager/OnlineLevelStatisticsReporter.cs")]
public sealed class OnlineLevelStatisticsReporter : Node
{
	internal enum Outcome
	{
		Success,
		Exhausted,
		Rejected,
		Uncertain
	}

	internal readonly record struct ReportResult(long EventId, string LevelId, string Statistic, int Attempts, Outcome Outcome, long TransportResult, long StatusCode, Error StartError);

	private sealed record PendingReport(long EventId, string LevelId, string Statistic, string Url, string[] Headers);

	private enum State
	{
		Idle,
		Sending,
		WaitingToRetry,
		Stopped
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Enqueue = "Enqueue";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName Pump = "Pump";

		public static readonly StringName StartAttempt = "StartAttempt";

		public static readonly StringName CompleteAttempt = "CompleteAttempt";

		public static readonly StringName Finish = "Finish";

		public static readonly StringName DetachCompletion = "DetachCompletion";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _levelsUrl = "_levelsUrl";

		public static readonly StringName _request = "_request";

		public static readonly StringName _state = "_state";

		public static readonly StringName _attempts = "_attempts";

		public static readonly StringName _eventSequence = "_eventSequence";

		public static readonly StringName _attemptSequence = "_attemptSequence";

		public static readonly StringName _retryAt = "_retryAt";

		public static readonly StringName _ready = "_ready";
	}

	public new class SignalName : Node.SignalName
	{
	}

	internal const int MaxAttempts = 3;

	internal const ulong RetryDelayMilliseconds = 10000uL;

	private const string LevelsUrl = "https://api.pvzhe.com/workshop/levels";

	private static readonly ILogger Logger = Log.CreateLogger<OnlineLevelStatisticsReporter>();

	private readonly Queue<PendingReport> _pending = new Queue<PendingReport>();

	private readonly string _levelsUrl;

	private readonly Func<ulong> _milliseconds;

	private readonly Func<string, string[], Action<long, long>, Error> _sendOverride;

	private readonly Action _cancelOverride;

	private NativeHttpRequest _request;

	private Action<long, long, string[], byte[]> _requestCompleted;

	private PendingReport _current;

	private State _state;

	private int _attempts;

	private long _eventSequence;

	private long _attemptSequence;

	private ulong _retryAt;

	private bool _ready;

	internal event Action<ReportResult> ReportCompleted;

	public OnlineLevelStatisticsReporter()
		: this("https://api.pvzhe.com/workshop/levels")
	{
	}

	internal OnlineLevelStatisticsReporter(string levelsUrl, Func<ulong> milliseconds = null, Func<string, string[], Action<long, long>, Error> send = null, Action cancel = null)
	{
		_levelsUrl = levelsUrl.TrimEnd('/');
		_milliseconds = milliseconds ?? ((Func<ulong>)(() => Time.GetTicksMsec()));
		_sendOverride = send;
		_cancelOverride = cancel;
		ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Ready()
	{
		if (_sendOverride == null)
		{
			_request = new NativeHttpRequest
			{
				FollowRedirects = false
			};
			AddChild(_request, forceReadableName: false, InternalMode.Disabled);
		}
		_ready = true;
		Pump();
	}

	public void Enqueue(string levelId, string statistic, string[] headers)
	{
		if (_state == State.Stopped)
		{
			return;
		}
		bool enabled = string.IsNullOrWhiteSpace(levelId);
		if (!enabled)
		{
			bool flag;
			switch (statistic)
			{
			case "abandon":
			case "completion":
			case "failure":
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
			enabled = !flag;
		}
		if (enabled)
		{
			ILogger logger = Logger;
			ZLoggerWarningInterpolatedStringHandler message = new ZLoggerWarningInterpolatedStringHandler(30, 2, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("在线关卡统计参数无效: level=");
				message.AppendFormatted(levelId, 0, null, "levelId");
				message.AppendLiteral(", statistic=");
				message.AppendFormatted(statistic, 0, null, "statistic");
			}
			logger.ZLogWarning(ref message);
		}
		else
		{
			string[] headers2 = ((headers == null) ? Array.Empty<string>() : ((string[])headers.Clone()));
			_pending.Enqueue(new PendingReport(++_eventSequence, levelId, statistic, $"{_levelsUrl}/{Uri.EscapeDataString(levelId)}/{statistic}", headers2));
			Pump();
		}
	}

	public override void _Process(double delta)
	{
		Pump();
	}

	internal void Pump()
	{
		if (!_ready || _state == State.Stopped || _state == State.Sending)
		{
			return;
		}
		if (_state == State.WaitingToRetry)
		{
			if (_milliseconds() >= _retryAt)
			{
				StartAttempt();
			}
		}
		else if (_pending.TryDequeue(out _current))
		{
			_attempts = 0;
			StartAttempt();
		}
	}

	private void StartAttempt()
	{
		_state = State.Sending;
		_attempts++;
		long attemptId = ++_attemptSequence;
		Action<long, long> completed = (long result, long status) =>
		{
			CompleteAttempt(attemptId, result, status);
		};
		Error error;
		if (_sendOverride != null)
		{
			error = _sendOverride(_current.Url, _current.Headers, completed);
		}
		else
		{
			_requestCompleted = (long result, long status, string[] headers, byte[] body) =>
			{
				completed(result, status);
			};
			_request.RequestCompleted += _requestCompleted;
			error = _request.Request(_current.Url, _current.Headers, HttpMethod.Post);
		}
		if (error != Error.Ok && _state == State.Sending && attemptId == _attemptSequence)
		{
			DetachCompletion();
			Finish(Outcome.Rejected, 6L, 0L, error);
		}
	}

	private void CompleteAttempt(long attemptId, long result, long status)
	{
		if (_state != State.Sending || attemptId != _attemptSequence)
		{
			return;
		}
		DetachCompletion();
		if (result == 0L && status >= 200 && status <= 299)
		{
			Finish(Outcome.Success, result, status, Error.Ok);
			return;
		}
		bool flag = result == 1 || result == 2 || result == 4 || (result == 0L && status >= 500 && status <= 599);
		if (flag && _attempts < 3)
		{
			_retryAt = _milliseconds() + 10000;
			_state = State.WaitingToRetry;
			ILogger logger = Logger;
			ZLoggerInformationInterpolatedStringHandler message = new ZLoggerInformationInterpolatedStringHandler(75, 8, logger, out var enabled);
			if (enabled)
			{
				message.AppendLiteral("在线关卡统计等待重试: event=");
				message.AppendFormatted(_current.EventId, 0, null, "_current.EventId");
				message.AppendLiteral(", level=");
				message.AppendFormatted(_current.LevelId, 0, null, "_current.LevelId");
				message.AppendLiteral(", statistic=");
				message.AppendFormatted(_current.Statistic, 0, null, "_current.Statistic");
				message.AppendLiteral(", attempt=");
				message.AppendFormatted(_attempts, 0, null, "_attempts");
				message.AppendLiteral("/");
				message.AppendFormatted(3, 0, null, "MaxAttempts");
				message.AppendLiteral(", result=");
				message.AppendFormatted(result, 0, null, "result");
				message.AppendLiteral(", http=");
				message.AppendFormatted(status, 0, null, "status");
				message.AppendLiteral(", delayMs=");
				message.AppendFormatted(10000uL, 0, null, "RetryDelayMilliseconds");
			}
			logger.ZLogInformation(ref message);
		}
		else
		{
			Outcome outcome = (flag ? Outcome.Exhausted : ((result == 0L && status >= 300 && status <= 499) ? Outcome.Rejected : Outcome.Uncertain));
			Finish(outcome, result, status, Error.Ok);
		}
	}

	private void Finish(Outcome outcome, long result, long status, Error startError = Error.Ok)
	{
		ReportResult obj = new ReportResult(_current.EventId, _current.LevelId, _current.Statistic, _attempts, outcome, result, status, startError);
		_current = null;
		_state = State.Idle;
		ILogger logger = Logger;
		ZLoggerInformationInterpolatedStringHandler message = new ZLoggerInformationInterpolatedStringHandler(86, 8, logger, out var enabled);
		if (enabled)
		{
			message.AppendLiteral("在线关卡统计结束: event=");
			message.AppendFormatted(obj.EventId, 0, null, "report.EventId");
			message.AppendLiteral(", level=");
			message.AppendFormatted(obj.LevelId, 0, null, "report.LevelId");
			message.AppendLiteral(", statistic=");
			message.AppendFormatted(obj.Statistic, 0, null, "report.Statistic");
			message.AppendLiteral(", attempts=");
			message.AppendFormatted(obj.Attempts, 0, null, "report.Attempts");
			message.AppendLiteral(", outcome=");
			message.AppendFormatted(outcome, 0, null, "outcome");
			message.AppendLiteral(", result=");
			message.AppendFormatted(result, 0, null, "result");
			message.AppendLiteral(", http=");
			message.AppendFormatted(status, 0, null, "status");
			message.AppendLiteral(", startError=");
			message.AppendFormatted(startError, 0, null, "startError");
		}
		logger.ZLogInformation(ref message);
		ReportCompleted?.Invoke(obj);
	}

	private void DetachCompletion()
	{
		if (_requestCompleted != null && GodotObject.IsInstanceValid(_request))
		{
			_request.RequestCompleted -= _requestCompleted;
		}
		_requestCompleted = null;
	}

	public override void _ExitTree()
	{
		if (_state != State.Stopped)
		{
			_state = State.Stopped;
			_ready = false;
			_pending.Clear();
			_current = null;
			DetachCompletion();
			if (GodotObject.IsInstanceValid(_request))
			{
				_request.CancelRequest();
			}
			_cancelOverride?.Invoke();
			ReportCompleted = null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Enqueue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "statistic", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Pump, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartAttempt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteAttempt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "attemptId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "status", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "outcome", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "status", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "startError", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DetachCompletion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Enqueue && args.Count == 3)
		{
			Enqueue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Pump && args.Count == 0)
		{
			Pump();
			ret = default;
			return true;
		}
		if (method == MethodName.StartAttempt && args.Count == 0)
		{
			StartAttempt();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteAttempt && args.Count == 3)
		{
			CompleteAttempt(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<long>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 4)
		{
			Finish(VariantUtils.ConvertTo<Outcome>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<long>(in args[2]), VariantUtils.ConvertTo<Error>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DetachCompletion && args.Count == 0)
		{
			DetachCompletion();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
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
		if (method == MethodName.Enqueue)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.Pump)
		{
			return true;
		}
		if (method == MethodName.StartAttempt)
		{
			return true;
		}
		if (method == MethodName.CompleteAttempt)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		if (method == MethodName.DetachCompletion)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._request)
		{
			_request = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName._state)
		{
			_state = VariantUtils.ConvertTo<State>(in value);
			return true;
		}
		if (name == PropertyName._attempts)
		{
			_attempts = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._eventSequence)
		{
			_eventSequence = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._attemptSequence)
		{
			_attemptSequence = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._retryAt)
		{
			_retryAt = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._ready)
		{
			_ready = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._levelsUrl)
		{
			value = VariantUtils.CreateFrom(in _levelsUrl);
			return true;
		}
		if (name == PropertyName._request)
		{
			value = VariantUtils.CreateFrom(in _request);
			return true;
		}
		if (name == PropertyName._state)
		{
			value = VariantUtils.CreateFrom(in _state);
			return true;
		}
		if (name == PropertyName._attempts)
		{
			value = VariantUtils.CreateFrom(in _attempts);
			return true;
		}
		if (name == PropertyName._eventSequence)
		{
			value = VariantUtils.CreateFrom(in _eventSequence);
			return true;
		}
		if (name == PropertyName._attemptSequence)
		{
			value = VariantUtils.CreateFrom(in _attemptSequence);
			return true;
		}
		if (name == PropertyName._retryAt)
		{
			value = VariantUtils.CreateFrom(in _retryAt);
			return true;
		}
		if (name == PropertyName._ready)
		{
			value = VariantUtils.CreateFrom(in _ready);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._levelsUrl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._request, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._state, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._attempts, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._eventSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._attemptSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._retryAt, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ready, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._request, Variant.From(in _request));
		info.AddProperty(PropertyName._state, Variant.From(in _state));
		info.AddProperty(PropertyName._attempts, Variant.From(in _attempts));
		info.AddProperty(PropertyName._eventSequence, Variant.From(in _eventSequence));
		info.AddProperty(PropertyName._attemptSequence, Variant.From(in _attemptSequence));
		info.AddProperty(PropertyName._retryAt, Variant.From(in _retryAt));
		info.AddProperty(PropertyName._ready, Variant.From(in _ready));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._request, out var value))
		{
			_request = value.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName._state, out var value2))
		{
			_state = value2.As<State>();
		}
		if (info.TryGetProperty(PropertyName._attempts, out var value3))
		{
			_attempts = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._eventSequence, out var value4))
		{
			_eventSequence = value4.As<long>();
		}
		if (info.TryGetProperty(PropertyName._attemptSequence, out var value5))
		{
			_attemptSequence = value5.As<long>();
		}
		if (info.TryGetProperty(PropertyName._retryAt, out var value6))
		{
			_retryAt = value6.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._ready, out var value7))
		{
			_ready = value7.As<bool>();
		}
	}
}
