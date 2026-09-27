using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ThreePeaterLaneDamageRuntimeTest.cs")]
public class ThreePeaterLaneDamageRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private const string ThreePeaterScenePath = "res://Asset/Anime/Character/Plant/Chapter0/ThreePeater/Scene/TowerDefensePlantThreePeater.tscn";

	private const string NormalZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string PeaConfigPath = "res://Asset/Config/Projectile/Pea/PeaDefault.tres";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		ResourceManager resources = ResourceManager.Instance;
		ProjectileUpdateManager projectileManager = ProjectileUpdateManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		ProcessModeEnum previousProjectileProcessMode = projectileManager?.ProcessMode ?? ProcessModeEnum.Inherit;
		Resource previousPeaConfig = null;
		bool hadPeaConfig = resources?.PROJECTILE_CONFIG.TryGetValue("Pea", out previousPeaConfig) ?? false;
		ThreePeaterLaneDamageControlStub control = new ThreePeaterLaneDamageControlStub
		{
			Name = "ThreePeaterLaneDamageControl",
			isGameRunning = false,
			isInit = true,
			levelConfig = new TowerDefenseLevelConfig()
		};
		Node2D node2D = new Node2D
		{
			Name = "CharacterNode"
		};
		BulletField field = null;
		TowerDefenseCharacter attacker = null;
		TowerDefenseCharacter[] targets = new TowerDefenseCharacter[3];
		double[] damageTaken = new double[3];
		try
		{
			Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
			Check(GodotObject.IsInstanceValid(resources), "ResourceManager autoload must be available.");
			if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(resources))
			{
				throw new InvalidOperationException("Required autoloads are unavailable.");
			}
			AddChild(control, forceReadableName: false, InternalMode.Disabled);
			control.characterNode = node2D;
			control.AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
			manager.currentControl = control;
			manager.gridBeginPos = new Vector2(0f, 100f);
			manager.gridSize = new Vector2(100f, 76f);
			manager.gridNum = new Vector2I(9, 5);
			resources.PROJECTILE_CONFIG["Pea"] = ResourceLoader.Load<TowerDefenseProjectileConfig>("res://Asset/Config/Projectile/Pea/PeaDefault.tres", null, ResourceLoader.CacheMode.Ignore);
			if (GodotObject.IsInstanceValid(projectileManager))
			{
				projectileManager.ProcessMode = ProcessModeEnum.Disabled;
			}
			field = new BulletField
			{
				Name = "ThreePeaterLaneDamageBulletField"
			};
			node2D.AddChild(field, forceReadableName: false, InternalMode.Disabled);
			Check(GodotObject.IsInstanceValid(field) && BulletField.Instance == field, "The dedicated BulletField must be mounted.");
			PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/ThreePeater/Scene/TowerDefensePlantThreePeater.tscn", null, ResourceLoader.CacheMode.Ignore);
			PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", null, ResourceLoader.CacheMode.Ignore);
			attacker = packedScene?.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
			Check(GodotObject.IsInstanceValid(attacker), "The real ThreePeater attacker must instantiate.");
			if (!GodotObject.IsInstanceValid(attacker) || packedScene2 == null)
			{
				throw new InvalidOperationException("ThreePeater combat fixtures are unavailable.");
			}
			attacker.Position = new Vector2(100f, 252f);
			attacker.gridPos = new Vector2I(1, 3);
			attacker.inGame = true;
			node2D.AddChild(attacker, forceReadableName: false, InternalMode.Disabled);
			Vector2[] array = new Vector2[3]
			{
				new Vector2(320f, 176f),
				new Vector2(150f, 252f),
				new Vector2(320f, 328f)
			};
			int[] targetRows = new int[3] { 2, 3, 4 };
			for (int i = 0; i < targets.Length; i++)
			{
				targets[i] = packedScene2.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(targets[i]), $"Row {targetRows[i]} zombie must instantiate.");
				if (!GodotObject.IsInstanceValid(targets[i]))
				{
					throw new InvalidOperationException($"Row {targetRows[i]} target is unavailable.");
				}
				targets[i].Position = array[i];
				targets[i].gridPos = new Vector2I((i == 1) ? 2 : 3, targetRows[i]);
				targets[i].inGame = true;
				node2D.AddChild(targets[i], forceReadableName: false, InternalMode.Disabled);
			}
			await WaitFrames(4);
			attacker.ProcessMode = ProcessModeEnum.Disabled;
			for (int j = 0; j < targets.Length; j++)
			{
				targets[j].ProcessMode = ProcessModeEnum.Disabled;
			}
			FireComponent fireComponent = attacker.componentManager?.GetRuntime<FireComponent>("character.fire");
			Check(fireComponent != null && !fireComponent.IsReleased, "The real ThreePeater FireComponent must be active.");
			if (fireComponent == null || fireComponent.IsReleased)
			{
				throw new InvalidOperationException("ThreePeater FireComponent is unavailable.");
			}
			List<int> spawnedIndices = new List<int>(3);
			field.OnBulletSpawned += CaptureSpawn;
			for (int k = 0; k < fireComponent.fireProjectileList.Count; k++)
			{
				FireComponentFireProjectileConfig fireComponentFireProjectileConfig = fireComponent.fireProjectileList[k];
				FireComponentCheckConfig fireComponentCheckConfig = fireComponent.fireCheckList[fireComponentFireProjectileConfig.checkProjectileId];
				Vector2 velocity = fireComponentFireProjectileConfig.speed * Vector2.FromAngle(Mathf.DegToRad(fireComponentFireProjectileConfig.dir));
				fireComponent.CreateProjectile(fireComponentFireProjectileConfig.firePosId, velocity, fireComponentCheckConfig.GetProjectile(), fireComponentCheckConfig.GetCollisionFlags(), attacker.camp, Vector2.Zero, fireComponentFireProjectileConfig.offsetLine, filterByLine: true);
			}
			field.OnBulletSpawned -= CaptureSpawn;
			Check(spawnedIndices.Count == 3, $"ThreePeater must spawn three lane projectiles, got {spawnedIndices.Count}.");
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int l = 0; l < spawnedIndices.Count; l++)
			{
				int num = spawnedIndices[l];
				if (field.IsBulletActive(num))
				{
					ref BulletData bulletDataRef = ref field.GetBulletDataRef(num);
					dictionary[bulletDataRef.gridY] = num;
					Check(!bulletDataRef.checkAll, $"Row {bulletDataRef.gridY} projectile must remain lane-scoped.");
					Check(bulletDataRef.lockGridY, $"Row {bulletDataRef.gridY} projectile must lock its authored lane.");
				}
			}
			Check(dictionary.ContainsKey(2) && dictionary.ContainsKey(3) && dictionary.ContainsKey(4), "ThreePeater projectiles must bind to rows 2, 3, and 4.");
			double[] array2 = new double[targets.Length];
			int[] hurtSignals = new int[targets.Length];
			for (int m = 0; m < targets.Length; m++)
			{
				array2[m] = targets[m].instance.hitpoints;
				int capturedIndex = m;
				targets[m].OnBodyHurt += (int _) =>
				{
					hurtSignals[capturedIndex]++;
				};
			}
			field.Update(1.0 / 60.0, 1uL);
			Check(dictionary.TryGetValue(2, out var value) && field.IsBulletActive(value) && field.GetBulletDataRef(value).gridY == 2, "Upper projectile must keep row 2 during its Y-offset transition.");
			Check(dictionary.TryGetValue(4, out var value2) && field.IsBulletActive(value2) && field.GetBulletDataRef(value2).gridY == 4, "Lower projectile must keep row 4 during its Y-offset transition.");
			for (ulong num2 = 2uL; num2 <= 180; num2++)
			{
				if (field.ActiveCount <= 0)
				{
					break;
				}
				field.Update(1.0 / 60.0, num2);
			}
			for (int num3 = 0; num3 < targets.Length; num3++)
			{
				damageTaken[num3] = array2[num3] - targets[num3].instance.hitpoints;
				Check(hurtSignals[num3] == 1 && damageTaken[num3] > 0.0, $"Row {targetRows[num3]} target must take exactly one projectile hit; signals={hurtSignals[num3]} damage={damageTaken[num3]:F1}.");
			}
			void CaptureSpawn(int index)
			{
				spawnedIndices.Add(index);
			}
		}
		catch (Exception value3)
		{
			_failures++;
			GD.PushError($"[ThreePeaterLaneDamageRuntimeTest] Unexpected exception: {value3}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(field))
			{
				field.ClearActiveBullets();
			}
			if (hadPeaConfig)
			{
				resources.PROJECTILE_CONFIG["Pea"] = previousPeaConfig;
			}
			else
			{
				resources?.PROJECTILE_CONFIG.Remove("Pea");
			}
			if (GodotObject.IsInstanceValid(projectileManager))
			{
				projectileManager.ProcessMode = previousProjectileProcessMode;
			}
			if (GodotObject.IsInstanceValid(attacker))
			{
				attacker.QueueFree();
			}
			for (int num4 = 0; num4 < targets.Length; num4++)
			{
				if (GodotObject.IsInstanceValid(targets[num4]))
				{
					targets[num4].QueueFree();
				}
			}
			if (GodotObject.IsInstanceValid(field))
			{
				field.QueueFree();
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
			await WaitFrames(3);
		}
		bool flag = _failures == 0;
		GD.Print($"THREE_PEATER_LANE_DAMAGE_RESULT passed={flag} checks={_checks} failures={_failures} upperDamage={damageTaken[0]:F1} middleDamage={damageTaken[1]:F1} lowerDamage={damageTaken[2]:F1}");
		GetTree().Quit((!flag) ? 2 : 0);
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
			GD.PushError("[ThreePeaterLaneDamageRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
