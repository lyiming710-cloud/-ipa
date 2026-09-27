using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateBattleZoomVisualIntegrityRuntimeTest.cs")]
public class AdobeAnimateBattleZoomVisualIntegrityRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SampleRegion = "SampleRegion";

		public static readonly StringName SamplesHaveSameIdentity = "SamplesHaveSameIdentity";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _originalMaxFps = "_originalMaxFps";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "ADOBE_ANIMATE_BATTLE_ZOOM_VISUAL_INTEGRITY_RESULT";

	private const string WallnutScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Wallnut.tscn";

	private const string FirenutScenePath = "res://Asset/Anime/Character/Plant/Chapter1/Firenut/Firenut.tscn";

	private const int ZoomRoundTripCount = 12;

	private static readonly Color BackgroundColor = new Color(0.02f, 0.02f, 0.02f);

	private static readonly Rect2I[] CaptureRegions = new Rect2I[4]
	{
		new Rect2I(80, 165, 150, 180),
		new Rect2I(280, 165, 150, 180),
		new Rect2I(480, 165, 150, 180),
		new Rect2I(680, 165, 150, 180)
	};

	private AdobeAnimateRenderBackend _originalBackend;

	private int _originalMaxFps;

	public override async void _Ready()
	{
		int exitCode = 2;
		List<AdobeAnimateSprite> sprites = new List<AdobeAnimateSprite>(CaptureRegions.Length);
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
			CanvasLayer canvasLayer = new CanvasLayer
			{
				Layer = -10
			};
			AddChild(canvasLayer, forceReadableName: false, InternalMode.Disabled);
			ColorRect node = new ColorRect
			{
				Color = BackgroundColor,
				Position = Vector2.Zero,
				Size = new Vector2(900f, 540f),
				MouseFilter = Control.MouseFilterEnum.Ignore
			};
			canvasLayer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			Node2D node2D = new Node2D();
			AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
			Camera2D camera = new Camera2D
			{
				Position = new Vector2(450f, 270f),
				Zoom = Vector2.One,
				Enabled = true
			};
			node2D.AddChild(camera, forceReadableName: false, InternalMode.Disabled);
			PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Wallnut.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
			PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter1/Firenut/Firenut.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
			if (!GodotObject.IsInstanceValid(packedScene) || !GodotObject.IsInstanceValid(packedScene2))
			{
				throw new InvalidOperationException("Production Wallnut or Firenut animation scene is unavailable.");
			}
			for (int i = 0; i < CaptureRegions.Length; i++)
			{
				AdobeAnimateSprite adobeAnimateSprite = ((i % 2 == 0) ? packedScene : packedScene2).Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
				adobeAnimateSprite.Position = CaptureRegions[i].GetCenter();
				adobeAnimateSprite.SetAnimation("Idle");
				adobeAnimateSprite.timeScale = 0.0;
				node2D.AddChild(adobeAnimateSprite, forceReadableName: false, InternalMode.Disabled);
				sprites.Add(adobeAnimateSprite);
			}
			await WaitFrames(24);
			long[][] baseline = await CaptureVisualSamples();
			int visibleRoundTrips = 0;
			int identityMatches = 0;
			int minimumPixels = 2147483647;
			for (int roundTrip = 0; roundTrip < 12; roundTrip++)
			{
				camera.Zoom = ((roundTrip % 2 == 0) ? (Vector2.One * 2f) : (Vector2.One * 0.4f));
				camera.Position = ((roundTrip % 2 == 0) ? new Vector2(700f, 270f) : new Vector2(200f, 270f));
				AdobeAnimateRuntimeManager.NotifyViewportTransformChanged();
				await WaitPhysicsFrames(2);
				camera.Zoom = Vector2.One;
				camera.Position = new Vector2(450f, 270f);
				AdobeAnimateRuntimeManager.NotifyViewportTransformChanged();
				await WaitPhysicsFrames(2);
				long[][] array = await CaptureVisualSamples();
				bool flag = true;
				bool flag2 = true;
				for (int j = 0; j < array.Length; j++)
				{
					minimumPixels = Math.Min(minimumPixels, (int)array[j][0]);
					flag &= baseline[j][0] > 0 && array[j][0] >= baseline[j][0] / 3;
					flag2 &= SamplesHaveSameIdentity(baseline[j], array[j]);
				}
				if (flag)
				{
					visibleRoundTrips++;
				}
				if (flag2)
				{
					identityMatches++;
				}
			}
			AdobeAnimateCrowdAggregateStats aggregateRenderStats = AdobeAnimateRenderManager.GetAggregateRenderStats();
			bool flag3 = Array.TrueForAll(baseline, (long[] sample) => sample[0] > 0) && visibleRoundTrips == 12 && identityMatches == 12 && minimumPixels > 0 && aggregateRenderStats.CrowdRoots == CaptureRegions.Length && aggregateRenderStats.FallbackRoots == 0;
			GD.Print($"{"ADOBE_ANIMATE_BATTLE_ZOOM_VISUAL_INTEGRITY_RESULT"} passed={flag3} visible={visibleRoundTrips}/{12} identity={identityMatches}/{12} minimumPixels={minimumPixels} baseline={DescribeSamples(baseline)} crowd={aggregateRenderStats.CrowdRoots} fallback={aggregateRenderStats.FallbackRoots} renderer={RenderingServer.GetCurrentRenderingMethod()}");
			exitCode = ((!flag3) ? 2 : 0);
		}
		catch (Exception value)
		{
			GD.PrintErr($"{"ADOBE_ANIMATE_BATTLE_ZOOM_VISUAL_INTEGRITY_RESULT"} exception={value}");
		}
		finally
		{
			if (Global.Instance != null)
			{
				Global.Instance.adobeAnimateRenderBackend = _originalBackend;
			}
			Engine.MaxFps = _originalMaxFps;
			foreach (AdobeAnimateSprite item in sprites)
			{
				if (GodotObject.IsInstanceValid(item))
				{
					item.QueueFree();
				}
			}
		}
		GetTree().Quit(exitCode);
	}

	private async Task WaitFrames(int count)
	{
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task WaitPhysicsFrames(int count)
	{
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private async Task<long[][]> CaptureVisualSamples()
	{
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		using Image image = GetViewport().GetTexture().GetImage();
		long[][] array = new long[CaptureRegions.Length][];
		for (int i = 0; i < CaptureRegions.Length; i++)
		{
			array[i] = SampleRegion(image, CaptureRegions[i]);
		}
		return array;
	}

	private static long[] SampleRegion(Image image, Rect2I region)
	{
		long[] array = new long[4];
		int num = Math.Min(region.End.X, image.GetWidth());
		int num2 = Math.Min(region.End.Y, image.GetHeight());
		for (int i = Math.Max(0, region.Position.Y); i < num2; i++)
		{
			for (int j = Math.Max(0, region.Position.X); j < num; j++)
			{
				Color pixel = image.GetPixel(j, i);
				if (!(Math.Abs(pixel.R - BackgroundColor.R) + Math.Abs(pixel.G - BackgroundColor.G) + Math.Abs(pixel.B - BackgroundColor.B) <= 0.08f))
				{
					array[0]++;
					array[1] += Mathf.RoundToInt(pixel.R * 255f);
					array[2] += Mathf.RoundToInt(pixel.G * 255f);
					array[3] += Mathf.RoundToInt(pixel.B * 255f);
				}
			}
		}
		return array;
	}

	private static bool SamplesHaveSameIdentity(long[] baseline, long[] current)
	{
		if (baseline[0] <= 0 || current[0] <= 0)
		{
			return false;
		}
		for (int i = 1; i <= 3; i++)
		{
			double num = (double)baseline[i] / (double)baseline[0];
			double num2 = (double)current[i] / (double)current[0];
			if (Math.Abs(num - num2) > 12.0)
			{
				return false;
			}
		}
		return true;
	}

	private static string DescribeSamples(long[][] samples)
	{
		string[] array = new string[samples.Length];
		for (int i = 0; i < samples.Length; i++)
		{
			long num = Math.Max(1L, samples[i][0]);
			array[i] = $"{samples[i][0]}:{samples[i][1] / num},{samples[i][2] / num},{samples[i][3] / num}";
		}
		return string.Join('|', array);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SampleRegion, new PropertyInfo(Variant.Type.PackedInt64Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Rect2I, "region", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SamplesHaveSameIdentity, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedInt64Array, "baseline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedInt64Array, "current", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SampleRegion && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<long[]>(SampleRegion(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Rect2I>(in args[1])));
			return true;
		}
		if (method == MethodName.SamplesHaveSameIdentity && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SamplesHaveSameIdentity(VariantUtils.ConvertTo<long[]>(in args[0]), VariantUtils.ConvertTo<long[]>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SampleRegion && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<long[]>(SampleRegion(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Rect2I>(in args[1])));
			return true;
		}
		if (method == MethodName.SamplesHaveSameIdentity && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SamplesHaveSameIdentity(VariantUtils.ConvertTo<long[]>(in args[0]), VariantUtils.ConvertTo<long[]>(in args[1])));
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
		if (method == MethodName.SampleRegion)
		{
			return true;
		}
		if (method == MethodName.SamplesHaveSameIdentity)
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
