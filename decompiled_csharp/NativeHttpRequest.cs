using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Core/Networking/NativeHttpRequest.cs")]
public class NativeHttpRequest : Node
{
	public enum Result
	{
		Success,
		CantConnect,
		CantResolve,
		ConnectionError,
		TlsHandshakeError,
		NoResponse,
		RequestFailed,
		RedirectLimitReached,
		ResponseTooLarge
	}

	private readonly record struct CompletedRequest(long Generation, Result Result, long ResponseCode, string[] Headers, byte[] Body);

	private sealed class ResponseTooLargeException : IOException
	{
	}

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Request = "Request";

		public static readonly StringName CancelRequest = "CancelRequest";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName EnqueueFailure = "EnqueueFailure";

		public static readonly StringName MapRequestError = "MapRequestError";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName FollowRedirects = "FollowRedirects";

		public static readonly StringName _requestGeneration = "_requestGeneration";

		public static readonly StringName _disposed = "_disposed";
	}

	public new class SignalName : Node.SignalName
	{
	}

	public static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(30L);

	public const int MaxBodyBytes = 33554432;

	private static readonly System.Net.Http.HttpClient Client = CreateClient(allowAutoRedirect: true);

	private static readonly System.Net.Http.HttpClient NoRedirectClient = CreateClient(allowAutoRedirect: false);

	private readonly ConcurrentQueue<CompletedRequest> _completedRequests = new ConcurrentQueue<CompletedRequest>();

	private readonly object _requestLock = new object();

	private CancellationTokenSource _requestCancellation;

	private long _requestGeneration;

	private bool _disposed;

	public bool FollowRedirects { get; set; } = true;

	public event Action<long, long, string[], byte[]> RequestCompleted;

	public NativeHttpRequest()
	{
		ProcessMode = ProcessModeEnum.Always;
	}

	public Error Request(string url, string[] headers)
	{
		return StartRequest(url, headers, HttpMethod.Get, null);
	}

	public Error Request(string url, string[] headers, HttpMethod method)
	{
		return StartRequest(url, headers, method, null);
	}

	public Error RequestRaw(string url, string[] headers, HttpMethod method, byte[] body)
	{
		return StartRequest(url, headers, method, body);
	}

	public void CancelRequest()
	{
		CancellationTokenSource requestCancellation;
		lock (_requestLock)
		{
			_requestGeneration++;
			requestCancellation = _requestCancellation;
			_requestCancellation = null;
		}
		CancelSafely(requestCancellation);
	}

	public override void _Process(double delta)
	{
		CompletedRequest result;
		while (_completedRequests.TryDequeue(out result))
		{
			lock (_requestLock)
			{
				if (_disposed || result.Generation != _requestGeneration)
				{
					continue;
				}
				_requestCancellation = null;
				goto IL_004c;
			}
			IL_004c:
			RequestCompleted?.Invoke((long)result.Result, result.ResponseCode, result.Headers, result.Body);
		}
	}

	public override void _ExitTree()
	{
		lock (_requestLock)
		{
			_disposed = true;
		}
		CancelRequest();
		RequestCompleted = null;
	}

	private static System.Net.Http.HttpClient CreateClient(bool allowAutoRedirect)
	{
		return new System.Net.Http.HttpClient(new SocketsHttpHandler
		{
			AllowAutoRedirect = allowAutoRedirect,
			MaxAutomaticRedirections = 8,
			AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli),
			PooledConnectionLifetime = TimeSpan.FromMinutes(10L)
		}, disposeHandler: true)
		{
			Timeout = RequestTimeout
		};
	}

	private Error StartRequest(string url, string[] headers, HttpMethod method, byte[] body)
	{
		if (!Uri.TryCreate(url, UriKind.Absolute, out Uri result) || (result.Scheme != Uri.UriSchemeHttp && result.Scheme != Uri.UriSchemeHttps))
		{
			return Error.InvalidParameter;
		}
		if (body != null && body.Length > 33554432)
		{
			return Error.InvalidParameter;
		}
		CancellationTokenSource requestCancellation;
		CancellationTokenSource cancellation;
		long generation;
		lock (_requestLock)
		{
			if (_disposed)
			{
				return Error.Unavailable;
			}
			requestCancellation = _requestCancellation;
			cancellation = (_requestCancellation = new CancellationTokenSource());
			generation = ++_requestGeneration;
		}
		CancelSafely(requestCancellation);
		System.Net.Http.HttpClient client = (FollowRedirects ? Client : NoRedirectClient);
		SendAsync(generation, result, headers, method, body, client, cancellation);
		return Error.Ok;
	}

	private async Task SendAsync(long generation, Uri url, string[] headers, HttpMethod method, byte[] body, System.Net.Http.HttpClient client, CancellationTokenSource cancellation)
	{
		using CancellationTokenSource timeoutCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellation.Token);
		timeoutCancellation.CancelAfter(RequestTimeout);
		try
		{
			using HttpRequestMessage request = new HttpRequestMessage(method, url);
			if (body != null || method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch)
			{
				request.Content = new ByteArrayContent(body ?? Array.Empty<byte>());
			}
			ApplyHeaders(request, headers);
			using HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeoutCancellation.Token);
			byte[] body2 = await ReadResponseBodyAsync(response.Content, timeoutCancellation.Token);
			EnqueueCompletion(new CompletedRequest(generation, Result.Success, (long)response.StatusCode, GetResponseHeaders(response), body2));
		}
		catch (OperationCanceledException) when (!cancellation.IsCancellationRequested)
		{
			EnqueueFailure(generation, Result.NoResponse);
		}
		catch (OperationCanceledException)
		{
		}
		catch (ResponseTooLargeException)
		{
			EnqueueFailure(generation, Result.ResponseTooLarge);
		}
		catch (HttpRequestException ex4)
		{
			EnqueueFailure(generation, MapRequestError(ex4.HttpRequestError));
		}
		catch (UriFormatException)
		{
			EnqueueFailure(generation, Result.RequestFailed);
		}
		catch (IOException)
		{
			EnqueueFailure(generation, Result.ConnectionError);
		}
		catch (Exception)
		{
			EnqueueFailure(generation, Result.RequestFailed);
		}
		finally
		{
			lock (_requestLock)
			{
				if (_requestCancellation == cancellation)
				{
					_requestCancellation = null;
				}
			}
			cancellation.Dispose();
		}
	}

	private static async Task<byte[]> ReadResponseBodyAsync(HttpContent content, CancellationToken cancellationToken)
	{
		long? contentLength = content.Headers.ContentLength;
		if (contentLength.HasValue && contentLength.Value > 33554432)
		{
			throw new ResponseTooLargeException();
		}
		int initialCapacity = (int)(contentLength.HasValue ? Math.Min(contentLength.Value, 33554432L) : 0);
		using Stream responseStream = await content.ReadAsStreamAsync(cancellationToken);
		using MemoryStream output = new MemoryStream(initialCapacity);
		byte[] buffer = ArrayPool<byte>.Shared.Rent(81920);
		try
		{
			while (true)
			{
				int num = await responseStream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken);
				if (num == 0)
				{
					break;
				}
				if (output.Length + num > 33554432)
				{
					throw new ResponseTooLargeException();
				}
				output.Write(buffer, 0, num);
			}
			return output.ToArray();
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(buffer);
		}
	}

	private static void CancelSafely(CancellationTokenSource cancellation)
	{
		if (cancellation == null)
		{
			return;
		}
		try
		{
			cancellation.Cancel();
		}
		catch (ObjectDisposedException)
		{
		}
	}

	private void EnqueueFailure(long generation, Result result)
	{
		EnqueueCompletion(new CompletedRequest(generation, result, 0L, Array.Empty<string>(), Array.Empty<byte>()));
	}

	private void EnqueueCompletion(CompletedRequest completed)
	{
		lock (_requestLock)
		{
			if (_disposed || completed.Generation != _requestGeneration)
			{
				return;
			}
		}
		_completedRequests.Enqueue(completed);
	}

	private static void ApplyHeaders(HttpRequestMessage request, string[] headers)
	{
		if (headers == null)
		{
			return;
		}
		foreach (string text in headers)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				continue;
			}
			int num = text.IndexOf(':');
			if (num > 0)
			{
				string text2 = text.Substring(0, num).Trim();
				string text3 = text;
				int num2 = num + 1;
				string value = text3.Substring(num2, text3.Length - num2).Trim();
				if (request.Content != null && text2.StartsWith("Content-", StringComparison.OrdinalIgnoreCase))
				{
					request.Content.Headers.TryAddWithoutValidation(text2, value);
				}
				else
				{
					request.Headers.TryAddWithoutValidation(text2, value);
				}
			}
		}
	}

	private static string[] GetResponseHeaders(HttpResponseMessage response)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
		{
			list.Add(header.Key + ": " + string.Join(", ", header.Value));
		}
		foreach (KeyValuePair<string, IEnumerable<string>> header2 in response.Content.Headers)
		{
			list.Add(header2.Key + ": " + string.Join(", ", header2.Value));
		}
		return list.ToArray();
	}

	private static Result MapRequestError(HttpRequestError error)
	{
		return error switch
		{
			HttpRequestError.NameResolutionError => Result.CantResolve, 
			HttpRequestError.ConnectionError => Result.CantConnect, 
			HttpRequestError.SecureConnectionError => Result.TlsHandshakeError, 
			HttpRequestError.InvalidResponse => Result.NoResponse, 
			HttpRequestError.ResponseEnded => Result.ConnectionError, 
			HttpRequestError.ConfigurationLimitExceeded => Result.RedirectLimitReached, 
			_ => Result.RequestFailed, 
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.Request, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "url", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnqueueFailure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "generation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MapRequestError, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Request && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Error>(Request(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.CancelRequest && args.Count == 0)
		{
			CancelRequest();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.EnqueueFailure && args.Count == 2)
		{
			EnqueueFailure(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<Result>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MapRequestError && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Result>(MapRequestError(VariantUtils.ConvertTo<HttpRequestError>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.MapRequestError && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Result>(MapRequestError(VariantUtils.ConvertTo<HttpRequestError>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Request)
		{
			return true;
		}
		if (method == MethodName.CancelRequest)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.EnqueueFailure)
		{
			return true;
		}
		if (method == MethodName.MapRequestError)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.FollowRedirects)
		{
			FollowRedirects = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._requestGeneration)
		{
			_requestGeneration = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._disposed)
		{
			_disposed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.FollowRedirects)
		{
			value = VariantUtils.CreateFrom<bool>(FollowRedirects);
			return true;
		}
		if (name == PropertyName._requestGeneration)
		{
			value = VariantUtils.CreateFrom(in _requestGeneration);
			return true;
		}
		if (name == PropertyName._disposed)
		{
			value = VariantUtils.CreateFrom(in _disposed);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.FollowRedirects, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._requestGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._disposed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.FollowRedirects, Variant.From<bool>(FollowRedirects));
		info.AddProperty(PropertyName._requestGeneration, Variant.From(in _requestGeneration));
		info.AddProperty(PropertyName._disposed, Variant.From(in _disposed));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.FollowRedirects, out var value))
		{
			FollowRedirects = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._requestGeneration, out var value2))
		{
			_requestGeneration = value2.As<long>();
		}
		if (info.TryGetProperty(PropertyName._disposed, out var value3))
		{
			_disposed = value3.As<bool>();
		}
	}
}
