using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/FirenutFirstFrameHandoffRuntimeTest.cs")]
public class FirenutFirstFrameHandoffRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName CreateAnimationSprite = "CreateAnimationSprite";

		public static readonly StringName CountForegroundPixels = "CountForegroundPixels";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _animationHost = "_animationHost";

		public static readonly StringName _originalBackend = "_originalBackend";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "FIRENUT_FIRST_FRAME_HANDOFF_RESULT";

	private static readonly Color BackgroundColor = new Color(0.02f, 0.02f, 0.02f);

	private static readonly Rect2I AnchorRegion = new Rect2I(85, 90, 150, 150);

	private static readonly Rect2I SpawnRegion = new Rect2I(330, 90, 150, 150);

	private static readonly Rect2I ReattachRegion = new Rect2I(590, 90, 150, 150);

	private static readonly Rect2I UnsuppressibleOriginRegion = new Rect2I(820, 300, 150, 150);

	private static readonly Rect2I UnsuppressibleMovedRegion = new Rect2I(1040, 300, 150, 150);

	private const int ReattachCycles = 4;

	private Action _lateFrameAction;

	private Node2D _animationHost;

	private AdobeAnimateRenderBackend _originalBackend;

	public override async void _Ready()
	{
		int exitCode = 2;
		AdobeAnimateSprite anchorSprite = null;
		AdobeAnimateSprite spawnedSprite = null;
		AdobeAnimateSprite cardPreviewSprite = null;
		AdobeAnimateSprite unsuppressibleSprite = null;
		AdobeAnimateSprite normalPlantSprite = null;
		try
		{
			if (Global.Instance == null)
			{
				throw new InvalidOperationException("Global autoload is unavailable.");
			}
			_originalBackend = Global.Instance.adobeAnimateRenderBackend;
			Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
			ProcessPriority = 1000;
			ColorRect node = new ColorRect
			{
				Color = BackgroundColor,
				Position = Vector2.Zero,
				Size = new Vector2(900f, 420f),
				MouseFilter = Control.MouseFilterEnum.Ignore
			};
			AddChild(node, forceReadableName: false, InternalMode.Disabled);
			CanvasLayer canvasLayer = new CanvasLayer
			{
				Layer = 1,
				FollowViewportEnabled = true
			};
			AddChild(canvasLayer, forceReadableName: false, InternalMode.Disabled);
			_animationHost = new Node2D();
			canvasLayer.AddChild(_animationHost, forceReadableName: false, InternalMode.Disabled);
			PackedScene firenutScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter1/Firenut/Firenut.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
			PackedScene peashooterScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/PeaShooter/PeaShooter.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
			if (!GodotObject.IsInstanceValid(firenutScene) || !GodotObject.IsInstanceValid(peashooterScene))
			{
				throw new InvalidOperationException("Production Firenut or Peashooter animation scene is unavailable.");
			}
			anchorSprite = CreateAnimationSprite(firenutScene, new Vector2(160f, 170f));
			_animationHost.AddChild(anchorSprite, forceReadableName: false, InternalMode.Disabled);
			await WaitFrames(12);
			AdobeAnimateSprite capturedSpawnedSprite;
			spawnedSprite = (capturedSpawnedSprite = CreateAnimationSprite(firenutScene, SpawnRegion.GetCenter()));
			_lateFrameAction = () =>
			{
				_animationHost.AddChild(capturedSpawnedSprite, forceReadableName: false, InternalMode.Disabled);
			};
			int spawnFirstFramePixels = await CaptureNextDisplayedFrame(SpawnRegion);
			await WaitFrames(4);
			int stablePixels = await CaptureNextDisplayedFrame(SpawnRegion);
			int visibleReattachFrames = 0;
			int minimumReattachPixels = 2147483647;
			for (int cycle = 0; cycle < 4; cycle++)
			{
				AdobeAnimateSprite cycleSprite = spawnedSprite;
				_lateFrameAction = () =>
				{
					_animationHost.RemoveChild(cycleSprite);
				};
				await CaptureNextDisplayedFrame(ReattachRegion);
				cycleSprite.Position = ReattachRegion.GetCenter();
				_lateFrameAction = () =>
				{
					_animationHost.AddChild(cycleSprite, forceReadableName: false, InternalMode.Disabled);
				};
				int num = await CaptureNextDisplayedFrame(ReattachRegion);
				minimumReattachPixels = Math.Min(minimumReattachPixels, num);
				if (num > 0)
				{
					visibleReattachFrames++;
				}
			}
			await WaitFrames(4);
			unsuppressibleSprite = CreateAnimationSprite(firenutScene, UnsuppressibleOriginRegion.GetCenter());
			ColorRect node2 = new ColorRect
			{
				Color = Colors.Transparent,
				Size = Vector2.One,
				MouseFilter = Control.MouseFilterEnum.Ignore
			};
			unsuppressibleSprite.AddChild(node2, forceReadableName: false, InternalMode.Disabled);
			AdobeAnimateSprite capturedUnsuppressibleSprite = unsuppressibleSprite;
			_lateFrameAction = () =>
			{
				_animationHost.AddChild(capturedUnsuppressibleSprite, forceReadableName: false, InternalMode.Disabled);
			};
			await WaitFrames(4);
			int unsuppressibleOriginPixels = await CaptureNextDisplayedFrame(UnsuppressibleOriginRegion);
			unsuppressibleSprite.Position = UnsuppressibleMovedRegion.GetCenter();
			await WaitFrames(2);
			int stalePixelsAfterMove = await CaptureNextDisplayedFrame(UnsuppressibleOriginRegion);
			int movedPixelsAfterMove = await CaptureNextDisplayedFrame(UnsuppressibleMovedRegion);
			_lateFrameAction = () =>
			{
				capturedUnsuppressibleSprite.QueueFree();
			};
			await WaitFrames(4);
			unsuppressibleSprite = null;
			normalPlantSprite = CreateAnimationSprite(peashooterScene, UnsuppressibleOriginRegion.GetCenter());
			ColorRect node3 = new ColorRect
			{
				Color = Colors.Transparent,
				Size = Vector2.One,
				MouseFilter = Control.MouseFilterEnum.Ignore
			};
			normalPlantSprite.AddChild(node3, forceReadableName: false, InternalMode.Disabled);
			AdobeAnimateSprite capturedNormalPlantSprite = normalPlantSprite;
			_lateFrameAction = () =>
			{
				_animationHost.AddChild(capturedNormalPlantSprite, forceReadableName: false, InternalMode.Disabled);
			};
			await WaitFrames(4);
			int normalPlantOriginPixels = await CaptureNextDisplayedFrame(UnsuppressibleOriginRegion);
			normalPlantSprite.Position = UnsuppressibleMovedRegion.GetCenter();
			await WaitFrames(2);
			int normalPlantStalePixelsAfterMove = await CaptureNextDisplayedFrame(UnsuppressibleOriginRegion);
			int normalPlantMovedPixelsAfterMove = await CaptureNextDisplayedFrame(UnsuppressibleMovedRegion);
			_lateFrameAction = () =>
			{
				capturedNormalPlantSprite.QueueFree();
			};
			await WaitFrames(4);
			normalPlantSprite = null;
			int stableCardRefreshFrames = 0;
			for (int cycle = 0; cycle < 4; cycle++)
			{
				AdobeAnimateSprite previousPreview = cardPreviewSprite;
				_lateFrameAction = () =>
				{
					if (GodotObject.IsInstanceValid(previousPreview))
					{
						previousPreview.ReleaseForcedCpuPoseData();
						previousPreview.QueueFree();
					}
					cardPreviewSprite = CreateAnimationSprite(firenutScene, new Vector2(900f, 170f));
					cardPreviewSprite.forceLocalRender = true;
					cardPreviewSprite.forceCpuPoseRender = true;
					_animationHost.AddChild(cardPreviewSprite, forceReadableName: false, InternalMode.Disabled);
				};
				if (await CaptureNextDisplayedFrame(AnchorRegion) > 0)
				{
					stableCardRefreshFrames++;
				}
			}
			spawnedSprite.GetRuntimeCrowdCullingDebugState(out var _, out var _, out var _, out var _, out var nativeCanvasSuppressed);
			AdobeAnimateCrowdAggregateStats aggregateRenderStats = AdobeAnimateRenderManager.GetAggregateRenderStats();
			bool flag = ((spawnFirstFramePixels > 0 && stablePixels > 0 && visibleReattachFrames == 4 && minimumReattachPixels > 0 && unsuppressibleOriginPixels > 0 && stalePixelsAfterMove == 0 && movedPixelsAfterMove > 0 && normalPlantOriginPixels > 0 && normalPlantStalePixelsAfterMove == 0 && normalPlantMovedPixelsAfterMove > 0 && stableCardRefreshFrames == 4) & nativeCanvasSuppressed) && aggregateRenderStats.CrowdRoots == 2 && aggregateRenderStats.FallbackRoots == 0;
			GD.Print($"{"FIRENUT_FIRST_FRAME_HANDOFF_RESULT"} passed={flag} spawnFirst={spawnFirstFramePixels} stable={stablePixels} reattachVisible={visibleReattachFrames}/{4} reattachMin={minimumReattachPixels} unsuppressibleOrigin={unsuppressibleOriginPixels} staleAfterMove={stalePixelsAfterMove} movedAfterMove={movedPixelsAfterMove} normalPlantOrigin={normalPlantOriginPixels} normalPlantStaleAfterMove={normalPlantStalePixelsAfterMove} normalPlantMovedAfterMove={normalPlantMovedPixelsAfterMove} cardRefreshStable={stableCardRefreshFrames}/{4} crowd={aggregateRenderStats.CrowdRoots} fallback={aggregateRenderStats.FallbackRoots} suppressed={nativeCanvasSuppressed} renderer={RenderingServer.GetCurrentRenderingMethod()}");
			exitCode = ((!flag) ? 2 : 0);
		}
		catch (Exception value)
		{
			GD.PrintErr($"{"FIRENUT_FIRST_FRAME_HANDOFF_RESULT"} exception={value}");
		}
		finally
		{
			_lateFrameAction = null;
			if (Global.Instance != null)
			{
				Global.Instance.adobeAnimateRenderBackend = _originalBackend;
			}
			if (GodotObject.IsInstanceValid(spawnedSprite))
			{
				spawnedSprite.QueueFree();
			}
			if (GodotObject.IsInstanceValid(cardPreviewSprite))
			{
				cardPreviewSprite.ReleaseForcedCpuPoseData();
				cardPreviewSprite.QueueFree();
			}
			if (GodotObject.IsInstanceValid(unsuppressibleSprite))
			{
				unsuppressibleSprite.QueueFree();
			}
			if (GodotObject.IsInstanceValid(normalPlantSprite))
			{
				normalPlantSprite.QueueFree();
			}
			if (GodotObject.IsInstanceValid(anchorSprite))
			{
				anchorSprite.QueueFree();
			}
		}
		GetTree().Quit(exitCode);
	}

	public override void _Process(double delta)
	{
		Action lateFrameAction = _lateFrameAction;
		if (lateFrameAction != null)
		{
			_lateFrameAction = null;
			lateFrameAction();
		}
	}

	private static AdobeAnimateSprite CreateAnimationSprite(PackedScene animationScene, Vector2 position)
	{
		AdobeAnimateSprite adobeAnimateSprite = animationScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		adobeAnimateSprite.Position = position;
		adobeAnimateSprite.forceLocalRender = false;
		return adobeAnimateSprite;
	}

	private async Task WaitFrames(int count)
	{
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task<int> CaptureNextDisplayedFrame(Rect2I region)
	{
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		using Image image = GetViewport().GetTexture().GetImage();
		Rect2 visibleRect = GetViewport().GetVisibleRect();
		Vector2 vector = new Vector2((float)image.GetWidth() / visibleRect.Size.X, (float)image.GetHeight() / visibleRect.Size.Y);
		Vector2 vector2 = (new Vector2(region.Position.X, region.Position.Y) - visibleRect.Position) * vector;
		Vector2 vector3 = new Vector2(region.Size.X, region.Size.Y) * vector;
		Rect2I region2 = new Rect2I(new Vector2I(Mathf.RoundToInt(vector2.X), Mathf.RoundToInt(vector2.Y)), new Vector2I(Mathf.RoundToInt(vector3.X), Mathf.RoundToInt(vector3.Y)));
		return CountForegroundPixels(image, region2);
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
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateAnimationSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animationScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateAnimationSprite && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(CreateAnimationSprite(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
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
		if (method == MethodName.CreateAnimationSprite && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(CreateAnimationSprite(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.CreateAnimationSprite)
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
		if (name == PropertyName._animationHost)
		{
			_animationHost = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._originalBackend)
		{
			_originalBackend = VariantUtils.ConvertTo<AdobeAnimateRenderBackend>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._animationHost)
		{
			value = VariantUtils.CreateFrom(in _animationHost);
			return true;
		}
		if (name == PropertyName._originalBackend)
		{
			value = VariantUtils.CreateFrom(in _originalBackend);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._animationHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._animationHost, Variant.From(in _animationHost));
		info.AddProperty(PropertyName._originalBackend, Variant.From(in _originalBackend));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._animationHost, out var value))
		{
			_animationHost = value.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._originalBackend, out var value2))
		{
			_originalBackend = value2.As<AdobeAnimateRenderBackend>();
		}
	}
}
