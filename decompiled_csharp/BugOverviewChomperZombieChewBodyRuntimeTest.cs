using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewChomperZombieChewBodyRuntimeTest.cs")]
public class BugOverviewChomperZombieChewBodyRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName IsWalkClip = "IsWalkClip";

		public static readonly StringName IsIdleClip = "IsIdleClip";

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

	private const string ChomperZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Chomper/TowerDefenseZombieNormalChomper.tscn";

	private const string SpringFumeShroomScenePath = "res://Asset/Anime/Character/Plant/Chapter6/SpringFumeShroom/Scene/TowerDefensePlantSpringFumeShroom.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BugOverviewChomperZombieChewBodyRuntimeControlStub control = null;
		TowerDefenseZombieNormalChomper zombie = null;
		try
		{
			_ = 9;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00fd;
				}
				control = new BugOverviewChomperZombieChewBodyRuntimeControlStub
				{
					Name = "ChomperZombieChewBodyRuntimeControl",
					isGameRunning = true,
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
				zombie = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Chomper/TowerDefenseZombieNormalChomper.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombieNormalChomper>(PackedScene.GenEditState.Disabled);
				TowerDefensePlantSpringFumeShroom springFumeShroom = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter6/SpringFumeShroom/Scene/TowerDefensePlantSpringFumeShroom.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefensePlantSpringFumeShroom>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(zombie) && zombie.config?.name == "ZombieNormalChomper" && GodotObject.IsInstanceValid(springFumeShroom) && springFumeShroom.config?.name == "PlantSpringFumeShroom", "The fixture must instantiate the real Chomper Zombie and QQ Spring Fumeshroom scenes.");
				if (!GodotObject.IsInstanceValid(zombie) || !GodotObject.IsInstanceValid(springFumeShroom))
				{
					goto end_IL_00fd;
				}
				control.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(springFumeShroom, forceReadableName: false, InternalMode.Disabled);
				zombie.inGame = true;
				zombie.editorPreviewMode = false;
				zombie.gridPos = new Vector2I(5, 2);
				zombie.GlobalPosition = new Vector2(500f, 152f);
				zombie.groundRight = 10000.0;
				springFumeShroom.inGame = true;
				springFumeShroom.editorPreviewMode = false;
				springFumeShroom.gridPos = new Vector2I(2, 2);
				springFumeShroom.GlobalPosition = new Vector2(200f, 152f);
				await WaitFrames(6);
				BugOverviewChomperZombieChewBodyRuntimeTest bugOverviewChomperZombieChewBodyRuntimeTest = this;
				ChomperComponent chomperComponent = zombie.chomperComponent;
				int condition;
				if (chomperComponent != null && !chomperComponent.IsReleased)
				{
					GroundMoveComponent groundMoveComponent = zombie.groundMoveComponent;
					condition = ((groundMoveComponent != null && !groundMoveComponent.IsReleased) ? 1 : 0);
				}
				else
				{
					condition = 0;
				}
				bugOverviewChomperZombieChewBodyRuntimeTest.Check((byte)condition != 0, "The real Chomper and GroundMove runtimes must be active.");
				Check(GodotObject.IsInstanceValid(zombie.sprite) && GodotObject.IsInstanceValid(zombie.chomperComponent?.sprite), "The real body and inserted plant-head sprites must be available.");
				chomperComponent = zombie.chomperComponent;
				if (chomperComponent == null || chomperComponent.IsReleased || !GodotObject.IsInstanceValid(zombie.sprite))
				{
					goto end_IL_00fd;
				}
				zombie.Walk();
				await WaitFrames(8);
				Check(IsWalkClip(zombie.sprite.clip), "The control phase must begin in the authored walking body clip; clip=" + zombie.sprite.clip + ".");
				Check(zombie.groundMoveComponent?.Alive ?? false, "Walking must enable the real animation-driven ground movement runtime.");
				zombie.Component();
				zombie.chomperComponent.currentChewTime = 30f;
				zombie.chomperComponent.SendStateEvent("ToChew");
				await WaitFrames(8);
				Vector2 chewPosition = zombie.GlobalPosition;
				Check(zombie.componentRunning, "The zombie must remain in its component state while the plant head chews.");
				Check(zombie.chomperComponent.isChew, "The real Chomper runtime must enter its chew state.");
				Check(zombie.chomperComponent.sprite.clip == zombie.chomperComponent.chewAnimeClips, "The inserted plant head must play its authored chew clip; clip=" + zombie.chomperComponent.sprite.clip + ".");
				Check(!IsWalkClip(zombie.sprite.clip) && IsIdleClip(zombie.sprite.clip), "The zombie body must leave Walk and use an idle clip while chewing; clip=" + zombie.sprite.clip + ".");
				BugOverviewChomperZombieChewBodyRuntimeTest bugOverviewChomperZombieChewBodyRuntimeTest2 = this;
				GroundMoveComponent groundMoveComponent2 = zombie.groundMoveComponent;
				bugOverviewChomperZombieChewBodyRuntimeTest2.Check(groundMoveComponent2 != null && !groundMoveComponent2.Alive, "Ground movement must stay disabled throughout chewing.");
				await WaitFrames(20);
				Check(zombie.chomperComponent.isChew && zombie.chomperComponent.sprite.clip == zombie.chomperComponent.chewAnimeClips, "The plant head must still be chewing during the observation window.");
				Check(!IsWalkClip(zombie.sprite.clip), "The body must not resume a walking animation midway through chewing.");
				Check(zombie.GlobalPosition.DistanceTo(chewPosition) < 0.05f, $"The chewing zombie must remain stationary; start={chewPosition}, end={zombie.GlobalPosition}.");
				float insideX = (float)(manager.GetMapGroundRight() - (double)manager.GetMapGridSize().X * 0.6);
				zombie.GlobalPosition = new Vector2(insideX, zombie.GlobalPosition.Y);
				Check(zombie.IsInsideComponentBattlefield, "The reported QQ Spring Fumeshroom push must begin inside the component battlefield.");
				springFumeShroom.Hit(zombie);
				Check(await WaitUntil(() => !zombie.IsInsideComponentBattlefield, 180), $"The real QQ Spring Fumeshroom must push the chewing zombie outside; x={zombie.GlobalPosition.X}, boundary={manager.GetMapGroundRight()}.");
				await WaitFrames(8);
				Check(zombie.CurrentStateHandle?.StableId == "zombie.walk" && !zombie.componentRunning, "A Chomper Zombie pushed outside must leave the blocking Component state; state=" + zombie.CurrentStateHandle?.StableId + ".");
				Check(zombie.chomperComponent.StateMachine?.CurrentStateHandle?.StableId == "chomper.idle" && !zombie.chomperComponent.isChew, "The interrupted digestion must cleanly return the Chomper runtime to Idle; state=" + zombie.chomperComponent.StateMachine?.CurrentStateHandle?.StableId + ".");
				Check((zombie.groundMoveComponent?.Alive ?? false) && IsWalkClip(zombie.sprite.clip), $"Off-field recovery must restore the walking body and movement runtime; clip={zombie.sprite.clip}, moveAlive={zombie.groundMoveComponent?.Alive}.");
				await WaitFrames(150);
				float movementStartX = zombie.GlobalPosition.X;
				await WaitFrames(30);
				Check(zombie.GlobalPosition.X < movementStartX - 0.05f, $"After the spring tween ends, the recovered zombie must move forward again; startX={movementStartX}, endX={zombie.GlobalPosition.X}.");
				zombie.GlobalPosition = new Vector2(insideX, zombie.GlobalPosition.Y);
				zombie.Component();
				zombie.chomperComponent.currentChewTime = 30f;
				zombie.chomperComponent.SendStateEvent("ToChew");
				await WaitFrames(6);
				Check(zombie.componentRunning && zombie.chomperComponent.isChew, "The same live Chomper Zombie must be able to enter a second digestion cycle.");
				springFumeShroom.Hit(zombie);
				Check(await WaitUntil(() => !zombie.IsInsideComponentBattlefield && zombie.CurrentStateHandle?.StableId == "zombie.walk" && zombie.chomperComponent.StateMachine?.CurrentStateHandle?.StableId == "chomper.idle" && (zombie.groundMoveComponent?.Alive ?? false), 180) && (zombie.groundMoveComponent?.Alive ?? false) && !zombie.chomperComponent.isChew, $"Repeated QQ Spring Fumeshroom pushes must not leave stale digestion or movement state; main={zombie.CurrentStateHandle?.StableId}, chomper={zombie.chomperComponent.StateMachine?.CurrentStateHandle?.StableId}, moveAlive={zombie.groundMoveComponent?.Alive}.");
				goto end_IL_00c9;
				end_IL_00fd:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[ChomperZombieChewBody] Unexpected exception: {value}");
				goto end_IL_00c9;
			}
			return;
			end_IL_00c9:;
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
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 22;
		GD.Print($"CHOMPER_ZOMBIE_CHEW_BODY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static bool IsWalkClip(string clip)
	{
		if (!string.IsNullOrEmpty(clip))
		{
			return clip.StartsWith("Walk", StringComparison.Ordinal);
		}
		return false;
	}

	private static bool IsIdleClip(string clip)
	{
		if (!string.IsNullOrEmpty(clip))
		{
			return clip.StartsWith("Idle", StringComparison.Ordinal);
		}
		return false;
	}

	private async Task<bool> WaitUntil(Func<bool> condition, int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (condition())
			{
				return true;
			}
			await WaitFrames(1);
		}
		return condition();
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
			GD.PushError("[ChomperZombieChewBody] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsWalkClip, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsIdleClip, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IsWalkClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsWalkClip(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsIdleClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIdleClip(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.IsWalkClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsWalkClip(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsIdleClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIdleClip(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.IsWalkClip)
		{
			return true;
		}
		if (method == MethodName.IsIdleClip)
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
