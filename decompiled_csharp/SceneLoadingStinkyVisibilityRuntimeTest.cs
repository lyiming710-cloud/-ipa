using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/SceneLoadingStinkyVisibilityRuntimeTest.cs")]
public class SceneLoadingStinkyVisibilityRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CountPublishedInstances = "CountPublishedInstances";

		public static readonly StringName CountChangedPixels = "CountChangedPixels";

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

	private const string SceneLoadingPath = "res://Core/SceneManager/SceneLoadeing/SceneLoading.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		AdobeAnimateRenderBackend previousBackend = Global.Instance.adobeAnimateRenderBackend;
		PackedScene scene = null;
		SceneLoading loading = null;
		try
		{
			_ = 2;
			try
			{
				Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
				AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
				AdobeAnimateDefinitionCache.Clear();
				scene = ResourceLoader.Load<PackedScene>("res://Core/SceneManager/SceneLoadeing/SceneLoading.tscn", null, ResourceLoader.CacheMode.Reuse);
				Check(GodotObject.IsInstanceValid(scene), "The production scene-loading overlay must load.");
				loading = scene?.Instantiate<SceneLoading>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(loading), "The production scene-loading overlay must instantiate.");
				if (!GodotObject.IsInstanceValid(loading))
				{
					throw new InvalidOperationException("The scene-loading overlay fixture could not be created.");
				}
				AdobeAnimateSpriteBase stinky = loading.GetNodeOrNull<AdobeAnimateSpriteBase>("%Stinky");
				Check(GodotObject.IsInstanceValid(stinky), "The loading overlay must contain the Stinky animation.");
				if (!GodotObject.IsInstanceValid(stinky))
				{
					throw new InvalidOperationException("The loading Stinky animation is unavailable.");
				}
				stinky.flashAnimeData = stinky.flashAnimeData?.Duplicate(deep: true) as AdobeAnimateData;
				AddChild(loading, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(40);
				loading.GetNode<Timer>("Timer").Stop();
				Control nodeOrNull = loading.GetNodeOrNull<Control>("%StinkyRenderMount");
				Check(stinky.forceLocalRender && GodotObject.IsInstanceValid(nodeOrNull), "SceneLoading must route Stinky through its CanvasLayer-local render mount.");
				stinky.SetAnimation("Crawl", loop: false);
				stinky.pause = true;
				stinky.Visible = true;
				stinky.Scale = Vector2.One;
				loading.background.Modulate = Colors.White;
				await WaitFrames(8);
				Check(stinky.IsVisibleInTree(), "The loading Stinky animation must be logically visible.");
				int publishedInstances = CountPublishedInstances(loading);
				Check(publishedInstances > 0, $"The Stinky-local render mount must publish animation instances; count={publishedInstances}.");
				using Image visibleImage = GetViewport().GetTexture().GetImage();
				stinky.Visible = false;
				await WaitFrames(6);
				using Image hidden = GetViewport().GetTexture().GetImage();
				int num = CountChangedPixels(visibleImage, hidden);
				Check(num >= 128, $"Hiding Stinky must remove visible loading-snail pixels; changed={num}.");
				GD.Print($"SCENE_LOADING_STINKY_VISIBILITY_METRIC changed={num} instances={publishedInstances} local={stinky.forceLocalRender}");
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[SceneLoadingStinkyVisibilityRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(loading))
			{
				loading.QueueFree();
			}
			await WaitFrames(6);
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			Global.Instance.adobeAnimateRenderBackend = previousBackend;
			scene?.Dispose();
		}
		bool flag = _failures == 0 && _checks == 7;
		GD.Print($"SCENE_LOADING_STINKY_VISIBILITY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static int CountPublishedInstances(Node node)
	{
		int num = ((node is AdobeAnimateMultiMeshBatcher adobeAnimateMultiMeshBatcher) ? adobeAnimateMultiMeshBatcher.GetVisibleInstanceCountForTest() : 0);
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			num += CountPublishedInstances(child);
		}
		return num;
	}

	private static int CountChangedPixels(Image visible, Image hidden)
	{
		int num = Math.Min(visible.GetWidth(), hidden.GetWidth());
		int num2 = Math.Min(visible.GetHeight(), hidden.GetHeight());
		int num3 = 0;
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				if (ColorDistance(visible.GetPixel(j, i), hidden.GetPixel(j, i)) > 0.08f)
				{
					num3++;
				}
			}
		}
		return num3;
	}

	private static float ColorDistance(in Color left, in Color right)
	{
		return Mathf.Max(Mathf.Max(Mathf.Abs(left.R - right.R), Mathf.Abs(left.G - right.G)), Mathf.Max(Mathf.Abs(left.B - right.B), Mathf.Abs(left.A - right.A)));
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
			GD.PushError("[SceneLoadingStinkyVisibilityRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountPublishedInstances, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CountChangedPixels, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Object, "hidden", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false)
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
		if (method == MethodName.CountPublishedInstances && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountPublishedInstances(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CountChangedPixels && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountChangedPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
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
		if (method == MethodName.CountPublishedInstances && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountPublishedInstances(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CountChangedPixels && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountChangedPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
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
		if (method == MethodName.CountPublishedInstances)
		{
			return true;
		}
		if (method == MethodName.CountChangedPixels)
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
