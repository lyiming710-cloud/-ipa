using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using Microsoft.Extensions.Logging;
using PVZHE.ModEditor.ModSystem;
using ZLogger;

[GlobalClass]
[ScriptPath("res://Core/TowerDefenseManager/TowerDefenseManager.cs")]
public class TowerDefenseManager : Node2D
{
	private sealed class FallingObjectReplicationCapture : IDisposable
	{
		private TowerDefenseManager _owner;

		private readonly FallingObjectReplicationCapture _parent;

		private readonly List<FallingObjectSpawnRecord> _records = new List<FallingObjectSpawnRecord>();

		public FallingObjectReplicationCapture(TowerDefenseManager owner, FallingObjectReplicationCapture parent)
		{
			_owner = owner;
			_parent = parent;
			owner._activeFallingObjectReplicationCapture = this;
		}

		public void Record(TowerDefenseGroundItemBase item, ObjectManagerConfig.OBJECT objectId, Vector2 position, double height, Vector2 velocity, double gravity)
		{
			_records.Add(new FallingObjectSpawnRecord
			{
				Item = item,
				ObjectId = objectId,
				Position = position,
				Height = height,
				Velocity = velocity,
				Gravity = gravity
			});
		}

		public void Dispose()
		{
			TowerDefenseManager owner = _owner;
			_owner = null;
			if (GodotObject.IsInstanceValid(owner))
			{
				if (owner._activeFallingObjectReplicationCapture == this)
				{
					owner._activeFallingObjectReplicationCapture = _parent;
				}
				owner.FlushFallingObjectSpawns(_records);
			}
		}
	}

	private sealed class FallingObjectSpawnRecord
	{
		public TowerDefenseGroundItemBase Item;

		public ObjectManagerConfig.OBJECT ObjectId;

		public Vector2 Position;

		public double Height;

		public Vector2 Velocity;

		public double Gravity;
	}

	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName CreateAward = "CreateAward";

		public static readonly StringName CreateAwardFromScene = "CreateAwardFromScene";

		public static readonly StringName GetbackgroundMusicConfig = "GetbackgroundMusicConfig";

		public static readonly StringName SunCreate = "SunCreate";

		public static readonly StringName GetCampFriendlyFromArea = "GetCampFriendlyFromArea";

		public static readonly StringName GetCampFriendlyLine = "GetCampFriendlyLine";

		public static readonly StringName GetCampFriendly = "GetCampFriendly";

		public static readonly StringName GetCampTarget = "GetCampTarget";

		public static readonly StringName GetCampTargetFromArray = "GetCampTargetFromArray";

		public static readonly StringName CountConveyorPacketsByCharacterName = "CountConveyorPacketsByCharacterName";

		public static readonly StringName GetCachedCharacterCount = "GetCachedCharacterCount";

		public static readonly StringName RefreshCharacterCountCache = "RefreshCharacterCountCache";

		public static readonly StringName GetCharacterNum = "GetCharacterNum";

		public static readonly StringName GetCharacterFromName = "GetCharacterFromName";

		public static readonly StringName GetRainModeFeature = "GetRainModeFeature";

		public static readonly StringName GetConveyorBeltFeature = "GetConveyorBeltFeature";

		public static readonly StringName GetGloveFeature = "GetGloveFeature";

		public static readonly StringName GetScreenEffectFeature = "GetScreenEffectFeature";

		public static readonly StringName GetBrainFeature = "GetBrainFeature";

		public static readonly StringName GetCurrentProcess = "GetCurrentProcess";

		public static readonly StringName GetPlant = "GetPlant";

		public static readonly StringName GetZombie = "GetZombie";

		public static readonly StringName GetCharacter = "GetCharacter";

		public static readonly StringName GetProjectile = "GetProjectile";

		public static readonly StringName GetEffect = "GetEffect";

		public static readonly StringName GetEffectCount = "GetEffectCount";

		public static readonly StringName GetEffectCountForPhysicsFrame = "GetEffectCountForPhysicsFrame";

		public static readonly StringName GetLineCharacters = "GetLineCharacters";

		public static readonly StringName GetCharacterHasTarget = "GetCharacterHasTarget";

		public static readonly StringName GetCharacterHasTargetFromArea = "GetCharacterHasTargetFromArea";

		public static readonly StringName GetCharacterTargetNearest = "GetCharacterTargetNearest";

		public static readonly StringName GetNearCharacter = "GetNearCharacter";

		public static readonly StringName GetTallCharacterTargetFromRect = "GetTallCharacterTargetFromRect";

		public static readonly StringName GetCharacterTargetNearestFromRect = "GetCharacterTargetNearestFromRect";

		public static readonly StringName GetCharacterHasTargetFromRect = "GetCharacterHasTargetFromRect";

		public static readonly StringName HasTrackTarget = "HasTrackTarget";

		public static readonly StringName GetCharacterTargetNearestFromArea = "GetCharacterTargetNearestFromArea";

		public static readonly StringName GetCharactersForLine = "GetCharactersForLine";

		public static readonly StringName GetProjectileHasTarget = "GetProjectileHasTarget";

		public static readonly StringName GetProjectileHasTargetFromArea = "GetProjectileHasTargetFromArea";

		public static readonly StringName GetProjectileTargetNearest = "GetProjectileTargetNearest";

		public static readonly StringName GetProjectileTargetNearestForPhysicsFrame = "GetProjectileTargetNearestForPhysicsFrame";

		public static readonly StringName GetProjectileTargetNearestProjectile = "GetProjectileTargetNearestProjectile";

		public static readonly StringName GetProjectileInitialTrackTarget = "GetProjectileInitialTrackTarget";

		public static readonly StringName BrainSunCreate = "BrainSunCreate";

		public static readonly StringName BungiSpawn = "BungiSpawn";

		public static readonly StringName IsBungiWaveOperationCurrent = "IsBungiWaveOperationCurrent";

		public static readonly StringName CreateBungiSpawnNode = "CreateBungiSpawnNode";

		public static readonly StringName CleanupFailedBungiSpawn = "CleanupFailedBungiSpawn";

		public static readonly StringName SyncBungiSpawn = "SyncBungiSpawn";

		public static readonly StringName GetCharacterNode = "GetCharacterNode";

		public static readonly StringName CharacterRegister = "CharacterRegister";

		public static readonly StringName CharacterUnregister = "CharacterUnregister";

		public static readonly StringName GetCharacterSprite = "GetCharacterSprite";

		public static readonly StringName GetPacketSpriteScene = "GetPacketSpriteScene";

		public static readonly StringName GetPacketSprite = "GetPacketSprite";

		public static readonly StringName GetChacraterScene = "GetChacraterScene";

		public static readonly StringName CreateCharacter = "CreateCharacter";

		public static readonly StringName GetCoin = "GetCoin";

		public static readonly StringName AddCoin = "AddCoin";

		public static readonly StringName UseCoin = "UseCoin";

		public static readonly StringName GetCollectable = "GetCollectable";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName MapIsChange = "MapIsChange";

		public static readonly StringName ReevaluateCharacterSleepStates = "ReevaluateCharacterSleepStates";

		public static readonly StringName CharacterDestroy = "CharacterDestroy";

		public static readonly StringName ClearDeathRecords = "ClearDeathRecords";

		public static readonly StringName HasDeathRecord = "HasDeathRecord";

		public static readonly StringName GetEffectDirtName = "GetEffectDirtName";

		public static readonly StringName GetEffectSprite = "GetEffectSprite";

		public static readonly StringName CreateEffectParticlesOnce = "CreateEffectParticlesOnce";

		public static readonly StringName CreateEffectSpriteOnce = "CreateEffectSpriteOnce";

		public static readonly StringName CreateEffectParticlesSceneOnce = "CreateEffectParticlesSceneOnce";

		public static readonly StringName CreateEffectSpriteSceneOnce = "CreateEffectSpriteSceneOnce";

		public static readonly StringName TryCreateEffectSceneOnceFast = "TryCreateEffectSceneOnceFast";

		public static readonly StringName FallingObjectCreate = "FallingObjectCreate";

		public static readonly StringName FallingObjectItemCreate = "FallingObjectItemCreate";

		public static readonly StringName CaptureFallingObjectSpawn = "CaptureFallingObjectSpawn";

		public static readonly StringName GetGameMethod = "GetGameMethod";

		public static readonly StringName IsIZMMode = "IsIZMMode";

		public static readonly StringName IsIZM2Mode = "IsIZM2Mode";

		public static readonly StringName IsLevelEditorStage = "IsLevelEditorStage";

		public static readonly StringName IsGameRunning = "IsGameRunning";

		public static readonly StringName IsUnlimitedFire = "IsUnlimitedFire";

		public static readonly StringName CoinCreate = "CoinCreate";

		public static readonly StringName AddSun = "AddSun";

		public static readonly StringName UseSun = "UseSun";

		public static readonly StringName SetSun = "SetSun";

		public static readonly StringName GetSun = "GetSun";

		public static readonly StringName GetLevelChapterFinishNum = "GetLevelChapterFinishNum";

		public static readonly StringName GetLevelEvent = "GetLevelEvent";

		public static readonly StringName SetNextLevel = "SetNextLevel";

		public static readonly StringName LoadNextLevelConfig = "LoadNextLevelConfig";

		public static readonly StringName CanOpenNextLevel = "CanOpenNextLevel";

		public static readonly StringName SaveNextLevelProgress = "SaveNextLevelProgress";

		public static readonly StringName GetLevelControl = "GetLevelControl";

		public static readonly StringName TipsPlay = "TipsPlay";

		public static readonly StringName GetLevelHomeworld = "GetLevelHomeworld";

		public static readonly StringName MagicSunCreate = "MagicSunCreate";

		public static readonly StringName GetMapAttackDpsLifestealRatio = "GetMapAttackDpsLifestealRatio";

		public static readonly StringName ApplyMapAttackDpsLifesteal = "ApplyMapAttackDpsLifesteal";

		public static readonly StringName ApplyMapCharacterRules = "ApplyMapCharacterRules";

		public static readonly StringName GetMapZombieColumnSpeedMultiplier = "GetMapZombieColumnSpeedMultiplier";

		public static readonly StringName GetCurrentMap = "GetCurrentMap";

		public static readonly StringName GetMapConfig = "GetMapConfig";

		public static readonly StringName MapChange = "MapChange";

		public static readonly StringName GetMapIsNight = "GetMapIsNight";

		public static readonly StringName MapDayNightSwitch = "MapDayNightSwitch";

		public static readonly StringName GetMapGridNum = "GetMapGridNum";

		public static readonly StringName GetMapGridSize = "GetMapGridSize";

		public static readonly StringName GetMapGridBeginPos = "GetMapGridBeginPos";

		public static readonly StringName GetMapPlantOffset = "GetMapPlantOffset";

		public static readonly StringName GetMapGridPos = "GetMapGridPos";

		public static readonly StringName GetMapCellPos = "GetMapCellPos";

		public static readonly StringName GetMapCellPosCenter = "GetMapCellPosCenter";

		public static readonly StringName CheckMapGridPosIn = "CheckMapGridPosIn";

		public static readonly StringName GetMapCell = "GetMapCell";

		public static readonly StringName SetMapGridType = "SetMapGridType";

		public static readonly StringName SetMapLineUse = "SetMapLineUse";

		public static readonly StringName GetMapLineUse = "GetMapLineUse";

		public static readonly StringName GetMapGroundLeft = "GetMapGroundLeft";

		public static readonly StringName GetMapGroundRight = "GetMapGroundRight";

		public static readonly StringName GetMapGroundUp = "GetMapGroundUp";

		public static readonly StringName GetMapGroundDown = "GetMapGroundDown";

		public static readonly StringName GetMapGridPosFromMouse = "GetMapGridPosFromMouse";

		public static readonly StringName GetMapControl = "GetMapControl";

		public static readonly StringName ReleaseBattleReferences = "ReleaseBattleReferences";

		public static readonly StringName GetMapFeature = "GetMapFeature";

		public static readonly StringName ApplyMapPacketCooldownRules = "ApplyMapPacketCooldownRules";

		public static readonly StringName MapIgnoresDynamicPacketCostGrowth = "MapIgnoresDynamicPacketCostGrowth";

		public static readonly StringName GetMapCellPlantPos = "GetMapCellPlantPos";

		public static readonly StringName GetMapLineY = "GetMapLineY";

		public static readonly StringName GetCurrentMapConfig = "GetCurrentMapConfig";

		public static readonly StringName GetMapCurrentMap = "GetMapCurrentMap";

		public static readonly StringName GetPacketPickControl = "GetPacketPickControl";

		public static readonly StringName MapLineHasType = "MapLineHasType";

		public static readonly StringName GetMapPlantGrid = "GetMapPlantGrid";

		public static readonly StringName GetMapLineUseArr = "GetMapLineUseArr";

		public static readonly StringName GetMapIceCapList = "GetMapIceCapList";

		public static readonly StringName GetGroundRect = "GetGroundRect";

		public static readonly StringName SetIceCapPos = "SetIceCapPos";

		public static readonly StringName MapHasSpecialRule = "MapHasSpecialRule";

		public static readonly StringName MapSpecialRulesPreventSleep = "MapSpecialRulesPreventSleep";

		public static readonly StringName GetActiveMapConfig = "GetActiveMapConfig";

		public static readonly StringName SelectNextModLevel = "SelectNextModLevel";

		public static readonly StringName GetMowerConfig = "GetMowerConfig";

		public static readonly StringName GetMowerList = "GetMowerList";

		public static readonly StringName GetMowerNum = "GetMowerNum";

		public static readonly StringName GetMower = "GetMower";

		public static readonly StringName CreateMower = "CreateMower";

		public static readonly StringName GetMowerFeature = "GetMowerFeature";

		public static readonly StringName GetMowerManager = "GetMowerManager";

		public static readonly StringName GetNpcTalk = "GetNpcTalk";

		public static readonly StringName GetPacketBank = "GetPacketBank";

		public static readonly StringName GetPacketBankFeature = "GetPacketBankFeature";

		public static readonly StringName GetPacketBankData = "GetPacketBankData";

		public static readonly StringName GetPacketConfigReadOnlyByCharacterName = "GetPacketConfigReadOnlyByCharacterName";

		public static readonly StringName GetPacketConfigCostLower = "GetPacketConfigCostLower";

		public static readonly StringName GetPacketConfigCostLowerWithTypeList = "GetPacketConfigCostLowerWithTypeList";

		public static readonly StringName GetPacketConfigCostUpper = "GetPacketConfigCostUpper";

		public static readonly StringName GetPacketConfigCostUpperWithTypeList = "GetPacketConfigCostUpperWithTypeList";

		public static readonly StringName GetPacketConfig = "GetPacketConfig";

		public static readonly StringName GetPacketConfigReadOnly = "GetPacketConfigReadOnly";

		public static readonly StringName CreatePacketShow = "CreatePacketShow";

		public static readonly StringName CreatePacketShowWithConfig = "CreatePacketShowWithConfig";

		public static readonly StringName BindSpawnedPacketPicker = "BindSpawnedPacketPicker";

		public static readonly StringName PublishSpawnedPacket = "PublishSpawnedPacket";

		public static readonly StringName GetPortalFeature = "GetPortalFeature";

		public static readonly StringName PortalCreate = "PortalCreate";

		public static readonly StringName ProtalCreate = "ProtalCreate";

		public static readonly StringName PortalChangePos = "PortalChangePos";

		public static readonly StringName ProtalChangePos = "ProtalChangePos";

		public static readonly StringName GetProjectileConfig = "GetProjectileConfig";

		public static readonly StringName QXSunCreate = "QXSunCreate";

		public static readonly StringName _GetCleanCharacters = "_GetCleanCharacters";

		public static readonly StringName _GetNodesInGroupCached = "_GetNodesInGroupCached";

		public static readonly StringName GetSeedBankFeature = "GetSeedBankFeature";

		public static readonly StringName GetCurrentPacketBankMethod = "GetCurrentPacketBankMethod";

		public static readonly StringName GetSeedBank = "GetSeedBank";

		public static readonly StringName ChangeCostAdd = "ChangeCostAdd";

		public static readonly StringName ChangeCostRemove = "ChangeCostRemove";

		public static readonly StringName GetPacketSlotNum = "GetPacketSlotNum";

		public static readonly StringName AddPacket = "AddPacket";

		public static readonly StringName GetSeedBankList = "GetSeedBankList";

		public static readonly StringName GetShovel = "GetShovel";

		public static readonly StringName GetShovelList = "GetShovelList";

		public static readonly StringName YBCreate = "YBCreate";

		public static readonly StringName PublishSpawnedCharacter = "PublishSpawnedCharacter";

		public static readonly StringName BroadcastSpawnedCharacter = "BroadcastSpawnedCharacter";

		public static readonly StringName CanContinueSpawnedCharacterPublish = "CanContinueSpawnedCharacterPublish";

		public static readonly StringName PublishSpawnedCharacterWhenReady = "PublishSpawnedCharacterWhenReady";

		public static readonly StringName DeferSpawnedCharacterPublish = "DeferSpawnedCharacterPublish";

		public static readonly StringName LuckyBagCreate = "LuckyBagCreate";

		public static readonly StringName GoldShardCreate = "GoldShardCreate";

		public static readonly StringName JalapenoSunCreate = "JalapenoSunCreate";

		public static readonly StringName _GetCharacterNode = "_GetCharacterNode";

		public static readonly StringName _IsGameRunning = "_IsGameRunning";

		public static readonly StringName GetSunFeature = "GetSunFeature";

		public static readonly StringName GetTutorial = "GetTutorial";

		public static readonly StringName PickRandomZomie = "PickRandomZomie";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName coinBank = "coinBank";

		public static readonly StringName _groupCacheFrame = "_groupCacheFrame";

		public static readonly StringName _characterCountFrame = "_characterCountFrame";

		public static readonly StringName _characterCountQueryRevision = "_characterCountQueryRevision";

		public static readonly StringName eventBus = "eventBus";

		public static readonly StringName characterRegistry = "characterRegistry";

		public static readonly StringName targetSystem = "targetSystem";

		public static readonly StringName damagePipeline = "damagePipeline";

		public static readonly StringName _coinBank = "_coinBank";

		public static readonly StringName currentControl = "currentControl";

		public static readonly StringName currentLevelConfig = "currentLevelConfig";

		public static readonly StringName currentDynamicLevel = "currentDynamicLevel";

		public static readonly StringName seedbankPacketMax = "seedbankPacketMax";

		public static readonly StringName runGameTime = "runGameTime";

		public static readonly StringName gridSize = "gridSize";

		public static readonly StringName gridBeginPos = "gridBeginPos";

		public static readonly StringName gridNum = "gridNum";

		public static readonly StringName pausePacket = "pausePacket";

		public static readonly StringName pauseZombie = "pauseZombie";

		public static readonly StringName backPacket = "backPacket";

		public static readonly StringName backZombie = "backZombie";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static PackedScene _towerDefenseAwardPacket;

	private static PackedScene _towerDefenseAwardPurse;

	private static PackedScene _towerDefenseAwardCollectable;

	private static PackedScene _towerDefenseAwardTrophy;

	private static PackedScene _towerDefenseZombieBungiSpawn;

	private static PackedScene _towerDefenseEffectParticlesOnce;

	private static PackedScene _towerDefenseEffectSpriteOnce;

	private FallingObjectReplicationCapture _activeFallingObjectReplicationCapture;

	private static FallingObjectConfig _zombieDeathFallingObject;

	private static readonly StringName FeatureName_PacketBank = new StringName("PacketBank");

	private static readonly StringName FeatureName_Portal = new StringName("Portal");

	private static readonly StringName FeatureName_SeedBank = new StringName("SeedBank");

	private static readonly StringName FeatureName_RainMode = new StringName("RainMode");

	private static readonly StringName FeatureName_ConveyorBelt = new StringName("ConveyorBelt");

	private static readonly StringName FeatureName_Glove = new StringName("Glove");

	private static readonly StringName FeatureName_ScreenEffect = new StringName("ScreenEffect");

	private static readonly StringName FeatureName_Sun = new StringName("Sun");

	private static readonly StringName FeatureName_Map = new StringName("Map");

	private static readonly StringName FeatureName_Mower = new StringName("Mower");

	private static readonly StringName FeatureName_Brain = new StringName("Brain");

	private static readonly ILogger _logger = Log.CreateLogger<TowerDefenseManager>();

	private static TowerDefenseBattleFeatureMap _cachedMapFeature = null;

	private static TowerDefenseControlNew _cachedMapFeatureControl = null;

	private static PackedScene _towerDefenseInGamePacketShow;

	internal const int MaxDeathRecordsPerCamp = 512;

	private static readonly TowerDefenseDeathHistory DeathHistory = new TowerDefenseDeathHistory(512);

	private System.Collections.Generic.Dictionary<string, Godot.Collections.Array> _groupCache = new System.Collections.Generic.Dictionary<string, Godot.Collections.Array>();

	private long _groupCacheFrame = -1L;

	private readonly System.Collections.Generic.Dictionary<string, int> _characterCountByName = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);

	private long _characterCountFrame = -1L;

	private ulong _characterCountQueryRevision = 18446744073709551615uL;

	private static System.Collections.Generic.Dictionary<string, TowerDefensePacketConfig> _packetConfigRefCache = new System.Collections.Generic.Dictionary<string, TowerDefensePacketConfig>();

	private static System.Collections.Generic.Dictionary<string, TowerDefenseProjectileConfig> _projectileConfigRefCache = new System.Collections.Generic.Dictionary<string, TowerDefenseProjectileConfig>();

	public BattleEventBus eventBus;

	public TowerDefenseBattleCharacterRegistry characterRegistry;

	public TargetSystem targetSystem;

	public DamagePipeline damagePipeline;

	private CoinBank _coinBank;

	public TowerDefenseControlNew currentControl;

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelBaseConfig currentLevelConfig;

	public int currentDynamicLevel = 3;

	public int seedbankPacketMax = 7;

	public double runGameTime;

	public Vector2 gridSize;

	public Vector2 gridBeginPos;

	public Vector2I gridNum;

	public bool pausePacket;

	public bool pauseZombie;

	public bool backPacket;

	public bool backZombie;

	private static PackedScene _towerDefenseSun;

	private static PackedScene _towerDefenseBrainSun;

	private static PackedScene TOWER_DEFENSE_AWARD_PACKET => _towerDefenseAwardPacket ?? (_towerDefenseAwardPacket = GD.Load<PackedScene>("uid://7l7qsvsxvioi"));

	private static PackedScene TOWER_DEFENSE_AWARD_PURSE => _towerDefenseAwardPurse ?? (_towerDefenseAwardPurse = GD.Load<PackedScene>("uid://mux1v63kv0d8"));

	private static PackedScene TOWER_DEFENSE_AWARD_COLLECTABLE => _towerDefenseAwardCollectable ?? (_towerDefenseAwardCollectable = GD.Load<PackedScene>("uid://d28midqtre16s"));

	private static PackedScene TOWER_DEFENSE_AWARD_TROPHY => _towerDefenseAwardTrophy ?? (_towerDefenseAwardTrophy = GD.Load<PackedScene>("uid://dpoujenmcn5tb"));

	private static PackedScene TOWER_DEFENSE_ZOMBIE_BUNGI_SPAWN => _towerDefenseZombieBungiSpawn ?? (_towerDefenseZombieBungiSpawn = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter5/Bungi/Scene/TowerDefenseZombieBungiSpawn.tscn"));

	private static PackedScene TOWER_DEFENSE_EFFECT_PARTICLES_ONCE => _towerDefenseEffectParticlesOnce ?? (_towerDefenseEffectParticlesOnce = GD.Load<PackedScene>("uid://dbyd0mqkya1j3"));

	private static PackedScene TOWER_DEFENSE_EFFECT_SPRITE_ONCE => _towerDefenseEffectSpriteOnce ?? (_towerDefenseEffectSpriteOnce = GD.Load<PackedScene>("uid://dwvgduivkprow"));

	private static FallingObjectConfig ZOMBIE_DEATH_FALLING_OBJECT => _zombieDeathFallingObject ?? (_zombieDeathFallingObject = GD.Load<FallingObjectConfig>("uid://ct867xau74s6u"));

	private static PackedScene TOWER_DEFENSE_IN_GAME_PACKET_SHOW => _towerDefenseInGamePacketShow ?? (_towerDefenseInGamePacketShow = GD.Load<PackedScene>("uid://bhqecss20rwpb"));

	internal static int DeathRecordCount => DeathHistory.Count;

	public CoinBank coinBank => _coinBank;

	public static bool HasGameplayAuthority
	{
		get
		{
			if (Global.IsMultiplayerMode)
			{
				return MultiPlayerManager.IsHost;
			}
			return true;
		}
	}

	public static TowerDefenseManager Instance { get; private set; }

	public static TowerDefenseControlNew CurrentControl
	{
		get
		{
			if (Instance == null)
			{
				return null;
			}
			return Instance.currentControl;
		}
	}

	private static PackedScene TOWER_DEFENSE_SUN => _towerDefenseSun ?? (_towerDefenseSun = GD.Load<PackedScene>("uid://dk3bkihnh1i0l"));

	private static PackedScene TOWER_DEFENSE_BRAIN_SUN => _towerDefenseBrainSun ?? (_towerDefenseBrainSun = GD.Load<PackedScene>("uid://d161xee5m0kkw"));

	public TowerDefenseAwardBase CreateAward(TowerDefenseEnum.LEVEL_REWARDTYPE type, string itemName, Vector2 pos)
	{
		return type switch
		{
			TowerDefenseEnum.LEVEL_REWARDTYPE.NOONE => CreateAwardFromScene(TOWER_DEFENSE_AWARD_PURSE, itemName, pos), 
			TowerDefenseEnum.LEVEL_REWARDTYPE.PACKET => CreateAwardFromScene(TOWER_DEFENSE_AWARD_PACKET, itemName, pos), 
			TowerDefenseEnum.LEVEL_REWARDTYPE.COLLECTABLE => CreateAwardFromScene(TOWER_DEFENSE_AWARD_COLLECTABLE, itemName, pos), 
			TowerDefenseEnum.LEVEL_REWARDTYPE.COIN => CreateAwardFromScene(TOWER_DEFENSE_AWARD_PURSE, itemName, pos), 
			TowerDefenseEnum.LEVEL_REWARDTYPE.TROPHY => CreateAwardFromScene(TOWER_DEFENSE_AWARD_TROPHY, itemName, pos), 
			_ => null, 
		};
	}

	private TowerDefenseAwardBase CreateAwardFromScene(PackedScene awardScene, string itemName, Vector2 pos)
	{
		Node node = awardScene.Instantiate(PackedScene.GenEditState.Disabled);
		GetCharacterNode().AddChild(node, forceReadableName: false, InternalMode.Disabled);
		((Node2D)node).GlobalPosition = pos;
		((TowerDefenseAwardBase)node).Init(itemName);
		return (TowerDefenseAwardBase)node;
	}

	public TowerDefenseBackgroundMusicConfig GetbackgroundMusicConfig(string backgroundMusic)
	{
		TowerDefenseBackgroundMusicConfig result = null;
		ResourceManager.Instance?.EnsureAllBgmsLoaded();
		if (!string.IsNullOrEmpty(backgroundMusic) && ResourceManager.Instance != null && ResourceManager.Instance.BGMS.ContainsKey(backgroundMusic))
		{
			result = (TowerDefenseBackgroundMusicConfig)ResourceManager.Instance.BGMS[backgroundMusic];
		}
		return result;
	}

	public TowerDefenseSunBase SunCreate(Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.LAND, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		return CreateSunDrop((IsIZMMode() || IsIZM2Mode()) ? ObjectManagerConfig.OBJECT.SUN_BRAIN : ObjectManagerConfig.OBJECT.SUN, EconomyAccountId.Local, SunDropOwnershipPolicy.LegacySharedReplica, pos, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
	}

	public TowerDefenseSunBase SunCreate(EconomyAccountId accountId, Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.LAND, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		if (!CanCreateAccountOwnedSunDrop(accountId))
		{
			return null;
		}
		return CreateSunDrop((IsIZMMode() || IsIZM2Mode()) ? ObjectManagerConfig.OBJECT.SUN_BRAIN : ObjectManagerConfig.OBJECT.SUN, accountId, SunDropOwnershipPolicy.AccountOwned, pos, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
	}

	public Godot.Collections.Array GetCampFriendlyFromArea(TowerDefenseEnum.CHARACTER_CAMP camp, AabbArea2D checkArea, bool fliterGraveStone = true)
	{
		Rect2 checkRect = AabbShapeUtil.ComputeAreaWorldRect(checkArea);
		List<TowerDefenseCharacter> charactersIntersectingRectList = characterRegistry.GetCharactersIntersectingRectList(checkRect);
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseCharacter item in charactersIntersectingRectList)
		{
			if (!item.die && !item.nearDie && !(item is TowerDefenseCrater) && !(item is TowerDefenseItem) && (!fliterGraveStone || !(item is TowerDefenseGravestone)) && item.camp == camp)
			{
				array.Add(item);
			}
		}
		return array;
	}

	public Godot.Collections.Array GetCampFriendlyFromArea(TowerDefenseEnum.CHARACTER_CAMP camp, IAabbAreaQuery2D checkArea, bool fliterGraveStone = true)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		if (checkArea == null || !checkArea.TryGetWorldRect(out var rect))
		{
			return array;
		}
		foreach (TowerDefenseCharacter charactersIntersectingRect in characterRegistry.GetCharactersIntersectingRectList(rect))
		{
			if (!charactersIntersectingRect.die && !charactersIntersectingRect.nearDie && !(charactersIntersectingRect is TowerDefenseCrater) && !(charactersIntersectingRect is TowerDefenseItem) && (!fliterGraveStone || !(charactersIntersectingRect is TowerDefenseGravestone)) && charactersIntersectingRect.camp == camp)
			{
				array.Add(charactersIntersectingRect);
			}
		}
		return array;
	}

	public Godot.Collections.Array GetCampFriendlyLine(TowerDefenseCharacter character, bool fliterGraveStone = true)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseCharacter item in _GetCleanCharacters())
		{
			if (!(item is TowerDefenseCrater) && !(item is TowerDefenseItem) && (!fliterGraveStone || !(item is TowerDefenseGravestone)) && item.camp == character.camp && item.gridPos.Y == character.gridPos.Y)
			{
				array.Add(item);
			}
		}
		return array;
	}

	public Godot.Collections.Array GetCampFriendly(TowerDefenseEnum.CHARACTER_CAMP camp, bool fliterGraveStone = true)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseCharacter item in _GetCleanCharacters())
		{
			if (!item.die && !item.nearDie && !(item is TowerDefenseCrater) && !(item is TowerDefenseItem) && (!fliterGraveStone || !(item is TowerDefenseGravestone)) && item.camp == camp)
			{
				array.Add(item);
			}
		}
		return array;
	}

	public Godot.Collections.Array GetCampTarget(TowerDefenseEnum.CHARACTER_CAMP camp, bool fliterGraveStone = true)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseCharacter item in _GetCleanCharacters())
		{
			if (!item.die && !item.nearDie && !(item is TowerDefenseCrater) && !(item is TowerDefenseItem) && (!fliterGraveStone || !(item is TowerDefenseGravestone)) && item.camp != camp)
			{
				array.Add(item);
			}
		}
		return array;
	}

	public Godot.Collections.Array GetCampTargetFromArray(AabbArea2D checkArea, TowerDefenseEnum.CHARACTER_CAMP camp, bool fliterGraveStone = true)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		Rect2 checkRect = AabbShapeUtil.ComputeAreaWorldRect(checkArea);
		foreach (TowerDefenseCharacter charactersIntersectingRect in characterRegistry.GetCharactersIntersectingRectList(checkRect))
		{
			if (!charactersIntersectingRect.die && !charactersIntersectingRect.nearDie && charactersIntersectingRect.camp != camp && !(charactersIntersectingRect is TowerDefenseCrater) && !(charactersIntersectingRect is TowerDefenseItem) && (!fliterGraveStone || !(charactersIntersectingRect is TowerDefenseGravestone)))
			{
				array.Add(charactersIntersectingRect);
			}
		}
		return array;
	}

	private int CountConveyorPacketsByCharacterName(string characterName)
	{
		int num = 0;
		TowerDefenseBattleFeatureConveyorBelt conveyorBeltFeature = GetConveyorBeltFeature();
		if (GodotObject.IsInstanceValid(conveyorBeltFeature))
		{
			foreach (Node packetChild in conveyorBeltFeature.GetPacketChildren())
			{
				if ((packetChild as TowerDefenseInGamePacketShow).config.saveKey == characterName)
				{
					num++;
				}
			}
		}
		return num;
	}

	private int GetCachedCharacterCount(string characterName)
	{
		if (!GodotObject.IsInstanceValid(characterRegistry))
		{
			return 0;
		}
		long physicsFrames = (long)Engine.GetPhysicsFrames();
		ulong queryRevision = characterRegistry.QueryRevision;
		if (_characterCountFrame != physicsFrames || _characterCountQueryRevision != queryRevision)
		{
			RefreshCharacterCountCache(physicsFrames, queryRevision);
		}
		if (characterName == null || !_characterCountByName.TryGetValue(characterName, out var value))
		{
			return 0;
		}
		return value;
	}

	private void RefreshCharacterCountCache(long currentFrame, ulong currentRevision)
	{
		_characterCountFrame = currentFrame;
		_characterCountQueryRevision = currentRevision;
		_characterCountByName.Clear();
		List<TowerDefenseCharacter> cleanCharactersList = characterRegistry.GetCleanCharactersList();
		for (int i = 0; i < cleanCharactersList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = cleanCharactersList[i];
			if (!towerDefenseCharacter.inGame || towerDefenseCharacter.config == null)
			{
				continue;
			}
			string name = towerDefenseCharacter.config.name;
			if (!string.IsNullOrEmpty(name))
			{
				if (_characterCountByName.TryGetValue(name, out var value))
				{
					_characterCountByName[name] = value + 1;
				}
				else
				{
					_characterCountByName[name] = 1;
				}
			}
		}
	}

	public int GetCharacterNum(string characterName, bool containConveyor = false)
	{
		int num = GetCachedCharacterCount(characterName);
		if (containConveyor)
		{
			num += CountConveyorPacketsByCharacterName(characterName);
		}
		return num;
	}

	public Godot.Collections.Array GetCharacterFromName(string characterName)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseCharacter item in _GetCleanCharacters())
		{
			if (item.inGame && item.config.name == characterName)
			{
				array.Add(item);
			}
		}
		return array;
	}

	public TowerDefenseBattleFeatureRainMode GetRainModeFeature()
	{
		TowerDefenseControlNew towerDefenseControlNew = currentControl;
		if (towerDefenseControlNew != null)
		{
			return towerDefenseControlNew.GetFeature(FeatureName_RainMode) as TowerDefenseBattleFeatureRainMode;
		}
		return null;
	}

	public TowerDefenseBattleFeatureConveyorBelt GetConveyorBeltFeature()
	{
		if (currentControl == null)
		{
			return null;
		}
		if (currentControl.GetFeature(FeatureName_ConveyorBelt) is TowerDefenseBattleFeatureConveyorBelt result)
		{
			return result;
		}
		return null;
	}

	public TowerDefenseBattleFeatureGlove GetGloveFeature()
	{
		TowerDefenseControlNew towerDefenseControlNew = currentControl;
		if (towerDefenseControlNew != null)
		{
			return towerDefenseControlNew.GetFeature(FeatureName_Glove) as TowerDefenseBattleFeatureGlove;
		}
		return null;
	}

	public TowerDefenseBattleFeatureScreenEffect GetScreenEffectFeature()
	{
		TowerDefenseControlNew towerDefenseControlNew = currentControl;
		if (towerDefenseControlNew != null)
		{
			return towerDefenseControlNew.GetFeature(FeatureName_ScreenEffect) as TowerDefenseBattleFeatureScreenEffect;
		}
		return null;
	}

	public TowerDefenseBattleFeatureBrain GetBrainFeature()
	{
		TowerDefenseControlNew towerDefenseControlNew = currentControl;
		if (towerDefenseControlNew != null)
		{
			return towerDefenseControlNew.GetFeature(FeatureName_Brain) as TowerDefenseBattleFeatureBrain;
		}
		return null;
	}

	public TowerDefenseBattleProcess GetCurrentProcess()
	{
		if (currentControl == null)
		{
			return null;
		}
		return currentControl.process;
	}

	public Godot.Collections.Array GetPlant()
	{
		return _GetNodesInGroupCached("Plant");
	}

	public Godot.Collections.Array GetZombie()
	{
		return _GetNodesInGroupCached("Zombie");
	}

	public Godot.Collections.Array GetCharacter()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseCharacter item in _GetCleanCharacters())
		{
			if (!item.characterFilter)
			{
				array.Add(item);
			}
		}
		return array;
	}

	public Godot.Collections.Array GetProjectile()
	{
		return _GetNodesInGroupCached("Projectile");
	}

	public Godot.Collections.Array GetEffect()
	{
		return _GetNodesInGroupCached("Effect");
	}

	public int GetEffectCount()
	{
		return characterRegistry.GetEffectCount();
	}

	internal int GetEffectCountForPhysicsFrame(ulong physicsFrame)
	{
		return characterRegistry.GetEffectCountForPhysicsFrame(physicsFrame);
	}

	public Godot.Collections.Array GetLineCharacters(int line)
	{
		return characterRegistry.GetLineCharacters(line);
	}

	public List<TowerDefenseCharacter> GetCharacterColumn(int column, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterColumn(column, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterColumnList(int column, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterColumnList(column, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetFarFromArray(TowerDefenseCharacter character, List<TowerDefenseCharacter> array, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetFarFromArray(character, array, method, checkLine, fliterGravestone);
	}

	public bool GetCharacterHasTarget(TowerDefenseCharacter character, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterHasTarget(character, checkLine, fliterGraveStone);
	}

	public bool GetCharacterHasTargetFromArray(TowerDefenseCharacter character, List<TowerDefenseCharacter> array, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterHasTargetFromArray(character, array, checkLine, fliterGraveStone);
	}

	public bool GetCharacterHasTargetFromArea(TowerDefenseCharacter character, AabbArea2D checkArea, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterHasTargetFromArea(character, checkArea, checkLine, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetLineWithCollisionFlags(TowerDefenseCharacter character, int collisionFlags, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetLineWithCollisionFlags(character, collisionFlags, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetLineFromArea(TowerDefenseCharacter character, AabbArea2D checkArea, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetLineFromArea(character, checkArea, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetLineFromAreaWithCollisionFlags(TowerDefenseCharacter character, int collisionFlags, AabbArea2D checkArea, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetLineFromAreaWithCollisionFlags(character, collisionFlags, checkArea, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetFromAreaWithCollisionFlags(TowerDefenseCharacter character, int collisionFlags, AabbArea2D checkArea, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetFromAreaWithCollisionFlags(character, collisionFlags, checkArea, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterLine(int line, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterLine(line, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterLineList(int line, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterLineList(line, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetLine(TowerDefenseCharacter character, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetLine(character, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetLineList(TowerDefenseCharacter character, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetLineList(character, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetLineFromArray(TowerDefenseCharacter character, List<TowerDefenseCharacter> array, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetLineFromArray(character, array, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTarget(TowerDefenseCharacter character, bool checkLine = false, bool checkCollision = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTarget(character, checkLine, checkCollision, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetList(TowerDefenseCharacter character, bool checkLine = false, bool checkCollision = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetList(character, checkLine, checkCollision, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetNearFromArea(TowerDefenseCharacter character, AabbArea2D checkArea, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetNearFromArea(character, checkArea, method, checkLine, fliterGravestone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetFarFromArrayWithCollisionFlags(TowerDefenseCharacter character, int collisionFlags, List<TowerDefenseCharacter> array, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetFarFromArrayWithCollisionFlags(character, collisionFlags, array, method, checkLine, fliterGravestone);
	}

	public TowerDefenseCharacter GetCharacterTargetNearestFromArrayWithCollisionFlags(TowerDefenseCharacter character, int collisionFlags, List<TowerDefenseCharacter> array, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetNearestFromArrayWithCollisionFlags(character, collisionFlags, array, method, checkLine, fliterGravestone);
	}

	public TowerDefenseCharacter GetCharacterTargetFarthestFromArrayWithCollisionFlags(TowerDefenseCharacter character, int collisionFlags, List<TowerDefenseCharacter> array, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetFarthestFromArrayWithCollisionFlags(character, collisionFlags, array, method, checkLine, fliterGravestone);
	}

	public TowerDefenseCharacter GetCharacterTargetNearest(TowerDefenseCharacter character, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetNearest(character, method, checkLine, fliterGravestone);
	}

	public TowerDefenseCharacter GetCharacterTargetNearestFromArray(TowerDefenseCharacter character, List<TowerDefenseCharacter> array, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetNearestFromArray(character, array, method, checkLine, fliterGravestone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetNear(TowerDefenseCharacter character, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetNear(character, method, checkLine, fliterGravestone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetNearFromArray(TowerDefenseCharacter character, List<TowerDefenseCharacter> array, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetNearFromArray(character, array, method, checkLine, fliterGravestone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetNearFromArrayWithCollisionFlags(TowerDefenseCharacter character, int collisionFlags, List<TowerDefenseCharacter> array, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetNearFromArrayWithCollisionFlags(character, collisionFlags, array, method, checkLine, fliterGravestone);
	}

	public TowerDefenseCharacter GetNearCharacter(TowerDefenseCharacter character, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false, bool fliterVase = true)
	{
		return targetSystem.GetNearCharacter(character, method, checkLine, fliterGravestone, fliterVase);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetFromRectWithCollisionFlags(TowerDefenseCharacter character, int collisionFlags, Rect2 checkRect, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetFromRectWithCollisionFlags(character, collisionFlags, checkRect, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetLineFromRectWithCollisionFlags(TowerDefenseCharacter character, int collisionFlags, Rect2 checkRect, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetLineFromRectWithCollisionFlags(character, collisionFlags, checkRect, fliterGraveStone);
	}

	public TowerDefenseCharacter GetTallCharacterTargetFromRect(TowerDefenseCharacter character, Rect2 checkRect, bool checkLine = false, int line = 0, double groundRight = 1.0 / 0.0)
	{
		return targetSystem.GetTallCharacterTargetFromRect(character, checkRect, checkLine, line, groundRight);
	}

	public List<TowerDefenseCharacter> GetCharactersIntersectingRect(Rect2 checkRect, bool checkLine = false, int line = 0)
	{
		return targetSystem.GetCharactersIntersectingRect(checkRect, checkLine, line);
	}

	public List<TowerDefenseCharacter> GetOverlappingCharactersFromRect(Rect2 checkRect, bool checkLine = false, int line = 0)
	{
		return GetCharactersIntersectingRect(checkRect, checkLine, line);
	}

	public TowerDefenseCharacter GetCharacterTargetNearestFromRect(TowerDefenseCharacter character, Rect2 checkRect, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetNearestFromRect(character, checkRect, method, fliterGravestone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetFromRect(TowerDefenseCharacter character, Rect2 checkRect, bool checkLine = false, bool fliterGraveStone = true, bool fliterVase = true)
	{
		return targetSystem.GetCharacterTargetFromRect(character, checkRect, checkLine, fliterGraveStone, fliterVase);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetFromRectList(TowerDefenseCharacter character, Rect2 checkRect, bool checkLine = false, bool fliterGraveStone = true, bool fliterVase = true)
	{
		return targetSystem.GetCharacterTargetFromRectList(character, checkRect, checkLine, fliterGraveStone, fliterVase);
	}

	public bool GetCharacterHasTargetFromRect(TowerDefenseCharacter character, Rect2 checkRect, bool checkLine = false, bool fliterGraveStone = true, bool fliterVase = true)
	{
		return targetSystem.GetCharacterHasTargetFromRect(character, checkRect, checkLine, fliterGraveStone, fliterVase);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetFromArray(TowerDefenseCharacter character, List<TowerDefenseCharacter> array, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetFromArray(character, array, checkLine, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetFromArrayWithCollisionFlags(TowerDefenseCharacter character, int collisionFlags, List<TowerDefenseCharacter> array, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetCharacterTargetFromArrayWithCollisionFlags(character, collisionFlags, array, checkLine, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetFromArea(TowerDefenseCharacter character, AabbArea2D checkArea, bool checkLine = false, bool fliterGraveStone = true, bool fliterVase = true)
	{
		return targetSystem.GetCharacterTargetFromArea(character, checkArea, checkLine, fliterGraveStone, fliterVase);
	}

	public List<TowerDefenseCharacter> GetCharacterTargetFromAreaList(TowerDefenseCharacter character, AabbArea2D checkArea, bool checkLine = false, bool fliterGraveStone = true, bool fliterVase = true)
	{
		return targetSystem.GetCharacterTargetFromAreaList(character, checkArea, checkLine, fliterGraveStone, fliterVase);
	}

	public bool HasTrackTarget(TowerDefenseCharacter parent, int collectionFlag, bool canTargetGargantuar, float groundRight)
	{
		return targetSystem.HasTrackTarget(parent, collectionFlag, canTargetGargantuar, groundRight);
	}

	public TowerDefenseCharacter GetCharacterTargetNearestFromArea(TowerDefenseCharacter character, AabbArea2D checkArea, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool fliterGravestone = false)
	{
		return targetSystem.GetCharacterTargetNearestFromArea(character, checkArea, method, fliterGravestone);
	}

	public Godot.Collections.Array GetCharactersForLine(int line)
	{
		return characterRegistry.GetCharactersForLine(line);
	}

	public bool GetProjectileHasTarget(TowerDefenseProjectile projectile, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetProjectileHasTarget(projectile, checkLine, fliterGraveStone);
	}

	public bool GetProjectileHasTargetFromArray(TowerDefenseProjectile projectile, List<TowerDefenseCharacter> array, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetProjectileHasTargetFromArray(projectile, array, checkLine, fliterGraveStone);
	}

	public bool GetProjectileHasTargetFromArea(TowerDefenseProjectile projectile, AabbArea2D checkArea, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetProjectileHasTargetFromArea(projectile, checkArea, checkLine, fliterGraveStone);
	}

	public TowerDefenseCharacter GetProjectileTargetNearest(TowerDefenseProjectile projectile, int collisionFlags = -1, bool fliterGravestone = true)
	{
		return targetSystem.GetProjectileTargetNearest(projectile, collisionFlags, fliterGravestone);
	}

	public TowerDefenseCharacter GetProjectileTargetNearest(Vector2 pos, Vector2I gridPos, int collisionFlags, TowerDefenseEnum.CHARACTER_CAMP camp, double speed, bool fliterGravestone = true, bool deprioritizeDisabledTargets = false)
	{
		return targetSystem.GetProjectileTargetNearest(pos, gridPos, collisionFlags, camp, speed, fliterGravestone, 18446744073709551615uL, deprioritizeDisabledTargets);
	}

	public TowerDefenseCharacter GetProjectileTargetNearestForPhysicsFrame(Vector2 pos, Vector2I gridPos, int collisionFlags, TowerDefenseEnum.CHARACTER_CAMP camp, double speed, ulong physicsFrame, bool fliterGravestone = true, bool deprioritizeDisabledTargets = false)
	{
		return targetSystem.GetProjectileTargetNearest(pos, gridPos, collisionFlags, camp, speed, fliterGravestone, physicsFrame, deprioritizeDisabledTargets);
	}

	public List<TowerDefenseCharacter> GetProjectileTargetNear(TowerDefenseProjectile projectile, int collisionFlags = -1, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = false)
	{
		return targetSystem.GetProjectileTargetNear(projectile, collisionFlags, method, checkLine, fliterGravestone);
	}

	public List<TowerDefenseCharacter> GetProjectileTargetNearProjectile(TowerDefenseProjectile projectile, int collisionFlags = -1, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool checkLine = false, bool fliterGravestone = true)
	{
		return targetSystem.GetProjectileTargetNearProjectile(projectile, collisionFlags, method, checkLine, fliterGravestone);
	}

	public List<TowerDefenseCharacter> GetProjectileTarget(TowerDefenseProjectile projectile, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetProjectileTarget(projectile, checkLine, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetProjectileTargetList(TowerDefenseProjectile projectile, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetProjectileTargetList(projectile, checkLine, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetProjectileTargetFromArray(TowerDefenseProjectile projectile, List<TowerDefenseCharacter> array, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetProjectileTargetFromArray(projectile, array, checkLine, fliterGraveStone);
	}

	public List<TowerDefenseCharacter> GetProjectileTargetFromArea(TowerDefenseProjectile projectile, AabbArea2D checkArea, bool checkLine = false, bool fliterGraveStone = true)
	{
		return targetSystem.GetProjectileTargetFromArea(projectile, checkArea, checkLine, fliterGraveStone);
	}

	public TowerDefenseCharacter GetProjectileTargetNearestProjectile(TowerDefenseProjectile projectile, int collisionFlags = -1, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.DEFAULT, bool fliterGravestone = true)
	{
		return targetSystem.GetProjectileTargetNearestProjectile(projectile, collisionFlags, method, fliterGravestone);
	}

	public TowerDefenseCharacter GetProjectileInitialTrackTarget(Vector2 projectilePos, int collisionFlags, TowerDefenseEnum.CHARACTER_CAMP camp, TowerDefenseEnum.TARGET_NEAR_METHOD method = TowerDefenseEnum.TARGET_NEAR_METHOD.POSITION, bool fliterGravestone = true)
	{
		return targetSystem.GetProjectileInitialTrackTarget(projectilePos, collisionFlags, camp, method, fliterGravestone);
	}

	public TowerDefenseSunBase BrainSunCreate(Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.LAND, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		return CreateSunDrop(ObjectManagerConfig.OBJECT.SUN_BRAIN, EconomyAccountId.Local, SunDropOwnershipPolicy.LegacySharedReplica, pos, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
	}

	public TowerDefenseSunBase BrainSunCreate(EconomyAccountId accountId, Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.LAND, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		if (!CanCreateAccountOwnedSunDrop(accountId))
		{
			return null;
		}
		return CreateSunDrop(ObjectManagerConfig.OBJECT.SUN_BRAIN, accountId, SunDropOwnershipPolicy.AccountOwned, pos, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
	}

	public TowerDefenseZombie BungiSpawn(string packetName, Vector2I gridPos, TowerDefenseCharacterOverride override_ = null, bool hypnoses = false, int waveOperationId = -1, bool skipPlacementCheck = false)
	{
		if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			return null;
		}
		TowerDefenseBattleFeatureWave instance = TowerDefenseBattleFeatureWave.Instance;
		if (!IsBungiWaveOperationCurrent(waveOperationId, instance))
		{
			return null;
		}
		if (!TryGetBungiSpawnContext(out var battleControl, out var characterNode))
		{
			return null;
		}
		int battleOperationId = battleControl.BeginPendingBattleOperation();
		int ownedWaveOperationId = (GodotObject.IsInstanceValid(instance) ? instance.BeginPendingSpawnOperation() : (-1));
		TowerDefenseZombieBungiSpawn towerDefenseZombieBungiSpawn = null;
		try
		{
			towerDefenseZombieBungiSpawn = CreateBungiSpawnNode(packetName, gridPos, override_, instance, ownedWaveOperationId, battleControl, battleOperationId, characterNode);
		}
		catch (Exception value)
		{
			CleanupFailedBungiSpawn(battleControl, battleOperationId, instance, ownedWaveOperationId, towerDefenseZombieBungiSpawn);
			GD.PushError($"[BungiSpawn] Failed to create {packetName}: {value}");
			return null;
		}
		if (hypnoses)
		{
			towerDefenseZombieBungiSpawn.Hypnoses();
		}
		SyncBungiSpawn(packetName, gridPos, hypnoses, towerDefenseZombieBungiSpawn, battleControl, skipPlacementCheck);
		return towerDefenseZombieBungiSpawn;
	}

	private static bool IsBungiWaveOperationCurrent(int waveOperationId, TowerDefenseBattleFeatureWave waveFeature)
	{
		if (waveOperationId < 0)
		{
			return true;
		}
		if (GodotObject.IsInstanceValid(waveFeature))
		{
			return waveFeature.IsPendingSpawnOperationCurrent(waveOperationId);
		}
		return false;
	}

	private bool TryGetBungiSpawnContext(out TowerDefenseControlNew battleControl, out Node characterNode)
	{
		battleControl = currentControl;
		characterNode = GetCharacterNode();
		if (!GodotObject.IsInstanceValid(battleControl) || !GodotObject.IsInstanceValid(characterNode))
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(battleControl.levelControl) && battleControl.levelControl.awardCreate)
		{
			return false;
		}
		return true;
	}

	private static TowerDefenseZombieBungiSpawn CreateBungiSpawnNode(string packetName, Vector2I gridPos, TowerDefenseCharacterOverride override_, TowerDefenseBattleFeatureWave waveFeature, int ownedWaveOperationId, TowerDefenseControlNew battleControl, int battleOperationId, Node characterNode)
	{
		TowerDefenseZombieBungiSpawn towerDefenseZombieBungiSpawn = TOWER_DEFENSE_ZOMBIE_BUNGI_SPAWN.Instantiate<TowerDefenseZombieBungiSpawn>(PackedScene.GenEditState.Disabled);
		towerDefenseZombieBungiSpawn.Set("characterName", packetName);
		towerDefenseZombieBungiSpawn.Set("override", override_);
		towerDefenseZombieBungiSpawn.waveOperationOwner = waveFeature;
		towerDefenseZombieBungiSpawn.waveOperationId = ownedWaveOperationId;
		towerDefenseZombieBungiSpawn.battleOperationOwner = battleControl;
		towerDefenseZombieBungiSpawn.battleOperationId = battleOperationId;
		towerDefenseZombieBungiSpawn.SetLogicalGlobalPosition(GetMapCellPlantPos(gridPos));
		towerDefenseZombieBungiSpawn.gridPos = gridPos;
		characterNode.AddChild(towerDefenseZombieBungiSpawn, forceReadableName: false, InternalMode.Disabled);
		return towerDefenseZombieBungiSpawn;
	}

	private static void CleanupFailedBungiSpawn(TowerDefenseControlNew battleControl, int battleOperationId, TowerDefenseBattleFeatureWave waveFeature, int ownedWaveOperationId, TowerDefenseZombieBungiSpawn zombie)
	{
		battleControl.CompletePendingBattleOperation(battleOperationId);
		if (ownedWaveOperationId >= 0 && GodotObject.IsInstanceValid(waveFeature))
		{
			waveFeature.CompletePendingSpawnOperation(ownedWaveOperationId);
		}
		if (GodotObject.IsInstanceValid(zombie) && !zombie.IsQueuedForDeletion())
		{
			zombie.QueueFree();
		}
	}

	private static void SyncBungiSpawn(string packetName, Vector2I gridPos, bool hypnoses, TowerDefenseZombieBungiSpawn zombie, TowerDefenseControlNew battleControl, bool skipPlacementCheck)
	{
		if (Global.Instance.isMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			int nextSyncId = battleControl.GetNextSyncId();
			double hitpointScale = (GodotObject.IsInstanceValid(zombie.instance) ? zombie.instance.hitpointScale : 1.0);
			double scaleVal = (GodotObject.IsInstanceValid(zombie.transformPoint) ? ((double)zombie.transformPoint.Scale.X) : 1.0);
			Dictionary spawnState = (skipPlacementCheck ? new Dictionary { ["plant_skip_placement_check"] = true } : null);
			battleControl.RegisterSyncCharacter(nextSyncId, zombie);
			MultiPlayerManager.Instance.SendSpawnCharacterAt(packetName, gridPos.X, gridPos.Y, nextSyncId, hitpointScale, scaleVal, hypnoses, 0.0, useCreate: false, 0.0, 0.0, walkAfterSpawn: false, 0.0, "", spawnState);
		}
	}

	public static Node2D GetCharacterNode()
	{
		if (Instance == null)
		{
			return null;
		}
		if (Global.Instance.isEditor && SceneManager.CurrentScene == "LevelEditorStage" && GodotObject.IsInstanceValid(LevelEditorMapEditor.instance))
		{
			return LevelEditorMapEditor.instance.characterNode;
		}
		if (!GodotObject.IsInstanceValid(Instance.currentControl))
		{
			return ObjectManager.Instance;
		}
		return Instance.currentControl.characterNode;
	}

	public void CharacterRegister(TowerDefenseCharacter character)
	{
		characterRegistry.Register(character);
	}

	public void CharacterUnregister(TowerDefenseCharacter character)
	{
		characterRegistry.Unregister(character);
	}

	public static AdobeAnimateSprite GetCharacterSprite(string charcterSpriteName)
	{
		PackedScene characterSprite = ResourceManager.Instance.GetCharacterSprite(charcterSpriteName);
		if (!GodotObject.IsInstanceValid(characterSprite))
		{
			return null;
		}
		return characterSprite.Instantiate(PackedScene.GenEditState.Disabled) as AdobeAnimateSprite;
	}

	public static PackedScene GetPacketSpriteScene(TowerDefensePacketConfig config)
	{
		if (!GodotObject.IsInstanceValid(config) || ResourceManager.Instance == null)
		{
			return null;
		}
		if (!string.IsNullOrEmpty(config.saveKey) && ResourceManager.Instance.CHARCTAER_SPRITE.TryGetValue(config.saveKey, out var value) && value is PackedScene result)
		{
			return result;
		}
		TowerDefenseCharacterConfig characterConfig = config.characterConfig;
		if (!GodotObject.IsInstanceValid(characterConfig) || string.IsNullOrEmpty(characterConfig.name))
		{
			return null;
		}
		return ResourceManager.Instance.GetCharacterSprite(characterConfig.name);
	}

	public static AdobeAnimateSprite GetPacketSprite(TowerDefensePacketConfig config)
	{
		PackedScene packetSpriteScene = GetPacketSpriteScene(config);
		if (!GodotObject.IsInstanceValid(packetSpriteScene))
		{
			return null;
		}
		return packetSpriteScene.Instantiate(PackedScene.GenEditState.Disabled) as AdobeAnimateSprite;
	}

	public static PackedScene GetChacraterScene(string charcterName)
	{
		return ResourceManager.Instance.GetCharacterScene(charcterName);
	}

	public TowerDefenseCharacter CreateCharacter(string characterName, Vector2I gridPos = default(Vector2I))
	{
		if (gridPos == default(Vector2I))
		{
			gridPos = new Vector2I(-1, -1);
		}
		if (!ModLoader.TryInstantiateEffectiveCharacter(characterName, out var character, out var diagnostic))
		{
			if (!string.IsNullOrWhiteSpace(diagnostic))
			{
				GD.PushWarning("[TowerDefenseManager] Mod 角色脚本实例化失败，回退内置场景路径：" + characterName + ": " + diagnostic);
			}
			character = GetChacraterScene(characterName).Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
		}
		character.gridPos = gridPos;
		return character;
	}

	public long GetCoin()
	{
		if (_coinBank != null)
		{
			return _coinBank.num;
		}
		return 0L;
	}

	public void AddCoin(long num)
	{
		if (_coinBank != null)
		{
			_coinBank.AddNum(num);
		}
	}

	public void UseCoin(long num)
	{
		if (_coinBank != null)
		{
			_coinBank.UseCoin(num);
		}
	}

	public static CollectableConfig GetCollectable(string collectableName)
	{
		return (CollectableConfig)ResourceManager.Instance.COLLECTABLES[collectableName];
	}

	public override void _Ready()
	{
		Instance = this;
		eventBus = BattleEventBus.Instance;
		characterRegistry = new TowerDefenseBattleCharacterRegistry();
		AddChild(characterRegistry, forceReadableName: false, InternalMode.Disabled);
		targetSystem = new TargetSystem(characterRegistry, this);
		AddChild(targetSystem, forceReadableName: false, InternalMode.Disabled);
		damagePipeline = new DamagePipeline();
		BattleEventBus.Instance.OnCharacterDestroy += CharacterDestroy;
		_coinBank = GetNode<CoinBank>("%CoinBank");
		GameSaveManager.Instance?.SyncCoinBankFromSave();
	}

	public void MapIsChange()
	{
		gridSize = GetMapGridSize();
		gridBeginPos = GetMapGridBeginPos();
		gridNum = GetMapGridNum();
		ReevaluateCharacterSleepStates();
	}

	private void ReevaluateCharacterSleepStates()
	{
		if (!GodotObject.IsInstanceValid(characterRegistry))
		{
			return;
		}
		List<TowerDefenseCharacter> cleanCharactersList = characterRegistry.GetCleanCharactersList();
		for (int i = 0; i < cleanCharactersList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = cleanCharactersList[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				(towerDefenseCharacter.componentManager?.GetRuntime<SleepComponent>("character.sleep"))?.ReevaluateEnvironmentState();
			}
		}
	}

	public void CharacterDestroy(TowerDefensePacketConfig _packet, Vector2 pos, Vector2 _gridPos, TowerDefenseEnum.CHARACTER_CAMP _camp, double _scale = 1.0, double _hitpointScale = 1.0)
	{
		if (GodotObject.IsInstanceValid(_packet) && _packet.characterConfig is TowerDefenseZombieConfig)
		{
			DeathHistory.Add(new TowerDefenseDeathRecord(_packet, pos, (Vector2I)_gridPos, _camp, _scale, _hitpointScale, invisible: false, _packet.saveKey != "ZombieAngel" && pos.X >= GetMapCellPos(new Vector2I(4, 0)).X));
		}
	}

	public static void ClearDeathRecords()
	{
		DeathHistory.Clear();
	}

	internal static bool HasDeathRecord(TowerDefenseEnum.CHARACTER_CAMP camp, bool requireAngelEligible = false)
	{
		return DeathHistory.HasDeathRecord(camp, requireAngelEligible);
	}

	internal static bool TryTakeLatestDeathRecord(TowerDefenseEnum.CHARACTER_CAMP camp, bool requireAngelEligible, out TowerDefenseDeathRecord record)
	{
		return DeathHistory.TryTakeLatest(camp, requireAngelEligible, out record);
	}

	public string GetEffectDirtName()
	{
		GeneralEnum.HOMEWORLD levelHomeworld = GetLevelHomeworld();
		Variant variant = ResourceManager.Instance.Get("effect_dirt_name");
		if (variant.VariantType == Variant.Type.Nil)
		{
			return "DirtSpawnDirt";
		}
		Dictionary dictionary = variant.AsGodotDictionary();
		int from = (int)levelHomeworld;
		if (!dictionary.ContainsKey(Variant.From(in from)))
		{
			return "DirtSpawnDirt";
		}
		from = (int)levelHomeworld;
		return dictionary[Variant.From(in from)].AsString();
	}

	public AdobeAnimateSprite GetEffectSprite(string effectSpriteName)
	{
		return ResourceManager.Instance.GetCharacterSprite(effectSpriteName).Instantiate(PackedScene.GenEditState.Disabled) as AdobeAnimateSprite;
	}

	public static TowerDefenseEffectParticlesOnce CreateEffectParticlesOnce(PackedScene scene, Vector2I gridPos = default(Vector2I))
	{
		if (gridPos == default(Vector2I))
		{
			gridPos = new Vector2I(-1, -1);
		}
		TowerDefenseEffectParticlesOnce obj = TOWER_DEFENSE_EFFECT_PARTICLES_ONCE.Instantiate(PackedScene.GenEditState.Disabled) as TowerDefenseEffectParticlesOnce;
		obj.Init(scene);
		obj.gridPos = gridPos;
		return obj;
	}

	public static TowerDefenseEffectSpriteOnce CreateEffectSpriteOnce(PackedScene scene, Vector2I gridPos = default(Vector2I), string clip = "")
	{
		if (gridPos == default(Vector2I))
		{
			gridPos = new Vector2I(-1, -1);
		}
		TowerDefenseEffectSpriteOnce obj = TOWER_DEFENSE_EFFECT_SPRITE_ONCE.Instantiate(PackedScene.GenEditState.Disabled) as TowerDefenseEffectSpriteOnce;
		obj.Init(scene, clip);
		obj.gridPos = gridPos;
		return obj;
	}

	public static bool TryCreateEffectSpriteOnceGpu(PackedScene scene, out TowerDefenseEffectSpriteOnceGpuHandle handle, Vector2I gridPos = default(Vector2I), string clips = "", Transform2D? transform = null, Action<string> onAnimeCompleted = null)
	{
		handle = default;
		if (scene == null)
		{
			return false;
		}
		if (gridPos == default(Vector2I))
		{
			gridPos = new Vector2I(-1, -1);
		}
		TowerDefenseEffectSpriteOnceBatcher orCreateNodeFreeFast = TowerDefenseEffectSpriteOnceBatcher.GetOrCreateNodeFreeFast();
		if (orCreateNodeFreeFast == null)
		{
			return false;
		}
		if (!transform.HasValue)
		{
			return orCreateNodeFreeFast.TryRegisterNodeFreeIdentity(scene, gridPos, clips, onAnimeCompleted, out handle);
		}
		return orCreateNodeFreeFast.TryRegisterNodeFree(scene, gridPos, clips, transform.Value, onAnimeCompleted, out handle);
	}

	public TowerDefenseEffectParticlesOnce CreateEffectParticlesSceneOnce(GPUParticles2DOnece scene, Vector2I gridPos = default(Vector2I))
	{
		if (gridPos == default(Vector2I))
		{
			gridPos = new Vector2I(-1, -1);
		}
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseEffectParticlesOnce.Create();
		towerDefenseEffectParticlesOnce.InitScene(scene);
		towerDefenseEffectParticlesOnce.gridPos = gridPos;
		return towerDefenseEffectParticlesOnce;
	}

	public TowerDefenseEffectSpriteOnce CreateEffectSpriteSceneOnce(AdobeAnimateSprite scene, Vector2I gridPos = default(Vector2I), string clip = "")
	{
		if (gridPos == default(Vector2I))
		{
			gridPos = new Vector2I(-1, -1);
		}
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseEffectSpriteOnce.Create();
		towerDefenseEffectSpriteOnce.InitScene(scene, clip);
		towerDefenseEffectSpriteOnce.gridPos = gridPos;
		return towerDefenseEffectSpriteOnce;
	}

	public static bool TryCreateEffectSceneOnceFast(PackedScene scene, Node2D parent, Vector2I gridPosition, Vector2 globalPosition, bool preferSprite)
	{
		if (scene == null || !GodotObject.IsInstanceValid(parent) || !globalPosition.IsFinite())
		{
			return false;
		}
		Transform2D value = new Transform2D(0f, globalPosition);
		TowerDefenseEffectSpriteOnceGpuHandle handle;
		TowerDefenseEffectParticlesOnce effect;
		if (preferSprite)
		{
			if (TryCreateEffectSpriteOnceGpu(scene, out handle, gridPosition, "", value))
			{
				return true;
			}
			return TowerDefenseEffectParticlesOnce.TrySpawnRetained(scene, parent, gridPosition, globalPosition, out effect);
		}
		if (TowerDefenseEffectParticlesOnce.TrySpawnRetained(scene, parent, gridPosition, globalPosition, out effect))
		{
			return true;
		}
		return TryCreateEffectSpriteOnceGpu(scene, out handle, gridPosition, "", value);
	}

	public TowerDefenseGroundItemBase FallingObjectCreate(Vector2 pos, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 0.0)
	{
		if (!TryPickZombieDeathFallingObject(out var id))
		{
			return null;
		}
		return FallingObjectItemCreate(id, pos, height, velocity, gravity);
	}

	public bool TryPickZombieDeathFallingObject(out ObjectManagerConfig.OBJECT id)
	{
		id = (GodotObject.IsInstanceValid(ZOMBIE_DEATH_FALLING_OBJECT) ? ZOMBIE_DEATH_FALLING_OBJECT.Pick() : ObjectManagerConfig.OBJECT.NOONE);
		return id != ObjectManagerConfig.OBJECT.NOONE;
	}

	public TowerDefenseGroundItemBase FallingObjectItemCreate(ObjectManagerConfig.OBJECT id, Vector2 pos, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 0.0)
	{
		if (id == ObjectManagerConfig.OBJECT.NOONE)
		{
			return null;
		}
		Node2D characterNode = GetCharacterNode();
		Node node = ObjectManager.PoolPop(id, characterNode);
		if (!(node is Node2D node2D))
		{
			if (GodotObject.IsInstanceValid(node))
			{
				ObjectManager.PoolPush(id, node);
			}
			return null;
		}
		if (!(node is TowerDefenseGroundItemBase towerDefenseGroundItemBase))
		{
			ObjectManager.PoolPush(id, node);
			return null;
		}
		node2D.GlobalPosition = pos;
		if (node is TowerDefenseCoinBase towerDefenseCoinBase)
		{
			DropItemConfig byId = DropItemRegistry.GetById(id);
			if (byId != null && byId.Value > 0)
			{
				towerDefenseCoinBase.num = byId.Value;
			}
			towerDefenseCoinBase.Init(height, velocity, gravity);
		}
		CaptureFallingObjectSpawn(towerDefenseGroundItemBase, id, pos, height, velocity, gravity);
		return towerDefenseGroundItemBase;
	}

	internal IDisposable BeginFallingObjectReplicationCapture()
	{
		return new FallingObjectReplicationCapture(this, _activeFallingObjectReplicationCapture);
	}

	private void CaptureFallingObjectSpawn(TowerDefenseGroundItemBase item, ObjectManagerConfig.OBJECT objectId, Vector2 position, double height, Vector2 velocity, double gravity)
	{
		_activeFallingObjectReplicationCapture?.Record(item, objectId, position, height, velocity, gravity);
	}

	private void FlushFallingObjectSpawns(List<FallingObjectSpawnRecord> records)
	{
		if (!Global.IsMultiplayerMode || !MultiPlayerManager.IsHost)
		{
			return;
		}
		foreach (FallingObjectSpawnRecord record in records)
		{
			if (GodotObject.IsInstanceValid(record.Item))
			{
				MultiPlayerManager.Instance?.SendSpawnFallingObject(record.ObjectId, record.Position.X, record.Position.Y, record.Velocity.X, record.Velocity.Y, record.Gravity, record.Height, record.Item.gridPos.X, record.Item.gridPos.Y);
			}
		}
	}

	public static TowerDefenseEnum.LEVEL_FINISH_METHOD GetGameMethod()
	{
		if (GodotObject.IsInstanceValid(Instance.currentControl))
		{
			if (Instance.currentControl.levelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig)
			{
				return towerDefenseLevelConfig.finishMethod;
			}
			return TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE;
		}
		return TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE;
	}

	public bool IsIZMMode()
	{
		if (IsLevelEditorStage())
		{
			if (TryGetEditorFinishMethod(out var finishMethod))
			{
				return finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM;
			}
			return false;
		}
		TowerDefenseEnum.LEVEL_FINISH_METHOD gameMethod = GetGameMethod();
		if (gameMethod != TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM)
		{
			return gameMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.QUIZ;
		}
		return true;
	}

	public bool IsIZM2Mode()
	{
		if (IsLevelEditorStage())
		{
			if (TryGetEditorFinishMethod(out var finishMethod))
			{
				return finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2;
			}
			return false;
		}
		return GetGameMethod() == TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2;
	}

	private static bool IsLevelEditorStage()
	{
		if (Global.Instance != null && Global.Instance.isEditor)
		{
			return SceneManager.CurrentScene == "LevelEditorStage";
		}
		return false;
	}

	private static bool TryGetEditorFinishMethod(out TowerDefenseEnum.LEVEL_FINISH_METHOD finishMethod)
	{
		finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE;
		if (!GodotObject.IsInstanceValid(LevelEditorInformationEditor.Instance))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(LevelEditorInformationEditor.Instance.levelConfig))
		{
			return false;
		}
		finishMethod = LevelEditorInformationEditor.Instance.levelConfig.finishMethod;
		return true;
	}

	public bool IsGameRunning()
	{
		if (GodotObject.IsInstanceValid(currentControl))
		{
			return currentControl.isGameRunning;
		}
		return false;
	}

	public static bool IsUnlimitedFire()
	{
		return CommandManager.Instance.debugUnlimitedFire;
	}

	public async void CoinCreate(Vector2 pos, int num, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 0.0, bool _collect = false)
	{
		Node2D characterNode = GetCharacterNode();
		while (num >= 1000)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase = Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_DIAMOND, pos, height, velocity, gravity);
			towerDefenseGroundItemBase.gridPos = new Vector2I(towerDefenseGroundItemBase.gridPos.X, 200);
			towerDefenseGroundItemBase.Reparent(characterNode, keepGlobalTransform: false);
			if (_collect)
			{
				towerDefenseGroundItemBase.Collection();
			}
			num -= 1000;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		while (num >= 50)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase2 = Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_GOLD, pos, height, velocity, gravity);
			towerDefenseGroundItemBase2.gridPos = new Vector2I(towerDefenseGroundItemBase2.gridPos.X, 200);
			towerDefenseGroundItemBase2.Reparent(characterNode, keepGlobalTransform: false);
			if (_collect)
			{
				towerDefenseGroundItemBase2.Collection();
			}
			num -= 50;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		while (num >= 10)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase3 = Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_SILVER, pos, height, velocity, gravity);
			towerDefenseGroundItemBase3.gridPos = new Vector2I(towerDefenseGroundItemBase3.gridPos.X, 200);
			towerDefenseGroundItemBase3.Reparent(characterNode, keepGlobalTransform: false);
			if (_collect)
			{
				towerDefenseGroundItemBase3.Collection();
			}
			num -= 10;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
	}

	public void AddSun(long num)
	{
		TowerDefenseBattleFeatureSun sunFeature = GetSunFeature();
		if (sunFeature != null)
		{
			if (Global.Instance.isMultiplayerMode && num > 0)
			{
				num = (long)Math.Max(1.0, Math.Floor((double)num / 2.0));
			}
			sunFeature.AddSun(num);
		}
	}

	public void UseSun(long num)
	{
		GetSunFeature()?.UseSun(num);
	}

	public void SetSun(long num)
	{
		GetSunFeature()?.SetSun(num);
	}

	public long GetSun()
	{
		return GetSunFeature()?.sunNum ?? (-1);
	}

	public int GetLevelChapterFinishNum(string levelListName, string chapterName)
	{
		Dictionary dictionary = (Dictionary)ResourceManager.LEVEL_RESOURCE.Get("data");
		if (!dictionary.ContainsKey(levelListName))
		{
			return -1;
		}
		foreach (Variant item in (Godot.Collections.Array)dictionary[levelListName].AsGodotDictionary()["Chapter"])
		{
			Dictionary dictionary2 = item.AsGodotDictionary();
			if (dictionary2["Name"].AsString() != chapterName)
			{
				continue;
			}
			int num = 0;
			foreach (Variant item2 in (Godot.Collections.Array)dictionary2["Level"])
			{
				Dictionary dictionary3 = item2.AsGodotDictionary();
				if (GameSaveManager.Instance.GetLevelValue(dictionary3["SaveKey"].AsString()).GetValueOrDefault("Key", Variant.From<Dictionary>(new Dictionary())).AsGodotDictionary()
					.GetValueOrDefault("Finish", 0)
					.AsInt32() > 0)
				{
					num++;
				}
			}
			return num;
		}
		return -1;
	}

	public void ExecuteLevelEvent(IEnumerable<TowerDefenseLevelEventBase> eventList)
	{
		if (eventList == null)
		{
			return;
		}
		int num = 0;
		foreach (TowerDefenseLevelEventBase @event in eventList)
		{
			try
			{
				if (GodotObject.IsInstanceValid(@event))
				{
					@event.Execute();
				}
			}
			catch (Exception value)
			{
				GD.PushError($"[LevelEvent] Event {num} failed: {value}");
			}
			num++;
		}
	}

	public TowerDefenseLevelEventBase GetLevelEvent(string eventName)
	{
		if (string.IsNullOrWhiteSpace(eventName))
		{
			return null;
		}
		TowerDefenseLevelEventBase towerDefenseLevelEventBase = TowerDefenseLevelEventRegistry.Create(eventName);
		if (GodotObject.IsInstanceValid(towerDefenseLevelEventBase))
		{
			return towerDefenseLevelEventBase;
		}
		if (!GodotObject.IsInstanceValid(ResourceManager.Instance) || !ResourceManager.Instance.TOWERDEFENSE_LEVEL_EVENT.TryGetValue(eventName, out var value))
		{
			return null;
		}
		if (value is GDScript gDScript)
		{
			return gDScript.New().AsGodotObject() as TowerDefenseLevelEventBase;
		}
		if (value is TowerDefenseLevelEventBase towerDefenseLevelEventBase2)
		{
			return towerDefenseLevelEventBase2.Duplicate(deep: true) as TowerDefenseLevelEventBase;
		}
		return null;
	}

	public TowerDefenseLevelBaseConfig SetNextLevel(string levelChoose, int chapterId, int levelId)
	{
		if (Global.Instance.enterLevelMode == "ModLevel")
		{
			return SelectNextModLevel();
		}
		if (string.IsNullOrEmpty(levelChoose) || chapterId < 0 || levelId < 0)
		{
			return null;
		}
		if (!TryGetNextLevelData(levelChoose, chapterId, levelId, out var nextLevel))
		{
			return null;
		}
		if (!CanOpenNextLevel(nextLevel))
		{
			return null;
		}
		TowerDefenseLevelConfig towerDefenseLevelConfig = LoadNextLevelConfig(nextLevel);
		if (towerDefenseLevelConfig == null)
		{
			return null;
		}
		Instance.currentLevelConfig = towerDefenseLevelConfig;
		SaveNextLevelProgress(chapterId, levelId);
		return Instance.currentLevelConfig as TowerDefenseLevelConfig;
	}

	private bool TryGetNextLevelData(string levelChoose, int chapterId, int levelId, out Dictionary nextLevel)
	{
		nextLevel = null;
		Dictionary dictionary = (Dictionary)ResourceManager.LEVEL_RESOURCE.Get("data");
		if (!dictionary.ContainsKey(levelChoose))
		{
			return false;
		}
		Godot.Collections.Array array = (Godot.Collections.Array)dictionary[levelChoose].AsGodotDictionary()["Chapter"];
		if (chapterId >= array.Count)
		{
			return false;
		}
		Godot.Collections.Array array2 = (Godot.Collections.Array)array[chapterId].AsGodotDictionary()["Level"];
		if (levelId + 1 >= array2.Count)
		{
			return false;
		}
		nextLevel = array2[levelId + 1].AsGodotDictionary();
		return true;
	}

	private TowerDefenseLevelConfig LoadNextLevelConfig(Dictionary nextLevel)
	{
		string text = GameSaveManager.Instance.GetKeyValue("CurrentDifficult").AsString();
		Dictionary dictionary = nextLevel["Level"].AsGodotDictionary();
		if (dictionary.ContainsKey(text) && dictionary[text].AsString() != "")
		{
			return GD.Load<TowerDefenseLevelConfig>(dictionary[text].AsString());
		}
		if (dictionary.ContainsKey("Normal") && dictionary["Normal"].AsString() != "")
		{
			return GD.Load<TowerDefenseLevelConfig>(dictionary["Normal"].AsString());
		}
		return null;
	}

	private bool CanOpenNextLevel(Dictionary nextLevel)
	{
		if (nextLevel["OpenKey"].AsString() == "Lock")
		{
			return false;
		}
		if (nextLevel["OpenKey"].AsString() != "" && GameSaveManager.Instance.GetLevelValue(nextLevel["OpenKey"].AsString()).GetValueOrDefault("Key", Variant.From<Dictionary>(new Dictionary())).AsGodotDictionary()
			.GetValueOrDefault("Finish", 0)
			.AsInt32() <= 0)
		{
			return false;
		}
		return true;
	}

	private void SaveNextLevelProgress(int chapterId, int levelId)
	{
		Global.Instance.currentLevelId = levelId + 1;
		GameSaveManager.Instance.SetKeyValue($"AdventureChapter{chapterId + 1}Index", levelId + 1);
		GameSaveManager.Instance.Save();
	}

	public TowerDefenseInGameLevelControl GetLevelControl()
	{
		if (!GodotObject.IsInstanceValid(currentControl))
		{
			return null;
		}
		return currentControl.levelControl;
	}

	public void TipsPlay(string text, double duration = 2.0)
	{
		if (Global.Instance.isMultiplayerMode && GodotObject.IsInstanceValid(currentControl))
		{
			currentControl.TipsPlay(text, duration);
		}
		else
		{
			GetLevelControl().TipsPlay(text, duration);
		}
	}

	public GeneralEnum.HOMEWORLD GetLevelHomeworld()
	{
		return GetLevelControl()?.config.homeWorld ?? GeneralEnum.HOMEWORLD.NOONE;
	}

	public TowerDefenseSunBase MagicSunCreate(Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.LAND, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		return CreateSunDrop(ObjectManagerConfig.OBJECT.SUN_MAGIC, EconomyAccountId.Local, SunDropOwnershipPolicy.LegacySharedReplica, pos, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
	}

	public TowerDefenseSunBase MagicSunCreate(EconomyAccountId accountId, Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.LAND, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		if (!CanCreateAccountOwnedSunDrop(accountId))
		{
			return null;
		}
		return CreateSunDrop(ObjectManagerConfig.OBJECT.SUN_MAGIC, accountId, SunDropOwnershipPolicy.AccountOwned, pos, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
	}

	public static double GetMapAttackDpsLifestealRatio()
	{
		TowerDefenseMapConfig activeMapConfig = GetActiveMapConfig();
		if (GodotObject.IsInstanceValid(activeMapConfig))
		{
			return activeMapConfig.GetAttackDpsLifestealRatio();
		}
		return 0.0;
	}

	public static double ApplyMapAttackDpsLifesteal(TowerDefenseCharacter attacker, double damage)
	{
		if (!GodotObject.IsInstanceValid(attacker?.instance) || !double.IsFinite(damage) || damage <= 0.0)
		{
			return 0.0;
		}
		double num = damage * GetMapAttackDpsLifestealRatio();
		if (!double.IsFinite(num) || num <= 0.0)
		{
			return 0.0;
		}
		attacker.instance.hitpoints += num;
		ShowHealthComponent showHealthComponent = attacker.showHealthComponent;
		if (showHealthComponent != null && !showHealthComponent.IsReleased)
		{
			attacker.showHealthComponent.MarkDirty();
		}
		return num;
	}

	public static void ApplyMapCharacterRules(TowerDefenseCharacter character)
	{
		TowerDefenseMapConfig activeMapConfig = GetActiveMapConfig();
		if (GodotObject.IsInstanceValid(activeMapConfig))
		{
			activeMapConfig.ApplyCharacterRules(character);
		}
	}

	public static double GetMapZombieColumnSpeedMultiplier(int gridX)
	{
		TowerDefenseMapConfig activeMapConfig = GetActiveMapConfig();
		if (GodotObject.IsInstanceValid(activeMapConfig))
		{
			return activeMapConfig.GetZombieColumnSpeedMultiplier(gridX);
		}
		return 1.0;
	}

	public TowerDefenseMap GetCurrentMap()
	{
		return GetMapCurrentMap();
	}

	public TowerDefenseMapConfig GetMapConfig(string mapName)
	{
		TowerDefenseMapConfig result = null;
		if (!string.IsNullOrEmpty(mapName) && ResourceManager.Instance.MAPS.ContainsKey(mapName))
		{
			result = (TowerDefenseMapConfig)ResourceManager.Instance.MAPS[mapName];
		}
		return result;
	}

	public void MapChange(string map, double duration = 0.0, double delay = 0.0)
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			TowerDefenseMapConfig mapConfig = GetMapConfig(map);
			if (GodotObject.IsInstanceValid(mapConfig))
			{
				mapFeature.MapChange(mapConfig, duration, delay);
			}
		}
	}

	public static bool GetMapIsNight()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.config))
		{
			return mapFeature.config.isNight;
		}
		return false;
	}

	public void MapDayNightSwitch(double duration = 2.0, double _switchTimer = 100.0, double returnDuration = 2.0)
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			mapFeature.MapDayNightSwitch(duration, _switchTimer, returnDuration);
		}
	}

	public Vector2I GetMapGridNum()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.config))
		{
			return mapFeature.config.gridNum;
		}
		return new Vector2I(25, 25);
	}

	public Vector2 GetMapGridSize()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.config) && GodotObject.IsInstanceValid(mapFeature.mapControl))
		{
			return mapFeature.config.gridSize * mapFeature.mapControl.GlobalScale;
		}
		return new Vector2(80f, 98f);
	}

	public Vector2 GetMapGridBeginPos()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.config) && GodotObject.IsInstanceValid(mapFeature.mapControl))
		{
			return mapFeature.config.gridBeginPos * mapFeature.mapControl.GlobalScale + mapFeature.mapControl.GlobalPosition;
		}
		return Vector2.Zero;
	}

	public double GetMapPlantOffset()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.config))
		{
			return mapFeature.config.plantOffset;
		}
		return 50.0;
	}

	public Vector2I GetMapGridPos(Vector2 pos)
	{
		Vector2 vector = ((pos - gridBeginPos) / gridSize).Floor();
		return new Vector2I((int)vector.X, (int)vector.Y) + Vector2I.One;
	}

	public Vector2 GetMapCellPos(Vector2I gridPos)
	{
		gridPos -= Vector2I.One;
		return gridBeginPos + new Vector2(gridPos.X, gridPos.Y) * gridSize;
	}

	public Vector2 GetMapCellPosCenter(Vector2I gridPos)
	{
		gridPos -= Vector2I.One;
		return gridBeginPos + new Vector2(gridPos.X, gridPos.Y) * gridSize + GetMapGridSize() / 2f;
	}

	public bool CheckMapGridPosIn(Vector2I gridPos)
	{
		if (gridPos.X < 1 || gridPos.Y < 1 || gridPos.X > gridNum.X || gridPos.Y > gridNum.Y)
		{
			return false;
		}
		return true;
	}

	public static TowerDefenseCellInstance GetMapCell(Vector2I gridPos)
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (!GodotObject.IsInstanceValid(mapFeature))
		{
			return null;
		}
		return mapFeature.GetPlantGridCell(gridPos);
	}

	public void SetMapGridType(TowerDefenseCellConfig cellConfig)
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			mapFeature.SetGridType(cellConfig);
		}
	}

	public void SetMapLineUse(int line, bool use)
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			mapFeature.SetLineUse(line, use);
		}
	}

	public bool GetMapLineUse(int line)
	{
		Array<bool> mapLineUseArr = GetMapLineUseArr();
		if (line > 0 && line < mapLineUseArr.Count)
		{
			return mapLineUseArr[line];
		}
		return false;
	}

	public double GetMapGroundLeft()
	{
		return gridBeginPos.X;
	}

	public double GetMapGroundRight()
	{
		return gridBeginPos.X + (float)gridNum.X * gridSize.X;
	}

	public double GetMapGroundUp()
	{
		return gridBeginPos.Y;
	}

	public double GetMapGroundDown()
	{
		return gridBeginPos.Y + (float)gridNum.Y * gridSize.Y;
	}

	public Vector2I GetMapGridPosFromMouse(Vector2 pos)
	{
		int num = (int)Mathf.Floor((pos.X - gridBeginPos.X) / gridSize.X);
		double num2 = (pos.X - (gridBeginPos.X + (float)num * gridSize.X)) / gridSize.X;
		Array<Godot.Collections.Array> mapPlantGrid = GetMapPlantGrid();
		if (mapPlantGrid.Count == 0)
		{
			return new Vector2I(num, -1);
		}
		for (int i = 0; i < gridNum.Y; i++)
		{
			Vector2I result = new Vector2I(num, i) + Vector2I.One;
			if (result.X < 1 || result.Y < 1 || result.X > gridNum.X || result.Y > gridNum.Y || mapPlantGrid.Count <= result.X)
			{
				continue;
			}
			Variant variant = mapPlantGrid[result.X][result.Y];
			if (variant.VariantType == Variant.Type.Nil)
			{
				continue;
			}
			TowerDefenseCellInstance towerDefenseCellInstance = variant.AsGodotObject() as TowerDefenseCellInstance;
			if (!GodotObject.IsInstanceValid(towerDefenseCellInstance))
			{
				continue;
			}
			int num3 = 0;
			CurveTexture groundHeightCurve = towerDefenseCellInstance.groundHeightCurve;
			if (GodotObject.IsInstanceValid(groundHeightCurve))
			{
				Curve curve = groundHeightCurve.Curve;
				if (curve != null)
				{
					num3 = (int)curve.Sample((float)num2);
				}
			}
			double num4 = gridBeginPos.Y + (float)i * gridSize.Y;
			if ((double)pos.Y > num4 - (double)num3 && (double)pos.Y < num4 - (double)num3 + (double)gridSize.Y)
			{
				return result;
			}
		}
		return new Vector2I(num, -1);
	}

	public TowerDefenseMapControl GetMapControl()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			return mapFeature.mapControl;
		}
		return null;
	}

	public void ReleaseBattleReferences(TowerDefenseControlNew control, bool preserveLevelConfig)
	{
		if (currentControl == control)
		{
			currentControl = null;
			if (!preserveLevelConfig)
			{
				currentLevelConfig = null;
			}
			_groupCache.Clear();
			_groupCacheFrame = -1L;
		}
		if (_cachedMapFeatureControl == control)
		{
			_cachedMapFeature = null;
			_cachedMapFeatureControl = null;
		}
	}

	public static TowerDefenseBattleFeatureMap GetMapFeature()
	{
		if (Instance == null)
		{
			return null;
		}
		TowerDefenseControlNew towerDefenseControlNew = Instance.currentControl;
		if (GodotObject.IsInstanceValid(towerDefenseControlNew))
		{
			if (_cachedMapFeature != null && _cachedMapFeatureControl == towerDefenseControlNew)
			{
				return _cachedMapFeature;
			}
			_cachedMapFeatureControl = towerDefenseControlNew;
			_cachedMapFeature = towerDefenseControlNew.GetFeature(FeatureName_Map) as TowerDefenseBattleFeatureMap;
			return _cachedMapFeature;
		}
		_cachedMapFeature = null;
		_cachedMapFeatureControl = null;
		if (GodotObject.IsInstanceValid(LevelEditorMapEditor.instance) && GodotObject.IsInstanceValid(LevelEditorMapEditor.instance.mapFeature))
		{
			return LevelEditorMapEditor.instance.mapFeature;
		}
		return null;
	}

	public static double ApplyMapPacketCooldownRules(TowerDefenseEnum.PACKET_TYPE packetType, double multiplier)
	{
		TowerDefenseMapConfig activeMapConfig = GetActiveMapConfig();
		if (GodotObject.IsInstanceValid(activeMapConfig))
		{
			return activeMapConfig.ApplyPacketCooldownRules(packetType, multiplier);
		}
		return multiplier;
	}

	public static bool MapIgnoresDynamicPacketCostGrowth(TowerDefenseEnum.PACKET_TYPE packetType)
	{
		TowerDefenseMapConfig activeMapConfig = GetActiveMapConfig();
		if (GodotObject.IsInstanceValid(activeMapConfig))
		{
			return activeMapConfig.IgnoresDynamicPacketCostGrowth(packetType);
		}
		return false;
	}

	public static Vector2 GetMapCellPlantPos(Vector2I gridPos)
	{
		if (Instance == null)
		{
			return Vector2.Zero;
		}
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (mapFeature == null || !GodotObject.IsInstanceValid(mapFeature.mapControl))
		{
			return Vector2.Zero;
		}
		gridPos -= Vector2I.One;
		return Instance.gridBeginPos + new Vector2(gridPos.X, gridPos.Y) * Instance.gridSize + new Vector2(Instance.gridSize.X / 2f, (float)Instance.GetMapPlantOffset() * mapFeature.mapControl.GlobalScale.Y);
	}

	public static double GetMapLineY(int line)
	{
		if (Instance == null)
		{
			return 0.0;
		}
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (mapFeature == null || !GodotObject.IsInstanceValid(mapFeature.mapControl))
		{
			return 0.0;
		}
		line--;
		return Instance.gridBeginPos.Y + (float)line * Instance.gridSize.Y + (float)Instance.GetMapPlantOffset() * mapFeature.mapControl.GlobalScale.Y;
	}

	public TowerDefenseMapConfig GetCurrentMapConfig()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			return mapFeature.config;
		}
		return null;
	}

	public TowerDefenseMap GetMapCurrentMap()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			return mapFeature.currentMap;
		}
		return null;
	}

	public PacketPickControl GetPacketPickControl()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			return mapFeature.packetPickControl;
		}
		return null;
	}

	public static bool MapLineHasType(int line, TowerDefenseEnum.PLANTGRIDTYPE type)
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			return mapFeature.LineHasType(line, type);
		}
		return false;
	}

	public Array<Godot.Collections.Array> GetMapPlantGrid()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			return mapFeature.plantGrid;
		}
		return new Array<Godot.Collections.Array>();
	}

	public Array<bool> GetMapLineUseArr()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			return mapFeature.lineUse;
		}
		return new Array<bool>();
	}

	public Godot.Collections.Array GetMapIceCapList()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			return mapFeature.iceCapList;
		}
		return new Godot.Collections.Array();
	}

	public Rect2 GetGroundRect()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			return mapFeature.groundRect;
		}
		return default;
	}

	public void SetIceCapPos(int line, Vector2 pos)
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			mapFeature.SetIceCapPos(line, pos);
		}
	}

	public bool TryGetIceCapFrontX(int line, out float frontX)
	{
		frontX = 0f;
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			return mapFeature.TryGetIceCapFrontX(line, out frontX);
		}
		return false;
	}

	public static bool MapHasSpecialRule(StringName ruleId)
	{
		TowerDefenseMapConfig activeMapConfig = GetActiveMapConfig();
		if (GodotObject.IsInstanceValid(activeMapConfig))
		{
			return activeMapConfig.HasSpecialRule(ruleId);
		}
		return false;
	}

	public static bool MapSpecialRulesPreventSleep()
	{
		TowerDefenseMapConfig activeMapConfig = GetActiveMapConfig();
		if (GodotObject.IsInstanceValid(activeMapConfig))
		{
			return activeMapConfig.SpecialRulesPreventSleep();
		}
		return false;
	}

	private static TowerDefenseMapConfig GetActiveMapConfig()
	{
		TowerDefenseBattleFeatureMap mapFeature = GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.config))
		{
			return mapFeature.config;
		}
		return null;
	}

	private TowerDefenseLevelBaseConfig SelectNextModLevel()
	{
		XWModLevelIdentity identity = XWModLevelSession.Current;
		if (identity == null)
		{
			return null;
		}
		XWModContentCatalog.Catalog catalog = XWModContentCatalog.GetCatalogs().FirstOrDefault((XWModContentCatalog.Catalog item) => string.Equals(item.OwnerModId, identity.OwnerModId, StringComparison.OrdinalIgnoreCase) && item.Key == identity.CatalogKey);
		if (catalog == null)
		{
			return null;
		}
		bool flag = false;
		Godot.Collections.Array array = catalog.Data["Chapter"].AsGodotArray();
		for (int num = 0; num < array.Count; num++)
		{
			Dictionary dictionary = array[num].AsGodotDictionary();
			Godot.Collections.Array array2 = dictionary["Level"].AsGodotArray();
			for (int num2 = 0; num2 < array2.Count; num2++)
			{
				Dictionary dictionary2 = array2[num2].AsGodotDictionary();
				string text = dictionary2["SaveKey"].AsString();
				if (!flag)
				{
					flag = text == identity.LevelSaveKey;
					continue;
				}
				if (dictionary.GetValueOrDefault("Lock", false).AsBool() || !ModOpen(dictionary.GetValueOrDefault("OpenKey", "").AsString(), identity) || !ModOpen(dictionary2.GetValueOrDefault("OpenKey", "").AsString(), identity))
				{
					return null;
				}
				Dictionary dictionary3 = dictionary2["Level"].AsGodotDictionary();
				XWModLevelIdentity identity2 = identity with
				{
					LevelSaveKey = text,
					Difficulty = ((dictionary3.GetValueOrDefault(identity.Difficulty, "").AsString().Length > 0) ? identity.Difficulty : "Normal")
				};
				if (!XWModContentCatalog.TryResolve(identity2, out var config, out var _))
				{
					return null;
				}
				currentLevelConfig = config;
				XWModLevelSession.Select(identity2);
				Global.Instance.currentChapterId = num;
				Global.Instance.currentLevelId = num2;
				return config;
			}
		}
		return null;
	}

	private static bool ModOpen(string key, XWModLevelIdentity identity)
	{
		if (key.Length != 0)
		{
			if (key != "Lock")
			{
				return XWModPlayerProgressService.FinishCount(identity with
				{
					LevelSaveKey = key
				}) > 0;
			}
			return false;
		}
		return true;
	}

	public static MowerConfig GetMowerConfig(string mowerName)
	{
		return (MowerConfig)ResourceManager.Instance.MOWERS[mowerName];
	}

	public static Godot.Collections.Array GetMowerList()
	{
		List<Variant> list = new List<Variant>();
		foreach (string key in ResourceManager.Instance.MOWERS.Keys)
		{
			list.Add(key);
		}
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (Variant item in list)
		{
			array.Add(item);
		}
		return array;
	}

	public int GetMowerNum()
	{
		return GetMower().Count;
	}

	public Godot.Collections.Array GetMower()
	{
		return _GetNodesInGroupCached("Mower");
	}

	public TowerDefenseMower CreateMower(int line)
	{
		return GetMowerFeature()?.CreateMower(line);
	}

	public TowerDefenseBattleFeatureMower GetMowerFeature()
	{
		TowerDefenseControlNew towerDefenseControlNew = currentControl;
		if (towerDefenseControlNew != null)
		{
			return towerDefenseControlNew.GetFeature(FeatureName_Mower) as TowerDefenseBattleFeatureMower;
		}
		return null;
	}

	public TowerDefenseMowerManager GetMowerManager()
	{
		return GetMowerFeature()?.mowerManager;
	}

	public static NpcTalkConfig GetNpcTalk(string npcTalkName)
	{
		if (string.IsNullOrEmpty(npcTalkName))
		{
			return null;
		}
		if (ResourceManager.Instance.TALKS.TryGetValue(npcTalkName, out var value) && value is NpcTalkConfig result)
		{
			return result;
		}
		GD.PushWarning("[NpcTalk] Talk '" + npcTalkName + "' is unavailable.");
		return null;
	}

	public TowerDefenseInGamePacketBank GetPacketBank()
	{
		return GetPacketBankFeature()?.packetBank;
	}

	public TowerDefenseBattleFeaturePacketBank GetPacketBankFeature()
	{
		TowerDefenseControlNew towerDefenseControlNew = currentControl;
		if (towerDefenseControlNew != null)
		{
			return towerDefenseControlNew.GetFeature(FeatureName_PacketBank) as TowerDefenseBattleFeaturePacketBank;
		}
		return null;
	}

	public static TowerDefensePacketBankData GetPacketBankData(string packetBank)
	{
		ZLoggerErrorInterpolatedStringHandler message;
		bool enabled;
		if (ResourceManager.Instance == null)
		{
			ILogger logger = _logger;
			ILogger logger2 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(51, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("GetPacketBankData: ResourceManager.Instance is null");
			}
			logger2.ZLogError(ref message);
			return null;
		}
		ResourceManager.Instance.RequireFullGameplayResourcesReady("GetPacketBankData(" + packetBank + ")");
		if (string.IsNullOrEmpty(packetBank) || !ResourceManager.Instance.TOWERDEFENSE_PACKETBANKS.ContainsKey(packetBank))
		{
			ILogger logger = _logger;
			ILogger logger3 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(66, 2, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("GetPacketBankData: '");
				message.AppendFormatted(packetBank, 0, null, "packetBank");
				message.AppendLiteral("' not found in TOWERDEFENSE_PACKETBANKS, keys=");
				message.AppendFormatted(string.Join(",", ResourceManager.Instance.TOWERDEFENSE_PACKETBANKS.Keys), 0, null, "string.Join(\",\", ResourceManager.Instance.TOWERDEFENSE_PACKETBANKS.Keys)");
			}
			logger3.ZLogError(ref message);
			return null;
		}
		TowerDefensePacketBankData towerDefensePacketBankData = ResourceManager.Instance.TOWERDEFENSE_PACKETBANKS[packetBank];
		if (towerDefensePacketBankData == null || !GodotObject.IsInstanceValid(towerDefensePacketBankData))
		{
			ILogger logger = _logger;
			ILogger logger4 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(43, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("GetPacketBankData: '");
				message.AppendFormatted(packetBank, 0, null, "packetBank");
				message.AppendLiteral("' value is null/invalid");
			}
			logger4.ZLogError(ref message);
			return null;
		}
		return towerDefensePacketBankData;
	}

	public static TowerDefensePacketConfig GetPacketConfigReadOnlyByCharacterName(string characterName)
	{
		if (ResourceManager.Instance == null || characterName == "")
		{
			return null;
		}
		ResourceManager.Instance.RequireFullGameplayResourcesReady("GetPacketConfigReadOnlyByCharacterName");
		foreach (string packetName in ResourceManager.Instance.GetPacketNames())
		{
			TowerDefensePacketConfig packetConfigReadOnly = GetPacketConfigReadOnly(packetName);
			if (GodotObject.IsInstanceValid(packetConfigReadOnly) && GodotObject.IsInstanceValid(packetConfigReadOnly.characterConfig) && packetConfigReadOnly.characterConfig.name == characterName)
			{
				return packetConfigReadOnly;
			}
		}
		return null;
	}

	public static Array<TowerDefensePacketConfig> GetPacketConfigCostLower(int cost, TowerDefenseEnum.PACKET_TYPE type)
	{
		return CollectPacketConfigsByCost((TowerDefensePacketConfig packetConfig) => packetConfig.GetCost() <= cost && (packetConfig._GetType() == type || type == TowerDefenseEnum.PACKET_TYPE.NOONE));
	}

	public static Array<TowerDefensePacketConfig> GetPacketConfigCostLowerWithTypeList(int cost, Array<TowerDefenseEnum.PACKET_TYPE> typeList)
	{
		return CollectPacketConfigsByCost((TowerDefensePacketConfig packetConfig) => packetConfig.GetCost() <= cost && typeList.Contains(packetConfig._GetType()));
	}

	private static Array<TowerDefensePacketConfig> CollectPacketConfigsByCost(Func<TowerDefensePacketConfig, bool> predicate)
	{
		List<TowerDefensePacketConfig> list = new List<TowerDefensePacketConfig>();
		ResourceManager.Instance.RequireFullGameplayResourcesReady("CollectPacketConfigsByCost");
		foreach (KeyValuePair<string, Resource> tOWERDEFENSE_PACKET in ResourceManager.Instance.TOWERDEFENSE_PACKETS)
		{
			if (tOWERDEFENSE_PACKET.Value is TowerDefensePacketConfig towerDefensePacketConfig)
			{
				string key = tOWERDEFENSE_PACKET.Key;
				_packetConfigRefCache.TryAdd(key, towerDefensePacketConfig);
				if ((towerDefensePacketConfig.characterConfig is TowerDefensePlantConfig || towerDefensePacketConfig.characterConfig is TowerDefenseZombieConfig) && predicate(towerDefensePacketConfig))
				{
					list.Add((TowerDefensePacketConfig)towerDefensePacketConfig.Duplicate(deep: true));
				}
			}
		}
		Array<TowerDefensePacketConfig> array = new Array<TowerDefensePacketConfig>();
		foreach (TowerDefensePacketConfig item in list)
		{
			array.Add(item);
		}
		return array;
	}

	public static Array<TowerDefensePacketConfig> GetPacketConfigCostUpper(int cost, TowerDefenseEnum.PACKET_TYPE type)
	{
		return CollectPacketConfigsByCost((TowerDefensePacketConfig packetConfig) => packetConfig.GetCost() >= cost && packetConfig._GetType() == type);
	}

	public static Array<TowerDefensePacketConfig> GetPacketConfigCostUpperWithTypeList(int cost, Array<TowerDefenseEnum.PACKET_TYPE> typeList)
	{
		return CollectPacketConfigsByCost((TowerDefensePacketConfig packetConfig) => packetConfig.GetCost() >= cost && typeList.Contains(packetConfig._GetType()));
	}

	public static TowerDefensePacketConfig GetPacketConfig(string packetName)
	{
		ZLoggerErrorInterpolatedStringHandler message;
		bool enabled;
		if (ResourceManager.Instance == null)
		{
			ILogger logger = _logger;
			ILogger logger2 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(49, 0, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("GetPacketConfig: ResourceManager.Instance is null");
			}
			logger2.ZLogError(ref message);
			return null;
		}
		Variant packet = ResourceManager.Instance.GetPacket(packetName);
		if (packet.VariantType == Variant.Type.Nil)
		{
			ILogger logger = _logger;
			ILogger logger3 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(50, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("GetPacketConfig: packet '");
				message.AppendFormatted(packetName, 0, null, "packetName");
				message.AppendLiteral("' not found (Nil variant)");
			}
			logger3.ZLogError(ref message);
			return null;
		}
		GodotObject godotObject = packet.AsGodotObject();
		if (godotObject == null || !GodotObject.IsInstanceValid(godotObject))
		{
			ILogger logger = _logger;
			ILogger logger4 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(52, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("GetPacketConfig: packet '");
				message.AppendFormatted(packetName, 0, null, "packetName");
				message.AppendLiteral("' object is null or invalid");
			}
			logger4.ZLogError(ref message);
			return null;
		}
		if (!(godotObject is Resource resource))
		{
			ILogger logger = _logger;
			ILogger logger5 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(51, 2, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("GetPacketConfig: packet '");
				message.AppendFormatted(packetName, 0, null, "packetName");
				message.AppendLiteral("' is not a Resource, type=");
				message.AppendFormatted(godotObject.GetType().Name, 0, null, "obj.GetType().Name");
			}
			logger5.ZLogError(ref message);
			return null;
		}
		_packetConfigRefCache.TryAdd(packetName, resource as TowerDefensePacketConfig);
		Resource resource2 = resource.Duplicate(deep: true);
		if (resource2 == null)
		{
			ILogger logger = _logger;
			ILogger logger6 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(50, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("GetPacketConfig: packet '");
				message.AppendFormatted(packetName, 0, null, "packetName");
				message.AppendLiteral("' Duplicate returned null");
			}
			logger6.ZLogError(ref message);
			return null;
		}
		return resource2 as TowerDefensePacketConfig;
	}

	public static TowerDefensePacketConfig GetPacketConfigReadOnly(string packetName)
	{
		Variant packet = ResourceManager.Instance.GetPacket(packetName);
		if (packet.VariantType == Variant.Type.Nil)
		{
			return null;
		}
		return (TowerDefensePacketConfig)packet.AsGodotObject();
	}

	public static TowerDefenseInGamePacketShow CreatePacketShow(string packetName = "")
	{
		TowerDefensePacketConfig config = null;
		if (packetName != "")
		{
			config = GetPacketConfig(packetName);
		}
		return CreatePacketShowWithConfig(config);
	}

	public static TowerDefenseInGamePacketShow CreatePacketShowWithConfig(TowerDefensePacketConfig config = null)
	{
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TOWER_DEFENSE_IN_GAME_PACKET_SHOW.Instantiate(PackedScene.GenEditState.Disabled) as TowerDefenseInGamePacketShow;
		if (GodotObject.IsInstanceValid(config))
		{
			towerDefenseInGamePacketShow.Init(config);
		}
		return towerDefenseInGamePacketShow;
	}

	public TowerDefenseInGamePacketShow SpawnPacket(TowerDefensePacketConfig packetConfig, Vector2 pos, double aliveTime, bool isFall, bool useCost = false, bool useRandf = true, Vector2? velocityOverride = null)
	{
		return SpawnPacketCore(default, packetConfig, pos, aliveTime, isFall, useCost, useRandf, velocityOverride);
	}

	public TowerDefenseInGamePacketShow SpawnPacket(EconomyAccountId accountId, TowerDefensePacketConfig packetConfig, Vector2 pos, double aliveTime, bool isFall, bool useCost = false, bool useRandf = true, Vector2? velocityOverride = null)
	{
		if (!accountId.IsValid)
		{
			return null;
		}
		return SpawnPacketCore(accountId, packetConfig, pos, aliveTime, isFall, useCost, useRandf, velocityOverride);
	}

	private TowerDefenseInGamePacketShow SpawnPacketCore(EconomyAccountId accountId, TowerDefensePacketConfig packetConfig, Vector2 pos, double aliveTime, bool isFall, bool useCost, bool useRandf, Vector2? velocityOverride)
	{
		if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			return null;
		}
		double height = GD.RandRange(Instance.GetMapGridBeginPos().Y + 200f, Instance.GetMapGroundDown() - (double)Instance.GetMapGridBeginPos().Y);
		Node2D characterNode = GetCharacterNode();
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = CreatePacketShow();
		if (accountId.IsValid && !towerDefenseInGamePacketShow.TryBindSunAccount(accountId))
		{
			towerDefenseInGamePacketShow.QueueFree();
			return null;
		}
		towerDefenseInGamePacketShow.GlobalPosition = pos;
		characterNode.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
		towerDefenseInGamePacketShow.Init(packetConfig);
		towerDefenseInGamePacketShow.onlyDraw = false;
		towerDefenseInGamePacketShow.showCost = useCost;
		towerDefenseInGamePacketShow.useCost = useCost;
		towerDefenseInGamePacketShow.plantOnce = true;
		towerDefenseInGamePacketShow.StartInit();
		towerDefenseInGamePacketShow.alive = true;
		towerDefenseInGamePacketShow.aliveTime = aliveTime;
		towerDefenseInGamePacketShow.ZIndex = 1024;
		if (useCost)
		{
			towerDefenseInGamePacketShow.start = true;
		}
		InitializeSpawnedPacketMovement(towerDefenseInGamePacketShow, isFall, height, useRandf, velocityOverride, out var velocityX, out var velocityY);
		BindSpawnedPacketPicker(towerDefenseInGamePacketShow);
		PublishSpawnedPacket(towerDefenseInGamePacketShow, packetConfig, pos, aliveTime, isFall, useCost, velocityX, velocityY, height);
		return towerDefenseInGamePacketShow;
	}

	private void InitializeSpawnedPacketMovement(TowerDefenseInGamePacketShow packet, bool isFall, double height, bool useRandf, Vector2? velocityOverride, out double velocityX, out double velocityY)
	{
		velocityX = 0.0;
		velocityY = -300.0;
		if (isFall)
		{
			packet.CreateTween().TweenProperty(packet, "global_position:y", height, (height - (double)GlobalPosition.Y) / 25.0);
			return;
		}
		packet.height = 1.0;
		packet.moveComponent.gravity = 980.0;
		if (velocityOverride.HasValue)
		{
			velocityX = velocityOverride.Value.X;
			velocityY = velocityOverride.Value.Y;
		}
		else if (useRandf)
		{
			if ((double)GD.Randf() > 0.5)
			{
				velocityX = GD.RandRange(-80.0, -50.0);
			}
			else
			{
				velocityX = GD.RandRange(30.0, 80.0);
			}
		}
		packet.moveComponent.velocity = new Vector2((float)velocityX, (float)velocityY);
	}

	private void BindSpawnedPacketPicker(TowerDefenseInGamePacketShow packet)
	{
		PacketPickControl packetPickControl = Instance.GetPacketPickControl();
		if (packetPickControl != null)
		{
			packet.OnPressed += packetPickControl.PickPacket;
		}
	}

	private void PublishSpawnedPacket(TowerDefenseInGamePacketShow packet, TowerDefensePacketConfig packetConfig, Vector2 pos, double aliveTime, bool isFall, bool useCost, double velocityX, double velocityY, double height)
	{
		if (Global.Instance.isMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			TowerDefenseControlNew towerDefenseControlNew = Instance.currentControl;
			if (GodotObject.IsInstanceValid(towerDefenseControlNew))
			{
				int nextPacketSyncId = towerDefenseControlNew.GetNextPacketSyncId();
				packet.SetMeta("packet_sync_id", nextPacketSyncId);
				towerDefenseControlNew.RegisterSyncPacket(nextPacketSyncId, packet);
				MultiPlayerManager.Instance.SendPacketSpawn(nextPacketSyncId, packetConfig.saveKey, pos.X, pos.Y, aliveTime, isFall, useCost, (float)velocityX, (float)velocityY, packet.ZIndex, (float)height, packetConfig, packet.SunAccountId);
			}
		}
	}

	public TowerDefenseBattleFeaturePortal GetPortalFeature()
	{
		if (GodotObject.IsInstanceValid(currentControl) && currentControl.GetFeature(FeatureName_Portal) is TowerDefenseBattleFeaturePortal result)
		{
			return result;
		}
		return null;
	}

	public void PortalCreate(string shape, Vector4I posRange, double changeTime = 0.0)
	{
		GetPortalFeature()?.PortalCreate(shape, posRange, changeTime);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ProtalCreate(string shape, Vector4I posRange, double changeTime = 0.0)
	{
		PortalCreate(shape, posRange, changeTime);
	}

	public void PortalChangePos()
	{
		GetPortalFeature()?.PortalChangePos();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ProtalChangePos()
	{
		PortalChangePos();
	}

	public static TowerDefenseProjectileConfig GetProjectileConfig(string projectileName)
	{
		if (ResourceManager.Instance == null)
		{
			ILogger logger = _logger;
			ZLoggerErrorInterpolatedStringHandler message = new ZLoggerErrorInterpolatedStringHandler(53, 0, logger, out var enabled);
			if (enabled)
			{
				message.AppendLiteral("GetProjectileConfig: ResourceManager.Instance is null");
			}
			logger.ZLogError(ref message);
			return null;
		}
		if (string.IsNullOrEmpty(projectileName) || !ResourceManager.Instance.PROJECTILE_CONFIG.ContainsKey(projectileName))
		{
			return null;
		}
		Resource resource = ResourceManager.Instance.PROJECTILE_CONFIG[projectileName];
		if (resource == null || !GodotObject.IsInstanceValid(resource))
		{
			return null;
		}
		if (!_projectileConfigRefCache.ContainsKey(projectileName))
		{
			_projectileConfigRefCache[projectileName] = resource as TowerDefenseProjectileConfig;
		}
		return resource as TowerDefenseProjectileConfig;
	}

	public TowerDefenseSunBase QXSunCreate(Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.LAND, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		return CreateSunDrop(ObjectManagerConfig.OBJECT.SUN_QX, EconomyAccountId.Local, SunDropOwnershipPolicy.LegacySharedReplica, pos, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
	}

	public TowerDefenseSunBase QXSunCreate(EconomyAccountId accountId, Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.LAND, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		if (!CanCreateAccountOwnedSunDrop(accountId))
		{
			return null;
		}
		return CreateSunDrop(ObjectManagerConfig.OBJECT.SUN_QX, accountId, SunDropOwnershipPolicy.AccountOwned, pos, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
	}

	public Array<TowerDefenseCharacter> _GetCleanCharacters()
	{
		return characterRegistry.GetCleanCharacters();
	}

	public Godot.Collections.Array _GetNodesInGroupCached(string groupName)
	{
		long physicsFrames = (long)Engine.GetPhysicsFrames();
		if (physicsFrames != _groupCacheFrame)
		{
			_groupCacheFrame = physicsFrames;
			_groupCache.Clear();
		}
		if (!_groupCache.TryGetValue(groupName, out var value))
		{
			value = new Godot.Collections.Array();
			foreach (Node item in GetTree().GetNodesInGroup(groupName))
			{
				value.Add(item);
			}
			_groupCache[groupName] = value;
		}
		return value;
	}

	public TowerDefenseBattleFeatureSeedBank GetSeedBankFeature()
	{
		TowerDefenseControlNew towerDefenseControlNew = currentControl;
		if (towerDefenseControlNew != null)
		{
			return towerDefenseControlNew.GetFeature(FeatureName_SeedBank) as TowerDefenseBattleFeatureSeedBank;
		}
		return null;
	}

	public TowerDefenseEnum.LEVEL_SEEDBANK_METHOD GetCurrentPacketBankMethod()
	{
		TowerDefenseBattleFeatureSeedBank seedBankFeature = GetSeedBankFeature();
		if (GodotObject.IsInstanceValid(seedBankFeature?.config))
		{
			return seedBankFeature.config.method;
		}
		if (currentLevelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig)
		{
			return towerDefenseLevelConfig.packetBankMethod;
		}
		return TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE;
	}

	public TowerDefenseInGameSeedBank GetSeedBank()
	{
		return GetSeedBankFeature()?.seedBank;
	}

	public static List<TowerDefensePacketChangeCost> GetChangeCostList()
	{
		if (Instance == null)
		{
			return new List<TowerDefensePacketChangeCost>();
		}
		TowerDefenseControlNew towerDefenseControlNew = Instance.currentControl;
		if (towerDefenseControlNew != null)
		{
			return towerDefenseControlNew.changeCostList;
		}
		return new List<TowerDefensePacketChangeCost>();
	}

	public bool ChangeCostAdd(TowerDefensePacketChangeCost changeCost)
	{
		return currentControl?.ChangeCostAdd(changeCost) ?? false;
	}

	public bool ChangeCostRemove(TowerDefensePacketChangeCost changeCost)
	{
		return currentControl?.ChangeCostRemove(changeCost) ?? false;
	}

	public int GetPacketSlotNum()
	{
		int num = 7;
		for (int i = 8; i < 17; i++)
		{
			if (GameSaveManager.Instance.GetFeatureValue($"PacketSlot{i}") <= 0)
			{
				break;
			}
			num++;
		}
		seedbankPacketMax = num;
		return num;
	}

	public void AddPacket(string packetName, TowerDefensePacketOverride override_ = null)
	{
		TowerDefenseInGameSeedBank seedBank = GetSeedBank();
		if (!GodotObject.IsInstanceValid(seedBank) || string.IsNullOrWhiteSpace(packetName))
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = GetPacketConfig(packetName);
		if (packetConfig != null)
		{
			if (GodotObject.IsInstanceValid(override_))
			{
				packetConfig._override = override_;
			}
			seedBank.AddPacket(packetConfig, isStart: true);
		}
	}

	public Array<TowerDefenseInGamePacketShow> GetSeedBankList()
	{
		TowerDefenseInGameSeedBank seedBank = GetSeedBank();
		if (GodotObject.IsInstanceValid(seedBank))
		{
			return seedBank.packetList;
		}
		return new Array<TowerDefenseInGamePacketShow>();
	}

	public static ShovelConfig GetShovel(string shovelName)
	{
		return (ShovelConfig)ResourceManager.Instance.SHOVELS[shovelName];
	}

	public static Godot.Collections.Array GetShovelList()
	{
		List<Variant> list = new List<Variant>();
		foreach (string key in ResourceManager.Instance.SHOVELS.Keys)
		{
			list.Add(key);
		}
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (Variant item in list)
		{
			array.Add(item);
		}
		return array;
	}

	public async void YBCreate(Vector2 pos, int num, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 0.0, bool _collect = false)
	{
		Node2D characterNode = GetCharacterNode();
		while (num >= 1000)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase = Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_YB2, pos, height, velocity, gravity);
			towerDefenseGroundItemBase.gridPos = new Vector2I(towerDefenseGroundItemBase.gridPos.X, 200);
			towerDefenseGroundItemBase.Reparent(characterNode, keepGlobalTransform: false);
			if (_collect)
			{
				towerDefenseGroundItemBase.Collection();
			}
			num -= 1000;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		while (num >= 50)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase2 = Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_YB1, pos, height, velocity, gravity);
			towerDefenseGroundItemBase2.gridPos = new Vector2I(towerDefenseGroundItemBase2.gridPos.X, 200);
			towerDefenseGroundItemBase2.Reparent(characterNode, keepGlobalTransform: false);
			if (_collect)
			{
				towerDefenseGroundItemBase2.Collection();
			}
			num -= 50;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		while (num >= 10)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase3 = Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_TQ, pos, height, velocity, gravity);
			towerDefenseGroundItemBase3.gridPos = new Vector2I(towerDefenseGroundItemBase3.gridPos.X, 200);
			towerDefenseGroundItemBase3.Reparent(characterNode, keepGlobalTransform: false);
			if (_collect)
			{
				towerDefenseGroundItemBase3.Collection();
			}
			num -= 10;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
	}

	public static int PublishSpawnedCharacter(string packetName, TowerDefenseCharacter character, bool useCreate, double riseDuration = 0.0, bool walkAfterSpawn = false, string sizeVal = "", Dictionary spawnState = null)
	{
		if (!CanBeginSpawnedCharacterPublish(packetName, character, out var control))
		{
			return -1;
		}
		int nextSyncId = control.GetNextSyncId();
		PublishSpawnedCharacterWhenReady(packetName, character, control, nextSyncId, useCreate, riseDuration, walkAfterSpawn, sizeVal, spawnState, 3);
		return nextSyncId;
	}

	private static void BroadcastSpawnedCharacter(string packetName, TowerDefenseCharacter character, TowerDefenseControlNew control, int syncId, bool useCreate, double riseDuration, bool walkAfterSpawn, string sizeVal, Dictionary spawnState)
	{
		double hitpointScale = (GodotObject.IsInstanceValid(character.instance) ? character.instance.hitpointScale : 1.0);
		double scaleVal = (GodotObject.IsInstanceValid(character.transformPoint) ? ((double)character.transformPoint.Scale.X) : 1.0);
		bool hypnoses = (GodotObject.IsInstanceValid(character.instance) && character.instance.hypnoses) || (spawnState?.GetValueOrDefault("hypnoses", false).AsBool() ?? false);
		string economyOwner = character.EconomyOwnerAccountId.ToString();
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		control.RegisterSyncCharacter(syncId, character);
		MultiPlayerManager.Instance.SendSpawnCharacterAt(packetName, character.gridPos.X, character.gridPos.Y, syncId, hitpointScale, scaleVal, hypnoses, riseDuration, useCreate, logicalGlobalPosition.X, logicalGlobalPosition.Y, walkAfterSpawn, character.groundHeight, sizeVal, spawnState, economyOwner);
	}

	private static bool CanBeginSpawnedCharacterPublish(string packetName, TowerDefenseCharacter character, out TowerDefenseControlNew control)
	{
		control = CurrentControl;
		if (!Global.IsMultiplayerMode || !MultiPlayerManager.IsHost)
		{
			return false;
		}
		if (string.IsNullOrEmpty(packetName) || !GodotObject.IsInstanceValid(character) || !GodotObject.IsInstanceValid(control) || !GodotObject.IsInstanceValid(MultiPlayerManager.Instance))
		{
			return false;
		}
		return true;
	}

	private static bool CanContinueSpawnedCharacterPublish(TowerDefenseCharacter character, TowerDefenseControlNew control)
	{
		if (!Global.IsMultiplayerMode || !MultiPlayerManager.IsHost)
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(control) || control != CurrentControl)
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(character) || character.IsQueuedForDeletion())
		{
			return false;
		}
		return GodotObject.IsInstanceValid(MultiPlayerManager.Instance);
	}

	private static void PublishSpawnedCharacterWhenReady(string packetName, TowerDefenseCharacter character, TowerDefenseControlNew control, int syncId, bool useCreate, double riseDuration, bool walkAfterSpawn, string sizeVal, Dictionary spawnState, int readyRetries)
	{
		if (CanContinueSpawnedCharacterPublish(character, control))
		{
			if (!character.IsNodeReady())
			{
				DeferSpawnedCharacterPublish(packetName, character, control, syncId, useCreate, riseDuration, walkAfterSpawn, sizeVal, spawnState, readyRetries);
			}
			else
			{
				BroadcastSpawnedCharacter(packetName, character, control, syncId, useCreate, riseDuration, walkAfterSpawn, sizeVal, spawnState);
			}
		}
	}

	private static void DeferSpawnedCharacterPublish(string packetName, TowerDefenseCharacter character, TowerDefenseControlNew control, int syncId, bool useCreate, double riseDuration, bool walkAfterSpawn, string sizeVal, Dictionary spawnState, int readyRetries)
	{
		if (readyRetries > 0)
		{
			Callable.From(() =>
			{
				PublishSpawnedCharacterWhenReady(packetName, character, control, syncId, useCreate, riseDuration, walkAfterSpawn, sizeVal, spawnState, readyRetries - 1);
			}).CallDeferred();
		}
	}

	public void LuckyBagCreate(Vector2 pos, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 0.0)
	{
		Node2D characterNode = GetCharacterNode();
		TowerDefenseGroundItemBase towerDefenseGroundItemBase = Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_LUCKY_BAG, pos, height, velocity, gravity);
		towerDefenseGroundItemBase.gridPos = new Vector2I(towerDefenseGroundItemBase.gridPos.X, 200);
		towerDefenseGroundItemBase.Reparent(characterNode, keepGlobalTransform: false);
	}

	public void GoldShardCreate(Vector2 pos, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 0.0)
	{
		Node2D characterNode = GetCharacterNode();
		TowerDefenseGroundItemBase towerDefenseGroundItemBase = Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_GOLD_SHARD, pos, height, velocity, gravity);
		towerDefenseGroundItemBase.gridPos = new Vector2I(towerDefenseGroundItemBase.gridPos.X, 200);
		towerDefenseGroundItemBase.Reparent(characterNode, keepGlobalTransform: false);
	}

	public TowerDefenseSunBase JalapenoSunCreate(Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.LAND, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		return CreateSunDrop(ObjectManagerConfig.OBJECT.SUN_JALAPENO, EconomyAccountId.Local, SunDropOwnershipPolicy.LegacySharedReplica, pos, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
	}

	public TowerDefenseSunBase JalapenoSunCreate(EconomyAccountId accountId, Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.LAND, double height = 0.0, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		if (!CanCreateAccountOwnedSunDrop(accountId))
		{
			return null;
		}
		return CreateSunDrop(ObjectManagerConfig.OBJECT.SUN_JALAPENO, accountId, SunDropOwnershipPolicy.AccountOwned, pos, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
	}

	public static Node2D _GetCharacterNode()
	{
		return GetCharacterNode();
	}

	public static bool _IsGameRunning()
	{
		if (Instance == null)
		{
			return false;
		}
		return Instance.IsGameRunning();
	}

	public EconomyAccountId GetLocalSunAccountId()
	{
		return GetSunFeature()?.LocalAccountId ?? EconomyAccountId.Local;
	}

	public bool TryResolveTrustedPeerSunAccount(string trustedSenderPeerId, out EconomyAccountId accountId)
	{
		accountId = default;
		if (string.IsNullOrWhiteSpace(trustedSenderPeerId))
		{
			return false;
		}
		MultiPlayerManager instance = MultiPlayerManager.Instance;
		if (instance != null && string.Equals(trustedSenderPeerId, instance.peerId, StringComparison.Ordinal))
		{
			accountId = EconomyAccountId.Local;
			return true;
		}
		if (!EconomyAccountId.TryFromPeerId(trustedSenderPeerId, out accountId))
		{
			return false;
		}
		return RegisterSunAccount(accountId);
	}

	public bool RegisterSunAccount(EconomyAccountId accountId, long? initialSun = null)
	{
		TowerDefenseBattleFeatureSun sunFeature = GetSunFeature();
		if (sunFeature == null)
		{
			return false;
		}
		long initialSun2 = initialSun ?? sunFeature.config?.begin ?? 0;
		return sunFeature.RegisterAccount(accountId, initialSun2);
	}

	private bool CanCreateAccountOwnedSunDrop(EconomyAccountId accountId)
	{
		if (accountId.IsValid && TryGetSun(accountId, out var _))
		{
			return true;
		}
		GD.PushWarning($"Rejected Sun drop for invalid or unregistered account {accountId}.");
		return false;
	}

	private TowerDefenseSunBase CreateSunDrop(ObjectManagerConfig.OBJECT poolKey, EconomyAccountId accountId, SunDropOwnershipPolicy ownershipPolicy, Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod, double height, Vector2 velocity, double gravity, double moveStopTime)
	{
		if (velocity == default(Vector2))
		{
			velocity = new Vector2((float)GD.RandRange(-50.0, 50.0), -400f);
		}
		Node node = ObjectManager.PoolPop(poolKey, GetCharacterNode());
		if (!(node is TowerDefenseSunBase towerDefenseSunBase))
		{
			if (GodotObject.IsInstanceValid(node))
			{
				GD.PushWarning($"Pool entry {poolKey} is not a TowerDefenseSunBase; returning it to the pool.");
				ObjectManager.PoolPush(poolKey, node);
			}
			return null;
		}
		towerDefenseSunBase.GlobalPosition = pos;
		if (ownershipPolicy == SunDropOwnershipPolicy.AccountOwned)
		{
			towerDefenseSunBase.Init(accountId, sunNum, movingMethod, height, velocity, gravity, moveStopTime);
		}
		else
		{
			towerDefenseSunBase.Init(sunNum, movingMethod, height, velocity, gravity, moveStopTime);
		}
		return towerDefenseSunBase;
	}

	public TowerDefenseBattleFeatureSun GetSunFeature()
	{
		TowerDefenseControlNew towerDefenseControlNew = currentControl;
		if (towerDefenseControlNew != null)
		{
			return towerDefenseControlNew.GetFeature(FeatureName_Sun) as TowerDefenseBattleFeatureSun;
		}
		return null;
	}

	public bool TryGetSun(EconomyAccountId accountId, out long balance)
	{
		balance = 0L;
		return GetSunFeature()?.TryGetSun(accountId, out balance) ?? false;
	}

	public long GetSun(EconomyAccountId accountId)
	{
		if (!TryGetSun(accountId, out var balance))
		{
			return -1L;
		}
		return balance;
	}

	public bool CanAffordSun(EconomyAccountId accountId, long amount)
	{
		if (TryGetSun(accountId, out var balance))
		{
			if (amount >= 0)
			{
				return balance >= amount;
			}
			return true;
		}
		return false;
	}

	public bool CreditSun(EconomyAccountId accountId, long amount, SunTransactionReason reason = SunTransactionReason.Reward)
	{
		return GetSunFeature()?.CreditSun(accountId, amount, reason) ?? false;
	}

	internal bool ApplyCollectedSunValue(EconomyAccountId accountId, long value)
	{
		return GetSunFeature()?.ApplyCollectedSunValue(accountId, value) ?? false;
	}

	public bool TrySpendSun(EconomyAccountId accountId, long amount, SunTransactionReason reason = SunTransactionReason.Cost)
	{
		return GetSunFeature()?.TrySpendSun(accountId, amount, reason) ?? false;
	}

	public bool TryBeginSunSpend(EconomyAccountId accountId, long amount, out SunSpendReceipt receipt, SunTransactionReason reason = SunTransactionReason.Cost)
	{
		receipt = null;
		return GetSunFeature()?.TryBeginSunSpend(accountId, amount, out receipt, reason) ?? false;
	}

	public bool SetSun(EconomyAccountId accountId, long value, SunTransactionReason reason = SunTransactionReason.Debug)
	{
		return GetSunFeature()?.SetSun(accountId, value, reason) ?? false;
	}

	public static TutorialConfig GetTutorial(string tutorialName)
	{
		if (string.IsNullOrEmpty(tutorialName))
		{
			return null;
		}
		if (ResourceManager.Instance.TUTORIALS.TryGetValue(tutorialName, out var value) && value is TutorialConfig result)
		{
			return result;
		}
		GD.PushWarning("[Tutorial] Tutorial '" + tutorialName + "' is unavailable.");
		return null;
	}

	public string PickRandomZomie(Godot.Collections.Array zombiePool)
	{
		Array<WeightPickItemBase> array = new Array<WeightPickItemBase>();
		foreach (Variant item2 in zombiePool)
		{
			string text = (string)item2;
			TowerDefensePacketConfig packetConfig = GetPacketConfig(text);
			TowerDefenseCharacterConfig characterConfig = packetConfig.characterConfig;
			TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig = new TowerDefenseLevelSpawnConfig
			{
				zombie = text
			};
			if (characterConfig is TowerDefenseZombieConfig)
			{
				int weight = packetConfig.GetWeight();
				WeightPickItemBase item = new WeightPickItemBase(towerDefenseLevelSpawnConfig, weight);
				array.Add(item);
			}
		}
		if (array.Count > 0)
		{
			return ((TowerDefenseLevelSpawnConfig)WeightPickMathine.Pick(array).item.AsGodotObject()).zombie;
		}
		return "";
	}

	public TowerDefenseManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/TowerDefenseManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(201)
		{
			new MethodInfo(MethodName.CreateAward, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "itemName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateAwardFromScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "awardScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.String, "itemName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetbackgroundMusicConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "backgroundMusic", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCampFriendlyFromArea, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "checkArea", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGraveStone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCampFriendlyLine, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGraveStone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCampFriendly, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGraveStone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCampTarget, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGraveStone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCampTargetFromArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "checkArea", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGraveStone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountConveyorPacketsByCharacterName, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCachedCharacterCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCharacterCountCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "currentFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "currentRevision", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterNum, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "containConveyor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterFromName, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetRainModeFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetConveyorBeltFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGloveFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetScreenEffectFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetBrainFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentProcess, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPlant, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetZombie, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCharacter, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProjectile, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffect, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectCountForPhysicsFrame, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLineCharacters, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterHasTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGraveStone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterHasTargetFromArea, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "checkArea", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGraveStone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterTargetNearest, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "method", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGravestone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNearCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "method", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGravestone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterVase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTallCharacterTargetFromRect, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Rect2, "checkRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "groundRight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterTargetNearestFromRect, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Rect2, "checkRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "method", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGravestone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterHasTargetFromRect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Rect2, "checkRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGraveStone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterVase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasTrackTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "collectionFlag", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canTargetGargantuar", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "groundRight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterTargetNearestFromArea, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "checkArea", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "method", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGravestone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharactersForLine, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileHasTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGraveStone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileHasTargetFromArea, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "checkArea", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGraveStone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileTargetNearest, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGravestone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileTargetNearest, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "speed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGravestone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "deprioritizeDisabledTargets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileTargetNearestForPhysicsFrame, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "speed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGravestone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "deprioritizeDisabledTargets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileTargetNearestProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "method", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGravestone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileInitialTrackTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "projectilePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "method", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fliterGravestone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BrainSunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BungiSpawn, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "override_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "hypnoses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "waveOperationId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "skipPlacementCheck", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsBungiWaveOperationCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveOperationId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "waveFeature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateBungiSpawnNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "override_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "waveFeature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "ownedWaveOperationId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "battleControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "battleOperationId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "characterNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CleanupFailedBungiSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "battleControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "battleOperationId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "waveFeature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "ownedWaveOperationId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncBungiSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hypnoses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "battleControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "skipPlacementCheck", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CharacterRegister, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CharacterUnregister, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "charcterSpriteName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketSpriteScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetChacraterScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "charcterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCoin, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddCoin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UseCoin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCollectable, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "collectableName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MapIsChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReevaluateCharacterSleepStates, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CharacterDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_scale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_hitpointScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearDeathRecords, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.HasDeathRecord, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "requireAngelEligible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetEffectDirtName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "effectSpriteName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEffectParticlesOnce, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEffectSpriteOnce, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEffectParticlesSceneOnce, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GPUParticles2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEffectSpriteSceneOnce, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryCreateEffectSceneOnceFast, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "preferSprite", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FallingObjectCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FallingObjectItemCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureFallingObjectSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "objectId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetGameMethod, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.IsIZMMode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsIZM2Mode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsLevelEditorStage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.IsGameRunning, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsUnlimitedFire, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CoinCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_collect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddSun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UseSun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSun, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLevelChapterFinishNum, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelListName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "chapterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLevelEvent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetNextLevel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelChoose", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "chapterId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "levelId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadNextLevelConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "nextLevel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanOpenNextLevel, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "nextLevel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveNextLevelProgress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "chapterId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "levelId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLevelControl, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TipsPlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLevelHomeworld, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MagicSunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapAttackDpsLifestealRatio, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ApplyMapAttackDpsLifesteal, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "attacker", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "damage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMapCharacterRules, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapZombieColumnSpeedMultiplier, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "gridX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentMap, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "mapName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MapChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "map", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapIsNight, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.MapDayNightSwitch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_switchTimer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "returnDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapGridNum, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapGridSize, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapGridBeginPos, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapPlantOffset, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapGridPos, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapCellPos, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapCellPosCenter, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckMapGridPosIn, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapCell, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMapGridType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cellConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetMapLineUse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "use", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapLineUse, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapGroundLeft, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapGroundRight, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapGroundUp, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapGroundDown, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapGridPosFromMouse, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapControl, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseBattleReferences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "preserveLevelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ApplyMapPacketCooldownRules, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "multiplier", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MapIgnoresDynamicPacketCostGrowth, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapCellPlantPos, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapLineY, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentMapConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapCurrentMap, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPacketPickControl, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MapLineHasType, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapPlantGrid, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapLineUseArr, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapIceCapList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGroundRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetIceCapPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MapHasSpecialRule, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "ruleId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MapSpecialRulesPreventSleep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetActiveMapConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.SelectNextModLevel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMowerConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "mowerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMowerList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetMowerNum, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMower, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMower, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMowerFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMowerManager, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetNpcTalk, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "npcTalkName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketBank, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPacketBankFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPacketBankData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetBank", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketConfigReadOnlyByCharacterName, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketConfigCostLower, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketConfigCostLowerWithTypeList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "typeList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketConfigCostUpper, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketConfigCostUpperWithTypeList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "typeList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketConfigReadOnly, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePacketShow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePacketShowWithConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindSpawnedPacketPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.PublishSpawnedPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "aliveTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isFall", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "velocityX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "velocityY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPortalFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PortalCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "shape", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector4I, "posRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "changeTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProtalCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "shape", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector4I, "posRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "changeTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PortalChangePos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProtalChangePos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProjectileConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QXSunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GetCleanCharacters, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetNodesInGroupCached, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "groupName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSeedBankFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentPacketBankMethod, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSeedBank, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChangeCostAdd, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "changeCost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ChangeCostRemove, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "changeCost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketSlotNum, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "override_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSeedBankList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetShovel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "shovelName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetShovelList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.YBCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_collect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PublishSpawnedCharacter, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "useCreate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "riseDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "walkAfterSpawn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sizeVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "spawnState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BroadcastSpawnedCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useCreate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "riseDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "walkAfterSpawn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sizeVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "spawnState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanContinueSpawnedCharacterPublish, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.PublishSpawnedCharacterWhenReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useCreate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "riseDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "walkAfterSpawn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sizeVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "spawnState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "readyRetries", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DeferSpawnedCharacterPublish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useCreate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "riseDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "walkAfterSpawn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sizeVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "spawnState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "readyRetries", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LuckyBagCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GoldShardCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.JalapenoSunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GetCharacterNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._IsGameRunning, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetSunFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetTutorial, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tutorialName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PickRandomZomie, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "zombiePool", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateAward && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseAwardBase>(CreateAward(VariantUtils.ConvertTo<TowerDefenseEnum.LEVEL_REWARDTYPE>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateAwardFromScene && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseAwardBase>(CreateAwardFromScene(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.GetbackgroundMusicConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBackgroundMusicConfig>(GetbackgroundMusicConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SunCreate && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(SunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6])));
			return true;
		}
		if (method == MethodName.GetCampFriendlyFromArea && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCampFriendlyFromArea(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<AabbArea2D>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.GetCampFriendlyLine && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCampFriendlyLine(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCampFriendly && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCampFriendly(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCampTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCampTarget(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCampTargetFromArray && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCampTargetFromArray(VariantUtils.ConvertTo<AabbArea2D>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.CountConveyorPacketsByCharacterName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountConveyorPacketsByCharacterName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCachedCharacterCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetCachedCharacterCount(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshCharacterCountCache && args.Count == 2)
		{
			RefreshCharacterCountCache(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCharacterNum && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetCharacterNum(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCharacterFromName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCharacterFromName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetRainModeFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureRainMode>(GetRainModeFeature());
			return true;
		}
		if (method == MethodName.GetConveyorBeltFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureConveyorBelt>(GetConveyorBeltFeature());
			return true;
		}
		if (method == MethodName.GetGloveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureGlove>(GetGloveFeature());
			return true;
		}
		if (method == MethodName.GetScreenEffectFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureScreenEffect>(GetScreenEffectFeature());
			return true;
		}
		if (method == MethodName.GetBrainFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureBrain>(GetBrainFeature());
			return true;
		}
		if (method == MethodName.GetCurrentProcess && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleProcess>(GetCurrentProcess());
			return true;
		}
		if (method == MethodName.GetPlant && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetPlant());
			return true;
		}
		if (method == MethodName.GetZombie && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetZombie());
			return true;
		}
		if (method == MethodName.GetCharacter && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCharacter());
			return true;
		}
		if (method == MethodName.GetProjectile && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetProjectile());
			return true;
		}
		if (method == MethodName.GetEffect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetEffect());
			return true;
		}
		if (method == MethodName.GetEffectCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectCount());
			return true;
		}
		if (method == MethodName.GetEffectCountForPhysicsFrame && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectCountForPhysicsFrame(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLineCharacters && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetLineCharacters(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterHasTarget && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(GetCharacterHasTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.GetCharacterHasTargetFromArea && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(GetCharacterHasTargetFromArea(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<AabbArea2D>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.GetCharacterTargetNearest && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetCharacterTargetNearest(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.TARGET_NEAR_METHOD>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.GetNearCharacter && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetNearCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.TARGET_NEAR_METHOD>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.GetTallCharacterTargetFromRect && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetTallCharacterTargetFromRect(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<double>(in args[4])));
			return true;
		}
		if (method == MethodName.GetCharacterTargetNearestFromRect && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetCharacterTargetNearestFromRect(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.TARGET_NEAR_METHOD>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.GetCharacterHasTargetFromRect && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<bool>(GetCharacterHasTargetFromRect(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.HasTrackTarget && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(HasTrackTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
			return true;
		}
		if (method == MethodName.GetCharacterTargetNearestFromArea && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetCharacterTargetNearestFromArea(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<AabbArea2D>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.TARGET_NEAR_METHOD>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.GetCharactersForLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCharactersForLine(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetProjectileHasTarget && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(GetProjectileHasTarget(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.GetProjectileHasTargetFromArea && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(GetProjectileHasTargetFromArea(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<AabbArea2D>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.GetProjectileTargetNearest && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetProjectileTargetNearest(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.GetProjectileTargetNearest && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetProjectileTargetNearest(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6])));
			return true;
		}
		if (method == MethodName.GetProjectileTargetNearestForPhysicsFrame && args.Count == 8)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetProjectileTargetNearestForPhysicsFrame(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<ulong>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7])));
			return true;
		}
		if (method == MethodName.GetProjectileTargetNearestProjectile && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetProjectileTargetNearestProjectile(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.TARGET_NEAR_METHOD>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.GetProjectileInitialTrackTarget && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetProjectileInitialTrackTarget(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.TARGET_NEAR_METHOD>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.BrainSunCreate && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(BrainSunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6])));
			return true;
		}
		if (method == MethodName.BungiSpawn && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(BungiSpawn(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.IsBungiWaveOperationCurrent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBungiWaveOperationCurrent(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateBungiSpawnNode && args.Count == 8)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombieBungiSpawn>(CreateBungiSpawnNode(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in args[2]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<Node>(in args[7])));
			return true;
		}
		if (method == MethodName.CleanupFailedBungiSpawn && args.Count == 5)
		{
			CleanupFailedBungiSpawn(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<TowerDefenseZombieBungiSpawn>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncBungiSpawn && args.Count == 6)
		{
			SyncBungiSpawn(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<TowerDefenseZombieBungiSpawn>(in args[3]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCharacterNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(GetCharacterNode());
			return true;
		}
		if (method == MethodName.CharacterRegister && args.Count == 1)
		{
			CharacterRegister(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CharacterUnregister && args.Count == 1)
		{
			CharacterUnregister(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCharacterSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(GetCharacterSprite(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketSpriteScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetPacketSpriteScene(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(GetPacketSprite(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetChacraterScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetChacraterScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(CreateCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCoin && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetCoin());
			return true;
		}
		if (method == MethodName.AddCoin && args.Count == 1)
		{
			AddCoin(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UseCoin && args.Count == 1)
		{
			UseCoin(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCollectable && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CollectableConfig>(GetCollectable(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.MapIsChange && args.Count == 0)
		{
			MapIsChange();
			ret = default;
			return true;
		}
		if (method == MethodName.ReevaluateCharacterSleepStates && args.Count == 0)
		{
			ReevaluateCharacterSleepStates();
			ret = default;
			return true;
		}
		if (method == MethodName.CharacterDestroy && args.Count == 6)
		{
			CharacterDestroy(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearDeathRecords && args.Count == 0)
		{
			ClearDeathRecords();
			ret = default;
			return true;
		}
		if (method == MethodName.HasDeathRecord && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasDeathRecord(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetEffectDirtName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetEffectDirtName());
			return true;
		}
		if (method == MethodName.GetEffectSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(GetEffectSprite(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateEffectParticlesOnce && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectParticlesOnce>(CreateEffectParticlesOnce(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateEffectSpriteOnce && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectSpriteOnce>(CreateEffectSpriteOnce(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateEffectParticlesSceneOnce && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectParticlesOnce>(CreateEffectParticlesSceneOnce(VariantUtils.ConvertTo<GPUParticles2DOnece>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateEffectSpriteSceneOnce && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectSpriteOnce>(CreateEffectSpriteSceneOnce(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.TryCreateEffectSceneOnceFast && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<bool>(TryCreateEffectSceneOnceFast(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.FallingObjectCreate && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseGroundItemBase>(FallingObjectCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
			return true;
		}
		if (method == MethodName.FallingObjectItemCreate && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseGroundItemBase>(FallingObjectItemCreate(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<double>(in args[4])));
			return true;
		}
		if (method == MethodName.CaptureFallingObjectSpawn && args.Count == 6)
		{
			CaptureFallingObjectSpawn(VariantUtils.ConvertTo<TowerDefenseGroundItemBase>(in args[0]), VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetGameMethod && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEnum.LEVEL_FINISH_METHOD>(GetGameMethod());
			return true;
		}
		if (method == MethodName.IsIZMMode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIZMMode());
			return true;
		}
		if (method == MethodName.IsIZM2Mode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIZM2Mode());
			return true;
		}
		if (method == MethodName.IsLevelEditorStage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLevelEditorStage());
			return true;
		}
		if (method == MethodName.IsGameRunning && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGameRunning());
			return true;
		}
		if (method == MethodName.IsUnlimitedFire && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsUnlimitedFire());
			return true;
		}
		if (method == MethodName.CoinCreate && args.Count == 6)
		{
			CoinCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSun && args.Count == 1)
		{
			AddSun(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UseSun && args.Count == 1)
		{
			UseSun(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSun && args.Count == 1)
		{
			SetSun(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSun && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetSun());
			return true;
		}
		if (method == MethodName.GetLevelChapterFinishNum && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetLevelChapterFinishNum(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetLevelEvent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelEventBase>(GetLevelEvent(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetNextLevel && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelBaseConfig>(SetNextLevel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.LoadNextLevelConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(LoadNextLevelConfig(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.CanOpenNextLevel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanOpenNextLevel(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.SaveNextLevelProgress && args.Count == 2)
		{
			SaveNextLevelProgress(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetLevelControl && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGameLevelControl>(GetLevelControl());
			return true;
		}
		if (method == MethodName.TipsPlay && args.Count == 2)
		{
			TipsPlay(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetLevelHomeworld && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<GeneralEnum.HOMEWORLD>(GetLevelHomeworld());
			return true;
		}
		if (method == MethodName.MagicSunCreate && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(MagicSunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6])));
			return true;
		}
		if (method == MethodName.GetMapAttackDpsLifestealRatio && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapAttackDpsLifestealRatio());
			return true;
		}
		if (method == MethodName.ApplyMapAttackDpsLifesteal && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyMapAttackDpsLifesteal(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyMapCharacterRules && args.Count == 1)
		{
			ApplyMapCharacterRules(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMapZombieColumnSpeedMultiplier && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapZombieColumnSpeedMultiplier(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurrentMap && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMap>(GetCurrentMap());
			return true;
		}
		if (method == MethodName.GetMapConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapConfig>(GetMapConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MapChange && args.Count == 3)
		{
			MapChange(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMapIsNight && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(GetMapIsNight());
			return true;
		}
		if (method == MethodName.MapDayNightSwitch && args.Count == 3)
		{
			MapDayNightSwitch(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMapGridNum && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetMapGridNum());
			return true;
		}
		if (method == MethodName.GetMapGridSize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMapGridSize());
			return true;
		}
		if (method == MethodName.GetMapGridBeginPos && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMapGridBeginPos());
			return true;
		}
		if (method == MethodName.GetMapPlantOffset && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapPlantOffset());
			return true;
		}
		if (method == MethodName.GetMapGridPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetMapGridPos(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapCellPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMapCellPos(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapCellPosCenter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMapCellPosCenter(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.CheckMapGridPosIn && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckMapGridPosIn(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapCell && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCellInstance>(GetMapCell(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.SetMapGridType && args.Count == 1)
		{
			SetMapGridType(VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMapLineUse && args.Count == 2)
		{
			SetMapLineUse(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMapLineUse && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(GetMapLineUse(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapGroundLeft && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapGroundLeft());
			return true;
		}
		if (method == MethodName.GetMapGroundRight && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapGroundRight());
			return true;
		}
		if (method == MethodName.GetMapGroundUp && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapGroundUp());
			return true;
		}
		if (method == MethodName.GetMapGroundDown && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapGroundDown());
			return true;
		}
		if (method == MethodName.GetMapGridPosFromMouse && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetMapGridPosFromMouse(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapControl && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapControl>(GetMapControl());
			return true;
		}
		if (method == MethodName.ReleaseBattleReferences && args.Count == 2)
		{
			ReleaseBattleReferences(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMapFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(GetMapFeature());
			return true;
		}
		if (method == MethodName.ApplyMapPacketCooldownRules && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyMapPacketCooldownRules(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.MapIgnoresDynamicPacketCostGrowth && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MapIgnoresDynamicPacketCostGrowth(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapCellPlantPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMapCellPlantPos(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapLineY && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapLineY(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurrentMapConfig && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapConfig>(GetCurrentMapConfig());
			return true;
		}
		if (method == MethodName.GetMapCurrentMap && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMap>(GetMapCurrentMap());
			return true;
		}
		if (method == MethodName.GetPacketPickControl && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PacketPickControl>(GetPacketPickControl());
			return true;
		}
		if (method == MethodName.MapLineHasType && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(MapLineHasType(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.PLANTGRIDTYPE>(in args[1])));
			return true;
		}
		if (method == MethodName.GetMapPlantGrid && args.Count == 0)
		{
			Array<Godot.Collections.Array> mapPlantGrid = GetMapPlantGrid();
			ret = VariantUtils.CreateFromArray(mapPlantGrid);
			return true;
		}
		if (method == MethodName.GetMapLineUseArr && args.Count == 0)
		{
			Array<bool> mapLineUseArr = GetMapLineUseArr();
			ret = VariantUtils.CreateFromArray(mapLineUseArr);
			return true;
		}
		if (method == MethodName.GetMapIceCapList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetMapIceCapList());
			return true;
		}
		if (method == MethodName.GetGroundRect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetGroundRect());
			return true;
		}
		if (method == MethodName.SetIceCapPos && args.Count == 2)
		{
			SetIceCapPos(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MapHasSpecialRule && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MapHasSpecialRule(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.MapSpecialRulesPreventSleep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(MapSpecialRulesPreventSleep());
			return true;
		}
		if (method == MethodName.GetActiveMapConfig && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapConfig>(GetActiveMapConfig());
			return true;
		}
		if (method == MethodName.SelectNextModLevel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelBaseConfig>(SelectNextModLevel());
			return true;
		}
		if (method == MethodName.GetMowerConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<MowerConfig>(GetMowerConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMowerList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetMowerList());
			return true;
		}
		if (method == MethodName.GetMowerNum && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetMowerNum());
			return true;
		}
		if (method == MethodName.GetMower && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetMower());
			return true;
		}
		if (method == MethodName.CreateMower && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMower>(CreateMower(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMowerFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMower>(GetMowerFeature());
			return true;
		}
		if (method == MethodName.GetMowerManager && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMowerManager>(GetMowerManager());
			return true;
		}
		if (method == MethodName.GetNpcTalk && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<NpcTalkConfig>(GetNpcTalk(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketBank && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketBank>(GetPacketBank());
			return true;
		}
		if (method == MethodName.GetPacketBankFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeaturePacketBank>(GetPacketBankFeature());
			return true;
		}
		if (method == MethodName.GetPacketBankData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketBankData>(GetPacketBankData(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketConfigReadOnlyByCharacterName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(GetPacketConfigReadOnlyByCharacterName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketConfigCostLower && args.Count == 2)
		{
			Array<TowerDefensePacketConfig> packetConfigCostLower = GetPacketConfigCostLower(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[1]));
			ret = VariantUtils.CreateFromArray(packetConfigCostLower);
			return true;
		}
		if (method == MethodName.GetPacketConfigCostLowerWithTypeList && args.Count == 2)
		{
			Array<TowerDefensePacketConfig> packetConfigCostLowerWithTypeList = GetPacketConfigCostLowerWithTypeList(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseEnum.PACKET_TYPE>(in args[1]));
			ret = VariantUtils.CreateFromArray(packetConfigCostLowerWithTypeList);
			return true;
		}
		if (method == MethodName.GetPacketConfigCostUpper && args.Count == 2)
		{
			Array<TowerDefensePacketConfig> packetConfigCostUpper = GetPacketConfigCostUpper(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[1]));
			ret = VariantUtils.CreateFromArray(packetConfigCostUpper);
			return true;
		}
		if (method == MethodName.GetPacketConfigCostUpperWithTypeList && args.Count == 2)
		{
			Array<TowerDefensePacketConfig> packetConfigCostUpperWithTypeList = GetPacketConfigCostUpperWithTypeList(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseEnum.PACKET_TYPE>(in args[1]));
			ret = VariantUtils.CreateFromArray(packetConfigCostUpperWithTypeList);
			return true;
		}
		if (method == MethodName.GetPacketConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(GetPacketConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketConfigReadOnly && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(GetPacketConfigReadOnly(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreatePacketShow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreatePacketShow(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreatePacketShowWithConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreatePacketShowWithConfig(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BindSpawnedPacketPicker && args.Count == 1)
		{
			BindSpawnedPacketPicker(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PublishSpawnedPacket && args.Count == 9)
		{
			PublishSpawnedPacket(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]), VariantUtils.ConvertTo<double>(in args[7]), VariantUtils.ConvertTo<double>(in args[8]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPortalFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeaturePortal>(GetPortalFeature());
			return true;
		}
		if (method == MethodName.PortalCreate && args.Count == 3)
		{
			PortalCreate(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector4I>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProtalCreate && args.Count == 3)
		{
			ProtalCreate(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector4I>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PortalChangePos && args.Count == 0)
		{
			PortalChangePos();
			ret = default;
			return true;
		}
		if (method == MethodName.ProtalChangePos && args.Count == 0)
		{
			ProtalChangePos();
			ret = default;
			return true;
		}
		if (method == MethodName.GetProjectileConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileConfig>(GetProjectileConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.QXSunCreate && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(QXSunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6])));
			return true;
		}
		if (method == MethodName._GetCleanCharacters && args.Count == 0)
		{
			Array<TowerDefenseCharacter> array = _GetCleanCharacters();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName._GetNodesInGroupCached && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(_GetNodesInGroupCached(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSeedBankFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureSeedBank>(GetSeedBankFeature());
			return true;
		}
		if (method == MethodName.GetCurrentPacketBankMethod && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(GetCurrentPacketBankMethod());
			return true;
		}
		if (method == MethodName.GetSeedBank && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGameSeedBank>(GetSeedBank());
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
		if (method == MethodName.GetPacketSlotNum && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPacketSlotNum());
			return true;
		}
		if (method == MethodName.AddPacket && args.Count == 2)
		{
			AddPacket(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketOverride>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSeedBankList && args.Count == 0)
		{
			Array<TowerDefenseInGamePacketShow> seedBankList = GetSeedBankList();
			ret = VariantUtils.CreateFromArray(seedBankList);
			return true;
		}
		if (method == MethodName.GetShovel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ShovelConfig>(GetShovel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetShovelList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetShovelList());
			return true;
		}
		if (method == MethodName.YBCreate && args.Count == 6)
		{
			YBCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.PublishSpawnedCharacter && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<int>(PublishSpawnedCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]), VariantUtils.ConvertTo<Dictionary>(in args[6])));
			return true;
		}
		if (method == MethodName.BroadcastSpawnedCharacter && args.Count == 9)
		{
			BroadcastSpawnedCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<string>(in args[7]), VariantUtils.ConvertTo<Dictionary>(in args[8]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanContinueSpawnedCharacterPublish && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanContinueSpawnedCharacterPublish(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[1])));
			return true;
		}
		if (method == MethodName.PublishSpawnedCharacterWhenReady && args.Count == 10)
		{
			PublishSpawnedCharacterWhenReady(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<string>(in args[7]), VariantUtils.ConvertTo<Dictionary>(in args[8]), VariantUtils.ConvertTo<int>(in args[9]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeferSpawnedCharacterPublish && args.Count == 10)
		{
			DeferSpawnedCharacterPublish(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<string>(in args[7]), VariantUtils.ConvertTo<Dictionary>(in args[8]), VariantUtils.ConvertTo<int>(in args[9]));
			ret = default;
			return true;
		}
		if (method == MethodName.LuckyBagCreate && args.Count == 4)
		{
			LuckyBagCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.GoldShardCreate && args.Count == 4)
		{
			GoldShardCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.JalapenoSunCreate && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(JalapenoSunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6])));
			return true;
		}
		if (method == MethodName._GetCharacterNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(_GetCharacterNode());
			return true;
		}
		if (method == MethodName._IsGameRunning && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(_IsGameRunning());
			return true;
		}
		if (method == MethodName.GetSunFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureSun>(GetSunFeature());
			return true;
		}
		if (method == MethodName.GetTutorial && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TutorialConfig>(GetTutorial(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.PickRandomZomie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(PickRandomZomie(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsBungiWaveOperationCurrent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBungiWaveOperationCurrent(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateBungiSpawnNode && args.Count == 8)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombieBungiSpawn>(CreateBungiSpawnNode(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in args[2]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<Node>(in args[7])));
			return true;
		}
		if (method == MethodName.CleanupFailedBungiSpawn && args.Count == 5)
		{
			CleanupFailedBungiSpawn(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<TowerDefenseZombieBungiSpawn>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncBungiSpawn && args.Count == 6)
		{
			SyncBungiSpawn(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<TowerDefenseZombieBungiSpawn>(in args[3]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCharacterNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(GetCharacterNode());
			return true;
		}
		if (method == MethodName.GetCharacterSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(GetCharacterSprite(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketSpriteScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetPacketSpriteScene(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(GetPacketSprite(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetChacraterScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetChacraterScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCollectable && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CollectableConfig>(GetCollectable(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearDeathRecords && args.Count == 0)
		{
			ClearDeathRecords();
			ret = default;
			return true;
		}
		if (method == MethodName.HasDeathRecord && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasDeathRecord(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateEffectParticlesOnce && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectParticlesOnce>(CreateEffectParticlesOnce(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateEffectSpriteOnce && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectSpriteOnce>(CreateEffectSpriteOnce(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.TryCreateEffectSceneOnceFast && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<bool>(TryCreateEffectSceneOnceFast(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.GetGameMethod && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEnum.LEVEL_FINISH_METHOD>(GetGameMethod());
			return true;
		}
		if (method == MethodName.IsLevelEditorStage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLevelEditorStage());
			return true;
		}
		if (method == MethodName.IsUnlimitedFire && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsUnlimitedFire());
			return true;
		}
		if (method == MethodName.GetMapAttackDpsLifestealRatio && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapAttackDpsLifestealRatio());
			return true;
		}
		if (method == MethodName.ApplyMapAttackDpsLifesteal && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyMapAttackDpsLifesteal(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyMapCharacterRules && args.Count == 1)
		{
			ApplyMapCharacterRules(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMapZombieColumnSpeedMultiplier && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapZombieColumnSpeedMultiplier(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapIsNight && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(GetMapIsNight());
			return true;
		}
		if (method == MethodName.GetMapCell && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCellInstance>(GetMapCell(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(GetMapFeature());
			return true;
		}
		if (method == MethodName.ApplyMapPacketCooldownRules && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyMapPacketCooldownRules(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.MapIgnoresDynamicPacketCostGrowth && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MapIgnoresDynamicPacketCostGrowth(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapCellPlantPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMapCellPlantPos(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapLineY && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetMapLineY(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.MapLineHasType && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(MapLineHasType(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.PLANTGRIDTYPE>(in args[1])));
			return true;
		}
		if (method == MethodName.MapHasSpecialRule && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MapHasSpecialRule(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.MapSpecialRulesPreventSleep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(MapSpecialRulesPreventSleep());
			return true;
		}
		if (method == MethodName.GetActiveMapConfig && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapConfig>(GetActiveMapConfig());
			return true;
		}
		if (method == MethodName.GetMowerConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<MowerConfig>(GetMowerConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMowerList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetMowerList());
			return true;
		}
		if (method == MethodName.GetNpcTalk && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<NpcTalkConfig>(GetNpcTalk(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketBankData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketBankData>(GetPacketBankData(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketConfigReadOnlyByCharacterName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(GetPacketConfigReadOnlyByCharacterName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketConfigCostLower && args.Count == 2)
		{
			Array<TowerDefensePacketConfig> packetConfigCostLower = GetPacketConfigCostLower(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[1]));
			ret = VariantUtils.CreateFromArray(packetConfigCostLower);
			return true;
		}
		if (method == MethodName.GetPacketConfigCostLowerWithTypeList && args.Count == 2)
		{
			Array<TowerDefensePacketConfig> packetConfigCostLowerWithTypeList = GetPacketConfigCostLowerWithTypeList(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseEnum.PACKET_TYPE>(in args[1]));
			ret = VariantUtils.CreateFromArray(packetConfigCostLowerWithTypeList);
			return true;
		}
		if (method == MethodName.GetPacketConfigCostUpper && args.Count == 2)
		{
			Array<TowerDefensePacketConfig> packetConfigCostUpper = GetPacketConfigCostUpper(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[1]));
			ret = VariantUtils.CreateFromArray(packetConfigCostUpper);
			return true;
		}
		if (method == MethodName.GetPacketConfigCostUpperWithTypeList && args.Count == 2)
		{
			Array<TowerDefensePacketConfig> packetConfigCostUpperWithTypeList = GetPacketConfigCostUpperWithTypeList(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseEnum.PACKET_TYPE>(in args[1]));
			ret = VariantUtils.CreateFromArray(packetConfigCostUpperWithTypeList);
			return true;
		}
		if (method == MethodName.GetPacketConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(GetPacketConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketConfigReadOnly && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(GetPacketConfigReadOnly(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreatePacketShow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreatePacketShow(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreatePacketShowWithConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreatePacketShowWithConfig(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetProjectileConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileConfig>(GetProjectileConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetShovel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ShovelConfig>(GetShovel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetShovelList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetShovelList());
			return true;
		}
		if (method == MethodName.PublishSpawnedCharacter && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<int>(PublishSpawnedCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]), VariantUtils.ConvertTo<Dictionary>(in args[6])));
			return true;
		}
		if (method == MethodName.BroadcastSpawnedCharacter && args.Count == 9)
		{
			BroadcastSpawnedCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<string>(in args[7]), VariantUtils.ConvertTo<Dictionary>(in args[8]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanContinueSpawnedCharacterPublish && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanContinueSpawnedCharacterPublish(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[1])));
			return true;
		}
		if (method == MethodName.PublishSpawnedCharacterWhenReady && args.Count == 10)
		{
			PublishSpawnedCharacterWhenReady(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<string>(in args[7]), VariantUtils.ConvertTo<Dictionary>(in args[8]), VariantUtils.ConvertTo<int>(in args[9]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeferSpawnedCharacterPublish && args.Count == 10)
		{
			DeferSpawnedCharacterPublish(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<string>(in args[7]), VariantUtils.ConvertTo<Dictionary>(in args[8]), VariantUtils.ConvertTo<int>(in args[9]));
			ret = default;
			return true;
		}
		if (method == MethodName._GetCharacterNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(_GetCharacterNode());
			return true;
		}
		if (method == MethodName._IsGameRunning && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(_IsGameRunning());
			return true;
		}
		if (method == MethodName.GetTutorial && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TutorialConfig>(GetTutorial(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.CreateAward)
		{
			return true;
		}
		if (method == MethodName.CreateAwardFromScene)
		{
			return true;
		}
		if (method == MethodName.GetbackgroundMusicConfig)
		{
			return true;
		}
		if (method == MethodName.SunCreate)
		{
			return true;
		}
		if (method == MethodName.GetCampFriendlyFromArea)
		{
			return true;
		}
		if (method == MethodName.GetCampFriendlyLine)
		{
			return true;
		}
		if (method == MethodName.GetCampFriendly)
		{
			return true;
		}
		if (method == MethodName.GetCampTarget)
		{
			return true;
		}
		if (method == MethodName.GetCampTargetFromArray)
		{
			return true;
		}
		if (method == MethodName.CountConveyorPacketsByCharacterName)
		{
			return true;
		}
		if (method == MethodName.GetCachedCharacterCount)
		{
			return true;
		}
		if (method == MethodName.RefreshCharacterCountCache)
		{
			return true;
		}
		if (method == MethodName.GetCharacterNum)
		{
			return true;
		}
		if (method == MethodName.GetCharacterFromName)
		{
			return true;
		}
		if (method == MethodName.GetRainModeFeature)
		{
			return true;
		}
		if (method == MethodName.GetConveyorBeltFeature)
		{
			return true;
		}
		if (method == MethodName.GetGloveFeature)
		{
			return true;
		}
		if (method == MethodName.GetScreenEffectFeature)
		{
			return true;
		}
		if (method == MethodName.GetBrainFeature)
		{
			return true;
		}
		if (method == MethodName.GetCurrentProcess)
		{
			return true;
		}
		if (method == MethodName.GetPlant)
		{
			return true;
		}
		if (method == MethodName.GetZombie)
		{
			return true;
		}
		if (method == MethodName.GetCharacter)
		{
			return true;
		}
		if (method == MethodName.GetProjectile)
		{
			return true;
		}
		if (method == MethodName.GetEffect)
		{
			return true;
		}
		if (method == MethodName.GetEffectCount)
		{
			return true;
		}
		if (method == MethodName.GetEffectCountForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.GetLineCharacters)
		{
			return true;
		}
		if (method == MethodName.GetCharacterHasTarget)
		{
			return true;
		}
		if (method == MethodName.GetCharacterHasTargetFromArea)
		{
			return true;
		}
		if (method == MethodName.GetCharacterTargetNearest)
		{
			return true;
		}
		if (method == MethodName.GetNearCharacter)
		{
			return true;
		}
		if (method == MethodName.GetTallCharacterTargetFromRect)
		{
			return true;
		}
		if (method == MethodName.GetCharacterTargetNearestFromRect)
		{
			return true;
		}
		if (method == MethodName.GetCharacterHasTargetFromRect)
		{
			return true;
		}
		if (method == MethodName.HasTrackTarget)
		{
			return true;
		}
		if (method == MethodName.GetCharacterTargetNearestFromArea)
		{
			return true;
		}
		if (method == MethodName.GetCharactersForLine)
		{
			return true;
		}
		if (method == MethodName.GetProjectileHasTarget)
		{
			return true;
		}
		if (method == MethodName.GetProjectileHasTargetFromArea)
		{
			return true;
		}
		if (method == MethodName.GetProjectileTargetNearest)
		{
			return true;
		}
		if (method == MethodName.GetProjectileTargetNearestForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.GetProjectileTargetNearestProjectile)
		{
			return true;
		}
		if (method == MethodName.GetProjectileInitialTrackTarget)
		{
			return true;
		}
		if (method == MethodName.BrainSunCreate)
		{
			return true;
		}
		if (method == MethodName.BungiSpawn)
		{
			return true;
		}
		if (method == MethodName.IsBungiWaveOperationCurrent)
		{
			return true;
		}
		if (method == MethodName.CreateBungiSpawnNode)
		{
			return true;
		}
		if (method == MethodName.CleanupFailedBungiSpawn)
		{
			return true;
		}
		if (method == MethodName.SyncBungiSpawn)
		{
			return true;
		}
		if (method == MethodName.GetCharacterNode)
		{
			return true;
		}
		if (method == MethodName.CharacterRegister)
		{
			return true;
		}
		if (method == MethodName.CharacterUnregister)
		{
			return true;
		}
		if (method == MethodName.GetCharacterSprite)
		{
			return true;
		}
		if (method == MethodName.GetPacketSpriteScene)
		{
			return true;
		}
		if (method == MethodName.GetPacketSprite)
		{
			return true;
		}
		if (method == MethodName.GetChacraterScene)
		{
			return true;
		}
		if (method == MethodName.CreateCharacter)
		{
			return true;
		}
		if (method == MethodName.GetCoin)
		{
			return true;
		}
		if (method == MethodName.AddCoin)
		{
			return true;
		}
		if (method == MethodName.UseCoin)
		{
			return true;
		}
		if (method == MethodName.GetCollectable)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.MapIsChange)
		{
			return true;
		}
		if (method == MethodName.ReevaluateCharacterSleepStates)
		{
			return true;
		}
		if (method == MethodName.CharacterDestroy)
		{
			return true;
		}
		if (method == MethodName.ClearDeathRecords)
		{
			return true;
		}
		if (method == MethodName.HasDeathRecord)
		{
			return true;
		}
		if (method == MethodName.GetEffectDirtName)
		{
			return true;
		}
		if (method == MethodName.GetEffectSprite)
		{
			return true;
		}
		if (method == MethodName.CreateEffectParticlesOnce)
		{
			return true;
		}
		if (method == MethodName.CreateEffectSpriteOnce)
		{
			return true;
		}
		if (method == MethodName.CreateEffectParticlesSceneOnce)
		{
			return true;
		}
		if (method == MethodName.CreateEffectSpriteSceneOnce)
		{
			return true;
		}
		if (method == MethodName.TryCreateEffectSceneOnceFast)
		{
			return true;
		}
		if (method == MethodName.FallingObjectCreate)
		{
			return true;
		}
		if (method == MethodName.FallingObjectItemCreate)
		{
			return true;
		}
		if (method == MethodName.CaptureFallingObjectSpawn)
		{
			return true;
		}
		if (method == MethodName.GetGameMethod)
		{
			return true;
		}
		if (method == MethodName.IsIZMMode)
		{
			return true;
		}
		if (method == MethodName.IsIZM2Mode)
		{
			return true;
		}
		if (method == MethodName.IsLevelEditorStage)
		{
			return true;
		}
		if (method == MethodName.IsGameRunning)
		{
			return true;
		}
		if (method == MethodName.IsUnlimitedFire)
		{
			return true;
		}
		if (method == MethodName.CoinCreate)
		{
			return true;
		}
		if (method == MethodName.AddSun)
		{
			return true;
		}
		if (method == MethodName.UseSun)
		{
			return true;
		}
		if (method == MethodName.SetSun)
		{
			return true;
		}
		if (method == MethodName.GetSun)
		{
			return true;
		}
		if (method == MethodName.GetLevelChapterFinishNum)
		{
			return true;
		}
		if (method == MethodName.GetLevelEvent)
		{
			return true;
		}
		if (method == MethodName.SetNextLevel)
		{
			return true;
		}
		if (method == MethodName.LoadNextLevelConfig)
		{
			return true;
		}
		if (method == MethodName.CanOpenNextLevel)
		{
			return true;
		}
		if (method == MethodName.SaveNextLevelProgress)
		{
			return true;
		}
		if (method == MethodName.GetLevelControl)
		{
			return true;
		}
		if (method == MethodName.TipsPlay)
		{
			return true;
		}
		if (method == MethodName.GetLevelHomeworld)
		{
			return true;
		}
		if (method == MethodName.MagicSunCreate)
		{
			return true;
		}
		if (method == MethodName.GetMapAttackDpsLifestealRatio)
		{
			return true;
		}
		if (method == MethodName.ApplyMapAttackDpsLifesteal)
		{
			return true;
		}
		if (method == MethodName.ApplyMapCharacterRules)
		{
			return true;
		}
		if (method == MethodName.GetMapZombieColumnSpeedMultiplier)
		{
			return true;
		}
		if (method == MethodName.GetCurrentMap)
		{
			return true;
		}
		if (method == MethodName.GetMapConfig)
		{
			return true;
		}
		if (method == MethodName.MapChange)
		{
			return true;
		}
		if (method == MethodName.GetMapIsNight)
		{
			return true;
		}
		if (method == MethodName.MapDayNightSwitch)
		{
			return true;
		}
		if (method == MethodName.GetMapGridNum)
		{
			return true;
		}
		if (method == MethodName.GetMapGridSize)
		{
			return true;
		}
		if (method == MethodName.GetMapGridBeginPos)
		{
			return true;
		}
		if (method == MethodName.GetMapPlantOffset)
		{
			return true;
		}
		if (method == MethodName.GetMapGridPos)
		{
			return true;
		}
		if (method == MethodName.GetMapCellPos)
		{
			return true;
		}
		if (method == MethodName.GetMapCellPosCenter)
		{
			return true;
		}
		if (method == MethodName.CheckMapGridPosIn)
		{
			return true;
		}
		if (method == MethodName.GetMapCell)
		{
			return true;
		}
		if (method == MethodName.SetMapGridType)
		{
			return true;
		}
		if (method == MethodName.SetMapLineUse)
		{
			return true;
		}
		if (method == MethodName.GetMapLineUse)
		{
			return true;
		}
		if (method == MethodName.GetMapGroundLeft)
		{
			return true;
		}
		if (method == MethodName.GetMapGroundRight)
		{
			return true;
		}
		if (method == MethodName.GetMapGroundUp)
		{
			return true;
		}
		if (method == MethodName.GetMapGroundDown)
		{
			return true;
		}
		if (method == MethodName.GetMapGridPosFromMouse)
		{
			return true;
		}
		if (method == MethodName.GetMapControl)
		{
			return true;
		}
		if (method == MethodName.ReleaseBattleReferences)
		{
			return true;
		}
		if (method == MethodName.GetMapFeature)
		{
			return true;
		}
		if (method == MethodName.ApplyMapPacketCooldownRules)
		{
			return true;
		}
		if (method == MethodName.MapIgnoresDynamicPacketCostGrowth)
		{
			return true;
		}
		if (method == MethodName.GetMapCellPlantPos)
		{
			return true;
		}
		if (method == MethodName.GetMapLineY)
		{
			return true;
		}
		if (method == MethodName.GetCurrentMapConfig)
		{
			return true;
		}
		if (method == MethodName.GetMapCurrentMap)
		{
			return true;
		}
		if (method == MethodName.GetPacketPickControl)
		{
			return true;
		}
		if (method == MethodName.MapLineHasType)
		{
			return true;
		}
		if (method == MethodName.GetMapPlantGrid)
		{
			return true;
		}
		if (method == MethodName.GetMapLineUseArr)
		{
			return true;
		}
		if (method == MethodName.GetMapIceCapList)
		{
			return true;
		}
		if (method == MethodName.GetGroundRect)
		{
			return true;
		}
		if (method == MethodName.SetIceCapPos)
		{
			return true;
		}
		if (method == MethodName.MapHasSpecialRule)
		{
			return true;
		}
		if (method == MethodName.MapSpecialRulesPreventSleep)
		{
			return true;
		}
		if (method == MethodName.GetActiveMapConfig)
		{
			return true;
		}
		if (method == MethodName.SelectNextModLevel)
		{
			return true;
		}
		if (method == MethodName.GetMowerConfig)
		{
			return true;
		}
		if (method == MethodName.GetMowerList)
		{
			return true;
		}
		if (method == MethodName.GetMowerNum)
		{
			return true;
		}
		if (method == MethodName.GetMower)
		{
			return true;
		}
		if (method == MethodName.CreateMower)
		{
			return true;
		}
		if (method == MethodName.GetMowerFeature)
		{
			return true;
		}
		if (method == MethodName.GetMowerManager)
		{
			return true;
		}
		if (method == MethodName.GetNpcTalk)
		{
			return true;
		}
		if (method == MethodName.GetPacketBank)
		{
			return true;
		}
		if (method == MethodName.GetPacketBankFeature)
		{
			return true;
		}
		if (method == MethodName.GetPacketBankData)
		{
			return true;
		}
		if (method == MethodName.GetPacketConfigReadOnlyByCharacterName)
		{
			return true;
		}
		if (method == MethodName.GetPacketConfigCostLower)
		{
			return true;
		}
		if (method == MethodName.GetPacketConfigCostLowerWithTypeList)
		{
			return true;
		}
		if (method == MethodName.GetPacketConfigCostUpper)
		{
			return true;
		}
		if (method == MethodName.GetPacketConfigCostUpperWithTypeList)
		{
			return true;
		}
		if (method == MethodName.GetPacketConfig)
		{
			return true;
		}
		if (method == MethodName.GetPacketConfigReadOnly)
		{
			return true;
		}
		if (method == MethodName.CreatePacketShow)
		{
			return true;
		}
		if (method == MethodName.CreatePacketShowWithConfig)
		{
			return true;
		}
		if (method == MethodName.BindSpawnedPacketPicker)
		{
			return true;
		}
		if (method == MethodName.PublishSpawnedPacket)
		{
			return true;
		}
		if (method == MethodName.GetPortalFeature)
		{
			return true;
		}
		if (method == MethodName.PortalCreate)
		{
			return true;
		}
		if (method == MethodName.ProtalCreate)
		{
			return true;
		}
		if (method == MethodName.PortalChangePos)
		{
			return true;
		}
		if (method == MethodName.ProtalChangePos)
		{
			return true;
		}
		if (method == MethodName.GetProjectileConfig)
		{
			return true;
		}
		if (method == MethodName.QXSunCreate)
		{
			return true;
		}
		if (method == MethodName._GetCleanCharacters)
		{
			return true;
		}
		if (method == MethodName._GetNodesInGroupCached)
		{
			return true;
		}
		if (method == MethodName.GetSeedBankFeature)
		{
			return true;
		}
		if (method == MethodName.GetCurrentPacketBankMethod)
		{
			return true;
		}
		if (method == MethodName.GetSeedBank)
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
		if (method == MethodName.GetPacketSlotNum)
		{
			return true;
		}
		if (method == MethodName.AddPacket)
		{
			return true;
		}
		if (method == MethodName.GetSeedBankList)
		{
			return true;
		}
		if (method == MethodName.GetShovel)
		{
			return true;
		}
		if (method == MethodName.GetShovelList)
		{
			return true;
		}
		if (method == MethodName.YBCreate)
		{
			return true;
		}
		if (method == MethodName.PublishSpawnedCharacter)
		{
			return true;
		}
		if (method == MethodName.BroadcastSpawnedCharacter)
		{
			return true;
		}
		if (method == MethodName.CanContinueSpawnedCharacterPublish)
		{
			return true;
		}
		if (method == MethodName.PublishSpawnedCharacterWhenReady)
		{
			return true;
		}
		if (method == MethodName.DeferSpawnedCharacterPublish)
		{
			return true;
		}
		if (method == MethodName.LuckyBagCreate)
		{
			return true;
		}
		if (method == MethodName.GoldShardCreate)
		{
			return true;
		}
		if (method == MethodName.JalapenoSunCreate)
		{
			return true;
		}
		if (method == MethodName._GetCharacterNode)
		{
			return true;
		}
		if (method == MethodName._IsGameRunning)
		{
			return true;
		}
		if (method == MethodName.GetSunFeature)
		{
			return true;
		}
		if (method == MethodName.GetTutorial)
		{
			return true;
		}
		if (method == MethodName.PickRandomZomie)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._groupCacheFrame)
		{
			_groupCacheFrame = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._characterCountFrame)
		{
			_characterCountFrame = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._characterCountQueryRevision)
		{
			_characterCountQueryRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.eventBus)
		{
			eventBus = VariantUtils.ConvertTo<BattleEventBus>(in value);
			return true;
		}
		if (name == PropertyName.characterRegistry)
		{
			characterRegistry = VariantUtils.ConvertTo<TowerDefenseBattleCharacterRegistry>(in value);
			return true;
		}
		if (name == PropertyName.targetSystem)
		{
			targetSystem = VariantUtils.ConvertTo<TargetSystem>(in value);
			return true;
		}
		if (name == PropertyName.damagePipeline)
		{
			damagePipeline = VariantUtils.ConvertTo<DamagePipeline>(in value);
			return true;
		}
		if (name == PropertyName._coinBank)
		{
			_coinBank = VariantUtils.ConvertTo<CoinBank>(in value);
			return true;
		}
		if (name == PropertyName.currentControl)
		{
			currentControl = VariantUtils.ConvertTo<TowerDefenseControlNew>(in value);
			return true;
		}
		if (name == PropertyName.currentLevelConfig)
		{
			currentLevelConfig = VariantUtils.ConvertTo<TowerDefenseLevelBaseConfig>(in value);
			return true;
		}
		if (name == PropertyName.currentDynamicLevel)
		{
			currentDynamicLevel = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.seedbankPacketMax)
		{
			seedbankPacketMax = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.runGameTime)
		{
			runGameTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.gridSize)
		{
			gridSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.gridBeginPos)
		{
			gridBeginPos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.gridNum)
		{
			gridNum = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.pausePacket)
		{
			pausePacket = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.pauseZombie)
		{
			pauseZombie = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.backPacket)
		{
			backPacket = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.backZombie)
		{
			backZombie = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.coinBank)
		{
			value = VariantUtils.CreateFrom<CoinBank>(coinBank);
			return true;
		}
		if (name == PropertyName._groupCacheFrame)
		{
			value = VariantUtils.CreateFrom(in _groupCacheFrame);
			return true;
		}
		if (name == PropertyName._characterCountFrame)
		{
			value = VariantUtils.CreateFrom(in _characterCountFrame);
			return true;
		}
		if (name == PropertyName._characterCountQueryRevision)
		{
			value = VariantUtils.CreateFrom(in _characterCountQueryRevision);
			return true;
		}
		if (name == PropertyName.eventBus)
		{
			value = VariantUtils.CreateFrom(in eventBus);
			return true;
		}
		if (name == PropertyName.characterRegistry)
		{
			value = VariantUtils.CreateFrom(in characterRegistry);
			return true;
		}
		if (name == PropertyName.targetSystem)
		{
			value = VariantUtils.CreateFrom(in targetSystem);
			return true;
		}
		if (name == PropertyName.damagePipeline)
		{
			value = VariantUtils.CreateFrom(in damagePipeline);
			return true;
		}
		if (name == PropertyName._coinBank)
		{
			value = VariantUtils.CreateFrom(in _coinBank);
			return true;
		}
		if (name == PropertyName.currentControl)
		{
			value = VariantUtils.CreateFrom(in currentControl);
			return true;
		}
		if (name == PropertyName.currentLevelConfig)
		{
			value = VariantUtils.CreateFrom(in currentLevelConfig);
			return true;
		}
		if (name == PropertyName.currentDynamicLevel)
		{
			value = VariantUtils.CreateFrom(in currentDynamicLevel);
			return true;
		}
		if (name == PropertyName.seedbankPacketMax)
		{
			value = VariantUtils.CreateFrom(in seedbankPacketMax);
			return true;
		}
		if (name == PropertyName.runGameTime)
		{
			value = VariantUtils.CreateFrom(in runGameTime);
			return true;
		}
		if (name == PropertyName.gridSize)
		{
			value = VariantUtils.CreateFrom(in gridSize);
			return true;
		}
		if (name == PropertyName.gridBeginPos)
		{
			value = VariantUtils.CreateFrom(in gridBeginPos);
			return true;
		}
		if (name == PropertyName.gridNum)
		{
			value = VariantUtils.CreateFrom(in gridNum);
			return true;
		}
		if (name == PropertyName.pausePacket)
		{
			value = VariantUtils.CreateFrom(in pausePacket);
			return true;
		}
		if (name == PropertyName.pauseZombie)
		{
			value = VariantUtils.CreateFrom(in pauseZombie);
			return true;
		}
		if (name == PropertyName.backPacket)
		{
			value = VariantUtils.CreateFrom(in backPacket);
			return true;
		}
		if (name == PropertyName.backZombie)
		{
			value = VariantUtils.CreateFrom(in backZombie);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._groupCacheFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._characterCountFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._characterCountQueryRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.eventBus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterRegistry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.targetSystem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.damagePipeline, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._coinBank, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.coinBank, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.currentControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.currentLevelConfig, PropertyHint.ResourceType, "TowerDefenseLevelBaseConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentDynamicLevel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.seedbankPacketMax, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.runGameTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.gridSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.gridBeginPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pausePacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pauseZombie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.backPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.backZombie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._groupCacheFrame, Variant.From(in _groupCacheFrame));
		info.AddProperty(PropertyName._characterCountFrame, Variant.From(in _characterCountFrame));
		info.AddProperty(PropertyName._characterCountQueryRevision, Variant.From(in _characterCountQueryRevision));
		info.AddProperty(PropertyName.eventBus, Variant.From(in eventBus));
		info.AddProperty(PropertyName.characterRegistry, Variant.From(in characterRegistry));
		info.AddProperty(PropertyName.targetSystem, Variant.From(in targetSystem));
		info.AddProperty(PropertyName.damagePipeline, Variant.From(in damagePipeline));
		info.AddProperty(PropertyName._coinBank, Variant.From(in _coinBank));
		info.AddProperty(PropertyName.currentControl, Variant.From(in currentControl));
		info.AddProperty(PropertyName.currentLevelConfig, Variant.From(in currentLevelConfig));
		info.AddProperty(PropertyName.currentDynamicLevel, Variant.From(in currentDynamicLevel));
		info.AddProperty(PropertyName.seedbankPacketMax, Variant.From(in seedbankPacketMax));
		info.AddProperty(PropertyName.runGameTime, Variant.From(in runGameTime));
		info.AddProperty(PropertyName.gridSize, Variant.From(in gridSize));
		info.AddProperty(PropertyName.gridBeginPos, Variant.From(in gridBeginPos));
		info.AddProperty(PropertyName.gridNum, Variant.From(in gridNum));
		info.AddProperty(PropertyName.pausePacket, Variant.From(in pausePacket));
		info.AddProperty(PropertyName.pauseZombie, Variant.From(in pauseZombie));
		info.AddProperty(PropertyName.backPacket, Variant.From(in backPacket));
		info.AddProperty(PropertyName.backZombie, Variant.From(in backZombie));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._groupCacheFrame, out var value))
		{
			_groupCacheFrame = value.As<long>();
		}
		if (info.TryGetProperty(PropertyName._characterCountFrame, out var value2))
		{
			_characterCountFrame = value2.As<long>();
		}
		if (info.TryGetProperty(PropertyName._characterCountQueryRevision, out var value3))
		{
			_characterCountQueryRevision = value3.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.eventBus, out var value4))
		{
			eventBus = value4.As<BattleEventBus>();
		}
		if (info.TryGetProperty(PropertyName.characterRegistry, out var value5))
		{
			characterRegistry = value5.As<TowerDefenseBattleCharacterRegistry>();
		}
		if (info.TryGetProperty(PropertyName.targetSystem, out var value6))
		{
			targetSystem = value6.As<TargetSystem>();
		}
		if (info.TryGetProperty(PropertyName.damagePipeline, out var value7))
		{
			damagePipeline = value7.As<DamagePipeline>();
		}
		if (info.TryGetProperty(PropertyName._coinBank, out var value8))
		{
			_coinBank = value8.As<CoinBank>();
		}
		if (info.TryGetProperty(PropertyName.currentControl, out var value9))
		{
			currentControl = value9.As<TowerDefenseControlNew>();
		}
		if (info.TryGetProperty(PropertyName.currentLevelConfig, out var value10))
		{
			currentLevelConfig = value10.As<TowerDefenseLevelBaseConfig>();
		}
		if (info.TryGetProperty(PropertyName.currentDynamicLevel, out var value11))
		{
			currentDynamicLevel = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName.seedbankPacketMax, out var value12))
		{
			seedbankPacketMax = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName.runGameTime, out var value13))
		{
			runGameTime = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.gridSize, out var value14))
		{
			gridSize = value14.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.gridBeginPos, out var value15))
		{
			gridBeginPos = value15.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.gridNum, out var value16))
		{
			gridNum = value16.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.pausePacket, out var value17))
		{
			pausePacket = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pauseZombie, out var value18))
		{
			pauseZombie = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.backPacket, out var value19))
		{
			backPacket = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.backZombie, out var value20))
		{
			backZombie = value20.As<bool>();
		}
	}
}
