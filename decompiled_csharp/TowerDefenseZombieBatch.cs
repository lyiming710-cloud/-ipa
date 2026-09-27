using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseZombieBatch.cs")]
public sealed class TowerDefenseZombieBatch : Node
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Register = "Register";

		public static readonly StringName UpdateProcessDispatch = "UpdateProcessDispatch";

		public static readonly StringName HasInstance = "HasInstance";

		public static readonly StringName Unregister = "Unregister";

		public static readonly StringName TryMount = "TryMount";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName GetPhysicsMetricName = "GetPhysicsMetricName";

		public static readonly StringName RemoveAtSwap = "RemoveAtSwap";

		public static readonly StringName UpdateProcessMembership = "UpdateProcessMembership";

		public static readonly StringName RemoveProcess = "RemoveProcess";

		public static readonly StringName RemoveProcessAtSwap = "RemoveProcessAtSwap";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private static bool _mountRequested;

	private readonly List<TowerDefenseZombie> _active = new List<TowerDefenseZombie>();

	private readonly Dictionary<TowerDefenseZombie, int> _activeIndices = new Dictionary<TowerDefenseZombie, int>(ReferenceEqualityComparer.Instance);

	private readonly List<TowerDefenseZombie> _processActive = new List<TowerDefenseZombie>();

	private readonly Dictionary<TowerDefenseZombie, int> _processActiveIndices = new Dictionary<TowerDefenseZombie, int>(ReferenceEqualityComparer.Instance);

	private readonly Dictionary<Type, string> _physicsMetricNames = new Dictionary<Type, string>();

	public static TowerDefenseZombieBatch Instance { get; private set; }

	public static bool Register(TowerDefenseZombie zombie)
	{
		if (!GodotObject.IsInstanceValid(zombie) || !zombie.IsInsideTree())
		{
			return false;
		}
		if (!HasInstance())
		{
			TryMount(zombie);
		}
		if (!HasInstance())
		{
			return false;
		}
		TowerDefenseZombieBatch instance = Instance;
		zombie.PrepareBatchRegistration();
		if (instance._activeIndices.TryAdd(zombie, instance._active.Count))
		{
			instance._active.Add(zombie);
		}
		instance.UpdateProcessMembership(zombie);
		zombie.SetOwnerBatchRegistration(registered: true);
		instance.SetProcess(instance._processActive.Count > 0);
		instance.SetPhysicsProcess(enable: true);
		return true;
	}

	public static void UpdateProcessDispatch(TowerDefenseZombie zombie)
	{
		if (HasInstance() && Instance._activeIndices.ContainsKey(zombie))
		{
			Instance.UpdateProcessMembership(zombie);
			Instance.SetProcess(Instance._processActive.Count > 0);
		}
	}

	private static bool HasInstance()
	{
		return GodotObject.IsInstanceValid(Instance);
	}

	public static void Unregister(TowerDefenseZombie zombie)
	{
		if (!GodotObject.IsInstanceValid(Instance))
		{
			zombie?.SetOwnerBatchRegistration(registered: false);
			return;
		}
		if (Instance._activeIndices.TryGetValue(zombie, out var value))
		{
			Instance.RemoveAtSwap(value);
		}
		zombie?.SetOwnerBatchRegistration(registered: false);
		Instance.SetProcess(Instance._processActive.Count > 0);
		if (Instance._active.Count == 0)
		{
			Instance.SetPhysicsProcess(enable: false);
		}
	}

	private static void TryMount(TowerDefenseZombie source)
	{
		if (!_mountRequested || !GodotObject.IsInstanceValid(Instance))
		{
			Node node = TowerDefenseProcessModeDispatch.ResolveBatchParent(source);
			if (GodotObject.IsInstanceValid(node))
			{
				TowerDefenseZombieBatch towerDefenseZombieBatch = (Instance = new TowerDefenseZombieBatch());
				_mountRequested = true;
				node.CallDeferred(Node.MethodName.AddChild, towerDefenseZombieBatch);
			}
		}
	}

	public override void _Ready()
	{
		Instance = this;
		_mountRequested = false;
		ProcessMode = ProcessModeEnum.Always;
		SetProcess(_processActive.Count > 0);
		SetPhysicsProcess(_active.Count > 0);
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
			_mountRequested = false;
		}
		for (int i = 0; i < _active.Count; i++)
		{
			if (GodotObject.IsInstanceValid(_active[i]))
			{
				_active[i].SetOwnerBatchRegistration(registered: false);
			}
		}
		_active.Clear();
		_activeIndices.Clear();
		_processActive.Clear();
		_processActiveIndices.Clear();
		_physicsMetricNames.Clear();
	}

	public override void _Process(double delta)
	{
		if (_processActive.Count == 0)
		{
			SetProcess(enable: false);
			return;
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		bool treePaused = GetTree()?.Paused ?? false;
		TowerDefenseProcessModeDispatch.BeginDispatchPass();
		int num = 0;
		int num2 = 0;
		while (num2 < _processActive.Count)
		{
			TowerDefenseZombie towerDefenseZombie = _processActive[num2];
			if (!GodotObject.IsInstanceValid(towerDefenseZombie) || !towerDefenseZombie.IsOwnerBatchDispatchActive || !towerDefenseZombie.WantsMainStateMachineProcessDispatch)
			{
				RemoveProcessAtSwap(num2);
				continue;
			}
			if (towerDefenseZombie.isPause)
			{
				num2++;
				continue;
			}
			if (!(towerDefenseZombie.BatchUsesInheritedProcessMode ? TowerDefenseProcessModeDispatch.ShouldDispatchInherited(towerDefenseZombie.BatchProcessModeParent, treePaused) : TowerDefenseProcessModeDispatch.ShouldDispatch(towerDefenseZombie, treePaused)))
			{
				num2++;
				continue;
			}
			towerDefenseZombie.BatchProcessUpdate(delta);
			num++;
			if (num2 < _processActive.Count && _processActive[num2] == towerDefenseZombie)
			{
				num2++;
			}
		}
		TowerDefensePerfProfiler.End("batch.zombieProcess", startTicks, num);
		if (_processActive.Count == 0)
		{
			SetProcess(enable: false);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_active.Count == 0)
		{
			SetPhysicsProcess(enable: false);
			return;
		}
		TowerDefenseProcessModeDispatch.EnterCharacterPhysicsBatchDispatch();
		try
		{
			long startTicks = TowerDefensePerfProfiler.Begin();
			bool treePaused = GetTree()?.Paused ?? false;
			TowerDefenseProcessModeDispatch.BeginFrame();
			ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			TowerDefenseProcessModeDispatch.BeginDispatchPass();
			bool flag = TowerDefensePerfProfiler.Enabled && TowerDefensePerfProfiler.DetailedHotPathMetrics;
			int num = 0;
			int num2 = 0;
			while (num2 < _active.Count)
			{
				TowerDefenseZombie towerDefenseZombie = _active[num2];
				if (!towerDefenseZombie.IsOwnerBatchDispatchActive)
				{
					RemoveAtSwap(num2);
					continue;
				}
				if (towerDefenseZombie.isPause)
				{
					num2++;
					continue;
				}
				if (!(towerDefenseZombie.BatchUsesInheritedProcessMode ? TowerDefenseProcessModeDispatch.ShouldDispatchInherited(towerDefenseZombie.BatchProcessModeParent, treePaused) : TowerDefenseProcessModeDispatch.ShouldDispatch(towerDefenseZombie, treePaused)))
				{
					num2++;
					continue;
				}
				if (flag)
				{
					long startTicks2 = TowerDefensePerfProfiler.BeginHotPath();
					towerDefenseZombie.BatchUpdateValidated(delta, currentPhysicsFrame);
					TowerDefensePerfProfiler.End(GetPhysicsMetricName(towerDefenseZombie), startTicks2);
				}
				else
				{
					towerDefenseZombie.BatchUpdateValidated(delta, currentPhysicsFrame);
				}
				num++;
				if (num2 < _active.Count && _active[num2] == towerDefenseZombie)
				{
					num2++;
				}
			}
			TowerDefensePerfProfiler.End("batch.zombie", startTicks, num);
			TowerDefensePerfProfiler.DumpIfNeeded();
			if (_active.Count == 0)
			{
				SetPhysicsProcess(enable: false);
			}
		}
		finally
		{
			TowerDefenseProcessModeDispatch.ExitCharacterPhysicsBatchDispatch();
		}
	}

	private string GetPhysicsMetricName(TowerDefenseZombie zombie)
	{
		Type type = zombie.GetType();
		if (_physicsMetricNames.TryGetValue(type, out var value))
		{
			return value;
		}
		value = "batch.zombie.type." + type.Name;
		_physicsMetricNames.Add(type, value);
		return value;
	}

	private void RemoveAtSwap(int index)
	{
		int num = _active.Count - 1;
		TowerDefenseZombie towerDefenseZombie = _active[index];
		RemoveProcess(towerDefenseZombie);
		if (index != num)
		{
			TowerDefenseZombie towerDefenseZombie2 = _active[num];
			_active[index] = towerDefenseZombie2;
			_activeIndices[towerDefenseZombie2] = index;
		}
		_active.RemoveAt(num);
		_activeIndices.Remove(towerDefenseZombie);
		if (GodotObject.IsInstanceValid(towerDefenseZombie))
		{
			towerDefenseZombie.SetOwnerBatchRegistration(registered: false);
		}
	}

	private void UpdateProcessMembership(TowerDefenseZombie zombie)
	{
		if (zombie.WantsMainStateMachineProcessDispatch)
		{
			if (_processActiveIndices.TryAdd(zombie, _processActive.Count))
			{
				_processActive.Add(zombie);
			}
		}
		else
		{
			RemoveProcess(zombie);
		}
	}

	private void RemoveProcess(TowerDefenseZombie zombie)
	{
		if (_processActiveIndices.TryGetValue(zombie, out var value))
		{
			RemoveProcessAtSwap(value);
		}
	}

	private void RemoveProcessAtSwap(int index)
	{
		int num = _processActive.Count - 1;
		TowerDefenseZombie key = _processActive[index];
		if (index != num)
		{
			TowerDefenseZombie towerDefenseZombie = _processActive[num];
			_processActive[index] = towerDefenseZombie;
			_processActiveIndices[towerDefenseZombie] = index;
		}
		_processActive.RemoveAt(num);
		_processActiveIndices.Remove(key);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateProcessDispatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasInstance, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Unregister, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryMount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
			new MethodInfo(MethodName.GetPhysicsMetricName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveAtSwap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateProcessMembership, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveProcessAtSwap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Register && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Register(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateProcessDispatch && args.Count == 1)
		{
			UpdateProcessDispatch(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasInstance && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasInstance());
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryMount && args.Count == 1)
		{
			TryMount(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
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
		if (method == MethodName.GetPhysicsMetricName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPhysicsMetricName(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveAtSwap && args.Count == 1)
		{
			RemoveAtSwap(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateProcessMembership && args.Count == 1)
		{
			UpdateProcessMembership(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveProcess && args.Count == 1)
		{
			RemoveProcess(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveProcessAtSwap && args.Count == 1)
		{
			RemoveProcessAtSwap(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Register && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Register(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateProcessDispatch && args.Count == 1)
		{
			UpdateProcessDispatch(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasInstance && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasInstance());
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryMount && args.Count == 1)
		{
			TryMount(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Register)
		{
			return true;
		}
		if (method == MethodName.UpdateProcessDispatch)
		{
			return true;
		}
		if (method == MethodName.HasInstance)
		{
			return true;
		}
		if (method == MethodName.Unregister)
		{
			return true;
		}
		if (method == MethodName.TryMount)
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
		if (method == MethodName.GetPhysicsMetricName)
		{
			return true;
		}
		if (method == MethodName.RemoveAtSwap)
		{
			return true;
		}
		if (method == MethodName.UpdateProcessMembership)
		{
			return true;
		}
		if (method == MethodName.RemoveProcess)
		{
			return true;
		}
		if (method == MethodName.RemoveProcessAtSwap)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
