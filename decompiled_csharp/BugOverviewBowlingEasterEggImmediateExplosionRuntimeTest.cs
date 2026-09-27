using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewBowlingEasterEggImmediateExplosionRuntimeTest.cs")]
public class BugOverviewBowlingEasterEggImmediateExplosionRuntimeTest : Node
{
	private sealed class ScenarioSetupException : Exception
	{
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName RegisterRealFixtures = "RegisterRealFixtures";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

		public static readonly StringName RestoreRealFixtures = "RestoreRealFixtures";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _previousBank = "_previousBank";

		public static readonly StringName _bankWasMissing = "_bankWasMissing";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string EggPacketPath = "res://Asset/Anime/Character/Plant/Star/BowlingEasterEgg/Packet/PlantBowlingEasterEgg.tres";

	private const string EggScenePath = "res://Asset/Anime/Character/Plant/Star/BowlingEasterEgg/Scene/TowerDefensePlantBowlingEasterEgg.tscn";

	private const string BoomPacketPath = "res://Asset/Anime/Character/Plant/Star/WallnutShooter/Packet/PlantBowlingWallnutShooterBoom.tres";

	private const string BoomScenePath = "res://Asset/Anime/Character/Plant/Star/WallnutShooter/Scene/BowlingBoom/TowerDefensePlantBowlingWallnutShooterBoom.tscn";

	private const string ZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string FocusedBank = "BugOverviewBowlingEasterEggBoomOnly";

	private static readonly Vector2I CollisionGrid = new Vector2I(4, 3);

	private static readonly Vector2I DirectCollisionGrid = new Vector2I(5, 3);

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private TowerDefensePacketBankData _previousBank;

	private bool _bankWasMissing;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BowlingEasterEggImmediateExplosionControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefensePlantBowlingWallnutShooterBoom directBoom = null;
		TowerDefenseZombie directZombie = null;
		TowerDefensePlantBowlingEasterEgg egg = null;
		TowerDefensePlantBowlingWallnutShooterBoom boom = null;
		TowerDefenseZombie zombie = null;
		string directBoomConfigName = null;
		bool directBoomBowlingRuntimeActive = false;
		bool directBoomExplodeRuntimeActive = false;
		bool directBoomDestroyObserved = false;
		int directBoomHitNumAtDestroy = -1;
		Vector2I directBoomGridAtDestroy = new Vector2I(-1, -1);
		Vector2 directBoomPositionAtDestroy = new Vector2(0f / 0f, 0f / 0f);
		new Vector2(0f / 0f, 0f / 0f);
		double directZombieHealthAfterExplosion = 0.0 / 0.0;
		int boomSpawnCount = 0;
		string boomConfigName = null;
		bool boomBowlingRuntimeActive = false;
		bool boomExplodeRuntimeActive = false;
		bool boomDestroyObserved = false;
		int boomHitNumAtDestroy = -1;
		Vector2I boomGridAtDestroy = new Vector2I(-1, -1);
		Vector2 boomPositionAtDestroy = new Vector2(0f / 0f, 0f / 0f);
		new Vector2(0f / 0f, 0f / 0f);
		double zombieHealthAfterExplosion = 0.0 / 0.0;
		try
		{
			_ = 5;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance), "TowerDefenseManager and ResourceManager autoloads must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new ScenarioSetupException();
				}
				control = new BowlingEasterEggImmediateExplosionControlStub
				{
					Name = "BowlingEasterEggImmediateExplosionControl",
					isGameRunning = false,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = new Vector2(0f, 100f);
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				RegisterRealFixtures();
				control.characterNode.ChildEnteredTree += (Node child) =>
				{
					TowerDefensePlantBowlingWallnutShooterBoom spawnedBoom = child as TowerDefensePlantBowlingWallnutShooterBoom;
					if (spawnedBoom != null)
					{
						boomSpawnCount++;
						boom = spawnedBoom;
						boomConfigName = spawnedBoom.config?.name;
						spawnedBoom.OnDestroy += (TowerDefenseCharacter _) =>
						{
							BowlingComponent bowlingComponent3 = spawnedBoom.componentManager?.GetRuntime<BowlingComponent>();
							ExplodeComponent explodeComponent2 = spawnedBoom.componentManager?.GetRuntime<ExplodeComponent>();
							boomBowlingRuntimeActive = bowlingComponent3 != null && !bowlingComponent3.IsReleased;
							boomExplodeRuntimeActive = explodeComponent2 != null && !explodeComponent2.IsReleased;
							boomHitNumAtDestroy = bowlingComponent3?.hitNum ?? (-1);
							boomGridAtDestroy = spawnedBoom.gridPos;
							boomPositionAtDestroy = spawnedBoom.GlobalPosition;
							boomDestroyObserved = true;
						};
					}
				};
				TowerDefensePacketConfig eggPacket = LoadPacket("res://Asset/Anime/Character/Plant/Star/BowlingEasterEgg/Packet/PlantBowlingEasterEgg.tres");
				TowerDefensePacketConfig boomPacket = LoadPacket("res://Asset/Anime/Character/Plant/Star/WallnutShooter/Packet/PlantBowlingWallnutShooterBoom.tres");
				TowerDefensePacketConfig zombiePacket = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres");
				Check(GodotObject.IsInstanceValid(eggPacket) && eggPacket.saveKey == "PlantBowlingEasterEgg" && !eggPacket.plantUseCell && GodotObject.IsInstanceValid(boomPacket) && boomPacket.saveKey == "PlantBowlingWallnutShooterBoom" && !boomPacket.plantUseCell && GodotObject.IsInstanceValid(zombiePacket) && zombiePacket.saveKey == "ZombieNormal", "The scenario must load the authored Easter Egg, Boom, and ZombieNormal packets.");
				directZombie = zombiePacket?.Plant(DirectCollisionGrid, playAudio: false) as TowerDefenseZombie;
				await WaitFrames(6);
				Check(GodotObject.IsInstanceValid(directZombie) && directZombie.config?.name == "ZombieNormal", "The direct close-placement fixture must instantiate a real normal zombie.");
				if (!GodotObject.IsInstanceValid(directZombie))
				{
					throw new ScenarioSetupException();
				}
				directZombie.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(DirectCollisionGrid);
				directZombie.gridPos = DirectCollisionGrid;
				directZombie.instance.hitpointsBase = 10000.0;
				directZombie.instance.hitpointsSave = 10000.0;
				directZombie.instance.hitpoints = 10000.0;
				double directZombieHealthBeforeExplosion = directZombie.instance.hitpoints;
				Vector2 directCollisionWorldPosition = directZombie.GlobalPosition;
				control.isGameRunning = true;
				directBoom = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Star/WallnutShooter/Scene/BowlingBoom/TowerDefensePlantBowlingWallnutShooterBoom.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefensePlantBowlingWallnutShooterBoom>(PackedScene.GenEditState.Disabled);
				if (GodotObject.IsInstanceValid(directBoom))
				{
					directBoomConfigName = directBoom.config?.name;
					directBoom.OnDestroy += (TowerDefenseCharacter _) =>
					{
						BowlingComponent bowlingComponent3 = directBoom.componentManager?.GetRuntime<BowlingComponent>();
						ExplodeComponent explodeComponent2 = directBoom.componentManager?.GetRuntime<ExplodeComponent>();
						directBoomBowlingRuntimeActive = bowlingComponent3 != null && !bowlingComponent3.IsReleased;
						directBoomExplodeRuntimeActive = explodeComponent2 != null && !explodeComponent2.IsReleased;
						directBoomHitNumAtDestroy = bowlingComponent3?.hitNum ?? (-1);
						directBoomGridAtDestroy = directBoom.gridPos;
						directBoomPositionAtDestroy = directBoom.GlobalPosition;
						directBoomDestroyObserved = true;
					};
					directBoom.packet = boomPacket;
					directBoom.gridPos = DirectCollisionGrid;
					directBoom.cost = boomPacket.characterConfig.cost;
					directBoom.groundHeight = directZombie.groundHeight;
					directBoom.z = directBoom.groundHeight;
					directBoom.SetGlobalPositionForPhysicsFrame(directCollisionWorldPosition, Engine.GetPhysicsFrames());
					control.characterNode.AddChild(directBoom, forceReadableName: false, InternalMode.Disabled);
				}
				await WaitFrames(6);
				if (GodotObject.IsInstanceValid(directBoom) && !directBoomDestroyObserved)
				{
					BowlingComponent bowlingComponent = directBoom.componentManager?.GetRuntime<BowlingComponent>();
					ExplodeComponent explodeComponent = directBoom.componentManager?.GetRuntime<ExplodeComponent>();
					directBoomBowlingRuntimeActive = bowlingComponent != null && !bowlingComponent.IsReleased;
					directBoomExplodeRuntimeActive = explodeComponent != null && !explodeComponent.IsReleased;
					directBoom.SetGlobalPositionForPhysicsFrame(directCollisionWorldPosition, Engine.GetPhysicsFrames());
					directBoom.gridPos = DirectCollisionGrid;
					if (bowlingComponent != null && !bowlingComponent.IsReleased)
					{
						bowlingComponent.SetAlive(alive: true);
						bowlingComponent.isRoll = true;
						bowlingComponent.Refresh();
						bowlingComponent.ChangeCheck();
					}
				}
				await WaitFrames(6);
				if (GodotObject.IsInstanceValid(directZombie?.instance))
				{
					directZombieHealthAfterExplosion = directZombie.instance.hitpoints;
				}
				Check((directBoomConfigName == "PlantBowlingWallnutShooterBoom") & directBoomBowlingRuntimeActive & directBoomExplodeRuntimeActive, "The direct close-placement Boom must expose the authored Bowling and Explode runtimes.");
				Check(directBoomDestroyObserved && directBoomHitNumAtDestroy == 1 && directBoomGridAtDestroy == DirectCollisionGrid && directBoomPositionAtDestroy.IsEqualApprox(directCollisionWorldPosition), "The directly placed Boom must be consumed by its birth-cell collision instead of rebounding first.");
				Check(double.IsFinite(directZombieHealthAfterExplosion) && directZombieHealthAfterExplosion < directZombieHealthBeforeExplosion, "The directly placed Boom must apply ExplodeComponent damage to the close zombie.");
				if (GodotObject.IsInstanceValid(directBoom))
				{
					directBoom.QueueFree();
				}
				if (GodotObject.IsInstanceValid(directZombie))
				{
					directZombie.QueueFree();
				}
				directBoom = null;
				directZombie = null;
				boomSpawnCount = 0;
				boom = null;
				boomConfigName = null;
				boomBowlingRuntimeActive = false;
				boomExplodeRuntimeActive = false;
				boomDestroyObserved = false;
				boomHitNumAtDestroy = -1;
				boomGridAtDestroy = new Vector2I(-1, -1);
				boomPositionAtDestroy = new Vector2(0f / 0f, 0f / 0f);
				control.isGameRunning = false;
				await WaitFrames(3);
				egg = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Star/BowlingEasterEgg/Scene/TowerDefensePlantBowlingEasterEgg.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefensePlantBowlingEasterEgg>(PackedScene.GenEditState.Disabled);
				if (GodotObject.IsInstanceValid(egg))
				{
					egg.packet = eggPacket;
					egg.gridPos = CollisionGrid;
					egg.cost = eggPacket.characterConfig.cost;
					TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(CollisionGrid);
					if (GodotObject.IsInstanceValid(mapCell))
					{
						egg.groundHeight = mapCell.GetGroundHeight();
					}
					egg.z = egg.groundHeight;
					egg.SetGlobalPositionForPhysicsFrame(TowerDefenseManager.GetMapCellPlantPos(CollisionGrid), Engine.GetPhysicsFrames());
					control.characterNode.AddChild(egg, forceReadableName: false, InternalMode.Disabled);
				}
				zombie = zombiePacket?.Plant(CollisionGrid, playAudio: false) as TowerDefenseZombie;
				await WaitFrames(6);
				Check(GodotObject.IsInstanceValid(egg) && egg.config?.name == "PlantBowlingEasterEgg" && GodotObject.IsInstanceValid(zombie) && zombie.config?.name == "ZombieNormal", "The live fixture must instantiate the real Easter Egg and normal zombie scenes.");
				if (!GodotObject.IsInstanceValid(egg) || !GodotObject.IsInstanceValid(zombie))
				{
					throw new ScenarioSetupException();
				}
				egg.ProcessMode = ProcessModeEnum.Disabled;
				zombie.ProcessMode = ProcessModeEnum.Disabled;
				egg.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(CollisionGrid);
				egg.gridPos = CollisionGrid;
				zombie.GlobalPosition = egg.GlobalPosition;
				zombie.gridPos = CollisionGrid;
				zombie.instance.hitpointsBase = 10000.0;
				zombie.instance.hitpointsSave = 10000.0;
				zombie.instance.hitpoints = 10000.0;
				Vector2 collisionWorldPosition = egg.GlobalPosition;
				BowlingComponent bowlingComponent2 = egg.componentManager?.GetRuntime<BowlingComponent>();
				Check(bowlingComponent2 != null && !bowlingComponent2.IsReleased && bowlingComponent2.hitLineUse && bowlingComponent2.useParentHitBox && egg.HasHitBox, "The real Easter Egg must expose its authored same-line parent-HitBox Bowling runtime.");
				if (bowlingComponent2?.IsReleased ?? true)
				{
					throw new ScenarioSetupException();
				}
				double hitpoints = zombie.instance.hitpoints;
				bowlingComponent2.SetAlive(alive: true);
				bowlingComponent2.isRoll = true;
				bowlingComponent2.ChangeCheck();
				Check(bowlingComponent2.hitNum == 1 && zombie.instance.hitpoints < hitpoints && !bowlingComponent2.isRoll && egg.moveComponent.velocity.IsZeroApprox(), "The real parent-HitBox collision must damage the zombie and start the Easter Egg Open transformation.");
				Check(egg.sprite.clip == "Open", "The first collision must enter the authored Open clip before transformation.");
				double healthAfterEggHit = zombie.instance.hitpoints;
				double zombieHealthBeforeExplosion = healthAfterEggHit;
				zombie.ProcessMode = ProcessModeEnum.Inherit;
				control.isGameRunning = true;
				egg.packetBank = "BugOverviewBowlingEasterEggBoomOnly";
				egg.AnimeCompleted("Open");
				await WaitFrames(6);
				if (GodotObject.IsInstanceValid(zombie?.instance))
				{
					zombieHealthAfterExplosion = zombie.instance.hitpoints;
				}
				Check(boomSpawnCount == 1, "The real Easter Egg transformation must add exactly one Boom to the live character tree.");
				Check(boomConfigName == "PlantBowlingWallnutShooterBoom", "The transformed character must use the authored explosive bowling Wall-nut config.");
				Check(boomBowlingRuntimeActive & boomExplodeRuntimeActive, "The transformed real Boom must expose active Bowling and Explode runtimes.");
				Check(boomGridAtDestroy == CollisionGrid && boomPositionAtDestroy.IsEqualApprox(collisionWorldPosition), "The Boom must be born in the collision cell instead of moving to a later column first.");
				Check(boomDestroyObserved, "The Boom must be consumed by that first collision, not rebound once before exploding.");
				Check(boomHitNumAtDestroy == 1, "The transformed Boom must resolve exactly one bowling collision in the authored overlap.");
				Check(double.IsFinite(zombieHealthBeforeExplosion) && double.IsFinite(zombieHealthAfterExplosion) && Math.Abs(zombieHealthBeforeExplosion - healthAfterEggHit) <= 0.001 && zombieHealthAfterExplosion < zombieHealthBeforeExplosion && zombie.ProcessMode != ProcessModeEnum.Disabled, "The first overlap must skip bowling impact damage and apply only the deferred ExplodeComponent range damage.");
			}
			catch (ScenarioSetupException)
			{
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewBowlingEasterEggImmediateExplosionRuntimeTest] Unexpected exception: {value}");
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
			mapFeature?.Destroy();
			if (GodotObject.IsInstanceValid(mapControl))
			{
				mapControl.Free();
			}
			if (GodotObject.IsInstanceValid(directBoom))
			{
				directBoom.QueueFree();
			}
			if (GodotObject.IsInstanceValid(directZombie))
			{
				directZombie.QueueFree();
			}
			if (GodotObject.IsInstanceValid(egg))
			{
				egg.QueueFree();
			}
			if (GodotObject.IsInstanceValid(boom))
			{
				boom.QueueFree();
			}
			if (GodotObject.IsInstanceValid(zombie))
			{
				zombie.QueueFree();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			RestoreRealFixtures();
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 17;
		GD.Print($"BOWLING_EASTER_EGG_IMMEDIATE_EXPLOSION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			gridNum = gridNum,
			gridBeginPos = new Vector2(0f, 100f),
			gridSize = new Vector2(100f, 76f),
			plantOffset = 50.0
		};
		towerDefenseMapConfig.cellConfig.Add(new TowerDefenseCellConfig
		{
			pos = new Vector4I(1, 1, gridNum.X, gridNum.Y)
		});
		for (int i = 1; i <= gridNum.Y; i++)
		{
			towerDefenseMapConfig.lineUse.Add(i);
		}
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig
		});
		towerDefenseBattleFeatureMap.PlantGridInit();
		return towerDefenseBattleFeatureMap;
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("PlantBowlingEasterEgg", "res://Asset/Anime/Character/Plant/Star/BowlingEasterEgg/Packet/PlantBowlingEasterEgg.tres");
		RegisterPacket("PlantBowlingWallnutShooterBoom", "res://Asset/Anime/Character/Plant/Star/WallnutShooter/Packet/PlantBowlingWallnutShooterBoom.tres");
		RegisterPacket("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres");
		RegisterCharacter("PlantBowlingEasterEgg", "res://Asset/Anime/Character/Plant/Star/BowlingEasterEgg/Scene/TowerDefensePlantBowlingEasterEgg.tscn");
		RegisterCharacter("PlantBowlingWallnutShooterBoom", "res://Asset/Anime/Character/Plant/Star/WallnutShooter/Scene/BowlingBoom/TowerDefensePlantBowlingWallnutShooterBoom.tscn");
		RegisterCharacter("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
		ResourceManager instance = ResourceManager.Instance;
		if (instance.TOWERDEFENSE_PACKETBANKS.TryGetValue("BugOverviewBowlingEasterEggBoomOnly", out var value))
		{
			_previousBank = value;
		}
		else
		{
			_bankWasMissing = true;
		}
		instance.TOWERDEFENSE_PACKETBANKS["BugOverviewBowlingEasterEggBoomOnly"] = new TowerDefensePacketBankData
		{
			category = new Dictionary { ["White"] = new Array<string> { "PlantBowlingWallnutShooterBoom" } }
		};
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
		if (_bankWasMissing)
		{
			instance.TOWERDEFENSE_PACKETBANKS.Remove("BugOverviewBowlingEasterEggBoomOnly");
		}
		else
		{
			instance.TOWERDEFENSE_PACKETBANKS["BugOverviewBowlingEasterEggBoomOnly"] = _previousBank;
		}
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
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
			GD.PushError("[BugOverviewBowlingEasterEggImmediateExplosionRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterRealFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreRealFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.LoadPacket)
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
		if (name == PropertyName._previousBank)
		{
			_previousBank = VariantUtils.ConvertTo<TowerDefensePacketBankData>(in value);
			return true;
		}
		if (name == PropertyName._bankWasMissing)
		{
			_bankWasMissing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
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
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._previousBank)
		{
			value = VariantUtils.CreateFrom(in _previousBank);
			return true;
		}
		if (name == PropertyName._bankWasMissing)
		{
			value = VariantUtils.CreateFrom(in _bankWasMissing);
			return true;
		}
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._previousBank, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._bankWasMissing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._previousBank, Variant.From(in _previousBank));
		info.AddProperty(PropertyName._bankWasMissing, Variant.From(in _bankWasMissing));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._previousBank, out var value))
		{
			_previousBank = value.As<TowerDefensePacketBankData>();
		}
		if (info.TryGetProperty(PropertyName._bankWasMissing, out var value2))
		{
			_bankWasMissing = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value3))
		{
			_checks = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value4))
		{
			_failures = value4.As<int>();
		}
	}
}
