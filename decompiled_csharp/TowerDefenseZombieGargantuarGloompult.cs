using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/Gloompult/TowerDefenseZombieGargantuarGloompult.cs")]
public class TowerDefenseZombieGargantuarGloompult : TowerDefenseZombieGargantuarBase
{
	public new class MethodName : TowerDefenseZombieGargantuarBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RefreshComponentRunning = "RefreshComponentRunning";

		public static readonly StringName OnAttackComponent2Ready = "OnAttackComponent2Ready";

		public static readonly StringName OnAttackComponent2Over = "OnAttackComponent2Over";

		public static readonly StringName OnFireComponentReady = "OnFireComponentReady";

		public static readonly StringName OnFireComponentOver = "OnFireComponentOver";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName TryStartGloompultAttack = "TryStartGloompultAttack";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public static readonly StringName GloompultAttack = "GloompultAttack";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName Purify = "Purify";
	}

	public new class PropertyName : TowerDefenseZombieGargantuarBase.PropertyName
	{
		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName fireNum = "fireNum";

		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName _gloompultAttackRunning = "_gloompultAttackRunning";

		public static readonly StringName _fireAttackRunning = "_fireAttackRunning";

		public static readonly StringName over = "over";

		public static readonly StringName _fireInterval = "_fireInterval";

		public static readonly StringName _fireNum = "_fireNum";

		public static readonly StringName _projectileName = "_projectileName";
	}

	public new class SignalName : TowerDefenseZombieGargantuarBase.SignalName
	{
	}

	private const string ZOMBIE_GARGANTUAR_HEAD2 = "uid://cfx6iqy08xujv";

	private const string ZOMBIE_GARGANTUAR_DUCKXING = "uid://6dy81rx4gaue";

	private const string ZOMBIE_GARGANTUAR_ZOMBIE = "uid://dtrl03qm2d0u7";

	private static PackedScene _TOWER_DEFENSE_PROJECTILE_EFFECT_GROOM;

	private FireComponent _fireComponent;

	private AttackComponent _attackComponent2;

	private bool _gloompultAttackRunning;

	private bool _fireAttackRunning;

	public bool over;

	private double _fireInterval = 3.0;

	private int _fireNum = 1;

	private string _projectileName = "Gloom";

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
			if (!IsNodeReady())
			{
				return;
			}
			FireComponent fireComponent = _fireComponent;
			if (fireComponent != null && !fireComponent.IsReleased)
			{
				_fireComponent.fireInterval = (float)_fireInterval;
				AttackComponent attackComponent = _attackComponent2;
				if (attackComponent != null && !attackComponent.IsReleased)
				{
					_attackComponent2.attackInterval = _fireInterval;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int fireNum
	{
		get
		{
			return _fireNum;
		}
		set
		{
			_fireNum = value;
			if (IsNodeReady())
			{
				FireComponent fireComponent = _fireComponent;
				if (fireComponent != null && !fireComponent.IsReleased)
				{
					_fireComponent.fireNum = _fireNum;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public string projectileName
	{
		get
		{
			return _projectileName;
		}
		set
		{
			_projectileName = value;
			if (IsNodeReady())
			{
				FireComponent fireComponent = _fireComponent;
				if (fireComponent != null && !fireComponent.IsReleased)
				{
					((FireComponentProjectileSingle)_fireComponent.fireCheckList[0].projectile).projectileName = _projectileName;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
			_attackComponent2.SetCheckAreaRectangleSize(0, TowerDefenseManager.Instance.GetMapGridSize() * 2.75f);
			double num = GD.Randf();
			if (num < 0.3)
			{
				sprite.SetAtlasReplace("Zombie_gargantuar_telephonepole.png", "uid://6dy81rx4gaue");
			}
			else if (num < 0.6)
			{
				sprite.SetAtlasReplace("Zombie_gargantuar_telephonepole.png", "uid://dtrl03qm2d0u7");
			}
			ConfigureWaterLineVisualLayers("Zombie_duckytube");
			if (!TowerDefenseManager.GetMapIsNight())
			{
				_fireComponent.timeScale *= 0.5f;
				walkSpeedScale *= 0.5;
			}
			else
			{
				_fireComponent.timeScale *= 1f;
				walkSpeedScale *= 1.0;
			}
			_attackComponent2.OnAttackReady += OnAttackComponent2Ready;
			_attackComponent2.OnAttackOver += OnAttackComponent2Over;
			_attackComponent2.OnAttack += GloompultAttack;
			_fireComponent.OnFireReady += OnFireComponentReady;
			_fireComponent.OnFireOver += OnFireComponentOver;
		}
	}

	private void RefreshComponentRunning()
	{
		componentRunning = _gloompultAttackRunning || _fireAttackRunning;
	}

	private void OnAttackComponent2Ready()
	{
		_gloompultAttackRunning = true;
		RefreshComponentRunning();
	}

	private void OnAttackComponent2Over()
	{
		_gloompultAttackRunning = false;
		RefreshComponentRunning();
	}

	private void OnFireComponentReady()
	{
		_fireAttackRunning = true;
		RefreshComponentRunning();
	}

	private void OnFireComponentOver()
	{
		_fireAttackRunning = false;
		RefreshComponentRunning();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		AttackComponent attackComponent = _attackComponent2;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			_attackComponent2.OnAttackReady -= OnAttackComponent2Ready;
			_attackComponent2.OnAttackOver -= OnAttackComponent2Over;
			_attackComponent2.OnAttack -= GloompultAttack;
		}
		FireComponent fireComponent = _fireComponent;
		if (fireComponent != null && !fireComponent.IsReleased)
		{
			_fireComponent.OnFireReady -= OnFireComponentReady;
			_fireComponent.OnFireOver -= OnFireComponentOver;
		}
	}

	public override void BatchUpdate(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			base.BatchUpdate(delta);
			AttackComponent attackComponent = _attackComponent2;
			if (attackComponent != null && !attackComponent.IsReleased)
			{
				_attackComponent2.attackInterval = _fireInterval;
				TryStartGloompultAttack();
			}
		}
	}

	private void TryStartGloompultAttack()
	{
		if (!_gloompultAttackRunning && !componentRunning && !die && !nearDie && _attackComponent2.alive && !(_attackComponent2.timer > 0.0) && (!GodotObject.IsInstanceValid(sprite) || !sprite.pause) && _attackComponent2.CanAttack())
		{
			_attackComponent2.SendStateEvent("ToAttack");
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		if (damangePointName == "Head")
		{
			sprite.SetAtlasReplace("Zombie_gargantuar_head.png", "uid://cfx6iqy08xujv");
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == impFireEvent)
		{
			((ZombieGargantuarGloompultSprite)sprite).puffShroomImpHead.Visible = false;
		}
	}

	public override void InWater()
	{
		base.InWater();
		sprite.SetFliter("Zombie_whitewater", open: true);
	}

	public override void OutWater()
	{
		base.OutWater();
		sprite.SetFliter("Zombie_whitewater", open: false);
	}

	public void GloompultAttack()
	{
		if (!TowerDefenseManager.Instance.backZombie)
		{
			TowerDefenseCharacter towerDefenseCharacter = _attackComponent2?.target;
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || (GodotObject.IsInstanceValid(towerDefenseCharacter.instance) && towerDefenseCharacter.instance.canBeCollection && !towerDefenseCharacter.die && !towerDefenseCharacter.nearDie && !towerDefenseCharacter.isDestroy))
			{
				TowerDefenseProjectileEffectGroom towerDefenseProjectileEffectGroom = TOWER_DEFENSE_PROJECTILE_EFFECT_GROOM.Instantiate<TowerDefenseProjectileEffectGroom>(PackedScene.GenEditState.Disabled);
				towerDefenseProjectileEffectGroom.Init(gridPos, camp, config.collisionFlags, null, groundHeight);
				towerDefenseProjectileEffectGroom.GlobalPosition = GetLogicalGlobalPosition();
				TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseProjectileEffectGroom, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	public override void DieEntered()
	{
		base.DieEntered();
		((ZombieGargantuarGloompultSprite)sprite).puffShroomImpHead.Visible = false;
	}

	public override void Purify()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			Destroy();
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantGloompult");
		if (cell.CanPacketPlant(packetConfig))
		{
			TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(gridPos);
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
					MultiPlayerManager.Instance.SendSpawnCharacterAt("PlantGloompult", gridPos.X, gridPos.Y, nextSyncId);
				}
			}
		}
		Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshComponentRunning, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAttackComponent2Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAttackComponent2Over, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnFireComponentReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnFireComponentOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryStartGloompultAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GloompultAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.RefreshComponentRunning && args.Count == 0)
		{
			RefreshComponentRunning();
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
		if (method == MethodName.OnFireComponentReady && args.Count == 0)
		{
			OnFireComponentReady();
			ret = default;
			return true;
		}
		if (method == MethodName.OnFireComponentOver && args.Count == 0)
		{
			OnFireComponentOver();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryStartGloompultAttack && args.Count == 0)
		{
			TryStartGloompultAttack();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InWater && args.Count == 0)
		{
			InWater();
			ret = default;
			return true;
		}
		if (method == MethodName.OutWater && args.Count == 0)
		{
			OutWater();
			ret = default;
			return true;
		}
		if (method == MethodName.GloompultAttack && args.Count == 0)
		{
			GloompultAttack();
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
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
		if (method == MethodName.RefreshComponentRunning)
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
		if (method == MethodName.OnFireComponentReady)
		{
			return true;
		}
		if (method == MethodName.OnFireComponentOver)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.TryStartGloompultAttack)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.InWater)
		{
			return true;
		}
		if (method == MethodName.OutWater)
		{
			return true;
		}
		if (method == MethodName.GloompultAttack)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
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
		if (name == PropertyName.fireNum)
		{
			fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._gloompultAttackRunning)
		{
			_gloompultAttackRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fireAttackRunning)
		{
			_fireAttackRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			_fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._fireNum)
		{
			_fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			_projectileName = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName.fireNum)
		{
			value = VariantUtils.CreateFrom<int>(fireNum);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom<string>(projectileName);
			return true;
		}
		if (name == PropertyName._gloompultAttackRunning)
		{
			value = VariantUtils.CreateFrom(in _gloompultAttackRunning);
			return true;
		}
		if (name == PropertyName._fireAttackRunning)
		{
			value = VariantUtils.CreateFrom(in _fireAttackRunning);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			value = VariantUtils.CreateFrom(in _fireInterval);
			return true;
		}
		if (name == PropertyName._fireNum)
		{
			value = VariantUtils.CreateFrom(in _fireNum);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			value = VariantUtils.CreateFrom(in _projectileName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._gloompultAttackRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._fireAttackRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._fireNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName.fireNum, Variant.From<int>(fireNum));
		info.AddProperty(PropertyName.projectileName, Variant.From<string>(projectileName));
		info.AddProperty(PropertyName._gloompultAttackRunning, Variant.From(in _gloompultAttackRunning));
		info.AddProperty(PropertyName._fireAttackRunning, Variant.From(in _fireAttackRunning));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
		info.AddProperty(PropertyName._fireNum, Variant.From(in _fireNum));
		info.AddProperty(PropertyName._projectileName, Variant.From(in _projectileName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fireInterval, out var value))
		{
			fireInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireNum, out var value2))
		{
			fireNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileName, out var value3))
		{
			projectileName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._gloompultAttackRunning, out var value4))
		{
			_gloompultAttackRunning = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fireAttackRunning, out var value5))
		{
			_fireAttackRunning = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value6))
		{
			over = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value7))
		{
			_fireInterval = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._fireNum, out var value8))
		{
			_fireNum = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._projectileName, out var value9))
		{
			_projectileName = value9.As<string>();
		}
	}
}
