using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/Chapter7MapCameraBoundsRuntimeTest.cs")]
public class Chapter7MapCameraBoundsRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifyMapCameraBounds = "VerifyMapCameraBounds";

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

	private const string CameraScenePath = "res://Registry/Battle/Feature/Camera/Control/TowerDefenseCameraControl.tscn";

	private static readonly string[] MapConfigPaths = new string[4] { "res://Asset/Config/Map/TreasureIslandFrontLawn/Config/TreasureIslandFrontlawnMapTreasureIslandFrontLawn.tres", "res://Asset/Config/Map/TreasureIslandFrontLawn/Config/TreasureIslandFrontlawnMapTreasureIslandFrontlawnNight.tres", "res://Asset/Config/Map/TreasureIslandBackyard/Config/TreasureIslandBackyardMapTreasureIslandBackyard.tres", "res://Asset/Config/Map/TreasureIslandBackyard/Config/TreasureIslandBackyardMapTreasureIslandBackyardNight.tres" };

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		TowerDefenseCameraControl towerDefenseCameraControl = null;
		try
		{
			towerDefenseCameraControl = ResourceLoader.Load<PackedScene>("res://Registry/Battle/Feature/Camera/Control/TowerDefenseCameraControl.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseCameraControl>(PackedScene.GenEditState.Disabled);
			Check(GodotObject.IsInstanceValid(towerDefenseCameraControl), "生产相机场景必须能够实例化。");
			if (!GodotObject.IsInstanceValid(towerDefenseCameraControl))
			{
				Finish();
				return;
			}
			AddChild(towerDefenseCameraControl, forceReadableName: false, InternalMode.Disabled);
			string[] mapConfigPaths = MapConfigPaths;
			foreach (string mapConfigPath in mapConfigPaths)
			{
				VerifyMapCameraBounds(towerDefenseCameraControl, mapConfigPath);
			}
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[Chapter7MapCameraBoundsRuntimeTest] 未处理异常：{value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(towerDefenseCameraControl))
			{
				towerDefenseCameraControl.QueueFree();
			}
		}
		Finish();
	}

	private void VerifyMapCameraBounds(TowerDefenseCameraControl cameraControl, string mapConfigPath)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = ResourceLoader.Load<TowerDefenseMapConfig>(mapConfigPath, null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(towerDefenseMapConfig) && towerDefenseMapConfig.mapSize == new Vector2(1400f, 600f) && towerDefenseMapConfig.mapOffset == new Vector2(-205f, 0f) && !string.IsNullOrWhiteSpace(towerDefenseMapConfig.mapTexturePath), "第七章地图必须保留 1400×600 逻辑范围、-205 背景偏移和背景资源路径：" + mapConfigPath);
		if (GodotObject.IsInstanceValid(towerDefenseMapConfig))
		{
			cameraControl.ApplyMapConfig(towerDefenseMapConfig);
			float x = GetViewport().GetVisibleRect().Size.X;
			float num = Mathf.Max(0f, towerDefenseMapConfig.mapSize.X - x);
			float b = Mathf.Max(0f, num - 56f);
			Check(Mathf.IsEqualApprox(cameraControl.camera.GlobalPosition.X, 0f) && Mathf.IsEqualApprox(cameraControl.cameraBeginMarker.GlobalPosition.X, 0f), $"第七章地图入场相机必须从逻辑 X=0 开始，不能从背景偏移 {towerDefenseMapConfig.mapOffset.X} 开始：{mapConfigPath}");
			Check(Mathf.IsEqualApprox(cameraControl.cameraRightViewMarker.GlobalPosition.X, num) && Mathf.IsEqualApprox(cameraControl.cameraPreViewMarker.GlobalPosition.X, b), $"第七章地图开场预览标记必须覆盖 1400 像素逻辑宽度，实际右侧={cameraControl.cameraRightViewMarker.GlobalPosition.X}，返回前={cameraControl.cameraPreViewMarker.GlobalPosition.X}：{mapConfigPath}");
			Check(cameraControl.downRightMarker.GlobalPosition.IsEqualApprox(towerDefenseMapConfig.mapSize), $"第七章地图右下相机边界必须使用逻辑地图尺寸，实际为 {cameraControl.downRightMarker.GlobalPosition}：{mapConfigPath}");
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[Chapter7MapCameraBoundsRuntimeTest] " + message);
		}
	}

	private void Finish()
	{
		bool flag = _failures == 0 && _checks == 17;
		GD.Print($"CHAPTER7_MAP_CAMERA_BOUNDS_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyMapCameraBounds, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cameraControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "mapConfigPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.VerifyMapCameraBounds && args.Count == 2)
		{
			VerifyMapCameraBounds(VariantUtils.ConvertTo<TowerDefenseCameraControl>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.VerifyMapCameraBounds)
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
