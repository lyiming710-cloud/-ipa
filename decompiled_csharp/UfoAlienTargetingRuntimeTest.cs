using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/UfoAlienTargetingRuntimeTest.cs")]
public class UfoAlienTargetingRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PrepareCharacter = "PrepareCharacter";

		public static readonly StringName FindReleasedAlien = "FindReleasedAlien";

		public static readonly StringName SetupBattleFixture = "SetupBattleFixture";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName RefreshRegistration = "RefreshRegistration";

		public static readonly StringName RegisterAlienFixture = "RegisterAlienFixture";

		public static readonly StringName RestoreAlienFixture = "RestoreAlienFixture";

		public static readonly StringName Check = "Check";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _previousAlienPacket = "_previousAlienPacket";

		public static readonly StringName _previousAlienCharacter = "_previousAlienCharacter";

		public static readonly StringName _hadAlienPacket = "_hadAlienPacket";

		public static readonly StringName _hadAlienCharacter = "_hadAlienCharacter";

		public static readonly StringName _control = "_control";

		public static readonly StringName _mapControl = "_mapControl";

		public static readonly StringName _mapFeature = "_mapFeature";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string UfoScenePath = "res://Asset/Anime/Character/Zombie/Challenge/Ufo/Scene/TowerDefenseZombieUfo.tscn";

	private const string AlienPacketPath = "res://Asset/Anime/Character/Zombie/Challenge/Alien/Packet/ZombieAlien.tres";

	private const string AlienScenePath = "res://Asset/Anime/Character/Zombie/Challenge/Alien/Scene/TowerDefenseZombieAlien.tscn";

	private const string WallnutScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn";

	private const string CardlessScenePath = "res://Asset/Anime/Character/Plant/Diamond/Cardless/Scene/TowerDefensePlantCardless.tscn";

	private const int TestLine = 2;

	private int _checks;

	private readonly List<string> _failures = new List<string>();

	private readonly List<Resource> _loadedResources = new List<Resource>();

	private Resource _previousAlienPacket;

	private Resource _previousAlienCharacter;

	private bool _hadAlienPacket;

	private bool _hadAlienCharacter;

	private UfoAlienTargetingRuntimeControlStub _control;

	private TowerDefenseMapControl _mapControl;

	private TowerDefenseBattleFeatureMap _mapFeature;

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
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance), "测试必须能够使用正式战斗与资源管理器。");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("缺少测试所需的自动加载管理器。");
				}
				await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
				RegisterAlienFixture();
				SetupBattleFixture(manager);
				await VerifyCardlessAbsorption();
				await VerifyHologramAbsorption();
				await VerifyAbsorptionStateChanges();
				await VerifyReleasedAlienTargetsPlant(manager);
			}
			catch (Exception value)
			{
				_failures.Add($"运行测试出现异常：{value}");
			}
		}
		finally
		{
			await CleanupBattleFixture(manager, previousControl, previousGridBegin, previousGridSize, previousGridNum);
		}
		Finish();
	}

	private async Task VerifyCardlessAbsorption()
	{
		Vector2I gridPosition = new Vector2I(5, 2);
		Vector2 cellPosition = TowerDefenseManager.GetMapCellPlantPos(gridPosition);
		TowerDefensePlantCardless cardless = SpawnAbsorptionCharacter<TowerDefensePlantCardless>("res://Asset/Anime/Character/Plant/Diamond/Cardless/Scene/TowerDefensePlantCardless.tscn", gridPosition);
		TowerDefenseZombieUfo ufo = SpawnAbsorptionCharacter<TowerDefenseZombieUfo>("res://Asset/Anime/Character/Zombie/Challenge/Ufo/Scene/TowerDefenseZombieUfo.tscn", gridPosition);
		await WaitFrames(6);
		Check(cardless.config.name == "PlantCardless" && !cardless.instance.canBeCollection && cardless.instance.invincible, "正式全息卡牌投影必须自行初始化为不可索敌状态。");
		Check(cardless.cell.characterList.Contains(cardless) && ufo.IsInsideComponentBattlefield && ufo.StateMachine.IsInitialized, "回归必须使用已注册的钻卡、有效战场和真实飞船状态机。");
		ufo.SendStateEvent("ToFly");
		ufo.SetLogicalGlobalPosition(cellPosition + new Vector2(10f, 0f));
		ufo.FlyProcessing(1.0);
		Check(ufo.GetLogicalGlobalPosition().X < cellPosition.X - 10f, "只有钻卡时飞船必须继续飞过植物中心，不得停下吸取。");
		Check(ufo.StateMachine.GetStateById("zombie.ufo.fly").IsActive, "只有钻卡时飞船必须保持飞行状态。");
		ufo.SetLogicalGlobalPosition(cellPosition);
		ufo.gridPos = gridPosition;
		ufo.FlyAttackEntered();
		ufo.AnimeCompleted("Up");
		Check(GetAbsorbTargets(ufo).Count == 0, "钻卡不得进入正式吸取目标列表。");
		ufo.AnimeCompleted("Down");
		Check(ufo.StateMachine.GetStateById("zombie.ufo.fly").IsActive, "收束动画结束后不得重新锁定钻卡。");
		TowerDefensePlant wallnut = SpawnAbsorptionCharacter<TowerDefensePlant>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn", gridPosition);
		await WaitFrames(6);
		Check(wallnut.instance.canBeCollection && !wallnut.instance.hologram && wallnut.cell.characterList.Contains(cardless) && wallnut.cell.characterList.Contains(wallnut), "正向对照必须让普通坚果与钻卡处于同一正式地图格。");
		ufo.FlyAttackEntered();
		ufo.AnimeCompleted("Up");
		List<TowerDefenseCharacter> absorbTargets = GetAbsorbTargets(ufo);
		Check(absorbTargets.Count == 1 && absorbTargets[0] == wallnut, "同格吸取列表必须只包含普通坚果。");
		double cardlessHealth = cardless.instance.hitpoints;
		Vector2 cardlessScale = cardless.sprite.Scale;
		double cardlessHeight = cardless.z;
		ufo.FlyAttackProcessing(10.0);
		Check(!cardless.die && wallnut.die, "吸取结算必须只将普通坚果标记为死亡。");
		await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);
		Check(GodotObject.IsInstanceValid(cardless) && !cardless.die && !cardless.isDestroy && cardless.instance.hitpoints == cardlessHealth && cardless.sprite.Scale.IsEqualApprox(cardlessScale) && Mathf.IsEqualApprox(cardless.z, cardlessHeight), "吸取动画结束后钻卡必须存活，生命、缩放和高度保持不变。");
		Check(!GodotObject.IsInstanceValid(wallnut), "普通坚果必须完成真实吸取动画并被移除。");
		ufo.AnimeCompleted("Down");
		Check(ufo.StateMachine.GetStateById("zombie.ufo.fly").IsActive, "吸走普通植物后，剩余钻卡不得触发下一轮吸取。");
		await CleanupCharacters(cardless, wallnut, ufo);
	}

	private async Task VerifyHologramAbsorption()
	{
		Vector2I gridPosition = new Vector2I(5, 2);
		Vector2 cellPosition = TowerDefenseManager.GetMapCellPlantPos(gridPosition);
		TowerDefensePlant hologram = SpawnAbsorptionCharacter<TowerDefensePlant>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn", gridPosition);
		TowerDefenseZombieUfo ufo = SpawnAbsorptionCharacter<TowerDefenseZombieUfo>("res://Asset/Anime/Character/Zombie/Challenge/Ufo/Scene/TowerDefenseZombieUfo.tscn", gridPosition);
		await WaitFrames(6);
		hologram.instance.hologram = true;
		Check(hologram.instance.canBeCollection && hologram.cell.characterList.Contains(hologram), "全息标记对照必须保留默认可索敌字段和正式地图注册。");
		ufo.SendStateEvent("ToFly");
		ufo.SetLogicalGlobalPosition(cellPosition + new Vector2(10f, 0f));
		ufo.FlyProcessing(1.0);
		Check(ufo.GetLogicalGlobalPosition().X < cellPosition.X - 10f, "全息植物不得截停飞船。");
		ufo.SetLogicalGlobalPosition(cellPosition);
		ufo.gridPos = gridPosition;
		ufo.FlyAttackEntered();
		ufo.AnimeCompleted("Up");
		Check(GetAbsorbTargets(ufo).Count == 0, "带全息标记的植物不得进入吸取目标列表。");
		await CleanupCharacters(hologram, ufo);
	}

	private async Task VerifyAbsorptionStateChanges()
	{
		bool[] array = new bool[2] { false, true };
		foreach (bool becomeHologram in array)
		{
			Vector2I gridPosition = new Vector2I(5, 2);
			TowerDefensePlant wallnut = SpawnAbsorptionCharacter<TowerDefensePlant>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn", gridPosition);
			TowerDefenseZombieUfo ufo = SpawnAbsorptionCharacter<TowerDefenseZombieUfo>("res://Asset/Anime/Character/Zombie/Challenge/Ufo/Scene/TowerDefenseZombieUfo.tscn", gridPosition);
			await WaitFrames(6);
			ufo.FlyAttackEntered();
			ufo.AnimeCompleted("Up");
			List<TowerDefenseCharacter> absorbTargets = GetAbsorbTargets(ufo);
			Check(absorbTargets.Count == 1 && absorbTargets[0] == wallnut, "状态变化前必须通过正式吸取入口锁定普通植物。");
			wallnut.instance.hologram = becomeHologram;
			wallnut.instance.canBeCollection = becomeHologram;
			double hitpoints = wallnut.instance.hitpoints;
			Vector2 scale = wallnut.sprite.Scale;
			double height = wallnut.z;
			ufo.FlyAttackProcessing(10.0);
			await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);
			Check(GodotObject.IsInstanceValid(wallnut) && !wallnut.die && !wallnut.isDestroy && wallnut.instance.hitpoints == hitpoints && wallnut.sprite.Scale.IsEqualApprox(scale) && Mathf.IsEqualApprox(wallnut.z, height), "蓄力期间变为" + (becomeHologram ? "全息投影" : "不可索敌") + "的植物必须存活，且不播放吸取缩放或上浮。");
			await CleanupCharacters(wallnut, ufo);
		}
	}

	private T SpawnAbsorptionCharacter<T>(string scenePath, Vector2I gridPosition) where T : TowerDefenseCharacter
	{
		T val = LoadCharacter<T>(scenePath);
		PrepareCharacter(val, gridPosition, TowerDefenseManager.GetMapCellPlantPos(gridPosition));
		val.ProcessMode = ProcessModeEnum.Disabled;
		_control.characterNode.AddChild(val, forceReadableName: false, InternalMode.Disabled);
		if (val is TowerDefensePlant)
		{
			val.cell = TowerDefenseManager.GetMapCell(gridPosition);
			val.cell.characterList.Add(val);
		}
		return val;
	}

	private static List<TowerDefenseCharacter> GetAbsorbTargets(TowerDefenseZombieUfo ufo)
	{
		return (List<TowerDefenseCharacter>)typeof(TowerDefenseZombieUfo).GetField("_absorbTargets", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ufo);
	}

	private async Task VerifyReleasedAlienTargetsPlant(TowerDefenseManager manager)
	{
		TowerDefenseZombieUfo ufo = LoadCharacter<TowerDefenseZombieUfo>("res://Asset/Anime/Character/Zombie/Challenge/Ufo/Scene/TowerDefenseZombieUfo.tscn");
		TowerDefensePlant wallnut = LoadCharacter<TowerDefensePlant>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn");
		Check(GodotObject.IsInstanceValid(ufo) && ufo.config?.name == "ZombieUfo" && GodotObject.IsInstanceValid(wallnut) && wallnut.config?.name == "PlantWallnut", "测试必须实例化正式 UFO 和坚果墙场景。");
		if (!GodotObject.IsInstanceValid(ufo) || !GodotObject.IsInstanceValid(wallnut))
		{
			return;
		}
		Vector2I vector2I = new Vector2I(4, 2);
		Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(vector2I);
		Vector2 passengerSpawnPosition = new Vector2(455f, mapCellPlantPos.Y);
		Vector2I expectedPassengerGridPosition = manager.GetMapGridPos(passengerSpawnPosition);
		Vector2 expectedLandingPosition = new Vector2(passengerSpawnPosition.X, TowerDefenseManager.GetMapCellPlantPos(expectedPassengerGridPosition).Y);
		PrepareCharacter(wallnut, vector2I, mapCellPlantPos);
		PrepareCharacter(ufo, new Vector2I(1, 2), passengerSpawnPosition);
		ufo.ProcessMode = ProcessModeEnum.Disabled;
		wallnut.ProcessMode = ProcessModeEnum.Disabled;
		_control.characterNode.AddChild(wallnut, forceReadableName: false, InternalMode.Disabled);
		_control.characterNode.AddChild(ufo, forceReadableName: false, InternalMode.Disabled);
		wallnut.cell = TowerDefenseManager.GetMapCell(vector2I);
		await WaitFrames(6);
		RefreshRegistration(manager, ufo, wallnut);
		System.Reflection.MethodInfo method = typeof(TowerDefenseZombieUfo).GetMethod("ReleaseAlienAt", BindingFlags.Instance | BindingFlags.NonPublic);
		Check(method != null, "测试必须能够调用 UFO 正式乘客释放入口。");
		if (method == null)
		{
			await CleanupCharacters(wallnut, ufo);
			return;
		}
		method.Invoke(ufo, new object[1] { passengerSpawnPosition });
		await WaitProcessFrame();
		TowerDefenseZombieAlien alien = FindReleasedAlien();
		Check(GodotObject.IsInstanceValid(alien), "UFO 必须按正式延迟生成时序创建外星僵尸乘客。");
		if (!GodotObject.IsInstanceValid(alien))
		{
			await CleanupCharacters(wallnut, ufo);
			return;
		}
		Check(alien.gridPos == expectedPassengerGridPosition && alien.gridPos != ufo.gridPos, $"外星僵尸必须使用自身落点格坐标；期望={expectedPassengerGridPosition}，实际={alien.gridPos}，UFO={ufo.gridPos}。");
		Check(alien.GetLogicalGlobalPosition().DistanceTo(expectedLandingPosition) <= 0.5f, $"外星僵尸逻辑身体必须立即位于自身落点；期望={expectedLandingPosition}，实际={alien.GetLogicalGlobalPosition()}。");
		Check(alien.HasComponentGameplayUntilBattlefieldEntry, "UFO 生成的外星僵尸必须在普通进场边界前启用组件玩法。");
		await WaitFrames(6);
		RefreshRegistration(manager, alien, wallnut);
		AttackComponent attack = alien.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
		Check(attack != null && !attack.IsReleased && GodotObject.IsInstanceValid(alien.sprite), "生成的外星僵尸必须初始化真实攻击组件与动画身体。");
		if (attack?.IsReleased ?? true)
		{
			await CleanupCharacters(alien, wallnut, ufo);
			return;
		}
		double hitpointsBefore = wallnut.instance.hitpoints;
		bool acquiredTarget = false;
		for (int frame = 0; frame < 360; frame++)
		{
			if (!(wallnut.instance.hitpoints >= hitpointsBefore))
			{
				break;
			}
			await WaitFrames(1);
			acquiredTarget |= attack.target == wallnut;
		}
		Check(acquiredTarget, "UFO 生成的外星僵尸必须索敌到攻击框内的坚果墙。");
		Check(wallnut.instance.hitpoints < hitpointsBefore, $"UFO 生成的外星僵尸必须造成真实激光伤害；生命={wallnut.instance.hitpoints}/{hitpointsBefore}。");
		await CleanupCharacters(alien, wallnut, ufo);
	}

	private static void PrepareCharacter(TowerDefenseCharacter character, Vector2I gridPosition, Vector2 position)
	{
		character.editorPreviewMode = false;
		character.inGame = true;
		character.gridPos = gridPosition;
		character.SetLogicalGlobalPosition(position);
	}

	private TowerDefenseZombieAlien FindReleasedAlien()
	{
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefenseZombieAlien towerDefenseZombieAlien && GodotObject.IsInstanceValid(towerDefenseZombieAlien))
			{
				return towerDefenseZombieAlien;
			}
		}
		return null;
	}

	private T LoadCharacter<T>(string path) where T : TowerDefenseCharacter
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		_loadedResources.Add(packedScene);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	private void SetupBattleFixture(TowerDefenseManager manager)
	{
		_control = new UfoAlienTargetingRuntimeControlStub
		{
			Name = "UfoAlienTargetingRuntimeControl",
			isGameRunning = true,
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

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNumber)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			gridNum = gridNumber,
			gridBeginPos = Vector2.Zero,
			gridSize = new Vector2(100f, 76f),
			plantOffset = 50.0
		};
		towerDefenseMapConfig.cellConfig.Add(new TowerDefenseCellConfig
		{
			pos = new Vector4I(1, 1, gridNumber.X, gridNumber.Y)
		});
		for (int i = 1; i <= gridNumber.Y; i++)
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

	private void RegisterAlienFixture()
	{
		ResourceManager instance = ResourceManager.Instance;
		_hadAlienPacket = instance.TOWERDEFENSE_PACKETS.TryGetValue("ZombieAlien", out _previousAlienPacket);
		_hadAlienCharacter = instance.TOWERDEFENSE_CHARCATERS.TryGetValue("ZombieAlien", out _previousAlienCharacter);
		TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Zombie/Challenge/Alien/Packet/ZombieAlien.tres", null, ResourceLoader.CacheMode.Ignore);
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Challenge/Alien/Scene/TowerDefenseZombieAlien.tscn", null, ResourceLoader.CacheMode.Ignore);
		_loadedResources.Add(towerDefensePacketConfig);
		_loadedResources.Add(packedScene);
		instance.TOWERDEFENSE_PACKETS["ZombieAlien"] = towerDefensePacketConfig;
		instance.TOWERDEFENSE_CHARCATERS["ZombieAlien"] = packedScene;
	}

	private void RestoreAlienFixture()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			if (_hadAlienPacket)
			{
				instance.TOWERDEFENSE_PACKETS["ZombieAlien"] = _previousAlienPacket;
			}
			else
			{
				instance.TOWERDEFENSE_PACKETS.Remove("ZombieAlien");
			}
			if (_hadAlienCharacter)
			{
				instance.TOWERDEFENSE_CHARCATERS["ZombieAlien"] = _previousAlienCharacter;
			}
			else
			{
				instance.TOWERDEFENSE_CHARCATERS.Remove("ZombieAlien");
			}
		}
	}

	private async Task CleanupCharacters(params TowerDefenseCharacter[] characters)
	{
		foreach (TowerDefenseCharacter towerDefenseCharacter in characters)
		{
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.IsQueuedForDeletion())
			{
				if (GodotObject.IsInstanceValid(towerDefenseCharacter.cell))
				{
					towerDefenseCharacter.cell.RemoveCharacter(towerDefenseCharacter);
				}
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
		RestoreAlienFixture();
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

	private async Task WaitProcessFrame()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
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
			GD.PushError("[UfoAlienTargetingRuntimeTest] " + failure);
		}
		bool flag = _failures.Count == 0 && _checks == 29;
		GD.Print($"UFO_ALIEN_TARGETING_RESULT passed={flag} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(10)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.PrepareCharacter, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.FindReleasedAlien, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.SetupBattleFixture, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateMapFeature, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2I, "gridNumber", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RefreshRegistration, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Array, "characters", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterAlienFixture, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RestoreAlienFixture, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.PrepareCharacter && args.Count == 3)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindReleasedAlien && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombieAlien>(FindReleasedAlien());
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
		if (method == MethodName.RegisterAlienFixture && args.Count == 0)
		{
			RegisterAlienFixture();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreAlienFixture && args.Count == 0)
		{
			RestoreAlienFixture();
			ret = default;
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
		if (method == MethodName.PrepareCharacter && args.Count == 3)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
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
		if (method == MethodName.PrepareCharacter)
		{
			return true;
		}
		if (method == MethodName.FindReleasedAlien)
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
		if (method == MethodName.RegisterAlienFixture)
		{
			return true;
		}
		if (method == MethodName.RestoreAlienFixture)
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
		if (name == PropertyName._previousAlienPacket)
		{
			_previousAlienPacket = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._previousAlienCharacter)
		{
			_previousAlienCharacter = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._hadAlienPacket)
		{
			_hadAlienPacket = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hadAlienCharacter)
		{
			_hadAlienCharacter = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<UfoAlienTargetingRuntimeControlStub>(in value);
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
		if (name == PropertyName._previousAlienPacket)
		{
			value = VariantUtils.CreateFrom(in _previousAlienPacket);
			return true;
		}
		if (name == PropertyName._previousAlienCharacter)
		{
			value = VariantUtils.CreateFrom(in _previousAlienCharacter);
			return true;
		}
		if (name == PropertyName._hadAlienPacket)
		{
			value = VariantUtils.CreateFrom(in _hadAlienPacket);
			return true;
		}
		if (name == PropertyName._hadAlienCharacter)
		{
			value = VariantUtils.CreateFrom(in _hadAlienCharacter);
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
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previousAlienPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previousAlienCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._hadAlienPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._hadAlienCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
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
		info.AddProperty(PropertyName._previousAlienPacket, Variant.From(in _previousAlienPacket));
		info.AddProperty(PropertyName._previousAlienCharacter, Variant.From(in _previousAlienCharacter));
		info.AddProperty(PropertyName._hadAlienPacket, Variant.From(in _hadAlienPacket));
		info.AddProperty(PropertyName._hadAlienCharacter, Variant.From(in _hadAlienCharacter));
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
		if (info.TryGetProperty(PropertyName._previousAlienPacket, out var value2))
		{
			_previousAlienPacket = value2.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._previousAlienCharacter, out var value3))
		{
			_previousAlienCharacter = value3.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._hadAlienPacket, out var value4))
		{
			_hadAlienPacket = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hadAlienCharacter, out var value5))
		{
			_hadAlienCharacter = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._control, out var value6))
		{
			_control = value6.As<UfoAlienTargetingRuntimeControlStub>();
		}
		if (info.TryGetProperty(PropertyName._mapControl, out var value7))
		{
			_mapControl = value7.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName._mapFeature, out var value8))
		{
			_mapFeature = value8.As<TowerDefenseBattleFeatureMap>();
		}
	}
}
