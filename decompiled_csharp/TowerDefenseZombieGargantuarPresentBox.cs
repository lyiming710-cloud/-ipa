using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/PresentBox/TowerDefenseZombieGargantuarPresentBox.cs")]
public class TowerDefenseZombieGargantuarPresentBox : TowerDefenseZombieGargantuarBase
{
	public new class MethodName : TowerDefenseZombieGargantuarBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName HitpointsNearDie = "HitpointsNearDie";

		public new static readonly StringName HitpointsEmpty = "HitpointsEmpty";

		public static readonly StringName CreateRandom = "CreateRandom";

		public new static readonly StringName Purify = "Purify";
	}

	public new class PropertyName : TowerDefenseZombieGargantuarBase.PropertyName
	{
		public static readonly StringName packetBank = "packetBank";

		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefenseZombieGargantuarBase.SignalName
	{
	}

	private const string ZOMBIE_GARGANTUAR_HEAD2 = "uid://cfx6iqy08xujv";

	private const string ZOMBIE_GARGANTUAR_DUCKXING = "uid://6dy81rx4gaue";

	private const string ZOMBIE_GARGANTUAR_ZOMBIE = "uid://dtrl03qm2d0u7";

	private static PackedScene _IMITATER_CLOUD;

	[Export(PropertyHint.None, "")]
	public string packetBank;

	public bool over;

	private static PackedScene IMITATER_CLOUD => _IMITATER_CLOUD ?? (_IMITATER_CLOUD = GD.Load<PackedScene>("uid://djvfnrjg7vtqn"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			double num = GD.Randf();
			if (num < 0.3)
			{
				sprite.SetAtlasReplace("Zombie_gargantuar_telephonepole.png", "uid://6dy81rx4gaue");
			}
			else if (num < 0.6)
			{
				sprite.SetAtlasReplace("Zombie_gargantuar_telephonepole.png", "uid://dtrl03qm2d0u7");
			}
			ConfigureWaterLineVisualLayers("Zombie_duckytube");
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		if (damangePointName == "Head")
		{
			sprite.SetAtlasReplace("Zombie_gargantuar_head.png", "uid://cfx6iqy08xujv");
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == impFireEvent)
		{
			((ZombieGargantuarPresentBoxSprite)sprite).presentBoxImpHead.Visible = false;
		}
	}

	public override void InWater()
	{
		base.InWater();
		sprite.SetFliter("Zombie_whitewater", open: true);
	}

	public override void OutWater()
	{
		base.OutWater();
		sprite.SetFliter("Zombie_whitewater", open: false);
	}

	public override void HitpointsNearDie()
	{
		base.HitpointsNearDie();
		CreateRandom();
	}

	public override void HitpointsEmpty()
	{
		base.HitpointsEmpty();
		CreateRandom();
	}

	public async void CreateRandom()
	{
		if (over)
		{
			return;
		}
		over = true;
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			CallDeferred("Destroy", true);
			return;
		}
		bool inheritedHypnoses = instance.hypnoses;
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(IMITATER_CLOUD, gridPos);
		towerDefenseEffectParticlesOnce.GlobalPosition = logicalGlobalPosition;
		TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		TowerDefensePacketBankData packetBankData = TowerDefenseManager.GetPacketBankData(packetBank);
		if (GodotObject.IsInstanceValid(packetBankData))
		{
			List<string> list = new List<string>(from v in packetBankData.GetZombieList()
				select v.AsString());
			if (!TowerDefenseManager.Instance.CheckMapGridPosIn(gridPos))
			{
				list.RemoveAll((string name) => name == "ZombieBungiBin");
			}
			if (list.Count == 0)
			{
				CallDeferred("Destroy", true);
				return;
			}
			string text = list[GD.RandRange(0, list.Count - 1)];
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(text);
			while (inheritedHypnoses && (packetConfig.characterConfig.unUseBuffFlags & 8) != 0 && list.Count > 1)
			{
				list.Remove(text);
				text = list[GD.RandRange(0, list.Count - 1)];
				packetConfig = TowerDefenseManager.GetPacketConfig(text);
			}
			if (GodotObject.IsInstanceValid(packetConfig))
			{
				TowerDefenseCharacter zombie = packetConfig.Plant(gridPos);
				if (GodotObject.IsInstanceValid(zombie))
				{
					double num = instance.hitpointScale * 2.0;
					Vector2 vector = transformPoint.Scale * 1.5f;
					zombie.SetLogicalGlobalPosition(new Vector2(logicalGlobalPosition.X, TowerDefenseManager.GetMapCellPlantPos(gridPos).Y));
					zombie.CallDeferred("SetHitpointAndScale", num, vector);
					if (inheritedHypnoses)
					{
						zombie.Hypnoses(-1.0, canFliter: false);
					}
					if (GodotObject.IsInstanceValid(TowerDefenseBattleFeatureWave.Instance))
					{
						Callable.From(() =>
						{
							if (GodotObject.IsInstanceValid(zombie) && GodotObject.IsInstanceValid(TowerDefenseBattleFeatureWave.Instance))
							{
								TowerDefenseBattleFeatureWave.Instance.currentHpPointTotal += zombie.GetTotalHitPoint() / 3.0;
								TowerDefenseBattleFeatureWave.Instance.currentHpPoint += zombie.GetTotalHitPoint() / 3.0;
							}
						}).CallDeferred();
					}
					if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
					{
						TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
						if (GodotObject.IsInstanceValid(currentControl))
						{
							int nextSyncId = currentControl.GetNextSyncId();
							currentControl.RegisterSyncCharacter(nextSyncId, zombie);
							MultiPlayerManager.Instance.SendSpawnCharacterAt(text, gridPos.X, gridPos.Y, nextSyncId, num, vector.X, inheritedHypnoses);
						}
					}
				}
			}
		}
		CallDeferred("Destroy", true);
	}

	public override void Purify()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			Destroy();
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantPresentBox");
		if (cell.CanPacketPlant(packetConfig))
		{
			TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(gridPos);
			towerDefenseCharacter.CallDeferred("WakeUp");
			if (instance.hypnoses)
			{
				towerDefenseCharacter.Hypnoses();
			}
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
				if (GodotObject.IsInstanceValid(currentControl))
				{
					int nextSyncId = currentControl.GetNextSyncId();
					currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
					MultiPlayerManager.Instance.SendSpawnCharacterAt("PlantPresentBox", gridPos.X, gridPos.Y, nextSyncId);
				}
			}
		}
		Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitpointsNearDie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateRandom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Purify, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InWater && args.Count == 0)
		{
			InWater();
			ret = default;
			return true;
		}
		if (method == MethodName.OutWater && args.Count == 0)
		{
			OutWater();
			ret = default;
			return true;
		}
		if (method == MethodName.HitpointsNearDie && args.Count == 0)
		{
			HitpointsNearDie();
			ret = default;
			return true;
		}
		if (method == MethodName.HitpointsEmpty && args.Count == 0)
		{
			HitpointsEmpty();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateRandom && args.Count == 0)
		{
			CreateRandom();
			ret = default;
			return true;
		}
		if (method == MethodName.Purify && args.Count == 0)
		{
			Purify();
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
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.InWater)
		{
			return true;
		}
		if (method == MethodName.OutWater)
		{
			return true;
		}
		if (method == MethodName.HitpointsNearDie)
		{
			return true;
		}
		if (method == MethodName.HitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.CreateRandom)
		{
			return true;
		}
		if (method == MethodName.Purify)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.packetBank)
		{
			packetBank = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
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
		if (name == PropertyName.packetBank)
		{
			value = VariantUtils.CreateFrom(in packetBank);
			return true;
		}
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
			new PropertyInfo(Variant.Type.String, PropertyName.packetBank, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetBank, Variant.From(in packetBank));
		info.AddProperty(PropertyName.over, Variant.From(in over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetBank, out var value))
		{
			packetBank = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value2))
		{
			over = value2.As<bool>();
		}
	}
}
