using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter7/Richwood/Scene/TowerDefensePlantRichwood.cs")]
public class TowerDefensePlantRichwood : TowerDefensePlant, IProjectileZone
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName TryStoreProjectileConfig = "TryStoreProjectileConfig";

		public static readonly StringName IsRichwoodOutputConfig = "IsRichwoodOutputConfig";

		public static readonly StringName GetStoredProjectileFireMethodFlags = "GetStoredProjectileFireMethodFlags";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName UpdateRect = "UpdateRect";

		public static readonly StringName OnProjectileIntersect = "OnProjectileIntersect";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName WorldRect = "WorldRect";

		public static readonly StringName GridY = "GridY";

		public static readonly StringName RowSpan = "RowSpan";

		public static readonly StringName _light = "_light";

		public static readonly StringName _zoneRect = "_zoneRect";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static readonly StringName ChestProjectileName = new StringName("Chest");

	private FireComponent _fireComponent;

	private PointLight2D _light;

	private Rect2 _zoneRect;

	private readonly List<BulletFieldStoredProjectile> _projectileList = new List<BulletFieldStoredProjectile>(6);

	public Rect2 WorldRect => _zoneRect;

	public int GridY => gridPos.Y;

	public int RowSpan => 0;

	public override void BatchUpdate(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			base.BatchUpdate(delta);
			_light.Visible = TowerDefenseManager.GetMapIsNight() && GameSaveManager.Instance.GetConfigValue("MapEffect").AsBool();
		}
	}

	private bool TryStoreProjectileConfig(TowerDefenseProjectileConfig projectileConfig, int sourceFireMethodFlags)
	{
		if (projectileConfig == null || IsRichwoodOutputConfig(projectileConfig))
		{
			return false;
		}
		_projectileList.Add(new BulletFieldStoredProjectile(projectileConfig, sourceFireMethodFlags));
		if (_projectileList.Count < 6)
		{
			return true;
		}
		BulletFieldStoredProjectile[] storedProjectiles = _projectileList.ToArray();
		_projectileList.Clear();
		Attack(storedProjectiles);
		return true;
	}

	private static bool IsRichwoodOutputConfig(TowerDefenseProjectileConfig projectileConfig)
	{
		if (projectileConfig != null)
		{
			return projectileConfig.NameSN == ChestProjectileName;
		}
		return false;
	}

	private static int GetStoredProjectileFireMethodFlags(int fireMethodFlags, bool trackOpen)
	{
		if (trackOpen)
		{
			fireMethodFlags |= 0x20;
		}
		return fireMethodFlags;
	}

	public void Attack(BulletFieldStoredProjectile[] storedProjectiles)
	{
		if (_fireComponent != null)
		{
			TowerDefenseProjectileCreateData projectileData = new TowerDefenseProjectileCreateData(ChestProjectileName)
			{
				baseDamage = 100.0
			};
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				gridYOverride = gridPos.Y,
				flipXOverride = (Scale.X < 0f),
				eventProjectiles = storedProjectiles
			};
			_fireComponent.CreateProjectileByData(0, new Vector2(600f, 0f), projectileData, -1, camp, Vector2.Zero, overrides);
		}
	}

	public override async void DestroySet()
	{
		if (!instance.hypnoses)
		{
			TowerDefenseCharacterEventGoldShardCreate towerDefenseCharacterEventGoldShardCreate = new TowerDefenseCharacterEventGoldShardCreate();
			towerDefenseCharacterEventGoldShardCreate.num = 1;
			towerDefenseCharacterEventGoldShardCreate.Execute(GetLogicalGlobalPosition(), this);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			_light = GetNode<PointLight2D>("%Light");
			if (BulletField.Instance != null)
			{
				BulletField.Instance.RegisterZone(this);
			}
		}
	}

	public override void _ExitTree()
	{
		if (BulletField.Instance != null)
		{
			BulletField.Instance.UnregisterZone(this);
		}
		base._ExitTree();
	}

	public void UpdateRect()
	{
		if (IsHitBoxEnabled)
		{
			_zoneRect = WorldHitRect;
		}
		else
		{
			_zoneRect = new Rect2(Vector2.Zero, Vector2.Zero);
		}
	}

	public void OnBulletIntersect(ref BulletData b, int index)
	{
		if (!die && !nearDie && componentAlive && b.active && b.config != null && !IsRichwoodOutputConfig(b.config) && b.camp == camp && (b.checkAll || b.gridPos.Y == gridPos.Y))
		{
			int storedProjectileFireMethodFlags = GetStoredProjectileFireMethodFlags(b.fireMethodFlags, b.trackOpen);
			if ((storedProjectileFireMethodFlags & 2) == 0 && TryStoreProjectileConfig(b.config, storedProjectileFireMethodFlags))
			{
				BulletField.Instance.Despawn(index);
			}
		}
	}

	public void OnProjectileIntersect(TowerDefenseProjectile projectile)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryStoreProjectileConfig, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "sourceFireMethodFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRichwoodOutputConfig, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetStoredProjectileFireMethodFlags, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "fireMethodFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "trackOpen", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateRect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnProjectileIntersect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryStoreProjectileConfig && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryStoreProjectileConfig(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.IsRichwoodOutputConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRichwoodOutputConfig(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetStoredProjectileFireMethodFlags && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetStoredProjectileFireMethodFlags(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
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
		if (method == MethodName.UpdateRect && args.Count == 0)
		{
			UpdateRect();
			ret = default;
			return true;
		}
		if (method == MethodName.OnProjectileIntersect && args.Count == 1)
		{
			OnProjectileIntersect(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsRichwoodOutputConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRichwoodOutputConfig(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetStoredProjectileFireMethodFlags && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetStoredProjectileFireMethodFlags(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.TryStoreProjectileConfig)
		{
			return true;
		}
		if (method == MethodName.IsRichwoodOutputConfig)
		{
			return true;
		}
		if (method == MethodName.GetStoredProjectileFireMethodFlags)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
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
		if (method == MethodName.UpdateRect)
		{
			return true;
		}
		if (method == MethodName.OnProjectileIntersect)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._light)
		{
			_light = VariantUtils.ConvertTo<PointLight2D>(in value);
			return true;
		}
		if (name == PropertyName._zoneRect)
		{
			_zoneRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.WorldRect)
		{
			value = VariantUtils.CreateFrom<Rect2>(WorldRect);
			return true;
		}
		int from;
		if (name == PropertyName.GridY)
		{
			from = GridY;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RowSpan)
		{
			from = RowSpan;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._light)
		{
			value = VariantUtils.CreateFrom(in _light);
			return true;
		}
		if (name == PropertyName._zoneRect)
		{
			value = VariantUtils.CreateFrom(in _zoneRect);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._light, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._zoneRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.WorldRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.GridY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RowSpan, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._light, Variant.From(in _light));
		info.AddProperty(PropertyName._zoneRect, Variant.From(in _zoneRect));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._light, out var value))
		{
			_light = value.As<PointLight2D>();
		}
		if (info.TryGetProperty(PropertyName._zoneRect, out var value2))
		{
			_zoneRect = value2.As<Rect2>();
		}
	}
}
