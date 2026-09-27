using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/YetiDiggerTurnDirectionRuntimeTest.cs")]
public class YetiDiggerTurnDirectionRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CheckRepeatedDigEntry = "CheckRepeatedDigEntry";

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

	private const string YetiDiggerScenePath = "res://Asset/Anime/Character/Zombie/Chapter2/YetiDigger/Scene/TowerDefenseZombieYetiDigger.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		YetiDiggerTurnDirectionControlStub control = null;
		TowerDefenseZombieYetiDigger yeti = null;
		Node2D movementSource = null;
		try
		{
			Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
			if (!GodotObject.IsInstanceValid(manager))
			{
				throw new InvalidOperationException("TowerDefenseManager was unavailable.");
			}
			control = new YetiDiggerTurnDirectionControlStub
			{
				Name = "YetiDiggerTurnDirectionControl",
				isGameRunning = false,
				isInit = true
			};
			AddChild(control, forceReadableName: false, InternalMode.Disabled);
			control.characterNode = new Node2D
			{
				Name = "CharacterNode"
			};
			control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
			manager.currentControl = control;
			manager.gridBeginPos = Vector2.Zero;
			manager.gridSize = new Vector2(100f, 76f);
			manager.gridNum = new Vector2I(9, 5);
			yeti = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter2/YetiDigger/Scene/TowerDefenseZombieYetiDigger.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombieYetiDigger>(PackedScene.GenEditState.Disabled);
			Check(GodotObject.IsInstanceValid(yeti) && yeti.config?.name == "ZombieYetiDigger", "The authored Yeti Digger scene must instantiate.");
			if (!GodotObject.IsInstanceValid(yeti))
			{
				throw new InvalidOperationException("ZombieYetiDigger did not instantiate.");
			}
			yeti.inGame = false;
			yeti.editorPreviewMode = true;
			yeti.gridPos = new Vector2I(5, 3);
			yeti.GlobalPosition = new Vector2(500f, 228f);
			control.characterNode.AddChild(yeti, forceReadableName: false, InternalMode.Disabled);
			await WaitFrames(5);
			yeti.ProcessMode = ProcessModeEnum.Disabled;
			GroundMoveComponent groundMoveComponent = (yeti.groundMoveComponent = yeti.componentManager?.GetRuntime<GroundMoveComponent>());
			Check(groundMoveComponent != null && !groundMoveComponent.IsReleased && groundMoveComponent.HasMovementSource, "雪人矿工的地面移动组件必须解析到实际动画移动源。");
			if (groundMoveComponent == null || groundMoveComponent.IsReleased)
			{
				throw new InvalidOperationException("Yeti GroundMoveComponent was unavailable.");
			}
			yeti.attackComponent = yeti.componentManager.GetRuntime<AttackComponent>("character.attack.0");
			CheckRepeatedDigEntry(yeti);
			yeti.Scale = Vector2.One;
			movementSource = new Node2D
			{
				Name = "DeterministicGroundSlot"
			};
			AddChild(movementSource, forceReadableName: false, InternalMode.Disabled);
			groundMoveComponent.groundLayerName = null;
			groundMoveComponent.groundNode = movementSource;
			groundMoveComponent.delay = 0f;
			groundMoveComponent.SetAlive(true);
			yeti.sprite.pause = false;
			yeti.sprite.blend = false;
			groundMoveComponent.RefreshDirectionCache();
			movementSource.Position = Vector2.Zero;
			groundMoveComponent.BatchUpdate(0.016);
			float x = ((yeti.spriteGroup.Scale.X * yeti.Scale.X * yeti.sprite.Scale.X >= 0f) ? 5f : (-5f));
			float y = yeti.GlobalPosition.Y;
			float x2 = yeti.GlobalPosition.X;
			movementSource.Position += new Vector2(x, 0f);
			groundMoveComponent.BatchUpdate(0.016);
			Check(yeti.GlobalPosition.X < x2 - 0.01f, "The primed real root-motion path must initially move the Yeti left.");
			float num = Mathf.Sign(yeti.Scale.X);
			yeti.TurnAroundAndRefreshGroundMove();
			Check((float)Mathf.Sign(yeti.Scale.X) == 0f - num, "The Yeti turnaround must flip its visual X direction.");
			groundMoveComponent.BatchUpdate(0.016);
			float num2 = 0f;
			for (int i = 0; i < 3; i++)
			{
				float x3 = yeti.GlobalPosition.X;
				movementSource.Position += new Vector2(x, 0f);
				groundMoveComponent.BatchUpdate(0.016);
				float num3 = yeti.GlobalPosition.X - x3;
				if (Mathf.Abs(num3) > 0.01f)
				{
					num2 = num3;
					break;
				}
			}
			Check(num2 > 0.01f, $"The first effective root-motion step after turning must move right; deltaX={num2}.");
			Check(Mathf.Abs(yeti.GlobalPosition.Y - y) < 0.01f, "Turning must not introduce Y-axis movement while moveYAxis is disabled.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[YetiDiggerTurnDirectionRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(yeti))
			{
				yeti.QueueFree();
			}
			if (GodotObject.IsInstanceValid(movementSource))
			{
				movementSource.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 12;
		GD.Print($"YETI_DIGGER_TURN_DIRECTION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void CheckRepeatedDigEntry(TowerDefenseZombieYetiDigger yeti)
	{
		yeti.Walk();
		Check(yeti.CurrentStateHandle?.StableId == "zombie.yeti_digger.dig" && !yeti.digOver, "首次行走必须进入未完成挖掘的地下状态。");
		yeti.Walk();
		Check(yeti.CurrentStateHandle?.StableId == "zombie.yeti_digger.dig" && !yeti.digOver, "重复行走请求不能把挖地状态重入误判为出土。");
		yeti.Idle();
		Check(!yeti.digOver, "挖地被临时中断不能标记为已出土。");
		yeti.Walk();
		Check(yeti.CurrentStateHandle?.StableId == "zombie.yeti_digger.dig" && !yeti.digOver, "中断后恢复行走必须继续挖地。");
		float x = (float)TowerDefenseManager.Instance.GetMapGroundLeft() + TowerDefenseManager.Instance.GetMapGridSize().X - 5f;
		yeti.SetGlobalPositionForPhysicsFrame(new Vector2(x, 228f), TowerDefenseProcessModeDispatch.CurrentPhysicsFrame);
		yeti.DigProcessing(0.0);
		Check(yeti.digOver && yeti.CurrentStateHandle?.StableId != "zombie.yeti_digger.dig", "重复行走后到达左侧边界仍必须正常出土。");
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
			GD.PushError("[YetiDiggerTurnDirectionRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckRepeatedDigEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "yeti", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.CheckRepeatedDigEntry && args.Count == 1)
		{
			CheckRepeatedDigEntry(VariantUtils.ConvertTo<TowerDefenseZombieYetiDigger>(in args[0]));
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
		if (method == MethodName.CheckRepeatedDigEntry)
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
