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

[ScriptPath("res://Test/PeriodicAreaEventComponentHotPathRuntimeTest.cs")]
public class PeriodicAreaEventComponentHotPathRuntimeTest : Node
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

		public static readonly StringName CountActiveInstances = "CountActiveInstances";

		public static readonly StringName ReleaseWorkload = "ReleaseWorkload";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _owners = "_owners";

		public static readonly StringName _managers = "_managers";

		public static readonly StringName _samples = "_samples";

		public static readonly StringName _manager = "_manager";

		public static readonly StringName _control = "_control";

		public static readonly StringName _checkShape = "_checkShape";

		public static readonly StringName _definition = "_definition";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSamples = 240;

	private const int MeasuredSamples = 1200;

	private const double PhysicsDelta = 0.0005;

	private readonly PeriodicAreaEventComponent[] _components = new PeriodicAreaEventComponent[1000];

	private readonly TowerDefenseCharacter[] _owners = new TowerDefenseCharacter[1000];

	private readonly ComponentManager[] _managers = new ComponentManager[1000];

	private readonly double[] _samples = new double[1200];

	private TowerDefenseManager _manager;

	private TowerDefenseControlNew _control;

	private AabbShape2DResource _checkShape;

	private PeriodicAreaEventComponentDefinition _definition;

	public override void _Ready()
	{
		RunAsync();
	}

	private async void RunAsync()
	{
		try
		{
			CreateGameRunningContext();
			CreateWorkload();
			bool flag = await RunLifecycleAndEventOrderContract();
			GD.Print($"PERIODIC_REGISTRY_AFTER_CONTRACT hasCharacters={_manager.characterRegistry.HasRegisteredCharacters} active={_manager.characterRegistry.ActiveCharacterCount}");
			RunWarmup();
			Benchmark("notDue", 10000f, dps: false, out var mean, out var p, out var p2, out var p3, out var max, out var allocatedBytes, out var gen, out var gen2, out var gen3, out var dispatched);
			Benchmark("due", 0.0005f, dps: false, out var mean2, out var p4, out var p5, out var p6, out var max2, out var allocatedBytes2, out var gen4, out var gen5, out var gen6, out var dispatched2);
			Benchmark("dpsNotDue", 10000f, dps: true, out var mean3, out var p7, out var p8, out var p9, out var max3, out var allocatedBytes3, out var gen7, out var gen8, out var gen9, out var dispatched3);
			Benchmark("dpsDue", 0.0005f, dps: true, out var mean4, out var p10, out var p11, out var p12, out var max4, out var allocatedBytes4, out var gen10, out var gen11, out var gen12, out var dispatched4);
			int num = CountTreeOwners();
			int num2 = CountActiveInstances();
			bool flag2 = flag && num == 1000 && num2 == 1000 && dispatched == 1200000 && dispatched2 == 1200000 && dispatched3 == 1200000 && dispatched4 == 1200000 && allocatedBytes == 0L && gen == 0 && gen2 == 0 && gen3 == 0 && allocatedBytes2 == 0L && gen4 == 0 && gen5 == 0 && gen6 == 0 && allocatedBytes3 == 0L && gen7 == 0 && gen8 == 0 && gen9 == 0 && allocatedBytes4 == 0L && gen10 == 0 && gen11 == 0 && gen12 == 0 && p3 < 0.2 && p6 < 0.2 && p9 < 0.2 && p12 < 0.2;
			GD.Print($"PERIODIC_AREA_EVENT_COMPONENT_HOT_PATH_RESULT passed={flag2} functionalPassed={flag} instances={1000} warmupSamples={240} measuredSamples={1200} treeOwners={num} activeInstances={num2} notDueMeanMs={mean:F6} notDueP50Ms={p:F6} notDueP95Ms={p2:F6} notDueP99Ms={p3:F6} notDueMaxMs={max:F6} notDueAllocatedBytes={allocatedBytes} notDueGen0={gen} notDueGen1={gen2} notDueGen2={gen3} notDueDispatchedInstances={dispatched} dueMeanMs={mean2:F6} dueP50Ms={p4:F6} dueP95Ms={p5:F6} dueP99Ms={p6:F6} dueMaxMs={max2:F6} dueAllocatedBytes={allocatedBytes2} dueGen0={gen4} dueGen1={gen5} dueGen2={gen6} dueDispatchedInstances={dispatched2} dpsNotDueMeanMs={mean3:F6} dpsNotDueP50Ms={p7:F6} dpsNotDueP95Ms={p8:F6} dpsNotDueP99Ms={p9:F6} dpsNotDueMaxMs={max3:F6} dpsNotDueAllocatedBytes={allocatedBytes3} dpsNotDueGen0={gen7} dpsNotDueGen1={gen8} dpsNotDueGen2={gen9} dpsNotDueDispatchedInstances={dispatched3} dpsDueMeanMs={mean4:F6} dpsDueP50Ms={p10:F6} dpsDueP95Ms={p11:F6} dpsDueP99Ms={p12:F6} dpsDueMaxMs={max4:F6} dpsDueAllocatedBytes={allocatedBytes4} dpsDueGen0={gen10} dpsDueGen1={gen11} dpsDueGen2={gen12} dpsDueDispatchedInstances={dispatched4}");
			GetTree().Quit((!flag2) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PrintErr("PERIODIC_AREA_EVENT_COMPONENT_HOT_PATH_EXCEPTION " + ex);
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
		_manager.characterRegistry = new TowerDefenseBattleCharacterRegistry();
		_manager.AddChild(_manager.characterRegistry, forceReadableName: false, InternalMode.Disabled);
		SetGameRunningContext(_manager, _control);
		_checkShape = new AabbShape2DResource
		{
			Geometry = new RectangleShape2D
			{
				Size = new Vector2(200f, 200f)
			}
		};
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
		_definition = new PeriodicAreaEventComponentDefinition
		{
			ComponentTypeId = "PeriodicAreaEventComponent",
			DefinitionId = "periodic.area.hotpath.runtime",
			InstanceId = "periodic.area.hotpath.runtime",
			WireIndex = 0,
			attackMethod = "StaticTime",
			staticTime = 10000f,
			maxIntervalTriggersPerFrame = 1,
			targetEffectType = "Disabled",
			checkAllLine = true,
			checkShape = _checkShape,
			eventList = new Array<TowerDefenseCharacterEventBase>
			{
				new TowerDefenseCharacterEventBase()
			}
		};
		for (int i = 0; i < 1000; i++)
		{
			PeriodicAreaEventHotPathProbeOwner periodicAreaEventHotPathProbeOwner = new PeriodicAreaEventHotPathProbeOwner
			{
				Name = "PeriodicAreaEventHotPathOwner" + i,
				ProcessMode = ProcessModeEnum.Disabled,
				camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT,
				instance = new TowerDefenseCharacterInstance
				{
					maskFlags = 1,
					collisionFlags = 1
				}
			};
			periodicAreaEventHotPathProbeOwner.instance.character = periodicAreaEventHotPathProbeOwner;
			AddChild(periodicAreaEventHotPathProbeOwner, forceReadableName: false, InternalMode.Disabled);
			ComponentManager componentManager = new ComponentManager();
			PeriodicAreaEventComponent periodicAreaEventComponent = new PeriodicAreaEventComponent();
			periodicAreaEventComponent.Bind(componentManager, periodicAreaEventHotPathProbeOwner, _definition);
			periodicAreaEventComponent.Activate();
			_owners[i] = periodicAreaEventHotPathProbeOwner;
			_managers[i] = componentManager;
			_components[i] = periodicAreaEventComponent;
		}
	}

	private async Task<bool> RunLifecycleAndEventOrderContract()
	{
		PeriodicProbeEvent.Order.Clear();
		PeriodicAreaEventHotPathProbeOwner target = new PeriodicAreaEventHotPathProbeOwner
		{
			Name = "PeriodicAreaEventFunctionalTarget",
			ProcessMode = ProcessModeEnum.Disabled,
			camp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE,
			HitBoxDefinition = new CharacterHitBoxDefinition
			{
				Size = new Vector2(20f, 20f)
			},
			instance = new TowerDefenseCharacterInstance
			{
				maskFlags = 1,
				collisionFlags = 1
			}
		};
		target.instance.character = target;
		AddChild(target, forceReadableName: false, InternalMode.Disabled);
		_manager.characterRegistry.Register(target);
		PeriodicAreaEventComponentDefinition definition = new PeriodicAreaEventComponentDefinition
		{
			ComponentTypeId = "PeriodicAreaEventComponent",
			DefinitionId = "periodic.area.event.order",
			InstanceId = "periodic.area.event.order",
			WireIndex = 0,
			attackMethod = "StaticTime",
			staticTime = 10f,
			targetEffectType = "Disabled",
			checkAllLine = true,
			checkShape = _checkShape,
			eventList = new Array<TowerDefenseCharacterEventBase>
			{
				new PeriodicProbeEvent("first"),
				new PeriodicProbeEvent("second")
			}
		};
		PeriodicAreaEventHotPathProbeOwner probeOwner = new PeriodicAreaEventHotPathProbeOwner
		{
			Name = "PeriodicAreaEventFunctionalOwner",
			ProcessMode = ProcessModeEnum.Disabled,
			camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT,
			instance = new TowerDefenseCharacterInstance
			{
				maskFlags = 1,
				collisionFlags = 1
			}
		};
		probeOwner.instance.character = probeOwner;
		AddChild(probeOwner, forceReadableName: false, InternalMode.Disabled);
		ComponentManager manager = new ComponentManager();
		PeriodicAreaEventComponent probe = new PeriodicAreaEventComponent();
		probe.Bind(manager, probeOwner, definition);
		probe.Activate();
		GD.Print($"PERIODIC_PROBE_CONFIG checkAllLine={probe.checkAllLine}");
		probe.EventExecute();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		bool num = PeriodicProbeEvent.Order.Count == 2 && PeriodicProbeEvent.Order[0] == "first" && PeriodicProbeEvent.Order[1] == "second";
		PeriodicProbeEvent.Order.Clear();
		probe.EventExecuteDps(0f, spawnEffect: false);
		bool flag = PeriodicProbeEvent.Order.Count == 2 && PeriodicProbeEvent.Order[0] == "first" && PeriodicProbeEvent.Order[1] == "second";
		probe.Release();
		probeOwner.Free();
		_manager.characterRegistry.Unregister(target);
		target.Free();
		return num & flag;
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

	private void Benchmark(string label, float staticTime, bool dps, out double mean, out double p50, out double p95, out double p99, out double max, out long allocatedBytes, out int gen0, out int gen1, out int gen2, out int dispatched)
	{
		FieldInfo field = typeof(PeriodicAreaEventComponent).GetField("_attackMode", BindingFlags.Instance | BindingFlags.NonPublic);
		object value = Enum.Parse(field.FieldType, dps ? "Dps" : "StaticTime");
		for (int i = 0; i < 1000; i++)
		{
			field.SetValue(_components[i], value);
			if (dps)
			{
				_components[i].dpsEffectInterval = staticTime;
				_components[i].SyncDeserialize(new Dictionary { ["dps_effect_timer"] = 0f });
			}
			else
			{
				_components[i].staticTime = staticTime;
				_components[i].timer = 0f;
			}
		}
		for (int j = 0; j < 240; j++)
		{
			ulong physicsFrame = (ulong)j;
			for (int k = 0; k < 1000; k++)
			{
				_components[k].PhysicsProcess(0.0005, physicsFrame);
			}
		}
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
		long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
		int num = GC.CollectionCount(0);
		int num2 = GC.CollectionCount(1);
		int num3 = GC.CollectionCount(2);
		dispatched = 0;
		for (int l = 0; l < 1200; l++)
		{
			long timestamp = Stopwatch.GetTimestamp();
			ulong physicsFrame2 = (ulong)(240 + l);
			for (int m = 0; m < 1000; m++)
			{
				_components[m].PhysicsProcess(0.0005, physicsFrame2);
				dispatched++;
			}
			_samples[l] = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
		}
		allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread;
		gen0 = GC.CollectionCount(0) - num;
		gen1 = GC.CollectionCount(1) - num2;
		gen2 = GC.CollectionCount(2) - num3;
		System.Array.Sort(_samples);
		mean = 0.0;
		for (int n = 0; n < _samples.Length; n++)
		{
			mean += _samples[n];
		}
		mean /= _samples.Length;
		p50 = Percentile(0.5);
		p95 = Percentile(0.95);
		p99 = Percentile(0.99);
		max = _samples[^1];
		GD.Print($"PERIODIC_AREA_EVENT_COMPONENT_{label.ToUpperInvariant()}_DETAIL meanMs={mean:F6} p50Ms={p50:F6} p95Ms={p95:F6} p99Ms={p99:F6} maxMs={max:F6}");
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
			if (GodotObject.IsInstanceValid(_owners[i]) && _owners[i].IsInsideTree())
			{
				num++;
			}
		}
		return num;
	}

	private int CountActiveInstances()
	{
		int num = 0;
		for (int i = 0; i < 1000; i++)
		{
			PeriodicAreaEventComponent obj = _components[i];
			if (obj != null && obj.Lifecycle == ComponentRuntimeLifecycle.Active && _components[i].Alive && _components[i].WantsPhysicsProcess)
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
		_manager = null;
		_control = null;
		_checkShape = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(10)
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
			new Godot.Bridge.MethodInfo(MethodName.CountActiveInstances, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.CountActiveInstances && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountActiveInstances());
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
		if (method == MethodName.CountActiveInstances)
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
		if (name == PropertyName._checkShape)
		{
			_checkShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName._definition)
		{
			_definition = VariantUtils.ConvertTo<PeriodicAreaEventComponentDefinition>(in value);
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
		if (name == PropertyName._checkShape)
		{
			value = VariantUtils.CreateFrom(in _checkShape);
			return true;
		}
		if (name == PropertyName._definition)
		{
			value = VariantUtils.CreateFrom(in _definition);
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
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._checkShape, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._manager, Variant.From(in _manager));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._checkShape, Variant.From(in _checkShape));
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
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
		if (info.TryGetProperty(PropertyName._checkShape, out var value3))
		{
			_checkShape = value3.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName._definition, out var value4))
		{
			_definition = value4.As<PeriodicAreaEventComponentDefinition>();
		}
	}
}
