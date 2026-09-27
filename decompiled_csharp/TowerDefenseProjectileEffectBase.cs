using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Projectile/ProjectileEffect/TowerDefenseProjectileEffectBase.cs")]
public class TowerDefenseProjectileEffectBase : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName ConfigureNetworkReplay = "ConfigureNetworkReplay";

		public static readonly StringName ConfigureNetworkVariant = "ConfigureNetworkVariant";

		public static readonly StringName PrepareNetworkEffect = "PrepareNetworkEffect";

		public static readonly StringName EnsureRandomSeed = "EnsureRandomSeed";

		public static readonly StringName CraterCreate = "CraterCreate";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName Eventlist = "Eventlist";

		public static readonly StringName NetworkEffectId = "NetworkEffectId";

		public static readonly StringName NetworkEffectVariant = "NetworkEffectVariant";

		public static readonly StringName NetworkRandomSeed = "NetworkRandomSeed";

		public static readonly StringName NetworkVisualOnly = "NetworkVisualOnly";

		public static readonly StringName gridPos = "gridPos";

		public static readonly StringName camp = "camp";

		public static readonly StringName collisionFlag = "collisionFlag";

		public static readonly StringName target = "target";

		public static readonly StringName height = "height";

		public static readonly StringName suppressDeathrattles = "suppressDeathrattles";

		public static readonly StringName _networkReplay = "_networkReplay";

		public static readonly StringName _networkSeedConfigured = "_networkSeedConfigured";

		public static readonly StringName _networkPrepared = "_networkPrepared";

		public static readonly StringName _networkRandomSeed = "_networkRandomSeed";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	public Vector2I gridPos;

	public TowerDefenseEnum.CHARACTER_CAMP camp;

	public int collisionFlag;

	public TowerDefenseCharacter target;

	public double height;

	public bool suppressDeathrattles;

	private bool _networkReplay;

	private bool _networkSeedConfigured;

	private bool _networkPrepared;

	private ulong _networkRandomSeed;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> Eventlist { get; set; }

	public virtual string NetworkEffectId => "";

	public virtual string NetworkEffectVariant => "";

	public ulong NetworkRandomSeed => _networkRandomSeed;

	public bool NetworkVisualOnly => _networkReplay;

	public virtual void Init(TowerDefenseProjectile projectile)
	{
	}

	public void Init(Vector2I _gridPos, TowerDefenseEnum.CHARACTER_CAMP _camp, int _collisionFlag, TowerDefenseCharacter _target, double _height = 0.0)
	{
		gridPos = _gridPos;
		camp = _camp;
		collisionFlag = _collisionFlag;
		target = _target;
		height = _height;
	}

	public void ConfigureNetworkReplay(ulong randomSeed)
	{
		_networkReplay = true;
		_networkSeedConfigured = true;
		_networkRandomSeed = randomSeed;
	}

	public virtual void ConfigureNetworkVariant(string variant)
	{
	}

	protected bool PrepareNetworkEffect()
	{
		if (_networkPrepared)
		{
			if (_networkReplay)
			{
				return IsInsideTree();
			}
			return true;
		}
		_networkPrepared = true;
		string networkEffectId = NetworkEffectId;
		if (networkEffectId == "")
		{
			EnsureRandomSeed();
			return true;
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost && !_networkReplay)
		{
			QueueFree();
			return false;
		}
		EnsureRandomSeed();
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && !_networkReplay && GodotObject.IsInstanceValid(MultiPlayerManager.Instance))
		{
			MultiPlayerManager.Instance.SendProjectileEffectSpawn(new ProjectileEffectSpawnDto
			{
				effect_id = networkEffectId,
				variant = NetworkEffectVariant,
				random_seed = _networkRandomSeed,
				px = GlobalPosition.X,
				py = GlobalPosition.Y,
				grid_x = gridPos.X,
				grid_y = gridPos.Y,
				camp = (int)camp,
				collision_flags = collisionFlag,
				height = height
			});
		}
		return true;
	}

	private void EnsureRandomSeed()
	{
		if (!_networkSeedConfigured)
		{
			_networkSeedConfigured = true;
			_networkRandomSeed = GD.Randi();
		}
	}

	public void CraterCreate(bool nolimit = false, string craterName = "CraterDayGround", bool halfCrater = false)
	{
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		if (GodotObject.IsInstanceValid(mapCell) && (nolimit || mapCell.CanCraterCreate()))
		{
			if (TowerDefenseManager.GetPacketConfig(craterName).Plant(gridPos, playAudio: false, noLimit: true) is TowerDefenseCrater towerDefenseCrater && halfCrater)
			{
				towerDefenseCrater.halfCrater = true;
			}
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				MultiPlayerManager.Instance.SendCraterCreate(gridPos.X, gridPos.Y, craterName);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "_gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_collisionFlag", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "_height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureNetworkReplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "randomSeed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureNetworkVariant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "variant", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareNetworkEffect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureRandomSeed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CraterCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "nolimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "craterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "halfCrater", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 5)
		{
			Init(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureNetworkReplay && args.Count == 1)
		{
			ConfigureNetworkReplay(VariantUtils.ConvertTo<ulong>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureNetworkVariant && args.Count == 1)
		{
			ConfigureNetworkVariant(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareNetworkEffect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(PrepareNetworkEffect());
			return true;
		}
		if (method == MethodName.EnsureRandomSeed && args.Count == 0)
		{
			EnsureRandomSeed();
			ret = default;
			return true;
		}
		if (method == MethodName.CraterCreate && args.Count == 3)
		{
			CraterCreate(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.ConfigureNetworkReplay)
		{
			return true;
		}
		if (method == MethodName.ConfigureNetworkVariant)
		{
			return true;
		}
		if (method == MethodName.PrepareNetworkEffect)
		{
			return true;
		}
		if (method == MethodName.EnsureRandomSeed)
		{
			return true;
		}
		if (method == MethodName.CraterCreate)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Eventlist)
		{
			Eventlist = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			gridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.camp)
		{
			camp = VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in value);
			return true;
		}
		if (name == PropertyName.collisionFlag)
		{
			collisionFlag = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.target)
		{
			target = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.height)
		{
			height = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.suppressDeathrattles)
		{
			suppressDeathrattles = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._networkReplay)
		{
			_networkReplay = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._networkSeedConfigured)
		{
			_networkSeedConfigured = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._networkPrepared)
		{
			_networkPrepared = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._networkRandomSeed)
		{
			_networkRandomSeed = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Eventlist)
		{
			value = VariantUtils.CreateFromArray(Eventlist);
			return true;
		}
		string from;
		if (name == PropertyName.NetworkEffectId)
		{
			from = NetworkEffectId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.NetworkEffectVariant)
		{
			from = NetworkEffectVariant;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.NetworkRandomSeed)
		{
			value = VariantUtils.CreateFrom<ulong>(NetworkRandomSeed);
			return true;
		}
		if (name == PropertyName.NetworkVisualOnly)
		{
			value = VariantUtils.CreateFrom<bool>(NetworkVisualOnly);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			value = VariantUtils.CreateFrom(in gridPos);
			return true;
		}
		if (name == PropertyName.camp)
		{
			value = VariantUtils.CreateFrom(in camp);
			return true;
		}
		if (name == PropertyName.collisionFlag)
		{
			value = VariantUtils.CreateFrom(in collisionFlag);
			return true;
		}
		if (name == PropertyName.target)
		{
			value = VariantUtils.CreateFrom(in target);
			return true;
		}
		if (name == PropertyName.height)
		{
			value = VariantUtils.CreateFrom(in height);
			return true;
		}
		if (name == PropertyName.suppressDeathrattles)
		{
			value = VariantUtils.CreateFrom(in suppressDeathrattles);
			return true;
		}
		if (name == PropertyName._networkReplay)
		{
			value = VariantUtils.CreateFrom(in _networkReplay);
			return true;
		}
		if (name == PropertyName._networkSeedConfigured)
		{
			value = VariantUtils.CreateFrom(in _networkSeedConfigured);
			return true;
		}
		if (name == PropertyName._networkPrepared)
		{
			value = VariantUtils.CreateFrom(in _networkPrepared);
			return true;
		}
		if (name == PropertyName._networkRandomSeed)
		{
			value = VariantUtils.CreateFrom(in _networkRandomSeed);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.Eventlist, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.camp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionFlag, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.target, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.height, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.suppressDeathrattles, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._networkReplay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._networkSeedConfigured, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._networkPrepared, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._networkRandomSeed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.NetworkEffectId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.NetworkEffectVariant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.NetworkRandomSeed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.NetworkVisualOnly, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Eventlist, Variant.CreateFrom(Eventlist));
		info.AddProperty(PropertyName.gridPos, Variant.From(in gridPos));
		info.AddProperty(PropertyName.camp, Variant.From(in camp));
		info.AddProperty(PropertyName.collisionFlag, Variant.From(in collisionFlag));
		info.AddProperty(PropertyName.target, Variant.From(in target));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.suppressDeathrattles, Variant.From(in suppressDeathrattles));
		info.AddProperty(PropertyName._networkReplay, Variant.From(in _networkReplay));
		info.AddProperty(PropertyName._networkSeedConfigured, Variant.From(in _networkSeedConfigured));
		info.AddProperty(PropertyName._networkPrepared, Variant.From(in _networkPrepared));
		info.AddProperty(PropertyName._networkRandomSeed, Variant.From(in _networkRandomSeed));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Eventlist, out var value))
		{
			Eventlist = value.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.gridPos, out var value2))
		{
			gridPos = value2.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.camp, out var value3))
		{
			camp = value3.As<TowerDefenseEnum.CHARACTER_CAMP>();
		}
		if (info.TryGetProperty(PropertyName.collisionFlag, out var value4))
		{
			collisionFlag = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.target, out var value5))
		{
			target = value5.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value6))
		{
			height = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.suppressDeathrattles, out var value7))
		{
			suppressDeathrattles = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._networkReplay, out var value8))
		{
			_networkReplay = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._networkSeedConfigured, out var value9))
		{
			_networkSeedConfigured = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._networkPrepared, out var value10))
		{
			_networkPrepared = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._networkRandomSeed, out var value11))
		{
			_networkRandomSeed = value11.As<ulong>();
		}
	}
}
