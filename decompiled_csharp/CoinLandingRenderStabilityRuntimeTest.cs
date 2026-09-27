using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/CoinLandingRenderStabilityRuntimeTest.cs")]
public class CoinLandingRenderStabilityRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindAnimationSprite = "FindAnimationSprite";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _originalMaxFps = "_originalMaxFps";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "COIN_LANDING_RENDER_STABILITY_RESULT";

	private static readonly Color BackgroundColor = new Color(0.015f, 0.015f, 0.015f);

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
			Node node2 = new Node
			{
				Name = "GoldMagnetTestMarker"
			};
			node2.AddToGroup("GoldMagnet");
			AddChild(node2, forceReadableName: false, InternalMode.Disabled);
			TowerDefenseCoinBase coin = GD.Load<PackedScene>("res://Prefab/TowerDefense/Coin/TowerDefenseCoinGold.tscn")?.Instantiate<TowerDefenseCoinBase>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(coin))
			{
				throw new InvalidOperationException("Gold coin scene could not be instantiated.");
			}
			coin.Position = new Vector2(540f, 150f);
			AddChild(coin, forceReadableName: false, InternalMode.Disabled);
			await WaitProcessFrames(20);
			AdobeAnimateSprite coinSprite = FindAnimationSprite(coin);
			if (!GodotObject.IsInstanceValid(coinSprite))
			{
				throw new InvalidOperationException("Gold coin animation sprite is unavailable.");
			}
			var (firstRange, firstBlankFrames) = await DropAndMeasure(coin, coinSprite, 180f);
			coin.Recycle();
			(double, int) tuple2 = await DropAndMeasure(coin, coinSprite, 180f);
			double item = tuple2.Item1;
			int item2 = tuple2.Item2;
			bool flag = firstBlankFrames == 0 && item2 == 0 && firstRange <= 0.75 && item <= 0.75;
			GD.Print($"{"COIN_LANDING_RENDER_STABILITY_RESULT"} passed={flag} firstBlank={firstBlankFrames} reusedBlank={item2} firstRange={firstRange:F4} reusedRange={item:F4} renderer={RenderingServer.GetCurrentRenderingMethod()}");
			exitCode = ((!flag) ? 2 : 0);
		}
		catch (Exception value)
		{
			GD.PrintErr($"{"COIN_LANDING_RENDER_STABILITY_RESULT"} exception={value}");
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

	private async Task<(double Range, int BlankFrames)> DropAndMeasure(TowerDefenseCoinBase coin, AdobeAnimateSprite coinSprite, float landingHeight)
	{
		coin.Refresh();
		coinSprite.SetAnimation("Idle");
		coinSprite.timeScale = 0.0;
		coin.Init(landingHeight, new Vector2(0f, -240f), 980.0);
		int remainingPhysicsFrames = 180;
		while (!coin.over && remainingPhysicsFrames-- > 0)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		if (!coin.over)
		{
			throw new InvalidOperationException("Gold coin did not reach its landing state.");
		}
		await WaitPhysicsFrames(3);
		if (coin.moveComponent.HasActiveMovement)
		{
			throw new InvalidOperationException("Gold coin movement remained active after landing.");
		}
		if (!Mathf.IsEqualApprox(coin.spriteNode.Position.Y, landingHeight))
		{
			throw new InvalidOperationException($"Gold coin landed at {coin.spriteNode.Position.Y:F4}, expected {landingHeight:F4}.");
		}
		double minCentroidY = 1.7976931348623157E+308;
		double maxCentroidY = -1.7976931348623157E+308;
		int blankFrames = 0;
		for (int frame = 0; frame < 60; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
			using Image image = GetViewport().GetTexture().GetImage();
			if (!TryMeasureCentroidY(image, 470, 610, 250, 430, out var centroidY))
			{
				blankFrames++;
				continue;
			}
			minCentroidY = Math.Min(minCentroidY, centroidY);
			maxCentroidY = Math.Max(maxCentroidY, centroidY);
		}
		return (Range: (maxCentroidY >= minCentroidY) ? (maxCentroidY - minCentroidY) : (1.0 / 0.0), BlankFrames: blankFrames);
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task WaitPhysicsFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private static AdobeAnimateSprite FindAnimationSprite(Node root)
	{
		if (root is AdobeAnimateSprite result)
		{
			return result;
		}
		foreach (Node child in root.GetChildren())
		{
			AdobeAnimateSprite adobeAnimateSprite = FindAnimationSprite(child);
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				return adobeAnimateSprite;
			}
		}
		return null;
	}

	private static bool TryMeasureCentroidY(Image image, int x0, int x1, int y0, int y1, out double centroidY)
	{
		double num = 0.0;
		double num2 = 0.0;
		int num3 = Math.Min(x1, image.GetWidth());
		int num4 = Math.Min(y1, image.GetHeight());
		for (int i = Math.Max(0, y0); i < num4; i += 2)
		{
			for (int j = Math.Max(0, x0); j < num3; j += 2)
			{
				Color pixel = image.GetPixel(j, i);
				double num5 = Math.Abs(pixel.R - BackgroundColor.R) + Math.Abs(pixel.G - BackgroundColor.G) + Math.Abs(pixel.B - BackgroundColor.B);
				if (!(num5 <= 0.08))
				{
					num += (double)i * num5;
					num2 += num5;
				}
			}
		}
		centroidY = ((num2 > 1.0) ? (num / num2) : 0.0);
		return num2 > 1.0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindAnimationSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.FindAnimationSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(FindAnimationSprite(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindAnimationSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(FindAnimationSprite(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.FindAnimationSprite)
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
