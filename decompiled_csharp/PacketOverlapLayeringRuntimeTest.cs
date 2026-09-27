using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/PacketOverlapLayeringRuntimeTest.cs")]
public class PacketOverlapLayeringRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ConfigureCard = "ConfigureCard";

		public static readonly StringName HasFlatCardZIndex = "HasFlatCardZIndex";

		public static readonly StringName HasFlatPreviewZIndex = "HasFlatPreviewZIndex";

		public static readonly StringName CreateSolidTexture = "CreateSolidTexture";

		public static readonly StringName Check = "Check";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "PACKET_OVERLAP_LAYERING_RESULT";

	private const string PacketScenePath = "res://Prefab/TowerDefense/GUI/InGame/Packet/TowerDefenseInGamePacketShow.tscn";

	private const string PumpkinPacketPath = "res://Asset/Anime/Character/Plant/Chapter0/Pumpkin/Packet/PlantPumpkin.tres";

	private const string PumpkinSpritePath = "res://Asset/Anime/Character/Plant/Chapter0/Pumpkin/Pumpkin.tscn";

	private const string ScreenshotPath = "res://.codex-tmp/packet-overlap-layering.png";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		_ = 3;
		try
		{
			PackedScene packetScene = GD.Load<PackedScene>("res://Prefab/TowerDefense/GUI/InGame/Packet/TowerDefenseInGamePacketShow.tscn");
			Check(GodotObject.IsInstanceValid(packetScene), "无法加载正式卡牌场景。");
			if (!GodotObject.IsInstanceValid(packetScene))
			{
				Finish();
				return;
			}
			ColorRect colorRect = new ColorRect();
			colorRect.Name = "ViewportBackground";
			colorRect.Color = Colors.Black;
			colorRect.Position = Vector2.Zero;
			colorRect.Size = new Vector2(1080f, 600f);
			colorRect.MouseFilter = Control.MouseFilterEnum.Ignore;
			AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
			TowerDefenseInGamePacketShow firstCard = packetScene.Instantiate<TowerDefenseInGamePacketShow>(PackedScene.GenEditState.Disabled);
			firstCard.Name = "FirstCard";
			firstCard.Position = new Vector2(100f, 100f);
			firstCard.setPcLayout = true;
			AddChild(firstCard, forceReadableName: false, InternalMode.Disabled);
			TowerDefenseInGamePacketShow secondCard = packetScene.Instantiate<TowerDefenseInGamePacketShow>(PackedScene.GenEditState.Disabled);
			secondCard.Name = "SecondCard";
			secondCard.Position = new Vector2(125f, 100f);
			secondCard.setPcLayout = true;
			AddChild(secondCard, forceReadableName: false, InternalMode.Disabled);
			await WaitFrames(2);
			ConfigureCard(firstCard, Colors.Red, Colors.LimeGreen, patternOnRightHalf: false);
			ConfigureCard(secondCard, Colors.Blue, Colors.Yellow, patternOnRightHalf: true);
			Check(HasFlatCardZIndex(firstCard), "第一张卡牌仍把卡面控件拆到不同 Z 层。");
			Check(HasFlatCardZIndex(secondCard), "第二张卡牌仍把卡面控件拆到不同 Z 层。");
			TowerDefensePacketConfig pumpkinConfig = GD.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Plant/Chapter0/Pumpkin/Packet/PlantPumpkin.tres");
			PackedScene packedScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/Pumpkin/Pumpkin.tscn");
			bool flag = GodotObject.IsInstanceValid(pumpkinConfig) && GodotObject.IsInstanceValid(packedScene) && GodotObject.IsInstanceValid(ResourceManager.Instance);
			Check(flag, "无法加载真实复合南瓜卡牌资源。");
			if (flag)
			{
				ResourceManager.Instance.CHARCTAER_SPRITE[pumpkinConfig.saveKey] = packedScene;
				TowerDefenseInGamePacketShow pumpkinCard = packetScene.Instantiate<TowerDefenseInGamePacketShow>(PackedScene.GenEditState.Disabled);
				pumpkinCard.Name = "PumpkinCard";
				pumpkinCard.Position = new Vector2(400f, 100f);
				pumpkinCard.setPcLayout = true;
				AddChild(pumpkinCard, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(1);
				pumpkinCard.Init(pumpkinConfig);
				await WaitFrames(2);
				Check(GodotObject.IsInstanceValid(pumpkinCard.sprite) && HasFlatPreviewZIndex(pumpkinCard.sprite), "真实复合南瓜预览仍包含跨卡牌排序的 Z 层。");
			}
			await WaitFrames(4);
			using Image image = GetViewport().GetTexture().GetImage();
			Error error = image.SavePng(ProjectSettings.GlobalizePath("res://.codex-tmp/packet-overlap-layering.png"));
			Check(error == Error.Ok, $"像素证据保存失败：{error}。");
			Vector2 size = GetViewport().GetVisibleRect().Size;
			int x = Mathf.RoundToInt(112f * (float)image.GetWidth() / size.X);
			int y = Mathf.RoundToInt(100f * (float)image.GetHeight() / size.Y);
			Color pixel = image.GetPixel(x, y);
			bool condition = pixel.B > 0.75f && pixel.G < 0.25f && pixel.R < 0.25f;
			Check(condition, $"重叠区仍被前一卡牌图案覆盖，采样颜色={pixel}。");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[{"PacketOverlapLayeringRuntimeTest"}] 运行异常：{value}");
		}
		Finish();
	}

	private static void ConfigureCard(TowerDefenseInGamePacketShow card, Color backgroundColor, Color patternColor, bool patternOnRightHalf)
	{
		card.backgroundTexture.Visible = true;
		card.backgroundTexture.Texture = CreateSolidTexture(backgroundColor);
		card.previewClip.Visible = true;
		card.previewClip.Position = Vector2.Zero;
		card.previewClip.Size = new Vector2(50f, 70f);
		card.itemCostLabel.Visible = false;
		card.selectTexture.Visible = false;
		card.coldDownProgressBar.Visible = false;
		card.loveButton.Visible = false;
		ColorRect colorRect = new ColorRect();
		colorRect.Name = "Pattern";
		colorRect.Color = patternColor;
		colorRect.Position = (patternOnRightHalf ? new Vector2(25f, 0f) : Vector2.Zero);
		colorRect.Size = (patternOnRightHalf ? new Vector2(25f, 70f) : new Vector2(50f, 70f));
		colorRect.MouseFilter = Control.MouseFilterEnum.Ignore;
		card.previewSpriteNode.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
	}

	private static bool HasFlatCardZIndex(TowerDefenseInGamePacketShow card)
	{
		if (card.layout.ZIndex == 0 && card.backgroundTexture.ZIndex == 0 && card.previewClip.ZIndex == 0 && card.previewSpriteNode.ZIndex == 0 && card.itemCostLabel.ZIndex == 0 && card.selectTexture.ZIndex == 0 && card.button.ZIndex == 0 && card.coldDownProgressBar.ZIndex == 0)
		{
			return card.loveButton.ZIndex == 0;
		}
		return false;
	}

	private static bool HasFlatPreviewZIndex(Node node)
	{
		if (node is CanvasItem { ZIndex: not 0 })
		{
			return false;
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			if (!HasFlatPreviewZIndex(child))
			{
				return false;
			}
		}
		return true;
	}

	private static ImageTexture CreateSolidTexture(Color color)
	{
		using Image image = Image.CreateEmpty(2, 2, useMipmaps: false, Image.Format.Rgba8);
		image.Fill(color);
		return ImageTexture.CreateFromImage(image);
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[PacketOverlapLayeringRuntimeTest] " + message);
		}
	}

	private void Finish()
	{
		bool flag = _failures == 0 && _checks == 7;
		GD.Print($"{"PACKET_OVERLAP_LAYERING_RESULT"} passed={flag} checks={_checks} failures={_failures} screenshot={"res://.codex-tmp/packet-overlap-layering.png"}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "card", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Color, "backgroundColor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "patternColor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "patternOnRightHalf", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasFlatCardZIndex, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "card", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasFlatPreviewZIndex, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSolidTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ImageTexture"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ConfigureCard && args.Count == 4)
		{
			ConfigureCard(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasFlatCardZIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFlatCardZIndex(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0])));
			return true;
		}
		if (method == MethodName.HasFlatPreviewZIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFlatPreviewZIndex(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateSolidTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ImageTexture>(CreateSolidTexture(VariantUtils.ConvertTo<Color>(in args[0])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConfigureCard && args.Count == 4)
		{
			ConfigureCard(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasFlatCardZIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFlatCardZIndex(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0])));
			return true;
		}
		if (method == MethodName.HasFlatPreviewZIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFlatPreviewZIndex(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateSolidTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ImageTexture>(CreateSolidTexture(VariantUtils.ConvertTo<Color>(in args[0])));
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
		if (method == MethodName.ConfigureCard)
		{
			return true;
		}
		if (method == MethodName.HasFlatCardZIndex)
		{
			return true;
		}
		if (method == MethodName.HasFlatPreviewZIndex)
		{
			return true;
		}
		if (method == MethodName.CreateSolidTexture)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Finish)
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
