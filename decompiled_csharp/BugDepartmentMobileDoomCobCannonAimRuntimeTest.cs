using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentMobileDoomCobCannonAimRuntimeTest.cs")]
public class BugDepartmentMobileDoomCobCannonAimRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName DispatchTouch = "DispatchTouch";

		public static readonly StringName DispatchMouse = "DispatchMouse";

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

	private const string ScenePath = "res://Asset/Anime/Character/Plant/Gold/DoomCobCannon/Scene/TowerDefensePlantDoomCobCannon.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		MobileDoomCobCannonAimRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefensePlantDoomCobCannon cannonPlant = null;
		PackedScene scene = null;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00df;
				}
				control = new MobileDoomCobCannonAimRuntimeControlStub
				{
					Name = "MobileDoomCobCannonAimRuntimeControl",
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
				manager.gridBeginPos = new Vector2(260f, 75f);
				manager.gridSize = new Vector2(80f, 98f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
				{
					mapControl = mapControl,
					control = control,
					config = new TowerDefenseMapConfig(),
					groundRect = new Rect2(-1000f, -1000f, 3000f, 3000f)
				});
				control.featureDictionary[new StringName("Map")] = mapFeature;
				scene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Gold/DoomCobCannon/Scene/TowerDefensePlantDoomCobCannon.tscn", null, ResourceLoader.CacheMode.Ignore);
				cannonPlant = scene?.Instantiate<TowerDefensePlantDoomCobCannon>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(scene) && GodotObject.IsInstanceValid(cannonPlant), "The real Doom Cob Cannon scene and production script must instantiate.");
				if (!GodotObject.IsInstanceValid(cannonPlant))
				{
					goto end_IL_00df;
				}
				cannonPlant.editorPreviewMode = true;
				cannonPlant.inGame = true;
				cannonPlant.GlobalPosition = new Vector2(300f, 300f);
				control.characterNode.AddChild(cannonPlant, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(6);
				CannonComponent cannon = cannonPlant.componentManager?.GetRuntime<CannonComponent>();
				MousePressComponent mousePress = cannonPlant.componentManager?.GetRuntime<MousePressComponent>();
				Check(cannon != null && !cannon.IsReleased && cannon.Lifecycle == ComponentRuntimeLifecycle.Active && mousePress != null && !mousePress.IsReleased && mousePress.Lifecycle == ComponentRuntimeLifecycle.Active, "The real CannonComponent and MousePressComponent runtimes must be active.");
				if (cannon == null || cannon.IsReleased || mousePress == null || mousePress.IsReleased)
				{
					goto end_IL_00df;
				}
				bool flag = mousePress.TryGetClickShapeScreenCenter(out var firstScreenPosition);
				Check(((string.Equals(cannon.mode, "Marker", StringComparison.OrdinalIgnoreCase) && mousePress.ToggleMode) & flag) && GodotObject.IsInstanceValid(mousePress.ClickShape) && GodotObject.IsInstanceValid(mousePress.ToggleTarget), "The real Doom Cob Cannon must use the two-stage marker aiming definition.");
				cannon.canFire = true;
				mousePress.SetAlive(alive: true);
				await WaitFrames(2);
				int fireCount = 0;
				cannon.OnFire += () =>
				{
					fireCount++;
				};
				Vector2 vector = new Vector2(620f, 360f);
				Vector2 screenPosition = cannonPlant.GetCanvasTransform() * vector;
				Check(manager.GetGroundRect().HasPoint(vector), "The confirmation touch must resolve inside the production ground target rect.");
				DispatchMouse(cannonPlant.componentManager, firstScreenPosition, pressed: true);
				Check(mousePress.IsPressed && mousePress.ToggleTarget.Visible && fireCount == 0 && cannon.canFire, "A mouse-first Android compatibility event may start aiming but must not fire.");
				DispatchTouch(cannonPlant.componentManager, 0, firstScreenPosition, pressed: true);
				Check(mousePress.IsPressed && mousePress.ToggleTarget.Visible && fireCount == 0 && cannon.canFire, "The matching first touch-down must be deduplicated instead of becoming confirmation.");
				DispatchTouch(cannonPlant.componentManager, 0, firstScreenPosition, pressed: false);
				DispatchMouse(cannonPlant.componentManager, firstScreenPosition, pressed: false);
				Check(mousePress.IsPressed && mousePress.ToggleTarget.Visible && fireCount == 0 && cannon.canFire, "Releasing the first mobile touch must preserve aiming without firing.");
				DispatchMouse(cannonPlant.componentManager, screenPosition, pressed: true);
				Check(mousePress.IsPressed && mousePress.ToggleTarget.Visible && fireCount == 0 && cannon.canFire, "After touch input is observed, its compatibility mouse copy must be ignored.");
				DispatchTouch(cannonPlant.componentManager, 1, screenPosition, pressed: true);
				Check(mousePress.IsPressed && mousePress.ToggleTarget.Visible && fireCount == 0 && cannon.canFire, "The confirmation touch-down must arm the selected target without firing early.");
				DispatchTouch(cannonPlant.componentManager, 1, screenPosition, pressed: false);
				DispatchMouse(cannonPlant.componentManager, screenPosition, pressed: false);
				Check(!mousePress.IsPressed && !mousePress.ToggleTarget.Visible && fireCount == 1 && !cannon.canFire, "Only the matching confirmation release may commit one cannon shot.");
				Check(cannon.targetPos.IsEqualApprox(vector), $"The committed cannon target must be the confirmation position; expected={vector}, actual={cannon.targetPos}.");
				goto end_IL_00cd;
				end_IL_00df:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentMobileDoomCobCannonAimRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00cd;
			}
			return;
			end_IL_00cd:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(cannonPlant) && !cannonPlant.IsQueuedForDeletion())
			{
				cannonPlant.QueueFree();
			}
			await WaitFrames(6);
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
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			await WaitFrames(6);
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			scene?.Dispose();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag2 = _failures == 0 && _checks == 12;
		GD.Print($"MOBILE_DOOM_COB_CANNON_AIM_RESULT version=2 passed={flag2} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag2) ? 2 : 0);
	}

	private static void DispatchTouch(ComponentManager manager, int index, Vector2 screenPosition, bool pressed)
	{
		InputEventScreenTouch inputEventScreenTouch = new InputEventScreenTouch
		{
			Index = index,
			Position = screenPosition,
			Pressed = pressed
		};
		manager._Input(inputEventScreenTouch);
		inputEventScreenTouch.Dispose();
	}

	private static void DispatchMouse(ComponentManager manager, Vector2 screenPosition, bool pressed)
	{
		InputEventMouseButton inputEventMouseButton = new InputEventMouseButton
		{
			ButtonIndex = MouseButton.Left,
			Position = screenPosition,
			Pressed = pressed
		};
		manager._Input(inputEventMouseButton);
		inputEventMouseButton.Dispose();
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
			GD.PushError("[BugDepartmentMobileDoomCobCannonAimRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DispatchTouch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "screenPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DispatchMouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "screenPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.DispatchTouch && args.Count == 4)
		{
			DispatchTouch(VariantUtils.ConvertTo<ComponentManager>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchMouse && args.Count == 3)
		{
			DispatchMouse(VariantUtils.ConvertTo<ComponentManager>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
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
		if (method == MethodName.DispatchTouch && args.Count == 4)
		{
			DispatchTouch(VariantUtils.ConvertTo<ComponentManager>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchMouse && args.Count == 3)
		{
			DispatchMouse(VariantUtils.ConvertTo<ComponentManager>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
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
		if (method == MethodName.DispatchTouch)
		{
			return true;
		}
		if (method == MethodName.DispatchMouse)
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
