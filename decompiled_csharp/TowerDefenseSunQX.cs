using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Sun/QX/TowerDefenseSunQX.cs")]
public class TowerDefenseSunQX : TowerDefenseSunBase
{
	public new class MethodName : TowerDefenseSunBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName GetGroupName = "GetGroupName";

		public new static readonly StringName GetPoolKey = "GetPoolKey";

		public new static readonly StringName OnCollectStart = "OnCollectStart";

		public new static readonly StringName GetCollectValue = "GetCollectValue";

		public new static readonly StringName ShouldAutoCollect = "ShouldAutoCollect";

		public new static readonly StringName OnDieDown = "OnDieDown";

		public new static readonly StringName DieDown = "DieDown";

		public new static readonly StringName OnRefresh = "OnRefresh";

		public static readonly StringName Explode = "Explode";

		public static readonly StringName TryApplyPriceChange = "TryApplyPriceChange";

		public static readonly StringName CreatePriceChangeKey = "CreatePriceChangeKey";

		public static readonly StringName HasLockedCost = "HasLockedCost";
	}

	public new class PropertyName : TowerDefenseSunBase.PropertyName
	{
		public static readonly StringName surcharge = "surcharge";
	}

	public new class SignalName : TowerDefenseSunBase.SignalName
	{
	}

	private const int PriceChangeAmount = 25;

	private const string DiscountKey = "QXSunDiscount";

	private const string SurchargeKey = "QXSunSurcharge";

	public bool surcharge;

	public override void _Ready()
	{
		base._Ready();
		dieDownTimer.Timeout += DieDown;
	}

	public override string GetGroupName()
	{
		return "QXSun";
	}

	public override ObjectManagerConfig.OBJECT GetPoolKey()
	{
		return ObjectManagerConfig.OBJECT.SUN_QX;
	}

	public override void OnCollectStart()
	{
		if (gridPos.X < 0 || gridPos.Y < 0)
		{
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(sprite.GlobalPosition);
		}
		Explode();
	}

	public override long GetCollectValue()
	{
		return sunNum;
	}

	public override bool ShouldAutoCollect()
	{
		autoCollect = true;
		return base.ShouldAutoCollect();
	}

	public override bool OnDieDown()
	{
		return false;
	}

	public override void DieDown()
	{
		if (!isCollect)
		{
			if (GameSaveManager.Instance.GetFeatureValue("SunCollect") != 0)
			{
				Collection();
			}
			else
			{
				base.DieDown();
			}
		}
	}

	public override void OnRefresh()
	{
		gridPos = new Vector2I(-1, -1);
		surcharge = false;
	}

	public void Explode()
	{
		if (surcharge)
		{
			TryApplyPriceChange("Increase", 25, "QXSunSurcharge");
		}
		else
		{
			TryApplyPriceChange("Decrease", 25, "QXSunDiscount");
		}
	}

	private void TryApplyPriceChange(string method, int amount, string key)
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		Array<TowerDefenseInGamePacketShow> seedBankList = instance.GetSeedBankList();
		if (seedBankList == null || seedBankList.Count == 0)
		{
			return;
		}
		List<TowerDefenseInGamePacketShow> list = new List<TowerDefenseInGamePacketShow>();
		foreach (TowerDefenseInGamePacketShow item in seedBankList)
		{
			if (GodotObject.IsInstanceValid(item) && item.start)
			{
				TowerDefensePacketConfig config = item.config;
				if (GodotObject.IsInstanceValid(config) && config._GetType() != TowerDefenseEnum.PACKET_TYPE.DIAMOND && config.canChangeCost && !HasLockedCost(config) && (surcharge || (config.GetCostBeforeModifiers() > 0 && config.GetCost() > 0)))
				{
					list.Add(item);
				}
			}
		}
		if (list.Count != 0)
		{
			int index = GD.RandRange(0, list.Count - 1);
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = list[index];
			TowerDefensePacketConfig config2 = towerDefenseInGamePacketShow.config;
			string key2 = CreatePriceChangeKey(key);
			TowerDefensePacketChangeCost changeCost = new TowerDefensePacketChangeCost
			{
				method = method,
				amontDictionary = new Dictionary
				{
					[0] = amount,
					[1] = amount,
					[3] = amount,
					[4] = amount,
					[5] = amount,
					[6] = amount,
					[7] = amount,
					[8] = amount
				},
				key = key2,
				consumeOnPurchase = true
			};
			if (config2.ChangeCostAdd(changeCost))
			{
				towerDefenseInGamePacketShow.RefreshRuntimeState(includeCost: true);
				towerDefenseInGamePacketShow.FlashCostChange();
			}
		}
	}

	private static string CreatePriceChangeKey(string key)
	{
		return $"{key}:{Guid.NewGuid():N}";
	}

	private static bool HasLockedCost(TowerDefensePacketConfig config)
	{
		if (!config.canChangeCost)
		{
			return true;
		}
		if (config.changeCostList == null)
		{
			return false;
		}
		foreach (TowerDefensePacketChangeCost changeCost in config.changeCostList)
		{
			if (changeCost.lockCost)
			{
				return true;
			}
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGroupName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPoolKey, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCollectStart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCollectValue, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldAutoCollect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDieDown, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryApplyPriceChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "method", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "amount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePriceChangeKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasLockedCost, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.GetGroupName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetGroupName());
			return true;
		}
		if (method == MethodName.GetPoolKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ObjectManagerConfig.OBJECT>(GetPoolKey());
			return true;
		}
		if (method == MethodName.OnCollectStart && args.Count == 0)
		{
			OnCollectStart();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCollectValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetCollectValue());
			return true;
		}
		if (method == MethodName.ShouldAutoCollect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldAutoCollect());
			return true;
		}
		if (method == MethodName.OnDieDown && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(OnDieDown());
			return true;
		}
		if (method == MethodName.DieDown && args.Count == 0)
		{
			DieDown();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRefresh && args.Count == 0)
		{
			OnRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.Explode && args.Count == 0)
		{
			Explode();
			ret = default;
			return true;
		}
		if (method == MethodName.TryApplyPriceChange && args.Count == 3)
		{
			TryApplyPriceChange(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePriceChangeKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CreatePriceChangeKey(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasLockedCost && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasLockedCost(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreatePriceChangeKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CreatePriceChangeKey(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasLockedCost && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasLockedCost(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
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
		if (method == MethodName.GetGroupName)
		{
			return true;
		}
		if (method == MethodName.GetPoolKey)
		{
			return true;
		}
		if (method == MethodName.OnCollectStart)
		{
			return true;
		}
		if (method == MethodName.GetCollectValue)
		{
			return true;
		}
		if (method == MethodName.ShouldAutoCollect)
		{
			return true;
		}
		if (method == MethodName.OnDieDown)
		{
			return true;
		}
		if (method == MethodName.DieDown)
		{
			return true;
		}
		if (method == MethodName.OnRefresh)
		{
			return true;
		}
		if (method == MethodName.Explode)
		{
			return true;
		}
		if (method == MethodName.TryApplyPriceChange)
		{
			return true;
		}
		if (method == MethodName.CreatePriceChangeKey)
		{
			return true;
		}
		if (method == MethodName.HasLockedCost)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.surcharge)
		{
			surcharge = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.surcharge)
		{
			value = VariantUtils.CreateFrom(in surcharge);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.surcharge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.surcharge, Variant.From(in surcharge));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.surcharge, out var value))
		{
			surcharge = value.As<bool>();
		}
	}
}
