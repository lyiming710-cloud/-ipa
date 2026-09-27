using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Item/Spikeball/Scene/TowerDefenseItemSpikeball.cs")]
public class TowerDefenseItemSpikeball : TowerDefenseItem, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefenseItem.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ComponentAttack = "ComponentAttack";

		public static readonly StringName Carry = "Carry";

		public static readonly StringName AttachCarryCharacter = "AttachCarryCharacter";

		public static readonly StringName UpdateCarryPresentation = "UpdateCarryPresentation";

		public static readonly StringName ResolvePendingCarrier = "ResolvePendingCarrier";

		public static readonly StringName QueueNetworkCarrier = "QueueNetworkCarrier";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public static readonly StringName DetachCarryCharacter = "DetachCarryCharacter";

		public static readonly StringName CarryCharacterDestroyed = "CarryCharacterDestroyed";

		public static readonly StringName ProcessCarryCarCollision = "ProcessCarryCarCollision";

		public static readonly StringName TryDestroyCarZombie = "TryDestroyCarZombie";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName _pendingCarrierSyncId = "_pendingCarrierSyncId";

		public static readonly StringName attack = "attack";

		public static readonly StringName _fireInterval = "_fireInterval";

		public static readonly StringName carryCharacter = "carryCharacter";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	private AttackComponent attackComponent;

	private int _pendingCarrierSyncId = -1;

	[Export(PropertyHint.None, "")]
	public double attack = 20.0;

	private double _fireInterval = 1.5;

	public TowerDefenseZombie carryCharacter;

	private static bool HasGameplayAuthority
	{
		get
		{
			if (Global.IsMultiplayerMode)
			{
				return MultiPlayerManager.IsHost;
			}
			return true;
		}
	}

	[Export(PropertyHint.None, "")]
	public double fireInterval
	{
		get
		{
			return _fireInterval;
		}
		set
		{
			_fireInterval = value;
			if (IsNodeReady())
			{
				AttackComponent attackComponent = this.attackComponent;
				if (attackComponent != null && !attackComponent.IsReleased)
				{
					this.attackComponent.attackInterval = value;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			attackComponent.OnAttack += ComponentAttack;
			attackComponent.SetCheckAreaRectangleWidth(0, TowerDefenseManager.Instance.GetMapGridSize().X * 1.2f);
			if (GodotObject.IsInstanceValid(carryCharacter))
			{
				attackComponent.SetCheckHitBoxSource(carryCharacter);
			}
			else
			{
				ResolvePendingCarrier();
			}
		}
	}

	public override void _ExitTree()
	{
		DetachCarryCharacter(clearFlag: true);
		base._ExitTree();
		AttackComponent attackComponent = this.attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			this.attackComponent.OnAttack -= ComponentAttack;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || !GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) || !TowerDefenseManager.CurrentControl.isGameRunning || !inGame || die || nearDie)
		{
			return;
		}
		ResolvePendingCarrier();
		if (GodotObject.IsInstanceValid(targetZombie) && Carry(targetZombie as TowerDefenseZombie) == null)
		{
			Destroy();
		}
		else
		{
			if (!GodotObject.IsInstanceValid(carryCharacter))
			{
				return;
			}
			if (carryCharacter.nearDie || carryCharacter.die)
			{
				Destroy();
				return;
			}
			UpdateCarryPresentation();
			if (HasGameplayAuthority)
			{
				ProcessCarryCarCollision();
			}
		}
	}

	public void ComponentAttack()
	{
		AudioManager.Instance.AudioPlay("ProjectileThrow");
		if (HasGameplayAuthority)
		{
			attackComponent.AttackAllFlag(attack, 2);
		}
	}

	public TowerDefenseCharacter Carry(TowerDefenseZombie character)
	{
		targetZombie = null;
		if (character != null)
		{
			if (character.isRise)
			{
				return null;
			}
			if (character.hasSpikeball)
			{
				return null;
			}
			if ((character.instance.maskFlags & 1) == 0)
			{
				return null;
			}
			if (character.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
			{
				return null;
			}
			if (character.camp != camp)
			{
				return null;
			}
			if (!character.targetRegistrationComponent.canCarry)
			{
				return null;
			}
		}
		AttachCarryCharacter(character);
		return carryCharacter;
	}

	private void AttachCarryCharacter(TowerDefenseZombie character)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			if (GodotObject.IsInstanceValid(carryCharacter) && carryCharacter != character)
			{
				DetachCarryCharacter(clearFlag: true);
			}
			carryCharacter = character;
			carryCharacter.OnDestroy -= CarryCharacterDestroyed;
			carryCharacter.OnDestroy += CarryCharacterDestroyed;
			carryCharacter.hasSpikeball = true;
			UpdateCarryPresentation();
			attackComponent?.SetCheckHitBoxSource(carryCharacter);
		}
	}

	private void UpdateCarryPresentation()
	{
		if (GodotObject.IsInstanceValid(carryCharacter))
		{
			ulong num = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			if (num == 18446744073709551615uL)
			{
				num = Engine.GetPhysicsFrames();
			}
			groundHeight = carryCharacter.groundHeight;
			z = 10.0;
			SetGlobalPositionForPhysicsFrame(carryCharacter.GetGlobalPositionForPhysicsFrame(num), num);
			gridPos = carryCharacter.gridPos;
		}
	}

	private void ResolvePendingCarrier()
	{
		if (_pendingCarrierSyncId >= 0)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl) && currentControl._syncCharacters.TryGetValue(_pendingCarrierSyncId, out var value) && value is TowerDefenseZombie character && GodotObject.IsInstanceValid(character))
			{
				_pendingCarrierSyncId = -1;
				targetZombie = null;
				AttachCarryCharacter(character);
			}
		}
	}

	private void QueueNetworkCarrier(Dictionary data)
	{
		int num = data.GetValueOrDefault("carrierSyncId", -1).AsInt32();
		if (num < 0)
		{
			_pendingCarrierSyncId = -1;
			DetachCarryCharacter(clearFlag: true);
		}
		else if (GodotObject.IsInstanceValid(carryCharacter) && carryCharacter.syncId == num)
		{
			_pendingCarrierSyncId = -1;
		}
		else
		{
			_pendingCarrierSyncId = num;
			ResolvePendingCarrier();
		}
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		if (data != null)
		{
			QueueNetworkCarrier(data);
		}
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		int num = (GodotObject.IsInstanceValid(carryCharacter) ? carryCharacter.syncId : _pendingCarrierSyncId);
		return new Dictionary { ["carrierSyncId"] = num };
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		if (data != null)
		{
			QueueNetworkCarrier(data);
		}
	}

	private void DetachCarryCharacter(bool clearFlag)
	{
		if (!GodotObject.IsInstanceValid(carryCharacter))
		{
			carryCharacter = null;
			return;
		}
		carryCharacter.OnDestroy -= CarryCharacterDestroyed;
		if (clearFlag && !carryCharacter.die && !carryCharacter.nearDie)
		{
			carryCharacter.hasSpikeball = false;
		}
		carryCharacter = null;
	}

	private void CarryCharacterDestroyed(TowerDefenseCharacter _character)
	{
		Destroy();
	}

	private void ProcessCarryCarCollision()
	{
		if (GodotObject.IsInstanceValid(carryCharacter) && carryCharacter.IsHitBoxEnabled)
		{
			List<TowerDefenseCharacter> charactersIntersectingRectList = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(carryCharacter.WorldHitRect, carryCharacter.gridPos.Y, includeAllLineCheck: true);
			for (int i = 0; i < charactersIntersectingRectList.Count && !TryDestroyCarZombie(charactersIntersectingRectList[i]); i++)
			{
			}
		}
	}

	private bool TryDestroyCarZombie(TowerDefenseCharacter character)
	{
		if (!(character is TowerDefenseZombie towerDefenseZombie))
		{
			return false;
		}
		if (towerDefenseZombie.nearDie || towerDefenseZombie.die)
		{
			return false;
		}
		if (towerDefenseZombie.instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.CAR)
		{
			return false;
		}
		towerDefenseZombie.Die();
		Destroy();
		return true;
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary
		{
			{ "attack", attack },
			{ "fireInterval", fireInterval }
		};
		if (GodotObject.IsInstanceValid(carryCharacter))
		{
			dictionary["carryCharacterNodeName"] = carryCharacter.Name;
		}
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		attack = data.GetValueOrDefault("attack", 20.0).AsDouble();
		fireInterval = data.GetValueOrDefault("fireInterval", fireInterval).AsDouble();
		string text = data.GetValueOrDefault("carryCharacterNodeName", "").AsString();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (GodotObject.IsInstanceValid(node2D))
		{
			TowerDefenseZombie nodeOrNull = node2D.GetNodeOrNull<TowerDefenseZombie>(new NodePath(text));
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				_pendingCarrierSyncId = -1;
				AttachCarryCharacter(nodeOrNull);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ComponentAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Carry, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AttachCarryCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCarryPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolvePendingCarrier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueNetworkCarrier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DetachCarryCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "clearFlag", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CarryCharacterDestroyed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessCarryCarCollision, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryDestroyCarZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ComponentAttack && args.Count == 0)
		{
			ComponentAttack();
			ret = default;
			return true;
		}
		if (method == MethodName.Carry && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(Carry(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.AttachCarryCharacter && args.Count == 1)
		{
			AttachCarryCharacter(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCarryPresentation && args.Count == 0)
		{
			UpdateCarryPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePendingCarrier && args.Count == 0)
		{
			ResolvePendingCarrier();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueNetworkCarrier && args.Count == 1)
		{
			QueueNetworkCarrier(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName.DetachCarryCharacter && args.Count == 1)
		{
			DetachCarryCharacter(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CarryCharacterDestroyed && args.Count == 1)
		{
			CarryCharacterDestroyed(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessCarryCarCollision && args.Count == 0)
		{
			ProcessCarryCarCollision();
			ret = default;
			return true;
		}
		if (method == MethodName.TryDestroyCarZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryDestroyCarZombie(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.ComponentAttack)
		{
			return true;
		}
		if (method == MethodName.Carry)
		{
			return true;
		}
		if (method == MethodName.AttachCarryCharacter)
		{
			return true;
		}
		if (method == MethodName.UpdateCarryPresentation)
		{
			return true;
		}
		if (method == MethodName.ResolvePendingCarrier)
		{
			return true;
		}
		if (method == MethodName.QueueNetworkCarrier)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState)
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
		if (method == MethodName.DetachCarryCharacter)
		{
			return true;
		}
		if (method == MethodName.CarryCharacterDestroyed)
		{
			return true;
		}
		if (method == MethodName.ProcessCarryCarCollision)
		{
			return true;
		}
		if (method == MethodName.TryDestroyCarZombie)
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
		if (name == PropertyName.fireInterval)
		{
			fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pendingCarrierSyncId)
		{
			_pendingCarrierSyncId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.attack)
		{
			attack = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			_fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.carryCharacter)
		{
			carryCharacter = VariantUtils.ConvertTo<TowerDefenseZombie>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.fireInterval)
		{
			value = VariantUtils.CreateFrom<double>(fireInterval);
			return true;
		}
		if (name == PropertyName._pendingCarrierSyncId)
		{
			value = VariantUtils.CreateFrom(in _pendingCarrierSyncId);
			return true;
		}
		if (name == PropertyName.attack)
		{
			value = VariantUtils.CreateFrom(in attack);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			value = VariantUtils.CreateFrom(in _fireInterval);
			return true;
		}
		if (name == PropertyName.carryCharacter)
		{
			value = VariantUtils.CreateFrom(in carryCharacter);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._pendingCarrierSyncId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.attack, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.carryCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName._pendingCarrierSyncId, Variant.From(in _pendingCarrierSyncId));
		info.AddProperty(PropertyName.attack, Variant.From(in attack));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
		info.AddProperty(PropertyName.carryCharacter, Variant.From(in carryCharacter));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fireInterval, out var value))
		{
			fireInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pendingCarrierSyncId, out var value2))
		{
			_pendingCarrierSyncId = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.attack, out var value3))
		{
			attack = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value4))
		{
			_fireInterval = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.carryCharacter, out var value5))
		{
			carryCharacter = value5.As<TowerDefenseZombie>();
		}
	}
}
