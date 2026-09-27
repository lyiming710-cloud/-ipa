using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/FireComponentPhysicsHotPathRuntimeTest.cs")]
public class FireComponentPhysicsHotPathRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateWorkload = "CreateWorkload";

		public static readonly StringName RunWarmup = "RunWarmup";

		public static readonly StringName Benchmark = "Benchmark";

		public static readonly StringName Percentile = "Percentile";

		public static readonly StringName Finish = "Finish";

		public static readonly StringName ReleaseWorkload = "ReleaseWorkload";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _owners = "_owners";

		public static readonly StringName _managers = "_managers";

		public static readonly StringName _samples = "_samples";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSamples = 240;

	private const int MeasuredSamples = 1200;

	private const double PhysicsDelta = 0.0005;

	private readonly FireComponent[] _components = new FireComponent[1000];

	private readonly FireComponentHotPathProbeOwner[] _owners = new FireComponentHotPathProbeOwner[1000];

	private readonly ComponentManager[] _managers = new ComponentManager[1000];

	private readonly double[] _samples = new double[1200];

	public override void _Ready()
	{
		try
		{
			CreateWorkload();
			RunWarmup();
			Benchmark();
		}
		catch (Exception ex)
		{
			GD.PrintErr("FIRE_COMPONENT_PHYSICS_HOT_PATH_EXCEPTION " + ex);
			Finish(passed: false, 0, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, 0.0, 0L, 0, 0, 0);
		}
		finally
		{
			ReleaseWorkload();
		}
	}

	private void CreateWorkload()
	{
		FireComponentDefinition definition = new FireComponentDefinition
		{
			ComponentTypeId = "FireComponent",
			DefinitionId = "fire.hotpath.runtime",
			InstanceId = "fire.hotpath.runtime",
			WireIndex = 0,
			checkUse = false,
			fireInterval = 1.5f,
			fireIntervalBase = 1.5f
		};
		for (int i = 0; i < 1000; i++)
		{
			FireComponentHotPathProbeOwner fireComponentHotPathProbeOwner = new FireComponentHotPathProbeOwner
			{
				Name = "FireComponentHotPathOwner" + i
			};
			AddChild(fireComponentHotPathProbeOwner, forceReadableName: false, InternalMode.Disabled);
			ComponentManager componentManager = new ComponentManager();
			FireComponent fireComponent = new FireComponent();
			fireComponent.Bind(componentManager, fireComponentHotPathProbeOwner, definition);
			fireComponent.timer = 1000f;
			fireComponent.isCheck = false;
			fireComponent.Activate();
			_owners[i] = fireComponentHotPathProbeOwner;
			_managers[i] = componentManager;
			_components[i] = fireComponent;
		}
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

	private void Benchmark()
	{
		long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
		int num = GC.CollectionCount(0);
		int num2 = GC.CollectionCount(1);
		int num3 = GC.CollectionCount(2);
		int num4 = 0;
		for (int i = 0; i < 1200; i++)
		{
			long timestamp = Stopwatch.GetTimestamp();
			ulong physicsFrame = (ulong)(240 + i);
			for (int j = 0; j < 1000; j++)
			{
				_components[j].PhysicsProcess(0.0005, physicsFrame);
				num4++;
			}
			long num5 = Stopwatch.GetTimestamp() - timestamp;
			_samples[i] = (double)num5 * 1000.0 / (double)Stopwatch.Frequency;
		}
		long num6 = GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread;
		int num7 = GC.CollectionCount(0) - num;
		int num8 = GC.CollectionCount(1) - num2;
		int num9 = GC.CollectionCount(2) - num3;
		Array.Sort(_samples);
		double num10 = 0.0;
		for (int k = 0; k < _samples.Length; k++)
		{
			num10 += _samples[k];
		}
		num10 /= (double)_samples.Length;
		double p = Percentile(0.5);
		double p2 = Percentile(0.95);
		double num11 = Percentile(0.99);
		double max = _samples[^1];
		int num12 = 0;
		int num13 = 0;
		for (int l = 0; l < 1000; l++)
		{
			if (_components[l].Alive && _components[l].Lifecycle == ComponentRuntimeLifecycle.Active && _components[l].WantsPhysicsProcess)
			{
				num12++;
			}
			if (GodotObject.IsInstanceValid(_owners[l]) && _owners[l].IsInsideTree())
			{
				num13++;
			}
		}
		bool passed = num4 == 1200000 && num12 == 1000 && num13 == 1000 && num6 == 0L && num7 == 0 && num8 == 0 && num9 == 0 && num11 < 0.2;
		Finish(passed, 1000, num13, num12, num4, num10, p, p2, num11, max, num6, num7, num8, num9);
	}

	private double Percentile(double fraction)
	{
		int num = Math.Clamp((int)Math.Ceiling(fraction * (double)_samples.Length) - 1, 0, _samples.Length - 1);
		return _samples[num];
	}

	private void Finish(bool passed, int instances, int treeOwners, int activeInstances, int dispatched, double mean, double p50, double p95, double p99, double max, long allocatedBytes, int gen0, int gen1, int gen2)
	{
		GD.Print($"FIRE_COMPONENT_PHYSICS_HOT_PATH_RESULT passed={passed} instances={instances} treeOwners={treeOwners} activeInstances={activeInstances} dispatchedInstances={dispatched} warmupSamples={240} measuredSamples={1200} meanMs={mean:F6} p50Ms={p50:F6} p95Ms={p95:F6} p99Ms={p99:F6} maxMs={max:F6} allocatedBytes={allocatedBytes} gen0={gen0} gen1={gen1} gen2={gen2}");
		GetTree().Quit((!passed) ? 2 : 0);
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
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateWorkload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunWarmup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Benchmark, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Percentile, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "fraction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "passed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "instances", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "treeOwners", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "activeInstances", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "dispatched", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "mean", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "p50", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "p95", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "p99", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "max", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "allocatedBytes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gen0", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gen1", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gen2", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseWorkload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Benchmark && args.Count == 0)
		{
			Benchmark();
			ret = default;
			return true;
		}
		if (method == MethodName.Percentile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(Percentile(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.Finish && args.Count == 14)
		{
			Finish(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]), VariantUtils.ConvertTo<double>(in args[7]), VariantUtils.ConvertTo<double>(in args[8]), VariantUtils.ConvertTo<double>(in args[9]), VariantUtils.ConvertTo<long>(in args[10]), VariantUtils.ConvertTo<int>(in args[11]), VariantUtils.ConvertTo<int>(in args[12]), VariantUtils.ConvertTo<int>(in args[13]));
			ret = default;
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
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
		if (method == MethodName.Benchmark)
		{
			return true;
		}
		if (method == MethodName.Percentile)
		{
			return true;
		}
		if (method == MethodName.Finish)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._owners, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._managers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedFloat64Array, PropertyName._samples, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
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
