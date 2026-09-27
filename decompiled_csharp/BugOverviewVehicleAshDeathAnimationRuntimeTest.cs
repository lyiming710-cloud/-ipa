using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewVehicleAshDeathAnimationRuntimeTest.cs")]
public class BugOverviewVehicleAshDeathAnimationRuntimeTest : Node
{
	private readonly record struct VehicleCase(string Label, string ScenePath, string LiveClip, string DeathClip);

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

	private static readonly VehicleCase[] VehicleCases = new VehicleCase[3]
	{
		new VehicleCase("Zamboni", "res://Asset/Anime/Character/Zombie/Chapter2/Zamboni/Scene/Normal/TowerDefenseZombieZamboni.tscn", "Drive", "Wheelie&Wheelie2"),
		new VehicleCase("Catapult", "res://Asset/Anime/Character/Zombie/Chapter5/Catapult/Scene/TowerDefenseZombieCatapult.tscn", "Walk", "Bounce"),
		new VehicleCase("GargantuarZamboni", "res://Asset/Anime/Character/Zombie/Challenge/GargantuarZamboni/Scene/Base/TowerDefenseZombieGargantuarZamboni.tscn", "Drive", "Wheelie")
	};

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		_ = 1;
		try
		{
			VehicleCase[] vehicleCases = VehicleCases;
			foreach (VehicleCase vehicleCase in vehicleCases)
			{
				await VerifyAshDeathKeepsCurrentPose(vehicleCase);
			}
			await VerifyOrdinaryDeathStillUsesAuthoredClip(VehicleCases[1]);
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BugOverviewVehicleAshDeathAnimationRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0 && _checks == 26;
		GD.Print($"VEHICLE_ASH_DEATH_ANIMATION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyAshDeathKeepsCurrentPose(VehicleCase vehicleCase)
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(vehicleCase.ScenePath, null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(packedScene), "The real " + vehicleCase.Label + " scene must load.");
		TowerDefenseZombie zombie = packedScene?.Instantiate<TowerDefenseZombie>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(zombie), "The real " + vehicleCase.Label + " zombie must instantiate.");
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return;
		}
		zombie.inGame = false;
		zombie.ProcessMode = ProcessModeEnum.Disabled;
		AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(3);
		Check(GodotObject.IsInstanceValid(zombie.sprite), "The real " + vehicleCase.Label + " animation sprite must bind.");
		BugOverviewVehicleAshDeathAnimationRuntimeTest bugOverviewVehicleAshDeathAnimationRuntimeTest = this;
		ZombieDeathComponent zombieDeathComponent = zombie.zombieDeathComponent;
		bugOverviewVehicleAshDeathAnimationRuntimeTest.Check(zombieDeathComponent != null && !zombieDeathComponent.IsReleased, "The real " + vehicleCase.Label + " death component must be active.");
		Check(zombie.config?.ashScene == null && zombie.dieAnimeClip == vehicleCase.DeathClip, vehicleCase.Label + " must use its authored vehicle death clip and shader-based ash fallback.");
		if (GodotObject.IsInstanceValid(zombie.sprite))
		{
			ZombieDeathComponent zombieDeathComponent2 = zombie.zombieDeathComponent;
			if (zombieDeathComponent2 != null && !zombieDeathComponent2.IsReleased)
			{
				zombie.sprite.SetAnimation(vehicleCase.LiveClip);
				Check(zombie.sprite.clip == vehicleCase.LiveClip, $"{vehicleCase.Label} fixture must start from {vehicleCase.LiveClip}; got {zombie.sprite.clip}.");
				zombie.isExplode = true;
				zombie.die = true;
				zombie.nearDie = true;
				zombie.zombieDeathComponent.DieEntered();
				Check(zombie.sprite.clip == vehicleCase.LiveClip, $"Ash death must keep {vehicleCase.Label} on {vehicleCase.LiveClip} instead of starting {vehicleCase.DeathClip}; got {zombie.sprite.clip}.");
			}
		}
		zombie.QueueFree();
		await WaitFrames(2);
	}

	private async Task VerifyOrdinaryDeathStillUsesAuthoredClip(VehicleCase vehicleCase)
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(vehicleCase.ScenePath, null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(packedScene), "The ordinary-death control scene must load.");
		TowerDefenseZombie zombie = packedScene?.Instantiate<TowerDefenseZombie>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(zombie), "The ordinary-death control zombie must instantiate.");
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return;
		}
		zombie.inGame = false;
		zombie.ProcessMode = ProcessModeEnum.Disabled;
		AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(3);
		BugOverviewVehicleAshDeathAnimationRuntimeTest bugOverviewVehicleAshDeathAnimationRuntimeTest = this;
		int condition;
		if (GodotObject.IsInstanceValid(zombie.sprite))
		{
			ZombieDeathComponent zombieDeathComponent = zombie.zombieDeathComponent;
			condition = ((zombieDeathComponent != null && !zombieDeathComponent.IsReleased) ? 1 : 0);
		}
		else
		{
			condition = 0;
		}
		bugOverviewVehicleAshDeathAnimationRuntimeTest.Check((byte)condition != 0, "The ordinary-death control must bind its real sprite and death component.");
		if (GodotObject.IsInstanceValid(zombie.sprite))
		{
			ZombieDeathComponent zombieDeathComponent2 = zombie.zombieDeathComponent;
			if (zombieDeathComponent2 != null && !zombieDeathComponent2.IsReleased)
			{
				zombie.sprite.SetAnimation(vehicleCase.LiveClip);
				Check(zombie.sprite.clip == vehicleCase.LiveClip, "The ordinary-death control must start from its live animation.");
				zombie.isExplode = false;
				zombie.die = true;
				zombie.nearDie = true;
				zombie.zombieDeathComponent.DieEntered();
				Check(zombie.zombieDeathComponent.IsDeathAnimationClip(zombie.sprite.clip), "Ordinary death must still start the authored death clip; got " + zombie.sprite.clip + ".");
			}
		}
		zombie.QueueFree();
		await WaitFrames(2);
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
			GD.PushError("[BugOverviewVehicleAshDeathAnimationRuntimeTest] " + message);
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
