using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PreSpawn/TowerDefenseBattleFeaturePreSpawn.cs")]
public class TowerDefenseBattleFeaturePreSpawn : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName HasPendingGravestoneReservation = "HasPendingGravestoneReservation";

		public static readonly StringName ReleasePendingPlacementReservationsDeferred = "ReleasePendingPlacementReservationsDeferred";

		public static readonly StringName RegisterPreSpawnCharacter = "RegisterPreSpawnCharacter";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName config = "config";

		public static readonly StringName preSpawnList = "preSpawnList";

		public static readonly StringName _pendingPlacementReservations = "_pendingPlacementReservations";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	public TowerDefenseBattleFeaturePreSpawnConfig config;

	public Array<TowerDefenseCharacter> preSpawnList = new Array<TowerDefenseCharacter>();

	private bool _pendingPlacementReservations;

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseBattleFeaturePreSpawnConfig();
		config.Init(data);
		_pendingPlacementReservations = config.preSpawnList.Count > 0;
	}

	public override async Task GameEntry()
	{
		_ = 1;
		try
		{
			if (!IsLifetimeActive || config == null || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || control.hasProgress || !control.isInit)
			{
				return;
			}
			TowerDefenseLevelConfig towerDefenseLevelConfig = control.levelConfig as TowerDefenseLevelConfig;
			if (towerDefenseLevelConfig.finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM && towerDefenseLevelConfig.izmManager.shuffle && control.process is TowerDefenseBattleProcessIZM towerDefenseBattleProcessIZM)
			{
				List<TowerDefenseCharacter> list = await towerDefenseBattleProcessIZM.Execute(config.preSpawnList);
				if (!IsLifetimeActive)
				{
					DestroyPendingCharacters(list);
					return;
				}
				{
					foreach (TowerDefenseCharacter item in list)
					{
						RegisterPreSpawnCharacter(item);
						preSpawnList.Add(item);
					}
					return;
				}
			}
			List<TowerDefenseCharacter> list2 = await SpawnPreSpawnCharactersWithRetry(config.preSpawnList);
			if (!IsLifetimeActive)
			{
				DestroyPendingCharacters(list2);
				return;
			}
			foreach (TowerDefenseCharacter item2 in list2)
			{
				RegisterPreSpawnCharacter(item2);
				preSpawnList.Add(item2);
			}
		}
		finally
		{
			ReleasePendingPlacementReservationsDeferred();
		}
	}

	public bool HasPendingGravestoneReservation(Vector2I gridPos)
	{
		if (!_pendingPlacementReservations || config == null)
		{
			return false;
		}
		foreach (TowerDefenseLevelPreSpawnConfig preSpawn in config.preSpawnList)
		{
			if (GodotObject.IsInstanceValid(preSpawn) && !(preSpawn.gridPos != gridPos))
			{
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(preSpawn.packetName);
				if (GodotObject.IsInstanceValid(packetConfig) && packetConfig.characterConfig is TowerDefenseGravestoneConfig)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void ReleasePendingPlacementReservationsDeferred()
	{
		if (_pendingPlacementReservations)
		{
			Callable.From(() => _pendingPlacementReservations = false).CallDeferred();
		}
	}

	private async Task<List<TowerDefenseCharacter>> SpawnPreSpawnCharactersWithRetry(IList<TowerDefenseLevelPreSpawnConfig> preSpawnConfigs)
	{
		List<TowerDefenseCharacter> spawnedCharacters = new List<TowerDefenseCharacter>();
		List<TowerDefenseLevelPreSpawnConfig> pending = new List<TowerDefenseLevelPreSpawnConfig>(preSpawnConfigs);
		int retryPass = 0;
		while (pending.Count > 0 && retryPass < config.maxRetryPasses)
		{
			List<TowerDefenseLevelPreSpawnConfig> list = new List<TowerDefenseLevelPreSpawnConfig>();
			int num = 0;
			foreach (TowerDefenseLevelPreSpawnConfig item in pending)
			{
				TowerDefenseCharacter towerDefenseCharacter = item.SpawnCharacter();
				if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
				{
					list.Add(item);
					continue;
				}
				num++;
				spawnedCharacters.Add(towerDefenseCharacter);
				if (GodotObject.IsInstanceValid(item.characterOverride))
				{
					item.characterOverride.ExecuteCharacter(towerDefenseCharacter);
				}
			}
			pending = list;
			if (pending.Count == 0 || num == 0)
			{
				break;
			}
			retryPass++;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (!IsLifetimeActive)
			{
				break;
			}
		}
		return spawnedCharacters;
	}

	private void RegisterPreSpawnCharacter(TowerDefenseCharacter character)
	{
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && GodotObject.IsInstanceValid(control) && GodotObject.IsInstanceValid(character) && character.syncId < 0)
		{
			int nextSyncId = control.GetNextSyncId();
			control.RegisterSyncCharacter(nextSyncId, character);
		}
	}

	public override Task GameStart()
	{
		if (!IsLifetimeActive)
		{
			return Task.CompletedTask;
		}
		foreach (TowerDefenseCharacter preSpawn in preSpawnList)
		{
			if (!GodotObject.IsInstanceValid(preSpawn))
			{
				continue;
			}
			preSpawn.ActivateGameplayProcessing();
			preSpawn.CallDeferred("ActivateGameplayProcessing");
			preSpawn.PreSpawn();
			if (preSpawn is TowerDefensePlant towerDefensePlant)
			{
				if (towerDefensePlant.CanSleep())
				{
					towerDefensePlant.CallDeferred("Sleep");
				}
				else if (!towerDefensePlant.componentRunning)
				{
					towerDefensePlant.CallDeferred("Idle");
				}
			}
			if (preSpawn is TowerDefenseZombie towerDefenseZombie)
			{
				towerDefenseZombie.CallDeferred("Walk");
			}
		}
		preSpawnList.Clear();
		return Task.CompletedTask;
	}

	private static void DestroyPendingCharacters(IEnumerable<TowerDefenseCharacter> characters)
	{
		if (characters == null)
		{
			return;
		}
		foreach (TowerDefenseCharacter character in characters)
		{
			if (GodotObject.IsInstanceValid(character))
			{
				character.Destroy();
			}
		}
	}

	public override void Destroy()
	{
		_pendingPlacementReservations = false;
		DestroyPendingCharacters(preSpawnList);
		preSpawnList.Clear();
		config?.Clear();
		config = null;
		base.Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasPendingGravestoneReservation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleasePendingPlacementReservationsDeferred, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterPreSpawnCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasPendingGravestoneReservation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPendingGravestoneReservation(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.ReleasePendingPlacementReservationsDeferred && args.Count == 0)
		{
			ReleasePendingPlacementReservationsDeferred();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterPreSpawnCharacter && args.Count == 1)
		{
			RegisterPreSpawnCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.HasPendingGravestoneReservation)
		{
			return true;
		}
		if (method == MethodName.ReleasePendingPlacementReservationsDeferred)
		{
			return true;
		}
		if (method == MethodName.RegisterPreSpawnCharacter)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseBattleFeaturePreSpawnConfig>(in value);
			return true;
		}
		if (name == PropertyName.preSpawnList)
		{
			preSpawnList = VariantUtils.ConvertToArray<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._pendingPlacementReservations)
		{
			_pendingPlacementReservations = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.preSpawnList)
		{
			value = VariantUtils.CreateFromArray(preSpawnList);
			return true;
		}
		if (name == PropertyName._pendingPlacementReservations)
		{
			value = VariantUtils.CreateFrom(in _pendingPlacementReservations);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.preSpawnList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingPlacementReservations, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.preSpawnList, Variant.CreateFrom(preSpawnList));
		info.AddProperty(PropertyName._pendingPlacementReservations, Variant.From(in _pendingPlacementReservations));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.config, out var value))
		{
			config = value.As<TowerDefenseBattleFeaturePreSpawnConfig>();
		}
		if (info.TryGetProperty(PropertyName.preSpawnList, out var value2))
		{
			preSpawnList = value2.AsGodotArray<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._pendingPlacementReservations, out var value3))
		{
			_pendingPlacementReservations = value3.As<bool>();
		}
	}
}
