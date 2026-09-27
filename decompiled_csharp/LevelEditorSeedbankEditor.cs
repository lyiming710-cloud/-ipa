using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/SeedbankEditor/LevelEditorSeedbankEditor.cs")]
public class LevelEditorSeedbankEditor : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName SetIZMMode = "SetIZMMode";

		public static readonly StringName OnChildPacketBankVisibilityChanged = "OnChildPacketBankVisibilityChanged";

		public static readonly StringName Save = "Save";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName FindOptionButtonId = "FindOptionButtonId";

		public static readonly StringName MethodOptionButtonItemSelected = "MethodOptionButtonItemSelected";

		public static readonly StringName SunUseCheckBoxToggled = "SunUseCheckBoxToggled";

		public static readonly StringName SunSpinBoxValueChanged = "SunSpinBoxValueChanged";

		public static readonly StringName PacketColdDownStartUseCheckBoxPressed = "PacketColdDownStartUseCheckBoxPressed";

		public static readonly StringName PacketColdDownUseCheckBoxPressed = "PacketColdDownUseCheckBoxPressed";

		public static readonly StringName SunSpawnIntervalSpinBoxValueChanged = "SunSpawnIntervalSpinBoxValueChanged";

		public static readonly StringName SunSpawnNumSpinBoxValueChanged = "SunSpawnNumSpinBoxValueChanged";

		public static readonly StringName RainModeIntervalSpinBoxValueChanged = "RainModeIntervalSpinBoxValueChanged";

		public static readonly StringName RainModeAliveTimeSpinBoxValueChanged = "RainModeAliveTimeSpinBoxValueChanged";

		public static readonly StringName PlantColumnCheckBoxToggled = "PlantColumnCheckBoxToggled";

		public static readonly StringName ConveyorTypeOptionButtonItemSelected = "ConveyorTypeOptionButtonItemSelected";

		public static readonly StringName RainModeTypeOptionButtonItemSelected = "RainModeTypeOptionButtonItemSelected";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _plantColumnCheckBox = "_plantColumnCheckBox";

		public static readonly StringName _sunContainer = "_sunContainer";

		public static readonly StringName _sunManagerContainer = "_sunManagerContainer";

		public static readonly StringName _conveyorContainer = "_conveyorContainer";

		public static readonly StringName _packetColdDownStartContainer = "_packetColdDownStartContainer";

		public static readonly StringName _packetColdDownUseContainer = "_packetColdDownUseContainer";

		public static readonly StringName _rainModeContainer = "_rainModeContainer";

		public static readonly StringName _methodOptionButton = "_methodOptionButton";

		public static readonly StringName _sunSpinBox = "_sunSpinBox";

		public static readonly StringName _sunUseCheckBox = "_sunUseCheckBox";

		public static readonly StringName _sunSpawnIntervalSpinBox = "_sunSpawnIntervalSpinBox";

		public static readonly StringName _sunSpawnNumSpinBox = "_sunSpawnNumSpinBox";

		public static readonly StringName _conveyorIntervalSpinBox = "_conveyorIntervalSpinBox";

		public static readonly StringName _conveyorTypeOptionButton = "_conveyorTypeOptionButton";

		public static readonly StringName _packetColdDownStartUseCheckBox = "_packetColdDownStartUseCheckBox";

		public static readonly StringName _packetColdDownUseCheckBox = "_packetColdDownUseCheckBox";

		public static readonly StringName _rainModeIntervalSpinBox = "_rainModeIntervalSpinBox";

		public static readonly StringName _rainModeAliveTimeSpinBox = "_rainModeAliveTimeSpinBox";

		public static readonly StringName _rainModeTypeOptionButton = "_rainModeTypeOptionButton";

		public static readonly StringName levelConfig = "levelConfig";

		public static readonly StringName isInit = "isInit";

		public static readonly StringName conveyorTypeTranslate = "conveyorTypeTranslate";

		public static readonly StringName conveyorTypeDictionary = "conveyorTypeDictionary";

		public static readonly StringName rainModeTypeTranslate = "rainModeTypeTranslate";

		public static readonly StringName rainModeTypeDictionary = "rainModeTypeDictionary";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private CheckBox _plantColumnCheckBox;

	private VBoxContainer _sunContainer;

	private VBoxContainer _sunManagerContainer;

	private VBoxContainer _conveyorContainer;

	private HBoxContainer _packetColdDownStartContainer;

	private HBoxContainer _packetColdDownUseContainer;

	private VBoxContainer _rainModeContainer;

	private OptionButton _methodOptionButton;

	private SpinBox _sunSpinBox;

	private CheckBox _sunUseCheckBox;

	private SpinBox _sunSpawnIntervalSpinBox;

	private SpinBox _sunSpawnNumSpinBox;

	private SpinBox _conveyorIntervalSpinBox;

	private OptionButton _conveyorTypeOptionButton;

	private CheckBox _packetColdDownStartUseCheckBox;

	private CheckBox _packetColdDownUseCheckBox;

	private SpinBox _rainModeIntervalSpinBox;

	private SpinBox _rainModeAliveTimeSpinBox;

	private OptionButton _rainModeTypeOptionButton;

	public static LevelEditorSeedbankEditor Instance;

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelConfig levelConfig;

	public bool isInit;

	public static readonly Dictionary methodTranslate = new Dictionary
	{
		{ 0, "无" },
		{ 1, "选卡模式" },
		{ 2, "预选卡模式" },
		{ 3, "传送带模式" },
		{ 4, "种子雨模式" }
	};

	public static readonly Dictionary methodDictionary = new Dictionary
	{
		{ "无", 0 },
		{ "选卡模式", 1 },
		{ "预选卡模式", 2 },
		{ "传送带模式", 3 },
		{ "种子雨模式", 4 }
	};

	public Dictionary conveyorTypeTranslate = new Dictionary
	{
		{ "Default", "默认" },
		{ "Sun", "消耗阳光" }
	};

	public Dictionary conveyorTypeDictionary = new Dictionary
	{
		{ "默认", "Default" },
		{ "消耗阳光", "Sun" }
	};

	public Dictionary rainModeTypeTranslate = new Dictionary
	{
		{ "Default", "默认" },
		{ "Sun", "消耗阳光" }
	};

	public Dictionary rainModeTypeDictionary = new Dictionary
	{
		{ "默认", "Default" },
		{ "消耗阳光", "Sun" }
	};

	public override void _Ready()
	{
		_plantColumnCheckBox = GetNode<CheckBox>("%PlantColumnCheckBox");
		_sunContainer = GetNode<VBoxContainer>("%SunContainer");
		_sunManagerContainer = GetNode<VBoxContainer>("%SunManagerContainer");
		_conveyorContainer = GetNode<VBoxContainer>("%ConveyorContainer");
		_packetColdDownStartContainer = GetNode<HBoxContainer>("%PacketColdDownStartContainer");
		_packetColdDownUseContainer = GetNode<HBoxContainer>("%PacketColdDownUseContainer");
		_rainModeContainer = GetNode<VBoxContainer>("%RainModeContainer");
		_methodOptionButton = GetNode<OptionButton>("%MethodOptionButton");
		_sunSpinBox = GetNode<SpinBox>("%SunSpinBox");
		_sunUseCheckBox = GetNode<CheckBox>("%SunUseCheckBox");
		_sunSpawnIntervalSpinBox = GetNode<SpinBox>("%SunSpawnIntervalSpinBox");
		_sunSpawnNumSpinBox = GetNode<SpinBox>("%SunSpawnNumSpinBox");
		_conveyorIntervalSpinBox = GetNode<SpinBox>("%ConveyorIntervalSpinBox");
		_conveyorTypeOptionButton = GetNode<OptionButton>("%ConveyorTypeOptionButton");
		_packetColdDownStartUseCheckBox = GetNode<CheckBox>("%PacketColdDownStartUseCheckBox");
		_packetColdDownUseCheckBox = GetNode<CheckBox>("%PacketColdDownUseCheckBox");
		_rainModeIntervalSpinBox = GetNode<SpinBox>("%RainModeIntervalSpinBox");
		_rainModeAliveTimeSpinBox = GetNode<SpinBox>("%RainModeAliveTimeSpinBox");
		_rainModeTypeOptionButton = GetNode<OptionButton>("%RainModeTypeOptionButton");
		LevelEditorDropdown.Configure(_methodOptionButton);
		LevelEditorDropdown.Configure(_conveyorTypeOptionButton);
		LevelEditorDropdown.Configure(_rainModeTypeOptionButton);
		VisibilityChanged += Save;
		VisibilityChanged += OnChildPacketBankVisibilityChanged;
		_plantColumnCheckBox.Toggled += PlantColumnCheckBoxToggled;
		_methodOptionButton.ItemSelected += (long index) =>
		{
			MethodOptionButtonItemSelected((int)index);
		};
		_packetColdDownUseCheckBox.Pressed += PacketColdDownUseCheckBoxPressed;
		_packetColdDownStartUseCheckBox.Pressed += PacketColdDownStartUseCheckBoxPressed;
		_conveyorIntervalSpinBox.ValueChanged += SunSpawnNumSpinBoxValueChanged;
		_conveyorTypeOptionButton.ItemSelected += (long index) =>
		{
			ConveyorTypeOptionButtonItemSelected((int)index);
		};
		_rainModeIntervalSpinBox.ValueChanged += RainModeIntervalSpinBoxValueChanged;
		_rainModeAliveTimeSpinBox.ValueChanged += RainModeAliveTimeSpinBoxValueChanged;
		_rainModeTypeOptionButton.ItemSelected += (long index) =>
		{
			RainModeTypeOptionButtonItemSelected((int)index);
		};
		_sunSpinBox.ValueChanged += SunSpinBoxValueChanged;
		_sunUseCheckBox.Toggled += SunUseCheckBoxToggled;
		_sunSpawnIntervalSpinBox.ValueChanged += SunSpawnIntervalSpinBoxValueChanged;
		_sunSpawnNumSpinBox.ValueChanged += SunSpawnNumSpinBoxValueChanged;
		Instance = this;
		_methodOptionButton.Clear();
		TowerDefenseEnum.LEVEL_SEEDBANK_METHOD[] values = Enum.GetValues<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>();
		foreach (TowerDefenseEnum.LEVEL_SEEDBANK_METHOD lEVEL_SEEDBANK_METHOD in values)
		{
			_methodOptionButton.AddItem((string)methodTranslate[(int)lEVEL_SEEDBANK_METHOD]);
		}
		_conveyorTypeOptionButton.Clear();
		foreach (Variant key in conveyorTypeTranslate.Keys)
		{
			string text = (string)key;
			_conveyorTypeOptionButton.AddItem((string)conveyorTypeTranslate[text]);
		}
		_rainModeTypeOptionButton.Clear();
		foreach (Variant key2 in rainModeTypeTranslate.Keys)
		{
			string text2 = (string)key2;
			_rainModeTypeOptionButton.AddItem((string)rainModeTypeTranslate[text2]);
		}
	}

	public void Init(TowerDefenseLevelConfig _levelConfig)
	{
		isInit = true;
		Clear();
		levelConfig = _levelConfig;
		_plantColumnCheckBox.ButtonPressed = levelConfig.plantColumn;
		_methodOptionButton.Selected = FindOptionButtonId(_methodOptionButton, (string)methodTranslate[(int)levelConfig.packetBankMethod]);
		MethodOptionButtonItemSelected(_methodOptionButton.Selected);
		if (!GodotObject.IsInstanceValid(levelConfig.conveyorData))
		{
			levelConfig.conveyorData = new TowerDefenseConveyorConfig();
		}
		else
		{
			levelConfig.conveyorData = levelConfig.conveyorData.Duplicate(deep: true) as TowerDefenseConveyorConfig;
		}
		_conveyorIntervalSpinBox.Value = levelConfig.conveyorData.interval;
		_conveyorTypeOptionButton.Selected = FindOptionButtonId(_conveyorTypeOptionButton, (string)conveyorTypeTranslate[levelConfig.conveyorData.type]);
		if (levelConfig.packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR)
		{
			ConveyorTypeOptionButtonItemSelected(_conveyorTypeOptionButton.Selected);
		}
		if (!GodotObject.IsInstanceValid(levelConfig.rainData))
		{
			levelConfig.rainData = new TowerDefenseRainModeConfig();
		}
		else
		{
			levelConfig.rainData = levelConfig.rainData.Duplicate(deep: true) as TowerDefenseRainModeConfig;
		}
		_rainModeIntervalSpinBox.Value = levelConfig.rainData.interval;
		_rainModeAliveTimeSpinBox.Value = levelConfig.rainData.aliveTime;
		_rainModeTypeOptionButton.Selected = FindOptionButtonId(_rainModeTypeOptionButton, (string)rainModeTypeTranslate[levelConfig.rainData.type]);
		if (levelConfig.packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN)
		{
			RainModeTypeOptionButtonItemSelected(_rainModeTypeOptionButton.Selected);
		}
		if (!GodotObject.IsInstanceValid(levelConfig.sunManager))
		{
			levelConfig.sunManager = new TowerDefenseLevelSunManagerConfig();
		}
		else
		{
			levelConfig.sunManager = levelConfig.sunManager.Duplicate(deep: true) as TowerDefenseLevelSunManagerConfig;
		}
		_sunSpinBox.Value = levelConfig.sunManager.begin;
		SunSpinBoxValueChanged(_sunSpinBox.Value);
		_sunUseCheckBox.ButtonPressed = levelConfig.sunManager.open;
		_sunSpawnIntervalSpinBox.Value = levelConfig.sunManager.spawnInterval;
		_sunSpawnNumSpinBox.Value = levelConfig.sunManager.spawnNum;
		_packetColdDownStartUseCheckBox.ButtonPressed = levelConfig.packetColdDownStart;
		_packetColdDownUseCheckBox.ButtonPressed = levelConfig.packetColdDownUse;
		switch (levelConfig.packetBankMethod)
		{
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE:
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET:
			LevelEditorSeedBankChoose.Instance.PacketListChoose(levelConfig.packetBankList);
			break;
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR:
			foreach (TowerDefenseConveyorPacketConfig packet in levelConfig.conveyorData.packetList)
			{
				LevelEditorSeedBankChoose.Instance.PacketNameChoose(packet.name);
			}
			break;
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN:
			foreach (Variant packet2 in levelConfig.rainData.packetList)
			{
				TowerDefenseRainModePacketConfig towerDefenseRainModePacketConfig = (TowerDefenseRainModePacketConfig)(GodotObject)packet2;
				LevelEditorSeedBankChoose.Instance.PacketNameChoose(towerDefenseRainModePacketConfig.name);
			}
			break;
		}
		isInit = false;
	}

	public void SetIZMMode(bool open)
	{
		if (open)
		{
			LevelEditorSeedbank.Instance.seedBankTexture.Texture = LevelEditorSeedbank.SEED_BANK_ZOMBIE;
		}
		else
		{
			LevelEditorSeedbank.Instance.seedBankTexture.Texture = LevelEditorSeedbank.SEED_BANK;
		}
	}

	public void OnChildPacketBankVisibilityChanged()
	{
		if (GodotObject.IsInstanceValid(LevelEditorSeedBankChoose.Instance))
		{
			LevelEditorSeedBankChoose.Instance.RefreshWhenVisible();
		}
	}

	public void Save()
	{
		if (!GodotObject.IsInstanceValid(levelConfig))
		{
			return;
		}
		levelConfig.plantColumn = _plantColumnCheckBox.ButtonPressed;
		levelConfig.conveyorData.interval = _conveyorIntervalSpinBox.Value;
		levelConfig.rainData.interval = _rainModeIntervalSpinBox.Value;
		levelConfig.rainData.aliveTime = _rainModeAliveTimeSpinBox.Value;
		levelConfig.sunManager.begin = (long)_sunSpinBox.Value;
		levelConfig.sunManager.open = _sunUseCheckBox.ButtonPressed;
		levelConfig.sunManager.spawnInterval = _sunSpawnIntervalSpinBox.Value;
		levelConfig.sunManager.spawnNum = (long)_sunSpawnNumSpinBox.Value;
		levelConfig.packetColdDownStart = _packetColdDownStartUseCheckBox.ButtonPressed;
		levelConfig.packetColdDownUse = _packetColdDownUseCheckBox.ButtonPressed;
		switch (levelConfig.packetBankMethod)
		{
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE:
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET:
			levelConfig.packetBankList.Clear();
			{
				foreach (TowerDefenseInGamePacketShow packet in LevelEditorSeedbank.Instance.packetList)
				{
					TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = new TowerDefenseLevelPacketConfig();
					towerDefenseLevelPacketConfig.packetName = packet.config.saveKey;
					levelConfig.packetBankList.Add(towerDefenseLevelPacketConfig);
				}
				break;
			}
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR:
			levelConfig.conveyorData.packetList.Clear();
			{
				foreach (TowerDefenseInGamePacketShow packet2 in LevelEditorSeedbank.Instance.packetList)
				{
					TowerDefenseConveyorPacketConfig towerDefenseConveyorPacketConfig = new TowerDefenseConveyorPacketConfig();
					towerDefenseConveyorPacketConfig.name = packet2.config.saveKey;
					levelConfig.conveyorData.packetList.Add(towerDefenseConveyorPacketConfig);
				}
				break;
			}
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN:
			levelConfig.rainData.packetList.Clear();
			{
				foreach (TowerDefenseInGamePacketShow packet3 in LevelEditorSeedbank.Instance.packetList)
				{
					TowerDefenseRainModePacketConfig towerDefenseRainModePacketConfig = new TowerDefenseRainModePacketConfig();
					towerDefenseRainModePacketConfig.name = packet3.config.saveKey;
					levelConfig.rainData.packetList.Add(towerDefenseRainModePacketConfig);
				}
				break;
			}
		}
	}

	public void Clear()
	{
		if (GodotObject.IsInstanceValid(LevelEditorSeedBankChoose.Instance))
		{
			LevelEditorSeedBankChoose.Instance.ClearSelectionAnimations();
		}
		LevelEditorSeedbank.Instance.Clear();
		levelConfig = null;
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

	public void MethodOptionButtonItemSelected(int index)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		string itemText = _methodOptionButton.GetItemText(index);
		levelConfig.SetPacketBankMethodFromEditor((TowerDefenseEnum.LEVEL_SEEDBANK_METHOD)(int)methodDictionary[itemText]);
		LevelEditorSeedbank.Instance.seedContanin.Visible = false;
		LevelEditorSeedbank.Instance.conveyor.Visible = false;
		LevelEditorSeedbank.Instance.packetContainer.Visible = false;
		_packetColdDownStartContainer.Visible = false;
		_packetColdDownUseContainer.Visible = false;
		_sunContainer.Visible = false;
		_conveyorContainer.Visible = false;
		_rainModeContainer.Visible = false;
		switch (levelConfig.packetBankMethod)
		{
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE:
			_packetColdDownStartContainer.Visible = true;
			_packetColdDownUseContainer.Visible = true;
			_sunContainer.Visible = true;
			LevelEditorSeedBankChoose.Instance.Visible = false;
			break;
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE:
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET:
			LevelEditorSeedbank.Instance.seedContanin.Visible = true;
			LevelEditorSeedbank.Instance.packetContainer.Visible = true;
			_packetColdDownStartContainer.Visible = true;
			_packetColdDownUseContainer.Visible = true;
			_sunContainer.Visible = true;
			LevelEditorSeedBankChoose.Instance.Visible = true;
			break;
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR:
			_sunUseCheckBox.ButtonPressed = false;
			LevelEditorSeedbank.Instance.conveyor.Visible = true;
			LevelEditorSeedbank.Instance.packetContainer.Visible = true;
			_conveyorContainer.Visible = true;
			LevelEditorSeedBankChoose.Instance.Visible = true;
			if (!GodotObject.IsInstanceValid(levelConfig.conveyorData))
			{
				levelConfig.conveyorData = new TowerDefenseConveyorConfig();
			}
			_conveyorTypeOptionButton.Selected = FindOptionButtonId(_conveyorTypeOptionButton, (string)conveyorTypeTranslate[levelConfig.conveyorData.type]);
			ConveyorTypeOptionButtonItemSelected(_conveyorTypeOptionButton.Selected);
			break;
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN:
			_sunUseCheckBox.ButtonPressed = false;
			LevelEditorSeedbank.Instance.seedContanin.Visible = true;
			LevelEditorSeedbank.Instance.packetContainer.Visible = true;
			_rainModeContainer.Visible = true;
			LevelEditorSeedBankChoose.Instance.Visible = true;
			if (!GodotObject.IsInstanceValid(levelConfig.rainData))
			{
				levelConfig.rainData = new TowerDefenseRainModeConfig();
			}
			_rainModeTypeOptionButton.Selected = FindOptionButtonId(_rainModeTypeOptionButton, (string)rainModeTypeTranslate[levelConfig.rainData.type]);
			RainModeTypeOptionButtonItemSelected(_rainModeTypeOptionButton.Selected);
			break;
		}
	}

	public void SunUseCheckBoxToggled(bool toggledOn)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		_sunManagerContainer.Visible = toggledOn;
	}

	public void SunSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		LevelEditorSeedbank.Instance.sunLabel.Text = ((int)value).ToString();
		LevelEditorSeedbank.Instance.conveyorSunLabel.Text = ((int)value).ToString();
	}

	public void PacketColdDownStartUseCheckBoxPressed()
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void PacketColdDownUseCheckBoxPressed()
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void SunSpawnIntervalSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void SunSpawnNumSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void RainModeIntervalSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void RainModeAliveTimeSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void PlantColumnCheckBoxToggled(bool toggledOn)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void ConveyorTypeOptionButtonItemSelected(int index)
	{
		string itemText = _conveyorTypeOptionButton.GetItemText(index);
		levelConfig.conveyorData.type = (string)conveyorTypeDictionary[itemText];
		if (levelConfig.conveyorData.type == "Sun")
		{
			LevelEditorSeedbank.Instance.conveyorBeltRectPC.Texture = LevelEditorSeedbank.CONVEYOR_BELT_SUN_BACKDROP;
			LevelEditorSeedbank.Instance.belt.Texture = LevelEditorSeedbank.CONVEYOR_BELT_SUN;
			LevelEditorSeedbank.Instance.conveyorSunBankTexture.Visible = true;
			_sunContainer.Visible = true;
		}
		else
		{
			LevelEditorSeedbank.Instance.conveyorBeltRectPC.Texture = LevelEditorSeedbank.CONVEYOR_BELT_BACKDROP;
			LevelEditorSeedbank.Instance.belt.Texture = LevelEditorSeedbank.CONVEYOR_BELT;
			LevelEditorSeedbank.Instance.conveyorSunBankTexture.Visible = false;
			_sunContainer.Visible = false;
		}
	}

	public void RainModeTypeOptionButtonItemSelected(int index)
	{
		string itemText = _rainModeTypeOptionButton.GetItemText(index);
		levelConfig.rainData.type = (string)rainModeTypeDictionary[itemText];
		_sunContainer.Visible = levelConfig.rainData.type == "Sun";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_levelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetIZMMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnChildPacketBankVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Save, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindOptionButtonId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "optionButton", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MethodOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SunUseCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SunSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PacketColdDownStartUseCheckBoxPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PacketColdDownUseCheckBoxPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SunSpawnIntervalSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SunSpawnNumSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RainModeIntervalSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RainModeAliveTimeSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlantColumnCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConveyorTypeOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RainModeTypeOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetIZMMode && args.Count == 1)
		{
			SetIZMMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnChildPacketBankVisibilityChanged && args.Count == 0)
		{
			OnChildPacketBankVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.Save && args.Count == 0)
		{
			Save();
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.FindOptionButtonId && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindOptionButtonId(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MethodOptionButtonItemSelected && args.Count == 1)
		{
			MethodOptionButtonItemSelected(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SunUseCheckBoxToggled && args.Count == 1)
		{
			SunUseCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SunSpinBoxValueChanged && args.Count == 1)
		{
			SunSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketColdDownStartUseCheckBoxPressed && args.Count == 0)
		{
			PacketColdDownStartUseCheckBoxPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.PacketColdDownUseCheckBoxPressed && args.Count == 0)
		{
			PacketColdDownUseCheckBoxPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SunSpawnIntervalSpinBoxValueChanged && args.Count == 1)
		{
			SunSpawnIntervalSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SunSpawnNumSpinBoxValueChanged && args.Count == 1)
		{
			SunSpawnNumSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RainModeIntervalSpinBoxValueChanged && args.Count == 1)
		{
			RainModeIntervalSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RainModeAliveTimeSpinBoxValueChanged && args.Count == 1)
		{
			RainModeAliveTimeSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlantColumnCheckBoxToggled && args.Count == 1)
		{
			PlantColumnCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConveyorTypeOptionButtonItemSelected && args.Count == 1)
		{
			ConveyorTypeOptionButtonItemSelected(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RainModeTypeOptionButtonItemSelected && args.Count == 1)
		{
			RainModeTypeOptionButtonItemSelected(VariantUtils.ConvertTo<int>(in args[0]));
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
		if (method == MethodName.SetIZMMode)
		{
			return true;
		}
		if (method == MethodName.OnChildPacketBankVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.Save)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.FindOptionButtonId)
		{
			return true;
		}
		if (method == MethodName.MethodOptionButtonItemSelected)
		{
			return true;
		}
		if (method == MethodName.SunUseCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.SunSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.PacketColdDownStartUseCheckBoxPressed)
		{
			return true;
		}
		if (method == MethodName.PacketColdDownUseCheckBoxPressed)
		{
			return true;
		}
		if (method == MethodName.SunSpawnIntervalSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.SunSpawnNumSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.RainModeIntervalSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.RainModeAliveTimeSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.PlantColumnCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.ConveyorTypeOptionButtonItemSelected)
		{
			return true;
		}
		if (method == MethodName.RainModeTypeOptionButtonItemSelected)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._plantColumnCheckBox)
		{
			_plantColumnCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._sunContainer)
		{
			_sunContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._sunManagerContainer)
		{
			_sunManagerContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._conveyorContainer)
		{
			_conveyorContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetColdDownStartContainer)
		{
			_packetColdDownStartContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetColdDownUseContainer)
		{
			_packetColdDownUseContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._rainModeContainer)
		{
			_rainModeContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._methodOptionButton)
		{
			_methodOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._sunSpinBox)
		{
			_sunSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._sunUseCheckBox)
		{
			_sunUseCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._sunSpawnIntervalSpinBox)
		{
			_sunSpawnIntervalSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._sunSpawnNumSpinBox)
		{
			_sunSpawnNumSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._conveyorIntervalSpinBox)
		{
			_conveyorIntervalSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._conveyorTypeOptionButton)
		{
			_conveyorTypeOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._packetColdDownStartUseCheckBox)
		{
			_packetColdDownStartUseCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._packetColdDownUseCheckBox)
		{
			_packetColdDownUseCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._rainModeIntervalSpinBox)
		{
			_rainModeIntervalSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._rainModeAliveTimeSpinBox)
		{
			_rainModeAliveTimeSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._rainModeTypeOptionButton)
		{
			_rainModeTypeOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName.levelConfig)
		{
			levelConfig = VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in value);
			return true;
		}
		if (name == PropertyName.isInit)
		{
			isInit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.conveyorTypeTranslate)
		{
			conveyorTypeTranslate = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.conveyorTypeDictionary)
		{
			conveyorTypeDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.rainModeTypeTranslate)
		{
			rainModeTypeTranslate = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.rainModeTypeDictionary)
		{
			rainModeTypeDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._plantColumnCheckBox)
		{
			value = VariantUtils.CreateFrom(in _plantColumnCheckBox);
			return true;
		}
		if (name == PropertyName._sunContainer)
		{
			value = VariantUtils.CreateFrom(in _sunContainer);
			return true;
		}
		if (name == PropertyName._sunManagerContainer)
		{
			value = VariantUtils.CreateFrom(in _sunManagerContainer);
			return true;
		}
		if (name == PropertyName._conveyorContainer)
		{
			value = VariantUtils.CreateFrom(in _conveyorContainer);
			return true;
		}
		if (name == PropertyName._packetColdDownStartContainer)
		{
			value = VariantUtils.CreateFrom(in _packetColdDownStartContainer);
			return true;
		}
		if (name == PropertyName._packetColdDownUseContainer)
		{
			value = VariantUtils.CreateFrom(in _packetColdDownUseContainer);
			return true;
		}
		if (name == PropertyName._rainModeContainer)
		{
			value = VariantUtils.CreateFrom(in _rainModeContainer);
			return true;
		}
		if (name == PropertyName._methodOptionButton)
		{
			value = VariantUtils.CreateFrom(in _methodOptionButton);
			return true;
		}
		if (name == PropertyName._sunSpinBox)
		{
			value = VariantUtils.CreateFrom(in _sunSpinBox);
			return true;
		}
		if (name == PropertyName._sunUseCheckBox)
		{
			value = VariantUtils.CreateFrom(in _sunUseCheckBox);
			return true;
		}
		if (name == PropertyName._sunSpawnIntervalSpinBox)
		{
			value = VariantUtils.CreateFrom(in _sunSpawnIntervalSpinBox);
			return true;
		}
		if (name == PropertyName._sunSpawnNumSpinBox)
		{
			value = VariantUtils.CreateFrom(in _sunSpawnNumSpinBox);
			return true;
		}
		if (name == PropertyName._conveyorIntervalSpinBox)
		{
			value = VariantUtils.CreateFrom(in _conveyorIntervalSpinBox);
			return true;
		}
		if (name == PropertyName._conveyorTypeOptionButton)
		{
			value = VariantUtils.CreateFrom(in _conveyorTypeOptionButton);
			return true;
		}
		if (name == PropertyName._packetColdDownStartUseCheckBox)
		{
			value = VariantUtils.CreateFrom(in _packetColdDownStartUseCheckBox);
			return true;
		}
		if (name == PropertyName._packetColdDownUseCheckBox)
		{
			value = VariantUtils.CreateFrom(in _packetColdDownUseCheckBox);
			return true;
		}
		if (name == PropertyName._rainModeIntervalSpinBox)
		{
			value = VariantUtils.CreateFrom(in _rainModeIntervalSpinBox);
			return true;
		}
		if (name == PropertyName._rainModeAliveTimeSpinBox)
		{
			value = VariantUtils.CreateFrom(in _rainModeAliveTimeSpinBox);
			return true;
		}
		if (name == PropertyName._rainModeTypeOptionButton)
		{
			value = VariantUtils.CreateFrom(in _rainModeTypeOptionButton);
			return true;
		}
		if (name == PropertyName.levelConfig)
		{
			value = VariantUtils.CreateFrom(in levelConfig);
			return true;
		}
		if (name == PropertyName.isInit)
		{
			value = VariantUtils.CreateFrom(in isInit);
			return true;
		}
		if (name == PropertyName.conveyorTypeTranslate)
		{
			value = VariantUtils.CreateFrom(in conveyorTypeTranslate);
			return true;
		}
		if (name == PropertyName.conveyorTypeDictionary)
		{
			value = VariantUtils.CreateFrom(in conveyorTypeDictionary);
			return true;
		}
		if (name == PropertyName.rainModeTypeTranslate)
		{
			value = VariantUtils.CreateFrom(in rainModeTypeTranslate);
			return true;
		}
		if (name == PropertyName.rainModeTypeDictionary)
		{
			value = VariantUtils.CreateFrom(in rainModeTypeDictionary);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._plantColumnCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunManagerContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._conveyorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetColdDownStartContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetColdDownUseContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rainModeContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._methodOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunUseCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunSpawnIntervalSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunSpawnNumSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._conveyorIntervalSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._conveyorTypeOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetColdDownStartUseCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetColdDownUseCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rainModeIntervalSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rainModeAliveTimeSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rainModeTypeOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelConfig, PropertyHint.ResourceType, "TowerDefenseLevelConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isInit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.conveyorTypeTranslate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.conveyorTypeDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.rainModeTypeTranslate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.rainModeTypeDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._plantColumnCheckBox, Variant.From(in _plantColumnCheckBox));
		info.AddProperty(PropertyName._sunContainer, Variant.From(in _sunContainer));
		info.AddProperty(PropertyName._sunManagerContainer, Variant.From(in _sunManagerContainer));
		info.AddProperty(PropertyName._conveyorContainer, Variant.From(in _conveyorContainer));
		info.AddProperty(PropertyName._packetColdDownStartContainer, Variant.From(in _packetColdDownStartContainer));
		info.AddProperty(PropertyName._packetColdDownUseContainer, Variant.From(in _packetColdDownUseContainer));
		info.AddProperty(PropertyName._rainModeContainer, Variant.From(in _rainModeContainer));
		info.AddProperty(PropertyName._methodOptionButton, Variant.From(in _methodOptionButton));
		info.AddProperty(PropertyName._sunSpinBox, Variant.From(in _sunSpinBox));
		info.AddProperty(PropertyName._sunUseCheckBox, Variant.From(in _sunUseCheckBox));
		info.AddProperty(PropertyName._sunSpawnIntervalSpinBox, Variant.From(in _sunSpawnIntervalSpinBox));
		info.AddProperty(PropertyName._sunSpawnNumSpinBox, Variant.From(in _sunSpawnNumSpinBox));
		info.AddProperty(PropertyName._conveyorIntervalSpinBox, Variant.From(in _conveyorIntervalSpinBox));
		info.AddProperty(PropertyName._conveyorTypeOptionButton, Variant.From(in _conveyorTypeOptionButton));
		info.AddProperty(PropertyName._packetColdDownStartUseCheckBox, Variant.From(in _packetColdDownStartUseCheckBox));
		info.AddProperty(PropertyName._packetColdDownUseCheckBox, Variant.From(in _packetColdDownUseCheckBox));
		info.AddProperty(PropertyName._rainModeIntervalSpinBox, Variant.From(in _rainModeIntervalSpinBox));
		info.AddProperty(PropertyName._rainModeAliveTimeSpinBox, Variant.From(in _rainModeAliveTimeSpinBox));
		info.AddProperty(PropertyName._rainModeTypeOptionButton, Variant.From(in _rainModeTypeOptionButton));
		info.AddProperty(PropertyName.levelConfig, Variant.From(in levelConfig));
		info.AddProperty(PropertyName.isInit, Variant.From(in isInit));
		info.AddProperty(PropertyName.conveyorTypeTranslate, Variant.From(in conveyorTypeTranslate));
		info.AddProperty(PropertyName.conveyorTypeDictionary, Variant.From(in conveyorTypeDictionary));
		info.AddProperty(PropertyName.rainModeTypeTranslate, Variant.From(in rainModeTypeTranslate));
		info.AddProperty(PropertyName.rainModeTypeDictionary, Variant.From(in rainModeTypeDictionary));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._plantColumnCheckBox, out var value))
		{
			_plantColumnCheckBox = value.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._sunContainer, out var value2))
		{
			_sunContainer = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._sunManagerContainer, out var value3))
		{
			_sunManagerContainer = value3.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._conveyorContainer, out var value4))
		{
			_conveyorContainer = value4.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetColdDownStartContainer, out var value5))
		{
			_packetColdDownStartContainer = value5.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetColdDownUseContainer, out var value6))
		{
			_packetColdDownUseContainer = value6.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._rainModeContainer, out var value7))
		{
			_rainModeContainer = value7.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._methodOptionButton, out var value8))
		{
			_methodOptionButton = value8.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._sunSpinBox, out var value9))
		{
			_sunSpinBox = value9.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._sunUseCheckBox, out var value10))
		{
			_sunUseCheckBox = value10.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._sunSpawnIntervalSpinBox, out var value11))
		{
			_sunSpawnIntervalSpinBox = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._sunSpawnNumSpinBox, out var value12))
		{
			_sunSpawnNumSpinBox = value12.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._conveyorIntervalSpinBox, out var value13))
		{
			_conveyorIntervalSpinBox = value13.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._conveyorTypeOptionButton, out var value14))
		{
			_conveyorTypeOptionButton = value14.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._packetColdDownStartUseCheckBox, out var value15))
		{
			_packetColdDownStartUseCheckBox = value15.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._packetColdDownUseCheckBox, out var value16))
		{
			_packetColdDownUseCheckBox = value16.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._rainModeIntervalSpinBox, out var value17))
		{
			_rainModeIntervalSpinBox = value17.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._rainModeAliveTimeSpinBox, out var value18))
		{
			_rainModeAliveTimeSpinBox = value18.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._rainModeTypeOptionButton, out var value19))
		{
			_rainModeTypeOptionButton = value19.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName.levelConfig, out var value20))
		{
			levelConfig = value20.As<TowerDefenseLevelConfig>();
		}
		if (info.TryGetProperty(PropertyName.isInit, out var value21))
		{
			isInit = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.conveyorTypeTranslate, out var value22))
		{
			conveyorTypeTranslate = value22.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.conveyorTypeDictionary, out var value23))
		{
			conveyorTypeDictionary = value23.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.rainModeTypeTranslate, out var value24))
		{
			rainModeTypeTranslate = value24.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.rainModeTypeDictionary, out var value25))
		{
			rainModeTypeDictionary = value25.As<Dictionary>();
		}
	}
}
