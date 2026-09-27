using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/OptimizationPerformanceGateRuntimeTest.cs")]
public sealed class OptimizationPerformanceGateRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";

		public static readonly StringName ExecuteBatch = "ExecuteBatch";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _fixture = "_fixture";

		public static readonly StringName _sink = "_sink";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int MeasuredSamples = 1200;

	private readonly int[] _fixture = new int[1000];

	private long _sink;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		Callable.From(Run).CallDeferred();
	}

	private void Run()
	{
		OptimizationResultIdentity identity = new OptimizationResultIdentity("measurement-contract", OptimizationWorkloadKind.HeadlessContract, "none", "res://Test/OptimizationPerformanceGateRuntimeTest.tscn", "none", "none", "none", "none", "noop-batch", "steady", OptimizationScheduleKind.BackToBack, "headless", 60, 240);
		OptimizationBatchSampler.PrepareForWarmup();
		long num = 0L;
		for (int i = 0; i < 240; i++)
		{
			long startTicks = OptimizationBatchSampler.BeginSample();
			ExecuteBatch();
			num += OptimizationBatchSampler.EndWarmupSample(startTicks);
		}
		long sink = _sink;
		OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
		optimizationBatchSampler.BeginMeasurement();
		for (int j = 0; j < 600; j++)
		{
			long startTicks2 = OptimizationBatchSampler.BeginSample();
			ExecuteBatch();
			optimizationBatchSampler.EndSample(startTicks2);
		}
		for (int k = 600; k < 1200; k++)
		{
			long startTicks3 = OptimizationBatchSampler.BeginSample();
			ExecuteBatch();
			optimizationBatchSampler.EndSample(startTicks3);
		}
		OptimizationBatchResult result = optimizationBatchSampler.Complete();
		long num2 = 499500L;
		bool functionalPassed = _sink - sink == num2 * 1200 && result.SampleCount == 1200 && num >= 0 && VerifyRejectedGateContracts(in identity);
		bool flag = OptimizationPerformanceGate.IsBareResultPassed(in result, functionalPassed, 1000, 240, 1000, 1000, in identity);
		GD.Print(OptimizationPerformanceGate.FormatBareResult(in identity, in result, functionalPassed, flag, 1000, 240, 1000, 1000));
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void ExecuteBatch()
	{
		long num = 0L;
		for (int i = 0; i < _fixture.Length; i++)
		{
			num += _fixture[i] + i;
		}
		_sink += num;
	}

	private static bool VerifyRejectedGateContracts(in OptimizationResultIdentity identity)
	{
		OptimizationBatchResult result = CreateSyntheticResult(0.199, 0L, 600);
		OptimizationBatchResult result2 = CreateSyntheticResult(0.2, 0L, 600);
		OptimizationBatchResult result3 = CreateSyntheticResult(0.1, 1L, 600);
		OptimizationBatchResult result4 = CreateSyntheticResult(0.1, 0L, 599);
		if (OptimizationPerformanceGate.IsBareResultPassed(in result, functionalPassed: true, 1000, 240, 1000, 1000, in identity) && !OptimizationPerformanceGate.IsBareResultPassed(in result, functionalPassed: true, 999, 240, 1000, 1000, in identity) && !OptimizationPerformanceGate.IsBareResultPassed(in result, functionalPassed: true, 1000, 240, 999, 1000, in identity) && !OptimizationPerformanceGate.IsBareResultPassed(in result, functionalPassed: true, 1000, 240, 1000, 999, in identity) && !OptimizationPerformanceGate.IsBareResultPassed(in result, functionalPassed: true, 1000, 239, 1000, 1000, in identity) && !OptimizationPerformanceGate.IsBareResultPassed(in result, functionalPassed: false, 1000, 240, 1000, 1000, in identity) && !OptimizationPerformanceGate.IsBareResultPassed(in result2, functionalPassed: true, 1000, 240, 1000, 1000, in identity) && !OptimizationPerformanceGate.IsBareResultPassed(in result3, functionalPassed: true, 1000, 240, 1000, 1000, in identity))
		{
			return !OptimizationPerformanceGate.IsBareResultPassed(in result4, functionalPassed: true, 1000, 240, 1000, 1000, in identity);
		}
		return false;
	}

	private static OptimizationBatchResult CreateSyntheticResult(double p99Milliseconds, long allocatedBytes, int sampleCount)
	{
		return new OptimizationBatchResult(0.1, 0.1, 0.1, p99Milliseconds, p99Milliseconds, allocatedBytes, sampleCount, (p99Milliseconds >= 0.2) ? 1 : 0, 0, 0, 0, 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ExecuteBatch && args.Count == 0)
		{
			ExecuteBatch();
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
		if (method == MethodName.ExecuteBatch)
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
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._fixture)
		{
			value = VariantUtils.CreateFrom(in _fixture);
			return true;
		}
		if (name == PropertyName._sink)
		{
			value = VariantUtils.CreateFrom(in _sink);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._fixture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._sink, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._sink, Variant.From(in _sink));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._sink, out var value))
		{
			_sink = value.As<long>();
		}
	}
}
