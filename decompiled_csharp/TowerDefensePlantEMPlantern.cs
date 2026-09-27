using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/EMPlantern/Scene/TowerDefensePlantEMPlantern.cs")]
public class TowerDefensePlantEMPlantern : TowerDefensePlant
{
	private sealed class PairState
	{
		public string Direction;

		public Vector2I TargetGrid;

		public double NextPulse;

		public double ActiveRemaining;

		public int Serial;

		public readonly HashSet<string> HitTargets = new HashSet<string>(StringComparer.Ordinal);

		public Node2D Effect;
	}

	private sealed class RestoredPairState
	{
		public string Direction;

		public Vector2I TargetGrid;

		public double NextPulse;

		public double ActiveRemaining;

		public int Serial;

		public readonly HashSet<string> HitTargets = new HashSet<string>(StringComparer.Ordinal);
	}

	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ApplyAuraSlow = "ApplyAuraSlow";

		public static readonly StringName DoAuraDamage = "DoAuraDamage";

		public static readonly StringName IsNormalTarget = "IsNormalTarget";

		public static readonly StringName UpdateAuthoritativePairs = "UpdateAuthoritativePairs";

		public static readonly StringName FindNearestPartner = "FindNearestPartner";

		public static readonly StringName PlayShootingAnimation = "PlayShootingAnimation";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName GetCurrentAnchorPosition = "GetCurrentAnchorPosition";

		public static readonly StringName RemovePair = "RemovePair";

		public static readonly StringName RemoveRestoredDirection = "RemoveRestoredDirection";

		public static readonly StringName GetPairSaveKey = "GetPairSaveKey";

		public static readonly StringName UpdateRemoteVisuals = "UpdateRemoteVisuals";

		public static readonly StringName FindPlantAtGrid = "FindPlantAtGrid";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public static readonly StringName IsElectromagneticTarget = "IsElectromagneticTarget";

		public static readonly StringName HasMetalArmor = "HasMetalArmor";

		public static readonly StringName IsBoss = "IsBoss";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _light = "_light";

		public static readonly StringName _auraTimer = "_auraTimer";

		public static readonly StringName _networkRevision = "_networkRevision";

		public static readonly StringName _lastShootFrame = "_lastShootFrame";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	public const double PulseInterval = 3.0;

	public const double AuraDamage = 80.0;

	public const double CurrentDamage = 200.0;

	public const double CurrentDuration = 1.6666666666666667;

	public const double EmpDuration = 1.0;

	public const double CurrentImmobilizeChance = 0.2;

	private static readonly (string Key, Vector2I Step)[] PairDirections = new (string, Vector2I)[2]
	{
		("Right", Vector2I.Right),
		("Down", Vector2I.Down)
	};

	private static PackedScene _currentScene;

	private readonly System.Collections.Generic.Dictionary<string, PairState> _pairs = new System.Collections.Generic.Dictionary<string, PairState>(StringComparer.Ordinal);

	private readonly System.Collections.Generic.Dictionary<string, RestoredPairState> _restoredPairs = new System.Collections.Generic.Dictionary<string, RestoredPairState>(StringComparer.Ordinal);

	private PointLight2D _light;

	private double _auraTimer = 3.0;

	private int _networkRevision;

	private ulong _lastShootFrame = 18446744073709551615uL;

	private static PackedScene CurrentScene => _currentScene ?? (_currentScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter9/EMPlantern/Effect/Current/EMPlanternCurrent.tscn"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_light = GetNodeOrNull<PointLight2D>("%Light");
			AudioManager.Instance.AudioPlay("Plantern");
		}
	}

	public override void _ExitTree()
	{
		foreach (PairState value in _pairs.Values)
		{
			RemoveEffect(value);
		}
		_pairs.Clear();
		base._ExitTree();
	}

	public override void BatchUpdate(double delta)
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		base.BatchUpdate(delta);
		if (GodotObject.IsInstanceValid(_light))
		{
			_light.Visible = TowerDefenseManager.GetMapIsNight() && GameSaveManager.Instance.GetConfigValue("MapEffect").AsBool();
		}
		if (!inGame || die || nearDie || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance) || !TowerDefenseManager.Instance.IsGameRunning())
		{
			return;
		}
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			UpdateRemoteVisuals(delta);
			return;
		}
		ApplyAuraSlow();
		_auraTimer -= delta;
		if (_auraTimer <= 0.0)
		{
			DoAuraDamage();
			_auraTimer += 3.0;
			_networkRevision++;
		}
		UpdateAuthoritativePairs(delta);
	}

	private void ApplyAuraSlow()
	{
		foreach (TowerDefenseCharacter item in EnumerateAuraTargets())
		{
			if (IsElectromagneticTarget(item) && !IsBoss(item) && !item.buff.BuffHas("IceSpeedDown") && !item.buff.BuffHas("Frozen"))
			{
				item.buff.AddBuff(new TowerDefenseCharacterBuffEMSpeedDown
				{
					time = 0.35
				});
			}
		}
	}

	private void DoAuraDamage()
	{
		foreach (TowerDefenseCharacter item in EnumerateAuraTargets())
		{
			double num = (IsElectromagneticTarget(item) ? 160.0 : 80.0);
			TowerDefenseCharacterEventHurt.Run(GetLogicalGlobalPosition(), item, num);
		}
	}

	private List<TowerDefenseCharacter> EnumerateAuraTargets()
	{
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return list;
		}
		foreach (Variant item in TowerDefenseManager.Instance.GetCampTarget(camp))
		{
			TowerDefenseCharacter towerDefenseCharacter = item.As<TowerDefenseCharacter>();
			if (IsNormalTarget(towerDefenseCharacter))
			{
				Vector2I vector2I = towerDefenseCharacter.gridPos - gridPos;
				if (Math.Abs(vector2I.X) <= 1 && Math.Abs(vector2I.Y) <= 1)
				{
					list.Add(towerDefenseCharacter);
				}
			}
		}
		return list;
	}

	private bool IsNormalTarget(TowerDefenseCharacter target)
	{
		if (GodotObject.IsInstanceValid(target) && GodotObject.IsInstanceValid(target.instance) && !target.die && !target.nearDie && !target.instance.invincible && CanTarget(target))
		{
			return CanCollision(target.instance.maskFlags);
		}
		return false;
	}

	private void UpdateAuthoritativePairs(double delta)
	{
		(string, Vector2I)[] pairDirections = PairDirections;
		for (int i = 0; i < pairDirections.Length; i++)
		{
			(string, Vector2I) tuple = pairDirections[i];
			string item = tuple.Item1;
			Vector2I item2 = tuple.Item2;
			TowerDefensePlantEMPlantern towerDefensePlantEMPlantern = FindNearestPartner(item2);
			if (!GodotObject.IsInstanceValid(towerDefensePlantEMPlantern))
			{
				RemovePair(item);
				RemoveRestoredDirection(item);
				continue;
			}
			if (!_pairs.TryGetValue(item, out var value) || value.TargetGrid != towerDefensePlantEMPlantern.gridPos)
			{
				RemovePair(item);
				RestoredPairState restoredPairState = TakeRestoredState(item, towerDefensePlantEMPlantern.gridPos);
				RemoveRestoredDirection(item);
				value = new PairState
				{
					Direction = item,
					TargetGrid = towerDefensePlantEMPlantern.gridPos,
					NextPulse = (restoredPairState?.NextPulse ?? 0.0),
					ActiveRemaining = (restoredPairState?.ActiveRemaining ?? 0.0),
					Serial = (restoredPairState?.Serial ?? 0)
				};
				if (restoredPairState != null)
				{
					value.HitTargets.UnionWith(restoredPairState.HitTargets);
				}
				_pairs[item] = value;
				_networkRevision++;
			}
			value.NextPulse -= delta;
			if (value.NextPulse <= 0.0)
			{
				TriggerPulse(value, towerDefensePlantEMPlantern);
				value.NextPulse += 3.0;
			}
			UpdateActivePulse(value, towerDefensePlantEMPlantern, delta, applyGameplay: true);
		}
	}

	private TowerDefensePlantEMPlantern FindNearestPartner(Vector2I step)
	{
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return null;
		}
		for (Vector2I vector2I = gridPos + step; TowerDefenseManager.Instance.CheckMapGridPosIn(vector2I); vector2I += step)
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(vector2I);
			TowerDefensePlantEMPlantern towerDefensePlantEMPlantern = null;
			if (GodotObject.IsInstanceValid(mapCell))
			{
				foreach (TowerDefenseCharacter character in mapCell.GetCharacterList())
				{
					if (character is TowerDefensePlantEMPlantern towerDefensePlantEMPlantern2 && towerDefensePlantEMPlantern2 != this && !towerDefensePlantEMPlantern2.die && !towerDefensePlantEMPlantern2.nearDie && towerDefensePlantEMPlantern2.camp == camp && (towerDefensePlantEMPlantern == null || towerDefensePlantEMPlantern2.GetInstanceId() < towerDefensePlantEMPlantern.GetInstanceId()))
					{
						towerDefensePlantEMPlantern = towerDefensePlantEMPlantern2;
					}
				}
			}
			if (GodotObject.IsInstanceValid(towerDefensePlantEMPlantern))
			{
				return towerDefensePlantEMPlantern;
			}
		}
		return null;
	}

	private void TriggerPulse(PairState state, TowerDefensePlantEMPlantern partner)
	{
		state.ActiveRemaining = 1.6666666666666667;
		state.Serial++;
		state.HitTargets.Clear();
		EnsureEffect(state);
		AudioManager.Instance.AudioPlay("Spike");
		PlayShootingAnimation();
		partner.PlayShootingAnimation();
		_networkRevision++;
	}

	private void UpdateActivePulse(PairState state, TowerDefensePlantEMPlantern partner, double delta, bool applyGameplay)
	{
		if (!(state.ActiveRemaining <= 0.0))
		{
			UpdateEffectTransform(state, partner);
			if (applyGameplay)
			{
				HitCurrentTargets(state);
			}
			state.ActiveRemaining -= delta;
			if (!(state.ActiveRemaining > 0.0))
			{
				state.ActiveRemaining = 0.0;
				RemoveEffect(state);
				_networkRevision++;
			}
		}
	}

	private void HitCurrentTargets(PairState state)
	{
		Vector2 mapCellPosCenter = TowerDefenseManager.Instance.GetMapCellPosCenter(gridPos);
		Vector2 mapCellPosCenter2 = TowerDefenseManager.Instance.GetMapCellPosCenter(state.TargetGrid);
		foreach (Variant item in TowerDefenseManager.Instance.GetCampTarget(camp))
		{
			TowerDefenseCharacter towerDefenseCharacter = item.As<TowerDefenseCharacter>();
			string text = (GodotObject.IsInstanceValid(towerDefenseCharacter) ? towerDefenseCharacter.Name.ToString().ValidateNodeName() : string.Empty);
			if (IsNormalTarget(towerDefenseCharacter) && !string.IsNullOrEmpty(text) && !state.HitTargets.Contains(text) && AabbShapeUtil.SegmentIntersectsRect(mapCellPosCenter, mapCellPosCenter2, towerDefenseCharacter.WorldHitRect, out var _))
			{
				state.HitTargets.Add(text);
				bool flag = IsElectromagneticTarget(towerDefenseCharacter);
				TowerDefenseCharacterEventHurt.Run((mapCellPosCenter + mapCellPosCenter2) * 0.5f, towerDefenseCharacter, flag ? 400.0 : 200.0);
				if (!IsBoss(towerDefenseCharacter) && (double)GD.Randf() < (flag ? 0.4 : 0.2))
				{
					towerDefenseCharacter.buff.AddBuff(new TowerDefenseCharacterBuffEMP
					{
						time = 1.0
					});
				}
			}
		}
	}

	private void PlayShootingAnimation()
	{
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		if (_lastShootFrame != currentPhysicsFrame)
		{
			_lastShootFrame = currentPhysicsFrame;
			if (GodotObject.IsInstanceValid(sprite) && sprite.HasClip("anim_shooting"))
			{
				sprite.SetAnimation("anim_shooting", loop: false);
			}
		}
	}

	public override void AnimeCompleted(string clipName)
	{
		base.AnimeCompleted(clipName);
		if (clipName == "anim_shooting" && GodotObject.IsInstanceValid(sprite) && sprite.HasClip("Idle"))
		{
			sprite.SetAnimation("Idle");
		}
	}

	private void EnsureEffect(PairState state)
	{
		if (!GodotObject.IsInstanceValid(state.Effect) && GodotObject.IsInstanceValid(CurrentScene))
		{
			state.Effect = CurrentScene.Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
			TowerDefenseManager.GetCharacterNode().AddChild(state.Effect, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void UpdateEffectTransform(PairState state, TowerDefensePlantEMPlantern partner)
	{
		EnsureEffect(state);
		if (GodotObject.IsInstanceValid(state.Effect) && GodotObject.IsInstanceValid(partner))
		{
			Vector2 currentAnchorPosition = GetCurrentAnchorPosition();
			Vector2 currentAnchorPosition2 = partner.GetCurrentAnchorPosition();
			Vector2 vector = currentAnchorPosition2 - currentAnchorPosition;
			state.Effect.GlobalPosition = (currentAnchorPosition + currentAnchorPosition2) * 0.5f;
			state.Effect.GlobalRotation = vector.Angle();
			state.Effect.Scale = new Vector2(Mathf.Max(0.01f, vector.Length() / 256f), 0.6f);
		}
	}

	private Vector2 GetCurrentAnchorPosition()
	{
		Marker2D nodeOrNull = GetNodeOrNull<Marker2D>("%CurrentAnchor");
		if (!GodotObject.IsInstanceValid(nodeOrNull))
		{
			return GetLogicalGlobalPosition();
		}
		return nodeOrNull.GlobalPosition;
	}

	private void RemovePair(string direction)
	{
		if (_pairs.Remove(direction, out var value))
		{
			RemoveEffect(value);
			_networkRevision++;
		}
	}

	private static void RemoveEffect(PairState state)
	{
		if (GodotObject.IsInstanceValid(state?.Effect))
		{
			state.Effect.QueueFree();
		}
		if (state != null)
		{
			state.Effect = null;
		}
	}

	private RestoredPairState TakeRestoredState(string direction, Vector2I targetGrid)
	{
		string pairSaveKey = GetPairSaveKey(direction, targetGrid);
		if (!_restoredPairs.Remove(pairSaveKey, out var value))
		{
			return null;
		}
		return value;
	}

	private void RemoveRestoredDirection(string direction)
	{
		foreach (string item in new List<string>(_restoredPairs.Keys))
		{
			if (_restoredPairs[item].Direction == direction)
			{
				_restoredPairs.Remove(item);
			}
		}
	}

	private static string GetPairSaveKey(string direction, Vector2I targetGrid)
	{
		return $"{direction}:{targetGrid.X}:{targetGrid.Y}";
	}

	private void UpdateRemoteVisuals(double delta)
	{
		foreach (PairState value in _pairs.Values)
		{
			if (!(value.ActiveRemaining <= 0.0))
			{
				TowerDefensePlantEMPlantern partner = FindPlantAtGrid(value.TargetGrid);
				if (GodotObject.IsInstanceValid(partner))
				{
					UpdateActivePulse(value, partner, delta, applyGameplay: false);
					continue;
				}
				RemoveEffect(value);
				value.ActiveRemaining = Math.Max(0.0, value.ActiveRemaining - delta);
			}
		}
	}

	private TowerDefensePlantEMPlantern FindPlantAtGrid(Vector2I targetGrid)
	{
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(targetGrid);
		if (!GodotObject.IsInstanceValid(mapCell))
		{
			return null;
		}
		foreach (TowerDefenseCharacter character in mapCell.GetCharacterList())
		{
			if (character is TowerDefensePlantEMPlantern towerDefensePlantEMPlantern && towerDefensePlantEMPlantern != this && !towerDefensePlantEMPlantern.die && !towerDefensePlantEMPlantern.nearDie && towerDefensePlantEMPlantern.camp == camp)
			{
				return towerDefensePlantEMPlantern;
			}
		}
		return null;
	}

	public override Dictionary ExportVariantSave()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		(string, Vector2I)[] pairDirections = PairDirections;
		for (int i = 0; i < pairDirections.Length; i++)
		{
			string item = pairDirections[i].Item1;
			if (_pairs.TryGetValue(item, out var value))
			{
				array.Add(CreatePairSaveData(item, value.TargetGrid, value.NextPulse, value.ActiveRemaining, value.Serial, value.HitTargets));
			}
		}
		foreach (RestoredPairState value2 in _restoredPairs.Values)
		{
			array.Add(CreatePairSaveData(value2.Direction, value2.TargetGrid, value2.NextPulse, value2.ActiveRemaining, value2.Serial, value2.HitTargets));
		}
		return new Dictionary
		{
			["auraTimer"] = _auraTimer,
			["pairTimers"] = array
		};
	}

	private static Dictionary CreatePairSaveData(string direction, Vector2I targetGrid, double nextPulse, double activeRemaining, int serial, IEnumerable<string> hitTargetNames)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		if (activeRemaining > 0.0)
		{
			foreach (string hitTargetName in hitTargetNames)
			{
				array.Add(hitTargetName);
			}
		}
		return new Dictionary
		{
			["direction"] = direction,
			["x"] = targetGrid.X,
			["y"] = targetGrid.Y,
			["next"] = nextPulse,
			["active"] = activeRemaining,
			["serial"] = serial,
			["hitTargets"] = array
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		_auraTimer = Mathf.Clamp(data.GetValueOrDefault("auraTimer", 3.0).AsDouble(), 0.0, 3.0);
		_restoredPairs.Clear();
		foreach (Variant item in data.GetValueOrDefault("pairTimers", new Godot.Collections.Array()).AsGodotArray())
		{
			if (item.VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary = item.AsGodotDictionary();
			string text = dictionary.GetValueOrDefault("direction", "").AsString();
			Vector2I targetGrid = new Vector2I(dictionary.GetValueOrDefault("x", 0).AsInt32(), dictionary.GetValueOrDefault("y", 0).AsInt32());
			if ((!(text == "Right") && !(text == "Down")) || 1 == 0)
			{
				continue;
			}
			RestoredPairState restoredPairState = new RestoredPairState
			{
				Direction = text,
				TargetGrid = targetGrid,
				NextPulse = Mathf.Clamp(dictionary.GetValueOrDefault("next", 3.0).AsDouble(), 0.0, 3.0),
				ActiveRemaining = Mathf.Clamp(dictionary.GetValueOrDefault("active", 0.0).AsDouble(), 0.0, 1.6666666666666667),
				Serial = Math.Max(0, dictionary.GetValueOrDefault("serial", 0).AsInt32())
			};
			foreach (Variant item2 in dictionary.GetValueOrDefault("hitTargets", new Godot.Collections.Array()).AsGodotArray())
			{
				string text2 = item2.AsString();
				if (!string.IsNullOrEmpty(text2))
				{
					restoredPairState.HitTargets.Add(text2);
				}
			}
			_restoredPairs[GetPairSaveKey(text, targetGrid)] = restoredPairState;
		}
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		(string, Vector2I)[] pairDirections = PairDirections;
		for (int i = 0; i < pairDirections.Length; i++)
		{
			string item = pairDirections[i].Item1;
			if (_pairs.TryGetValue(item, out var value))
			{
				array.Add(new Dictionary
				{
					["vertical"] = item == "Down",
					["x"] = value.TargetGrid.X,
					["y"] = value.TargetGrid.Y,
					["active"] = ((value.ActiveRemaining > 0.0) ? 1.6666666666666667 : 0.0),
					["serial"] = value.Serial
				});
			}
		}
		return new Dictionary { ["pairs"] = array };
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		if (TowerDefenseManager.HasGameplayAuthority || data == null)
		{
			return;
		}
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		foreach (Variant item in data.GetValueOrDefault("pairs", new Godot.Collections.Array()).AsGodotArray())
		{
			if (item.VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary = item.AsGodotDictionary();
			string text;
			if (dictionary.ContainsKey("vertical"))
			{
				text = (dictionary["vertical"].AsBool() ? "Down" : "Right");
			}
			else
			{
				Variant valueOrDefault = dictionary.GetValueOrDefault("direction", -1);
				Variant.Type variantType = valueOrDefault.VariantType;
				bool flag = (((ulong)(variantType - 2) <= 1uL) ? true : false);
				string text2 = ((!flag) ? valueOrDefault.AsString() : (valueOrDefault.AsInt32() switch
				{
					0 => "Right", 
					1 => "Down", 
					_ => "", 
				}));
				text = text2;
			}
			if ((text == "Right" || text == "Down") ? true : false)
			{
				hashSet.Add(text);
				Vector2I vector2I = new Vector2I(dictionary.GetValueOrDefault("x", 0).AsInt32(), dictionary.GetValueOrDefault("y", 0).AsInt32());
				int num = dictionary.GetValueOrDefault("serial", 0).AsInt32();
				double num2 = Math.Max(0.0, dictionary.GetValueOrDefault("active", 0.0).AsDouble());
				if (!_pairs.TryGetValue(text, out var value) || value.TargetGrid != vector2I)
				{
					RemovePair(text);
					value = new PairState
					{
						Direction = text,
						TargetGrid = vector2I
					};
					_pairs[text] = value;
				}
				bool num3 = num != value.Serial && num2 > 0.0;
				value.Serial = num;
				value.ActiveRemaining = num2;
				if (num2 > 0.0)
				{
					EnsureEffect(value);
				}
				else
				{
					RemoveEffect(value);
				}
				if (num3)
				{
					PlayShootingAnimation();
				}
			}
		}
		foreach (string item2 in new List<string>(_pairs.Keys))
		{
			if (!hashSet.Contains(item2))
			{
				RemovePair(item2);
			}
		}
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return _networkRevision;
	}

	public static bool IsElectromagneticTarget(TowerDefenseCharacter target)
	{
		if (!GodotObject.IsInstanceValid(target) || !GodotObject.IsInstanceValid(target.instance))
		{
			return false;
		}
		if ((target.instance.physiqueTypeFlags & 0x800) != 0)
		{
			return true;
		}
		if (!HasMetalArmor(target.GetArmorHeadCover()) && !HasMetalArmor(target.GetArmorShield()) && !HasMetalArmor(target.GetArmorHelment()))
		{
			return HasMetalArmor(target.GetArmor());
		}
		return true;
	}

	private static bool HasMetalArmor(Array<TowerDefenseArmorInstance> armors)
	{
		foreach (TowerDefenseArmorInstance armor in armors)
		{
			if (GodotObject.IsInstanceValid(armor) && !armor.isRemove && armor.IsMetallic())
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsBoss(TowerDefenseCharacter target)
	{
		if (target is TowerDefenseZombie)
		{
			return target.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(24)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyAuraSlow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoAuraDamage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsNormalTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateAuthoritativePairs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindNearestPartner, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayShootingAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentAnchorPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemovePair, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveRestoredDirection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPairSaveKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "targetGrid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateRemoteVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindPlantAtGrid, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "targetGrid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsElectromagneticTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasMetalArmor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "armors", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsBoss, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.ApplyAuraSlow && args.Count == 0)
		{
			ApplyAuraSlow();
			ret = default;
			return true;
		}
		if (method == MethodName.DoAuraDamage && args.Count == 0)
		{
			DoAuraDamage();
			ret = default;
			return true;
		}
		if (method == MethodName.IsNormalTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNormalTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateAuthoritativePairs && args.Count == 1)
		{
			UpdateAuthoritativePairs(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindNearestPartner && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlantEMPlantern>(FindNearestPartner(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.PlayShootingAnimation && args.Count == 0)
		{
			PlayShootingAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentAnchorPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCurrentAnchorPosition());
			return true;
		}
		if (method == MethodName.RemovePair && args.Count == 1)
		{
			RemovePair(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveRestoredDirection && args.Count == 1)
		{
			RemoveRestoredDirection(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPairSaveKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetPairSaveKey(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.UpdateRemoteVisuals && args.Count == 1)
		{
			UpdateRemoteVisuals(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindPlantAtGrid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlantEMPlantern>(FindPlantAtGrid(VariantUtils.ConvertTo<Vector2I>(in args[0])));
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
		if (method == MethodName.ExportNetworkSpecialState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpecialState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState && args.Count == 1)
		{
			ImportNetworkSpecialState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNetworkSpecialStateRevision());
			return true;
		}
		if (method == MethodName.IsElectromagneticTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsElectromagneticTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.HasMetalArmor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasMetalArmor(VariantUtils.ConvertToArray<TowerDefenseArmorInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.IsBoss && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBoss(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetPairSaveKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetPairSaveKey(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.IsElectromagneticTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsElectromagneticTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.HasMetalArmor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasMetalArmor(VariantUtils.ConvertToArray<TowerDefenseArmorInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.IsBoss && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBoss(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.ApplyAuraSlow)
		{
			return true;
		}
		if (method == MethodName.DoAuraDamage)
		{
			return true;
		}
		if (method == MethodName.IsNormalTarget)
		{
			return true;
		}
		if (method == MethodName.UpdateAuthoritativePairs)
		{
			return true;
		}
		if (method == MethodName.FindNearestPartner)
		{
			return true;
		}
		if (method == MethodName.PlayShootingAnimation)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.GetCurrentAnchorPosition)
		{
			return true;
		}
		if (method == MethodName.RemovePair)
		{
			return true;
		}
		if (method == MethodName.RemoveRestoredDirection)
		{
			return true;
		}
		if (method == MethodName.GetPairSaveKey)
		{
			return true;
		}
		if (method == MethodName.UpdateRemoteVisuals)
		{
			return true;
		}
		if (method == MethodName.FindPlantAtGrid)
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
		if (method == MethodName.ExportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision)
		{
			return true;
		}
		if (method == MethodName.IsElectromagneticTarget)
		{
			return true;
		}
		if (method == MethodName.HasMetalArmor)
		{
			return true;
		}
		if (method == MethodName.IsBoss)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._light)
		{
			_light = VariantUtils.ConvertTo<PointLight2D>(in value);
			return true;
		}
		if (name == PropertyName._auraTimer)
		{
			_auraTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._networkRevision)
		{
			_networkRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastShootFrame)
		{
			_lastShootFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._light)
		{
			value = VariantUtils.CreateFrom(in _light);
			return true;
		}
		if (name == PropertyName._auraTimer)
		{
			value = VariantUtils.CreateFrom(in _auraTimer);
			return true;
		}
		if (name == PropertyName._networkRevision)
		{
			value = VariantUtils.CreateFrom(in _networkRevision);
			return true;
		}
		if (name == PropertyName._lastShootFrame)
		{
			value = VariantUtils.CreateFrom(in _lastShootFrame);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._light, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._auraTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._networkRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastShootFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._light, Variant.From(in _light));
		info.AddProperty(PropertyName._auraTimer, Variant.From(in _auraTimer));
		info.AddProperty(PropertyName._networkRevision, Variant.From(in _networkRevision));
		info.AddProperty(PropertyName._lastShootFrame, Variant.From(in _lastShootFrame));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._light, out var value))
		{
			_light = value.As<PointLight2D>();
		}
		if (info.TryGetProperty(PropertyName._auraTimer, out var value2))
		{
			_auraTimer = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._networkRevision, out var value3))
		{
			_networkRevision = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastShootFrame, out var value4))
		{
			_lastShootFrame = value4.As<ulong>();
		}
	}
}
