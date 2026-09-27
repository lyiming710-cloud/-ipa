using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/GatlingZamboniHolographicBaseRuntimeTest.cs")]
public class GatlingZamboniHolographicBaseRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMap = "CreateMap";

		public static readonly StringName AdvanceUntilPast = "AdvanceUntilPast";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _control = "_control";

		public static readonly StringName _bulletField = "_bulletField";

		public static readonly StringName _simulationFrame = "_simulationFrame";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ZamboniScenePath = "res://Asset/Anime/Character/Zombie/Chapter2/Zamboni/Scene/GatlingPea/TowerDefenseZombieZamboniGatlingPea.tscn";

	private const string SunflowerScenePath = "res://Asset/Anime/Character/Plant/Chapter9/SunFlowerQX/Scene/TowerDefensePlantSunFlowerQX.tscn";

	private const string NormalSunflowerScenePath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn";

	private readonly List<int> _spawnedBullets = new List<int>();

	private int _checks;

	private int _failures;

	private GatlingZamboniHolographicBaseControl _control;

	private BulletField _bulletField;

	private ulong _simulationFrame;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager.currentControl;
		Vector2 previousGridBegin = manager.gridBeginPos;
		Vector2 previousGridSize = manager.gridSize;
		Vector2I previousGridNum = manager.gridNum;
		ProjectileUpdateManager projectileManager = ProjectileUpdateManager.Instance;
		ProcessModeEnum previousProjectileProcessMode = projectileManager.ProcessMode;
		TowerDefenseBattleFeatureMap map = null;
		TowerDefenseMapControl mapControl = null;
		bool ownsBulletField = false;
		try
		{
			manager.gridBeginPos = Vector2.Zero;
			manager.gridSize = new Vector2(100f, 76f);
			manager.gridNum = new Vector2I(9, 5);
			_control = new GatlingZamboniHolographicBaseControl
			{
				isGameRunning = false,
				isInit = true,
				levelConfig = new TowerDefenseLevelConfig(),
				characterNode = new Node2D()
			};
			AddChild(_control, forceReadableName: false, InternalMode.Disabled);
			_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
			manager.currentControl = _control;
			mapControl = new TowerDefenseMapControl();
			map = CreateMap(mapControl, manager);
			_control.featureDictionary["Map"] = map;
			TowerDefenseProjectileRegistry.Init();
			projectileManager.ProcessMode = ProcessModeEnum.Disabled;
			_bulletField = BulletField.Instance;
			if (!GodotObject.IsInstanceValid(_bulletField))
			{
				_bulletField = new BulletField();
				AddChild(_bulletField, forceReadableName: false, InternalMode.Disabled);
				ownsBulletField = true;
			}
			_bulletField.OnBulletSpawned += _spawnedBullets.Add;
			TowerDefensePlantSunFlowerQX sunflower = Spawn<TowerDefensePlantSunFlowerQX>("res://Asset/Anime/Character/Plant/Chapter9/SunFlowerQX/Scene/TowerDefensePlantSunFlowerQX.tscn", new Vector2I(4, 3));
			TowerDefensePlant normalSunflower = Spawn<TowerDefensePlant>("res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn", new Vector2I(2, 3));
			TowerDefenseZombieZamboniGatlingPea zamboni = Spawn<TowerDefenseZombieZamboniGatlingPea>("res://Asset/Anime/Character/Zombie/Chapter2/Zamboni/Scene/GatlingPea/TowerDefenseZombieZamboniGatlingPea.tscn", new Vector2I(7, 3));
			await WaitFrames(3);
			foreach (Node child in _control.characterNode.GetChildren())
			{
				child.ProcessMode = ProcessModeEnum.Disabled;
			}
			_control.isGameRunning = true;
			_simulationFrame = Engine.GetPhysicsFrames();
			FireComponent runtime = zamboni.componentManager.GetRuntime<FireComponent>("character.fire");
			Check(runtime != null && !runtime.IsReleased, "机枪冰车必须加载正式开火组件。");
			FireComponentCheckConfig fireComponentCheckConfig = (runtime.runningCheck = runtime.fireCheckList[0]);
			sunflower.SetupBaseState();
			Check(sunflower.isDowned && !sunflower.die && sunflower.instance.hitpoints > 0.0, "全息葵必须进入存活的底座状态。");
			normalSunflower.instance.canBeCollection = false;
			Check(!runtime.CanFireCheckOnce(fireComponentCheckConfig.GetProjectile(), fireComponentCheckConfig.GetCollisionFlags()), "只有全息葵底座时，机枪冰车不应因此开火。");
			normalSunflower.instance.canBeCollection = true;
			Check(runtime.CanFireCheckOnce(fireComponentCheckConfig.GetProjectile(), fireComponentCheckConfig.GetCollisionFlags()), "底座后方存在普通植物时，机枪冰车仍应开火。");
			double hitpoints = sunflower.instance.hitpoints;
			double hitpoints2 = normalSunflower.instance.hitpoints;
			int index = FireOneProjectile(runtime);
			AdvanceUntilPast(index, sunflower.GlobalPosition.X - 50f);
			Check(_bulletField.IsBulletActive(index) && _bulletField.GetBulletDataRef(index).pos.X < sunflower.GlobalPosition.X - 50f, "真实子弹必须穿过全息葵底座并继续飞行。");
			Check(Mathf.IsEqualApprox(sunflower.instance.hitpoints, hitpoints), "子弹穿过底座后，底座生命值必须保持不变。");
			AdvanceUntilPast(index, normalSunflower.GlobalPosition.X - 60f);
			Check(normalSunflower.instance.hitpoints < hitpoints2 && !_bulletField.IsBulletActive(index), "穿过底座的同一颗子弹必须命中后方普通植物并结算伤害。");
			GD.Print($"GATLING_ZAMBONI_BASE_PASS baseHp={hitpoints}->{sunflower.instance.hitpoints} rearHp={hitpoints2}->{normalSunflower.instance.hitpoints}");
			sunflower.RestoreNormalState();
			normalSunflower.instance.canBeCollection = false;
			Check(!sunflower.isDowned && runtime.CanFireCheckOnce(fireComponentCheckConfig.GetProjectile(), fireComponentCheckConfig.GetCollisionFlags()), "全息葵复活后必须重新成为子弹目标。");
			double hitpoints3 = sunflower.instance.hitpoints;
			index = FireOneProjectile(runtime);
			AdvanceUntilPast(index, sunflower.GlobalPosition.X - 60f);
			Check(sunflower.instance.hitpoints < hitpoints3 && !_bulletField.IsBulletActive(index), "机枪冰车子弹必须正常伤害复活后的全息葵。");
			sunflower.SetupBaseState();
			zamboni.GlobalPosition = sunflower.GlobalPosition;
			zamboni.gridPos = sunflower.gridPos;
			Check(zamboni.attackComponent.GetTarget() == sunflower, "车身接触索敌必须仍能识别具有爆胎能力的底座。");
			Check((sunflower.instance.physiqueTypeFlags & 0x10) != 0 && sunflower.instance.spikeHurt > 0.0, "底座必须保留爆胎能力。");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[GatlingZamboniHolographicBaseRuntimeTest] {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(_bulletField))
			{
				_bulletField.OnBulletSpawned -= _spawnedBullets.Add;
				_bulletField.ClearActiveBullets();
				if (ownsBulletField)
				{
					_bulletField.QueueFree();
				}
			}
			if (GodotObject.IsInstanceValid(_control))
			{
				_control.isGameRunning = false;
				_control.QueueFree();
			}
			await WaitFrames(3);
			manager.currentControl = previousControl;
			manager.gridBeginPos = previousGridBegin;
			manager.gridSize = previousGridSize;
			manager.gridNum = previousGridNum;
			projectileManager.ProcessMode = previousProjectileProcessMode;
			map?.Destroy();
			if (GodotObject.IsInstanceValid(mapControl))
			{
				mapControl.Free();
			}
		}
		bool flag = _failures == 0 && _checks == 13;
		GD.Print($"GATLING_ZAMBONI_HOLOGRAPHIC_BASE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private TowerDefenseBattleFeatureMap CreateMap(TowerDefenseMapControl mapControl, TowerDefenseManager manager)
	{
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
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			control = _control,
			mapControl = mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig,
			rect = new Rect2(-200f, -200f, 1400f, 900f)
		});
		towerDefenseBattleFeatureMap.PlantGridInit();
		return towerDefenseBattleFeatureMap;
	}

	private T Spawn<T>(string scenePath, Vector2I grid) where T : TowerDefenseCharacter
	{
		T val = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Reuse).Instantiate<T>(PackedScene.GenEditState.Disabled);
		val.inGame = true;
		val.gridPos = grid;
		val.Position = TowerDefenseManager.GetMapCellPlantPos(grid);
		_control.characterNode.AddChild(val, forceReadableName: false, InternalMode.Disabled);
		return val;
	}

	private int FireOneProjectile(FireComponent fire)
	{
		_bulletField.ClearActiveBullets();
		_spawnedBullets.Clear();
		fire.Fire();
		Check(_spawnedBullets.Count == 1, "一次正式开火必须生成一颗机枪冰车子弹。");
		return _spawnedBullets[0];
	}

	private void AdvanceUntilPast(int index, float x)
	{
		for (int i = 0; i < 180; i++)
		{
			if (!_bulletField.IsBulletActive(index))
			{
				break;
			}
			if (!(_bulletField.GetBulletDataRef(index).pos.X >= x))
			{
				break;
			}
			_bulletField.Update(1.0 / 60.0, ++_simulationFrame);
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
			GD.PushError("[GatlingZamboniHolographicBaseRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMap, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AdvanceUntilPast, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "x", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateMap && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMap(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<TowerDefenseManager>(in args[1])));
			return true;
		}
		if (method == MethodName.AdvanceUntilPast && args.Count == 2)
		{
			AdvanceUntilPast(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
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
		if (method == MethodName.CreateMap)
		{
			return true;
		}
		if (method == MethodName.AdvanceUntilPast)
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
			_control = VariantUtils.ConvertTo<GatlingZamboniHolographicBaseControl>(in value);
			return true;
		}
		if (name == PropertyName._bulletField)
		{
			_bulletField = VariantUtils.ConvertTo<BulletField>(in value);
			return true;
		}
		if (name == PropertyName._simulationFrame)
		{
			_simulationFrame = VariantUtils.ConvertTo<ulong>(in value);
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
		if (name == PropertyName._bulletField)
		{
			value = VariantUtils.CreateFrom(in _bulletField);
			return true;
		}
		if (name == PropertyName._simulationFrame)
		{
			value = VariantUtils.CreateFrom(in _simulationFrame);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._bulletField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._simulationFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._bulletField, Variant.From(in _bulletField));
		info.AddProperty(PropertyName._simulationFrame, Variant.From(in _simulationFrame));
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
			_control = value3.As<GatlingZamboniHolographicBaseControl>();
		}
		if (info.TryGetProperty(PropertyName._bulletField, out var value4))
		{
			_bulletField = value4.As<BulletField>();
		}
		if (info.TryGetProperty(PropertyName._simulationFrame, out var value5))
		{
			_simulationFrame = value5.As<ulong>();
		}
	}
}
