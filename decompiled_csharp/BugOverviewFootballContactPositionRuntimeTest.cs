using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewFootballContactPositionRuntimeTest.cs")]
public class BugOverviewFootballContactPositionRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

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

	private const int ScenarioCount = 6;

	private static readonly Vector2 GridSize = new Vector2(100f, 76f);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BugOverviewFootballContactPositionControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_00c9;
				}
				await LoadFullGameplayResourcesAsync();
				control = new BugOverviewFootballContactPositionControlStub
				{
					Name = "FootballContactPositionControl",
					isGameRunning = true,
					isInit = false,
					levelConfig = new TowerDefenseLevelConfig
					{
						finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE
					}
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = GridSize;
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				Check(GodotObject.IsInstanceValid(TowerDefenseManager.GetMapFeature()), "The focused encounter must publish a real map feature.");
				for (int scenario = 0; scenario < 6; scenario++)
				{
					await VerifyContactScenarioAsync(control, scenario);
				}
				goto end_IL_00b7;
				end_IL_00c9:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[FootballContactPosition] Unexpected exception: {value}");
				goto end_IL_00b7;
			}
			return;
			end_IL_00b7:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(control?.characterNode))
			{
				foreach (Node child in control.characterNode.GetChildren())
				{
					if (!child.IsQueuedForDeletion())
					{
						child.QueueFree();
					}
				}
			}
			await WaitFramesAsync(6);
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			mapFeature?.Destroy();
			if (GodotObject.IsInstanceValid(mapControl))
			{
				mapControl.Free();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
		}
		bool flag = _failures == 0 && _checks == 52;
		GD.Print($"FOOTBALL_CONTACT_POSITION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyContactScenarioAsync(BugOverviewFootballContactPositionControlStub control, int scenario)
	{
		int line = scenario % 5 + 1;
		TowerDefensePlant wallnut = TowerDefenseManager.GetPacketConfig("PlantWallnut")?.Plant(new Vector2I(4, line), playAudio: false) as TowerDefensePlant;
		TowerDefenseZombieFootball football = TowerDefenseManager.GetPacketConfig("ZombieFootball")?.Plant(new Vector2I(6, line), playAudio: false) as TowerDefenseZombieFootball;
		await WaitFramesAsync(6);
		Check(GodotObject.IsInstanceValid(wallnut) && wallnut.config?.name == "PlantWallnut", $"Scenario {scenario + 1} must create the real Wall-nut target.");
		Check(GodotObject.IsInstanceValid(football) && football.config?.name == "ZombieFootball", $"Scenario {scenario + 1} must create the real normal Football Zombie.");
		if (!GodotObject.IsInstanceValid(wallnut) || !GodotObject.IsInstanceValid(football))
		{
			return;
		}
		Vector2 previousPosition = football.GetLogicalGlobalPosition();
		float initialY = previousPosition.Y;
		float maxHorizontalStep = 0f;
		float maxVerticalDeviation = 0f;
		bool rowStable = true;
		bool enteredAttack = false;
		football.Walk();
		for (int frame = 0; frame < 720; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			Vector2 logicalGlobalPosition = football.GetLogicalGlobalPosition();
			maxHorizontalStep = Mathf.Max(maxHorizontalStep, Mathf.Abs(logicalGlobalPosition.X - previousPosition.X));
			maxVerticalDeviation = Mathf.Max(maxVerticalDeviation, Mathf.Abs(logicalGlobalPosition.Y - initialY));
			rowStable &= football.gridPos.Y == line;
			previousPosition = logicalGlobalPosition;
			if (football.CurrentStateHandle?.StableId == "zombie.attack" && football.attackComponent?.target == wallnut)
			{
				enteredAttack = true;
				break;
			}
		}
		Check(enteredAttack, $"Scenario {scenario + 1} must naturally enter the authored contact attack state.");
		Check(rowStable && football.gridPos.Y == line, $"Scenario {scenario + 1} must retain logical row {line}; actual={football.gridPos.Y}.");
		Check(maxVerticalDeviation < 0.5f, $"Scenario {scenario + 1} must not jump upward by a row; maximum vertical deviation={maxVerticalDeviation}.");
		Check(maxHorizontalStep < 12f, $"Scenario {scenario + 1} must not jump forward or backward by a cell; maximum one-frame horizontal step={maxHorizontalStep}.");
		Vector2 attackPosition = football.GetLogicalGlobalPosition();
		float maxAttackDrift = 0f;
		for (int frame = 0; frame < 60; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			maxAttackDrift = Mathf.Max(maxAttackDrift, football.GetLogicalGlobalPosition().DistanceTo(attackPosition));
		}
		Check(maxAttackDrift < 0.5f, $"Scenario {scenario + 1} must stop root movement after contact; maximum attack drift={maxAttackDrift}.");
		Check(football.attackComponent?.target == wallnut && football.CurrentStateHandle?.StableId == "zombie.attack", $"Scenario {scenario + 1} must keep the contacted Wall-nut without a position-reset transition.");
		wallnut.QueueFree();
		football.QueueFree();
		await WaitFramesAsync(8);
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
		Check(resourcesLoaded, "Production resources must finish loading before the Football contact scenarios.");
		void OnLoadOver()
		{
			resourcesLoaded = true;
		}
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			gridNum = gridNum,
			gridBeginPos = Vector2.Zero,
			gridSize = GridSize,
			plantOffset = 50.0
		};
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig,
			rect = TowerDefenseBattleFeatureMap.BuildProjectileBoundaryRect(towerDefenseMapConfig)
		});
		towerDefenseBattleFeatureMap.PlantGridInit();
		for (int i = 1; i <= gridNum.X; i++)
		{
			for (int j = 1; j <= gridNum.Y; j++)
			{
				TowerDefenseCellConfig config = new TowerDefenseCellConfig
				{
					gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
					{
						TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
						TowerDefenseEnum.PLANTGRIDTYPE.AIR
					}
				};
				towerDefenseBattleFeatureMap.GetPlantGridCell(new Vector2I(i, j)).Init(config);
			}
		}
		return towerDefenseBattleFeatureMap;
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
			GD.PushError("[FootballContactPosition] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
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
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
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
		if (method == MethodName.CreateMapFeature)
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
