using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/GarlicComponentSyncHotPathRuntimeTest.cs")]
public class GarlicComponentSyncHotPathRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";

		public static readonly StringName CreateWorkload = "CreateWorkload";

		public static readonly StringName RunFunctionalContract = "RunFunctionalContract";

		public static readonly StringName RestoreBenchmarkFixture = "RestoreBenchmarkFixture";

		public static readonly StringName PrimePayloadCapacity = "PrimePayloadCapacity";

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

	private readonly GarlicComponent[] _components = new GarlicComponent[1000];

	private readonly TowerDefenseZombie[] _owners = new TowerDefenseZombie[1000];

	private readonly ComponentManager[] _managers = new ComponentManager[1000];

	private GarlicComponentDefinition _definition;

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
			flag = RunFunctionalContract();
			RestoreBenchmarkFixture();
			PrimePayloadCapacity();
			OptimizationBatchSampler.PrepareForWarmup();
			optimizationBatchResult = Benchmark();
			int num = CountTreeOwners();
			int num2 = CountActiveInstances();
			bool flag2 = flag && num == 1000 && num2 == 1000 && optimizationBatchResult.AllocatedBytes == 0L && optimizationBatchResult.Gen0Collections == 0 && optimizationBatchResult.Gen1Collections == 0 && optimizationBatchResult.Gen2Collections == 0 && optimizationBatchResult.P99Milliseconds < 0.2;
			GD.Print($"GARLIC_COMPONENT_SYNC_HOT_PATH_RESULT passed={flag2} functionalPassed={flag} instances={1000} treeOwners={num} activeInstances={num2} warmupSamples={240} measuredSamples={1200} meanMs={optimizationBatchResult.MeanMilliseconds:F6} p50Ms={optimizationBatchResult.P50Milliseconds:F6} p95Ms={optimizationBatchResult.P95Milliseconds:F6} p99Ms={optimizationBatchResult.P99Milliseconds:F6} maxMs={optimizationBatchResult.MaximumMilliseconds:F6} allocatedBytes={optimizationBatchResult.AllocatedBytes} gen0={optimizationBatchResult.Gen0Collections} gen1={optimizationBatchResult.Gen1Collections} gen2={optimizationBatchResult.Gen2Collections}");
			GetTree().Quit((!flag2) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PrintErr("GARLIC_COMPONENT_SYNC_HOT_PATH_EXCEPTION " + ex);
			GetTree().Quit(2);
		}
		finally
		{
			ReleaseWorkload();
		}
	}

	private void CreateWorkload()
	{
		_definition = new GarlicComponentDefinition
		{
			ComponentTypeId = "GarlicComponent",
			DefinitionId = "garlic.sync.hotpath.runtime",
			InstanceId = "garlic.sync.hotpath.runtime",
			WireIndex = 0
		};
		for (int i = 0; i < 1000; i++)
		{
			GarlicSyncProbeOwner garlicSyncProbeOwner = new GarlicSyncProbeOwner
			{
				camp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE,
				instance = new TowerDefenseCharacterInstance(),
				ProcessMode = ProcessModeEnum.Disabled
			};
			garlicSyncProbeOwner.instance.character = garlicSyncProbeOwner;
			AddChild(garlicSyncProbeOwner, forceReadableName: false, InternalMode.Disabled);
			ComponentManager componentManager = new ComponentManager();
			GarlicComponent garlicComponent = new GarlicComponent();
			garlicComponent.Bind(componentManager, garlicSyncProbeOwner, _definition);
			garlicComponent.Activate();
			garlicComponent._syncMoveDir = 1;
			_owners[i] = garlicSyncProbeOwner;
			_managers[i] = componentManager;
			_components[i] = garlicComponent;
		}
	}

	private bool RunFunctionalContract()
	{
		GarlicComponent garlicComponent = _components[0];
		Dictionary dictionary = garlicComponent.SyncSerialize();
		bool num = dictionary.Count == 2 && dictionary.ContainsKey("move_dir") && dictionary["move_dir"].AsInt32() == 1 && dictionary.ContainsKey("is_change_line") && !dictionary["is_change_line"].AsBool();
		dictionary["_alive"] = true;
		Dictionary dictionary2 = garlicComponent.SyncSerialize();
		bool flag = dictionary == dictionary2 && dictionary2.Count == 2 && !dictionary2.ContainsKey("_alive");
		dictionary2["foreign"] = true;
		Dictionary dictionary3 = garlicComponent.SyncSerialize();
		bool flag2 = dictionary2 == dictionary3 && dictionary3.Count == 2 && !dictionary3.ContainsKey("foreign");
		garlicComponent.SyncDeserialize(new Dictionary
		{
			["move_dir"] = -1,
			["is_change_line"] = true
		});
		Dictionary dictionary4 = garlicComponent.SyncSerialize();
		bool flag3 = dictionary4["move_dir"].AsInt32() == -1 && dictionary4["is_change_line"].AsBool() && garlicComponent.parent.isChangeLine;
		bool flag4 = garlicComponent.ExportComponentSave().Count == 0;
		garlicComponent.Detach(ComponentDetachReason.TemporaryTreeExit);
		bool flag5 = garlicComponent.Lifecycle == ComponentRuntimeLifecycle.Detached && garlicComponent.SyncSerialize().Count == 2 && !garlicComponent.SyncSerialize()["is_change_line"].AsBool();
		ComponentManager manager = new ComponentManager();
		GarlicSyncProbeOwner garlicSyncProbeOwner = new GarlicSyncProbeOwner
		{
			camp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE,
			instance = new TowerDefenseCharacterInstance(),
			ProcessMode = ProcessModeEnum.Disabled
		};
		garlicSyncProbeOwner.instance.character = garlicSyncProbeOwner;
		AddChild(garlicSyncProbeOwner, forceReadableName: false, InternalMode.Disabled);
		garlicComponent.Bind(manager, garlicSyncProbeOwner, _definition);
		garlicComponent.Activate();
		bool flag6 = garlicComponent.IsAttached && !garlicComponent.IsReleased && garlicComponent.SyncSerialize().Count == 2;
		garlicComponent.Release();
		bool isReleased = garlicComponent.IsReleased;
		garlicSyncProbeOwner.Free();
		return num & flag & flag2 & flag3 & flag4 & flag5 & flag6 & isReleased;
	}

	private void RestoreBenchmarkFixture()
	{
		_owners[0].isChangeLine = false;
		GarlicComponent garlicComponent = new GarlicComponent
		{
			_syncMoveDir = 1
		};
		garlicComponent.Bind(_managers[0], _owners[0], _definition);
		garlicComponent.Activate();
		_components[0] = garlicComponent;
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

	private int CountTreeOwners()
	{
		int num = 0;
		for (int i = 0; i < 1000; i++)
		{
			TowerDefenseZombie obj = _owners[i];
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
			GarlicComponent obj = _components[i];
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
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateWorkload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunFunctionalContract, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreBenchmarkFixture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrimePayloadCapacity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.RestoreBenchmarkFixture && args.Count == 0)
		{
			RestoreBenchmarkFixture();
			ret = default;
			return true;
		}
		if (method == MethodName.PrimePayloadCapacity && args.Count == 0)
		{
			PrimePayloadCapacity();
			ret = default;
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
		if (method == MethodName.RestoreBenchmarkFixture)
		{
			return true;
		}
		if (method == MethodName.PrimePayloadCapacity)
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
			_definition = VariantUtils.ConvertTo<GarlicComponentDefinition>(in value);
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
			_definition = value.As<GarlicComponentDefinition>();
		}
	}
}
