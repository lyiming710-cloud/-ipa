using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Item/Sheild/Scene/TowerDefenseItemSheild.cs")]
public class TowerDefenseItemSheild : TowerDefenseItem
{
	public new class MethodName : TowerDefenseItem.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName _RefreshLabelIfHitpointsChanged = "_RefreshLabelIfHitpointsChanged";

		public static readonly StringName _RefreshProjectileTargetability = "_RefreshProjectileTargetability";

		public static readonly StringName _HasGroundProjectileCarrier = "_HasGroundProjectileCarrier";

		public static readonly StringName _CanCarryGroundProjectile = "_CanCarryGroundProjectile";

		public static readonly StringName ApplyPendingShieldHitpoints = "ApplyPendingShieldHitpoints";

		public static readonly StringName _ShieldUpdateLabel = "_ShieldUpdateLabel";

		public static readonly StringName ShieldGetLayerCount = "ShieldGetLayerCount";

		public static readonly StringName ShieldAddHitpoints = "ShieldAddHitpoints";

		public static readonly StringName ShieldAbsorbDamage = "ShieldAbsorbDamage";

		public static readonly StringName ShieldBlockLethal = "ShieldBlockLethal";

		public static readonly StringName ShieldBlockCharm = "ShieldBlockCharm";

		public static readonly StringName ShieldDeflateVehicle = "ShieldDeflateVehicle";

		public static readonly StringName _ShieldUpdateAppearance = "_ShieldUpdateAppearance";

		public static readonly StringName CreateOnCell = "CreateOnCell";

		public static readonly StringName CreateOnCellWithHP = "CreateOnCellWithHP";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public static readonly StringName shieldType = "shieldType";

		public static readonly StringName shieldHitpoints = "shieldHitpoints";

		public static readonly StringName _shieldType = "_shieldType";

		public static readonly StringName _pendingShieldHitpoints = "_pendingShieldHitpoints";

		public static readonly StringName _layerLabel = "_layerLabel";

		public static readonly StringName _lastLabelHitpoints = "_lastLabelHitpoints";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	public const int MAX_LAYERS = 8;

	public const double HP_PER_LAYER = 1000.0;

	public const double MAX_HP = 8000.0;

	private StringName _shieldType = new StringName("Default");

	private double _pendingShieldHitpoints = -1.0;

	private Label _layerLabel;

	private double _lastLabelHitpoints = 0.0 / 0.0;

	[Export(PropertyHint.None, "")]
	public StringName shieldType
	{
		get
		{
			return _shieldType;
		}
		set
		{
			StringName stringName = _shieldType;
			_shieldType = value;
			if (IsNodeReady() && stringName != _shieldType)
			{
				_ShieldUpdateAppearance();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public double shieldHitpoints
	{
		get
		{
			if (GodotObject.IsInstanceValid(instance))
			{
				return instance.hitpoints;
			}
			if (!(_pendingShieldHitpoints >= 0.0))
			{
				return 1000.0;
			}
			return _pendingShieldHitpoints;
		}
		set
		{
			_pendingShieldHitpoints = Mathf.Clamp(value, 0.0, 8000.0);
			ApplyPendingShieldHitpoints();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_layerLabel = GetNodeOrNull<Label>("%LayerLabel");
			_RefreshProjectileTargetability();
			ApplyPendingShieldHitpoints();
			_ShieldUpdateAppearance();
			_ShieldUpdateLabel();
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && GodotObject.IsInstanceValid(instance))
		{
			_RefreshProjectileTargetability();
			_RefreshLabelIfHitpointsChanged();
		}
	}

	private void _RefreshLabelIfHitpointsChanged()
	{
		if (GodotObject.IsInstanceValid(_layerLabel) && instance.hitpoints != _lastLabelHitpoints)
		{
			_ShieldUpdateLabel();
		}
	}

	private void _RefreshProjectileTargetability()
	{
		if (GodotObject.IsInstanceValid(instance))
		{
			instance.canBeCollection = !_HasGroundProjectileCarrier();
		}
	}

	private bool _HasGroundProjectileCarrier()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return false;
		}
		foreach (TowerDefenseCharacter character in cell.characterList)
		{
			if (_CanCarryGroundProjectile(character))
			{
				return true;
			}
		}
		return _CanCarryGroundProjectile(cell.characterSurround);
	}

	private bool _CanCarryGroundProjectile(TowerDefenseCharacter character)
	{
		if (!(character is TowerDefensePlant) || !GodotObject.IsInstanceValid(character.instance))
		{
			return false;
		}
		if (!character.instance.canBeCollection || character.instance.hypnoses != instance.hypnoses)
		{
			return false;
		}
		int num = 6;
		if ((character.instance.physiqueTypeFlags & num) != 0)
		{
			return false;
		}
		return (character.instance.maskFlags & 1) != 0;
	}

	private void ApplyPendingShieldHitpoints()
	{
		if (!(_pendingShieldHitpoints < 0.0) && GodotObject.IsInstanceValid(instance))
		{
			instance.hitpoints = Mathf.Min(_pendingShieldHitpoints, 8000.0);
			_ShieldUpdateLabel();
		}
	}

	public void _ShieldUpdateLabel()
	{
		if (GodotObject.IsInstanceValid(_layerLabel))
		{
			_lastLabelHitpoints = instance.hitpoints;
			_layerLabel.Text = ((int)instance.hitpoints).ToString();
		}
	}

	public int ShieldGetLayerCount()
	{
		return Mathf.Clamp((int)(instance.hitpoints / 1000.0), 0, 8);
	}

	public void ShieldAddHitpoints(double num, StringName newShieldType = null)
	{
		if (!(num <= 0.0))
		{
			if ((object)newShieldType == null)
			{
				newShieldType = new StringName("Default");
			}
			instance.hitpoints = Mathf.Min(instance.hitpoints + num, 8000.0);
			if (_shieldType != newShieldType)
			{
				_shieldType = newShieldType;
				_ShieldUpdateAppearance();
			}
			_ShieldUpdateLabel();
		}
	}

	public double ShieldAbsorbDamage(double num)
	{
		if (num <= 0.0 || instance.hitpoints <= 0.0)
		{
			return num;
		}
		double num2 = Mathf.Min(num, instance.hitpoints);
		instance.hitpoints -= num2;
		_ShieldUpdateLabel();
		if (instance.hitpoints <= 0.0)
		{
			instance.hitpoints = 0.0;
			instance.EmitHitpointsEmpty();
		}
		return num - num2;
	}

	public bool ShieldBlockLethal()
	{
		if (instance.hitpoints <= 0.0)
		{
			return false;
		}
		double num = Mathf.Min(1000.0, instance.hitpoints);
		instance.hitpoints -= num;
		_ShieldUpdateLabel();
		if (instance.hitpoints <= 0.0)
		{
			instance.hitpoints = 0.0;
			instance.EmitHitpointsEmpty();
		}
		return true;
	}

	public bool ShieldBlockCharm()
	{
		return ShieldBlockLethal();
	}

	public void ShieldDeflateVehicle(TowerDefenseCharacter attacker)
	{
		if (GodotObject.IsInstanceValid(attacker) && attacker is TowerDefenseZombie towerDefenseZombie && towerDefenseZombie.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.CAR)
		{
			towerDefenseZombie.Die();
		}
	}

	public void _ShieldUpdateAppearance()
	{
		AdobeAnimateSpriteBase adobeAnimateSpriteBase = sprite as AdobeAnimateSpriteBase;
		if (GodotObject.IsInstanceValid(adobeAnimateSpriteBase))
		{
			adobeAnimateSpriteBase.SetFliters(new Array { "body", "body2", "left", "left2", "right", "right2", "TX", "TX2" }, open: false);
			if ((string?)_shieldType == "MG")
			{
				adobeAnimateSpriteBase.SetFliters(new Array { "body2", "left2", "right2", "TX2" }, open: true);
			}
			else
			{
				adobeAnimateSpriteBase.SetFliters(new Array { "body", "left", "right", "TX" }, open: true);
			}
		}
	}

	public static void CreateOnCell(TowerDefenseCellInstance cell, StringName newShieldType = null, int layers = 1)
	{
		CreateOnCellWithHP(cell, newShieldType, (double)layers * 1000.0);
	}

	public static void CreateOnCellWithHP(TowerDefenseCellInstance cell, StringName newShieldType, double hp)
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		if ((object)newShieldType == null)
		{
			newShieldType = new StringName("Default");
		}
		if (GodotObject.IsInstanceValid(cell.itemShield))
		{
			cell.itemShield.ShieldAddHitpoints(hp, newShieldType);
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ItemSheild");
		if (packetConfig == null)
		{
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(cell.gridPos, playAudio: false, noLimit: true);
		TowerDefenseItemSheild shield = towerDefenseCharacter as TowerDefenseItemSheild;
		if (shield == null)
		{
			return;
		}
		shield.shieldType = newShieldType;
		shield.shieldHitpoints = hp;
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(shield))
			{
				shield.instance.hitpoints = Mathf.Min(hp, 8000.0);
				shield._ShieldUpdateLabel();
			}
		}).CallDeferred();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RefreshLabelIfHitpointsChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._RefreshProjectileTargetability, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._HasGroundProjectileCarrier, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._CanCarryGroundProjectile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPendingShieldHitpoints, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ShieldUpdateLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShieldGetLayerCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShieldAddHitpoints, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "newShieldType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShieldAbsorbDamage, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShieldBlockLethal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShieldBlockCharm, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShieldDeflateVehicle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "attacker", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName._ShieldUpdateAppearance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateOnCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "newShieldType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateOnCellWithHP, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "newShieldType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "hp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._RefreshLabelIfHitpointsChanged && args.Count == 0)
		{
			_RefreshLabelIfHitpointsChanged();
			ret = default;
			return true;
		}
		if (method == MethodName._RefreshProjectileTargetability && args.Count == 0)
		{
			_RefreshProjectileTargetability();
			ret = default;
			return true;
		}
		if (method == MethodName._HasGroundProjectileCarrier && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(_HasGroundProjectileCarrier());
			return true;
		}
		if (method == MethodName._CanCarryGroundProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(_CanCarryGroundProjectile(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyPendingShieldHitpoints && args.Count == 0)
		{
			ApplyPendingShieldHitpoints();
			ret = default;
			return true;
		}
		if (method == MethodName._ShieldUpdateLabel && args.Count == 0)
		{
			_ShieldUpdateLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.ShieldGetLayerCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ShieldGetLayerCount());
			return true;
		}
		if (method == MethodName.ShieldAddHitpoints && args.Count == 2)
		{
			ShieldAddHitpoints(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShieldAbsorbDamage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ShieldAbsorbDamage(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.ShieldBlockLethal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShieldBlockLethal());
			return true;
		}
		if (method == MethodName.ShieldBlockCharm && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShieldBlockCharm());
			return true;
		}
		if (method == MethodName.ShieldDeflateVehicle && args.Count == 1)
		{
			ShieldDeflateVehicle(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ShieldUpdateAppearance && args.Count == 0)
		{
			_ShieldUpdateAppearance();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateOnCell && args.Count == 3)
		{
			CreateOnCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateOnCellWithHP && args.Count == 3)
		{
			CreateOnCellWithHP(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateOnCell && args.Count == 3)
		{
			CreateOnCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateOnCellWithHP && args.Count == 3)
		{
			CreateOnCellWithHP(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
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
		if (method == MethodName._RefreshLabelIfHitpointsChanged)
		{
			return true;
		}
		if (method == MethodName._RefreshProjectileTargetability)
		{
			return true;
		}
		if (method == MethodName._HasGroundProjectileCarrier)
		{
			return true;
		}
		if (method == MethodName._CanCarryGroundProjectile)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingShieldHitpoints)
		{
			return true;
		}
		if (method == MethodName._ShieldUpdateLabel)
		{
			return true;
		}
		if (method == MethodName.ShieldGetLayerCount)
		{
			return true;
		}
		if (method == MethodName.ShieldAddHitpoints)
		{
			return true;
		}
		if (method == MethodName.ShieldAbsorbDamage)
		{
			return true;
		}
		if (method == MethodName.ShieldBlockLethal)
		{
			return true;
		}
		if (method == MethodName.ShieldBlockCharm)
		{
			return true;
		}
		if (method == MethodName.ShieldDeflateVehicle)
		{
			return true;
		}
		if (method == MethodName._ShieldUpdateAppearance)
		{
			return true;
		}
		if (method == MethodName.CreateOnCell)
		{
			return true;
		}
		if (method == MethodName.CreateOnCellWithHP)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.shieldType)
		{
			shieldType = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.shieldHitpoints)
		{
			shieldHitpoints = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._shieldType)
		{
			_shieldType = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._pendingShieldHitpoints)
		{
			_pendingShieldHitpoints = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._layerLabel)
		{
			_layerLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._lastLabelHitpoints)
		{
			_lastLabelHitpoints = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.shieldType)
		{
			value = VariantUtils.CreateFrom<StringName>(shieldType);
			return true;
		}
		if (name == PropertyName.shieldHitpoints)
		{
			value = VariantUtils.CreateFrom<double>(shieldHitpoints);
			return true;
		}
		if (name == PropertyName._shieldType)
		{
			value = VariantUtils.CreateFrom(in _shieldType);
			return true;
		}
		if (name == PropertyName._pendingShieldHitpoints)
		{
			value = VariantUtils.CreateFrom(in _pendingShieldHitpoints);
			return true;
		}
		if (name == PropertyName._layerLabel)
		{
			value = VariantUtils.CreateFrom(in _layerLabel);
			return true;
		}
		if (name == PropertyName._lastLabelHitpoints)
		{
			value = VariantUtils.CreateFrom(in _lastLabelHitpoints);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName._shieldType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.shieldType, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingShieldHitpoints, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.shieldHitpoints, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._layerLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._lastLabelHitpoints, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.shieldType, Variant.From<StringName>(shieldType));
		info.AddProperty(PropertyName.shieldHitpoints, Variant.From<double>(shieldHitpoints));
		info.AddProperty(PropertyName._shieldType, Variant.From(in _shieldType));
		info.AddProperty(PropertyName._pendingShieldHitpoints, Variant.From(in _pendingShieldHitpoints));
		info.AddProperty(PropertyName._layerLabel, Variant.From(in _layerLabel));
		info.AddProperty(PropertyName._lastLabelHitpoints, Variant.From(in _lastLabelHitpoints));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.shieldType, out var value))
		{
			shieldType = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.shieldHitpoints, out var value2))
		{
			shieldHitpoints = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._shieldType, out var value3))
		{
			_shieldType = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._pendingShieldHitpoints, out var value4))
		{
			_pendingShieldHitpoints = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._layerLabel, out var value5))
		{
			_layerLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._lastLabelHitpoints, out var value6))
		{
			_lastLabelHitpoints = value6.As<double>();
		}
	}
}
