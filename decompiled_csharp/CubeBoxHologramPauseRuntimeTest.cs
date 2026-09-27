using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/CubeBoxHologramPauseRuntimeTest.cs")]
public class CubeBoxHologramPauseRuntimeTest : Node
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
		double previousTimeScale = Engine.TimeScale;
		CubeBoxBloverControl control = null;
		try
		{
			_ = 3;
			try
			{
				control = new CubeBoxBloverControl
				{
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig(),
					ProcessMode = ProcessModeEnum.Pausable
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D();
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				TowerDefenseManager.Instance.currentControl = control;
				TowerDefensePlant plant = GD.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn").Instantiate<TowerDefensePlant>(PackedScene.GenEditState.Disabled);
				plant.Position = new Vector2(500f, 250f);
				plant.gridPos = new Vector2I(3, 2);
				control.characterNode.AddChild(plant, forceReadableName: false, InternalMode.Disabled);
				plant.instance.hologram = true;
				TowerDefensePlantCubeBox box = GD.Load<PackedScene>("res://Asset/Anime/Character/Plant/Diamond/CubeBox/Scene/TowerDefensePlantCubeBox.tscn").Instantiate<TowerDefensePlantCubeBox>(PackedScene.GenEditState.Disabled);
				box.editorPreviewMode = true;
				control.characterNode.AddChild(box, forceReadableName: false, InternalMode.Disabled);
				typeof(TowerDefensePlantCubeBox).GetMethod("ScheduleExpire", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(box, new object[1]
				{
					new List<TowerDefenseCharacter> { plant }
				});
				box.QueueFree();
				Engine.TimeScale = 20.0;
				await WaitWallTime(0.25);
				Check(!GodotObject.IsInstanceValid(box), "盒子本体必须已经被释放。");
				Check(GodotObject.IsInstanceValid(plant), "计时约5秒后投影必须仍存在。");
				GetTree().Paused = true;
				await WaitWallTime(2.0);
				Check(GodotObject.IsInstanceValid(plant), "暂停期间经过超过30秒的缩放时间也不能消耗投影寿命。");
				GetTree().Paused = false;
				await WaitWallTime(0.25);
				Check(GodotObject.IsInstanceValid(plant), "恢复后继续剩余时间，不能立即消失。");
				await WaitWallTime(1.2);
				Check(!GodotObject.IsInstanceValid(plant), "恢复后累计运行超过30秒时投影必须到期释放。");
			}
			catch (Exception ex)
			{
				_failures++;
				GD.PushError(ex.ToString());
			}
		}
		finally
		{
			GetTree().Paused = false;
			Engine.TimeScale = previousTimeScale;
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitWallTime(0.1);
		}
		GD.Print($"CUBE_BOX_HOLOGRAM_PAUSE_RESULT passed={_failures == 0 && _checks == 5} checks={_checks} failures={_failures}");
		GetTree().Quit((_failures != 0 || _checks != 5) ? 2 : 0);
	}

	private async Task WaitWallTime(double seconds)
	{
		await ToSignal(GetTree().CreateTimer(seconds, processAlways: true, processInPhysics: false, ignoreTimeScale: true), SceneTreeTimer.SignalName.Timeout);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.Print("CUBE_BOX_HOLOGRAM_PAUSE_FAILURE " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(2)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
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
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
