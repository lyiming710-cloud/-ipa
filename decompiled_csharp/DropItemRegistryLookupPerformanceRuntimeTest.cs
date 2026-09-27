using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/DropItemRegistryLookupPerformanceRuntimeTest.cs")]
public sealed class DropItemRegistryLookupPerformanceRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";

		public static readonly StringName PrepareLookupRequests = "PrepareLookupRequests";

		public static readonly StringName ValidateLookupSemantics = "ValidateLookupSemantics";

		public static readonly StringName ExecuteLegacyBatch = "ExecuteLegacyBatch";

		public static readonly StringName ExecuteOptimizedBatch = "ExecuteOptimizedBatch";

		public static readonly StringName Lookup = "Lookup";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _lookupKinds = "_lookupKinds";

		public static readonly StringName _names = "_names";

		public static readonly StringName _coinObjectIds = "_coinObjectIds";

		public static readonly StringName _expected = "_expected";

		public static readonly StringName _sink = "_sink";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _functionalPassed = "_functionalPassed";

		public static readonly StringName _baselineEquivalent = "_baselineEquivalent";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSamples = 240;

	private const int MeasuredSamples = 1200;

	private readonly byte[] _lookupKinds = new byte[1000];

	private readonly StringName[] _names = new StringName[1000];

	private readonly ObjectManagerConfig.OBJECT[] _ids = new ObjectManagerConfig.OBJECT[1000];

	private readonly int[] _coinObjectIds = new int[1000];

	private readonly DropItemConfig[] _expected = new DropItemConfig[1000];

	private long _sink;

	private int _checks;

	private int _failures;

	private bool _functionalPassed;

	private bool _baselineEquivalent;

	private OptimizationBatchResult _baselineResult;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		Callable.From(Run).CallDeferred();
	}

	private void Run()
	{
		try
		{
			PrepareLookupRequests();
			ValidateLookupSemantics();
			_baselineResult = Measure(ExecuteLegacyBatch);
			OptimizationBatchResult result = Measure(ExecuteOptimizedBatch);
			bool flag = OptimizationPerformanceGate.IsBareResultPassed(in result, _functionalPassed, 1000, 240, 1000, 1000, CreateIdentity());
			GD.Print(FormatBaseline(in _baselineResult));
			GD.Print(OptimizationPerformanceGate.FormatBareResult(CreateIdentity(), in result, _functionalPassed, flag, 1000, 240, 1000, 1000));
			Check(flag, "Drop-item registry optimized lookups must pass the strict 1000-operation gate.");
		}
		catch (Exception ex)
		{
			_failures++;
			GD.PushError(ex.ToString());
		}
		finally
		{
			bool flag2 = _failures == 0;
			GD.Print($"DROP_ITEM_REGISTRY_LOOKUP_RESULT passed={flag2} checks={_checks} failures={_failures} functional={_functionalPassed} baselineEquivalent={_baselineEquivalent} sink={_sink}");
			GetTree().Quit((!flag2) ? 2 : 0);
		}
	}

	private void PrepareLookupRequests()
	{
		DropItemRegistry.Init();
		StringName stringName = new StringName("Sun");
		StringName stringName2 = new StringName("__drop_item_registry_lookup_missing__");
		DropItemConfig dropItem = DropItemRegistry.GetDropItem(stringName);
		DropItemConfig byId = DropItemRegistry.GetById(ObjectManagerConfig.OBJECT.COIN_GOLD);
		DropItemConfig byCoinObjectId = DropItemRegistry.GetByCoinObjectId(1);
		Check(dropItem != null && byId != null && byCoinObjectId != null, "Production drop-item registry fixtures must initialize.");
		for (int i = 0; i < 1000; i++)
		{
			byte b = (byte)(i % 3);
			bool flag = (i & 3) != 3;
			_lookupKinds[i] = b;
			switch (b)
			{
			case 0:
				_names[i] = (flag ? stringName : stringName2);
				_expected[i] = (flag ? dropItem : null);
				break;
			case 1:
				_ids[i] = (flag ? ObjectManagerConfig.OBJECT.COIN_GOLD : ObjectManagerConfig.OBJECT.NOONE);
				_expected[i] = (flag ? byId : null);
				break;
			default:
				_coinObjectIds[i] = (flag ? 1 : (-2147483648));
				_expected[i] = (flag ? byCoinObjectId : null);
				break;
			}
		}
	}

	private void ValidateLookupSemantics()
	{
		bool flag = true;
		bool flag2 = true;
		for (int i = 0; i < 1000; i++)
		{
			DropItemConfig dropItemConfig = Lookup(i, legacy: false);
			DropItemConfig dropItemConfig2 = Lookup(i, legacy: true);
			flag &= dropItemConfig == dropItemConfig2;
			flag2 &= dropItemConfig == _expected[i];
		}
		_baselineEquivalent = flag;
		_functionalPassed = flag & flag2;
		Check(flag, "TryGetValue lookups must preserve legacy hit and miss behavior.");
		Check(flag2, "All 1000 mixed production lookup requests must return the expected config identity.");
	}

	private OptimizationBatchResult Measure(Action dispatch)
	{
		OptimizationBatchSampler.PrepareForWarmup();
		for (int i = 0; i < 240; i++)
		{
			long startTicks = OptimizationBatchSampler.BeginSample();
			dispatch();
			OptimizationBatchSampler.EndWarmupSample(startTicks);
		}
		OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
		optimizationBatchSampler.BeginMeasurement();
		for (int j = 0; j < 1200; j++)
		{
			long startTicks2 = OptimizationBatchSampler.BeginSample();
			dispatch();
			optimizationBatchSampler.EndSample(startTicks2);
		}
		return optimizationBatchSampler.Complete();
	}

	private void ExecuteLegacyBatch()
	{
		long num = _sink;
		for (int i = 0; i < 1000; i++)
		{
			if (Lookup(i, legacy: true) != null)
			{
				num++;
			}
		}
		_sink = num;
	}

	private void ExecuteOptimizedBatch()
	{
		long num = _sink;
		for (int i = 0; i < 1000; i++)
		{
			if (Lookup(i, legacy: false) != null)
			{
				num++;
			}
		}
		_sink = num;
	}

	private DropItemConfig Lookup(int index, bool legacy)
	{
		return _lookupKinds[index] switch
		{
			0 => legacy ? DropItemRegistry.GetDropItemLegacyForTests(_names[index]) : DropItemRegistry.GetDropItem(_names[index]), 
			1 => legacy ? DropItemRegistry.GetByIdLegacyForTests(_ids[index]) : DropItemRegistry.GetById(_ids[index]), 
			_ => legacy ? DropItemRegistry.GetByCoinObjectIdLegacyForTests(_coinObjectIds[index]) : DropItemRegistry.GetByCoinObjectId(_coinObjectIds[index]), 
		};
	}

	private static OptimizationResultIdentity CreateIdentity()
	{
		return new OptimizationResultIdentity("drop-item-registry-single-probe", OptimizationWorkloadKind.BareFunction, "drop-item-config", "res://Test/DropItemRegistryLookupPerformanceRuntimeTest.tscn", "DropItemRegistry", "builtin-drop-item-configs", "mixed-name-id-coin-id", "none", "none", "steady-hit-and-miss", OptimizationScheduleKind.BackToBack, "headless-mobile", 60, 240);
	}

	private static string FormatBaseline(in OptimizationBatchResult result)
	{
		CultureInfo invariantCulture = CultureInfo.InvariantCulture;
		return "DROP_ITEM_REGISTRY_LEGACY_BASELINE " + $"instances={1000} activeInstances={1000} " + $"dispatchedInstances={1000} " + $"warmupSamples={240} " + $"samples={result.SampleCount} " + "batchMeanMs=" + result.MeanMilliseconds.ToString("F6", invariantCulture) + " batchP50Ms=" + result.P50Milliseconds.ToString("F6", invariantCulture) + " batchP95Ms=" + result.P95Milliseconds.ToString("F6", invariantCulture) + " batchP99Ms=" + result.P99Milliseconds.ToString("F6", invariantCulture) + " batchMaxMs=" + result.MaximumMilliseconds.ToString("F6", invariantCulture) + " " + $"overBudgetSamples={result.OverBudgetSamples} " + $"maxSpikeSample={result.MaximumSampleIndex + 1} " + $"allocatedBytes={result.AllocatedBytes} " + $"gen0={result.Gen0Collections} " + $"gen1={result.Gen1Collections} " + $"gen2={result.Gen2Collections} accepted=False";
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError(message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareLookupRequests, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ValidateLookupSemantics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteLegacyBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteOptimizedBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Lookup, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "legacy", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName.PrepareLookupRequests && args.Count == 0)
		{
			PrepareLookupRequests();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateLookupSemantics && args.Count == 0)
		{
			ValidateLookupSemantics();
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteLegacyBatch && args.Count == 0)
		{
			ExecuteLegacyBatch();
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteOptimizedBatch && args.Count == 0)
		{
			ExecuteOptimizedBatch();
			ret = default;
			return true;
		}
		if (method == MethodName.Lookup && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(Lookup(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.PrepareLookupRequests)
		{
			return true;
		}
		if (method == MethodName.ValidateLookupSemantics)
		{
			return true;
		}
		if (method == MethodName.ExecuteLegacyBatch)
		{
			return true;
		}
		if (method == MethodName.ExecuteOptimizedBatch)
		{
			return true;
		}
		if (method == MethodName.Lookup)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._sink)
		{
			_sink = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._functionalPassed)
		{
			_functionalPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._baselineEquivalent)
		{
			_baselineEquivalent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._lookupKinds)
		{
			value = VariantUtils.CreateFrom(in _lookupKinds);
			return true;
		}
		if (name == PropertyName._names)
		{
			value = VariantUtils.CreateFrom(in _names);
			return true;
		}
		if (name == PropertyName._coinObjectIds)
		{
			value = VariantUtils.CreateFrom(in _coinObjectIds);
			return true;
		}
		if (name == PropertyName._expected)
		{
			GodotObject[] expected = _expected;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(expected);
			return true;
		}
		if (name == PropertyName._sink)
		{
			value = VariantUtils.CreateFrom(in _sink);
			return true;
		}
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		if (name == PropertyName._functionalPassed)
		{
			value = VariantUtils.CreateFrom(in _functionalPassed);
			return true;
		}
		if (name == PropertyName._baselineEquivalent)
		{
			value = VariantUtils.CreateFrom(in _baselineEquivalent);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.PackedByteArray, PropertyName._lookupKinds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._names, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._coinObjectIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._expected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._sink, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._functionalPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._baselineEquivalent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._sink, Variant.From(in _sink));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._functionalPassed, Variant.From(in _functionalPassed));
		info.AddProperty(PropertyName._baselineEquivalent, Variant.From(in _baselineEquivalent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._sink, out var value))
		{
			_sink = value.As<long>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value2))
		{
			_checks = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value3))
		{
			_failures = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._functionalPassed, out var value4))
		{
			_functionalPassed = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._baselineEquivalent, out var value5))
		{
			_baselineEquivalent = value5.As<bool>();
		}
	}
}
