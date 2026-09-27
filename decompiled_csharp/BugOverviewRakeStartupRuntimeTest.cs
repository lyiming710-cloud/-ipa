using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewRakeStartupRuntimeTest.cs")]
public class BugOverviewRakeStartupRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetupBattleFixture = "SetupBattleFixture";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName ReadMainStateMachineDispatchEnabled = "ReadMainStateMachineDispatchEnabled";

		public static readonly StringName ManagerContainsEncounter = "ManagerContainsEncounter";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName RegisterRealFixtures = "RegisterRealFixtures";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

		public static readonly StringName RestoreRealFixtures = "RestoreRealFixtures";

		public static readonly StringName FreeDetachedUiNodes = "FreeDetachedUiNodes";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _control = "_control";

		public static readonly StringName _mapFeature = "_mapFeature";

		public static readonly StringName _mapControl = "_mapControl";

		public static readonly StringName _levelControl = "_levelControl";

		public static readonly StringName _pauseButton = "_pauseButton";

		public static readonly StringName _speedCheckBox = "_speedCheckBox";

		public static readonly StringName _optionButton = "_optionButton";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string RakePacketPath = "res://Asset/Anime/Character/Item/Rake/Packet/ItemRake.tres";

	private const string RakeScenePath = "res://Asset/Anime/Character/Item/Rake/Scene/TowerDefenseItemRake.tscn";

	private const string SunRakePacketPath = "res://Asset/Anime/Character/Item/RakeSun/Packet/ItemRakeSun.tres";

	private const string SunRakeScenePath = "res://Asset/Anime/Character/Item/RakeSun/Scene/TowerDefenseItemRakeSun.tscn";

	private const string ZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Football/Packet/ZombieFootball.tres";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Football/Scene/Normal/TowerDefenseZombieFootball.tscn";

	private static readonly Vector2I EncounterGrid = new Vector2I(5, 3);

	private int _checks;

	private int _failures;

	private RakeStartupRuntimeControlStub _control;

	private TowerDefenseBattleFeatureMap _mapFeature;

	private TowerDefenseMapControl _mapControl;

	private TowerDefenseInGameLevelControl _levelControl;

	private MainButton _pauseButton;

	private CheckBox _speedCheckBox;

	private SpriteBrightButton _optionButton;

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		try
		{
			_ = 7;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("Required autoloads are unavailable.");
				}
				RegisterRealFixtures();
				SetupBattleFixture(manager);
				await VerifyPitchforkFollowsFirstWaveLane();
				var (rake, zombie) = await SpawnPreparedEncounter();
				Check(GodotObject.IsInstanceValid(rake), "GameReady-equivalent setup must spawn a real ItemRake before combat starts.");
				Check(GodotObject.IsInstanceValid(zombie), "The startup encounter must use a real fast ZombieFootball.");
				if (!GodotObject.IsInstanceValid(rake) || !GodotObject.IsInstanceValid(zombie))
				{
					throw new InvalidOperationException("The real rake startup encounter did not spawn.");
				}
				Check(ManagerContainsEncounter(manager, rake, zombie), "The real rake and zombie must be visible to GameRunningEntered through GetCharacter.");
				Check(rake.StateMachine?.IsInitialized ?? false, "The prepared rake must initialize its authored main state machine.");
				AttackComponent attack = rake.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
				Check(attack != null && !attack.IsReleased, "The real rake must expose its authored AttackComponent runtime.");
				Check(attack != null && attack.checkIntrevalNow > 0 && !GodotObject.IsInstanceValid(attack.target), "The prepared rake must retain a pending search interval before the already-overlapping football zombie enters active combat.");
				Check(!ReadMainStateMachineDispatchEnabled(rake), "A rake created while isGameRunning is false must keep main-state dispatch disabled.");
				double preparedHitpoints = zombie.instance.hitpoints;
				await WaitFrames(12);
				Check(rake.sprite.pause, "The prepared rake must remain paused while the ready phase is still active.");
				Check(Math.Abs(zombie.instance.hitpoints - preparedHitpoints) < 0.001, "A prepared rake must not damage a zombie before the formal game-running transition.");
				Check(!rake.over, "A prepared rake must not consume its one-shot trigger before combat starts.");
				_control.GameRunningEntered();
				bool flag = await WaitUntil(() => GodotObject.IsInstanceValid(rake) && rake.over, 2);
				Check(_control.isGameRunning && !_control.isInit, "GameRunningEntered must publish the formal combat-running state.");
				Check(ReadMainStateMachineDispatchEnabled(rake), "GameRunningEntered must wake the already registered rake state machine.");
				Check(flag && rake.over && attack.target == zombie, "The real rake must immediately trigger when active combat begins with a real football zombie already on it, without waiting out the generic attack search interval.");
				Check(!rake.sprite.pause, "A triggered real rake must resume its authored bonk animation.");
				double hitpointsBeforeBonk = zombie.instance.hitpoints;
				bool flag2 = await WaitUntil(() => !GodotObject.IsInstanceValid(zombie) || zombie.die || zombie.isDestroy || zombie.instance.hitpoints < hitpointsBeforeBonk, 60);
				bool flag3 = !GodotObject.IsInstanceValid(zombie) || zombie.die || zombie.isDestroy || zombie.instance.hitpoints < hitpointsBeforeBonk;
				Check(flag2 & flag3, "The triggered real rake animation must naturally complete and execute its authored 10000-damage hit against ZombieFootball.");
				await CleanupEncounter(rake, zombie);
				await VerifySunRakeStartup(manager);
				await VerifyPreSpawnRakeActivation(manager);
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewRakeStartupRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			_mapFeature?.Destroy();
			if (GodotObject.IsInstanceValid(_mapControl))
			{
				_mapControl.Free();
			}
			if (GodotObject.IsInstanceValid(_levelControl))
			{
				_levelControl.Free();
			}
			if (GodotObject.IsInstanceValid(_control))
			{
				_control.QueueFree();
			}
			FreeDetachedUiNodes();
			RestoreRealFixtures();
			await WaitFrames(3);
		}
		bool flag4 = _failures == 0 && _checks == 52;
		GD.Print($"RAKE_STARTUP_RESULT passed={flag4} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag4) ? 2 : 0);
	}

	private void SetupBattleFixture(TowerDefenseManager manager)
	{
		_pauseButton = new MainButton();
		_speedCheckBox = new CheckBox();
		_optionButton = new SpriteBrightButton();
		_control = new RakeStartupRuntimeControlStub
		{
			Name = "RakeStartupRuntimeControl",
			isGameRunning = false,
			isInit = true,
			levelConfig = new TowerDefenseLevelConfig(),
			buttonPause = _pauseButton,
			checkBox2X = _speedCheckBox,
			optionButton = _optionButton
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_control.characterNode = new Node2D
		{
			Name = "CharacterNode"
		};
		_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
		_levelControl = new TowerDefenseInGameLevelControl
		{
			awardCreate = false
		};
		_control.levelControl = _levelControl;
		manager.currentControl = _control;
		manager.gridBeginPos = Vector2.Zero;
		manager.gridSize = new Vector2(100f, 76f);
		manager.gridNum = new Vector2I(9, 5);
		_mapControl = new TowerDefenseMapControl
		{
			Name = "MapControl"
		};
		_mapFeature = CreateMapFeature(_mapControl, manager.gridNum);
		_mapFeature.control = _control;
		_control.featureDictionary[new StringName("Map")] = _mapFeature;
	}

	private async Task VerifyPitchforkFollowsFirstWaveLane()
	{
		TowerDefenseItemRake rake = LoadPacket("res://Asset/Anime/Character/Item/Rake/Packet/ItemRake.tres")?.Plant(EncounterGrid, playAudio: false) as TowerDefenseItemRake;
		await WaitFrames(4);
		Check(GodotObject.IsInstanceValid(rake) && rake.config?.name == "ItemRake", "The lane-order fixture must place the production ItemRake scene and packet.");
		TowerDefenseLevelWaveConfig towerDefenseLevelWaveConfig = new TowerDefenseLevelWaveConfig();
		towerDefenseLevelWaveConfig.spawn.Add(new TowerDefenseLevelSpawnConfig
		{
			zombie = "ZombieFootball",
			line = 1,
			num = 1
		});
		towerDefenseLevelWaveConfig.spawn.Add(new TowerDefenseLevelSpawnConfig
		{
			zombie = "ZombieFootball",
			line = 5,
			num = 1
		});
		TowerDefenseLevelWaveManagerConfig towerDefenseLevelWaveManagerConfig = new TowerDefenseLevelWaveManagerConfig
		{
			flagZombieUse = false,
			spawnMaxCharactersPerFrame = 8,
			spawnFrameBudgetMilliseconds = 16.0
		};
		towerDefenseLevelWaveManagerConfig.wave.Add(towerDefenseLevelWaveConfig);
		TowerDefenseBattleFeatureWave waveFeature = new TowerDefenseBattleFeatureWave
		{
			control = _control,
			levelControl = _levelControl,
			mapFeature = _mapFeature,
			config = towerDefenseLevelWaveManagerConfig,
			currentDynamic = new TowerDefenseLevelDynamicConfig(),
			savePitchforkLine = EncounterGrid.Y
		};
		_control.featureDictionary[new StringName("Wave")] = waveFeature;
		Check(waveFeature.config.wave[0].spawn.Count == 2 && waveFeature.config.wave[0].spawn[0].line != EncounterGrid.Y && waveFeature.config.wave[0].spawn[1].line != EncounterGrid.Y, "The focused Wave must use two pre-generated lanes that differ from the rake lane.");
		List<TowerDefenseZombie> spawnOrder = new List<TowerDefenseZombie>();
		_control.characterNode.ChildEnteredTree += ChildEntered;
		try
		{
			_control.isInit = false;
			_control.isGameRunning = true;
			await waveFeature.SpawnZombie(0);
			await WaitFrames(6);
			Check(spawnOrder.Count == 2, $"The real Wave feature must create exactly two pre-generated zombies; actual={spawnOrder.Count}.");
			TowerDefenseZombie towerDefenseZombie = ((spawnOrder.Count > 0) ? spawnOrder[0] : null);
			Check(towerDefenseZombie is TowerDefenseZombieFootball && towerDefenseZombie.config?.name == "ZombieFootball", "The first observed Wave child must be the production ZombieFootball scene and packet.");
			Check(GodotObject.IsInstanceValid(towerDefenseZombie) && GodotObject.IsInstanceValid(rake) && towerDefenseZombie.gridPos.Y == rake.gridPos.Y, $"The real rake lane must equal the first actual zombie lane; rake={rake?.gridPos.Y}, firstZombie={towerDefenseZombie?.gridPos.Y}.");
			Check(spawnOrder.Exists((TowerDefenseZombie zombie) => GodotObject.IsInstanceValid(zombie) && zombie.gridPos.Y != EncounterGrid.Y), "The scene must also spawn a zombie on another lane, proving the assertion is not a one-lane shortcut.");
			Check(waveFeature.savePitchforkLine == -1, "The Wave feature must consume the rake lane after assigning the first real zombie.");
		}
		finally
		{
			_control.characterNode.ChildEnteredTree -= ChildEntered;
			_control.isGameRunning = false;
			_control.isInit = true;
			_control.featureDictionary.Remove(new StringName("Wave"));
			waveFeature.Destroy();
			foreach (TowerDefenseZombie item2 in spawnOrder)
			{
				if (GodotObject.IsInstanceValid(item2) && !item2.IsQueuedForDeletion())
				{
					item2.QueueFree();
				}
			}
			if (GodotObject.IsInstanceValid(rake) && !rake.IsQueuedForDeletion())
			{
				rake.QueueFree();
			}
			await WaitFrames(4);
		}
		void ChildEntered(Node child)
		{
			if (child is TowerDefenseZombie item)
			{
				spawnOrder.Add(item);
			}
		}
	}

	private async Task<(TowerDefenseItemRake, TowerDefenseZombieFootball)> SpawnPreparedEncounter()
	{
		TowerDefenseItemRake rake = LoadPacket("res://Asset/Anime/Character/Item/Rake/Packet/ItemRake.tres")?.Plant(EncounterGrid, playAudio: false) as TowerDefenseItemRake;
		await WaitFrames(4);
		TowerDefenseZombieFootball zombie = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Football/Packet/ZombieFootball.tres")?.Plant(EncounterGrid, playAudio: false) as TowerDefenseZombieFootball;
		await WaitFrames(5);
		return (rake, zombie);
	}

	private async Task VerifySunRakeStartup(TowerDefenseManager manager)
	{
		_control.isGameRunning = false;
		_control.isInit = true;
		TowerDefenseItemRakeSun rake = LoadPacket("res://Asset/Anime/Character/Item/RakeSun/Packet/ItemRakeSun.tres")?.Plant(EncounterGrid, playAudio: false) as TowerDefenseItemRakeSun;
		await WaitFrames(4);
		TowerDefenseZombieFootball zombie = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Football/Packet/ZombieFootball.tres")?.Plant(EncounterGrid, playAudio: false) as TowerDefenseZombieFootball;
		await WaitFrames(5);
		Check(GodotObject.IsInstanceValid(rake), "GameReady-equivalent setup must spawn a real ItemRakeSun before combat starts.");
		Check(GodotObject.IsInstanceValid(zombie), "The Sun rake startup encounter must use a real fast ZombieFootball.");
		if (!GodotObject.IsInstanceValid(rake) || !GodotObject.IsInstanceValid(zombie))
		{
			throw new InvalidOperationException("The real Sun rake startup encounter did not spawn.");
		}
		Check(ManagerContainsEncounter(manager, rake, zombie), "The real Sun rake and zombie must be visible to GameRunningEntered through GetCharacter.");
		Check(rake.StateMachine?.IsInitialized ?? false, "The prepared Sun rake must initialize its authored main state machine.");
		AttackComponent attack = rake.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
		Check(attack != null && !attack.IsReleased, "The real Sun rake must expose its authored AttackComponent runtime.");
		Check(attack != null && attack.checkIntrevalNow > 0 && !GodotObject.IsInstanceValid(attack.target), "The prepared Sun rake must retain a pending search interval before active combat.");
		Check(!ReadMainStateMachineDispatchEnabled(rake), "A Sun rake created while isGameRunning is false must keep main-state dispatch disabled.");
		zombie.GlobalPosition = rake.GlobalPosition + new Vector2(80f, 0f);
		Check(attack.TryGetCheckAreaWorldRect(out var checkRect) && !AabbShapeUtil.Intersects(checkRect, zombie.WorldHitRect), "The fast-crossing fixture must begin outside the Sun rake's authored 33-pixel contact area.");
		double preparedHitpoints = zombie.instance.hitpoints;
		await WaitFrames(12);
		Check(rake.sprite.pause, "The prepared Sun rake must remain paused while the ready phase is still active.");
		Check(Math.Abs(zombie.instance.hitpoints - preparedHitpoints) < 0.001, "A prepared Sun rake must not damage a zombie before the formal game-running transition.");
		Check(!rake.over, "A prepared Sun rake must not consume its one-shot trigger before combat starts.");
		_control.GameRunningEntered();
		await WaitFrames(2);
		Check(!rake.over, "The Sun rake must stay armed after recording a nearby zombie that has not touched it.");
		Rect2 worldHitRect = zombie.WorldHitRect;
		zombie.GlobalPosition = rake.GlobalPosition + new Vector2(-80f, 0f);
		zombie.InvalidateHitBoxBounds();
		Rect2 worldHitRect2 = zombie.WorldHitRect;
		Check(!AabbShapeUtil.Intersects(checkRect, worldHitRect) && !AabbShapeUtil.Intersects(checkRect, worldHitRect2) && AabbShapeUtil.Intersects(checkRect, AabbShapeUtil.Union(worldHitRect, worldHitRect2)), "The regression fixture must cross the complete contact area without either sampled position overlapping it.");
		bool flag = await WaitUntil(() => GodotObject.IsInstanceValid(rake) && rake.over, 2);
		Check(_control.isGameRunning && !_control.isInit, "GameRunningEntered must publish the formal combat-running state for the Sun rake.");
		Check(ReadMainStateMachineDispatchEnabled(rake), "GameRunningEntered must wake the already registered Sun rake state machine.");
		Check(flag && rake.over && attack.target == zombie, "The real Sun rake must acquire the ZombieFootball that crossed its complete contact area between samples.");
		Check(!rake.sprite.pause, "A triggered real Sun rake must resume its authored bonk animation.");
		double hitpointsBeforeBonk = zombie.instance.hitpoints;
		bool flag2 = await WaitUntil(() => !GodotObject.IsInstanceValid(zombie) || zombie.die || zombie.isDestroy || zombie.instance.hitpoints < hitpointsBeforeBonk, 60);
		bool flag3 = !GodotObject.IsInstanceValid(zombie) || zombie.die || zombie.isDestroy || zombie.instance.hitpoints < hitpointsBeforeBonk;
		Check(flag2 & flag3, "The triggered real Sun rake animation must naturally complete and execute its 10000-damage hit.");
		foreach (Node item in GetTree().GetNodesInGroup("Sun"))
		{
			if (GodotObject.IsInstanceValid(item) && !item.IsQueuedForDeletion())
			{
				item.QueueFree();
			}
		}
		await CleanupEncounter(rake, zombie);
	}

	private async Task VerifyPreSpawnRakeActivation(TowerDefenseManager manager)
	{
		_control.isGameRunning = false;
		_control.isInit = true;
		TowerDefenseBattleFeaturePreSpawn preSpawnFeature = new TowerDefenseBattleFeaturePreSpawn
		{
			control = _control
		};
		preSpawnFeature.Init(new Dictionary { ["Packet"] = new Godot.Collections.Array
		{
			new Dictionary
			{
				["Name"] = "ItemRake",
				["GridPos"] = new Godot.Collections.Array { EncounterGrid.X, EncounterGrid.Y }
			}
		} });
		TowerDefenseItemRake rake = null;
		TowerDefenseZombieFootball zombie = null;
		List<Node> transientEffects = new List<Node>();
		try
		{
			await preSpawnFeature.GameEntry();
			await WaitFrames(4);
			CaptureTransientCharacterEffects(transientEffects);
			Check(preSpawnFeature.preSpawnList.Count == 1, "The real PreSpawn GameEntry lifecycle must retain exactly one authored rake until GameStart.");
			rake = ((preSpawnFeature.preSpawnList.Count == 1) ? (preSpawnFeature.preSpawnList[0] as TowerDefenseItemRake) : null);
			Check(GodotObject.IsInstanceValid(rake), "The real PreSpawn feature must create the production ItemRake.");
			if (!GodotObject.IsInstanceValid(rake))
			{
				throw new InvalidOperationException("The real PreSpawn rake did not spawn.");
			}
			AttackComponent attack = rake.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
			Check(rake.sprite.pause && !rake.over && attack != null && !attack.IsReleased && !GodotObject.IsInstanceValid(attack.target), "The authored PreSpawn rake must enter GameStart armed and paused without a target.");
			_control.isGameRunning = true;
			_control.isInit = false;
			await preSpawnFeature.GameStart();
			Check(preSpawnFeature.preSpawnList.Count == 0, "PreSpawn GameStart must release temporary ownership after activating the rake.");
			Check(rake.sprite.pause && !rake.over, "PreSpawn gameplay activation must preserve the rake's intentional armed pause.");
			await WaitFrames(3);
			Check(GodotObject.IsInstanceValid(rake) && rake.sprite.pause && !rake.over, "The activated PreSpawn rake must remain armed while no zombie is touching it.");
			zombie = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Football/Packet/ZombieFootball.tres")?.Plant(EncounterGrid, playAudio: false) as TowerDefenseZombieFootball;
			await WaitFrames(5);
			CaptureTransientCharacterEffects(transientEffects);
			Check(GodotObject.IsInstanceValid(zombie), "The PreSpawn regression encounter must create a real ZombieFootball.");
			if (!GodotObject.IsInstanceValid(zombie))
			{
				throw new InvalidOperationException("The PreSpawn regression zombie did not spawn.");
			}
			Check(await WaitUntil(() => GodotObject.IsInstanceValid(rake) && rake.over, 2) && attack.target == zombie, "The activated PreSpawn rake must acquire the first real zombie that steps on it.");
			Check(!rake.sprite.pause, "The triggered PreSpawn rake must resume its authored bonk animation.");
			double hitpointsBeforeBonk = zombie.instance.hitpoints;
			bool flag = await WaitUntil(() => !GodotObject.IsInstanceValid(zombie) || zombie.die || zombie.isDestroy || zombie.instance.hitpoints < hitpointsBeforeBonk, 60);
			bool flag2 = !GodotObject.IsInstanceValid(zombie) || zombie.die || zombie.isDestroy || zombie.instance.hitpoints < hitpointsBeforeBonk;
			Check(flag & flag2, "The real PreSpawn rake must naturally complete its bonk and deal 10000 damage.");
		}
		finally
		{
			preSpawnFeature.Destroy();
			await CleanupEncounter(rake, zombie);
			await CleanupTransientCharacterEffects(transientEffects);
		}
	}

	private void CaptureTransientCharacterEffects(List<Node> effects)
	{
		if (!GodotObject.IsInstanceValid(_control?.characterNode))
		{
			return;
		}
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (!(child is TowerDefenseCharacter) && GodotObject.IsInstanceValid(child) && !effects.Contains(child))
			{
				effects.Add(child);
			}
		}
	}

	private async Task CleanupTransientCharacterEffects(List<Node> effects)
	{
		CaptureTransientCharacterEffects(effects);
		foreach (Node effect in effects)
		{
			if (GodotObject.IsInstanceValid(effect) && !effect.IsQueuedForDeletion())
			{
				effect.Free();
			}
		}
		effects.Clear();
		await WaitFrames(4);
	}

	private async Task CleanupEncounter(TowerDefenseItemRakeBase rake, TowerDefenseZombie zombie)
	{
		if (GodotObject.IsInstanceValid(rake) && !rake.IsQueuedForDeletion())
		{
			rake.QueueFree();
		}
		if (GodotObject.IsInstanceValid(zombie) && !zombie.IsQueuedForDeletion())
		{
			zombie.QueueFree();
		}
		await WaitFrames(4);
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private static bool ReadMainStateMachineDispatchEnabled(TowerDefenseCharacter character)
	{
		object obj = typeof(TowerDefenseCharacter).GetField("_mainStateMachineDispatchEnabled", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(character);
		if (obj is bool)
		{
			return (bool)obj;
		}
		return false;
	}

	private static bool ManagerContainsEncounter(TowerDefenseManager manager, TowerDefenseItemRakeBase rake, TowerDefenseZombie zombie)
	{
		bool flag = false;
		bool flag2 = false;
		foreach (Variant item in manager.GetCharacter())
		{
			GodotObject godotObject = item.AsGodotObject();
			flag |= godotObject == rake;
			flag2 |= godotObject == zombie;
		}
		return flag & flag2;
	}

	private async Task<bool> WaitUntil(Func<bool> predicate, int maximumFrames)
	{
		for (int frame = 0; frame < maximumFrames; frame++)
		{
			if (predicate())
			{
				return true;
			}
			await WaitFrames(1);
		}
		return predicate();
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			gridNum = gridNum,
			gridBeginPos = Vector2.Zero,
			gridSize = new Vector2(100f, 76f),
			plantOffset = 50.0
		};
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig
		});
		towerDefenseBattleFeatureMap.PlantGridInit();
		for (int i = 1; i <= gridNum.X; i++)
		{
			for (int j = 1; j <= gridNum.Y; j++)
			{
				TowerDefenseCellInstance plantGridCell = towerDefenseBattleFeatureMap.GetPlantGridCell(new Vector2I(i, j));
				plantGridCell.Init(new TowerDefenseCellConfig());
				plantGridCell.gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
				{
					TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
					TowerDefenseEnum.PLANTGRIDTYPE.AIR
				};
				plantGridCell.slot.Clear();
				plantGridCell.slot[TowerDefenseEnum.PLANTGRIDTYPE.GROUND] = null;
				plantGridCell.slot[TowerDefenseEnum.PLANTGRIDTYPE.AIR] = null;
			}
		}
		for (int k = 1; k <= gridNum.Y; k++)
		{
			towerDefenseBattleFeatureMap.lineUse[k] = true;
		}
		return towerDefenseBattleFeatureMap;
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("ItemRake", "res://Asset/Anime/Character/Item/Rake/Packet/ItemRake.tres");
		RegisterPacket("ItemRakeSun", "res://Asset/Anime/Character/Item/RakeSun/Packet/ItemRakeSun.tres");
		RegisterPacket("ZombieFootball", "res://Asset/Anime/Character/Zombie/Chapter1/Football/Packet/ZombieFootball.tres");
		RegisterCharacter("ItemRake", "res://Asset/Anime/Character/Item/Rake/Scene/TowerDefenseItemRake.tscn");
		RegisterCharacter("ItemRakeSun", "res://Asset/Anime/Character/Item/RakeSun/Scene/TowerDefenseItemRakeSun.tscn");
		RegisterCharacter("ZombieFootball", "res://Asset/Anime/Character/Zombie/Chapter1/Football/Scene/Normal/TowerDefenseZombieFootball.tscn");
	}

	private void RegisterPacket(string key, string path)
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.TOWERDEFENSE_PACKETS.TryGetValue(key, out var value))
		{
			_previousPackets[key] = value;
		}
		else
		{
			_missingPackets.Add(key);
		}
		instance.TOWERDEFENSE_PACKETS[key] = ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore);
	}

	private void RegisterCharacter(string key, string path)
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.TOWERDEFENSE_CHARCATERS.TryGetValue(key, out var value))
		{
			_previousCharacters[key] = value;
		}
		else
		{
			_missingCharacters.Add(key);
		}
		instance.TOWERDEFENSE_CHARCATERS[key] = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
	}

	private void RestoreRealFixtures()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		foreach (string missingPacket in _missingPackets)
		{
			instance.TOWERDEFENSE_PACKETS.Remove(missingPacket);
		}
		foreach (KeyValuePair<string, Resource> previousPacket in _previousPackets)
		{
			instance.TOWERDEFENSE_PACKETS[previousPacket.Key] = previousPacket.Value;
		}
		foreach (string missingCharacter in _missingCharacters)
		{
			instance.TOWERDEFENSE_CHARCATERS.Remove(missingCharacter);
		}
		foreach (KeyValuePair<string, Resource> previousCharacter in _previousCharacters)
		{
			instance.TOWERDEFENSE_CHARCATERS[previousCharacter.Key] = previousCharacter.Value;
		}
	}

	private void FreeDetachedUiNodes()
	{
		if (GodotObject.IsInstanceValid(_pauseButton))
		{
			_pauseButton.Free();
		}
		if (GodotObject.IsInstanceValid(_speedCheckBox))
		{
			_speedCheckBox.Free();
		}
		if (GodotObject.IsInstanceValid(_optionButton))
		{
			_optionButton.Free();
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewRakeStartupRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(12)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.SetupBattleFixture, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.LoadPacket, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ReadMainStateMachineDispatchEnabled, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ManagerContainsEncounter, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "rake", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateMapFeature, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterRealFixtures, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterPacket, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterCharacter, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RestoreRealFixtures, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.FreeDetachedUiNodes, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SetupBattleFixture && args.Count == 1)
		{
			SetupBattleFixture(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadMainStateMachineDispatchEnabled && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadMainStateMachineDispatchEnabled(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.ManagerContainsEncounter && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ManagerContainsEncounter(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<TowerDefenseItemRakeBase>(in args[1]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.RegisterRealFixtures && args.Count == 0)
		{
			RegisterRealFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterPacket && args.Count == 2)
		{
			RegisterPacket(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterCharacter && args.Count == 2)
		{
			RegisterCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreRealFixtures && args.Count == 0)
		{
			RestoreRealFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.FreeDetachedUiNodes && args.Count == 0)
		{
			FreeDetachedUiNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadMainStateMachineDispatchEnabled && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadMainStateMachineDispatchEnabled(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.ManagerContainsEncounter && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ManagerContainsEncounter(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<TowerDefenseItemRakeBase>(in args[1]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
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
		if (method == MethodName.SetupBattleFixture)
		{
			return true;
		}
		if (method == MethodName.LoadPacket)
		{
			return true;
		}
		if (method == MethodName.ReadMainStateMachineDispatchEnabled)
		{
			return true;
		}
		if (method == MethodName.ManagerContainsEncounter)
		{
			return true;
		}
		if (method == MethodName.CreateMapFeature)
		{
			return true;
		}
		if (method == MethodName.RegisterRealFixtures)
		{
			return true;
		}
		if (method == MethodName.RegisterPacket)
		{
			return true;
		}
		if (method == MethodName.RegisterCharacter)
		{
			return true;
		}
		if (method == MethodName.RestoreRealFixtures)
		{
			return true;
		}
		if (method == MethodName.FreeDetachedUiNodes)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<RakeStartupRuntimeControlStub>(in value);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			_mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName._mapControl)
		{
			_mapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName._levelControl)
		{
			_levelControl = VariantUtils.ConvertTo<TowerDefenseInGameLevelControl>(in value);
			return true;
		}
		if (name == PropertyName._pauseButton)
		{
			_pauseButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._speedCheckBox)
		{
			_speedCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._optionButton)
		{
			_optionButton = VariantUtils.ConvertTo<SpriteBrightButton>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			value = VariantUtils.CreateFrom(in _mapFeature);
			return true;
		}
		if (name == PropertyName._mapControl)
		{
			value = VariantUtils.CreateFrom(in _mapControl);
			return true;
		}
		if (name == PropertyName._levelControl)
		{
			value = VariantUtils.CreateFrom(in _levelControl);
			return true;
		}
		if (name == PropertyName._pauseButton)
		{
			value = VariantUtils.CreateFrom(in _pauseButton);
			return true;
		}
		if (name == PropertyName._speedCheckBox)
		{
			value = VariantUtils.CreateFrom(in _speedCheckBox);
			return true;
		}
		if (name == PropertyName._optionButton)
		{
			value = VariantUtils.CreateFrom(in _optionButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._levelControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._pauseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._speedCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._optionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._mapFeature, Variant.From(in _mapFeature));
		info.AddProperty(PropertyName._mapControl, Variant.From(in _mapControl));
		info.AddProperty(PropertyName._levelControl, Variant.From(in _levelControl));
		info.AddProperty(PropertyName._pauseButton, Variant.From(in _pauseButton));
		info.AddProperty(PropertyName._speedCheckBox, Variant.From(in _speedCheckBox));
		info.AddProperty(PropertyName._optionButton, Variant.From(in _optionButton));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._control, out var value3))
		{
			_control = value3.As<RakeStartupRuntimeControlStub>();
		}
		if (info.TryGetProperty(PropertyName._mapFeature, out var value4))
		{
			_mapFeature = value4.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName._mapControl, out var value5))
		{
			_mapControl = value5.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName._levelControl, out var value6))
		{
			_levelControl = value6.As<TowerDefenseInGameLevelControl>();
		}
		if (info.TryGetProperty(PropertyName._pauseButton, out var value7))
		{
			_pauseButton = value7.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._speedCheckBox, out var value8))
		{
			_speedCheckBox = value8.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._optionButton, out var value9))
		{
			_optionButton = value9.As<SpriteBrightButton>();
		}
	}
}
