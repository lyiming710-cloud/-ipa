using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateRuntimeRestartImmediateProbe.cs")]
public class AdobeAnimateRuntimeRestartImmediateProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CountVisibleImmediateFallbackInstances = "CountVisibleImmediateFallbackInstances";

		public static readonly StringName CountVisibleRuntimeCrowdInstances = "CountVisibleRuntimeCrowdInstances";

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

	private const string JacksonXAnimationScenePath = "res://Asset/Anime/Character/Zombie/Challenge/JacksonX/ZombieJacksonX.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		try
		{
			await RunProbe();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[AdobeAnimateRuntimeRestartImmediateProbe] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"ADOBE_ANIMATE_RUNTIME_RESTART_IMMEDIATE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private async Task RunProbe()
	{
		PackedScene packedScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Challenge/JacksonX/ZombieJacksonX.tscn");
		Check(GodotObject.IsInstanceValid(packedScene), "JacksonX animation scene must load.");
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			return;
		}
		AdobeAnimateSprite sprite = packedScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		sprite.Name = "JacksonXRestartProbe";
		sprite.Position = new Vector2(320f, 220f);
		AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
		await WaitProcessFrames(4);
		int num = CountVisibleRuntimeCrowdInstances(GetTree().Root);
		Check(num > 0, "The live animation must publish a runtime Crowd batch.");
		AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
		await WaitProcessFrames(4);
		int num2 = CountVisibleRuntimeCrowdInstances(GetTree().Root);
		Check(num2 > 0, "Clearing GPU graph caches must rebuild still-live animation output on the next transaction.");
		int blankTransitionFrames = 0;
		int minimumVisibleInstances = 2147483647;
		for (int frame = 0; frame < 120; frame++)
		{
			if (frame % 30 == 0)
			{
				bool flag = frame / 30 % 2 == 0;
				sprite.SetAnimation(flag ? "PointDown" : "Walk", loop: true, 0.2);
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			int num3 = CountVisibleRuntimeCrowdInstances(GetTree().Root) + CountVisibleImmediateFallbackInstances(GetTree().Root);
			minimumVisibleInstances = Math.Min(minimumVisibleInstances, num3);
			if (num3 <= 0)
			{
				blankTransitionFrames++;
			}
		}
		Check(blankTransitionFrames == 0, $"Live ClipBlend routing must not publish blank animation frames; blank={blankTransitionFrames}, minimumVisible={minimumVisibleInstances}.");
		sprite.SetAnimation("PointDown", loop: false);
		for (int frame = 0; frame < 360; frame++)
		{
			if (sprite.clipOver)
			{
				break;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		Check(sprite.clipOver, "The non-looping PointDown clip must reach its retained final pose.");
		await WaitProcessFrames(4);
		int num4 = CountVisibleImmediateFallbackInstances(GetTree().Root);
		Check(num4 > 0, "Stopping a non-looping runtime clip must exercise the persistent immediate fallback path.");
		sprite.SetAnimation("Walk");
		await WaitProcessFrames(4);
		Check(sprite.IsRuntimeActive, "Restarting Walk must register the sprite with the runtime manager.");
		int num5 = CountVisibleImmediateFallbackInstances(GetTree().Root);
		Check(num5 == 0, $"Restarting managed animation must release the old immediate pose, got {num5} visible instances.");
		sprite.forceLocalRender = true;
		sprite.SetFrozenPreview(frozen: true);
		await WaitProcessFrames(2);
		int num6 = CountVisibleImmediateFallbackInstances(GetTree().Root);
		Check(num6 > 0, "A frozen local preview must retain one immediate pose.");
		AdobeAnimateRenderManager.ReleaseImmediateSubmission(sprite);
		Check(CountVisibleImmediateFallbackInstances(GetTree().Root) == 0, "The visibility-loss simulation must release the frozen immediate pose.");
		sprite.EnsureFrozenPreviewRenderSubmission();
		await WaitProcessFrames(2);
		Check(CountVisibleImmediateFallbackInstances(GetTree().Root) > 0, "A visible frozen preview must be able to republish without resetting playback.");
		sprite.QueueFree();
		await WaitProcessFrames(2);
		int num7 = CountVisibleImmediateFallbackInstances(GetTree().Root);
		Check(num7 == 0, $"Freeing the sprite must remove its retained immediate pose, got {num7} visible instances.");
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private static int CountVisibleImmediateFallbackInstances(Node node)
	{
		int num = 0;
		if (node is AdobeAnimateMultiMeshBatcher adobeAnimateMultiMeshBatcher && node.Name.ToString().StartsWith("AdobeAnimateSnapshotFallbackZ_", StringComparison.Ordinal))
		{
			num += adobeAnimateMultiMeshBatcher.GetVisibleInstanceCountForTest();
		}
		foreach (Node child in node.GetChildren())
		{
			num += CountVisibleImmediateFallbackInstances(child);
		}
		return num;
	}

	private static int CountVisibleRuntimeCrowdInstances(Node node)
	{
		int num = 0;
		if (node is MultiMeshInstance2D multiMeshInstance2D && node.Name.ToString().StartsWith("AdobeAnimateCrowdZ_", StringComparison.Ordinal) && GodotObject.IsInstanceValid(multiMeshInstance2D.Multimesh))
		{
			num += multiMeshInstance2D.Multimesh.VisibleInstanceCount;
		}
		foreach (Node child in node.GetChildren())
		{
			num += CountVisibleRuntimeCrowdInstances(child);
		}
		return num;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[AdobeAnimateRuntimeRestartImmediateProbe] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountVisibleImmediateFallbackInstances, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CountVisibleRuntimeCrowdInstances, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.CountVisibleImmediateFallbackInstances && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountVisibleImmediateFallbackInstances(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CountVisibleRuntimeCrowdInstances && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountVisibleRuntimeCrowdInstances(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.CountVisibleImmediateFallbackInstances && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountVisibleImmediateFallbackInstances(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CountVisibleRuntimeCrowdInstances && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountVisibleRuntimeCrowdInstances(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.CountVisibleImmediateFallbackInstances)
		{
			return true;
		}
		if (method == MethodName.CountVisibleRuntimeCrowdInstances)
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
