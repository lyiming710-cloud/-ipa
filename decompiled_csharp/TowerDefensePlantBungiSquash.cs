using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter8/BungiSquash/Scene/TowerDefensePlantBungiSquash.cs")]
public class TowerDefensePlantBungiSquash : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnLanded = "OnLanded";

		public static readonly StringName SummonBungi = "SummonBungi";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private SquashComponent _squashComponent;

	private AttackComponent _attackComponent;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_squashComponent = componentManager.GetRuntime<SquashComponent>();
			_attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			if (TowerDefenseManager.Instance.IsGameRunning())
			{
				instance.biteHurt = 0.0;
				z = 600.0;
				isGround = false;
				sprite.SetAnimation("JumpUp", loop: false);
				OnLand += OnLanded;
			}
		}
	}

	public void OnLanded()
	{
		if (GodotObject.IsInstanceValid(this))
		{
			OnLand -= OnLanded;
		}
		if (isDestroy || isShovel)
		{
			return;
		}
		sprite.SetAnimation("JumpDown", loop: false);
		SummonBungi();
		GetTree().CreateTimer(0.5, processAlways: false).Timeout += () =>
		{
			if (GodotObject.IsInstanceValid(this))
			{
				Destroy();
			}
		};
	}

	public void SummonBungi()
	{
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseZombieBungi towerDefenseZombieBungi = (EconomyOwnerAccountId.IsValid ? (TowerDefenseCharacter.CreateCharacter(EconomyOwnerAccountId, "ZombieBungi", logicalGlobalPosition, gridPos, 0.0) as TowerDefenseZombieBungi) : (TowerDefenseCharacter.CreateCharacter("ZombieBungi", logicalGlobalPosition, gridPos, 0.0) as TowerDefenseZombieBungi));
		if (GodotObject.IsInstanceValid(towerDefenseZombieBungi))
		{
			Dictionary dictionary = new Dictionary { ["skipBungeeTarget"] = true };
			towerDefenseZombieBungi.ImportNetworkSpawnState(dictionary);
			if (!instance.hypnoses)
			{
				towerDefenseZombieBungi.Hypnoses();
			}
			TowerDefenseManager.PublishSpawnedCharacter("ZombieBungi", towerDefenseZombieBungi, useCreate: true, 0.0, walkAfterSpawn: true, "", dictionary);
			towerDefenseZombieBungi.CallDeferred("Walk");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnLanded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SummonBungi, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnLanded && args.Count == 0)
		{
			OnLanded();
			ret = default;
			return true;
		}
		if (method == MethodName.SummonBungi && args.Count == 0)
		{
			SummonBungi();
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
		if (method == MethodName.OnLanded)
		{
			return true;
		}
		if (method == MethodName.SummonBungi)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
