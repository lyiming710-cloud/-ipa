using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/TrashBin/Scene/TowerDefenseTrashBin.cs")]
public class TowerDefenseTrashBin : TowerDefenseGravestone
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName DestroySet = "DestroySet";
	}

	public new class PropertyName : TowerDefenseGravestone.PropertyName
	{
		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefenseGravestone.SignalName
	{
	}

	private const string DisableMetaKey = "disabled_by_trash_bin";

	private const string RefCountMetaKey = "character_disabled_ref_count";

	public bool over;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			RemoveFromGroup("Gravestone");
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || !GodotObject.IsInstanceValid(cell) || nearDie || die)
		{
			return;
		}
		foreach (TowerDefenseCharacter character in cell.GetCharacterList())
		{
			if (character is TowerDefensePlant towerDefensePlant && CanTarget(towerDefensePlant) && !(towerDefensePlant is TowerDefensePlantBowlingBase))
			{
				if (!towerDefensePlant.HasMeta("disabled_by_trash_bin"))
				{
					towerDefensePlant.SetMeta("disabled_by_trash_bin", true);
					int num = towerDefensePlant.GetMeta("character_disabled_ref_count", 0).AsInt32() + 1;
					towerDefensePlant.SetMeta("character_disabled_ref_count", num);
				}
				towerDefensePlant.characterDisabled = true;
				towerDefensePlant.invisible = true;
				towerDefensePlant.instance.canBeCollection = false;
				towerDefensePlant.skipDestroySet = true;
				if (towerDefensePlant is TowerDefensePlantCardless towerDefensePlantCardless)
				{
					towerDefensePlantCardless.instance.invincible = false;
					TowerDefenseManager.Instance.ChangeCostRemove(towerDefensePlantCardless.changeCost);
				}
			}
		}
	}

	public override void DestroySet()
	{
		if (GodotObject.IsInstanceValid(cell))
		{
			foreach (TowerDefenseCharacter character in cell.GetCharacterList())
			{
				if (!(character is TowerDefensePlant towerDefensePlant) || towerDefensePlant is TowerDefensePlantBowlingBase || !towerDefensePlant.HasMeta("disabled_by_trash_bin"))
				{
					continue;
				}
				if (towerDefensePlant.HasMeta("disabled_by_trash_bin"))
				{
					towerDefensePlant.RemoveMeta("disabled_by_trash_bin");
					int num = towerDefensePlant.GetMeta("character_disabled_ref_count", 1).AsInt32() - 1;
					if (num <= 0)
					{
						towerDefensePlant.RemoveMeta("character_disabled_ref_count");
						towerDefensePlant.characterDisabled = false;
					}
					else
					{
						towerDefensePlant.SetMeta("character_disabled_ref_count", num);
					}
				}
				towerDefensePlant.invisible = false;
				towerDefensePlant.instance.canBeCollection = true;
				towerDefensePlant.skipDestroySet = false;
				if (towerDefensePlant is TowerDefensePlantCardless towerDefensePlantCardless)
				{
					towerDefensePlantCardless.instance.invincible = true;
					TowerDefenseManager.Instance.ChangeCostAdd(towerDefensePlantCardless.changeCost);
				}
			}
		}
		base.DestroySet();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.BatchUpdate)
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
