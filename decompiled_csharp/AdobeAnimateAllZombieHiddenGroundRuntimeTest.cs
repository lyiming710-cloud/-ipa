using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateAllZombieHiddenGroundRuntimeTest.cs")]
public class AdobeAnimateAllZombieHiddenGroundRuntimeTest : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindAnimationSprite = "FindAnimationSprite";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _originalRasterCompositeEnabled = "_originalRasterCompositeEnabled";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const string ResultMarker = "ADOBE_ANIMATE_ALL_ZOMBIE_HIDDEN_GROUND_RESULT";

	private const string ZombieAnimationRoot = "res://Asset/Anime/Character/Zombie";

	private const string HiddenGroundDeclaration = "Animation/LayerVisible/_ground = false";

	private static readonly StringName HiddenGroundLayer = "_ground";

	private AdobeAnimateRenderBackend _originalBackend;

	private bool _originalRasterCompositeEnabled;

	public override async void _Ready()
	{
		int exitCode = 2;
		List<string> failures = new List<string>();
		try
		{
			if (Global.Instance == null)
			{
				throw new InvalidOperationException("Global autoload is unavailable.");
			}
			_originalBackend = Global.Instance.adobeAnimateRenderBackend;
			_originalRasterCompositeEnabled = AdobeAnimateRenderManager.RasterCompositeEnabled;
			Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
			AdobeAnimateRenderManager.RasterCompositeEnabled = false;
			List<string> scenePaths = new List<string>();
			CollectHiddenGroundScenePaths("res://Asset/Anime/Character/Zombie", scenePaths);
			for (int start = 0; start < scenePaths.Count; start += 8)
			{
				List<(string Path, AdobeAnimateSprite Sprite)> sprites = new List<(string, AdobeAnimateSprite)>();
				int num = Math.Min(scenePaths.Count, start + 8);
				for (int i = start; i < num; i++)
				{
					string text = scenePaths[i];
					Node node = GD.Load<PackedScene>(text)?.Instantiate(PackedScene.GenEditState.Disabled);
					AdobeAnimateSprite adobeAnimateSprite = FindAnimationSprite(node);
					if (!GodotObject.IsInstanceValid(adobeAnimateSprite))
					{
						node?.Free();
						failures.Add(text + ": no AdobeAnimateSprite root");
					}
					else
					{
						adobeAnimateSprite.Position = new Vector2(120f + (float)(i - start) * 110f, 300f);
						AddChild(node, forceReadableName: false, InternalMode.Disabled);
						sprites.Add((text, adobeAnimateSprite));
					}
				}
				await WaitProcessFrames(4);
				for (int j = 0; j < sprites.Count; j++)
				{
					ValidateHiddenGround(sprites[j].Path, sprites[j].Sprite, failures);
				}
				for (int k = 0; k < sprites.Count; k++)
				{
					Node node2 = sprites[k].Sprite;
					while (GodotObject.IsInstanceValid(node2.GetParent()) && node2.GetParent() != this)
					{
						node2 = node2.GetParent();
					}
					node2.QueueFree();
				}
				await WaitProcessFrames(2);
			}
			if (failures.Count > 0)
			{
				throw new InvalidOperationException(string.Join(" | ", failures));
			}
			GD.Print($"{"ADOBE_ANIMATE_ALL_ZOMBIE_HIDDEN_GROUND_RESULT"} passed=True scenes={scenePaths.Count} renderer={RenderingServer.GetCurrentRenderingMethod()}");
			exitCode = 0;
		}
		catch (Exception value)
		{
			GD.PrintErr($"{"ADOBE_ANIMATE_ALL_ZOMBIE_HIDDEN_GROUND_RESULT"} passed=False exception={value}");
		}
		finally
		{
			if (Global.Instance != null)
			{
				Global.Instance.adobeAnimateRenderBackend = _originalBackend;
			}
			AdobeAnimateRenderManager.RasterCompositeEnabled = _originalRasterCompositeEnabled;
		}
		GetTree().Quit(exitCode);
	}

	private static void CollectHiddenGroundScenePaths(string directoryPath, List<string> result)
	{
		using DirAccess dirAccess = DirAccess.Open(directoryPath);
		if (dirAccess == null)
		{
			return;
		}
		string[] files = dirAccess.GetFiles();
		foreach (string text in files)
		{
			if (text.EndsWith(".tscn", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = directoryPath.PathJoin(text);
				if (FileAccess.GetFileAsString(text2).Contains("Animation/LayerVisible/_ground = false", StringComparison.Ordinal))
				{
					result.Add(text2);
				}
			}
		}
		files = dirAccess.GetDirectories();
		foreach (string file in files)
		{
			CollectHiddenGroundScenePaths(directoryPath.PathJoin(file), result);
		}
	}

	private static AdobeAnimateSprite FindAnimationSprite(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return null;
		}
		if (node is AdobeAnimateSprite result)
		{
			return result;
		}
		for (int i = 0; i < node.GetChildCount(); i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = FindAnimationSprite(node.GetChild(i));
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				return adobeAnimateSprite;
			}
		}
		return null;
	}

	private static void ValidateHiddenGround(string scenePath, AdobeAnimateSprite sprite, List<string> failures)
	{
		if (sprite.GetFliter(HiddenGroundLayer))
		{
			failures.Add(scenePath + ": logical _ground became visible");
			return;
		}
		AdobeAnimateData flashAnimeData = sprite.flashAnimeData;
		if (!GodotObject.IsInstanceValid(flashAnimeData) || flashAnimeData.layerDictionary == null || !flashAnimeData.layerDictionary.ContainsKey(HiddenGroundLayer))
		{
			failures.Add(scenePath + ": _ground layer metadata missing");
			return;
		}
		int layerId = flashAnimeData.layerDictionary[HiddenGroundLayer].AsInt32();
		AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult = sprite.TryBuildCrowdRenderState(out var state);
		if (adobeAnimateCrowdRenderStateResult != AdobeAnimateCrowdRenderStateResult.Submitted || state == null || state.Mode != AdobeAnimateCrowdRenderMode.GpuGraph)
		{
			failures.Add($"{scenePath}: GPU graph state unavailable result={adobeAnimateCrowdRenderStateResult} mode={state?.Mode}");
		}
		else if (IsLayerVisible(state.GpuGraphRootOwnerState, layerId))
		{
			failures.Add(scenePath + ": built GPU owner state exposed _ground");
		}
	}

	private static bool IsLayerVisible(AdobeAnimateGpuGraphOwnerState state, int layerId)
	{
		if (state == null || layerId < 0 || layerId >= state.LayerCount)
		{
			return true;
		}
		if (state.AllLayersVisible)
		{
			return true;
		}
		if (state.CanUseLayerMask && layerId < 64)
		{
			return (state.LayerMask & (ulong)(1L << layerId)) != 0;
		}
		if (state.LayerVisible != null && layerId < state.LayerVisible.Count)
		{
			return state.LayerVisible[layerId];
		}
		return true;
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindAnimationSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName.FindAnimationSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(FindAnimationSprite(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindAnimationSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(FindAnimationSprite(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.FindAnimationSprite)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._originalBackend)
		{
			_originalBackend = VariantUtils.ConvertTo<AdobeAnimateRenderBackend>(in value);
			return true;
		}
		if (name == PropertyName._originalRasterCompositeEnabled)
		{
			_originalRasterCompositeEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._originalBackend)
		{
			value = VariantUtils.CreateFrom(in _originalBackend);
			return true;
		}
		if (name == PropertyName._originalRasterCompositeEnabled)
		{
			value = VariantUtils.CreateFrom(in _originalRasterCompositeEnabled);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._originalBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._originalRasterCompositeEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._originalBackend, Variant.From(in _originalBackend));
		info.AddProperty(PropertyName._originalRasterCompositeEnabled, Variant.From(in _originalRasterCompositeEnabled));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._originalBackend, out var value))
		{
			_originalBackend = value.As<AdobeAnimateRenderBackend>();
		}
		if (info.TryGetProperty(PropertyName._originalRasterCompositeEnabled, out var value2))
		{
			_originalRasterCompositeEnabled = value2.As<bool>();
		}
	}
}
