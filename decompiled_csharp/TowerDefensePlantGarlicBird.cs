using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter8/GarlicBird/Scene/TowerDefensePlantGarlicBird.cs")]
public class TowerDefensePlantGarlicBird : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName AttackDeal = "AttackDeal";

		public static readonly StringName ApplySuanNiao = "ApplySuanNiao";

		public static readonly StringName ApplySuanNiaoToCharacter = "ApplySuanNiaoToCharacter";

		public static readonly StringName IsSuanNiaoImmune = "IsSuanNiaoImmune";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName RunDeathrattleCharge = "RunDeathrattleCharge";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _over = "_over";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static PackedScene _SUANIAO_SCENE;

	private static PackedScene _GARLIC_BIRD_SCENE;

	private bool _over;

	private static PackedScene SUANIAO_SCENE => _SUANIAO_SCENE ?? (_SUANIAO_SCENE = GD.Load<PackedScene>("uid://d108rh2c5elsf"));

	private static PackedScene GARLIC_BIRD_SCENE => _GARLIC_BIRD_SCENE ?? (_GARLIC_BIRD_SCENE = GD.Load<PackedScene>("uid://5g1exh0phn56"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			instance.invincibleHurt = true;
		}
	}

	public override void AttackDeal(TowerDefenseCharacter character, string type, double num)
	{
		base.AttackDeal(character, type, num);
		if (GodotObject.IsInstanceValid(character))
		{
			switch (type)
			{
			case "Eat":
				SkipInvincibleHurt(Mathf.Max(10.0, num));
				ApplySuanNiao(character);
				break;
			case "Smash":
				SkipInvincibleHurt(num);
				break;
			case "Chomp":
				Destroy();
				break;
			}
		}
	}

	public void ApplySuanNiao(TowerDefenseCharacter character)
	{
		ApplySuanNiaoToCharacter(character);
	}

	private static void ApplySuanNiaoToCharacter(TowerDefenseCharacter character)
	{
		TowerDefenseZombie zombie = character as TowerDefenseZombie;
		if (zombie == null || zombie.nearDie || zombie.die || zombie.isGarlicBird || zombie.isChangeLine || IsSuanNiaoImmune(zombie))
		{
			return;
		}
		zombie.isGarlicBird = true;
		zombie.attackComponent.alive = false;
		zombie.Walk();
		Node2D effect = SUANIAO_SCENE.Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
		effect.Position = new Vector2(0f, -100f);
		zombie.AddChild(effect, forceReadableName: false, InternalMode.Disabled);
		Timer timer = new Timer();
		timer.OneShot = true;
		timer.WaitTime = 5.0;
		zombie.AddChild(timer, forceReadableName: false, InternalMode.Disabled);
		timer.Timeout += () =>
		{
			if (GodotObject.IsInstanceValid(effect))
			{
				effect.QueueFree();
			}
			if (GodotObject.IsInstanceValid(zombie) && !zombie.nearDie && !zombie.die)
			{
				zombie.attackComponent.alive = true;
				zombie.ChangeLine();
			}
			if (GodotObject.IsInstanceValid(zombie))
			{
				zombie.isGarlicBird = false;
			}
			if (GodotObject.IsInstanceValid(timer))
			{
				timer.QueueFree();
			}
		};
		timer.Start();
	}

	private static bool IsSuanNiaoImmune(TowerDefenseZombie zombie)
	{
		return !GodotObject.IsInstanceValid(zombie.instance);
	}

	public override void DestroySet()
	{
		if (!_over)
		{
			_over = true;
			base.DestroySet();
			instance.invincible = true;
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			float num = (float)(TowerDefenseManager.Instance.GetMapGroundRight() + 200.0);
			float num2 = num - logicalGlobalPosition.X;
			double duration = Mathf.Max(2.0, (double)num2 / 200.0);
			SceneTree tree = GetTree();
			Node2D node2D = new Node2D();
			node2D.Name = "GarlicBirdDeathrattle";
			node2D.Scale = Scale;
			node2D.ZIndex = ZIndex;
			TowerDefenseManager.GetCharacterNode().AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
			AdobeAnimateSpriteBase adobeAnimateSpriteBase = GARLIC_BIRD_SCENE.Instantiate<AdobeAnimateSpriteBase>(PackedScene.GenEditState.Disabled);
			node2D.AddChild(adobeAnimateSpriteBase, forceReadableName: false, InternalMode.Disabled);
			adobeAnimateSpriteBase.SetAnimation("Fire", loop: false, 0.2);
			RunDeathrattleCharge(node2D, tree, gridPos.Y, camp, logicalGlobalPosition, num, duration);
		}
	}

	private static async void RunDeathrattleCharge(Node2D runner, SceneTree tree, int gridY, TowerDefenseEnum.CHARACTER_CAMP sourceCamp, Vector2 startPosition, float targetX, double duration)
	{
		if (!GodotObject.IsInstanceValid(runner) || !GodotObject.IsInstanceValid(tree))
		{
			return;
		}
		runner.GlobalPosition = startPosition;
		Tween chargeTween = runner.CreateTween();
		chargeTween.TweenProperty(runner, "global_position:x", targetX, duration);
		List<TowerDefenseCharacter> hitZombies = new List<TowerDefenseCharacter>();
		while (GodotObject.IsInstanceValid(runner) && chargeTween.IsValid() && chargeTween.IsRunning())
		{
			List<TowerDefenseCharacter> characterLine = TowerDefenseManager.Instance.GetCharacterLine(gridY, fliterGraveStone: false);
			float x = runner.GlobalPosition.X;
			foreach (TowerDefenseCharacter item in characterLine)
			{
				if (GodotObject.IsInstanceValid(item) && item.camp != sourceCamp && !item.nearDie && !item.die && item.HasHitBox && item.IsHitBoxMonitorable && !hitZombies.Contains(item) && Mathf.Abs(item.GetLogicalGlobalPosition().X - x) < 100f)
				{
					item.SkipInvincibleHurt(1000.0);
					ApplySuanNiaoToCharacter(item);
					hitZombies.Add(item);
				}
			}
			await runner.ToSignal(tree, SceneTree.SignalName.PhysicsFrame);
		}
		if (GodotObject.IsInstanceValid(runner))
		{
			runner.QueueFree();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackDeal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySuanNiao, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySuanNiaoToCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsSuanNiaoImmune, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunDeathrattleCharge, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "runner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "tree", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SceneTree"), exported: false),
				new PropertyInfo(Variant.Type.Int, "gridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sourceCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "startPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "targetX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.AttackDeal && args.Count == 3)
		{
			AttackDeal(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySuanNiao && args.Count == 1)
		{
			ApplySuanNiao(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySuanNiaoToCharacter && args.Count == 1)
		{
			ApplySuanNiaoToCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSuanNiaoImmune && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSuanNiaoImmune(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.RunDeathrattleCharge && args.Count == 7)
		{
			RunDeathrattleCharge(VariantUtils.ConvertTo<Node2D>(in args[0]), VariantUtils.ConvertTo<SceneTree>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<float>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ApplySuanNiaoToCharacter && args.Count == 1)
		{
			ApplySuanNiaoToCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSuanNiaoImmune && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSuanNiaoImmune(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.RunDeathrattleCharge && args.Count == 7)
		{
			RunDeathrattleCharge(VariantUtils.ConvertTo<Node2D>(in args[0]), VariantUtils.ConvertTo<SceneTree>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<float>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.AttackDeal)
		{
			return true;
		}
		if (method == MethodName.ApplySuanNiao)
		{
			return true;
		}
		if (method == MethodName.ApplySuanNiaoToCharacter)
		{
			return true;
		}
		if (method == MethodName.IsSuanNiaoImmune)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.RunDeathrattleCharge)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._over)
		{
			_over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._over)
		{
			value = VariantUtils.CreateFrom(in _over);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._over, Variant.From(in _over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._over, out var value))
		{
			_over = value.As<bool>();
		}
	}
}
