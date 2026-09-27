using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewSunTransferDeathrattleRuntimeTest.cs")]
public class BugOverviewSunTransferDeathrattleRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CountLivingSunTransfers = "CountLivingSunTransfers";

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

	private const string PlantScenePath = "res://Asset/Anime/Character/Plant/Chapter9/SunTransfer/Scene/TowerDefensePlantSunTransfer.tscn";

	private static readonly Vector2I TestGrid = new Vector2I(4, 3);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		ObjectManager objectManager = ObjectManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		SunTransferDeathrattleRuntimeControlStub control = null;
		TowerDefenseBattleFeatureSun sunFeature = null;
		TowerDefensePlantSunTransfer plant = null;
		TowerDefensePlantSunTransfer restoredPlant = null;
		Vector2 deathPosition;
		bool timeoutObserved;
		bool deathrattleEntered;
		int timeoutBrainSunCount;
		long timeoutBrainSunAmount;
		bool timeoutBrainSunPositionMatches;
		try
		{
			_ = 5;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(objectManager), "ObjectManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(objectManager))
				{
					throw new InvalidOperationException("Required runtime autoloads are unavailable.");
				}
				objectManager.Clear();
				control = new SunTransferDeathrattleRuntimeControlStub
				{
					Name = "SunTransferDeathrattleRuntimeControl",
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
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				sunFeature = new TowerDefenseBattleFeatureSun
				{
					control = control
				};
				sunFeature.Init(new Dictionary { ["Begin"] = 0L });
				control.featureDictionary[new StringName("Sun")] = sunFeature;
				PackedScene packed = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter9/SunTransfer/Scene/TowerDefensePlantSunTransfer.tscn", null, ResourceLoader.CacheMode.Ignore);
				plant = packed?.Instantiate<TowerDefensePlantSunTransfer>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(plant) && plant.config?.name == "PlantSunTransfer", "The fixture must instantiate the real authored Sun Transfer scene.");
				if (!GodotObject.IsInstanceValid(plant))
				{
					throw new InvalidOperationException("The real Sun Transfer did not instantiate.");
				}
				Check(plant.TryAssignEconomyOwner(EconomyAccountId.Local), "The real Sun Transfer must bind the local economy account.");
				plant.inGame = true;
				plant.editorPreviewMode = false;
				plant.gridPos = TestGrid;
				plant.GlobalPosition = new Vector2(400f, 260f);
				control.characterNode.AddChild(plant, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(6);
				Check(TowerDefenseManager.HasGameplayAuthority, "The isolated runtime fixture must own gameplay authority.");
				CharacterTimerComponent lifetimeTimer = plant.componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
				Check(lifetimeTimer != null && !lifetimeTimer.IsReleased, "The real Sun Transfer must resolve its character.timer lifetime runtime.");
				Check(lifetimeTimer != null && lifetimeTimer.timerDictionary.ContainsKey("Destroy") && Mathf.IsEqualApprox((float)lifetimeTimer.timerDictionary["Destroy"].AsDouble(), 50f), "The authored Sun Transfer lifetime must be exactly 50 seconds.");
				Check(lifetimeTimer != null && lifetimeTimer.IsRunning("Destroy") && lifetimeTimer.timerWaitTime.TryGetValue("Destroy", out var value) && Mathf.IsEqualApprox((float)value, 50f), "Planting Sun Transfer must start its 50-second lifetime timer.");
				TowerDefenseSunBase towerDefenseSunBase = FindSingleDrop(ObjectManagerConfig.OBJECT.SUN, out var count);
				TowerDefenseSunBase towerDefenseSunBase2 = FindSingleDrop(ObjectManagerConfig.OBJECT.SUN_BRAIN, out var count2);
				Check(count == 1 && towerDefenseSunBase != null && towerDefenseSunBase.sunNum == 300, "Planting the real Sun Transfer must create exactly one 300 Sun drop.");
				Check(count2 == 0 && towerDefenseSunBase2 == null, "The deathrattle Brain Sun must not exist before the lifetime expires.");
				restoredPlant = packed.Instantiate<TowerDefensePlantSunTransfer>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(restoredPlant), "The progress-restore fixture must instantiate a second real Sun Transfer.");
				if (!GodotObject.IsInstanceValid(restoredPlant))
				{
					throw new InvalidOperationException("The progress-restore Sun Transfer did not instantiate.");
				}
				if (!restoredPlant.TryAssignEconomyOwner(EconomyAccountId.Local))
				{
					throw new InvalidOperationException("The progress-restore Sun Transfer could not bind the local economy account.");
				}
				restoredPlant.inGame = true;
				restoredPlant.editorPreviewMode = false;
				restoredPlant.gridPos = TestGrid;
				restoredPlant.GlobalPosition = new Vector2(400f, 260f);
				restoredPlant.PrepareForProgressRestore();
				control.characterNode.AddChild(restoredPlant, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				restoredPlant.ImportVariantSave(new Dictionary
				{
					["over"] = false,
					["produced"] = true
				});
				restoredPlant.FinalizeProgressRestore();
				await WaitFrames(4);
				FindSingleDrop(ObjectManagerConfig.OBJECT.SUN, out var count3);
				Check(count3 == 1, "Progress restore must not replay Sun Transfer's one-time 300 Sun planting production.");
				restoredPlant.QueueFree();
				restoredPlant = null;
				await WaitFrames(4);
				deathPosition = plant.GetLogicalGlobalPosition(plant.spriteGroup);
				timeoutObserved = false;
				deathrattleEntered = false;
				timeoutBrainSunCount = -1;
				timeoutBrainSunAmount = -1L;
				timeoutBrainSunPositionMatches = false;
				lifetimeTimer.OnTimeout += ObserveLifetimeTimeout;
				lifetimeTimer.Run("Destroy", 0.0);
				await WaitFrames(4);
				GD.Print($"SUN_TRANSFER_LIFETIME_DEATHRATTLE timeout={timeoutObserved} entered={deathrattleEntered} brain_count={timeoutBrainSunCount} brain_amount={timeoutBrainSunAmount}");
				Check(timeoutObserved, "The accelerated 50-second lifetime timer must emit its Destroy timeout.");
				Check(deathrattleEntered, "Lifetime expiry must enter the Sun Transfer DestroySet deathrattle.");
				Check(timeoutBrainSunCount == 1 && timeoutBrainSunAmount == 300, "Lifetime expiry must create exactly one 300 Brain Sun deathrattle drop.");
				Check(timeoutBrainSunPositionMatches, "The Brain Sun deathrattle must originate from the plant's logical position.");
				await WaitFrames(4);
				FindSingleDrop(ObjectManagerConfig.OBJECT.SUN, out var count4);
				Check(count4 == 1, "The deathrattle must not duplicate or replace the planting Sun drop.");
				Check(!GodotObject.IsInstanceValid(plant) || plant.isDestroy || plant.IsQueuedForDeletion(), "The destroyed Sun Transfer must use the standard destroy lifecycle.");
				Check(CountLivingSunTransfers(control.characterNode) == 0, "Sun Transfer death must not leave or respawn a hologram base.");
			}
			catch (Exception value2)
			{
				_failures++;
				GD.PushError($"[BugOverviewSunTransferDeathrattleRuntimeTest] Unexpected exception: {value2}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			sunFeature?.Destroy();
			if (GodotObject.IsInstanceValid(plant) && !plant.IsQueuedForDeletion())
			{
				if (plant.IsInsideTree())
				{
					plant.QueueFree();
				}
				else
				{
					plant.Free();
				}
			}
			if (GodotObject.IsInstanceValid(restoredPlant) && !restoredPlant.IsQueuedForDeletion())
			{
				if (restoredPlant.IsInsideTree())
				{
					restoredPlant.QueueFree();
				}
				else
				{
					restoredPlant.Free();
				}
			}
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			objectManager?.Clear();
			await WaitFrames(4);
		}
		bool flag = _failures == 0 && _checks == 19;
		GD.Print($"SUN_TRANSFER_DEATHRATTLE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
		void ObserveLifetimeTimeout(string timerName)
		{
			if (!(timerName != "Destroy"))
			{
				timeoutObserved = true;
				deathrattleEntered = plant.ExportVariantSave().GetValueOrDefault("over", false).AsBool();
				TowerDefenseSunBase towerDefenseSunBase3 = FindSingleDrop(ObjectManagerConfig.OBJECT.SUN_BRAIN, out timeoutBrainSunCount);
				timeoutBrainSunAmount = towerDefenseSunBase3?.sunNum ?? (-1);
				timeoutBrainSunPositionMatches = GodotObject.IsInstanceValid(towerDefenseSunBase3) && towerDefenseSunBase3.GlobalPosition.IsEqualApprox(deathPosition);
			}
		}
	}

	private TowerDefenseSunBase FindSingleDrop(ObjectManagerConfig.OBJECT poolKey, out int count)
	{
		TowerDefenseSunBase result = null;
		count = 0;
		foreach (Node item in GetTree().GetNodesInGroup("SunDropItem"))
		{
			if (item is TowerDefenseSunBase towerDefenseSunBase && GodotObject.IsInstanceValid(towerDefenseSunBase) && !towerDefenseSunBase.die && !towerDefenseSunBase.isCollect && towerDefenseSunBase.GetPoolKey() == poolKey)
			{
				result = towerDefenseSunBase;
				count++;
			}
		}
		if (count != 1)
		{
			return null;
		}
		return result;
	}

	private static int CountLivingSunTransfers(Node characterNode)
	{
		if (!GodotObject.IsInstanceValid(characterNode))
		{
			return 0;
		}
		int num = 0;
		foreach (Node child in characterNode.GetChildren())
		{
			if (child is TowerDefensePlantSunTransfer towerDefensePlantSunTransfer && GodotObject.IsInstanceValid(towerDefensePlantSunTransfer) && !towerDefensePlantSunTransfer.isDestroy && !towerDefensePlantSunTransfer.IsQueuedForDeletion())
			{
				num++;
			}
		}
		return num;
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
			GD.PushError("[BugOverviewSunTransferDeathrattleRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountLivingSunTransfers, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "characterNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.CountLivingSunTransfers && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountLivingSunTransfers(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.CountLivingSunTransfers && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountLivingSunTransfers(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.CountLivingSunTransfers)
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
