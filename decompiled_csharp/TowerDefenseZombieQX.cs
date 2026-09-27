using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter9/QX/Scene/TowerDefenseZombieQX.cs")]
public class TowerDefenseZombieQX : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ScanRangeDeaths = "ScanRangeDeaths";

		public static readonly StringName CanWatchZombie = "CanWatchZombie";

		public static readonly StringName TriggerProjection = "TriggerProjection";

		public static readonly StringName SpawnHologramProjection = "SpawnHologramProjection";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _checkSize = "_checkSize";

		public static readonly StringName _projecting = "_projecting";

		public static readonly StringName _projectingSource = "_projectingSource";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private HashSet<TowerDefenseZombie> _livingTracked = new HashSet<TowerDefenseZombie>();

	private readonly HashSet<TowerDefenseZombie> _deathHandled = new HashSet<TowerDefenseZombie>();

	private readonly List<TowerDefenseCharacter> _candidates = new List<TowerDefenseCharacter>();

	private HashSet<TowerDefenseZombie> _currentAlive = new HashSet<TowerDefenseZombie>();

	private Vector2 _checkSize;

	private bool _projecting;

	private TowerDefenseZombie _projectingSource;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_projecting = false;
			_checkSize = TowerDefenseManager.Instance.GetMapGridSize() * 2.75f;
		}
	}

	public override void _ExitTree()
	{
		_livingTracked.Clear();
		_currentAlive.Clear();
		_deathHandled.Clear();
		_candidates.Clear();
		base._ExitTree();
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || !TowerDefenseManager.Instance.IsGameRunning() || !inGame || _projecting || nearDie || die)
		{
			return;
		}
		if (!IsInsideComponentBattlefield)
		{
			if (_livingTracked.Count > 0)
			{
				_livingTracked.Clear();
			}
			if (_deathHandled.Count > 0)
			{
				_deathHandled.Clear();
			}
		}
		else
		{
			ScanRangeDeaths();
		}
	}

	private void ScanRangeDeaths()
	{
		_candidates.Clear();
		Rect2 checkRect = AabbShapeUtil.RectFromCenter(GetLogicalGlobalPosition(), _checkSize);
		TowerDefenseManager.Instance.characterRegistry.FillCharactersIntersectingRectListForCamp(checkRect, camp, _candidates);
		_currentAlive.Clear();
		for (int i = 0; i < _candidates.Count; i++)
		{
			if (!(_candidates[i] is TowerDefenseZombie towerDefenseZombie) || !CanWatchZombie(towerDefenseZombie))
			{
				continue;
			}
			if (towerDefenseZombie.die || towerDefenseZombie.nearDie)
			{
				if (_livingTracked.Contains(towerDefenseZombie) && _deathHandled.Add(towerDefenseZombie))
				{
					TriggerProjection(towerDefenseZombie);
					return;
				}
			}
			else
			{
				_currentAlive.Add(towerDefenseZombie);
			}
		}
		HashSet<TowerDefenseZombie> currentAlive = _currentAlive;
		HashSet<TowerDefenseZombie> livingTracked = _livingTracked;
		_livingTracked = currentAlive;
		_currentAlive = livingTracked;
	}

	private bool CanWatchZombie(TowerDefenseZombie zombie)
	{
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return false;
		}
		if (zombie == this)
		{
			return false;
		}
		if (zombie.isRise)
		{
			return false;
		}
		TowerDefenseCharacterInstance towerDefenseCharacterInstance = zombie.instance;
		if (towerDefenseCharacterInstance != null && towerDefenseCharacterInstance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
		{
			return false;
		}
		if (((zombie.instance?.maskFlags ?? 0) & 2) != 0)
		{
			return false;
		}
		return true;
	}

	private void TriggerProjection(TowerDefenseZombie deadZombie)
	{
		if (!_projecting && GodotObject.IsInstanceValid(deadZombie?.packet))
		{
			_projectingSource = deadZombie;
			_projecting = true;
			SetMainStateMachineDispatchEnabled(enabled: false);
			die = true;
			sprite.SetAnimation("Shooting", loop: false, 0.2);
		}
	}

	private void SpawnHologramProjection()
	{
		TowerDefenseZombie projectingSource = _projectingSource;
		if (!GodotObject.IsInstanceValid(projectingSource) || !GodotObject.IsInstanceValid(projectingSource.packet))
		{
			Destroy();
			return;
		}
		string saveKey = projectingSource.packet.saveKey;
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(saveKey);
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			Destroy();
			return;
		}
		Vector2I vector2I = gridPos;
		Vector2 pos = TowerDefenseManager.GetMapCellPlantPos(vector2I) / TowerDefenseManager.GetMapFeature().mapControl.GlobalScale.Y;
		double hitpointScale = projectingSource.instance?.hitpointScale ?? 1.0;
		Vector2 scale = projectingSource.transformPoint.Scale;
		bool hypnoses = projectingSource.instance?.hypnoses ?? false;
		bool invisible = projectingSource.invisible;
		TowerDefenseZombie projection = packetConfig.Create(pos, vector2I, groundHeight) as TowerDefenseZombie;
		if (GodotObject.IsInstanceValid(projection))
		{
			TowerDefenseManager.GetCharacterNode().AddChild(projection, forceReadableName: false, InternalMode.Disabled);
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
					projection.invisible = invisible;
				}
			}).CallDeferred();
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
				if (GodotObject.IsInstanceValid(currentControl))
				{
					int nextSyncId = currentControl.GetNextSyncId();
					currentControl.RegisterSyncCharacter(nextSyncId, projection);
					Dictionary spawnState = new Dictionary { ["hologram"] = true };
					MultiPlayerManager.Instance.SendSpawnCharacterAt(saveKey, vector2I.X, vector2I.Y, nextSyncId, hitpointScale, scale.X, hypnoses, 0.0, useCreate: true, pos.X, pos.Y, walkAfterSpawn: true, groundHeight, "", spawnState);
				}
			}
		}
		Destroy();
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		if (Engine.IsEditorHint() || !inGame || die || nearDie || _projecting)
		{
			return;
		}
		sprite.timeScale = timeScale * (double)(float)walkSpeedScale;
		if (!attackComponent.CanAttack())
		{
			return;
		}
		if (GodotObject.IsInstanceValid(attackComponent.target?.cell) && attackComponent.target.cell.HasSpike())
		{
			attackComponent.target = attackComponent.target.cell.GetSpike();
		}
		if (GodotObject.IsInstanceValid(attackComponent.target) && (attackComponent.target.instance.physiqueTypeFlags & 0x10) == 0)
		{
			attackComponent.SmashAttackCell(((TowerDefenseZombieConfig)config).smashAttack);
		}
		else if (GodotObject.IsInstanceValid(attackComponent.target))
		{
			if (attackComponent.target.instance.spikeHurt != -1.0)
			{
				TowerDefenseCharacter target = attackComponent.target;
				double spikeHurt = attackComponent.target.instance.spikeHurt;
				target.Hurt(100000.0, playSplatAudio: true, default, createDamagePart: true, spikeHurt);
			}
			Die();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		if (_projecting && clip == "Shooting")
		{
			SpawnHologramProjection();
		}
		else
		{
			base.AnimeCompleted(clip);
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
			new MethodInfo(MethodName.ScanRangeDeaths, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanWatchZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TriggerProjection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "deadZombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnHologramProjection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ScanRangeDeaths && args.Count == 0)
		{
			ScanRangeDeaths();
			ret = default;
			return true;
		}
		if (method == MethodName.CanWatchZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanWatchZombie(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.TriggerProjection && args.Count == 1)
		{
			TriggerProjection(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnHologramProjection && args.Count == 0)
		{
			SpawnHologramProjection();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.ScanRangeDeaths)
		{
			return true;
		}
		if (method == MethodName.CanWatchZombie)
		{
			return true;
		}
		if (method == MethodName.TriggerProjection)
		{
			return true;
		}
		if (method == MethodName.SpawnHologramProjection)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
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
		if (name == PropertyName._checkSize)
		{
			_checkSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._projecting)
		{
			_projecting = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._projectingSource)
		{
			_projectingSource = VariantUtils.ConvertTo<TowerDefenseZombie>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checkSize)
		{
			value = VariantUtils.CreateFrom(in _checkSize);
			return true;
		}
		if (name == PropertyName._projecting)
		{
			value = VariantUtils.CreateFrom(in _projecting);
			return true;
		}
		if (name == PropertyName._projectingSource)
		{
			value = VariantUtils.CreateFrom(in _projectingSource);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2, PropertyName._checkSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._projecting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectingSource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checkSize, Variant.From(in _checkSize));
		info.AddProperty(PropertyName._projecting, Variant.From(in _projecting));
		info.AddProperty(PropertyName._projectingSource, Variant.From(in _projectingSource));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checkSize, out var value))
		{
			_checkSize = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._projecting, out var value2))
		{
			_projecting = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._projectingSource, out var value3))
		{
			_projectingSource = value3.As<TowerDefenseZombie>();
		}
	}
}
