using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewSwanRiderFireFallRuntimeTest.cs")]
public class BugOverviewSwanRiderFireFallRuntimeTest : Node
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
			_ = 1;
			try
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Challenge/SwanRider/Scene/TowerDefenseZombieSwanRider.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(packedScene), "The real SwanRider scene must load.");
				swan = packedScene?.Instantiate<TowerDefenseZombieSwanRider>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(swan), "The real SwanRider must instantiate.");
				if (!GodotObject.IsInstanceValid(swan))
				{
					goto end_IL_004c;
				}
				swan.inGame = false;
				swan.ProcessMode = ProcessModeEnum.Disabled;
				swan.skipDestroySet = true;
				AddChild(swan, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(3);
				Check(GodotObject.IsInstanceValid(swan.sprite), "The real SwanRider animation sprite must bind.");
				BugOverviewSwanRiderFireFallRuntimeTest bugOverviewSwanRiderFireFallRuntimeTest = this;
				DestroyComponent destroyComponent = swan.destroyComponent;
				int condition;
				if (destroyComponent != null && !destroyComponent.IsReleased)
				{
					ZombieDeathComponent zombieDeathComponent = swan.zombieDeathComponent;
					condition = ((zombieDeathComponent != null && !zombieDeathComponent.IsReleased) ? 1 : 0);
				}
				else
				{
					condition = 0;
				}
				bugOverviewSwanRiderFireFallRuntimeTest.Check((byte)condition != 0, "The real destroy and zombie-death components must be active.");
				Check(swan.config?.ashScene == null, "SwanRider must use the shader-based ash fallback used by fire-line kills.");
				swan.sprite.SetAnimation("Rise", loop: false);
				swan.isFly = true;
				swan.isFlying = false;
				Check(swan.sprite.clip == "Rise", "The fire-death fixture must start during Rise; got " + swan.sprite.clip + ".");
				Check(!swan.die && !swan.isDestroy && !swan.isFlying, "The fixture must begin alive and before airborne movement starts.");
				double num = swan.ExplodeHurt(swan.instance.hitpoints + 1.0, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.JALA, playSplatAudio: false);
				Check(num > 0.0 && swan.instance.hitpoints <= 0.0, $"A real Jala explosion must be lethal; damage={num}, hp={swan.instance.hitpoints}.");
				Check(swan.die && swan.isDestroy, "A lethal Jala explosion must enter both death and ash-destroy lifecycles.");
				Check(swan.sprite.clip == "Rise", "Shader-based ash death must initially preserve the airborne Rise pose; got " + swan.sprite.clip + ".");
				swan.AnimeCompleted("Rise");
				Check(swan.sprite.clip == "Rise", "A stale Rise completion after fire death must not switch the corpse to " + swan.sprite.clip + ".");
				Check(!swan.isFlying, "A stale Rise completion after fire death must not enable airborne movement.");
				Check(swan.StateMachine?.CurrentStateHandle?.StableId != "zombie.swan_rider.fly", "A stale Rise completion after fire death must not re-enter the Fly state.");
				await WaitFrames(2);
				Check(swan.sprite.pause, "The real ash-destroy path must pause the preserved Swan pose.");
				Check(swan.sprite.clip == "Rise" && !swan.isFlying, $"The burned Swan must remain stable through ash setup; clip={swan.sprite.clip}, isFlying={swan.isFlying}.");
				goto end_IL_003a;
				end_IL_004c:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewSwanRiderFireFallRuntimeTest] Unexpected exception: {value}");
				goto end_IL_003a;
			}
			return;
			end_IL_003a:;
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
		bool flag = _failures == 0 && _checks == 15;
		GD.Print($"SWAN_RIDER_FIRE_FALL_RESULT passed={flag} checks={_checks} failures={_failures}");
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
			GD.PushError("[BugOverviewSwanRiderFireFallRuntimeTest] " + message);
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
