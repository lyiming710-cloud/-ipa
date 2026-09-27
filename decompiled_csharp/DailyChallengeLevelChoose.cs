using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/DialogBox/DailyChallenge/LevelChoose/DailyChallengeLevelChoose.cs")]
public class DailyChallengeLevelChoose : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PopulateCards = "PopulateCards";

		public static readonly StringName ClearCards = "ClearCards";

		public static readonly StringName ResetPagination = "ResetPagination";

		public static readonly StringName PreviousPageButtonPressed = "PreviousPageButtonPressed";

		public static readonly StringName NextPageButtonPressed = "NextPageButtonPressed";

		public static readonly StringName NavigateToPage = "NavigateToPage";

		public static readonly StringName PageTweenFinished = "PageTweenFinished";

		public static readonly StringName CardsScrollValueChanged = "CardsScrollValueChanged";

		public static readonly StringName FindNearestPage = "FindNearestPage";

		public static readonly StringName GetPageScrollTarget = "GetPageScrollTarget";

		public static readonly StringName StopPageTween = "StopPageTween";

		public static readonly StringName RefreshPaginationUi = "RefreshPaginationUi";

		public static readonly StringName SetPageButtonState = "SetPageButtonState";

		public static readonly StringName CardSelected = "CardSelected";

		public static readonly StringName SelectCard = "SelectCard";

		public static readonly StringName PlayButtonPressed = "PlayButtonPressed";

		public static readonly StringName LevelHttpRequestCompleted = "LevelHttpRequestCompleted";

		public static readonly StringName HasServerError = "HasServerError";

		public static readonly StringName SetBusy = "SetBusy";

		public static readonly StringName RestoreAfterLoadError = "RestoreAfterLoadError";

		public static readonly StringName SetStatus = "SetStatus";

		public static readonly StringName RefreshStatusLabel = "RefreshStatusLabel";

		public static readonly StringName IsLevelFinished = "IsLevelFinished";

		public static readonly StringName CloseButtonPressed = "CloseButtonPressed";

		public static readonly StringName ToBattle = "ToBattle";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName Cards = "Cards";

		public static readonly StringName SelectedCard = "SelectedCard";

		public static readonly StringName CardsScroll = "CardsScroll";

		public static readonly StringName IsBusy = "IsBusy";

		public static readonly StringName SelectedLevelId = "SelectedLevelId";

		public static readonly StringName StatusText = "StatusText";

		public static readonly StringName _cards = "_cards";

		public static readonly StringName _cardsContainer = "_cardsContainer";

		public static readonly StringName _cardsScroll = "_cardsScroll";

		public static readonly StringName _dateLabel = "_dateLabel";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _closeButton = "_closeButton";

		public static readonly StringName _previousPageButton = "_previousPageButton";

		public static readonly StringName _nextPageButton = "_nextPageButton";

		public static readonly StringName _horizontalScrollBar = "_horizontalScrollBar";

		public static readonly StringName _selectedCard = "_selectedCard";

		public static readonly StringName _pageTween = "_pageTween";

		public static readonly StringName _isBusy = "_isBusy";

		public static readonly StringName _isPaging = "_isPaging";

		public static readonly StringName _statusIsError = "_statusIsError";

		public static readonly StringName _currentPage = "_currentPage";

		public static readonly StringName _pageCount = "_pageCount";

		public static readonly StringName _statusMessage = "_statusMessage";

		public static readonly StringName _pendingDate = "_pendingDate";

		public static readonly StringName _pendingLevelId = "_pendingLevelId";

		public static readonly StringName _pendingMetadata = "_pendingMetadata";

		public static readonly StringName date = "date";

		public static readonly StringName levelList = "levelList";

		public static readonly StringName levelHttpRequest = "levelHttpRequest";

		public static readonly StringName playButton = "playButton";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	private const string DailyLevelApiOrigin = "https://api.pvzhe.com";

	private const int CardsPerPage = 2;

	private const double PageTweenDuration = 0.2;

	private static readonly Color PageButtonDisabledColor = new Color(0.48f, 0.48f, 0.48f, 0.7f);

	private static PackedScene _dailyChallengeLevelCardScene;

	private readonly Array<DailyChallengeLevelPreview> _cards = new Array<DailyChallengeLevelPreview>();

	private HBoxContainer _cardsContainer;

	private ScrollContainer _cardsScroll;

	private Label _dateLabel;

	private Label _statusLabel;

	private TextureButton _closeButton;

	private TextureButton _previousPageButton;

	private TextureButton _nextPageButton;

	private ScrollBar _horizontalScrollBar;

	private DailyChallengeLevelPreview _selectedCard;

	private Tween _pageTween;

	private bool _isBusy;

	private bool _isPaging;

	private bool _statusIsError;

	private int _currentPage;

	private int _pageCount;

	private string _statusMessage = "";

	private string _pendingDate = "";

	private string _pendingLevelId = "";

	private Dictionary _pendingMetadata = new Dictionary();

	[Export(PropertyHint.None, "")]
	public string date = "";

	public Godot.Collections.Array levelList = new Godot.Collections.Array();

	public NativeHttpRequest levelHttpRequest;

	public MainButton playButton;

	private static PackedScene DAILY_CHALLENGE_LEVEL_CARD_SCENE => _dailyChallengeLevelCardScene ?? (_dailyChallengeLevelCardScene = GD.Load<PackedScene>("uid://8c5010cwpik"));

	public Array<DailyChallengeLevelPreview> Cards => _cards;

	public DailyChallengeLevelPreview SelectedCard => _selectedCard;

	public ScrollContainer CardsScroll => _cardsScroll;

	public bool IsBusy => _isBusy;

	public string SelectedLevelId => _selectedCard?.LevelId ?? "";

	public string StatusText
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_statusLabel))
			{
				return "";
			}
			return _statusLabel.Text;
		}
	}

	internal Func<string, string[], Error> LevelRequestOverride { get; set; }

	internal Action<Json> BattleStartOverride { get; set; }

	public override void Init(Dictionary data)
	{
		date = data?.GetValueOrDefault("date", "").AsString() ?? "";
	}

	public override void _Ready()
	{
		base._Ready();
		_cardsContainer = GetNode<HBoxContainer>("%CardsContainer");
		_cardsScroll = GetNode<ScrollContainer>("%CardsScroll");
		_dateLabel = GetNode<Label>("%DateLabel");
		_statusLabel = GetNode<Label>("%StatusLabel");
		_closeButton = GetNode<TextureButton>("%CloseButton");
		_previousPageButton = GetNode<TextureButton>("%PreviousPageButton");
		_nextPageButton = GetNode<TextureButton>("%NextPageButton");
		_horizontalScrollBar = _cardsScroll.GetHScrollBar();
		playButton = GetNode<MainButton>("%PlayButton");
		levelHttpRequest = new NativeHttpRequest();
		AddChild(levelHttpRequest, forceReadableName: false, InternalMode.Disabled);
		levelHttpRequest.RequestCompleted += LevelHttpRequestCompleted;
		playButton.Pressed += PlayButtonPressed;
		_closeButton.Pressed += CloseButtonPressed;
		_previousPageButton.Pressed += PreviousPageButtonPressed;
		_nextPageButton.Pressed += NextPageButtonPressed;
		_horizontalScrollBar.ValueChanged += CardsScrollValueChanged;
		PopulateCards();
	}

	public void PopulateCards()
	{
		ClearCards();
		_selectedCard = null;
		_statusMessage = "";
		_statusIsError = false;
		levelList = new Godot.Collections.Array();
		playButton.Disabled = true;
		_dateLabel.Text = date;
		if (string.IsNullOrWhiteSpace(date))
		{
			SetStatus("未指定挑战日期", error: true);
			return;
		}
		if (!GodotObject.IsInstanceValid(ResourceManager.Instance))
		{
			SetStatus("关卡数据尚未加载", error: true);
			return;
		}
		System.Collections.Generic.Dictionary<string, Variant> dAILY_LEVEL_DATA = ResourceManager.Instance.DAILY_LEVEL_DATA;
		if (dAILY_LEVEL_DATA == null || !dAILY_LEVEL_DATA.ContainsKey("LevelDateMap") || dAILY_LEVEL_DATA["LevelDateMap"].VariantType != Variant.Type.Dictionary || !dAILY_LEVEL_DATA.ContainsKey("LevelMeta") || dAILY_LEVEL_DATA["LevelMeta"].VariantType != Variant.Type.Dictionary)
		{
			SetStatus("关卡数据尚未加载", error: true);
			return;
		}
		Dictionary dictionary = dAILY_LEVEL_DATA["LevelDateMap"].AsGodotDictionary();
		Dictionary dictionary2 = dAILY_LEVEL_DATA["LevelMeta"].AsGodotDictionary();
		if (!dictionary.ContainsKey(date) || dictionary[date].VariantType != Variant.Type.Array)
		{
			SetStatus("当日暂无可用关卡", error: true);
			return;
		}
		levelList = dictionary[date].AsGodotArray().Duplicate(deep: true);
		PackedScene dAILY_CHALLENGE_LEVEL_CARD_SCENE = DAILY_CHALLENGE_LEVEL_CARD_SCENE;
		if (!GodotObject.IsInstanceValid(dAILY_CHALLENGE_LEVEL_CARD_SCENE))
		{
			SetStatus("关卡预览资源加载失败", error: true);
			return;
		}
		foreach (Variant level in levelList)
		{
			string text = level.AsString();
			Dictionary metadata = new Dictionary();
			if (!string.IsNullOrWhiteSpace(text) && dictionary2.ContainsKey(text) && dictionary2[text].VariantType == Variant.Type.Dictionary)
			{
				metadata = dictionary2[text].AsGodotDictionary().Duplicate(deep: true);
			}
			DailyChallengeLevelPreview dailyChallengeLevelPreview = dAILY_CHALLENGE_LEVEL_CARD_SCENE.Instantiate<DailyChallengeLevelPreview>(PackedScene.GenEditState.Disabled);
			dailyChallengeLevelPreview.Bind(text, metadata, IsLevelFinished("DailyLevel-" + date + "-" + text));
			dailyChallengeLevelPreview.OnSelected += CardSelected;
			_cardsContainer.AddChild(dailyChallengeLevelPreview, forceReadableName: false, InternalMode.Disabled);
			_cards.Add(dailyChallengeLevelPreview);
		}
		ResetPagination();
		foreach (DailyChallengeLevelPreview card in _cards)
		{
			if (card.IsAvailable)
			{
				SelectCard(card, playAudio: false);
				return;
			}
		}
		SetStatus((levelList.Count == 0) ? "当日暂无可用关卡" : "当日关卡数据不可用", error: true);
	}

	private void ClearCards()
	{
		StopPageTween();
		_currentPage = 0;
		_pageCount = 0;
		if (GodotObject.IsInstanceValid(_cardsScroll))
		{
			_cardsScroll.ScrollHorizontal = 0;
		}
		foreach (DailyChallengeLevelPreview card in _cards)
		{
			if (GodotObject.IsInstanceValid(card))
			{
				card.OnSelected -= CardSelected;
				if (card.GetParent() == _cardsContainer)
				{
					_cardsContainer.RemoveChild(card);
				}
				card.QueueFree();
			}
		}
		_cards.Clear();
		RefreshPaginationUi();
	}

	private void ResetPagination()
	{
		StopPageTween();
		_currentPage = 0;
		_pageCount = Mathf.CeilToInt((float)_cards.Count / 2f);
		_cardsScroll.ScrollHorizontal = 0;
		RefreshPaginationUi();
	}

	private void PreviousPageButtonPressed()
	{
		NavigateToPage(_currentPage - 1);
	}

	private void NextPageButtonPressed()
	{
		NavigateToPage(_currentPage + 1);
	}

	private void NavigateToPage(int targetPage)
	{
		if (_isBusy || _isPaging || _pageCount <= 1)
		{
			return;
		}
		targetPage = Mathf.Clamp(targetPage, 0, _pageCount - 1);
		if (targetPage == _currentPage)
		{
			return;
		}
		_currentPage = targetPage;
		int pageScrollTarget = GetPageScrollTarget(targetPage);
		if (pageScrollTarget == _cardsScroll.ScrollHorizontal)
		{
			RefreshPaginationUi();
			return;
		}
		_isPaging = true;
		RefreshPaginationUi();
		if (GodotObject.IsInstanceValid(AudioManager.Instance))
		{
			AudioManager.Instance.AudioPlay("ButtonPress");
		}
		_pageTween = CreateTween();
		_pageTween.SetTrans(Tween.TransitionType.Quart);
		_pageTween.SetEase(Tween.EaseType.Out);
		_pageTween.TweenProperty(_cardsScroll, "scroll_horizontal", pageScrollTarget, 0.2);
		_pageTween.Finished += PageTweenFinished;
	}

	private void PageTweenFinished()
	{
		_pageTween = null;
		_isPaging = false;
		_cardsScroll.ScrollHorizontal = GetPageScrollTarget(_currentPage);
		RefreshPaginationUi();
	}

	private void CardsScrollValueChanged(double value)
	{
		if (!_isPaging && _pageCount > 1)
		{
			int num = FindNearestPage(value);
			if (num != _currentPage)
			{
				_currentPage = num;
				RefreshPaginationUi();
			}
		}
	}

	private int FindNearestPage(double scrollValue)
	{
		int result = 0;
		double num = 1.7976931348623157E+308;
		for (int i = 0; i < _pageCount; i++)
		{
			double num2 = Math.Abs(scrollValue - (double)GetPageScrollTarget(i));
			if (!(num2 >= num))
			{
				result = i;
				num = num2;
			}
		}
		return result;
	}

	private int GetPageScrollTarget(int page)
	{
		if (!GodotObject.IsInstanceValid(_horizontalScrollBar) || !GodotObject.IsInstanceValid(_cardsScroll))
		{
			return 0;
		}
		double val = Math.Max(0.0, _horizontalScrollBar.MaxValue - _horizontalScrollBar.Page);
		return Mathf.RoundToInt(Math.Min((float)Mathf.Clamp(page, 0, Math.Max(0, _pageCount - 1)) * _cardsScroll.Size.X, val));
	}

	private void StopPageTween()
	{
		if (GodotObject.IsInstanceValid(_pageTween))
		{
			_pageTween.Kill();
		}
		_pageTween = null;
		_isPaging = false;
	}

	private void RefreshPaginationUi()
	{
		bool flag = _pageCount > 1;
		if (GodotObject.IsInstanceValid(_previousPageButton))
		{
			_previousPageButton.Visible = flag;
			SetPageButtonState(_previousPageButton, !flag || _isBusy || _isPaging || _currentPage <= 0);
		}
		if (GodotObject.IsInstanceValid(_nextPageButton))
		{
			_nextPageButton.Visible = flag;
			SetPageButtonState(_nextPageButton, !flag || _isBusy || _isPaging || _currentPage >= _pageCount - 1);
		}
		RefreshStatusLabel();
	}

	private static void SetPageButtonState(TextureButton button, bool disabled)
	{
		button.Disabled = disabled;
		button.Modulate = (disabled ? PageButtonDisabledColor : Colors.White);
		button.MouseDefaultCursorShape = (CursorShape)(disabled ? 0 : 2);
	}

	private void CardSelected(DailyChallengeLevelPreview card)
	{
		if (!_isBusy && GodotObject.IsInstanceValid(card) && card.IsAvailable)
		{
			SelectCard(card, playAudio: true);
		}
	}

	private void SelectCard(DailyChallengeLevelPreview card, bool playAudio)
	{
		if (!GodotObject.IsInstanceValid(card) || !card.IsAvailable)
		{
			return;
		}
		foreach (DailyChallengeLevelPreview card2 in _cards)
		{
			card2.SetSelected(card2 == card);
		}
		_selectedCard = card;
		playButton.Disabled = false;
		SetStatus("已选择：" + card.DisplayName);
		if (playAudio && GodotObject.IsInstanceValid(AudioManager.Instance))
		{
			AudioManager.Instance.AudioPlay("ButtonPress");
		}
	}

	public void PlayButtonPressed()
	{
		if (_isBusy || !GodotObject.IsInstanceValid(_selectedCard) || !_selectedCard.IsAvailable)
		{
			return;
		}
		_pendingDate = date;
		_pendingLevelId = _selectedCard.LevelId;
		_pendingMetadata = _selectedCard.Metadata.Duplicate(deep: true);
		SetBusy(busy: true);
		if (!TryBuildDailyLevelUrl(_selectedCard.ApiPath, out var levelUrl))
		{
			RestoreAfterLoadError("关卡下载地址无效");
			return;
		}
		string[] array = (GodotObject.IsInstanceValid(Global.Instance) ? Global.Instance.header : System.Array.Empty<string>());
		Error error = ((LevelRequestOverride != null) ? LevelRequestOverride(levelUrl, array) : levelHttpRequest.Request(levelUrl, array));
		if (error != Error.Ok)
		{
			RestoreAfterLoadError($"无法开始下载关卡（{error}）");
		}
	}

	private static bool TryBuildDailyLevelUrl(string apiPath, out string levelUrl)
	{
		levelUrl = "";
		if (!DailyChallengeLevelPreview.IsValidApiPath(apiPath))
		{
			return false;
		}
		if (!Uri.TryCreate("https://api.pvzhe.com", UriKind.Absolute, out Uri result) || !Uri.TryCreate(result, apiPath, out Uri result2) || !string.Equals(result2.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) || !string.Equals(result2.Host, result.Host, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		levelUrl = result2.AbsoluteUri;
		return true;
	}

	public void LevelHttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (!_isBusy)
		{
			return;
		}
		if (result != 0L)
		{
			string message = (GodotObject.IsInstanceValid(Global.Instance) ? Global.Instance.GetHTTPRequestErrorMessage(result) : "获取关卡失败，请重新尝试");
			RestoreAfterLoadError(message);
			return;
		}
		if (responseCode < 200 || responseCode >= 300)
		{
			RestoreAfterLoadError($"获取关卡失败（HTTP {responseCode}）");
			return;
		}
		Json json = new Json();
		if (json.Parse(body?.GetStringFromUtf8() ?? "") != Error.Ok || json.Data.VariantType != Variant.Type.Dictionary)
		{
			RestoreAfterLoadError("关卡数据格式错误，请重新尝试");
			return;
		}
		Dictionary dictionary = json.Data.AsGodotDictionary();
		if (HasServerError(dictionary))
		{
			RestoreAfterLoadError("服务器未返回有效关卡，请重新尝试");
			return;
		}
		if (!_pendingMetadata.ContainsKey("reward") || _pendingMetadata["reward"].VariantType != Variant.Type.Dictionary)
		{
			RestoreAfterLoadError("关卡奖励数据无效，请重新尝试");
			return;
		}
		Dictionary dictionary2 = _pendingMetadata["reward"].AsGodotDictionary();
		string text = dictionary2.GetValueOrDefault("type", "").AsString();
		if (string.IsNullOrWhiteSpace(text) || !dictionary2.ContainsKey("value"))
		{
			RestoreAfterLoadError("关卡奖励数据无效，请重新尝试");
			return;
		}
		string text2 = "DailyLevel-" + _pendingDate + "-" + _pendingLevelId;
		dictionary["Name"] = text2;
		dictionary["Reward"] = new Dictionary
		{
			["RewardType"] = text,
			["RewardFirst"] = dictionary2["value"]
		};
		Json json2 = new Json();
		if (json2.Parse(Json.Stringify(dictionary)) != Error.Ok || json2.Data.VariantType != Variant.Type.Dictionary)
		{
			RestoreAfterLoadError("关卡数据处理失败，请重新尝试");
			return;
		}
		GameSaveManager.Instance.SaveDailyLevel(text2, json2);
		_closeButton.Disabled = true;
		SetStatus("关卡加载完成，正在进入…");
		if (BattleStartOverride != null)
		{
			BattleStartOverride(json2);
		}
		else
		{
			ToBattle(json2);
		}
	}

	private static bool HasServerError(Dictionary data)
	{
		if (data == null || !data.ContainsKey("error") || data["error"].VariantType == Variant.Type.Nil)
		{
			return false;
		}
		Variant variant = data["error"];
		Variant.Type variantType = variant.VariantType;
		Variant.Type num = variantType - 1;
		if ((ulong)num <= 3uL)
		{
			switch ((int)num)
			{
			case 0:
				return variant.AsBool();
			case 1:
				return variant.AsInt64() != 0;
			case 2:
				return !Mathf.IsZeroApprox(variant.AsDouble());
			case 3:
				return !string.IsNullOrWhiteSpace(variant.AsString());
			}
		}
		return true;
	}

	private void SetBusy(bool busy)
	{
		_isBusy = busy;
		foreach (DailyChallengeLevelPreview card in _cards)
		{
			card.SetInteractionEnabled(!busy);
		}
		playButton.Disabled = busy || !GodotObject.IsInstanceValid(_selectedCard) || !_selectedCard.IsAvailable;
		if (busy)
		{
			SetStatus("正在加载关卡…");
		}
		RefreshPaginationUi();
	}

	private void RestoreAfterLoadError(string message)
	{
		levelHttpRequest?.CancelRequest();
		_isBusy = false;
		foreach (DailyChallengeLevelPreview card in _cards)
		{
			card.SetInteractionEnabled(enabled: true);
		}
		playButton.Disabled = !GodotObject.IsInstanceValid(_selectedCard) || !_selectedCard.IsAvailable;
		_pendingDate = "";
		_pendingLevelId = "";
		_pendingMetadata = new Dictionary();
		SetStatus(message, error: true);
		RefreshPaginationUi();
		if (GodotObject.IsInstanceValid(BroadCastManager.Instance))
		{
			BroadCastManager.Instance.BroadCastFloatCreate(message, Colors.Red);
		}
	}

	private void SetStatus(string text, bool error = false)
	{
		_statusMessage = text ?? "";
		_statusIsError = error;
		RefreshStatusLabel();
	}

	private void RefreshStatusLabel()
	{
		if (GodotObject.IsInstanceValid(_statusLabel))
		{
			Label statusLabel = _statusLabel;
			string text;
			if (_pageCount <= 1)
			{
				text = _statusMessage;
			}
			else
			{
				text = (string.IsNullOrWhiteSpace(_statusMessage) ? $"第 {_currentPage + 1}/{_pageCount} 页" : $"第 {_currentPage + 1}/{_pageCount} 页 · {_statusMessage}");
			}
			statusLabel.Text = text;
			_statusLabel.AddThemeColorOverride("font_color", _statusIsError ? new Color(0.78f, 0.05f, 0.02f) : new Color(0.12f, 0.38f, 0.08f));
		}
	}

	private static bool IsLevelFinished(string levelName)
	{
		GameSaveManager instance = GameSaveManager.Instance;
		if (!GodotObject.IsInstanceValid(instance) || instance.config == null || string.IsNullOrWhiteSpace(levelName))
		{
			return false;
		}
		string userCurrent = instance.GetUserCurrent();
		if (string.IsNullOrWhiteSpace(userCurrent) || instance.config.saveDictionary == null || !instance.config.saveDictionary.ContainsKey(userCurrent))
		{
			return false;
		}
		Variant variant = instance.config.saveDictionary[userCurrent];
		if (variant.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		Dictionary dictionary = variant.AsGodotDictionary();
		if (!dictionary.ContainsKey("Level") || dictionary["Level"].VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		Dictionary dictionary2 = dictionary["Level"].AsGodotDictionary();
		if (!dictionary2.ContainsKey(levelName) || dictionary2[levelName].VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		Dictionary dictionary3 = dictionary2[levelName].AsGodotDictionary();
		if (!dictionary3.ContainsKey("Key") || dictionary3["Key"].VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		return dictionary3["Key"].AsGodotDictionary().GetValueOrDefault("Finish", 0).AsInt32() > 0;
	}

	public void CloseButtonPressed()
	{
		levelHttpRequest?.CancelRequest();
		if (GodotObject.IsInstanceValid(AudioManager.Instance))
		{
			AudioManager.Instance.AudioPlay("ButtonPress");
		}
		CloseDialog();
	}

	public async void ToBattle(Json json)
	{
		TowerDefenseLevelConfig towerDefenseLevelConfig = new TowerDefenseLevelConfig
		{
			data = json
		};
		towerDefenseLevelConfig.Init();
		TowerDefenseManager.Instance.currentLevelConfig = towerDefenseLevelConfig;
		Global.Instance.enterLevelMode = "DailyLevel";
		if (Global.IsMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			await MultiPlayerManager.Instance.StartCompatibleGameAsync();
		}
		else
		{
			SceneManager.Instance.ChangeScene("TowerDefense");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(27)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetPagination, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviousPageButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextPageButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NavigateToPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "targetPage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PageTweenFinished, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CardsScrollValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindNearestPage, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "scrollValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPageScrollTarget, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "page", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StopPageTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPaginationUi, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPageButtonState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureButton"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "disabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CardSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "card", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SelectCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "card", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LevelHttpRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasServerError, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetBusy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "busy", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreAfterLoadError, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshStatusLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsLevelFinished, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToBattle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "json", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false)
			}, null)
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
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateCards && args.Count == 0)
		{
			PopulateCards();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearCards && args.Count == 0)
		{
			ClearCards();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetPagination && args.Count == 0)
		{
			ResetPagination();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviousPageButtonPressed && args.Count == 0)
		{
			PreviousPageButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.NextPageButtonPressed && args.Count == 0)
		{
			NextPageButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateToPage && args.Count == 1)
		{
			NavigateToPage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PageTweenFinished && args.Count == 0)
		{
			PageTweenFinished();
			ret = default;
			return true;
		}
		if (method == MethodName.CardsScrollValueChanged && args.Count == 1)
		{
			CardsScrollValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindNearestPage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindNearestPage(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPageScrollTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetPageScrollTarget(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.StopPageTween && args.Count == 0)
		{
			StopPageTween();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPaginationUi && args.Count == 0)
		{
			RefreshPaginationUi();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPageButtonState && args.Count == 2)
		{
			SetPageButtonState(VariantUtils.ConvertTo<TextureButton>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CardSelected && args.Count == 1)
		{
			CardSelected(VariantUtils.ConvertTo<DailyChallengeLevelPreview>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectCard && args.Count == 2)
		{
			SelectCard(VariantUtils.ConvertTo<DailyChallengeLevelPreview>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayButtonPressed && args.Count == 0)
		{
			PlayButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LevelHttpRequestCompleted && args.Count == 4)
		{
			LevelHttpRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasServerError && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasServerError(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.SetBusy && args.Count == 1)
		{
			SetBusy(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreAfterLoadError && args.Count == 1)
		{
			RestoreAfterLoadError(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetStatus && args.Count == 2)
		{
			SetStatus(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshStatusLabel && args.Count == 0)
		{
			RefreshStatusLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.IsLevelFinished && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLevelFinished(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CloseButtonPressed && args.Count == 0)
		{
			CloseButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ToBattle && args.Count == 1)
		{
			ToBattle(VariantUtils.ConvertTo<Json>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetPageButtonState && args.Count == 2)
		{
			SetPageButtonState(VariantUtils.ConvertTo<TextureButton>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasServerError && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasServerError(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.IsLevelFinished && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLevelFinished(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.PopulateCards)
		{
			return true;
		}
		if (method == MethodName.ClearCards)
		{
			return true;
		}
		if (method == MethodName.ResetPagination)
		{
			return true;
		}
		if (method == MethodName.PreviousPageButtonPressed)
		{
			return true;
		}
		if (method == MethodName.NextPageButtonPressed)
		{
			return true;
		}
		if (method == MethodName.NavigateToPage)
		{
			return true;
		}
		if (method == MethodName.PageTweenFinished)
		{
			return true;
		}
		if (method == MethodName.CardsScrollValueChanged)
		{
			return true;
		}
		if (method == MethodName.FindNearestPage)
		{
			return true;
		}
		if (method == MethodName.GetPageScrollTarget)
		{
			return true;
		}
		if (method == MethodName.StopPageTween)
		{
			return true;
		}
		if (method == MethodName.RefreshPaginationUi)
		{
			return true;
		}
		if (method == MethodName.SetPageButtonState)
		{
			return true;
		}
		if (method == MethodName.CardSelected)
		{
			return true;
		}
		if (method == MethodName.SelectCard)
		{
			return true;
		}
		if (method == MethodName.PlayButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LevelHttpRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.HasServerError)
		{
			return true;
		}
		if (method == MethodName.SetBusy)
		{
			return true;
		}
		if (method == MethodName.RestoreAfterLoadError)
		{
			return true;
		}
		if (method == MethodName.SetStatus)
		{
			return true;
		}
		if (method == MethodName.RefreshStatusLabel)
		{
			return true;
		}
		if (method == MethodName.IsLevelFinished)
		{
			return true;
		}
		if (method == MethodName.CloseButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ToBattle)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._cardsContainer)
		{
			_cardsContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._cardsScroll)
		{
			_cardsScroll = VariantUtils.ConvertTo<ScrollContainer>(in value);
			return true;
		}
		if (name == PropertyName._dateLabel)
		{
			_dateLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._closeButton)
		{
			_closeButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName._previousPageButton)
		{
			_previousPageButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName._nextPageButton)
		{
			_nextPageButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName._horizontalScrollBar)
		{
			_horizontalScrollBar = VariantUtils.ConvertTo<ScrollBar>(in value);
			return true;
		}
		if (name == PropertyName._selectedCard)
		{
			_selectedCard = VariantUtils.ConvertTo<DailyChallengeLevelPreview>(in value);
			return true;
		}
		if (name == PropertyName._pageTween)
		{
			_pageTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._isBusy)
		{
			_isBusy = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isPaging)
		{
			_isPaging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._statusIsError)
		{
			_statusIsError = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._currentPage)
		{
			_currentPage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pageCount)
		{
			_pageCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._statusMessage)
		{
			_statusMessage = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pendingDate)
		{
			_pendingDate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pendingLevelId)
		{
			_pendingLevelId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pendingMetadata)
		{
			_pendingMetadata = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.date)
		{
			date = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.levelList)
		{
			levelList = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.levelHttpRequest)
		{
			levelHttpRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.playButton)
		{
			playButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Cards)
		{
			value = VariantUtils.CreateFromArray(Cards);
			return true;
		}
		if (name == PropertyName.SelectedCard)
		{
			value = VariantUtils.CreateFrom<DailyChallengeLevelPreview>(SelectedCard);
			return true;
		}
		if (name == PropertyName.CardsScroll)
		{
			value = VariantUtils.CreateFrom<ScrollContainer>(CardsScroll);
			return true;
		}
		if (name == PropertyName.IsBusy)
		{
			value = VariantUtils.CreateFrom<bool>(IsBusy);
			return true;
		}
		string from;
		if (name == PropertyName.SelectedLevelId)
		{
			from = SelectedLevelId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.StatusText)
		{
			from = StatusText;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._cards)
		{
			value = VariantUtils.CreateFromArray(_cards);
			return true;
		}
		if (name == PropertyName._cardsContainer)
		{
			value = VariantUtils.CreateFrom(in _cardsContainer);
			return true;
		}
		if (name == PropertyName._cardsScroll)
		{
			value = VariantUtils.CreateFrom(in _cardsScroll);
			return true;
		}
		if (name == PropertyName._dateLabel)
		{
			value = VariantUtils.CreateFrom(in _dateLabel);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._closeButton)
		{
			value = VariantUtils.CreateFrom(in _closeButton);
			return true;
		}
		if (name == PropertyName._previousPageButton)
		{
			value = VariantUtils.CreateFrom(in _previousPageButton);
			return true;
		}
		if (name == PropertyName._nextPageButton)
		{
			value = VariantUtils.CreateFrom(in _nextPageButton);
			return true;
		}
		if (name == PropertyName._horizontalScrollBar)
		{
			value = VariantUtils.CreateFrom(in _horizontalScrollBar);
			return true;
		}
		if (name == PropertyName._selectedCard)
		{
			value = VariantUtils.CreateFrom(in _selectedCard);
			return true;
		}
		if (name == PropertyName._pageTween)
		{
			value = VariantUtils.CreateFrom(in _pageTween);
			return true;
		}
		if (name == PropertyName._isBusy)
		{
			value = VariantUtils.CreateFrom(in _isBusy);
			return true;
		}
		if (name == PropertyName._isPaging)
		{
			value = VariantUtils.CreateFrom(in _isPaging);
			return true;
		}
		if (name == PropertyName._statusIsError)
		{
			value = VariantUtils.CreateFrom(in _statusIsError);
			return true;
		}
		if (name == PropertyName._currentPage)
		{
			value = VariantUtils.CreateFrom(in _currentPage);
			return true;
		}
		if (name == PropertyName._pageCount)
		{
			value = VariantUtils.CreateFrom(in _pageCount);
			return true;
		}
		if (name == PropertyName._statusMessage)
		{
			value = VariantUtils.CreateFrom(in _statusMessage);
			return true;
		}
		if (name == PropertyName._pendingDate)
		{
			value = VariantUtils.CreateFrom(in _pendingDate);
			return true;
		}
		if (name == PropertyName._pendingLevelId)
		{
			value = VariantUtils.CreateFrom(in _pendingLevelId);
			return true;
		}
		if (name == PropertyName._pendingMetadata)
		{
			value = VariantUtils.CreateFrom(in _pendingMetadata);
			return true;
		}
		if (name == PropertyName.date)
		{
			value = VariantUtils.CreateFrom(in date);
			return true;
		}
		if (name == PropertyName.levelList)
		{
			value = VariantUtils.CreateFrom(in levelList);
			return true;
		}
		if (name == PropertyName.levelHttpRequest)
		{
			value = VariantUtils.CreateFrom(in levelHttpRequest);
			return true;
		}
		if (name == PropertyName.playButton)
		{
			value = VariantUtils.CreateFrom(in playButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._cards, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardsContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardsScroll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._dateLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._closeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousPageButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextPageButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._horizontalScrollBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedCard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pageTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isBusy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isPaging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._statusIsError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentPage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pageCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._statusMessage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingDate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingLevelId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._pendingMetadata, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.date, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.levelList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelHttpRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.playButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.Cards, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.SelectedCard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CardsScroll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsBusy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.SelectedLevelId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.StatusText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._cardsContainer, Variant.From(in _cardsContainer));
		info.AddProperty(PropertyName._cardsScroll, Variant.From(in _cardsScroll));
		info.AddProperty(PropertyName._dateLabel, Variant.From(in _dateLabel));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._closeButton, Variant.From(in _closeButton));
		info.AddProperty(PropertyName._previousPageButton, Variant.From(in _previousPageButton));
		info.AddProperty(PropertyName._nextPageButton, Variant.From(in _nextPageButton));
		info.AddProperty(PropertyName._horizontalScrollBar, Variant.From(in _horizontalScrollBar));
		info.AddProperty(PropertyName._selectedCard, Variant.From(in _selectedCard));
		info.AddProperty(PropertyName._pageTween, Variant.From(in _pageTween));
		info.AddProperty(PropertyName._isBusy, Variant.From(in _isBusy));
		info.AddProperty(PropertyName._isPaging, Variant.From(in _isPaging));
		info.AddProperty(PropertyName._statusIsError, Variant.From(in _statusIsError));
		info.AddProperty(PropertyName._currentPage, Variant.From(in _currentPage));
		info.AddProperty(PropertyName._pageCount, Variant.From(in _pageCount));
		info.AddProperty(PropertyName._statusMessage, Variant.From(in _statusMessage));
		info.AddProperty(PropertyName._pendingDate, Variant.From(in _pendingDate));
		info.AddProperty(PropertyName._pendingLevelId, Variant.From(in _pendingLevelId));
		info.AddProperty(PropertyName._pendingMetadata, Variant.From(in _pendingMetadata));
		info.AddProperty(PropertyName.date, Variant.From(in date));
		info.AddProperty(PropertyName.levelList, Variant.From(in levelList));
		info.AddProperty(PropertyName.levelHttpRequest, Variant.From(in levelHttpRequest));
		info.AddProperty(PropertyName.playButton, Variant.From(in playButton));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._cardsContainer, out var value))
		{
			_cardsContainer = value.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._cardsScroll, out var value2))
		{
			_cardsScroll = value2.As<ScrollContainer>();
		}
		if (info.TryGetProperty(PropertyName._dateLabel, out var value3))
		{
			_dateLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value4))
		{
			_statusLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._closeButton, out var value5))
		{
			_closeButton = value5.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName._previousPageButton, out var value6))
		{
			_previousPageButton = value6.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName._nextPageButton, out var value7))
		{
			_nextPageButton = value7.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName._horizontalScrollBar, out var value8))
		{
			_horizontalScrollBar = value8.As<ScrollBar>();
		}
		if (info.TryGetProperty(PropertyName._selectedCard, out var value9))
		{
			_selectedCard = value9.As<DailyChallengeLevelPreview>();
		}
		if (info.TryGetProperty(PropertyName._pageTween, out var value10))
		{
			_pageTween = value10.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._isBusy, out var value11))
		{
			_isBusy = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isPaging, out var value12))
		{
			_isPaging = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._statusIsError, out var value13))
		{
			_statusIsError = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._currentPage, out var value14))
		{
			_currentPage = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pageCount, out var value15))
		{
			_pageCount = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName._statusMessage, out var value16))
		{
			_statusMessage = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingDate, out var value17))
		{
			_pendingDate = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingLevelId, out var value18))
		{
			_pendingLevelId = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingMetadata, out var value19))
		{
			_pendingMetadata = value19.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.date, out var value20))
		{
			date = value20.As<string>();
		}
		if (info.TryGetProperty(PropertyName.levelList, out var value21))
		{
			levelList = value21.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.levelHttpRequest, out var value22))
		{
			levelHttpRequest = value22.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.playButton, out var value23))
		{
			playButton = value23.As<MainButton>();
		}
	}
}
