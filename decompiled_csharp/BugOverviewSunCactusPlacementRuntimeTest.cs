using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewSunCactusPlacementRuntimeTest.cs")]
public class BugOverviewSunCactusPlacementRuntimeTest : Node
{
	private readonly record struct VisualMeasure(int PixelCount, Rect2I Bounds, float FootCenterX, float BottomY);

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName IsForeground = "IsForeground";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string SunCactusScenePath = "res://Asset/Anime/Character/Plant/Gold/SunCactus/Scene/TowerDefensePlantSunCactus.tscn";

	private const string CactusScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Cactus/Scene/TowerDefensePlantCactus.tscn";

	private static readonly Vector2 RenderAnchor = new Vector2(256f, 128f);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		Node2D sunHost = null;
		Node2D cactusHost = null;
		PackedScene sunPacked = null;
		PackedScene cactusPacked = null;
		try
		{
			_ = 6;
			try
			{
				sunPacked = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Gold/SunCactus/Scene/TowerDefensePlantSunCactus.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
				cactusPacked = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/Cactus/Scene/TowerDefensePlantCactus.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
				Check(GodotObject.IsInstanceValid(sunPacked) && GodotObject.IsInstanceValid(cactusPacked), "Real SunCactus and Cactus character scenes must load.");
				if (!GodotObject.IsInstanceValid(sunPacked) || !GodotObject.IsInstanceValid(cactusPacked))
				{
					goto end_IL_008a;
				}
				sunHost = MountProductionVisual(sunPacked, "SunCactus", RenderAnchor, out var sunSprite, out var sunVisualPosition);
				Check(GodotObject.IsInstanceValid(sunSprite), "The production SunCactus scene must expose its real Adobe animation node.");
				if (!GodotObject.IsInstanceValid(sunSprite))
				{
					goto end_IL_008a;
				}
				for (int frame = 0; frame < 3; frame++)
				{
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				}
				sunSprite.SetAnimation("UpIdle");
				sunSprite.ResetAnimation();
				sunSprite.SetFrozenPreview(frozen: true);
				for (int frame = 0; frame < 6; frame++)
				{
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				}
				await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
				Image image = GetViewport().GetTexture().GetImage();
				VisualMeasure sun = MeasureVisual(image, image.GetPixel(0, 0), 0, 512, RenderAnchor);
				Check(sunSprite.clip == "UpIdle", "SunCactus must remain in the reported extended posture during capture; got " + sunSprite.clip + ".");
				sunHost.QueueFree();
				sunHost = null;
				for (int frame = 0; frame < 4; frame++)
				{
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				}
				cactusHost = MountProductionVisual(cactusPacked, "Cactus", RenderAnchor, out var cactusSprite, out var visualPosition);
				Check(GodotObject.IsInstanceValid(cactusSprite), "The production Cactus scene must expose its real Adobe animation node.");
				if (!GodotObject.IsInstanceValid(cactusSprite))
				{
					goto end_IL_008a;
				}
				Check(visualPosition.IsEqualApprox(Vector2.Zero), $"The Cactus family control must retain its neutral visual anchor; got {visualPosition}.");
				for (int frame = 0; frame < 3; frame++)
				{
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				}
				cactusSprite.SetAnimation("UpIdle");
				cactusSprite.ResetAnimation();
				cactusSprite.SetFrozenPreview(frozen: true);
				for (int frame = 0; frame < 6; frame++)
				{
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				}
				await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
				Image image2 = GetViewport().GetTexture().GetImage();
				VisualMeasure visualMeasure = MeasureVisual(image2, image2.GetPixel(0, 0), 0, 512, RenderAnchor);
				Check(cactusSprite.clip == "UpIdle", "Cactus control must remain in its extended comparison posture; got " + cactusSprite.clip + ".");
				Check(sun.PixelCount > 300 && visualMeasure.PixelCount > 200, $"Both real cactus visuals must render enough pixels; sun={sun.PixelCount}, cactus={visualMeasure.PixelCount}.");
				float value = sun.FootCenterX - visualMeasure.FootCenterX;
				float value2 = sun.BottomY - visualMeasure.BottomY;
				GD.Print($"SUN_CACTUS_PLACEMENT_METRICS sunPosition={sunVisualPosition} sunBounds={sun.Bounds} sunFootX={sun.FootCenterX:F2} sunBottomY={sun.BottomY:F2} cactusBounds={visualMeasure.Bounds} cactusFootX={visualMeasure.FootCenterX:F2} cactusBottomY={visualMeasure.BottomY:F2} footDeltaX={value:F2} bottomDeltaY={value2:F2}");
				Check(Math.Abs(value) <= 4f, $"SunCactus must be horizontally centered on the Cactus family planting anchor; delta={value:F2}.");
				Check(Math.Abs(value2) <= 4f, $"SunCactus must share the Cactus family ground contact; delta={value2:F2}.");
				goto end_IL_0063;
				end_IL_008a:;
			}
			catch (Exception value3)
			{
				_failures++;
				GD.PushError($"[SunCactusPlacement] Unexpected exception: {value3}");
				goto end_IL_0063;
			}
			return;
			end_IL_0063:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(sunHost))
			{
				sunHost.QueueFree();
			}
			if (GodotObject.IsInstanceValid(cactusHost))
			{
				cactusHost.QueueFree();
			}
			for (int frame = 0; frame < 3; frame++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}
			await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			sunPacked?.Dispose();
			cactusPacked?.Dispose();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag = _failures == 0;
		GD.Print($"BUG_OVERVIEW_I118_SUN_CACTUS_PLACEMENT_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private Node2D MountProductionVisual(PackedScene packed, string visualName, Vector2 anchor, out AdobeAnimateSprite sprite, out Vector2 visualPosition)
	{
		Node2D node2D = packed.Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
		Node2D node = node2D.GetNode<Node2D>("SpriteGroup/TransformPoint");
		sprite = node.GetNode<AdobeAnimateSprite>(visualName);
		visualPosition = node.Position + sprite.Position;
		node2D.GetNode<CanvasItem>("ShadowSprite").Visible = false;
		node2D.Position = anchor;
		node2D.ProcessMode = ProcessModeEnum.Disabled;
		AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
		return node2D;
	}

	private static VisualMeasure MeasureVisual(Image image, Color background, int beginX, int endX, Vector2 anchor)
	{
		int num = 0;
		int num2 = endX;
		int num3 = image.GetHeight();
		int num4 = beginX - 1;
		int num5 = -1;
		for (int i = 0; i < image.GetHeight(); i++)
		{
			for (int j = beginX; j < endX; j++)
			{
				if (IsForeground(image.GetPixel(j, i), background))
				{
					num++;
					num2 = Math.Min(num2, j);
					num3 = Math.Min(num3, i);
					num4 = Math.Max(num4, j);
					num5 = Math.Max(num5, i);
				}
			}
		}
		if (num == 0)
		{
			return new VisualMeasure(0, default, 0f / 0f, 0f / 0f);
		}
		int num6 = num5 - Math.Max(4, (num5 - num3 + 1) / 5);
		double num7 = 0.0;
		int num8 = 0;
		for (int k = num6; k <= num5; k++)
		{
			for (int l = beginX; l < endX; l++)
			{
				if (IsForeground(image.GetPixel(l, k), background))
				{
					num7 += (double)l;
					num8++;
				}
			}
		}
		Rect2I bounds = new Rect2I(num2, num3, num4 - num2 + 1, num5 - num3 + 1);
		float footCenterX = ((num8 == 0) ? (0f / 0f) : ((float)(num7 / (double)num8 - (double)anchor.X)));
		float bottomY = (float)num5 - anchor.Y;
		return new VisualMeasure(num, bounds, footCenterX, bottomY);
	}

	private static bool IsForeground(Color pixel, Color background)
	{
		return Math.Abs(pixel.R - background.R) + Math.Abs(pixel.G - background.G) + Math.Abs(pixel.B - background.B) > 0.05f;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[SunCactusPlacement] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsForeground, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "pixel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "background", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IsForeground && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsForeground(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
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
		if (method == MethodName.IsForeground && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsForeground(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
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
		if (method == MethodName.IsForeground)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
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
	}
}
