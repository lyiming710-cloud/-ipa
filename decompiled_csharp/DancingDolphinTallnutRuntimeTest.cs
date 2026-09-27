using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/DancingDolphinTallnutRuntimeTest.cs")]
public class DancingDolphinTallnutRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private readonly List<string> _failures = new List<string>();

	private int _checks;

	private DancingDolphinTallnutRuntimeControl _control;

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
			_ = 2;
			try
			{
				await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
				SetupBattle(manager);
				string[] array = new string[3] { "PlantTallnut", "PlantSeaNut", "PlantWallnut" };
				foreach (string plantPacket in array)
				{
					await VerifyJump(plantPacket, useSummonedDancer: false);
					await VerifyJump(plantPacket, useSummonedDancer: true);
				}
			}
			catch (Exception value)
			{
				_failures.Add($"运行回归异常：{value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(_control))
			{
				_control.isGameRunning = false;
				_control.QueueFree();
			}
			await WaitFrames(6);
			manager.currentControl = previousControl;
			manager.gridBeginPos = previousBegin;
			manager.gridSize = previousSize;
			manager.gridNum = previousNumber;
			_map?.Destroy();
			if (GodotObject.IsInstanceValid(_mapControl))
			{
				_mapControl.Free();
			}
			ObjectManager.Instance.Clear();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		foreach (string failure in _failures)
		{
			GD.PushError("[DancingDolphinTallnutRuntimeTest] " + failure);
		}
		bool flag = _checks == 48 && _failures.Count == 0;
		GD.Print($"DANCING_DOLPHIN_TALLNUT_RESULT passed={flag} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyJump(string plantPacket, bool useSummonedDancer)
	{
		string label = (useSummonedDancer ? "召唤舞伴" : "舞王") + "/" + plantPacket;
		TowerDefenseZombieDolphinMJ leader = TowerDefenseManager.GetPacketConfig("ZombieDolphinMJ").Plant(new Vector2I(8, 3), playAudio: false) as TowerDefenseZombieDolphinMJ;
		leader.timer = 60.0;
		leader.ProcessMode = ProcessModeEnum.Disabled;
		await WaitFrames(6);
		TowerDefenseZombie zombie = leader;
		if (useSummonedDancer)
		{
			leader.SpawnDancer();
			await WaitFrames(6);
			Check(leader.dancerList.Count == 4 && leader.dancerList.All((TowerDefenseCharacter dancer) => GodotObject.IsInstanceValid(dancer) && dancer is TowerDefenseZombieDolphinDC), label + " 必须由正式召唤流程生成四名伴舞海豚。");
			zombie = leader.dancerList[2] as TowerDefenseZombieDolphinDC;
			for (int frame = 0; frame < 180; frame++)
			{
				if (!zombie.isRise)
				{
					break;
				}
				await WaitFrames(1);
			}
			Check(zombie is TowerDefenseZombieDolphinDC towerDefenseZombieDolphinDC && towerDefenseZombieDolphinDC.GetJackson() == leader && towerDefenseZombieDolphinDC.dolphin && !towerDefenseZombieDolphinDC.isRise, label + " 必须保留舞王归属并携带海豚完成出土。");
			foreach (TowerDefenseCharacter dancer in leader.dancerList)
			{
				dancer.ProcessMode = ProcessModeEnum.Disabled;
			}
		}
		Vector2I targetGrid = new Vector2I(5, 3);
		if (plantPacket != "PlantSeaNut")
		{
			TowerDefenseCharacter lily = TowerDefenseManager.GetPacketConfig("PlantLilyPad").Plant(targetGrid, playAudio: false);
			await WaitFrames(6);
			GD.Print($"DANCING_DOLPHIN_SETUP lily={GodotObject.IsInstanceValid(lily)} types={string.Join(',', TowerDefenseManager.GetMapCell(targetGrid).gridType)} water={zombie.inWater}");
		}
		TowerDefensePlant plant = (TowerDefenseManager.GetPacketConfig(plantPacket).Plant(targetGrid, playAudio: false) as TowerDefensePlant) ?? throw new InvalidOperationException("无法种植测试植物 " + plantPacket + "。");
		await WaitFrames(6);
		bool isTall = plantPacket != "PlantWallnut";
		Check(plant != null && plant.instance.height >= TowerDefenseEnum.CHARACTER_HEIGHT.TALL == isTall && zombie.inWater, label + " 必须使用正式植物高度与水中海豚状态。");
		Vector2 startPosition = plant.GetLogicalGlobalPosition() + new Vector2(65f, 0f);
		zombie.SetLogicalGlobalPosition(startPosition);
		zombie.gridPos = TowerDefenseManager.Instance.GetMapGridPos(startPosition);
		zombie.inSwimPlay = true;
		zombie.ProcessMode = ProcessModeEnum.Inherit;
		zombie.Walk();
		bool enteredJump = false;
		ulong timeoutFrame = Engine.GetPhysicsFrames() + (ulong)(Engine.PhysicsTicksPerSecond * 8);
		while (Engine.GetPhysicsFrames() < timeoutFrame)
		{
			await WaitFrames(1);
			bool flag = enteredJump;
			string text = zombie.CurrentStateHandle?.StableId;
			bool flag2 = ((text == "zombie.dolphin_mj.jump" || text == "zombie.dolphin_dc.jump") ? true : false);
			enteredJump = flag | flag2;
			if (enteredJump)
			{
				text = zombie.CurrentStateHandle?.StableId;
				if (!(text == "zombie.dolphin_mj.jump") && !(text == "zombie.dolphin_dc.jump"))
				{
					break;
				}
			}
		}
		zombie.walkSpeedScale = 0.0;
		await WaitFrames(6);
		zombie.ProcessMode = ProcessModeEnum.Disabled;
		Vector2 logicalGlobalPosition = zombie.GetLogicalGlobalPosition();
		bool flag3 = ((zombie is TowerDefenseZombieDolphinMJ towerDefenseZombieDolphinMJ) ? towerDefenseZombieDolphinMJ.dolphin : ((TowerDefenseZombieDolphinDC)zombie).dolphin);
		bool flag4 = ((zombie is TowerDefenseZombieDolphinMJ towerDefenseZombieDolphinMJ2) ? towerDefenseZombieDolphinMJ2.isJump : ((TowerDefenseZombieDolphinDC)zombie).isJump);
		Check(enteredJump, label + " 必须经自然索敌进入跳跃状态。");
		Check(!flag3 && !flag4, label + " 拦截或完成跳跃后必须丢弃海豚并退出跳跃。");
		Check(isTall ? (logicalGlobalPosition.X >= plant.GetLogicalGlobalPosition().X + 30f) : (logicalGlobalPosition.X < plant.GetLogicalGlobalPosition().X), $"{label} 落点必须位于{(isTall ? "高大植物前方" : "普通植物后方")}，实际 {logicalGlobalPosition}，植物 {plant.GetLogicalGlobalPosition()}。");
		Check(zombie.instance.collisionFlags != 0 && zombie.instance.maskFlags != 0, label + " 落地或拦截后必须恢复碰撞标记。");
		zombie.AnimeCompleted("DolphinJump");
		Check(zombie.GetLogicalGlobalPosition().IsEqualApprox(logicalGlobalPosition), label + " 迟到的跳跃完成回调不得再次移动僵尸。");
		Check(zombie.gridPos == TowerDefenseManager.Instance.GetMapGridPos(logicalGlobalPosition), $"{label} 落点必须同步逻辑格子，实际 {zombie.gridPos}，预期 {TowerDefenseManager.Instance.GetMapGridPos(logicalGlobalPosition)}。");
		GD.Print($"DANCING_DOLPHIN_TALLNUT case={label} jumped={enteredJump} tall={isTall} start={startPosition} final={logicalGlobalPosition} plant={plant.GetLogicalGlobalPosition()} dolphin={flag3} state={zombie.CurrentStateHandle?.StableId}");
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefenseCharacter && !child.IsQueuedForDeletion())
			{
				child.QueueFree();
			}
		}
		await WaitFrames(6);
	}

	private void SetupBattle(TowerDefenseManager manager)
	{
		_control = new DancingDolphinTallnutRuntimeControl
		{
			isGameRunning = true,
			isInit = true,
			levelConfig = new TowerDefenseLevelConfig(),
			characterNode = new Node2D()
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
		manager.currentControl = _control;
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig();
		manager.gridBeginPos = towerDefenseMapConfig.gridBeginPos;
		manager.gridSize = towerDefenseMapConfig.gridSize;
		manager.gridNum = towerDefenseMapConfig.gridNum;
		towerDefenseMapConfig.cellConfig.Add(new TowerDefenseCellConfig
		{
			pos = new Vector4I(1, 1, towerDefenseMapConfig.gridNum.X, towerDefenseMapConfig.gridNum.Y),
			gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
			{
				TowerDefenseEnum.PLANTGRIDTYPE.WATER,
				TowerDefenseEnum.PLANTGRIDTYPE.AIR
			}
		});
		for (int i = 1; i <= towerDefenseMapConfig.gridNum.Y; i++)
		{
			towerDefenseMapConfig.lineUse.Add(i);
		}
		_mapControl = new TowerDefenseMapControl();
		_map = new TowerDefenseBattleFeatureMap
		{
			control = _control,
			mapControl = _mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig,
			rect = TowerDefenseBattleFeatureMap.BuildProjectileBoundaryRect(towerDefenseMapConfig)
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
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
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
			_control = VariantUtils.ConvertTo<DancingDolphinTallnutRuntimeControl>(in value);
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
			_control = value2.As<DancingDolphinTallnutRuntimeControl>();
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
