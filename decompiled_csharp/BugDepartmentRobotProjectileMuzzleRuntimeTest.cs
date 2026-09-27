using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentRobotProjectileMuzzleRuntimeTest.cs")]
public class BugDepartmentRobotProjectileMuzzleRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetupBattleFixture = "SetupBattleFixture";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _control = "_control";

		public static readonly StringName _robot = "_robot";

		public static readonly StringName _bulletField = "_bulletField";

		public static readonly StringName _ownsBulletField = "_ownsBulletField";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string RobotScenePath = "res://Asset/Anime/Character/Plant/Star/Robot/Scene/TowerDefensePlantRobot.tscn";

	private const string PeaConfigPath = "res://Asset/Config/Projectile/Pea/PeaDefault.tres";

	private static readonly Vector2I ScenarioGrid = new Vector2I(4, 3);

	private int _checks;

	private int _failures;

	private RobotProjectileMuzzleControlStub _control;

	private TowerDefensePlantRobot _robot;

	private BulletField _bulletField;

	private bool _ownsBulletField;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		ProjectileUpdateManager projectileManager = ProjectileUpdateManager.Instance;
		ProcessModeEnum previousProjectileProcessMode = projectileManager?.ProcessMode ?? ProcessModeEnum.Inherit;
		ResourceManager resources = ResourceManager.Instance;
		Resource previousPea = null;
		bool hadPreviousPea = resources?.PROJECTILE_CONFIG.TryGetValue("Pea", out previousPea) ?? false;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(resources), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(resources))
				{
					throw new InvalidOperationException("Required autoloads are unavailable.");
				}
				SetupBattleFixture(manager);
				resources.PROJECTILE_CONFIG["Pea"] = ResourceLoader.Load<TowerDefenseProjectileConfig>("res://Asset/Config/Projectile/Pea/PeaDefault.tres", null, ResourceLoader.CacheMode.Ignore);
				if (GodotObject.IsInstanceValid(projectileManager))
				{
					projectileManager.ProcessMode = ProcessModeEnum.Disabled;
				}
				_bulletField = BulletField.Instance;
				if (!GodotObject.IsInstanceValid(_bulletField))
				{
					_bulletField = new BulletField
					{
						Name = "RobotProjectileMuzzleBulletField"
					};
					AddChild(_bulletField, forceReadableName: false, InternalMode.Disabled);
					_ownsBulletField = true;
					await WaitFrames(1);
				}
				Check(GodotObject.IsInstanceValid(_bulletField), "The real BulletField must be mounted for the Robot projectile scenario.");
				_robot = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Star/Robot/Scene/TowerDefensePlantRobot.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefensePlantRobot>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(_robot) && _robot.config?.name == "PlantRobot", "The fixture must instantiate the real Robot plant scene and config.");
				if (!GodotObject.IsInstanceValid(_robot) || !GodotObject.IsInstanceValid(_bulletField))
				{
					throw new InvalidOperationException("The real Robot or BulletField is unavailable.");
				}
				_robot.inGame = false;
				_robot.editorPreviewMode = true;
				_robot.gridPos = ScenarioGrid;
				_robot.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(ScenarioGrid);
				_control.characterNode.AddChild(_robot, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				_robot.ProcessMode = ProcessModeEnum.Disabled;
				FireComponent fireComponent = _robot.componentManager?.GetRuntime<FireComponent>("character.fire");
				Check(fireComponent != null && !fireComponent.IsReleased, "The real Robot must expose its resource-backed mode-A FireComponent.");
				if (fireComponent == null || fireComponent.IsReleased)
				{
					throw new InvalidOperationException("The real Robot FireComponent is unavailable.");
				}
				Marker2D marker2D = ((fireComponent.firePosMarker.Count > 0) ? fireComponent.firePosMarker[0] : null);
				Marker2D marker2D2 = ((fireComponent.firePosMarker.Count > 1) ? fireComponent.firePosMarker[1] : null);
				Check(fireComponent.firePosMarker.Count == 2 && GodotObject.IsInstanceValid(marker2D) && GodotObject.IsInstanceValid(marker2D2), "The real Robot mode-A scene must provide both authored projectile muzzle markers.");
				Check(marker2D.GlobalPosition.DistanceTo(marker2D2.GlobalPosition) > 1f, "The two animated Robot muzzles must resolve to distinct world positions.");
				_bulletField.ClearActiveBullets();
				List<int> spawnedIndices = new List<int>();
				_bulletField.OnBulletSpawned += CaptureBullet;
				fireComponent.runningCheck = fireComponent.fireCheckList[0];
				fireComponent.Fire();
				_bulletField.OnBulletSpawned -= CaptureBullet;
				Check(spawnedIndices.Count == 2, $"The real Robot mode-A fire event must create two Pea projectiles; got {spawnedIndices.Count}.");
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				foreach (int item in spawnedIndices)
				{
					if (_bulletField.IsBulletActive(item))
					{
						ref BulletData bulletDataRef = ref _bulletField.GetBulletDataRef(item);
						flag |= bulletDataRef.pos.DistanceTo(marker2D.GlobalPosition) <= 0.01f;
						flag2 |= bulletDataRef.pos.DistanceTo(marker2D2.GlobalPosition) <= 0.01f;
						flag3 |= bulletDataRef.pos.DistanceTo(_robot.GlobalPosition) <= 0.01f;
					}
				}
				Check(flag, "One real Robot Pea must originate at the first animated muzzle marker.");
				Check(flag2, "The other real Robot Pea must originate at the second animated muzzle marker.");
				Check(!flag3, "No Robot projectile may fall back to the plant root, which causes the reported visual offset.");
				void CaptureBullet(int index)
				{
					spawnedIndices.Add(index);
				}
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentRobotProjectileMuzzleRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(_bulletField))
			{
				_bulletField.ClearActiveBullets();
			}
			if (hadPreviousPea)
			{
				resources.PROJECTILE_CONFIG["Pea"] = previousPea;
			}
			else
			{
				resources?.PROJECTILE_CONFIG.Remove("Pea");
			}
			if (GodotObject.IsInstanceValid(projectileManager))
			{
				projectileManager.ProcessMode = previousProjectileProcessMode;
			}
			if (GodotObject.IsInstanceValid(_robot))
			{
				_robot.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (_ownsBulletField && GodotObject.IsInstanceValid(_bulletField))
			{
				_bulletField.QueueFree();
			}
			if (GodotObject.IsInstanceValid(_control))
			{
				_control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag4 = _failures == 0 && _checks == 11;
		GD.Print($"ROBOT_PROJECTILE_MUZZLE_RESULT passed={flag4} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag4) ? 2 : 0);
	}

	private void SetupBattleFixture(TowerDefenseManager manager)
	{
		_control = new RobotProjectileMuzzleControlStub
		{
			Name = "RobotProjectileMuzzleControl",
			isGameRunning = false,
			isInit = true,
			levelConfig = new TowerDefenseLevelConfig()
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_control.characterNode = new Node2D
		{
			Name = "CharacterNode"
		};
		_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
		manager.currentControl = _control;
		manager.gridBeginPos = new Vector2(100f, 100f);
		manager.gridSize = new Vector2(100f, 76f);
		manager.gridNum = new Vector2I(9, 5);
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
			GD.PushError("[BugDepartmentRobotProjectileMuzzleRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupBattleFixture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.SetupBattleFixture && args.Count == 1)
		{
			SetupBattleFixture(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]));
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.SetupBattleFixture)
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
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<RobotProjectileMuzzleControlStub>(in value);
			return true;
		}
		if (name == PropertyName._robot)
		{
			_robot = VariantUtils.ConvertTo<TowerDefensePlantRobot>(in value);
			return true;
		}
		if (name == PropertyName._bulletField)
		{
			_bulletField = VariantUtils.ConvertTo<BulletField>(in value);
			return true;
		}
		if (name == PropertyName._ownsBulletField)
		{
			_ownsBulletField = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
			return true;
		}
		if (name == PropertyName._robot)
		{
			value = VariantUtils.CreateFrom(in _robot);
			return true;
		}
		if (name == PropertyName._bulletField)
		{
			value = VariantUtils.CreateFrom(in _bulletField);
			return true;
		}
		if (name == PropertyName._ownsBulletField)
		{
			value = VariantUtils.CreateFrom(in _ownsBulletField);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._robot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bulletField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ownsBulletField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._robot, Variant.From(in _robot));
		info.AddProperty(PropertyName._bulletField, Variant.From(in _bulletField));
		info.AddProperty(PropertyName._ownsBulletField, Variant.From(in _ownsBulletField));
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
		if (info.TryGetProperty(PropertyName._control, out var value3))
		{
			_control = value3.As<RobotProjectileMuzzleControlStub>();
		}
		if (info.TryGetProperty(PropertyName._robot, out var value4))
		{
			_robot = value4.As<TowerDefensePlantRobot>();
		}
		if (info.TryGetProperty(PropertyName._bulletField, out var value5))
		{
			_bulletField = value5.As<BulletField>();
		}
		if (info.TryGetProperty(PropertyName._ownsBulletField, out var value6))
		{
			_ownsBulletField = value6.As<bool>();
		}
	}
}
