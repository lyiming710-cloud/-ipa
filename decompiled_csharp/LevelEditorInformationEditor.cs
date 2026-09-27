using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/InformationEditor/LevelEditorInformationEditor.cs")]
public class LevelEditorInformationEditor : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName SetMapTexture = "SetMapTexture";

		public static readonly StringName FreshFinishMethod = "FreshFinishMethod";

		public static readonly StringName FindOptionButtonId = "FindOptionButtonId";

		public static readonly StringName LevelNumberSpinBoxValueChanged = "LevelNumberSpinBoxValueChanged";

		public static readonly StringName LevelNameLineEditTextChanged = "LevelNameLineEditTextChanged";

		public static readonly StringName LevelDescriptionLineEditTextChanged = "LevelDescriptionLineEditTextChanged";

		public static readonly StringName HomeWorldOptionButtonItemSelected = "HomeWorldOptionButtonItemSelected";

		public static readonly StringName MapOptionButtonItemSelected = "MapOptionButtonItemSelected";

		public static readonly StringName BgmOptionButtonItemSelected = "BgmOptionButtonItemSelected";

		public static readonly StringName FinishMethodOptionButtonItemSelected = "FinishMethodOptionButtonItemSelected";

		public static readonly StringName TalkOptionButtonItemSelected = "TalkOptionButtonItemSelected";

		public static readonly StringName TutorialOptionButtonItemSelected = "TutorialOptionButtonItemSelected";

		public static readonly StringName TalkCheckBoxToggled = "TalkCheckBoxToggled";

		public static readonly StringName TutorialCheckBoxToggled = "TutorialCheckBoxToggled";

		public static readonly StringName MowerUseCheckBoxToggled = "MowerUseCheckBoxToggled";

		public static readonly StringName StormOpenCheckBoxToggled = "StormOpenCheckBoxToggled";

		public static readonly StringName FogUseCheckBoxToggled = "FogUseCheckBoxToggled";

		public static readonly StringName FogBeginColumnSpinBoxValueChanged = "FogBeginColumnSpinBoxValueChanged";

		public static readonly StringName VaseShuffleCheckBoxToggled = "VaseShuffleCheckBoxToggled";

		public static readonly StringName IZMShuffleCheckBoxToggled = "IZMShuffleCheckBoxToggled";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName levelConfig = "levelConfig";

		public static readonly StringName levelNumberSpinBox = "levelNumberSpinBox";

		public static readonly StringName levelNameLineEdit = "levelNameLineEdit";

		public static readonly StringName levelDescriptionTextEdit = "levelDescriptionTextEdit";

		public static readonly StringName homeWorldOptionButton = "homeWorldOptionButton";

		public static readonly StringName mapOptionButton = "mapOptionButton";

		public static readonly StringName bgmOptionButton = "bgmOptionButton";

		public static readonly StringName talkOptionButton = "talkOptionButton";

		public static readonly StringName talkCheckBox = "talkCheckBox";

		public static readonly StringName tutorialOptionButton = "tutorialOptionButton";

		public static readonly StringName tutorialCheckBox = "tutorialCheckBox";

		public static readonly StringName finishMethodOptionButton = "finishMethodOptionButton";

		public static readonly StringName mowerUseCheckBox = "mowerUseCheckBox";

		public static readonly StringName vaseShuffleCheckBox = "vaseShuffleCheckBox";

		public static readonly StringName iZMShuffleCheckBox = "iZMShuffleCheckBox";

		public static readonly StringName fogUseContainer = "fogUseContainer";

		public static readonly StringName fogbeginColumnContainer = "fogbeginColumnContainer";

		public static readonly StringName fogUseCheckBox = "fogUseCheckBox";

		public static readonly StringName fogbeginColumnSpinBox = "fogbeginColumnSpinBox";

		public static readonly StringName stormOpenContainer = "stormOpenContainer";

		public static readonly StringName stormOpenCheckBox = "stormOpenCheckBox";

		public static readonly StringName mapTexture = "mapTexture";

		public static readonly StringName vaseShuffleContainer = "vaseShuffleContainer";

		public static readonly StringName iZMShuffleContainer = "iZMShuffleContainer";

		public static readonly StringName homeWorldTranslate = "homeWorldTranslate";

		public static readonly StringName homeWorldDictionary = "homeWorldDictionary";

		public static readonly StringName finishMethodTranslate = "finishMethodTranslate";

		public static readonly StringName finishMethodDictionary = "finishMethodDictionary";

		public static readonly StringName mapDictionary = "mapDictionary";

		public static readonly StringName bgmDictionary = "bgmDictionary";

		public static readonly StringName talkDictionary = "talkDictionary";

		public static readonly StringName tutorialDictionary = "tutorialDictionary";

		public static readonly StringName graveStoneDictionary = "graveStoneDictionary";

		public static readonly StringName isInit = "isInit";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private SpinBox levelNumberSpinBox;

	private LineEdit levelNameLineEdit;

	private TextEdit levelDescriptionTextEdit;

	private OptionButton homeWorldOptionButton;

	private OptionButton mapOptionButton;

	private OptionButton bgmOptionButton;

	private OptionButton talkOptionButton;

	private CheckBox talkCheckBox;

	private OptionButton tutorialOptionButton;

	private CheckBox tutorialCheckBox;

	private OptionButton finishMethodOptionButton;

	private CheckBox mowerUseCheckBox;

	private CheckBox vaseShuffleCheckBox;

	private CheckBox iZMShuffleCheckBox;

	private HBoxContainer fogUseContainer;

	private HBoxContainer fogbeginColumnContainer;

	private CheckBox fogUseCheckBox;

	private SpinBox fogbeginColumnSpinBox;

	private HBoxContainer stormOpenContainer;

	private CheckBox stormOpenCheckBox;

	private TextureRect mapTexture;

	private HBoxContainer vaseShuffleContainer;

	private HBoxContainer iZMShuffleContainer;

	public static LevelEditorInformationEditor Instance;

	public Dictionary homeWorldTranslate = new Dictionary
	{
		[0] = "无",
		[1] = "现代"
	};

	public Dictionary homeWorldDictionary = new Dictionary
	{
		["现代"] = 1,
		["无"] = 0
	};

	public Dictionary finishMethodTranslate = new Dictionary
	{
		[0] = "波模式",
		[1] = "罐子模式",
		[2] = "我是僵尸模式"
	};

	public Dictionary finishMethodDictionary = new Dictionary
	{
		["波模式"] = 0,
		["罐子模式"] = 1,
		["我是僵尸模式"] = 2
	};

	public Dictionary mapDictionary = new Dictionary();

	public Dictionary bgmDictionary = new Dictionary();

	public Dictionary talkDictionary = new Dictionary();

	public Dictionary tutorialDictionary = new Dictionary();

	public Dictionary graveStoneDictionary = new Dictionary();

	public bool isInit;

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelConfig levelConfig { get; set; }

	public override void _Ready()
	{
		levelNumberSpinBox = GetNode<SpinBox>("%LevelNumberSpinBox");
		levelNameLineEdit = GetNode<LineEdit>("%LevelNameLineEdit");
		levelDescriptionTextEdit = GetNode<TextEdit>("%LevelDescriptionTextEdit");
		homeWorldOptionButton = GetNode<OptionButton>("%HomeWorldOptionButton");
		mapOptionButton = GetNode<OptionButton>("%MapOptionButton");
		bgmOptionButton = GetNode<OptionButton>("%BgmOptionButton");
		talkOptionButton = GetNode<OptionButton>("%TalkOptionButton");
		talkCheckBox = GetNode<CheckBox>("%TalkCheckBox");
		tutorialOptionButton = GetNode<OptionButton>("%TutorialOptionButton");
		tutorialCheckBox = GetNode<CheckBox>("%TutorialCheckBox");
		finishMethodOptionButton = GetNode<OptionButton>("%FinishMethodOptionButton");
		mowerUseCheckBox = GetNode<CheckBox>("%MowerUseCheckBox");
		vaseShuffleCheckBox = GetNode<CheckBox>("%VaseShuffleCheckBox");
		iZMShuffleCheckBox = GetNode<CheckBox>("%IZMShuffleCheckBox");
		fogUseContainer = GetNode<HBoxContainer>("%FogUseContainer");
		fogbeginColumnContainer = GetNode<HBoxContainer>("%FogBeginColumnContainer");
		fogUseCheckBox = GetNode<CheckBox>("%FogUseCheckBox");
		fogbeginColumnSpinBox = GetNode<SpinBox>("%FogBeginColumnSpinBox");
		stormOpenContainer = GetNode<HBoxContainer>("%StormOpenContainer");
		stormOpenCheckBox = GetNode<CheckBox>("%StormOpenCheckBox");
		mapTexture = GetNode<TextureRect>("%MapTexture");
		vaseShuffleContainer = GetNode<HBoxContainer>("%VaseShuffleContainer");
		iZMShuffleContainer = GetNode<HBoxContainer>("%IZMShuffleContainer");
		LevelEditorDropdown.Configure(homeWorldOptionButton);
		LevelEditorDropdown.Configure(mapOptionButton);
		LevelEditorDropdown.Configure(bgmOptionButton);
		LevelEditorDropdown.Configure(talkOptionButton);
		LevelEditorDropdown.Configure(tutorialOptionButton);
		LevelEditorDropdown.Configure(finishMethodOptionButton);
		levelNumberSpinBox.ValueChanged += LevelNumberSpinBoxValueChanged;
		levelNameLineEdit.TextChanged += LevelNameLineEditTextChanged;
		levelDescriptionTextEdit.TextChanged += LevelDescriptionLineEditTextChanged;
		homeWorldOptionButton.ItemSelected += HomeWorldOptionButtonItemSelected;
		mapOptionButton.ItemSelected += MapOptionButtonItemSelected;
		bgmOptionButton.ItemSelected += BgmOptionButtonItemSelected;
		talkOptionButton.ItemSelected += TalkOptionButtonItemSelected;
		talkCheckBox.Toggled += TalkCheckBoxToggled;
		tutorialOptionButton.ItemSelected += TutorialOptionButtonItemSelected;
		tutorialCheckBox.Toggled += TutorialCheckBoxToggled;
		finishMethodOptionButton.ItemSelected += FinishMethodOptionButtonItemSelected;
		stormOpenCheckBox.Toggled += StormOpenCheckBoxToggled;
		fogUseCheckBox.Toggled += FogUseCheckBoxToggled;
		fogbeginColumnSpinBox.ValueChanged += FogBeginColumnSpinBoxValueChanged;
		mowerUseCheckBox.Toggled += MowerUseCheckBoxToggled;
		vaseShuffleCheckBox.Toggled += VaseShuffleCheckBoxToggled;
		iZMShuffleCheckBox.Toggled += IZMShuffleCheckBoxToggled;
		Instance = this;
		GeneralEnum.HOMEWORLD[] values = Enum.GetValues<GeneralEnum.HOMEWORLD>();
		foreach (GeneralEnum.HOMEWORLD hOMEWORLD in values)
		{
			homeWorldOptionButton.AddItem(homeWorldTranslate[(int)hOMEWORLD].AsString());
		}
		foreach (Variant key in finishMethodTranslate.Keys)
		{
			finishMethodOptionButton.AddItem(finishMethodTranslate[key].AsString());
		}
		foreach (string key2 in ResourceManager.Instance.MAPS.Keys)
		{
			TowerDefenseMapConfig towerDefenseMapConfig = (TowerDefenseMapConfig)ResourceManager.Instance.MAPS[key2];
			mapOptionButton.AddItem(towerDefenseMapConfig.translate);
			mapDictionary[towerDefenseMapConfig.translate] = key2;
		}
		ResourceManager.Instance.EnsureAllBgmsLoaded();
		foreach (string key3 in ResourceManager.Instance.BGMS.Keys)
		{
			TowerDefenseBackgroundMusicConfig towerDefenseBackgroundMusicConfig = (TowerDefenseBackgroundMusicConfig)ResourceManager.Instance.BGMS[key3];
			bgmOptionButton.AddItem(towerDefenseBackgroundMusicConfig.translate);
			bgmDictionary[towerDefenseBackgroundMusicConfig.translate] = key3;
		}
		foreach (string key4 in ResourceManager.Instance.TALKS.Keys)
		{
			talkOptionButton.AddItem(key4);
		}
		foreach (string key5 in ResourceManager.Instance.TUTORIALS.Keys)
		{
			tutorialOptionButton.AddItem(key5);
		}
	}

	public void Init(TowerDefenseLevelConfig _levelConfig)
	{
		isInit = true;
		levelConfig = _levelConfig;
		levelNumberSpinBox.Value = levelConfig.levelNumber;
		levelNameLineEdit.Text = levelConfig.levelName;
		levelDescriptionTextEdit.Text = levelConfig.description;
		stormOpenCheckBox.ButtonPressed = levelConfig.stormOpen;
		mowerUseCheckBox.ButtonPressed = levelConfig.mowerUse;
		homeWorldOptionButton.Selected = FindOptionButtonId(homeWorldOptionButton, homeWorldTranslate[(int)levelConfig.homeWorld].AsString());
		finishMethodOptionButton.Selected = FindOptionButtonId(finishMethodOptionButton, finishMethodTranslate[(int)levelConfig.finishMethod].AsString());
		FinishMethodOptionButtonItemSelected(finishMethodOptionButton.Selected);
		TowerDefenseMapConfig towerDefenseMapConfig = (TowerDefenseMapConfig)ResourceManager.Instance.MAPS[levelConfig.map];
		mapOptionButton.Selected = FindOptionButtonId(mapOptionButton, towerDefenseMapConfig.translate);
		MapOptionButtonItemSelected(mapOptionButton.Selected);
		TowerDefenseBackgroundMusicConfig towerDefenseBackgroundMusicConfig = (TowerDefenseBackgroundMusicConfig)ResourceManager.Instance.BGMS[levelConfig.backgroundMusic];
		bgmOptionButton.Selected = FindOptionButtonId(bgmOptionButton, towerDefenseBackgroundMusicConfig.translate);
		talkOptionButton.Selected = FindOptionButtonId(talkOptionButton, levelConfig.talk.AsString());
		tutorialOptionButton.Selected = FindOptionButtonId(tutorialOptionButton, levelConfig.tutorial.AsString());
		talkCheckBox.ButtonPressed = levelConfig.isCustomTalk;
		tutorialCheckBox.ButtonPressed = levelConfig.isCustomTutorial;
		if (!GodotObject.IsInstanceValid(levelConfig.fogManager))
		{
			levelConfig.fogManager = new TowerDefenseLevelFogManagerConfig();
		}
		fogUseCheckBox.ButtonPressed = levelConfig.fogManager.open;
		fogbeginColumnSpinBox.Value = levelConfig.fogManager.beginColumn;
		switch (levelConfig.finishMethod)
		{
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE:
			if (!GodotObject.IsInstanceValid(levelConfig.vaseManager))
			{
				levelConfig.vaseManager = new TowerDefenseLevelVaseManagerConfig();
			}
			vaseShuffleCheckBox.ButtonPressed = levelConfig.vaseManager.shuffle;
			break;
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM:
			if (!GodotObject.IsInstanceValid(levelConfig.izmManager))
			{
				levelConfig.izmManager = new TowerDefenseLevelIZMManagerConfig();
			}
			iZMShuffleCheckBox.ButtonPressed = levelConfig.izmManager.shuffle;
			break;
		}
		isInit = false;
	}

	public void Clear()
	{
		levelConfig = null;
	}

	public void SetMapTexture(Texture2D texture)
	{
		TowerDefenseMapConfig.ApplyMapPreviewTexture(mapTexture, texture);
	}

	public void FreshFinishMethod()
	{
		vaseShuffleContainer.Visible = false;
		iZMShuffleContainer.Visible = false;
		switch (levelConfig.finishMethod)
		{
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE:
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE:
			LevelEditorSeedbankEditor.Instance.SetIZMMode(open: false);
			levelConfig.packetBank = "GeneralPlant";
			break;
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM:
			iZMShuffleContainer.Visible = true;
			if (!GodotObject.IsInstanceValid(levelConfig.izmManager))
			{
				levelConfig.izmManager = new TowerDefenseLevelIZMManagerConfig();
			}
			iZMShuffleCheckBox.ButtonPressed = levelConfig.izmManager.shuffle;
			LevelEditorSeedbankEditor.Instance.SetIZMMode(open: true);
			levelConfig.packetBank = "GeneralZombie";
			break;
		}
		switch (levelConfig.finishMethod)
		{
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE:
		{
			vaseShuffleContainer.Visible = true;
			if (!GodotObject.IsInstanceValid(levelConfig.vaseManager))
			{
				levelConfig.vaseManager = new TowerDefenseLevelVaseManagerConfig();
			}
			vaseShuffleCheckBox.ButtonPressed = levelConfig.vaseManager.shuffle;
			Array<TowerDefenseLevelPreSpawnConfig> array = new Array<TowerDefenseLevelPreSpawnConfig>();
			foreach (TowerDefenseLevelPreSpawnConfig preSpawn in levelConfig.preSpawnList)
			{
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(preSpawn.packetName);
				if (packetConfig.characterConfig is TowerDefenseVaseConfig)
				{
					TowerDefenseLevelVaseConfig towerDefenseLevelVaseConfig = new TowerDefenseLevelVaseConfig();
					towerDefenseLevelVaseConfig.gridPos = preSpawn.gridPos;
					if (GodotObject.IsInstanceValid(preSpawn.characterOverride) && preSpawn.characterOverride.propertyChange.Count > 0)
					{
						foreach (TowerDefenseCharacterPropertyChangeConfig item in preSpawn.characterOverride.propertyChange)
						{
							if (item.propertyName == "packetName")
							{
								towerDefenseLevelVaseConfig.packetName = item.value.AsString();
								break;
							}
						}
					}
					string type = packetConfig.saveKey;
					if (!(type == "VasePlant"))
					{
						if (type == "VaseZombie")
						{
							towerDefenseLevelVaseConfig.type = "Zombie";
						}
						else
						{
							towerDefenseLevelVaseConfig.type = "Normal";
						}
					}
					else
					{
						towerDefenseLevelVaseConfig.type = "Plant";
					}
					levelConfig.vaseManager.vaseList.Add(towerDefenseLevelVaseConfig);
				}
				else
				{
					array.Add(preSpawn);
				}
			}
			levelConfig.preSpawnList = array;
			break;
		}
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE:
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM:
			switch (levelConfig.finishMethod)
			{
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE:
				levelConfig.packetBank = "GeneralPlant";
				break;
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM:
				levelConfig.packetBank = "GeneralZombie";
				break;
			}
			if (!GodotObject.IsInstanceValid(levelConfig.vaseManager))
			{
				break;
			}
			foreach (TowerDefenseLevelVaseConfig vase in levelConfig.vaseManager.vaseList)
			{
				TowerDefenseLevelPreSpawnConfig towerDefenseLevelPreSpawnConfig = new TowerDefenseLevelPreSpawnConfig();
				towerDefenseLevelPreSpawnConfig.gridPos = vase.gridPos;
				string type = vase.type;
				if (!(type == "Plant"))
				{
					if (type == "Zombie")
					{
						towerDefenseLevelPreSpawnConfig.packetName = "VaseZombie";
					}
					else
					{
						towerDefenseLevelPreSpawnConfig.packetName = "VaseNormal";
					}
				}
				else
				{
					towerDefenseLevelPreSpawnConfig.packetName = "VasePlant";
				}
				if (vase.packetName != "")
				{
					towerDefenseLevelPreSpawnConfig.characterOverride = new TowerDefenseCharacterOverride();
					TowerDefenseCharacterPropertyChangeConfig towerDefenseCharacterPropertyChangeConfig = new TowerDefenseCharacterPropertyChangeConfig();
					towerDefenseCharacterPropertyChangeConfig.propertyName = "packetName";
					towerDefenseCharacterPropertyChangeConfig.value = vase.packetName;
					towerDefenseLevelPreSpawnConfig.characterOverride.propertyChange.Add(towerDefenseCharacterPropertyChangeConfig);
				}
				levelConfig.preSpawnList.Add(towerDefenseLevelPreSpawnConfig);
			}
			break;
		}
		switch (levelConfig.finishMethod)
		{
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE:
			levelConfig.izmManager = null;
			break;
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM:
			levelConfig.vaseManager = null;
			break;
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE:
			levelConfig.izmManager = null;
			levelConfig.vaseManager = null;
			break;
		}
	}

	public int FindOptionButtonId(OptionButton optionButton, string key)
	{
		for (int i = 0; i < optionButton.ItemCount; i++)
		{
			if (optionButton.GetItemText(i) == key)
			{
				return optionButton.GetItemId(i);
			}
		}
		return -1;
	}

	public void LevelNumberSpinBoxValueChanged(double value)
	{
		levelConfig.levelNumber = (int)value;
	}

	public void LevelNameLineEditTextChanged(string newText)
	{
		levelConfig.levelName = newText;
	}

	public void LevelDescriptionLineEditTextChanged()
	{
		levelConfig.description = levelDescriptionTextEdit.Text;
	}

	public void HomeWorldOptionButtonItemSelected(long index)
	{
		string itemText = homeWorldOptionButton.GetItemText((int)index);
		levelConfig.homeWorld = (GeneralEnum.HOMEWORLD)homeWorldDictionary[itemText].AsInt32();
	}

	public void MapOptionButtonItemSelected(long index)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		string itemText = mapOptionButton.GetItemText((int)index);
		TowerDefenseMapConfig towerDefenseMapConfig = (TowerDefenseMapConfig)ResourceManager.Instance.MAPS[(string)mapDictionary[itemText]];
		Texture2D instance = null;
		if (GodotObject.IsInstanceValid(LevelEditorMapEditor.instance) && GodotObject.IsInstanceValid(LevelEditorMapEditor.instance.mapFeature))
		{
			TowerDefenseBattleFeatureMap mapFeature = LevelEditorMapEditor.instance.mapFeature;
			if (mapFeature.MapInit(towerDefenseMapConfig) && GodotObject.IsInstanceValid(mapFeature.mapControl) && GodotObject.IsInstanceValid(mapFeature.mapControl.editorSprite) && GodotObject.IsInstanceValid(mapFeature.mapControl.editorSprite.Texture))
			{
				LevelEditorMapEditor.instance.ApplyMapPreviewConfig(towerDefenseMapConfig);
				instance = mapFeature.mapControl.editorSprite.Texture;
			}
		}
		LevelEditorWaveEditor.Instance.MapChange(towerDefenseMapConfig);
		if (!GodotObject.IsInstanceValid(instance))
		{
			instance = towerDefenseMapConfig.GetMapTexture(cache: false);
		}
		SetMapTexture(instance);
		levelConfig.SetMapFromEditor(mapDictionary[itemText].AsString());
		fogbeginColumnSpinBox.MaxValue = towerDefenseMapConfig.gridNum.X;
	}

	public void BgmOptionButtonItemSelected(long index)
	{
		string itemText = bgmOptionButton.GetItemText((int)index);
		levelConfig.backgroundMusic = bgmDictionary[itemText].AsString();
	}

	public void FinishMethodOptionButtonItemSelected(long index)
	{
		string itemText = finishMethodOptionButton.GetItemText((int)index);
		levelConfig.SetFinishMethodFromEditor((TowerDefenseEnum.LEVEL_FINISH_METHOD)finishMethodDictionary[itemText].AsInt32());
		FreshFinishMethod();
	}

	public void TalkOptionButtonItemSelected(long index)
	{
		string itemText = talkOptionButton.GetItemText((int)index);
		levelConfig.talk = itemText;
	}

	public void TutorialOptionButtonItemSelected(long index)
	{
		string itemText = tutorialOptionButton.GetItemText((int)index);
		levelConfig.tutorial = itemText;
	}

	public void TalkCheckBoxToggled(bool toggledOn)
	{
		talkOptionButton.Disabled = toggledOn;
		levelConfig.isCustomTalk = toggledOn;
		if (toggledOn)
		{
			talkOptionButton.Selected = -1;
			levelConfig.talk = "";
		}
	}

	public void TutorialCheckBoxToggled(bool toggledOn)
	{
		tutorialOptionButton.Disabled = toggledOn;
		tutorialOptionButton.Selected = -1;
		if (toggledOn)
		{
			levelConfig.isCustomTutorial = toggledOn;
			levelConfig.tutorial = "";
		}
	}

	public void MowerUseCheckBoxToggled(bool toggledOn)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		levelConfig.mowerUse = toggledOn;
	}

	public void StormOpenCheckBoxToggled(bool toggledOn)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		levelConfig.stormOpen = toggledOn;
	}

	public void FogUseCheckBoxToggled(bool toggledOn)
	{
		if (!GodotObject.IsInstanceValid(levelConfig.fogManager))
		{
			levelConfig.fogManager = new TowerDefenseLevelFogManagerConfig();
		}
		levelConfig.fogManager.open = toggledOn;
		fogbeginColumnContainer.Visible = toggledOn;
	}

	public void FogBeginColumnSpinBoxValueChanged(double value)
	{
		if (!GodotObject.IsInstanceValid(levelConfig.fogManager))
		{
			levelConfig.fogManager = new TowerDefenseLevelFogManagerConfig();
		}
		levelConfig.fogManager.beginColumn = (int)value;
	}

	public void VaseShuffleCheckBoxToggled(bool toggledOn)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		if (GodotObject.IsInstanceValid(levelConfig.vaseManager))
		{
			levelConfig.vaseManager.shuffle = toggledOn;
		}
	}

	public void IZMShuffleCheckBoxToggled(bool toggledOn)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		if (GodotObject.IsInstanceValid(levelConfig.izmManager))
		{
			levelConfig.izmManager.shuffle = toggledOn;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(23)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_levelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetMapTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FreshFinishMethod, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindOptionButtonId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "optionButton", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelNumberSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelNameLineEditTextChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelDescriptionLineEditTextChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HomeWorldOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MapOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BgmOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishMethodOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TalkOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TutorialOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TalkCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TutorialCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MowerUseCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StormOpenCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FogUseCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FogBeginColumnSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.VaseShuffleCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IZMShuffleCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.SetMapTexture && args.Count == 1)
		{
			SetMapTexture(VariantUtils.ConvertTo<Texture2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreshFinishMethod && args.Count == 0)
		{
			FreshFinishMethod();
			ret = default;
			return true;
		}
		if (method == MethodName.FindOptionButtonId && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindOptionButtonId(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.LevelNumberSpinBoxValueChanged && args.Count == 1)
		{
			LevelNumberSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelNameLineEditTextChanged && args.Count == 1)
		{
			LevelNameLineEditTextChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelDescriptionLineEditTextChanged && args.Count == 0)
		{
			LevelDescriptionLineEditTextChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.HomeWorldOptionButtonItemSelected && args.Count == 1)
		{
			HomeWorldOptionButtonItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MapOptionButtonItemSelected && args.Count == 1)
		{
			MapOptionButtonItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BgmOptionButtonItemSelected && args.Count == 1)
		{
			BgmOptionButtonItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishMethodOptionButtonItemSelected && args.Count == 1)
		{
			FinishMethodOptionButtonItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TalkOptionButtonItemSelected && args.Count == 1)
		{
			TalkOptionButtonItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TutorialOptionButtonItemSelected && args.Count == 1)
		{
			TutorialOptionButtonItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TalkCheckBoxToggled && args.Count == 1)
		{
			TalkCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TutorialCheckBoxToggled && args.Count == 1)
		{
			TutorialCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MowerUseCheckBoxToggled && args.Count == 1)
		{
			MowerUseCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StormOpenCheckBoxToggled && args.Count == 1)
		{
			StormOpenCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FogUseCheckBoxToggled && args.Count == 1)
		{
			FogUseCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FogBeginColumnSpinBoxValueChanged && args.Count == 1)
		{
			FogBeginColumnSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.VaseShuffleCheckBoxToggled && args.Count == 1)
		{
			VaseShuffleCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IZMShuffleCheckBoxToggled && args.Count == 1)
		{
			IZMShuffleCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.SetMapTexture)
		{
			return true;
		}
		if (method == MethodName.FreshFinishMethod)
		{
			return true;
		}
		if (method == MethodName.FindOptionButtonId)
		{
			return true;
		}
		if (method == MethodName.LevelNumberSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.LevelNameLineEditTextChanged)
		{
			return true;
		}
		if (method == MethodName.LevelDescriptionLineEditTextChanged)
		{
			return true;
		}
		if (method == MethodName.HomeWorldOptionButtonItemSelected)
		{
			return true;
		}
		if (method == MethodName.MapOptionButtonItemSelected)
		{
			return true;
		}
		if (method == MethodName.BgmOptionButtonItemSelected)
		{
			return true;
		}
		if (method == MethodName.FinishMethodOptionButtonItemSelected)
		{
			return true;
		}
		if (method == MethodName.TalkOptionButtonItemSelected)
		{
			return true;
		}
		if (method == MethodName.TutorialOptionButtonItemSelected)
		{
			return true;
		}
		if (method == MethodName.TalkCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.TutorialCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.MowerUseCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.StormOpenCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.FogUseCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.FogBeginColumnSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.VaseShuffleCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.IZMShuffleCheckBoxToggled)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.levelConfig)
		{
			levelConfig = VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in value);
			return true;
		}
		if (name == PropertyName.levelNumberSpinBox)
		{
			levelNumberSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName.levelNameLineEdit)
		{
			levelNameLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName.levelDescriptionTextEdit)
		{
			levelDescriptionTextEdit = VariantUtils.ConvertTo<TextEdit>(in value);
			return true;
		}
		if (name == PropertyName.homeWorldOptionButton)
		{
			homeWorldOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName.mapOptionButton)
		{
			mapOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName.bgmOptionButton)
		{
			bgmOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName.talkOptionButton)
		{
			talkOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName.talkCheckBox)
		{
			talkCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.tutorialOptionButton)
		{
			tutorialOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName.tutorialCheckBox)
		{
			tutorialCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.finishMethodOptionButton)
		{
			finishMethodOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName.mowerUseCheckBox)
		{
			mowerUseCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.vaseShuffleCheckBox)
		{
			vaseShuffleCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.iZMShuffleCheckBox)
		{
			iZMShuffleCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.fogUseContainer)
		{
			fogUseContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.fogbeginColumnContainer)
		{
			fogbeginColumnContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.fogUseCheckBox)
		{
			fogUseCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.fogbeginColumnSpinBox)
		{
			fogbeginColumnSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName.stormOpenContainer)
		{
			stormOpenContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.stormOpenCheckBox)
		{
			stormOpenCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.mapTexture)
		{
			mapTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.vaseShuffleContainer)
		{
			vaseShuffleContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.iZMShuffleContainer)
		{
			iZMShuffleContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.homeWorldTranslate)
		{
			homeWorldTranslate = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.homeWorldDictionary)
		{
			homeWorldDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.finishMethodTranslate)
		{
			finishMethodTranslate = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.finishMethodDictionary)
		{
			finishMethodDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.mapDictionary)
		{
			mapDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.bgmDictionary)
		{
			bgmDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.talkDictionary)
		{
			talkDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.tutorialDictionary)
		{
			tutorialDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.graveStoneDictionary)
		{
			graveStoneDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.isInit)
		{
			isInit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.levelConfig)
		{
			value = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(levelConfig);
			return true;
		}
		if (name == PropertyName.levelNumberSpinBox)
		{
			value = VariantUtils.CreateFrom(in levelNumberSpinBox);
			return true;
		}
		if (name == PropertyName.levelNameLineEdit)
		{
			value = VariantUtils.CreateFrom(in levelNameLineEdit);
			return true;
		}
		if (name == PropertyName.levelDescriptionTextEdit)
		{
			value = VariantUtils.CreateFrom(in levelDescriptionTextEdit);
			return true;
		}
		if (name == PropertyName.homeWorldOptionButton)
		{
			value = VariantUtils.CreateFrom(in homeWorldOptionButton);
			return true;
		}
		if (name == PropertyName.mapOptionButton)
		{
			value = VariantUtils.CreateFrom(in mapOptionButton);
			return true;
		}
		if (name == PropertyName.bgmOptionButton)
		{
			value = VariantUtils.CreateFrom(in bgmOptionButton);
			return true;
		}
		if (name == PropertyName.talkOptionButton)
		{
			value = VariantUtils.CreateFrom(in talkOptionButton);
			return true;
		}
		if (name == PropertyName.talkCheckBox)
		{
			value = VariantUtils.CreateFrom(in talkCheckBox);
			return true;
		}
		if (name == PropertyName.tutorialOptionButton)
		{
			value = VariantUtils.CreateFrom(in tutorialOptionButton);
			return true;
		}
		if (name == PropertyName.tutorialCheckBox)
		{
			value = VariantUtils.CreateFrom(in tutorialCheckBox);
			return true;
		}
		if (name == PropertyName.finishMethodOptionButton)
		{
			value = VariantUtils.CreateFrom(in finishMethodOptionButton);
			return true;
		}
		if (name == PropertyName.mowerUseCheckBox)
		{
			value = VariantUtils.CreateFrom(in mowerUseCheckBox);
			return true;
		}
		if (name == PropertyName.vaseShuffleCheckBox)
		{
			value = VariantUtils.CreateFrom(in vaseShuffleCheckBox);
			return true;
		}
		if (name == PropertyName.iZMShuffleCheckBox)
		{
			value = VariantUtils.CreateFrom(in iZMShuffleCheckBox);
			return true;
		}
		if (name == PropertyName.fogUseContainer)
		{
			value = VariantUtils.CreateFrom(in fogUseContainer);
			return true;
		}
		if (name == PropertyName.fogbeginColumnContainer)
		{
			value = VariantUtils.CreateFrom(in fogbeginColumnContainer);
			return true;
		}
		if (name == PropertyName.fogUseCheckBox)
		{
			value = VariantUtils.CreateFrom(in fogUseCheckBox);
			return true;
		}
		if (name == PropertyName.fogbeginColumnSpinBox)
		{
			value = VariantUtils.CreateFrom(in fogbeginColumnSpinBox);
			return true;
		}
		if (name == PropertyName.stormOpenContainer)
		{
			value = VariantUtils.CreateFrom(in stormOpenContainer);
			return true;
		}
		if (name == PropertyName.stormOpenCheckBox)
		{
			value = VariantUtils.CreateFrom(in stormOpenCheckBox);
			return true;
		}
		if (name == PropertyName.mapTexture)
		{
			value = VariantUtils.CreateFrom(in mapTexture);
			return true;
		}
		if (name == PropertyName.vaseShuffleContainer)
		{
			value = VariantUtils.CreateFrom(in vaseShuffleContainer);
			return true;
		}
		if (name == PropertyName.iZMShuffleContainer)
		{
			value = VariantUtils.CreateFrom(in iZMShuffleContainer);
			return true;
		}
		if (name == PropertyName.homeWorldTranslate)
		{
			value = VariantUtils.CreateFrom(in homeWorldTranslate);
			return true;
		}
		if (name == PropertyName.homeWorldDictionary)
		{
			value = VariantUtils.CreateFrom(in homeWorldDictionary);
			return true;
		}
		if (name == PropertyName.finishMethodTranslate)
		{
			value = VariantUtils.CreateFrom(in finishMethodTranslate);
			return true;
		}
		if (name == PropertyName.finishMethodDictionary)
		{
			value = VariantUtils.CreateFrom(in finishMethodDictionary);
			return true;
		}
		if (name == PropertyName.mapDictionary)
		{
			value = VariantUtils.CreateFrom(in mapDictionary);
			return true;
		}
		if (name == PropertyName.bgmDictionary)
		{
			value = VariantUtils.CreateFrom(in bgmDictionary);
			return true;
		}
		if (name == PropertyName.talkDictionary)
		{
			value = VariantUtils.CreateFrom(in talkDictionary);
			return true;
		}
		if (name == PropertyName.tutorialDictionary)
		{
			value = VariantUtils.CreateFrom(in tutorialDictionary);
			return true;
		}
		if (name == PropertyName.graveStoneDictionary)
		{
			value = VariantUtils.CreateFrom(in graveStoneDictionary);
			return true;
		}
		if (name == PropertyName.isInit)
		{
			value = VariantUtils.CreateFrom(in isInit);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.levelNumberSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelNameLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelDescriptionTextEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.homeWorldOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.bgmOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.talkOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.talkCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.tutorialOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.tutorialCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.finishMethodOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mowerUseCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.vaseShuffleCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.iZMShuffleCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.fogUseContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.fogbeginColumnContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.fogUseCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.fogbeginColumnSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.stormOpenContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.stormOpenCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.vaseShuffleContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.iZMShuffleContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelConfig, PropertyHint.ResourceType, "TowerDefenseLevelConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.homeWorldTranslate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.homeWorldDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.finishMethodTranslate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.finishMethodDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.mapDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.bgmDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.talkDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.tutorialDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.graveStoneDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isInit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.levelConfig, Variant.From<TowerDefenseLevelConfig>(levelConfig));
		info.AddProperty(PropertyName.levelNumberSpinBox, Variant.From(in levelNumberSpinBox));
		info.AddProperty(PropertyName.levelNameLineEdit, Variant.From(in levelNameLineEdit));
		info.AddProperty(PropertyName.levelDescriptionTextEdit, Variant.From(in levelDescriptionTextEdit));
		info.AddProperty(PropertyName.homeWorldOptionButton, Variant.From(in homeWorldOptionButton));
		info.AddProperty(PropertyName.mapOptionButton, Variant.From(in mapOptionButton));
		info.AddProperty(PropertyName.bgmOptionButton, Variant.From(in bgmOptionButton));
		info.AddProperty(PropertyName.talkOptionButton, Variant.From(in talkOptionButton));
		info.AddProperty(PropertyName.talkCheckBox, Variant.From(in talkCheckBox));
		info.AddProperty(PropertyName.tutorialOptionButton, Variant.From(in tutorialOptionButton));
		info.AddProperty(PropertyName.tutorialCheckBox, Variant.From(in tutorialCheckBox));
		info.AddProperty(PropertyName.finishMethodOptionButton, Variant.From(in finishMethodOptionButton));
		info.AddProperty(PropertyName.mowerUseCheckBox, Variant.From(in mowerUseCheckBox));
		info.AddProperty(PropertyName.vaseShuffleCheckBox, Variant.From(in vaseShuffleCheckBox));
		info.AddProperty(PropertyName.iZMShuffleCheckBox, Variant.From(in iZMShuffleCheckBox));
		info.AddProperty(PropertyName.fogUseContainer, Variant.From(in fogUseContainer));
		info.AddProperty(PropertyName.fogbeginColumnContainer, Variant.From(in fogbeginColumnContainer));
		info.AddProperty(PropertyName.fogUseCheckBox, Variant.From(in fogUseCheckBox));
		info.AddProperty(PropertyName.fogbeginColumnSpinBox, Variant.From(in fogbeginColumnSpinBox));
		info.AddProperty(PropertyName.stormOpenContainer, Variant.From(in stormOpenContainer));
		info.AddProperty(PropertyName.stormOpenCheckBox, Variant.From(in stormOpenCheckBox));
		info.AddProperty(PropertyName.mapTexture, Variant.From(in mapTexture));
		info.AddProperty(PropertyName.vaseShuffleContainer, Variant.From(in vaseShuffleContainer));
		info.AddProperty(PropertyName.iZMShuffleContainer, Variant.From(in iZMShuffleContainer));
		info.AddProperty(PropertyName.homeWorldTranslate, Variant.From(in homeWorldTranslate));
		info.AddProperty(PropertyName.homeWorldDictionary, Variant.From(in homeWorldDictionary));
		info.AddProperty(PropertyName.finishMethodTranslate, Variant.From(in finishMethodTranslate));
		info.AddProperty(PropertyName.finishMethodDictionary, Variant.From(in finishMethodDictionary));
		info.AddProperty(PropertyName.mapDictionary, Variant.From(in mapDictionary));
		info.AddProperty(PropertyName.bgmDictionary, Variant.From(in bgmDictionary));
		info.AddProperty(PropertyName.talkDictionary, Variant.From(in talkDictionary));
		info.AddProperty(PropertyName.tutorialDictionary, Variant.From(in tutorialDictionary));
		info.AddProperty(PropertyName.graveStoneDictionary, Variant.From(in graveStoneDictionary));
		info.AddProperty(PropertyName.isInit, Variant.From(in isInit));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.levelConfig, out var value))
		{
			levelConfig = value.As<TowerDefenseLevelConfig>();
		}
		if (info.TryGetProperty(PropertyName.levelNumberSpinBox, out var value2))
		{
			levelNumberSpinBox = value2.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName.levelNameLineEdit, out var value3))
		{
			levelNameLineEdit = value3.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName.levelDescriptionTextEdit, out var value4))
		{
			levelDescriptionTextEdit = value4.As<TextEdit>();
		}
		if (info.TryGetProperty(PropertyName.homeWorldOptionButton, out var value5))
		{
			homeWorldOptionButton = value5.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName.mapOptionButton, out var value6))
		{
			mapOptionButton = value6.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName.bgmOptionButton, out var value7))
		{
			bgmOptionButton = value7.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName.talkOptionButton, out var value8))
		{
			talkOptionButton = value8.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName.talkCheckBox, out var value9))
		{
			talkCheckBox = value9.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.tutorialOptionButton, out var value10))
		{
			tutorialOptionButton = value10.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName.tutorialCheckBox, out var value11))
		{
			tutorialCheckBox = value11.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.finishMethodOptionButton, out var value12))
		{
			finishMethodOptionButton = value12.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName.mowerUseCheckBox, out var value13))
		{
			mowerUseCheckBox = value13.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.vaseShuffleCheckBox, out var value14))
		{
			vaseShuffleCheckBox = value14.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.iZMShuffleCheckBox, out var value15))
		{
			iZMShuffleCheckBox = value15.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.fogUseContainer, out var value16))
		{
			fogUseContainer = value16.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.fogbeginColumnContainer, out var value17))
		{
			fogbeginColumnContainer = value17.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.fogUseCheckBox, out var value18))
		{
			fogUseCheckBox = value18.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.fogbeginColumnSpinBox, out var value19))
		{
			fogbeginColumnSpinBox = value19.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName.stormOpenContainer, out var value20))
		{
			stormOpenContainer = value20.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.stormOpenCheckBox, out var value21))
		{
			stormOpenCheckBox = value21.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.mapTexture, out var value22))
		{
			mapTexture = value22.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.vaseShuffleContainer, out var value23))
		{
			vaseShuffleContainer = value23.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.iZMShuffleContainer, out var value24))
		{
			iZMShuffleContainer = value24.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.homeWorldTranslate, out var value25))
		{
			homeWorldTranslate = value25.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.homeWorldDictionary, out var value26))
		{
			homeWorldDictionary = value26.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.finishMethodTranslate, out var value27))
		{
			finishMethodTranslate = value27.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.finishMethodDictionary, out var value28))
		{
			finishMethodDictionary = value28.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.mapDictionary, out var value29))
		{
			mapDictionary = value29.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.bgmDictionary, out var value30))
		{
			bgmDictionary = value30.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.talkDictionary, out var value31))
		{
			talkDictionary = value31.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.tutorialDictionary, out var value32))
		{
			tutorialDictionary = value32.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.graveStoneDictionary, out var value33))
		{
			graveStoneDictionary = value33.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.isInit, out var value34))
		{
			isInit = value34.As<bool>();
		}
	}
}
