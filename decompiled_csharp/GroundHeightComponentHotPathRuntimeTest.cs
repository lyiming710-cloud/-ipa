using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/GroundHeightComponentHotPathRuntimeTest.cs")]
public class GroundHeightComponentHotPathRuntimeTest : Node
{
	private enum BenchmarkScenario
	{
		Interpolation,
		Cooldown,
		Settled,
		Idle
	}

	private struct BenchmarkResult
	{
		public double Mean;

		public double P50;

		public double P95;

		public double P99;

		public double Max;

		public long AllocatedBytes;

		public int Gen0;

		public int Gen1;

		public int Gen2;

		public int ActiveInstances;

		public int EligibilityChecks;

		public int DispatchedInstances;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";

		public static readonly StringName CreateWorkload = "CreateWorkload";

		public static readonly StringName RunFunctionalContract = "RunFunctionalContract";

		public static readonly StringName ConfigureScenario = "ConfigureScenario";

		public static readonly StringName PrepareSample = "PrepareSample";

		public static readonly StringName Percentile = "Percentile";

		public static readonly StringName CountTreeOwners = "CountTreeOwners";

		public static readonly StringName CountBoundActiveInstances = "CountBoundActiveInstances";

		public static readonly StringName ReleaseWorkload = "ReleaseWorkload";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _owners = "_owners";

		public static readonly StringName _managers = "_managers";

		public static readonly StringName _samples = "_samples";

		public static readonly StringName _definition = "_definition";

		public static readonly StringName _ownerMount = "_ownerMount";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSamples = 240;

	private const int MeasuredSamples = 1200;

	private const double PhysicsDelta = 1.0 / 60.0;

	private readonly GroundHeightComponent[] _components = new GroundHeightComponent[1000];

	private readonly TowerDefenseCharacter[] _owners = new TowerDefenseCharacter[1000];

	private readonly ComponentManager[] _managers = new ComponentManager[1000];

	private readonly double[] _samples = new double[1200];

	private GroundHeightComponentDefinition _definition;

	private Node2D _ownerMount;

	public override void _Ready()
	{
		Callable.From(Run).CallDeferred();
	}

	private void Run()
	{
		try
		{
			CreateWorkload();
			bool flag = RunFunctionalContract();
			BenchmarkResult result = BenchmarkScenarioState(BenchmarkScenario.Interpolation);
			BenchmarkResult result2 = BenchmarkScenarioState(BenchmarkScenario.Cooldown);
			BenchmarkResult result3 = BenchmarkScenarioState(BenchmarkScenario.Settled);
			BenchmarkResult result4 = BenchmarkScenarioState(BenchmarkScenario.Idle);
			bool flag2 = ScenarioPassed(result, expectDispatch: true) && ScenarioPassed(result2, expectDispatch: true) && ScenarioPassed(result3, expectDispatch: true) && ScenarioPassed(result4, expectDispatch: false);
			int num = CountTreeOwners();
			int num2 = CountBoundActiveInstances();
			bool flag3 = (flag & flag2) && num == 1000 && num2 == 1000;
			double value = Math.Max(Math.Max(result.P99, result2.P99), Math.Max(result3.P99, result4.P99));
			long value2 = result.AllocatedBytes + result2.AllocatedBytes + result3.AllocatedBytes + result4.AllocatedBytes;
			int value3 = result.Gen0 + result2.Gen0 + result3.Gen0 + result4.Gen0;
			int value4 = result.Gen1 + result2.Gen1 + result3.Gen1 + result4.Gen1;
			int value5 = result.Gen2 + result2.Gen2 + result3.Gen2 + result4.Gen2;
			GD.Print("GROUND_HEIGHT_COMPONENT_HOT_PATH_RESULT " + $"passed={flag3} " + $"functionalPassed={flag} " + $"performancePassed={flag2} " + "renderer=" + RenderingServer.GetCurrentRenderingDriverName() + "_" + RenderingServer.GetCurrentRenderingMethod() + " " + $"instances={1000} " + $"treeOwners={num} " + $"boundActiveInstances={num2} " + $"warmupSamples={240} " + $"measuredSamples={1200} " + $"p99Ms={value:F6} " + $"allocatedBytes={value2} " + $"gen0={value3} gen1={value4} " + $"gen2={value5} " + FormatScenario("interpolation", result) + FormatScenario("cooldown", result2) + FormatScenario("settled", result3) + FormatScenario("idle", result4));
			GetTree().Quit((!flag3) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PrintErr("GROUND_HEIGHT_COMPONENT_HOT_PATH_EXCEPTION " + ex);
			GetTree().Quit(2);
		}
		finally
		{
			ReleaseWorkload();
		}
	}

	private static bool ScenarioPassed(BenchmarkResult result, bool expectDispatch)
	{
		int num = 1200000;
		if (result.ActiveInstances == (expectDispatch ? 1000 : 0) && result.EligibilityChecks == num && result.DispatchedInstances == (expectDispatch ? num : 0) && result.AllocatedBytes == 0L && result.Gen0 == 0 && result.Gen1 == 0 && result.Gen2 == 0)
		{
			return result.P99 < 0.2;
		}
		return false;
	}

	private static string FormatScenario(string name, BenchmarkResult result)
	{
		return $"{name}MeanMs={result.Mean:F6} {name}P50Ms={result.P50:F6} {name}P95Ms={result.P95:F6} {name}P99Ms={result.P99:F6} {name}MaxMs={result.Max:F6} {name}AllocatedBytes={result.AllocatedBytes} {name}Gen0={result.Gen0} {name}Gen1={result.Gen1} {name}Gen2={result.Gen2} {name}ActiveInstances={result.ActiveInstances} {name}EligibilityChecks={result.EligibilityChecks} {name}DispatchedInstances={result.DispatchedInstances} ";
	}

	private void CreateWorkload()
	{
		_ownerMount = new Node2D
		{
			Name = "GroundHeightOwnerMount"
		};
		AddChild(_ownerMount, forceReadableName: false, InternalMode.Disabled);
		_definition = new GroundHeightComponentDefinition
		{
			ComponentTypeId = "GroundHeightComponent",
			DefinitionId = "builtin.component.ground_height.hot_path",
			InstanceId = "character.ground_height",
			WireIndex = 0,
			InitiallyAlive = true,
			interpolationSpeed = 2f,
			threshold = 0.1f,
			waterHeight = 35f,
			ladderHeight = 60f,
			handleWaterHeight = true,
			handleLadder = false,
			detectWater = false,
			detectLadder = false
		};
		for (int i = 0; i < 1000; i++)
		{
			GroundHeightHotPathProbeOwner groundHeightHotPathProbeOwner = new GroundHeightHotPathProbeOwner
			{
				gridPos = new Vector2I(i % 9, 1 + i % 5),
				groundHeight = i % 7,
				ProcessMode = ProcessModeEnum.Disabled
			};
			_ownerMount.AddChild(groundHeightHotPathProbeOwner, forceReadableName: false, InternalMode.Disabled);
			ComponentManager componentManager = new ComponentManager();
			GroundHeightComponent groundHeightComponent = new GroundHeightComponent();
			groundHeightComponent.Bind(componentManager, groundHeightHotPathProbeOwner, _definition);
			groundHeightComponent.Activate();
			_owners[i] = groundHeightHotPathProbeOwner;
			_managers[i] = componentManager;
			_components[i] = groundHeightComponent;
		}
	}

	private bool RunFunctionalContract()
	{
		GroundHeightComponent groundHeightComponent = _components[0];
		TowerDefenseCharacter towerDefenseCharacter = _owners[0];
		ComponentManager manager = _managers[0];
		Vector2I gridPos = towerDefenseCharacter.gridPos;
		groundHeightComponent.interpolationSpeed = 1f;
		groundHeightComponent.threshold = 0.01f;
		towerDefenseCharacter.groundHeight = 10.0;
		groundHeightComponent.PhysicsProcess(0.5, 1uL);
		bool flag = Math.Abs(towerDefenseCharacter.groundHeight - 5.0) < 0.0001;
		towerDefenseCharacter.groundHeight = 0.005;
		groundHeightComponent.PhysicsProcess(1.0 / 60.0, 2uL);
		bool flag2 = Math.Abs(towerDefenseCharacter.groundHeight) < 0.0001;
		groundHeightComponent._waterExitCooldown = 0.25f;
		groundHeightComponent.PhysicsProcess(0.1, 3uL);
		bool flag3 = Math.Abs(groundHeightComponent._waterExitCooldown - 0.15f) < 0.0001f;
		towerDefenseCharacter.syncId = 23;
		towerDefenseCharacter.groundHeight = 12.34;
		towerDefenseCharacter.inWater = true;
		groundHeightComponent.onLadder = true;
		Dictionary dictionary = groundHeightComponent.SyncSerialize();
		Dictionary dictionary2 = groundHeightComponent.SyncSerialize();
		bool flag4 = dictionary == dictionary2 && dictionary.ContainsKey("height") && dictionary.ContainsKey("water") && dictionary.ContainsKey("ladder");
		groundHeightComponent.SyncDeserialize(new Dictionary
		{
			["height"] = 7.5,
			["water"] = false,
			["ladder"] = false
		});
		bool flag5 = Math.Abs(towerDefenseCharacter.groundHeight - 7.5) < 0.0001 && !towerDefenseCharacter.inWater && !groundHeightComponent.onLadder;
		groundHeightComponent.SetAlive(alive: false);
		towerDefenseCharacter.groundHeight = 4.0;
		groundHeightComponent.PhysicsProcess(0.5, 4uL);
		bool flag6 = Math.Abs(towerDefenseCharacter.groundHeight - 4.0) < 0.0001;
		groundHeightComponent.SetAlive(alive: true);
		groundHeightComponent.Detach(ComponentDetachReason.TemporaryTreeExit);
		bool flag7 = groundHeightComponent.parent == null && groundHeightComponent.Owner == null && groundHeightComponent.Lifecycle == ComponentRuntimeLifecycle.Detached;
		groundHeightComponent.Bind(manager, towerDefenseCharacter, _definition);
		groundHeightComponent.Activate();
		bool flag8 = groundHeightComponent.parent == towerDefenseCharacter && groundHeightComponent.Owner == towerDefenseCharacter && groundHeightComponent.Lifecycle == ComponentRuntimeLifecycle.Active;
		bool flag9 = towerDefenseCharacter.gridPos == gridPos;
		GD.Print("GROUND_HEIGHT_COMPONENT_FUNCTIONAL " + $"interpolation={flag} " + $"threshold={flag2} " + $"cooldown={flag3} " + $"payload={flag4} sync={flag5} " + $"inactive={flag6} detach={flag7} " + $"rebind={flag8} grid={flag9}");
		towerDefenseCharacter.syncId = -1;
		towerDefenseCharacter.inWater = false;
		towerDefenseCharacter.groundHeight = 0.0;
		groundHeightComponent.onLadder = false;
		groundHeightComponent._waterExitCooldown = 0f;
		groundHeightComponent.interpolationSpeed = 2f;
		groundHeightComponent.threshold = 0.1f;
		groundHeightComponent.handleWaterHeight = true;
		groundHeightComponent.handleLadder = false;
		groundHeightComponent.detectWater = false;
		groundHeightComponent.detectLadder = false;
		return flag & flag2 & flag3 & flag4 & flag5 & flag6 & flag7 & flag8 & flag9;
	}

	private BenchmarkResult BenchmarkScenarioState(BenchmarkScenario scenario)
	{
		ConfigureScenario(scenario);
		for (int i = 0; i < 240; i++)
		{
			PrepareSample(scenario);
			ulong physicsFrame = (ulong)i;
			for (int j = 0; j < 1000; j++)
			{
				GroundHeightComponent groundHeightComponent = _components[j];
				if (groundHeightComponent.HasRuntimePhysicsWork)
				{
					groundHeightComponent.PhysicsProcess(1.0 / 60.0, physicsFrame);
				}
			}
		}
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
		long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
		int num = GC.CollectionCount(0);
		int num2 = GC.CollectionCount(1);
		int num3 = GC.CollectionCount(2);
		int num4 = 0;
		int num5 = 0;
		for (int k = 0; k < 1200; k++)
		{
			PrepareSample(scenario);
			long timestamp = Stopwatch.GetTimestamp();
			ulong physicsFrame2 = (ulong)(240 + k);
			for (int l = 0; l < 1000; l++)
			{
				GroundHeightComponent groundHeightComponent2 = _components[l];
				num4++;
				if (groundHeightComponent2.HasRuntimePhysicsWork)
				{
					groundHeightComponent2.PhysicsProcess(1.0 / 60.0, physicsFrame2);
					num5++;
				}
			}
			_samples[k] = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
		}
		long allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread;
		int gen = GC.CollectionCount(0) - num;
		int gen2 = GC.CollectionCount(1) - num2;
		int gen3 = GC.CollectionCount(2) - num3;
		System.Array.Sort(_samples);
		double num6 = 0.0;
		for (int m = 0; m < _samples.Length; m++)
		{
			num6 += _samples[m];
		}
		num6 /= (double)_samples.Length;
		int num7 = 0;
		for (int n = 0; n < 1000; n++)
		{
			if (_components[n].HasRuntimePhysicsWork)
			{
				num7++;
			}
		}
		return new BenchmarkResult
		{
			Mean = num6,
			P50 = Percentile(0.5),
			P95 = Percentile(0.95),
			P99 = Percentile(0.99),
			Max = _samples[^1],
			AllocatedBytes = allocatedBytes,
			Gen0 = gen,
			Gen1 = gen2,
			Gen2 = gen3,
			ActiveInstances = num7,
			EligibilityChecks = num4,
			DispatchedInstances = num5
		};
	}

	private void ConfigureScenario(BenchmarkScenario scenario)
	{
		bool handleWaterHeight = scenario != BenchmarkScenario.Idle;
		for (int i = 0; i < 1000; i++)
		{
			GroundHeightComponent groundHeightComponent = _components[i];
			TowerDefenseCharacter obj = _owners[i];
			groundHeightComponent.handleWaterHeight = handleWaterHeight;
			groundHeightComponent.handleLadder = false;
			groundHeightComponent.detectWater = false;
			groundHeightComponent.detectLadder = false;
			groundHeightComponent.interpolationSpeed = 2f;
			groundHeightComponent.threshold = 0.1f;
			groundHeightComponent._waterExitCooldown = ((scenario == BenchmarkScenario.Cooldown) ? 0.5f : 0f);
			obj.inWater = false;
			obj.groundHeight = ((scenario == BenchmarkScenario.Interpolation) ? 10.0 : 0.0);
			groundHeightComponent.NotifyEnvironmentChanged();
			if (scenario == BenchmarkScenario.Idle && groundHeightComponent.HasRuntimePhysicsWork)
			{
				groundHeightComponent.PhysicsProcess(1.0 / 60.0, 0uL);
			}
		}
	}

	private void PrepareSample(BenchmarkScenario scenario)
	{
		switch (scenario)
		{
		case BenchmarkScenario.Interpolation:
		{
			for (int j = 0; j < 1000; j++)
			{
				_owners[j].groundHeight = 10.0;
			}
			break;
		}
		case BenchmarkScenario.Cooldown:
		{
			for (int i = 0; i < 1000; i++)
			{
				_components[i]._waterExitCooldown = 0.5f;
			}
			break;
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

	private int CountBoundActiveInstances()
	{
		int num = 0;
		for (int i = 0; i < 1000; i++)
		{
			GroundHeightComponent obj = _components[i];
			if (obj != null && obj.Lifecycle == ComponentRuntimeLifecycle.Active)
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
		_definition?.Dispose();
		if (GodotObject.IsInstanceValid(_ownerMount))
		{
			RemoveChild(_ownerMount);
			_ownerMount.Free();
		}
		_definition = null;
		_ownerMount = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateWorkload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunFunctionalContract, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "scenario", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareSample, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "scenario", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Percentile, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "fraction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountTreeOwners, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountBoundActiveInstances, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.Run && args.Count == 0)
		{
			Run();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateWorkload && args.Count == 0)
		{
			CreateWorkload();
			ret = default;
			return true;
		}
		if (method == MethodName.RunFunctionalContract && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunFunctionalContract());
			return true;
		}
		if (method == MethodName.ConfigureScenario && args.Count == 1)
		{
			ConfigureScenario(VariantUtils.ConvertTo<BenchmarkScenario>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareSample && args.Count == 1)
		{
			PrepareSample(VariantUtils.ConvertTo<BenchmarkScenario>(in args[0]));
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
		if (method == MethodName.CountBoundActiveInstances && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountBoundActiveInstances());
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
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.CreateWorkload)
		{
			return true;
		}
		if (method == MethodName.RunFunctionalContract)
		{
			return true;
		}
		if (method == MethodName.ConfigureScenario)
		{
			return true;
		}
		if (method == MethodName.PrepareSample)
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
		if (method == MethodName.CountBoundActiveInstances)
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
		if (name == PropertyName._definition)
		{
			_definition = VariantUtils.ConvertTo<GroundHeightComponentDefinition>(in value);
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
		if (name == PropertyName._definition)
		{
			value = VariantUtils.CreateFrom(in _definition);
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
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._owners, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._managers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedFloat64Array, PropertyName._samples, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ownerMount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
		info.AddProperty(PropertyName._ownerMount, Variant.From(in _ownerMount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._definition, out var value))
		{
			_definition = value.As<GroundHeightComponentDefinition>();
		}
		if (info.TryGetProperty(PropertyName._ownerMount, out var value2))
		{
			_ownerMount = value2.As<Node2D>();
		}
	}
}
