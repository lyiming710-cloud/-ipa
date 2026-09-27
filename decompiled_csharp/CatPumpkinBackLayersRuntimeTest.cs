using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/CatPumpkinBackLayersRuntimeTest.cs")]
public class CatPumpkinBackLayersRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		TowerDefensePlantCatPumpkin plant = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter4/CatPumpkin/Scene/TowerDefensePlantCatPumpkin.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<TowerDefensePlantCatPumpkin>(PackedScene.GenEditState.Disabled);
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				plant.Position = new Vector2(400f, 300f);
				if (!string.IsNullOrEmpty(OS.GetEnvironment("CAT_PUMPKIN_CAPTURE_DIR")))
				{
					Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.CpuPose;
					plant.sprite.forceLocalRender = true;
					plant.sprite.forceCpuPoseRender = true;
				}
				AddChild(plant, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				CatPumpkinSprite sprite = (CatPumpkinSprite)plant.sprite;
				string[] array = new string[3] { "", "Custom0", "" };
				foreach (string skin in array)
				{
					plant.SwitchCustom(skin);
					string[] array2 = new string[2] { "Idle", "Fire" };
					foreach (string clip in array2)
					{
						sprite.SetAnimation(clip);
						bool condition = true;
						foreach (Variant key in sprite.back.flashAnimeData.layerDictionary.Keys)
						{
							StringName stringName = (StringName)key;
							if (stringName != (StringName)"Pumpkin_back" && stringName != (StringName)"skin2" && sprite.back.GetFliter(stringName))
							{
								condition = false;
								GD.Print($"CAT_PUMPKIN_UNEXPECTED_BACK_LAYER skin={skin} clip={clip} layer={stringName}");
							}
						}
						Check(condition, skin + "/" + clip + "：Back只能绘制后半壳，不能重复绘制尾巴、前壳或爪子。");
						Check(sprite.back.GetFliter("Pumpkin_back") == (skin == "") && sprite.back.GetFliter("skin2") == (skin == "Custom0"), skin + "/" + clip + "：后半壳必须保留正确的外观图层。");
						Check(sprite.GetFliter("Cattail_tail2") == (skin == "") && sprite.GetFliter("skin4_2") == (skin == "Custom0"), skin + "/" + clip + "：主体必须保留本外观的尾巴。");
						string captureDirectory = OS.GetEnvironment("CAT_PUMPKIN_CAPTURE_DIR");
						if (!string.IsNullOrEmpty(captureDirectory))
						{
							plant.Scale = Vector2.One * 3f;
							plant.Visible = true;
							sprite.Visible = true;
							sprite.pause = true;
							sprite.back.pause = true;
							sprite.frameIndex = sprite.clipRange.X + 8;
							sprite.elapsedTimer = 8.0 / sprite.frameRate;
							sprite.BatchPhysicsUpdate(0.0);
							sprite.UpdateChild();
							sprite.back.UpdateChild();
							sprite.QueueRedraw();
							sprite.back.QueueRedraw();
							for (int frame = 0; frame < 10; frame++)
							{
								await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
							}
							using Image image = GetViewport().GetTexture().GetImage();
							image.SavePng(Path.Combine(captureDirectory, ((skin == "") ? "Default" : skin) + "-" + clip + ".png"));
						}
					}
				}
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[CatPumpkinBackLayers] {value}");
			}
		}
		finally
		{
			plant.QueueFree();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		GD.Print($"CAT_PUMPKIN_BACK_LAYERS_RESULT passed={_checks == 18 && _failures == 0} checks={_checks} failures={_failures}");
		GetTree().Quit((_checks != 18 || _failures != 0) ? 2 : 0);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[CatPumpkinBackLayers] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
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
