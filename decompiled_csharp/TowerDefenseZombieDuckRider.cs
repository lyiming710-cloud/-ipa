using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter3/DuckRider/Scene/TowerDefenseZombieDuckRider.cs")]
public class TowerDefenseZombieDuckRider : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName ReleaseCarriedCharacter = "ReleaseCarriedCharacter";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName ProcessCarryCandidate = "ProcessCarryCandidate";

		public static readonly StringName TryCarryZombie = "TryCarryZombie";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName useCone = "useCone";

		public static readonly StringName useBucket = "useBucket";

		public static readonly StringName over = "over";

		public static readonly StringName mowerKill = "mowerKill";

		public static readonly StringName carryCharacter = "carryCharacter";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	public bool useCone;

	public bool useBucket;

	public bool over;

	public bool mowerKill;

	public TowerDefenseCharacter carryCharacter;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !IsProgressRestoreInFlight)
		{
			float num = GD.Randf();
			if ((double)num < 0.1)
			{
				useBucket = true;
				sprite.SetFliters(new Array { "anim_bucket" }, open: true);
			}
			else if ((double)num < 0.3)
			{
				useCone = true;
				sprite.SetFliters(new Array { "anim_cone" }, open: true);
			}
		}
	}

	public override void _ExitTree()
	{
		if (!Engine.IsEditorHint())
		{
			ReleaseCarriedCharacter();
		}
		base._ExitTree();
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) && TowerDefenseManager.CurrentControl.isGameRunning && inGame && !over && !die && !nearDie)
		{
			if (GodotObject.IsInstanceValid(carryCharacter))
			{
				ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
				Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
				Vector2 globalPositionForPhysicsFrame2 = carryCharacter.GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
				globalPositionForPhysicsFrame2.X = globalPositionForPhysicsFrame.X + (float)(instance.hypnoses ? (-30) : 30);
				carryCharacter.SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame2, currentPhysicsFrame);
				carryCharacter.groundHeight = z + 40.0;
				carryCharacter.z = z + 40.0;
			}
			else
			{
				ProcessCarryCandidate();
			}
		}
	}

	public override void WalkProcessing(double delta)
	{
		sprite.timeScale = timeScale * walkSpeedScale;
		if (!attackComponent.CanAttack())
		{
			return;
		}
		if (GodotObject.IsInstanceValid(attackComponent.target.cell) && attackComponent.target.cell.HasSpike())
		{
			attackComponent.target = attackComponent.target.cell.GetSpike();
		}
		if ((attackComponent.target.instance.physiqueTypeFlags & 0x10) == 0)
		{
			attackComponent.SmashAttackCell(((TowerDefenseZombieConfig)config).smashAttack);
			return;
		}
		if (attackComponent.target.instance.spikeHurt != -1.0)
		{
			TowerDefenseCharacter target = attackComponent.target;
			double spikeHurt = attackComponent.target.instance.spikeHurt;
			target.Hurt(100000.0, playSplatAudio: true, default, createDamagePart: true, spikeHurt);
		}
		Die();
	}

	public override void DieEntered()
	{
		base.DieEntered();
	}

	public override async void DestroySet()
	{
		if (over)
		{
			return;
		}
		over = true;
		HitBoxDestroy();
		sprite.SetFliters(new Array
		{
			"anim_bucket", "anim_cone", "anim_hair", "Zombie_outerarm_lower", "Zombie_outerarm_upper", "Zombie_outerarm_hand", "anim_head2", "Zombie_tie", "Zombie_body", "Zombie_outerleg_lower",
			"Zombie_outerleg_foot", "Zombie_outerleg_upper", "Zombie_innerleg_foot", "Zombie_innerleg_lower", "Zombie_innerleg_upper", "anim_head1", "Zombie_neck", "anim_innerarm1", "anim_innerarm2", "anim_innerarm3"
		}, open: false);
		ReleaseCarriedCharacter();
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		if (mowerKill)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			return;
		}
		string packetName = "ZombieNormal";
		if (useCone)
		{
			packetName = "ZombieNormalCone";
		}
		if (useBucket)
		{
			packetName = "ZombieNormalBucket";
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseCharacter zombie = packetConfig.Create(logicalGlobalPosition, gridPos, 40.0);
		TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", zombie);
		double hitpointScale = instance.hitpointScale;
		Vector2 scale = transformPoint.Scale;
		zombie.CallDeferred("SetHitpointAndScale", hitpointScale, scale);
		if (instance.hypnoses)
		{
			Callable.From(() =>
			{
				zombie.Hypnoses();
			}).CallDeferred();
		}
		zombie.CallDeferred("WalkReady");
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, zombie);
				MultiPlayerManager.Instance.SendSpawnCharacterAt(packetName, gridPos.X, gridPos.Y, nextSyncId, hitpointScale, scale.X, instance.hypnoses, 0.0, useCreate: true, logicalGlobalPosition.X, logicalGlobalPosition.Y, walkAfterSpawn: true, 40.0);
			}
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	private void ReleaseCarriedCharacter()
	{
		TowerDefenseZombieCarryRelation.Release(this, ref carryCharacter, groundHeight);
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary();
		TowerDefenseZombieCarryRelation.Export(dictionary, carryCharacter);
		dictionary["useCone"] = useCone;
		dictionary["useBucket"] = useBucket;
		return dictionary;
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		useCone = data.GetValueOrDefault("useCone", false).AsBool();
		useBucket = data.GetValueOrDefault("useBucket", false).AsBool();
		sprite.SetFliters(new Array { "anim_bucket" }, useBucket);
		sprite.SetFliters(new Array { "anim_cone" }, useCone);
		TowerDefenseZombie towerDefenseZombie = TowerDefenseZombieCarryRelation.Resolve(data);
		if (!GodotObject.IsInstanceValid(towerDefenseZombie) || towerDefenseZombie == this || !TowerDefenseZombieCarryRelation.Attach(this, ref carryCharacter, towerDefenseZombie, z + 40.0))
		{
			TowerDefenseZombieCarryRelation.Release(this, ref carryCharacter, groundHeight);
		}
	}

	private void ProcessCarryCandidate()
	{
		if (TryGetActiveWorldHitRect(out var rect))
		{
			List<TowerDefenseCharacter> charactersIntersectingRectListForCamp = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectListForCamp(rect, camp);
			for (int i = 0; i < charactersIntersectingRectListForCamp.Count && (!(charactersIntersectingRectListForCamp[i] is TowerDefenseZombie zombie) || !TryCarryZombie(zombie)); i++)
			{
			}
		}
	}

	private bool TryCarryZombie(TowerDefenseZombie zombie)
	{
		if (over)
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) || !TowerDefenseManager.CurrentControl.isGameRunning)
		{
			return false;
		}
		if (!inGame)
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(carryCharacter))
		{
			return false;
		}
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
		if (zombie.camp != camp)
		{
			return false;
		}
		if (!zombie.CanCollision(instance.maskFlags))
		{
			return false;
		}
		if (zombie.instance.zombiePhysique > TowerDefenseEnum.ZOMBIE_PHYSIQUE.NORMAL)
		{
			return false;
		}
		if (!zombie.targetRegistrationComponent.canCarry)
		{
			return false;
		}
		return TowerDefenseZombieCarryRelation.Attach(this, ref carryCharacter, zombie, z + 40.0);
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			ReleaseCarriedCharacter();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseCarriedCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessCarryCandidate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryCarryZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseCarriedCharacter && args.Count == 0)
		{
			ReleaseCarriedCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportVariantSaveWhenEmpty());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessCarryCandidate && args.Count == 0)
		{
			ProcessCarryCandidate();
			ret = default;
			return true;
		}
		if (method == MethodName.TryCarryZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryCarryZombie(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
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
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.ReleaseCarriedCharacter)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ProcessCarryCandidate)
		{
			return true;
		}
		if (method == MethodName.TryCarryZombie)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.useCone)
		{
			useCone = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useBucket)
		{
			useBucket = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mowerKill)
		{
			mowerKill = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.carryCharacter)
		{
			carryCharacter = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.useCone)
		{
			value = VariantUtils.CreateFrom(in useCone);
			return true;
		}
		if (name == PropertyName.useBucket)
		{
			value = VariantUtils.CreateFrom(in useBucket);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.mowerKill)
		{
			value = VariantUtils.CreateFrom(in mowerKill);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.useCone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useBucket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mowerKill, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.carryCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.useCone, Variant.From(in useCone));
		info.AddProperty(PropertyName.useBucket, Variant.From(in useBucket));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.mowerKill, Variant.From(in mowerKill));
		info.AddProperty(PropertyName.carryCharacter, Variant.From(in carryCharacter));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.useCone, out var value))
		{
			useCone = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useBucket, out var value2))
		{
			useBucket = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value3))
		{
			over = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mowerKill, out var value4))
		{
			mowerKill = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.carryCharacter, out var value5))
		{
			carryCharacter = value5.As<TowerDefenseCharacter>();
		}
	}
}
