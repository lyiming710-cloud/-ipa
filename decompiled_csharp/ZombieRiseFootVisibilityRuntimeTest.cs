using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ZombieRiseFootVisibilityRuntimeTest.cs")]
public class ZombieRiseFootVisibilityRuntimeTest : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ResolveGroundScreenY = "ResolveGroundScreenY";

		public static readonly StringName CountFootPixels = "CountFootPixels";

		public static readonly StringName ColorDistance = "ColorDistance";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _originalRasterCompositeEnabled = "_originalRasterCompositeEnabled";

		public static readonly StringName _originalTimeScale = "_originalTimeScale";

		public static readonly StringName _zombie = "_zombie";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const string NormalZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string ResultMarker = "ZOMBIE_RISE_FOOT_VISIBILITY_RESULT";

	private static readonly Color BackgroundColor = new Color(0.02f, 0.02f, 0.025f);

	private AdobeAnimateRenderBackend _originalBackend;

	private bool _originalRasterCompositeEnabled;

	private double _originalTimeScale;

	private TowerDefenseZombieNormal _zombie;

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
			_originalRasterCompositeEnabled = AdobeAnimateRenderManager.RasterCompositeEnabled;
			_originalTimeScale = Engine.TimeScale;
			Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
			AdobeAnimateRenderManager.RasterCompositeEnabled = true;
			Engine.TimeScale = 2.0;
			ColorRect node = new ColorRect
			{
				Color = BackgroundColor,
				Position = Vector2.Zero,
				Size = new Vector2(1080f, 600f),
				MouseFilter = Control.MouseFilterEnum.Ignore,
				ZIndex = -4096
			};
			AddChild(node, forceReadableName: false, InternalMode.Disabled);
			PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
			if (!GodotObject.IsInstanceValid(packedScene))
			{
				throw new InvalidOperationException("Normal zombie scene could not be loaded.");
			}
			_zombie = packedScene.Instantiate<TowerDefenseZombieNormal>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(_zombie))
			{
				throw new InvalidOperationException("Normal zombie scene could not be instantiated.");
			}
			_zombie.inGame = false;
			_zombie.editorPreviewMode = false;
			_zombie.Position = new Vector2(540f, 330f);
			_zombie.Visible = true;
			AddChild(_zombie, forceReadableName: false, InternalMode.Disabled);
			await WaitProcessFrames(16);
			_zombie.sprite.SetAnimation("Walk1");
			_zombie.sprite.timeScale = 0.0;
			_zombie.sprite.SetVerticalClip(enabled: false, -10000f, 10000f);
			await WaitProcessFrames(6);
			int groundScreenY = ResolveGroundScreenY(_zombie);
			using Image baseline = await CaptureFrame();
			int baselineFootPixels = CountFootPixels(baseline, groundScreenY);
			if (baselineFootPixels < 24)
			{
				throw new InvalidOperationException($"Baseline zombie feet are not visible enough; pixels={baselineFootPixels} groundY={groundScreenY}.");
			}
			_zombie.Rise(1.0, 0.0, createDirt: false);
			await WaitForNearSurfaceRise(240);
			double nearSurfaceGroundHeight = _zombie.groundHeight;
			using Image nearSurface = await CaptureFrame();
			int nearSurfaceFootPixels = CountFootPixels(nearSurface, groundScreenY);
			VerticalClipState nearSurfaceClip = _zombie.sprite.GetVerticalClipState();
			bool nearSurfaceFeetVisible = _zombie.isRise && nearSurfaceFootPixels >= Mathf.CeilToInt((float)baselineFootPixels * 0.25f);
			bool nearSurfaceClipHasFootMargin = nearSurfaceClip.Enabled && nearSurfaceClip.DownY >= (float)groundScreenY + 16f;
			string nearSurfaceCapturePath = ProjectSettings.GlobalizePath("user://ZombieRiseFootVisibilityNearSurface.png");
			nearSurface.SavePng(nearSurfaceCapturePath);
			await WaitForRiseCompletion(180);
			_zombie.sprite.SetAnimation("Walk1");
			_zombie.sprite.timeScale = 0.0;
			await WaitProcessFrames(6);
			using Image image = await CaptureFrame();
			int num = CountFootPixels(image, groundScreenY);
			VerticalClipState verticalClipState = _zombie.sprite.GetVerticalClipState();
			bool visible = _zombie.shadowSprite.Visible;
			bool flag = num >= Mathf.CeilToInt((float)baselineFootPixels * 0.9f);
			bool flag2 = ((nearSurfaceFeetVisible & nearSurfaceClipHasFootMargin) && !_zombie.isRise && !verticalClipState.Enabled) & visible & flag;
			string text = ProjectSettings.GlobalizePath("user://ZombieRiseFootVisibilityRuntime.png");
			image.SavePng(text);
			GD.Print($"{"ZOMBIE_RISE_FOOT_VISIBILITY_RESULT"} passed={flag2} nearSurfaceGroundHeight={nearSurfaceGroundHeight:F2} nearSurfaceClipEnabled={nearSurfaceClip.Enabled} nearSurfaceClipDown={nearSurfaceClip.DownY:F1} nearSurfaceClipHasFootMargin={nearSurfaceClipHasFootMargin} nearSurfaceFootPixels={nearSurfaceFootPixels} nearSurfaceFeetVisible={nearSurfaceFeetVisible} isRise={_zombie.isRise} clipEnabled={verticalClipState.Enabled} clipDown={verticalClipState.DownY:F1} shadowVisible={visible} baselineFootPixels={baselineFootPixels} settledFootPixels={num} groundY={groundScreenY} renderer={RenderingServer.GetCurrentRenderingMethod()} nearCapture={nearSurfaceCapturePath} capture={text}");
			exitCode = ((!flag2) ? 2 : 0);
		}
		catch (Exception value)
		{
			GD.PrintErr($"{"ZOMBIE_RISE_FOOT_VISIBILITY_RESULT"} passed=False exception={value}");
		}
		finally
		{
			if (Global.Instance != null)
			{
				Global.Instance.adobeAnimateRenderBackend = _originalBackend;
			}
			AdobeAnimateRenderManager.RasterCompositeEnabled = _originalRasterCompositeEnabled;
			Engine.TimeScale = _originalTimeScale;
			if (GodotObject.IsInstanceValid(_zombie))
			{
				_zombie.QueueFree();
			}
		}
		GetTree().Quit(exitCode);
	}

	private static int ResolveGroundScreenY(TowerDefenseCharacter character)
	{
		Transform2D screenTransform = character.GetViewport().GetScreenTransform();
		screenTransform.Origin = Vector2.Zero;
		Vector2 vector = character.GetLogicalGlobalPosition(character.transformPoint) + new Vector2(0f, (float)character.groundHeight);
		return Mathf.RoundToInt((screenTransform * vector).Y);
	}

	private static int CountFootPixels(Image image, int groundScreenY)
	{
		int num = 0;
		int num2 = Math.Max(0, 465);
		int num3 = Math.Min(image.GetWidth(), 615);
		int num4 = Math.Max(0, groundScreenY + 1);
		int num5 = Math.Min(image.GetHeight(), groundScreenY + 34);
		for (int i = num4; i < num5; i++)
		{
			for (int j = num2; j < num3; j++)
			{
				if (ColorDistance(image.GetPixel(j, i), BackgroundColor) > 0.08f)
				{
					num++;
				}
			}
		}
		return num;
	}

	private async Task WaitForRiseCompletion(int maximumFrames)
	{
		for (int frame = 0; frame < maximumFrames; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (!_zombie.isRise)
			{
				return;
			}
		}
		throw new TimeoutException($"Rise did not complete within {maximumFrames} process frames.");
	}

	private async Task WaitForNearSurfaceRise(int maximumFrames)
	{
		for (int frame = 0; frame < maximumFrames; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (_zombie.isRise && _zombie.groundHeight >= -8.0)
			{
				return;
			}
			if (!_zombie.isRise)
			{
				break;
			}
		}
		throw new TimeoutException($"Rise did not enter the near-surface interval within {maximumFrames} process frames.");
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task<Image> CaptureFrame()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		return GetViewport().GetTexture().GetImage();
	}

	private static float ColorDistance(Color left, Color right)
	{
		return Mathf.Max(Mathf.Max(Mathf.Abs(left.R - right.R), Mathf.Abs(left.G - right.G)), Mathf.Max(Mathf.Abs(left.B - right.B), Mathf.Abs(left.A - right.A)));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveGroundScreenY, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CountFootPixels, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Int, "groundScreenY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ColorDistance, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ResolveGroundScreenY && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveGroundScreenY(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.CountFootPixels && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountFootPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ColorDistance && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(ColorDistance(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveGroundScreenY && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveGroundScreenY(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.CountFootPixels && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountFootPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ColorDistance && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(ColorDistance(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
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
		if (method == MethodName.ResolveGroundScreenY)
		{
			return true;
		}
		if (method == MethodName.CountFootPixels)
		{
			return true;
		}
		if (method == MethodName.ColorDistance)
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
		if (name == PropertyName._originalTimeScale)
		{
			_originalTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._zombie)
		{
			_zombie = VariantUtils.ConvertTo<TowerDefenseZombieNormal>(in value);
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
		if (name == PropertyName._originalTimeScale)
		{
			value = VariantUtils.CreateFrom(in _originalTimeScale);
			return true;
		}
		if (name == PropertyName._zombie)
		{
			value = VariantUtils.CreateFrom(in _zombie);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName._originalRasterCompositeEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._originalTimeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zombie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._originalBackend, Variant.From(in _originalBackend));
		info.AddProperty(PropertyName._originalRasterCompositeEnabled, Variant.From(in _originalRasterCompositeEnabled));
		info.AddProperty(PropertyName._originalTimeScale, Variant.From(in _originalTimeScale));
		info.AddProperty(PropertyName._zombie, Variant.From(in _zombie));
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
		if (info.TryGetProperty(PropertyName._originalTimeScale, out var value3))
		{
			_originalTimeScale = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._zombie, out var value4))
		{
			_zombie = value4.As<TowerDefenseZombieNormal>();
		}
	}
}
