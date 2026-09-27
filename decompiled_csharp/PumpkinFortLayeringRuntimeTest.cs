using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/PumpkinFortLayeringRuntimeTest.cs")]
public class PumpkinFortLayeringRuntimeTest : Node
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

	private const string PumpkinFortScenePath = "res://Asset/Anime/Character/Plant/Cover/PumpkinFort/Scene/TowerDefensePlantPumpkinFort.tscn";

	private const string PumpkinScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Pumpkin/Scene/TowerDefensePlantPumpkin.tscn";

	private const string SunflowerScenePath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn";

	private static readonly Vector2I TestGridPosition = new Vector2I(3, 2);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefensePlantPumpkinFort gameFort = null;
		TowerDefensePlant innerPlant = null;
		TowerDefensePlantPumpkinFort previewFort = null;
		TowerDefensePlantPumpkin gamePumpkin = null;
		TowerDefensePlantPumpkin previewPumpkin = null;
		try
		{
			_ = 4;
			try
			{
				PackedScene fortScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Cover/PumpkinFort/Scene/TowerDefensePlantPumpkinFort.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(fortScene != null, "The real PumpkinFort character scene must load.");
				gameFort = fortScene?.Instantiate<TowerDefensePlantPumpkinFort>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(gameFort), "The real PumpkinFort character must instantiate for gameplay.");
				if (GodotObject.IsInstanceValid(gameFort))
				{
					Node2D nodeOrNull = gameFort.GetNodeOrNull<Node2D>("SpriteGroup/TransformPoint/PumpkinFort/Node2D/Back");
					Check(GodotObject.IsInstanceValid(nodeOrNull), "PumpkinFort must expose its authored Back visual node.");
					int authoredBackZ = nodeOrNull?.ZIndex ?? (-2147483648);
					Check(authoredBackZ == -2, $"PumpkinFort.tscn must author Back at -2; got {authoredBackZ}.");
					gameFort.inGame = true;
					gameFort.editorPreviewMode = true;
					gameFort.gridPos = TestGridPosition;
					AddChild(gameFort, forceReadableName: false, InternalMode.Disabled);
					await WaitFramePair();
					innerPlant = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Scene/TowerDefensePlantSunFlower.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefensePlant>(PackedScene.GenEditState.Disabled);
					Check(GodotObject.IsInstanceValid(innerPlant), "A real Sunflower must instantiate as the plant inside PumpkinFort.");
					if (GodotObject.IsInstanceValid(innerPlant))
					{
						innerPlant.inGame = true;
						innerPlant.editorPreviewMode = true;
						innerPlant.gridPos = TestGridPosition;
						AddChild(innerPlant, forceReadableName: false, InternalMode.Disabled);
						await WaitFramePair();
						gameFort.FreshZIndex();
						innerPlant.FreshZIndex();
					}
					PumpkinFortSprite pumpkinFortSprite = gameFort.sprite as PumpkinFortSprite;
					Check(GodotObject.IsInstanceValid(pumpkinFortSprite) && GodotObject.IsInstanceValid(pumpkinFortSprite.back), "The real PumpkinFort sprite must bind its split Back visual.");
					if (GodotObject.IsInstanceValid(pumpkinFortSprite?.back) && GodotObject.IsInstanceValid(innerPlant))
					{
						Check(pumpkinFortSprite.back.ZAsRelative, "PumpkinFort Back must remain relative to the character render layer.");
						Check(pumpkinFortSprite.back.ZIndex == authoredBackZ, $"Gameplay _Ready must preserve the authored Back ZIndex {authoredBackZ}; got {pumpkinFortSprite.back.ZIndex}.");
						int num = gameFort.ZIndex + pumpkinFortSprite.back.ZIndex;
						Check(num < innerPlant.ZIndex, $"PumpkinFort Back must render behind its inner plant; back={num}, inner={innerPlant.ZIndex}.");
						Check(gameFort.ZIndex > innerPlant.ZIndex, $"PumpkinFort Front must render in front of its inner plant; front={gameFort.ZIndex}, inner={innerPlant.ZIndex}.");
					}
				}
				previewFort = fortScene?.Instantiate<TowerDefensePlantPumpkinFort>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(previewFort), "The real PumpkinFort character must instantiate for non-game preview.");
				if (GodotObject.IsInstanceValid(previewFort))
				{
					previewFort.inGame = false;
					previewFort.editorPreviewMode = true;
					AddChild(previewFort, forceReadableName: false, InternalMode.Disabled);
					await WaitFramePair();
					PumpkinFortSprite pumpkinFortSprite2 = previewFort.sprite as PumpkinFortSprite;
					Check(GodotObject.IsInstanceValid(pumpkinFortSprite2?.back), "The PumpkinFort preview must bind its Back visual.");
					Check(pumpkinFortSprite2 != null && pumpkinFortSprite2.back.ZIndex == 0, $"Non-game PumpkinFort preview must flatten Back to 0; got {pumpkinFortSprite2?.back.ZIndex}.");
				}
				PackedScene pumpkinScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/Pumpkin/Scene/TowerDefensePlantPumpkin.tscn", null, ResourceLoader.CacheMode.Ignore);
				gamePumpkin = pumpkinScene?.Instantiate<TowerDefensePlantPumpkin>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(gamePumpkin), "The real ordinary Pumpkin control must instantiate for gameplay.");
				if (GodotObject.IsInstanceValid(gamePumpkin))
				{
					gamePumpkin.inGame = true;
					gamePumpkin.editorPreviewMode = true;
					AddChild(gamePumpkin, forceReadableName: false, InternalMode.Disabled);
					await WaitFramePair();
					PumpkinSprite pumpkinSprite = gamePumpkin.sprite as PumpkinSprite;
					Check(GodotObject.IsInstanceValid(pumpkinSprite?.back), "The ordinary Pumpkin gameplay control must bind its Back visual.");
					Check(pumpkinSprite != null && pumpkinSprite.back.ZIndex == -2, $"Ordinary Pumpkin gameplay must preserve Back at -2; got {pumpkinSprite?.back.ZIndex}.");
				}
				previewPumpkin = pumpkinScene?.Instantiate<TowerDefensePlantPumpkin>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(previewPumpkin), "The real ordinary Pumpkin control must instantiate for non-game preview.");
				if (GodotObject.IsInstanceValid(previewPumpkin))
				{
					previewPumpkin.inGame = false;
					previewPumpkin.editorPreviewMode = true;
					AddChild(previewPumpkin, forceReadableName: false, InternalMode.Disabled);
					await WaitFramePair();
					PumpkinSprite pumpkinSprite2 = previewPumpkin.sprite as PumpkinSprite;
					Check(GodotObject.IsInstanceValid(pumpkinSprite2?.back), "The ordinary Pumpkin preview control must bind its Back visual.");
					Check(pumpkinSprite2 != null && pumpkinSprite2.back.ZIndex == 0, $"Ordinary Pumpkin non-game preview must flatten Back to 0; got {pumpkinSprite2?.back.ZIndex}.");
				}
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[PumpkinFortLayeringRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			Node[] array = new Node[5] { gameFort, innerPlant, previewFort, gamePumpkin, previewPumpkin };
			foreach (Node node in array)
			{
				if (GodotObject.IsInstanceValid(node))
				{
					node.QueueFree();
				}
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0 && _checks == 19;
		GD.Print($"PUMPKIN_FORT_LAYERING_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task WaitFramePair()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[PumpkinFortLayeringRuntimeTest] " + message);
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
