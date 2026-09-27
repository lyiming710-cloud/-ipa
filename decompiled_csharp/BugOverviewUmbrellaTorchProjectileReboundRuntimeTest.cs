using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewUmbrellaTorchProjectileReboundRuntimeTest.cs")]
public class BugOverviewUmbrellaTorchProjectileReboundRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RunComponentOrderChecks = "RunComponentOrderChecks";

		public static readonly StringName RunFirstSpawnAndReuseChecks = "RunFirstSpawnAndReuseChecks";

		public static readonly StringName RunAllRegisteredProjectileChecks = "RunAllRegisteredProjectileChecks";

		public static readonly StringName DispatchRegisteredResourceZones = "DispatchRegisteredResourceZones";

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

	private const string FixtureScenePath = "res://Asset/Anime/Character/Plant/Chapter7/UmbrellaTorch/Scene/TowerDefensePlantUmbrellaTorch.tscn";

	private const string UmbrellaTorchComponentSetPath = "res://Asset/Anime/Character/Plant/Chapter7/UmbrellaTorch/Scene/TowerDefensePlantUmbrellaTorchComponentSet.tres";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		BulletField bulletField = null;
		TowerDefensePlant plant = null;
		PackedScene fixtureScene = null;
		TowerDefenseControlNew previousControl = TowerDefenseManager.Instance.currentControl;
		TowerDefenseControlNew control = new TowerDefenseControlNew();
		TowerDefenseBattleFeatureMap map = new TowerDefenseBattleFeatureMap
		{
			rect = new Rect2(-10000f, -10000f, 20000f, 20000f)
		};
		try
		{
			_ = 1;
			try
			{
				control.featureDictionary["Map"] = map;
				TowerDefenseManager.Instance.currentControl = control;
				TowerDefenseProjectileRegistry.Init();
				bulletField = new BulletField
				{
					Name = "UmbrellaTorchReboundBulletField"
				};
				AddChild(bulletField, forceReadableName: false, InternalMode.Disabled);
				fixtureScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter7/UmbrellaTorch/Scene/TowerDefensePlantUmbrellaTorch.tscn", null, ResourceLoader.CacheMode.Ignore);
				plant = fixtureScene?.Instantiate<TowerDefensePlant>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(plant), "The live plant fixture scene must instantiate.");
				if (GodotObject.IsInstanceValid(plant))
				{
					CharacterComponentSet characterComponentSet = ResourceLoader.Load<CharacterComponentSet>("res://Asset/Anime/Character/Plant/Chapter7/UmbrellaTorch/Scene/TowerDefensePlantUmbrellaTorchComponentSet.tres", null, ResourceLoader.CacheMode.Ignore);
					Check(characterComponentSet != null, "The real Umbrella Torch component set must load.");
					plant.Name = "UmbrellaTorchReboundFixture";
					plant.editorPreviewMode = true;
					plant.inGame = false;
					plant.camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT;
					plant.gridPos = new Vector2I(3, 2);
					plant.Position = new Vector2(320f, 180f);
					plant.ComponentSet = characterComponentSet;
					AddChild(plant, forceReadableName: false, InternalMode.Disabled);
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
					await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
					ComponentManager componentManager = plant.componentManager;
					componentManager.InitializeResourceComponents();
					componentManager.ActivateResourceComponents();
					RunComponentOrderChecks(componentManager);
					RunFirstSpawnAndReuseChecks(bulletField, plant, componentManager);
					RunAllRegisteredProjectileChecks(bulletField, plant, componentManager);
				}
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[UmbrellaTorchProjectileRebound] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(bulletField))
			{
				bulletField.ClearActiveBullets();
			}
			if (GodotObject.IsInstanceValid(plant))
			{
				plant.QueueFree();
			}
			if (GodotObject.IsInstanceValid(bulletField))
			{
				bulletField.QueueFree();
			}
			for (int frame = 0; frame < 4; frame++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			}
			TowerDefenseManager.Instance.currentControl = previousControl;
			control.Free();
			map.Dispose();
			fixtureScene?.Dispose();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag = _failures == 0;
		GD.Print($"UMBRELLA_TORCH_PROJECTILE_REBOUND_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void RunComponentOrderChecks(ComponentManager manager)
	{
		Check(GodotObject.IsInstanceValid(manager), "The fixture must initialize its ComponentManager.");
		if (GodotObject.IsInstanceValid(manager))
		{
			BlockComponent runtime = manager.GetRuntime<BlockComponent>("character.block");
			ChangeProjectileComponent runtime2 = manager.GetRuntime<ChangeProjectileComponent>("change_projectile");
			Check(runtime != null && runtime.Lifecycle == ComponentRuntimeLifecycle.Active, "The real Umbrella Torch BlockComponent must be active.");
			Check(runtime2 != null && runtime2.Lifecycle == ComponentRuntimeLifecycle.Active, "The real Umbrella Torch ChangeProjectileComponent must be active.");
			int num = IndexOfRuntime(manager, runtime);
			int num2 = IndexOfRuntime(manager, runtime2);
			Check(num >= 0 && num2 >= 0 && num < num2, "The rebound zone must run before fire conversion can end same-frame zone dispatch.");
		}
	}

	private void RunFirstSpawnAndReuseChecks(BulletField bulletField, TowerDefensePlant plant, ComponentManager manager)
	{
		Check(GodotObject.IsInstanceValid(bulletField), "The runtime check requires a live BulletField.");
		TowerDefenseProjectileConfig towerDefenseProjectileConfig = new TowerDefenseProjectileCreateData(new StringName("Pea")).BuildConfig();
		Check(towerDefenseProjectileConfig != null, "The canonical Pea config must build for the rebound check.");
		Check(TowerDefenseProjectileRegistry.GetProjectileChange(new StringName("Pea"), new StringName("Fire")) == new StringName("FirePea"), "The live projectile registry must map Pea through Fire conversion.");
		if (!GodotObject.IsInstanceValid(bulletField) || towerDefenseProjectileConfig == null)
		{
			return;
		}
		int num = -1;
		for (int i = 1; i <= 2; i++)
		{
			bulletField.ClearActiveBullets();
			Vector2 vel = new Vector2(500f, 120f);
			Vector2 pos = plant.GlobalPosition + new Vector2(-8f, -16f);
			int num2 = bulletField.TrySpawnFromConfig(towerDefenseProjectileConfig, pos, vel, vel.Length(), null, plant.camp, plant.gridPos, plant.gridPos.Y, new Rect2(-10000f, -10000f, 20000f, 20000f), null, 0.0, 0.0, towerDefenseProjectileConfig.collisionFlags);
			Check(num2 >= 0, $"Pass {i} must spawn a struct Pea.");
			if (num2 >= 0)
			{
				if (i == 1)
				{
					num = num2;
				}
				else
				{
					Check(num2 == num, "The second pass must exercise the same recycled BulletField slot.");
				}
				ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(num2);
				Check(bulletDataRef.vel.X > 0f && Mathf.IsEqualApprox(bulletDataRef.vel.Y, vel.Y) && bulletDataRef.fireDirX > 0f, $"Pass {i} must begin with a right-moving Pea that has vertical velocity.");
				DispatchRegisteredResourceZones(bulletField, manager, num2);
				Check(bulletField.IsBulletActive(num2), $"Pass {i} must keep the reflected projectile active.");
				if (bulletField.IsBulletActive(num2))
				{
					ref BulletData bulletDataRef2 = ref bulletField.GetBulletDataRef(num2);
					Check(bulletDataRef2.config?.NameSN == new StringName("FirePea"), $"Pass {i} must still apply Umbrella Torch fire conversion.");
					Check(bulletDataRef2.vel.X < 0f && Mathf.IsEqualApprox(bulletDataRef2.vel.Y, vel.Y) && bulletDataRef2.fireDirX < 0f, $"Pass {i} must reverse only projectile X movement before conversion ends dispatch.");
					Check(!GodotObject.IsInstanceValid(bulletDataRef2.target) && !GodotObject.IsInstanceValid(bulletDataRef2.fireCharacter), $"Pass {i} must release stale targeting after rebound.");
				}
			}
		}
	}

	private void RunAllRegisteredProjectileChecks(BulletField bulletField, TowerDefensePlant plant, ComponentManager manager)
	{
		System.Reflection.MethodInfo method = typeof(BulletField).GetMethod("ProcessZonesForBullet", BindingFlags.Instance | BindingFlags.NonPublic);
		BlockComponent runtime = manager.GetRuntime<BlockComponent>("character.block");
		bulletField.UpdateZoneRects();
		TowerDefenseProjectile towerDefenseProjectile = ResourceLoader.Load<PackedScene>("res://Prefab/TowerDefense/Projectile/TowerDefenseProjectile.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<TowerDefenseProjectile>(PackedScene.GenEditState.Disabled);
		AddChild(towerDefenseProjectile, forceReadableName: false, InternalMode.Disabled);
		int reboundEvents = 0;
		BlockComponent.ProjectileReboundEventHandler value = () =>
		{
			reboundEvents++;
		};
		runtime.OnProjectileRebound += value;
		int num = 0;
		int num2 = 0;
		foreach (StringName key in TowerDefenseProjectileRegistry.ProjectileDictionary.Keys)
		{
			TowerDefenseProjectileConfig towerDefenseProjectileConfig = new TowerDefenseProjectileCreateData(key).BuildConfig();
			Check(towerDefenseProjectileConfig != null, $"{key}: 配置必须能够构建。");
			if (towerDefenseProjectileConfig == null)
			{
				continue;
			}
			num++;
			bool flag = (towerDefenseProjectileConfig.fireMethodFlags & 2) == 0 && ((towerDefenseProjectileConfig.fireMethodFlags & 1) != 0 || towerDefenseProjectileConfig.isStar);
			if (flag)
			{
				num2++;
			}
			else
			{
				GD.Print($"UMBRELLA_FILTERED name={key} flags={towerDefenseProjectileConfig.fireMethodFlags} star={towerDefenseProjectileConfig.isStar}");
			}
			for (int num3 = 0; num3 < 10; num3++)
			{
				bulletField.ClearActiveBullets();
				float num4 = ((num3 == 1) ? 60f : 0f);
				Vector2 vector = plant.GlobalPosition + new Vector2(num3 switch
				{
					2 => -20f, 
					4 => 8f, 
					_ => -8f, 
				}, -16f - num4);
				Vector2 vector2 = new Vector2((num3 == 3 || num3 == 4) ? (-500f) : 500f, 0f);
				int num5 = num3 switch
				{
					6 => 2, 
					5 => 32, 
					_ => towerDefenseProjectileConfig.fireMethodFlags, 
				};
				TowerDefenseEnum.CHARACTER_CAMP camp = ((num3 == 7) ? TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE : plant.camp);
				BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
				{
					fireMethodFlagsOverride = num5
				};
				Vector2 vel = vector2;
				Vector2I gridPos = plant.gridPos;
				int y = plant.gridPos.Y;
				Rect2 rect = new Rect2(-10000f, -10000f, 20000f, 20000f);
				double height = num4;
				bool useGravity = num3 == 8;
				int num6 = bulletField.TrySpawnFromConfig(towerDefenseProjectileConfig, vector, vel, 500.0, null, camp, gridPos, y, rect, null, height, 0.0, -1, -1f, checkHeight: false, checkAll: false, num3 == 9, useGravity, 1.5f, 0f, 0f, overrides);
				Check(num6 >= 0, $"{key}/{num3}: 真实子弹必须成功生成。");
				if (num6 >= 0)
				{
					int num7 = reboundEvents;
					method.Invoke(bulletField, new object[1] { num6 });
					ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(num6);
					bool flag2 = flag && num3 != 3 && num3 != 6 && num3 != 7 && (num3 != 5 || towerDefenseProjectileConfig.isStar);
					Check(reboundEvents - num7 == (flag2 ? 1 : 0) && (!flag2 || Mathf.Sign(bulletDataRef.vel.X) == -Mathf.Sign(vector2.X)), $"{key}/{num3}: 批量子弹反弹错误，flags={num5}, vel={bulletDataRef.vel}。");
					towerDefenseProjectile.config = towerDefenseProjectileConfig;
					towerDefenseProjectile.GlobalPosition = vector;
					towerDefenseProjectile.hitBox.Position = new Vector2(0f, num4);
					towerDefenseProjectile.velocity = vector2;
					towerDefenseProjectile.camp = camp;
					towerDefenseProjectile.fireMethodFlags = num5;
					towerDefenseProjectile.catapultOpen = num3 == 6;
					towerDefenseProjectile.trackOpen = num3 == 5;
					towerDefenseProjectile.useGravity = num3 == 8;
					towerDefenseProjectile.useFall = num3 == 9;
					num7 = reboundEvents;
					runtime.OnProjectileIntersect(towerDefenseProjectile);
					Check(reboundEvents - num7 == (flag2 ? 1 : 0) && (!flag2 || Mathf.Sign(towerDefenseProjectile.velocity.X) == -Mathf.Sign(vector2.X)), $"{key}/{num3}: 节点子弹反弹错误，flags={num5}, vel={towerDefenseProjectile.velocity}。");
				}
			}
		}
		runtime.OnProjectileRebound -= value;
		towerDefenseProjectile.Free();
		GD.Print($"UMBRELLA_REGISTRY_AUDIT total={num} rebound={num2} excluded={num - num2}");
	}

	private static int IndexOfRuntime(ComponentManager manager, CharacterComponentRuntime target)
	{
		for (int i = 0; i < manager.ResourceComponents.Count; i++)
		{
			if (manager.ResourceComponents[i] == target)
			{
				return i;
			}
		}
		return -1;
	}

	private static void DispatchRegisteredResourceZones(BulletField bulletField, ComponentManager manager, int bulletIndex)
	{
		TowerDefenseProjectileConfig config = bulletField.GetBulletDataRef(bulletIndex).config;
		for (int i = 0; i < manager.ResourceComponents.Count; i++)
		{
			if (!(manager.ResourceComponents[i] is IProjectileZone projectileZone))
			{
				continue;
			}
			projectileZone.UpdateRect();
			if (bulletField.IsBulletIntersectingRect(bulletIndex, projectileZone.WorldRect))
			{
				projectileZone.OnBulletIntersect(ref bulletField.GetBulletDataRef(bulletIndex), bulletIndex);
				if (!bulletField.IsBulletActive(bulletIndex) || bulletField.GetBulletDataRef(bulletIndex).config != config)
				{
					break;
				}
			}
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[UmbrellaTorchProjectileRebound] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(6)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RunComponentOrderChecks, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RunFirstSpawnAndReuseChecks, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "bulletField", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RunAllRegisteredProjectileChecks, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "bulletField", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.DispatchRegisteredResourceZones, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "bulletField", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "bulletIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RunComponentOrderChecks && args.Count == 1)
		{
			RunComponentOrderChecks(VariantUtils.ConvertTo<ComponentManager>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunFirstSpawnAndReuseChecks && args.Count == 3)
		{
			RunFirstSpawnAndReuseChecks(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<TowerDefensePlant>(in args[1]), VariantUtils.ConvertTo<ComponentManager>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunAllRegisteredProjectileChecks && args.Count == 3)
		{
			RunAllRegisteredProjectileChecks(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<TowerDefensePlant>(in args[1]), VariantUtils.ConvertTo<ComponentManager>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchRegisteredResourceZones && args.Count == 3)
		{
			DispatchRegisteredResourceZones(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<ComponentManager>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
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
		if (method == MethodName.DispatchRegisteredResourceZones && args.Count == 3)
		{
			DispatchRegisteredResourceZones(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<ComponentManager>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
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
		if (method == MethodName.RunComponentOrderChecks)
		{
			return true;
		}
		if (method == MethodName.RunFirstSpawnAndReuseChecks)
		{
			return true;
		}
		if (method == MethodName.RunAllRegisteredProjectileChecks)
		{
			return true;
		}
		if (method == MethodName.DispatchRegisteredResourceZones)
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
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
