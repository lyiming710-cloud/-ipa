using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/VaseEntryAnimationVerticalStabilityRuntimeTest.cs")]
public class VaseEntryAnimationVerticalStabilityRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ReadCachedRenderTransform = "ReadCachedRenderTransform";

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

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		_ = 1;
		try
		{
			await VerifyEntryTweenOwnsVerticalMotion();
			await VerifyNestedBackCompletesScaleRefresh();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[VaseEntryVerticalStability] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"VASE_ENTRY_VERTICAL_STABILITY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyEntryTweenOwnsVerticalMotion()
	{
		VaseEntryAnimationVerticalStabilityProbeCharacter character = new VaseEntryAnimationVerticalStabilityProbeCharacter
		{
			Name = "VaseEntryProbe",
			groundHeight = 12.0,
			gravityUse = true,
			ySpeed = 0.0
		};
		character.AddChild(character.transformPoint = new Marker2D
		{
			Name = "TransformPoint"
		}, forceReadableName: false, InternalMode.Disabled);
		AddChild(character, forceReadableName: false, InternalMode.Disabled);
		ComponentManager manager = new ComponentManager();
		EntryAnimationComponentDefinition definition = new EntryAnimationComponentDefinition
		{
			defaultFallHeight = 120f,
			defaultFallDelay = 0.2f,
			defaultFallDuration = 0.1f,
			impactDuration = 0.05f,
			settleDuration = 0.05f
		};
		EntryAnimationComponent runtime = new EntryAnimationComponent();
		runtime.Bind(manager, character, definition);
		runtime.Activate();
		runtime.PlayConfiguredFallBounce();
		Check(!character.gravityUse, "The entry tween must suppress base gravity while it owns z.");
		Check(!character.isGround, "The vase must remain marked airborne during the entry animation.");
		Check(Mathf.IsZeroApprox((float)character.ySpeed), "Entry must clear inherited vertical speed before the delayed tween.");
		double z = character.z;
		character.PhysiceUpdate(0.1f);
		Check(Mathf.IsEqualApprox((float)character.z, (float)z), "Base physics changed z during the random entry delay.");
		Check(Mathf.IsZeroApprox((float)character.ySpeed), "Base physics accumulated vertical speed during the random entry delay.");
		await ToSignal(GetTree().CreateTimer(0.6), SceneTreeTimer.SignalName.Timeout);
		Check(character.gravityUse, "The original gravity setting must be restored after entry completes.");
		Check(character.isGround, "Entry completion must restore the grounded state.");
		Check(Mathf.IsEqualApprox((float)character.z, (float)character.groundHeight), "Entry completion must settle exactly at the current ground height.");
		runtime.Release();
		character.QueueFree();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}

	private async Task VerifyNestedBackCompletesScaleRefresh()
	{
		Node2D transformPoint = new Node2D
		{
			Name = "VaseTransformPoint"
		};
		PackedScene packedScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Vase/Normal/VaseNormal.tscn");
		AdobeAnimateSprite front = packedScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		transformPoint.AddChild(front, forceReadableName: false, InternalMode.Disabled);
		AddChild(transformPoint, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		AdobeAnimateSprite back = front.GetNode<AdobeAnimateSprite>("Node2D/Back");
		ReadCachedRenderTransform(front);
		ReadCachedRenderTransform(back);
		transformPoint.Scale = new Vector2(1.5f, 0.5f);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		Check(ReadCachedRenderTransform(front).IsEqualApprox(front.GlobalTransform), "The front vase layer must refresh at the impact scale.");
		Check(ReadCachedRenderTransform(back).IsEqualApprox(back.GlobalTransform), "The nested Back layer must refresh at the impact scale.");
		transformPoint.Scale = Vector2.One;
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		Check(ReadCachedRenderTransform(front).IsEqualApprox(front.GlobalTransform), "The front vase layer must refresh at the settled scale.");
		Check(ReadCachedRenderTransform(back).IsEqualApprox(back.GlobalTransform), "The nested Back layer must finish refreshing at the settled scale.");
		transformPoint.QueueFree();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}

	private static Transform2D ReadCachedRenderTransform(AdobeAnimateSprite sprite)
	{
		System.Reflection.MethodInfo? method = typeof(AdobeAnimateSprite).GetMethod("GetCachedGlobalTransformForRender", BindingFlags.Instance | BindingFlags.NonPublic);
		if (method == null)
		{
			throw new MissingMethodException(typeof(AdobeAnimateSprite).FullName, "GetCachedGlobalTransformForRender");
		}
		return (Transform2D)method.Invoke(sprite, null);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[VaseEntryVerticalStability] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(3)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ReadCachedRenderTransform, new Godot.Bridge.PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ReadCachedRenderTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ReadCachedRenderTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
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
		if (method == MethodName.ReadCachedRenderTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ReadCachedRenderTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
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
		if (method == MethodName.ReadCachedRenderTransform)
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
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
