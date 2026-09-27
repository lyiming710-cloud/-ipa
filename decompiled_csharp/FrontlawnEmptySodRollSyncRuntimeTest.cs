using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/FrontlawnEmptySodRollSyncRuntimeTest.cs")]
public class FrontlawnEmptySodRollSyncRuntimeTest : Node
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

	private const string ResultMarker = "FRONTLAWN_EMPTY_SOD_ROLL_SYNC_RESULT";

	private const string MapScenePath = "res://Asset/Config/Map/Frontlawn/Scene/Empty/TowerDefenseMapFrontlawnEmpty.tscn";

	private const string SodRollScenePath = "res://Asset/Anime/Effect/SodRoll/SodRoll.tscn";

	private const double ExpectedRevealDistance = 790.0;

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		TowerDefenseMapFrontlawnEmpty towerDefenseMapFrontlawnEmpty = null;
		AdobeAnimateSprite adobeAnimateSprite = null;
		try
		{
			PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Config/Map/Frontlawn/Scene/Empty/TowerDefenseMapFrontlawnEmpty.tscn", null, ResourceLoader.CacheMode.Ignore);
			PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Effect/SodRoll/SodRoll.tscn", null, ResourceLoader.CacheMode.Ignore);
			if (!GodotObject.IsInstanceValid(packedScene) || !GodotObject.IsInstanceValid(packedScene2))
			{
				throw new InvalidOperationException("无法加载 Empty 地图或草卷动画正式场景。");
			}
			towerDefenseMapFrontlawnEmpty = packedScene.Instantiate<TowerDefenseMapFrontlawnEmpty>(PackedScene.GenEditState.Disabled);
			adobeAnimateSprite = packedScene2.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			Check(GodotObject.IsInstanceValid(towerDefenseMapFrontlawnEmpty) && GodotObject.IsInstanceValid(adobeAnimateSprite), "必须能够实例化 Empty 地图与正式草卷动画。");
			AddChild(towerDefenseMapFrontlawnEmpty, forceReadableName: false, InternalMode.Disabled);
			Sprite2D nodeOrNull = towerDefenseMapFrontlawnEmpty.GetNodeOrNull<Sprite2D>("%FrontlawnSod1Row");
			Check(GodotObject.IsInstanceValid(nodeOrNull), "Empty 地图必须包含第一行草坪节点。");
			if (!GodotObject.IsInstanceValid(nodeOrNull))
			{
				throw new InvalidOperationException("第一行草坪节点无效。");
			}
			Check(nodeOrNull.Scale == Vector2.One && GodotObject.IsInstanceValid(nodeOrNull.Texture) && nodeOrNull.Texture.GetWidth() == 770, "缩小后的第一行草坪贴图必须以一倍缩放显示 770 像素宽资源。");
			adobeAnimateSprite.SetAnimation("Idle", loop: false);
			adobeAnimateSprite.pause = true;
			towerDefenseMapFrontlawnEmpty.followSodRoll = adobeAnimateSprite;
			towerDefenseMapFrontlawnEmpty.createRowId = 0;
			towerDefenseMapFrontlawnEmpty.alive = true;
			int[] array = new int[5] { 0, 35, 70, 105, 140 };
			for (int i = 0; i < array.Length; i++)
			{
				int value = (adobeAnimateSprite.frameIndex = array[i]);
				double progress = adobeAnimateSprite.GetProgress();
				towerDefenseMapFrontlawnEmpty._PhysicsProcess(0.0);
				double num = nodeOrNull.RegionRect.Size.X;
				double num2 = progress * 790.0;
				Check(Math.Abs(num - num2) <= 0.01, $"草卷帧 {value} 的草坪揭示宽度应为 {num2:F3}，实际为 {num:F3}。");
			}
			towerDefenseMapFrontlawnEmpty.Finish();
			Check(Math.Abs((double)nodeOrNull.RegionRect.Size.X - 790.0) <= 0.01 && !towerDefenseMapFrontlawnEmpty.alive && towerDefenseMapFrontlawnEmpty.createRowId == -1 && towerDefenseMapFrontlawnEmpty.followSodRoll == null, "草卷完成后第一行草坪必须停在 790 像素并清理动画跟随状态。");
		}
		catch (Exception ex)
		{
			_failures++;
			GD.PushError(ex.ToString());
		}
		finally
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.Free();
			}
			if (GodotObject.IsInstanceValid(towerDefenseMapFrontlawnEmpty))
			{
				towerDefenseMapFrontlawnEmpty.Free();
			}
		}
		bool flag = _failures == 0;
		GD.Print($"{"FRONTLAWN_EMPTY_SOD_ROLL_SYNC_RESULT"} passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError(message);
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
