using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ShopNpcReentryRuntimeProbe.cs")]
public class ShopNpcReentryRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreatePreviewShop = "CreatePreviewShop";

		public static readonly StringName GetNpcSprite = "GetNpcSprite";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _dialogLayer = "_dialogLayer";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ShopScenePath = "res://Prefab/GUI/DialogBox/Shop/Shop.tscn";

	private int _checks;

	private int _failures;

	private CanvasLayer _dialogLayer;

	public override async void _Ready()
	{
		try
		{
			await RunProbe();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[ShopNpcReentryRuntimeProbe] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"SHOP_NPC_REENTRY_RUNTIME_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private async Task RunProbe()
	{
		PackedScene scene = GD.Load<PackedScene>("res://Prefab/GUI/DialogBox/Shop/Shop.tscn");
		Check(GodotObject.IsInstanceValid(scene), "The shop scene must load.");
		if (GodotObject.IsInstanceValid(scene))
		{
			_dialogLayer = new CanvasLayer
			{
				Name = "DialogLayer",
				Layer = 100
			};
			AddChild(_dialogLayer, forceReadableName: false, InternalMode.Disabled);
			Shop firstShop = CreatePreviewShop(scene, "FirstShop");
			AdobeAnimateSprite firstSprite = GetNpcSprite(firstShop);
			await WaitProcessFrames(30);
			Check(firstSprite.IsVisibleInTree(), "The NPC must be visible on the first shop entry.");
			Check(firstSprite.IsRuntimeActive, "The NPC animation must be active on the first shop entry.");
			int firstEntryPixels = await MeasureNpcPixels(firstShop);
			Check(firstEntryPixels > 200, $"The first shop entry must render the NPC, got {firstEntryPixels} changed pixels.");
			firstShop.Hide();
			await WaitProcessFrames(3);
			Check(!firstSprite.IsVisibleInTree(), "The NPC must inherit the hidden shop state while away from the shop.");
			firstShop.Show();
			await WaitProcessFrames(8);
			Check(firstSprite.IsVisibleInTree(), "The NPC must become visible when returning to the same shop instance.");
			Check(firstSprite.IsRuntimeActive, "The NPC animation must resume when returning to the same shop instance.");
			int resumedEntryPixels = await MeasureNpcPixels(firstShop);
			Check(resumedEntryPixels > 200, $"Returning to the same shop must render the NPC, got {resumedEntryPixels} changed pixels.");
			firstShop.QueueFree();
			await WaitProcessFrames(3);
			Shop secondShop = CreatePreviewShop(scene, "SecondShop");
			AdobeAnimateSprite secondSprite = GetNpcSprite(secondShop);
			await WaitProcessFrames(30);
			Check(secondSprite.IsVisibleInTree(), "The NPC must be visible on the second shop instance.");
			Check(secondSprite.IsRuntimeActive, "The NPC animation must be active on the second shop instance.");
			int num = await MeasureNpcPixels(secondShop);
			Check(num > 200, $"The second shop entry must render the NPC, got {num} changed pixels.");
			GD.Print($"SHOP_NPC_REENTRY_PIXELS first={firstEntryPixels} resumed={resumedEntryPixels} second={num}");
			secondShop.QueueFree();
			await WaitProcessFrames(2);
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			_dialogLayer.QueueFree();
			scene.Dispose();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}

	private Shop CreatePreviewShop(PackedScene scene, string name)
	{
		Shop shop = scene.Instantiate<Shop>(PackedScene.GenEditState.Disabled);
		shop.Name = name;
		shop.editorPreviewMode = true;
		_dialogLayer.AddChild(shop, forceReadableName: false, InternalMode.Disabled);
		return shop;
	}

	private static AdobeAnimateSprite GetNpcSprite(Shop shop)
	{
		return shop.GetNode<NpcBase>("NpcNode/NpcWeiWeiMi").sprite;
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task<int> MeasureNpcPixels(Shop shop)
	{
		NpcBase npc = shop.GetNode<NpcBase>("NpcNode/NpcWeiWeiMi");
		Image visible = GetViewport().GetTexture().GetImage();
		npc.Hide();
		await WaitProcessFrames(3);
		Image hidden = GetViewport().GetTexture().GetImage();
		npc.Show();
		await WaitProcessFrames(3);
		try
		{
			int num = 0;
			int num2 = Math.Min(visible.GetWidth(), hidden.GetWidth());
			int num3 = Math.Min(visible.GetHeight(), hidden.GetHeight());
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					Color pixel = visible.GetPixel(j, i);
					Color pixel2 = hidden.GetPixel(j, i);
					if (Math.Abs(pixel.R - pixel2.R) + Math.Abs(pixel.G - pixel2.G) + Math.Abs(pixel.B - pixel2.B) + Math.Abs(pixel.A - pixel2.A) > 0.08f)
					{
						num++;
					}
				}
			}
			return num;
		}
		finally
		{
			visible.Dispose();
			hidden.Dispose();
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[ShopNpcReentryRuntimeProbe] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreatePreviewShop, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNpcSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shop", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
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
		if (method == MethodName.CreatePreviewShop && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Shop>(CreatePreviewShop(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetNpcSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(GetNpcSprite(VariantUtils.ConvertTo<Shop>(in args[0])));
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
		if (method == MethodName.GetNpcSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(GetNpcSprite(VariantUtils.ConvertTo<Shop>(in args[0])));
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
		if (method == MethodName.CreatePreviewShop)
		{
			return true;
		}
		if (method == MethodName.GetNpcSprite)
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
		if (name == PropertyName._dialogLayer)
		{
			_dialogLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
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
		if (name == PropertyName._dialogLayer)
		{
			value = VariantUtils.CreateFrom(in _dialogLayer);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._dialogLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._dialogLayer, Variant.From(in _dialogLayer));
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
		if (info.TryGetProperty(PropertyName._dialogLayer, out var value3))
		{
			_dialogLayer = value3.As<CanvasLayer>();
		}
	}
}
