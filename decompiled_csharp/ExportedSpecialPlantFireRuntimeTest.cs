using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/ExportedSpecialPlantFireRuntimeTest.cs")]
public class ExportedSpecialPlantFireRuntimeTest : Node
{
	private readonly record struct PlantCase(string Name, string PacketName, string PacketPath, string ScenePath, Vector2I GridPos, string ProjectileName, int ExpectedProjectiles);

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifyStandalonePoseFallback = "VerifyStandalonePoseFallback";

		public static readonly StringName VerifyAllPoseManifestSources = "VerifyAllPoseManifestSources";

		public static readonly StringName ResolveManifestDefinitionPath = "ResolveManifestDefinitionPath";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName RegisterFixtures = "RegisterFixtures";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

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

	private const string FirePeaSAnimationPath = "res://Asset/Config/Projectile/Pea/Sprite/FirePeaS/FirePeaS.tres";

	private static readonly PlantCase[] Cases = new PlantCase[2]
	{
		new PlantCase("SunPeashooter", "PlantSunPeashooter", "res://Asset/Anime/Character/Plant/Chapter5/SunPeashooter/Packet/PlantSunPeashooter.tres", "res://Asset/Anime/Character/Plant/Chapter5/SunPeashooter/Scene/TowerDefensePlantSunPeashooter.tscn", new Vector2I(2, 1), "WhiteFirePea", 1),
		new PlantCase("ThreePeaterU", "PlantThreePeaterU", "res://Asset/Anime/Character/Plant/Gold/ThreePeaterU/Packet/PlantThreePeaterU.tres", "res://Asset/Anime/Character/Plant/Gold/ThreePeaterU/Scene/TowerDefensePlantThreePeaterU.tscn", new Vector2I(2, 3), "FirePeaS", 3)
	};

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		ProcessModeEnum previousProjectileProcessMode = ProjectileUpdateManager.Instance?.ProcessMode ?? ProcessModeEnum.Inherit;
		ExportedSpecialPlantFireControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ProjectileUpdateManager.Instance), "ProjectileUpdateManager must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance) || !GodotObject.IsInstanceValid(ProjectileUpdateManager.Instance))
				{
					goto end_IL_00e6;
				}
				ProjectileUpdateManager.Instance.ProcessMode = ProcessModeEnum.Disabled;
				TowerDefenseProjectileRegistry.Init();
				RegisterFixtures();
				control = new ExportedSpecialPlantFireControlStub
				{
					Name = "ExportedSpecialPlantFireControl",
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
				PlantCase[] cases = Cases;
				for (int i = 0; i < cases.Length; i++)
				{
					PlantCase item = cases[i];
					TowerDefensePlant plant = LoadPacket(item.PacketPath)?.Plant(item.GridPos, playAudio: false, noLimit: true, default, skipPlacementCheck: true) as TowerDefensePlant;
					await WaitFrames(5);
					AdobeAnimateRuntimeDefinition poisonedDefinition = ((item.ProjectileName == "FirePeaS") ? PoisonFirePeaSDefinitionCache() : null);
					await VerifyAnimatedVolley(item, plant, control);
					if (poisonedDefinition != null)
					{
						VerifyFirePeaSDefinitionRecovered(poisonedDefinition);
					}
					if (GodotObject.IsInstanceValid(plant))
					{
						plant.QueueFree();
					}
					await WaitFrames(3);
				}
				VerifyAllPoseManifestSources();
				VerifyStandalonePoseFallback();
				goto end_IL_00cf;
				end_IL_00e6:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[ExportedSpecialPlantFireRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00cf;
			}
			return;
			end_IL_00cf:;
		}
		finally
		{
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
			RestoreFixtures();
			await WaitFrames(3);
		}
		bool flag = _failures == 0;
		GD.Print($"EXPORTED_SPECIAL_PLANT_FIRE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private AdobeAnimateRuntimeDefinition PoisonFirePeaSDefinitionCache()
	{
		AdobeAnimateData adobeAnimateData = ResourceLoader.Load<AdobeAnimateData>("res://Asset/Config/Projectile/Pea/Sprite/FirePeaS/FirePeaS.tres", null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(adobeAnimateData), "FirePeaS animation data must load before the exported-cache regression.");
		if (!GodotObject.IsInstanceValid(adobeAnimateData))
		{
			return null;
		}
		AdobeAnimateRuntimeDefinition orBuildForCompactCrowd = AdobeAnimateDefinitionCache.GetOrBuildForCompactCrowd(adobeAnimateData);
		Check(HasCompactCrowdTextureArrays(orBuildForCompactCrowd), "FirePeaS must initially resolve both shared exported texture arrays.");
		if (orBuildForCompactCrowd == null)
		{
			return null;
		}
		Check(orBuildForCompactCrowd.HasGpuPoseManifestEntry, "FirePeaS must have a pose allocation in the exported atlas manifest.");
		Check(orBuildForCompactCrowd.GpuPoseSignature == orBuildForCompactCrowd.GpuPoseManifestSignature, $"FirePeaS runtime and manifest pose signatures must match; runtime={orBuildForCompactCrowd.GpuPoseSignature:X16}, manifest={orBuildForCompactCrowd.GpuPoseManifestSignature:X16}.");
		orBuildForCompactCrowd.GpuPoseTexture = null;
		orBuildForCompactCrowd.GpuPoseTextureArray = null;
		orBuildForCompactCrowd.GpuPoseTextureRid = default;
		orBuildForCompactCrowd.GpuPoseTextureSize = Vector2I.Zero;
		orBuildForCompactCrowd.UsesGpuPoseTextureArray = false;
		Check(!HasCompactCrowdTextureArrays(orBuildForCompactCrowd), "The regression fixture must reproduce an early cached definition without a pose array.");
		return orBuildForCompactCrowd;
	}

	private void VerifyFirePeaSDefinitionRecovered(AdobeAnimateRuntimeDefinition poisonedDefinition)
	{
		AdobeAnimateRuntimeDefinition orBuild = AdobeAnimateDefinitionCache.GetOrBuild(ResourceLoader.Load<AdobeAnimateData>("res://Asset/Config/Projectile/Pea/Sprite/FirePeaS/FirePeaS.tres", null, ResourceLoader.CacheMode.Reuse));
		Check(poisonedDefinition != orBuild, "BulletField registration must replace the incomplete cached FirePeaS definition.");
		Check(HasCompactCrowdTextureArrays(orBuild), "Recovered FirePeaS definition must retain valid visual and pose arrays.");
		Check(orBuild != null && orBuild.HasGpuPoseManifestEntry && orBuild.GpuPoseSignature == orBuild.GpuPoseManifestSignature, "Recovered FirePeaS definition must bind the exact manifest pose signature.");
	}

	private void VerifyStandalonePoseFallback()
	{
		AdobeAnimateData adobeAnimateData = ResourceLoader.Load<AdobeAnimateData>("res://Asset/Config/Projectile/Pea/Sprite/FirePeaS/FirePeaS.tres", null, ResourceLoader.CacheMode.Ignore);
		bool flag = GodotObject.IsInstanceValid(adobeAnimateData) && (adobeAnimateData.HasPackedRuntimeData() || adobeAnimateData.EnsurePackedRuntimeData());
		Check(flag, "FirePeaS packed data must be available for the stale-manifest fallback regression.");
		if (flag && adobeAnimateData.sliceAlpha.Length != 0)
		{
			adobeAnimateData.sliceAlpha = (float[])adobeAnimateData.sliceAlpha.Clone();
			adobeAnimateData.sliceAlpha[0] = Math.Clamp(adobeAnimateData.sliceAlpha[0] - 0.125f, 0f, 1f);
			AdobeAnimateDefinitionCache.Invalidate(adobeAnimateData);
			AdobeAnimateRuntimeDefinition orBuildForCompactCrowd = AdobeAnimateDefinitionCache.GetOrBuildForCompactCrowd(adobeAnimateData);
			Check(HasCompactCrowdTextureArrays(orBuildForCompactCrowd), "A stale shared pose signature must rebuild a usable standalone pose array.");
			Check(orBuildForCompactCrowd != null && !orBuildForCompactCrowd.UsesBakedGpuPoseTexture && orBuildForCompactCrowd.UsesGpuPoseTextureArray, "The stale-signature recovery must use the runtime-built pose array, not mislabel it as baked.");
			Check(orBuildForCompactCrowd != null && orBuildForCompactCrowd.HasGpuPoseManifestEntry && orBuildForCompactCrowd.GpuPoseSignature != orBuildForCompactCrowd.GpuPoseManifestSignature, "The fallback regression must preserve the manifest mismatch that triggered runtime rebuilding.");
			bool value = HasCompactCrowdTextureArrays(orBuildForCompactCrowd) && orBuildForCompactCrowd != null && !orBuildForCompactCrowd.UsesBakedGpuPoseTexture && orBuildForCompactCrowd.UsesGpuPoseTextureArray && orBuildForCompactCrowd.HasGpuPoseManifestEntry && orBuildForCompactCrowd.GpuPoseSignature != orBuildForCompactCrowd.GpuPoseManifestSignature;
			GD.Print($"EXPORTED_ADOBE_ANIMATE_POSE_FALLBACK_RESULT passed={value} runtime={orBuildForCompactCrowd?.GpuPoseSignature:X16} manifest={orBuildForCompactCrowd?.GpuPoseManifestSignature:X16}");
			AdobeAnimateDefinitionCache.Invalidate(adobeAnimateData);
		}
	}

	private void VerifyAllPoseManifestSources()
	{
		AdobeAnimateGlobalAtlasManifest adobeAnimateGlobalAtlasManifest = ResourceLoader.Load<AdobeAnimateGlobalAtlasManifest>("res://addons/AdobeAnimateEditor/GeneratedAtlas/AdobeAnimateGlobalAtlasManifest.tres", null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(adobeAnimateGlobalAtlasManifest) && adobeAnimateGlobalAtlasManifest.SourceKeys.Count > 0, "The exported global atlas manifest must expose its animation sources.");
		if (!GodotObject.IsInstanceValid(adobeAnimateGlobalAtlasManifest) || adobeAnimateGlobalAtlasManifest.SourceKeys.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		int num = 0;
		for (int i = 0; i < adobeAnimateGlobalAtlasManifest.SourceKeys.Count; i++)
		{
			string text = adobeAnimateGlobalAtlasManifest.SourceKeys[i];
			string text2 = ResolveManifestDefinitionPath(text);
			AdobeAnimateData adobeAnimateData = ResourceLoader.Load<AdobeAnimateData>(text2, null, ResourceLoader.CacheMode.Ignore);
			if (!GodotObject.IsInstanceValid(adobeAnimateData))
			{
				list.Add(text + ": missing " + text2);
				continue;
			}
			AdobeAnimateRuntimeDefinition orBuild = AdobeAnimateDefinitionCache.GetOrBuild(adobeAnimateData);
			if (!HasCompactCrowdTextureArrays(orBuild) || !orBuild.HasGpuPoseManifestEntry || orBuild.GpuPoseSignature != orBuild.GpuPoseManifestSignature)
			{
				list.Add($"{text}: runtime={orBuild?.GpuPoseSignature:X16} manifest={orBuild?.GpuPoseManifestSignature:X16} arrays={HasCompactCrowdTextureArrays(orBuild)}");
			}
			else
			{
				num++;
			}
		}
		string value = ((list.Count == 0) ? string.Empty : string.Join("; ", list.GetRange(0, Math.Min(8, list.Count))));
		Check(list.Count == 0 && num == adobeAnimateGlobalAtlasManifest.SourceKeys.Count, $"Every DAT-free exported animation must bind its shared pose atlas; verified={num}/{adobeAnimateGlobalAtlasManifest.SourceKeys.Count}, failures={list.Count}, first={value}.");
		GD.Print($"EXPORTED_ADOBE_ANIMATE_POSE_MANIFEST_RESULT passed={list.Count == 0} verified={num} total={adobeAnimateGlobalAtlasManifest.SourceKeys.Count} failures={list.Count}");
	}

	private static string ResolveManifestDefinitionPath(string sourceKey)
	{
		return sourceKey switch
		{
			"res://Asset/Anime/Character/GraveStone/Skeleton/Skeletuar.dat" => "res://Asset/Anime/Character/GraveStone/Skeleton/Skeleton.tres", 
			"res://Asset/Anime/Character/Plant/Other/FumeSeaShroom/FumeShroomSea.dat" => "res://Asset/Anime/Character/Plant/Other/FumeSeaShroom/FumeSeaShroom.tres", 
			"res://Asset/Anime/Character/Plant/Other/SplitPeaCat/SpiltPeaCat.dat" => "res://Asset/Anime/Character/Plant/Other/SplitPeaCat/SplitPeaCat.tres", 
			"res://Asset/Anime/Character/Plant/Other/ThreeHybirdPeater/ThreeHybridPeater.dat" => "res://Asset/Anime/Character/Plant/Other/ThreeHybirdPeater/ThreeHybirdPeater.tres", 
			_ => sourceKey.GetBaseDir().PathJoin(sourceKey.GetFile().GetBaseName() + ".tres"), 
		};
	}

	private static bool HasCompactCrowdTextureArrays(AdobeAnimateRuntimeDefinition definition)
	{
		if (definition != null && definition.AtlasTextureArrayRid.IsValid && GodotObject.IsInstanceValid(definition.AtlasTextureArray) && definition.AtlasTextureArraySize.X > 0f && definition.AtlasTextureArraySize.Y > 0f && definition.UsesGpuPoseTextureArray && definition.GpuPoseTextureRid.IsValid && GodotObject.IsInstanceValid(definition.GpuPoseTextureArray) && definition.GpuPoseTextureSize.X > 0)
		{
			return definition.GpuPoseTextureSize.Y > 0;
		}
		return false;
	}

	private async Task VerifyAnimatedVolley(PlantCase item, TowerDefensePlant plant, ExportedSpecialPlantFireControlStub control)
	{
		Check(GodotObject.IsInstanceValid(plant) && plant.config?.name == item.PacketName, item.Name + " real scene must instantiate inside the battlefield.");
		if (!GodotObject.IsInstanceValid(plant))
		{
			return;
		}
		FireComponent fire = plant.componentManager?.GetRuntime<FireComponent>("character.fire");
		Check(fire != null && !fire.IsReleased, item.Name + " must expose an active FireComponent runtime.");
		if (fire == null || fire.IsReleased)
		{
			return;
		}
		FireComponentCheckConfig fireComponentCheckConfig = ((fire.fireCheckList.Count > 0) ? fire.fireCheckList[0] : null);
		TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = fireComponentCheckConfig?.projectile?.GetProjectile();
		Check(GodotObject.IsInstanceValid(towerDefenseProjectileCreateData) && towerDefenseProjectileCreateData.projectileName == new StringName(item.ProjectileName), item.Name + " must retain projectile " + item.ProjectileName + " in its runtime definition.");
		Check(GodotObject.IsInstanceValid(fire.sprite) && fire.sprite.HasClip(fire.fireAnimeClips), item.Name + " must retain fire animation " + fire.fireAnimeClips + ".");
		if (!GodotObject.IsInstanceValid(towerDefenseProjectileCreateData) || !GodotObject.IsInstanceValid(fire.sprite))
		{
			return;
		}
		BulletField field = BulletField.EnsureMountedOnCharacterNode();
		Check(GodotObject.IsInstanceValid(field), item.Name + " must have a mounted BulletField.");
		if (!GodotObject.IsInstanceValid(field))
		{
			return;
		}
		field.ClearActiveBullets();
		int animationEvents = 0;
		int volleySignals = 0;
		int spawnedProjectiles = 0;
		bool previousCheckUse = fire.checkUse;
		fire.sprite.OnAnimeEvent += OnAnimationEvent;
		fire.OnFireVolley += OnVolley;
		field.OnBulletSpawned += OnBulletSpawned;
		try
		{
			control.isGameRunning = true;
			fire.alive = true;
			fire.checkUse = false;
			fire.runningCheck = fireComponentCheckConfig;
			fire.runningCheckId = 0;
			fire.currentFireNum = 0;
			fire.AttackEntered();
			for (int frame = 0; frame < 240; frame++)
			{
				if (spawnedProjectiles >= item.ExpectedProjectiles)
				{
					break;
				}
				fire.AttackProcessing(1.0 / 60.0);
				plant.BatchUpdate(1.0 / 60.0);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			}
		}
		finally
		{
			fire.checkUse = previousCheckUse;
			fire.alive = false;
			fire.sprite.OnAnimeEvent -= OnAnimationEvent;
			fire.OnFireVolley -= OnVolley;
			field.OnBulletSpawned -= OnBulletSpawned;
		}
		Check(animationEvents == 1, $"{item.Name} packed fire animation must emit one fire event; events={animationEvents}.");
		Check(volleySignals == item.ExpectedProjectiles, $"{item.Name} fire event must emit {item.ExpectedProjectiles} volley signals; volleys={volleySignals}.");
		Check(spawnedProjectiles == item.ExpectedProjectiles, $"{item.Name} fire event must spawn {item.ExpectedProjectiles} BulletField projectiles; spawned={spawnedProjectiles}.");
		void OnAnimationEvent(string command, Variant _)
		{
			if (command == fire.fireEventName)
			{
				animationEvents++;
			}
		}
		void OnBulletSpawned(int index)
		{
			if (field.GetBulletDataRef(index).fireCharacter == plant)
			{
				spawnedProjectiles++;
			}
		}
		void OnVolley(ulong _)
		{
			volleySignals++;
		}
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

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private void RegisterFixtures()
	{
		PlantCase[] cases = Cases;
		for (int i = 0; i < cases.Length; i++)
		{
			PlantCase plantCase = cases[i];
			RegisterPacket(plantCase.PacketName, plantCase.PacketPath);
			RegisterCharacter(plantCase.PacketName, plantCase.ScenePath);
		}
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

	private void RestoreFixtures()
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
			GD.PushError("[ExportedSpecialPlantFireRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyStandalonePoseFallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyAllPoseManifestSources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveManifestDefinitionPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.VerifyStandalonePoseFallback && args.Count == 0)
		{
			VerifyStandalonePoseFallback();
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyAllPoseManifestSources && args.Count == 0)
		{
			VerifyAllPoseManifestSources();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveManifestDefinitionPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveManifestDefinitionPath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.RegisterFixtures && args.Count == 0)
		{
			RegisterFixtures();
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
		if (method == MethodName.ResolveManifestDefinitionPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveManifestDefinitionPath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.VerifyStandalonePoseFallback)
		{
			return true;
		}
		if (method == MethodName.VerifyAllPoseManifestSources)
		{
			return true;
		}
		if (method == MethodName.ResolveManifestDefinitionPath)
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
		if (method == MethodName.RegisterFixtures)
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
