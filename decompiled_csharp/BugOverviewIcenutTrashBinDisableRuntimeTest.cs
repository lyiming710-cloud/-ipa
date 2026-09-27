using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewIcenutTrashBinDisableRuntimeTest.cs")]
public class BugOverviewIcenutTrashBinDisableRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifyDisableLifecycle = "VerifyDisableLifecycle";

		public static readonly StringName PlaceCharacter = "PlaceCharacter";

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

	private const string IcenutScenePath = "res://Asset/Anime/Character/Plant/Chapter2/Icenut/Scene/TowerDefensePlantIcenut.tscn";

	private const string TrashBinScenePath = "res://Asset/Anime/Character/GraveStone/TrashBin/Scene/TowerDefenseTrashBin.tscn";

	private static readonly Vector2 GridBegin = new Vector2(100f, 100f);

	private static readonly Vector2 GridSize = new Vector2(100f, 76f);

	private static readonly Vector2I GridNum = new Vector2I(9, 5);

	private static readonly Vector2I TestGrid = new Vector2I(4, 2);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BugOverviewIcenutTrashBinDisableControlStub control = null;
		TowerDefensePlantIcenut icenut = null;
		TowerDefenseTrashBin trashBin = null;
		bool icenutRegistered = false;
		bool trashBinRegistered = false;
		try
		{
			Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
			if (!GodotObject.IsInstanceValid(manager))
			{
				throw new InvalidOperationException("TowerDefenseManager is unavailable.");
			}
			control = new BugOverviewIcenutTrashBinDisableControlStub
			{
				Name = "IcenutTrashBinDisableControl",
				isGameRunning = true,
				isInit = true,
				levelConfig = new TowerDefenseLevelConfig()
			};
			AddChild(control, forceReadableName: false, InternalMode.Disabled);
			control.characterNode = new Node2D
			{
				Name = "CharacterNode"
			};
			control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
			manager.currentControl = control;
			manager.gridBeginPos = GridBegin;
			manager.gridSize = GridSize;
			manager.gridNum = GridNum;
			icenut = Instantiate<TowerDefensePlantIcenut>("res://Asset/Anime/Character/Plant/Chapter2/Icenut/Scene/TowerDefensePlantIcenut.tscn");
			trashBin = Instantiate<TowerDefenseTrashBin>("res://Asset/Anime/Character/GraveStone/TrashBin/Scene/TowerDefenseTrashBin.tscn");
			Check(GodotObject.IsInstanceValid(icenut), "The real Icenut scene must instantiate.");
			Check(GodotObject.IsInstanceValid(trashBin), "The real TrashBin scene must instantiate.");
			if (!GodotObject.IsInstanceValid(icenut) || !GodotObject.IsInstanceValid(trashBin))
			{
				throw new InvalidOperationException("A real character fixture failed to instantiate.");
			}
			PlaceCharacter(icenut);
			PlaceCharacter(trashBin);
			TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
			{
				gridPos = TestGrid
			};
			towerDefenseCellInstance.characterList.Add(icenut);
			towerDefenseCellInstance.characterList.Add(trashBin);
			icenut.cell = towerDefenseCellInstance;
			trashBin.cell = towerDefenseCellInstance;
			control.characterNode.AddChild(icenut, forceReadableName: false, InternalMode.Disabled);
			control.characterNode.AddChild(trashBin, forceReadableName: false, InternalMode.Disabled);
			await WaitFrames(3);
			manager.CharacterRegister(icenut);
			icenutRegistered = true;
			manager.CharacterRegister(trashBin);
			trashBinRegistered = true;
			VerifyDisableLifecycle(icenut, trashBin);
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BugOverviewIcenutTrashBinDisableRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				if (trashBinRegistered && GodotObject.IsInstanceValid(trashBin))
				{
					manager.CharacterUnregister(trashBin);
				}
				if (icenutRegistered && GodotObject.IsInstanceValid(icenut))
				{
					manager.CharacterUnregister(icenut);
				}
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(trashBin))
			{
				trashBin.QueueFree();
			}
			if (GodotObject.IsInstanceValid(icenut))
			{
				icenut.QueueFree();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag = _failures == 0;
		GD.Print($"BUG_OVERVIEW_ICENUT_TRASHBIN_DISABLE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void VerifyDisableLifecycle(TowerDefensePlantIcenut icenut, TowerDefenseTrashBin trashBin)
	{
		PeriodicAreaEventComponent periodicAreaEventComponent = icenut.componentManager?.GetRuntime<PeriodicAreaEventComponent>("character.periodic_area_event");
		Check(periodicAreaEventComponent != null && !periodicAreaEventComponent.IsReleased, "The real Icenut PeriodicAreaEventComponent must be active.");
		if (periodicAreaEventComponent == null || periodicAreaEventComponent.IsReleased)
		{
			throw new InvalidOperationException("Icenut periodic component is unavailable.");
		}
		Check(icenut.componentAlive, "Icenut components must begin enabled.");
		Check(trashBin.camp == TowerDefenseEnum.CHARACTER_CAMP.ALL && icenut.CanTarget(trashBin) && icenut.CanCollision(trashBin.instance.maskFlags), "The real TrashBin must be an eligible target for Icenut's contact damage.");
		double hitpoints = trashBin.instance.hitpoints;
		periodicAreaEventComponent.EventExecute();
		double num = hitpoints - trashBin.instance.hitpoints;
		Check(Math.Abs(num - 20.0) < 0.001, $"Enabled Icenut contact damage must deal 20 damage to the real TrashBin; dealt {num}.");
		trashBin.BatchUpdate(0.0);
		Check(icenut.characterDisabled && !icenut.componentAlive && icenut.invisible && !icenut.instance.canBeCollection && icenut.skipDestroySet, "The real TrashBin cover pass must disable and hide Icenut.");
		double hitpoints2 = trashBin.instance.hitpoints;
		float timer = periodicAreaEventComponent.timer;
		periodicAreaEventComponent.EventExecute();
		periodicAreaEventComponent.PhysicsProcess(1.5, Engine.GetPhysicsFrames() + 100);
		Check(Math.Abs(trashBin.instance.hitpoints - hitpoints2) < 0.001, "Covered Icenut must not keep damaging TrashBin through periodic events.");
		Check((double)Math.Abs(periodicAreaEventComponent.timer - timer) < 0.001, "Covered Icenut's periodic timer must pause instead of accumulating hidden attacks.");
		trashBin.DestroySet();
		Check(!icenut.characterDisabled && icenut.componentAlive && !icenut.invisible && icenut.instance.canBeCollection && !icenut.skipDestroySet, "Destroying the real TrashBin must restore Icenut's component lifecycle.");
		double hitpoints3 = trashBin.instance.hitpoints;
		periodicAreaEventComponent.EventExecute();
		double num2 = hitpoints3 - trashBin.instance.hitpoints;
		Check(Math.Abs(num2 - 20.0) < 0.001, $"Icenut contact damage must resume after TrashBin release; dealt {num2}.");
	}

	private static T Instantiate<T>(string path) where T : Node
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	private static void PlaceCharacter(TowerDefenseCharacter character)
	{
		character.Position = GridBegin + new Vector2((float)(TestGrid.X - 1) * GridSize.X, (float)(TestGrid.Y - 1) * GridSize.Y);
		character.gridPos = TestGrid;
		character.inGame = false;
	}

	private async Task WaitFrames(int count)
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
			GD.PushError("[BugOverviewIcenutTrashBinDisableRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyDisableLifecycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "icenut", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "trashBin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlaceCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.VerifyDisableLifecycle && args.Count == 2)
		{
			VerifyDisableLifecycle(VariantUtils.ConvertTo<TowerDefensePlantIcenut>(in args[0]), VariantUtils.ConvertTo<TowerDefenseTrashBin>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlaceCharacter && args.Count == 1)
		{
			PlaceCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
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
		if (method == MethodName.PlaceCharacter && args.Count == 1)
		{
			PlaceCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.VerifyDisableLifecycle)
		{
			return true;
		}
		if (method == MethodName.PlaceCharacter)
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
