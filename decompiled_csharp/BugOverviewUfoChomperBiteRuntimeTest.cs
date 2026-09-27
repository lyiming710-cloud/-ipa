using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewUfoChomperBiteRuntimeTest.cs")]
public class BugOverviewUfoChomperBiteRuntimeTest : Node
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

	private const string ChomperScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Chomper/Scene/TowerDefensePlantChomper.tscn";

	private const string UfoScenePath = "res://Asset/Anime/Character/Zombie/Challenge/Ufo/Scene/TowerDefenseZombieUfo.tscn";

	private const double ExpectedBiteDamage = 100.0;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		BugOverviewUfoChomperBiteControlStub control = null;
		TowerDefensePlantChomper chomper = null;
		TowerDefenseZombieUfo ufo = null;
		try
		{
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_0068;
				}
				control = new BugOverviewUfoChomperBiteControlStub
				{
					Name = "UfoChomperControl",
					isGameRunning = true,
					isInit = false
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				chomper = LoadCharacter<TowerDefensePlantChomper>("res://Asset/Anime/Character/Plant/Chapter0/Chomper/Scene/TowerDefensePlantChomper.tscn");
				ufo = LoadCharacter<TowerDefenseZombieUfo>("res://Asset/Anime/Character/Zombie/Challenge/Ufo/Scene/TowerDefenseZombieUfo.tscn");
				Check(GodotObject.IsInstanceValid(chomper) && chomper.config?.name == "PlantChomper", "The fixture must use the real Chomper scene.");
				Check(GodotObject.IsInstanceValid(ufo) && ufo.config?.name == "ZombieUfo", "The reported alien spaceship must resolve to the real ZombieUfo scene.");
				if (!GodotObject.IsInstanceValid(chomper) || !GodotObject.IsInstanceValid(ufo))
				{
					goto end_IL_0068;
				}
				Check(ufo.config is TowerDefenseZombieConfig { physique: TowerDefenseEnum.ZOMBIE_PHYSIQUE.CAR } towerDefenseZombieConfig && (towerDefenseZombieConfig.collisionFlags & 2) != 0, "ZombieUfo must retain its authored flying-vehicle identity.");
				Check(Math.Abs(ufo.config.biteHurt - 100.0) < 0.001, $"ZombieUfo must author a finite Chomper bite response instead of the default swallow sentinel; biteHurt={ufo.config.biteHurt}.");
				chomper.editorPreviewMode = true;
				ufo.editorPreviewMode = true;
				chomper.inGame = false;
				ufo.inGame = false;
				chomper.gridPos = new Vector2I(2, 2);
				ufo.gridPos = new Vector2I(3, 2);
				chomper.SetLogicalGlobalPosition(new Vector2(200f, 200f));
				ufo.SetLogicalGlobalPosition(new Vector2(280f, 200f));
				control.characterNode.AddChild(chomper, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(ufo, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				ChomperComponent chomperComponent = chomper.componentManager?.GetRuntime<ChomperComponent>();
				AttackComponent attackComponent = chomper.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
				Check(chomperComponent != null && !chomperComponent.IsReleased && attackComponent != null && !attackComponent.IsReleased, "The real Chomper bite runtimes must be active.");
				BugOverviewUfoChomperBiteRuntimeTest bugOverviewUfoChomperBiteRuntimeTest = this;
				TowerDefenseCharacterInstance instance = ufo.instance;
				bugOverviewUfoChomperBiteRuntimeTest.Check(instance != null && instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.CAR && Math.Abs(ufo.instance.biteHurt - 100.0) < 0.001, $"The runtime UFO must inherit its finite bite rule; physique={ufo.instance?.zombiePhysique}, biteHurt={ufo.instance?.biteHurt}.");
				if ((chomperComponent?.IsReleased ?? true) || (attackComponent?.IsReleased ?? true))
				{
					goto end_IL_0068;
				}
				ufo.LandEntered();
				Check((ufo.instance.collisionFlags & 1) != 0 && (ufo.instance.maskFlags & 1) != 0 && chomper.CanCollision(ufo.instance.maskFlags), "A landed ZombieUfo must remain reachable by a ground Chomper bite.");
				Check(chomper.CanTarget(ufo) && chomper.camp != ufo.camp, "The real plant and UFO must be opposing targets.");
				double currentHitPoint = ufo.GetCurrentHitPoint();
				Check(currentHitPoint > 100.0, $"The real UFO needs enough health to survive one finite bite; hp={currentHitPoint}.");
				chomperComponent.BitCharacter(ufo);
				double currentHitPoint2 = ufo.GetCurrentHitPoint();
				Check(Math.Abs(currentHitPoint - currentHitPoint2 - 100.0) < 0.001, $"A normal Chomper must bite the landed UFO for {100.0} damage; before={currentHitPoint}, after={currentHitPoint2}.");
				Check(!ufo.die && !ufo.nearDie && !ufo.isDestroy, "One Chomper bite must not kill or destroy the 4000-health UFO.");
				Check(!ufo.isChomp && !chomperComponent.eatCharacter, "The UFO must not enter the swallowed/chewing path.");
				goto end_IL_005f;
				end_IL_0068:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewUfoChomperBiteRuntimeTest] Unexpected exception: {value}");
				goto end_IL_005f;
			}
			return;
			end_IL_005f:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(chomper) && !chomper.IsQueuedForDeletion())
			{
				chomper.QueueFree();
			}
			if (GodotObject.IsInstanceValid(ufo) && !ufo.IsQueuedForDeletion())
			{
				ufo.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 13;
		GD.Print($"UFO_CHOMPER_BITE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
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
		for (int index = 0; index < count; index++)
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
			GD.PushError("[BugOverviewUfoChomperBiteRuntimeTest] " + message);
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
