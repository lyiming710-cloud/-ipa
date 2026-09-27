using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Save/DropItem/TowerDefenseDropItemSaveConfigCSharp.cs")]
public class TowerDefenseDropItemSaveConfigCSharp : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName SaveDropItem = "SaveDropItem";

		public static readonly StringName LoadDropItem = "LoadDropItem";

		public static readonly StringName HasValidNumericState = "HasValidNumericState";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName dropItemType = "dropItemType";

		public static readonly StringName pos = "pos";

		public static readonly StringName sunNum = "sunNum";

		public static readonly StringName economyAccountId = "economyAccountId";

		public static readonly StringName ownershipPolicy = "ownershipPolicy";

		public static readonly StringName movingMethod = "movingMethod";

		public static readonly StringName isCollect = "isCollect";

		public static readonly StringName die = "die";

		public static readonly StringName over = "over";

		public static readonly StringName autoCollect = "autoCollect";

		public static readonly StringName height = "height";

		public static readonly StringName velocity = "velocity";

		public static readonly StringName gravity = "gravity";

		public static readonly StringName ySpeed = "ySpeed";

		public static readonly StringName z = "z";

		public static readonly StringName groundHeight = "groundHeight";

		public static readonly StringName timerTimeLeft = "timerTimeLeft";

		public static readonly StringName spriteSave = "spriteSave";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int dropItemType;

	[Export(PropertyHint.None, "")]
	public Vector2 pos;

	[Export(PropertyHint.None, "")]
	public long sunNum;

	[Export(PropertyHint.None, "")]
	public string economyAccountId = "local";

	[Export(PropertyHint.None, "")]
	public int ownershipPolicy;

	[Export(PropertyHint.None, "")]
	public int movingMethod;

	[Export(PropertyHint.None, "")]
	public bool isCollect;

	[Export(PropertyHint.None, "")]
	public bool die;

	[Export(PropertyHint.None, "")]
	public bool over;

	[Export(PropertyHint.None, "")]
	public bool autoCollect;

	[Export(PropertyHint.None, "")]
	public double height;

	[Export(PropertyHint.None, "")]
	public Vector2 velocity;

	[Export(PropertyHint.None, "")]
	public double gravity;

	[Export(PropertyHint.None, "")]
	public double ySpeed;

	[Export(PropertyHint.None, "")]
	public double z;

	[Export(PropertyHint.None, "")]
	public double groundHeight;

	[Export(PropertyHint.None, "")]
	public double timerTimeLeft;

	[Export(PropertyHint.None, "")]
	public Dictionary spriteSave = new Dictionary();

	public void SaveDropItem(Node dropItem)
	{
		if (spriteSave == null)
		{
			spriteSave = new Dictionary();
		}
		pos = ((Node2D)dropItem).GlobalPosition;
		GD.Print($"[Save] 保存掉落物: pos=({pos.X:F1}, {pos.Y:F1})");
		if (dropItem is TowerDefenseSunBase towerDefenseSunBase)
		{
			sunNum = towerDefenseSunBase.sunNum;
			economyAccountId = towerDefenseSunBase.AccountId.Value;
			ownershipPolicy = (int)towerDefenseSunBase.OwnershipPolicy;
			movingMethod = (int)towerDefenseSunBase.movingMethod;
			isCollect = towerDefenseSunBase.isCollect;
			die = towerDefenseSunBase.die;
			over = towerDefenseSunBase.over;
			autoCollect = towerDefenseSunBase.autoCollect;
			height = towerDefenseSunBase.height;
			if (GodotObject.IsInstanceValid(towerDefenseSunBase.moveComponent))
			{
				velocity = towerDefenseSunBase.moveComponent.velocity;
				gravity = towerDefenseSunBase.moveComponent.gravity;
			}
			if (GodotObject.IsInstanceValid(towerDefenseSunBase.dieDownTimer))
			{
				timerTimeLeft = towerDefenseSunBase.dieDownTimer.TimeLeft;
			}
			if (GodotObject.IsInstanceValid(towerDefenseSunBase.sprite))
			{
				spriteSave = towerDefenseSunBase.sprite.ExportSpriteSave();
			}
			dropItemType = (int)towerDefenseSunBase.GetPoolKey();
			GD.Print($"[Save] 掉落物保存完成: type={dropItemType} sunNum={sunNum} 动画={((spriteSave.Count > 0) ? "有" : "无")}");
		}
	}

	public bool LoadDropItem(TowerDefenseLevelSaveConfigCSharp owner)
	{
		if (spriteSave == null)
		{
			spriteSave = new Dictionary();
		}
		GD.Print($"[Load] 加载掉落物: type={dropItemType} pos=({pos.X:F1}, {pos.Y:F1}) sunNum={sunNum}");
		if (isCollect || die)
		{
			GD.PushWarning($"Skipped inactive saved sun drop: type={dropItemType}.");
			return false;
		}
		ObjectManagerConfig.OBJECT id = (ObjectManagerConfig.OBJECT)dropItemType;
		DropItemConfig byId = DropItemRegistry.GetById(id);
		if (byId == null || byId.Category != TowerDefenseEnum.DROP_ITEM_CATEGORY.SUN)
		{
			GD.PushWarning($"Skipped invalid saved sun drop type: {dropItemType}.");
			return false;
		}
		if (!HasValidNumericState())
		{
			GD.PushWarning($"Skipped saved sun drop with invalid numeric state: type={dropItemType}.");
			return false;
		}
		bool flag = EconomyAccountId.TryParse(economyAccountId, out var accountId);
		bool flag2 = !flag && string.IsNullOrWhiteSpace(economyAccountId);
		if (!flag && !flag2)
		{
			GD.PushWarning("Skipped saved sun drop with malformed account id: " + economyAccountId + ".");
			return false;
		}
		if (flag && !accountId.IsLocal)
		{
			GD.PushWarning($"Skipped remote saved sun drop without a persisted ledger: account={accountId}.");
			return false;
		}
		if (flag2)
		{
			accountId = EconomyAccountId.Local;
		}
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		Node node = ObjectManager.PoolPop(id, characterNode);
		try
		{
			if (node is TowerDefenseSunBase towerDefenseSunBase)
			{
				towerDefenseSunBase.GlobalPosition = pos;
				towerDefenseSunBase.InitFromSave(accountId, (!flag2 && ownershipPolicy == 1) ? SunDropOwnershipPolicy.AccountOwned : SunDropOwnershipPolicy.LegacySharedReplica, sunNum, (TowerDefenseEnum.SUN_MOVING_METHOD)movingMethod, height, velocity, gravity);
				towerDefenseSunBase.isCollect = isCollect;
				towerDefenseSunBase.die = die;
				towerDefenseSunBase.over = over;
				towerDefenseSunBase.RestoreAutoCollect(autoCollect);
				if (timerTimeLeft > 0.0 && GodotObject.IsInstanceValid(towerDefenseSunBase.dieDownTimer))
				{
					towerDefenseSunBase.dieDownTimer.Start(timerTimeLeft);
				}
				if (GodotObject.IsInstanceValid(towerDefenseSunBase.sprite) && spriteSave != null && spriteSave.Count > 0)
				{
					towerDefenseSunBase.sprite.ImportSpriteSave(spriteSave);
				}
				GD.Print($"[Load] 掉落物加载完成: type={dropItemType} sunNum={sunNum} 动画={((spriteSave.Count > 0) ? "已恢复" : "无")}");
				return true;
			}
			if (GodotObject.IsInstanceValid(node))
			{
				GD.PushWarning($"Pool entry {dropItemType} is not a TowerDefenseSunBase; returning it to the pool.");
				ObjectManager.PoolPush(id, node);
			}
			return false;
		}
		catch (Exception ex)
		{
			GD.PushWarning($"Skipped invalid saved sun drop {dropItemType}: {ex.Message}");
			if (GodotObject.IsInstanceValid(node))
			{
				ObjectManager.PoolPush(id, node);
			}
			return false;
		}
	}

	internal bool HasValidNumericState()
	{
		if (float.IsFinite(pos.X) && float.IsFinite(pos.Y) && float.IsFinite(velocity.X) && float.IsFinite(velocity.Y) && double.IsFinite(height) && double.IsFinite(gravity) && double.IsFinite(timerTimeLeft) && timerTimeLeft >= 0.0)
		{
			return Enum.IsDefined(typeof(TowerDefenseEnum.SUN_MOVING_METHOD), movingMethod);
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.SaveDropItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dropItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadDropItem, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasValidNumericState, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SaveDropItem && args.Count == 1)
		{
			SaveDropItem(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadDropItem && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(LoadDropItem(VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[0])));
			return true;
		}
		if (method == MethodName.HasValidNumericState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasValidNumericState());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SaveDropItem)
		{
			return true;
		}
		if (method == MethodName.LoadDropItem)
		{
			return true;
		}
		if (method == MethodName.HasValidNumericState)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dropItemType)
		{
			dropItemType = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.pos)
		{
			pos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			sunNum = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.economyAccountId)
		{
			economyAccountId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ownershipPolicy)
		{
			ownershipPolicy = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.movingMethod)
		{
			movingMethod = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.isCollect)
		{
			isCollect = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.die)
		{
			die = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.autoCollect)
		{
			autoCollect = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.height)
		{
			height = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.velocity)
		{
			velocity = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.gravity)
		{
			gravity = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.ySpeed)
		{
			ySpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.z)
		{
			z = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.groundHeight)
		{
			groundHeight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.timerTimeLeft)
		{
			timerTimeLeft = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spriteSave)
		{
			spriteSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.dropItemType)
		{
			value = VariantUtils.CreateFrom(in dropItemType);
			return true;
		}
		if (name == PropertyName.pos)
		{
			value = VariantUtils.CreateFrom(in pos);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			value = VariantUtils.CreateFrom(in sunNum);
			return true;
		}
		if (name == PropertyName.economyAccountId)
		{
			value = VariantUtils.CreateFrom(in economyAccountId);
			return true;
		}
		if (name == PropertyName.ownershipPolicy)
		{
			value = VariantUtils.CreateFrom(in ownershipPolicy);
			return true;
		}
		if (name == PropertyName.movingMethod)
		{
			value = VariantUtils.CreateFrom(in movingMethod);
			return true;
		}
		if (name == PropertyName.isCollect)
		{
			value = VariantUtils.CreateFrom(in isCollect);
			return true;
		}
		if (name == PropertyName.die)
		{
			value = VariantUtils.CreateFrom(in die);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.autoCollect)
		{
			value = VariantUtils.CreateFrom(in autoCollect);
			return true;
		}
		if (name == PropertyName.height)
		{
			value = VariantUtils.CreateFrom(in height);
			return true;
		}
		if (name == PropertyName.velocity)
		{
			value = VariantUtils.CreateFrom(in velocity);
			return true;
		}
		if (name == PropertyName.gravity)
		{
			value = VariantUtils.CreateFrom(in gravity);
			return true;
		}
		if (name == PropertyName.ySpeed)
		{
			value = VariantUtils.CreateFrom(in ySpeed);
			return true;
		}
		if (name == PropertyName.z)
		{
			value = VariantUtils.CreateFrom(in z);
			return true;
		}
		if (name == PropertyName.groundHeight)
		{
			value = VariantUtils.CreateFrom(in groundHeight);
			return true;
		}
		if (name == PropertyName.timerTimeLeft)
		{
			value = VariantUtils.CreateFrom(in timerTimeLeft);
			return true;
		}
		if (name == PropertyName.spriteSave)
		{
			value = VariantUtils.CreateFrom(in spriteSave);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.dropItemType, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.pos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.economyAccountId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ownershipPolicy, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.movingMethod, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isCollect, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.die, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.autoCollect, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.height, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.velocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.gravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.ySpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.z, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.groundHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.timerTimeLeft, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.spriteSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dropItemType, Variant.From(in dropItemType));
		info.AddProperty(PropertyName.pos, Variant.From(in pos));
		info.AddProperty(PropertyName.sunNum, Variant.From(in sunNum));
		info.AddProperty(PropertyName.economyAccountId, Variant.From(in economyAccountId));
		info.AddProperty(PropertyName.ownershipPolicy, Variant.From(in ownershipPolicy));
		info.AddProperty(PropertyName.movingMethod, Variant.From(in movingMethod));
		info.AddProperty(PropertyName.isCollect, Variant.From(in isCollect));
		info.AddProperty(PropertyName.die, Variant.From(in die));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.autoCollect, Variant.From(in autoCollect));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.velocity, Variant.From(in velocity));
		info.AddProperty(PropertyName.gravity, Variant.From(in gravity));
		info.AddProperty(PropertyName.ySpeed, Variant.From(in ySpeed));
		info.AddProperty(PropertyName.z, Variant.From(in z));
		info.AddProperty(PropertyName.groundHeight, Variant.From(in groundHeight));
		info.AddProperty(PropertyName.timerTimeLeft, Variant.From(in timerTimeLeft));
		info.AddProperty(PropertyName.spriteSave, Variant.From(in spriteSave));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dropItemType, out var value))
		{
			dropItemType = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.pos, out var value2))
		{
			pos = value2.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.sunNum, out var value3))
		{
			sunNum = value3.As<long>();
		}
		if (info.TryGetProperty(PropertyName.economyAccountId, out var value4))
		{
			economyAccountId = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ownershipPolicy, out var value5))
		{
			ownershipPolicy = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.movingMethod, out var value6))
		{
			movingMethod = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.isCollect, out var value7))
		{
			isCollect = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.die, out var value8))
		{
			die = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value9))
		{
			over = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.autoCollect, out var value10))
		{
			autoCollect = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value11))
		{
			height = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.velocity, out var value12))
		{
			velocity = value12.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.gravity, out var value13))
		{
			gravity = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.ySpeed, out var value14))
		{
			ySpeed = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.z, out var value15))
		{
			z = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName.groundHeight, out var value16))
		{
			groundHeight = value16.As<double>();
		}
		if (info.TryGetProperty(PropertyName.timerTimeLeft, out var value17))
		{
			timerTimeLeft = value17.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spriteSave, out var value18))
		{
			spriteSave = value18.As<Dictionary>();
		}
	}
}
