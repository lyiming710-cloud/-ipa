using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.ScriptEditor;

[ScriptPath("res://Tests/ModEditorScriptIdeRuntimeProbe.cs")]
public class ModEditorScriptIdeRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ChildrenFitWithin = "ChildrenFitWithin";

		public static readonly StringName PopupContainsId = "PopupContainsId";

		public static readonly StringName SetCaretAt = "SetCaretAt";

		public static readonly StringName TryDelete = "TryDelete";

		public static readonly StringName SamePath = "SamePath";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _probeParent = "_probeParent";

		public static readonly StringName _editor = "_editor";

		public static readonly StringName _codeEdit = "_codeEdit";

		public static readonly StringName _monitorFrames = "_monitorFrames";

		public static readonly StringName _maxFrameIntervalMilliseconds = "_maxFrameIntervalMilliseconds";

		public static readonly StringName _debugHostIdleProcess = "_debugHostIdleProcess";

		public static readonly StringName _debugHostQueuedDispatch = "_debugHostQueuedDispatch";

		public static readonly StringName _debugHostCleanupProcess = "_debugHostCleanupProcess";

		public static readonly StringName _debugHostSettledIdle = "_debugHostSettledIdle";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private string _probeParent = "";

	private XWScriptEditor _editor;

	private XWCodeEdit _codeEdit;

	private bool _monitorFrames;

	private double _maxFrameIntervalMilliseconds;

	private bool _debugHostIdleProcess;

	private bool _debugHostQueuedDispatch;

	private bool _debugHostCleanupProcess;

	private bool _debugHostSettledIdle;

	public override async void _Ready()
	{
		bool definition = false;
		bool references = false;
		bool rename = false;
		bool hover = false;
		bool signature = false;
		bool format = false;
		bool quickFix = false;
		bool breakpoint = false;
		bool debug = false;
		bool debugMainThread = false;
		bool debugPreviewFrames = false;
		bool debugExitCleanup = false;
		bool undoRedo = false;
		bool savedReloaded = false;
		bool responsive = false;
		bool layout1000 = false;
		bool actionsCallable = false;
		bool hiddenIdle = false;
		bool inspectorUntouched = false;
		bool pathDenied = false;
		bool concurrentDenied = false;
		bool semanticRaceSafe = false;
		bool renameRaceSafe = false;
		try
		{
			_ = 43;
			try
			{
				ModEditorManager modEditorManager = ModEditorManager.Instance;
				if (!GodotObject.IsInstanceValid(modEditorManager))
				{
					modEditorManager = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Core/ModEditorManager.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<ModEditorManager>(PackedScene.GenEditState.Disabled);
					if (GodotObject.IsInstanceValid(modEditorManager))
					{
						AddChild(modEditorManager, forceReadableName: false, InternalMode.Disabled);
					}
				}
				Require(GodotObject.IsInstanceValid(modEditorManager), "ModEditorManager could not be created.");
				await WaitFrames(2);
				Input.ParseInputEvent(new InputEventKey
				{
					Keycode = Key.F3,
					PhysicalKeycode = Key.F3,
					Pressed = true
				});
				Input.ParseInputEvent(new InputEventKey
				{
					Keycode = Key.F3,
					PhysicalKeycode = Key.F3,
					Pressed = false
				});
				ModEditorPanel panel = await WaitForPanel(900);
				Require(GodotObject.IsInstanceValid(panel), "F3 did not open the real ModEditor panel.");
				_editor = XWEditorInterface.Instance?.GetScriptEditor();
				_codeEdit = _editor?.GetNodeOrNull<XWCodeEdit>("%XWCodeEdit");
				Require(GodotObject.IsInstanceValid(_editor) && GodotObject.IsInstanceValid(_codeEdit), "The real F3 window did not mount the script editor.");
				if (!GodotObject.IsInstanceValid(_editor) || !GodotObject.IsInstanceValid(_codeEdit))
				{
					Finish(definition, references, rename, hover, signature, format, quickFix, breakpoint, debug, debugMainThread, debugPreviewFrames, debugExitCleanup, undoRedo, savedReloaded, responsive, layout1000, actionsCallable, hiddenIdle, inspectorUntouched, pathDenied, concurrentDenied, semanticRaceSafe, renameRaceSafe);
					return;
				}
				_monitorFrames = true;
				MonitorFrameIntervals();
				GodotObject inspectorBefore = XWEditorInterface.Instance.GetInspector();
				ulong inspectorId = (GodotObject.IsInstanceValid(inspectorBefore) ? inspectorBefore.GetInstanceId() : 0);
				string text = Guid.NewGuid().ToString("N");
				_probeParent = ProjectSettings.GlobalizePath("user://ScriptIdeProbe_" + text);
				Directory.CreateDirectory(_probeParent);
				ModProject project = ModProject.Create(_probeParent, "IdeMod" + text, "1.0.0", "probe", "IDE probe");
				Require(project != null, "Could not create the external Mod project.");
				string root = project?.ProjectPath ?? "";
				string scripts = Path.Combine(root, "Scripts");
				Directory.CreateDirectory(scripts);
				string markerPath = Path.Combine(root, "debug-entry.txt");
				string framePath = Path.Combine(root, "debug-frames.txt");
				string cleanupPath = Path.Combine(root, "debug-preview-cleanup.txt");
				string cancelledPath = Path.Combine(root, "debug-cancelled.txt");
				string failFlagPath = Path.Combine(root, "debug-fail.flag");
				string nonCooperativeFlagPath = Path.Combine(root, "debug-noncooperative.flag");
				string exitFlagPath = Path.Combine(root, "debug-exit.flag");
				string exitCleanupPath = Path.Combine(root, "debug-exit-cleanup.txt");
				string definitionPath = Path.Combine(scripts, "HeroService.cs");
				string consumerPath = Path.Combine(scripts, "HeroConsumer.cs");
				string quickFixPath = Path.Combine(scripts, "QuickFixProbe.cs");
				string text2 = markerPath.Replace("\"", "\"\"");
				string text3 = framePath.Replace("\"", "\"\"");
				string text4 = cleanupPath.Replace("\"", "\"\"");
				string text5 = cancelledPath.Replace("\"", "\"\"");
				string text6 = failFlagPath.Replace("\"", "\"\"");
				string text7 = nonCooperativeFlagPath.Replace("\"", "\"\"");
				string value = exitFlagPath.Replace("\"", "\"\"");
				string value2 = exitCleanupPath.Replace("\"", "\"\"");
				string definitionSource = "using System;\nusing System.IO;\nusing System.Threading;\nusing System.Threading.Tasks;\nusing Godot;\nusing PVZHE.ModEditor.ScriptEditor;\n/// <summary>Provides garden calculations.</summary>\npublic class HeroService\n{\n    /// <summary>Returns the adjusted amount.</summary>\n    public int Compute(int amount) => amount + 1;\n    public static async Task ModEditorDebugEntry(XWScriptDebugSession.DebugContext context)\n    {\n        if (File.Exists(@\"" + text6 + "\")) throw new InvalidOperationException(\"probe failure\");\n        var previewNode = new Node { Name = \"ScriptDebugProbeNode\" };\n        context.PreviewRoot.AddChild(previewNode);\n        File.WriteAllText(@\"" + text2 + "\", \"entry=\" + System.Environment.CurrentManagedThreadId + \";main=\" + context.MainThreadId + \";inside=\" + previewNode.IsInsideTree());\n        try\n        {\n            await context.NextProcessFrameAsync();\n            await context.NextProcessFrameAsync();\n            await context.RunOnMainThreadAsync(() => File.WriteAllText(@\"" + text3 + "\", \"thread=\" + System.Environment.CurrentManagedThreadId + \";inside=\" + previewNode.IsInsideTree()));\n            if (File.Exists(@\"" + text7 + "\")) { await Task.Delay(650); return; }\n            await Task.Delay(-1, context.CancellationToken);\n        }\n        catch (OperationCanceledException) { File.WriteAllText(@\"" + text5 + "\", \"cancelled\"); }\n        finally\n        {\n            await context.RunOnMainThreadAsync(() =>\n            {\n                if (GodotObject.IsInstanceValid(previewNode))\n                {\n                    previewNode.GetParent()?.RemoveChild(previewNode);\n                    previewNode.Free();\n                }\n                File.WriteAllText(@\"" + text4 + "\", \"clean\");\n" + $"                if (File.Exists(@\"{value}\")) File.WriteAllText(@\"{value2}\", \"clean\");\n" + "            });\n        }\n    }\n}\n";
				string consumerSource = "public class HeroConsumer\n{\n    public int Run()\n    {\n        var service = new HeroService();\n        return service.Compute(2);\n    }\n}\n";
				string quickFixSource = "public class QuickFixProbe\n{\n    public int Value = 1\n}\n";
				File.WriteAllText(definitionPath, definitionSource);
				File.WriteAllText(consumerPath, consumerSource);
				File.WriteAllText(quickFixPath, quickFixSource.Replace(" = 1\n", " = 1;\n"));
				XWModManifestSyncService.SyncProject(root);
				System.Reflection.MethodInfo enterProject = typeof(ModEditorPanel).GetMethod("EnterProject", BindingFlags.Instance | BindingFlags.NonPublic);
				Require(enterProject != null, "Could not locate the real project-entry workflow.");
				enterProject?.Invoke(panel, new object[1] { project });
				XWEditorInterface.Instance?.FocusPanel("script_editor");
				await WaitFrames(12);
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] project-ready");
				_editor.OpenFile(consumerPath);
				await WaitFrames(3);
				SetCaretAt("HeroService", 1);
				Stopwatch stopwatch = Stopwatch.StartNew();
				Task<bool> definitionTask = _editor.NavigateToDefinitionAtCaretAsync();
				long requestMilliseconds = stopwatch.ElapsedMilliseconds;
				await WaitFrames(1);
				definition = await definitionTask && SamePath(_editor.GetCurrentFilePath(), definitionPath) && _codeEdit.GetCaretLine() == 7;
				responsive = requestMilliseconds < 100;
				Require(definition, "F12 did not navigate from a real reference to its Mod source definition.");
				Require(responsive, $"Starting semantic navigation blocked the UI for {requestMilliseconds}ms.");
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] navigation-ready");
				SetCaretAt("HeroService", 1);
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] references-start");
				int num = await _editor.ShowReferencesAtCaretAsync();
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] references-end");
				Window nodeOrNull = _editor.GetNodeOrNull<Window>("%ReferencesWindow");
				references = num >= 2 && GodotObject.IsInstanceValid(nodeOrNull) && nodeOrNull.Visible;
				nodeOrNull?.Hide();
				Require(references, "Shift+F12 did not show the real project reference list.");
				SetCaretAt("Compute", 1);
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] hover-start");
				XWCSharpIdeService.SymbolInfoResult symbolInfoResult = await _editor.GetHoverAndSignatureAtCaretAsync();
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] hover-end");
				hover = symbolInfoResult?.Documentation.Contains("adjusted amount", StringComparison.OrdinalIgnoreCase) ?? false;
				signature = symbolInfoResult != null && symbolInfoResult.Signature.Contains("Compute", StringComparison.Ordinal) && symbolInfoResult.Signature.Contains("int amount", StringComparison.Ordinal);
				Require(hover, "Hover information did not resolve XML documentation for the real symbol.");
				Require(signature, "Method signature help did not report the resolved C# signature.");
				(semanticRaceSafe, renameRaceSafe) = await VerifySemanticRequestStamps(panel, enterProject, project, root, consumerPath, definitionPath, quickFixPath, consumerSource, definitionSource);
				Require(semanticRaceSafe, "Definition/reference/hover results crossed a tab, text, or Mod project boundary.");
				Require(renameRaceSafe, "An older rename preview replaced or applied over the newest preview.");
				string text8 = _codeEdit.Text;
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] format-start");
				_codeEdit.SetAllText("public class FormatProbe{public int Value(){return 1;}}");
				_codeEdit.EmitSignal(TextEdit.SignalName.TextChanged);
				format = _editor.FormatDocumentOrSelection() && _codeEdit.Text.Contains("public class FormatProbe", StringComparison.Ordinal) && _codeEdit.Text.Contains("\n", StringComparison.Ordinal);
				_codeEdit.Undo();
				format = format && _codeEdit.Text == "public class FormatProbe{public int Value(){return 1;}}";
				_codeEdit.SetAllText(text8);
				_codeEdit.EmitSignal(TextEdit.SignalName.TextChanged);
				_editor.SaveFile();
				Require(format, "Document formatting was not one undoable CodeEdit operation.");
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] format-end");
				_editor.OpenFile(quickFixPath);
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] quickfix-start");
				await WaitFrames(2);
				_codeEdit.SetAllText(quickFixSource);
				_codeEdit.EmitSignal(TextEdit.SignalName.TextChanged);
				_codeEdit.SetExternalDiagnostics(new List<XWCodeErrorChecker.ErrorData>
				{
					new XWCodeErrorChecker.ErrorData
					{
						Line = 2,
						Column = 24,
						Code = "MISSING_SEMICOLON",
						Message = "missing semicolon",
						SeverityLevel = XWCodeErrorChecker.Severity.Error
					}
				});
				_codeEdit.SetCaretLine(2);
				quickFix = _editor.ApplyQuickFixAtCaret() && _codeEdit.GetLine(2).TrimEnd().EndsWith(';');
				_codeEdit.Undo();
				quickFix = quickFix && _codeEdit.Text == quickFixSource;
				_codeEdit.SetAllText(quickFixSource.Replace(" = 1\n", " = 1;\n"));
				_codeEdit.EmitSignal(TextEdit.SignalName.TextChanged);
				_editor.SaveFile();
				Require(quickFix, "Diagnostic quick fix was not applied as one undoable edit.");
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] quickfix-end");
				_editor.OpenFile(definitionPath);
				await WaitFrames(2);
				SetCaretAt("HeroService", 1);
				_codeEdit.SetLineAsBreakpoint(5, breakpointed: true);
				breakpoint = _codeEdit.GuttersDrawBreakpointsGutter && _codeEdit.IsLineBreakpointed(5) && _editor.GetProjectBreakpoints().Count > 0;
				Require(breakpoint, "The built-in breakpoint gutter did not persist into the debug session input.");
				string text9 = Path.Combine(_probeParent, "outside.cs");
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] safety-start");
				string linked = Path.Combine(scripts, "LinkedOutside.cs");
				File.WriteAllText(text9, "public class OutsideSymbol { }");
				try
				{
					File.CreateSymbolicLink(linked, text9);
					try
					{
						await XWCSharpIdeService.PreviewRenameAsync(root, linked, File.ReadAllText(linked), 14, "DeniedSymbol");
					}
					catch (InvalidOperationException)
					{
						pathDenied = true;
					}
				}
				catch (Exception ex2)
				{
					_failures.Add("Could not create the symbolic-link safety fixture: " + ex2.Message);
				}
				finally
				{
					try
					{
						if (File.Exists(linked))
						{
							File.Delete(linked);
						}
					}
					catch
					{
					}
				}
				Require(pathDenied, "Rename accepted a source file routed through a symbolic link/reparse point.");
				SetCaretAt("HeroService", 1);
				XWCSharpIdeService.RenamePreview preview = await _editor.PreviewRenameAtCaretAsync("ConcurrentService");
				File.AppendAllText(consumerPath, "// external change\n");
				concurrentDenied = !(await XWCSharpIdeService.ApplyRenameAsync(preview)) && File.ReadAllText(definitionPath).Contains("HeroService", StringComparison.Ordinal) && File.ReadAllText(consumerPath).Contains("external change", StringComparison.Ordinal);
				Require(concurrentDenied, "Rename overwrote a file changed after the preview was produced.");
				_editor.OpenFile(consumerPath);
				_codeEdit.SetAllText(consumerSource);
				_codeEdit.EmitSignal(TextEdit.SignalName.TextChanged);
				_editor.SaveFile();
				_editor.OpenFile(definitionPath);
				_codeEdit.SetAllText(definitionSource);
				_codeEdit.EmitSignal(TextEdit.SignalName.TextChanged);
				_editor.SaveFile();
				await WaitFrames(2);
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] safety-end");
				SetCaretAt("HeroService", 1);
				XWCSharpIdeService.RenamePreview renamePreview = await _editor.PreviewRenameAtCaretAsync("GardenService");
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] rename-preview-end");
				rename = renamePreview != null && renamePreview.IsValid && renamePreview.UpdatedDocuments.Count == 2 && renamePreview.Occurrences.Count >= 2;
				Require(rename, "F2 preview did not resolve the project-scoped rename set.");
				bool applied = await _editor.ApplyPendingRenameAsync();
				string text10 = File.ReadAllText(definitionPath);
				string text11 = File.ReadAllText(consumerPath);
				GD.Print($"[MOD_EDITOR_SCRIPT_IDE_RENAME] applied={applied} error={XWCSharpIdeService.LastRenameError} definitionChanged={text10.Contains("GardenService", StringComparison.Ordinal)} consumerChanged={text11.Contains("GardenService", StringComparison.Ordinal)}");
				savedReloaded = applied && text10.Contains("GardenService", StringComparison.Ordinal) && text11.Contains("GardenService", StringComparison.Ordinal) && _codeEdit.Text == text10;
				bool undone = await _editor.UndoLastProjectRenameAsync();
				bool originalOnDisk = File.ReadAllText(definitionPath).Contains("HeroService", StringComparison.Ordinal) && File.ReadAllText(consumerPath).Contains("HeroService", StringComparison.Ordinal);
				bool redone = await _editor.RedoLastProjectRenameAsync();
				bool renamedAgain = File.ReadAllText(definitionPath).Contains("GardenService", StringComparison.Ordinal);
				bool flag = await _editor.UndoLastProjectRenameAsync();
				GD.Print($"[MOD_EDITOR_SCRIPT_IDE_RENAME] undone={undone} originalOnDisk={originalOnDisk} redone={redone} renamedAgain={renamedAgain} finalUndo={flag} error={XWCSharpIdeService.LastRenameError}");
				undoRedo = applied & undone & originalOnDisk & redone & renamedAgain & flag;
				savedReloaded = savedReloaded && File.ReadAllText(definitionPath) == definitionSource && _codeEdit.Text == definitionSource;
				Require(savedReloaded, "Rename did not save and reload the uncached on-disk documents.");
				Require(undoRedo, "Project rename did not complete its apply/undo/redo/undo transaction.");
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] refactor-ready");
				Stopwatch debugWatch = Stopwatch.StartNew();
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_DEBUG_STAGE] normal-start");
				Task<XWScriptDebugSession.StartResult> debugStartTask = _editor.StartDebugSessionAsync();
				await WaitFrames(1);
				XWScriptDebugSession.StartResult startResult = await _editor.StartDebugSessionAsync();
				bool debugConcurrentDenied = (startResult == null || !startResult.Success) && (startResult?.Message.Contains("并发", StringComparison.Ordinal) ?? false);
				XWScriptDebugSession.StartResult debugStart = await debugStartTask;
				if (debugStart == null || !debugStart.Success)
				{
					GD.Print("[MOD_EDITOR_SCRIPT_IDE_DEBUG_FAILURE] " + debugStart?.Message);
					foreach (XWCodeErrorChecker.ErrorData item in debugStart?.CompileResult?.Diagnostics ?? new List<XWCodeErrorChecker.ErrorData>())
					{
						GD.Print($"[MOD_EDITOR_SCRIPT_IDE_DEBUG_DIAGNOSTIC] {item.FilePath}:{item.Line + 1}:{item.Column + 1} {item.Message}");
					}
				}
				bool flag2 = await WaitForFile(framePath, 90);
				string text12 = (File.Exists(markerPath) ? File.ReadAllText(markerPath) : "");
				string text13 = (flag2 ? File.ReadAllText(framePath) : "");
				debugMainThread = text12.Contains($"entry={_editor.DebugMainThreadId};main={_editor.DebugMainThreadId}", StringComparison.Ordinal) && text12.Contains("inside=True", StringComparison.Ordinal) && _editor.DebugMainThreadId == System.Environment.CurrentManagedThreadId;
				debugPreviewFrames = flag2 && text13.Contains($"thread={_editor.DebugMainThreadId}", StringComparison.Ordinal) && text13.Contains("inside=True", StringComparison.Ordinal) && _editor.DebugPreviewChildCount == 1;
				bool running = ((debugStart?.Success ?? false) && debugStart.AssemblyLoaded && debugStart.EntryPointInvoked && _editor.IsDebugSessionRunning && _editor.DebugEntryInvoked && _editor.DebugLoadedAssemblyCount == 1) & debugMainThread & debugPreviewFrames;
				GD.Print($"[MOD_EDITOR_SCRIPT_IDE_DEBUG_STAGE] normal-ready success={debugStart?.Success} running={running} concurrentDenied={debugConcurrentDenied} elapsedMs={debugWatch.ElapsedMilliseconds}");
				bool stopped = await _editor.StopDebugSessionAsync();
				for (int i = 0; i < 4; i++)
				{
					if (!_editor.DebugLoadContextAlive)
					{
						break;
					}
					GC.Collect();
					GC.WaitForPendingFinalizers();
					await WaitFrames(1);
				}
				bool cooperativeStopped = stopped && !_editor.IsDebugSessionRunning && _editor.DebugEntryTaskCompleted && _editor.DebugLoadedAssemblyCount == 0 && File.Exists(cancelledPath) && File.ReadAllText(cancelledPath) == "cancelled" && File.Exists(cleanupPath) && File.ReadAllText(cleanupPath) == "clean" && _editor.DebugPreviewChildCount == 0 && !_editor.DebugLoadContextAlive;
				GD.Print($"[MOD_EDITOR_SCRIPT_IDE_DEBUG_STAGE] cooperative-stop stopped={stopped} verified={cooperativeStopped} contextAlive={_editor.DebugLoadContextAlive} elapsedMs={debugWatch.ElapsedMilliseconds}");
				File.WriteAllText(failFlagPath, "fail");
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_DEBUG_STAGE] failing-entry-start");
				XWScriptDebugSession.StartResult startResult2 = await _editor.StartDebugSessionAsync();
				bool failedEntrySafe = (startResult2 == null || !startResult2.Success) && !_editor.IsDebugSessionRunning && _editor.DebugLoadedAssemblyCount == 0;
				GD.Print($"[MOD_EDITOR_SCRIPT_IDE_DEBUG_STAGE] failing-entry-end safe={failedEntrySafe} message={startResult2?.Message} elapsedMs={debugWatch.ElapsedMilliseconds}");
				File.Delete(failFlagPath);
				File.WriteAllText(nonCooperativeFlagPath, "non-cooperative");
				TryDelete(framePath);
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_DEBUG_STAGE] noncooperative-start");
				XWScriptDebugSession.StartResult nonCooperativeStart = await _editor.StartDebugSessionAsync();
				bool nonCooperativeEntered = await WaitForFile(framePath, 90);
				bool timedStop = await _editor.StopDebugSessionAsync(100);
				XWScriptDebugSession.StartResult restartWhileActive = await _editor.StartDebugSessionAsync();
				int num2;
				if (((nonCooperativeStart?.Success ?? false) & nonCooperativeEntered) && !timedStop && _editor.IsDebugSessionRunning)
				{
					if (restartWhileActive == null || !restartWhileActive.Success)
					{
						num2 = ((restartWhileActive?.Message.Contains("先成功停止", StringComparison.Ordinal) ?? false) ? 1 : 0);
						goto IL_2479;
					}
				}
				num2 = 0;
				goto IL_2479;
				IL_2479:
				bool nonCooperativeDenied = (byte)num2 != 0;
				await Task.Delay(750);
				bool flag3 = await _editor.StopDebugSessionAsync();
				File.Delete(nonCooperativeFlagPath);
				nonCooperativeDenied = (nonCooperativeDenied & flag3) && !_editor.IsDebugSessionRunning && _editor.DebugLoadedAssemblyCount == 0 && _editor.DebugPreviewChildCount == 0;
				GD.Print($"[MOD_EDITOR_SCRIPT_IDE_DEBUG_STAGE] noncooperative-end safe={nonCooperativeDenied} entered={nonCooperativeEntered} timedStop={timedStop} eventualStop={flag3} restartMessage={restartWhileActive?.Message} elapsedMs={debugWatch.ElapsedMilliseconds}");
				File.WriteAllText(exitFlagPath, "exit");
				TryDelete(markerPath);
				TryDelete(framePath);
				TryDelete(cleanupPath);
				TryDelete(cancelledPath);
				XWScriptDebugHost debugHost = GetTree().Root.GetNodeOrNull<XWScriptDebugHost>("ModEditorScriptDebugHost");
				await WaitFrames(2);
				_debugHostIdleProcess = GodotObject.IsInstanceValid(debugHost) && debugHost.OwnedCleanupCount == 0 && !debugHost.IsProcessing();
				int queuedDispatchThread = 0;
				bool processingDuringDispatch = false;
				Stopwatch dispatchWatch = Stopwatch.StartNew();
				Task queuedDispatch = Task.Run(async () =>
				{
					await debugHost.DispatchAsync(() =>
					{
						queuedDispatchThread = System.Environment.CurrentManagedThreadId;
						processingDuringDispatch = debugHost.IsProcessing();
					});
				});
				bool queuedDispatchCompleted = await WaitForCondition(() => queuedDispatch.IsCompleted, 120);
				if (queuedDispatchCompleted)
				{
					await queuedDispatch;
				}
				_debugHostQueuedDispatch = ((queuedDispatchCompleted && queuedDispatchThread == debugHost.MainThreadId) & processingDuringDispatch) && dispatchWatch.ElapsedMilliseconds < 2000;
				int previewRootsBeforeExitEditor = debugHost?.PreviewRootCount ?? (-1);
				XWScriptEditor exitEditor = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ScriptEditor/GUI/XWScriptEditor.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<XWScriptEditor>(PackedScene.GenEditState.Disabled);
				if (GodotObject.IsInstanceValid(exitEditor))
				{
					AddChild(exitEditor, forceReadableName: false, InternalMode.Disabled);
				}
				await WaitFrames(2);
				bool flag4 = ((!GodotObject.IsInstanceValid(exitEditor)) ? null : (await exitEditor.StartDebugSessionAsync()))?.Success ?? false;
				if (flag4)
				{
					flag4 = await WaitForFile(framePath, 90);
				}
				bool exitMounted = flag4 && exitEditor.DebugPreviewChildCount == 1;
				exitEditor?.QueueFree();
				await WaitFrames(1);
				_debugHostCleanupProcess = GodotObject.IsInstanceValid(debugHost) && debugHost.OwnedCleanupCount > 0 && debugHost.IsProcessing();
				await WaitFrames(1);
				bool exitCleaned = await WaitForCondition(() => GodotObject.IsInstanceValid(debugHost) && debugHost.OwnedCleanupCount == 0 && debugHost.PreviewRootCount == previewRootsBeforeExitEditor && File.Exists(exitCleanupPath), 240);
				await WaitFrames(2);
				_debugHostSettledIdle = GodotObject.IsInstanceValid(debugHost) && debugHost.OwnedCleanupCount == 0 && !debugHost.IsProcessing();
				debugExitCleanup = (exitMounted & exitCleaned) && File.ReadAllText(exitCleanupPath) == "clean";
				TryDelete(exitFlagPath);
				debug = (debugConcurrentDenied & running & cooperativeStopped & failedEntrySafe & nonCooperativeDenied & debugMainThread & debugPreviewFrames & debugExitCleanup) && _debugHostIdleProcess && _debugHostQueuedDispatch && _debugHostCleanupProcess && _debugHostSettledIdle;
				Require(debug, "Debug did not build, load, invoke ModEditorDebugEntry, then unload the real assembly.");
				concurrentDenied = concurrentDenied & debugConcurrentDenied & nonCooperativeDenied;
				GD.Print("[MOD_EDITOR_SCRIPT_IDE_STAGE] debug-ready");
				(layout1000, actionsCallable, hiddenIdle) = await VerifyResponsiveWorkbench();
				Require(layout1000, "Script IDE toolbar or search/replace controls clipped at 1000px available width.");
				Require(actionsCallable, "Responsive toolbar lost one or more original Script IDE actions.");
				Require(hiddenIdle, "Hidden Script IDE retained processing or pending UI timers.");
				GodotObject inspector = XWEditorInterface.Instance.GetInspector();
				inspectorUntouched = inspectorBefore == inspector && (!GodotObject.IsInstanceValid(inspectorBefore) || inspector.GetInstanceId() == inspectorId);
				Require(inspectorUntouched, "Script IDE actions changed or replaced the Inspector surface.");
				responsive = responsive && _maxFrameIntervalMilliseconds < 1000.0;
				XWCSharpIdeService.IdeMetrics metrics = XWCSharpIdeService.GetMetrics();
				Require(metrics.MetadataBuildRuns == 1, $"Metadata references rebuilt {metrics.MetadataBuildRuns} times.");
				Require(metrics.SemanticCacheHits > 0, "Consecutive semantic queries did not reuse the cached compilation.");
				Require(responsive, $"Main-thread frame gap reached {_maxFrameIntervalMilliseconds:F1}ms.");
				GD.Print($"[MOD_EDITOR_SCRIPT_IDE_PERF] semanticBuilds={metrics.SemanticBuildRuns} semanticHits={metrics.SemanticCacheHits} metadataBuilds={metrics.MetadataBuildRuns} maxFrameMs={_maxFrameIntervalMilliseconds:F1}");
				Button nodeOrNull2 = _editor.GetNodeOrNull<Button>("%DefinitionButton");
				Button nodeOrNull3 = _editor.GetNodeOrNull<Button>("%ReferencesButton");
				Button nodeOrNull4 = _editor.GetNodeOrNull<Button>("%RenameButton");
				Button nodeOrNull5 = _editor.GetNodeOrNull<Button>("%FormatButton");
				Button nodeOrNull6 = _editor.GetNodeOrNull<Button>("%QuickFixButton");
				Button nodeOrNull7 = _editor.GetNodeOrNull<Button>("%DebugStartButton");
				Require(GodotObject.IsInstanceValid(nodeOrNull2) && nodeOrNull2.Icon != null && nodeOrNull3?.Icon != null && nodeOrNull4?.Icon != null && nodeOrNull5?.Icon != null && nodeOrNull6?.Icon != null && nodeOrNull7?.Icon != null, "Game-style visual IDE actions or their icons are missing.");
			}
			catch (Exception ex3)
			{
				_failures.Add(ex3.ToString());
			}
		}
		finally
		{
			_monitorFrames = false;
			try
			{
				if (_editor != null)
				{
					await _editor.StopDebugSessionAsync();
				}
			}
			catch
			{
			}
			try
			{
				if (!string.IsNullOrWhiteSpace(_probeParent) && Directory.Exists(_probeParent))
				{
					Directory.Delete(_probeParent, recursive: true);
				}
			}
			catch (Exception ex4)
			{
				_failures.Add("Cleanup failed: " + ex4.Message);
			}
		}
		Finish(definition, references, rename, hover, signature, format, quickFix, breakpoint, debug, debugMainThread, debugPreviewFrames, debugExitCleanup, undoRedo, savedReloaded, responsive, layout1000, actionsCallable, hiddenIdle, inspectorUntouched, pathDenied, concurrentDenied, semanticRaceSafe, renameRaceSafe);
	}

	private async Task<(bool SemanticRaceSafe, bool RenameRaceSafe)> VerifySemanticRequestStamps(ModEditorPanel panel, System.Reflection.MethodInfo enterProject, ModProject originalProject, string originalRoot, string consumerPath, string definitionPath, string quickFixPath, string consumerSource, string definitionSource)
	{
		bool definitionStaleDenied = false;
		bool referencesStaleDenied = false;
		bool hoverStaleDenied = false;
		bool projectStaleDenied = false;
		bool renameLatestWins = false;
		try
		{
			_editor.OpenFile(consumerPath);
			await WaitFrames(2);
			SetCaretAt("HeroService", 1);
			TaskCompletionSource<bool> definitionEntered = NewGate();
			TaskCompletionSource<bool> definitionRelease = NewGate();
			_editor.IdeResultGateForProbeAsync = (XWScriptEditor.IdeRequestKind kind, string _) =>
			{
				if (kind != XWScriptEditor.IdeRequestKind.Definition)
				{
					return Task.CompletedTask;
				}
				definitionEntered.TrySetResult(result: true);
				return definitionRelease.Task;
			};
			Task<bool> staleDefinitionTask = _editor.NavigateToDefinitionAtCaretAsync();
			bool definitionReachedGate = await WaitForCondition(() => definitionEntered.Task.IsCompleted, 600);
			_editor.OpenFile(quickFixPath);
			await WaitFrames(2);
			definitionRelease.TrySetResult(result: true);
			bool flag = await staleDefinitionTask;
			definitionStaleDenied = definitionReachedGate && !flag && SamePath(_editor.GetCurrentFilePath(), quickFixPath);
			_editor.OpenFile(consumerPath);
			await WaitFrames(2);
			SetCaretAt("HeroService", 1);
			Window referencesWindow = _editor.GetNodeOrNull<Window>("%ReferencesWindow");
			ItemList referencesList = _editor.GetNodeOrNull<ItemList>("%ReferencesList");
			referencesWindow?.Hide();
			referencesList?.Clear();
			TaskCompletionSource<bool> referencesEntered = NewGate();
			TaskCompletionSource<bool> referencesRelease = NewGate();
			_editor.IdeResultGateForProbeAsync = (XWScriptEditor.IdeRequestKind kind, string _) =>
			{
				if (kind != XWScriptEditor.IdeRequestKind.References)
				{
					return Task.CompletedTask;
				}
				referencesEntered.TrySetResult(result: true);
				return referencesRelease.Task;
			};
			Task<int> staleReferencesTask = _editor.ShowReferencesAtCaretAsync();
			bool referencesReachedGate = await WaitForCondition(() => referencesEntered.Task.IsCompleted, 600);
			_codeEdit.SetAllText(consumerSource + "// stamp invalidation\n");
			_codeEdit.EmitSignal(TextEdit.SignalName.TextChanged);
			await WaitFrames(1);
			referencesRelease.TrySetResult(result: true);
			int num = await staleReferencesTask;
			int num2;
			if (referencesReachedGate && num == 0)
			{
				if (referencesWindow == null || !referencesWindow.Visible)
				{
					num2 = ((referencesList != null && referencesList.ItemCount == 0) ? 1 : 0);
					goto IL_066e;
				}
			}
			num2 = 0;
			goto IL_066e;
			IL_066e:
			referencesStaleDenied = (byte)num2 != 0;
			_codeEdit.SetAllText(consumerSource);
			_codeEdit.EmitSignal(TextEdit.SignalName.TextChanged);
			_editor.SaveFile();
			_editor.OpenFile(consumerPath);
			await WaitFrames(2);
			SetCaretAt("Compute", 1);
			PopupPanel symbolPopup = _editor.GetNodeOrNull<PopupPanel>("%SymbolInfoPopup");
			symbolPopup?.Hide();
			int caretLine = _codeEdit.GetCaretLine();
			int caretColumn = _codeEdit.GetCaretColumn();
			TaskCompletionSource<bool> hoverEntered = NewGate();
			TaskCompletionSource<bool> hoverRelease = NewGate();
			_editor.IdeResultGateForProbeAsync = (XWScriptEditor.IdeRequestKind kind, string _) =>
			{
				if (kind != XWScriptEditor.IdeRequestKind.HoverPopup)
				{
					return Task.CompletedTask;
				}
				hoverEntered.TrySetResult(result: true);
				return hoverRelease.Task;
			};
			Task<bool> staleHoverTask = _editor.RequestHoverForProbeAsync(caretLine, caretColumn);
			bool hoverReachedGate = await WaitForCondition(() => hoverEntered.Task.IsCompleted, 600);
			_editor.OpenFile(quickFixPath);
			await WaitFrames(2);
			hoverRelease.TrySetResult(result: true);
			bool flag2 = await staleHoverTask;
			int num3;
			if (hoverReachedGate && !flag2)
			{
				if (symbolPopup == null || !symbolPopup.Visible)
				{
					num3 = (SamePath(_editor.GetCurrentFilePath(), quickFixPath) ? 1 : 0);
					goto IL_095b;
				}
			}
			num3 = 0;
			goto IL_095b;
			IL_095b:
			hoverStaleDenied = (byte)num3 != 0;
			_editor.OpenFile(consumerPath);
			await WaitFrames(2);
			SetCaretAt("HeroService", 1);
			TaskCompletionSource<bool> projectEntered = NewGate();
			TaskCompletionSource<bool> projectRelease = NewGate();
			_editor.IdeResultGateForProbeAsync = (XWScriptEditor.IdeRequestKind kind, string _) =>
			{
				if (kind != XWScriptEditor.IdeRequestKind.Definition)
				{
					return Task.CompletedTask;
				}
				projectEntered.TrySetResult(result: true);
				return projectRelease.Task;
			};
			Task<bool> projectDefinitionTask = _editor.NavigateToDefinitionAtCaretAsync();
			bool projectReachedGate = await WaitForCondition(() => projectEntered.Task.IsCompleted, 600);
			ModProject otherProject = ModProject.Create(_probeParent, "StampOther" + Guid.NewGuid().ToString("N"), "1.0.0", "probe", "semantic stamp boundary");
			string otherScriptPath = "";
			if (otherProject != null)
			{
				string text = Path.Combine(otherProject.ProjectPath, "Scripts");
				Directory.CreateDirectory(text);
				otherScriptPath = Path.Combine(text, "OtherProject.cs");
				File.WriteAllText(otherScriptPath, "public class OtherProject { }\n");
				XWModManifestSyncService.SyncProject(otherProject.ProjectPath);
			}
			int num4;
			object obj;
			if (otherProject != null)
			{
				obj = enterProject?.Invoke(panel, new object[1] { otherProject });
				num4 = ((obj is bool && (bool)obj) ? 1 : 0);
			}
			else
			{
				num4 = 0;
			}
			bool switchedProject = (byte)num4 != 0;
			await WaitFrames(4);
			projectRelease.TrySetResult(result: true);
			bool flag3 = await projectDefinitionTask;
			projectStaleDenied = (projectReachedGate & switchedProject) && !flag3 && SamePath(panel.GetCurrentProject()?.ProjectPath, otherProject?.ProjectPath) && !SamePath(_editor.GetCurrentFilePath(), definitionPath);
			obj = enterProject?.Invoke(panel, new object[1] { originalProject });
			bool restoredProject = obj is bool && (bool)obj;
			XWEditorInterface.Instance?.FocusPanel("script_editor");
			await WaitFrames(6);
			Require(restoredProject && SamePath(panel.GetCurrentProject()?.ProjectPath, originalRoot), "Semantic stamp probe could not restore the original Mod project.");
			_editor.OpenFile(definitionPath);
			await WaitFrames(2);
			SetCaretAt("HeroService", 1);
			TaskCompletionSource<bool> renameEntered = NewGate();
			TaskCompletionSource<bool> renameRelease = NewGate();
			_editor.IdeResultGateForProbeAsync = (XWScriptEditor.IdeRequestKind kind, string discriminator) =>
			{
				if (kind != XWScriptEditor.IdeRequestKind.RenamePreview || !string.Equals(discriminator, "OldRaceService", StringComparison.Ordinal))
				{
					return Task.CompletedTask;
				}
				renameEntered.TrySetResult(result: true);
				return renameRelease.Task;
			};
			Task<XWCSharpIdeService.RenamePreview> oldRenameTask = _editor.PreviewRenameAtCaretAsync("OldRaceService");
			bool renameReachedGate = await WaitForCondition(() => renameEntered.Task.IsCompleted, 600);
			XWCSharpIdeService.RenamePreview renamePreview = await _editor.PreviewRenameAtCaretAsync("LatestRaceService");
			bool latestPendingBeforeRelease = renamePreview != null && renamePreview.IsValid && _editor.LastRenamePreview?.NewName == "LatestRaceService";
			renameRelease.TrySetResult(result: true);
			bool latestPendingAfterRelease = await oldRenameTask == null && _editor.LastRenamePreview?.NewName == "LatestRaceService";
			bool flag4 = await _editor.ApplyPendingRenameAsync();
			string text2 = File.ReadAllText(definitionPath);
			string text3 = File.ReadAllText(consumerPath);
			bool onlyLatestWritten = flag4 && text2.Contains("LatestRaceService", StringComparison.Ordinal) && text3.Contains("LatestRaceService", StringComparison.Ordinal) && !text2.Contains("OldRaceService", StringComparison.Ordinal) && !text3.Contains("OldRaceService", StringComparison.Ordinal);
			int num5;
			if (flag4)
			{
				obj = enterProject?.Invoke(panel, new object[1] { otherProject });
				num5 = ((obj is bool && (bool)obj) ? 1 : 0);
			}
			else
			{
				num5 = 0;
			}
			bool switchedToForeignForUndo = (byte)num5 != 0;
			XWEditorInterface.Instance?.FocusPanel("script_editor");
			await WaitFrames(5);
			if (switchedToForeignForUndo)
			{
				_editor.OpenFile(otherScriptPath);
			}
			await WaitFrames(2);
			bool flag5 = switchedToForeignForUndo;
			if (flag5)
			{
				flag5 = !(await _editor.UndoLastProjectRenameAsync());
			}
			bool foreignUndoDenied = flag5 && File.ReadAllText(definitionPath).Contains("LatestRaceService", StringComparison.Ordinal) && File.ReadAllText(consumerPath).Contains("LatestRaceService", StringComparison.Ordinal);
			obj = enterProject?.Invoke(panel, new object[1] { originalProject });
			bool switchedBackForOwnerUndo = obj is bool && (bool)obj;
			XWEditorInterface.Instance?.FocusPanel("script_editor");
			await WaitFrames(5);
			if (switchedBackForOwnerUndo)
			{
				_editor.OpenFile(definitionPath);
			}
			await WaitFrames(2);
			flag5 = switchedBackForOwnerUndo;
			if (flag5)
			{
				flag5 = await _editor.UndoLastProjectRenameAsync();
			}
			bool flag6 = flag5;
			renameLatestWins = (renameReachedGate & latestPendingBeforeRelease & latestPendingAfterRelease & onlyLatestWritten & foreignUndoDenied & flag6) && File.ReadAllText(definitionPath) == definitionSource && File.ReadAllText(consumerPath) == consumerSource;
		}
		finally
		{
			_editor.IdeResultGateForProbeAsync = null;
			if (!SamePath(panel.GetCurrentProject()?.ProjectPath, originalRoot))
			{
				enterProject?.Invoke(panel, new object[1] { originalProject });
				XWEditorInterface.Instance?.FocusPanel("script_editor");
				await WaitFrames(4);
			}
			_editor.OpenFile(consumerPath);
			await WaitFrames(2);
		}
		GD.Print($"[MOD_EDITOR_SCRIPT_IDE_STAMP] definition={definitionStaleDenied} references={referencesStaleDenied} hover={hoverStaleDenied} project={projectStaleDenied} renameLatestWins={renameLatestWins}");
		return (SemanticRaceSafe: definitionStaleDenied & referencesStaleDenied & hoverStaleDenied & projectStaleDenied, RenameRaceSafe: renameLatestWins);
	}

	private async Task<(bool Layout, bool Actions, bool HiddenIdle)> VerifyResponsiveWorkbench()
	{
		SubViewport viewport = null;
		XWScriptEditor responsiveEditor = null;
		(bool Layout, bool Actions, bool HiddenIdle) result;
		try
		{
			viewport = new SubViewport
			{
				Name = "ScriptResponsiveViewport",
				Size = new Vector2I(1000, 720),
				Disable3D = true,
				RenderTargetUpdateMode = SubViewport.UpdateMode.Disabled
			};
			AddChild(viewport, forceReadableName: false, InternalMode.Disabled);
			responsiveEditor = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ScriptEditor/GUI/XWScriptEditor.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<XWScriptEditor>(PackedScene.GenEditState.Disabled);
			Require(GodotObject.IsInstanceValid(responsiveEditor), "Responsive Script IDE scene could not be instantiated.");
			Control menuBar;
			Control findRow;
			Control replaceRow;
			XWCodeEdit codeEdit;
			int num;
			if (GodotObject.IsInstanceValid(responsiveEditor))
			{
				viewport.AddChild(responsiveEditor, forceReadableName: false, InternalMode.Disabled);
				responsiveEditor.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect, Control.LayoutPresetMode.Minsize);
				await WaitFrames(4);
				menuBar = responsiveEditor.GetNodeOrNull<Control>("MainVBox/MenuBar");
				findRow = responsiveEditor.GetNodeOrNull<Control>("MainVBox/ContentSplit/RightPanel/SearchBar/SearchMargin/SearchRows/FindRow");
				replaceRow = responsiveEditor.GetNodeOrNull<Control>("%ReplaceRow");
				PanelContainer nodeOrNull = responsiveEditor.GetNodeOrNull<PanelContainer>("%SearchBar");
				codeEdit = responsiveEditor.GetNodeOrNull<XWCodeEdit>("%XWCodeEdit");
				nodeOrNull?.Show();
				replaceRow?.Show();
				await WaitFrames(4);
				if (GodotObject.IsInstanceValid(menuBar) && GodotObject.IsInstanceValid(findRow) && GodotObject.IsInstanceValid(replaceRow) && menuBar.Size.X >= 995f && ChildrenFitWithin(menuBar) && ChildrenFitWithin(findRow) && ChildrenFitWithin(replaceRow))
				{
					if (codeEdit != null && codeEdit.Size.X >= 500f)
					{
						num = ((codeEdit != null && codeEdit.Size.Y >= 300f) ? 1 : 0);
						goto IL_0326;
					}
				}
				num = 0;
				goto IL_0326;
			}
			result = (Layout: false, Actions: false, HiddenIdle: false);
			goto end_IL_0048;
			IL_0326:
			bool layout = (byte)num != 0;
			string[] array = new string[14]
			{
				"DefinitionButton", "ReferencesButton", "RenameButton", "RenameUndoButton", "RenameRedoButton", "FormatButton", "QuickFixButton", "CompileButton", "ValidateButton", "PreviousProblemButton",
				"NextProblemButton", "OpenBlueprintButton", "DebugStartButton", "DebugStopButton"
			};
			bool actions = true;
			string[] array2 = array;
			foreach (string text in array2)
			{
				actions &= GodotObject.IsInstanceValid(responsiveEditor.GetNodeOrNull<Button>("%" + text));
			}
			array2 = new string[7] { "DefinitionButton", "ReferencesButton", "RenameButton", "QuickFixButton", "CompileButton", "DebugStartButton", "DebugStopButton" };
			foreach (string text2 in array2)
			{
				Button nodeOrNull2 = responsiveEditor.GetNodeOrNull<Button>("%" + text2);
				actions &= nodeOrNull2 != null && nodeOrNull2.Visible && nodeOrNull2.Icon != null && !string.IsNullOrWhiteSpace(nodeOrNull2.TooltipText);
			}
			MenuButton nodeOrNull3 = responsiveEditor.GetNodeOrNull<MenuButton>("%OverflowMenuBtn");
			PopupMenu popup = nodeOrNull3?.GetPopup();
			int[] array3 = new int[7] { 0, 1, 3, 4, 6, 7, 9 };
			foreach (int expectedId in array3)
			{
				actions &= PopupContainsId(popup, expectedId);
			}
			actions &= nodeOrNull3 != null && nodeOrNull3.Visible && nodeOrNull3.Icon != null;
			codeEdit?.InsertTextAtCaret(" ");
			responsiveEditor.Hide();
			await WaitFrames(2);
			bool isHiddenWorkQuiescent = responsiveEditor.IsHiddenWorkQuiescent;
			GD.Print($"[MOD_EDITOR_SCRIPT_IDE_LAYOUT] width=1000 toolbarWidth={menuBar?.Size.X:0.0} toolbarHeight={menuBar?.Size.Y:0.0} findWidth={findRow?.Size.X:0.0} replaceWidth={replaceRow?.Size.X:0.0} codeWidth={codeEdit?.Size.X:0.0} layout={layout} actions={actions} hiddenIdle={isHiddenWorkQuiescent} hiddenState=({responsiveEditor.HiddenWorkDiagnostic})");
			result = (Layout: layout, Actions: actions, HiddenIdle: isHiddenWorkQuiescent);
			end_IL_0048:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(responsiveEditor))
			{
				responsiveEditor.QueueFree();
			}
			if (GodotObject.IsInstanceValid(viewport))
			{
				viewport.QueueFree();
			}
			await WaitFrames(2);
		}
		return result;
	}

	private static bool ChildrenFitWithin(Control container)
	{
		if (!GodotObject.IsInstanceValid(container) || container.Size.X <= 0f || container.Size.Y <= 0f)
		{
			return false;
		}
		foreach (Node child in container.GetChildren())
		{
			if (child is Control { Visible: not false } control && (control.Position.X < -0.5f || control.Position.Y < -0.5f || control.Position.X + control.Size.X > container.Size.X + 0.5f || control.Position.Y + control.Size.Y > container.Size.Y + 0.5f))
			{
				return false;
			}
		}
		return true;
	}

	private static bool PopupContainsId(PopupMenu popup, int expectedId)
	{
		if (!GodotObject.IsInstanceValid(popup))
		{
			return false;
		}
		for (int i = 0; i < popup.ItemCount; i++)
		{
			if (!popup.IsItemSeparator(i) && popup.GetItemId(i) == expectedId)
			{
				return true;
			}
		}
		return false;
	}

	private void SetCaretAt(string needle, int occurrence)
	{
		string text = _codeEdit.Text;
		int num = -1;
		int startIndex = 0;
		for (int i = 0; i < occurrence; i++)
		{
			num = text.IndexOf(needle, startIndex, StringComparison.Ordinal);
			if (num < 0)
			{
				throw new InvalidOperationException($"Could not find {needle} occurrence {occurrence}.");
			}
			startIndex = num + needle.Length;
		}
		int num2 = 0;
		int num3 = 0;
		for (int j = 0; j < num; j++)
		{
			if (text[j] == '\n')
			{
				num2++;
				num3 = 0;
			}
			else
			{
				num3++;
			}
		}
		_codeEdit.SetCaretLine(num2);
		_codeEdit.SetCaretColumn(num3 + 1);
	}

	private async Task<ModEditorPanel> WaitForPanel(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			ModEditorPanel modEditorPanel = XWEditorInterface.Instance?.GetEditorPanel() as ModEditorPanel;
			Node instance = modEditorPanel?.FindChild("LoadingOverlay", recursive: true, owned: false);
			if (GodotObject.IsInstanceValid(modEditorPanel) && !GodotObject.IsInstanceValid(instance))
			{
				return modEditorPanel;
			}
			await WaitFrames(1);
		}
		return null;
	}

	private async Task WaitFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task<bool> WaitForFile(string path, int maxFrames)
	{
		return await WaitForCondition(() => File.Exists(path), maxFrames);
	}

	private async Task<bool> WaitForCondition(Func<bool> condition, int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			if (condition())
			{
				return true;
			}
			await WaitFrames(1);
		}
		return condition();
	}

	private static TaskCompletionSource<bool> NewGate()
	{
		return new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
	}

	private static void TryDelete(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch
		{
		}
	}

	private async Task MonitorFrameIntervals()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		long previous = stopwatch.ElapsedTicks;
		while (_monitorFrames && GodotObject.IsInstanceValid(this))
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			long elapsedTicks = stopwatch.ElapsedTicks;
			double num = (double)(elapsedTicks - previous) * 1000.0 / (double)Stopwatch.Frequency;
			if (num > _maxFrameIntervalMilliseconds)
			{
				_maxFrameIntervalMilliseconds = num;
			}
			previous = elapsedTicks;
		}
	}

	private static bool SamePath(string left, string right)
	{
		try
		{
			return string.Equals(Path.GetFullPath(left), Path.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
		}
		catch
		{
			return false;
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
		}
	}

	private void Finish(bool definition, bool references, bool rename, bool hover, bool signature, bool format, bool quickFix, bool breakpoint, bool debug, bool debugMainThread, bool debugPreviewFrames, bool debugExitCleanup, bool undoRedo, bool savedReloaded, bool responsive, bool layout1000, bool actionsCallable, bool hiddenIdle, bool inspectorUntouched, bool pathDenied, bool concurrentDenied, bool semanticRaceSafe, bool renameRaceSafe)
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_SCRIPT_IDE_FAILURE] " + failure);
		}
		GD.Print($"[MOD_EDITOR_SCRIPT_IDE_PROBE] definition={definition} references={references} rename={rename} hover={hover} signature={signature} format={format} quickFix={quickFix} breakpoint={breakpoint} debug={debug} debugMainThread={debugMainThread} debugPreviewFrames={debugPreviewFrames} debugExitCleanup={debugExitCleanup} debugHostIdleProcess={_debugHostIdleProcess} debugHostQueuedDispatch={_debugHostQueuedDispatch} debugHostCleanupProcess={_debugHostCleanupProcess} debugHostSettledIdle={_debugHostSettledIdle} undoRedo={undoRedo} savedReloaded={savedReloaded} responsive={responsive} layout1000={layout1000} actionsCallable={actionsCallable} hiddenIdle={hiddenIdle} inspectorUntouched={inspectorUntouched} pathDenied={pathDenied} concurrentDenied={concurrentDenied} semanticRaceSafe={semanticRaceSafe} renameRaceSafe={renameRaceSafe} failures={_failures.Count}");
		int exitCode = ((_failures.Count != 0) ? 1 : 0);
		Console.Out.Flush();
		Console.Error.Flush();
		GetTree().Quit(exitCode);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(8)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ChildrenFitWithin, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.PopupContainsId, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "popup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "expectedId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.SetCaretAt, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "needle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "occurrence", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.TryDelete, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.SamePath, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Require, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Finish, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "references", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "rename", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "hover", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "signature", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "format", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "quickFix", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "breakpoint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "debug", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "debugMainThread", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "debugPreviewFrames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "debugExitCleanup", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "undoRedo", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "savedReloaded", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "responsive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "layout1000", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "actionsCallable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "hiddenIdle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "inspectorUntouched", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "pathDenied", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "concurrentDenied", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "semanticRaceSafe", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "renameRaceSafe", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ChildrenFitWithin && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ChildrenFitWithin(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.PopupContainsId && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(PopupContainsId(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.SetCaretAt && args.Count == 2)
		{
			SetCaretAt(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryDelete && args.Count == 1)
		{
			TryDelete(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SamePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SamePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 23)
		{
			Finish(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]), VariantUtils.ConvertTo<bool>(in args[8]), VariantUtils.ConvertTo<bool>(in args[9]), VariantUtils.ConvertTo<bool>(in args[10]), VariantUtils.ConvertTo<bool>(in args[11]), VariantUtils.ConvertTo<bool>(in args[12]), VariantUtils.ConvertTo<bool>(in args[13]), VariantUtils.ConvertTo<bool>(in args[14]), VariantUtils.ConvertTo<bool>(in args[15]), VariantUtils.ConvertTo<bool>(in args[16]), VariantUtils.ConvertTo<bool>(in args[17]), VariantUtils.ConvertTo<bool>(in args[18]), VariantUtils.ConvertTo<bool>(in args[19]), VariantUtils.ConvertTo<bool>(in args[20]), VariantUtils.ConvertTo<bool>(in args[21]), VariantUtils.ConvertTo<bool>(in args[22]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ChildrenFitWithin && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ChildrenFitWithin(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.PopupContainsId && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(PopupContainsId(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.TryDelete && args.Count == 1)
		{
			TryDelete(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SamePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SamePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.ChildrenFitWithin)
		{
			return true;
		}
		if (method == MethodName.PopupContainsId)
		{
			return true;
		}
		if (method == MethodName.SetCaretAt)
		{
			return true;
		}
		if (method == MethodName.TryDelete)
		{
			return true;
		}
		if (method == MethodName.SamePath)
		{
			return true;
		}
		if (method == MethodName.Require)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._probeParent)
		{
			_probeParent = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XWScriptEditor>(in value);
			return true;
		}
		if (name == PropertyName._codeEdit)
		{
			_codeEdit = VariantUtils.ConvertTo<XWCodeEdit>(in value);
			return true;
		}
		if (name == PropertyName._monitorFrames)
		{
			_monitorFrames = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._maxFrameIntervalMilliseconds)
		{
			_maxFrameIntervalMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._debugHostIdleProcess)
		{
			_debugHostIdleProcess = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._debugHostQueuedDispatch)
		{
			_debugHostQueuedDispatch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._debugHostCleanupProcess)
		{
			_debugHostCleanupProcess = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._debugHostSettledIdle)
		{
			_debugHostSettledIdle = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._probeParent)
		{
			value = VariantUtils.CreateFrom(in _probeParent);
			return true;
		}
		if (name == PropertyName._editor)
		{
			value = VariantUtils.CreateFrom(in _editor);
			return true;
		}
		if (name == PropertyName._codeEdit)
		{
			value = VariantUtils.CreateFrom(in _codeEdit);
			return true;
		}
		if (name == PropertyName._monitorFrames)
		{
			value = VariantUtils.CreateFrom(in _monitorFrames);
			return true;
		}
		if (name == PropertyName._maxFrameIntervalMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _maxFrameIntervalMilliseconds);
			return true;
		}
		if (name == PropertyName._debugHostIdleProcess)
		{
			value = VariantUtils.CreateFrom(in _debugHostIdleProcess);
			return true;
		}
		if (name == PropertyName._debugHostQueuedDispatch)
		{
			value = VariantUtils.CreateFrom(in _debugHostQueuedDispatch);
			return true;
		}
		if (name == PropertyName._debugHostCleanupProcess)
		{
			value = VariantUtils.CreateFrom(in _debugHostCleanupProcess);
			return true;
		}
		if (name == PropertyName._debugHostSettledIdle)
		{
			value = VariantUtils.CreateFrom(in _debugHostSettledIdle);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName._probeParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._codeEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._monitorFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Float, PropertyName._maxFrameIntervalMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._debugHostIdleProcess, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._debugHostQueuedDispatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._debugHostCleanupProcess, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._debugHostSettledIdle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._probeParent, Variant.From(in _probeParent));
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._codeEdit, Variant.From(in _codeEdit));
		info.AddProperty(PropertyName._monitorFrames, Variant.From(in _monitorFrames));
		info.AddProperty(PropertyName._maxFrameIntervalMilliseconds, Variant.From(in _maxFrameIntervalMilliseconds));
		info.AddProperty(PropertyName._debugHostIdleProcess, Variant.From(in _debugHostIdleProcess));
		info.AddProperty(PropertyName._debugHostQueuedDispatch, Variant.From(in _debugHostQueuedDispatch));
		info.AddProperty(PropertyName._debugHostCleanupProcess, Variant.From(in _debugHostCleanupProcess));
		info.AddProperty(PropertyName._debugHostSettledIdle, Variant.From(in _debugHostSettledIdle));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._probeParent, out var value))
		{
			_probeParent = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._editor, out var value2))
		{
			_editor = value2.As<XWScriptEditor>();
		}
		if (info.TryGetProperty(PropertyName._codeEdit, out var value3))
		{
			_codeEdit = value3.As<XWCodeEdit>();
		}
		if (info.TryGetProperty(PropertyName._monitorFrames, out var value4))
		{
			_monitorFrames = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._maxFrameIntervalMilliseconds, out var value5))
		{
			_maxFrameIntervalMilliseconds = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._debugHostIdleProcess, out var value6))
		{
			_debugHostIdleProcess = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._debugHostQueuedDispatch, out var value7))
		{
			_debugHostQueuedDispatch = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._debugHostCleanupProcess, out var value8))
		{
			_debugHostCleanupProcess = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._debugHostSettledIdle, out var value9))
		{
			_debugHostSettledIdle = value9.As<bool>();
		}
	}
}
