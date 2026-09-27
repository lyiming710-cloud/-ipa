using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/LookStarCompletionRuntimeTest.cs")]
public class LookStarCompletionRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CountVisiblePreviews = "CountVisiblePreviews";

		public static readonly StringName CountAwards = "CountAwards";

		public static readonly StringName GetFinishCount = "GetFinishCount";

		public static readonly StringName OnVictory = "OnVictory";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _victories = "_victories";

		public static readonly StringName _control = "_control";

		public static readonly StringName _map = "_map";

		public static readonly StringName _mapControl = "_mapControl";

		public static readonly StringName _process = "_process";

		public static readonly StringName _wave = "_wave";

		public static readonly StringName _lookStar = "_lookStar";

		public static readonly StringName _level = "_level";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private int _checks;

	private int _victories;

	private LookStarCompletionRuntimeControl _control;

	private TowerDefenseBattleFeatureMap _map;

	private TowerDefenseMapControl _mapControl;

	private TowerDefenseBattleProcessWave _process;

	private TowerDefenseBattleFeatureWave _wave;

	private TowerDefenseBattleFeatureLookStar _lookStar;

	private TowerDefenseLevelConfig _level;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager.currentControl;
		TowerDefenseLevelBaseConfig previousLevel = manager.currentLevelConfig;
		Vector2 previousBegin = manager.gridBeginPos;
		Vector2 previousSize = manager.gridSize;
		Vector2I previousNumber = manager.gridNum;
		string previousMode = Global.Instance.enterLevelMode;
		bool previousEditor = Global.Instance.isEditor;
		try
		{
			_ = 3;
			try
			{
				Global.Instance.enterLevelMode = "LevelTest";
				Global.Instance.isEditor = false;
				GameSaveManager.Instance.EnsureLoaded();
				GameSaveManager.Instance.SetUserCurrent("LookStarCompletionRuntimeTest");
				await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
				BattleEventBus.Instance.OnGameVictory += OnVictory;
				await VerifyCompletion("MiniGames_Level7_1", withZombie: false, pendingOperation: false);
				await VerifyCompletion("MiniGames_Level7_1", withZombie: true, pendingOperation: false);
				await VerifyCompletion("MiniGames_Level7_1_D", withZombie: true, pendingOperation: true);
			}
			catch (Exception value)
			{
				_failures.Add($"运行回归异常：{value}");
			}
		}
		finally
		{
			BattleEventBus.Instance.OnGameVictory -= OnVictory;
			await ClearBattle();
			manager.currentControl = previousControl;
			manager.currentLevelConfig = previousLevel;
			manager.gridBeginPos = previousBegin;
			manager.gridSize = previousSize;
			manager.gridNum = previousNumber;
			Global.Instance.enterLevelMode = previousMode;
			Global.Instance.isEditor = previousEditor;
			ObjectManager.Instance.Clear();
			AudioManager.Instance.AudioStopAll();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		foreach (string failure in _failures)
		{
			GD.PushError("[LookStarCompletionRuntimeTest] " + failure);
		}
		bool flag = _checks > 0 && _failures.Count == 0;
		GD.Print($"LOOK_STAR_COMPLETION_RESULT passed={flag} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyCompletion(string levelName, bool withZombie, bool pendingOperation)
	{
		await SetupBattle(levelName);
		string label = $"{levelName} zombie={withZombie} pending={pendingOperation}";
		int previousFinishCount = GetFinishCount();
		Check(_lookStar.config.checkList.Count == 14, label + "：必须加载正式观星第一关的十四格阵型。");
		for (int index = 0; index < _lookStar.config.checkList.Count - 1; index++)
		{
			await PlantRequirement(_lookStar.config.checkList[index]);
		}
		_lookStar.Process(1.0);
		Check(!_lookStar.isfinish && !_control.levelControl.awardCreate && CountVisiblePreviews() == 1, label + "：缺少最后一株时必须保留虚影且不能通关。");
		if (withZombie)
		{
			TowerDefenseCharacter zombie = TowerDefenseManager.GetPacketConfig("ZombieNormal").Plant(new Vector2I(9, 3), playAudio: false);
			await WaitFrames(4);
			Check(GodotObject.IsInstanceValid(zombie) && zombie.inGame, label + "：复现必须包含正式存活僵尸。");
			zombie.ProcessMode = ProcessModeEnum.Disabled;
		}
		int pendingId = (pendingOperation ? _control.BeginPendingBattleOperation() : (-1));
		int spawnId = (pendingOperation ? _wave.BeginPendingSpawnOperation() : (-1));
		if (pendingOperation)
		{
			_wave.spawnOver = false;
		}
		LookStarCompletionRuntimeTest lookStarCompletionRuntimeTest = this;
		Array<TowerDefenseLevelLookStarCheckConfig> checkList = _lookStar.config.checkList;
		await lookStarCompletionRuntimeTest.PlantRequirement(checkList[checkList.Count - 1]);
		_lookStar.Process(1.0);
		Check(CountVisiblePreviews() == 0, label + "：阵型摆齐后全部正式虚影必须消失。");
		if (pendingOperation)
		{
			Check(!_lookStar.isfinish && !_control.levelControl.awardCreate, label + "：已有异步操作未完成时必须保留重试机会。");
			_control.CompletePendingBattleOperation(pendingId);
			_lookStar.Process(1.0);
			Check(!_lookStar.isfinish && !_control.levelControl.awardCreate, label + "：波次异步生成仍未完成时也不能接受胜利请求。");
			_wave.CompletePendingSpawnOperation(spawnId);
			_lookStar.Process(1.0);
		}
		GD.Print($"LOOK_STAR_COMPLETION_STAGE case={label} formation={_lookStar.isfinish} award={_control.levelControl.awardCreate} pendingBattle={_control.HasPendingBattleOperations} waveFinal={_wave.waveFinal} spawnOver={_wave.spawnOver}");
		Check(_lookStar.isfinish, label + "：满足阵型且可以完成时必须接收胜利请求。");
		Check(_wave.waveFinal && _wave.spawnOver, label + "：阵型完成必须停止刷怪并进入可以重试的末波结算。");
		if (withZombie)
		{
			Check(_control.HasPendingBattleOperations && !_control.levelControl.awardCreate, label + "：清场必须经过正式跨帧死亡结算，不能提前生成奖励。");
		}
		for (int index = 0; index < 120; index++)
		{
			if (_control.levelControl.awardCreate)
			{
				break;
			}
			await WaitFrames(1);
			_lookStar.Process(1.0 / 60.0);
			_process.PhysicsProcess(1.0 / 60.0);
		}
		Check(_control.levelControl.awardCreate && _victories == 1 && CountAwards() == 1, $"{label}：清场结算后必须发出一次胜利事件并创建真实奖励，实际 award={_control.levelControl.awardCreate} victories={_victories} rewards={CountAwards()}。");
		Check(GetFinishCount() == previousFinishCount + 1, label + "：正式关卡的完成次数必须增加一次。");
		Check(TowerDefenseManager.Instance.characterRegistry.GetZombieCount() == 0 && !_control.HasPendingBattleOperations, label + "：胜利后僵尸与死亡结算必须清空。");
		for (int index = 0; index < 10; index++)
		{
			_lookStar.Process(1.0);
			_process.PhysicsProcess(0.1);
			await WaitFrames(1);
		}
		Check(_victories == 1 && CountAwards() == 1 && GetFinishCount() == previousFinishCount + 1, label + "：后续帧不能重复发奖或记录通关。");
		GD.Print($"LOOK_STAR_COMPLETION_STAGE case={label} settled={_control.levelControl.awardCreate} victories={_victories} rewards={CountAwards()} finishCount={GetFinishCount()}");
		await ClearBattle();
	}

	private async Task SetupBattle(string levelName)
	{
		_victories = 0;
		_level = ResourceLoader.Load<TowerDefenseLevelConfig>("res://Asset/Config/Level/TowerDefense/MiniGames/" + levelName + ".tres", null, ResourceLoader.CacheMode.IgnoreDeep);
		_level.Init();
		_control = new LookStarCompletionRuntimeControl
		{
			isGameRunning = true,
			isInit = true,
			levelConfig = _level,
			characterNode = new Node2D()
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
		_control.AddChild(new Camera2D(), forceReadableName: false, InternalMode.Disabled);
		_control.levelControl = new LookStarCompletionRuntimeLevelControl
		{
			config = _level
		};
		_control.AddChild(_control.levelControl, forceReadableName: false, InternalMode.Disabled);
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		instance.currentControl = _control;
		instance.currentLevelConfig = _level;
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig();
		instance.gridBeginPos = towerDefenseMapConfig.gridBeginPos;
		instance.gridSize = towerDefenseMapConfig.gridSize;
		instance.gridNum = towerDefenseMapConfig.gridNum;
		towerDefenseMapConfig.cellConfig.Add(new TowerDefenseCellConfig
		{
			pos = new Vector4I(1, 1, towerDefenseMapConfig.gridNum.X, towerDefenseMapConfig.gridNum.Y)
		});
		for (int i = 1; i <= towerDefenseMapConfig.gridNum.Y; i++)
		{
			towerDefenseMapConfig.lineUse.Add(i);
		}
		_mapControl = new TowerDefenseMapControl
		{
			mapIceCap = new Node2D()
		};
		_control.AddChild(_mapControl.mapIceCap, forceReadableName: false, InternalMode.Disabled);
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
		_wave = new TowerDefenseBattleFeatureWave
		{
			control = _control,
			levelControl = _control.levelControl,
			mapFeature = _map,
			progressFeature = new TowerDefenseBattleFeatureProgress(),
			config = _level.waveManager,
			currentDynamic = new TowerDefenseLevelDynamicConfig(),
			isRunning = true,
			readySetPlantOver = true,
			waveStart = true,
			spawnOver = true,
			nextWaveTime = 600.0
		};
		_control.featureDictionary["Wave"] = _wave;
		_process = new TowerDefenseBattleProcessWave
		{
			control = _control,
			levelControl = _control.levelControl,
			waveFeature = _wave
		};
		_control.process = _process;
		_lookStar = new TowerDefenseBattleFeatureLookStar
		{
			control = _control
		};
		_lookStar.Init(_level.featureData["LookStar"]);
		_control.featureDictionary["LookStar"] = _lookStar;
		await _lookStar.GameInit();
	}

	private async Task PlantRequirement(TowerDefenseLevelLookStarCheckConfig check)
	{
		TowerDefenseCharacter plant = TowerDefenseManager.GetPacketConfig(check.packetName).Plant(check.gridPos, playAudio: false);
		await WaitFrames(4);
		Check(GodotObject.IsInstanceValid(plant) && TowerDefenseManager.GetMapCell(check.gridPos).HasCharacter(check.packetName), $"正式要求植物必须注册在指定格子：{check.packetName} {check.gridPos}。");
		plant.ProcessMode = ProcessModeEnum.Disabled;
	}

	private int CountVisiblePreviews()
	{
		int num = 0;
		foreach (Variant value in _lookStar.checkDictionary.Values)
		{
			if (value.AsGodotObject() is AdobeAnimateSprite { Visible: not false })
			{
				num++;
			}
		}
		return num;
	}

	private int CountAwards()
	{
		int num = 0;
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefenseAwardBase)
			{
				num++;
			}
		}
		return num;
	}

	private int GetFinishCount()
	{
		return GameSaveManager.Instance.GetLevelValue(_level.name).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary()
			.GetValueOrDefault("Finish", 0)
			.AsInt32();
	}

	private void OnVictory()
	{
		_victories++;
	}

	private async Task ClearBattle()
	{
		if (GodotObject.IsInstanceValid(_control))
		{
			_control.isGameRunning = false;
			_control.QueueFree();
			await WaitFrames(6);
		}
		_control = null;
		if (GodotObject.IsInstanceValid(_mapControl))
		{
			_mapControl.Free();
		}
		_mapControl = null;
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
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountVisiblePreviews, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountAwards, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFinishCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVictory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.CountVisiblePreviews && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountVisiblePreviews());
			return true;
		}
		if (method == MethodName.CountAwards && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountAwards());
			return true;
		}
		if (method == MethodName.GetFinishCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetFinishCount());
			return true;
		}
		if (method == MethodName.OnVictory && args.Count == 0)
		{
			OnVictory();
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
		if (method == MethodName.CountVisiblePreviews)
		{
			return true;
		}
		if (method == MethodName.CountAwards)
		{
			return true;
		}
		if (method == MethodName.GetFinishCount)
		{
			return true;
		}
		if (method == MethodName.OnVictory)
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
		if (name == PropertyName._victories)
		{
			_victories = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<LookStarCompletionRuntimeControl>(in value);
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
		if (name == PropertyName._process)
		{
			_process = VariantUtils.ConvertTo<TowerDefenseBattleProcessWave>(in value);
			return true;
		}
		if (name == PropertyName._wave)
		{
			_wave = VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in value);
			return true;
		}
		if (name == PropertyName._lookStar)
		{
			_lookStar = VariantUtils.ConvertTo<TowerDefenseBattleFeatureLookStar>(in value);
			return true;
		}
		if (name == PropertyName._level)
		{
			_level = VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in value);
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
		if (name == PropertyName._victories)
		{
			value = VariantUtils.CreateFrom(in _victories);
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
		if (name == PropertyName._process)
		{
			value = VariantUtils.CreateFrom(in _process);
			return true;
		}
		if (name == PropertyName._wave)
		{
			value = VariantUtils.CreateFrom(in _wave);
			return true;
		}
		if (name == PropertyName._lookStar)
		{
			value = VariantUtils.CreateFrom(in _lookStar);
			return true;
		}
		if (name == PropertyName._level)
		{
			value = VariantUtils.CreateFrom(in _level);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._victories, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._map, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._process, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._wave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lookStar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._level, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._victories, Variant.From(in _victories));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._map, Variant.From(in _map));
		info.AddProperty(PropertyName._mapControl, Variant.From(in _mapControl));
		info.AddProperty(PropertyName._process, Variant.From(in _process));
		info.AddProperty(PropertyName._wave, Variant.From(in _wave));
		info.AddProperty(PropertyName._lookStar, Variant.From(in _lookStar));
		info.AddProperty(PropertyName._level, Variant.From(in _level));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._victories, out var value2))
		{
			_victories = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._control, out var value3))
		{
			_control = value3.As<LookStarCompletionRuntimeControl>();
		}
		if (info.TryGetProperty(PropertyName._map, out var value4))
		{
			_map = value4.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName._mapControl, out var value5))
		{
			_mapControl = value5.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName._process, out var value6))
		{
			_process = value6.As<TowerDefenseBattleProcessWave>();
		}
		if (info.TryGetProperty(PropertyName._wave, out var value7))
		{
			_wave = value7.As<TowerDefenseBattleFeatureWave>();
		}
		if (info.TryGetProperty(PropertyName._lookStar, out var value8))
		{
			_lookStar = value8.As<TowerDefenseBattleFeatureLookStar>();
		}
		if (info.TryGetProperty(PropertyName._level, out var value9))
		{
			_level = value9.As<TowerDefenseLevelConfig>();
		}
	}
}
