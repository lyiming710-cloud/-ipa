using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Wave/TowerDefenseBattleFeatureWave.cs")]
public class TowerDefenseBattleFeatureWave : TowerDefenseBattleFeature, ITowerDefenseProgressSaveGuard
{
	public delegate void WaveReadyEventHandler();

	public delegate void WaveBeginEventHandler(int id, bool isBigWave, bool isFinalWave);

	public delegate void BigWaveBeginEventHandler(int bigWaveId);

	public delegate void FinalEventHandler();

	public delegate void AddWaveReinforcementHandler(int lane, TowerDefenseLevelSpawnConfig spawn);

	public delegate void CollectWaveReinforcementsEventHandler(AddWaveReinforcementHandler addSpawn);

	private sealed class PendingGravestoneSpawnCallback
	{
		public int OperationId;

		public Action Callback;

		public readonly TaskCompletionSource<bool> Completion = new TaskCompletionSource<bool>();
	}

	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public static readonly StringName EmitWaveReady = "EmitWaveReady";

		public static readonly StringName EmitWaveBegin = "EmitWaveBegin";

		public static readonly StringName EmitBigWaveBegin = "EmitBigWaveBegin";

		public static readonly StringName EmitFinal = "EmitFinal";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName OnReady = "OnReady";

		public static readonly StringName SetupUI = "SetupUI";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName WavePhysicsProcess = "WavePhysicsProcess";

		public static readonly StringName StartWave = "StartWave";

		public static readonly StringName NextWave = "NextWave";

		public static readonly StringName AddSpawnCharacter = "AddSpawnCharacter";

		public static readonly StringName TrackCurrentCharacter = "TrackCurrentCharacter";

		public static readonly StringName UntrackCurrentCharacter = "UntrackCurrentCharacter";

		public static readonly StringName ClearCurrentCharacterTracking = "ClearCurrentCharacterTracking";

		public static readonly StringName CancelPendingSpawnOperations = "CancelPendingSpawnOperations";

		public static readonly StringName BeginPendingSpawnOperation = "BeginPendingSpawnOperation";

		public static readonly StringName IsPendingSpawnOperationCurrent = "IsPendingSpawnOperationCurrent";

		public static readonly StringName CompletePendingSpawnOperation = "CompletePendingSpawnOperation";

		public static readonly StringName ProcessPendingGravestoneSpawnCallbacks = "ProcessPendingGravestoneSpawnCallbacks";

		public static readonly StringName ClearPendingSpawnTimers = "ClearPendingSpawnTimers";

		public static readonly StringName AddSpawnCharacterWhenReady = "AddSpawnCharacterWhenReady";

		public static readonly StringName Spawn = "Spawn";

		public static readonly StringName WaveEventExecute = "WaveEventExecute";

		public static readonly StringName ShouldYieldSpawnFrame = "ShouldYieldSpawnFrame";

		public static readonly StringName HpPointDecrease = "HpPointDecrease";

		public static readonly StringName CharacterDestroy = "CharacterDestroy";

		public static readonly StringName ShowCharacter = "ShowCharacter";

		public static readonly StringName ClearShowCharacter = "ClearShowCharacter";

		public static readonly StringName CreateZombieWeightPick = "CreateZombieWeightPick";

		public static readonly StringName GravestoneSpawn = "GravestoneSpawn";

		public static readonly StringName GridSpawnZombie = "GridSpawnZombie";

		public static readonly StringName BungiSpawnZombie = "BungiSpawnZombie";

		public static readonly StringName SurvivalReady = "SurvivalReady";

		public static readonly StringName ShowInformation = "ShowInformation";

		public static readonly StringName ReadyEventExecute = "ReadyEventExecute";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName EnsureTrioRuntime = "EnsureTrioRuntime";

		public static readonly StringName ScheduleTrioAmbush = "ScheduleTrioAmbush";

		public static readonly StringName DestroyTrioRuntime = "DestroyTrioRuntime";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName HasPendingSpawnOperations = "HasPendingSpawnOperations";

		public static readonly StringName IsSpawnPipelineActive = "IsSpawnPipelineActive";

		public static readonly StringName IsFinalWaveSpawnSettled = "IsFinalWaveSpawnSettled";

		public static readonly StringName awaitGravestoneSpawn = "awaitGravestoneSpawn";

		public static readonly StringName awaitGridSpawn = "awaitGridSpawn";

		public static readonly StringName TrioRuntime = "TrioRuntime";

		public static readonly StringName _waveEventExecutionId = "_waveEventExecutionId";

		public static readonly StringName config = "config";

		public static readonly StringName currentDynamic = "currentDynamic";

		public static readonly StringName isSurvival = "isSurvival";

		public static readonly StringName survivalRunner = "survivalRunner";

		public static readonly StringName currentSpawnPoint = "currentSpawnPoint";

		public static readonly StringName isRunning = "isRunning";

		public static readonly StringName nextWaveTime = "nextWaveTime";

		public static readonly StringName timer = "timer";

		public static readonly StringName waveStart = "waveStart";

		public static readonly StringName waveFinal = "waveFinal";

		public static readonly StringName awardTime = "awardTime";

		public static readonly StringName awardPos = "awardPos";

		public static readonly StringName currentWave = "currentWave";

		public static readonly StringName currentCharacter = "currentCharacter";

		public static readonly StringName _trackingEpoch = "_trackingEpoch";

		public static readonly StringName currentHpPointTotal = "currentHpPointTotal";

		public static readonly StringName currentHpPoint = "currentHpPoint";

		public static readonly StringName savePitchforkLine = "savePitchforkLine";

		public static readonly StringName spawnOver = "spawnOver";

		public static readonly StringName showCharacterList = "showCharacterList";

		public static readonly StringName awaitSpawn = "awaitSpawn";

		public static readonly StringName _spawnCreationActive = "_spawnCreationActive";

		public static readonly StringName _spawnExecutionActive = "_spawnExecutionActive";

		public static readonly StringName _spawnExecutionEpoch = "_spawnExecutionEpoch";

		public static readonly StringName readySetPlantOver = "readySetPlantOver";

		public static readonly StringName sunFeature = "sunFeature";

		public static readonly StringName mapFeature = "mapFeature";

		public static readonly StringName progressFeature = "progressFeature";

		public static readonly StringName cameraFeature = "cameraFeature";

		public static readonly StringName seedBankFeature = "seedBankFeature";

		public static readonly StringName packetBankFeature = "packetBankFeature";

		public static readonly StringName mowerFeature = "mowerFeature";

		public static readonly StringName levelControl = "levelControl";

		public static readonly StringName _trioRuntime = "_trioRuntime";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private int _waveEventExecutionId;

	private static Texture2D _zombieSeaweed;

	public static TowerDefenseBattleFeatureWave Instance;

	public TowerDefenseLevelWaveManagerConfig config;

	public TowerDefenseLevelDynamicConfig currentDynamic;

	public bool isSurvival;

	public TowerDefenseLevelSurvivalRunner survivalRunner;

	public int currentSpawnPoint;

	public bool isRunning;

	public double nextWaveTime;

	public double timer;

	public bool waveStart;

	public bool waveFinal;

	public bool awardTime;

	public Vector2 awardPos = Vector2.Zero;

	public int currentWave;

	public Array<TowerDefenseCharacter> currentCharacter = new Array<TowerDefenseCharacter>();

	private int _trackingEpoch;

	public double currentHpPointTotal;

	public double currentHpPoint;

	public int savePitchforkLine = -1;

	public bool spawnOver;

	public Array<TowerDefenseCharacter> showCharacterList = new Array<TowerDefenseCharacter>();

	public bool awaitSpawn;

	private bool _spawnCreationActive;

	private bool _spawnExecutionActive;

	private int _spawnExecutionEpoch;

	private readonly TowerDefenseBattleOperationTracker _pendingSpawnOperations = new TowerDefenseBattleOperationTracker();

	private readonly System.Collections.Generic.Dictionary<SceneTreeTimer, Action> _pendingSpawnTimers = new System.Collections.Generic.Dictionary<SceneTreeTimer, Action>();

	private readonly Queue<PendingGravestoneSpawnCallback> _pendingGravestoneSpawnCallbacks = new Queue<PendingGravestoneSpawnCallback>();

	private readonly HashSet<PendingGravestoneSpawnCallback> _pendingGravestoneSpawnTickets = new HashSet<PendingGravestoneSpawnCallback>();

	public bool readySetPlantOver;

	public TowerDefenseBattleFeatureSun sunFeature;

	public TowerDefenseBattleFeatureMap mapFeature;

	public TowerDefenseBattleFeatureProgress progressFeature;

	public TowerDefenseBattleFeatureCamera cameraFeature;

	public TowerDefenseBattleFeatureSeedBank seedBankFeature;

	public TowerDefenseBattleFeaturePacketBank packetBankFeature;

	public TowerDefenseBattleFeatureMower mowerFeature;

	public TowerDefenseInGameLevelControl levelControl;

	private TrioAmbushRuntime _trioRuntime;

	private static Texture2D ZOMBIE_SEAWEED => _zombieSeaweed ?? (_zombieSeaweed = GD.Load<Texture2D>("uid://bpyla4k7iutq3"));

	public bool HasPendingSpawnOperations => _pendingSpawnOperations.HasPending;

	public bool IsSpawnPipelineActive
	{
		get
		{
			if (!awaitSpawn && !_spawnCreationActive && !_spawnExecutionActive)
			{
				return _pendingSpawnOperations.HasPending;
			}
			return true;
		}
	}

	public bool IsFinalWaveSpawnSettled
	{
		get
		{
			if (waveFinal && GodotObject.IsInstanceValid(config) && currentWave >= config.wave.Count && !awaitSpawn && !_spawnCreationActive && !_spawnExecutionActive)
			{
				return !_pendingSpawnOperations.HasPending;
			}
			return false;
		}
	}

	public bool awaitGravestoneSpawn => _pendingSpawnOperations.HasPending;

	public bool awaitGridSpawn => _pendingSpawnOperations.HasPending;

	internal TrioAmbushRuntime TrioRuntime => EnsureTrioRuntime();

	public event WaveReadyEventHandler OnWaveReady;

	public event WaveBeginEventHandler OnWaveBegin;

	public event BigWaveBeginEventHandler OnBigWaveBegin;

	public event FinalEventHandler OnFinal;

	public event CollectWaveReinforcementsEventHandler OnCollectWaveReinforcements;

	public void EmitWaveReady()
	{
		OnWaveReady?.Invoke();
	}

	public void EmitWaveBegin(int id, bool isBigWave, bool isFinalWave)
	{
		OnWaveBegin?.Invoke(id, isBigWave, isFinalWave);
	}

	public void EmitBigWaveBegin(int bigWaveId)
	{
		OnBigWaveBegin?.Invoke(bigWaveId);
	}

	public void EmitFinal()
	{
		OnFinal?.Invoke();
	}

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		_waveEventExecutionId = 0;
		config = new TowerDefenseLevelWaveManagerConfig();
		config.Init(data);
		if (config.dynamic.Count > TowerDefenseManager.Instance.currentDynamicLevel && config.dynamic[TowerDefenseManager.Instance.currentDynamicLevel] != null)
		{
			currentDynamic = config.dynamic[TowerDefenseManager.Instance.currentDynamicLevel];
		}
		else
		{
			currentDynamic = new TowerDefenseLevelDynamicConfig();
		}
	}

	public override void OnReady()
	{
		Instance = this;
		EnsureTrioRuntime();
	}

	public void SetupUI()
	{
		RunLifetimeTask(SetupUIAsync, "SetupUI");
	}

	private async Task SetupUIAsync()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (IsLifetimeActive && GodotObject.IsInstanceValid(progressFeature) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			string text = control?.ModLevelIdentity?.Difficulty ?? GameSaveManager.Instance.GetKeyValue("CurrentDifficult").AsString();
			string difficultText = "";
			switch (text)
			{
			case "Normal":
				difficultText = "正常";
				break;
			case "Difficult":
				difficultText = "困难";
				break;
			case "Ultimate":
				difficultText = "极限";
				break;
			}
			progressFeature.SetDifficultModulate(text);
			string levelName = Tr(TowerDefenseManager.Instance.currentLevelConfig.levelName).Replace("{LevelNumber}", TowerDefenseManager.Instance.currentLevelConfig.levelNumber.ToString());
			progressFeature.SetLevelName(levelName);
			progressFeature.SetDifficultText(difficultText);
			if (isSurvival)
			{
				progressFeature.SetSurvivalText(survivalRunner.roundNum);
			}
		}
	}

	public void Refresh()
	{
		isRunning = false;
		waveStart = false;
		waveFinal = false;
		spawnOver = false;
		currentWave = 0;
		ClearCurrentCharacterTracking();
		currentHpPointTotal = 0.0;
		currentHpPoint = 0.0;
		awaitSpawn = false;
		_spawnCreationActive = false;
		_spawnExecutionEpoch++;
		_spawnExecutionActive = false;
		readySetPlantOver = false;
		currentSpawnPoint = 0;
		if (progressFeature != null)
		{
			progressFeature.ProgressRefresh(isSurvival, isSurvival ? survivalRunner.roundNum : 0);
		}
	}

	public void WavePhysicsProcess(double delta)
	{
		if (!IsLifetimeActive)
		{
			return;
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		try
		{
			if (CommandManager.Instance.debug && CommandManager.Instance.debugWavePaused)
			{
				return;
			}
			ProcessPendingGravestoneSpawnCallbacks();
			if (!readySetPlantOver || IsSpawnPipelineActive || !isRunning || waveFinal)
			{
				return;
			}
			bool flag = false;
			double num = 1.0;
			if (spawnOver && waveStart)
			{
				if (currentHpPointTotal != 0.0)
				{
					num = currentHpPoint / currentHpPointTotal;
				}
				TowerDefensePerfProfiler.Sample("wave.currentCharacters", currentCharacter.Count);
				long startTicks2 = TowerDefensePerfProfiler.Begin();
				TowerDefenseManager instance = TowerDefenseManager.Instance;
				int num2 = ((GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(instance.characterRegistry)) ? instance.characterRegistry.GetZombieCount() : 0);
				TowerDefensePerfProfiler.End("wave.zombieGroupCount", startTicks2, num2);
				if (num2 <= 0 && timer > config.spawnColStart)
				{
					flag = true;
				}
			}
			if (timer < nextWaveTime)
			{
				timer += delta;
				if (num < config.maxNextWaveHealthPercentage && timer > config.spawnColStart)
				{
					flag = true;
				}
				if (num < config.minNextWaveHealthPercentage)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (!waveFinal && flag)
			{
				timer = 0.0;
				awaitSpawn = true;
				TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
				long startTicks3 = TowerDefensePerfProfiler.Begin();
				NextWave();
				TowerDefensePerfProfiler.End("wave.nextWave", startTicks3);
				TowerDefensePerfProfiler.EndSpikeProbe("wave.nextWave", in probe);
			}
		}
		finally
		{
			TowerDefensePerfProfiler.End("wave.physics", startTicks, currentCharacter.Count);
		}
	}

	public void StartWave()
	{
		EmitWaveReady();
		currentWave = 0;
		isRunning = true;
		waveStart = false;
		currentSpawnPoint = currentDynamic.startingPoints;
		nextWaveTime = config.beginCol;
		timer = 0.0;
		if (isSurvival && survivalRunner.roundNum > 0)
		{
			nextWaveTime = 6.0;
		}
	}

	public void NextWave()
	{
		RunLifetimeTask(NextWaveAsync, "NextWave");
	}

	private async Task NextWaveAsync()
	{
		if (!IsLifetimeActive || waveFinal || !(await WaitForWaveResumeAsync()))
		{
			return;
		}
		spawnOver = false;
		if (!waveStart)
		{
			AudioManager.Instance.AudioPlay("WaveBegin");
			progressFeature.SetProgressMeterVisible(visible: true);
			if (GetFeature("LookStar") is TowerDefenseBattleFeatureLookStar towerDefenseBattleFeatureLookStar && towerDefenseBattleFeatureLookStar.IsOpen())
			{
				progressFeature.SetProgressMeterVisible(visible: false);
			}
			waveStart = true;
			EmitBigWaveBegin(0);
		}
		if (currentWave + 1 >= config.wave.Count)
		{
			waveFinal = true;
		}
		if ((currentWave + 1) % config.flagWaveInterval == 0)
		{
			AudioManager.Instance.AudioPlay("WaveHuge");
			if (TowerDefenseManager.Instance.IsIZM2Mode())
			{
				TowerDefenseManager.Instance.TipsPlay("TOWERDEFENSE_TIPS_IZM2_HUGEWAVE", 4.0);
			}
			else
			{
				TowerDefenseManager.Instance.TipsPlay("TOWERDEFENSE_TIPS_HUGEWAVE", 4.0);
			}
			await ToSignal(GetTree().CreateTimer(4.0, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			if (!IsLifetimeActive || !GodotObject.IsInstanceValid(control) || !control.isGameRunning)
			{
				return;
			}
			AudioManager.Instance.AudioPlay("WaveHugeBegin");
		}
		bool dynamic = false;
		if (currentWave + 1 >= currentDynamic.startingWave)
		{
			currentSpawnPoint += currentDynamic.pointIncrementPerWave;
			dynamic = true;
		}
		if (currentWave < config.wave.Count)
		{
			Spawn(currentWave, dynamic);
			currentWave++;
		}
		nextWaveTime = config.spawnColEnd;
		awaitSpawn = false;
		if (currentWave == 1)
		{
			nextWaveTime *= 2.0;
		}
		if (currentWave == 2)
		{
			nextWaveTime *= 1.5;
		}
		if (currentWave % config.flagWaveInterval == 0)
		{
			nextWaveTime += 5.0;
		}
		if (currentWave == config.wave.Count && (!(GetFeature("LookStar") is TowerDefenseBattleFeatureLookStar towerDefenseBattleFeatureLookStar2) || !towerDefenseBattleFeatureLookStar2.OnWaveReachFinal(this)) && (!(GetFeature("GemMatch") is TowerDefenseBattleFeatureGemMatch towerDefenseBattleFeatureGemMatch) || !towerDefenseBattleFeatureGemMatch.OnWaveReachFinal(this)))
		{
			EmitFinal();
			AudioManager.Instance.AudioPlay("WaveFinal");
			if (!isSurvival)
			{
				TowerDefenseManager.Instance.TipsPlay("TOWERDEFENSE_TIPS_FINALWAVE");
			}
			else if (survivalRunner.config.roundLimit != -1 && survivalRunner.roundNum + 1 >= survivalRunner.config.roundLimit)
			{
				TowerDefenseManager.Instance.TipsPlay("TOWERDEFENSE_TIPS_FINALWAVE");
			}
		}
	}

	public void AddSpawnCharacter(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character) || currentCharacter.Contains(character))
		{
			return;
		}
		if (!character.IsNodeReady())
		{
			AddSpawnCharacterWhenReady(character);
		}
		else if (GodotObject.IsInstanceValid(character.instance))
		{
			double totalHitPoint = character.GetTotalHitPoint();
			if (TrackCurrentCharacter(character))
			{
				currentHpPointTotal += totalHitPoint;
				currentHpPoint += totalHitPoint;
			}
		}
	}

	private bool TrackCurrentCharacter(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character) || currentCharacter.Contains(character))
		{
			return false;
		}
		character.OnBodyHurt -= HpPointDecrease;
		character.OnArmorHurt -= HpPointDecrease;
		character.OnDestroy -= CharacterDestroy;
		character.OnBodyHurt += HpPointDecrease;
		character.OnArmorHurt += HpPointDecrease;
		character.OnDestroy += CharacterDestroy;
		currentCharacter.Add(character);
		return true;
	}

	private void UntrackCurrentCharacter(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			character.OnBodyHurt -= HpPointDecrease;
			character.OnArmorHurt -= HpPointDecrease;
			character.OnDestroy -= CharacterDestroy;
		}
		currentCharacter.Remove(character);
	}

	private void ClearCurrentCharacterTracking()
	{
		_trackingEpoch++;
		foreach (TowerDefenseCharacter item in currentCharacter)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.OnBodyHurt -= HpPointDecrease;
				item.OnArmorHurt -= HpPointDecrease;
				item.OnDestroy -= CharacterDestroy;
			}
		}
		currentCharacter.Clear();
	}

	private void CancelPendingSpawnOperations()
	{
		_pendingSpawnOperations.Clear();
		foreach (PendingGravestoneSpawnCallback pendingGravestoneSpawnTicket in _pendingGravestoneSpawnTickets)
		{
			pendingGravestoneSpawnTicket.Completion.TrySetResult(result: false);
		}
		_pendingGravestoneSpawnTickets.Clear();
		_pendingGravestoneSpawnCallbacks.Clear();
	}

	public int BeginPendingSpawnOperation()
	{
		return _pendingSpawnOperations.Begin();
	}

	public bool IsPendingSpawnOperationCurrent(int operationId)
	{
		return _pendingSpawnOperations.IsCurrent(operationId);
	}

	public void CompletePendingSpawnOperation(int operationId)
	{
		_pendingSpawnOperations.Complete(operationId);
	}

	private void ScheduleSpawnCallback(double delay, Action callback)
	{
		SceneTree tree = GetTree();
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(tree) || callback == null)
		{
			return;
		}
		double timeSec = Mathf.Max(0.0, delay);
		SceneTreeTimer timer = tree.CreateTimer(timeSec, processAlways: false);
		Action timeoutHandler = null;
		timeoutHandler = () =>
		{
			timer.Timeout -= timeoutHandler;
			_pendingSpawnTimers.Remove(timer);
			if (IsLifetimeActive)
			{
				callback();
			}
		};
		_pendingSpawnTimers[timer] = timeoutHandler;
		timer.Timeout += timeoutHandler;
	}

	private async Task<bool> WaitForWaveResumeAsync()
	{
		SceneTree tree = GetTree();
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(tree))
		{
			return false;
		}
		while (IsLifetimeActive && GodotObject.IsInstanceValid(tree) && tree.Paused)
		{
			await ToSignal(tree, SceneTree.SignalName.PhysicsFrame);
		}
		return IsLifetimeActive && GodotObject.IsInstanceValid(tree) && !tree.Paused;
	}

	private async Task<bool> WaitForWavePhysicsFrameAsync()
	{
		SceneTree tree = GetTree();
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(tree))
		{
			return false;
		}
		await ToSignal(tree, SceneTree.SignalName.PhysicsFrame);
		return await WaitForWaveResumeAsync();
	}

	private Task ScheduleGravestoneSpawnCallback(double delay, int operationId, Action callback)
	{
		PendingGravestoneSpawnCallback ticket = new PendingGravestoneSpawnCallback
		{
			OperationId = operationId,
			Callback = callback
		};
		_pendingGravestoneSpawnTickets.Add(ticket);
		double num = Mathf.Max(0.0, delay);
		SceneTree tree = GetTree();
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(tree) || callback == null)
		{
			CompleteGravestoneSpawnCallback(ticket, executed: false);
			return ticket.Completion.Task;
		}
		if (num <= 0.0)
		{
			QueueCallback();
			return ticket.Completion.Task;
		}
		SceneTreeTimer timer = tree.CreateTimer(num, processAlways: false);
		Action timeoutHandler = null;
		timeoutHandler = () =>
		{
			timer.Timeout -= timeoutHandler;
			_pendingSpawnTimers.Remove(timer);
			QueueCallback();
		};
		_pendingSpawnTimers[timer] = timeoutHandler;
		timer.Timeout += timeoutHandler;
		return ticket.Completion.Task;
		void QueueCallback()
		{
			if (!ticket.Completion.Task.IsCompleted)
			{
				if (!IsLifetimeActive || !IsPendingSpawnOperationCurrent(ticket.OperationId))
				{
					CompleteGravestoneSpawnCallback(ticket, executed: false);
				}
				else
				{
					_pendingGravestoneSpawnCallbacks.Enqueue(ticket);
				}
			}
		}
	}

	private void ProcessPendingGravestoneSpawnCallbacks()
	{
		if (_pendingGravestoneSpawnCallbacks.Count == 0)
		{
			return;
		}
		int num = Mathf.Max(1, config?.spawnMaxCharactersPerFrame ?? 8);
		double num2 = Mathf.Clamp(config?.spawnFrameBudgetMilliseconds ?? 6.0, 0.25, 16.0);
		ulong ticksUsec = Time.GetTicksUsec();
		int num3 = 0;
		while (_pendingGravestoneSpawnCallbacks.Count > 0 && num3 < num && (num3 <= 0 || !((double)(Time.GetTicksUsec() - ticksUsec) / 1000.0 >= num2)))
		{
			PendingGravestoneSpawnCallback pendingGravestoneSpawnCallback = _pendingGravestoneSpawnCallbacks.Dequeue();
			bool executed = false;
			try
			{
				if (IsLifetimeActive && IsPendingSpawnOperationCurrent(pendingGravestoneSpawnCallback.OperationId) && pendingGravestoneSpawnCallback.Callback != null)
				{
					pendingGravestoneSpawnCallback.Callback();
					executed = true;
				}
			}
			catch (Exception value)
			{
				GD.PushError($"[Wave] Gravestone spawn callback failed: {value}");
			}
			finally
			{
				CompleteGravestoneSpawnCallback(pendingGravestoneSpawnCallback, executed);
			}
			num3++;
		}
	}

	private void CompleteGravestoneSpawnCallback(PendingGravestoneSpawnCallback ticket, bool executed)
	{
		if (ticket != null)
		{
			_pendingGravestoneSpawnTickets.Remove(ticket);
			ticket.Callback = null;
			ticket.Completion.TrySetResult(executed);
		}
	}

	private void ClearPendingSpawnTimers()
	{
		foreach (KeyValuePair<SceneTreeTimer, Action> pendingSpawnTimer in _pendingSpawnTimers)
		{
			if (GodotObject.IsInstanceValid(pendingSpawnTimer.Key))
			{
				pendingSpawnTimer.Key.Timeout -= pendingSpawnTimer.Value;
			}
		}
		_pendingSpawnTimers.Clear();
	}

	private async Task WaitForPendingSpawnCallbacks(double maxDelay)
	{
		SceneTree tree = GetTree();
		if (tree == null)
		{
			return;
		}
		double num = Mathf.Max(0.0, maxDelay);
		if (num > 0.0)
		{
			await ToSignal(tree.CreateTimer(num, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		for (int frame = 0; frame < 2; frame++)
		{
			if (!IsLifetimeActive)
			{
				break;
			}
			if (!GodotObject.IsInstanceValid(control))
			{
				break;
			}
			if (!(await WaitForWavePhysicsFrameAsync()))
			{
				break;
			}
		}
	}

	private void AddSpawnCharacterWhenReady(TowerDefenseCharacter character)
	{
		RunLifetimeTask(() => AddSpawnCharacterWhenReadyAsync(character), "AddSpawnCharacterWhenReady");
	}

	private async Task AddSpawnCharacterWhenReadyAsync(TowerDefenseCharacter character)
	{
		int scheduledEpoch = _trackingEpoch;
		if (GodotObject.IsInstanceValid(character))
		{
			await ToSignal(character, Node.SignalName.Ready);
			if (IsLifetimeActive && GodotObject.IsInstanceValid(character) && scheduledEpoch == _trackingEpoch)
			{
				AddSpawnCharacter(character);
			}
		}
	}

	public void Spawn(int waveId, bool dynamic = false)
	{
		RunLifetimeTask(() => RunSpawnExecutionAsync(waveId, dynamic), "Spawn");
	}

	private async Task RunSpawnExecutionAsync(int waveId, bool dynamic)
	{
		_spawnExecutionEpoch++;
		if (_spawnExecutionEpoch <= 0)
		{
			_spawnExecutionEpoch = 1;
		}
		int executionEpoch = _spawnExecutionEpoch;
		_spawnExecutionActive = true;
		try
		{
			if (await WaitForWaveResumeAsync())
			{
				await SpawnAsync(waveId, dynamic, executionEpoch);
			}
		}
		finally
		{
			if (executionEpoch == _spawnExecutionEpoch)
			{
				_spawnExecutionActive = false;
			}
		}
	}

	private async Task SpawnAsync(int waveId, bool dynamic, int executionEpoch)
	{
		if (!IsLifetimeActive)
		{
			return;
		}
		spawnOver = false;
		progressFeature.SetProgressMeterWaveCurrent(waveId + 1);
		bool flag = (waveId + 1) % config.flagWaveInterval == 0;
		bool isFinalWave = waveId + 1 == config.wave.Count;
		EmitWaveBegin(waveId + 1, flag, isFinalWave);
		if (isSurvival)
		{
			survivalRunner.WaveReach(waveId + 1, flag);
		}
		if (flag)
		{
			EmitBigWaveBegin((int)((float)(waveId + 1) / (float)config.flagWaveInterval));
		}
		CancelPendingSpawnOperations();
		ClearCurrentCharacterTracking();
		currentHpPointTotal = 0.0;
		_spawnCreationActive = true;
		TowerDefenseCharacterSpawnBudget.TryBeginSpawnNoGcRegion(1024);
		try
		{
			ulong ticksUsec = Time.GetTicksUsec();
			WaveEventExecute(waveId);
			TowerDefenseCharacterSpawnBudget.RecordFrameWork(ticksUsec, updateAtomicEstimate: false);
			await SpawnGrid(waveId);
			if (!IsLifetimeActive)
			{
				return;
			}
			await SpawnZombie(waveId, dynamic);
		}
		finally
		{
			TowerDefenseCharacterSpawnBudget.EndSpawnNoGcRegion();
			_spawnCreationActive = false;
		}
		if (IsLifetimeActive && await WaitForWavePhysicsFrameAsync() && IsLifetimeActive && GodotObject.IsInstanceValid(control) && control.isGameRunning && !levelControl.awardCreate && _spawnExecutionEpoch == executionEpoch)
		{
			spawnOver = true;
			currentHpPoint = currentHpPointTotal;
		}
	}

	public void WaveEventExecute(int waveId)
	{
		if (waveId < 0 || waveId >= config.wave.Count)
		{
			return;
		}
		TowerDefenseLevelWaveConfig towerDefenseLevelWaveConfig = config.wave[waveId];
		TowerDefenseManager.Instance.ExecuteLevelEvent(towerDefenseLevelWaveConfig.eventList);
		if (!Global.IsMultiplayerMode || !MultiPlayerManager.IsHost)
		{
			return;
		}
		Godot.Collections.Array array = new Godot.Collections.Array();
		for (int i = 0; i < towerDefenseLevelWaveConfig.eventList.Count; i++)
		{
			try
			{
				TowerDefenseLevelEventBase towerDefenseLevelEventBase = towerDefenseLevelWaveConfig.eventList[i];
				if (GodotObject.IsInstanceValid(towerDefenseLevelEventBase))
				{
					array.Add(towerDefenseLevelEventBase.Export());
				}
			}
			catch (Exception value)
			{
				GD.PushError($"[WaveEvent] Failed to export wave {waveId} item {i}: {value}");
			}
		}
		if (towerDefenseLevelWaveConfig.eventList.Count > 0 && GodotObject.IsInstanceValid(MultiPlayerManager.Instance))
		{
			_waveEventExecutionId++;
			if (_waveEventExecutionId <= 0)
			{
				_waveEventExecutionId = 1;
			}
			MultiPlayerManager.Instance.SendWaveEventExecute(waveId, _waveEventExecutionId, Json.Stringify(array));
		}
	}

	public async Task SpawnZombie(int waveId, bool dynamic = false)
	{
		if (!IsLifetimeActive || !(await WaitForWaveResumeAsync()) || (CommandManager.Instance.debug && CommandManager.Instance.debugNoZombieSpawn) || levelControl.awardCreate)
		{
			return;
		}
		bool flag = (waveId + 1) % config.flagWaveInterval == 0;
		TowerDefenseLevelWaveConfig towerDefenseLevelWaveConfig = config.wave[waveId];
		bool flag2 = false;
		for (int i = 0; i < mapFeature.config.gridNum.Y; i++)
		{
			if (mapFeature.lineUse[i + 1])
			{
				flag2 = true;
				break;
			}
		}
		if (!flag2)
		{
			return;
		}
		int num = currentSpawnPoint;
		int num2 = 100000000;
		Godot.Collections.Array[] spawnList = new Godot.Collections.Array[mapFeature.config.gridNum.Y + 1];
		for (int j = 0; j < spawnList.Length; j++)
		{
			spawnList[j] = new Godot.Collections.Array();
		}
		int pitchforkSpawnLine = -1;
		if (towerDefenseLevelWaveConfig.dynamic != null)
		{
			num += towerDefenseLevelWaveConfig.dynamic.points;
		}
		if (isSurvival)
		{
			num = ((!flag) ? (num + survivalRunner.point) : (num + (int)Mathf.Floor((double)survivalRunner.point * survivalRunner.config.pointBigWaveScale)));
		}
		for (int k = 0; k < mapFeature.config.gridNum.Y; k++)
		{
			spawnList[k + 1] = new Godot.Collections.Array();
		}
		if (config.flagZombieUse && flag && config.flagZombie != "")
		{
			_ = 1 + GD.Randi() % (uint)mapFeature.config.gridNum.Y;
			int num3 = 10000;
			TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(config.flagZombie);
			Array<int> array = new Array<int>();
			for (int l = 0; l < mapFeature.config.gridNum.Y; l++)
			{
				if (mapFeature.lineUse[l + 1])
				{
					num3 = Mathf.Min(num3, spawnList[l + 1].Count);
				}
				array.Add(l + 1);
			}
			int num4 = array.PickRandom();
			array.Remove(num4);
			while (array.Count > 0 && (!mapFeature.lineUse[num4] || (num3 != spawnList[num4].Count && !packetConfigReadOnly.HasSpawnLimit()) || !packetConfigReadOnly.CanSpawn(num4)))
			{
				num4 = array.PickRandom();
				array.Remove(num4);
			}
			TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig = new TowerDefenseLevelSpawnConfig();
			towerDefenseLevelSpawnConfig.zombie = config.flagZombie;
			spawnList[num4].Add(towerDefenseLevelSpawnConfig);
		}
		towerDefenseLevelWaveConfig.spawn.Shuffle();
		if (!isSurvival)
		{
			foreach (TowerDefenseLevelSpawnConfig item3 in towerDefenseLevelWaveConfig.spawn)
			{
				for (int m = 0; m < item3.num; m++)
				{
					if (TryAssignPitchforkLine(item3))
					{
						continue;
					}
					if (item3.line != -1)
					{
						spawnList[item3.line].Add(item3);
						continue;
					}
					_ = 1 + GD.Randi() % (uint)mapFeature.config.gridNum.Y;
					int num5 = 10000;
					TowerDefensePacketConfig packetConfigReadOnly2 = TowerDefenseManager.GetPacketConfigReadOnly(item3.zombie);
					Array<int> array2 = new Array<int>();
					for (int n = 0; n < mapFeature.config.gridNum.Y; n++)
					{
						if (mapFeature.lineUse[n + 1])
						{
							num5 = Mathf.Min(num5, spawnList[n + 1].Count);
						}
						array2.Add(n + 1);
					}
					int num6 = array2.PickRandom();
					array2.Remove(num6);
					while (array2.Count > 0 && (!mapFeature.lineUse[num6] || (num5 != spawnList[num6].Count && !packetConfigReadOnly2.HasSpawnLimit()) || !packetConfigReadOnly2.CanSpawn(num6)))
					{
						num6 = array2.PickRandom();
						array2.Remove(num6);
					}
					spawnList[num6].Add(item3);
				}
			}
		}
		if (dynamic || towerDefenseLevelWaveConfig.dynamic != null || isSurvival)
		{
			Array<WeightPickItemBase> array3 = new Array<WeightPickItemBase>();
			Array<string> array4 = new Array<string>();
			if (currentDynamic != null)
			{
				foreach (string item4 in currentDynamic.zombiePool)
				{
					array4.Add(item4);
				}
			}
			if (towerDefenseLevelWaveConfig.dynamic != null)
			{
				foreach (string item5 in towerDefenseLevelWaveConfig.dynamic.zombiePool)
				{
					array4.Add(item5);
				}
			}
			if (isSurvival)
			{
				foreach (Variant item6 in survivalRunner.currentZombiePool)
				{
					string item = (string)item6;
					array4.Add(item);
				}
			}
			if (array4.Count > 0)
			{
				foreach (string item7 in array4)
				{
					ResourceManager.Instance.TOWERDEFENSE_PACKETS.TryGetValue(item7, out var value);
					TowerDefensePacketConfig towerDefensePacketConfig = value as TowerDefensePacketConfig;
					if (!GodotObject.IsInstanceValid(towerDefensePacketConfig) || !(towerDefensePacketConfig.characterConfig is TowerDefenseZombieConfig))
					{
						control.RejectLevelConfiguration($"第 {waveId + 1} 波动态僵尸卡不可用：{item7}");
						return;
					}
					if (towerDefensePacketConfig.GetWavePointCost() > 0)
					{
						TowerDefenseCharacterConfig characterConfig = towerDefensePacketConfig.characterConfig;
						TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig2 = new TowerDefenseLevelSpawnConfig
						{
							zombie = item7
						};
						if (characterConfig is TowerDefenseZombieConfig)
						{
							int weight = towerDefensePacketConfig.GetWeight();
							WeightPickItemBase item2 = new WeightPickItemBase(towerDefenseLevelSpawnConfig2, weight);
							num2 = Mathf.Min(num2, towerDefensePacketConfig.GetWavePointCost());
							array3.Add(item2);
						}
					}
				}
				if (num > 0 && array3.Count > 0)
				{
					while (num2 > 0 && num >= num2)
					{
						WeightPickItemBase weightPickItemBase = WeightPickMathine.Pick(array3);
						TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig3 = weightPickItemBase.item.AsGodotObject() as TowerDefenseLevelSpawnConfig;
						ResourceManager.Instance.TOWERDEFENSE_PACKETS.TryGetValue(towerDefenseLevelSpawnConfig3.zombie, out var value2);
						TowerDefensePacketConfig towerDefensePacketConfig2 = value2 as TowerDefensePacketConfig;
						if (!GodotObject.IsInstanceValid(towerDefensePacketConfig2) || !(towerDefensePacketConfig2.characterConfig is TowerDefenseZombieConfig) || towerDefensePacketConfig2.GetWavePointCost() <= 0)
						{
							control.RejectLevelConfiguration($"第 {waveId + 1} 波动态僵尸卡不可用：{towerDefenseLevelSpawnConfig3.zombie}");
							return;
						}
						TowerDefenseCharacterConfig characterConfig2 = towerDefensePacketConfig2.characterConfig;
						int num7 = (int)weightPickItemBase.weight;
						if (characterConfig2 is TowerDefenseZombieConfig)
						{
							num7 = towerDefensePacketConfig2.GetWavePointCost();
						}
						if (num < num7)
						{
							continue;
						}
						num -= num7;
						if (TryAssignPitchforkLine(towerDefenseLevelSpawnConfig3))
						{
							continue;
						}
						_ = 1 + GD.Randi() % (uint)mapFeature.config.gridNum.Y;
						int num8 = 10000;
						Array<int> array5 = new Array<int>();
						for (int num9 = 0; num9 < mapFeature.config.gridNum.Y; num9++)
						{
							if (mapFeature.lineUse[num9 + 1])
							{
								num8 = Mathf.Min(num8, spawnList[num9 + 1].Count);
							}
							array5.Add(num9 + 1);
						}
						int num10 = array5.PickRandom();
						array5.Remove(num10);
						while (array5.Count > 0 && (!mapFeature.lineUse[num10] || (num8 != spawnList[num10].Count && !towerDefensePacketConfig2.HasSpawnLimit()) || !towerDefensePacketConfig2.CanSpawn(num10)))
						{
							num10 = array5.PickRandom();
							array5.Remove(num10);
						}
						spawnList[num10].Add(towerDefenseLevelSpawnConfig3);
					}
				}
				else
				{
					num = -num;
					while (num2 > 0 && num >= num2)
					{
						WeightPickItemBase weightPickItemBase2 = WeightPickMathine.Pick(array3);
						TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig4 = weightPickItemBase2.item.AsGodotObject() as TowerDefenseLevelSpawnConfig;
						TowerDefensePacketConfig packetConfigReadOnly3 = TowerDefenseManager.GetPacketConfigReadOnly(towerDefenseLevelSpawnConfig4.zombie);
						TowerDefenseCharacterConfig characterConfig3 = packetConfigReadOnly3.characterConfig;
						int num11 = (int)weightPickItemBase2.weight;
						if (characterConfig3 is TowerDefenseZombieConfig)
						{
							num11 = packetConfigReadOnly3.GetWavePointCost();
						}
						if (num < num11)
						{
							continue;
						}
						bool flag3 = false;
						for (int num12 = 0; num12 < mapFeature.config.gridNum.Y; num12++)
						{
							if (!mapFeature.lineUse[num12 + 1])
							{
								continue;
							}
							foreach (Variant item8 in spawnList[num12 + 1])
							{
								if (item8.AsGodotObject() is TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig5 && towerDefenseLevelSpawnConfig5.zombie == towerDefenseLevelSpawnConfig4.zombie && towerDefenseLevelSpawnConfig5.spawnEvent.Count <= 0 && towerDefenseLevelSpawnConfig5.dieEvent.Count <= 0)
								{
									flag3 = true;
									break;
								}
							}
							if (flag3)
							{
								break;
							}
						}
						if (!flag3)
						{
							continue;
						}
						num -= num11;
						int num13 = (int)(1 + GD.Randi() % (uint)mapFeature.config.gridNum.Y);
						Godot.Collections.Array array6 = spawnList[num13];
						while (!mapFeature.lineUse[num13])
						{
							bool flag4 = false;
							foreach (Variant item9 in array6)
							{
								if (item9.AsGodotObject() is TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig6 && towerDefenseLevelSpawnConfig6.zombie == towerDefenseLevelSpawnConfig4.zombie && towerDefenseLevelSpawnConfig6.spawnEvent.Count <= 0 && towerDefenseLevelSpawnConfig6.dieEvent.Count <= 0)
								{
									flag4 = true;
									break;
								}
							}
							if (flag4)
							{
								break;
							}
							num13 = (int)(1 + GD.Randi() % (uint)mapFeature.config.gridNum.Y);
							array6 = spawnList[num13];
						}
						foreach (Variant item10 in array6.Duplicate())
						{
							if (item10.AsGodotObject() is TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig7 && towerDefenseLevelSpawnConfig7.zombie == towerDefenseLevelSpawnConfig4.zombie && towerDefenseLevelSpawnConfig7.spawnEvent.Count <= 0 && towerDefenseLevelSpawnConfig7.dieEvent.Count <= 0)
							{
								spawnList[num13].Remove(towerDefenseLevelSpawnConfig7);
								break;
							}
						}
					}
				}
			}
		}
		HashSet<TowerDefenseLevelSpawnConfig> reinforcements = new HashSet<TowerDefenseLevelSpawnConfig>();
		if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
		{
			OnCollectWaveReinforcements?.Invoke((int lane, TowerDefenseLevelSpawnConfig reinforcement) =>
			{
				if (lane >= 1 && lane < spawnList.Length && mapFeature.lineUse[lane] && GodotObject.IsInstanceValid(reinforcement) && reinforcements.Add(reinforcement))
				{
					spawnList[lane].Add(reinforcement);
				}
			});
		}
		int maxNum = 0;
		int gridNumY = mapFeature.config.gridNum.Y;
		List<int> spawnLineOrder = new List<int>(gridNumY);
		if (pitchforkSpawnLine > 0 && pitchforkSpawnLine <= gridNumY)
		{
			spawnLineOrder.Add(pitchforkSpawnLine - 1);
		}
		for (int num14 = 0; num14 < gridNumY; num14++)
		{
			Godot.Collections.Array array7 = spawnList[num14 + 1];
			array7.Shuffle();
			maxNum = Mathf.Max(maxNum, array7.Count);
			if (num14 + 1 != pitchforkSpawnLine)
			{
				spawnLineOrder.Add(num14);
			}
		}
		bool isMultiplayerMode = Global.IsMultiplayerMode;
		bool isMultiplayerHost = isMultiplayerMode && MultiPlayerManager.IsHost;
		bool isIZM2Mode = TowerDefenseManager.Instance.IsIZM2Mode();
		System.Collections.Generic.Dictionary<string, TowerDefensePacketConfig> spawnPacketBaseConfigs = new System.Collections.Generic.Dictionary<string, TowerDefensePacketConfig>(StringComparer.Ordinal);
		ulong ticksUsec = Time.GetTicksUsec();
		int num15 = 0;
		for (int spawnNameId = 0; spawnNameId < maxNum; spawnNameId++)
		{
			foreach (int spawnLine in spawnLineOrder)
			{
				Godot.Collections.Array array8 = spawnList[spawnLine + 1];
				if (spawnNameId >= array8.Count)
				{
					continue;
				}
				TowerDefenseLevelSpawnConfig spawn = array8[spawnNameId].AsGodotObject() as TowerDefenseLevelSpawnConfig;
				string spawnName = spawn.zombie;
				if (spawnName == "")
				{
					continue;
				}
				if (!spawnPacketBaseConfigs.TryGetValue(spawnName, out var packetBaseConfig))
				{
					packetBaseConfig = TowerDefenseManager.GetPacketConfigReadOnly(spawnName);
					if (!GodotObject.IsInstanceValid(packetBaseConfig))
					{
						continue;
					}
					spawnPacketBaseConfigs[spawnName] = packetBaseConfig;
				}
				double spawnOffsetX = (double)spawnNameId * 60.0 + (double)GD.Randf() * 60.0;
				int multiplayerCount = ((!isMultiplayerMode || reinforcements.Contains(spawn)) ? 1 : 2);
				for (int _mpi = 0; _mpi < multiplayerCount; _mpi++)
				{
					if (ShouldYieldSpawnFrame(ticksUsec, num15))
					{
						if (!(await WaitForWavePhysicsFrameAsync()) || !IsLifetimeActive || !GodotObject.IsInstanceValid(control) || !control.isGameRunning)
						{
							return;
						}
						ticksUsec = Time.GetTicksUsec();
						num15 = 0;
					}
					ulong ticksUsec2 = Time.GetTicksUsec();
					int num16 = spawnLine;
					double num17 = spawnOffsetX;
					if (isMultiplayerMode && _mpi == 1)
					{
						num16 = GD.RandRange(0, gridNumY - 1);
						num17 = (double)spawnNameId * 60.0 + (double)GD.Randf() * 60.0;
					}
					long startTicks = TowerDefensePerfProfiler.BeginHotPath();
					TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
					TowerDefenseCharacter towerDefenseCharacter = packetBaseConfig.SpawnWaveZombieFromReadOnlyConfig(num16 + 1, (float)num17);
					TowerDefensePerfProfiler.End("wave.spawn.character", startTicks);
					TowerDefensePerfProfiler.EndSpikeProbe("wave.spawn.character", in probe, 1);
					long startTicks2 = TowerDefensePerfProfiler.BeginHotPath();
					towerDefenseCharacter.invisible = config.zombieInvisible;
					if (GodotObject.IsInstanceValid(config.spawnOverride))
					{
						config.spawnOverride.ExecuteCharacter(towerDefenseCharacter);
					}
					if (GodotObject.IsInstanceValid(spawn.overrideVal))
					{
						spawn.overrideVal.ExecuteCharacter(towerDefenseCharacter);
					}
					foreach (TowerDefenseCharacterEventBase item11 in spawn.spawnEvent)
					{
						item11.Execute(towerDefenseCharacter.GetLogicalGlobalPosition(), towerDefenseCharacter);
					}
					if (spawn.dieEvent.Count > 0)
					{
						towerDefenseCharacter.dieEvent.AddRange(spawn.dieEvent);
					}
					if (towerDefenseCharacter is TowerDefenseZombie && !isIZM2Mode)
					{
						AddSpawnCharacter(towerDefenseCharacter);
					}
					if (isMultiplayerHost)
					{
						int nextSyncId = TowerDefenseManager.CurrentControl.GetNextSyncId();
						string spawnOverrideData = "";
						string spawnConfigOverrideData = "";
						if (GodotObject.IsInstanceValid(config.spawnOverride))
						{
							spawnOverrideData = Json.Stringify(config.spawnOverride.Export());
						}
						if (GodotObject.IsInstanceValid(spawn.overrideVal))
						{
							spawnConfigOverrideData = Json.Stringify(spawn.overrideVal.Export());
						}
						MultiPlayerManager.Instance.SendSpawnZombie(spawnName, num16 + 1, num17, nextSyncId, spawnOverrideData, spawnConfigOverrideData);
						TowerDefenseManager.CurrentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
					}
					TowerDefensePerfProfiler.End("wave.spawn.post", startTicks2);
					num15++;
					TowerDefenseCharacterSpawnBudget.RecordFrameWork(ticksUsec2);
					if (ShouldYieldSpawnFrame(ticksUsec, num15))
					{
						if (!(await WaitForWavePhysicsFrameAsync()) || !IsLifetimeActive || !GodotObject.IsInstanceValid(control) || !control.isGameRunning)
						{
							return;
						}
						ticksUsec = Time.GetTicksUsec();
						num15 = 0;
					}
				}
				if (ShouldYieldSpawnFrame(ticksUsec, num15))
				{
					if (!(await WaitForWavePhysicsFrameAsync()) || !IsLifetimeActive || !GodotObject.IsInstanceValid(control) || !control.isGameRunning)
					{
						return;
					}
					ticksUsec = Time.GetTicksUsec();
					num15 = 0;
				}
				packetBaseConfig = null;
			}
		}
		bool TryAssignPitchforkLine(TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig8)
		{
			if (savePitchforkLine == -1)
			{
				return false;
			}
			if (savePitchforkLine <= 0 || savePitchforkLine > mapFeature.config.gridNum.Y || !mapFeature.lineUse[savePitchforkLine])
			{
				savePitchforkLine = -1;
				return false;
			}
			pitchforkSpawnLine = savePitchforkLine;
			spawnList[pitchforkSpawnLine].Add(towerDefenseLevelSpawnConfig8);
			savePitchforkLine = -1;
			return true;
		}
	}

	private bool ShouldYieldSpawnFrame(ulong frameStartUsec, int spawnedCharacters)
	{
		if (spawnedCharacters >= Mathf.Max(1, config.spawnMaxCharactersPerFrame))
		{
			return true;
		}
		if (TowerDefenseCharacterSpawnBudget.IsFrameBudgetExhausted(config.spawnFrameBudgetMilliseconds))
		{
			return true;
		}
		ulong num = Time.GetTicksUsec() - frameStartUsec;
		double num2 = TowerDefenseCharacterSpawnBudget.ClampFrameBudget(config.spawnFrameBudgetMilliseconds) * 1000.0;
		return (double)num >= num2;
	}

	public async Task SpawnGrid(int waveId)
	{
		if (!(await WaitForWaveResumeAsync()) || !GodotObject.IsInstanceValid(control) || !control.isGameRunning)
		{
			return;
		}
		ulong ticksUsec = Time.GetTicksUsec();
		int num = 0;
		Vector2I gridNum = mapFeature.config.gridNum;
		Array<TowerDefenseCellInstance> emptyCell = new Array<TowerDefenseCellInstance>();
		for (int i = 1; i <= gridNum.X; i++)
		{
			for (int j = 1; j <= gridNum.Y; j++)
			{
				if (mapFeature.lineUse[j])
				{
					TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(i, j));
					if (mapCell != null && mapCell.characterList.Count == 0)
					{
						emptyCell.Add(mapCell);
					}
				}
			}
		}
		TowerDefenseLevelWaveConfig towerDefenseLevelWaveConfig = config.wave[waveId];
		foreach (TowerDefenseLevelGridSpawnConfig item in towerDefenseLevelWaveConfig.gridSpawn)
		{
			TowerDefenseCellInstance mapCell2 = TowerDefenseManager.GetMapCell(item.gridPos);
			if (mapCell2 != null)
			{
				emptyCell.Remove(mapCell2);
			}
		}
		foreach (TowerDefenseLevelGridSpawnConfig spawn in towerDefenseLevelWaveConfig.gridSpawn)
		{
			TowerDefensePacketConfig packetBaseConfig = TowerDefenseManager.GetPacketConfigReadOnly(spawn.packet);
			if (!GodotObject.IsInstanceValid(packetBaseConfig))
			{
				continue;
			}
			Vector2I gridPos = spawn.gridPos;
			TowerDefenseCellInstance cell = TowerDefenseManager.GetMapCell(gridPos);
			if (cell == null)
			{
				continue;
			}
			bool flag = true;
			if (!packetBaseConfig.characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.ALL) && !cell.CanPacketPlant(packetBaseConfig))
			{
				flag = false;
				foreach (TowerDefenseCellInstance item2 in emptyCell)
				{
					if (cell.CanMoveToCell(item2))
					{
						cell.MoveToCell(item2, jump: true);
						flag = true;
						emptyCell.Remove(item2);
						break;
					}
				}
			}
			if (!flag)
			{
				continue;
			}
			if (ShouldYieldSpawnFrame(ticksUsec, num))
			{
				if (!(await WaitForWavePhysicsFrameAsync()) || !IsLifetimeActive || !GodotObject.IsInstanceValid(control) || !control.isGameRunning)
				{
					return;
				}
				ticksUsec = Time.GetTicksUsec();
				num = 0;
			}
			if (!cell.CanPacketPlant(packetBaseConfig))
			{
				continue;
			}
			TowerDefensePacketConfig towerDefensePacketConfig = packetBaseConfig.CreateSpawnRuntimeCopy();
			if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
			{
				continue;
			}
			ulong ticksUsec2 = Time.GetTicksUsec();
			TowerDefenseCharacter towerDefenseCharacter = towerDefensePacketConfig.Plant(gridPos, playAudio: true, noLimit: true);
			if (GodotObject.IsInstanceValid(config.spawnOverride))
			{
				config.spawnOverride.ExecuteCharacter(towerDefenseCharacter);
			}
			if (GodotObject.IsInstanceValid(spawn.overrideVal))
			{
				spawn.overrideVal.ExecuteCharacter(towerDefenseCharacter);
			}
			foreach (TowerDefenseCharacterEventBase item3 in spawn.spawnEvent)
			{
				item3.Execute(towerDefenseCharacter.GetLogicalGlobalPosition(), towerDefenseCharacter);
			}
			if (spawn.dieEvent.Count > 0)
			{
				towerDefenseCharacter.dieEvent.AddRange(spawn.dieEvent);
			}
			if (towerDefenseCharacter is TowerDefensePlant && TowerDefenseManager.Instance.IsIZM2Mode())
			{
				AddSpawnCharacter(towerDefenseCharacter);
			}
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				int nextSyncId = TowerDefenseManager.CurrentControl.GetNextSyncId();
				MultiPlayerManager.Instance.SendSpawnGrid(spawn.packet, gridPos.X, gridPos.Y, nextSyncId);
				if (GodotObject.IsInstanceValid(towerDefenseCharacter))
				{
					TowerDefenseManager.CurrentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
				}
			}
			num++;
			TowerDefenseCharacterSpawnBudget.RecordFrameWork(ticksUsec2);
			if (ShouldYieldSpawnFrame(ticksUsec, num))
			{
				if (!(await WaitForWavePhysicsFrameAsync()) || !IsLifetimeActive || !GodotObject.IsInstanceValid(control) || !control.isGameRunning)
				{
					return;
				}
				ticksUsec = Time.GetTicksUsec();
				num = 0;
			}
		}
	}

	public void HpPointDecrease(int num)
	{
		currentHpPoint -= num;
	}

	public void CharacterDestroy(TowerDefenseCharacter character)
	{
		currentHpPoint -= character.GetCurrentHitPoint();
		UntrackCurrentCharacter(character);
	}

	public void ShowCharacter()
	{
		TowerDefenseLevelDynamicConfig towerDefenseLevelDynamicConfig = null;
		if (config.dynamic.Count > TowerDefenseManager.Instance.currentDynamicLevel)
		{
			towerDefenseLevelDynamicConfig = config.dynamic[TowerDefenseManager.Instance.currentDynamicLevel];
		}
		List<string> list = new List<string>();
		System.Collections.Generic.Dictionary<string, int> dictionary = new System.Collections.Generic.Dictionary<string, int>();
		List<Vector2I> list2 = new List<Vector2I>();
		List<string> list3 = new List<string>();
		if (towerDefenseLevelDynamicConfig != null)
		{
			foreach (string item5 in towerDefenseLevelDynamicConfig.zombiePool)
			{
				list3.Add(item5);
			}
		}
		if (isSurvival)
		{
			foreach (Variant item6 in survivalRunner.currentZombiePool)
			{
				string item = (string)item6;
				list3.Add(item);
			}
		}
		if (!isSurvival)
		{
			foreach (TowerDefenseLevelWaveConfig item7 in config.wave)
			{
				foreach (TowerDefenseLevelSpawnConfig item8 in item7.spawn)
				{
					string zombie = item8.zombie;
					if (!list.Contains(zombie))
					{
						list.Add(zombie);
						dictionary[zombie] = 0;
					}
					dictionary[zombie]++;
				}
				if (item7.dynamic == null)
				{
					continue;
				}
				foreach (string item9 in item7.dynamic.zombiePool)
				{
					list3.Add(item9);
				}
			}
		}
		foreach (string item10 in list3)
		{
			if (!list.Contains(item10))
			{
				list.Add(item10);
				dictionary[item10] = 0;
			}
			dictionary[item10]++;
		}
		foreach (string item11 in list)
		{
			Vector2I item2 = new Vector2I(GD.RandRange(0, 3), GD.RandRange(1, mapFeature.config.gridNum.Y));
			while (list2.Contains(item2))
			{
				item2 = new Vector2I(GD.RandRange(0, 3), GD.RandRange(1, mapFeature.config.gridNum.Y));
			}
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(item11);
			float num = item2.X * 60;
			if (item2.Y % 2 == 0)
			{
				num -= 30f;
			}
			TowerDefenseCharacter towerDefenseCharacter = packetConfig.Spawn(item2.Y, num, isIdle: true);
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				towerDefenseCharacter.invisible = config.zombieInvisible;
				if (GodotObject.IsInstanceValid(config.spawnOverride))
				{
					config.spawnOverride.ExecuteCharacter(towerDefenseCharacter);
				}
				towerDefenseCharacter.gridPos = new Vector2I(-1, -1);
				towerDefenseCharacter.ActivateLevelEntryPreview(packetConfig.packetAnimeClip);
				showCharacterList.Add(towerDefenseCharacter);
				list2.Add(item2);
				if (list2.Count >= 4 * mapFeature.config.gridNum.Y)
				{
					break;
				}
			}
		}
		if (list.Count <= 0 || list.Count >= 8)
		{
			return;
		}
		Array<WeightPickItemBase> array = new Array<WeightPickItemBase>();
		foreach (string item12 in list)
		{
			WeightPickItemBase item3 = new WeightPickItemBase(item12, dictionary[item12]);
			array.Add(item3);
		}
		for (int i = 0; i < 8 - list.Count; i++)
		{
			string packetName = WeightPickMathine.Pick(array).item.AsString();
			Vector2I item4 = new Vector2I(GD.RandRange(0, 3), GD.RandRange(1, mapFeature.config.gridNum.Y));
			while (list2.Contains(item4))
			{
				item4 = new Vector2I(GD.RandRange(0, 3), GD.RandRange(1, mapFeature.config.gridNum.Y));
			}
			TowerDefensePacketConfig packetConfig2 = TowerDefenseManager.GetPacketConfig(packetName);
			if (!(packetConfig2.characterConfig is TowerDefenseZombieConfig { preview: not false }))
			{
				continue;
			}
			float num2 = item4.X * 60;
			if (item4.Y % 2 == 0)
			{
				num2 -= 30f;
			}
			TowerDefenseCharacter towerDefenseCharacter2 = packetConfig2.Spawn(item4.Y, num2, isIdle: true);
			if (GodotObject.IsInstanceValid(towerDefenseCharacter2))
			{
				towerDefenseCharacter2.invisible = config.zombieInvisible;
				if (GodotObject.IsInstanceValid(config.spawnOverride))
				{
					config.spawnOverride.ExecuteCharacter(towerDefenseCharacter2);
				}
				towerDefenseCharacter2.gridPos = new Vector2I(-1, -1);
				towerDefenseCharacter2.ActivateLevelEntryPreview(packetConfig2.packetAnimeClip);
				showCharacterList.Add(towerDefenseCharacter2);
				list2.Add(item4);
			}
		}
	}

	public void ClearShowCharacter()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		foreach (TowerDefenseCharacter showCharacter in showCharacterList)
		{
			if (GodotObject.IsInstanceValid(showCharacter))
			{
				if (GodotObject.IsInstanceValid(instance))
				{
					instance.CharacterUnregister(showCharacter);
				}
				showCharacter.RemoveFromGroup("Character");
				showCharacter.QueueFree();
			}
		}
		showCharacterList.Clear();
	}

	private static Array<WeightPickItemBase> CreateZombieWeightPick(Godot.Collections.Array zombieNames)
	{
		Array<WeightPickItemBase> array = new Array<WeightPickItemBase>();
		foreach (Variant zombieName in zombieNames)
		{
			string text = (string)zombieName;
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(text);
			if (GodotObject.IsInstanceValid(packetConfig) && packetConfig.characterConfig is TowerDefenseZombieConfig towerDefenseZombieConfig)
			{
				int num = ((packetConfig.overrideWeight != -1) ? packetConfig.overrideWeight : towerDefenseZombieConfig.weight);
				if (num > 0)
				{
					array.Add(new WeightPickItemBase(text, num));
				}
			}
		}
		return array;
	}

	public void GravestoneSpawn(Godot.Collections.Array zombieNames, int zombieNum, Vector2 delay, TowerDefenseCharacterOverride override_)
	{
		RunLifetimeTask(() => GravestoneSpawnAsync(zombieNames, zombieNum, delay, override_), "GravestoneSpawn");
	}

	private async Task GravestoneSpawnAsync(Godot.Collections.Array zombieNames, int zombieNum, Vector2 delay, TowerDefenseCharacterOverride override_)
	{
		if (!IsLifetimeActive || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || !(await WaitForWaveResumeAsync()))
		{
			return;
		}
		int operationId = BeginPendingSpawnOperation();
		try
		{
			Array<WeightPickItemBase> weightPick = CreateZombieWeightPick(zombieNames);
			if (weightPick.Count == 0 || zombieNum <= 0 || !(await WaitForWavePhysicsFrameAsync()) || !IsLifetimeActive || !GodotObject.IsInstanceValid(control) || !control.isGameRunning || !IsPendingSpawnOperationCurrent(operationId))
			{
				return;
			}
			Array<Vector2I> cansSpawnPos = new Array<Vector2I>();
			foreach (Node item in Global.Instance.GetTree().GetNodesInGroup("Gravestone"))
			{
				if (item is TowerDefenseGravestone towerDefenseGravestone)
				{
					cansSpawnPos.Add(towerDefenseGravestone.gridPos);
				}
			}
			int num = Mathf.Min(cansSpawnPos.Count, zombieNum);
			if (num <= 0)
			{
				return;
			}
			List<Task> list = new List<Task>(num);
			while (num > 0)
			{
				double delay2 = Mathf.Max(0.0, GD.RandRange(delay.X, delay.Y));
				Array<WeightPickItemBase> _weightPick = weightPick;
				TowerDefenseCharacterOverride _override = override_;
				list.Add(ScheduleGravestoneSpawnCallback(delay2, operationId, () =>
				{
					if (IsPendingSpawnOperationCurrent(operationId) && GodotObject.IsInstanceValid(levelControl) && !levelControl.awardCreate && cansSpawnPos.Count != 0)
					{
						Array<Node> nodesInGroup = Global.Instance.GetTree().GetNodesInGroup("Gravestone");
						Array<Vector2I> array = new Array<Vector2I>();
						foreach (Node item2 in nodesInGroup)
						{
							if (item2 is TowerDefenseGravestone towerDefenseGravestone2)
							{
								array.Add(towerDefenseGravestone2.gridPos);
							}
						}
						WeightPickItemBase weightPickItemBase = WeightPickMathine.Pick(_weightPick);
						TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(weightPickItemBase.item.AsString());
						Vector2I vector2I = cansSpawnPos.PickRandom();
						if (array.Contains(vector2I))
						{
							AudioManager.Instance.AudioPlay("GravestoneRumble");
							TowerDefenseZombie zombie = packetConfig.Plant(vector2I, playAudio: false) as TowerDefenseZombie;
							double riseDuration = GD.RandRange(0.4, 0.6);
							TowerDefenseCharacterOverride overrideCapture = _override;
							string zombieNameCapture = weightPickItemBase.item.AsString();
							Vector2I gridPosCapture = vector2I;
							Callable.From(() =>
							{
								if (IsLifetimeActive && IsPendingSpawnOperationCurrent(operationId) && GodotObject.IsInstanceValid(zombie))
								{
									zombie.Rise(riseDuration);
									if (overrideCapture != null)
									{
										overrideCapture.ExecuteCharacter(zombie);
									}
									AddSpawnCharacter(zombie);
									if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && GodotObject.IsInstanceValid(control))
									{
										int nextSyncId = control.GetNextSyncId();
										double hitpointScale = (GodotObject.IsInstanceValid(zombie.instance) ? zombie.instance.hitpointScale : 1.0);
										double scaleVal = (GodotObject.IsInstanceValid(zombie.transformPoint) ? ((double)zombie.transformPoint.Scale.X) : 1.0);
										control.RegisterSyncCharacter(nextSyncId, zombie);
										MultiPlayerManager.Instance.SendSpawnCharacterAt(zombieNameCapture, gridPosCapture.X, gridPosCapture.Y, nextSyncId, hitpointScale, scaleVal, hypnoses: false, riseDuration);
									}
								}
							}).CallDeferred();
						}
						cansSpawnPos.Remove(vector2I);
					}
				}));
				num--;
			}
			await Task.WhenAll(list);
		}
		finally
		{
			CompletePendingSpawnOperation(operationId);
		}
	}

	public void GridSpawnZombie(Godot.Collections.Array zombieNames, int zombieNum, Vector2 delay, TowerDefenseCharacterOverride override_, Vector4I spawnPos, string spawnType)
	{
		RunLifetimeTask(() => GridSpawnZombieAsync(zombieNames, zombieNum, delay, override_, spawnPos, spawnType), "GridSpawnZombie");
	}

	private async Task GridSpawnZombieAsync(Godot.Collections.Array zombieNames, int zombieNum, Vector2 delay, TowerDefenseCharacterOverride override_, Vector4I spawnPos, string spawnType)
	{
		if (!IsLifetimeActive || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || !(await WaitForWaveResumeAsync()))
		{
			return;
		}
		int operationId = BeginPendingSpawnOperation();
		try
		{
			Array<WeightPickItemBase> array = CreateZombieWeightPick(zombieNames);
			if (array.Count == 0 || zombieNum <= 0)
			{
				return;
			}
			Array<Vector2I> cansSpawnPos = new Array<Vector2I>();
			for (int i = spawnPos.X; i <= spawnPos.Z; i++)
			{
				for (int j = spawnPos.Y; j <= spawnPos.W; j++)
				{
					Vector2I vector2I = new Vector2I(i, j);
					if (GodotObject.IsInstanceValid(TowerDefenseManager.GetMapCell(vector2I)))
					{
						cansSpawnPos.Add(vector2I);
					}
				}
			}
			int num = Mathf.Min(cansSpawnPos.Count, zombieNum);
			if (num <= 0)
			{
				return;
			}
			while (num > 0)
			{
				double delay2 = Mathf.Max(0.0, GD.RandRange(delay.X, delay.Y));
				Array<WeightPickItemBase> _weightPick = array;
				TowerDefenseCharacterOverride _override = override_;
				string _spawnType = spawnType;
				ScheduleSpawnCallback(delay2, () =>
				{
					if (IsPendingSpawnOperationCurrent(operationId) && GodotObject.IsInstanceValid(levelControl) && !levelControl.awardCreate && cansSpawnPos.Count > 0)
					{
						WeightPickItemBase weightPickItemBase = WeightPickMathine.Pick(_weightPick);
						Vector2I vector2I2 = cansSpawnPos.PickRandom();
						if (_spawnType == "Bungi")
						{
							TowerDefenseManager.Instance.BungiSpawn(weightPickItemBase.item.AsString(), vector2I2, _override, hypnoses: false, operationId);
						}
						else
						{
							AudioManager.Instance.AudioPlay("GravestoneRumble");
							TowerDefenseZombie towerDefenseZombie = TowerDefenseManager.GetPacketConfig(weightPickItemBase.item.AsString()).Plant(vector2I2, playAudio: false) as TowerDefenseZombie;
							if (GodotObject.IsInstanceValid(towerDefenseZombie))
							{
								double num2 = GD.RandRange(0.4, 0.6);
								towerDefenseZombie.Rise(num2);
								TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(vector2I2);
								if (GodotObject.IsInstanceValid(mapCell) && mapCell.isWater)
								{
									float num3 = towerDefenseZombie.Scale.X * towerDefenseZombie.sprite.Scale.X;
									int num4 = ((!(towerDefenseZombie.sprite.Scale.X < 0f)) ? 1 : (-1));
									(int, Vector2, bool, float)[] array2 = new (int, Vector2, bool, float)[4]
									{
										(0, new Vector2(-30f, -10f), true, 1f),
										(1, new Vector2(-20f, 20f), false, 0.9f),
										(2, new Vector2(25f, -20f), true, 1.1f),
										(3, new Vector2(30f, 15f), false, 0.85f)
									};
									Vector2 logicalGlobalPosition = towerDefenseZombie.GetLogicalGlobalPosition(towerDefenseZombie.sprite);
									if (GodotObject.IsInstanceValid(towerDefenseZombie.headSlot))
									{
										towerDefenseZombie.headSlot.Update();
										logicalGlobalPosition = towerDefenseZombie.GetLogicalGlobalPosition(towerDefenseZombie.headSlot);
									}
									(int, Vector2, bool, float)[] array3 = array2;
									for (int k = 0; k < array3.Length; k++)
									{
										(int, Vector2, bool, float) tuple = array3[k];
										Sprite2D sprite2D = new Sprite2D
										{
											Texture = ZOMBIE_SEAWEED,
											Hframes = 4,
											Frame = tuple.Item1
										};
										int num5 = ((!tuple.Item3) ? 1 : (-1));
										sprite2D.Scale = new Vector2(num3 * tuple.Item4 * (float)num5 * (float)num4, num3 * tuple.Item4);
										towerDefenseZombie.spriteGroup.AddChild(sprite2D, forceReadableName: false, Node.InternalMode.Disabled);
										sprite2D.GlobalPosition = logicalGlobalPosition + new Vector2(tuple.Item2.X * (float)num4, tuple.Item2.Y);
									}
								}
								if (_override != null)
								{
									_override.ExecuteCharacter(towerDefenseZombie);
								}
								AddSpawnCharacter(towerDefenseZombie);
								if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && GodotObject.IsInstanceValid(control))
								{
									int nextSyncId = control.GetNextSyncId();
									double hitpointScale = (GodotObject.IsInstanceValid(towerDefenseZombie.instance) ? towerDefenseZombie.instance.hitpointScale : 1.0);
									double scaleVal = (GodotObject.IsInstanceValid(towerDefenseZombie.transformPoint) ? ((double)towerDefenseZombie.transformPoint.Scale.X) : 1.0);
									control.RegisterSyncCharacter(nextSyncId, towerDefenseZombie);
									MultiPlayerManager.Instance.SendSpawnCharacterAt(weightPickItemBase.item.AsString(), vector2I2.X, vector2I2.Y, nextSyncId, hitpointScale, scaleVal, hypnoses: false, num2);
								}
							}
						}
						cansSpawnPos.Remove(vector2I2);
					}
				});
				num--;
			}
			await WaitForPendingSpawnCallbacks(Mathf.Max(delay.X, delay.Y));
		}
		finally
		{
			CompletePendingSpawnOperation(operationId);
		}
	}

	public void BungiSpawnZombie(Godot.Collections.Array zombieNames, int zombieNum, Vector2 delay, TowerDefenseCharacterOverride override_, Vector4I spawnPos, bool hypnoses)
	{
		RunLifetimeTask(() => BungiSpawnZombieAsync(zombieNames, zombieNum, delay, override_, spawnPos, hypnoses), "BungiSpawnZombie");
	}

	private async Task BungiSpawnZombieAsync(Godot.Collections.Array zombieNames, int zombieNum, Vector2 delay, TowerDefenseCharacterOverride override_, Vector4I spawnPos, bool hypnoses)
	{
		if (!IsLifetimeActive || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || !(await WaitForWaveResumeAsync()))
		{
			return;
		}
		int operationId = BeginPendingSpawnOperation();
		try
		{
			Array<WeightPickItemBase> weightPick = CreateZombieWeightPick(zombieNames);
			if (weightPick.Count == 0 || zombieNum <= 0)
			{
				return;
			}
			Array<Vector2I> spawnPositions = new Array<Vector2I>();
			for (int i = spawnPos.X; i <= spawnPos.Z; i++)
			{
				for (int j = spawnPos.Y; j <= spawnPos.W; j++)
				{
					spawnPositions.Add(new Vector2I(i, j));
				}
			}
			int num = Mathf.Min(spawnPositions.Count, zombieNum);
			if (num <= 0)
			{
				return;
			}
			for (int k = 0; k < num; k++)
			{
				double delay2 = Mathf.Max(0.0, GD.RandRange(delay.X, delay.Y));
				ScheduleSpawnCallback(delay2, () =>
				{
					if (IsPendingSpawnOperationCurrent(operationId) && GodotObject.IsInstanceValid(levelControl) && !levelControl.awardCreate && spawnPositions.Count != 0)
					{
						WeightPickItemBase weightPickItemBase = WeightPickMathine.Pick(weightPick);
						Vector2I vector2I = spawnPositions.PickRandom();
						spawnPositions.Remove(vector2I);
						TowerDefenseManager.Instance.BungiSpawn(weightPickItemBase.item.AsString(), vector2I, override_, hypnoses, operationId);
					}
				});
			}
			await WaitForPendingSpawnCallbacks(Mathf.Max(delay.X, delay.Y));
		}
		finally
		{
			CompletePendingSpawnOperation(operationId);
		}
	}

	public override Task GameInit()
	{
		levelControl = control.levelControl;
		sunFeature = GetFeature("Sun") as TowerDefenseBattleFeatureSun;
		mapFeature = GetFeature("Map") as TowerDefenseBattleFeatureMap;
		progressFeature = GetFeature("Progress") as TowerDefenseBattleFeatureProgress;
		cameraFeature = GetFeature("Camera") as TowerDefenseBattleFeatureCamera;
		seedBankFeature = GetFeature("SeedBank") as TowerDefenseBattleFeatureSeedBank;
		packetBankFeature = GetFeature("PacketBank") as TowerDefenseBattleFeaturePacketBank;
		mowerFeature = GetFeature("Mower") as TowerDefenseBattleFeatureMower;
		progressFeature.ProgressInit(config.wave.Count, config.flagWaveInterval);
		if (config.survival.AsString() != "" || GodotObject.IsInstanceValid(config.customSurvival))
		{
			isSurvival = true;
			survivalRunner = new TowerDefenseLevelSurvivalRunner();
			if (config.isCustomSurvival)
			{
				survivalRunner.Init(config.customSurvival);
			}
			else
			{
				TowerDefenseLevelSurvivalConfig towerDefenseLevelSurvivalConfig = ResourceManager.Instance.SURVIVALS[(string)config.survival] as TowerDefenseLevelSurvivalConfig;
				towerDefenseLevelSurvivalConfig.Init();
				survivalRunner.Init(towerDefenseLevelSurvivalConfig);
			}
		}
		ResourceManager.Instance.RequireFullGameplayResourcesReady("TowerDefenseBattleFeatureWave");
		SetupUI();
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		levelControl = control.levelControl;
		sunFeature = GetFeature("Sun") as TowerDefenseBattleFeatureSun;
		mapFeature = GetFeature("Map") as TowerDefenseBattleFeatureMap;
		progressFeature = GetFeature("Progress") as TowerDefenseBattleFeatureProgress;
		cameraFeature = GetFeature("Camera") as TowerDefenseBattleFeatureCamera;
		seedBankFeature = GetFeature("SeedBank") as TowerDefenseBattleFeatureSeedBank;
		packetBankFeature = GetFeature("PacketBank") as TowerDefenseBattleFeaturePacketBank;
		mowerFeature = GetFeature("Mower") as TowerDefenseBattleFeatureMower;
		if (config.survival.AsString() != "" || GodotObject.IsInstanceValid(config.customSurvival))
		{
			isSurvival = true;
			survivalRunner = new TowerDefenseLevelSurvivalRunner();
			if (config.isCustomSurvival)
			{
				survivalRunner.Init(config.customSurvival);
			}
			else
			{
				TowerDefenseLevelSurvivalConfig towerDefenseLevelSurvivalConfig = ResourceManager.Instance.SURVIVALS[(string)config.survival] as TowerDefenseLevelSurvivalConfig;
				towerDefenseLevelSurvivalConfig.Init();
				survivalRunner.Init(towerDefenseLevelSurvivalConfig);
			}
		}
		progressFeature.ProgressInit(config.wave.Count, config.flagWaveInterval);
		ResourceManager.Instance.RequireFullGameplayResourcesReady("TowerDefenseBattleFeatureWave");
		SetupUI();
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		StartWave();
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		progressFeature.SetProgressMeterVisible(visible: true);
		if (isSurvival)
		{
			progressFeature.SetSurvivalVisible(visible: true);
		}
		progressFeature?.SetProgressMeterWaveCurrent(currentWave);
		return Task.CompletedTask;
	}

	public void SurvivalReady()
	{
		ClearShowCharacter();
		progressFeature.SetDifficultVisible(visible: true);
		if (isSurvival)
		{
			progressFeature.SetSurvivalVisible(visible: true);
		}
		ShowInformation();
		ReadyEventExecute();
	}

	public void ShowInformation()
	{
		progressFeature.SetLevelNameVisible(visible: true);
		if (Global.Instance.enterLevelMode == "DailyLevel" || Global.Instance.enterLevelMode == "OnlineLevel" || Global.Instance.enterLevelMode == "LevelTest" || Global.Instance.enterLevelMode == "DiyLevel")
		{
			progressFeature.SetDifficultVisible(visible: false);
		}
	}

	public void ReadyEventExecute()
	{
		if (data.ContainsKey("EventReady"))
		{
			TowerDefenseManager.Instance.ExecuteLevelEvent(data["EventReady"].AsGodotArray<TowerDefenseLevelEventBase>());
		}
	}

	public override Dictionary SyncSerialize()
	{
		return new Dictionary
		{
			{ "current_wave", currentWave },
			{ "wave_final", waveFinal },
			{ "spawn_over", spawnOver },
			{
				"timer",
				Mathf.Snapped(timer, 1.0)
			},
			{ "next_wave_time", nextWaveTime },
			{ "is_running", isRunning },
			{ "wave_start", waveStart },
			{ "current_spawn_point", currentSpawnPoint },
			{ "await_spawn", awaitSpawn },
			{ "current_hp_point_total", currentHpPointTotal },
			{ "current_hp_point", currentHpPoint }
		};
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		if (_data.ContainsKey("current_wave"))
		{
			currentWave = _data["current_wave"].AsInt32();
		}
		if (_data.ContainsKey("wave_final"))
		{
			waveFinal = _data["wave_final"].AsBool();
		}
		if (_data.ContainsKey("spawn_over"))
		{
			spawnOver = _data["spawn_over"].AsBool();
		}
		if (_data.ContainsKey("timer"))
		{
			timer = _data["timer"].AsDouble();
		}
		if (_data.ContainsKey("next_wave_time"))
		{
			nextWaveTime = _data["next_wave_time"].AsDouble();
		}
		if (_data.ContainsKey("is_running"))
		{
			isRunning = _data["is_running"].AsBool();
		}
		if (_data.ContainsKey("wave_start"))
		{
			waveStart = _data["wave_start"].AsBool();
		}
		if (_data.ContainsKey("current_spawn_point"))
		{
			currentSpawnPoint = _data["current_spawn_point"].AsInt32();
		}
		if (_data.ContainsKey("await_spawn"))
		{
			awaitSpawn = _data["await_spawn"].AsBool();
		}
		if (_data.ContainsKey("current_hp_point_total"))
		{
			currentHpPointTotal = _data["current_hp_point_total"].AsDouble();
		}
		if (_data.ContainsKey("current_hp_point"))
		{
			currentHpPoint = _data["current_hp_point"].AsDouble();
		}
		if (GodotObject.IsInstanceValid(progressFeature))
		{
			progressFeature.SetProgressMeterWaveCurrent(currentWave);
			progressFeature.SetProgressMeterVisible(waveStart);
		}
	}

	public override Dictionary SaveFeature()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseCharacter item in currentCharacter)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				array.Add(item.Name.ToString().ValidateNodeName());
			}
		}
		Dictionary dictionary = new Dictionary
		{
			{
				"trioIceRemaining",
				EnsureTrioRuntime()?.Clock.IceRemaining ?? 0.0
			},
			{ "currentWave", currentWave },
			{ "waveFinal", waveFinal },
			{ "waveStart", waveStart },
			{ "spawnOver", spawnOver },
			{ "timer", timer },
			{ "nextWaveTime", nextWaveTime },
			{ "isRunning", isRunning },
			{ "currentSpawnPoint", currentSpawnPoint },
			{ "currentHpPointTotal", currentHpPointTotal },
			{ "currentHpPoint", currentHpPoint },
			{ "isSurvival", isSurvival },
			{ "savePitchforkLine", savePitchforkLine },
			{ "awaitSpawn", awaitSpawn },
			{ "awaitGravestoneSpawn", awaitGravestoneSpawn },
			{ "awaitGridSpawn", awaitGridSpawn },
			{ "awardTime", awardTime },
			{ "awardPos", awardPos },
			{ "currentCharacterNames", array }
		};
		if (isSurvival && GodotObject.IsInstanceValid(survivalRunner))
		{
			dictionary["survivalPoint"] = survivalRunner.point;
			dictionary["survivalRoundNum"] = survivalRunner.roundNum;
			dictionary["survivalZombiePool"] = survivalRunner.zombiePool;
			dictionary["survivalAddZombiePoolReachId"] = survivalRunner.addZombiePoolReachId;
			dictionary["survivalCurrentZombiePool"] = survivalRunner.currentZombiePool;
		}
		return dictionary;
	}

	public bool CanSaveProgress(out string reason)
	{
		if (awaitSpawn || _pendingSpawnOperations.HasPending)
		{
			reason = "wave spawn task is still running";
			return false;
		}
		if (waveStart && !spawnOver)
		{
			reason = "current wave has not reached a stable spawn checkpoint";
			return false;
		}
		reason = "";
		return true;
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		EnsureTrioRuntime()?.Clock.RestoreIce(_data.GetValueOrDefault("trioIceRemaining", 0.0).AsDouble());
		currentWave = _data.GetValueOrDefault("currentWave", 0).AsInt32();
		waveFinal = _data.GetValueOrDefault("waveFinal", false).AsBool();
		waveStart = _data.GetValueOrDefault("waveStart", false).AsBool();
		spawnOver = _data.GetValueOrDefault("spawnOver", false).AsBool();
		timer = _data.GetValueOrDefault("timer", 0.0).AsDouble();
		nextWaveTime = _data.GetValueOrDefault("nextWaveTime", 0.0).AsDouble();
		isRunning = _data.GetValueOrDefault("isRunning", false).AsBool();
		currentSpawnPoint = _data.GetValueOrDefault("currentSpawnPoint", 0).AsInt32();
		currentHpPointTotal = _data.GetValueOrDefault("currentHpPointTotal", 0.0).AsDouble();
		currentHpPoint = _data.GetValueOrDefault("currentHpPoint", 0.0).AsDouble();
		isSurvival = _data.GetValueOrDefault("isSurvival", false).AsBool();
		savePitchforkLine = _data.GetValueOrDefault("savePitchforkLine", -1).AsInt32();
		bool flag = _data.GetValueOrDefault("awaitSpawn", false).AsBool();
		bool flag2 = _data.GetValueOrDefault("awaitGravestoneSpawn", false).AsBool();
		bool flag3 = _data.GetValueOrDefault("awaitGridSpawn", false).AsBool();
		bool num = (flag | flag2 | flag3) || (waveStart && !spawnOver);
		awaitSpawn = false;
		if (num)
		{
			GD.PushWarning("[Wave] Loaded a legacy in-flight checkpoint; treating already restored characters as the completed spawn set.");
			spawnOver = true;
		}
		awardTime = _data.GetValueOrDefault("awardTime", false).AsBool();
		awardPos = _data.GetValueOrDefault("awardPos", Vector2.Zero).AsVector2();
		CancelPendingSpawnOperations();
		ClearCurrentCharacterTracking();
		foreach (Variant item in _data.GetValueOrDefault("currentCharacterNames", new Godot.Collections.Array()).AsGodotArray())
		{
			StringName key = (StringName)item;
			if (_owner.charcterDicionary.ContainsKey(key))
			{
				TowerDefenseCharacter character = _owner.charcterDicionary[key];
				TrackCurrentCharacter(character);
			}
		}
		progressFeature?.ConfigureWaveProgress(config.wave.Count, config.flagWaveInterval, currentWave);
		if (isSurvival && GodotObject.IsInstanceValid(survivalRunner) && _data.ContainsKey("survivalPoint"))
		{
			survivalRunner.RestoreProgressState(_data.GetValueOrDefault("survivalPoint", 0).AsInt32(), _data.GetValueOrDefault("survivalRoundNum", 0).AsInt32(), _data.GetValueOrDefault("survivalCurrentZombiePool", new Godot.Collections.Array()).AsGodotArray());
		}
	}

	public override void Destroy()
	{
		DestroyTrioRuntime();
		isRunning = false;
		awaitSpawn = false;
		if (_spawnCreationActive)
		{
			TowerDefenseCharacterSpawnBudget.EndSpawnNoGcRegion();
		}
		_spawnCreationActive = false;
		_spawnExecutionEpoch++;
		_spawnExecutionActive = false;
		ClearPendingSpawnTimers();
		CancelPendingSpawnOperations();
		ClearCurrentCharacterTracking();
		ClearShowCharacter();
		OnWaveReady = null;
		OnWaveBegin = null;
		OnBigWaveBegin = null;
		OnFinal = null;
		OnCollectWaveReinforcements = null;
		if (Instance == this)
		{
			Instance = null;
		}
		survivalRunner = null;
		sunFeature = null;
		mapFeature = null;
		progressFeature = null;
		cameraFeature = null;
		seedBankFeature = null;
		packetBankFeature = null;
		mowerFeature = null;
		levelControl = null;
		currentDynamic = null;
		config = null;
		base.Destroy();
	}

	private TrioAmbushRuntime EnsureTrioRuntime()
	{
		if (GodotObject.IsInstanceValid(_trioRuntime))
		{
			return _trioRuntime;
		}
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(control) || !control.IsInsideTree())
		{
			return null;
		}
		_trioRuntime = new TrioAmbushRuntime
		{
			Name = "TrioAmbushRuntime",
			Wave = this
		};
		control.AddChild(_trioRuntime, forceReadableName: false, Node.InternalMode.Disabled);
		return _trioRuntime;
	}

	public void ScheduleTrioAmbush(bool coral)
	{
		EnsureTrioRuntime()?.Schedule(coral);
	}

	private void DestroyTrioRuntime()
	{
		if (GodotObject.IsInstanceValid(_trioRuntime))
		{
			_trioRuntime.Cancel();
			_trioRuntime.QueueFree();
		}
		_trioRuntime = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(44)
		{
			new MethodInfo(MethodName.EmitWaveReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitWaveBegin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isBigWave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isFinalWave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitBigWaveBegin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "bigWaveId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitFinal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WavePhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddSpawnCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TrackCurrentCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.UntrackCurrentCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearCurrentCharacterTracking, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelPendingSpawnOperations, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginPendingSpawnOperation, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsPendingSpawnOperationCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "operationId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CompletePendingSpawnOperation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "operationId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessPendingGravestoneSpawnCallbacks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearPendingSpawnTimers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddSpawnCharacterWhenReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Spawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "dynamic", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WaveEventExecute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldYieldSpawnFrame, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameStartUsec", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "spawnedCharacters", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HpPointDecrease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CharacterDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearShowCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateZombieWeightPick, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "zombieNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GravestoneSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "zombieNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "zombieNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "override_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GridSpawnZombie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "zombieNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "zombieNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "override_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector4I, "spawnPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "spawnType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BungiSpawnZombie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "zombieNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "zombieNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "override_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector4I, "spawnPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hypnoses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SurvivalReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowInformation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadyEventExecute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureTrioRuntime, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleTrioAmbush, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "coral", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroyTrioRuntime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EmitWaveReady && args.Count == 0)
		{
			EmitWaveReady();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitWaveBegin && args.Count == 3)
		{
			EmitWaveBegin(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitBigWaveBegin && args.Count == 1)
		{
			EmitBigWaveBegin(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitFinal && args.Count == 0)
		{
			EmitFinal();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnReady && args.Count == 0)
		{
			OnReady();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupUI && args.Count == 0)
		{
			SetupUI();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.WavePhysicsProcess && args.Count == 1)
		{
			WavePhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartWave && args.Count == 0)
		{
			StartWave();
			ret = default;
			return true;
		}
		if (method == MethodName.NextWave && args.Count == 0)
		{
			NextWave();
			ret = default;
			return true;
		}
		if (method == MethodName.AddSpawnCharacter && args.Count == 1)
		{
			AddSpawnCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrackCurrentCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TrackCurrentCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.UntrackCurrentCharacter && args.Count == 1)
		{
			UntrackCurrentCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearCurrentCharacterTracking && args.Count == 0)
		{
			ClearCurrentCharacterTracking();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelPendingSpawnOperations && args.Count == 0)
		{
			CancelPendingSpawnOperations();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginPendingSpawnOperation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(BeginPendingSpawnOperation());
			return true;
		}
		if (method == MethodName.IsPendingSpawnOperationCurrent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPendingSpawnOperationCurrent(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CompletePendingSpawnOperation && args.Count == 1)
		{
			CompletePendingSpawnOperation(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessPendingGravestoneSpawnCallbacks && args.Count == 0)
		{
			ProcessPendingGravestoneSpawnCallbacks();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPendingSpawnTimers && args.Count == 0)
		{
			ClearPendingSpawnTimers();
			ret = default;
			return true;
		}
		if (method == MethodName.AddSpawnCharacterWhenReady && args.Count == 1)
		{
			AddSpawnCharacterWhenReady(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Spawn && args.Count == 2)
		{
			Spawn(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.WaveEventExecute && args.Count == 1)
		{
			WaveEventExecute(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldYieldSpawnFrame && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldYieldSpawnFrame(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.HpPointDecrease && args.Count == 1)
		{
			HpPointDecrease(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CharacterDestroy && args.Count == 1)
		{
			CharacterDestroy(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCharacter && args.Count == 0)
		{
			ShowCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearShowCharacter && args.Count == 0)
		{
			ClearShowCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateZombieWeightPick && args.Count == 1)
		{
			Array<WeightPickItemBase> array = CreateZombieWeightPick(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.GravestoneSpawn && args.Count == 4)
		{
			GravestoneSpawn(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.GridSpawnZombie && args.Count == 6)
		{
			GridSpawnZombie(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in args[3]), VariantUtils.ConvertTo<Vector4I>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.BungiSpawnZombie && args.Count == 6)
		{
			BungiSpawnZombie(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in args[3]), VariantUtils.ConvertTo<Vector4I>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.SurvivalReady && args.Count == 0)
		{
			SurvivalReady();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowInformation && args.Count == 0)
		{
			ShowInformation();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadyEventExecute && args.Count == 0)
		{
			ReadyEventExecute();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncSerialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SyncSerialize());
			return true;
		}
		if (method == MethodName.SyncDeserialize && args.Count == 1)
		{
			SyncDeserialize(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveFeature());
			return true;
		}
		if (method == MethodName.LoadFeature && args.Count == 2)
		{
			LoadFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureTrioRuntime && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TrioAmbushRuntime>(EnsureTrioRuntime());
			return true;
		}
		if (method == MethodName.ScheduleTrioAmbush && args.Count == 1)
		{
			ScheduleTrioAmbush(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyTrioRuntime && args.Count == 0)
		{
			DestroyTrioRuntime();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateZombieWeightPick && args.Count == 1)
		{
			Array<WeightPickItemBase> array = CreateZombieWeightPick(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EmitWaveReady)
		{
			return true;
		}
		if (method == MethodName.EmitWaveBegin)
		{
			return true;
		}
		if (method == MethodName.EmitBigWaveBegin)
		{
			return true;
		}
		if (method == MethodName.EmitFinal)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.OnReady)
		{
			return true;
		}
		if (method == MethodName.SetupUI)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.WavePhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.StartWave)
		{
			return true;
		}
		if (method == MethodName.NextWave)
		{
			return true;
		}
		if (method == MethodName.AddSpawnCharacter)
		{
			return true;
		}
		if (method == MethodName.TrackCurrentCharacter)
		{
			return true;
		}
		if (method == MethodName.UntrackCurrentCharacter)
		{
			return true;
		}
		if (method == MethodName.ClearCurrentCharacterTracking)
		{
			return true;
		}
		if (method == MethodName.CancelPendingSpawnOperations)
		{
			return true;
		}
		if (method == MethodName.BeginPendingSpawnOperation)
		{
			return true;
		}
		if (method == MethodName.IsPendingSpawnOperationCurrent)
		{
			return true;
		}
		if (method == MethodName.CompletePendingSpawnOperation)
		{
			return true;
		}
		if (method == MethodName.ProcessPendingGravestoneSpawnCallbacks)
		{
			return true;
		}
		if (method == MethodName.ClearPendingSpawnTimers)
		{
			return true;
		}
		if (method == MethodName.AddSpawnCharacterWhenReady)
		{
			return true;
		}
		if (method == MethodName.Spawn)
		{
			return true;
		}
		if (method == MethodName.WaveEventExecute)
		{
			return true;
		}
		if (method == MethodName.ShouldYieldSpawnFrame)
		{
			return true;
		}
		if (method == MethodName.HpPointDecrease)
		{
			return true;
		}
		if (method == MethodName.CharacterDestroy)
		{
			return true;
		}
		if (method == MethodName.ShowCharacter)
		{
			return true;
		}
		if (method == MethodName.ClearShowCharacter)
		{
			return true;
		}
		if (method == MethodName.CreateZombieWeightPick)
		{
			return true;
		}
		if (method == MethodName.GravestoneSpawn)
		{
			return true;
		}
		if (method == MethodName.GridSpawnZombie)
		{
			return true;
		}
		if (method == MethodName.BungiSpawnZombie)
		{
			return true;
		}
		if (method == MethodName.SurvivalReady)
		{
			return true;
		}
		if (method == MethodName.ShowInformation)
		{
			return true;
		}
		if (method == MethodName.ReadyEventExecute)
		{
			return true;
		}
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.EnsureTrioRuntime)
		{
			return true;
		}
		if (method == MethodName.ScheduleTrioAmbush)
		{
			return true;
		}
		if (method == MethodName.DestroyTrioRuntime)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._waveEventExecutionId)
		{
			_waveEventExecutionId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseLevelWaveManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName.currentDynamic)
		{
			currentDynamic = VariantUtils.ConvertTo<TowerDefenseLevelDynamicConfig>(in value);
			return true;
		}
		if (name == PropertyName.isSurvival)
		{
			isSurvival = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.survivalRunner)
		{
			survivalRunner = VariantUtils.ConvertTo<TowerDefenseLevelSurvivalRunner>(in value);
			return true;
		}
		if (name == PropertyName.currentSpawnPoint)
		{
			currentSpawnPoint = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.isRunning)
		{
			isRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.nextWaveTime)
		{
			nextWaveTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.waveStart)
		{
			waveStart = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.waveFinal)
		{
			waveFinal = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.awardTime)
		{
			awardTime = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.awardPos)
		{
			awardPos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.currentWave)
		{
			currentWave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentCharacter)
		{
			currentCharacter = VariantUtils.ConvertToArray<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._trackingEpoch)
		{
			_trackingEpoch = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentHpPointTotal)
		{
			currentHpPointTotal = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.currentHpPoint)
		{
			currentHpPoint = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.savePitchforkLine)
		{
			savePitchforkLine = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.spawnOver)
		{
			spawnOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.showCharacterList)
		{
			showCharacterList = VariantUtils.ConvertToArray<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.awaitSpawn)
		{
			awaitSpawn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._spawnCreationActive)
		{
			_spawnCreationActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._spawnExecutionActive)
		{
			_spawnExecutionActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._spawnExecutionEpoch)
		{
			_spawnExecutionEpoch = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.readySetPlantOver)
		{
			readySetPlantOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sunFeature)
		{
			sunFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureSun>(in value);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName.progressFeature)
		{
			progressFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureProgress>(in value);
			return true;
		}
		if (name == PropertyName.cameraFeature)
		{
			cameraFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureCamera>(in value);
			return true;
		}
		if (name == PropertyName.seedBankFeature)
		{
			seedBankFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureSeedBank>(in value);
			return true;
		}
		if (name == PropertyName.packetBankFeature)
		{
			packetBankFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeaturePacketBank>(in value);
			return true;
		}
		if (name == PropertyName.mowerFeature)
		{
			mowerFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMower>(in value);
			return true;
		}
		if (name == PropertyName.levelControl)
		{
			levelControl = VariantUtils.ConvertTo<TowerDefenseInGameLevelControl>(in value);
			return true;
		}
		if (name == PropertyName._trioRuntime)
		{
			_trioRuntime = VariantUtils.ConvertTo<TrioAmbushRuntime>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.HasPendingSpawnOperations)
		{
			from = HasPendingSpawnOperations;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsSpawnPipelineActive)
		{
			from = IsSpawnPipelineActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsFinalWaveSpawnSettled)
		{
			from = IsFinalWaveSpawnSettled;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.awaitGravestoneSpawn)
		{
			from = awaitGravestoneSpawn;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.awaitGridSpawn)
		{
			from = awaitGridSpawn;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TrioRuntime)
		{
			value = VariantUtils.CreateFrom<TrioAmbushRuntime>(TrioRuntime);
			return true;
		}
		if (name == PropertyName._waveEventExecutionId)
		{
			value = VariantUtils.CreateFrom(in _waveEventExecutionId);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.currentDynamic)
		{
			value = VariantUtils.CreateFrom(in currentDynamic);
			return true;
		}
		if (name == PropertyName.isSurvival)
		{
			value = VariantUtils.CreateFrom(in isSurvival);
			return true;
		}
		if (name == PropertyName.survivalRunner)
		{
			value = VariantUtils.CreateFrom(in survivalRunner);
			return true;
		}
		if (name == PropertyName.currentSpawnPoint)
		{
			value = VariantUtils.CreateFrom(in currentSpawnPoint);
			return true;
		}
		if (name == PropertyName.isRunning)
		{
			value = VariantUtils.CreateFrom(in isRunning);
			return true;
		}
		if (name == PropertyName.nextWaveTime)
		{
			value = VariantUtils.CreateFrom(in nextWaveTime);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName.waveStart)
		{
			value = VariantUtils.CreateFrom(in waveStart);
			return true;
		}
		if (name == PropertyName.waveFinal)
		{
			value = VariantUtils.CreateFrom(in waveFinal);
			return true;
		}
		if (name == PropertyName.awardTime)
		{
			value = VariantUtils.CreateFrom(in awardTime);
			return true;
		}
		if (name == PropertyName.awardPos)
		{
			value = VariantUtils.CreateFrom(in awardPos);
			return true;
		}
		if (name == PropertyName.currentWave)
		{
			value = VariantUtils.CreateFrom(in currentWave);
			return true;
		}
		if (name == PropertyName.currentCharacter)
		{
			value = VariantUtils.CreateFromArray(currentCharacter);
			return true;
		}
		if (name == PropertyName._trackingEpoch)
		{
			value = VariantUtils.CreateFrom(in _trackingEpoch);
			return true;
		}
		if (name == PropertyName.currentHpPointTotal)
		{
			value = VariantUtils.CreateFrom(in currentHpPointTotal);
			return true;
		}
		if (name == PropertyName.currentHpPoint)
		{
			value = VariantUtils.CreateFrom(in currentHpPoint);
			return true;
		}
		if (name == PropertyName.savePitchforkLine)
		{
			value = VariantUtils.CreateFrom(in savePitchforkLine);
			return true;
		}
		if (name == PropertyName.spawnOver)
		{
			value = VariantUtils.CreateFrom(in spawnOver);
			return true;
		}
		if (name == PropertyName.showCharacterList)
		{
			value = VariantUtils.CreateFromArray(showCharacterList);
			return true;
		}
		if (name == PropertyName.awaitSpawn)
		{
			value = VariantUtils.CreateFrom(in awaitSpawn);
			return true;
		}
		if (name == PropertyName._spawnCreationActive)
		{
			value = VariantUtils.CreateFrom(in _spawnCreationActive);
			return true;
		}
		if (name == PropertyName._spawnExecutionActive)
		{
			value = VariantUtils.CreateFrom(in _spawnExecutionActive);
			return true;
		}
		if (name == PropertyName._spawnExecutionEpoch)
		{
			value = VariantUtils.CreateFrom(in _spawnExecutionEpoch);
			return true;
		}
		if (name == PropertyName.readySetPlantOver)
		{
			value = VariantUtils.CreateFrom(in readySetPlantOver);
			return true;
		}
		if (name == PropertyName.sunFeature)
		{
			value = VariantUtils.CreateFrom(in sunFeature);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			value = VariantUtils.CreateFrom(in mapFeature);
			return true;
		}
		if (name == PropertyName.progressFeature)
		{
			value = VariantUtils.CreateFrom(in progressFeature);
			return true;
		}
		if (name == PropertyName.cameraFeature)
		{
			value = VariantUtils.CreateFrom(in cameraFeature);
			return true;
		}
		if (name == PropertyName.seedBankFeature)
		{
			value = VariantUtils.CreateFrom(in seedBankFeature);
			return true;
		}
		if (name == PropertyName.packetBankFeature)
		{
			value = VariantUtils.CreateFrom(in packetBankFeature);
			return true;
		}
		if (name == PropertyName.mowerFeature)
		{
			value = VariantUtils.CreateFrom(in mowerFeature);
			return true;
		}
		if (name == PropertyName.levelControl)
		{
			value = VariantUtils.CreateFrom(in levelControl);
			return true;
		}
		if (name == PropertyName._trioRuntime)
		{
			value = VariantUtils.CreateFrom(in _trioRuntime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._waveEventExecutionId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.currentDynamic, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isSurvival, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.survivalRunner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentSpawnPoint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.nextWaveTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.waveStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.waveFinal, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.awardTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.awardPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentWave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.currentCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._trackingEpoch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentHpPointTotal, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentHpPoint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.savePitchforkLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.spawnOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.showCharacterList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.awaitSpawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._spawnCreationActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._spawnExecutionActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._spawnExecutionEpoch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasPendingSpawnOperations, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsSpawnPipelineActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsFinalWaveSpawnSettled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.awaitGravestoneSpawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.awaitGridSpawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.readySetPlantOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sunFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.progressFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cameraFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.seedBankFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBankFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mowerFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._trioRuntime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.TrioRuntime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._waveEventExecutionId, Variant.From(in _waveEventExecutionId));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.currentDynamic, Variant.From(in currentDynamic));
		info.AddProperty(PropertyName.isSurvival, Variant.From(in isSurvival));
		info.AddProperty(PropertyName.survivalRunner, Variant.From(in survivalRunner));
		info.AddProperty(PropertyName.currentSpawnPoint, Variant.From(in currentSpawnPoint));
		info.AddProperty(PropertyName.isRunning, Variant.From(in isRunning));
		info.AddProperty(PropertyName.nextWaveTime, Variant.From(in nextWaveTime));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName.waveStart, Variant.From(in waveStart));
		info.AddProperty(PropertyName.waveFinal, Variant.From(in waveFinal));
		info.AddProperty(PropertyName.awardTime, Variant.From(in awardTime));
		info.AddProperty(PropertyName.awardPos, Variant.From(in awardPos));
		info.AddProperty(PropertyName.currentWave, Variant.From(in currentWave));
		info.AddProperty(PropertyName.currentCharacter, Variant.CreateFrom(currentCharacter));
		info.AddProperty(PropertyName._trackingEpoch, Variant.From(in _trackingEpoch));
		info.AddProperty(PropertyName.currentHpPointTotal, Variant.From(in currentHpPointTotal));
		info.AddProperty(PropertyName.currentHpPoint, Variant.From(in currentHpPoint));
		info.AddProperty(PropertyName.savePitchforkLine, Variant.From(in savePitchforkLine));
		info.AddProperty(PropertyName.spawnOver, Variant.From(in spawnOver));
		info.AddProperty(PropertyName.showCharacterList, Variant.CreateFrom(showCharacterList));
		info.AddProperty(PropertyName.awaitSpawn, Variant.From(in awaitSpawn));
		info.AddProperty(PropertyName._spawnCreationActive, Variant.From(in _spawnCreationActive));
		info.AddProperty(PropertyName._spawnExecutionActive, Variant.From(in _spawnExecutionActive));
		info.AddProperty(PropertyName._spawnExecutionEpoch, Variant.From(in _spawnExecutionEpoch));
		info.AddProperty(PropertyName.readySetPlantOver, Variant.From(in readySetPlantOver));
		info.AddProperty(PropertyName.sunFeature, Variant.From(in sunFeature));
		info.AddProperty(PropertyName.mapFeature, Variant.From(in mapFeature));
		info.AddProperty(PropertyName.progressFeature, Variant.From(in progressFeature));
		info.AddProperty(PropertyName.cameraFeature, Variant.From(in cameraFeature));
		info.AddProperty(PropertyName.seedBankFeature, Variant.From(in seedBankFeature));
		info.AddProperty(PropertyName.packetBankFeature, Variant.From(in packetBankFeature));
		info.AddProperty(PropertyName.mowerFeature, Variant.From(in mowerFeature));
		info.AddProperty(PropertyName.levelControl, Variant.From(in levelControl));
		info.AddProperty(PropertyName._trioRuntime, Variant.From(in _trioRuntime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._waveEventExecutionId, out var value))
		{
			_waveEventExecutionId = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseLevelWaveManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.currentDynamic, out var value3))
		{
			currentDynamic = value3.As<TowerDefenseLevelDynamicConfig>();
		}
		if (info.TryGetProperty(PropertyName.isSurvival, out var value4))
		{
			isSurvival = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.survivalRunner, out var value5))
		{
			survivalRunner = value5.As<TowerDefenseLevelSurvivalRunner>();
		}
		if (info.TryGetProperty(PropertyName.currentSpawnPoint, out var value6))
		{
			currentSpawnPoint = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.isRunning, out var value7))
		{
			isRunning = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.nextWaveTime, out var value8))
		{
			nextWaveTime = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value9))
		{
			timer = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.waveStart, out var value10))
		{
			waveStart = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.waveFinal, out var value11))
		{
			waveFinal = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.awardTime, out var value12))
		{
			awardTime = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.awardPos, out var value13))
		{
			awardPos = value13.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.currentWave, out var value14))
		{
			currentWave = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentCharacter, out var value15))
		{
			currentCharacter = value15.AsGodotArray<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._trackingEpoch, out var value16))
		{
			_trackingEpoch = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentHpPointTotal, out var value17))
		{
			currentHpPointTotal = value17.As<double>();
		}
		if (info.TryGetProperty(PropertyName.currentHpPoint, out var value18))
		{
			currentHpPoint = value18.As<double>();
		}
		if (info.TryGetProperty(PropertyName.savePitchforkLine, out var value19))
		{
			savePitchforkLine = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName.spawnOver, out var value20))
		{
			spawnOver = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.showCharacterList, out var value21))
		{
			showCharacterList = value21.AsGodotArray<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.awaitSpawn, out var value22))
		{
			awaitSpawn = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._spawnCreationActive, out var value23))
		{
			_spawnCreationActive = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._spawnExecutionActive, out var value24))
		{
			_spawnExecutionActive = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._spawnExecutionEpoch, out var value25))
		{
			_spawnExecutionEpoch = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName.readySetPlantOver, out var value26))
		{
			readySetPlantOver = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sunFeature, out var value27))
		{
			sunFeature = value27.As<TowerDefenseBattleFeatureSun>();
		}
		if (info.TryGetProperty(PropertyName.mapFeature, out var value28))
		{
			mapFeature = value28.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName.progressFeature, out var value29))
		{
			progressFeature = value29.As<TowerDefenseBattleFeatureProgress>();
		}
		if (info.TryGetProperty(PropertyName.cameraFeature, out var value30))
		{
			cameraFeature = value30.As<TowerDefenseBattleFeatureCamera>();
		}
		if (info.TryGetProperty(PropertyName.seedBankFeature, out var value31))
		{
			seedBankFeature = value31.As<TowerDefenseBattleFeatureSeedBank>();
		}
		if (info.TryGetProperty(PropertyName.packetBankFeature, out var value32))
		{
			packetBankFeature = value32.As<TowerDefenseBattleFeaturePacketBank>();
		}
		if (info.TryGetProperty(PropertyName.mowerFeature, out var value33))
		{
			mowerFeature = value33.As<TowerDefenseBattleFeatureMower>();
		}
		if (info.TryGetProperty(PropertyName.levelControl, out var value34))
		{
			levelControl = value34.As<TowerDefenseInGameLevelControl>();
		}
		if (info.TryGetProperty(PropertyName._trioRuntime, out var value35))
		{
			_trioRuntime = value35.As<TrioAmbushRuntime>();
		}
	}
}
