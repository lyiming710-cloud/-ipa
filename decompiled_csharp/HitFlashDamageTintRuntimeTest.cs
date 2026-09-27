using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/HitFlashDamageTintRuntimeTest.cs")]
public class HitFlashDamageTintRuntimeTest : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InstallHighLoadCadenceFixture = "InstallHighLoadCadenceFixture";

		public static readonly StringName RestoreHighLoadCadenceFixture = "RestoreHighLoadCadenceFixture";

		public static readonly StringName MeasureAgainstBaselineMask = "MeasureAgainstBaselineMask";

		public static readonly StringName MeasureAverageRgbDifference = "MeasureAverageRgbDifference";

		public static readonly StringName ColorDistance = "ColorDistance";

		public static readonly StringName Ensure = "Ensure";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _originalRasterCompositeEnabled = "_originalRasterCompositeEnabled";

		public static readonly StringName _originalMaxFps = "_originalMaxFps";

		public static readonly StringName _target = "_target";

		public static readonly StringName _originalTowerDefenseManager = "_originalTowerDefenseManager";

		public static readonly StringName _cadenceFixtureManager = "_cadenceFixtureManager";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const string ResultMarker = "HIT_FLASH_DAMAGE_TINT_RESULT";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private static readonly Color BackgroundColor = new Color(0.015f, 0.015f, 0.02f);

	private static readonly Color BuffTint = new Color(0.2f, 0.35f, 1f);

	private AdobeAnimateRenderBackend _originalBackend;

	private bool _originalRasterCompositeEnabled;

	private int _originalMaxFps;

	private TowerDefenseCharacter _target;

	private TowerDefenseManager _originalTowerDefenseManager;

	private TowerDefenseManager _cadenceFixtureManager;

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
			_originalMaxFps = Engine.MaxFps;
			Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
			AdobeAnimateRenderManager.RasterCompositeEnabled = false;
			Engine.MaxFps = Math.Max(120, Engine.PhysicsTicksPerSecond * 2);
			ColorRect node = new ColorRect
			{
				Color = BackgroundColor,
				Position = Vector2.Zero,
				Size = new Vector2(1080f, 600f),
				MouseFilter = Control.MouseFilterEnum.Ignore,
				ZIndex = -4096
			};
			AddChild(node, forceReadableName: false, InternalMode.Disabled);
			PackedScene packedScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
			if (!GodotObject.IsInstanceValid(packedScene))
			{
				throw new InvalidOperationException("Normal zombie scene could not be loaded.");
			}
			_target = packedScene.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(_target))
			{
				throw new InvalidOperationException("Normal zombie could not be instantiated.");
			}
			_target.inGame = false;
			_target.editorPreviewMode = true;
			_target.Visible = true;
			_target.Position = new Vector2(540f, 330f);
			_target.Scale = Vector2.One * 1.5f;
			AddChild(_target, forceReadableName: false, InternalMode.Disabled);
			_target.sprite.SetAnimation("Walk1");
			_target.sprite.SetVerticalClip(enabled: false, -10000f, 10000f);
			await WaitProcessFrames(24);
			if (!GodotObject.IsInstanceValid(_target.sprite) || _target.hitFlashComponent == null || _target.hitFlashComponent.IsReleased)
			{
				throw new InvalidOperationException("Normal zombie hit-flash runtime is unavailable.");
			}
			_target.sprite.timeScale = 0.0;
			_target.sprite.pause = false;
			InstallHighLoadCadenceFixture();
			await WaitForHighLoadCadence();
			AdobeAnimateRuntimeManager.NotifyDisplayRenderSubmission(_target.sprite);
			using Image untintedFrame = await CaptureNextRenderTransaction();
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			long transactionsBeforeBuff = AdobeAnimateRenderManager.RenderTransactionCountForTests;
			TowerDefenseCharacterBuffIceSpeedDown iceSpeedDown = new TowerDefenseCharacterBuffIceSpeedDown
			{
				time = 10.0
			};
			_target.buff.AddBuff(iceSpeedDown);
			Ensure(Mathf.IsZeroApprox((float)iceSpeedDown.currentTime), $"Adding a Buff must not advance gameplay time; currentTime={iceSpeedDown.currentTime:F6}.");
			Ensure(ColorDistance(_target.sprite.meshColor, BuffTint) <= 0.02f, $"Production ice Buff tint was not applied synchronously; actual={_target.sprite.meshColor}.");
			using Image baseline = await CapturePendingDraw();
			long num = AdobeAnimateRenderManager.RenderTransactionCountForTests - transactionsBeforeBuff;
			float num2 = MeasureAverageRgbDifference(untintedFrame, baseline);
			AdobeAnimateCrowdRenderStateResult value = _target.sprite.TryBuildCrowdRenderState(out var state);
			Color value2 = state?.GpuGraphRootOwnerState?.Modulate ?? Colors.Transparent;
			AdobeAnimateCrowdAggregateStats aggregateRenderStats = AdobeAnimateRenderManager.GetAggregateRenderStats();
			GD.Print($"{"HIT_FLASH_DAMAGE_TINT_RESULT"} stage=buff-first-frame transactions={num} pixelDifference={num2:F4} stateResult={value} submittedModulate={value2} crowdRoots={aggregateRenderStats.CrowdRoots} fallbackRoots={aggregateRenderStats.FallbackRoots}");
			Ensure(num >= 1, $"The Buff tint missed the first post-physics render transaction; transactions={num}.");
			Ensure(num2 >= 0.08f, $"The first post-physics frame still showed the untinted Crowd state; difference={num2:F4}.");
			GD.Print($"{"HIT_FLASH_DAMAGE_TINT_RESULT"} stage=buff-immediate color={_target.sprite.meshColor} currentTime={iceSpeedDown.currentTime:F6} transactions={num} pixelDifference={num2:F4} visualTicks={AdobeAnimateRuntimeManager.Instance.CurrentVisualTicksPerSecond:F1} battleCharacters={AdobeAnimateRuntimeManager.Instance.CurrentBattleCharacterCount}");
			Vector4 baselineMetrics = MeasureAgainstBaselineMask(baseline, baseline);
			AdobeAnimateCrowdAggregateStats aggregateRenderStats2 = AdobeAnimateRenderManager.GetAggregateRenderStats();
			GD.Print($"{"HIT_FLASH_DAMAGE_TINT_RESULT"} stage=baseline targetVisible={_target.Visible} targetVisibleInTree={_target.IsVisibleInTree()} spriteVisible={_target.sprite.Visible} spriteVisibleInTree={_target.sprite.IsVisibleInTree()} crowdRoots={aggregateRenderStats2.CrowdRoots} fallbackRoots={aggregateRenderStats2.FallbackRoots}");
			Ensure(baselineMetrics.X >= 300f, $"Tinted zombie render was blank; foreground={baselineMetrics.X:F0}.");
			_target.White(0.5, 0.0, 0.2);
			float firstStartStrength = _target.hitFlashComponent.CurrentWhiteStrength;
			AdobeAnimateCrowdRenderStateResult value3 = _target.sprite.TryBuildCrowdRenderState(out var state2);
			AdobeAnimateGpuHitFlashState valueOrDefault = (state2?.GpuGraphRootOwnerState?.GpuWhiteFlash).GetValueOrDefault();
			GD.Print($"{"HIT_FLASH_DAMAGE_TINT_RESULT"} stage=white-state result={value3} mode={state2?.Mode} enabled={valueOrDefault.Enabled} strength={valueOrDefault.Strength:F4} duration={valueOrDefault.Duration:F4}");
			Ensure(_target.hitFlashComponent.IsGpuFlashEnvelopeActive, "First white flash did not use the GPU envelope.");
			Ensure(firstStartStrength >= 0.45f, $"First white flash did not start at full strength; strength={firstStartStrength:F4}.");
			using Image firstFlash = await CaptureFrame();
			Vector4 firstMetrics = MeasureAgainstBaselineMask(baseline, firstFlash);
			Ensure(firstMetrics.Y >= baselineMetrics.Y + 0.12f, $"White flash did not overcome blue Buff tint; baselineRed={baselineMetrics.Y:F4} flashRed={firstMetrics.Y:F4}.");
			Ensure(firstMetrics.W <= baselineMetrics.W * 0.82f, $"White flash remained Buff-colored; baselineRange={baselineMetrics.W:F4} flashRange={firstMetrics.W:F4}.");
			float beforeRestartStrength = firstStartStrength;
			for (int frame = 0; frame < 120; frame++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				beforeRestartStrength = _target.hitFlashComponent.CurrentWhiteStrength;
				if (beforeRestartStrength > 0f && beforeRestartStrength <= 0.15f)
				{
					break;
				}
			}
			Ensure(beforeRestartStrength > 0f && beforeRestartStrength <= 0.15f, $"First flash did not reach the restart window; strength={beforeRestartStrength:F4}.");
			_target.White(0.5, 0.0, 0.2);
			float restartedStrength = _target.hitFlashComponent.CurrentWhiteStrength;
			Ensure(_target.hitFlashComponent.IsGpuFlashEnvelopeActive, "Second white flash did not stay on the GPU envelope.");
			Ensure(restartedStrength >= 0.45f && restartedStrength >= beforeRestartStrength + 0.25f, $"Second hit did not reset the white flash; before={beforeRestartStrength:F4} after={restartedStrength:F4}.");
			using Image sample = await CaptureFrame();
			Vector4 vector = MeasureAgainstBaselineMask(baseline, sample);
			Ensure(vector.Y >= baselineMetrics.Y + 0.12f, $"Restarted flash was hidden by Buff tint; baselineRed={baselineMetrics.Y:F4} restartRed={vector.Y:F4}.");
			_target.buff.DeleteBuff("IceSpeedDown");
			Ensure(!_target.buff.BuffHas("IceSpeedDown"), "Deleting the ice Buff must remove it synchronously.");
			Ensure(ColorDistance(_target.sprite.meshColor, Colors.White) <= 0.02f, $"Deleting the ice Buff did not restore the base color synchronously; actual={_target.sprite.meshColor}.");
			GD.Print($"{"HIT_FLASH_DAMAGE_TINT_RESULT"} stage=buff-remove-immediate color={_target.sprite.meshColor}");
			GD.Print($"{"HIT_FLASH_DAMAGE_TINT_RESULT"} passed=True firstStart={firstStartStrength:F4} beforeRestart={beforeRestartStrength:F4} restarted={restartedStrength:F4} baselineRed={baselineMetrics.Y:F4} firstRed={firstMetrics.Y:F4} restartRed={vector.Y:F4} baselineRange={baselineMetrics.W:F4} firstRange={firstMetrics.W:F4} renderer={RenderingServer.GetCurrentRenderingMethod()}");
			exitCode = 0;
		}
		catch (Exception value4)
		{
			GD.PrintErr($"{"HIT_FLASH_DAMAGE_TINT_RESULT"} passed=False exception={value4}");
		}
		finally
		{
			RestoreHighLoadCadenceFixture();
			if (Global.Instance != null)
			{
				Global.Instance.adobeAnimateRenderBackend = _originalBackend;
			}
			AdobeAnimateRenderManager.RasterCompositeEnabled = _originalRasterCompositeEnabled;
			Engine.MaxFps = _originalMaxFps;
		}
		GetTree().Quit(exitCode);
	}

	private void InstallHighLoadCadenceFixture()
	{
		_originalTowerDefenseManager = TowerDefenseManager.Instance;
		_cadenceFixtureManager = new TowerDefenseManager
		{
			characterRegistry = new TowerDefenseBattleCharacterRegistry()
		};
		System.Reflection.PropertyInfo property = typeof(TowerDefenseManager).GetProperty("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		FieldInfo field = typeof(TowerDefenseBattleCharacterRegistry).GetField("_activeCharacters", BindingFlags.Instance | BindingFlags.NonPublic);
		if (property == null || !(field?.GetValue(_cadenceFixtureManager.characterRegistry) is List<TowerDefenseCharacter> list))
		{
			throw new InvalidOperationException("High-load cadence fixture reflection contract is unavailable.");
		}
		for (int i = 0; i < 1000; i++)
		{
			list.Add(null);
		}
		property.SetValue(null, _cadenceFixtureManager);
	}

	private void RestoreHighLoadCadenceFixture()
	{
		typeof(TowerDefenseManager).GetProperty("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(null, _originalTowerDefenseManager);
		if (GodotObject.IsInstanceValid(_cadenceFixtureManager?.characterRegistry))
		{
			_cadenceFixtureManager.characterRegistry.Free();
		}
		if (GodotObject.IsInstanceValid(_cadenceFixtureManager))
		{
			_cadenceFixtureManager.Free();
		}
		_cadenceFixtureManager = null;
		_originalTowerDefenseManager = null;
	}

	private async Task WaitForHighLoadCadence()
	{
		for (int frame = 0; frame < 120; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			AdobeAnimateRuntimeManager instance = AdobeAnimateRuntimeManager.Instance;
			if (GodotObject.IsInstanceValid(instance) && instance.CurrentBattleCharacterCount == 1000 && Mathf.IsEqualApprox((float)instance.CurrentVisualTicksPerSecond, 30f))
			{
				return;
			}
		}
		throw new InvalidOperationException("Adobe Animate runtime manager did not enter the 30 Hz high-load cadence.");
	}

	private async Task<Image> CaptureNextRenderTransaction()
	{
		long initialTransactions = AdobeAnimateRenderManager.RenderTransactionCountForTests;
		for (int frame = 0; frame < 120; frame++)
		{
			await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
			if (AdobeAnimateRenderManager.RenderTransactionCountForTests > initialTransactions)
			{
				return GetViewport().GetTexture().GetImage();
			}
		}
		throw new InvalidOperationException("No Crowd render transaction completed while aligning the cadence fixture.");
	}

	private async Task<Image> CapturePendingDraw()
	{
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		return GetViewport().GetTexture().GetImage();
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

	private static Vector4 MeasureAgainstBaselineMask(Image baseline, Image sample)
	{
		int num = 0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		int num5 = Math.Min(baseline.GetWidth(), sample.GetWidth());
		int num6 = Math.Min(baseline.GetHeight(), sample.GetHeight());
		for (int i = 0; i < num6; i += 2)
		{
			for (int j = 0; j < num5; j += 2)
			{
				if (!(ColorDistance(baseline.GetPixel(j, i), BackgroundColor) <= 0.08f))
				{
					Color pixel = sample.GetPixel(j, i);
					num2 += (double)pixel.R;
					num3 += (double)(pixel.R + pixel.G + pixel.B) / 3.0;
					num4 += (double)(Math.Max(pixel.R, Math.Max(pixel.G, pixel.B)) - Math.Min(pixel.R, Math.Min(pixel.G, pixel.B)));
					num++;
				}
			}
		}
		if (num == 0)
		{
			return Vector4.Zero;
		}
		return new Vector4(num, (float)(num2 / (double)num), (float)(num3 / (double)num), (float)(num4 / (double)num));
	}

	private static float MeasureAverageRgbDifference(Image baseline, Image sample)
	{
		double num = 0.0;
		int num2 = 0;
		int num3 = Math.Min(baseline.GetWidth(), sample.GetWidth());
		int num4 = Math.Min(baseline.GetHeight(), sample.GetHeight());
		for (int i = 0; i < num4; i += 2)
		{
			for (int j = 0; j < num3; j += 2)
			{
				Color pixel = baseline.GetPixel(j, i);
				if (!(ColorDistance(pixel, BackgroundColor) <= 0.08f))
				{
					Color pixel2 = sample.GetPixel(j, i);
					num += (double)ColorDistance(pixel, pixel2) / 3.0;
					num2++;
				}
			}
		}
		if (num2 <= 0)
		{
			return 0f;
		}
		return (float)(num / (double)num2);
	}

	private static float ColorDistance(Color left, Color right)
	{
		return Mathf.Abs(left.R - right.R) + Mathf.Abs(left.G - right.G) + Mathf.Abs(left.B - right.B);
	}

	private static void Ensure(bool condition, string message)
	{
		if (!condition)
		{
			throw new InvalidOperationException(message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(7)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.InstallHighLoadCadenceFixture, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RestoreHighLoadCadenceFixture, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.MeasureAgainstBaselineMask, new Godot.Bridge.PropertyInfo(Variant.Type.Vector4, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "baseline", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sample", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.MeasureAverageRgbDifference, new Godot.Bridge.PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "baseline", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sample", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ColorDistance, new Godot.Bridge.PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Color, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Color, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Ensure, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.InstallHighLoadCadenceFixture && args.Count == 0)
		{
			InstallHighLoadCadenceFixture();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreHighLoadCadenceFixture && args.Count == 0)
		{
			RestoreHighLoadCadenceFixture();
			ret = default;
			return true;
		}
		if (method == MethodName.MeasureAgainstBaselineMask && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector4>(MeasureAgainstBaselineMask(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
			return true;
		}
		if (method == MethodName.MeasureAverageRgbDifference && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(MeasureAverageRgbDifference(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
			return true;
		}
		if (method == MethodName.ColorDistance && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(ColorDistance(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.Ensure && args.Count == 2)
		{
			Ensure(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.MeasureAgainstBaselineMask && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector4>(MeasureAgainstBaselineMask(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
			return true;
		}
		if (method == MethodName.MeasureAverageRgbDifference && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(MeasureAverageRgbDifference(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
			return true;
		}
		if (method == MethodName.ColorDistance && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(ColorDistance(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.Ensure && args.Count == 2)
		{
			Ensure(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
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
		if (method == MethodName.InstallHighLoadCadenceFixture)
		{
			return true;
		}
		if (method == MethodName.RestoreHighLoadCadenceFixture)
		{
			return true;
		}
		if (method == MethodName.MeasureAgainstBaselineMask)
		{
			return true;
		}
		if (method == MethodName.MeasureAverageRgbDifference)
		{
			return true;
		}
		if (method == MethodName.ColorDistance)
		{
			return true;
		}
		if (method == MethodName.Ensure)
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
		if (name == PropertyName._originalMaxFps)
		{
			_originalMaxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._target)
		{
			_target = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._originalTowerDefenseManager)
		{
			_originalTowerDefenseManager = VariantUtils.ConvertTo<TowerDefenseManager>(in value);
			return true;
		}
		if (name == PropertyName._cadenceFixtureManager)
		{
			_cadenceFixtureManager = VariantUtils.ConvertTo<TowerDefenseManager>(in value);
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
		if (name == PropertyName._originalMaxFps)
		{
			value = VariantUtils.CreateFrom(in _originalMaxFps);
			return true;
		}
		if (name == PropertyName._target)
		{
			value = VariantUtils.CreateFrom(in _target);
			return true;
		}
		if (name == PropertyName._originalTowerDefenseManager)
		{
			value = VariantUtils.CreateFrom(in _originalTowerDefenseManager);
			return true;
		}
		if (name == PropertyName._cadenceFixtureManager)
		{
			value = VariantUtils.CreateFrom(in _cadenceFixtureManager);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._originalBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._originalRasterCompositeEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._originalMaxFps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._target, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._originalTowerDefenseManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._cadenceFixtureManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._originalBackend, Variant.From(in _originalBackend));
		info.AddProperty(PropertyName._originalRasterCompositeEnabled, Variant.From(in _originalRasterCompositeEnabled));
		info.AddProperty(PropertyName._originalMaxFps, Variant.From(in _originalMaxFps));
		info.AddProperty(PropertyName._target, Variant.From(in _target));
		info.AddProperty(PropertyName._originalTowerDefenseManager, Variant.From(in _originalTowerDefenseManager));
		info.AddProperty(PropertyName._cadenceFixtureManager, Variant.From(in _cadenceFixtureManager));
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
		if (info.TryGetProperty(PropertyName._originalMaxFps, out var value3))
		{
			_originalMaxFps = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._target, out var value4))
		{
			_target = value4.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._originalTowerDefenseManager, out var value5))
		{
			_originalTowerDefenseManager = value5.As<TowerDefenseManager>();
		}
		if (info.TryGetProperty(PropertyName._cadenceFixtureManager, out var value6))
		{
			_cadenceFixtureManager = value6.As<TowerDefenseManager>();
		}
	}
}
