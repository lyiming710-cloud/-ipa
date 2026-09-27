using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Resource/TowerDefenseBattleComponentBase.cs")]
public class TowerDefenseBattleComponentBase : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName CancelLifetime = "CancelLifetime";

		public static readonly StringName Destroy = "Destroy";

		public static readonly StringName OnReady = "OnReady";

		public static readonly StringName Process = "Process";

		public static readonly StringName CanLoadProgress = "CanLoadProgress";

		public static readonly StringName GameFail = "GameFail";

		public static readonly StringName ZombieEnterHouse = "ZombieEnterHouse";

		public static readonly StringName SyncSerialize = "SyncSerialize";

		public static readonly StringName SyncDeserialize = "SyncDeserialize";

		public static readonly StringName SyncProcess = "SyncProcess";

		public static readonly StringName GetFeature = "GetFeature";

		public static readonly StringName GetLevelControl = "GetLevelControl";

		public static readonly StringName GetTree = "GetTree";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName IsLifetimeActive = "IsLifetimeActive";

		public static readonly StringName _lifetimeEnded = "_lifetimeEnded";

		public static readonly StringName control = "control";

		public static readonly StringName data = "data";

		public static readonly StringName dependenceData = "dependenceData";

		public static readonly StringName gameStartPriority = "gameStartPriority";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private readonly CancellationTokenSource _lifetimeCancellation = new CancellationTokenSource();

	private bool _lifetimeEnded;

	public TowerDefenseControlNew control;

	public Dictionary data = new Dictionary();

	public TowerDefenseBattleDependenceData dependenceData;

	public int gameStartPriority;

	public CancellationToken LifetimeToken => _lifetimeCancellation.Token;

	public bool IsLifetimeActive
	{
		get
		{
			if (!_lifetimeEnded)
			{
				return !_lifetimeCancellation.IsCancellationRequested;
			}
			return false;
		}
	}

	public virtual void Init(Dictionary _data)
	{
		if (_lifetimeEnded)
		{
			throw new InvalidOperationException(GetType().Name + " cannot be initialized after its battle lifetime ended.");
		}
		data = _data;
	}

	public void CancelLifetime()
	{
		if (_lifetimeEnded)
		{
			return;
		}
		_lifetimeEnded = true;
		try
		{
			_lifetimeCancellation.Cancel();
		}
		catch (AggregateException value)
		{
			GD.PushError($"[BattleLifetime] Cancellation callback failed for {GetType().Name}: {value}");
		}
	}

	protected void RunLifetimeTask(Func<Task> taskFactory, string operationName)
	{
		if (!IsLifetimeActive || taskFactory == null)
		{
			return;
		}
		try
		{
			ObserveLifetimeTask(taskFactory(), operationName);
		}
		catch (Exception value)
		{
			GD.PushError($"[BattleTask] {GetType().Name}.{operationName} failed to start: {value}");
		}
	}

	private async Task ObserveLifetimeTask(Task task, string operationName)
	{
		try
		{
			await task;
		}
		catch (OperationCanceledException) when (!IsLifetimeActive)
		{
		}
		catch (Exception value)
		{
			GD.PushError($"[BattleTask] {GetType().Name}.{operationName} failed: {value}");
		}
	}

	protected async Task<bool> WaitForTaskOrLifetime(Task task)
	{
		if (!IsLifetimeActive || task == null)
		{
			return false;
		}
		TaskCompletionSource<bool> lifetimeWaiter = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		using (LifetimeToken.Register(() =>
		{
			lifetimeWaiter.TrySetResult(result: false);
		}))
		{
			if (await Task.WhenAny(task, lifetimeWaiter.Task) != task || !IsLifetimeActive)
			{
				return false;
			}
			await task;
			return IsLifetimeActive;
		}
	}

	protected async Task<bool> WaitForTimerOrLifetime(double seconds)
	{
		SceneTree tree = GetTree();
		if (!GodotObject.IsInstanceValid(tree) || !IsLifetimeActive)
		{
			return false;
		}
		seconds = (double.IsFinite(seconds) ? Math.Max(0.0, seconds) : 0.0);
		SceneTreeTimer timer = tree.CreateTimer(seconds, processAlways: false);
		TaskCompletionSource<bool> completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		timer.Timeout += TimeoutHandler;
		try
		{
			return await WaitForTaskOrLifetime(completion.Task);
		}
		finally
		{
			if (GodotObject.IsInstanceValid(timer))
			{
				timer.Timeout -= TimeoutHandler;
			}
		}
		void TimeoutHandler()
		{
			completion.TrySetResult(result: true);
		}
	}

	protected async Task<bool> WaitForTweenOrLifetime(Tween tween)
	{
		if (!GodotObject.IsInstanceValid(tween) || !IsLifetimeActive)
		{
			return false;
		}
		TaskCompletionSource<bool> completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		tween.Finished += FinishedHandler;
		try
		{
			return await WaitForTaskOrLifetime(completion.Task);
		}
		finally
		{
			if (GodotObject.IsInstanceValid(tween))
			{
				tween.Finished -= FinishedHandler;
			}
		}
		void FinishedHandler()
		{
			completion.TrySetResult(result: true);
		}
	}

	public virtual void Destroy()
	{
		CancelLifetime();
	}

	public virtual void OnReady()
	{
	}

	public virtual void Process(double delta)
	{
	}

	public virtual bool CanLoadProgress()
	{
		return true;
	}

	public virtual Task GameInit()
	{
		return Task.CompletedTask;
	}

	public virtual Task GameInitFromProgress()
	{
		return Task.CompletedTask;
	}

	public virtual Task GameEntry()
	{
		return Task.CompletedTask;
	}

	public virtual Task GameReady()
	{
		return Task.CompletedTask;
	}

	public virtual Task GameStart()
	{
		return Task.CompletedTask;
	}

	public virtual async Task GameStartFromProgress()
	{
		await GameStart();
	}

	public virtual void GameFail()
	{
	}

	public virtual void ZombieEnterHouse(TowerDefenseCharacter character)
	{
	}

	public virtual Dictionary SyncSerialize()
	{
		return new Dictionary();
	}

	public virtual void SyncDeserialize(Dictionary _data)
	{
	}

	public virtual void SyncProcess(double delta)
	{
	}

	public TowerDefenseBattleFeature GetFeature(StringName featureName)
	{
		if (control != null)
		{
			return control.GetFeature(featureName);
		}
		return null;
	}

	public T GetFeature<T>(StringName featureName) where T : TowerDefenseBattleFeature
	{
		TowerDefenseControlNew towerDefenseControlNew = control;
		if (towerDefenseControlNew == null)
		{
			return null;
		}
		return towerDefenseControlNew.GetFeature<T>(featureName);
	}

	public bool TryGetFeature<T>(StringName featureName, out T feature) where T : TowerDefenseBattleFeature
	{
		TowerDefenseControlNew towerDefenseControlNew = control;
		feature = ((towerDefenseControlNew != null) ? towerDefenseControlNew.GetFeature<T>(featureName) : null);
		return feature != null;
	}

	public TowerDefenseInGameLevelControl GetLevelControl()
	{
		if (control != null)
		{
			return control.levelControl;
		}
		return null;
	}

	public SceneTree GetTree()
	{
		if (control != null)
		{
			return control.GetTree();
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelLifetime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanLoadProgress, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ZombieEnterHouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLevelControl, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetTree, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SceneTree"), exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.CancelLifetime && args.Count == 0)
		{
			CancelLifetime();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.OnReady && args.Count == 0)
		{
			OnReady();
			ret = default;
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanLoadProgress && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanLoadProgress());
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 0)
		{
			GameFail();
			ret = default;
			return true;
		}
		if (method == MethodName.ZombieEnterHouse && args.Count == 1)
		{
			ZombieEnterHouse(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.SyncProcess && args.Count == 1)
		{
			SyncProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetFeature && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeature>(GetFeature(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLevelControl && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGameLevelControl>(GetLevelControl());
			return true;
		}
		if (method == MethodName.GetTree && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<SceneTree>(GetTree());
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
		if (method == MethodName.CancelLifetime)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.OnReady)
		{
			return true;
		}
		if (method == MethodName.Process)
		{
			return true;
		}
		if (method == MethodName.CanLoadProgress)
		{
			return true;
		}
		if (method == MethodName.GameFail)
		{
			return true;
		}
		if (method == MethodName.ZombieEnterHouse)
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
		if (method == MethodName.SyncProcess)
		{
			return true;
		}
		if (method == MethodName.GetFeature)
		{
			return true;
		}
		if (method == MethodName.GetLevelControl)
		{
			return true;
		}
		if (method == MethodName.GetTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._lifetimeEnded)
		{
			_lifetimeEnded = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.control)
		{
			control = VariantUtils.ConvertTo<TowerDefenseControlNew>(in value);
			return true;
		}
		if (name == PropertyName.data)
		{
			data = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.dependenceData)
		{
			dependenceData = VariantUtils.ConvertTo<TowerDefenseBattleDependenceData>(in value);
			return true;
		}
		if (name == PropertyName.gameStartPriority)
		{
			gameStartPriority = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.IsLifetimeActive)
		{
			value = VariantUtils.CreateFrom<bool>(IsLifetimeActive);
			return true;
		}
		if (name == PropertyName._lifetimeEnded)
		{
			value = VariantUtils.CreateFrom(in _lifetimeEnded);
			return true;
		}
		if (name == PropertyName.control)
		{
			value = VariantUtils.CreateFrom(in control);
			return true;
		}
		if (name == PropertyName.data)
		{
			value = VariantUtils.CreateFrom(in data);
			return true;
		}
		if (name == PropertyName.dependenceData)
		{
			value = VariantUtils.CreateFrom(in dependenceData);
			return true;
		}
		if (name == PropertyName.gameStartPriority)
		{
			value = VariantUtils.CreateFrom(in gameStartPriority);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._lifetimeEnded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.dependenceData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.gameStartPriority, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsLifetimeActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._lifetimeEnded, Variant.From(in _lifetimeEnded));
		info.AddProperty(PropertyName.control, Variant.From(in control));
		info.AddProperty(PropertyName.data, Variant.From(in data));
		info.AddProperty(PropertyName.dependenceData, Variant.From(in dependenceData));
		info.AddProperty(PropertyName.gameStartPriority, Variant.From(in gameStartPriority));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._lifetimeEnded, out var value))
		{
			_lifetimeEnded = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.control, out var value2))
		{
			control = value2.As<TowerDefenseControlNew>();
		}
		if (info.TryGetProperty(PropertyName.data, out var value3))
		{
			data = value3.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.dependenceData, out var value4))
		{
			dependenceData = value4.As<TowerDefenseBattleDependenceData>();
		}
		if (info.TryGetProperty(PropertyName.gameStartPriority, out var value5))
		{
			gameStartPriority = value5.As<int>();
		}
	}
}
