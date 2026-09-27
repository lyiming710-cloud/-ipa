using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateAdaptiveVisualCadenceRuntimeTest.cs")]
public class AdobeAnimateAdaptiveVisualCadenceRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	public override void _Ready()
	{
		Check(180, 0, 180.0);
		Check(180, 299, 180.0);
		Check(180, 300, 90.0);
		Check(180, 499, 90.0);
		Check(180, 500, 60.0);
		Check(180, 799, 60.0);
		Check(180, 800, 45.0);
		Check(180, 999, 45.0);
		Check(180, 1000, 30.0);
		Check(360, 0, 180.0);
		Check(144, 300, 90.0);
		Check(24, 1000, 24.0);
		bool flag = _failures.Count == 0;
		GD.Print($"ADOBE_ANIMATE_ADAPTIVE_CADENCE_RESULT passed={flag} checks=12 failures={_failures.Count}");
		for (int i = 0; i < _failures.Count; i++)
		{
			GD.PrintErr(_failures[i]);
		}
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private void Check(int renderFrameRate, int characterCount, double expected)
	{
		double num = AdobeAnimateRuntimeManager.ResolveAdaptiveVisualTicksPerSecond(renderFrameRate, characterCount);
		if (!(Math.Abs(num - expected) <= 0.0001))
		{
			_failures.Add($"render={renderFrameRate} characters={characterCount} expected={expected:F2} actual={num:F2}");
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
				new PropertyInfo(Variant.Type.Int, "renderFrameRate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "characterCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Check && args.Count == 3)
		{
			Check(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
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
