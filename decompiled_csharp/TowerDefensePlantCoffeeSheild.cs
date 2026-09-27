using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Diamond/CoffeeSheild/Scene/TowerDefensePlantCoffeeSheild.cs")]
public class TowerDefensePlantCoffeeSheild : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName OnBuffDelete = "OnBuffDelete";

		public static readonly StringName Explode = "Explode";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public static readonly StringName _SyncShieldHypnoses = "_SyncShieldHypnoses";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	public static readonly StringName SHIELD_TYPE = new StringName("CommonShield");

	public const double SHIELD_HP = 4000.0;

	private ExplodeComponent _explodeComponent;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			_explodeComponent.OnExplode += Explode;
			BuffComponent buffComponent = buff;
			if (buffComponent != null && !buffComponent.IsReleased)
			{
				buff.OnBuffDelete += OnBuffDelete;
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent != null && !explodeComponent.IsReleased)
		{
			_explodeComponent.OnExplode -= Explode;
		}
		BuffComponent buffComponent = buff;
		if (buffComponent != null && !buffComponent.IsReleased)
		{
			buff.OnBuffDelete -= OnBuffDelete;
		}
	}

	private void OnBuffDelete(string key)
	{
		if (key == "Hypnoses")
		{
			_SyncShieldHypnoses();
		}
	}

	public void Explode()
	{
		foreach (TowerDefenseCellInstance shieldCell in GetShieldCells())
		{
			foreach (TowerDefenseCharacter character in shieldCell.characterList)
			{
				if (GodotObject.IsInstanceValid(character) && character.camp == camp)
				{
					character.WakeUp();
				}
			}
			TowerDefenseItemSheild.CreateOnCellWithHP(shieldCell, SHIELD_TYPE, 4000.0);
		}
		if (instance.hypnoses)
		{
			Callable.From(_SyncShieldHypnoses).CallDeferred();
		}
	}

	private List<TowerDefenseCellInstance> GetShieldCells()
	{
		List<TowerDefenseCellInstance> list = new List<TowerDefenseCellInstance>();
		if (GodotObject.IsInstanceValid(cell))
		{
			list.Add(cell);
		}
		if (GodotObject.IsInstanceValid(targetPlant) && targetPlant.config is TowerDefensePlantConfig towerDefensePlantConfig)
		{
			foreach (Vector2I item in towerDefensePlantConfig.extendGrid)
			{
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(targetPlant.gridPos + item);
				if (GodotObject.IsInstanceValid(mapCell) && !list.Contains(mapCell))
				{
					list.Add(mapCell);
				}
			}
		}
		return list;
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		_SyncShieldHypnoses();
	}

	private void _SyncShieldHypnoses()
	{
		foreach (TowerDefenseCellInstance shieldCell in GetShieldCells())
		{
			TowerDefenseItemSheild itemShield = shieldCell.itemShield;
			if (GodotObject.IsInstanceValid(itemShield) && itemShield.instance.hypnoses != instance.hypnoses)
			{
				itemShield.Hypnoses();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnBuffDelete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._SyncShieldHypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnBuffDelete && args.Count == 1)
		{
			OnBuffDelete(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Explode && args.Count == 0)
		{
			Explode();
			ret = default;
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName._SyncShieldHypnoses && args.Count == 0)
		{
			_SyncShieldHypnoses();
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
		if (method == MethodName.OnBuffDelete)
		{
			return true;
		}
		if (method == MethodName.Explode)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		if (method == MethodName._SyncShieldHypnoses)
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
