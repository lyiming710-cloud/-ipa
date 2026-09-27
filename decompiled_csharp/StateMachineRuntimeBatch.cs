using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://addons/godot_state_charts/ResourceRuntime/StateMachineRuntimeBatch.cs")]
public class StateMachineRuntimeBatch : Node
{
	private readonly struct PendingUpdate(StateMachineRuntime runtime, bool processActive, bool physicsActive, bool remove)
	{
		public readonly StateMachineRuntime Runtime = runtime;

		public readonly bool ProcessActive = processActive;

		public readonly bool PhysicsActive = physicsActive;

		public readonly bool Remove = remove;
	}

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName UpdateDispatchFlags = "UpdateDispatchFlags";

		public static readonly StringName Unregister = "Unregister";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName EnsureMounted = "EnsureMounted";

		public static readonly StringName RemoveProcess = "RemoveProcess";

		public static readonly StringName RemovePhysics = "RemovePhysics";

		public static readonly StringName RemoveProcessAtSwap = "RemoveProcessAtSwap";

		public static readonly StringName RemovePhysicsAtSwap = "RemovePhysicsAtSwap";

		public static readonly StringName FlushPendingUpdates = "FlushPendingUpdates";

		public static readonly StringName UpdateGodotProcessing = "UpdateGodotProcessing";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _dispatching = "_dispatching";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly Dictionary<StateChart, StateMachineRuntime> _runtimes = new Dictionary<StateChart, StateMachineRuntime>();

	private readonly List<StateChart> _processHosts = new List<StateChart>();

	private readonly List<StateMachineRuntime> _processRuntimes = new List<StateMachineRuntime>();

	private readonly List<Action<double>> _processCallbacks = new List<Action<double>>();

	private readonly Dictionary<StateChart, int> _processIndices = new Dictionary<StateChart, int>();

	private readonly List<StateChart> _physicsHosts = new List<StateChart>();

	private readonly List<StateMachineRuntime> _physicsRuntimes = new List<StateMachineRuntime>();

	private readonly List<Action<double>> _physicsCallbacks = new List<Action<double>>();

	private readonly Dictionary<StateChart, int> _physicsIndices = new Dictionary<StateChart, int>();

	private readonly Dictionary<StateChart, PendingUpdate> _pendingUpdates = new Dictionary<StateChart, PendingUpdate>();

	private bool _dispatching;

	public static StateMachineRuntimeBatch Instance { get; private set; }

	public static int RegistrationCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(Instance))
			{
				return 0;
			}
			return Instance._runtimes.Count;
		}
	}

	public static int ProcessRegistrationCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(Instance))
			{
				return 0;
			}
			return Instance._processCallbacks.Count;
		}
	}

	public static int PhysicsRegistrationCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(Instance))
			{
				return 0;
			}
			return Instance._physicsCallbacks.Count;
		}
	}

	public static bool Register(StateChart host, StateMachineRuntime runtime, bool processActive, bool physicsActive)
	{
		if (!GodotObject.IsInstanceValid(host) || runtime == null)
		{
			return false;
		}
		StateMachineRuntimeBatch stateMachineRuntimeBatch = EnsureMounted(host);
		if (!GodotObject.IsInstanceValid(stateMachineRuntimeBatch))
		{
			return false;
		}
		stateMachineRuntimeBatch.RequestUpdate(host, runtime, processActive, physicsActive, remove: false);
		return true;
	}

	public static void UpdateDispatchFlags(StateChart host, bool processActive, bool physicsActive)
	{
		if (GodotObject.IsInstanceValid(Instance) && GodotObject.IsInstanceValid(host))
		{
			StateMachineRuntimeBatch instance = Instance;
			if (instance._runtimes.TryGetValue(host, out var value))
			{
				instance.RequestUpdate(host, value, processActive, physicsActive, remove: false);
			}
		}
	}

	public static void Unregister(StateChart host)
	{
		if (GodotObject.IsInstanceValid(Instance) && host != null)
		{
			StateMachineRuntimeBatch instance = Instance;
			instance._runtimes.TryGetValue(host, out var value);
			instance.RequestUpdate(host, value, processActive: false, physicsActive: false, remove: true);
		}
	}

	public override void _Ready()
	{
		Instance = this;
		UpdateGodotProcessing();
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
		}
		_runtimes.Clear();
		_processHosts.Clear();
		_processRuntimes.Clear();
		_processCallbacks.Clear();
		_processIndices.Clear();
		_physicsHosts.Clear();
		_physicsRuntimes.Clear();
		_physicsCallbacks.Clear();
		_physicsIndices.Clear();
		_pendingUpdates.Clear();
	}

	public override void _Process(double delta)
	{
		if (_processCallbacks.Count == 0)
		{
			SetProcess(enable: false);
			return;
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		bool flag = TowerDefensePerfProfiler.Enabled && TowerDefensePerfProfiler.DetailedHotPathMetrics;
		bool paused = GetTree().Paused;
		TowerDefenseProcessModeDispatch.BeginDispatchPass();
		_dispatching = true;
		try
		{
			for (int i = 0; i < _processCallbacks.Count; i++)
			{
				StateChart stateChart = _processHosts[i];
				if (GodotObject.IsInstanceValid(stateChart) && TowerDefenseProcessModeDispatch.ShouldDispatch(stateChart, paused))
				{
					if (flag)
					{
						_processRuntimes[i].TickProcessDetailed(delta);
					}
					else
					{
						_processCallbacks[i](delta);
					}
				}
			}
		}
		finally
		{
			_dispatching = false;
			TowerDefensePerfProfiler.End("batch.stateResourceProcess", startTicks, _processCallbacks.Count);
			FlushPendingUpdates();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_physicsCallbacks.Count == 0)
		{
			SetPhysicsProcess(enable: false);
			return;
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		bool flag = TowerDefensePerfProfiler.Enabled && TowerDefensePerfProfiler.DetailedHotPathMetrics;
		bool paused = GetTree().Paused;
		TowerDefenseProcessModeDispatch.BeginDispatchPass();
		_dispatching = true;
		try
		{
			for (int i = 0; i < _physicsCallbacks.Count; i++)
			{
				StateChart stateChart = _physicsHosts[i];
				if (GodotObject.IsInstanceValid(stateChart) && TowerDefenseProcessModeDispatch.ShouldDispatch(stateChart, paused))
				{
					if (flag)
					{
						_physicsRuntimes[i].TickPhysicsDetailed(delta);
					}
					else
					{
						_physicsCallbacks[i](delta);
					}
				}
			}
		}
		finally
		{
			_dispatching = false;
			TowerDefensePerfProfiler.End("batch.stateResourcePhysics", startTicks, _physicsCallbacks.Count);
			TowerDefensePerfProfiler.DumpIfNeeded();
			FlushPendingUpdates();
		}
	}

	private static StateMachineRuntimeBatch EnsureMounted(StateChart host)
	{
		if (GodotObject.IsInstanceValid(Instance))
		{
			return Instance;
		}
		SceneTree tree = host.GetTree();
		if (!GodotObject.IsInstanceValid(tree))
		{
			return null;
		}
		Node root = tree.Root;
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		StateMachineRuntimeBatch stateMachineRuntimeBatch = (Instance = new StateMachineRuntimeBatch
		{
			Name = "StateMachineRuntimeBatch",
			ProcessMode = ProcessModeEnum.Always
		});
		root.CallDeferred("add_child", stateMachineRuntimeBatch);
		return stateMachineRuntimeBatch;
	}

	private void RequestUpdate(StateChart host, StateMachineRuntime runtime, bool processActive, bool physicsActive, bool remove)
	{
		if (_dispatching)
		{
			_pendingUpdates[host] = new PendingUpdate(runtime, processActive, physicsActive, remove);
			return;
		}
		ApplyUpdate(host, runtime, processActive, physicsActive, remove);
		UpdateGodotProcessing();
	}

	private void ApplyUpdate(StateChart host, StateMachineRuntime runtime, bool processActive, bool physicsActive, bool remove)
	{
		if (remove)
		{
			RemoveProcess(host);
			RemovePhysics(host);
			_runtimes.Remove(host);
			return;
		}
		_runtimes[host] = runtime;
		if (processActive)
		{
			AddOrUpdateProcess(host, runtime);
		}
		else
		{
			RemoveProcess(host);
		}
		if (physicsActive)
		{
			AddOrUpdatePhysics(host, runtime);
		}
		else
		{
			RemovePhysics(host);
		}
	}

	private void AddOrUpdateProcess(StateChart host, StateMachineRuntime runtime)
	{
		if (_processIndices.TryGetValue(host, out var value))
		{
			_processRuntimes[value] = runtime;
			_processCallbacks[value] = runtime.TickProcess;
			return;
		}
		_processIndices[host] = _processHosts.Count;
		_processHosts.Add(host);
		_processRuntimes.Add(runtime);
		_processCallbacks.Add(runtime.TickProcess);
	}

	private void AddOrUpdatePhysics(StateChart host, StateMachineRuntime runtime)
	{
		if (_physicsIndices.TryGetValue(host, out var value))
		{
			_physicsRuntimes[value] = runtime;
			_physicsCallbacks[value] = runtime.TickPhysics;
			return;
		}
		_physicsIndices[host] = _physicsHosts.Count;
		_physicsHosts.Add(host);
		_physicsRuntimes.Add(runtime);
		_physicsCallbacks.Add(runtime.TickPhysics);
	}

	private void RemoveProcess(StateChart host)
	{
		if (_processIndices.TryGetValue(host, out var value))
		{
			RemoveProcessAtSwap(value);
		}
	}

	private void RemovePhysics(StateChart host)
	{
		if (_physicsIndices.TryGetValue(host, out var value))
		{
			RemovePhysicsAtSwap(value);
		}
	}

	private void RemoveProcessAtSwap(int index)
	{
		int num = _processHosts.Count - 1;
		StateChart key = _processHosts[index];
		_processIndices.Remove(key);
		if (index != num)
		{
			StateChart stateChart = _processHosts[num];
			_processHosts[index] = stateChart;
			_processRuntimes[index] = _processRuntimes[num];
			_processCallbacks[index] = _processCallbacks[num];
			_processIndices[stateChart] = index;
		}
		_processHosts.RemoveAt(num);
		_processRuntimes.RemoveAt(num);
		_processCallbacks.RemoveAt(num);
	}

	private void RemovePhysicsAtSwap(int index)
	{
		int num = _physicsHosts.Count - 1;
		StateChart key = _physicsHosts[index];
		_physicsIndices.Remove(key);
		if (index != num)
		{
			StateChart stateChart = _physicsHosts[num];
			_physicsHosts[index] = stateChart;
			_physicsRuntimes[index] = _physicsRuntimes[num];
			_physicsCallbacks[index] = _physicsCallbacks[num];
			_physicsIndices[stateChart] = index;
		}
		_physicsHosts.RemoveAt(num);
		_physicsRuntimes.RemoveAt(num);
		_physicsCallbacks.RemoveAt(num);
	}

	private void FlushPendingUpdates()
	{
		if (_pendingUpdates.Count == 0)
		{
			UpdateGodotProcessing();
			return;
		}
		foreach (KeyValuePair<StateChart, PendingUpdate> pendingUpdate in _pendingUpdates)
		{
			PendingUpdate value = pendingUpdate.Value;
			ApplyUpdate(pendingUpdate.Key, value.Runtime, value.ProcessActive, value.PhysicsActive, value.Remove);
		}
		_pendingUpdates.Clear();
		UpdateGodotProcessing();
	}

	private void UpdateGodotProcessing()
	{
		SetProcess(_processCallbacks.Count > 0);
		SetPhysicsProcess(_physicsCallbacks.Count > 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.UpdateDispatchFlags, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "processActive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "physicsActive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Unregister, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureMounted, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemovePhysics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveProcessAtSwap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemovePhysicsAtSwap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlushPendingUpdates, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateGodotProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.UpdateDispatchFlags && args.Count == 3)
		{
			UpdateDispatchFlags(VariantUtils.ConvertTo<StateChart>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<StateChart>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureMounted && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineRuntimeBatch>(EnsureMounted(VariantUtils.ConvertTo<StateChart>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveProcess && args.Count == 1)
		{
			RemoveProcess(VariantUtils.ConvertTo<StateChart>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemovePhysics && args.Count == 1)
		{
			RemovePhysics(VariantUtils.ConvertTo<StateChart>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveProcessAtSwap && args.Count == 1)
		{
			RemoveProcessAtSwap(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemovePhysicsAtSwap && args.Count == 1)
		{
			RemovePhysicsAtSwap(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlushPendingUpdates && args.Count == 0)
		{
			FlushPendingUpdates();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateGodotProcessing && args.Count == 0)
		{
			UpdateGodotProcessing();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.UpdateDispatchFlags && args.Count == 3)
		{
			UpdateDispatchFlags(VariantUtils.ConvertTo<StateChart>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<StateChart>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureMounted && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineRuntimeBatch>(EnsureMounted(VariantUtils.ConvertTo<StateChart>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.UpdateDispatchFlags)
		{
			return true;
		}
		if (method == MethodName.Unregister)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.EnsureMounted)
		{
			return true;
		}
		if (method == MethodName.RemoveProcess)
		{
			return true;
		}
		if (method == MethodName.RemovePhysics)
		{
			return true;
		}
		if (method == MethodName.RemoveProcessAtSwap)
		{
			return true;
		}
		if (method == MethodName.RemovePhysicsAtSwap)
		{
			return true;
		}
		if (method == MethodName.FlushPendingUpdates)
		{
			return true;
		}
		if (method == MethodName.UpdateGodotProcessing)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._dispatching)
		{
			_dispatching = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._dispatching)
		{
			value = VariantUtils.CreateFrom(in _dispatching);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._dispatching, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._dispatching, Variant.From(in _dispatching));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._dispatching, out var value))
		{
			_dispatching = value.As<bool>();
		}
	}
}
