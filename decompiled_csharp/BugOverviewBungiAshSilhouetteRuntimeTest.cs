using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewBungiAshSilhouetteRuntimeTest.cs")]
public class BugOverviewBungiAshSilhouetteRuntimeTest : Node
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

	private const string BungiScenePath = "res://Asset/Anime/Character/Zombie/Chapter5/Bungi/Scene/TowerDefenseZombieBungi.tscn";

	private const string NormalConfigPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Config/TowerDefenseZombieNormal.tres";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseZombieBungi bungi = null;
		try
		{
			_ = 1;
			try
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter5/Bungi/Scene/TowerDefenseZombieBungi.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(packedScene), "The real Bungi scene must load.");
				TowerDefenseZombieConfig towerDefenseZombieConfig = ResourceLoader.Load<TowerDefenseZombieConfig>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Config/TowerDefenseZombieNormal.tres", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(towerDefenseZombieConfig), "The ordinary-zombie control config must load.");
				Check(GodotObject.IsInstanceValid(towerDefenseZombieConfig?.ashScene), "Ordinary zombies must retain the authored GeneralAsh scene.");
				bungi = packedScene?.Instantiate<TowerDefenseZombieBungi>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(bungi), "The real Bungi zombie must instantiate.");
				if (!GodotObject.IsInstanceValid(bungi))
				{
					goto end_IL_004c;
				}
				bungi.inGame = false;
				bungi.ProcessMode = ProcessModeEnum.Disabled;
				bungi.skipDestroySet = true;
				bungi.skipBungeeTarget = true;
				AddChild(bungi, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(3);
				Check(GodotObject.IsInstanceValid(bungi.sprite), "The real Bungi animation sprite must bind.");
				BugOverviewBungiAshSilhouetteRuntimeTest bugOverviewBungiAshSilhouetteRuntimeTest = this;
				DestroyComponent destroyComponent = bungi.destroyComponent;
				int condition;
				if (destroyComponent != null && !destroyComponent.IsReleased)
				{
					ZombieDeathComponent zombieDeathComponent = bungi.zombieDeathComponent;
					condition = ((zombieDeathComponent != null && !zombieDeathComponent.IsReleased) ? 1 : 0);
				}
				else
				{
					condition = 0;
				}
				bugOverviewBungiAshSilhouetteRuntimeTest.Check((byte)condition != 0, "The real Bungi destroy and death components must be active.");
				Check(bungi.config?.ashScene == null, "Bungi must use its own current silhouette instead of ZombieGeneralAsh.");
				string liveClip = bungi.sprite?.clip ?? string.Empty;
				Check(!string.IsNullOrEmpty(liveClip), "The Bungi fixture must begin on a real authored animation clip.");
				bungi.instance.invincible = false;
				double num = bungi.ExplodeHurt(bungi.instance.hitpoints + 1.0, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, playSplatAudio: false);
				Check(num > 0.0 && bungi.instance.hitpoints <= 0.0, $"A real bomb hit must be lethal; damage={num}, hp={bungi.instance.hitpoints}.");
				Check(bungi.die && bungi.isDestroy, "A lethal bomb hit must enter both death and destroy lifecycles.");
				await WaitFrames(2);
				bool flag = GodotObject.IsInstanceValid(bungi.sprite);
				Check(flag && bungi.sprite.pause, "The shader-based ash fallback must freeze the Bungi silhouette.");
				Check(flag && bungi.sprite.clip == liveClip, "Bungi ash must retain its authored silhouette clip " + liveClip + ".");
				goto end_IL_003a;
				end_IL_004c:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewBungiAshSilhouetteRuntimeTest] Unexpected exception: {value}");
				goto end_IL_003a;
			}
			return;
			end_IL_003a:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(bungi))
			{
				bungi.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag2 = _failures == 0 && _checks == 12;
		GD.Print($"BUNGI_ASH_SILHOUETTE_RESULT passed={flag2} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag2) ? 2 : 0);
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
			GD.PushError("[BugOverviewBungiAshSilhouetteRuntimeTest] " + message);
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
