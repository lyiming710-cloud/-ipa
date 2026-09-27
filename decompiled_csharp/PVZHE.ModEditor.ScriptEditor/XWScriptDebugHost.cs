using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ScriptEditor;

[ScriptPath("res://addons/ModEditor/ScriptEditor/Compiler/XWScriptDebugHost.cs")]
public class XWScriptDebugHost : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName GetOrCreate = "GetOrCreate";

		public static readonly StringName CreatePreviewRoot = "CreatePreviewRoot";

		public static readonly StringName DrainMainThreadQueue = "DrainMainThreadQueue";

		public static readonly StringName HasPendingWork = "HasPendingWork";

		public static readonly StringName RequestProcessing = "RequestProcessing";

		public static readonly StringName FreePreviewRoot = "FreePreviewRoot";

		public static readonly StringName EnsureMainThread = "EnsureMainThread";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName MainThreadId = "MainThreadId";

		public static readonly StringName OwnedCleanupCount = "OwnedCleanupCount";

		public static readonly StringName PreviewRootCount = "PreviewRootCount";

		public static readonly StringName _mainThreadId = "_mainThreadId";

		public static readonly StringName _wakeRequestPending = "_wakeRequestPending";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string HostNodeName = "ModEditorScriptDebugHost";

	private readonly object _dispatchLock = new object();

	private readonly Queue<Action> _mainThreadQueue = new Queue<Action>();

	private readonly List<Task> _ownedCleanupTasks = new List<Task>();

	private int _mainThreadId;

	private SynchronizationContext _mainThreadContext;

	private int _wakeRequestPending;

	public int MainThreadId => _mainThreadId;

	public int OwnedCleanupCount => _ownedCleanupTasks.Count;

	public int PreviewRootCount
	{
		get
		{
			int num = 0;
			foreach (Node child in GetChildren())
			{
				if (child.Name.ToString().StartsWith("ScriptDebugPreview_", StringComparison.Ordinal))
				{
					num++;
				}
			}
			return num;
		}
	}

	public override void _Ready()
	{
		_mainThreadId = System.Environment.CurrentManagedThreadId;
		_mainThreadContext = SynchronizationContext.Current;
		ProcessMode = ProcessModeEnum.Always;
		SetProcess(enable: false);
	}

	public override void _Process(double delta)
	{
		DrainMainThreadQueue();
		for (int num = _ownedCleanupTasks.Count - 1; num >= 0; num--)
		{
			Task task = _ownedCleanupTasks[num];
			if (task.IsCompleted)
			{
				if (task.IsFaulted)
				{
					GD.PushError("Script debug cleanup failed: " + task.Exception?.GetBaseException().Message);
				}
				_ownedCleanupTasks.RemoveAt(num);
			}
		}
		if (!HasPendingWork())
		{
			SetProcess(enable: false);
		}
	}

	public static XWScriptDebugHost GetOrCreate(Node requester)
	{
		SceneTree sceneTree = requester?.GetTree();
		if (sceneTree?.Root == null)
		{
			return null;
		}
		XWScriptDebugHost nodeOrNull = sceneTree.Root.GetNodeOrNull<XWScriptDebugHost>("ModEditorScriptDebugHost");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			return nodeOrNull;
		}
		XWScriptDebugHost xWScriptDebugHost = new XWScriptDebugHost
		{
			Name = "ModEditorScriptDebugHost"
		};
		sceneTree.Root.AddChild(xWScriptDebugHost, forceReadableName: false, InternalMode.Disabled);
		return xWScriptDebugHost;
	}

	public Node CreatePreviewRoot()
	{
		EnsureMainThread();
		Node node = new Node
		{
			Name = $"ScriptDebugPreview_{Guid.NewGuid():N}",
			ProcessMode = ProcessModeEnum.Always
		};
		AddChild(node, forceReadableName: false, InternalMode.Disabled);
		return node;
	}

	public Task DispatchAsync(Action action)
	{
		if (action == null)
		{
			return Task.CompletedTask;
		}
		if (System.Environment.CurrentManagedThreadId == _mainThreadId && IsInsideTree())
		{
			action();
			return Task.CompletedTask;
		}
		TaskCompletionSource<bool> completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		lock (_dispatchLock)
		{
			_mainThreadQueue.Enqueue(() =>
			{
				try
				{
					action();
					completion.TrySetResult(result: true);
				}
				catch (Exception exception)
				{
					completion.TrySetException(exception);
				}
			});
		}
		RequestProcessing();
		return completion.Task;
	}

	public Task<T> DispatchAsync<T>(Func<T> action)
	{
		if (action == null)
		{
			return Task.FromResult<T>(default);
		}
		if (System.Environment.CurrentManagedThreadId == _mainThreadId && IsInsideTree())
		{
			return Task.FromResult(action());
		}
		TaskCompletionSource<T> completion = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
		lock (_dispatchLock)
		{
			_mainThreadQueue.Enqueue(() =>
			{
				try
				{
					completion.TrySetResult(action());
				}
				catch (Exception exception)
				{
					completion.TrySetException(exception);
				}
			});
		}
		RequestProcessing();
		return completion.Task;
	}

	public Task NextProcessFrameAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskCompletionSource<bool> completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		CancellationTokenRegistration registration = default;
		DispatchAsync(() =>
		{
			if (cancellationToken.IsCancellationRequested)
			{
				completion.TrySetCanceled(cancellationToken);
			}
			else
			{
				SceneTree tree = GetTree();
				Action handler = null;
				handler = () =>
				{
					tree.ProcessFrame -= handler;
					registration.Dispose();
					completion.TrySetResult(result: true);
				};
				tree.ProcessFrame += handler;
				if (cancellationToken.CanBeCanceled)
				{
					registration = cancellationToken.Register(() =>
					{
						DispatchAsync(() =>
						{
							if (GodotObject.IsInstanceValid(tree))
							{
								tree.ProcessFrame -= handler;
							}
							completion.TrySetCanceled(cancellationToken);
						});
					});
				}
			}
		}).ContinueWith((Task task) =>
		{
			if (task.IsFaulted)
			{
				completion.TrySetException(task.Exception?.GetBaseException() ?? new InvalidOperationException("Main-thread dispatch failed."));
			}
			else if (task.IsCanceled)
			{
				completion.TrySetCanceled();
			}
		}, TaskScheduler.Default);
		return completion.Task;
	}

	public void OwnSessionShutdown(XWScriptDebugSession session, Node previewRoot)
	{
		EnsureMainThread();
		if (session == null)
		{
			FreePreviewRoot(previewRoot);
			return;
		}
		_ownedCleanupTasks.Add(StopUntilReleasedAsync(session, previewRoot));
		RequestProcessing();
	}

	internal Task ClearPreviewRootAsync(Node previewRoot)
	{
		return DispatchAsync(() =>
		{
			if (!GodotObject.IsInstanceValid(previewRoot))
			{
				return;
			}
			foreach (Node child in previewRoot.GetChildren())
			{
				previewRoot.RemoveChild(child);
				child.Free();
			}
		});
	}

	private async Task StopUntilReleasedAsync(XWScriptDebugSession session, Node previewRoot)
	{
		while (session.HasLiveSession)
		{
			try
			{
				if (await session.StopAsync(500) || !session.HasLiveSession)
				{
					break;
				}
				goto IL_00e1;
			}
			catch (Exception ex)
			{
				GD.PushError("Script debug shutdown retry: " + ex.GetBaseException().Message);
				goto IL_00e1;
			}
			IL_00e1:
			if (session.HasLiveSession)
			{
				await Task.Delay(50);
			}
		}
		await ClearPreviewRootAsync(previewRoot);
		await DispatchAsync(() =>
		{
			FreePreviewRoot(previewRoot);
		});
	}

	private void DrainMainThreadQueue()
	{
		while (true)
		{
			Action action;
			lock (_dispatchLock)
			{
				if (_mainThreadQueue.Count == 0)
				{
					break;
				}
				action = _mainThreadQueue.Dequeue();
			}
			action();
		}
	}

	private bool HasPendingWork()
	{
		lock (_dispatchLock)
		{
			return _mainThreadQueue.Count > 0 || _ownedCleanupTasks.Count > 0;
		}
	}

	private void RequestProcessing()
	{
		if (_mainThreadId != 0 && System.Environment.CurrentManagedThreadId == _mainThreadId && IsInsideTree())
		{
			SetProcess(enable: true);
			return;
		}
		SynchronizationContext mainThreadContext = _mainThreadContext;
		if (mainThreadContext == null || Interlocked.Exchange(ref _wakeRequestPending, 1) != 0)
		{
			return;
		}
		mainThreadContext.Post((object? _) =>
		{
			Interlocked.Exchange(ref _wakeRequestPending, 0);
			if (GodotObject.IsInstanceValid(this) && IsInsideTree())
			{
				SetProcess(enable: true);
			}
		}, null);
	}

	private static void FreePreviewRoot(Node previewRoot)
	{
		if (GodotObject.IsInstanceValid(previewRoot))
		{
			previewRoot.GetParent()?.RemoveChild(previewRoot);
			previewRoot.Free();
		}
	}

	private void EnsureMainThread()
	{
		if (_mainThreadId != 0 && System.Environment.CurrentManagedThreadId != _mainThreadId)
		{
			throw new InvalidOperationException("Script debug host tree mutations must run on the Godot main thread.");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetOrCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "requester", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePreviewRoot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrainMainThreadQueue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasPendingWork, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FreePreviewRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "previewRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureMainThread, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetOrCreate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWScriptDebugHost>(GetOrCreate(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CreatePreviewRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(CreatePreviewRoot());
			return true;
		}
		if (method == MethodName.DrainMainThreadQueue && args.Count == 0)
		{
			DrainMainThreadQueue();
			ret = default;
			return true;
		}
		if (method == MethodName.HasPendingWork && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPendingWork());
			return true;
		}
		if (method == MethodName.RequestProcessing && args.Count == 0)
		{
			RequestProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.FreePreviewRoot && args.Count == 1)
		{
			FreePreviewRoot(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureMainThread && args.Count == 0)
		{
			EnsureMainThread();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetOrCreate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWScriptDebugHost>(GetOrCreate(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FreePreviewRoot && args.Count == 1)
		{
			FreePreviewRoot(VariantUtils.ConvertTo<Node>(in args[0]));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.GetOrCreate)
		{
			return true;
		}
		if (method == MethodName.CreatePreviewRoot)
		{
			return true;
		}
		if (method == MethodName.DrainMainThreadQueue)
		{
			return true;
		}
		if (method == MethodName.HasPendingWork)
		{
			return true;
		}
		if (method == MethodName.RequestProcessing)
		{
			return true;
		}
		if (method == MethodName.FreePreviewRoot)
		{
			return true;
		}
		if (method == MethodName.EnsureMainThread)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._mainThreadId)
		{
			_mainThreadId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._wakeRequestPending)
		{
			_wakeRequestPending = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.MainThreadId)
		{
			from = MainThreadId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.OwnedCleanupCount)
		{
			from = OwnedCleanupCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PreviewRootCount)
		{
			from = PreviewRootCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._mainThreadId)
		{
			value = VariantUtils.CreateFrom(in _mainThreadId);
			return true;
		}
		if (name == PropertyName._wakeRequestPending)
		{
			value = VariantUtils.CreateFrom(in _wakeRequestPending);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._mainThreadId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._wakeRequestPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.MainThreadId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.OwnedCleanupCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PreviewRootCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._mainThreadId, Variant.From(in _mainThreadId));
		info.AddProperty(PropertyName._wakeRequestPending, Variant.From(in _wakeRequestPending));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._mainThreadId, out var value))
		{
			_mainThreadId = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._wakeRequestPending, out var value2))
		{
			_wakeRequestPending = value2.As<int>();
		}
	}
}
