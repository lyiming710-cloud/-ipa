using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/OnlineLevel/LevelEditorOnlineLevel.cs")]
public class LevelEditorOnlineLevel : Control
{
	private sealed record BrowseState(int Page, string Suffix, string Suffix2, string Search, string Tags, bool MyCollection, int TypeIndex, string SearchText, string AuthorUid, string AuthorName, BrowseState Parent);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RestoreBrowseState = "RestoreBrowseState";

		public static readonly StringName OpenAuthorLevels = "OpenAuthorLevels";

		public static readonly StringName TryReturnToLevelList = "TryReturnToLevelList";

		public static readonly StringName RefreshBrowseMode = "RefreshBrowseMode";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName GetPage = "GetPage";

		public static readonly StringName ShowLoadingLabelAfterDelay = "ShowLoadingLabelAfterDelay";

		public static readonly StringName ClearLevelItems = "ClearLevelItems";

		public static readonly StringName LoadTagsData = "LoadTagsData";

		public static readonly StringName TagsOptionButtonItemSelected = "TagsOptionButtonItemSelected";

		public static readonly StringName LoadMyCollectionPage = "LoadMyCollectionPage";

		public static readonly StringName LoadPageData = "LoadPageData";

		public static readonly StringName PageLabelFresh = "PageLabelFresh";

		public static readonly StringName PreButtonPressed = "PreButtonPressed";

		public static readonly StringName NextButtonpressed = "NextButtonpressed";

		public static readonly StringName EnterLevelButtonPressed = "EnterLevelButtonPressed";

		public static readonly StringName SearchLevelButtonPressed = "SearchLevelButtonPressed";

		public static readonly StringName JumpPageToButtonPressed = "JumpPageToButtonPressed";

		public static readonly StringName ToBattle = "ToBattle";

		public static readonly StringName EnterLevel = "EnterLevel";

		public static readonly StringName LevelGetHTTPRequestCompleted = "LevelGetHTTPRequestCompleted";

		public static readonly StringName LevelInformationGetHTTPRequestCompleted = "LevelInformationGetHTTPRequestCompleted";

		public static readonly StringName MyCollectionButtonPressed = "MyCollectionButtonPressed";

		public static readonly StringName SmartRecommendationButtonPressed = "SmartRecommendationButtonPressed";

		public static readonly StringName OfficialRecommendationButtonPressed = "OfficialRecommendationButtonPressed";

		public static readonly StringName SortByTimeButtonPressed = "SortByTimeButtonPressed";

		public static readonly StringName SortByNumberOfPlayedButtonPressed = "SortByNumberOfPlayedButtonPressed";

		public static readonly StringName ResetTags = "ResetTags";

		public static readonly StringName ExchangeButtonPressed = "ExchangeButtonPressed";

		public static readonly StringName TypeOptionButtonItemSelected = "TypeOptionButtonItemSelected";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName IsAuthorPage = "IsAuthorPage";

		public static readonly StringName _authorUid = "_authorUid";

		public static readonly StringName _authorName = "_authorName";

		public static readonly StringName _requestedPage = "_requestedPage";

		public static readonly StringName _levelGetHTTPRequest = "_levelGetHTTPRequest";

		public static readonly StringName _levelInformationGetHTTPRequest = "_levelInformationGetHTTPRequest";

		public static readonly StringName _onlineLevelContainer = "_onlineLevelContainer";

		public static readonly StringName _levelContainer = "_levelContainer";

		public static readonly StringName _pageLabel = "_pageLabel";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _levelIdLineEdit = "_levelIdLineEdit";

		public static readonly StringName _enterLevelButton = "_enterLevelButton";

		public static readonly StringName _jumpPageToSpinBox = "_jumpPageToSpinBox";

		public static readonly StringName _jumpPageToButton = "_jumpPageToButton";

		public static readonly StringName _typeOptionButton = "_typeOptionButton";

		public static readonly StringName _tagsOptionButton = "_tagsOptionButton";

		public static readonly StringName _pageLoading = "_pageLoading";

		public static readonly StringName levelNum = "levelNum";

		public static readonly StringName maxPage = "maxPage";

		public static readonly StringName currentPage = "currentPage";

		public static readonly StringName currentPageList = "currentPageList";

		public static readonly StringName suffix = "suffix";

		public static readonly StringName suffix2 = "suffix2";

		public static readonly StringName search = "search";

		public static readonly StringName tags = "tags";

		public static readonly StringName currentLevelId = "currentLevelId";

		public static readonly StringName myCollection = "myCollection";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static PackedScene _levelEditorOnlineLevelItem;

	private static readonly Regex LevelIdRegex = new Regex("^[0-9a-zA-Z_-]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static BrowseState _returnBrowseState;

	private BrowseState _previousBrowseState;

	private string _authorUid = "";

	private string _authorName = "";

	private int _requestedPage = 1;

	private NativeHttpRequest _levelGetHTTPRequest;

	private NativeHttpRequest _levelInformationGetHTTPRequest;

	private HFlowContainer _onlineLevelContainer;

	private MarginContainer _levelContainer;

	private Label _pageLabel;

	private Label _statusLabel;

	private LineEdit _levelIdLineEdit;

	private MainButton _enterLevelButton;

	private SpinBox _jumpPageToSpinBox;

	private MainButton _jumpPageToButton;

	private OptionButton _typeOptionButton;

	private OptionButton _tagsOptionButton;

	private bool _pageLoading;

	public int levelNum = 1;

	public int maxPage = 1;

	public int currentPage = -1;

	public Array currentPageList = new Array();

	public string suffix = "";

	public string suffix2 = "";

	public string search = "";

	public string tags = "";

	public string currentLevelId = "";

	public bool myCollection;

	private static PackedScene LEVEL_EDITOR_ONLINE_LEVEL_ITEM => _levelEditorOnlineLevelItem ?? (_levelEditorOnlineLevelItem = GD.Load<PackedScene>("uid://7tkumqfm20gv"));

	public bool IsAuthorPage => _authorUid != "";

	public override void _Ready()
	{
		_levelGetHTTPRequest = new NativeHttpRequest();
		_levelInformationGetHTTPRequest = new NativeHttpRequest();
		AddChild(_levelGetHTTPRequest, forceReadableName: false, InternalMode.Disabled);
		AddChild(_levelInformationGetHTTPRequest, forceReadableName: false, InternalMode.Disabled);
		_onlineLevelContainer = GetNode<HFlowContainer>("%OnlineLevelContainer");
		_levelContainer = GetNode<MarginContainer>("%LevelContainer");
		_pageLabel = GetNode<Label>("%PageLabel");
		_statusLabel = GetNode<Label>("%StatusLabel");
		_levelIdLineEdit = GetNode<LineEdit>("%LevelIdLineEdit");
		_enterLevelButton = GetNode<MainButton>("%EnterLevelButton");
		_jumpPageToSpinBox = GetNode<SpinBox>("%JumpPageToSpinBox");
		_jumpPageToButton = GetNode<MainButton>("%JumpPageToButton");
		_typeOptionButton = GetNode<OptionButton>("%TypeOptionButton");
		_tagsOptionButton = GetNode<OptionButton>("%TagsOptionButton");
		LevelEditorDropdown.Configure(_typeOptionButton);
		LevelEditorDropdown.Configure(_tagsOptionButton);
		RestoreBrowseState();
		_levelGetHTTPRequest.RequestCompleted += LevelGetHTTPRequestCompleted;
		_levelInformationGetHTTPRequest.RequestCompleted += LevelInformationGetHTTPRequestCompleted;
		GetNode<SpriteBrightButton>("%PreCustomButton").OnPressed += PreButtonPressed;
		GetNode<SpriteBrightButton>("%NextCustomButton").OnPressed += NextButtonpressed;
		_enterLevelButton.Pressed += EnterLevelButtonPressed;
		GetNode<MainButton>("%SearchLevelButton").Pressed += SearchLevelButtonPressed;
		_typeOptionButton.ItemSelected += (long index) =>
		{
			TypeOptionButtonItemSelected((int)index);
		};
		_tagsOptionButton.ItemSelected += (long index) =>
		{
			TagsOptionButtonItemSelected((int)index);
		};
		_jumpPageToButton.Pressed += JumpPageToButtonPressed;
		GetNode<NinePatchButtonBase>("HBoxContainer3/MyCollectionButton").OnPressed += MyCollectionButtonPressed;
		GetNode<NinePatchButtonBase>("HBoxContainer3/SmartRecommendationButton").OnPressed += SmartRecommendationButtonPressed;
		GetNode<NinePatchButtonBase>("HBoxContainer3/OfficialRecommendationButton").OnPressed += OfficialRecommendationButtonPressed;
		GetNode<NinePatchButtonBase>("HBoxContainer3/SortByTimeButton").OnPressed += SortByTimeButtonPressed;
		GetNode<NinePatchButtonBase>("HBoxContainer3/SortByNumberOfPlayedButton").OnPressed += SortByNumberOfPlayedButtonPressed;
		GetNode<TextureButton>("ExchangeButton").Pressed += ExchangeButtonPressed;
		InternetServerManager.Instance.OnOnlineLevelGet += LoadPageData;
		InternetServerManager.Instance.OnWorkshopTagsGet += LoadTagsData;
		InternetServerManager.Instance.GetWorkshopTags();
	}

	private void RestoreBrowseState()
	{
		BrowseState returnBrowseState = _returnBrowseState;
		_returnBrowseState = null;
		if (!(returnBrowseState == null))
		{
			ApplyBrowseState(returnBrowseState);
		}
	}

	private BrowseState CaptureBrowseState()
	{
		return new BrowseState(Mathf.Max(1, currentPage), suffix, suffix2, search, tags, myCollection, _typeOptionButton.Selected, _levelIdLineEdit.Text, _authorUid, _authorName, _previousBrowseState);
	}

	private void ApplyBrowseState(BrowseState state)
	{
		currentPage = state.Page;
		suffix = state.Suffix;
		suffix2 = state.Suffix2;
		search = state.Search;
		tags = state.Tags;
		myCollection = state.MyCollection;
		_typeOptionButton.Select(state.TypeIndex);
		_levelIdLineEdit.Text = state.SearchText;
		_authorUid = state.AuthorUid;
		_authorName = state.AuthorName;
		_previousBrowseState = state.Parent;
		for (int i = 0; i < _tagsOptionButton.ItemCount; i++)
		{
			if (i == 0 || _tagsOptionButton.GetItemText(i) == tags)
			{
				_tagsOptionButton.Select(i);
			}
		}
		RefreshBrowseMode();
	}

	public void OpenAuthorLevels(string uid, string name)
	{
		if (!string.IsNullOrEmpty(uid))
		{
			if (!IsAuthorPage)
			{
				_previousBrowseState = CaptureBrowseState();
			}
			_authorUid = uid;
			_authorName = name;
			myCollection = false;
			suffix = (suffix2 = (search = (tags = "")));
			_typeOptionButton.Select(0);
			_levelIdLineEdit.Text = "";
			ResetTags();
			RefreshBrowseMode();
			maxPage = 1;
			GetPage();
		}
	}

	public bool TryReturnToLevelList()
	{
		if (!IsAuthorPage || _previousBrowseState == null)
		{
			return false;
		}
		ApplyBrowseState(_previousBrowseState);
		GetPage(currentPage);
		return true;
	}

	private void RefreshBrowseMode()
	{
		GetNode<Control>("HBoxContainer").Visible = !IsAuthorPage;
		GetNode<Control>("HBoxContainer3").Visible = !IsAuthorPage;
		GetNode<Control>("ExchangeButton").Visible = !IsAuthorPage;
		GetNode<Label>("Label").Text = (IsAuthorPage ? "作者关卡" : "在线关卡");
		Label node = GetNode<Label>("%AuthorNameLabel");
		node.Visible = IsAuthorPage;
		node.Text = _authorName;
		node.TooltipText = _authorName;
	}

	public override void _ExitTree()
	{
		InternetServerManager.Instance.OnOnlineLevelGet -= LoadPageData;
		InternetServerManager.Instance.OnWorkshopTagsGet -= LoadTagsData;
		base._ExitTree();
	}

	public void GetPage(int pageIndex = 1)
	{
		_requestedPage = Mathf.Clamp(pageIndex, 1, 100000);
		currentPage = _requestedPage;
		maxPage = Mathf.Max(currentPage, maxPage);
		PageLabelFresh();
		_levelContainer.Visible = false;
		_pageLoading = true;
		_statusLabel.Visible = false;
		ShowLoadingLabelAfterDelay(1f);
		ClearLevelItems();
		if (IsAuthorPage)
		{
			InternetServerManager.Instance.GetOnlineLevelPage(_requestedPage, "", "", "", "", _authorUid);
			return;
		}
		if (!myCollection)
		{
			InternetServerManager.Instance.GetOnlineLevelPage(_requestedPage, suffix, suffix2, search, tags);
			return;
		}
		InternetServerManager.Instance.onlineLevelHTTPRequest.CancelRequest();
		LoadMyCollectionPage(_requestedPage);
	}

	private async void ShowLoadingLabelAfterDelay(float delay)
	{
		await ToSignal(GetTree().CreateTimer(delay, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		if (_pageLoading && GodotObject.IsInstanceValid(_statusLabel))
		{
			_statusLabel.Text = "关卡加载中...";
			_statusLabel.Visible = true;
		}
	}

	private void ClearLevelItems()
	{
		foreach (Node child in _onlineLevelContainer.GetChildren())
		{
			_onlineLevelContainer.RemoveChild(child);
			child.QueueFree();
		}
		GetNode<ScrollContainer>("LevelContainer/ScrollContainer").ScrollVertical = 0;
	}

	public void LoadTagsData(Array data)
	{
		if (!GodotObject.IsInstanceValid(_tagsOptionButton))
		{
			return;
		}
		_tagsOptionButton.Clear();
		_tagsOptionButton.AddItem("全部标签", 0);
		int idx = 0;
		for (int i = 0; i < data.Count; i++)
		{
			string text = data[i].AsGodotDictionary().GetValueOrDefault("name", "").AsString();
			if (text != "")
			{
				_tagsOptionButton.AddItem(text, i + 1);
				if (text == tags)
				{
					idx = _tagsOptionButton.ItemCount - 1;
				}
			}
		}
		_tagsOptionButton.Select(idx);
	}

	public void TagsOptionButtonItemSelected(int index)
	{
		myCollection = false;
		if (index <= 0)
		{
			tags = "";
		}
		else
		{
			tags = _tagsOptionButton.GetItemText(index);
		}
		GetPage();
	}

	public void LoadMyCollectionPage(int pageIndex = 1)
	{
		_pageLoading = false;
		_levelContainer.Visible = true;
		Array array = GameSaveManager.Instance.GetKeyValue("OnlineMyCollection").AsGodotDictionary().GetValueOrDefault("Level", new Array())
			.AsGodotArray();
		levelNum = array.Count;
		maxPage = Mathf.Max(1, (int)Mathf.Ceil((double)array.Count / 10.0));
		currentPage = Mathf.Clamp(pageIndex, 1, maxPage);
		_statusLabel.Visible = levelNum == 0;
		_statusLabel.Text = "暂无收藏关卡";
		PageLabelFresh();
		for (int i = (currentPage - 1) * 10; i < Mathf.Min(currentPage * 10, levelNum); i++)
		{
			Dictionary dictionary = array[i].AsGodotDictionary();
			if (dictionary.GetValueOrDefault("id", "-1").AsString() != "-1")
			{
				LevelEditorOnlineLevelItem levelEditorOnlineLevelItem = LEVEL_EDITOR_ONLINE_LEVEL_ITEM.Instantiate<LevelEditorOnlineLevelItem>(PackedScene.GenEditState.Disabled);
				_onlineLevelContainer.AddChild(levelEditorOnlineLevelItem, forceReadableName: false, InternalMode.Disabled);
				levelEditorOnlineLevelItem.Init(dictionary);
				levelEditorOnlineLevelItem.OnSelect += EnterLevel;
				levelEditorOnlineLevelItem.OnAuthorSelected += OpenAuthorLevels;
			}
		}
	}

	public void LoadPageData(Dictionary data)
	{
		if (myCollection || !GodotObject.IsInstanceValid(_levelContainer))
		{
			return;
		}
		_pageLoading = false;
		_levelContainer.Visible = true;
		ClearLevelItems();
		if (!data.TryGetValue("list", out var value) || value.VariantType != Variant.Type.Array)
		{
			currentPage = _requestedPage;
			maxPage = Mathf.Max(currentPage, maxPage);
			currentPageList = new Array();
			levelNum = 0;
			_statusLabel.Text = "关卡加载失败，请点击跳转页数重试";
			_statusLabel.Visible = true;
			PageLabelFresh();
			return;
		}
		levelNum = data.GetValueOrDefault("total", 0).AsInt32();
		maxPage = Mathf.Max(1, data.GetValueOrDefault("maxPage", data.GetValueOrDefault("max_page", 1)).AsInt32());
		currentPage = Mathf.Clamp(data.GetValueOrDefault("page", _requestedPage).AsInt32(), 1, maxPage);
		currentPageList = value.AsGodotArray();
		_statusLabel.Visible = currentPageList.Count == 0;
		_statusLabel.Text = (IsAuthorPage ? "该作者暂无公开关卡" : "暂无关卡");
		PageLabelFresh();
		foreach (Variant currentPage in currentPageList)
		{
			Dictionary dictionary = currentPage.AsGodotDictionary();
			if (dictionary.GetValueOrDefault("id", "-1").AsString() != "-1")
			{
				LevelEditorOnlineLevelItem levelEditorOnlineLevelItem = LEVEL_EDITOR_ONLINE_LEVEL_ITEM.Instantiate<LevelEditorOnlineLevelItem>(PackedScene.GenEditState.Disabled);
				_onlineLevelContainer.AddChild(levelEditorOnlineLevelItem, forceReadableName: false, InternalMode.Disabled);
				levelEditorOnlineLevelItem.Init(dictionary);
				levelEditorOnlineLevelItem.OnSelect += EnterLevel;
				levelEditorOnlineLevelItem.OnAuthorSelected += OpenAuthorLevels;
			}
		}
	}

	public void PageLabelFresh()
	{
		_jumpPageToSpinBox.MaxValue = maxPage;
		_jumpPageToSpinBox.Value = currentPage;
		_pageLabel.Text = $"第{currentPage}页，共{maxPage}页";
	}

	public void PreButtonPressed()
	{
		if (currentPage != -1)
		{
			int num = currentPage - 1;
			if (num < 1)
			{
				num = maxPage;
			}
			GetPage(num);
		}
	}

	public void NextButtonpressed()
	{
		if (currentPage != -1)
		{
			int num = currentPage + 1;
			if (num > maxPage)
			{
				num = 1;
			}
			GetPage(num);
		}
	}

	public void EnterLevelButtonPressed()
	{
		if (LevelIdRegex.IsMatch(_levelIdLineEdit.Text))
		{
			string url = $"https://api.pvzhe.com/workshop/levels/{_levelIdLineEdit.Text}";
			_levelInformationGetHTTPRequest.Request(url, Global.Instance.header);
			_enterLevelButton.Disabled = true;
		}
	}

	public void SearchLevelButtonPressed()
	{
		myCollection = false;
		search = _levelIdLineEdit.Text.Trim();
		GetPage();
	}

	public void JumpPageToButtonPressed()
	{
		GetPage((int)_jumpPageToSpinBox.Value);
	}

	public async void ToBattle(Json json)
	{
		TowerDefenseLevelConfig towerDefenseLevelConfig = new TowerDefenseLevelConfig();
		towerDefenseLevelConfig.data = json;
		towerDefenseLevelConfig.Init();
		_returnBrowseState = CaptureBrowseState();
		TowerDefenseManager.Instance.currentLevelConfig = towerDefenseLevelConfig;
		Global.Instance.enterLevelMode = "OnlineLevel";
		if (Global.Instance.isMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			MultiPlayerManager.Instance.selectedLevelId = "";
			await MultiPlayerManager.Instance.StartCompatibleGameAsync();
		}
		else
		{
			SceneManager.Instance.ChangeScene("TowerDefense");
		}
	}

	public void EnterLevel(string url, string id)
	{
		currentLevelId = id;
		if (url != "")
		{
			string url2 = $"https://api.pvzhe.com{url}";
			_levelGetHTTPRequest.Request(url2, Global.Instance.header);
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

	public void LevelInformationGetHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != 0L)
		{
			BroadCastManager.Instance.BroadCastFloatCreate(Global.Instance.GetHTTPRequestErrorMessage(result), Colors.Red);
			_enterLevelButton.Disabled = false;
			return;
		}
		Json json = new Json();
		json.Parse(body.GetStringFromUtf8());
		if (json.Data.VariantType != Variant.Type.Nil)
		{
			Dictionary dictionary = json.Data.AsGodotDictionary();
			string _id = dictionary.GetValueOrDefault("id", "-1").AsString();
			if (_id != "-1")
			{
				OnlineLevelPreview onlineLevelPreview = (OnlineLevelPreview)DialogManager.Instance.DialogCreate("OnlineLevelPreview");
				onlineLevelPreview.OnAuthorSelected += OpenAuthorLevels;
				onlineLevelPreview.InitDialogData(dictionary);
				onlineLevelPreview.OnSelect += (string url) =>
				{
					EnterLevel(url, _id);
				};
			}
			else
			{
				BroadCastManager.Instance.BroadCastFloatCreate("关卡Id无效", Colors.Red);
			}
		}
		else
		{
			BroadCastManager.Instance.BroadCastFloatCreate("关卡Id无效", Colors.Red);
		}
		_enterLevelButton.Disabled = false;
	}

	public void MyCollectionButtonPressed()
	{
		myCollection = true;
		suffix = "";
		_typeOptionButton.Selected = 0;
		TypeOptionButtonItemSelected(0);
		ResetTags();
		GetPage();
	}

	public void SmartRecommendationButtonPressed()
	{
		myCollection = false;
		suffix = "&recp";
		ResetTags();
		GetPage();
	}

	public void OfficialRecommendationButtonPressed()
	{
		myCollection = false;
		suffix = "&feature=rec";
		ResetTags();
		GetPage();
	}

	public void SortByTimeButtonPressed()
	{
		myCollection = false;
		suffix = "";
		ResetTags();
		GetPage();
	}

	public void SortByNumberOfPlayedButtonPressed()
	{
		myCollection = false;
		suffix = "&sort=plays";
		ResetTags();
		GetPage();
	}

	private void ResetTags()
	{
		tags = "";
		if (_tagsOptionButton != null && _tagsOptionButton.ItemCount > 0)
		{
			_tagsOptionButton.Select(0);
		}
	}

	public void ExchangeButtonPressed()
	{
		DialogManager.Instance.DialogCreate("OnlineLevelExchange");
	}

	public void TypeOptionButtonItemSelected(int index)
	{
		switch (index)
		{
		case 0:
			suffix2 = "";
			break;
		case 1:
			suffix2 = "&finish_method=wave&round_mode=default&is_lucky=false";
			break;
		case 2:
			suffix2 = "&finish_method=vase";
			break;
		case 3:
			suffix2 = "&finish_method=izm";
			break;
		case 4:
			suffix2 = "&finish_method=izm2";
			break;
		case 5:
			suffix2 = "&round_mode=survival";
			break;
		case 6:
			suffix2 = "&round_mode=endless";
			break;
		case 7:
			suffix2 = "&is_lucky=true";
			break;
		}
		GetPage();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(31)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreBrowseState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenAuthorLevels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "uid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryReturnToLevelList, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshBrowseMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "pageIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowLoadingLabelAfterDelay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearLevelItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadTagsData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TagsOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadMyCollectionPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "pageIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadPageData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PageLabelFresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextButtonpressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterLevelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SearchLevelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpPageToButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToBattle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "json", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnterLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "url", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelGetHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelInformationGetHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MyCollectionButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SmartRecommendationButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OfficialRecommendationButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SortByTimeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SortByNumberOfPlayedButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetTags, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExchangeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TypeOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RestoreBrowseState && args.Count == 0)
		{
			RestoreBrowseState();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenAuthorLevels && args.Count == 2)
		{
			OpenAuthorLevels(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryReturnToLevelList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryReturnToLevelList());
			return true;
		}
		if (method == MethodName.RefreshBrowseMode && args.Count == 0)
		{
			RefreshBrowseMode();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPage && args.Count == 1)
		{
			GetPage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowLoadingLabelAfterDelay && args.Count == 1)
		{
			ShowLoadingLabelAfterDelay(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearLevelItems && args.Count == 0)
		{
			ClearLevelItems();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadTagsData && args.Count == 1)
		{
			LoadTagsData(VariantUtils.ConvertTo<Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TagsOptionButtonItemSelected && args.Count == 1)
		{
			TagsOptionButtonItemSelected(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadMyCollectionPage && args.Count == 1)
		{
			LoadMyCollectionPage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPageData && args.Count == 1)
		{
			LoadPageData(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PageLabelFresh && args.Count == 0)
		{
			PageLabelFresh();
			ret = default;
			return true;
		}
		if (method == MethodName.PreButtonPressed && args.Count == 0)
		{
			PreButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.NextButtonpressed && args.Count == 0)
		{
			NextButtonpressed();
			ret = default;
			return true;
		}
		if (method == MethodName.EnterLevelButtonPressed && args.Count == 0)
		{
			EnterLevelButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SearchLevelButtonPressed && args.Count == 0)
		{
			SearchLevelButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.JumpPageToButtonPressed && args.Count == 0)
		{
			JumpPageToButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ToBattle && args.Count == 1)
		{
			ToBattle(VariantUtils.ConvertTo<Json>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnterLevel && args.Count == 2)
		{
			EnterLevel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelGetHTTPRequestCompleted && args.Count == 4)
		{
			LevelGetHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelInformationGetHTTPRequestCompleted && args.Count == 4)
		{
			LevelInformationGetHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.MyCollectionButtonPressed && args.Count == 0)
		{
			MyCollectionButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SmartRecommendationButtonPressed && args.Count == 0)
		{
			SmartRecommendationButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OfficialRecommendationButtonPressed && args.Count == 0)
		{
			OfficialRecommendationButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SortByTimeButtonPressed && args.Count == 0)
		{
			SortByTimeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SortByNumberOfPlayedButtonPressed && args.Count == 0)
		{
			SortByNumberOfPlayedButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetTags && args.Count == 0)
		{
			ResetTags();
			ret = default;
			return true;
		}
		if (method == MethodName.ExchangeButtonPressed && args.Count == 0)
		{
			ExchangeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.TypeOptionButtonItemSelected && args.Count == 1)
		{
			TypeOptionButtonItemSelected(VariantUtils.ConvertTo<int>(in args[0]));
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
		if (method == MethodName.RestoreBrowseState)
		{
			return true;
		}
		if (method == MethodName.OpenAuthorLevels)
		{
			return true;
		}
		if (method == MethodName.TryReturnToLevelList)
		{
			return true;
		}
		if (method == MethodName.RefreshBrowseMode)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.GetPage)
		{
			return true;
		}
		if (method == MethodName.ShowLoadingLabelAfterDelay)
		{
			return true;
		}
		if (method == MethodName.ClearLevelItems)
		{
			return true;
		}
		if (method == MethodName.LoadTagsData)
		{
			return true;
		}
		if (method == MethodName.TagsOptionButtonItemSelected)
		{
			return true;
		}
		if (method == MethodName.LoadMyCollectionPage)
		{
			return true;
		}
		if (method == MethodName.LoadPageData)
		{
			return true;
		}
		if (method == MethodName.PageLabelFresh)
		{
			return true;
		}
		if (method == MethodName.PreButtonPressed)
		{
			return true;
		}
		if (method == MethodName.NextButtonpressed)
		{
			return true;
		}
		if (method == MethodName.EnterLevelButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SearchLevelButtonPressed)
		{
			return true;
		}
		if (method == MethodName.JumpPageToButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ToBattle)
		{
			return true;
		}
		if (method == MethodName.EnterLevel)
		{
			return true;
		}
		if (method == MethodName.LevelGetHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.LevelInformationGetHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.MyCollectionButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SmartRecommendationButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OfficialRecommendationButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SortByTimeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SortByNumberOfPlayedButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ResetTags)
		{
			return true;
		}
		if (method == MethodName.ExchangeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.TypeOptionButtonItemSelected)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._authorUid)
		{
			_authorUid = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._authorName)
		{
			_authorName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._requestedPage)
		{
			_requestedPage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._levelGetHTTPRequest)
		{
			_levelGetHTTPRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName._levelInformationGetHTTPRequest)
		{
			_levelInformationGetHTTPRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName._onlineLevelContainer)
		{
			_onlineLevelContainer = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._levelContainer)
		{
			_levelContainer = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName._pageLabel)
		{
			_pageLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._levelIdLineEdit)
		{
			_levelIdLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._enterLevelButton)
		{
			_enterLevelButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._jumpPageToSpinBox)
		{
			_jumpPageToSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._jumpPageToButton)
		{
			_jumpPageToButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._typeOptionButton)
		{
			_typeOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._tagsOptionButton)
		{
			_tagsOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._pageLoading)
		{
			_pageLoading = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.levelNum)
		{
			levelNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.maxPage)
		{
			maxPage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentPage)
		{
			currentPage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentPageList)
		{
			currentPageList = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.suffix)
		{
			suffix = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.suffix2)
		{
			suffix2 = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.search)
		{
			search = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.tags)
		{
			tags = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentLevelId)
		{
			currentLevelId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.myCollection)
		{
			myCollection = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.IsAuthorPage)
		{
			value = VariantUtils.CreateFrom<bool>(IsAuthorPage);
			return true;
		}
		if (name == PropertyName._authorUid)
		{
			value = VariantUtils.CreateFrom(in _authorUid);
			return true;
		}
		if (name == PropertyName._authorName)
		{
			value = VariantUtils.CreateFrom(in _authorName);
			return true;
		}
		if (name == PropertyName._requestedPage)
		{
			value = VariantUtils.CreateFrom(in _requestedPage);
			return true;
		}
		if (name == PropertyName._levelGetHTTPRequest)
		{
			value = VariantUtils.CreateFrom(in _levelGetHTTPRequest);
			return true;
		}
		if (name == PropertyName._levelInformationGetHTTPRequest)
		{
			value = VariantUtils.CreateFrom(in _levelInformationGetHTTPRequest);
			return true;
		}
		if (name == PropertyName._onlineLevelContainer)
		{
			value = VariantUtils.CreateFrom(in _onlineLevelContainer);
			return true;
		}
		if (name == PropertyName._levelContainer)
		{
			value = VariantUtils.CreateFrom(in _levelContainer);
			return true;
		}
		if (name == PropertyName._pageLabel)
		{
			value = VariantUtils.CreateFrom(in _pageLabel);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._levelIdLineEdit)
		{
			value = VariantUtils.CreateFrom(in _levelIdLineEdit);
			return true;
		}
		if (name == PropertyName._enterLevelButton)
		{
			value = VariantUtils.CreateFrom(in _enterLevelButton);
			return true;
		}
		if (name == PropertyName._jumpPageToSpinBox)
		{
			value = VariantUtils.CreateFrom(in _jumpPageToSpinBox);
			return true;
		}
		if (name == PropertyName._jumpPageToButton)
		{
			value = VariantUtils.CreateFrom(in _jumpPageToButton);
			return true;
		}
		if (name == PropertyName._typeOptionButton)
		{
			value = VariantUtils.CreateFrom(in _typeOptionButton);
			return true;
		}
		if (name == PropertyName._tagsOptionButton)
		{
			value = VariantUtils.CreateFrom(in _tagsOptionButton);
			return true;
		}
		if (name == PropertyName._pageLoading)
		{
			value = VariantUtils.CreateFrom(in _pageLoading);
			return true;
		}
		if (name == PropertyName.levelNum)
		{
			value = VariantUtils.CreateFrom(in levelNum);
			return true;
		}
		if (name == PropertyName.maxPage)
		{
			value = VariantUtils.CreateFrom(in maxPage);
			return true;
		}
		if (name == PropertyName.currentPage)
		{
			value = VariantUtils.CreateFrom(in currentPage);
			return true;
		}
		if (name == PropertyName.currentPageList)
		{
			value = VariantUtils.CreateFrom(in currentPageList);
			return true;
		}
		if (name == PropertyName.suffix)
		{
			value = VariantUtils.CreateFrom(in suffix);
			return true;
		}
		if (name == PropertyName.suffix2)
		{
			value = VariantUtils.CreateFrom(in suffix2);
			return true;
		}
		if (name == PropertyName.search)
		{
			value = VariantUtils.CreateFrom(in search);
			return true;
		}
		if (name == PropertyName.tags)
		{
			value = VariantUtils.CreateFrom(in tags);
			return true;
		}
		if (name == PropertyName.currentLevelId)
		{
			value = VariantUtils.CreateFrom(in currentLevelId);
			return true;
		}
		if (name == PropertyName.myCollection)
		{
			value = VariantUtils.CreateFrom(in myCollection);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._authorUid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._authorName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._requestedPage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsAuthorPage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelGetHTTPRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelInformationGetHTTPRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._onlineLevelContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelIdLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._enterLevelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jumpPageToSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jumpPageToButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._tagsOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pageLoading, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.levelNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxPage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentPage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.currentPageList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.suffix, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.suffix2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.search, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.tags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentLevelId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.myCollection, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._authorUid, Variant.From(in _authorUid));
		info.AddProperty(PropertyName._authorName, Variant.From(in _authorName));
		info.AddProperty(PropertyName._requestedPage, Variant.From(in _requestedPage));
		info.AddProperty(PropertyName._levelGetHTTPRequest, Variant.From(in _levelGetHTTPRequest));
		info.AddProperty(PropertyName._levelInformationGetHTTPRequest, Variant.From(in _levelInformationGetHTTPRequest));
		info.AddProperty(PropertyName._onlineLevelContainer, Variant.From(in _onlineLevelContainer));
		info.AddProperty(PropertyName._levelContainer, Variant.From(in _levelContainer));
		info.AddProperty(PropertyName._pageLabel, Variant.From(in _pageLabel));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._levelIdLineEdit, Variant.From(in _levelIdLineEdit));
		info.AddProperty(PropertyName._enterLevelButton, Variant.From(in _enterLevelButton));
		info.AddProperty(PropertyName._jumpPageToSpinBox, Variant.From(in _jumpPageToSpinBox));
		info.AddProperty(PropertyName._jumpPageToButton, Variant.From(in _jumpPageToButton));
		info.AddProperty(PropertyName._typeOptionButton, Variant.From(in _typeOptionButton));
		info.AddProperty(PropertyName._tagsOptionButton, Variant.From(in _tagsOptionButton));
		info.AddProperty(PropertyName._pageLoading, Variant.From(in _pageLoading));
		info.AddProperty(PropertyName.levelNum, Variant.From(in levelNum));
		info.AddProperty(PropertyName.maxPage, Variant.From(in maxPage));
		info.AddProperty(PropertyName.currentPage, Variant.From(in currentPage));
		info.AddProperty(PropertyName.currentPageList, Variant.From(in currentPageList));
		info.AddProperty(PropertyName.suffix, Variant.From(in suffix));
		info.AddProperty(PropertyName.suffix2, Variant.From(in suffix2));
		info.AddProperty(PropertyName.search, Variant.From(in search));
		info.AddProperty(PropertyName.tags, Variant.From(in tags));
		info.AddProperty(PropertyName.currentLevelId, Variant.From(in currentLevelId));
		info.AddProperty(PropertyName.myCollection, Variant.From(in myCollection));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._authorUid, out var value))
		{
			_authorUid = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._authorName, out var value2))
		{
			_authorName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName._requestedPage, out var value3))
		{
			_requestedPage = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._levelGetHTTPRequest, out var value4))
		{
			_levelGetHTTPRequest = value4.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName._levelInformationGetHTTPRequest, out var value5))
		{
			_levelInformationGetHTTPRequest = value5.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName._onlineLevelContainer, out var value6))
		{
			_onlineLevelContainer = value6.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._levelContainer, out var value7))
		{
			_levelContainer = value7.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName._pageLabel, out var value8))
		{
			_pageLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value9))
		{
			_statusLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._levelIdLineEdit, out var value10))
		{
			_levelIdLineEdit = value10.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._enterLevelButton, out var value11))
		{
			_enterLevelButton = value11.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._jumpPageToSpinBox, out var value12))
		{
			_jumpPageToSpinBox = value12.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._jumpPageToButton, out var value13))
		{
			_jumpPageToButton = value13.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._typeOptionButton, out var value14))
		{
			_typeOptionButton = value14.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._tagsOptionButton, out var value15))
		{
			_tagsOptionButton = value15.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._pageLoading, out var value16))
		{
			_pageLoading = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.levelNum, out var value17))
		{
			levelNum = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName.maxPage, out var value18))
		{
			maxPage = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentPage, out var value19))
		{
			currentPage = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentPageList, out var value20))
		{
			currentPageList = value20.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.suffix, out var value21))
		{
			suffix = value21.As<string>();
		}
		if (info.TryGetProperty(PropertyName.suffix2, out var value22))
		{
			suffix2 = value22.As<string>();
		}
		if (info.TryGetProperty(PropertyName.search, out var value23))
		{
			search = value23.As<string>();
		}
		if (info.TryGetProperty(PropertyName.tags, out var value24))
		{
			tags = value24.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentLevelId, out var value25))
		{
			currentLevelId = value25.As<string>();
		}
		if (info.TryGetProperty(PropertyName.myCollection, out var value26))
		{
			myCollection = value26.As<bool>();
		}
	}
}
