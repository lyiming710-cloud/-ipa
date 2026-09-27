using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewNewspaperArmorAngryVisualRuntimeTest.cs")]
public class BugOverviewNewspaperArmorAngryVisualRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindArmor = "FindArmor";

		public static readonly StringName Require = "Require";

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

	private const string NewspaperScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Paper/Scene/TowerDefenseZombiePaper.tscn";

	private const string ArmoredPacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Paper/Packet/ZombiePaperCone.tres";

	private const string AngryHeadReference = "uid://s4hnj2igecma";

	private const string ResultMarker = "BUG_OVERVIEW_NEWSPAPER_ARMOR_ANGRY_VISUAL_RESULT";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		BugOverviewNewspaperArmorAngryVisualControlStub control = null;
		TowerDefenseZombiePaper zombie = null;
		try
		{
			_ = 5;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Require(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload is unavailable.");
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Paper/Scene/TowerDefenseZombiePaper.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
				TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Zombie/Chapter1/Paper/Packet/ZombiePaperCone.tres", null, ResourceLoader.CacheMode.IgnoreDeep);
				Check(GodotObject.IsInstanceValid(packedScene) && packedScene.CanInstantiate() && GodotObject.IsInstanceValid(towerDefensePacketConfig), "The real Newspaper Zombie scene and armored packet must load.");
				Require(GodotObject.IsInstanceValid(packedScene) && packedScene.CanInstantiate() && GodotObject.IsInstanceValid(towerDefensePacketConfig), "One or more authored Newspaper Zombie resources failed to load.");
				Check(towerDefensePacketConfig.saveKey == "ZombiePaperCone" && towerDefensePacketConfig.characterConfig?.name == "ZombiePaper" && towerDefensePacketConfig.initArmor.Count == 1 && towerDefensePacketConfig.initArmor[0] == "Cone", "The real armored packet must author Cone armor for ZombiePaper.");
				control = new BugOverviewNewspaperArmorAngryVisualControlStub
				{
					Name = "NewspaperArmorAngryVisualControl",
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
				zombie = packedScene.Instantiate<TowerDefenseZombiePaper>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(zombie) && zombie.config?.name == "ZombiePaper" && zombie.SceneFilePath == "res://Asset/Anime/Character/Zombie/Chapter1/Paper/Scene/TowerDefenseZombiePaper.tscn", "The fixture must instantiate the reported real Newspaper Zombie scene.");
				Require(GodotObject.IsInstanceValid(zombie), "The Newspaper Zombie scene failed to instantiate.");
				foreach (string item in towerDefensePacketConfig.initArmor)
				{
					zombie.currentArmor.Add(item);
				}
				Check(zombie.currentArmor.Contains("Paper") && zombie.currentArmor.Contains("Cone"), "The character must enter the tree with both its newspaper and packet armor.");
				zombie.inGame = true;
				zombie.editorPreviewMode = false;
				zombie.ProcessMode = ProcessModeEnum.Disabled;
				control.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				Check(GodotObject.IsInstanceValid(zombie.instance) && GodotObject.IsInstanceValid(zombie.sprite) && GodotObject.IsInstanceValid(zombie.headSlot), "The real character instance, Adobe Animate body, and HeadSlot must initialize.");
				Require(GodotObject.IsInstanceValid(zombie.instance) && GodotObject.IsInstanceValid(zombie.sprite) && GodotObject.IsInstanceValid(zombie.headSlot), "The Newspaper Zombie runtime did not initialize.");
				zombie.WalkEntered();
				zombie.WalkExited();
				await WaitFrames(8);
				BugOverviewNewspaperArmorAngryVisualRuntimeTest bugOverviewNewspaperArmorAngryVisualRuntimeTest = this;
				GroundMoveComponent groundMoveComponent = zombie.groundMoveComponent;
				bugOverviewNewspaperArmorAngryVisualRuntimeTest.Check(groundMoveComponent != null && !groundMoveComponent.Alive, "被破报过渡打断的旧行走计时器不得再次开启地面位移。");
				TowerDefenseArmorInstance towerDefenseArmorInstance = FindArmor(zombie, "Paper");
				TowerDefenseArmorInstance cone = FindArmor(zombie, "Cone");
				Check(towerDefenseArmorInstance != null && cone != null && zombie.instance.ArmorHas("Paper") && zombie.instance.ArmorHas("Cone"), "Both real armor instances must be active before the newspaper tears.");
				Require(towerDefenseArmorInstance != null && cone != null, "The real Paper or Cone armor instance is missing.");
				AdobeAnimatePart conePart = cone.sprite;
				string coneTexturePath = ((GodotObject.IsInstanceValid(cone.typeData) && cone.typeData.stageAnimeTexturePaths.Count > 0) ? cone.typeData.stageAnimeTexturePaths[0] : string.Empty);
				Check(GodotObject.IsInstanceValid(conePart) && conePart.GetParent() == zombie.headSlot && conePart.externalAtlasTexturePath == coneTexturePath, "The packet-authored Cone must use its shared-atlas part on HeadSlot.");
				Check(zombie.headSlot.drawLayerId == -2, $"Newspaper HeadSlot must retain the authored Top layer; actual={zombie.headSlot.drawLayerId}.");
				Check(conePart.IsInsideTree() && AdobeAnimateManagedSprite2D.GetLogicalVisible(conePart), "The real Cone shared-atlas part must remain logically visible in HeadSlot.");
				Check(conePart.externalAtlasCentered, "The Cone shared-atlas part must retain Sprite2D-compatible centering.");
				towerDefenseArmorInstance.Hurt(towerDefenseArmorInstance.hitPoints, playSplatAudio: false, default, createDamagePart: false);
				await WaitFrames(2);
				Check(!zombie.instance.ArmorHas("Paper") && zombie.CurrentStateHandle?.StableId == "zombie.paper.gasp" && zombie.headSlot.followSlotId == 18 && zombie.sprite.clip == "Gasp", $"Breaking Paper must enter Gasp on its live head layer; state={zombie.CurrentStateHandle?.StableId}, follow={zombie.headSlot.followSlotId}, clip={zombie.sprite.clip}.");
				Check(zombie.instance.ArmorHas("Cone") && FindArmor(zombie, "Cone") == cone, "Breaking Paper must not remove or recreate the packet-authored Cone.");
				Check(GodotObject.IsInstanceValid(conePart) && conePart.GetParent() == zombie.headSlot && conePart.externalAtlasTexturePath == coneTexturePath && AdobeAnimateManagedSprite2D.GetLogicalVisible(conePart), "The Cone part and atlas path must remain intact during the real Gasp clip.");
				Check(conePart.IsInsideTree(), "The real Gasp scene tree must still contain the Cone part.");
				zombie.AnimeCompleted("Gasp");
				await WaitFrames(2);
				string atlasReplacePath = zombie.sprite.GetAtlasReplacePath("Zombie_head.png");
				bool flag = AdobeAnimateGlobalAtlasCache.TryGetReplaceTextureAllocation(atlasReplacePath, out var allocation);
				Check(((zombie.angry && Math.Abs(zombie.timeScaleInit - 3.0) < 0.0001 && atlasReplacePath == "uid://s4hnj2igecma") & flag) && allocation.UsesTextureArray, "Gasp completion must enable anger and install the authored angry head texture.");
				Check(zombie.CurrentStateHandle?.StableId == "zombie.walk" && zombie.headSlot.followSlotId == 17 && zombie.sprite.clip == "AngryWalk", $"Anger must restore the authored head layer and enter AngryWalk; state={zombie.CurrentStateHandle?.StableId}, follow={zombie.headSlot.followSlotId}, clip={zombie.sprite.clip}.");
				int num = (int)zombie.sprite.flashAnimeData.mediaDictionary["Zombie_head.png"];
				AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult = zombie.sprite.TryBuildCrowdRenderState(out var state);
				AdobeAnimateGpuGraphOwnerState adobeAnimateGpuGraphOwnerState = state?.GpuGraphRootOwnerState;
				Check(adobeAnimateCrowdRenderStateResult == AdobeAnimateCrowdRenderStateResult.Submitted && state != null && state.Mode == AdobeAnimateCrowdRenderMode.GpuGraph && adobeAnimateGpuGraphOwnerState != null && adobeAnimateGpuGraphOwnerState.HasMediaReplace && num >= 0 && num < adobeAnimateGpuGraphOwnerState.MediaReplaceUse.Count && adobeAnimateGpuGraphOwnerState.MediaReplaceUse[num] && adobeAnimateGpuGraphOwnerState.MediaReplaceRect[num] == allocation.Rect && adobeAnimateGpuGraphOwnerState.MediaReplaceAtlasPages[num] == allocation.AtlasPage, "愤怒头像必须进入读报僵尸下一次 GPU Graph 根角色提交，而不只是保留逻辑路径。");
				Check(zombie.instance.ArmorHas("Cone") && cone.sprite == conePart && conePart.externalAtlasTexturePath == coneTexturePath && conePart.GetParent() == zombie.headSlot, "AngryWalk must preserve the exact Cone part, atlas path, and HeadSlot binding.");
				Check(conePart.IsInsideTree() && AdobeAnimateManagedSprite2D.GetLogicalVisible(conePart), "AngryWalk must keep the Cone shared-atlas part logically visible.");
				Check(conePart.externalAtlasCentered, "AngryWalk must preserve the Cone part centering.");
				zombie.AttackEntered();
				await WaitFrames(2);
				Check(zombie.sprite.clip == "AngryEat", "The angry attack callback must enter AngryEat; clip=" + zombie.sprite.clip + ".");
				Check(zombie.instance.ArmorHas("Cone") && cone.sprite == conePart && conePart.externalAtlasTexturePath == coneTexturePath, "AngryEat must retain the same packet-authored Cone atlas part.");
				Check(conePart.IsInsideTree() && AdobeAnimateManagedSprite2D.GetLogicalVisible(conePart), "AngryEat must keep the Cone part logically visible.");
				zombie.sprite.SetAtlasReplace("Zombie_head.png", string.Empty);
				zombie.ImportVariantSave(new Dictionary { { "angry", true } });
				await WaitFrames(2);
				Check(zombie.sprite.GetAtlasReplacePath("Zombie_head.png") == "uid://s4hnj2igecma" && zombie.walkAnimeClip == "AngryWalk" && zombie.attackAnimeClip == "AngryEat", "恢复愤怒存档时必须重新应用头像、行走和攻击表现。");
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[{"BugOverviewNewspaperArmorAngryVisualRuntimeTest"}] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(zombie) && !zombie.IsQueuedForDeletion())
			{
				zombie.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			await WaitFrames(4);
		}
		bool flag2 = _checks == 26 && _failures == 0;
		GD.Print($"{"BUG_OVERVIEW_NEWSPAPER_ARMOR_ANGRY_VISUAL_RESULT"} passed={flag2} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag2) ? 2 : 0);
	}

	private static TowerDefenseArmorInstance FindArmor(TowerDefenseCharacter character, string armorName)
	{
		if (character?.instance?.armorList == null)
		{
			return null;
		}
		foreach (TowerDefenseArmorInstance armor in character.instance.armorList)
		{
			if (armor?.slotConfig?.armorName == armorName && !armor.isRemove)
			{
				return armor;
			}
		}
		return null;
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
			_failures++;
			GD.PushError("[BugOverviewNewspaperArmorAngryVisualRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindArmor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.FindArmor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorInstance>(FindArmor(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindArmor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorInstance>(FindArmor(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
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
		if (method == MethodName.FindArmor)
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
