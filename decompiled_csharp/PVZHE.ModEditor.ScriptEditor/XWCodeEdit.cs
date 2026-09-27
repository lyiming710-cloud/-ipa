using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.OutPutPanel;

namespace PVZHE.ModEditor.ScriptEditor;

[ScriptPath("res://addons/ModEditor/ScriptEditor/GUI/XWCodeEdit.cs")]
public class XWCodeEdit : CodeEdit
{
	[Signal]
	public delegate void ErrorsChangedEventHandler(int errorCount, int warningCount);

	[Signal]
	public delegate void DiagnosticsChangedEventHandler();

	[Signal]
	public delegate void ErrorClickedEventHandler(int line, int column);

	public readonly struct ValidationMetrics(int requests, int coalescedRequests, int executedChecks, int publishedResults, int staleResults, int canceledChecks, int activeWorkers, int peakActiveWorkers, int pendingRequests, int lastDiagnosticCount, long maxCaptureMilliseconds, long maxApplyMilliseconds)
	{
		public readonly int Requests = requests;

		public readonly int CoalescedRequests = coalescedRequests;

		public readonly int ExecutedChecks = executedChecks;

		public readonly int PublishedResults = publishedResults;

		public readonly int StaleResults = staleResults;

		public readonly int CanceledChecks = canceledChecks;

		public readonly int ActiveWorkers = activeWorkers;

		public readonly int PeakActiveWorkers = peakActiveWorkers;

		public readonly int PendingRequests = pendingRequests;

		public readonly int LastDiagnosticCount = lastDiagnosticCount;

		public readonly long MaxCaptureMilliseconds = maxCaptureMilliseconds;

		public readonly long MaxApplyMilliseconds = maxApplyMilliseconds;
	}

	private sealed class ValidationRequest
	{
		public string Code = "";

		public string Extension = "";

		public int Generation;
	}

	public readonly struct CompletionMetrics(int requests, int coalescedRequests, int executedAnalyses, int workerEnteredAnalyses, int activeAnalysisWorkers, int appliedResults, int canceledAnalyses, int staleResults, int activeWorkers, int peakActiveWorkers, int pendingRequests, int lastRequestedRevision, int lastAppliedRevision, int lastOptionCount, long maxCaptureMilliseconds, long maxApplyMilliseconds, string lastAppliedPrefix)
	{
		public readonly int Requests = requests;

		public readonly int CoalescedRequests = coalescedRequests;

		public readonly int ExecutedAnalyses = executedAnalyses;

		public readonly int WorkerEnteredAnalyses = workerEnteredAnalyses;

		public readonly int ActiveAnalysisWorkers = activeAnalysisWorkers;

		public readonly int AppliedResults = appliedResults;

		public readonly int CanceledAnalyses = canceledAnalyses;

		public readonly int StaleResults = staleResults;

		public readonly int ActiveWorkers = activeWorkers;

		public readonly int PeakActiveWorkers = peakActiveWorkers;

		public readonly int PendingRequests = pendingRequests;

		public readonly int LastRequestedRevision = lastRequestedRevision;

		public readonly int LastAppliedRevision = lastAppliedRevision;

		public readonly int LastOptionCount = lastOptionCount;

		public readonly long MaxCaptureMilliseconds = maxCaptureMilliseconds;

		public readonly long MaxApplyMilliseconds = maxApplyMilliseconds;

		public readonly string LastAppliedPrefix = lastAppliedPrefix ?? "";
	}

	private sealed class CompletionRequest
	{
		public string Source = "";

		public string DocumentPath = "";

		public int CaretLine;

		public int CaretColumn;

		public int Revision;
	}

	public new class MethodName : CodeEdit.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadIconSafe = "LoadIconSafe";

		public static readonly StringName SetupErrorGutter = "SetupErrorGutter";

		public static readonly StringName CreateCompletionTimer = "CreateCompletionTimer";

		public static readonly StringName CreateValidationTimer = "CreateValidationTimer";

		public static readonly StringName SetFileExtension = "SetFileExtension";

		public static readonly StringName SuspendBackgroundWork = "SuspendBackgroundWork";

		public static readonly StringName ResumeBackgroundWork = "ResumeBackgroundWork";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName SetDocumentPath = "SetDocumentPath";

		public static readonly StringName SetProjectRoot = "SetProjectRoot";

		public static readonly StringName GetDocumentPath = "GetDocumentPath";

		public static readonly StringName GetProjectRoot = "GetProjectRoot";

		public static readonly StringName GetBreakpointLines = "GetBreakpointLines";

		public static readonly StringName ToggleBreakpoint = "ToggleBreakpoint";

		public static readonly StringName UpdateCompletionProvider = "UpdateCompletionProvider";

		public static readonly StringName UpdateSyntaxHighlighter = "UpdateSyntaxHighlighter";

		public static readonly StringName CheckSyntaxErrors = "CheckSyntaxErrors";

		public static readonly StringName UpdateValidationDelay = "UpdateValidationDelay";

		public static readonly StringName OnCompletionTimerTimeout = "OnCompletionTimerTimeout";

		public static readonly StringName OnValidationTimerTimeout = "OnValidationTimerTimeout";

		public static readonly StringName OnTextChanged = "OnTextChanged";

		public static readonly StringName OnCaretChanged = "OnCaretChanged";

		public static readonly StringName RequestCompletionNow = "RequestCompletionNow";

		public static readonly StringName QueueCompletionRequest = "QueueCompletionRequest";

		public static readonly StringName StartCompletionWorkerLockedIfNeeded = "StartCompletionWorkerLockedIfNeeded";

		public static readonly StringName NormalizeIndexPath = "NormalizeIndexPath";

		public static readonly StringName InvalidateCompletionWork = "InvalidateCompletionWork";

		public static readonly StringName GetPendingCompletionRequestCount = "GetPendingCompletionRequestCount";

		public static readonly StringName OnCompletionAnalysisWorkerEntered = "OnCompletionAnalysisWorkerEntered";

		public static readonly StringName OnCompletionAnalysisWorkerExited = "OnCompletionAnalysisWorkerExited";

		public static readonly StringName LastCompletionContainsOption = "LastCompletionContainsOption";

		public static readonly StringName PublishedCompletionContainsOption = "PublishedCompletionContainsOption";

		public static readonly StringName ResetCompletionMetricsForProbe = "ResetCompletionMetricsForProbe";

		public static readonly StringName RequestValidationNow = "RequestValidationNow";

		public static readonly StringName QueueValidationRequest = "QueueValidationRequest";

		public static readonly StringName StartValidationWorkerLockedIfNeeded = "StartValidationWorkerLockedIfNeeded";

		public static readonly StringName CancelValidationWork = "CancelValidationWork";

		public static readonly StringName ClearPendingValidationRequest = "ClearPendingValidationRequest";

		public static readonly StringName CancelActiveValidationCheck = "CancelActiveValidationCheck";

		public static readonly StringName GetPendingValidationRequestCount = "GetPendingValidationRequestCount";

		public static readonly StringName UpdatePeakActiveWorkers = "UpdatePeakActiveWorkers";

		public static readonly StringName ResetValidationMetricsForProbe = "ResetValidationMetricsForProbe";

		public static readonly StringName AddError = "AddError";

		public static readonly StringName SetErrorGutterIcon = "SetErrorGutterIcon";

		public static readonly StringName ClearErrorGutterIcon = "ClearErrorGutterIcon";

		public static readonly StringName ClearAllErrors = "ClearAllErrors";

		public static readonly StringName GetErrorCount = "GetErrorCount";

		public static readonly StringName GetWarningCount = "GetWarningCount";

		public static readonly StringName HasAnyErrors = "HasAnyErrors";

		public static readonly StringName GetAllText = "GetAllText";

		public static readonly StringName SetAllText = "SetAllText";

		public static readonly StringName LoadDocumentText = "LoadDocumentText";

		public static readonly StringName InsertAtCursor = "InsertAtCursor";

		public new static readonly StringName _CanDropData = "_CanDropData";

		public new static readonly StringName _DropData = "_DropData";

		public static readonly StringName SetCaretFromDropPosition = "SetCaretFromDropPosition";

		public static readonly StringName IsCSharpTypeBodyDropContext = "IsCSharpTypeBodyDropContext";

		public static readonly StringName GetTextOffsetFromDropPosition = "GetTextOffsetFromDropPosition";

		public static readonly StringName GetDropLineColumn = "GetDropLineColumn";

		public static readonly StringName IsCSharpTypeBodyOffset = "IsCSharpTypeBodyOffset";

		public static readonly StringName IsTypeDeclarationBrace = "IsTypeDeclarationBrace";
	}

	public new class PropertyName : CodeEdit.PropertyName
	{
		public static readonly StringName HasPendingBackgroundWork = "HasPendingBackgroundWork";

		public static readonly StringName _completionTimer = "_completionTimer";

		public static readonly StringName _validationTimer = "_validationTimer";

		public static readonly StringName _validationGeneration = "_validationGeneration";

		public static readonly StringName _validationWorkerRunning = "_validationWorkerRunning";

		public static readonly StringName _validationSchedulerEnabled = "_validationSchedulerEnabled";

		public static readonly StringName _validationRequests = "_validationRequests";

		public static readonly StringName _validationCoalescedRequests = "_validationCoalescedRequests";

		public static readonly StringName _validationExecutedChecks = "_validationExecutedChecks";

		public static readonly StringName _validationPublishedResults = "_validationPublishedResults";

		public static readonly StringName _validationStaleResults = "_validationStaleResults";

		public static readonly StringName _validationCanceledChecks = "_validationCanceledChecks";

		public static readonly StringName _validationActiveWorkers = "_validationActiveWorkers";

		public static readonly StringName _validationPeakActiveWorkers = "_validationPeakActiveWorkers";

		public static readonly StringName _validationLastDiagnosticCount = "_validationLastDiagnosticCount";

		public static readonly StringName _validationMaxCaptureMilliseconds = "_validationMaxCaptureMilliseconds";

		public static readonly StringName _validationMaxApplyMilliseconds = "_validationMaxApplyMilliseconds";

		public static readonly StringName _completionWorkerRunning = "_completionWorkerRunning";

		public static readonly StringName _completionSchedulerEnabled = "_completionSchedulerEnabled";

		public static readonly StringName _completionDocumentRevision = "_completionDocumentRevision";

		public static readonly StringName _completionRequests = "_completionRequests";

		public static readonly StringName _completionCoalescedRequests = "_completionCoalescedRequests";

		public static readonly StringName _completionExecutedAnalyses = "_completionExecutedAnalyses";

		public static readonly StringName _completionWorkerEnteredAnalyses = "_completionWorkerEnteredAnalyses";

		public static readonly StringName _completionActiveAnalysisWorkers = "_completionActiveAnalysisWorkers";

		public static readonly StringName _completionAppliedResults = "_completionAppliedResults";

		public static readonly StringName _completionCanceledAnalyses = "_completionCanceledAnalyses";

		public static readonly StringName _completionStaleResults = "_completionStaleResults";

		public static readonly StringName _completionActiveWorkers = "_completionActiveWorkers";

		public static readonly StringName _completionPeakActiveWorkers = "_completionPeakActiveWorkers";

		public static readonly StringName _completionLastRequestedRevision = "_completionLastRequestedRevision";

		public static readonly StringName _completionLastAppliedRevision = "_completionLastAppliedRevision";

		public static readonly StringName _completionLastOptionCount = "_completionLastOptionCount";

		public static readonly StringName _completionMaxCaptureMilliseconds = "_completionMaxCaptureMilliseconds";

		public static readonly StringName _completionMaxApplyMilliseconds = "_completionMaxApplyMilliseconds";

		public static readonly StringName _completionLastAppliedPrefix = "_completionLastAppliedPrefix";

		public static readonly StringName _currentCompletionProvider = "_currentCompletionProvider";

		public static readonly StringName _csharpCompletionProvider = "_csharpCompletionProvider";

		public static readonly StringName _csharpHighlighter = "_csharpHighlighter";

		public static readonly StringName _largeDocumentSyntaxHighlightSuppressed = "_largeDocumentSyntaxHighlightSuppressed";

		public static readonly StringName _currentFileExtension = "_currentFileExtension";

		public static readonly StringName _currentDocumentPath = "_currentDocumentPath";

		public static readonly StringName _projectRoot = "_projectRoot";

		public static readonly StringName _lastIndexedSource = "_lastIndexedSource";

		public static readonly StringName _errorGutterIndex = "_errorGutterIndex";

		public static readonly StringName _lastErrorCount = "_lastErrorCount";

		public static readonly StringName _lastWarningCount = "_lastWarningCount";

		public static readonly StringName _errorIcon = "_errorIcon";

		public static readonly StringName _warningIcon = "_warningIcon";
	}

	public new class SignalName : CodeEdit.SignalName
	{
		public static readonly StringName ErrorsChanged = "ErrorsChanged";

		public static readonly StringName DiagnosticsChanged = "DiagnosticsChanged";

		public static readonly StringName ErrorClicked = "ErrorClicked";
	}

	private Godot.Timer _completionTimer;

	private Godot.Timer _validationTimer;

	private const float CompletionDelay = 0.3f;

	private const int CompletionCoalesceDelayMilliseconds = 10;

	private const float ValidationDelay = 1.5f;

	private const float ValidationDelayWithErrors = 0.5f;

	private const int ValidationCoalesceDelayMilliseconds = 10;

	private const int ValidationDiagnosticLimit = 256;

	private int _validationGeneration;

	private readonly object _validationGate = new object();

	private CancellationTokenSource _validationLifetimeCts;

	private CancellationTokenSource _activeValidationCts;

	private Task _validationWorkerTask = Task.CompletedTask;

	private ValidationRequest _pendingValidationRequest;

	private bool _validationWorkerRunning;

	private bool _validationSchedulerEnabled;

	private int _validationRequests;

	private int _validationCoalescedRequests;

	private int _validationExecutedChecks;

	private int _validationPublishedResults;

	private int _validationStaleResults;

	private int _validationCanceledChecks;

	private int _validationActiveWorkers;

	private int _validationPeakActiveWorkers;

	private int _validationLastDiagnosticCount;

	private long _validationMaxCaptureMilliseconds;

	private long _validationMaxApplyMilliseconds;

	private readonly object _completionGate = new object();

	private CancellationTokenSource _completionLifetimeCts;

	private CancellationTokenSource _activeCompletionCts;

	private CompletionRequest _activeCompletionRequest;

	private Task _completionWorkerTask = Task.CompletedTask;

	private CompletionRequest _pendingCompletionRequest;

	private bool _completionWorkerRunning;

	private bool _completionSchedulerEnabled;

	private int _completionDocumentRevision;

	private int _completionRequests;

	private int _completionCoalescedRequests;

	private int _completionExecutedAnalyses;

	private int _completionWorkerEnteredAnalyses;

	private int _completionActiveAnalysisWorkers;

	private int _completionAppliedResults;

	private int _completionCanceledAnalyses;

	private int _completionStaleResults;

	private int _completionActiveWorkers;

	private int _completionPeakActiveWorkers;

	private int _completionLastRequestedRevision;

	private int _completionLastAppliedRevision;

	private int _completionLastOptionCount;

	private long _completionMaxCaptureMilliseconds;

	private long _completionMaxApplyMilliseconds;

	private string _completionLastAppliedPrefix = "";

	private readonly HashSet<string> _completionLastAppliedInserts = new HashSet<string>(StringComparer.Ordinal);

	private XWCodeCompletionProvider _currentCompletionProvider;

	private XWCSharpCompletionProvider _csharpCompletionProvider;

	private CodeHighlighter _csharpHighlighter;

	private const int LargeDocumentSyntaxHighlightThreshold = 400000;

	private bool _largeDocumentSyntaxHighlightSuppressed;

	private string _currentFileExtension = "";

	private string _currentDocumentPath = "";

	private string _projectRoot = "";

	private string _lastIndexedSource;

	private readonly List<XWCodeErrorChecker.ErrorData> _diagnostics = new List<XWCodeErrorChecker.ErrorData>();

	private readonly System.Collections.Generic.Dictionary<int, XWCodeErrorChecker.ErrorData> _errorLines = new System.Collections.Generic.Dictionary<int, XWCodeErrorChecker.ErrorData>();

	private readonly System.Collections.Generic.Dictionary<int, XWCodeErrorChecker.ErrorData> _warningLines = new System.Collections.Generic.Dictionary<int, XWCodeErrorChecker.ErrorData>();

	private static readonly Color ErrorBackgroundColor = new Color(0.3f, 0.1f, 0.1f, 0.5f);

	private static readonly Color WarningBackgroundColor = new Color(0.3f, 0.3f, 0.1f, 0.5f);

	private int _errorGutterIndex = -1;

	private int _lastErrorCount;

	private int _lastWarningCount;

	private Texture2D _errorIcon;

	private Texture2D _warningIcon;

	private ErrorsChangedEventHandler backing_ErrorsChanged;

	private DiagnosticsChangedEventHandler backing_DiagnosticsChanged;

	private ErrorClickedEventHandler backing_ErrorClicked;

	public bool HasPendingBackgroundWork
	{
		get
		{
			Godot.Timer completionTimer = _completionTimer;
			if (completionTimer == null || completionTimer.ProcessMode != ProcessModeEnum.Disabled)
			{
				Godot.Timer completionTimer2 = _completionTimer;
				if (completionTimer2 != null && !completionTimer2.IsStopped())
				{
					goto IL_0093;
				}
			}
			Godot.Timer validationTimer = _validationTimer;
			if (validationTimer == null || validationTimer.ProcessMode != ProcessModeEnum.Disabled)
			{
				Godot.Timer validationTimer2 = _validationTimer;
				if (validationTimer2 != null && !validationTimer2.IsStopped())
				{
					goto IL_0093;
				}
			}
			if (Volatile.Read(in _validationActiveWorkers) <= 0 && GetPendingValidationRequestCount() <= 0 && Volatile.Read(in _completionActiveWorkers) <= 0)
			{
				return GetPendingCompletionRequestCount() > 0;
			}
			goto IL_0093;
			IL_0093:
			return true;
		}
	}

	public event ErrorsChangedEventHandler ErrorsChanged
	{
		add
		{
			backing_ErrorsChanged = (ErrorsChangedEventHandler)Delegate.Combine(backing_ErrorsChanged, value);
		}
		remove
		{
			backing_ErrorsChanged = (ErrorsChangedEventHandler)Delegate.Remove(backing_ErrorsChanged, value);
		}
	}

	public event DiagnosticsChangedEventHandler DiagnosticsChanged
	{
		add
		{
			backing_DiagnosticsChanged = (DiagnosticsChangedEventHandler)Delegate.Combine(backing_DiagnosticsChanged, value);
		}
		remove
		{
			backing_DiagnosticsChanged = (DiagnosticsChangedEventHandler)Delegate.Remove(backing_DiagnosticsChanged, value);
		}
	}

	public event ErrorClickedEventHandler ErrorClicked
	{
		add
		{
			backing_ErrorClicked = (ErrorClickedEventHandler)Delegate.Combine(backing_ErrorClicked, value);
		}
		remove
		{
			backing_ErrorClicked = (ErrorClickedEventHandler)Delegate.Remove(backing_ErrorClicked, value);
		}
	}

	public override void _Ready()
	{
		_errorIcon = LoadIconSafe("res://addons/ModEditor/Icons/StatusError.svg");
		_warningIcon = LoadIconSafe("res://addons/ModEditor/Icons/StatusWarning.svg");
		_csharpCompletionProvider = new XWCSharpCompletionProvider(this);
		_csharpHighlighter = new XWCSharpHighlighter();
		CreateCompletionTimer();
		CreateValidationTimer();
		SetupErrorGutter();
		TextChanged += OnTextChanged;
		CaretChanged += OnCaretChanged;
		CodeCompletionEnabled = true;
		lock (_validationGate)
		{
			_validationSchedulerEnabled = true;
			_validationLifetimeCts = new CancellationTokenSource();
		}
		lock (_completionGate)
		{
			_completionSchedulerEnabled = true;
			_completionLifetimeCts = new CancellationTokenSource();
		}
	}

	private static Texture2D LoadIconSafe(string path)
	{
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse));
	}

	private void SetupErrorGutter()
	{
		_errorGutterIndex = GetGutterCount();
		AddGutter(1);
		SetGutterName(_errorGutterIndex, "error");
		SetGutterDraw(_errorGutterIndex, draw: true);
		SetGutterOverwritable(_errorGutterIndex, overwritable: false);
		SetGutterWidth(_errorGutterIndex, 16);
	}

	private void CreateCompletionTimer()
	{
		_completionTimer = new Godot.Timer
		{
			OneShot = true,
			WaitTime = 0.30000001192092896
		};
		_completionTimer.Timeout += OnCompletionTimerTimeout;
		AddChild(_completionTimer, forceReadableName: false, InternalMode.Disabled);
	}

	private void CreateValidationTimer()
	{
		_validationTimer = new Godot.Timer
		{
			OneShot = true,
			WaitTime = 1.5
		};
		_validationTimer.Timeout += OnValidationTimerTimeout;
		AddChild(_validationTimer, forceReadableName: false, InternalMode.Disabled);
	}

	public void SetFileExtension(string extension)
	{
		_validationGeneration++;
		InvalidateCompletionWork(disableScheduler: false);
		_completionTimer?.Stop();
		_validationTimer?.Stop();
		ClearPendingValidationRequest();
		CancelActiveValidationCheck();
		_currentFileExtension = extension?.ToLower() ?? "";
		UpdateCompletionProvider();
		UpdateSyntaxHighlighter(_currentFileExtension);
	}

	public void SuspendBackgroundWork()
	{
		_completionTimer?.Stop();
		_validationTimer?.Stop();
		if (_completionTimer != null)
		{
			_completionTimer.ProcessMode = ProcessModeEnum.Disabled;
		}
		if (_validationTimer != null)
		{
			_validationTimer.ProcessMode = ProcessModeEnum.Disabled;
		}
		CancelCodeCompletion();
		InvalidateCompletionWork(disableScheduler: true);
		CancelValidationWork(disableScheduler: true);
	}

	public void ResumeBackgroundWork()
	{
		if (_completionTimer != null)
		{
			_completionTimer.ProcessMode = ProcessModeEnum.Inherit;
		}
		if (_validationTimer != null)
		{
			_validationTimer.ProcessMode = ProcessModeEnum.Inherit;
		}
		lock (_validationGate)
		{
			_validationSchedulerEnabled = true;
			if (_validationLifetimeCts == null || _validationLifetimeCts.IsCancellationRequested)
			{
				_validationLifetimeCts = new CancellationTokenSource();
			}
			StartValidationWorkerLockedIfNeeded();
		}
		lock (_completionGate)
		{
			_completionSchedulerEnabled = true;
			if (_completionLifetimeCts == null || _completionLifetimeCts.IsCancellationRequested)
			{
				_completionLifetimeCts = new CancellationTokenSource();
			}
			StartCompletionWorkerLockedIfNeeded();
		}
	}

	public override void _ExitTree()
	{
		TextChanged -= OnTextChanged;
		CaretChanged -= OnCaretChanged;
		if (_completionTimer != null)
		{
			_completionTimer.Stop();
			_completionTimer.Timeout -= OnCompletionTimerTimeout;
		}
		if (_validationTimer != null)
		{
			_validationTimer.Stop();
			_validationTimer.Timeout -= OnValidationTimerTimeout;
		}
		InvalidateCompletionWork(disableScheduler: true);
		CancelValidationWork(disableScheduler: true);
	}

	public void SetDocumentPath(string path)
	{
		InvalidateCompletionWork(disableScheduler: false);
		_currentDocumentPath = path ?? "";
		if (_currentDocumentPath.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || _currentDocumentPath.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			_currentDocumentPath = ProjectSettings.GlobalizePath(_currentDocumentPath);
		}
		_lastIndexedSource = null;
	}

	public void SetProjectRoot(string path)
	{
		string projectRoot = _projectRoot;
		_projectRoot = path ?? "";
		if (_projectRoot.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || _projectRoot.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			_projectRoot = ProjectSettings.GlobalizePath(_projectRoot);
		}
		if (!string.Equals(projectRoot, _projectRoot, StringComparison.OrdinalIgnoreCase))
		{
			_lastIndexedSource = null;
			InvalidateCompletionWork(disableScheduler: false);
		}
		if (!string.IsNullOrWhiteSpace(_projectRoot))
		{
			XWCSharpCodeModel.RequestBackgroundWarmup(_projectRoot);
		}
	}

	public string GetDocumentPath()
	{
		return _currentDocumentPath;
	}

	public string GetProjectRoot()
	{
		return _projectRoot;
	}

	public int[] GetBreakpointLines()
	{
		return GetBreakpointedLines();
	}

	public void ToggleBreakpoint(int line)
	{
		if (line >= 0 && line < GetLineCount())
		{
			SetLineAsBreakpoint(line, !IsLineBreakpointed(line));
		}
	}

	public void RestoreBreakpointLines(IEnumerable<int> lines)
	{
		ClearBreakpointedLines();
		if (lines == null)
		{
			return;
		}
		foreach (int line in lines)
		{
			if (line >= 0 && line < GetLineCount())
			{
				SetLineAsBreakpoint(line, breakpointed: true);
			}
		}
	}

	private void UpdateCompletionProvider()
	{
		if (_currentFileExtension == "cs")
		{
			_currentCompletionProvider = _csharpCompletionProvider;
			CodeCompletionEnabled = true;
		}
		else
		{
			_currentCompletionProvider = null;
			CodeCompletionEnabled = false;
		}
	}

	private void UpdateSyntaxHighlighter(string extension)
	{
		if (_largeDocumentSyntaxHighlightSuppressed)
		{
			SyntaxHighlighter = null;
		}
		else if (extension == "cs")
		{
			SyntaxHighlighter = _csharpHighlighter;
		}
		else
		{
			SyntaxHighlighter = null;
		}
	}

	public void CheckSyntaxErrors()
	{
		if (string.IsNullOrEmpty(_currentFileExtension))
		{
			SetExternalDiagnostics(new List<XWCodeErrorChecker.ErrorData>());
		}
		else
		{
			RequestValidationNow();
		}
	}

	public void SetExternalDiagnostics(List<XWCodeErrorChecker.ErrorData> diagnostics)
	{
		_validationGeneration++;
		ClearPendingValidationRequest();
		CancelActiveValidationCheck();
		ReplaceDiagnostics(diagnostics);
	}

	private void ReplaceDiagnostics(List<XWCodeErrorChecker.ErrorData> diagnostics)
	{
		ClearAllErrors();
		ApplyDiagnostics(diagnostics);
	}

	private void ApplyDiagnostics(List<XWCodeErrorChecker.ErrorData> diagnostics)
	{
		if (diagnostics == null)
		{
			diagnostics = new List<XWCodeErrorChecker.ErrorData>();
		}
		foreach (XWCodeErrorChecker.ErrorData diagnostic in diagnostics)
		{
			AddError(diagnostic);
		}
		int errorCount = GetErrorCount();
		int warningCount = GetWarningCount();
		if (errorCount != _lastErrorCount || warningCount != _lastWarningCount)
		{
			_lastErrorCount = errorCount;
			_lastWarningCount = warningCount;
			EmitSignal(SignalName.ErrorsChanged, errorCount, warningCount);
		}
		EmitSignal(SignalName.DiagnosticsChanged);
		UpdateValidationDelay();
	}

	private void UpdateValidationDelay()
	{
		_validationTimer.WaitTime = ((_lastErrorCount > 0) ? 0.5f : 1.5f);
	}

	private void OnCompletionTimerTimeout()
	{
		if (CodeCompletionEnabled && _currentCompletionProvider == _csharpCompletionProvider)
		{
			string text = Text ?? "";
			if (_currentFileExtension == "cs" && !string.IsNullOrEmpty(_currentDocumentPath) && !string.Equals(text, _lastIndexedSource, StringComparison.Ordinal))
			{
				PublishCurrentSourceToIndexAsync(_currentDocumentPath, text, _completionDocumentRevision);
			}
			QueueCompletionRequest(text);
		}
	}

	private void OnValidationTimerTimeout()
	{
		QueueValidationRequest();
	}

	private void OnTextChanged()
	{
		_validationGeneration++;
		InvalidateCompletionWork(disableScheduler: false);
		CancelCodeCompletion();
		_completionTimer.Stop();
		if (CodeCompletionEnabled)
		{
			_completionTimer.Start();
		}
		_validationTimer.Start();
	}

	private void OnCaretChanged()
	{
		int caretLine = GetCaretLine();
		int caretColumn = GetCaretColumn();
		lock (_completionGate)
		{
			CompletionRequest completionRequest = _pendingCompletionRequest ?? _activeCompletionRequest;
			if (completionRequest == null || (completionRequest.CaretLine == caretLine && completionRequest.CaretColumn == caretColumn))
			{
				return;
			}
		}
		CancelCodeCompletion();
		InvalidateCompletionWork(disableScheduler: false);
	}

	public void RequestCompletionNow()
	{
		_completionTimer?.Stop();
		if (CodeCompletionEnabled && _currentCompletionProvider == _csharpCompletionProvider)
		{
			QueueCompletionRequest(Text ?? "");
		}
	}

	private void QueueCompletionRequest(string source)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		CompletionRequest completionRequest = new CompletionRequest
		{
			Source = (source ?? ""),
			DocumentPath = _currentDocumentPath,
			CaretLine = GetCaretLine(),
			CaretColumn = GetCaretColumn(),
			Revision = _completionDocumentRevision
		};
		stopwatch.Stop();
		UpdateMax(ref _completionMaxCaptureMilliseconds, stopwatch.ElapsedMilliseconds);
		Interlocked.Increment(ref _completionRequests);
		Volatile.Write(ref _completionLastRequestedRevision, completionRequest.Revision);
		lock (_completionGate)
		{
			if (_completionSchedulerEnabled)
			{
				if (_pendingCompletionRequest != null || _completionWorkerRunning)
				{
					Interlocked.Increment(ref _completionCoalescedRequests);
				}
				_pendingCompletionRequest = completionRequest;
				_activeCompletionCts?.Cancel();
				StartCompletionWorkerLockedIfNeeded();
			}
		}
	}

	private void StartCompletionWorkerLockedIfNeeded()
	{
		if (_completionSchedulerEnabled && !_completionWorkerRunning && _pendingCompletionRequest != null)
		{
			if (_completionLifetimeCts == null || _completionLifetimeCts.IsCancellationRequested)
			{
				_completionLifetimeCts = new CancellationTokenSource();
			}
			_completionWorkerRunning = true;
			int value = Interlocked.Increment(ref _completionActiveWorkers);
			UpdatePeak(ref _completionPeakActiveWorkers, value);
			_completionWorkerTask = RunCompletionWorkerAsync(_completionLifetimeCts.Token);
		}
	}

	private async Task RunCompletionWorkerAsync(CancellationToken lifetimeToken)
	{
		_ = 1;
		try
		{
			while (true)
			{
				await Task.Delay(10, lifetimeToken);
				CompletionRequest request;
				CancellationTokenSource requestCts;
				lock (_completionGate)
				{
					if (!_completionSchedulerEnabled || _pendingCompletionRequest == null)
					{
						break;
					}
					request = _pendingCompletionRequest;
					_pendingCompletionRequest = null;
					requestCts = (_activeCompletionCts = CancellationTokenSource.CreateLinkedTokenSource(_completionLifetimeCts.Token));
					_activeCompletionRequest = request;
				}
				XWCSharpCompletionProvider.CompletionAnalysis completionAnalysis;
				try
				{
					Interlocked.Increment(ref _completionExecutedAnalyses);
					completionAnalysis = await _csharpCompletionProvider.AnalyzeCompletionAsync(request.Source, request.DocumentPath, request.CaretLine, request.CaretColumn, OnCompletionAnalysisWorkerEntered, OnCompletionAnalysisWorkerExited, requestCts.Token);
				}
				catch (OperationCanceledException)
				{
					Interlocked.Increment(ref _completionCanceledAnalyses);
					continue;
				}
				catch (Exception ex2)
				{
					Interlocked.Increment(ref _completionStaleResults);
					if (GodotObject.IsInstanceValid(this))
					{
						GD.PushError("C# completion analysis failed: " + ex2.Message);
					}
					continue;
				}
				finally
				{
					lock (_completionGate)
					{
						if (_activeCompletionCts == requestCts)
						{
							_activeCompletionCts = null;
							_activeCompletionRequest = null;
						}
					}
					requestCts.Dispose();
				}
				if (!GodotObject.IsInstanceValid(this) || request.Revision != _completionDocumentRevision || !string.Equals(request.DocumentPath, _currentDocumentPath, StringComparison.Ordinal) || request.CaretLine != GetCaretLine() || request.CaretColumn != GetCaretColumn() || !CodeCompletionEnabled || _currentCompletionProvider != _csharpCompletionProvider)
				{
					Interlocked.Increment(ref _completionStaleResults);
					continue;
				}
				Stopwatch stopwatch = Stopwatch.StartNew();
				_csharpCompletionProvider.ApplyCompletion(completionAnalysis);
				stopwatch.Stop();
				UpdateMax(ref _completionMaxApplyMilliseconds, stopwatch.ElapsedMilliseconds);
				Interlocked.Increment(ref _completionAppliedResults);
				Volatile.Write(ref _completionLastAppliedRevision, request.Revision);
				Volatile.Write(ref _completionLastOptionCount, completionAnalysis.Options.Count);
				_completionLastAppliedPrefix = completionAnalysis.Prefix;
				_completionLastAppliedInserts.Clear();
				foreach (XWCSharpCompletionProvider.CompletionOptionData option in completionAnalysis.Options)
				{
					_completionLastAppliedInserts.Add(option.Insert);
				}
				request = null;
				requestCts = null;
			}
		}
		catch (OperationCanceledException)
		{
			Interlocked.Increment(ref _completionCanceledAnalyses);
		}
		finally
		{
			Interlocked.Decrement(ref _completionActiveWorkers);
			lock (_completionGate)
			{
				_completionWorkerRunning = false;
				_completionWorkerTask = Task.CompletedTask;
				StartCompletionWorkerLockedIfNeeded();
			}
		}
	}

	private async Task PublishCurrentSourceToIndexAsync(string path, string source, int revision)
	{
		await XWCSharpCodeModel.RequestSourceUpdate(path, source);
		string key = NormalizeIndexPath(path);
		XWCSharpProjectIndex.Snapshot snapshot = XWCSharpProjectIndex.GetSnapshot();
		if (GodotObject.IsInstanceValid(this) && revision == _completionDocumentRevision && string.Equals(path, _currentDocumentPath, StringComparison.Ordinal) && snapshot.Documents.TryGetValue(key, out var value) && string.Equals(value, source, StringComparison.Ordinal))
		{
			_lastIndexedSource = source;
		}
	}

	private static string NormalizeIndexPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		try
		{
			return Path.GetFullPath(path).Replace('\\', '/');
		}
		catch
		{
			return path.Replace('\\', '/');
		}
	}

	private void InvalidateCompletionWork(bool disableScheduler)
	{
		Interlocked.Increment(ref _completionDocumentRevision);
		CancellationTokenSource cancellation = null;
		Task workerTask = Task.CompletedTask;
		CancellationTokenSource activeCompletionCts;
		lock (_completionGate)
		{
			if (disableScheduler)
			{
				_completionSchedulerEnabled = false;
				cancellation = _completionLifetimeCts;
				_completionLifetimeCts = null;
				workerTask = _completionWorkerTask;
			}
			_pendingCompletionRequest = null;
			activeCompletionCts = _activeCompletionCts;
		}
		activeCompletionCts?.Cancel();
		CancelAndDisposeWhenComplete(cancellation, workerTask);
	}

	private int GetPendingCompletionRequestCount()
	{
		lock (_completionGate)
		{
			return (_pendingCompletionRequest != null) ? 1 : 0;
		}
	}

	private void OnCompletionAnalysisWorkerEntered()
	{
		Interlocked.Increment(ref _completionWorkerEnteredAnalyses);
		Interlocked.Increment(ref _completionActiveAnalysisWorkers);
	}

	private void OnCompletionAnalysisWorkerExited()
	{
		Interlocked.Decrement(ref _completionActiveAnalysisWorkers);
	}

	private static void UpdatePeak(ref int target, int value)
	{
		int num;
		do
		{
			num = Volatile.Read(in target);
		}
		while (value > num && Interlocked.CompareExchange(ref target, value, num) != num);
	}

	private static void UpdateMax(ref long target, long value)
	{
		long num;
		do
		{
			num = Interlocked.Read(in target);
		}
		while (value > num && Interlocked.CompareExchange(ref target, value, num) != num);
	}

	public CompletionMetrics GetCompletionMetrics()
	{
		return new CompletionMetrics(Volatile.Read(in _completionRequests), Volatile.Read(in _completionCoalescedRequests), Volatile.Read(in _completionExecutedAnalyses), Volatile.Read(in _completionWorkerEnteredAnalyses), Volatile.Read(in _completionActiveAnalysisWorkers), Volatile.Read(in _completionAppliedResults), Volatile.Read(in _completionCanceledAnalyses), Volatile.Read(in _completionStaleResults), Volatile.Read(in _completionActiveWorkers), Volatile.Read(in _completionPeakActiveWorkers), GetPendingCompletionRequestCount(), Volatile.Read(in _completionLastRequestedRevision), Volatile.Read(in _completionLastAppliedRevision), Volatile.Read(in _completionLastOptionCount), Interlocked.Read(in _completionMaxCaptureMilliseconds), Interlocked.Read(in _completionMaxApplyMilliseconds), _completionLastAppliedPrefix);
	}

	public bool LastCompletionContainsOption(string insert)
	{
		if (!string.IsNullOrEmpty(insert))
		{
			return _completionLastAppliedInserts.Contains(insert);
		}
		return false;
	}

	public bool PublishedCompletionContainsOption(string insert)
	{
		if (string.IsNullOrEmpty(insert))
		{
			return false;
		}
		foreach (Dictionary codeCompletionOption in GetCodeCompletionOptions())
		{
			if (codeCompletionOption.TryGetValue("insert_text", out var value) && string.Equals(value.AsString(), insert, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	public bool ResetCompletionMetricsForProbe()
	{
		CompletionMetrics completionMetrics = GetCompletionMetrics();
		if (completionMetrics.ActiveWorkers != 0 || completionMetrics.PendingRequests != 0)
		{
			return false;
		}
		Interlocked.Exchange(ref _completionRequests, 0);
		Interlocked.Exchange(ref _completionCoalescedRequests, 0);
		Interlocked.Exchange(ref _completionExecutedAnalyses, 0);
		Interlocked.Exchange(ref _completionWorkerEnteredAnalyses, 0);
		Interlocked.Exchange(ref _completionActiveAnalysisWorkers, 0);
		Interlocked.Exchange(ref _completionAppliedResults, 0);
		Interlocked.Exchange(ref _completionCanceledAnalyses, 0);
		Interlocked.Exchange(ref _completionStaleResults, 0);
		Interlocked.Exchange(ref _completionPeakActiveWorkers, 0);
		Interlocked.Exchange(ref _completionLastOptionCount, 0);
		Interlocked.Exchange(ref _completionMaxCaptureMilliseconds, 0L);
		Interlocked.Exchange(ref _completionMaxApplyMilliseconds, 0L);
		_completionLastAppliedPrefix = "";
		_completionLastAppliedInserts.Clear();
		return true;
	}

	public void RequestValidationNow()
	{
		_validationGeneration++;
		_validationTimer.Stop();
		QueueValidationRequest();
	}

	private void QueueValidationRequest()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		ValidationRequest pendingValidationRequest = new ValidationRequest
		{
			Code = (Text ?? ""),
			Extension = _currentFileExtension,
			Generation = _validationGeneration
		};
		stopwatch.Stop();
		UpdateMax(ref _validationMaxCaptureMilliseconds, stopwatch.ElapsedMilliseconds);
		Interlocked.Increment(ref _validationRequests);
		lock (_validationGate)
		{
			if (_validationSchedulerEnabled)
			{
				if (_pendingValidationRequest != null || _validationWorkerRunning)
				{
					Interlocked.Increment(ref _validationCoalescedRequests);
				}
				_pendingValidationRequest = pendingValidationRequest;
				_activeValidationCts?.Cancel();
				StartValidationWorkerLockedIfNeeded();
			}
		}
	}

	private void StartValidationWorkerLockedIfNeeded()
	{
		if (_validationSchedulerEnabled && !_validationWorkerRunning && _pendingValidationRequest != null)
		{
			if (_validationLifetimeCts == null || _validationLifetimeCts.IsCancellationRequested)
			{
				_validationLifetimeCts = new CancellationTokenSource();
			}
			_validationWorkerRunning = true;
			int activeWorkers = Interlocked.Increment(ref _validationActiveWorkers);
			UpdatePeakActiveWorkers(activeWorkers);
			_validationWorkerTask = RunValidationWorkerAsync(_validationLifetimeCts.Token);
		}
	}

	private async Task RunValidationWorkerAsync(CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			while (true)
			{
				await Task.Delay(10, cancellationToken);
				ValidationRequest request;
				lock (_validationGate)
				{
					request = _pendingValidationRequest;
					_pendingValidationRequest = null;
				}
				if (request == null)
				{
					break;
				}
				CancellationTokenSource requestCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
				try
				{
					lock (_validationGate)
					{
						_activeValidationCts = requestCts;
					}
					List<XWCodeErrorChecker.ErrorData> list;
					try
					{
						Interlocked.Increment(ref _validationExecutedChecks);
						XWCodeErrorCheckerRegistry.Init();
						list = await Task.Run(() => RunValidationCheckBelowNormalPriority(request.Code, request.Extension, requestCts.Token), requestCts.Token);
					}
					catch (OperationCanceledException)
					{
						Interlocked.Increment(ref _validationCanceledChecks);
						goto end_IL_00f6;
					}
					catch (Exception ex2)
					{
						list = new List<XWCodeErrorChecker.ErrorData>
						{
							new XWCodeErrorChecker.ErrorData
							{
								Line = 0,
								Column = 0,
								SeverityLevel = XWCodeErrorChecker.Severity.Error,
								Code = "VALIDATION_WORKER_FAILED",
								Message = "C# validation failed: " + ex2.Message
							}
						};
					}
					finally
					{
						lock (_validationGate)
						{
							if (_activeValidationCts == requestCts)
							{
								_activeValidationCts = null;
							}
						}
					}
					cancellationToken.ThrowIfCancellationRequested();
					if (!GodotObject.IsInstanceValid(this))
					{
						break;
					}
					if (request.Generation != _validationGeneration)
					{
						Interlocked.Increment(ref _validationStaleResults);
						continue;
					}
					if (list.Count > 256)
					{
						list = list.GetRange(0, 256);
					}
					Stopwatch stopwatch = Stopwatch.StartNew();
					ReplaceDiagnostics(list);
					stopwatch.Stop();
					UpdateMax(ref _validationMaxApplyMilliseconds, stopwatch.ElapsedMilliseconds);
					Volatile.Write(ref _validationLastDiagnosticCount, list.Count);
					Interlocked.Increment(ref _validationPublishedResults);
					end_IL_00f6:;
				}
				finally
				{
					if (requestCts != null)
					{
						((IDisposable)requestCts).Dispose();
					}
				}
			}
		}
		catch (OperationCanceledException)
		{
			Interlocked.Increment(ref _validationCanceledChecks);
		}
		finally
		{
			Interlocked.Decrement(ref _validationActiveWorkers);
			lock (_validationGate)
			{
				_validationWorkerRunning = false;
				_validationWorkerTask = Task.CompletedTask;
				StartValidationWorkerLockedIfNeeded();
			}
		}
	}

	private void CancelValidationWork(bool disableScheduler)
	{
		_validationGeneration++;
		CancellationTokenSource cancellation = null;
		Task workerTask = Task.CompletedTask;
		lock (_validationGate)
		{
			if (disableScheduler)
			{
				_validationSchedulerEnabled = false;
				cancellation = _validationLifetimeCts;
				_validationLifetimeCts = null;
				workerTask = _validationWorkerTask;
			}
			_pendingValidationRequest = null;
		}
		CancelAndDisposeWhenComplete(cancellation, workerTask);
	}

	private static void CancelAndDisposeWhenComplete(CancellationTokenSource cancellation, Task workerTask)
	{
		if (cancellation == null)
		{
			return;
		}
		cancellation.Cancel();
		if (workerTask == null || workerTask.IsCompleted)
		{
			cancellation.Dispose();
			return;
		}
		workerTask.ContinueWith((Task _) =>
		{
			cancellation.Dispose();
		}, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
	}

	private void ClearPendingValidationRequest()
	{
		lock (_validationGate)
		{
			_pendingValidationRequest = null;
		}
	}

	private void CancelActiveValidationCheck()
	{
		lock (_validationGate)
		{
			_activeValidationCts?.Cancel();
		}
	}

	private static List<XWCodeErrorChecker.ErrorData> RunValidationCheckBelowNormalPriority(string code, string extension, CancellationToken cancellationToken)
	{
		Thread currentThread = Thread.CurrentThread;
		ThreadPriority priority = currentThread.Priority;
		try
		{
			currentThread.Priority = ThreadPriority.BelowNormal;
			return XWCodeErrorCheckerRegistry.CheckCode(code, extension, cancellationToken);
		}
		finally
		{
			currentThread.Priority = priority;
		}
	}

	private int GetPendingValidationRequestCount()
	{
		lock (_validationGate)
		{
			return (_pendingValidationRequest != null) ? 1 : 0;
		}
	}

	private void UpdatePeakActiveWorkers(int activeWorkers)
	{
		int num;
		do
		{
			num = Volatile.Read(in _validationPeakActiveWorkers);
		}
		while (activeWorkers > num && Interlocked.CompareExchange(ref _validationPeakActiveWorkers, activeWorkers, num) != num);
	}

	public ValidationMetrics GetValidationMetrics()
	{
		return new ValidationMetrics(Volatile.Read(in _validationRequests), Volatile.Read(in _validationCoalescedRequests), Volatile.Read(in _validationExecutedChecks), Volatile.Read(in _validationPublishedResults), Volatile.Read(in _validationStaleResults), Volatile.Read(in _validationCanceledChecks), Volatile.Read(in _validationActiveWorkers), Volatile.Read(in _validationPeakActiveWorkers), GetPendingValidationRequestCount(), Volatile.Read(in _validationLastDiagnosticCount), Interlocked.Read(in _validationMaxCaptureMilliseconds), Interlocked.Read(in _validationMaxApplyMilliseconds));
	}

	public bool ResetValidationMetricsForProbe()
	{
		ValidationMetrics validationMetrics = GetValidationMetrics();
		if (validationMetrics.ActiveWorkers != 0 || validationMetrics.PendingRequests != 0)
		{
			return false;
		}
		Interlocked.Exchange(ref _validationRequests, 0);
		Interlocked.Exchange(ref _validationCoalescedRequests, 0);
		Interlocked.Exchange(ref _validationExecutedChecks, 0);
		Interlocked.Exchange(ref _validationPublishedResults, 0);
		Interlocked.Exchange(ref _validationStaleResults, 0);
		Interlocked.Exchange(ref _validationCanceledChecks, 0);
		Interlocked.Exchange(ref _validationPeakActiveWorkers, 0);
		Interlocked.Exchange(ref _validationLastDiagnosticCount, 0);
		Interlocked.Exchange(ref _validationMaxCaptureMilliseconds, 0L);
		Interlocked.Exchange(ref _validationMaxApplyMilliseconds, 0L);
		return true;
	}

	private void AddError(int line, int column, string message, int severity, string code)
	{
		XWCodeErrorChecker.ErrorData data = new XWCodeErrorChecker.ErrorData
		{
			Line = line,
			Column = column,
			Message = message,
			SeverityLevel = (XWCodeErrorChecker.Severity)severity,
			Code = code
		};
		AddError(data);
	}

	private void AddError(XWCodeErrorChecker.ErrorData data)
	{
		_diagnostics.Add(data);
		int line = data.Line;
		Color color;
		if (data.SeverityLevel == XWCodeErrorChecker.Severity.Error)
		{
			_errorLines[line] = data;
			color = ErrorBackgroundColor;
			SetErrorGutterIcon(line, isError: true);
		}
		else
		{
			_warningLines[line] = data;
			color = WarningBackgroundColor;
			SetErrorGutterIcon(line, isError: false);
		}
		if (line >= 0 && line < GetLineCount())
		{
			SetLineBackgroundColor(line, color);
		}
	}

	private void SetErrorGutterIcon(int line, bool isError)
	{
		if (_errorGutterIndex >= 0 && line >= 0 && line < GetLineCount())
		{
			SetLineGutterIcon(line, _errorGutterIndex, isError ? _errorIcon : _warningIcon);
		}
	}

	private void ClearErrorGutterIcon(int line)
	{
		if (_errorGutterIndex >= 0 && line >= 0 && line < GetLineCount())
		{
			SetLineGutterIcon(line, _errorGutterIndex, null);
		}
	}

	public void ClearAllErrors()
	{
		foreach (int key in _errorLines.Keys)
		{
			if (key >= 0 && key < GetLineCount())
			{
				SetLineBackgroundColor(key, new Color(0f, 0f, 0f, 0f));
				ClearErrorGutterIcon(key);
			}
		}
		foreach (int key2 in _warningLines.Keys)
		{
			if (key2 >= 0 && key2 < GetLineCount())
			{
				SetLineBackgroundColor(key2, new Color(0f, 0f, 0f, 0f));
				ClearErrorGutterIcon(key2);
			}
		}
		_errorLines.Clear();
		_warningLines.Clear();
		_diagnostics.Clear();
	}

	public int GetErrorCount()
	{
		int num = 0;
		foreach (XWCodeErrorChecker.ErrorData diagnostic in _diagnostics)
		{
			if (diagnostic.SeverityLevel == XWCodeErrorChecker.Severity.Error)
			{
				num++;
			}
		}
		return num;
	}

	public int GetWarningCount()
	{
		int num = 0;
		foreach (XWCodeErrorChecker.ErrorData diagnostic in _diagnostics)
		{
			if (diagnostic.SeverityLevel == XWCodeErrorChecker.Severity.Warning)
			{
				num++;
			}
		}
		return num;
	}

	public bool HasAnyErrors()
	{
		return _diagnostics.Count > 0;
	}

	public XWCodeErrorChecker.ErrorData GetFirstError()
	{
		XWCodeErrorChecker.ErrorData errorData = null;
		foreach (XWCodeErrorChecker.ErrorData diagnostic in _diagnostics)
		{
			if (diagnostic.SeverityLevel == XWCodeErrorChecker.Severity.Error && (errorData == null || diagnostic.Line < errorData.Line))
			{
				errorData = diagnostic;
			}
		}
		return errorData;
	}

	public XWCodeErrorChecker.ErrorData GetFirstWarning()
	{
		XWCodeErrorChecker.ErrorData errorData = null;
		foreach (XWCodeErrorChecker.ErrorData diagnostic in _diagnostics)
		{
			if (diagnostic.SeverityLevel == XWCodeErrorChecker.Severity.Warning && (errorData == null || diagnostic.Line < errorData.Line))
			{
				errorData = diagnostic;
			}
		}
		return errorData;
	}

	public List<XWCodeErrorChecker.ErrorData> GetAllErrors()
	{
		List<XWCodeErrorChecker.ErrorData> list = new List<XWCodeErrorChecker.ErrorData>(_diagnostics);
		list.Sort(CompareDiagnostics);
		return list;
	}

	public List<XWCodeErrorChecker.ErrorData> GetOnlyErrors()
	{
		List<XWCodeErrorChecker.ErrorData> list = new List<XWCodeErrorChecker.ErrorData>();
		foreach (XWCodeErrorChecker.ErrorData diagnostic in _diagnostics)
		{
			if (diagnostic.SeverityLevel == XWCodeErrorChecker.Severity.Error)
			{
				list.Add(diagnostic);
			}
		}
		list.Sort(CompareDiagnostics);
		return list;
	}

	public List<XWCodeErrorChecker.ErrorData> GetOnlyWarnings()
	{
		List<XWCodeErrorChecker.ErrorData> list = new List<XWCodeErrorChecker.ErrorData>();
		foreach (XWCodeErrorChecker.ErrorData diagnostic in _diagnostics)
		{
			if (diagnostic.SeverityLevel == XWCodeErrorChecker.Severity.Warning)
			{
				list.Add(diagnostic);
			}
		}
		list.Sort(CompareDiagnostics);
		return list;
	}

	private static int CompareDiagnostics(XWCodeErrorChecker.ErrorData a, XWCodeErrorChecker.ErrorData b)
	{
		int num = a.Line.CompareTo(b.Line);
		if (num != 0)
		{
			return num;
		}
		int num2 = a.Column.CompareTo(b.Column);
		if (num2 != 0)
		{
			return num2;
		}
		return string.Compare(a.Code, b.Code, StringComparison.Ordinal);
	}

	public void JumpToError(XWCodeErrorChecker.ErrorData errorData)
	{
		if (errorData != null)
		{
			int line = errorData.Line;
			int column = errorData.Column;
			if (line >= 0 && line < GetLineCount())
			{
				column = Mathf.Clamp(column, 0, GetLine(line).Length);
				SetCaretLine(line);
				SetCaretColumn(column);
				CenterViewportToCaret();
				EmitSignal(SignalName.ErrorClicked, line, column);
			}
		}
	}

	public string GetAllText()
	{
		return Text;
	}

	public void SetAllText(string text)
	{
		if (Editable)
		{
			LoadDocumentText(text);
		}
	}

	public void LoadDocumentText(string text)
	{
		string text2 = text ?? "";
		bool flag = text2.Length >= 400000;
		if (flag && !_largeDocumentSyntaxHighlightSuppressed)
		{
			SyntaxHighlighter = null;
		}
		_largeDocumentSyntaxHighlightSuppressed = flag;
		Text = text2;
		if (!flag)
		{
			UpdateSyntaxHighlighter(_currentFileExtension);
		}
	}

	public void InsertAtCursor(string text)
	{
		if (Editable)
		{
			InsertTextAtCaret(text);
		}
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		if (!Editable)
		{
			return false;
		}
		List<string> list = XWFileSystemDropHelper.ExtractDropPaths(data);
		bool preferFieldDeclaration = IsCSharpTypeBodyDropContext(atPosition);
		if (list.Count > 0)
		{
			return !string.IsNullOrWhiteSpace(XWScriptResourceDropCodeGenerator.BuildSnippetForPaths(list, _currentFileExtension, preferFieldDeclaration));
		}
		return false;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		if (Editable)
		{
			List<string> list = XWFileSystemDropHelper.ExtractDropPaths(data);
			bool preferFieldDeclaration = IsCSharpTypeBodyDropContext(atPosition);
			string text = XWScriptResourceDropCodeGenerator.BuildSnippetForPaths(list, _currentFileExtension, preferFieldDeclaration);
			if (!string.IsNullOrWhiteSpace(text))
			{
				SetCaretFromDropPosition(atPosition);
				InsertTextAtCaret(text);
				XWEditorInterface.Instance?.AddOutputMessage($"脚本: 从资源拖拽生成代码 ({list.Count})", XWOutputPanel.MessageType.Editor);
			}
		}
	}

	private void SetCaretFromDropPosition(Vector2 atPosition)
	{
		if (GetLineCount() > 0)
		{
			Vector2I dropLineColumn = GetDropLineColumn(atPosition);
			SetCaretLine(dropLineColumn.X);
			SetCaretColumn(dropLineColumn.Y);
		}
	}

	private bool IsCSharpTypeBodyDropContext(Vector2 atPosition)
	{
		if (_currentFileExtension != "cs" || GetLineCount() <= 0)
		{
			return false;
		}
		int textOffsetFromDropPosition = GetTextOffsetFromDropPosition(atPosition);
		return IsCSharpTypeBodyOffset(Text ?? "", textOffsetFromDropPosition);
	}

	private int GetTextOffsetFromDropPosition(Vector2 atPosition)
	{
		Vector2I dropLineColumn = GetDropLineColumn(atPosition);
		int num = 0;
		for (int i = 0; i < dropLineColumn.X; i++)
		{
			num += GetLine(i).Length + 1;
		}
		return num + dropLineColumn.Y;
	}

	private Vector2I GetDropLineColumn(Vector2 atPosition)
	{
		Vector2I lineColumnAtPos = GetLineColumnAtPos(new Vector2I(Mathf.RoundToInt(atPosition.X), Mathf.RoundToInt(atPosition.Y)), allowOutOfBounds: true);
		int num = Mathf.Clamp(lineColumnAtPos.X, 0, GetLineCount() - 1);
		int y = Mathf.Clamp(lineColumnAtPos.Y, 0, GetLine(num).Length);
		return new Vector2I(num, y);
	}

	private static bool IsCSharpTypeBodyOffset(string text, int offset)
	{
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		offset = Mathf.Clamp(offset, 0, text.Length);
		int num = 0;
		HashSet<int> hashSet = new HashSet<int>();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		for (int i = 0; i < offset; i++)
		{
			char c = text[i];
			char c2 = ((i + 1 < text.Length) ? text[i + 1] : '\0');
			if (flag)
			{
				if (c == '\n')
				{
					flag = false;
				}
				continue;
			}
			if (flag2)
			{
				if (c == '*' && c2 == '/')
				{
					flag2 = false;
					i++;
				}
				continue;
			}
			if (flag3)
			{
				if (flag4)
				{
					if (c == '"' && c2 == '"')
					{
						i++;
					}
					else if (c == '"')
					{
						flag3 = false;
						flag4 = false;
					}
					continue;
				}
				switch (c)
				{
				case '\\':
					i++;
					break;
				case '"':
					flag3 = false;
					break;
				}
				continue;
			}
			if (flag5)
			{
				switch (c)
				{
				case '\\':
					i++;
					break;
				case '\'':
					flag5 = false;
					break;
				}
				continue;
			}
			if (c == '/' && c2 == '/')
			{
				flag = true;
				i++;
				continue;
			}
			if (c == '/' && c2 == '*')
			{
				flag2 = true;
				i++;
				continue;
			}
			if (c == '@' && c2 == '"')
			{
				flag3 = true;
				flag4 = true;
				i++;
				continue;
			}
			switch (c)
			{
			case '"':
				flag3 = true;
				flag4 = false;
				break;
			case '\'':
				flag5 = true;
				break;
			case '{':
				num++;
				if (IsTypeDeclarationBrace(text, i))
				{
					hashSet.Add(num);
				}
				break;
			case '}':
				hashSet.Remove(num);
				num = Math.Max(0, num - 1);
				break;
			}
		}
		return hashSet.Contains(num);
	}

	private static bool IsTypeDeclarationBrace(string text, int braceIndex)
	{
		int num = 0;
		for (int num2 = braceIndex - 1; num2 >= 0; num2--)
		{
			char c = text[num2];
			if (c == '{' || c == '}' || c == ';')
			{
				num = num2 + 1;
				break;
			}
		}
		return Regex.IsMatch(text.Substring(num, braceIndex - num), "\\b(class|struct|interface|record)\\s+[A-Za-z_][A-Za-z0-9_]*");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(62)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadIconSafe, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetupErrorGutter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCompletionTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateValidationTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetFileExtension, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "extension", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SuspendBackgroundWork, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResumeBackgroundWork, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetDocumentPath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProjectRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDocumentPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProjectRoot, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetBreakpointLines, new PropertyInfo(Variant.Type.PackedInt32Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleBreakpoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCompletionProvider, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSyntaxHighlighter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "extension", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckSyntaxErrors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateValidationDelay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCompletionTimerTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnValidationTimerTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTextChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCaretChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestCompletionNow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueCompletionRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartCompletionWorkerLockedIfNeeded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeIndexPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InvalidateCompletionWork, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "disableScheduler", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPendingCompletionRequestCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCompletionAnalysisWorkerEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCompletionAnalysisWorkerExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LastCompletionContainsOption, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "insert", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PublishedCompletionContainsOption, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "insert", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetCompletionMetricsForProbe, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestValidationNow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueValidationRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartValidationWorkerLockedIfNeeded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelValidationWork, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "disableScheduler", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearPendingValidationRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelActiveValidationCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPendingValidationRequestCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePeakActiveWorkers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "activeWorkers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetValidationMetricsForProbe, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddError, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "severity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "code", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetErrorGutterIcon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isError", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearErrorGutterIcon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearAllErrors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetErrorCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetWarningCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasAnyErrors, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetAllText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetAllText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadDocumentText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InsertAtCursor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CanDropData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._DropData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCaretFromDropPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsCSharpTypeBodyDropContext, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTextOffsetFromDropPosition, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDropLineColumn, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsCSharpTypeBodyOffset, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsTypeDeclarationBrace, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "braceIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.LoadIconSafe && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadIconSafe(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetupErrorGutter && args.Count == 0)
		{
			SetupErrorGutter();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCompletionTimer && args.Count == 0)
		{
			CreateCompletionTimer();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateValidationTimer && args.Count == 0)
		{
			CreateValidationTimer();
			ret = default;
			return true;
		}
		if (method == MethodName.SetFileExtension && args.Count == 1)
		{
			SetFileExtension(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SuspendBackgroundWork && args.Count == 0)
		{
			SuspendBackgroundWork();
			ret = default;
			return true;
		}
		if (method == MethodName.ResumeBackgroundWork && args.Count == 0)
		{
			ResumeBackgroundWork();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.SetDocumentPath && args.Count == 1)
		{
			SetDocumentPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProjectRoot && args.Count == 1)
		{
			SetProjectRoot(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDocumentPath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetDocumentPath());
			return true;
		}
		if (method == MethodName.GetProjectRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetProjectRoot());
			return true;
		}
		if (method == MethodName.GetBreakpointLines && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int[]>(GetBreakpointLines());
			return true;
		}
		if (method == MethodName.ToggleBreakpoint && args.Count == 1)
		{
			ToggleBreakpoint(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCompletionProvider && args.Count == 0)
		{
			UpdateCompletionProvider();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSyntaxHighlighter && args.Count == 1)
		{
			UpdateSyntaxHighlighter(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckSyntaxErrors && args.Count == 0)
		{
			CheckSyntaxErrors();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateValidationDelay && args.Count == 0)
		{
			UpdateValidationDelay();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCompletionTimerTimeout && args.Count == 0)
		{
			OnCompletionTimerTimeout();
			ret = default;
			return true;
		}
		if (method == MethodName.OnValidationTimerTimeout && args.Count == 0)
		{
			OnValidationTimerTimeout();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTextChanged && args.Count == 0)
		{
			OnTextChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCaretChanged && args.Count == 0)
		{
			OnCaretChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RequestCompletionNow && args.Count == 0)
		{
			RequestCompletionNow();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueCompletionRequest && args.Count == 1)
		{
			QueueCompletionRequest(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartCompletionWorkerLockedIfNeeded && args.Count == 0)
		{
			StartCompletionWorkerLockedIfNeeded();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeIndexPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeIndexPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.InvalidateCompletionWork && args.Count == 1)
		{
			InvalidateCompletionWork(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPendingCompletionRequestCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPendingCompletionRequestCount());
			return true;
		}
		if (method == MethodName.OnCompletionAnalysisWorkerEntered && args.Count == 0)
		{
			OnCompletionAnalysisWorkerEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCompletionAnalysisWorkerExited && args.Count == 0)
		{
			OnCompletionAnalysisWorkerExited();
			ret = default;
			return true;
		}
		if (method == MethodName.LastCompletionContainsOption && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(LastCompletionContainsOption(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.PublishedCompletionContainsOption && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(PublishedCompletionContainsOption(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResetCompletionMetricsForProbe && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ResetCompletionMetricsForProbe());
			return true;
		}
		if (method == MethodName.RequestValidationNow && args.Count == 0)
		{
			RequestValidationNow();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueValidationRequest && args.Count == 0)
		{
			QueueValidationRequest();
			ret = default;
			return true;
		}
		if (method == MethodName.StartValidationWorkerLockedIfNeeded && args.Count == 0)
		{
			StartValidationWorkerLockedIfNeeded();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelValidationWork && args.Count == 1)
		{
			CancelValidationWork(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPendingValidationRequest && args.Count == 0)
		{
			ClearPendingValidationRequest();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelActiveValidationCheck && args.Count == 0)
		{
			CancelActiveValidationCheck();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPendingValidationRequestCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPendingValidationRequestCount());
			return true;
		}
		if (method == MethodName.UpdatePeakActiveWorkers && args.Count == 1)
		{
			UpdatePeakActiveWorkers(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetValidationMetricsForProbe && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ResetValidationMetricsForProbe());
			return true;
		}
		if (method == MethodName.AddError && args.Count == 5)
		{
			AddError(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetErrorGutterIcon && args.Count == 2)
		{
			SetErrorGutterIcon(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearErrorGutterIcon && args.Count == 1)
		{
			ClearErrorGutterIcon(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearAllErrors && args.Count == 0)
		{
			ClearAllErrors();
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
		if (method == MethodName.HasAnyErrors && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasAnyErrors());
			return true;
		}
		if (method == MethodName.GetAllText && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetAllText());
			return true;
		}
		if (method == MethodName.SetAllText && args.Count == 1)
		{
			SetAllText(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadDocumentText && args.Count == 1)
		{
			LoadDocumentText(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InsertAtCursor && args.Count == 1)
		{
			InsertAtCursor(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._CanDropData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_CanDropData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._DropData && args.Count == 2)
		{
			_DropData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCaretFromDropPosition && args.Count == 1)
		{
			SetCaretFromDropPosition(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsCSharpTypeBodyDropContext && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCSharpTypeBodyDropContext(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTextOffsetFromDropPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetTextOffsetFromDropPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDropLineColumn && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetDropLineColumn(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCSharpTypeBodyOffset && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCSharpTypeBodyOffset(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.IsTypeDeclarationBrace && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTypeDeclarationBrace(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadIconSafe && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadIconSafe(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeIndexPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeIndexPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCSharpTypeBodyOffset && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCSharpTypeBodyOffset(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.IsTypeDeclarationBrace && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTypeDeclarationBrace(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
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
		if (method == MethodName.LoadIconSafe)
		{
			return true;
		}
		if (method == MethodName.SetupErrorGutter)
		{
			return true;
		}
		if (method == MethodName.CreateCompletionTimer)
		{
			return true;
		}
		if (method == MethodName.CreateValidationTimer)
		{
			return true;
		}
		if (method == MethodName.SetFileExtension)
		{
			return true;
		}
		if (method == MethodName.SuspendBackgroundWork)
		{
			return true;
		}
		if (method == MethodName.ResumeBackgroundWork)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.SetDocumentPath)
		{
			return true;
		}
		if (method == MethodName.SetProjectRoot)
		{
			return true;
		}
		if (method == MethodName.GetDocumentPath)
		{
			return true;
		}
		if (method == MethodName.GetProjectRoot)
		{
			return true;
		}
		if (method == MethodName.GetBreakpointLines)
		{
			return true;
		}
		if (method == MethodName.ToggleBreakpoint)
		{
			return true;
		}
		if (method == MethodName.UpdateCompletionProvider)
		{
			return true;
		}
		if (method == MethodName.UpdateSyntaxHighlighter)
		{
			return true;
		}
		if (method == MethodName.CheckSyntaxErrors)
		{
			return true;
		}
		if (method == MethodName.UpdateValidationDelay)
		{
			return true;
		}
		if (method == MethodName.OnCompletionTimerTimeout)
		{
			return true;
		}
		if (method == MethodName.OnValidationTimerTimeout)
		{
			return true;
		}
		if (method == MethodName.OnTextChanged)
		{
			return true;
		}
		if (method == MethodName.OnCaretChanged)
		{
			return true;
		}
		if (method == MethodName.RequestCompletionNow)
		{
			return true;
		}
		if (method == MethodName.QueueCompletionRequest)
		{
			return true;
		}
		if (method == MethodName.StartCompletionWorkerLockedIfNeeded)
		{
			return true;
		}
		if (method == MethodName.NormalizeIndexPath)
		{
			return true;
		}
		if (method == MethodName.InvalidateCompletionWork)
		{
			return true;
		}
		if (method == MethodName.GetPendingCompletionRequestCount)
		{
			return true;
		}
		if (method == MethodName.OnCompletionAnalysisWorkerEntered)
		{
			return true;
		}
		if (method == MethodName.OnCompletionAnalysisWorkerExited)
		{
			return true;
		}
		if (method == MethodName.LastCompletionContainsOption)
		{
			return true;
		}
		if (method == MethodName.PublishedCompletionContainsOption)
		{
			return true;
		}
		if (method == MethodName.ResetCompletionMetricsForProbe)
		{
			return true;
		}
		if (method == MethodName.RequestValidationNow)
		{
			return true;
		}
		if (method == MethodName.QueueValidationRequest)
		{
			return true;
		}
		if (method == MethodName.StartValidationWorkerLockedIfNeeded)
		{
			return true;
		}
		if (method == MethodName.CancelValidationWork)
		{
			return true;
		}
		if (method == MethodName.ClearPendingValidationRequest)
		{
			return true;
		}
		if (method == MethodName.CancelActiveValidationCheck)
		{
			return true;
		}
		if (method == MethodName.GetPendingValidationRequestCount)
		{
			return true;
		}
		if (method == MethodName.UpdatePeakActiveWorkers)
		{
			return true;
		}
		if (method == MethodName.ResetValidationMetricsForProbe)
		{
			return true;
		}
		if (method == MethodName.AddError)
		{
			return true;
		}
		if (method == MethodName.SetErrorGutterIcon)
		{
			return true;
		}
		if (method == MethodName.ClearErrorGutterIcon)
		{
			return true;
		}
		if (method == MethodName.ClearAllErrors)
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
		if (method == MethodName.HasAnyErrors)
		{
			return true;
		}
		if (method == MethodName.GetAllText)
		{
			return true;
		}
		if (method == MethodName.SetAllText)
		{
			return true;
		}
		if (method == MethodName.LoadDocumentText)
		{
			return true;
		}
		if (method == MethodName.InsertAtCursor)
		{
			return true;
		}
		if (method == MethodName._CanDropData)
		{
			return true;
		}
		if (method == MethodName._DropData)
		{
			return true;
		}
		if (method == MethodName.SetCaretFromDropPosition)
		{
			return true;
		}
		if (method == MethodName.IsCSharpTypeBodyDropContext)
		{
			return true;
		}
		if (method == MethodName.GetTextOffsetFromDropPosition)
		{
			return true;
		}
		if (method == MethodName.GetDropLineColumn)
		{
			return true;
		}
		if (method == MethodName.IsCSharpTypeBodyOffset)
		{
			return true;
		}
		if (method == MethodName.IsTypeDeclarationBrace)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._completionTimer)
		{
			_completionTimer = VariantUtils.ConvertTo<Godot.Timer>(in value);
			return true;
		}
		if (name == PropertyName._validationTimer)
		{
			_validationTimer = VariantUtils.ConvertTo<Godot.Timer>(in value);
			return true;
		}
		if (name == PropertyName._validationGeneration)
		{
			_validationGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._validationWorkerRunning)
		{
			_validationWorkerRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._validationSchedulerEnabled)
		{
			_validationSchedulerEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._validationRequests)
		{
			_validationRequests = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._validationCoalescedRequests)
		{
			_validationCoalescedRequests = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._validationExecutedChecks)
		{
			_validationExecutedChecks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._validationPublishedResults)
		{
			_validationPublishedResults = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._validationStaleResults)
		{
			_validationStaleResults = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._validationCanceledChecks)
		{
			_validationCanceledChecks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._validationActiveWorkers)
		{
			_validationActiveWorkers = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._validationPeakActiveWorkers)
		{
			_validationPeakActiveWorkers = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._validationLastDiagnosticCount)
		{
			_validationLastDiagnosticCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._validationMaxCaptureMilliseconds)
		{
			_validationMaxCaptureMilliseconds = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._validationMaxApplyMilliseconds)
		{
			_validationMaxApplyMilliseconds = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._completionWorkerRunning)
		{
			_completionWorkerRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._completionSchedulerEnabled)
		{
			_completionSchedulerEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._completionDocumentRevision)
		{
			_completionDocumentRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionRequests)
		{
			_completionRequests = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionCoalescedRequests)
		{
			_completionCoalescedRequests = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionExecutedAnalyses)
		{
			_completionExecutedAnalyses = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionWorkerEnteredAnalyses)
		{
			_completionWorkerEnteredAnalyses = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionActiveAnalysisWorkers)
		{
			_completionActiveAnalysisWorkers = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionAppliedResults)
		{
			_completionAppliedResults = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionCanceledAnalyses)
		{
			_completionCanceledAnalyses = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionStaleResults)
		{
			_completionStaleResults = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionActiveWorkers)
		{
			_completionActiveWorkers = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionPeakActiveWorkers)
		{
			_completionPeakActiveWorkers = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionLastRequestedRevision)
		{
			_completionLastRequestedRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionLastAppliedRevision)
		{
			_completionLastAppliedRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionLastOptionCount)
		{
			_completionLastOptionCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._completionMaxCaptureMilliseconds)
		{
			_completionMaxCaptureMilliseconds = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._completionMaxApplyMilliseconds)
		{
			_completionMaxApplyMilliseconds = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._completionLastAppliedPrefix)
		{
			_completionLastAppliedPrefix = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._currentCompletionProvider)
		{
			_currentCompletionProvider = VariantUtils.ConvertTo<XWCodeCompletionProvider>(in value);
			return true;
		}
		if (name == PropertyName._csharpCompletionProvider)
		{
			_csharpCompletionProvider = VariantUtils.ConvertTo<XWCSharpCompletionProvider>(in value);
			return true;
		}
		if (name == PropertyName._csharpHighlighter)
		{
			_csharpHighlighter = VariantUtils.ConvertTo<CodeHighlighter>(in value);
			return true;
		}
		if (name == PropertyName._largeDocumentSyntaxHighlightSuppressed)
		{
			_largeDocumentSyntaxHighlightSuppressed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._currentFileExtension)
		{
			_currentFileExtension = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._currentDocumentPath)
		{
			_currentDocumentPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._projectRoot)
		{
			_projectRoot = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._lastIndexedSource)
		{
			_lastIndexedSource = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._errorGutterIndex)
		{
			_errorGutterIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastErrorCount)
		{
			_lastErrorCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastWarningCount)
		{
			_lastWarningCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._errorIcon)
		{
			_errorIcon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._warningIcon)
		{
			_warningIcon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.HasPendingBackgroundWork)
		{
			value = VariantUtils.CreateFrom<bool>(HasPendingBackgroundWork);
			return true;
		}
		if (name == PropertyName._completionTimer)
		{
			value = VariantUtils.CreateFrom(in _completionTimer);
			return true;
		}
		if (name == PropertyName._validationTimer)
		{
			value = VariantUtils.CreateFrom(in _validationTimer);
			return true;
		}
		if (name == PropertyName._validationGeneration)
		{
			value = VariantUtils.CreateFrom(in _validationGeneration);
			return true;
		}
		if (name == PropertyName._validationWorkerRunning)
		{
			value = VariantUtils.CreateFrom(in _validationWorkerRunning);
			return true;
		}
		if (name == PropertyName._validationSchedulerEnabled)
		{
			value = VariantUtils.CreateFrom(in _validationSchedulerEnabled);
			return true;
		}
		if (name == PropertyName._validationRequests)
		{
			value = VariantUtils.CreateFrom(in _validationRequests);
			return true;
		}
		if (name == PropertyName._validationCoalescedRequests)
		{
			value = VariantUtils.CreateFrom(in _validationCoalescedRequests);
			return true;
		}
		if (name == PropertyName._validationExecutedChecks)
		{
			value = VariantUtils.CreateFrom(in _validationExecutedChecks);
			return true;
		}
		if (name == PropertyName._validationPublishedResults)
		{
			value = VariantUtils.CreateFrom(in _validationPublishedResults);
			return true;
		}
		if (name == PropertyName._validationStaleResults)
		{
			value = VariantUtils.CreateFrom(in _validationStaleResults);
			return true;
		}
		if (name == PropertyName._validationCanceledChecks)
		{
			value = VariantUtils.CreateFrom(in _validationCanceledChecks);
			return true;
		}
		if (name == PropertyName._validationActiveWorkers)
		{
			value = VariantUtils.CreateFrom(in _validationActiveWorkers);
			return true;
		}
		if (name == PropertyName._validationPeakActiveWorkers)
		{
			value = VariantUtils.CreateFrom(in _validationPeakActiveWorkers);
			return true;
		}
		if (name == PropertyName._validationLastDiagnosticCount)
		{
			value = VariantUtils.CreateFrom(in _validationLastDiagnosticCount);
			return true;
		}
		if (name == PropertyName._validationMaxCaptureMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _validationMaxCaptureMilliseconds);
			return true;
		}
		if (name == PropertyName._validationMaxApplyMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _validationMaxApplyMilliseconds);
			return true;
		}
		if (name == PropertyName._completionWorkerRunning)
		{
			value = VariantUtils.CreateFrom(in _completionWorkerRunning);
			return true;
		}
		if (name == PropertyName._completionSchedulerEnabled)
		{
			value = VariantUtils.CreateFrom(in _completionSchedulerEnabled);
			return true;
		}
		if (name == PropertyName._completionDocumentRevision)
		{
			value = VariantUtils.CreateFrom(in _completionDocumentRevision);
			return true;
		}
		if (name == PropertyName._completionRequests)
		{
			value = VariantUtils.CreateFrom(in _completionRequests);
			return true;
		}
		if (name == PropertyName._completionCoalescedRequests)
		{
			value = VariantUtils.CreateFrom(in _completionCoalescedRequests);
			return true;
		}
		if (name == PropertyName._completionExecutedAnalyses)
		{
			value = VariantUtils.CreateFrom(in _completionExecutedAnalyses);
			return true;
		}
		if (name == PropertyName._completionWorkerEnteredAnalyses)
		{
			value = VariantUtils.CreateFrom(in _completionWorkerEnteredAnalyses);
			return true;
		}
		if (name == PropertyName._completionActiveAnalysisWorkers)
		{
			value = VariantUtils.CreateFrom(in _completionActiveAnalysisWorkers);
			return true;
		}
		if (name == PropertyName._completionAppliedResults)
		{
			value = VariantUtils.CreateFrom(in _completionAppliedResults);
			return true;
		}
		if (name == PropertyName._completionCanceledAnalyses)
		{
			value = VariantUtils.CreateFrom(in _completionCanceledAnalyses);
			return true;
		}
		if (name == PropertyName._completionStaleResults)
		{
			value = VariantUtils.CreateFrom(in _completionStaleResults);
			return true;
		}
		if (name == PropertyName._completionActiveWorkers)
		{
			value = VariantUtils.CreateFrom(in _completionActiveWorkers);
			return true;
		}
		if (name == PropertyName._completionPeakActiveWorkers)
		{
			value = VariantUtils.CreateFrom(in _completionPeakActiveWorkers);
			return true;
		}
		if (name == PropertyName._completionLastRequestedRevision)
		{
			value = VariantUtils.CreateFrom(in _completionLastRequestedRevision);
			return true;
		}
		if (name == PropertyName._completionLastAppliedRevision)
		{
			value = VariantUtils.CreateFrom(in _completionLastAppliedRevision);
			return true;
		}
		if (name == PropertyName._completionLastOptionCount)
		{
			value = VariantUtils.CreateFrom(in _completionLastOptionCount);
			return true;
		}
		if (name == PropertyName._completionMaxCaptureMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _completionMaxCaptureMilliseconds);
			return true;
		}
		if (name == PropertyName._completionMaxApplyMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _completionMaxApplyMilliseconds);
			return true;
		}
		if (name == PropertyName._completionLastAppliedPrefix)
		{
			value = VariantUtils.CreateFrom(in _completionLastAppliedPrefix);
			return true;
		}
		if (name == PropertyName._currentCompletionProvider)
		{
			value = VariantUtils.CreateFrom(in _currentCompletionProvider);
			return true;
		}
		if (name == PropertyName._csharpCompletionProvider)
		{
			value = VariantUtils.CreateFrom(in _csharpCompletionProvider);
			return true;
		}
		if (name == PropertyName._csharpHighlighter)
		{
			value = VariantUtils.CreateFrom(in _csharpHighlighter);
			return true;
		}
		if (name == PropertyName._largeDocumentSyntaxHighlightSuppressed)
		{
			value = VariantUtils.CreateFrom(in _largeDocumentSyntaxHighlightSuppressed);
			return true;
		}
		if (name == PropertyName._currentFileExtension)
		{
			value = VariantUtils.CreateFrom(in _currentFileExtension);
			return true;
		}
		if (name == PropertyName._currentDocumentPath)
		{
			value = VariantUtils.CreateFrom(in _currentDocumentPath);
			return true;
		}
		if (name == PropertyName._projectRoot)
		{
			value = VariantUtils.CreateFrom(in _projectRoot);
			return true;
		}
		if (name == PropertyName._lastIndexedSource)
		{
			value = VariantUtils.CreateFrom(in _lastIndexedSource);
			return true;
		}
		if (name == PropertyName._errorGutterIndex)
		{
			value = VariantUtils.CreateFrom(in _errorGutterIndex);
			return true;
		}
		if (name == PropertyName._lastErrorCount)
		{
			value = VariantUtils.CreateFrom(in _lastErrorCount);
			return true;
		}
		if (name == PropertyName._lastWarningCount)
		{
			value = VariantUtils.CreateFrom(in _lastWarningCount);
			return true;
		}
		if (name == PropertyName._errorIcon)
		{
			value = VariantUtils.CreateFrom(in _errorIcon);
			return true;
		}
		if (name == PropertyName._warningIcon)
		{
			value = VariantUtils.CreateFrom(in _warningIcon);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._completionTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._validationTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._validationWorkerRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._validationSchedulerEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationRequests, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationCoalescedRequests, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationExecutedChecks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationPublishedResults, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationStaleResults, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationCanceledChecks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationActiveWorkers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationPeakActiveWorkers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationLastDiagnosticCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationMaxCaptureMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validationMaxApplyMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._completionWorkerRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._completionSchedulerEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionDocumentRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionRequests, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionCoalescedRequests, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionExecutedAnalyses, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionWorkerEnteredAnalyses, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionActiveAnalysisWorkers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionAppliedResults, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionCanceledAnalyses, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionStaleResults, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionActiveWorkers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionPeakActiveWorkers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionLastRequestedRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionLastAppliedRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionLastOptionCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionMaxCaptureMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completionMaxApplyMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._completionLastAppliedPrefix, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._currentCompletionProvider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._csharpCompletionProvider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._csharpHighlighter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._largeDocumentSyntaxHighlightSuppressed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._currentFileExtension, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._currentDocumentPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._projectRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._lastIndexedSource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._errorGutterIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastErrorCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastWarningCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._errorIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._warningIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasPendingBackgroundWork, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._completionTimer, Variant.From(in _completionTimer));
		info.AddProperty(PropertyName._validationTimer, Variant.From(in _validationTimer));
		info.AddProperty(PropertyName._validationGeneration, Variant.From(in _validationGeneration));
		info.AddProperty(PropertyName._validationWorkerRunning, Variant.From(in _validationWorkerRunning));
		info.AddProperty(PropertyName._validationSchedulerEnabled, Variant.From(in _validationSchedulerEnabled));
		info.AddProperty(PropertyName._validationRequests, Variant.From(in _validationRequests));
		info.AddProperty(PropertyName._validationCoalescedRequests, Variant.From(in _validationCoalescedRequests));
		info.AddProperty(PropertyName._validationExecutedChecks, Variant.From(in _validationExecutedChecks));
		info.AddProperty(PropertyName._validationPublishedResults, Variant.From(in _validationPublishedResults));
		info.AddProperty(PropertyName._validationStaleResults, Variant.From(in _validationStaleResults));
		info.AddProperty(PropertyName._validationCanceledChecks, Variant.From(in _validationCanceledChecks));
		info.AddProperty(PropertyName._validationActiveWorkers, Variant.From(in _validationActiveWorkers));
		info.AddProperty(PropertyName._validationPeakActiveWorkers, Variant.From(in _validationPeakActiveWorkers));
		info.AddProperty(PropertyName._validationLastDiagnosticCount, Variant.From(in _validationLastDiagnosticCount));
		info.AddProperty(PropertyName._validationMaxCaptureMilliseconds, Variant.From(in _validationMaxCaptureMilliseconds));
		info.AddProperty(PropertyName._validationMaxApplyMilliseconds, Variant.From(in _validationMaxApplyMilliseconds));
		info.AddProperty(PropertyName._completionWorkerRunning, Variant.From(in _completionWorkerRunning));
		info.AddProperty(PropertyName._completionSchedulerEnabled, Variant.From(in _completionSchedulerEnabled));
		info.AddProperty(PropertyName._completionDocumentRevision, Variant.From(in _completionDocumentRevision));
		info.AddProperty(PropertyName._completionRequests, Variant.From(in _completionRequests));
		info.AddProperty(PropertyName._completionCoalescedRequests, Variant.From(in _completionCoalescedRequests));
		info.AddProperty(PropertyName._completionExecutedAnalyses, Variant.From(in _completionExecutedAnalyses));
		info.AddProperty(PropertyName._completionWorkerEnteredAnalyses, Variant.From(in _completionWorkerEnteredAnalyses));
		info.AddProperty(PropertyName._completionActiveAnalysisWorkers, Variant.From(in _completionActiveAnalysisWorkers));
		info.AddProperty(PropertyName._completionAppliedResults, Variant.From(in _completionAppliedResults));
		info.AddProperty(PropertyName._completionCanceledAnalyses, Variant.From(in _completionCanceledAnalyses));
		info.AddProperty(PropertyName._completionStaleResults, Variant.From(in _completionStaleResults));
		info.AddProperty(PropertyName._completionActiveWorkers, Variant.From(in _completionActiveWorkers));
		info.AddProperty(PropertyName._completionPeakActiveWorkers, Variant.From(in _completionPeakActiveWorkers));
		info.AddProperty(PropertyName._completionLastRequestedRevision, Variant.From(in _completionLastRequestedRevision));
		info.AddProperty(PropertyName._completionLastAppliedRevision, Variant.From(in _completionLastAppliedRevision));
		info.AddProperty(PropertyName._completionLastOptionCount, Variant.From(in _completionLastOptionCount));
		info.AddProperty(PropertyName._completionMaxCaptureMilliseconds, Variant.From(in _completionMaxCaptureMilliseconds));
		info.AddProperty(PropertyName._completionMaxApplyMilliseconds, Variant.From(in _completionMaxApplyMilliseconds));
		info.AddProperty(PropertyName._completionLastAppliedPrefix, Variant.From(in _completionLastAppliedPrefix));
		info.AddProperty(PropertyName._currentCompletionProvider, Variant.From(in _currentCompletionProvider));
		info.AddProperty(PropertyName._csharpCompletionProvider, Variant.From(in _csharpCompletionProvider));
		info.AddProperty(PropertyName._csharpHighlighter, Variant.From(in _csharpHighlighter));
		info.AddProperty(PropertyName._largeDocumentSyntaxHighlightSuppressed, Variant.From(in _largeDocumentSyntaxHighlightSuppressed));
		info.AddProperty(PropertyName._currentFileExtension, Variant.From(in _currentFileExtension));
		info.AddProperty(PropertyName._currentDocumentPath, Variant.From(in _currentDocumentPath));
		info.AddProperty(PropertyName._projectRoot, Variant.From(in _projectRoot));
		info.AddProperty(PropertyName._lastIndexedSource, Variant.From(in _lastIndexedSource));
		info.AddProperty(PropertyName._errorGutterIndex, Variant.From(in _errorGutterIndex));
		info.AddProperty(PropertyName._lastErrorCount, Variant.From(in _lastErrorCount));
		info.AddProperty(PropertyName._lastWarningCount, Variant.From(in _lastWarningCount));
		info.AddProperty(PropertyName._errorIcon, Variant.From(in _errorIcon));
		info.AddProperty(PropertyName._warningIcon, Variant.From(in _warningIcon));
		info.AddSignalEventDelegate(SignalName.ErrorsChanged, backing_ErrorsChanged);
		info.AddSignalEventDelegate(SignalName.DiagnosticsChanged, backing_DiagnosticsChanged);
		info.AddSignalEventDelegate(SignalName.ErrorClicked, backing_ErrorClicked);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._completionTimer, out var value))
		{
			_completionTimer = value.As<Godot.Timer>();
		}
		if (info.TryGetProperty(PropertyName._validationTimer, out var value2))
		{
			_validationTimer = value2.As<Godot.Timer>();
		}
		if (info.TryGetProperty(PropertyName._validationGeneration, out var value3))
		{
			_validationGeneration = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._validationWorkerRunning, out var value4))
		{
			_validationWorkerRunning = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._validationSchedulerEnabled, out var value5))
		{
			_validationSchedulerEnabled = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._validationRequests, out var value6))
		{
			_validationRequests = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._validationCoalescedRequests, out var value7))
		{
			_validationCoalescedRequests = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName._validationExecutedChecks, out var value8))
		{
			_validationExecutedChecks = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._validationPublishedResults, out var value9))
		{
			_validationPublishedResults = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._validationStaleResults, out var value10))
		{
			_validationStaleResults = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._validationCanceledChecks, out var value11))
		{
			_validationCanceledChecks = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName._validationActiveWorkers, out var value12))
		{
			_validationActiveWorkers = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName._validationPeakActiveWorkers, out var value13))
		{
			_validationPeakActiveWorkers = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName._validationLastDiagnosticCount, out var value14))
		{
			_validationLastDiagnosticCount = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName._validationMaxCaptureMilliseconds, out var value15))
		{
			_validationMaxCaptureMilliseconds = value15.As<long>();
		}
		if (info.TryGetProperty(PropertyName._validationMaxApplyMilliseconds, out var value16))
		{
			_validationMaxApplyMilliseconds = value16.As<long>();
		}
		if (info.TryGetProperty(PropertyName._completionWorkerRunning, out var value17))
		{
			_completionWorkerRunning = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._completionSchedulerEnabled, out var value18))
		{
			_completionSchedulerEnabled = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._completionDocumentRevision, out var value19))
		{
			_completionDocumentRevision = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionRequests, out var value20))
		{
			_completionRequests = value20.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionCoalescedRequests, out var value21))
		{
			_completionCoalescedRequests = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionExecutedAnalyses, out var value22))
		{
			_completionExecutedAnalyses = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionWorkerEnteredAnalyses, out var value23))
		{
			_completionWorkerEnteredAnalyses = value23.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionActiveAnalysisWorkers, out var value24))
		{
			_completionActiveAnalysisWorkers = value24.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionAppliedResults, out var value25))
		{
			_completionAppliedResults = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionCanceledAnalyses, out var value26))
		{
			_completionCanceledAnalyses = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionStaleResults, out var value27))
		{
			_completionStaleResults = value27.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionActiveWorkers, out var value28))
		{
			_completionActiveWorkers = value28.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionPeakActiveWorkers, out var value29))
		{
			_completionPeakActiveWorkers = value29.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionLastRequestedRevision, out var value30))
		{
			_completionLastRequestedRevision = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionLastAppliedRevision, out var value31))
		{
			_completionLastAppliedRevision = value31.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionLastOptionCount, out var value32))
		{
			_completionLastOptionCount = value32.As<int>();
		}
		if (info.TryGetProperty(PropertyName._completionMaxCaptureMilliseconds, out var value33))
		{
			_completionMaxCaptureMilliseconds = value33.As<long>();
		}
		if (info.TryGetProperty(PropertyName._completionMaxApplyMilliseconds, out var value34))
		{
			_completionMaxApplyMilliseconds = value34.As<long>();
		}
		if (info.TryGetProperty(PropertyName._completionLastAppliedPrefix, out var value35))
		{
			_completionLastAppliedPrefix = value35.As<string>();
		}
		if (info.TryGetProperty(PropertyName._currentCompletionProvider, out var value36))
		{
			_currentCompletionProvider = value36.As<XWCodeCompletionProvider>();
		}
		if (info.TryGetProperty(PropertyName._csharpCompletionProvider, out var value37))
		{
			_csharpCompletionProvider = value37.As<XWCSharpCompletionProvider>();
		}
		if (info.TryGetProperty(PropertyName._csharpHighlighter, out var value38))
		{
			_csharpHighlighter = value38.As<CodeHighlighter>();
		}
		if (info.TryGetProperty(PropertyName._largeDocumentSyntaxHighlightSuppressed, out var value39))
		{
			_largeDocumentSyntaxHighlightSuppressed = value39.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._currentFileExtension, out var value40))
		{
			_currentFileExtension = value40.As<string>();
		}
		if (info.TryGetProperty(PropertyName._currentDocumentPath, out var value41))
		{
			_currentDocumentPath = value41.As<string>();
		}
		if (info.TryGetProperty(PropertyName._projectRoot, out var value42))
		{
			_projectRoot = value42.As<string>();
		}
		if (info.TryGetProperty(PropertyName._lastIndexedSource, out var value43))
		{
			_lastIndexedSource = value43.As<string>();
		}
		if (info.TryGetProperty(PropertyName._errorGutterIndex, out var value44))
		{
			_errorGutterIndex = value44.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastErrorCount, out var value45))
		{
			_lastErrorCount = value45.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastWarningCount, out var value46))
		{
			_lastWarningCount = value46.As<int>();
		}
		if (info.TryGetProperty(PropertyName._errorIcon, out var value47))
		{
			_errorIcon = value47.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._warningIcon, out var value48))
		{
			_warningIcon = value48.As<Texture2D>();
		}
		if (info.TryGetSignalEventDelegate<ErrorsChangedEventHandler>(SignalName.ErrorsChanged, out var value49))
		{
			backing_ErrorsChanged = value49;
		}
		if (info.TryGetSignalEventDelegate<DiagnosticsChangedEventHandler>(SignalName.DiagnosticsChanged, out var value50))
		{
			backing_DiagnosticsChanged = value50;
		}
		if (info.TryGetSignalEventDelegate<ErrorClickedEventHandler>(SignalName.ErrorClicked, out var value51))
		{
			backing_ErrorClicked = value51;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(SignalName.ErrorsChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "errorCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "warningCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.DiagnosticsChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ErrorClicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalErrorsChanged(int errorCount, int warningCount)
	{
		StringName errorsChanged = SignalName.ErrorsChanged;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = errorCount;
		buffer[1] = warningCount;
		EmitSignal(errorsChanged, buffer);
	}

	protected void EmitSignalDiagnosticsChanged()
	{
		EmitSignal(SignalName.DiagnosticsChanged, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalErrorClicked(int line, int column)
	{
		StringName errorClicked = SignalName.ErrorClicked;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = line;
		buffer[1] = column;
		EmitSignal(errorClicked, buffer);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ErrorsChanged && args.Count == 2)
		{
			backing_ErrorsChanged?.Invoke(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
		}
		else if (signal == SignalName.DiagnosticsChanged && args.Count == 0)
		{
			backing_DiagnosticsChanged?.Invoke();
		}
		else if (signal == SignalName.ErrorClicked && args.Count == 2)
		{
			backing_ErrorClicked?.Invoke(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ErrorsChanged)
		{
			return true;
		}
		if (signal == SignalName.DiagnosticsChanged)
		{
			return true;
		}
		if (signal == SignalName.ErrorClicked)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
