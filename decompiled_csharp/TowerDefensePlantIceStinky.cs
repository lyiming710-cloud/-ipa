using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Colour/IceStinky/Scene/TowerDefensePlantIceStinky.cs")]
public class TowerDefensePlantIceStinky : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName ProcessHitBoxOverlaps = "ProcessHitBoxOverlaps";

		public static readonly StringName HandleHitBoxEntered = "HandleHitBoxEntered";

		public new static readonly StringName AttackDeal = "AttackDeal";

		public static readonly StringName AddBuffForzen = "AddBuffForzen";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName forzenTime = "forzenTime";

		public static readonly StringName isNut = "isNut";

		public static readonly StringName isCrawl = "isCrawl";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double forzenTime = 5.0;

	private CharacterMoveComponent _moveComponent;

	public bool isNut;

	public bool isCrawl;

	private readonly HashSet<TowerDefenseCharacter> _hitOverlaps = new HashSet<TowerDefenseCharacter>();

	private readonly HashSet<TowerDefenseCharacter> _hitScratch = new HashSet<TowerDefenseCharacter>();

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_moveComponent = componentManager.GetRuntime<CharacterMoveComponent>();
			instance.ClearHitpointsEmptyListeners();
			instance.keepAlive = true;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && isCrawl)
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			shadowComponent.saveShadowPosition = new Vector2(shadowComponent.saveShadowPosition.X, logicalGlobalPosition.Y + 30f);
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(logicalGlobalPosition);
			if ((double)logicalGlobalPosition.X > groundRight + 150.0 || logicalGlobalPosition.X < -100f)
			{
				Destroy();
			}
			ProcessHitBoxOverlaps();
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		sprite.timeScale = timeScale;
		if (instance.hitpoints <= 0.0)
		{
			instance.invincible = true;
			instance.hitpoints = 300.0;
			instance.die = false;
			destroyComponent?.EndDeathSettlement();
			sprite.SetAnimation("Out", loop: false, 0.2);
			sprite.AddAnimation("Crawl", 0.0);
			CharacterMoveComponent moveComponent = _moveComponent;
			if (moveComponent != null && !moveComponent.IsReleased)
			{
				_moveComponent.SetVelocity((instance.hypnoses ? Vector2.Left : Vector2.Right) * 20f);
			}
			isCrawl = true;
			instance.collisionFlags = 1;
			cell.CharacterDestroy(this);
		}
	}

	private void ProcessHitBoxOverlaps()
	{
		_hitScratch.Clear();
		if (!TryGetActiveWorldHitRect(out var rect))
		{
			return;
		}
		List<TowerDefenseCharacter> charactersIntersectingRectList = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(rect, gridPos.Y, includeAllLineCheck: true);
		for (int i = 0; i < charactersIntersectingRectList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = charactersIntersectingRectList[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				_hitScratch.Add(towerDefenseCharacter);
				if (!_hitOverlaps.Contains(towerDefenseCharacter))
				{
					HandleHitBoxEntered(towerDefenseCharacter);
				}
			}
		}
		_hitOverlaps.RemoveWhere((TowerDefenseCharacter character) => !GodotObject.IsInstanceValid(character) || !_hitScratch.Contains(character));
		foreach (TowerDefenseCharacter item in _hitScratch)
		{
			_hitOverlaps.Add(item);
		}
	}

	private void HandleHitBoxEntered(TowerDefenseCharacter tdCharacter)
	{
		if (isCrawl && !tdCharacter.instance.die && !tdCharacter.instance.nearDie && !(tdCharacter is TowerDefenseGravestone) && !(tdCharacter is TowerDefenseCrater) && !(tdCharacter is TowerDefensePlantBowlingBase) && (!(tdCharacter is TowerDefenseZombie towerDefenseZombie) || towerDefenseZombie.instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS) && CanCollision(tdCharacter.instance.maskFlags) && CheckDifferentCamp(tdCharacter.camp) && tdCharacter.IsTargetableFromLine(gridPos.Y))
		{
			AddBuffForzen(tdCharacter);
			if (tdCharacter is TowerDefenseZombie towerDefenseZombie2 && towerDefenseZombie2.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.CAR)
			{
				towerDefenseZombie2.Die();
			}
		}
	}

	public override void AttackDeal(TowerDefenseCharacter character, string type, double num)
	{
		base.AttackDeal(character, type, num);
		if (GodotObject.IsInstanceValid(character) && !instance.sleep)
		{
			if (!isNut)
			{
				isNut = true;
				sprite.SetAnimation("In", loop: false, 0.2);
				sprite.AddAnimation("Nut", 0.0);
			}
			AddBuffForzen(character);
		}
	}

	public void AddBuffForzen(TowerDefenseCharacter character)
	{
		TowerDefenseCharacterBuffFrozen towerDefenseCharacterBuffFrozen = new TowerDefenseCharacterBuffFrozen();
		towerDefenseCharacterBuffFrozen.time = (float)forzenTime;
		towerDefenseCharacterBuffFrozen.iceSpeedDownTime = 0.0;
		character.BuffAdd(towerDefenseCharacterBuffFrozen);
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["forzenTime"] = forzenTime,
			["isNut"] = isNut,
			["isCrawl"] = isCrawl
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		forzenTime = (data.ContainsKey("forzenTime") ? data["forzenTime"].AsDouble() : 5.0);
		isNut = data.ContainsKey("isNut") && data["isNut"].AsBool();
		isCrawl = data.ContainsKey("isCrawl") && data["isCrawl"].AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessHitBoxOverlaps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleHitBoxEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tdCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AttackDeal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddBuffForzen, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessHitBoxOverlaps && args.Count == 0)
		{
			ProcessHitBoxOverlaps();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleHitBoxEntered && args.Count == 1)
		{
			HandleHitBoxEntered(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackDeal && args.Count == 3)
		{
			AttackDeal(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddBuffForzen && args.Count == 1)
		{
			AddBuffForzen(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.ProcessHitBoxOverlaps)
		{
			return true;
		}
		if (method == MethodName.HandleHitBoxEntered)
		{
			return true;
		}
		if (method == MethodName.AttackDeal)
		{
			return true;
		}
		if (method == MethodName.AddBuffForzen)
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
		if (name == PropertyName.forzenTime)
		{
			forzenTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.isNut)
		{
			isNut = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isCrawl)
		{
			isCrawl = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.forzenTime)
		{
			value = VariantUtils.CreateFrom(in forzenTime);
			return true;
		}
		if (name == PropertyName.isNut)
		{
			value = VariantUtils.CreateFrom(in isNut);
			return true;
		}
		if (name == PropertyName.isCrawl)
		{
			value = VariantUtils.CreateFrom(in isCrawl);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.forzenTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isNut, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isCrawl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.forzenTime, Variant.From(in forzenTime));
		info.AddProperty(PropertyName.isNut, Variant.From(in isNut));
		info.AddProperty(PropertyName.isCrawl, Variant.From(in isCrawl));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.forzenTime, out var value))
		{
			forzenTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isNut, out var value2))
		{
			isNut = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isCrawl, out var value3))
		{
			isCrawl = value3.As<bool>();
		}
	}
}
