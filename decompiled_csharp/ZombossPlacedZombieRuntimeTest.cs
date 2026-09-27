using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/ZombossPlacedZombieRuntimeTest.cs")]
public class ZombossPlacedZombieRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InstantiateBoss = "InstantiateBoss";

		public static readonly StringName PrepareSpawnPosition = "PrepareSpawnPosition";

		public static readonly StringName CreateTargetPlant = "CreateTargetPlant";

		public static readonly StringName InvokeZombieSpawn = "InvokeZombieSpawn";

		public static readonly StringName FindPlacedZombie = "FindPlacedZombie";

		public static readonly StringName SetupBattleFixture = "SetupBattleFixture";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName RefreshRegistration = "RefreshRegistration";

		public static readonly StringName RegisterFixture = "RegisterFixture";

		public static readonly StringName RestoreFixtures = "RestoreFixtures";

		public static readonly StringName ReadRenderedTransform = "ReadRenderedTransform";

		public static readonly StringName Check = "Check";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _control = "_control";

		public static readonly StringName _mapControl = "_mapControl";

		public static readonly StringName _mapFeature = "_mapFeature";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string BossScenePath = "res://Asset/Anime/Character/Zombie/Boss/Boss/Scene/TowerDefenseZombieBoss.tscn";

	private const string BossDaveScenePath = "res://Asset/Anime/Character/Zombie/Boss/BossDave/Scene/TowerDefenseZombieBossDave.tscn";

	private const string FootballZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Football/Packet/ZombieFootball.tres";

	private const string FootballZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Football/Scene/Normal/TowerDefenseZombieFootball.tscn";

	private const string PaperZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Paper/Packet/ZombiePaper.tres";

	private const string PaperZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Paper/Scene/TowerDefenseZombiePaper.tscn";

	private const string SunflowerPacketPath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Packet/PlantSunFlower.tres";

	private const string SunflowerScenePath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn";

	private const int SpawnLine = 2;

	private int _checks;

	private readonly List<string> _failures = new List<string>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private readonly List<Resource> _loadedResources = new List<Resource>();

	private ZombossPlacedZombieRuntimeControlStub _control;

	private TowerDefenseMapControl _mapControl;

	private TowerDefenseBattleFeatureMap _mapFeature;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		AdobeAnimateRenderBackend previousBackend = Global.Instance.adobeAnimateRenderBackend;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance), "测试必须能够使用正式战斗与资源管理器。");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("缺少测试所需的自动加载管理器。");
				}
				Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
				RegisterFixture("ZombieFootball", "res://Asset/Anime/Character/Zombie/Chapter1/Football/Packet/ZombieFootball.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Football/Scene/Normal/TowerDefenseZombieFootball.tscn");
				RegisterFixture("ZombiePaper", "res://Asset/Anime/Character/Zombie/Chapter1/Paper/Packet/ZombiePaper.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Paper/Scene/TowerDefenseZombiePaper.tscn");
				RegisterFixture("PlantSunFlower", "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Packet/PlantSunFlower.tres", "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn");
				SetupBattleFixture(manager);
				await VerifyBossPlacement(manager, "res://Asset/Anime/Character/Zombie/Boss/Boss/Scene/TowerDefenseZombieBoss.tscn", isDave: false, "ZombieFootball", "普通僵王");
				await VerifyBossPlacement(manager, "res://Asset/Anime/Character/Zombie/Boss/BossDave/Scene/TowerDefenseZombieBossDave.tscn", isDave: true, "ZombiePaper", "戴夫僵王");
			}
			catch (Exception value)
			{
				_failures.Add($"运行测试出现异常：{value}");
			}
		}
		finally
		{
			Global.Instance.adobeAnimateRenderBackend = previousBackend;
			await CleanupBattleFixture(manager, previousControl, previousGridBegin, previousGridSize, previousGridNum);
		}
		Finish();
	}

	private async Task VerifyBossPlacement(TowerDefenseManager manager, string scenePath, bool isDave, string packetName, string label)
	{
		TowerDefenseZombie boss = InstantiateBoss(scenePath, isDave);
		Check(GodotObject.IsInstanceValid(boss), label + "必须从正式角色场景实例化。");
		if (!GodotObject.IsInstanceValid(boss))
		{
			return;
		}
		boss.Name = label + "Runtime";
		boss.inGame = true;
		boss.ProcessMode = ProcessModeEnum.Disabled;
		_control.characterNode.AddChild(boss, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(6);
		AdobeAnimateSprite sprite = boss.sprite;
		Check(GodotObject.IsInstanceValid(sprite), label + "必须提供正式 Adobe Animate 贴图。");
		if (!GodotObject.IsInstanceValid(sprite))
		{
			boss.QueueFree();
			await WaitFrames(3);
			return;
		}
		Vector2 spawnPosition = PrepareSpawnPosition(boss, sprite, isDave);
		Vector2I expectedGridPosition = manager.GetMapGridPos(spawnPosition);
		expectedGridPosition.Y = 2;
		Vector2I vector2I = new Vector2I(Mathf.Clamp(expectedGridPosition.X, 1, manager.gridNum.X), 2);
		Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(vector2I);
		TowerDefensePlant targetPlant = CreateTargetPlant(mapCellPlantPos, vector2I);
		Check(GodotObject.IsInstanceValid(targetPlant), label + "回归必须创建真实向日葵目标。");
		if (!GodotObject.IsInstanceValid(targetPlant))
		{
			boss.QueueFree();
			await WaitFrames(3);
			return;
		}
		await WaitFrames(6);
		RefreshRegistration(manager, boss, targetPlant);
		_control.isGameRunning = true;
		InvokeZombieSpawn(boss, isDave, packetName);
		TowerDefenseZombie spawnedZombie = FindPlacedZombie(boss, packetName);
		Check(GodotObject.IsInstanceValid(spawnedZombie), label + "必须从正式放置入口创建" + packetName + "。");
		if (!GodotObject.IsInstanceValid(spawnedZombie))
		{
			await CleanupCharacters(targetPlant, boss);
			return;
		}
		Check(spawnedZombie.GetLogicalGlobalPosition().DistanceTo(spawnPosition) <= 0.5f, $"{label}放置后的逻辑坐标必须立即停在手部落点；期望={spawnPosition}，实际={spawnedZombie.GetLogicalGlobalPosition()}。");
		Check(spawnedZombie.gridPos == expectedGridPosition, $"{label}放置后的格坐标必须立即对应手部落点；期望={expectedGridPosition}，实际={spawnedZombie.gridPos}。");
		Check(spawnedZombie.HasComponentGameplayUntilBattlefieldEntry && spawnedZombie.IsInsideComponentBattlefield, label + "放置的僵尸必须在普通进场边界前立即启用组件玩法。");
		Check(GodotObject.IsInstanceValid(spawnedZombie.sprite), label + "放置的复杂僵尸必须提供正式 Adobe Animate 身体。");
		Check(!(await ObserveWalkStateInCurrentDeferredQueue(spawnedZombie)), label + "放置的复杂僵尸必须先完成一个物理姿态边界，再进入 Walk 动画。");
		bool renderStayedAligned = GodotObject.IsInstanceValid(spawnedZombie.sprite);
		double hitpointsBefore = targetPlant.instance.hitpoints;
		bool observedAttack = false;
		float maximumFrameDisplacement = 0f;
		Vector2 previousPosition = spawnedZombie.GetLogicalGlobalPosition();
		for (int frame = 0; frame < 480; frame++)
		{
			if (!(targetPlant.instance.hitpoints >= hitpointsBefore))
			{
				break;
			}
			await WaitFrames(1);
			Vector2 logicalGlobalPosition = spawnedZombie.GetLogicalGlobalPosition();
			maximumFrameDisplacement = Mathf.Max(maximumFrameDisplacement, logicalGlobalPosition.DistanceTo(previousPosition));
			previousPosition = logicalGlobalPosition;
			if (spawnedZombie.CurrentStateHandle?.StableId == "zombie.attack")
			{
				observedAttack = true;
			}
			if (GodotObject.IsInstanceValid(spawnedZombie.sprite))
			{
				renderStayedAligned &= ReadRenderedTransform(spawnedZombie.sprite).IsEqualApprox(spawnedZombie.sprite.GlobalTransform);
			}
		}
		Check(renderStayedAligned, label + "放置僵尸的 GPU 贴图必须持续跟随逻辑身体变换。");
		Check(maximumFrameDisplacement < manager.gridSize.X * 0.5f, $"{label}放置僵尸进入移动与攻击时不得瞬移；最大单帧位移={maximumFrameDisplacement}。");
		Check(observedAttack, label + "放置的僵尸必须识别最右侧有效格的植物并进入攻击状态。");
		Check(targetPlant.instance.hitpoints < hitpointsBefore, $"{label}放置的僵尸必须实际啃咬植物；生命={targetPlant.instance.hitpoints}/{hitpointsBefore}。");
		await CleanupCharacters(spawnedZombie, targetPlant, boss);
		_control.isGameRunning = false;
	}

	private TowerDefenseZombie InstantiateBoss(string scenePath, bool isDave)
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Ignore);
		_loadedResources.Add(packedScene);
		if (!isDave)
		{
			return packedScene?.Instantiate<TowerDefenseZombieBoss>(PackedScene.GenEditState.Disabled);
		}
		return packedScene?.Instantiate<TowerDefenseZombieBossDave>(PackedScene.GenEditState.Disabled);
	}

	private Vector2 PrepareSpawnPosition(TowerDefenseZombie boss, AdobeAnimateSprite bossSprite, bool isDave)
	{
		Vector2 vector;
		if (isDave)
		{
			ZombieBossDave obj = bossSprite as ZombieBossDave;
			obj?.SetSpawn(2, 0.0);
			bossSprite.SetAnimation("Spawn1", loop: false);
			bossSprite.frameIndex = bossSprite.clipRange.Y;
			bossSprite.elapsedTimer = 0.0;
			bossSprite.UpdateChild();
			vector = obj?.GetSpawnMarkerGlobalPos(boss) ?? boss.GetLogicalGlobalPosition();
		}
		else
		{
			ZombieBoss obj2 = bossSprite as ZombieBoss;
			obj2?.SetSpawn(2, 0.0);
			bossSprite.SetAnimation("Spawn1", loop: false);
			bossSprite.frameIndex = bossSprite.clipRange.Y;
			bossSprite.elapsedTimer = 0.0;
			bossSprite.UpdateChild();
			vector = obj2?.GetSpawnMarkerGlobalPos(boss) ?? boss.GetLogicalGlobalPosition();
		}
		return new Vector2(vector.X, TowerDefenseManager.GetMapCellPlantPos(new Vector2I(0, 2)).Y);
	}

	private TowerDefensePlant CreateTargetPlant(Vector2 position, Vector2I gridPosition)
	{
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantSunFlower");
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return null;
		}
		TowerDefensePlant towerDefensePlant = packetConfig.Create(_control.characterNode.ToLocal(position), gridPosition) as TowerDefensePlant;
		if (!GodotObject.IsInstanceValid(towerDefensePlant))
		{
			return null;
		}
		_control.characterNode.AddChild(towerDefensePlant, forceReadableName: false, InternalMode.Disabled);
		towerDefensePlant.SetLogicalGlobalPosition(position);
		towerDefensePlant.cell = TowerDefenseManager.GetMapCell(gridPosition);
		return towerDefensePlant;
	}

	private static void InvokeZombieSpawn(TowerDefenseZombie boss, bool isDave, string packetName)
	{
		if (isDave && boss is TowerDefenseZombieBossDave towerDefenseZombieBossDave)
		{
			towerDefenseZombieBossDave.spawnZomie = packetName;
			towerDefenseZombieBossDave.spawnLine = 2;
			towerDefenseZombieBossDave.ZombieSpawn();
		}
		else if (boss is TowerDefenseZombieBoss towerDefenseZombieBoss)
		{
			towerDefenseZombieBoss.spawnZomie = packetName;
			towerDefenseZombieBoss.spawnLine = 2;
			towerDefenseZombieBoss.ZombieSpawn();
		}
	}

	private TowerDefenseZombie FindPlacedZombie(TowerDefenseZombie boss, string packetName)
	{
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefenseZombie towerDefenseZombie && towerDefenseZombie != boss && towerDefenseZombie.config?.name == packetName && GodotObject.IsInstanceValid(towerDefenseZombie))
			{
				return towerDefenseZombie;
			}
		}
		return null;
	}

	private void SetupBattleFixture(TowerDefenseManager manager)
	{
		_control = new ZombossPlacedZombieRuntimeControlStub
		{
			Name = "ZombossPlacedZombieRuntimeControl",
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

	private async Task CleanupCharacters(params TowerDefenseCharacter[] characters)
	{
		foreach (TowerDefenseCharacter towerDefenseCharacter in characters)
		{
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.IsQueuedForDeletion())
			{
				towerDefenseCharacter.QueueFree();
			}
		}
		await WaitFrames(6);
	}

	private async Task CleanupBattleFixture(TowerDefenseManager manager, TowerDefenseControlNew previousControl, Vector2 previousGridBegin, Vector2 previousGridSize, Vector2I previousGridNum)
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

	private static Transform2D ReadRenderedTransform(AdobeAnimateSprite sprite)
	{
		FieldInfo? field = typeof(AdobeAnimateSprite).GetField("_renderGlobalTransform", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field == null)
		{
			throw new MissingFieldException(typeof(AdobeAnimateSprite).FullName, "_renderGlobalTransform");
		}
		return (Transform2D)field.GetValue(sprite);
	}

	private static async Task<bool> ObserveWalkStateInCurrentDeferredQueue(TowerDefenseZombie zombie)
	{
		TaskCompletionSource<bool> completion = new TaskCompletionSource<bool>();
		Callable.From(() =>
		{
			completion.TrySetResult(GodotObject.IsInstanceValid(zombie) && zombie.CurrentStateHandle?.StableId == "zombie.walk");
		}).CallDeferred();
		return await completion.Task;
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
			_failures.Add(message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PushError("[ZombossPlacedZombieRuntimeTest] " + failure);
		}
		bool flag = _failures.Count == 0 && _checks == 27;
		GD.Print($"ZOMBOSS_PLACED_ZOMBIE_RESULT passed={flag} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(14)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.InstantiateBoss, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "scenePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.PrepareSpawnPosition, new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "bossSprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateTargetPlant, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.InvokeZombieSpawn, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.FindPlacedZombie, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.SetupBattleFixture, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateMapFeature, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RefreshRegistration, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Array, "characters", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterFixture, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "packetPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "characterPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RestoreFixtures, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ReadRenderedTransform, new Godot.Bridge.PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Finish, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.InstantiateBoss && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(InstantiateBoss(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.PrepareSpawnPosition && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(PrepareSpawnPosition(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateTargetPlant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlant>(CreateTargetPlant(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.InvokeZombieSpawn && args.Count == 3)
		{
			InvokeZombieSpawn(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindPlacedZombie && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(FindPlacedZombie(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.ReadRenderedTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ReadRenderedTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.InvokeZombieSpawn && args.Count == 3)
		{
			InvokeZombieSpawn(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
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
		if (method == MethodName.ReadRenderedTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ReadRenderedTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
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
		if (method == MethodName.InstantiateBoss)
		{
			return true;
		}
		if (method == MethodName.PrepareSpawnPosition)
		{
			return true;
		}
		if (method == MethodName.CreateTargetPlant)
		{
			return true;
		}
		if (method == MethodName.InvokeZombieSpawn)
		{
			return true;
		}
		if (method == MethodName.FindPlacedZombie)
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
		if (method == MethodName.ReadRenderedTransform)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Finish)
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
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<ZombossPlacedZombieRuntimeControlStub>(in value);
			return true;
		}
		if (name == PropertyName._mapControl)
		{
			_mapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			_mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
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
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
			return true;
		}
		if (name == PropertyName._mapControl)
		{
			value = VariantUtils.CreateFrom(in _mapControl);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			value = VariantUtils.CreateFrom(in _mapFeature);
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
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._mapControl, Variant.From(in _mapControl));
		info.AddProperty(PropertyName._mapFeature, Variant.From(in _mapFeature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._control, out var value2))
		{
			_control = value2.As<ZombossPlacedZombieRuntimeControlStub>();
		}
		if (info.TryGetProperty(PropertyName._mapControl, out var value3))
		{
			_mapControl = value3.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName._mapFeature, out var value4))
		{
			_mapFeature = value4.As<TowerDefenseBattleFeatureMap>();
		}
	}
}
