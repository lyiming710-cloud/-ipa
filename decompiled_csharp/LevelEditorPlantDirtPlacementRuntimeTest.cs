using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/LevelEditorPlantDirtPlacementRuntimeTest.cs")]
public class LevelEditorPlantDirtPlacementRuntimeTest : Node
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

	private const string EditorScenePath = "res://Prefab/GUI/LevelEditor/MapEditor/LevelEditorMapEditor.tscn";

	private const string MapConfigPath = "res://Asset/Config/Map/Frontlawn/Config/FrontlawnMapFrontlawn.tres";

	private const string PacketPath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Packet/PlantSunFlower.tres";

	private const string CharacterScenePath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn";

	private const string CharacterKey = "PlantSunFlower";

	private static readonly Vector2I TestGrid = new Vector2I(4, 2);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		bool previousEditor = Global.IsEditor;
		string previousScene = SceneManager.CurrentScene;
		Resource previousCharacterScene = null;
		bool hadCharacterScene = false;
		LevelEditorMapEditor editor = null;
		TowerDefensePacketConfig packet = null;
		TowerDefenseMapConfig mapConfig = null;
		bool dirtObserved = false;
		Vector2 observedDirtGlobalPosition = default;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(Global.Instance) && GodotObject.IsInstanceValid(SceneManager.Instance) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && GodotObject.IsInstanceValid(ResourceManager.Instance), "Required autoloads must be available.");
				if (!GodotObject.IsInstanceValid(Global.Instance) || !GodotObject.IsInstanceValid(SceneManager.Instance) || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_009b;
				}
				Global.Instance.isEditor = true;
				SceneManager.Instance.currentScene = "LevelEditorStage";
				editor = ResourceLoader.Load<PackedScene>("res://Prefab/GUI/LevelEditor/MapEditor/LevelEditorMapEditor.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<LevelEditorMapEditor>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(editor), "The real LevelEditorMapEditor scene must instantiate.");
				if (!GodotObject.IsInstanceValid(editor))
				{
					goto end_IL_009b;
				}
				editor.levelConfig = new TowerDefenseLevelConfig();
				AddChild(editor, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(2);
				Check(GodotObject.IsInstanceValid(editor.characterNode) && Mathf.IsEqualApprox(editor.characterNode.GlobalScale.X, 0.85f), $"The real editor CharacterNode must retain its 0.85 scale; got {editor.characterNode?.GlobalScale}.");
				editor.characterNode.ChildEnteredTree += (Node child) =>
				{
					TowerDefenseEffectParticlesOnce effect = child as TowerDefenseEffectParticlesOnce;
					if (effect != null && effect.gridPos == TestGrid)
					{
						Callable.From(() =>
						{
							if (GodotObject.IsInstanceValid(effect))
							{
								dirtObserved = true;
								observedDirtGlobalPosition = effect.GlobalPosition;
							}
						}).CallDeferred();
					}
				};
				mapConfig = ResourceLoader.Load<TowerDefenseMapConfig>("res://Asset/Config/Map/Frontlawn/Config/FrontlawnMapFrontlawn.tres", null, ResourceLoader.CacheMode.IgnoreDeep);
				Check(GodotObject.IsInstanceValid(mapConfig) && editor.mapFeature.MapInit(mapConfig), "The real Frontlawn map must initialize in the editor feature.");
				hadCharacterScene = ResourceManager.Instance.TOWERDEFENSE_CHARCATERS.TryGetValue("PlantSunFlower", out previousCharacterScene);
				ResourceManager.Instance.TOWERDEFENSE_CHARCATERS["PlantSunFlower"] = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn", null, ResourceLoader.CacheMode.Ignore);
				packet = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Packet/PlantSunFlower.tres", null, ResourceLoader.CacheMode.IgnoreDeep);
				Check(GodotObject.IsInstanceValid(packet), "The real SunFlower packet must load.");
				if (!GodotObject.IsInstanceValid(packet))
				{
					goto end_IL_009b;
				}
				TowerDefenseCharacter character = packet.Plant(TestGrid, playAudio: false, noLimit: true);
				Check(GodotObject.IsInstanceValid(character), "The real editor placement path must create the SunFlower.");
				if (!GodotObject.IsInstanceValid(character))
				{
					goto end_IL_009b;
				}
				await WaitFrames(3);
				Check(dirtObserved, "Editor placement must create the real Dirt particle effect.");
				if (dirtObserved)
				{
					Vector2 globalPosition = character.transformPoint.GlobalPosition;
					Vector2 vector = observedDirtGlobalPosition;
					GD.Print($"LEVEL_EDITOR_DIRT_POSITION expected={globalPosition} actual={vector} delta={vector - globalPosition} parentScale={editor.characterNode.GlobalScale}");
					Check(vector.DistanceTo(globalPosition) <= 0.5f, $"Editor Dirt must stay on the planted unit instead of shifting left; expected={globalPosition}, actual={vector}.");
				}
				goto end_IL_0089;
				end_IL_009b:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[LevelEditorPlantDirtPlacementRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0089;
			}
			return;
			end_IL_0089:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(ResourceManager.Instance))
			{
				if (hadCharacterScene)
				{
					ResourceManager.Instance.TOWERDEFENSE_CHARCATERS["PlantSunFlower"] = previousCharacterScene;
				}
				else
				{
					ResourceManager.Instance.TOWERDEFENSE_CHARCATERS.Remove("PlantSunFlower");
				}
			}
			if (GodotObject.IsInstanceValid(editor))
			{
				editor.QueueFree();
			}
			if (GodotObject.IsInstanceValid(Global.Instance))
			{
				Global.Instance.isEditor = previousEditor;
			}
			if (GodotObject.IsInstanceValid(SceneManager.Instance))
			{
				SceneManager.Instance.currentScene = previousScene;
			}
			packet?.Dispose();
			mapConfig?.Dispose();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0 && _checks == 8;
		GD.Print($"LEVEL_EDITOR_PLANT_DIRT_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
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
			GD.PushError("[LevelEditorPlantDirtPlacementRuntimeTest] " + message);
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
