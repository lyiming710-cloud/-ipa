using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/GUI/InGame/LevelControl/TowerDefenseInGameLevelControl.cs")]
public class TowerDefenseInGameLevelControl : Control
{
	public new class MethodName : Control.MethodName
	{
		public static readonly StringName Init = "Init";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName TipsPlay = "TipsPlay";

		public static readonly StringName AwardCreate = "AwardCreate";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName worldEntryLabel = "worldEntryLabel";

		public static readonly StringName survivleLabel = "survivleLabel";

		public static readonly StringName _tipsLabel = "_tipsLabel";

		public static readonly StringName _animationPlayer = "_animationPlayer";

		public static readonly StringName config = "config";

		public static readonly StringName awardPos = "awardPos";

		public static readonly StringName awardCreate = "awardCreate";

		public static readonly StringName hasSpawn = "hasSpawn";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public static TowerDefenseInGameLevelControl instance;

	public RichTextLabel worldEntryLabel;

	public Label survivleLabel;

	private Label _tipsLabel;

	private AnimationPlayer _animationPlayer;

	public TowerDefenseLevelConfig config;

	public Vector2 awardPos = Vector2.Zero;

	public bool awardCreate;

	public bool hasSpawn;

	public void Init(TowerDefenseLevelConfig _config)
	{
		config = _config;
		config.Init();
		string text = Tr(config.description).Replace("{UserName}", GameSaveManager.Instance.GetUserCurrent());
		worldEntryLabel.Text = text;
	}

	public override void _Ready()
	{
		instance = this;
		worldEntryLabel = GetNode<RichTextLabel>("%WorldEntryLabel");
		survivleLabel = GetNode<Label>("%SurvivleLabel");
		_tipsLabel = GetNode<Label>("%TipsLabel");
		_tipsLabel.MouseFilter = MouseFilterEnum.Ignore;
		_animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
		worldEntryLabel.Visible = false;
	}

	public override void _ExitTree()
	{
		if (instance == this)
		{
			instance = null;
		}
		config = null;
		worldEntryLabel = null;
		survivleLabel = null;
		_tipsLabel = null;
		_animationPlayer = null;
	}

	public async Task ReadySetPlantPlay()
	{
		AudioManager.Instance.AudioPlay("ReadySetPlants");
		if (_animationPlayer != null)
		{
			string text = (TowerDefenseManager.Instance.IsIZM2Mode() ? "ReadyPlaceZombies" : "ReadySetPlants");
			_animationPlayer.Play(text);
		}
		await ToSignal(GetTree().CreateTimer(2.0, processAlways: false), SceneTreeTimer.SignalName.Timeout);
	}

	public async void TipsPlay(string text, double duration = 2.0)
	{
		if (_tipsLabel != null)
		{
			_tipsLabel.Visible = true;
			_tipsLabel.Text = text;
		}
		if (_animationPlayer != null)
		{
			_animationPlayer.Play("Tips");
		}
		await ToSignal(GetTree().CreateTimer(duration, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		if (_tipsLabel != null)
		{
			_tipsLabel.Visible = false;
		}
	}

	public async void AwardCreate(Vector2 pos)
	{
		if (awardCreate)
		{
			return;
		}
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		if (GodotObject.IsInstanceValid(currentControl) && currentControl.HasPendingBattleOperations)
		{
			return;
		}
		awardCreate = true;
		if (!Global.IsMultiplayerMode && GodotObject.IsInstanceValid(GameSaveManager.Instance) && config != null && !string.IsNullOrEmpty(config.name))
		{
			GameSaveManager.Instance.DeleteLevelProgress(config.name, currentControl?.ModLevelIdentity);
		}
		BattleEventBus.Instance.EmitGameVictory();
		Global global = Global.Instance;
		XWModLevelIdentity xWModLevelIdentity = currentControl?.ModLevelIdentity;
		if ((object)xWModLevelIdentity != null)
		{
			if (!Global.IsMultiplayerMode)
			{
				GameSaveManager.Instance.DeleteLevelProgress(xWModLevelIdentity.LevelSaveKey, xWModLevelIdentity);
				Dictionary level = XWModPlayerProgressService.GetLevel(xWModLevelIdentity);
				Dictionary dictionary = level.GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
				dictionary["Finish"] = dictionary.GetValueOrDefault("Finish", 0).AsInt32() + 1;
				level["Key"] = dictionary;
				level[xWModLevelIdentity.Difficulty] = true;
				TowerDefenseBattleFeatureMower mowerFeature = TowerDefenseManager.Instance.GetMowerFeature();
				if (mowerFeature != null && !mowerFeature.mowerHasRun)
				{
					level["Mower"] = true;
				}
				XWModPlayerProgressService.SetLevel(xWModLevelIdentity, level);
			}
			global.currentAwardType = 0;
			TowerDefenseManager.Instance.CreateAward(TowerDefenseEnum.LEVEL_REWARDTYPE.TROPHY, "0", pos);
			return;
		}
		if ((Global.IsEditor && global.enterLevelMode == "DiyLevel") || global.enterLevelMode == "LoadLevel" || global.enterLevelMode == "OnlineLevel")
		{
			if (global.enterLevelMode == "OnlineLevel")
			{
				InternetServerManager.Instance.OnlineLevelPost(global.enterLevelId, "completion");
				Dictionary levelValue = GameSaveManager.Instance.GetLevelValue(config.name);
				if (!levelValue.ContainsKey("Key"))
				{
					levelValue["Key"] = new Dictionary();
				}
				Dictionary dictionary2 = levelValue["Key"].AsGodotDictionary();
				int num = dictionary2.GetValueOrDefault("Finish", 0).AsInt32();
				dictionary2["Finish"] = num + 1;
				GameSaveManager.Instance.SetKeyValue("CrystalNum", GameSaveManager.Instance.GetKeyValue("CrystalNum").AsInt32() + 1);
				GameSaveManager.Instance.SetLevelValue(config.name, levelValue);
				GameSaveManager.Instance.Save();
				if (global.enterLevelIsBattle)
				{
					global.enterLevelIsBattleFinish = true;
				}
			}
			if (global.enterLevelMode == "DiyLevel" && TowerDefenseManager.Instance.currentLevelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig)
			{
				towerDefenseLevelConfig.canExport = true;
			}
			if (Global.IsEditor && global.enterLevelMode == "DiyLevel")
			{
				DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("DialogBoxTips");
				dialogBoxBase.Set("text", "[center][font_size=24]您已完成关卡[/font_size][/center]");
				await ToSignal(dialogBoxBase, Node.SignalName.TreeExited);
				GetTree().Paused = false;
				Global.TimeScale = 1.0;
				SceneManager.Instance.ChangeScene("LevelEditorStage");
			}
			else
			{
				TowerDefenseManager.Instance.CreateAward(TowerDefenseEnum.LEVEL_REWARDTYPE.TROPHY, "250", pos);
			}
			return;
		}
		if (config.talk.AsString() != "")
		{
			NpcTalkConfig npcTalk = TowerDefenseManager.GetNpcTalk(config.talk.AsString());
			if (GodotObject.IsInstanceValid(npcTalk))
			{
				GameSaveManager.Instance.SetTutorialValue(npcTalk.saveKey, value: true);
			}
		}
		if (config.tutorial.AsString() != "")
		{
			TutorialConfig tutorial = TowerDefenseManager.GetTutorial(config.tutorial.AsString());
			if (GodotObject.IsInstanceValid(tutorial))
			{
				GameSaveManager.Instance.SetTutorialValue(tutorial.saveKey, value: true);
			}
		}
		bool flag = false;
		string text = GameSaveManager.Instance.GetKeyValue("CurrentDifficult").AsString();
		Dictionary levelValue2 = GameSaveManager.Instance.GetLevelValue(config.name);
		if (!levelValue2.ContainsKey("Key"))
		{
			levelValue2["Key"] = new Dictionary();
		}
		Dictionary dictionary3 = levelValue2["Key"].AsGodotDictionary();
		int num2 = dictionary3.GetValueOrDefault("Finish", 0).AsInt32();
		dictionary3["Finish"] = num2 + 1;
		TowerDefenseBattleFeatureMower mowerFeature2 = TowerDefenseManager.Instance.GetMowerFeature();
		if ((mowerFeature2 != null && !mowerFeature2.mowerHasRun) || TowerDefenseManager.Instance.IsIZM2Mode())
		{
			levelValue2["Mower"] = true;
		}
		if (!levelValue2.GetValueOrDefault(text, false).AsBool())
		{
			levelValue2[text] = true;
		}
		GameSaveManager.Instance.SetLevelValue(config.name, levelValue2);
		global.currentAwardType = (int)config.firstRewardType;
		switch (config.firstRewardType)
		{
		case TowerDefenseEnum.LEVEL_REWARDTYPE.PACKET:
			if (config.firstRewardValue.VariantType == Variant.Type.String)
			{
				string text2 = config.firstRewardValue.AsString();
				Dictionary towerDefensePacketValue = GameSaveManager.Instance.GetTowerDefensePacketValue(text2);
				if (!towerDefensePacketValue.GetValueOrDefault("Unlock", false).AsBool())
				{
					towerDefensePacketValue["Unlock"] = true;
					GameSaveManager.Instance.SetTowerDefensePacketValue(text2, towerDefensePacketValue);
					global.currentAwardValue = text2;
					flag = true;
				}
			}
			else
			{
				if (config.firstRewardValue.VariantType != Variant.Type.Array)
				{
					break;
				}
				foreach (Variant item in config.firstRewardValue.AsGodotArray())
				{
					string text3 = item.AsString();
					Dictionary towerDefensePacketValue2 = GameSaveManager.Instance.GetTowerDefensePacketValue(text3);
					if (!towerDefensePacketValue2.GetValueOrDefault("Unlock", false).AsBool())
					{
						towerDefensePacketValue2["Unlock"] = true;
						GameSaveManager.Instance.SetTowerDefensePacketValue(text3, towerDefensePacketValue2);
						if (!flag)
						{
							global.currentAwardValue = text3;
							flag = true;
						}
					}
				}
			}
			break;
		case TowerDefenseEnum.LEVEL_REWARDTYPE.COLLECTABLE:
			if (config.firstRewardValue.VariantType == Variant.Type.String)
			{
				string text4 = config.firstRewardValue.AsString();
				if (GameSaveManager.Instance.GetFeatureValue(text4) == 0)
				{
					GameSaveManager.Instance.SetFeatureValue(text4, true);
					global.currentAwardValue = text4;
					flag = true;
				}
			}
			else
			{
				if (config.firstRewardValue.VariantType != Variant.Type.Array)
				{
					break;
				}
				foreach (Variant item2 in config.firstRewardValue.AsGodotArray())
				{
					string text5 = item2.AsString();
					if (GameSaveManager.Instance.GetFeatureValue(text5) == 0)
					{
						GameSaveManager.Instance.SetFeatureValue(text5, true);
						if (!flag)
						{
							global.currentAwardValue = text5;
							flag = true;
						}
					}
				}
			}
			break;
		case TowerDefenseEnum.LEVEL_REWARDTYPE.COIN:
			if (config.firstRewardValue.VariantType == Variant.Type.String && dictionary3["Finish"].AsInt32() == 1 && !flag)
			{
				global.currentAwardValue = config.firstRewardValue.AsString();
				flag = true;
			}
			break;
		}
		if (!flag)
		{
			global.currentAwardType = 0;
			TowerDefenseManager.Instance.CreateAward(TowerDefenseEnum.LEVEL_REWARDTYPE.NOONE, "250", pos);
		}
		else
		{
			TowerDefenseManager.Instance.CreateAward(config.firstRewardType, global.currentAwardValue, pos);
		}
		GameSaveManager.Instance.Save();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TipsPlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AwardCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.TipsPlay && args.Count == 2)
		{
			TipsPlay(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AwardCreate && args.Count == 1)
		{
			AwardCreate(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.TipsPlay)
		{
			return true;
		}
		if (method == MethodName.AwardCreate)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.worldEntryLabel)
		{
			worldEntryLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.survivleLabel)
		{
			survivleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._tipsLabel)
		{
			_tipsLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationPlayer)
		{
			_animationPlayer = VariantUtils.ConvertTo<AnimationPlayer>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in value);
			return true;
		}
		if (name == PropertyName.awardPos)
		{
			awardPos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.awardCreate)
		{
			awardCreate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hasSpawn)
		{
			hasSpawn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.worldEntryLabel)
		{
			value = VariantUtils.CreateFrom(in worldEntryLabel);
			return true;
		}
		if (name == PropertyName.survivleLabel)
		{
			value = VariantUtils.CreateFrom(in survivleLabel);
			return true;
		}
		if (name == PropertyName._tipsLabel)
		{
			value = VariantUtils.CreateFrom(in _tipsLabel);
			return true;
		}
		if (name == PropertyName._animationPlayer)
		{
			value = VariantUtils.CreateFrom(in _animationPlayer);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.awardPos)
		{
			value = VariantUtils.CreateFrom(in awardPos);
			return true;
		}
		if (name == PropertyName.awardCreate)
		{
			value = VariantUtils.CreateFrom(in awardCreate);
			return true;
		}
		if (name == PropertyName.hasSpawn)
		{
			value = VariantUtils.CreateFrom(in hasSpawn);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.worldEntryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.survivleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._tipsLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.awardPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.awardCreate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasSpawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.worldEntryLabel, Variant.From(in worldEntryLabel));
		info.AddProperty(PropertyName.survivleLabel, Variant.From(in survivleLabel));
		info.AddProperty(PropertyName._tipsLabel, Variant.From(in _tipsLabel));
		info.AddProperty(PropertyName._animationPlayer, Variant.From(in _animationPlayer));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.awardPos, Variant.From(in awardPos));
		info.AddProperty(PropertyName.awardCreate, Variant.From(in awardCreate));
		info.AddProperty(PropertyName.hasSpawn, Variant.From(in hasSpawn));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.worldEntryLabel, out var value))
		{
			worldEntryLabel = value.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.survivleLabel, out var value2))
		{
			survivleLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._tipsLabel, out var value3))
		{
			_tipsLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationPlayer, out var value4))
		{
			_animationPlayer = value4.As<AnimationPlayer>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value5))
		{
			config = value5.As<TowerDefenseLevelConfig>();
		}
		if (info.TryGetProperty(PropertyName.awardPos, out var value6))
		{
			awardPos = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.awardCreate, out var value7))
		{
			awardCreate = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hasSpawn, out var value8))
		{
			hasSpawn = value8.As<bool>();
		}
	}
}
