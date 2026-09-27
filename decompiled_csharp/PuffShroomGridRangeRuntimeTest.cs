using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/PuffShroomGridRangeRuntimeTest.cs")]
public class PuffShroomGridRangeRuntimeTest : Node
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

	private const string PuffShroomScenePath = "res://Asset/Anime/Character/Plant/Chapter0/PuffShroom/Scene/TowerDefensePlantPuffShroom.tscn";

	private const string GroundZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		try
		{
			await RunScenario();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[PuffShroomGridRangeRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"PUFF_SHROOM_GRID_RANGE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task RunScenario()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
		if (!GodotObject.IsInstanceValid(manager))
		{
			return;
		}
		TowerDefenseControlNew control = new TowerDefenseControlNew
		{
			isGameRunning = true
		};
		Node2D characterNode = new Node2D
		{
			Name = "PuffShroomGridRangeCharacters"
		};
		TowerDefenseCharacter puffShroom = null;
		TowerDefenseCharacter target = null;
		try
		{
			AddChild(characterNode, forceReadableName: false, InternalMode.Disabled);
			control.characterNode = characterNode;
			manager.currentControl = control;
			manager.gridBeginPos = new Vector2(0f, 100f);
			manager.gridSize = manager.GetMapGridSize();
			manager.gridNum = new Vector2I(9, 5);
			PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/PuffShroom/Scene/TowerDefensePlantPuffShroom.tscn", null, ResourceLoader.CacheMode.Ignore);
			PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", null, ResourceLoader.CacheMode.Ignore);
			puffShroom = packedScene?.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
			target = packedScene2?.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
			Check(GodotObject.IsInstanceValid(puffShroom) && GodotObject.IsInstanceValid(target), "Real Puff-shroom and ground Zombie scenes must instantiate.");
			if (GodotObject.IsInstanceValid(puffShroom) && GodotObject.IsInstanceValid(target))
			{
				puffShroom.Position = new Vector2(40f, 100f);
				puffShroom.gridPos = new Vector2I(1, 1);
				puffShroom.inGame = true;
				target.Position = new Vector2(360f, 100f);
				target.gridPos = new Vector2I(5, 1);
				target.inGame = true;
				characterNode.AddChild(puffShroom, forceReadableName: false, InternalMode.Disabled);
				characterNode.AddChild(target, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				FireComponent fire = puffShroom.componentManager?.GetRuntime<FireComponent>("character.fire");
				Check(fire != null && !fire.IsReleased, "Puff-shroom FireComponent runtime must be active.");
				if (fire != null && !fire.IsReleased)
				{
					Check(Mathf.IsEqualApprox(fire.checkLength, 3f), $"Puff-shroom checkLength must remain three cells; got {fire.checkLength}.");
					fire.groundRight = 10000f;
					int groundFlags = 9;
					Check(!fire.CanFireCheckOnce(null, groundFlags), "Puff-shroom must not acquire a target four cells ahead.");
					target.Position = new Vector2(280f, 100f);
					target.gridPos = new Vector2I(4, 1);
					await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
					Check(fire.CanFireCheckOnce(null, groundFlags), "Puff-shroom must acquire a target three cells ahead.");
					Check(fire.firstCharacter == target, "The row-range query must preserve Puff-shroom firstCharacter.");
					return;
				}
				return;
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(puffShroom))
			{
				puffShroom.QueueFree();
			}
			if (GodotObject.IsInstanceValid(target))
			{
				target.QueueFree();
			}
			if (GodotObject.IsInstanceValid(characterNode))
			{
				characterNode.QueueFree();
			}
			control.Free();
			manager.currentControl = null;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[PuffShroomGridRangeRuntimeTest] " + message);
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
