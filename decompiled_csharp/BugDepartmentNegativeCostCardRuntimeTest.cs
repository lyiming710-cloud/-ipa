using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentNegativeCostCardRuntimeTest.cs")]
public class BugDepartmentNegativeCostCardRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RegisterRealVaseScene = "RegisterRealVaseScene";

		public static readonly StringName RestoreRealVaseScene = "RestoreRealVaseScene";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _previousVaseScene = "_previousVaseScene";

		public static readonly StringName _vaseSceneWasMissing = "_vaseSceneWasMissing";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string VasePacketPath = "res://Asset/Anime/Character/Vase/Zombie/Packet/VaseZombie.tres";

	private const string VaseScenePath = "res://Asset/Anime/Character/Vase/Zombie/Scene/TowerDefenseVaseZombie.tscn";

	private static readonly Vector2I TestGrid = new Vector2I(4, 3);

	private int _checks;

	private int _failures;

	private Resource _previousVaseScene;

	private bool _vaseSceneWasMissing;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		NegativeCostCardRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance), "TowerDefenseManager and ResourceManager autoloads must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_00e8;
				}
				RegisterRealVaseScene();
				control = new NegativeCostCardRuntimeControlStub
				{
					Name = "NegativeCostCardRuntimeControl",
					isGameRunning = true,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig
					{
						packetBankMethod = TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE,
						packetColdDownUse = false
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
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				TowerDefenseBattleFeatureSun sunFeature = new TowerDefenseBattleFeatureSun
				{
					control = control
				};
				sunFeature.Init(new Dictionary { ["Begin"] = 0L });
				control.featureDictionary[new StringName("Sun")] = sunFeature;
				TowerDefensePacketConfig vasePacket = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Vase/Zombie/Packet/VaseZombie.tres", null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
				Check(GodotObject.IsInstanceValid(vasePacket) && vasePacket.GetCost() == -100, $"The regression must use the authored -100 Zombie Vase packet; cost={vasePacket?.GetCost()}.");
				if (!GodotObject.IsInstanceValid(vasePacket))
				{
					goto end_IL_00e8;
				}
				TowerDefenseInGamePacketShow slot = TowerDefenseManager.CreatePacketShow();
				control.characterNode.AddChild(slot, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(2);
				slot.Init(vasePacket);
				slot.useCost = true;
				slot.start = true;
				slot.coldDownOpen = false;
				slot.alive = true;
				Check(slot.TryBindSunAccount(EconomyAccountId.Local), "The real Zombie Vase card must bind the local Sun account.");
				Check(slot.itemCost == -100 && slot.itemCostLabel?.Text == "-100", $"The real card must preserve and display its authored negative cost; item={slot.itemCost}, label={slot.itemCostLabel?.Text}.");
				await WaitFrames(3);
				Check(manager.CanAffordSun(EconomyAccountId.Local, -100L), "A registered account must be able to afford a negative card cost.");
				Check(slot.alive, "The real negative-cost card must remain selectable after its live affordability update.");
				slot.alive = true;
				bool flag = slot.TryBeginPendingUse(out var spendReceipt);
				Check(flag && spendReceipt != null && spendReceipt.IsActive && spendReceipt.Amount == -100, "A pending negative-cost placement must create a signed, rollback-capable receipt.");
				Check(sunFeature.GetSun(EconomyAccountId.Local) == 100, $"Beginning the signed cost must provision the 100-Sun reward atomically; balance={sunFeature.GetSun(EconomyAccountId.Local)}.");
				Check(spendReceipt != null && spendReceipt.TryRollback() && sunFeature.GetSun(EconomyAccountId.Local) == 0, "Cancelling the pending card must reverse the provisional reward exactly once.");
				Check(spendReceipt != null && !spendReceipt.TryRollback(), "A rolled-back signed receipt must remain one-shot.");
				slot.alive = true;
				slot.coldDownOpen = false;
				TowerDefenseCharacter instance = slot.Plant(new Vector2I(-1, -1));
				Check(!GodotObject.IsInstanceValid(instance), "The real packet must reject an invalid map cell.");
				Check(sunFeature.GetSun(EconomyAccountId.Local) == 0, "A failed real placement must roll back the provisional negative-cost reward.");
				slot.alive = true;
				slot.coldDownOpen = false;
				TowerDefenseCharacter spawned = slot.Plant(TestGrid);
				await WaitFrames(8);
				Check(spawned is TowerDefenseVaseZombie && GodotObject.IsInstanceValid(spawned), "A successful card use must create the real Zombie Vase scene.");
				Check(spawned?.gridPos == TestGrid && spawned?.cell?.gridPos == TestGrid, "The real Zombie Vase must finish binding to the requested map cell.");
				Check(sunFeature.GetSun(EconomyAccountId.Local) == 100, $"Only the successful real placement must commit the 100-Sun reward; balance={sunFeature.GetSun(EconomyAccountId.Local)}.");
				goto end_IL_00d1;
				end_IL_00e8:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentNegativeCostCardRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00d1;
			}
			return;
			end_IL_00d1:;
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
			await WaitFrames(4);
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
			RestoreRealVaseScene();
			await WaitFrames(4);
		}
		bool flag2 = _failures == 0 && _checks >= 15;
		GD.Print($"NEGATIVE_COST_CARD_RESULT passed={flag2} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag2) ? 2 : 0);
	}

	private void RegisterRealVaseScene()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.TOWERDEFENSE_CHARCATERS.TryGetValue("VaseZombie", out var value))
		{
			_previousVaseScene = value;
		}
		else
		{
			_vaseSceneWasMissing = true;
		}
		instance.TOWERDEFENSE_CHARCATERS["VaseZombie"] = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Vase/Zombie/Scene/TowerDefenseVaseZombie.tscn", null, ResourceLoader.CacheMode.Ignore);
	}

	private void RestoreRealVaseScene()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			if (_vaseSceneWasMissing)
			{
				instance.TOWERDEFENSE_CHARCATERS.Remove("VaseZombie");
			}
			else
			{
				instance.TOWERDEFENSE_CHARCATERS["VaseZombie"] = _previousVaseScene;
			}
		}
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			gridNum = gridNum,
			gridBeginPos = Vector2.Zero,
			gridSize = new Vector2(100f, 76f),
			plantOffset = 50.0
		};
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig
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
		towerDefenseBattleFeatureMap.lineUse.Resize(gridNum.Y + 1);
		for (int k = 1; k <= gridNum.Y; k++)
		{
			towerDefenseBattleFeatureMap.lineUse[k] = true;
		}
		return towerDefenseBattleFeatureMap;
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
			GD.PushError("[BugDepartmentNegativeCostCardRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterRealVaseScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreRealVaseScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.RegisterRealVaseScene && args.Count == 0)
		{
			RegisterRealVaseScene();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreRealVaseScene && args.Count == 0)
		{
			RestoreRealVaseScene();
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
		if (method == MethodName.RegisterRealVaseScene)
		{
			return true;
		}
		if (method == MethodName.RestoreRealVaseScene)
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
		if (name == PropertyName._previousVaseScene)
		{
			_previousVaseScene = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._vaseSceneWasMissing)
		{
			_vaseSceneWasMissing = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._previousVaseScene)
		{
			value = VariantUtils.CreateFrom(in _previousVaseScene);
			return true;
		}
		if (name == PropertyName._vaseSceneWasMissing)
		{
			value = VariantUtils.CreateFrom(in _vaseSceneWasMissing);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousVaseScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._vaseSceneWasMissing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._previousVaseScene, Variant.From(in _previousVaseScene));
		info.AddProperty(PropertyName._vaseSceneWasMissing, Variant.From(in _vaseSceneWasMissing));
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
		if (info.TryGetProperty(PropertyName._previousVaseScene, out var value3))
		{
			_previousVaseScene = value3.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._vaseSceneWasMissing, out var value4))
		{
			_vaseSceneWasMissing = value4.As<bool>();
		}
	}
}
