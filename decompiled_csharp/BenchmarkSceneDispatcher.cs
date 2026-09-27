using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BenchmarkSceneDispatcher.cs")]
public class BenchmarkSceneDispatcher : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Dispatch = "Dispatch";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ArgumentPrefix = "--benchmark-scene=";

	private static readonly IReadOnlyDictionary<string, string> AllowedScenes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		["short"] = "res://Test/DamageHotPathWorkloadTest.tscn",
		["damage"] = "res://Test/DamageHotPathWorkloadTest.tscn",
		["characters"] = "res://Test/CharacterStressWorkloadTest.tscn",
		["bullets"] = "res://Test/BulletFieldWorkloadTest.tscn",
		["mod"] = "res://Test/ModRuntimeCompatibilityTest.tscn"
	};

	public override void _Ready()
	{
		string[] cmdlineUserArgs = OS.GetCmdlineUserArgs();
		foreach (string text in cmdlineUserArgs)
		{
			if (text.StartsWith("--benchmark-scene=", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = text;
				int length = "--benchmark-scene=".Length;
				string benchmarkName = text2.Substring(length, text2.Length - length).Trim();
				Callable.From(() =>
				{
					Dispatch(benchmarkName);
				}).CallDeferred();
				break;
			}
		}
	}

	private void Dispatch(string benchmarkName)
	{
		if (!AllowedScenes.TryGetValue(benchmarkName, out var value))
		{
			GD.PrintErr("[BenchmarkSceneDispatcher] Unknown benchmark scene '" + benchmarkName + "'.");
			GetTree().Quit(2);
			return;
		}
		PackedScene packedScene = GD.Load<PackedScene>(value);
		if (packedScene == null || !packedScene.CanInstantiate())
		{
			GD.PrintErr("[BenchmarkSceneDispatcher] Failed to load '" + value + "'.");
			GetTree().Quit(2);
			return;
		}
		Error error = GetTree().ChangeSceneToPacked(packedScene);
		GD.Print($"[BenchmarkSceneDispatcher] benchmark={benchmarkName} scene={value} error={error}");
		if (error != Error.Ok)
		{
			GetTree().Quit(2);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Dispatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "benchmarkName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Dispatch && args.Count == 1)
		{
			Dispatch(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.Dispatch)
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
