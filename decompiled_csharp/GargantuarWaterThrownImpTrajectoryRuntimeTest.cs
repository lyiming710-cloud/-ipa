using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/GargantuarWaterThrownImpTrajectoryRuntimeTest.cs")]
public class GargantuarWaterThrownImpTrajectoryRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SpawnThrownImp = "SpawnThrownImp";

		public static readonly StringName SpawnReadyPreparedThrownImp = "SpawnReadyPreparedThrownImp";

		public static readonly StringName CheckSpawnStart = "CheckSpawnStart";

		public static readonly StringName CheckMidFlight = "CheckMidFlight";

		public static readonly StringName Require = "Require";

		public static readonly StringName Check = "Check";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName controlCharacterNode = "controlCharacterNode";

		public static readonly StringName _checks = "_checks";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ImpScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Scene/Base/TowerDefenseZombieImp.tscn";

	private const string ResultMarker = "GARGANTUAR_WATER_THROWN_IMP_TRAJECTORY_RESULT";

	private static readonly Vector2I WaterGrid = new Vector2I(5, 2);

	private readonly List<string> _failures = new List<string>();

	private int _checks;

	private Node2D controlCharacterNode => (TowerDefenseManager.Instance.currentControl as GargantuarWaterThrownImpTrajectoryControlStub)?.characterNode;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		bool previousUseBatch = TowerDefenseZombie.UseBatch;
		AdobeAnimateRenderBackend previousBackend = Global.Instance.adobeAnimateRenderBackend;
		GargantuarWaterThrownImpTrajectoryControlStub control = null;
		PackedScene scene = null;
		TowerDefenseZombieImpBase firstImp = null;
		TowerDefenseZombieImpBase secondImp = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Require(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload is unavailable.");
				control = new GargantuarWaterThrownImpTrajectoryControlStub
				{
					Name = "GargantuarWaterThrownImpTrajectoryControl",
					isGameRunning = true,
					isInit = true
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				TowerDefenseZombie.UseBatch = false;
				Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.CpuPose;
				TowerDefenseCellConfig towerDefenseCellConfig = new TowerDefenseCellConfig();
				towerDefenseCellConfig.gridType.Add(TowerDefenseEnum.PLANTGRIDTYPE.WATER);
				TowerDefenseCellInstance waterCell = new TowerDefenseCellInstance
				{
					gridPos = WaterGrid
				};
				waterCell.Init(towerDefenseCellConfig);
				Check(waterCell.isWater, "The fixture must use a real WATER cell.");
				scene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Imp/Scene/Base/TowerDefenseZombieImp.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
				Check(GodotObject.IsInstanceValid(scene), "The real base Imp scene must load.");
				Require(GodotObject.IsInstanceValid(scene), "The real base Imp scene failed to load.");
				firstImp = SpawnThrownImp(scene, waterCell, 500f, 250f);
				CheckSpawnStart(firstImp, "first");
				await WaitFrames(4);
				CheckMidFlight(firstImp, 500f, 250f, "first");
				firstImp.QueueFree();
				firstImp = null;
				await WaitFrames(3);
				secondImp = SpawnReadyPreparedThrownImp(scene, waterCell, 520f, 270f);
				CheckSpawnStart(secondImp, "second");
				await WaitFrames(4);
				CheckMidFlight(secondImp, 520f, 270f, "second");
			}
			catch (Exception value)
			{
				_failures.Add($"Unexpected runtime exception: {value}");
			}
		}
		finally
		{
			TowerDefenseZombie.UseBatch = previousUseBatch;
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			if (GodotObject.IsInstanceValid(firstImp) && !firstImp.IsQueuedForDeletion())
			{
				firstImp.QueueFree();
			}
			if (GodotObject.IsInstanceValid(secondImp) && !secondImp.IsQueuedForDeletion())
			{
				secondImp.QueueFree();
			}
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			Global.Instance.adobeAnimateRenderBackend = previousBackend;
			await WaitFrames(4);
			scene?.Dispose();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
		}
		Finish();
	}

	private TowerDefenseZombieImpBase SpawnThrownImp(PackedScene scene, TowerDefenseCellInstance waterCell, float startX, float landX)
	{
		TowerDefenseZombieImpBase towerDefenseZombieImpBase = scene.Instantiate<TowerDefenseZombieImpBase>(PackedScene.GenEditState.Disabled);
		Require(GodotObject.IsInstanceValid(towerDefenseZombieImpBase), "The real base Imp scene failed to instantiate.");
		towerDefenseZombieImpBase.inGame = true;
		towerDefenseZombieImpBase.editorPreviewMode = false;
		towerDefenseZombieImpBase.gridPos = WaterGrid;
		towerDefenseZombieImpBase.cell = waterCell;
		towerDefenseZombieImpBase.GlobalPosition = new Vector2(startX, 180f);
		towerDefenseZombieImpBase.z = 90.0;
		towerDefenseZombieImpBase.ImportNetworkSpawnState(new Dictionary
		{
			["throw"] = true,
			["y_speed"] = -60.0,
			["land_pos_x"] = landX,
			["fall_duration"] = 1.0,
			["throw_ease"] = 1,
			["throw_transition"] = 4
		});
		controlCharacterNode.AddChild(towerDefenseZombieImpBase, forceReadableName: false, InternalMode.Disabled);
		return towerDefenseZombieImpBase;
	}

	private TowerDefenseZombieImpBase SpawnReadyPreparedThrownImp(PackedScene scene, TowerDefenseCellInstance waterCell, float startX, float landX)
	{
		TowerDefenseZombieImpBase towerDefenseZombieImpBase = scene.Instantiate<TowerDefenseZombieImpBase>(PackedScene.GenEditState.Disabled);
		Require(GodotObject.IsInstanceValid(towerDefenseZombieImpBase), "The ready-prepared base Imp scene failed to instantiate.");
		towerDefenseZombieImpBase.inGame = false;
		towerDefenseZombieImpBase.editorPreviewMode = false;
		towerDefenseZombieImpBase.Visible = false;
		towerDefenseZombieImpBase.ProcessMode = ProcessModeEnum.Disabled;
		controlCharacterNode.AddChild(towerDefenseZombieImpBase, forceReadableName: false, InternalMode.Disabled);
		Require(towerDefenseZombieImpBase.IsNodeReady(), "The ready-prepared Imp must finish Ready before throw state import.");
		towerDefenseZombieImpBase.gridPos = WaterGrid;
		towerDefenseZombieImpBase.cell = waterCell;
		towerDefenseZombieImpBase.GlobalPosition = new Vector2(startX, 180f);
		towerDefenseZombieImpBase.z = 90.0;
		towerDefenseZombieImpBase.ImportNetworkSpawnState(new Dictionary
		{
			["throw"] = true,
			["y_speed"] = -60.0,
			["land_pos_x"] = landX,
			["fall_duration"] = 1.0,
			["throw_ease"] = 1,
			["throw_transition"] = 4
		});
		towerDefenseZombieImpBase.inGame = true;
		towerDefenseZombieImpBase.Visible = true;
		towerDefenseZombieImpBase.ProcessMode = ProcessModeEnum.Inherit;
		towerDefenseZombieImpBase.ActivateGameplayProcessing();
		towerDefenseZombieImpBase.groundHeightComponent?.DetectEnvironment();
		return towerDefenseZombieImpBase;
	}

	private void CheckSpawnStart(TowerDefenseZombieImpBase imp, string label)
	{
		Check(GodotObject.IsInstanceValid(imp) && imp.SceneFilePath == "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Scene/Base/TowerDefenseZombieImp.tscn" && imp.config?.name == "ZombieImp", "The " + label + " spawn must use the real base Imp scene/config.");
		Check(imp._throw && !imp.landOver && !imp.isGround, "The " + label + " thrown Imp must enter the tree in airborne state.");
		Check(imp.inWater && Mathf.IsEqualApprox((float)imp.groundHeight, (float)(0.0 - imp.waterHeight)), "The " + label + " thrown Imp must retain the real water-lane landing surface.");
		Check(imp.z > imp.groundHeight + 80.0, $"The {label} thrown Imp must retain its hand-height z on water entry; z={imp.z}, ground={imp.groundHeight}.");
	}

	private void CheckMidFlight(TowerDefenseZombieImpBase imp, float startX, float landX, string label)
	{
		string text = imp.impFlightComponent?.StateMachine?.CurrentStateHandle?.StableId;
		Check(text == "imp_flight.fly", $"The {label} thrown Imp must still own the Fly state mid-trajectory; main={imp.CurrentStateHandle?.StableId}, flight={text}.");
		Check(imp.z > imp.groundHeight + 70.0, $"The {label} thrown Imp must remain visibly above the water mid-trajectory; z={imp.z}, ground={imp.groundHeight}, ySpeed={imp.ySpeed}, isGround={imp.isGround}, inWater={imp.inWater}.");
		float num = Mathf.Min(startX, landX);
		float num2 = Mathf.Max(startX, landX);
		Check(imp.GlobalPosition.X > num + 1f && imp.GlobalPosition.X < num2 - 1f, $"The {label} thrown Imp must traverse between hand and landing positions; x={imp.GlobalPosition.X}, range=({num}, {num2}).");
	}

	private static void Require(bool condition, string message)
	{
		if (!condition)
		{
			throw new InvalidOperationException(message);
		}
	}

	private async Task WaitFrames(int count)
	{
		for (int index = 0; index < count; index++)
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
			_failures.Add(message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PushError("[GargantuarWaterThrownImpTrajectoryRuntimeTest] " + failure);
		}
		bool flag = _failures.Count == 0 && _checks == 17;
		GD.Print($"{"GARGANTUAR_WATER_THROWN_IMP_TRAJECTORY_RESULT"} passed={flag} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnThrownImp, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Object, "waterCell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "startX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "landX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnReadyPreparedThrownImp, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Object, "waterCell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "startX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "landX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckSpawnStart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "imp", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckMidFlight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "imp", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "startX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "landX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SpawnThrownImp && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombieImpBase>(SpawnThrownImp(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
			return true;
		}
		if (method == MethodName.SpawnReadyPreparedThrownImp && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombieImpBase>(SpawnReadyPreparedThrownImp(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
			return true;
		}
		if (method == MethodName.CheckSpawnStart && args.Count == 2)
		{
			CheckSpawnStart(VariantUtils.ConvertTo<TowerDefenseZombieImpBase>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckMidFlight && args.Count == 4)
		{
			CheckMidFlight(VariantUtils.ConvertTo<TowerDefenseZombieImpBase>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.SpawnThrownImp)
		{
			return true;
		}
		if (method == MethodName.SpawnReadyPreparedThrownImp)
		{
			return true;
		}
		if (method == MethodName.CheckSpawnStart)
		{
			return true;
		}
		if (method == MethodName.CheckMidFlight)
		{
			return true;
		}
		if (method == MethodName.Require)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Finish)
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
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.controlCharacterNode)
		{
			value = VariantUtils.CreateFrom<Node2D>(controlCharacterNode);
			return true;
		}
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.controlCharacterNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
	}
}
