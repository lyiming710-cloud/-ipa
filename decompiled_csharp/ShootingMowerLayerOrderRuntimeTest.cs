using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ShootingMowerLayerOrderRuntimeTest.cs")]
public class ShootingMowerLayerOrderRuntimeTest : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateCharacter = "CreateCharacter";

		public static readonly StringName StartStackMove = "StartStackMove";

		public static readonly StringName StartMove = "StartMove";

		public static readonly StringName CheckLayerOrderAtVisualPosition = "CheckLayerOrderAtVisualPosition";

		public static readonly StringName CheckGpuOrder = "CheckGpuOrder";

		public static readonly StringName GetCellVisualPosition = "GetCellVisualPosition";

		public static readonly StringName FreeCharacter = "FreeCharacter";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static readonly Vector2I SourceGridPosition = new Vector2I(2, 4);

	private static readonly Vector2I FirstTargetGridPosition = new Vector2I(2, 2);

	private static readonly Vector2I SecondTargetGridPosition = new Vector2I(2, 5);

	private static readonly Vector2I FinalTargetGridPosition = new Vector2I(2, 1);

	private static readonly Vector2 TestGridBeginPosition = Vector2.Zero;

	private static readonly Vector2 TestGridSize = new Vector2(100f, 100f);

	private static readonly Vector2I TestGridCount = new Vector2I(9, 5);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		Vector2 previousGridBeginPosition = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridCount = manager?.gridNum ?? Vector2I.Zero;
		ShootingMowerLayerOrderCharacterStub flowerPot = null;
		ShootingMowerLayerOrderCharacterStub plant = null;
		ShootingMowerLayerOrderCharacterStub pumpkin = null;
		try
		{
			Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
			if (!GodotObject.IsInstanceValid(manager))
			{
				return;
			}
			manager.gridBeginPos = TestGridBeginPosition;
			manager.gridSize = TestGridSize;
			manager.gridNum = TestGridCount;
			Vector2 cellVisualPosition = GetCellVisualPosition(SourceGridPosition);
			Vector2 firstTargetPosition = GetCellVisualPosition(FirstTargetGridPosition);
			Vector2 secondTargetPosition = GetCellVisualPosition(SecondTargetGridPosition);
			Vector2 finalTargetPosition = GetCellVisualPosition(FinalTargetGridPosition);
			flowerPot = CreateCharacter("FlowerPot", TowerDefenseEnum.LAYER_GROUNDITEM.PLANT_UNDER, cellVisualPosition);
			plant = CreateCharacter("Plant", TowerDefenseEnum.LAYER_GROUNDITEM.PLANT, cellVisualPosition);
			pumpkin = CreateCharacter("Pumpkin", TowerDefenseEnum.LAYER_GROUNDITEM.PLANT_FRONT, cellVisualPosition);
			for (int frame = 0; frame < 5; frame++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			}
			CheckLayerOrderAtVisualPosition(flowerPot, plant, pumpkin, "warmup");
			StartStackMove(flowerPot, plant, pumpkin, FirstTargetGridPosition, firstTargetPosition);
			CheckLayerOrderAtVisualPosition(flowerPot, plant, pumpkin, "first-move-start");
			for (int frame = 0; frame < 2; frame++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
				CheckLayerOrderAtVisualPosition(flowerPot, plant, pumpkin, $"first-move-frame-{frame}");
			}
			StartStackMove(flowerPot, plant, pumpkin, SecondTargetGridPosition, secondTargetPosition);
			CheckLayerOrderAtVisualPosition(flowerPot, plant, pumpkin, "second-move-start");
			for (int frame = 0; frame < 2; frame++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
				CheckLayerOrderAtVisualPosition(flowerPot, plant, pumpkin, $"second-move-frame-{frame}");
			}
			StartStackMove(flowerPot, plant, pumpkin, FinalTargetGridPosition, finalTargetPosition);
			CheckLayerOrderAtVisualPosition(flowerPot, plant, pumpkin, "final-move-start");
			for (int frame = 0; frame < 40; frame++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
				CheckLayerOrderAtVisualPosition(flowerPot, plant, pumpkin, $"final-move-frame-{frame}");
			}
			Check(flowerPot.GetLogicalGlobalPosition().IsEqualApprox(finalTargetPosition) && plant.GetLogicalGlobalPosition().IsEqualApprox(finalTargetPosition) && pumpkin.GetLogicalGlobalPosition().IsEqualApprox(finalTargetPosition), "The whole shooting-mode stack must finish at the latest requested target position.");
			Check(flowerPot.gridPos == FinalTargetGridPosition && plant.gridPos == FinalTargetGridPosition && pumpkin.gridPos == FinalTargetGridPosition, "Logical grid authority must switch to the latest target before the visual tween finishes.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[ShootingMowerLayerOrderRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			FreeCharacter(flowerPot);
			FreeCharacter(plant);
			FreeCharacter(pumpkin);
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.gridBeginPos = previousGridBeginPosition;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridCount;
			}
		}
		bool flag = _failures == 0;
		GD.Print($"SHOOTING_MOWER_LAYER_ORDER_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private ShootingMowerLayerOrderCharacterStub CreateCharacter(string characterName, TowerDefenseEnum.LAYER_GROUNDITEM layer, Vector2 position)
	{
		ShootingMowerLayerOrderCharacterStub shootingMowerLayerOrderCharacterStub = new ShootingMowerLayerOrderCharacterStub
		{
			Name = characterName,
			itemLayer = layer,
			gridPos = SourceGridPosition,
			Position = position
		};
		AddChild(shootingMowerLayerOrderCharacterStub, forceReadableName: false, InternalMode.Disabled);
		shootingMowerLayerOrderCharacterStub.SetPhysicsProcess(enable: false);
		string text;
		if (characterName == "FlowerPot")
		{
			text = "Pot/Pot";
		}
		else
		{
			text = ((!(characterName == "Plant")) ? "Pumpkin/Pumpkin" : "PeaShooter/PeaShooter");
		}
		string text2 = text;
		shootingMowerLayerOrderCharacterStub.sprite = GD.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/" + text2 + ".tscn").Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		shootingMowerLayerOrderCharacterStub.AddChild(shootingMowerLayerOrderCharacterStub.sprite, forceReadableName: false, InternalMode.Disabled);
		return shootingMowerLayerOrderCharacterStub;
	}

	private static void StartStackMove(ShootingMowerLayerOrderCharacterStub flowerPot, ShootingMowerLayerOrderCharacterStub plant, ShootingMowerLayerOrderCharacterStub pumpkin, Vector2I targetGridPosition, Vector2 targetPosition)
	{
		StartMove(flowerPot, targetGridPosition, targetPosition);
		StartMove(plant, targetGridPosition, targetPosition);
		StartMove(pumpkin, targetGridPosition, targetPosition);
	}

	private static void StartMove(ShootingMowerLayerOrderCharacterStub character, Vector2I targetGridPosition, Vector2 targetPosition)
	{
		character.gridPos = targetGridPosition;
		TowerDefenseCellInstance.CreateCharacterCellMoveTween(character, targetPosition);
	}

	private void CheckLayerOrderAtVisualPosition(ShootingMowerLayerOrderCharacterStub flowerPot, ShootingMowerLayerOrderCharacterStub plant, ShootingMowerLayerOrderCharacterStub pumpkin, string phase)
	{
		int y = TowerDefenseManager.Instance.GetMapGridPos(plant.GetLogicalGlobalPosition()).Y;
		int num = y * 15;
		bool flag = flowerPot.ZIndex == num + 2 && plant.ZIndex == num + 4 && pumpkin.ZIndex == num + 5;
		bool flag2 = flowerPot.ZIndex < plant.ZIndex && plant.ZIndex < pumpkin.ZIndex;
		Check(flag & flag2, $"The flower pot, plant, and pumpkin must follow visual row {y} without reordering during {phase}; z={flowerPot.ZIndex},{plant.ZIndex},{pumpkin.ZIndex}.");
		CheckGpuOrder(flowerPot, phase);
		CheckGpuOrder(plant, phase);
		CheckGpuOrder(pumpkin, phase);
	}

	private void CheckGpuOrder(ShootingMowerLayerOrderCharacterStub character, string phase)
	{
		AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult = character.sprite.TryBuildCrowdRenderState(out var state);
		int num = character.ZIndex + character.sprite.ZIndex;
		Check(adobeAnimateCrowdRenderStateResult == AdobeAnimateCrowdRenderStateResult.Submitted && state.Mode == AdobeAnimateCrowdRenderMode.GpuGraph && state.EffectiveZIndex == num, $"GPU_ORDER {character.Name} phase={phase} result={adobeAnimateCrowdRenderStateResult} mode={state?.Mode} gpuZ={state?.EffectiveZIndex} expectedZ={num}");
	}

	private static Vector2 GetCellVisualPosition(Vector2I gridPosition)
	{
		return TestGridBeginPosition + new Vector2(((float)gridPosition.X - 0.5f) * TestGridSize.X, ((float)gridPosition.Y - 0.5f) * TestGridSize.Y);
	}

	private static void FreeCharacter(ShootingMowerLayerOrderCharacterStub character)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			character.CancelCellMoveTween();
			character.Free();
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[ShootingMowerLayerOrderRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartStackMove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "flowerPot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "pumpkin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "targetGridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "targetPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartMove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "targetGridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "targetPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckLayerOrderAtVisualPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "flowerPot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "pumpkin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckGpuOrder, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCellVisualPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FreeCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.CreateCharacter && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<ShootingMowerLayerOrderCharacterStub>(CreateCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.LAYER_GROUNDITEM>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.StartStackMove && args.Count == 5)
		{
			StartStackMove(VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[0]), VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[1]), VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[2]), VariantUtils.ConvertTo<Vector2I>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartMove && args.Count == 3)
		{
			StartMove(VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckLayerOrderAtVisualPosition && args.Count == 4)
		{
			CheckLayerOrderAtVisualPosition(VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[0]), VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[1]), VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckGpuOrder && args.Count == 2)
		{
			CheckGpuOrder(VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCellVisualPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCellVisualPosition(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.FreeCharacter && args.Count == 1)
		{
			FreeCharacter(VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[0]));
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
		if (method == MethodName.StartStackMove && args.Count == 5)
		{
			StartStackMove(VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[0]), VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[1]), VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[2]), VariantUtils.ConvertTo<Vector2I>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartMove && args.Count == 3)
		{
			StartMove(VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCellVisualPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCellVisualPosition(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.FreeCharacter && args.Count == 1)
		{
			FreeCharacter(VariantUtils.ConvertTo<ShootingMowerLayerOrderCharacterStub>(in args[0]));
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
		if (method == MethodName.CreateCharacter)
		{
			return true;
		}
		if (method == MethodName.StartStackMove)
		{
			return true;
		}
		if (method == MethodName.StartMove)
		{
			return true;
		}
		if (method == MethodName.CheckLayerOrderAtVisualPosition)
		{
			return true;
		}
		if (method == MethodName.CheckGpuOrder)
		{
			return true;
		}
		if (method == MethodName.GetCellVisualPosition)
		{
			return true;
		}
		if (method == MethodName.FreeCharacter)
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
