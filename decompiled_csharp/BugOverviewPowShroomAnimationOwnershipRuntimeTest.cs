using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewPowShroomAnimationOwnershipRuntimeTest.cs")]
public class BugOverviewPowShroomAnimationOwnershipRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private const string ScenePath = "res://Asset/Anime/Character/Plant/Chapter7/PowShroom/Scene/TowerDefensePlantPowShroom.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		PowShroomAnimationOwnershipRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefensePlantPowShroom plant = null;
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
				control = new PowShroomAnimationOwnershipRuntimeControlStub
				{
					Name = "PowShroomAnimationOwnershipRuntimeControl",
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
				scene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter7/PowShroom/Scene/TowerDefensePlantPowShroom.tscn", null, ResourceLoader.CacheMode.Ignore);
				plant = scene?.Instantiate<TowerDefensePlantPowShroom>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(scene) && GodotObject.IsInstanceValid(plant), "The real Pow Shroom scene and production script must instantiate.");
				if (!GodotObject.IsInstanceValid(plant))
				{
					goto end_IL_00df;
				}
				plant.editorPreviewMode = true;
				plant.inGame = true;
				plant.GlobalPosition = new Vector2(300f, 300f);
				control.characterNode.AddChild(plant, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(6);
				CannonComponent cannon = plant.componentManager?.GetRuntime<CannonComponent>("character.cannon");
				AttackComponent attack = plant.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
				Check(cannon != null && !cannon.IsReleased && attack != null && !attack.IsReleased && cannon.Lifecycle == ComponentRuntimeLifecycle.Active && attack.Lifecycle == ComponentRuntimeLifecycle.Active, "The real CannonComponent and AttackComponent runtimes must be active.");
				Check(GodotObject.IsInstanceValid(cannon?.sprite) && cannon?.sprite == attack?.sprite, "Pow Shroom must exercise the production shared animation sprite.");
				if (cannon == null || cannon.IsReleased || attack == null || attack.IsReleased || !GodotObject.IsInstanceValid(cannon.sprite))
				{
					goto end_IL_00df;
				}
				plant.WakeUp();
				plant.componentManager.GetRuntime<SleepComponent>()?.ReevaluateEnvironmentState();
				await WaitFrames(6);
				Check(!plant.instance.sleep && plant.componentAlive, "The production wake-up path must restore both sleep and component activity.");
				attack.checkAll = true;
				for (int i = 0; i < 2; i++)
				{
					Check(cannon.SendStateEvent("ToRest"), $"Cycle {i}: Cannon must enter Rest.");
					Check(attack.SendStateEvent("ToAttack"), $"Cycle {i}: shared attack must enter Attack.");
					Check(attack.StateMachine?.CurrentStateHandle?.StableId == "attack.attack" && cannon.sprite.clip == attack.attackAnimeClips, $"Cycle {i}: Attack must own the Fire clip.");
					attack.AttackProcessing(1.0 / 60.0);
					double timeScale = cannon.sprite.timeScale;
					cannon.RestProcessing(1.0 / 60.0);
					Check(Mathf.IsEqualApprox((float)cannon.sprite.timeScale, (float)timeScale) && timeScale > cannon.restAnimeTimeScale, $"Cycle {i}: Rest processing must not slow the active attack.");
					cannon.Timeout("Rest");
					cannon.ChargeProcessing(1.0 / 60.0);
					Check(cannon.StateMachine?.CurrentStateHandle?.StableId == "cannon.charge" && cannon.canFire && cannon.sprite.clip == attack.attackAnimeClips, $"Cycle {i}: becoming clickable must not replace the active Fire clip.");
					attack.AnimeCompleted(attack.attackAnimeClips);
					cannon.ChargeProcessing(1.0 / 60.0);
					Check(attack.StateMachine?.CurrentStateHandle?.StableId == "attack.idle" && cannon.sprite.clip == cannon.chargeAnimeClips && Mathf.IsEqualApprox((float)cannon.sprite.timeScale, (float)(plant.timeScale * cannon.chargeAnimeTimeScale)), $"Cycle {i}: attack exit must restore the charged cannon animation.");
				}
				cannon.FireAt(new Vector2(620f, 360f));
				Check(!cannon.canFire && cannon.StateMachine?.CurrentStateHandle?.StableId == "cannon.fire" && cannon.sprite.clip == cannon.fireAnimeClips, "The restored charged state must still accept a click and play Pow.");
				goto end_IL_00cd;
				end_IL_00df:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewPowShroomAnimationOwnershipRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00cd;
			}
			return;
			end_IL_00cd:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(plant) && !plant.IsQueuedForDeletion())
			{
				plant.QueueFree();
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
		bool flag = _failures == 0 && _checks == 18;
		GD.Print($"POW_SHROOM_ANIMATION_OWNERSHIP_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
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
			GD.PushError("[BugOverviewPowShroomAnimationOwnershipRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
