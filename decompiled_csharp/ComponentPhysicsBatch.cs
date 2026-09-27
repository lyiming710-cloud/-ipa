using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class ComponentPhysicsBatch : Node
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Register = "Register";

		public static readonly StringName Unregister = "Unregister";

		public static readonly StringName TryMount = "TryMount";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName RemoveAt = "RemoveAt";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private static bool _mountRequested;

	private readonly List<ComponentBase> _active = new List<ComponentBase>();

	private readonly Dictionary<ComponentBase, int> _activeIndices = new Dictionary<ComponentBase, int>(ReferenceEqualityComparer.Instance);

	public static ComponentPhysicsBatch Instance { get; private set; }

	public static bool Register(ComponentBase component)
	{
		if (!GodotObject.IsInstanceValid(Instance))
		{
			TryMount(component);
		}
		if (!GodotObject.IsInstanceValid(Instance))
		{
			return false;
		}
		component.PrepareSharedBatchRegistration();
		if (Instance._activeIndices.TryAdd(component, Instance._active.Count))
		{
			Instance._active.Add(component);
		}
		Instance.SetPhysicsProcess(enable: true);
		return true;
	}

	public static void Unregister(ComponentBase component)
	{
		if (GodotObject.IsInstanceValid(Instance) && Instance._activeIndices.TryGetValue(component, out var value))
		{
			Instance.RemoveAt(value);
			if (Instance._active.Count == 0)
			{
				Instance.SetPhysicsProcess(enable: false);
			}
		}
	}

	private static void TryMount(Node source)
	{
		if (!_mountRequested)
		{
			Node node = TowerDefenseProcessModeDispatch.ResolveBatchParent(source);
			if (GodotObject.IsInstanceValid(node))
			{
				Instance = new ComponentPhysicsBatch();
				_mountRequested = true;
				node.CallDeferred(Node.MethodName.AddChild, Instance);
			}
		}
	}

	public override void _Ready()
	{
		Instance = this;
		_mountRequested = false;
		ProcessMode = ProcessModeEnum.Always;
		SetPhysicsProcess(_active.Count > 0);
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
		}
		_mountRequested = false;
		_active.Clear();
		_activeIndices.Clear();
	}

	public override void _PhysicsProcess(double delta)
	{
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		long startTicks = TowerDefensePerfProfiler.Begin();
		bool treePaused = GetTree()?.Paused ?? false;
		TowerDefenseProcessModeDispatch.BeginFrame();
		TowerDefenseProcessModeDispatch.BeginDispatchPass();
		int num = 0;
		while (num < _active.Count)
		{
			ComponentBase componentBase = _active[num];
			if (!componentBase.IsSharedBatchDispatchActive)
			{
				RemoveAt(num);
				continue;
			}
			if (TowerDefenseProcessModeDispatch.ShouldDispatch(componentBase, treePaused))
			{
				componentBase.SharedBatchPhysicsProcess(delta);
			}
			if (num < _active.Count && _active[num] == componentBase)
			{
				num++;
			}
		}
		if (_active.Count == 0)
		{
			SetPhysicsProcess(enable: false);
		}
		TowerDefensePerfProfiler.End("batch.component", startTicks, _active.Count);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.ComponentPhysics, startBytes);
		TowerDefensePerfProfiler.DumpIfNeeded();
	}

	private void RemoveAt(int index)
	{
		int num = _active.Count - 1;
		ComponentBase key = _active[index];
		if (index != num)
		{
			ComponentBase componentBase = _active[num];
			_active[index] = componentBase;
			_activeIndices[componentBase] = index;
		}
		_active.RemoveAt(num);
		_activeIndices.Remove(key);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "component", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Unregister, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "component", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryMount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveAt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
			ret = VariantUtils.CreateFrom<bool>(Register(VariantUtils.ConvertTo<ComponentBase>(in args[0])));
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<ComponentBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryMount && args.Count == 1)
		{
			TryMount(VariantUtils.ConvertTo<Node>(in args[0]));
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveAt && args.Count == 1)
		{
			RemoveAt(VariantUtils.ConvertTo<int>(in args[0]));
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
			ret = VariantUtils.CreateFrom<bool>(Register(VariantUtils.ConvertTo<ComponentBase>(in args[0])));
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<ComponentBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryMount && args.Count == 1)
		{
			TryMount(VariantUtils.ConvertTo<Node>(in args[0]));
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.RemoveAt)
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
