using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/MagnetPumpkin/Scene/TowerDefensePlantMagnetPumpkin.cs")]
public class TowerDefensePlantMagnetPumpkin : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName RefreshNearbyMagnetSubscriptions = "RefreshNearbyMagnetSubscriptions";

		public static readonly StringName UnsubscribePlant = "UnsubscribePlant";

		public static readonly StringName UnsubscribeAll = "UnsubscribeAll";

		public static readonly StringName OnSelfIronBreakDown = "OnSelfIronBreakDown";

		public static readonly StringName OnNearbyIronBreakDown = "OnNearbyIronBreakDown";

		public static readonly StringName GenerateShieldShield = "GenerateShieldShield";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _refreshTimer = "_refreshTimer";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static readonly StringName ShieldTypeMG = new StringName("MG");

	private MagnetComponent _magnetComponent;

	private readonly HashSet<TowerDefenseCharacter> _subscribedMagnetPlants = new HashSet<TowerDefenseCharacter>();

	private float _refreshTimer;

	private const float REFRESH_INTERVAL = 1f;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_magnetComponent = componentManager.GetRuntime<MagnetComponent>();
			MagnetComponent magnetComponent = _magnetComponent;
			if (magnetComponent != null && !magnetComponent.IsReleased)
			{
				_magnetComponent.OnBreakDown += OnSelfIronBreakDown;
			}
		}
	}

	public override void _ExitTree()
	{
		MagnetComponent magnetComponent = _magnetComponent;
		if (magnetComponent != null && !magnetComponent.IsReleased)
		{
			_magnetComponent.OnBreakDown -= OnSelfIronBreakDown;
		}
		UnsubscribeAll();
		base._ExitTree();
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && GodotObject.IsInstanceValid(instance))
		{
			_refreshTimer += (float)delta;
			if (_refreshTimer >= 1f)
			{
				_refreshTimer = 0f;
				RefreshNearbyMagnetSubscriptions();
			}
		}
	}

	private void RefreshNearbyMagnetSubscriptions()
	{
		if (!GodotObject.IsInstanceValid(cell) || !GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
		HashSet<TowerDefenseCharacter> hashSet = new HashSet<TowerDefenseCharacter>();
		foreach (Variant item in TowerDefenseManager.Instance.GetCampFriendly(camp))
		{
			TowerDefenseCharacter towerDefenseCharacter = item.As<TowerDefenseCharacter>();
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter?.instance) || towerDefenseCharacter.instance == instance || towerDefenseCharacter.isDestroy || towerDefenseCharacter.die)
			{
				continue;
			}
			Vector2 vector = (towerDefenseCharacter.GetLogicalGlobalPosition() - logicalGlobalPosition).Abs();
			if (!(vector.X > mapGridSize.X) && !(vector.Y > mapGridSize.Y))
			{
				MagnetComponent magnetComponent = towerDefenseCharacter.componentManager?.GetRuntime<MagnetComponent>();
				if (magnetComponent != null && !magnetComponent.IsReleased)
				{
					hashSet.Add(towerDefenseCharacter);
				}
			}
		}
		foreach (TowerDefenseCharacter item2 in hashSet)
		{
			if (!_subscribedMagnetPlants.Contains(item2))
			{
				MagnetComponent runtime = item2.componentManager.GetRuntime<MagnetComponent>();
				if (runtime != null && !runtime.IsReleased)
				{
					runtime.OnBreakDown += OnNearbyIronBreakDown;
					_subscribedMagnetPlants.Add(item2);
				}
			}
		}
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
		foreach (TowerDefenseCharacter subscribedMagnetPlant in _subscribedMagnetPlants)
		{
			if (!hashSet.Contains(subscribedMagnetPlant))
			{
				UnsubscribePlant(subscribedMagnetPlant);
				list.Add(subscribedMagnetPlant);
			}
		}
		foreach (TowerDefenseCharacter item3 in list)
		{
			_subscribedMagnetPlants.Remove(item3);
		}
	}

	private void UnsubscribePlant(TowerDefenseCharacter plant)
	{
		if (GodotObject.IsInstanceValid(plant))
		{
			MagnetComponent magnetComponent = plant.componentManager?.GetRuntime<MagnetComponent>();
			if (magnetComponent != null && !magnetComponent.IsReleased)
			{
				magnetComponent.OnBreakDown -= OnNearbyIronBreakDown;
			}
		}
	}

	private void UnsubscribeAll()
	{
		foreach (TowerDefenseCharacter subscribedMagnetPlant in _subscribedMagnetPlants)
		{
			UnsubscribePlant(subscribedMagnetPlant);
		}
		_subscribedMagnetPlants.Clear();
	}

	private void OnSelfIronBreakDown(TowerDefenseArmorInstance _armor)
	{
		GenerateShieldShield(1000.0);
	}

	private void OnNearbyIronBreakDown(TowerDefenseArmorInstance _armor)
	{
		GenerateShieldShield(500.0);
	}

	private void GenerateShieldShield(double hp)
	{
		if (GodotObject.IsInstanceValid(cell))
		{
			TowerDefenseItemSheild.CreateOnCellWithHP(cell, ShieldTypeMG, hp);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshNearbyMagnetSubscriptions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnsubscribePlant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.UnsubscribeAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSelfIronBreakDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_armor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnNearbyIronBreakDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_armor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateShieldShield, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
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
		if (method == MethodName.RefreshNearbyMagnetSubscriptions && args.Count == 0)
		{
			RefreshNearbyMagnetSubscriptions();
			ret = default;
			return true;
		}
		if (method == MethodName.UnsubscribePlant && args.Count == 1)
		{
			UnsubscribePlant(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnsubscribeAll && args.Count == 0)
		{
			UnsubscribeAll();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSelfIronBreakDown && args.Count == 1)
		{
			OnSelfIronBreakDown(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnNearbyIronBreakDown && args.Count == 1)
		{
			OnNearbyIronBreakDown(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GenerateShieldShield && args.Count == 1)
		{
			GenerateShieldShield(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.RefreshNearbyMagnetSubscriptions)
		{
			return true;
		}
		if (method == MethodName.UnsubscribePlant)
		{
			return true;
		}
		if (method == MethodName.UnsubscribeAll)
		{
			return true;
		}
		if (method == MethodName.OnSelfIronBreakDown)
		{
			return true;
		}
		if (method == MethodName.OnNearbyIronBreakDown)
		{
			return true;
		}
		if (method == MethodName.GenerateShieldShield)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._refreshTimer)
		{
			_refreshTimer = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._refreshTimer)
		{
			value = VariantUtils.CreateFrom(in _refreshTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._refreshTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._refreshTimer, Variant.From(in _refreshTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._refreshTimer, out var value))
		{
			_refreshTimer = value.As<float>();
		}
	}
}
