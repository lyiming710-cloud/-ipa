using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.ScriptEditor;

[ScriptPath("res://Test/ModRuntimeCompatibilityTest.cs")]
public class ModRuntimeCompatibilityTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RunProbe = "RunProbe";

		public static readonly StringName FormatBool = "FormatBool";

		public static readonly StringName ApplyCommandLineArguments = "ApplyCommandLineArguments";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName AssemblyPath = "AssemblyPath";

		public static readonly StringName RuntimeProfile = "RuntimeProfile";

		public static readonly StringName Iterations = "Iterations";
	}

	public new class SignalName : Node.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string AssemblyPath { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public string RuntimeProfile { get; set; } = "unknown";

	[Export(PropertyHint.None, "")]
	public int Iterations { get; set; } = 30;

	public override void _Ready()
	{
		ApplyCommandLineArguments();
		Callable.From(RunProbe).CallDeferred();
	}

	[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "This benchmark intentionally probes runtime-loaded Mod reflection.")]
	[UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "The dynamically loaded fixture has no statically analyzable members.")]
	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "The dynamically loaded fixture has no statically analyzable members.")]
	private void RunProbe()
	{
		if (string.IsNullOrWhiteSpace(AssemblyPath))
		{
			GD.PrintErr("[ModRuntimeCompatibilityTest] Fixture assembly was not found: " + AssemblyPath);
			GetTree().Quit(2);
			return;
		}
		string fullPath = Path.GetFullPath(AssemblyPath);
		if (!File.Exists(fullPath))
		{
			GD.PrintErr("[ModRuntimeCompatibilityTest] Fixture assembly was not found: " + fullPath);
			GetTree().Quit(2);
			return;
		}
		bool flag = !string.Equals(RuntimeProfile, "aot", StringComparison.OrdinalIgnoreCase);
		XWModAssemblyLoader xWModAssemblyLoader = new XWModAssemblyLoader();
		long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
		long totalAllocatedBytes = GC.GetTotalAllocatedBytes();
		int num = GC.CollectionCount(0);
		int num2 = GC.CollectionCount(1);
		int num3 = GC.CollectionCount(2);
		long timestamp = Stopwatch.GetTimestamp();
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		Exception ex = null;
		bool flag2 = false;
		try
		{
			for (int i = 0; i < Iterations; i++)
			{
				string modId = $"dynamic-mod-probe-{i}";
				xWModAssemblyLoader.LoadModAssembly(modId, fullPath, new string[1] { fullPath.GetBaseDir() });
				num4++;
				Type type = xWModAssemblyLoader.FindTypesAssignableTo(modId, typeof(object)).Find((Type type2) => type2.FullName == "DynamicModProbe.EntryPoint");
				if (type == null)
				{
					throw new TypeLoadException("DynamicModProbe.EntryPoint was not discovered.");
				}
				object obj = Activator.CreateInstance(type) ?? throw new MissingMethodException(type.FullName, ".ctor()");
				object obj2 = (type.GetMethod("Run", BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null) ?? throw new MissingMethodException(type.FullName, "Run()")).Invoke(obj, null);
				if (!(obj2 is int num8) || num8 != 42)
				{
					throw new InvalidOperationException($"Unexpected Mod probe result: {obj2 ?? "<null>"}.");
				}
				num7 = num8;
				num5++;
				if (!xWModAssemblyLoader.UnloadMod(modId))
				{
					throw new InvalidOperationException($"Mod probe unload failed for iteration {i}.");
				}
				num6++;
			}
			flag2 = true;
		}
		catch (Exception exception)
		{
			ex = UnwrapException(exception);
			GD.PrintErr($"[ModRuntimeCompatibilityTest] profile={RuntimeProfile} failure={ex.GetType().Name}: {ex.Message}");
		}
		finally
		{
			xWModAssemblyLoader.UnloadAll();
		}
		double value = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
		long value2 = GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread;
		long value3 = GC.GetTotalAllocatedBytes() - totalAllocatedBytes;
		int value4 = GC.CollectionCount(0) - num;
		int value5 = GC.CollectionCount(1) - num2;
		int value6 = GC.CollectionCount(2) - num3;
		bool flag3 = !flag && !flag2 && IsDynamicLoadingUnsupported(ex);
		bool flag4 = ((!flag) ? flag3 : (flag2 && num4 == Iterations && num5 == Iterations && num6 == Iterations && num7 == 42 && xWModAssemblyLoader.LoadedAssemblies.Count == 0));
		string value7 = ex?.GetType().Name ?? "none";
		GD.Print($"[ModRuntimeCompatibilityResult] runtimeProfile={RuntimeProfile} expectedSupported={FormatBool(flag)} supported={FormatBool(flag2)} iterations={Iterations} loads={num4} invokes={num5} unloads={num6} result={num7} failureType={value7} elapsedMs={value:F3} threadAllocatedBytes={value2} totalAllocatedBytes={value3} gen0={value4} gen1={value5} gen2={value6} passed={FormatBool(flag4)}");
		GetTree().Quit((!flag4) ? 1 : 0);
	}

	private static Exception UnwrapException(Exception exception)
	{
		while (exception is TargetInvocationException && exception.InnerException != null)
		{
			exception = exception.InnerException;
		}
		return exception;
	}

	private static bool IsDynamicLoadingUnsupported(Exception exception)
	{
		for (Exception ex = exception; ex != null; ex = ex.InnerException)
		{
			if (ex is PlatformNotSupportedException || ex is NotSupportedException)
			{
				return true;
			}
		}
		return false;
	}

	private static string FormatBool(bool value)
	{
		if (!value)
		{
			return "false";
		}
		return "true";
	}

	private void ApplyCommandLineArguments()
	{
		string[] cmdlineUserArgs = OS.GetCmdlineUserArgs();
		foreach (string text in cmdlineUserArgs)
		{
			int value;
			if (text.StartsWith("--mod-probe-assembly=", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = text;
				int length = "--mod-probe-assembly=".Length;
				AssemblyPath = text2.Substring(length, text2.Length - length).Trim();
			}
			else if (text.StartsWith("--runtime-profile=", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = text;
				int length = "--runtime-profile=".Length;
				RuntimeProfile = text2.Substring(length, text2.Length - length).Trim();
			}
			else if (TryReadInt(text, "--mod-probe-iterations=", out value))
			{
				Iterations = Math.Max(1, value);
			}
		}
	}

	private static bool TryReadInt(string arg, string prefix, out int value)
	{
		value = 0;
		if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			int length = prefix.Length;
			return int.TryParse(arg.Substring(length, arg.Length - length), out value);
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(4)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RunProbe, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.FormatBool, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ApplyCommandLineArguments, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.RunProbe && args.Count == 0)
		{
			RunProbe();
			ret = default;
			return true;
		}
		if (method == MethodName.FormatBool && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatBool(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyCommandLineArguments && args.Count == 0)
		{
			ApplyCommandLineArguments();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FormatBool && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatBool(VariantUtils.ConvertTo<bool>(in args[0])));
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
		if (method == MethodName.RunProbe)
		{
			return true;
		}
		if (method == MethodName.FormatBool)
		{
			return true;
		}
		if (method == MethodName.ApplyCommandLineArguments)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.AssemblyPath)
		{
			AssemblyPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeProfile)
		{
			RuntimeProfile = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Iterations)
		{
			Iterations = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.AssemblyPath)
		{
			from = AssemblyPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RuntimeProfile)
		{
			from = RuntimeProfile;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Iterations)
		{
			value = VariantUtils.CreateFrom<int>(Iterations);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName.AssemblyPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName.RuntimeProfile, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName.Iterations, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.AssemblyPath, Variant.From<string>(AssemblyPath));
		info.AddProperty(PropertyName.RuntimeProfile, Variant.From<string>(RuntimeProfile));
		info.AddProperty(PropertyName.Iterations, Variant.From<int>(Iterations));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.AssemblyPath, out var value))
		{
			AssemblyPath = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeProfile, out var value2))
		{
			RuntimeProfile = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Iterations, out var value3))
		{
			Iterations = value3.As<int>();
		}
	}
}
