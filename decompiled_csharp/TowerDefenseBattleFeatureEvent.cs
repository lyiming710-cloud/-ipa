using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Event/TowerDefenseBattleFeatureEvent.cs")]
public class TowerDefenseBattleFeatureEvent : TowerDefenseBattleFeature
{
	private enum PhaseWaitResult
	{
		Received,
		TimedOut,
		Cancelled
	}

	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName TryExecuteBufferedPhase = "TryExecuteBufferedPhase";

		public static readonly StringName ApplyRemoteEventExecute = "ApplyRemoteEventExecute";

		public static readonly StringName ExecuteEventsFromData = "ExecuteEventsFromData";

		public static readonly StringName CancelPendingWaiters = "CancelPendingWaiters";

		public static readonly StringName IsRemoteClient = "IsRemoteClient";

		public static readonly StringName IsKnownPhase = "IsKnownPhase";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName remotePhaseTimeoutSeconds = "remotePhaseTimeoutSeconds";

		public static readonly StringName _destroyed = "_destroyed";

		public static readonly StringName _restoredRuneStorms = "_restoredRuneStorms";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private const double DefaultRemotePhaseTimeoutSeconds = 10.0;

	private const double MaxRemotePhaseTimeoutSeconds = 120.0;

	public readonly List<TowerDefenseLevelEventBase> eventInit = new List<TowerDefenseLevelEventBase>();

	public readonly List<TowerDefenseLevelEventBase> eventEntry = new List<TowerDefenseLevelEventBase>();

	public readonly List<TowerDefenseLevelEventBase> eventReady = new List<TowerDefenseLevelEventBase>();

	public readonly List<TowerDefenseLevelEventBase> eventStart = new List<TowerDefenseLevelEventBase>();

	private readonly System.Collections.Generic.Dictionary<string, Godot.Collections.Array> _remotePhaseBuffer = new System.Collections.Generic.Dictionary<string, Godot.Collections.Array>(StringComparer.Ordinal);

	private readonly System.Collections.Generic.Dictionary<string, TaskCompletionSource<PhaseWaitResult>> _phaseWaiters = new System.Collections.Generic.Dictionary<string, TaskCompletionSource<PhaseWaitResult>>(StringComparer.Ordinal);

	private readonly HashSet<string> _completedPhases = new HashSet<string>(StringComparer.Ordinal);

	private bool _destroyed;

	private Dictionary _restoredRuneStorms;

	public double remotePhaseTimeoutSeconds { get; private set; } = 10.0;

	public override void Init(Dictionary _data)
	{
		CancelPendingWaiters();
		base.Init(_data ?? new Dictionary());
		eventInit.Clear();
		eventEntry.Clear();
		eventReady.Clear();
		eventStart.Clear();
		_remotePhaseBuffer.Clear();
		_completedPhases.Clear();
		_destroyed = false;
		remotePhaseTimeoutSeconds = Math.Clamp(data.GetValueOrDefault("RemotePhaseTimeoutSeconds", 10.0).AsDouble(), 0.0, 120.0);
		LoadEventList("EventInit", eventInit);
		LoadEventList("EventEntry", eventEntry);
		LoadEventList("EventReady", eventReady);
		LoadEventList("EventStart", eventStart);
	}

	public override Task GameInit()
	{
		if (!GodotObject.IsInstanceValid(control) || !control.isInit)
		{
			return Task.CompletedTask;
		}
		return ExecutePhaseAsync("init", eventInit);
	}

	public override Task GameInitFromProgress()
	{
		return Task.CompletedTask;
	}

	public override Task GameEntry()
	{
		if (!GodotObject.IsInstanceValid(control) || control.hasProgress)
		{
			return Task.CompletedTask;
		}
		return ExecutePhaseAsync("entry", eventEntry);
	}

	public override Task GameReady()
	{
		if (!GodotObject.IsInstanceValid(control) || control.hasProgress)
		{
			return Task.CompletedTask;
		}
		return ExecutePhaseAsync("ready", eventReady);
	}

	public override Task GameStart()
	{
		if (!GodotObject.IsInstanceValid(control) || control.hasProgress)
		{
			return Task.CompletedTask;
		}
		return ExecutePhaseAsync("start", eventStart);
	}

	public override void Destroy()
	{
		_destroyed = true;
		_restoredRuneStorms = null;
		CancelPendingWaiters();
		_remotePhaseBuffer.Clear();
		_completedPhases.Clear();
		eventInit.Clear();
		eventEntry.Clear();
		eventReady.Clear();
		eventStart.Clear();
		base.Destroy();
	}

	private async Task ExecutePhaseAsync(string phase, IReadOnlyList<TowerDefenseLevelEventBase> fallbackEvents)
	{
		if (_destroyed || _completedPhases.Contains(phase))
		{
			return;
		}
		TaskCompletionSource<PhaseWaitResult> waiter;
		if (!IsRemoteClient())
		{
			_completedPhases.Add(phase);
			ExecuteLocalEvents(fallbackEvents);
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				BroadcastEventExecute(phase, fallbackEvents);
			}
		}
		else
		{
			if (TryExecuteBufferedPhase(phase))
			{
				return;
			}
			if (remotePhaseTimeoutSeconds <= 0.0)
			{
				ExecuteFallbackPhase(phase, fallbackEvents);
				return;
			}
			SceneTree sceneTree = (GodotObject.IsInstanceValid(control) ? control.GetTree() : null);
			if (!GodotObject.IsInstanceValid(sceneTree))
			{
				return;
			}
			waiter = new TaskCompletionSource<PhaseWaitResult>();
			_phaseWaiters[phase] = waiter;
			SceneTreeTimer timer = sceneTree.CreateTimer(remotePhaseTimeoutSeconds, processAlways: true, processInPhysics: false, ignoreTimeScale: true);
			timer.Timeout += TimeoutHandler;
			PhaseWaitResult phaseWaitResult;
			try
			{
				phaseWaitResult = await waiter.Task;
			}
			finally
			{
				if (GodotObject.IsInstanceValid(timer))
				{
					timer.Timeout -= TimeoutHandler;
				}
			}
			if (_phaseWaiters.TryGetValue(phase, out var value) && value == waiter)
			{
				_phaseWaiters.Remove(phase);
			}
			if (!_destroyed && phaseWaitResult != PhaseWaitResult.Cancelled && !_completedPhases.Contains(phase) && (phaseWaitResult != PhaseWaitResult.Received || !TryExecuteBufferedPhase(phase)))
			{
				ExecuteFallbackPhase(phase, fallbackEvents);
			}
		}
		void TimeoutHandler()
		{
			waiter.TrySetResult(PhaseWaitResult.TimedOut);
		}
	}

	private void ExecuteFallbackPhase(string phase, IReadOnlyList<TowerDefenseLevelEventBase> fallbackEvents)
	{
		if (_completedPhases.Add(phase))
		{
			_remotePhaseBuffer.Remove(phase);
			GD.PushWarning($"[Event] Remote phase '{phase}' was not received within {remotePhaseTimeoutSeconds:0.###} seconds; executing local level data once.");
			ExecuteLocalEvents(fallbackEvents);
		}
	}

	private bool TryExecuteBufferedPhase(string phase)
	{
		if (_completedPhases.Contains(phase))
		{
			return true;
		}
		if (!_remotePhaseBuffer.TryGetValue(phase, out var value))
		{
			return false;
		}
		_remotePhaseBuffer.Remove(phase);
		_completedPhases.Add(phase);
		ExecuteEventsFromData(value, phase);
		return true;
	}

	public void ApplyRemoteEventExecute(string phase, Godot.Collections.Array eventsData)
	{
		if (!_destroyed && IsKnownPhase(phase) && eventsData != null && !_completedPhases.Contains(phase) && !_remotePhaseBuffer.ContainsKey(phase))
		{
			_remotePhaseBuffer[phase] = eventsData.Duplicate(deep: true);
			if (_phaseWaiters.TryGetValue(phase, out var value))
			{
				value.TrySetResult(PhaseWaitResult.Received);
			}
		}
	}

	private void LoadEventList(string dataKey, List<TowerDefenseLevelEventBase> target)
	{
		Godot.Collections.Array array = data.GetValueOrDefault(dataKey, new Godot.Collections.Array()).AsGodotArray();
		for (int i = 0; i < array.Count; i++)
		{
			try
			{
				if (array[i].VariantType != Variant.Type.Dictionary)
				{
					continue;
				}
				Dictionary dictionary = array[i].AsGodotDictionary();
				string text = dictionary.GetValueOrDefault("EventName", "").AsString();
				if (!string.IsNullOrEmpty(text))
				{
					TowerDefenseLevelEventBase towerDefenseLevelEventBase = TowerDefenseLevelEventRegistry.Create(text);
					if (!GodotObject.IsInstanceValid(towerDefenseLevelEventBase))
					{
						GD.PushError($"[Event] Unknown event '{text}' in {dataKey}[{i}].");
					}
					else
					{
						towerDefenseLevelEventBase.Init(dictionary.GetValueOrDefault("Value", new Dictionary()).AsGodotDictionary());
						target.Add(towerDefenseLevelEventBase);
					}
				}
			}
			catch (Exception value)
			{
				GD.PushError($"[Event] Failed to initialize {dataKey}[{i}]: {value}");
			}
		}
	}

	private static void ExecuteLocalEvents(IEnumerable<TowerDefenseLevelEventBase> events)
	{
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			TowerDefenseManager.Instance.ExecuteLevelEvent(events);
		}
	}

	private static void ExecuteEventsFromData(Godot.Collections.Array eventsData, string phase)
	{
		for (int i = 0; i < eventsData.Count; i++)
		{
			try
			{
				if (eventsData[i].VariantType != Variant.Type.Dictionary)
				{
					continue;
				}
				Dictionary dictionary = eventsData[i].AsGodotDictionary();
				string text = dictionary.GetValueOrDefault("EventName", "").AsString();
				if (!string.IsNullOrEmpty(text))
				{
					TowerDefenseLevelEventBase towerDefenseLevelEventBase = TowerDefenseLevelEventRegistry.Create(text);
					if (!GodotObject.IsInstanceValid(towerDefenseLevelEventBase))
					{
						GD.PushError($"[Event] Unknown remote event '{text}' in phase '{phase}'.");
					}
					else
					{
						towerDefenseLevelEventBase.Init(dictionary.GetValueOrDefault("Value", new Dictionary()).AsGodotDictionary());
						towerDefenseLevelEventBase.Execute();
					}
				}
			}
			catch (Exception value)
			{
				GD.PushError($"[Event] Failed to execute remote phase '{phase}' item {i}: {value}");
			}
		}
	}

	private static void BroadcastEventExecute(string phase, IEnumerable<TowerDefenseLevelEventBase> events)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		int num = 0;
		foreach (TowerDefenseLevelEventBase @event in events)
		{
			try
			{
				if (GodotObject.IsInstanceValid(@event))
				{
					array.Add(@event.Export());
				}
			}
			catch (Exception value)
			{
				GD.PushError($"[Event] Failed to export phase '{phase}' item {num}: {value}");
			}
			num++;
		}
		if (GodotObject.IsInstanceValid(MultiPlayerManager.Instance))
		{
			MultiPlayerManager.Instance.SendEventExecute(phase, Json.Stringify(array));
		}
	}

	private void CancelPendingWaiters()
	{
		if (_phaseWaiters.Count == 0)
		{
			return;
		}
		List<TaskCompletionSource<PhaseWaitResult>> list = new List<TaskCompletionSource<PhaseWaitResult>>(_phaseWaiters.Values);
		_phaseWaiters.Clear();
		foreach (TaskCompletionSource<PhaseWaitResult> item in list)
		{
			item.TrySetResult(PhaseWaitResult.Cancelled);
		}
	}

	private static bool IsRemoteClient()
	{
		if (Global.IsMultiplayerMode)
		{
			return !MultiPlayerManager.IsHost;
		}
		return false;
	}

	private static bool IsKnownPhase(string phase)
	{
		switch (phase)
		{
		case "init":
		case "entry":
		case "ready":
		case "start":
			return true;
		default:
			return false;
		}
	}

	public override Dictionary SyncSerialize()
	{
		return new Dictionary();
	}

	public override void SyncDeserialize(Dictionary _data)
	{
	}

	public override Dictionary SaveFeature()
	{
		Dictionary dictionary = TowerDefenseLevelEventRuneStorm.SaveStorms(control);
		if (dictionary["forming"].AsGodotArray().Count == 0 && dictionary["active"].AsGodotArray().Count == 0)
		{
			return new Dictionary();
		}
		return new Dictionary { ["runeStorms"] = dictionary };
	}

	public override void LoadFeature(Dictionary data, TowerDefenseLevelSaveConfigCSharp owner)
	{
		_restoredRuneStorms = data.GetValueOrDefault("runeStorms", new Dictionary()).AsGodotDictionary().Duplicate(deep: true);
	}

	public override Task GameStartFromProgress()
	{
		if (_restoredRuneStorms != null)
		{
			TowerDefenseLevelEventRuneStorm.RestoreStorms(control, _restoredRuneStorms);
			_restoredRuneStorms = null;
		}
		return Task.CompletedTask;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryExecuteBufferedPhase, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRemoteEventExecute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "eventsData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteEventsFromData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "eventsData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelPendingWaiters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsRemoteClient, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.IsKnownPhase, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
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
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.TryExecuteBufferedPhase && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryExecuteBufferedPhase(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyRemoteEventExecute && args.Count == 2)
		{
			ApplyRemoteEventExecute(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteEventsFromData && args.Count == 2)
		{
			ExecuteEventsFromData(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CancelPendingWaiters && args.Count == 0)
		{
			CancelPendingWaiters();
			ret = default;
			return true;
		}
		if (method == MethodName.IsRemoteClient && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRemoteClient());
			return true;
		}
		if (method == MethodName.IsKnownPhase && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsKnownPhase(VariantUtils.ConvertTo<string>(in args[0])));
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ExecuteEventsFromData && args.Count == 2)
		{
			ExecuteEventsFromData(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsRemoteClient && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRemoteClient());
			return true;
		}
		if (method == MethodName.IsKnownPhase && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsKnownPhase(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.TryExecuteBufferedPhase)
		{
			return true;
		}
		if (method == MethodName.ApplyRemoteEventExecute)
		{
			return true;
		}
		if (method == MethodName.ExecuteEventsFromData)
		{
			return true;
		}
		if (method == MethodName.CancelPendingWaiters)
		{
			return true;
		}
		if (method == MethodName.IsRemoteClient)
		{
			return true;
		}
		if (method == MethodName.IsKnownPhase)
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
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.remotePhaseTimeoutSeconds)
		{
			remotePhaseTimeoutSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._destroyed)
		{
			_destroyed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._restoredRuneStorms)
		{
			_restoredRuneStorms = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.remotePhaseTimeoutSeconds)
		{
			value = VariantUtils.CreateFrom<double>(remotePhaseTimeoutSeconds);
			return true;
		}
		if (name == PropertyName._destroyed)
		{
			value = VariantUtils.CreateFrom(in _destroyed);
			return true;
		}
		if (name == PropertyName._restoredRuneStorms)
		{
			value = VariantUtils.CreateFrom(in _restoredRuneStorms);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.remotePhaseTimeoutSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._destroyed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._restoredRuneStorms, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.remotePhaseTimeoutSeconds, Variant.From<double>(remotePhaseTimeoutSeconds));
		info.AddProperty(PropertyName._destroyed, Variant.From(in _destroyed));
		info.AddProperty(PropertyName._restoredRuneStorms, Variant.From(in _restoredRuneStorms));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.remotePhaseTimeoutSeconds, out var value))
		{
			remotePhaseTimeoutSeconds = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._destroyed, out var value2))
		{
			_destroyed = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._restoredRuneStorms, out var value3))
		{
			_restoredRuneStorms = value3.As<Dictionary>();
		}
	}
}
