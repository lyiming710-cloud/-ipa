using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/PumpkinFireIceSlowRuntimeTest.cs")]
public class PumpkinFireIceSlowRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName AddCold = "AddCold";

		public static readonly StringName SetupBattle = "SetupBattle";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _control = "_control";

		public static readonly StringName _map = "_map";

		public static readonly StringName _mapControl = "_mapControl";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string PumpkinPath = "res://Asset/Anime/Character/Plant/Cover/PumpkinFireIce/Scene/TowerDefensePlantPumpkinFireIce.tscn";

	private const string ZombiePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string WallnutPath = "res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn";

	private const string AirPlantPath = "res://Asset/Anime/Character/Plant/Star/LilyLeaf/Scene/TowerDefensePlantLilyLeaf.tscn";

	private readonly List<TowerDefenseCharacter> _characters = new List<TowerDefenseCharacter>();

	private readonly List<string> _failures = new List<string>();

	private int _checks;

	private PumpkinFireIceSlowRuntimeControl _control;

	private TowerDefenseBattleFeatureMap _map;

	private TowerDefenseMapControl _mapControl;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager.currentControl;
		Vector2 previousBegin = manager.gridBeginPos;
		Vector2 previousSize = manager.gridSize;
		Vector2I previousNumber = manager.gridNum;
		try
		{
			_ = 1;
			try
			{
				await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
				SetupBattle(manager);
				await VerifyBurnAndProtection();
			}
			catch (Exception value)
			{
				_failures.Add($"运行回归出现异常：{value}");
			}
		}
		finally
		{
			foreach (TowerDefenseCharacter character in _characters)
			{
				if (GodotObject.IsInstanceValid(character))
				{
					character.cell?.RemoveCharacter(character);
					character.QueueFree();
				}
			}
			await WaitFrames(5);
			if (GodotObject.IsInstanceValid(_control))
			{
				_control.isGameRunning = false;
				_control.QueueFree();
			}
			manager.currentControl = previousControl;
			manager.gridBeginPos = previousBegin;
			manager.gridSize = previousSize;
			manager.gridNum = previousNumber;
			_map?.Destroy();
			if (GodotObject.IsInstanceValid(_mapControl))
			{
				_mapControl.Free();
			}
			await WaitFrames(5);
			ObjectManager.Instance.Clear();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		foreach (string failure in _failures)
		{
			GD.PushError("[PumpkinFireIceSlowRuntimeTest] " + failure);
		}
		bool flag = _checks == 20 && _failures.Count == 0;
		GD.Print($"PUMPKIN_FIRE_ICE_SLOW_RESULT passed={flag} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyBurnAndProtection()
	{
		Vector2I vector2I = new Vector2I(5, 2);
		TowerDefensePlantPumpkinFireIce pumpkin = Spawn<TowerDefensePlantPumpkinFireIce>("res://Asset/Anime/Character/Plant/Cover/PumpkinFireIce/Scene/TowerDefensePlantPumpkinFireIce.tscn", vector2I);
		TowerDefenseZombie enemy = Spawn<TowerDefenseZombie>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", vector2I);
		TowerDefensePlant ally = Spawn<TowerDefensePlant>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn", vector2I);
		TowerDefensePlant air = Spawn<TowerDefensePlant>("res://Asset/Anime/Character/Plant/Star/LilyLeaf/Scene/TowerDefensePlantLilyLeaf.tscn", vector2I);
		TowerDefensePlant outside = Spawn<TowerDefensePlant>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn", vector2I + Vector2I.Right);
		TowerDefensePlant opposingPlant = Spawn<TowerDefensePlant>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn", vector2I);
		TowerDefenseZombie friendlyZombie = Spawn<TowerDefenseZombie>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", vector2I);
		await WaitFrames(6);
		opposingPlant.Hypnoses();
		friendlyZombie.Hypnoses();
		await WaitFrames(2);
		Check(pumpkin.config.name == "PlantPumpkinFireIce" && pumpkin.allEventList.Count > 0, "必须使用带正式保护事件的冰焰南瓜壳。");
		Check(air.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR), "飞行植物对照必须使用正式飞行种植类型。");
		Check(ally.camp == pumpkin.camp && enemy.camp != pumpkin.camp && opposingPlant.camp != pumpkin.camp && friendlyZombie.camp == pumpkin.camp, "敌方、友方和魅惑角色必须具备正确阵营。");
		PeriodicAreaEventComponent runtime = pumpkin.componentManager.GetRuntime<PeriodicAreaEventComponent>();
		Check(runtime != null && !runtime.IsReleased && Mathf.IsEqualApprox(runtime.staticTime, 1.5f), "必须装配每 1.5 秒执行的正式灼烧组件。");
		double hitpoints = enemy.instance.hitpoints;
		runtime.PhysicsProcess(1.5, Engine.GetPhysicsFrames());
		Check(Mathf.IsEqualApprox(hitpoints - enemy.instance.hitpoints, 40.0), "一次正式灼烧必须造成 40 点伤害。");
		Check(enemy.buff.BuffHas("IceSpeedDown") && enemy.iceSpeedDown, "灼烧必须立即给普通僵尸施加冰减速。");
		enemy.BatchUpdate(1.0 / 60.0);
		Check(Mathf.IsEqualApprox(enemy.timeScale, enemy.timeScaleInit * 0.5), "灼烧减速必须把僵尸实际时间倍率降至一半。");
		AddCold(ally, frozen: true);
		AddCold(air, frozen: false);
		AddCold(outside, frozen: true);
		AddCold(opposingPlant, frozen: true);
		AddCold(friendlyZombie, frozen: false);
		Check(ally.buff.BuffHas("Frozen") && ally.buff.BuffHas("IceSpeedDown"), "友方植物必须先进入真实冻结和减速状态。");
		Check(air.buff.BuffHas("IceSpeedDown") && outside.buff.BuffHas("Frozen") && opposingPlant.buff.BuffHas("Frozen") && friendlyZombie.buff.BuffHas("IceSpeedDown"), "保护范围的排除项必须先具备真实冰系状态。");
		await PulseProtection(pumpkin);
		Check(enemy.buff.BuffHas("IceSpeedDown") && enemy.iceSpeedDown, "保护脉冲不得清除僵尸刚获得的灼烧减速。");
		enemy.BatchUpdate(1.0 / 60.0);
		Check(Mathf.IsEqualApprox(enemy.timeScale, enemy.timeScaleInit * 0.5), "保护脉冲后僵尸仍须保持半速。");
		Check(!ally.buff.BuffHas("Frozen") && !ally.buff.BuffHas("IceSpeedDown"), "本格友方非飞行植物必须解除冻结和减速。");
		Check(air.buff.BuffHas("IceSpeedDown"), "同格飞行植物不应获得南瓜壳保护。");
		Check(outside.buff.BuffHas("Frozen") && outside.buff.BuffHas("IceSpeedDown"), "相邻格植物不应获得本格保护。");
		Check(opposingPlant.buff.BuffHas("Frozen") && opposingPlant.buff.BuffHas("IceSpeedDown"), "同格敌方植物不应获得友方保护。");
		Check(friendlyZombie.buff.BuffHas("IceSpeedDown"), "友方僵尸也不属于南瓜壳保护的植物。");
		for (int pulse = 0; pulse < 3; pulse++)
		{
			await PulseProtection(pumpkin);
		}
		Check(enemy.buff.BuffHas("IceSpeedDown"), "连续保护脉冲不能抵消持续的灼烧减速。");
		pumpkin.Hypnoses();
		await WaitFrames(2);
		AddCold(ally, frozen: true);
		AddCold(opposingPlant, frozen: true);
		Check(pumpkin.camp == opposingPlant.camp && pumpkin.camp != ally.camp, "南瓜壳被魅惑后必须使用转换后的实际阵营。");
		await PulseProtection(pumpkin);
		Check(!opposingPlant.buff.BuffHas("Frozen") && !opposingPlant.buff.BuffHas("IceSpeedDown"), "被魅惑南瓜壳必须保护同格的新友方植物。");
		Check(ally.buff.BuffHas("Frozen") && ally.buff.BuffHas("IceSpeedDown"), "被魅惑南瓜壳不得继续保护原阵营植物。");
	}

	private async Task PulseProtection(TowerDefensePlantPumpkinFireIce pumpkin)
	{
		foreach (TowerDefenseCharacter character in _characters)
		{
			character.ProcessMode = ProcessModeEnum.Inherit;
		}
		try
		{
			pumpkin.coldCheckInterval = 0;
			pumpkin.BatchUpdate(1.0 / 60.0);
			TowerDefenseExplode.DrainPendingWorkForTest();
		}
		finally
		{
			foreach (TowerDefenseCharacter character2 in _characters)
			{
				character2.ProcessMode = ProcessModeEnum.Disabled;
			}
		}
		await WaitFrames(3);
	}

	private static void AddCold(TowerDefenseCharacter character, bool frozen)
	{
		character.buff.DeleteBuff("FireHit");
		character.buff.AddBuff(new TowerDefenseCharacterBuffIceSpeedDown());
		if (frozen)
		{
			character.buff.AddBuff(new TowerDefenseCharacterBuffFrozen());
		}
	}

	private T Spawn<T>(string scenePath, Vector2I grid) where T : TowerDefenseCharacter
	{
		T val = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Reuse).Instantiate<T>(PackedScene.GenEditState.Disabled);
		val.inGame = true;
		val.editorPreviewMode = false;
		val.gridPos = grid;
		val.SetLogicalGlobalPosition(TowerDefenseManager.GetMapCellPlantPos(grid));
		val.ProcessMode = ProcessModeEnum.Disabled;
		_control.characterNode.AddChild(val, forceReadableName: false, InternalMode.Disabled);
		if (val is TowerDefensePlant)
		{
			val.cell = TowerDefenseManager.GetMapCell(grid);
			val.cell.characterList.Add(val);
		}
		_characters.Add(val);
		return val;
	}

	private void SetupBattle(TowerDefenseManager manager)
	{
		_control = new PumpkinFireIceSlowRuntimeControl
		{
			isGameRunning = true,
			isInit = true,
			levelConfig = new TowerDefenseLevelConfig(),
			characterNode = new Node2D()
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
		manager.currentControl = _control;
		manager.gridBeginPos = Vector2.Zero;
		manager.gridSize = new Vector2(100f, 76f);
		manager.gridNum = new Vector2I(9, 5);
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			gridNum = manager.gridNum,
			gridBeginPos = manager.gridBeginPos,
			gridSize = manager.gridSize,
			plantOffset = 50.0
		};
		towerDefenseMapConfig.cellConfig.Add(new TowerDefenseCellConfig
		{
			pos = new Vector4I(1, 1, manager.gridNum.X, manager.gridNum.Y)
		});
		for (int i = 1; i <= manager.gridNum.Y; i++)
		{
			towerDefenseMapConfig.lineUse.Add(i);
		}
		_mapControl = new TowerDefenseMapControl();
		_map = new TowerDefenseBattleFeatureMap
		{
			control = _control,
			mapControl = _mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig
		};
		_mapControl.mapFeature = _map;
		_map.PlantGridInit();
		_control.featureDictionary["Map"] = _map;
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddCold, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "frozen", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetupBattle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.AddCold && args.Count == 2)
		{
			AddCold(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetupBattle && args.Count == 1)
		{
			SetupBattle(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]));
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
		if (method == MethodName.AddCold && args.Count == 2)
		{
			AddCold(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
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
		if (method == MethodName.AddCold)
		{
			return true;
		}
		if (method == MethodName.SetupBattle)
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
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<PumpkinFireIceSlowRuntimeControl>(in value);
			return true;
		}
		if (name == PropertyName._map)
		{
			_map = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
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
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
			return true;
		}
		if (name == PropertyName._map)
		{
			value = VariantUtils.CreateFrom(in _map);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._map, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._map, Variant.From(in _map));
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
		if (info.TryGetProperty(PropertyName._control, out var value2))
		{
			_control = value2.As<PumpkinFireIceSlowRuntimeControl>();
		}
		if (info.TryGetProperty(PropertyName._map, out var value3))
		{
			_map = value3.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName._mapControl, out var value4))
		{
			_mapControl = value4.As<TowerDefenseMapControl>();
		}
	}
}
