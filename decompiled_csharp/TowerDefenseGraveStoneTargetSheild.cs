using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/TargetSheild/Scene/TowerDefenseGraveStoneTargetSheild.cs")]
public class TowerDefenseGraveStoneTargetSheild : TowerDefenseGravestone
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ProcessHitBoxOverlaps = "ProcessHitBoxOverlaps";

		public static readonly StringName CanShieldZombie = "CanShieldZombie";

		public static readonly StringName HandleZombieEntered = "HandleZombieEntered";

		public static readonly StringName HandleZombieExited = "HandleZombieExited";
	}

	public new class PropertyName : TowerDefenseGravestone.PropertyName
	{
	}

	public new class SignalName : TowerDefenseGravestone.SignalName
	{
	}

	private readonly HashSet<TowerDefenseZombie> _overlappingZombies = new HashSet<TowerDefenseZombie>();

	private readonly HashSet<TowerDefenseZombie> _overlapScratch = new HashSet<TowerDefenseZombie>();

	public override void _Ready()
	{
		base._Ready();
		Engine.IsEditorHint();
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint())
		{
			ProcessHitBoxOverlaps();
		}
	}

	private void ProcessHitBoxOverlaps()
	{
		_overlapScratch.Clear();
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) || !TowerDefenseManager.CurrentControl.isGameRunning || !inGame || nearDie || die || !TryGetActiveWorldHitRect(out var rect))
		{
			return;
		}
		List<TowerDefenseCharacter> charactersIntersectingRectList = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(rect, gridPos.Y);
		for (int i = 0; i < charactersIntersectingRectList.Count; i++)
		{
			if (charactersIntersectingRectList[i] is TowerDefenseZombie towerDefenseZombie && CanShieldZombie(towerDefenseZombie))
			{
				_overlapScratch.Add(towerDefenseZombie);
				if (!_overlappingZombies.Contains(towerDefenseZombie))
				{
					HandleZombieEntered(towerDefenseZombie);
				}
			}
		}
		foreach (TowerDefenseZombie overlappingZombie in _overlappingZombies)
		{
			if (!GodotObject.IsInstanceValid(overlappingZombie) || !_overlapScratch.Contains(overlappingZombie))
			{
				HandleZombieExited(overlappingZombie);
			}
		}
		_overlappingZombies.RemoveWhere((TowerDefenseZombie zombie) => !GodotObject.IsInstanceValid(zombie) || !_overlapScratch.Contains(zombie));
		foreach (TowerDefenseZombie item in _overlapScratch)
		{
			_overlappingZombies.Add(item);
		}
	}

	private bool CanShieldZombie(TowerDefenseZombie zombie)
	{
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return false;
		}
		if (zombie.isRise)
		{
			return false;
		}
		if ((zombie.instance.maskFlags & 1) == 0)
		{
			return false;
		}
		if (zombie.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
		{
			return false;
		}
		if (zombie.camp != camp)
		{
			return false;
		}
		if (zombie.gridPos.Y != gridPos.Y)
		{
			return false;
		}
		return true;
	}

	private void HandleZombieEntered(TowerDefenseZombie zombie)
	{
		double explosionHurt = zombie.instance.explosionHurt;
		if (explosionHurt != 0.0)
		{
			zombie.instance.explosionHurtSave = explosionHurt;
			zombie.instance.explosionHurt = 0.0;
		}
	}

	private void HandleZombieExited(TowerDefenseZombie zombie)
	{
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return;
		}
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(zombie.gridPos);
		if ((mapCell == null || !mapCell.HasCharacter(config.name)) && zombie.instance.explosionHurt == 0.0)
		{
			double explosionHurtSave = zombie.instance.explosionHurtSave;
			if (explosionHurtSave >= 0.0)
			{
				zombie.instance.explosionHurt = explosionHurtSave;
			}
			else
			{
				zombie.instance.explosionHurt = -1.0;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessHitBoxOverlaps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanShieldZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleZombieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleZombieExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessHitBoxOverlaps && args.Count == 0)
		{
			ProcessHitBoxOverlaps();
			ret = default;
			return true;
		}
		if (method == MethodName.CanShieldZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanShieldZombie(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.HandleZombieEntered && args.Count == 1)
		{
			HandleZombieEntered(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleZombieExited && args.Count == 1)
		{
			HandleZombieExited(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
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
		if (method == MethodName.ProcessHitBoxOverlaps)
		{
			return true;
		}
		if (method == MethodName.CanShieldZombie)
		{
			return true;
		}
		if (method == MethodName.HandleZombieEntered)
		{
			return true;
		}
		if (method == MethodName.HandleZombieExited)
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
