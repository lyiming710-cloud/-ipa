using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentRobotClickWasdMoveRuntimeTest.cs")]
public class BugDepartmentRobotClickWasdMoveRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName RegisterRealFixtures = "RegisterRealFixtures";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

		public static readonly StringName RestoreRealFixtures = "RestoreRealFixtures";

		public static readonly StringName DispatchMouseClick = "DispatchMouseClick";

		public static readonly StringName DispatchTouchTap = "DispatchTouchTap";

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

	private const string RobotPacketPath = "res://Asset/Anime/Character/Plant/Star/Robot/Packet/PlantRobot.tres";

	private const string RobotScenePath = "res://Asset/Anime/Character/Plant/Star/Robot/Scene/TowerDefensePlantRobot.tscn";

	private const string MapControlScenePath = "res://Registry/Battle/Feature/Map/Control/TowerDefenseMapControl.tscn";

	private static readonly Vector2I StartGrid = new Vector2I(4, 3);

	private static readonly Vector2I ExpectedGrid = new Vector2I(5, 3);

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
		RobotClickWasdMoveControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefensePlantRobot robot = null;
		try
		{
			_ = 6;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("Required autoloads are unavailable.");
				}
				RegisterRealFixtures();
				control = new RobotClickWasdMoveControlStub
				{
					Name = "RobotClickWasdMoveControl",
					isGameRunning = true,
					isInit = false,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = new Vector2(100f, 100f);
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = ResourceLoader.Load<PackedScene>("res://Registry/Battle/Feature/Map/Control/TowerDefenseMapControl.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseMapControl>(PackedScene.GenEditState.Disabled);
				if (!GodotObject.IsInstanceValid(mapControl))
				{
					throw new InvalidOperationException("The real map control scene did not instantiate.");
				}
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				control.AddChild(mapControl, forceReadableName: false, InternalMode.Disabled);
				robot = LoadPacket("res://Asset/Anime/Character/Plant/Star/Robot/Packet/PlantRobot.tres")?.Plant(StartGrid, playAudio: false) as TowerDefensePlantRobot;
				await WaitFrames(6);
				Check(GodotObject.IsInstanceValid(robot) && robot.config?.name == "PlantRobot", "The fixture must instantiate the real Robot packet and scene.");
				if (!GodotObject.IsInstanceValid(robot))
				{
					throw new InvalidOperationException("The real Robot did not instantiate.");
				}
				TowerDefenseCellInstance startCell = TowerDefenseManager.GetMapCell(StartGrid);
				TowerDefenseCellInstance expectedCell = TowerDefenseManager.GetMapCell(ExpectedGrid);
				TowerDefenseCellInstance expectedExtendCell = TowerDefenseManager.GetMapCell(ExpectedGrid + Vector2I.Right);
				Check(GodotObject.IsInstanceValid(startCell) && startCell.characterList.Contains(robot) && robot.cell == startCell, "The real Robot must begin in its authored map cell.");
				DragMoveComponent dragMoveComponent = robot.componentManager?.GetRuntime<DragMoveComponent>();
				Check(dragMoveComponent != null && !dragMoveComponent.IsReleased && dragMoveComponent.allowMouseDrag && dragMoveComponent.allowDirectionalInput, "The real Robot must expose active drag and directional movement input.");
				Check(dragMoveComponent?.moveUpAction == (StringName)"P1Up" && dragMoveComponent.moveDownAction == (StringName)"P1Down" && dragMoveComponent.moveLeftAction == (StringName)"P1Left" && dragMoveComponent.moveRightAction == (StringName)"P1Right", "The real Robot directional controls must remain mapped to WASD actions.");
				if (dragMoveComponent == null || dragMoveComponent.IsReleased)
				{
					throw new InvalidOperationException("The real Robot DragMoveComponent is unavailable.");
				}
				Vector2 globalPosition = robot.GlobalPosition;
				InputEventMouseButton inputEvent = new InputEventMouseButton
				{
					ButtonIndex = MouseButton.Left,
					Pressed = true,
					Position = globalPosition,
					GlobalPosition = globalPosition
				};
				robot.componentManager._Input(inputEvent);
				Check(DragMoveComponent.CurrentMoveCharacter == robot && dragMoveComponent.drag, "Pressing the real Robot must select it and begin pointer dragging.");
				InputEventMouseButton inputEvent2 = new InputEventMouseButton
				{
					ButtonIndex = MouseButton.Left,
					Pressed = false,
					Position = globalPosition,
					GlobalPosition = globalPosition
				};
				robot.componentManager._Input(inputEvent2);
				Check(DragMoveComponent.CurrentMoveCharacter == robot && !dragMoveComponent.drag, "Releasing a click must end dragging but keep the real Robot selected for WASD.");
				InputEventKey inputEventKey = new InputEventKey
				{
					Keycode = Key.D,
					PhysicalKeycode = Key.D,
					Unicode = 100L,
					Pressed = true,
					Echo = false
				};
				Check(inputEventKey.IsActionPressed("P1Right"), "The synthesized physical D key must resolve through the authored P1Right input action.");
				Check(GodotObject.IsInstanceValid(expectedCell) && expectedCell.CanMoveCharacterHere(robot), "The adjacent destination cell must accept the real Robot while ignoring its old two-cell footprint.");
				robot.componentManager._Input(inputEventKey);
				await WaitFrames(40);
				Check(robot.gridPos == ExpectedGrid, $"Pressing D after clicking must move the real Robot one cell right; got {robot.gridPos}.");
				Check(GodotObject.IsInstanceValid(expectedCell) && expectedCell.characterList.Contains(robot) && robot.cell == expectedCell, "The WASD move must register the real Robot in the destination map cell.");
				Check(GodotObject.IsInstanceValid(expectedExtendCell) && expectedExtendCell.characterList.Contains(robot), "The WASD move must register the real Robot in its new authored extension cell.");
				Check(!startCell.characterList.Contains(robot), "The WASD move must remove the real Robot from its previous map cell.");
				Check(robot.GlobalPosition.DistanceTo(TowerDefenseManager.GetMapCellPlantPos(ExpectedGrid)) <= 0.01f, "The real Robot world position must snap to the destination cell after WASD movement.");
				FireComponent fire = robot.componentManager?.GetRuntime<FireComponent>("character.fire");
				FireComponent fire2 = robot.componentManager?.GetRuntime<FireComponent>("character.fire.1");
				MousePressComponent mousePressComponent = robot.componentManager?.GetRuntime<MousePressComponent>();
				if (mousePressComponent == null || mousePressComponent.IsReleased)
				{
					throw new InvalidOperationException("The real Robot MousePressComponent is unavailable.");
				}
				Check(robot.modeId == 0 && robot.CurrentStateHandle?.StableId == "character.idle" && robot.sprite?.clip == "IdleA", $"The real Robot must begin form switching from settled mode A; mode={robot.modeId}, state={robot.CurrentStateHandle?.StableId}, clip={robot.sprite?.clip}.");
				int doublePressedCount = 0;
				mousePressComponent.OnDoublePressed += (Vector2 _) =>
				{
					doublePressedCount++;
				};
				string[] expectedDownClips = new string[3] { "DownA", "DownB", "DownC" };
				string[] expectedIdleClips = new string[3] { "IdleB", "IdleC", "IdleA" };
				Vector2 globalPosition2 = robot.GlobalPosition;
				DispatchMouseClick(robot.componentManager, globalPosition2);
				DispatchTouchTap(robot.componentManager, globalPosition2, doubleTap: false);
				await WaitFrames(1);
				Check(doublePressedCount == 0 && robot.modeId == 0, "An Android compatibility mouse event followed by its matching touch must remain one tap instead of triggering a false Robot form switch.");
				await WaitFrames(20);
				for (int switchIndex = 0; switchIndex < 3; switchIndex++)
				{
					Vector2 switchPosition = robot.GlobalPosition;
					DispatchTouchTap(robot.componentManager, switchPosition, doubleTap: false);
					await WaitFrames(20);
					DispatchTouchTap(robot.componentManager, switchPosition, doubleTap: true);
					await WaitFrames(1);
					Check(robot.CurrentStateHandle?.StableId == "plant.robot.down" && robot.sprite?.clip == expectedDownClips[switchIndex], $"Robot switch {switchIndex + 1} must enter its authored down animation; state={robot.CurrentStateHandle?.StableId}, clip={robot.sprite?.clip}.");
					int expectedMode = (switchIndex + 1) % 3;
					await WaitUntil(() => robot.modeId == expectedMode && robot.CurrentStateHandle?.StableId == "character.idle" && robot.sprite?.clip == expectedIdleClips[switchIndex], 180);
					Check(robot.modeId == expectedMode && robot.CurrentStateHandle?.StableId == "character.idle" && robot.sprite?.clip == expectedIdleClips[switchIndex], $"Robot switch {switchIndex + 1} must settle in the next form; mode={robot.modeId}, state={robot.CurrentStateHandle?.StableId}, clip={robot.sprite?.clip}.");
				}
				Check(doublePressedCount == 3, $"Three real double-clicks must emit three Robot switch requests; got {doublePressedCount}.");
				Check(fire != null && fire.alive && fire2 != null && !fire2.alive, "Returning to Robot mode A must reactivate only its mode-A fire component.");
				Check(robot.instance.explosionHurt == robot.config.explosionHurt && robot.instance.smashHurt == robot.config.smashHurt && robot.instance.dragHurt == robot.config.dragHurt && robot.instance.spikeHurt == robot.config.spikeHurt && robot.instance.biteHurt == robot.config.biteHurt, "Returning from Robot mode C must restore all authored damage multipliers.");
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentRobotClickWasdMoveRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			DragMoveComponent.ClearCurrentMoveCharacter();
			if (GodotObject.IsInstanceValid(robot) && !robot.IsQueuedForDeletion())
			{
				robot.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			mapFeature?.Destroy();
			if (GodotObject.IsInstanceValid(mapControl))
			{
				mapControl.Free();
			}
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			RestoreRealFixtures();
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 26;
		GD.Print($"ROBOT_CLICK_WASD_MOVE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			gridNum = gridNum,
			gridBeginPos = new Vector2(100f, 100f),
			gridSize = new Vector2(100f, 76f),
			plantOffset = 50.0
		};
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig
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
		towerDefenseBattleFeatureMap.lineUse.Resize(gridNum.Y + 1);
		for (int k = 1; k <= gridNum.Y; k++)
		{
			towerDefenseBattleFeatureMap.lineUse[k] = true;
		}
		return towerDefenseBattleFeatureMap;
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("PlantRobot", "res://Asset/Anime/Character/Plant/Star/Robot/Packet/PlantRobot.tres");
		RegisterCharacter("PlantRobot", "res://Asset/Anime/Character/Plant/Star/Robot/Scene/TowerDefensePlantRobot.tscn");
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
		instance.TOWERDEFENSE_PACKETS[key] = ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore);
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
		instance.TOWERDEFENSE_CHARCATERS[key] = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
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

	private async Task WaitUntil(Func<bool> predicate, int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (predicate())
			{
				break;
			}
			await WaitFrames(1);
		}
	}

	private static void DispatchMouseClick(ComponentManager manager, Vector2 screenPosition)
	{
		InputEventMouseButton inputEventMouseButton = new InputEventMouseButton
		{
			ButtonIndex = MouseButton.Left,
			Pressed = true,
			Position = screenPosition,
			GlobalPosition = screenPosition
		};
		manager._Input(inputEventMouseButton);
		inputEventMouseButton.Dispose();
		InputEventMouseButton inputEventMouseButton2 = new InputEventMouseButton
		{
			ButtonIndex = MouseButton.Left,
			Pressed = false,
			Position = screenPosition,
			GlobalPosition = screenPosition
		};
		manager._Input(inputEventMouseButton2);
		inputEventMouseButton2.Dispose();
	}

	private static void DispatchTouchTap(ComponentManager manager, Vector2 screenPosition, bool doubleTap)
	{
		InputEventScreenTouch inputEventScreenTouch = new InputEventScreenTouch
		{
			Index = 0,
			Pressed = true,
			DoubleTap = doubleTap,
			Position = screenPosition
		};
		manager._Input(inputEventScreenTouch);
		inputEventScreenTouch.Dispose();
		InputEventScreenTouch inputEventScreenTouch2 = new InputEventScreenTouch
		{
			Index = 0,
			Pressed = false,
			Position = screenPosition
		};
		manager._Input(inputEventScreenTouch2);
		inputEventScreenTouch2.Dispose();
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugDepartmentRobotClickWasdMoveRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.DispatchMouseClick, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "screenPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DispatchTouchTap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "screenPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "doubleTap", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.DispatchMouseClick && args.Count == 2)
		{
			DispatchMouseClick(VariantUtils.ConvertTo<ComponentManager>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchTouchTap && args.Count == 3)
		{
			DispatchTouchTap(VariantUtils.ConvertTo<ComponentManager>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
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
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DispatchMouseClick && args.Count == 2)
		{
			DispatchMouseClick(VariantUtils.ConvertTo<ComponentManager>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchTouchTap && args.Count == 3)
		{
			DispatchTouchTap(VariantUtils.ConvertTo<ComponentManager>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
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
		if (method == MethodName.CreateMapFeature)
		{
			return true;
		}
		if (method == MethodName.LoadPacket)
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
		if (method == MethodName.DispatchMouseClick)
		{
			return true;
		}
		if (method == MethodName.DispatchTouchTap)
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
