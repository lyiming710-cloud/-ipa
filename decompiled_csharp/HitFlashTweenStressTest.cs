using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/HitFlashTweenStressTest.cs")]
public class HitFlashTweenStressTest : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName Setup = "Setup";

		public static readonly StringName BeginMeasurement = "BeginMeasurement";

		public static readonly StringName TryTriggerFlash = "TryTriggerFlash";

		public static readonly StringName Finish = "Finish";

		public static readonly StringName ObserveFlashState = "ObserveFlashState";

		public static readonly StringName CalculateOnePercentLowFps = "CalculateOnePercentLowFps";

		public static readonly StringName Percentile = "Percentile";

		public static readonly StringName GetAllocatedBytesPerRequest = "GetAllocatedBytesPerRequest";

		public static readonly StringName GetAverageFlashBurstMilliseconds = "GetAverageFlashBurstMilliseconds";

		public static readonly StringName GetMilliseconds = "GetMilliseconds";

		public static readonly StringName GetElapsedSeconds = "GetElapsedSeconds";

		public static readonly StringName FailSetup = "FailSetup";

		public static readonly StringName ApplyCommandLineArguments = "ApplyCommandLineArguments";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName TargetCount = "TargetCount";

		public static readonly StringName WarmupSeconds = "WarmupSeconds";

		public static readonly StringName MeasureSeconds = "MeasureSeconds";

		public static readonly StringName FlashIntervalSeconds = "FlashIntervalSeconds";

		public static readonly StringName FlashDurationSeconds = "FlashDurationSeconds";

		public static readonly StringName EnableFlash = "EnableFlash";

		public static readonly StringName VisibleTargets = "VisibleTargets";

		public static readonly StringName TargetScale = "TargetScale";

		public static readonly StringName GridColumns = "GridColumns";

		public static readonly StringName MaxFrameSamples = "MaxFrameSamples";

		public static readonly StringName BenchmarkMaxFps = "BenchmarkMaxFps";

		public static readonly StringName ProcessedTweenLimit = "ProcessedTweenLimit";

		public static readonly StringName RequireGpuFlashEnvelope = "RequireGpuFlashEnvelope";

		public static readonly StringName _runtimeProfile = "_runtimeProfile";

		public static readonly StringName _frameSeconds = "_frameSeconds";

		public static readonly StringName _setupComplete = "_setupComplete";

		public static readonly StringName _measuring = "_measuring";

		public static readonly StringName _phaseStartTimestamp = "_phaseStartTimestamp";

		public static readonly StringName _lastFrameTimestamp = "_lastFrameTimestamp";

		public static readonly StringName _nextFlashSeconds = "_nextFlashSeconds";

		public static readonly StringName _frameCount = "_frameCount";

		public static readonly StringName _flashBursts = "_flashBursts";

		public static readonly StringName _flashRequests = "_flashRequests";

		public static readonly StringName _maxFlashBurstTicks = "_maxFlashBurstTicks";

		public static readonly StringName _totalFlashBurstTicks = "_totalFlashBurstTicks";

		public static readonly StringName _maxProcessedTweens = "_maxProcessedTweens";

		public static readonly StringName _maxBatchedFlashComponents = "_maxBatchedFlashComponents";

		public static readonly StringName _maxGpuFlashComponents = "_maxGpuFlashComponents";

		public static readonly StringName _physicsProcessingTargets = "_physicsProcessingTargets";

		public static readonly StringName _observedBrightStrength = "_observedBrightStrength";

		public static readonly StringName _observedCompletedFlashReset = "_observedCompletedFlashReset";

		public static readonly StringName _threadAllocatedBefore = "_threadAllocatedBefore";

		public static readonly StringName _totalAllocatedBefore = "_totalAllocatedBefore";

		public static readonly StringName _gen0Before = "_gen0Before";

		public static readonly StringName _gen1Before = "_gen1Before";

		public static readonly StringName _gen2Before = "_gen2Before";

		public static readonly StringName _originalMaxFps = "_originalMaxFps";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private readonly List<TowerDefenseCharacter> _targets = new List<TowerDefenseCharacter>();

	private string _runtimeProfile = "unknown";

	private double[] _frameSeconds;

	private bool _setupComplete;

	private bool _measuring;

	private long _phaseStartTimestamp;

	private long _lastFrameTimestamp;

	private double _nextFlashSeconds;

	private int _frameCount;

	private int _flashBursts;

	private long _flashRequests;

	private long _maxFlashBurstTicks;

	private long _totalFlashBurstTicks;

	private int _maxProcessedTweens;

	private int _maxBatchedFlashComponents;

	private int _maxGpuFlashComponents;

	private int _physicsProcessingTargets;

	private bool _observedBrightStrength;

	private bool _observedCompletedFlashReset;

	private long _threadAllocatedBefore;

	private long _totalAllocatedBefore;

	private int _gen0Before;

	private int _gen1Before;

	private int _gen2Before;

	private int _originalMaxFps;

	[Export(PropertyHint.None, "")]
	public int TargetCount { get; set; } = 1000;

	[Export(PropertyHint.None, "")]
	public double WarmupSeconds { get; set; } = 2.0;

	[Export(PropertyHint.None, "")]
	public double MeasureSeconds { get; set; } = 5.0;

	[Export(PropertyHint.None, "")]
	public double FlashIntervalSeconds { get; set; } = 0.25;

	[Export(PropertyHint.None, "")]
	public float FlashDurationSeconds { get; set; } = 0.2f;

	[Export(PropertyHint.None, "")]
	public bool EnableFlash { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool VisibleTargets { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public float TargetScale { get; set; } = 0.35f;

	[Export(PropertyHint.None, "")]
	public int GridColumns { get; set; } = 40;

	[Export(PropertyHint.None, "")]
	public int MaxFrameSamples { get; set; } = 100000;

	[Export(PropertyHint.None, "")]
	public int BenchmarkMaxFps { get; set; } = 360;

	[Export(PropertyHint.None, "")]
	public int ProcessedTweenLimit { get; set; }

	[Export(PropertyHint.None, "")]
	public bool RequireGpuFlashEnvelope { get; set; } = true;

	public override void _Ready()
	{
		SetProcess(enable: false);
		ApplyCommandLineArguments();
		_originalMaxFps = Engine.MaxFps;
		Callable.From(Setup).CallDeferred();
	}

	public override void _Process(double delta)
	{
		if (!_setupComplete)
		{
			return;
		}
		if (Engine.MaxFps != BenchmarkMaxFps)
		{
			Engine.MaxFps = BenchmarkMaxFps;
		}
		long timestamp = Stopwatch.GetTimestamp();
		double elapsedSeconds = GetElapsedSeconds(_phaseStartTimestamp, timestamp);
		ObserveFlashState();
		if (!_measuring)
		{
			TryTriggerFlash(elapsedSeconds);
			if (elapsedSeconds >= WarmupSeconds)
			{
				BeginMeasurement();
			}
			return;
		}
		if (_frameCount >= _frameSeconds.Length)
		{
			Finish(capacitySafe: false, "frame-sample-capacity");
			return;
		}
		double elapsedSeconds2 = GetElapsedSeconds(_lastFrameTimestamp, timestamp);
		_lastFrameTimestamp = timestamp;
		if (elapsedSeconds2 > 0.0 && double.IsFinite(elapsedSeconds2))
		{
			_frameSeconds[_frameCount++] = elapsedSeconds2;
		}
		TryTriggerFlash(elapsedSeconds);
		if (elapsedSeconds >= MeasureSeconds)
		{
			Finish(capacitySafe: true, string.Empty);
		}
	}

	private void Setup()
	{
		PackedScene packedScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			FailSetup("zombie-scene-load");
			return;
		}
		int num = Math.Max(1, GridColumns);
		for (int i = 0; i < TargetCount; i++)
		{
			if (!(packedScene.Instantiate(PackedScene.GenEditState.Disabled) is TowerDefenseCharacter towerDefenseCharacter))
			{
				FailSetup($"target-instantiate-{i}");
				return;
			}
			towerDefenseCharacter.inGame = false;
			towerDefenseCharacter.editorPreviewMode = true;
			towerDefenseCharacter.Visible = VisibleTargets;
			towerDefenseCharacter.Scale = Vector2.One * Math.Max(0.01f, TargetScale);
			towerDefenseCharacter.Position = new Vector2(20f + (float)(i % num) * 30f, 30f + (float)(i / num) * 36f);
			AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
			if (towerDefenseCharacter.IsPhysicsProcessing())
			{
				_physicsProcessingTargets++;
			}
			if (towerDefenseCharacter.hitFlashComponent == null || towerDefenseCharacter.hitFlashComponent.IsReleased)
			{
				FailSetup($"hit-flash-runtime-{i}");
				return;
			}
			towerDefenseCharacter.hitFlashComponent.restartBrightOnHit = false;
			_targets.Add(towerDefenseCharacter);
		}
		_frameSeconds = new double[Math.Max(256, MaxFrameSamples)];
		Engine.MaxFps = BenchmarkMaxFps;
		_setupComplete = true;
		_phaseStartTimestamp = Stopwatch.GetTimestamp();
		_lastFrameTimestamp = _phaseStartTimestamp;
		_nextFlashSeconds = 0.0;
		SetProcess(enable: true);
		GD.Print($"[HitFlashTweenStressReady] runtimeProfile={_runtimeProfile} targets={_targets.Count}/{TargetCount} flashEnabled={EnableFlash} physicsProcessingTargets={_physicsProcessingTargets} visibleTargets={VisibleTargets} warmupSeconds={WarmupSeconds:F3} measureSeconds={MeasureSeconds:F3} intervalSeconds={FlashIntervalSeconds:F3} durationSeconds={FlashDurationSeconds:F3} maxFps={BenchmarkMaxFps}");
	}

	private void BeginMeasurement()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
		_frameCount = 0;
		_flashBursts = 0;
		_flashRequests = 0L;
		_maxFlashBurstTicks = 0L;
		_totalFlashBurstTicks = 0L;
		_maxProcessedTweens = 0;
		_maxBatchedFlashComponents = 0;
		_maxGpuFlashComponents = 0;
		_observedBrightStrength = false;
		_observedCompletedFlashReset = false;
		_threadAllocatedBefore = GC.GetAllocatedBytesForCurrentThread();
		_totalAllocatedBefore = GC.GetTotalAllocatedBytes();
		_gen0Before = GC.CollectionCount(0);
		_gen1Before = GC.CollectionCount(1);
		_gen2Before = GC.CollectionCount(2);
		_measuring = true;
		_phaseStartTimestamp = Stopwatch.GetTimestamp();
		_lastFrameTimestamp = _phaseStartTimestamp;
		_nextFlashSeconds = 0.0;
	}

	private void TryTriggerFlash(double phaseElapsed)
	{
		if (!EnableFlash || phaseElapsed < _nextFlashSeconds)
		{
			return;
		}
		_nextFlashSeconds = phaseElapsed + Math.Max(0.001, FlashIntervalSeconds);
		long timestamp = Stopwatch.GetTimestamp();
		int num = 0;
		for (int i = 0; i < _targets.Count; i++)
		{
			_targets[i].Bright(0.5, 0.0, 0.5, 0.0, Math.Max(0.001f, FlashDurationSeconds));
			if (_targets[i].hitFlashComponent.IsGpuFlashEnvelopeActive)
			{
				num++;
			}
		}
		_flashBursts++;
		_flashRequests += _targets.Count;
		long num2 = Stopwatch.GetTimestamp() - timestamp;
		_maxFlashBurstTicks = Math.Max(_maxFlashBurstTicks, num2);
		_totalFlashBurstTicks += num2;
		_maxProcessedTweens = Math.Max(_maxProcessedTweens, GetTree().GetProcessedTweens().Count);
		_maxBatchedFlashComponents = Math.Max(_maxBatchedFlashComponents, TowerDefenseHitFlashBatch.ActiveCount);
		_maxGpuFlashComponents = Math.Max(_maxGpuFlashComponents, num);
	}

	private void Finish(bool capacitySafe, string failureReason)
	{
		SetProcess(enable: false);
		long num = GC.GetAllocatedBytesForCurrentThread() - _threadAllocatedBefore;
		long num2 = GC.GetTotalAllocatedBytes() - _totalAllocatedBefore;
		int value = GC.CollectionCount(0) - _gen0Before;
		int value2 = GC.CollectionCount(1) - _gen1Before;
		int value3 = GC.CollectionCount(2) - _gen2Before;
		Array.Sort(_frameSeconds, 0, _frameCount);
		double num3 = 0.0;
		for (int i = 0; i < _frameCount; i++)
		{
			num3 += _frameSeconds[i];
		}
		double num4 = ((num3 > 0.0) ? ((double)_frameCount / num3) : 0.0);
		double num5 = CalculateOnePercentLowFps();
		double value4 = Percentile(50.0) * 1000.0;
		double value5 = Percentile(95.0) * 1000.0;
		double num6 = Percentile(99.0) * 1000.0;
		double value6 = ((_frameCount > 0) ? (_frameSeconds[_frameCount - 1] * 1000.0) : 0.0);
		bool flag = capacitySafe && _targets.Count == TargetCount && _physicsProcessingTargets == 0 && _frameCount >= 30 && double.IsFinite(num4) && double.IsFinite(num5) && double.IsFinite(num6) && num >= 0 && num2 >= 0 && _maxProcessedTweens <= ProcessedTweenLimit && (!EnableFlash || ((!RequireGpuFlashEnvelope || _maxGpuFlashComponents == TargetCount) && _observedBrightStrength && _observedCompletedFlashReset));
		GD.Print($"[HitFlashTweenStressResult] runtimeProfile={_runtimeProfile} targets={_targets.Count}/{TargetCount} flashEnabled={EnableFlash} physicsProcessingTargets={_physicsProcessingTargets} visibleTargets={VisibleTargets} maxFps={BenchmarkMaxFps} frames={_frameCount} averageFps={num4:F3} onePercentLowFps={num5:F3} frameP50Ms={value4:F6} frameP95Ms={value5:F6} frameP99Ms={num6:F6} frameMaxMs={value6:F6} flashBursts={_flashBursts} flashRequests={_flashRequests} averageFlashBurstMs={GetAverageFlashBurstMilliseconds():F6} maxFlashBurstMs={GetMilliseconds(_maxFlashBurstTicks):F6} maxProcessedTweens={_maxProcessedTweens} processedTweenLimit={ProcessedTweenLimit} maxBatchedFlashComponents={_maxBatchedFlashComponents} maxGpuFlashComponents={_maxGpuFlashComponents} requireGpuFlashEnvelope={RequireGpuFlashEnvelope} observedBrightStrength={_observedBrightStrength} observedCompletedFlashReset={_observedCompletedFlashReset} threadAllocatedBytes={num} totalAllocatedBytes={num2} allocatedBytesPerFlashRequest={GetAllocatedBytesPerRequest(num):F3} gen0={value} gen1={value2} gen2={value3} failureReason={failureReason} passed={flag}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private void ObserveFlashState()
	{
		if (!EnableFlash || _targets.Count == 0)
		{
			return;
		}
		HitFlashComponent hitFlashComponent = _targets[0].hitFlashComponent;
		if (hitFlashComponent != null && !hitFlashComponent.IsReleased)
		{
			if (hitFlashComponent.CurrentBrightStrength > 0.0001f)
			{
				_observedBrightStrength = true;
			}
			if (_observedBrightStrength && !hitFlashComponent.IsVisualFlashActive && Mathf.IsZeroApprox(hitFlashComponent.CurrentBrightStrength))
			{
				_observedCompletedFlashReset = true;
			}
		}
	}

	private double CalculateOnePercentLowFps()
	{
		if (_frameCount <= 0)
		{
			return 0.0;
		}
		int num = Math.Max(1, (int)Math.Ceiling((double)_frameCount * 0.01));
		double num2 = 0.0;
		for (int i = _frameCount - num; i < _frameCount; i++)
		{
			num2 += _frameSeconds[i];
		}
		if (!(num2 > 0.0))
		{
			return 0.0;
		}
		return (double)num / num2;
	}

	private double Percentile(double percentile)
	{
		if (_frameCount <= 0)
		{
			return 0.0;
		}
		double num = Math.Clamp(percentile, 0.0, 100.0) / 100.0 * (double)(_frameCount - 1);
		int num2 = (int)Math.Floor(num);
		int num3 = (int)Math.Ceiling(num);
		if (num2 == num3)
		{
			return _frameSeconds[num2];
		}
		double num4 = num - (double)num2;
		return _frameSeconds[num2] + (_frameSeconds[num3] - _frameSeconds[num2]) * num4;
	}

	private double GetAllocatedBytesPerRequest(long allocatedBytes)
	{
		if (_flashRequests <= 0)
		{
			return 0.0;
		}
		return (double)allocatedBytes / (double)_flashRequests;
	}

	private double GetAverageFlashBurstMilliseconds()
	{
		if (_flashBursts <= 0)
		{
			return 0.0;
		}
		return GetMilliseconds(_totalFlashBurstTicks) / (double)_flashBursts;
	}

	private static double GetMilliseconds(long ticks)
	{
		return (double)ticks * 1000.0 / (double)Stopwatch.Frequency;
	}

	private static double GetElapsedSeconds(long start, long end)
	{
		return (double)(end - start) / (double)Stopwatch.Frequency;
	}

	private void FailSetup(string reason)
	{
		GD.PrintErr($"[HitFlashTweenStressResult] runtimeProfile={_runtimeProfile} targets={_targets.Count}/{TargetCount} failureReason={reason} passed=False");
		GetTree().Quit(2);
	}

	private void ApplyCommandLineArguments()
	{
		string[] cmdlineUserArgs = OS.GetCmdlineUserArgs();
		foreach (string argument in cmdlineUserArgs)
		{
			double value2;
			double value3;
			double value4;
			double value5;
			bool value6;
			bool value7;
			int value8;
			bool value9;
			int value10;
			string value11;
			if (TryReadInt(argument, "--hit-flash-targets=", out var value))
			{
				TargetCount = Math.Max(1, value);
			}
			else if (TryReadDouble(argument, "--hit-flash-warmup=", out value2))
			{
				WarmupSeconds = Math.Max(0.0, value2);
			}
			else if (TryReadDouble(argument, "--hit-flash-measure=", out value3))
			{
				MeasureSeconds = Math.Max(0.25, value3);
			}
			else if (TryReadDouble(argument, "--hit-flash-interval=", out value4))
			{
				FlashIntervalSeconds = Math.Max(0.001, value4);
			}
			else if (TryReadDouble(argument, "--hit-flash-duration=", out value5))
			{
				FlashDurationSeconds = (float)Math.Max(0.001, value5);
			}
			else if (TryReadBool(argument, "--hit-flash-enabled=", out value6))
			{
				EnableFlash = value6;
			}
			else if (TryReadBool(argument, "--hit-flash-visible=", out value7))
			{
				VisibleTargets = value7;
			}
			else if (TryReadInt(argument, "--hit-flash-max-fps=", out value8))
			{
				BenchmarkMaxFps = Math.Max(0, value8);
			}
			else if (TryReadBool(argument, "--hit-flash-require-gpu-envelope=", out value9))
			{
				RequireGpuFlashEnvelope = value9;
			}
			else if (TryReadInt(argument, "--hit-flash-processed-tween-limit=", out value10))
			{
				ProcessedTweenLimit = Math.Max(0, value10);
			}
			else if (TryReadString(argument, "--runtime-profile=", out value11))
			{
				_runtimeProfile = value11;
			}
		}
	}

	public override void _ExitTree()
	{
		Engine.MaxFps = _originalMaxFps;
	}

	private static bool TryReadInt(string argument, string prefix, out int value)
	{
		value = 0;
		if (argument.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			int length = prefix.Length;
			return int.TryParse(argument.Substring(length, argument.Length - length), out value);
		}
		return false;
	}

	private static bool TryReadDouble(string argument, string prefix, out double value)
	{
		value = 0.0;
		if (argument.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			int length = prefix.Length;
			return double.TryParse(argument.Substring(length, argument.Length - length), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
		}
		return false;
	}

	private static bool TryReadBool(string argument, string prefix, out bool value)
	{
		value = false;
		if (argument.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			int length = prefix.Length;
			return bool.TryParse(argument.Substring(length, argument.Length - length), out value);
		}
		return false;
	}

	private static bool TryReadString(string argument, string prefix, out string value)
	{
		value = string.Empty;
		if (!argument.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		int length = prefix.Length;
		value = argument.Substring(length, argument.Length - length).Trim();
		return value.Length > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Setup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginMeasurement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryTriggerFlash, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "phaseElapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "capacitySafe", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "failureReason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ObserveFlashState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CalculateOnePercentLowFps, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Percentile, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "percentile", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAllocatedBytesPerRequest, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "allocatedBytes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAverageFlashBurstMilliseconds, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMilliseconds, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "ticks", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetElapsedSeconds, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "start", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "end", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FailSetup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCommandLineArguments, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Setup && args.Count == 0)
		{
			Setup();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginMeasurement && args.Count == 0)
		{
			BeginMeasurement();
			ret = default;
			return true;
		}
		if (method == MethodName.TryTriggerFlash && args.Count == 1)
		{
			TryTriggerFlash(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 2)
		{
			Finish(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ObserveFlashState && args.Count == 0)
		{
			ObserveFlashState();
			ret = default;
			return true;
		}
		if (method == MethodName.CalculateOnePercentLowFps && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(CalculateOnePercentLowFps());
			return true;
		}
		if (method == MethodName.Percentile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(Percentile(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAllocatedBytesPerRequest && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetAllocatedBytesPerRequest(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAverageFlashBurstMilliseconds && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetAverageFlashBurstMilliseconds());
			return true;
		}
		if (method == MethodName.GetMilliseconds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetMilliseconds(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.GetElapsedSeconds && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(GetElapsedSeconds(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1])));
			return true;
		}
		if (method == MethodName.FailSetup && args.Count == 1)
		{
			FailSetup(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCommandLineArguments && args.Count == 0)
		{
			ApplyCommandLineArguments();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetMilliseconds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetMilliseconds(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.GetElapsedSeconds && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(GetElapsedSeconds(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1])));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.Setup)
		{
			return true;
		}
		if (method == MethodName.BeginMeasurement)
		{
			return true;
		}
		if (method == MethodName.TryTriggerFlash)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		if (method == MethodName.ObserveFlashState)
		{
			return true;
		}
		if (method == MethodName.CalculateOnePercentLowFps)
		{
			return true;
		}
		if (method == MethodName.Percentile)
		{
			return true;
		}
		if (method == MethodName.GetAllocatedBytesPerRequest)
		{
			return true;
		}
		if (method == MethodName.GetAverageFlashBurstMilliseconds)
		{
			return true;
		}
		if (method == MethodName.GetMilliseconds)
		{
			return true;
		}
		if (method == MethodName.GetElapsedSeconds)
		{
			return true;
		}
		if (method == MethodName.FailSetup)
		{
			return true;
		}
		if (method == MethodName.ApplyCommandLineArguments)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.TargetCount)
		{
			TargetCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.WarmupSeconds)
		{
			WarmupSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.MeasureSeconds)
		{
			MeasureSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.FlashIntervalSeconds)
		{
			FlashIntervalSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.FlashDurationSeconds)
		{
			FlashDurationSeconds = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.EnableFlash)
		{
			EnableFlash = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.VisibleTargets)
		{
			VisibleTargets = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.TargetScale)
		{
			TargetScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.GridColumns)
		{
			GridColumns = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.MaxFrameSamples)
		{
			MaxFrameSamples = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.BenchmarkMaxFps)
		{
			BenchmarkMaxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ProcessedTweenLimit)
		{
			ProcessedTweenLimit = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.RequireGpuFlashEnvelope)
		{
			RequireGpuFlashEnvelope = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeProfile)
		{
			_runtimeProfile = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._frameSeconds)
		{
			_frameSeconds = VariantUtils.ConvertTo<double[]>(in value);
			return true;
		}
		if (name == PropertyName._setupComplete)
		{
			_setupComplete = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._measuring)
		{
			_measuring = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._phaseStartTimestamp)
		{
			_phaseStartTimestamp = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._lastFrameTimestamp)
		{
			_lastFrameTimestamp = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._nextFlashSeconds)
		{
			_nextFlashSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._frameCount)
		{
			_frameCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._flashBursts)
		{
			_flashBursts = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._flashRequests)
		{
			_flashRequests = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._maxFlashBurstTicks)
		{
			_maxFlashBurstTicks = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._totalFlashBurstTicks)
		{
			_totalFlashBurstTicks = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._maxProcessedTweens)
		{
			_maxProcessedTweens = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._maxBatchedFlashComponents)
		{
			_maxBatchedFlashComponents = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._maxGpuFlashComponents)
		{
			_maxGpuFlashComponents = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._physicsProcessingTargets)
		{
			_physicsProcessingTargets = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._observedBrightStrength)
		{
			_observedBrightStrength = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._observedCompletedFlashReset)
		{
			_observedCompletedFlashReset = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._threadAllocatedBefore)
		{
			_threadAllocatedBefore = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._totalAllocatedBefore)
		{
			_totalAllocatedBefore = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._gen0Before)
		{
			_gen0Before = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gen1Before)
		{
			_gen1Before = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gen2Before)
		{
			_gen2Before = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._originalMaxFps)
		{
			_originalMaxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.TargetCount)
		{
			from = TargetCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		double from2;
		if (name == PropertyName.WarmupSeconds)
		{
			from2 = WarmupSeconds;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.MeasureSeconds)
		{
			from2 = MeasureSeconds;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.FlashIntervalSeconds)
		{
			from2 = FlashIntervalSeconds;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		float from3;
		if (name == PropertyName.FlashDurationSeconds)
		{
			from3 = FlashDurationSeconds;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		bool from4;
		if (name == PropertyName.EnableFlash)
		{
			from4 = EnableFlash;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.VisibleTargets)
		{
			from4 = VisibleTargets;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.TargetScale)
		{
			from3 = TargetScale;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.GridColumns)
		{
			from = GridColumns;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MaxFrameSamples)
		{
			from = MaxFrameSamples;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.BenchmarkMaxFps)
		{
			from = BenchmarkMaxFps;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ProcessedTweenLimit)
		{
			from = ProcessedTweenLimit;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RequireGpuFlashEnvelope)
		{
			from4 = RequireGpuFlashEnvelope;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName._runtimeProfile)
		{
			value = VariantUtils.CreateFrom(in _runtimeProfile);
			return true;
		}
		if (name == PropertyName._frameSeconds)
		{
			value = VariantUtils.CreateFrom(in _frameSeconds);
			return true;
		}
		if (name == PropertyName._setupComplete)
		{
			value = VariantUtils.CreateFrom(in _setupComplete);
			return true;
		}
		if (name == PropertyName._measuring)
		{
			value = VariantUtils.CreateFrom(in _measuring);
			return true;
		}
		if (name == PropertyName._phaseStartTimestamp)
		{
			value = VariantUtils.CreateFrom(in _phaseStartTimestamp);
			return true;
		}
		if (name == PropertyName._lastFrameTimestamp)
		{
			value = VariantUtils.CreateFrom(in _lastFrameTimestamp);
			return true;
		}
		if (name == PropertyName._nextFlashSeconds)
		{
			value = VariantUtils.CreateFrom(in _nextFlashSeconds);
			return true;
		}
		if (name == PropertyName._frameCount)
		{
			value = VariantUtils.CreateFrom(in _frameCount);
			return true;
		}
		if (name == PropertyName._flashBursts)
		{
			value = VariantUtils.CreateFrom(in _flashBursts);
			return true;
		}
		if (name == PropertyName._flashRequests)
		{
			value = VariantUtils.CreateFrom(in _flashRequests);
			return true;
		}
		if (name == PropertyName._maxFlashBurstTicks)
		{
			value = VariantUtils.CreateFrom(in _maxFlashBurstTicks);
			return true;
		}
		if (name == PropertyName._totalFlashBurstTicks)
		{
			value = VariantUtils.CreateFrom(in _totalFlashBurstTicks);
			return true;
		}
		if (name == PropertyName._maxProcessedTweens)
		{
			value = VariantUtils.CreateFrom(in _maxProcessedTweens);
			return true;
		}
		if (name == PropertyName._maxBatchedFlashComponents)
		{
			value = VariantUtils.CreateFrom(in _maxBatchedFlashComponents);
			return true;
		}
		if (name == PropertyName._maxGpuFlashComponents)
		{
			value = VariantUtils.CreateFrom(in _maxGpuFlashComponents);
			return true;
		}
		if (name == PropertyName._physicsProcessingTargets)
		{
			value = VariantUtils.CreateFrom(in _physicsProcessingTargets);
			return true;
		}
		if (name == PropertyName._observedBrightStrength)
		{
			value = VariantUtils.CreateFrom(in _observedBrightStrength);
			return true;
		}
		if (name == PropertyName._observedCompletedFlashReset)
		{
			value = VariantUtils.CreateFrom(in _observedCompletedFlashReset);
			return true;
		}
		if (name == PropertyName._threadAllocatedBefore)
		{
			value = VariantUtils.CreateFrom(in _threadAllocatedBefore);
			return true;
		}
		if (name == PropertyName._totalAllocatedBefore)
		{
			value = VariantUtils.CreateFrom(in _totalAllocatedBefore);
			return true;
		}
		if (name == PropertyName._gen0Before)
		{
			value = VariantUtils.CreateFrom(in _gen0Before);
			return true;
		}
		if (name == PropertyName._gen1Before)
		{
			value = VariantUtils.CreateFrom(in _gen1Before);
			return true;
		}
		if (name == PropertyName._gen2Before)
		{
			value = VariantUtils.CreateFrom(in _gen2Before);
			return true;
		}
		if (name == PropertyName._originalMaxFps)
		{
			value = VariantUtils.CreateFrom(in _originalMaxFps);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.TargetCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.WarmupSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MeasureSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.FlashIntervalSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.FlashDurationSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableFlash, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.VisibleTargets, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.TargetScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.GridColumns, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.MaxFrameSamples, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.BenchmarkMaxFps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ProcessedTweenLimit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RequireGpuFlashEnvelope, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._runtimeProfile, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedFloat64Array, PropertyName._frameSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._setupComplete, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._measuring, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._phaseStartTimestamp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastFrameTimestamp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._nextFlashSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._flashBursts, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._flashRequests, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._maxFlashBurstTicks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._totalFlashBurstTicks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._maxProcessedTweens, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._maxBatchedFlashComponents, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._maxGpuFlashComponents, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._physicsProcessingTargets, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._observedBrightStrength, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._observedCompletedFlashReset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._threadAllocatedBefore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._totalAllocatedBefore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gen0Before, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gen1Before, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gen2Before, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalMaxFps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.TargetCount, Variant.From<int>(TargetCount));
		info.AddProperty(PropertyName.WarmupSeconds, Variant.From<double>(WarmupSeconds));
		info.AddProperty(PropertyName.MeasureSeconds, Variant.From<double>(MeasureSeconds));
		info.AddProperty(PropertyName.FlashIntervalSeconds, Variant.From<double>(FlashIntervalSeconds));
		info.AddProperty(PropertyName.FlashDurationSeconds, Variant.From<float>(FlashDurationSeconds));
		info.AddProperty(PropertyName.EnableFlash, Variant.From<bool>(EnableFlash));
		info.AddProperty(PropertyName.VisibleTargets, Variant.From<bool>(VisibleTargets));
		info.AddProperty(PropertyName.TargetScale, Variant.From<float>(TargetScale));
		info.AddProperty(PropertyName.GridColumns, Variant.From<int>(GridColumns));
		info.AddProperty(PropertyName.MaxFrameSamples, Variant.From<int>(MaxFrameSamples));
		info.AddProperty(PropertyName.BenchmarkMaxFps, Variant.From<int>(BenchmarkMaxFps));
		info.AddProperty(PropertyName.ProcessedTweenLimit, Variant.From<int>(ProcessedTweenLimit));
		info.AddProperty(PropertyName.RequireGpuFlashEnvelope, Variant.From<bool>(RequireGpuFlashEnvelope));
		info.AddProperty(PropertyName._runtimeProfile, Variant.From(in _runtimeProfile));
		info.AddProperty(PropertyName._frameSeconds, Variant.From(in _frameSeconds));
		info.AddProperty(PropertyName._setupComplete, Variant.From(in _setupComplete));
		info.AddProperty(PropertyName._measuring, Variant.From(in _measuring));
		info.AddProperty(PropertyName._phaseStartTimestamp, Variant.From(in _phaseStartTimestamp));
		info.AddProperty(PropertyName._lastFrameTimestamp, Variant.From(in _lastFrameTimestamp));
		info.AddProperty(PropertyName._nextFlashSeconds, Variant.From(in _nextFlashSeconds));
		info.AddProperty(PropertyName._frameCount, Variant.From(in _frameCount));
		info.AddProperty(PropertyName._flashBursts, Variant.From(in _flashBursts));
		info.AddProperty(PropertyName._flashRequests, Variant.From(in _flashRequests));
		info.AddProperty(PropertyName._maxFlashBurstTicks, Variant.From(in _maxFlashBurstTicks));
		info.AddProperty(PropertyName._totalFlashBurstTicks, Variant.From(in _totalFlashBurstTicks));
		info.AddProperty(PropertyName._maxProcessedTweens, Variant.From(in _maxProcessedTweens));
		info.AddProperty(PropertyName._maxBatchedFlashComponents, Variant.From(in _maxBatchedFlashComponents));
		info.AddProperty(PropertyName._maxGpuFlashComponents, Variant.From(in _maxGpuFlashComponents));
		info.AddProperty(PropertyName._physicsProcessingTargets, Variant.From(in _physicsProcessingTargets));
		info.AddProperty(PropertyName._observedBrightStrength, Variant.From(in _observedBrightStrength));
		info.AddProperty(PropertyName._observedCompletedFlashReset, Variant.From(in _observedCompletedFlashReset));
		info.AddProperty(PropertyName._threadAllocatedBefore, Variant.From(in _threadAllocatedBefore));
		info.AddProperty(PropertyName._totalAllocatedBefore, Variant.From(in _totalAllocatedBefore));
		info.AddProperty(PropertyName._gen0Before, Variant.From(in _gen0Before));
		info.AddProperty(PropertyName._gen1Before, Variant.From(in _gen1Before));
		info.AddProperty(PropertyName._gen2Before, Variant.From(in _gen2Before));
		info.AddProperty(PropertyName._originalMaxFps, Variant.From(in _originalMaxFps));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.TargetCount, out var value))
		{
			TargetCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.WarmupSeconds, out var value2))
		{
			WarmupSeconds = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.MeasureSeconds, out var value3))
		{
			MeasureSeconds = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.FlashIntervalSeconds, out var value4))
		{
			FlashIntervalSeconds = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.FlashDurationSeconds, out var value5))
		{
			FlashDurationSeconds = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.EnableFlash, out var value6))
		{
			EnableFlash = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.VisibleTargets, out var value7))
		{
			VisibleTargets = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.TargetScale, out var value8))
		{
			TargetScale = value8.As<float>();
		}
		if (info.TryGetProperty(PropertyName.GridColumns, out var value9))
		{
			GridColumns = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.MaxFrameSamples, out var value10))
		{
			MaxFrameSamples = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.BenchmarkMaxFps, out var value11))
		{
			BenchmarkMaxFps = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ProcessedTweenLimit, out var value12))
		{
			ProcessedTweenLimit = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName.RequireGpuFlashEnvelope, out var value13))
		{
			RequireGpuFlashEnvelope = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeProfile, out var value14))
		{
			_runtimeProfile = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName._frameSeconds, out var value15))
		{
			_frameSeconds = value15.As<double[]>();
		}
		if (info.TryGetProperty(PropertyName._setupComplete, out var value16))
		{
			_setupComplete = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._measuring, out var value17))
		{
			_measuring = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._phaseStartTimestamp, out var value18))
		{
			_phaseStartTimestamp = value18.As<long>();
		}
		if (info.TryGetProperty(PropertyName._lastFrameTimestamp, out var value19))
		{
			_lastFrameTimestamp = value19.As<long>();
		}
		if (info.TryGetProperty(PropertyName._nextFlashSeconds, out var value20))
		{
			_nextFlashSeconds = value20.As<double>();
		}
		if (info.TryGetProperty(PropertyName._frameCount, out var value21))
		{
			_frameCount = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName._flashBursts, out var value22))
		{
			_flashBursts = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName._flashRequests, out var value23))
		{
			_flashRequests = value23.As<long>();
		}
		if (info.TryGetProperty(PropertyName._maxFlashBurstTicks, out var value24))
		{
			_maxFlashBurstTicks = value24.As<long>();
		}
		if (info.TryGetProperty(PropertyName._totalFlashBurstTicks, out var value25))
		{
			_totalFlashBurstTicks = value25.As<long>();
		}
		if (info.TryGetProperty(PropertyName._maxProcessedTweens, out var value26))
		{
			_maxProcessedTweens = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._maxBatchedFlashComponents, out var value27))
		{
			_maxBatchedFlashComponents = value27.As<int>();
		}
		if (info.TryGetProperty(PropertyName._maxGpuFlashComponents, out var value28))
		{
			_maxGpuFlashComponents = value28.As<int>();
		}
		if (info.TryGetProperty(PropertyName._physicsProcessingTargets, out var value29))
		{
			_physicsProcessingTargets = value29.As<int>();
		}
		if (info.TryGetProperty(PropertyName._observedBrightStrength, out var value30))
		{
			_observedBrightStrength = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._observedCompletedFlashReset, out var value31))
		{
			_observedCompletedFlashReset = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._threadAllocatedBefore, out var value32))
		{
			_threadAllocatedBefore = value32.As<long>();
		}
		if (info.TryGetProperty(PropertyName._totalAllocatedBefore, out var value33))
		{
			_totalAllocatedBefore = value33.As<long>();
		}
		if (info.TryGetProperty(PropertyName._gen0Before, out var value34))
		{
			_gen0Before = value34.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gen1Before, out var value35))
		{
			_gen1Before = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gen2Before, out var value36))
		{
			_gen2Before = value36.As<int>();
		}
		if (info.TryGetProperty(PropertyName._originalMaxFps, out var value37))
		{
			_originalMaxFps = value37.As<int>();
		}
	}
}
