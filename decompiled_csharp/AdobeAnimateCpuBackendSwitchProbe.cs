using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateCpuBackendSwitchProbe.cs")]
public class AdobeAnimateCpuBackendSwitchProbe : Node
{
	private readonly record struct AnimationStateSnapshot(string Clip, int FrameIndex, double ElapsedTimer)
	{
		public double Clock => (double)FrameIndex + ElapsedTimer;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CurrentClock = "CurrentClock";

		public static readonly StringName OnCpuAnimationEvent = "OnCpuAnimationEvent";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _root = "_root";

		public static readonly StringName _child = "_child";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _cpuEventCount = "_cpuEventCount";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "ADOBE_ANIMATE_CPU_BACKEND_SWITCH_RESULT";

	private const string AnimationScenePath = "res://Asset/Anime/Character/Zombie/Challenge/JacksonX/ZombieJacksonX.tscn";

	private readonly List<string> _failures = new List<string>();

	private AdobeAnimateRenderBackend _originalBackend;

	private AdobeAnimateSprite _root;

	private AdobeAnimateSprite _child;

	private int _checks;

	private int _cpuEventCount;

	public override async void _Ready()
	{
		try
		{
			if (Global.Instance == null)
			{
				throw new InvalidOperationException("Global autoload is unavailable.");
			}
			_originalBackend = Global.Instance.adobeAnimateRenderBackend;
			await RunProbe();
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		finally
		{
			await Cleanup();
		}
		bool flag = _failures.Count == 0 && _checks == 10;
		GD.Print($"{"ADOBE_ANIMATE_CPU_BACKEND_SWITCH_RESULT"} passed={flag} checks={_checks} failures={_failures.Count}");
		foreach (string failure in _failures)
		{
			GD.PrintErr("ADOBE_ANIMATE_CPU_BACKEND_SWITCH_RESULT failure=" + failure);
		}
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task RunProbe()
	{
		PackedScene packedScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Challenge/JacksonX/ZombieJacksonX.tscn");
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			throw new InvalidOperationException("JacksonX animation scene must load.");
		}
		_root = packedScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		_root.Name = "BackendSwitchRoot";
		_root.Position = new Vector2(320f, 240f);
		AddChild(_root, forceReadableName: false, InternalMode.Disabled);
		_child = _root.GetNodeOrNull<AdobeAnimateSprite>("ZombieDuckytube");
		if (!GodotObject.IsInstanceValid(_child))
		{
			throw new InvalidOperationException("JacksonX nested child animation must load.");
		}
		_root.SetAnimation("Walk");
		_root.OnAnimeEvent += OnCpuAnimationEvent;
		Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
		await WaitProcessFrames(5);
		AdobeAnimateCrowdAggregateStats aggregateRenderStats = AdobeAnimateRenderManager.GetAggregateRenderStats();
		Check(Global.Instance.adobeAnimateRenderBackend == AdobeAnimateRenderBackend.GpuCrowd && aggregateRenderStats.CpuRoots == 0 && aggregateRenderStats.CrowdRoots + aggregateRenderStats.FallbackRoots > 0, "GPU Crowd must publish visible output without CPU Pose roots.");
		AnimationStateSnapshot beforeCpu = CaptureState();
		ProcessModeEnum processMode = _root.ProcessMode;
		_root.ProcessMode = ProcessModeEnum.Disabled;
		Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.CpuPose;
		await WaitProcessFrames(4);
		AdobeAnimateCrowdAggregateStats aggregateRenderStats2 = AdobeAnimateRenderManager.GetAggregateRenderStats();
		Check(Global.Instance.adobeAnimateRenderBackend == AdobeAnimateRenderBackend.CpuPose && aggregateRenderStats2.CpuRoots == 1 && aggregateRenderStats2.CrowdRoots == aggregateRenderStats2.CpuRoots && aggregateRenderStats2.FallbackRoots == 0, "CPU Pose must publish captured CPU state through the shared render graph.");
		Check(aggregateRenderStats2.FallbackRuns == 0 && aggregateRenderStats2.CpuFallbackRoots == 0 && aggregateRenderStats2.CpuValidationFailures == 0, "CPU Pose must avoid full-pose fallback while the switch boundary is frozen.");
		Check(CaptureState() == beforeCpu, "GPU-to-CPU switching must preserve the exact animation state.");
		Vector2 childBeforeCpuPlayback = _child.Position;
		_root.ProcessMode = processMode;
		await WaitProcessSeconds(0.15);
		Check(CurrentClock() > beforeCpu.Clock && _root.UsesRuntimeGpuClockInterpolation, "CPU Pose playback must continue with visual interpolation after the switch boundary.");
		Check(_child.Position.DistanceSquaredTo(childBeforeCpuPlayback) <= 0.0001f, "CPU Pose must leave visual-only nested attachment transforms on the shared graph.");
		_cpuEventCount = 0;
		_root.SetAnimation("PointUp");
		await WaitProcessSeconds(0.85);
		Check(_cpuEventCount > 0, "CPU Pose must keep authored animation events on the CPU while visual interpolation is retained.");
		AnimationStateSnapshot beforeGpu = CaptureState();
		_root.ProcessMode = ProcessModeEnum.Disabled;
		Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
		await WaitProcessFrames(4);
		AdobeAnimateCrowdAggregateStats aggregateRenderStats3 = AdobeAnimateRenderManager.GetAggregateRenderStats();
		Check(aggregateRenderStats3.CpuRoots == 0 && aggregateRenderStats3.CrowdRoots + aggregateRenderStats3.FallbackRoots > 0 && CaptureState() == beforeGpu, "CPU-to-GPU switching must retire CPU Pose output without advancing animation state.");
		_root.ProcessMode = processMode;
		await WaitProcessSeconds(0.15);
		Check(CaptureState() != beforeGpu, "GPU Crowd playback must continue after returning from CPU Pose.");
		Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
		Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.CpuPose;
		await WaitProcessFrames(4);
		AdobeAnimateCrowdAggregateStats aggregateRenderStats4 = AdobeAnimateRenderManager.GetAggregateRenderStats();
		Check(aggregateRenderStats4.CpuRoots == 1 && aggregateRenderStats4.CrowdRoots == aggregateRenderStats4.CpuRoots && aggregateRenderStats4.FallbackRoots == 0, "A rapid switch must apply the final CPU Pose value.");
	}

	private async Task Cleanup()
	{
		if (Global.Instance != null)
		{
			Global.Instance.adobeAnimateRenderBackend = _originalBackend;
		}
		if (GodotObject.IsInstanceValid(_root))
		{
			_root.OnAnimeEvent -= OnCpuAnimationEvent;
		}
		if (GodotObject.IsInstanceValid(_root) && !_root.IsQueuedForDeletion())
		{
			_root.QueueFree();
		}
		await WaitProcessFrames(2);
	}

	private AnimationStateSnapshot CaptureState()
	{
		return new AnimationStateSnapshot(_root.clip, _root.frameIndex, _root.elapsedTimer);
	}

	private double CurrentClock()
	{
		return (double)_root.frameIndex + _root.elapsedTimer;
	}

	private void OnCpuAnimationEvent(string command, Variant argument)
	{
		if (command == "spawn")
		{
			_cpuEventCount++;
		}
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task WaitProcessSeconds(double seconds)
	{
		await ToSignal(GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures.Add(message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CurrentClock, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCpuAnimationEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
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
		if (method == MethodName.CurrentClock && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(CurrentClock());
			return true;
		}
		if (method == MethodName.OnCpuAnimationEvent && args.Count == 2)
		{
			OnCpuAnimationEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
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
		if (method == MethodName.CurrentClock)
		{
			return true;
		}
		if (method == MethodName.OnCpuAnimationEvent)
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
		if (name == PropertyName._originalBackend)
		{
			_originalBackend = VariantUtils.ConvertTo<AdobeAnimateRenderBackend>(in value);
			return true;
		}
		if (name == PropertyName._root)
		{
			_root = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._child)
		{
			_child = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cpuEventCount)
		{
			_cpuEventCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._originalBackend)
		{
			value = VariantUtils.CreateFrom(in _originalBackend);
			return true;
		}
		if (name == PropertyName._root)
		{
			value = VariantUtils.CreateFrom(in _root);
			return true;
		}
		if (name == PropertyName._child)
		{
			value = VariantUtils.CreateFrom(in _child);
			return true;
		}
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._cpuEventCount)
		{
			value = VariantUtils.CreateFrom(in _cpuEventCount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._originalBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._root, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._child, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cpuEventCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._originalBackend, Variant.From(in _originalBackend));
		info.AddProperty(PropertyName._root, Variant.From(in _root));
		info.AddProperty(PropertyName._child, Variant.From(in _child));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._cpuEventCount, Variant.From(in _cpuEventCount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._originalBackend, out var value))
		{
			_originalBackend = value.As<AdobeAnimateRenderBackend>();
		}
		if (info.TryGetProperty(PropertyName._root, out var value2))
		{
			_root = value2.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._child, out var value3))
		{
			_child = value3.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value4))
		{
			_checks = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cpuEventCount, out var value5))
		{
			_cpuEventCount = value5.As<int>();
		}
	}
}
