using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/RandomTransformationComponentSyncHotPathRuntimeTest.cs")]
public class RandomTransformationComponentSyncHotPathRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";

		public static readonly StringName CreateWorkload = "CreateWorkload";

		public static readonly StringName RunFunctionalContract = "RunFunctionalContract";

		public static readonly StringName RunLifecycleContract = "RunLifecycleContract";

		public static readonly StringName RestoreBenchmarkFixture = "RestoreBenchmarkFixture";

		public static readonly StringName RunClientPendingAuthorityContract = "RunClientPendingAuthorityContract";

		public static readonly StringName ToManagedNames = "ToManagedNames";

		public static readonly StringName PrimePayloadCapacity = "PrimePayloadCapacity";

		public static readonly StringName CreateOwner = "CreateOwner";

		public static readonly StringName HasNames = "HasNames";

		public static readonly StringName CountTreeOwners = "CountTreeOwners";

		public static readonly StringName CountActiveInstances = "CountActiveInstances";

		public static readonly StringName ReleaseWorkload = "ReleaseWorkload";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _owners = "_owners";

		public static readonly StringName _managers = "_managers";

		public static readonly StringName _definition = "_definition";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSamples = 240;

	private const int MeasuredSamples = 1200;

	private readonly RandomTransformationComponent[] _components = new RandomTransformationComponent[1000];

	private readonly RandomTransformationSyncProbeOwner[] _owners = new RandomTransformationSyncProbeOwner[1000];

	private readonly ComponentManager[] _managers = new ComponentManager[1000];

	private RandomTransformationComponentDefinition _definition;

	public override async void _Ready()
	{
		for (int frame = 0; frame < 30; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		Run();
	}

	private void Run()
	{
		bool flag = false;
		bool flag2 = false;
		OptimizationBatchResult[] array = new OptimizationBatchResult[14];
		bool isMultiplayerMode = Global.Instance?.isMultiplayerMode ?? false;
		bool isHost = MultiPlayerManager.Instance?.isHost ?? false;
		try
		{
			if (!GodotObject.IsInstanceValid(Global.Instance) || !GodotObject.IsInstanceValid(MultiPlayerManager.Instance))
			{
				throw new InvalidOperationException("Multiplayer authority autoloads are unavailable.");
			}
			Global.Instance.isMultiplayerMode = true;
			MultiPlayerManager.Instance.isHost = true;
			CreateWorkload();
			flag = RunFunctionalContract();
			bool flag3 = BenchmarkStateTwice(new Array<string>(), array, 0);
			bool flag4 = BenchmarkStateTwice(new Array<string> { "pea" }, array, 2);
			bool flag5 = BenchmarkStateTwice(new Array<string> { "pea", "sun", "star" }, array, 4);
			int num = CountTreeOwners();
			int num2 = CountActiveInstances();
			ReleaseWorkload();
			MultiPlayerManager.Instance.isHost = false;
			CreateWorkload();
			flag2 = RunClientPendingAuthorityContract();
			array[6] = BenchmarkPreparedState();
			array[7] = BenchmarkPreparedState();
			bool flag6 = BenchmarkStateTwice(new Array<string>(), array, 8);
			bool flag7 = BenchmarkStateTwice(new Array<string> { "pea" }, array, 10);
			bool flag8 = BenchmarkStateTwice(new Array<string> { "pea", "sun", "star" }, array, 12);
			int num3 = CountTreeOwners();
			int num4 = CountActiveInstances();
			bool flag9 = true;
			double num5 = 0.0;
			long num6 = 0L;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				flag9 &= ResultPassed(array[i]);
				num5 = Math.Max(num5, array[i].P99Milliseconds);
				num6 += array[i].AllocatedBytes;
				num7 += array[i].Gen0Collections;
				num8 += array[i].Gen1Collections;
				num9 += array[i].Gen2Collections;
			}
			bool flag10 = ((flag & flag2) && num == 1000 && num2 == 1000 && num3 == 1000 && num4 == 1000) & flag3 & flag4 & flag5 & flag6 & flag7 & flag8 & flag9;
			GD.Print($"RANDOM_TRANSFORMATION_COMPONENT_SYNC_HOT_PATH_RESULT passed={flag10} functionalPassed={flag} authorityFunctionalPassed={flag2} instances={1000} hostTreeOwners={num} hostActiveInstances={num2} clientTreeOwners={num3} clientActiveInstances={num4} warmupSamples={240} measuredSamples={1200} hostEmptyRound1P99Ms={array[0].P99Milliseconds:F6} hostEmptyRound2P99Ms={array[1].P99Milliseconds:F6} hostSingleRound1P99Ms={array[2].P99Milliseconds:F6} hostSingleRound2P99Ms={array[3].P99Milliseconds:F6} hostMultiRound1P99Ms={array[4].P99Milliseconds:F6} hostMultiRound2P99Ms={array[5].P99Milliseconds:F6} clientPendingRound1P99Ms={array[6].P99Milliseconds:F6} clientPendingRound2P99Ms={array[7].P99Milliseconds:F6} clientEmptyRound1P99Ms={array[8].P99Milliseconds:F6} clientEmptyRound2P99Ms={array[9].P99Milliseconds:F6} clientSingleRound1P99Ms={array[10].P99Milliseconds:F6} clientSingleRound2P99Ms={array[11].P99Milliseconds:F6} clientMultiRound1P99Ms={array[12].P99Milliseconds:F6} clientMultiRound2P99Ms={array[13].P99Milliseconds:F6} p99Ms={num5:F6} allocatedBytes={num6} gen0={num7} gen1={num8} gen2={num9}");
			GetTree().Quit((!flag10) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PrintErr("RANDOM_TRANSFORMATION_COMPONENT_SYNC_HOT_PATH_EXCEPTION " + ex);
			GetTree().Quit(2);
		}
		finally
		{
			ReleaseWorkload();
			if (GodotObject.IsInstanceValid(Global.Instance))
			{
				Global.Instance.isMultiplayerMode = isMultiplayerMode;
			}
			if (GodotObject.IsInstanceValid(MultiPlayerManager.Instance))
			{
				MultiPlayerManager.Instance.isHost = isHost;
			}
		}
	}

	private void CreateWorkload()
	{
		_definition = new RandomTransformationComponentDefinition
		{
			ComponentTypeId = "RandomTransformationComponent",
			DefinitionId = "random_transformation.sync.hotpath.runtime",
			InstanceId = "random_transformation.sync.hotpath.runtime",
			WireIndex = 0,
			includePacketNameList = new Array<string> { "pea", "sun" }
		};
		for (int i = 0; i < 1000; i++)
		{
			RandomTransformationSyncProbeOwner randomTransformationSyncProbeOwner = CreateOwner();
			AddChild(randomTransformationSyncProbeOwner, forceReadableName: false, InternalMode.Disabled);
			ComponentManager componentManager = new ComponentManager();
			RandomTransformationComponent randomTransformationComponent = new RandomTransformationComponent();
			randomTransformationComponent.Bind(componentManager, randomTransformationSyncProbeOwner, _definition);
			randomTransformationComponent.Activate();
			_owners[i] = randomTransformationSyncProbeOwner;
			_managers[i] = componentManager;
			_components[i] = randomTransformationComponent;
		}
	}

	private bool RunFunctionalContract()
	{
		RandomTransformationComponent randomTransformationComponent = _components[0];
		Dictionary dictionary = randomTransformationComponent.SyncSerialize();
		bool flag = HasNames(dictionary, "pea", "sun");
		dictionary["_alive"] = true;
		Dictionary dictionary2 = randomTransformationComponent.SyncSerialize();
		bool flag2 = dictionary == dictionary2 && !dictionary2.ContainsKey("_alive") && HasNames(dictionary2, "pea", "sun");
		dictionary2["foreign"] = 99;
		Dictionary dictionary3 = randomTransformationComponent.SyncSerialize();
		bool flag3 = dictionary == dictionary3 && !dictionary3.ContainsKey("foreign") && HasNames(dictionary3, "pea", "sun");
		randomTransformationComponent.packetList.Add("star");
		bool flag4 = HasNames(randomTransformationComponent.SyncSerialize(), "pea", "sun", "star");
		randomTransformationComponent.SyncDeserialize(new Dictionary { ["packetList"] = new Array<string>
		{
			"moon",
			"moon",
			string.Empty
		} });
		bool flag5 = HasNames(randomTransformationComponent.SyncSerialize(), "moon");
		Dictionary payload = randomTransformationComponent.ExportComponentSave();
		randomTransformationComponent.ImportComponentSave(new Dictionary { ["packetList"] = new Array<string> { "pea", "star" } }, null);
		bool flag6 = HasNames(payload, "moon") && HasNames(randomTransformationComponent.SyncSerialize(), "pea", "star");
		randomTransformationComponent.SetAlive(alive: false);
		bool flag7 = !randomTransformationComponent.Alive && HasNames(randomTransformationComponent.SyncSerialize(), "pea", "star");
		randomTransformationComponent.SetAlive(alive: true);
		if (flag & flag2 & flag3 & flag4 & flag5 & flag6 & flag7)
		{
			return RunLifecycleContract();
		}
		return false;
	}

	private bool RunLifecycleContract()
	{
		RandomTransformationSyncProbeOwner randomTransformationSyncProbeOwner = CreateOwner();
		AddChild(randomTransformationSyncProbeOwner, forceReadableName: false, InternalMode.Disabled);
		ComponentManager manager = new ComponentManager();
		RandomTransformationComponent randomTransformationComponent = new RandomTransformationComponent();
		randomTransformationComponent.Bind(manager, randomTransformationSyncProbeOwner, _definition);
		randomTransformationComponent.Activate();
		randomTransformationComponent.SyncDeserialize(new Dictionary { ["packetList"] = new Array<string> { "moon" } });
		randomTransformationComponent.Detach(ComponentDetachReason.TemporaryTreeExit);
		bool flag = randomTransformationComponent.Lifecycle == ComponentRuntimeLifecycle.Detached && HasNames(randomTransformationComponent.SyncSerialize(), "moon");
		RandomTransformationSyncProbeOwner randomTransformationSyncProbeOwner2 = CreateOwner();
		AddChild(randomTransformationSyncProbeOwner2, forceReadableName: false, InternalMode.Disabled);
		ComponentManager manager2 = new ComponentManager();
		randomTransformationComponent.Bind(manager2, randomTransformationSyncProbeOwner2, _definition);
		randomTransformationComponent.Activate();
		bool flag2 = randomTransformationComponent.IsAttached && !randomTransformationComponent.IsReleased && HasNames(randomTransformationComponent.SyncSerialize(), "moon");
		randomTransformationComponent.Release();
		bool flag3 = randomTransformationComponent.IsReleased && HasNames(randomTransformationComponent.SyncSerialize());
		randomTransformationSyncProbeOwner.Free();
		randomTransformationSyncProbeOwner2.Free();
		return flag & flag2 & flag3;
	}

	private bool RestoreBenchmarkFixture(Array<string> names)
	{
		Dictionary data = new Dictionary { ["packetList"] = names };
		string[] expected = ToManagedNames(names);
		for (int i = 0; i < 1000; i++)
		{
			RandomTransformationComponent randomTransformationComponent = _components[i];
			randomTransformationComponent.SyncDeserialize(data);
			if (randomTransformationComponent.Lifecycle != ComponentRuntimeLifecycle.Active || !HasNames(randomTransformationComponent.SyncSerialize(), expected))
			{
				return false;
			}
		}
		return true;
	}

	private bool BenchmarkStateTwice(Array<string> names, OptimizationBatchResult[] results, int resultOffset)
	{
		if (!RestoreBenchmarkFixture(names))
		{
			return false;
		}
		results[resultOffset] = BenchmarkPreparedState();
		results[resultOffset + 1] = BenchmarkPreparedState();
		return true;
	}

	private bool RunClientPendingAuthorityContract()
	{
		RandomTransformationSyncProbeOwner randomTransformationSyncProbeOwner = CreateOwner();
		AddChild(randomTransformationSyncProbeOwner, forceReadableName: false, InternalMode.Disabled);
		ComponentManager manager = new ComponentManager();
		RandomTransformationComponent randomTransformationComponent = new RandomTransformationComponent();
		randomTransformationComponent.Bind(manager, randomTransformationSyncProbeOwner, _definition);
		randomTransformationComponent.Activate();
		Dictionary dictionary = randomTransformationComponent.SyncSerialize();
		bool flag = Global.IsMultiplayerMode && !MultiPlayerManager.IsHost && HasNames(dictionary) && randomTransformationComponent.GetRandomPacketName() == string.Empty && randomTransformationComponent.includePacketNameList.Count == 2;
		dictionary["_alive"] = true;
		bool flag2 = dictionary == randomTransformationComponent.SyncSerialize() && !dictionary.ContainsKey("_alive") && HasNames(dictionary);
		randomTransformationComponent.SetAlive(alive: false);
		bool flag3 = !randomTransformationComponent.Alive && HasNames(randomTransformationComponent.SyncSerialize());
		randomTransformationComponent.SetAlive(alive: true);
		randomTransformationComponent.SyncDeserialize(new Dictionary { ["packetList"] = new Array<string> { "moon" } });
		bool flag4 = HasNames(randomTransformationComponent.SyncSerialize(), "moon");
		randomTransformationComponent.Detach(ComponentDetachReason.TemporaryTreeExit);
		bool flag5 = randomTransformationComponent.Lifecycle == ComponentRuntimeLifecycle.Detached && HasNames(randomTransformationComponent.SyncSerialize(), "moon");
		RandomTransformationSyncProbeOwner randomTransformationSyncProbeOwner2 = CreateOwner();
		AddChild(randomTransformationSyncProbeOwner2, forceReadableName: false, InternalMode.Disabled);
		ComponentManager manager2 = new ComponentManager();
		randomTransformationComponent.Bind(manager2, randomTransformationSyncProbeOwner2, _definition);
		randomTransformationComponent.Activate();
		bool flag6 = randomTransformationComponent.IsAttached && HasNames(randomTransformationComponent.SyncSerialize(), "moon");
		randomTransformationComponent.Release();
		bool flag7 = randomTransformationComponent.IsReleased && HasNames(randomTransformationComponent.SyncSerialize());
		randomTransformationSyncProbeOwner.Free();
		randomTransformationSyncProbeOwner2.Free();
		return flag & flag2 & flag3 & flag4 & flag5 & flag6 & flag7;
	}

	private OptimizationBatchResult BenchmarkPreparedState()
	{
		PrimePayloadCapacity();
		OptimizationBatchSampler.PrepareForWarmup();
		return Benchmark();
	}

	private static bool ResultPassed(OptimizationBatchResult result)
	{
		if (result.AllocatedBytes == 0L && result.Gen0Collections == 0 && result.Gen1Collections == 0 && result.Gen2Collections == 0)
		{
			return result.P99Milliseconds < 0.2;
		}
		return false;
	}

	private static string[] ToManagedNames(Array<string> names)
	{
		string[] array = new string[names.Count];
		for (int i = 0; i < names.Count; i++)
		{
			array[i] = names[i];
		}
		return array;
	}

	private void PrimePayloadCapacity()
	{
		for (int i = 0; i < 240; i++)
		{
			for (int j = 0; j < 1000; j++)
			{
				_components[j].SyncSerialize();
			}
		}
	}

	private OptimizationBatchResult Benchmark()
	{
		OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
		optimizationBatchSampler.BeginMeasurement();
		for (int i = 0; i < 1200; i++)
		{
			long startTicks = OptimizationBatchSampler.BeginSample();
			for (int j = 0; j < 1000; j++)
			{
				_components[j].SyncSerialize();
			}
			optimizationBatchSampler.EndSample(startTicks);
		}
		return optimizationBatchSampler.Complete();
	}

	private static RandomTransformationSyncProbeOwner CreateOwner()
	{
		RandomTransformationSyncProbeOwner randomTransformationSyncProbeOwner = new RandomTransformationSyncProbeOwner
		{
			camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT,
			instance = new TowerDefenseCharacterInstance(),
			ProcessMode = ProcessModeEnum.Disabled
		};
		randomTransformationSyncProbeOwner.instance.character = randomTransformationSyncProbeOwner;
		return randomTransformationSyncProbeOwner;
	}

	private static bool HasNames(Dictionary payload, params string[] expected)
	{
		if (payload.Count != 1 || !payload.TryGetValue("packetList", out var value))
		{
			return false;
		}
		Array<string> array = value.AsGodotArray<string>();
		if (array.Count != expected.Length)
		{
			return false;
		}
		for (int i = 0; i < expected.Length; i++)
		{
			if (array[i] != expected[i])
			{
				return false;
			}
		}
		return true;
	}

	private int CountTreeOwners()
	{
		int num = 0;
		for (int i = 0; i < 1000; i++)
		{
			RandomTransformationSyncProbeOwner obj = _owners[i];
			if (obj != null && obj.IsInsideTree())
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
			RandomTransformationComponent obj = _components[i];
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
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateWorkload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunFunctionalContract, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunLifecycleContract, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreBenchmarkFixture, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "names", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunClientPendingAuthorityContract, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToManagedNames, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "names", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrimePayloadCapacity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateOwner, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.HasNames, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "payload", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountTreeOwners, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountActiveInstances, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.RunLifecycleContract && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunLifecycleContract());
			return true;
		}
		if (method == MethodName.RestoreBenchmarkFixture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RestoreBenchmarkFixture(VariantUtils.ConvertToArray<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RunClientPendingAuthorityContract && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunClientPendingAuthorityContract());
			return true;
		}
		if (method == MethodName.ToManagedNames && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(ToManagedNames(VariantUtils.ConvertToArray<string>(in args[0])));
			return true;
		}
		if (method == MethodName.PrimePayloadCapacity && args.Count == 0)
		{
			PrimePayloadCapacity();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateOwner && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<RandomTransformationSyncProbeOwner>(CreateOwner());
			return true;
		}
		if (method == MethodName.HasNames && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasNames(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
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
		if (method == MethodName.ToManagedNames && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(ToManagedNames(VariantUtils.ConvertToArray<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateOwner && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<RandomTransformationSyncProbeOwner>(CreateOwner());
			return true;
		}
		if (method == MethodName.HasNames && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasNames(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
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
		if (method == MethodName.RunLifecycleContract)
		{
			return true;
		}
		if (method == MethodName.RestoreBenchmarkFixture)
		{
			return true;
		}
		if (method == MethodName.RunClientPendingAuthorityContract)
		{
			return true;
		}
		if (method == MethodName.ToManagedNames)
		{
			return true;
		}
		if (method == MethodName.PrimePayloadCapacity)
		{
			return true;
		}
		if (method == MethodName.CreateOwner)
		{
			return true;
		}
		if (method == MethodName.HasNames)
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
		if (name == PropertyName._definition)
		{
			_definition = VariantUtils.ConvertTo<RandomTransformationComponentDefinition>(in value);
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
		if (name == PropertyName._definition)
		{
			value = VariantUtils.CreateFrom(in _definition);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._definition, out var value))
		{
			_definition = value.As<RandomTransformationComponentDefinition>();
		}
	}
}
