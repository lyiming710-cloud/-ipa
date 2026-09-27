using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateGpuOwnerValidityPerformanceRuntimeTest.cs")]
public sealed class AdobeAnimateGpuOwnerValidityPerformanceRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";

		public static readonly StringName ExecuteBatch = "ExecuteBatch";

		public static readonly StringName VerifyValidOwnerLifecycle = "VerifyValidOwnerLifecycle";

		public static readonly StringName VerifyNullOwnerLifecycle = "VerifyNullOwnerLifecycle";

		public static readonly StringName VerifyFreedOwnerLifecycle = "VerifyFreedOwnerLifecycle";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _owners = "_owners";

		public static readonly StringName _legacyOuterValidity = "_legacyOuterValidity";

		public static readonly StringName _rejectedSink = "_rejectedSink";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSampleCount = 240;

	private const int SampleCount = 1200;

	private readonly AdobeAnimateSprite[] _owners = new AdobeAnimateSprite[1000];

	private bool _legacyOuterValidity;

	private long _rejectedSink;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		Callable.From(Run).CallDeferred();
	}

	private void Run()
	{
		try
		{
			_legacyOuterValidity = Array.Exists(OS.GetCmdlineUserArgs(), (string argument) => argument == "--legacy-outer-validity");
			for (int num = 0; num < _owners.Length; num++)
			{
				_owners[num] = new AdobeAnimateSprite();
			}
			OptimizationBatchSampler.PrepareForWarmup();
			long num2 = 0L;
			for (int num3 = 0; num3 < 240; num3++)
			{
				long startTicks = OptimizationBatchSampler.BeginSample();
				ExecuteBatch();
				num2 += OptimizationBatchSampler.EndWarmupSample(startTicks);
			}
			long rejectedSink = _rejectedSink;
			OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
			optimizationBatchSampler.BeginMeasurement();
			for (int num4 = 0; num4 < 1200; num4++)
			{
				long startTicks2 = OptimizationBatchSampler.BeginSample();
				ExecuteBatch();
				optimizationBatchSampler.EndSample(startTicks2);
			}
			OptimizationBatchResult result = optimizationBatchSampler.Complete();
			long num5 = 1200000L;
			bool flag = VerifyValidOwnerLifecycle();
			bool flag2 = VerifyNullOwnerLifecycle();
			bool flag3 = VerifyFreedOwnerLifecycle();
			bool functionalPassed = (_rejectedSink - rejectedSink == num5 && result.SampleCount == 1200 && num2 >= 0) & flag & flag2 & flag3;
			OptimizationResultIdentity identity = new OptimizationResultIdentity("adobe_animate_gpu_owner_validity_fast_path", OptimizationWorkloadKind.BareFunction, "adobe_animate_gpu_owner", "res://Test/AdobeAnimateGpuOwnerValidityPerformanceRuntimeTest.tscn", "AdobeAnimateSprite", "none", "none", "none", "owner_definition_validation", "render_prepare", OptimizationScheduleKind.BackToBack, "headless", Math.Max(1, Engine.PhysicsTicksPerSecond), 240);
			bool flag4 = OptimizationPerformanceGate.IsBareResultPassed(in result, functionalPassed, 1000, 240, 1000, 1000, in identity);
			GD.Print(OptimizationPerformanceGate.FormatBareResult(in identity, in result, functionalPassed, flag4, 1000, 240, 1000, 1000));
			GD.Print("ADOBE_ANIMATE_GPU_OWNER_VALIDITY_SEMANTICS " + $"legacyOuterValidity={_legacyOuterValidity} " + $"validOwnerLifecycle={flag} " + $"nullOwnerLifecycle={flag2} " + $"freedOwnerLifecycle={flag3}");
			GetTree().Quit((!flag4) ? 2 : 0);
		}
		catch (Exception value)
		{
			GD.PrintErr("ADOBE_ANIMATE_GPU_OWNER_VALIDITY_SEMANTICS " + $"passed=False exception={value}");
			GetTree().Quit(2);
		}
		finally
		{
			for (int num6 = 0; num6 < _owners.Length; num6++)
			{
				AdobeAnimateSprite adobeAnimateSprite = _owners[num6];
				if (GodotObject.IsInstanceValid(adobeAnimateSprite))
				{
					adobeAnimateSprite.Free();
				}
				_owners[num6] = null;
			}
		}
	}

	private void ExecuteBatch()
	{
		int num = 0;
		AdobeAnimateRuntimeDefinition definition;
		if (_legacyOuterValidity)
		{
			for (int i = 0; i < _owners.Length; i++)
			{
				AdobeAnimateSprite adobeAnimateSprite = _owners[i];
				if (!GodotObject.IsInstanceValid(adobeAnimateSprite) || !adobeAnimateSprite.TryGetGpuGraphOwnerDefinitionForRender(out definition))
				{
					num++;
				}
			}
		}
		else
		{
			for (int j = 0; j < _owners.Length; j++)
			{
				AdobeAnimateSprite adobeAnimateSprite2 = _owners[j];
				if (adobeAnimateSprite2 == null || !adobeAnimateSprite2.TryGetGpuGraphOwnerDefinitionForRender(out definition))
				{
					num++;
				}
			}
		}
		_rejectedSink += num;
	}

	private bool VerifyValidOwnerLifecycle()
	{
		AdobeAnimateSprite adobeAnimateSprite = _owners[0];
		AdobeAnimateRuntimeDefinition definition;
		if (GodotObject.IsInstanceValid(adobeAnimateSprite) && adobeAnimateSprite != null)
		{
			return !adobeAnimateSprite.TryGetGpuGraphOwnerDefinitionForRender(out definition);
		}
		return false;
	}

	private static bool VerifyNullOwnerLifecycle()
	{
		return null == null;
	}

	private static bool VerifyFreedOwnerLifecycle()
	{
		AdobeAnimateSprite adobeAnimateSprite = new AdobeAnimateSprite();
		adobeAnimateSprite.Free();
		AdobeAnimateRuntimeDefinition definition;
		if (adobeAnimateSprite != null && !GodotObject.IsInstanceValid(adobeAnimateSprite))
		{
			return !adobeAnimateSprite.TryGetGpuGraphOwnerDefinitionForRender(out definition);
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyValidOwnerLifecycle, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyNullOwnerLifecycle, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.VerifyFreedOwnerLifecycle, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
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
		if (method == MethodName.VerifyValidOwnerLifecycle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyValidOwnerLifecycle());
			return true;
		}
		if (method == MethodName.VerifyNullOwnerLifecycle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyNullOwnerLifecycle());
			return true;
		}
		if (method == MethodName.VerifyFreedOwnerLifecycle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyFreedOwnerLifecycle());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.VerifyNullOwnerLifecycle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyNullOwnerLifecycle());
			return true;
		}
		if (method == MethodName.VerifyFreedOwnerLifecycle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyFreedOwnerLifecycle());
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
		if (method == MethodName.ExecuteBatch)
		{
			return true;
		}
		if (method == MethodName.VerifyValidOwnerLifecycle)
		{
			return true;
		}
		if (method == MethodName.VerifyNullOwnerLifecycle)
		{
			return true;
		}
		if (method == MethodName.VerifyFreedOwnerLifecycle)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._legacyOuterValidity)
		{
			_legacyOuterValidity = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rejectedSink)
		{
			_rejectedSink = VariantUtils.ConvertTo<long>(in value);
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
		if (name == PropertyName._legacyOuterValidity)
		{
			value = VariantUtils.CreateFrom(in _legacyOuterValidity);
			return true;
		}
		if (name == PropertyName._rejectedSink)
		{
			value = VariantUtils.CreateFrom(in _rejectedSink);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName._legacyOuterValidity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._rejectedSink, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._legacyOuterValidity, Variant.From(in _legacyOuterValidity));
		info.AddProperty(PropertyName._rejectedSink, Variant.From(in _rejectedSink));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._legacyOuterValidity, out var value))
		{
			_legacyOuterValidity = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rejectedSink, out var value2))
		{
			_rejectedSink = value2.As<long>();
		}
	}
}
