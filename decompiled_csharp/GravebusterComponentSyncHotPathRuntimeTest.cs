using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/GravebusterComponentSyncHotPathRuntimeTest.cs")]
public class GravebusterComponentSyncHotPathRuntimeTest : Node
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

		public static readonly StringName _graveStone = "_graveStone";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSamples = 240;

	private const int MeasuredSamples = 1200;

	private readonly GravebusterComponent[] _components = new GravebusterComponent[1000];

	private readonly TowerDefenseCharacter[] _owners = new TowerDefenseCharacter[1000];

	private readonly ComponentManager[] _managers = new ComponentManager[1000];

	private GravebusterComponentDefinition _definition;

	private TowerDefenseGravestone _graveStone;

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
			GD.Print($"GRAVEBUSTER_COMPONENT_SYNC_HOT_PATH_RESULT passed={flag2} functionalPassed={flag} instances={1000} treeOwners={num} activeInstances={num2} warmupSamples={240} measuredSamples={1200} meanMs={optimizationBatchResult.MeanMilliseconds:F6} p50Ms={optimizationBatchResult.P50Milliseconds:F6} p95Ms={optimizationBatchResult.P95Milliseconds:F6} p99Ms={optimizationBatchResult.P99Milliseconds:F6} maxMs={optimizationBatchResult.MaximumMilliseconds:F6} allocatedBytes={optimizationBatchResult.AllocatedBytes} gen0={optimizationBatchResult.Gen0Collections} gen1={optimizationBatchResult.Gen1Collections} gen2={optimizationBatchResult.Gen2Collections}");
			GetTree().Quit((!flag2) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PrintErr("GRAVEBUSTER_COMPONENT_SYNC_HOT_PATH_EXCEPTION " + ex);
			GetTree().Quit(2);
		}
		finally
		{
			ReleaseWorkload();
		}
	}

	private void CreateWorkload()
	{
		_definition = ResourceLoader.Load<GravebusterComponentDefinition>("res://Script/Component/TowerDefense/Character/GravebusterComponent/GravebusterComponentDefinition.tres", null, ResourceLoader.CacheMode.Reuse);
		if (_definition == null)
		{
			throw new InvalidOperationException("Gravebuster definition resource failed to load.");
		}
		_graveStone = new TowerDefenseGravestone
		{
			syncId = 4321
		};
		for (int i = 0; i < 1000; i++)
		{
			GravebusterSyncProbeOwner gravebusterSyncProbeOwner = new GravebusterSyncProbeOwner
			{
				camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT,
				instance = new TowerDefenseCharacterInstance(),
				ProcessMode = ProcessModeEnum.Disabled
			};
			gravebusterSyncProbeOwner.instance.character = gravebusterSyncProbeOwner;
			AddChild(gravebusterSyncProbeOwner, forceReadableName: false, InternalMode.Disabled);
			ComponentManager componentManager = new ComponentManager();
			GravebusterComponent gravebusterComponent = new GravebusterComponent();
			gravebusterComponent.Bind(componentManager, gravebusterSyncProbeOwner, _definition);
			gravebusterComponent.Activate();
			gravebusterComponent.RegisterStateRuntime();
			gravebusterComponent.graveStone = _graveStone;
			gravebusterComponent.SyncDeserialize(new Dictionary
			{
				["drop_velocity_x"] = 12.5f,
				["drop_velocity_y"] = -30f
			});
			_owners[i] = gravebusterSyncProbeOwner;
			_managers[i] = componentManager;
			_components[i] = gravebusterComponent;
		}
	}

	private bool RunFunctionalContract()
	{
		GravebusterComponent gravebusterComponent = _components[0];
		Dictionary dictionary = gravebusterComponent.SyncSerialize();
		bool num = dictionary.Count == 4 && dictionary.ContainsKey("state") && dictionary["state"].AsString() == "gravebuster.idle" && dictionary.ContainsKey("grave_stone_sync_id") && dictionary["grave_stone_sync_id"].AsInt32() == 4321 && dictionary.ContainsKey("drop_velocity_x") && dictionary.ContainsKey("drop_velocity_y");
		dictionary["_alive"] = true;
		Dictionary dictionary2 = gravebusterComponent.SyncSerialize();
		bool flag = dictionary == dictionary2 && dictionary2.Count == 4 && !dictionary2.ContainsKey("_alive");
		dictionary2["foreign"] = true;
		Dictionary dictionary3 = gravebusterComponent.SyncSerialize();
		bool flag2 = dictionary2 == dictionary3 && dictionary3.Count == 4 && !dictionary3.ContainsKey("foreign");
		bool flag3 = gravebusterComponent.SendStateEvent("ToGravebuster");
		Dictionary dictionary4 = gravebusterComponent.SyncSerialize();
		bool flag4 = flag3 && dictionary4["state"].AsString() == "gravebuster.consume" && dictionary4["grave_stone_sync_id"].AsInt32() == 4321;
		gravebusterComponent.SyncDeserialize(new Dictionary
		{
			["drop_velocity_x"] = 0f,
			["drop_velocity_y"] = 0f
		});
		Dictionary dictionary5 = gravebusterComponent.SyncSerialize();
		bool flag5 = dictionary5.Count == 2 && dictionary5["state"].AsString() == "gravebuster.consume" && dictionary5["grave_stone_sync_id"].AsInt32() == 4321;
		Dictionary dictionary6 = gravebusterComponent.ExportComponentSave();
		bool flag6 = dictionary6.Count > 0 && !dictionary6.ContainsKey("_alive") && dictionary6 != dictionary5;
		gravebusterComponent.Detach(ComponentDetachReason.TemporaryTreeExit);
		bool flag7 = gravebusterComponent.Lifecycle == ComponentRuntimeLifecycle.Detached && gravebusterComponent.SyncSerialize().Count == 1;
		ComponentManager manager = new ComponentManager();
		GravebusterSyncProbeOwner gravebusterSyncProbeOwner = new GravebusterSyncProbeOwner
		{
			camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT,
			instance = new TowerDefenseCharacterInstance(),
			ProcessMode = ProcessModeEnum.Disabled
		};
		gravebusterSyncProbeOwner.instance.character = gravebusterSyncProbeOwner;
		AddChild(gravebusterSyncProbeOwner, forceReadableName: false, InternalMode.Disabled);
		gravebusterComponent.Bind(manager, gravebusterSyncProbeOwner, _definition);
		gravebusterComponent.Activate();
		gravebusterComponent.RegisterStateRuntime();
		bool flag8 = gravebusterComponent.IsAttached && !gravebusterComponent.IsReleased && gravebusterComponent.SyncSerialize().Count == 1;
		gravebusterComponent.Release();
		bool isReleased = gravebusterComponent.IsReleased;
		gravebusterSyncProbeOwner.Free();
		return num & flag & flag2 & flag4 & flag5 & flag6 & flag7 & flag8 & isReleased;
	}

	private void RestoreBenchmarkFixture()
	{
		GravebusterComponent gravebusterComponent = new GravebusterComponent();
		gravebusterComponent.Bind(_managers[0], _owners[0], _definition);
		gravebusterComponent.Activate();
		gravebusterComponent.RegisterStateRuntime();
		gravebusterComponent.graveStone = _graveStone;
		gravebusterComponent.SyncDeserialize(new Dictionary
		{
			["drop_velocity_x"] = 12.5f,
			["drop_velocity_y"] = -30f
		});
		_components[0] = gravebusterComponent;
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
			GravebusterComponent obj = _components[i];
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
		_graveStone?.Free();
		_graveStone = null;
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
			_definition = VariantUtils.ConvertTo<GravebusterComponentDefinition>(in value);
			return true;
		}
		if (name == PropertyName._graveStone)
		{
			_graveStone = VariantUtils.ConvertTo<TowerDefenseGravestone>(in value);
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
		if (name == PropertyName._graveStone)
		{
			value = VariantUtils.CreateFrom(in _graveStone);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._graveStone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
		info.AddProperty(PropertyName._graveStone, Variant.From(in _graveStone));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._definition, out var value))
		{
			_definition = value.As<GravebusterComponentDefinition>();
		}
		if (info.TryGetProperty(PropertyName._graveStone, out var value2))
		{
			_graveStone = value2.As<TowerDefenseGravestone>();
		}
	}
}
