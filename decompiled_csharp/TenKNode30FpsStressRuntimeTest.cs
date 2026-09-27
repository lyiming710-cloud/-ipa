using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/TenKNode30FpsStressRuntimeTest.cs")]
public sealed class TenKNode30FpsStressRuntimeTest : Node
{
	private sealed class StressNode : Node
	{
		public new class MethodName : Node.MethodName
		{
			public new static readonly StringName _Process = "_Process";
		}

		public new class PropertyName : Node.PropertyName
		{
			public static readonly StringName _elapsed = "_elapsed";
		}

		public new class SignalName : Node.SignalName
		{
		}

		public static long ProcessCalls;

		private double _elapsed;

		public override void _Process(double delta)
		{
			_elapsed += delta;
			if (_elapsed >= 0.0)
			{
				ProcessCalls++;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if (method == MethodName._Process && args.Count == 1)
			{
				_Process(VariantUtils.ConvertTo<double>(in args[0]));
				ret = default;
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			if (method == MethodName._Process)
			{
				return true;
			}
			return base.HasGodotClassMethod(in method);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if (name == PropertyName._elapsed)
			{
				_elapsed = VariantUtils.ConvertTo<double>(in value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if (name == PropertyName._elapsed)
			{
				value = VariantUtils.CreateFrom(in _elapsed);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, PropertyName._elapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(PropertyName._elapsed, Variant.From(in _elapsed));
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			if (info.TryGetProperty(PropertyName._elapsed, out var value))
			{
				_elapsed = value.As<double>();
			}
		}
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _stressParent = "_stressParent";

		public static readonly StringName _previousMaxFps = "_previousMaxFps";

		public static readonly StringName _maxFpsOverridden = "_maxFpsOverridden";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 10000;

	private const int WarmupFrames = 30;

	private const int MeasuredFrames = 120;

	private Node _stressParent;

	private int _previousMaxFps;

	private bool _maxFpsOverridden;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		OS.LowProcessorUsageMode = false;
		Callable.From(Run).CallDeferred();
	}

	private async void Run()
	{
		bool passed = false;
		long processCalls = 0L;
		double actualFps = 0.0;
		double meanFrameMs = 0.0;
		try
		{
			_previousMaxFps = Engine.MaxFps;
			Engine.MaxFps = 30;
			_maxFpsOverridden = true;
			_stressParent = new Node
			{
				Name = "TenKStressNodes"
			};
			AddChild(_stressParent, forceReadableName: false, InternalMode.Disabled);
			for (int i = 0; i < 10000; i++)
			{
				_stressParent.AddChild(new StressNode(), forceReadableName: false, InternalMode.Disabled);
			}
			if (_stressParent.GetChildCount() != 10000)
			{
				throw new InvalidOperationException($"Expected {10000} real Nodes, got {_stressParent.GetChildCount()}.");
			}
			for (int frame = 0; frame < 30; frame++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}
			StressNode.ProcessCalls = 0L;
			long startTicks = Stopwatch.GetTimestamp();
			for (int frame = 0; frame < 120; frame++)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}
			double num = (double)(Stopwatch.GetTimestamp() - startTicks) * 1000.0 / (double)Stopwatch.Frequency;
			actualFps = ((num > 0.0) ? (120000.0 / num) : 0.0);
			meanFrameMs = num / 120.0;
			processCalls = StressNode.ProcessCalls;
			long num2 = 1200000L;
			passed = processCalls == num2 && actualFps >= 28.5 && actualFps <= 32.0;
		}
		catch (Exception ex)
		{
			GD.PushError(ex.ToString());
		}
		finally
		{
			if (_maxFpsOverridden)
			{
				Engine.MaxFps = _previousMaxFps;
			}
			GD.Print($"TEN_K_NODE_30FPS_STRESS passed={passed} instances={10000} warmupFrames={30} measuredFrames={120} processCalls={processCalls} expectedCalls={1200000L} actualFps={actualFps:F3} meanFrameMs={meanFrameMs:F6} maxFps={Engine.MaxFps}");
			GetTree().Quit((!passed) ? 2 : 0);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Run && args.Count == 0)
		{
			Run();
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
		if (method == MethodName.Run)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._stressParent)
		{
			_stressParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._previousMaxFps)
		{
			_previousMaxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._maxFpsOverridden)
		{
			_maxFpsOverridden = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._stressParent)
		{
			value = VariantUtils.CreateFrom(in _stressParent);
			return true;
		}
		if (name == PropertyName._previousMaxFps)
		{
			value = VariantUtils.CreateFrom(in _previousMaxFps);
			return true;
		}
		if (name == PropertyName._maxFpsOverridden)
		{
			value = VariantUtils.CreateFrom(in _maxFpsOverridden);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._stressParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._previousMaxFps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._maxFpsOverridden, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stressParent, Variant.From(in _stressParent));
		info.AddProperty(PropertyName._previousMaxFps, Variant.From(in _previousMaxFps));
		info.AddProperty(PropertyName._maxFpsOverridden, Variant.From(in _maxFpsOverridden));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stressParent, out var value))
		{
			_stressParent = value.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._previousMaxFps, out var value2))
		{
			_previousMaxFps = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._maxFpsOverridden, out var value3))
		{
			_maxFpsOverridden = value3.As<bool>();
		}
	}
}
