using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketBank/Resource/Packet/TowerDefensePacketConfig.cs")]
public class TowerDefensePacketConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName SetPreparedCharacterGlobalPosition = "SetPreparedCharacterGlobalPosition";

		public static readonly StringName CreateSpawnRuntimeCopy = "CreateSpawnRuntimeCopy";

		public static readonly StringName CreateRuntimeStateCopy = "CreateRuntimeStateCopy";

		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";

		public static readonly StringName _GetType = "_GetType";

		public static readonly StringName GetCostRise = "GetCostRise";

		public static readonly StringName GetCostMultiple = "GetCostMultiple";

		public static readonly StringName GetCostBeforeModifiers = "GetCostBeforeModifiers";

		public static readonly StringName GetCost = "GetCost";

		public static readonly StringName GetPacketCooldown = "GetPacketCooldown";

		public static readonly StringName GetStartingCooldown = "GetStartingCooldown";

		public static readonly StringName GetWeight = "GetWeight";

		public static readonly StringName GetWavePointCost = "GetWavePointCost";

		public static readonly StringName GetPlantCover = "GetPlantCover";

		public static readonly StringName GetCoverCanDirectPlant = "GetCoverCanDirectPlant";

		public static readonly StringName GetHypnoses = "GetHypnoses";

		public static readonly StringName IsLimitGridNum = "IsLimitGridNum";

		public static readonly StringName ShouldReplacePlantGridOverrideTarget = "ShouldReplacePlantGridOverrideTarget";

		public static readonly StringName CanReplacePlantGridOverrideTarget = "CanReplacePlantGridOverrideTarget";

		public static readonly StringName HasSpawnLimit = "HasSpawnLimit";

		public static readonly StringName CanSpawn = "CanSpawn";

		public static readonly StringName SpawnWaveZombieFromReadOnlyConfig = "SpawnWaveZombieFromReadOnlyConfig";

		public static readonly StringName ApplyInitialArmor = "ApplyInitialArmor";

		public static readonly StringName IsEditorPreviewContext = "IsEditorPreviewContext";

		public static readonly StringName ResolvePreparedCharacterMountPosition = "ResolvePreparedCharacterMountPosition";

		public static readonly StringName RandomAnime = "RandomAnime";

		public static readonly StringName IsZombieWalk = "IsZombieWalk";

		public static readonly StringName Unlock = "Unlock";

		public static readonly StringName ColdDownDecreaseAdd = "ColdDownDecreaseAdd";

		public static readonly StringName ColdDownDecreaseDelete = "ColdDownDecreaseDelete";

		public static readonly StringName ChangeCostAdd = "ChangeCostAdd";

		public static readonly StringName ChangeCostRemove = "ChangeCostRemove";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName characterConfig = "characterConfig";

		public static readonly StringName @override = "override";

		public static readonly StringName saveKey = "saveKey";

		public static readonly StringName unlockCheckList = "unlockCheckList";

		public static readonly StringName name = "name";

		public static readonly StringName describe = "describe";

		public static readonly StringName plantfood = "plantfood";

		public static readonly StringName handbookDescribe = "handbookDescribe";

		public static readonly StringName handbookStory = "handbookStory";

		public static readonly StringName packetFlip = "packetFlip";

		public static readonly StringName packetAnimeClip = "packetAnimeClip";

		public static readonly StringName handbookPacketAnimeOffset = "handbookPacketAnimeOffset";

		public static readonly StringName packetAnimeOffset = "packetAnimeOffset";

		public static readonly StringName packetAnimeScale = "packetAnimeScale";

		public static readonly StringName _characterConfig = "_characterConfig";

		public static readonly StringName type = "type";

		public static readonly StringName canChangeCost = "canChangeCost";

		public static readonly StringName spawnMethod = "spawnMethod";

		public static readonly StringName pressedActions = "pressedActions";

		public static readonly StringName useSucceededActions = "useSucceededActions";

		public static readonly StringName behaviorIds = "behaviorIds";

		public static readonly StringName behaviors = "behaviors";

		public static readonly StringName _override = "_override";

		public static readonly StringName overrideHypnoses = "overrideHypnoses";

		public static readonly StringName overrideCostRise = "overrideCostRise";

		public static readonly StringName overrideCost = "overrideCost";

		public static readonly StringName overridePacketCooldown = "overridePacketCooldown";

		public static readonly StringName overrideStartingCooldown = "overrideStartingCooldown";

		public static readonly StringName overrideWeight = "overrideWeight";

		public static readonly StringName overrideWavePointCost = "overrideWavePointCost";

		public static readonly StringName initArmor = "initArmor";

		public static readonly StringName plantUseCell = "plantUseCell";

		public static readonly StringName izmPlantAllCell = "izmPlantAllCell";

		public static readonly StringName izmPlantLeft = "izmPlantLeft";

		public static readonly StringName disableWhenSunNegative = "disableWhenSunNegative";

		public static readonly StringName expireSun = "expireSun";

		public static readonly StringName canPlaceOnZombie = "canPlaceOnZombie";

		public static readonly StringName coldDownDecreaseDictionary = "coldDownDecreaseDictionary";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string saveKey = "";

	[Export(PropertyHint.None, "")]
	public Array<UnlockConditionBaseConfig> unlockCheckList = new Array<UnlockConditionBaseConfig>();

	[Export(PropertyHint.None, "")]
	public string name;

	[Export(PropertyHint.None, "")]
	public string describe;

	[Export(PropertyHint.None, "")]
	public string plantfood;

	[Export(PropertyHint.None, "")]
	public string handbookDescribe;

	[Export(PropertyHint.None, "")]
	public string handbookStory;

	[Export(PropertyHint.None, "")]
	public bool packetFlip;

	[Export(PropertyHint.None, "")]
	public string packetAnimeClip = "Idle";

	[Export(PropertyHint.None, "")]
	public Vector2 handbookPacketAnimeOffset = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public Vector2 packetAnimeOffset = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public Vector2 packetAnimeScale = Vector2.Zero;

	private TowerDefenseCharacterConfig _characterConfig;

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.PACKET_TYPE type;

	[Export(PropertyHint.None, "")]
	public bool canChangeCost = true;

	[ExportCategory("Spawn")]
	[Export(PropertyHint.Enum, "Noone,Rise,FlyDown")]
	public string spawnMethod = "Rise";

	[ExportCategory("Action Behavior")]
	[Export(PropertyHint.None, "")]
	public Array<CardActionBehaviorDefinition> pressedActions = new Array<CardActionBehaviorDefinition>();

	[Export(PropertyHint.None, "")]
	public Array<CardActionBehaviorDefinition> useSucceededActions = new Array<CardActionBehaviorDefinition>();

	[ExportCategory("Behavior")]
	[Export(PropertyHint.None, "")]
	public Array<StringName> behaviorIds = new Array<StringName>();

	[Export(PropertyHint.None, "")]
	public Array<CardBehaviorDefinition> behaviors = new Array<CardBehaviorDefinition>();

	[ExportCategory("Override")]
	public TowerDefensePacketOverride _override;

	[Export(PropertyHint.None, "")]
	public bool overrideHypnoses;

	[Export(PropertyHint.None, "")]
	public int overrideCostRise = -1;

	[Export(PropertyHint.None, "")]
	public int overrideCost = -1;

	[Export(PropertyHint.None, "")]
	public double overridePacketCooldown = -1.0;

	[Export(PropertyHint.None, "")]
	public double overrideStartingCooldown = -1.0;

	[Export(PropertyHint.None, "")]
	public int overrideWeight = -1;

	[Export(PropertyHint.None, "")]
	public int overrideWavePointCost = -1;

	[ExportCategory("Other")]
	[Export(PropertyHint.None, "")]
	public Array<string> initArmor = new Array<string>();

	[Export(PropertyHint.None, "")]
	public bool plantUseCell = true;

	[Export(PropertyHint.None, "")]
	public bool izmPlantAllCell;

	[Export(PropertyHint.None, "")]
	public bool izmPlantLeft;

	[Export(PropertyHint.None, "")]
	public bool disableWhenSunNegative;

	[Export(PropertyHint.None, "")]
	public int expireSun;

	[ExportCategory("ZombiePlace")]
	[Export(PropertyHint.None, "")]
	public bool canPlaceOnZombie;

	public List<TowerDefensePacketChangeCost> changeCostList = new List<TowerDefensePacketChangeCost>();

	public Dictionary coldDownDecreaseDictionary = new Dictionary();

	[Export(PropertyHint.None, "")]
	public TowerDefenseCharacterConfig characterConfig
	{
		get
		{
			return _characterConfig;
		}
		set
		{
			_characterConfig = value;
			NotifyPropertyListChanged();
		}
	}

	[Export(PropertyHint.None, "")]
	public TowerDefensePacketOverride @override
	{
		get
		{
			return _override;
		}
		set
		{
			_override = value;
		}
	}

	internal static void SetPreparedCharacterGlobalPosition(TowerDefenseCharacter character, Vector2 globalPosition)
	{
		ulong num = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		if (num == 18446744073709551615uL)
		{
			num = Engine.GetPhysicsFrames();
		}
		character.SetGlobalPositionForPhysicsFrame(globalPosition, num);
	}

	public TowerDefensePacketConfig CreateSpawnRuntimeCopy()
	{
		TowerDefensePacketConfig towerDefensePacketConfig = Duplicate() as TowerDefensePacketConfig;
		if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
		{
			return null;
		}
		towerDefensePacketConfig._override = (GodotObject.IsInstanceValid(_override) ? (_override.Duplicate(deep: true) as TowerDefensePacketOverride) : null);
		towerDefensePacketConfig.changeCostList = new List<TowerDefensePacketChangeCost>();
		towerDefensePacketConfig.coldDownDecreaseDictionary = new Dictionary();
		return towerDefensePacketConfig;
	}

	public TowerDefensePacketConfig CreateRuntimeStateCopy()
	{
		TowerDefensePacketConfig towerDefensePacketConfig = Duplicate() as TowerDefensePacketConfig;
		if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
		{
			return null;
		}
		towerDefensePacketConfig._override = (GodotObject.IsInstanceValid(_override) ? (_override.Duplicate() as TowerDefensePacketOverride) : null);
		towerDefensePacketConfig.changeCostList = new List<TowerDefensePacketChangeCost>();
		if (changeCostList != null)
		{
			for (int i = 0; i < changeCostList.Count; i++)
			{
				TowerDefensePacketChangeCost towerDefensePacketChangeCost = changeCostList[i];
				if (GodotObject.IsInstanceValid(towerDefensePacketChangeCost))
				{
					if (towerDefensePacketChangeCost.GetType() == typeof(TowerDefensePacketChangeCost))
					{
						towerDefensePacketConfig.changeCostList.Add(TowerDefensePacketChangeCost.ImportSave(towerDefensePacketChangeCost.ExportSave()));
					}
					else
					{
						towerDefensePacketConfig.changeCostList.Add(towerDefensePacketChangeCost);
					}
				}
			}
		}
		towerDefensePacketConfig.coldDownDecreaseDictionary = new Dictionary();
		return towerDefensePacketConfig;
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 4L;
		Array<Dictionary> array = new Array<Dictionary>();
		if (GodotObject.IsInstanceValid(characterConfig))
		{
			if (GodotObject.IsInstanceValid(characterConfig.armorData))
			{
				string value = string.Join(",", characterConfig.armorData.armorDictionary.Keys);
				array.Add(new Dictionary
				{
					["name"] = "Armor",
					["type"] = 28,
					["hint"] = 2,
					["hint_string"] = $"{4}/{2}:{value}",
					["usage"] = num
				});
			}
			array.Add(new Dictionary
			{
				["name"] = "cell/plantUseCell",
				["type"] = 1,
				["usage"] = num
			});
			if (characterConfig is TowerDefenseZombieConfig)
			{
				array.Add(new Dictionary
				{
					["name"] = "Override/overrideWeight",
					["type"] = 2,
					["usage"] = num
				});
				array.Add(new Dictionary
				{
					["name"] = "Override/overrideWavePointCost",
					["type"] = 2,
					["usage"] = num
				});
			}
		}
		return array;
	}

	public override bool _Set(StringName property, Variant value)
	{
		switch (property.ToString())
		{
		case "eventPress":
			pressedActions = CardActionBehaviorDefinition.ReadLegacyArray(value, CardActionBehaviorTrigger.Pressed);
			return true;
		case "eventPlant":
			useSucceededActions = CardActionBehaviorDefinition.ReadLegacyArray(value, CardActionBehaviorTrigger.UseSucceeded);
			return true;
		case "_override":
			_override = value.As<TowerDefensePacketOverride>();
			return true;
		case "Armor":
			initArmor.Clear();
			foreach (Variant item in value.AsGodotArray())
			{
				initArmor.Add(item.AsString());
			}
			return true;
		case "Override/overrideWeight":
			overrideWeight = value.AsInt32();
			return true;
		case "Override/overrideWavePointCost":
			overrideWavePointCost = value.AsInt32();
			return true;
		case "cell/plantUseCell":
			plantUseCell = value.AsBool();
			return true;
		default:
			return false;
		}
	}

	public override Variant _Get(StringName property)
	{
		return property.ToString() switch
		{
			"Armor" => (Variant)initArmor, 
			"Override/overrideWeight" => overrideWeight, 
			"Override/overrideWavePointCost" => overrideWavePointCost, 
			"cell/plantUseCell" => plantUseCell, 
			_ => default, 
		};
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		switch (property.ToString())
		{
		case "Armor":
		case "Override/overrideWeight":
		case "Override/overrideWavePointCost":
		case "cell/plantUseCell":
			return true;
		default:
			return false;
		}
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		return property.ToString() switch
		{
			"Armor" => (Variant)new Array<string>(), 
			"Override/overrideWeight" => -1, 
			"Override/overrideWavePointCost" => -1, 
			"cell/plantUseCell" => true, 
			_ => default, 
		};
	}

	public TowerDefenseEnum.PACKET_TYPE _GetType()
	{
		if (GodotObject.IsInstanceValid(_override) && _override.type != TowerDefenseEnum.PACKET_TYPE.NOONE)
		{
			return _override.type;
		}
		return type;
	}

	public int GetCostRise()
	{
		if (GodotObject.IsInstanceValid(_override) && _override.costRise != -1)
		{
			return _override.costRise;
		}
		if (overrideCostRise != -1)
		{
			return overrideCostRise;
		}
		if (!GodotObject.IsInstanceValid(characterConfig))
		{
			return -1;
		}
		return characterConfig.costRise;
	}

	public double GetCostMultiple()
	{
		if (GodotObject.IsInstanceValid(_override) && _override.costMultiple != -1.0)
		{
			return _override.costMultiple;
		}
		if (!GodotObject.IsInstanceValid(characterConfig))
		{
			return -1.0;
		}
		return characterConfig.costMultiple;
	}

	public int GetCostBeforeModifiers()
	{
		if (!GodotObject.IsInstanceValid(characterConfig))
		{
			return 0;
		}
		int result = characterConfig.cost;
		if (overrideCost != -1)
		{
			result = overrideCost;
		}
		if (GodotObject.IsInstanceValid(_override) && _override.cost != -1)
		{
			result = _override.cost;
		}
		if (characterConfig.costNight != -1 && TowerDefenseManager.GetMapIsNight())
		{
			result = characterConfig.costNight;
		}
		return result;
	}

	public int GetCost(bool skipGlobalChangeCost = false)
	{
		int costBeforeModifiers = GetCostBeforeModifiers();
		return CardBehaviorDispatcher.ApplyCost(this, costBeforeModifiers, skipGlobalChangeCost);
	}

	public double GetPacketCooldown()
	{
		double num = 1.0;
		if (TowerDefenseManager.IsUnlimitedFire())
		{
			num *= 2.0 / 3.0;
		}
		Variant[] array = coldDownDecreaseDictionary.Keys.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			string text = (string)array[i];
			Array array2 = (Array)((Dictionary)coldDownDecreaseDictionary[text])["ControlCharacterList"];
			for (int j = 0; j < array2.Count; j++)
			{
				for (int k = 0; k < array2.Count; k++)
				{
					if (!GodotObject.IsInstanceValid((GodotObject)array2[k]))
					{
						array2.RemoveAt(k);
						break;
					}
				}
			}
			if (array2.Count <= 0)
			{
				coldDownDecreaseDictionary.Remove(text);
			}
		}
		double num2 = 0.0;
		foreach (Variant key in coldDownDecreaseDictionary.Keys)
		{
			string text2 = (string)key;
			Dictionary dictionary = (Dictionary)coldDownDecreaseDictionary[text2];
			num2 += dictionary["Percentage"].AsDouble();
		}
		if (1.0 + num2 > 0.001)
		{
			num /= 1.0 + num2;
		}
		num = TowerDefenseManager.ApplyMapPacketCooldownRules(_GetType(), num);
		if (num < 0.0)
		{
			num = 0.0;
		}
		if (GodotObject.IsInstanceValid(_override) && _override.packetCooldown != -1.0)
		{
			return _override.packetCooldown * num;
		}
		if (overridePacketCooldown != -1.0)
		{
			return overridePacketCooldown * num;
		}
		if (!GodotObject.IsInstanceValid(characterConfig))
		{
			return 0.0;
		}
		return characterConfig.packetCooldown * num;
	}

	public double GetStartingCooldown()
	{
		double num = 1.0;
		if (TowerDefenseManager.IsUnlimitedFire())
		{
			num *= 2.0 / 3.0;
		}
		if (GodotObject.IsInstanceValid(_override) && _override.startingCooldown != -1.0)
		{
			return _override.startingCooldown * num;
		}
		if (overrideStartingCooldown != -1.0)
		{
			return overrideStartingCooldown * num;
		}
		if (!GodotObject.IsInstanceValid(characterConfig))
		{
			return 0.0;
		}
		return characterConfig.startingCooldown * num;
	}

	public int GetWeight()
	{
		if (GodotObject.IsInstanceValid(_override) && _override.weight != -1)
		{
			return _override.weight;
		}
		if (overrideWeight != -1)
		{
			return overrideWeight;
		}
		return (characterConfig as TowerDefenseZombieConfig)?.weight ?? (-1);
	}

	public int GetWavePointCost()
	{
		if (GodotObject.IsInstanceValid(_override) && _override.wavePointCost != -1)
		{
			return _override.wavePointCost;
		}
		if (overrideWavePointCost != -1)
		{
			return overrideWavePointCost;
		}
		return (characterConfig as TowerDefenseZombieConfig)?.wavePointCost ?? (-1);
	}

	public Array<string> GetPlantCover()
	{
		if (GodotObject.IsInstanceValid(_override) && _override.plantCover != null && _override.plantCover.Count > 0)
		{
			return _override.plantCover;
		}
		if (!GodotObject.IsInstanceValid(characterConfig) || characterConfig.plantCover == null)
		{
			return new Array<string>();
		}
		return characterConfig.plantCover;
	}

	public bool GetCoverCanDirectPlant()
	{
		if (GodotObject.IsInstanceValid(_override))
		{
			return _override.coverCanDirectPlant;
		}
		return false;
	}

	public bool GetHypnoses()
	{
		if (GodotObject.IsInstanceValid(_override) && _override.hypnoses)
		{
			return _override.hypnoses;
		}
		return overrideHypnoses;
	}

	public bool IsLimitGridNum()
	{
		if (GodotObject.IsInstanceValid(_override))
		{
			return _override.islimitGridNum;
		}
		return true;
	}

	private bool TryGetPlantGridOverrideReplaceTarget(TowerDefenseCellInstance cell, TowerDefenseCharacter character, bool noLimit, out TowerDefenseCharacter replaceTarget)
	{
		replaceTarget = null;
		if (noLimit)
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(cell) || !GodotObject.IsInstanceValid(character))
		{
			return false;
		}
		if (!(characterConfig is TowerDefensePlantConfig))
		{
			return false;
		}
		if (GetPlantCover().Count > 0)
		{
			return false;
		}
		if (characterConfig.plantGridOverrideType == TowerDefenseEnum.PLANTGRIDTYPE.NOONE)
		{
			return false;
		}
		if (!ShouldReplacePlantGridOverrideTarget())
		{
			return false;
		}
		cell.ClearEmpty();
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in cell.GetGridType())
		{
			if (cell.slot.ContainsKey(item))
			{
				TowerDefenseCharacter towerDefenseCharacter = cell.slot[item];
				if (CanReplacePlantGridOverrideTarget(towerDefenseCharacter, item))
				{
					replaceTarget = towerDefenseCharacter;
					return true;
				}
			}
		}
		return false;
	}

	private bool ShouldReplacePlantGridOverrideTarget()
	{
		bool flag;
		switch (characterConfig.plantGridOverrideType)
		{
		case TowerDefenseEnum.PLANTGRIDTYPE.NOONE:
			return false;
		case TowerDefenseEnum.PLANTGRIDTYPE.LILYPAD:
		case TowerDefenseEnum.PLANTGRIDTYPE.POT:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			return false;
		}
		return true;
	}

	private bool CanReplacePlantGridOverrideTarget(TowerDefenseCharacter slotCharacter, TowerDefenseEnum.PLANTGRIDTYPE type)
	{
		if (!GodotObject.IsInstanceValid(slotCharacter))
		{
			return false;
		}
		if (slotCharacter is TowerDefenseItemSheild)
		{
			return false;
		}
		if (!(slotCharacter is TowerDefensePlant))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(slotCharacter.instance))
		{
			return false;
		}
		if (slotCharacter.instance.hypnoses != GetHypnoses() || slotCharacter.instance.hologram)
		{
			return false;
		}
		if (!characterConfig.plantGridType.Contains(type))
		{
			return false;
		}
		return slotCharacter.config.plantGridType.Contains(characterConfig.plantGridOverrideType);
	}

	public TowerDefenseCharacter Plant(EconomyAccountId economyOwnerAccountId, Vector2I gridPos, bool playAudio = true, bool noLimit = false, bool skipPlacementCheck = false)
	{
		if (!economyOwnerAccountId.IsValid)
		{
			return null;
		}
		return Plant(gridPos, playAudio, noLimit, economyOwnerAccountId, skipPlacementCheck);
	}

	public TowerDefenseCharacter Plant(Vector2I gridPos, bool playAudio = true, bool noLimit = false, EconomyAccountId economyOwnerAccountId = default(EconomyAccountId), bool skipPlacementCheck = false, bool editorPreviewMode = false)
	{
		bool isEditorPreview = IsEditorPreviewContext(editorPreviewMode);
		TowerDefenseCellInstance cell = TowerDefenseManager.GetMapCell(gridPos);
		if (GodotObject.IsInstanceValid(cell) && GodotObject.IsInstanceValid(cell.FindPlantingBlocker(this)))
		{
			return null;
		}
		if (Global.Instance.isEditor && SceneManager.CurrentScene == "LevelEditorStage" && GodotObject.IsInstanceValid(cell) && !(characterConfig is TowerDefenseVaseConfig) && cell.HasVase())
		{
			((TowerDefenseVase)cell.GetVase()).packetConfig = (TowerDefensePacketConfig)Duplicate(deep: true);
			return null;
		}
		if (!(characterConfig is TowerDefenseZombieConfig) && !GodotObject.IsInstanceValid(cell))
		{
			return null;
		}
		if (skipPlacementCheck && characterConfig is TowerDefensePlantConfig towerDefensePlantConfig)
		{
			foreach (Vector2I item in towerDefensePlantConfig.extendGrid)
			{
				if (!GodotObject.IsInstanceValid(TowerDefenseManager.GetMapCell(gridPos + item)))
				{
					return null;
				}
			}
		}
		if (!(characterConfig is TowerDefenseZombieConfig) && !skipPlacementCheck && !cell.CanPacketPlant(this, noLimit))
		{
			return null;
		}
		string text = characterConfig.name;
		TowerDefenseCharacter character = (isEditorPreview ? TowerDefenseManager.GetChacraterScene(text).Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled) : TowerDefenseManager.Instance.CreateCharacter(text));
		AssignEconomyOwner(character, economyOwnerAccountId);
		if (isEditorPreview)
		{
			character.inGame = false;
			character.editorPreviewMode = true;
			character.editorMapPreviewMode = true;
			character.ProcessMode = Node.ProcessModeEnum.Disabled;
		}
		ApplyInitialArmor(character);
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(gridPos);
		Vector2 vector = ResolvePreparedCharacterMountPosition(characterNode, mapCellPlantPos, isEditorPreview);
		if (isEditorPreview)
		{
			character.SetLogicalGlobalPosition(vector);
		}
		else
		{
			SetPreparedCharacterGlobalPosition(character, vector);
		}
		character.gridPos = gridPos;
		character.cost = characterConfig.cost;
		character.packet = this;
		if (packetFlip)
		{
			character.Scale = new Vector2(0f - character.Scale.X, character.Scale.Y);
		}
		if (GodotObject.IsInstanceValid(cell))
		{
			character.groundHeight = cell.GetGroundHeight();
		}
		character.z = character.groundHeight;
		if (GodotObject.IsInstanceValid(TowerDefenseInGameLevelControl.instance))
		{
			TowerDefenseInGameLevelControl.instance.hasSpawn = true;
		}
		characterNode.CallDeferred("add_child", character);
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(characterNode) && !character.IsQueuedForDeletion() && !characterNode.IsQueuedForDeletion())
			{
				if (GetHypnoses())
				{
					character.Hypnoses();
				}
				if (GodotObject.IsInstanceValid(_override) && GodotObject.IsInstanceValid(_override.characterOverride))
				{
					_override.characterOverride.ExecuteCharacter(character);
				}
				if (GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(characterNode) && !character.IsQueuedForDeletion() && !characterNode.IsQueuedForDeletion())
				{
					if (isEditorPreview)
					{
						character.inGame = false;
						character.editorPreviewMode = true;
						character.ProcessMode = Node.ProcessModeEnum.Disabled;
						character.sprite.ProcessMode = Node.ProcessModeEnum.Always;
						character.sprite.SetAnimation(packetAnimeClip);
						if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
						{
							TowerDefenseManager.Instance.CharacterRegister(character);
						}
					}
					if (character.GetParent() == characterNode)
					{
						int num = -1;
						int num2 = -1;
						for (int i = 0; i < characterNode.GetChildCount(); i++)
						{
							if (characterNode.GetChild(i) is TowerDefenseCharacter towerDefenseCharacter && towerDefenseCharacter != character && towerDefenseCharacter.itemLayer == character.itemLayer && towerDefenseCharacter.gridPos.Y == character.gridPos.Y && towerDefenseCharacter.gridPos.X > num2 && towerDefenseCharacter.gridPos.X <= character.gridPos.X)
							{
								num2 = towerDefenseCharacter.gridPos.X;
								num = i;
							}
						}
						if (num != -1)
						{
							characterNode.MoveChild(character, num);
						}
					}
					if (overrideCost != -1)
					{
						character.cost = characterConfig.cost;
					}
					TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(GD.Load<PackedScene>("uid://du8ukldfc7fh7"), gridPos);
					characterNode.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, Node.InternalMode.Disabled);
					towerDefenseEffectParticlesOnce.GlobalPosition = character.GetLogicalGlobalPosition(character.transformPoint);
					if (plantUseCell && !(characterConfig is TowerDefenseZombieConfig))
					{
						if (TryGetPlantGridOverrideReplaceTarget(cell, character, noLimit, out var replaceTarget))
						{
							cell.CharacterReplace(replaceTarget, character);
						}
						else
						{
							cell.CharacterPlant(this, character, noLimit);
						}
						if (characterConfig is TowerDefensePlantConfig towerDefensePlantConfig2)
						{
							foreach (Vector2I item2 in towerDefensePlantConfig2.extendGrid)
							{
								TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos + item2);
								if (mapCell != cell)
								{
									mapCell.CharacterPlant(this, character, noLimit);
								}
							}
						}
					}
					if (GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) && TowerDefenseManager.CurrentControl.isGameRunning && (!Global.Instance.isEditor || SceneManager.CurrentScene != "LevelEditorStage") && characterConfig is TowerDefenseZombieConfig && character is TowerDefenseZombie character2)
					{
						IsZombieWalk(character2);
					}
					if (playAudio)
					{
						if (GodotObject.IsInstanceValid(cell) && cell.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.WATER))
						{
							AudioManager.Instance.AudioPlay("PlantWater");
						}
						else
						{
							AudioManager.Instance.AudioPlay("Plant");
						}
					}
				}
			}
		}).CallDeferred();
		return character;
	}

	public TowerDefenseCharacter PlantOnZombie(EconomyAccountId economyOwnerAccountId, TowerDefenseCharacter zombie, bool hypnoses = false, bool playAudio = true)
	{
		if (!economyOwnerAccountId.IsValid)
		{
			return null;
		}
		return PlantOnZombie(zombie, hypnoses, playAudio, economyOwnerAccountId);
	}

	public TowerDefenseCharacter PlantOnZombie(TowerDefenseCharacter zombie, bool hypnoses = false, bool playAudio = true, EconomyAccountId economyOwnerAccountId = default(EconomyAccountId))
	{
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return null;
		}
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(zombie.gridPos);
		if (GodotObject.IsInstanceValid(mapCell) && GodotObject.IsInstanceValid(mapCell.FindPlantingBlocker(this)))
		{
			return null;
		}
		if (zombie.camp != ((!hypnoses) ? TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE : TowerDefenseEnum.CHARACTER_CAMP.PLANT))
		{
			return null;
		}
		TowerDefenseCharacter towerDefenseCharacter = TowerDefenseManager.GetChacraterScene(characterConfig.name).Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
		AssignEconomyOwner(towerDefenseCharacter, economyOwnerAccountId);
		ApplyInitialArmor(towerDefenseCharacter);
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		SetPreparedCharacterGlobalPosition(towerDefenseCharacter, zombie.GetLogicalGlobalPosition() / TowerDefenseManager.GetMapFeature().mapControl.GlobalScale.Y);
		towerDefenseCharacter.gridPos = zombie.gridPos;
		towerDefenseCharacter.cost = characterConfig.cost;
		towerDefenseCharacter.packet = this;
		if (packetFlip)
		{
			towerDefenseCharacter.Scale = new Vector2(0f - towerDefenseCharacter.Scale.X, towerDefenseCharacter.Scale.Y);
		}
		if (GodotObject.IsInstanceValid(towerDefenseCharacter.cell))
		{
			towerDefenseCharacter.groundHeight = towerDefenseCharacter.cell.GetGroundHeight();
		}
		towerDefenseCharacter.z = towerDefenseCharacter.groundHeight;
		characterNode.AddChild(towerDefenseCharacter, forceReadableName: false, Node.InternalMode.Disabled);
		if (hypnoses)
		{
			towerDefenseCharacter.Hypnoses();
		}
		if (GodotObject.IsInstanceValid(_override) && GodotObject.IsInstanceValid(_override.characterOverride))
		{
			_override.characterOverride.ExecuteCharacter(towerDefenseCharacter);
		}
		TowerDefensePlant plantChar = towerDefenseCharacter as TowerDefensePlant;
		if (plantChar != null)
		{
			plantChar.targetZombie = zombie;
			towerDefenseCharacter.itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.EFFECT;
			zombie.OnDestroy += (TowerDefenseCharacter c) =>
			{
				plantChar.OnTargetZombieDestroyed();
			};
		}
		else
		{
			TowerDefenseItem itemChar = towerDefenseCharacter as TowerDefenseItem;
			if (itemChar != null)
			{
				itemChar.targetZombie = zombie;
				towerDefenseCharacter.itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.EFFECT;
				zombie.OnDestroy += (TowerDefenseCharacter c) =>
				{
					itemChar.OnTargetZombieDestroyed();
				};
			}
		}
		if (playAudio)
		{
			AudioManager.Instance.AudioPlay("Plant");
		}
		if (GodotObject.IsInstanceValid(TowerDefenseInGameLevelControl.instance))
		{
			TowerDefenseInGameLevelControl.instance.hasSpawn = true;
		}
		return towerDefenseCharacter;
	}

	public TowerDefenseCharacter PlantOnPlant(EconomyAccountId economyOwnerAccountId, TowerDefenseCharacter plant, bool playAudio = true)
	{
		if (!economyOwnerAccountId.IsValid)
		{
			return null;
		}
		return PlantOnPlant(plant, playAudio, economyOwnerAccountId);
	}

	public TowerDefenseCharacter PlantOnPlant(TowerDefenseCharacter plant, bool playAudio = true, EconomyAccountId economyOwnerAccountId = default(EconomyAccountId), bool editorPreviewMode = false)
	{
		if (!GodotObject.IsInstanceValid(plant))
		{
			return null;
		}
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(plant.gridPos);
		if (GodotObject.IsInstanceValid(mapCell) && GodotObject.IsInstanceValid(mapCell.FindPlantingBlocker(this)))
		{
			return null;
		}
		bool hypnoses = GetHypnoses();
		bool flag = IsEditorPreviewContext(editorPreviewMode);
		TowerDefenseCharacter towerDefenseCharacter = TowerDefenseManager.GetChacraterScene(characterConfig.name).Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
		towerDefenseCharacter.InheritCoverSleepState(plant);
		AssignEconomyOwner(towerDefenseCharacter, economyOwnerAccountId);
		if (flag)
		{
			towerDefenseCharacter.inGame = false;
			towerDefenseCharacter.editorPreviewMode = true;
			towerDefenseCharacter.editorMapPreviewMode = true;
			towerDefenseCharacter.ProcessMode = Node.ProcessModeEnum.Disabled;
		}
		ApplyInitialArmor(towerDefenseCharacter);
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		Vector2 mapWorldPosition = (flag ? characterNode.ToGlobal(plant.Position) : plant.GetLogicalGlobalPosition());
		Vector2 vector = ResolvePreparedCharacterMountPosition(characterNode, mapWorldPosition, flag);
		if (flag)
		{
			towerDefenseCharacter.SetLogicalGlobalPosition(vector);
		}
		else
		{
			SetPreparedCharacterGlobalPosition(towerDefenseCharacter, vector);
		}
		towerDefenseCharacter.gridPos = plant.gridPos;
		towerDefenseCharacter.cost = characterConfig.cost;
		towerDefenseCharacter.packet = this;
		if (packetFlip)
		{
			towerDefenseCharacter.Scale = new Vector2(0f - towerDefenseCharacter.Scale.X, towerDefenseCharacter.Scale.Y);
		}
		if (GodotObject.IsInstanceValid(towerDefenseCharacter.cell))
		{
			towerDefenseCharacter.groundHeight = towerDefenseCharacter.cell.GetGroundHeight();
		}
		towerDefenseCharacter.z = towerDefenseCharacter.groundHeight;
		characterNode.AddChild(towerDefenseCharacter, forceReadableName: false, Node.InternalMode.Disabled);
		if (flag)
		{
			towerDefenseCharacter.ProcessMode = Node.ProcessModeEnum.Disabled;
			towerDefenseCharacter.sprite.ProcessMode = Node.ProcessModeEnum.Always;
			towerDefenseCharacter.sprite.SetAnimation(packetAnimeClip);
			if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
			{
				TowerDefenseManager.Instance.CharacterRegister(towerDefenseCharacter);
			}
		}
		if (hypnoses)
		{
			towerDefenseCharacter.Hypnoses();
		}
		if (GodotObject.IsInstanceValid(_override) && GodotObject.IsInstanceValid(_override.characterOverride))
		{
			_override.characterOverride.ExecuteCharacter(towerDefenseCharacter);
		}
		TowerDefensePlant plantChar = towerDefenseCharacter as TowerDefensePlant;
		if (plantChar != null)
		{
			plantChar.targetPlant = plant;
			towerDefenseCharacter.itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.EFFECT;
			plant.OnDestroy += (TowerDefenseCharacter c) =>
			{
				plantChar.OnTargetPlantDestroyed();
			};
		}
		else
		{
			TowerDefenseItem itemChar = towerDefenseCharacter as TowerDefenseItem;
			if (itemChar != null)
			{
				itemChar.targetPlant = plant;
				towerDefenseCharacter.itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.EFFECT;
				plant.OnDestroy += (TowerDefenseCharacter c) =>
				{
					itemChar.OnTargetPlantDestroyed();
				};
			}
		}
		if (playAudio)
		{
			AudioManager.Instance.AudioPlay("Plant");
		}
		if (GodotObject.IsInstanceValid(TowerDefenseInGameLevelControl.instance))
		{
			TowerDefenseInGameLevelControl.instance.hasSpawn = true;
		}
		return towerDefenseCharacter;
	}

	public bool HasSpawnLimit()
	{
		if (characterConfig is TowerDefenseZombieConfig towerDefenseZombieConfig)
		{
			if (towerDefenseZombieConfig.spawnLineNeed.Count > 0)
			{
				return true;
			}
			if (towerDefenseZombieConfig.excludeLineGridType.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool CanSpawn(int line)
	{
		bool result = true;
		if (characterConfig is TowerDefenseZombieConfig towerDefenseZombieConfig)
		{
			foreach (TowerDefenseEnum.PLANTGRIDTYPE item in towerDefenseZombieConfig.spawnLineNeed)
			{
				if (!TowerDefenseManager.MapLineHasType(line, item))
				{
					result = false;
					break;
				}
			}
			foreach (TowerDefenseEnum.PLANTGRIDTYPE item2 in towerDefenseZombieConfig.excludeLineGridType)
			{
				if (TowerDefenseManager.MapLineHasType(line, item2))
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	public TowerDefenseCharacter Spawn(EconomyAccountId economyOwnerAccountId, int line, double offsetX = 0.0, bool isIdle = false)
	{
		if (!economyOwnerAccountId.IsValid)
		{
			return null;
		}
		return Spawn(line, offsetX, isIdle, economyOwnerAccountId);
	}

	internal TowerDefenseCharacter SpawnWaveZombieFromReadOnlyConfig(int line, double offsetX = 0.0, bool isIdle = false)
	{
		if (!(characterConfig is TowerDefenseZombieConfig))
		{
			return null;
		}
		return Spawn(line, offsetX, isIdle);
	}

	public TowerDefenseCharacter Spawn(int line, double offsetX = 0.0, bool isIdle = false, EconomyAccountId economyOwnerAccountId = default(EconomyAccountId))
	{
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		string text = characterConfig.name;
		long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
		long startTicks2 = TowerDefensePerfProfiler.BeginHotPath();
		TowerDefenseManager.GetChacraterScene(text);
		TowerDefensePerfProfiler.End("character.spawn.sceneLookup", startTicks2);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.CharacterSpawnSceneLookup, startBytes2);
		long startBytes3 = TowerDefenseAllocationTelemetry.Begin();
		long startTicks3 = TowerDefensePerfProfiler.BeginHotPath();
		TowerDefenseCharacter towerDefenseCharacter = TowerDefenseManager.Instance.CreateCharacter(text);
		TowerDefensePerfProfiler.End("character.spawn.instantiate", startTicks3);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.CharacterSpawnInstantiate, startBytes3);
		long startBytes4 = TowerDefenseAllocationTelemetry.Begin();
		long startTicks4 = TowerDefensePerfProfiler.BeginHotPath();
		AssignEconomyOwner(towerDefenseCharacter, economyOwnerAccountId);
		ApplyInitialArmor(towerDefenseCharacter);
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		Vector2 globalPosition = new Vector2((float)((double)(mapFeature.config.edge.Z + 40f) + offsetX), (float)TowerDefenseManager.GetMapLineY(line));
		SetPreparedCharacterGlobalPosition(towerDefenseCharacter, globalPosition);
		towerDefenseCharacter.gridPos = new Vector2I(-1, line);
		towerDefenseCharacter.packet = this;
		TowerDefensePerfProfiler.End("character.spawn.initialize", startTicks4);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.CharacterSpawnInitialize, startBytes4);
		long startBytes5 = TowerDefenseAllocationTelemetry.Begin();
		long startTicks5 = TowerDefensePerfProfiler.BeginHotPath();
		characterNode.AddChild(towerDefenseCharacter, forceReadableName: false, Node.InternalMode.Disabled);
		TowerDefensePerfProfiler.End("character.spawn.attachFresh", startTicks5);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.CharacterSpawnAttachFresh, startBytes5);
		long startBytes6 = TowerDefenseAllocationTelemetry.Begin();
		long startTicks6 = TowerDefensePerfProfiler.BeginHotPath();
		if (!isIdle)
		{
			string text2 = spawnMethod;
			if (!(text2 == "Rise"))
			{
				if (text2 == "FlyDown")
				{
					towerDefenseCharacter.isGround = false;
					towerDefenseCharacter.z = 900.0;
				}
			}
			else
			{
				towerDefenseCharacter.Rise();
			}
			towerDefenseCharacter.Spawn();
		}
		if (!isIdle && characterConfig is TowerDefenseZombieConfig && towerDefenseCharacter is TowerDefenseZombie character)
		{
			IsZombieWalk(character);
		}
		RandomAnime(towerDefenseCharacter);
		if (GodotObject.IsInstanceValid(TowerDefenseInGameLevelControl.instance))
		{
			TowerDefenseInGameLevelControl.instance.hasSpawn = true;
		}
		TowerDefensePerfProfiler.End("character.spawn.entry", startTicks6);
		TowerDefensePerfProfiler.End("character.spawn.total", startTicks);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.CharacterSpawnEntry, startBytes6);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.CharacterSpawnTotal, startBytes);
		return towerDefenseCharacter;
	}

	public TowerDefenseCharacter Create(EconomyAccountId economyOwnerAccountId, Vector2 pos, Vector2I gridPos, double height = 0.0)
	{
		if (!economyOwnerAccountId.IsValid)
		{
			return null;
		}
		return Create(pos, gridPos, height, economyOwnerAccountId);
	}

	public TowerDefenseCharacter Create(Vector2 pos, Vector2I gridPos, double height = 0.0, EconomyAccountId economyOwnerAccountId = default(EconomyAccountId))
	{
		string characterName = characterConfig.name;
		TowerDefenseCharacter towerDefenseCharacter = TowerDefenseManager.Instance.CreateCharacter(characterName);
		AssignEconomyOwner(towerDefenseCharacter, economyOwnerAccountId);
		ApplyInitialArmor(towerDefenseCharacter);
		towerDefenseCharacter.z = height;
		SetPreparedCharacterGlobalPosition(towerDefenseCharacter, pos);
		towerDefenseCharacter.gridPos = gridPos;
		towerDefenseCharacter.packet = this;
		if (GodotObject.IsInstanceValid(TowerDefenseInGameLevelControl.instance))
		{
			TowerDefenseInGameLevelControl.instance.hasSpawn = true;
		}
		return towerDefenseCharacter;
	}

	private static void AssignEconomyOwner(TowerDefenseCharacter character, EconomyAccountId economyOwnerAccountId)
	{
		if (economyOwnerAccountId.IsValid)
		{
			character.TryAssignEconomyOwner(economyOwnerAccountId);
		}
	}

	private void ApplyInitialArmor(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character) || !GodotObject.IsInstanceValid(characterConfig?.armorData) || initArmor == null || initArmor.Count == 0)
		{
			return;
		}
		bool flag = GodotObject.IsInstanceValid(character.instance);
		foreach (string item in initArmor)
		{
			if (!string.IsNullOrEmpty(item))
			{
				if (!character.currentArmor.Contains(item))
				{
					character.currentArmor.Add(item);
				}
				if (flag)
				{
					character.instance.ArmorAdd(item);
				}
			}
		}
	}

	private static bool IsEditorPreviewContext(bool explicitlyRequested)
	{
		if (explicitlyRequested || (Global.Instance.isEditor && SceneManager.CurrentScene == "LevelEditorStage"))
		{
			return true;
		}
		LevelEditorMapEditor instance = LevelEditorMapEditor.instance;
		if (GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(instance.mapFeature))
		{
			return instance.mapFeature.editorPreviewMode;
		}
		return false;
	}

	internal static Vector2 ResolvePreparedCharacterMountPosition(Node2D characterNode, Vector2 mapWorldPosition, bool isEditorPreview)
	{
		if (isEditorPreview)
		{
			return characterNode.ToLocal(mapWorldPosition);
		}
		return mapWorldPosition / TowerDefenseManager.GetMapFeature().mapControl.GlobalScale.Y;
	}

	public void RandomAnime(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			character.sprite.frameIndex += GD.RandRange(0, 20);
		}
	}

	public async void IsZombieWalk(TowerDefenseCharacter character)
	{
		await ToSignal(character.GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (!character.die && !character.nearDie && !character.isRise)
		{
			character.CallDeferred("Walk");
		}
	}

	public bool Unlock()
	{
		if (XWModPlayerProgressService.TryPacketUnlock(this, out var unlocked))
		{
			return unlocked;
		}
		if (CommandManager.Instance.debugPacketOpenAll)
		{
			return true;
		}
		Dictionary towerDefensePacketValue = GameSaveManager.Instance.GetTowerDefensePacketValue(saveKey);
		if (!towerDefensePacketValue.GetValueOrDefault("Unlock", false).AsBool())
		{
			if (unlockCheckList == null || unlockCheckList.Count <= 0)
			{
				return false;
			}
			foreach (UnlockConditionBaseConfig unlockCheck in unlockCheckList)
			{
				if (!unlockCheck.Check())
				{
					return false;
				}
			}
			towerDefensePacketValue["Unlock"] = true;
			GameSaveManager.Instance.SetTowerDefensePacketValue(saveKey, towerDefensePacketValue);
			GameSaveManager.Instance.Save();
		}
		return true;
	}

	public void ColdDownDecreaseAdd(TowerDefenseCharacter controlCharacter, string key, double percentage)
	{
		if (!coldDownDecreaseDictionary.ContainsKey(key))
		{
			coldDownDecreaseDictionary[key] = new Dictionary
			{
				["ControlCharacterList"] = new Array(),
				["Percentage"] = percentage
			};
		}
		Array array = (Array)((Dictionary)coldDownDecreaseDictionary[key])["ControlCharacterList"];
		if (!array.Contains(controlCharacter))
		{
			controlCharacter.OnDestroy += (TowerDefenseCharacter c) =>
			{
				ColdDownDecreaseDelete(controlCharacter, key);
			};
			array.Add(controlCharacter);
		}
	}

	public void ColdDownDecreaseDelete(TowerDefenseCharacter controlCharacter, string key)
	{
		if (coldDownDecreaseDictionary.ContainsKey(key))
		{
			Array array = (Array)((Dictionary)coldDownDecreaseDictionary[key])["ControlCharacterList"];
			array.Remove(controlCharacter);
			if (array.Count <= 0)
			{
				coldDownDecreaseDictionary.Remove(key);
			}
		}
	}

	public bool ChangeCostAdd(TowerDefensePacketChangeCost changeCost)
	{
		if (changeCost.key != "")
		{
			foreach (TowerDefensePacketChangeCost changeCost2 in changeCostList)
			{
				if (changeCost2.key == changeCost.key)
				{
					return false;
				}
			}
		}
		changeCostList.Add(changeCost);
		if (changeCost.lockCost)
		{
			canChangeCost = false;
		}
		return true;
	}

	public bool ChangeCostRemove(TowerDefensePacketChangeCost changeCost)
	{
		int num = changeCostList.IndexOf(changeCost);
		if (num == -1)
		{
			return false;
		}
		changeCostList.RemoveAt(num);
		if (changeCost.lockCost)
		{
			bool flag = false;
			foreach (TowerDefensePacketChangeCost changeCost2 in changeCostList)
			{
				if (changeCost2.lockCost)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				canChangeCost = true;
			}
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(36)
		{
			new MethodInfo(MethodName.SetPreparedCharacterGlobalPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSpawnRuntimeCopy, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateRuntimeStateCopy, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName._GetType, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCostRise, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCostMultiple, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCostBeforeModifiers, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCost, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "skipGlobalChangeCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketCooldown, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetStartingCooldown, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetWeight, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetWavePointCost, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPlantCover, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCoverCanDirectPlant, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetHypnoses, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsLimitGridNum, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldReplacePlantGridOverrideTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanReplacePlantGridOverrideTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "slotCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasSpawnLimit, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanSpawn, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnWaveZombieFromReadOnlyConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "offsetX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isIdle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyInitialArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsEditorPreviewContext, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "explicitlyRequested", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePreparedCharacterMountPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "characterNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mapWorldPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isEditorPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RandomAnime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsZombieWalk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Unlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ColdDownDecreaseAdd, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "controlCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ColdDownDecreaseDelete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "controlCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangeCostAdd, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "changeCost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ChangeCostRemove, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "changeCost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetPreparedCharacterGlobalPosition && args.Count == 2)
		{
			SetPreparedCharacterGlobalPosition(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSpawnRuntimeCopy && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(CreateSpawnRuntimeCopy());
			return true;
		}
		if (method == MethodName.CreateRuntimeStateCopy && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(CreateRuntimeStateCopy());
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
		if (method == MethodName._GetType && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEnum.PACKET_TYPE>(_GetType());
			return true;
		}
		if (method == MethodName.GetCostRise && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCostRise());
			return true;
		}
		if (method == MethodName.GetCostMultiple && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetCostMultiple());
			return true;
		}
		if (method == MethodName.GetCostBeforeModifiers && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCostBeforeModifiers());
			return true;
		}
		if (method == MethodName.GetCost && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetCost(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketCooldown && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetPacketCooldown());
			return true;
		}
		if (method == MethodName.GetStartingCooldown && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetStartingCooldown());
			return true;
		}
		if (method == MethodName.GetWeight && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetWeight());
			return true;
		}
		if (method == MethodName.GetWavePointCost && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetWavePointCost());
			return true;
		}
		if (method == MethodName.GetPlantCover && args.Count == 0)
		{
			Array<string> plantCover = GetPlantCover();
			ret = VariantUtils.CreateFromArray(plantCover);
			return true;
		}
		if (method == MethodName.GetCoverCanDirectPlant && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(GetCoverCanDirectPlant());
			return true;
		}
		if (method == MethodName.GetHypnoses && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(GetHypnoses());
			return true;
		}
		if (method == MethodName.IsLimitGridNum && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLimitGridNum());
			return true;
		}
		if (method == MethodName.ShouldReplacePlantGridOverrideTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldReplacePlantGridOverrideTarget());
			return true;
		}
		if (method == MethodName.CanReplacePlantGridOverrideTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanReplacePlantGridOverrideTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.PLANTGRIDTYPE>(in args[1])));
			return true;
		}
		if (method == MethodName.HasSpawnLimit && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSpawnLimit());
			return true;
		}
		if (method == MethodName.CanSpawn && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSpawn(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SpawnWaveZombieFromReadOnlyConfig && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(SpawnWaveZombieFromReadOnlyConfig(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.ApplyInitialArmor && args.Count == 1)
		{
			ApplyInitialArmor(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsEditorPreviewContext && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEditorPreviewContext(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolvePreparedCharacterMountPosition && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ResolvePreparedCharacterMountPosition(VariantUtils.ConvertTo<Node2D>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.RandomAnime && args.Count == 1)
		{
			RandomAnime(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsZombieWalk && args.Count == 1)
		{
			IsZombieWalk(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Unlock && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Unlock());
			return true;
		}
		if (method == MethodName.ColdDownDecreaseAdd && args.Count == 3)
		{
			ColdDownDecreaseAdd(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ColdDownDecreaseDelete && args.Count == 2)
		{
			ColdDownDecreaseDelete(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeCostAdd && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ChangeCostAdd(VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in args[0])));
			return true;
		}
		if (method == MethodName.ChangeCostRemove && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ChangeCostRemove(VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetPreparedCharacterGlobalPosition && args.Count == 2)
		{
			SetPreparedCharacterGlobalPosition(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsEditorPreviewContext && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEditorPreviewContext(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolvePreparedCharacterMountPosition && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ResolvePreparedCharacterMountPosition(VariantUtils.ConvertTo<Node2D>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SetPreparedCharacterGlobalPosition)
		{
			return true;
		}
		if (method == MethodName.CreateSpawnRuntimeCopy)
		{
			return true;
		}
		if (method == MethodName.CreateRuntimeStateCopy)
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
		if (method == MethodName._GetType)
		{
			return true;
		}
		if (method == MethodName.GetCostRise)
		{
			return true;
		}
		if (method == MethodName.GetCostMultiple)
		{
			return true;
		}
		if (method == MethodName.GetCostBeforeModifiers)
		{
			return true;
		}
		if (method == MethodName.GetCost)
		{
			return true;
		}
		if (method == MethodName.GetPacketCooldown)
		{
			return true;
		}
		if (method == MethodName.GetStartingCooldown)
		{
			return true;
		}
		if (method == MethodName.GetWeight)
		{
			return true;
		}
		if (method == MethodName.GetWavePointCost)
		{
			return true;
		}
		if (method == MethodName.GetPlantCover)
		{
			return true;
		}
		if (method == MethodName.GetCoverCanDirectPlant)
		{
			return true;
		}
		if (method == MethodName.GetHypnoses)
		{
			return true;
		}
		if (method == MethodName.IsLimitGridNum)
		{
			return true;
		}
		if (method == MethodName.ShouldReplacePlantGridOverrideTarget)
		{
			return true;
		}
		if (method == MethodName.CanReplacePlantGridOverrideTarget)
		{
			return true;
		}
		if (method == MethodName.HasSpawnLimit)
		{
			return true;
		}
		if (method == MethodName.CanSpawn)
		{
			return true;
		}
		if (method == MethodName.SpawnWaveZombieFromReadOnlyConfig)
		{
			return true;
		}
		if (method == MethodName.ApplyInitialArmor)
		{
			return true;
		}
		if (method == MethodName.IsEditorPreviewContext)
		{
			return true;
		}
		if (method == MethodName.ResolvePreparedCharacterMountPosition)
		{
			return true;
		}
		if (method == MethodName.RandomAnime)
		{
			return true;
		}
		if (method == MethodName.IsZombieWalk)
		{
			return true;
		}
		if (method == MethodName.Unlock)
		{
			return true;
		}
		if (method == MethodName.ColdDownDecreaseAdd)
		{
			return true;
		}
		if (method == MethodName.ColdDownDecreaseDelete)
		{
			return true;
		}
		if (method == MethodName.ChangeCostAdd)
		{
			return true;
		}
		if (method == MethodName.ChangeCostRemove)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.characterConfig)
		{
			characterConfig = VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in value);
			return true;
		}
		if (name == PropertyName.@override)
		{
			@override = VariantUtils.ConvertTo<TowerDefensePacketOverride>(in value);
			return true;
		}
		if (name == PropertyName.saveKey)
		{
			saveKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.unlockCheckList)
		{
			unlockCheckList = VariantUtils.ConvertToArray<UnlockConditionBaseConfig>(in value);
			return true;
		}
		if (name == PropertyName.name)
		{
			this.name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.describe)
		{
			describe = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.plantfood)
		{
			plantfood = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.handbookDescribe)
		{
			handbookDescribe = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.handbookStory)
		{
			handbookStory = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.packetFlip)
		{
			packetFlip = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetAnimeClip)
		{
			packetAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.handbookPacketAnimeOffset)
		{
			handbookPacketAnimeOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.packetAnimeOffset)
		{
			packetAnimeOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.packetAnimeScale)
		{
			packetAnimeScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._characterConfig)
		{
			_characterConfig = VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in value);
			return true;
		}
		if (name == PropertyName.type)
		{
			type = VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in value);
			return true;
		}
		if (name == PropertyName.canChangeCost)
		{
			canChangeCost = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.spawnMethod)
		{
			spawnMethod = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.pressedActions)
		{
			pressedActions = VariantUtils.ConvertToArray<CardActionBehaviorDefinition>(in value);
			return true;
		}
		if (name == PropertyName.useSucceededActions)
		{
			useSucceededActions = VariantUtils.ConvertToArray<CardActionBehaviorDefinition>(in value);
			return true;
		}
		if (name == PropertyName.behaviorIds)
		{
			behaviorIds = VariantUtils.ConvertToArray<StringName>(in value);
			return true;
		}
		if (name == PropertyName.behaviors)
		{
			behaviors = VariantUtils.ConvertToArray<CardBehaviorDefinition>(in value);
			return true;
		}
		if (name == PropertyName._override)
		{
			_override = VariantUtils.ConvertTo<TowerDefensePacketOverride>(in value);
			return true;
		}
		if (name == PropertyName.overrideHypnoses)
		{
			overrideHypnoses = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideCostRise)
		{
			overrideCostRise = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.overrideCost)
		{
			overrideCost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.overridePacketCooldown)
		{
			overridePacketCooldown = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.overrideStartingCooldown)
		{
			overrideStartingCooldown = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.overrideWeight)
		{
			overrideWeight = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.overrideWavePointCost)
		{
			overrideWavePointCost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.initArmor)
		{
			initArmor = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.plantUseCell)
		{
			plantUseCell = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.izmPlantAllCell)
		{
			izmPlantAllCell = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.izmPlantLeft)
		{
			izmPlantLeft = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.disableWhenSunNegative)
		{
			disableWhenSunNegative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.expireSun)
		{
			expireSun = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.canPlaceOnZombie)
		{
			canPlaceOnZombie = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.coldDownDecreaseDictionary)
		{
			coldDownDecreaseDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.characterConfig)
		{
			value = VariantUtils.CreateFrom<TowerDefenseCharacterConfig>(characterConfig);
			return true;
		}
		if (name == PropertyName.@override)
		{
			value = VariantUtils.CreateFrom<TowerDefensePacketOverride>(@override);
			return true;
		}
		if (name == PropertyName.saveKey)
		{
			value = VariantUtils.CreateFrom(in saveKey);
			return true;
		}
		if (name == PropertyName.unlockCheckList)
		{
			value = VariantUtils.CreateFromArray(unlockCheckList);
			return true;
		}
		if (name == PropertyName.name)
		{
			value = VariantUtils.CreateFrom(in this.name);
			return true;
		}
		if (name == PropertyName.describe)
		{
			value = VariantUtils.CreateFrom(in describe);
			return true;
		}
		if (name == PropertyName.plantfood)
		{
			value = VariantUtils.CreateFrom(in plantfood);
			return true;
		}
		if (name == PropertyName.handbookDescribe)
		{
			value = VariantUtils.CreateFrom(in handbookDescribe);
			return true;
		}
		if (name == PropertyName.handbookStory)
		{
			value = VariantUtils.CreateFrom(in handbookStory);
			return true;
		}
		if (name == PropertyName.packetFlip)
		{
			value = VariantUtils.CreateFrom(in packetFlip);
			return true;
		}
		if (name == PropertyName.packetAnimeClip)
		{
			value = VariantUtils.CreateFrom(in packetAnimeClip);
			return true;
		}
		if (name == PropertyName.handbookPacketAnimeOffset)
		{
			value = VariantUtils.CreateFrom(in handbookPacketAnimeOffset);
			return true;
		}
		if (name == PropertyName.packetAnimeOffset)
		{
			value = VariantUtils.CreateFrom(in packetAnimeOffset);
			return true;
		}
		if (name == PropertyName.packetAnimeScale)
		{
			value = VariantUtils.CreateFrom(in packetAnimeScale);
			return true;
		}
		if (name == PropertyName._characterConfig)
		{
			value = VariantUtils.CreateFrom(in _characterConfig);
			return true;
		}
		if (name == PropertyName.type)
		{
			value = VariantUtils.CreateFrom(in type);
			return true;
		}
		if (name == PropertyName.canChangeCost)
		{
			value = VariantUtils.CreateFrom(in canChangeCost);
			return true;
		}
		if (name == PropertyName.spawnMethod)
		{
			value = VariantUtils.CreateFrom(in spawnMethod);
			return true;
		}
		if (name == PropertyName.pressedActions)
		{
			value = VariantUtils.CreateFromArray(pressedActions);
			return true;
		}
		if (name == PropertyName.useSucceededActions)
		{
			value = VariantUtils.CreateFromArray(useSucceededActions);
			return true;
		}
		if (name == PropertyName.behaviorIds)
		{
			value = VariantUtils.CreateFromArray(behaviorIds);
			return true;
		}
		if (name == PropertyName.behaviors)
		{
			value = VariantUtils.CreateFromArray(behaviors);
			return true;
		}
		if (name == PropertyName._override)
		{
			value = VariantUtils.CreateFrom(in _override);
			return true;
		}
		if (name == PropertyName.overrideHypnoses)
		{
			value = VariantUtils.CreateFrom(in overrideHypnoses);
			return true;
		}
		if (name == PropertyName.overrideCostRise)
		{
			value = VariantUtils.CreateFrom(in overrideCostRise);
			return true;
		}
		if (name == PropertyName.overrideCost)
		{
			value = VariantUtils.CreateFrom(in overrideCost);
			return true;
		}
		if (name == PropertyName.overridePacketCooldown)
		{
			value = VariantUtils.CreateFrom(in overridePacketCooldown);
			return true;
		}
		if (name == PropertyName.overrideStartingCooldown)
		{
			value = VariantUtils.CreateFrom(in overrideStartingCooldown);
			return true;
		}
		if (name == PropertyName.overrideWeight)
		{
			value = VariantUtils.CreateFrom(in overrideWeight);
			return true;
		}
		if (name == PropertyName.overrideWavePointCost)
		{
			value = VariantUtils.CreateFrom(in overrideWavePointCost);
			return true;
		}
		if (name == PropertyName.initArmor)
		{
			value = VariantUtils.CreateFromArray(initArmor);
			return true;
		}
		if (name == PropertyName.plantUseCell)
		{
			value = VariantUtils.CreateFrom(in plantUseCell);
			return true;
		}
		if (name == PropertyName.izmPlantAllCell)
		{
			value = VariantUtils.CreateFrom(in izmPlantAllCell);
			return true;
		}
		if (name == PropertyName.izmPlantLeft)
		{
			value = VariantUtils.CreateFrom(in izmPlantLeft);
			return true;
		}
		if (name == PropertyName.disableWhenSunNegative)
		{
			value = VariantUtils.CreateFrom(in disableWhenSunNegative);
			return true;
		}
		if (name == PropertyName.expireSun)
		{
			value = VariantUtils.CreateFrom(in expireSun);
			return true;
		}
		if (name == PropertyName.canPlaceOnZombie)
		{
			value = VariantUtils.CreateFrom(in canPlaceOnZombie);
			return true;
		}
		if (name == PropertyName.coldDownDecreaseDictionary)
		{
			value = VariantUtils.CreateFrom(in coldDownDecreaseDictionary);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.saveKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.unlockCheckList, PropertyHint.TypeString, "24/17:UnlockConditionBaseConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.name, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.describe, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.plantfood, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.handbookDescribe, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.handbookStory, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.packetFlip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.packetAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.handbookPacketAnimeOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.packetAnimeOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.packetAnimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterConfig, PropertyHint.ResourceType, "TowerDefenseCharacterConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.type, PropertyHint.Enum, "NOONE:-1,WHITE:0,GOLD:1,DIAMOND:2,COLOUR:3,STAR:4,ORIGINAL:5,ZOMBIE:6,COVER:7,GRAY:8", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canChangeCost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Spawn", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.spawnMethod, PropertyHint.Enum, "Noone,Rise,FlyDown", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Action Behavior", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.pressedActions, PropertyHint.TypeString, "24/17:CardActionBehaviorDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.useSucceededActions, PropertyHint.TypeString, "24/17:CardActionBehaviorDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Behavior", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.behaviorIds, PropertyHint.TypeString, "21/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.behaviors, PropertyHint.TypeString, "24/17:CardBehaviorDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Override", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._override, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.@override, PropertyHint.ResourceType, "TowerDefensePacketOverride", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideHypnoses, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.overrideCostRise, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.overrideCost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.overridePacketCooldown, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.overrideStartingCooldown, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.overrideWeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.overrideWavePointCost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Other", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.initArmor, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.plantUseCell, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.izmPlantAllCell, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.izmPlantLeft, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.disableWhenSunNegative, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.expireSun, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "ZombiePlace", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canPlaceOnZombie, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.coldDownDecreaseDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.characterConfig, Variant.From<TowerDefenseCharacterConfig>(characterConfig));
		info.AddProperty(PropertyName.@override, Variant.From<TowerDefensePacketOverride>(@override));
		info.AddProperty(PropertyName.saveKey, Variant.From(in saveKey));
		info.AddProperty(PropertyName.unlockCheckList, Variant.CreateFrom(unlockCheckList));
		info.AddProperty(PropertyName.name, Variant.From(in name));
		info.AddProperty(PropertyName.describe, Variant.From(in describe));
		info.AddProperty(PropertyName.plantfood, Variant.From(in plantfood));
		info.AddProperty(PropertyName.handbookDescribe, Variant.From(in handbookDescribe));
		info.AddProperty(PropertyName.handbookStory, Variant.From(in handbookStory));
		info.AddProperty(PropertyName.packetFlip, Variant.From(in packetFlip));
		info.AddProperty(PropertyName.packetAnimeClip, Variant.From(in packetAnimeClip));
		info.AddProperty(PropertyName.handbookPacketAnimeOffset, Variant.From(in handbookPacketAnimeOffset));
		info.AddProperty(PropertyName.packetAnimeOffset, Variant.From(in packetAnimeOffset));
		info.AddProperty(PropertyName.packetAnimeScale, Variant.From(in packetAnimeScale));
		info.AddProperty(PropertyName._characterConfig, Variant.From(in _characterConfig));
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName.canChangeCost, Variant.From(in canChangeCost));
		info.AddProperty(PropertyName.spawnMethod, Variant.From(in spawnMethod));
		info.AddProperty(PropertyName.pressedActions, Variant.CreateFrom(pressedActions));
		info.AddProperty(PropertyName.useSucceededActions, Variant.CreateFrom(useSucceededActions));
		info.AddProperty(PropertyName.behaviorIds, Variant.CreateFrom(behaviorIds));
		info.AddProperty(PropertyName.behaviors, Variant.CreateFrom(behaviors));
		info.AddProperty(PropertyName._override, Variant.From(in _override));
		info.AddProperty(PropertyName.overrideHypnoses, Variant.From(in overrideHypnoses));
		info.AddProperty(PropertyName.overrideCostRise, Variant.From(in overrideCostRise));
		info.AddProperty(PropertyName.overrideCost, Variant.From(in overrideCost));
		info.AddProperty(PropertyName.overridePacketCooldown, Variant.From(in overridePacketCooldown));
		info.AddProperty(PropertyName.overrideStartingCooldown, Variant.From(in overrideStartingCooldown));
		info.AddProperty(PropertyName.overrideWeight, Variant.From(in overrideWeight));
		info.AddProperty(PropertyName.overrideWavePointCost, Variant.From(in overrideWavePointCost));
		info.AddProperty(PropertyName.initArmor, Variant.CreateFrom(initArmor));
		info.AddProperty(PropertyName.plantUseCell, Variant.From(in plantUseCell));
		info.AddProperty(PropertyName.izmPlantAllCell, Variant.From(in izmPlantAllCell));
		info.AddProperty(PropertyName.izmPlantLeft, Variant.From(in izmPlantLeft));
		info.AddProperty(PropertyName.disableWhenSunNegative, Variant.From(in disableWhenSunNegative));
		info.AddProperty(PropertyName.expireSun, Variant.From(in expireSun));
		info.AddProperty(PropertyName.canPlaceOnZombie, Variant.From(in canPlaceOnZombie));
		info.AddProperty(PropertyName.coldDownDecreaseDictionary, Variant.From(in coldDownDecreaseDictionary));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.characterConfig, out var value))
		{
			characterConfig = value.As<TowerDefenseCharacterConfig>();
		}
		if (info.TryGetProperty(PropertyName.@override, out var value2))
		{
			@override = value2.As<TowerDefensePacketOverride>();
		}
		if (info.TryGetProperty(PropertyName.saveKey, out var value3))
		{
			saveKey = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.unlockCheckList, out var value4))
		{
			unlockCheckList = value4.AsGodotArray<UnlockConditionBaseConfig>();
		}
		if (info.TryGetProperty(PropertyName.name, out var value5))
		{
			name = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.describe, out var value6))
		{
			describe = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.plantfood, out var value7))
		{
			plantfood = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.handbookDescribe, out var value8))
		{
			handbookDescribe = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.handbookStory, out var value9))
		{
			handbookStory = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.packetFlip, out var value10))
		{
			packetFlip = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetAnimeClip, out var value11))
		{
			packetAnimeClip = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.handbookPacketAnimeOffset, out var value12))
		{
			handbookPacketAnimeOffset = value12.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.packetAnimeOffset, out var value13))
		{
			packetAnimeOffset = value13.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.packetAnimeScale, out var value14))
		{
			packetAnimeScale = value14.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._characterConfig, out var value15))
		{
			_characterConfig = value15.As<TowerDefenseCharacterConfig>();
		}
		if (info.TryGetProperty(PropertyName.type, out var value16))
		{
			type = value16.As<TowerDefenseEnum.PACKET_TYPE>();
		}
		if (info.TryGetProperty(PropertyName.canChangeCost, out var value17))
		{
			canChangeCost = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.spawnMethod, out var value18))
		{
			spawnMethod = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName.pressedActions, out var value19))
		{
			pressedActions = value19.AsGodotArray<CardActionBehaviorDefinition>();
		}
		if (info.TryGetProperty(PropertyName.useSucceededActions, out var value20))
		{
			useSucceededActions = value20.AsGodotArray<CardActionBehaviorDefinition>();
		}
		if (info.TryGetProperty(PropertyName.behaviorIds, out var value21))
		{
			behaviorIds = value21.AsGodotArray<StringName>();
		}
		if (info.TryGetProperty(PropertyName.behaviors, out var value22))
		{
			behaviors = value22.AsGodotArray<CardBehaviorDefinition>();
		}
		if (info.TryGetProperty(PropertyName._override, out var value23))
		{
			_override = value23.As<TowerDefensePacketOverride>();
		}
		if (info.TryGetProperty(PropertyName.overrideHypnoses, out var value24))
		{
			overrideHypnoses = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideCostRise, out var value25))
		{
			overrideCostRise = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName.overrideCost, out var value26))
		{
			overrideCost = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName.overridePacketCooldown, out var value27))
		{
			overridePacketCooldown = value27.As<double>();
		}
		if (info.TryGetProperty(PropertyName.overrideStartingCooldown, out var value28))
		{
			overrideStartingCooldown = value28.As<double>();
		}
		if (info.TryGetProperty(PropertyName.overrideWeight, out var value29))
		{
			overrideWeight = value29.As<int>();
		}
		if (info.TryGetProperty(PropertyName.overrideWavePointCost, out var value30))
		{
			overrideWavePointCost = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName.initArmor, out var value31))
		{
			initArmor = value31.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.plantUseCell, out var value32))
		{
			plantUseCell = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.izmPlantAllCell, out var value33))
		{
			izmPlantAllCell = value33.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.izmPlantLeft, out var value34))
		{
			izmPlantLeft = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.disableWhenSunNegative, out var value35))
		{
			disableWhenSunNegative = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.expireSun, out var value36))
		{
			expireSun = value36.As<int>();
		}
		if (info.TryGetProperty(PropertyName.canPlaceOnZombie, out var value37))
		{
			canPlaceOnZombie = value37.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.coldDownDecreaseDictionary, out var value38))
		{
			coldDownDecreaseDictionary = value38.As<Dictionary>();
		}
	}
}
