using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Process/IZM/TowerDefenseBattleProcessIZM.cs")]
public class TowerDefenseBattleProcessIZM : TowerDefenseBattleProcess
{
	public new class MethodName : TowerDefenseBattleProcess.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName OnReady = "OnReady";

		public static readonly StringName ResolveDependencies = "ResolveDependencies";

		public static readonly StringName BuildLineTopologyKey = "BuildLineTopologyKey";

		public new static readonly StringName GameFail = "GameFail";

		public new static readonly StringName ZombieEnterHouse = "ZombieEnterHouse";

		public static readonly StringName RemoveEnteredZombie = "RemoveEnteredZombie";

		public new static readonly StringName ViewMap = "ViewMap";

		public static readonly StringName SetPacketDrawOnly = "SetPacketDrawOnly";

		public static readonly StringName RestoreViewMapState = "RestoreViewMapState";

		public static readonly StringName PlayPacketBankAnimation = "PlayPacketBankAnimation";

		public new static readonly StringName InputProcess = "InputProcess";

		public new static readonly StringName CheckFinal = "CheckFinal";

		public new static readonly StringName CheckFail = "CheckFail";

		public new static readonly StringName Finish = "Finish";

		public new static readonly StringName PhysicsProcess = "PhysicsProcess";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleProcess.PropertyName
	{
		public static readonly StringName _nextFailureCheckFrame = "_nextFailureCheckFrame";

		public static readonly StringName _viewMapRunning = "_viewMapRunning";

		public static readonly StringName _viewBroadcastActive = "_viewBroadcastActive";

		public static readonly StringName config = "config";

		public static readonly StringName mapFeature = "mapFeature";

		public static readonly StringName progressFeature = "progressFeature";

		public static readonly StringName seedBankFeature = "seedBankFeature";

		public static readonly StringName packetBankFeature = "packetBankFeature";

		public static readonly StringName brainFeature = "brainFeature";

		public static readonly StringName conveyorBeltFeature = "conveyorBeltFeature";

		public static readonly StringName levelControl = "levelControl";
	}

	public new class SignalName : TowerDefenseBattleProcess.SignalName
	{
	}

	private ulong _nextFailureCheckFrame;

	private readonly List<TowerDefenseInGamePacketShow> _viewPacketShows = new List<TowerDefenseInGamePacketShow>();

	private bool _viewMapRunning;

	private bool _viewBroadcastActive;

	public TowerDefenseLevelIZMManagerConfig config;

	public TowerDefenseBattleFeatureMap mapFeature;

	public TowerDefenseBattleFeatureProgress progressFeature;

	public TowerDefenseBattleFeatureSeedBank seedBankFeature;

	public TowerDefenseBattleFeaturePacketBank packetBankFeature;

	public TowerDefenseBattleFeatureBrain brainFeature;

	public TowerDefenseBattleFeatureConveyorBelt conveyorBeltFeature;

	public TowerDefenseInGameLevelControl levelControl;

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseLevelIZMManagerConfig();
		config.Init(data);
	}

	public override void OnReady()
	{
	}

	private async Task SetupUIAsync()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (GodotObject.IsInstanceValid(control) && progressFeature != null)
		{
			string levelName = Tr(TowerDefenseManager.Instance.currentLevelConfig.levelName).Replace("{LevelNumber}", TowerDefenseManager.Instance.currentLevelConfig.levelNumber.ToString());
			progressFeature.SetLevelName(levelName);
			progressFeature.SetDifficultVisible(visible: false);
		}
	}

	public override Task GameInit()
	{
		ResolveDependencies();
		return SetupUIAsync();
	}

	public override Task GameInitFromProgress()
	{
		ResolveDependencies();
		return SetupUIAsync();
	}

	private void ResolveDependencies()
	{
		levelControl = control.levelControl;
		mapFeature = GetFeature<TowerDefenseBattleFeatureMap>("Map");
		progressFeature = GetFeature<TowerDefenseBattleFeatureProgress>("Progress");
		seedBankFeature = GetFeature<TowerDefenseBattleFeatureSeedBank>("SeedBank");
		packetBankFeature = GetFeature<TowerDefenseBattleFeaturePacketBank>("PacketBank");
		brainFeature = GetFeature<TowerDefenseBattleFeatureBrain>("Brain");
		conveyorBeltFeature = GetFeature<TowerDefenseBattleFeatureConveyorBelt>("ConveyorBelt");
	}

	public async Task<List<TowerDefenseCharacter>> Execute(IList<TowerDefenseLevelPreSpawnConfig> preSpawnlist)
	{
		if (!IsLifetimeActive || preSpawnlist == null)
		{
			return new List<TowerDefenseCharacter>();
		}
		List<TowerDefenseLevelPreSpawnConfig> list = new List<TowerDefenseLevelPreSpawnConfig>(preSpawnlist.Count);
		List<TowerDefenseCharacter> spawnedCharacters = new List<TowerDefenseCharacter>();
		List<TowerDefenseLevelPreSpawnConfig> list2 = new List<TowerDefenseLevelPreSpawnConfig>();
		foreach (TowerDefenseLevelPreSpawnConfig item in preSpawnlist)
		{
			if (!GodotObject.IsInstanceValid(item))
			{
				continue;
			}
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(item.packetName);
			if (!GodotObject.IsInstanceValid(packetConfig) || !GodotObject.IsInstanceValid(packetConfig.characterConfig))
			{
				continue;
			}
			if (packetConfig.characterConfig is TowerDefensePlantConfig)
			{
				TowerDefenseLevelPreSpawnConfig towerDefenseLevelPreSpawnConfig = item.Duplicate() as TowerDefenseLevelPreSpawnConfig;
				if (GodotObject.IsInstanceValid(towerDefenseLevelPreSpawnConfig))
				{
					list.Add(towerDefenseLevelPreSpawnConfig);
				}
			}
			else
			{
				list2.Add(item);
			}
		}
		int y = mapFeature.config.gridNum.Y;
		int x = mapFeature.config.gridNum.X;
		List<TowerDefenseLevelPreSpawnConfig>[][] array = new List<TowerDefenseLevelPreSpawnConfig>[y + 1][];
		for (int i = 0; i <= y; i++)
		{
			array[i] = new List<TowerDefenseLevelPreSpawnConfig>[x + 1];
			for (int j = 0; j <= x; j++)
			{
				array[i][j] = new List<TowerDefenseLevelPreSpawnConfig>();
			}
		}
		System.Collections.Generic.Dictionary<string, List<int>> dictionary = new System.Collections.Generic.Dictionary<string, List<int>>(StringComparer.Ordinal);
		for (int k = 1; k <= y; k++)
		{
			string key = BuildLineTopologyKey(k, x);
			if (!dictionary.TryGetValue(key, out var value))
			{
				value = new List<int>();
				dictionary.Add(key, value);
			}
			value.Add(k);
		}
		int[] array2 = new int[y + 1];
		array2[0] = -1;
		foreach (List<int> value4 in dictionary.Values)
		{
			List<int> list3 = new List<int>(value4);
			for (int num = list3.Count - 1; num > 0; num--)
			{
				int num2 = GD.RandRange(0, num);
				List<int> list4 = list3;
				int index = num;
				int index2 = num2;
				int value2 = list3[num2];
				int value3 = list3[num];
				list4[index] = value2;
				list3[index2] = value3;
			}
			for (int l = 0; l < value4.Count; l++)
			{
				array2[value4[l]] = list3[l];
			}
		}
		foreach (TowerDefenseLevelPreSpawnConfig item2 in list)
		{
			int x2 = item2.gridPos.X;
			int y2 = item2.gridPos.Y;
			if (x2 > 0 && x2 <= x && y2 > 0 && y2 <= y)
			{
				int num3 = array2[y2];
				array[num3][x2].Add(item2);
			}
		}
		for (int m = 1; m <= y; m++)
		{
			List<int> list5 = new List<int>();
			for (int n = 1; n <= x; n++)
			{
				foreach (TowerDefenseLevelPreSpawnConfig item3 in array[m][n])
				{
					_ = item3;
					list5.Add(n);
				}
			}
			if (list5.Count <= 0)
			{
				continue;
			}
			for (int num4 = 1; num4 <= x; num4++)
			{
				if (array[m][num4].Count > 0 && (double)GD.Randf() > 0.5)
				{
					int num5 = list5.PickRandom();
					List<TowerDefenseLevelPreSpawnConfig> list6 = array[m][num5];
					array[m][num5] = array[m][num4];
					array[m][num4] = list6;
				}
			}
		}
		for (int num6 = 1; num6 <= y; num6++)
		{
			for (int num7 = 1; num7 <= x; num7++)
			{
				foreach (TowerDefenseLevelPreSpawnConfig item4 in array[num6][num7])
				{
					item4.gridPos = new Vector2I(num7, num6);
				}
			}
		}
		foreach (TowerDefenseLevelPreSpawnConfig item5 in list2)
		{
			list.Add(item5);
		}
		List<TowerDefenseCharacter> list7 = spawnedCharacters;
		list7.AddRange(await SpawnPreSpawnCharactersWithRetry(list));
		return spawnedCharacters;
	}

	private static string BuildLineTopologyKey(int line, int columnCount)
	{
		StringBuilder stringBuilder = new StringBuilder(columnCount * 12);
		for (int i = 1; i <= columnCount; i++)
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(i, line));
			if (!GodotObject.IsInstanceValid(mapCell))
			{
				stringBuilder.Append("!;");
				continue;
			}
			int[] array = new int[mapCell.gridType.Count];
			for (int j = 0; j < mapCell.gridType.Count; j++)
			{
				array[j] = (int)mapCell.gridType[j];
			}
			System.Array.Sort(array);
			int[] array2 = array;
			foreach (int value in array2)
			{
				stringBuilder.Append(value).Append(',');
			}
			stringBuilder.Append(';');
		}
		return stringBuilder.ToString();
	}

	private async Task<List<TowerDefenseCharacter>> SpawnPreSpawnCharactersWithRetry(IList<TowerDefenseLevelPreSpawnConfig> preSpawnConfigs)
	{
		List<TowerDefenseCharacter> spawnedCharacters = new List<TowerDefenseCharacter>();
		List<TowerDefenseLevelPreSpawnConfig> pending = new List<TowerDefenseLevelPreSpawnConfig>(preSpawnConfigs);
		int retryPass = 0;
		while (IsLifetimeActive && pending.Count > 0 && retryPass < config.preSpawnMaxRetryPasses)
		{
			List<TowerDefenseLevelPreSpawnConfig> list = new List<TowerDefenseLevelPreSpawnConfig>();
			int num = 0;
			foreach (TowerDefenseLevelPreSpawnConfig item in pending)
			{
				TowerDefenseCharacter towerDefenseCharacter = item.SpawnCharacter();
				if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
				{
					list.Add(item);
					continue;
				}
				num++;
				spawnedCharacters.Add(towerDefenseCharacter);
				if (GodotObject.IsInstanceValid(item.characterOverride))
				{
					item.characterOverride.ExecuteCharacter(towerDefenseCharacter);
				}
			}
			pending = list;
			if (pending.Count == 0 || num == 0)
			{
				break;
			}
			retryPass++;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (!IsLifetimeActive)
			{
				break;
			}
		}
		return spawnedCharacters;
	}

	public override async Task GameEntry()
	{
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(control))
		{
			return;
		}
		bool mobilePreset = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
		TaskCompletionSource<bool> tcs;
		if (control.hasProgress)
		{
			if (seedBankFeature != null)
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
			if (packetBankFeature != null)
			{
				packetBankFeature.packetBank.Visible = false;
			}
		}
		else if (GodotObject.IsInstanceValid(packetBankFeature) && !packetBankFeature.skipPacketChoose && GodotObject.IsInstanceValid(packetBankFeature.packetBank))
		{
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
			control.buttonPause.Visible = true;
			if (seedBankFeature != null)
			{
				seedBankFeature.seedBank.packetSlotContainer.Visible = true;
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
			if (IsLifetimeActive && GodotObject.IsInstanceValid(packetBankFeature?.packetBank))
			{
				if (seedBankFeature != null)
				{
					seedBankFeature.seedBank.ReadyPackets();
				}
				if (mobilePreset)
				{
					packetBankFeature.packetBank.packetBankAnimationPlayer.Play("MobileExit");
				}
				else
				{
					packetBankFeature.packetBank.packetBankAnimationPlayer.Play("Exit");
				}
				await WaitForTimerOrLifetime(config.packetBankExitDelay);
			}
		}
		else
		{
			if (seedBankFeature != null)
			{
				seedBankFeature.seedBank.ReadyPackets();
			}
			if (mobilePreset)
			{
				control.uiTopAnimationPlayer.Play("MobileEnter");
			}
			else
			{
				control.uiTopAnimationPlayer.Play("Enter");
			}
		}
		void ChooseOverHandler()
		{
			tcs.TrySetResult(result: true);
		}
	}

	public override Task GameReady()
	{
		if (levelControl == null)
		{
			return Task.CompletedTask;
		}
		if (!control.hasProgress && brainFeature != null)
		{
			brainFeature.BrainInit();
		}
		if (progressFeature != null)
		{
			int num = brainFeature?.GetAliveBrainCount() ?? 0;
			progressFeature.SetProgressMeterMaxValue(num);
			progressFeature.SetProgressMeterWaveNum(num);
			progressFeature.SetProgressMeterPreviewWave(0);
			progressFeature.SetProgressMeterValue(0.0);
			progressFeature.SetLevelNameVisible(visible: true);
			if (Global.Instance.enterLevelMode == "DailyLevel" || Global.Instance.enterLevelMode == "OnlineLevel" || Global.Instance.enterLevelMode == "LevelTest" || Global.Instance.enterLevelMode == "DiyLevel")
			{
				progressFeature.SetDifficultVisible(visible: false);
			}
		}
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		if (progressFeature != null)
		{
			progressFeature.SetProgressMeterHideItem(hide: true);
			progressFeature.SetProgressMeterVisible(visible: true);
		}
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		if (progressFeature != null)
		{
			progressFeature.SetProgressMeterHideItem(hide: true);
			progressFeature.SetProgressMeterVisible(visible: true);
		}
		return Task.CompletedTask;
	}

	public override void GameFail(TowerDefenseCharacter enterCharacter)
	{
		control.ZombieWonLevelFail(playAnime: false);
	}

	public override void ZombieEnterHouse(TowerDefenseCharacter character)
	{
		if (TowerDefenseManager.HasGameplayAuthority && character is TowerDefenseZombie towerDefenseZombie && GodotObject.IsInstanceValid(towerDefenseZombie.instance) && !towerDefenseZombie.instance.hypnoses && brainFeature != null && brainFeature.TryGetAliveBrain(towerDefenseZombie.gridPos.Y, out var brain))
		{
			if (GodotObject.IsInstanceValid(AudioManager.Instance))
			{
				AudioManager.Instance.AudioPlay("Chomp");
			}
			brain.Destroy();
		}
		RemoveEnteredZombie(character);
	}

	private void RemoveEnteredZombie(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return;
		}
		character.CreateTween().TweenProperty(character.sprite, "meshColor:a", 0.0, config.enterHouseFadeDuration).Finished += () =>
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
		try
		{
			bool mobilePreset = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
			PlayPacketBankAnimation(mobilePreset ? "MobileExit" : "Exit");
			SetPacketDrawOnly();
			if (!GodotObject.IsInstanceValid(control))
			{
				return;
			}
			control.isView = true;
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
			if (IsLifetimeActive)
			{
				RestoreViewMapState();
				PlayPacketBankAnimation(mobilePreset ? "MobileEnter" : "Enter");
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

	public override void InputProcess(InputEvent event_)
	{
		if (control != null && control.isView && Input.IsAnythingPressed())
		{
			control.EmitViewBack();
		}
	}

	public override bool CheckFinal()
	{
		if (GodotObject.IsInstanceValid(control) && control.HasPendingBattleOperations)
		{
			return false;
		}
		if (levelControl == null)
		{
			return false;
		}
		if (brainFeature != null && brainFeature.TryGetFirstAliveBrain(out var brain))
		{
			levelControl.awardPos = brain.GetLogicalGlobalPosition();
			return false;
		}
		return true;
	}

	public override bool CheckFail()
	{
		if (GodotObject.IsInstanceValid(control) && control.HasPendingBattleOperations)
		{
			return false;
		}
		SceneTree tree = GetTree();
		if (!GodotObject.IsInstanceValid(tree))
		{
			return false;
		}
		if (tree.GetNodeCountInGroup("Sun") > 0 || tree.GetNodeCountInGroup("BrainSun") > 0 || tree.GetNodeCountInGroup("JalapenoSun") > 0 || tree.GetNodeCountInGroup("QXSun") > 0)
		{
			return false;
		}
		EconomyAccountId local = EconomyAccountId.Local;
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return false;
		}
		bool flag = true;
		if (GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
		{
			foreach (TowerDefenseInGamePacketShow packet in seedBankFeature.seedBank.packetList)
			{
				if (!packet.useCost || instance.CanAffordSun(local, packet.itemCost))
				{
					flag = false;
					break;
				}
			}
		}
		if (flag && GodotObject.IsInstanceValid(conveyorBeltFeature?.conveyorBeltManager))
		{
			if (conveyorBeltFeature.IsSunType())
			{
				foreach (Node packetChild in conveyorBeltFeature.conveyorBeltManager.GetPacketChildren(GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool()))
				{
					if (packetChild is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow && (!towerDefenseInGamePacketShow.useCost || instance.CanAffordSun(local, towerDefenseInGamePacketShow.itemCost)))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					foreach (TowerDefenseConveyorPacketConfig packet2 in conveyorBeltFeature.packetList)
					{
						TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packet2.name);
						if (GodotObject.IsInstanceValid(packetConfig) && instance.CanAffordSun(local, packetConfig.GetCost()))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					foreach (TowerDefenseLevelPacketConfig packetPrioritySpawn in conveyorBeltFeature.packetPrioritySpawnList)
					{
						TowerDefensePacketConfig packetConfig2 = TowerDefenseManager.GetPacketConfig(packetPrioritySpawn.packetName);
						if (GodotObject.IsInstanceValid(packetConfig2) && instance.CanAffordSun(local, packetConfig2.GetCost()))
						{
							flag = false;
							break;
						}
					}
				}
			}
			else if (conveyorBeltFeature.running || conveyorBeltFeature.conveyorBeltManager.GetPacketCount() > 0)
			{
				flag = false;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(instance.characterRegistry))
		{
			foreach (TowerDefenseCharacter cleanCharacters in instance.characterRegistry.GetCleanCharactersList())
			{
				if (cleanCharacters is TowerDefenseCrater { HasPendingRevival: not false, RevivalCamp: TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE })
				{
					return false;
				}
			}
		}
		foreach (Variant item in instance.GetCampFriendly(TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE))
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || !GodotObject.IsInstanceValid(towerDefenseCharacter.config) || towerDefenseCharacter.config.name != config.failureIgnoredZombieName)
			{
				return false;
			}
		}
		return true;
	}

	public override void Finish()
	{
		if (CanFinish() && levelControl != null && !levelControl.awardCreate)
		{
			ViewManager.Instance.FullScreenColorBlink(Colors.White, 0.2, rise: false);
			AudioManager.Instance.AudioPlay("WaveHuge");
			if (brainFeature != null && brainFeature.TryGetFirstAliveBrain(out var brain))
			{
				levelControl.awardPos = brain.GetLogicalGlobalPosition();
			}
			levelControl.AwardCreate(levelControl.awardPos);
		}
	}

	public override void PhysicsProcess(double delta)
	{
		if ((Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || levelControl == null)
		{
			return;
		}
		if (CanFinish() && CheckFinal())
		{
			levelControl.AwardCreate(levelControl.awardPos);
			return;
		}
		if (levelControl.hasSpawn)
		{
			levelControl.hasSpawn = false;
			return;
		}
		ulong physicsFrames = Engine.GetPhysicsFrames();
		if (physicsFrames >= _nextFailureCheckFrame)
		{
			_nextFailureCheckFrame = physicsFrames + (ulong)config.failureCheckIntervalFrames;
			if (CheckFail() && control != null)
			{
				control.GameFail(null);
			}
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		if (_viewMapRunning || _viewBroadcastActive || _viewPacketShows.Count > 0)
		{
			RestoreViewMapState();
		}
		_viewMapRunning = false;
		_nextFailureCheckFrame = 0uL;
		config = null;
		mapFeature = null;
		progressFeature = null;
		seedBankFeature = null;
		packetBankFeature = null;
		brainFeature = null;
		conveyorBeltFeature = null;
		levelControl = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveDependencies, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildLineTopologyKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "columnCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "enterCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ZombieEnterHouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveEnteredZombie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ViewMap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPacketDrawOnly, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreViewMapState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayPacketBankAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "animationName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InputProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.CheckFinal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckFail, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.ResolveDependencies && args.Count == 0)
		{
			ResolveDependencies();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildLineTopologyKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildLineTopologyKey(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
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
		if (method == MethodName.RemoveEnteredZombie && args.Count == 1)
		{
			RemoveEnteredZombie(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ViewMap && args.Count == 0)
		{
			ViewMap();
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
		if (method == MethodName.InputProcess && args.Count == 1)
		{
			InputProcess(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckFinal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckFinal());
			return true;
		}
		if (method == MethodName.CheckFail && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckFail());
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		if (method == MethodName.PhysicsProcess && args.Count == 1)
		{
			PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.BuildLineTopologyKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildLineTopologyKey(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
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
		if (method == MethodName.ResolveDependencies)
		{
			return true;
		}
		if (method == MethodName.BuildLineTopologyKey)
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
		if (method == MethodName.RemoveEnteredZombie)
		{
			return true;
		}
		if (method == MethodName.ViewMap)
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
		if (method == MethodName.InputProcess)
		{
			return true;
		}
		if (method == MethodName.CheckFinal)
		{
			return true;
		}
		if (method == MethodName.CheckFail)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		if (method == MethodName.PhysicsProcess)
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
		if (name == PropertyName._nextFailureCheckFrame)
		{
			_nextFailureCheckFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._viewMapRunning)
		{
			_viewMapRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._viewBroadcastActive)
		{
			_viewBroadcastActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseLevelIZMManagerConfig>(in value);
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
		if (name == PropertyName.brainFeature)
		{
			brainFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureBrain>(in value);
			return true;
		}
		if (name == PropertyName.conveyorBeltFeature)
		{
			conveyorBeltFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureConveyorBelt>(in value);
			return true;
		}
		if (name == PropertyName.levelControl)
		{
			levelControl = VariantUtils.ConvertTo<TowerDefenseInGameLevelControl>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._nextFailureCheckFrame)
		{
			value = VariantUtils.CreateFrom(in _nextFailureCheckFrame);
			return true;
		}
		if (name == PropertyName._viewMapRunning)
		{
			value = VariantUtils.CreateFrom(in _viewMapRunning);
			return true;
		}
		if (name == PropertyName._viewBroadcastActive)
		{
			value = VariantUtils.CreateFrom(in _viewBroadcastActive);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
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
		if (name == PropertyName.brainFeature)
		{
			value = VariantUtils.CreateFrom(in brainFeature);
			return true;
		}
		if (name == PropertyName.conveyorBeltFeature)
		{
			value = VariantUtils.CreateFrom(in conveyorBeltFeature);
			return true;
		}
		if (name == PropertyName.levelControl)
		{
			value = VariantUtils.CreateFrom(in levelControl);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._nextFailureCheckFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._viewMapRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._viewBroadcastActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.progressFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.seedBankFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBankFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.brainFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyorBeltFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._nextFailureCheckFrame, Variant.From(in _nextFailureCheckFrame));
		info.AddProperty(PropertyName._viewMapRunning, Variant.From(in _viewMapRunning));
		info.AddProperty(PropertyName._viewBroadcastActive, Variant.From(in _viewBroadcastActive));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.mapFeature, Variant.From(in mapFeature));
		info.AddProperty(PropertyName.progressFeature, Variant.From(in progressFeature));
		info.AddProperty(PropertyName.seedBankFeature, Variant.From(in seedBankFeature));
		info.AddProperty(PropertyName.packetBankFeature, Variant.From(in packetBankFeature));
		info.AddProperty(PropertyName.brainFeature, Variant.From(in brainFeature));
		info.AddProperty(PropertyName.conveyorBeltFeature, Variant.From(in conveyorBeltFeature));
		info.AddProperty(PropertyName.levelControl, Variant.From(in levelControl));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._nextFailureCheckFrame, out var value))
		{
			_nextFailureCheckFrame = value.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._viewMapRunning, out var value2))
		{
			_viewMapRunning = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._viewBroadcastActive, out var value3))
		{
			_viewBroadcastActive = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value4))
		{
			config = value4.As<TowerDefenseLevelIZMManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.mapFeature, out var value5))
		{
			mapFeature = value5.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName.progressFeature, out var value6))
		{
			progressFeature = value6.As<TowerDefenseBattleFeatureProgress>();
		}
		if (info.TryGetProperty(PropertyName.seedBankFeature, out var value7))
		{
			seedBankFeature = value7.As<TowerDefenseBattleFeatureSeedBank>();
		}
		if (info.TryGetProperty(PropertyName.packetBankFeature, out var value8))
		{
			packetBankFeature = value8.As<TowerDefenseBattleFeaturePacketBank>();
		}
		if (info.TryGetProperty(PropertyName.brainFeature, out var value9))
		{
			brainFeature = value9.As<TowerDefenseBattleFeatureBrain>();
		}
		if (info.TryGetProperty(PropertyName.conveyorBeltFeature, out var value10))
		{
			conveyorBeltFeature = value10.As<TowerDefenseBattleFeatureConveyorBelt>();
		}
		if (info.TryGetProperty(PropertyName.levelControl, out var value11))
		{
			levelControl = value11.As<TowerDefenseInGameLevelControl>();
		}
	}
}
