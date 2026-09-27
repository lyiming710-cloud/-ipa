using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/TargetGhost/Scene/TowerDefenseGraveStoneTargetGhost.cs")]
public class TowerDefenseGraveStoneTargetGhost : TowerDefenseGravestone
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ProcessHitBoxOverlaps = "ProcessHitBoxOverlaps";

		public static readonly StringName CanGhostZombie = "CanGhostZombie";

		public static readonly StringName HandleZombieEntered = "HandleZombieEntered";
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
		if (!Engine.IsEditorHint() && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
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
			if (charactersIntersectingRectList[i] is TowerDefenseZombie towerDefenseZombie && CanGhostZombie(towerDefenseZombie))
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

	private bool CanGhostZombie(TowerDefenseZombie zombie)
	{
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return false;
		}
		if (zombie.isRise)
		{
			return false;
		}
		if (zombie.hasGhost)
		{
			return false;
		}
		int maskFlags = zombie.instance.maskFlags;
		if ((maskFlags & 0x10) != 0 && (maskFlags & 1) == 0)
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
		if (!zombie.targetRegistrationComponent.canCarry)
		{
			return false;
		}
		if (zombie.gridPos.Y != gridPos.Y)
		{
			return false;
		}
		return true;
	}

	private async void HandleZombieEntered(TowerDefenseZombie zombie)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ZombieGhost");
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseZombieGhost ghostZombie = packetConfig.Create(logicalGlobalPosition, gridPos, groundHeight) as TowerDefenseZombieGhost;
		if (!GodotObject.IsInstanceValid(ghostZombie))
		{
			return;
		}
		ghostZombie.carryCharacter = zombie;
		TowerDefenseManager.GetCharacterNode().AddChild(ghostZombie, forceReadableName: false, InternalMode.Disabled);
		double riseDuration = GD.RandRange(0.4, 0.6);
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(ghostZombie))
			{
				ghostZombie.Rise(riseDuration);
			}
		}).CallDeferred();
		Hurt(200.0);
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, ghostZombie);
				Dictionary spawnState = new Dictionary
				{
					["carrierSyncId"] = zombie.syncId,
					["ghostTimeScaleSave"] = ghostZombie.ghostTimeScaleSave
				};
				MultiPlayerManager.Instance.SendSpawnCharacterAt("ZombieGhost", gridPos.X, gridPos.Y, nextSyncId, 1.0, 1.0, hypnoses: false, riseDuration, useCreate: true, logicalGlobalPosition.X, logicalGlobalPosition.Y, walkAfterSpawn: false, groundHeight, "", spawnState);
			}
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessHitBoxOverlaps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanGhostZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleZombieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.CanGhostZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanGhostZombie(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.HandleZombieEntered && args.Count == 1)
		{
			HandleZombieEntered(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
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
		if (method == MethodName.CanGhostZombie)
		{
			return true;
		}
		if (method == MethodName.HandleZombieEntered)
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
