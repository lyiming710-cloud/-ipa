using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateAllAnimationGpuGraphAuditTest.cs")]
public class AdobeAnimateAllAnimationGpuGraphAuditTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName AuditScene = "AuditScene";

		public static readonly StringName AddFailure = "AddFailure";

		public static readonly StringName ReleaseRetainedInstances = "ReleaseRetainedInstances";

		public static readonly StringName PrintResult = "PrintResult";

		public static readonly StringName GetNodePath = "GetNodePath";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _animationScenes = "_animationScenes";

		public static readonly StringName _loadFailures = "_loadFailures";

		public static readonly StringName _instantiateFailures = "_instantiateFailures";

		public static readonly StringName _renderRoots = "_renderRoots";

		public static readonly StringName _emptyPlaceholders = "_emptyPlaceholders";

		public static readonly StringName _gpuGraphRoots = "_gpuGraphRoots";

		public static readonly StringName _gpuGraphFailures = "_gpuGraphFailures";

		public static readonly StringName _animationOwners = "_animationOwners";

		public static readonly StringName _animationClips = "_animationClips";

		public static readonly StringName _gpuGraphOwners = "_gpuGraphOwners";

		public static readonly StringName _gpuGraphSlots = "_gpuGraphSlots";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string AnimationRoot = "res://Asset/Anime";

	private const int YieldInterval = 8;

	private const int MaximumPrintedFailures = 200;

	private readonly List<string> _scenePaths = new List<string>(1200);

	private readonly List<AdobeAnimateSprite> _sprites = new List<AdobeAnimateSprite>(32);

	private readonly List<PackedScene> _retainedScenes = new List<PackedScene>(1200);

	private readonly List<Node> _retainedInstances = new List<Node>(1200);

	private readonly Dictionary<string, int> _failureReasonCounts = new Dictionary<string, int>(StringComparer.Ordinal);

	private readonly List<string> _failures = new List<string>();

	private int _animationScenes;

	private int _loadFailures;

	private int _instantiateFailures;

	private int _renderRoots;

	private int _emptyPlaceholders;

	private int _gpuGraphRoots;

	private int _gpuGraphFailures;

	private int _animationOwners;

	private int _animationClips;

	private long _gpuGraphOwners;

	private long _gpuGraphSlots;

	public override async void _Ready()
	{
		bool previousGpuGraphEnabled = AdobeAnimateRenderManager.GpuRenderGraphEnabled;
		try
		{
			AdobeAnimateRenderManager.GpuRenderGraphEnabled = true;
			AdobeAnimateDefinitionCache.Clear();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches(notifyRuntime: false);
			CollectScenePaths("res://Asset/Anime", _scenePaths);
			_scenePaths.Sort(StringComparer.OrdinalIgnoreCase);
			for (int sceneIndex = 0; sceneIndex < _scenePaths.Count; sceneIndex++)
			{
				AuditScene(_scenePaths[sceneIndex]);
				if ((sceneIndex + 1) % 8 == 0)
				{
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				}
			}
		}
		catch (Exception value)
		{
			AddFailure("<audit>", $"unexpected exception: {value}");
		}
		finally
		{
			ReleaseRetainedInstances();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches(notifyRuntime: false);
			AdobeAnimateDefinitionCache.Clear();
			AdobeAnimateRenderManager.GpuRenderGraphEnabled = previousGpuGraphEnabled;
		}
		PrintResult();
		bool flag = _loadFailures == 0 && _instantiateFailures == 0 && _gpuGraphFailures == 0;
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void AuditScene(string scenePath)
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Reuse);
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			_loadFailures++;
			AddFailure(scenePath, "PackedScene load failed");
			return;
		}
		_retainedScenes.Add(packedScene);
		Node node = null;
		try
		{
			node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(node))
			{
				_instantiateFailures++;
				AddFailure(scenePath, "PackedScene instantiate returned an invalid node");
				return;
			}
			_retainedInstances.Add(node);
			_sprites.Clear();
			CollectSprites(node, _sprites);
			if (_sprites.Count == 0)
			{
				return;
			}
			_animationScenes++;
			for (int i = 0; i < _sprites.Count; i++)
			{
				AdobeAnimateSprite adobeAnimateSprite = _sprites[i];
				adobeAnimateSprite.PrepareDetachedGpuGraphWarmup();
				if (GodotObject.IsInstanceValid(adobeAnimateSprite.flashAnimeData))
				{
					_animationOwners++;
					_animationClips += adobeAnimateSprite.flashAnimeData.clips?.Count ?? 0;
				}
			}
			for (int j = 0; j < _sprites.Count; j++)
			{
				AdobeAnimateSprite adobeAnimateSprite2 = _sprites[j];
				if (adobeAnimateSprite2.IsRenderedByParentSpriteForRender())
				{
					continue;
				}
				if (adobeAnimateSprite2.IsEmptyHiddenGpuGraphPlaceholderForRender())
				{
					_emptyPlaceholders++;
					continue;
				}
				_renderRoots++;
				if (!AdobeAnimateGpuRenderGraphBuilder.TryBuild(adobeAnimateSprite2, out var graph, out var ownerSprites, out var failureReason))
				{
					_gpuGraphFailures++;
					AddFailure(scenePath + ":" + GetNodePath(adobeAnimateSprite2), string.IsNullOrWhiteSpace(failureReason) ? "unknown GPU Graph rejection" : failureReason);
				}
				else
				{
					_gpuGraphRoots++;
					_gpuGraphOwners += ownerSprites.Length;
					_gpuGraphSlots += graph.RenderSlots.Length;
				}
			}
		}
		catch (Exception ex)
		{
			_instantiateFailures++;
			AddFailure(scenePath, "instantiate/audit exception: " + ex.Message);
		}
		finally
		{
			_sprites.Clear();
		}
	}

	private void AddFailure(string source, string reason)
	{
		if (reason == null)
		{
			reason = "unknown";
		}
		_failureReasonCounts.TryGetValue(reason, out var value);
		_failureReasonCounts[reason] = value + 1;
		if (_failures.Count < 200)
		{
			_failures.Add(source + " | " + reason);
		}
	}

	private void ReleaseRetainedInstances()
	{
		_sprites.Clear();
		for (int num = _retainedInstances.Count - 1; num >= 0; num--)
		{
			Node node = _retainedInstances[num];
			if (GodotObject.IsInstanceValid(node))
			{
				node.Free();
			}
		}
	}

	private void PrintResult()
	{
		for (int i = 0; i < _failures.Count; i++)
		{
			GD.Print("[AllAnimationGpuGraphAuditFailure] " + _failures[i]);
		}
		foreach (KeyValuePair<string, int> failureReasonCount in _failureReasonCounts)
		{
			GD.Print($"[AllAnimationGpuGraphAuditReason] count={failureReasonCount.Value} reason={failureReasonCount.Key}");
		}
		bool value = _loadFailures == 0 && _instantiateFailures == 0 && _gpuGraphFailures == 0;
		GD.Print($"ALL_ANIMATION_GPU_GRAPH_AUDIT_RESULT passed={value} tscn={_scenePaths.Count} animationScenes={_animationScenes} loadFailures={_loadFailures} instantiateFailures={_instantiateFailures} renderRoots={_renderRoots} emptyPlaceholders={_emptyPlaceholders} gpuGraphRoots={_gpuGraphRoots} gpuGraphFailures={_gpuGraphFailures} animationOwners={_animationOwners} animationClips={_animationClips} graphOwners={_gpuGraphOwners} graphSlots={_gpuGraphSlots}");
	}

	private static string GetNodePath(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return "<invalid>";
		}
		List<string> list = new List<string>();
		Node node2 = node;
		while (GodotObject.IsInstanceValid(node2))
		{
			list.Add(node2.Name.ToString());
			node2 = node2.GetParent();
		}
		list.Reverse();
		return string.Join("/", list);
	}

	private static void CollectSprites(Node node, List<AdobeAnimateSprite> output)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSprite item)
		{
			output.Add(item);
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			CollectSprites(child, output);
		}
	}

	private static void CollectScenePaths(string directoryPath, List<string> output)
	{
		using DirAccess dirAccess = DirAccess.Open(directoryPath);
		if (dirAccess == null)
		{
			return;
		}
		dirAccess.ListDirBegin();
		while (true)
		{
			string next = dirAccess.GetNext();
			if (string.IsNullOrEmpty(next))
			{
				break;
			}
			if (!(next == ".") && !(next == ".."))
			{
				string text = directoryPath + "/" + next;
				if (dirAccess.CurrentIsDir())
				{
					CollectScenePaths(text, output);
				}
				else if (next.EndsWith(".tscn", StringComparison.OrdinalIgnoreCase))
				{
					output.Add(text);
				}
			}
		}
		dirAccess.ListDirEnd();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AuditScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "scenePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFailure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseRetainedInstances, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrintResult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetNodePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.AuditScene && args.Count == 1)
		{
			AuditScene(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddFailure && args.Count == 2)
		{
			AddFailure(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseRetainedInstances && args.Count == 0)
		{
			ReleaseRetainedInstances();
			ret = default;
			return true;
		}
		if (method == MethodName.PrintResult && args.Count == 0)
		{
			PrintResult();
			ret = default;
			return true;
		}
		if (method == MethodName.GetNodePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetNodePath(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetNodePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetNodePath(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.AuditScene)
		{
			return true;
		}
		if (method == MethodName.AddFailure)
		{
			return true;
		}
		if (method == MethodName.ReleaseRetainedInstances)
		{
			return true;
		}
		if (method == MethodName.PrintResult)
		{
			return true;
		}
		if (method == MethodName.GetNodePath)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._animationScenes)
		{
			_animationScenes = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._loadFailures)
		{
			_loadFailures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._instantiateFailures)
		{
			_instantiateFailures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._renderRoots)
		{
			_renderRoots = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._emptyPlaceholders)
		{
			_emptyPlaceholders = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphRoots)
		{
			_gpuGraphRoots = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphFailures)
		{
			_gpuGraphFailures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationOwners)
		{
			_animationOwners = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationClips)
		{
			_animationClips = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphOwners)
		{
			_gpuGraphOwners = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphSlots)
		{
			_gpuGraphSlots = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._animationScenes)
		{
			value = VariantUtils.CreateFrom(in _animationScenes);
			return true;
		}
		if (name == PropertyName._loadFailures)
		{
			value = VariantUtils.CreateFrom(in _loadFailures);
			return true;
		}
		if (name == PropertyName._instantiateFailures)
		{
			value = VariantUtils.CreateFrom(in _instantiateFailures);
			return true;
		}
		if (name == PropertyName._renderRoots)
		{
			value = VariantUtils.CreateFrom(in _renderRoots);
			return true;
		}
		if (name == PropertyName._emptyPlaceholders)
		{
			value = VariantUtils.CreateFrom(in _emptyPlaceholders);
			return true;
		}
		if (name == PropertyName._gpuGraphRoots)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphRoots);
			return true;
		}
		if (name == PropertyName._gpuGraphFailures)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphFailures);
			return true;
		}
		if (name == PropertyName._animationOwners)
		{
			value = VariantUtils.CreateFrom(in _animationOwners);
			return true;
		}
		if (name == PropertyName._animationClips)
		{
			value = VariantUtils.CreateFrom(in _animationClips);
			return true;
		}
		if (name == PropertyName._gpuGraphOwners)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphOwners);
			return true;
		}
		if (name == PropertyName._gpuGraphSlots)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphSlots);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._animationScenes, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._loadFailures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._instantiateFailures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderRoots, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._emptyPlaceholders, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphRoots, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphFailures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationOwners, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationClips, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphOwners, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphSlots, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._animationScenes, Variant.From(in _animationScenes));
		info.AddProperty(PropertyName._loadFailures, Variant.From(in _loadFailures));
		info.AddProperty(PropertyName._instantiateFailures, Variant.From(in _instantiateFailures));
		info.AddProperty(PropertyName._renderRoots, Variant.From(in _renderRoots));
		info.AddProperty(PropertyName._emptyPlaceholders, Variant.From(in _emptyPlaceholders));
		info.AddProperty(PropertyName._gpuGraphRoots, Variant.From(in _gpuGraphRoots));
		info.AddProperty(PropertyName._gpuGraphFailures, Variant.From(in _gpuGraphFailures));
		info.AddProperty(PropertyName._animationOwners, Variant.From(in _animationOwners));
		info.AddProperty(PropertyName._animationClips, Variant.From(in _animationClips));
		info.AddProperty(PropertyName._gpuGraphOwners, Variant.From(in _gpuGraphOwners));
		info.AddProperty(PropertyName._gpuGraphSlots, Variant.From(in _gpuGraphSlots));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._animationScenes, out var value))
		{
			_animationScenes = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._loadFailures, out var value2))
		{
			_loadFailures = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._instantiateFailures, out var value3))
		{
			_instantiateFailures = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._renderRoots, out var value4))
		{
			_renderRoots = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._emptyPlaceholders, out var value5))
		{
			_emptyPlaceholders = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphRoots, out var value6))
		{
			_gpuGraphRoots = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphFailures, out var value7))
		{
			_gpuGraphFailures = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationOwners, out var value8))
		{
			_animationOwners = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationClips, out var value9))
		{
			_animationClips = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphOwners, out var value10))
		{
			_gpuGraphOwners = value10.As<long>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphSlots, out var value11))
		{
			_gpuGraphSlots = value11.As<long>();
		}
	}
}
