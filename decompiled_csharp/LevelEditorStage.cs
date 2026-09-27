using System.Collections.Generic;
using System.ComponentModel;
using Cysharp.Text;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Scene/LevelEditorStage/LevelEditorStage.cs")]
public class LevelEditorStage : Control
{
	public new class MethodName : Control.MethodName
	{
		public static readonly StringName Init = "Init";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ModLevelsButtonPressed = "ModLevelsButtonPressed";

		public static readonly StringName PanelChange = "PanelChange";

		public static readonly StringName LevelMineButtonPressed = "LevelMineButtonPressed";

		public static readonly StringName MigrateDiyLevelTresIfNeeded = "MigrateDiyLevelTresIfNeeded";

		public static readonly StringName MyLevelSelect = "MyLevelSelect";

		public static readonly StringName MyLevelDelete = "MyLevelDelete";

		public static readonly StringName LevelOnlineButtonPressed = "LevelOnlineButtonPressed";

		public static readonly StringName LevelDiyButtonPressed = "LevelDiyButtonPressed";

		public static readonly StringName LevelLoadButtonPressed = "LevelLoadButtonPressed";

		public static readonly StringName LevelBattleButtonPressed = "LevelBattleButtonPressed";

		public static readonly StringName LevelQuizButtonPressed = "LevelQuizButtonPressed";

		public static readonly StringName LevelTestButtonPressed = "LevelTestButtonPressed";

		public static readonly StringName Save = "Save";

		public static readonly StringName Export = "Export";

		public static readonly StringName SaveFileTo = "SaveFileTo";

		public static readonly StringName LevelEditorBackButtonPressed = "LevelEditorBackButtonPressed";

		public static readonly StringName AnimeExit = "AnimeExit";

		public static readonly StringName AnimeEnter = "AnimeEnter";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName LevelEditorStageTop = "LevelEditorStageTop";

		public static readonly StringName LevelEditorStageLeft = "LevelEditorStageLeft";

		public static readonly StringName LevelEditorStageRight = "LevelEditorStageRight";

		public static readonly StringName LevelEditorChooseLayer = "LevelEditorChooseLayer";

		public static readonly StringName LevelEditorMyLevelLayer = "LevelEditorMyLevelLayer";

		public static readonly StringName LevelEditorOnlineLevelLayer = "LevelEditorOnlineLevelLayer";

		public static readonly StringName LevelEditorLayer = "LevelEditorLayer";

		public static readonly StringName LevelEditorBattleLayer = "LevelEditorBattleLayer";

		public static readonly StringName LevelEditorQuizLayer = "LevelEditorQuizLayer";

		public static readonly StringName LevelEditorInformationEditor = "LevelEditorInformationEditor";

		public static readonly StringName LevelEditorEventEditor = "LevelEditorEventEditor";

		public static readonly StringName LevelEditorMapEditor = "LevelEditorMapEditor";

		public static readonly StringName LevelEditorSeedbankEditor = "LevelEditorSeedbankEditor";

		public static readonly StringName LevelEditorWaveEditor = "LevelEditorWaveEditor";

		public static readonly StringName LevelEditorBattle = "LevelEditorBattle";

		public static readonly StringName LevelEditorQuiz = "LevelEditorQuiz";

		public static readonly StringName LevelInformationButton = "LevelInformationButton";

		public static readonly StringName LevelEventButton = "LevelEventButton";

		public static readonly StringName LevelMapButton = "LevelMapButton";

		public static readonly StringName LevelSeedbankButton = "LevelSeedbankButton";

		public static readonly StringName LevelWaveButton = "LevelWaveButton";

		public static readonly StringName MyLevelContainer = "MyLevelContainer";

		public static readonly StringName LevelEditorOnlineLevel = "LevelEditorOnlineLevel";

		public static readonly StringName LevelConfig = "LevelConfig";

		public static readonly StringName CurrentUid = "CurrentUid";

		public static readonly StringName LevelUid = "LevelUid";

		public static readonly StringName Skip = "Skip";

		public static readonly StringName Over = "Over";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static ButtonGroup _levelEditorPanelButtonGroup;

	private static PackedScene _levelEditorMyLevelItem;

	private const string Path = "user://Csharp/Diy";

	public TextureRect LevelEditorStageTop;

	public TextureRect LevelEditorStageLeft;

	public TextureRect LevelEditorStageRight;

	public CanvasLayer LevelEditorChooseLayer;

	public CanvasLayer LevelEditorMyLevelLayer;

	public CanvasLayer LevelEditorOnlineLevelLayer;

	public CanvasLayer LevelEditorLayer;

	public CanvasLayer LevelEditorBattleLayer;

	public CanvasLayer LevelEditorQuizLayer;

	public LevelEditorInformationEditor LevelEditorInformationEditor;

	public LevelEditorEventEditor LevelEditorEventEditor;

	public LevelEditorMapEditor LevelEditorMapEditor;

	public LevelEditorSeedbankEditor LevelEditorSeedbankEditor;

	public LevelEditorWaveEditor LevelEditorWaveEditor;

	public LevelEditorBattle LevelEditorBattle;

	public Control LevelEditorQuiz;

	public MainButton LevelInformationButton;

	public MainButton LevelEventButton;

	public MainButton LevelMapButton;

	public MainButton LevelSeedbankButton;

	public MainButton LevelWaveButton;

	public HFlowContainer MyLevelContainer;

	public LevelEditorOnlineLevel LevelEditorOnlineLevel;

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelConfig LevelConfig;

	public static bool ShowTips;

	public string CurrentUid = "";

	public Array<string> LevelUid = new Array<string>();

	public bool Skip;

	public bool Over;

	private static ButtonGroup LevelEditorPanelButtonGroup => _levelEditorPanelButtonGroup ?? (_levelEditorPanelButtonGroup = GD.Load<ButtonGroup>("uid://clop83geu1mo3"));

	private static PackedScene LevelEditorMyLevelItem => _levelEditorMyLevelItem ?? (_levelEditorMyLevelItem = GD.Load<PackedScene>("uid://jtdmoji4acxv"));

	public void Init(TowerDefenseLevelConfig _levelConfig)
	{
		LevelConfig = _levelConfig;
		LevelEditorInformationEditor.Init(LevelConfig);
		LevelEditorEventEditor.Init(LevelConfig);
		LevelEditorMapEditor.Init(LevelConfig);
		LevelEditorSeedbankEditor.Init(LevelConfig);
		LevelEditorWaveEditor.Init(LevelConfig);
	}

	public override void _EnterTree()
	{
		LevelEditorStageTop = GetNode<TextureRect>("%LevelEditorStageTop");
		LevelEditorStageLeft = GetNode<TextureRect>("%LevelEditorStageLeft");
		LevelEditorStageRight = GetNode<TextureRect>("%LevelEditorStageRight");
		LevelEditorChooseLayer = GetNode<CanvasLayer>("%LevelEditorChooseLayer");
		LevelEditorMyLevelLayer = GetNode<CanvasLayer>("%LevelEditorMyLevelLayer");
		LevelEditorOnlineLevelLayer = GetNode<CanvasLayer>("%LevelEditorOnlineLevelLayer");
		LevelEditorLayer = GetNode<CanvasLayer>("%LevelEditorLayer");
		LevelEditorBattleLayer = GetNode<CanvasLayer>("%LevelEditorBattleLayer");
		LevelEditorQuizLayer = GetNode<CanvasLayer>("%LevelEditorQuizLayer");
		LevelEditorInformationEditor = GetNode<LevelEditorInformationEditor>("%LevelEditorInformationEditor");
		LevelEditorEventEditor = GetNode<LevelEditorEventEditor>("%LevelEditorEventEditor");
		LevelEditorMapEditor = GetNode<LevelEditorMapEditor>("%LevelEditorMapEditor");
		LevelEditorSeedbankEditor = GetNode<LevelEditorSeedbankEditor>("%LevelEditorSeedbankEditor");
		LevelEditorWaveEditor = GetNode<LevelEditorWaveEditor>("%LevelEditorWaveEditor");
		LevelEditorBattle = GetNode<LevelEditorBattle>("%LevelEditorBattle");
		LevelEditorQuiz = GetNode<Control>("%LevelEditorQuiz");
		LevelInformationButton = GetNode<MainButton>("%LevelInformationButton");
		LevelEventButton = GetNode<MainButton>("%LevelEventButton");
		LevelMapButton = GetNode<MainButton>("%LevelMapButton");
		LevelSeedbankButton = GetNode<MainButton>("%LevelSeedbankButton");
		LevelWaveButton = GetNode<MainButton>("%LevelWaveButton");
		MyLevelContainer = GetNode<HFlowContainer>("%MyLevelContainer");
		LevelEditorOnlineLevel = GetNode<LevelEditorOnlineLevel>("%LevelEditorOnlineLevel");
		if (Global.IsEditor && Global.Instance.enterLevelMode == "DiyLevel")
		{
			Skip = true;
			CurrentUid = Global.Instance.currentDiyLevelUid;
			Global.Instance.enterLevelMode = "";
		}
		Global.Instance.isEditor = true;
	}

	public override void _Ready()
	{
		AudioManager.Instance.AudioPlay("ZenGarden", AudioManagerEnum.TYPE.MUSIC);
		if (!DirAccess.DirExistsAbsolute("user://Csharp/Diy"))
		{
			DirAccess.MakeDirRecursiveAbsolute("user://Csharp/Diy");
		}
		string[] filesAt = DirAccess.GetFilesAt("user://Csharp/Diy");
		foreach (string instance in filesAt)
		{
			LevelUid.Add(instance.GetBaseName());
		}
		AnimeEnter();
		LevelInformationButton.ButtonGroup = LevelEditorPanelButtonGroup;
		LevelEventButton.ButtonGroup = LevelInformationButton.ButtonGroup;
		LevelMapButton.ButtonGroup = LevelInformationButton.ButtonGroup;
		LevelSeedbankButton.ButtonGroup = LevelInformationButton.ButtonGroup;
		LevelWaveButton.ButtonGroup = LevelInformationButton.ButtonGroup;
		if (Skip)
		{
			if (TowerDefenseManager.Instance.currentLevelConfig is TowerDefenseLevelConfig levelConfig)
			{
				Init(levelConfig);
				LevelDiyButtonPressed(init: false);
			}
			else
			{
				GD.PushError("[LevelEditorStage] DIY level configuration is missing or incompatible.");
			}
		}
		else if (!ShowTips)
		{
			DialogManager.Instance.DialogCreate("LevelEditorTips");
			ShowTips = true;
		}
		if (Global.Instance.enterLevelIsBattle)
		{
			if (!Global.Instance.enterLevelIsBattleFinish)
			{
				GameSaveManager.Instance.SetKeyValue("LevelEditorBattleFailNum", GameSaveManager.Instance.GetKeyValue("LevelEditorBattleFailNum").AsInt32() + 1);
			}
			else
			{
				GameSaveManager.Instance.SetKeyValue("LevelEditorBattleFinishNum", GameSaveManager.Instance.GetKeyValue("LevelEditorBattleFinishNum").AsInt32() + 1);
			}
			GameSaveManager.Instance.SetKeyValue("LevelEditorBattleCurrentLevel", "-1");
			GameSaveManager.Instance.Save();
			Global.Instance.enterLevelIsBattleFinish = false;
			Global.Instance.enterLevelIsBattle = false;
			LevelBattleButtonPressed();
		}
		else if (Global.Instance.enterLevelId != "-1")
		{
			LevelOnlineButtonPressed();
		}
		if (Global.Instance.enterLevelId != "-1")
		{
			OnlineLevelPreview onlineLevelPreview = (OnlineLevelPreview)DialogManager.Instance.DialogCreate("OnlineLevelPreview");
			if (LevelEditorOnlineLevelLayer.Visible)
			{
				onlineLevelPreview.OnAuthorSelected += LevelEditorOnlineLevel.OpenAuthorLevels;
			}
			onlineLevelPreview.InitDialog(Global.Instance.enterLevelId);
			onlineLevelPreview.OnSelect += (string url) =>
			{
				LevelEditorOnlineLevel.EnterLevel(url, Global.Instance.enterLevelId);
			};
			Global.Instance.enterLevelId = "-1";
		}
		GetNode<BaseButton>("GUI/LevelEditorChooseLayer/LevelEditorChooseButtonNode/VBoxContainer/LevelMineButton").Pressed += LevelMineButtonPressed;
		GetNode<BaseButton>("GUI/LevelEditorChooseLayer/LevelEditorChooseButtonNode/VBoxContainer/LevelOnlineButton").Pressed += LevelOnlineButtonPressed;
		GetNode<BaseButton>("GUI/LevelEditorChooseLayer/LevelEditorChooseButtonNode/VBoxContainer/LevelDiyButton").Pressed += () =>
		{
			LevelDiyButtonPressed();
		};
		GetNode<BaseButton>("GUI/LevelEditorChooseLayer/LevelEditorChooseButtonNode/VBoxContainer/LevelLoadButton").Pressed += LevelLoadButtonPressed;
		GetNode<BaseButton>("GUI/LevelEditorChooseLayer/LevelBattleButton").Pressed += LevelBattleButtonPressed;
		GetNode<BaseButton>("GUI/LevelEditorChooseLayer/LevelQuizButton").Pressed += LevelQuizButtonPressed;
		GetNode<MainButton>("GUI/LevelEditorChooseLayer/ModLevelsButton").Pressed += ModLevelsButtonPressed;
		GetNode<BaseButton>("%LevelInformationButton").Pressed += PanelChange;
		GetNode<BaseButton>("%LevelEventButton").Pressed += PanelChange;
		GetNode<BaseButton>("%LevelMapButton").Pressed += PanelChange;
		GetNode<BaseButton>("%LevelSeedbankButton").Pressed += PanelChange;
		GetNode<BaseButton>("%LevelWaveButton").Pressed += PanelChange;
		GetNode<BaseButton>("%LevelTestButton").Pressed += LevelTestButtonPressed;
		GetNode<BaseButton>("%LevelSaveButton").Pressed += Save;
		GetNode<BaseButton>("%LevelExportButton").Pressed += Export;
		GetNode<BaseButton>("GUI/ButtonLayer/LevelEditorBackButton").Pressed += LevelEditorBackButtonPressed;
	}

	private void ModLevelsButtonPressed()
	{
		if (!Over && LevelEditorChooseLayer.Visible)
		{
			Over = true;
			XWModLevelSession.Clear();
			Global.Instance.isEditor = false;
			Global.Instance.currentLevelChoose = "__ModLevels";
			Global.Instance.currentChapterId = -1;
			Global.Instance.currentLevelId = -1;
			SceneManager.Instance.ChangeScene("LevelChoose");
		}
	}

	public void PanelChange()
	{
		LevelEditorInformationEditor.Visible = LevelInformationButton.ButtonPressed;
		LevelEditorEventEditor.Visible = LevelEventButton.ButtonPressed;
		LevelEditorMapEditor.Visible = LevelMapButton.ButtonPressed;
		LevelEditorSeedbankEditor.Visible = LevelSeedbankButton.ButtonPressed;
		LevelEditorWaveEditor.Visible = LevelWaveButton.ButtonPressed;
	}

	public void LevelMineButtonPressed()
	{
		LevelEditorChooseLayer.Visible = false;
		LevelEditorMyLevelLayer.Visible = true;
		AnimeExit();
		LevelUid.Clear();
		foreach (Node child in MyLevelContainer.GetChildren())
		{
			child.QueueFree();
		}
		List<System.Collections.Generic.Dictionary<string, Variant>> list = new List<System.Collections.Generic.Dictionary<string, Variant>>();
		string[] filesAt = DirAccess.GetFilesAt("user://Csharp/Diy");
		foreach (string text in filesAt)
		{
			ulong modifiedTime = FileAccess.GetModifiedTime("user://Csharp/Diy".PathJoin(text));
			System.Collections.Generic.Dictionary<string, Variant> item = new System.Collections.Generic.Dictionary<string, Variant>
			{
				["fileName"] = text,
				["fileTime"] = modifiedTime
			};
			list.Add(item);
		}
		list.Sort((System.Collections.Generic.Dictionary<string, Variant> a, System.Collections.Generic.Dictionary<string, Variant> b) => (int)((long)b["fileTime"] - (long)a["fileTime"]));
		foreach (System.Collections.Generic.Dictionary<string, Variant> item2 in list)
		{
			LevelEditorMyLevelItem levelEditorMyLevelItem = LevelEditorMyLevelItem.Instantiate<LevelEditorMyLevelItem>(PackedScene.GenEditState.Disabled);
			levelEditorMyLevelItem.OnSelect += MyLevelSelect;
			levelEditorMyLevelItem.OnDelete += MyLevelDelete;
			MyLevelContainer.AddChild(levelEditorMyLevelItem, forceReadableName: false, InternalMode.Disabled);
			levelEditorMyLevelItem.Init(item2["fileName"].AsString().GetBaseName());
			LevelUid.Add(item2["fileName"].AsString().GetBaseName());
		}
	}

	public static bool MigrateDiyLevelTresIfNeeded(string filePath)
	{
		return ResourceTextScriptMigration.MigrateLevelEditorResourceForLoadIfNeeded(filePath);
	}

	public void MyLevelSelect(string uid)
	{
		LevelEditorMyLevelLayer.Visible = false;
		string text = ZString.Concat("user://Csharp/Diy", "/", uid, ".tres");
		if (FileAccess.FileExists(text) && (MigrateDiyLevelTresIfNeeded(text) ? ResourceLoader.Load(text, "", ResourceLoader.CacheMode.Ignore) : GD.Load(text)) is TowerDefenseLevelConfig levelConfig)
		{
			CurrentUid = uid;
			Init(levelConfig);
			LevelDiyButtonPressed(init: false, isLoad: true);
		}
	}

	public void MyLevelDelete(string uid)
	{
		string path = ZString.Concat("user://Csharp/Diy", "/", uid, ".tres");
		if (FileAccess.FileExists(path))
		{
			DirAccess.RemoveAbsolute(path);
			LevelUid.Remove(uid);
		}
	}

	public void LevelOnlineButtonPressed()
	{
		LevelEditorChooseLayer.Visible = false;
		LevelEditorOnlineLevelLayer.Visible = true;
		AnimeExit();
		LevelEditorOnlineLevel.GetPage(Mathf.Max(1, LevelEditorOnlineLevel.currentPage));
	}

	public void LevelDiyButtonPressed(bool init = true, bool isLoad = false)
	{
		if (init)
		{
			TowerDefenseLevelConfig levelConfig = new TowerDefenseLevelConfig();
			LevelEditorChooseLayer.Visible = false;
			LevelEditorLayer.Visible = true;
			uint num = GD.Randi();
			while (LevelUid.Contains(num.ToString()))
			{
				num = GD.Randi();
			}
			LevelUid.Add(num.ToString());
			CurrentUid = num.ToString();
			Init(levelConfig);
			Save();
			if (!isLoad)
			{
				AnimeExit();
			}
		}
		else
		{
			LevelEditorChooseLayer.Visible = false;
			LevelEditorLayer.Visible = true;
			if (!isLoad)
			{
				AnimeExit();
			}
		}
	}

	public void LevelLoadButtonPressed()
	{
		CommandManager.Instance.LoadLevelButtonPressed();
	}

	public void LevelBattleButtonPressed()
	{
		TowerDefenseManager.Instance.coinBank.ShowCoinBank(new Vector2(450f, 557f), still: true);
		LevelEditorChooseLayer.Visible = false;
		LevelEditorBattle._Show();
		LevelEditorBattleLayer.Visible = true;
	}

	public void LevelQuizButtonPressed()
	{
		LevelEditorQuizLayer.Visible = true;
		LevelEditorChooseLayer.Visible = false;
	}

	public void LevelTestButtonPressed()
	{
		if (!Over)
		{
			Save();
			Over = true;
			Global.Instance.enterLevelMode = "DiyLevel";
			Global.Instance.currentDiyLevelUid = CurrentUid;
			LevelEditorLayer.Visible = false;
			TowerDefenseManager.Instance.currentLevelConfig = LevelConfig;
			SceneManager.Instance.ChangeScene("TowerDefense");
		}
	}

	public async void Save()
	{
		if (Over)
		{
			return;
		}
		if (GodotObject.IsInstanceValid(LevelEditorWaveEditor) && LevelEditorWaveEditor.isLoading)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (GodotObject.IsInstanceValid(LevelEditorWaveEditor) && LevelEditorWaveEditor.isLoading)
			{
				return;
			}
		}
		LevelEditorEventEditor.Save();
		LevelEditorMapEditor.Save(isSave: true);
		LevelEditorSeedbankEditor.Save();
		LevelEditorWaveEditor.Save();
		LevelConfig.PrepareForEditorSave();
		ResourceSaver.Save(LevelConfig, ZString.Concat("user://Csharp/Diy", "/", CurrentUid, ".tres"), ResourceSaver.SaverFlags.None);
	}

	public void Export()
	{
		Save();
		DisplayServer.FileDialogShow("另存为项目", "", "", showHidden: false, DisplayServer.FileDialogMode.SaveFile, new string[1] { "*.json" }, Callable.From<bool, string[], int>(SaveFileTo));
	}

	private void SaveFileTo(bool status, string[] selectedPaths, int selectedFilterIndex)
	{
		if (selectedPaths.Length >= 1)
		{
			string text = selectedPaths[0];
			if (text.GetExtension() != "json")
			{
				text += ".json";
			}
			Save();
			FileAccess fileAccess = FileAccess.Open(text, FileAccess.ModeFlags.WriteRead);
			if (fileAccess != null)
			{
				fileAccess.StoreString(Json.Stringify(LevelConfig.Export()));
				fileAccess.Close();
			}
		}
	}

	public void LevelEditorBackButtonPressed()
	{
		if (LevelEditorChooseLayer.Visible)
		{
			Global.Instance.isEditor = false;
			Global.Instance.enterLevelIsBattle = false;
			Global.Instance.enterLevelIsBattleFinish = false;
			SceneManager.Instance.ChangeScene("MainMenu");
		}
		else if (LevelEditorMyLevelLayer.Visible)
		{
			AnimeEnter();
			LevelEditorChooseLayer.Visible = true;
			LevelEditorMyLevelLayer.Visible = false;
		}
		else if (LevelEditorOnlineLevelLayer.Visible)
		{
			if (!LevelEditorOnlineLevel.TryReturnToLevelList())
			{
				AnimeEnter();
				LevelEditorChooseLayer.Visible = true;
				LevelEditorOnlineLevelLayer.Visible = false;
			}
		}
		else if (LevelEditorLayer.Visible)
		{
			LevelInformationButton.ButtonPressed = true;
			PanelChange();
			Save();
			LevelEditorInformationEditor.Clear();
			LevelEditorEventEditor.Clear();
			LevelEditorMapEditor.Clear();
			LevelEditorSeedbankEditor.Clear();
			LevelEditorWaveEditor.Clear();
			AnimeEnter();
			LevelEditorChooseLayer.Visible = true;
			LevelEditorLayer.Visible = false;
		}
		else if (LevelEditorBattleLayer.Visible)
		{
			if (!LevelEditorBattle.overNode.Visible)
			{
				if (LevelEditorBattle.tipsNode.Visible)
				{
					TowerDefenseManager.Instance.coinBank.ShowCoinBank(new Vector2(450f, 557f), still: true);
					LevelEditorBattle.tipsNode.Visible = false;
					LevelEditorBattle.mainNode.Visible = true;
				}
				else if (LevelEditorBattle.awardNode.Visible)
				{
					TowerDefenseManager.Instance.coinBank.ShowCoinBank(new Vector2(450f, 557f), still: true);
					LevelEditorBattle.awardNode.Visible = false;
					LevelEditorBattle.mainNode.Visible = true;
				}
				else if (LevelEditorBattle.historyNode.Visible)
				{
					TowerDefenseManager.Instance.coinBank.ShowCoinBank(new Vector2(450f, 557f), still: true);
					LevelEditorBattle.historyNode.Visible = false;
					LevelEditorBattle.mainNode.Visible = true;
				}
				else
				{
					TowerDefenseManager.Instance.coinBank.CallDeferred("StartHide");
					LevelEditorChooseLayer.Visible = true;
					LevelEditorBattleLayer.Visible = false;
				}
			}
		}
		else if (LevelEditorQuizLayer.Visible && !(bool)LevelEditorQuiz.Get("mapChooseOver"))
		{
			LevelEditorQuizLayer.Visible = false;
			LevelEditorChooseLayer.Visible = true;
		}
	}

	public void AnimeExit()
	{
		Rect2 viewportRect = GetViewportRect();
		Tween tween = CreateTween();
		tween.SetParallel();
		tween.SetTrans(Tween.TransitionType.Quart);
		tween.SetEase(Tween.EaseType.Out);
		tween.TweenProperty(LevelEditorStageTop, "global_position", new Vector2(0f, -120f), 0.5);
		tween.TweenProperty(LevelEditorStageLeft, "global_position", new Vector2(-300f, 0f), 0.5);
		tween.TweenProperty(LevelEditorStageRight, "global_position", new Vector2(viewportRect.Position.X + viewportRect.Size.X + 300f, 0f), 0.5);
	}

	public void AnimeEnter()
	{
		Rect2 viewportRect = GetViewportRect();
		Tween tween = CreateTween();
		tween.SetParallel();
		tween.SetTrans(Tween.TransitionType.Quart);
		tween.SetEase(Tween.EaseType.Out);
		tween.TweenProperty(LevelEditorStageTop, "global_position", new Vector2(0f, 0f), 0.5);
		tween.TweenProperty(LevelEditorStageLeft, "global_position", new Vector2(0f, 0f), 0.5);
		tween.TweenProperty(LevelEditorStageRight, "global_position", new Vector2(viewportRect.Position.X + viewportRect.Size.X - 200f, 0f), 0.5);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_levelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ModLevelsButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PanelChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LevelMineButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MigrateDiyLevelTresIfNeeded, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MyLevelSelect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "uid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MyLevelDelete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "uid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelOnlineButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LevelDiyButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "init", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isLoad", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelLoadButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LevelBattleButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LevelQuizButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LevelTestButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Save, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFileTo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "status", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "selectedPaths", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "selectedFilterIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelEditorBackButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeExit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEnter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.ModLevelsButtonPressed && args.Count == 0)
		{
			ModLevelsButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.PanelChange && args.Count == 0)
		{
			PanelChange();
			ret = default;
			return true;
		}
		if (method == MethodName.LevelMineButtonPressed && args.Count == 0)
		{
			LevelMineButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.MigrateDiyLevelTresIfNeeded && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MigrateDiyLevelTresIfNeeded(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MyLevelSelect && args.Count == 1)
		{
			MyLevelSelect(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MyLevelDelete && args.Count == 1)
		{
			MyLevelDelete(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelOnlineButtonPressed && args.Count == 0)
		{
			LevelOnlineButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LevelDiyButtonPressed && args.Count == 2)
		{
			LevelDiyButtonPressed(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelLoadButtonPressed && args.Count == 0)
		{
			LevelLoadButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LevelBattleButtonPressed && args.Count == 0)
		{
			LevelBattleButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LevelQuizButtonPressed && args.Count == 0)
		{
			LevelQuizButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LevelTestButtonPressed && args.Count == 0)
		{
			LevelTestButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.Save && args.Count == 0)
		{
			Save();
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			Export();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFileTo && args.Count == 3)
		{
			SaveFileTo(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelEditorBackButtonPressed && args.Count == 0)
		{
			LevelEditorBackButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeExit && args.Count == 0)
		{
			AnimeExit();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEnter && args.Count == 0)
		{
			AnimeEnter();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.MigrateDiyLevelTresIfNeeded && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MigrateDiyLevelTresIfNeeded(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName._EnterTree)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.ModLevelsButtonPressed)
		{
			return true;
		}
		if (method == MethodName.PanelChange)
		{
			return true;
		}
		if (method == MethodName.LevelMineButtonPressed)
		{
			return true;
		}
		if (method == MethodName.MigrateDiyLevelTresIfNeeded)
		{
			return true;
		}
		if (method == MethodName.MyLevelSelect)
		{
			return true;
		}
		if (method == MethodName.MyLevelDelete)
		{
			return true;
		}
		if (method == MethodName.LevelOnlineButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LevelDiyButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LevelLoadButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LevelBattleButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LevelQuizButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LevelTestButtonPressed)
		{
			return true;
		}
		if (method == MethodName.Save)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.SaveFileTo)
		{
			return true;
		}
		if (method == MethodName.LevelEditorBackButtonPressed)
		{
			return true;
		}
		if (method == MethodName.AnimeExit)
		{
			return true;
		}
		if (method == MethodName.AnimeEnter)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.LevelEditorStageTop)
		{
			LevelEditorStageTop = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorStageLeft)
		{
			LevelEditorStageLeft = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorStageRight)
		{
			LevelEditorStageRight = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorChooseLayer)
		{
			LevelEditorChooseLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorMyLevelLayer)
		{
			LevelEditorMyLevelLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorOnlineLevelLayer)
		{
			LevelEditorOnlineLevelLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorLayer)
		{
			LevelEditorLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorBattleLayer)
		{
			LevelEditorBattleLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorQuizLayer)
		{
			LevelEditorQuizLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorInformationEditor)
		{
			LevelEditorInformationEditor = VariantUtils.ConvertTo<LevelEditorInformationEditor>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorEventEditor)
		{
			LevelEditorEventEditor = VariantUtils.ConvertTo<LevelEditorEventEditor>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorMapEditor)
		{
			LevelEditorMapEditor = VariantUtils.ConvertTo<LevelEditorMapEditor>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorSeedbankEditor)
		{
			LevelEditorSeedbankEditor = VariantUtils.ConvertTo<LevelEditorSeedbankEditor>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorWaveEditor)
		{
			LevelEditorWaveEditor = VariantUtils.ConvertTo<LevelEditorWaveEditor>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorBattle)
		{
			LevelEditorBattle = VariantUtils.ConvertTo<LevelEditorBattle>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorQuiz)
		{
			LevelEditorQuiz = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.LevelInformationButton)
		{
			LevelInformationButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.LevelEventButton)
		{
			LevelEventButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.LevelMapButton)
		{
			LevelMapButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.LevelSeedbankButton)
		{
			LevelSeedbankButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.LevelWaveButton)
		{
			LevelWaveButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.MyLevelContainer)
		{
			MyLevelContainer = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName.LevelEditorOnlineLevel)
		{
			LevelEditorOnlineLevel = VariantUtils.ConvertTo<LevelEditorOnlineLevel>(in value);
			return true;
		}
		if (name == PropertyName.LevelConfig)
		{
			LevelConfig = VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in value);
			return true;
		}
		if (name == PropertyName.CurrentUid)
		{
			CurrentUid = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.LevelUid)
		{
			LevelUid = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.Skip)
		{
			Skip = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Over)
		{
			Over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.LevelEditorStageTop)
		{
			value = VariantUtils.CreateFrom(in LevelEditorStageTop);
			return true;
		}
		if (name == PropertyName.LevelEditorStageLeft)
		{
			value = VariantUtils.CreateFrom(in LevelEditorStageLeft);
			return true;
		}
		if (name == PropertyName.LevelEditorStageRight)
		{
			value = VariantUtils.CreateFrom(in LevelEditorStageRight);
			return true;
		}
		if (name == PropertyName.LevelEditorChooseLayer)
		{
			value = VariantUtils.CreateFrom(in LevelEditorChooseLayer);
			return true;
		}
		if (name == PropertyName.LevelEditorMyLevelLayer)
		{
			value = VariantUtils.CreateFrom(in LevelEditorMyLevelLayer);
			return true;
		}
		if (name == PropertyName.LevelEditorOnlineLevelLayer)
		{
			value = VariantUtils.CreateFrom(in LevelEditorOnlineLevelLayer);
			return true;
		}
		if (name == PropertyName.LevelEditorLayer)
		{
			value = VariantUtils.CreateFrom(in LevelEditorLayer);
			return true;
		}
		if (name == PropertyName.LevelEditorBattleLayer)
		{
			value = VariantUtils.CreateFrom(in LevelEditorBattleLayer);
			return true;
		}
		if (name == PropertyName.LevelEditorQuizLayer)
		{
			value = VariantUtils.CreateFrom(in LevelEditorQuizLayer);
			return true;
		}
		if (name == PropertyName.LevelEditorInformationEditor)
		{
			value = VariantUtils.CreateFrom(in LevelEditorInformationEditor);
			return true;
		}
		if (name == PropertyName.LevelEditorEventEditor)
		{
			value = VariantUtils.CreateFrom(in LevelEditorEventEditor);
			return true;
		}
		if (name == PropertyName.LevelEditorMapEditor)
		{
			value = VariantUtils.CreateFrom(in LevelEditorMapEditor);
			return true;
		}
		if (name == PropertyName.LevelEditorSeedbankEditor)
		{
			value = VariantUtils.CreateFrom(in LevelEditorSeedbankEditor);
			return true;
		}
		if (name == PropertyName.LevelEditorWaveEditor)
		{
			value = VariantUtils.CreateFrom(in LevelEditorWaveEditor);
			return true;
		}
		if (name == PropertyName.LevelEditorBattle)
		{
			value = VariantUtils.CreateFrom(in LevelEditorBattle);
			return true;
		}
		if (name == PropertyName.LevelEditorQuiz)
		{
			value = VariantUtils.CreateFrom(in LevelEditorQuiz);
			return true;
		}
		if (name == PropertyName.LevelInformationButton)
		{
			value = VariantUtils.CreateFrom(in LevelInformationButton);
			return true;
		}
		if (name == PropertyName.LevelEventButton)
		{
			value = VariantUtils.CreateFrom(in LevelEventButton);
			return true;
		}
		if (name == PropertyName.LevelMapButton)
		{
			value = VariantUtils.CreateFrom(in LevelMapButton);
			return true;
		}
		if (name == PropertyName.LevelSeedbankButton)
		{
			value = VariantUtils.CreateFrom(in LevelSeedbankButton);
			return true;
		}
		if (name == PropertyName.LevelWaveButton)
		{
			value = VariantUtils.CreateFrom(in LevelWaveButton);
			return true;
		}
		if (name == PropertyName.MyLevelContainer)
		{
			value = VariantUtils.CreateFrom(in MyLevelContainer);
			return true;
		}
		if (name == PropertyName.LevelEditorOnlineLevel)
		{
			value = VariantUtils.CreateFrom(in LevelEditorOnlineLevel);
			return true;
		}
		if (name == PropertyName.LevelConfig)
		{
			value = VariantUtils.CreateFrom(in LevelConfig);
			return true;
		}
		if (name == PropertyName.CurrentUid)
		{
			value = VariantUtils.CreateFrom(in CurrentUid);
			return true;
		}
		if (name == PropertyName.LevelUid)
		{
			value = VariantUtils.CreateFromArray(LevelUid);
			return true;
		}
		if (name == PropertyName.Skip)
		{
			value = VariantUtils.CreateFrom(in Skip);
			return true;
		}
		if (name == PropertyName.Over)
		{
			value = VariantUtils.CreateFrom(in Over);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorStageTop, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorStageLeft, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorStageRight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorChooseLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorMyLevelLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorOnlineLevelLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorBattleLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorQuizLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorInformationEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorEventEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorMapEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorSeedbankEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorWaveEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorBattle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorQuiz, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelInformationButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEventButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelMapButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelSeedbankButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelWaveButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.MyLevelContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelEditorOnlineLevel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LevelConfig, PropertyHint.ResourceType, "TowerDefenseLevelConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.CurrentUid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.LevelUid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Skip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.LevelEditorStageTop, Variant.From(in LevelEditorStageTop));
		info.AddProperty(PropertyName.LevelEditorStageLeft, Variant.From(in LevelEditorStageLeft));
		info.AddProperty(PropertyName.LevelEditorStageRight, Variant.From(in LevelEditorStageRight));
		info.AddProperty(PropertyName.LevelEditorChooseLayer, Variant.From(in LevelEditorChooseLayer));
		info.AddProperty(PropertyName.LevelEditorMyLevelLayer, Variant.From(in LevelEditorMyLevelLayer));
		info.AddProperty(PropertyName.LevelEditorOnlineLevelLayer, Variant.From(in LevelEditorOnlineLevelLayer));
		info.AddProperty(PropertyName.LevelEditorLayer, Variant.From(in LevelEditorLayer));
		info.AddProperty(PropertyName.LevelEditorBattleLayer, Variant.From(in LevelEditorBattleLayer));
		info.AddProperty(PropertyName.LevelEditorQuizLayer, Variant.From(in LevelEditorQuizLayer));
		info.AddProperty(PropertyName.LevelEditorInformationEditor, Variant.From(in LevelEditorInformationEditor));
		info.AddProperty(PropertyName.LevelEditorEventEditor, Variant.From(in LevelEditorEventEditor));
		info.AddProperty(PropertyName.LevelEditorMapEditor, Variant.From(in LevelEditorMapEditor));
		info.AddProperty(PropertyName.LevelEditorSeedbankEditor, Variant.From(in LevelEditorSeedbankEditor));
		info.AddProperty(PropertyName.LevelEditorWaveEditor, Variant.From(in LevelEditorWaveEditor));
		info.AddProperty(PropertyName.LevelEditorBattle, Variant.From(in LevelEditorBattle));
		info.AddProperty(PropertyName.LevelEditorQuiz, Variant.From(in LevelEditorQuiz));
		info.AddProperty(PropertyName.LevelInformationButton, Variant.From(in LevelInformationButton));
		info.AddProperty(PropertyName.LevelEventButton, Variant.From(in LevelEventButton));
		info.AddProperty(PropertyName.LevelMapButton, Variant.From(in LevelMapButton));
		info.AddProperty(PropertyName.LevelSeedbankButton, Variant.From(in LevelSeedbankButton));
		info.AddProperty(PropertyName.LevelWaveButton, Variant.From(in LevelWaveButton));
		info.AddProperty(PropertyName.MyLevelContainer, Variant.From(in MyLevelContainer));
		info.AddProperty(PropertyName.LevelEditorOnlineLevel, Variant.From(in LevelEditorOnlineLevel));
		info.AddProperty(PropertyName.LevelConfig, Variant.From(in LevelConfig));
		info.AddProperty(PropertyName.CurrentUid, Variant.From(in CurrentUid));
		info.AddProperty(PropertyName.LevelUid, Variant.CreateFrom(LevelUid));
		info.AddProperty(PropertyName.Skip, Variant.From(in Skip));
		info.AddProperty(PropertyName.Over, Variant.From(in Over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.LevelEditorStageTop, out var value))
		{
			LevelEditorStageTop = value.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorStageLeft, out var value2))
		{
			LevelEditorStageLeft = value2.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorStageRight, out var value3))
		{
			LevelEditorStageRight = value3.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorChooseLayer, out var value4))
		{
			LevelEditorChooseLayer = value4.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorMyLevelLayer, out var value5))
		{
			LevelEditorMyLevelLayer = value5.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorOnlineLevelLayer, out var value6))
		{
			LevelEditorOnlineLevelLayer = value6.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorLayer, out var value7))
		{
			LevelEditorLayer = value7.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorBattleLayer, out var value8))
		{
			LevelEditorBattleLayer = value8.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorQuizLayer, out var value9))
		{
			LevelEditorQuizLayer = value9.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorInformationEditor, out var value10))
		{
			LevelEditorInformationEditor = value10.As<LevelEditorInformationEditor>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorEventEditor, out var value11))
		{
			LevelEditorEventEditor = value11.As<LevelEditorEventEditor>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorMapEditor, out var value12))
		{
			LevelEditorMapEditor = value12.As<LevelEditorMapEditor>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorSeedbankEditor, out var value13))
		{
			LevelEditorSeedbankEditor = value13.As<LevelEditorSeedbankEditor>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorWaveEditor, out var value14))
		{
			LevelEditorWaveEditor = value14.As<LevelEditorWaveEditor>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorBattle, out var value15))
		{
			LevelEditorBattle = value15.As<LevelEditorBattle>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorQuiz, out var value16))
		{
			LevelEditorQuiz = value16.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.LevelInformationButton, out var value17))
		{
			LevelInformationButton = value17.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.LevelEventButton, out var value18))
		{
			LevelEventButton = value18.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.LevelMapButton, out var value19))
		{
			LevelMapButton = value19.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.LevelSeedbankButton, out var value20))
		{
			LevelSeedbankButton = value20.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.LevelWaveButton, out var value21))
		{
			LevelWaveButton = value21.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.MyLevelContainer, out var value22))
		{
			MyLevelContainer = value22.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName.LevelEditorOnlineLevel, out var value23))
		{
			LevelEditorOnlineLevel = value23.As<LevelEditorOnlineLevel>();
		}
		if (info.TryGetProperty(PropertyName.LevelConfig, out var value24))
		{
			LevelConfig = value24.As<TowerDefenseLevelConfig>();
		}
		if (info.TryGetProperty(PropertyName.CurrentUid, out var value25))
		{
			CurrentUid = value25.As<string>();
		}
		if (info.TryGetProperty(PropertyName.LevelUid, out var value26))
		{
			LevelUid = value26.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.Skip, out var value27))
		{
			Skip = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Over, out var value28))
		{
			Over = value28.As<bool>();
		}
	}
}
