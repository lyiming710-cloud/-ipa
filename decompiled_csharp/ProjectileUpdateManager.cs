using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/TowerDefense/Projectile/ProjectileUpdateManager.cs")]
public class ProjectileUpdateManager : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _UnhandledInput = "_UnhandledInput";

		public static readonly StringName Clear = "Clear";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	public static ProjectileUpdateManager Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
		ProcessPriority = 2147483646;
	}

	public override void _PhysicsProcess(double delta)
	{
		long startTicks = TowerDefensePerfProfiler.Begin();
		ulong physicsFrames = Engine.GetPhysicsFrames();
		BulletField bulletField = (GodotObject.IsInstanceValid(BulletField.Instance) ? BulletField.Instance : BulletField.EnsureMountedOnCharacterNode());
		int items = 0;
		if (GodotObject.IsInstanceValid(bulletField))
		{
			TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
			bulletField.ProcessQueuedRenderTemplateWarmups(1);
			bulletField.UpdateSimulation(delta, physicsFrames);
			items = bulletField.ActiveCount;
			TowerDefensePerfProfiler.EndSpikeProbe("projectile.updateManager", in probe, items);
		}
		TowerDefensePerfProfiler.End("projectile.updateManager", startTicks, items);
		TowerDefensePerfProfiler.DumpIfNeeded();
	}

	public override void _Process(double delta)
	{
		BulletField instance = BulletField.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			instance.PublishRenderState(delta, Engine.GetPhysicsFrames());
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey { Pressed: not false, Echo: false } inputEventKey && inputEventKey.Keycode == Key.Key9)
		{
			bool flag = (TowerDefensePerfProfiler.DetailedHotPathMetrics = (TowerDefensePerfProfiler.Enabled = !TowerDefensePerfProfiler.Enabled));
			if (flag)
			{
				TowerDefensePerfProfiler.MaxMetricsPerDump = 48;
			}
			TowerDefensePerfProfiler.Reset();
			GD.Print($"[TDPerf] Enabled={flag} detailed={TowerDefensePerfProfiler.DetailedHotPathMetrics} dumpFrames={TowerDefensePerfProfiler.DumpIntervalFrames} maxMetrics={TowerDefensePerfProfiler.MaxMetricsPerDump}");
		}
	}

	public void Clear()
	{
		if (GodotObject.IsInstanceValid(BulletField.Instance))
		{
			BulletField.Instance.ClearActiveBullets();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._UnhandledInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._UnhandledInput && args.Count == 1)
		{
			_UnhandledInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._UnhandledInput)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
