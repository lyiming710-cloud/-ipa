using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/TowerDefenseManager/SubSystem/DamagePipeline.cs")]
public class DamagePipeline : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName ApplyHurtWithAttackConfig = "ApplyHurtWithAttackConfig";

		public static readonly StringName ApplyPreparedHurtWithAttackConfig = "ApplyPreparedHurtWithAttackConfig";

		public static readonly StringName ApplyHurt = "ApplyHurt";

		public static readonly StringName ApplyPreparedHurt = "ApplyPreparedHurt";

		public static readonly StringName IsRemoteSyncedCharacter = "IsRemoteSyncedCharacter";

		public static readonly StringName ApplyExplodeHurt = "ApplyExplodeHurt";

		public static readonly StringName ApplyPreparedExplodeHurt = "ApplyPreparedExplodeHurt";

		public static readonly StringName ApplyExplosionState = "ApplyExplosionState";

		public static readonly StringName ApplyFlagHurt = "ApplyFlagHurt";

		public static readonly StringName ApplyPreparedFlagHurt = "ApplyPreparedFlagHurt";

		public static readonly StringName ApplyProjectileHurt = "ApplyProjectileHurt";

		public static readonly StringName ApplyPreparedProjectileHurt = "ApplyPreparedProjectileHurt";

		public static readonly StringName ApplySkipInvincibleHurt = "ApplySkipInvincibleHurt";

		public static readonly StringName ApplyPreparedSkipInvincibleHurt = "ApplyPreparedSkipInvincibleHurt";

		public static readonly StringName ApplySmashHurt = "ApplySmashHurt";

		public static readonly StringName ApplyPreparedSmashHurt = "ApplyPreparedSmashHurt";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public double ApplyHurtWithAttackConfig(TowerDefenseCharacter character, AttackConfig attackConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true)
	{
		if (!TryPrepareDamage(character, out var instance))
		{
			return 0.0;
		}
		return ApplyPreparedHurtWithAttackConfig(character, instance, attackConfig, playSplatAudio, velocity, createDamagePart);
	}

	internal double ApplyPreparedHurtWithAttackConfig(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance, AttackConfig attackConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true)
	{
		double num = instance.HurtWithAttackConfig(attackConfig, playSplatAudio, velocity, createDamagePart);
		BattleEventBus.Instance.EmitCharacterHurt(character, (int)num, null);
		return num;
	}

	public double ApplyHurt(TowerDefenseCharacter character, double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool hitShield = true, bool createDamagePart = true, double damageLimit = -1.0)
	{
		if (!TryPrepareDamage(character, out var instance))
		{
			return 0.0;
		}
		return ApplyPreparedHurt(character, instance, num, playSplatAudio, velocity, hitShield, createDamagePart, damageLimit);
	}

	internal double ApplyPreparedHurt(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance, double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool hitShield = true, bool createDamagePart = true, double damageLimit = -1.0)
	{
		double num2 = instance.Hurt(num, playSplatAudio, velocity, hitShield, createDamagePart, damageLimit);
		BattleEventBus.Instance.EmitCharacterHurt(character, (int)num2, null);
		return num2;
	}

	private static bool IsRemoteSyncedCharacter(TowerDefenseCharacter character)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost && GodotObject.IsInstanceValid(character))
		{
			return character.syncId >= 0;
		}
		return false;
	}

	internal static bool TryPrepareCharacter(TowerDefenseCharacter character, out TowerDefenseCharacterInstance instance)
	{
		instance = null;
		if (!GodotObject.IsInstanceValid(character) || IsRemoteSyncedCharacter(character))
		{
			return false;
		}
		instance = character.instance;
		return GodotObject.IsInstanceValid(instance);
	}

	internal bool TryPrepareDamage(TowerDefenseCharacter character, out TowerDefenseCharacterInstance instance)
	{
		return TryPrepareCharacter(character, out instance);
	}

	public double ApplyExplodeHurt(TowerDefenseCharacter character, double num, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind = TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		if (!TryPrepareDamage(character, out var instance))
		{
			return 0.0;
		}
		return ApplyPreparedExplodeHurt(character, instance, num, damageKind, playSplatAudio, velocity);
	}

	internal double ApplyPreparedExplodeHurt(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance, double num, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind = TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		BattleEventBus instance2 = BattleEventBus.Instance;
		bool num2 = TowerDefensePerfProfiler.DetailedHotPathMetrics && TowerDefensePerfProfiler.Enabled;
		bool flag = instance2?.HasCharacterHurtSubscribers ?? false;
		if (!num2 && !flag)
		{
			return ApplyExplosionState(character, instance, num, damageKind, playSplatAudio, velocity);
		}
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		double num3 = ApplyExplosionState(character, instance, num, damageKind, playSplatAudio, velocity);
		TowerDefensePerfProfiler.End("damage.explode", startTicks, 1);
		instance2?.EmitCharacterHurt(character, (int)num3, null);
		return num3;
	}

	private static double ApplyExplosionState(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance, double num, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind, bool playSplatAudio, Vector2 velocity)
	{
		character.isExplode = true;
		try
		{
			return instance.ExplodeHurt(num, damageKind, playSplatAudio, velocity);
		}
		finally
		{
			character.isExplode = false;
		}
	}

	public double ApplyFlagHurt(TowerDefenseCharacter character, double num, int damageFlags, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true, bool isRange = false, int projectileHeight = -1)
	{
		if (!TryPrepareDamage(character, out var instance))
		{
			return 0.0;
		}
		return ApplyPreparedFlagHurt(character, instance, num, damageFlags, playSplatAudio, velocity, createDamagePart, isRange, projectileHeight);
	}

	internal double ApplyPreparedFlagHurt(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance, double num, int damageFlags, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true, bool isRange = false, int projectileHeight = -1)
	{
		double num2 = instance.FlagHurt(num, damageFlags, playSplatAudio, velocity, createDamagePart, isRange, projectileHeight);
		BattleEventBus.Instance.EmitCharacterHurt(character, (int)num2, null);
		return num2;
	}

	private static ProjectileHitInfo CreateProjectileHitInfo(TowerDefenseProjectile projectile, TowerDefenseProjectileConfig projectileConfig)
	{
		ProjectileHitInfo result = new ProjectileHitInfo
		{
			damage = projectileConfig.baseDamage,
			damageFlags = projectileConfig.damageFlags,
			position = default,
			projectileHeight = TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL,
			config = projectileConfig,
			fireCharacter = null,
			camp = TowerDefenseEnum.CHARACTER_CAMP.NOONE,
			collisionFlags = 0,
			gridPos = Vector2I.Zero,
			height = 0.0,
			onSourceDespawn = null
		};
		if (GodotObject.IsInstanceValid(projectile))
		{
			result.damage = projectile.damage;
			result.damageFlags = projectile.damageFlags;
			result.position = projectile.GlobalPosition;
			result.projectileHeight = projectile.projectileHeight;
			result.onSourceDespawn = projectile.Over;
		}
		return result;
	}

	public double ApplyProjectileHurt(TowerDefenseCharacter character, in ProjectileHitInfo info, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false, bool createDamagePart = true)
	{
		if (!TryPrepareDamage(character, out var instance))
		{
			return 0.0;
		}
		return ApplyPreparedProjectileHurt(character, instance, in info, projectileConfig, playSplatAudio, velocity, isRange, createDamagePart);
	}

	internal double ApplyPreparedProjectileHurt(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance, in ProjectileHitInfo info, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false, bool createDamagePart = true)
	{
		BattleEventBus instance2 = BattleEventBus.Instance;
		if ((!TowerDefensePerfProfiler.DetailedHotPathMetrics || !TowerDefensePerfProfiler.Enabled) && instance2 != null && !instance2.HasCharacterHurtSubscribers)
		{
			return instance.ProjectileHurt(in info, projectileConfig, playSplatAudio, velocity, isRange, createDamagePart);
		}
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
		double num = instance.ProjectileHurt(in info, projectileConfig, playSplatAudio, velocity, isRange, createDamagePart);
		TowerDefensePerfProfiler.End("damage.projectile", startTicks, 1);
		TowerDefensePerfProfiler.EndSpikeProbe("damage.projectile", in probe, 1);
		instance2.EmitCharacterHurt(character, (int)num, null);
		return num;
	}

	public double ApplyProjectileHurt(TowerDefenseCharacter character, TowerDefenseProjectile projectile, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false, bool createDamagePart = true)
	{
		if (!TryPrepareDamage(character, out var instance))
		{
			return 0.0;
		}
		return ApplyPreparedProjectileHurt(character, instance, projectile, projectileConfig, playSplatAudio, velocity, isRange, createDamagePart);
	}

	internal double ApplyPreparedProjectileHurt(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance, TowerDefenseProjectile projectile, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false, bool createDamagePart = true)
	{
		TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		double num = instance.ProjectileHurt(CreateProjectileHitInfo(projectile, projectileConfig), projectileConfig, playSplatAudio, velocity, isRange, createDamagePart);
		TowerDefensePerfProfiler.End("damage.projectile", startTicks, 1);
		TowerDefensePerfProfiler.EndSpikeProbe("damage.projectile", in probe, 1);
		BattleEventBus.Instance.EmitCharacterHurt(character, (int)num, projectile);
		return num;
	}

	public double ApplySkipInvincibleHurt(TowerDefenseCharacter character, double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool hitShield = true, bool createDamagePart = true)
	{
		if (!TryPrepareDamage(character, out var instance))
		{
			return 0.0;
		}
		return ApplyPreparedSkipInvincibleHurt(character, instance, num, playSplatAudio, velocity, hitShield, createDamagePart);
	}

	internal double ApplyPreparedSkipInvincibleHurt(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance, double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool hitShield = true, bool createDamagePart = true)
	{
		double num2 = instance.SkipInvincibleHurt(num, playSplatAudio, velocity, hitShield, createDamagePart);
		BattleEventBus.Instance.EmitCharacterHurt(character, (int)num2, null);
		return num2;
	}

	public double ApplySmashHurt(TowerDefenseCharacter character, double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		if (!TryPrepareDamage(character, out var instance))
		{
			return 0.0;
		}
		return ApplyPreparedSmashHurt(character, instance, num, playSplatAudio, velocity);
	}

	internal double ApplyPreparedSmashHurt(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance, double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		if (character is TowerDefensePlant)
		{
			double num2 = 100000.0;
			if (instance.smashHurt != -1.0)
			{
				num2 = Math.Min(instance.smashHurt, num2);
			}
			if (GodotObject.IsInstanceValid(character.cell))
			{
				bool flag = character.cell.smashAbsorbedFrame == Engine.GetPhysicsFrames();
				TowerDefenseCharacter slot = character.cell.GetSlot(character);
				if ((flag || (GodotObject.IsInstanceValid(slot) && (slot.instance.invincibleSmash || slot.instance.hitpoints - num2 > 0.0))) && instance.hitpoints - num2 <= 0.0)
				{
					return num;
				}
			}
		}
		character.isSmash = true;
		double num3;
		try
		{
			num3 = instance.SmashHurtApply(num, playSplatAudio, velocity);
		}
		finally
		{
			character.isSmash = false;
		}
		BattleEventBus.Instance.EmitCharacterHurt(character, (int)num3, null);
		return num3;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName.ApplyHurtWithAttackConfig, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "attackConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPreparedHurtWithAttackConfig, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "attackConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hitShield", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "damageLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPreparedHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hitShield", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "damageLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRemoteSyncedCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyExplodeHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPreparedExplodeHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyExplosionState, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyFlagHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "projectileHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPreparedFlagHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "projectileHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyProjectileHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPreparedProjectileHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySkipInvincibleHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hitShield", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPreparedSkipInvincibleHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hitShield", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySmashHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPreparedSmashHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ApplyHurtWithAttackConfig && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyHurtWithAttackConfig(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<AttackConfig>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.ApplyPreparedHurtWithAttackConfig && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyPreparedHurtWithAttackConfig(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<AttackConfig>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.ApplyHurt && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<double>(in args[6])));
			return true;
		}
		if (method == MethodName.ApplyPreparedHurt && args.Count == 8)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyPreparedHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<double>(in args[7])));
			return true;
		}
		if (method == MethodName.IsRemoteSyncedCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRemoteSyncedCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyExplodeHurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyExplodeHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4])));
			return true;
		}
		if (method == MethodName.ApplyPreparedExplodeHurt && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyPreparedExplodeHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<Vector2>(in args[5])));
			return true;
		}
		if (method == MethodName.ApplyExplosionState && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyExplosionState(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<Vector2>(in args[5])));
			return true;
		}
		if (method == MethodName.ApplyFlagHurt && args.Count == 8)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyFlagHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<int>(in args[7])));
			return true;
		}
		if (method == MethodName.ApplyPreparedFlagHurt && args.Count == 9)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyPreparedFlagHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<Vector2>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]), VariantUtils.ConvertTo<int>(in args[8])));
			return true;
		}
		if (method == MethodName.ApplyProjectileHurt && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyProjectileHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6])));
			return true;
		}
		if (method == MethodName.ApplyPreparedProjectileHurt && args.Count == 8)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyPreparedProjectileHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[2]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<Vector2>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7])));
			return true;
		}
		if (method == MethodName.ApplySkipInvincibleHurt && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<double>(ApplySkipInvincibleHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.ApplyPreparedSkipInvincibleHurt && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyPreparedSkipInvincibleHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6])));
			return true;
		}
		if (method == MethodName.ApplySmashHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ApplySmashHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.ApplyPreparedSmashHurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyPreparedSmashHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsRemoteSyncedCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRemoteSyncedCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyExplosionState && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyExplosionState(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<Vector2>(in args[5])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ApplyHurtWithAttackConfig)
		{
			return true;
		}
		if (method == MethodName.ApplyPreparedHurtWithAttackConfig)
		{
			return true;
		}
		if (method == MethodName.ApplyHurt)
		{
			return true;
		}
		if (method == MethodName.ApplyPreparedHurt)
		{
			return true;
		}
		if (method == MethodName.IsRemoteSyncedCharacter)
		{
			return true;
		}
		if (method == MethodName.ApplyExplodeHurt)
		{
			return true;
		}
		if (method == MethodName.ApplyPreparedExplodeHurt)
		{
			return true;
		}
		if (method == MethodName.ApplyExplosionState)
		{
			return true;
		}
		if (method == MethodName.ApplyFlagHurt)
		{
			return true;
		}
		if (method == MethodName.ApplyPreparedFlagHurt)
		{
			return true;
		}
		if (method == MethodName.ApplyProjectileHurt)
		{
			return true;
		}
		if (method == MethodName.ApplyPreparedProjectileHurt)
		{
			return true;
		}
		if (method == MethodName.ApplySkipInvincibleHurt)
		{
			return true;
		}
		if (method == MethodName.ApplyPreparedSkipInvincibleHurt)
		{
			return true;
		}
		if (method == MethodName.ApplySmashHurt)
		{
			return true;
		}
		if (method == MethodName.ApplyPreparedSmashHurt)
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
