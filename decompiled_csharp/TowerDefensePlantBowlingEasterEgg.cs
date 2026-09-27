using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/BowlingEasterEgg/Scene/TowerDefensePlantBowlingEasterEgg.cs")]
public class TowerDefensePlantBowlingEasterEgg : TowerDefensePlantBowlingBase
{
	public new class MethodName : TowerDefensePlantBowlingBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Bowling = "Bowling";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName ResolveOpenGridPos = "ResolveOpenGridPos";

		public static readonly StringName FindNearbyEmptyCell = "FindNearbyEmptyCell";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlantBowlingBase.PropertyName
	{
		public static readonly StringName packetBank = "packetBank";

		public static readonly StringName _lastBowlingHitGridPos = "_lastBowlingHitGridPos";
	}

	public new class SignalName : TowerDefensePlantBowlingBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string packetBank = "WallnutBowling";

	private Vector2I _lastBowlingHitGridPos = new Vector2I(-1, -1);

	private static PackedScene _IMITATER_CLOUD;

	private static PackedScene IMITATER_CLOUD => _IMITATER_CLOUD ?? (_IMITATER_CLOUD = GD.Load<PackedScene>("uid://djvfnrjg7vtqn"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			base.bowlingComponent = componentManager.GetRuntime<BowlingComponent>();
			BowlingComponent bowlingComponent = base.bowlingComponent;
			if (bowlingComponent != null && !bowlingComponent.IsReleased)
			{
				base.bowlingComponent.OnBowling += Bowling;
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		BowlingComponent bowlingComponent = base.bowlingComponent;
		if (bowlingComponent != null && !bowlingComponent.IsReleased)
		{
			base.bowlingComponent.OnBowling -= Bowling;
		}
	}

	public void Bowling(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			_lastBowlingHitGridPos = character.gridPos;
		}
		moveComponent.velocity = Vector2.Zero;
		bowlingComponent.isRoll = false;
		sprite.timeScale = timeScale * 2.0;
		sprite.SetAnimation("Open", loop: false, 0.1);
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!(clip == "Open"))
		{
			return;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		Vector2I vector2I = ResolveOpenGridPos(logicalGlobalPosition);
		if (!TowerDefenseManager.Instance.CheckMapGridPosIn(vector2I))
		{
			Destroy();
			return;
		}
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(vector2I);
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(IMITATER_CLOUD, vector2I);
		towerDefenseEffectParticlesOnce.GlobalPosition = logicalGlobalPosition;
		TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			Destroy();
			return;
		}
		TowerDefensePacketBankData packetBankData = TowerDefenseManager.GetPacketBankData(packetBank);
		if (GodotObject.IsInstanceValid(packetBankData))
		{
			Array array = new Array();
			array.AddRange(packetBankData.GetCategory("White"));
			array.AddRange(packetBankData.GetCategory("Original"));
			array.AddRange(packetBankData.GetCategory("Gold"));
			string text = (string)array.PickRandom();
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(text);
			while (packetConfig.characterConfig is TowerDefensePlantConfig && array.Count > 1 && (!GodotObject.IsInstanceValid(mapCell) || !mapCell.CanPacketPlant(packetConfig)))
			{
				array.Remove(text);
				text = (string)array.PickRandom();
				packetConfig = TowerDefenseManager.GetPacketConfig(text);
			}
			if (packetConfig.characterConfig is TowerDefensePlantConfig)
			{
				Vector2I vector2I2 = vector2I;
				if (GodotObject.IsInstanceValid(mapCell) && !mapCell.CanPacketPlant(packetConfig))
				{
					vector2I2 = FindNearbyEmptyCell(vector2I, packetConfig);
				}
				if (TowerDefenseManager.Instance.CheckMapGridPosIn(vector2I2))
				{
					TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(vector2I2);
					if (GodotObject.IsInstanceValid(towerDefenseCharacter))
					{
						if (towerDefenseCharacter is TowerDefensePlantBowlingWallnutShooterBoom towerDefensePlantBowlingWallnutShooterBoom)
						{
							towerDefensePlantBowlingWallnutShooterBoom.ArmExplosionImmediately();
						}
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
								MultiPlayerManager.Instance.SendSpawnCharacterAt(text, vector2I2.X, vector2I2.Y, nextSyncId, 1.0, 1.0, instance.hypnoses);
							}
						}
					}
				}
			}
		}
		Destroy();
	}

	private Vector2I ResolveOpenGridPos(Vector2 logicalPosition)
	{
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(towerDefenseManager))
		{
			return gridPos;
		}
		if (towerDefenseManager.CheckMapGridPosIn(gridPos))
		{
			return gridPos;
		}
		Vector2I mapGridPos = towerDefenseManager.GetMapGridPos(logicalPosition);
		if (towerDefenseManager.CheckMapGridPosIn(mapGridPos))
		{
			return mapGridPos;
		}
		if (towerDefenseManager.CheckMapGridPosIn(_lastBowlingHitGridPos))
		{
			return _lastBowlingHitGridPos;
		}
		Vector2 mapGridSize = towerDefenseManager.GetMapGridSize();
		float num = Mathf.Max(1f, mapGridSize.X * 0.001f);
		float num2 = Mathf.Max(1f, mapGridSize.Y * 0.001f);
		Vector2 pos = new Vector2(Mathf.Clamp(logicalPosition.X, (float)towerDefenseManager.GetMapGroundLeft(), (float)towerDefenseManager.GetMapGroundRight() - num), Mathf.Clamp(logicalPosition.Y, (float)towerDefenseManager.GetMapGroundUp(), (float)towerDefenseManager.GetMapGroundDown() - num2));
		Vector2I mapGridPos2 = towerDefenseManager.GetMapGridPos(pos);
		if (towerDefenseManager.CheckMapGridPosIn(mapGridPos2))
		{
			return mapGridPos2;
		}
		return _lastBowlingHitGridPos;
	}

	private Vector2I FindNearbyEmptyCell(Vector2I startPos, TowerDefensePacketConfig packetConfig)
	{
		Vector2I[] array = new Vector2I[8]
		{
			new Vector2I(-1, 0),
			new Vector2I(1, 0),
			new Vector2I(0, -1),
			new Vector2I(0, 1),
			new Vector2I(-1, -1),
			new Vector2I(1, -1),
			new Vector2I(-1, 1),
			new Vector2I(1, 1)
		};
		foreach (Vector2I vector2I in array)
		{
			Vector2I result = startPos + vector2I;
			if (TowerDefenseManager.Instance.CheckMapGridPosIn(result))
			{
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(result);
				if (GodotObject.IsInstanceValid(mapCell) && mapCell.CanPacketPlant(packetConfig))
				{
					return result;
				}
			}
		}
		return startPos;
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "packetBank", packetBank } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		packetBank = (string)data.GetValueOrDefault("packetBank", "WallnutBowling");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Bowling, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveOpenGridPos, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "logicalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindNearbyEmptyCell, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "startPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.Bowling && args.Count == 1)
		{
			Bowling(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveOpenGridPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ResolveOpenGridPos(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.FindNearbyEmptyCell && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(FindNearbyEmptyCell(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.Bowling)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.ResolveOpenGridPos)
		{
			return true;
		}
		if (method == MethodName.FindNearbyEmptyCell)
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
		if (name == PropertyName.packetBank)
		{
			packetBank = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._lastBowlingHitGridPos)
		{
			_lastBowlingHitGridPos = VariantUtils.ConvertTo<Vector2I>(in value);
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
		if (name == PropertyName._lastBowlingHitGridPos)
		{
			value = VariantUtils.CreateFrom(in _lastBowlingHitGridPos);
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
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._lastBowlingHitGridPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetBank, Variant.From(in packetBank));
		info.AddProperty(PropertyName._lastBowlingHitGridPos, Variant.From(in _lastBowlingHitGridPos));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetBank, out var value))
		{
			packetBank = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._lastBowlingHitGridPos, out var value2))
		{
			_lastBowlingHitGridPos = value2.As<Vector2I>();
		}
	}
}
