using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentAlmanacCharacterShadowRuntimeTest.cs")]
public class BugDepartmentAlmanacCharacterShadowRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RegisterScene = "RegisterScene";

		public static readonly StringName RestoreScenes = "RestoreScenes";

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

	private const string PlantPacketPath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Packet/PlantSunFlower.tres";

	private const string PlantScenePath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn";

	private const string ZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string PlantPanelPath = "res://Prefab/GUI/InformationPanel/InformationPanel.tscn";

	private const string ZombiePanelPath = "res://Prefab/GUI/InformationPanel/InformationPanelZombie.tscn";

	private readonly Dictionary<string, Resource> _previousScenes = new Dictionary<string, Resource>();

	private readonly HashSet<string> _missingScenes = new HashSet<string>();

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		bool previousUseMultiMesh = ShadowComponent.UseMultiMesh;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available for real Almanac previews.");
				if (!GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_0045;
				}
				RegisterScene("PlantSunFlower", "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn");
				RegisterScene("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
				ShadowComponent.UseMultiMesh = true;
				await VerifyPanel("res://Prefab/GUI/InformationPanel/InformationPanel.tscn", "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Packet/PlantSunFlower.tres", "PlantSunFlower", "real Sunflower");
				await VerifyPanel("res://Prefab/GUI/InformationPanel/InformationPanelZombie.tscn", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres", "ZombieNormal", "real Normal zombie");
				goto end_IL_0033;
				end_IL_0045:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentAlmanacCharacterShadowRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0033;
			}
			return;
			end_IL_0033:;
		}
		finally
		{
			ShadowComponent.UseMultiMesh = previousUseMultiMesh;
			RestoreScenes();
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks >= 23;
		GD.Print($"BUG_DEPARTMENT_ALMANAC_SHADOW_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyPanel(string panelPath, string packetPath, string expectedCharacterName, string fixtureName)
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(panelPath, null, ResourceLoader.CacheMode.Ignore);
		TowerDefensePacketConfig packet = ResourceLoader.Load<TowerDefensePacketConfig>(packetPath, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
		Check(GodotObject.IsInstanceValid(packedScene) && GodotObject.IsInstanceValid(packet), "The " + fixtureName + " packet and its production InformationPanel scene must load.");
		if (!GodotObject.IsInstanceValid(packedScene) || !GodotObject.IsInstanceValid(packet))
		{
			return;
		}
		InformationPanel panel = packedScene.Instantiate<InformationPanel>(PackedScene.GenEditState.Disabled);
		AddChild(panel, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(2);
		panel.InitPacket(packet);
		await WaitFrames(4);
		TowerDefenseCharacter character = panel.currentCharcter;
		Check(GodotObject.IsInstanceValid(character) && character.config?.name == expectedCharacterName, "The " + fixtureName + " Almanac panel must instantiate the corresponding production character scene.");
		Check(character?.shadowComponent != null && !character.shadowComponent.IsReleased, "The " + fixtureName + " preview must retain an active ShadowComponent.");
		Check(GodotObject.IsInstanceValid(character?.shadowSprite) && GodotObject.IsInstanceValid(character.shadowSprite.Texture), "The " + fixtureName + " preview must retain its authored ShadowSprite and texture.");
		if (GodotObject.IsInstanceValid(character?.shadowSprite) && character.shadowComponent != null)
		{
			Control parent = panel.GetNode<Control>("%SpriteNode").GetParent<Control>();
			Check(parent.ClipContents, $"The {fixtureName} production preview must retain child clipping; clip={parent.ClipContents}, path={parent.GetPath()}.");
			Check(parent.IsAncestorOf(character) && character.IsAncestorOf((Sprite2D)character.shadowSprite), $"The {fixtureName} shadow must remain inside the production clipped preview tree; clip={parent.GetPath()}, character={character.GetPath()}, shadow={character.shadowSprite.GetPath()}.");
			Check(!character.shadowComponent.preferMultiMesh, "The " + fixtureName + " Almanac shadow must use the local Sprite2D path even when battle MultiMesh shadows are enabled.");
			Check(character.shadowSprite.Visible && character.shadowSprite.VisibilityLayer != 0 && character.shadowSprite.IsVisibleInTree(), "The " + fixtureName + " shadow must be visibly submitted in the live InformationPanel tree.");
			Check(character.shadowSprite.ZIndex == -1 && character.shadowSprite.ZAsRelative, "The " + fixtureName + " shadow must remain directly below the preview body.");
			Vector2 initialShadowPosition = character.shadowSprite.GlobalPosition;
			for (int frame = 0; frame < 8; frame++)
			{
				character.shadowComponent.BatchUpdate();
				await WaitFrames(1);
			}
			Check(character.shadowSprite.Visible && character.shadowSprite.VisibilityLayer != 0 && character.shadowSprite.IsVisibleInTree(), "The " + fixtureName + " shadow must remain visible after repeated live shadow updates.");
			Check(character.shadowSprite.GlobalPosition.IsEqualApprox(initialShadowPosition), $"The {fixtureName} Almanac shadow must remain grounded after repeated updates; expected={initialShadowPosition}, actual={character.shadowSprite.GlobalPosition}.");
		}
		panel.Clear();
		panel.QueueFree();
		await WaitFrames(3);
	}

	private void RegisterScene(string key, string scenePath)
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.TOWERDEFENSE_CHARCATERS.TryGetValue(key, out var value))
		{
			_previousScenes[key] = value;
		}
		else
		{
			_missingScenes.Add(key);
		}
		instance.TOWERDEFENSE_CHARCATERS[key] = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Ignore);
	}

	private void RestoreScenes()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		foreach (string missingScene in _missingScenes)
		{
			instance.TOWERDEFENSE_CHARCATERS.Remove(missingScene);
		}
		foreach (var (key, value) in _previousScenes)
		{
			instance.TOWERDEFENSE_CHARCATERS[key] = value;
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
			GD.PushError("[BugDepartmentAlmanacCharacterShadowRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "scenePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreScenes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.RegisterScene && args.Count == 2)
		{
			RegisterScene(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreScenes && args.Count == 0)
		{
			RestoreScenes();
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
		if (method == MethodName.RegisterScene)
		{
			return true;
		}
		if (method == MethodName.RestoreScenes)
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
