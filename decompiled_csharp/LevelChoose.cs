using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Scene/LevelChoose/LevelChoose.cs")]
public class LevelChoose : Control
{
	public enum LevelChooseMode
	{
		Chapter,
		Level,
		Option
	}

	public new class MethodName : Control.MethodName
	{
		public static readonly StringName ReadProgress = "ReadProgress";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitChapter = "InitChapter";

		public static readonly StringName InitLevel = "InitLevel";

		public static readonly StringName Select = "Select";

		public static readonly StringName Back = "Back";

		public static readonly StringName OptionButtonPressed = "OptionButtonPressed";

		public static readonly StringName DifficultChange = "DifficultChange";

		public static readonly StringName LevelMenuFlattenCheckBoxToggled = "LevelMenuFlattenCheckBoxToggled";

		public static readonly StringName ShowModCatalogs = "ShowModCatalogs";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName IsModBrowser = "IsModBrowser";

		public static readonly StringName camera = "camera";

		public static readonly StringName levelChooseMarker = "levelChooseMarker";

		public static readonly StringName defaultMarker = "defaultMarker";

		public static readonly StringName gui = "gui";

		public static readonly StringName guiFollow = "guiFollow";

		public static readonly StringName optionGUI = "optionGUI";

		public static readonly StringName informationLabel = "informationLabel";

		public static readonly StringName chapterMenu = "chapterMenu";

		public static readonly StringName levelMenu = "levelMenu";

		public static readonly StringName normalButton = "normalButton";

		public static readonly StringName difficultButton = "difficultButton";

		public static readonly StringName normalLabel = "normalLabel";

		public static readonly StringName difficultLabel = "difficultLabel";

		public static readonly StringName levelMenuFlattenCheckBox = "levelMenuFlattenCheckBox";

		public static readonly StringName chapterTexture = "chapterTexture";

		public static readonly StringName chapterSelectBackground = "chapterSelectBackground";

		public static readonly StringName levelMenuFlatten = "levelMenuFlatten";

		public static readonly StringName levelContainer = "levelContainer";

		public static readonly StringName currentChapterList = "currentChapterList";

		public static readonly StringName currentChapter = "currentChapter";

		public static readonly StringName currentChapterIndex = "currentChapterIndex";

		public static readonly StringName currentMode = "currentMode";

		public static readonly StringName saveBackMode = "saveBackMode";

		public static readonly StringName tween = "tween";

		public static readonly StringName _showingModCatalogs = "_showingModCatalogs";

		public static readonly StringName _modDifficulty = "_modDifficulty";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static PackedScene _LevelSelectItem;

	private static PackedScene _DragMenuSelectItemLevel;

	private static PackedScene _DragMenuSelectItemChapter;

	public Camera2D camera;

	public Marker2D levelChooseMarker;

	public Marker2D defaultMarker;

	public CanvasLayer gui;

	public CanvasLayer guiFollow;

	public CanvasLayer optionGUI;

	public Label informationLabel;

	public Node chapterMenu;

	public DragMenu levelMenu;

	public TextureButton normalButton;

	public TextureButton difficultButton;

	public Label normalLabel;

	public Label difficultLabel;

	public CheckBox levelMenuFlattenCheckBox;

	public TextureRect chapterTexture;

	public TextureRect chapterSelectBackground;

	public Control levelMenuFlatten;

	public HFlowContainer levelContainer;

	public Array currentChapterList;

	public Dictionary currentChapter;

	public int currentChapterIndex = -1;

	public LevelChooseMode currentMode;

	public LevelChooseMode saveBackMode;

	public Tween tween;

	private bool _showingModCatalogs;

	private string _modDifficulty = "Normal";

	private static PackedScene levelSelectItem => _LevelSelectItem ?? (_LevelSelectItem = GD.Load<PackedScene>("uid://cp8mgtcrcrxwf"));

	private static PackedScene dragMenuSelectItemLevel => _DragMenuSelectItemLevel ?? (_DragMenuSelectItemLevel = GD.Load<PackedScene>("uid://bftr1dw41t3pu"));

	private static PackedScene dragMenuSelectItemChapter => _DragMenuSelectItemChapter ?? (_DragMenuSelectItemChapter = GD.Load<PackedScene>("uid://rsb7ubyly6ta"));

	private bool IsModBrowser => Global.Instance.currentLevelChoose == "__ModLevels";

	private Dictionary ReadProgress(string key)
	{
		if (!IsModBrowser)
		{
			return GameSaveManager.Instance.GetLevelValue(key);
		}
		return XWModPlayerProgressService.GetLevel(XWModLevelSession.ForLevel(key, _modDifficulty));
	}

	private XWModLevelIdentity IdentityFor(string key)
	{
		if (!IsModBrowser)
		{
			return null;
		}
		return XWModLevelSession.ForLevel(key, _modDifficulty);
	}

	public override void _Ready()
	{
		camera = GetNode<Camera2D>("%Camera");
		levelChooseMarker = GetNode<Marker2D>("%LevelChooseMarker");
		defaultMarker = GetNode<Marker2D>("%DefaultMarker");
		gui = GetNode<CanvasLayer>("%GUI");
		guiFollow = GetNode<CanvasLayer>("%GUIFollow");
		optionGUI = GetNode<CanvasLayer>("%OptionGUI");
		informationLabel = GetNode<Label>("%InformationLabel");
		chapterMenu = GetNode("%ChapterMenu");
		levelMenu = GetNode<DragMenu>("%LevelMenu");
		normalButton = GetNode<TextureButton>("%NormalButton");
		difficultButton = GetNode<TextureButton>("%DifficultButton");
		normalLabel = GetNode<Label>("%NormalLabel");
		difficultLabel = GetNode<Label>("%DifficultLabel");
		levelMenuFlattenCheckBox = GetNode<CheckBox>("%LevelMenuFlattenCheckBox");
		chapterTexture = GetNode<TextureRect>("%ChapterTexture");
		chapterSelectBackground = GetNode<TextureRect>("%ChapterSelectBackground");
		levelMenuFlatten = GetNode<Control>("%LevelMenuFlatten");
		levelContainer = GetNode<HFlowContainer>("%LevelContainer");
		AudioManager.Instance.AudioPlay("ZenGarden", AudioManagerEnum.TYPE.MUSIC);
		if (IsModBrowser)
		{
			_modDifficulty = XWModLevelSession.Current?.Difficulty ?? "Normal";
			currentChapterList = new Array();
			ShowModCatalogs();
		}
		else
		{
			XWModLevelSession.Clear();
			currentChapterList = (Array)ResourceManager.Instance.LEVELS[Global.Instance.currentLevelChoose]["Chapter"];
		}
		InitChapter();
		if (!IsModBrowser && Global.Instance.currentAwardMode)
		{
			Global.Instance.currentAwardMode = false;
			int num = (int)GameSaveManager.Instance.GetKeyValue("AdventureChapterIndex");
			if (num != -1)
			{
				Select(num);
			}
		}
		else if (!IsModBrowser && Global.Instance.currentChapterId != -1 && Global.Instance.enterLevelMode == "LevelChoose")
		{
			Select(Global.Instance.currentChapterId);
		}
		normalLabel.Modulate = Colors.White;
		difficultLabel.Modulate = Colors.White;
		string text = (IsModBrowser ? _modDifficulty : GameSaveManager.Instance.GetKeyValue("CurrentDifficult").AsString());
		if (!(text == "Normal"))
		{
			if (text == "Difficult")
			{
				difficultButton.ButtonPressed = true;
				difficultLabel.Modulate = Colors.OrangeRed;
			}
			else
			{
				GameSaveManager.Instance.SetKeyValue("CurrentDifficult", "Normal");
				normalButton.ButtonPressed = true;
				normalLabel.Modulate = Colors.Green;
			}
		}
		else
		{
			normalButton.ButtonPressed = true;
			normalLabel.Modulate = Colors.Green;
		}
		levelMenuFlattenCheckBox.ButtonPressed = GameSaveManager.Instance.GetKeyValue("LevelMenuFlatten").AsBool();
		if (levelMenuFlattenCheckBox.ButtonPressed)
		{
			levelMenu.Visible = false;
			levelMenuFlatten.Visible = true;
			gui.FollowViewportEnabled = true;
		}
		else
		{
			levelMenu.Visible = true;
			levelMenuFlatten.Visible = false;
			gui.FollowViewportEnabled = false;
		}
		GetNode<SpriteBrightButton>("GUI/OptionButton").OnPressed += OptionButtonPressed;
		GetNode<BaseButton>("%NormalButton").Pressed += DifficultChange;
		GetNode<BaseButton>("%DifficultButton").Pressed += DifficultChange;
		GetNode<CheckBox>("%LevelMenuFlattenCheckBox").Toggled += LevelMenuFlattenCheckBoxToggled;
		GetNode<BaseButton>("%BackButton").Pressed += Back;
	}

	public void InitChapter()
	{
		for (int i = 0; i < currentChapterList.Count; i++)
		{
			Dictionary dictionary = (Dictionary)currentChapterList[i];
			DragMenuSelectItemChapter dragMenuSelectItemChapter = LevelChoose.dragMenuSelectItemChapter.Instantiate<DragMenuSelectItemChapter>(PackedScene.GenEditState.Disabled);
			dragMenuSelectItemChapter.index = i;
			dragMenuSelectItemChapter.OnSelect += Select;
			chapterMenu.AddChild(dragMenuSelectItemChapter, forceReadableName: false, InternalMode.Disabled);
			dragMenuSelectItemChapter.Init(dictionary, IdentityFor(""));
			if (dictionary["OpenKey"].AsString() == "")
			{
				if (dictionary["UnlockImage"].AsString().Length > 0)
				{
					dragMenuSelectItemChapter.sprite.Texture = GD.Load<Texture2D>(dictionary["UnlockImage"].AsString());
				}
				continue;
			}
			if (ReadProgress(dictionary["OpenKey"].AsString()).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary().GetValueOrDefault("Finish", 0)
				.AsInt32() > 0 || CommandManager.Instance.debugOpenAllLevel)
			{
				if (dictionary["UnlockImage"].AsString().Length > 0)
				{
					dragMenuSelectItemChapter.sprite.Texture = GD.Load<Texture2D>(dictionary["UnlockImage"].AsString());
				}
				continue;
			}
			if (dictionary["LockImage"].AsString().Length > 0)
			{
				dragMenuSelectItemChapter.sprite.Texture = GD.Load<Texture2D>(dictionary["LockImage"].AsString());
			}
			dragMenuSelectItemChapter.@lock = true;
		}
	}

	public void InitLevel(int chapterId)
	{
		currentChapter = (Dictionary)currentChapterList[chapterId];
		if (currentChapter.GetValueOrDefault("Background", "").AsString() != "")
		{
			chapterTexture.Texture = GD.Load<Texture2D>(currentChapter["Background"].AsString());
		}
		if (currentChapter.GetValueOrDefault("Building", "").AsString() != "")
		{
			chapterSelectBackground.Texture = GD.Load<Texture2D>(currentChapter["Building"].AsString());
		}
		int num = currentChapter.GetValueOrDefault("PreOpen", 0).AsInt32() - 1;
		Array array = (Array)currentChapter["Level"];
		for (int i = 0; i < array.Count; i++)
		{
			Dictionary dictionary = (Dictionary)array[i];
			if (dictionary["OpenKey"].AsString() == "Lock")
			{
				continue;
			}
			Dictionary dictionary2 = ReadProgress(dictionary["OpenKey"].AsString()).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
			if (!CommandManager.Instance.debugOpenAllLevel && !(dictionary["OpenKey"].AsString() == "") && dictionary2.GetValueOrDefault("Finish", 0).AsInt32() <= 0)
			{
				if (num <= 0)
				{
					continue;
				}
				num--;
			}
			DragMenuSelectItemlevel dragMenuSelectItemlevel = dragMenuSelectItemLevel.Instantiate<DragMenuSelectItemlevel>(PackedScene.GenEditState.Disabled);
			dragMenuSelectItemlevel.index = i;
			dragMenuSelectItemlevel.OnSelect += Select;
			levelMenu.AddChild(dragMenuSelectItemlevel, forceReadableName: false, InternalMode.Disabled);
			if (((Dictionary)((Array)currentChapter["Level"])[i])["SaveKey"].AsString() != "")
			{
				dragMenuSelectItemlevel.Init(dictionary["SaveKey"].AsString(), IdentityFor(dictionary["SaveKey"].AsString()));
			}
			if (dictionary["UnlockImage"].AsString().Length > 0)
			{
				dragMenuSelectItemlevel.sprite.Texture = GD.Load<Texture2D>(dictionary["UnlockImage"].AsString());
			}
			DragMenuSelectItemlevel dragMenuSelectItemlevel2 = dragMenuSelectItemLevel.Instantiate<DragMenuSelectItemlevel>(PackedScene.GenEditState.Disabled);
			dragMenuSelectItemlevel2.index = i;
			dragMenuSelectItemlevel2.OnSelect += Select;
			levelContainer.AddChild(dragMenuSelectItemlevel2, forceReadableName: false, InternalMode.Disabled);
			dragMenuSelectItemlevel2.graphics.Scale = Vector2.One * 0.5f;
			if (((Dictionary)((Array)currentChapter["Level"])[i])["SaveKey"].AsString() != "")
			{
				dragMenuSelectItemlevel2.Init(dictionary["SaveKey"].AsString(), IdentityFor(dictionary["SaveKey"].AsString()));
			}
			if (dictionary["UnlockImage"].AsString().Length > 0)
			{
				dragMenuSelectItemlevel2.sprite.Texture = GD.Load<Texture2D>(dictionary["UnlockImage"].AsString());
			}
		}
		int num2 = (IsModBrowser ? (-1) : GameSaveManager.Instance.GetKeyValue($"AdventureChapter{chapterId + 1}Index").AsInt32());
		if (num2 != -1)
		{
			levelMenu.CallDeferred("SetPos", num2);
		}
	}

	public async void Select(int id)
	{
		if (GodotObject.IsInstanceValid(tween) && tween.IsRunning())
		{
			return;
		}
		switch (currentMode)
		{
		case LevelChooseMode.Chapter:
			if (((Dictionary)currentChapterList[id]).GetValueOrDefault("Lock", false).AsBool())
			{
				DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]敬请期待[/font_size][/center]");
				break;
			}
			currentChapterIndex = id;
			Global.Instance.currentChapterId = id;
			if (!IsModBrowser)
			{
				GameSaveManager.Instance.SetKeyValue("AdventureChapterIndex", id);
			}
			chapterMenu.Set("alive", false);
			tween = camera.CreateTween();
			tween.SetEase(Tween.EaseType.InOut);
			tween.SetTrans(Tween.TransitionType.Quad);
			tween.TweenProperty(camera, "global_position:y", levelChooseMarker.GlobalPosition.Y, 0.5);
			currentMode = LevelChooseMode.Level;
			InitLevel(id);
			informationLabel.Text = currentChapter["Name"].AsString();
			break;
		case LevelChooseMode.Level:
		{
			ResourceManager.Instance.RequireFullGameplayResourcesReady("Select");
			if (IsModBrowser)
			{
				await LaunchModLevel(id);
				break;
			}
			string text = GameSaveManager.Instance.GetKeyValue("CurrentDifficult").AsString();
			chapterMenu.Set("alive", true);
			GameSaveManager.Instance.SetKeyValue($"AdventureChapter{currentChapterIndex + 1}Index", id);
			GameSaveManager.Instance.Save();
			Dictionary dictionary = (Dictionary)((Array)currentChapter["Level"])[id];
			Dictionary dictionary2 = (Dictionary)dictionary["Level"];
			if (dictionary2[text].AsString() != "")
			{
				TowerDefenseManager.Instance.currentLevelConfig = GD.Load<TowerDefenseLevelConfig>(dictionary2[text].AsString());
			}
			else
			{
				TowerDefenseManager.Instance.currentLevelConfig = GD.Load<TowerDefenseLevelConfig>(dictionary2["Normal"].AsString());
			}
			Global.Instance.currentLevelId = id;
			Global.Instance.enterLevelMode = "LevelChoose";
			if (Global.Instance.isMultiplayerMode && MultiPlayerManager.Instance.isHost)
			{
				MultiPlayerManager.Instance.SendSelectLevel(dictionary["SaveKey"].AsString());
				await MultiPlayerManager.Instance.StartCompatibleGameAsync();
			}
			else
			{
				SceneManager.Instance.ChangeScene("TowerDefense");
			}
			break;
		}
		}
	}

	public async void Back()
	{
		if (IsModBrowser && currentMode == LevelChooseMode.Chapter)
		{
			if (_showingModCatalogs)
			{
				XWModLevelSession.Clear();
				SceneManager.Instance.ChangeScene("LevelEditorStage");
			}
			else
			{
				ShowModCatalogs();
			}
			return;
		}
		switch (currentMode)
		{
		case LevelChooseMode.Chapter:
			GameSaveManager.Instance.Save();
			if (Global.Instance.currentLevelChoose == "MiniGames" || Global.Instance.currentLevelChoose == "Puzzle" || Global.Instance.currentLevelChoose == "IZM2" || Global.Instance.currentLevelChoose == "HybridPark")
			{
				Global.Instance.mainMenuShowMoreModes = true;
			}
			SceneManager.Instance.ChangeScene("MainMenu");
			break;
		case LevelChooseMode.Level:
			informationLabel.Text = "章节选择";
			tween = camera.CreateTween();
			tween.SetEase(Tween.EaseType.InOut);
			tween.SetTrans(Tween.TransitionType.Quad);
			tween.TweenProperty(camera, "global_position:y", defaultMarker.GlobalPosition.Y, 0.5);
			currentMode = LevelChooseMode.Chapter;
			chapterMenu.Set("alive", true);
			await ToSignal(tween, Tween.SignalName.Finished);
			foreach (Node child in levelMenu.GetChildren())
			{
				child.QueueFree();
			}
			{
				foreach (Node child2 in levelContainer.GetChildren())
				{
					child2.QueueFree();
				}
				break;
			}
		case LevelChooseMode.Option:
			gui.Visible = true;
			guiFollow.Visible = true;
			optionGUI.Visible = false;
			currentMode = saveBackMode;
			break;
		}
	}

	public void OptionButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		saveBackMode = currentMode;
		currentMode = LevelChooseMode.Option;
		gui.Visible = false;
		guiFollow.Visible = false;
		optionGUI.Visible = true;
	}

	public void DifficultChange()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		string text = "Normal";
		normalLabel.Modulate = Colors.White;
		difficultLabel.Modulate = Colors.White;
		bool flag = false;
		if (normalButton.ButtonPressed)
		{
			text = "Normal";
			normalLabel.Modulate = Colors.Green;
			flag = true;
		}
		if (difficultButton.ButtonPressed)
		{
			DialogManager.Instance.DialogCreate("DifficultWarning");
			text = "Difficult";
			difficultLabel.Modulate = Colors.OrangeRed;
			flag = true;
		}
		if (!flag)
		{
			text = "Normal";
			normalLabel.Modulate = Colors.Green;
			flag = true;
		}
		if (IsModBrowser)
		{
			_modDifficulty = text;
			return;
		}
		GameSaveManager.Instance.SetKeyValue("CurrentDifficult", text);
		GameSaveManager.Instance.Save();
	}

	public void LevelMenuFlattenCheckBoxToggled(bool toggle)
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		if (toggle)
		{
			levelMenu.Visible = false;
			levelMenuFlatten.Visible = true;
			gui.FollowViewportEnabled = true;
		}
		else
		{
			levelMenu.Visible = true;
			levelMenuFlatten.Visible = false;
			gui.FollowViewportEnabled = false;
		}
		GameSaveManager.Instance.SetKeyValue("LevelMenuFlatten", toggle);
		GameSaveManager.Instance.Save();
	}

	private void ShowModCatalogs()
	{
		_showingModCatalogs = true;
		Global.Instance.currentAwardMode = false;
		gui.Visible = false;
		guiFollow.Visible = false;
		chapterMenu.Set("alive", false);
		CanvasLayer browser = GetNode<CanvasLayer>("ModBrowser");
		browser.Visible = true;
		VBoxContainer node = browser.GetNode<VBoxContainer>("Panel/Scroll/Catalogs");
		foreach (Node child in node.GetChildren())
		{
			node.RemoveChild(child);
			child.QueueFree();
		}
		node.AddChild(new Label
		{
			Text = "Mod 关卡",
			HorizontalAlignment = HorizontalAlignment.Center
		}, forceReadableName: false, InternalMode.Disabled);
		IReadOnlyList<XWModContentCatalog.Catalog> catalogs = XWModContentCatalog.GetCatalogs();
		if (catalogs.Count == 0)
		{
			node.AddChild(new Label
			{
				Text = "没有可用的 Mod 关卡。请在 Mod 管理中安装并启用包含关卡目录的 Mod。",
				AutowrapMode = TextServer.AutowrapMode.WordSmart
			}, forceReadableName: false, InternalMode.Disabled);
		}
		foreach (XWModContentCatalog.Catalog catalog in catalogs)
		{
			Button button = new Button
			{
				Text = catalog.Title + "  ·  " + catalog.OwnerModId,
				CustomMinimumSize = new Vector2(0f, 56f)
			};
			button.Pressed += () =>
			{
				if (!XWModPlayerProgressService.CanPlay(catalog.OwnerModId, out var reason))
				{
					DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", reason);
				}
				else
				{
					XWModLevelSession.SelectCatalog(catalog.OwnerModId, catalog.Key);
					currentChapterList = catalog.Data["Chapter"].AsGodotArray();
					foreach (Node child2 in chapterMenu.GetChildren())
					{
						chapterMenu.RemoveChild(child2);
						child2.QueueFree();
					}
					browser.Visible = false;
					_showingModCatalogs = false;
					gui.Visible = true;
					guiFollow.Visible = true;
					chapterMenu.Set("alive", true);
					informationLabel.Text = catalog.Title;
					InitChapter();
				}
			};
			node.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private async Task LaunchModLevel(int id)
	{
		Dictionary dictionary = currentChapter["Level"].AsGodotArray()[id].AsGodotDictionary();
		XWModLevelIdentity xWModLevelIdentity = XWModLevelSession.ForLevel(difficulty: (dictionary["Level"].AsGodotDictionary().GetValueOrDefault(_modDifficulty, "").AsString()
			.Length > 0) ? _modDifficulty : "Normal", key: dictionary["SaveKey"].AsString());
		if (!XWModPlayerProgressService.CanPlay(xWModLevelIdentity.OwnerModId, out var reason))
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", reason);
			return;
		}
		if (!XWModContentCatalog.TryResolve(xWModLevelIdentity, out var config, out var error))
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", error);
			return;
		}
		XWModLevelSession.Select(xWModLevelIdentity);
		TowerDefenseManager.Instance.currentLevelConfig = config;
		Global.Instance.currentLevelId = id;
		Global.Instance.enterLevelMode = "ModLevel";
		if (Global.IsMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			MultiPlayerManager.Instance.SendSelectLevel(xWModLevelIdentity.LevelSaveKey);
			await MultiPlayerManager.Instance.StartCompatibleGameAsync();
		}
		else
		{
			SceneManager.Instance.ChangeScene("TowerDefense");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName.ReadProgress, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitChapter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "chapterId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Select, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Back, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OptionButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DifficultChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LevelMenuFlattenCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowModCatalogs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReadProgress && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ReadProgress(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.InitChapter && args.Count == 0)
		{
			InitChapter();
			ret = default;
			return true;
		}
		if (method == MethodName.InitLevel && args.Count == 1)
		{
			InitLevel(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Select && args.Count == 1)
		{
			Select(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Back && args.Count == 0)
		{
			Back();
			ret = default;
			return true;
		}
		if (method == MethodName.OptionButtonPressed && args.Count == 0)
		{
			OptionButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.DifficultChange && args.Count == 0)
		{
			DifficultChange();
			ret = default;
			return true;
		}
		if (method == MethodName.LevelMenuFlattenCheckBoxToggled && args.Count == 1)
		{
			LevelMenuFlattenCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowModCatalogs && args.Count == 0)
		{
			ShowModCatalogs();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ReadProgress)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.InitChapter)
		{
			return true;
		}
		if (method == MethodName.InitLevel)
		{
			return true;
		}
		if (method == MethodName.Select)
		{
			return true;
		}
		if (method == MethodName.Back)
		{
			return true;
		}
		if (method == MethodName.OptionButtonPressed)
		{
			return true;
		}
		if (method == MethodName.DifficultChange)
		{
			return true;
		}
		if (method == MethodName.LevelMenuFlattenCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.ShowModCatalogs)
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
		if (name == PropertyName.levelChooseMarker)
		{
			levelChooseMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.defaultMarker)
		{
			defaultMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.gui)
		{
			gui = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.guiFollow)
		{
			guiFollow = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.optionGUI)
		{
			optionGUI = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.informationLabel)
		{
			informationLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.chapterMenu)
		{
			chapterMenu = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName.levelMenu)
		{
			levelMenu = VariantUtils.ConvertTo<DragMenu>(in value);
			return true;
		}
		if (name == PropertyName.normalButton)
		{
			normalButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.difficultButton)
		{
			difficultButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.normalLabel)
		{
			normalLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.difficultLabel)
		{
			difficultLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.levelMenuFlattenCheckBox)
		{
			levelMenuFlattenCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.chapterTexture)
		{
			chapterTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.chapterSelectBackground)
		{
			chapterSelectBackground = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.levelMenuFlatten)
		{
			levelMenuFlatten = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.levelContainer)
		{
			levelContainer = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName.currentChapterList)
		{
			currentChapterList = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.currentChapter)
		{
			currentChapter = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.currentChapterIndex)
		{
			currentChapterIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentMode)
		{
			currentMode = VariantUtils.ConvertTo<LevelChooseMode>(in value);
			return true;
		}
		if (name == PropertyName.saveBackMode)
		{
			saveBackMode = VariantUtils.ConvertTo<LevelChooseMode>(in value);
			return true;
		}
		if (name == PropertyName.tween)
		{
			tween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._showingModCatalogs)
		{
			_showingModCatalogs = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._modDifficulty)
		{
			_modDifficulty = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.IsModBrowser)
		{
			value = VariantUtils.CreateFrom<bool>(IsModBrowser);
			return true;
		}
		if (name == PropertyName.camera)
		{
			value = VariantUtils.CreateFrom(in camera);
			return true;
		}
		if (name == PropertyName.levelChooseMarker)
		{
			value = VariantUtils.CreateFrom(in levelChooseMarker);
			return true;
		}
		if (name == PropertyName.defaultMarker)
		{
			value = VariantUtils.CreateFrom(in defaultMarker);
			return true;
		}
		if (name == PropertyName.gui)
		{
			value = VariantUtils.CreateFrom(in gui);
			return true;
		}
		if (name == PropertyName.guiFollow)
		{
			value = VariantUtils.CreateFrom(in guiFollow);
			return true;
		}
		if (name == PropertyName.optionGUI)
		{
			value = VariantUtils.CreateFrom(in optionGUI);
			return true;
		}
		if (name == PropertyName.informationLabel)
		{
			value = VariantUtils.CreateFrom(in informationLabel);
			return true;
		}
		if (name == PropertyName.chapterMenu)
		{
			value = VariantUtils.CreateFrom(in chapterMenu);
			return true;
		}
		if (name == PropertyName.levelMenu)
		{
			value = VariantUtils.CreateFrom(in levelMenu);
			return true;
		}
		if (name == PropertyName.normalButton)
		{
			value = VariantUtils.CreateFrom(in normalButton);
			return true;
		}
		if (name == PropertyName.difficultButton)
		{
			value = VariantUtils.CreateFrom(in difficultButton);
			return true;
		}
		if (name == PropertyName.normalLabel)
		{
			value = VariantUtils.CreateFrom(in normalLabel);
			return true;
		}
		if (name == PropertyName.difficultLabel)
		{
			value = VariantUtils.CreateFrom(in difficultLabel);
			return true;
		}
		if (name == PropertyName.levelMenuFlattenCheckBox)
		{
			value = VariantUtils.CreateFrom(in levelMenuFlattenCheckBox);
			return true;
		}
		if (name == PropertyName.chapterTexture)
		{
			value = VariantUtils.CreateFrom(in chapterTexture);
			return true;
		}
		if (name == PropertyName.chapterSelectBackground)
		{
			value = VariantUtils.CreateFrom(in chapterSelectBackground);
			return true;
		}
		if (name == PropertyName.levelMenuFlatten)
		{
			value = VariantUtils.CreateFrom(in levelMenuFlatten);
			return true;
		}
		if (name == PropertyName.levelContainer)
		{
			value = VariantUtils.CreateFrom(in levelContainer);
			return true;
		}
		if (name == PropertyName.currentChapterList)
		{
			value = VariantUtils.CreateFrom(in currentChapterList);
			return true;
		}
		if (name == PropertyName.currentChapter)
		{
			value = VariantUtils.CreateFrom(in currentChapter);
			return true;
		}
		if (name == PropertyName.currentChapterIndex)
		{
			value = VariantUtils.CreateFrom(in currentChapterIndex);
			return true;
		}
		if (name == PropertyName.currentMode)
		{
			value = VariantUtils.CreateFrom(in currentMode);
			return true;
		}
		if (name == PropertyName.saveBackMode)
		{
			value = VariantUtils.CreateFrom(in saveBackMode);
			return true;
		}
		if (name == PropertyName.tween)
		{
			value = VariantUtils.CreateFrom(in tween);
			return true;
		}
		if (name == PropertyName._showingModCatalogs)
		{
			value = VariantUtils.CreateFrom(in _showingModCatalogs);
			return true;
		}
		if (name == PropertyName._modDifficulty)
		{
			value = VariantUtils.CreateFrom(in _modDifficulty);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.levelChooseMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.defaultMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.gui, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.guiFollow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.optionGUI, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.informationLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.chapterMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.normalButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.difficultButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.normalLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.difficultLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelMenuFlattenCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.chapterTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.chapterSelectBackground, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelMenuFlatten, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.currentChapterList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.currentChapter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentChapterIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.saveBackMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.tween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsModBrowser, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._showingModCatalogs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._modDifficulty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.camera, Variant.From(in camera));
		info.AddProperty(PropertyName.levelChooseMarker, Variant.From(in levelChooseMarker));
		info.AddProperty(PropertyName.defaultMarker, Variant.From(in defaultMarker));
		info.AddProperty(PropertyName.gui, Variant.From(in gui));
		info.AddProperty(PropertyName.guiFollow, Variant.From(in guiFollow));
		info.AddProperty(PropertyName.optionGUI, Variant.From(in optionGUI));
		info.AddProperty(PropertyName.informationLabel, Variant.From(in informationLabel));
		info.AddProperty(PropertyName.chapterMenu, Variant.From(in chapterMenu));
		info.AddProperty(PropertyName.levelMenu, Variant.From(in levelMenu));
		info.AddProperty(PropertyName.normalButton, Variant.From(in normalButton));
		info.AddProperty(PropertyName.difficultButton, Variant.From(in difficultButton));
		info.AddProperty(PropertyName.normalLabel, Variant.From(in normalLabel));
		info.AddProperty(PropertyName.difficultLabel, Variant.From(in difficultLabel));
		info.AddProperty(PropertyName.levelMenuFlattenCheckBox, Variant.From(in levelMenuFlattenCheckBox));
		info.AddProperty(PropertyName.chapterTexture, Variant.From(in chapterTexture));
		info.AddProperty(PropertyName.chapterSelectBackground, Variant.From(in chapterSelectBackground));
		info.AddProperty(PropertyName.levelMenuFlatten, Variant.From(in levelMenuFlatten));
		info.AddProperty(PropertyName.levelContainer, Variant.From(in levelContainer));
		info.AddProperty(PropertyName.currentChapterList, Variant.From(in currentChapterList));
		info.AddProperty(PropertyName.currentChapter, Variant.From(in currentChapter));
		info.AddProperty(PropertyName.currentChapterIndex, Variant.From(in currentChapterIndex));
		info.AddProperty(PropertyName.currentMode, Variant.From(in currentMode));
		info.AddProperty(PropertyName.saveBackMode, Variant.From(in saveBackMode));
		info.AddProperty(PropertyName.tween, Variant.From(in tween));
		info.AddProperty(PropertyName._showingModCatalogs, Variant.From(in _showingModCatalogs));
		info.AddProperty(PropertyName._modDifficulty, Variant.From(in _modDifficulty));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.camera, out var value))
		{
			camera = value.As<Camera2D>();
		}
		if (info.TryGetProperty(PropertyName.levelChooseMarker, out var value2))
		{
			levelChooseMarker = value2.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.defaultMarker, out var value3))
		{
			defaultMarker = value3.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.gui, out var value4))
		{
			gui = value4.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.guiFollow, out var value5))
		{
			guiFollow = value5.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.optionGUI, out var value6))
		{
			optionGUI = value6.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.informationLabel, out var value7))
		{
			informationLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.chapterMenu, out var value8))
		{
			chapterMenu = value8.As<Node>();
		}
		if (info.TryGetProperty(PropertyName.levelMenu, out var value9))
		{
			levelMenu = value9.As<DragMenu>();
		}
		if (info.TryGetProperty(PropertyName.normalButton, out var value10))
		{
			normalButton = value10.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.difficultButton, out var value11))
		{
			difficultButton = value11.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.normalLabel, out var value12))
		{
			normalLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.difficultLabel, out var value13))
		{
			difficultLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.levelMenuFlattenCheckBox, out var value14))
		{
			levelMenuFlattenCheckBox = value14.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.chapterTexture, out var value15))
		{
			chapterTexture = value15.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.chapterSelectBackground, out var value16))
		{
			chapterSelectBackground = value16.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.levelMenuFlatten, out var value17))
		{
			levelMenuFlatten = value17.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.levelContainer, out var value18))
		{
			levelContainer = value18.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName.currentChapterList, out var value19))
		{
			currentChapterList = value19.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.currentChapter, out var value20))
		{
			currentChapter = value20.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.currentChapterIndex, out var value21))
		{
			currentChapterIndex = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentMode, out var value22))
		{
			currentMode = value22.As<LevelChooseMode>();
		}
		if (info.TryGetProperty(PropertyName.saveBackMode, out var value23))
		{
			saveBackMode = value23.As<LevelChooseMode>();
		}
		if (info.TryGetProperty(PropertyName.tween, out var value24))
		{
			tween = value24.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._showingModCatalogs, out var value25))
		{
			_showingModCatalogs = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._modDifficulty, out var value26))
		{
			_modDifficulty = value26.As<string>();
		}
	}
}
