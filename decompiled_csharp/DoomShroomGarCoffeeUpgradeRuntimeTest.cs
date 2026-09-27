using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/DoomShroomGarCoffeeUpgradeRuntimeTest.cs")]
public class DoomShroomGarCoffeeUpgradeRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Register = "Register";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _control = "_control";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private CubeBoxBloverControl _control;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		_ = 3;
		try
		{
			typeof(ResourceManager).GetField("_gameplayResourceLoadState", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ResourceManager.Instance, GameplayResourceLoadState.Ready);
			Register("PlantGargantuarShroom", "Plant/Chapter2/GargantuarShroom/Packet/PlantGargantuarShroom.tres", "Plant/Chapter2/GargantuarShroom/Scene/TowerDefensePlantGargantuarShroom.tscn");
			Register("PlantCoffeebean", "Plant/Chapter0/Coffeebean/Packet/PlantCoffeebean.tres", "Plant/Chapter0/Coffeebean/Scene/TowerDefensePlantCoffeebean.tscn");
			Register("PlantDoomShroomGar", "Plant/Cover/DoomShroomGar/Packet/DoomShroomGar.tres", "Plant/Cover/DoomShroomGar/Scene/TowerDefensePlantDoomShroomGar.tscn");
			Register("CraterDayGround", "Crater/CraterDayGround/Packet/CraterDayGround.tres", "Crater/CraterDayGround/Scene/TowerDefenseCraterDayGround.tscn");
			_control = new CubeBoxBloverControl
			{
				isInit = true,
				isGameRunning = true,
				levelConfig = new TowerDefenseLevelConfig()
			};
			AddChild(_control, forceReadableName: false, InternalMode.Disabled);
			_control.characterNode = new Node2D();
			_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			instance.currentControl = _control;
			TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
			{
				isNight = false
			};
			instance.gridNum = towerDefenseMapConfig.gridNum;
			instance.gridSize = towerDefenseMapConfig.gridSize;
			instance.gridBeginPos = towerDefenseMapConfig.gridBeginPos;
			TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = new TowerDefenseBattleFeatureMap
			{
				config = towerDefenseMapConfig,
				mapConfig = towerDefenseMapConfig,
				control = _control
			};
			towerDefenseBattleFeatureMap.mapControl = new CubeBoxBloverMap
			{
				mapFeature = towerDefenseBattleFeatureMap
			};
			_control.AddChild(towerDefenseBattleFeatureMap.mapControl, forceReadableName: false, InternalMode.Disabled);
			_control.featureDictionary["Map"] = towerDefenseBattleFeatureMap;
			towerDefenseBattleFeatureMap.PlantGridInit();
			for (int i = 1; i <= 9; i++)
			{
				for (int j = 1; j <= 5; j++)
				{
					towerDefenseBattleFeatureMap.GetPlantGridCell(new Vector2I(i, j)).Init(new TowerDefenseCellConfig());
				}
			}
			await Probe(coffeeFirst: true, new Vector2I(3, 2));
			await Probe(coffeeFirst: false, new Vector2I(6, 4));
			await Probe(coffeeFirst: true, new Vector2I(2, 3), plantOnPlant: true);
			_control.isGameRunning = false;
			_control.QueueFree();
			await WaitFrames(5);
			bool flag = _failures == 0 && _checks == 16;
			GD.Print($"DOOM_GAR_COFFEE_UPGRADE_RESULT passed={flag} checks={_checks} failures={_failures}");
			GetTree().Quit((!flag) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PushError(ex.ToString());
			GetTree().Quit(2);
		}
	}

	private void Register(string key, string packet, string scene)
	{
		ResourceManager.Instance.TOWERDEFENSE_PACKETS[key] = GD.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/" + packet);
		ResourceManager.Instance.TOWERDEFENSE_CHARCATERS[key] = GD.Load<PackedScene>("res://Asset/Anime/Character/" + scene);
	}

	private async Task Probe(bool coffeeFirst, Vector2I grid, bool plantOnPlant = false)
	{
		TowerDefensePlant source = TowerDefenseManager.GetPacketConfig("PlantGargantuarShroom").Plant(grid, playAudio: false) as TowerDefensePlant;
		await WaitFrames(10);
		Check(source.IsSleep(), "白天红眼菇必须先进入睡眠。");
		if (coffeeFirst)
		{
			TowerDefenseManager.GetPacketConfig("PlantCoffeebean").Plant(grid, playAudio: false);
			await WaitFrames(120);
			Check(source.instance.wakeUp && !source.IsSleep() && source.componentAlive, "普通咖啡豆必须真实唤醒红眼菇。");
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantDoomShroomGar");
		TowerDefensePlant upgrade = (plantOnPlant ? packetConfig.PlantOnPlant(source, playAudio: false) : packetConfig.Plant(grid, playAudio: false)) as TowerDefensePlant;
		int explosions = 0;
		if (upgrade.IsNodeReady())
		{
			upgrade.componentManager.GetRuntime<ExplodeComponent>().OnExplode += () =>
			{
				explosions++;
			};
		}
		else
		{
			upgrade.Ready += () =>
			{
				upgrade.componentManager.GetRuntime<ExplodeComponent>().OnExplode += () =>
				{
					explosions++;
				};
			};
		}
		await WaitFrames(3);
		SleepComponent runtime = upgrade.componentManager.GetRuntime<SleepComponent>();
		GD.Print($"DOOM_GAR_TRACE coffeeFirst={coffeeFirst} plantOnPlant={plantOnPlant} wake={upgrade.instance.wakeUp} sleep={upgrade.instance.sleep} alive={upgrade.componentAlive} state={upgrade.CurrentStateHandle?.StableId}");
		if (coffeeFirst)
		{
			Check(upgrade.instance.wakeUp && !upgrade.IsSleep() && upgrade.componentAlive, "升级必须继承咖啡唤醒状态并恢复技能组件。");
			Check(!GodotObject.IsInstanceValid(runtime.sleepSprite) || !runtime.sleepSprite.Visible, "已唤醒升级植物不能残留睡眠特效。");
		}
		else
		{
			await WaitFrames(90);
			Check(upgrade.IsSleep() && explosions == 0, "未唤醒时升级不能提前爆炸。");
			TowerDefenseManager.GetPacketConfig("PlantCoffeebean").Plant(grid, playAudio: false);
		}
		await WaitFrames(180);
		Check(explosions == 1, $"coffeeFirst={coffeeFirst}：唤醒后必须爆炸一次，实际{explosions}次。");
		Check(!GodotObject.IsInstanceValid(upgrade) || upgrade.isDestroy, "爆炸结束后必须正常清理升级植物。");
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.Print("DOOM_GAR_COFFEE_UPGRADE_FAILURE " + message);
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(3)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Register, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.Register && args.Count == 3)
		{
			Register(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
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
		if (method == MethodName.Register)
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
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<CubeBoxBloverControl>(in value);
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
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
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
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._control, out var value))
		{
			_control = value.As<CubeBoxBloverControl>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value2))
		{
			_checks = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value3))
		{
			_failures = value3.As<int>();
		}
	}
}
