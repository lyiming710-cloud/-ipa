using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BulletFieldAnimationMeshHandoffRuntimeTest.cs")]
public class BulletFieldAnimationMeshHandoffRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadProjectileSprite = "LoadProjectileSprite";

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

	private const string FourSliceScenePath = "res://Asset/Config/Projectile/Pea/Sprite/FirePea/FirePea.tscn";

	private const string FiveSliceScenePath = "res://Asset/Config/Projectile/Puff/Sprite/HypnoPuff/HypnoPuff.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot rdStatisticsBefore = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.Statistics;
		AnimateMultiMeshRenderer animateMultiMeshRenderer = null;
		AdobeAnimateSprite adobeAnimateSprite = null;
		AdobeAnimateSprite adobeAnimateSprite2 = null;
		try
		{
			adobeAnimateSprite = LoadProjectileSprite("res://Asset/Config/Projectile/Pea/Sprite/FirePea/FirePea.tscn");
			adobeAnimateSprite2 = LoadProjectileSprite("res://Asset/Config/Projectile/Puff/Sprite/HypnoPuff/HypnoPuff.tscn");
			Check(GodotObject.IsInstanceValid(adobeAnimateSprite), "The four-slice production projectile must instantiate.");
			Check(GodotObject.IsInstanceValid(adobeAnimateSprite2), "The five-slice production projectile must instantiate.");
			if (!GodotObject.IsInstanceValid(adobeAnimateSprite) || !GodotObject.IsInstanceValid(adobeAnimateSprite2))
			{
				throw new InvalidOperationException("Production projectile scenes could not be instantiated.");
			}
			animateMultiMeshRenderer = new AnimateMultiMeshRenderer();
			AddChild(animateMultiMeshRenderer, forceReadableName: false, InternalMode.Disabled);
			int layer = 9;
			int num = animateMultiMeshRenderer.RegisterDefinition(adobeAnimateSprite.flashAnimeData, adobeAnimateSprite.clip, layer);
			Check(num >= 0, "The four-slice projectile must register for Compact Crowd rendering.");
			animateMultiMeshRenderer.BeginFrame();
			animateMultiMeshRenderer.DrawInstance(num, 1, new Transform2D(0f, new Vector2(200f, 200f)), 0f, Colors.White);
			animateMultiMeshRenderer.EndFrame();
			Check(animateMultiMeshRenderer.GetVisibleInstanceCountForTest() == 1, "The first projectile must be visible before the mesh grows.");
			AnimateMultiMeshRenderer.Definition definition = animateMultiMeshRenderer.GetDefinition(num);
			ArrayMesh arrayMesh = definition?.resourceGroup?.Mesh;
			Check(GodotObject.IsInstanceValid(arrayMesh) && arrayMesh.GetRid().IsValid, "The currently published projectile mesh must be valid.");
			int num2 = animateMultiMeshRenderer.RegisterDefinition(adobeAnimateSprite2.flashAnimeData, adobeAnimateSprite2.clip, layer);
			Check(num2 >= 0, "The five-slice projectile must register for Compact Crowd rendering.");
			animateMultiMeshRenderer.PrewarmDefinitionRows(num2, 1);
			AnimateMultiMeshRenderer.Definition definition2 = animateMultiMeshRenderer.GetDefinition(num2);
			Check(definition?.resourceGroup == definition2?.resourceGroup, "The two production projectiles must exercise one shared resource group.");
			Check(definition2 != null && definition2.resourceGroup?.MeshMaxSlices >= 5, "The later projectile must expand the shared mesh.");
			Check(animateMultiMeshRenderer.GetRetiredMeshCountForTest() == 1, "The previously visible mesh must remain retained before replacement publication.");
			Check(GodotObject.IsInstanceValid(arrayMesh) && arrayMesh.GetRid().IsValid, "The previously visible mesh must not be released during definition prewarm.");
			animateMultiMeshRenderer.BeginFrame();
			animateMultiMeshRenderer.DrawInstance(num, 1, new Transform2D(0f, new Vector2(200f, 200f)), 1f, Colors.White);
			animateMultiMeshRenderer.DrawInstance(num2, 1, new Transform2D(0f, new Vector2(260f, 200f)), 1f, Colors.White);
			animateMultiMeshRenderer.EndFrame();
			Check(animateMultiMeshRenderer.GetVisibleInstanceCountForTest() == 2, "The replacement frame must publish both projectile definitions.");
			Check(animateMultiMeshRenderer.GetRetiredMeshCountForTest() == 0, "The old mesh must be released only after replacement publication.");
			bool condition = DrawGpuClockSimpleFrame(animateMultiMeshRenderer, num, 2, 5000, out var context);
			Check(condition, "The high-load GPU-clock projectile frame must publish every root.");
			Check(animateMultiMeshRenderer.GetVisibleInstanceCountForTest() == 5000, "The high-load frame must expose every projectile.");
			int valueOrDefault = (context.Bucket?.RenderBucket?.GpuInstanceCapacityForTest).GetValueOrDefault();
			Check(valueOrDefault >= 5000, "The high-load bucket must allocate a stable GPU capacity.");
			bool condition2 = DrawGpuClockSimpleFrame(animateMultiMeshRenderer, num, 2, 4999, out var context2);
			Check(condition2, "The frame after one projectile unload must publish every remaining root.");
			Check(animateMultiMeshRenderer.GetVisibleInstanceCountForTest() == 4999, "Unloading one projectile must only reduce the visible root count.");
			Check(context.Bucket == context2.Bucket, "Projectile unload must reuse the same exact-Z bucket.");
			BulletFieldAnimationMeshHandoffRuntimeTest bulletFieldAnimationMeshHandoffRuntimeTest = this;
			AnimateMultiMeshRenderer.UnifiedBucket bucket = context2.Bucket;
			bulletFieldAnimationMeshHandoffRuntimeTest.Check(bucket != null && bucket.RenderBucket?.GpuInstanceCapacityForTest == valueOrDefault, "Projectile unload above 4K must not rebuild or shrink the MultiMesh instance storage.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BulletFieldAnimationMeshHandoffRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(animateMultiMeshRenderer))
			{
				animateMultiMeshRenderer.QueueFree();
			}
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.Free();
			}
			if (GodotObject.IsInstanceValid(adobeAnimateSprite2))
			{
				adobeAnimateSprite2.Free();
			}
		}
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot statistics = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.Statistics;
		Check(statistics.AppliedUploadCount - rdStatisticsBefore.AppliedUploadCount >= 4, "Every published BulletField frame must execute its RD BufferUpdate command.");
		Check(statistics.AppliedVisibilityCount - rdStatisticsBefore.AppliedVisibilityCount >= 4, "Every published BulletField frame must publish visibility on the render thread.");
		Check(statistics.FailedBatchCount == rdStatisticsBefore.FailedBatchCount, "BulletField RD batches must complete without failures.");
		Check(statistics.BufferUpdateFailureCount == rdStatisticsBefore.BufferUpdateFailureCount, "BulletField RD BufferUpdate calls must complete without errors.");
		Check(statistics.InvalidTargetCount == rdStatisticsBefore.InvalidTargetCount, "BulletField must keep every queued MultiMesh target valid until render-thread application.");
		Check(statistics.RenderingDeviceUnavailableCount == rdStatisticsBefore.RenderingDeviceUnavailableCount, "BulletField must use the available global screen RenderingDevice.");
		Check(statistics.CurrentQueueDepth <= rdStatisticsBefore.CurrentQueueDepth, "BulletField RD queue must drain before the lifecycle test completes.");
		bool flag = _failures == 0;
		GD.Print($"BULLET_FIELD_ANIMATION_MESH_HANDOFF_RESULT passed={flag} checks={_checks} failures={_failures} rdUploads={statistics.AppliedUploadCount - rdStatisticsBefore.AppliedUploadCount} rdVisibility={statistics.AppliedVisibilityCount - rdStatisticsBefore.AppliedVisibilityCount} rdFailures={statistics.FailedBatchCount - rdStatisticsBefore.FailedBatchCount} rdQueueDepth={statistics.CurrentQueueDepth} renderer={RenderingServer.GetCurrentRenderingMethod()} driver={RenderingServer.GetCurrentRenderingDriverName()}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static AdobeAnimateSprite LoadProjectileSprite(string path)
	{
		return GD.Load<PackedScene>(path)?.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
	}

	private static bool DrawGpuClockSimpleFrame(AnimateMultiMeshRenderer renderer, int definitionId, int zGroup, int count, out AnimateMultiMeshRenderer.SimpleDrawContext context)
	{
		renderer.BeginFrame();
		if (!renderer.TryPrepareSimpleDrawContext(definitionId, zGroup, count, out context))
		{
			renderer.EndFrame();
			return false;
		}
		bool flag = true;
		for (int i = 0; i < count; i++)
		{
			flag &= renderer.DrawPreparedSimpleInstance(in context, new Transform2D(0f, new Vector2(100f + (float)i * 0.01f, 320f)), i % 25, 24f);
		}
		renderer.EndFrame();
		return flag;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BulletFieldAnimationMeshHandoffRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadProjectileSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.LoadProjectileSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(LoadProjectileSprite(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.LoadProjectileSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(LoadProjectileSprite(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.LoadProjectileSprite)
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
