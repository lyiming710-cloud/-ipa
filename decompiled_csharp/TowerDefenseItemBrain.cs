using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Item/Brain/Scene/TowerDefenseItemBrain.cs")]
public class TowerDefenseItemBrain : TowerDefenseItem
{
	public delegate void BrainDestroyEventHandler(TowerDefenseItemBrain brain);

	public new class MethodName : TowerDefenseItem.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName DestroySet = "DestroySet";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	public bool over;

	public event BrainDestroyEventHandler OnBrainDestroy;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
			if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
			{
				base.groundHeightComponent.SetAlive(alive: false);
			}
			AddToGroup("Brain", persistent: true);
			targetRegistrationComponent.attackGridColumnAliasOffset = 1;
			targetRegistrationComponent.canProjectileCheck = false;
		}
	}

	public override async void DestroySet()
	{
		if (over)
		{
			return;
		}
		over = true;
		OnBrainDestroy?.Invoke(this);
		if ((Global.IsEditor && Global.Instance.enterLevelMode == "DiyLevel") || Global.Instance.enterLevelMode == "LoadLevel" || Global.Instance.enterLevelMode == "OnlineLevel")
		{
			return;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		double num = GD.Randf();
		if (num <= 0.02)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_DIAMOND, logicalGlobalPosition, 70.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			if (GodotObject.IsInstanceValid(towerDefenseGroundItemBase))
			{
				towerDefenseGroundItemBase.gridPos = gridPos;
			}
		}
		else if (num < 0.2)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase2 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_GOLD, logicalGlobalPosition, 70.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			if (GodotObject.IsInstanceValid(towerDefenseGroundItemBase2))
			{
				towerDefenseGroundItemBase2.gridPos = gridPos;
			}
		}
		else
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase3 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_SILVER, logicalGlobalPosition, 70.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			if (GodotObject.IsInstanceValid(towerDefenseGroundItemBase3))
			{
				towerDefenseGroundItemBase3.gridPos = gridPos;
			}
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
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
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.over, Variant.From(in over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.over, out var value))
		{
			over = value.As<bool>();
		}
	}
}
