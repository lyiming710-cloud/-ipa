using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BuffComponentIdleHotPathRuntimeTest.cs")]
public class BuffComponentIdleHotPathRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";

		public static readonly StringName CreateWorkload = "CreateWorkload";

		public static readonly StringName RunFunctionalContract = "RunFunctionalContract";

		public static readonly StringName Warmup = "Warmup";

		public static readonly StringName CountTreeOwners = "CountTreeOwners";

		public static readonly StringName CountActiveInstances = "CountActiveInstances";

		public static readonly StringName ReleaseWorkload = "ReleaseWorkload";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _owners = "_owners";

		public static readonly StringName _sprites = "_sprites";

		public static readonly StringName _managers = "_managers";

		public static readonly StringName _definition = "_definition";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSamples = 240;

	private const int MeasuredSamples = 1200;

	private const double Delta = 1.0 / 60.0;

	private readonly BuffComponent[] _components = new BuffComponent[1000];

	private readonly BuffIdleHotPathProbeOwner[] _owners = new BuffIdleHotPathProbeOwner[1000];

	private readonly AdobeAnimateSprite[] _sprites = new AdobeAnimateSprite[1000];

	private readonly ComponentManager[] _managers = new ComponentManager[1000];

	private BuffComponentDefinition _definition;

	public override void _Ready()
	{
		Run();
	}

	private void Run()
	{
		try
		{
			CreateWorkload();
			bool flag = RunFunctionalContract();
			Warmup();
			OptimizationBatchResult optimizationBatchResult = BenchmarkBaseline();
			OptimizationBatchResult optimizationBatchResult2 = BenchmarkCandidate();
			int num = CountTreeOwners();
			int num2 = CountActiveInstances();
			bool flag2 = optimizationBatchResult2.P99Milliseconds < 0.2 && optimizationBatchResult2.MeanMilliseconds < optimizationBatchResult.MeanMilliseconds && optimizationBatchResult2.P50Milliseconds < optimizationBatchResult.P50Milliseconds && optimizationBatchResult2.AllocatedBytes == 0L && optimizationBatchResult2.Gen0Collections == 0 && optimizationBatchResult2.Gen1Collections == 0 && optimizationBatchResult2.Gen2Collections == 0;
			bool flag3 = (flag & flag2) && num == 1000 && num2 == 1000;
			double value = ((optimizationBatchResult.MeanMilliseconds <= 0.0) ? 0.0 : ((optimizationBatchResult.MeanMilliseconds - optimizationBatchResult2.MeanMilliseconds) / optimizationBatchResult.MeanMilliseconds * 100.0));
			GD.Print($"BUFF_COMPONENT_IDLE_HOT_PATH_RESULT passed={flag3} functionalPassed={flag} performancePassed={flag2} instances={1000} treeOwners={num} activeInstances={num2} warmupSamples={240} measuredSamples={1200} baselineMeanMs={optimizationBatchResult.MeanMilliseconds:F6} baselineP50Ms={optimizationBatchResult.P50Milliseconds:F6} baselineP99Ms={optimizationBatchResult.P99Milliseconds:F6} candidateMeanMs={optimizationBatchResult2.MeanMilliseconds:F6} candidateP50Ms={optimizationBatchResult2.P50Milliseconds:F6} candidateP99Ms={optimizationBatchResult2.P99Milliseconds:F6} meanReductionPercent={value:F2} allocatedBytes={optimizationBatchResult2.AllocatedBytes} gen0={optimizationBatchResult2.Gen0Collections} gen1={optimizationBatchResult2.Gen1Collections} gen2={optimizationBatchResult2.Gen2Collections}");
			GetTree().Quit((!flag3) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PrintErr("BUFF_COMPONENT_IDLE_HOT_PATH_EXCEPTION " + ex);
			GetTree().Quit(2);
		}
		finally
		{
			ReleaseWorkload();
		}
	}

	private void CreateWorkload()
	{
		_definition = new BuffComponentDefinition
		{
			ComponentTypeId = "BuffComponent",
			DefinitionId = "buff.idle.hotpath.runtime",
			InstanceId = "buff.idle.hotpath.runtime",
			WireIndex = 0
		};
		for (int i = 0; i < 1000; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = new AdobeAnimateSprite
			{
				ProcessMode = ProcessModeEnum.Disabled
			};
			BuffIdleHotPathProbeOwner buffIdleHotPathProbeOwner = new BuffIdleHotPathProbeOwner
			{
				instance = new TowerDefenseCharacterInstance(),
				sprite = adobeAnimateSprite,
				ProcessMode = ProcessModeEnum.Disabled
			};
			buffIdleHotPathProbeOwner.instance.character = buffIdleHotPathProbeOwner;
			AddChild(buffIdleHotPathProbeOwner, forceReadableName: false, InternalMode.Disabled);
			ComponentManager componentManager = new ComponentManager();
			BuffComponent buffComponent = new BuffComponent();
			buffComponent.Bind(componentManager, buffIdleHotPathProbeOwner, _definition);
			buffComponent.Activate();
			_sprites[i] = adobeAnimateSprite;
			_owners[i] = buffIdleHotPathProbeOwner;
			_managers[i] = componentManager;
			_components[i] = buffComponent;
		}
	}

	private bool RunFunctionalContract()
	{
		BuffComponent buffComponent = _components[0];
		AdobeAnimateSprite adobeAnimateSprite = _sprites[0];
		adobeAnimateSprite.meshColor = Colors.Red;
		RunCandidateFrame(buffComponent, adobeAnimateSprite);
		bool flag = adobeAnimateSprite.meshColor == Colors.White;
		buffComponent.buffDictionary["runtime-probe"] = new TowerDefenseCharacterBuffConfig
		{
			key = "runtime-probe",
			character = _owners[0]
		};
		bool hasFrameUpdateWork = buffComponent.HasFrameUpdateWork;
		buffComponent.SetAlive(alive: false);
		bool flag2 = !buffComponent.HasFrameUpdateWork;
		buffComponent.SetAlive(alive: true);
		buffComponent.buffDictionary.Clear();
		if (flag & hasFrameUpdateWork & flag2)
		{
			return !buffComponent.HasFrameUpdateWork;
		}
		return false;
	}

	private void Warmup()
	{
		for (int i = 0; i < 240; i++)
		{
			for (int j = 0; j < 1000; j++)
			{
				_components[j].BuffUpdate(1.0 / 60.0);
			}
			for (int k = 0; k < 1000; k++)
			{
				RunCandidateFrame(_components[k], _sprites[k]);
			}
		}
		OptimizationBatchSampler.PrepareForWarmup();
	}

	private OptimizationBatchResult BenchmarkBaseline()
	{
		OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
		optimizationBatchSampler.BeginMeasurement();
		for (int i = 0; i < 1200; i++)
		{
			long startTicks = OptimizationBatchSampler.BeginSample();
			for (int j = 0; j < 1000; j++)
			{
				_components[j].BuffUpdate(1.0 / 60.0);
			}
			optimizationBatchSampler.EndSample(startTicks);
		}
		return optimizationBatchSampler.Complete();
	}

	private OptimizationBatchResult BenchmarkCandidate()
	{
		OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
		optimizationBatchSampler.BeginMeasurement();
		for (int i = 0; i < 1200; i++)
		{
			long startTicks = OptimizationBatchSampler.BeginSample();
			for (int j = 0; j < 1000; j++)
			{
				RunCandidateFrame(_components[j], _sprites[j]);
			}
			optimizationBatchSampler.EndSample(startTicks);
		}
		return optimizationBatchSampler.Complete();
	}

	private static void RunCandidateFrame(BuffComponent component, AdobeAnimateSprite sprite)
	{
		if (component != null && component.HasFrameUpdateWork)
		{
			component.BuffUpdate(1.0 / 60.0);
		}
		else if (sprite.meshColor != Colors.White)
		{
			sprite.meshColor = Colors.White;
		}
	}

	private int CountTreeOwners()
	{
		int num = 0;
		for (int i = 0; i < 1000; i++)
		{
			BuffIdleHotPathProbeOwner obj = _owners[i];
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
			BuffComponent obj = _components[i];
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
			_sprites[i]?.Free();
			_owners[i]?.Free();
			_components[i] = null;
			_owners[i] = null;
			_sprites[i] = null;
			_managers[i] = null;
		}
		_definition?.Dispose();
		_definition = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateWorkload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunFunctionalContract, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Warmup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.Warmup && args.Count == 0)
		{
			Warmup();
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
		if (method == MethodName.Warmup)
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
			_definition = VariantUtils.ConvertTo<BuffComponentDefinition>(in value);
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
		if (name == PropertyName._sprites)
		{
			GodotObject[] owners = _sprites;
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
			new PropertyInfo(Variant.Type.Array, PropertyName._sprites, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
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
			_definition = value.As<BuffComponentDefinition>();
		}
	}
}
