using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/FloorQX/Scene/TowerDefenseFloorQX.cs")]
public class TowerDefenseFloorQX : TowerDefenseGravestone
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ProcessHitBoxOverlaps = "ProcessHitBoxOverlaps";

		public static readonly StringName CanProjectZombie = "CanProjectZombie";

		public static readonly StringName HandleZombieEntered = "HandleZombieEntered";

		public static readonly StringName SpawnHologramProjection = "SpawnHologramProjection";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : TowerDefenseGravestone.PropertyName
	{
		public static readonly StringName _triggered = "_triggered";
	}

	public new class SignalName : TowerDefenseGravestone.SignalName
	{
	}

	private readonly HashSet<TowerDefenseZombie> _overlappingZombies = new HashSet<TowerDefenseZombie>();

	private readonly HashSet<TowerDefenseZombie> _overlapScratch = new HashSet<TowerDefenseZombie>();

	private bool _triggered;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			instance.canBeCollection = false;
			RemoveFromGroup("Gravestone");
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost) && !_triggered && !nearDie && !die)
		{
			ProcessHitBoxOverlaps();
		}
	}

	private void ProcessHitBoxOverlaps()
	{
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) || !TowerDefenseManager.CurrentControl.isGameRunning || !inGame || !TryGetActiveWorldHitRect(out var rect))
		{
			return;
		}
		List<TowerDefenseCharacter> charactersIntersectingRectList = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(rect, gridPos.Y);
		for (int i = 0; i < charactersIntersectingRectList.Count; i++)
		{
			if (charactersIntersectingRectList[i] is TowerDefenseZombie towerDefenseZombie && CanProjectZombie(towerDefenseZombie))
			{
				_overlapScratch.Add(towerDefenseZombie);
				if (!_overlappingZombies.Contains(towerDefenseZombie))
				{
					HandleZombieEntered(towerDefenseZombie);
				}
			}
		}
		_overlappingZombies.RemoveWhere((TowerDefenseZombie zombie) => !GodotObject.IsInstanceValid(zombie) || !_overlapScratch.Contains(zombie));
		foreach (TowerDefenseZombie item in _overlapScratch)
		{
			_overlappingZombies.Add(item);
		}
	}

	private bool CanProjectZombie(TowerDefenseZombie zombie)
	{
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return false;
		}
		if (zombie.isRise)
		{
			return false;
		}
		if (zombie.gridPos.Y != gridPos.Y)
		{
			return false;
		}
		if (zombie.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
		{
			return false;
		}
		if ((zombie.instance.maskFlags & 2) != 0)
		{
			return false;
		}
		return true;
	}

	private void HandleZombieEntered(TowerDefenseZombie zombie)
	{
		if (!_triggered)
		{
			_triggered = true;
			SpawnHologramProjection(zombie, gridPos.Y - 1);
			SpawnHologramProjection(zombie, gridPos.Y + 1);
			die = true;
			sprite.SetAnimation("Shooting", loop: false, 0.2);
		}
	}

	private void SpawnHologramProjection(TowerDefenseZombie sourceZombie, int row)
	{
		if (!GodotObject.IsInstanceValid(sourceZombie) || !GodotObject.IsInstanceValid(sourceZombie.packet) || row < 1 || row > TowerDefenseManager.Instance.GetMapGridNum().Y)
		{
			return;
		}
		Vector2I vector2I = new Vector2I(gridPos.X, row);
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(vector2I);
		if (mapCell == null)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(sourceZombie.packet.saveKey);
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return;
		}
		Vector2 pos = TowerDefenseManager.GetMapCellPlantPos(vector2I) / TowerDefenseManager.GetMapFeature().mapControl.GlobalScale.Y;
		TowerDefenseZombie projection = packetConfig.Create(pos, vector2I, groundHeight) as TowerDefenseZombie;
		if (!GodotObject.IsInstanceValid(projection))
		{
			return;
		}
		TowerDefenseManager.GetCharacterNode().AddChild(projection, forceReadableName: false, InternalMode.Disabled);
		bool hypnoses = sourceZombie.instance.hypnoses;
		double hitpointScale = sourceZombie.instance.hitpointScale * 0.3;
		Vector2 scale = sourceZombie.transformPoint.Scale;
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(projection))
			{
				projection.Walk();
				projection.SetSpriteGroupShaderParameter("hologram", true);
				if (hypnoses)
				{
					projection.Hypnoses();
				}
				projection.instance.hologram = true;
				projection.SetHitpointAndScale(hitpointScale, scale);
				projection.invisible = sourceZombie.invisible;
			}
		}).CallDeferred();
		mapCell.ReleaseHologramGridOccupancy(projection);
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl) && GodotObject.IsInstanceValid(projection))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, projection);
				Dictionary spawnState = new Dictionary { ["hologram"] = true };
				MultiPlayerManager.Instance.SendSpawnCharacterAt(packetConfig.saveKey, vector2I.X, vector2I.Y, nextSyncId, hitpointScale, scale.X, hypnoses, 0.0, useCreate: true, pos.X, pos.Y, walkAfterSpawn: true, groundHeight, "", spawnState);
			}
		}
	}

	public override void AnimeCompleted(string clip)
	{
		if (clip == "Shooting")
		{
			Destroy();
		}
		else
		{
			base.AnimeCompleted(clip);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessHitBoxOverlaps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanProjectZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleZombieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnHologramProjection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sourceZombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "row", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CanProjectZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanProjectZombie(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.HandleZombieEntered && args.Count == 1)
		{
			HandleZombieEntered(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnHologramProjection && args.Count == 2)
		{
			SpawnHologramProjection(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.CanProjectZombie)
		{
			return true;
		}
		if (method == MethodName.HandleZombieEntered)
		{
			return true;
		}
		if (method == MethodName.SpawnHologramProjection)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._triggered)
		{
			_triggered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._triggered)
		{
			value = VariantUtils.CreateFrom(in _triggered);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._triggered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._triggered, Variant.From(in _triggered));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._triggered, out var value))
		{
			_triggered = value.As<bool>();
		}
	}
}
