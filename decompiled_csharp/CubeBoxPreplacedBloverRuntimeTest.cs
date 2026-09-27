using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/CubeBoxPreplacedBloverRuntimeTest.cs")]
public class CubeBoxPreplacedBloverRuntimeTest : Node
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
		_ = 2;
		try
		{
			typeof(ResourceManager).GetField("_gameplayResourceLoadState", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ResourceManager.Instance, GameplayResourceLoadState.Ready);
			Register("Diamond/CubeBox", "CubeBox");
			Register("Chapter0/Blover", "Blover");
			Register("Chapter8/CoffeeBlover", "CoffeeBlover");
			Register("Chapter8/HypnoBlover", "HypnoBlover");
			_control = new CubeBoxBloverControl
			{
				isInit = true,
				levelConfig = new TowerDefenseLevelConfig()
			};
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
			string[] array = new string[3] { "PlantBlover", "PlantCoffeeBlover", "PlantHypnoBlover" };
			foreach (string key in array)
			{
				await Probe(key, preplaced: true);
				await Probe(key, preplaced: false);
			}
			_control.QueueFree();
			await WaitFrames(5);
			GD.Print($"CUBE_BOX_BLOVER_RESULT passed={_failures == 0 && _checks == 30} checks={_checks} failures={_failures}");
			GetTree().Quit((_failures != 0 || _checks != 30) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PushError(ex.ToString());
			GetTree().Quit(2);
		}
	}

	private void Register(string folder, string name)
	{
		string text = "res://Asset/Anime/Character/Plant/" + folder;
		ResourceManager.Instance.TOWERDEFENSE_PACKETS["Plant" + name] = GD.Load<TowerDefensePacketConfig>(text + "/Packet/Plant" + name + ".tres");
		ResourceManager.Instance.TOWERDEFENSE_CHARCATERS["Plant" + name] = GD.Load<PackedScene>(text + "/Scene/TowerDefensePlant" + name + ".tscn");
	}

	private async Task Probe(string key, bool preplaced)
	{
		_control.isGameRunning = !preplaced;
		GD.Print($"CUBE_BOX_BLOVER_CASE key={key} preplaced={preplaced}");
		TowerDefensePacketBankData towerDefensePacketBankData = new TowerDefensePacketBankData();
		towerDefensePacketBankData.category["White"] = new Godot.Collections.Array { key };
		ResourceManager.Instance.TOWERDEFENSE_PACKETBANKS["PlantPresentBox"] = towerDefensePacketBankData;
		ResourceManager.Instance.TOWERDEFENSE_PACKETBANKS["GeneralPlant"] = towerDefensePacketBankData;
		TowerDefenseBattleFeaturePreSpawn preSpawn = new TowerDefenseBattleFeaturePreSpawn
		{
			control = _control,
			config = new TowerDefenseBattleFeaturePreSpawnConfig()
		};
		preSpawn.config.preSpawnList.Add(new TowerDefenseLevelPreSpawnConfig
		{
			packetName = "PlantCubeBox",
			gridPos = new Vector2I(4, 3)
		});
		_control.featureDictionary["PreSpawn"] = preSpawn;
		if (preplaced)
		{
			await preSpawn.GameEntry();
		}
		else
		{
			TowerDefenseManager.GetPacketConfig("PlantCubeBox").Plant(new Vector2I(4, 3), playAudio: false);
		}
		await WaitFrames(preplaced ? 15 : 3);
		List<TowerDefensePlant> plants = new List<TowerDefensePlant>();
		int completed = 0;
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefensePlant towerDefensePlant && towerDefensePlant.config.name == key)
			{
				plants.Add(towerDefensePlant);
				towerDefensePlant.componentManager.GetRuntime<BloverComponent>().OnBlowOver += () =>
				{
					completed++;
				};
			}
		}
		Check(plants.Count == 8, key + "：真实魔方盒子必须在周围生成八个植物。");
		Check(preSpawn.preSpawnList.Count == (preplaced ? 9 : 0), key + "：开战前必须登记盒子和八个全息植物，局内生成无需登记。");
		Check(plants.TrueForAll((TowerDefensePlant plant) => plant.instance.hologram && !plant.isDestroy && (!preplaced || !plant.instance.invincible)), key + "：保留全息属性，开战前不能提前启动吹风。");
		_control.isGameRunning = true;
		if (preplaced)
		{
			await preSpawn.GameStart();
		}
		await WaitFrames(180);
		Check(completed == 8, $"{key}：开战后八个预生成植物必须完成吹风，实际{completed}个。");
		Check(plants.TrueForAll((TowerDefensePlant plant) => !GodotObject.IsInstanceValid(plant) || plant.isDestroy), key + "：技能结束后必须正常销毁。");
		_control.isGameRunning = false;
		foreach (Node child2 in _control.characterNode.GetChildren())
		{
			if (child2 is TowerDefenseCharacter towerDefenseCharacter)
			{
				towerDefenseCharacter.suppressDeathrattles = true;
				towerDefenseCharacter.ClearFromMap();
			}
		}
		await WaitFrames(5);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.Print("CUBE_BOX_BLOVER_FAILURE " + message);
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
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "folder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Register && args.Count == 2)
		{
			Register(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
