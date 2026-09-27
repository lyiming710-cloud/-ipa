using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/ChestCoin/Scene/TowerDefenseChestCoin.cs")]
public class TowerDefenseChestCoin : TowerDefenseGravestone
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName DestroySet = "DestroySet";
	}

	public new class PropertyName : TowerDefenseGravestone.PropertyName
	{
	}

	public new class SignalName : TowerDefenseGravestone.SignalName
	{
	}

	private const string CHEST_COIN_WATER_1 = "uid://bdcvatet7ks35";

	private const string CHEST_COIN_WATER_2 = "uid://fmoke6o3c88f";

	private const string CHEST_COIN_WATER_3 = "uid://1vj81vk4dvil";

	private const string CHEST_COIN_WATER_4 = "uid://dtfupjwagghmo";

	private const string CHEST_COIN_WATER_5 = "uid://o6x68dbqcw44";

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			AddToGroup("ChestCoin", persistent: true);
			RemoveFromGroup("Gravestone");
			if (GodotObject.IsInstanceValid(cell) && cell.isWater)
			{
				shadowSprite.Visible = false;
				sprite.SetAtlasReplace("CoinChest1_1.png", "uid://bdcvatet7ks35");
			}
		}
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		if (GodotObject.IsInstanceValid(cell) && cell.isWater)
		{
			switch (damagePointName)
			{
			case "Damage0":
				sprite.SetAtlasReplace("CoinChest1_1.png", "uid://bdcvatet7ks35");
				break;
			case "Damage1":
				sprite.SetAtlasReplace("CoinChest1_1.png", "uid://fmoke6o3c88f");
				break;
			case "Damage2":
				sprite.SetAtlasReplace("CoinChest1_1.png", "uid://1vj81vk4dvil");
				break;
			case "Damage3":
				sprite.SetAtlasReplace("CoinChest1_1.png", "uid://dtfupjwagghmo");
				break;
			case "Damage4":
				sprite.SetAtlasReplace("CoinChest1_1.png", "uid://o6x68dbqcw44");
				break;
			}
		}
	}

	public override async void DestroySet()
	{
		if ((Global.IsEditor && Global.Instance.enterLevelMode == "DiyLevel") || Global.Instance.enterLevelMode == "LoadLevel" || Global.Instance.enterLevelMode == "OnlineLevel")
		{
			return;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		double height = GetGroundHeight(logicalGlobalPosition.Y);
		if ((double)GD.Randf() > 0.0025)
		{
			for (int i = 0; i < 8; i++)
			{
				TowerDefenseGroundItemBase towerDefenseGroundItemBase = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_SILVER, logicalGlobalPosition, height, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f), 980.0);
				if (GodotObject.IsInstanceValid(towerDefenseGroundItemBase))
				{
					towerDefenseGroundItemBase.gridPos = gridPos;
				}
			}
		}
		else
		{
			for (int j = 0; j < 2; j++)
			{
				TowerDefenseGroundItemBase towerDefenseGroundItemBase2 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_GOLD, logicalGlobalPosition, height, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f), 980.0);
				if (GodotObject.IsInstanceValid(towerDefenseGroundItemBase2))
				{
					towerDefenseGroundItemBase2.gridPos = gridPos;
				}
			}
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.DamagePointReach)
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
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
