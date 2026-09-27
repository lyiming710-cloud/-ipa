using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/TallnutArmedZamboniRuntimeTest.cs")]
public class TallnutArmedZamboniRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Register = "Register";

		public static readonly StringName FindArmor = "FindArmor";

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

	private TallnutArmedZamboniControl _control;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		_ = 5;
		try
		{
			typeof(ResourceManager).GetField("_gameplayResourceLoadState", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ResourceManager.Instance, GameplayResourceLoadState.Ready);
			Register("PlantTallnutArmed", "Plant/Star/TallnutArmed/Packet/PlantTallnutArmed.tres", "Plant/Star/TallnutArmed/Scene/TowerDefensePlantTallnutArmed.tscn");
			Register("ZombieGargantuarZamboni", "Zombie/Challenge/GargantuarZamboni/Packet/Base/ZombieGargantuarZamboni.tres", "Zombie/Challenge/GargantuarZamboni/Scene/Base/TowerDefenseZombieGargantuarZamboni.tscn");
			Register("ZombieGargantuar", "Zombie/Chapter1/Gargantuar/Packet/Base/ZombieGargantuar.tres", "Zombie/Chapter1/Gargantuar/Scene/Base/TowerDefenseZombieGargantuar.tscn");
			Register("ZombieGargantuarZamboniRedEyes", "Zombie/Challenge/GargantuarZamboni/Packet/RedEyes/ZombieGargantuarZamboniRedEyes.tres", "Zombie/Challenge/GargantuarZamboni/Scene/RedEyes/TowerDefenseZombieGargantuarZamboniRedEyes.tscn");
			Register("ZombieGargantuarRedEyes", "Zombie/Chapter1/Gargantuar/Packet/Redeyes/ZombieGargantuarRedeyes.tres", "Zombie/Chapter1/Gargantuar/Scene/RedEyes/TowerDefenseZombieGargantuarRedEyes.tscn");
			_control = new TallnutArmedZamboniControl
			{
				isInit = true,
				levelConfig = new TowerDefenseLevelConfig()
			};
			if (OS.GetEnvironment("TALLNUT_ZAMBONI_MODE") == "IZM")
			{
				((TowerDefenseLevelConfig)_control.levelConfig).finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM;
			}
			AddChild(_control, forceReadableName: false, InternalMode.Disabled);
			_control.characterNode = new Node2D();
			_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			instance.currentControl = _control;
			TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig();
			instance.gridNum = towerDefenseMapConfig.gridNum;
			instance.gridSize = towerDefenseMapConfig.gridSize;
			instance.gridBeginPos = towerDefenseMapConfig.gridBeginPos;
			TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = new TowerDefenseBattleFeatureMap
			{
				config = towerDefenseMapConfig,
				mapConfig = towerDefenseMapConfig,
				control = _control
			};
			TallnutArmedZamboniMap tallnutArmedZamboniMap = new TallnutArmedZamboniMap
			{
				mapFeature = towerDefenseBattleFeatureMap,
				mapIceCap = new Node2D()
			};
			tallnutArmedZamboniMap.AddChild(tallnutArmedZamboniMap.mapIceCap, forceReadableName: false, InternalMode.Disabled);
			_control.AddChild(tallnutArmedZamboniMap, forceReadableName: false, InternalMode.Disabled);
			towerDefenseBattleFeatureMap.mapControl = tallnutArmedZamboniMap;
			_control.featureDictionary["Map"] = towerDefenseBattleFeatureMap;
			towerDefenseBattleFeatureMap.PlantGridInit();
			for (int i = 1; i <= 9; i++)
			{
				for (int j = 1; j <= 5; j++)
				{
					towerDefenseBattleFeatureMap.GetPlantGridCell(new Vector2I(i, j)).Init(new TowerDefenseCellConfig());
				}
			}
			await Probe("ZombieGargantuarZamboni", 1, 3);
			await Probe("ZombieGargantuarZamboniRedEyes", 2, 3);
			await Probe("ZombieGargantuarZamboni", 3, 2);
			await Probe("ZombieGargantuarZamboniRedEyes", 4, 2);
			_control.isGameRunning = false;
			foreach (Node child in _control.characterNode.GetChildren())
			{
				if (child is TowerDefenseCharacter towerDefenseCharacter)
				{
					towerDefenseCharacter.suppressDeathrattles = true;
					towerDefenseCharacter.ClearFromMap();
				}
			}
			await WaitFrames(5);
			_control.QueueFree();
			await WaitFrames(5);
			GD.Print($"TALLNUT_ZAMBONI_RESULT passed={_failures == 0 && _checks == 32} checks={_checks} failures={_failures}");
			GetTree().Quit((_failures != 0 || _checks != 32) ? 2 : 0);
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

	private async Task Probe(string packet, int row, int column)
	{
		Vector2I grid = new Vector2I(3, row);
		TowerDefensePlantTallnutArmed plant = TowerDefenseManager.GetPacketConfig("PlantTallnutArmed").Plant(grid, playAudio: true, noLimit: true) as TowerDefensePlantTallnutArmed;
		await WaitFrames(5);
		Check(GodotObject.IsInstanceValid(plant) && plant.instance.hitpoints == 8000.0, $"{packet}/{column}：初始装甲必须为8000血。");
		_control.isGameRunning = true;
		TowerDefenseZombie zombie = TowerDefenseManager.GetPacketConfig(packet).Plant(new Vector2I(column, row), playAudio: true, noLimit: true) as TowerDefenseZombie;
		for (int frame = 0; frame < 90; frame++)
		{
			await WaitFrames(1);
			if (frame == 15)
			{
				TowerDefensePlantTallnutArmed towerDefensePlantTallnutArmed = FindArmor(row);
				Check(towerDefensePlantTallnutArmed != null && towerDefensePlantTallnutArmed.dieNum == 1 && towerDefensePlantTallnutArmed.instance.hitpoints == 6000.0, $"{packet}/{column}：爆胎后必须先保留6000血的下一层装甲。");
			}
		}
		Check(!GodotObject.IsInstanceValid(zombie) || zombie.isDestroy, $"{packet}/{column}：车辆接触尖刺后必须爆胎。");
		TowerDefensePlantTallnutArmed survivor = FindArmor(row);
		int expectedStage = ((column != 2) ? 1 : 2);
		Check(survivor != null && survivor.dieNum == expectedStage && survivor.instance.hitpoints == (double)(8000 - expectedStage * 2000), $"{packet}/{column}：后续巨人的一次正常锤击只能再打掉一层，冰道不能阻止复活。");
		int num = 0;
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefenseZombie towerDefenseZombie && towerDefenseZombie.gridPos.Y == row && !towerDefenseZombie.isDestroy)
			{
				num++;
				towerDefenseZombie.suppressDeathrattles = true;
				towerDefenseZombie.ClearFromMap();
			}
		}
		Check(num == 1, $"{packet}/{column}：爆胎后必须只生成一个巨人。");
		await WaitFrames(5);
		if (survivor == null)
		{
			return;
		}
		for (int frame = expectedStage + 1; frame <= 4; frame++)
		{
			survivor.Hurt(8000.0);
			await WaitFrames(10);
			survivor = FindArmor(row);
			Check((frame == 4) ? (survivor == null) : (survivor != null && survivor.dieNum == frame && survivor.instance.hitpoints == (double)(8000 - frame * 2000)), $"{packet}/{column}：第{frame}次致命伤害后装甲阶段与血量必须正确。");
			if (survivor == null)
			{
				break;
			}
		}
		if (column == 2)
		{
			Check(!TowerDefenseManager.GetMapCell(grid).CanPacketPlant(TowerDefenseManager.GetPacketConfig("PlantTallnutArmed"), noLimit: true), packet + "：原地复活不能解除冰道对普通种植的限制。");
		}
	}

	private TowerDefensePlantTallnutArmed FindArmor(int row)
	{
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefensePlantTallnutArmed towerDefensePlantTallnutArmed && towerDefensePlantTallnutArmed.gridPos.Y == row && !towerDefensePlantTallnutArmed.isDestroy)
			{
				return towerDefensePlantTallnutArmed;
			}
		}
		return null;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.Print("TALLNUT_ZAMBONI_FAILURE " + message);
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
		return new List<Godot.Bridge.MethodInfo>(4)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Register, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.FindArmor, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "row", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.FindArmor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlantTallnutArmed>(FindArmor(VariantUtils.ConvertTo<int>(in args[0])));
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
		if (method == MethodName.FindArmor)
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
			_control = VariantUtils.ConvertTo<TallnutArmedZamboniControl>(in value);
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
			_control = value.As<TallnutArmedZamboniControl>();
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
