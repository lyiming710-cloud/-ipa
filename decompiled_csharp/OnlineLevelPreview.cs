using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/OnlineLevel/OnlineLevelPreview.cs")]
public class OnlineLevelPreview : DialogBoxBase
{
	public delegate void SelectEventHandler(string url);

	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitDialog = "InitDialog";

		public static readonly StringName InitDialogData = "InitDialogData";

		public static readonly StringName ReadAuthorUid = "ReadAuthorUid";

		public static readonly StringName AuthorMetaClicked = "AuthorMetaClicked";

		public static readonly StringName CloseButtonPressed = "CloseButtonPressed";

		public static readonly StringName SetFinishIcon = "SetFinishIcon";

		public static readonly StringName AddIconTexture = "AddIconTexture";

		public static readonly StringName SetTags = "SetTags";

		public static readonly StringName PlayButtonPressed = "PlayButtonPressed";

		public static readonly StringName LevelInformationGetHTTPRequestCompleted = "LevelInformationGetHTTPRequestCompleted";

		public static readonly StringName CollectionButtonToggled = "CollectionButtonToggled";

		public static readonly StringName LikeButtonPressed = "LikeButtonPressed";

		public static readonly StringName DislikeButtonPressed = "DislikeButtonPressed";

		public static readonly StringName CreateCoin = "CreateCoin";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName layer = "layer";

		public static readonly StringName levelInformationGetHTTPRequest = "levelInformationGetHTTPRequest";

		public static readonly StringName levelLikeSendHTTPRequest = "levelLikeSendHTTPRequest";

		public static readonly StringName nameLabel = "nameLabel";

		public static readonly StringName mapTexture = "mapTexture";

		public static readonly StringName playButton = "playButton";

		public static readonly StringName closeButton = "closeButton";

		public static readonly StringName iconBox = "iconBox";

		public static readonly StringName authorLabel = "authorLabel";

		public static readonly StringName describeLabel = "describeLabel";

		public static readonly StringName levelIdLabel = "levelIdLabel";

		public static readonly StringName playedNumLabel = "playedNumLabel";

		public static readonly StringName completionRateTexture = "completionRateTexture";

		public static readonly StringName completionRateLabel = "completionRateLabel";

		public static readonly StringName dateLabel = "dateLabel";

		public static readonly StringName collectionButton = "collectionButton";

		public static readonly StringName likeButton = "likeButton";

		public static readonly StringName dislikeButton = "dislikeButton";

		public static readonly StringName likeLabel = "likeLabel";

		public static readonly StringName dislikeLabel = "dislikeLabel";

		public static readonly StringName tagsLabel = "tagsLabel";

		public static readonly StringName levelData = "levelData";

		public static readonly StringName id = "id";

		public static readonly StringName levelName = "levelName";

		public static readonly StringName map = "map";

		public static readonly StringName author = "author";

		public static readonly StringName authorUid = "authorUid";

		public static readonly StringName description = "description";

		public static readonly StringName fileUrl = "fileUrl";

		public static readonly StringName playedNum = "playedNum";

		public static readonly StringName date = "date";

		public static readonly StringName likeNum = "likeNum";

		public static readonly StringName dislikeNum = "dislikeNum";

		public static readonly StringName completions = "completions";

		public static readonly StringName failures = "failures";

		public static readonly StringName abandons = "abandons";

		public static readonly StringName loadOver = "loadOver";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	private CanvasLayer layer;

	private NativeHttpRequest levelInformationGetHTTPRequest;

	private NativeHttpRequest levelLikeSendHTTPRequest;

	private static Texture2D _izIcon;

	private static Texture2D _iz2Icon;

	private static Texture2D _vrIcon;

	private static Texture2D _endlessIcon;

	private static Texture2D _survivalIcon;

	private static Texture2D _luckyIcon;

	private Label nameLabel;

	private TextureRect mapTexture;

	private MainButton playButton;

	private TextureButton closeButton;

	private HBoxContainer iconBox;

	private RichTextLabel authorLabel;

	private RichTextLabel describeLabel;

	private RichTextLabel levelIdLabel;

	private RichTextLabel playedNumLabel;

	private TextureRect completionRateTexture;

	private RichTextLabel completionRateLabel;

	private RichTextLabel dateLabel;

	private TextureButton collectionButton;

	private TextureButton likeButton;

	private TextureButton dislikeButton;

	private RichTextLabel likeLabel;

	private RichTextLabel dislikeLabel;

	private RichTextLabel tagsLabel;

	public Dictionary levelData = new Dictionary();

	public string id = "";

	public string levelName = "";

	public string map = "";

	public string author = "";

	public string authorUid = "";

	public string description = "";

	public string fileUrl = "";

	public int playedNum;

	public long date;

	public int likeNum;

	public int dislikeNum;

	public int completions;

	public int failures;

	public int abandons;

	public bool loadOver;

	private static Texture2D IZ_ICON => _izIcon ?? (_izIcon = GD.Load<Texture2D>("uid://ciyj1718ypbih"));

	private static Texture2D IZ2_ICON => _iz2Icon ?? (_iz2Icon = GD.Load<Texture2D>("uid://b8rsot26eb7od"));

	private static Texture2D Vr_ICON => _vrIcon ?? (_vrIcon = GD.Load<Texture2D>("uid://24j6iw1ww08b"));

	private static Texture2D ENDLESS_ICON => _endlessIcon ?? (_endlessIcon = GD.Load<Texture2D>("uid://bcjx34t665il2"));

	private static Texture2D SURVIVAL_ICON => _survivalIcon ?? (_survivalIcon = GD.Load<Texture2D>("uid://crvtrh32bqu0r"));

	private static Texture2D LUCKY_ICON => _luckyIcon ?? (_luckyIcon = GD.Load<Texture2D>("uid://caqbmeu30cbsa"));

	public event SelectEventHandler OnSelect;

	public event Action<string, string> OnAuthorSelected;

	public override void _Ready()
	{
		base._Ready();
		layer = GetNode<CanvasLayer>("%Layer");
		levelInformationGetHTTPRequest = new NativeHttpRequest();
		levelLikeSendHTTPRequest = new NativeHttpRequest();
		AddChild(levelInformationGetHTTPRequest, forceReadableName: false, InternalMode.Disabled);
		AddChild(levelLikeSendHTTPRequest, forceReadableName: false, InternalMode.Disabled);
		nameLabel = GetNode<Label>("%NameLabel");
		mapTexture = GetNode<TextureRect>("%MapTexture");
		playButton = GetNode<MainButton>("%PlayButton");
		closeButton = GetNode<TextureButton>("%CloseButton");
		iconBox = GetNode<HBoxContainer>("%IconBox");
		authorLabel = GetNode<RichTextLabel>("%AuthorLabel");
		authorLabel.MetaClicked += AuthorMetaClicked;
		describeLabel = GetNode<RichTextLabel>("%DescribeLabel");
		levelIdLabel = GetNode<RichTextLabel>("%LevelIDLabel");
		playedNumLabel = GetNode<RichTextLabel>("%PlayedNumLabel");
		completionRateTexture = GetNode<TextureRect>("%CompletionRateTexture");
		completionRateLabel = GetNode<RichTextLabel>("%CompletionRateLabel");
		dateLabel = GetNode<RichTextLabel>("%DateLabel");
		collectionButton = GetNode<TextureButton>("%CollectionButton");
		likeButton = GetNode<TextureButton>("%LikeButton");
		dislikeButton = GetNode<TextureButton>("%DislikeButton");
		likeLabel = GetNode<RichTextLabel>("%LikeLabel");
		dislikeLabel = GetNode<RichTextLabel>("%DislikeLabel");
		tagsLabel = GetNode<RichTextLabel>("%TagsLabel");
		levelInformationGetHTTPRequest.RequestCompleted += LevelInformationGetHTTPRequestCompleted;
		closeButton.Pressed += CloseButtonPressed;
		playButton.Pressed += PlayButtonPressed;
		likeButton.Pressed += LikeButtonPressed;
		dislikeButton.Pressed += DislikeButtonPressed;
	}

	public void InitDialog(string _id)
	{
		id = _id;
		string url = $"https://api.pvzhe.com/workshop/levels/{_id}";
		levelInformationGetHTTPRequest.Request(url, Global.Instance.header);
	}

	public void InitDialogData(Dictionary _levelData)
	{
		levelData = _levelData;
		id = levelData.GetValueOrDefault("id", "").AsString();
		levelName = levelData.GetValueOrDefault("name", "").AsString();
		map = levelData.GetValueOrDefault("map", "").AsString();
		author = levelData.GetValueOrDefault("author", "").AsString();
		authorUid = ReadAuthorUid(levelData);
		description = levelData.GetValueOrDefault("description", "").AsString();
		fileUrl = levelData.GetValueOrDefault("fileUrl", "").AsString();
		playedNum = levelData.GetValueOrDefault("plays", 0).AsInt32();
		date = levelData.GetValueOrDefault("uploadTime", 0).AsInt64();
		likeNum = levelData.GetValueOrDefault("likes", 0).AsInt32();
		dislikeNum = levelData.GetValueOrDefault("dislikes", 0).AsInt32();
		completions = levelData.GetValueOrDefault("completions", 0).AsInt32();
		failures = levelData.GetValueOrDefault("failures", 0).AsInt32();
		abandons = levelData.GetValueOrDefault("abandons", 0).AsInt32();
		nameLabel.Text = levelName;
		TowerDefenseMapConfig mapConfig = TowerDefenseManager.Instance.GetMapConfig(map);
		TowerDefenseMapConfig.ApplyMapPreviewTexture(mapTexture, mapConfig?.GetMapThumbnail());
		SetFinishIcon(levelData);
		authorLabel.Clear();
		authorLabel.AddText("作者:");
		bool flag = authorUid != "" && OnAuthorSelected != null;
		authorLabel.MouseFilter = (MouseFilterEnum)(flag ? 0 : 2);
		authorLabel.TooltipText = (flag ? "查看该作者的关卡" : "");
		if (flag)
		{
			authorLabel.PushColor(new Color("ffd700"));
			authorLabel.PushMeta("author");
		}
		authorLabel.AddText(author);
		if (flag)
		{
			authorLabel.Pop();
			authorLabel.Pop();
		}
		describeLabel.Text = $"简介:{description}";
		levelIdLabel.Text = $"关卡ID:{id}";
		SetTags(levelData);
		int num = completions + failures;
		playedNumLabel.Text = $"游玩次数:{playedNum}";
		float num2 = 0f;
		if (num > 0)
		{
			num2 = (float)completions / (float)num * 100f;
		}
		completionRateLabel.Text = $"通关率:{num2:F2}%";
		if (levelData.ContainsKey("survivalRoundlimit") && levelData["survivalRoundlimit"].VariantType != Variant.Type.Nil && levelData.GetValueOrDefault("survivalRoundlimit", -1).AsInt32() == -1)
		{
			completionRateTexture.Visible = false;
		}
		Dictionary timeZoneFromSystem = Time.GetTimeZoneFromSystem();
		dateLabel.Text = string.Format("上传日期:{0}", Time.GetDatetimeStringFromUnixTime(date + (long)timeZoneFromSystem["bias"] * 60, useSpace: true));
		foreach (Variant item in GameSaveManager.Instance.GetKeyValue("OnlineMyCollection").AsGodotDictionary().GetValueOrDefault("Level", new Godot.Collections.Array())
			.AsGodotArray())
		{
			if (item.AsGodotDictionary().GetValueOrDefault("id", "-1").AsString() == id)
			{
				collectionButton.ButtonPressed = true;
				break;
			}
		}
		collectionButton.Disabled = false;
		collectionButton.Toggled += CollectionButtonToggled;
		loadOver = true;
		string key = $"OnlineLevel-{id}";
		int num3 = GameSaveManager.Instance.GetLevelValue(key).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary()
			.GetValueOrDefault("Like", -1)
			.AsInt32();
		if (num3 != -1)
		{
			likeButton.Disabled = false;
			dislikeButton.Disabled = false;
			if (num3 == 0)
			{
				dislikeButton.ButtonPressed = true;
			}
			else
			{
				likeButton.ButtonPressed = true;
			}
			likeLabel.Text = $"赞\n{likeNum}";
			dislikeLabel.Text = $"踩\n{dislikeNum}";
			likeButton.MouseFilter = MouseFilterEnum.Ignore;
			dislikeButton.MouseFilter = MouseFilterEnum.Ignore;
		}
		else
		{
			likeButton.Disabled = false;
			dislikeButton.Disabled = false;
		}
		if (id.StartsWith("-"))
		{
			collectionButton.Visible = false;
			likeButton.Visible = false;
			dislikeButton.Visible = false;
		}
	}

	internal static string ReadAuthorUid(Dictionary data)
	{
		string[] array = new string[2] { "authorUid", "author_uid" };
		foreach (string text in array)
		{
			if (!data.TryGetValue(text, out var value))
			{
				continue;
			}
			Variant.Type variantType = value.VariantType;
			Variant.Type num = variantType - 2;
			if ((ulong)num > 2uL)
			{
				goto IL_00a6;
			}
			switch ((int)num)
			{
			case 2:
				break;
			case 0:
				goto IL_006f;
			case 1:
				goto IL_0088;
			default:
				goto IL_00a6;
			}
			string s = value.AsString().Trim();
			goto IL_00ad;
			IL_006f:
			s = value.AsInt64().ToString(CultureInfo.InvariantCulture);
			goto IL_00ad;
			IL_00ad:
			if (long.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out var result) && result > 0)
			{
				return result.ToString(CultureInfo.InvariantCulture);
			}
			continue;
			IL_00a6:
			s = "";
			goto IL_00ad;
			IL_0088:
			s = value.AsDouble().ToString("R", CultureInfo.InvariantCulture);
			goto IL_00ad;
		}
		return "";
	}

	private void AuthorMetaClicked(Variant meta)
	{
		if (loadOver && !playButton.Disabled && !(authorUid == "") && OnAuthorSelected != null && !IsQueuedForDeletion())
		{
			AudioManager.Instance.AudioPlay("ButtonPress");
			OnAuthorSelected(authorUid, author);
			CloseDialog();
		}
	}

	public void CloseButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		CloseDialog();
	}

	private void SetFinishIcon(Dictionary data)
	{
		if (iconBox == null)
		{
			return;
		}
		foreach (Node child in iconBox.GetChildren())
		{
			child.QueueFree();
		}
		switch (data.GetValueOrDefault("finishMethod", "WAVE").AsString().ToUpper())
		{
		case "WAVE":
			if (data.ContainsKey("survivalRoundlimit") && data["survivalRoundlimit"].VariantType != Variant.Type.Nil)
			{
				int num = data.GetValueOrDefault("survivalRoundlimit", -1).AsInt32();
				if (num == -1)
				{
					AddIconTexture(ENDLESS_ICON);
				}
				else if (num >= 0)
				{
					AddIconTexture(SURVIVAL_ICON);
				}
			}
			break;
		case "VASE":
			AddIconTexture(Vr_ICON);
			break;
		case "IZM":
			AddIconTexture(IZ_ICON);
			break;
		case "IZM2":
			AddIconTexture(IZ2_ICON);
			break;
		}
		if (data.GetValueOrDefault("lucky", false).AsBool())
		{
			AddIconTexture(LUCKY_ICON);
		}
	}

	public void AddIconTexture(Texture2D iconTexture)
	{
		if (iconBox != null && iconBox.GetChildCount() < 2)
		{
			TextureRect textureRect = new TextureRect();
			textureRect.Texture = iconTexture;
			textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
			textureRect.ExpandMode = TextureRect.ExpandModeEnum.KeepSize;
			textureRect.MouseFilter = MouseFilterEnum.Ignore;
			iconBox.AddChild(textureRect, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void SetTags(Dictionary data)
	{
		if (tagsLabel == null)
		{
			return;
		}
		bool flag = data.GetValueOrDefault("recommended", false).AsBool();
		Godot.Collections.Array array = ((data.ContainsKey("tags") && data["tags"].VariantType == Variant.Type.Array) ? data["tags"].AsGodotArray() : new Godot.Collections.Array());
		List<string> list = new List<string>();
		for (int i = 0; i < array.Count; i++)
		{
			string text = array[i].AsString();
			if (text != "")
			{
				list.Add(text);
			}
		}
		if (list.Count == 0 && !flag)
		{
			tagsLabel.Visible = false;
			return;
		}
		tagsLabel.Visible = true;
		StringBuilder stringBuilder = new StringBuilder("标签:");
		if (flag)
		{
			stringBuilder.Append("[color=#00ff00]官方推荐[/color]");
			if (list.Count > 0)
			{
				stringBuilder.Append("\u3000");
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (j > 0)
			{
				stringBuilder.Append("\u3000");
			}
			stringBuilder.Append("[color=#ffd700]").Append(list[j]).Append("[/color]");
		}
		tagsLabel.Text = stringBuilder.ToString();
	}

	public void PlayButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		if (loadOver)
		{
			Global.Instance.enterLevelId = id;
			OnSelect?.Invoke(fileUrl);
			playButton.Disabled = true;
			closeButton.Disabled = true;
		}
		else
		{
			BroadCastManager.Instance.BroadCastFloatCreate("关卡正在加载中...", Colors.Red);
		}
	}

	public void LevelInformationGetHTTPRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != 0L)
		{
			BroadCastManager.Instance.BroadCastFloatCreate(Global.Instance.GetHTTPRequestErrorMessage(result), Colors.Red);
			CloseDialog();
			return;
		}
		Json json = new Json();
		json.Parse(body.GetStringFromUtf8());
		Dictionary dictionary = json.Data.AsGodotDictionary();
		if (dictionary.GetValueOrDefault("id", "-1").AsString() != "-1")
		{
			InitDialogData(dictionary);
			return;
		}
		BroadCastManager.Instance.BroadCastFloatCreate("关卡不存在或者被删除", Colors.Red);
		CloseDialog();
	}

	public void CollectionButtonToggled(bool toggledOn)
	{
		Dictionary dictionary = GameSaveManager.Instance.GetKeyValue("OnlineMyCollection").AsGodotDictionary();
		Godot.Collections.Array array = dictionary.GetValueOrDefault("Level", new Godot.Collections.Array()).AsGodotArray();
		if (toggledOn)
		{
			array.Add(new Dictionary
			{
				{ "id", id },
				{ "name", levelName },
				{ "map", map }
			});
		}
		else
		{
			for (int i = 0; i < array.Count; i++)
			{
				if (array[i].AsGodotDictionary().GetValueOrDefault("id", "-1").AsString() == id)
				{
					array.RemoveAt(i);
					break;
				}
			}
		}
		dictionary["Level"] = array;
		GameSaveManager.Instance.SetKeyValue("OnlineMyCollection", dictionary);
		GameSaveManager.Instance.Save();
	}

	public void LikeButtonPressed()
	{
		string key = $"OnlineLevel-{id}";
		Dictionary levelValue = GameSaveManager.Instance.GetLevelValue(key);
		Dictionary dictionary = levelValue.GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
		if (dictionary.GetValueOrDefault("Play", 0).AsInt32() > 0 || dictionary.GetValueOrDefault("Finish", 0).AsInt32() > 0)
		{
			dictionary.GetValueOrDefault("Like", -1);
			dictionary["Like"] = 1;
			levelValue["Key"] = dictionary;
			GameSaveManager.Instance.SetLevelValue(key, levelValue);
			GameSaveManager.Instance.Save();
			levelLikeSendHTTPRequest.Request($"https://api.pvzhe.com/workshop/levels/{id}/like", Global.Instance.header, HttpMethod.Post);
			likeButton.MouseFilter = MouseFilterEnum.Ignore;
			dislikeButton.MouseFilter = MouseFilterEnum.Ignore;
			likeNum++;
			likeLabel.Text = $"赞\n{likeNum}";
			dislikeLabel.Text = $"踩\n{dislikeNum}";
			likeButton.ButtonPressed = true;
			CreateCoin(likeButton.GlobalPosition, 100);
		}
		else
		{
			DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]至少游玩一次才能评价[/font_size][/center]");
			likeButton.ButtonPressed = false;
		}
	}

	public void DislikeButtonPressed()
	{
		string key = $"OnlineLevel-{id}";
		Dictionary levelValue = GameSaveManager.Instance.GetLevelValue(key);
		Dictionary dictionary = levelValue.GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
		if (dictionary.GetValueOrDefault("Play", 0).AsInt32() > 0 || dictionary.GetValueOrDefault("Finish", 0).AsInt32() > 0)
		{
			dictionary.GetValueOrDefault("Like", -1);
			dictionary["Like"] = 0;
			levelValue["Key"] = dictionary;
			GameSaveManager.Instance.SetLevelValue(key, levelValue);
			GameSaveManager.Instance.Save();
			levelLikeSendHTTPRequest.Request($"https://api.pvzhe.com/workshop/levels/{id}/dislike", Global.Instance.header, HttpMethod.Post);
			likeButton.MouseFilter = MouseFilterEnum.Ignore;
			dislikeButton.MouseFilter = MouseFilterEnum.Ignore;
			dislikeNum++;
			likeLabel.Text = $"赞\n{likeNum}";
			dislikeLabel.Text = $"踩\n{dislikeNum}";
			dislikeButton.ButtonPressed = true;
			CreateCoin(dislikeButton.GlobalPosition, 100);
		}
		else
		{
			DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]至少游玩一次才能评价[/font_size][/center]");
			dislikeButton.ButtonPressed = false;
		}
	}

	public async void CreateCoin(Vector2 pos, int num)
	{
		while (num >= 1000)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_DIAMOND, pos, 30.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			towerDefenseGroundItemBase.gridPos = new Vector2I(towerDefenseGroundItemBase.gridPos.X, 200);
			towerDefenseGroundItemBase.Reparent(collectionButton, keepGlobalTransform: false);
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
			TowerDefenseGroundItemBase towerDefenseGroundItemBase2 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_GOLD, pos, 30.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			towerDefenseGroundItemBase2.gridPos = new Vector2I(towerDefenseGroundItemBase2.gridPos.X, 200);
			towerDefenseGroundItemBase2.Reparent(collectionButton, keepGlobalTransform: false);
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
			TowerDefenseGroundItemBase towerDefenseGroundItemBase3 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_SILVER, pos, 30.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			towerDefenseGroundItemBase3.gridPos = new Vector2I(towerDefenseGroundItemBase3.gridPos.X, 200);
			towerDefenseGroundItemBase3.Reparent(collectionButton, keepGlobalTransform: false);
			if (towerDefenseGroundItemBase3 is TowerDefenseCoinBase towerDefenseCoinBase3)
			{
				GetTree().CreateTimer(1.0, processAlways: false).Timeout += towerDefenseCoinBase3.moveComponent.MoveClear;
				GetTree().CreateTimer(1.5, processAlways: false).Timeout += towerDefenseCoinBase3.Collection;
			}
			num -= 10;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitDialogData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_levelData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadAuthorUid, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AuthorMetaClicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "meta", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetFinishIcon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddIconTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "iconTexture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetTags, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LevelInformationGetHTTPRequestCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "result", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "responseCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "headers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedByteArray, "body", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CollectionButtonToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LikeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DislikeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCoin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.InitDialog && args.Count == 1)
		{
			InitDialog(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitDialogData && args.Count == 1)
		{
			InitDialogData(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadAuthorUid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadAuthorUid(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.AuthorMetaClicked && args.Count == 1)
		{
			AuthorMetaClicked(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloseButtonPressed && args.Count == 0)
		{
			CloseButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SetFinishIcon && args.Count == 1)
		{
			SetFinishIcon(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddIconTexture && args.Count == 1)
		{
			AddIconTexture(VariantUtils.ConvertTo<Texture2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTags && args.Count == 1)
		{
			SetTags(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayButtonPressed && args.Count == 0)
		{
			PlayButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LevelInformationGetHTTPRequestCompleted && args.Count == 4)
		{
			LevelInformationGetHTTPRequestCompleted(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2]), VariantUtils.ConvertTo<byte[]>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CollectionButtonToggled && args.Count == 1)
		{
			CollectionButtonToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LikeButtonPressed && args.Count == 0)
		{
			LikeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.DislikeButtonPressed && args.Count == 0)
		{
			DislikeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCoin && args.Count == 2)
		{
			CreateCoin(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReadAuthorUid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadAuthorUid(VariantUtils.ConvertTo<Dictionary>(in args[0])));
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
		if (method == MethodName.InitDialog)
		{
			return true;
		}
		if (method == MethodName.InitDialogData)
		{
			return true;
		}
		if (method == MethodName.ReadAuthorUid)
		{
			return true;
		}
		if (method == MethodName.AuthorMetaClicked)
		{
			return true;
		}
		if (method == MethodName.CloseButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SetFinishIcon)
		{
			return true;
		}
		if (method == MethodName.AddIconTexture)
		{
			return true;
		}
		if (method == MethodName.SetTags)
		{
			return true;
		}
		if (method == MethodName.PlayButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LevelInformationGetHTTPRequestCompleted)
		{
			return true;
		}
		if (method == MethodName.CollectionButtonToggled)
		{
			return true;
		}
		if (method == MethodName.LikeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.DislikeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.CreateCoin)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.layer)
		{
			layer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.levelInformationGetHTTPRequest)
		{
			levelInformationGetHTTPRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.levelLikeSendHTTPRequest)
		{
			levelLikeSendHTTPRequest = VariantUtils.ConvertTo<NativeHttpRequest>(in value);
			return true;
		}
		if (name == PropertyName.nameLabel)
		{
			nameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.mapTexture)
		{
			mapTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.playButton)
		{
			playButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.closeButton)
		{
			closeButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.iconBox)
		{
			iconBox = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.authorLabel)
		{
			authorLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.describeLabel)
		{
			describeLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.levelIdLabel)
		{
			levelIdLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.playedNumLabel)
		{
			playedNumLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.completionRateTexture)
		{
			completionRateTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.completionRateLabel)
		{
			completionRateLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.dateLabel)
		{
			dateLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.collectionButton)
		{
			collectionButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.likeButton)
		{
			likeButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.dislikeButton)
		{
			dislikeButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.likeLabel)
		{
			likeLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.dislikeLabel)
		{
			dislikeLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.tagsLabel)
		{
			tagsLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.levelData)
		{
			levelData = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.id)
		{
			id = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.levelName)
		{
			levelName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.map)
		{
			map = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.author)
		{
			author = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.authorUid)
		{
			authorUid = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.description)
		{
			description = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.fileUrl)
		{
			fileUrl = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.playedNum)
		{
			playedNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.date)
		{
			date = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.likeNum)
		{
			likeNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.dislikeNum)
		{
			dislikeNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.completions)
		{
			completions = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.failures)
		{
			failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.abandons)
		{
			abandons = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.loadOver)
		{
			loadOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.layer)
		{
			value = VariantUtils.CreateFrom(in layer);
			return true;
		}
		if (name == PropertyName.levelInformationGetHTTPRequest)
		{
			value = VariantUtils.CreateFrom(in levelInformationGetHTTPRequest);
			return true;
		}
		if (name == PropertyName.levelLikeSendHTTPRequest)
		{
			value = VariantUtils.CreateFrom(in levelLikeSendHTTPRequest);
			return true;
		}
		if (name == PropertyName.nameLabel)
		{
			value = VariantUtils.CreateFrom(in nameLabel);
			return true;
		}
		if (name == PropertyName.mapTexture)
		{
			value = VariantUtils.CreateFrom(in mapTexture);
			return true;
		}
		if (name == PropertyName.playButton)
		{
			value = VariantUtils.CreateFrom(in playButton);
			return true;
		}
		if (name == PropertyName.closeButton)
		{
			value = VariantUtils.CreateFrom(in closeButton);
			return true;
		}
		if (name == PropertyName.iconBox)
		{
			value = VariantUtils.CreateFrom(in iconBox);
			return true;
		}
		if (name == PropertyName.authorLabel)
		{
			value = VariantUtils.CreateFrom(in authorLabel);
			return true;
		}
		if (name == PropertyName.describeLabel)
		{
			value = VariantUtils.CreateFrom(in describeLabel);
			return true;
		}
		if (name == PropertyName.levelIdLabel)
		{
			value = VariantUtils.CreateFrom(in levelIdLabel);
			return true;
		}
		if (name == PropertyName.playedNumLabel)
		{
			value = VariantUtils.CreateFrom(in playedNumLabel);
			return true;
		}
		if (name == PropertyName.completionRateTexture)
		{
			value = VariantUtils.CreateFrom(in completionRateTexture);
			return true;
		}
		if (name == PropertyName.completionRateLabel)
		{
			value = VariantUtils.CreateFrom(in completionRateLabel);
			return true;
		}
		if (name == PropertyName.dateLabel)
		{
			value = VariantUtils.CreateFrom(in dateLabel);
			return true;
		}
		if (name == PropertyName.collectionButton)
		{
			value = VariantUtils.CreateFrom(in collectionButton);
			return true;
		}
		if (name == PropertyName.likeButton)
		{
			value = VariantUtils.CreateFrom(in likeButton);
			return true;
		}
		if (name == PropertyName.dislikeButton)
		{
			value = VariantUtils.CreateFrom(in dislikeButton);
			return true;
		}
		if (name == PropertyName.likeLabel)
		{
			value = VariantUtils.CreateFrom(in likeLabel);
			return true;
		}
		if (name == PropertyName.dislikeLabel)
		{
			value = VariantUtils.CreateFrom(in dislikeLabel);
			return true;
		}
		if (name == PropertyName.tagsLabel)
		{
			value = VariantUtils.CreateFrom(in tagsLabel);
			return true;
		}
		if (name == PropertyName.levelData)
		{
			value = VariantUtils.CreateFrom(in levelData);
			return true;
		}
		if (name == PropertyName.id)
		{
			value = VariantUtils.CreateFrom(in id);
			return true;
		}
		if (name == PropertyName.levelName)
		{
			value = VariantUtils.CreateFrom(in levelName);
			return true;
		}
		if (name == PropertyName.map)
		{
			value = VariantUtils.CreateFrom(in map);
			return true;
		}
		if (name == PropertyName.author)
		{
			value = VariantUtils.CreateFrom(in author);
			return true;
		}
		if (name == PropertyName.authorUid)
		{
			value = VariantUtils.CreateFrom(in authorUid);
			return true;
		}
		if (name == PropertyName.description)
		{
			value = VariantUtils.CreateFrom(in description);
			return true;
		}
		if (name == PropertyName.fileUrl)
		{
			value = VariantUtils.CreateFrom(in fileUrl);
			return true;
		}
		if (name == PropertyName.playedNum)
		{
			value = VariantUtils.CreateFrom(in playedNum);
			return true;
		}
		if (name == PropertyName.date)
		{
			value = VariantUtils.CreateFrom(in date);
			return true;
		}
		if (name == PropertyName.likeNum)
		{
			value = VariantUtils.CreateFrom(in likeNum);
			return true;
		}
		if (name == PropertyName.dislikeNum)
		{
			value = VariantUtils.CreateFrom(in dislikeNum);
			return true;
		}
		if (name == PropertyName.completions)
		{
			value = VariantUtils.CreateFrom(in completions);
			return true;
		}
		if (name == PropertyName.failures)
		{
			value = VariantUtils.CreateFrom(in failures);
			return true;
		}
		if (name == PropertyName.abandons)
		{
			value = VariantUtils.CreateFrom(in abandons);
			return true;
		}
		if (name == PropertyName.loadOver)
		{
			value = VariantUtils.CreateFrom(in loadOver);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.layer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelInformationGetHTTPRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelLikeSendHTTPRequest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.nameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.playButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.closeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.iconBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.authorLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.describeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelIdLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.playedNumLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.completionRateTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.completionRateLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.dateLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.collectionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.likeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.dislikeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.likeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.dislikeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.tagsLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.levelData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.id, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.levelName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.map, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.author, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.authorUid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.description, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.fileUrl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.playedNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.date, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.likeNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.dislikeNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.completions, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.abandons, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.loadOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.layer, Variant.From(in layer));
		info.AddProperty(PropertyName.levelInformationGetHTTPRequest, Variant.From(in levelInformationGetHTTPRequest));
		info.AddProperty(PropertyName.levelLikeSendHTTPRequest, Variant.From(in levelLikeSendHTTPRequest));
		info.AddProperty(PropertyName.nameLabel, Variant.From(in nameLabel));
		info.AddProperty(PropertyName.mapTexture, Variant.From(in mapTexture));
		info.AddProperty(PropertyName.playButton, Variant.From(in playButton));
		info.AddProperty(PropertyName.closeButton, Variant.From(in closeButton));
		info.AddProperty(PropertyName.iconBox, Variant.From(in iconBox));
		info.AddProperty(PropertyName.authorLabel, Variant.From(in authorLabel));
		info.AddProperty(PropertyName.describeLabel, Variant.From(in describeLabel));
		info.AddProperty(PropertyName.levelIdLabel, Variant.From(in levelIdLabel));
		info.AddProperty(PropertyName.playedNumLabel, Variant.From(in playedNumLabel));
		info.AddProperty(PropertyName.completionRateTexture, Variant.From(in completionRateTexture));
		info.AddProperty(PropertyName.completionRateLabel, Variant.From(in completionRateLabel));
		info.AddProperty(PropertyName.dateLabel, Variant.From(in dateLabel));
		info.AddProperty(PropertyName.collectionButton, Variant.From(in collectionButton));
		info.AddProperty(PropertyName.likeButton, Variant.From(in likeButton));
		info.AddProperty(PropertyName.dislikeButton, Variant.From(in dislikeButton));
		info.AddProperty(PropertyName.likeLabel, Variant.From(in likeLabel));
		info.AddProperty(PropertyName.dislikeLabel, Variant.From(in dislikeLabel));
		info.AddProperty(PropertyName.tagsLabel, Variant.From(in tagsLabel));
		info.AddProperty(PropertyName.levelData, Variant.From(in levelData));
		info.AddProperty(PropertyName.id, Variant.From(in id));
		info.AddProperty(PropertyName.levelName, Variant.From(in levelName));
		info.AddProperty(PropertyName.map, Variant.From(in map));
		info.AddProperty(PropertyName.author, Variant.From(in author));
		info.AddProperty(PropertyName.authorUid, Variant.From(in authorUid));
		info.AddProperty(PropertyName.description, Variant.From(in description));
		info.AddProperty(PropertyName.fileUrl, Variant.From(in fileUrl));
		info.AddProperty(PropertyName.playedNum, Variant.From(in playedNum));
		info.AddProperty(PropertyName.date, Variant.From(in date));
		info.AddProperty(PropertyName.likeNum, Variant.From(in likeNum));
		info.AddProperty(PropertyName.dislikeNum, Variant.From(in dislikeNum));
		info.AddProperty(PropertyName.completions, Variant.From(in completions));
		info.AddProperty(PropertyName.failures, Variant.From(in failures));
		info.AddProperty(PropertyName.abandons, Variant.From(in abandons));
		info.AddProperty(PropertyName.loadOver, Variant.From(in loadOver));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.layer, out var value))
		{
			layer = value.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.levelInformationGetHTTPRequest, out var value2))
		{
			levelInformationGetHTTPRequest = value2.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.levelLikeSendHTTPRequest, out var value3))
		{
			levelLikeSendHTTPRequest = value3.As<NativeHttpRequest>();
		}
		if (info.TryGetProperty(PropertyName.nameLabel, out var value4))
		{
			nameLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.mapTexture, out var value5))
		{
			mapTexture = value5.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.playButton, out var value6))
		{
			playButton = value6.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.closeButton, out var value7))
		{
			closeButton = value7.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.iconBox, out var value8))
		{
			iconBox = value8.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.authorLabel, out var value9))
		{
			authorLabel = value9.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.describeLabel, out var value10))
		{
			describeLabel = value10.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.levelIdLabel, out var value11))
		{
			levelIdLabel = value11.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.playedNumLabel, out var value12))
		{
			playedNumLabel = value12.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.completionRateTexture, out var value13))
		{
			completionRateTexture = value13.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.completionRateLabel, out var value14))
		{
			completionRateLabel = value14.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.dateLabel, out var value15))
		{
			dateLabel = value15.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.collectionButton, out var value16))
		{
			collectionButton = value16.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.likeButton, out var value17))
		{
			likeButton = value17.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.dislikeButton, out var value18))
		{
			dislikeButton = value18.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.likeLabel, out var value19))
		{
			likeLabel = value19.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.dislikeLabel, out var value20))
		{
			dislikeLabel = value20.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.tagsLabel, out var value21))
		{
			tagsLabel = value21.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.levelData, out var value22))
		{
			levelData = value22.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.id, out var value23))
		{
			id = value23.As<string>();
		}
		if (info.TryGetProperty(PropertyName.levelName, out var value24))
		{
			levelName = value24.As<string>();
		}
		if (info.TryGetProperty(PropertyName.map, out var value25))
		{
			map = value25.As<string>();
		}
		if (info.TryGetProperty(PropertyName.author, out var value26))
		{
			author = value26.As<string>();
		}
		if (info.TryGetProperty(PropertyName.authorUid, out var value27))
		{
			authorUid = value27.As<string>();
		}
		if (info.TryGetProperty(PropertyName.description, out var value28))
		{
			description = value28.As<string>();
		}
		if (info.TryGetProperty(PropertyName.fileUrl, out var value29))
		{
			fileUrl = value29.As<string>();
		}
		if (info.TryGetProperty(PropertyName.playedNum, out var value30))
		{
			playedNum = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName.date, out var value31))
		{
			date = value31.As<long>();
		}
		if (info.TryGetProperty(PropertyName.likeNum, out var value32))
		{
			likeNum = value32.As<int>();
		}
		if (info.TryGetProperty(PropertyName.dislikeNum, out var value33))
		{
			dislikeNum = value33.As<int>();
		}
		if (info.TryGetProperty(PropertyName.completions, out var value34))
		{
			completions = value34.As<int>();
		}
		if (info.TryGetProperty(PropertyName.failures, out var value35))
		{
			failures = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName.abandons, out var value36))
		{
			abandons = value36.As<int>();
		}
		if (info.TryGetProperty(PropertyName.loadOver, out var value37))
		{
			loadOver = value37.As<bool>();
		}
	}
}
