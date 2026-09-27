using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Sun/Magic/TowerDefenseSunMagic.cs")]
public class TowerDefenseSunMagic : TowerDefenseSunBase
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

		public static readonly StringName Purify = "Purify";

		public static readonly StringName GetSunCamp = "GetSunCamp";

		public static readonly StringName PurifyCell = "PurifyCell";

		public static readonly StringName AlignPlantHypnoses = "AlignPlantHypnoses";

		public static readonly StringName HasHypnosesBuff = "HasHypnosesBuff";

		public static readonly StringName RemoveLadder = "RemoveLadder";

		public static readonly StringName RemoveAbnormalBuffs = "RemoveAbnormalBuffs";

		public static readonly StringName HealPurifiedPlant = "HealPurifiedPlant";

		public static readonly StringName CreatePurifiedSun = "CreatePurifiedSun";

		public static readonly StringName PlayPurifyEffect = "PlayPurifyEffect";
	}

	public new class PropertyName : TowerDefenseSunBase.PropertyName
	{
		public static readonly StringName zombieCamp = "zombieCamp";
	}

	public new class SignalName : TowerDefenseSunBase.SignalName
	{
	}

	private const string PurifyEffectScenePath = "res://Asset/Anime/Effect/MagicSunBuff/MagicSunBuff.tscn";

	private const string PurifyEffectClip = "Fire";

	private const long PurifyCreateNum = 25L;

	private const double PurifyHealPercentage = 0.1;

	private static readonly string[] PurifyBuffKeys = new string[16]
	{
		"Frozen", "IceSpeedDown", "EMSpeedDown", "TimeMagic", "AttackSpeedDown", "Butter", "EMP", "Dizziness", "MagicImmobilize", "Sleep",
		"Squid", "Poisoning", "RedHeat", "Cherry", "Burn", "TabooBean"
	};

	private static PackedScene _purifyEffectScene;

	public bool zombieCamp;

	public override void _Ready()
	{
		base._Ready();
		dieDownTimer.Timeout += DieDown;
	}

	public override string GetGroupName()
	{
		return "MagicSun";
	}

	public override ObjectManagerConfig.OBJECT GetPoolKey()
	{
		return ObjectManagerConfig.OBJECT.SUN_MAGIC;
	}

	public override void OnCollectStart()
	{
		if (gridPos.X < 0 || gridPos.Y < 0)
		{
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(sprite.GlobalPosition);
		}
		Purify();
	}

	public override long GetCollectValue()
	{
		if (!zombieCamp)
		{
			return sunNum;
		}
		return -sunNum;
	}

	public override bool ShouldAutoCollect()
	{
		autoCollect = false;
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
		zombieCamp = false;
	}

	public void Purify()
	{
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				PurifyCell(TowerDefenseManager.GetMapCell(gridPos + new Vector2I(i, j)));
			}
		}
	}

	private TowerDefenseEnum.CHARACTER_CAMP GetSunCamp()
	{
		if (!zombieCamp)
		{
			return TowerDefenseEnum.CHARACTER_CAMP.PLANT;
		}
		return TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
	}

	private void PurifyCell(TowerDefenseCellInstance cell)
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		List<TowerDefensePlant> list = CollectPurifyTargets(cell);
		if (list.Count == 0)
		{
			return;
		}
		bool flag = RemoveLadder(cell);
		TowerDefenseEnum.CHARACTER_CAMP sunCamp = GetSunCamp();
		for (int i = 0; i < list.Count; i++)
		{
			TowerDefensePlant towerDefensePlant = list[i];
			if (GodotObject.IsInstanceValid(towerDefensePlant) && !towerDefensePlant.isDestroy && !towerDefensePlant.die)
			{
				bool flag2 = AlignPlantHypnoses(towerDefensePlant, sunCamp);
				if (GodotObject.IsInstanceValid(towerDefensePlant) && towerDefensePlant.camp == sunCamp)
				{
					flag2 |= RemoveAbnormalBuffs(towerDefensePlant);
				}
				if (flag2 || flag)
				{
					HealPurifiedPlant(towerDefensePlant);
					CreatePurifiedSun(towerDefensePlant);
					PlayPurifyEffect(towerDefensePlant);
				}
			}
		}
	}

	private List<TowerDefensePlant> CollectPurifyTargets(TowerDefenseCellInstance cell)
	{
		List<TowerDefensePlant> list = new List<TowerDefensePlant>();
		List<TowerDefenseCharacter> characterList = cell.characterList;
		for (int i = 0; i < characterList.Count; i++)
		{
			if (characterList[i] is TowerDefensePlant towerDefensePlant && !(towerDefensePlant is TowerDefensePlantBowlingBase))
			{
				list.Add(towerDefensePlant);
			}
		}
		return list;
	}

	private static bool AlignPlantHypnoses(TowerDefensePlant plant, TowerDefenseEnum.CHARACTER_CAMP sunCamp)
	{
		bool flag = sunCamp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
		bool flag2 = HasHypnosesBuff(plant);
		if (flag2 == flag)
		{
			return false;
		}
		plant.Hypnoses();
		if (!GodotObject.IsInstanceValid(plant))
		{
			return false;
		}
		return HasHypnosesBuff(plant) != flag2;
	}

	private static bool HasHypnosesBuff(TowerDefensePlant plant)
	{
		BuffComponent buff = plant.buff;
		if (buff != null && !buff.IsReleased)
		{
			return buff.BuffHas("Hypnoses");
		}
		return false;
	}

	private static bool RemoveLadder(TowerDefenseCellInstance cell)
	{
		TowerDefenseCharacter characterLadder = cell.characterLadder;
		if (!GodotObject.IsInstanceValid(characterLadder))
		{
			return false;
		}
		characterLadder.Destroy();
		return true;
	}

	private static bool RemoveAbnormalBuffs(TowerDefensePlant plant)
	{
		BuffComponent buff = plant.buff;
		if (buff == null || buff.IsReleased)
		{
			return false;
		}
		bool result = false;
		for (int i = 0; i < PurifyBuffKeys.Length; i++)
		{
			string key = PurifyBuffKeys[i];
			if (buff.BuffHas(key))
			{
				buff.DeleteBuff(key);
				result = true;
			}
		}
		return result;
	}

	private static void HealPurifiedPlant(TowerDefensePlant plant)
	{
		TowerDefenseCharacterInstance instance = plant.instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		double hitpointsSave = instance.hitpointsSave;
		if (hitpointsSave > 0.0)
		{
			double num = Math.Min(hitpointsSave * 0.1, hitpointsSave - instance.hitpoints);
			if (num > 0.0)
			{
				plant.Health(num);
			}
		}
	}

	private void CreatePurifiedSun(TowerDefensePlant plant)
	{
		TowerDefenseCharacterEventSunCreate.Run(plant, 25.0, _dieCreate: false, _mustForzen: false, _fromPacket: false, 1.0, _byCamp: false, zombieCamp);
	}

	private static void PlayPurifyEffect(TowerDefensePlant plant)
	{
		if (_purifyEffectScene == null)
		{
			_purifyEffectScene = GD.Load<PackedScene>("res://Asset/Anime/Effect/MagicSunBuff/MagicSunBuff.tscn");
		}
		if (!GodotObject.IsInstanceValid(_purifyEffectScene))
		{
			return;
		}
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(_purifyEffectScene, plant.gridPos, "Fire");
		if (GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce))
		{
			Node2D characterNode = TowerDefenseManager.GetCharacterNode();
			if (!GodotObject.IsInstanceValid(characterNode))
			{
				towerDefenseEffectSpriteOnce.QueueFree();
				return;
			}
			characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			towerDefenseEffectSpriteOnce.gridPos = plant.gridPos;
			towerDefenseEffectSpriteOnce.GlobalPosition = plant.GetLogicalGlobalPosition();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
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
			new MethodInfo(MethodName.Purify, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSunCamp, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PurifyCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AlignPlantHypnoses, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "sunCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasHypnosesBuff, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveLadder, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveAbnormalBuffs, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HealPurifiedPlant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePurifiedSun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlayPurifyEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.Purify && args.Count == 0)
		{
			Purify();
			ret = default;
			return true;
		}
		if (method == MethodName.GetSunCamp && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEnum.CHARACTER_CAMP>(GetSunCamp());
			return true;
		}
		if (method == MethodName.PurifyCell && args.Count == 1)
		{
			PurifyCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AlignPlantHypnoses && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AlignPlantHypnoses(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.HasHypnosesBuff && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasHypnosesBuff(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveLadder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveLadder(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveAbnormalBuffs && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveAbnormalBuffs(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0])));
			return true;
		}
		if (method == MethodName.HealPurifiedPlant && args.Count == 1)
		{
			HealPurifiedPlant(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePurifiedSun && args.Count == 1)
		{
			CreatePurifiedSun(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayPurifyEffect && args.Count == 1)
		{
			PlayPurifyEffect(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AlignPlantHypnoses && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AlignPlantHypnoses(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.HasHypnosesBuff && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasHypnosesBuff(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveLadder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveLadder(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveAbnormalBuffs && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveAbnormalBuffs(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0])));
			return true;
		}
		if (method == MethodName.HealPurifiedPlant && args.Count == 1)
		{
			HealPurifiedPlant(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayPurifyEffect && args.Count == 1)
		{
			PlayPurifyEffect(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]));
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
		if (method == MethodName.Purify)
		{
			return true;
		}
		if (method == MethodName.GetSunCamp)
		{
			return true;
		}
		if (method == MethodName.PurifyCell)
		{
			return true;
		}
		if (method == MethodName.AlignPlantHypnoses)
		{
			return true;
		}
		if (method == MethodName.HasHypnosesBuff)
		{
			return true;
		}
		if (method == MethodName.RemoveLadder)
		{
			return true;
		}
		if (method == MethodName.RemoveAbnormalBuffs)
		{
			return true;
		}
		if (method == MethodName.HealPurifiedPlant)
		{
			return true;
		}
		if (method == MethodName.CreatePurifiedSun)
		{
			return true;
		}
		if (method == MethodName.PlayPurifyEffect)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.zombieCamp)
		{
			zombieCamp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.zombieCamp)
		{
			value = VariantUtils.CreateFrom(in zombieCamp);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.zombieCamp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.zombieCamp, Variant.From(in zombieCamp));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.zombieCamp, out var value))
		{
			zombieCamp = value.As<bool>();
		}
	}
}
