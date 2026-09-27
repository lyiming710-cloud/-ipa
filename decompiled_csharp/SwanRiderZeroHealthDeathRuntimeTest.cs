using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/SwanRiderZeroHealthDeathRuntimeTest.cs")]
public class SwanRiderZeroHealthDeathRuntimeTest : Node
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

	private const string SwanScenePath = "res://Asset/Anime/Character/Zombie/Challenge/SwanRider/Scene/TowerDefenseZombieSwanRider.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseZombieSwanRider swan = null;
		try
		{
			try
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Challenge/SwanRider/Scene/TowerDefenseZombieSwanRider.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(packedScene), "The real SwanRider scene must load.");
				swan = packedScene?.Instantiate<TowerDefenseZombieSwanRider>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(swan), "The real SwanRider must instantiate.");
				if (!GodotObject.IsInstanceValid(swan))
				{
					goto end_IL_003f;
				}
				swan.inGame = false;
				swan.ProcessMode = ProcessModeEnum.Disabled;
				swan.skipDestroySet = true;
				AddChild(swan, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(3);
				Check(swan.StateMachine?.IsInitialized ?? false, "The SwanRider state machine must initialize.");
				Check(swan.SendStateEvent("ToWalk"), "The fixture must enter the normal walking state.");
				Check(swan.StateMachine?.CurrentStateHandle?.StableId == "zombie.walk", "The fixture must start walking; got " + swan.StateMachine?.CurrentStateHandle?.StableId + ".");
				double num = swan.Hurt(swan.instance.hitpoints + 1.0, playSplatAudio: false);
				Check(num > 0.0 && swan.instance.hitpoints <= 0.0, $"The real hit must be lethal; damage={num}, hp={swan.instance.hitpoints}.");
				Check(swan.die && swan.instance.die, "A zero-health SwanRider must be marked dead in both runtime layers.");
				Check(swan.StateMachine?.CurrentStateHandle?.StableId == "zombie.die", "Lethal damage must enter Die; got " + swan.StateMachine?.CurrentStateHandle?.StableId + ".");
				swan.Walk();
				Check(swan.StateMachine?.CurrentStateHandle?.StableId == "zombie.die", "A stale Walk callback must preserve Die; got " + swan.StateMachine?.CurrentStateHandle?.StableId + ".");
				Check(swan.sprite.clip == swan.dieAnimeClip, "A stale Walk callback must preserve the death clip; got " + swan.sprite.clip + ".");
				Check(swan.die && swan.instance.hitpoints <= 0.0, "The SwanRider must remain terminal after the stale callback.");
				goto end_IL_0036;
				end_IL_003f:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[SwanRiderZeroHealthDeathRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0036;
			}
			return;
			end_IL_0036:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(swan))
			{
				swan.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0 && _checks == 11;
		GD.Print($"SWAN_RIDER_ZERO_HEALTH_DEATH_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
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
			GD.PushError("[SwanRiderZeroHealthDeathRuntimeTest] " + message);
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
