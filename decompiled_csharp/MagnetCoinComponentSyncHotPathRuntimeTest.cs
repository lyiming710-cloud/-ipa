using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/MagnetCoinComponentSyncHotPathRuntimeTest.cs")]
public class MagnetCoinComponentSyncHotPathRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";

		public static readonly StringName CreateWorkload = "CreateWorkload";

		public static readonly StringName RunFunctionalContract = "RunFunctionalContract";

		public static readonly StringName RunLifecycleContract = "RunLifecycleContract";

		public static readonly StringName RestoreBenchmarkFixture = "RestoreBenchmarkFixture";

		public static readonly StringName PrimePayloadCapacity = "PrimePayloadCapacity";

		public static readonly StringName CreateOwner = "CreateOwner";

		public static readonly StringName HasTimer = "HasTimer";

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

	private readonly MagnetCoinComponent[] _components = new MagnetCoinComponent[1000];

	private readonly TowerDefenseCharacter[] _owners = new TowerDefenseCharacter[1000];

	private readonly ComponentManager[] _managers = new ComponentManager[1000];

	private MagnetCoinComponentDefinition _definition;

	public override void _Ready()
	{
		Run();
	}

	private void Run()
	{
		bool flag = false;
		OptimizationBatchResult optimizationBatchResult = default;
		try
		{
			CreateWorkload();
			flag = RunFunctionalContract() && RestoreBenchmarkFixture();
			PrimePayloadCapacity();
			OptimizationBatchSampler.PrepareForWarmup();
			optimizationBatchResult = Benchmark();
			int num = CountTreeOwners();
			int num2 = CountActiveInstances();
			bool flag2 = flag && num == 1000 && num2 == 1000 && optimizationBatchResult.AllocatedBytes == 0L && optimizationBatchResult.Gen0Collections == 0 && optimizationBatchResult.Gen1Collections == 0 && optimizationBatchResult.Gen2Collections == 0 && optimizationBatchResult.P99Milliseconds < 0.2;
			GD.Print($"MAGNET_COIN_COMPONENT_SYNC_HOT_PATH_RESULT passed={flag2} functionalPassed={flag} instances={1000} treeOwners={num} activeInstances={num2} warmupSamples={240} measuredSamples={1200} meanMs={optimizationBatchResult.MeanMilliseconds:F6} p50Ms={optimizationBatchResult.P50Milliseconds:F6} p95Ms={optimizationBatchResult.P95Milliseconds:F6} p99Ms={optimizationBatchResult.P99Milliseconds:F6} maxMs={optimizationBatchResult.MaximumMilliseconds:F6} allocatedBytes={optimizationBatchResult.AllocatedBytes} gen0={optimizationBatchResult.Gen0Collections} gen1={optimizationBatchResult.Gen1Collections} gen2={optimizationBatchResult.Gen2Collections}");
			GetTree().Quit((!flag2) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PrintErr("MAGNET_COIN_COMPONENT_SYNC_HOT_PATH_EXCEPTION " + ex);
			GetTree().Quit(2);
		}
		finally
		{
			ReleaseWorkload();
		}
	}

	private void CreateWorkload()
	{
		_definition = new MagnetCoinComponentDefinition
		{
			ComponentTypeId = "MagnetCoinComponent",
			DefinitionId = "magnet_coin.sync.hotpath.runtime",
			InstanceId = "magnet_coin.sync.hotpath.runtime",
			WireIndex = 0,
			useRegistryCoinTypes = false,
			objectList = new int[1],
			magnetTime = 5f
		};
		for (int i = 0; i < 1000; i++)
		{
			MagnetCoinSyncProbeOwner magnetCoinSyncProbeOwner = CreateOwner();
			AddChild(magnetCoinSyncProbeOwner, forceReadableName: false, InternalMode.Disabled);
			ComponentManager componentManager = new ComponentManager();
			MagnetCoinComponent magnetCoinComponent = new MagnetCoinComponent();
			magnetCoinComponent.Bind(componentManager, magnetCoinSyncProbeOwner, _definition);
			magnetCoinComponent.Activate();
			_owners[i] = magnetCoinSyncProbeOwner;
			_managers[i] = componentManager;
			_components[i] = magnetCoinComponent;
		}
	}

	private bool RunFunctionalContract()
	{
		MagnetCoinComponent magnetCoinComponent = _components[0];
		Dictionary dictionary = magnetCoinComponent.SyncSerialize();
		bool flag = HasTimer(dictionary, 0f);
		dictionary["_alive"] = true;
		Dictionary dictionary2 = magnetCoinComponent.SyncSerialize();
		bool flag2 = dictionary == dictionary2 && !dictionary2.ContainsKey("_alive") && HasTimer(dictionary2, 0f);
		dictionary2["foreign"] = 99;
		Dictionary dictionary3 = magnetCoinComponent.SyncSerialize();
		bool flag3 = dictionary == dictionary3 && !dictionary3.ContainsKey("foreign") && HasTimer(dictionary3, 0f);
		magnetCoinComponent.SyncDeserialize(new Dictionary { ["timer"] = 2f });
		bool flag4 = magnetCoinComponent.timer == 2f && HasTimer(magnetCoinComponent.SyncSerialize(), 2f);
		Dictionary dictionary4 = magnetCoinComponent.ExportComponentSave();
		bool flag5 = dictionary4.Count == 1 && dictionary4["timer"].AsSingle() == 2f;
		magnetCoinComponent.ImportComponentSave(new Dictionary { ["timer"] = 3f }, null);
		flag5 = flag5 && magnetCoinComponent.timer == 3f;
		bool flag6 = RunLifecycleContract();
		return flag & flag2 & flag3 & flag4 & flag5 & flag6;
	}

	private bool RunLifecycleContract()
	{
		MagnetCoinSyncProbeOwner magnetCoinSyncProbeOwner = CreateOwner();
		AddChild(magnetCoinSyncProbeOwner, forceReadableName: false, InternalMode.Disabled);
		ComponentManager manager = new ComponentManager();
		MagnetCoinComponent magnetCoinComponent = new MagnetCoinComponent();
		magnetCoinComponent.Bind(manager, magnetCoinSyncProbeOwner, _definition);
		magnetCoinComponent.Activate();
		magnetCoinComponent.SyncDeserialize(new Dictionary { ["timer"] = 4f });
		magnetCoinComponent.Detach(ComponentDetachReason.TemporaryTreeExit);
		bool num = magnetCoinComponent.Lifecycle == ComponentRuntimeLifecycle.Detached && magnetCoinComponent.timer == 4f && HasTimer(magnetCoinComponent.SyncSerialize(), 4f);
		MagnetCoinSyncProbeOwner magnetCoinSyncProbeOwner2 = CreateOwner();
		AddChild(magnetCoinSyncProbeOwner2, forceReadableName: false, InternalMode.Disabled);
		ComponentManager manager2 = new ComponentManager();
		magnetCoinComponent.Bind(manager2, magnetCoinSyncProbeOwner2, _definition);
		magnetCoinComponent.Activate();
		bool flag = magnetCoinComponent.IsAttached && !magnetCoinComponent.IsReleased && magnetCoinComponent.timer == 4f && HasTimer(magnetCoinComponent.SyncSerialize(), 4f);
		magnetCoinComponent.Release();
		bool flag2 = magnetCoinComponent.IsReleased && magnetCoinComponent.timer == 4f && HasTimer(magnetCoinComponent.SyncSerialize(), 4f);
		magnetCoinSyncProbeOwner.Free();
		magnetCoinSyncProbeOwner2.Free();
		return num & flag & flag2;
	}

	private bool RestoreBenchmarkFixture()
	{
		for (int i = 0; i < 1000; i++)
		{
			MagnetCoinComponent magnetCoinComponent = _components[i];
			magnetCoinComponent.timer = 1f;
			magnetCoinComponent.SetAlive(alive: true);
			if (magnetCoinComponent.Lifecycle != ComponentRuntimeLifecycle.Active || !HasTimer(magnetCoinComponent.SyncSerialize(), 1f))
			{
				return false;
			}
		}
		return true;
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

	private static MagnetCoinSyncProbeOwner CreateOwner()
	{
		MagnetCoinSyncProbeOwner magnetCoinSyncProbeOwner = new MagnetCoinSyncProbeOwner
		{
			camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT,
			instance = new TowerDefenseCharacterInstance(),
			ProcessMode = ProcessModeEnum.Disabled
		};
		magnetCoinSyncProbeOwner.instance.character = magnetCoinSyncProbeOwner;
		return magnetCoinSyncProbeOwner;
	}

	private static bool HasTimer(Dictionary payload, float timer)
	{
		if (payload.Count == 1 && payload.TryGetValue("timer", out var value) && value.VariantType == Variant.Type.Float)
		{
			return value.AsSingle() == timer;
		}
		return false;
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

	private int CountActiveInstances()
	{
		int num = 0;
		for (int i = 0; i < 1000; i++)
		{
			MagnetCoinComponent obj = _components[i];
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
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateWorkload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunFunctionalContract, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunLifecycleContract, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreBenchmarkFixture, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrimePayloadCapacity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateOwner, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.HasTimer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "payload", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "timer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RestoreBenchmarkFixture && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RestoreBenchmarkFixture());
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
			ret = VariantUtils.CreateFrom<MagnetCoinSyncProbeOwner>(CreateOwner());
			return true;
		}
		if (method == MethodName.HasTimer && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasTimer(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
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
		if (method == MethodName.CreateOwner && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<MagnetCoinSyncProbeOwner>(CreateOwner());
			return true;
		}
		if (method == MethodName.HasTimer && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasTimer(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
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
		if (method == MethodName.PrimePayloadCapacity)
		{
			return true;
		}
		if (method == MethodName.CreateOwner)
		{
			return true;
		}
		if (method == MethodName.HasTimer)
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
			_definition = VariantUtils.ConvertTo<MagnetCoinComponentDefinition>(in value);
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
			_definition = value.As<MagnetCoinComponentDefinition>();
		}
	}
}
