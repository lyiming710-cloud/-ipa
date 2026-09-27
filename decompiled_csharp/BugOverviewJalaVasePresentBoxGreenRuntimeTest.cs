using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewJalaVasePresentBoxGreenRuntimeTest.cs")]
public class BugOverviewJalaVasePresentBoxGreenRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindDroppedPacket = "FindDroppedPacket";

		public static readonly StringName FindGeneratedZombie = "FindGeneratedZombie";

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

	private static readonly Vector2I TestGrid = new Vector2I(2, 2);

	private static readonly Vector2I ZombieTestGrid = new Vector2I(4, 2);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		JalaVasePresentBoxGreenRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		try
		{
			_ = 4;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("Required runtime autoload is unavailable.");
				}
				ResourceManager.Instance.BeginLoad();
				for (int frame = 0; frame < 7200; frame++)
				{
					if (ResourceManager.Instance.CurrentGameplayResourceLoadState == GameplayResourceLoadState.Ready)
					{
						break;
					}
					if (ResourceManager.Instance.CurrentGameplayResourceLoadState == GameplayResourceLoadState.Failed)
					{
						break;
					}
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				}
				Check(ResourceManager.Instance.AreFullGameplayResourcesReady, $"Full gameplay resources must be ready before PresentBox generation: state={ResourceManager.Instance.CurrentGameplayResourceLoadState}, error={ResourceManager.Instance.FullGameplayResourceLoadError}");
				if (!ResourceManager.Instance.AreFullGameplayResourcesReady)
				{
					throw new InvalidOperationException("Full gameplay resources did not become ready.");
				}
				int lateLoadCountBefore = ResourceManager.Instance.LateCharacterResourceLoadCount;
				control = new JalaVasePresentBoxGreenRuntimeControlStub
				{
					Name = "JalaVasePresentBoxGreenRuntimeControl",
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
				sunFeature.Init(new Dictionary { ["Begin"] = 450L });
				control.featureDictionary[new StringName("Sun")] = sunFeature;
				manager.currentControl = control;
				Check(GodotObject.IsInstanceValid(TowerDefenseManager.GetMapCell(TestGrid)), "The PresentBox runtime fixture must publish its map cell before character generation.");
				TowerDefensePacketConfig greenBox = TowerDefenseManager.GetPacketConfig("PlantPresentBoxGreen");
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantPeaShooter");
				Check(GodotObject.IsInstanceValid(greenBox) && greenBox.saveKey == "PlantPresentBoxGreen" && greenBox.characterConfig.cost == 0, "The input fixture must be the real zero-cost green PresentBox packet.");
				Check(GodotObject.IsInstanceValid(packetConfig) && packetConfig.saveKey == "PlantPeaShooter" && packetConfig.characterConfig.cost == 200, "The deterministic replacement must be the real 200-sun PeaShooter packet.");
				TowerDefensePlantJalaVase vase = TowerDefenseManager.GetPacketConfig("PlantJalaVase")?.Plant(TestGrid, playAudio: false, noLimit: true, default, skipPlacementCheck: true) as TowerDefensePlantJalaVase;
				Check(GodotObject.IsInstanceValid(vase), "The real JalaVase packet must instantiate its authored plant scene.");
				if (!GodotObject.IsInstanceValid(vase) || !GodotObject.IsInstanceValid(greenBox) || !GodotObject.IsInstanceValid(packetConfig))
				{
					throw new InvalidOperationException("PresentBox production resources could not instantiate the runtime fixture.");
				}
				await WaitFrames(6);
				BugOverviewJalaVasePresentBoxGreenRuntimeTest bugOverviewJalaVasePresentBoxGreenRuntimeTest = this;
				int condition;
				if (vase.config?.name == "PlantJalaVase" && vase.packet?.saveKey == "PlantJalaVase")
				{
					DestroyComponent destroyComponent = vase.destroyComponent;
					condition = ((destroyComponent != null && !destroyComponent.IsReleased) ? 1 : 0);
				}
				else
				{
					condition = 0;
				}
				bugOverviewJalaVasePresentBoxGreenRuntimeTest.Check((byte)condition != 0, "The runtime fixture must use the real JalaVase character and destroy lifecycle.");
				Check(TowerDefenseManager.GetMapCell(TestGrid)?.HasCharacter("PlantJalaVase") ?? false, "The real JalaVase must occupy its authored map cell before loading.");
				vase.jalaNameList = new Godot.Collections.Array { "PlantPeaShooter" };
				Check(vase.jalaList.Count == 0 && vase.jalaNameList.Count == 1, "The vase must start empty with one deterministic replacement candidate.");
				Check(manager.SetSun(EconomyAccountId.Local, 450L) && sunFeature.GetSun(EconomyAccountId.Local) == 450, "The real local sun ledger must start at 450.");
				vase.AddJala(greenBox);
				Check(vase.jalaList.Count == 1, "Loading one green PresentBox must fill exactly one JalaVase slot.");
				Check(vase.jalaList[0]?.saveKey == "PlantPeaShooter", "JalaVase must persist the resolved replacement rather than the green PresentBox card.");
				Check(vase.jalaList[0]?.saveKey != "PlantPresentBoxGreen", "The original self-paid box card must not remain as the vase content.");
				Check(sunFeature.GetSun(EconomyAccountId.Local) == 250, "Loading the zero-cost box must additionally charge the selected 200-sun content exactly once.");
				bool destroyObserved = false;
				vase.OnDestroy += (TowerDefenseCharacter _) =>
				{
					destroyObserved = true;
				};
				vase.HammerAnimeCompleted("OpenPot");
				await WaitFrames(12);
				TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = FindDroppedPacket(control.characterNode, "PlantPeaShooter");
				TowerDefenseInGamePacketShow instance = FindDroppedPacket(control.characterNode, "PlantPresentBoxGreen");
				Check(destroyObserved && !GodotObject.IsInstanceValid(vase), "The real OpenPot completion must destroy and release the JalaVase shell.");
				Check(GodotObject.IsInstanceValid(towerDefenseInGamePacketShow) && towerDefenseInGamePacketShow.config?.saveKey == "PlantPeaShooter", "Opening the vase must drop the resolved replacement card through the production packet path.");
				Check(!GodotObject.IsInstanceValid(instance), "Opening the vase must not resurrect the original green PresentBox card.");
				Check(sunFeature.GetSun(EconomyAccountId.Local) == 250, "Opening the already-paid vase content must not charge a second time.");
				TowerDefenseZombieNormalPresentBox zombiePresentBox = TowerDefenseManager.GetPacketConfig("ZombieNormalPresentBox")?.Plant(ZombieTestGrid) as TowerDefenseZombieNormalPresentBox;
				Check(GodotObject.IsInstanceValid(zombiePresentBox), "The real ZombieNormalPresentBox packet must instantiate its authored zombie scene.");
				if (GodotObject.IsInstanceValid(zombiePresentBox))
				{
					zombiePresentBox.packetBank = "ZombieImpPresentBox";
					await WaitFrames(4);
					zombiePresentBox.CreateRandom();
					await WaitFrames(12);
				}
				TowerDefenseZombie towerDefenseZombie = FindGeneratedZombie(control.characterNode);
				Check(GodotObject.IsInstanceValid(towerDefenseZombie) && towerDefenseZombie.config?.name != "ZombieNormalPresentBox", "The real zombie PresentBox must generate one resident zombie candidate.");
				Check(ResourceManager.Instance.LateCharacterResourceLoadCount == lateLoadCountBefore, $"PresentBox generation triggered a late character resource load: before={lateLoadCountBefore}, after={ResourceManager.Instance.LateCharacterResourceLoadCount}");
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewJalaVasePresentBoxGreenRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
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
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0 && _checks == 22;
		GD.Print($"JALA_VASE_PRESENT_BOX_GREEN_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseInGamePacketShow FindDroppedPacket(Node parent, string saveKey)
	{
		if (!GodotObject.IsInstanceValid(parent))
		{
			return null;
		}
		foreach (Node child in parent.GetChildren())
		{
			if (child is TowerDefenseInGamePacketShow { config: var config } towerDefenseInGamePacketShow && config?.saveKey == saveKey)
			{
				return towerDefenseInGamePacketShow;
			}
		}
		return null;
	}

	private static TowerDefenseZombie FindGeneratedZombie(Node parent)
	{
		if (!GodotObject.IsInstanceValid(parent))
		{
			return null;
		}
		foreach (Node child in parent.GetChildren())
		{
			if (child is TowerDefenseZombie { config: var config } towerDefenseZombie && config?.name != "ZombieNormalPresentBox")
			{
				return towerDefenseZombie;
			}
		}
		return null;
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = new TowerDefenseMapConfig
			{
				gridNum = gridNum,
				gridSize = new Vector2(100f, 76f),
				gridBeginPos = Vector2.Zero
			}
		});
		towerDefenseBattleFeatureMap.PlantGridInit();
		for (int i = 1; i <= gridNum.X; i++)
		{
			for (int j = 1; j <= gridNum.Y; j++)
			{
				towerDefenseBattleFeatureMap.GetPlantGridCell(new Vector2I(i, j))?.Init(new TowerDefenseCellConfig());
			}
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
			GD.PushError("[BugOverviewJalaVasePresentBoxGreenRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindDroppedPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "saveKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindGeneratedZombie, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.FindDroppedPacket && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(FindDroppedPacket(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindGeneratedZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(FindGeneratedZombie(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.FindDroppedPacket && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(FindDroppedPacket(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindGeneratedZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(FindGeneratedZombie(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.FindDroppedPacket)
		{
			return true;
		}
		if (method == MethodName.FindGeneratedZombie)
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
