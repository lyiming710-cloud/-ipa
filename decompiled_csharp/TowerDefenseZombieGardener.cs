using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Puzzle/Gardener/Scene/TowerDefenseZombieGardener.cs")]
public class TowerDefenseZombieGardener : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName Barrow = "Barrow";

		public static readonly StringName TurnTowardBarrowExit = "TurnTowardBarrowExit";

		public static readonly StringName Plant = "Plant";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _potNode = "_potNode";

		public static readonly StringName _plantNode = "_plantNode";

		public static readonly StringName _pendingBarrowPlantNodeName = "_pendingBarrowPlantNodeName";

		public static readonly StringName canBarrow = "canBarrow";

		public static readonly StringName hasBarrow = "hasBarrow";

		public static readonly StringName barrowPlant = "barrowPlant";

		public static readonly StringName barrowPlantTranform = "barrowPlantTranform";

		public static readonly StringName over = "over";

		public static readonly StringName isPlant = "isPlant";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private AttackComponent _attackComponent2;

	private Node2D _potNode;

	private Node2D _plantNode;

	private string _pendingBarrowPlantNodeName = "";

	public bool canBarrow = true;

	public bool hasBarrow = true;

	public TowerDefenseCharacter barrowPlant;

	public Transform2D barrowPlantTranform;

	public bool over;

	public bool isPlant;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
			_potNode = GetNode<Node2D>("%PotNode");
			_plantNode = GetNode<Node2D>("%PlantNode");
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (GodotObject.IsInstanceValid(barrowPlant))
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			Vector2 logicalGlobalPosition2 = GetLogicalGlobalPosition(_potNode);
			Vector2 logicalGlobalPosition3 = barrowPlant.GetLogicalGlobalPosition();
			barrowPlant.gridPos = new Vector2I(gridPos.X, barrowPlant.gridPos.Y);
			barrowPlant.SetLogicalGlobalPosition(new Vector2(logicalGlobalPosition2.X, logicalGlobalPosition3.Y));
			barrowPlant.groundHeight = (logicalGlobalPosition2.Y - logicalGlobalPosition.Y) / transformPoint.GlobalScale.Y;
		}
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		if (!hasBarrow)
		{
			base.Hypnoses(time, canFliter, hypnosesConfig);
		}
	}

	public override void WalkProcessing(double delta)
	{
		if (hasBarrow && _attackComponent2.CanAttack() && (_attackComponent2.target.instance.physiqueTypeFlags & 0x10) != 0)
		{
			if (GodotObject.IsInstanceValid(_attackComponent2.target.cell) && _attackComponent2.target.cell.HasSpike())
			{
				_attackComponent2.target = _attackComponent2.target.cell.GetSpike();
			}
			if (_attackComponent2.target.instance.spikeHurt != -1.0)
			{
				TowerDefenseCharacter target = _attackComponent2.target;
				double spikeHurt = _attackComponent2.target.instance.spikeHurt;
				target.Hurt(100000.0, playSplatAudio: true, default, createDamagePart: true, spikeHurt);
				instance.ArmorDelete("Barrow");
				return;
			}
		}
		if (hasBarrow && canBarrow && !GodotObject.IsInstanceValid(barrowPlant) && _attackComponent2.CanAttack() && (_attackComponent2.target.instance.physiqueTypeFlags & 0x10) == 0)
		{
			foreach (TowerDefenseCharacter target2 in _attackComponent2.GetTargetList())
			{
				if (!(target2 is TowerDefensePlant { inGame: not false, componentAlive: not false } towerDefensePlant))
				{
					continue;
				}
				if (towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.GROUND) || towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.POT) || towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.LILYPAD) || towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.SOIL) || towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.BRICK) || towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.SURROUND))
				{
					if (GodotObject.IsInstanceValid(towerDefensePlant.cell) && towerDefensePlant.cell.characterSlotDictionary.TryGetValue(towerDefensePlant, out var value) && GodotObject.IsInstanceValid(value))
					{
						continue;
					}
					Barrow(towerDefensePlant);
				}
				TurnTowardBarrowExit();
				break;
			}
		}
		base.WalkProcessing(delta);
		if (!GodotObject.IsInstanceValid(barrowPlant))
		{
			sprite.timeScale = timeScale * walkSpeedScale * 2.0;
		}
	}

	public override void AttackProcessing(double delta)
	{
		if (hasBarrow && _attackComponent2.CanAttack() && (_attackComponent2.target.instance.physiqueTypeFlags & 0x10) != 0 && _attackComponent2.target.instance.spikeHurt != -1.0)
		{
			TowerDefenseCharacter target = _attackComponent2.target;
			double spikeHurt = _attackComponent2.target.instance.spikeHurt;
			target.Hurt(100000.0, playSplatAudio: true, default, createDamagePart: true, spikeHurt);
			instance.ArmorDelete("Barrow");
			return;
		}
		if (hasBarrow && canBarrow && !GodotObject.IsInstanceValid(barrowPlant) && _attackComponent2.CanAttack() && (_attackComponent2.target.instance.physiqueTypeFlags & 0x10) == 0)
		{
			foreach (TowerDefenseCharacter target2 in _attackComponent2.GetTargetList())
			{
				if (!(target2 is TowerDefensePlant { inGame: not false, componentAlive: not false } towerDefensePlant))
				{
					continue;
				}
				if (towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.GROUND) || towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.POT) || towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.LILYPAD) || towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.SOIL) || towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.BRICK) || towerDefensePlant.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.SURROUND))
				{
					if (GodotObject.IsInstanceValid(towerDefensePlant.cell) && towerDefensePlant.cell.characterSlotDictionary.TryGetValue(towerDefensePlant, out var value) && GodotObject.IsInstanceValid(value))
					{
						continue;
					}
					Barrow(towerDefensePlant);
				}
				TurnTowardBarrowExit();
				break;
			}
		}
		if (hasBarrow)
		{
			if (!attackComponent.CanAttack())
			{
				Walk();
			}
			sprite.timeScale = timeScale * 2.0;
		}
		else
		{
			base.AttackProcessing(delta);
		}
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Barrow")
		{
			useAttackDps = true;
			hasBarrow = false;
			canBarrow = false;
			walkAnimeClip = "Walk";
			swimAnimeClip = "Walk";
			attackAnimeClip = "Eat";
			attackComponent.checkVase = false;
			attackComponent.attackType = "Eat";
			attackComponent.target = null;
			_attackComponent2.target = null;
			_attackComponent2.SetAlive(alive: false);
			Walk();
			instance.collisionFlags = 1;
			if (!isPlant)
			{
				isPlant = true;
				Plant();
			}
		}
	}

	public override void DestroySet()
	{
		if (!over)
		{
			over = true;
			Plant();
		}
	}

	public void Barrow(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character) || !character.inGame || !character.componentAlive)
		{
			return;
		}
		canBarrow = false;
		barrowPlant = character;
		TowerDefenseCellInstance towerDefenseCellInstance = barrowPlant.cell;
		if (GodotObject.IsInstanceValid(towerDefenseCellInstance))
		{
			TowerDefenseCharacter target = towerDefenseCellInstance.GetTarget(instance.maskFlags, camp);
			if (GodotObject.IsInstanceValid(target))
			{
				barrowPlant = target;
			}
			if (GodotObject.IsInstanceValid(barrowPlant))
			{
				towerDefenseCellInstance.RemoveCharacter(barrowPlant);
			}
		}
		barrowPlantTranform = barrowPlant.Transform;
		barrowPlant.SetHitBoxSuppressed(HitBoxSuppressionReason.Carried, suppressed: true);
		barrowPlant.gridPos = new Vector2I(-1, barrowPlant.gridPos.Y);
		barrowPlant.inGame = false;
		barrowPlant.componentAlive = false;
		attackComponent.target = null;
		_attackComponent2.target = null;
		barrowPlant.Reparent(_plantNode);
		barrowPlant.shadowSprite.Visible = false;
		barrowPlant.Scale = new Vector2(barrowPlant.Scale.X, (float)Mathf.Sign(barrowPlant.Scale.Y) * barrowPlant.Scale.Y);
		barrowPlant.Rotation = 0f;
		barrowPlant.SetLogicalGlobalPosition(GetLogicalGlobalPosition(_plantNode));
	}

	internal void TurnTowardBarrowExit()
	{
		Scale = new Vector2(0f - Scale.X, Scale.Y);
		GroundMoveComponent groundMoveComponent = base.groundMoveComponent;
		if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
		{
			base.groundMoveComponent.RefreshDirectionCache();
		}
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.NotifyAncestorTransformChangedForRender(changedInPhysicsFrame: true);
		}
	}

	public async void Plant()
	{
		if (!GodotObject.IsInstanceValid(barrowPlant))
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(cell))
		{
			barrowPlant.componentAlive = true;
			barrowPlant.die = true;
			barrowPlant.Destroy();
			return;
		}
		if (!cell.CanPacketPlant(barrowPlant.packet))
		{
			barrowPlant.componentAlive = true;
			barrowPlant.die = true;
			barrowPlant.Destroy();
			return;
		}
		cell.CharacterPlant(barrowPlant.packet, barrowPlant);
		if (barrowPlant.HasHitBox)
		{
			barrowPlant.SetHitBoxSuppressed(HitBoxSuppressionReason.Carried, suppressed: false);
		}
		barrowPlant.gridPos = gridPos;
		barrowPlant.Reparent(TowerDefenseGroundItemBase.characterNode);
		barrowPlant.Rotation = barrowPlantTranform.Rotation;
		barrowPlant.Scale = barrowPlantTranform.Scale;
		barrowPlant.shadowSprite.Visible = !barrowPlant.invisible;
		if (barrowPlant.Scale.Y < 0f)
		{
			barrowPlant.Scale = new Vector2(barrowPlant.Scale.X, 0f - barrowPlant.Scale.Y);
			barrowPlant.RotationDegrees -= 180f;
		}
		Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(gridPos);
		barrowPlant.SetLogicalGlobalPosition(mapCellPlantPos);
		barrowPlant.shadowComponent.saveShadowPosition = new Vector2(mapCellPlantPos.X, barrowPlant.shadowComponent.saveShadowPosition.Y);
		barrowPlant.shadowSprite.GlobalPosition = new Vector2(mapCellPlantPos.X, barrowPlant.shadowSprite.GlobalPosition.Y);
		if (GodotObject.IsInstanceValid(barrowPlant))
		{
			barrowPlant.inGame = true;
			barrowPlant.componentAlive = true;
			barrowPlant.groundHeight = 0.0;
			barrowPlant.shadowSprite.Visible = !barrowPlant.invisible;
			barrowPlant = null;
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "attack")
		{
			_attackComponent2.AttackExecute(((TowerDefenseZombieConfig)config).smashAttack);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary
		{
			{ "canBarrow", canBarrow },
			{ "hasBarrow", hasBarrow },
			{ "over", over },
			{ "isPlant", isPlant }
		};
		if (GodotObject.IsInstanceValid(barrowPlant))
		{
			dictionary["barrowPlantNodeName"] = barrowPlant.Name.ToString();
			dictionary["barrowPlantTransform"] = barrowPlantTranform;
		}
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		canBarrow = data.GetValueOrDefault("canBarrow", true).AsBool();
		hasBarrow = data.GetValueOrDefault("hasBarrow", true).AsBool();
		over = data.GetValueOrDefault("over", false).AsBool();
		isPlant = data.GetValueOrDefault("isPlant", false).AsBool();
		_pendingBarrowPlantNodeName = data.GetValueOrDefault("barrowPlantNodeName", "").AsString();
		if (data.ContainsKey("barrowPlantTransform"))
		{
			barrowPlantTranform = data["barrowPlantTransform"].AsTransform2D();
		}
	}

	public override void FinalizeProgressRestore()
	{
		base.FinalizeProgressRestore();
		if (!string.IsNullOrEmpty(_pendingBarrowPlantNodeName) && GodotObject.IsInstanceValid(_plantNode))
		{
			TowerDefenseCharacter nodeOrNull = _plantNode.GetNodeOrNull<TowerDefenseCharacter>(new NodePath(_pendingBarrowPlantNodeName));
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				barrowPlant = nodeOrNull;
			}
			_pendingBarrowPlantNodeName = "";
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Barrow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TurnTowardBarrowExit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Plant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.Barrow && args.Count == 1)
		{
			Barrow(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TurnTowardBarrowExit && args.Count == 0)
		{
			TurnTowardBarrowExit();
			ret = default;
			return true;
		}
		if (method == MethodName.Plant && args.Count == 0)
		{
			Plant();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
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
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
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
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.Barrow)
		{
			return true;
		}
		if (method == MethodName.TurnTowardBarrowExit)
		{
			return true;
		}
		if (method == MethodName.Plant)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
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
		if (method == MethodName.FinalizeProgressRestore)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._potNode)
		{
			_potNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._plantNode)
		{
			_plantNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._pendingBarrowPlantNodeName)
		{
			_pendingBarrowPlantNodeName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.canBarrow)
		{
			canBarrow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hasBarrow)
		{
			hasBarrow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.barrowPlant)
		{
			barrowPlant = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.barrowPlantTranform)
		{
			barrowPlantTranform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isPlant)
		{
			isPlant = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._potNode)
		{
			value = VariantUtils.CreateFrom(in _potNode);
			return true;
		}
		if (name == PropertyName._plantNode)
		{
			value = VariantUtils.CreateFrom(in _plantNode);
			return true;
		}
		if (name == PropertyName._pendingBarrowPlantNodeName)
		{
			value = VariantUtils.CreateFrom(in _pendingBarrowPlantNodeName);
			return true;
		}
		if (name == PropertyName.canBarrow)
		{
			value = VariantUtils.CreateFrom(in canBarrow);
			return true;
		}
		if (name == PropertyName.hasBarrow)
		{
			value = VariantUtils.CreateFrom(in hasBarrow);
			return true;
		}
		if (name == PropertyName.barrowPlant)
		{
			value = VariantUtils.CreateFrom(in barrowPlant);
			return true;
		}
		if (name == PropertyName.barrowPlantTranform)
		{
			value = VariantUtils.CreateFrom(in barrowPlantTranform);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.isPlant)
		{
			value = VariantUtils.CreateFrom(in isPlant);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._potNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._plantNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingBarrowPlantNodeName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canBarrow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasBarrow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.barrowPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.barrowPlantTranform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._potNode, Variant.From(in _potNode));
		info.AddProperty(PropertyName._plantNode, Variant.From(in _plantNode));
		info.AddProperty(PropertyName._pendingBarrowPlantNodeName, Variant.From(in _pendingBarrowPlantNodeName));
		info.AddProperty(PropertyName.canBarrow, Variant.From(in canBarrow));
		info.AddProperty(PropertyName.hasBarrow, Variant.From(in hasBarrow));
		info.AddProperty(PropertyName.barrowPlant, Variant.From(in barrowPlant));
		info.AddProperty(PropertyName.barrowPlantTranform, Variant.From(in barrowPlantTranform));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.isPlant, Variant.From(in isPlant));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._potNode, out var value))
		{
			_potNode = value.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._plantNode, out var value2))
		{
			_plantNode = value2.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._pendingBarrowPlantNodeName, out var value3))
		{
			_pendingBarrowPlantNodeName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.canBarrow, out var value4))
		{
			canBarrow = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hasBarrow, out var value5))
		{
			hasBarrow = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.barrowPlant, out var value6))
		{
			barrowPlant = value6.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.barrowPlantTranform, out var value7))
		{
			barrowPlantTranform = value7.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value8))
		{
			over = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isPlant, out var value9))
		{
			isPlant = value9.As<bool>();
		}
	}
}
