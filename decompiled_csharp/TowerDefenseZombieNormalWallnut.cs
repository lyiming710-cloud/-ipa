using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Wallnut/TowerDefenseZombieNormalWallnut.cs")]
public class TowerDefenseZombieNormalWallnut : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName AttackEntered = "AttackEntered";

		public new static readonly StringName AttackExited = "AttackExited";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName SetWallnutHeadDamageStage = "SetWallnutHeadDamageStage";

		public static readonly StringName TryApplyWallnutHeadDamageStage = "TryApplyWallnutHeadDamageStage";

		public new static readonly StringName ArmorDamagePointReach = "ArmorDamagePointReach";

		public new static readonly StringName Purify = "Purify";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _zombieSprite = "_zombieSprite";

		public static readonly StringName _wallnutHeadDamageStage = "_wallnutHeadDamageStage";

		public static readonly StringName _appliedWallnutHeadDamageStage = "_appliedWallnutHeadDamageStage";

		public static readonly StringName halfHp = "halfHp";

		public static readonly StringName isAttack = "isAttack";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const string WALLNUT_BODY_MEDIA = "Wallnut_body.png";

	private const string WALLNUT_CRACKED_1 = "uid://dpnwmtm6ypomi";

	private const string WALLNUT_CRACKED_2 = "uid://dq2ayfqxj5mfx";

	private const string WALLNUT_DAMAGE_STAGE_KEY = "wallnutHeadDamageStage";

	private const int WALLNUT_HEAD_DAMAGE_STAGE_NONE = 0;

	private const int WALLNUT_HEAD_DAMAGE_STAGE_ONE = 1;

	private const int WALLNUT_HEAD_DAMAGE_STAGE_TWO = 2;

	private ZombieNormalWallnutSprite _zombieSprite;

	private int _wallnutHeadDamageStage;

	private int _appliedWallnutHeadDamageStage = -1;

	public bool halfHp;

	public bool isAttack;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_zombieSprite = sprite as ZombieNormalWallnutSprite;
			TryApplyWallnutHeadDamageStage();
			ConfigureWaterLineVisualLayers("Zombie_duckytube", "Zombie_whitewater", "Zombie_whitewater2");
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (_appliedWallnutHeadDamageStage != _wallnutHeadDamageStage)
		{
			TryApplyWallnutHeadDamageStage();
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
		switch (damagePointName)
		{
		case "Arm":
			halfHp = true;
			sprite.SetFliters(new Array { "Zombie_outerarm_upper" }, open: true);
			break;
		case "Head":
			DamagePartCreate("Head", _zombieSprite.head, new Vector2(GD.RandRange(-100, 100), -300f), keepSlotScale: false, new Vector2(-25f, -30f), fromSync: false, null, 0L);
			break;
		case "Damage1":
			SetWallnutHeadDamageStage(1);
			break;
		case "Damage2":
			SetWallnutHeadDamageStage(2);
			break;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["wallnutHeadDamageStage"] = _wallnutHeadDamageStage;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		int wallnutHeadDamageStage = data["wallnutHeadDamageStage"].AsInt32();
		SetWallnutHeadDamageStage(wallnutHeadDamageStage);
	}

	private void SetWallnutHeadDamageStage(int stage)
	{
		if (stage < 0)
		{
			stage = 0;
		}
		if (stage > 2)
		{
			stage = 2;
		}
		_wallnutHeadDamageStage = stage;
		TryApplyWallnutHeadDamageStage();
	}

	private bool TryApplyWallnutHeadDamageStage()
	{
		if (!GodotObject.IsInstanceValid(_zombieSprite) || !GodotObject.IsInstanceValid(_zombieSprite.head))
		{
			return false;
		}
		string text = _wallnutHeadDamageStage switch
		{
			1 => "uid://dpnwmtm6ypomi", 
			2 => "uid://dq2ayfqxj5mfx", 
			_ => string.Empty, 
		};
		if (_appliedWallnutHeadDamageStage == _wallnutHeadDamageStage && _zombieSprite.head.GetAtlasReplacePath("Wallnut_body.png") == text)
		{
			return true;
		}
		if (!_zombieSprite.head.SetAtlasReplace("Wallnut_body.png", text))
		{
			return false;
		}
		_appliedWallnutHeadDamageStage = _wallnutHeadDamageStage;
		return true;
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
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantWallnut");
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
					MultiPlayerManager.Instance.SendSpawnCharacterAt("PlantWallnut", gridPos.X, gridPos.Y, nextSyncId);
				}
			}
		}
		Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
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
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetWallnutHeadDamageStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryApplyWallnutHeadDamageStage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ArmorDamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetWallnutHeadDamageStage && args.Count == 1)
		{
			SetWallnutHeadDamageStage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryApplyWallnutHeadDamageStage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryApplyWallnutHeadDamageStage());
			return true;
		}
		if (method == MethodName.ArmorDamagePointReach && args.Count == 2)
		{
			ArmorDamagePointReach(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
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
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.SetWallnutHeadDamageStage)
		{
			return true;
		}
		if (method == MethodName.TryApplyWallnutHeadDamageStage)
		{
			return true;
		}
		if (method == MethodName.ArmorDamagePointReach)
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
			_zombieSprite = VariantUtils.ConvertTo<ZombieNormalWallnutSprite>(in value);
			return true;
		}
		if (name == PropertyName._wallnutHeadDamageStage)
		{
			_wallnutHeadDamageStage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._appliedWallnutHeadDamageStage)
		{
			_appliedWallnutHeadDamageStage = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._wallnutHeadDamageStage)
		{
			value = VariantUtils.CreateFrom(in _wallnutHeadDamageStage);
			return true;
		}
		if (name == PropertyName._appliedWallnutHeadDamageStage)
		{
			value = VariantUtils.CreateFrom(in _appliedWallnutHeadDamageStage);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._zombieSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._wallnutHeadDamageStage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._appliedWallnutHeadDamageStage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.halfHp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isAttack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._zombieSprite, Variant.From(in _zombieSprite));
		info.AddProperty(PropertyName._wallnutHeadDamageStage, Variant.From(in _wallnutHeadDamageStage));
		info.AddProperty(PropertyName._appliedWallnutHeadDamageStage, Variant.From(in _appliedWallnutHeadDamageStage));
		info.AddProperty(PropertyName.halfHp, Variant.From(in halfHp));
		info.AddProperty(PropertyName.isAttack, Variant.From(in isAttack));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._zombieSprite, out var value))
		{
			_zombieSprite = value.As<ZombieNormalWallnutSprite>();
		}
		if (info.TryGetProperty(PropertyName._wallnutHeadDamageStage, out var value2))
		{
			_wallnutHeadDamageStage = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._appliedWallnutHeadDamageStage, out var value3))
		{
			_appliedWallnutHeadDamageStage = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.halfHp, out var value4))
		{
			halfHp = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isAttack, out var value5))
		{
			isAttack = value5.As<bool>();
		}
	}
}
