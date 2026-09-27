using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewCatGatlingRepeatedVolleyRuntimeTest.cs")]
public class BugOverviewCatGatlingRepeatedVolleyRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName OnHeadAnimeEvent = "OnHeadAnimeEvent";

		public static readonly StringName OnNormalVolley = "OnNormalVolley";

		public static readonly StringName OnBulletSpawned = "OnBulletSpawned";

		public static readonly StringName OnFireOver = "OnFireOver";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName RegisterRealFixtures = "RegisterRealFixtures";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

		public static readonly StringName RegisterProjectile = "RegisterProjectile";

		public static readonly StringName RestoreRealFixtures = "RestoreRealFixtures";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _spawnedProjectiles = "_spawnedProjectiles";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _roundsCompleted = "_roundsCompleted";

		public static readonly StringName _totalFireEvents = "_totalFireEvents";

		public static readonly StringName _totalNormalShots = "_totalNormalShots";

		public static readonly StringName _previousRoundFireEvents = "_previousRoundFireEvents";

		public static readonly StringName _previousRoundNormalShots = "_previousRoundNormalShots";

		public static readonly StringName _plant = "_plant";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string PlantScenePath = "res://Asset/Anime/Character/Plant/Gold/CatGatlingPea/Scene/TowerDefensePlantCatGatlingPea.tscn";

	private const string PlantPacketPath = "res://Asset/Anime/Character/Plant/Gold/CatGatlingPea/Packet/PlantCatGatlingPea.tres";

	private const string TargetScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string TargetPacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres";

	private const string StarConfigPath = "res://Asset/Config/Projectile/Star/StarDefault.tres";

	private const int ExpectedRounds = 3;

	private const int ExpectedNormalShotsPerRound = 4;

	private const int ExpectedScatterShotsPerRound = 5;

	private int _spawnedProjectiles;

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousProjectiles = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private readonly HashSet<string> _missingProjectiles = new HashSet<string>();

	private readonly List<int> _fireEventsPerRound = new List<int>();

	private readonly List<int> _normalShotsPerRound = new List<int>();

	private int _checks;

	private int _failures;

	private int _roundsCompleted;

	private int _totalFireEvents;

	private int _totalNormalShots;

	private int _previousRoundFireEvents;

	private int _previousRoundNormalShots;

	private TowerDefensePlantCatGatlingPea _plant;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		ProcessModeEnum previousProjectileProcessMode = ProjectileUpdateManager.Instance?.ProcessMode ?? ProcessModeEnum.Inherit;
		BugOverviewCatGatlingRepeatedVolleyControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		BulletField bulletField = null;
		try
		{
			_ = 4;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00fe;
				}
				ProjectileUpdateManager.Instance.ProcessMode = ProcessModeEnum.Disabled;
				bulletField = BulletField.EnsureMountedOnCharacterNode();
				Check(GodotObject.IsInstanceValid(bulletField), "The BulletField must mount for the repeated-volley probe.");
				if (!GodotObject.IsInstanceValid(bulletField))
				{
					goto end_IL_00fe;
				}
				bulletField.ClearActiveBullets();
				bulletField.OnBulletSpawned += OnBulletSpawned;
				TowerDefenseProjectileRegistry.Init();
				RegisterRealFixtures();
				control = new BugOverviewCatGatlingRepeatedVolleyControlStub
				{
					Name = "CatGatlingRepeatedVolleyControl",
					isGameRunning = false,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				Node2D node2D = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = node2D;
				manager.currentControl = control;
				manager.gridBeginPos = new Vector2(0f, 100f);
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				_plant = LoadPacket("res://Asset/Anime/Character/Plant/Gold/CatGatlingPea/Packet/PlantCatGatlingPea.tres")?.Plant(new Vector2I(2, 3), playAudio: false) as TowerDefensePlantCatGatlingPea;
				await WaitPhysicsFrames(4);
				TowerDefenseCharacter target = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(new Vector2I(7, 3), playAudio: false);
				await WaitPhysicsFrames(5);
				Check(GodotObject.IsInstanceValid(_plant) && _plant.config?.name == "PlantCatGatlingPea", "The reported real Cat Star Gatling scene must instantiate.");
				Check(GodotObject.IsInstanceValid(target) && target.config?.name == "ZombieNormal", "The fixture must instantiate a real normal-zombie target.");
				if (!GodotObject.IsInstanceValid(_plant) || !GodotObject.IsInstanceValid(target))
				{
					goto end_IL_00fe;
				}
				_plant.timeScale = 4.0;
				target.instance.hitpoints = 1000000.0;
				target.instance.hitpointsBase = 1000000.0;
				target.ProcessMode = ProcessModeEnum.Disabled;
				FireComponent fire = _plant.componentManager?.GetRuntime<FireComponent>("character.fire");
				AdobeAnimateSprite adobeAnimateSprite = (_plant.sprite as CatGatlingPeaSprite)?.head;
				Check(fire != null && !fire.IsReleased && fire.fireNum == 4, "The real Cat Star Gatling FireComponent must retain a four-shot volley.");
				Check(GodotObject.IsInstanceValid(adobeAnimateSprite) && adobeAnimateSprite.HasClip("HeadFire"), "The real nested head sprite must expose the HeadFire animation.");
				if (fire == null || fire.IsReleased || !GodotObject.IsInstanceValid(adobeAnimateSprite))
				{
					goto end_IL_00fe;
				}
				adobeAnimateSprite.OnAnimeEvent += OnHeadAnimeEvent;
				fire.OnFireVolley += OnNormalVolley;
				fire.OnFireOver += OnFireOver;
				fire.alive = true;
				fire.timer = 0f;
				fire.checkInterval = 0;
				FireComponentCheckConfig fireComponentCheckConfig = fire.fireCheckList[0];
				TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = fireComponentCheckConfig.projectile?.GetProjectile();
				control.isGameRunning = true;
				Check(GodotObject.IsInstanceValid(towerDefenseProjectileCreateData) && fire.CanFireCheckOnce(towerDefenseProjectileCreateData, fireComponentCheckConfig.GetCollisionFlags()), "The real six-direction Cat Star Gatling check must acquire the live target.");
				fire.runningCheck = fireComponentCheckConfig;
				fire.runningCheckId = 0;
				for (int round = 0; round < 3; round++)
				{
					int expectedCompletedRounds = round + 1;
					fire.AttackEntered();
					for (int frame = 0; frame < 240; frame++)
					{
						if (_roundsCompleted >= expectedCompletedRounds)
						{
							break;
						}
						fire.AttackProcessing(1.0 / 60.0);
						_plant.BatchUpdate(1.0 / 60.0);
						await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
						await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
					}
				}
				await WaitPhysicsFrames(2);
				Check(_roundsCompleted == 3, $"The real animation must complete {3} repeated attack rounds; got {_roundsCompleted}.");
				Check(_fireEventsPerRound.Count == 3 && _fireEventsPerRound.TrueForAll((int count) => count == 4), "Every natural HeadFire round must emit four fire events; got [" + string.Join(", ", _fireEventsPerRound) + "].");
				Check(_normalShotsPerRound.Count == 3 && _normalShotsPerRound.TrueForAll((int count) => count == 4), "Every natural round must create four normal bullets; got [" + string.Join(", ", _normalShotsPerRound) + "].");
				Check(_totalFireEvents == 12, $"Three natural rounds must emit 12 fire events; got {_totalFireEvents}.");
				Check(_totalNormalShots == 12, $"Three natural rounds must create 12 normal bullets; got {_totalNormalShots}.");
				int num = 27;
				Check(_spawnedProjectiles == num, $"Three real rounds must create 27 total bullets including five first-shot scatter stars per round; got {_spawnedProjectiles}.");
				Check(fire.currentFireNum == 0 && _plant.currentFireNum == 0, $"Both volley counters must reset after repeated rounds; component={fire.currentFireNum}, plant={_plant.currentFireNum}.");
				goto end_IL_00df;
				end_IL_00fe:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewCatGatlingRepeatedVolleyRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00df;
			}
			return;
			end_IL_00df:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(bulletField))
			{
				bulletField.OnBulletSpawned -= OnBulletSpawned;
			}
			if (GodotObject.IsInstanceValid(ProjectileUpdateManager.Instance))
			{
				ProjectileUpdateManager.Instance.ProcessMode = previousProjectileProcessMode;
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
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			RestoreRealFixtures();
			_plant = null;
			for (int round = 0; round < 4; round++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}
		}
		bool flag = _failures == 0;
		GD.Print($"CAT_GATLING_REPEATED_VOLLEY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = new TowerDefenseMapConfig()
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

	private void OnHeadAnimeEvent(string command, Variant argument)
	{
		if (command == "fire")
		{
			_totalFireEvents++;
		}
	}

	private void OnNormalVolley(ulong randomSeed)
	{
		_totalNormalShots++;
	}

	private void OnBulletSpawned(int index)
	{
		BulletField instance = BulletField.Instance;
		if (GodotObject.IsInstanceValid(instance) && instance.IsBulletActive(index) && instance.GetBulletDataRef(index).fireCharacter == _plant)
		{
			_spawnedProjectiles++;
		}
	}

	private void OnFireOver()
	{
		_roundsCompleted++;
		_fireEventsPerRound.Add(_totalFireEvents - _previousRoundFireEvents);
		_normalShotsPerRound.Add(_totalNormalShots - _previousRoundNormalShots);
		_previousRoundFireEvents = _totalFireEvents;
		_previousRoundNormalShots = _totalNormalShots;
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("PlantCatGatlingPea", "res://Asset/Anime/Character/Plant/Gold/CatGatlingPea/Packet/PlantCatGatlingPea.tres");
		RegisterPacket("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres");
		RegisterCharacter("PlantCatGatlingPea", "res://Asset/Anime/Character/Plant/Gold/CatGatlingPea/Scene/TowerDefensePlantCatGatlingPea.tscn");
		RegisterCharacter("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
		RegisterProjectile("Star", "res://Asset/Config/Projectile/Star/StarDefault.tres");
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

	private void RegisterProjectile(string key, string path)
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.PROJECTILE_CONFIG.TryGetValue(key, out var value))
		{
			_previousProjectiles[key] = value;
		}
		else
		{
			_missingProjectiles.Add(key);
		}
		instance.PROJECTILE_CONFIG[key] = ResourceLoader.Load<TowerDefenseProjectileConfig>(path, null, ResourceLoader.CacheMode.Ignore);
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
		foreach (string missingProjectile in _missingProjectiles)
		{
			instance.PROJECTILE_CONFIG.Remove(missingProjectile);
		}
		foreach (KeyValuePair<string, Resource> previousProjectile in _previousProjectiles)
		{
			instance.PROJECTILE_CONFIG[previousProjectile.Key] = previousProjectile.Value;
		}
	}

	private async Task WaitPhysicsFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewCatGatlingRepeatedVolleyRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnHeadAnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.OnNormalVolley, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "randomSeed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnBulletSpawned, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnFireOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.RegisterProjectile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.OnHeadAnimeEvent && args.Count == 2)
		{
			OnHeadAnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnNormalVolley && args.Count == 1)
		{
			OnNormalVolley(VariantUtils.ConvertTo<ulong>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnBulletSpawned && args.Count == 1)
		{
			OnBulletSpawned(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnFireOver && args.Count == 0)
		{
			OnFireOver();
			ret = default;
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
		if (method == MethodName.RegisterProjectile && args.Count == 2)
		{
			RegisterProjectile(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.OnHeadAnimeEvent)
		{
			return true;
		}
		if (method == MethodName.OnNormalVolley)
		{
			return true;
		}
		if (method == MethodName.OnBulletSpawned)
		{
			return true;
		}
		if (method == MethodName.OnFireOver)
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
		if (method == MethodName.RegisterProjectile)
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
		if (name == PropertyName._spawnedProjectiles)
		{
			_spawnedProjectiles = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._roundsCompleted)
		{
			_roundsCompleted = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._totalFireEvents)
		{
			_totalFireEvents = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._totalNormalShots)
		{
			_totalNormalShots = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._previousRoundFireEvents)
		{
			_previousRoundFireEvents = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._previousRoundNormalShots)
		{
			_previousRoundNormalShots = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._plant)
		{
			_plant = VariantUtils.ConvertTo<TowerDefensePlantCatGatlingPea>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._spawnedProjectiles)
		{
			value = VariantUtils.CreateFrom(in _spawnedProjectiles);
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
		if (name == PropertyName._roundsCompleted)
		{
			value = VariantUtils.CreateFrom(in _roundsCompleted);
			return true;
		}
		if (name == PropertyName._totalFireEvents)
		{
			value = VariantUtils.CreateFrom(in _totalFireEvents);
			return true;
		}
		if (name == PropertyName._totalNormalShots)
		{
			value = VariantUtils.CreateFrom(in _totalNormalShots);
			return true;
		}
		if (name == PropertyName._previousRoundFireEvents)
		{
			value = VariantUtils.CreateFrom(in _previousRoundFireEvents);
			return true;
		}
		if (name == PropertyName._previousRoundNormalShots)
		{
			value = VariantUtils.CreateFrom(in _previousRoundNormalShots);
			return true;
		}
		if (name == PropertyName._plant)
		{
			value = VariantUtils.CreateFrom(in _plant);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._spawnedProjectiles, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._roundsCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._totalFireEvents, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._totalNormalShots, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._previousRoundFireEvents, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._previousRoundNormalShots, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._plant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._spawnedProjectiles, Variant.From(in _spawnedProjectiles));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._roundsCompleted, Variant.From(in _roundsCompleted));
		info.AddProperty(PropertyName._totalFireEvents, Variant.From(in _totalFireEvents));
		info.AddProperty(PropertyName._totalNormalShots, Variant.From(in _totalNormalShots));
		info.AddProperty(PropertyName._previousRoundFireEvents, Variant.From(in _previousRoundFireEvents));
		info.AddProperty(PropertyName._previousRoundNormalShots, Variant.From(in _previousRoundNormalShots));
		info.AddProperty(PropertyName._plant, Variant.From(in _plant));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._spawnedProjectiles, out var value))
		{
			_spawnedProjectiles = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value2))
		{
			_checks = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value3))
		{
			_failures = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._roundsCompleted, out var value4))
		{
			_roundsCompleted = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._totalFireEvents, out var value5))
		{
			_totalFireEvents = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._totalNormalShots, out var value6))
		{
			_totalNormalShots = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._previousRoundFireEvents, out var value7))
		{
			_previousRoundFireEvents = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName._previousRoundNormalShots, out var value8))
		{
			_previousRoundNormalShots = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._plant, out var value9))
		{
			_plant = value9.As<TowerDefensePlantCatGatlingPea>();
		}
	}
}
