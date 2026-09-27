using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateZombieWonGpuFlickerProbe.cs")]
public class AdobeAnimateZombieWonGpuFlickerProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CountForegroundPixels = "CountForegroundPixels";

		public static readonly StringName MeasureLocalContrast = "MeasureLocalContrast";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string Marker = "ZOMBIE_WON_GPU_FLICKER_PROBE";

	private static readonly Color BackgroundColor = new Color(0.02f, 0.02f, 0.02f);

	private AdobeAnimateRenderBackend _originalBackend;

	public override async void _Ready()
	{
		int exitCode = 2;
		try
		{
			if (Global.Instance == null)
			{
				throw new InvalidOperationException("Global autoload is unavailable.");
			}
			_originalBackend = Global.Instance.adobeAnimateRenderBackend;
			Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
			ColorRect node = new ColorRect
			{
				Color = BackgroundColor,
				Position = Vector2.Zero,
				Size = new Vector2(1080f, 600f),
				MouseFilter = Control.MouseFilterEnum.Ignore
			};
			AddChild(node, forceReadableName: false, InternalMode.Disabled);
			CanvasLayer canvasLayer = new CanvasLayer
			{
				Layer = 0,
				FollowViewportEnabled = true
			};
			AddChild(canvasLayer, forceReadableName: false, InternalMode.Disabled);
			Node2D battleHost = new Node2D();
			canvasLayer.AddChild(battleHost, forceReadableName: false, InternalMode.Disabled);
			PackedScene packedScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/PeaShooter/PeaShooter.tscn");
			AdobeAnimateSprite pea = packedScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			pea.Position = new Vector2(970f, 300f);
			pea.SetAnimation("Idle");
			battleHost.AddChild(pea, forceReadableName: false, InternalMode.Disabled);
			CanvasLayer canvasLayer2 = new CanvasLayer
			{
				Layer = 10
			};
			AddChild(canvasLayer2, forceReadableName: false, InternalMode.Disabled);
			PackedScene packedScene2 = GD.Load<PackedScene>("res://Prefab/TowerDefense/GUI/InGame/ZombieWon/TowerDefenseZombieWon.tscn");
			TowerDefenseZombieWon zombieWon = packedScene2.Instantiate<TowerDefenseZombieWon>(PackedScene.GenEditState.Disabled);
			canvasLayer2.AddChild(zombieWon, forceReadableName: false, InternalMode.Disabled);
			await WaitFrames(16);
			battleHost.ProcessMode = ProcessModeEnum.Disabled;
			await WaitFrames(4);
			int baseline = await CaptureForegroundPixels(880, 1080, 60, 540);
			pea.GetRuntimeCrowdCullingDebugState(out var cached, out var cachedVisible, out var currentVisible, out var currentVisibleWithPrefetch, out var baselineNativeCanvasSuppressed);
			pea.Position = new Vector2(5000f, 300f);
			pea.NotifyAncestorTransformChangedForRender();
			AdobeAnimateRenderManager.RenderActive(new AdobeAnimateSprite[1] { pea }, activeListAlreadyValidated: true, renderRootsAlreadyFiltered: true);
			pea.GetRuntimeCrowdCullingDebugState(out currentVisibleWithPrefetch, out currentVisible, out cachedVisible, out cached, out var skippedNativeCanvasSuppressed);
			pea.Position = new Vector2(970f, 300f);
			pea.NotifyAncestorTransformChangedForRender();
			AdobeAnimateRenderManager.RenderActive(new AdobeAnimateSprite[1] { pea }, activeListAlreadyValidated: true, renderRootsAlreadyFiltered: true);
			await WaitFrames(4);
			int recoveredBattle = await CaptureForegroundPixels(880, 1080, 60, 540);
			long buildsBefore = AdobeAnimateRenderManager.GpuRenderGraphBuildCount;
			int allocationsBefore = AdobeAnimateRenderManager.GpuRenderGraphAtlasAllocationCount;
			zombieWon.LevelFail();
			int minBattle = 2147483647;
			int maxBattle = 0;
			int blankBattleFrames = 0;
			int minZombieWon = 2147483647;
			int maxZombieWon = 0;
			int blankZombieWonFrames = 0;
			int crowdZeroFrames = 0;
			double tailSignalMin = 1.7976931348623157E+308;
			double tailSignalMax = 0.0;
			for (int frame = 0; frame < 150; frame++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
				using Image image = GetViewport().GetTexture().GetImage();
				int num = CountForegroundPixels(image, 880, 1080, 60, 540);
				double val = MeasureLocalContrast(image, 880, 1080, 60, 540, 1070, 50);
				minBattle = Math.Min(minBattle, num);
				maxBattle = Math.Max(maxBattle, num);
				if (baseline > 0 && num < baseline / 5)
				{
					blankBattleFrames++;
				}
				if (frame >= 75)
				{
					tailSignalMin = Math.Min(tailSignalMin, val);
					tailSignalMax = Math.Max(tailSignalMax, val);
				}
				if (frame >= 45)
				{
					int num2 = CountForegroundPixels(image, 80, 850, 20, 580);
					minZombieWon = Math.Min(minZombieWon, num2);
					maxZombieWon = Math.Max(maxZombieWon, num2);
					if (num2 < 100)
					{
						blankZombieWonFrames++;
					}
				}
				AdobeAnimateCrowdAggregateStats aggregateRenderStats = AdobeAnimateRenderManager.GetAggregateRenderStats();
				if (aggregateRenderStats.CrowdRoots + aggregateRenderStats.FallbackRoots <= 0)
				{
					crowdZeroFrames++;
				}
			}
			long gpuRenderGraphBuildCount = AdobeAnimateRenderManager.GpuRenderGraphBuildCount;
			int gpuRenderGraphAtlasAllocationCount = AdobeAnimateRenderManager.GpuRenderGraphAtlasAllocationCount;
			bool flag = ((baseline > 0) & baselineNativeCanvasSuppressed) && !skippedNativeCanvasSuppressed && recoveredBattle >= baseline / 5 && blankBattleFrames == 0 && blankZombieWonFrames == 0 && crowdZeroFrames == 0;
			GD.Print($"{"ZOMBIE_WON_GPU_FLICKER_PROBE"} passed={flag} baseline={baseline} battleMin={minBattle} battleMax={maxBattle} baselineSuppressed={baselineNativeCanvasSuppressed} skippedSuppressed={skippedNativeCanvasSuppressed} recoveredBattle={recoveredBattle} battleBlank={blankBattleFrames} zombieMin={minZombieWon} zombieMax={maxZombieWon} zombieBlank={blankZombieWonFrames} crowdZero={crowdZeroFrames} tailSignalMin={tailSignalMin:F3} tailSignalMax={tailSignalMax:F3} builds={buildsBefore}->{gpuRenderGraphBuildCount} allocations={allocationsBefore}->{gpuRenderGraphAtlasAllocationCount} renderer={RenderingServer.GetCurrentRenderingMethod()}");
			exitCode = ((!flag) ? 2 : 0);
		}
		catch (Exception value)
		{
			GD.PrintErr($"{"ZOMBIE_WON_GPU_FLICKER_PROBE"} exception={value}");
		}
		finally
		{
			if (Global.Instance != null)
			{
				Global.Instance.adobeAnimateRenderBackend = _originalBackend;
			}
		}
		GetTree().Quit(exitCode);
	}

	private async Task WaitFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task<int> CaptureForegroundPixels(int x0, int x1, int y0, int y1)
	{
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		using Image image = GetViewport().GetTexture().GetImage();
		return CountForegroundPixels(image, x0, x1, y0, y1);
	}

	private static int CountForegroundPixels(Image image, int x0, int x1, int y0, int y1)
	{
		int num = 0;
		int num2 = Math.Min(x1, image.GetWidth());
		int num3 = Math.Min(y1, image.GetHeight());
		for (int i = Math.Max(0, y0); i < num3; i++)
		{
			for (int j = Math.Max(0, x0); j < num2; j++)
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

	private static double MeasureLocalContrast(Image image, int x0, int x1, int y0, int y1, int sampleX, int sampleY)
	{
		Color pixel = image.GetPixel(sampleX, sampleY);
		double num = 0.0;
		int num2 = Math.Min(x1, image.GetWidth());
		int num3 = Math.Min(y1, image.GetHeight());
		for (int i = Math.Max(0, y0); i < num3; i += 2)
		{
			for (int j = Math.Max(0, x0); j < num2; j += 2)
			{
				Color pixel2 = image.GetPixel(j, i);
				num += (double)(Math.Abs(pixel2.R - pixel.R) + Math.Abs(pixel2.G - pixel.G) + Math.Abs(pixel2.B - pixel.B));
			}
		}
		return num;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountForegroundPixels, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Int, "x0", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "x1", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "y0", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "y1", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MeasureLocalContrast, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Int, "x0", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "x1", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "y0", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "y1", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sampleX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sampleY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CountForegroundPixels && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<int>(CountForegroundPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4])));
			return true;
		}
		if (method == MethodName.MeasureLocalContrast && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<double>(MeasureLocalContrast(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<int>(in args[6])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CountForegroundPixels && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<int>(CountForegroundPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4])));
			return true;
		}
		if (method == MethodName.MeasureLocalContrast && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<double>(MeasureLocalContrast(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<int>(in args[6])));
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
		if (method == MethodName.CountForegroundPixels)
		{
			return true;
		}
		if (method == MethodName.MeasureLocalContrast)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._originalBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._originalBackend, Variant.From(in _originalBackend));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._originalBackend, out var value))
		{
			_originalBackend = value.As<AdobeAnimateRenderBackend>();
		}
	}
}
