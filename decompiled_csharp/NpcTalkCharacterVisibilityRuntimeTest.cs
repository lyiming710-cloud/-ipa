using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/NpcTalkCharacterVisibilityRuntimeTest.cs")]
public class NpcTalkCharacterVisibilityRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CountPublishedInstances = "CountPublishedInstances";

		public static readonly StringName CountChangedPixels = "CountChangedPixels";

		public static readonly StringName ColorDistance = "ColorDistance";

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

	private const string CrazyDaveScenePath = "res://Prefab/Npc/CrazyDave/NpcCrazyDave.tscn";

	private const string NpcTalkControlScenePath = "res://Registry/Battle/Feature/NpcTalk/Control/NpcTalkControl.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		AdobeAnimateRenderBackend previousBackend = Global.Instance.adobeAnimateRenderBackend;
		CanvasLayer backgroundLayer = null;
		CanvasLayer npcLayer = null;
		try
		{
			_ = 2;
			try
			{
				Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
				AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
				AdobeAnimateDefinitionCache.Clear();
				backgroundLayer = new CanvasLayer
				{
					Name = "BackgroundLayer",
					Layer = -10
				};
				AddChild(backgroundLayer, forceReadableName: false, InternalMode.Disabled);
				backgroundLayer.AddChild(new ColorRect
				{
					Name = "Background",
					Position = Vector2.Zero,
					Size = GetViewport().GetVisibleRect().Size,
					Color = new Color(0.08f, 0.12f, 0.16f),
					MouseFilter = Control.MouseFilterEnum.Ignore
				}, forceReadableName: false, InternalMode.Disabled);
				npcLayer = new CanvasLayer
				{
					Name = "NpcTalkLayer",
					Layer = 4
				};
				AddChild(npcLayer, forceReadableName: false, InternalMode.Disabled);
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Registry/Battle/Feature/NpcTalk/Control/NpcTalkControl.tscn", null, ResourceLoader.CacheMode.Reuse);
				Check(GodotObject.IsInstanceValid(packedScene), "The production NPC talk control scene must load.");
				NpcTalkControl control = packedScene?.Instantiate<NpcTalkControl>(PackedScene.GenEditState.Disabled);
				if (!GodotObject.IsInstanceValid(control))
				{
					throw new InvalidOperationException("NPC talk control fixture could not be created.");
				}
				npcLayer.AddChild(control, forceReadableName: false, InternalMode.Disabled);
				PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://Prefab/Npc/CrazyDave/NpcCrazyDave.tscn", null, ResourceLoader.CacheMode.Reuse);
				Check(GodotObject.IsInstanceValid(packedScene2), "The production Crazy Dave scene must load.");
				NpcCrazyDave npc = packedScene2?.Instantiate<NpcCrazyDave>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(npc), "The production Crazy Dave scene must instantiate.");
				if (!GodotObject.IsInstanceValid(npc))
				{
					throw new InvalidOperationException("Crazy Dave fixture could not be created.");
				}
				npc.sprite.flashAnimeData = npc.sprite.flashAnimeData?.Duplicate(deep: true) as AdobeAnimateData;
				control.AddNpc(npc);
				await WaitFrames(2);
				TextureRect bubble = npc.GetNodeOrNull<TextureRect>("%TalkBubble");
				Control nodeOrNull = control.GetNodeOrNull<Control>("%NpcRenderMount");
				Check(GodotObject.IsInstanceValid(npc.sprite) && GodotObject.IsInstanceValid(bubble), "The NPC must keep both its Adobe Animate body and ordinary talk bubble.");
				if (!GodotObject.IsInstanceValid(npc.sprite) || !GodotObject.IsInstanceValid(bubble))
				{
					throw new InvalidOperationException("Crazy Dave visual nodes are unavailable.");
				}
				Check(npc.sprite.forceLocalRender && GodotObject.IsInstanceValid(nodeOrNull), "NpcTalkControl.AddNpc must route the body through the NPC-local render mount.");
				npc.sprite.SetAnimation("Idle");
				npc.sprite.pause = true;
				npc.sprite.Visible = true;
				bubble.Visible = true;
				npc.GetNode<Label>("%TalkLabel").Text = "NPC 对话人物可见性回归";
				await WaitFrames(12);
				Check(npc.IsVisibleInTree() && npc.sprite.IsVisibleInTree() && bubble.IsVisibleInTree(), "The production NPC, body, and talk bubble must all be logically visible.");
				int publishedInstances = CountPublishedInstances(GetTree().Root);
				Check(publishedInstances > 0, $"The NPC fallback render mount must publish body instances; count={publishedInstances}.");
				using Image visibleImage = GetViewport().GetTexture().GetImage();
				npc.sprite.Visible = false;
				await WaitFrames(6);
				using Image hidden = GetViewport().GetTexture().GetImage();
				int num = CountChangedPixels(visibleImage, hidden);
				Check(num >= 256, $"Hiding only the NPC body must remove visible character pixels while the bubble stays mounted; changed={num}.");
				GD.Print($"NPC_TALK_CHARACTER_VISIBILITY_METRIC changed={num} instances={publishedInstances} bubbleVisible={bubble.IsVisibleInTree()}");
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[NpcTalkCharacterVisibilityRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(npcLayer))
			{
				npcLayer.QueueFree();
			}
			if (GodotObject.IsInstanceValid(backgroundLayer))
			{
				backgroundLayer.QueueFree();
			}
			await WaitFrames(6);
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			Global.Instance.adobeAnimateRenderBackend = previousBackend;
		}
		bool flag = _failures == 0 && _checks == 8;
		GD.Print($"NPC_TALK_CHARACTER_VISIBILITY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static int CountPublishedInstances(Node node)
	{
		int num = ((node is AdobeAnimateMultiMeshBatcher adobeAnimateMultiMeshBatcher) ? adobeAnimateMultiMeshBatcher.GetVisibleInstanceCountForTest() : 0);
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			num += CountPublishedInstances(child);
		}
		return num;
	}

	private static int CountChangedPixels(Image visible, Image hidden)
	{
		int num = Math.Min(visible.GetWidth(), hidden.GetWidth());
		int num2 = Math.Min(visible.GetHeight(), hidden.GetHeight());
		int num3 = 0;
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				if (ColorDistance(visible.GetPixel(j, i), hidden.GetPixel(j, i)) > 0.08f)
				{
					num3++;
				}
			}
		}
		return num3;
	}

	private static float ColorDistance(Color left, Color right)
	{
		return Mathf.Max(Mathf.Max(Mathf.Abs(left.R - right.R), Mathf.Abs(left.G - right.G)), Mathf.Max(Mathf.Abs(left.B - right.B), Mathf.Abs(left.A - right.A)));
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[NpcTalkCharacterVisibilityRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountPublishedInstances, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CountChangedPixels, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Object, "hidden", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false)
			}, null),
			new MethodInfo(MethodName.ColorDistance, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CountPublishedInstances && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountPublishedInstances(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CountChangedPixels && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountChangedPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
			return true;
		}
		if (method == MethodName.ColorDistance && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(ColorDistance(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
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
		if (method == MethodName.CountPublishedInstances && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountPublishedInstances(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CountChangedPixels && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountChangedPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
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
		if (method == MethodName.CountPublishedInstances)
		{
			return true;
		}
		if (method == MethodName.CountChangedPixels)
		{
			return true;
		}
		if (method == MethodName.ColorDistance)
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
