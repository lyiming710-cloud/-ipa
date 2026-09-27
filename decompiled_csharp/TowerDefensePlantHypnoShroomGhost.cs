using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Diamond/HypnoShroomGhost/Scene/TowerDefensePlantHypnoShroomGhost.cs")]
public class TowerDefensePlantHypnoShroomGhost : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ResetBattleState = "ResetBattleState";

		public static readonly StringName OnCharacterHypnotized = "OnCharacterHypnotized";

		public static readonly StringName EnsureBaseCostInitialized = "EnsureBaseCostInitialized";

		public static readonly StringName SetPacketCost = "SetPacketCost";

		public static readonly StringName ResetPacketCost = "ResetPacketCost";

		public static readonly StringName Explode = "Explode";

		public static readonly StringName DecreaseCost = "DecreaseCost";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _initialized = "_initialized";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const int CostDecreasePerHypnosis = 25;

	private const string PacketName = "PlantHypnoShroomGhost";

	private ExplodeComponent _explodeComponent;

	private bool _initialized;

	private static readonly HashSet<TowerDefenseCharacter> _explodingZombies = new HashSet<TowerDefenseCharacter>();

	private static readonly Dictionary<TowerDefensePacketConfig, int> _baseCosts = new Dictionary<TowerDefensePacketConfig, int>();

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			_explodeComponent.OnExplode += Explode;
			_initialized = true;
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
		_explodeComponent = null;
		_initialized = false;
	}

	internal static void ResetBattleState()
	{
		_explodingZombies.Clear();
		_baseCosts.Clear();
	}

	internal static void OnCharacterHypnotized(TowerDefenseCharacter character)
	{
		if (_explodingZombies.Contains(character))
		{
			return;
		}
		List<TowerDefenseInGamePacketShow> hypnoShroomPacketShows = GetHypnoShroomPacketShows();
		TowerDefensePacketConfig towerDefensePacketConfig = null;
		foreach (TowerDefenseInGamePacketShow item in hypnoShroomPacketShows)
		{
			if (GodotObject.IsInstanceValid(item) && GodotObject.IsInstanceValid(item.config) && ((item.originalSaveKey != "") ? item.originalSaveKey : item.config.saveKey) == "PlantHypnoShroomGhost")
			{
				towerDefensePacketConfig = item.config;
				break;
			}
		}
		if (GodotObject.IsInstanceValid(towerDefensePacketConfig) && (towerDefensePacketConfig.GetHypnoses() ? (character is TowerDefensePlant) : (character is TowerDefenseZombie)))
		{
			DecreaseCost();
		}
	}

	private static List<TowerDefenseInGamePacketShow> GetHypnoShroomPacketShows()
	{
		List<TowerDefenseInGamePacketShow> list = new List<TowerDefenseInGamePacketShow>();
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return list;
		}
		list.AddRange(TowerDefenseManager.Instance.GetSeedBankList());
		TowerDefenseBattleFeatureConveyorBelt conveyorBeltFeature = TowerDefenseManager.Instance.GetConveyorBeltFeature();
		if (GodotObject.IsInstanceValid(conveyorBeltFeature))
		{
			foreach (Node packetChild in conveyorBeltFeature.GetPacketChildren())
			{
				if (packetChild is TowerDefenseInGamePacketShow item)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	private static void EnsureBaseCostInitialized(TowerDefensePacketConfig config)
	{
		if (!_baseCosts.ContainsKey(config))
		{
			_baseCosts[config] = config.GetCost();
		}
	}

	private static void SetPacketCost(TowerDefensePacketConfig config, int cost)
	{
		if (GodotObject.IsInstanceValid(config._override) && _baseCosts.ContainsKey(config) && _baseCosts[config] != config.characterConfig.cost)
		{
			config._override.cost = cost;
		}
		else if (GodotObject.IsInstanceValid(config._override) && config._override.cost != -1)
		{
			config._override.cost = cost;
		}
		else
		{
			config.overrideCost = cost;
		}
	}

	private static void ResetPacketCost(TowerDefensePacketConfig config)
	{
		if (_baseCosts.TryGetValue(config, out var value))
		{
			SetPacketCost(config, value);
			return;
		}
		config.overrideCost = -1;
		if (GodotObject.IsInstanceValid(config._override))
		{
			config._override.cost = -1;
		}
	}

	public void Explode()
	{
		_explodingZombies.Clear();
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ZombieFootballGargantuarBlack");
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			Callable.From(() =>
			{
				_explodingZombies.Clear();
			}).CallDeferred();
			return;
		}
		for (int num = 1; num <= mapGridNum.Y; num++)
		{
			if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
			{
				Destroy();
				Callable.From(() =>
				{
					_explodingZombies.Clear();
				}).CallDeferred();
				return;
			}
			try
			{
				Vector2I vector2I = new Vector2I((!instance.hypnoses) ? 1 : mapGridNum.X, num);
				Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(vector2I);
				TowerDefenseCharacter zombie = packetConfig.Create(mapCellPlantPos, vector2I, groundHeight);
				if (!GodotObject.IsInstanceValid(zombie))
				{
					continue;
				}
				TowerDefenseGroundItemBase.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				if (!instance.hypnoses)
				{
					_explodingZombies.Add(zombie);
					zombie.Hypnoses();
				}
				IStateMachineController stateMachine = zombie.StateMachine;
				if (stateMachine != null && stateMachine.IsInitialized)
				{
					zombie.SetMainStateMachineDispatchEnabled(enabled: false);
				}
				Tween tween = zombie.CreateTween();
				tween.SetEase(Tween.EaseType.Out);
				tween.SetTrans(Tween.TransitionType.Back);
				tween.TweenProperty(zombie.transformPoint, "scale", Vector2.One, 0.5).From(Vector2.One * 0.5f);
				tween.Finished += () =>
				{
					if (GodotObject.IsInstanceValid(zombie))
					{
						((TowerDefenseZombie)zombie).Walk();
						IStateMachineController stateMachine2 = zombie.StateMachine;
						if (stateMachine2 != null && stateMachine2.IsInitialized)
						{
							zombie.SetMainStateMachineDispatchEnabled(enabled: true);
						}
					}
				};
				if (Global.Instance.isMultiplayerMode && MultiPlayerManager.Instance.isHost)
				{
					TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
					if (GodotObject.IsInstanceValid(currentControl))
					{
						int nextSyncId = currentControl.GetNextSyncId();
						currentControl.RegisterSyncCharacter(nextSyncId, zombie);
						MultiPlayerManager.Instance.SendSpawnCharacterAt("ZombieFootballGargantuarBlack", vector2I.X, vector2I.Y, nextSyncId, 1.0, 1.0, !instance.hypnoses, 0.0, useCreate: true, mapCellPlantPos.X, mapCellPlantPos.Y, walkAfterSpawn: true, groundHeight);
					}
				}
			}
			catch (Exception ex)
			{
				GD.PrintErr($"HypnoShroomGhost Explode error at row {num}: {ex.Message}");
			}
		}
		foreach (TowerDefenseInGamePacketShow hypnoShroomPacketShow in GetHypnoShroomPacketShows())
		{
			if (GodotObject.IsInstanceValid(hypnoShroomPacketShow) && GodotObject.IsInstanceValid(hypnoShroomPacketShow.config) && !(((hypnoShroomPacketShow.originalSaveKey != "") ? hypnoShroomPacketShow.originalSaveKey : hypnoShroomPacketShow.config.saveKey) != "PlantHypnoShroomGhost"))
			{
				EnsureBaseCostInitialized(hypnoShroomPacketShow.config);
				ResetPacketCost(hypnoShroomPacketShow.config);
			}
		}
		Callable.From(() =>
		{
			_explodingZombies.Clear();
		}).CallDeferred();
	}

	private static void DecreaseCost()
	{
		foreach (TowerDefenseInGamePacketShow hypnoShroomPacketShow in GetHypnoShroomPacketShows())
		{
			if (!GodotObject.IsInstanceValid(hypnoShroomPacketShow) || !GodotObject.IsInstanceValid(hypnoShroomPacketShow.config) || ((hypnoShroomPacketShow.originalSaveKey != "") ? hypnoShroomPacketShow.originalSaveKey : hypnoShroomPacketShow.config.saveKey) != "PlantHypnoShroomGhost")
			{
				continue;
			}
			EnsureBaseCostInitialized(hypnoShroomPacketShow.config);
			int num = hypnoShroomPacketShow.config.GetCost();
			if (num >= 0)
			{
				int num2 = num - 25;
				if (num2 < 0)
				{
					num2 = 0;
				}
				SetPacketCost(hypnoShroomPacketShow.config, num2);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetBattleState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.OnCharacterHypnotized, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureBaseCostInitialized, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetPacketCost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetPacketCost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DecreaseCost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
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
		if (method == MethodName.ResetBattleState && args.Count == 0)
		{
			ResetBattleState();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCharacterHypnotized && args.Count == 1)
		{
			OnCharacterHypnotized(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureBaseCostInitialized && args.Count == 1)
		{
			EnsureBaseCostInitialized(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPacketCost && args.Count == 2)
		{
			SetPacketCost(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetPacketCost && args.Count == 1)
		{
			ResetPacketCost(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Explode && args.Count == 0)
		{
			Explode();
			ret = default;
			return true;
		}
		if (method == MethodName.DecreaseCost && args.Count == 0)
		{
			DecreaseCost();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResetBattleState && args.Count == 0)
		{
			ResetBattleState();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCharacterHypnotized && args.Count == 1)
		{
			OnCharacterHypnotized(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureBaseCostInitialized && args.Count == 1)
		{
			EnsureBaseCostInitialized(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPacketCost && args.Count == 2)
		{
			SetPacketCost(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetPacketCost && args.Count == 1)
		{
			ResetPacketCost(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DecreaseCost && args.Count == 0)
		{
			DecreaseCost();
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
		if (method == MethodName.ResetBattleState)
		{
			return true;
		}
		if (method == MethodName.OnCharacterHypnotized)
		{
			return true;
		}
		if (method == MethodName.EnsureBaseCostInitialized)
		{
			return true;
		}
		if (method == MethodName.SetPacketCost)
		{
			return true;
		}
		if (method == MethodName.ResetPacketCost)
		{
			return true;
		}
		if (method == MethodName.Explode)
		{
			return true;
		}
		if (method == MethodName.DecreaseCost)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._initialized)
		{
			_initialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._initialized)
		{
			value = VariantUtils.CreateFrom(in _initialized);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._initialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._initialized, Variant.From(in _initialized));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._initialized, out var value))
		{
			_initialized = value.As<bool>();
		}
	}
}
