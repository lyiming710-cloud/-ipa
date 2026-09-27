using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewIZMReentryPlantAttackRuntimeTest.cs")]
public class BugOverviewIZMReentryPlantAttackRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindRangedAttacker = "FindRangedAttacker";

		public static readonly StringName FindRangedAttackerInTree = "FindRangedAttackerInTree";

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

	private const string LevelPath = "res://Asset/Config/Level/TowerDefense/IZM/IZMLevel2.tres";

	private const string BattleScenePath = "res://Scene/TowerDefesne/TowerDefenseNew/TowerDefenseControlNew.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		try
		{
			Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
			Check(GodotObject.IsInstanceValid(TowerDefenseManager.Instance), "TowerDefenseManager autoload must be available.");
			Check(GodotObject.IsInstanceValid(GameSaveManager.Instance), "GameSaveManager autoload must be available.");
			Check(GodotObject.IsInstanceValid(Global.Instance), "Global autoload must be available.");
			if (!GodotObject.IsInstanceValid(ResourceManager.Instance) || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance) || !GodotObject.IsInstanceValid(GameSaveManager.Instance) || !GodotObject.IsInstanceValid(Global.Instance))
			{
				return;
			}
			GameSaveManager.Instance.EnsureLoaded();
			if (string.IsNullOrEmpty(GameSaveManager.Instance.EnsureUser()))
			{
				GameSaveManager.Instance.SetUserCurrent("IZMReentryPlantAttackProbe");
			}
			await LoadFullGameplayResourcesAsync();
			TowerDefenseLevelConfig level = ResourceLoader.Load<TowerDefenseLevelConfig>("res://Asset/Config/Level/TowerDefense/IZM/IZMLevel2.tres", null, ResourceLoader.CacheMode.IgnoreDeep);
			PackedScene battleScene = ResourceLoader.Load<PackedScene>("res://Scene/TowerDefesne/TowerDefenseNew/TowerDefenseControlNew.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
			Check(GodotObject.IsInstanceValid(level) && level.name == "IZM_Level2", "The real IZM Level 2 resource must load.");
			Check(GodotObject.IsInstanceValid(battleScene) && battleScene.CanInstantiate(), "The production battle scene must load and instantiate.");
			if (!GodotObject.IsInstanceValid(level) || !GodotObject.IsInstanceValid(battleScene) || !battleScene.CanInstantiate())
			{
				return;
			}
			TowerDefenseControlNew firstBattle = await EnterBattleAsync(level, battleScene, "first");
			if (!GodotObject.IsInstanceValid(firstBattle) || !firstBattle.isGameRunning)
			{
				return;
			}
			await VerifyPlantAttackAsync(firstBattle, "first");
			firstBattle.QueueFree();
			await WaitFramesAsync(12);
			Check(!GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentControl), "Exiting the first IZ battle must release the active battle controller.");
			TowerDefenseControlNew towerDefenseControlNew = await EnterBattleAsync(level, battleScene, "second");
			if (!GodotObject.IsInstanceValid(towerDefenseControlNew) || !towerDefenseControlNew.isGameRunning)
			{
				return;
			}
			Check(TowerDefenseManager.Instance.currentControl == towerDefenseControlNew, "The re-entered IZ battle must become the live manager controller.");
			Check(TowerDefenseManager.Instance.IsGameRunning(), "The live game-running query must observe the second IZ battle.");
			await VerifyPlantAttackAsync(towerDefenseControlNew, "second");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[IZMReentryPlantAttack] Unexpected exception: {value}");
		}
		bool flag = _failures == 0 && _checks == 28;
		GD.Print($"IZM_REENTRY_PLANT_ATTACK_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task LoadFullGameplayResourcesAsync()
	{
		bool resourcesLoaded = false;
		ResourceManager.Instance.OnLoadOver += OnLoadOver;
		try
		{
			ResourceManager.Instance.BeginLoad();
			for (int frame = 0; frame < 7200; frame++)
			{
				if (resourcesLoaded)
				{
					break;
				}
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}
		}
		finally
		{
			ResourceManager.Instance.OnLoadOver -= OnLoadOver;
		}
		Check(resourcesLoaded, "Production resources must finish loading before the IZ re-entry scenario.");
		void OnLoadOver()
		{
			resourcesLoaded = true;
		}
	}

	private async Task<TowerDefenseControlNew> EnterBattleAsync(TowerDefenseLevelConfig level, PackedScene battleScene, string phase)
	{
		Global.Instance.enterLevelMode = "LevelChoose";
		TowerDefenseManager.Instance.currentLevelConfig = level;
		TowerDefenseControlNew battle = battleScene.Instantiate<TowerDefenseControlNew>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(battle), "The " + phase + " IZ battle controller must instantiate.");
		if (!GodotObject.IsInstanceValid(battle))
		{
			return null;
		}
		AddChild(battle, forceReadableName: false, InternalMode.Disabled);
		for (int frame = 0; frame < 1500; frame++)
		{
			if (battle.isGameRunning)
			{
				break;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		Check(battle.isGameRunning, "The " + phase + " IZ battle must reach GameRunning.");
		Check(battle.process is TowerDefenseBattleProcessIZM, $"The {phase} battle must use the production IZM process; actual={battle.process?.GetType().Name}.");
		Check(GodotObject.IsInstanceValid(TowerDefenseManager.GetMapFeature()), "The " + phase + " IZ battle must initialize its real map.");
		return battle;
	}

	private async Task VerifyPlantAttackAsync(TowerDefenseControlNew battle, string phase)
	{
		TowerDefensePlant towerDefensePlant = FindRangedAttacker(battle);
		FireComponent fire = towerDefensePlant?.componentManager?.GetRuntime<FireComponent>();
		Check(GodotObject.IsInstanceValid(towerDefensePlant), "The " + phase + " IZ battle must contain a real pre-spawn ranged plant.");
		Check(fire != null && !fire.IsReleased && fire.alive, "The " + phase + " IZ plant must retain a live FireComponent.");
		if (!GodotObject.IsInstanceValid(towerDefensePlant) || fire == null || fire.IsReleased)
		{
			return;
		}
		TowerDefenseZombie zombie = (TowerDefenseManager.GetPacketConfig("ZombieNormal")?.CreateSpawnRuntimeCopy())?.Spawn(towerDefensePlant.gridPos.Y) as TowerDefenseZombie;
		Check(GodotObject.IsInstanceValid(zombie), "The " + phase + " IZ battle must spawn a real normal zombie target.");
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return;
		}
		double hitpointsBefore = zombie.instance.hitpoints;
		bool acquired = false;
		bool damaged = false;
		for (int frame = 0; frame < 900; frame++)
		{
			if (!GodotObject.IsInstanceValid(zombie))
			{
				break;
			}
			if (zombie.isDestroy)
			{
				break;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			if (fire.fireCheckList.Count > 0 && GodotObject.IsInstanceValid(fire.fireCheckList[0]?.projectile))
			{
				acquired |= fire.CanFireCheckOnceByData(fire.fireCheckList[0].projectile.GetProjectile());
			}
			damaged |= zombie.instance.hitpoints < hitpointsBefore;
			if (damaged)
			{
				break;
			}
		}
		Check(acquired | damaged, "The " + phase + " IZ plant must acquire the spawned zombie through its live automatic attack path.");
		Check(damaged || !GodotObject.IsInstanceValid(zombie) || zombie.isDestroy, $"The {phase} IZ plant must damage the spawned zombie; before={hitpointsBefore}, after={zombie?.instance?.hitpoints}.");
	}

	private static TowerDefensePlant FindRangedAttacker(TowerDefenseControlNew battle)
	{
		if (!GodotObject.IsInstanceValid(battle?.characterNode))
		{
			return null;
		}
		return FindRangedAttackerInTree(battle.characterNode);
	}

	private static TowerDefensePlant FindRangedAttackerInTree(Node root)
	{
		foreach (Node child in root.GetChildren())
		{
			if (child is TowerDefensePlant towerDefensePlant && GodotObject.IsInstanceValid(towerDefensePlant) && !towerDefensePlant.isDestroy)
			{
				string text = towerDefensePlant.config?.name ?? "";
				if (text.Contains("Pea", StringComparison.Ordinal) || text == "PlantThreePeater")
				{
					FireComponent fireComponent = towerDefensePlant.componentManager?.GetRuntime<FireComponent>();
					if (fireComponent != null && !fireComponent.IsReleased)
					{
						return towerDefensePlant;
					}
				}
			}
			TowerDefensePlant towerDefensePlant2 = FindRangedAttackerInTree(child);
			if (GodotObject.IsInstanceValid(towerDefensePlant2))
			{
				return towerDefensePlant2;
			}
		}
		return null;
	}

	private async Task WaitFramesAsync(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[IZMReentryPlantAttack] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindRangedAttacker, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "battle", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindRangedAttackerInTree, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
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
		if (method == MethodName.FindRangedAttacker && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlant>(FindRangedAttacker(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0])));
			return true;
		}
		if (method == MethodName.FindRangedAttackerInTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlant>(FindRangedAttackerInTree(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.FindRangedAttacker && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlant>(FindRangedAttacker(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0])));
			return true;
		}
		if (method == MethodName.FindRangedAttackerInTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlant>(FindRangedAttackerInTree(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.FindRangedAttacker)
		{
			return true;
		}
		if (method == MethodName.FindRangedAttackerInTree)
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
