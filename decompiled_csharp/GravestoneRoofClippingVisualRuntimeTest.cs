using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/GravestoneRoofClippingVisualRuntimeTest.cs")]
public class GravestoneRoofClippingVisualRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CountPixels = "CountPixels";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _gravestone = "_gravestone";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private int _checks;

	private int _failures;

	private TowerDefenseGravestone _gravestone;

	public override async void _Ready()
	{
		int exitCode = 2;
		AdobeAnimateRenderBackend originalBackend = Global.Instance.adobeAnimateRenderBackend;
		int originalMaxFps = Engine.MaxFps;
		try
		{
			_ = 2;
			try
			{
				Engine.MaxFps = 120;
				AddChild(new ColorRect
				{
					Color = Colors.Black,
					Size = new Vector2(1080f, 600f),
					ZIndex = -4096,
					MouseFilter = Control.MouseFilterEnum.Ignore
				}, forceReadableName: false, InternalMode.Disabled);
				TowerDefenseMapConfig roof = GD.Load<TowerDefenseMapConfig>("res://Asset/Config/Map/Roof/Config/FrontlawnMapRoof.tres");
				AdobeAnimateRenderBackend[] array = new AdobeAnimateRenderBackend[2]
				{
					AdobeAnimateRenderBackend.GpuCrowd,
					AdobeAnimateRenderBackend.CpuPose
				};
				foreach (AdobeAnimateRenderBackend backend in array)
				{
					Global.Instance.adobeAnimateRenderBackend = backend;
					int[] array2 = new int[4] { 1, 3, 5, 6 };
					foreach (int num in array2)
					{
						TowerDefenseCellConfig towerDefenseCellConfig = null;
						foreach (TowerDefenseCellConfig item in roof.cellConfig)
						{
							if (num >= item.pos.X && num <= item.pos.Z)
							{
								towerDefenseCellConfig = item;
								break;
							}
						}
						using TowerDefenseCellInstance cell = new TowerDefenseCellInstance
						{
							groundHeightCurve = towerDefenseCellConfig?.groundHeightCurve
						};
						await VerifyRise(backend, $"roof-column-{num}", cell, (float)cell.GetGroundHeight(), inWater: false, 1f);
					}
					using TowerDefenseCellInstance scaledCell = new TowerDefenseCellInstance
					{
						groundHeightCurve = roof.cellConfig[0].groundHeightCurve
					};
					await VerifyRise(backend, "roof-scaled", scaledCell, (float)scaledCell.GetGroundHeight(), inWater: false, 1.25f);
					await VerifyRise(backend, "water", null, -25f, inWater: true, 1f);
				}
				exitCode = ((_failures != 0) ? 2 : 0);
				GD.Print($"GRAVESTONE_ROOF_CLIPPING_VISUAL_RESULT passed={_failures == 0} checks={_checks} failures={_failures} renderer={RenderingServer.GetCurrentRenderingMethod()}");
			}
			catch (Exception value)
			{
				GD.PrintErr($"GRAVESTONE_ROOF_CLIPPING_VISUAL_RESULT passed=False checks={_checks} exception={value}");
			}
		}
		finally
		{
			Global.Instance.adobeAnimateRenderBackend = originalBackend;
			Engine.MaxFps = originalMaxFps;
			if (GodotObject.IsInstanceValid(_gravestone))
			{
				_gravestone.QueueFree();
			}
			await WaitFrames(3);
		}
		GetTree().Quit(exitCode);
	}

	private async Task VerifyRise(AdobeAnimateRenderBackend backend, string scenario, TowerDefenseCellInstance cell, float groundHeight, bool inWater, float scale)
	{
		_gravestone = GD.Load<PackedScene>("res://Asset/Anime/Character/GraveStone/Default/Scene/TowerDefenseGraveStoneDefault.tscn").Instantiate<TowerDefenseGravestone>(PackedScene.GenEditState.Disabled);
		_gravestone.inGame = false;
		_gravestone.rise = false;
		_gravestone.Position = new Vector2(540f, 280f);
		_gravestone.Scale = Vector2.One * scale;
		_gravestone.cell = cell;
		_gravestone.groundHeight = groundHeight;
		AddChild(_gravestone, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(8);
		_gravestone.groundHeightComponent?.SetAlive(alive: false);
		_gravestone.groundHeight = groundHeight;
		_gravestone.inWater = inWater;
		_gravestone.sprite.SetAnimation("Idle1");
		_gravestone.sprite.pause = true;
		_gravestone.sprite.timeScale = 0.0;
		_gravestone.shadowComponent.SetShadowVisible(visible: false);
		await WaitFrames(4);
		using Image reference = await CaptureImage();
		int referencePixels = CountPixels(reference);
		float waterSurfaceY = _gravestone.ToGlobal(_gravestone.transformPoint.Position).Y;
		Check(referencePixels > 2000, $"{backend}/{scenario} 静止墓碑未完整绘出: pixels={referencePixels}");
		_gravestone.Rise(0.65, 0.0, createDirt: false, changeState: false, 150.0, emitRiseOver: false);
		await WaitForSurface(groundHeight);
		using Image nearSurface = await CaptureImage();
		int nearSurfacePixels = CountPixels(nearSurface);
		VerticalClipState clip = _gravestone.sprite.GetVerticalClipState();
		Check(_gravestone.isRise && clip.Enabled, $"{backend}/{scenario} 没有采集到出土期间的裁剪帧。");
		if (inWater)
		{
			Check(Math.Abs(clip.DownY - waterSurfaceY) < 1f, $"{backend}/{scenario} 水面裁剪线发生变化: actual={clip.DownY}, expected={waterSurfaceY}");
			Check((float)nearSurfacePixels < (float)referencePixels * 0.9f && (float)nearSurfacePixels > (float)referencePixels * 0.25f, $"{backend}/{scenario} 水下部分未按水面裁剪: {nearSurfacePixels}/{referencePixels}");
		}
		else
		{
			bool flag = (float)nearSurfacePixels >= (float)referencePixels * 0.97f;
			Check(flag, $"{backend}/{scenario} 接近地表时墓碑仍然缺块: {nearSurfacePixels}/{referencePixels}, clip={clip.DownY}, height={_gravestone.groundHeight:F2}");
			if (!flag)
			{
				reference.SavePng(ProjectSettings.GlobalizePath($"user://{backend}-{scenario}-reference.png"));
				nearSurface.SavePng(ProjectSettings.GlobalizePath($"user://{backend}-{scenario}-clipped.png"));
			}
		}
		for (int frame = 0; frame < 120; frame++)
		{
			if (!_gravestone.isRise)
			{
				break;
			}
			await WaitFrames(1);
		}
		await WaitFrames(4);
		using Image settled = await CaptureImage();
		int num = CountPixels(settled);
		Check(!_gravestone.isRise && Math.Abs(_gravestone.groundHeight - (double)groundHeight) < 0.009999999776482582, $"{backend}/{scenario} 出土后没有恢复目标地面高度。");
		if (!inWater)
		{
			Check(!_gravestone.sprite.GetVerticalClipState().Enabled && (float)num >= (float)referencePixels * 0.99f, $"{backend}/{scenario} 出土后残留裁剪: {num}/{referencePixels}");
		}
		GD.Print($"GRAVESTONE_ROOF_CLIPPING_PIXELS backend={backend} scenario={scenario} height={groundHeight} scale={scale} full={referencePixels} nearSurface={nearSurfacePixels} settled={num} clip={clip.DownY}");
		_gravestone.QueueFree();
		await WaitFrames(3);
		_gravestone = null;
	}

	private async Task WaitForSurface(float groundHeight)
	{
		for (int frame = 0; frame < 180; frame++)
		{
			await WaitFrames(1);
			if (_gravestone.isRise && _gravestone.groundHeight >= (double)(groundHeight - 2f))
			{
				return;
			}
		}
		throw new TimeoutException("墓碑未进入接近地表的出土阶段。");
	}

	private static int CountPixels(Image image)
	{
		int num = 0;
		for (int i = 0; i < image.GetHeight(); i++)
		{
			for (int j = 0; j < image.GetWidth(); j++)
			{
				Color pixel = image.GetPixel(j, i);
				if (Math.Max(pixel.R, Math.Max(pixel.G, pixel.B)) > 0.08f)
				{
					num++;
				}
			}
		}
		return num;
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private async Task<Image> CaptureImage()
	{
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		return GetViewport().GetTexture().GetImage();
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PrintErr("GRAVESTONE_ROOF_CLIPPING_FAILURE " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountPixels, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CountPixels && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountPixels(VariantUtils.ConvertTo<Image>(in args[0])));
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
		if (method == MethodName.CountPixels && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountPixels(VariantUtils.ConvertTo<Image>(in args[0])));
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
		if (method == MethodName.CountPixels)
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
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gravestone)
		{
			_gravestone = VariantUtils.ConvertTo<TowerDefenseGravestone>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		if (name == PropertyName._gravestone)
		{
			value = VariantUtils.CreateFrom(in _gravestone);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gravestone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._gravestone, Variant.From(in _gravestone));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gravestone, out var value3))
		{
			_gravestone = value3.As<TowerDefenseGravestone>();
		}
	}
}
