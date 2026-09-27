using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Process/Vase/TowerDefenseBattleProcessVase.cs")]
public class TowerDefenseBattleProcessVase : TowerDefenseBattleProcess, ITowerDefenseProgressSaveGuard
{
	public new class MethodName : TowerDefenseBattleProcess.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName BindFeatures = "BindFeatures";

		public static readonly StringName ClonePacketConfig = "ClonePacketConfig";

		public static readonly StringName ExportChangeCosts = "ExportChangeCosts";

		public static readonly StringName GetVaseType = "GetVaseType";

		public static readonly StringName GetVaseShellPacketConfig = "GetVaseShellPacketConfig";

		public static readonly StringName ApplyVaseContent = "ApplyVaseContent";

		public static readonly StringName IsVaseContentStateCurrent = "IsVaseContentStateCurrent";

		public static readonly StringName CreateSyncedVase = "CreateSyncedVase";

		public static readonly StringName RemoveSyncedVase = "RemoveSyncedVase";

		public new static readonly StringName GameFail = "GameFail";

		public new static readonly StringName ZombieEnterHouse = "ZombieEnterHouse";

		public new static readonly StringName ViewMap = "ViewMap";

		public static readonly StringName SetPacketDrawOnly = "SetPacketDrawOnly";

		public static readonly StringName RestoreViewMapState = "RestoreViewMapState";

		public static readonly StringName PlayPacketBankAnimation = "PlayPacketBankAnimation";

		public new static readonly StringName InputProcess = "InputProcess";

		public new static readonly StringName CheckFinal = "CheckFinal";

		public static readonly StringName IsIgnoredVaseObjectiveTarget = "IsIgnoredVaseObjectiveTarget";

		public new static readonly StringName Finish = "Finish";

		public new static readonly StringName PhysicsProcess = "PhysicsProcess";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public new static readonly StringName SaveProcess = "SaveProcess";

		public static readonly StringName ClearVasePacketShowsForLoad = "ClearVasePacketShowsForLoad";

		public new static readonly StringName LoadProcess = "LoadProcess";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleProcess.PropertyName
	{
		public static readonly StringName config = "config";

		public static readonly StringName mapFeature = "mapFeature";

		public static readonly StringName progressFeature = "progressFeature";

		public static readonly StringName seedBankFeature = "seedBankFeature";

		public static readonly StringName packetBankFeature = "packetBankFeature";

		public static readonly StringName mowerFeature = "mowerFeature";

		public static readonly StringName levelControl = "levelControl";

		public static readonly StringName _gameFailRunning = "_gameFailRunning";

		public static readonly StringName _viewMapRunning = "_viewMapRunning";
	}

	public new class SignalName : TowerDefenseBattleProcess.SignalName
	{
	}

	public TowerDefenseLevelVaseManagerConfig config;

	public TowerDefenseBattleFeatureMap mapFeature;

	public TowerDefenseBattleFeatureProgress progressFeature;

	public TowerDefenseBattleFeatureSeedBank seedBankFeature;

	public TowerDefenseBattleFeaturePacketBank packetBankFeature;

	public TowerDefenseBattleFeatureMower mowerFeature;

	public TowerDefenseInGameLevelControl levelControl;

	private readonly List<TowerDefenseInGamePacketShow> _viewPacketShows = new List<TowerDefenseInGamePacketShow>();

	private bool _gameFailRunning;

	private bool _viewMapRunning;

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseLevelVaseManagerConfig();
		config.Init(data);
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

	private void BindFeatures()
	{
		levelControl = control.levelControl;
		mapFeature = GetFeature<TowerDefenseBattleFeatureMap>("Map");
		progressFeature = GetFeature<TowerDefenseBattleFeatureProgress>("Progress");
		seedBankFeature = GetFeature<TowerDefenseBattleFeatureSeedBank>("SeedBank");
		packetBankFeature = GetFeature<TowerDefenseBattleFeaturePacketBank>("PacketBank");
		mowerFeature = GetFeature<TowerDefenseBattleFeatureMower>("Mower");
	}

	private void BuildFillPools(out List<TowerDefensePacketConfig> all, out List<TowerDefensePacketConfig> plants, out List<TowerDefensePacketConfig> zombies)
	{
		all = new List<TowerDefensePacketConfig>();
		plants = new List<TowerDefensePacketConfig>();
		zombies = new List<TowerDefensePacketConfig>();
		foreach (TowerDefenseLevelVaseFillConfig vaseFill in config.vaseFillList)
		{
			TowerDefensePacketConfig towerDefensePacketConfig = vaseFill?.GetPacket();
			if (GodotObject.IsInstanceValid(towerDefensePacketConfig) && GodotObject.IsInstanceValid(towerDefensePacketConfig.characterConfig))
			{
				all.Add(towerDefensePacketConfig);
				if (towerDefensePacketConfig.characterConfig is TowerDefensePlantConfig)
				{
					plants.Add(towerDefensePacketConfig);
				}
				else if (towerDefensePacketConfig.characterConfig is TowerDefenseZombieConfig)
				{
					zombies.Add(towerDefensePacketConfig);
				}
			}
		}
	}

	private static TowerDefensePacketConfig ClonePacketConfig(TowerDefensePacketConfig source)
	{
		if (!GodotObject.IsInstanceValid(source))
		{
			return null;
		}
		return source.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private static Godot.Collections.Array ExportChangeCosts(TowerDefensePacketConfig packetConfig)
	{
		return TowerDefensePacketRuntimeState.ExportChangeCosts(packetConfig);
	}

	private static bool TryCreateRuntimePacketConfig(string saveKey, Dictionary state, string overrideKey, string canChangeCostKey, string changeCostListKey, out TowerDefensePacketConfig packetConfig)
	{
		return TowerDefensePacketRuntimeState.TryCreate(saveKey, state, overrideKey, canChangeCostKey, changeCostListKey, out packetConfig);
	}

	private static string GetVaseType(TowerDefenseVase vase)
	{
		if (!GodotObject.IsInstanceValid(vase) || !GodotObject.IsInstanceValid(vase.config))
		{
			return "Normal";
		}
		string name = vase.config.name;
		if (!(name == "VasePlant"))
		{
			if (name == "VaseZombie")
			{
				return "Zombie";
			}
			return "Normal";
		}
		return "Plant";
	}

	private static TowerDefensePacketConfig GetVaseShellPacketConfig(string vaseType)
	{
		string packetName;
		if (vaseType == "Plant")
		{
			packetName = "VasePlant";
		}
		else
		{
			packetName = ((!(vaseType == "Zombie")) ? "VaseNormal" : "VaseZombie");
		}
		return TowerDefenseManager.GetPacketConfig(packetName);
	}

	private static void ApplyVaseContent(TowerDefenseVase vase, Dictionary state, string nameKey, string overrideKey, string canChangeCostKey, string changeCostListKey)
	{
		if (!GodotObject.IsInstanceValid(vase))
		{
			return;
		}
		string text = state.GetValueOrDefault(nameKey, "").AsString();
		if (!IsVaseContentStateCurrent(vase, text, state, overrideKey, canChangeCostKey, changeCostListKey))
		{
			if (!TryCreateRuntimePacketConfig(text, state, overrideKey, canChangeCostKey, changeCostListKey, out var packetConfig))
			{
				text = "";
				packetConfig = null;
			}
			vase.SetContentConfig(packetConfig);
		}
	}

	private static bool IsVaseContentStateCurrent(TowerDefenseVase vase, string contentName, Dictionary state, string overrideKey, string canChangeCostKey, string changeCostListKey)
	{
		TowerDefensePacketConfig packetConfig = vase.packetConfig;
		if (!string.Equals(GodotObject.IsInstanceValid(packetConfig) ? packetConfig.saveKey : "", contentName, StringComparison.Ordinal))
		{
			return false;
		}
		if (!state.ContainsKey(overrideKey) && !state.ContainsKey(canChangeCostKey) && !state.ContainsKey(changeCostListKey))
		{
			return true;
		}
		Dictionary dictionary = new Dictionary();
		Dictionary dictionary2 = new Dictionary();
		if (state.ContainsKey(overrideKey))
		{
			dictionary[overrideKey] = (GodotObject.IsInstanceValid(packetConfig?._override) ? packetConfig._override.Export() : new Dictionary());
			dictionary2[overrideKey] = state[overrideKey];
		}
		if (state.ContainsKey(canChangeCostKey))
		{
			dictionary[canChangeCostKey] = !GodotObject.IsInstanceValid(packetConfig) || packetConfig.canChangeCost;
			dictionary2[canChangeCostKey] = state[canChangeCostKey];
		}
		if (state.ContainsKey(changeCostListKey))
		{
			dictionary[changeCostListKey] = ExportChangeCosts(packetConfig);
			dictionary2[changeCostListKey] = state[changeCostListKey];
		}
		return NetworkVariantComparer.DictionaryApproxEquals(dictionary, dictionary2);
	}

	private TowerDefenseVase CreateSyncedVase(Vector2I gridPos, string vaseType, Dictionary state)
	{
		TowerDefensePacketConfig vaseShellPacketConfig = GetVaseShellPacketConfig(vaseType);
		if (!GodotObject.IsInstanceValid(vaseShellPacketConfig))
		{
			return null;
		}
		TowerDefenseVase towerDefenseVase = vaseShellPacketConfig.Plant(gridPos) as TowerDefenseVase;
		if (GodotObject.IsInstanceValid(towerDefenseVase))
		{
			ApplyVaseContent(towerDefenseVase, state, "content_name", "content_override", "content_can_change_cost", "content_change_cost_list");
		}
		return towerDefenseVase;
	}

	private void RemoveSyncedVase(TowerDefenseVase vase)
	{
		if (GodotObject.IsInstanceValid(vase))
		{
			control?.CleanupCharacterCell(vase);
			if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
			{
				TowerDefenseManager.Instance.CharacterUnregister(vase);
			}
			vase.RemoveFromGroup("Vase");
			vase.RemoveFromGroup("Character");
			if (!vase.IsQueuedForDeletion())
			{
				vase.QueueFree();
			}
		}
	}

	public override Task GameInit()
	{
		BindFeatures();
		Task result = SetupUIAsync();
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return result;
		}
		BuildFillPools(out var all, out var plants, out var zombies);
		if (!config.shuffle)
		{
			foreach (TowerDefenseLevelVaseConfig vase in config.vaseList)
			{
				if (!GodotObject.IsInstanceValid(vase))
				{
					continue;
				}
				string type = vase.type;
				TowerDefensePacketConfig towerDefensePacketConfig;
				if (type == "Plant")
				{
					towerDefensePacketConfig = TowerDefenseManager.GetPacketConfig("VasePlant");
				}
				else
				{
					towerDefensePacketConfig = ((!(type == "Zombie")) ? TowerDefenseManager.GetPacketConfig("VaseNormal") : TowerDefenseManager.GetPacketConfig("VaseZombie"));
				}
				if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
				{
					continue;
				}
				TowerDefenseVase towerDefenseVase = towerDefensePacketConfig.Plant(vase.gridPos) as TowerDefenseVase;
				if (!GodotObject.IsInstanceValid(towerDefenseVase))
				{
					continue;
				}
				if (vase.packetName != "")
				{
					towerDefenseVase.SetContentConfig(ClonePacketConfig(vase.GetPacket()));
					continue;
				}
				type = vase.type;
				List<TowerDefensePacketConfig> list;
				if (type == "Plant")
				{
					list = plants;
				}
				else
				{
					list = ((!(type == "Zombie")) ? all : zombies);
				}
				List<TowerDefensePacketConfig> list2 = list;
				if (list2.Count > 0)
				{
					towerDefenseVase.SetContentConfig(ClonePacketConfig(list2.PickRandom()));
				}
			}
		}
		else
		{
			List<Vector2I> list3 = new List<Vector2I>();
			List<string> list4 = new List<string>();
			List<TowerDefensePacketConfig> list5 = new List<TowerDefensePacketConfig>();
			foreach (TowerDefenseLevelVaseConfig vase2 in config.vaseList)
			{
				if (GodotObject.IsInstanceValid(vase2))
				{
					list3.Add(vase2.gridPos);
					list4.Add(vase2.type);
					if (vase2.packetName == "")
					{
						list5.Add((all.Count > 0) ? all.PickRandom() : null);
					}
					else
					{
						list5.Add(vase2.GetPacket());
					}
				}
			}
			list3.Shuffle();
			list4.Shuffle();
			list5.Shuffle();
			foreach (TowerDefensePacketConfig item in list5)
			{
				int index = list3.Count - 1;
				Vector2I gridPos = list3[index];
				list3.RemoveAt(index);
				string text = null;
				if (GodotObject.IsInstanceValid(item))
				{
					if (item.characterConfig is TowerDefensePlantConfig)
					{
						text = "Plant";
					}
					else if (item.characterConfig is TowerDefenseZombieConfig)
					{
						text = "Zombie";
					}
				}
				string text2;
				if (text != null && list4.Remove(text))
				{
					text2 = text;
				}
				else if (text != null && list4.Remove("Normal"))
				{
					text2 = "Normal";
				}
				else
				{
					if (list4.Count == 0)
					{
						break;
					}
					int index2 = list4.Count - 1;
					text2 = list4[index2];
					list4.RemoveAt(index2);
				}
				TowerDefensePacketConfig towerDefensePacketConfig2;
				if (text2 == "Plant")
				{
					towerDefensePacketConfig2 = TowerDefenseManager.GetPacketConfig("VasePlant");
				}
				else
				{
					towerDefensePacketConfig2 = ((!(text2 == "Zombie")) ? TowerDefenseManager.GetPacketConfig("VaseNormal") : TowerDefenseManager.GetPacketConfig("VaseZombie"));
				}
				if (GodotObject.IsInstanceValid(towerDefensePacketConfig2))
				{
					TowerDefenseVase towerDefenseVase2 = towerDefensePacketConfig2.Plant(gridPos) as TowerDefenseVase;
					if (GodotObject.IsInstanceValid(towerDefenseVase2))
					{
						towerDefenseVase2.SetContentConfig(ClonePacketConfig(item));
					}
				}
			}
		}
		return result;
	}

	public override Task GameInitFromProgress()
	{
		BindFeatures();
		return SetupUIAsync();
	}

	public override async Task GameEntry()
	{
		bool mobilePreset = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
		if (control.hasProgress)
		{
			if (seedBankFeature != null && GodotObject.IsInstanceValid(seedBankFeature.seedBank))
			{
				seedBankFeature.seedBank.packetSlotContainer.Visible = true;
				if (mobilePreset)
				{
					seedBankFeature.seedBank.animationPlayer?.Play("MobileEnter");
				}
				else
				{
					seedBankFeature.seedBank.animationPlayer?.Play("Enter");
				}
			}
			if (mobilePreset)
			{
				control.uiTopAnimationPlayer.Play("MobileEnter");
			}
			else
			{
				control.uiTopAnimationPlayer.Play("Enter");
			}
			return;
		}
		int packetBankMethod = (int)config.packetBankMethod;
		TaskCompletionSource<bool> tcs;
		if ((uint)(packetBankMethod - 1) <= 2u)
		{
			if (packetBankFeature != null && !packetBankFeature.skipPacketChoose && GodotObject.IsInstanceValid(packetBankFeature.packetBank) && seedBankFeature != null && GodotObject.IsInstanceValid(seedBankFeature.seedBank))
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
				seedBankFeature.seedBank.packetSlotContainer.Visible = true;
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
				if (!IsLifetimeActive || !GodotObject.IsInstanceValid(control) || !GodotObject.IsInstanceValid(packetBankFeature?.packetBank) || !GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
				{
					return;
				}
				seedBankFeature.seedBank.ReadyPackets();
				if (mobilePreset)
				{
					packetBankFeature.packetBank.packetBankAnimationPlayer.Play("MobileExit");
				}
				else
				{
					packetBankFeature.packetBank.packetBankAnimationPlayer.Play("Exit");
				}
				if (await WaitForTimerOrLifetime(config.packetBankExitDelay) && GodotObject.IsInstanceValid(control))
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
				return;
			}
			if (seedBankFeature != null && GodotObject.IsInstanceValid(seedBankFeature.seedBank))
			{
				seedBankFeature.seedBank.ReadyPackets();
				if (mobilePreset)
				{
					seedBankFeature.seedBank.animationPlayer?.Play("MobileEnter");
				}
				else
				{
					seedBankFeature.seedBank.animationPlayer?.Play("Enter");
				}
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
		else if (mobilePreset)
		{
			control.uiTopAnimationPlayer.Play("MobileEnter");
		}
		else
		{
			control.uiTopAnimationPlayer.Play("Enter");
		}
		void ChooseOverHandler()
		{
			tcs.TrySetResult(result: true);
		}
	}

	public override Task GameReady()
	{
		if (!control.hasProgress && config.mowerUse && mowerFeature != null)
		{
			mowerFeature.MowerInit();
		}
		if (progressFeature != null)
		{
			progressFeature.SetLevelNameVisible(visible: true);
			if (Global.Instance.enterLevelMode == "DailyLevel" || Global.Instance.enterLevelMode == "OnlineLevel" || Global.Instance.enterLevelMode == "LevelTest" || Global.Instance.enterLevelMode == "DiyLevel")
			{
				progressFeature.SetDifficultVisible(visible: false);
			}
		}
		return Task.CompletedTask;
	}

	public override void GameFail(TowerDefenseCharacter enterCharacter)
	{
		if (!_gameFailRunning && IsLifetimeActive)
		{
			_gameFailRunning = true;
			RunLifetimeTask(() => GameFailAsync(enterCharacter), "GameFail");
		}
	}

	private async Task GameFailAsync(TowerDefenseCharacter enterCharacter)
	{
		try
		{
			foreach (Variant item in TowerDefenseManager.Instance.GetCharacter())
			{
				TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
				if (towerDefenseCharacter != enterCharacter)
				{
					towerDefenseCharacter.ProcessMode = Node.ProcessModeEnum.Disabled;
				}
			}
			if (GodotObject.IsInstanceValid(packetBankFeature?.packetBank))
			{
				packetBankFeature.packetBank.Visible = false;
			}
			if (GodotObject.IsInstanceValid(control?.bankUILayer))
			{
				control.bankUILayer.Visible = false;
			}
			bool flag = GodotObject.IsInstanceValid(mapFeature?.currentMap);
			if (flag)
			{
				flag = !(await WaitForTaskOrLifetime(mapFeature.currentMap.EnterRoom(enterCharacter)));
			}
			if (!flag && IsLifetimeActive && GodotObject.IsInstanceValid(control))
			{
				if (GodotObject.IsInstanceValid(enterCharacter))
				{
					enterCharacter.ProcessMode = Node.ProcessModeEnum.Disabled;
				}
				control.ZombieWonLevelFail();
			}
		}
		finally
		{
			_gameFailRunning = false;
		}
	}

	public override void ZombieEnterHouse(TowerDefenseCharacter character)
	{
		if (CommandManager.Instance.debug && CommandManager.Instance.debugNoLose)
		{
			character.CreateTween().TweenProperty(character.sprite, "meshColor:a", 0.0, 1.0).Finished += () =>
			{
				if (GodotObject.IsInstanceValid(character))
				{
					character.Destroy();
				}
			};
		}
		else if (control != null)
		{
			control.GameFail(character);
		}
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
			BroadCastConfig broadCastConfig = new BroadCastConfig
			{
				broadCastString = "INGAME_VIEW_BACK"
			};
			BroadCastManager.Instance.BroadCastAdd(broadCastConfig);
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
		if (GodotObject.IsInstanceValid(BroadCastManager.Instance))
		{
			BroadCastManager.Instance.BraodCastClear();
		}
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
		if (!GodotObject.IsInstanceValid(levelControl))
		{
			return false;
		}
		if (!CanFinish())
		{
			return false;
		}
		SceneTree tree = GetTree();
		TowerDefenseBattleCharacterRegistry towerDefenseBattleCharacterRegistry = TowerDefenseManager.Instance?.characterRegistry;
		if (!GodotObject.IsInstanceValid(tree) || !GodotObject.IsInstanceValid(towerDefenseBattleCharacterRegistry) || towerDefenseBattleCharacterRegistry.GetVaseCount() > 0 || tree.GetNodeCountInGroup("Vase") > 0)
		{
			return false;
		}
		Godot.Collections.Array campTarget = TowerDefenseManager.Instance.GetCampTarget(TowerDefenseEnum.CHARACTER_CAMP.PLANT);
		foreach (Variant item in campTarget)
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
			if (!IsIgnoredVaseObjectiveTarget(towerDefenseCharacter))
			{
				levelControl.awardPos = towerDefenseCharacter.GetLogicalGlobalPosition();
				return false;
			}
		}
		foreach (Node item2 in tree.GetNodesInGroup("Zombie"))
		{
			if (item2 is TowerDefenseCharacter { isDestroy: not false, skipDestroySet: false })
			{
				return false;
			}
		}
		foreach (Variant item3 in campTarget)
		{
			TowerDefenseCharacter towerDefenseCharacter3 = (TowerDefenseCharacter)(GodotObject)item3;
			if (GodotObject.IsInstanceValid(towerDefenseCharacter3))
			{
				towerDefenseCharacter3.Destroy();
			}
		}
		return true;
	}

	private static bool IsIgnoredVaseObjectiveTarget(TowerDefenseCharacter target)
	{
		if (!GodotObject.IsInstanceValid(target))
		{
			return true;
		}
		if (GodotObject.IsInstanceValid(target.instance) && target.instance.die)
		{
			return true;
		}
		string text = (GodotObject.IsInstanceValid(target.config) ? target.config.name : "");
		if (!text.StartsWith("ZombieDigger", StringComparison.Ordinal))
		{
			return text.StartsWith("ZombieYetiDigger", StringComparison.Ordinal);
		}
		return true;
	}

	public override void Finish()
	{
		if (!CanFinish() || levelControl == null || levelControl.awardCreate)
		{
			return;
		}
		ViewManager.Instance.FullScreenColorBlink(Colors.White, 0.2, rise: false);
		AudioManager.Instance.AudioPlay("WaveHuge");
		Godot.Collections.Array campTarget = TowerDefenseManager.Instance.GetCampTarget(TowerDefenseEnum.CHARACTER_CAMP.PLANT);
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

	public override void PhysicsProcess(double delta)
	{
		if (GodotObject.IsInstanceValid(levelControl) && !levelControl.awardCreate)
		{
			if (levelControl.hasSpawn)
			{
				levelControl.hasSpawn = false;
			}
			else if ((!Global.IsMultiplayerMode || MultiPlayerManager.IsHost) && CanFinish() && CheckFinal())
			{
				levelControl.AwardCreate(levelControl.awardPos);
			}
		}
	}

	public bool CanSaveProgress(out string reason)
	{
		if (GodotObject.IsInstanceValid(levelControl) && levelControl.hasSpawn)
		{
			reason = "vase content is still spawning";
			return false;
		}
		SceneTree tree = GetTree();
		if (GodotObject.IsInstanceValid(tree))
		{
			foreach (Node item in tree.GetNodesInGroup("Vase"))
			{
				if (item is TowerDefenseVase towerDefenseVase && (towerDefenseVase.over || towerDefenseVase.isDestroy || towerDefenseVase.IsQueuedForDeletion()))
				{
					reason = "a vase is being opened";
					return false;
				}
			}
			foreach (Node item2 in tree.GetNodesInGroup("VasePacketShow"))
			{
				if (item2 is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow && (towerDefenseInGamePacketShow.select || towerDefenseInGamePacketShow.HasMeta("packet_pending_plant")))
				{
					reason = "a vase packet is being placed";
					return false;
				}
			}
		}
		reason = "";
		return true;
	}

	public override Dictionary SyncSerialize()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		Dictionary dictionary = new Dictionary();
		Godot.Collections.Array array2 = new Godot.Collections.Array();
		SceneTree tree = GetTree();
		if (!GodotObject.IsInstanceValid(tree))
		{
			return new Dictionary { { "vases", array } };
		}
		foreach (Node item in tree.GetNodesInGroup("Vase"))
		{
			if (!GodotObject.IsInstanceValid(item) || !(item is TowerDefenseVase { over: false, isDestroy: false } towerDefenseVase) || towerDefenseVase.IsQueuedForDeletion())
			{
				continue;
			}
			TowerDefensePacketConfig packetConfig = towerDefenseVase.packetConfig;
			Dictionary dictionary2 = (GodotObject.IsInstanceValid(packetConfig?._override) ? packetConfig._override.Export() : dictionary);
			Godot.Collections.Array array3;
			if (GodotObject.IsInstanceValid(packetConfig))
			{
				List<TowerDefensePacketChangeCost> changeCostList = packetConfig.changeCostList;
				if (changeCostList != null && changeCostList.Count > 0)
				{
					array3 = ExportChangeCosts(packetConfig);
					goto IL_00fc;
				}
			}
			array3 = array2;
			goto IL_00fc;
			IL_00fc:
			Godot.Collections.Array array4 = array3;
			array.Add(new Dictionary
			{
				{
					"grid_x",
					towerDefenseVase.gridPos.X
				},
				{
					"grid_y",
					towerDefenseVase.gridPos.Y
				},
				{
					"type",
					GetVaseType(towerDefenseVase)
				},
				{
					"content_name",
					GodotObject.IsInstanceValid(packetConfig) ? packetConfig.saveKey : ""
				},
				{ "content_override", dictionary2 },
				{
					"content_can_change_cost",
					!GodotObject.IsInstanceValid(packetConfig) || packetConfig.canChangeCost
				},
				{ "content_change_cost_list", array4 }
			});
		}
		return new Dictionary { { "vases", array } };
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		if (!_data.ContainsKey("vases"))
		{
			return;
		}
		SceneTree tree = GetTree();
		if (!GodotObject.IsInstanceValid(tree))
		{
			return;
		}
		System.Collections.Generic.Dictionary<Vector2I, Dictionary> dictionary = new System.Collections.Generic.Dictionary<Vector2I, Dictionary>();
		foreach (Variant item in _data["vases"].AsGodotArray())
		{
			Dictionary dictionary2 = item.AsGodotDictionary();
			int x = dictionary2.GetValueOrDefault("grid_x", 0).AsInt32();
			int y = dictionary2.GetValueOrDefault("grid_y", 0).AsInt32();
			dictionary[new Vector2I(x, y)] = dictionary2;
		}
		System.Collections.Generic.Dictionary<Vector2I, TowerDefenseVase> dictionary3 = new System.Collections.Generic.Dictionary<Vector2I, TowerDefenseVase>();
		foreach (Node item2 in tree.GetNodesInGroup("Vase"))
		{
			if (GodotObject.IsInstanceValid(item2) && item2 is TowerDefenseVase towerDefenseVase)
			{
				if (towerDefenseVase.over || towerDefenseVase.isDestroy || towerDefenseVase.IsQueuedForDeletion() || dictionary3.ContainsKey(towerDefenseVase.gridPos))
				{
					RemoveSyncedVase(towerDefenseVase);
				}
				else
				{
					dictionary3[towerDefenseVase.gridPos] = towerDefenseVase;
				}
			}
		}
		foreach (KeyValuePair<Vector2I, Dictionary> item3 in dictionary)
		{
			Vector2I key = item3.Key;
			Dictionary value = item3.Value;
			string text = value.GetValueOrDefault("type", "Normal").AsString();
			if (dictionary3.TryGetValue(key, out var value2))
			{
				dictionary3.Remove(key);
				if (GetVaseType(value2) == text)
				{
					ApplyVaseContent(value2, value, "content_name", "content_override", "content_can_change_cost", "content_change_cost_list");
					continue;
				}
				RemoveSyncedVase(value2);
			}
			CreateSyncedVase(key, text, value);
		}
		foreach (TowerDefenseVase value3 in dictionary3.Values)
		{
			RemoveSyncedVase(value3);
		}
	}

	public override Dictionary SaveProcess()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (Node item in GetTree().GetNodesInGroup("Vase"))
		{
			if (GodotObject.IsInstanceValid(item) && item is TowerDefenseVase { over: false, isDestroy: false } towerDefenseVase && !towerDefenseVase.IsQueuedForDeletion())
			{
				TowerDefensePacketConfig packetConfig = towerDefenseVase.packetConfig;
				array.Add(new Dictionary
				{
					{
						"nodeName",
						towerDefenseVase.Name.ToString().ValidateNodeName()
					},
					{
						"gridPosX",
						towerDefenseVase.gridPos.X
					},
					{
						"gridPosY",
						towerDefenseVase.gridPos.Y
					},
					{
						"contentName",
						GodotObject.IsInstanceValid(packetConfig) ? packetConfig.saveKey : ""
					},
					{
						"contentOverride",
						GodotObject.IsInstanceValid(packetConfig?._override) ? packetConfig._override.Export() : new Dictionary()
					},
					{
						"contentCanChangeCost",
						!GodotObject.IsInstanceValid(packetConfig) || packetConfig.canChangeCost
					},
					{
						"contentChangeCostList",
						ExportChangeCosts(packetConfig)
					}
				});
			}
		}
		Godot.Collections.Array array2 = new Godot.Collections.Array();
		foreach (Node item2 in GetTree().GetNodesInGroup("VasePacketShow"))
		{
			if (GodotObject.IsInstanceValid(item2) && item2 is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow && !towerDefenseInGamePacketShow.IsQueuedForDeletion() && towerDefenseInGamePacketShow.alive && GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.config))
			{
				Dictionary dictionary = new Dictionary();
				if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.moveComponent))
				{
					dictionary = towerDefenseInGamePacketShow.moveComponent.ExportComponentSave();
				}
				array2.Add(new Dictionary
				{
					{
						"saveKey",
						towerDefenseInGamePacketShow.config.saveKey
					},
					{
						"overrideSave",
						GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.config._override) ? towerDefenseInGamePacketShow.config._override.Export() : new Dictionary()
					},
					{
						"canChangeCost",
						towerDefenseInGamePacketShow.config.canChangeCost
					},
					{
						"changeCostList",
						ExportChangeCosts(towerDefenseInGamePacketShow.config)
					},
					{
						"posX",
						towerDefenseInGamePacketShow.GlobalPosition.X
					},
					{
						"posY",
						towerDefenseInGamePacketShow.GlobalPosition.Y
					},
					{ "zIndex", towerDefenseInGamePacketShow.ZIndex },
					{ "showLove", towerDefenseInGamePacketShow.showLove },
					{ "showCost", towerDefenseInGamePacketShow.showCost },
					{ "onlyDraw", towerDefenseInGamePacketShow.onlyDraw },
					{ "alive", towerDefenseInGamePacketShow.alive },
					{ "lock", towerDefenseInGamePacketShow.@lock },
					{ "plantOnce", towerDefenseInGamePacketShow.plantOnce },
					{ "useCost", towerDefenseInGamePacketShow.useCost },
					{ "openShadow", towerDefenseInGamePacketShow.openShadow },
					{ "start", towerDefenseInGamePacketShow.start },
					{ "select", towerDefenseInGamePacketShow.select },
					{ "coldDown", towerDefenseInGamePacketShow.coldDown },
					{ "coldDownOpen", towerDefenseInGamePacketShow.coldDownOpen },
					{ "coldDownTimer", towerDefenseInGamePacketShow.coldDownTimer },
					{ "aliveTime", towerDefenseInGamePacketShow.aliveTime },
					{ "aliveTimer", towerDefenseInGamePacketShow.aliveTimer },
					{ "blinkTimer", towerDefenseInGamePacketShow.blinkTimer },
					{ "blink", towerDefenseInGamePacketShow.blink },
					{ "height", towerDefenseInGamePacketShow.height },
					{
						"savePosX",
						towerDefenseInGamePacketShow.savePos.X
					},
					{
						"savePosY",
						towerDefenseInGamePacketShow.savePos.Y
					},
					{ "canPressPutBack", towerDefenseInGamePacketShow.canPressPutBack },
					{ "moveData", dictionary }
				});
			}
		}
		return new Dictionary
		{
			{ "vaseList", array },
			{ "packetShowList", array2 }
		};
	}

	private void ClearVasePacketShowsForLoad()
	{
		SceneTree tree = GetTree();
		if (!GodotObject.IsInstanceValid(tree))
		{
			return;
		}
		PacketPickControl packetPickControl = TowerDefenseManager.Instance?.GetPacketPickControl();
		foreach (Node item in tree.GetNodesInGroup("VasePacketShow"))
		{
			if (item is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow && GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				if (towerDefenseInGamePacketShow.HasMeta("packet_sync_id"))
				{
					int syncId = towerDefenseInGamePacketShow.GetMeta("packet_sync_id").AsInt32();
					control?.UnregisterSyncPacket(syncId);
					towerDefenseInGamePacketShow.RemoveMeta("packet_sync_id");
				}
				towerDefenseInGamePacketShow.RemoveMeta("packet_pending_plant");
				if (GodotObject.IsInstanceValid(packetPickControl) && packetPickControl.packetPick == towerDefenseInGamePacketShow)
				{
					packetPickControl.PacketPickRelease();
				}
				towerDefenseInGamePacketShow.ClearEventHandlers();
				towerDefenseInGamePacketShow.RemoveFromGroup("VasePacketShow");
				if (!towerDefenseInGamePacketShow.IsQueuedForDeletion())
				{
					towerDefenseInGamePacketShow.QueueFree();
				}
			}
		}
	}

	public override void LoadProcess(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		ClearVasePacketShowsForLoad();
		foreach (Variant item in _data.GetValueOrDefault("vaseList", new Godot.Collections.Array()).AsGodotArray())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			StringName key = dictionary.GetValueOrDefault("nodeName", "").AsStringName();
			TowerDefenseVase towerDefenseVase = null;
			if (_owner != null && _owner.charcterDicionary.ContainsKey(key))
			{
				towerDefenseVase = _owner.charcterDicionary[key] as TowerDefenseVase;
			}
			if (GodotObject.IsInstanceValid(towerDefenseVase))
			{
				ApplyVaseContent(towerDefenseVase, dictionary, "contentName", "contentOverride", "contentCanChangeCost", "contentChangeCostList");
			}
		}
		foreach (Variant item2 in _data.GetValueOrDefault("packetShowList", new Godot.Collections.Array()).AsGodotArray())
		{
			Dictionary dictionary2 = item2.AsGodotDictionary();
			if (!TryCreateRuntimePacketConfig(dictionary2.GetValueOrDefault("saveKey", "").AsString(), dictionary2, "overrideSave", "canChangeCost", "changeCostList", out var packetConfig) || !GodotObject.IsInstanceValid(packetConfig))
			{
				continue;
			}
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
			if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow) && GodotObject.IsInstanceValid(TowerDefenseGroundItemBase.characterNode))
			{
				towerDefenseInGamePacketShow.ZIndex = dictionary2.GetValueOrDefault("zIndex", 0).AsInt32();
				towerDefenseInGamePacketShow.GlobalPosition = new Vector2((float)dictionary2.GetValueOrDefault("posX", 0.0).AsDouble(), (float)dictionary2.GetValueOrDefault("posY", 0.0).AsDouble());
				TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, Node.InternalMode.Disabled);
				towerDefenseInGamePacketShow.Init(packetConfig);
				towerDefenseInGamePacketShow.StartInit();
				towerDefenseInGamePacketShow.showLove = dictionary2.GetValueOrDefault("showLove", false).AsBool();
				towerDefenseInGamePacketShow.showCost = dictionary2.GetValueOrDefault("showCost", false).AsBool();
				towerDefenseInGamePacketShow.onlyDraw = dictionary2.GetValueOrDefault("onlyDraw", false).AsBool();
				towerDefenseInGamePacketShow.alive = dictionary2.GetValueOrDefault("alive", true).AsBool();
				towerDefenseInGamePacketShow.@lock = dictionary2.GetValueOrDefault("lock", false).AsBool();
				towerDefenseInGamePacketShow.plantOnce = dictionary2.GetValueOrDefault("plantOnce", true).AsBool();
				towerDefenseInGamePacketShow.useCost = dictionary2.GetValueOrDefault("useCost", false).AsBool();
				towerDefenseInGamePacketShow.openShadow = dictionary2.GetValueOrDefault("openShadow", false).AsBool();
				towerDefenseInGamePacketShow.start = dictionary2.GetValueOrDefault("start", false).AsBool();
				towerDefenseInGamePacketShow.select = false;
				towerDefenseInGamePacketShow.coldDown = dictionary2.GetValueOrDefault("coldDown", towerDefenseInGamePacketShow.coldDown).AsDouble();
				towerDefenseInGamePacketShow.coldDownOpen = dictionary2.GetValueOrDefault("coldDownOpen", false).AsBool();
				towerDefenseInGamePacketShow.coldDownTimer = dictionary2.GetValueOrDefault("coldDownTimer", 0.0).AsDouble();
				towerDefenseInGamePacketShow.canPressPutBack = dictionary2.GetValueOrDefault("canPressPutBack", false).AsBool();
				towerDefenseInGamePacketShow.aliveTime = dictionary2.GetValueOrDefault("aliveTime", 15.0).AsDouble();
				towerDefenseInGamePacketShow.aliveTimer = dictionary2.GetValueOrDefault("aliveTimer", 0.0).AsDouble();
				towerDefenseInGamePacketShow.blinkTimer = dictionary2.GetValueOrDefault("blinkTimer", 0.0).AsDouble();
				towerDefenseInGamePacketShow.blink = dictionary2.GetValueOrDefault("blink", false).AsBool();
				towerDefenseInGamePacketShow.height = dictionary2.GetValueOrDefault("height", -1.0).AsDouble();
				towerDefenseInGamePacketShow.savePos = new Vector2((float)dictionary2.GetValueOrDefault("savePosX", 0.0).AsDouble(), (float)dictionary2.GetValueOrDefault("savePosY", 0.0).AsDouble());
				Dictionary dictionary3 = dictionary2.GetValueOrDefault("moveData", new Dictionary()).AsGodotDictionary();
				if (dictionary3.Count > 0 && GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.moveComponent))
				{
					towerDefenseInGamePacketShow.moveComponent.ImportComponentSave(dictionary3, _owner);
				}
				else if (dictionary3.Count == 0 && GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.moveComponent))
				{
					towerDefenseInGamePacketShow.moveComponent.QueueFree();
				}
				PacketPickControl packetPickControl = TowerDefenseManager.Instance.GetPacketPickControl();
				if (GodotObject.IsInstanceValid(packetPickControl))
				{
					towerDefenseInGamePacketShow.OnPressed += packetPickControl.PickPacket;
				}
				towerDefenseInGamePacketShow.AddToGroup("VasePacketShow");
			}
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		if (_viewMapRunning || _viewPacketShows.Count > 0)
		{
			RestoreViewMapState();
		}
		_gameFailRunning = false;
		_viewMapRunning = false;
		config = null;
		mapFeature = null;
		progressFeature = null;
		seedBankFeature = null;
		packetBankFeature = null;
		mowerFeature = null;
		levelControl = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(27)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindFeatures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClonePacketConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportChangeCosts, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetVaseType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "vase", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetVaseShellPacketConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "vaseType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyVaseContent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "vase", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "nameKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "overrideKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "canChangeCostKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "changeCostListKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsVaseContentStateCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "vase", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "contentName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "overrideKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "canChangeCostKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "changeCostListKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSyncedVase, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "vaseType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSyncedVase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "vase", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "enterCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ZombieEnterHouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
			new MethodInfo(MethodName.IsIgnoredVaseObjectiveTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveProcess, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearVasePacketShowsForLoad, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.BindFeatures && args.Count == 0)
		{
			BindFeatures();
			ret = default;
			return true;
		}
		if (method == MethodName.ClonePacketConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(ClonePacketConfig(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ExportChangeCosts && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(ExportChangeCosts(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetVaseType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetVaseType(VariantUtils.ConvertTo<TowerDefenseVase>(in args[0])));
			return true;
		}
		if (method == MethodName.GetVaseShellPacketConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(GetVaseShellPacketConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyVaseContent && args.Count == 6)
		{
			ApplyVaseContent(VariantUtils.ConvertTo<TowerDefenseVase>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsVaseContentStateCurrent && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<bool>(IsVaseContentStateCurrent(VariantUtils.ConvertTo<TowerDefenseVase>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Dictionary>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<string>(in args[5])));
			return true;
		}
		if (method == MethodName.CreateSyncedVase && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseVase>(CreateSyncedVase(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Dictionary>(in args[2])));
			return true;
		}
		if (method == MethodName.RemoveSyncedVase && args.Count == 1)
		{
			RemoveSyncedVase(VariantUtils.ConvertTo<TowerDefenseVase>(in args[0]));
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
		if (method == MethodName.IsIgnoredVaseObjectiveTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIgnoredVaseObjectiveTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
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
		if (method == MethodName.SaveProcess && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveProcess());
			return true;
		}
		if (method == MethodName.ClearVasePacketShowsForLoad && args.Count == 0)
		{
			ClearVasePacketShowsForLoad();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadProcess && args.Count == 2)
		{
			LoadProcess(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
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
		if (method == MethodName.ClonePacketConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(ClonePacketConfig(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ExportChangeCosts && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(ExportChangeCosts(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetVaseType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetVaseType(VariantUtils.ConvertTo<TowerDefenseVase>(in args[0])));
			return true;
		}
		if (method == MethodName.GetVaseShellPacketConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(GetVaseShellPacketConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyVaseContent && args.Count == 6)
		{
			ApplyVaseContent(VariantUtils.ConvertTo<TowerDefenseVase>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsVaseContentStateCurrent && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<bool>(IsVaseContentStateCurrent(VariantUtils.ConvertTo<TowerDefenseVase>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Dictionary>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<string>(in args[5])));
			return true;
		}
		if (method == MethodName.IsIgnoredVaseObjectiveTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIgnoredVaseObjectiveTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
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
		if (method == MethodName.BindFeatures)
		{
			return true;
		}
		if (method == MethodName.ClonePacketConfig)
		{
			return true;
		}
		if (method == MethodName.ExportChangeCosts)
		{
			return true;
		}
		if (method == MethodName.GetVaseType)
		{
			return true;
		}
		if (method == MethodName.GetVaseShellPacketConfig)
		{
			return true;
		}
		if (method == MethodName.ApplyVaseContent)
		{
			return true;
		}
		if (method == MethodName.IsVaseContentStateCurrent)
		{
			return true;
		}
		if (method == MethodName.CreateSyncedVase)
		{
			return true;
		}
		if (method == MethodName.RemoveSyncedVase)
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
		if (method == MethodName.IsIgnoredVaseObjectiveTarget)
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
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		if (method == MethodName.SaveProcess)
		{
			return true;
		}
		if (method == MethodName.ClearVasePacketShowsForLoad)
		{
			return true;
		}
		if (method == MethodName.LoadProcess)
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
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseLevelVaseManagerConfig>(in value);
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
		if (name == PropertyName._gameFailRunning)
		{
			_gameFailRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._viewMapRunning)
		{
			_viewMapRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
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
		if (name == PropertyName._gameFailRunning)
		{
			value = VariantUtils.CreateFrom(in _gameFailRunning);
			return true;
		}
		if (name == PropertyName._viewMapRunning)
		{
			value = VariantUtils.CreateFrom(in _viewMapRunning);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.progressFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.seedBankFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBankFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mowerFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gameFailRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._viewMapRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.mapFeature, Variant.From(in mapFeature));
		info.AddProperty(PropertyName.progressFeature, Variant.From(in progressFeature));
		info.AddProperty(PropertyName.seedBankFeature, Variant.From(in seedBankFeature));
		info.AddProperty(PropertyName.packetBankFeature, Variant.From(in packetBankFeature));
		info.AddProperty(PropertyName.mowerFeature, Variant.From(in mowerFeature));
		info.AddProperty(PropertyName.levelControl, Variant.From(in levelControl));
		info.AddProperty(PropertyName._gameFailRunning, Variant.From(in _gameFailRunning));
		info.AddProperty(PropertyName._viewMapRunning, Variant.From(in _viewMapRunning));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.config, out var value))
		{
			config = value.As<TowerDefenseLevelVaseManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.mapFeature, out var value2))
		{
			mapFeature = value2.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName.progressFeature, out var value3))
		{
			progressFeature = value3.As<TowerDefenseBattleFeatureProgress>();
		}
		if (info.TryGetProperty(PropertyName.seedBankFeature, out var value4))
		{
			seedBankFeature = value4.As<TowerDefenseBattleFeatureSeedBank>();
		}
		if (info.TryGetProperty(PropertyName.packetBankFeature, out var value5))
		{
			packetBankFeature = value5.As<TowerDefenseBattleFeaturePacketBank>();
		}
		if (info.TryGetProperty(PropertyName.mowerFeature, out var value6))
		{
			mowerFeature = value6.As<TowerDefenseBattleFeatureMower>();
		}
		if (info.TryGetProperty(PropertyName.levelControl, out var value7))
		{
			levelControl = value7.As<TowerDefenseInGameLevelControl>();
		}
		if (info.TryGetProperty(PropertyName._gameFailRunning, out var value8))
		{
			_gameFailRunning = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._viewMapRunning, out var value9))
		{
			_viewMapRunning = value9.As<bool>();
		}
	}
}
