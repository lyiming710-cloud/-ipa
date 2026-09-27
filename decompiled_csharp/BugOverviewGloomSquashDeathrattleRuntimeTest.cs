using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewGloomSquashDeathrattleRuntimeTest.cs")]
public class BugOverviewGloomSquashDeathrattleRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateNightMapFeature = "CreateNightMapFeature";

		public static readonly StringName AddToCell = "AddToCell";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName LoadScene = "LoadScene";

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

	private const string GloomPacketPath = "res://Asset/Anime/Character/Plant/Chapter6/GloomSquash/Packet/PlantGloomSquash.tres";

	private const string GloomScenePath = "res://Asset/Anime/Character/Plant/Chapter6/GloomSquash/Scene/TowerDefensePlantGloomSquash.tscn";

	private const string GloomSquashDefinitionPath = "res://Script/Component/TowerDefense/Character/SquashComponent/Definitions/GloomSquashDefinition.tres";

	private const string ZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private static readonly Vector2I TestGrid = new Vector2I(4, 3);

	private readonly Dictionary<string, Resource> _previousPackets = new Dictionary<string, Resource>();

	private readonly Dictionary<string, Resource> _previousCharacters = new Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BugOverviewGloomSquashDeathrattleRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance), "TowerDefenseManager and ResourceManager autoloads must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_0137;
				}
				TowerDefensePacketConfig towerDefensePacketConfig = LoadPacket("res://Asset/Anime/Character/Plant/Chapter6/GloomSquash/Packet/PlantGloomSquash.tres");
				PackedScene packedScene = LoadScene("res://Asset/Anime/Character/Plant/Chapter6/GloomSquash/Scene/TowerDefensePlantGloomSquash.tscn");
				SquashComponentDefinition squashComponentDefinition = ResourceLoader.Load<SquashComponentDefinition>("res://Script/Component/TowerDefense/Character/SquashComponent/Definitions/GloomSquashDefinition.tres", null, ResourceLoader.CacheMode.IgnoreDeep);
				Check(GodotObject.IsInstanceValid(towerDefensePacketConfig) && GodotObject.IsInstanceValid(packedScene) && packedScene.CanInstantiate() && GodotObject.IsInstanceValid(squashComponentDefinition), "The scenario must load the authored GloomSquash packet, scene, and Squash definition.");
				Check(towerDefensePacketConfig?.saveKey == "PlantGloomSquash" && towerDefensePacketConfig.characterConfig?.name == "PlantGloomSquash" && squashComponentDefinition != null && !squashComponentDefinition.InitiallyAlive, "The authored GloomSquash must keep a dormant Squash definition until zero hitpoints.");
				TowerDefensePacketConfig zombiePacket = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres");
				PackedScene packedScene2 = LoadScene("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
				Check(GodotObject.IsInstanceValid(zombiePacket) && zombiePacket.saveKey == "ZombieNormal" && GodotObject.IsInstanceValid(packedScene2) && packedScene2.CanInstantiate(), "The target fixture must load the authored ZombieNormal packet and scene.");
				if (!GodotObject.IsInstanceValid(towerDefensePacketConfig) || !GodotObject.IsInstanceValid(packedScene) || !GodotObject.IsInstanceValid(squashComponentDefinition) || !GodotObject.IsInstanceValid(zombiePacket) || !GodotObject.IsInstanceValid(packedScene2))
				{
					goto end_IL_0137;
				}
				RegisterRealFixtures();
				control = new BugOverviewGloomSquashDeathrattleRuntimeControlStub
				{
					Name = "GloomSquashDeathrattleRuntimeControl",
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
					Name = "NightMapControl"
				};
				mapFeature = CreateNightMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				TowerDefenseCellInstance testCell = TowerDefenseManager.GetMapCell(TestGrid);
				Check(TowerDefenseManager.GetMapIsNight() && GodotObject.IsInstanceValid(testCell) && testCell.gridPos == TestGrid, "The scenario must use a real cell from an active night map feature.");
				TowerDefensePlantGloomSquash gloom = towerDefensePacketConfig.Plant(TestGrid, playAudio: false) as TowerDefensePlantGloomSquash;
				await WaitFrames(10);
				TowerDefenseZombieNormal zombie = zombiePacket.Plant(TestGrid, playAudio: false) as TowerDefenseZombieNormal;
				await WaitFrames(8);
				Check(GodotObject.IsInstanceValid(gloom) && gloom.config?.name == "PlantGloomSquash" && gloom.SceneFilePath == "res://Asset/Anime/Character/Plant/Chapter6/GloomSquash/Scene/TowerDefensePlantGloomSquash.tscn", "The test object must be the authored TowerDefensePlantGloomSquash scene.");
				Check(GodotObject.IsInstanceValid(zombie) && zombie.config?.name == "ZombieNormal" && zombie.SceneFilePath == "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", "The target must be the authored TowerDefenseZombieNormal scene.");
				if (!GodotObject.IsInstanceValid(gloom) || !GodotObject.IsInstanceValid(zombie))
				{
					goto end_IL_0137;
				}
				gloom.ProcessMode = ProcessModeEnum.Disabled;
				zombie.ProcessMode = ProcessModeEnum.Disabled;
				gloom.inGame = true;
				zombie.inGame = true;
				gloom.gridPos = TestGrid;
				zombie.gridPos = TestGrid;
				gloom.cell = testCell;
				zombie.cell = testCell;
				zombie.GlobalPosition = gloom.GlobalPosition;
				AddToCell(gloom, testCell);
				AddToCell(zombie, testCell);
				Check(testCell.characterList.Contains(gloom) && testCell.characterList.Contains(zombie), "The real GloomSquash and ZombieNormal must share the same production map cell.");
				SquashComponent runtime = gloom.componentManager.GetRuntime<SquashComponent>("character.squash");
				AttackComponent runtime2 = gloom.componentManager.GetRuntime<AttackComponent>("character.attack.0");
				AttackComponent runtime3 = gloom.componentManager.GetRuntime<AttackComponent>("character.attack.1");
				Check(runtime != null && !runtime.IsReleased && runtime2 != null && !runtime2.IsReleased && runtime3 != null && !runtime3.IsReleased, "The real scene must expose its Squash and both authored Attack runtimes.");
				if (runtime == null || runtime.IsReleased || runtime2 == null || runtime2.IsReleased || runtime3 == null || runtime3.IsReleased)
				{
					goto end_IL_0137;
				}
				Check(!runtime.Alive && !runtime.IsRunning(), "The real Squash component must begin dormant.");
				runtime.IdleProcessing(0.0);
				Check(!runtime.IsRunning() && runtime.target == null, "A co-located real zombie must not trigger the dormant Squash component.");
				double hitpoints = gloom.instance.hitpoints;
				gloom.Hurt(10.0, playSplatAudio: false, Vector2.Zero, createDamagePart: false);
				Check(!gloom.instance.invincible && Mathf.IsEqualApprox(gloom.instance.hitpoints, hitpoints - 10.0), "A real nonlethal Hurt must pass through the production damage pipeline.");
				Check(gloom.instance.hitpoints > 0.0, "The nonlethal damage fixture must leave positive hitpoints.");
				Check(!gloom.instance.nearDie && !gloom.nearDie, "Nonlethal damage must not set either near-death layer.");
				Check(!runtime.Alive && !runtime.IsRunning(), "Nonlethal real damage must keep the Squash component dormant.");
				gloom.instance.nearDie = true;
				gloom.nearDie = true;
				gloom.BatchUpdate(0.0);
				Check(gloom.instance.hitpoints > 0.0 && !runtime.Alive && !runtime.IsRunning(), "Near-death flags alone at positive hitpoints must not arm the Squash component.");
				gloom.instance.nearDie = false;
				gloom.nearDie = false;
				gloom.SkipInvincibleHurt(gloom.instance.hitpoints + 1.0, playSplatAudio: false, Vector2.Zero, createDamagePart: false);
				Check(gloom.instance.hitpoints <= 0.0, "A lethal SkipInvincibleHurt must drive the real plant to zero hitpoints.");
				Check(gloom.instance.nearDie && gloom.nearDie, "The production lethal-damage path must set both near-death layers.");
				Check(!runtime.Alive && !runtime.IsRunning(), "The Squash component must remain dormant until GloomSquash.BatchUpdate observes zero hitpoints.");
				gloom.BatchUpdate(0.0);
				Check(runtime.Alive && !runtime2.Alive && runtime3.Alive, "Zero hitpoints must arm the real Squash component, stop Attack0, and retain Attack1 targeting.");
				Check(Mathf.IsEqualApprox(gloom.instance.hitpoints, 300.0) && !gloom.instance.nearDie && !gloom.nearDie && !gloom.instance.die && !gloom.die, "Deathrattle activation must restore 300 hitpoints and clear both instance and character death state.");
				control.isGameRunning = true;
				gloom.componentAlive = true;
				gloom.componentRunning = false;
				runtime3.target = null;
				runtime3.checkIntrevalNow = 0;
				runtime.IdleProcessing(0.0);
				Check(runtime.IsRunning() && runtime.target == zombie, "The armed production Squash component must run against the real co-located ZombieNormal.");
				goto end_IL_0122;
				end_IL_0137:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[GloomSquashDeathrattle] Unexpected exception: {value}");
				goto end_IL_0122;
			}
			return;
			end_IL_0122:;
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
			RestoreRealFixtures();
			await WaitFrames(8);
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			await WaitFrames(2);
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag = _failures == 0 && _checks == 22;
		GD.Print($"GLOOM_SQUASH_DEATHRATTLE_ACTIVATION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseBattleFeatureMap CreateNightMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			isNight = true,
			gridNum = gridNum,
			gridBeginPos = Vector2.Zero,
			gridSize = new Vector2(100f, 76f),
			plantOffset = 50.0
		};
		towerDefenseMapConfig.cellConfig.Add(new TowerDefenseCellConfig
		{
			pos = new Vector4I(1, 1, gridNum.X, gridNum.Y)
		});
		for (int i = 1; i <= gridNum.Y; i++)
		{
			towerDefenseMapConfig.lineUse.Add(i);
		}
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig
		});
		towerDefenseBattleFeatureMap.PlantGridInit();
		return towerDefenseBattleFeatureMap;
	}

	private static void AddToCell(TowerDefenseCharacter character, TowerDefenseCellInstance cell)
	{
		if (GodotObject.IsInstanceValid(cell) && !cell.characterList.Contains(character))
		{
			cell.characterList.Add(character);
		}
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.IgnoreDeep)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private static PackedScene LoadScene(string path)
	{
		return ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.IgnoreDeep);
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("PlantGloomSquash", "res://Asset/Anime/Character/Plant/Chapter6/GloomSquash/Packet/PlantGloomSquash.tres");
		RegisterPacket("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres");
		RegisterCharacter("PlantGloomSquash", "res://Asset/Anime/Character/Plant/Chapter6/GloomSquash/Scene/TowerDefensePlantGloomSquash.tscn");
		RegisterCharacter("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
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
		instance.TOWERDEFENSE_PACKETS[key] = ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.IgnoreDeep);
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
		instance.TOWERDEFENSE_CHARCATERS[key] = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.IgnoreDeep);
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
			GD.PushError("[GloomSquashDeathrattle] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateNightMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddToCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName.CreateNightMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateNightMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.AddToCell && args.Count == 2)
		{
			AddToCell(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(LoadScene(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.CreateNightMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateNightMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.AddToCell && args.Count == 2)
		{
			AddToCell(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(LoadScene(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.CreateNightMapFeature)
		{
			return true;
		}
		if (method == MethodName.AddToCell)
		{
			return true;
		}
		if (method == MethodName.LoadPacket)
		{
			return true;
		}
		if (method == MethodName.LoadScene)
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
