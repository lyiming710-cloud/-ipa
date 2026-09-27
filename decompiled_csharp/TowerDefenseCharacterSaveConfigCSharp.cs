using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Save/Character/TowerDefenseCharacterSaveConfigCSharp.cs")]
public class TowerDefenseCharacterSaveConfigCSharp : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName SaveCharacter = "SaveCharacter";

		public static readonly StringName RestoreFinite = "RestoreFinite";

		public static readonly StringName InstantiateCharacterForRestore = "InstantiateCharacterForRestore";

		public static readonly StringName RestoreCharacter = "RestoreCharacter";

		public static readonly StringName LoadCharacter = "LoadCharacter";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName nodeName = "nodeName";

		public static readonly StringName packetName = "packetName";

		public static readonly StringName pos = "pos";

		public static readonly StringName gridPos = "gridPos";

		public static readonly StringName height = "height";

		public static readonly StringName economyOwnerAccountId = "economyOwnerAccountId";

		public static readonly StringName characterNodeSave = "characterNodeSave";

		public static readonly StringName instanceSave = "instanceSave";

		public static readonly StringName buffSave = "buffSave";

		public static readonly StringName currentArmor = "currentArmor";

		public static readonly StringName currentCustom = "currentCustom";

		public static readonly StringName timeScale = "timeScale";

		public static readonly StringName timeScaleInit = "timeScaleInit";

		public static readonly StringName timeScaleSave = "timeScaleSave";

		public static readonly StringName stateMachineSave = "stateMachineSave";

		public static readonly StringName stateChartSave = "stateChartSave";

		public static readonly StringName spriteSave = "spriteSave";

		public static readonly StringName componentSaveList = "componentSaveList";

		public static readonly StringName characterFlags = "characterFlags";

		public static readonly StringName zombieExtraSave = "zombieExtraSave";

		public static readonly StringName scaleX = "scaleX";

		public static readonly StringName scaleY = "scaleY";

		public static readonly StringName transformPointScaleX = "transformPointScaleX";

		public static readonly StringName transformPointScaleY = "transformPointScaleY";

		public static readonly StringName overrideSave = "overrideSave";

		public static readonly StringName variantSave = "variantSave";

		public static readonly StringName canChangeCostSave = "canChangeCostSave";

		public static readonly StringName changeCostListSave = "changeCostListSave";

		public static readonly StringName z = "z";

		public static readonly StringName ySpeed = "ySpeed";

		public static readonly StringName isGround = "isGround";

		public static readonly StringName cellPercentage = "cellPercentage";

		public static readonly StringName cost = "cost";

		public static readonly StringName owner = "owner";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StringName nodeName;

	[Export(PropertyHint.None, "")]
	public string packetName;

	[Export(PropertyHint.None, "")]
	public Vector2 pos;

	[Export(PropertyHint.None, "")]
	public Vector2I gridPos;

	[Export(PropertyHint.None, "")]
	public double height;

	[Export(PropertyHint.None, "")]
	public string economyOwnerAccountId = "";

	[Export(PropertyHint.None, "")]
	public TowerDefenseNodeSaveConfigCSharp characterNodeSave;

	[Export(PropertyHint.None, "")]
	public Dictionary instanceSave = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Array<Dictionary> buffSave = new Array<Dictionary>();

	[Export(PropertyHint.None, "")]
	public Array<string> currentArmor = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<string> currentCustom = new Array<string>();

	[Export(PropertyHint.None, "")]
	public double timeScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double timeScaleInit = 1.0;

	[Export(PropertyHint.None, "")]
	public double timeScaleSave = 1.0;

	[Export(PropertyHint.None, "")]
	public Dictionary stateMachineSave = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary stateChartSave = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary spriteSave = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Array<Dictionary> componentSaveList = new Array<Dictionary>();

	[Export(PropertyHint.None, "")]
	public Dictionary characterFlags = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary zombieExtraSave = new Dictionary();

	[Export(PropertyHint.None, "")]
	public double scaleX = 1.0;

	[Export(PropertyHint.None, "")]
	public double scaleY = 1.0;

	[Export(PropertyHint.None, "")]
	public double transformPointScaleX = 1.0;

	[Export(PropertyHint.None, "")]
	public double transformPointScaleY = 1.0;

	[Export(PropertyHint.None, "")]
	public Dictionary overrideSave = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary variantSave = new Dictionary();

	[Export(PropertyHint.None, "")]
	public bool canChangeCostSave = true;

	[Export(PropertyHint.None, "")]
	public Array<Dictionary> changeCostListSave = new Array<Dictionary>();

	[Export(PropertyHint.None, "")]
	public double z;

	[Export(PropertyHint.None, "")]
	public double ySpeed;

	[Export(PropertyHint.None, "")]
	public bool isGround = true;

	[Export(PropertyHint.None, "")]
	public double cellPercentage = 0.5;

	[Export(PropertyHint.None, "")]
	public double cost;

	public TowerDefenseLevelSaveConfigCSharp owner;

	public void SaveCharacter(TowerDefenseCharacter character)
	{
		owner = null;
		nodeName = new StringName(character.Name.ToString().ValidateNodeName());
		packetName = character.config.name;
		pos = character.GetLogicalGlobalPosition();
		gridPos = character.gridPos;
		height = character.groundHeight;
		economyOwnerAccountId = (character.HasEconomyOwner ? character.EconomyOwnerAccountId.Value : "");
		instanceSave = character.instance.ExportSave();
		BuffComponent buff = character.buff;
		buffSave = ((buff != null && !buff.IsReleased) ? character.buff.ExportSave() : new Array<Dictionary>());
		currentArmor.Clear();
		foreach (TowerDefenseArmorInstance armor in character.instance.armorList)
		{
			if (!armor.isRemove)
			{
				currentArmor.Add(armor.slotConfig.armorName);
			}
		}
		currentCustom = character.currentCustom;
		timeScale = character.timeScale;
		timeScaleInit = character.timeScaleInit;
		timeScaleSave = character.timeScaleSave;
		scaleX = character.Scale.X;
		scaleY = character.Scale.Y;
		if (GodotObject.IsInstanceValid(character.transformPoint))
		{
			transformPointScaleX = character.transformPoint.Scale.X;
			transformPointScaleY = character.transformPoint.Scale.Y;
		}
		if (GodotObject.IsInstanceValid(character.packet) && GodotObject.IsInstanceValid(character.packet._override))
		{
			overrideSave = character.packet._override.Export();
		}
		else
		{
			overrideSave = new Dictionary();
		}
		if (GodotObject.IsInstanceValid(character.packet))
		{
			canChangeCostSave = character.packet.canChangeCost;
			changeCostListSave.Clear();
			foreach (TowerDefensePacketChangeCost changeCost in character.packet.changeCostList)
			{
				changeCostListSave.Add(changeCost.ExportSave());
			}
		}
		stateMachineSave = character.CaptureMainStateMachineSnapshotData();
		stateChartSave.Clear();
		if (GodotObject.IsInstanceValid(character.sprite))
		{
			spriteSave = character.sprite.ExportSpriteSave();
		}
		componentSaveList.Clear();
		if (GodotObject.IsInstanceValid(character.componentManager))
		{
			foreach (ComponentBase component in character.componentManager.componentList)
			{
				Dictionary dictionary = component.ExportComponentSave() ?? new Dictionary();
				if (!component.alive)
				{
					dictionary["_alive"] = false;
				}
				Dictionary dictionary2 = component.CaptureStateMachineSnapshotData();
				if (dictionary2.Count > 0)
				{
					dictionary["_stateMachine"] = dictionary2;
				}
				if (dictionary.Count > 0)
				{
					dictionary["_componentName"] = component.Name;
					if (character.componentManager.TryGetWireKey(component, out var wireKey))
					{
						dictionary["_componentKey"] = wireKey;
					}
					componentSaveList.Add(dictionary);
				}
			}
			foreach (CharacterComponentRuntime resourceComponent in character.componentManager.ResourceComponents)
			{
				if (resourceComponent == null || resourceComponent.IsReleased)
				{
					continue;
				}
				Dictionary dictionary3 = resourceComponent.ExportComponentSave()?.Duplicate(deep: true) ?? new Dictionary();
				dictionary3["_alive"] = resourceComponent.Alive;
				Dictionary dictionary4 = resourceComponent.CaptureStateMachineSnapshotData();
				if (dictionary4.Count > 0)
				{
					dictionary3["_stateMachine"] = dictionary4;
				}
				if (dictionary3.Count != 0)
				{
					CharacterComponentDefinition componentDefinition = resourceComponent.ComponentDefinition;
					if (character.componentManager.TryGetWireKey(resourceComponent, out var wireKey2))
					{
						dictionary3["_componentKey"] = wireKey2;
					}
					if (!string.IsNullOrEmpty(componentDefinition?.InstanceId))
					{
						dictionary3["_componentInstanceId"] = componentDefinition.InstanceId;
					}
					if (!string.IsNullOrEmpty(componentDefinition?.DefinitionId))
					{
						dictionary3["_componentDefinitionId"] = componentDefinition.DefinitionId;
					}
					if (componentDefinition != null)
					{
						dictionary3["_componentSchemaVersion"] = componentDefinition.SchemaVersion;
					}
					componentSaveList.Add(dictionary3);
				}
			}
		}
		characterFlags = new Dictionary
		{
			["componentAlive"] = character.componentAlive,
			["componentRunning"] = character.componentRunning,
			["invisible"] = character.invisible,
			["inWater"] = character.inWater,
			["iceSpeedDown"] = character.iceSpeedDown,
			["isRise"] = character.isRise,
			["isShovel"] = character.isShovel,
			["isSmash"] = character.isSmash,
			["isExplode"] = character.isExplode,
			["isChomp"] = character.isChomp,
			["isUnlimitedFire"] = character.isUnlimitedFire,
			["useIdleAnimeReset"] = character.useIdleAnimeReset,
			["nearDie"] = character.nearDie,
			["canMowerMove"] = character.canMowerMove,
			["die"] = character.die,
			["inGame"] = character.inGame,
			["characterFilter"] = character.characterFilter,
			["componentGameplayUntilBattlefieldEntry"] = character.HasComponentGameplayUntilBattlefieldEntry
		};
		ShaderEffectComponent shaderEffectComponent = character.shaderEffectComponent;
		if (shaderEffectComponent != null && !shaderEffectComponent.IsReleased)
		{
			characterFlags["effectFlags"] = character.shaderEffectComponent.GetEffectFlags();
		}
		z = character.z;
		ySpeed = character.ySpeed;
		isGround = character.isGround;
		cellPercentage = character.cellPercentage;
		cost = character.cost;
		if (character.HasMeta("trio_coral"))
		{
			characterFlags["trio_coral"] = true;
		}
		variantSave = character.ExportVariantSave();
		if (character is TowerDefenseZombie towerDefenseZombie)
		{
			zombieExtraSave = new Dictionary
			{
				["isPause"] = towerDefenseZombie.isPause,
				["isGarlic"] = towerDefenseZombie.isGarlic,
				["isChangeLine"] = towerDefenseZombie.isChangeLine,
				["inSwimPlay"] = towerDefenseZombie.inSwimPlay,
				["inGround"] = towerDefenseZombie.inGround,
				["startAttack"] = towerDefenseZombie.startAttack,
				["sizeUpNum"] = towerDefenseZombie.sizeUpNum,
				["hasGhost"] = towerDefenseZombie.hasGhost,
				["hasSpikeball"] = towerDefenseZombie.hasSpikeball,
				["spritePause"] = towerDefenseZombie.spritePause,
				["walkSpeedScale"] = towerDefenseZombie.walkSpeedScale
			};
		}
	}

	private double RestoreFinite(double value, double fallback)
	{
		if (double.IsFinite(value) && value <= 3.4028234663852886E+38 && value >= -3.4028234663852886E+38)
		{
			return value;
		}
		owner.RestoreReport.Record("CharacterState", "Invalid character number; using default.");
		return fallback;
	}

	public TowerDefenseCharacter InstantiateCharacterForRestore()
	{
		if (owner == null)
		{
			owner = new TowerDefenseLevelSaveConfigCSharp();
		}
		if (characterFlags == null)
		{
			characterFlags = new Dictionary();
		}
		if (zombieExtraSave == null)
		{
			zombieExtraSave = new Dictionary();
		}
		if (variantSave == null)
		{
			variantSave = new Dictionary();
		}
		if (stateMachineSave == null)
		{
			stateMachineSave = new Dictionary();
		}
		if (stateChartSave == null)
		{
			stateChartSave = new Dictionary();
		}
		if (spriteSave == null)
		{
			spriteSave = new Dictionary();
		}
		if (currentArmor == null)
		{
			currentArmor = new Array<string>();
		}
		if (currentCustom == null)
		{
			currentCustom = new Array<string>();
		}
		if (!pos.IsFinite())
		{
			pos = Vector2.Zero;
			owner.RestoreReport.Record("CharacterState", "Invalid character position.");
		}
		height = RestoreFinite(height, 0.0);
		z = RestoreFinite(z, 0.0);
		ySpeed = RestoreFinite(ySpeed, 0.0);
		scaleX = RestoreFinite(scaleX, 1.0);
		scaleY = RestoreFinite(scaleY, 1.0);
		transformPointScaleX = RestoreFinite(transformPointScaleX, 1.0);
		transformPointScaleY = RestoreFinite(transformPointScaleY, 1.0);
		timeScale = RestoreFinite(timeScale, 1.0);
		timeScaleInit = RestoreFinite(timeScaleInit, 1.0);
		timeScaleSave = RestoreFinite(timeScaleSave, 1.0);
		EconomyAccountId accountId = default;
		if (!string.IsNullOrWhiteSpace(economyOwnerAccountId) && (!EconomyAccountId.TryParse(economyOwnerAccountId, out accountId) || !accountId.IsLocal))
		{
			owner.RestoreReport.Record("CharacterState", "Using local character ownership.");
			accountId = default;
		}
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(packetName);
		if (!GodotObject.IsInstanceValid(packetConfigReadOnly))
		{
			return null;
		}
		TowerDefensePacketConfig towerDefensePacketConfig = packetConfigReadOnly.Duplicate(deep: true) as TowerDefensePacketConfig;
		if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
		{
			return null;
		}
		Dictionary dictionary = overrideSave;
		if (dictionary != null && dictionary.Count > 0)
		{
			TowerDefensePacketOverride towerDefensePacketOverride = new TowerDefensePacketOverride();
			towerDefensePacketOverride.Init(overrideSave);
			towerDefensePacketConfig._override = towerDefensePacketOverride;
		}
		towerDefensePacketConfig.canChangeCost = canChangeCostSave;
		towerDefensePacketConfig.changeCostList = new List<TowerDefensePacketChangeCost>();
		foreach (Dictionary item in changeCostListSave ?? new Array<Dictionary>())
		{
			towerDefensePacketConfig.changeCostList.Add(TowerDefensePacketChangeCost.ImportSave(item));
		}
		TowerDefenseCharacter towerDefenseCharacter = towerDefensePacketConfig.Create(pos, gridPos, height);
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			return null;
		}
		try
		{
			if (accountId.IsValid && !towerDefenseCharacter.TryAssignEconomyOwner(accountId))
			{
				towerDefenseCharacter.QueueFree();
				return null;
			}
			if (nodeName != null && !nodeName.IsEmpty)
			{
				towerDefenseCharacter.Name = nodeName;
			}
			towerDefenseCharacter.groundHeight = height;
			towerDefenseCharacter.currentArmor = currentArmor;
			towerDefenseCharacter.currentCustom = currentCustom;
			towerDefenseCharacter.packet = towerDefensePacketConfig;
			towerDefenseCharacter.PrepareForProgressRestore();
			characterNode.AddChild(towerDefenseCharacter, forceReadableName: false, Node.InternalMode.Disabled);
			return towerDefenseCharacter;
		}
		catch
		{
			TowerDefenseLevelSaveConfigCSharp.DiscardRestoredCharacter(towerDefenseCharacter);
			throw;
		}
	}

	public void RestoreCharacter(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return;
		}
		ProgressRestoreReport restoreReport = owner?.RestoreReport ?? new ProgressRestoreReport();
		character.RestoreFromSave(this);
		character.Scale = new Vector2((float)scaleX, (float)scaleY);
		if (GodotObject.IsInstanceValid(character.transformPoint))
		{
			character.transformPoint.Scale = new Vector2((float)transformPointScaleX, (float)transformPointScaleY);
		}
		ShadowComponent shadowComponent = character.shadowComponent;
		if (shadowComponent != null && !shadowComponent.IsReleased)
		{
			character.shadowComponent.Init();
		}
		WaterInteractionComponent waterInteractionComponent = (GodotObject.IsInstanceValid(character.componentManager) ? character.componentManager.GetRuntime<WaterInteractionComponent>() : null);
		if (waterInteractionComponent != null && !waterInteractionComponent.IsReleased)
		{
			waterInteractionComponent.saveTransformPointScaleY = character.transformPoint.Scale.Y;
			waterInteractionComponent.saveSpriteGroupScaleY = character.spriteGroup.Scale.Y;
		}
		character.isGround = isGround;
		character.z = z;
		character.ySpeed = ySpeed;
		character.cellPercentage = cellPercentage;
		character.cost = cost;
		character.componentAlive = characterFlags.GetValueOrDefault("componentAlive", true).AsBool();
		character.componentRunning = characterFlags.GetValueOrDefault("componentRunning", false).AsBool();
		character.invisible = characterFlags.GetValueOrDefault("invisible", false).AsBool();
		character.iceSpeedDown = characterFlags.GetValueOrDefault("iceSpeedDown", false).AsBool();
		character.isRise = characterFlags.GetValueOrDefault("isRise", false).AsBool();
		character.isShovel = characterFlags.GetValueOrDefault("isShovel", false).AsBool();
		character.isSmash = characterFlags.GetValueOrDefault("isSmash", false).AsBool();
		character.isExplode = characterFlags.GetValueOrDefault("isExplode", false).AsBool();
		character.isChomp = characterFlags.GetValueOrDefault("isChomp", false).AsBool();
		character.isUnlimitedFire = characterFlags.GetValueOrDefault("isUnlimitedFire", false).AsBool();
		character.useIdleAnimeReset = characterFlags.GetValueOrDefault("useIdleAnimeReset", true).AsBool();
		character.nearDie = characterFlags.GetValueOrDefault("nearDie", false).AsBool();
		character.canMowerMove = characterFlags.GetValueOrDefault("canMowerMove", false).AsBool();
		character.die = characterFlags.GetValueOrDefault("die", false).AsBool();
		character.inGame = characterFlags.GetValueOrDefault("inGame", true).AsBool();
		character.characterFilter = characterFlags.GetValueOrDefault("characterFilter", false).AsBool();
		if (characterFlags.GetValueOrDefault("componentGameplayUntilBattlefieldEntry", false).AsBool())
		{
			character.EnableComponentGameplayUntilBattlefieldEntry();
		}
		foreach (Dictionary item in componentSaveList ?? new Array<Dictionary>())
		{
			try
			{
				string text = item.GetValueOrDefault("_componentName", "").AsString();
				string text2 = item.GetValueOrDefault("_componentKey", "").AsString();
				string text3 = item.GetValueOrDefault("_componentInstanceId", "").AsString();
				item.GetValueOrDefault("_componentDefinitionId", "").AsString();
				item.GetValueOrDefault("_componentSchemaVersion", 0).AsInt32();
				CharacterComponentRuntime runtime = null;
				if (text3 != "")
				{
					character.componentManager.TryGetRuntimeByInstanceId(text3, out runtime);
				}
				else if (text2 != "")
				{
					character.componentManager.TryGetRuntimeByWireKey(text2, out runtime);
				}
				if (runtime == null && text3 == "" && text != "")
				{
					character.componentManager.TryGetRuntimeByLegacyNodeName(text, out runtime);
				}
				if (runtime != null)
				{
					StateMachineSnapshot snapshot = null;
					bool flag = item.ContainsKey("_stateMachine") && ComponentBase.TryDecodeStateMachineSnapshot(item["_stateMachine"].AsGodotDictionary(), out snapshot) && runtime.CanRestoreStateMachineSnapshot(snapshot, progressCompatible: true);
					if (item.ContainsKey("_stateMachine") && !flag)
					{
						restoreReport.Record("Component", "Using initial component state.");
					}
					if (flag)
					{
						runtime.BeginAuthoritativeStateRestore();
					}
					try
					{
						runtime.ImportComponentSave(item, owner);
					}
					finally
					{
						if (flag)
						{
							runtime.EndAuthoritativeStateRestore();
						}
					}
					if (flag)
					{
						runtime.RestoreStateMachineSnapshot(snapshot, remote: false, suppressEntryEffects: true, progressCompatible: true);
					}
					if (item.ContainsKey("_alive"))
					{
						runtime.SetAlive(item["_alive"].AsBool());
					}
					continue;
				}
				if (text3 != "")
				{
					restoreReport.Record("Component", "Skipped missing resource component save '" + text3 + "'.");
					continue;
				}
				ComponentBase component = null;
				if (text2 != "")
				{
					character.componentManager.TryGetComponentByWireKey(text2, out component);
				}
				if (!GodotObject.IsInstanceValid(component) && text != "")
				{
					component = character.componentManager.GetComponentFromName(text);
				}
				if (GodotObject.IsInstanceValid(component))
				{
					StateMachineSnapshot snapshot2 = null;
					bool flag2 = item.ContainsKey("_stateMachine") && ComponentBase.TryDecodeStateMachineSnapshot(item["_stateMachine"].AsGodotDictionary(), out snapshot2) && component.CanRestoreStateMachineSnapshot(snapshot2, progressCompatible: true);
					if (item.ContainsKey("_stateMachine") && !flag2)
					{
						restoreReport.Record("Component", "Using initial component state.");
					}
					if (flag2)
					{
						component.BeginAuthoritativeStateRestore();
					}
					try
					{
						component.ImportComponentSave(item, owner);
					}
					finally
					{
						if (flag2)
						{
							component.EndAuthoritativeStateRestore();
						}
					}
					if (flag2)
					{
						component.RestoreStateMachineSnapshot(snapshot2, remote: false, suppressEntryEffects: true, progressCompatible: true);
					}
					if (item.ContainsKey("_alive"))
					{
						component.SetAlive(item["_alive"].AsBool());
					}
				}
				else
				{
					restoreReport.Record("Component", "Missing component: " + text + ".");
				}
			}
			catch (Exception ex)
			{
				restoreReport.Record("Component", ex.Message);
			}
		}
		character.inWater = characterFlags.GetValueOrDefault("inWater", false).AsBool();
		if (character is TowerDefenseZombie towerDefenseZombie && zombieExtraSave.Count > 0)
		{
			towerDefenseZombie.isGarlic = zombieExtraSave.GetValueOrDefault("isGarlic", false).AsBool();
			towerDefenseZombie.isChangeLine = zombieExtraSave.GetValueOrDefault("isChangeLine", false).AsBool();
			towerDefenseZombie.inSwimPlay = zombieExtraSave.GetValueOrDefault("inSwimPlay", false).AsBool();
			towerDefenseZombie.inGround = zombieExtraSave.GetValueOrDefault("inGround", false).AsBool();
			towerDefenseZombie.startAttack = zombieExtraSave.GetValueOrDefault("startAttack", false).AsBool();
			towerDefenseZombie.sizeUpNum = zombieExtraSave.GetValueOrDefault("sizeUpNum", 2).AsInt32();
			towerDefenseZombie.hasGhost = zombieExtraSave.GetValueOrDefault("hasGhost", false).AsBool();
			towerDefenseZombie.hasSpikeball = zombieExtraSave.GetValueOrDefault("hasSpikeball", false).AsBool();
			towerDefenseZombie.walkSpeedScale = zombieExtraSave.GetValueOrDefault("walkSpeedScale", 1.0).AsDouble();
		}
		if (variantSave.Count > 0 || character.ImportVariantSaveWhenEmpty())
		{
			restoreReport.Try("CharacterState", () =>
			{
				character.ImportVariantSave(variantSave);
			});
		}
		restoreReport.Try("CharacterState", () =>
		{
			if (stateMachineSave.Count > 0)
			{
				if (!character.RestoreMainStateMachineSnapshotData(stateMachineSave, remote: false, suppressEntryEffects: false, progressCompatible: true))
				{
					restoreReport.Record("CharacterState", "Using initial character state.");
				}
				character.timeScaleInit = timeScaleInit;
				character.timeScaleSave = timeScaleSave;
				if (character is TowerDefensePlant && GodotObject.IsInstanceValid(character.sprite))
				{
					character.sprite.timeScale = timeScale;
				}
			}
			else
			{
				StateChart nodeOrNull = character.GetNodeOrNull<StateChart>("StateChart");
				string stateName;
				if (nodeOrNull != null && GodotObject.IsInstanceValid(nodeOrNull.CurrentState) && stateChartSave.Count > 0)
				{
					SavedState savedState = new SavedState
					{
						childStates = stateChartSave.GetValueOrDefault("child_states", new Dictionary()).AsGodotDictionary(),
						pendingTransitionName = new NodePath(stateChartSave.GetValueOrDefault("pending_transition_name", "").AsString()),
						pendingTransitionRemainingDelay = (float)stateChartSave.GetValueOrDefault("pending_transition_remaining_delay", 0.0).AsDouble(),
						pendingTransitionInitialDelay = (float)stateChartSave.GetValueOrDefault("pending_transition_initial_delay", 0.0).AsDouble()
					};
					nodeOrNull.CurrentState.StateRestore(savedState);
					character.timeScaleInit = timeScaleInit;
					character.timeScaleSave = timeScaleSave;
					if (character is TowerDefensePlant && GodotObject.IsInstanceValid(character.sprite))
					{
						character.sprite.timeScale = timeScale;
					}
				}
				else if (stateChartSave.Count > 0 && TryGetLegacyActiveLeaf(stateChartSave.GetValueOrDefault("child_states", new Dictionary()).AsGodotDictionary(), out stateName))
				{
					character.RestoreLegacyMainState(stateName);
					character.timeScaleInit = timeScaleInit;
					character.timeScaleSave = timeScaleSave;
					if (character is TowerDefensePlant && GodotObject.IsInstanceValid(character.sprite))
					{
						character.sprite.timeScale = timeScale;
					}
				}
			}
		});
		if (GodotObject.IsInstanceValid(character.sprite) && spriteSave.Count > 0)
		{
			restoreReport.Try("CharacterState", () =>
			{
				character.sprite.ImportSpriteSave(spriteSave);
			});
		}
		if (character is TowerDefenseZombie towerDefenseZombie2 && zombieExtraSave.Count > 0)
		{
			towerDefenseZombie2.spritePause = zombieExtraSave.GetValueOrDefault("spritePause", false).AsBool();
			towerDefenseZombie2.isPause = false;
		}
		if (characterFlags.ContainsKey("effectFlags"))
		{
			ShaderEffectComponent shaderEffectComponent = character.shaderEffectComponent;
			if (shaderEffectComponent != null && !shaderEffectComponent.IsReleased)
			{
				character.shaderEffectComponent.SetEffectFlags(characterFlags.GetValueOrDefault("effectFlags", 0).AsInt32());
			}
		}
		character.FinalizeProgressRestore();
		if (character is TowerDefenseZombie zombie && characterFlags.GetValueOrDefault("trio_coral", false).AsBool())
		{
			TrioAmbushMember.Attach(zombie, coral: true, 0.0, null, -1, restored: true);
		}
	}

	public TowerDefenseCharacter LoadCharacter()
	{
		TowerDefenseCharacter towerDefenseCharacter = InstantiateCharacterForRestore();
		RestoreCharacter(towerDefenseCharacter);
		return towerDefenseCharacter;
	}

	private static bool TryGetLegacyActiveLeaf(Dictionary childStates, out string stateName)
	{
		stateName = string.Empty;
		if (childStates == null || childStates.Count == 0)
		{
			return false;
		}
		foreach (Variant key in childStates.Keys)
		{
			string text = key.AsString();
			if (!string.IsNullOrWhiteSpace(text))
			{
				stateName = text;
				Variant variant = childStates[key];
				Dictionary dictionary = null;
				if (variant.VariantType == Variant.Type.Object && variant.As<GodotObject>() is SavedState savedState)
				{
					dictionary = savedState.childStates;
				}
				else if (variant.VariantType == Variant.Type.Dictionary)
				{
					Dictionary dictionary2 = variant.AsGodotDictionary();
					dictionary = (dictionary2.ContainsKey("child_states") ? dictionary2["child_states"].AsGodotDictionary() : dictionary2);
				}
				if (dictionary != null && TryGetLegacyActiveLeaf(dictionary, out var stateName2))
				{
					stateName = stateName2;
				}
				return true;
			}
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.SaveCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreFinite, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InstantiateCharacterForRestore, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SaveCharacter && args.Count == 1)
		{
			SaveCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreFinite && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(RestoreFinite(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.InstantiateCharacterForRestore && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(InstantiateCharacterForRestore());
			return true;
		}
		if (method == MethodName.RestoreCharacter && args.Count == 1)
		{
			RestoreCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadCharacter && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(LoadCharacter());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SaveCharacter)
		{
			return true;
		}
		if (method == MethodName.RestoreFinite)
		{
			return true;
		}
		if (method == MethodName.InstantiateCharacterForRestore)
		{
			return true;
		}
		if (method == MethodName.RestoreCharacter)
		{
			return true;
		}
		if (method == MethodName.LoadCharacter)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.nodeName)
		{
			nodeName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.packetName)
		{
			packetName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.pos)
		{
			pos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			gridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.height)
		{
			height = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.economyOwnerAccountId)
		{
			economyOwnerAccountId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.characterNodeSave)
		{
			characterNodeSave = VariantUtils.ConvertTo<TowerDefenseNodeSaveConfigCSharp>(in value);
			return true;
		}
		if (name == PropertyName.instanceSave)
		{
			instanceSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.buffSave)
		{
			buffSave = VariantUtils.ConvertToArray<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.currentArmor)
		{
			currentArmor = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.currentCustom)
		{
			currentCustom = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			timeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.timeScaleInit)
		{
			timeScaleInit = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.timeScaleSave)
		{
			timeScaleSave = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.stateMachineSave)
		{
			stateMachineSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.stateChartSave)
		{
			stateChartSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.spriteSave)
		{
			spriteSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.componentSaveList)
		{
			componentSaveList = VariantUtils.ConvertToArray<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.characterFlags)
		{
			characterFlags = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.zombieExtraSave)
		{
			zombieExtraSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.scaleX)
		{
			scaleX = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.scaleY)
		{
			scaleY = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.transformPointScaleX)
		{
			transformPointScaleX = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.transformPointScaleY)
		{
			transformPointScaleY = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.overrideSave)
		{
			overrideSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.variantSave)
		{
			variantSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.canChangeCostSave)
		{
			canChangeCostSave = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.changeCostListSave)
		{
			changeCostListSave = VariantUtils.ConvertToArray<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.z)
		{
			z = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.ySpeed)
		{
			ySpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.isGround)
		{
			isGround = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.cellPercentage)
		{
			cellPercentage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.cost)
		{
			cost = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.owner)
		{
			owner = VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.nodeName)
		{
			value = VariantUtils.CreateFrom(in nodeName);
			return true;
		}
		if (name == PropertyName.packetName)
		{
			value = VariantUtils.CreateFrom(in packetName);
			return true;
		}
		if (name == PropertyName.pos)
		{
			value = VariantUtils.CreateFrom(in pos);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			value = VariantUtils.CreateFrom(in gridPos);
			return true;
		}
		if (name == PropertyName.height)
		{
			value = VariantUtils.CreateFrom(in height);
			return true;
		}
		if (name == PropertyName.economyOwnerAccountId)
		{
			value = VariantUtils.CreateFrom(in economyOwnerAccountId);
			return true;
		}
		if (name == PropertyName.characterNodeSave)
		{
			value = VariantUtils.CreateFrom(in characterNodeSave);
			return true;
		}
		if (name == PropertyName.instanceSave)
		{
			value = VariantUtils.CreateFrom(in instanceSave);
			return true;
		}
		if (name == PropertyName.buffSave)
		{
			value = VariantUtils.CreateFromArray(buffSave);
			return true;
		}
		if (name == PropertyName.currentArmor)
		{
			value = VariantUtils.CreateFromArray(currentArmor);
			return true;
		}
		if (name == PropertyName.currentCustom)
		{
			value = VariantUtils.CreateFromArray(currentCustom);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			value = VariantUtils.CreateFrom(in timeScale);
			return true;
		}
		if (name == PropertyName.timeScaleInit)
		{
			value = VariantUtils.CreateFrom(in timeScaleInit);
			return true;
		}
		if (name == PropertyName.timeScaleSave)
		{
			value = VariantUtils.CreateFrom(in timeScaleSave);
			return true;
		}
		if (name == PropertyName.stateMachineSave)
		{
			value = VariantUtils.CreateFrom(in stateMachineSave);
			return true;
		}
		if (name == PropertyName.stateChartSave)
		{
			value = VariantUtils.CreateFrom(in stateChartSave);
			return true;
		}
		if (name == PropertyName.spriteSave)
		{
			value = VariantUtils.CreateFrom(in spriteSave);
			return true;
		}
		if (name == PropertyName.componentSaveList)
		{
			value = VariantUtils.CreateFromArray(componentSaveList);
			return true;
		}
		if (name == PropertyName.characterFlags)
		{
			value = VariantUtils.CreateFrom(in characterFlags);
			return true;
		}
		if (name == PropertyName.zombieExtraSave)
		{
			value = VariantUtils.CreateFrom(in zombieExtraSave);
			return true;
		}
		if (name == PropertyName.scaleX)
		{
			value = VariantUtils.CreateFrom(in scaleX);
			return true;
		}
		if (name == PropertyName.scaleY)
		{
			value = VariantUtils.CreateFrom(in scaleY);
			return true;
		}
		if (name == PropertyName.transformPointScaleX)
		{
			value = VariantUtils.CreateFrom(in transformPointScaleX);
			return true;
		}
		if (name == PropertyName.transformPointScaleY)
		{
			value = VariantUtils.CreateFrom(in transformPointScaleY);
			return true;
		}
		if (name == PropertyName.overrideSave)
		{
			value = VariantUtils.CreateFrom(in overrideSave);
			return true;
		}
		if (name == PropertyName.variantSave)
		{
			value = VariantUtils.CreateFrom(in variantSave);
			return true;
		}
		if (name == PropertyName.canChangeCostSave)
		{
			value = VariantUtils.CreateFrom(in canChangeCostSave);
			return true;
		}
		if (name == PropertyName.changeCostListSave)
		{
			value = VariantUtils.CreateFromArray(changeCostListSave);
			return true;
		}
		if (name == PropertyName.z)
		{
			value = VariantUtils.CreateFrom(in z);
			return true;
		}
		if (name == PropertyName.ySpeed)
		{
			value = VariantUtils.CreateFrom(in ySpeed);
			return true;
		}
		if (name == PropertyName.isGround)
		{
			value = VariantUtils.CreateFrom(in isGround);
			return true;
		}
		if (name == PropertyName.cellPercentage)
		{
			value = VariantUtils.CreateFrom(in cellPercentage);
			return true;
		}
		if (name == PropertyName.cost)
		{
			value = VariantUtils.CreateFrom(in cost);
			return true;
		}
		if (name == PropertyName.owner)
		{
			value = VariantUtils.CreateFrom(in owner);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.nodeName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.packetName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.pos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.height, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.economyOwnerAccountId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterNodeSave, PropertyHint.ResourceType, "TowerDefenseNodeSaveConfigCSharp", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.instanceSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.buffSave, PropertyHint.TypeString, "27/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.currentArmor, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.currentCustom, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScaleInit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScaleSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.stateMachineSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.stateChartSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.spriteSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.componentSaveList, PropertyHint.TypeString, "27/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.characterFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.zombieExtraSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.scaleX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.scaleY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.transformPointScaleX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.transformPointScaleY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.overrideSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.variantSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canChangeCostSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.changeCostListSave, PropertyHint.TypeString, "27/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.z, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.ySpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isGround, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cellPercentage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.owner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.nodeName, Variant.From(in nodeName));
		info.AddProperty(PropertyName.packetName, Variant.From(in packetName));
		info.AddProperty(PropertyName.pos, Variant.From(in pos));
		info.AddProperty(PropertyName.gridPos, Variant.From(in gridPos));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.economyOwnerAccountId, Variant.From(in economyOwnerAccountId));
		info.AddProperty(PropertyName.characterNodeSave, Variant.From(in characterNodeSave));
		info.AddProperty(PropertyName.instanceSave, Variant.From(in instanceSave));
		info.AddProperty(PropertyName.buffSave, Variant.CreateFrom(buffSave));
		info.AddProperty(PropertyName.currentArmor, Variant.CreateFrom(currentArmor));
		info.AddProperty(PropertyName.currentCustom, Variant.CreateFrom(currentCustom));
		info.AddProperty(PropertyName.timeScale, Variant.From(in timeScale));
		info.AddProperty(PropertyName.timeScaleInit, Variant.From(in timeScaleInit));
		info.AddProperty(PropertyName.timeScaleSave, Variant.From(in timeScaleSave));
		info.AddProperty(PropertyName.stateMachineSave, Variant.From(in stateMachineSave));
		info.AddProperty(PropertyName.stateChartSave, Variant.From(in stateChartSave));
		info.AddProperty(PropertyName.spriteSave, Variant.From(in spriteSave));
		info.AddProperty(PropertyName.componentSaveList, Variant.CreateFrom(componentSaveList));
		info.AddProperty(PropertyName.characterFlags, Variant.From(in characterFlags));
		info.AddProperty(PropertyName.zombieExtraSave, Variant.From(in zombieExtraSave));
		info.AddProperty(PropertyName.scaleX, Variant.From(in scaleX));
		info.AddProperty(PropertyName.scaleY, Variant.From(in scaleY));
		info.AddProperty(PropertyName.transformPointScaleX, Variant.From(in transformPointScaleX));
		info.AddProperty(PropertyName.transformPointScaleY, Variant.From(in transformPointScaleY));
		info.AddProperty(PropertyName.overrideSave, Variant.From(in overrideSave));
		info.AddProperty(PropertyName.variantSave, Variant.From(in variantSave));
		info.AddProperty(PropertyName.canChangeCostSave, Variant.From(in canChangeCostSave));
		info.AddProperty(PropertyName.changeCostListSave, Variant.CreateFrom(changeCostListSave));
		info.AddProperty(PropertyName.z, Variant.From(in z));
		info.AddProperty(PropertyName.ySpeed, Variant.From(in ySpeed));
		info.AddProperty(PropertyName.isGround, Variant.From(in isGround));
		info.AddProperty(PropertyName.cellPercentage, Variant.From(in cellPercentage));
		info.AddProperty(PropertyName.cost, Variant.From(in cost));
		info.AddProperty(PropertyName.owner, Variant.From(in owner));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.nodeName, out var value))
		{
			nodeName = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.packetName, out var value2))
		{
			packetName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.pos, out var value3))
		{
			pos = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.gridPos, out var value4))
		{
			gridPos = value4.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value5))
		{
			height = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.economyOwnerAccountId, out var value6))
		{
			economyOwnerAccountId = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.characterNodeSave, out var value7))
		{
			characterNodeSave = value7.As<TowerDefenseNodeSaveConfigCSharp>();
		}
		if (info.TryGetProperty(PropertyName.instanceSave, out var value8))
		{
			instanceSave = value8.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.buffSave, out var value9))
		{
			buffSave = value9.AsGodotArray<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.currentArmor, out var value10))
		{
			currentArmor = value10.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.currentCustom, out var value11))
		{
			currentCustom = value11.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.timeScale, out var value12))
		{
			timeScale = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.timeScaleInit, out var value13))
		{
			timeScaleInit = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.timeScaleSave, out var value14))
		{
			timeScaleSave = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.stateMachineSave, out var value15))
		{
			stateMachineSave = value15.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.stateChartSave, out var value16))
		{
			stateChartSave = value16.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.spriteSave, out var value17))
		{
			spriteSave = value17.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.componentSaveList, out var value18))
		{
			componentSaveList = value18.AsGodotArray<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.characterFlags, out var value19))
		{
			characterFlags = value19.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.zombieExtraSave, out var value20))
		{
			zombieExtraSave = value20.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.scaleX, out var value21))
		{
			scaleX = value21.As<double>();
		}
		if (info.TryGetProperty(PropertyName.scaleY, out var value22))
		{
			scaleY = value22.As<double>();
		}
		if (info.TryGetProperty(PropertyName.transformPointScaleX, out var value23))
		{
			transformPointScaleX = value23.As<double>();
		}
		if (info.TryGetProperty(PropertyName.transformPointScaleY, out var value24))
		{
			transformPointScaleY = value24.As<double>();
		}
		if (info.TryGetProperty(PropertyName.overrideSave, out var value25))
		{
			overrideSave = value25.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.variantSave, out var value26))
		{
			variantSave = value26.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.canChangeCostSave, out var value27))
		{
			canChangeCostSave = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.changeCostListSave, out var value28))
		{
			changeCostListSave = value28.AsGodotArray<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.z, out var value29))
		{
			z = value29.As<double>();
		}
		if (info.TryGetProperty(PropertyName.ySpeed, out var value30))
		{
			ySpeed = value30.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isGround, out var value31))
		{
			isGround = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.cellPercentage, out var value32))
		{
			cellPercentage = value32.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cost, out var value33))
		{
			cost = value33.As<double>();
		}
		if (info.TryGetProperty(PropertyName.owner, out var value34))
		{
			owner = value34.As<TowerDefenseLevelSaveConfigCSharp>();
		}
	}
}
