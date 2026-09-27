using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/ZombieNormalRasterCompositeRuntimeTest.cs")]
public class ZombieNormalRasterCompositeRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ApplyProductionDormantArmorReplacements = "ApplyProductionDormantArmorReplacements";

		public static readonly StringName CreateEmptyMediaPaths = "CreateEmptyMediaPaths";

		public static readonly StringName CountRasterStates = "CountRasterStates";

		public static readonly StringName CountVisiblePixels = "CountVisiblePixels";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _originalMaxFps = "_originalMaxFps";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "ZOMBIE_NORMAL_RASTER_COMPOSITE_RESULT";

	private const string SpriteScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Sprite/Normal/ZombieNormal.tscn";

	private const int VisibleSpriteCount = 24;

	private readonly List<AdobeAnimateSprite> _sprites = new List<AdobeAnimateSprite>(24);

	private readonly List<string> _failures = new List<string>();

	private AdobeAnimateRenderBackend _originalBackend;

	private int _originalMaxFps;

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
			_originalMaxFps = Engine.MaxFps;
			Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
			AdobeAnimateRenderManager.RasterCompositeEnabled = true;
			Engine.MaxFps = Math.Max(120, Engine.PhysicsTicksPerSecond * 2);
			PackedScene packedScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Sprite/Normal/ZombieNormal.tscn");
			if (!GodotObject.IsInstanceValid(packedScene))
			{
				throw new InvalidOperationException("ZombieNormal sprite scene could not be loaded.");
			}
			for (int i = 0; i < 24; i++)
			{
				AdobeAnimateSprite adobeAnimateSprite = packedScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
				if (!GodotObject.IsInstanceValid(adobeAnimateSprite))
				{
					throw new InvalidOperationException($"ZombieNormal sprite {i} could not be instantiated.");
				}
				adobeAnimateSprite.Name = $"RasterZombie{i}";
				adobeAnimateSprite.Position = new Vector2(80f + (float)(i % 12) * 82f, 160f + (float)(i / 12) * 230f);
				adobeAnimateSprite.SetFliter("anim_tongue", (i & 1) != 0);
				AdobeAnimateSprite adobeAnimateSprite2 = adobeAnimateSprite;
				string clipName;
				if (i % 3 == 2)
				{
					clipName = "Eat";
				}
				else
				{
					clipName = ((i % 3 == 1) ? "Walk2" : "Walk1");
				}
				adobeAnimateSprite2.SetAnimation(clipName);
				if (i >= 12)
				{
					ApplyProductionDormantArmorReplacements(adobeAnimateSprite);
				}
				if (i % 3 == 0)
				{
					adobeAnimateSprite.SetVerticalClip(enabled: true, -10000f, 10000f);
				}
				AddChild(adobeAnimateSprite, forceReadableName: false, InternalMode.Disabled);
				_sprites.Add(adobeAnimateSprite);
			}
			await WaitProcessFrames(24);
			int rasterStates = CountRasterStates();
			AdobeAnimateCrowdAggregateStats stats = AdobeAnimateRenderManager.GetAggregateRenderStats();
			Check(rasterStates == 24, $"Every pristine Normal zombie must use RasterComposite; actual={rasterStates}/{24}.");
			Check(stats.CrowdRoots >= 24, $"GPU Crowd did not publish every visible root; crowd={stats.CrowdRoots} fallback={stats.FallbackRoots}.");
			Check(stats.FallbackRoots == 0, $"Pristine Normal zombies unexpectedly used fallback roots={stats.FallbackRoots}.");
			Check(stats.CrowdMeshQuadCapacity > 0 && stats.CrowdMeshQuadCapacity <= 8, $"Raster batch must stay within the one-quad mesh growth floor; capacity={stats.CrowdMeshQuadCapacity}.");
			Vector2[] originalPositions = new Vector2[_sprites.Count];
			for (int j = 0; j < _sprites.Count; j++)
			{
				originalPositions[j] = _sprites[j].Position;
				_sprites[j].Position += new Vector2(18f, -6f);
			}
			await WaitProcessFrames(3);
			int translatedRasterStates = CountRasterStates();
			Check(translatedRasterStates == 24, $"Translation-only movement must retain every raster state; actual={translatedRasterStates}/{24}.");
			for (int k = 0; k < _sprites.Count; k++)
			{
				_sprites[k].Position += new Vector2(5000f, 0f);
			}
			await WaitProcessFrames(3);
			int offscreenRasterStates = CountRasterStates();
			Check(offscreenRasterStates == 0, $"Translation-only movement outside the padded viewport must cull every raster state; actual={offscreenRasterStates}.");
			for (int l = 0; l < _sprites.Count; l++)
			{
				_sprites[l].Position = originalPositions[l];
			}
			await WaitProcessFrames(3);
			int restoredRasterStates = CountRasterStates();
			Check(restoredRasterStates == 24, $"Returning translated sprites to the viewport must restore every raster state; actual={restoredRasterStates}/{24}.");
			await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
			using Image capture = GetViewport().GetTexture().GetImage();
			int visiblePixelCount = CountVisiblePixels(capture);
			int repeatedQuadLeakPixels = CountVisiblePixels(capture, 0, 520, 1080, 600);
			Check(visiblePixelCount >= 12000, $"Raster composite output is blank or incomplete; visiblePixels={visiblePixelCount}.");
			Check(repeatedQuadLeakPixels == 0, $"Unused shared-mesh slots repeated the raster quad; leakPixels={repeatedQuadLeakPixels}.");
			string capturePath = ProjectSettings.GlobalizePath("user://ZombieNormalRasterCompositeRuntime.png");
			Check(capture.SavePng(capturePath) == Error.Ok, "Runtime visual capture could not be saved to " + capturePath + ".");
			AdobeAnimateSprite dynamicSprite = _sprites[0];
			dynamicSprite.SetFliter("Zombie_outerarm_upper", open: false);
			await WaitProcessFrames(3);
			AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult = dynamicSprite.TryBuildCrowdRenderState(out var fallbackState);
			Check(adobeAnimateCrowdRenderStateResult == AdobeAnimateCrowdRenderStateResult.Submitted && fallbackState.Mode != AdobeAnimateCrowdRenderMode.RasterComposite, "A dynamic layer mutation must fall back to the original GPU rendering path.");
			AdobeAnimateSprite replacementSprite = _sprites[1];
			Array<string> array = CreateEmptyMediaPaths();
			array[1] = "res://Asset/AtlasSource/Armor/Texture/Character/Armor/Bucket/ZombieBucket1.png";
			replacementSprite.mediaReplaceAtlasPaths = array;
			await WaitProcessFrames(3);
			AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult2 = replacementSprite.TryBuildCrowdRenderState(out var state);
			Check(adobeAnimateCrowdRenderStateResult2 == AdobeAnimateCrowdRenderStateResult.Submitted && state.Mode != AdobeAnimateCrowdRenderMode.RasterComposite, "A replacement on a visible body media ID must fall back to the original GPU rendering path.");
			bool flag = _failures.Count == 0;
			GD.Print($"{"ZOMBIE_NORMAL_RASTER_COMPOSITE_RESULT"} passed={flag} raster={rasterStates}/{24} translated={translatedRasterStates}/{24} offscreen={offscreenRasterStates} restored={restoredRasterStates}/{24} visiblePixels={visiblePixelCount} repeatedQuadLeakPixels={repeatedQuadLeakPixels} crowdRoots={stats.CrowdRoots} fallbackRoots={stats.FallbackRoots} quadCapacity={stats.CrowdMeshQuadCapacity} dynamicMode={fallbackState.Mode} renderer={RenderingServer.GetCurrentRenderingMethod()} capture={capturePath}");
			foreach (string failure in _failures)
			{
				GD.PrintErr("ZOMBIE_NORMAL_RASTER_COMPOSITE_RESULT failure=" + failure);
			}
			exitCode = ((!flag) ? 2 : 0);
		}
		catch (Exception value)
		{
			GD.PrintErr($"{"ZOMBIE_NORMAL_RASTER_COMPOSITE_RESULT"} exception={value}");
		}
		finally
		{
			if (Global.Instance != null)
			{
				Global.Instance.adobeAnimateRenderBackend = _originalBackend;
			}
			Engine.MaxFps = _originalMaxFps;
		}
		GetTree().Quit(exitCode);
	}

	private static void ApplyProductionDormantArmorReplacements(AdobeAnimateSprite sprite)
	{
		Array<string> array = CreateEmptyMediaPaths();
		array[2] = "res://Asset/AtlasSource/Armor/Texture/Character/Armor/Bucket/ZombieBucket1.png";
		array[3] = "res://Asset/AtlasSource/Armor/Texture/Character/Armor/Cone/ZombieCone1.png";
		sprite.mediaReplaceAtlasPaths = array;
	}

	private static Array<string> CreateEmptyMediaPaths()
	{
		Array<string> array = new Array<string>();
		array.Resize(38);
		return array;
	}

	private int CountRasterStates()
	{
		int num = 0;
		foreach (AdobeAnimateSprite sprite in _sprites)
		{
			if (sprite.TryBuildCrowdRenderState(out var state) == AdobeAnimateCrowdRenderStateResult.Submitted && state.Mode == AdobeAnimateCrowdRenderMode.RasterComposite)
			{
				num++;
			}
		}
		return num;
	}

	private static int CountVisiblePixels(Image image)
	{
		return CountVisiblePixels(image, 20, 40, 1060, 560);
	}

	private static int CountVisiblePixels(Image image, int left, int top, int right, int bottom)
	{
		int num = 0;
		for (int i = Math.Max(0, top); i < Math.Min(image.GetHeight(), bottom); i += 2)
		{
			for (int j = Math.Max(0, left); j < Math.Min(image.GetWidth(), right); j += 2)
			{
				Color pixel = image.GetPixel(j, i);
				if (pixel.R + pixel.G + pixel.B >= 0.12f)
				{
					num++;
				}
			}
		}
		return num;
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Check(bool condition, string failure)
	{
		if (!condition)
		{
			_failures.Add(failure);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyProductionDormantArmorReplacements, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEmptyMediaPaths, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CountRasterStates, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountVisiblePixels, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false)
			}, null),
			new MethodInfo(MethodName.CountVisiblePixels, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Int, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "top", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "bottom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "failure", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ApplyProductionDormantArmorReplacements && args.Count == 1)
		{
			ApplyProductionDormantArmorReplacements(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateEmptyMediaPaths && args.Count == 0)
		{
			Array<string> array = CreateEmptyMediaPaths();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CountRasterStates && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountRasterStates());
			return true;
		}
		if (method == MethodName.CountVisiblePixels && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountVisiblePixels(VariantUtils.ConvertTo<Image>(in args[0])));
			return true;
		}
		if (method == MethodName.CountVisiblePixels && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<int>(CountVisiblePixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4])));
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
		if (method == MethodName.ApplyProductionDormantArmorReplacements && args.Count == 1)
		{
			ApplyProductionDormantArmorReplacements(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateEmptyMediaPaths && args.Count == 0)
		{
			Array<string> array = CreateEmptyMediaPaths();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CountVisiblePixels && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountVisiblePixels(VariantUtils.ConvertTo<Image>(in args[0])));
			return true;
		}
		if (method == MethodName.CountVisiblePixels && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<int>(CountVisiblePixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4])));
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
		if (method == MethodName.ApplyProductionDormantArmorReplacements)
		{
			return true;
		}
		if (method == MethodName.CreateEmptyMediaPaths)
		{
			return true;
		}
		if (method == MethodName.CountRasterStates)
		{
			return true;
		}
		if (method == MethodName.CountVisiblePixels)
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
