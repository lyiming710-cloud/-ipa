using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewChomperGargantuarActionContinuityRuntimeTest.cs")]
public class BugOverviewChomperGargantuarActionContinuityRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName IsState = "IsState";

		public static readonly StringName GetChomperAttackTarget = "GetChomperAttackTarget";

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

	private const string ChomperScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Chomper/Scene/TowerDefensePlantChomper.tscn";

	private const string GargantuarScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/Base/TowerDefenseZombieGargantuar.tscn";

	private const double ExpectedBiteDamage = 100.0;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BugOverviewChomperGargantuarActionContinuityControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefensePlantChomper chomper = null;
		TowerDefenseZombieGargantuar gargantuar = null;
		try
		{
			_ = 6;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00ec;
				}
				control = new BugOverviewChomperGargantuarActionContinuityControlStub
				{
					Name = "ChomperGargantuarActionContinuityControl",
					isGameRunning = false,
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
				manager.gridBeginPos = new Vector2(0f, 100f);
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				chomper = LoadCharacter<TowerDefensePlantChomper>("res://Asset/Anime/Character/Plant/Chapter0/Chomper/Scene/TowerDefensePlantChomper.tscn");
				gargantuar = LoadCharacter<TowerDefenseZombieGargantuar>("res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/Base/TowerDefenseZombieGargantuar.tscn");
				Check(GodotObject.IsInstanceValid(chomper) && chomper.config?.name == "PlantChomper" && GodotObject.IsInstanceValid(gargantuar) && gargantuar.config?.name == "ZombieGargantuar", "The fixture must use the real Chomper and Gargantuar scenes.");
				if (!GodotObject.IsInstanceValid(chomper) || !GodotObject.IsInstanceValid(gargantuar))
				{
					goto end_IL_00ec;
				}
				chomper.editorPreviewMode = false;
				gargantuar.editorPreviewMode = false;
				chomper.inGame = true;
				gargantuar.inGame = true;
				chomper.gridPos = new Vector2I(2, 2);
				gargantuar.gridPos = new Vector2I(3, 2);
				chomper.GlobalPosition = new Vector2(200f, 200f);
				gargantuar.GlobalPosition = new Vector2(280f, 200f);
				control.characterNode.AddChild(chomper, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(gargantuar, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				TowerDefenseCellInstance towerDefenseCellInstance = (chomper.cell = TowerDefenseManager.GetMapCell(chomper.gridPos));
				if (GodotObject.IsInstanceValid(towerDefenseCellInstance) && !towerDefenseCellInstance.characterList.Contains(chomper))
				{
					towerDefenseCellInstance.characterList.Add(chomper);
				}
				Check(GodotObject.IsInstanceValid(towerDefenseCellInstance) && towerDefenseCellInstance.characterList.Contains(chomper) && chomper.cell == towerDefenseCellInstance, "The live Chomper/Gargantuar probe must use a real target cell.");
				chomper.instance.hitpointsBase = 100000.0;
				chomper.instance.hitpointsSave = 100000.0;
				chomper.instance.hitpoints = 100000.0;
				chomper.timeScale = 4.0;
				gargantuar.timeScale = 4.0;
				double gargantuarHitpointsBeforeLiveProbe = gargantuar.GetCurrentHitPoint();
				double chomperHitpointsBeforeLiveProbe = chomper.GetCurrentHitPoint();
				bool chomperTargetedGargantuar = false;
				bool gargantuarTargetedChomper = false;
				bool chomperBiteObserved = false;
				bool smashStarted = false;
				bool smashRestartedBeforeImpact = false;
				bool smashImpactObserved = false;
				int previousSmashFrame = -1;
				chomper.ActivateGameplayProcessing();
				chomper.Idle();
				gargantuar.Walk();
				await WaitFrames(2);
				control.isGameRunning = true;
				for (int frame = 0; frame < 360; frame++)
				{
					await WaitFrames(1);
					chomperTargetedGargantuar |= GetChomperAttackTarget(chomper) == gargantuar;
					gargantuarTargetedChomper |= gargantuar.attackComponent?.target == chomper;
					chomperBiteObserved |= gargantuar.GetCurrentHitPoint() < gargantuarHitpointsBeforeLiveProbe;
					if (gargantuar.CurrentStateHandle?.StableId == "zombie.attack" && gargantuar.sprite?.clip == gargantuar.attackAnimeClip)
					{
						smashStarted = true;
						int frameIndex = gargantuar.sprite.frameIndex;
						if (previousSmashFrame >= 0 && frameIndex + 1 < previousSmashFrame)
						{
							smashRestartedBeforeImpact = true;
						}
						previousSmashFrame = frameIndex;
					}
					else if (smashStarted)
					{
						smashRestartedBeforeImpact = true;
					}
					if (chomper.GetCurrentHitPoint() < chomperHitpointsBeforeLiveProbe)
					{
						smashImpactObserved = true;
						break;
					}
				}
				Check(chomperTargetedGargantuar & gargantuarTargetedChomper, "The live probe must let the real Chomper and Gargantuar naturally acquire each other.");
				Check(chomperBiteObserved, "The live probe must observe the real Chomper's authored finite bite damage.");
				Check(smashImpactObserved && !smashRestartedBeforeImpact, $"Chomper bites must not restart the Gargantuar's committed Smash before impact; impact={smashImpactObserved}, restarted={smashRestartedBeforeImpact}, state={gargantuar.CurrentStateHandle?.StableId}, clip={gargantuar.sprite?.clip}, frame={gargantuar.sprite?.frameIndex}.");
				control.isGameRunning = false;
				chomper.ProcessMode = ProcessModeEnum.Disabled;
				gargantuar.ProcessMode = ProcessModeEnum.Disabled;
				chomper.QueueFree();
				gargantuar.QueueFree();
				await WaitFrames(2);
				chomper = LoadCharacter<TowerDefensePlantChomper>("res://Asset/Anime/Character/Plant/Chapter0/Chomper/Scene/TowerDefensePlantChomper.tscn");
				gargantuar = LoadCharacter<TowerDefenseZombieGargantuar>("res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/Base/TowerDefenseZombieGargantuar.tscn");
				chomper.editorPreviewMode = false;
				gargantuar.editorPreviewMode = false;
				chomper.inGame = true;
				gargantuar.inGame = true;
				chomper.gridPos = new Vector2I(2, 2);
				gargantuar.gridPos = new Vector2I(3, 2);
				chomper.GlobalPosition = new Vector2(200f, 200f);
				gargantuar.GlobalPosition = new Vector2(280f, 200f);
				control.characterNode.AddChild(chomper, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(gargantuar, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				chomper.ProcessMode = ProcessModeEnum.Disabled;
				gargantuar.ProcessMode = ProcessModeEnum.Disabled;
				control.isGameRunning = true;
				ChomperComponent component = chomper.componentManager?.GetRuntime<ChomperComponent>();
				BugOverviewChomperGargantuarActionContinuityRuntimeTest bugOverviewChomperGargantuarActionContinuityRuntimeTest = this;
				int condition;
				if (component != null && !component.IsReleased)
				{
					AttackComponent attackComponent = gargantuar.attackComponent;
					condition = ((attackComponent != null && !attackComponent.IsReleased) ? 1 : 0);
				}
				else
				{
					condition = 0;
				}
				bugOverviewChomperGargantuarActionContinuityRuntimeTest.Check((byte)condition != 0, "The real Chomper bite and Gargantuar attack runtimes must be active.");
				BugOverviewChomperGargantuarActionContinuityRuntimeTest bugOverviewChomperGargantuarActionContinuityRuntimeTest2 = this;
				TowerDefenseCharacterInstance instance = gargantuar.instance;
				bugOverviewChomperGargantuarActionContinuityRuntimeTest2.Check(instance != null && instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.HUGE && Math.Abs(gargantuar.instance.biteHurt - 100.0) < 0.001, $"Gargantuar must retain its finite bite response; physique={gargantuar.instance?.zombiePhysique}, biteHurt={gargantuar.instance?.biteHurt}.");
				Check(chomper.CanTarget(gargantuar) && chomper.CanCollision(gargantuar.instance.maskFlags), $"The opposing real Gargantuar must be reachable by the real Chomper bite; chomperCamp={chomper.camp}, gargantuarCamp={gargantuar.camp}, chomperCollision={chomper.instance.collisionFlags}, gargantuarMask={gargantuar.instance.maskFlags}, die={gargantuar.die}/{gargantuar.nearDie}.");
				if ((component?.IsReleased ?? true) || (gargantuar.attackComponent?.IsReleased ?? true))
				{
					goto end_IL_00ec;
				}
				gargantuar.attackComponent.target = chomper;
				gargantuar.Attack();
				await WaitFrames(1);
				Check(IsState(gargantuar, "zombie.attack") && gargantuar.sprite.clip == gargantuar.attackAnimeClip, $"The real Gargantuar must begin its Smash action; state={gargantuar.CurrentStateHandle?.StableId}, clip={gargantuar.sprite?.clip}.");
				gargantuar.instance.hitpoints = 1550.0;
				double currentHitPoint = gargantuar.GetCurrentHitPoint();
				component.BitCharacter(gargantuar);
				Check(Math.Abs(currentHitPoint - gargantuar.GetCurrentHitPoint() - 100.0) < 0.001, "A non-lethal Chomper bite must apply the Gargantuar's authored finite damage.");
				Check(IsState(gargantuar, "zombie.attack") && gargantuar.sprite.clip == gargantuar.attackAnimeClip, $"A non-lethal bite must not interrupt or restart the committed Smash action; state={gargantuar.CurrentStateHandle?.StableId}, clip={gargantuar.sprite?.clip}.");
				Check(!gargantuar.isChomp && !gargantuar.isDestroy && !component.eatCharacter, "A finite Smash-window bite must not mark the Gargantuar as swallowed or destroyed.");
				gargantuar.impThrowFlag = true;
				gargantuar.Fire();
				await WaitFrames(1);
				Check(IsState(gargantuar, "gargantuar.fire") && gargantuar.sprite.clip == gargantuar.fireAnimeClip, $"The real Gargantuar must begin its Imp throw action; state={gargantuar.CurrentStateHandle?.StableId}, clip={gargantuar.sprite?.clip}.");
				double currentHitPoint2 = gargantuar.GetCurrentHitPoint();
				component.BitCharacter(gargantuar);
				Check(Math.Abs(currentHitPoint2 - gargantuar.GetCurrentHitPoint() - 100.0) < 0.001, "A Chomper bite during Imp throw must still apply only finite damage.");
				Check(IsState(gargantuar, "gargantuar.fire") && gargantuar.sprite.clip == gargantuar.fireAnimeClip, $"A non-lethal bite must not interrupt or restart the Imp throw action; state={gargantuar.CurrentStateHandle?.StableId}, clip={gargantuar.sprite?.clip}.");
				Check(gargantuar.impThrowFlag && !gargantuar.isChomp && !gargantuar.isDestroy && !component.eatCharacter, "The bite must not consume the pending Imp throw or enter Chomper-owned destruction.");
				gargantuar.instance.hitpoints = 100.0;
				Check(!gargantuar.die && !gargantuar.nearDie, "The lethal-boundary probe must start with a live Gargantuar.");
				component.BitCharacter(gargantuar);
				Check(gargantuar.die && gargantuar.nearDie && IsState(gargantuar, "zombie.die") && gargantuar.sprite.clip == gargantuar.dieAnimeClip, $"A lethal finite bite must enter the Gargantuar's own death action; state={gargantuar.CurrentStateHandle?.StableId}, clip={gargantuar.sprite?.clip}.");
				Check(!gargantuar.isChomp && !gargantuar.isDestroy && !component.eatCharacter, "A lethal finite bite must not overwrite Gargantuar death with Chomper swallowing/destruction.");
				goto end_IL_00c5;
				end_IL_00ec:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewChomperGargantuarActionContinuityRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00c5;
			}
			return;
			end_IL_00c5:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(chomper) && !chomper.IsQueuedForDeletion())
			{
				chomper.QueueFree();
			}
			if (GodotObject.IsInstanceValid(gargantuar) && !gargantuar.IsQueuedForDeletion())
			{
				gargantuar.QueueFree();
			}
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
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 20;
		GD.Print($"CHOMPER_GARGANTUAR_ACTION_CONTINUITY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static bool IsState(TowerDefenseCharacter character, string stateId)
	{
		return character.CurrentStateHandle?.StableId == stateId;
	}

	private static TowerDefenseCharacter GetChomperAttackTarget(TowerDefensePlantChomper chomper)
	{
		return chomper?.componentManager?.GetRuntime<ChomperComponent>()?.attackComponent?.target;
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = new TowerDefenseMapConfig()
		});
		towerDefenseBattleFeatureMap.plantGrid.Resize(gridNum.X + 1);
		for (int i = 0; i <= gridNum.X; i++)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			array.Resize(gridNum.Y + 1);
			for (int j = 1; j <= gridNum.Y; j++)
			{
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
				{
					gridPos = new Vector2I(i, j)
				};
				towerDefenseCellInstance.Init(new TowerDefenseCellConfig());
				array[j] = towerDefenseCellInstance;
			}
			towerDefenseBattleFeatureMap.plantGrid[i] = array;
		}
		towerDefenseBattleFeatureMap.iceCapList.Resize(gridNum.Y + 1);
		return towerDefenseBattleFeatureMap;
	}

	private static T LoadCharacter<T>(string path) where T : TowerDefenseCharacter
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
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
			_failures++;
			GD.PushError("[BugOverviewChomperGargantuarActionContinuityRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsState, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "stateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetChomperAttackTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "chomper", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
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
		if (method == MethodName.IsState && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsState(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetChomperAttackTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetChomperAttackTarget(VariantUtils.ConvertTo<TowerDefensePlantChomper>(in args[0])));
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
		if (method == MethodName.IsState && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsState(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetChomperAttackTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetChomperAttackTarget(VariantUtils.ConvertTo<TowerDefensePlantChomper>(in args[0])));
			return true;
		}
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
		if (method == MethodName.IsState)
		{
			return true;
		}
		if (method == MethodName.GetChomperAttackTarget)
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
