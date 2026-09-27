using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/CharacterTimerComponentHotPathRuntimeTest.cs")]
public class CharacterTimerComponentHotPathRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RunAsync = "RunAsync";

		public static readonly StringName CreateGameRunningContext = "CreateGameRunningContext";

		public static readonly StringName SetGameRunningContext = "SetGameRunningContext";

		public static readonly StringName CreateWorkload = "CreateWorkload";

		public static readonly StringName RunWarmup = "RunWarmup";

		public static readonly StringName Percentile = "Percentile";

		public static readonly StringName CountTreeOwners = "CountTreeOwners";

		public static readonly StringName ReleaseWorkload = "ReleaseWorkload";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _owners = "_owners";

		public static readonly StringName _managers = "_managers";

		public static readonly StringName _samples = "_samples";

		public static readonly StringName _manager = "_manager";

		public static readonly StringName _control = "_control";

		public static readonly StringName _ownerMount = "_ownerMount";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSamples = 240;

	private const int MeasuredSamples = 1200;

	private const double PhysicsDelta = 0.0005;

	private readonly CharacterTimerComponent[] _components = new CharacterTimerComponent[1000];

	private readonly TowerDefenseCharacter[] _owners = new TowerDefenseCharacter[1000];

	private readonly ComponentManager[] _managers = new ComponentManager[1000];

	private readonly double[] _samples = new double[1200];

	private TowerDefenseManager _manager;

	private TowerDefenseControlNew _control;

	private Node2D _ownerMount;

	public override void _Ready()
	{
		RunAsync();
	}

	private async void RunAsync()
	{
		long allocatedBytes = 0L;
		int gen = 0;
		int gen2 = 0;
		int gen3 = 0;
		try
		{
			CreateGameRunningContext();
			CreateWorkload();
			bool flag = await RunLifecycleContract();
			RunWarmup();
			Benchmark(out var mean, out var p, out var p2, out var p3, out var max, out allocatedBytes, out gen, out gen2, out gen3, out var activeInstances, out var dispatched);
			int num = CountTreeOwners();
			bool flag2 = flag && num == 1000 && activeInstances == 1000 && dispatched == 1200000 && allocatedBytes == 0L && gen == 0 && gen2 == 0 && gen3 == 0 && p3 < 0.2;
			GD.Print($"CHARACTER_TIMER_COMPONENT_HOT_PATH_RESULT passed={flag2} functionalPassed={flag} instances={1000} treeOwners={num} activeInstances={activeInstances} dispatchedInstances={dispatched} warmupSamples={240} measuredSamples={1200} meanMs={mean:F6} p50Ms={p:F6} p95Ms={p2:F6} p99Ms={p3:F6} maxMs={max:F6} allocatedBytes={allocatedBytes} gen0={gen} gen1={gen2} gen2={gen3}");
			GetTree().Quit((!flag2) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PrintErr("CHARACTER_TIMER_COMPONENT_HOT_PATH_EXCEPTION " + ex);
			GetTree().Quit(2);
		}
		finally
		{
			ReleaseWorkload();
			SetGameRunningContext(null, null);
		}
	}

	private void CreateGameRunningContext()
	{
		_manager = new TowerDefenseManager();
		_control = new TowerDefenseControlNew
		{
			isGameRunning = true
		};
		_manager.currentControl = _control;
		SetGameRunningContext(_manager, _control);
	}

	private static void SetGameRunningContext(TowerDefenseManager manager, TowerDefenseControlNew control)
	{
		typeof(TowerDefenseManager).GetProperty("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(null, manager);
		if (manager != null)
		{
			manager.currentControl = control;
		}
	}

	private void CreateWorkload()
	{
		_ownerMount = new Node2D
		{
			Name = "CharacterTimerOwnerMount"
		};
		AddChild(_ownerMount, forceReadableName: false, InternalMode.Disabled);
		CharacterTimerComponentDefinition definition = new CharacterTimerComponentDefinition
		{
			ComponentTypeId = "CharacterTimerComponent",
			DefinitionId = "character.timer.hotpath.runtime",
			InstanceId = "character.timer.hotpath.runtime",
			WireIndex = 0,
			timerDictionary = new Dictionary { ["tick"] = 10000.0 },
			timeScale = 1.0
		};
		for (int i = 0; i < 1000; i++)
		{
			CharacterTimerHotPathProbeOwner characterTimerHotPathProbeOwner = new CharacterTimerHotPathProbeOwner
			{
				ProcessMode = ProcessModeEnum.Disabled
			};
			_ownerMount.AddChild(characterTimerHotPathProbeOwner, forceReadableName: false, InternalMode.Disabled);
			ComponentManager componentManager = new ComponentManager();
			CharacterTimerComponent characterTimerComponent = new CharacterTimerComponent();
			characterTimerComponent.Bind(componentManager, characterTimerHotPathProbeOwner, definition);
			characterTimerComponent.Activate();
			characterTimerComponent.Run("tick", 10000.0);
			_owners[i] = characterTimerHotPathProbeOwner;
			_managers[i] = componentManager;
			_components[i] = characterTimerComponent;
		}
	}

	private async Task<bool> RunLifecycleContract()
	{
		int callbackCount = 0;
		string callbackKey = null;
		CharacterTimerComponent probe = _components[0];
		probe.OnTimeout += (string key) =>
		{
			callbackCount++;
			callbackKey = key;
		};
		probe.Run("tick", 0.0);
		probe.PhysicsProcess(0.0005, 1uL);
		bool expired = !probe.WantsPhysicsProcess;
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		bool flag = callbackCount == 1 && callbackKey == "tick";
		probe.Run("tick", 10000.0);
		probe.Stop("tick");
		probe.PhysicsProcess(0.0005, 2uL);
		bool flag2 = callbackCount == 1 && !probe.WantsPhysicsProcess;
		probe.Run("tick", 10000.0);
		return expired & flag & flag2;
	}

	private void RunWarmup()
	{
		for (int i = 0; i < 240; i++)
		{
			ulong physicsFrame = (ulong)i;
			for (int j = 0; j < 1000; j++)
			{
				_components[j].PhysicsProcess(0.0005, physicsFrame);
			}
		}
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

	private void Benchmark(out double mean, out double p50, out double p95, out double p99, out double max, out long allocatedBytes, out int gen0, out int gen1, out int gen2, out int activeInstances, out int dispatched)
	{
		long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
		int num = GC.CollectionCount(0);
		int num2 = GC.CollectionCount(1);
		int num3 = GC.CollectionCount(2);
		dispatched = 0;
		for (int i = 0; i < 1200; i++)
		{
			long timestamp = Stopwatch.GetTimestamp();
			ulong physicsFrame = (ulong)(240 + i);
			for (int j = 0; j < 1000; j++)
			{
				_components[j].PhysicsProcess(0.0005, physicsFrame);
				dispatched++;
			}
			_samples[i] = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
		}
		allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread;
		gen0 = GC.CollectionCount(0) - num;
		gen1 = GC.CollectionCount(1) - num2;
		gen2 = GC.CollectionCount(2) - num3;
		System.Array.Sort(_samples);
		mean = 0.0;
		for (int k = 0; k < _samples.Length; k++)
		{
			mean += _samples[k];
		}
		mean /= _samples.Length;
		p50 = Percentile(0.5);
		p95 = Percentile(0.95);
		p99 = Percentile(0.99);
		max = _samples[^1];
		activeInstances = 0;
		for (int l = 0; l < 1000; l++)
		{
			if (_components[l].Alive && _components[l].WantsPhysicsProcess)
			{
				activeInstances++;
			}
		}
	}

	private double Percentile(double fraction)
	{
		int num = Math.Clamp((int)Math.Ceiling(fraction * (double)_samples.Length) - 1, 0, _samples.Length - 1);
		return _samples[num];
	}

	private int CountTreeOwners()
	{
		int num = 0;
		for (int i = 0; i < 1000; i++)
		{
			TowerDefenseCharacter obj = _owners[i];
			if (obj != null && obj.IsInsideTree())
			{
				num++;
			}
		}
		return num;
	}

	private void ReleaseWorkload()
	{
		for (int i = 0; i < 1000; i++)
		{
			_components[i]?.Release();
			_owners[i]?.Free();
			_components[i] = null;
			_owners[i] = null;
			_managers[i] = null;
		}
		_manager?.Free();
		_control?.Free();
		if (GodotObject.IsInstanceValid(_ownerMount))
		{
			RemoveChild(_ownerMount);
			_ownerMount.Free();
		}
		_manager = null;
		_control = null;
		_ownerMount = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(9)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RunAsync, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateGameRunningContext, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.SetGameRunningContext, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateWorkload, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RunWarmup, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Percentile, new Godot.Bridge.PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Float, "fraction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CountTreeOwners, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ReleaseWorkload, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.RunAsync && args.Count == 0)
		{
			RunAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateGameRunningContext && args.Count == 0)
		{
			CreateGameRunningContext();
			ret = default;
			return true;
		}
		if (method == MethodName.SetGameRunningContext && args.Count == 2)
		{
			SetGameRunningContext(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateWorkload && args.Count == 0)
		{
			CreateWorkload();
			ret = default;
			return true;
		}
		if (method == MethodName.RunWarmup && args.Count == 0)
		{
			RunWarmup();
			ret = default;
			return true;
		}
		if (method == MethodName.Percentile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(Percentile(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.CountTreeOwners && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountTreeOwners());
			return true;
		}
		if (method == MethodName.ReleaseWorkload && args.Count == 0)
		{
			ReleaseWorkload();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetGameRunningContext && args.Count == 2)
		{
			SetGameRunningContext(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[1]));
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
		if (method == MethodName.RunAsync)
		{
			return true;
		}
		if (method == MethodName.CreateGameRunningContext)
		{
			return true;
		}
		if (method == MethodName.SetGameRunningContext)
		{
			return true;
		}
		if (method == MethodName.CreateWorkload)
		{
			return true;
		}
		if (method == MethodName.RunWarmup)
		{
			return true;
		}
		if (method == MethodName.Percentile)
		{
			return true;
		}
		if (method == MethodName.CountTreeOwners)
		{
			return true;
		}
		if (method == MethodName.ReleaseWorkload)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._manager)
		{
			_manager = VariantUtils.ConvertTo<TowerDefenseManager>(in value);
			return true;
		}
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<TowerDefenseControlNew>(in value);
			return true;
		}
		if (name == PropertyName._ownerMount)
		{
			_ownerMount = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._owners)
		{
			GodotObject[] owners = _owners;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(owners);
			return true;
		}
		if (name == PropertyName._managers)
		{
			GodotObject[] owners = _managers;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(owners);
			return true;
		}
		if (name == PropertyName._samples)
		{
			value = VariantUtils.CreateFrom(in _samples);
			return true;
		}
		if (name == PropertyName._manager)
		{
			value = VariantUtils.CreateFrom(in _manager);
			return true;
		}
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
			return true;
		}
		if (name == PropertyName._ownerMount)
		{
			value = VariantUtils.CreateFrom(in _ownerMount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Array, PropertyName._owners, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Array, PropertyName._managers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.PackedFloat64Array, PropertyName._samples, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._manager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._ownerMount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._manager, Variant.From(in _manager));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._ownerMount, Variant.From(in _ownerMount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._manager, out var value))
		{
			_manager = value.As<TowerDefenseManager>();
		}
		if (info.TryGetProperty(PropertyName._control, out var value2))
		{
			_control = value2.As<TowerDefenseControlNew>();
		}
		if (info.TryGetProperty(PropertyName._ownerMount, out var value3))
		{
			_ownerMount = value3.As<Node2D>();
		}
	}
}
