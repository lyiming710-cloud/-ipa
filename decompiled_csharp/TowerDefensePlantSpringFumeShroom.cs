using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter6/SpringFumeShroom/Scene/TowerDefensePlantSpringFumeShroom.cs")]
public class TowerDefensePlantSpringFumeShroom : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName CanSleep = "CanSleep";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ProcessHitboxOverlaps = "ProcessHitboxOverlaps";

		public static readonly StringName HitboxEntered = "HitboxEntered";

		public static readonly StringName Hit = "Hit";

		public static readonly StringName Flip = "Flip";

		public static readonly StringName BlockCharacter = "BlockCharacter";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName over = "over";

		public static readonly StringName springList = "springList";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private BlockComponent _blockComponent;

	public bool over;

	public Array springList = new Array();

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_blockComponent = componentManager.GetRuntime<BlockComponent>();
			if (_blockComponent != null)
			{
				_blockComponent.OnBlock += BlockCharacter;
				GetNode<Button>("SpriteGroup/Button").Pressed += Flip;
				_blockComponent.SetCheckRectangleSize(TowerDefenseManager.Instance.GetMapGridSize() * 0.5f);
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_blockComponent != null)
		{
			_blockComponent.OnBlock -= BlockCharacter;
		}
		_blockComponent = null;
	}

	public override bool CanSleep()
	{
		if (TowerDefenseManager.Instance.IsIZMMode())
		{
			return false;
		}
		return base.CanSleep();
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint())
		{
			instance.canBeCollection = true;
			ProcessHitboxOverlaps();
		}
	}

	private void ProcessHitboxOverlaps()
	{
		if (TryGetActiveWorldHitRect(out var rect))
		{
			List<TowerDefenseCharacter> charactersIntersectingRectList = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(rect, gridPos.Y);
			for (int i = 0; i < charactersIntersectingRectList.Count; i++)
			{
				HitboxEntered(charactersIntersectingRectList[i]);
			}
		}
	}

	public virtual async void HitboxEntered(TowerDefenseCharacter character)
	{
		if (!instance.sleep && !CanSleep() && !springList.Contains(character) && GodotObject.IsInstanceValid(character) && !character.die && !character.nearDie && !character.instance.invincible && CheckDifferentCamp(character.camp) && CanCollision(character.instance.maskFlags) && character.IsTargetableFromLine(gridPos.Y, includeAllLineCheck: false))
		{
			Hit(character);
			springList.Add(character);
			await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);
			springList.Remove(character);
		}
	}

	public virtual void Hit(TowerDefenseCharacter character)
	{
		if (!TowerDefenseManager.Instance.IsGameRunning() || !(character is TowerDefenseZombie) || !CanCollision(character.instance.maskFlags) || !CanTarget(character) || (character.instance.unUseBuffFlags & 0x200) != 0)
		{
			return;
		}
		sprite.SetAnimation("Attack", loop: false, 0.2);
		sprite.AddAnimation("Idle", 0.0);
		if (((character.config is TowerDefenseZombieConfig towerDefenseZombieConfig) ? towerDefenseZombieConfig.physique : TowerDefenseEnum.ZOMBIE_PHYSIQUE.NORMAL) < TowerDefenseEnum.ZOMBIE_PHYSIQUE.HUGE)
		{
			Hurt(50.0);
		}
		else
		{
			Hurt(100.0);
		}
		character.ySpeed = -200.0;
		Tween tween = character.CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Cubic);
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		float x = logicalGlobalPosition.X + TowerDefenseManager.Instance.GetMapGridSize().X * 2f;
		if (instance.hypnoses)
		{
			x = logicalGlobalPosition.X - TowerDefenseManager.Instance.GetMapGridSize().X * 2f;
		}
		tween.TweenMethod(Callable.From<Vector2>(character.SetLogicalGlobalPosition), logicalGlobalPosition, new Vector2(x, logicalGlobalPosition.Y), 2.0);
		if (character.Get("jumpOver").VariantType == Variant.Type.Nil)
		{
			return;
		}
		character.Set("jumpOver", true);
		if (character.Get("moveTween").VariantType != Variant.Type.Nil)
		{
			Tween tween2 = character.Get("moveTween").As<Tween>();
			if (GodotObject.IsInstanceValid(tween2) && tween2.IsRunning())
			{
				tween2.Kill();
			}
		}
		((TowerDefenseZombie)character).Walk();
	}

	public virtual void Flip()
	{
		if (!instance.sleep)
		{
			sprite.SetAnimation("Attack", loop: false, 0.2);
			sprite.AddAnimation("Idle", 0.0);
		}
		else
		{
			sprite.SetAnimation("Attack", loop: false, 0.2);
			sprite.AddAnimation("Sleep", 0.0);
		}
	}

	public virtual void BlockCharacter()
	{
		if (!instance.sleep && !CanSleep())
		{
			sprite.SetAnimation("Attack", loop: false, 0.2);
			sprite.AddAnimation("Idle", 0.0);
			Hurt(50.0);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "over", over } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		over = data.GetValueOrDefault("over", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanSleep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessHitboxOverlaps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitboxEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Hit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Flip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.CanSleep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSleep());
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessHitboxOverlaps && args.Count == 0)
		{
			ProcessHitboxOverlaps();
			ret = default;
			return true;
		}
		if (method == MethodName.HitboxEntered && args.Count == 1)
		{
			HitboxEntered(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Hit && args.Count == 1)
		{
			Hit(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Flip && args.Count == 0)
		{
			Flip();
			ret = default;
			return true;
		}
		if (method == MethodName.BlockCharacter && args.Count == 0)
		{
			BlockCharacter();
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.CanSleep)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.ProcessHitboxOverlaps)
		{
			return true;
		}
		if (method == MethodName.HitboxEntered)
		{
			return true;
		}
		if (method == MethodName.Hit)
		{
			return true;
		}
		if (method == MethodName.Flip)
		{
			return true;
		}
		if (method == MethodName.BlockCharacter)
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
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.springList)
		{
			springList = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.springList)
		{
			value = VariantUtils.CreateFrom(in springList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.springList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.springList, Variant.From(in springList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.over, out var value))
		{
			over = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.springList, out var value2))
		{
			springList = value2.As<Array>();
		}
	}
}
