using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Football/Scene/GloomShroom/TowerDefenseZombieFootballGloomShroom.cs")]
public class TowerDefenseZombieFootballGloomShroom : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnAttackComponent2Ready = "OnAttackComponent2Ready";

		public static readonly StringName OnAttackComponent2Over = "OnAttackComponent2Over";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public static readonly StringName TryStartGloomShroomAttack = "TryStartGloomShroomAttack";

		public static readonly StringName GloomShroomAttack = "GloomShroomAttack";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName Purify = "Purify";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName _gloomShroomAttackRunning = "_gloomShroomAttackRunning";

		public static readonly StringName _fireInterval = "_fireInterval";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private static PackedScene _TOWER_DEFENSE_PROJECTILE_EFFECT_GROOM;

	private AttackComponent _attackComponent2;

	private bool _gloomShroomAttackRunning;

	private double _fireInterval = 2.0;

	private static PackedScene TOWER_DEFENSE_PROJECTILE_EFFECT_GROOM => _TOWER_DEFENSE_PROJECTILE_EFFECT_GROOM ?? (_TOWER_DEFENSE_PROJECTILE_EFFECT_GROOM = GD.Load<PackedScene>("uid://bto1eksfijahm"));

	[Export(PropertyHint.None, "")]
	public double fireInterval
	{
		get
		{
			return _fireInterval;
		}
		set
		{
			_fireInterval = value;
			if (IsNodeReady())
			{
				AttackComponent attackComponent = _attackComponent2;
				if (attackComponent != null && !attackComponent.IsReleased)
				{
					_attackComponent2.attackInterval = _fireInterval;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
			_attackComponent2.OnAttackReady += OnAttackComponent2Ready;
			_attackComponent2.OnAttackOver += OnAttackComponent2Over;
			_attackComponent2.OnAttack += GloomShroomAttack;
			_attackComponent2.SetCheckAreaRectangleSize(0, TowerDefenseManager.Instance.GetMapGridSize() * 2.75f);
			ConfigureWaterLineVisualLayers("Zombie_duckytube", "Zombie_whitewater");
			if (!TowerDefenseManager.GetMapIsNight())
			{
				_attackComponent2.timeScale *= 0.5;
				walkSpeedScale *= 0.5;
			}
			else
			{
				_attackComponent2.timeScale *= 1.0;
				walkSpeedScale *= 1.0;
			}
		}
	}

	private void OnAttackComponent2Ready()
	{
		_gloomShroomAttackRunning = true;
	}

	private void OnAttackComponent2Over()
	{
		_gloomShroomAttackRunning = false;
	}

	public override void BatchUpdate(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			base.BatchUpdate(delta);
			TryStartGloomShroomAttack();
		}
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		sprite.timeScale = timeScale * 3.0;
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	private void TryStartGloomShroomAttack()
	{
		if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
		{
			AttackComponent attackComponent = _attackComponent2;
			if (attackComponent != null && !attackComponent.IsReleased && !_gloomShroomAttackRunning && !die && !nearDie && _attackComponent2.alive && !(_attackComponent2.timer > 0.0) && GodotObject.IsInstanceValid(sprite) && !sprite.pause && !(sprite.timeScale <= 0.0) && _attackComponent2.CanAttack())
			{
				_attackComponent2.SendStateEvent("ToAttack");
			}
		}
	}

	public void GloomShroomAttack()
	{
		TowerDefenseProjectileEffectGroom towerDefenseProjectileEffectGroom = TOWER_DEFENSE_PROJECTILE_EFFECT_GROOM.Instantiate<TowerDefenseProjectileEffectGroom>(PackedScene.GenEditState.Disabled);
		towerDefenseProjectileEffectGroom.Init(gridPos, camp, config.collisionFlags, null, groundHeight);
		towerDefenseProjectileEffectGroom.GlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseProjectileEffectGroom, forceReadableName: false, InternalMode.Disabled);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		AttackComponent attackComponent = _attackComponent2;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			_attackComponent2.OnAttackReady -= OnAttackComponent2Ready;
			_attackComponent2.OnAttackOver -= OnAttackComponent2Over;
			_attackComponent2.OnAttack -= GloomShroomAttack;
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		if (damangePointName == "Head")
		{
			DamagePartCreate("Head", ((ZombieFootballGloomShroom)sprite).head, new Vector2(GD.RandRange(-100, 100), -300f), keepSlotScale: false, new Vector2(-25f, -30f), fromSync: false, null, 0L);
		}
	}

	public override void Purify()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantGloomShroom");
		if (cell.CanPacketPlant(packetConfig, noLimit: true))
		{
			TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(gridPos, playAudio: true, noLimit: true);
			towerDefenseCharacter.WakeUp();
			if (instance.hypnoses)
			{
				towerDefenseCharacter.Hypnoses();
			}
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
				if (GodotObject.IsInstanceValid(currentControl))
				{
					int nextSyncId = currentControl.GetNextSyncId();
					currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
					MultiPlayerManager.Instance.SendSpawnCharacterAt("PlantGloomShroom", gridPos.X, gridPos.Y, nextSyncId);
				}
			}
		}
		Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAttackComponent2Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAttackComponent2Over, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryStartGloomShroomAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GloomShroomAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OnAttackComponent2Ready && args.Count == 0)
		{
			OnAttackComponent2Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAttackComponent2Over && args.Count == 0)
		{
			OnAttackComponent2Over();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryStartGloomShroomAttack && args.Count == 0)
		{
			TryStartGloomShroomAttack();
			ret = default;
			return true;
		}
		if (method == MethodName.GloomShroomAttack && args.Count == 0)
		{
			GloomShroomAttack();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.OnAttackComponent2Ready)
		{
			return true;
		}
		if (method == MethodName.OnAttackComponent2Over)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.TryStartGloomShroomAttack)
		{
			return true;
		}
		if (method == MethodName.GloomShroomAttack)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
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
		if (name == PropertyName.fireInterval)
		{
			fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._gloomShroomAttackRunning)
		{
			_gloomShroomAttackRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			_fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.fireInterval)
		{
			value = VariantUtils.CreateFrom<double>(fireInterval);
			return true;
		}
		if (name == PropertyName._gloomShroomAttackRunning)
		{
			value = VariantUtils.CreateFrom(in _gloomShroomAttackRunning);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			value = VariantUtils.CreateFrom(in _fireInterval);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._gloomShroomAttackRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName._gloomShroomAttackRunning, Variant.From(in _gloomShroomAttackRunning));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fireInterval, out var value))
		{
			fireInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._gloomShroomAttackRunning, out var value2))
		{
			_gloomShroomAttackRunning = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value3))
		{
			_fireInterval = value3.As<double>();
		}
	}
}
