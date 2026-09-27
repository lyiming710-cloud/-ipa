using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Core/CrashLogger/CrashLogger.cs")]
public class CrashLogger : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName _InitPaths = "_InitPaths";

		public static readonly StringName _CopyPreviousGodotLogToExportDir = "_CopyPreviousGodotLogToExportDir";

		public static readonly StringName _FindPreviousGodotLogPath = "_FindPreviousGodotLogPath";

		public static readonly StringName _CleanupOldStartupLogExports = "_CleanupOldStartupLogExports";

		public static readonly StringName _EnsureDir = "_EnsureDir";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _startupLogExportDir = "_startupLogExportDir";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string StartupLogExportDirName = "PVZHE_Logs";

	private const int MaxStartupLogExports = 10;

	private string _startupLogExportDir = "";

	public static CrashLogger Instance;

	public override void _Ready()
	{
		Instance = this;
		if (!Engine.IsEditorHint())
		{
			_InitPaths();
			_CopyPreviousGodotLogToExportDir();
		}
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void _InitPaths()
	{
		if (OS.GetName() == "Android")
		{
			string systemDir = OS.GetSystemDir(OS.SystemDir.Documents);
			_startupLogExportDir = systemDir + "/PVZHE_Logs";
		}
		else
		{
			_startupLogExportDir = "user://PVZHE_Logs";
		}
	}

	private void _CopyPreviousGodotLogToExportDir()
	{
		string text = _FindPreviousGodotLogPath();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		_EnsureDir(_startupLogExportDir);
		using FileAccess fileAccess = FileAccess.Open(text, FileAccess.ModeFlags.Read);
		if (fileAccess == null)
		{
			GD.PushWarning("[CrashLogger] Failed to open previous Godot log: " + text);
			return;
		}
		string asText = fileAccess.GetAsText();
		string text2 = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
		string text3 = _startupLogExportDir + "/godot_startup_" + text2 + ".log";
		using FileAccess fileAccess2 = FileAccess.Open(text3, FileAccess.ModeFlags.Write);
		if (fileAccess2 == null)
		{
			GD.PushWarning("[CrashLogger] Failed to export previous Godot log: " + text3);
			return;
		}
		fileAccess2.StoreString(asText);
		_CleanupOldStartupLogExports();
		GD.Print("[CrashLogger] Previous Godot log exported: " + text3);
	}

	private static string _FindPreviousGodotLogPath()
	{
		string text = OS.GetUserDataDir() + "/logs";
		using DirAccess dirAccess = DirAccess.Open(text);
		if (dirAccess == null)
		{
			return "";
		}
		string result = "";
		ulong num = 0uL;
		dirAccess.ListDirBegin();
		string next = dirAccess.GetNext();
		while (next != "")
		{
			if (dirAccess.CurrentIsDir() || next == "godot.log" || !next.EndsWith(".log", StringComparison.OrdinalIgnoreCase))
			{
				next = dirAccess.GetNext();
				continue;
			}
			string text2 = text + "/" + next;
			ulong modifiedTime = FileAccess.GetModifiedTime(text2);
			if (modifiedTime >= num)
			{
				num = modifiedTime;
				result = text2;
			}
			next = dirAccess.GetNext();
		}
		dirAccess.ListDirEnd();
		return result;
	}

	private void _CleanupOldStartupLogExports()
	{
		using DirAccess dirAccess = DirAccess.Open(_startupLogExportDir);
		if (dirAccess == null)
		{
			return;
		}
		List<string> list = new List<string>();
		dirAccess.ListDirBegin();
		string next = dirAccess.GetNext();
		while (next != "")
		{
			if (next.StartsWith("godot_startup_", StringComparison.Ordinal) && next.EndsWith(".log", StringComparison.OrdinalIgnoreCase))
			{
				list.Add(next);
			}
			next = dirAccess.GetNext();
		}
		dirAccess.ListDirEnd();
		list.Sort(StringComparer.Ordinal);
		while (list.Count > 10)
		{
			string text = list[0];
			list.RemoveAt(0);
			DirAccess.RemoveAbsolute(_startupLogExportDir + "/" + text);
		}
	}

	private static void _EnsureDir(string path)
	{
		if (!DirAccess.DirExistsAbsolute(path))
		{
			DirAccess.MakeDirRecursiveAbsolute(path);
		}
	}

	public CrashLogger()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/CrashLogger");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._InitPaths, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._CopyPreviousGodotLogToExportDir, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._FindPreviousGodotLogPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CleanupOldStartupLogExports, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._EnsureDir, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._InitPaths && args.Count == 0)
		{
			_InitPaths();
			ret = default;
			return true;
		}
		if (method == MethodName._CopyPreviousGodotLogToExportDir && args.Count == 0)
		{
			_CopyPreviousGodotLogToExportDir();
			ret = default;
			return true;
		}
		if (method == MethodName._FindPreviousGodotLogPath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_FindPreviousGodotLogPath());
			return true;
		}
		if (method == MethodName._CleanupOldStartupLogExports && args.Count == 0)
		{
			_CleanupOldStartupLogExports();
			ret = default;
			return true;
		}
		if (method == MethodName._EnsureDir && args.Count == 1)
		{
			_EnsureDir(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._FindPreviousGodotLogPath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_FindPreviousGodotLogPath());
			return true;
		}
		if (method == MethodName._EnsureDir && args.Count == 1)
		{
			_EnsureDir(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._InitPaths)
		{
			return true;
		}
		if (method == MethodName._CopyPreviousGodotLogToExportDir)
		{
			return true;
		}
		if (method == MethodName._FindPreviousGodotLogPath)
		{
			return true;
		}
		if (method == MethodName._CleanupOldStartupLogExports)
		{
			return true;
		}
		if (method == MethodName._EnsureDir)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._startupLogExportDir)
		{
			_startupLogExportDir = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._startupLogExportDir)
		{
			value = VariantUtils.CreateFrom(in _startupLogExportDir);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._startupLogExportDir, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._startupLogExportDir, Variant.From(in _startupLogExportDir));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._startupLogExportDir, out var value))
		{
			_startupLogExportDir = value.As<string>();
		}
	}
}
