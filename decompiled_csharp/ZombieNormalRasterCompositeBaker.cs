using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/ZombieNormalRasterCompositeBaker.cs")]
public class ZombieNormalRasterCompositeBaker : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ComputeLayerVisibilitySignature = "ComputeLayerVisibilitySignature";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _originalMaxFps = "_originalMaxFps";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "ZOMBIE_NORMAL_RASTER_BAKER_RESULT";

	private const string SpriteScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Sprite/Normal/ZombieNormal.tscn";

	private const string AtlasOutputPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Generated/ZombieNormalRasterComposite.png";

	private static readonly string[] Clips = new string[3] { "Walk1", "Walk2", "Eat" };

	private static readonly Vector2I ViewportSize = new Vector2I(320, 320);

	private static readonly Vector2 CaptureAnchor = new Vector2(160f, 220f);

	private const int Padding = 2;

	private const int AtlasColumns = 32;

	private AdobeAnimateRenderBackend _originalBackend;

	private int _originalMaxFps;

	public override async void _Ready()
	{
		int exitCode = 2;
		List<Image> frames = new List<Image>(262);
		try
		{
			if (Global.Instance == null)
			{
				throw new InvalidOperationException("Global autoload is unavailable.");
			}
			_originalBackend = Global.Instance.adobeAnimateRenderBackend;
			_originalMaxFps = Engine.MaxFps;
			Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
			Engine.MaxFps = Math.Max(120, Engine.PhysicsTicksPerSecond * 2);
			SubViewport viewport = new SubViewport
			{
				Size = ViewportSize,
				TransparentBg = true,
				RenderTargetClearMode = SubViewport.ClearMode.Always,
				RenderTargetUpdateMode = SubViewport.UpdateMode.Always,
				Disable3D = true
			};
			AddChild(viewport, forceReadableName: false, InternalMode.Disabled);
			AdobeAnimateSprite sprite = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Sprite/Normal/ZombieNormal.tscn")?.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(sprite))
			{
				throw new InvalidOperationException("ZombieNormal animation scene could not be instantiated.");
			}
			sprite.Position = CaptureAnchor;
			sprite.timeScale = 0.0;
			sprite.pause = false;
			viewport.AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
			await WaitProcessFrames(12);
			Rect2I union = default;
			bool hasPixels = false;
			List<ulong> variantSignatures = new List<ulong>(2);
			for (int variant = 0; variant < 2; variant++)
			{
				sprite.SetFliter("anim_tongue", variant == 1);
				await WaitProcessFrames(2);
				variantSignatures.Add(ComputeLayerVisibilitySignature(sprite.layerVisible));
				string[] clips = Clips;
				foreach (string clip in clips)
				{
					sprite.SetAnimation(clip);
					await WaitProcessFrames(2);
					for (int frame = sprite.clipRange.X; frame < sprite.clipRange.Y; frame++)
					{
						sprite.frameIndex = frame;
						sprite.elapsedTimer = 0.0;
						sprite.QueueRedraw();
						await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
						await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
						Image image = viewport.GetTexture().GetImage();
						Rect2I usedRect = image.GetUsedRect();
						if (usedRect.Size.X <= 0 || usedRect.Size.Y <= 0)
						{
							image.Dispose();
							throw new InvalidOperationException($"Captured frame is blank: variant={variant} clip={clip} frame={frame}.");
						}
						union = (hasPixels ? union.Merge(usedRect) : usedRect);
						hasPixels = true;
						frames.Add(image);
					}
				}
			}
			if (frames.Count != 262)
			{
				throw new InvalidOperationException($"Expected 262 frames, captured {frames.Count}.");
			}
			Rect2I srcRect = union.Grow(2).Intersection(new Rect2I(Vector2I.Zero, ViewportSize));
			int num = Mathf.CeilToInt((float)frames.Count / 32f);
			Vector2I vector2I = new Vector2I(srcRect.Size.X * 32, srcRect.Size.Y * num);
			using Image image2 = Image.CreateEmpty(vector2I.X, vector2I.Y, useMipmaps: false, Image.Format.Rgba8);
			image2.Fill(Colors.Transparent);
			for (int j = 0; j < frames.Count; j++)
			{
				image2.BlitRect(dst: new Vector2I(j % 32 * srcRect.Size.X, j / 32 * srcRect.Size.Y), src: frames[j], srcRect: srcRect);
			}
			string text = ProjectSettings.GlobalizePath("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Generated/ZombieNormalRasterComposite.png");
			DirAccess.MakeDirRecursiveAbsolute(text.GetBaseDir());
			Error error = image2.SavePng(text);
			if (error != Error.Ok)
			{
				throw new InvalidOperationException($"Failed to save raster atlas: {error}.");
			}
			Vector2 vector = new Vector2(srcRect.Position.X, srcRect.Position.Y) - CaptureAnchor;
			GD.Print($"{"ZOMBIE_NORMAL_RASTER_BAKER_RESULT"} passed=True frames={frames.Count} variants=2 signature0={variantSignatures[0]} signature1={variantSignatures[1]} tile_w={srcRect.Size.X} tile_h={srcRect.Size.Y} origin_x={vector.X} origin_y={vector.Y} atlas_w={vector2I.X} atlas_h={vector2I.Y} columns={32} path={"res://Asset/Anime/Character/Zombie/Chapter1/Normal/Generated/ZombieNormalRasterComposite.png"} renderer={RenderingServer.GetCurrentRenderingMethod()}");
			exitCode = 0;
		}
		catch (Exception value)
		{
			GD.PrintErr($"{"ZOMBIE_NORMAL_RASTER_BAKER_RESULT"} exception={value}");
		}
		finally
		{
			for (int k = 0; k < frames.Count; k++)
			{
				frames[k]?.Dispose();
			}
			if (Global.Instance != null)
			{
				Global.Instance.adobeAnimateRenderBackend = _originalBackend;
			}
			Engine.MaxFps = _originalMaxFps;
		}
		GetTree().Quit(exitCode);
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private static ulong ComputeLayerVisibilitySignature(Array<bool> values)
	{
		ulong num = 1469598103934665603uL;
		for (int i = 0; i < values.Count; i++)
		{
			num ^= (ulong)(values[i] ? 1 : 0);
			num *= 1099511628211L;
		}
		return num;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComputeLayerVisibilitySignature, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "values", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ComputeLayerVisibilitySignature && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ulong>(ComputeLayerVisibilitySignature(VariantUtils.ConvertToArray<bool>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ComputeLayerVisibilitySignature && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ulong>(ComputeLayerVisibilitySignature(VariantUtils.ConvertToArray<bool>(in args[0])));
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
		if (method == MethodName.ComputeLayerVisibilitySignature)
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
