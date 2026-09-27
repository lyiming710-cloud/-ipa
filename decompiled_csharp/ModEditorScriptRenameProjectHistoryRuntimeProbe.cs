using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.ScriptEditor;

[ScriptPath("res://Tests/ModEditorScriptRenameProjectHistoryRuntimeProbe.cs")]
public class ModEditorScriptRenameProjectHistoryRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName TryDeleteProbeRoot = "TryDeleteProbeRoot";

		public static readonly StringName Require = "Require";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	public override async void _Ready()
	{
		bool historyScoped = false;
		bool foreignUndoDenied = false;
		bool ownerUndoSucceeded = false;
		bool foreignRedoDenied = false;
		bool ownerRedoSucceeded = false;
		string probeRoot = ProjectSettings.GlobalizePath("user://mod_editor_script_rename_project_history");
		string rootA = Path.Combine(probeRoot, "ModA");
		string rootB = Path.Combine(probeRoot, "ModB");
		string pathA = Path.Combine(rootA, "A.cs");
		string pathB = Path.Combine(rootB, "B.cs");
		try
		{
			TryDeleteProbeRoot(probeRoot);
			Directory.CreateDirectory(rootA);
			Directory.CreateDirectory(rootB);
			File.WriteAllText(pathA, "namespace ModA;\npublic class Alpha { public Alpha Next; }\n");
			File.WriteAllText(pathB, "namespace ModB;\npublic class Beta { }\n");
			int offset = "namespace ModA;\npublic class Alpha { public Alpha Next; }\n".IndexOf("Alpha", StringComparison.Ordinal);
			XWCSharpIdeService.RenamePreview renamePreview = await XWCSharpIdeService.PreviewRenameAsync(rootA, pathA, "namespace ModA;\npublic class Alpha { public Alpha Next; }\n", offset, "RenamedAlpha");
			Require(renamePreview?.IsValid ?? false, "Could not prepare the Mod A rename transaction.");
			bool flag = renamePreview?.IsValid ?? false;
			if (flag)
			{
				flag = await XWCSharpIdeService.ApplyRenameAsync(renamePreview);
			}
			bool flag2 = flag;
			Require(flag2 && File.ReadAllText(pathA) == "namespace ModA;\npublic class RenamedAlpha { public RenamedAlpha Next; }\n", "Could not apply the Mod A rename transaction.");
			XWCSharpIdeService.RenameHistoryAvailability renameHistoryAvailability = XWCSharpIdeService.GetRenameHistoryAvailability(rootA);
			XWCSharpIdeService.RenameHistoryAvailability renameHistoryAvailability2 = XWCSharpIdeService.GetRenameHistoryAvailability(rootB);
			historyScoped = renameHistoryAvailability.CanUndo && !renameHistoryAvailability.CanRedo && !renameHistoryAvailability2.CanUndo && !renameHistoryAvailability2.CanRedo;
			Require(historyScoped, "Rename history availability leaked from Mod A into Mod B.");
			foreignUndoDenied = !(await XWCSharpIdeService.UndoLastRenameAsync(rootB)) && File.ReadAllText(pathA) == "namespace ModA;\npublic class RenamedAlpha { public RenamedAlpha Next; }\n" && File.ReadAllText(pathB) == "namespace ModB;\npublic class Beta { }\n";
			Require(foreignUndoDenied, "Mod B undo modified the previous Mod A transaction.");
			bool flag3 = await XWCSharpIdeService.UndoLastRenameAsync(rootA);
			XWCSharpIdeService.RenameHistoryAvailability renameHistoryAvailability3 = XWCSharpIdeService.GetRenameHistoryAvailability(rootA);
			ownerUndoSucceeded = flag3 && File.ReadAllText(pathA) == "namespace ModA;\npublic class Alpha { public Alpha Next; }\n" && !renameHistoryAvailability3.CanUndo && renameHistoryAvailability3.CanRedo;
			Require(ownerUndoSucceeded, "The owning Mod could not undo its rename transaction.");
			foreignRedoDenied = !(await XWCSharpIdeService.RedoLastRenameAsync(rootB)) && File.ReadAllText(pathA) == "namespace ModA;\npublic class Alpha { public Alpha Next; }\n" && File.ReadAllText(pathB) == "namespace ModB;\npublic class Beta { }\n";
			Require(foreignRedoDenied, "Mod B redo modified the previous Mod A transaction.");
			ownerRedoSucceeded = await XWCSharpIdeService.RedoLastRenameAsync(rootA) && File.ReadAllText(pathA) == "namespace ModA;\npublic class RenamedAlpha { public RenamedAlpha Next; }\n" && File.ReadAllText(pathB) == "namespace ModB;\npublic class Beta { }\n";
			Require(ownerRedoSucceeded, "The owning Mod could not redo its rename transaction.");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		finally
		{
			XWCSharpCodeModel.ResetIndexForProbe();
			TryDeleteProbeRoot(probeRoot);
		}
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_SCRIPT_RENAME_HISTORY_FAILURE] " + failure);
		}
		GD.Print($"[MOD_EDITOR_SCRIPT_RENAME_HISTORY_PROBE] historyScoped={historyScoped} foreignUndoDenied={foreignUndoDenied} ownerUndoSucceeded={ownerUndoSucceeded} foreignRedoDenied={foreignRedoDenied} ownerRedoSucceeded={ownerRedoSucceeded} failures={_failures.Count}");
		Console.Out.Flush();
		Console.Error.Flush();
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	private static void TryDeleteProbeRoot(string path)
	{
		try
		{
			string fullPath = Path.GetFullPath(path);
			string value = Path.GetFullPath(ProjectSettings.GlobalizePath("user://")).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
			if (fullPath.StartsWith(value, StringComparison.OrdinalIgnoreCase) && Directory.Exists(fullPath))
			{
				Directory.Delete(fullPath, recursive: true);
			}
		}
		catch
		{
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryDeleteProbeRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.TryDeleteProbeRoot && args.Count == 1)
		{
			TryDeleteProbeRoot(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.TryDeleteProbeRoot && args.Count == 1)
		{
			TryDeleteProbeRoot(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
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
		if (method == MethodName.TryDeleteProbeRoot)
		{
			return true;
		}
		if (method == MethodName.Require)
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
