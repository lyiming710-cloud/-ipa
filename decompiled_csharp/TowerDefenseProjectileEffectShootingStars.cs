using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ProjectileEffect/ShootingStars/TowerDefenseProjectileEffectShootingStars.cs")]
public class TowerDefenseProjectileEffectShootingStars : TowerDefenseProjectileEffectBase
{
	public new class MethodName : TowerDefenseProjectileEffectBase.MethodName
	{
		public new static readonly StringName ConfigureNetworkVariant = "ConfigureNetworkVariant";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : TowerDefenseProjectileEffectBase.PropertyName
	{
		public new static readonly StringName NetworkEffectId = "NetworkEffectId";

		public new static readonly StringName NetworkEffectVariant = "NetworkEffectVariant";

		public static readonly StringName _projectileName = "_projectileName";

		public static readonly StringName _random = "_random";

		public static readonly StringName _normalProjectileData = "_normalProjectileData";

		public static readonly StringName _bigProjectileData = "_bigProjectileData";
	}

	public new class SignalName : TowerDefenseProjectileEffectBase.SignalName
	{
	}

	private string _projectileName = "StarFull";

	private RandomNumberGenerator _random;

	private TowerDefenseProjectileCreateData _normalProjectileData;

	private TowerDefenseProjectileCreateData _bigProjectileData;

	public override string NetworkEffectId => "shooting_stars_rain";

	public override string NetworkEffectVariant => _projectileName;

	public event Action OnCompleted;

	public override void ConfigureNetworkVariant(string variant)
	{
		_projectileName = (string.IsNullOrWhiteSpace(variant) ? "StarFull" : variant);
	}

	public override void _Ready()
	{
		if (PrepareNetworkEffect())
		{
			_random = new RandomNumberGenerator
			{
				Seed = NetworkRandomSeed
			};
			_normalProjectileData = new TowerDefenseProjectileCreateData(new StringName(_projectileName));
			_bigProjectileData = new TowerDefenseProjectileCreateData(new StringName("ShootingStartsBigStarFull"));
			_bigProjectileData.SetScale(new Vector2(2f, 2f));
			CreateRainAsync();
		}
	}

	public override void _ExitTree()
	{
		_random?.Dispose();
		_normalProjectileData?.Dispose();
		_bigProjectileData?.Dispose();
		OnCompleted = null;
	}

	private async Task CreateRainAsync()
	{
		Vector2I gridNum = TowerDefenseManager.Instance.GetMapGridNum();
		Vector2 gridSize = TowerDefenseManager.Instance.GetMapGridSize();
		for (int wave = 0; wave < 25; wave++)
		{
			for (int i = 1; i <= gridNum.X; i++)
			{
				for (int j = 1; j <= gridNum.Y; j++)
				{
					TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(i, j));
					if (GodotObject.IsInstanceValid(mapCell))
					{
						Vector2 pos = TowerDefenseManager.GetMapCellPlantPos(new Vector2I(i, j)) - new Vector2(150f, 0f) + new Vector2(_random.RandfRange((0f - gridSize.X) / 2f, gridSize.X / 2f), 0f);
						double num = _random.RandfRange(0f, 200f);
						bool flag = _random.Randf() < 0.05f;
						BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
						{
							useFall = true,
							gridYOverride = j,
							zOverride = 600.0 + num,
							ySpeedOverride = 400.0,
							suppressGameplay = NetworkVisualOnly
						};
						FireComponent.CreateProjectilePositionByData(null, null, (float)(height + 30.0 - mapCell.GetGroundHeight()), pos, new Vector2(_random.RandfRange(50f, 150f), 0f), flag ? _bigProjectileData : _normalProjectileData, collisionFlag, camp, default, overrides);
					}
				}
			}
			await ToSignal(GetTree().CreateTimer(0.3, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		OnCompleted?.Invoke();
		QueueFree();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.ConfigureNetworkVariant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "variant", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConfigureNetworkVariant && args.Count == 1)
		{
			ConfigureNetworkVariant(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ConfigureNetworkVariant)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._projectileName)
		{
			_projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._random)
		{
			_random = VariantUtils.ConvertTo<RandomNumberGenerator>(in value);
			return true;
		}
		if (name == PropertyName._normalProjectileData)
		{
			_normalProjectileData = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName._bigProjectileData)
		{
			_bigProjectileData = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
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
		if (name == PropertyName._projectileName)
		{
			value = VariantUtils.CreateFrom(in _projectileName);
			return true;
		}
		if (name == PropertyName._random)
		{
			value = VariantUtils.CreateFrom(in _random);
			return true;
		}
		if (name == PropertyName._normalProjectileData)
		{
			value = VariantUtils.CreateFrom(in _normalProjectileData);
			return true;
		}
		if (name == PropertyName._bigProjectileData)
		{
			value = VariantUtils.CreateFrom(in _bigProjectileData);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._random, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._normalProjectileData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bigProjectileData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.NetworkEffectId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.NetworkEffectVariant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._projectileName, Variant.From(in _projectileName));
		info.AddProperty(PropertyName._random, Variant.From(in _random));
		info.AddProperty(PropertyName._normalProjectileData, Variant.From(in _normalProjectileData));
		info.AddProperty(PropertyName._bigProjectileData, Variant.From(in _bigProjectileData));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._projectileName, out var value))
		{
			_projectileName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._random, out var value2))
		{
			_random = value2.As<RandomNumberGenerator>();
		}
		if (info.TryGetProperty(PropertyName._normalProjectileData, out var value3))
		{
			_normalProjectileData = value3.As<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName._bigProjectileData, out var value4))
		{
			_bigProjectileData = value4.As<TowerDefenseProjectileCreateData>();
		}
	}
}
