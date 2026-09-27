using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/WallnutReadySetPlantFlickerRuntimeTest.cs")]
public class WallnutReadySetPlantFlickerRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ConfigurePreviewRenderTree = "ConfigurePreviewRenderTree";

		public static readonly StringName ReleasePreviewRenderTree = "ReleasePreviewRenderTree";

		public static readonly StringName CountForegroundPixels = "CountForegroundPixels";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _originalMaxFps = "_originalMaxFps";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "WALLNUT_READY_SET_PLANT_FLICKER_RESULT";

	private const string WallnutScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Wallnut.tscn";

	private const string NestedPreviewScenePath = "res://Asset/Anime/Character/Plant/Chapter1/SunflowerPea/SunflowerPea.tscn";

	private const int SampleFrameCount = 90;

	private static readonly Color BackgroundColor = new Color(0.02f, 0.02f, 0.02f);

	private static readonly Rect2I[] CaptureRegions = new Rect2I[1]
	{
		new Rect2I(90, 100, 150, 180)
	};

	private AdobeAnimateRenderBackend _originalBackend;

	private int _originalMaxFps;

	public override async void _Ready()
	{
		int exitCode = 2;
		List<AdobeAnimateSprite> wallnuts = new List<AdobeAnimateSprite>(CaptureRegions.Length);
		AdobeAnimateSprite nestedPreview = null;
		try
		{
			ProcessMode = ProcessModeEnum.Always;
			if (Global.Instance == null)
			{
				throw new InvalidOperationException("Global autoload is unavailable.");
			}
			_originalBackend = Global.Instance.adobeAnimateRenderBackend;
			_originalMaxFps = Engine.MaxFps;
			Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
			Engine.MaxFps = 120;
			ColorRect node = new ColorRect
			{
				Color = BackgroundColor,
				Position = Vector2.Zero,
				Size = new Vector2(900f, 420f),
				MouseFilter = Control.MouseFilterEnum.Ignore
			};
			AddChild(node, forceReadableName: false, InternalMode.Disabled);
			CanvasLayer battleLayer = new CanvasLayer
			{
				Layer = 1,
				FollowViewportEnabled = true,
				ProcessMode = ProcessModeEnum.Pausable
			};
			AddChild(battleLayer, forceReadableName: false, InternalMode.Disabled);
			Node2D characterHost = new Node2D();
			battleLayer.AddChild(characterHost, forceReadableName: false, InternalMode.Disabled);
			PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Wallnut.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
			if (!GodotObject.IsInstanceValid(packedScene))
			{
				throw new InvalidOperationException("Production Wallnut animation scene is unavailable.");
			}
			for (int i = 0; i < CaptureRegions.Length; i++)
			{
				AdobeAnimateSprite adobeAnimateSprite = packedScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
				adobeAnimateSprite.Position = CaptureRegions[i].GetCenter();
				adobeAnimateSprite.SetAnimation("Idle");
				characterHost.AddChild(adobeAnimateSprite, forceReadableName: false, InternalMode.Disabled);
				wallnuts.Add(adobeAnimateSprite);
			}
			await WaitFrames(16);
			int[] baselinePixels = await CaptureRegionPixels();
			PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter1/SunflowerPea/SunflowerPea.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
			if (!GodotObject.IsInstanceValid(packedScene2))
			{
				throw new InvalidOperationException("Production nested preview animation scene is unavailable.");
			}
			Control control = new Control
			{
				Name = "WallnutReadySetPlantPreviewMount",
				MouseFilter = Control.MouseFilterEnum.Ignore
			};
			battleLayer.AddChild(control, forceReadableName: false, InternalMode.Disabled);
			nestedPreview = packedScene2.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			ConfigurePreviewRenderTree(nestedPreview, control);
			nestedPreview.Position = new Vector2(-1000f, -1000f);
			battleLayer.AddChild(nestedPreview, forceReadableName: false, InternalMode.Disabled);
			characterHost.ProcessMode = ProcessModeEnum.Disabled;
			await WaitFrames(30);
			characterHost.ProcessMode = ProcessModeEnum.Inherit;
			int blankFrames = 0;
			int minimumPixels = 2147483647;
			int nativeCanvasFrames = 0;
			for (int frame = 0; frame < 90; frame++)
			{
				int[] array = await CaptureRegionPixels();
				bool flag = true;
				for (int j = 0; j < array.Length; j++)
				{
					minimumPixels = Math.Min(minimumPixels, array[j]);
					if (baselinePixels[j] <= 0 || array[j] < baselinePixels[j] / 4)
					{
						flag = false;
					}
					wallnuts[j].GetRuntimeCrowdCullingDebugState(out var _, out var _, out var _, out var _, out var nativeCanvasSuppressed);
					if (!nativeCanvasSuppressed)
					{
						nativeCanvasFrames++;
					}
				}
				if (!flag)
				{
					blankFrames++;
				}
			}
			AdobeAnimateCrowdAggregateStats aggregateRenderStats = AdobeAnimateRenderManager.GetAggregateRenderStats();
			bool flag2 = Array.TrueForAll(baselinePixels, (int pixels) => pixels > 0) && blankFrames == 0 && minimumPixels > 0 && aggregateRenderStats.CrowdRoots == CaptureRegions.Length && aggregateRenderStats.FallbackRoots == 0;
			GD.Print($"{"WALLNUT_READY_SET_PLANT_FLICKER_RESULT"} passed={flag2} blank={blankFrames}/{90} minimumPixels={minimumPixels} baseline={string.Join(',', baselinePixels)} nativeCanvasFrames={nativeCanvasFrames} crowd={aggregateRenderStats.CrowdRoots} fallback={aggregateRenderStats.FallbackRoots} renderer={RenderingServer.GetCurrentRenderingMethod()}");
			exitCode = ((!flag2) ? 2 : 0);
		}
		catch (Exception value)
		{
			GD.PrintErr($"{"WALLNUT_READY_SET_PLANT_FLICKER_RESULT"} exception={value}");
		}
		finally
		{
			if (Global.Instance != null)
			{
				Global.Instance.adobeAnimateRenderBackend = _originalBackend;
			}
			Engine.MaxFps = _originalMaxFps;
			if (GodotObject.IsInstanceValid(nestedPreview))
			{
				ReleasePreviewRenderTree(nestedPreview);
				nestedPreview.QueueFree();
			}
			foreach (AdobeAnimateSprite item in wallnuts)
			{
				if (GodotObject.IsInstanceValid(item))
				{
					item.QueueFree();
				}
			}
		}
		GetTree().Quit(exitCode);
	}

	private static void ConfigurePreviewRenderTree(Node node, Control renderMount)
	{
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.forceLocalRender = true;
			adobeAnimateSprite.forceCpuPoseRender = true;
			adobeAnimateSprite.SetRenderClipControl(renderMount);
		}
		for (int i = 0; i < node.GetChildCount(); i++)
		{
			ConfigurePreviewRenderTree(node.GetChild(i), renderMount);
		}
	}

	private static void ReleasePreviewRenderTree(Node node)
	{
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.ClearRenderClipControl();
			adobeAnimateSprite.ReleaseForcedCpuPoseData();
		}
		for (int i = 0; i < node.GetChildCount(); i++)
		{
			ReleasePreviewRenderTree(node.GetChild(i));
		}
	}

	private async Task WaitFrames(int count)
	{
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task<int[]> CaptureRegionPixels()
	{
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		using Image image = GetViewport().GetTexture().GetImage();
		int[] array = new int[CaptureRegions.Length];
		for (int i = 0; i < CaptureRegions.Length; i++)
		{
			array[i] = CountForegroundPixels(image, CaptureRegions[i]);
		}
		return array;
	}

	private static int CountForegroundPixels(Image image, Rect2I region)
	{
		int num = 0;
		int num2 = Math.Min(region.End.X, image.GetWidth());
		int num3 = Math.Min(region.End.Y, image.GetHeight());
		for (int i = Math.Max(0, region.Position.Y); i < num3; i++)
		{
			for (int j = Math.Max(0, region.Position.X); j < num2; j++)
			{
				Color pixel = image.GetPixel(j, i);
				if (Math.Abs(pixel.R - BackgroundColor.R) + Math.Abs(pixel.G - BackgroundColor.G) + Math.Abs(pixel.B - BackgroundColor.B) > 0.08f)
				{
					num++;
				}
			}
		}
		return num;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigurePreviewRenderTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "renderMount", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReleasePreviewRenderTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CountForegroundPixels, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Rect2I, "region", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ConfigurePreviewRenderTree && args.Count == 2)
		{
			ConfigurePreviewRenderTree(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleasePreviewRenderTree && args.Count == 1)
		{
			ReleasePreviewRenderTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountForegroundPixels && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountForegroundPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Rect2I>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConfigurePreviewRenderTree && args.Count == 2)
		{
			ConfigurePreviewRenderTree(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleasePreviewRenderTree && args.Count == 1)
		{
			ReleasePreviewRenderTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountForegroundPixels && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountForegroundPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Rect2I>(in args[1])));
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
		if (method == MethodName.ConfigurePreviewRenderTree)
		{
			return true;
		}
		if (method == MethodName.ReleasePreviewRenderTree)
		{
			return true;
		}
		if (method == MethodName.CountForegroundPixels)
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
		if (name == PropertyName._originalMaxFps)
		{
			_originalMaxFps = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._originalMaxFps)
		{
			value = VariantUtils.CreateFrom(in _originalMaxFps);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._originalMaxFps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._originalBackend, Variant.From(in _originalBackend));
		info.AddProperty(PropertyName._originalMaxFps, Variant.From(in _originalMaxFps));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._originalBackend, out var value))
		{
			_originalBackend = value.As<AdobeAnimateRenderBackend>();
		}
		if (info.TryGetProperty(PropertyName._originalMaxFps, out var value2))
		{
			_originalMaxFps = value2.As<int>();
		}
	}
}
