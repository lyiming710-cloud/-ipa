using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Scene/MainMenu/MainMenu.cs")]
public class MainMenu : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName OnGlobalFeatureChanged = "OnGlobalFeatureChanged";

		public static readonly StringName RefreshShopVisibility = "RefreshShopVisibility";

		public static readonly StringName AdventureButtonPressed = "AdventureButtonPressed";

		public static readonly StringName ChallengeButtonPressed = "ChallengeButtonPressed";

		public static readonly StringName SurvivalButtonPressed = "SurvivalButtonPressed";

		public static readonly StringName PuzzleGameButtonPressed = "PuzzleGameButtonPressed";

		public static readonly StringName MiniGameButtonPresed = "MiniGameButtonPresed";

		public static readonly StringName IZM2GameButtonPresed = "IZM2GameButtonPresed";

		public static readonly StringName BattleGameButtonPresed = "BattleGameButtonPresed";

		public static readonly StringName StarsExchangeButtonPresed = "StarsExchangeButtonPresed";

		public static readonly StringName HybridParkGameButtonPressed = "HybridParkGameButtonPressed";

		public static readonly StringName MoreButtonPressed = "MoreButtonPressed";

		public static readonly StringName AlmanacButtonPressed = "AlmanacButtonPressed";

		public static readonly StringName ShopButtonPressed = "ShopButtonPressed";

		public static readonly StringName DiyButtonPressed = "DiyButtonPressed";

		public static readonly StringName MoreBackButtonPressed = "MoreBackButtonPressed";

		public static readonly StringName HelpPressed = "HelpPressed";

		public static readonly StringName OptionButtonPressed = "OptionButtonPressed";

		public static readonly StringName ExitButtonPressed = "ExitButtonPressed";

		public static readonly StringName NewUser = "NewUser";

		public static readonly StringName WoodFileButtonPressed = "WoodFileButtonPressed";

		public static readonly StringName DailyChallengeButtonPressed = "DailyChallengeButtonPressed";

		public static readonly StringName AnimeFinish = "AnimeFinish";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName camera = "camera";

		public static readonly StringName moreGameMarker = "moreGameMarker";

		public static readonly StringName defaultMarker = "defaultMarker";

		public static readonly StringName helpButton = "helpButton";

		public static readonly StringName optionButton = "optionButton";

		public static readonly StringName quitButton = "quitButton";

		public static readonly StringName woodButton = "woodButton";

		public static readonly StringName adventureButton = "adventureButton";

		public static readonly StringName challengeButton = "challengeButton";

		public static readonly StringName survivalButton = "survivalButton";

		public static readonly StringName woodNameLabel = "woodNameLabel";

		public static readonly StringName woodFileButton = "woodFileButton";

		public static readonly StringName selectAnimationPlayer = "selectAnimationPlayer";

		public static readonly StringName currentVersionLabel = "currentVersionLabel";

		public static readonly StringName newVersionLinkButton = "newVersionLinkButton";

		public static readonly StringName shopButton = "shopButton";

		public static readonly StringName choose = "choose";

		public static readonly StringName wait = "wait";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public Camera2D camera;

	public Marker2D moreGameMarker;

	public Marker2D defaultMarker;

	public TextureButton helpButton;

	public TextureButton optionButton;

	public TextureButton quitButton;

	public TextureButton woodButton;

	public TextureButton adventureButton;

	public TextureButton challengeButton;

	public TextureButton survivalButton;

	public RichTextLabel woodNameLabel;

	public TextureButton woodFileButton;

	public AnimationPlayer selectAnimationPlayer;

	public Label currentVersionLabel;

	public LinkButton newVersionLinkButton;

	public TextureButton shopButton;

	public bool choose;

	public bool wait;

	public override void _Ready()
	{
		ResourceManager.Instance.RequireFullGameplayResourcesReady("MainMenu");
		camera = GetNode<Camera2D>("%Camera");
		moreGameMarker = GetNode<Marker2D>("%MoreGameMarker");
		defaultMarker = GetNode<Marker2D>("%DefaultMarker");
		helpButton = GetNode<TextureButton>("%HelpTextureButton");
		optionButton = GetNode<TextureButton>("%OptionTextureButton");
		quitButton = GetNode<TextureButton>("%QuitTextureButton");
		woodButton = GetNode<TextureButton>("%WoodFileButton");
		adventureButton = GetNode<TextureButton>("%AdventureButton");
		challengeButton = GetNode<TextureButton>("%ChallengeButton");
		survivalButton = GetNode<TextureButton>("%SurvivalButton");
		woodNameLabel = GetNode<RichTextLabel>("%WoodNameLabel");
		woodFileButton = GetNode<TextureButton>("%WoodFileButton");
		selectAnimationPlayer = GetNode<AnimationPlayer>("%SelectAnimationPlayer");
		currentVersionLabel = GetNode<Label>("%CurrentVersionLabel");
		newVersionLinkButton = GetNode<LinkButton>("%NewVersionLinkButton");
		shopButton = GetNode<TextureButton>("%ShopButton");
		if (GlobalFeatureManager.Instance != null)
		{
			GlobalFeatureManager.Instance.FeatureChanged += OnGlobalFeatureChanged;
			GlobalFeatureManager.Instance.FeaturesReloaded += RefreshShopVisibility;
		}
		RefreshShopVisibility();
		if ((!Global.Instance.newVersionSkip && Global.Instance.hasNewVersion) || GameSaveManager.Instance.GetUserCurrent() == "")
		{
			wait = true;
		}
		currentVersionLabel.Text = $"当前版本:{Global.Instance.version}";
		AudioManager.Instance.AudioPlay("MainMenu", AudioManagerEnum.TYPE.MUSIC);
		if (Global.Instance.mainMenuShowMoreModes)
		{
			Global.Instance.mainMenuShowMoreModes = false;
			camera.GlobalPosition = new Vector2(moreGameMarker.GlobalPosition.X, camera.GlobalPosition.Y);
		}
		if (Global.Instance.isMultiplayerMode)
		{
			DialogManager.Instance.DialogCreate("MultiplayerLobby");
			if (MultiPlayerManager.Instance.currentMatchId == "")
			{
				Global.Instance.isMultiplayerMode = false;
				Global.Instance.isMultiplayerHost = false;
			}
		}
		if (Global.Instance.enterLevelMode == "LevelChoose" && Global.Instance.currentLevelChoose == "TryLevel")
		{
			DialogManager.Instance.DialogCreate("TryLevel");
		}
		adventureButton.Pressed += AdventureButtonPressed;
		XWModLevelSession.Clear();
		challengeButton.Pressed += ChallengeButtonPressed;
		survivalButton.Pressed += SurvivalButtonPressed;
		helpButton.Pressed += HelpPressed;
		optionButton.Pressed += OptionButtonPressed;
		quitButton.Pressed += ExitButtonPressed;
		shopButton.Pressed += ShopButtonPressed;
		woodFileButton.Pressed += WoodFileButtonPressed;
		selectAnimationPlayer.AnimationFinished += AnimeFinish;
		GetNode<BaseButton>("Background/MenuTexture/DiyButton").Pressed += DiyButtonPressed;
		GetNode<BaseButton>("Background/MenuTexture/MoreTexture/MoreButton").Pressed += MoreButtonPressed;
		GetNode<BaseButton>("Background/MenuTexture/AlmanacButton").Pressed += AlmanacButtonPressed;
		GetNode<BaseButton>("Background/MoreBackground/MoreBackButton").Pressed += MoreBackButtonPressed;
		GetNode<BaseButton>("Background/MoreBackground/MiniGameButton").Pressed += MiniGameButtonPresed;
		GetNode<BaseButton>("Background/MoreBackground/PuzzleGameButton").Pressed += PuzzleGameButtonPressed;
		GetNode<BaseButton>("Background/MoreBackground/BattleGameButton").Pressed += BattleGameButtonPresed;
		GetNode<BaseButton>("Background/MoreBackground/IZM2GameButton").Pressed += IZM2GameButtonPresed;
		GetNode<BaseButton>("Background/MoreBackground/StarsExchangeButton").Pressed += StarsExchangeButtonPresed;
		GetNode<BaseButton>("Background/MoreBackground/HybridParkGameButton").Pressed += HybridParkGameButtonPressed;
		GetNode<BaseButton>("Wood/DailyChallengeButton").Pressed += DailyChallengeButtonPressed;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GameSaveManager.Instance.GetUserCurrent() != "")
		{
			woodNameLabel.Text = GameSaveManager.Instance.GetUserCurrent() + "!";
		}
	}

	public override void _ExitTree()
	{
		if (GlobalFeatureManager.Instance != null)
		{
			GlobalFeatureManager.Instance.FeatureChanged -= OnGlobalFeatureChanged;
			GlobalFeatureManager.Instance.FeaturesReloaded -= RefreshShopVisibility;
		}
		base._ExitTree();
	}

	private void OnGlobalFeatureChanged(string featureId, int _oldValue, int _newValue)
	{
		if (featureId == "Shop")
		{
			RefreshShopVisibility();
		}
	}

	private void RefreshShopVisibility()
	{
		if (GodotObject.IsInstanceValid(shopButton))
		{
			shopButton.Visible = GlobalFeatureManager.Instance?.IsUnlocked("Shop") ?? false;
		}
	}

	public void AdventureButtonPressed()
	{
		if (!wait)
		{
			Global.Instance.currentLevelChoose = "Adventure";
			Global.Instance.currentChapterId = -1;
			Global.Instance.currentLevelId = -1;
			SceneManager.Instance.ChangeScene("LevelChoose");
			adventureButton.GlobalPosition += Vector2.One * 2f;
		}
	}

	public void ChallengeButtonPressed()
	{
		if (!wait)
		{
			Global.Instance.currentLevelChoose = "Challenge";
			Global.Instance.currentChapterId = -1;
			Global.Instance.currentLevelId = -1;
			SceneManager.Instance.ChangeScene("LevelChoose");
			challengeButton.GlobalPosition += Vector2.One * 2f;
		}
	}

	public void SurvivalButtonPressed()
	{
		if (!wait)
		{
			Global.Instance.currentLevelChoose = "Survival";
			Global.Instance.currentChapterId = -1;
			Global.Instance.currentLevelId = -1;
			SceneManager.Instance.ChangeScene("LevelChoose");
			survivalButton.GlobalPosition += Vector2.One * 2f;
		}
	}

	public void PuzzleGameButtonPressed()
	{
		if (!wait)
		{
			Global.Instance.currentLevelChoose = "Puzzle";
			Global.Instance.currentChapterId = -1;
			Global.Instance.currentLevelId = -1;
			SceneManager.Instance.ChangeScene("LevelChoose");
		}
	}

	public void MiniGameButtonPresed()
	{
		if (!wait)
		{
			Global.Instance.currentLevelChoose = "MiniGames";
			Global.Instance.currentChapterId = -1;
			Global.Instance.currentLevelId = -1;
			SceneManager.Instance.ChangeScene("LevelChoose");
		}
	}

	public void IZM2GameButtonPresed()
	{
		if (!wait)
		{
			Global.Instance.currentLevelChoose = "IZM2";
			Global.Instance.currentChapterId = -1;
			Global.Instance.currentLevelId = -1;
			SceneManager.Instance.ChangeScene("LevelChoose");
		}
	}

	public void BattleGameButtonPresed()
	{
		if (!wait)
		{
			DialogManager.Instance.DialogCreate("MultiplayerLobby");
		}
	}

	public void StarsExchangeButtonPresed()
	{
		if (!wait)
		{
			DialogManager.Instance.DialogCreate("StarExchange");
		}
	}

	public void HybridParkGameButtonPressed()
	{
		if (!wait)
		{
			Global.Instance.currentLevelChoose = "HybridPark";
			Global.Instance.currentChapterId = -1;
			Global.Instance.currentLevelId = -1;
			SceneManager.Instance.ChangeScene("LevelChoose");
		}
	}

	public void MoreButtonPressed()
	{
		if (!wait)
		{
			AudioManager.Instance.AudioPlay("GraveButtonPress");
			Tween tween = camera.CreateTween();
			tween.SetEase(Tween.EaseType.InOut);
			tween.SetTrans(Tween.TransitionType.Quad);
			tween.TweenProperty(camera, "global_position:x", moreGameMarker.GlobalPosition.X, 1.0);
		}
	}

	public void AlmanacButtonPressed()
	{
		if (!wait)
		{
			ResourceManager.Instance.RequireFullGameplayResourcesReady("AlmanacButtonPressed");
			DialogManager.Instance.DialogCreate("Almanac");
		}
	}

	public void ShopButtonPressed()
	{
		if (!wait)
		{
			GlobalFeatureManager instance = GlobalFeatureManager.Instance;
			if (instance != null && instance.IsUnlocked("Shop"))
			{
				DialogManager.Instance.DialogCreate("Shop");
			}
		}
	}

	public void DiyButtonPressed()
	{
		if (!wait)
		{
			if (Global.Instance.hasNewVersion)
			{
				BroadCastManager.Instance.BroadCastFloatCreate("使用此功能需更新至最新版本", Colors.Red);
			}
			else
			{
				SceneManager.Instance.ChangeScene("LevelEditorStage");
			}
		}
	}

	public void MoreBackButtonPressed()
	{
		if (!wait)
		{
			AudioManager.Instance.AudioPlay("GraveButtonPress");
			Tween tween = camera.CreateTween();
			tween.SetEase(Tween.EaseType.InOut);
			tween.SetTrans(Tween.TransitionType.Quad);
			tween.TweenProperty(camera, "global_position:x", defaultMarker.GlobalPosition.X, 1.0);
		}
	}

	public void HelpPressed()
	{
		if (!wait)
		{
			AudioManager.Instance.AudioPlay("ButtonPress");
			DialogManager.Instance.DialogCreate("Help");
		}
	}

	public void OptionButtonPressed()
	{
		if (!wait)
		{
			AudioManager.Instance.AudioPlay("ButtonPress");
			DialogManager.Instance.DialogCreate("MainMenuOption");
		}
	}

	public void ExitButtonPressed()
	{
		if (!wait)
		{
			AudioManager.Instance.AudioPlay("ButtonPress");
			DialogManager.Instance.DialogCreate("ExitGame");
		}
	}

	public void NewUser()
	{
		DialogManager.Instance.DialogCreate("NewUser");
	}

	public async void WoodFileButtonPressed()
	{
		if (!wait)
		{
			AudioManager.Instance.AudioPlay("ButtonPress");
			await DialogManager.Instance.DialogCreate("User").WaitForClose();
			RequestReady();
		}
	}

	public async void DailyChallengeButtonPressed()
	{
		if (Global.Instance.hasNewVersion)
		{
			BroadCastManager.Instance.BroadCastFloatCreate("使用此功能需更新至最新版本", Colors.Red);
			return;
		}
		Dictionary datetimeDictFromSystem = Time.GetDatetimeDictFromSystem();
		int curYear = (int)(long)datetimeDictFromSystem["year"];
		int curMonth = (int)(long)datetimeDictFromSystem["month"];
		if (InternetServerManager.Instance.DailyLevelMonthNeedRefresh(curYear, curMonth))
		{
			bool forceRefresh = InternetServerManager.Instance.dailyLevelLoadedMonths.ContainsKey($"{curYear}-{curMonth:D2}");
			InternetServerManager.Instance.GetDailyLevel(curYear, curMonth, forceRefresh);
			ulong startMs = Time.GetTicksMsec();
			while (InternetServerManager.Instance.IsDailyLevelRequestPending(curYear, curMonth) && Time.GetTicksMsec() - startMs <= 15000)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			}
			if (InternetServerManager.Instance.DailyLevelMonthNeedRefresh(curYear, curMonth))
			{
				BroadCastManager.Instance.BroadCastFloatCreate("正在获取每日挑战列表,请稍后重试", Colors.Red);
				return;
			}
		}
		AudioManager.Instance.AudioPlay("ButtonPress");
		await DialogManager.Instance.DialogCreate("DailyChallenge").WaitForClose();
	}

	public async void AnimeFinish(StringName animName)
	{
		if (Global.Instance.hasNewVersion)
		{
			newVersionLinkButton.Visible = true;
			newVersionLinkButton.Text = $"最新版本:{Global.Instance.newVersion}(点此跳转更新)";
			newVersionLinkButton.Uri = Global.Instance.uri;
			if (!Global.Instance.newVersionSkip)
			{
				DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("NewVersion");
				dialogBoxBase.Set("uri", Global.Instance.uri);
				dialogBoxBase.Set("message", InternetServerManager.Instance.versionMessage);
				await dialogBoxBase.WaitForClose();
				Global.Instance.newVersionSkip = true;
			}
		}
		wait = false;
		if (animName == (StringName)"Enter" && GameSaveManager.Instance.GetUserCurrent() == "")
		{
			NewUser();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(26)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGlobalFeatureChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_oldValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshShopVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdventureButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChallengeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SurvivalButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PuzzleGameButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MiniGameButtonPresed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IZM2GameButtonPresed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BattleGameButtonPresed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StarsExchangeButtonPresed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HybridParkGameButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoreButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AlmanacButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShopButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DiyButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoreBackButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HelpPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OptionButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExitButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NewUser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WoodFileButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DailyChallengeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeFinish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "animName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGlobalFeatureChanged && args.Count == 3)
		{
			OnGlobalFeatureChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshShopVisibility && args.Count == 0)
		{
			RefreshShopVisibility();
			ret = default;
			return true;
		}
		if (method == MethodName.AdventureButtonPressed && args.Count == 0)
		{
			AdventureButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ChallengeButtonPressed && args.Count == 0)
		{
			ChallengeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SurvivalButtonPressed && args.Count == 0)
		{
			SurvivalButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.PuzzleGameButtonPressed && args.Count == 0)
		{
			PuzzleGameButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.MiniGameButtonPresed && args.Count == 0)
		{
			MiniGameButtonPresed();
			ret = default;
			return true;
		}
		if (method == MethodName.IZM2GameButtonPresed && args.Count == 0)
		{
			IZM2GameButtonPresed();
			ret = default;
			return true;
		}
		if (method == MethodName.BattleGameButtonPresed && args.Count == 0)
		{
			BattleGameButtonPresed();
			ret = default;
			return true;
		}
		if (method == MethodName.StarsExchangeButtonPresed && args.Count == 0)
		{
			StarsExchangeButtonPresed();
			ret = default;
			return true;
		}
		if (method == MethodName.HybridParkGameButtonPressed && args.Count == 0)
		{
			HybridParkGameButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.MoreButtonPressed && args.Count == 0)
		{
			MoreButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.AlmanacButtonPressed && args.Count == 0)
		{
			AlmanacButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ShopButtonPressed && args.Count == 0)
		{
			ShopButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.DiyButtonPressed && args.Count == 0)
		{
			DiyButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.MoreBackButtonPressed && args.Count == 0)
		{
			MoreBackButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.HelpPressed && args.Count == 0)
		{
			HelpPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OptionButtonPressed && args.Count == 0)
		{
			OptionButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ExitButtonPressed && args.Count == 0)
		{
			ExitButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.NewUser && args.Count == 0)
		{
			NewUser();
			ret = default;
			return true;
		}
		if (method == MethodName.WoodFileButtonPressed && args.Count == 0)
		{
			WoodFileButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.DailyChallengeButtonPressed && args.Count == 0)
		{
			DailyChallengeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeFinish && args.Count == 1)
		{
			AnimeFinish(VariantUtils.ConvertTo<StringName>(in args[0]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.OnGlobalFeatureChanged)
		{
			return true;
		}
		if (method == MethodName.RefreshShopVisibility)
		{
			return true;
		}
		if (method == MethodName.AdventureButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ChallengeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SurvivalButtonPressed)
		{
			return true;
		}
		if (method == MethodName.PuzzleGameButtonPressed)
		{
			return true;
		}
		if (method == MethodName.MiniGameButtonPresed)
		{
			return true;
		}
		if (method == MethodName.IZM2GameButtonPresed)
		{
			return true;
		}
		if (method == MethodName.BattleGameButtonPresed)
		{
			return true;
		}
		if (method == MethodName.StarsExchangeButtonPresed)
		{
			return true;
		}
		if (method == MethodName.HybridParkGameButtonPressed)
		{
			return true;
		}
		if (method == MethodName.MoreButtonPressed)
		{
			return true;
		}
		if (method == MethodName.AlmanacButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ShopButtonPressed)
		{
			return true;
		}
		if (method == MethodName.DiyButtonPressed)
		{
			return true;
		}
		if (method == MethodName.MoreBackButtonPressed)
		{
			return true;
		}
		if (method == MethodName.HelpPressed)
		{
			return true;
		}
		if (method == MethodName.OptionButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ExitButtonPressed)
		{
			return true;
		}
		if (method == MethodName.NewUser)
		{
			return true;
		}
		if (method == MethodName.WoodFileButtonPressed)
		{
			return true;
		}
		if (method == MethodName.DailyChallengeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.AnimeFinish)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.camera)
		{
			camera = VariantUtils.ConvertTo<Camera2D>(in value);
			return true;
		}
		if (name == PropertyName.moreGameMarker)
		{
			moreGameMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.defaultMarker)
		{
			defaultMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.helpButton)
		{
			helpButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.optionButton)
		{
			optionButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.quitButton)
		{
			quitButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.woodButton)
		{
			woodButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.adventureButton)
		{
			adventureButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.challengeButton)
		{
			challengeButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.survivalButton)
		{
			survivalButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.woodNameLabel)
		{
			woodNameLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.woodFileButton)
		{
			woodFileButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.selectAnimationPlayer)
		{
			selectAnimationPlayer = VariantUtils.ConvertTo<AnimationPlayer>(in value);
			return true;
		}
		if (name == PropertyName.currentVersionLabel)
		{
			currentVersionLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.newVersionLinkButton)
		{
			newVersionLinkButton = VariantUtils.ConvertTo<LinkButton>(in value);
			return true;
		}
		if (name == PropertyName.shopButton)
		{
			shopButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.choose)
		{
			choose = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.wait)
		{
			wait = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.camera)
		{
			value = VariantUtils.CreateFrom(in camera);
			return true;
		}
		if (name == PropertyName.moreGameMarker)
		{
			value = VariantUtils.CreateFrom(in moreGameMarker);
			return true;
		}
		if (name == PropertyName.defaultMarker)
		{
			value = VariantUtils.CreateFrom(in defaultMarker);
			return true;
		}
		if (name == PropertyName.helpButton)
		{
			value = VariantUtils.CreateFrom(in helpButton);
			return true;
		}
		if (name == PropertyName.optionButton)
		{
			value = VariantUtils.CreateFrom(in optionButton);
			return true;
		}
		if (name == PropertyName.quitButton)
		{
			value = VariantUtils.CreateFrom(in quitButton);
			return true;
		}
		if (name == PropertyName.woodButton)
		{
			value = VariantUtils.CreateFrom(in woodButton);
			return true;
		}
		if (name == PropertyName.adventureButton)
		{
			value = VariantUtils.CreateFrom(in adventureButton);
			return true;
		}
		if (name == PropertyName.challengeButton)
		{
			value = VariantUtils.CreateFrom(in challengeButton);
			return true;
		}
		if (name == PropertyName.survivalButton)
		{
			value = VariantUtils.CreateFrom(in survivalButton);
			return true;
		}
		if (name == PropertyName.woodNameLabel)
		{
			value = VariantUtils.CreateFrom(in woodNameLabel);
			return true;
		}
		if (name == PropertyName.woodFileButton)
		{
			value = VariantUtils.CreateFrom(in woodFileButton);
			return true;
		}
		if (name == PropertyName.selectAnimationPlayer)
		{
			value = VariantUtils.CreateFrom(in selectAnimationPlayer);
			return true;
		}
		if (name == PropertyName.currentVersionLabel)
		{
			value = VariantUtils.CreateFrom(in currentVersionLabel);
			return true;
		}
		if (name == PropertyName.newVersionLinkButton)
		{
			value = VariantUtils.CreateFrom(in newVersionLinkButton);
			return true;
		}
		if (name == PropertyName.shopButton)
		{
			value = VariantUtils.CreateFrom(in shopButton);
			return true;
		}
		if (name == PropertyName.choose)
		{
			value = VariantUtils.CreateFrom(in choose);
			return true;
		}
		if (name == PropertyName.wait)
		{
			value = VariantUtils.CreateFrom(in wait);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.camera, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.moreGameMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.defaultMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.helpButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.optionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.quitButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.woodButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.adventureButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.challengeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.survivalButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.woodNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.woodFileButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.selectAnimationPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.currentVersionLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.newVersionLinkButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.shopButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.choose, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.wait, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.camera, Variant.From(in camera));
		info.AddProperty(PropertyName.moreGameMarker, Variant.From(in moreGameMarker));
		info.AddProperty(PropertyName.defaultMarker, Variant.From(in defaultMarker));
		info.AddProperty(PropertyName.helpButton, Variant.From(in helpButton));
		info.AddProperty(PropertyName.optionButton, Variant.From(in optionButton));
		info.AddProperty(PropertyName.quitButton, Variant.From(in quitButton));
		info.AddProperty(PropertyName.woodButton, Variant.From(in woodButton));
		info.AddProperty(PropertyName.adventureButton, Variant.From(in adventureButton));
		info.AddProperty(PropertyName.challengeButton, Variant.From(in challengeButton));
		info.AddProperty(PropertyName.survivalButton, Variant.From(in survivalButton));
		info.AddProperty(PropertyName.woodNameLabel, Variant.From(in woodNameLabel));
		info.AddProperty(PropertyName.woodFileButton, Variant.From(in woodFileButton));
		info.AddProperty(PropertyName.selectAnimationPlayer, Variant.From(in selectAnimationPlayer));
		info.AddProperty(PropertyName.currentVersionLabel, Variant.From(in currentVersionLabel));
		info.AddProperty(PropertyName.newVersionLinkButton, Variant.From(in newVersionLinkButton));
		info.AddProperty(PropertyName.shopButton, Variant.From(in shopButton));
		info.AddProperty(PropertyName.choose, Variant.From(in choose));
		info.AddProperty(PropertyName.wait, Variant.From(in wait));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.camera, out var value))
		{
			camera = value.As<Camera2D>();
		}
		if (info.TryGetProperty(PropertyName.moreGameMarker, out var value2))
		{
			moreGameMarker = value2.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.defaultMarker, out var value3))
		{
			defaultMarker = value3.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.helpButton, out var value4))
		{
			helpButton = value4.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.optionButton, out var value5))
		{
			optionButton = value5.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.quitButton, out var value6))
		{
			quitButton = value6.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.woodButton, out var value7))
		{
			woodButton = value7.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.adventureButton, out var value8))
		{
			adventureButton = value8.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.challengeButton, out var value9))
		{
			challengeButton = value9.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.survivalButton, out var value10))
		{
			survivalButton = value10.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.woodNameLabel, out var value11))
		{
			woodNameLabel = value11.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.woodFileButton, out var value12))
		{
			woodFileButton = value12.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.selectAnimationPlayer, out var value13))
		{
			selectAnimationPlayer = value13.As<AnimationPlayer>();
		}
		if (info.TryGetProperty(PropertyName.currentVersionLabel, out var value14))
		{
			currentVersionLabel = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.newVersionLinkButton, out var value15))
		{
			newVersionLinkButton = value15.As<LinkButton>();
		}
		if (info.TryGetProperty(PropertyName.shopButton, out var value16))
		{
			shopButton = value16.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.choose, out var value17))
		{
			choose = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.wait, out var value18))
		{
			wait = value18.As<bool>();
		}
	}
}
