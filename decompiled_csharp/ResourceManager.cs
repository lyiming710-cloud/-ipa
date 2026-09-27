using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using Microsoft.Extensions.Logging;
using ZLogger;

[GlobalClass]
[ScriptPath("res://Core/ResourceManager/ResourceManager.cs")]
public class ResourceManager : Node2D
{
	private sealed class FullGameplayCoreSnapshot
	{
		public readonly System.Collections.Generic.Dictionary<string, Resource> Survivals = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, Dictionary> Levels = new System.Collections.Generic.Dictionary<string, Dictionary>();

		public readonly System.Collections.Generic.Dictionary<string, Resource> Maps = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, Resource> Bgms = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, string> BgmPaths = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		public readonly System.Collections.Generic.Dictionary<string, Resource> ProjectileConfigs = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, Resource> Audios = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, Resource> Talks = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, Resource> Tutorials = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, Resource> Collectables = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, Resource> Shovels = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, Resource> Mowers = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, Resource> Shops = new System.Collections.Generic.Dictionary<string, Resource>();

		public readonly System.Collections.Generic.Dictionary<string, Variant> DailyLevelData = new System.Collections.Generic.Dictionary<string, Variant>();

		public readonly System.Collections.Generic.Dictionary<string, Resource> TowerDefenseLevelEvents = new System.Collections.Generic.Dictionary<string, Resource>();

		public FullGameplayRegistryRoots RegistryRoots;
	}

	private sealed class FullGameplayRootSnapshot
	{
		public readonly System.Collections.Generic.Dictionary<string, Resource> CharacterScenes;

		public readonly System.Collections.Generic.Dictionary<string, Resource> Sprites;

		public readonly System.Collections.Generic.Dictionary<string, Resource> Packets;

		public readonly System.Collections.Generic.Dictionary<string, TowerDefensePacketBankData> PacketBanks;

		public FullGameplayRootSnapshot(System.Collections.Generic.Dictionary<string, Resource> characterScenes, System.Collections.Generic.Dictionary<string, Resource> sprites, System.Collections.Generic.Dictionary<string, Resource> packets, System.Collections.Generic.Dictionary<string, TowerDefensePacketBankData> packetBanks)
		{
			CharacterScenes = characterScenes;
			Sprites = sprites;
			Packets = packets;
			PacketBanks = packetBanks;
		}
	}

	public enum GameplayAtlasLoadState
	{
		NotStarted,
		LoadingVisual,
		LoadingPose,
		Ready,
		Failed
	}

	public delegate void LoadOverEventHandler();

	public delegate void LoadFailedEventHandler(string error);

	public delegate void LoadPercentageEventHandler(double persontage, string stepName, string resourceName, int resourceIndex, int resourceTotal);

	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName BeginLoad = "BeginLoad";

		public static readonly StringName RequireFullGameplayResourcesReady = "RequireFullGameplayResourcesReady";

		public static readonly StringName WarmConfigResourcesOnMainThread = "WarmConfigResourcesOnMainThread";

		public static readonly StringName InitializeRegistriesOnMainThread = "InitializeRegistriesOnMainThread";

		public static readonly StringName MergePacketBankCategories = "MergePacketBankCategories";

		public static readonly StringName RequireJsonDictionary = "RequireJsonDictionary";

		public static readonly StringName NotifyFullGameplayLoadReady = "NotifyFullGameplayLoadReady";

		public static readonly StringName NotifyLoadOverSubscribers = "NotifyLoadOverSubscribers";

		public static readonly StringName NotifyLoadFailedSubscribers = "NotifyLoadFailedSubscribers";

		public static readonly StringName RequireMainThread = "RequireMainThread";

		public static readonly StringName EmitLoadPercentageForCurrentStep = "EmitLoadPercentageForCurrentStep";

		public static readonly StringName EmitLoadResourceProgress = "EmitLoadResourceProgress";

		public static readonly StringName EmitLoadPercentage = "EmitLoadPercentage";

		public static readonly StringName EnsureAllBgmsLoaded = "EnsureAllBgmsLoaded";

		public static readonly StringName PreloadAdobeAnimateGlobalAtlases = "PreloadAdobeAnimateGlobalAtlases";

		public static readonly StringName BeginGameplayAtlasPreload = "BeginGameplayAtlasPreload";

		public static readonly StringName TryRequestGameplayAtlas = "TryRequestGameplayAtlas";

		public static readonly StringName PumpGameplayAtlasPreload = "PumpGameplayAtlasPreload";

		public static readonly StringName PumpGameplayVisualAtlas = "PumpGameplayVisualAtlas";

		public static readonly StringName PumpGameplayPoseAtlas = "PumpGameplayPoseAtlas";

		public static readonly StringName FailGameplayAtlasLoad = "FailGameplayAtlasLoad";

		public static readonly StringName EnsureAdobeAnimateVisualTextureArrayLoaded = "EnsureAdobeAnimateVisualTextureArrayLoaded";

		public static readonly StringName EnsureAdobeAnimateGpuPoseTextureArrayLoaded = "EnsureAdobeAnimateGpuPoseTextureArrayLoaded";

		public static readonly StringName GetCharacterSprite = "GetCharacterSprite";

		public static readonly StringName GetCharacterScene = "GetCharacterScene";

		public static readonly StringName GetPacket = "GetPacket";

		public static readonly StringName GetRandomSpriteName = "GetRandomSpriteName";

		public static readonly StringName ReleaseTransientResources = "ReleaseTransientResources";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName FullGameplayLoadStartedUsec = "FullGameplayLoadStartedUsec";

		public static readonly StringName FullGameplayResourcesReadyUsec = "FullGameplayResourcesReadyUsec";

		public static readonly StringName stepMax = "stepMax";

		public static readonly StringName stepName = "stepName";

		public static readonly StringName currentStep = "currentStep";

		public static readonly StringName CurrentGameplayAtlasLoadState = "CurrentGameplayAtlasLoadState";

		public static readonly StringName AreGameplayAtlasesReady = "AreGameplayAtlasesReady";

		public static readonly StringName HasGameplayAtlasLoadFailed = "HasGameplayAtlasLoadFailed";

		public static readonly StringName GameplayAtlasLoadError = "GameplayAtlasLoadError";

		public static readonly StringName CurrentGameplayResourceLoadState = "CurrentGameplayResourceLoadState";

		public static readonly StringName AreFullGameplayResourcesReady = "AreFullGameplayResourcesReady";

		public static readonly StringName FullGameplayResourceLoadError = "FullGameplayResourceLoadError";

		public static readonly StringName LateCharacterResourceLoadCount = "LateCharacterResourceLoadCount";

		public static readonly StringName _adobeAnimateVisualTextureArray = "_adobeAnimateVisualTextureArray";

		public static readonly StringName _adobeAnimateGpuPoseTextureArray = "_adobeAnimateGpuPoseTextureArray";

		public static readonly StringName _gameplayAtlasLoadState = "_gameplayAtlasLoadState";

		public static readonly StringName _gameplayResourceLoadState = "_gameplayResourceLoadState";

		public static readonly StringName _gameplayAtlasLoadError = "_gameplayAtlasLoadError";

		public static readonly StringName _fullGameplayResourceLoadError = "_fullGameplayResourceLoadError";

		public static readonly StringName _gameplayAtlasLoadStartedUsec = "_gameplayAtlasLoadStartedUsec";

		public static readonly StringName _gameplayAtlasVisualReadyUsec = "_gameplayAtlasVisualReadyUsec";

		public static readonly StringName _gameplayAtlasPoseReadyUsec = "_gameplayAtlasPoseReadyUsec";

		public static readonly StringName _fullLoadStartedUsec = "_fullLoadStartedUsec";

		public static readonly StringName _loadStarted = "_loadStarted";

		public static readonly StringName _bgmResourcesLoaded = "_bgmResourcesLoaded";

		public static readonly StringName _mainThreadId = "_mainThreadId";

		public static readonly StringName _lateCharacterResourceLoadCount = "_lateCharacterResourceLoadCount";

		public static readonly StringName _packetBankMissingCount = "_packetBankMissingCount";

		public static readonly StringName _coreMilliseconds = "_coreMilliseconds";

		public static readonly StringName _characterSceneMilliseconds = "_characterSceneMilliseconds";

		public static readonly StringName _spriteMilliseconds = "_spriteMilliseconds";

		public static readonly StringName _packetMilliseconds = "_packetMilliseconds";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static readonly ILogger _logger = Log.CreateLogger<ResourceManager>();

	private static readonly Random _random = new Random();

	private static Json _survivalResource;

	private static Json _levelResource;

	private static Json _mapResource;

	private static Json _bgmResource;

	private static Json _projectileResource;

	private static Json _audioResource;

	private static Json _characterResource;

	private static Json _fullGameplayResourceManifest;

	private static Json _talkResource;

	private static Json _tutorialResource;

	private static Json _packetBankResource;

	private static Json _collectableResource;

	private static Json _shovelResource;

	private static Json _mowerResource;

	private static Json _shopResource;

	private static Json _dailyLevelAward;

	private System.Collections.Generic.Dictionary<string, string> _bgmPaths = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private readonly List<FullGameplayRootLoadMetric> _rootLoadMetrics = new List<FullGameplayRootLoadMetric>();

	private readonly TaskCompletionSource<bool> _fullReadyCompletion = CreateReadyCompletion();

	private System.Collections.Generic.Dictionary<string, string> _characterSpritePaths = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private System.Collections.Generic.Dictionary<string, string> _characterScenePaths = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private System.Collections.Generic.Dictionary<string, string> _packetPaths = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private System.Collections.Generic.Dictionary<string, string> _characterNameByPacket = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private TextureLayered _adobeAnimateVisualTextureArray;

	private TextureLayered _adobeAnimateGpuPoseTextureArray;

	private GameplayAtlasLoadState _gameplayAtlasLoadState;

	private GameplayResourceLoadState _gameplayResourceLoadState;

	private string _gameplayAtlasLoadError = string.Empty;

	private string _fullGameplayResourceLoadError = string.Empty;

	private ulong _gameplayAtlasLoadStartedUsec;

	private ulong _gameplayAtlasVisualReadyUsec;

	private ulong _gameplayAtlasPoseReadyUsec;

	private ulong _fullLoadStartedUsec;

	private bool _loadStarted;

	private bool _bgmResourcesLoaded;

	private int _mainThreadId;

	private int _lateCharacterResourceLoadCount;

	private int _packetBankMissingCount;

	private double _coreMilliseconds;

	private double _characterSceneMilliseconds;

	private double _spriteMilliseconds;

	private double _packetMilliseconds;

	private static Json SURVIVAL_RESOURCE => _survivalResource ?? (_survivalResource = GD.Load<Json>("res://Asset/Config/Survival/SurvivalResource.json"));

	private static Json MAP_RESOURCE => _mapResource ?? (_mapResource = GD.Load<Json>("res://Asset/Config/Map/MapResource.json"));

	private static Json BGM_RESOURCE => _bgmResource ?? (_bgmResource = GD.Load<Json>("res://Asset/Config/BGM/BGMResource.json"));

	private static Json PROJECTILE_RESOURCE => _projectileResource ?? (_projectileResource = GD.Load<Json>("res://Asset/Config/Projectile/ProjectileResource.json"));

	private static Json AUDIO_RESOURCE => _audioResource ?? (_audioResource = GD.Load<Json>("res://Asset/Config/Audio/AudioResource.json"));

	private static Json CHARACTER_RESOURCE => _characterResource ?? (_characterResource = GD.Load<Json>("res://Asset/Config/Character/CharacterResource.json"));

	private static Json FULL_GAMEPLAY_RESOURCE_MANIFEST => _fullGameplayResourceManifest ?? (_fullGameplayResourceManifest = GD.Load<Json>("res://Asset/Config/Character/FullGameplayResources.json"));

	private static Json TALK_RESOURCE => _talkResource ?? (_talkResource = GD.Load<Json>("res://Asset/Config/Npc/TalkResource.json"));

	private static Json TUTORIAL_RESOURCE => _tutorialResource ?? (_tutorialResource = GD.Load<Json>("res://Asset/Config/Tutorial/TutorialResource.json"));

	private static Json PACKET_BANK_RESOURCE => _packetBankResource ?? (_packetBankResource = GD.Load<Json>("res://Asset/Config/PacketBank/PacketBankResource.json"));

	private static Json COLLECTABLE_RESOURCE => _collectableResource ?? (_collectableResource = GD.Load<Json>("res://Asset/Config/Collectable/CollectableResource.json"));

	private static Json SHOVEL_RESOURCE => _shovelResource ?? (_shovelResource = GD.Load<Json>("res://Asset/Config/Shovel/ShovelResource.json"));

	private static Json MOWER_RESOURCE => _mowerResource ?? (_mowerResource = GD.Load<Json>("res://Asset/Config/Mower/MowerResource.json"));

	private static Json SHOP_RESOURCE => _shopResource ?? (_shopResource = GD.Load<Json>("res://Asset/Config/Shop/ShopResource.json"));

	public ulong FullGameplayLoadStartedUsec => _fullLoadStartedUsec;

	public ulong FullGameplayResourcesReadyUsec { get; private set; }

	public System.Collections.Generic.Dictionary<string, Resource> SURVIVALS { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public static Json LEVEL_RESOURCE => _levelResource ?? (_levelResource = GD.Load<Json>("res://Asset/Config/Level/LevelResource.json"));

	public System.Collections.Generic.Dictionary<string, Dictionary> LEVELS { get; private set; } = new System.Collections.Generic.Dictionary<string, Dictionary>();

	public System.Collections.Generic.Dictionary<string, Resource> MAPS { get; set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public System.Collections.Generic.Dictionary<string, Resource> BGMS { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public System.Collections.Generic.Dictionary<string, Resource> PROJECTILE_CONFIG { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public System.Collections.Generic.Dictionary<string, Resource> AUDIOS { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public System.Collections.Generic.Dictionary<string, Resource> CHARCTAER_SPRITE { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>(StringComparer.OrdinalIgnoreCase);

	public System.Collections.Generic.Dictionary<string, Resource> TOWERDEFENSE_CHARCATERS { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>(StringComparer.OrdinalIgnoreCase);

	public System.Collections.Generic.Dictionary<string, Resource> TOWERDEFENSE_PACKETS { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>(StringComparer.OrdinalIgnoreCase);

	public System.Collections.Generic.Dictionary<string, Resource> TALKS { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public System.Collections.Generic.Dictionary<string, Resource> TUTORIALS { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public System.Collections.Generic.Dictionary<string, TowerDefensePacketBankData> TOWERDEFENSE_PACKETBANKS { get; private set; } = new System.Collections.Generic.Dictionary<string, TowerDefensePacketBankData>(StringComparer.OrdinalIgnoreCase);

	public System.Collections.Generic.Dictionary<string, Resource> COLLECTABLES { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public System.Collections.Generic.Dictionary<string, Resource> SHOVELS { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public System.Collections.Generic.Dictionary<string, Resource> MOWERS { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public System.Collections.Generic.Dictionary<string, Resource> SHOPS { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public System.Collections.Generic.Dictionary<string, Variant> DAILY_LEVEL_DATA { get; private set; } = new System.Collections.Generic.Dictionary<string, Variant>();

	public System.Collections.Generic.Dictionary<string, Resource> TOWERDEFENSE_LEVEL_EVENT { get; private set; } = new System.Collections.Generic.Dictionary<string, Resource>();

	public static Json DAILY_LEVEL_AWARD => _dailyLevelAward ?? (_dailyLevelAward = GD.Load<Json>("res://Asset/Config/DailyChallenge/DailyChallengeAward.json"));

	public static ResourceManager Instance { get; private set; }

	public int stepMax { get; private set; } = 18;

	public string stepName { get; private set; } = "LOAD_STARTUP";

	public int currentStep { get; private set; }

	public GameplayAtlasLoadState CurrentGameplayAtlasLoadState => _gameplayAtlasLoadState;

	public bool AreGameplayAtlasesReady
	{
		get
		{
			if (_gameplayAtlasLoadState == GameplayAtlasLoadState.Ready && GodotObject.IsInstanceValid(_adobeAnimateVisualTextureArray))
			{
				return GodotObject.IsInstanceValid(_adobeAnimateGpuPoseTextureArray);
			}
			return false;
		}
	}

	public bool HasGameplayAtlasLoadFailed => _gameplayAtlasLoadState == GameplayAtlasLoadState.Failed;

	public string GameplayAtlasLoadError => _gameplayAtlasLoadError;

	public GameplayResourceLoadState CurrentGameplayResourceLoadState => _gameplayResourceLoadState;

	public bool AreFullGameplayResourcesReady => _gameplayResourceLoadState == GameplayResourceLoadState.Ready;

	public string FullGameplayResourceLoadError => _fullGameplayResourceLoadError;

	public int LateCharacterResourceLoadCount => Volatile.Read(in _lateCharacterResourceLoadCount);

	public FullGameplayResourceLoadMetricsSnapshot FullGameplayResourceLoadMetrics { get; private set; } = FullGameplayResourceLoadMetricsSnapshot.CreateEmpty();

	public event LoadOverEventHandler OnLoadOver;

	public event LoadFailedEventHandler OnLoadFailed;

	public event LoadPercentageEventHandler OnLoadPercentage;

	public override void _Ready()
	{
		Instance = this;
		_mainThreadId = System.Environment.CurrentManagedThreadId;
	}

	public override void _Process(double delta)
	{
		PumpGameplayAtlasPreload();
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public void BeginLoad()
	{
		RequireMainThread("BeginLoad");
		if (_gameplayResourceLoadState == GameplayResourceLoadState.Ready)
		{
			NotifyLoadOverSubscribers();
		}
		else if (_gameplayResourceLoadState == GameplayResourceLoadState.Failed)
		{
			NotifyLoadFailedSubscribers(_fullGameplayResourceLoadError);
		}
		else if (!_loadStarted)
		{
			_loadStarted = true;
			_fullLoadStartedUsec = Time.GetTicksUsec();
			_gameplayResourceLoadState = GameplayResourceLoadState.LoadingCore;
			stepName = "LOAD_CONFIG";
			EmitLoadPercentage(0.0, stepName);
			LoadFullGameplayResourcesAsync();
		}
	}

	public async Task EnsureFullGameplayResourcesReadyAsync()
	{
		RequireMainThread("EnsureFullGameplayResourcesReadyAsync");
		if (_gameplayResourceLoadState == GameplayResourceLoadState.NotStarted)
		{
			BeginLoad();
		}
		await _fullReadyCompletion.Task;
	}

	public void RequireFullGameplayResourcesReady(string context)
	{
		if (_gameplayResourceLoadState == GameplayResourceLoadState.Ready)
		{
			return;
		}
		string text = ((_gameplayResourceLoadState == GameplayResourceLoadState.Failed) ? _fullGameplayResourceLoadError : $"state={_gameplayResourceLoadState}");
		throw new InvalidOperationException("Full gameplay resources are required by " + context + ": " + text);
	}

	private async Task LoadFullGameplayResourcesAsync()
	{
		_ = 2;
		try
		{
			BeginGameplayAtlasPreload();
			ulong ticksUsec = Time.GetTicksUsec();
			StartupLoadDiagnostics.Mark("core.config.begin");
			WarmConfigResourcesOnMainThread();
			StartupLoadDiagnostics.Mark("core.config.end");
			StartupLoadDiagnostics.Mark("core.registries.begin");
			InitializeRegistriesOnMainThread();
			StartupLoadDiagnostics.Mark("core.registries.end");
			StartupLoadDiagnostics.Mark("core.resources.begin");
			FullGameplayCoreSnapshot coreSnapshot = LoadCoreResourcesOnMainThread();
			StartupLoadDiagnostics.Mark("core.resources.end");
			_coreMilliseconds = (double)(Time.GetTicksUsec() - ticksUsec) / 1000.0;
			await EnsureGameplayAtlasesReadyAsync();
			await YieldAfterStageAsync();
			_gameplayResourceLoadState = GameplayResourceLoadState.LoadingGameplay;
			PublishReady(coreSnapshot, await LoadFullGameplayRootsOnMainThreadAsync(coreSnapshot.RegistryRoots));
		}
		catch (Exception exception)
		{
			FailFullGameplayLoad(exception);
		}
	}

	private async Task YieldAfterStageAsync()
	{
		if (IsInsideTree())
		{
			if (string.Equals(DisplayServer.GetName(), "headless", StringComparison.OrdinalIgnoreCase))
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}
			else
			{
				await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
			}
		}
	}

	private static void WarmConfigResourcesOnMainThread()
	{
		_ = SURVIVAL_RESOURCE;
		_ = LEVEL_RESOURCE;
		_ = MAP_RESOURCE;
		_ = BGM_RESOURCE;
		_ = PROJECTILE_RESOURCE;
		_ = AUDIO_RESOURCE;
		_ = CHARACTER_RESOURCE;
		_ = FULL_GAMEPLAY_RESOURCE_MANIFEST;
		_ = TALK_RESOURCE;
		_ = TUTORIAL_RESOURCE;
		_ = PACKET_BANK_RESOURCE;
		_ = COLLECTABLE_RESOURCE;
		_ = SHOVEL_RESOURCE;
		_ = MOWER_RESOURCE;
		_ = SHOP_RESOURCE;
		_ = DAILY_LEVEL_AWARD;
	}

	private static void InitializeRegistriesOnMainThread()
	{
		TowerDefenseProjectileRegistry.Init();
		TowerDefenseArmorRegistry.Init();
		TowerDefenseBattleRegistry.Init();
		CommandRegistry.Init();
	}

	private FullGameplayCoreSnapshot LoadCoreResourcesOnMainThread()
	{
		FullGameplayCoreSnapshot fullGameplayCoreSnapshot = new FullGameplayCoreSnapshot();
		currentStep = 0;
		LoadResourceDictionary(MAP_RESOURCE, fullGameplayCoreSnapshot.Maps, "CoreMap", "LOAD_MAP");
		currentStep = 1;
		IndexBgmPaths(fullGameplayCoreSnapshot.BgmPaths);
		currentStep = 2;
		LoadResourceDictionary(PROJECTILE_RESOURCE, fullGameplayCoreSnapshot.ProjectileConfigs, "CoreProjectile", "LOAD_PROJECTILE");
		currentStep = 3;
		EmitLoadPercentageForCurrentStep("LOAD_ARMOR");
		currentStep = 4;
		LoadResourceDictionary(AUDIO_RESOURCE, fullGameplayCoreSnapshot.Audios, "CoreAudio", "LOAD_AUDIO");
		currentStep = 5;
		EmitLoadPercentageForCurrentStep("LOAD_CHARACTER_INDEX");
		fullGameplayCoreSnapshot.RegistryRoots = FullGameplayResourceManifest.GetRegistryRoots(CHARACTER_RESOURCE);
		currentStep = 6;
		LoadResourceDictionary(TALK_RESOURCE, fullGameplayCoreSnapshot.Talks, "CoreTalk", "LOAD_TALK");
		currentStep = 7;
		LoadResourceDictionary(TUTORIAL_RESOURCE, fullGameplayCoreSnapshot.Tutorials, "CoreTutorial", "LOAD_TUTORIAL");
		currentStep = 8;
		EmitLoadPercentageForCurrentStep("LOAD_PACKETBANK");
		currentStep = 9;
		LoadResourceDictionary(COLLECTABLE_RESOURCE, fullGameplayCoreSnapshot.Collectables, "CoreCollectable", "LOAD_COLLECTABLE");
		currentStep = 10;
		LoadResourceDictionary(SHOVEL_RESOURCE, fullGameplayCoreSnapshot.Shovels, "CoreShovel", "LOAD_SHOVEL");
		currentStep = 11;
		LoadResourceDictionary(MOWER_RESOURCE, fullGameplayCoreSnapshot.Mowers, "CoreMower", "LOAD_MOWER");
		currentStep = 12;
		LoadResourceDictionary(SHOP_RESOURCE, fullGameplayCoreSnapshot.Shops, "CoreShop", "LOAD_SHOP");
		currentStep = 13;
		LoadResourceDictionary(SURVIVAL_RESOURCE, fullGameplayCoreSnapshot.Survivals, "CoreSurvival", "LOAD_LEVEL");
		LoadLevelData(fullGameplayCoreSnapshot.Levels);
		return fullGameplayCoreSnapshot;
	}

	private void IndexBgmPaths(System.Collections.Generic.Dictionary<string, string> destination)
	{
		stepName = "LOAD_BGM";
		EmitLoadPercentageForCurrentStep(stepName);
		destination.Clear();
		Dictionary dictionary = RequireJsonDictionary(BGM_RESOURCE, "BGMResource.json");
		foreach (Variant key in dictionary.Keys)
		{
			destination[key.AsString()] = dictionary[key].AsString();
		}
	}

	private void LoadResourceDictionary(Json registry, System.Collections.Generic.Dictionary<string, Resource> destination, string category, string loadingStep)
	{
		stepName = loadingStep;
		Dictionary dictionary = RequireJsonDictionary(registry, loadingStep);
		destination.Clear();
		int num = 0;
		foreach (Variant key in dictionary.Keys)
		{
			num++;
			string text = key.AsString();
			EmitLoadResourceProgress(currentStep, loadingStep, text, num, dictionary.Count);
			Resource resource = LoadBuiltInRoot(dictionary[key].AsString(), category, text, out var actualLoadMilliseconds);
			ulong ticksUsec = Time.GetTicksUsec();
			destination[text] = resource;
			double publishMilliseconds = (double)(Time.GetTicksUsec() - ticksUsec) / 1000.0;
			_rootLoadMetrics.Add(new FullGameplayRootLoadMetric(category, resource.ResourcePath, actualLoadMilliseconds, publishMilliseconds));
		}
	}

	private void LoadLevelData(System.Collections.Generic.Dictionary<string, Dictionary> destination)
	{
		destination.Clear();
		Dictionary dictionary = RequireJsonDictionary(LEVEL_RESOURCE, "LevelResource.json");
		int num = 0;
		foreach (Variant key in dictionary.Keys)
		{
			num++;
			string text = key.AsString();
			EmitLoadResourceProgress(currentStep, "LOAD_LEVEL", text, num, dictionary.Count);
			destination[text] = dictionary[key].AsGodotDictionary().Duplicate(deep: true);
		}
	}

	private async Task<FullGameplayRootSnapshot> LoadFullGameplayRootsOnMainThreadAsync(FullGameplayRegistryRoots roots)
	{
		FullGameplayResourceManifestData manifest = FullGameplayResourceManifest.ReadAndValidate(FULL_GAMEPLAY_RESOURCE_MANIFEST, CHARACTER_RESOURCE, PACKET_BANK_RESOURCE);
		List<string> list = FullGameplayResourceManifest.ValidatePacketBankClosure(PACKET_BANK_RESOURCE, roots);
		_packetBankMissingCount = list.Count;
		if (list.Count > 0)
		{
			throw new InvalidOperationException("PacketBank closure validation failed: " + string.Join(" | ", list));
		}
		System.Collections.Generic.Dictionary<string, TowerDefensePacketBankData> packetBanks = BuildExpandedPacketBanks();
		currentStep = 14;
		stepName = "LOAD_CHARACTER_SCENE_ROOTS";
		ulong ticksUsec = Time.GetTicksUsec();
		StartupLoadDiagnostics.Mark("characters.begin");
		StartupLoadDiagnostics.MeasureCharacterDependencies(manifest.CharacterSceneRoots);
		System.Collections.Generic.Dictionary<string, Resource> loadedRoots = LoadUniquePackedSceneRoots(manifest.CharacterSceneRoots, "CharacterScene");
		System.Collections.Generic.Dictionary<string, Resource> characterScenes = BindPackedSceneNames(new System.Collections.Generic.Dictionary<string, string>(roots.ScenePathByCharacter, StringComparer.OrdinalIgnoreCase), loadedRoots, "CharacterScene");
		_characterSceneMilliseconds = (double)(Time.GetTicksUsec() - ticksUsec) / 1000.0;
		StartupLoadDiagnostics.Mark("characters.end");
		await YieldAfterStageAsync();
		currentStep = 15;
		stepName = "LOAD_CHARACTER_SPRITE_ROOTS";
		ulong ticksUsec2 = Time.GetTicksUsec();
		System.Collections.Generic.Dictionary<string, Resource> independentRoots = LoadUniquePackedSceneRoots(manifest.IndependentSpriteRoots, "IndependentSprite");
		System.Collections.Generic.Dictionary<string, Resource> sprites = BindSpriteNamesFromCache(new System.Collections.Generic.Dictionary<string, string>(roots.SpritePathByCharacter, StringComparer.OrdinalIgnoreCase), independentRoots);
		_spriteMilliseconds = (double)(Time.GetTicksUsec() - ticksUsec2) / 1000.0;
		await YieldAfterStageAsync();
		currentStep = 16;
		stepName = "LOAD_PACKET_ROOTS";
		ulong ticksUsec3 = Time.GetTicksUsec();
		System.Collections.Generic.Dictionary<string, Resource> loadedRoots2 = LoadUniqueResourceRoots(manifest.PacketRoots, "Packet");
		System.Collections.Generic.Dictionary<string, Resource> packets = BindResourceNames(new System.Collections.Generic.Dictionary<string, string>(roots.PacketPathByName, StringComparer.OrdinalIgnoreCase), loadedRoots2, "Packet");
		_packetMilliseconds = (double)(Time.GetTicksUsec() - ticksUsec3) / 1000.0;
		await YieldAfterStageAsync();
		currentStep = 17;
		stepName = "VALIDATE_FULL_GAMEPLAY_RESOURCES";
		EmitLoadPercentageForCurrentStep(stepName);
		ValidateCompletePublication(roots, manifest, characterScenes, sprites, packets, packetBanks);
		return new FullGameplayRootSnapshot(characterScenes, sprites, packets, packetBanks);
	}

	private System.Collections.Generic.Dictionary<string, Resource> LoadUniquePackedSceneRoots(IReadOnlyList<string> paths, string category)
	{
		System.Collections.Generic.Dictionary<string, Resource> dictionary = LoadUniqueResourceRoots(paths, category);
		foreach (var (value, resource2) in dictionary)
		{
			if (!(resource2 is PackedScene))
			{
				throw new InvalidOperationException($"Full gameplay root has the wrong type: category={category}, path={value}, actual={resource2.GetType().FullName}, expected=PackedScene");
			}
		}
		return dictionary;
	}

	private System.Collections.Generic.Dictionary<string, Resource> LoadUniqueResourceRoots(IReadOnlyList<string> paths, string category)
	{
		System.Collections.Generic.Dictionary<string, Resource> dictionary = new System.Collections.Generic.Dictionary<string, Resource>(StringComparer.OrdinalIgnoreCase);
		for (int i = 0; i < paths.Count; i++)
		{
			string text = paths[i];
			EmitLoadResourceProgress(currentStep, stepName, text, i + 1, paths.Count);
			Resource value = LoadBuiltInRoot(text, category, text, out var actualLoadMilliseconds);
			ulong ticksUsec = Time.GetTicksUsec();
			dictionary[text] = value;
			double publishMilliseconds = (double)(Time.GetTicksUsec() - ticksUsec) / 1000.0;
			_rootLoadMetrics.Add(new FullGameplayRootLoadMetric(category, text, actualLoadMilliseconds, publishMilliseconds));
		}
		return dictionary;
	}

	private static System.Collections.Generic.Dictionary<string, Resource> BindResourceNames(System.Collections.Generic.Dictionary<string, string> configuredPaths, System.Collections.Generic.Dictionary<string, Resource> loadedRoots, string category)
	{
		System.Collections.Generic.Dictionary<string, Resource> dictionary = new System.Collections.Generic.Dictionary<string, Resource>(StringComparer.OrdinalIgnoreCase);
		foreach (var (text3, text4) in configuredPaths)
		{
			if (!loadedRoots.TryGetValue(text4, out var value) || !GodotObject.IsInstanceValid(value))
			{
				throw new InvalidOperationException($"Loaded root cannot satisfy binding: category={category}, key={text3}, path={text4}");
			}
			dictionary[text3] = value;
		}
		return dictionary;
	}

	private static System.Collections.Generic.Dictionary<string, Resource> BindPackedSceneNames(System.Collections.Generic.Dictionary<string, string> configuredPaths, System.Collections.Generic.Dictionary<string, Resource> loadedRoots, string category)
	{
		System.Collections.Generic.Dictionary<string, Resource> dictionary = BindResourceNames(configuredPaths, loadedRoots, category);
		foreach (var (value, resource2) in dictionary)
		{
			if (!(resource2 is PackedScene))
			{
				throw new InvalidOperationException($"PackedScene binding has the wrong type: category={category}, key={value}, actual={resource2.GetType().FullName}");
			}
		}
		return dictionary;
	}

	private System.Collections.Generic.Dictionary<string, Resource> BindSpriteNamesFromCache(System.Collections.Generic.Dictionary<string, string> configuredPaths, System.Collections.Generic.Dictionary<string, Resource> independentRoots)
	{
		System.Collections.Generic.Dictionary<string, Resource> dictionary = new System.Collections.Generic.Dictionary<string, Resource>(StringComparer.OrdinalIgnoreCase);
		System.Collections.Generic.Dictionary<string, Resource> dictionary2 = new System.Collections.Generic.Dictionary<string, Resource>(independentRoots, StringComparer.OrdinalIgnoreCase);
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (var (text3, text4) in configuredPaths)
		{
			bool flag = dictionary2.TryGetValue(text4, out var value);
			if (!flag && ResourceLoader.HasCached(text4))
			{
				value = ResourceLoader.GetCachedRef(text4);
				flag = GodotObject.IsInstanceValid(value);
				if (flag)
				{
					dictionary2[text4] = value;
				}
			}
			if (!flag)
			{
				if (hashSet.Add(text4))
				{
					_rootLoadMetrics.Add(new FullGameplayRootLoadMetric("SpriteCacheBinding", text4, 0.0, 0.0, 0, 1));
				}
				throw new InvalidOperationException("Character Sprite was not brought into the main-thread cache by its declared scene root. The manifest is stale or the dependency graph is broken: key=" + text3 + ", path=" + text4);
			}
			if (!(value is PackedScene))
			{
				throw new InvalidOperationException($"Character Sprite cache binding has the wrong type: key={text3}, path={text4}, actual={value.GetType().FullName}, expected=PackedScene");
			}
			if (hashSet.Add(text4))
			{
				_rootLoadMetrics.Add(new FullGameplayRootLoadMetric("SpriteCacheBinding", text4, 0.0, 0.0, 1));
			}
			dictionary[text3] = value;
		}
		return dictionary;
	}

	private static System.Collections.Generic.Dictionary<string, TowerDefensePacketBankData> BuildExpandedPacketBanks()
	{
		Dictionary dictionary = RequireJsonDictionary(PACKET_BANK_RESOURCE, "PacketBankResource.json");
		System.Collections.Generic.Dictionary<string, TowerDefensePacketBankData> dictionary2 = new System.Collections.Generic.Dictionary<string, TowerDefensePacketBankData>(StringComparer.OrdinalIgnoreCase);
		foreach (Variant key in dictionary.Keys)
		{
			BuildExpandedPacketBank(key.AsString(), dictionary, dictionary2, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
		}
		return dictionary2;
	}

	private static TowerDefensePacketBankData BuildExpandedPacketBank(string bankName, Dictionary rawBanks, System.Collections.Generic.Dictionary<string, TowerDefensePacketBankData> cache, HashSet<string> visiting)
	{
		if (cache.TryGetValue(bankName, out var value))
		{
			return value;
		}
		if (!rawBanks.TryGetValue(bankName, out var value2) || value2.VariantType != Variant.Type.Dictionary)
		{
			throw new InvalidOperationException("PacketBank Include references missing bank: " + bankName);
		}
		if (!visiting.Add(bankName))
		{
			throw new InvalidOperationException("PacketBank Include cycle detected while expanding " + bankName + ".");
		}
		Dictionary dictionary = value2.AsGodotDictionary();
		TowerDefensePacketBankData towerDefensePacketBankData = new TowerDefensePacketBankData();
		if (dictionary.TryGetValue("Category", out var value3) && value3.VariantType == Variant.Type.Dictionary)
		{
			towerDefensePacketBankData.category = value3.AsGodotDictionary().Duplicate(deep: true);
		}
		if (dictionary.TryGetValue("Include", out var value4) && value4.VariantType == Variant.Type.Array)
		{
			foreach (Variant item in value4.AsGodotArray())
			{
				TowerDefensePacketBankData towerDefensePacketBankData2 = BuildExpandedPacketBank(item.AsString(), rawBanks, cache, visiting);
				MergePacketBankCategories(towerDefensePacketBankData.category, towerDefensePacketBankData2.category);
			}
		}
		visiting.Remove(bankName);
		cache[bankName] = towerDefensePacketBankData;
		return towerDefensePacketBankData;
	}

	private static void MergePacketBankCategories(Dictionary target, Dictionary source)
	{
		foreach (Variant key in source.Keys)
		{
			string text = key.AsString();
			Godot.Collections.Array array = source[key].AsGodotArray();
			if (target.TryGetValue(text, out var value) && value.VariantType == Variant.Type.Array)
			{
				value.AsGodotArray().AddRange(array);
			}
			else
			{
				target[text] = array.Duplicate(deep: true);
			}
		}
	}

	private static void ValidateCompletePublication(FullGameplayRegistryRoots roots, FullGameplayResourceManifestData manifest, System.Collections.Generic.Dictionary<string, Resource> characterScenes, System.Collections.Generic.Dictionary<string, Resource> sprites, System.Collections.Generic.Dictionary<string, Resource> packets, System.Collections.Generic.Dictionary<string, TowerDefensePacketBankData> packetBanks)
	{
		if (characterScenes.Count != roots.ScenePathByCharacter.Count || characterScenes.Count > manifest.RegisteredCharacterCount)
		{
			throw new InvalidOperationException($"Character Scene publication count mismatch: actual={characterScenes.Count}, expectedBindings={roots.ScenePathByCharacter.Count}, registeredCharacters={manifest.RegisteredCharacterCount}");
		}
		if (sprites.Count != manifest.RegisteredSpriteCount)
		{
			throw new InvalidOperationException($"Character Sprite publication count mismatch: actual={sprites.Count}, expected={manifest.RegisteredSpriteCount}");
		}
		if (packets.Count != manifest.RegisteredPacketCount)
		{
			throw new InvalidOperationException($"Packet publication count mismatch: actual={packets.Count}, expected={manifest.RegisteredPacketCount}");
		}
		foreach (var (value, towerDefensePacketBankData2) in packetBanks)
		{
			foreach (Variant packet in towerDefensePacketBankData2.GetPacketList())
			{
				string text2 = packet.AsString();
				bool flag = roots.CharacterNameByPacket.TryGetValue(text2, out var value2);
				if (!packets.ContainsKey(text2) || !flag || !characterScenes.ContainsKey(value2) || !sprites.ContainsKey(value2))
				{
					throw new InvalidOperationException($"Expanded PacketBank candidate is not fully resident: bank={value}, packet={text2}, character={value2}");
				}
			}
		}
	}

	private Resource LoadBuiltInRoot(string configuredPath, string category, string key, out double actualLoadMilliseconds)
	{
		RequireMainThread("LoadBuiltInRoot");
		if (_gameplayResourceLoadState == GameplayResourceLoadState.Ready)
		{
			Interlocked.Increment(ref _lateCharacterResourceLoadCount);
			throw new InvalidOperationException($"Late built-in gameplay resource load is forbidden: category={category}, key={key}, path={configuredPath}");
		}
		string text = ProjectResourceUidCache.ResolveResourcePath(configuredPath).Replace('\\', '/');
		if (string.IsNullOrWhiteSpace(text) || !ResourceLoader.Exists(text))
		{
			throw new InvalidOperationException($"Built-in gameplay resource path is missing: category={category}, key={key}, path={configuredPath}, resolved={text}");
		}
		ulong ticksUsec = Time.GetTicksUsec();
		Resource resource = GD.Load(text);
		actualLoadMilliseconds = (double)(Time.GetTicksUsec() - ticksUsec) / 1000.0;
		if (!GodotObject.IsInstanceValid(resource))
		{
			throw new InvalidOperationException($"Built-in gameplay resource failed to load: category={category}, key={key}, path={text}");
		}
		return resource;
	}

	private static Dictionary RequireJsonDictionary(Json resource, string context)
	{
		if (!GodotObject.IsInstanceValid(resource) || resource.Data.VariantType != Variant.Type.Dictionary)
		{
			throw new InvalidOperationException("JSON registry is unavailable or invalid: " + context);
		}
		return resource.Data.AsGodotDictionary();
	}

	private void PublishReady(FullGameplayCoreSnapshot coreSnapshot, FullGameplayRootSnapshot rootSnapshot)
	{
		currentStep = stepMax;
		stepName = string.Empty;
		double fullWallMilliseconds = (double)(Time.GetTicksUsec() - _fullLoadStartedUsec) / 1000.0;
		double textureArrayMilliseconds = ((_gameplayAtlasPoseReadyUsec > _gameplayAtlasLoadStartedUsec) ? ((double)(_gameplayAtlasPoseReadyUsec - _gameplayAtlasLoadStartedUsec) / 1000.0) : 0.0);
		FullGameplayResourceLoadMetrics = new FullGameplayResourceLoadMetricsSnapshot(_coreMilliseconds, textureArrayMilliseconds, _characterSceneMilliseconds, _spriteMilliseconds, _packetMilliseconds, fullWallMilliseconds, 0, LateCharacterResourceLoadCount, _packetBankMissingCount, _rootLoadMetrics);
		SURVIVALS = coreSnapshot.Survivals;
		LEVELS = coreSnapshot.Levels;
		MAPS = coreSnapshot.Maps;
		BGMS = coreSnapshot.Bgms;
		PROJECTILE_CONFIG = coreSnapshot.ProjectileConfigs;
		AUDIOS = coreSnapshot.Audios;
		TALKS = coreSnapshot.Talks;
		TUTORIALS = coreSnapshot.Tutorials;
		COLLECTABLES = coreSnapshot.Collectables;
		SHOVELS = coreSnapshot.Shovels;
		MOWERS = coreSnapshot.Mowers;
		SHOPS = coreSnapshot.Shops;
		DAILY_LEVEL_DATA = coreSnapshot.DailyLevelData;
		TOWERDEFENSE_LEVEL_EVENT = coreSnapshot.TowerDefenseLevelEvents;
		TOWERDEFENSE_CHARCATERS = rootSnapshot.CharacterScenes;
		CHARCTAER_SPRITE = rootSnapshot.Sprites;
		TOWERDEFENSE_PACKETS = rootSnapshot.Packets;
		TOWERDEFENSE_PACKETBANKS = rootSnapshot.PacketBanks;
		_bgmPaths = coreSnapshot.BgmPaths;
		_bgmResourcesLoaded = false;
		_characterScenePaths = new System.Collections.Generic.Dictionary<string, string>(coreSnapshot.RegistryRoots.ScenePathByCharacter, StringComparer.OrdinalIgnoreCase);
		_characterSpritePaths = new System.Collections.Generic.Dictionary<string, string>(coreSnapshot.RegistryRoots.SpritePathByCharacter, StringComparer.OrdinalIgnoreCase);
		_packetPaths = new System.Collections.Generic.Dictionary<string, string>(coreSnapshot.RegistryRoots.PacketPathByName, StringComparer.OrdinalIgnoreCase);
		_characterNameByPacket = new System.Collections.Generic.Dictionary<string, string>(coreSnapshot.RegistryRoots.CharacterNameByPacket, StringComparer.OrdinalIgnoreCase);
		FullGameplayResourcesReadyUsec = Time.GetTicksUsec();
		_gameplayResourceLoadState = GameplayResourceLoadState.Ready;
		_fullGameplayResourceLoadError = string.Empty;
		_fullReadyCompletion.TrySetResult(result: true);
		EmitLoadPercentage(1.0, stepName);
		NotifyFullGameplayLoadReady();
	}

	private void FailFullGameplayLoad(Exception exception)
	{
		if (_gameplayResourceLoadState != GameplayResourceLoadState.Ready && _gameplayResourceLoadState != GameplayResourceLoadState.Failed)
		{
			_fullGameplayResourceLoadError = exception?.Message ?? "Unknown full gameplay resource load error.";
			_gameplayResourceLoadState = GameplayResourceLoadState.Failed;
			double fullWallMilliseconds = ((_fullLoadStartedUsec == 0L) ? 0.0 : ((double)(Time.GetTicksUsec() - _fullLoadStartedUsec) / 1000.0));
			double textureArrayMilliseconds = ((_gameplayAtlasPoseReadyUsec > _gameplayAtlasLoadStartedUsec) ? ((double)(_gameplayAtlasPoseReadyUsec - _gameplayAtlasLoadStartedUsec) / 1000.0) : 0.0);
			FullGameplayResourceLoadMetrics = new FullGameplayResourceLoadMetricsSnapshot(_coreMilliseconds, textureArrayMilliseconds, _characterSceneMilliseconds, _spriteMilliseconds, _packetMilliseconds, fullWallMilliseconds, 1, LateCharacterResourceLoadCount, _packetBankMissingCount, _rootLoadMetrics);
			InvalidOperationException exception2 = new InvalidOperationException("Full gameplay resource loading failed: " + _fullGameplayResourceLoadError, exception);
			_fullReadyCompletion.TrySetException(exception2);
			NotifyFullGameplayLoadFailed(exception);
		}
	}

	private void NotifyFullGameplayLoadReady()
	{
		try
		{
			LogMetrics(FullGameplayResourceLoadMetrics);
		}
		catch (Exception exception)
		{
			ReportNotificationFailure("Full gameplay Ready metrics logging", exception);
		}
		NotifyLoadOverSubscribers();
	}

	private void NotifyFullGameplayLoadFailed(Exception exception)
	{
		try
		{
			ILogger logger = _logger;
			ZLoggerErrorInterpolatedStringHandler message = new ZLoggerErrorInterpolatedStringHandler(40, 2, logger, out var enabled);
			if (enabled)
			{
				message.AppendLiteral("Full gameplay resource loading failed: ");
				message.AppendFormatted(_fullGameplayResourceLoadError, 0, null, "_fullGameplayResourceLoadError");
				message.AppendLiteral("\n");
				message.AppendFormatted(exception, 0, null, "exception");
			}
			logger.ZLogError(ref message);
		}
		catch (Exception exception2)
		{
			ReportNotificationFailure("Full gameplay Failed logging", exception2);
		}
		NotifyLoadFailedSubscribers(_fullGameplayResourceLoadError);
	}

	private void NotifyLoadOverSubscribers()
	{
		LoadOverEventHandler loadOverEventHandler = OnLoadOver;
		if (loadOverEventHandler == null)
		{
			return;
		}
		Delegate[] invocationList = loadOverEventHandler.GetInvocationList();
		foreach (Delegate obj in invocationList)
		{
			try
			{
				((LoadOverEventHandler)obj)();
			}
			catch (Exception exception)
			{
				ReportNotificationFailure("OnLoadOver subscriber", exception);
			}
		}
	}

	private void NotifyLoadFailedSubscribers(string error)
	{
		LoadFailedEventHandler loadFailedEventHandler = OnLoadFailed;
		if (loadFailedEventHandler == null)
		{
			return;
		}
		Delegate[] invocationList = loadFailedEventHandler.GetInvocationList();
		foreach (Delegate obj in invocationList)
		{
			try
			{
				((LoadFailedEventHandler)obj)(error);
			}
			catch (Exception exception)
			{
				ReportNotificationFailure("OnLoadFailed subscriber", exception);
			}
		}
	}

	private static void ReportNotificationFailure(string context, Exception exception)
	{
		try
		{
			ILogger logger = _logger;
			ZLoggerErrorInterpolatedStringHandler message = new ZLoggerErrorInterpolatedStringHandler(41, 2, logger, out var enabled);
			if (enabled)
			{
				message.AppendFormatted(context, 0, null, "context");
				message.AppendLiteral(" failed without changing resource state: ");
				message.AppendFormatted(exception, 0, null, "exception");
			}
			logger.ZLogError(ref message);
		}
		catch
		{
			try
			{
				GD.PushError($"{context} failed without changing resource state: {exception}");
			}
			catch
			{
			}
		}
	}

	private static void LogMetrics(FullGameplayResourceLoadMetricsSnapshot snapshot)
	{
		ILogger logger = _logger;
		ILogger logger2 = logger;
		ZLoggerInformationInterpolatedStringHandler message = new ZLoggerInformationInterpolatedStringHandler(167, 8, logger, out var enabled);
		if (enabled)
		{
			message.AppendLiteral("Full gameplay resources ready; coreMs=");
			message.AppendFormatted(snapshot.CoreMilliseconds, 0, "F2", "snapshot.CoreMilliseconds");
			message.AppendLiteral(", textureArrayMs=");
			message.AppendFormatted(snapshot.TextureArrayMilliseconds, 0, "F2", "snapshot.TextureArrayMilliseconds");
			message.AppendLiteral(", characterSceneMs=");
			message.AppendFormatted(snapshot.CharacterSceneMilliseconds, 0, "F2", "snapshot.CharacterSceneMilliseconds");
			message.AppendLiteral(", spriteMs=");
			message.AppendFormatted(snapshot.SpriteMilliseconds, 0, "F2", "snapshot.SpriteMilliseconds");
			message.AppendLiteral(", packetMs=");
			message.AppendFormatted(snapshot.PacketMilliseconds, 0, "F2", "snapshot.PacketMilliseconds");
			message.AppendLiteral(", fullWallMs=");
			message.AppendFormatted(snapshot.FullWallMilliseconds, 0, "F2", "snapshot.FullWallMilliseconds");
			message.AppendLiteral(", lateCharacterResourceLoadCount=");
			message.AppendFormatted(snapshot.LateCharacterResourceLoadCount, 0, null, "snapshot.LateCharacterResourceLoadCount");
			message.AppendLiteral(", packetBankMissingCount=");
			message.AppendFormatted(snapshot.PacketBankMissingCount, 0, null, "snapshot.PacketBankMissingCount");
		}
		logger2.ZLogInformation(ref message);
		foreach (var (value, fullGameplayResourceCategoryMetrics2) in snapshot.CategoryMetrics)
		{
			logger = _logger;
			ILogger logger3 = logger;
			message = new ZLoggerInformationInterpolatedStringHandler(144, 9, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Full gameplay category metrics; category=");
				message.AppendFormatted(value, 0, null, "category");
				message.AppendLiteral(", roots=");
				message.AppendFormatted(fullGameplayResourceCategoryMetrics2.RootCount, 0, null, "metrics.RootCount");
				message.AppendLiteral(", queueWaitMs=0.00, actualLoadMs=");
				message.AppendFormatted(fullGameplayResourceCategoryMetrics2.TotalActualLoadMilliseconds, 0, "F2", "metrics.TotalActualLoadMilliseconds");
				message.AppendLiteral(", publishMs=");
				message.AppendFormatted(fullGameplayResourceCategoryMetrics2.TotalPublishMilliseconds, 0, "F2", "metrics.TotalPublishMilliseconds");
				message.AppendLiteral(", p50Ms=");
				message.AppendFormatted(fullGameplayResourceCategoryMetrics2.P50ActualLoadMilliseconds, 0, "F2", "metrics.P50ActualLoadMilliseconds");
				message.AppendLiteral(", p95Ms=");
				message.AppendFormatted(fullGameplayResourceCategoryMetrics2.P95ActualLoadMilliseconds, 0, "F2", "metrics.P95ActualLoadMilliseconds");
				message.AppendLiteral(", maxMs=");
				message.AppendFormatted(fullGameplayResourceCategoryMetrics2.MaximumActualLoadMilliseconds, 0, "F2", "metrics.MaximumActualLoadMilliseconds");
				message.AppendLiteral(", cacheHits=");
				message.AppendFormatted(fullGameplayResourceCategoryMetrics2.CacheBindingHitCount, 0, null, "metrics.CacheBindingHitCount");
				message.AppendLiteral(", cacheMisses=");
				message.AppendFormatted(fullGameplayResourceCategoryMetrics2.CacheBindingMissCount, 0, null, "metrics.CacheBindingMissCount");
			}
			logger3.ZLogInformation(ref message);
		}
	}

	private static TaskCompletionSource<bool> CreateReadyCompletion()
	{
		return new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
	}

	private void RequireMainThread(string context)
	{
		if (System.Environment.CurrentManagedThreadId != _mainThreadId)
		{
			throw new InvalidOperationException(context + " must be called from the Godot main thread.");
		}
	}

	private void EmitLoadPercentageForCurrentStep(string loadingStep)
	{
		stepName = loadingStep;
		EmitLoadPercentage((double)currentStep / (double)stepMax, loadingStep);
	}

	private void EmitLoadResourceProgress(int step, string loadingStep, string resourceName, int resourceIndex, int resourceTotal)
	{
		double num = ((resourceTotal > 0) ? ((double)resourceIndex / (double)resourceTotal) : 0.0);
		EmitLoadPercentage(((double)step + num) / (double)stepMax, loadingStep, resourceName, resourceIndex, resourceTotal);
	}

	private void EmitLoadPercentage(double percentage, string loadingStep, string resourceName = "", int resourceIndex = 0, int resourceTotal = 0)
	{
		LoadPercentageEventHandler loadPercentageEventHandler = OnLoadPercentage;
		if (loadPercentageEventHandler == null)
		{
			return;
		}
		Delegate[] invocationList = loadPercentageEventHandler.GetInvocationList();
		foreach (Delegate obj in invocationList)
		{
			try
			{
				((LoadPercentageEventHandler)obj)(percentage, loadingStep, resourceName, resourceIndex, resourceTotal);
			}
			catch (Exception exception)
			{
				ReportNotificationFailure("OnLoadPercentage subscriber", exception);
			}
		}
	}

	public void EnsureAllBgmsLoaded()
	{
		if (_bgmResourcesLoaded)
		{
			return;
		}
		_bgmResourcesLoaded = true;
		foreach (var (text3, path) in _bgmPaths)
		{
			string text4 = ProjectResourceUidCache.ResolveResourcePath(path);
			Resource resource = GD.Load(text4);
			if (!GodotObject.IsInstanceValid(resource))
			{
				throw new InvalidOperationException("BGM resource failed to load: key=" + text3 + ", path=" + text4);
			}
			BGMS[text3] = resource;
		}
	}

	public bool PreloadAdobeAnimateGlobalAtlases()
	{
		if (_gameplayAtlasLoadState == GameplayAtlasLoadState.Failed)
		{
			return false;
		}
		TextureLayered instance = EnsureAdobeAnimateVisualTextureArrayLoaded();
		TextureLayered instance2 = EnsureAdobeAnimateGpuPoseTextureArrayLoaded();
		int num;
		if (GodotObject.IsInstanceValid(instance))
		{
			num = (GodotObject.IsInstanceValid(instance2) ? 1 : 0);
			if (num != 0)
			{
				_gameplayAtlasLoadState = GameplayAtlasLoadState.Ready;
				_gameplayAtlasLoadError = string.Empty;
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	public void BeginGameplayAtlasPreload()
	{
		RequireMainThread("BeginGameplayAtlasPreload");
		if (!AreGameplayAtlasesReady && _gameplayAtlasLoadState != GameplayAtlasLoadState.LoadingVisual && _gameplayAtlasLoadState != GameplayAtlasLoadState.LoadingPose && _gameplayAtlasLoadState != GameplayAtlasLoadState.Failed)
		{
			_gameplayAtlasLoadError = string.Empty;
			_adobeAnimateVisualTextureArray = null;
			_adobeAnimateGpuPoseTextureArray = null;
			_gameplayAtlasLoadStartedUsec = Time.GetTicksUsec();
			_gameplayAtlasVisualReadyUsec = 0uL;
			_gameplayAtlasPoseReadyUsec = 0uL;
			if (!TryRequestGameplayAtlas("res://addons/AdobeAnimateEditor/GeneratedAtlas/AdobeAnimateVisualTextureArray.png", GameplayAtlasLoadState.LoadingVisual))
			{
				FailGameplayAtlasLoad("Visual Texture2DArray request failed: res://addons/AdobeAnimateEditor/GeneratedAtlas/AdobeAnimateVisualTextureArray.png");
			}
		}
	}

	private bool TryRequestGameplayAtlas(string resourcePath, GameplayAtlasLoadState loadingState)
	{
		if (string.IsNullOrWhiteSpace(resourcePath) || !ResourceLoader.Exists(resourcePath))
		{
			return false;
		}
		Error error = ResourceLoader.LoadThreadedRequest(resourcePath, "", useSubThreads: false, ResourceLoader.CacheMode.Reuse);
		ResourceLoader.ThreadLoadStatus threadLoadStatus = ResourceLoader.LoadThreadedGetStatus(resourcePath);
		if (error != Error.Ok && threadLoadStatus != ResourceLoader.ThreadLoadStatus.InProgress && threadLoadStatus != ResourceLoader.ThreadLoadStatus.Loaded)
		{
			return false;
		}
		_gameplayAtlasLoadState = loadingState;
		return true;
	}

	private void PumpGameplayAtlasPreload()
	{
		switch (_gameplayAtlasLoadState)
		{
		case GameplayAtlasLoadState.LoadingVisual:
			PumpGameplayVisualAtlas();
			break;
		case GameplayAtlasLoadState.LoadingPose:
			PumpGameplayPoseAtlas();
			break;
		}
	}

	private void PumpGameplayVisualAtlas()
	{
		string text = "res://addons/AdobeAnimateEditor/GeneratedAtlas/AdobeAnimateVisualTextureArray.png";
		ResourceLoader.ThreadLoadStatus threadLoadStatus = ResourceLoader.LoadThreadedGetStatus(text);
		switch (threadLoadStatus)
		{
		case ResourceLoader.ThreadLoadStatus.InProgress:
			break;
		default:
			FailGameplayAtlasLoad($"Visual Texture2DArray load failed: status={threadLoadStatus}, path={text}");
			break;
		case ResourceLoader.ThreadLoadStatus.Loaded:
			_adobeAnimateVisualTextureArray = ResourceLoader.LoadThreadedGet(text) as TextureLayered;
			if (!GodotObject.IsInstanceValid(_adobeAnimateVisualTextureArray) || !_adobeAnimateVisualTextureArray.GetRid().IsValid)
			{
				FailGameplayAtlasLoad("Visual Texture2DArray is invalid after threaded load: " + text);
				break;
			}
			_gameplayAtlasVisualReadyUsec = Time.GetTicksUsec();
			if (!TryRequestGameplayAtlas("res://addons/AdobeAnimateEditor/GeneratedAtlas/AdobeAnimateGpuPoseTextureArray.exr", GameplayAtlasLoadState.LoadingPose))
			{
				FailGameplayAtlasLoad("Pose Texture2DArray request failed: res://addons/AdobeAnimateEditor/GeneratedAtlas/AdobeAnimateGpuPoseTextureArray.exr");
			}
			break;
		}
	}

	private void PumpGameplayPoseAtlas()
	{
		string text = "res://addons/AdobeAnimateEditor/GeneratedAtlas/AdobeAnimateGpuPoseTextureArray.exr";
		ResourceLoader.ThreadLoadStatus threadLoadStatus = ResourceLoader.LoadThreadedGetStatus(text);
		switch (threadLoadStatus)
		{
		case ResourceLoader.ThreadLoadStatus.InProgress:
			break;
		default:
			FailGameplayAtlasLoad($"Pose Texture2DArray load failed: status={threadLoadStatus}, path={text}");
			break;
		case ResourceLoader.ThreadLoadStatus.Loaded:
		{
			_adobeAnimateGpuPoseTextureArray = ResourceLoader.LoadThreadedGet(text) as TextureLayered;
			if (!GodotObject.IsInstanceValid(_adobeAnimateGpuPoseTextureArray) || !_adobeAnimateGpuPoseTextureArray.GetRid().IsValid)
			{
				FailGameplayAtlasLoad("Pose Texture2DArray is invalid after threaded load: " + text);
				break;
			}
			TextureLayered textureLayered = AdobeAnimateGlobalAtlasCache.PreloadVisualTextureArray();
			TextureLayered textureLayered2 = AdobeAnimateGlobalAtlasCache.PreloadGpuPoseTextureArray();
			if (!GodotObject.IsInstanceValid(textureLayered) || !GodotObject.IsInstanceValid(textureLayered2) || textureLayered.GetRid() != _adobeAnimateVisualTextureArray.GetRid() || textureLayered2.GetRid() != _adobeAnimateGpuPoseTextureArray.GetRid())
			{
				FailGameplayAtlasLoad("Gameplay atlas cache did not retain the same Visual/Pose TextureArray RIDs loaded by ResourceManager.");
				break;
			}
			_adobeAnimateVisualTextureArray = textureLayered;
			_adobeAnimateGpuPoseTextureArray = textureLayered2;
			_gameplayAtlasPoseReadyUsec = Time.GetTicksUsec();
			_gameplayAtlasLoadState = GameplayAtlasLoadState.Ready;
			_gameplayAtlasLoadError = string.Empty;
			break;
		}
		}
	}

	private void FailGameplayAtlasLoad(string error)
	{
		_gameplayAtlasLoadError = error ?? "Unknown gameplay atlas load error.";
		_gameplayAtlasLoadState = GameplayAtlasLoadState.Failed;
		ILogger logger = _logger;
		ZLoggerErrorInterpolatedStringHandler message = new ZLoggerErrorInterpolatedStringHandler(31, 1, logger, out var enabled);
		if (enabled)
		{
			message.AppendLiteral("Gameplay atlas preload failed: ");
			message.AppendFormatted(_gameplayAtlasLoadError, 0, null, "_gameplayAtlasLoadError");
		}
		logger.ZLogError(ref message);
	}

	public async Task EnsureGameplayAtlasesReadyAsync()
	{
		RequireMainThread("EnsureGameplayAtlasesReadyAsync");
		if (_gameplayAtlasLoadState == GameplayAtlasLoadState.NotStarted)
		{
			BeginGameplayAtlasPreload();
		}
		while (_gameplayAtlasLoadState == GameplayAtlasLoadState.LoadingVisual || _gameplayAtlasLoadState == GameplayAtlasLoadState.LoadingPose)
		{
			if (!IsInsideTree())
			{
				throw new InvalidOperationException("SceneTree became unavailable while waiting for gameplay atlases.");
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		if (!AreGameplayAtlasesReady)
		{
			throw new InvalidOperationException($"Gameplay atlases are not ready: {_gameplayAtlasLoadState}, {_gameplayAtlasLoadError}");
		}
	}

	public TextureLayered EnsureAdobeAnimateVisualTextureArrayLoaded()
	{
		if (!GodotObject.IsInstanceValid(_adobeAnimateVisualTextureArray))
		{
			_adobeAnimateVisualTextureArray = AdobeAnimateGlobalAtlasCache.PreloadVisualTextureArray();
		}
		return _adobeAnimateVisualTextureArray;
	}

	public TextureLayered EnsureAdobeAnimateGpuPoseTextureArrayLoaded()
	{
		if (!GodotObject.IsInstanceValid(_adobeAnimateGpuPoseTextureArray))
		{
			_adobeAnimateGpuPoseTextureArray = AdobeAnimateGlobalAtlasCache.PreloadGpuPoseTextureArray();
		}
		return _adobeAnimateGpuPoseTextureArray;
	}

	public PackedScene GetCharacterSprite(string characterName)
	{
		RequireFullGameplayResourcesReady("GetCharacterSprite(" + characterName + ")");
		if (string.IsNullOrWhiteSpace(characterName) || !CHARCTAER_SPRITE.TryGetValue(characterName, out var value))
		{
			throw new KeyNotFoundException("Built-in Character Sprite is not registered: " + characterName);
		}
		return (value as PackedScene) ?? throw new InvalidOperationException("Built-in Character Sprite is not a PackedScene: " + characterName);
	}

	public PackedScene GetCharacterScene(string characterName)
	{
		RequireFullGameplayResourcesReady("GetCharacterScene(" + characterName + ")");
		if (string.IsNullOrWhiteSpace(characterName) || !TOWERDEFENSE_CHARCATERS.TryGetValue(characterName, out var value))
		{
			throw new KeyNotFoundException("Built-in Character Scene is not registered: " + characterName);
		}
		return (value as PackedScene) ?? throw new InvalidOperationException("Built-in Character Scene is not a PackedScene: " + characterName);
	}

	public Variant GetPacket(string packetName)
	{
		RequireFullGameplayResourcesReady("GetPacket(" + packetName + ")");
		if (string.IsNullOrWhiteSpace(packetName) || !TOWERDEFENSE_PACKETS.TryGetValue(packetName, out var value))
		{
			throw new KeyNotFoundException("Built-in Packet config is not registered: " + packetName);
		}
		return value;
	}

	public List<string> GetPacketNames()
	{
		RequireFullGameplayResourcesReady("GetPacketNames");
		return new List<string>(TOWERDEFENSE_PACKETS.Keys);
	}

	public string GetRandomSpriteName()
	{
		RequireFullGameplayResourcesReady("GetRandomSpriteName");
		if (_characterSpritePaths.Count == 0)
		{
			throw new InvalidOperationException("No built-in Character Sprite is registered.");
		}
		int num = _random.Next(_characterSpritePaths.Count);
		int num2 = 0;
		foreach (string key in _characterSpritePaths.Keys)
		{
			if (num2 == num)
			{
				return key;
			}
			num2++;
		}
		throw new InvalidOperationException("Random Character Sprite selection exceeded the registered key range.");
	}

	public void ReleaseTransientResources(bool clearThumbnailCaches = true)
	{
		foreach (Resource value in MAPS.Values)
		{
			if (value is TowerDefenseMapConfig towerDefenseMapConfig)
			{
				towerDefenseMapConfig.ClearLoadedMapResources(clearThumbnail: true);
			}
		}
		TransientStaticTextureRelease.Release();
		AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
	}

	public ResourceManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/ResourceManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(31)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginLoad, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequireFullGameplayResourcesReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "context", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WarmConfigResourcesOnMainThread, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.InitializeRegistriesOnMainThread, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.MergePacketBankCategories, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RequireJsonDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false),
				new PropertyInfo(Variant.Type.String, "context", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyFullGameplayLoadReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifyLoadOverSubscribers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifyLoadFailedSubscribers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RequireMainThread, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "context", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitLoadPercentageForCurrentStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "loadingStep", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitLoadResourceProgress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "loadingStep", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "resourceName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resourceIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resourceTotal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitLoadPercentage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "loadingStep", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "resourceName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resourceIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resourceTotal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureAllBgmsLoaded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreloadAdobeAnimateGlobalAtlases, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginGameplayAtlasPreload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryRequestGameplayAtlas, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "loadingState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PumpGameplayAtlasPreload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PumpGameplayVisualAtlas, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PumpGameplayPoseAtlas, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FailGameplayAtlasLoad, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureAdobeAnimateVisualTextureArrayLoaded, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureAdobeAnimateGpuPoseTextureArrayLoaded, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCharacterSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetRandomSpriteName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseTransientResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "clearThumbnailCaches", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginLoad && args.Count == 0)
		{
			BeginLoad();
			ret = default;
			return true;
		}
		if (method == MethodName.RequireFullGameplayResourcesReady && args.Count == 1)
		{
			RequireFullGameplayResourcesReady(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WarmConfigResourcesOnMainThread && args.Count == 0)
		{
			WarmConfigResourcesOnMainThread();
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeRegistriesOnMainThread && args.Count == 0)
		{
			InitializeRegistriesOnMainThread();
			ret = default;
			return true;
		}
		if (method == MethodName.MergePacketBankCategories && args.Count == 2)
		{
			MergePacketBankCategories(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RequireJsonDictionary && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(RequireJsonDictionary(VariantUtils.ConvertTo<Json>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.NotifyFullGameplayLoadReady && args.Count == 0)
		{
			NotifyFullGameplayLoadReady();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyLoadOverSubscribers && args.Count == 0)
		{
			NotifyLoadOverSubscribers();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyLoadFailedSubscribers && args.Count == 1)
		{
			NotifyLoadFailedSubscribers(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RequireMainThread && args.Count == 1)
		{
			RequireMainThread(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitLoadPercentageForCurrentStep && args.Count == 1)
		{
			EmitLoadPercentageForCurrentStep(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitLoadResourceProgress && args.Count == 5)
		{
			EmitLoadResourceProgress(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitLoadPercentage && args.Count == 5)
		{
			EmitLoadPercentage(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureAllBgmsLoaded && args.Count == 0)
		{
			EnsureAllBgmsLoaded();
			ret = default;
			return true;
		}
		if (method == MethodName.PreloadAdobeAnimateGlobalAtlases && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(PreloadAdobeAnimateGlobalAtlases());
			return true;
		}
		if (method == MethodName.BeginGameplayAtlasPreload && args.Count == 0)
		{
			BeginGameplayAtlasPreload();
			ret = default;
			return true;
		}
		if (method == MethodName.TryRequestGameplayAtlas && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryRequestGameplayAtlas(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<GameplayAtlasLoadState>(in args[1])));
			return true;
		}
		if (method == MethodName.PumpGameplayAtlasPreload && args.Count == 0)
		{
			PumpGameplayAtlasPreload();
			ret = default;
			return true;
		}
		if (method == MethodName.PumpGameplayVisualAtlas && args.Count == 0)
		{
			PumpGameplayVisualAtlas();
			ret = default;
			return true;
		}
		if (method == MethodName.PumpGameplayPoseAtlas && args.Count == 0)
		{
			PumpGameplayPoseAtlas();
			ret = default;
			return true;
		}
		if (method == MethodName.FailGameplayAtlasLoad && args.Count == 1)
		{
			FailGameplayAtlasLoad(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureAdobeAnimateVisualTextureArrayLoaded && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TextureLayered>(EnsureAdobeAnimateVisualTextureArrayLoaded());
			return true;
		}
		if (method == MethodName.EnsureAdobeAnimateGpuPoseTextureArrayLoaded && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TextureLayered>(EnsureAdobeAnimateGpuPoseTextureArrayLoaded());
			return true;
		}
		if (method == MethodName.GetCharacterSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetCharacterSprite(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetCharacterScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetRandomSpriteName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetRandomSpriteName());
			return true;
		}
		if (method == MethodName.ReleaseTransientResources && args.Count == 1)
		{
			ReleaseTransientResources(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.WarmConfigResourcesOnMainThread && args.Count == 0)
		{
			WarmConfigResourcesOnMainThread();
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeRegistriesOnMainThread && args.Count == 0)
		{
			InitializeRegistriesOnMainThread();
			ret = default;
			return true;
		}
		if (method == MethodName.MergePacketBankCategories && args.Count == 2)
		{
			MergePacketBankCategories(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RequireJsonDictionary && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(RequireJsonDictionary(VariantUtils.ConvertTo<Json>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BeginLoad)
		{
			return true;
		}
		if (method == MethodName.RequireFullGameplayResourcesReady)
		{
			return true;
		}
		if (method == MethodName.WarmConfigResourcesOnMainThread)
		{
			return true;
		}
		if (method == MethodName.InitializeRegistriesOnMainThread)
		{
			return true;
		}
		if (method == MethodName.MergePacketBankCategories)
		{
			return true;
		}
		if (method == MethodName.RequireJsonDictionary)
		{
			return true;
		}
		if (method == MethodName.NotifyFullGameplayLoadReady)
		{
			return true;
		}
		if (method == MethodName.NotifyLoadOverSubscribers)
		{
			return true;
		}
		if (method == MethodName.NotifyLoadFailedSubscribers)
		{
			return true;
		}
		if (method == MethodName.RequireMainThread)
		{
			return true;
		}
		if (method == MethodName.EmitLoadPercentageForCurrentStep)
		{
			return true;
		}
		if (method == MethodName.EmitLoadResourceProgress)
		{
			return true;
		}
		if (method == MethodName.EmitLoadPercentage)
		{
			return true;
		}
		if (method == MethodName.EnsureAllBgmsLoaded)
		{
			return true;
		}
		if (method == MethodName.PreloadAdobeAnimateGlobalAtlases)
		{
			return true;
		}
		if (method == MethodName.BeginGameplayAtlasPreload)
		{
			return true;
		}
		if (method == MethodName.TryRequestGameplayAtlas)
		{
			return true;
		}
		if (method == MethodName.PumpGameplayAtlasPreload)
		{
			return true;
		}
		if (method == MethodName.PumpGameplayVisualAtlas)
		{
			return true;
		}
		if (method == MethodName.PumpGameplayPoseAtlas)
		{
			return true;
		}
		if (method == MethodName.FailGameplayAtlasLoad)
		{
			return true;
		}
		if (method == MethodName.EnsureAdobeAnimateVisualTextureArrayLoaded)
		{
			return true;
		}
		if (method == MethodName.EnsureAdobeAnimateGpuPoseTextureArrayLoaded)
		{
			return true;
		}
		if (method == MethodName.GetCharacterSprite)
		{
			return true;
		}
		if (method == MethodName.GetCharacterScene)
		{
			return true;
		}
		if (method == MethodName.GetPacket)
		{
			return true;
		}
		if (method == MethodName.GetRandomSpriteName)
		{
			return true;
		}
		if (method == MethodName.ReleaseTransientResources)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.FullGameplayResourcesReadyUsec)
		{
			FullGameplayResourcesReadyUsec = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.stepMax)
		{
			stepMax = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.stepName)
		{
			stepName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentStep)
		{
			currentStep = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._adobeAnimateVisualTextureArray)
		{
			_adobeAnimateVisualTextureArray = VariantUtils.ConvertTo<TextureLayered>(in value);
			return true;
		}
		if (name == PropertyName._adobeAnimateGpuPoseTextureArray)
		{
			_adobeAnimateGpuPoseTextureArray = VariantUtils.ConvertTo<TextureLayered>(in value);
			return true;
		}
		if (name == PropertyName._gameplayAtlasLoadState)
		{
			_gameplayAtlasLoadState = VariantUtils.ConvertTo<GameplayAtlasLoadState>(in value);
			return true;
		}
		if (name == PropertyName._gameplayResourceLoadState)
		{
			_gameplayResourceLoadState = VariantUtils.ConvertTo<GameplayResourceLoadState>(in value);
			return true;
		}
		if (name == PropertyName._gameplayAtlasLoadError)
		{
			_gameplayAtlasLoadError = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._fullGameplayResourceLoadError)
		{
			_fullGameplayResourceLoadError = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._gameplayAtlasLoadStartedUsec)
		{
			_gameplayAtlasLoadStartedUsec = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._gameplayAtlasVisualReadyUsec)
		{
			_gameplayAtlasVisualReadyUsec = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._gameplayAtlasPoseReadyUsec)
		{
			_gameplayAtlasPoseReadyUsec = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._fullLoadStartedUsec)
		{
			_fullLoadStartedUsec = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._loadStarted)
		{
			_loadStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._bgmResourcesLoaded)
		{
			_bgmResourcesLoaded = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mainThreadId)
		{
			_mainThreadId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lateCharacterResourceLoadCount)
		{
			_lateCharacterResourceLoadCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._packetBankMissingCount)
		{
			_packetBankMissingCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._coreMilliseconds)
		{
			_coreMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._characterSceneMilliseconds)
		{
			_characterSceneMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._spriteMilliseconds)
		{
			_spriteMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._packetMilliseconds)
		{
			_packetMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		ulong from;
		if (name == PropertyName.FullGameplayLoadStartedUsec)
		{
			from = FullGameplayLoadStartedUsec;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.FullGameplayResourcesReadyUsec)
		{
			from = FullGameplayResourcesReadyUsec;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.stepMax)
		{
			from2 = stepMax;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		string from3;
		if (name == PropertyName.stepName)
		{
			from3 = stepName;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.currentStep)
		{
			from2 = currentStep;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CurrentGameplayAtlasLoadState)
		{
			value = VariantUtils.CreateFrom<GameplayAtlasLoadState>(CurrentGameplayAtlasLoadState);
			return true;
		}
		bool from4;
		if (name == PropertyName.AreGameplayAtlasesReady)
		{
			from4 = AreGameplayAtlasesReady;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.HasGameplayAtlasLoadFailed)
		{
			from4 = HasGameplayAtlasLoadFailed;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.GameplayAtlasLoadError)
		{
			from3 = GameplayAtlasLoadError;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CurrentGameplayResourceLoadState)
		{
			value = VariantUtils.CreateFrom<GameplayResourceLoadState>(CurrentGameplayResourceLoadState);
			return true;
		}
		if (name == PropertyName.AreFullGameplayResourcesReady)
		{
			from4 = AreFullGameplayResourcesReady;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.FullGameplayResourceLoadError)
		{
			from3 = FullGameplayResourceLoadError;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.LateCharacterResourceLoadCount)
		{
			from2 = LateCharacterResourceLoadCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._adobeAnimateVisualTextureArray)
		{
			value = VariantUtils.CreateFrom(in _adobeAnimateVisualTextureArray);
			return true;
		}
		if (name == PropertyName._adobeAnimateGpuPoseTextureArray)
		{
			value = VariantUtils.CreateFrom(in _adobeAnimateGpuPoseTextureArray);
			return true;
		}
		if (name == PropertyName._gameplayAtlasLoadState)
		{
			value = VariantUtils.CreateFrom(in _gameplayAtlasLoadState);
			return true;
		}
		if (name == PropertyName._gameplayResourceLoadState)
		{
			value = VariantUtils.CreateFrom(in _gameplayResourceLoadState);
			return true;
		}
		if (name == PropertyName._gameplayAtlasLoadError)
		{
			value = VariantUtils.CreateFrom(in _gameplayAtlasLoadError);
			return true;
		}
		if (name == PropertyName._fullGameplayResourceLoadError)
		{
			value = VariantUtils.CreateFrom(in _fullGameplayResourceLoadError);
			return true;
		}
		if (name == PropertyName._gameplayAtlasLoadStartedUsec)
		{
			value = VariantUtils.CreateFrom(in _gameplayAtlasLoadStartedUsec);
			return true;
		}
		if (name == PropertyName._gameplayAtlasVisualReadyUsec)
		{
			value = VariantUtils.CreateFrom(in _gameplayAtlasVisualReadyUsec);
			return true;
		}
		if (name == PropertyName._gameplayAtlasPoseReadyUsec)
		{
			value = VariantUtils.CreateFrom(in _gameplayAtlasPoseReadyUsec);
			return true;
		}
		if (name == PropertyName._fullLoadStartedUsec)
		{
			value = VariantUtils.CreateFrom(in _fullLoadStartedUsec);
			return true;
		}
		if (name == PropertyName._loadStarted)
		{
			value = VariantUtils.CreateFrom(in _loadStarted);
			return true;
		}
		if (name == PropertyName._bgmResourcesLoaded)
		{
			value = VariantUtils.CreateFrom(in _bgmResourcesLoaded);
			return true;
		}
		if (name == PropertyName._mainThreadId)
		{
			value = VariantUtils.CreateFrom(in _mainThreadId);
			return true;
		}
		if (name == PropertyName._lateCharacterResourceLoadCount)
		{
			value = VariantUtils.CreateFrom(in _lateCharacterResourceLoadCount);
			return true;
		}
		if (name == PropertyName._packetBankMissingCount)
		{
			value = VariantUtils.CreateFrom(in _packetBankMissingCount);
			return true;
		}
		if (name == PropertyName._coreMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _coreMilliseconds);
			return true;
		}
		if (name == PropertyName._characterSceneMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _characterSceneMilliseconds);
			return true;
		}
		if (name == PropertyName._spriteMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _spriteMilliseconds);
			return true;
		}
		if (name == PropertyName._packetMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _packetMilliseconds);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._adobeAnimateVisualTextureArray, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._adobeAnimateGpuPoseTextureArray, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gameplayAtlasLoadState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gameplayResourceLoadState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._gameplayAtlasLoadError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._fullGameplayResourceLoadError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gameplayAtlasLoadStartedUsec, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gameplayAtlasVisualReadyUsec, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gameplayAtlasPoseReadyUsec, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._fullLoadStartedUsec, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.FullGameplayLoadStartedUsec, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.FullGameplayResourcesReadyUsec, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._loadStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._bgmResourcesLoaded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._mainThreadId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lateCharacterResourceLoadCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._packetBankMissingCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._coreMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._characterSceneMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._spriteMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._packetMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.stepMax, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.stepName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CurrentGameplayAtlasLoadState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AreGameplayAtlasesReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasGameplayAtlasLoadFailed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.GameplayAtlasLoadError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CurrentGameplayResourceLoadState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AreFullGameplayResourcesReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.FullGameplayResourceLoadError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LateCharacterResourceLoadCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.FullGameplayResourcesReadyUsec, Variant.From<ulong>(FullGameplayResourcesReadyUsec));
		info.AddProperty(PropertyName.stepMax, Variant.From<int>(stepMax));
		info.AddProperty(PropertyName.stepName, Variant.From<string>(stepName));
		info.AddProperty(PropertyName.currentStep, Variant.From<int>(currentStep));
		info.AddProperty(PropertyName._adobeAnimateVisualTextureArray, Variant.From(in _adobeAnimateVisualTextureArray));
		info.AddProperty(PropertyName._adobeAnimateGpuPoseTextureArray, Variant.From(in _adobeAnimateGpuPoseTextureArray));
		info.AddProperty(PropertyName._gameplayAtlasLoadState, Variant.From(in _gameplayAtlasLoadState));
		info.AddProperty(PropertyName._gameplayResourceLoadState, Variant.From(in _gameplayResourceLoadState));
		info.AddProperty(PropertyName._gameplayAtlasLoadError, Variant.From(in _gameplayAtlasLoadError));
		info.AddProperty(PropertyName._fullGameplayResourceLoadError, Variant.From(in _fullGameplayResourceLoadError));
		info.AddProperty(PropertyName._gameplayAtlasLoadStartedUsec, Variant.From(in _gameplayAtlasLoadStartedUsec));
		info.AddProperty(PropertyName._gameplayAtlasVisualReadyUsec, Variant.From(in _gameplayAtlasVisualReadyUsec));
		info.AddProperty(PropertyName._gameplayAtlasPoseReadyUsec, Variant.From(in _gameplayAtlasPoseReadyUsec));
		info.AddProperty(PropertyName._fullLoadStartedUsec, Variant.From(in _fullLoadStartedUsec));
		info.AddProperty(PropertyName._loadStarted, Variant.From(in _loadStarted));
		info.AddProperty(PropertyName._bgmResourcesLoaded, Variant.From(in _bgmResourcesLoaded));
		info.AddProperty(PropertyName._mainThreadId, Variant.From(in _mainThreadId));
		info.AddProperty(PropertyName._lateCharacterResourceLoadCount, Variant.From(in _lateCharacterResourceLoadCount));
		info.AddProperty(PropertyName._packetBankMissingCount, Variant.From(in _packetBankMissingCount));
		info.AddProperty(PropertyName._coreMilliseconds, Variant.From(in _coreMilliseconds));
		info.AddProperty(PropertyName._characterSceneMilliseconds, Variant.From(in _characterSceneMilliseconds));
		info.AddProperty(PropertyName._spriteMilliseconds, Variant.From(in _spriteMilliseconds));
		info.AddProperty(PropertyName._packetMilliseconds, Variant.From(in _packetMilliseconds));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.FullGameplayResourcesReadyUsec, out var value))
		{
			FullGameplayResourcesReadyUsec = value.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.stepMax, out var value2))
		{
			stepMax = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.stepName, out var value3))
		{
			stepName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentStep, out var value4))
		{
			currentStep = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._adobeAnimateVisualTextureArray, out var value5))
		{
			_adobeAnimateVisualTextureArray = value5.As<TextureLayered>();
		}
		if (info.TryGetProperty(PropertyName._adobeAnimateGpuPoseTextureArray, out var value6))
		{
			_adobeAnimateGpuPoseTextureArray = value6.As<TextureLayered>();
		}
		if (info.TryGetProperty(PropertyName._gameplayAtlasLoadState, out var value7))
		{
			_gameplayAtlasLoadState = value7.As<GameplayAtlasLoadState>();
		}
		if (info.TryGetProperty(PropertyName._gameplayResourceLoadState, out var value8))
		{
			_gameplayResourceLoadState = value8.As<GameplayResourceLoadState>();
		}
		if (info.TryGetProperty(PropertyName._gameplayAtlasLoadError, out var value9))
		{
			_gameplayAtlasLoadError = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName._fullGameplayResourceLoadError, out var value10))
		{
			_fullGameplayResourceLoadError = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName._gameplayAtlasLoadStartedUsec, out var value11))
		{
			_gameplayAtlasLoadStartedUsec = value11.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._gameplayAtlasVisualReadyUsec, out var value12))
		{
			_gameplayAtlasVisualReadyUsec = value12.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._gameplayAtlasPoseReadyUsec, out var value13))
		{
			_gameplayAtlasPoseReadyUsec = value13.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._fullLoadStartedUsec, out var value14))
		{
			_fullLoadStartedUsec = value14.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._loadStarted, out var value15))
		{
			_loadStarted = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._bgmResourcesLoaded, out var value16))
		{
			_bgmResourcesLoaded = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mainThreadId, out var value17))
		{
			_mainThreadId = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lateCharacterResourceLoadCount, out var value18))
		{
			_lateCharacterResourceLoadCount = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName._packetBankMissingCount, out var value19))
		{
			_packetBankMissingCount = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName._coreMilliseconds, out var value20))
		{
			_coreMilliseconds = value20.As<double>();
		}
		if (info.TryGetProperty(PropertyName._characterSceneMilliseconds, out var value21))
		{
			_characterSceneMilliseconds = value21.As<double>();
		}
		if (info.TryGetProperty(PropertyName._spriteMilliseconds, out var value22))
		{
			_spriteMilliseconds = value22.As<double>();
		}
		if (info.TryGetProperty(PropertyName._packetMilliseconds, out var value23))
		{
			_packetMilliseconds = value23.As<double>();
		}
	}
}
