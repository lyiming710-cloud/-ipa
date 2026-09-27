using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BungiCarryVisualInspection.cs")]
public class BungiCarryVisualInspection : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _UnhandledInput = "_UnhandledInput";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName DrawLandingMarker = "DrawLandingMarker";

		public static readonly StringName CreateBackdrop = "CreateBackdrop";

		public static readonly StringName CreateHud = "CreateHud";

		public static readonly StringName RememberGlobalState = "RememberGlobalState";

		public static readonly StringName SetupBattleFixture = "SetupBattleFixture";

		public static readonly StringName SpawnPair = "SpawnPair";

		public static readonly StringName ClearCharacters = "ClearCharacters";

		public static readonly StringName Describe = "Describe";

		public static readonly StringName IsMidRise = "IsMidRise";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName RegisterRealFixtures = "RegisterRealFixtures";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

		public static readonly StringName RestoreRealFixtures = "RestoreRealFixtures";

		public static readonly StringName Cleanup = "Cleanup";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _manager = "_manager";

		public static readonly StringName _previousControl = "_previousControl";

		public static readonly StringName _previousGridBegin = "_previousGridBegin";

		public static readonly StringName _previousGridSize = "_previousGridSize";

		public static readonly StringName _previousGridNum = "_previousGridNum";

		public static readonly StringName _previousTimeScale = "_previousTimeScale";

		public static readonly StringName _control = "_control";

		public static readonly StringName _mapFeature = "_mapFeature";

		public static readonly StringName _mapControl = "_mapControl";

		public static readonly StringName _characterNode = "_characterNode";

		public static readonly StringName _dryCarrier = "_dryCarrier";

		public static readonly StringName _waterCarrier = "_waterCarrier";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _cycleLabel = "_cycleLabel";

		public static readonly StringName _initialized = "_initialized";

		public static readonly StringName _cleanupStarted = "_cleanupStarted";

		public static readonly StringName _pausedAtRise = "_pausedAtRise";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const string CrewPacketPath = "res://Asset/Anime/Character/Zombie/Chapter7/Crew/Packet/ZombieCrew.tres";

	private const string CrewScenePath = "res://Asset/Anime/Character/Zombie/Chapter7/Crew/Scene/TowerDefenseZombieCrew.tscn";

	private const string SnorklePacketPath = "res://Asset/Anime/Character/Zombie/Chapter3/Snorkle/Packet/ZombieSnorkle.tres";

	private const string SnorkleScenePath = "res://Asset/Anime/Character/Zombie/Chapter3/Snorkle/Scene/Base/TowerDefenseZombieSnorkle.tscn";

	private static readonly Vector2I DryGrid = new Vector2I(4, 2);

	private static readonly Vector2I WaterGrid = new Vector2I(7, 3);

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private TowerDefenseManager _manager;

	private TowerDefenseControlNew _previousControl;

	private Vector2 _previousGridBegin;

	private Vector2 _previousGridSize;

	private Vector2I _previousGridNum;

	private double _previousTimeScale;

	private BungiCarryVisualInspectionControlStub _control;

	private TowerDefenseBattleFeatureMap _mapFeature;

	private TowerDefenseMapControl _mapControl;

	private Node2D _characterNode;

	private TowerDefenseZombieBungiSpawn _dryCarrier;

	private TowerDefenseZombieBungiSpawn _waterCarrier;

	private Label _statusLabel;

	private Label _cycleLabel;

	private bool _initialized;

	private bool _cleanupStarted;

	private bool _pausedAtRise;

	public override async void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		_manager = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(_manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
		{
			GD.PushError("[BungiCarryVisualInspection] Required autoloads are unavailable.");
			return;
		}
		RememberGlobalState();
		_previousTimeScale = Engine.TimeScale;
		Engine.TimeScale = 0.25;
		TowerDefenseProjectileRegistry.Init();
		CreateBackdrop();
		CreateHud();
		RegisterRealFixtures();
		SetupBattleFixture();
		await WaitPhysicsFrames(4);
		_initialized = true;
		SpawnPair();
	}

	public override void _Process(double delta)
	{
		if (_initialized)
		{
			_statusLabel.Text = $"timeScale={Engine.TimeScale:F2}\nDRY  {Describe(_dryCarrier)}\nWATER {Describe(_waterCarrier)}";
			if (!_pausedAtRise && IsMidRise(_dryCarrier) && IsMidRise(_waterCarrier))
			{
				_pausedAtRise = true;
				Engine.TimeScale = 0.0;
				_cycleLabel.Text += "  |  PAUSED MID-RISE - inspect rope/body, SPACE resumes";
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey { Pressed: not false, Echo: false, PhysicalKeycode: var physicalKeycode })
		{
			switch (physicalKeycode)
			{
			case Key.Space:
				Engine.TimeScale = ((Engine.TimeScale < 0.05) ? 0.25 : 0.0);
				GetViewport().SetInputAsHandled();
				break;
			case Key.Escape:
				GetTree().Quit();
				GetViewport().SetInputAsHandled();
				break;
			}
		}
	}

	public override void _ExitTree()
	{
		Cleanup();
	}

	public override void _Draw()
	{
		DrawRect(new Rect2(0f, 0f, 1080f, 600f), new Color(0.025f, 0.04f, 0.025f));
		DrawRect(new Rect2(0f, 95f, 1080f, 250f), new Color(0.12f, 0.26f, 0.1f));
		DrawRect(new Rect2(0f, 345f, 1080f, 255f), new Color(0.03f, 0.2f, 0.34f));
		DrawLine(new Vector2(0f, 345f), new Vector2(1080f, 345f), new Color(0.55f, 0.85f, 1f), 3f);
		DrawLandingMarker(DryGrid, new Color(1f, 0.9f, 0.25f));
		DrawLandingMarker(WaterGrid, new Color(0.25f, 0.95f, 1f));
	}

	private void DrawLandingMarker(Vector2I grid, Color color)
	{
		Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(grid);
		DrawCircle(mapCellPlantPos, 36f, new Color(color, 0.15f), filled: false, 4f);
		DrawLine(mapCellPlantPos - new Vector2(45f, 0f), mapCellPlantPos + new Vector2(45f, 0f), color, 2f);
		DrawLine(mapCellPlantPos - new Vector2(0f, 45f), mapCellPlantPos + new Vector2(0f, 45f), color, 2f);
	}

	private void CreateBackdrop()
	{
		ZIndex = -4096;
		QueueRedraw();
	}

	private void CreateHud()
	{
		CanvasLayer canvasLayer = new CanvasLayer
		{
			Layer = 1000
		};
		AddChild(canvasLayer, forceReadableName: false, InternalMode.Disabled);
		Label label = new Label
		{
			Position = new Vector2(18f, 12f),
			Text = "BUNGI CARRY VISUAL INSPECTION  |  0.25x speed"
		};
		label.AddThemeFontSizeOverride("font_size", 24);
		canvasLayer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		Label label2 = new Label
		{
			Position = new Vector2(20f, 48f),
			Text = "SPACE: pause/resume   ESC: quit   Yellow=dry, Cyan=water"
		};
		label2.AddThemeColorOverride("font_color", new Color(0.85f, 0.85f, 0.85f));
		canvasLayer.AddChild(label2, forceReadableName: false, InternalMode.Disabled);
		_cycleLabel = new Label
		{
			Position = new Vector2(20f, 76f)
		};
		_cycleLabel.AddThemeColorOverride("font_color", new Color(1f, 0.82f, 0.2f));
		canvasLayer.AddChild(_cycleLabel, forceReadableName: false, InternalMode.Disabled);
		ColorRect node = new ColorRect
		{
			Position = new Vector2(612f, 92f),
			Size = new Vector2(455f, 94f),
			Color = new Color(0f, 0f, 0f, 0.72f),
			MouseFilter = Control.MouseFilterEnum.Ignore
		};
		canvasLayer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		_statusLabel = new Label
		{
			Position = new Vector2(624f, 101f),
			Text = "waiting for real characters..."
		};
		_statusLabel.AddThemeFontSizeOverride("font_size", 16);
		canvasLayer.AddChild(_statusLabel, forceReadableName: false, InternalMode.Disabled);
	}

	private void RememberGlobalState()
	{
		_previousControl = _manager.currentControl;
		_previousGridBegin = _manager.gridBeginPos;
		_previousGridSize = _manager.gridSize;
		_previousGridNum = _manager.gridNum;
	}

	private void SetupBattleFixture()
	{
		_control = new BungiCarryVisualInspectionControlStub
		{
			Name = "BungiCarryVisualInspectionControl",
			isGameRunning = true,
			isInit = true,
			levelConfig = new TowerDefenseLevelConfig()
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_characterNode = new Node2D
		{
			Name = "CharacterNode",
			ZIndex = 0
		};
		_control.AddChild(_characterNode, forceReadableName: false, InternalMode.Disabled);
		_control.characterNode = _characterNode;
		_manager.currentControl = _control;
		_manager.gridBeginPos = new Vector2(20f, 100f);
		_manager.gridSize = new Vector2(120f, 160f);
		_manager.gridNum = new Vector2I(9, 3);
		_mapControl = new TowerDefenseMapControl
		{
			Name = "MapControl"
		};
		_mapFeature = CreateMapFeature(_mapControl, _manager.gridNum);
		_mapFeature.control = _control;
		_control.featureDictionary[new StringName("Map")] = _mapFeature;
	}

	private void SpawnPair()
	{
		if (_initialized && GodotObject.IsInstanceValid(_characterNode))
		{
			_cycleLabel.Text = "single clean run: real ZombieCrew + real ZombieSnorkle";
			_dryCarrier = _manager.BungiSpawn("ZombieCrew", DryGrid) as TowerDefenseZombieBungiSpawn;
			_waterCarrier = _manager.BungiSpawn("ZombieSnorkle", WaterGrid) as TowerDefenseZombieBungiSpawn;
			if (!GodotObject.IsInstanceValid(_dryCarrier) || !GodotObject.IsInstanceValid(_waterCarrier))
			{
				GD.PushError("[BungiCarryVisualInspection] A real bungee carrier failed to spawn.");
			}
		}
	}

	private void ClearCharacters()
	{
		foreach (Node child in _characterNode.GetChildren())
		{
			if (GodotObject.IsInstanceValid(child))
			{
				child.QueueFree();
			}
		}
		_dryCarrier = null;
		_waterCarrier = null;
	}

	private static string Describe(TowerDefenseZombieBungiSpawn carrier)
	{
		if (!GodotObject.IsInstanceValid(carrier))
		{
			return "finished";
		}
		string value = carrier.CurrentStateHandle?.StableId ?? "<none>";
		TowerDefenseCharacter character = carrier.character;
		if (character != null && GodotObject.IsInstanceValid(character))
		{
			float value2 = carrier.GetLogicalGlobalPosition().DistanceTo(character.GetLogicalGlobalPosition());
			double value3 = Math.Abs(carrier.z - character.z);
			return $"state={value,-26} posDelta={value2,7:F3}  zDelta={value3,7:F3}  carrierZ={carrier.z,7:F2} payloadZ={character.z,7:F2}";
		}
		return $"state={value}  carrierZ={carrier.z,7:F2}  payload=<creating>";
	}

	private static bool IsMidRise(TowerDefenseZombieBungiSpawn carrier)
	{
		if (GodotObject.IsInstanceValid(carrier) && carrier.CurrentStateHandle?.StableId == "zombie.bungi_spawn.rise" && carrier.z >= 220.0)
		{
			return carrier.z <= 420.0;
		}
		return false;
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			gridNum = gridNum,
			gridBeginPos = new Vector2(20f, 100f),
			gridSize = new Vector2(120f, 160f),
			plantOffset = 90.0
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
				TowerDefenseCellConfig towerDefenseCellConfig = new TowerDefenseCellConfig();
				if (j == WaterGrid.Y)
				{
					towerDefenseCellConfig.gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
					{
						TowerDefenseEnum.PLANTGRIDTYPE.WATER,
						TowerDefenseEnum.PLANTGRIDTYPE.AIR
					};
				}
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
				{
					gridPos = new Vector2I(i, j)
				};
				towerDefenseCellInstance.Init(towerDefenseCellConfig);
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

	private void RegisterRealFixtures()
	{
		RegisterPacket("ZombieCrew", "res://Asset/Anime/Character/Zombie/Chapter7/Crew/Packet/ZombieCrew.tres");
		RegisterCharacter("ZombieCrew", "res://Asset/Anime/Character/Zombie/Chapter7/Crew/Scene/TowerDefenseZombieCrew.tscn");
		RegisterPacket("ZombieSnorkle", "res://Asset/Anime/Character/Zombie/Chapter3/Snorkle/Packet/ZombieSnorkle.tres");
		RegisterCharacter("ZombieSnorkle", "res://Asset/Anime/Character/Zombie/Chapter3/Snorkle/Scene/Base/TowerDefenseZombieSnorkle.tscn");
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

	private async Task WaitPhysicsFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Cleanup()
	{
		if (!_cleanupStarted)
		{
			_cleanupStarted = true;
			_initialized = false;
			if (GodotObject.IsInstanceValid(_characterNode))
			{
				ClearCharacters();
			}
			if (GodotObject.IsInstanceValid(_manager))
			{
				_manager.currentControl = _previousControl;
				_manager.gridBeginPos = _previousGridBegin;
				_manager.gridSize = _previousGridSize;
				_manager.gridNum = _previousGridNum;
			}
			_mapFeature?.Destroy();
			RestoreRealFixtures();
			Engine.TimeScale = _previousTimeScale;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(20)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._UnhandledInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawLandingMarker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "grid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateBackdrop, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateHud, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RememberGlobalState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupBattleFixture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnPair, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearCharacters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Describe, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "carrier", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsMidRise, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "carrier", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.Cleanup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._UnhandledInput && args.Count == 1)
		{
			_UnhandledInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawLandingMarker && args.Count == 2)
		{
			DrawLandingMarker(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateBackdrop && args.Count == 0)
		{
			CreateBackdrop();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateHud && args.Count == 0)
		{
			CreateHud();
			ret = default;
			return true;
		}
		if (method == MethodName.RememberGlobalState && args.Count == 0)
		{
			RememberGlobalState();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupBattleFixture && args.Count == 0)
		{
			SetupBattleFixture();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnPair && args.Count == 0)
		{
			SpawnPair();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearCharacters && args.Count == 0)
		{
			ClearCharacters();
			ret = default;
			return true;
		}
		if (method == MethodName.Describe && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(Describe(VariantUtils.ConvertTo<TowerDefenseZombieBungiSpawn>(in args[0])));
			return true;
		}
		if (method == MethodName.IsMidRise && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMidRise(VariantUtils.ConvertTo<TowerDefenseZombieBungiSpawn>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
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
		if (method == MethodName.Cleanup && args.Count == 0)
		{
			Cleanup();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Describe && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(Describe(VariantUtils.ConvertTo<TowerDefenseZombieBungiSpawn>(in args[0])));
			return true;
		}
		if (method == MethodName.IsMidRise && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMidRise(VariantUtils.ConvertTo<TowerDefenseZombieBungiSpawn>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._UnhandledInput)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.DrawLandingMarker)
		{
			return true;
		}
		if (method == MethodName.CreateBackdrop)
		{
			return true;
		}
		if (method == MethodName.CreateHud)
		{
			return true;
		}
		if (method == MethodName.RememberGlobalState)
		{
			return true;
		}
		if (method == MethodName.SetupBattleFixture)
		{
			return true;
		}
		if (method == MethodName.SpawnPair)
		{
			return true;
		}
		if (method == MethodName.ClearCharacters)
		{
			return true;
		}
		if (method == MethodName.Describe)
		{
			return true;
		}
		if (method == MethodName.IsMidRise)
		{
			return true;
		}
		if (method == MethodName.CreateMapFeature)
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
		if (method == MethodName.Cleanup)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._manager)
		{
			_manager = VariantUtils.ConvertTo<TowerDefenseManager>(in value);
			return true;
		}
		if (name == PropertyName._previousControl)
		{
			_previousControl = VariantUtils.ConvertTo<TowerDefenseControlNew>(in value);
			return true;
		}
		if (name == PropertyName._previousGridBegin)
		{
			_previousGridBegin = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._previousGridSize)
		{
			_previousGridSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._previousGridNum)
		{
			_previousGridNum = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._previousTimeScale)
		{
			_previousTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<BungiCarryVisualInspectionControlStub>(in value);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			_mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName._mapControl)
		{
			_mapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName._characterNode)
		{
			_characterNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._dryCarrier)
		{
			_dryCarrier = VariantUtils.ConvertTo<TowerDefenseZombieBungiSpawn>(in value);
			return true;
		}
		if (name == PropertyName._waterCarrier)
		{
			_waterCarrier = VariantUtils.ConvertTo<TowerDefenseZombieBungiSpawn>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._cycleLabel)
		{
			_cycleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._initialized)
		{
			_initialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cleanupStarted)
		{
			_cleanupStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pausedAtRise)
		{
			_pausedAtRise = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._manager)
		{
			value = VariantUtils.CreateFrom(in _manager);
			return true;
		}
		if (name == PropertyName._previousControl)
		{
			value = VariantUtils.CreateFrom(in _previousControl);
			return true;
		}
		if (name == PropertyName._previousGridBegin)
		{
			value = VariantUtils.CreateFrom(in _previousGridBegin);
			return true;
		}
		if (name == PropertyName._previousGridSize)
		{
			value = VariantUtils.CreateFrom(in _previousGridSize);
			return true;
		}
		if (name == PropertyName._previousGridNum)
		{
			value = VariantUtils.CreateFrom(in _previousGridNum);
			return true;
		}
		if (name == PropertyName._previousTimeScale)
		{
			value = VariantUtils.CreateFrom(in _previousTimeScale);
			return true;
		}
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			value = VariantUtils.CreateFrom(in _mapFeature);
			return true;
		}
		if (name == PropertyName._mapControl)
		{
			value = VariantUtils.CreateFrom(in _mapControl);
			return true;
		}
		if (name == PropertyName._characterNode)
		{
			value = VariantUtils.CreateFrom(in _characterNode);
			return true;
		}
		if (name == PropertyName._dryCarrier)
		{
			value = VariantUtils.CreateFrom(in _dryCarrier);
			return true;
		}
		if (name == PropertyName._waterCarrier)
		{
			value = VariantUtils.CreateFrom(in _waterCarrier);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._cycleLabel)
		{
			value = VariantUtils.CreateFrom(in _cycleLabel);
			return true;
		}
		if (name == PropertyName._initialized)
		{
			value = VariantUtils.CreateFrom(in _initialized);
			return true;
		}
		if (name == PropertyName._cleanupStarted)
		{
			value = VariantUtils.CreateFrom(in _cleanupStarted);
			return true;
		}
		if (name == PropertyName._pausedAtRise)
		{
			value = VariantUtils.CreateFrom(in _pausedAtRise);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._manager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previousGridBegin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previousGridSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._previousGridNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._previousTimeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._dryCarrier, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._waterCarrier, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cycleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._initialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cleanupStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pausedAtRise, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._manager, Variant.From(in _manager));
		info.AddProperty(PropertyName._previousControl, Variant.From(in _previousControl));
		info.AddProperty(PropertyName._previousGridBegin, Variant.From(in _previousGridBegin));
		info.AddProperty(PropertyName._previousGridSize, Variant.From(in _previousGridSize));
		info.AddProperty(PropertyName._previousGridNum, Variant.From(in _previousGridNum));
		info.AddProperty(PropertyName._previousTimeScale, Variant.From(in _previousTimeScale));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._mapFeature, Variant.From(in _mapFeature));
		info.AddProperty(PropertyName._mapControl, Variant.From(in _mapControl));
		info.AddProperty(PropertyName._characterNode, Variant.From(in _characterNode));
		info.AddProperty(PropertyName._dryCarrier, Variant.From(in _dryCarrier));
		info.AddProperty(PropertyName._waterCarrier, Variant.From(in _waterCarrier));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._cycleLabel, Variant.From(in _cycleLabel));
		info.AddProperty(PropertyName._initialized, Variant.From(in _initialized));
		info.AddProperty(PropertyName._cleanupStarted, Variant.From(in _cleanupStarted));
		info.AddProperty(PropertyName._pausedAtRise, Variant.From(in _pausedAtRise));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._manager, out var value))
		{
			_manager = value.As<TowerDefenseManager>();
		}
		if (info.TryGetProperty(PropertyName._previousControl, out var value2))
		{
			_previousControl = value2.As<TowerDefenseControlNew>();
		}
		if (info.TryGetProperty(PropertyName._previousGridBegin, out var value3))
		{
			_previousGridBegin = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._previousGridSize, out var value4))
		{
			_previousGridSize = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._previousGridNum, out var value5))
		{
			_previousGridNum = value5.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._previousTimeScale, out var value6))
		{
			_previousTimeScale = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._control, out var value7))
		{
			_control = value7.As<BungiCarryVisualInspectionControlStub>();
		}
		if (info.TryGetProperty(PropertyName._mapFeature, out var value8))
		{
			_mapFeature = value8.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName._mapControl, out var value9))
		{
			_mapControl = value9.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName._characterNode, out var value10))
		{
			_characterNode = value10.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._dryCarrier, out var value11))
		{
			_dryCarrier = value11.As<TowerDefenseZombieBungiSpawn>();
		}
		if (info.TryGetProperty(PropertyName._waterCarrier, out var value12))
		{
			_waterCarrier = value12.As<TowerDefenseZombieBungiSpawn>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value13))
		{
			_statusLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._cycleLabel, out var value14))
		{
			_cycleLabel = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._initialized, out var value15))
		{
			_initialized = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cleanupStarted, out var value16))
		{
			_cleanupStarted = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pausedAtRise, out var value17))
		{
			_pausedAtRise = value17.As<bool>();
		}
	}
}
