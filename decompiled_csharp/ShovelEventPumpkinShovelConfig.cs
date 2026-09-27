using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Shovel/Resource/Event/ShovelEventPumpkinShovelConfig.cs")]
public class ShovelEventPumpkinShovelConfig : ShovelEventConfig
{
	public new class MethodName : ShovelEventConfig.MethodName
	{
		public new static readonly StringName Execute = "Execute";
	}

	public new class PropertyName : ShovelEventConfig.PropertyName
	{
	}

	public new class SignalName : ShovelEventConfig.SignalName
	{
	}

	public override void Execute(TowerDefenseCharacter character)
	{
		bool flag = false;
		bool flag2 = false;
		TowerDefenseCharacter towerDefenseCharacter = null;
		if (GodotObject.IsInstanceValid(character.cell))
		{
			towerDefenseCharacter = character.cell.GetSurround();
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				flag2 = true;
				if (towerDefenseCharacter == character)
				{
					flag = true;
				}
			}
		}
		if (!flag)
		{
			double transferHP = character.instance.hitpoints;
			if (flag2 && towerDefenseCharacter.config.name == "PlantPumpkin")
			{
				towerDefenseCharacter.instance.Health(transferHP);
			}
			else if (!flag2)
			{
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantPumpkin");
				TowerDefenseCharacter pumpkin = packetConfig.Plant(character.gridPos);
				if (GodotObject.IsInstanceValid(pumpkin))
				{
					Callable.From(() =>
					{
						if (GodotObject.IsInstanceValid(pumpkin) && GodotObject.IsInstanceValid(pumpkin.instance))
						{
							pumpkin.instance.hitpoints = transferHP;
							pumpkin.instance.RefreshDamagePoint();
						}
					}).CallDeferred();
				}
			}
			character.ShovelDestroy();
		}
		else if (towerDefenseCharacter.config.name == "PlantPumpkin")
		{
			double percentage = Mathf.Min(1.0, 0.5 * (towerDefenseCharacter.instance.hitpoints / towerDefenseCharacter.config.hitpoints));
			towerDefenseCharacter.Recycle(percentage);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Execute && args.Count == 1)
		{
			Execute(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Execute)
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
