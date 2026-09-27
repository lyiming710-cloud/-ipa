using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/Alien/Scene/TowerDefenseZombieAlien.cs")]
public class TowerDefenseZombieAlien : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public static readonly StringName TryFireLaser = "TryFireLaser";

		public static readonly StringName FireLaser = "FireLaser";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName DestroySet = "DestroySet";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const double ShootDamage = 80.0;

	private const int BrainSunCount = 3;

	private const long BrainSunValue = 25L;

	private AttackComponent _shootAttack;

	public override Dictionary ExportVariantSave()
	{
		return base.ExportVariantSave();
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_shootAttack = componentManager?.GetRuntime<AttackComponent>("character.attack.0");
			attackComponent = componentManager?.GetRuntime<AttackComponent>("character.attack.1");
			useAttackDps = true;
			walkSpeedScale = 1.5;
		}
	}

	public override void WalkProcessing(double delta)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			base.WalkProcessing(delta);
		}
		else if (!die && !nearDie && GodotObject.IsInstanceValid(sprite) && !sprite.pause && !(timeScale <= 0.0) && !(sprite.timeScale <= 0.0))
		{
			TryFireLaser();
			base.WalkProcessing(delta);
		}
	}

	public override void AttackProcessing(double delta)
	{
		if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
		{
			TryFireLaser();
		}
		base.AttackProcessing(delta);
	}

	private void TryFireLaser()
	{
		AttackComponent shootAttack = _shootAttack;
		if (shootAttack != null && !shootAttack.IsReleased && _shootAttack.timer <= 0.0 && _shootAttack.CanAttack())
		{
			FireLaser();
			_shootAttack.Refresh();
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.SetAnimation("Shooting", loop: false, 0.2);
			}
		}
	}

	private void FireLaser()
	{
		AudioManager.Instance.AudioPlay("Spike");
		foreach (TowerDefenseCharacter target in _shootAttack.GetTargetList())
		{
			if (!GodotObject.IsInstanceValid(target))
			{
				continue;
			}
			if (target is TowerDefensePlant towerDefensePlant)
			{
				TowerDefenseEnum.CHARACTER_HEIGHT height = towerDefensePlant.instance.height;
				if (height == TowerDefenseEnum.CHARACTER_HEIGHT.LOW || height == TowerDefenseEnum.CHARACTER_HEIGHT.GROUND)
				{
					continue;
				}
			}
			target.AttackDeal(this, "Smash", 80.0);
			if (GodotObject.IsInstanceValid(target.cell))
			{
				target.cell.AttackDeal(this, "Smash", 80.0);
			}
			target.Hurt(80.0, playSplatAudio: true, Vector2.Zero);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!die && clip == "Shooting")
		{
			if (die)
			{
				SendStateEvent("ToDie");
			}
			else if (GodotObject.IsInstanceValid(sprite))
			{
				bool flag = AttackStateHandle?.IsActive ?? false;
				sprite.SetAnimation(flag ? attackAnimeClip : walkAnimeClip, loop: true, 0.2);
			}
		}
	}

	public override void DestroySet()
	{
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		for (int i = 0; i < 3; i++)
		{
			Vector2 vector = new Vector2(GD.RandRange(-40, 40), GD.RandRange(-30, 30));
			if (instance.hypnoses)
			{
				SunCreate(logicalGlobalPosition + vector, 25L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, new Vector2(GD.RandRange(-50, 50), -400f));
			}
			else
			{
				BrainSunCreate(logicalGlobalPosition + vector, 25L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, new Vector2(GD.RandRange(-50, 50), -400f));
			}
		}
		base.DestroySet();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryFireLaser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FireLaser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
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
		if (method == MethodName.TryFireLaser && args.Count == 0)
		{
			TryFireLaser();
			ret = default;
			return true;
		}
		if (method == MethodName.FireLaser && args.Count == 0)
		{
			FireLaser();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
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
		if (method == MethodName._Ready)
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
		if (method == MethodName.TryFireLaser)
		{
			return true;
		}
		if (method == MethodName.FireLaser)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
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
