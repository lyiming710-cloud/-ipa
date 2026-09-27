using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Battle/LevelEditorBattle.cs")]
public class LevelEditorBattle : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName _Show = "_Show";

		public static readonly StringName RandomLevel = "RandomLevel";

		public static readonly StringName TryRetryOrFail = "TryRetryOrFail";

		public static readonly StringName FreshFail = "FreshFail";

		public static readonly StringName FreshFinish = "FreshFinish";

		public static readonly StringName EnterLevel = "EnterLevel";

		public static readonly StringName LevelListGetHTTPRequestCompleted = "LevelListGetHTTPRequestCompleted";

		public static readonly StringName LevelDataGetHTTPRequestCompleted = "LevelDataGetHTTPRequestCompleted";

		public static readonly StringName LevelGetHTTPRequestCompleted = "LevelGetHTTPRequestCompleted";

		public static readonly StringName ToBattle = "ToBattle";

		public static readonly StringName RefreshButtonPressed = "RefreshButtonPressed";

		public static readonly StringName TipsButtonPressed = "TipsButtonPressed";

		public static readonly StringName RewardButtonPressed = "RewardButtonPressed";

		public static readonly StringName Over = "Over";

		public static readonly StringName RandomCustom = "RandomCustom";

		public static readonly StringName RandomPlant = "RandomPlant";

		public static readonly StringName HistoryButtonPressed = "HistoryButtonPressed";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName failCheckBox = "failCheckBox";

		public static readonly StringName mainNode = "mainNode";

		public static readonly StringName tipsNode = "tipsNode";

		public static readonly StringName awardNode = "awardNode";

		public static readonly StringName overNode = "overNode";

		public static readonly StringName historyNode = "historyNode";

		public static readonly StringName levelGetHttpRequest = "levelGetHttpRequest";

		public static readonly StringName levelDataGetHttpRequest = "levelDataGetHttpRequest";

		public static readonly StringName levelListGetHttpRequest = "levelListGetHttpRequest";

		public static readonly StringName levelEditorOnlineLevelItem = "levelEditorOnlineLevelItem";

		public static readonly StringName finishLabel = "finishLabel";

		public static readonly StringName ticketLabel = "ticketLabel";

		public static readonly StringName awardLabel = "awardLabel";

		public static readonly StringName rewardButton = "rewardButton";

		public static readonly StringName costomRewardTexture1 = "costomRewardTexture1";

		public static readonly StringName costomRewardTexture2 = "costomRewardTexture2";

		public static readonly StringName historyDragMenu = "historyDragMenu";

		public static readonly StringName historyNooneLabel = "historyNooneLabel";

		public static readonly StringName currentLevelId = "currentLevelId";

		public static readonly StringName currentLevelData = "currentLevelData";

		public static readonly StringName failNum = "failNum";

		public static readonly StringName finishNum = "finishNum";

		public static readonly StringName _refreshRetryCount = "_refreshRetryCount";

		public static readonly StringName _isPaidRefresh = "_isPaidRefresh";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static PackedScene _levelEditorOnlineLevelItemScene;

	private static Json _onlineLevelExchangeResource;

	private static Texture2D _arenaReward001;

	private static Texture2D _arenaReward002;

	private static Texture2D _arenaReward003;

	private static Texture2D _arenaReward004;

	private static Texture2D _arenaReward005;

	private CheckBox[] failCheckBox;

	public Control mainNode;

	public Control tipsNode;

	public Control awardNode;

	public Control overNode;

	public Control historyNode;

	private NativeHttpRequest levelGetHttpRequest;

	private NativeHttpRequest levelDataGetHttpRequest;

	private NativeHttpRequest levelListGetHttpRequest;

	private LevelEditorOnlineLevelItem levelEditorOnlineLevelItem;

	private Label finishLabel;

	private Label ticketLabel;

	private Label awardLabel;

	private SpriteBrightButton rewardButton;

	private Sprite2D costomRewardTexture1;

	private Sprite2D costomRewardTexture2;

	private DragMenu historyDragMenu;

	private Label historyNooneLabel;

	public static Array LevelList = new Array();

	public string currentLevelId = "";

	public Dictionary currentLevelData = new Dictionary();

	public int failNum;

	public int finishNum;

	private const int MaxRefreshRetry = 10;

	private int _refreshRetryCount;

	private bool _isPaidRefresh;

	private static readonly int[] CoinNumList = new int[13]
	{
		500, 1000, 2000, 3000, 5000, 8000, 12000, 16000, 20000, 30000,
		40000, 50000, 80000
	};

	private static readonly string[] AwardString = new string[13]
	{
		"奖励500金币", "奖励1000金币", "奖励2000金币", "奖励3000金币", "奖励5000金币", "奖励8000金币", "奖励12000金币", "奖励16000金币", "奖励20000金币", "奖励30000金币",
		"奖励40000金币+随机皮肤*1", "奖励50000金币+随机皮肤*1", "奖励80000金币+随机皮肤*2"
	};

	private static PackedScene LevelEditorOnlineLevelItemScene => _levelEditorOnlineLevelItemScene ?? (_levelEditorOnlineLevelItemScene = GD.Load<PackedScene>("uid://7tkumqfm20gv"));

	private static Json ONLINE_LEVEL_EXCHANGE_RESOURCE => _onlineLevelExchangeResource ?? (_onlineLevelExchangeResource = GD.Load<Json>("res://Asset/Config/OnlineLevelExchange/OnlineLevelExchangeResource.json"));

	private static Texture2D ArenaReward001 => _arenaReward001 ?? (_arenaReward001 = GD.Load<Texture2D>("uid://c6wk0iut0oxr2"));

	private static Texture2D ArenaReward002 => _arenaReward002 ?? (_arenaReward002 = GD.Load<Texture2D>("uid://camqjpt2tnw07"));

	private static Texture2D ArenaReward003 => _arenaReward003 ?? (_arenaReward003 = GD.Load<Texture2D>("uid://g0nshu8rrw67"));

	private static Texture2D ArenaReward004 => _arenaReward004 ?? (_arenaReward004 = GD.Load<Texture2D>("uid://oe6bi12ymuwh"));

	private static Texture2D ArenaReward005 => _arenaReward005 ?? (_arenaReward005 = GD.Load<Texture2D>("uid://ctlmif5y5srn"));

	public override void _Ready()
	{
		failCheckBox = new CheckBox[3]
		{
			GetNode<CheckBox>("%FailCheckBox"),
			GetNode<CheckBox>("%FailCheckBox2"),
			GetNode<CheckBox>("%FailCheckBox3")
		};
		mainNode = GetNode<Control>("%MainNode");
		tipsNode = GetNode<Control>("%TipsNode");
		awardNode = GetNode<Control>("%AwardNode");
		overNode = GetNode<Control>("%OverNode");
		historyNode = GetNode<Control>("%HistoryNode");
		levelGetHttpRequest = new NativeHttpRequest();
		levelDataGetHttpRequest = new NativeHttpRequest();
		levelListGetHttpRequest = new NativeHttpRequest();
		AddChild(levelGetHttpRequest, forceReadableName: false, InternalMode.Disabled);
		AddChild(levelDataGetHttpRequest, forceReadableName: false, InternalMode.Disabled);
		AddChild(levelListGetHttpRequest, forceReadableName: false, InternalMode.Disabled);
		levelEditorOnlineLevelItem = GetNode<LevelEditorOnlineLevelItem>("%LevelEditorOnlineLevelItem");
		levelEditorOnlineLevelItem.OnSelect += EnterLevel;
		finishLabel = GetNode<Label>("%FinishLabel");
		ticketLabel = GetNode<Label>("%TicketLabel");
		awardLabel = GetNode<Label>("%AwardLabel");
		rewardButton = GetNode<SpriteBrightButton>("%RewardButton");
		costomRewardTexture1 = GetNode<Sprite2D>("%CostomRewardTexture1");
		costomRewardTexture2 = GetNode<Sprite2D>("%CostomRewardTexture2");
		historyDragMenu = GetNode<DragMenu>("%HistoryDragMenu");
		historyNooneLabel = GetNode<Label>("%HistoryNooneLabel");
		levelListGetHttpRequest.RequestCompleted += LevelListGetHTTPRequestCompleted;
		levelDataGetHttpRequest.RequestCompleted += LevelDataGetHTTPRequestCompleted;
		levelGetHttpRequest.RequestCompleted += LevelGetHTTPRequestCompleted;
		GetNode<TextureButton>("MainNode/LevelEditorOnlineLevelItem/RefreshButton").Pressed += RefreshButtonPressed;
		GetNode<TextureButton>("MainNode/TipsButton").Pressed += TipsButtonPressed;
		rewardButton.OnPressed += RewardButtonPressed;
		GetNode<SpriteBrightButton>("MainNode/HistoryButton").OnPressed += HistoryButtonPressed;
	}

	public async void _Show()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		currentLevelId = GameSaveManager.Instance.GetKeyValue("LevelEditorBattleCurrentLevel").AsString();
		failNum = GameSaveManager.Instance.GetKeyValue("LevelEditorBattleFailNum").AsInt32();
		finishNum = GameSaveManager.Instance.GetKeyValue("LevelEditorBattleFinishNum").AsInt32();
		FreshFinish(finishNum);
		FreshFail(failNum);
		if (currentLevelId == "-1")
		{
			RandomLevel();
		}
		else
		{
			levelDataGetHttpRequest.Request("https://api.pvzhe.com/workshop/levels?include_id=" + currentLevelId, Global.Instance.header);
		}
	}

	public void RandomLevel()
	{
		levelDataGetHttpRequest.Request("https://api.pvzhe.com/workshop/levels/ids?feature=comp&random=1", Global.Instance.header);
	}

	private void TryRetryOrFail()
	{
		_refreshRetryCount++;
		if (_refreshRetryCount < 10)
		{
			RandomLevel();
			return;
		}
		if (_isPaidRefresh)
		{
			TowerDefenseManager.Instance.AddCoin(1000L);
			_isPaidRefresh = false;
		}
		_refreshRetryCount = 0;
		DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]网络不好,请重新刷新[/font_size][/center]");
	}

	public void FreshFail(int num)
	{
		switch (num)
		{
		case 0:
			failCheckBox[0].ButtonPressed = false;
			failCheckBox[1].ButtonPressed = false;
			failCheckBox[2].ButtonPressed = false;
			break;
		case 1:
			failCheckBox[0].ButtonPressed = true;
			failCheckBox[1].ButtonPressed = false;
			failCheckBox[2].ButtonPressed = false;
			break;
		case 2:
			failCheckBox[0].ButtonPressed = true;
			failCheckBox[1].ButtonPressed = true;
			failCheckBox[2].ButtonPressed = false;
			break;
		case 3:
			failCheckBox[0].ButtonPressed = true;
			failCheckBox[1].ButtonPressed = true;
			failCheckBox[2].ButtonPressed = true;
			Over(fail: true);
			break;
		}
	}

	public void FreshFinish(int num)
	{
		finishLabel.Text = $"胜场:{num}";
		costomRewardTexture1.Visible = false;
		costomRewardTexture2.Visible = false;
		switch (num)
		{
		case 0:
		case 1:
		case 2:
			rewardButton.Texture = ArenaReward001;
			break;
		case 3:
		case 4:
		case 5:
			rewardButton.Texture = ArenaReward002;
			break;
		case 6:
		case 7:
		case 8:
			rewardButton.Texture = ArenaReward003;
			break;
		case 9:
			rewardButton.Texture = ArenaReward004;
			break;
		case 10:
		case 11:
			rewardButton.Texture = ArenaReward004;
			costomRewardTexture1.Visible = true;
			break;
		case 12:
			rewardButton.Texture = ArenaReward005;
			costomRewardTexture1.Visible = true;
			costomRewardTexture2.Visible = true;
			break;
		default:
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]数据错误[/font_size][/center]");
			break;
		}
		switch (num)
		{
		case 0:
			awardLabel.Text = $"奖池：{500}金币";
			break;
		case 1:
			awardLabel.Text = $"奖池：{1000}金币";
			break;
		case 2:
			awardLabel.Text = $"奖池：{2000}金币";
			break;
		case 3:
			awardLabel.Text = $"奖池：{3000}金币";
			break;
		case 4:
			awardLabel.Text = $"奖池：{5000}金币";
			break;
		case 5:
			awardLabel.Text = $"奖池：{8000}金币";
			break;
		case 6:
			awardLabel.Text = $"奖池：{12000}金币";
			break;
		case 7:
			awardLabel.Text = $"奖池：{16000}金币";
			break;
		case 8:
			awardLabel.Text = $"奖池：{20000}金币";
			break;
		case 9:
			awardLabel.Text = $"奖池：{30000}金币";
			break;
		case 10:
			awardLabel.Text = $"奖池：{40000}金币+随机皮肤*1";
			break;
		case 11:
			awardLabel.Text = $"奖池：{50000}金币+随机皮肤*1";
			break;
		case 12:
			awardLabel.Text = $"奖池：{80000}金币+随机皮肤*2";
			Over(fail: false);
			break;
		default:
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]数据错误[/font_size][/center]");
			break;
		}
	}

	public void EnterLevel(string url, string id)
	{
		currentLevelId = id;
		if (url != "")
		{
			string url2 = "https://api.pvzhe.com" + url;
			levelGetHttpRequest.Request(url2, Global.Instance.header);
		}
	}

	public void LevelListGetHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != 0L)
		{
			BroadCastManager.Instance.BroadCastFloatCreate(Global.Instance.GetHTTPRequestErrorMessage(result), Colors.Red);
			return;
		}
		Json json = new Json();
		json.Parse(body.GetStringFromUtf8());
		if (json.Data.VariantType != Variant.Type.Nil)
		{
			LevelList = json.Data.AsGodotArray();
			if (currentLevelId == "-1" && LevelList.Count > 0)
			{
				RandomLevel();
			}
		}
	}

	public void LevelDataGetHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != 0L)
		{
			BroadCastManager.Instance.BroadCastFloatCreate(Global.Instance.GetHTTPRequestErrorMessage(result), Colors.Red);
			return;
		}
		Json json = new Json();
		json.Parse(body.GetStringFromUtf8());
		if (json.Data.VariantType == Variant.Type.Nil)
		{
			return;
		}
		if (json.Data.VariantType == Variant.Type.Dictionary)
		{
			Dictionary dictionary = json.Data.AsGodotDictionary();
			if (dictionary.ContainsKey("list") && dictionary["list"].AsGodotArray().Count > 0)
			{
				currentLevelData = dictionary["list"].AsGodotArray()[0].AsGodotDictionary();
				levelEditorOnlineLevelItem.Call("Init", currentLevelData);
				_refreshRetryCount = 0;
				_isPaidRefresh = false;
			}
			else
			{
				TryRetryOrFail();
			}
		}
		if (json.Data.VariantType == Variant.Type.Array)
		{
			string text = ((int)json.Data.AsGodotArray()[0].AsDouble()).ToString();
			string key = "OnlineLevel-" + text;
			Dictionary levelValue = GameSaveManager.Instance.GetLevelValue(key);
			Dictionary dictionary2 = (levelValue.ContainsKey("Key") ? levelValue["Key"].AsGodotDictionary() : new Dictionary());
			if (!dictionary2.ContainsKey("Finish"))
			{
				dictionary2["Finish"] = 0;
			}
			if (dictionary2["Finish"].AsInt32() <= 0)
			{
				currentLevelId = text;
				GameSaveManager.Instance.SetKeyValue("LevelEditorBattleCurrentLevel", currentLevelId);
				levelDataGetHttpRequest.Request("https://api.pvzhe.com/workshop/levels?include_id=" + currentLevelId, Global.Instance.header);
			}
			else
			{
				TryRetryOrFail();
			}
		}
	}

	public void LevelGetHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != 0L)
		{
			BroadCastManager.Instance.BroadCastFloatCreate(Global.Instance.GetHTTPRequestErrorMessage(result), Colors.Red);
			return;
		}
		string text = "OnlineLevel-" + currentLevelId;
		Json json = new Json();
		json.Parse(body.GetStringFromUtf8());
		if (json.Data.VariantType != Variant.Type.Nil)
		{
			Dictionary dictionary = json.Data.AsGodotDictionary();
			if (dictionary.ContainsKey("error") && dictionary["error"].AsBool())
			{
				BroadCastManager.Instance.BroadCastFloatCreate("关卡Id无效", Colors.Red);
				return;
			}
			dictionary["Name"] = text;
			dictionary["Reward"] = new Dictionary();
			dictionary["Reward"].AsGodotDictionary()["RewardType"] = "Coin";
			dictionary["Reward"].AsGodotDictionary()["RewardFirst"] = "1000";
			Json json2 = new Json();
			json2.Parse(Json.Stringify(dictionary));
			GameSaveManager.Instance.SaveOnlineLevel(text, json2);
			ToBattle(json2);
		}
	}

	public async void ToBattle(Json json)
	{
		Dictionary dictionary = GameSaveManager.Instance.GetKeyValue("OnlinehBattleHistory").AsGodotDictionary();
		if (!dictionary.ContainsKey("Level"))
		{
			dictionary["Level"] = new Array();
		}
		Array array = dictionary["Level"].AsGodotArray();
		if (array.Count > 20)
		{
			array.RemoveAt(array.Count - 1);
		}
		array.Insert(0, new Dictionary
		{
			["id"] = currentLevelData.GetValueOrDefault("id", ""),
			["name"] = currentLevelData.GetValueOrDefault("name", ""),
			["map"] = currentLevelData.GetValueOrDefault("map", "")
		});
		GameSaveManager.Instance.SetKeyValue("OnlinehBattleHistory", dictionary);
		GameSaveManager.Instance.Save();
		TowerDefenseLevelConfig towerDefenseLevelConfig = new TowerDefenseLevelConfig();
		towerDefenseLevelConfig.data = json;
		towerDefenseLevelConfig.Init();
		TowerDefenseManager.Instance.currentLevelConfig = towerDefenseLevelConfig;
		Global.Instance.enterLevelMode = "OnlineLevel";
		Global.Instance.enterLevelIsBattle = true;
		if (Global.Instance.isMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			MultiPlayerManager.Instance.selectedLevelId = "";
			if (await MultiPlayerManager.Instance.StartCompatibleGameAsync())
			{
				TowerDefenseManager.Instance.UseCoin(500L);
			}
		}
		else
		{
			TowerDefenseManager.Instance.UseCoin(500L);
			SceneManager.Instance.ChangeScene("TowerDefense");
		}
	}

	public void RefreshButtonPressed()
	{
		if (TowerDefenseManager.Instance.GetCoin() < 1000)
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]您的金币数量不足1000\n无法刷新关卡[/font_size][/center]");
			return;
		}
		DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("DialogBoxChoose");
		dialogBoxBase.Set("text", "[center][font_size=24]刷新关卡[/font_size][/center]\n[center][font_size=16]是否花费1000金币刷新关卡？[/font_size][/center]");
		((DialogBoxChoose)dialogBoxBase).OnChooseTrue += () =>
		{
			_refreshRetryCount = 0;
			_isPaidRefresh = true;
			RandomLevel();
			TowerDefenseManager.Instance.UseCoin(1000L);
		};
	}

	public void TipsButtonPressed()
	{
		TowerDefenseManager.Instance.coinBank.CallDeferred("StartHide");
		mainNode.Visible = false;
		tipsNode.Visible = true;
	}

	public void RewardButtonPressed()
	{
		TowerDefenseManager.Instance.coinBank.CallDeferred("StartHide");
		mainNode.Visible = false;
		awardNode.Visible = true;
	}

	public async void Over(bool fail)
	{
		overNode.Visible = true;
		Tween tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(rewardButton, "global_position", new Vector2(410f, 204f), 1.0);
		await ToSignal(tween, Tween.SignalName.Finished);
		string text = "";
		int num = CoinNumList[finishNum];
		string value = ((!fail) ? "闯关成功" : "失败到达上限");
		switch (finishNum)
		{
		case 10:
		{
			Dictionary dictionary5 = RandomCustom();
			text = text + "\n" + dictionary5.GetValueOrDefault("String", "").AsString();
			num += dictionary5.GetValueOrDefault("Coin", 0).AsInt32();
			break;
		}
		case 11:
		{
			Dictionary dictionary4 = RandomCustom();
			text = text + "\n" + dictionary4.GetValueOrDefault("String", "").AsString();
			num += dictionary4.GetValueOrDefault("Coin", 0).AsInt32();
			break;
		}
		case 12:
		{
			Dictionary dictionary = RandomCustom();
			text = text + "\n" + dictionary.GetValueOrDefault("String", "").AsString();
			num += dictionary.GetValueOrDefault("Coin", 0).AsInt32();
			Dictionary dictionary2 = RandomCustom();
			text = text + "\n" + dictionary2.GetValueOrDefault("String", "").AsString();
			num += dictionary2.GetValueOrDefault("Coin", 0).AsInt32();
			Dictionary dictionary3 = RandomPlant();
			text = text + "\n" + dictionary3.GetValueOrDefault("String", "").AsString();
			num += dictionary3.GetValueOrDefault("Coin", 0).AsInt32();
			break;
		}
		}
		string text2 = AwardString[finishNum];
		if (finishNum == 12)
		{
			text2 += "+随机植物*1";
		}
		BroadCastConfig broadCastConfig = new BroadCastConfig();
		broadCastConfig.broadCastString = $"{value}，您一共完成{finishNum}个关卡\n{text2}{text}";
		BroadCastManager.Instance.BroadCastAdd(broadCastConfig);
		await CreateCoin(num);
		await ToSignal(GetTree().CreateTimer(2.0, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		BroadCastManager.Instance.BraodCastClear();
		tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(rewardButton, "global_position", new Vector2(470f, 320f), 1.0);
		await ToSignal(tween, Tween.SignalName.Finished);
		overNode.Visible = false;
		GameSaveManager.Instance.SetKeyValue("LevelEditorBattleFailNum", 0);
		GameSaveManager.Instance.SetKeyValue("LevelEditorBattleFinishNum", 0);
		failNum = GameSaveManager.Instance.GetKeyValue("LevelEditorBattleFailNum").AsInt32();
		finishNum = GameSaveManager.Instance.GetKeyValue("LevelEditorBattleFinishNum").AsInt32();
		FreshFinish(finishNum);
		FreshFail(failNum);
		GameSaveManager.Instance.Save();
	}

	public Dictionary RandomCustom()
	{
		string text = "";
		CharacterCustomConfig characterCustomConfig = null;
		TowerDefensePacketConfig packetConfig;
		TowerDefenseCharacterConfig characterConfig;
		do
		{
			List<string> packetNames = ResourceManager.Instance.GetPacketNames();
			packetConfig = TowerDefenseManager.GetPacketConfig(packetNames[GD.RandRange(0, packetNames.Count - 1)]);
			characterConfig = packetConfig.characterConfig;
		}
		while (!GodotObject.IsInstanceValid(characterConfig.customData) || characterConfig.customData.customList.Count <= 0);
		text = Tr(packetConfig.name);
		Array<CharacterCustomConfig> customList = characterConfig.customData.customList;
		characterCustomConfig = customList[GD.RandRange(0, customList.Count - 1)];
		if (GodotObject.IsInstanceValid(characterCustomConfig))
		{
			if (characterCustomConfig.openKey == "" || GameSaveManager.Instance.GetFeatureValue(characterCustomConfig.openKey) > 0)
			{
				int num = 10000;
				if (characterCustomConfig.type == "Gold")
				{
					num = 20000;
				}
				return new Dictionary
				{
					["String"] = $"你已获得{text}的皮肤:{Tr(characterCustomConfig.customHandbookName)},为你转化为{num}金币",
					["Coin"] = num
				};
			}
			GameSaveManager.Instance.SetFeatureValue(characterCustomConfig.openKey, true);
			return new Dictionary
			{
				["String"] = "恭喜你获得" + text + "的皮肤:" + Tr(characterCustomConfig.customHandbookName),
				["Coin"] = 0
			};
		}
		return new Dictionary();
	}

	public Dictionary RandomPlant()
	{
		if (!GodotObject.IsInstanceValid(ONLINE_LEVEL_EXCHANGE_RESOURCE) || ONLINE_LEVEL_EXCHANGE_RESOURCE.Data.VariantType != Variant.Type.Dictionary)
		{
			return new Dictionary();
		}
		Array array = ((Dictionary)ONLINE_LEVEL_EXCHANGE_RESOURCE.Data)["Exchange"].AsGodotArray();
		Array array2 = new Array();
		foreach (Variant item in array)
		{
			if (item.VariantType == Variant.Type.Dictionary)
			{
				Dictionary dictionary = item.AsGodotDictionary();
				if (!(dictionary.GetValueOrDefault("Type", "Packet").AsString() != "Packet"))
				{
					array2.Add(dictionary);
				}
			}
		}
		if (array2.Count == 0)
		{
			return new Dictionary();
		}
		Dictionary dictionary2 = array2.PickRandom().AsGodotDictionary();
		string text = dictionary2.GetValueOrDefault("Key", "").AsString();
		int num = dictionary2.GetValueOrDefault("CrystalNum", 0).AsInt32();
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(text);
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return new Dictionary();
		}
		string text2 = Tr(packetConfig.name);
		Dictionary towerDefensePacketValue = GameSaveManager.Instance.GetTowerDefensePacketValue(text);
		if (towerDefensePacketValue.GetValueOrDefault("Unlock", false).AsBool())
		{
			GameSaveManager.Instance.SetKeyValue("CrystalNum", GameSaveManager.Instance.GetKeyValue("CrystalNum").AsInt32() + num);
			return new Dictionary
			{
				["String"] = $"你已获得{text2},为你转化为{num}在线水晶",
				["Coin"] = 0
			};
		}
		towerDefensePacketValue["Unlock"] = true;
		GameSaveManager.Instance.SetTowerDefensePacketValue(text, towerDefensePacketValue);
		return new Dictionary
		{
			["String"] = "恭喜你获得" + text2,
			["Coin"] = 0
		};
	}

	public async Task CreateCoin(int num)
	{
		while (num >= 1000)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_DIAMOND, rewardButton.GlobalPosition + new Vector2(50f, 40f), 30.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			towerDefenseGroundItemBase.gridPos = new Vector2I(towerDefenseGroundItemBase.gridPos.X, 200);
			towerDefenseGroundItemBase.Reparent(this, keepGlobalTransform: false);
			if (towerDefenseGroundItemBase is TowerDefenseCoinBase towerDefenseCoinBase)
			{
				GetTree().CreateTimer(1.0, processAlways: false).Timeout += towerDefenseCoinBase.moveComponent.MoveClear;
				GetTree().CreateTimer(1.5, processAlways: false).Timeout += towerDefenseCoinBase.Collection;
			}
			num -= 1000;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		while (num >= 50)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase2 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_GOLD, rewardButton.GlobalPosition + new Vector2(50f, 40f), 30.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			towerDefenseGroundItemBase2.gridPos = new Vector2I(towerDefenseGroundItemBase2.gridPos.X, 200);
			towerDefenseGroundItemBase2.Reparent(this, keepGlobalTransform: false);
			if (towerDefenseGroundItemBase2 is TowerDefenseCoinBase towerDefenseCoinBase2)
			{
				GetTree().CreateTimer(1.0, processAlways: false).Timeout += towerDefenseCoinBase2.moveComponent.MoveClear;
				GetTree().CreateTimer(1.5, processAlways: false).Timeout += towerDefenseCoinBase2.Collection;
			}
			num -= 50;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		while (num >= 10)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase3 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_SILVER, rewardButton.GlobalPosition + new Vector2(50f, 40f), 30.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			towerDefenseGroundItemBase3.gridPos = new Vector2I(towerDefenseGroundItemBase3.gridPos.X, 200);
			towerDefenseGroundItemBase3.Reparent(this, keepGlobalTransform: false);
			if (towerDefenseGroundItemBase3 is TowerDefenseCoinBase towerDefenseCoinBase3)
			{
				GetTree().CreateTimer(1.0, processAlways: false).Timeout += towerDefenseCoinBase3.moveComponent.MoveClear;
				GetTree().CreateTimer(1.5, processAlways: false).Timeout += towerDefenseCoinBase3.Collection;
			}
			num -= 10;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
	}

	public void HistoryButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		historyNode.Visible = true;
		historyNooneLabel.Visible = false;
		Dictionary dictionary = GameSaveManager.Instance.GetKeyValue("OnlinehBattleHistory").AsGodotDictionary();
		if (!dictionary.ContainsKey("Level"))
		{
			dictionary["Level"] = new Array();
		}
		Array array = dictionary["Level"].AsGodotArray();
		foreach (Node child in historyDragMenu.GetChildren())
		{
			child.QueueFree();
		}
		foreach (Variant item in array)
		{
			Control control = LevelEditorOnlineLevelItemScene.Instantiate<Control>(PackedScene.GenEditState.Disabled);
			historyDragMenu.AddChild(control, forceReadableName: false, InternalMode.Disabled);
			control.PivotOffset = control.Size / 2f;
			control.Call("Init", item);
		}
		if (array.Count <= 0)
		{
			historyNooneLabel.Visible = true;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Show, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RandomLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryRetryOrFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FreshFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FreshFinish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnterLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "url", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelListGetHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelDataGetHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelGetHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToBattle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "json", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TipsButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RewardButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Over, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "fail", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RandomCustom, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RandomPlant, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HistoryButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Show && args.Count == 0)
		{
			_Show();
			ret = default;
			return true;
		}
		if (method == MethodName.RandomLevel && args.Count == 0)
		{
			RandomLevel();
			ret = default;
			return true;
		}
		if (method == MethodName.TryRetryOrFail && args.Count == 0)
		{
			TryRetryOrFail();
			ret = default;
			return true;
		}
		if (method == MethodName.FreshFail && args.Count == 1)
		{
			FreshFail(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreshFinish && args.Count == 1)
		{
			FreshFinish(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnterLevel && args.Count == 2)
		{
			EnterLevel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelListGetHTTPRequestCompleted && args.Count == 4)
		{
			LevelListGetHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelDataGetHTTPRequestCompleted && args.Count == 4)
		{
			LevelDataGetHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelGetHTTPRequestCompleted && args.Count == 4)
		{
			LevelGetHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToBattle && args.Count == 1)
		{
			ToBattle(VariantUtils.ConvertTo<Json>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshButtonPressed && args.Count == 0)
		{
			RefreshButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.TipsButtonPressed && args.Count == 0)
		{
			TipsButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.RewardButtonPressed && args.Count == 0)
		{
			RewardButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.Over && args.Count == 1)
		{
			Over(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RandomCustom && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(RandomCustom());
			return true;
		}
		if (method == MethodName.RandomPlant && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(RandomPlant());
			return true;
		}
		if (method == MethodName.HistoryButtonPressed && args.Count == 0)
		{
			HistoryButtonPressed();
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
		if (method == MethodName._Show)
		{
			return true;
		}
		if (method == MethodName.RandomLevel)
		{
			return true;
		}
		if (method == MethodName.TryRetryOrFail)
		{
			return true;
		}
		if (method == MethodName.FreshFail)
		{
			return true;
		}
		if (method == MethodName.FreshFinish)
		{
			return true;
		}
		if (method == MethodName.EnterLevel)
		{
			return true;
		}
		if (method == MethodName.LevelListGetHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.LevelDataGetHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.LevelGetHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.ToBattle)
		{
			return true;
		}
		if (method == MethodName.RefreshButtonPressed)
		{
			return true;
		}
		if (method == MethodName.TipsButtonPressed)
		{
			return true;
		}
		if (method == MethodName.RewardButtonPressed)
		{
			return true;
		}
		if (method == MethodName.Over)
		{
			return true;
		}
		if (method == MethodName.RandomCustom)
		{
			return true;
		}
		if (method == MethodName.RandomPlant)
		{
			return true;
		}
		if (method == MethodName.HistoryButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.failCheckBox)
		{
			failCheckBox = VariantUtils.ConvertToSystemArrayOfGodotObject<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.mainNode)
		{
			mainNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.tipsNode)
		{
			tipsNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.awardNode)
		{
			awardNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.overNode)
		{
			overNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.historyNode)
		{
			historyNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.levelGetHttpRequest)
		{
			levelGetHttpRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.levelDataGetHttpRequest)
		{
			levelDataGetHttpRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.levelListGetHttpRequest)
		{
			levelListGetHttpRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.levelEditorOnlineLevelItem)
		{
			levelEditorOnlineLevelItem = VariantUtils.ConvertTo<LevelEditorOnlineLevelItem>(in value);
			return true;
		}
		if (name == PropertyName.finishLabel)
		{
			finishLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.ticketLabel)
		{
			ticketLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.awardLabel)
		{
			awardLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.rewardButton)
		{
			rewardButton = VariantUtils.ConvertTo<SpriteBrightButton>(in value);
			return true;
		}
		if (name == PropertyName.costomRewardTexture1)
		{
			costomRewardTexture1 = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.costomRewardTexture2)
		{
			costomRewardTexture2 = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.historyDragMenu)
		{
			historyDragMenu = VariantUtils.ConvertTo<DragMenu>(in value);
			return true;
		}
		if (name == PropertyName.historyNooneLabel)
		{
			historyNooneLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.currentLevelId)
		{
			currentLevelId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentLevelData)
		{
			currentLevelData = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.failNum)
		{
			failNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.finishNum)
		{
			finishNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._refreshRetryCount)
		{
			_refreshRetryCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._isPaidRefresh)
		{
			_isPaidRefresh = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.failCheckBox)
		{
			GodotObject[] array = failCheckBox;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(array);
			return true;
		}
		if (name == PropertyName.mainNode)
		{
			value = VariantUtils.CreateFrom(in mainNode);
			return true;
		}
		if (name == PropertyName.tipsNode)
		{
			value = VariantUtils.CreateFrom(in tipsNode);
			return true;
		}
		if (name == PropertyName.awardNode)
		{
			value = VariantUtils.CreateFrom(in awardNode);
			return true;
		}
		if (name == PropertyName.overNode)
		{
			value = VariantUtils.CreateFrom(in overNode);
			return true;
		}
		if (name == PropertyName.historyNode)
		{
			value = VariantUtils.CreateFrom(in historyNode);
			return true;
		}
		if (name == PropertyName.levelGetHttpRequest)
		{
			value = VariantUtils.CreateFrom(in levelGetHttpRequest);
			return true;
		}
		if (name == PropertyName.levelDataGetHttpRequest)
		{
			value = VariantUtils.CreateFrom(in levelDataGetHttpRequest);
			return true;
		}
		if (name == PropertyName.levelListGetHttpRequest)
		{
			value = VariantUtils.CreateFrom(in levelListGetHttpRequest);
			return true;
		}
		if (name == PropertyName.levelEditorOnlineLevelItem)
		{
			value = VariantUtils.CreateFrom(in levelEditorOnlineLevelItem);
			return true;
		}
		if (name == PropertyName.finishLabel)
		{
			value = VariantUtils.CreateFrom(in finishLabel);
			return true;
		}
		if (name == PropertyName.ticketLabel)
		{
			value = VariantUtils.CreateFrom(in ticketLabel);
			return true;
		}
		if (name == PropertyName.awardLabel)
		{
			value = VariantUtils.CreateFrom(in awardLabel);
			return true;
		}
		if (name == PropertyName.rewardButton)
		{
			value = VariantUtils.CreateFrom(in rewardButton);
			return true;
		}
		if (name == PropertyName.costomRewardTexture1)
		{
			value = VariantUtils.CreateFrom(in costomRewardTexture1);
			return true;
		}
		if (name == PropertyName.costomRewardTexture2)
		{
			value = VariantUtils.CreateFrom(in costomRewardTexture2);
			return true;
		}
		if (name == PropertyName.historyDragMenu)
		{
			value = VariantUtils.CreateFrom(in historyDragMenu);
			return true;
		}
		if (name == PropertyName.historyNooneLabel)
		{
			value = VariantUtils.CreateFrom(in historyNooneLabel);
			return true;
		}
		if (name == PropertyName.currentLevelId)
		{
			value = VariantUtils.CreateFrom(in currentLevelId);
			return true;
		}
		if (name == PropertyName.currentLevelData)
		{
			value = VariantUtils.CreateFrom(in currentLevelData);
			return true;
		}
		if (name == PropertyName.failNum)
		{
			value = VariantUtils.CreateFrom(in failNum);
			return true;
		}
		if (name == PropertyName.finishNum)
		{
			value = VariantUtils.CreateFrom(in finishNum);
			return true;
		}
		if (name == PropertyName._refreshRetryCount)
		{
			value = VariantUtils.CreateFrom(in _refreshRetryCount);
			return true;
		}
		if (name == PropertyName._isPaidRefresh)
		{
			value = VariantUtils.CreateFrom(in _isPaidRefresh);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.failCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mainNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.tipsNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.awardNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.overNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.historyNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelGetHttpRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelDataGetHttpRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelListGetHttpRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelEditorOnlineLevelItem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.finishLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ticketLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.awardLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.rewardButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.costomRewardTexture1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.costomRewardTexture2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.historyDragMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.historyNooneLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentLevelId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.currentLevelData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.failNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.finishNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._refreshRetryCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isPaidRefresh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		StringName name = PropertyName.failCheckBox;
		GodotObject[] array = failCheckBox;
		info.AddProperty(name, Variant.CreateFrom(array));
		info.AddProperty(PropertyName.mainNode, Variant.From(in mainNode));
		info.AddProperty(PropertyName.tipsNode, Variant.From(in tipsNode));
		info.AddProperty(PropertyName.awardNode, Variant.From(in awardNode));
		info.AddProperty(PropertyName.overNode, Variant.From(in overNode));
		info.AddProperty(PropertyName.historyNode, Variant.From(in historyNode));
		info.AddProperty(PropertyName.levelGetHttpRequest, Variant.From(in levelGetHttpRequest));
		info.AddProperty(PropertyName.levelDataGetHttpRequest, Variant.From(in levelDataGetHttpRequest));
		info.AddProperty(PropertyName.levelListGetHttpRequest, Variant.From(in levelListGetHttpRequest));
		info.AddProperty(PropertyName.levelEditorOnlineLevelItem, Variant.From(in levelEditorOnlineLevelItem));
		info.AddProperty(PropertyName.finishLabel, Variant.From(in finishLabel));
		info.AddProperty(PropertyName.ticketLabel, Variant.From(in ticketLabel));
		info.AddProperty(PropertyName.awardLabel, Variant.From(in awardLabel));
		info.AddProperty(PropertyName.rewardButton, Variant.From(in rewardButton));
		info.AddProperty(PropertyName.costomRewardTexture1, Variant.From(in costomRewardTexture1));
		info.AddProperty(PropertyName.costomRewardTexture2, Variant.From(in costomRewardTexture2));
		info.AddProperty(PropertyName.historyDragMenu, Variant.From(in historyDragMenu));
		info.AddProperty(PropertyName.historyNooneLabel, Variant.From(in historyNooneLabel));
		info.AddProperty(PropertyName.currentLevelId, Variant.From(in currentLevelId));
		info.AddProperty(PropertyName.currentLevelData, Variant.From(in currentLevelData));
		info.AddProperty(PropertyName.failNum, Variant.From(in failNum));
		info.AddProperty(PropertyName.finishNum, Variant.From(in finishNum));
		info.AddProperty(PropertyName._refreshRetryCount, Variant.From(in _refreshRetryCount));
		info.AddProperty(PropertyName._isPaidRefresh, Variant.From(in _isPaidRefresh));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.failCheckBox, out var value))
		{
			failCheckBox = value.AsGodotObjectArray<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.mainNode, out var value2))
		{
			mainNode = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.tipsNode, out var value3))
		{
			tipsNode = value3.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.awardNode, out var value4))
		{
			awardNode = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.overNode, out var value5))
		{
			overNode = value5.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.historyNode, out var value6))
		{
			historyNode = value6.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.levelGetHttpRequest, out var value7))
		{
			levelGetHttpRequest = value7.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.levelDataGetHttpRequest, out var value8))
		{
			levelDataGetHttpRequest = value8.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.levelListGetHttpRequest, out var value9))
		{
			levelListGetHttpRequest = value9.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.levelEditorOnlineLevelItem, out var value10))
		{
			levelEditorOnlineLevelItem = value10.As<LevelEditorOnlineLevelItem>();
		}
		if (info.TryGetProperty(PropertyName.finishLabel, out var value11))
		{
			finishLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.ticketLabel, out var value12))
		{
			ticketLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.awardLabel, out var value13))
		{
			awardLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.rewardButton, out var value14))
		{
			rewardButton = value14.As<SpriteBrightButton>();
		}
		if (info.TryGetProperty(PropertyName.costomRewardTexture1, out var value15))
		{
			costomRewardTexture1 = value15.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.costomRewardTexture2, out var value16))
		{
			costomRewardTexture2 = value16.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.historyDragMenu, out var value17))
		{
			historyDragMenu = value17.As<DragMenu>();
		}
		if (info.TryGetProperty(PropertyName.historyNooneLabel, out var value18))
		{
			historyNooneLabel = value18.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.currentLevelId, out var value19))
		{
			currentLevelId = value19.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentLevelData, out var value20))
		{
			currentLevelData = value20.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.failNum, out var value21))
		{
			failNum = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName.finishNum, out var value22))
		{
			finishNum = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName._refreshRetryCount, out var value23))
		{
			_refreshRetryCount = value23.As<int>();
		}
		if (info.TryGetProperty(PropertyName._isPaidRefresh, out var value24))
		{
			_isPaidRefresh = value24.As<bool>();
		}
	}
}
