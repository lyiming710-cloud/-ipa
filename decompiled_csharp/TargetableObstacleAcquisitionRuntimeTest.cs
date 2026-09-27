using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/TargetableObstacleAcquisitionRuntimeTest.cs")]
public class TargetableObstacleAcquisitionRuntimeTest : Node
{
	private readonly record struct PlantCase(string Name, string ScenePath);

	private readonly record struct GraveCase(string Name, string ScenePath, bool ExpectedTargetable);

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InstantiateCharacter = "InstantiateCharacter";

		public static readonly StringName Place = "Place";

		public static readonly StringName GroundAndGridFlags = "GroundAndGridFlags";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private static readonly PlantCase[] Plants = new PlantCase[2]
	{
		new PlantCase("PuffShroom", "res://Asset/Anime/Character/Plant/Chapter0/PuffShroom/Scene/TowerDefensePlantPuffShroom.tscn"),
		new PlantCase("PuffPot", "res://Asset/Anime/Character/Plant/Chapter1/PuffPot/Scene/TowerDefensePlantPuffPot.tscn")
	};

	private static readonly GraveCase[] Graves = new GraveCase[4]
	{
		new GraveCase("SunGravestone", "res://Asset/Anime/Character/GraveStone/TargetSun/Scene/TowerDefenseGraveStoneTargetSun.tscn", ExpectedTargetable: true),
		new GraveCase("RuneStones", "res://Asset/Anime/Character/GraveStone/RuneStones/Scene/TowerDefenseRuneStones.tscn", ExpectedTargetable: true),
		new GraveCase("TrashBin", "res://Asset/Anime/Character/GraveStone/TrashBin/Scene/TowerDefenseTrashBin.tscn", ExpectedTargetable: true),
		new GraveCase("OrdinaryGravestone", "res://Asset/Anime/Character/GraveStone/Default/Scene/TowerDefenseGraveStoneDefault.tscn", ExpectedTargetable: false)
	};

	private const string FumeShroomScene = "res://Asset/Anime/Character/Plant/Chapter0/FumeShroom/Scene/TowerDefensePlantFumeShroom.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		try
		{
			await RunScenarios();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[TargetableObstacleAcquisitionRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"TARGETABLE_OBSTACLE_ACQUISITION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task RunScenarios()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
		if (!GodotObject.IsInstanceValid(manager))
		{
			return;
		}
		TowerDefenseControlNew control = (manager.currentControl = new TowerDefenseControlNew
		{
			isGameRunning = true
		});
		manager.gridBeginPos = new Vector2(0f, 100f);
		manager.gridSize = new Vector2(100f, 76f);
		manager.gridNum = new Vector2I(9, 5);
		PlantCase[] plants = Plants;
		foreach (PlantCase plantCase in plants)
		{
			GraveCase[] graves = Graves;
			foreach (GraveCase graveCase in graves)
			{
				await RunOneScenario(plantCase, graveCase);
			}
			await RunExplicitOptOutScenario(plantCase);
		}
		await RunAttackComponentScenario();
		manager.currentControl = null;
		control.Free();
	}

	private async Task RunAttackComponentScenario()
	{
		TowerDefenseCharacter plant = InstantiateCharacter("res://Asset/Anime/Character/Plant/Chapter0/FumeShroom/Scene/TowerDefensePlantFumeShroom.tscn");
		TowerDefenseCharacter grave = InstantiateCharacter(Graves[2].ScenePath);
		Check(GodotObject.IsInstanceValid(plant) && GodotObject.IsInstanceValid(grave), "FumeShroom and TrashBin AttackComponent scenario must instantiate.");
		if (GodotObject.IsInstanceValid(plant) && GodotObject.IsInstanceValid(grave))
		{
			if (grave is TowerDefenseGravestone towerDefenseGravestone)
			{
				towerDefenseGravestone.rise = false;
			}
			Place(plant, new Vector2(100f, 100f), new Vector2I(1, 1));
			Place(grave, new Vector2(200f, 100f), new Vector2I(2, 1));
			AddChild(plant, forceReadableName: false, InternalMode.Disabled);
			AddChild(grave, forceReadableName: false, InternalMode.Disabled);
			await SettleRegistration();
			AttackComponent attackComponent = plant.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
			Check(attackComponent != null && !attackComponent.IsReleased, "FumeShroom AttackComponent must be active.");
			if (attackComponent != null && !attackComponent.IsReleased)
			{
				attackComponent.alive = true;
				attackComponent.groundRight = 10000.0;
				Check(attackComponent.checkGravestone, "FumeShroom must inherit checkGravestone=true from its shared Attack definition.");
				Check(attackComponent.GetTargetList().Contains(grave), "FumeShroom AttackComponent must include the attackable TrashBin in its real target list.");
				Check(attackComponent.GetTarget() == grave, "FumeShroom AttackComponent must acquire the attackable TrashBin as its real target.");
			}
			plant.QueueFree();
			grave.QueueFree();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task RunOneScenario(PlantCase plantCase, GraveCase graveCase)
	{
		TowerDefenseCharacter plant = InstantiateCharacter(plantCase.ScenePath);
		TowerDefenseCharacter grave = InstantiateCharacter(graveCase.ScenePath);
		Check(GodotObject.IsInstanceValid(plant) && GodotObject.IsInstanceValid(grave), plantCase.Name + " and " + graveCase.Name + " must instantiate.");
		if (!GodotObject.IsInstanceValid(plant) || !GodotObject.IsInstanceValid(grave))
		{
			return;
		}
		if (grave is TowerDefenseGravestone towerDefenseGravestone)
		{
			towerDefenseGravestone.rise = false;
		}
		Place(plant, new Vector2(100f, 100f), new Vector2I(1, 1));
		Place(grave, new Vector2(300f, 100f), new Vector2I(3, 1));
		AddChild(plant, forceReadableName: false, InternalMode.Disabled);
		AddChild(grave, forceReadableName: false, InternalMode.Disabled);
		await SettleRegistration();
		FireComponent fireComponent = plant.componentManager?.GetRuntime<FireComponent>("character.fire");
		Check(fireComponent != null && !fireComponent.IsReleased, plantCase.Name + " FireComponent must be active.");
		if (fireComponent != null && !fireComponent.IsReleased)
		{
			Check(fireComponent.checkGravestone, plantCase.Name + " must inherit checkGravestone=true from its shared Fire definition.");
			if (graveCase.ExpectedTargetable)
			{
				Check(grave.camp != plant.camp, graveCase.Name + " must be in the opposing camp.");
				Check(!grave.instance.invincible && grave.instance.canBeCollection, graveCase.Name + " must be a collectible, non-invincible target.");
				Check(grave.targetRegistrationComponent?.canProjectileCheck ?? false, graveCase.Name + " must remain registered for projectile checks.");
				Check(grave.HasHitBox, graveCase.Name + " must retain an attackable hit box.");
				Check((GroundAndGridFlags() & grave.instance.maskFlags) != 0, graveCase.Name + " mask must accept ground/grid projectile acquisition.");
				Check(AabbShapeUtil.SegmentIntersectsRect(plant.GlobalPosition, plant.GlobalPosition + new Vector2(2000f, 0f), grave.WorldHitRect, out var _), plantCase.Name + "'s forward ray must intersect " + graveCase.Name + "'s hit box.");
				List<TowerDefenseCharacter> characterTargetLineWithCollisionFlags = TowerDefenseManager.Instance.GetCharacterTargetLineWithCollisionFlags(plant, GroundAndGridFlags(), fliterGraveStone: false);
				Check(characterTargetLineWithCollisionFlags.Contains(grave), $"The shared target system must expose {graveCase.Name} to {plantCase.Name}.");
			}
			PrepareFire(fireComponent);
			bool flag = fireComponent.CanFireCheckOnce(null, GroundAndGridFlags());
			Check(flag == graveCase.ExpectedTargetable, $"{plantCase.Name} acquisition for {graveCase.Name} expected {graveCase.ExpectedTargetable}, got {flag}.");
		}
		plant.QueueFree();
		grave.QueueFree();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}

	private async Task RunExplicitOptOutScenario(PlantCase plantCase)
	{
		TowerDefenseCharacter plant = InstantiateCharacter(plantCase.ScenePath);
		TowerDefenseCharacter grave = InstantiateCharacter(Graves[0].ScenePath);
		Check(GodotObject.IsInstanceValid(plant) && GodotObject.IsInstanceValid(grave), plantCase.Name + " explicit opt-out scenario must instantiate.");
		if (GodotObject.IsInstanceValid(plant) && GodotObject.IsInstanceValid(grave))
		{
			if (grave is TowerDefenseGravestone towerDefenseGravestone)
			{
				towerDefenseGravestone.rise = false;
			}
			Place(plant, new Vector2(100f, 100f), new Vector2I(1, 1));
			Place(grave, new Vector2(300f, 100f), new Vector2I(3, 1));
			AddChild(plant, forceReadableName: false, InternalMode.Disabled);
			AddChild(grave, forceReadableName: false, InternalMode.Disabled);
			await SettleRegistration();
			FireComponent fireComponent = plant.componentManager?.GetRuntime<FireComponent>("character.fire");
			Check(fireComponent != null && !fireComponent.IsReleased, plantCase.Name + " opt-out FireComponent must be active.");
			if (fireComponent != null && !fireComponent.IsReleased)
			{
				fireComponent.checkGravestone = false;
				PrepareFire(fireComponent);
				Check(!fireComponent.CanFireCheckOnce(null, GroundAndGridFlags()), plantCase.Name + " must ignore an attackable gravestone after explicit checkGravestone=false.");
			}
			plant.QueueFree();
			grave.QueueFree();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private TowerDefenseCharacter InstantiateCharacter(string path)
	{
		return ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
	}

	private static void Place(TowerDefenseCharacter character, Vector2 position, Vector2I gridPosition)
	{
		character.Position = position;
		character.gridPos = gridPosition;
		character.inGame = true;
	}

	private async Task SettleRegistration()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}

	private static void PrepareFire(FireComponent fire)
	{
		fire.alive = true;
		fire.groundRight = 10000f;
		fire.timer = 0f;
		fire.checkInterval = 0;
	}

	private static int GroundAndGridFlags()
	{
		return 9;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[TargetableObstacleAcquisitionRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InstantiateCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Place, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GroundAndGridFlags, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.InstantiateCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(InstantiateCharacter(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Place && args.Count == 3)
		{
			Place(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GroundAndGridFlags && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GroundAndGridFlags());
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Place && args.Count == 3)
		{
			Place(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GroundAndGridFlags && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GroundAndGridFlags());
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
		if (method == MethodName.InstantiateCharacter)
		{
			return true;
		}
		if (method == MethodName.Place)
		{
			return true;
		}
		if (method == MethodName.GroundAndGridFlags)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
