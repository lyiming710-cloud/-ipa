using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewRelifeCustomPreviewRuntimeTest.cs")]
public class BugOverviewRelifeCustomPreviewRuntimeTest : Node
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

	private const string PacketPath = "res://Asset/Anime/Character/Plant/Diamond/Relife/Packet/PlantRelife.tres";

	private const string SpriteScenePath = "res://Asset/Anime/Character/Plant/Diamond/Relife/Relife.tscn";

	private const string CharacterScenePath = "res://Asset/Anime/Character/Plant/Diamond/Relife/Scene/TowerDefensePlantRelife.tscn";

	private const string PacketPreviewScenePath = "res://Prefab/TowerDefense/GUI/InGame/Packet/TowerDefenseInGamePacketShow.tscn";

	private const string PacketSaveKey = "PlantRelife";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		Resource previousSprite = null;
		bool hadPreviousSprite = false;
		Dictionary previousSave = null;
		string previousUser = null;
		TowerDefenseInGamePacketShow preview = null;
		TowerDefensePlantRelife character = null;
		try
		{
			_ = 6;
			try
			{
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(GameSaveManager.Instance), "GameSaveManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(BattleEventBus.Instance), "BattleEventBus autoload must be available.");
				if (!GodotObject.IsInstanceValid(ResourceManager.Instance) || !GodotObject.IsInstanceValid(GameSaveManager.Instance) || !GodotObject.IsInstanceValid(BattleEventBus.Instance))
				{
					goto end_IL_0098;
				}
				previousUser = GameSaveManager.Instance.GetUserCurrent();
				if (string.IsNullOrEmpty(previousUser))
				{
					GameSaveManager.Instance.SetUserCurrent("RelifePreviewRuntimeTest");
				}
				TowerDefensePacketConfig packet = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Plant/Diamond/Relife/Packet/PlantRelife.tres", null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Diamond/Relife/Relife.tscn", null, ResourceLoader.CacheMode.Ignore);
				PackedScene characterScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Diamond/Relife/Scene/TowerDefensePlantRelife.tscn", null, ResourceLoader.CacheMode.Ignore);
				PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://Prefab/TowerDefense/GUI/InGame/Packet/TowerDefenseInGamePacketShow.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(packet) && packet.saveKey == "PlantRelife", "The regression must load the real Relife packet.");
				Check(GodotObject.IsInstanceValid(packedScene) && GodotObject.IsInstanceValid(characterScene) && GodotObject.IsInstanceValid(packedScene2), "The regression must load the real Relife sprite, character, and packet preview scenes.");
				if (!GodotObject.IsInstanceValid(packet) || !GodotObject.IsInstanceValid(packedScene) || !GodotObject.IsInstanceValid(characterScene) || !GodotObject.IsInstanceValid(packedScene2))
				{
					goto end_IL_0098;
				}
				hadPreviousSprite = ResourceManager.Instance.CHARCTAER_SPRITE.TryGetValue("PlantRelife", out previousSprite);
				ResourceManager.Instance.CHARCTAER_SPRITE["PlantRelife"] = packedScene;
				Dictionary towerDefensePacketValue = GameSaveManager.Instance.GetTowerDefensePacketValue("PlantRelife");
				previousSave = towerDefensePacketValue.Duplicate(deep: true);
				Dictionary dictionary = towerDefensePacketValue.GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
				dictionary["Custom"] = "Custom0";
				towerDefensePacketValue["Key"] = dictionary;
				GameSaveManager.Instance.SetTowerDefensePacketValue("PlantRelife", towerDefensePacketValue);
				string text = GameSaveManager.Instance.GetTowerDefensePacketValue("PlantRelife").GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary()
					.GetValueOrDefault("Custom", "")
					.AsString();
				Check(text == "Custom0", "The isolated save fixture must equip Relife Custom0 before creating the preview.");
				preview = packedScene2.Instantiate<TowerDefenseInGamePacketShow>(PackedScene.GenEditState.Disabled);
				AddChild(preview, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(2);
				preview.Init(packet);
				await WaitFrames(3);
				AdobeAnimateSprite previewRoot = preview.sprite;
				AdobeAnimateSprite previewBack = previewRoot?.FindChild("Back", recursive: true, owned: false) as AdobeAnimateSprite;
				Check(GodotObject.IsInstanceValid(previewRoot) && GodotObject.IsInstanceValid(previewBack), "The real packet preview must contain the Relife root and nested Back sprites.");
				Check(previewRoot.GetFliter("skin1_1"), "The equipped Custom0 must be applied to the packet preview root.");
				Check(previewBack.GetFliter("skin1_4") && !previewBack.GetFliter("Pumpkin_back"), "The initially equipped Custom0 must replace the nested pumpkin Back with skin1_4.");
				Check(previewRoot.GetFliter("图层_5"), "Relife Custom0 must retain the shared green bottom-leaf layer 图层_5.");
				BattleEventBus.Instance.EmitCharacterSkinSwitched("PlantRelife", "");
				await WaitFrames(2);
				Check(!previewRoot.GetFliter("skin1_1"), "Switching the packet preview to default must clear the root Custom0 layers.");
				Check(!previewBack.GetFliter("skin1_4") && previewBack.GetFliter("Pumpkin_back"), "Switching the packet preview to default must restore the nested pumpkin Back.");
				BattleEventBus.Instance.EmitCharacterSkinSwitched("PlantRelife", "Custom0");
				await WaitFrames(2);
				Check(previewRoot.GetFliter("skin1_1"), "Switching the existing packet preview back to Custom0 must refresh the root in place.");
				Check(previewBack.GetFliter("skin1_4") && !previewBack.GetFliter("Pumpkin_back"), "Switching the existing packet preview back to Custom0 must refresh the nested Back in place.");
				character = characterScene.Instantiate<TowerDefensePlantRelife>(PackedScene.GenEditState.Disabled);
				character.inGame = false;
				character.currentCustom = new Array<string> { "Custom0" };
				AddChild(character, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(3);
				RelifeSprite relifeSprite = character.sprite as RelifeSprite;
				AdobeAnimateSprite characterBack = relifeSprite?.FindChild("Back", recursive: true, owned: false) as AdobeAnimateSprite;
				Check(GodotObject.IsInstanceValid(relifeSprite) && GodotObject.IsInstanceValid(characterBack), "The real Relife character must retain its root and separate Back sprites.");
				Check(characterBack.GetFliter("skin1_4") && !characterBack.GetFliter("Pumpkin_back"), "The complete Relife character must still display the Custom0 Back.");
				character.SwitchCustom("");
				await WaitFrames(2);
				Check(!characterBack.GetFliter("skin1_4") && characterBack.GetFliter("Pumpkin_back"), "The complete Relife character must still restore its default Back.");
				character.SwitchCustom("Custom0");
				await WaitFrames(2);
				Check(characterBack.GetFliter("skin1_4") && !characterBack.GetFliter("Pumpkin_back"), "The complete Relife character must still switch back to Custom0.");
				goto end_IL_0071;
				end_IL_0098:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewRelifeCustomPreviewRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0071;
			}
			return;
			end_IL_0071:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(preview))
			{
				preview.QueueFree();
			}
			if (GodotObject.IsInstanceValid(character))
			{
				character.QueueFree();
			}
			if (GodotObject.IsInstanceValid(ResourceManager.Instance))
			{
				if (hadPreviousSprite)
				{
					ResourceManager.Instance.CHARCTAER_SPRITE["PlantRelife"] = previousSprite;
				}
				else
				{
					ResourceManager.Instance.CHARCTAER_SPRITE.Remove("PlantRelife");
				}
			}
			if (GodotObject.IsInstanceValid(GameSaveManager.Instance) && previousSave != null)
			{
				GameSaveManager.Instance.SetTowerDefensePacketValue("PlantRelife", previousSave);
			}
			if (GodotObject.IsInstanceValid(GameSaveManager.Instance) && previousUser != null)
			{
				GameSaveManager.Instance.config.userCurrent = previousUser;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0 && _checks == 18;
		GD.Print($"RELIFE_CUSTOM_PREVIEW_RESULT passed={flag} checks={_checks} failures={_failures}");
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
			GD.PushError("[BugOverviewRelifeCustomPreviewRuntimeTest] " + message);
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
