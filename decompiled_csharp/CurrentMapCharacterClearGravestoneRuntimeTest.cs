using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/CurrentMapCharacterClearGravestoneRuntimeTest.cs")]
public class CurrentMapCharacterClearGravestoneRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName CountLiveGravestones = "CountLiveGravestones";

		public static readonly StringName CellHasLiveGravestone = "CellHasLiveGravestone";

		public static readonly StringName RegisterRealResources = "RegisterRealResources";

		public static readonly StringName RestoreRealResources = "RestoreRealResources";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _previousPacket = "_previousPacket";

		public static readonly StringName _previousCharacter = "_previousCharacter";

		public static readonly StringName _packetWasMissing = "_packetWasMissing";

		public static readonly StringName _characterWasMissing = "_characterWasMissing";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string PacketKey = "GraveStoneDefault";

	private const string PacketPath = "res://Asset/Anime/Character/GraveStone/Default/Packet/GraveStoneDefault.tres";

	private const string ScenePath = "res://Asset/Anime/Character/GraveStone/Default/Scene/TowerDefenseGraveStoneDefault.tscn";

	private const int ExpectedChecks = 9;

	private int _checks;

	private int _failures;

	private Resource _previousPacket;

	private Resource _previousCharacter;

	private bool _packetWasMissing;

	private bool _characterWasMissing;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		CurrentMapCharacterClearGravestoneControlStub control = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseMap currentMap = null;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("Required gameplay autoloads are unavailable.");
				}
				RegisterRealResources();
				control = new CurrentMapCharacterClearGravestoneControlStub
				{
					Name = "CurrentMapCharacterClearGravestoneControl",
					isGameRunning = true,
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
				TowerDefenseLevelEventCurrentMapCharacterClear clearEvent = new TowerDefenseLevelEventCurrentMapCharacterClear();
				TowerDefenseGravestone existing = LoadPacket()?.Plant(new Vector2I(5, 3), playAudio: false) as TowerDefenseGravestone;
				Check(GodotObject.IsInstanceValid(existing), "The fixture must create a real already-entered gravestone.");
				await WaitFrames(3);
				Check(CountLiveGravestones(manager) == 1, "The real default gravestone must be alive in the persistent Gravestone group.");
				Check(GetRegisteredGravestones(manager).Count == 0, "The default gravestone fixture must reproduce its intentional targeting-registry exclusion.");
				clearEvent.Execute();
				Check(GodotObject.IsInstanceValid(existing) && existing.isDestroy && existing.die, "CharacterClear must enter the normal destroy lifecycle for an existing gravestone.");
				await WaitFrames(3);
				Check(CountLiveGravestones(manager) == 0, "The existing gravestone must leave the battlefield after CharacterClear.");
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(5, 3));
				Check(GodotObject.IsInstanceValid(mapCell), "The real gravestone must use a valid map cell.");
				Check(!CellHasLiveGravestone(mapCell), "The cleared gravestone must leave map-cell occupancy.");
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[CurrentMapCharacterClearGravestoneRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
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
			RestoreRealResources();
			await WaitFrames(8);
		}
		bool flag = _failures == 0 && _checks == 9;
		GD.Print($"CURRENT_MAP_CHARACTER_CLEAR_GRAVESTONE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = new TowerDefenseMapConfig
			{
				gridNum = gridNum
			}
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
		return towerDefenseBattleFeatureMap;
	}

	private static TowerDefensePacketConfig LoadPacket()
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/GraveStone/Default/Packet/GraveStoneDefault.tres", null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private static List<TowerDefenseGravestone> GetRegisteredGravestones(TowerDefenseManager manager)
	{
		List<TowerDefenseGravestone> list = new List<TowerDefenseGravestone>();
		foreach (Variant item in manager.GetCharacter())
		{
			if (item.AsGodotObject() is TowerDefenseGravestone towerDefenseGravestone && GodotObject.IsInstanceValid(towerDefenseGravestone) && !towerDefenseGravestone.isDestroy)
			{
				list.Add(towerDefenseGravestone);
			}
		}
		return list;
	}

	private static int CountLiveGravestones(TowerDefenseManager manager)
	{
		int num = 0;
		foreach (Node item in manager.GetTree().GetNodesInGroup("Gravestone"))
		{
			if (item is TowerDefenseGravestone towerDefenseGravestone && GodotObject.IsInstanceValid(towerDefenseGravestone) && !towerDefenseGravestone.isDestroy)
			{
				num++;
			}
		}
		return num;
	}

	private static bool CellHasLiveGravestone(TowerDefenseCellInstance cell)
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return false;
		}
		cell.ClearEmpty();
		foreach (TowerDefenseCharacter character in cell.characterList)
		{
			if (character is TowerDefenseGravestone && GodotObject.IsInstanceValid(character) && !character.isDestroy)
			{
				return true;
			}
		}
		return false;
	}

	private void RegisterRealResources()
	{
		ResourceManager instance = ResourceManager.Instance;
		_packetWasMissing = !instance.TOWERDEFENSE_PACKETS.TryGetValue("GraveStoneDefault", out _previousPacket);
		_characterWasMissing = !instance.TOWERDEFENSE_CHARCATERS.TryGetValue("GraveStoneDefault", out _previousCharacter);
		instance.TOWERDEFENSE_PACKETS["GraveStoneDefault"] = ResourceLoader.Load<Resource>("res://Asset/Anime/Character/GraveStone/Default/Packet/GraveStoneDefault.tres", null, ResourceLoader.CacheMode.Ignore);
		instance.TOWERDEFENSE_CHARCATERS["GraveStoneDefault"] = ResourceLoader.Load<Resource>("res://Asset/Anime/Character/GraveStone/Default/Scene/TowerDefenseGraveStoneDefault.tscn", null, ResourceLoader.CacheMode.Ignore);
	}

	private void RestoreRealResources()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			if (_packetWasMissing)
			{
				instance.TOWERDEFENSE_PACKETS.Remove("GraveStoneDefault");
			}
			else if (GodotObject.IsInstanceValid(_previousPacket))
			{
				instance.TOWERDEFENSE_PACKETS["GraveStoneDefault"] = _previousPacket;
			}
			if (_characterWasMissing)
			{
				instance.TOWERDEFENSE_CHARCATERS.Remove("GraveStoneDefault");
			}
			else if (GodotObject.IsInstanceValid(_previousCharacter))
			{
				instance.TOWERDEFENSE_CHARCATERS["GraveStoneDefault"] = _previousCharacter;
			}
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
			GD.PushError("[CurrentMapCharacterClearGravestoneRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CountLiveGravestones, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CellHasLiveGravestone, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterRealResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreRealResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.LoadPacket && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket());
			return true;
		}
		if (method == MethodName.CountLiveGravestones && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountLiveGravestones(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0])));
			return true;
		}
		if (method == MethodName.CellHasLiveGravestone && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CellHasLiveGravestone(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterRealResources && args.Count == 0)
		{
			RegisterRealResources();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreRealResources && args.Count == 0)
		{
			RestoreRealResources();
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
		if (method == MethodName.LoadPacket && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket());
			return true;
		}
		if (method == MethodName.CountLiveGravestones && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountLiveGravestones(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0])));
			return true;
		}
		if (method == MethodName.CellHasLiveGravestone && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CellHasLiveGravestone(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0])));
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
		if (method == MethodName.LoadPacket)
		{
			return true;
		}
		if (method == MethodName.CountLiveGravestones)
		{
			return true;
		}
		if (method == MethodName.CellHasLiveGravestone)
		{
			return true;
		}
		if (method == MethodName.RegisterRealResources)
		{
			return true;
		}
		if (method == MethodName.RestoreRealResources)
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
		if (name == PropertyName._previousPacket)
		{
			_previousPacket = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._previousCharacter)
		{
			_previousCharacter = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._packetWasMissing)
		{
			_packetWasMissing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._characterWasMissing)
		{
			_characterWasMissing = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._previousPacket)
		{
			value = VariantUtils.CreateFrom(in _previousPacket);
			return true;
		}
		if (name == PropertyName._previousCharacter)
		{
			value = VariantUtils.CreateFrom(in _previousCharacter);
			return true;
		}
		if (name == PropertyName._packetWasMissing)
		{
			value = VariantUtils.CreateFrom(in _packetWasMissing);
			return true;
		}
		if (name == PropertyName._characterWasMissing)
		{
			value = VariantUtils.CreateFrom(in _characterWasMissing);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._previousPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._packetWasMissing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._characterWasMissing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._previousPacket, Variant.From(in _previousPacket));
		info.AddProperty(PropertyName._previousCharacter, Variant.From(in _previousCharacter));
		info.AddProperty(PropertyName._packetWasMissing, Variant.From(in _packetWasMissing));
		info.AddProperty(PropertyName._characterWasMissing, Variant.From(in _characterWasMissing));
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
		if (info.TryGetProperty(PropertyName._previousPacket, out var value3))
		{
			_previousPacket = value3.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._previousCharacter, out var value4))
		{
			_previousCharacter = value4.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._packetWasMissing, out var value5))
		{
			_packetWasMissing = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._characterWasMissing, out var value6))
		{
			_characterWasMissing = value6.As<bool>();
		}
	}
}
