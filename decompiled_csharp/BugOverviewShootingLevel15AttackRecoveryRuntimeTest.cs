using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewShootingLevel15AttackRecoveryRuntimeTest.cs")]
public class BugOverviewShootingLevel15AttackRecoveryRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private const string MarigoldGScenePath = "res://Asset/Anime/Character/Plant/Diamond/MarigoldG/Scene/TowerDefensePlantMarigoldG.tscn";

	private const string GroundZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private static readonly Vector2 ChessGridBegin = new Vector2(260f, 75f);

	private static readonly Vector2 ChessGridSize = new Vector2(80f, 98f);

	private static readonly Vector2I ChessGridNum = new Vector2I(9, 5);

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
		BugOverviewShootingLevel15AttackRecoveryControlStub control = null;
		BulletField bulletField = null;
		bool ownsBulletField = false;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00d8;
				}
				manager.gridBeginPos = ChessGridBegin;
				manager.gridSize = ChessGridSize;
				manager.gridNum = ChessGridNum;
				control = new BugOverviewShootingLevel15AttackRecoveryControlStub
				{
					Name = "ShootingLevel15MarigoldRecoveryControl",
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
				TowerDefenseProjectileRegistry.Init();
				bulletField = BulletField.Instance;
				if (!GodotObject.IsInstanceValid(bulletField))
				{
					bulletField = new BulletField
					{
						Name = "ShootingLevel15MarigoldBulletField"
					};
					AddChild(bulletField, forceReadableName: false, InternalMode.Disabled);
					ownsBulletField = true;
					await WaitFrames(1);
				}
				Check(GodotObject.IsInstanceValid(bulletField), "The live BulletField must be available for completed-attack checks.");
				if (!GodotObject.IsInstanceValid(bulletField))
				{
					goto end_IL_00d8;
				}
				projectileUpdateManager = ProjectileUpdateManager.Instance;
				if (GodotObject.IsInstanceValid(projectileUpdateManager))
				{
					previousProjectileProcessMode = projectileUpdateManager.ProcessMode;
					projectileUpdateManager.ProcessMode = ProcessModeEnum.Disabled;
				}
				bulletField.ClearActiveBullets();
				await RunMarigoldTargetRecovery(control, bulletField);
				goto end_IL_00c6;
				end_IL_00d8:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[ShootingLevel15MarigoldRecovery] Unexpected exception: {value}");
				goto end_IL_00c6;
			}
			return;
			end_IL_00c6:;
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
			await WaitFrames(2);
		}
		bool flag = _failures == 0;
		GD.Print($"BUG_OVERVIEW_I93_SHOOTING_LEVEL15_ATTACK_RECOVERY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task RunMarigoldTargetRecovery(BugOverviewShootingLevel15AttackRecoveryControlStub control, BulletField bulletField)
	{
		TowerDefensePlantMarigoldG marigold = null;
		TowerDefenseCharacter targetA = null;
		TowerDefenseCharacter targetB = null;
		try
		{
			marigold = Instantiate<TowerDefensePlantMarigoldG>("res://Asset/Anime/Character/Plant/Diamond/MarigoldG/Scene/TowerDefensePlantMarigoldG.tscn");
			Check(GodotObject.IsInstanceValid(marigold), "The real MarigoldG scene must instantiate.");
			if (GodotObject.IsInstanceValid(marigold))
			{
				PlaceCharacter(marigold, new Vector2I(1, 3));
				control.characterNode.AddChild(marigold, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				marigold.ProcessMode = ProcessModeEnum.Disabled;
				marigold.fireInterval = 1.6;
				marigold.fireNum = 10;
				marigold.canMowerMove = true;
				FireComponent fire = marigold.componentManager?.GetRuntime<FireComponent>("character.fire");
				Check(fire != null && !fire.IsReleased, "The real MarigoldG FireComponent must be active.");
				if (fire != null && !fire.IsReleased)
				{
					Check(fire.fireNum == 10 && Mathf.IsEqualApprox(fire.fireInterval, 1.6f) && marigold.canMowerMove, $"Shooting Level 1-15 overrides must be live; fireNum={fire.fireNum}, interval={fire.fireInterval}, mower={marigold.canMowerMove}.");
					Check(fire.fireCheckList.Count == 1 && fire.checkAllLine, "MarigoldG must retain its authored all-line weighted coin check.");
					targetA = Instantiate<TowerDefenseCharacter>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
					Check(GodotObject.IsInstanceValid(targetA), "Target A must instantiate from the real Normal zombie scene.");
					if (GodotObject.IsInstanceValid(targetA))
					{
						PlaceCharacter(targetA, new Vector2I(6, 3));
						control.isGameRunning = true;
						control.characterNode.AddChild(targetA, forceReadableName: false, InternalMode.Disabled);
						await WaitFrames(3);
						targetA.ProcessMode = ProcessModeEnum.Disabled;
						fire.groundRight = 10000f;
						fire.timer = 0f;
						fire.checkInterval = 0;
						TowerDefenseProjectileCreateData projetile = fire.fireCheckList[0].projectile.GetProjetile();
						Check(GodotObject.IsInstanceValid(projetile) && fire.CanFireCheckOnceByData(projetile), "MarigoldG must acquire target A through its real weighted coin check.");
						int beforeA = bulletField.ActiveCount;
						marigold.IdleProcessing(0.0);
						await WaitForVolleyCompletion();
						int activeCount = bulletField.ActiveCount;
						Check(activeCount - beforeA == 10, $"MarigoldG must complete its first Level 1-15 ten-coin attack; created={activeCount - beforeA}.");
						targetA.QueueFree();
						await WaitFrames(3);
						TowerDefenseProjectileCreateData projetile2 = fire.fireCheckList[0].projectile.GetProjetile();
						Check(!fire.CanFireCheckOnceByData(projetile2), "After target A leaves, MarigoldG must observe an empty target set instead of retaining A.");
						targetB = Instantiate<TowerDefenseCharacter>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
						Check(GodotObject.IsInstanceValid(targetB), "Target B must instantiate from the real Normal zombie scene.");
						if (GodotObject.IsInstanceValid(targetB))
						{
							PlaceCharacter(targetB, new Vector2I(7, 4));
							control.characterNode.AddChild(targetB, forceReadableName: false, InternalMode.Disabled);
							await WaitFrames(3);
							targetB.ProcessMode = ProcessModeEnum.Disabled;
							fire.timer = 0f;
							fire.checkInterval = 0;
							TowerDefenseProjectileCreateData projetile3 = fire.fireCheckList[0].projectile.GetProjetile();
							Check(GodotObject.IsInstanceValid(projetile3) && fire.CanFireCheckOnceByData(projetile3), "MarigoldG must reacquire target B on another row through checkAllLine.");
							int beforeB = bulletField.ActiveCount;
							marigold.IdleProcessing(0.0);
							await WaitForVolleyCompletion();
							int activeCount2 = bulletField.ActiveCount;
							Check(activeCount2 - beforeB == 10, $"After losing A, MarigoldG must complete a new ten-coin attack against B; created={activeCount2 - beforeB}.");
							Check(activeCount2 == 20, $"Both complete attacks must remain observable in the paused BulletField; active={activeCount2}.");
							return;
						}
						return;
					}
					return;
				}
				return;
			}
		}
		finally
		{
			control.isGameRunning = false;
			if (GodotObject.IsInstanceValid(marigold))
			{
				marigold.QueueFree();
			}
			if (GodotObject.IsInstanceValid(targetA))
			{
				targetA.QueueFree();
			}
			if (GodotObject.IsInstanceValid(targetB))
			{
				targetB.QueueFree();
			}
			await WaitFrames(3);
			bulletField.ClearActiveBullets();
		}
	}

	private async Task WaitForVolleyCompletion()
	{
		await ToSignal(GetTree().CreateTimer(1.25, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		await WaitFrames(2);
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
		character.Position = ChessGridBegin + new Vector2((float)(gridPos.X - 1) * ChessGridSize.X, (float)(gridPos.Y - 1) * ChessGridSize.Y);
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
			GD.PushError("[ShootingLevel15MarigoldRecovery] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
