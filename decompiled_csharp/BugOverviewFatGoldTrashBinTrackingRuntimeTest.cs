using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewFatGoldTrashBinTrackingRuntimeTest.cs")]
public class BugOverviewFatGoldTrashBinTrackingRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifyRealFatGoldTracking = "VerifyRealFatGoldTracking";

		public static readonly StringName PlaceCharacter = "PlaceCharacter";

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

	private const string FatGoldScenePath = "res://Asset/Anime/Character/Zombie/Chapter7/FatGold/Scene/TowerDefenseZombieFatGold.tscn";

	private const string PeaShooterScenePath = "res://Asset/Anime/Character/Plant/Chapter0/PeaShooter/Scene/TowerDefensePlantPeaShooter.tscn";

	private const string TrashBinScenePath = "res://Asset/Anime/Character/GraveStone/TrashBin/Scene/TowerDefenseTrashBin.tscn";

	private static readonly Vector2 GridBegin = new Vector2(100f, 100f);

	private static readonly Vector2 GridSize = new Vector2(100f, 76f);

	private static readonly Vector2I GridNum = new Vector2I(9, 5);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		ProjectileUpdateManager projectileUpdateManager = null;
		ProcessModeEnum previousProjectileProcessMode = ProcessModeEnum.Inherit;
		BugOverviewFatGoldTrashBinTrackingControlStub control = null;
		BulletField bulletField = null;
		bool ownsBulletField = false;
		TowerDefenseZombieFatGold fatGold = null;
		TowerDefenseCharacter plant = null;
		TowerDefenseGravestone trashBin = null;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					throw new InvalidOperationException("TowerDefenseManager is unavailable.");
				}
				control = new BugOverviewFatGoldTrashBinTrackingControlStub
				{
					Name = "FatGoldTrashBinTrackingControl",
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
				manager.gridBeginPos = GridBegin;
				manager.gridSize = GridSize;
				manager.gridNum = GridNum;
				TowerDefenseProjectileRegistry.Init();
				bulletField = BulletField.Instance;
				if (!GodotObject.IsInstanceValid(bulletField))
				{
					bulletField = new BulletField
					{
						Name = "FatGoldTrashBinTrackingBulletField"
					};
					AddChild(bulletField, forceReadableName: false, InternalMode.Disabled);
					ownsBulletField = true;
					await WaitFrames(1);
				}
				Check(GodotObject.IsInstanceValid(bulletField), "A live BulletField must be available for FatGold's real tracking projectile.");
				if (!GodotObject.IsInstanceValid(bulletField))
				{
					throw new InvalidOperationException("BulletField is unavailable.");
				}
				projectileUpdateManager = ProjectileUpdateManager.Instance;
				if (GodotObject.IsInstanceValid(projectileUpdateManager))
				{
					previousProjectileProcessMode = projectileUpdateManager.ProcessMode;
					projectileUpdateManager.ProcessMode = ProcessModeEnum.Disabled;
				}
				bulletField.ClearActiveBullets();
				fatGold = Instantiate<TowerDefenseZombieFatGold>("res://Asset/Anime/Character/Zombie/Chapter7/FatGold/Scene/TowerDefenseZombieFatGold.tscn");
				plant = Instantiate<TowerDefenseCharacter>("res://Asset/Anime/Character/Plant/Chapter0/PeaShooter/Scene/TowerDefensePlantPeaShooter.tscn");
				trashBin = Instantiate<TowerDefenseGravestone>("res://Asset/Anime/Character/GraveStone/TrashBin/Scene/TowerDefenseTrashBin.tscn");
				Check(GodotObject.IsInstanceValid(fatGold), "The real FatGold scene must instantiate.");
				Check(GodotObject.IsInstanceValid(plant), "The real PeaShooter target scene must instantiate.");
				Check(GodotObject.IsInstanceValid(trashBin), "The real TrashBin gravestone scene must instantiate.");
				if (!GodotObject.IsInstanceValid(fatGold) || !GodotObject.IsInstanceValid(plant) || !GodotObject.IsInstanceValid(trashBin))
				{
					throw new InvalidOperationException("A real character fixture failed to instantiate.");
				}
				PlaceCharacter(fatGold, new Vector2I(8, 3));
				PlaceCharacter(plant, new Vector2I(4, 3));
				PlaceCharacter(trashBin, new Vector2I(7, 3));
				trashBin.rise = false;
				control.characterNode.AddChild(fatGold, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(plant, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(trashBin, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				fatGold.ProcessMode = ProcessModeEnum.Disabled;
				plant.ProcessMode = ProcessModeEnum.Disabled;
				trashBin.ProcessMode = ProcessModeEnum.Disabled;
				VerifyRealFatGoldTracking(manager, bulletField, fatGold, plant, trashBin);
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewFatGoldTrashBinTrackingRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(bulletField))
			{
				bulletField.ClearActiveBullets();
			}
			if (GodotObject.IsInstanceValid(projectileUpdateManager))
			{
				projectileUpdateManager.ProcessMode = previousProjectileProcessMode;
			}
			if (GodotObject.IsInstanceValid(fatGold))
			{
				fatGold.QueueFree();
			}
			if (GodotObject.IsInstanceValid(plant))
			{
				plant.QueueFree();
			}
			if (GodotObject.IsInstanceValid(trashBin))
			{
				trashBin.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (ownsBulletField && GodotObject.IsInstanceValid(bulletField))
			{
				bulletField.QueueFree();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag = _failures == 0;
		GD.Print($"BUG_OVERVIEW_I62_FATGOLD_TRASHBIN_TRACKING_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void VerifyRealFatGoldTracking(TowerDefenseManager manager, BulletField bulletField, TowerDefenseZombieFatGold fatGold, TowerDefenseCharacter plant, TowerDefenseGravestone trashBin)
	{
		FireComponent fireComponent = fatGold.componentManager?.GetRuntime<FireComponent>("character.fire");
		Check(fireComponent != null && !fireComponent.IsReleased, "The real FatGold FireComponent must be active.");
		if (fireComponent == null || fireComponent.IsReleased)
		{
			throw new InvalidOperationException("FatGold FireComponent is unavailable.");
		}
		Check(fireComponent.FilterGravestone, "Zombie FireComponent targeting must filter gravestones even when checkGravestone is authored true.");
		int condition;
		if (trashBin.camp != fatGold.camp)
		{
			TargetRegistrationComponent targetRegistrationComponent = trashBin.targetRegistrationComponent;
			if (targetRegistrationComponent != null && targetRegistrationComponent.canProjectileCheck && trashBin.IsHitBoxEnabled)
			{
				condition = (((trashBin.instance.maskFlags & 1) != 0) ? 1 : 0);
				goto IL_009b;
			}
		}
		condition = 0;
		goto IL_009b;
		IL_009b:
		Check((byte)condition != 0, "The real TrashBin must be an otherwise eligible opposing ground target for this regression.");
		TowerDefenseProjectile towerDefenseProjectile = new TowerDefenseProjectile
		{
			fireCharacter = fatGold,
			camp = fatGold.camp
		};
		try
		{
			Check(!towerDefenseProjectile.CanTarget(trashBin), "The Node projectile path must apply the same zombie gravestone filter as BulletField.");
			towerDefenseProjectile.fireCharacter = plant;
			towerDefenseProjectile.camp = plant.camp;
			Check(towerDefenseProjectile.CanTarget(trashBin), "Plant projectiles must remain able to target the attackable TrashBin.");
		}
		finally
		{
			towerDefenseProjectile.Free();
		}
		TowerDefenseCharacter projectileInitialTrackTarget = manager.GetProjectileInitialTrackTarget(fatGold.GlobalPosition, 1, fatGold.camp, TowerDefenseEnum.TARGET_NEAR_METHOD.POSITION, fireComponent.FilterGravestone);
		Check(projectileInitialTrackTarget == plant, "FatGold's production initial tracking query must select the plant and exclude the nearer TrashBin.");
		int activeCount = bulletField.ActiveCount;
		fatGold.ThrowEntered();
		int lastSpawnedIndex = bulletField.LastSpawnedIndex;
		bool flag = bulletField.ActiveCount == activeCount + 1 && lastSpawnedIndex >= 0 && bulletField.IsBulletActive(lastSpawnedIndex);
		Check(flag, "FatGold ThrowEntered must emit one real currency projectile through BulletField.");
		if (!flag)
		{
			throw new InvalidOperationException("FatGold did not emit a BulletField projectile.");
		}
		ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(lastSpawnedIndex);
		Check(!bulletDataRef.trackOpen && bulletDataRef.fireCharacter == fatGold && bulletDataRef.camp == fatGold.camp && bulletDataRef.collisionFlags == 1 && (bulletDataRef.fireMethodFlags & 1) != 0, "The emitted currency projectile must retain FatGold's real SHOOTER/camp/collision configuration.");
		bool flag2 = bulletDataRef.config?.NameSN.ToString() == fatGold.projectileName;
		if (flag2)
		{
			bool flag3;
			switch (fatGold.projectileName)
			{
			case "CoinSilver":
			case "CoinGold":
			case "CoinDiamond":
				flag3 = true;
				break;
			default:
				flag3 = false;
				break;
			}
			flag2 = flag3;
		}
		Check(flag2, $"FatGold must emit one of its real currency configs; projectile={fatGold.projectileName}, config={bulletDataRef.config?.NameSN}.");
		Check(!GodotObject.IsInstanceValid(bulletDataRef.target), "FatGold's straight currency projectile must not carry a TRACK target.");
		double hitpoints = trashBin.instance.hitpoints;
		bulletDataRef.pos = new Vector2(trashBin.GlobalPosition.X, (float)((double)trashBin.GlobalPosition.Y - bulletDataRef.height));
		bulletField.Update(0.0, Engine.GetPhysicsFrames() + 101);
		Check(bulletField.IsBulletActive(lastSpawnedIndex), "FatGold's currency projectile must pass through the real TrashBin instead of being consumed by it.");
		Check(Math.Abs(trashBin.instance.hitpoints - hitpoints) < 0.001, $"FatGold's currency projectile must not damage TrashBin; hp={trashBin.instance.hitpoints}/{hitpoints}.");
		if (bulletField.IsBulletActive(lastSpawnedIndex))
		{
			ref BulletData bulletDataRef2 = ref bulletField.GetBulletDataRef(lastSpawnedIndex);
			double hitpoints2 = plant.instance.hitpoints;
			bulletDataRef2.pos = new Vector2(plant.GlobalPosition.X, (float)((double)plant.GlobalPosition.Y - bulletDataRef2.height));
			bulletField.Update(0.0, Engine.GetPhysicsFrames() + 102);
			Check(!bulletField.IsBulletActive(lastSpawnedIndex), "After passing TrashBin, FatGold's currency projectile must still be consumed by the real plant target.");
			Check(plant.instance.hitpoints < hitpoints2, $"After passing TrashBin, FatGold's currency projectile must damage the plant; hp={plant.instance.hitpoints}/{hitpoints2}.");
		}
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

	private static void PlaceCharacter(TowerDefenseCharacter character, Vector2I gridPos)
	{
		character.Position = GridBegin + new Vector2((float)(gridPos.X - 1) * GridSize.X, (float)(gridPos.Y - 1) * GridSize.Y);
		character.gridPos = gridPos;
		character.inGame = true;
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
			GD.PushError("[BugOverviewFatGoldTrashBinTrackingRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyRealFatGoldTracking, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "bulletField", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "fatGold", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "trashBin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlaceCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.VerifyRealFatGoldTracking && args.Count == 5)
		{
			VerifyRealFatGoldTracking(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<BulletField>(in args[1]), VariantUtils.ConvertTo<TowerDefenseZombieFatGold>(in args[2]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[3]), VariantUtils.ConvertTo<TowerDefenseGravestone>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlaceCharacter && args.Count == 2)
		{
			PlaceCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
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
		if (method == MethodName.PlaceCharacter && args.Count == 2)
		{
			PlaceCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
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
		if (method == MethodName.VerifyRealFatGoldTracking)
		{
			return true;
		}
		if (method == MethodName.PlaceCharacter)
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
