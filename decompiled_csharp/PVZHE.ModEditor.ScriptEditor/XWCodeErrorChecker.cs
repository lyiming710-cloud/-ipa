using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ScriptEditor;

[ScriptPath("res://addons/ModEditor/ScriptEditor/ErrorChecker/XWCodeErrorChecker.cs")]
public abstract class XWCodeErrorChecker : RefCounted
{
	public enum Severity
	{
		Error,
		Warning,
		Info
	}

	public class ErrorData
	{
		public int Line;

		public int Column;

		public int EndLine = -1;

		public int EndColumn = -1;

		public string Message = "";

		public Severity SeverityLevel;

		public string Code = "";

		public string FilePath = "";
	}

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName DoCheck = "DoCheck";

		public static readonly StringName ThrowIfCancellationRequested = "ThrowIfCancellationRequested";

		public static readonly StringName AddError = "AddError";

		public static readonly StringName AddErrorRange = "AddErrorRange";

		public static readonly StringName GetErrorCount = "GetErrorCount";

		public static readonly StringName GetWarningCount = "GetWarningCount";

		public static readonly StringName HasErrors = "HasErrors";

		public static readonly StringName HasWarnings = "HasWarnings";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public const int MaxDiagnostics = 256;

	protected readonly List<ErrorData> _errors = new List<ErrorData>();

	private CancellationToken _cancellationToken;

	public IReadOnlyList<ErrorData> Errors => _errors;

	protected CancellationToken CancellationToken => _cancellationToken;

	public List<ErrorData> CheckCode(string code)
	{
		return CheckCode(code, CancellationToken.None);
	}

	public List<ErrorData> CheckCode(string code, CancellationToken cancellationToken)
	{
		_errors.Clear();
		_cancellationToken = cancellationToken;
		try
		{
			ThrowIfCancellationRequested();
			DoCheck(code);
			ThrowIfCancellationRequested();
			return _errors;
		}
		finally
		{
			_cancellationToken = CancellationToken.None;
		}
	}

	protected abstract void DoCheck(string code);

	protected void ThrowIfCancellationRequested()
	{
		_cancellationToken.ThrowIfCancellationRequested();
	}

	protected void AddError(int line, int column, string message, Severity severity = Severity.Error, string code = "")
	{
		if (_errors.Count < 256)
		{
			_errors.Add(new ErrorData
			{
				Line = line,
				Column = column,
				Message = message,
				SeverityLevel = severity,
				Code = code
			});
		}
	}

	protected void AddErrorRange(int startLine, int startColumn, int endLine, int endColumn, string message, Severity severity = Severity.Error, string code = "")
	{
		if (_errors.Count < 256)
		{
			_errors.Add(new ErrorData
			{
				Line = startLine,
				Column = startColumn,
				EndLine = endLine,
				EndColumn = endColumn,
				Message = message,
				SeverityLevel = severity,
				Code = code
			});
		}
	}

	public int GetErrorCount()
	{
		int num = 0;
		foreach (ErrorData error in _errors)
		{
			if (error.SeverityLevel == Severity.Error)
			{
				num++;
			}
		}
		return num;
	}

	public int GetWarningCount()
	{
		int num = 0;
		foreach (ErrorData error in _errors)
		{
			if (error.SeverityLevel == Severity.Warning)
			{
				num++;
			}
		}
		return num;
	}

	public bool HasErrors()
	{
		foreach (ErrorData error in _errors)
		{
			if (error.SeverityLevel == Severity.Error)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasWarnings()
	{
		foreach (ErrorData error in _errors)
		{
			if (error.SeverityLevel == Severity.Warning)
			{
				return true;
			}
		}
		return false;
	}

	public List<ErrorData> GetOnlyErrors()
	{
		List<ErrorData> list = new List<ErrorData>();
		foreach (ErrorData error in _errors)
		{
			if (error.SeverityLevel == Severity.Error)
			{
				list.Add(error);
			}
		}
		return list;
	}

	public List<ErrorData> GetOnlyWarnings()
	{
		List<ErrorData> list = new List<ErrorData>();
		foreach (ErrorData error in _errors)
		{
			if (error.SeverityLevel == Severity.Warning)
			{
				list.Add(error);
			}
		}
		return list;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.DoCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "code", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ThrowIfCancellationRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddError, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "severity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "code", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddErrorRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "startLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "startColumn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "endLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "endColumn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "severity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "code", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetErrorCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetWarningCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasErrors, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasWarnings, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.DoCheck && args.Count == 1)
		{
			DoCheck(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ThrowIfCancellationRequested && args.Count == 0)
		{
			ThrowIfCancellationRequested();
			ret = default;
			return true;
		}
		if (method == MethodName.AddError && args.Count == 5)
		{
			AddError(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Severity>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddErrorRange && args.Count == 7)
		{
			AddErrorRange(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<Severity>(in args[5]), VariantUtils.ConvertTo<string>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetErrorCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetErrorCount());
			return true;
		}
		if (method == MethodName.GetWarningCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetWarningCount());
			return true;
		}
		if (method == MethodName.HasErrors && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasErrors());
			return true;
		}
		if (method == MethodName.HasWarnings && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasWarnings());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.DoCheck)
		{
			return true;
		}
		if (method == MethodName.ThrowIfCancellationRequested)
		{
			return true;
		}
		if (method == MethodName.AddError)
		{
			return true;
		}
		if (method == MethodName.AddErrorRange)
		{
			return true;
		}
		if (method == MethodName.GetErrorCount)
		{
			return true;
		}
		if (method == MethodName.GetWarningCount)
		{
			return true;
		}
		if (method == MethodName.HasErrors)
		{
			return true;
		}
		if (method == MethodName.HasWarnings)
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
