using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewMobileRoofFifthRowTapRuntimeTest.cs")]
public class BugOverviewMobileRoofFifthRowTapRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateRoofMapFeature = "CreateRoofMapFeature";

		public static readonly StringName GetPlacementPoint = "GetPlacementPoint";

		public static readonly StringName SelectCard = "SelectCard";

		public static readonly StringName DispatchPressAction = "DispatchPressAction";

		public static readonly StringName FindPot = "FindPot";

		public static readonly StringName CountPlacedPots = "CountPlacedPots";

		public static readonly StringName RegisterPotCharacter = "RegisterPotCharacter";

		public static readonly StringName RestorePotCharacter = "RestorePotCharacter";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName repeatSelectionOnly = "repeatSelectionOnly";

		public static readonly StringName droppedCardSelectionOnly = "droppedCardSelectionOnly";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _previousPotCharacter = "_previousPotCharacter";

		public static readonly StringName _potCharacterWasMissing = "_potCharacterWasMissing";

		public static readonly StringName _previousPotSprite = "_previousPotSprite";

		public static readonly StringName _potSpriteWasMissing = "_potSpriteWasMissing";
	}

	public new class SignalName : Node.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool repeatSelectionOnly;

	[Export(PropertyHint.None, "")]
	public bool droppedCardSelectionOnly;

	private const string RoofConfigPath = "res://Asset/Config/Map/Roof/Config/FrontlawnMapRoof.tres";

	private const string PotPacketPath = "res://Asset/Anime/Character/Plant/Chapter0/Pot/Packet/PlantPot.tres";

	private const string PotScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Pot/Scene/TowerDefensePlantPot.tscn";

	private const string PotSpritePath = "res://Asset/Anime/Character/Plant/Chapter0/Pot/Pot.tscn";

	private int _checks;

	private int _failures;

	private Resource _previousPotCharacter;

	private bool _potCharacterWasMissing;

	private Resource _previousPotSprite;

	private bool _potSpriteWasMissing;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		bool previousMobile = Global.Instance?.isMobile ?? false;
		bool previousEditor = Global.Instance?.isEditor ?? false;
		bool previousMultiplayer = Global.Instance?.isMultiplayerMode ?? false;
		MobileRoofFifthRowTapRuntimeControlStub control = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		PacketPickControl packetPickControl = null;
		TowerDefenseMapConfig roofConfig = null;
		List<TowerDefenseInGamePacketShow> slots = new List<TowerDefenseInGamePacketShow>();
		try
		{
			_ = 5;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(Global.Instance) && GodotObject.IsInstanceValid(ResourceManager.Instance), "Manager, Global, and ResourceManager autoloads must be available.");
				Check(InputMap.HasAction("Press"), "The production Press action must exist for the mobile input regression.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(Global.Instance) || !GodotObject.IsInstanceValid(ResourceManager.Instance) || !InputMap.HasAction("Press"))
				{
					goto end_IL_0150;
				}
				RegisterPotCharacter();
				roofConfig = ResourceLoader.Load<TowerDefenseMapConfig>("res://Asset/Config/Map/Roof/Config/FrontlawnMapRoof.tres", null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefenseMapConfig;
				Check(GodotObject.IsInstanceValid(roofConfig) && roofConfig.gridNum == new Vector2I(9, 5), "The regression must use the real five-row Roof map config.");
				if (!GodotObject.IsInstanceValid(roofConfig))
				{
					goto end_IL_0150;
				}
				Global.Instance.isMobile = true;
				Global.Instance.isEditor = false;
				Global.Instance.isMultiplayerMode = false;
				control = new MobileRoofFifthRowTapRuntimeControlStub
				{
					Name = "MobileRoofFifthRowTapRuntimeControl",
					isGameRunning = false,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig
					{
						packetColdDownUse = repeatSelectionOnly
					}
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				MobileRoofFifthRowTapRuntimeMapControlStub mobileRoofFifthRowTapRuntimeMapControlStub = new MobileRoofFifthRowTapRuntimeMapControlStub
				{
					Name = "MapControl"
				};
				AddChild(mobileRoofFifthRowTapRuntimeMapControlStub, forceReadableName: false, InternalMode.Disabled);
				mobileRoofFifthRowTapRuntimeMapControlStub.spriteNode = new Node2D
				{
					Name = "SpriteNode"
				};
				mobileRoofFifthRowTapRuntimeMapControlStub.AddChild(mobileRoofFifthRowTapRuntimeMapControlStub.spriteNode, forceReadableName: false, InternalMode.Disabled);
				mapFeature = (mobileRoofFifthRowTapRuntimeMapControlStub.mapFeature = CreateRoofMapFeature(control, mobileRoofFifthRowTapRuntimeMapControlStub, roofConfig));
				control.featureDictionary[new StringName("Map")] = mapFeature;
				manager.gridBeginPos = roofConfig.gridBeginPos;
				manager.gridSize = roofConfig.gridSize;
				manager.gridNum = roofConfig.gridNum;
				packetPickControl = new PacketPickControl
				{
					Name = "PacketPickControl"
				};
				mobileRoofFifthRowTapRuntimeMapControlStub.AddChild(packetPickControl, forceReadableName: false, InternalMode.Disabled);
				packetPickControl.Init(mobileRoofFifthRowTapRuntimeMapControlStub, mapFeature, new TowerDefenseBattleFeaturePacketPickConfig
				{
					packetSelectionDebounceFrames = 5
				});
				mapFeature.packetPickControl = packetPickControl;
				for (int index = 0; index < 4; index++)
				{
					slots.Add(await CreatePotSlot(control.characterNode, packetPickControl));
				}
				bool flag = slots.Count == 4;
				foreach (TowerDefenseInGamePacketShow item in slots)
				{
					flag &= GodotObject.IsInstanceValid(item);
				}
				Check(flag, "Four real Flower Pot cards must be ready for independent input routes.");
				if (!flag)
				{
					goto end_IL_0150;
				}
				VerifyRepeatSelectionChain(packetPickControl, slots);
				System.Reflection.MethodInfo placementSurfaceMethod;
				if (droppedCardSelectionOnly)
				{
					await VerifyDroppedCardSelectionGesture(packetPickControl, mapFeature, slots[0]);
				}
				else if (repeatSelectionOnly)
				{
					await VerifyPlantCooldownRepeatSelection(packetPickControl, slots[0]);
				}
				else
				{
					Vector2I tapFifthGrid = new Vector2I(1, 5);
					Vector2I dragFifthGrid = new Vector2I(2, 5);
					Vector2I otherRowGrid = new Vector2I(3, 2);
					Vector2 placementPoint = GetPlacementPoint(mapFeature, tapFifthGrid);
					Vector2 dragFifthPoint = GetPlacementPoint(mapFeature, dragFifthGrid);
					Vector2 otherRowPoint = GetPlacementPoint(mapFeature, otherRowGrid);
					Check(!mapFeature.groundRect.HasPoint(placementPoint) && manager.GetMapGridPosFromMouse(placementPoint) == tapFifthGrid && GodotObject.IsInstanceValid(TowerDefenseManager.GetMapCell(tapFifthGrid)), $"The real sloped Roof row five point must be outside the legacy rectangle but resolve to a valid cell; point={placementPoint}, rect={mapFeature.groundRect}.");
					bool flag2 = true;
					for (int i = 1; i <= 4; i++)
					{
						Vector2I vector2I = new Vector2I(1, i);
						flag2 &= manager.GetMapGridPosFromMouse(GetPlacementPoint(mapFeature, vector2I)) == vector2I;
					}
					Check(flag2, "Roof rows one through four must retain their height-curve-aware coordinate mapping.");
					placementSurfaceMethod = typeof(PacketPickControl).GetMethod("IsPlacementSurfacePoint", BindingFlags.Instance | BindingFlags.NonPublic);
					Check(placementSurfaceMethod != null, "The mobile touch-down path must expose its placement-surface predicate.");
					if (placementSurfaceMethod == null)
					{
						goto end_IL_0150;
					}
					Check(IsPlacementSurface(placementPoint) && packetPickControl.packetPick == slots[0], "Touch-down on the valid fifth Roof row must be treated as placement surface and retain the card.");
					TowerDefenseCharacter tapPot = slots[0].Plant(tapFifthGrid, useSun: false);
					await WaitFrames(5);
					Check(tapPot is TowerDefensePlantPot && FindPot(control, tapFifthGrid) != null, "Point-select then tap must be able to place a real Flower Pot on Roof row five.");
					packetPickControl.Release();
					Check(!GodotObject.IsInstanceValid(packetPickControl.packetPick), "A successful point-selected placement must release the picked card.");
					SelectCard(slots[1]);
					Check(packetPickControl.packetPick == slots[1] && IsPlacementSurface(dragFifthPoint), "The existing drag target must remain a valid Roof row-five placement surface.");
					TowerDefenseCharacter dragPot = slots[1].Plant(dragFifthGrid, useSun: false);
					await WaitFrames(5);
					Check(dragPot is TowerDefensePlantPot && FindPot(control, dragFifthGrid) != null, "The existing drag-and-release route must still be able to place on Roof row five.");
					packetPickControl.Release();
					Check(!GodotObject.IsInstanceValid(packetPickControl.packetPick), "A successful dragged placement must release the picked card.");
					SelectCard(slots[2]);
					Check(packetPickControl.packetPick == slots[2] && mapFeature.groundRect.HasPoint(otherRowPoint) && IsPlacementSurface(otherRowPoint), "An ordinary Roof row touch must retain the selected card through touch-down.");
					TowerDefenseCharacter otherPot = slots[2].Plant(otherRowGrid, useSun: false);
					await WaitFrames(5);
					Check(otherPot is TowerDefensePlantPot && FindPot(control, otherRowGrid) != null, "Point-select then tap must continue to place on another Roof row.");
					packetPickControl.Release();
					SelectCard(slots[3]);
					Vector2 vector = roofConfig.gridBeginPos - new Vector2(120f, 120f);
					Check(packetPickControl.packetPick == slots[3] && !IsPlacementSurface(vector) && manager.GetMapGridPosFromMouse(vector).Y == -1, "A true off-map mobile touch must remain outside the placement surface.");
					if (!IsPlacementSurface(vector))
					{
						packetPickControl.Release();
					}
					Check(!GodotObject.IsInstanceValid(packetPickControl.packetPick), "The production off-map branch must still cancel the selected card.");
					Check(CountPlacedPots(control) == 3, "Exactly the fifth-row tap, fifth-row drag, and ordinary-row tap must place.");
				}
				goto end_IL_012d;
				end_IL_0150:
				bool IsPlacementSurface(Vector2 point)
				{
					return (bool)placementSurfaceMethod.Invoke(packetPickControl, new object[1] { point });
				}
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewMobileRoofFifthRowTapRuntimeTest] Unexpected exception: {value}");
				goto end_IL_012d;
			}
			return;
			end_IL_012d:;
		}
		finally
		{
			DispatchPressAction(pressed: false);
			if (GodotObject.IsInstanceValid(packetPickControl))
			{
				packetPickControl.DisposeBattleState();
			}
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
			await WaitFrames(6);
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(Global.Instance))
			{
				Global.Instance.isMobile = previousMobile;
				Global.Instance.isEditor = previousEditor;
				Global.Instance.isMultiplayerMode = previousMultiplayer;
			}
			mapFeature?.Destroy();
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			RestorePotCharacter();
			await WaitFrames(6);
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			roofConfig?.Dispose();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		int num;
		if (droppedCardSelectionOnly)
		{
			num = 14;
		}
		else
		{
			num = (repeatSelectionOnly ? 17 : 20);
		}
		bool flag3 = _failures == 0 && _checks >= num;
		string value2;
		if (droppedCardSelectionOnly)
		{
			value2 = "DROPPED_CARD_SELECTION_GESTURE_RESULT";
		}
		else
		{
			value2 = (repeatSelectionOnly ? "PACKET_PICK_REPEAT_SELECTION_RESULT" : "MOBILE_ROOF_FIFTH_ROW_TAP_RESULT");
		}
		GD.Print($"{value2} passed={flag3} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag3) ? 2 : 0);
	}

	private async Task<TowerDefenseInGamePacketShow> CreatePotSlot(Node parent, PacketPickControl packetPickControl)
	{
		TowerDefensePacketConfig config = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Plant/Chapter0/Pot/Packet/PlantPot.tres", null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
		if (!GodotObject.IsInstanceValid(config))
		{
			return null;
		}
		TowerDefenseInGamePacketShow slot = TowerDefenseManager.CreatePacketShow();
		parent.AddChild(slot, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(2);
		slot.Init(config);
		slot.useCost = false;
		slot.start = true;
		slot.SetCentralRuntimeStateRefresh(enabled: true);
		slot.coldDownOpen = false;
		slot.alive = true;
		slot.OnPressed += packetPickControl.PickPacket;
		return slot;
	}

	private void VerifyRepeatSelectionChain(PacketPickControl packetPickControl, List<TowerDefenseInGamePacketShow> slots)
	{
		SelectCard(slots[0]);
		Check(packetPickControl.packetPick == slots[0], "The point-selection route must select the real Flower Pot card first.");
		packetPickControl.Release();
		Check(!GodotObject.IsInstanceValid(packetPickControl.packetPick) && !slots[0].select && slots[0].pressDelayTimer <= 0.0, "Putting a card back must clear its selection and the previous click debounce.");
		slots[0].Pressed();
		Check(packetPickControl.packetPick == slots[0] && slots[0].select, "The same card must be selectable again immediately after it is put back.");
		slots[1].Pressed();
		Check(packetPickControl.packetPick == slots[1] && slots[1].select && !slots[0].select && slots[0].pressDelayTimer <= 0.0, "Clicking another card must immediately release the previous card and select the new one.");
		slots[0].Pressed();
		Check(packetPickControl.packetPick == slots[0] && slots[0].select && !slots[1].select && slots[1].pressDelayTimer <= 0.0, "Rapidly switching back must not inherit the other card's selection or click debounce.");
	}

	private async Task VerifyPlantCooldownRepeatSelection(PacketPickControl packetPickControl, TowerDefenseInGamePacketShow slot)
	{
		TowerDefenseCharacter firstPot = slot.Plant(new Vector2I(1, 5), useSun: false);
		await WaitFrames(5);
		Check(firstPot is TowerDefensePlantPot && slot.coldDownOpen && !slot.select, "The first real placement must start cooldown and clear the card selection.");
		packetPickControl.Release();
		Check(!GodotObject.IsInstanceValid(packetPickControl.packetPick), "The first successful placement must release the picked card.");
		slot.coldDownTimer = 0.0;
		await WaitFrames(3);
		Check(!slot.coldDownOpen && slot.alive, "The real card must become alive again when its post-placement cooldown completes.");
		Check(!slot.coldDownProgressBar.Visible, "Cooldown completion must hide the overlay covering the real card button.");
		Check(slot.coldDownProgressBar.MouseFilter == Control.MouseFilterEnum.Ignore, "The visual cooldown overlay must never consume pointer input.");
		Check(slot.button.MouseFilter != Control.MouseFilterEnum.Ignore, "Cooldown completion must leave the real card button input enabled.");
		slot.button.EmitSignal(BaseButton.SignalName.Pressed);
		Check(packetPickControl.packetPick == slot && slot.select, "The real card Button signal must select the same card again after cooldown.");
		TowerDefenseCharacter secondPot = slot.Plant(new Vector2I(2, 5), useSun: false);
		await WaitFrames(5);
		Check(secondPot is TowerDefensePlantPot && slot.coldDownOpen, "The same real card must successfully plant a second time after cooldown.");
		packetPickControl.Release();
	}

	private async Task VerifyDroppedCardSelectionGesture(PacketPickControl packetPickControl, TowerDefenseBattleFeatureMap mapFeature, TowerDefenseInGamePacketShow slot)
	{
		Vector2I dropGrid = new Vector2I(4, 3);
		TowerDefenseCellInstance dropCell = TowerDefenseManager.GetMapCell(dropGrid);
		Vector2 dropPoint = GetPlacementPoint(mapFeature, dropGrid);
		packetPickControl.Release();
		slot.alive = true;
		slot.select = false;
		slot.pressDelayTimer = 0.0;
		DispatchPressAction(pressed: true);
		slot.button.EmitSignal(BaseButton.SignalName.Pressed);
		Check(packetPickControl.packetPick == slot && slot.select, "Pressing the real dropped-card button must select the card while the Press action is held.");
		for (int i = 0; i < 8; i++)
		{
			packetPickControl.ProcessPacketPick(dropCell, dropGrid, dropPoint);
		}
		Check(FindPot(TowerDefenseManager.Instance.currentControl, dropGrid) == null, "Holding a dropped card beyond the frame debounce must not plant it.");
		DispatchPressAction(pressed: false);
		bool flag = await WaitForPressRelease();
		packetPickControl.ProcessPacketPick(dropCell, dropGrid, dropPoint);
		Check((FindPot(TowerDefenseManager.Instance.currentControl, dropGrid) == null && packetPickControl.packetPick == slot) & flag, "Releasing the gesture that selected a dropped card must be observed and retain the card instead of planting below it.");
		await WaitFrames(2);
		DispatchPressAction(pressed: true);
		packetPickControl.ProcessPacketPick(dropCell, dropGrid, dropPoint);
		Check(FindPot(TowerDefenseManager.Instance.currentControl, dropGrid) == null && packetPickControl.packetPick == slot, "A subsequent placement gesture must still wait for the mobile release confirmation.");
		await WaitFrames(2);
		DispatchPressAction(pressed: false);
		bool placementReleaseObserved = await WaitForPressRelease();
		packetPickControl.ProcessPacketPick(dropCell, dropGrid, dropPoint);
		await WaitFrames(5);
		TowerDefensePlantPot towerDefensePlantPot = FindPot(TowerDefenseManager.Instance.currentControl, dropGrid);
		Check(towerDefensePlantPot != null && !GodotObject.IsInstanceValid(packetPickControl.packetPick), $"A second independent tap must plant the selected dropped card and release the selection; placed={GodotObject.IsInstanceValid(towerDefensePlantPot)}, picked={GodotObject.IsInstanceValid(packetPickControl.packetPick)}, slot_alive={slot.alive}, slot_select={slot.select}, release_observed={placementReleaseObserved}.");
	}

	private async Task<bool> WaitForPressRelease()
	{
		for (int frame = 0; frame < 4; frame++)
		{
			if (Input.IsActionJustReleased("Press"))
			{
				return true;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (Input.IsActionJustReleased("Press"))
			{
				return true;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		return Input.IsActionJustReleased("Press");
	}

	private static TowerDefenseBattleFeatureMap CreateRoofMapFeature(TowerDefenseControlNew control, TowerDefenseMapControl mapControl, TowerDefenseMapConfig config)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = new TowerDefenseBattleFeatureMap();
		towerDefenseBattleFeatureMap.control = control;
		towerDefenseBattleFeatureMap.mapControl = mapControl;
		towerDefenseBattleFeatureMap.config = config;
		towerDefenseBattleFeatureMap.mapConfig = config;
		towerDefenseBattleFeatureMap.groundRect = new Rect2(config.gridBeginPos, config.gridSize * new Vector2(config.gridNum.X, config.gridNum.Y));
		towerDefenseBattleFeatureMap.PlantGridInit();
		return towerDefenseBattleFeatureMap;
	}

	private static Vector2 GetPlacementPoint(TowerDefenseBattleFeatureMap mapFeature, Vector2I gridPos)
	{
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		return TowerDefenseManager.GetMapCellPlantPos(gridPos) - new Vector2(0f, (float)mapFeature.GetGroundHeight(mapCell));
	}

	private static void SelectCard(TowerDefenseInGamePacketShow slot)
	{
		slot.alive = true;
		slot.select = false;
		slot.pressDelayTimer = 0.0;
		slot.Pressed();
	}

	private static void DispatchPressAction(bool pressed)
	{
		Input.ParseInputEvent(new InputEventAction
		{
			Action = "Press",
			Pressed = pressed,
			Strength = (pressed ? 1f : 0f)
		});
		Input.FlushBufferedEvents();
	}

	private static TowerDefensePlantPot FindPot(TowerDefenseControlNew control, Vector2I gridPos)
	{
		foreach (Node child in control.characterNode.GetChildren())
		{
			if (child is TowerDefensePlantPot towerDefensePlantPot && towerDefensePlantPot.gridPos == gridPos)
			{
				return towerDefensePlantPot;
			}
		}
		return null;
	}

	private static int CountPlacedPots(TowerDefenseControlNew control)
	{
		int num = 0;
		foreach (Node child in control.characterNode.GetChildren())
		{
			if (child is TowerDefensePlantPot)
			{
				num++;
			}
		}
		return num;
	}

	private void RegisterPotCharacter()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.TOWERDEFENSE_CHARCATERS.TryGetValue("PlantPot", out var value))
		{
			_previousPotCharacter = value;
		}
		else
		{
			_potCharacterWasMissing = true;
		}
		instance.TOWERDEFENSE_CHARCATERS["PlantPot"] = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/Pot/Scene/TowerDefensePlantPot.tscn", null, ResourceLoader.CacheMode.Ignore);
		if (instance.CHARCTAER_SPRITE.TryGetValue("PlantPot", out var value2))
		{
			_previousPotSprite = value2;
		}
		else
		{
			_potSpriteWasMissing = true;
		}
		instance.CHARCTAER_SPRITE["PlantPot"] = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/Pot/Pot.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
	}

	private void RestorePotCharacter()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			if (_potCharacterWasMissing)
			{
				instance.TOWERDEFENSE_CHARCATERS.Remove("PlantPot");
			}
			else if (GodotObject.IsInstanceValid(_previousPotCharacter))
			{
				instance.TOWERDEFENSE_CHARCATERS["PlantPot"] = _previousPotCharacter;
			}
			if (_potSpriteWasMissing)
			{
				instance.CHARCTAER_SPRITE.Remove("PlantPot");
			}
			else if (GodotObject.IsInstanceValid(_previousPotSprite))
			{
				instance.CHARCTAER_SPRITE["PlantPot"] = _previousPotSprite;
			}
		}
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
			GD.PushError("[BugOverviewMobileRoofFifthRowTapRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(10)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateRoofMapFeature, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetPlacementPoint, new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "mapFeature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.SelectCard, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.DispatchPressAction, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.FindPot, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CountPlacedPots, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterPotCharacter, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RestorePotCharacter, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateRoofMapFeature && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateRoofMapFeature(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[1]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[2])));
			return true;
		}
		if (method == MethodName.GetPlacementPoint && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetPlacementPoint(VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.SelectCard && args.Count == 1)
		{
			SelectCard(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchPressAction && args.Count == 1)
		{
			DispatchPressAction(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindPot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlantPot>(FindPot(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CountPlacedPots && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountPlacedPots(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterPotCharacter && args.Count == 0)
		{
			RegisterPotCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.RestorePotCharacter && args.Count == 0)
		{
			RestorePotCharacter();
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
		if (method == MethodName.CreateRoofMapFeature && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateRoofMapFeature(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[1]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[2])));
			return true;
		}
		if (method == MethodName.GetPlacementPoint && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetPlacementPoint(VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.SelectCard && args.Count == 1)
		{
			SelectCard(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchPressAction && args.Count == 1)
		{
			DispatchPressAction(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindPot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlantPot>(FindPot(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CountPlacedPots && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountPlacedPots(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0])));
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
		if (method == MethodName.CreateRoofMapFeature)
		{
			return true;
		}
		if (method == MethodName.GetPlacementPoint)
		{
			return true;
		}
		if (method == MethodName.SelectCard)
		{
			return true;
		}
		if (method == MethodName.DispatchPressAction)
		{
			return true;
		}
		if (method == MethodName.FindPot)
		{
			return true;
		}
		if (method == MethodName.CountPlacedPots)
		{
			return true;
		}
		if (method == MethodName.RegisterPotCharacter)
		{
			return true;
		}
		if (method == MethodName.RestorePotCharacter)
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
		if (name == PropertyName.repeatSelectionOnly)
		{
			repeatSelectionOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.droppedCardSelectionOnly)
		{
			droppedCardSelectionOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
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
		if (name == PropertyName._previousPotCharacter)
		{
			_previousPotCharacter = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._potCharacterWasMissing)
		{
			_potCharacterWasMissing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._previousPotSprite)
		{
			_previousPotSprite = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._potSpriteWasMissing)
		{
			_potSpriteWasMissing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.repeatSelectionOnly)
		{
			value = VariantUtils.CreateFrom(in repeatSelectionOnly);
			return true;
		}
		if (name == PropertyName.droppedCardSelectionOnly)
		{
			value = VariantUtils.CreateFrom(in droppedCardSelectionOnly);
			return true;
		}
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
		if (name == PropertyName._previousPotCharacter)
		{
			value = VariantUtils.CreateFrom(in _previousPotCharacter);
			return true;
		}
		if (name == PropertyName._potCharacterWasMissing)
		{
			value = VariantUtils.CreateFrom(in _potCharacterWasMissing);
			return true;
		}
		if (name == PropertyName._previousPotSprite)
		{
			value = VariantUtils.CreateFrom(in _previousPotSprite);
			return true;
		}
		if (name == PropertyName._potSpriteWasMissing)
		{
			value = VariantUtils.CreateFrom(in _potSpriteWasMissing);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName.repeatSelectionOnly, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName.droppedCardSelectionOnly, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previousPotCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._potCharacterWasMissing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previousPotSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._potSpriteWasMissing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.repeatSelectionOnly, Variant.From(in repeatSelectionOnly));
		info.AddProperty(PropertyName.droppedCardSelectionOnly, Variant.From(in droppedCardSelectionOnly));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._previousPotCharacter, Variant.From(in _previousPotCharacter));
		info.AddProperty(PropertyName._potCharacterWasMissing, Variant.From(in _potCharacterWasMissing));
		info.AddProperty(PropertyName._previousPotSprite, Variant.From(in _previousPotSprite));
		info.AddProperty(PropertyName._potSpriteWasMissing, Variant.From(in _potSpriteWasMissing));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.repeatSelectionOnly, out var value))
		{
			repeatSelectionOnly = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.droppedCardSelectionOnly, out var value2))
		{
			droppedCardSelectionOnly = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value3))
		{
			_checks = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value4))
		{
			_failures = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._previousPotCharacter, out var value5))
		{
			_previousPotCharacter = value5.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._potCharacterWasMissing, out var value6))
		{
			_potCharacterWasMissing = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._previousPotSprite, out var value7))
		{
			_previousPotSprite = value7.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._potSpriteWasMissing, out var value8))
		{
			_potSpriteWasMissing = value8.As<bool>();
		}
	}
}
