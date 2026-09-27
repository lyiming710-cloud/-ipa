using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentZombossCompositeRuntimeTest.cs")]
public class BugDepartmentZombossCompositeRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetupBattleFixture = "SetupBattleFixture";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName RefreshRegistration = "RefreshRegistration";

		public static readonly StringName RegisterFixture = "RegisterFixture";

		public static readonly StringName RestoreFixtures = "RestoreFixtures";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _control = "_control";

		public static readonly StringName _mapFeature = "_mapFeature";

		public static readonly StringName _mapControl = "_mapControl";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string BossScenePath = "res://Asset/Anime/Character/Zombie/Boss/Boss/Scene/TowerDefenseZombieBoss.tscn";

	private const string SunflowerPacketPath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Packet/PlantSunFlower.tres";

	private const string SunflowerScenePath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn";

	private const string BungiPacketPath = "res://Asset/Anime/Character/Zombie/Chapter5/Bungi/Packet/ZombieBungi.tres";

	private const string BungiScenePath = "res://Asset/Anime/Character/Zombie/Chapter5/Bungi/Scene/TowerDefenseZombieBungi.tscn";

	private const string NormalZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres";

	private const string NormalZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private static readonly Vector2I RvTargetGrid = new Vector2I(3, 2);

	private static readonly Vector2I RvLowerLaneGrid = new Vector2I(4, 3);

	private static readonly Vector2I BungiTargetGrid = new Vector2I(2, 4);

	private int _checks;

	private int _failures;

	private ZombossCompositeRuntimeControlStub _control;

	private TowerDefenseBattleFeatureMap _mapFeature;

	private TowerDefenseMapControl _mapControl;

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private readonly List<Resource> _loadedResources = new List<Resource>();

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		try
		{
			_ = 4;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance), "TowerDefenseManager and ResourceManager autoloads must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("Required autoloads are unavailable.");
				}
				RegisterFixture("PlantSunFlower", "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Packet/PlantSunFlower.tres", "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn");
				RegisterFixture("ZombieBungi", "res://Asset/Anime/Character/Zombie/Chapter5/Bungi/Packet/ZombieBungi.tres", "res://Asset/Anime/Character/Zombie/Chapter5/Bungi/Scene/TowerDefenseZombieBungi.tscn");
				RegisterFixture("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
				SetupBattleFixture(manager);
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Boss/Boss/Scene/TowerDefenseZombieBoss.tscn", null, ResourceLoader.CacheMode.Ignore);
				_loadedResources.Add(packedScene);
				Check(GodotObject.IsInstanceValid(packedScene), "The production Zomboss scene must load.");
				TowerDefenseZombieBoss boss = packedScene?.Instantiate<TowerDefenseZombieBoss>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(boss), "The regression must instantiate the production Zomboss character.");
				if (!GodotObject.IsInstanceValid(boss))
				{
					throw new InvalidOperationException("Production Zomboss could not be instantiated.");
				}
				boss.Name = "RuntimeZomboss";
				boss.inGame = true;
				boss.ProcessMode = ProcessModeEnum.Disabled;
				_control.characterNode.AddChild(boss, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(5);
				await VerifyDriverAndMachineStayAttached(boss);
				await VerifyRvOnlyCrushesSelectedLane(manager, boss);
				await VerifyBossSpawnedZombieCanBite(manager, boss);
				await VerifyBossBungiIsTargetable(manager, boss);
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentZombossCompositeRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(_control?.characterNode))
			{
				foreach (Node child in _control.characterNode.GetChildren())
				{
					if (GodotObject.IsInstanceValid(child) && !child.IsQueuedForDeletion())
					{
						child.QueueFree();
					}
				}
			}
			await WaitFrames(6);
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
			if (GodotObject.IsInstanceValid(_control) && !_control.IsQueuedForDeletion())
			{
				_control.QueueFree();
			}
			RestoreFixtures();
			await WaitFrames(5);
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			foreach (Resource loadedResource in _loadedResources)
			{
				loadedResource?.Dispose();
			}
			_loadedResources.Clear();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag = _failures == 0 && _checks >= 24;
		GD.Print($"BUG_DEPARTMENT_ZOMBOSS_COMPOSITE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyDriverAndMachineStayAttached(TowerDefenseZombieBoss boss)
	{
		ZombieBoss bossSprite = boss.sprite as ZombieBoss;
		AdobeAnimateSpriteBase adobeAnimateSpriteBase = bossSprite?.GetNodeOrNull<AdobeAnimateSpriteBase>("%Driver");
		AdobeAnimateSpriteBase arm = bossSprite?.GetNodeOrNull<AdobeAnimateSpriteBase>("%Arm");
		AdobeAnimateSpriteBase rv = bossSprite?.GetNodeOrNull<AdobeAnimateSpriteBase>("%RV");
		Check(GodotObject.IsInstanceValid(bossSprite) && GodotObject.IsInstanceValid(adobeAnimateSpriteBase) && GodotObject.IsInstanceValid(arm) && GodotObject.IsInstanceValid(rv), "The real Zomboss must expose its driver, arm and RV authored sprites.");
		if (!GodotObject.IsInstanceValid(bossSprite) || !GodotObject.IsInstanceValid(adobeAnimateSpriteBase) || !GodotObject.IsInstanceValid(arm) || !GodotObject.IsInstanceValid(rv))
		{
			return;
		}
		Check(adobeAnimateSpriteBase.parentSprite == bossSprite && adobeAnimateSpriteBase.useFollowVisible && adobeAnimateSpriteBase.followParentSpriteLayerId >= 0, "The driver must remain authored as a managed child of the machine body.");
		int num = 0;
		int num2 = 0;
		string[] array = new string[5] { "Enter", "Idle", "HeadIdle", "RV", "Death" };
		foreach (string text in array)
		{
			bossSprite.SetAnimation(text);
			bossSprite.pause = true;
			int num3 = Math.Max(1, (bossSprite.clipRange.Y - bossSprite.clipRange.X) / 8);
			for (int j = bossSprite.clipRange.X; j <= bossSprite.clipRange.Y; j += num3)
			{
				bossSprite.frameIndex = j;
				bossSprite.elapsedTimer = 0.0;
				bossSprite.UpdateChild();
				if (!bossSprite.TryGetInterpolatedSlotPose(adobeAnimateSpriteBase.followParentSpriteLayerId, out var mediaId, out var transform) || mediaId < 0)
				{
					num2++;
					continue;
				}
				num++;
				Vector2 vector = transform.Origin + bossSprite.offset;
				Check(adobeAnimateSpriteBase.Visible && adobeAnimateSpriteBase.Position.DistanceTo(vector) <= 0.5f, $"The driver must be attached to the machine pose in {text} frame {j}; expected={vector}, actual={adobeAnimateSpriteBase.Position}, visible={adobeAnimateSpriteBase.Visible}.");
			}
		}
		Check(num >= 3, $"The real animation sweep must observe attached driver poses; samples={num}.");
		Check(num2 > 0, "The sweep must also cover frames where follow-visible correctly hides the driver slot.");
		await WaitFrames(2);
		bossSprite.frameIndex += 2;
		bossSprite.elapsedTimer = 0.125;
		bossSprite.BatchPhysicsUpdate(0.0);
		Check(arm.frameIndex == bossSprite.frameIndex && Math.Abs(arm.elapsedTimer - bossSprite.elapsedTimer) < 0.0001, "The separated arm render must stay on the machine's exact display frame.");
		Check(rv.frameIndex == bossSprite.frameIndex && Math.Abs(rv.elapsedTimer - bossSprite.elapsedTimer) < 0.0001, "The separated RV render must stay on the machine's exact display frame.");
	}

	private async Task VerifyRvOnlyCrushesSelectedLane(TowerDefenseManager manager, TowerDefenseZombieBoss boss)
	{
		_control.isGameRunning = false;
		TowerDefensePlant target = TowerDefenseManager.GetPacketConfig("PlantSunFlower")?.Plant(RvTargetGrid, playAudio: false) as TowerDefensePlant;
		TowerDefensePlant lowerLane = TowerDefenseManager.GetPacketConfig("PlantSunFlower")?.Plant(RvLowerLaneGrid, playAudio: false) as TowerDefensePlant;
		await WaitFrames(6);
		Check(GodotObject.IsInstanceValid(target) && GodotObject.IsInstanceValid(lowerLane), "The RV regression must place two real Sunflowers in adjacent lanes.");
		if (GodotObject.IsInstanceValid(target) && GodotObject.IsInstanceValid(lowerLane))
		{
			RefreshRegistration(manager, boss, target, lowerLane);
			_control.isGameRunning = true;
			boss.RVSpawn();
			AttackComponent attackComponent = boss.componentManager?.GetRuntime<AttackComponent>("character.attack.4");
			Check(attackComponent != null && !attackComponent.IsReleased, "The production Zomboss must expose its RV smash component.");
			Check(boss.rvPos == RvTargetGrid, $"The only eligible upper-lane plant must be the selected RV target; actual={boss.rvPos}.");
			Check(attackComponent?.TryGetCheckAreaRectangleSize(0, out var _) ?? false, "The production RV attack must expose a rectangle footprint.");
			if (attackComponent != null && attackComponent.TryGetCheckAreaRectangleSize(0, out var size2))
			{
				Check(Math.Abs(size2.X - manager.gridSize.X * 3f) <= 0.01f, $"The RV must retain its authored three-column width; actual={size2.X}.");
				Check(Math.Abs(size2.Y - manager.gridSize.Y) <= 0.01f, $"The RV smash must be limited to the selected lane; actual height={size2.Y}.");
			}
			double targetBefore = target.instance.hitpoints;
			double lowerBefore = lowerLane.instance.hitpoints;
			boss.AnimeEvent("RVAttack", default);
			await WaitFrames(3);
			Check(target.die || target.isDestroy || target.instance.hitpoints < targetBefore, "The selected upper-lane Sunflower must be crushed by the real RV attack.");
			Check(!lowerLane.die && !lowerLane.isDestroy && Math.Abs(lowerLane.instance.hitpoints - lowerBefore) < 0.001, $"The adjacent lower-lane Sunflower must survive; hp={lowerLane.instance.hitpoints}/{lowerBefore}.");
			if (GodotObject.IsInstanceValid(target) && !target.IsQueuedForDeletion())
			{
				target.QueueFree();
			}
			if (GodotObject.IsInstanceValid(lowerLane) && !lowerLane.IsQueuedForDeletion())
			{
				lowerLane.QueueFree();
			}
			await WaitFrames(5);
		}
	}

	private async Task VerifyBossBungiIsTargetable(TowerDefenseManager manager, TowerDefenseZombieBoss boss)
	{
		_control.isGameRunning = false;
		TowerDefensePlant targetPlant = TowerDefenseManager.GetPacketConfig("PlantSunFlower")?.Plant(BungiTargetGrid, playAudio: false) as TowerDefensePlant;
		await WaitFrames(5);
		Check(GodotObject.IsInstanceValid(targetPlant), "The boss Bungi regression must place a real target plant.");
		if (!GodotObject.IsInstanceValid(targetPlant))
		{
			return;
		}
		RefreshRegistration(manager, boss, targetPlant);
		_control.isGameRunning = true;
		boss.BungeeSpawn();
		await WaitFrames(8);
		TowerDefenseZombieBungi towerDefenseZombieBungi = null;
		foreach (Variant bungee in boss.bungeeList)
		{
			if (bungee.AsGodotObject() is TowerDefenseZombieBungi towerDefenseZombieBungi2 && GodotObject.IsInstanceValid(towerDefenseZombieBungi2))
			{
				towerDefenseZombieBungi = towerDefenseZombieBungi2;
				break;
			}
		}
		Check(GodotObject.IsInstanceValid(towerDefenseZombieBungi) && towerDefenseZombieBungi.skipBungeeTarget && !towerDefenseZombieBungi.instance.hypnoses, "Zomboss must summon the real fixed-position, non-hypnotized Bungi variant.");
		if (GodotObject.IsInstanceValid(towerDefenseZombieBungi))
		{
			Check(!towerDefenseZombieBungi.instance.invincible && towerDefenseZombieBungi.instance.canBeCollection, $"The boss-summoned Bungi must be damageable and collectible; invincible={towerDefenseZombieBungi.instance.invincible}, collection={towerDefenseZombieBungi.instance.canBeCollection}.");
			Check((towerDefenseZombieBungi.targetRegistrationComponent?.canProjectileCheck ?? false) && towerDefenseZombieBungi.instance.maskFlags != 0, $"The boss-summoned Bungi must participate in projectile targeting; projectile={towerDefenseZombieBungi.targetRegistrationComponent?.canProjectileCheck}, mask={towerDefenseZombieBungi.instance.maskFlags}.");
			Check(towerDefenseZombieBungi.instance.unUseBuffFlags != -1, "The boss-summoned Bungi must not reject every buff while descending or landed.");
			double hitpoints = towerDefenseZombieBungi.instance.hitpoints;
			towerDefenseZombieBungi.Hurt(50.0, playSplatAudio: true, Vector2.Zero);
			Check(towerDefenseZombieBungi.instance.hitpoints < hitpoints, $"A normal attack must damage the boss-summoned Bungi; hp={towerDefenseZombieBungi.instance.hitpoints}/{hitpoints}.");
		}
	}

	private async Task VerifyBossSpawnedZombieCanBite(TowerDefenseManager manager, TowerDefenseZombieBoss boss)
	{
		_control.isGameRunning = false;
		ZombieBoss bossSprite = boss.sprite as ZombieBoss;
		bossSprite?.SetSpawn(2, 0.0);
		bossSprite?.SetAnimation("Spawn1", loop: false);
		if (GodotObject.IsInstanceValid(bossSprite))
		{
			bossSprite.frameIndex = bossSprite.clipRange.Y;
			bossSprite.elapsedTimer = 0.0;
			bossSprite.UpdateChild();
		}
		await WaitFrames(2);
		Vector2 markerPosition = bossSprite?.GetSpawnMarkerGlobalPos(boss) ?? boss.GetLogicalGlobalPosition();
		int x = Mathf.Clamp(manager.GetMapGridPos(markerPosition).X, 2, manager.gridNum.X);
		Vector2I gridPos = new Vector2I(x, 2);
		TowerDefensePlant targetPlant = TowerDefenseManager.GetPacketConfig("PlantSunFlower")?.Plant(gridPos, playAudio: false) as TowerDefensePlant;
		await WaitFrames(5);
		Check(GodotObject.IsInstanceValid(targetPlant), "The boss normal-spawn regression must place a real target plant.");
		if (!GodotObject.IsInstanceValid(targetPlant))
		{
			return;
		}
		RefreshRegistration(manager, boss, targetPlant);
		_control.isGameRunning = true;
		boss.spawnZomie = "ZombieNormal";
		boss.spawnLine = 2;
		boss.ZombieSpawn();
		await WaitFrames(5);
		TowerDefenseZombie spawnedZombie = null;
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefenseZombie towerDefenseZombie && towerDefenseZombie != boss && !(towerDefenseZombie is TowerDefenseZombieBungi) && GodotObject.IsInstanceValid(towerDefenseZombie))
			{
				spawnedZombie = towerDefenseZombie;
				break;
			}
		}
		Check(GodotObject.IsInstanceValid(spawnedZombie), "Zomboss must create the production normal zombie at its hand marker.");
		if (!GodotObject.IsInstanceValid(spawnedZombie))
		{
			return;
		}
		double battlefieldEntryX = manager.GetMapGroundRight() - (double)(manager.gridSize.X * 0.5f);
		Check((double)spawnedZombie.GetLogicalGlobalPosition().X > battlefieldEntryX && spawnedZombie.HasComponentGameplayUntilBattlefieldEntry && spawnedZombie.IsInsideComponentBattlefield, $"A zombie deliberately placed by Zomboss must activate gameplay at the hand marker; x={spawnedZombie.GetLogicalGlobalPosition().X}, boundary={battlefieldEntryX}, exception={spawnedZombie.HasComponentGameplayUntilBattlefieldEntry}, inside={spawnedZombie.IsInsideComponentBattlefield}.");
		double hitpointsBefore = targetPlant.instance.hitpoints;
		int attackSamples = 0;
		int walkSamplesAfterFirstAttack = 0;
		bool observedAttack = false;
		float firstAttackX = 0f / 0f;
		for (int frame = 0; frame < 480; frame++)
		{
			if (!(targetPlant.instance.hitpoints >= hitpointsBefore))
			{
				break;
			}
			await WaitFrames(1);
			StringName stringName = spawnedZombie.CurrentStateHandle?.StableId ?? null;
			if (stringName == (StringName)"zombie.attack")
			{
				if (!observedAttack)
				{
					firstAttackX = spawnedZombie.GetLogicalGlobalPosition().X;
				}
				observedAttack = true;
				attackSamples++;
			}
			else if (observedAttack && stringName == (StringName)"zombie.walk")
			{
				walkSamplesAfterFirstAttack++;
			}
		}
		Check(observedAttack, $"The boss-spawned zombie must enter attack after reaching the plant; spawn={markerPosition}, plant={targetPlant.GetLogicalGlobalPosition()}, zombie={spawnedZombie.GetLogicalGlobalPosition()}, inside={spawnedZombie.IsInsideComponentBattlefield}.");
		Check((double)firstAttackX > battlefieldEntryX, $"The boss-spawned zombie must start attacking on first contact before the ordinary wave-entry boundary; attackX={firstAttackX}, boundary={battlefieldEntryX}.");
		Check(targetPlant.instance.hitpoints < hitpointsBefore, $"The boss-spawned zombie must bite and damage the plant; hp={targetPlant.instance.hitpoints}/{hitpointsBefore}, attackSamples={attackSamples}, walkAfterAttack={walkSamplesAfterFirstAttack}, zombie={spawnedZombie.GetLogicalGlobalPosition()}.");
		Check(walkSamplesAfterFirstAttack < 12, $"The boss-spawned zombie must not twitch between attack and walk; walkAfterAttack={walkSamplesAfterFirstAttack}, attackSamples={attackSamples}.");
		if (GodotObject.IsInstanceValid(targetPlant) && !targetPlant.IsQueuedForDeletion())
		{
			targetPlant.QueueFree();
		}
		spawnedZombie.SetLogicalGlobalPosition(new Vector2((float)(battlefieldEntryX - 1.0), spawnedZombie.GetLogicalGlobalPosition().Y));
		spawnedZombie.Walk();
		await WaitFrames(6);
		Check(!spawnedZombie.HasComponentGameplayUntilBattlefieldEntry && spawnedZombie.IsInsideComponentBattlefield, "The Zomboss placement exception must be consumed after the zombie enters the normal battlefield.");
		if (GodotObject.IsInstanceValid(spawnedZombie) && !spawnedZombie.IsQueuedForDeletion())
		{
			spawnedZombie.QueueFree();
		}
		await WaitFrames(5);
	}

	private void SetupBattleFixture(TowerDefenseManager manager)
	{
		_control = new ZombossCompositeRuntimeControlStub
		{
			Name = "ZombossCompositeRuntimeControl",
			isGameRunning = false,
			isInit = true,
			levelConfig = new TowerDefenseLevelConfig()
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_control.characterNode = new Node2D
		{
			Name = "CharacterNode"
		};
		_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
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
		towerDefenseBattleFeatureMap.plantGrid.Resize(gridNum.X + 1);
		for (int i = 0; i <= gridNum.X; i++)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			array.Resize(gridNum.Y + 1);
			for (int j = 1; j <= gridNum.Y; j++)
			{
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
				{
					gridPos = new Vector2I(i, j)
				};
				towerDefenseCellInstance.Init(new TowerDefenseCellConfig());
				array[j] = towerDefenseCellInstance;
			}
			towerDefenseBattleFeatureMap.plantGrid[i] = array;
		}
		towerDefenseBattleFeatureMap.iceCapList.Resize(gridNum.Y + 1);
		towerDefenseBattleFeatureMap.lineUse.Resize(gridNum.Y + 1);
		for (int k = 1; k <= gridNum.Y; k++)
		{
			towerDefenseBattleFeatureMap.lineUse[k] = true;
		}
		return towerDefenseBattleFeatureMap;
	}

	private static void RefreshRegistration(TowerDefenseManager manager, params TowerDefenseCharacter[] characters)
	{
		foreach (TowerDefenseCharacter towerDefenseCharacter in characters)
		{
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				manager.CharacterUnregister(towerDefenseCharacter);
				manager.CharacterRegister(towerDefenseCharacter);
			}
		}
	}

	private void RegisterFixture(string key, string packetPath, string characterPath)
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
		if (instance.TOWERDEFENSE_CHARCATERS.TryGetValue(key, out var value2))
		{
			_previousCharacters[key] = value2;
		}
		else
		{
			_missingCharacters.Add(key);
		}
		TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>(packetPath, null, ResourceLoader.CacheMode.Ignore);
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(characterPath, null, ResourceLoader.CacheMode.Ignore);
		_loadedResources.Add(towerDefensePacketConfig);
		_loadedResources.Add(packedScene);
		instance.TOWERDEFENSE_PACKETS[key] = towerDefensePacketConfig;
		instance.TOWERDEFENSE_CHARCATERS[key] = packedScene;
	}

	private void RestoreFixtures()
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

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugDepartmentZombossCompositeRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupBattleFixture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshRegistration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Array, "characters", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterFixture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "packetPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "characterPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.RefreshRegistration && args.Count == 2)
		{
			RefreshRegistration(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertToSystemArrayOfGodotObject<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterFixture && args.Count == 3)
		{
			RegisterFixture(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreFixtures && args.Count == 0)
		{
			RestoreFixtures();
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
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.RefreshRegistration && args.Count == 2)
		{
			RefreshRegistration(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertToSystemArrayOfGodotObject<TowerDefenseCharacter>(in args[1]));
			ret = default;
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
		if (method == MethodName.CreateMapFeature)
		{
			return true;
		}
		if (method == MethodName.RefreshRegistration)
		{
			return true;
		}
		if (method == MethodName.RegisterFixture)
		{
			return true;
		}
		if (method == MethodName.RestoreFixtures)
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
			_control = VariantUtils.ConvertTo<ZombossCompositeRuntimeControlStub>(in value);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
			_control = value3.As<ZombossCompositeRuntimeControlStub>();
		}
		if (info.TryGetProperty(PropertyName._mapFeature, out var value4))
		{
			_mapFeature = value4.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName._mapControl, out var value5))
		{
			_mapControl = value5.As<TowerDefenseMapControl>();
		}
	}
}
