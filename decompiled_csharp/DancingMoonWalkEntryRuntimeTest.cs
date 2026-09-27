using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/DancingMoonWalkEntryRuntimeTest.cs")]
public class DancingMoonWalkEntryRuntimeTest : Node
{
	private readonly record struct DancingCase(string Name, string PacketPath, string ScenePath, bool UsesDancingComponent, string ExpectedStateId);

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName RegisterFixtures = "RegisterFixtures";

		public static readonly StringName RestoreFixtures = "RestoreFixtures";

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

	private static readonly DancingCase[] Cases = new DancingCase[5]
	{
		new DancingCase("ZombieJackson", "res://Asset/Anime/Character/Zombie/Chapter2/Jackson/Packet/ZombieJackson.tres", "res://Asset/Anime/Character/Zombie/Chapter2/Jackson/Scene/TowerDefenseZombieJackson.tscn", UsesDancingComponent: true, "dancing.moon_walk"),
		new DancingCase("ZombieJacksonX", "res://Asset/Anime/Character/Zombie/Challenge/JacksonX/Packet/ZombieJacksonX.tres", "res://Asset/Anime/Character/Zombie/Challenge/JacksonX/Scene/TowerDefenseZombieJacksonX.tscn", UsesDancingComponent: true, "dancing.moon_walk"),
		new DancingCase("ZombieDiscoGargantuar", "res://Asset/Anime/Character/Zombie/Chapter6/DiscoGargantuar/Packet/ZombieDiscoGargantuar.tres", "res://Asset/Anime/Character/Zombie/Chapter6/DiscoGargantuar/Scene/TowerDefenseZombieDiscoGargantuar.tscn", UsesDancingComponent: true, "dancing.moon_walk"),
		new DancingCase("ZombieDisco", "res://Asset/Anime/Character/Zombie/Challenge/Disco/Packet/ZombieDisco.tres", "res://Asset/Anime/Character/Zombie/Challenge/Disco/Scene/TowerDefenseZombieDisco.tscn", UsesDancingComponent: false, "zombie.disco.moon_walk"),
		new DancingCase("ZombieDiscoFire", "res://Asset/Anime/Character/Zombie/Challenge/DiscoFire/Packet/ZombieDiscoFire.tres", "res://Asset/Anime/Character/Zombie/Challenge/DiscoFire/Scene/TowerDefenseZombieDiscoFire.tscn", UsesDancingComponent: false, "zombie.disco_fire.moon_walk")
	};

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
		DancingMoonWalkEntryControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		try
		{
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_00bf;
				}
				RegisterFixtures();
				control = new DancingMoonWalkEntryControlStub
				{
					Name = "DancingMoonWalkEntryControl",
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
				mapFeature = CreateMapFeature(mapControl, manager);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				DancingCase[] cases = Cases;
				foreach (DancingCase dancingCase in cases)
				{
					await VerifyEntry(dancingCase, manager, control);
				}
				goto end_IL_00b6;
				end_IL_00bf:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[DancingMoonWalkEntryRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00b6;
			}
			return;
			end_IL_00b6:;
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
			mapFeature?.Destroy();
			if (GodotObject.IsInstanceValid(mapControl))
			{
				mapControl.Free();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			RestoreFixtures();
			await WaitFrames(8);
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag = _failures == 0 && _checks == 69;
		GD.Print($"DANCING_MOONWALK_ENTRY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyEntry(DancingCase dancingCase, TowerDefenseManager manager, DancingMoonWalkEntryControlStub control)
	{
		control.isGameRunning = false;
		TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>(dancingCase.PacketPath, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
		Check(GodotObject.IsInstanceValid(towerDefensePacketConfig), dancingCase.Name + " packet must load.");
		TowerDefenseZombie zombie = towerDefensePacketConfig?.Spawn(3) as TowerDefenseZombie;
		await WaitFrames(6);
		Check(GodotObject.IsInstanceValid(zombie) && zombie.config?.name == dancingCase.Name, dancingCase.Name + " must instantiate its real scene.");
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return;
		}
		GroundMoveComponent movement = zombie.groundMoveComponent;
		DancingComponent dancing = ((!dancingCase.UsesDancingComponent) ? null : zombie.componentManager?.GetRuntime<DancingComponent>());
		DancingMoonWalkEntryRuntimeTest dancingMoonWalkEntryRuntimeTest = this;
		int condition;
		if (movement != null && !movement.IsReleased)
		{
			if (dancingCase.UsesDancingComponent)
			{
				condition = ((dancing != null && !dancing.IsReleased) ? 1 : 0);
			}
			else
			{
				condition = 1;
			}
		}
		else
		{
			condition = 0;
		}
		dancingMoonWalkEntryRuntimeTest.Check((byte)condition != 0, dancingCase.Name + " must bind its real movement/dancing runtime.");
		float x = (float)(manager.GetMapGroundRight() + (double)manager.GetMapGridSize().X);
		zombie.inGame = true;
		zombie.die = false;
		zombie.nearDie = false;
		zombie.SetLogicalGlobalPosition(new Vector2(x, zombie.GetLogicalGlobalPosition().Y));
		zombie.gridPos = new Vector2I(-1, 3);
		movement?.SetAlive(false);
		control.isGameRunning = true;
		zombie.WalkReady();
		await WaitFrames(4);
		string text = ((!dancingCase.UsesDancingComponent) ? zombie.CurrentStateHandle?.StableId : dancing?.StateMachine?.CurrentStateHandle?.StableId);
		Check(text == dancingCase.ExpectedStateId, dancingCase.Name + " must start in MoonWalk outside; state=" + text + ".");
		Check(zombie.sprite?.clip == "MoonWalk" && zombie.sprite.Scale.X < 0f, dancingCase.Name + " must show the flipped MoonWalk clip on entry.");
		Check(movement?.Alive ?? false, dancingCase.Name + " MoonWalk must keep entry movement enabled.");
		Check(zombie.IsOwnerBatchDispatchActive || zombie.IsPhysicsProcessing(), dancingCase.Name + " custom Walk entry must activate gameplay processing.");
		Node2D movementSource = new Node2D
		{
			Name = dancingCase.Name + "DeterministicGroundSlot"
		};
		AddChild(movementSource, forceReadableName: false, InternalMode.Disabled);
		movement.groundLayerName = null;
		movement.groundNode = movementSource;
		movement.ResolveMovementSource();
		movement.delay = 0f;
		zombie.sprite.pause = false;
		zombie.sprite.blend = false;
		zombie.sprite.playBack = false;
		movement.RefreshDirectionCache();
		movementSource.Position = Vector2.Zero;
		movement.BatchUpdate(0.016);
		float x2 = ((zombie.spriteGroup.Scale.X * zombie.Scale.X * zombie.sprite.Scale.X >= 0f) ? 5f : (-5f));
		float x3 = zombie.GetLogicalGlobalPosition().X;
		movementSource.Position += new Vector2(x2, 0f);
		movement.BatchUpdate(0.016);
		float x4 = zombie.GetLogicalGlobalPosition().X;
		Check(x4 < x3 - 0.5f, $"{dancingCase.Name} MoonWalk must move left into the lawn; start={x3:F3}, end={x4:F3}.");
		if (dancingCase.UsesDancingComponent)
		{
			await VerifyDancingZeroHealthDeath(dancingCase.Name, zombie, dancing, movement, manager);
		}
		control.isGameRunning = false;
		if (GodotObject.IsInstanceValid(zombie))
		{
			zombie.QueueFree();
		}
		movementSource.QueueFree();
		await WaitFrames(4);
	}

	private async Task VerifyDancingZeroHealthDeath(string caseName, TowerDefenseZombie zombie, DancingComponent dancing, GroundMoveComponent movement, TowerDefenseManager manager)
	{
		Vector2 logicalGlobalPosition = zombie.GetLogicalGlobalPosition();
		zombie.SetLogicalGlobalPosition(new Vector2((float)(manager.GetMapGroundRight() - (double)manager.GetMapGridSize().X), logicalGlobalPosition.Y));
		zombie.Component();
		Check(dancing?.SendStateEvent("ToDance") ?? false, caseName + " must enter its DancingComponent dance state.");
		Check(zombie.CurrentStateHandle?.StableId == "character.component" && dancing?.StateMachine?.CurrentStateHandle?.StableId == "dancing.dance", caseName + " must be in the component/dance state before lethal damage.");
		zombie.instance.hitpoints = 0.0;
		await WaitFrames(4);
		Check(zombie.instance.die && zombie.die, "A zero-health " + caseName + " must commit death in both runtime layers.");
		Check(zombie.CurrentStateHandle?.StableId == "zombie.die", $"A zero-health {caseName} must enter zombie.die; got {zombie.CurrentStateHandle?.StableId ?? "<null>"}.");
		Check(zombie.zombieDeathComponent?.IsDeathAnimationClip(zombie.sprite.clip) ?? false, $"A zero-health {caseName} must play a death clip; got {zombie.sprite.clip}.");
		Check(movement != null && !movement.Alive, "A zero-health " + caseName + " must stop its movement runtime.");
		dancing.moonWalkMode = true;
		dancing.savePos = zombie.GetLogicalGlobalPosition() - new Vector2(1000f, 0f);
		dancing.MoonWalkProcessing(0.016);
		Check(zombie.zombieDeathComponent.IsDeathAnimationClip(zombie.sprite.clip), $"Terminal {caseName} must keep its death clip; got {zombie.sprite.clip}.");
		Check(dancing.StateMachine?.CurrentStateHandle?.StableId != "dancing.point", "Terminal " + caseName + " must reject late dance-state transitions.");
		for (int frame = 0; frame < 240; frame++)
		{
			if (!GodotObject.IsInstanceValid(zombie))
			{
				break;
			}
			await WaitFrames(1);
		}
		Check(!GodotObject.IsInstanceValid(zombie), "A zero-health " + caseName + " must finish its death lifecycle and be freed.");
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, TowerDefenseManager manager)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = new TowerDefenseMapConfig
			{
				gridNum = manager.gridNum,
				gridSize = manager.gridSize,
				gridBeginPos = manager.gridBeginPos
			}
		});
		towerDefenseBattleFeatureMap.plantGrid.Resize(manager.gridNum.X + 1);
		for (int i = 0; i <= manager.gridNum.X; i++)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			array.Resize(manager.gridNum.Y + 1);
			for (int j = 1; j <= manager.gridNum.Y; j++)
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
		towerDefenseBattleFeatureMap.iceCapList.Resize(manager.gridNum.Y + 1);
		return towerDefenseBattleFeatureMap;
	}

	private void RegisterFixtures()
	{
		DancingCase[] cases = Cases;
		for (int i = 0; i < cases.Length; i++)
		{
			DancingCase dancingCase = cases[i];
			RegisterResource(ResourceManager.Instance.TOWERDEFENSE_PACKETS, _previousPackets, _missingPackets, dancingCase.Name, dancingCase.PacketPath, packedScene: false);
			RegisterResource(ResourceManager.Instance.TOWERDEFENSE_CHARCATERS, _previousCharacters, _missingCharacters, dancingCase.Name, dancingCase.ScenePath, packedScene: true);
		}
	}

	private static void RegisterResource(System.Collections.Generic.Dictionary<string, Resource> registry, System.Collections.Generic.Dictionary<string, Resource> previous, HashSet<string> missing, string key, string path, bool packedScene)
	{
		if (registry.TryGetValue(key, out var value))
		{
			previous[key] = value;
		}
		else
		{
			missing.Add(key);
		}
		registry[key] = (packedScene ? ((Resource)ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore)) : ((Resource)ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)));
	}

	private void RestoreFixtures()
	{
		if (GodotObject.IsInstanceValid(ResourceManager.Instance))
		{
			RestoreRegistry(ResourceManager.Instance.TOWERDEFENSE_PACKETS, _previousPackets, _missingPackets);
			RestoreRegistry(ResourceManager.Instance.TOWERDEFENSE_CHARCATERS, _previousCharacters, _missingCharacters);
		}
	}

	private static void RestoreRegistry(System.Collections.Generic.Dictionary<string, Resource> registry, System.Collections.Generic.Dictionary<string, Resource> previous, HashSet<string> missing)
	{
		foreach (string item in missing)
		{
			registry.Remove(item);
		}
		foreach (KeyValuePair<string, Resource> previou in previous)
		{
			registry[previou.Key] = previou.Value;
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
			GD.PushError("[DancingMoonWalkEntryRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<TowerDefenseManager>(in args[1])));
			return true;
		}
		if (method == MethodName.RegisterFixtures && args.Count == 0)
		{
			RegisterFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreFixtures && args.Count == 0)
		{
			RestoreFixtures();
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
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<TowerDefenseManager>(in args[1])));
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
		if (method == MethodName.RegisterFixtures)
		{
			return true;
		}
		if (method == MethodName.RestoreFixtures)
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
