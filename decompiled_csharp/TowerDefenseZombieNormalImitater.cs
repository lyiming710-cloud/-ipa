using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Imitater/TowerDefenseZombieNormalImitater.cs")]
public class TowerDefenseZombieNormalImitater : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName AttackEntered = "AttackEntered";

		public new static readonly StringName AttackExited = "AttackExited";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName ArmorDamagePointReach = "ArmorDamagePointReach";

		public static readonly StringName HeadAnimeCompleted = "HeadAnimeCompleted";

		public new static readonly StringName Purify = "Purify";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _zombieSprite = "_zombieSprite";

		public static readonly StringName halfHp = "halfHp";

		public static readonly StringName isAttack = "isAttack";

		public static readonly StringName over = "over";

		public static readonly StringName spawn = "spawn";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private ZombieNormalLmitaterSprite _zombieSprite;

	public bool halfHp;

	public bool isAttack;

	public bool over;

	public bool spawn;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_zombieSprite = sprite as ZombieNormalLmitaterSprite;
			ConfigureWaterLineVisualLayers("Zombie_duckytube", "Zombie_whitewater", "Zombie_whitewater2");
			_zombieSprite.head.OnAnimeCompleted += HeadAnimeCompleted;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || over || !TowerDefenseManager.Instance.IsGameRunning() || !inGame || !((double)GetLogicalGlobalPosition().X < TowerDefenseManager.Instance.GetMapGroundRight() - 50.0))
		{
			return;
		}
		bool flag = false;
		foreach (Variant item in TowerDefenseManager.Instance.GetZombie())
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
			if (towerDefenseCharacter.config.canImitate && instance.hypnoses == towerDefenseCharacter.instance.hypnoses)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			over = true;
			Idle();
			if (!die && !nearDie)
			{
				_zombieSprite.head.SetAnimation("Explode", loop: false);
			}
		}
	}

	public override void AttackEntered()
	{
		base.AttackEntered();
		isAttack = true;
		if (HasShield())
		{
			sprite.SetFliters(new Array { "Zombie_outerarm_upper" }, open: true);
			if (!halfHp)
			{
				sprite.SetFliters(new Array { "Zombie_outerarm_hand", "Zombie_outerarm_lower" }, open: true);
			}
		}
	}

	public override void AttackExited()
	{
		base.AttackExited();
		isAttack = false;
		if (HasShield())
		{
			sprite.SetFliters(new Array { "Zombie_outerarm_upper", "Zombie_outerarm_hand", "Zombie_outerarm_lower" }, open: false);
		}
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		if (!(damagePointName == "Arm"))
		{
			if (damagePointName == "Head")
			{
				DamagePartCreate("Head", _zombieSprite.head, new Vector2(GD.RandRange(-100, 100), -300f), keepSlotScale: false, new Vector2(-25f, -30f), fromSync: false, null, 0L);
			}
		}
		else
		{
			halfHp = true;
			sprite.SetFliters(new Array { "Zombie_outerarm_upper" }, open: true);
		}
	}

	public override void ArmorDamagePointReach(string armorName, int stage)
	{
		base.ArmorDamagePointReach(armorName, stage);
		if (isAttack && HasShield() && stage > 0)
		{
			sprite.SetFliters(new Array { "Zombie_outerarm_upper" }, open: true);
			if (!halfHp)
			{
				sprite.SetFliters(new Array { "Zombie_outerarm_hand", "Zombie_outerarm_lower" }, open: true);
			}
		}
	}

	public void HeadAnimeCompleted(string clip)
	{
		if (!(clip == "Explode") || spawn)
		{
			return;
		}
		spawn = true;
		Array zombie = TowerDefenseManager.Instance.GetZombie();
		double num = -10000.0;
		TowerDefenseCharacter towerDefenseCharacter = null;
		foreach (Variant item in zombie)
		{
			TowerDefenseCharacter towerDefenseCharacter2 = (TowerDefenseCharacter)(GodotObject)item;
			if (towerDefenseCharacter2.config.canImitate && instance.hypnoses == towerDefenseCharacter2.instance.hypnoses && towerDefenseCharacter2.instance.hitpoints > num)
			{
				towerDefenseCharacter = towerDefenseCharacter2;
				num = towerDefenseCharacter2.instance.hitpoints;
			}
		}
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			Walk();
			_zombieSprite.head.SetAnimation("Idle", loop: false);
			spawn = false;
			over = false;
			return;
		}
		if (GodotObject.IsInstanceValid(towerDefenseCharacter.packet))
		{
			TowerDefenseCharacter zombie2 = towerDefenseCharacter.packet.Plant(gridPos);
			zombie2.SetLogicalGlobalPosition(new Vector2(GetLogicalGlobalPosition().X, TowerDefenseManager.GetMapCellPlantPos(gridPos).Y));
			bool hypnoses = instance.hypnoses;
			Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(zombie2))
				{
					if (hypnoses)
					{
						zombie2.Hypnoses();
					}
					zombie2.SetSpriteGroupShaderParameter("imitater", true);
					if (GodotObject.IsInstanceValid(TowerDefenseBattleFeatureWave.Instance))
					{
						TowerDefenseBattleFeatureWave.Instance.currentHpPointTotal += zombie2.GetTotalHitPoint() / 3.0;
						TowerDefenseBattleFeatureWave.Instance.currentHpPoint += zombie2.GetTotalHitPoint() / 3.0;
					}
				}
			}).CallDeferred();
		}
		CallDeferred("Destroy", true);
	}

	public override void Purify()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			Destroy();
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantLmitater");
		if (cell.CanPacketPlant(packetConfig))
		{
			TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(gridPos);
			towerDefenseCharacter.WakeUp();
			if (instance.hypnoses)
			{
				towerDefenseCharacter.Hypnoses();
			}
			if (Global.IsMultiplayerMode && MultiPlayerManager.Instance.isHost)
			{
				TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
				if (GodotObject.IsInstanceValid(currentControl))
				{
					int nextSyncId = currentControl.GetNextSyncId();
					currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
					MultiPlayerManager.Instance.SendSpawnCharacterAt("PlantLmitater", gridPos.X, gridPos.Y, nextSyncId);
				}
			}
		}
		Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorDamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HeadAnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Purify, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackEntered && args.Count == 0)
		{
			AttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackExited && args.Count == 0)
		{
			AttackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorDamagePointReach && args.Count == 2)
		{
			ArmorDamagePointReach(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HeadAnimeCompleted && args.Count == 1)
		{
			HeadAnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Purify && args.Count == 0)
		{
			Purify();
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.AttackEntered)
		{
			return true;
		}
		if (method == MethodName.AttackExited)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.ArmorDamagePointReach)
		{
			return true;
		}
		if (method == MethodName.HeadAnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.Purify)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._zombieSprite)
		{
			_zombieSprite = VariantUtils.ConvertTo<ZombieNormalLmitaterSprite>(in value);
			return true;
		}
		if (name == PropertyName.halfHp)
		{
			halfHp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isAttack)
		{
			isAttack = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.spawn)
		{
			spawn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._zombieSprite)
		{
			value = VariantUtils.CreateFrom(in _zombieSprite);
			return true;
		}
		if (name == PropertyName.halfHp)
		{
			value = VariantUtils.CreateFrom(in halfHp);
			return true;
		}
		if (name == PropertyName.isAttack)
		{
			value = VariantUtils.CreateFrom(in isAttack);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.spawn)
		{
			value = VariantUtils.CreateFrom(in spawn);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._zombieSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.halfHp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isAttack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.spawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._zombieSprite, Variant.From(in _zombieSprite));
		info.AddProperty(PropertyName.halfHp, Variant.From(in halfHp));
		info.AddProperty(PropertyName.isAttack, Variant.From(in isAttack));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.spawn, Variant.From(in spawn));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._zombieSprite, out var value))
		{
			_zombieSprite = value.As<ZombieNormalLmitaterSprite>();
		}
		if (info.TryGetProperty(PropertyName.halfHp, out var value2))
		{
			halfHp = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isAttack, out var value3))
		{
			isAttack = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value4))
		{
			over = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.spawn, out var value5))
		{
			spawn = value5.As<bool>();
		}
	}
}
