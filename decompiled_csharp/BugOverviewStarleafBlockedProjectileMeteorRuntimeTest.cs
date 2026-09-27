using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewStarleafBlockedProjectileMeteorRuntimeTest.cs")]
public class BugOverviewStarleafBlockedProjectileMeteorRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CaptureMeteor = "CaptureMeteor";

		public static readonly StringName CheckMeteorRenderState = "CheckMeteorRenderState";

		public static readonly StringName IsMeteorName = "IsMeteorName";

		public static readonly StringName PlaceAtGrid = "PlaceAtGrid";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _captureField = "_captureField";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string StarleafScenePath = "res://Asset/Anime/Character/Plant/Chapter6/Starleaf/Scene/TowerDefensePlantStarleaf.tscn";

	private const string NormalZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private static readonly Vector2I PlantGrid = new Vector2I(4, 2);

	private static readonly Vector2I ZombieGrid = new Vector2I(6, 2);

	private int _checks;

	private int _failures;

	private readonly List<StringName> _spawnedMeteorNames = new List<StringName>();

	private readonly List<int> _spawnedMeteorIndices = new List<int>();

	private BulletField _captureField;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		ProcessModeEnum previousProjectileProcessMode = ProjectileUpdateManager.Instance?.ProcessMode ?? ProcessModeEnum.Inherit;
		BugOverviewStarleafBlockedProjectileMeteorRuntimeControlStub control = null;
		BulletField bulletField = null;
		try
		{
			_ = 3;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00f1;
				}
				TowerDefenseProjectileRegistry.Init();
				if (GodotObject.IsInstanceValid(ProjectileUpdateManager.Instance))
				{
					ProjectileUpdateManager.Instance.ProcessMode = ProcessModeEnum.Disabled;
				}
				control = new BugOverviewStarleafBlockedProjectileMeteorRuntimeControlStub
				{
					Name = "StarleafBlockedProjectileMeteorRuntimeControl",
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
				bulletField = BulletField.EnsureMountedOnCharacterNode();
				Check(GodotObject.IsInstanceValid(bulletField), "The BulletField must mount for the Starleaf probe.");
				if (!GodotObject.IsInstanceValid(bulletField))
				{
					goto end_IL_00f1;
				}
				bulletField.ClearActiveBullets();
				TowerDefensePlantStarleaf plant = Instantiate<TowerDefensePlantStarleaf>("res://Asset/Anime/Character/Plant/Chapter6/Starleaf/Scene/TowerDefensePlantStarleaf.tscn");
				TowerDefenseZombieNormal zombie = Instantiate<TowerDefenseZombieNormal>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
				Check(GodotObject.IsInstanceValid(plant) && GodotObject.IsInstanceValid(zombie), "The real Starleaf and normal Zombie scenes must instantiate.");
				if (!GodotObject.IsInstanceValid(plant) || !GodotObject.IsInstanceValid(zombie))
				{
					goto end_IL_00f1;
				}
				control.characterNode.AddChild(plant, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				PlaceAtGrid(plant, PlantGrid);
				PlaceAtGrid(zombie, ZombieGrid);
				await WaitFrames(5);
				control.isGameRunning = true;
				BlockComponent block = plant.componentManager?.GetRuntime<BlockComponent>("character.block");
				Check(block != null && block.Lifecycle == ComponentRuntimeLifecycle.Active, "The real Starleaf BlockComponent must be active.");
				Check(manager.GetCharacterTargetNearest(plant, TowerDefenseEnum.TARGET_NEAR_METHOD.POSITION) == zombie, "The real Starleaf must acquire the live Zombie as its meteor target.");
				if (block == null || block.Lifecycle != ComponentRuntimeLifecycle.Active)
				{
					goto end_IL_00f1;
				}
				block.BindBulletField(bulletField);
				block.UpdateRect();
				_captureField = bulletField;
				bulletField.OnBulletSpawned += CaptureMeteor;
				RunBlockedProjectilePass(plant, zombie, block, bulletField, 1, out var firstIndex);
				RunBlockedProjectilePass(plant, zombie, block, bulletField, 2, out var overlappingIndex);
				await WaitFrames(3);
				bulletField.Update(0.0, Engine.GetPhysicsFrames());
				Check(_spawnedMeteorNames.Count == 2, "Two projectiles blocked during the same Block animation must each create a meteor; " + $"count={_spawnedMeteorNames.Count}.");
				Check(_spawnedMeteorNames.TrueForAll(IsMeteorName), "Every overlapping blocked projectile must create MeteorStar or MeteorStarS.");
				CheckMeteorRenderState(bulletField, "Overlapping blocked projectiles", 2);
				await WaitFrames(90);
				bulletField.ClearActiveBullets();
				_spawnedMeteorNames.Clear();
				_spawnedMeteorIndices.Clear();
				RunBlockedProjectilePass(plant, zombie, block, bulletField, 3, out var bulletIndex);
				Check(bulletIndex == firstIndex || bulletIndex == overlappingIndex, "The later pass must reuse a released BulletField slot.");
				await WaitFrames(3);
				bulletField.Update(0.0, Engine.GetPhysicsFrames());
				Check(_spawnedMeteorNames.Count == 1, $"A blocked projectile after slot reuse must create one meteor; count={_spawnedMeteorNames.Count}.");
				Check(IsMeteorName((_spawnedMeteorNames.Count > 0) ? _spawnedMeteorNames[0] : null), "The reused blocked projectile must create MeteorStar or MeteorStarS.");
				CheckMeteorRenderState(bulletField, "Reused blocked projectile");
				goto end_IL_00d6;
				end_IL_00f1:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[StarleafBlockedProjectileMeteor] Unexpected exception: {value}");
				goto end_IL_00d6;
			}
			return;
			end_IL_00d6:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(bulletField))
			{
				bulletField.OnBulletSpawned -= CaptureMeteor;
				bulletField.ClearActiveBullets();
			}
			_spawnedMeteorNames.Clear();
			_spawnedMeteorIndices.Clear();
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
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
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(8);
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag = _failures == 0;
		GD.Print($"STARLEAF_BLOCKED_PROJECTILE_METEOR_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void RunBlockedProjectilePass(TowerDefensePlantStarleaf plant, TowerDefenseZombieNormal zombie, BlockComponent block, BulletField bulletField, int pass, out int bulletIndex)
	{
		TowerDefenseProjectileConfig towerDefenseProjectileConfig = new TowerDefenseProjectileCreateData(new StringName("Pea")).BuildConfig();
		Check(towerDefenseProjectileConfig != null, $"Pass {pass} must build the projectile fixture config.");
		bulletIndex = -1;
		if (towerDefenseProjectileConfig != null)
		{
			Vector2 globalPosition = plant.GlobalPosition;
			bulletIndex = bulletField.TrySpawnFromConfig(towerDefenseProjectileConfig, globalPosition, Vector2.Left * 200f, 200.0, null, TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE, plant.gridPos, plant.gridPos.Y, new Rect2(-10000f, -10000f, 20000f, 20000f), plant, 0.0, 0.0, towerDefenseProjectileConfig.collisionFlags);
			Check(bulletIndex >= 0, $"Pass {pass} must spawn the thrown projectile.");
			if (bulletIndex >= 0)
			{
				ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(bulletIndex);
				bulletDataRef.catapultOpen = true;
				bulletDataRef.catapultTime = 1.0;
				bulletDataRef.catapultTimer = 0.8;
				bulletDataRef.z = 0.0;
				bulletDataRef.gridPos = plant.gridPos;
				bulletDataRef.gridY = plant.gridPos.Y;
				bulletDataRef.target = plant;
				bulletDataRef.camp = zombie.camp;
				block.OnBulletIntersect(ref bulletDataRef, bulletIndex);
				Check(bulletDataRef.blocked && bulletDataRef.hitOver, $"Pass {pass} must visibly bounce the thrown projectile.");
			}
		}
	}

	private void CaptureMeteor(int index)
	{
		if (GodotObject.IsInstanceValid(_captureField) && _captureField.IsBulletActive(index))
		{
			StringName stringName = _captureField.GetBulletDataRef(index).config?.NameSN ?? null;
			if (IsMeteorName(stringName))
			{
				_spawnedMeteorNames.Add(stringName);
				_spawnedMeteorIndices.Add(index);
			}
		}
	}

	private void CheckMeteorRenderState(BulletField bulletField, string context, int expectedMeteorCount = 1)
	{
		Check(_spawnedMeteorIndices.Count == expectedMeteorCount, $"{context} must retain {expectedMeteorCount} captured meteor indices.");
		if (_spawnedMeteorIndices.Count != expectedMeteorCount)
		{
			return;
		}
		for (int i = 0; i < _spawnedMeteorIndices.Count; i++)
		{
			int index = _spawnedMeteorIndices[i];
			Check(bulletField.IsBulletActive(index), $"{context} meteor {i + 1} must remain active while falling.");
			if (bulletField.IsBulletActive(index))
			{
				ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(index);
				Check(bulletDataRef.renderMode == BulletRenderMode.ANIMATED_MESH, $"{context} meteor {i + 1} must use the animated-mesh render path.");
				Check(bulletDataRef.animDefId >= 0, $"{context} meteor {i + 1} must retain a valid animation definition.");
			}
		}
		Check(bulletField.GetAnimatedMeshBucketCountForTest() > 0, context + " meteor must create an animated-mesh draw bucket.");
		Check(bulletField.GetAnimatedMeshVisibleInstanceCountForTest() >= expectedMeteorCount, context + " must submit every meteor as a visible animated-mesh instance.");
	}

	private static bool IsMeteorName(StringName name)
	{
		if (!(name == new StringName("MeteorStar")))
		{
			return name == new StringName("MeteorStarS");
		}
		return true;
	}

	private static T Instantiate<T>(string path) where T : Node
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	private static void PlaceAtGrid(TowerDefenseCharacter character, Vector2I grid)
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		character.inGame = true;
		character.editorPreviewMode = false;
		character.gridPos = grid;
		character.GlobalPosition = instance.GetMapCellPosCenter(grid);
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
			GD.PushError("[StarleafBlockedProjectileMeteor] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CaptureMeteor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckMeteorRenderState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "bulletField", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "context", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedMeteorCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMeteorName, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlaceAtGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "grid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CaptureMeteor && args.Count == 1)
		{
			CaptureMeteor(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckMeteorRenderState && args.Count == 3)
		{
			CheckMeteorRenderState(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsMeteorName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMeteorName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.PlaceAtGrid && args.Count == 2)
		{
			PlaceAtGrid(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
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
		if (method == MethodName.IsMeteorName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMeteorName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.PlaceAtGrid && args.Count == 2)
		{
			PlaceAtGrid(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
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
		if (method == MethodName.CaptureMeteor)
		{
			return true;
		}
		if (method == MethodName.CheckMeteorRenderState)
		{
			return true;
		}
		if (method == MethodName.IsMeteorName)
		{
			return true;
		}
		if (method == MethodName.PlaceAtGrid)
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
		if (name == PropertyName._captureField)
		{
			_captureField = VariantUtils.ConvertTo<BulletField>(in value);
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
		if (name == PropertyName._captureField)
		{
			value = VariantUtils.CreateFrom(in _captureField);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._captureField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._captureField, Variant.From(in _captureField));
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
		if (info.TryGetProperty(PropertyName._captureField, out var value3))
		{
			_captureField = value3.As<BulletField>();
		}
	}
}
