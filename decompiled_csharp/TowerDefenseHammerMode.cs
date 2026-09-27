using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/GameMode/TowerDefenseHammerMode.cs")]
public class TowerDefenseHammerMode : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName Attack = "Attack";

		public static readonly StringName TryRepairCraterAtMouse = "TryRepairCraterAtMouse";

		public static readonly StringName TryRepairCraterAtCell = "TryRepairCraterAtCell";

		public static readonly StringName PlayHammerHitEffect = "PlayHammerHitEffect";

		public static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName hammer = "hammer";

		public static readonly StringName isAttack = "isAttack";

		public static readonly StringName checkCharacterList = "checkCharacterList";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static PackedScene _hammerExplosion;

	private AdobeAnimateSpriteBase hammer;

	public bool isAttack;

	public Godot.Collections.Array checkCharacterList = new Godot.Collections.Array();

	private static PackedScene HAMMER_EXPLOSION => _hammerExplosion ?? (_hammerExplosion = GD.Load<PackedScene>("res://Prefab/Particles/Explosion/Hammer/HammerExplosion.tscn"));

	public override void _Ready()
	{
		hammer = GetNode<AdobeAnimateSpriteBase>("%Hammer");
		hammer.OnAnimeCompleted += AnimeCompleted;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!TowerDefenseManager.Instance.currentControl.isGameFail)
		{
			hammer.GlobalPosition = GetGlobalMousePosition();
			if (Input.IsActionJustPressed("Press"))
			{
				AudioManager.Instance.AudioPlay("Swing");
				hammer.SetAnimation("WhackZombie", loop: false, 0.1);
				isAttack = true;
				checkCharacterList.Clear();
				Attack();
			}
		}
	}

	public async void Attack()
	{
		if (!isAttack)
		{
			return;
		}
		isAttack = false;
		Vector2 pos = hammer.GlobalPosition;
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (TryRepairCraterAtMouse(pos))
		{
			return;
		}
		Rect2 checkRect = AabbShapeUtil.RectFromCenter(pos, new Vector2(40f, 80f));
		foreach (TowerDefenseCharacter charactersIntersectingRect in TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(checkRect))
		{
			if (checkCharacterList.Contains(charactersIntersectingRect))
			{
				return;
			}
			if (charactersIntersectingRect.isRise || charactersIntersectingRect is TowerDefenseGravestone || TowerDefenseManager.Instance.IsIZM2Mode() != (charactersIntersectingRect.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT))
			{
				continue;
			}
			PlayHammerHitEffect(charactersIntersectingRect.gridPos, hammer.GlobalPosition);
			checkCharacterList.Add(charactersIntersectingRect);
			double num = 400.0;
			if (charactersIntersectingRect.instance.HasAnyArmor)
			{
				num = Math.Min(num, charactersIntersectingRect.instance.armorList[0].hitPoints);
			}
			if (!(charactersIntersectingRect.Hurt(num) > 0.0) && !(charactersIntersectingRect.instance.hitpoints <= charactersIntersectingRect.config.hitpointsNearDeath))
			{
				return;
			}
			if ((double)GD.Randf() < 0.1)
			{
				for (int i = 0; i < 3; i++)
				{
					charactersIntersectingRect.SunCreate(charactersIntersectingRect.GetLogicalGlobalPosition(), 25L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
				}
			}
			charactersIntersectingRect.Destroy();
			return;
		}
		TryRepairCraterAtMouse(pos);
	}

	private bool TryRepairCraterAtMouse(Vector2 pos)
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return false;
		}
		Vector2I mapGridPosFromMouse = instance.GetMapGridPosFromMouse(pos);
		if (!instance.CheckMapGridPosIn(mapGridPosFromMouse))
		{
			return false;
		}
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(mapGridPosFromMouse);
		return TryRepairCraterAtCell(mapCell, pos);
	}

	public bool TryRepairCraterAtCell(TowerDefenseCellInstance cell, Vector2 hitPosition)
	{
		TowerDefenseCrater towerDefenseCrater = (GodotObject.IsInstanceValid(cell) ? cell.GetRepairableCrater() : null);
		if (!GodotObject.IsInstanceValid(towerDefenseCrater))
		{
			return false;
		}
		PlayHammerHitEffect(towerDefenseCrater.gridPos, hitPosition);
		towerDefenseCrater.Destroy();
		return true;
	}

	private void PlayHammerHitEffect(Vector2I gridPos, Vector2 hitPosition)
	{
		AudioManager.Instance?.AudioPlay("Bonk");
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (GodotObject.IsInstanceValid(characterNode))
		{
			TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(HAMMER_EXPLOSION, gridPos);
			towerDefenseEffectParticlesOnce.GlobalPosition = hitPosition;
			characterNode.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public void AnimeCompleted(string clip)
	{
		if (clip == "WhackZombie")
		{
			hammer.SetAnimation("Idle", loop: false, 0.1);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.Attack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryRepairCraterAtMouse, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryRepairCraterAtCell, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "hitPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayHammerHitEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "hitPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Attack && args.Count == 0)
		{
			Attack();
			ret = default;
			return true;
		}
		if (method == MethodName.TryRepairCraterAtMouse && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryRepairCraterAtMouse(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.TryRepairCraterAtCell && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryRepairCraterAtCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.PlayHammerHitEffect && args.Count == 2)
		{
			PlayHammerHitEffect(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.Attack)
		{
			return true;
		}
		if (method == MethodName.TryRepairCraterAtMouse)
		{
			return true;
		}
		if (method == MethodName.TryRepairCraterAtCell)
		{
			return true;
		}
		if (method == MethodName.PlayHammerHitEffect)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.hammer)
		{
			hammer = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName.isAttack)
		{
			isAttack = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkCharacterList)
		{
			checkCharacterList = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.hammer)
		{
			value = VariantUtils.CreateFrom(in hammer);
			return true;
		}
		if (name == PropertyName.isAttack)
		{
			value = VariantUtils.CreateFrom(in isAttack);
			return true;
		}
		if (name == PropertyName.checkCharacterList)
		{
			value = VariantUtils.CreateFrom(in checkCharacterList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.hammer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isAttack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.checkCharacterList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.hammer, Variant.From(in hammer));
		info.AddProperty(PropertyName.isAttack, Variant.From(in isAttack));
		info.AddProperty(PropertyName.checkCharacterList, Variant.From(in checkCharacterList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.hammer, out var value))
		{
			hammer = value.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName.isAttack, out var value2))
		{
			isAttack = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkCharacterList, out var value3))
		{
			checkCharacterList = value3.As<Godot.Collections.Array>();
		}
	}
}
