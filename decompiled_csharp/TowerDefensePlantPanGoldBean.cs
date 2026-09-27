using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter7/PanGoldBean/Scene/TowerDefensePlantPanGoldBean.cs")]
public class TowerDefensePlantPanGoldBean : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Explode = "Explode";

		public static readonly StringName FinalizeReplacementPlant = "FinalizeReplacementPlant";

		public static readonly StringName RestoreOriginalPlants = "RestoreOriginalPlants";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private ExplodeComponent _explodeComponent;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			_explodeComponent.OnExplode += Explode;
		}
	}

	public override void _ExitTree()
	{
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent != null && !explodeComponent.IsReleased)
		{
			_explodeComponent.OnExplode -= Explode;
		}
		base._ExitTree();
	}

	public void Explode()
	{
		if (!TowerDefenseManager.HasGameplayAuthority || !GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		double height = GetGroundHeight(logicalGlobalPosition.Y);
		List<TowerDefenseCharacter> characterList = cell.GetCharacterList();
		Array<TowerDefenseCharacter> filteredList = new Array<TowerDefenseCharacter>();
		foreach (TowerDefenseCharacter item in characterList)
		{
			if (item is TowerDefensePlant && item.config.name != config.name && item.camp == camp)
			{
				filteredList.Add(item);
			}
		}
		foreach (TowerDefenseCharacter item2 in filteredList)
		{
			if (!GodotObject.IsInstanceValid(item2))
			{
				continue;
			}
			item2.WakeUp();
			if (item2.cost > 0.0 && !instance.hypnoses)
			{
				int num = (int)Mathf.Floor(item2.cost / 50.0);
				for (int i = 0; i < num; i++)
				{
					TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_GOLD, logicalGlobalPosition, height, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f), 980.0).gridPos = gridPos;
				}
				int num2 = (int)Mathf.Floor((item2.cost - (double)(num * 50)) / 10.0);
				for (int j = 0; j < num2; j++)
				{
					TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_SILVER, logicalGlobalPosition, height, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f), 980.0).gridPos = gridPos;
				}
			}
		}
		if (filteredList.Count <= 0 || !((double)GD.Randf() < 0.05))
		{
			return;
		}
		foreach (TowerDefenseCharacter item3 in filteredList)
		{
			if (GodotObject.IsInstanceValid(item3))
			{
				cell.RemoveCharacter(item3);
			}
		}
		TowerDefensePacketBankData packetBankData = TowerDefenseManager.GetPacketBankData("GeneralPlant");
		if (!GodotObject.IsInstanceValid(packetBankData))
		{
			RestoreOriginalPlants(cell, filteredList);
			return;
		}
		Array category = packetBankData.GetCategory("Gold");
		if (category == null || category.Count == 0)
		{
			RestoreOriginalPlants(cell, filteredList);
			return;
		}
		string packetRandom = category.PickRandom().AsString();
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetRandom);
		while ((!GodotObject.IsInstanceValid(packetConfig) || !cell.CanPacketPlant(packetConfig)) && category.Count > 1)
		{
			category.Remove(packetRandom);
			packetRandom = category.PickRandom().AsString();
			packetConfig = TowerDefenseManager.GetPacketConfig(packetRandom);
		}
		if (GodotObject.IsInstanceValid(packetConfig) && cell.CanPacketPlant(packetConfig))
		{
			TowerDefenseCharacter plant = (HasEconomyOwner ? packetConfig.Plant(EconomyOwnerAccountId, gridPos) : packetConfig.Plant(gridPos));
			if (!GodotObject.IsInstanceValid(plant))
			{
				RestoreOriginalPlants(cell, filteredList);
				return;
			}
			TowerDefenseCellInstance replacementCell = cell;
			bool shouldHypnotize = instance.hypnoses;
			Callable.From(() =>
			{
				FinalizeReplacementPlant(plant, packetRandom, filteredList, replacementCell, shouldHypnotize, 3);
			}).CallDeferred();
		}
		else
		{
			RestoreOriginalPlants(cell, filteredList);
		}
	}

	private static void FinalizeReplacementPlant(TowerDefenseCharacter plant, string packetName, Array<TowerDefenseCharacter> originalPlants, TowerDefenseCellInstance replacementCell, bool shouldHypnotize, int readyRetries)
	{
		if (!GodotObject.IsInstanceValid(plant))
		{
			RestoreOriginalPlants(replacementCell, originalPlants);
			return;
		}
		if (!plant.IsNodeReady())
		{
			if (readyRetries > 0)
			{
				Callable.From(() =>
				{
					FinalizeReplacementPlant(plant, packetName, originalPlants, replacementCell, shouldHypnotize, readyRetries - 1);
				}).CallDeferred();
				return;
			}
			if (GodotObject.IsInstanceValid(replacementCell))
			{
				replacementCell.RemoveCharacter(plant);
			}
			plant.QueueFree();
			RestoreOriginalPlants(replacementCell, originalPlants);
			return;
		}
		plant.WakeUp();
		if (shouldHypnotize)
		{
			plant.Hypnoses();
		}
		Dictionary spawnState = new Dictionary { ["wake_up"] = true };
		TowerDefenseManager.PublishSpawnedCharacter(packetName, plant, useCreate: false, 0.0, walkAfterSpawn: false, "", spawnState);
		foreach (TowerDefenseCharacter originalPlant in originalPlants)
		{
			if (GodotObject.IsInstanceValid(originalPlant))
			{
				originalPlant.Destroy(freeInstance: false);
				originalPlant.QueueFree();
			}
		}
	}

	private static void RestoreOriginalPlants(TowerDefenseCellInstance replacementCell, Array<TowerDefenseCharacter> originalPlants)
	{
		if (!GodotObject.IsInstanceValid(replacementCell))
		{
			return;
		}
		foreach (TowerDefenseCharacter originalPlant in originalPlants)
		{
			if (GodotObject.IsInstanceValid(originalPlant) && !originalPlant.IsQueuedForDeletion())
			{
				replacementCell.CharacterPlant(originalPlant.packet, originalPlant);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinalizeReplacementPlant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "originalPlants", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "replacementCell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "shouldHypnotize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "readyRetries", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreOriginalPlants, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "replacementCell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Array, "originalPlants", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Explode && args.Count == 0)
		{
			Explode();
			ret = default;
			return true;
		}
		if (method == MethodName.FinalizeReplacementPlant && args.Count == 6)
		{
			FinalizeReplacementPlant(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreOriginalPlants && args.Count == 2)
		{
			RestoreOriginalPlants(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FinalizeReplacementPlant && args.Count == 6)
		{
			FinalizeReplacementPlant(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreOriginalPlants && args.Count == 2)
		{
			RestoreOriginalPlants(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[1]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.Explode)
		{
			return true;
		}
		if (method == MethodName.FinalizeReplacementPlant)
		{
			return true;
		}
		if (method == MethodName.RestoreOriginalPlants)
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
