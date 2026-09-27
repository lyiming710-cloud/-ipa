using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/GravestoneInitPreSpawnReservationRuntimeTest.cs")]
public class GravestoneInitPreSpawnReservationRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName CreatePreSpawnData = "CreatePreSpawnData";

		public static readonly StringName CreateSingleCellGravestoneEvent = "CreateSingleCellGravestoneEvent";

		public static readonly StringName CountCellCharacters = "CountCellCharacters";

		public static readonly StringName CountLiveCharacters = "CountLiveCharacters";

		public static readonly StringName RegisterRealFixtures = "RegisterRealFixtures";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

		public static readonly StringName RestoreRealFixtures = "RestoreRealFixtures";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string HolographicGravestonePacketPath = "res://Asset/Anime/Character/GraveStone/TargetQX/Packet/GraveStoneTargetQX.tres";

	private const string HolographicGravestoneScenePath = "res://Asset/Anime/Character/GraveStone/TargetQX/Scene/TowerDefenseGraveStoneTargetQX.tscn";

	private const string TimeBombPacketPath = "res://Asset/Anime/Character/GraveStone/TimeBomb/Packet/TimeBomb.tres";

	private const string TimeBombScenePath = "res://Asset/Anime/Character/GraveStone/TimeBomb/Scene/TowerDefenseTimeBomb.tscn";

	private static readonly Vector2I TestGridPosition = new Vector2I(5, 3);

	private static readonly Vector2I EventOnlyGridPosition = new Vector2I(6, 3);

	private int _checks;

	private int _failures;

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		GravestoneInitPreSpawnReservationRuntimeControlStub control = null;
		TowerDefenseBattleFeaturePreSpawn preSpawnFeature = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseMap currentMap = null;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager 自动加载必须可用。");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager 自动加载必须可用。");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("墓碑冲突回归测试缺少必要的自动加载。");
				}
				RegisterRealFixtures();
				control = new GravestoneInitPreSpawnReservationRuntimeControlStub
				{
					Name = "GravestoneInitPreSpawnReservationRuntimeControl",
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
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				currentMap = (mapFeature.currentMap = new TowerDefenseMap
				{
					Name = "CurrentMap"
				});
				preSpawnFeature = new TowerDefenseBattleFeaturePreSpawn
				{
					control = control
				};
				preSpawnFeature.Init(CreatePreSpawnData());
				control.featureDictionary[new StringName("PreSpawn")] = preSpawnFeature;
				Check(preSpawnFeature.HasPendingGravestoneReservation(TestGridPosition), "Init 事件执行前必须预留固定预放置墓碑格。");
				CreateSingleCellGravestoneEvent("TimeBomb", TestGridPosition).Execute();
				CreateSingleCellGravestoneEvent("TimeBomb", EventOnlyGridPosition).Execute();
				CreateSingleCellGravestoneEvent("GraveStoneTargetQX", EventOnlyGridPosition).Execute();
				await preSpawnFeature.GameEntry();
				Check(preSpawnFeature.HasPendingGravestoneReservation(TestGridPosition), "预放置角色尚未执行延迟占格时必须继续保留格子预留。");
				await WaitFrames(6);
				Check(!preSpawnFeature.HasPendingGravestoneReservation(TestGridPosition), "预放置延迟占格完成后必须释放临时格子预留。");
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(TestGridPosition);
				Check(GodotObject.IsInstanceValid(mapCell), "竞争格子必须存在。");
				Check(CountCellCharacters(mapCell, "GraveStoneTargetQX") == 1, "固定预放置的全息墓碑必须保留在目标格。");
				Check(CountCellCharacters(mapCell, "TimeBomb") == 0, "Init 随机定时炸弹不得与预放置全息墓碑叠在同一格。");
				TowerDefenseCellInstance mapCell2 = TowerDefenseManager.GetMapCell(EventOnlyGridPosition);
				Check(CountCellCharacters(mapCell2, "TimeBomb") == 1, "同一 Init 阶段先执行的定时炸弹事件必须保留唯一落点。");
				Check(CountCellCharacters(mapCell2, "GraveStoneTargetQX") == 0, "后执行的全息墓碑事件不得抢占尚未完成延迟占格的定时炸弹格。");
				Check(CountLiveCharacters(control.characterNode, "GraveStoneTargetQX") == 1, "战场中必须只生成一个全息墓碑。");
				Check(CountLiveCharacters(control.characterNode, "TimeBomb") == 1, "战场中只能保留未与预放置冲突的一个定时炸弹。");
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[GravestoneInitPreSpawnReservationRuntimeTest] 发生未预期异常：{value}");
			}
		}
		finally
		{
			preSpawnFeature?.Destroy();
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (mapFeature != null)
			{
				mapFeature.mapControl = null;
				mapFeature.currentMap = null;
				mapFeature.Destroy();
			}
			if (GodotObject.IsInstanceValid(currentMap))
			{
				currentMap.Free();
			}
			if (GodotObject.IsInstanceValid(mapControl))
			{
				mapControl.Free();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			RestoreRealFixtures();
			await WaitFrames(8);
		}
		bool flag = _failures == 0;
		GD.Print($"GRAVESTONE_INIT_PRESPAWN_RESERVATION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseCellConfig item = new TowerDefenseCellConfig
		{
			pos = new Vector4I(1, 1, gridNum.X, gridNum.Y)
		};
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = new TowerDefenseMapConfig
			{
				gridNum = gridNum,
				cellConfig = new Array<TowerDefenseCellConfig> { item }
			}
		});
		towerDefenseBattleFeatureMap.PlantGridInit();
		return towerDefenseBattleFeatureMap;
	}

	private static Dictionary CreatePreSpawnData()
	{
		return new Dictionary { ["Packet"] = new Godot.Collections.Array
		{
			new Dictionary
			{
				["Name"] = "GraveStoneTargetQX",
				["GridPos"] = new Godot.Collections.Array { TestGridPosition.X, TestGridPosition.Y }
			}
		} };
	}

	private static TowerDefenseLevelEventGravestoneCreateRandom CreateSingleCellGravestoneEvent(string packetName, Vector2I gridPosition)
	{
		return new TowerDefenseLevelEventGravestoneCreateRandom
		{
			gravestoneNames = new Godot.Collections.Array { packetName },
			gravestoneNum = 1,
			gravestonePos = new Vector4I(gridPosition.X, gridPosition.Y, gridPosition.X, gridPosition.Y)
		};
	}

	private static int CountCellCharacters(TowerDefenseCellInstance cell, string configName)
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return 0;
		}
		cell.ClearEmpty();
		int num = 0;
		foreach (TowerDefenseCharacter character in cell.characterList)
		{
			if (GodotObject.IsInstanceValid(character) && !character.isDestroy && character.config?.name == configName)
			{
				num++;
			}
		}
		return num;
	}

	private static int CountLiveCharacters(Node characterNode, string configName)
	{
		if (!GodotObject.IsInstanceValid(characterNode))
		{
			return 0;
		}
		int num = 0;
		foreach (Node child in characterNode.GetChildren())
		{
			if (child is TowerDefenseCharacter towerDefenseCharacter && GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.isDestroy && towerDefenseCharacter.config?.name == configName)
			{
				num++;
			}
		}
		return num;
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("GraveStoneTargetQX", "res://Asset/Anime/Character/GraveStone/TargetQX/Packet/GraveStoneTargetQX.tres");
		RegisterPacket("TimeBomb", "res://Asset/Anime/Character/GraveStone/TimeBomb/Packet/TimeBomb.tres");
		RegisterCharacter("GraveStoneTargetQX", "res://Asset/Anime/Character/GraveStone/TargetQX/Scene/TowerDefenseGraveStoneTargetQX.tscn");
		RegisterCharacter("TimeBomb", "res://Asset/Anime/Character/GraveStone/TimeBomb/Scene/TowerDefenseTimeBomb.tscn");
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
		instance.TOWERDEFENSE_PACKETS[key] = ResourceLoader.Load<Resource>(path, null, ResourceLoader.CacheMode.Ignore);
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
		instance.TOWERDEFENSE_CHARCATERS[key] = ResourceLoader.Load<Resource>(path, null, ResourceLoader.CacheMode.Ignore);
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
			GD.PushError("[GravestoneInitPreSpawnReservationRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePreSpawnData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateSingleCellGravestoneEvent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountCellCharacters, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "configName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountLiveCharacters, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "characterNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "configName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreatePreSpawnData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreatePreSpawnData());
			return true;
		}
		if (method == MethodName.CreateSingleCellGravestoneEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelEventGravestoneCreateRandom>(CreateSingleCellGravestoneEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CountCellCharacters && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountCellCharacters(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CountLiveCharacters && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountLiveCharacters(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.CreatePreSpawnData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreatePreSpawnData());
			return true;
		}
		if (method == MethodName.CreateSingleCellGravestoneEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelEventGravestoneCreateRandom>(CreateSingleCellGravestoneEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CountCellCharacters && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountCellCharacters(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CountLiveCharacters && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountLiveCharacters(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.CreatePreSpawnData)
		{
			return true;
		}
		if (method == MethodName.CreateSingleCellGravestoneEvent)
		{
			return true;
		}
		if (method == MethodName.CountCellCharacters)
		{
			return true;
		}
		if (method == MethodName.CountLiveCharacters)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
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
	}
}
