using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/IZMGrowUpProduceSunNumRuntimeTest.cs")]
public class IZMGrowUpProduceSunNumRuntimeTest : Node
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

	private const string SunShroomScenePath = "res://Asset/Anime/Character/Plant/Chapter1/SunShroom/Scene/TowerDefensePlantSunShroom.tscn";

	private const int ExpectedGrownSunNum = 137;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		IZMGrowUpProduceSunNumRuntimeControlStub control = null;
		TowerDefensePlantSunShroom sunShroom = null;
		try
		{
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_0069;
				}
				control = new IZMGrowUpProduceSunNumRuntimeControlStub
				{
					Name = "IZMGrowUpProduceSunNumRuntimeControl",
					isGameRunning = true,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig
					{
						finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM,
						izmManager = new TowerDefenseLevelIZMManagerConfig()
					}
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				Check(manager.IsIZMMode(), "The fixture must run through the production IZM mode check.");
				sunShroom = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter1/SunShroom/Scene/TowerDefensePlantSunShroom.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefensePlantSunShroom>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(sunShroom), "The real SunShroom scene must instantiate.");
				if (!GodotObject.IsInstanceValid(sunShroom))
				{
					goto end_IL_0069;
				}
				sunShroom.inGame = false;
				sunShroom.growUpSunNum = 137;
				AddChild(sunShroom, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				GrowUpComponent growUpComponent = sunShroom.componentManager?.GetRuntime<GrowUpComponent>();
				ProduceComponent produceComponent = sunShroom.componentManager?.GetRuntime<ProduceComponent>();
				Check(growUpComponent != null && !growUpComponent.IsReleased && produceComponent != null && !produceComponent.IsReleased, "The real growth and production runtimes must both be active.");
				Check(growUpComponent?.growUpReach == growUpComponent?.growUpTime.Count && growUpComponent != null && growUpComponent.growUpReach == 1, $"IZM must fast-forward the real growth runtime; reach={growUpComponent?.growUpReach}.");
				Check(produceComponent != null && produceComponent.num == 137, $"The IZM growth callback must synchronize grown sunNum to ProduceComponent; actual={produceComponent?.num}.");
				IZMGrowUpProduceSunNumRuntimeTest iZMGrowUpProduceSunNumRuntimeTest = this;
				Vector2? vector = sunShroom.transformPoint?.Scale;
				Vector2 one = Vector2.One;
				float? num = growUpComponent?.growUpSize[0];
				iZMGrowUpProduceSunNumRuntimeTest.Check(vector == one * num, "IZM fast growth must still apply the final configured visual scale.");
				goto end_IL_0060;
				end_IL_0069:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[IZMGrowUpProduceSunNumRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0060;
			}
			return;
			end_IL_0060:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			if (GodotObject.IsInstanceValid(sunShroom))
			{
				sunShroom.QueueFree();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0 && _checks == 7;
		GD.Print($"IZM_GROW_UP_PRODUCE_SUN_NUM_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[IZMGrowUpProduceSunNumRuntimeTest] " + message);
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
