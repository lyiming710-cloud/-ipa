using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BulletFieldWorkloadTest.cs")]
public class BulletFieldWorkloadTest : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName StartMeasurementTelemetry = "StartMeasurementTelemetry";

		public static readonly StringName LoadConfigsAndReferences = "LoadConfigsAndReferences";

		public static readonly StringName SpawnWorkload = "SpawnWorkload";

		public static readonly StringName CountActiveWorkloadVariants = "CountActiveWorkloadVariants";

		public static readonly StringName BuildTransitionConfigs = "BuildTransitionConfigs";

		public static readonly StringName RunBurstTransition = "RunBurstTransition";

		public static readonly StringName PrimeRoutes = "PrimeRoutes";

		public static readonly StringName CreateBackgroundAndLabels = "CreateBackgroundAndLabels";

		public static readonly StringName AddLabel = "AddLabel";

		public static readonly StringName CaptureScreenshot = "CaptureScreenshot";

		public static readonly StringName FinishTest = "FinishTest";

		public static readonly StringName ApplyCommandLineArguments = "ApplyCommandLineArguments";

		public static readonly StringName StartContentionThreads = "StartContentionThreads";

		public static readonly StringName RunContentionWorker = "RunContentionWorker";

		public static readonly StringName StopContentionThreads = "StopContentionThreads";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName SpawnCount = "SpawnCount";

		public static readonly StringName ZBucketCount = "ZBucketCount";

		public static readonly StringName BulletScale = "BulletScale";

		public static readonly StringName WarmupSeconds = "WarmupSeconds";

		public static readonly StringName MeasureSeconds = "MeasureSeconds";

		public static readonly StringName PrimeSpawnRoutes = "PrimeSpawnRoutes";

		public static readonly StringName MaxPrimedSpawnMilliseconds = "MaxPrimedSpawnMilliseconds";

		public static readonly StringName ScreenshotPath = "ScreenshotPath";

		public static readonly StringName RuntimeProfile = "RuntimeProfile";

		public static readonly StringName EnableDetailedProfiler = "EnableDetailedProfiler";

		public static readonly StringName BurstTransitionMode = "BurstTransitionMode";

		public static readonly StringName BurstTransitionBudgetMilliseconds = "BurstTransitionBudgetMilliseconds";

		public static readonly StringName BenchmarkMaxFps = "BenchmarkMaxFps";

		public static readonly StringName MinimumAverageFps = "MinimumAverageFps";

		public static readonly StringName MinimumOnePercentLowFps = "MinimumOnePercentLowFps";

		public static readonly StringName MinimumZeroPointOnePercentLowFps = "MinimumZeroPointOnePercentLowFps";

		public static readonly StringName BackgroundContentionThreads = "BackgroundContentionThreads";

		public static readonly StringName MixedProjectileDefinitions = "MixedProjectileDefinitions";

		public static readonly StringName MinimumAnimationDefinitionCount = "MinimumAnimationDefinitionCount";

		public static readonly StringName _workloadVariantSpawnCounts = "_workloadVariantSpawnCounts";

		public static readonly StringName _bulletField = "_bulletField";

		public static readonly StringName _manager = "_manager";

		public static readonly StringName _originalGridSize = "_originalGridSize";

		public static readonly StringName _originalGridBegin = "_originalGridBegin";

		public static readonly StringName _originalGridNum = "_originalGridNum";

		public static readonly StringName _originalMaxFps = "_originalMaxFps";

		public static readonly StringName _originalPhysicsTicks = "_originalPhysicsTicks";

		public static readonly StringName _originalTimeScale = "_originalTimeScale";

		public static readonly StringName _projectileUpdateManager = "_projectileUpdateManager";

		public static readonly StringName _originalProjectileManagerProcessMode = "_originalProjectileManagerProcessMode";

		public static readonly StringName _projectileManagerProcessModeCaptured = "_projectileManagerProcessModeCaptured";

		public static readonly StringName _originalPerfProfilerEnabled = "_originalPerfProfilerEnabled";

		public static readonly StringName _originalDetailedHotPathMetrics = "_originalDetailedHotPathMetrics";

		public static readonly StringName _originalProfilerDumpIntervalFrames = "_originalProfilerDumpIntervalFrames";

		public static readonly StringName _originalProfilerMaxMetricsPerDump = "_originalProfilerMaxMetricsPerDump";

		public static readonly StringName _elapsed = "_elapsed";

		public static readonly StringName _measuredRenderSeconds = "_measuredRenderSeconds";

		public static readonly StringName _spawnFailures = "_spawnFailures";

		public static readonly StringName _spawnElapsedMs = "_spawnElapsedMs";

		public static readonly StringName _ready = "_ready";

		public static readonly StringName _screenshotSaved = "_screenshotSaved";

		public static readonly StringName _finished = "_finished";

		public static readonly StringName _measurementStarted = "_measurementStarted";

		public static readonly StringName _measurementThreadAllocatedBefore = "_measurementThreadAllocatedBefore";

		public static readonly StringName _measurementTotalAllocatedBefore = "_measurementTotalAllocatedBefore";

		public static readonly StringName _measurementGen0Before = "_measurementGen0Before";

		public static readonly StringName _measurementGen1Before = "_measurementGen1Before";

		public static readonly StringName _measurementGen2Before = "_measurementGen2Before";

		public static readonly StringName _lastRenderTimestamp = "_lastRenderTimestamp";

		public static readonly StringName _firstPublicationMeasured = "_firstPublicationMeasured";

		public static readonly StringName _firstPublicationMs = "_firstPublicationMs";

		public static readonly StringName _firstPublicationVisible = "_firstPublicationVisible";

		public static readonly StringName _burstTransitionExecuted = "_burstTransitionExecuted";

		public static readonly StringName _contentionIterations = "_contentionIterations";

		public static readonly StringName _stopContentionThreads = "_stopContentionThreads";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static readonly string[] CompactProjectileSceneCandidates = new string[7] { "res://Asset/Config/Projectile/Pea/Sprite/FirePea/FirePea.tscn", "res://Asset/Config/Projectile/Pea/Sprite/FirePeaS/FirePeaS.tscn", "res://Asset/Config/Projectile/Pea/Sprite/IceFirePea/IceFirePea.tscn", "res://Asset/Config/Projectile/Pea/Sprite/MegaFirePea/MegaFirePea.tscn", "res://Asset/Config/Projectile/Star/FireStar/FireStar.tscn", "res://Asset/Config/Projectile/Pea/Sprite/WhiteFirePea/WhiteFirePea.tscn", "res://Asset/Config/Projectile/Puff/Sprite/HypnoPuff/HypnoPuff.tscn" };

	private const int WorkloadVariantCount = 6;

	private readonly List<TowerDefenseProjectileConfig> _configs = new List<TowerDefenseProjectileConfig>();

	private readonly List<int> _spawnedIndices = new List<int>(65536);

	private readonly List<Vector2> _initialPositions = new List<Vector2>(65536);

	private readonly HashSet<int> _spawnedAnimationDefinitionIds = new HashSet<int>();

	private readonly int[] _workloadVariantSpawnCounts = new int[6];

	private readonly Dictionary<int, TowerDefenseProjectileConfig> _transitionConfigs = new Dictionary<int, TowerDefenseProjectileConfig>();

	private readonly Dictionary<int, BulletField.PreparedProjectileChange> _transitionPreparedChanges = new Dictionary<int, BulletField.PreparedProjectileChange>();

	private readonly List<double> _renderFrameTimes = new List<double>(131072);

	private readonly List<double> _processTimesMs = new List<double>(131072);

	private readonly List<double> _physicsFrameTimesMs = new List<double>(131072);

	private readonly List<double> _physicsUpdateTimesMs = new List<double>(16384);

	private BulletField _bulletField;

	private TowerDefenseManager _manager;

	private Vector2 _originalGridSize;

	private Vector2 _originalGridBegin;

	private Vector2I _originalGridNum;

	private int _originalMaxFps;

	private int _originalPhysicsTicks;

	private double _originalTimeScale;

	private ProjectileUpdateManager _projectileUpdateManager;

	private ProcessModeEnum _originalProjectileManagerProcessMode;

	private bool _projectileManagerProcessModeCaptured;

	private bool _originalPerfProfilerEnabled;

	private bool _originalDetailedHotPathMetrics;

	private int _originalProfilerDumpIntervalFrames;

	private int _originalProfilerMaxMetricsPerDump;

	private double _elapsed;

	private double _measuredRenderSeconds;

	private int _spawnFailures;

	private double _spawnElapsedMs;

	private bool _ready;

	private bool _screenshotSaved;

	private bool _finished;

	private bool _measurementStarted;

	private long _measurementThreadAllocatedBefore;

	private long _measurementTotalAllocatedBefore;

	private int _measurementGen0Before;

	private int _measurementGen1Before;

	private int _measurementGen2Before;

	private long _lastRenderTimestamp;

	private bool _firstPublicationMeasured;

	private double _firstPublicationMs;

	private int _firstPublicationVisible;

	private bool _burstTransitionExecuted;

	private Thread[] _contentionThreads = Array.Empty<Thread>();

	private long[] _contentionIterations = Array.Empty<long>();

	private int _stopContentionThreads;

	[Export(PropertyHint.None, "")]
	public int SpawnCount { get; set; } = 10000;

	[Export(PropertyHint.None, "")]
	public int ZBucketCount { get; set; } = 200;

	[Export(PropertyHint.None, "")]
	public float BulletScale { get; set; } = 0.12f;

	[Export(PropertyHint.None, "")]
	public double WarmupSeconds { get; set; } = 2.0;

	[Export(PropertyHint.None, "")]
	public double MeasureSeconds { get; set; } = 4.0;

	[Export(PropertyHint.None, "")]
	public bool PrimeSpawnRoutes { get; set; }

	[Export(PropertyHint.None, "")]
	public double MaxPrimedSpawnMilliseconds { get; set; } = 50.0;

	[Export(PropertyHint.None, "")]
	public string ScreenshotPath { get; set; } = "res://TestResults/bulletfield-workload.png";

	[Export(PropertyHint.None, "")]
	public string RuntimeProfile { get; set; } = "unknown";

	[Export(PropertyHint.None, "")]
	public bool EnableDetailedProfiler { get; set; }

	[Export(PropertyHint.None, "")]
	public string BurstTransitionMode { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public double BurstTransitionBudgetMilliseconds { get; set; } = 250.0;

	[Export(PropertyHint.None, "")]
	public int BenchmarkMaxFps { get; set; } = 180;

	[Export(PropertyHint.None, "")]
	public double MinimumAverageFps { get; set; } = -1.0;

	[Export(PropertyHint.None, "")]
	public double MinimumOnePercentLowFps { get; set; } = -1.0;

	[Export(PropertyHint.None, "")]
	public double MinimumZeroPointOnePercentLowFps { get; set; } = -1.0;

	[Export(PropertyHint.None, "")]
	public int BackgroundContentionThreads { get; set; }

	[Export(PropertyHint.None, "")]
	public bool MixedProjectileDefinitions { get; set; }

	[Export(PropertyHint.None, "")]
	public int MinimumAnimationDefinitionCount { get; set; } = 1;

	public override void _Ready()
	{
		ApplyCommandLineArguments();
		_renderFrameTimes.Clear();
		_processTimesMs.Clear();
		_physicsFrameTimesMs.Clear();
		_physicsUpdateTimesMs.Clear();
		_spawnedIndices.Clear();
		_initialPositions.Clear();
		_spawnedAnimationDefinitionIds.Clear();
		Array.Clear(_workloadVariantSpawnCounts);
		_transitionConfigs.Clear();
		_transitionPreparedChanges.Clear();
		_burstTransitionExecuted = false;
		_measurementStarted = false;
		_firstPublicationMeasured = false;
		_firstPublicationMs = 0.0;
		_firstPublicationVisible = 0;
		_lastRenderTimestamp = Stopwatch.GetTimestamp();
		_originalMaxFps = Engine.MaxFps;
		_originalPhysicsTicks = Engine.PhysicsTicksPerSecond;
		_originalTimeScale = Engine.TimeScale;
		_originalPerfProfilerEnabled = TowerDefensePerfProfiler.Enabled;
		_originalDetailedHotPathMetrics = TowerDefensePerfProfiler.DetailedHotPathMetrics;
		_originalProfilerDumpIntervalFrames = TowerDefensePerfProfiler.DumpIntervalFrames;
		_originalProfilerMaxMetricsPerDump = TowerDefensePerfProfiler.MaxMetricsPerDump;
		if (EnableDetailedProfiler)
		{
			TowerDefensePerfProfiler.Enabled = true;
			TowerDefensePerfProfiler.DetailedHotPathMetrics = true;
			TowerDefensePerfProfiler.DumpIntervalFrames = 60;
			TowerDefensePerfProfiler.MaxMetricsPerDump = 48;
			TowerDefensePerfProfiler.Reset();
		}
		Engine.MaxFps = Math.Max(0, BenchmarkMaxFps);
		Engine.PhysicsTicksPerSecond = 60;
		Engine.TimeScale = 1.0;
		_manager = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(_manager) || !GodotObject.IsInstanceValid(_manager.characterRegistry) || !GodotObject.IsInstanceValid(_manager.targetSystem))
		{
			GD.PrintErr("[BulletFieldWorkloadTest] TowerDefenseManager autoload is not ready.");
			GetTree().Quit(2);
			return;
		}
		_projectileUpdateManager = ProjectileUpdateManager.Instance;
		if (!GodotObject.IsInstanceValid(_projectileUpdateManager))
		{
			GD.PrintErr("[BulletFieldWorkloadTest] ProjectileUpdateManager autoload is not ready.");
			GetTree().Quit(2);
			return;
		}
		_originalProjectileManagerProcessMode = _projectileUpdateManager.ProcessMode;
		_projectileManagerProcessModeCaptured = true;
		_projectileUpdateManager.ProcessMode = ProcessModeEnum.Disabled;
		_originalGridSize = _manager.gridSize;
		_originalGridBegin = _manager.gridBeginPos;
		_originalGridNum = _manager.gridNum;
		Vector2 size = GetViewportRect().Size;
		float num = Math.Max(2f, (size.Y - 285f) / (float)Math.Max(1, ZBucketCount));
		_manager.gridSize = new Vector2(80f, num);
		_manager.gridBeginPos = new Vector2(0f, 285f);
		_manager.gridNum = new Vector2I(64, Math.Max(1, ZBucketCount));
		CreateBackgroundAndLabels();
		_bulletField = new BulletField
		{
			Name = "BulletFieldWorkload"
		};
		AddChild(_bulletField, forceReadableName: false, InternalMode.Disabled);
		LoadConfigsAndReferences();
		if (PrimeSpawnRoutes)
		{
			PrimeRoutes();
		}
		long timestamp = Stopwatch.GetTimestamp();
		SpawnWorkload(size, 285f, num);
		BuildTransitionConfigs();
		_spawnElapsedMs = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
		_screenshotSaved = string.IsNullOrWhiteSpace(ScreenshotPath);
		_ready = _spawnFailures == 0 && _bulletField.ActiveCount == SpawnCount && _configs.Count == 6 && _spawnedAnimationDefinitionIds.Count >= MinimumAnimationDefinitionCount && CountActiveWorkloadVariants() == 6;
		GD.Print($"[BulletFieldWorkloadTest] ready spawn={_bulletField.ActiveCount}/{SpawnCount} failures={_spawnFailures} variants={_configs.Count} requestedZBuckets={ZBucketCount} animationDefinitions={_spawnedAnimationDefinitionIds.Count}/{MinimumAnimationDefinitionCount} mixedDefinitions={MixedProjectileDefinitions} spawnMs={_spawnElapsedMs:F3} workloads=shooter,spin,gravity,fall,catapult,track");
		if (_ready)
		{
			StartContentionThreads();
		}
		else
		{
			GetTree().Quit(2);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_ready || _finished)
		{
			return;
		}
		if (!string.IsNullOrEmpty(BurstTransitionMode) && !_burstTransitionExecuted && _elapsed >= WarmupSeconds)
		{
			RunBurstTransition(delta);
			return;
		}
		bool flag = _elapsed >= WarmupSeconds;
		if (flag)
		{
			StartMeasurementTelemetry();
		}
		long timestamp = Stopwatch.GetTimestamp();
		_bulletField.Update(delta, Engine.GetPhysicsFrames());
		double num = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
		if (!_firstPublicationMeasured)
		{
			_firstPublicationMeasured = true;
			_firstPublicationMs = num;
			_firstPublicationVisible = _bulletField.GetAnimatedMeshVisibleInstanceCountForTest();
		}
		if (flag)
		{
			_physicsUpdateTimesMs.Add(num);
		}
	}

	public override void _Process(double delta)
	{
		long timestamp = Stopwatch.GetTimestamp();
		double num = ((_lastRenderTimestamp > 0) ? ((double)(timestamp - _lastRenderTimestamp) / (double)Stopwatch.Frequency) : delta);
		_lastRenderTimestamp = timestamp;
		if (!_ready || _finished)
		{
			return;
		}
		_elapsed += delta;
		if (string.IsNullOrEmpty(BurstTransitionMode))
		{
			double num2 = Math.Max(0.5, WarmupSeconds - 0.35);
			if (!_screenshotSaved && _elapsed >= num2)
			{
				_screenshotSaved = CaptureScreenshot();
			}
			if (_elapsed >= WarmupSeconds)
			{
				StartMeasurementTelemetry();
				_measuredRenderSeconds += num;
				_renderFrameTimes.Add(num);
				_processTimesMs.Add(Performance.GetMonitor(Performance.Monitor.TimeProcess) * 1000.0);
				_physicsFrameTimesMs.Add(Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess) * 1000.0);
			}
			if (_measuredRenderSeconds >= MeasureSeconds)
			{
				FinishTest();
			}
		}
	}

	private void StartMeasurementTelemetry()
	{
		if (!_measurementStarted)
		{
			_measurementStarted = true;
			_measurementThreadAllocatedBefore = GC.GetAllocatedBytesForCurrentThread();
			_measurementTotalAllocatedBefore = GC.GetTotalAllocatedBytes();
			_measurementGen0Before = GC.CollectionCount(0);
			_measurementGen1Before = GC.CollectionCount(1);
			_measurementGen2Before = GC.CollectionCount(2);
		}
	}

	private void LoadConfigsAndReferences()
	{
		List<PackedScene> list = ResolveCompactProjectileScenes();
		if (list.Count == 0)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			PackedScene packedScene = list[i % list.Count];
			TowerDefenseProjectileConfig item = new TowerDefenseProjectileConfig
			{
				name = $"BulletFieldWorkload{i}",
				projectileScene = packedScene,
				scale = Vector2.One * BulletScale,
				size = new Vector2(28f, 28f),
				collisionFlags = 0,
				fireMethodFlags = 1,
				rotateScale = ((i % 2 == 0) ? 4.0 : 0.0),
				trackSearchInterval = 15
			};
			_configs.Add(item);
			if (packedScene.Instantiate(PackedScene.GenEditState.Disabled) is AdobeAnimateSprite adobeAnimateSprite)
			{
				AddChild(adobeAnimateSprite, forceReadableName: false, InternalMode.Disabled);
				adobeAnimateSprite.SetClip(adobeAnimateSprite.clip);
				adobeAnimateSprite.pause = true;
				adobeAnimateSprite.frameIndex = adobeAnimateSprite.clipRange.X + Math.Min(2, Math.Max(0, adobeAnimateSprite.clipRange.Y - adobeAnimateSprite.clipRange.X - 1));
				adobeAnimateSprite.elapsedTimer = 0.25;
				adobeAnimateSprite.Position = new Vector2(125f + (float)i * 165f, 130f);
				adobeAnimateSprite.Scale = Vector2.One * 0.75f;
				adobeAnimateSprite.ZAsRelative = false;
				adobeAnimateSprite.ZIndex = 4000;
				if (i < list.Count)
				{
					GD.Print($"[BulletFieldWorkloadTest] compactTemplate index={i} " + AnimateMultiMeshRenderer.DescribeCompactCompatibility(adobeAnimateSprite.flashAnimeData));
				}
			}
		}
	}

	private List<PackedScene> ResolveCompactProjectileScenes()
	{
		List<PackedScene> list = new List<PackedScene>();
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 0; i < CompactProjectileSceneCandidates.Length; i++)
		{
			string text = CompactProjectileSceneCandidates[i];
			PackedScene packedScene = GD.Load<PackedScene>(text);
			if (packedScene != null && ProbeCompactScene(packedScene, text, out var definitionId) && hashSet.Add(definitionId))
			{
				list.Add(packedScene);
				GD.Print($"[BulletFieldWorkloadTest] compactTemplate selected={text} definition={definitionId}");
				if (!MixedProjectileDefinitions)
				{
					break;
				}
			}
		}
		if (list.Count < MinimumAnimationDefinitionCount)
		{
			GD.PrintErr($"[BulletFieldWorkloadTest] only {list.Count} unique compact projectile definitions were found; required {MinimumAnimationDefinitionCount}.");
		}
		return list;
	}

	private bool ProbeCompactScene(PackedScene scene, string path, out int definitionId)
	{
		definitionId = -1;
		TowerDefenseProjectileConfig config = new TowerDefenseProjectileConfig
		{
			name = "BulletFieldCompactProbe:" + path,
			projectileScene = scene,
			scale = Vector2.One * BulletScale,
			size = new Vector2(28f, 28f),
			collisionFlags = 0,
			fireMethodFlags = 1
		};
		int num = _bulletField.TrySpawnFromConfig(config, Vector2.Zero, Vector2.Right, 1.0, null, TowerDefenseEnum.CHARACTER_CAMP.PLANT, Vector2I.One, 1, new Rect2(-1000f, -1000f, 2000f, 2000f), null, 0.0, 0.0, 0);
		if (num < 0)
		{
			return false;
		}
		ref BulletData bulletDataRef = ref _bulletField.GetBulletDataRef(num);
		bool flag = bulletDataRef.renderMode == BulletRenderMode.ANIMATED_MESH && bulletDataRef.animDefId >= 0;
		definitionId = (flag ? bulletDataRef.animDefId : (-1));
		_bulletField.Despawn(num);
		GD.Print($"[BulletFieldWorkloadTest] compactProbe path={path} compatible={flag} definition={definitionId}");
		return flag;
	}

	private void SpawnWorkload(Vector2 viewportSize, float stressTop, float rowHeight)
	{
		if (_configs.Count == 0)
		{
			_spawnFailures = SpawnCount;
			return;
		}
		Rect2 rect = new Rect2(new Vector2(-2000000f, -2000000f), new Vector2(4000000f, 4000000f));
		float num = Math.Max(1f, viewportSize.X - 40f);
		for (int i = 0; i < SpawnCount; i++)
		{
			int num2 = 1 + i % Math.Max(1, ZBucketCount);
			float num3 = stressTop + ((float)num2 - 0.5f) * rowHeight;
			float num4 = 20f + (float)(i * 73 % Math.Max(1, (int)num));
			float y = 0f;
			Vector2 vector = new Vector2(24f + (float)(i % 13), y);
			Vector2 pos = new Vector2(num4, num3);
			Vector2I gridPos = new Vector2I(Math.Max(1, Mathf.FloorToInt(num4 / 80f) + 1), num2);
			int num5 = i % 6;
			int index = (MixedProjectileDefinitions ? (i / 6 % _configs.Count) : num5);
			TowerDefenseProjectileConfig config = _configs[index];
			int num6 = _bulletField.TrySpawnFromConfig(config, pos, vector, vector.Length(), null, TowerDefenseEnum.CHARACTER_CAMP.PLANT, gridPos, num2, rect);
			if (num6 < 0)
			{
				_spawnFailures++;
				continue;
			}
			_spawnedIndices.Add(num6);
			ref BulletData bulletDataRef = ref _bulletField.GetBulletDataRef(num6);
			if (bulletDataRef.renderMode != BulletRenderMode.ANIMATED_MESH || bulletDataRef.animDefId < 0)
			{
				_spawnFailures++;
				continue;
			}
			ConfigureWorkloadBullet(ref bulletDataRef, i, num3, vector, rect);
			_spawnedAnimationDefinitionIds.Add(bulletDataRef.animDefId);
			_workloadVariantSpawnCounts[num5]++;
			_initialPositions.Add(bulletDataRef.pos);
		}
	}

	private int CountActiveWorkloadVariants()
	{
		int num = 0;
		for (int i = 0; i < _workloadVariantSpawnCounts.Length; i++)
		{
			if (_workloadVariantSpawnCounts[i] > 0)
			{
				num++;
			}
		}
		return num;
	}

	private void BuildTransitionConfigs()
	{
		if (_configs.Count == 0)
		{
			return;
		}
		for (int i = 0; i < _spawnedIndices.Count; i++)
		{
			ref BulletData bulletDataRef = ref _bulletField.GetBulletDataRef(_spawnedIndices[i]);
			if (!_transitionConfigs.ContainsKey(bulletDataRef.fireMethodFlags))
			{
				TowerDefenseProjectileConfig towerDefenseProjectileConfig = (TowerDefenseProjectileConfig)_configs[0].Duplicate(deep: true);
				towerDefenseProjectileConfig.name = $"BulletFieldTransition{bulletDataRef.fireMethodFlags}";
				towerDefenseProjectileConfig.fireMethodFlags = bulletDataRef.fireMethodFlags;
				towerDefenseProjectileConfig.baseDamage++;
				_transitionConfigs.Add(bulletDataRef.fireMethodFlags, towerDefenseProjectileConfig);
				if (_bulletField.TryPrepareProjectileChange(towerDefenseProjectileConfig, out var prepared))
				{
					_transitionPreparedChanges.Add(bulletDataRef.fireMethodFlags, prepared);
				}
			}
		}
	}

	private void RunBurstTransition(double delta)
	{
		_burstTransitionExecuted = true;
		int num = 0;
		long timestamp = Stopwatch.GetTimestamp();
		if (BurstTransitionMode.Equals("despawn", StringComparison.OrdinalIgnoreCase))
		{
			for (int i = 0; i < _spawnedIndices.Count; i++)
			{
				int index = _spawnedIndices[i];
				if (_bulletField.IsBulletActive(index))
				{
					_bulletField.Despawn(index);
					num++;
				}
			}
		}
		else
		{
			if (!BurstTransitionMode.Equals("change", StringComparison.OrdinalIgnoreCase))
			{
				GD.PrintErr("[BulletFieldBurstTransitionResult] unsupported mode=" + BurstTransitionMode);
				GetTree().Quit(2);
				return;
			}
			for (int j = 0; j < _spawnedIndices.Count; j++)
			{
				int num2 = _spawnedIndices[j];
				if (_bulletField.IsBulletActive(num2))
				{
					ref BulletData bulletDataRef = ref _bulletField.GetBulletDataRef(num2);
					if (_transitionConfigs.TryGetValue(bulletDataRef.fireMethodFlags, out var value) && _transitionPreparedChanges.TryGetValue(bulletDataRef.fireMethodFlags, out var value2) && _bulletField.ChangeBulletDataPrepared(num2, value, null, in value2, value.baseDamage) == num2)
					{
						num++;
					}
				}
			}
		}
		double value3 = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
		long timestamp2 = Stopwatch.GetTimestamp();
		_bulletField.Update(delta, Engine.GetPhysicsFrames());
		double value4 = (double)(Stopwatch.GetTimestamp() - timestamp2) * 1000.0 / (double)Stopwatch.Frequency;
		double num3 = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
		int num4 = ((!BurstTransitionMode.Equals("despawn", StringComparison.OrdinalIgnoreCase)) ? SpawnCount : 0);
		int num5 = num4;
		int animatedMeshVisibleInstanceCountForTest = _bulletField.GetAnimatedMeshVisibleInstanceCountForTest();
		bool flag = num == SpawnCount && _bulletField.ActiveCount == num4 && animatedMeshVisibleInstanceCountForTest == num5 && num3 <= BurstTransitionBudgetMilliseconds;
		GD.Print($"[BulletFieldBurstTransitionResult] mode={BurstTransitionMode} requested={SpawnCount} succeeded={num} active={_bulletField.ActiveCount}/{num4} visible={animatedMeshVisibleInstanceCountForTest}/{num5} transitionMs={value3:F3} updateMs={value4:F3} elapsedMs={num3:F3}/{BurstTransitionBudgetMilliseconds:F3} passed={flag}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private void PrimeRoutes()
	{
		Rect2 rect = new Rect2(-1000f, -1000f, 2000f, 2000f);
		for (int i = 0; i < _configs.Count; i++)
		{
			int num = _bulletField.TrySpawnFromConfig(_configs[i], Vector2.Zero, Vector2.Right, 1.0, null, TowerDefenseEnum.CHARACTER_CAMP.PLANT, Vector2I.One, 1, rect, null, 0.0, 0.0, 0);
			if (num >= 0)
			{
				_bulletField.Despawn(num);
			}
		}
	}

	private static void ConfigureWorkloadBullet(ref BulletData bullet, int index, float visualY, Vector2 velocity, Rect2 worldRect)
	{
		bullet.randFreshIndex = index % 5;
		bullet.rect = worldRect;
		bullet.fireLength = -1;
		bullet.lockGridY = true;
		bullet.flipX = (index & 1) != 0;
		BulletField.SetSpriteRotation(ref bullet, ((index & 0xF) == 0) ? Mathf.DegToRad((float)(index % 5 - 2) * 15f) : 0f);
		if (bullet.animFrameMax > 0)
		{
			bullet.animElapsedTimer = (float)((double)index * 0.61803398875 % (double)bullet.animFrameMax);
		}
		int fireMethodFlags = 1;
		int fireMethodFlags2 = 32;
		switch (index % 6)
		{
		case 0:
			bullet.fireMethodFlags = fireMethodFlags;
			bullet.rotateScale = 0f;
			break;
		case 1:
			bullet.fireMethodFlags = fireMethodFlags;
			bullet.rotateScale = 5f + (float)(index % 4);
			break;
		case 2:
			bullet.fireMethodFlags = fireMethodFlags;
			bullet.useGravity = true;
			bullet.gravity = 20.0;
			bullet.gravityScale = 1f;
			bullet.z = 1000000.0 + (double)(index % 30);
			bullet.ySpeed = -10.0;
			bullet.pos.Y = visualY + (float)bullet.z - 20f;
			bullet.rotateFollowVelocity = true;
			bullet.collisionEnabled = false;
			break;
		case 3:
			bullet.fireMethodFlags = fireMethodFlags;
			bullet.useFall = true;
			bullet.gravity = 20.0;
			bullet.gravityScale = 1f;
			bullet.z = 1000000.0 + (double)(index % 30);
			bullet.ySpeed = -8.0;
			bullet.pos.Y = visualY + (float)bullet.z - 20f;
			bullet.rotateFollowVelocity = true;
			bullet.collisionEnabled = false;
			break;
		case 4:
			bullet.fireMethodFlags = fireMethodFlags;
			bullet.catapultOpen = true;
			bullet.z = 1000000.0;
			bullet.ySpeed = -10000.0;
			bullet.pos.Y = visualY + (float)bullet.z;
			bullet.rotateFollowVelocity = true;
			bullet.collisionEnabled = false;
			break;
		default:
			bullet.fireMethodFlags = fireMethodFlags2;
			bullet.trackOpen = true;
			bullet.trackSearchInterval = 15;
			bullet.trackNoTargetInterval = 60;
			bullet.speed = velocity.Length();
			bullet.rotation = velocity.Angle();
			bullet.checkAll = true;
			break;
		}
	}

	private void CreateBackgroundAndLabels()
	{
		Vector2 size = GetViewportRect().Size;
		ColorRect node = new ColorRect
		{
			Color = new Color(0.025f, 0.055f, 0.04f),
			Size = size,
			MouseFilter = Control.MouseFilterEnum.Ignore,
			ZAsRelative = false,
			ZIndex = -4096
		};
		AddChild(node, forceReadableName: false, InternalMode.Disabled);
		AddLabel("Full Sprite references", new Vector2(18f, 18f));
		AddLabel($"BulletField real workload: {SpawnCount} bullets / {ZBucketCount} requested Z buckets", new Vector2(18f, 245f));
	}

	private void AddLabel(string text, Vector2 position)
	{
		Label label = new Label
		{
			Text = text,
			Position = position,
			ZAsRelative = false,
			ZIndex = 4096
		};
		label.AddThemeFontSizeOverride("font_size", 22);
		AddChild(label, forceReadableName: false, InternalMode.Disabled);
	}

	private bool CaptureScreenshot()
	{
		Image image = GetViewport().GetTexture().GetImage();
		if (image == null || image.IsEmpty())
		{
			return false;
		}
		string text = ScreenshotPath.Replace('\\', '/');
		string text2 = (text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) ? ProjectSettings.GlobalizePath(text) : text);
		Error error = DirAccess.MakeDirRecursiveAbsolute(text2.GetBaseDir());
		if (error != Error.Ok && error != Error.AlreadyExists)
		{
			return false;
		}
		Error error2 = image.SavePng(text2);
		if (error2 == Error.Ok)
		{
			GD.Print($"[BulletFieldWorkloadTest] screenshot saved path={text} size={image.GetWidth()}x{image.GetHeight()}");
		}
		return error2 == Error.Ok;
	}

	private void FinishTest()
	{
		_finished = true;
		StopContentionThreads();
		long num = (_measurementStarted ? (GC.GetAllocatedBytesForCurrentThread() - _measurementThreadAllocatedBefore) : 0);
		long num2 = (_measurementStarted ? (GC.GetTotalAllocatedBytes() - _measurementTotalAllocatedBefore) : 0);
		int value = (_measurementStarted ? (GC.CollectionCount(0) - _measurementGen0Before) : 0);
		int value2 = (_measurementStarted ? (GC.CollectionCount(1) - _measurementGen1Before) : 0);
		int value3 = (_measurementStarted ? (GC.CollectionCount(2) - _measurementGen2Before) : 0);
		double[] array = _renderFrameTimes.ToArray();
		double[] array2 = _processTimesMs.ToArray();
		double[] array3 = _physicsFrameTimesMs.ToArray();
		double[] array4 = _physicsUpdateTimesMs.ToArray();
		double num3 = (double)_renderFrameTimes.Count / Math.Max(0.0001, _measuredRenderSeconds);
		double num4 = BenchmarkStatistics.WorstFractionFps(array, array.Length, 0.01);
		double num5 = BenchmarkStatistics.WorstFractionFps(array, array.Length, 0.001);
		double num6 = BenchmarkStatistics.Percentile(array, array.Length, 50.0) * 1000.0;
		double num7 = BenchmarkStatistics.Percentile(array, array.Length, 95.0) * 1000.0;
		double num8 = BenchmarkStatistics.Percentile(array, array.Length, 99.0) * 1000.0;
		double num9 = BenchmarkStatistics.Maximum(array, array.Length) * 1000.0;
		double value4 = BenchmarkStatistics.Percentile(array2, array2.Length, 50.0);
		double value5 = BenchmarkStatistics.Percentile(array2, array2.Length, 95.0);
		double value6 = BenchmarkStatistics.Percentile(array2, array2.Length, 99.0);
		double value7 = BenchmarkStatistics.Maximum(array2, array2.Length);
		double value8 = BenchmarkStatistics.Percentile(array3, array3.Length, 50.0);
		double value9 = BenchmarkStatistics.Percentile(array3, array3.Length, 95.0);
		double value10 = BenchmarkStatistics.Percentile(array3, array3.Length, 99.0);
		double value11 = BenchmarkStatistics.Maximum(array3, array3.Length);
		double value12 = BenchmarkStatistics.Average(array4, array4.Length);
		double value13 = BenchmarkStatistics.Percentile(array4, array4.Length, 50.0);
		double value14 = BenchmarkStatistics.Percentile(array4, array4.Length, 95.0);
		double value15 = BenchmarkStatistics.Percentile(array4, array4.Length, 99.0);
		double value16 = BenchmarkStatistics.Maximum(array4, array4.Length);
		double monitor = Performance.GetMonitor(Performance.Monitor.MemoryStatic);
		double monitor2 = Performance.GetMonitor(Performance.Monitor.MemoryStaticMax);
		double monitor3 = Performance.GetMonitor(Performance.Monitor.MemoryMessageBufferMax);
		double monitor4 = Performance.GetMonitor(Performance.Monitor.RenderVideoMemUsed);
		double monitor5 = Performance.GetMonitor(Performance.Monitor.RenderTextureMemUsed);
		double monitor6 = Performance.GetMonitor(Performance.Monitor.RenderBufferMemUsed);
		double monitor7 = Performance.GetMonitor(Performance.Monitor.ObjectCount);
		double monitor8 = Performance.GetMonitor(Performance.Monitor.ObjectNodeCount);
		double monitor9 = Performance.GetMonitor(Performance.Monitor.ObjectOrphanNodeCount);
		double monitor10 = Performance.GetMonitor(Performance.Monitor.ObjectResourceCount);
		double monitor11 = Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame);
		double monitor12 = Performance.GetMonitor(Performance.Monitor.RenderTotalObjectsInFrame);
		double monitor13 = Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame);
		AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot statistics = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.Statistics;
		bool flag = statistics.AppliedUploadCount > 0 && statistics.UploadedByteCount > 0 && statistics.FailedBatchCount == 0L && statistics.BufferUpdateFailureCount == 0L && statistics.InvalidTargetCount == 0L && statistics.RenderingDeviceUnavailableCount == 0;
		StringName value17 = RenderingServer.GetCurrentRenderingDriverName();
		StringName value18 = RenderingServer.GetCurrentRenderingMethod();
		int animatedMeshVisibleInstanceCountForTest = _bulletField.GetAnimatedMeshVisibleInstanceCountForTest();
		int animatedMeshBucketCountForTest = _bulletField.GetAnimatedMeshBucketCountForTest();
		int num10 = CountActiveWorkloadVariants();
		long num11 = 0L;
		for (int i = 0; i < _contentionIterations.Length; i++)
		{
			num11 += _contentionIterations[i];
		}
		int num12 = 0;
		int num13 = Math.Min(_spawnedIndices.Count, _initialPositions.Count);
		for (int j = 0; j < num13; j++)
		{
			int index = _spawnedIndices[j];
			if (_bulletField.IsBulletActive(index) && _bulletField.GetBulletDataRef(index).pos.DistanceSquaredTo(_initialPositions[j]) > 0.0001f)
			{
				num12++;
			}
		}
		double num14;
		if (MinimumAverageFps >= 0.0)
		{
			num14 = MinimumAverageFps;
		}
		else if (SpawnCount >= 60000)
		{
			num14 = 59.0;
		}
		else
		{
			num14 = ((SpawnCount <= 10000) ? 175.0 : 0.0);
		}
		double num15 = Math.Max(0.0, MinimumOnePercentLowFps);
		double num16 = Math.Max(0.0, MinimumZeroPointOnePercentLowFps);
		double num17 = MaxPrimedSpawnMilliseconds * Math.Max(1.0, (double)SpawnCount / 10000.0);
		bool flag2 = !PrimeSpawnRoutes || _spawnElapsedMs <= num17;
		bool flag3 = _measurementStarted && array.Length != 0 && array2.Length == array.Length && array3.Length == array.Length && array4.Length != 0 && double.IsFinite(num6) && double.IsFinite(num7) && double.IsFinite(num8) && double.IsFinite(num9) && num >= 0 && num2 >= 0;
		bool flag4 = ((_screenshotSaved && _spawnFailures == 0 && _bulletField.ActiveCount == SpawnCount && animatedMeshVisibleInstanceCountForTest == SpawnCount && num12 == SpawnCount && _spawnedAnimationDefinitionIds.Count >= MinimumAnimationDefinitionCount && num10 == 6 && _firstPublicationMeasured && _firstPublicationVisible == SpawnCount && animatedMeshBucketCountForTest > 0) & flag & flag2 & flag3) && num3 >= num14 && num4 >= num15 && num5 >= num16;
		GD.Print($"[BulletFieldWorkloadTestResult] runtimeProfile={RuntimeProfile} spawn={SpawnCount} active={_bulletField.ActiveCount} visible={animatedMeshVisibleInstanceCountForTest}/{SpawnCount} moved={num12}/{SpawnCount} buckets={animatedMeshBucketCountForTest} renderFrames={_renderFrameTimes.Count} animationDefinitions={_spawnedAnimationDefinitionIds.Count}/{MinimumAnimationDefinitionCount} mixedDefinitions={MixedProjectileDefinitions} firingForms={num10}/{6} variantCounts={string.Join(',', _workloadVariantSpawnCounts)} workers={_bulletField.GetBackgroundWorkerCountForTest()} contentionThreads={BackgroundContentionThreads} contentionIterations={num11} maxFps={BenchmarkMaxFps} minimumAverageFps={num14:F2} minimumOnePercentLowFps={num15:F2} minimumZeroPointOnePercentLowFps={num16:F2} averageFps={num3:F2} onePercentLowFps={num4:F2} zeroPointOnePercentLowFps={num5:F2} frameP50Ms={num6:F3} frameP95Ms={num7:F3} frameP99Ms={num8:F3} frameMaxMs={num9:F3} processP50Ms={value4:F3} processP95Ms={value5:F3} processP99Ms={value6:F3} processMaxMs={value7:F3} physicsP50Ms={value8:F3} physicsP95Ms={value9:F3} physicsP99Ms={value10:F3} physicsMaxMs={value11:F3} spawnMs={_spawnElapsedMs:F3}/{num17:F3} spawnPassed={flag2} firstPublicationMs={_firstPublicationMs:F3} firstPublicationVisible={_firstPublicationVisible} fieldPhysicsAverageMs={value12:F3} fieldPhysicsP50Ms={value13:F3} fieldPhysicsP95Ms={value14:F3} fieldPhysicsP99Ms={value15:F3} fieldPhysicsMaxMs={value16:F3} staticMemoryBytes={monitor:F0} staticMemoryPeakBytes={monitor2:F0} messageBufferPeakBytes={monitor3:F0} videoMemoryBytes={monitor4:F0} textureMemoryBytes={monitor5:F0} bufferMemoryBytes={monitor6:F0} objectCount={monitor7:F0} nodeCount={monitor8:F0} orphanNodeCount={monitor9:F0} resourceCount={monitor10:F0} drawCalls={monitor11:F0} renderObjects={monitor12:F0} renderPrimitives={monitor13:F0} rdBatches={statistics.AppliedBatchCount}/{statistics.SubmittedBatchCount} rdUploads={statistics.AppliedUploadCount}/{statistics.QueuedUploadCount} rdUploadedBytes={statistics.UploadedByteCount} rdRidRefreshes={statistics.BufferRidRefreshCount} rdFailures={statistics.FailedBatchCount}/{statistics.BufferUpdateFailureCount}/{statistics.InvalidTargetCount}/{statistics.RenderingDeviceUnavailableCount} rdStaleDrops={statistics.StaleGenerationDropCount} rdQueueDepth={statistics.CurrentQueueDepth}/{statistics.MaximumQueueDepth} rdUploadPassed={flag} threadAllocatedBytes={num} totalAllocatedBytes={num2} gen0={value} gen1={value2} gen2={value3} telemetryPassed={flag3} driver={value17} renderer={value18} screenshotSaved={_screenshotSaved} failures={_spawnFailures} passed={flag4}");
		GetTree().Quit((!flag4) ? 1 : 0);
	}

	private void ApplyCommandLineArguments()
	{
		string[] cmdlineUserArgs = OS.GetCmdlineUserArgs();
		foreach (string text in cmdlineUserArgs)
		{
			int value2;
			double value3;
			double value4;
			int value5;
			double value6;
			int value7;
			double value8;
			double value9;
			double value10;
			int value11;
			if (TryReadInt(text, "--bullet-workload-spawn=", out var value))
			{
				SpawnCount = Math.Max(0, value);
			}
			else if (TryReadInt(text, "--bullet-workload-z-buckets=", out value2))
			{
				ZBucketCount = Math.Max(1, value2);
			}
			else if (TryReadDouble(text, "--bullet-workload-warmup=", out value3))
			{
				WarmupSeconds = Math.Max(0.0, value3);
			}
			else if (TryReadDouble(text, "--bullet-workload-measure=", out value4))
			{
				MeasureSeconds = Math.Max(0.1, value4);
			}
			else if (text.StartsWith("--runtime-profile=", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = text;
				int length = "--runtime-profile=".Length;
				RuntimeProfile = text2.Substring(length, text2.Length - length).Trim();
			}
			else if (text.StartsWith("--bullet-workload-screenshot=", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = text;
				int length = "--bullet-workload-screenshot=".Length;
				ScreenshotPath = text2.Substring(length, text2.Length - length);
			}
			else if (text.Equals("--bullet-workload-prime-routes", StringComparison.OrdinalIgnoreCase))
			{
				PrimeSpawnRoutes = true;
			}
			else if (text.Equals("--bullet-workload-detailed-profile", StringComparison.OrdinalIgnoreCase))
			{
				EnableDetailedProfiler = true;
			}
			else if (text.Equals("--bullet-workload-mixed-definitions", StringComparison.OrdinalIgnoreCase))
			{
				MixedProjectileDefinitions = true;
			}
			else if (TryReadInt(text, "--bullet-workload-minimum-animation-definitions=", out value5))
			{
				MinimumAnimationDefinitionCount = Math.Max(1, value5);
			}
			else if (text.StartsWith("--bullet-workload-transition=", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = text;
				int length = "--bullet-workload-transition=".Length;
				BurstTransitionMode = text2.Substring(length, text2.Length - length).Trim();
			}
			else if (TryReadDouble(text, "--bullet-workload-transition-budget-ms=", out value6))
			{
				BurstTransitionBudgetMilliseconds = Math.Max(0.1, value6);
			}
			else if (TryReadInt(text, "--bullet-workload-max-fps=", out value7))
			{
				BenchmarkMaxFps = Math.Max(0, value7);
			}
			else if (TryReadDouble(text, "--bullet-workload-minimum-fps=", out value8))
			{
				MinimumAverageFps = Math.Max(0.0, value8);
			}
			else if (TryReadDouble(text, "--bullet-workload-minimum-1pct-low=", out value9))
			{
				MinimumOnePercentLowFps = Math.Max(0.0, value9);
			}
			else if (TryReadDouble(text, "--bullet-workload-minimum-0.1pct-low=", out value10))
			{
				MinimumZeroPointOnePercentLowFps = Math.Max(0.0, value10);
			}
			else if (TryReadInt(text, "--bullet-workload-contention-threads=", out value11))
			{
				BackgroundContentionThreads = Math.Clamp(value11, 0, 32);
			}
		}
		if (MixedProjectileDefinitions)
		{
			MinimumAnimationDefinitionCount = Math.Max(3, MinimumAnimationDefinitionCount);
		}
	}

	private void StartContentionThreads()
	{
		if (BackgroundContentionThreads <= 0)
		{
			return;
		}
		Volatile.Write(ref _stopContentionThreads, 0);
		_contentionThreads = new Thread[BackgroundContentionThreads];
		_contentionIterations = new long[BackgroundContentionThreads];
		for (int i = 0; i < _contentionThreads.Length; i++)
		{
			int workerIndex = i;
			Thread thread = new Thread(() =>
			{
				RunContentionWorker(workerIndex);
			})
			{
				IsBackground = true,
				Name = $"BulletFieldContention{workerIndex + 1}"
			};
			_contentionThreads[i] = thread;
			thread.Start();
		}
	}

	private void RunContentionWorker(int workerIndex)
	{
		double num = (double)workerIndex + 1.0;
		long num2 = 0L;
		while (Volatile.Read(in _stopContentionThreads) == 0)
		{
			for (int i = 0; i < 256; i++)
			{
				num = num * 1.0000001192092896 + 9.536743164E-07;
			}
			num2++;
			Thread.SpinWait(32);
		}
		_contentionIterations[workerIndex] = num2;
		GC.KeepAlive(num);
	}

	private void StopContentionThreads()
	{
		if (_contentionThreads.Length != 0)
		{
			Volatile.Write(ref _stopContentionThreads, 1);
			for (int i = 0; i < _contentionThreads.Length; i++)
			{
				_contentionThreads[i]?.Join();
			}
			_contentionThreads = Array.Empty<Thread>();
		}
	}

	private static bool TryReadInt(string arg, string prefix, out int value)
	{
		value = 0;
		if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			int length = prefix.Length;
			return int.TryParse(arg.Substring(length, arg.Length - length), out value);
		}
		return false;
	}

	private static bool TryReadDouble(string arg, string prefix, out double value)
	{
		value = 0.0;
		if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			int length = prefix.Length;
			return double.TryParse(arg.Substring(length, arg.Length - length), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
		}
		return false;
	}

	public override void _ExitTree()
	{
		StopContentionThreads();
		if (GodotObject.IsInstanceValid(_manager))
		{
			_manager.gridSize = _originalGridSize;
			_manager.gridBeginPos = _originalGridBegin;
			_manager.gridNum = _originalGridNum;
		}
		Engine.MaxFps = _originalMaxFps;
		Engine.PhysicsTicksPerSecond = _originalPhysicsTicks;
		Engine.TimeScale = _originalTimeScale;
		if (_projectileManagerProcessModeCaptured && GodotObject.IsInstanceValid(_projectileUpdateManager))
		{
			_projectileUpdateManager.ProcessMode = _originalProjectileManagerProcessMode;
		}
		TowerDefensePerfProfiler.Enabled = _originalPerfProfilerEnabled;
		TowerDefensePerfProfiler.DetailedHotPathMetrics = _originalDetailedHotPathMetrics;
		TowerDefensePerfProfiler.DumpIntervalFrames = _originalProfilerDumpIntervalFrames;
		TowerDefensePerfProfiler.MaxMetricsPerDump = _originalProfilerMaxMetricsPerDump;
		TowerDefensePerfProfiler.Reset();
		base._ExitTree();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartMeasurementTelemetry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadConfigsAndReferences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnWorkload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "viewportSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "stressTop", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "rowHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountActiveWorkloadVariants, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildTransitionConfigs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunBurstTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrimeRoutes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateBackgroundAndLabels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureScreenshot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinishTest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyCommandLineArguments, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartContentionThreads, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunContentionWorker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "workerIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StopContentionThreads, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartMeasurementTelemetry && args.Count == 0)
		{
			StartMeasurementTelemetry();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadConfigsAndReferences && args.Count == 0)
		{
			LoadConfigsAndReferences();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnWorkload && args.Count == 3)
		{
			SpawnWorkload(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountActiveWorkloadVariants && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountActiveWorkloadVariants());
			return true;
		}
		if (method == MethodName.BuildTransitionConfigs && args.Count == 0)
		{
			BuildTransitionConfigs();
			ret = default;
			return true;
		}
		if (method == MethodName.RunBurstTransition && args.Count == 1)
		{
			RunBurstTransition(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrimeRoutes && args.Count == 0)
		{
			PrimeRoutes();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateBackgroundAndLabels && args.Count == 0)
		{
			CreateBackgroundAndLabels();
			ret = default;
			return true;
		}
		if (method == MethodName.AddLabel && args.Count == 2)
		{
			AddLabel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureScreenshot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CaptureScreenshot());
			return true;
		}
		if (method == MethodName.FinishTest && args.Count == 0)
		{
			FinishTest();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCommandLineArguments && args.Count == 0)
		{
			ApplyCommandLineArguments();
			ret = default;
			return true;
		}
		if (method == MethodName.StartContentionThreads && args.Count == 0)
		{
			StartContentionThreads();
			ret = default;
			return true;
		}
		if (method == MethodName.RunContentionWorker && args.Count == 1)
		{
			RunContentionWorker(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StopContentionThreads && args.Count == 0)
		{
			StopContentionThreads();
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.StartMeasurementTelemetry)
		{
			return true;
		}
		if (method == MethodName.LoadConfigsAndReferences)
		{
			return true;
		}
		if (method == MethodName.SpawnWorkload)
		{
			return true;
		}
		if (method == MethodName.CountActiveWorkloadVariants)
		{
			return true;
		}
		if (method == MethodName.BuildTransitionConfigs)
		{
			return true;
		}
		if (method == MethodName.RunBurstTransition)
		{
			return true;
		}
		if (method == MethodName.PrimeRoutes)
		{
			return true;
		}
		if (method == MethodName.CreateBackgroundAndLabels)
		{
			return true;
		}
		if (method == MethodName.AddLabel)
		{
			return true;
		}
		if (method == MethodName.CaptureScreenshot)
		{
			return true;
		}
		if (method == MethodName.FinishTest)
		{
			return true;
		}
		if (method == MethodName.ApplyCommandLineArguments)
		{
			return true;
		}
		if (method == MethodName.StartContentionThreads)
		{
			return true;
		}
		if (method == MethodName.RunContentionWorker)
		{
			return true;
		}
		if (method == MethodName.StopContentionThreads)
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
		if (name == PropertyName.SpawnCount)
		{
			SpawnCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ZBucketCount)
		{
			ZBucketCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.BulletScale)
		{
			BulletScale = VariantUtils.ConvertTo<float>(in value);
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
		if (name == PropertyName.PrimeSpawnRoutes)
		{
			PrimeSpawnRoutes = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.MaxPrimedSpawnMilliseconds)
		{
			MaxPrimedSpawnMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.ScreenshotPath)
		{
			ScreenshotPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeProfile)
		{
			RuntimeProfile = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.EnableDetailedProfiler)
		{
			EnableDetailedProfiler = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.BurstTransitionMode)
		{
			BurstTransitionMode = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.BurstTransitionBudgetMilliseconds)
		{
			BurstTransitionBudgetMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.BenchmarkMaxFps)
		{
			BenchmarkMaxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.MinimumAverageFps)
		{
			MinimumAverageFps = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.MinimumOnePercentLowFps)
		{
			MinimumOnePercentLowFps = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.MinimumZeroPointOnePercentLowFps)
		{
			MinimumZeroPointOnePercentLowFps = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.BackgroundContentionThreads)
		{
			BackgroundContentionThreads = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.MixedProjectileDefinitions)
		{
			MixedProjectileDefinitions = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.MinimumAnimationDefinitionCount)
		{
			MinimumAnimationDefinitionCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._bulletField)
		{
			_bulletField = VariantUtils.ConvertTo<BulletField>(in value);
			return true;
		}
		if (name == PropertyName._manager)
		{
			_manager = VariantUtils.ConvertTo<TowerDefenseManager>(in value);
			return true;
		}
		if (name == PropertyName._originalGridSize)
		{
			_originalGridSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._originalGridBegin)
		{
			_originalGridBegin = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._originalGridNum)
		{
			_originalGridNum = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._originalMaxFps)
		{
			_originalMaxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._originalPhysicsTicks)
		{
			_originalPhysicsTicks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._originalTimeScale)
		{
			_originalTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._projectileUpdateManager)
		{
			_projectileUpdateManager = VariantUtils.ConvertTo<ProjectileUpdateManager>(in value);
			return true;
		}
		if (name == PropertyName._originalProjectileManagerProcessMode)
		{
			_originalProjectileManagerProcessMode = VariantUtils.ConvertTo<ProcessModeEnum>(in value);
			return true;
		}
		if (name == PropertyName._projectileManagerProcessModeCaptured)
		{
			_projectileManagerProcessModeCaptured = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._originalPerfProfilerEnabled)
		{
			_originalPerfProfilerEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._originalDetailedHotPathMetrics)
		{
			_originalDetailedHotPathMetrics = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._originalProfilerDumpIntervalFrames)
		{
			_originalProfilerDumpIntervalFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._originalProfilerMaxMetricsPerDump)
		{
			_originalProfilerMaxMetricsPerDump = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._elapsed)
		{
			_elapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._measuredRenderSeconds)
		{
			_measuredRenderSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._spawnFailures)
		{
			_spawnFailures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._spawnElapsedMs)
		{
			_spawnElapsedMs = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._ready)
		{
			_ready = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._screenshotSaved)
		{
			_screenshotSaved = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._finished)
		{
			_finished = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._measurementStarted)
		{
			_measurementStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._measurementThreadAllocatedBefore)
		{
			_measurementThreadAllocatedBefore = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._measurementTotalAllocatedBefore)
		{
			_measurementTotalAllocatedBefore = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._measurementGen0Before)
		{
			_measurementGen0Before = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._measurementGen1Before)
		{
			_measurementGen1Before = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._measurementGen2Before)
		{
			_measurementGen2Before = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastRenderTimestamp)
		{
			_lastRenderTimestamp = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._firstPublicationMeasured)
		{
			_firstPublicationMeasured = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._firstPublicationMs)
		{
			_firstPublicationMs = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._firstPublicationVisible)
		{
			_firstPublicationVisible = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._burstTransitionExecuted)
		{
			_burstTransitionExecuted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._contentionIterations)
		{
			_contentionIterations = VariantUtils.ConvertTo<long[]>(in value);
			return true;
		}
		if (name == PropertyName._stopContentionThreads)
		{
			_stopContentionThreads = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.SpawnCount)
		{
			from = SpawnCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ZBucketCount)
		{
			from = ZBucketCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.BulletScale)
		{
			value = VariantUtils.CreateFrom<float>(BulletScale);
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
		bool from3;
		if (name == PropertyName.PrimeSpawnRoutes)
		{
			from3 = PrimeSpawnRoutes;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.MaxPrimedSpawnMilliseconds)
		{
			from2 = MaxPrimedSpawnMilliseconds;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		string from4;
		if (name == PropertyName.ScreenshotPath)
		{
			from4 = ScreenshotPath;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.RuntimeProfile)
		{
			from4 = RuntimeProfile;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.EnableDetailedProfiler)
		{
			from3 = EnableDetailedProfiler;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.BurstTransitionMode)
		{
			from4 = BurstTransitionMode;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.BurstTransitionBudgetMilliseconds)
		{
			from2 = BurstTransitionBudgetMilliseconds;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.BenchmarkMaxFps)
		{
			from = BenchmarkMaxFps;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MinimumAverageFps)
		{
			from2 = MinimumAverageFps;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.MinimumOnePercentLowFps)
		{
			from2 = MinimumOnePercentLowFps;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.MinimumZeroPointOnePercentLowFps)
		{
			from2 = MinimumZeroPointOnePercentLowFps;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.BackgroundContentionThreads)
		{
			from = BackgroundContentionThreads;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MixedProjectileDefinitions)
		{
			from3 = MixedProjectileDefinitions;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.MinimumAnimationDefinitionCount)
		{
			from = MinimumAnimationDefinitionCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._workloadVariantSpawnCounts)
		{
			value = VariantUtils.CreateFrom(in _workloadVariantSpawnCounts);
			return true;
		}
		if (name == PropertyName._bulletField)
		{
			value = VariantUtils.CreateFrom(in _bulletField);
			return true;
		}
		if (name == PropertyName._manager)
		{
			value = VariantUtils.CreateFrom(in _manager);
			return true;
		}
		if (name == PropertyName._originalGridSize)
		{
			value = VariantUtils.CreateFrom(in _originalGridSize);
			return true;
		}
		if (name == PropertyName._originalGridBegin)
		{
			value = VariantUtils.CreateFrom(in _originalGridBegin);
			return true;
		}
		if (name == PropertyName._originalGridNum)
		{
			value = VariantUtils.CreateFrom(in _originalGridNum);
			return true;
		}
		if (name == PropertyName._originalMaxFps)
		{
			value = VariantUtils.CreateFrom(in _originalMaxFps);
			return true;
		}
		if (name == PropertyName._originalPhysicsTicks)
		{
			value = VariantUtils.CreateFrom(in _originalPhysicsTicks);
			return true;
		}
		if (name == PropertyName._originalTimeScale)
		{
			value = VariantUtils.CreateFrom(in _originalTimeScale);
			return true;
		}
		if (name == PropertyName._projectileUpdateManager)
		{
			value = VariantUtils.CreateFrom(in _projectileUpdateManager);
			return true;
		}
		if (name == PropertyName._originalProjectileManagerProcessMode)
		{
			value = VariantUtils.CreateFrom(in _originalProjectileManagerProcessMode);
			return true;
		}
		if (name == PropertyName._projectileManagerProcessModeCaptured)
		{
			value = VariantUtils.CreateFrom(in _projectileManagerProcessModeCaptured);
			return true;
		}
		if (name == PropertyName._originalPerfProfilerEnabled)
		{
			value = VariantUtils.CreateFrom(in _originalPerfProfilerEnabled);
			return true;
		}
		if (name == PropertyName._originalDetailedHotPathMetrics)
		{
			value = VariantUtils.CreateFrom(in _originalDetailedHotPathMetrics);
			return true;
		}
		if (name == PropertyName._originalProfilerDumpIntervalFrames)
		{
			value = VariantUtils.CreateFrom(in _originalProfilerDumpIntervalFrames);
			return true;
		}
		if (name == PropertyName._originalProfilerMaxMetricsPerDump)
		{
			value = VariantUtils.CreateFrom(in _originalProfilerMaxMetricsPerDump);
			return true;
		}
		if (name == PropertyName._elapsed)
		{
			value = VariantUtils.CreateFrom(in _elapsed);
			return true;
		}
		if (name == PropertyName._measuredRenderSeconds)
		{
			value = VariantUtils.CreateFrom(in _measuredRenderSeconds);
			return true;
		}
		if (name == PropertyName._spawnFailures)
		{
			value = VariantUtils.CreateFrom(in _spawnFailures);
			return true;
		}
		if (name == PropertyName._spawnElapsedMs)
		{
			value = VariantUtils.CreateFrom(in _spawnElapsedMs);
			return true;
		}
		if (name == PropertyName._ready)
		{
			value = VariantUtils.CreateFrom(in _ready);
			return true;
		}
		if (name == PropertyName._screenshotSaved)
		{
			value = VariantUtils.CreateFrom(in _screenshotSaved);
			return true;
		}
		if (name == PropertyName._finished)
		{
			value = VariantUtils.CreateFrom(in _finished);
			return true;
		}
		if (name == PropertyName._measurementStarted)
		{
			value = VariantUtils.CreateFrom(in _measurementStarted);
			return true;
		}
		if (name == PropertyName._measurementThreadAllocatedBefore)
		{
			value = VariantUtils.CreateFrom(in _measurementThreadAllocatedBefore);
			return true;
		}
		if (name == PropertyName._measurementTotalAllocatedBefore)
		{
			value = VariantUtils.CreateFrom(in _measurementTotalAllocatedBefore);
			return true;
		}
		if (name == PropertyName._measurementGen0Before)
		{
			value = VariantUtils.CreateFrom(in _measurementGen0Before);
			return true;
		}
		if (name == PropertyName._measurementGen1Before)
		{
			value = VariantUtils.CreateFrom(in _measurementGen1Before);
			return true;
		}
		if (name == PropertyName._measurementGen2Before)
		{
			value = VariantUtils.CreateFrom(in _measurementGen2Before);
			return true;
		}
		if (name == PropertyName._lastRenderTimestamp)
		{
			value = VariantUtils.CreateFrom(in _lastRenderTimestamp);
			return true;
		}
		if (name == PropertyName._firstPublicationMeasured)
		{
			value = VariantUtils.CreateFrom(in _firstPublicationMeasured);
			return true;
		}
		if (name == PropertyName._firstPublicationMs)
		{
			value = VariantUtils.CreateFrom(in _firstPublicationMs);
			return true;
		}
		if (name == PropertyName._firstPublicationVisible)
		{
			value = VariantUtils.CreateFrom(in _firstPublicationVisible);
			return true;
		}
		if (name == PropertyName._burstTransitionExecuted)
		{
			value = VariantUtils.CreateFrom(in _burstTransitionExecuted);
			return true;
		}
		if (name == PropertyName._contentionIterations)
		{
			value = VariantUtils.CreateFrom(in _contentionIterations);
			return true;
		}
		if (name == PropertyName._stopContentionThreads)
		{
			value = VariantUtils.CreateFrom(in _stopContentionThreads);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.SpawnCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ZBucketCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.BulletScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.WarmupSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MeasureSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.PrimeSpawnRoutes, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MaxPrimedSpawnMilliseconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ScreenshotPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.RuntimeProfile, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableDetailedProfiler, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.BurstTransitionMode, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.BurstTransitionBudgetMilliseconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.BenchmarkMaxFps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MinimumAverageFps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MinimumOnePercentLowFps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MinimumZeroPointOnePercentLowFps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.BackgroundContentionThreads, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.MixedProjectileDefinitions, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.MinimumAnimationDefinitionCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._workloadVariantSpawnCounts, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bulletField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._manager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._originalGridSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._originalGridBegin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._originalGridNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalMaxFps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalPhysicsTicks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._originalTimeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileUpdateManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalProjectileManagerProcessMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._projectileManagerProcessModeCaptured, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._originalPerfProfilerEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._originalDetailedHotPathMetrics, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalProfilerDumpIntervalFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalProfilerMaxMetricsPerDump, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._elapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._measuredRenderSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._spawnFailures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._spawnElapsedMs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ready, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._screenshotSaved, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._finished, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._measurementStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementThreadAllocatedBefore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementTotalAllocatedBefore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementGen0Before, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementGen1Before, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementGen2Before, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastRenderTimestamp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._firstPublicationMeasured, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._firstPublicationMs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._firstPublicationVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._burstTransitionExecuted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt64Array, PropertyName._contentionIterations, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._stopContentionThreads, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.SpawnCount, Variant.From<int>(SpawnCount));
		info.AddProperty(PropertyName.ZBucketCount, Variant.From<int>(ZBucketCount));
		info.AddProperty(PropertyName.BulletScale, Variant.From<float>(BulletScale));
		info.AddProperty(PropertyName.WarmupSeconds, Variant.From<double>(WarmupSeconds));
		info.AddProperty(PropertyName.MeasureSeconds, Variant.From<double>(MeasureSeconds));
		info.AddProperty(PropertyName.PrimeSpawnRoutes, Variant.From<bool>(PrimeSpawnRoutes));
		info.AddProperty(PropertyName.MaxPrimedSpawnMilliseconds, Variant.From<double>(MaxPrimedSpawnMilliseconds));
		info.AddProperty(PropertyName.ScreenshotPath, Variant.From<string>(ScreenshotPath));
		info.AddProperty(PropertyName.RuntimeProfile, Variant.From<string>(RuntimeProfile));
		info.AddProperty(PropertyName.EnableDetailedProfiler, Variant.From<bool>(EnableDetailedProfiler));
		info.AddProperty(PropertyName.BurstTransitionMode, Variant.From<string>(BurstTransitionMode));
		info.AddProperty(PropertyName.BurstTransitionBudgetMilliseconds, Variant.From<double>(BurstTransitionBudgetMilliseconds));
		info.AddProperty(PropertyName.BenchmarkMaxFps, Variant.From<int>(BenchmarkMaxFps));
		info.AddProperty(PropertyName.MinimumAverageFps, Variant.From<double>(MinimumAverageFps));
		info.AddProperty(PropertyName.MinimumOnePercentLowFps, Variant.From<double>(MinimumOnePercentLowFps));
		info.AddProperty(PropertyName.MinimumZeroPointOnePercentLowFps, Variant.From<double>(MinimumZeroPointOnePercentLowFps));
		info.AddProperty(PropertyName.BackgroundContentionThreads, Variant.From<int>(BackgroundContentionThreads));
		info.AddProperty(PropertyName.MixedProjectileDefinitions, Variant.From<bool>(MixedProjectileDefinitions));
		info.AddProperty(PropertyName.MinimumAnimationDefinitionCount, Variant.From<int>(MinimumAnimationDefinitionCount));
		info.AddProperty(PropertyName._bulletField, Variant.From(in _bulletField));
		info.AddProperty(PropertyName._manager, Variant.From(in _manager));
		info.AddProperty(PropertyName._originalGridSize, Variant.From(in _originalGridSize));
		info.AddProperty(PropertyName._originalGridBegin, Variant.From(in _originalGridBegin));
		info.AddProperty(PropertyName._originalGridNum, Variant.From(in _originalGridNum));
		info.AddProperty(PropertyName._originalMaxFps, Variant.From(in _originalMaxFps));
		info.AddProperty(PropertyName._originalPhysicsTicks, Variant.From(in _originalPhysicsTicks));
		info.AddProperty(PropertyName._originalTimeScale, Variant.From(in _originalTimeScale));
		info.AddProperty(PropertyName._projectileUpdateManager, Variant.From(in _projectileUpdateManager));
		info.AddProperty(PropertyName._originalProjectileManagerProcessMode, Variant.From(in _originalProjectileManagerProcessMode));
		info.AddProperty(PropertyName._projectileManagerProcessModeCaptured, Variant.From(in _projectileManagerProcessModeCaptured));
		info.AddProperty(PropertyName._originalPerfProfilerEnabled, Variant.From(in _originalPerfProfilerEnabled));
		info.AddProperty(PropertyName._originalDetailedHotPathMetrics, Variant.From(in _originalDetailedHotPathMetrics));
		info.AddProperty(PropertyName._originalProfilerDumpIntervalFrames, Variant.From(in _originalProfilerDumpIntervalFrames));
		info.AddProperty(PropertyName._originalProfilerMaxMetricsPerDump, Variant.From(in _originalProfilerMaxMetricsPerDump));
		info.AddProperty(PropertyName._elapsed, Variant.From(in _elapsed));
		info.AddProperty(PropertyName._measuredRenderSeconds, Variant.From(in _measuredRenderSeconds));
		info.AddProperty(PropertyName._spawnFailures, Variant.From(in _spawnFailures));
		info.AddProperty(PropertyName._spawnElapsedMs, Variant.From(in _spawnElapsedMs));
		info.AddProperty(PropertyName._ready, Variant.From(in _ready));
		info.AddProperty(PropertyName._screenshotSaved, Variant.From(in _screenshotSaved));
		info.AddProperty(PropertyName._finished, Variant.From(in _finished));
		info.AddProperty(PropertyName._measurementStarted, Variant.From(in _measurementStarted));
		info.AddProperty(PropertyName._measurementThreadAllocatedBefore, Variant.From(in _measurementThreadAllocatedBefore));
		info.AddProperty(PropertyName._measurementTotalAllocatedBefore, Variant.From(in _measurementTotalAllocatedBefore));
		info.AddProperty(PropertyName._measurementGen0Before, Variant.From(in _measurementGen0Before));
		info.AddProperty(PropertyName._measurementGen1Before, Variant.From(in _measurementGen1Before));
		info.AddProperty(PropertyName._measurementGen2Before, Variant.From(in _measurementGen2Before));
		info.AddProperty(PropertyName._lastRenderTimestamp, Variant.From(in _lastRenderTimestamp));
		info.AddProperty(PropertyName._firstPublicationMeasured, Variant.From(in _firstPublicationMeasured));
		info.AddProperty(PropertyName._firstPublicationMs, Variant.From(in _firstPublicationMs));
		info.AddProperty(PropertyName._firstPublicationVisible, Variant.From(in _firstPublicationVisible));
		info.AddProperty(PropertyName._burstTransitionExecuted, Variant.From(in _burstTransitionExecuted));
		info.AddProperty(PropertyName._contentionIterations, Variant.From(in _contentionIterations));
		info.AddProperty(PropertyName._stopContentionThreads, Variant.From(in _stopContentionThreads));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.SpawnCount, out var value))
		{
			SpawnCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ZBucketCount, out var value2))
		{
			ZBucketCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.BulletScale, out var value3))
		{
			BulletScale = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.WarmupSeconds, out var value4))
		{
			WarmupSeconds = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.MeasureSeconds, out var value5))
		{
			MeasureSeconds = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.PrimeSpawnRoutes, out var value6))
		{
			PrimeSpawnRoutes = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.MaxPrimedSpawnMilliseconds, out var value7))
		{
			MaxPrimedSpawnMilliseconds = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.ScreenshotPath, out var value8))
		{
			ScreenshotPath = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeProfile, out var value9))
		{
			RuntimeProfile = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.EnableDetailedProfiler, out var value10))
		{
			EnableDetailedProfiler = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.BurstTransitionMode, out var value11))
		{
			BurstTransitionMode = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.BurstTransitionBudgetMilliseconds, out var value12))
		{
			BurstTransitionBudgetMilliseconds = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.BenchmarkMaxFps, out var value13))
		{
			BenchmarkMaxFps = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName.MinimumAverageFps, out var value14))
		{
			MinimumAverageFps = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.MinimumOnePercentLowFps, out var value15))
		{
			MinimumOnePercentLowFps = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName.MinimumZeroPointOnePercentLowFps, out var value16))
		{
			MinimumZeroPointOnePercentLowFps = value16.As<double>();
		}
		if (info.TryGetProperty(PropertyName.BackgroundContentionThreads, out var value17))
		{
			BackgroundContentionThreads = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName.MixedProjectileDefinitions, out var value18))
		{
			MixedProjectileDefinitions = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.MinimumAnimationDefinitionCount, out var value19))
		{
			MinimumAnimationDefinitionCount = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName._bulletField, out var value20))
		{
			_bulletField = value20.As<BulletField>();
		}
		if (info.TryGetProperty(PropertyName._manager, out var value21))
		{
			_manager = value21.As<TowerDefenseManager>();
		}
		if (info.TryGetProperty(PropertyName._originalGridSize, out var value22))
		{
			_originalGridSize = value22.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._originalGridBegin, out var value23))
		{
			_originalGridBegin = value23.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._originalGridNum, out var value24))
		{
			_originalGridNum = value24.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._originalMaxFps, out var value25))
		{
			_originalMaxFps = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName._originalPhysicsTicks, out var value26))
		{
			_originalPhysicsTicks = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._originalTimeScale, out var value27))
		{
			_originalTimeScale = value27.As<double>();
		}
		if (info.TryGetProperty(PropertyName._projectileUpdateManager, out var value28))
		{
			_projectileUpdateManager = value28.As<ProjectileUpdateManager>();
		}
		if (info.TryGetProperty(PropertyName._originalProjectileManagerProcessMode, out var value29))
		{
			_originalProjectileManagerProcessMode = value29.As<ProcessModeEnum>();
		}
		if (info.TryGetProperty(PropertyName._projectileManagerProcessModeCaptured, out var value30))
		{
			_projectileManagerProcessModeCaptured = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._originalPerfProfilerEnabled, out var value31))
		{
			_originalPerfProfilerEnabled = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._originalDetailedHotPathMetrics, out var value32))
		{
			_originalDetailedHotPathMetrics = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._originalProfilerDumpIntervalFrames, out var value33))
		{
			_originalProfilerDumpIntervalFrames = value33.As<int>();
		}
		if (info.TryGetProperty(PropertyName._originalProfilerMaxMetricsPerDump, out var value34))
		{
			_originalProfilerMaxMetricsPerDump = value34.As<int>();
		}
		if (info.TryGetProperty(PropertyName._elapsed, out var value35))
		{
			_elapsed = value35.As<double>();
		}
		if (info.TryGetProperty(PropertyName._measuredRenderSeconds, out var value36))
		{
			_measuredRenderSeconds = value36.As<double>();
		}
		if (info.TryGetProperty(PropertyName._spawnFailures, out var value37))
		{
			_spawnFailures = value37.As<int>();
		}
		if (info.TryGetProperty(PropertyName._spawnElapsedMs, out var value38))
		{
			_spawnElapsedMs = value38.As<double>();
		}
		if (info.TryGetProperty(PropertyName._ready, out var value39))
		{
			_ready = value39.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._screenshotSaved, out var value40))
		{
			_screenshotSaved = value40.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._finished, out var value41))
		{
			_finished = value41.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._measurementStarted, out var value42))
		{
			_measurementStarted = value42.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._measurementThreadAllocatedBefore, out var value43))
		{
			_measurementThreadAllocatedBefore = value43.As<long>();
		}
		if (info.TryGetProperty(PropertyName._measurementTotalAllocatedBefore, out var value44))
		{
			_measurementTotalAllocatedBefore = value44.As<long>();
		}
		if (info.TryGetProperty(PropertyName._measurementGen0Before, out var value45))
		{
			_measurementGen0Before = value45.As<int>();
		}
		if (info.TryGetProperty(PropertyName._measurementGen1Before, out var value46))
		{
			_measurementGen1Before = value46.As<int>();
		}
		if (info.TryGetProperty(PropertyName._measurementGen2Before, out var value47))
		{
			_measurementGen2Before = value47.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastRenderTimestamp, out var value48))
		{
			_lastRenderTimestamp = value48.As<long>();
		}
		if (info.TryGetProperty(PropertyName._firstPublicationMeasured, out var value49))
		{
			_firstPublicationMeasured = value49.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._firstPublicationMs, out var value50))
		{
			_firstPublicationMs = value50.As<double>();
		}
		if (info.TryGetProperty(PropertyName._firstPublicationVisible, out var value51))
		{
			_firstPublicationVisible = value51.As<int>();
		}
		if (info.TryGetProperty(PropertyName._burstTransitionExecuted, out var value52))
		{
			_burstTransitionExecuted = value52.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._contentionIterations, out var value53))
		{
			_contentionIterations = value53.As<long[]>();
		}
		if (info.TryGetProperty(PropertyName._stopContentionThreads, out var value54))
		{
			_stopContentionThreads = value54.As<int>();
		}
	}
}
