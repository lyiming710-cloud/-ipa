using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ZombieZeroHealthDeathGuardRuntimeTest.cs")]
public class ZombieZeroHealthDeathGuardRuntimeTest : Node
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

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string AxeZombieScenePath = "res://Asset/Anime/Character/Zombie/Challenge/Axe/Scene/TowerDefenseZombieAxe.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseZombieNormal zombie = null;
		TowerDefenseZombieNormal survivor = null;
		TowerDefenseZombieAxe axeZombie = null;
		try
		{
			_ = 2;
			try
			{
				zombie = await CreateFixture<TowerDefenseZombieNormal>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
				Check(GodotObject.IsInstanceValid(zombie), "The real normal zombie fixture must instantiate.");
				if (!GodotObject.IsInstanceValid(zombie))
				{
					goto end_IL_0063;
				}
				Check(zombie.StateMachine?.IsInitialized ?? false, "The zombie state machine must initialize.");
				Check(zombie.SendStateEvent("ToWalk"), "The fixture must accept its initial walk event.");
				Check(zombie.CurrentStateHandle?.StableId == "zombie.walk", "The fixture must start walking; got " + (zombie.CurrentStateHandle?.StableId ?? "<null>") + ".");
				zombie.instance.hitpoints = 0.0;
				zombie.BatchUpdate(1.0 / 60.0);
				Check(zombie.instance.die && zombie.die, "Fixed update must commit zero-health death in both runtime layers.");
				Check(zombie.instance.nearDie && zombie.nearDie, "Fixed update must also converge the near-death flags.");
				Check(zombie.CurrentStateHandle?.StableId == "zombie.die", "Zero health must enter zombie.die; got " + (zombie.CurrentStateHandle?.StableId ?? "<null>") + ".");
				Check(zombie.zombieDeathComponent?.IsDeathAnimationClip(zombie.sprite.clip) ?? false, "Zero health must play the death clip; got " + zombie.sprite.clip + ".");
				zombie.Walk();
				zombie.Attack();
				zombie.SendStateEvent("ToIdle");
				Check(zombie.CurrentStateHandle?.StableId == "zombie.die", "Live-state callbacks must preserve zombie.die; got " + (zombie.CurrentStateHandle?.StableId ?? "<null>") + ".");
				Check(zombie.zombieDeathComponent?.IsDeathAnimationClip(zombie.sprite.clip) ?? false, "Live-state callbacks must preserve the death clip; got " + zombie.sprite.clip + ".");
				Check(zombie.instance.die && zombie.die && zombie.instance.hitpoints <= 0.0, "The zero-health zombie must remain terminal after stale callbacks.");
				survivor = await CreateFixture<TowerDefenseZombieNormal>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
				Check(GodotObject.IsInstanceValid(survivor), "The keepAlive fixture must instantiate.");
				if (!GodotObject.IsInstanceValid(survivor))
				{
					goto end_IL_0063;
				}
				survivor.instance.keepAlive = true;
				survivor.instance.hitpoints = 0.0;
				survivor.BatchUpdate(1.0 / 60.0);
				survivor.Walk();
				Check(!survivor.instance.die && !survivor.die, "keepAlive must remain exempt from forced zero-health death.");
				Check(survivor.CurrentStateHandle?.StableId == "zombie.walk", "A keepAlive zombie may still walk; got " + (survivor.CurrentStateHandle?.StableId ?? "<null>") + ".");
				axeZombie = await CreateFixture<TowerDefenseZombieAxe>("res://Asset/Anime/Character/Zombie/Challenge/Axe/Scene/TowerDefenseZombieAxe.tscn");
				Check(GodotObject.IsInstanceValid(axeZombie), "The axe zombie fixture must instantiate.");
				if (!GodotObject.IsInstanceValid(axeZombie))
				{
					goto end_IL_0063;
				}
				axeZombie.instance.hitpoints = 0.0;
				axeZombie.BatchUpdate(1.0 / 60.0);
				Check(axeZombie.instance.die && axeZombie.die, "Axe zero-health convergence must finish without recursive damage events.");
				Check(axeZombie.CurrentStateHandle?.StableId == "zombie.die", "Axe zero health must enter zombie.die; got " + (axeZombie.CurrentStateHandle?.StableId ?? "<null>") + ".");
				goto end_IL_004c;
				end_IL_0063:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[ZombieZeroHealthDeathGuardRuntimeTest] Unexpected exception: {value}");
				goto end_IL_004c;
			}
			return;
			end_IL_004c:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(zombie))
			{
				zombie.QueueFree();
			}
			if (GodotObject.IsInstanceValid(survivor))
			{
				survivor.QueueFree();
			}
			if (GodotObject.IsInstanceValid(axeZombie))
			{
				axeZombie.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0 && _checks == 17;
		GD.Print($"ZOMBIE_ZERO_HEALTH_DEATH_GUARD_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task<T> CreateFixture<T>(string scenePath) where T : TowerDefenseZombie
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Ignore);
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			return null;
		}
		T zombie = packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
		zombie.inGame = false;
		zombie.ProcessMode = ProcessModeEnum.Disabled;
		zombie.skipDestroySet = true;
		AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
		for (int frame = 0; frame < 3; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		return zombie;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[ZombieZeroHealthDeathGuardRuntimeTest] " + message);
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
