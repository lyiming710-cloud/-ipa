using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewWallnutSquashBowlingJumpRuntimeTest.cs")]
public class BugOverviewWallnutSquashBowlingJumpRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

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

	private const string BowlingScenePath = "res://Asset/Anime/Character/Plant/Chapter6/WallnutSquash/Scene/TowerDefensePlantBowlingWallnutSquash.tscn";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		WallnutSquashBowlingJumpRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefensePlantBowlingWallnutSquash bowlingPlant = null;
		TowerDefenseZombie zombie = null;
		SquashComponent squash = null;
		SquashComponent.JumpDownSmashEventHandler impactHandler = null;
		bool impactObserved = false;
		Vector2 squashImpactPosition = Vector2.Zero;
		Vector2 zombieImpactPosition = Vector2.Zero;
		double healthAtImpactStart = 0.0;
		bool impactStayedOutsideBattlefield = false;
		try
		{
			_ = 3;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					return;
				}
				control = new WallnutSquashBowlingJumpRuntimeControlStub
				{
					Name = "WallnutSquashBowlingJumpRuntimeControl",
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
				bowlingPlant = LoadCharacter<TowerDefensePlantBowlingWallnutSquash>("res://Asset/Anime/Character/Plant/Chapter6/WallnutSquash/Scene/TowerDefensePlantBowlingWallnutSquash.tscn");
				zombie = LoadCharacter<TowerDefenseZombie>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
				Check(GodotObject.IsInstanceValid(bowlingPlant) && bowlingPlant.config?.name == "PlantWallnutSquashBowling" && GodotObject.IsInstanceValid(zombie) && zombie.config?.name == "ZombieNormal", "The fixture must use the real Bowling Wallnut Squash and normal zombie scenes.");
				if (!GodotObject.IsInstanceValid(bowlingPlant) || !GodotObject.IsInstanceValid(zombie))
				{
					return;
				}
				bowlingPlant.editorPreviewMode = false;
				bowlingPlant.inGame = false;
				bowlingPlant.gridPos = new Vector2I(3, 3);
				bowlingPlant.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(bowlingPlant.gridPos);
				zombie.editorPreviewMode = false;
				zombie.inGame = true;
				zombie.gridPos = new Vector2I(4, 3);
				zombie.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(zombie.gridPos);
				control.characterNode.AddChild(bowlingPlant, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(6);
				BowlingComponent bowling = bowlingPlant.componentManager?.GetRuntime<BowlingComponent>();
				squash = bowlingPlant.componentManager?.GetRuntime<SquashComponent>();
				CharacterMoveComponent movement = bowlingPlant.componentManager?.GetRuntime<CharacterMoveComponent>();
				GroundMoveComponent zombieGroundMove = zombie.componentManager?.GetRuntime<GroundMoveComponent>();
				BugOverviewWallnutSquashBowlingJumpRuntimeTest bugOverviewWallnutSquashBowlingJumpRuntimeTest = this;
				int condition;
				if (bowling != null && !bowling.IsReleased)
				{
					SquashComponent squashComponent = squash;
					if (squashComponent != null && !squashComponent.IsReleased)
					{
						if (movement != null && !movement.IsReleased)
						{
							if (zombieGroundMove != null && !zombieGroundMove.IsReleased && zombieGroundMove.HasMovementSource)
							{
								condition = (squash.trackMovingTargetUntilImpact ? 1 : 0);
								goto IL_05dd;
							}
						}
					}
				}
				condition = 0;
				goto IL_05dd;
				IL_05dd:
				bugOverviewWallnutSquashBowlingJumpRuntimeTest.Check((byte)condition != 0, "The real Bowling, Squash, plant movement, and ZombieNormal GroundMove runtimes must be active.");
				if ((bowling?.IsReleased ?? true) || (squash?.IsReleased ?? true) || (movement?.IsReleased ?? true) || (zombieGroundMove?.IsReleased ?? true) || !zombieGroundMove.HasMovementSource || !squash.trackMovingTargetUntilImpact)
				{
					goto end_IL_014f;
				}
				zombie.instance.hitpointsBase = 10000.0;
				zombie.instance.hitpointsSave = 10000.0;
				zombie.instance.hitpoints = 10000.0;
				control.isGameRunning = true;
				zombie.Walk();
				float zombieWalkStartX = zombie.GlobalPosition.X;
				Vector2 previousGroundPose = zombieGroundMove.groundPosSave;
				bool observedPreImpactGroundPoseAdvance = false;
				for (int frame = 0; frame < 90; frame++)
				{
					await WaitFrames(1);
					if (zombieGroundMove.groundPosInit && !zombieGroundMove.groundPosSave.IsEqualApprox(previousGroundPose))
					{
						observedPreImpactGroundPoseAdvance = true;
					}
					previousGroundPose = zombieGroundMove.groundPosSave;
					if (observedPreImpactGroundPoseAdvance && zombie.GlobalPosition.X < zombieWalkStartX - 1f)
					{
						break;
					}
				}
				Check((zombieGroundMove.Alive & observedPreImpactGroundPoseAdvance) && zombie.GlobalPosition.X < zombieWalkStartX - 1f, "The real ZombieNormal must walk left through its authored GroundSlot before collision.");
				float num = (float)(manager.GetMapGroundRight() + (double)manager.GetMapGridSize().X);
				zombie.GlobalPosition = new Vector2(num, zombie.GlobalPosition.Y);
				bowlingPlant.inGame = true;
				bowlingPlant.GlobalPosition = new Vector2(num - 36f, zombie.GlobalPosition.Y);
				Check(!bowlingPlant.IsInsideComponentBattlefield && !zombie.IsInsideComponentBattlefield, "The reported collision must begin to the right of the component battlefield boundary.");
				bowling.SetAlive(alive: true);
				bowling.isRoll = true;
				movement.velocity = new Vector2(200f, 0f);
				Check(bowling.Alive && bowling.isRoll && movement.velocity.X > 0f, "The regression must begin from a live, horizontally rolling state.");
				impactHandler = () =>
				{
					impactObserved = true;
					squashImpactPosition = bowlingPlant.GlobalPosition;
					zombieImpactPosition = zombie.GlobalPosition;
					healthAtImpactStart = zombie.instance.hitpoints;
					impactStayedOutsideBattlefield = !bowlingPlant.IsInsideComponentBattlefield && !zombie.IsInsideComponentBattlefield;
				};
				squash.OnJumpDownSmash += impactHandler;
				double healthBefore = zombie.instance.hitpoints;
				double groundHeight = bowlingPlant.groundHeight;
				Vector2 startPosition = bowlingPlant.GlobalPosition;
				Vector2 groundPoseAtCollision = zombieGroundMove.groundPosSave;
				float zombiePositionAtCollision = zombie.GlobalPosition.X;
				bowling.BowlingHit(zombie);
				Check(zombie.instance.hitpoints < healthBefore, "The real bowling collision must still deliver its hit payload.");
				Check(!bowling.Alive && !bowling.isRoll, "The collision must stop the Bowling runtime before Squash takes ownership.");
				Check(movement.velocity.IsZeroApprox(), "The collision must clear horizontal rolling velocity.");
				Check(squash.IsRunning() && squash.target == zombie, "The collided real zombie must be handed to the live Squash runtime.");
				double maxHeight = bowlingPlant.z;
				float maxHorizontalTravel = 0f;
				bool reachedJumpState = false;
				bool observedAirborneGroundFlag = false;
				bool groundMoveStayedActive = true;
				bool observedGroundPoseAdvanceDuringSquash = false;
				float maxZombieTravel = 0f;
				int framesAfterImpact = 0;
				for (int frame = 0; frame < 100; frame++)
				{
					if (!GodotObject.IsInstanceValid(bowlingPlant))
					{
						break;
					}
					await WaitFrames(1);
					if (!GodotObject.IsInstanceValid(bowlingPlant))
					{
						break;
					}
					if (!impactObserved)
					{
						groundMoveStayedActive &= zombieGroundMove.Alive;
					}
					observedGroundPoseAdvanceDuringSquash |= zombieGroundMove.groundPosInit && !zombieGroundMove.groundPosSave.IsEqualApprox(groundPoseAtCollision);
					maxZombieTravel = Math.Max(maxZombieTravel, Mathf.Abs(zombie.GlobalPosition.X - zombiePositionAtCollision));
					maxHeight = Math.Max(maxHeight, bowlingPlant.z);
					maxHorizontalTravel = Math.Max(maxHorizontalTravel, Mathf.Abs(bowlingPlant.GlobalPosition.X - startPosition.X));
					reachedJumpState |= squash.StateMachine?.CurrentStateHandle?.StableId == "squash.jump";
					observedAirborneGroundFlag |= !bowlingPlant.isGround;
					if (impactObserved)
					{
						int num2 = framesAfterImpact + 1;
						framesAfterImpact = num2;
						if (num2 >= 4)
						{
							break;
						}
					}
				}
				Check(reachedJumpState, "The real Squash state machine must leave Ready and enter squash.jump.");
				Check(maxHeight > groundHeight + 10.0, $"The bowling plant must visibly rise above ground instead of only translating; maxZ={maxHeight:F3}, ground={groundHeight:F3}.");
				Check(observedAirborneGroundFlag, "The jump must publish an airborne ground-state interval.");
				Check(maxHorizontalTravel > 1f, "The authored squash jump must still travel toward the collided zombie.");
				Check(maxHeight - groundHeight > (double)maxHorizontalTravel * 0.1, "The observed motion must contain a material vertical jump, not a horizontal-only slide.");
				Check(maxZombieTravel > 1f, "The real ZombieNormal must keep changing world position throughout collision and chase.");
				Check(groundMoveStayedActive & observedGroundPoseAdvanceDuringSquash, "ZombieNormal's real GroundMoveComponent must remain active and advance authored GroundSlot poses during the Squash chase.");
				Check(impactObserved, "The real Squash runtime must reach its authored jump-down smash callback.");
				Check(impactObserved & impactStayedOutsideBattlefield, "The Squash ready/jump lifecycle must complete while the collided zombie is still right of the battlefield.");
				Check(impactObserved && squashImpactPosition.X <= zombieImpactPosition.X + 0.5f && Mathf.Abs(squashImpactPosition.X - zombieImpactPosition.X) <= 0.5f && Mathf.Abs(squashImpactPosition.Y - zombieImpactPosition.Y) <= 0.5f, $"The Bowling Wallnut Squash landing point must not pass behind the moving zombie; squash={squashImpactPosition}, zombie={zombieImpactPosition}.");
				Check(impactObserved && healthAtImpactStart < healthBefore && zombie.instance.hitpoints < healthAtImpactStart, "The aligned landing must execute the real deferred smash payload against the moving ZombieNormal.");
				Check(await WaitUntilInvalid(bowlingPlant, 120), "The right-off-field impact visual must finish cleanup instead of remaining as an inert sprite.");
				goto end_IL_0134;
				end_IL_014f:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewWallnutSquashBowlingJumpRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0134;
			}
			return;
			end_IL_0134:;
		}
		finally
		{
			if (squash != null && impactHandler != null)
			{
				squash.OnJumpDownSmash -= impactHandler;
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
			if (GodotObject.IsInstanceValid(bowlingPlant))
			{
				bowlingPlant.QueueFree();
			}
			if (GodotObject.IsInstanceValid(zombie))
			{
				zombie.QueueFree();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag = _failures == 0;
		GD.Print($"WALLNUT_SQUASH_BOWLING_JUMP_RESULT passed={flag} checks={_checks} failures={_failures}");
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

	private static T LoadCharacter<T>(string path) where T : TowerDefenseCharacter
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private async Task<bool> WaitUntilInvalid(GodotObject value, int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (!GodotObject.IsInstanceValid(value))
			{
				return true;
			}
			await WaitFrames(1);
		}
		return !GodotObject.IsInstanceValid(value);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewWallnutSquashBowlingJumpRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
