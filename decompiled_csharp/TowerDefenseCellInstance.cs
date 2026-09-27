using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/Resource/Cell/TowerDefenseCellInstance.cs")]
public class TowerDefenseCellInstance : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName TryBeginPurify = "TryBeginPurify";

		public static readonly StringName CancelPurify = "CancelPurify";

		public static readonly StringName Init = "Init";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName ClearEmpty = "ClearEmpty";

		public static readonly StringName RemoveInvalidCharacterSlotKeys = "RemoveInvalidCharacterSlotKeys";

		public static readonly StringName CharacterPlant = "CharacterPlant";

		public static readonly StringName CharacterReplace = "CharacterReplace";

		public static readonly StringName CharacterDestroy = "CharacterDestroy";

		public static readonly StringName ScheduleSleepEnvironmentRefresh = "ScheduleSleepEnvironmentRefresh";

		public static readonly StringName ReevaluateCharacterSleepStates = "ReevaluateCharacterSleepStates";

		public static readonly StringName GetSlotCharacterList = "GetSlotCharacterList";

		public static readonly StringName HasSlotCharacterList = "HasSlotCharacterList";

		public static readonly StringName GetCharacterListSave = "GetCharacterListSave";

		public static readonly StringName IsCurrentItemShield = "IsCurrentItemShield";

		public static readonly StringName CanUseExtendedGridOccupantAsSlot = "CanUseExtendedGridOccupantAsSlot";

		public static readonly StringName GetGridType = "GetGridType";

		public static readonly StringName GetCharacterWhoHasGridType = "GetCharacterWhoHasGridType";

		public static readonly StringName FindPlantingBlocker = "FindPlantingBlocker";

		public static readonly StringName NotifyBlockedPlanting = "NotifyBlockedPlanting";

		public static readonly StringName CanNutBandageSurround = "CanNutBandageSurround";

		public static readonly StringName CanPacketPlant = "CanPacketPlant";

		public static readonly StringName CanMoveCharacterHere = "CanMoveCharacterHere";

		public static readonly StringName RemoveCharacter = "RemoveCharacter";

		public static readonly StringName ReleaseHologramGridOccupancy = "ReleaseHologramGridOccupancy";

		public static readonly StringName HasPendingRandomGravestonePlacement = "HasPendingRandomGravestonePlacement";

		public static readonly StringName ReserveRandomGravestonePlacementUntilDeferredBind = "ReserveRandomGravestonePlacementUntilDeferredBind";

		public static readonly StringName CanShovel = "CanShovel";

		public static readonly StringName IsShovelable = "IsShovelable";

		public static readonly StringName GetShovelCharacter = "GetShovelCharacter";

		public static readonly StringName Shovel = "Shovel";

		public static readonly StringName IsValidTarget = "IsValidTarget";

		public static readonly StringName GetTarget = "GetTarget";

		public static readonly StringName GetSlot = "GetSlot";

		public static readonly StringName GetRepairableCrater = "GetRepairableCrater";

		public static readonly StringName GetSurround = "GetSurround";

		public static readonly StringName FindSlotParent = "FindSlotParent";

		public static readonly StringName HasPhysiqueType = "HasPhysiqueType";

		public static readonly StringName HasWallnut = "HasWallnut";

		public static readonly StringName HasVase = "HasVase";

		public static readonly StringName HasLight = "HasLight";

		public static readonly StringName HasCoffee = "HasCoffee";

		public static readonly StringName HasSpike = "HasSpike";

		public static readonly StringName HasCharacter = "HasCharacter";

		public static readonly StringName GetCharacterryPhysiqueType = "GetCharacterryPhysiqueType";

		public static readonly StringName GetVase = "GetVase";

		public static readonly StringName GetSpike = "GetSpike";

		public static readonly StringName CheckWater = "CheckWater";

		public static readonly StringName CanCraterCreate = "CanCraterCreate";

		public static readonly StringName GetGroundHeight = "GetGroundHeight";

		public static readonly StringName HasPlant = "HasPlant";

		public static readonly StringName CanMowerMove = "CanMowerMove";

		public static readonly StringName CanMoveToCell = "CanMoveToCell";

		public static readonly StringName CreateCharacterCellMoveTween = "CreateCharacterCellMoveTween";

		public static readonly StringName MoveCharacterToCell = "MoveCharacterToCell";

		public static readonly StringName MoveToCell = "MoveToCell";

		public static readonly StringName AttackDeal = "AttackDeal";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName gridType = "gridType";

		public static readonly StringName elementFlags = "elementFlags";

		public static readonly StringName isWater = "isWater";

		public static readonly StringName characterSurround = "characterSurround";

		public static readonly StringName characterLadder = "characterLadder";

		public static readonly StringName itemShield = "itemShield";

		public static readonly StringName smashAbsorbedFrame = "smashAbsorbedFrame";

		public static readonly StringName gridPos = "gridPos";

		public static readonly StringName groundHeightCurve = "groundHeightCurve";

		public static readonly StringName config = "config";

		public static readonly StringName dirty = "dirty";

		public static readonly StringName _sleepEnvironmentRefreshScheduled = "_sleepEnvironmentRefreshScheduled";

		public static readonly StringName _pendingRandomGravestonePlacement = "_pendingRandomGravestonePlacement";

		public static readonly StringName _transformPending = "_transformPending";

		public static readonly StringName _purifyPending = "_purifyPending";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public static readonly List<TowerDefenseEnum.PLANTGRIDTYPE> SURROUND_INCLUDE_GRID = new List<TowerDefenseEnum.PLANTGRIDTYPE>
	{
		TowerDefenseEnum.PLANTGRIDTYPE.SOIL,
		TowerDefenseEnum.PLANTGRIDTYPE.BRICK,
		TowerDefenseEnum.PLANTGRIDTYPE.WATER
	};

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseEnum.PLANTGRIDTYPE> gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
	{
		TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
		TowerDefenseEnum.PLANTGRIDTYPE.AIR
	};

	[Export(PropertyHint.None, "")]
	public int elementFlags;

	[Export(PropertyHint.None, "")]
	public bool isWater;

	public List<TowerDefenseCharacter> characterList = new List<TowerDefenseCharacter>();

	public System.Collections.Generic.Dictionary<TowerDefenseCharacter, TowerDefenseCharacter> characterSlotDictionary = new System.Collections.Generic.Dictionary<TowerDefenseCharacter, TowerDefenseCharacter>();

	public System.Collections.Generic.Dictionary<TowerDefenseEnum.PLANTGRIDTYPE, TowerDefenseCharacter> slot = new System.Collections.Generic.Dictionary<TowerDefenseEnum.PLANTGRIDTYPE, TowerDefenseCharacter>();

	public TowerDefenseCharacter characterSurround;

	public TowerDefenseCharacter characterLadder;

	public TowerDefenseItemSheild itemShield;

	public ulong smashAbsorbedFrame = 18446744073709551615uL;

	public Vector2I gridPos = Vector2I.Zero;

	public CurveTexture groundHeightCurve;

	public TowerDefenseCellConfig config;

	public bool dirty;

	private bool _sleepEnvironmentRefreshScheduled;

	private bool _pendingRandomGravestonePlacement;

	private bool _transformPending;

	private bool _purifyPending;

	public bool TryBeginPurify()
	{
		if (_purifyPending)
		{
			return false;
		}
		_purifyPending = true;
		Callable.From(() => _purifyPending = false).CallDeferred();
		return true;
	}

	public void CancelPurify()
	{
		_purifyPending = false;
	}

	public void Init(TowerDefenseCellConfig _config)
	{
		config = _config;
		gridType = config.gridType.Duplicate(deep: true);
		elementFlags = config.ElementFlags;
		isWater = gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.WATER);
		characterList.Clear();
		characterSlotDictionary.Clear();
		slot.Clear();
		_pendingRandomGravestonePlacement = false;
		groundHeightCurve = config.groundHeightCurve;
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in gridType)
		{
			slot[item] = null;
		}
	}

	public void Clear(bool suppressDeathrattles = false)
	{
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
		for (int i = 0; i < characterList.Count; i++)
		{
			list.Add(characterList[i]);
		}
		foreach (TowerDefenseCharacter item in list)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.suppressDeathrattles |= suppressDeathrattles;
				item.ClearFromMap();
			}
		}
		foreach (TowerDefenseEnum.PLANTGRIDTYPE key in slot.Keys)
		{
			if (GodotObject.IsInstanceValid(slot[key]))
			{
				slot[key].suppressDeathrattles |= suppressDeathrattles;
				slot[key].ClearFromMap();
			}
			slot[key] = null;
		}
		characterList.Clear();
		characterSlotDictionary.Clear();
		_pendingRandomGravestonePlacement = false;
		if (GodotObject.IsInstanceValid(characterSurround))
		{
			characterSurround.suppressDeathrattles |= suppressDeathrattles;
			characterSurround.Destroy();
		}
		if (GodotObject.IsInstanceValid(characterLadder))
		{
			characterLadder.suppressDeathrattles |= suppressDeathrattles;
			characterLadder.Destroy();
		}
		dirty = false;
	}

	public void ClearEmpty()
	{
		if (!dirty)
		{
			return;
		}
		dirty = false;
		for (int num = characterList.Count - 1; num >= 0; num--)
		{
			if (!GodotObject.IsInstanceValid(characterList[num]))
			{
				characterList.RemoveAt(num);
			}
		}
		RemoveInvalidCharacterSlotKeys();
		if (!GodotObject.IsInstanceValid(characterSurround))
		{
			characterSurround = null;
		}
		if (!GodotObject.IsInstanceValid(characterLadder))
		{
			characterLadder = null;
		}
		if (!GodotObject.IsInstanceValid(itemShield))
		{
			itemShield = null;
		}
	}

	private void RemoveInvalidCharacterSlotKeys()
	{
		while (true)
		{
			TowerDefenseCharacter key = null;
			bool flag = false;
			foreach (TowerDefenseCharacter key2 in characterSlotDictionary.Keys)
			{
				if (!GodotObject.IsInstanceValid(key2))
				{
					key = key2;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
			characterSlotDictionary.Remove(key);
		}
	}

	public void CharacterPlant(TowerDefensePacketConfig packetConfig, TowerDefenseCharacter character, bool noLimit = false)
	{
		ClearEmpty();
		ScheduleSleepEnvironmentRefresh();
		bool flag = GameSaveManager.Instance.GetFeatureValue("NutBandaging") > 0;
		bool flag2 = GameSaveManager.Instance.GetFeatureValue("PotReplacement") > 0;
		TowerDefenseCharacterConfig characterConfig = packetConfig.characterConfig;
		if (characterConfig is TowerDefenseItemConfig)
		{
			if (((TowerDefenseItemConfig)characterConfig).isLadder && HasWallnut())
			{
				characterLadder = character;
				characterList.Add(character);
				characterSlotDictionary[character] = null;
				character.OnDestroy += CharacterDestroy;
				return;
			}
			if (((TowerDefenseItemConfig)characterConfig).isShield && character is TowerDefenseItemSheild towerDefenseItemSheild)
			{
				if (GodotObject.IsInstanceValid(itemShield))
				{
					itemShield.ShieldAddHitpoints(towerDefenseItemSheild.shieldHitpoints, towerDefenseItemSheild.shieldType);
					towerDefenseItemSheild.QueueFree();
					return;
				}
				itemShield = towerDefenseItemSheild;
				characterList.Add(character);
				characterSlotDictionary[character] = null;
				character.OnDestroy += CharacterDestroy;
				return;
			}
		}
		if (packetConfig.GetPlantCover().Count > 0 && !noLimit)
		{
			foreach (TowerDefenseCharacter character2 in characterList)
			{
				if (!GodotObject.IsInstanceValid(character2) || !packetConfig.GetPlantCover().Contains(character2.config.name))
				{
					continue;
				}
				if (!Global.IsEditor || SceneManager.CurrentScene != "LevelEditorStage")
				{
					int num = packetConfig.GetPlantCover().IndexOf(character2.config.name);
					int num2 = ((characterConfig.plantCoverRecycle.Count > num) ? characterConfig.plantCoverRecycle[num] : 0);
					if (num2 != 0)
					{
						bool flag3 = GodotObject.IsInstanceValid(character2.instance) && character2.instance.hypnoses && character2 is TowerDefensePlant;
						bool flag4 = ((num2 < 0) ? (!flag3) : flag3);
						int num3 = Mathf.Abs(num2);
						Vector2 velocity = new Vector2((float)GD.RandRange(-50.0, 50.0), -400f);
						if (flag4)
						{
							character.BrainSunCreate(character.GetLogicalGlobalPosition(), num3, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, velocity);
						}
						else
						{
							character.SunCreate(character.GetLogicalGlobalPosition(), num3, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, velocity);
						}
					}
				}
				CharacterReplace(character2, character);
				return;
			}
		}
		if (characterConfig is TowerDefensePlantConfig && ((TowerDefensePlantConfig)characterConfig).extendCoverDictionary.Keys.Count > 0)
		{
			bool flag5 = false;
			foreach (Vector2I key in ((TowerDefensePlantConfig)characterConfig).extendCoverDictionary.Keys)
			{
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos + key);
				if (!GodotObject.IsInstanceValid(mapCell))
				{
					continue;
				}
				foreach (TowerDefenseCharacter character3 in mapCell.characterList)
				{
					if (GodotObject.IsInstanceValid(character3) && character3.config.name == ((TowerDefensePlantConfig)characterConfig).extendCoverDictionary[key])
					{
						if (key == Vector2I.Zero)
						{
							mapCell.CharacterReplace(character3, character);
							flag5 = true;
						}
						else
						{
							character3.Destroy();
						}
						break;
					}
				}
			}
			if (flag5)
			{
				return;
			}
		}
		if (packetConfig.characterConfig.plantCoverSelf)
		{
			foreach (TowerDefenseCharacter character4 in characterList)
			{
				if (character4.config.name == packetConfig.characterConfig.name)
				{
					CharacterReplace(character4, character);
					return;
				}
			}
		}
		if (characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.SURROUND) && !GodotObject.IsInstanceValid(characterSurround))
		{
			characterSurround = character;
			characterList.Add(character);
			characterSlotDictionary[character] = null;
			character.OnDestroy += CharacterDestroy;
			return;
		}
		if (flag && (characterConfig.physiqueTypeFlags & 1) != 0)
		{
			foreach (TowerDefenseCharacter character5 in characterList)
			{
				if (GodotObject.IsInstanceValid(character5) && !(characterConfig.name != character5.config.name) && (character5.instance.physiqueTypeFlags & 1) != 0 && character5.instance.damagePointIndex > 1)
				{
					CharacterReplace(character5, character);
					return;
				}
			}
		}
		if (flag2)
		{
			if ((characterConfig.physiqueTypeFlags & 2) != 0)
			{
				foreach (TowerDefenseCharacter character6 in characterList)
				{
					if (GodotObject.IsInstanceValid(character6) && !(characterConfig.name == character6.config.name) && (character6.instance.physiqueTypeFlags & 2) != 0 && ((character6.instance.physiqueTypeFlags & 0x200) == 0 || !isWater))
					{
						CharacterReplace(character6, character);
						return;
					}
				}
			}
			if ((characterConfig.physiqueTypeFlags & 4) != 0)
			{
				foreach (TowerDefenseCharacter character7 in characterList)
				{
					if (GodotObject.IsInstanceValid(character7) && !(characterConfig.name == character7.config.name) && (character7.instance.physiqueTypeFlags & 4) != 0)
					{
						CharacterReplace(character7, character);
						return;
					}
				}
			}
		}
		characterList.Add(character);
		bool flag6 = false;
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in gridType)
		{
			if (slot.ContainsKey(item) && GodotObject.IsInstanceValid(slot[item]) && !IsCurrentItemShield(slot[item]) && characterConfig.plantGridType.Contains(item))
			{
				TowerDefenseCharacter towerDefenseCharacter = slot[item];
				if (towerDefenseCharacter.config.plantGridType.Contains(characterConfig.plantGridOverrideType))
				{
					slot[item] = character;
					characterSlotDictionary[character] = towerDefenseCharacter;
					flag6 = true;
					break;
				}
			}
		}
		if (!flag6)
		{
			characterSlotDictionary[character] = null;
		}
		character.OnDestroy += CharacterDestroy;
		if (flag6)
		{
			return;
		}
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item2 in gridType)
		{
			if (characterConfig.plantGridType.Contains(item2) && slot.ContainsKey(item2) && !GodotObject.IsInstanceValid(slot[item2]))
			{
				slot[item2] = character;
				return;
			}
		}
		TowerDefenseCharacter characterWhoHasGridType = GetCharacterWhoHasGridType(packetConfig);
		if (GodotObject.IsInstanceValid(characterWhoHasGridType))
		{
			if (characterWhoHasGridType is TowerDefenseCrater)
			{
				characterSlotDictionary[character] = characterWhoHasGridType;
			}
			else
			{
				characterSlotDictionary[characterWhoHasGridType] = character;
			}
		}
	}

	public void CharacterReplace(TowerDefenseCharacter character, TowerDefenseCharacter replaceCharacter)
	{
		ClearEmpty();
		replaceCharacter.InheritCoverSleepState(character);
		replaceCharacter.OnDestroy += CharacterDestroy;
		characterList.Add(replaceCharacter);
		characterSlotDictionary[replaceCharacter] = (characterSlotDictionary.ContainsKey(character) ? characterSlotDictionary[character] : null);
		bool flag = false;
		if (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage")
		{
			flag = true;
		}
		if (characterSurround == character)
		{
			characterSurround = replaceCharacter;
			replaceCharacter.Cover(character);
		}
		foreach (TowerDefenseCharacter key in characterSlotDictionary.Keys)
		{
			if (characterSlotDictionary[key] == character)
			{
				if (!flag)
				{
					replaceCharacter.Cover(character);
					flag = true;
				}
				characterSlotDictionary[key] = replaceCharacter;
			}
		}
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in gridType)
		{
			if (slot.ContainsKey(item) && GodotObject.IsInstanceValid(slot[item]) && slot[item] == character)
			{
				if (!flag)
				{
					replaceCharacter.Cover(character);
					flag = true;
				}
				slot[item] = replaceCharacter;
				break;
			}
		}
		if (GodotObject.IsInstanceValid(character.instance))
		{
			character.instance.canBeCollection = false;
		}
		character.isShovel = true;
		character.Destroy();
	}

	public void CharacterDestroy(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return;
		}
		if (characterSlotDictionary.ContainsKey(character) && GodotObject.IsInstanceValid(characterSlotDictionary[character]))
		{
			foreach (TowerDefenseEnum.PLANTGRIDTYPE item in gridType)
			{
				if (characterSlotDictionary[character].config.plantGridType.Contains(item) && (!GodotObject.IsInstanceValid(slot[item]) || slot[item] == character))
				{
					slot[item] = characterSlotDictionary[character];
					break;
				}
			}
		}
		if ((character.instance.physiqueTypeFlags & 1) != 0 && GodotObject.IsInstanceValid(characterLadder))
		{
			characterLadder.Destroy();
		}
		foreach (TowerDefenseCharacter key in characterSlotDictionary.Keys)
		{
			if (!GodotObject.IsInstanceValid(characterSlotDictionary[key]) || characterSlotDictionary[key] == character)
			{
				characterSlotDictionary[key] = null;
			}
		}
		foreach (TowerDefenseEnum.PLANTGRIDTYPE key2 in slot.Keys)
		{
			if (!GodotObject.IsInstanceValid(slot[key2]) || slot[key2] == character)
			{
				slot[key2] = null;
			}
		}
		characterList.Remove(character);
		characterSlotDictionary.Remove(character);
		if (characterSurround == character)
		{
			characterSurround = null;
		}
		if (characterLadder == character)
		{
			characterLadder = null;
		}
		if (itemShield == character)
		{
			itemShield = null;
		}
		dirty = true;
		ScheduleSleepEnvironmentRefresh();
	}

	private void ScheduleSleepEnvironmentRefresh()
	{
		if (!_sleepEnvironmentRefreshScheduled)
		{
			_sleepEnvironmentRefreshScheduled = true;
			Callable.From(() =>
			{
				_sleepEnvironmentRefreshScheduled = false;
				ReevaluateCharacterSleepStates();
			}).CallDeferred();
		}
	}

	private void ReevaluateCharacterSleepStates()
	{
		ClearEmpty();
		for (int i = 0; i < characterList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = characterList[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				(towerDefenseCharacter.componentManager?.GetRuntime<SleepComponent>("character.sleep"))?.ReevaluateEnvironmentState();
			}
		}
	}

	public Array<TowerDefenseCharacter> GetSlotCharacterList()
	{
		ClearEmpty();
		Array<TowerDefenseCharacter> array = new Array<TowerDefenseCharacter>();
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in SURROUND_INCLUDE_GRID)
		{
			if (gridType.Contains(item) && slot.ContainsKey(item) && GodotObject.IsInstanceValid(slot[item]))
			{
				array.Add(slot[item]);
			}
		}
		return array;
	}

	public bool HasSlotCharacterList()
	{
		return GetSlotCharacterList().Count > 0;
	}

	public Array<TowerDefenseCharacter> GetCharacterListSave(bool checkSlot = true)
	{
		ClearEmpty();
		Array<TowerDefenseCharacter> array = new Array<TowerDefenseCharacter>();
		Array<TowerDefenseCharacter> array2 = new Array<TowerDefenseCharacter>();
		if (checkSlot)
		{
			foreach (TowerDefenseCharacter slotCharacter in GetSlotCharacterList())
			{
				array2.Add(slotCharacter);
			}
		}
		else
		{
			foreach (TowerDefenseCharacter slotCharacter2 in GetSlotCharacterList())
			{
				array.Add(slotCharacter2);
			}
		}
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (!array.Contains(character) && !array2.Contains(character) && character != characterLadder && character != characterSurround)
			{
				array2.Add(character);
			}
		}
		if (GodotObject.IsInstanceValid(characterSurround))
		{
			array2.Add(characterSurround);
		}
		if (GodotObject.IsInstanceValid(characterLadder))
		{
			array2.Add(characterLadder);
		}
		return array2;
	}

	public List<TowerDefenseCharacter> GetCharacterList()
	{
		return characterList;
	}

	private bool IsCurrentItemShield(TowerDefenseCharacter character)
	{
		if (!(character is TowerDefenseItemSheild))
		{
			if (GodotObject.IsInstanceValid(itemShield))
			{
				return character == itemShield;
			}
			return false;
		}
		return true;
	}

	private bool CanUseExtendedGridOccupantAsSlot(TowerDefensePacketConfig packetConfig, TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character) || IsCurrentItemShield(character))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(character.instance))
		{
			return false;
		}
		TowerDefenseCharacterConfig characterConfig = packetConfig.characterConfig;
		if (characterConfig.plantGridOverrideType == TowerDefenseEnum.PLANTGRIDTYPE.NOONE)
		{
			return false;
		}
		if (character.instance.hypnoses != packetConfig.GetHypnoses() || character.instance.hologram)
		{
			return false;
		}
		if (!character.config.plantGridType.Contains(characterConfig.plantGridOverrideType))
		{
			return false;
		}
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in gridType)
		{
			if (characterConfig.plantGridType.Contains(item))
			{
				return true;
			}
		}
		return false;
	}

	public Array<TowerDefenseEnum.PLANTGRIDTYPE> GetGridType()
	{
		return gridType;
	}

	public TowerDefenseCharacter GetCharacterWhoHasGridType(TowerDefensePacketConfig packetConfig)
	{
		ClearEmpty();
		Array<TowerDefenseEnum.PLANTGRIDTYPE> plantGridType = packetConfig.characterConfig.plantGridType;
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (GodotObject.IsInstanceValid(character) && (!characterSlotDictionary.ContainsKey(character) || !GodotObject.IsInstanceValid(characterSlotDictionary[character])) && !IsCurrentItemShield(character) && !character.instance.hologram && GodotObject.IsInstanceValid(character))
			{
				TowerDefenseCharacterConfig towerDefenseCharacterConfig = character.config;
				if (towerDefenseCharacterConfig.plantGridOverrideType != TowerDefenseEnum.PLANTGRIDTYPE.NOONE && towerDefenseCharacterConfig.plantGridOverrideType != packetConfig.characterConfig.plantGridOverrideType && plantGridType.Contains(towerDefenseCharacterConfig.plantGridOverrideType))
				{
					return character;
				}
			}
		}
		return null;
	}

	public TowerDefenseGravestone FindPlantingBlocker(TowerDefensePacketConfig packetConfig, bool includeExtended = true)
	{
		if (!(packetConfig?.characterConfig is TowerDefensePlantConfig towerDefensePlantConfig))
		{
			return null;
		}
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (GodotObject.IsInstanceValid(character) && !character.isDestroy && character is TowerDefenseGravestone { config: TowerDefenseGravestoneConfig { blocksPlanting: not false } } towerDefenseGravestone)
			{
				return towerDefenseGravestone;
			}
		}
		if (includeExtended)
		{
			foreach (Vector2I item in towerDefensePlantConfig.extendGrid)
			{
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos + item);
				if (GodotObject.IsInstanceValid(mapCell))
				{
					TowerDefenseGravestone towerDefenseGravestone2 = mapCell.FindPlantingBlocker(packetConfig, includeExtended: false);
					if (GodotObject.IsInstanceValid(towerDefenseGravestone2))
					{
						return towerDefenseGravestone2;
					}
				}
			}
		}
		return null;
	}

	public bool NotifyBlockedPlanting(TowerDefensePacketConfig packetConfig)
	{
		TowerDefenseGravestone towerDefenseGravestone = FindPlantingBlocker(packetConfig);
		if (!GodotObject.IsInstanceValid(towerDefenseGravestone))
		{
			return false;
		}
		towerDefenseGravestone.OnPlantingBlocked();
		return true;
	}

	private bool CanNutBandageSurround(TowerDefenseCharacter character, TowerDefenseCharacterConfig characterConfig, TowerDefensePacketConfig packetConfig)
	{
		if (GameSaveManager.Instance.GetFeatureValue("NutBandaging") <= 0)
		{
			return false;
		}
		if ((characterConfig.physiqueTypeFlags & 1) == 0)
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(character))
		{
			return false;
		}
		if (IsCurrentItemShield(character))
		{
			return false;
		}
		if (character.instance.hypnoses != packetConfig.GetHypnoses() || character.instance.hologram)
		{
			return false;
		}
		if (characterConfig.name != character.config.name)
		{
			return false;
		}
		if ((character.instance.physiqueTypeFlags & 1) == 0)
		{
			return false;
		}
		if (character.instance.damagePointIndex <= 1)
		{
			return false;
		}
		return true;
	}

	public bool CanPacketPlant(TowerDefensePacketConfig packetConfig, bool noLimit = false, bool isCheck = false)
	{
		if (packetConfig == null)
		{
			return false;
		}
		ClearEmpty();
		TowerDefenseCharacterConfig characterConfig = packetConfig.characterConfig;
		if (GodotObject.IsInstanceValid(FindPlantingBlocker(packetConfig, !isCheck)))
		{
			return false;
		}
		if (!noLimit && !isCheck && characterConfig.name == "PlantGravebusterG")
		{
			foreach (TowerDefenseCharacter character in characterList)
			{
				if (GodotObject.IsInstanceValid(character) && character.config.name == "CraterG")
				{
					return false;
				}
			}
		}
		if (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage" && HasVase() && !(packetConfig.characterConfig is TowerDefenseVaseConfig))
		{
			return true;
		}
		if (!isCheck && characterConfig is TowerDefensePlantConfig && ((((TowerDefensePlantConfig)characterConfig).extendCoverDictionary.Count == 0) | noLimit))
		{
			foreach (Vector2I item in ((TowerDefensePlantConfig)characterConfig).extendGrid)
			{
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos + item);
				if (!GodotObject.IsInstanceValid(mapCell))
				{
					return false;
				}
				if (!mapCell.CanPacketPlant(packetConfig, noLimit, isCheck: true))
				{
					return false;
				}
			}
		}
		if (characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.ALL))
		{
			return true;
		}
		if (!isCheck && characterConfig is TowerDefensePlantConfig)
		{
			foreach (TowerDefenseCharacter character2 in characterList)
			{
				if (GodotObject.IsInstanceValid(character2) && !IsCurrentItemShield(character2) && character2.config is TowerDefensePlantConfig && ((TowerDefensePlantConfig)character2.config).extendGrid.Count > 0 && character2.gridPos != gridPos && !characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR) && !character2.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.SURROUND) && !CanUseExtendedGridOccupantAsSlot(packetConfig, character2))
				{
					return false;
				}
			}
		}
		if (characterConfig is TowerDefenseItemConfig)
		{
			if (((TowerDefenseItemConfig)characterConfig).isLadder)
			{
				if (HasWallnut())
				{
					return !GodotObject.IsInstanceValid(characterLadder);
				}
				return false;
			}
			if (((TowerDefenseItemConfig)characterConfig).isShield)
			{
				return true;
			}
		}
		if (!characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.ICECAP))
		{
			TowerDefenseIceCap towerDefenseIceCap = TowerDefenseManager.Instance.GetMapIceCapList()[gridPos.Y].As<TowerDefenseIceCap>();
			if (GodotObject.IsInstanceValid(towerDefenseIceCap) && !characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.GRAVESTONE) && !characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR) && TowerDefenseManager.Instance.GetMapGridPos(towerDefenseIceCap.iceCapSprite.GlobalPosition).X <= gridPos.X)
			{
				return false;
			}
		}
		if (characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.SURROUND) && !characterConfig.plantSurroundCanHasSlot)
		{
			foreach (TowerDefenseCharacter character3 in characterList)
			{
				if (!IsCurrentItemShield(character3) && character3.config.plantGridOverrideType != TowerDefenseEnum.PLANTGRIDTYPE.NOONE)
				{
					return false;
				}
			}
		}
		if (GodotObject.IsInstanceValid(characterSurround) && CanNutBandageSurround(characterSurround, characterConfig, packetConfig))
		{
			return true;
		}
		if (!characterConfig.plantCanHasSurround && GodotObject.IsInstanceValid(characterSurround))
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(characterSurround) && characterConfig is TowerDefensePlantConfig && ((TowerDefensePlantConfig)characterConfig).extendGrid.Count > 0 && characterSurround.config is TowerDefensePlantConfig && ((TowerDefensePlantConfig)characterSurround.config).extendGrid.Count == 0)
		{
			return false;
		}
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item2 in gridType)
		{
			if (slot.ContainsKey(item2) && GodotObject.IsInstanceValid(slot[item2]) && slot[item2].instance.hypnoses == packetConfig.GetHypnoses() && !slot[item2].instance.hologram)
			{
				if (slot[item2] is TowerDefenseGravestone && !characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.GRAVESTONE) && !characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR))
				{
					return false;
				}
				if (slot[item2] is TowerDefenseCrater && !characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR) && !characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.CRATER))
				{
					return false;
				}
			}
		}
		if (packetConfig.GetPlantCover().Count > 0 && !noLimit)
		{
			foreach (TowerDefenseCharacter character4 in characterList)
			{
				if (GodotObject.IsInstanceValid(character4) && !IsCurrentItemShield(character4) && character4.instance.hypnoses == packetConfig.GetHypnoses() && !character4.instance.hologram && (!GodotObject.IsInstanceValid(characterSlotDictionary.ContainsKey(character4) ? characterSlotDictionary[character4] : null) || characterConfig.plantGridOverrideType == character4.config.plantGridOverrideType) && packetConfig.GetPlantCover().Contains(character4.config.name))
				{
					return true;
				}
			}
			if (!packetConfig.GetCoverCanDirectPlant())
			{
				return false;
			}
		}
		if (characterConfig is TowerDefensePlantConfig && !noLimit && ((TowerDefensePlantConfig)characterConfig).extendCoverDictionary.Count > 0)
		{
			foreach (Vector2I key in ((TowerDefensePlantConfig)characterConfig).extendCoverDictionary.Keys)
			{
				TowerDefenseCellInstance mapCell2 = TowerDefenseManager.GetMapCell(gridPos + key);
				if (!GodotObject.IsInstanceValid(mapCell2))
				{
					return false;
				}
				if (!mapCell2.HasCharacter(((TowerDefensePlantConfig)characterConfig).extendCoverDictionary[key]))
				{
					return false;
				}
			}
			return true;
		}
		if (packetConfig.characterConfig.plantCoverSelf)
		{
			foreach (TowerDefenseCharacter character5 in characterList)
			{
				if (GodotObject.IsInstanceValid(character5) && !IsCurrentItemShield(character5) && character5.config.name == packetConfig.characterConfig.name)
				{
					return true;
				}
			}
		}
		bool flag = GameSaveManager.Instance.GetFeatureValue("NutBandaging") > 0;
		bool flag2 = GameSaveManager.Instance.GetFeatureValue("PotReplacement") > 0;
		if (characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.SURROUND))
		{
			bool flag3 = true;
			foreach (TowerDefenseCharacter character6 in characterList)
			{
				if (GodotObject.IsInstanceValid(character6) && !IsCurrentItemShield(character6) && character6.instance.hypnoses == packetConfig.GetHypnoses() && !character6.instance.hologram)
				{
					if (!character6.config.plantCanHasSurround)
					{
						return false;
					}
					if (characterConfig is TowerDefensePlantConfig && ((TowerDefensePlantConfig)characterConfig).extendGrid.Count == 0 && character6.config is TowerDefensePlantConfig && ((TowerDefensePlantConfig)character6.config).extendGrid.Count > 0)
					{
						return false;
					}
				}
			}
			if (flag3)
			{
				foreach (TowerDefenseEnum.PLANTGRIDTYPE item3 in gridType)
				{
					if (!GodotObject.IsInstanceValid(slot.ContainsKey(item3) ? slot[item3] : null))
					{
						if (SURROUND_INCLUDE_GRID.Contains(item3))
						{
							flag3 = false;
							break;
						}
					}
					else if (slot[item3].instance.hypnoses == packetConfig.GetHypnoses() && !slot[item3].instance.hologram && !slot[item3].config.plantCanHasSurround)
					{
						flag3 = false;
						break;
					}
				}
				foreach (TowerDefenseEnum.PLANTGRIDTYPE item4 in gridType)
				{
					if (slot.ContainsKey(item4) && GodotObject.IsInstanceValid(slot[item4]) && !slot[item4].instance.hypnoses && !slot[item4].instance.hologram && characterConfig.plantSurroundCanPlantWater && item4 == TowerDefenseEnum.PLANTGRIDTYPE.WATER)
					{
						flag3 = true;
						break;
					}
				}
			}
			if (flag3 && !GodotObject.IsInstanceValid(characterSurround))
			{
				bool flag4 = false;
				foreach (TowerDefenseEnum.PLANTGRIDTYPE item5 in gridType)
				{
					if (item5 != TowerDefenseEnum.PLANTGRIDTYPE.AIR)
					{
						flag4 = true;
						break;
					}
				}
				if (!flag4)
				{
					return false;
				}
				return true;
			}
		}
		if (flag && (characterConfig.physiqueTypeFlags & 1) != 0)
		{
			foreach (TowerDefenseCharacter character7 in characterList)
			{
				if (GodotObject.IsInstanceValid(character7) && !IsCurrentItemShield(character7) && character7.instance.hypnoses == packetConfig.GetHypnoses() && !character7.instance.hologram && !(characterConfig.name != character7.config.name) && (character7.instance.physiqueTypeFlags & 1) != 0 && character7.instance.damagePointIndex > 1)
				{
					return true;
				}
			}
		}
		if (flag2)
		{
			if ((characterConfig.physiqueTypeFlags & 2) != 0)
			{
				foreach (TowerDefenseCharacter character8 in characterList)
				{
					if (GodotObject.IsInstanceValid(character8) && !IsCurrentItemShield(character8) && character8.instance.hypnoses == packetConfig.GetHypnoses() && !character8.instance.hologram && !(characterConfig.name == character8.config.name) && (character8.instance.physiqueTypeFlags & 2) != 0 && ((character8.instance.physiqueTypeFlags & 0x200) == 0 || !isWater))
					{
						return true;
					}
				}
			}
			if ((characterConfig.physiqueTypeFlags & 4) != 0)
			{
				foreach (TowerDefenseCharacter character9 in characterList)
				{
					if (GodotObject.IsInstanceValid(character9) && !IsCurrentItemShield(character9) && character9.instance.hypnoses == packetConfig.GetHypnoses() && !character9.instance.hologram && !(characterConfig.name == character9.config.name) && (character9.instance.physiqueTypeFlags & 4) != 0)
					{
						return true;
					}
				}
			}
		}
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item6 in gridType)
		{
			if (slot.ContainsKey(item6) && GodotObject.IsInstanceValid(slot[item6]) && slot[item6].instance.hypnoses == packetConfig.GetHypnoses() && !slot[item6].instance.hologram && characterConfig.plantGridType.Contains(item6) && slot[item6].config.plantGridType.Contains(characterConfig.plantGridOverrideType))
			{
				return true;
			}
		}
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item7 in gridType)
		{
			if (slot.ContainsKey(item7) && !GodotObject.IsInstanceValid(slot[item7]) && characterConfig.plantGridType.Contains(item7))
			{
				return true;
			}
		}
		if ((characterConfig is TowerDefensePlantConfig || characterConfig is TowerDefenseGravestoneConfig) && GetCharacterWhoHasGridType(packetConfig) != null)
		{
			return true;
		}
		return false;
	}

	public bool CanMoveCharacterHere(TowerDefenseCharacter character)
	{
		return CanMoveCharactersHere(new TowerDefenseCharacter[1] { character });
	}

	public bool CanMoveCharactersHere(IEnumerable<TowerDefenseCharacter> characters)
	{
		if (characters == null)
		{
			return false;
		}
		List<TowerDefenseCharacter> list = characters.Distinct().ToList();
		if (list.Count == 0 || list.Any((TowerDefenseCharacter character) => !GodotObject.IsInstanceValid(character) || !GodotObject.IsInstanceValid(character.packet) || !GodotObject.IsInstanceValid(character.instance)))
		{
			return false;
		}
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(list[0].gridPos);
		System.Collections.Generic.Dictionary<TowerDefenseCharacter, List<TowerDefenseCellInstance>> dictionary = new System.Collections.Generic.Dictionary<TowerDefenseCharacter, List<TowerDefenseCellInstance>>();
		foreach (TowerDefenseCharacter item in list)
		{
			List<TowerDefenseCellInstance> list2 = (dictionary[item] = GetOccupiedCells(item));
			foreach (TowerDefenseCellInstance item2 in list2)
			{
				item2.RemoveCharacter(item);
			}
		}
		try
		{
			List<TowerDefenseCharacter> list3 = list.Where((TowerDefenseCharacter character) => !CanPacketPlant(character.packet)).ToList();
			if (list3.Count == 0)
			{
				return true;
			}
			int supportFlags = 6;
			bool flag = list.Any((TowerDefenseCharacter character) => (character.instance.physiqueTypeFlags & supportFlags) != 0);
			bool flag2 = list3.All((TowerDefenseCharacter character) => (character.instance.physiqueTypeFlags & supportFlags) == 0 && (!(character.config is TowerDefensePlantConfig towerDefensePlantConfig) || towerDefensePlantConfig.extendGrid.Count == 0));
			return (flag & flag2) && GodotObject.IsInstanceValid(mapCell) && mapCell.gridType.SequenceEqual(gridType) && characterList.Count == 0;
		}
		finally
		{
			foreach (TowerDefenseCharacter item3 in list)
			{
				foreach (TowerDefenseCellInstance item4 in dictionary[item3])
				{
					item4.CharacterPlant(item3.packet, item3, noLimit: true);
				}
			}
		}
	}

	private static List<TowerDefenseCellInstance> GetOccupiedCells(TowerDefenseCharacter character)
	{
		List<TowerDefenseCellInstance> occupiedCells = new List<TowerDefenseCellInstance>();
		AddOccupiedCell(character.gridPos);
		if (character.config is TowerDefensePlantConfig towerDefensePlantConfig && towerDefensePlantConfig.extendCoverDictionary.Count == 0)
		{
			foreach (Vector2I item in towerDefensePlantConfig.extendGrid)
			{
				AddOccupiedCell(character.gridPos + item);
			}
		}
		return occupiedCells;
		void AddOccupiedCell(Vector2I gridPosition)
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPosition);
			if (GodotObject.IsInstanceValid(mapCell) && mapCell.characterList.Contains(character) && !occupiedCells.Contains(mapCell))
			{
				occupiedCells.Add(mapCell);
			}
		}
	}

	public void RemoveCharacter(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return;
		}
		ClearEmpty();
		character.OnDestroy -= CharacterDestroy;
		if (characterSlotDictionary.ContainsKey(character) && GodotObject.IsInstanceValid(characterSlotDictionary[character]))
		{
			foreach (TowerDefenseEnum.PLANTGRIDTYPE item in gridType)
			{
				if (characterSlotDictionary[character].config.plantGridType.Contains(item) && (!GodotObject.IsInstanceValid(slot[item]) || slot[item] == character))
				{
					slot[item] = characterSlotDictionary[character];
					break;
				}
			}
		}
		foreach (TowerDefenseCharacter key in characterSlotDictionary.Keys)
		{
			if (!GodotObject.IsInstanceValid(characterSlotDictionary[key]) || characterSlotDictionary[key] == character)
			{
				characterSlotDictionary[key] = null;
			}
		}
		foreach (TowerDefenseEnum.PLANTGRIDTYPE key2 in slot.Keys)
		{
			if (!GodotObject.IsInstanceValid(slot[key2]) || slot[key2] == character)
			{
				slot[key2] = null;
			}
		}
		characterList.Remove(character);
		characterSlotDictionary.Remove(character);
		if (characterSurround == character)
		{
			characterSurround = null;
		}
		if (characterLadder == character)
		{
			characterLadder = null;
		}
		dirty = true;
	}

	public void ReleaseHologramGridOccupancy(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character) || !GodotObject.IsInstanceValid(character.instance) || !character.instance.hologram)
		{
			return;
		}
		ClearEmpty();
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in slot.Keys.ToList())
		{
			if (slot[item] == character)
			{
				slot[item] = null;
			}
		}
		foreach (TowerDefenseCharacter item2 in characterSlotDictionary.Keys.ToList())
		{
			if (!GodotObject.IsInstanceValid(item2))
			{
				characterSlotDictionary.Remove(item2);
			}
			else if (item2 == character || characterSlotDictionary[item2] == character)
			{
				characterSlotDictionary[item2] = null;
			}
		}
		dirty = true;
	}

	public bool HasPendingRandomGravestonePlacement()
	{
		return _pendingRandomGravestonePlacement;
	}

	public void ReserveRandomGravestonePlacementUntilDeferredBind()
	{
		if (!_pendingRandomGravestonePlacement)
		{
			_pendingRandomGravestonePlacement = true;
			Callable.From(() => _pendingRandomGravestonePlacement = false).CallDeferred();
		}
	}

	public bool CanShovel(double percentage, ShovelConfig shovelConfig = null)
	{
		return GetShovelCharacter(percentage, shovelConfig) != null;
	}

	public bool IsShovelable(TowerDefenseCharacter character, ShovelConfig shovelConfig = null)
	{
		if (character is TowerDefenseGravestone { IsPermanentObstacle: not false } && (!Global.IsEditor || !(SceneManager.CurrentScene == "LevelEditorStage")))
		{
			return false;
		}
		if (character is TowerDefenseItemSheild)
		{
			if (character.instance.hypnoses || character.instance.hologram)
			{
				return false;
			}
			return true;
		}
		if (character is TowerDefenseItem)
		{
			if (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage")
			{
				return true;
			}
			return false;
		}
		if (character is TowerDefenseGravestone || character is TowerDefenseCrater)
		{
			if (GodotObject.IsInstanceValid(shovelConfig) && shovelConfig.shovelableNames.Count > 0 && shovelConfig.shovelableNames.Contains(character.config.name))
			{
				return true;
			}
			if (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage")
			{
				return true;
			}
			return false;
		}
		if (character.instance.hypnoses || character.instance.hologram)
		{
			return false;
		}
		return true;
	}

	public TowerDefenseCharacter GetShovelCharacter(double percentage, ShovelConfig shovelConfig = null)
	{
		ClearEmpty();
		if (percentage < 0.25 && gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR) && slot.ContainsKey(TowerDefenseEnum.PLANTGRIDTYPE.AIR) && GodotObject.IsInstanceValid(slot[TowerDefenseEnum.PLANTGRIDTYPE.AIR]) && IsShovelable(slot[TowerDefenseEnum.PLANTGRIDTYPE.AIR], shovelConfig))
		{
			return slot[TowerDefenseEnum.PLANTGRIDTYPE.AIR];
		}
		if (percentage > 0.5 && GodotObject.IsInstanceValid(characterSurround) && IsShovelable(characterSurround, shovelConfig))
		{
			return characterSurround;
		}
		if (GodotObject.IsInstanceValid(itemShield) && IsShovelable(itemShield, shovelConfig))
		{
			return itemShield;
		}
		foreach (TowerDefenseCharacter key in characterSlotDictionary.Keys)
		{
			if (GodotObject.IsInstanceValid(characterSlotDictionary[key]) && IsShovelable(characterSlotDictionary[key], shovelConfig))
			{
				return characterSlotDictionary[key];
			}
		}
		foreach (TowerDefenseEnum.PLANTGRIDTYPE key2 in slot.Keys)
		{
			if (GodotObject.IsInstanceValid(slot[key2]) && IsShovelable(slot[key2], shovelConfig))
			{
				return slot[key2];
			}
		}
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (IsShovelable(character, shovelConfig))
			{
				return character;
			}
		}
		if (GodotObject.IsInstanceValid(characterSurround) && IsShovelable(characterSurround, shovelConfig))
		{
			return characterSurround;
		}
		if (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage")
		{
			foreach (Variant item in TowerDefenseManager.Instance.GetCharacter())
			{
				TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
				if (towerDefenseCharacter.gridPos == gridPos && IsShovelable(towerDefenseCharacter, shovelConfig))
				{
					return towerDefenseCharacter;
				}
			}
		}
		return null;
	}

	public void Shovel(ShovelConfig shovelConfig, double percentage)
	{
		TowerDefenseCharacter shovelCharacter = GetShovelCharacter(percentage, shovelConfig);
		if (shovelCharacter == null)
		{
			return;
		}
		if (shovelCharacter is TowerDefenseCrater)
		{
			if (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage")
			{
				shovelCharacter.Destroy();
			}
			else
			{
				((TowerDefenseCrater)shovelCharacter).DieDown();
			}
			ClearEmpty();
		}
		else if (shovelCharacter is TowerDefensePlant || shovelCharacter is TowerDefenseItemSheild || (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage"))
		{
			Node2D characterNode = TowerDefenseManager.GetCharacterNode();
			TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(GD.Load<PackedScene>("uid://du8ukldfc7fh7"), shovelCharacter.gridPos);
			characterNode.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, Node.InternalMode.Disabled);
			towerDefenseEffectParticlesOnce.GlobalPosition = shovelCharacter.GetLogicalGlobalPosition(shovelCharacter.transformPoint);
			shovelConfig.Execute(shovelCharacter);
			ClearEmpty();
		}
	}

	public bool IsValidTarget(TowerDefenseCharacter character, TowerDefenseEnum.CHARACTER_CAMP camp, int maskFlags, bool checkInvincible)
	{
		if (character is TowerDefenseGravestone)
		{
			return false;
		}
		if (character is TowerDefenseCrater)
		{
			return false;
		}
		if (character is TowerDefenseVase)
		{
			return false;
		}
		if (checkInvincible && character.instance.invincible)
		{
			return false;
		}
		if (!character.instance.canBeCollection)
		{
			return false;
		}
		if (!character.HasHitBox)
		{
			return false;
		}
		if (!character.CheckDifferentCamp(camp))
		{
			return false;
		}
		if (!character.CanCollision(maskFlags))
		{
			return false;
		}
		if ((character.instance.maskFlags & maskFlags) == 0)
		{
			return false;
		}
		return true;
	}

	public TowerDefenseCharacter GetTarget(int maskFlags = 0, TowerDefenseEnum.CHARACTER_CAMP camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT, bool checkInvincible = true, bool isCataplut = false)
	{
		ClearEmpty();
		if (!isCataplut && GodotObject.IsInstanceValid(characterSurround) && characterSurround.instance.canBeCollection && characterSurround.CheckDifferentCamp(camp) && characterSurround.CanCollision(maskFlags) && (characterSurround.instance.maskFlags & maskFlags) != 0)
		{
			return characterSurround;
		}
		if (isCataplut && slot.ContainsKey(TowerDefenseEnum.PLANTGRIDTYPE.AIR) && GodotObject.IsInstanceValid(slot[TowerDefenseEnum.PLANTGRIDTYPE.AIR]) && slot[TowerDefenseEnum.PLANTGRIDTYPE.AIR].instance.canBeCollection && slot[TowerDefenseEnum.PLANTGRIDTYPE.AIR].CheckDifferentCamp(camp))
		{
			return slot[TowerDefenseEnum.PLANTGRIDTYPE.AIR];
		}
		foreach (KeyValuePair<TowerDefenseCharacter, TowerDefenseCharacter> item in characterSlotDictionary)
		{
			TowerDefenseCharacter value = item.Value;
			if (GodotObject.IsInstanceValid(value) && (!checkInvincible || !GodotObject.IsInstanceValid(item.Key) || !item.Key.instance.invincible) && IsValidTarget(value, camp, maskFlags, checkInvincible))
			{
				return value;
			}
		}
		foreach (KeyValuePair<TowerDefenseEnum.PLANTGRIDTYPE, TowerDefenseCharacter> item2 in slot)
		{
			TowerDefenseCharacter value2 = item2.Value;
			if (!GodotObject.IsInstanceValid(value2))
			{
				continue;
			}
			if (checkInvincible)
			{
				TowerDefenseCharacter towerDefenseCharacter = FindSlotParent(value2);
				if (GodotObject.IsInstanceValid(towerDefenseCharacter) && towerDefenseCharacter.instance.invincible)
				{
					continue;
				}
			}
			if (IsValidTarget(value2, camp, maskFlags, checkInvincible))
			{
				return value2;
			}
		}
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (!GodotObject.IsInstanceValid(character))
			{
				continue;
			}
			if (checkInvincible)
			{
				TowerDefenseCharacter towerDefenseCharacter2 = FindSlotParent(character);
				if (GodotObject.IsInstanceValid(towerDefenseCharacter2) && towerDefenseCharacter2.instance.invincible)
				{
					continue;
				}
			}
			if (IsValidTarget(character, camp, maskFlags, checkInvincible))
			{
				return character;
			}
		}
		if (isCataplut && GodotObject.IsInstanceValid(characterSurround) && characterSurround.instance.canBeCollection && characterSurround.CheckDifferentCamp(camp) && characterSurround.CanCollision(maskFlags) && (characterSurround.instance.maskFlags & maskFlags) != 0)
		{
			return characterSurround;
		}
		return null;
	}

	public TowerDefenseCharacter GetSlot(TowerDefenseCharacter character)
	{
		ClearEmpty();
		if (!GodotObject.IsInstanceValid(character))
		{
			return null;
		}
		if (characterSlotDictionary.TryGetValue(character, out var value) && GodotObject.IsInstanceValid(value))
		{
			return value;
		}
		if (character.config.plantGridOverrideType == TowerDefenseEnum.PLANTGRIDTYPE.NOONE)
		{
			return null;
		}
		if (characterSlotDictionary.ContainsKey(character))
		{
			foreach (TowerDefenseCharacter character2 in characterList)
			{
				if (GodotObject.IsInstanceValid(character2) && !IsCurrentItemShield(character2) && character2.config.plantGridType.Contains(character.config.plantGridOverrideType) && !slot.ContainsValue(character2))
				{
					characterSlotDictionary[character] = character2;
					break;
				}
			}
			if (GodotObject.IsInstanceValid(characterSlotDictionary[character]))
			{
				return characterSlotDictionary[character];
			}
		}
		return null;
	}

	public TowerDefenseCrater GetRepairableCrater()
	{
		ClearEmpty();
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (IsRepairableCrater(character, out var crater))
			{
				return crater;
			}
		}
		foreach (TowerDefenseCharacter value in slot.Values)
		{
			if (IsRepairableCrater(value, out var crater2))
			{
				return crater2;
			}
		}
		foreach (TowerDefenseCharacter value2 in characterSlotDictionary.Values)
		{
			if (IsRepairableCrater(value2, out var crater3))
			{
				return crater3;
			}
		}
		return null;
	}

	private static bool IsRepairableCrater(TowerDefenseCharacter character, out TowerDefenseCrater crater)
	{
		crater = character as TowerDefenseCrater;
		if (GodotObject.IsInstanceValid(crater))
		{
			return !crater.isDestroy;
		}
		return false;
	}

	public TowerDefenseCharacter GetSurround()
	{
		if (GodotObject.IsInstanceValid(characterSurround))
		{
			return characterSurround;
		}
		return null;
	}

	public TowerDefenseCharacter FindSlotParent(TowerDefenseCharacter character)
	{
		foreach (KeyValuePair<TowerDefenseCharacter, TowerDefenseCharacter> item in characterSlotDictionary)
		{
			if (item.Value == character)
			{
				return item.Key;
			}
		}
		return null;
	}

	public bool HasPhysiqueType(int flag)
	{
		ClearEmpty();
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (GodotObject.IsInstanceValid(character) && (character.instance.physiqueTypeFlags & flag) != 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasWallnut()
	{
		return HasPhysiqueType(1);
	}

	public bool HasVase()
	{
		return HasPhysiqueType(32);
	}

	public bool HasLight()
	{
		return HasPhysiqueType(64);
	}

	public bool HasCoffee(int camp = -2)
	{
		if (camp == -2)
		{
			return HasPhysiqueType(8);
		}
		ClearEmpty();
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (GodotObject.IsInstanceValid(character) && (character.instance.physiqueTypeFlags & 8) != 0 && character.camp == (TowerDefenseEnum.CHARACTER_CAMP)camp)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasSpike()
	{
		return HasPhysiqueType(16);
	}

	public bool HasCharacter(string characterName)
	{
		ClearEmpty();
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (GodotObject.IsInstanceValid(character) && character.config.name == characterName)
			{
				return true;
			}
		}
		return false;
	}

	public TowerDefenseCharacter GetCharacterryPhysiqueType(int flag)
	{
		ClearEmpty();
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (GodotObject.IsInstanceValid(character) && (character.instance.physiqueTypeFlags & flag) != 0)
			{
				return character;
			}
		}
		return null;
	}

	public TowerDefenseCharacter GetVase()
	{
		return GetCharacterryPhysiqueType(32);
	}

	public TowerDefenseCharacter GetSpike()
	{
		return GetCharacterryPhysiqueType(16);
	}

	public bool CheckWater()
	{
		return gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.WATER);
	}

	public bool CanCraterCreate()
	{
		ClearEmpty();
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in gridType)
		{
			if (slot.ContainsKey(item) && GodotObject.IsInstanceValid(slot[item]))
			{
				return false;
			}
		}
		return true;
	}

	public double GetGroundHeight(double percentage = 0.5)
	{
		if (!GodotObject.IsInstanceValid(groundHeightCurve))
		{
			return 0.0;
		}
		return groundHeightCurve.Curve.Sample((float)percentage);
	}

	public bool HasPlant()
	{
		ClearEmpty();
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (GodotObject.IsInstanceValid(character) && character is TowerDefensePlant)
			{
				return true;
			}
		}
		return false;
	}

	public bool CanMowerMove()
	{
		if (characterList.Count <= 0)
		{
			return false;
		}
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (!GodotObject.IsInstanceValid(character))
			{
				return false;
			}
			if (!character.canMowerMove)
			{
				return false;
			}
		}
		return true;
	}

	public bool CanMoveToCell(TowerDefenseCellInstance cell, bool checkGridType = true)
	{
		if (cell.characterList.Count > 0)
		{
			return false;
		}
		if (checkGridType && !gridType.SequenceEqual(cell.gridType))
		{
			return false;
		}
		return true;
	}

	internal static Tween CreateCharacterCellMoveTween(TowerDefenseCharacter character, Vector2 targetPosition, double duration = 0.5)
	{
		character.CancelCellMoveTween();
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		character.RefreshCellMoveRenderOrder(logicalGlobalPosition);
		Tween tween = character.CreateTween();
		tween.SetProcessMode(Tween.TweenProcessMode.Physics);
		tween.SetParallel();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Quart);
		duration = (double.IsFinite(duration) ? Math.Clamp(duration, 0.01, 10.0) : 0.5);
		tween.TweenMethod(Callable.From<Vector2>(character.SetCellMoveLogicalGlobalPosition), logicalGlobalPosition, targetPosition, duration);
		ShadowComponent shadowComponent = character.shadowComponent;
		if (shadowComponent != null && !shadowComponent.IsReleased)
		{
			character.shadowComponent.TweenSaveShadowPosition(tween, character.shadowComponent.saveShadowPosition + targetPosition - logicalGlobalPosition, duration);
		}
		character.TrackCellMoveTween(tween);
		return tween;
	}

	public void MoveCharacterToCell(TowerDefenseCharacter character, TowerDefenseCellInstance cell, double duration = 0.5)
	{
		cell.CharacterPlant(character.packet, character, noLimit: true);
		if (character.config is TowerDefensePlantConfig towerDefensePlantConfig)
		{
			foreach (Vector2I item in towerDefensePlantConfig.extendGrid)
			{
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(cell.gridPos + item);
				if (GodotObject.IsInstanceValid(mapCell) && mapCell != cell)
				{
					mapCell.CharacterPlant(character.packet, character, noLimit: true);
				}
			}
		}
		character.gridPos = cell.gridPos;
		CreateCharacterCellMoveTween(character, TowerDefenseManager.GetMapCellPlantPos(cell.gridPos), duration);
	}

	public void MoveToCell(TowerDefenseCellInstance cell, bool jump = false)
	{
		cell.characterList = new List<TowerDefenseCharacter>(characterList);
		cell.characterSlotDictionary = new System.Collections.Generic.Dictionary<TowerDefenseCharacter, TowerDefenseCharacter>(characterSlotDictionary);
		cell.slot = new System.Collections.Generic.Dictionary<TowerDefenseEnum.PLANTGRIDTYPE, TowerDefenseCharacter>(slot);
		if (GodotObject.IsInstanceValid(characterSurround))
		{
			cell.characterSurround = characterSurround;
		}
		if (GodotObject.IsInstanceValid(characterLadder))
		{
			cell.characterLadder = characterLadder;
		}
		foreach (TowerDefenseCharacter character in characterList)
		{
			character.OnDestroy -= CharacterDestroy;
			character.OnDestroy += cell.CharacterDestroy;
			character.gridPos = cell.gridPos;
			CreateCharacterCellMoveTween(character, TowerDefenseManager.GetMapCellPlantPos(cell.gridPos));
			if (jump)
			{
				character.ySpeed = -200.0;
			}
		}
		characterList.Clear();
		characterSlotDictionary.Clear();
		slot.Clear();
		characterSurround = null;
		characterLadder = null;
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in gridType)
		{
			slot[item] = null;
		}
	}

	public void TransformPlantsTo(string targetName, PackedScene effectScene = null, Func<TowerDefenseCharacter, bool> skipMember = null)
	{
		if (_transformPending)
		{
			return;
		}
		_transformPending = true;
		Callable.From(() => _transformPending = false).CallDeferred();
		ClearEmpty();
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(targetName);
		if (packetConfig == null || !(packetConfig.characterConfig is TowerDefensePlantConfig))
		{
			return;
		}
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
		foreach (TowerDefenseCharacter character in characterList)
		{
			if (GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(character.instance) && character is TowerDefensePlant && !character.die && !character.nearDie && (skipMember == null || !skipMember(character)))
			{
				list.Add(character);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		foreach (TowerDefenseCharacter item in list)
		{
			RemoveCharacter(item);
		}
		if (!CanPacketPlant(packetConfig))
		{
			foreach (TowerDefenseCharacter item2 in list)
			{
				if (GodotObject.IsInstanceValid(item2))
				{
					item2.SilentlyRemoveForTransformation();
				}
			}
			return;
		}
		if (!GodotObject.IsInstanceValid(list[list.Count - 1].TransformTo(targetName, null, effectScene)))
		{
			foreach (TowerDefenseCharacter item3 in list)
			{
				if (GodotObject.IsInstanceValid(item3))
				{
					CharacterPlant(item3.packet, item3, noLimit: true);
				}
			}
			return;
		}
		for (int num = 0; num < list.Count - 1; num++)
		{
			if (GodotObject.IsInstanceValid(list[num]))
			{
				list[num].SilentlyRemoveForTransformation();
			}
		}
	}

	public void AttackDeal(TowerDefenseCharacter character, string type, double num)
	{
		if (GodotObject.IsInstanceValid(character) && type == "Eat" && HasCharacter("PlantPotGarlic"))
		{
			character.Garlic();
			TowerDefenseCharacter target = GetTarget(character.instance.maskFlags);
			if (GodotObject.IsInstanceValid(target))
			{
				target.Hurt(10.0);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(57)
		{
			new MethodInfo(MethodName.TryBeginPurify, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelPurify, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "suppressDeathrattles", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveInvalidCharacterSlotKeys, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CharacterPlant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "noLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CharacterReplace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "replaceCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CharacterDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ScheduleSleepEnvironmentRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReevaluateCharacterSleepStates, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSlotCharacterList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasSlotCharacterList, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCharacterListSave, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "checkSlot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsCurrentItemShield, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanUseExtendedGridOccupantAsSlot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetGridType, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCharacterWhoHasGridType, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindPlantingBlocker, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "includeExtended", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyBlockedPlanting, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanNutBandageSurround, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "characterConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanPacketPlant, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "noLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isCheck", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanMoveCharacterHere, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseHologramGridOccupancy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasPendingRandomGravestonePlacement, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReserveRandomGravestonePlacementUntilDeferredBind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanShovel, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "shovelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsShovelable, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "shovelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetShovelCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "shovelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Shovel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shovelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsValidTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "maskFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkInvincible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maskFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkInvincible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isCataplut", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSlot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetRepairableCrater, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSurround, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindSlotParent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasPhysiqueType, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "flag", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasWallnut, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasVase, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasLight, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCoffee, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasSpike, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterryPhysiqueType, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "flag", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetVase, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSpike, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckWater, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanCraterCreate, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGroundHeight, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasPlant, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanMowerMove, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanMoveToCell, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkGridType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateCharacterCellMoveTween, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Tween"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "targetPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveCharacterToCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveToCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "jump", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackDeal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.TryBeginPurify && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryBeginPurify());
			return true;
		}
		if (method == MethodName.CancelPurify && args.Count == 0)
		{
			CancelPurify();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 1)
		{
			Clear(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearEmpty && args.Count == 0)
		{
			ClearEmpty();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveInvalidCharacterSlotKeys && args.Count == 0)
		{
			RemoveInvalidCharacterSlotKeys();
			ret = default;
			return true;
		}
		if (method == MethodName.CharacterPlant && args.Count == 3)
		{
			CharacterPlant(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CharacterReplace && args.Count == 2)
		{
			CharacterReplace(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CharacterDestroy && args.Count == 1)
		{
			CharacterDestroy(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleSleepEnvironmentRefresh && args.Count == 0)
		{
			ScheduleSleepEnvironmentRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.ReevaluateCharacterSleepStates && args.Count == 0)
		{
			ReevaluateCharacterSleepStates();
			ret = default;
			return true;
		}
		if (method == MethodName.GetSlotCharacterList && args.Count == 0)
		{
			Array<TowerDefenseCharacter> slotCharacterList = GetSlotCharacterList();
			ret = VariantUtils.CreateFromArray(slotCharacterList);
			return true;
		}
		if (method == MethodName.HasSlotCharacterList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSlotCharacterList());
			return true;
		}
		if (method == MethodName.GetCharacterListSave && args.Count == 1)
		{
			Array<TowerDefenseCharacter> characterListSave = GetCharacterListSave(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = VariantUtils.CreateFromArray(characterListSave);
			return true;
		}
		if (method == MethodName.IsCurrentItemShield && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCurrentItemShield(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.CanUseExtendedGridOccupantAsSlot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanUseExtendedGridOccupantAsSlot(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		if (method == MethodName.GetGridType && args.Count == 0)
		{
			Array<TowerDefenseEnum.PLANTGRIDTYPE> array = GetGridType();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.GetCharacterWhoHasGridType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetCharacterWhoHasGridType(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FindPlantingBlocker && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseGravestone>(FindPlantingBlocker(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.NotifyBlockedPlanting && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(NotifyBlockedPlanting(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.CanNutBandageSurround && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(CanNutBandageSurround(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[2])));
			return true;
		}
		if (method == MethodName.CanPacketPlant && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(CanPacketPlant(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.CanMoveCharacterHere && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanMoveCharacterHere(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveCharacter && args.Count == 1)
		{
			RemoveCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseHologramGridOccupancy && args.Count == 1)
		{
			ReleaseHologramGridOccupancy(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasPendingRandomGravestonePlacement && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPendingRandomGravestonePlacement());
			return true;
		}
		if (method == MethodName.ReserveRandomGravestonePlacementUntilDeferredBind && args.Count == 0)
		{
			ReserveRandomGravestonePlacementUntilDeferredBind();
			ret = default;
			return true;
		}
		if (method == MethodName.CanShovel && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanShovel(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ShovelConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.IsShovelable && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsShovelable(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<ShovelConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.GetShovelCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetShovelCharacter(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ShovelConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.Shovel && args.Count == 2)
		{
			Shovel(VariantUtils.ConvertTo<ShovelConfig>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsValidTarget && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.GetTarget && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetTarget(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.GetSlot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetSlot(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.GetRepairableCrater && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCrater>(GetRepairableCrater());
			return true;
		}
		if (method == MethodName.GetSurround && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetSurround());
			return true;
		}
		if (method == MethodName.FindSlotParent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindSlotParent(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.HasPhysiqueType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPhysiqueType(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.HasWallnut && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasWallnut());
			return true;
		}
		if (method == MethodName.HasVase && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasVase());
			return true;
		}
		if (method == MethodName.HasLight && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasLight());
			return true;
		}
		if (method == MethodName.HasCoffee && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCoffee(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.HasSpike && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSpike());
			return true;
		}
		if (method == MethodName.HasCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCharacter(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterryPhysiqueType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetCharacterryPhysiqueType(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetVase && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetVase());
			return true;
		}
		if (method == MethodName.GetSpike && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetSpike());
			return true;
		}
		if (method == MethodName.CheckWater && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckWater());
			return true;
		}
		if (method == MethodName.CanCraterCreate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCraterCreate());
			return true;
		}
		if (method == MethodName.GetGroundHeight && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetGroundHeight(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.HasPlant && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPlant());
			return true;
		}
		if (method == MethodName.CanMowerMove && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanMowerMove());
			return true;
		}
		if (method == MethodName.CanMoveToCell && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanMoveToCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateCharacterCellMoveTween && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Tween>(CreateCharacterCellMoveTween(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.MoveCharacterToCell && args.Count == 3)
		{
			MoveCharacterToCell(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveToCell && args.Count == 2)
		{
			MoveToCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackDeal && args.Count == 3)
		{
			AttackDeal(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateCharacterCellMoveTween && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Tween>(CreateCharacterCellMoveTween(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.TryBeginPurify)
		{
			return true;
		}
		if (method == MethodName.CancelPurify)
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
		if (method == MethodName.ClearEmpty)
		{
			return true;
		}
		if (method == MethodName.RemoveInvalidCharacterSlotKeys)
		{
			return true;
		}
		if (method == MethodName.CharacterPlant)
		{
			return true;
		}
		if (method == MethodName.CharacterReplace)
		{
			return true;
		}
		if (method == MethodName.CharacterDestroy)
		{
			return true;
		}
		if (method == MethodName.ScheduleSleepEnvironmentRefresh)
		{
			return true;
		}
		if (method == MethodName.ReevaluateCharacterSleepStates)
		{
			return true;
		}
		if (method == MethodName.GetSlotCharacterList)
		{
			return true;
		}
		if (method == MethodName.HasSlotCharacterList)
		{
			return true;
		}
		if (method == MethodName.GetCharacterListSave)
		{
			return true;
		}
		if (method == MethodName.IsCurrentItemShield)
		{
			return true;
		}
		if (method == MethodName.CanUseExtendedGridOccupantAsSlot)
		{
			return true;
		}
		if (method == MethodName.GetGridType)
		{
			return true;
		}
		if (method == MethodName.GetCharacterWhoHasGridType)
		{
			return true;
		}
		if (method == MethodName.FindPlantingBlocker)
		{
			return true;
		}
		if (method == MethodName.NotifyBlockedPlanting)
		{
			return true;
		}
		if (method == MethodName.CanNutBandageSurround)
		{
			return true;
		}
		if (method == MethodName.CanPacketPlant)
		{
			return true;
		}
		if (method == MethodName.CanMoveCharacterHere)
		{
			return true;
		}
		if (method == MethodName.RemoveCharacter)
		{
			return true;
		}
		if (method == MethodName.ReleaseHologramGridOccupancy)
		{
			return true;
		}
		if (method == MethodName.HasPendingRandomGravestonePlacement)
		{
			return true;
		}
		if (method == MethodName.ReserveRandomGravestonePlacementUntilDeferredBind)
		{
			return true;
		}
		if (method == MethodName.CanShovel)
		{
			return true;
		}
		if (method == MethodName.IsShovelable)
		{
			return true;
		}
		if (method == MethodName.GetShovelCharacter)
		{
			return true;
		}
		if (method == MethodName.Shovel)
		{
			return true;
		}
		if (method == MethodName.IsValidTarget)
		{
			return true;
		}
		if (method == MethodName.GetTarget)
		{
			return true;
		}
		if (method == MethodName.GetSlot)
		{
			return true;
		}
		if (method == MethodName.GetRepairableCrater)
		{
			return true;
		}
		if (method == MethodName.GetSurround)
		{
			return true;
		}
		if (method == MethodName.FindSlotParent)
		{
			return true;
		}
		if (method == MethodName.HasPhysiqueType)
		{
			return true;
		}
		if (method == MethodName.HasWallnut)
		{
			return true;
		}
		if (method == MethodName.HasVase)
		{
			return true;
		}
		if (method == MethodName.HasLight)
		{
			return true;
		}
		if (method == MethodName.HasCoffee)
		{
			return true;
		}
		if (method == MethodName.HasSpike)
		{
			return true;
		}
		if (method == MethodName.HasCharacter)
		{
			return true;
		}
		if (method == MethodName.GetCharacterryPhysiqueType)
		{
			return true;
		}
		if (method == MethodName.GetVase)
		{
			return true;
		}
		if (method == MethodName.GetSpike)
		{
			return true;
		}
		if (method == MethodName.CheckWater)
		{
			return true;
		}
		if (method == MethodName.CanCraterCreate)
		{
			return true;
		}
		if (method == MethodName.GetGroundHeight)
		{
			return true;
		}
		if (method == MethodName.HasPlant)
		{
			return true;
		}
		if (method == MethodName.CanMowerMove)
		{
			return true;
		}
		if (method == MethodName.CanMoveToCell)
		{
			return true;
		}
		if (method == MethodName.CreateCharacterCellMoveTween)
		{
			return true;
		}
		if (method == MethodName.MoveCharacterToCell)
		{
			return true;
		}
		if (method == MethodName.MoveToCell)
		{
			return true;
		}
		if (method == MethodName.AttackDeal)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.gridType)
		{
			gridType = VariantUtils.ConvertToArray<TowerDefenseEnum.PLANTGRIDTYPE>(in value);
			return true;
		}
		if (name == PropertyName.elementFlags)
		{
			elementFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.isWater)
		{
			isWater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.characterSurround)
		{
			characterSurround = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.characterLadder)
		{
			characterLadder = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.itemShield)
		{
			itemShield = VariantUtils.ConvertTo<TowerDefenseItemSheild>(in value);
			return true;
		}
		if (name == PropertyName.smashAbsorbedFrame)
		{
			smashAbsorbedFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			gridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.groundHeightCurve)
		{
			groundHeightCurve = VariantUtils.ConvertTo<CurveTexture>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseCellConfig>(in value);
			return true;
		}
		if (name == PropertyName.dirty)
		{
			dirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sleepEnvironmentRefreshScheduled)
		{
			_sleepEnvironmentRefreshScheduled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingRandomGravestonePlacement)
		{
			_pendingRandomGravestonePlacement = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._transformPending)
		{
			_transformPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._purifyPending)
		{
			_purifyPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.gridType)
		{
			value = VariantUtils.CreateFromArray(gridType);
			return true;
		}
		if (name == PropertyName.elementFlags)
		{
			value = VariantUtils.CreateFrom(in elementFlags);
			return true;
		}
		if (name == PropertyName.isWater)
		{
			value = VariantUtils.CreateFrom(in isWater);
			return true;
		}
		if (name == PropertyName.characterSurround)
		{
			value = VariantUtils.CreateFrom(in characterSurround);
			return true;
		}
		if (name == PropertyName.characterLadder)
		{
			value = VariantUtils.CreateFrom(in characterLadder);
			return true;
		}
		if (name == PropertyName.itemShield)
		{
			value = VariantUtils.CreateFrom(in itemShield);
			return true;
		}
		if (name == PropertyName.smashAbsorbedFrame)
		{
			value = VariantUtils.CreateFrom(in smashAbsorbedFrame);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			value = VariantUtils.CreateFrom(in gridPos);
			return true;
		}
		if (name == PropertyName.groundHeightCurve)
		{
			value = VariantUtils.CreateFrom(in groundHeightCurve);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.dirty)
		{
			value = VariantUtils.CreateFrom(in dirty);
			return true;
		}
		if (name == PropertyName._sleepEnvironmentRefreshScheduled)
		{
			value = VariantUtils.CreateFrom(in _sleepEnvironmentRefreshScheduled);
			return true;
		}
		if (name == PropertyName._pendingRandomGravestonePlacement)
		{
			value = VariantUtils.CreateFrom(in _pendingRandomGravestonePlacement);
			return true;
		}
		if (name == PropertyName._transformPending)
		{
			value = VariantUtils.CreateFrom(in _transformPending);
			return true;
		}
		if (name == PropertyName._purifyPending)
		{
			value = VariantUtils.CreateFrom(in _purifyPending);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.gridType, PropertyHint.TypeString, "2/2:ALL:-1,NOONE:0,SOIL:1,GROUND:2,WATER:3,AIR:4,LILYPAD:5,POT:6,SURROUND:7,GRAVESTONE:8,CRATER:9,BRICK:10,ICECAP:11,PLANT:12", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.elementFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isWater, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterSurround, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterLadder, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.itemShield, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.smashAbsorbedFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.groundHeightCurve, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.dirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sleepEnvironmentRefreshScheduled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingRandomGravestonePlacement, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._transformPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._purifyPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.gridType, Variant.CreateFrom(gridType));
		info.AddProperty(PropertyName.elementFlags, Variant.From(in elementFlags));
		info.AddProperty(PropertyName.isWater, Variant.From(in isWater));
		info.AddProperty(PropertyName.characterSurround, Variant.From(in characterSurround));
		info.AddProperty(PropertyName.characterLadder, Variant.From(in characterLadder));
		info.AddProperty(PropertyName.itemShield, Variant.From(in itemShield));
		info.AddProperty(PropertyName.smashAbsorbedFrame, Variant.From(in smashAbsorbedFrame));
		info.AddProperty(PropertyName.gridPos, Variant.From(in gridPos));
		info.AddProperty(PropertyName.groundHeightCurve, Variant.From(in groundHeightCurve));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.dirty, Variant.From(in dirty));
		info.AddProperty(PropertyName._sleepEnvironmentRefreshScheduled, Variant.From(in _sleepEnvironmentRefreshScheduled));
		info.AddProperty(PropertyName._pendingRandomGravestonePlacement, Variant.From(in _pendingRandomGravestonePlacement));
		info.AddProperty(PropertyName._transformPending, Variant.From(in _transformPending));
		info.AddProperty(PropertyName._purifyPending, Variant.From(in _purifyPending));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.gridType, out var value))
		{
			gridType = value.AsGodotArray<TowerDefenseEnum.PLANTGRIDTYPE>();
		}
		if (info.TryGetProperty(PropertyName.elementFlags, out var value2))
		{
			elementFlags = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.isWater, out var value3))
		{
			isWater = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.characterSurround, out var value4))
		{
			characterSurround = value4.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.characterLadder, out var value5))
		{
			characterLadder = value5.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.itemShield, out var value6))
		{
			itemShield = value6.As<TowerDefenseItemSheild>();
		}
		if (info.TryGetProperty(PropertyName.smashAbsorbedFrame, out var value7))
		{
			smashAbsorbedFrame = value7.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.gridPos, out var value8))
		{
			gridPos = value8.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.groundHeightCurve, out var value9))
		{
			groundHeightCurve = value9.As<CurveTexture>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value10))
		{
			config = value10.As<TowerDefenseCellConfig>();
		}
		if (info.TryGetProperty(PropertyName.dirty, out var value11))
		{
			dirty = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sleepEnvironmentRefreshScheduled, out var value12))
		{
			_sleepEnvironmentRefreshScheduled = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingRandomGravestonePlacement, out var value13))
		{
			_pendingRandomGravestonePlacement = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._transformPending, out var value14))
		{
			_transformPending = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._purifyPending, out var value15))
		{
			_purifyPending = value15.As<bool>();
		}
	}
}
