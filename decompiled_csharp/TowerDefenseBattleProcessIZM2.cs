using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Process/IZM2/TowerDefenseBattleProcessIZM2.cs")]
public class TowerDefenseBattleProcessIZM2 : TowerDefenseBattleProcess
{
	public new class MethodName : TowerDefenseBattleProcess.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName OnReady = "OnReady";

		public new static readonly StringName PhysicsProcess = "PhysicsProcess";

		public new static readonly StringName InputProcess = "InputProcess";

		public static readonly StringName ResolveDependencies = "ResolveDependencies";

		public new static readonly StringName GameFail = "GameFail";

		public new static readonly StringName ZombieEnterHouse = "ZombieEnterHouse";

		public new static readonly StringName ViewMap = "ViewMap";

		public static readonly StringName ClearEntryBroadcast = "ClearEntryBroadcast";

		public static readonly StringName SetPacketDrawOnly = "SetPacketDrawOnly";

		public static readonly StringName RestoreViewMapState = "RestoreViewMapState";

		public static readonly StringName PlayPacketBankAnimation = "PlayPacketBankAnimation";

		public new static readonly StringName CheckFinal = "CheckFinal";

		public static readonly StringName IsIZM2ObjectiveTarget = "IsIZM2ObjectiveTarget";

		public new static readonly StringName Finish = "Finish";

		public new static readonly StringName CanFinish = "CanFinish";

		public static readonly StringName PauseEntryCharacters = "PauseEntryCharacters";

		public static readonly StringName RestoreEntryCharacters = "RestoreEntryCharacters";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleProcess.PropertyName
	{
		public static readonly StringName waveFeature = "waveFeature";

		public static readonly StringName config = "config";

		public static readonly StringName progressFeature = "progressFeature";

		public static readonly StringName cameraFeature = "cameraFeature";

		public static readonly StringName seedBankFeature = "seedBankFeature";

		public static readonly StringName packetBankFeature = "packetBankFeature";

		public static readonly StringName mowerFeature = "mowerFeature";

		public static readonly StringName levelControl = "levelControl";

		public static readonly StringName _viewMapRunning = "_viewMapRunning";

		public static readonly StringName _entryBroadcastActive = "_entryBroadcastActive";

		public static readonly StringName _viewBroadcastActive = "_viewBroadcastActive";
	}

	public new class SignalName : TowerDefenseBattleProcess.SignalName
	{
	}

	public TowerDefenseBattleFeatureWave waveFeature;

	public TowerDefenseBattleProcessIZM2Config config;

	public TowerDefenseBattleFeatureProgress progressFeature;

	public TowerDefenseBattleFeatureCamera cameraFeature;

	public TowerDefenseBattleFeatureSeedBank seedBankFeature;

	public TowerDefenseBattleFeaturePacketBank packetBankFeature;

	public TowerDefenseBattleFeatureMower mowerFeature;

	public TowerDefenseInGameLevelControl levelControl;

	private readonly List<TowerDefenseInGamePacketShow> _viewPacketShows = new List<TowerDefenseInGamePacketShow>();

	private readonly System.Collections.Generic.Dictionary<Node, Node.ProcessModeEnum> _pausedEntryCharacters = new System.Collections.Generic.Dictionary<Node, Node.ProcessModeEnum>();

	private bool _viewMapRunning;

	private bool _entryBroadcastActive;

	private bool _viewBroadcastActive;

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseBattleProcessIZM2Config();
		config.Init(data);
	}

	public override void OnReady()
	{
	}

	public override void PhysicsProcess(double delta)
	{
		if ((Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || !GodotObject.IsInstanceValid(levelControl) || levelControl.awardCreate || waveFeature == null)
		{
			return;
		}
		waveFeature.WavePhysicsProcess(delta);
		if (levelControl.awardCreate)
		{
			return;
		}
		if (levelControl.hasSpawn)
		{
			levelControl.hasSpawn = false;
		}
		else
		{
			if (!waveFeature.IsFinalWaveSpawnSettled || !CanFinish() || !CheckFinal())
			{
				return;
			}
			if (!waveFeature.isSurvival)
			{
				levelControl.AwardCreate(levelControl.awardPos);
			}
			else if (waveFeature.survivalRunner.config.roundLimit == -1 || waveFeature.survivalRunner.config.roundLimit > waveFeature.survivalRunner.roundNum + 1)
			{
				Action persistProgress = null;
				if (Global.Instance.enterLevelMode != "OnlineLevel" && !Global.IsMultiplayerMode)
				{
					persistProgress = () =>
					{
						GameSaveManager.Instance.SaveLevelProgress(control.levelConfig.name, control.ModLevelIdentity);
					};
				}
				SurvivalRoundProgressTransaction.AdvanceAndPersist(waveFeature.survivalRunner, persistProgress, waveFeature.survivalRunner.BeginRoundTransition);
				if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
				{
					MultiPlayerManager.Instance.SendGameEntry(waveFeature.survivalRunner.roundNum);
				}
				control.GameEntry();
				levelControl.awardCreate = true;
			}
			else
			{
				GameSaveManager.Instance.DeleteLevelProgress(control.levelConfig.name, control.ModLevelIdentity);
				levelControl.AwardCreate(levelControl.awardPos);
			}
		}
	}

	public override void InputProcess(InputEvent event_)
	{
		if (CommandManager.Instance.debug && Input.IsActionJustPressed("DebugWaveNext"))
		{
			waveFeature.NextWave();
		}
		if (control != null && control.isView && Input.IsAnythingPressed())
		{
			control.EmitViewBack();
		}
	}

	public override Task GameInit()
	{
		ResolveDependencies();
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		ResolveDependencies();
		return Task.CompletedTask;
	}

	private void ResolveDependencies()
	{
		levelControl = control.levelControl;
		waveFeature = GetFeature<TowerDefenseBattleFeatureWave>("Wave");
		progressFeature = GetFeature<TowerDefenseBattleFeatureProgress>("Progress");
		cameraFeature = GetFeature<TowerDefenseBattleFeatureCamera>("Camera");
		seedBankFeature = GetFeature<TowerDefenseBattleFeatureSeedBank>("SeedBank");
		packetBankFeature = GetFeature<TowerDefenseBattleFeaturePacketBank>("PacketBank");
		mowerFeature = GetFeature<TowerDefenseBattleFeatureMower>("Mower");
	}

	public override async Task GameEntry()
	{
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(control))
		{
			return;
		}
		bool mobilePreset = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
		bool firstEntry = !control.isInit;
		if (control.hasProgress)
		{
			PauseEntryCharacters();
		}
		if (firstEntry && GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
		{
			seedBankFeature.seedBank.ReadyPackets();
		}
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(control))
		{
			RestoreEntryCharacters();
			return;
		}
		if (control.hasProgress)
		{
			if (GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
			{
				seedBankFeature.seedBank.packetSlotContainer.Visible = true;
			}
			if (mobilePreset)
			{
				control.uiTopAnimationPlayer.Play("MobileEnter");
			}
			else
			{
				control.uiTopAnimationPlayer.Play("Enter");
			}
			if (GodotObject.IsInstanceValid(packetBankFeature?.packetBank))
			{
				packetBankFeature.packetBank.Visible = false;
			}
			return;
		}
		if (firstEntry)
		{
			BroadCastConfig broadCastConfig = new BroadCastConfig();
			broadCastConfig.broadCastString = "更多的僵尸要来了！";
			if (GodotObject.IsInstanceValid(BroadCastManager.Instance))
			{
				BroadCastManager.Instance.BroadCastAdd(broadCastConfig);
				_entryBroadcastActive = true;
			}
			try
			{
				if (!(await WaitForTimerOrLifetime(config.entryBroadcastDuration)))
				{
					return;
				}
			}
			finally
			{
				ClearEntryBroadcast();
			}
			if (!GodotObject.IsInstanceValid(control))
			{
				return;
			}
			if (mobilePreset)
			{
				control.uiTopAnimationPlayer.Play("MobileExit");
			}
			else
			{
				control.uiTopAnimationPlayer.Play("Exit");
			}
			if (_pausedEntryCharacters.Count == 0)
			{
				PauseEntryCharacters();
			}
			if (GodotObject.IsInstanceValid(seedBankFeature?.seedBank) && GodotObject.IsInstanceValid(packetBankFeature?.packetBank) && !packetBankFeature.skipPacketChoose)
			{
				seedBankFeature.seedBank.Prepare();
			}
		}
		if (!GodotObject.IsInstanceValid(waveFeature) || !GodotObject.IsInstanceValid(levelControl) || !GodotObject.IsInstanceValid(cameraFeature?.cameraControl?.camera) || !GodotObject.IsInstanceValid(cameraFeature.cameraControl.cameraRightViewMarker) || !GodotObject.IsInstanceValid(cameraFeature.cameraControl.cameraPreViewMarker) || !GodotObject.IsInstanceValid(cameraFeature.cameraControl.cameraBeginMarker))
		{
			RestoreEntryCharacters();
			return;
		}
		if (GodotObject.IsInstanceValid(packetBankFeature?.packetBank))
		{
			packetBankFeature.packetBank.Visible = true;
			levelControl.worldEntryLabel.Visible = false;
		}
		waveFeature.Refresh();
		waveFeature.ShowCharacter();
		levelControl.worldEntryLabel.Visible = true;
		if (waveFeature.isSurvival)
		{
			levelControl.survivleLabel.Text = $"{waveFeature.survivalRunner.roundNum}轮完成";
			levelControl.survivleLabel.Visible = true;
		}
		RunLifetimeTask(HideEntryLabelsAfterDelayAsync, "HideEntryLabelsAfterDelayAsync");
		Tween tween = cameraFeature.cameraControl.camera.CreateTween();
		tween.SetEase(Tween.EaseType.InOut);
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(cameraFeature.cameraControl.camera, "global_position:x", cameraFeature.cameraControl.cameraRightViewMarker.GlobalPosition.X, config.cameraTravelDuration);
		if (!(await WaitForTweenOrLifetime(tween)))
		{
			return;
		}
		TaskCompletionSource<bool> tcs;
		if (GodotObject.IsInstanceValid(packetBankFeature?.packetBank) && !packetBankFeature.skipPacketChoose)
		{
			tween = cameraFeature.cameraControl.camera.CreateTween();
			tween.SetEase(Tween.EaseType.InOut);
			tween.SetTrans(Tween.TransitionType.Quart);
			tween.TweenProperty(cameraFeature.cameraControl.camera, "global_position:x", cameraFeature.cameraControl.cameraPreViewMarker.GlobalPosition.X, config.cameraTravelDuration);
			if (!(await WaitForTweenOrLifetime(tween)))
			{
				return;
			}
			control.buttonPause.Visible = true;
			if (GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
			{
				seedBankFeature.seedBank.packetSlotContainer.Visible = true;
			}
			if (mobilePreset)
			{
				packetBankFeature.packetBank.packetBankAnimationPlayer.Play("MobileEnter");
				control.uiTopAnimationPlayer.Play("MobileEnter");
			}
			else
			{
				packetBankFeature.packetBank.packetBankAnimationPlayer.Play("Enter");
				control.uiTopAnimationPlayer.Play("Enter");
			}
			TowerDefenseBattleFeaturePacketBank chooseFeature = packetBankFeature;
			tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
			chooseFeature.OnChooseOver += ChooseOverHandler;
			try
			{
				if (!(await WaitForTaskOrLifetime(tcs.Task)))
				{
					return;
				}
			}
			finally
			{
				if (GodotObject.IsInstanceValid(chooseFeature))
				{
					chooseFeature.OnChooseOver -= ChooseOverHandler;
				}
			}
			if (!IsLifetimeActive || !GodotObject.IsInstanceValid(control) || !GodotObject.IsInstanceValid(packetBankFeature?.packetBank))
			{
				return;
			}
			if (mobilePreset)
			{
				packetBankFeature.packetBank.packetBankAnimationPlayer.Play("MobileExit");
			}
			else
			{
				packetBankFeature.packetBank.packetBankAnimationPlayer.Play("Exit");
			}
			if (GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
			{
				seedBankFeature.seedBank.ReadyPackets();
			}
			if (!(await WaitForTimerOrLifetime(config.packetBankExitDelay)))
			{
				return;
			}
		}
		else
		{
			if (GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
			{
				seedBankFeature.seedBank.ReadyPackets();
			}
			if (!(await WaitForTimerOrLifetime(config.packetBankExitDelay)))
			{
				return;
			}
		}
		tween = cameraFeature.cameraControl.camera.CreateTween();
		tween.SetEase(Tween.EaseType.InOut);
		tween.SetTrans(Tween.TransitionType.Sine);
		tween.TweenProperty(cameraFeature.cameraControl.camera, "global_position:x", cameraFeature.cameraControl.cameraBeginMarker.GlobalPosition.X, config.cameraTravelDuration);
		if (!(await WaitForTweenOrLifetime(tween)))
		{
			return;
		}
		if (GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
		{
			seedBankFeature.seedBank.packetSlotContainer.Visible = true;
		}
		if (GodotObject.IsInstanceValid(packetBankFeature))
		{
			if (packetBankFeature.skipPacketChoose)
			{
				if (mobilePreset)
				{
					control.uiTopAnimationPlayer.Play("MobileEnter");
				}
				else
				{
					control.uiTopAnimationPlayer.Play("Enter");
				}
			}
		}
		else if (mobilePreset)
		{
			control.uiTopAnimationPlayer.Play("MobileEnter");
		}
		else
		{
			control.uiTopAnimationPlayer.Play("Enter");
		}
		if (firstEntry)
		{
			waveFeature.SurvivalReady();
			RestoreEntryCharacters();
		}
		void ChooseOverHandler()
		{
			tcs.TrySetResult(result: true);
		}
	}

	public override async Task GameReady()
	{
		if (!control.hasProgress && config.mowerUse && (!waveFeature.isSurvival || control.isInit))
		{
			mowerFeature.MowerInit();
		}
		mowerFeature.IZM2Init();
		waveFeature.ClearShowCharacter();
		progressFeature.SetDifficultVisible(visible: true);
		if (waveFeature.isSurvival)
		{
			progressFeature.SetSurvivalVisible(visible: true);
		}
		progressFeature.SetLevelNameVisible(visible: true);
		if (Global.Instance.enterLevelMode == "DailyLevel" || Global.Instance.enterLevelMode == "OnlineLevel" || Global.Instance.enterLevelMode == "LevelTest" || Global.Instance.enterLevelMode == "DiyLevel")
		{
			progressFeature.SetDifficultVisible(visible: false);
		}
		if (control.hasProgress)
		{
			waveFeature.readySetPlantOver = true;
			return;
		}
		if (seedBankFeature != null)
		{
			foreach (Node child in seedBankFeature.seedBank.packetContainer.GetChildren())
			{
				if (child is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow)
				{
					towerDefenseInGamePacketShow.onlyDraw = true;
				}
			}
		}
		if (await WaitForTaskOrLifetime(control.ReadySetPlantPlay()))
		{
			waveFeature.readySetPlantOver = true;
		}
	}

	public override Task GameStart()
	{
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		RestoreEntryCharacters();
		return Task.CompletedTask;
	}

	public override void GameFail(TowerDefenseCharacter enterCharacter)
	{
		control.ZombieWonLevelFail(playAnime: false);
	}

	public override void ZombieEnterHouse(TowerDefenseCharacter character)
	{
		if (!CommandManager.Instance.debug || !CommandManager.Instance.debugNoLose)
		{
			return;
		}
		character.CreateTween().TweenProperty(character.sprite, "meshColor:a", 0.0, config.debugEnterHouseFadeDuration).Finished += () =>
		{
			if (GodotObject.IsInstanceValid(character))
			{
				character.Destroy();
			}
		};
	}

	public override void ViewMap()
	{
		if (!_viewMapRunning && IsLifetimeActive)
		{
			_viewMapRunning = true;
			RunLifetimeTask(ViewMapAsync, "ViewMap");
		}
	}

	private async Task ViewMapAsync()
	{
		_ = 2;
		try
		{
			bool mobilePreset = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
			PlayPacketBankAnimation(mobilePreset ? "MobileExit" : "Exit");
			SetPacketDrawOnly();
			TowerDefenseCameraControl cameraControl = cameraFeature?.cameraControl;
			if (!GodotObject.IsInstanceValid(control) || !GodotObject.IsInstanceValid(cameraControl?.camera) || !GodotObject.IsInstanceValid(cameraControl.cameraBeginMarker))
			{
				return;
			}
			Tween tween = cameraControl.camera.CreateTween();
			tween.SetEase(Tween.EaseType.InOut);
			tween.SetTrans(Tween.TransitionType.Sine);
			tween.TweenProperty(cameraControl.camera, "global_position:x", cameraControl.cameraBeginMarker.GlobalPosition.X, config.cameraTravelDuration);
			control.isView = true;
			if (!(await WaitForTweenOrLifetime(tween)))
			{
				return;
			}
			if (GodotObject.IsInstanceValid(BroadCastManager.Instance))
			{
				BroadCastConfig broadCastConfig = new BroadCastConfig
				{
					broadCastString = "INGAME_VIEW_BACK"
				};
				BroadCastManager.Instance.BroadCastAdd(broadCastConfig);
				_viewBroadcastActive = true;
			}
			TowerDefenseControlNew viewControl = control;
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
			viewControl.OnViewBack += ViewBackHandler;
			try
			{
				if (!(await WaitForTaskOrLifetime(tcs.Task)))
				{
					return;
				}
			}
			finally
			{
				if (GodotObject.IsInstanceValid(viewControl))
				{
					viewControl.OnViewBack -= ViewBackHandler;
				}
			}
			if (IsLifetimeActive && GodotObject.IsInstanceValid(cameraControl?.camera) && GodotObject.IsInstanceValid(cameraControl.cameraPreViewMarker))
			{
				RestoreViewMapState();
				tween = cameraControl.camera.CreateTween();
				tween.SetEase(Tween.EaseType.InOut);
				tween.SetTrans(Tween.TransitionType.Sine);
				tween.TweenProperty(cameraControl.camera, "global_position:x", cameraControl.cameraPreViewMarker.GlobalPosition.X, config.cameraTravelDuration);
				if (await WaitForTweenOrLifetime(tween))
				{
					PlayPacketBankAnimation(mobilePreset ? "MobileEnter" : "Enter");
				}
			}
			void ViewBackHandler()
			{
				tcs.TrySetResult(result: true);
			}
		}
		finally
		{
			RestoreViewMapState();
			_viewMapRunning = false;
		}
	}

	private async Task HideEntryLabelsAfterDelayAsync()
	{
		if (await WaitForTimerOrLifetime(config.entryLabelDuration) && GodotObject.IsInstanceValid(levelControl))
		{
			levelControl.worldEntryLabel.Visible = false;
			if (GodotObject.IsInstanceValid(GetLevelControl()) && GodotObject.IsInstanceValid(waveFeature) && waveFeature.isSurvival)
			{
				levelControl.survivleLabel.Visible = false;
			}
		}
	}

	private void ClearEntryBroadcast()
	{
		if (_entryBroadcastActive && GodotObject.IsInstanceValid(BroadCastManager.Instance))
		{
			BroadCastManager.Instance.BraodCastClear();
		}
		_entryBroadcastActive = false;
	}

	private void SetPacketDrawOnly()
	{
		_viewPacketShows.Clear();
		if (!GodotObject.IsInstanceValid(seedBankFeature?.seedBank?.packetContainer))
		{
			return;
		}
		foreach (Node child in seedBankFeature.seedBank.packetContainer.GetChildren())
		{
			if (child is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow && GodotObject.IsInstanceValid(towerDefenseInGamePacketShow) && !towerDefenseInGamePacketShow.onlyDraw)
			{
				towerDefenseInGamePacketShow.onlyDraw = true;
				_viewPacketShows.Add(towerDefenseInGamePacketShow);
			}
		}
	}

	private void RestoreViewMapState()
	{
		foreach (TowerDefenseInGamePacketShow viewPacketShow in _viewPacketShows)
		{
			if (GodotObject.IsInstanceValid(viewPacketShow))
			{
				viewPacketShow.onlyDraw = false;
			}
		}
		_viewPacketShows.Clear();
		if (GodotObject.IsInstanceValid(control))
		{
			control.isView = false;
		}
		if (_viewBroadcastActive && GodotObject.IsInstanceValid(BroadCastManager.Instance))
		{
			BroadCastManager.Instance.BraodCastClear();
		}
		_viewBroadcastActive = false;
	}

	private void PlayPacketBankAnimation(StringName animationName)
	{
		if (GodotObject.IsInstanceValid(packetBankFeature?.packetBank?.packetBankAnimationPlayer))
		{
			packetBankFeature.packetBank.packetBankAnimationPlayer.Play(animationName);
		}
	}

	public override bool CheckFinal()
	{
		if (GodotObject.IsInstanceValid(control) && control.HasPendingBattleOperations)
		{
			return false;
		}
		TowerDefenseBattleCharacterRegistry towerDefenseBattleCharacterRegistry = TowerDefenseManager.Instance?.characterRegistry;
		if (!GodotObject.IsInstanceValid(towerDefenseBattleCharacterRegistry) || towerDefenseBattleCharacterRegistry.GetVaseCount() > 0)
		{
			return false;
		}
		TowerDefenseCharacter towerDefenseCharacter = null;
		List<TowerDefenseCharacter> cleanCharactersList = towerDefenseBattleCharacterRegistry.GetCleanCharactersList();
		for (int i = 0; i < cleanCharactersList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter2 = cleanCharactersList[i];
			if (IsIZM2ObjectiveTarget(towerDefenseCharacter2))
			{
				towerDefenseCharacter = towerDefenseCharacter2;
				break;
			}
		}
		if (GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			levelControl.awardPos = towerDefenseCharacter.GetLogicalGlobalPosition();
			return false;
		}
		foreach (Variant item in TowerDefenseManager.Instance.GetCampTarget(TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE))
		{
			((TowerDefenseCharacter)(GodotObject)item).Hurt(1000000000000.0);
		}
		return true;
	}

	private static bool IsIZM2ObjectiveTarget(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character) && character is TowerDefenseCrater towerDefenseCrater)
		{
			if (towerDefenseCrater.HasPendingRevival)
			{
				return towerDefenseCrater.RevivalCamp != TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
			}
			return false;
		}
		if (!GodotObject.IsInstanceValid(character) || character.die || character.nearDie || character.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE || character is TowerDefenseCrater || character is TowerDefenseItem || character is TowerDefenseGravestone)
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(character.instance) && character.instance.die)
		{
			return false;
		}
		if (character is TowerDefensePlant { config: TowerDefensePlantConfig towerDefensePlantConfig })
		{
			return !towerDefensePlantConfig.izm2Fliter;
		}
		return true;
	}

	public override void Finish()
	{
		if (!CanFinish() || levelControl.awardCreate)
		{
			return;
		}
		ViewManager.Instance.FullScreenColorBlink(Colors.White, 0.2, rise: false);
		AudioManager.Instance.AudioPlay("WaveHuge");
		Godot.Collections.Array campTarget = TowerDefenseManager.Instance.GetCampTarget(TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE);
		if (campTarget.Count > 0)
		{
			levelControl.awardPos = ((TowerDefenseCharacter)(GodotObject)campTarget[0]).GetLogicalGlobalPosition();
		}
		foreach (Variant item in campTarget)
		{
			((TowerDefenseCharacter)(GodotObject)item).Destroy();
		}
		levelControl.AwardCreate(levelControl.awardPos);
	}

	public override bool CanFinish()
	{
		if (base.CanFinish() && GodotObject.IsInstanceValid(waveFeature))
		{
			return !waveFeature.HasPendingSpawnOperations;
		}
		return false;
	}

	private void PauseEntryCharacters()
	{
		_pausedEntryCharacters.Clear();
		SceneTree tree = GetTree();
		if (!GodotObject.IsInstanceValid(tree))
		{
			return;
		}
		foreach (Node item in tree.GetNodesInGroup("Character"))
		{
			if (GodotObject.IsInstanceValid(item) && !_pausedEntryCharacters.ContainsKey(item))
			{
				_pausedEntryCharacters[item] = item.ProcessMode;
				item.ProcessMode = Node.ProcessModeEnum.Disabled;
			}
		}
	}

	private void RestoreEntryCharacters()
	{
		foreach (var (node2, processMode) in _pausedEntryCharacters)
		{
			if (GodotObject.IsInstanceValid(node2))
			{
				node2.ProcessMode = processMode;
			}
		}
		_pausedEntryCharacters.Clear();
	}

	public override void Destroy()
	{
		base.Destroy();
		ClearEntryBroadcast();
		RestoreEntryCharacters();
		if (_viewMapRunning || _viewBroadcastActive || _viewPacketShows.Count > 0)
		{
			RestoreViewMapState();
		}
		_viewMapRunning = false;
		if (GodotObject.IsInstanceValid(levelControl?.worldEntryLabel))
		{
			levelControl.worldEntryLabel.Visible = false;
		}
		if (GodotObject.IsInstanceValid(levelControl?.survivleLabel))
		{
			levelControl.survivleLabel.Visible = false;
		}
		waveFeature = null;
		config = null;
		progressFeature = null;
		cameraFeature = null;
		seedBankFeature = null;
		packetBankFeature = null;
		mowerFeature = null;
		levelControl = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InputProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveDependencies, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "enterCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ZombieEnterHouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ViewMap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearEntryBroadcast, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPacketDrawOnly, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreViewMapState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayPacketBankAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "animationName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckFinal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsIZM2ObjectiveTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanFinish, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PauseEntryCharacters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreEntryCharacters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName.PhysicsProcess && args.Count == 1)
		{
			PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InputProcess && args.Count == 1)
		{
			InputProcess(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDependencies && args.Count == 0)
		{
			ResolveDependencies();
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 1)
		{
			GameFail(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ZombieEnterHouse && args.Count == 1)
		{
			ZombieEnterHouse(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ViewMap && args.Count == 0)
		{
			ViewMap();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearEntryBroadcast && args.Count == 0)
		{
			ClearEntryBroadcast();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPacketDrawOnly && args.Count == 0)
		{
			SetPacketDrawOnly();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreViewMapState && args.Count == 0)
		{
			RestoreViewMapState();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayPacketBankAnimation && args.Count == 1)
		{
			PlayPacketBankAnimation(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckFinal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckFinal());
			return true;
		}
		if (method == MethodName.IsIZM2ObjectiveTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIZM2ObjectiveTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		if (method == MethodName.CanFinish && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanFinish());
			return true;
		}
		if (method == MethodName.PauseEntryCharacters && args.Count == 0)
		{
			PauseEntryCharacters();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreEntryCharacters && args.Count == 0)
		{
			RestoreEntryCharacters();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsIZM2ObjectiveTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIZM2ObjectiveTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.OnReady)
		{
			return true;
		}
		if (method == MethodName.PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.InputProcess)
		{
			return true;
		}
		if (method == MethodName.ResolveDependencies)
		{
			return true;
		}
		if (method == MethodName.GameFail)
		{
			return true;
		}
		if (method == MethodName.ZombieEnterHouse)
		{
			return true;
		}
		if (method == MethodName.ViewMap)
		{
			return true;
		}
		if (method == MethodName.ClearEntryBroadcast)
		{
			return true;
		}
		if (method == MethodName.SetPacketDrawOnly)
		{
			return true;
		}
		if (method == MethodName.RestoreViewMapState)
		{
			return true;
		}
		if (method == MethodName.PlayPacketBankAnimation)
		{
			return true;
		}
		if (method == MethodName.CheckFinal)
		{
			return true;
		}
		if (method == MethodName.IsIZM2ObjectiveTarget)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		if (method == MethodName.CanFinish)
		{
			return true;
		}
		if (method == MethodName.PauseEntryCharacters)
		{
			return true;
		}
		if (method == MethodName.RestoreEntryCharacters)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.waveFeature)
		{
			waveFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseBattleProcessIZM2Config>(in value);
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
		if (name == PropertyName._viewMapRunning)
		{
			_viewMapRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._entryBroadcastActive)
		{
			_entryBroadcastActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._viewBroadcastActive)
		{
			_viewBroadcastActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.waveFeature)
		{
			value = VariantUtils.CreateFrom(in waveFeature);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
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
		if (name == PropertyName._viewMapRunning)
		{
			value = VariantUtils.CreateFrom(in _viewMapRunning);
			return true;
		}
		if (name == PropertyName._entryBroadcastActive)
		{
			value = VariantUtils.CreateFrom(in _entryBroadcastActive);
			return true;
		}
		if (name == PropertyName._viewBroadcastActive)
		{
			value = VariantUtils.CreateFrom(in _viewBroadcastActive);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.waveFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.progressFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cameraFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.seedBankFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBankFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mowerFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._viewMapRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._entryBroadcastActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._viewBroadcastActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.waveFeature, Variant.From(in waveFeature));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.progressFeature, Variant.From(in progressFeature));
		info.AddProperty(PropertyName.cameraFeature, Variant.From(in cameraFeature));
		info.AddProperty(PropertyName.seedBankFeature, Variant.From(in seedBankFeature));
		info.AddProperty(PropertyName.packetBankFeature, Variant.From(in packetBankFeature));
		info.AddProperty(PropertyName.mowerFeature, Variant.From(in mowerFeature));
		info.AddProperty(PropertyName.levelControl, Variant.From(in levelControl));
		info.AddProperty(PropertyName._viewMapRunning, Variant.From(in _viewMapRunning));
		info.AddProperty(PropertyName._entryBroadcastActive, Variant.From(in _entryBroadcastActive));
		info.AddProperty(PropertyName._viewBroadcastActive, Variant.From(in _viewBroadcastActive));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.waveFeature, out var value))
		{
			waveFeature = value.As<TowerDefenseBattleFeatureWave>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseBattleProcessIZM2Config>();
		}
		if (info.TryGetProperty(PropertyName.progressFeature, out var value3))
		{
			progressFeature = value3.As<TowerDefenseBattleFeatureProgress>();
		}
		if (info.TryGetProperty(PropertyName.cameraFeature, out var value4))
		{
			cameraFeature = value4.As<TowerDefenseBattleFeatureCamera>();
		}
		if (info.TryGetProperty(PropertyName.seedBankFeature, out var value5))
		{
			seedBankFeature = value5.As<TowerDefenseBattleFeatureSeedBank>();
		}
		if (info.TryGetProperty(PropertyName.packetBankFeature, out var value6))
		{
			packetBankFeature = value6.As<TowerDefenseBattleFeaturePacketBank>();
		}
		if (info.TryGetProperty(PropertyName.mowerFeature, out var value7))
		{
			mowerFeature = value7.As<TowerDefenseBattleFeatureMower>();
		}
		if (info.TryGetProperty(PropertyName.levelControl, out var value8))
		{
			levelControl = value8.As<TowerDefenseInGameLevelControl>();
		}
		if (info.TryGetProperty(PropertyName._viewMapRunning, out var value9))
		{
			_viewMapRunning = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._entryBroadcastActive, out var value10))
		{
			_entryBroadcastActive = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._viewBroadcastActive, out var value11))
		{
			_viewBroadcastActive = value11.As<bool>();
		}
	}
}
