using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentHypnotizedJacksonDancerSpawnRuntimeTest.cs")]
public class BugDepartmentHypnotizedJacksonDancerSpawnRuntimeTest : Node
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

		public static readonly StringName ColorApproximately = "ColorApproximately";

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

	private const string JacksonPacketPath = "res://Asset/Anime/Character/Zombie/Chapter2/Jackson/Packet/ZombieJackson.tres";

	private const string JacksonScenePath = "res://Asset/Anime/Character/Zombie/Chapter2/Jackson/Scene/TowerDefenseZombieJackson.tscn";

	private const string DancerPacketPath = "res://Asset/Anime/Character/Zombie/Chapter2/Dancer/Packet/ZombieDancer.tres";

	private const string DancerScenePath = "res://Asset/Anime/Character/Zombie/Chapter2/Dancer/Scene/TowerDefenseZombieDancer.tscn";

	private static readonly Vector2I JacksonGrid = new Vector2I(5, 3);

	private int _checks;

	private int _failures;

	private readonly Dictionary<string, Resource> _previousPackets = new Dictionary<string, Resource>();

	private readonly Dictionary<string, Resource> _previousCharacters = new Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private readonly List<Resource> _registeredResources = new List<Resource>();

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		HypnotizedJacksonDancerSpawnRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefenseZombieJackson jackson = null;
		List<TowerDefenseZombie> spawned = new List<TowerDefenseZombie>();
		TowerDefensePacketConfig jacksonPacket = null;
		TowerDefensePacketConfig dancerPacket = null;
		PackedScene jacksonScene = null;
		PackedScene dancerScene = null;
		try
		{
			_ = 7;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_0134;
				}
				GameSaveManager.Instance.SetConfigValue("Backgrounder", true);
				await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
				RegisterRealFixtures();
				jacksonPacket = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter2/Jackson/Packet/ZombieJackson.tres");
				dancerPacket = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter2/Dancer/Packet/ZombieDancer.tres");
				jacksonScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter2/Jackson/Scene/TowerDefenseZombieJackson.tscn", null, ResourceLoader.CacheMode.Ignore);
				dancerScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter2/Dancer/Scene/TowerDefenseZombieDancer.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(jacksonPacket) && GodotObject.IsInstanceValid(dancerPacket) && GodotObject.IsInstanceValid(jacksonScene) && GodotObject.IsInstanceValid(dancerScene), "The regression must load the real Jackson and backup-dancer packets and scenes.");
				if (!GodotObject.IsInstanceValid(jacksonPacket) || !GodotObject.IsInstanceValid(dancerPacket) || !GodotObject.IsInstanceValid(jacksonScene) || !GodotObject.IsInstanceValid(dancerScene))
				{
					goto end_IL_0134;
				}
				control = new HypnotizedJacksonDancerSpawnRuntimeControlStub
				{
					Name = "HypnotizedJacksonDancerSpawnRuntimeControl",
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
				manager.gridBeginPos = new Vector2(256f, 45f);
				manager.gridSize = new Vector2(80f, 98f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(JacksonGrid);
				jackson = jacksonPacket.Create(mapCellPlantPos, JacksonGrid) as TowerDefenseZombieJackson;
				Check(GodotObject.IsInstanceValid(jackson), "The real Jackson packet must create the real dance leader.");
				if (!GodotObject.IsInstanceValid(jackson))
				{
					goto end_IL_0134;
				}
				control.characterNode.AddChild(jackson, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				BugDepartmentHypnotizedJacksonDancerSpawnRuntimeTest bugDepartmentHypnotizedJacksonDancerSpawnRuntimeTest = this;
				int condition;
				if (jackson.IsNodeReady())
				{
					DancingComponent dancingComponent = jackson.dancingComponent;
					condition = ((dancingComponent != null && dancingComponent.Lifecycle == ComponentRuntimeLifecycle.Active) ? 1 : 0);
				}
				else
				{
					condition = 0;
				}
				bugDepartmentHypnotizedJacksonDancerSpawnRuntimeTest.Check((byte)condition != 0, "The real Jackson DancingComponent must be active before summoning.");
				Vector2 jacksonInsidePosition = jackson.GlobalPosition;
				jackson.SetLogicalGlobalPosition(new Vector2((float)manager.GetMapGroundRight() + 100f, jackson.GlobalPosition.Y));
				jackson.gridPos = new Vector2I(12, JacksonGrid.Y);
				jackson.sprite.runtimeViewportCullingEnabled = false;
				jackson.Walk();
				await WaitFrames(4);
				Check(!jackson.dancingComponent.moonWalkMode && (jackson.groundMoveComponent?.Alive ?? false), "An off-field Jackson must use normal entry movement instead of reversed moonwalk root motion.");
				jackson.dancingComponent.SpawnDancer();
				Check(!jackson.dancingComponent.CanSpawnDancer() && jackson.dancingComponent.dancerList.TrueForAll((TowerDefenseCharacter dancer) => !GodotObject.IsInstanceValid(dancer)), "An off-field Jackson must not spawn backup dancers.");
				StateMachineSnapshot stateMachineSnapshot = jackson.dancingComponent.StateMachine.CaptureSnapshot();
				stateMachineSnapshot.ActiveStateIds.Clear();
				stateMachineSnapshot.ActiveStateIds.Add("dancing.moon_walk");
				jackson.dancingComponent.moonWalkMode = false;
				jackson.dancingComponent.moonWalkOver = false;
				jackson.SetLogicalGlobalPosition(new Vector2((float)manager.GetMapGroundRight() + manager.gridSize.X * 4f, jackson.GlobalPosition.Y));
				jackson.gridPos = new Vector2I(14, JacksonGrid.Y);
				bool restoredRunaway = jackson.dancingComponent.RestoreStateMachineSnapshot(stateMachineSnapshot, remote: false, suppressEntryEffects: true, progressCompatible: true);
				await WaitFrames(4);
				Check(restoredRunaway && (double)jackson.GlobalPosition.X <= manager.GetMapGroundRight() + (double)manager.gridSize.X && jackson.gridPos.X == manager.gridNum.X + 1 && !jackson.dancingComponent.moonWalkMode, "A legacy far-right moonwalk snapshot must be repaired to the entry boundary and entry movement mode.");
				jackson.SetLogicalGlobalPosition(jacksonInsidePosition);
				jackson.gridPos = JacksonGrid;
				jackson.Hypnoses();
				await WaitFrames(2);
				Check((jackson.instance?.hypnoses ?? false) && jackson.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT, "The dance leader must already be hypnotized into the Plant camp before summoning.");
				jackson.SetLogicalGlobalPosition(new Vector2((float)manager.GetMapGroundRight() + 100f, jackson.GlobalPosition.Y));
				await WaitFrames(2);
				Color expected = new Color(0.83f, 0.34f, 0.81f);
				Check(ColorApproximately(jackson.sprite.meshColor, expected), "An off-field hypnotized character must keep its Buff tint after the per-frame meshColor reset.");
				AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult = jackson.sprite.TryBuildCrowdRenderState(out var state);
				Color actual = ((state.Mode == AdobeAnimateCrowdRenderMode.GpuGraph) ? state.GpuGraphRootOwnerState.Modulate : state.Modulate);
				Check(adobeAnimateCrowdRenderStateResult == AdobeAnimateCrowdRenderStateResult.Submitted && ColorApproximately(actual, expected), "GPU Crowd state must preserve the hypnotized character tint.");
				jackson.SetLogicalGlobalPosition(jacksonInsidePosition);
				jackson.gridPos = JacksonGrid;
				jackson.dancingComponent.SpawnDancer();
				foreach (TowerDefenseCharacter dancer in jackson.dancingComponent.dancerList)
				{
					if (dancer is TowerDefenseZombie item)
					{
						spawned.Add(item);
					}
				}
				Check(spawned.Count == 4, $"A central Jackson must create all four real backup dancers; got {spawned.Count}.");
				HashSet<ulong> hashSet = new HashSet<ulong>();
				foreach (TowerDefenseZombie item2 in spawned)
				{
					hashSet.Add(item2.GetInstanceId());
				}
				Check(hashSet.Count == 4, "Every dancer slot must own a distinct real backup dancer.");
				await WaitFrames(2);
				bool flag = true;
				bool flag2 = true;
				bool flag3 = true;
				bool flag4 = true;
				HashSet<Vector2I> hashSet2 = new HashSet<Vector2I>
				{
					JacksonGrid - new Vector2I(0, 1),
					JacksonGrid + new Vector2I(0, 1),
					JacksonGrid - new Vector2I(1, 0),
					JacksonGrid + new Vector2I(1, 0)
				};
				foreach (TowerDefenseZombie item3 in spawned)
				{
					flag &= GodotObject.IsInstanceValid(item3) && item3.IsNodeReady() && item3 is TowerDefenseZombieDancer;
					flag2 &= hashSet2.Remove(item3.gridPos);
					flag3 &= (item3.instance?.hypnoses ?? false) && item3.camp == jackson.camp;
					bool flag5 = flag4;
					AttackComponent attackComponent = item3.attackComponent;
					flag4 = flag5 & (attackComponent != null && attackComponent.Lifecycle == ComponentRuntimeLifecycle.Active);
				}
				Check(flag, "All four real backup dancers must enter the tree and finish _Ready.");
				Check(flag2 && hashSet2.Count == 0, "The real summoning path must fill the upper, lower, left, and right dancer slots; actual=" + string.Join(", ", spawned.ConvertAll((TowerDefenseZombie dancer) => dancer.gridPos.ToString())) + ".");
				Check(flag3, "Every real backup dancer must inherit the already-hypnotized leader's camp.");
				Check(flag4, "Every real backup dancer must run its production AttackComponent during the observation.");
				bool everyObservedFrameSameCamp = true;
				bool neverTargetedSameCamp = true;
				bool neverTargetedLeader = true;
				for (int sample = 0; sample < 120; sample++)
				{
					await WaitFrames(1);
					foreach (TowerDefenseZombie item4 in spawned)
					{
						everyObservedFrameSameCamp &= GodotObject.IsInstanceValid(item4) && (item4.instance?.hypnoses ?? false) && item4.camp == jackson.camp;
						TowerDefenseCharacter towerDefenseCharacter = item4.attackComponent?.target;
						if (GodotObject.IsInstanceValid(towerDefenseCharacter))
						{
							neverTargetedSameCamp &= towerDefenseCharacter.camp != item4.camp;
							neverTargetedLeader &= towerDefenseCharacter != jackson;
						}
					}
				}
				Check(everyObservedFrameSameCamp, "All four dancers must remain in the hypnotized leader's camp from their first observable frame.");
				Check(neverTargetedSameCamp & neverTargetedLeader, "No dancer AttackComponent may briefly acquire its hypnotized leader or another same-camp unit.");
				Check((jackson.instance?.hypnoses ?? false) && jackson.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT, "The leader must remain hypnotized throughout the complete summon and rise interval.");
				goto end_IL_0109;
				end_IL_0134:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentHypnotizedJacksonDancerSpawnRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0109;
			}
			return;
			end_IL_0109:;
		}
		finally
		{
			foreach (TowerDefenseZombie item5 in spawned)
			{
				if (GodotObject.IsInstanceValid(item5) && !item5.IsQueuedForDeletion())
				{
					item5.QueueFree();
				}
			}
			if (GodotObject.IsInstanceValid(jackson) && !jackson.IsQueuedForDeletion())
			{
				jackson.QueueFree();
			}
			await WaitFrames(6);
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
			await WaitFrames(8);
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			await WaitFrames(2);
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			jacksonPacket?.Dispose();
			dancerPacket?.Dispose();
			jacksonScene?.Dispose();
			dancerScene?.Dispose();
			foreach (Resource registeredResource in _registeredResources)
			{
				registeredResource?.Dispose();
			}
			_registeredResources.Clear();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag6 = _failures == 0 && _checks == 20;
		GD.Print($"HYPNOTIZED_JACKSON_DANCER_SPAWN_RESULT version=5 passed={flag6} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag6) ? 2 : 0);
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = new TowerDefenseMapConfig
			{
				gridNum = gridNum,
				gridBeginPos = new Vector2(256f, 45f),
				gridSize = new Vector2(80f, 98f)
			}
		});
		towerDefenseBattleFeatureMap.mapConfig = towerDefenseBattleFeatureMap.config;
		towerDefenseBattleFeatureMap.groundRect = new Rect2(new Vector2(256f, 45f), new Vector2(80 * gridNum.X, 98 * gridNum.Y));
		towerDefenseBattleFeatureMap.rect = new Rect2(-1000f, -1000f, 4000f, 4000f);
		towerDefenseBattleFeatureMap.config.cellConfig.Add(new TowerDefenseCellConfig
		{
			pos = new Vector4I(1, 1, gridNum.X, gridNum.Y)
		});
		for (int i = 1; i <= gridNum.Y; i++)
		{
			towerDefenseBattleFeatureMap.config.lineUse.Add(i);
		}
		towerDefenseBattleFeatureMap.PlantGridInit();
		return towerDefenseBattleFeatureMap;
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("ZombieJackson", "res://Asset/Anime/Character/Zombie/Chapter2/Jackson/Packet/ZombieJackson.tres");
		RegisterPacket("ZombieDancer", "res://Asset/Anime/Character/Zombie/Chapter2/Dancer/Packet/ZombieDancer.tres");
		RegisterCharacter("ZombieJackson", "res://Asset/Anime/Character/Zombie/Chapter2/Jackson/Scene/TowerDefenseZombieJackson.tscn");
		RegisterCharacter("ZombieDancer", "res://Asset/Anime/Character/Zombie/Chapter2/Dancer/Scene/TowerDefenseZombieDancer.tscn");
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
		TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore);
		_registeredResources.Add(towerDefensePacketConfig);
		instance.TOWERDEFENSE_PACKETS[key] = towerDefensePacketConfig;
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
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		_registeredResources.Add(packedScene);
		instance.TOWERDEFENSE_CHARCATERS[key] = packedScene;
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

	private static bool ColorApproximately(Color actual, Color expected)
	{
		if (Mathf.Abs(actual.R - expected.R) <= 0.001f && Mathf.Abs(actual.G - expected.G) <= 0.001f && Mathf.Abs(actual.B - expected.B) <= 0.001f)
		{
			return Mathf.Abs(actual.A - expected.A) <= 0.001f;
		}
		return false;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugDepartmentHypnotizedJacksonDancerSpawnRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
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
			new MethodInfo(MethodName.ColorApproximately, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "actual", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ColorApproximately && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ColorApproximately(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
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
		if (method == MethodName.ColorApproximately && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ColorApproximately(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
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
		if (method == MethodName.ColorApproximately)
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
