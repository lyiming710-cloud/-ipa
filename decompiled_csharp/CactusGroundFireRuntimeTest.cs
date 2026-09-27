using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/CactusGroundFireRuntimeTest.cs")]
public class CactusGroundFireRuntimeTest : Node
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

	private const string CactusScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Cactus/Scene/TowerDefensePlantCactus.tscn";

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
			GD.PushError($"[CactusGroundFireRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"CACTUS_GROUND_FIRE_RUNTIME_RESULT passed={flag} checks={_checks} failures={_failures}");
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
			Name = "CactusGroundFireCharacters"
		};
		TowerDefensePlantCactus cactus = null;
		TowerDefenseCharacter target = null;
		try
		{
			AddChild(characterNode, forceReadableName: false, InternalMode.Disabled);
			control.characterNode = characterNode;
			manager.currentControl = control;
			manager.gridBeginPos = new Vector2(0f, 100f);
			manager.gridSize = new Vector2(100f, 76f);
			manager.gridNum = new Vector2I(9, 5);
			PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/Cactus/Scene/TowerDefensePlantCactus.tscn", null, ResourceLoader.CacheMode.Ignore);
			PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", null, ResourceLoader.CacheMode.Ignore);
			cactus = packedScene?.Instantiate<TowerDefensePlantCactus>(PackedScene.GenEditState.Disabled);
			target = packedScene2?.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
			Check(GodotObject.IsInstanceValid(cactus) && GodotObject.IsInstanceValid(target), "Real Cactus and ground Zombie scenes must instantiate.");
			if (GodotObject.IsInstanceValid(cactus) && GodotObject.IsInstanceValid(target))
			{
				cactus.Position = new Vector2(100f, 100f);
				cactus.gridPos = new Vector2I(1, 1);
				cactus.inGame = true;
				target.Position = new Vector2(500f, 100f);
				target.gridPos = new Vector2I(5, 1);
				target.inGame = true;
				characterNode.AddChild(cactus, forceReadableName: false, InternalMode.Disabled);
				characterNode.AddChild(target, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				FireComponent fireComponent = cactus.componentManager?.GetRuntime<FireComponent>("character.fire");
				FireComponentExtendCactus fireComponentExtendCactus = cactus.componentManager?.GetRuntime<FireComponentExtendCactus>("character.fire.cactus");
				Check(fireComponent != null && !fireComponent.IsReleased && fireComponentExtendCactus != null && !fireComponentExtendCactus.IsReleased, "Cactus fire runtimes must be active.");
				if (fireComponent != null && !fireComponent.IsReleased && fireComponentExtendCactus != null && !fireComponentExtendCactus.IsReleased)
				{
					fireComponent.groundRight = 10000f;
					fireComponent.timer = 0f;
					fireComponent.checkInterval = 0;
					int collectionFlag = 9;
					Check(fireComponent.CanFireCheckOnce(null, collectionFlag), "Cactus must acquire a real ground Zombie in its forward ray.");
					fireComponentExtendCactus.IdleProcessing(0.0);
					Check(fireComponentExtendCactus.CanRun(), "Cactus extension must release the FireComponent gate for a ground target.");
					fireComponent.IdleProcessing(0.0);
					Check(fireComponent.StateMachine?.CurrentStateHandle?.StableId == "fire.attack", "Cactus must enter fire.attack; got " + (fireComponent.StateMachine?.CurrentStateHandle?.StableId ?? "<null>") + ".");
					Check(fireComponent.runningCheckId == 1, $"Cactus must use its ground fire check; got {fireComponent.runningCheckId}.");
					return;
				}
				return;
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(cactus))
			{
				cactus.QueueFree();
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
			GD.PushError("[CactusGroundFireRuntimeTest] " + message);
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
