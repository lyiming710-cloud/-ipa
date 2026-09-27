using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter10/WildMine/Scene/TowerDefensePlantWildMine.cs")]
public class TowerDefensePlantWildMine : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName TryAcquireContactTarget = "TryAcquireContactTarget";

		public static readonly StringName TryAcquireSweptContactTarget = "TryAcquireSweptContactTarget";

		public static readonly StringName StartRampage = "StartRampage";

		public static readonly StringName SmashExplode = "SmashExplode";

		public static readonly StringName RestoreRampageState = "RestoreRampageState";

		public static readonly StringName RampageStep = "RampageStep";

		public static readonly StringName ThrowCannonball = "ThrowCannonball";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName readyTime = "readyTime";

		public static readonly StringName _rampaging = "_rampaging";

		public static readonly StringName _step = "_step";

		public static readonly StringName _stepTimer = "_stepTimer";

		public static readonly StringName _readyTime = "_readyTime";

		public static readonly StringName explodeCount = "explodeCount";

		public static readonly StringName explodeInterval = "explodeInterval";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string CannonballProjectileName = "WildMineCannon";

	private const string PotatoMinePacketName = "PlantPotatoMine";

	private PotatoComponent _potatoComponent;

	private ExplodeComponent _explodeComponent;

	private AttackComponent _attackComponent;

	private bool _rampaging;

	private int _step;

	private double _stepTimer;

	private readonly HashSet<Vector2I> _searchedGrids = new HashSet<Vector2I>();

	private System.Collections.Generic.Dictionary<ulong, Rect2> _previousTargetRects = new System.Collections.Generic.Dictionary<ulong, Rect2>();

	private System.Collections.Generic.Dictionary<ulong, Rect2> _currentTargetRects = new System.Collections.Generic.Dictionary<ulong, Rect2>();

	private double _readyTime = 15.0;

	[Export(PropertyHint.None, "")]
	public int explodeCount = 5;

	[Export(PropertyHint.None, "")]
	public double explodeInterval = 0.2;

	[Export(PropertyHint.None, "")]
	public double readyTime
	{
		get
		{
			return _readyTime;
		}
		set
		{
			_readyTime = value;
			PotatoComponent potatoComponent = _potatoComponent;
			if (potatoComponent != null && !potatoComponent.IsReleased)
			{
				_potatoComponent.readyTime = (float)value;
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode && inGame)
		{
			_potatoComponent = componentManager.GetRuntime<PotatoComponent>();
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			_attackComponent = componentManager.GetRuntime<AttackComponent>();
			PotatoComponent potatoComponent = _potatoComponent;
			if (potatoComponent != null && !potatoComponent.IsReleased)
			{
				_potatoComponent.readyTime = (float)readyTime;
				_potatoComponent.smashExplodeHandler = SmashExplode;
			}
			RestoreRampageState();
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || editorPreviewMode || !inGame || IsRemoteNetworkReplica || die || isDestroy || !componentAlive || !TowerDefenseManager.Instance.IsGameRunning())
		{
			return;
		}
		if (_rampaging)
		{
			_stepTimer -= delta * Math.Max(0.0, timeScale);
			while (_rampaging && _stepTimer <= 0.0)
			{
				RampageStep();
			}
			return;
		}
		PotatoComponent potatoComponent = _potatoComponent;
		if (potatoComponent != null && !potatoComponent.IsReleased && !_potatoComponent.over && _potatoComponent.isCharge)
		{
			AttackComponent attackComponent = _attackComponent;
			if (attackComponent != null && !attackComponent.IsReleased && TryAcquireContactTarget())
			{
				StartRampage();
			}
		}
	}

	private bool TryAcquireContactTarget()
	{
		AttackComponent attackComponent = _attackComponent;
		if (attackComponent != null && attackComponent.CanAttackOnContact())
		{
			return true;
		}
		return TryAcquireSweptContactTarget();
	}

	private bool TryAcquireSweptContactTarget()
	{
		if (_attackComponent == null || !_attackComponent.TryGetCheckAreaWorldRect(out var worldRect) || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance?.characterRegistry))
		{
			return false;
		}
		_currentTargetRects.Clear();
		List<TowerDefenseCharacter> charactersForLineList = TowerDefenseManager.Instance.characterRegistry.GetCharactersForLineList(gridPos.Y);
		for (int i = 0; i < charactersForLineList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = charactersForLineList[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && towerDefenseCharacter != this && towerDefenseCharacter.HasHitBox)
			{
				Rect2 worldHitRect = towerDefenseCharacter.WorldHitRect;
				ulong instanceId = towerDefenseCharacter.GetInstanceId();
				_currentTargetRects[instanceId] = worldHitRect;
				bool flag = AabbShapeUtil.Intersects(worldRect, worldHitRect);
				bool flag2 = _previousTargetRects.TryGetValue(instanceId, out var value) && value != worldHitRect && AabbShapeUtil.Intersects(worldRect, AabbShapeUtil.Union(value, worldHitRect));
				if ((flag | flag2) && _attackComponent.TryCommitContactTarget(towerDefenseCharacter))
				{
					_previousTargetRects.Clear();
					_currentTargetRects.Clear();
					return true;
				}
			}
		}
		System.Collections.Generic.Dictionary<ulong, Rect2> currentTargetRects = _currentTargetRects;
		System.Collections.Generic.Dictionary<ulong, Rect2> previousTargetRects = _previousTargetRects;
		_previousTargetRects = currentTargetRects;
		_currentTargetRects = previousTargetRects;
		return false;
	}

	private void StartRampage()
	{
		if (!_rampaging)
		{
			_rampaging = true;
			_step = 0;
			_stepTimer = 0.0;
			_searchedGrids.Clear();
			_previousTargetRects.Clear();
			_currentTargetRects.Clear();
			RestoreRampageState();
			RampageStep();
		}
	}

	private bool SmashExplode()
	{
		StartRampage();
		return true;
	}

	private void RestoreRampageState()
	{
		if (_rampaging)
		{
			PotatoComponent potatoComponent = _potatoComponent;
			if (potatoComponent != null && !potatoComponent.IsReleased)
			{
				_potatoComponent.over = true;
			}
			if (GodotObject.IsInstanceValid(instance))
			{
				instance.invincible = true;
			}
		}
	}

	private void RampageStep()
	{
		_stepTimer += Math.Max(0.1, explodeInterval);
		_step++;
		_explodeComponent?.Explode();
		try
		{
			ThrowCannonball();
		}
		catch (Exception ex)
		{
			GD.PushWarning("[TowerDefensePlantWildMine] Cannonball throw skipped: " + ex.Message);
		}
		if (_step >= explodeCount)
		{
			_rampaging = false;
			Destroy();
		}
	}

	private void ThrowCannonball()
	{
		if (TryPickCannonballTarget(out var targetGrid))
		{
			_searchedGrids.Add(targetGrid);
			int num = 2;
			TowerDefenseProjectileCreateData projectileData = new TowerDefenseProjectileCreateData("WildMineCannon")
			{
				baseDamage = 0.0,
				damageFlags = 0,
				fireMethodFlags = num,
				collisionFlags = 0
			};
			Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(targetGrid);
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				gridYOverride = targetGrid.Y,
				fireMethodFlagsOverride = num,
				catapultStartPositionOverride = GetLogicalGlobalPosition(),
				catapultTargetPositionOverride = mapCellPlantPos
			};
			FireComponent.CreateProjectilePositionByData(null, null, 0.0, mapCellPlantPos, Vector2.Zero, projectileData, 0, camp, default, overrides);
		}
	}

	private bool TryPickCannonballTarget(out Vector2I targetGrid)
	{
		targetGrid = default;
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantPotatoMine");
		List<Vector2I> list = new List<Vector2I>();
		List<Vector2I> list2 = new List<Vector2I>();
		List<Vector2I> list3 = new List<Vector2I>();
		for (int i = 1; i <= mapGridNum.X; i++)
		{
			for (int j = 1; j <= mapGridNum.Y; j++)
			{
				Vector2I item = new Vector2I(i, j);
				if (_searchedGrids.Contains(item))
				{
					continue;
				}
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(item);
				if (GodotObject.IsInstanceValid(mapCell) && WildMineCannonLandingEvent.IsInsidePlantRegion(mapCell))
				{
					list3.Add(item);
					if (WildMineCannonLandingEvent.CellHasEnemyCharacter(mapCell, camp))
					{
						list.Add(item);
					}
					else if (WildMineCannonLandingEvent.CanPlantOriginalMine(mapCell, camp, packetConfig))
					{
						list2.Add(item);
					}
				}
			}
		}
		List<Vector2I> list4;
		if (list.Count > 0)
		{
			list4 = list;
		}
		else
		{
			list4 = ((list2.Count > 0) ? list2 : list3);
		}
		if (list4.Count == 0)
		{
			return false;
		}
		targetGrid = list4[GD.RandRange(0, list4.Count - 1)];
		return true;
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["readyTime"] = readyTime,
			["rampaging"] = _rampaging,
			["step"] = _step,
			["stepTimer"] = _stepTimer,
			["searchedGrids"] = new Array<Vector2I>(_searchedGrids)
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		readyTime = data.GetValueOrDefault("readyTime", 15.0).AsDouble();
		_rampaging = data.GetValueOrDefault("rampaging", false).AsBool();
		_step = Math.Clamp(data.GetValueOrDefault("step", 0).AsInt32(), 0, Math.Max(0, explodeCount));
		_stepTimer = data.GetValueOrDefault("stepTimer", 0.0).AsDouble();
		_searchedGrids.Clear();
		foreach (Variant item in data.GetValueOrDefault("searchedGrids", new Godot.Collections.Array()).AsGodotArray())
		{
			_searchedGrids.Add(item.AsVector2I());
		}
		RestoreRampageState();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryAcquireContactTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryAcquireSweptContactTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartRampage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SmashExplode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreRampageState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RampageStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ThrowCannonball, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.TryAcquireContactTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryAcquireContactTarget());
			return true;
		}
		if (method == MethodName.TryAcquireSweptContactTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryAcquireSweptContactTarget());
			return true;
		}
		if (method == MethodName.StartRampage && args.Count == 0)
		{
			StartRampage();
			ret = default;
			return true;
		}
		if (method == MethodName.SmashExplode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(SmashExplode());
			return true;
		}
		if (method == MethodName.RestoreRampageState && args.Count == 0)
		{
			RestoreRampageState();
			ret = default;
			return true;
		}
		if (method == MethodName.RampageStep && args.Count == 0)
		{
			RampageStep();
			ret = default;
			return true;
		}
		if (method == MethodName.ThrowCannonball && args.Count == 0)
		{
			ThrowCannonball();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName.TryAcquireContactTarget)
		{
			return true;
		}
		if (method == MethodName.TryAcquireSweptContactTarget)
		{
			return true;
		}
		if (method == MethodName.StartRampage)
		{
			return true;
		}
		if (method == MethodName.SmashExplode)
		{
			return true;
		}
		if (method == MethodName.RestoreRampageState)
		{
			return true;
		}
		if (method == MethodName.RampageStep)
		{
			return true;
		}
		if (method == MethodName.ThrowCannonball)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.readyTime)
		{
			readyTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._rampaging)
		{
			_rampaging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._step)
		{
			_step = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._stepTimer)
		{
			_stepTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._readyTime)
		{
			_readyTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.explodeCount)
		{
			explodeCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.explodeInterval)
		{
			explodeInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.readyTime)
		{
			value = VariantUtils.CreateFrom<double>(readyTime);
			return true;
		}
		if (name == PropertyName._rampaging)
		{
			value = VariantUtils.CreateFrom(in _rampaging);
			return true;
		}
		if (name == PropertyName._step)
		{
			value = VariantUtils.CreateFrom(in _step);
			return true;
		}
		if (name == PropertyName._stepTimer)
		{
			value = VariantUtils.CreateFrom(in _stepTimer);
			return true;
		}
		if (name == PropertyName._readyTime)
		{
			value = VariantUtils.CreateFrom(in _readyTime);
			return true;
		}
		if (name == PropertyName.explodeCount)
		{
			value = VariantUtils.CreateFrom(in explodeCount);
			return true;
		}
		if (name == PropertyName.explodeInterval)
		{
			value = VariantUtils.CreateFrom(in explodeInterval);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._rampaging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._step, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._stepTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.readyTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._readyTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.explodeCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.explodeInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.readyTime, Variant.From<double>(readyTime));
		info.AddProperty(PropertyName._rampaging, Variant.From(in _rampaging));
		info.AddProperty(PropertyName._step, Variant.From(in _step));
		info.AddProperty(PropertyName._stepTimer, Variant.From(in _stepTimer));
		info.AddProperty(PropertyName._readyTime, Variant.From(in _readyTime));
		info.AddProperty(PropertyName.explodeCount, Variant.From(in explodeCount));
		info.AddProperty(PropertyName.explodeInterval, Variant.From(in explodeInterval));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.readyTime, out var value))
		{
			readyTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._rampaging, out var value2))
		{
			_rampaging = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._step, out var value3))
		{
			_step = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._stepTimer, out var value4))
		{
			_stepTimer = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._readyTime, out var value5))
		{
			_readyTime = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.explodeCount, out var value6))
		{
			explodeCount = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.explodeInterval, out var value7))
		{
			explodeInterval = value7.As<double>();
		}
	}
}
