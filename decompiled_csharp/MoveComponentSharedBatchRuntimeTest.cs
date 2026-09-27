using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/MoveComponentSharedBatchRuntimeTest.cs")]
public class MoveComponentSharedBatchRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Check = "Check";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	public override async void _Ready()
	{
		Node2D host = new Node2D();
		MoveComponent movement = new MoveComponent();
		host.AddChild(movement, forceReadableName: false, InternalMode.Disabled);
		AddChild(host, forceReadableName: false, InternalMode.Disabled);
		try
		{
			await WaitPhysicsFrames(3);
			Check(!movement.HasActiveMovement, "new movement component should start idle");
			Check(!movement.IsPhysicsProcessing(), "idle movement component kept a per-node physics callback");
			Vector2 start = host.GlobalPosition;
			movement.velocity = new Vector2(120f, 0f);
			await WaitPhysicsFrames(4);
			Check(host.GlobalPosition.X > start.X + 0.01f, "direct velocity assignment did not reactivate movement");
			Check(GodotObject.IsInstanceValid(ComponentPhysicsBatch.Instance), "shared component dispatcher was not mounted");
			Check(!movement.IsPhysicsProcessing(), "active movement used its per-node callback instead of the shared dispatcher");
			movement.MoveClear();
			Vector2 stopped = host.GlobalPosition;
			await WaitPhysicsFrames(3);
			Check(host.GlobalPosition.IsEqualApprox(stopped), "MoveClear did not stop movement");
			Check(!movement.IsPhysicsProcessing(), "MoveClear did not disable the per-node fallback");
			movement.gravity = 240.0;
			await WaitPhysicsFrames(4);
			Check(host.GlobalPosition.Y > stopped.Y + 0.01f, "direct gravity assignment did not reactivate movement");
			movement.MoveClear();
		}
		catch (Exception ex)
		{
			_failures.Add("runtime exception: " + ex);
		}
		finally
		{
			host.QueueFree();
		}
		Finish();
	}

	private async Task WaitPhysicsFrames(int count)
	{
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string failure)
	{
		if (!condition)
		{
			_failures.Add(failure);
		}
	}

	private void Finish()
	{
		for (int i = 0; i < _failures.Count; i++)
		{
			GD.PrintErr("MOVE_COMPONENT_SHARED_BATCH_FAILURE " + _failures[i]);
		}
		bool flag = _failures.Count == 0;
		GD.Print($"MOVE_COMPONENT_SHARED_BATCH_RESULT passed={flag} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "failure", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
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
		if (method == MethodName.Finish)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
