using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentTlsHandshakeRuntimeTest.cs")]
public class BugDepartmentTlsHandshakeRuntimeTest : Node
{
	private readonly record struct ResponseSnapshot(long Result, long ResponseCode, string[] Headers, byte[] Body);

	private readonly record struct AttemptSnapshot(Error RequestError, bool Completed, ResponseSnapshot Response);

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnVersionRequestCompleted = "OnVersionRequestCompleted";

		public static readonly StringName IsRetryableNetworkFailure = "IsRetryableNetworkFailure";

		public static readonly StringName IsProductionVersionJson = "IsProductionVersionJson";

		public static readonly StringName Check = "Check";

		public static readonly StringName Require = "Require";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ProductionEndpoint = "https://api.pvzhe.com/new_version";

	private const string ResultMarker = "BUG_DEPARTMENT_TLS_HANDSHAKE_RESULT";

	private const ulong AttemptTimeoutMilliseconds = 35000uL;

	private int _checks;

	private int _failures;

	private TaskCompletionSource<ResponseSnapshot> _responseCompletion;

	public override async void _Ready()
	{
		InternetServerManager manager = InternetServerManager.Instance;
		NativeHttpRequest request = manager?.versionHttpRequest;
		AttemptSnapshot finalAttempt = default;
		int attempts = 0;
		string terminalStatus = "Failed";
		try
		{
			_ = 4;
			try
			{
				bool condition = GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(Global.Instance) && GodotObject.IsInstanceValid(request);
				Check(condition, "InternetServerManager, Global and the production native HTTP request must be available.");
				Require(condition, "Required production autoload state is unavailable.");
				Check(request.GetParent() == manager, "The test must use InternetServerManager.Instance.versionHttpRequest itself.");
				BugDepartmentTlsHandshakeRuntimeTest bugDepartmentTlsHandshakeRuntimeTest = this;
				string[] header = Global.Instance.header;
				bugDepartmentTlsHandshakeRuntimeTest.Check(header != null && header.Length > 0 && Array.Exists(Global.Instance.header, (string text) => text.StartsWith("X-PVZHE-Client-Version:", StringComparison.OrdinalIgnoreCase)), "The real production client-version header must be initialized.");
				request.CancelRequest();
				await WaitProcessFrames(2);
				manager.versionGetOver = false;
				manager.newVersion = string.Empty;
				request.RequestCompleted += OnVersionRequestCompleted;
				finalAttempt = await RunAttempt(request);
				attempts++;
				if (IsRetryableNetworkFailure(finalAttempt))
				{
					await WaitProcessFrames(2);
					manager.versionGetOver = false;
					manager.newVersion = string.Empty;
					finalAttempt = await RunAttempt(request);
					attempts++;
				}
				if (attempts == 2 && IsRetryableNetworkFailure(finalAttempt))
				{
					terminalStatus = "Inconclusive";
					_failures++;
				}
				else
				{
					Check(finalAttempt.RequestError == Error.Ok, $"The production request call must be accepted; error={finalAttempt.RequestError}.");
					Check(finalAttempt.Completed, "The production endpoint must complete before the per-attempt timeout.");
					Require(finalAttempt.RequestError == Error.Ok && finalAttempt.Completed, "The production request did not reach a completed response.");
					long result = finalAttempt.Response.Result;
					Check(result != 4, "The production endpoint must not fail with TlsHandshakeError.");
					Check(result == 0, $"The production endpoint must complete successfully; result={(NativeHttpRequest.Result)result}.");
					Check(finalAttempt.Response.ResponseCode == 200, $"The production endpoint must return HTTP 200; code={finalAttempt.Response.ResponseCode}.");
					Check(IsProductionVersionJson(finalAttempt.Response.Body), "The HTTP 200 body must contain the production platform/version JSON contract.");
					await WaitProcessFrames(2);
					Check(manager.versionGetOver, "The production handler must set versionGetOver after parsing the response.");
					Check(!string.IsNullOrWhiteSpace(manager.newVersion), "The production handler must publish a non-empty newVersion.");
					terminalStatus = ((_failures == 0) ? "Passed" : "Failed");
				}
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentTlsHandshakeRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(request))
			{
				request.RequestCompleted -= OnVersionRequestCompleted;
				request.CancelRequest();
			}
			if (GodotObject.IsInstanceValid(manager?.dailyLevelHTTPRequest))
			{
				manager.dailyLevelHTTPRequest.CancelRequest();
			}
			await WaitProcessFrames(3);
			await ToSignal(GetTree().CreateTimer(0.25), SceneTreeTimer.SignalName.Timeout);
		}
		if (terminalStatus == "Inconclusive")
		{
			GD.Print($"{"BUG_DEPARTMENT_TLS_HANDSHAKE_RESULT"} status=Inconclusive passed=False checks={_checks} failures={_failures} attempts={attempts} result={FormatResult(finalAttempt)} reason=network_unavailable");
			GetTree().Quit(3);
		}
		else
		{
			bool flag = terminalStatus == "Passed" && _failures == 0 && _checks == 11;
			GD.Print($"{"BUG_DEPARTMENT_TLS_HANDSHAKE_RESULT"} status={terminalStatus} passed={flag} checks={_checks} failures={_failures} attempts={attempts} result={FormatResult(finalAttempt)} http={finalAttempt.Response.ResponseCode}");
			GetTree().Quit((!flag) ? 2 : 0);
		}
	}

	private async Task<AttemptSnapshot> RunAttempt(NativeHttpRequest request)
	{
		request.CancelRequest();
		_responseCompletion = new TaskCompletionSource<ResponseSnapshot>(TaskCreationOptions.RunContinuationsAsynchronously);
		Error requestError = request.Request("https://api.pvzhe.com/new_version", Global.Instance.header);
		if (requestError != Error.Ok)
		{
			return new AttemptSnapshot(requestError, Completed: false, default);
		}
		ulong deadline = Time.GetTicksMsec() + 35000;
		while (!_responseCompletion.Task.IsCompleted && Time.GetTicksMsec() < deadline)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		if (!_responseCompletion.Task.IsCompleted)
		{
			request.CancelRequest();
			return new AttemptSnapshot(requestError, Completed: false, default);
		}
		Error requestError2 = requestError;
		return new AttemptSnapshot(requestError2, Completed: true, await _responseCompletion.Task);
	}

	private void OnVersionRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		_responseCompletion?.TrySetResult(new ResponseSnapshot(result, responseCode, headers ?? Array.Empty<string>(), body ?? Array.Empty<byte>()));
	}

	private static bool IsRetryableNetworkFailure(long result)
	{
		if (result != 1)
		{
			return result == 2;
		}
		return true;
	}

	private static bool IsRetryableNetworkFailure(AttemptSnapshot attempt)
	{
		if (attempt.Completed)
		{
			return IsRetryableNetworkFailure(attempt.Response.Result);
		}
		if (attempt.RequestError != Error.CantConnect)
		{
			return attempt.RequestError == Error.CantResolve;
		}
		return true;
	}

	private static bool IsProductionVersionJson(byte[] body)
	{
		if (body == null || body.Length <= 0)
		{
			return false;
		}
		try
		{
			using JsonDocument jsonDocument = JsonDocument.Parse(body);
			JsonElement rootElement = jsonDocument.RootElement;
			string propertyName = (Global.IsMobile ? "android" : "pc");
			JsonElement value;
			JsonElement value2;
			return rootElement.ValueKind == JsonValueKind.Object && rootElement.TryGetProperty(propertyName, out value) && value.ValueKind == JsonValueKind.Object && value.TryGetProperty("version", out value2) && value2.ValueKind == JsonValueKind.Array && value2.GetArrayLength() > 0;
		}
		catch (JsonException)
		{
			return false;
		}
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private static string FormatResult(AttemptSnapshot attempt)
	{
		if (!attempt.Completed)
		{
			if (attempt.RequestError != Error.Ok)
			{
				return $"RequestError:{attempt.RequestError}";
			}
			return "Timeout";
		}
		return ((NativeHttpRequest.Result)attempt.Response.Result/*cast due to constrained. prefix*/).ToString();
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugDepartmentTlsHandshakeRuntimeTest] " + message);
		}
	}

	private static void Require(bool condition, string message)
	{
		if (!condition)
		{
			throw new InvalidOperationException(message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVersionRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRetryableNetworkFailure, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsProductionVersionJson, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OnVersionRequestCompleted && args.Count == 4)
		{
			OnVersionRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsRetryableNetworkFailure && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRetryableNetworkFailure(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.IsProductionVersionJson && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProductionVersionJson(VariantUtils.ConvertTo<byte[]>(in args[0])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsRetryableNetworkFailure && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRetryableNetworkFailure(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.IsProductionVersionJson && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProductionVersionJson(VariantUtils.ConvertTo<byte[]>(in args[0])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.OnVersionRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.IsRetryableNetworkFailure)
		{
			return true;
		}
		if (method == MethodName.IsProductionVersionJson)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Require)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
