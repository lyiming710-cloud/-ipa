using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter10/PotVampire/Scene/TowerDefensePlantPotVampire.cs")]
public class TowerDefensePlantPotVampire : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName Timeout = "Timeout";

		public static readonly StringName TryDrain = "TryDrain";

		public static readonly StringName FindDrainTarget = "FindDrainTarget";

		public static readonly StringName PlayDrainEffect = "PlayDrainEffect";

		public static readonly StringName ShareHealthInCell = "ShareHealthInCell";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static PackedScene _drainScene;

	private const double DrainInterval = 5.0;

	private const double DrainRatio = 0.1;

	private CharacterTimerComponent _timerComponent;

	private static PackedScene DrainScene => _drainScene ?? (_drainScene = GD.Load<PackedScene>("uid://je8no5cuugta"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_timerComponent = componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
			_timerComponent.OnTimeout += Timeout;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased)
		{
			_timerComponent.OnTimeout -= Timeout;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && TowerDefenseManager.Instance.IsGameRunning() && inGame && !_timerComponent.IsRunning("Spawn"))
		{
			_timerComponent.Run("Spawn", 5.0);
		}
	}

	public void Timeout(string timerName)
	{
		if (timerName == "Spawn")
		{
			TryDrain();
			_timerComponent.Run("Spawn", 5.0);
		}
	}

	public void TryDrain()
	{
		if (sprite.pause || instance.sleep || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost))
		{
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter = FindDrainTarget();
		if (GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			double num = towerDefenseCharacter.instance.hitpointsSave * 0.1;
			if (double.IsFinite(num) && !(num <= 0.0))
			{
				sprite.SetAnimation("Attack", loop: false);
				PlayDrainEffect(towerDefenseCharacter);
				towerDefenseCharacter.FlagHurt(num, 16);
				ShareHealthInCell(num);
			}
		}
	}

	private TowerDefenseCharacter FindDrainTarget()
	{
		Array campTarget = TowerDefenseManager.Instance.GetCampTarget(camp);
		double num = -1.0;
		TowerDefenseCharacter result = null;
		foreach (Variant item in campTarget)
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && towerDefenseCharacter.instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS && !(towerDefenseCharacter.instance.hitpoints <= num))
			{
				num = towerDefenseCharacter.instance.hitpoints;
				result = towerDefenseCharacter;
			}
		}
		return result;
	}

	private void PlayDrainEffect(TowerDefenseCharacter target)
	{
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(DrainScene, target.gridPos);
		if (GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce))
		{
			towerDefenseEffectSpriteOnce.GlobalPosition = target.GetLogicalGlobalPosition();
			TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void ShareHealthInCell(double drainHp)
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
		foreach (TowerDefenseCharacter character in cell.GetCharacterList())
		{
			if (GodotObject.IsInstanceValid(character) && !character.die && !character.nearDie && !(character is TowerDefenseCrater) && !(character is TowerDefenseItem) && !(character is TowerDefenseGravestone) && !(character is TowerDefensePlantBowlingBase) && !CheckDifferentCamp(character.camp) && character.instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
			{
				list.Add(character);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		double num = drainHp / (double)list.Count;
		foreach (TowerDefenseCharacter item in list)
		{
			item.Health(num);
			if (item.instance.hitpoints >= item.instance.hitpointsSave)
			{
				item.instance.hitpoints = item.instance.hitpointsSave;
			}
			ShowHealthComponent showHealthComponent = item.showHealthComponent;
			if (showHealthComponent != null && !showHealthComponent.IsReleased)
			{
				item.showHealthComponent.MarkDirty();
			}
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Attack")
		{
			sprite.SetAnimation("Idle");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Timeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryDrain, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindDrainTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayDrainEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShareHealthInCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "drainHp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Timeout && args.Count == 1)
		{
			Timeout(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryDrain && args.Count == 0)
		{
			TryDrain();
			ret = default;
			return true;
		}
		if (method == MethodName.FindDrainTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindDrainTarget());
			return true;
		}
		if (method == MethodName.PlayDrainEffect && args.Count == 1)
		{
			PlayDrainEffect(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShareHealthInCell && args.Count == 1)
		{
			ShareHealthInCell(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.Timeout)
		{
			return true;
		}
		if (method == MethodName.TryDrain)
		{
			return true;
		}
		if (method == MethodName.FindDrainTarget)
		{
			return true;
		}
		if (method == MethodName.PlayDrainEffect)
		{
			return true;
		}
		if (method == MethodName.ShareHealthInCell)
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
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
