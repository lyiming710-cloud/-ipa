using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/TowerDefenseLevelConfig.cs")]
public class TowerDefenseLevelConfig : TowerDefenseLevelBaseConfig
{
	private static class EnumCache<T> where T : struct, Enum
	{
		private static readonly System.Collections.Generic.Dictionary<string, T> _map = BuildMap();

		private static System.Collections.Generic.Dictionary<string, T> BuildMap()
		{
			System.Collections.Generic.Dictionary<string, T> dictionary = new System.Collections.Generic.Dictionary<string, T>(StringComparer.Ordinal);
			T[] values = Enum.GetValues<T>();
			for (int i = 0; i < values.Length; i++)
			{
				T value = values[i];
				dictionary[value.ToString().ToUpperInvariant()] = value;
			}
			return dictionary;
		}

		public static T Parse(string value, T defaultValue)
		{
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}
			if (!_map.TryGetValue(value.ToUpperInvariant(), out var value2))
			{
				return defaultValue;
			}
			return value2;
		}
	}

	public new class MethodName : TowerDefenseLevelBaseConfig.MethodName
	{
		public static readonly StringName HasProcessName = "HasProcessName";

		public static readonly StringName GetDict = "GetDict";

		public static readonly StringName GetArray = "GetArray";

		public static readonly StringName ExportPacketBankList = "ExportPacketBankList";

		public static readonly StringName SetFinishMethodFromEditor = "SetFinishMethodFromEditor";

		public static readonly StringName SetMapFromEditor = "SetMapFromEditor";

		public static readonly StringName SetPacketBankMethodFromEditor = "SetPacketBankMethodFromEditor";

		public static readonly StringName MarkEventDataEditedFromEditor = "MarkEventDataEditedFromEditor";

		public static readonly StringName MarkPreSpawnDataEditedFromEditor = "MarkPreSpawnDataEditedFromEditor";

		public static readonly StringName PrepareForEditorSave = "PrepareForEditorSave";

		public static readonly StringName RefreshEditorDataSnapshot = "RefreshEditorDataSnapshot";

		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";

		public static readonly StringName Clear = "Clear";

		public new static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName BuildBaseFeatureData = "BuildBaseFeatureData";

		public static readonly StringName BuildPreSpawnFeatureData = "BuildPreSpawnFeatureData";

		public static readonly StringName BuildEventFeatureData = "BuildEventFeatureData";

		public static readonly StringName AppendEventExports = "AppendEventExports";

		public static readonly StringName HasEventEntries = "HasEventEntries";

		public static readonly StringName ApplyEditedFeatureData = "ApplyEditedFeatureData";

		public static readonly StringName ApplyEditedEventFeatureData = "ApplyEditedEventFeatureData";

		public static readonly StringName ApplyEditedPreSpawnFeatureData = "ApplyEditedPreSpawnFeatureData";

		public static readonly StringName MergeLegacyPreSpawnFeatureData = "MergeLegacyPreSpawnFeatureData";

		public static readonly StringName BuildPacketBankFeatureData = "BuildPacketBankFeatureData";

		public static readonly StringName ExportToFeatureProcess = "ExportToFeatureProcess";

		public static readonly StringName HasExplicitSection = "HasExplicitSection";

		public static readonly StringName SynchronizeMapFeature = "SynchronizeMapFeature";

		public static readonly StringName HasSynchronizedMapFeature = "HasSynchronizedMapFeature";

		public static readonly StringName DuplicateFeatureData = "DuplicateFeatureData";

		public static readonly StringName MergePreservedFeatureData = "MergePreservedFeatureData";

		public static readonly StringName MergePreservedProcessData = "MergePreservedProcessData";

		public static readonly StringName ConveyorPreset = "ConveyorPreset";

		public static readonly StringName RainPreset = "RainPreset";
	}

	public new class PropertyName : TowerDefenseLevelBaseConfig.PropertyName
	{
		public static readonly StringName canExport = "canExport";

		public static readonly StringName data = "data";

		public static readonly StringName HasPendingEditorFeatureChanges = "HasPendingEditorFeatureChanges";

		public static readonly StringName packetBankMethod = "packetBankMethod";

		public static readonly StringName _canExport = "_canExport";

		public static readonly StringName _eventDataEditedFromEditor = "_eventDataEditedFromEditor";

		public static readonly StringName _preSpawnDataEditedFromEditor = "_preSpawnDataEditedFromEditor";

		public static readonly StringName _data = "_data";

		public static readonly StringName featureData = "featureData";

		public static readonly StringName processName = "processName";

		public static readonly StringName processData = "processData";

		public static readonly StringName finishMethod = "finishMethod";

		public static readonly StringName baseTimeScale = "baseTimeScale";

		public static readonly StringName mowerUse = "mowerUse";

		public static readonly StringName talk = "talk";

		public static readonly StringName tutorial = "tutorial";

		public static readonly StringName map = "map";

		public static readonly StringName backgroundMusic = "backgroundMusic";

		public static readonly StringName firstRewardType = "firstRewardType";

		public static readonly StringName firstRewardValue = "firstRewardValue";

		public static readonly StringName eventRemotePhaseTimeoutSeconds = "eventRemotePhaseTimeoutSeconds";

		public static readonly StringName eventInit = "eventInit";

		public static readonly StringName eventEntry = "eventEntry";

		public static readonly StringName eventReady = "eventReady";

		public static readonly StringName eventStart = "eventStart";

		public static readonly StringName preSpawnList = "preSpawnList";

		public static readonly StringName limitGridPlantNum = "limitGridPlantNum";

		public static readonly StringName plantColumn = "plantColumn";

		public static readonly StringName packetColdDownStart = "packetColdDownStart";

		public static readonly StringName packetColdDownUse = "packetColdDownUse";

		public static readonly StringName _packetBankMethod = "_packetBankMethod";

		public static readonly StringName stormOpen = "stormOpen";

		public static readonly StringName sunManager = "sunManager";

		public static readonly StringName fogManager = "fogManager";

		public static readonly StringName lookStarManager = "lookStarManager";

		public static readonly StringName waveManager = "waveManager";

		public static readonly StringName vaseManager = "vaseManager";

		public static readonly StringName izmManager = "izmManager";

		public static readonly StringName isCustomTalk = "isCustomTalk";

		public static readonly StringName isCustomTutorial = "isCustomTutorial";

		public static readonly StringName customTalk = "customTalk";

		public static readonly StringName customTutorial = "customTutorial";

		public static readonly StringName packetBank = "packetBank";

		public static readonly StringName packetBankList = "packetBankList";

		public static readonly StringName conveyorData = "conveyorData";

		public static readonly StringName rainData = "rainData";
	}

	public new class SignalName : TowerDefenseLevelBaseConfig.SignalName
	{
	}

	private bool _canExport;

	private bool _eventDataEditedFromEditor;

	private bool _preSpawnDataEditedFromEditor;

	private Json _data;

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<StringName, Dictionary> featureData = new Godot.Collections.Dictionary<StringName, Dictionary>();

	[Export(PropertyHint.None, "")]
	public StringName processName;

	[Export(PropertyHint.None, "")]
	public Dictionary processData = new Dictionary();

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.LEVEL_FINISH_METHOD finishMethod;

	[Export(PropertyHint.None, "")]
	public double baseTimeScale = 1.0;

	[ExportCategory("Mower")]
	[Export(PropertyHint.None, "")]
	public bool mowerUse = true;

	[ExportCategory("Tutorial")]
	[Export(PropertyHint.None, "")]
	public Variant talk = "";

	[Export(PropertyHint.None, "")]
	public Variant tutorial = "";

	[ExportCategory("Map")]
	[Export(PropertyHint.None, "")]
	public string map = "Frontlawn";

	[ExportCategory("BGM")]
	[Export(PropertyHint.None, "")]
	public string backgroundMusic = "Frontlawn";

	[ExportCategory("Reward")]
	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.LEVEL_REWARDTYPE firstRewardType = TowerDefenseEnum.LEVEL_REWARDTYPE.COIN;

	[Export(PropertyHint.None, "")]
	public Variant firstRewardValue = 2000;

	[ExportCategory("Event")]
	[Export(PropertyHint.Range, "0,120,0.1")]
	public double eventRemotePhaseTimeoutSeconds = 10.0;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelEventBase> eventInit = new Array<TowerDefenseLevelEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelEventBase> eventEntry = new Array<TowerDefenseLevelEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelEventBase> eventReady = new Array<TowerDefenseLevelEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelEventBase> eventStart = new Array<TowerDefenseLevelEventBase>();

	[ExportCategory("PreSpawn")]
	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelPreSpawnConfig> preSpawnList = new Array<TowerDefenseLevelPreSpawnConfig>();

	[ExportCategory("PacketBank")]
	[Export(PropertyHint.None, "")]
	public int limitGridPlantNum = -1;

	[Export(PropertyHint.None, "")]
	public bool plantColumn;

	[Export(PropertyHint.None, "")]
	public bool packetColdDownStart = true;

	[Export(PropertyHint.None, "")]
	public bool packetColdDownUse = true;

	private TowerDefenseEnum.LEVEL_SEEDBANK_METHOD _packetBankMethod = TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE;

	[ExportCategory("Effect")]
	[Export(PropertyHint.None, "")]
	public bool stormOpen;

	[ExportCategory("Sun")]
	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelSunManagerConfig sunManager;

	[ExportCategory("Fog")]
	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelFogManagerConfig fogManager;

	[ExportCategory("LookStar")]
	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelLookStarManagerConfig lookStarManager;

	[ExportCategory("Wave")]
	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelWaveManagerConfig waveManager;

	[ExportCategory("Vase")]
	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelVaseManagerConfig vaseManager;

	[ExportCategory("Vase")]
	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelIZMManagerConfig izmManager;

	[ExportCategory("Option")]
	[Export(PropertyHint.None, "")]
	public bool isCustomTalk;

	[Export(PropertyHint.None, "")]
	public bool isCustomTutorial;

	[Export(PropertyHint.None, "")]
	public NpcTalkConfig customTalk;

	[Export(PropertyHint.None, "")]
	public TutorialConfig customTutorial;

	[Export(PropertyHint.None, "")]
	public string packetBank = "GeneralPlant";

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Array packetBankList = new Godot.Collections.Array();

	[Export(PropertyHint.None, "")]
	public TowerDefenseConveyorConfig conveyorData;

	[Export(PropertyHint.None, "")]
	public TowerDefenseRainModeConfig rainData;

	[Export(PropertyHint.None, "")]
	public bool canExport
	{
		get
		{
			return _canExport;
		}
		set
		{
			_canExport = value;
		}
	}

	[Export(PropertyHint.None, "")]
	public Json data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			Init();
		}
	}

	public bool HasPendingEditorFeatureChanges
	{
		get
		{
			if (!_eventDataEditedFromEditor)
			{
				return _preSpawnDataEditedFromEditor;
			}
			return true;
		}
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.LEVEL_SEEDBANK_METHOD packetBankMethod
	{
		get
		{
			return _packetBankMethod;
		}
		set
		{
			_packetBankMethod = value;
			NotifyPropertyListChanged();
		}
	}

	private static bool HasProcessName(StringName value)
	{
		if (value != null)
		{
			return !value.IsEmpty;
		}
		return false;
	}

	private static bool TryGetFinishMethodFromProcessName(string processName, out TowerDefenseEnum.LEVEL_FINISH_METHOD finishMethod)
	{
		switch (processName)
		{
		case "Wave":
			finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE;
			return true;
		case "Vase":
			finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE;
			return true;
		case "IZM":
			finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM;
			return true;
		case "Quiz":
			finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.QUIZ;
			return true;
		case "IZM2":
			finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2;
			return true;
		case "Empty":
			finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.EMPTY;
			return true;
		default:
			finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE;
			return false;
		}
	}

	private static Dictionary GetDict(Dictionary src, string key)
	{
		if (src != null && src.TryGetValue(key, out var value) && value.VariantType == Variant.Type.Dictionary)
		{
			return value.AsGodotDictionary();
		}
		return new Dictionary();
	}

	private static Godot.Collections.Array GetArray(Dictionary src, string key)
	{
		if (src != null && src.TryGetValue(key, out var value) && value.VariantType == Variant.Type.Array)
		{
			return (Godot.Collections.Array)value;
		}
		return new Godot.Collections.Array();
	}

	private Godot.Collections.Array ExportPacketBankList()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (Variant packetBank in packetBankList)
		{
			if (packetBank.VariantType == Variant.Type.Object)
			{
				TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = packetBank.As<TowerDefenseLevelPacketConfig>();
				if (towerDefenseLevelPacketConfig != null)
				{
					array.Add(towerDefenseLevelPacketConfig.Export());
				}
			}
			else if (packetBank.VariantType == Variant.Type.String)
			{
				array.Add(packetBank);
			}
		}
		return array;
	}

	public bool SetFinishMethodFromEditor(TowerDefenseEnum.LEVEL_FINISH_METHOD value)
	{
		if (finishMethod == value)
		{
			return false;
		}
		finishMethod = value;
		if (value != TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE && value != TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2)
		{
			featureData?.Remove("Wave");
		}
		processName = "";
		processData = new Dictionary();
		return true;
	}

	public bool SetMapFromEditor(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		bool result = !string.Equals(map, value, StringComparison.Ordinal) || !HasSynchronizedMapFeature(value);
		map = value;
		SynchronizeMapFeature();
		return result;
	}

	public bool SetPacketBankMethodFromEditor(TowerDefenseEnum.LEVEL_SEEDBANK_METHOD value)
	{
		if (packetBankMethod == value)
		{
			return false;
		}
		packetBankMethod = value;
		featureData?.Remove("RainMode");
		featureData?.Remove("ConveyorBelt");
		featureData?.Remove("SeedBank");
		featureData?.Remove("PacketBank");
		if (featureData != null && featureData.TryGetValue(new StringName("ScreenEffect"), out var value2) && value2 != null)
		{
			value2["PacketBankMethod"] = (int)value;
		}
		if (processData != null && processData.ContainsKey("PacketBankMethod"))
		{
			processData["PacketBankMethod"] = (int)value;
		}
		return true;
	}

	public void MarkEventDataEditedFromEditor()
	{
		_eventDataEditedFromEditor = true;
		ApplyEditedEventFeatureData();
	}

	public void MarkPreSpawnDataEditedFromEditor()
	{
		_preSpawnDataEditedFromEditor = true;
		ApplyEditedPreSpawnFeatureData();
	}

	public void PrepareForEditorSave()
	{
		ApplyEditedFeatureData();
		ExportToFeatureProcess(refreshEditorValues: true);
		SynchronizeMapFeature();
		ApplyEditedFeatureData();
		RefreshEditorDataSnapshot();
		_eventDataEditedFromEditor = false;
		_preSpawnDataEditedFromEditor = false;
	}

	private void RefreshEditorDataSnapshot()
	{
		Dictionary dictionary = Export();
		Json json = new Json();
		if (json.Parse(Json.Stringify(dictionary)) != Error.Ok)
		{
			GD.PushError("[TowerDefenseLevelConfig] Failed to refresh editor JSON snapshot.");
		}
		else
		{
			_data = json;
		}
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 6L;
		Array<Dictionary> array = new Array<Dictionary>();
		if (packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE)
		{
			array.Add(new Dictionary
			{
				["name"] = "PacketBank/Name",
				["type"] = 4,
				["usage"] = num
			});
		}
		return array;
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (property.ToString() == "PacketBank/Name")
		{
			packetBank = value.AsString();
			return true;
		}
		return false;
	}

	public override Variant _Get(StringName property)
	{
		if (property.ToString() == "PacketBank/Name")
		{
			return packetBank;
		}
		return default;
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		if (property.ToString() == "PacketBank/Name")
		{
			return true;
		}
		return false;
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		if (property.ToString() == "PacketBank/Name")
		{
			return "";
		}
		return default;
	}

	public void Clear()
	{
		homeWorld = GeneralEnum.HOMEWORLD.NOONE;
		finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE;
		packetBankMethod = TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE;
		eventInit.Clear();
		eventEntry.Clear();
		eventReady.Clear();
		eventStart.Clear();
		sunManager = null;
		fogManager = null;
		lookStarManager = null;
		waveManager = null;
		vaseManager = null;
		izmManager = null;
		isCustomTalk = false;
		isCustomTutorial = false;
		customTalk = null;
		customTutorial = null;
		packetBankList.Clear();
		conveyorData = null;
		rainData = null;
		plantColumn = false;
		limitGridPlantNum = -1;
		packetColdDownStart = true;
		packetColdDownUse = true;
		featureData.Clear();
		processName = "";
		processData = new Dictionary();
	}

	public override void Init()
	{
		if (!GodotObject.IsInstanceValid(data))
		{
			ExportToFeatureProcess();
			return;
		}
		Clear();
		Dictionary dictionary = data.Data.AsGodotDictionary();
		featureData.Clear();
		foreach (Variant item in GetArray(dictionary, "Feature"))
		{
			Dictionary dictionary2 = item.AsGodotDictionary();
			StringName key = new StringName(dictionary2.GetValueOrDefault("Name", "").AsString());
			Dictionary dict = GetDict(dictionary2, "Data");
			featureData[key] = dict;
		}
		Dictionary dict2 = GetDict(dictionary, "Process");
		if (dict2.Count > 0)
		{
			processName = dict2.GetValueOrDefault("Name", "").AsString();
			processData = GetDict(dict2, "Data");
		}
		name = dictionary.GetValueOrDefault("Name", "").AsString();
		levelName = dictionary.GetValueOrDefault("LevelName", "").AsString();
		description = dictionary.GetValueOrDefault("Description", "").AsString();
		levelNumber = dictionary.GetValueOrDefault("LevelNumber", 0).AsInt32();
		nextLevel = dictionary.GetValueOrDefault("NextLevel", "").AsString();
		homeWorld = EnumCache<GeneralEnum.HOMEWORLD>.Parse(dictionary.GetValueOrDefault("HomeWorld", "NOONE").AsString(), GeneralEnum.HOMEWORLD.NOONE);
		finishMethod = EnumCache<TowerDefenseEnum.LEVEL_FINISH_METHOD>.Parse(dictionary.GetValueOrDefault("FinishMethod", "WAVE").AsString(), TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE);
		if (!dictionary.ContainsKey("FinishMethod") && TryGetFinishMethodFromProcessName(processName.ToString(), out var lEVEL_FINISH_METHOD))
		{
			finishMethod = lEVEL_FINISH_METHOD;
		}
		baseTimeScale = dictionary.GetValueOrDefault("BaseTimeScale", 1.0).AsDouble();
		mowerUse = (processData.TryGetValue("MowerUse", out var value) ? value.AsBool() : dictionary.GetValueOrDefault("MowerUse", true).AsBool());
		if (featureData.TryGetValue(new StringName("NpcTalk"), out var value2) && value2 != null && value2.Count > 0)
		{
			if (value2.ContainsKey("TalkName"))
			{
				isCustomTalk = false;
				talk = value2.GetValueOrDefault("TalkName", "").AsString();
			}
			else
			{
				isCustomTalk = true;
				customTalk = new NpcTalkConfig();
				customTalk.Load(value2);
			}
		}
		else
		{
			Variant valueOrDefault = dictionary.GetValueOrDefault("Talk", "");
			if (valueOrDefault.VariantType == Variant.Type.Dictionary)
			{
				isCustomTalk = true;
				customTalk = new NpcTalkConfig();
				customTalk.Load(valueOrDefault.AsGodotDictionary());
			}
			else
			{
				isCustomTalk = false;
				talk = valueOrDefault;
			}
		}
		if (featureData.TryGetValue(new StringName("Tutorial"), out var value3) && value3 != null && value3.Count > 0)
		{
			if (value3.ContainsKey("TutorialName"))
			{
				isCustomTutorial = false;
				tutorial = value3.GetValueOrDefault("TutorialName", "").AsString();
			}
			else
			{
				isCustomTutorial = true;
				customTutorial = new TutorialConfig();
				customTutorial.Load(value3);
			}
		}
		else
		{
			Variant valueOrDefault2 = dictionary.GetValueOrDefault("Tutorial", "");
			if (valueOrDefault2.VariantType == Variant.Type.Dictionary)
			{
				isCustomTutorial = true;
				customTutorial = new TutorialConfig();
				customTutorial.Load(valueOrDefault2.AsGodotDictionary());
			}
			else
			{
				isCustomTutorial = false;
				tutorial = valueOrDefault2;
			}
		}
		if (featureData.TryGetValue(new StringName("Map"), out var value4) && value4 != null && value4.TryGetValue("MapName", out var value5))
		{
			map = value5.AsString();
		}
		else
		{
			map = dictionary.GetValueOrDefault("Map", map).AsString();
		}
		if (featureData.TryGetValue(new StringName("BGM"), out var value6) && value6 != null && value6.TryGetValue("BackgroundMusic", out var value7))
		{
			backgroundMusic = value7.AsString();
		}
		else
		{
			backgroundMusic = dictionary.GetValueOrDefault("BGM", backgroundMusic).AsString();
		}
		if (featureData.TryGetValue(new StringName("ScreenEffect"), out var value8) && value8 != null && value8.TryGetValue("StormOpen", out var value9))
		{
			stormOpen = value9.AsBool();
		}
		else
		{
			stormOpen = dictionary.GetValueOrDefault("StormOpen", false).AsBool();
		}
		Dictionary dict3 = GetDict(dictionary, "Reward");
		firstRewardType = EnumCache<TowerDefenseEnum.LEVEL_REWARDTYPE>.Parse(dict3.GetValueOrDefault("RewardType", "NOONE").AsString(), TowerDefenseEnum.LEVEL_REWARDTYPE.NOONE);
		firstRewardValue = dict3.GetValueOrDefault("RewardFirst", 2000);
		Dictionary dictionary3 = GetDict(dictionary, "Event");
		if (featureData.TryGetValue(new StringName("Event"), out var value10) && value10 != null && value10.Count > 0)
		{
			dictionary3 = value10;
		}
		eventRemotePhaseTimeoutSeconds = Math.Clamp(dictionary3.GetValueOrDefault("RemotePhaseTimeoutSeconds", 10.0).AsDouble(), 0.0, 120.0);
		foreach (Variant item2 in GetArray(dictionary3, "EventInit"))
		{
			Dictionary dictionary4 = item2.AsGodotDictionary();
			string text = dictionary4.GetValueOrDefault("EventName", "").AsString();
			if (text != "")
			{
				TowerDefenseLevelEventBase towerDefenseLevelEventBase = TowerDefenseLevelEventRegistry.Create(text);
				if (towerDefenseLevelEventBase == null)
				{
					GD.PushError("[TowerDefenseLevelConfig] EventInit 未知的 EventName: " + text);
					continue;
				}
				Dictionary dict4 = GetDict(dictionary4, "Value");
				towerDefenseLevelEventBase.Init(dict4);
				eventInit.Add(towerDefenseLevelEventBase);
			}
		}
		foreach (Variant item3 in GetArray(dictionary3, "EventEntry"))
		{
			Dictionary dictionary5 = item3.AsGodotDictionary();
			string text2 = dictionary5.GetValueOrDefault("EventName", "").AsString();
			if (text2 != "")
			{
				TowerDefenseLevelEventBase towerDefenseLevelEventBase2 = TowerDefenseLevelEventRegistry.Create(text2);
				if (towerDefenseLevelEventBase2 == null)
				{
					GD.PushError("[TowerDefenseLevelConfig] EventEntry 未知的 EventName: " + text2);
					continue;
				}
				Dictionary dict5 = GetDict(dictionary5, "Value");
				towerDefenseLevelEventBase2.Init(dict5);
				eventEntry.Add(towerDefenseLevelEventBase2);
			}
		}
		foreach (Variant item4 in GetArray(dictionary3, "EventReady"))
		{
			Dictionary dictionary6 = item4.AsGodotDictionary();
			string text3 = dictionary6.GetValueOrDefault("EventName", "").AsString();
			if (text3 != "")
			{
				TowerDefenseLevelEventBase towerDefenseLevelEventBase3 = TowerDefenseLevelEventRegistry.Create(text3);
				if (towerDefenseLevelEventBase3 == null)
				{
					GD.PushError("[TowerDefenseLevelConfig] EventReady 未知的 EventName: " + text3);
					continue;
				}
				Dictionary dict6 = GetDict(dictionary6, "Value");
				towerDefenseLevelEventBase3.Init(dict6);
				eventReady.Add(towerDefenseLevelEventBase3);
			}
		}
		foreach (Variant item5 in GetArray(dictionary3, "EventStart"))
		{
			Dictionary dictionary7 = item5.AsGodotDictionary();
			string text4 = dictionary7.GetValueOrDefault("EventName", "").AsString();
			if (text4 != "")
			{
				TowerDefenseLevelEventBase towerDefenseLevelEventBase4 = TowerDefenseLevelEventRegistry.Create(text4);
				if (towerDefenseLevelEventBase4 == null)
				{
					GD.PushError("[TowerDefenseLevelConfig] EventStart 未知的 EventName: " + text4);
					continue;
				}
				Dictionary dict7 = GetDict(dictionary7, "Value");
				towerDefenseLevelEventBase4.Init(dict7);
				eventStart.Add(towerDefenseLevelEventBase4);
			}
		}
		preSpawnList = new Array<TowerDefenseLevelPreSpawnConfig>();
		Dictionary src = GetDict(dictionary, "PreSpawn");
		if (featureData.TryGetValue(new StringName("PreSpawn"), out var value11) && value11 != null && value11.Count > 0)
		{
			src = value11;
		}
		foreach (Variant item6 in GetArray(src, "Packet"))
		{
			if (!GodotObject.IsInstanceValid(item6.AsGodotObject()) && item6.VariantType != Variant.Type.Nil)
			{
				TowerDefenseLevelPreSpawnConfig towerDefenseLevelPreSpawnConfig = new TowerDefenseLevelPreSpawnConfig();
				towerDefenseLevelPreSpawnConfig.Init(item6.AsGodotDictionary());
				preSpawnList.Add(towerDefenseLevelPreSpawnConfig);
			}
		}
		Dictionary dict8 = GetDict(dictionary, "PacketBank");
		Godot.Collections.Array array = new Godot.Collections.Array();
		Dictionary value13;
		Dictionary value14;
		if (featureData.TryGetValue(new StringName("SeedBank"), out var value12) && value12 != null && value12.Count > 0)
		{
			limitGridPlantNum = value12.GetValueOrDefault("LimitGridPlantNum", -1).AsInt32();
			packetColdDownStart = value12.GetValueOrDefault("ColdDownStart", true).AsBool();
			packetColdDownUse = value12.GetValueOrDefault("ColdDownUse", true).AsBool();
			packetBankMethod = EnumCache<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>.Parse(value12.GetValueOrDefault("Method", "NOONE").AsString(), TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE);
			plantColumn = value12.GetValueOrDefault("PlantColumn", false).AsBool();
			array = GetArray(value12, "Packet");
		}
		else if (featureData.TryGetValue(new StringName("ConveyorBelt"), out value13) && value13 != null && value13.Count > 0)
		{
			packetBankMethod = TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR;
			conveyorData = new TowerDefenseConveyorConfig();
			conveyorData.Init(value13);
			plantColumn = value13.GetValueOrDefault("PlantColumn", false).AsBool();
		}
		else if (featureData.TryGetValue(new StringName("RainMode"), out value14) && value14 != null && value14.Count > 0)
		{
			packetBankMethod = TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN;
			rainData = new TowerDefenseRainModeConfig();
			rainData.Init(value14);
			plantColumn = value14.GetValueOrDefault("PlantColumn", false).AsBool();
		}
		else
		{
			limitGridPlantNum = dict8.GetValueOrDefault("LimitGridPlantNum", -1).AsInt32();
			packetColdDownStart = dict8.GetValueOrDefault("ColdDownStart", true).AsBool();
			packetColdDownUse = dict8.GetValueOrDefault("ColdDownUse", true).AsBool();
			packetBankMethod = EnumCache<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>.Parse(dict8.GetValueOrDefault("Method", "NOONE").AsString(), TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE);
			plantColumn = dict8.GetValueOrDefault("PlantColumn", false).AsBool();
			array = GetArray(dict8, "Value");
		}
		switch (packetBankMethod)
		{
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET:
			foreach (Variant item7 in array)
			{
				TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = new TowerDefenseLevelPacketConfig();
				towerDefenseLevelPacketConfig.Init(item7);
				packetBankList.Add(towerDefenseLevelPacketConfig);
			}
			break;
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE:
		{
			foreach (Variant item8 in array)
			{
				TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig3 = new TowerDefenseLevelPacketConfig();
				towerDefenseLevelPacketConfig3.Init(item8);
				packetBankList.Add(towerDefenseLevelPacketConfig3);
			}
			if (featureData.TryGetValue(new StringName("PacketBank"), out var value15) && value15 != null && value15.Count > 0)
			{
				packetBank = value15.GetValueOrDefault("PacketBankName", "").AsString();
			}
			else
			{
				packetBank = dict8.GetValueOrDefault("Type", "").AsString();
			}
			break;
		}
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR:
			if (conveyorData != null)
			{
				break;
			}
			conveyorData = new TowerDefenseConveyorConfig();
			if (array.Count == 1 && array[0].VariantType == Variant.Type.Dictionary && array[0].AsGodotDictionary().ContainsKey("Packet"))
			{
				conveyorData.Init(array[0].AsGodotDictionary());
				break;
			}
			conveyorData.Init(GetDict(dict8, "ConveyorPreset"));
			foreach (Variant item9 in array)
			{
				TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig2 = new TowerDefenseLevelPacketConfig();
				towerDefenseLevelPacketConfig2.Init(item9);
				conveyorData.packetPrioritySpawnList.Add(towerDefenseLevelPacketConfig2);
			}
			break;
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN:
			if (rainData == null)
			{
				rainData = new TowerDefenseRainModeConfig();
				if (array.Count == 1 && array[0].VariantType == Variant.Type.Dictionary && array[0].AsGodotDictionary().ContainsKey("Packet"))
				{
					rainData.Init(array[0].AsGodotDictionary());
				}
				else
				{
					rainData.Init(GetDict(dict8, "RainPreset"));
				}
			}
			break;
		}
		Dictionary sunManagerData = GetDict(dictionary, "SunManager");
		if (featureData.TryGetValue(new StringName("Sun"), out var value16) && value16 != null && value16.Count > 0)
		{
			sunManagerData = value16;
		}
		sunManager = new TowerDefenseLevelSunManagerConfig();
		sunManager.Init(sunManagerData);
		Dictionary fogManagerData = GetDict(dictionary, "FogManager");
		if (featureData.TryGetValue(new StringName("Fog"), out var value17) && value17 != null && value17.Count > 0)
		{
			fogManagerData = value17;
		}
		fogManager = new TowerDefenseLevelFogManagerConfig();
		fogManager.Init(fogManagerData);
		Dictionary lookStarManagerData = GetDict(dictionary, "LookStarManager");
		if (featureData.TryGetValue(new StringName("LookStar"), out var value18) && value18 != null && value18.Count > 0)
		{
			lookStarManagerData = value18;
		}
		lookStarManager = new TowerDefenseLevelLookStarManagerConfig();
		lookStarManager.Init(lookStarManagerData);
		switch (finishMethod)
		{
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE:
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2:
		{
			Dictionary waveManagerData = GetDict(dictionary, "WaveManager");
			if (featureData.TryGetValue(new StringName("Wave"), out var value19) && value19 != null && value19.Count > 0)
			{
				waveManagerData = value19;
			}
			waveManager = new TowerDefenseLevelWaveManagerConfig();
			waveManager.Init(waveManagerData);
			break;
		}
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE:
		{
			Dictionary dict10 = GetDict(dictionary, "VaseManager");
			if (processName == (StringName)"Vase" && processData.Count > 0)
			{
				dict10 = processData;
			}
			vaseManager = new TowerDefenseLevelVaseManagerConfig();
			vaseManager.Init(dict10);
			break;
		}
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM:
		{
			Dictionary dict9 = GetDict(dictionary, "IZMManager");
			if (processName == (StringName)"IZM" && processData.Count > 0)
			{
				dict9 = processData;
			}
			izmManager = new TowerDefenseLevelIZMManagerConfig();
			izmManager.Init(dict9);
			break;
		}
		}
		if (packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR)
		{
			featureData.Remove(new StringName("SeedBank"));
			featureData.Remove(new StringName("PacketBank"));
		}
		if (featureData.Count == 0)
		{
			BuildBaseFeatureData();
			if (isCustomTalk && GodotObject.IsInstanceValid(customTalk))
			{
				Dictionary dictionary8 = GetDict(dictionary, "Talk").Duplicate();
				dictionary8["isCustom"] = true;
				featureData["NpcTalk"] = dictionary8;
			}
			else if (talk.AsString() != "")
			{
				featureData["NpcTalk"] = new Dictionary { ["TalkName"] = talk };
			}
			Dictionary dict11 = GetDict(dictionary, "SunManager");
			if (dict11.Count > 0)
			{
				featureData["Sun"] = dict11;
			}
			BuildPacketBankFeatureData();
			featureData["BGM"] = new Dictionary { ["BackgroundMusic"] = backgroundMusic };
			Dictionary dict12 = GetDict(dictionary, "FogManager");
			if (dict12.Count > 0 && dict12.GetValueOrDefault("Open", false).AsBool())
			{
				featureData["Fog"] = dict12;
			}
			Dictionary dict13 = GetDict(dictionary, "LookStarManager");
			if (dict13.Count > 0 && dict13.GetValueOrDefault("Open", false).AsBool())
			{
				featureData["LookStar"] = dict13;
			}
			featureData["ScreenEffect"] = new Dictionary
			{
				["StormOpen"] = stormOpen,
				["PacketBankMethod"] = (int)packetBankMethod
			};
			if (isCustomTutorial && GodotObject.IsInstanceValid(customTutorial))
			{
				Dictionary dictionary9 = GetDict(dictionary, "Tutorial").Duplicate();
				dictionary9["isCustom"] = true;
				featureData["Tutorial"] = dictionary9;
			}
			else if (tutorial.AsString() != "")
			{
				featureData["Tutorial"] = new Dictionary { ["TutorialName"] = tutorial };
			}
			Dictionary dict14 = GetDict(dictionary, "Event");
			if (dict14.Count > 0)
			{
				featureData["Event"] = dict14;
			}
			if (finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE || finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2)
			{
				featureData["Wave"] = GetDict(dictionary, "WaveManager");
			}
		}
		MergeLegacyPreSpawnFeatureData(dictionary);
		if (!HasProcessName(processName))
		{
			switch (finishMethod)
			{
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE:
				processName = "Wave";
				processData["StormOpen"] = stormOpen;
				processData["MowerUse"] = mowerUse;
				break;
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2:
				processName = "IZM2";
				processData["StormOpen"] = stormOpen;
				processData["MowerUse"] = mowerUse;
				break;
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE:
				processName = "Vase";
				processData = GetDict(dictionary, "VaseManager").Duplicate();
				processData["PacketBankMethod"] = (int)packetBankMethod;
				processData["MowerUse"] = mowerUse;
				break;
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM:
				processName = "IZM";
				processData = GetDict(dictionary, "IZMManager").Duplicate();
				break;
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.QUIZ:
				processName = "Quiz";
				break;
			}
		}
	}

	public Dictionary Export()
	{
		ExportToFeatureProcess();
		Dictionary dictionary = new Dictionary
		{
			["Name"] = name,
			["LevelName"] = levelName,
			["Description"] = description,
			["LevelNumber"] = levelNumber,
			["NextLevel"] = nextLevel,
			["HomeWorld"] = Enum.GetName(typeof(GeneralEnum.HOMEWORLD), homeWorld),
			["Version"] = "1.0",
			["Reward"] = new Dictionary
			{
				["RewardType"] = Enum.GetName(typeof(TowerDefenseEnum.LEVEL_REWARDTYPE), firstRewardType),
				["RewardFirst"] = firstRewardValue
			},
			["BaseTimeScale"] = baseTimeScale,
			["Feature"] = new Godot.Collections.Array(),
			["Process"] = new Dictionary()
		};
		foreach (StringName key14 in featureData.Keys)
		{
			if (!(key14 == (StringName)""))
			{
				((Godot.Collections.Array)dictionary["Feature"]).Add(new Dictionary
				{
					["Name"] = key14,
					["Data"] = featureData[key14]?.Duplicate(deep: true) ?? new Dictionary()
				});
			}
		}
		if (HasProcessName(processName))
		{
			dictionary["Process"] = new Dictionary
			{
				["Name"] = processName,
				["Data"] = processData?.Duplicate(deep: true) ?? new Dictionary()
			};
		}
		return dictionary;
	}

	private void BuildBaseFeatureData()
	{
		featureData["Camera"] = new Dictionary();
		featureData["Map"] = new Dictionary { ["MapName"] = map };
		featureData["Shovel"] = new Dictionary();
		featureData["PacketPick"] = new Dictionary();
		featureData["Mower"] = new Dictionary();
		featureData["Brain"] = new Dictionary();
		featureData["Progress"] = new Dictionary();
	}

	private Dictionary BuildPreSpawnFeatureData()
	{
		if (preSpawnList == null || preSpawnList.Count == 0)
		{
			return null;
		}
		Dictionary dictionary = new Dictionary { ["Packet"] = new Godot.Collections.Array() };
		foreach (TowerDefenseLevelPreSpawnConfig preSpawn in preSpawnList)
		{
			if (GodotObject.IsInstanceValid(preSpawn))
			{
				((Godot.Collections.Array)dictionary["Packet"]).Add(preSpawn.Export());
			}
		}
		return dictionary;
	}

	private Dictionary BuildEventFeatureData()
	{
		Dictionary dictionary = new Dictionary
		{
			["RemotePhaseTimeoutSeconds"] = eventRemotePhaseTimeoutSeconds,
			["EventInit"] = new Godot.Collections.Array(),
			["EventEntry"] = new Godot.Collections.Array(),
			["EventReady"] = new Godot.Collections.Array(),
			["EventStart"] = new Godot.Collections.Array()
		};
		AppendEventExports(dictionary, "EventInit", eventInit);
		AppendEventExports(dictionary, "EventEntry", eventEntry);
		AppendEventExports(dictionary, "EventReady", eventReady);
		AppendEventExports(dictionary, "EventStart", eventStart);
		return dictionary;
	}

	private static void AppendEventExports(Dictionary target, string key, Array<TowerDefenseLevelEventBase> events)
	{
		if (events == null)
		{
			return;
		}
		foreach (TowerDefenseLevelEventBase @event in events)
		{
			if (GodotObject.IsInstanceValid(@event))
			{
				((Godot.Collections.Array)target[key]).Add(@event.Export());
			}
		}
	}

	private static bool HasEventEntries(Dictionary eventFeatureData)
	{
		if (((Godot.Collections.Array)eventFeatureData["EventInit"]).Count <= 0 && ((Godot.Collections.Array)eventFeatureData["EventEntry"]).Count <= 0 && ((Godot.Collections.Array)eventFeatureData["EventReady"]).Count <= 0)
		{
			return ((Godot.Collections.Array)eventFeatureData["EventStart"]).Count > 0;
		}
		return true;
	}

	private void ApplyEditedFeatureData()
	{
		if (_eventDataEditedFromEditor)
		{
			ApplyEditedEventFeatureData();
		}
		if (_preSpawnDataEditedFromEditor)
		{
			ApplyEditedPreSpawnFeatureData();
		}
	}

	private void ApplyEditedEventFeatureData()
	{
		if (featureData == null)
		{
			featureData = new Godot.Collections.Dictionary<StringName, Dictionary>();
		}
		StringName key = new StringName("Event");
		Dictionary dictionary = BuildEventFeatureData();
		Dictionary dictionary2 = ((featureData.TryGetValue(key, out var value) && value != null) ? value.Duplicate(deep: true) : new Dictionary());
		dictionary2["EventInit"] = dictionary["EventInit"];
		dictionary2["EventReady"] = dictionary["EventReady"];
		dictionary2["EventStart"] = dictionary["EventStart"];
		if ((eventEntry?.Count ?? 0) > 0 || !dictionary2.ContainsKey("EventEntry"))
		{
			dictionary2["EventEntry"] = dictionary["EventEntry"];
		}
		if (!dictionary2.ContainsKey("RemotePhaseTimeoutSeconds"))
		{
			dictionary2["RemotePhaseTimeoutSeconds"] = dictionary["RemotePhaseTimeoutSeconds"];
		}
		if (!HasEventEntries(dictionary2))
		{
			featureData.Remove(new StringName("Event"));
		}
		else
		{
			featureData[key] = dictionary2;
		}
	}

	private void ApplyEditedPreSpawnFeatureData()
	{
		if (featureData == null)
		{
			featureData = new Godot.Collections.Dictionary<StringName, Dictionary>();
		}
		StringName key = new StringName("PreSpawn");
		Dictionary dictionary = BuildPreSpawnFeatureData();
		Dictionary dictionary2 = ((featureData.TryGetValue(key, out var value) && value != null) ? value.Duplicate(deep: true) : new Dictionary());
		if (dictionary == null)
		{
			dictionary2.Remove("Packet");
		}
		else
		{
			dictionary2["Packet"] = dictionary["Packet"];
		}
		if (dictionary2.Count == 0)
		{
			featureData.Remove(key);
		}
		else
		{
			featureData[key] = dictionary2;
		}
	}

	private void MergeLegacyPreSpawnFeatureData(Dictionary levelData)
	{
		if (featureData.ContainsKey("PreSpawn"))
		{
			return;
		}
		Godot.Collections.Array array = GetArray(GetDict(levelData, "PreSpawn"), "Packet");
		if (array.Count == 0)
		{
			return;
		}
		Dictionary dictionary = new Dictionary { ["Packet"] = new Godot.Collections.Array() };
		foreach (Variant item in array)
		{
			((Godot.Collections.Array)dictionary["Packet"]).Add(item);
		}
		featureData["PreSpawn"] = dictionary;
	}

	private void BuildPacketBankFeatureData()
	{
		if (packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN && GodotObject.IsInstanceValid(rainData))
		{
			Dictionary dictionary = rainData.Export();
			dictionary["PlantColumn"] = plantColumn;
			featureData["RainMode"] = dictionary;
		}
		if (packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR && GodotObject.IsInstanceValid(conveyorData))
		{
			Dictionary dictionary2 = conveyorData.Export();
			dictionary2["PlantColumn"] = plantColumn;
			featureData["ConveyorBelt"] = dictionary2;
		}
		if (packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET || packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE)
		{
			featureData["PacketBank"] = new Dictionary { ["PacketBankName"] = packetBank };
			Dictionary dictionary3 = new Dictionary();
			dictionary3["Method"] = Enum.GetName(typeof(TowerDefenseEnum.LEVEL_SEEDBANK_METHOD), packetBankMethod);
			dictionary3["PlantColumn"] = plantColumn;
			dictionary3["LimitGridPlantNum"] = limitGridPlantNum;
			dictionary3["ColdDownStart"] = packetColdDownStart;
			dictionary3["ColdDownUse"] = packetColdDownUse;
			dictionary3["Packet"] = ExportPacketBankList();
			featureData["SeedBank"] = dictionary3;
		}
	}

	public void ExportToFeatureProcess()
	{
		ExportToFeatureProcess(refreshEditorValues: false);
	}

	private void ExportToFeatureProcess(bool refreshEditorValues)
	{
		if (!GodotObject.IsInstanceValid(data) && HasProcessName(processName) && TryGetFinishMethodFromProcessName(processName.ToString(), out var lEVEL_FINISH_METHOD))
		{
			finishMethod = lEVEL_FINISH_METHOD;
		}
		bool preferExplicitData = !refreshEditorValues && HasExplicitSection("Feature");
		bool preferExplicitData2 = !refreshEditorValues && HasExplicitSection("Process");
		Godot.Collections.Dictionary<StringName, Dictionary> preserved = DuplicateFeatureData(featureData);
		StringName preservedName = processName;
		Dictionary preservedData = processData?.Duplicate(deep: true) ?? new Dictionary();
		if (featureData == null)
		{
			featureData = new Godot.Collections.Dictionary<StringName, Dictionary>();
		}
		featureData.Clear();
		BuildBaseFeatureData();
		Dictionary dictionary = BuildPreSpawnFeatureData();
		if (dictionary != null)
		{
			featureData["PreSpawn"] = dictionary;
		}
		if (isCustomTalk && GodotObject.IsInstanceValid(customTalk))
		{
			if (GodotObject.IsInstanceValid(customTalk.data) && customTalk.data.Data.VariantType == Variant.Type.Dictionary)
			{
				Dictionary dictionary2 = customTalk.data.Data.AsGodotDictionary().Duplicate();
				dictionary2["isCustom"] = true;
				featureData["NpcTalk"] = dictionary2;
			}
		}
		else if (talk.AsString() != "")
		{
			featureData["NpcTalk"] = new Dictionary { ["TalkName"] = talk };
		}
		if (GodotObject.IsInstanceValid(sunManager))
		{
			featureData["Sun"] = sunManager.Export();
		}
		BuildPacketBankFeatureData();
		featureData["BGM"] = new Dictionary { ["BackgroundMusic"] = backgroundMusic };
		if (GodotObject.IsInstanceValid(fogManager) && fogManager.open)
		{
			featureData["Fog"] = fogManager.Export();
		}
		if (GodotObject.IsInstanceValid(lookStarManager) && lookStarManager.open)
		{
			featureData["LookStar"] = lookStarManager.Export();
		}
		featureData["ScreenEffect"] = new Dictionary
		{
			["StormOpen"] = stormOpen,
			["PacketBankMethod"] = (int)packetBankMethod
		};
		if (isCustomTutorial && GodotObject.IsInstanceValid(customTutorial))
		{
			if (GodotObject.IsInstanceValid(customTutorial.data) && customTutorial.data.Data.VariantType == Variant.Type.Dictionary)
			{
				Dictionary dictionary3 = customTutorial.data.Data.AsGodotDictionary().Duplicate();
				dictionary3["isCustom"] = true;
				featureData["Tutorial"] = dictionary3;
			}
		}
		else if (tutorial.AsString() != "")
		{
			featureData["Tutorial"] = new Dictionary { ["TutorialName"] = tutorial };
		}
		Dictionary dictionary4 = BuildEventFeatureData();
		if (HasEventEntries(dictionary4))
		{
			featureData["Event"] = dictionary4;
		}
		if ((finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE || finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2) && GodotObject.IsInstanceValid(waveManager))
		{
			featureData["Wave"] = waveManager.Export();
		}
		processName = "";
		processData = new Dictionary();
		switch (finishMethod)
		{
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE:
			processName = "Wave";
			processData["StormOpen"] = stormOpen;
			processData["MowerUse"] = mowerUse;
			break;
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2:
			processName = "IZM2";
			processData["StormOpen"] = stormOpen;
			processData["MowerUse"] = mowerUse;
			break;
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE:
			processName = "Vase";
			if (GodotObject.IsInstanceValid(vaseManager))
			{
				processData = vaseManager.Export();
			}
			processData["PacketBankMethod"] = (int)packetBankMethod;
			processData["MowerUse"] = mowerUse;
			break;
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM:
			processName = "IZM";
			if (GodotObject.IsInstanceValid(izmManager))
			{
				processData = izmManager.Export();
			}
			break;
		case TowerDefenseEnum.LEVEL_FINISH_METHOD.QUIZ:
			processName = "Quiz";
			break;
		}
		MergePreservedFeatureData(preserved, preferExplicitData);
		MergePreservedProcessData(preservedName, preservedData, preferExplicitData2);
	}

	private bool HasExplicitSection(string sectionName)
	{
		if (!GodotObject.IsInstanceValid(data) || data.Data.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		if (!data.Data.AsGodotDictionary().TryGetValue(sectionName, out var value))
		{
			return false;
		}
		return value.VariantType switch
		{
			Variant.Type.Array => value.AsGodotArray().Count > 0, 
			Variant.Type.Dictionary => value.AsGodotDictionary().Count > 0, 
			_ => false, 
		};
	}

	private void SynchronizeMapFeature()
	{
		if (featureData == null)
		{
			featureData = new Godot.Collections.Dictionary<StringName, Dictionary>();
		}
		StringName key = new StringName("Map");
		if (!featureData.TryGetValue(key, out var value) || value == null)
		{
			value = new Dictionary();
			featureData[key] = value;
		}
		value["MapName"] = map;
	}

	private bool HasSynchronizedMapFeature(string value)
	{
		StringName key = new StringName("Map");
		if (featureData != null && featureData.TryGetValue(key, out var value2) && value2 != null)
		{
			return string.Equals(value2.GetValueOrDefault("MapName", "").AsString(), value, StringComparison.Ordinal);
		}
		return false;
	}

	private static Godot.Collections.Dictionary<StringName, Dictionary> DuplicateFeatureData(Godot.Collections.Dictionary<StringName, Dictionary> source)
	{
		Godot.Collections.Dictionary<StringName, Dictionary> dictionary = new Godot.Collections.Dictionary<StringName, Dictionary>();
		if (source == null)
		{
			return dictionary;
		}
		foreach (StringName key in source.Keys)
		{
			dictionary[key] = source[key]?.Duplicate(deep: true) ?? new Dictionary();
		}
		return dictionary;
	}

	private void MergePreservedFeatureData(Godot.Collections.Dictionary<StringName, Dictionary> preserved, bool preferExplicitData)
	{
		foreach (StringName key in preserved.Keys)
		{
			Dictionary dictionary = preserved[key];
			if (featureData.TryGetValue(key, out var value) && value != null)
			{
				if (preferExplicitData)
				{
					Dictionary dictionary2 = dictionary;
					dictionary = value.Duplicate(deep: true);
					foreach (Variant key2 in dictionary2.Keys)
					{
						dictionary[key2] = dictionary2[key2];
					}
				}
				else
				{
					foreach (Variant key3 in value.Keys)
					{
						dictionary[key3] = value[key3];
					}
				}
			}
			featureData[key] = dictionary;
		}
	}

	private void MergePreservedProcessData(StringName preservedName, Dictionary preservedData, bool preferExplicitData)
	{
		if (!HasProcessName(preservedName))
		{
			return;
		}
		if (processName != preservedName)
		{
			if (preferExplicitData)
			{
				processName = preservedName;
				processData = preservedData;
			}
			return;
		}
		if (preferExplicitData)
		{
			foreach (Variant key in preservedData.Keys)
			{
				processData[key] = preservedData[key];
			}
			return;
		}
		foreach (Variant key2 in processData.Keys)
		{
			preservedData[key2] = processData[key2];
		}
		processData = preservedData;
	}

	public void ConveyorPreset()
	{
		packetBankMethod = TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR;
		waveManager.minNextWaveHealthPercentage = 0.45;
		waveManager.maxNextWaveHealthPercentage = 0.35;
		waveManager.beginCol = 9.0;
		waveManager.spawnColEnd = 15.0;
		waveManager.spawnColStart = 6.0;
		sunManager.open = false;
	}

	public void RainPreset()
	{
		packetBankMethod = TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN;
		sunManager.open = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(39)
		{
			new MethodInfo(MethodName.HasProcessName, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDict, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "src", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "src", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportPacketBankList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetFinishMethodFromEditor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMapFromEditor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPacketBankMethodFromEditor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MarkEventDataEditedFromEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkPreSpawnDataEditedFromEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareForEditorSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshEditorDataSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetPropertyList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._Get, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyCanRevert, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyGetRevert, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildBaseFeatureData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildPreSpawnFeatureData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildEventFeatureData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AppendEventExports, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "events", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasEventEntries, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "eventFeatureData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyEditedFeatureData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyEditedEventFeatureData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyEditedPreSpawnFeatureData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MergeLegacyPreSpawnFeatureData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "levelData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPacketBankFeatureData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportToFeatureProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportToFeatureProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "refreshEditorValues", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasExplicitSection, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sectionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SynchronizeMapFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasSynchronizedMapFeature, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DuplicateFeatureData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MergePreservedFeatureData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "preserved", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "preferExplicitData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MergePreservedProcessData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "preservedName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "preservedData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "preferExplicitData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConveyorPreset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RainPreset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasProcessName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProcessName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDict && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetDict(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetArray && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetArray(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ExportPacketBankList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(ExportPacketBankList());
			return true;
		}
		if (method == MethodName.SetFinishMethodFromEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetFinishMethodFromEditor(VariantUtils.ConvertTo<TowerDefenseEnum.LEVEL_FINISH_METHOD>(in args[0])));
			return true;
		}
		if (method == MethodName.SetMapFromEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetMapFromEditor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPacketBankMethodFromEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetPacketBankMethodFromEditor(VariantUtils.ConvertTo<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(in args[0])));
			return true;
		}
		if (method == MethodName.MarkEventDataEditedFromEditor && args.Count == 0)
		{
			MarkEventDataEditedFromEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkPreSpawnDataEditedFromEditor && args.Count == 0)
		{
			MarkPreSpawnDataEditedFromEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareForEditorSave && args.Count == 0)
		{
			PrepareForEditorSave();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEditorDataSnapshot && args.Count == 0)
		{
			RefreshEditorDataSnapshot();
			ret = default;
			return true;
		}
		if (method == MethodName._GetPropertyList && args.Count == 0)
		{
			Array<Dictionary> array = _GetPropertyList();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._Get && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_Get(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyCanRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(_PropertyCanRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyGetRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_PropertyGetRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.BuildBaseFeatureData && args.Count == 0)
		{
			BuildBaseFeatureData();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildPreSpawnFeatureData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(BuildPreSpawnFeatureData());
			return true;
		}
		if (method == MethodName.BuildEventFeatureData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(BuildEventFeatureData());
			return true;
		}
		if (method == MethodName.AppendEventExports && args.Count == 3)
		{
			AppendEventExports(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasEventEntries && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasEventEntries(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyEditedFeatureData && args.Count == 0)
		{
			ApplyEditedFeatureData();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyEditedEventFeatureData && args.Count == 0)
		{
			ApplyEditedEventFeatureData();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyEditedPreSpawnFeatureData && args.Count == 0)
		{
			ApplyEditedPreSpawnFeatureData();
			ret = default;
			return true;
		}
		if (method == MethodName.MergeLegacyPreSpawnFeatureData && args.Count == 1)
		{
			MergeLegacyPreSpawnFeatureData(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildPacketBankFeatureData && args.Count == 0)
		{
			BuildPacketBankFeatureData();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportToFeatureProcess && args.Count == 0)
		{
			ExportToFeatureProcess();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportToFeatureProcess && args.Count == 1)
		{
			ExportToFeatureProcess(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasExplicitSection && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasExplicitSection(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SynchronizeMapFeature && args.Count == 0)
		{
			SynchronizeMapFeature();
			ret = default;
			return true;
		}
		if (method == MethodName.HasSynchronizedMapFeature && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSynchronizedMapFeature(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DuplicateFeatureData && args.Count == 1)
		{
			Godot.Collections.Dictionary<StringName, Dictionary> dictionary = DuplicateFeatureData(VariantUtils.ConvertToDictionary<StringName, Dictionary>(in args[0]));
			ret = VariantUtils.CreateFromDictionary(dictionary);
			return true;
		}
		if (method == MethodName.MergePreservedFeatureData && args.Count == 2)
		{
			MergePreservedFeatureData(VariantUtils.ConvertToDictionary<StringName, Dictionary>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MergePreservedProcessData && args.Count == 3)
		{
			MergePreservedProcessData(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConveyorPreset && args.Count == 0)
		{
			ConveyorPreset();
			ret = default;
			return true;
		}
		if (method == MethodName.RainPreset && args.Count == 0)
		{
			RainPreset();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasProcessName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProcessName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDict && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetDict(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetArray && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetArray(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AppendEventExports && args.Count == 3)
		{
			AppendEventExports(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasEventEntries && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasEventEntries(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.DuplicateFeatureData && args.Count == 1)
		{
			Godot.Collections.Dictionary<StringName, Dictionary> dictionary = DuplicateFeatureData(VariantUtils.ConvertToDictionary<StringName, Dictionary>(in args[0]));
			ret = VariantUtils.CreateFromDictionary(dictionary);
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.HasProcessName)
		{
			return true;
		}
		if (method == MethodName.GetDict)
		{
			return true;
		}
		if (method == MethodName.GetArray)
		{
			return true;
		}
		if (method == MethodName.ExportPacketBankList)
		{
			return true;
		}
		if (method == MethodName.SetFinishMethodFromEditor)
		{
			return true;
		}
		if (method == MethodName.SetMapFromEditor)
		{
			return true;
		}
		if (method == MethodName.SetPacketBankMethodFromEditor)
		{
			return true;
		}
		if (method == MethodName.MarkEventDataEditedFromEditor)
		{
			return true;
		}
		if (method == MethodName.MarkPreSpawnDataEditedFromEditor)
		{
			return true;
		}
		if (method == MethodName.PrepareForEditorSave)
		{
			return true;
		}
		if (method == MethodName.RefreshEditorDataSnapshot)
		{
			return true;
		}
		if (method == MethodName._GetPropertyList)
		{
			return true;
		}
		if (method == MethodName._Set)
		{
			return true;
		}
		if (method == MethodName._Get)
		{
			return true;
		}
		if (method == MethodName._PropertyCanRevert)
		{
			return true;
		}
		if (method == MethodName._PropertyGetRevert)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.BuildBaseFeatureData)
		{
			return true;
		}
		if (method == MethodName.BuildPreSpawnFeatureData)
		{
			return true;
		}
		if (method == MethodName.BuildEventFeatureData)
		{
			return true;
		}
		if (method == MethodName.AppendEventExports)
		{
			return true;
		}
		if (method == MethodName.HasEventEntries)
		{
			return true;
		}
		if (method == MethodName.ApplyEditedFeatureData)
		{
			return true;
		}
		if (method == MethodName.ApplyEditedEventFeatureData)
		{
			return true;
		}
		if (method == MethodName.ApplyEditedPreSpawnFeatureData)
		{
			return true;
		}
		if (method == MethodName.MergeLegacyPreSpawnFeatureData)
		{
			return true;
		}
		if (method == MethodName.BuildPacketBankFeatureData)
		{
			return true;
		}
		if (method == MethodName.ExportToFeatureProcess)
		{
			return true;
		}
		if (method == MethodName.HasExplicitSection)
		{
			return true;
		}
		if (method == MethodName.SynchronizeMapFeature)
		{
			return true;
		}
		if (method == MethodName.HasSynchronizedMapFeature)
		{
			return true;
		}
		if (method == MethodName.DuplicateFeatureData)
		{
			return true;
		}
		if (method == MethodName.MergePreservedFeatureData)
		{
			return true;
		}
		if (method == MethodName.MergePreservedProcessData)
		{
			return true;
		}
		if (method == MethodName.ConveyorPreset)
		{
			return true;
		}
		if (method == MethodName.RainPreset)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.canExport)
		{
			canExport = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.data)
		{
			data = VariantUtils.ConvertTo<Json>(in value);
			return true;
		}
		if (name == PropertyName.packetBankMethod)
		{
			packetBankMethod = VariantUtils.ConvertTo<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(in value);
			return true;
		}
		if (name == PropertyName._canExport)
		{
			_canExport = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._eventDataEditedFromEditor)
		{
			_eventDataEditedFromEditor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._preSpawnDataEditedFromEditor)
		{
			_preSpawnDataEditedFromEditor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._data)
		{
			_data = VariantUtils.ConvertTo<Json>(in value);
			return true;
		}
		if (name == PropertyName.featureData)
		{
			featureData = VariantUtils.ConvertToDictionary<StringName, Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.processName)
		{
			processName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.processData)
		{
			processData = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.finishMethod)
		{
			finishMethod = VariantUtils.ConvertTo<TowerDefenseEnum.LEVEL_FINISH_METHOD>(in value);
			return true;
		}
		if (name == PropertyName.baseTimeScale)
		{
			baseTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.mowerUse)
		{
			mowerUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.talk)
		{
			talk = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.tutorial)
		{
			tutorial = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.map)
		{
			map = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.backgroundMusic)
		{
			backgroundMusic = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.firstRewardType)
		{
			firstRewardType = VariantUtils.ConvertTo<TowerDefenseEnum.LEVEL_REWARDTYPE>(in value);
			return true;
		}
		if (name == PropertyName.firstRewardValue)
		{
			firstRewardValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.eventRemotePhaseTimeoutSeconds)
		{
			eventRemotePhaseTimeoutSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.eventInit)
		{
			eventInit = VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in value);
			return true;
		}
		if (name == PropertyName.eventEntry)
		{
			eventEntry = VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in value);
			return true;
		}
		if (name == PropertyName.eventReady)
		{
			eventReady = VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in value);
			return true;
		}
		if (name == PropertyName.eventStart)
		{
			eventStart = VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in value);
			return true;
		}
		if (name == PropertyName.preSpawnList)
		{
			preSpawnList = VariantUtils.ConvertToArray<TowerDefenseLevelPreSpawnConfig>(in value);
			return true;
		}
		if (name == PropertyName.limitGridPlantNum)
		{
			limitGridPlantNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.plantColumn)
		{
			plantColumn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetColdDownStart)
		{
			packetColdDownStart = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetColdDownUse)
		{
			packetColdDownUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._packetBankMethod)
		{
			_packetBankMethod = VariantUtils.ConvertTo<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(in value);
			return true;
		}
		if (name == PropertyName.stormOpen)
		{
			stormOpen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sunManager)
		{
			sunManager = VariantUtils.ConvertTo<TowerDefenseLevelSunManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName.fogManager)
		{
			fogManager = VariantUtils.ConvertTo<TowerDefenseLevelFogManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName.lookStarManager)
		{
			lookStarManager = VariantUtils.ConvertTo<TowerDefenseLevelLookStarManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName.waveManager)
		{
			waveManager = VariantUtils.ConvertTo<TowerDefenseLevelWaveManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName.vaseManager)
		{
			vaseManager = VariantUtils.ConvertTo<TowerDefenseLevelVaseManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName.izmManager)
		{
			izmManager = VariantUtils.ConvertTo<TowerDefenseLevelIZMManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName.isCustomTalk)
		{
			isCustomTalk = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isCustomTutorial)
		{
			isCustomTutorial = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.customTalk)
		{
			customTalk = VariantUtils.ConvertTo<NpcTalkConfig>(in value);
			return true;
		}
		if (name == PropertyName.customTutorial)
		{
			customTutorial = VariantUtils.ConvertTo<TutorialConfig>(in value);
			return true;
		}
		if (name == PropertyName.packetBank)
		{
			packetBank = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.packetBankList)
		{
			packetBankList = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.conveyorData)
		{
			conveyorData = VariantUtils.ConvertTo<TowerDefenseConveyorConfig>(in value);
			return true;
		}
		if (name == PropertyName.rainData)
		{
			rainData = VariantUtils.ConvertTo<TowerDefenseRainModeConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.canExport)
		{
			from = canExport;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.data)
		{
			value = VariantUtils.CreateFrom<Json>(data);
			return true;
		}
		if (name == PropertyName.HasPendingEditorFeatureChanges)
		{
			from = HasPendingEditorFeatureChanges;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.packetBankMethod)
		{
			value = VariantUtils.CreateFrom<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(packetBankMethod);
			return true;
		}
		if (name == PropertyName._canExport)
		{
			value = VariantUtils.CreateFrom(in _canExport);
			return true;
		}
		if (name == PropertyName._eventDataEditedFromEditor)
		{
			value = VariantUtils.CreateFrom(in _eventDataEditedFromEditor);
			return true;
		}
		if (name == PropertyName._preSpawnDataEditedFromEditor)
		{
			value = VariantUtils.CreateFrom(in _preSpawnDataEditedFromEditor);
			return true;
		}
		if (name == PropertyName._data)
		{
			value = VariantUtils.CreateFrom(in _data);
			return true;
		}
		if (name == PropertyName.featureData)
		{
			value = VariantUtils.CreateFromDictionary(featureData);
			return true;
		}
		if (name == PropertyName.processName)
		{
			value = VariantUtils.CreateFrom(in processName);
			return true;
		}
		if (name == PropertyName.processData)
		{
			value = VariantUtils.CreateFrom(in processData);
			return true;
		}
		if (name == PropertyName.finishMethod)
		{
			value = VariantUtils.CreateFrom(in finishMethod);
			return true;
		}
		if (name == PropertyName.baseTimeScale)
		{
			value = VariantUtils.CreateFrom(in baseTimeScale);
			return true;
		}
		if (name == PropertyName.mowerUse)
		{
			value = VariantUtils.CreateFrom(in mowerUse);
			return true;
		}
		if (name == PropertyName.talk)
		{
			value = VariantUtils.CreateFrom(in talk);
			return true;
		}
		if (name == PropertyName.tutorial)
		{
			value = VariantUtils.CreateFrom(in tutorial);
			return true;
		}
		if (name == PropertyName.map)
		{
			value = VariantUtils.CreateFrom(in map);
			return true;
		}
		if (name == PropertyName.backgroundMusic)
		{
			value = VariantUtils.CreateFrom(in backgroundMusic);
			return true;
		}
		if (name == PropertyName.firstRewardType)
		{
			value = VariantUtils.CreateFrom(in firstRewardType);
			return true;
		}
		if (name == PropertyName.firstRewardValue)
		{
			value = VariantUtils.CreateFrom(in firstRewardValue);
			return true;
		}
		if (name == PropertyName.eventRemotePhaseTimeoutSeconds)
		{
			value = VariantUtils.CreateFrom(in eventRemotePhaseTimeoutSeconds);
			return true;
		}
		if (name == PropertyName.eventInit)
		{
			value = VariantUtils.CreateFromArray(eventInit);
			return true;
		}
		if (name == PropertyName.eventEntry)
		{
			value = VariantUtils.CreateFromArray(eventEntry);
			return true;
		}
		if (name == PropertyName.eventReady)
		{
			value = VariantUtils.CreateFromArray(eventReady);
			return true;
		}
		if (name == PropertyName.eventStart)
		{
			value = VariantUtils.CreateFromArray(eventStart);
			return true;
		}
		if (name == PropertyName.preSpawnList)
		{
			value = VariantUtils.CreateFromArray(preSpawnList);
			return true;
		}
		if (name == PropertyName.limitGridPlantNum)
		{
			value = VariantUtils.CreateFrom(in limitGridPlantNum);
			return true;
		}
		if (name == PropertyName.plantColumn)
		{
			value = VariantUtils.CreateFrom(in plantColumn);
			return true;
		}
		if (name == PropertyName.packetColdDownStart)
		{
			value = VariantUtils.CreateFrom(in packetColdDownStart);
			return true;
		}
		if (name == PropertyName.packetColdDownUse)
		{
			value = VariantUtils.CreateFrom(in packetColdDownUse);
			return true;
		}
		if (name == PropertyName._packetBankMethod)
		{
			value = VariantUtils.CreateFrom(in _packetBankMethod);
			return true;
		}
		if (name == PropertyName.stormOpen)
		{
			value = VariantUtils.CreateFrom(in stormOpen);
			return true;
		}
		if (name == PropertyName.sunManager)
		{
			value = VariantUtils.CreateFrom(in sunManager);
			return true;
		}
		if (name == PropertyName.fogManager)
		{
			value = VariantUtils.CreateFrom(in fogManager);
			return true;
		}
		if (name == PropertyName.lookStarManager)
		{
			value = VariantUtils.CreateFrom(in lookStarManager);
			return true;
		}
		if (name == PropertyName.waveManager)
		{
			value = VariantUtils.CreateFrom(in waveManager);
			return true;
		}
		if (name == PropertyName.vaseManager)
		{
			value = VariantUtils.CreateFrom(in vaseManager);
			return true;
		}
		if (name == PropertyName.izmManager)
		{
			value = VariantUtils.CreateFrom(in izmManager);
			return true;
		}
		if (name == PropertyName.isCustomTalk)
		{
			value = VariantUtils.CreateFrom(in isCustomTalk);
			return true;
		}
		if (name == PropertyName.isCustomTutorial)
		{
			value = VariantUtils.CreateFrom(in isCustomTutorial);
			return true;
		}
		if (name == PropertyName.customTalk)
		{
			value = VariantUtils.CreateFrom(in customTalk);
			return true;
		}
		if (name == PropertyName.customTutorial)
		{
			value = VariantUtils.CreateFrom(in customTutorial);
			return true;
		}
		if (name == PropertyName.packetBank)
		{
			value = VariantUtils.CreateFrom(in packetBank);
			return true;
		}
		if (name == PropertyName.packetBankList)
		{
			value = VariantUtils.CreateFrom(in packetBankList);
			return true;
		}
		if (name == PropertyName.conveyorData)
		{
			value = VariantUtils.CreateFrom(in conveyorData);
			return true;
		}
		if (name == PropertyName.rainData)
		{
			value = VariantUtils.CreateFrom(in rainData);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.canExport, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._canExport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._eventDataEditedFromEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preSpawnDataEditedFromEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.data, PropertyHint.ResourceType, "JSON", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasPendingEditorFeatureChanges, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.featureData, PropertyHint.TypeString, "21/0:;27/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.processName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.processData, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.finishMethod, PropertyHint.Enum, "WAVE,VASE,IZM,QUIZ,IZM2,EMPTY", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.baseTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Mower", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mowerUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Tutorial", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Nil, PropertyName.talk, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable | PropertyUsageFlags.NilIsVariant, exported: true),
			new PropertyInfo(Variant.Type.Nil, PropertyName.tutorial, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable | PropertyUsageFlags.NilIsVariant, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Map", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.map, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "BGM", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.backgroundMusic, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Reward", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.firstRewardType, PropertyHint.Enum, "NOONE,PACKET,COLLECTABLE,COIN,TROPHY", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, PropertyName.firstRewardValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable | PropertyUsageFlags.NilIsVariant, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Event", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.eventRemotePhaseTimeoutSeconds, PropertyHint.Range, "0,120,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventInit, PropertyHint.TypeString, "24/17:TowerDefenseLevelEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventEntry, PropertyHint.TypeString, "24/17:TowerDefenseLevelEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventReady, PropertyHint.TypeString, "24/17:TowerDefenseLevelEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventStart, PropertyHint.TypeString, "24/17:TowerDefenseLevelEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "PreSpawn", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.preSpawnList, PropertyHint.TypeString, "24/17:TowerDefenseLevelPreSpawnConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "PacketBank", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.limitGridPlantNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.plantColumn, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.packetColdDownStart, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.packetColdDownUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.packetBankMethod, PropertyHint.Enum, "NOONE,CHOOSE,PRESET,CONVEYOR,RAIN", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._packetBankMethod, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, "Effect", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.stormOpen, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Sun", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.sunManager, PropertyHint.ResourceType, "TowerDefenseLevelSunManagerConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Fog", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.fogManager, PropertyHint.ResourceType, "TowerDefenseLevelFogManagerConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "LookStar", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.lookStarManager, PropertyHint.ResourceType, "TowerDefenseLevelLookStarManagerConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Wave", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.waveManager, PropertyHint.ResourceType, "TowerDefenseLevelWaveManagerConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Vase", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.vaseManager, PropertyHint.ResourceType, "TowerDefenseLevelVaseManagerConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Vase", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.izmManager, PropertyHint.ResourceType, "TowerDefenseLevelIZMManagerConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Option", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isCustomTalk, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isCustomTutorial, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.customTalk, PropertyHint.ResourceType, "NpcTalkConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.customTutorial, PropertyHint.ResourceType, "TutorialConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.packetBank, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetBankList, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyorData, PropertyHint.ResourceType, "TowerDefenseConveyorConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.rainData, PropertyHint.ResourceType, "TowerDefenseRainModeConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.canExport, Variant.From<bool>(canExport));
		info.AddProperty(PropertyName.data, Variant.From<Json>(data));
		info.AddProperty(PropertyName.packetBankMethod, Variant.From<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(packetBankMethod));
		info.AddProperty(PropertyName._canExport, Variant.From(in _canExport));
		info.AddProperty(PropertyName._eventDataEditedFromEditor, Variant.From(in _eventDataEditedFromEditor));
		info.AddProperty(PropertyName._preSpawnDataEditedFromEditor, Variant.From(in _preSpawnDataEditedFromEditor));
		info.AddProperty(PropertyName._data, Variant.From(in _data));
		info.AddProperty(PropertyName.featureData, Variant.CreateFrom(featureData));
		info.AddProperty(PropertyName.processName, Variant.From(in processName));
		info.AddProperty(PropertyName.processData, Variant.From(in processData));
		info.AddProperty(PropertyName.finishMethod, Variant.From(in finishMethod));
		info.AddProperty(PropertyName.baseTimeScale, Variant.From(in baseTimeScale));
		info.AddProperty(PropertyName.mowerUse, Variant.From(in mowerUse));
		info.AddProperty(PropertyName.talk, Variant.From(in talk));
		info.AddProperty(PropertyName.tutorial, Variant.From(in tutorial));
		info.AddProperty(PropertyName.map, Variant.From(in map));
		info.AddProperty(PropertyName.backgroundMusic, Variant.From(in backgroundMusic));
		info.AddProperty(PropertyName.firstRewardType, Variant.From(in firstRewardType));
		info.AddProperty(PropertyName.firstRewardValue, Variant.From(in firstRewardValue));
		info.AddProperty(PropertyName.eventRemotePhaseTimeoutSeconds, Variant.From(in eventRemotePhaseTimeoutSeconds));
		info.AddProperty(PropertyName.eventInit, Variant.CreateFrom(eventInit));
		info.AddProperty(PropertyName.eventEntry, Variant.CreateFrom(eventEntry));
		info.AddProperty(PropertyName.eventReady, Variant.CreateFrom(eventReady));
		info.AddProperty(PropertyName.eventStart, Variant.CreateFrom(eventStart));
		info.AddProperty(PropertyName.preSpawnList, Variant.CreateFrom(preSpawnList));
		info.AddProperty(PropertyName.limitGridPlantNum, Variant.From(in limitGridPlantNum));
		info.AddProperty(PropertyName.plantColumn, Variant.From(in plantColumn));
		info.AddProperty(PropertyName.packetColdDownStart, Variant.From(in packetColdDownStart));
		info.AddProperty(PropertyName.packetColdDownUse, Variant.From(in packetColdDownUse));
		info.AddProperty(PropertyName._packetBankMethod, Variant.From(in _packetBankMethod));
		info.AddProperty(PropertyName.stormOpen, Variant.From(in stormOpen));
		info.AddProperty(PropertyName.sunManager, Variant.From(in sunManager));
		info.AddProperty(PropertyName.fogManager, Variant.From(in fogManager));
		info.AddProperty(PropertyName.lookStarManager, Variant.From(in lookStarManager));
		info.AddProperty(PropertyName.waveManager, Variant.From(in waveManager));
		info.AddProperty(PropertyName.vaseManager, Variant.From(in vaseManager));
		info.AddProperty(PropertyName.izmManager, Variant.From(in izmManager));
		info.AddProperty(PropertyName.isCustomTalk, Variant.From(in isCustomTalk));
		info.AddProperty(PropertyName.isCustomTutorial, Variant.From(in isCustomTutorial));
		info.AddProperty(PropertyName.customTalk, Variant.From(in customTalk));
		info.AddProperty(PropertyName.customTutorial, Variant.From(in customTutorial));
		info.AddProperty(PropertyName.packetBank, Variant.From(in packetBank));
		info.AddProperty(PropertyName.packetBankList, Variant.From(in packetBankList));
		info.AddProperty(PropertyName.conveyorData, Variant.From(in conveyorData));
		info.AddProperty(PropertyName.rainData, Variant.From(in rainData));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.canExport, out var value))
		{
			canExport = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.data, out var value2))
		{
			data = value2.As<Json>();
		}
		if (info.TryGetProperty(PropertyName.packetBankMethod, out var value3))
		{
			packetBankMethod = value3.As<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>();
		}
		if (info.TryGetProperty(PropertyName._canExport, out var value4))
		{
			_canExport = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._eventDataEditedFromEditor, out var value5))
		{
			_eventDataEditedFromEditor = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._preSpawnDataEditedFromEditor, out var value6))
		{
			_preSpawnDataEditedFromEditor = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._data, out var value7))
		{
			_data = value7.As<Json>();
		}
		if (info.TryGetProperty(PropertyName.featureData, out var value8))
		{
			featureData = value8.AsGodotDictionary<StringName, Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.processName, out var value9))
		{
			processName = value9.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.processData, out var value10))
		{
			processData = value10.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.finishMethod, out var value11))
		{
			finishMethod = value11.As<TowerDefenseEnum.LEVEL_FINISH_METHOD>();
		}
		if (info.TryGetProperty(PropertyName.baseTimeScale, out var value12))
		{
			baseTimeScale = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.mowerUse, out var value13))
		{
			mowerUse = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.talk, out var value14))
		{
			talk = value14.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.tutorial, out var value15))
		{
			tutorial = value15.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.map, out var value16))
		{
			map = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName.backgroundMusic, out var value17))
		{
			backgroundMusic = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName.firstRewardType, out var value18))
		{
			firstRewardType = value18.As<TowerDefenseEnum.LEVEL_REWARDTYPE>();
		}
		if (info.TryGetProperty(PropertyName.firstRewardValue, out var value19))
		{
			firstRewardValue = value19.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.eventRemotePhaseTimeoutSeconds, out var value20))
		{
			eventRemotePhaseTimeoutSeconds = value20.As<double>();
		}
		if (info.TryGetProperty(PropertyName.eventInit, out var value21))
		{
			eventInit = value21.AsGodotArray<TowerDefenseLevelEventBase>();
		}
		if (info.TryGetProperty(PropertyName.eventEntry, out var value22))
		{
			eventEntry = value22.AsGodotArray<TowerDefenseLevelEventBase>();
		}
		if (info.TryGetProperty(PropertyName.eventReady, out var value23))
		{
			eventReady = value23.AsGodotArray<TowerDefenseLevelEventBase>();
		}
		if (info.TryGetProperty(PropertyName.eventStart, out var value24))
		{
			eventStart = value24.AsGodotArray<TowerDefenseLevelEventBase>();
		}
		if (info.TryGetProperty(PropertyName.preSpawnList, out var value25))
		{
			preSpawnList = value25.AsGodotArray<TowerDefenseLevelPreSpawnConfig>();
		}
		if (info.TryGetProperty(PropertyName.limitGridPlantNum, out var value26))
		{
			limitGridPlantNum = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName.plantColumn, out var value27))
		{
			plantColumn = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetColdDownStart, out var value28))
		{
			packetColdDownStart = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetColdDownUse, out var value29))
		{
			packetColdDownUse = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._packetBankMethod, out var value30))
		{
			_packetBankMethod = value30.As<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>();
		}
		if (info.TryGetProperty(PropertyName.stormOpen, out var value31))
		{
			stormOpen = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sunManager, out var value32))
		{
			sunManager = value32.As<TowerDefenseLevelSunManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.fogManager, out var value33))
		{
			fogManager = value33.As<TowerDefenseLevelFogManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.lookStarManager, out var value34))
		{
			lookStarManager = value34.As<TowerDefenseLevelLookStarManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.waveManager, out var value35))
		{
			waveManager = value35.As<TowerDefenseLevelWaveManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.vaseManager, out var value36))
		{
			vaseManager = value36.As<TowerDefenseLevelVaseManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.izmManager, out var value37))
		{
			izmManager = value37.As<TowerDefenseLevelIZMManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.isCustomTalk, out var value38))
		{
			isCustomTalk = value38.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isCustomTutorial, out var value39))
		{
			isCustomTutorial = value39.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.customTalk, out var value40))
		{
			customTalk = value40.As<NpcTalkConfig>();
		}
		if (info.TryGetProperty(PropertyName.customTutorial, out var value41))
		{
			customTutorial = value41.As<TutorialConfig>();
		}
		if (info.TryGetProperty(PropertyName.packetBank, out var value42))
		{
			packetBank = value42.As<string>();
		}
		if (info.TryGetProperty(PropertyName.packetBankList, out var value43))
		{
			packetBankList = value43.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.conveyorData, out var value44))
		{
			conveyorData = value44.As<TowerDefenseConveyorConfig>();
		}
		if (info.TryGetProperty(PropertyName.rainData, out var value45))
		{
			rainData = value45.As<TowerDefenseRainModeConfig>();
		}
	}
}
