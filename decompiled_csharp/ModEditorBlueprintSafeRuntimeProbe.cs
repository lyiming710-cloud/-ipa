using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Blueprint;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

[ScriptPath("res://Tests/ModEditorBlueprintSafeRuntimeProbe.cs")]
public class ModEditorBlueprintSafeRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BuildSuccessfulGraph = "BuildSuccessfulGraph";

		public static readonly StringName BuildBudgetGraph = "BuildBudgetGraph";

		public static readonly StringName BuildLoadResourceGraph = "BuildLoadResourceGraph";

		public static readonly StringName BuildSelfAdapterGraph = "BuildSelfAdapterGraph";

		public static readonly StringName BuildDeniedMethodGraph = "BuildDeniedMethodGraph";

		public static readonly StringName BuildSelfCollectionMutationGraph = "BuildSelfCollectionMutationGraph";

		public static readonly StringName CreateGetPropertyNode = "CreateGetPropertyNode";

		public static readonly StringName CreateGetSelfNode = "CreateGetSelfNode";

		public static readonly StringName CreateSetPropertyNode = "CreateSetPropertyNode";

		public static readonly StringName CreatePropertyData = "CreatePropertyData";

		public static readonly StringName BuildCollectionGraph = "BuildCollectionGraph";

		public static readonly StringName AddLiteral = "AddLiteral";

		public static readonly StringName Add = "Add";

		public new static readonly StringName Connect = "Connect";

		public static readonly StringName FirstGraph = "FirstGraph";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _editor = "_editor";

		public static readonly StringName _graphEdit = "_graphEdit";

		public static readonly StringName _sandboxRoot = "_sandboxRoot";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWBPEditor _editor;

	private XWBPGraphEdit _graphEdit;

	private string _sandboxRoot = string.Empty;

	public override async void _Ready()
	{
		_ = 15;
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
			Require(GodotObject.IsInstanceValid(modEditorManager), "ModEditorManager could not be instantiated.");
			if (!GodotObject.IsInstanceValid(modEditorManager))
			{
				Finish();
				return;
			}
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
			bool flag = await WaitForEditor(900);
			Require(flag, "F3 did not initialize the real Blueprint editor within 900 frames.");
			if (!flag)
			{
				Finish();
				return;
			}
			bool window = FindAncestorWindow(_editor) != null;
			Require(window, "Blueprint editor is not mounted under the F3 ModEditor window.");
			bool startupValidation = await ProbeStartupValidation();
			XWBPScript xWBPScript = XWBPScript.Create();
			xWBPScript.ResourceName = "BlueprintSafeRuntimeProbe";
			_editor.Init(xWBPScript);
			await WaitFrames(4);
			XWBPGraphData graph = FirstGraph(_editor.BpScriptData);
			Require(graph != null, "Blueprint script did not create a graph for the safe-runtime probe.");
			if (graph == null)
			{
				Finish();
				return;
			}
			BuildSuccessfulGraph(graph);
			_graphEdit.Init(null);
			_graphEdit.Init(graph);
			await WaitFrames(4);
			Button button = _graphEdit.FindChild("RunPreviewButton", recursive: true, owned: false) as Button;
			Label statusLabel = _graphEdit.FindChild("PreviewStatusLabel", recursive: true, owned: false) as Label;
			bool button2 = GodotObject.IsInstanceValid(button) && GodotObject.IsInstanceValid(button.Icon) && !button.Disabled;
			bool status = GodotObject.IsInstanceValid(statusLabel) && statusLabel.Text.Contains("预览", StringComparison.Ordinal);
			Require(button2, "Blueprint safe preview does not expose an enabled visual play button with an icon.");
			Require(status, "Blueprint safe preview status pill is missing from the graph surface.");
			XWInspector inspector = XWEditorInterface.Instance?.GetInspector() as XWInspector;
			Node sentinel = new Node
			{
				Name = "BlueprintSafeRuntimeInspectorSentinel"
			};
			AddChild(sentinel, forceReadableName: false, InternalMode.Disabled);
			XWEditorInterface.Instance?.InspectObject(sentinel);
			(XWEditorInterface.Instance?.GetLayoutManager())?.FocusPanel("bp_editor");
			Variant originalFive = graph.GetNode(4).OutputPorts[0].Value;
			Variant originalBranch = graph.GetNode(2).InputPorts[1].Value;
			ulong framesBefore = Engine.GetProcessFrames();
			button?.EmitSignal(BaseButton.SignalName.Pressed);
			bool condition = await WaitForPreview(600);
			ulong processFrames = Engine.GetProcessFrames();
			Require(condition, "Visual Blueprint safe preview did not complete within 600 frames.");
			XWBlueprintRuntimeResult result = _editor.LastSafePreviewResult;
			bool output = result != null && result.Success && result.OutputMessages.SequenceEqual(new string[1] { "5" });
			bool trace = result != null && new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.All((int item) => result.TraceNodeIds.Contains(item));
			bool asyncRun = processFrames > framesBefore && result != null && result.StepCount >= 9;
			int nodeId = result?.TraceNodeIds.LastOrDefault() ?? (-1);
			XWBPGraphNode graphNode = _graphEdit.GetGraphNode(nodeId);
			bool highlighted = GodotObject.IsInstanceValid(graphNode) && graphNode.Selected && graphNode.Modulate != Colors.White;
			bool statusUpdated = GodotObject.IsInstanceValid(statusLabel) && statusLabel.Text.StartsWith("✓", StringComparison.Ordinal);
			bool inspectorUntouched = inspector == null || inspector.CurrentObject == sentinel;
			bool immutable = graph.GetNode(4).OutputPorts[0].Value.Equals(originalFive) && graph.GetNode(2).InputPorts[1].Value.Equals(originalBranch);
			Require(output, "Safe preview did not execute the true branch and print exactly 5.");
			Require(trace, "Safe preview trace did not include the entry, branch, value, math, and print nodes.");
			Require(asyncRun, "Safe preview did not yield frames while tracing the visual graph.");
			Require(highlighted, "Safe preview did not select and highlight the last traced graph node.");
			Require(statusUpdated, "Safe preview did not update the visual status pill after success.");
			Require(inspectorUntouched, "Safe preview replaced the raw Inspector object.");
			Require(immutable, "Safe preview mutated editable Blueprint node values.");
			_sandboxRoot = ProjectSettings.GlobalizePath($"user://BlueprintSafeAdapterProbe/{Guid.NewGuid():N}");
			Directory.CreateDirectory(Path.Combine(_sandboxRoot, "Resources"));
			string resourcePath = Path.Combine(_sandboxRoot, "Resources", "probe.tres");
			Error error = ResourceSaver.Save(new Resource
			{
				ResourceName = "safe_resource"
			}, resourcePath, ResourceSaver.SaverFlags.None);
			Require(error == Error.Ok, $"Could not save contained adapter probe resource: {error}.");
			string nestedRootPath = Path.Combine(_sandboxRoot, "Resources", "nested_root.tres");
			string path = Path.Combine(_sandboxRoot, "Resources", "nested_child.tres");
			string text = Path.Combine(_sandboxRoot, "Scripts");
			Directory.CreateDirectory(text);
			File.WriteAllText(Path.Combine(text, "evil.gd"), "extends Resource\n");
			File.WriteAllText(path, "[gd_resource type=\"Resource\" load_steps=2 format=3]\n\n[ext_resource type=\"Script\" path=\"../Scripts/evil.gd\" id=\"1\"]\n\n[resource]\nscript = ExtResource(\"1\")\n");
			File.WriteAllText(nestedRootPath, "[gd_resource type=\"Resource\" load_steps=2 format=3]\n\n[ext_resource type=\"Resource\" path=\"nested_child.tres\" id=\"1\"]\n\n[resource]\nchild = ExtResource(\"1\")\n");
			Godot.Collections.Array sourceBag = new Godot.Collections.Array { Variant.From<long>(1L) };
			System.Collections.Generic.Dictionary<string, Variant> selfProperties = new System.Collections.Generic.Dictionary<string, Variant>(StringComparer.Ordinal)
			{
				["score"] = Variant.From<long>(7L),
				["bag"] = Variant.From(in sourceBag)
			};
			XWBlueprintRuntimeContext context = BuildAdapterContext(_sandboxRoot, selfProperties);
			XWBlueprintRuntimeResult xWBlueprintRuntimeResult = await XWBlueprintSafeRuntime.ExecuteGraphAsync(BuildLoadResourceGraph(resourcePath), context);
			bool loadResource = xWBlueprintRuntimeResult != null && xWBlueprintRuntimeResult.Success && xWBlueprintRuntimeResult.OutputMessages.SequenceEqual(new string[1] { "safe_resource" });
			Require(loadResource, "Contained LoadResource adapter failed: " + xWBlueprintRuntimeResult?.StopReason);
			XWBlueprintRuntimeResult xWBlueprintRuntimeResult2 = await XWBlueprintSafeRuntime.ExecuteGraphAsync(BuildSelfAdapterGraph(), BuildAdapterContext(_sandboxRoot, selfProperties));
			bool getSelf = xWBlueprintRuntimeResult2?.Success ?? false;
			bool getSet = getSelf && xWBlueprintRuntimeResult2.OutputMessages.Count >= 1 && xWBlueprintRuntimeResult2.OutputMessages[0] == "9";
			bool callWhitelist = getSelf && xWBlueprintRuntimeResult2.OutputMessages.Count >= 2 && xWBlueprintRuntimeResult2.OutputMessages[1] == "true";
			bool isValid = getSelf && xWBlueprintRuntimeResult2.OutputMessages.Count >= 3 && xWBlueprintRuntimeResult2.OutputMessages[2] == "true";
			Require(getSelf, "GetSelf adapter graph failed: " + xWBlueprintRuntimeResult2?.StopReason);
			Require(getSet, "Whitelisted Get/SetProperty did not update only the isolated Self snapshot.");
			Require(callWhitelist, "Whitelisted CallMethod did not run the safe has_property adapter.");
			Require(isValid, "IsValid did not recognize the registered isolated Self adapter.");
			XWBlueprintRuntimeResult xWBlueprintRuntimeResult3 = await XWBlueprintSafeRuntime.ExecuteGraphAsync(BuildLoadResourceGraph("../outside.tres"), BuildAdapterContext(_sandboxRoot, selfProperties));
			bool pathDenied = xWBlueprintRuntimeResult3 != null && !xWBlueprintRuntimeResult3.Success && xWBlueprintRuntimeResult3.StopReason.Contains("escapes the active Mod project root", StringComparison.Ordinal);
			Require(pathDenied, "LoadResource did not reject a path escaping the active Mod project root.");
			XWBlueprintRuntimeResult xWBlueprintRuntimeResult4 = await XWBlueprintSafeRuntime.ExecuteGraphAsync(BuildLoadResourceGraph(nestedRootPath), BuildAdapterContext(_sandboxRoot, selfProperties));
			bool nestedDenied = xWBlueprintRuntimeResult4 != null && !xWBlueprintRuntimeResult4.Success && xWBlueprintRuntimeResult4.StopReason.Contains("Nested resource dependency", StringComparison.Ordinal) && xWBlueprintRuntimeResult4.StopReason.Contains("Script, assembly", StringComparison.Ordinal);
			Require(nestedDenied, "LoadResource did not recursively reject a script hidden in a nested text resource.");
			XWBlueprintRuntimeResult xWBlueprintRuntimeResult5 = await XWBlueprintSafeRuntime.ExecuteGraphAsync(BuildDeniedMethodGraph(), BuildAdapterContext(_sandboxRoot, selfProperties));
			bool methodDenied = xWBlueprintRuntimeResult5 != null && !xWBlueprintRuntimeResult5.Success && xWBlueprintRuntimeResult5.StopReason.Contains("not in the preview adapter whitelist", StringComparison.Ordinal) && xWBlueprintRuntimeResult5.StopReason.Contains("free", StringComparison.Ordinal);
			Require(methodDenied, "CallMethod did not reject a method outside the exact adapter whitelist.");
			XWBlueprintRuntimeResult xWBlueprintRuntimeResult6 = await XWBlueprintSafeRuntime.ExecuteGraphAsync(BuildSelfCollectionMutationGraph(), BuildAdapterContext(_sandboxRoot, selfProperties));
			bool flag2 = xWBlueprintRuntimeResult6 != null && xWBlueprintRuntimeResult6.Success && sourceBag.Count == 1 && sourceBag[0].AsInt64() == 1;
			Require(flag2, "Array mutation escaped the isolated Self-property snapshot.");
			Resource resource = ResourceLoader.Load<Resource>(resourcePath, "", ResourceLoader.CacheMode.Ignore);
			immutable = ((immutable && selfProperties["score"].AsInt64() == 7) & flag2) && GodotObject.IsInstanceValid(resource) && resource.ResourceName == "safe_resource";
			Require(immutable, "Adapter execution mutated its source dictionary, resource, or editable graph data.");
			XWBlueprintRuntimeResult xWBlueprintRuntimeResult7 = await XWBlueprintSafeRuntime.ExecuteGraphAsync(BuildBudgetGraph());
			bool iterationBudget = xWBlueprintRuntimeResult7 != null && !xWBlueprintRuntimeResult7.Success && xWBlueprintRuntimeResult7.StopReason.Contains("iteration preview limit", StringComparison.Ordinal);
			XWBPGraphData graph2 = new XWBPGraphData
			{
				Name = "StepBudgetProbe"
			};
			BuildSuccessfulGraph(graph2);
			XWBlueprintRuntimeResult xWBlueprintRuntimeResult8 = await XWBlueprintSafeRuntime.ExecuteGraphAsync(graph2, new XWBlueprintRuntimeContext
			{
				Limits = new XWBlueprintRuntimeLimits
				{
					MaxSteps = 2
				}
			});
			bool nodeBudget = xWBlueprintRuntimeResult8 != null && !xWBlueprintRuntimeResult8.Success && xWBlueprintRuntimeResult8.StopReason.Contains("step limit", StringComparison.Ordinal);
			XWBPGraphData graph3 = new XWBPGraphData
			{
				Name = "TimeBudgetProbe"
			};
			BuildSuccessfulGraph(graph3);
			XWBlueprintRuntimeResult xWBlueprintRuntimeResult9 = await XWBlueprintSafeRuntime.ExecuteGraphAsync(graph3, new XWBlueprintRuntimeContext
			{
				Limits = new XWBlueprintRuntimeLimits
				{
					MaxExecutionMilliseconds = 1
				},
				NodeVisitedAsync = async (int _) =>
				{
					await Task.Delay(10);
				}
			});
			bool flag3 = xWBlueprintRuntimeResult9 != null && !xWBlueprintRuntimeResult9.Success && xWBlueprintRuntimeResult9.StopReason.Contains("execution time limit", StringComparison.Ordinal);
			bool budget = iterationBudget & nodeBudget & flag3;
			Require(iterationBudget, "Oversized Blueprint loop was not stopped by the iteration budget.");
			Require(nodeBudget, "Blueprint execution was not stopped by the node-step budget.");
			Require(flag3, "Blueprint execution was not stopped by the wall-clock budget.");
			XWBlueprintRuntimeResult xWBlueprintRuntimeResult10 = await XWBlueprintSafeRuntime.ExecuteGraphAsync(BuildCollectionGraph());
			bool flag4 = xWBlueprintRuntimeResult10 != null && xWBlueprintRuntimeResult10.Success && xWBlueprintRuntimeResult10.OutputMessages.SequenceEqual(new string[2] { "5", "9" });
			Require(flag4, "Array/Dictionary mutation nodes did not preserve isolated runtime state across the flow.");
			GD.Print($"[MOD_EDITOR_BLUEPRINT_SAFE_RUNTIME_PROBE] window={window} button={button2} status={status} output={output} trace={trace} asyncRun={asyncRun} highlighted={highlighted} statusUpdated={statusUpdated} isValid={isValid} collections={flag4} nestedDenied={nestedDenied} loadResource={loadResource} getSelf={getSelf} getSet={getSet} callWhitelist={callWhitelist} pathDenied={pathDenied} methodDenied={methodDenied} immutable={immutable} budget={budget} inspectorUntouched={inspectorUntouched} startupValidation={startupValidation} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private async Task<bool> ProbeStartupValidation()
	{
		int failuresBefore = _failures.Count;
		XWBPScript xWBPScript = ResourceLoader.Load<XWBPScript>("res://Test/Fixtures/中文状态机蓝图示例Mod/Scripts/ExampleStartupBlueprint.tres", null, ResourceLoader.CacheMode.Reuse);
		Require(xWBPScript != null, "Startup validation fixture could not be loaded.");
		if (xWBPScript == null)
		{
			return false;
		}
		XWBPScriptData xWBPScriptData = xWBPScript.Deserialize();
		XWBPGraphData graph = FirstGraph(xWBPScriptData);
		Require(graph != null, "Startup validation fixture has no graph.");
		if (graph == null)
		{
			return false;
		}
		using XWBPValidator validator = new XWBPValidator(xWBPScriptData);
		validator.ValidateGraph(graph);
		Require(!validator.Results.Any((XWBPValidationResult result) => result.TypeError == XWBPValidationResult.ErrorType.MissingEntry || result.TypeError == XWBPValidationResult.ErrorType.OrphanNode), "OnStart fixture was rejected as missing an entry or containing unreachable nodes.");
		XWBlueprintRuntimeResult xWBlueprintRuntimeResult = await XWBlueprintSafeRuntime.ExecuteGraphAsync(graph);
		Require(xWBlueprintRuntimeResult.Success && xWBlueprintRuntimeResult.OutputMessages.SequenceEqual(new string[1] { "测试 Mod 蓝图已启动" }), "Startup fixture did not execute its expected print through safe preview.");
		Add(graph, new XWBPNodePrint().CreateNodeData(), 3, Vector2.Zero);
		validator.Results.Clear();
		validator.ValidateOrphanNodes(graph);
		Require(validator.Results.Any((XWBPValidationResult result) => result.TypeError == XWBPValidationResult.ErrorType.OrphanNode && result.NodeId == 3), "OnStart reachability did not diagnose the disconnected Print node.");
		Connect(graph, 2, 0, 3, 0);
		graph.Connections.Add(new XWBPNodeConnectionData(3, 0, 2, 0));
		graph.RebuildConnectionIndex();
		validator.Results.Clear();
		validator.ValidateDeadLoop(graph);
		Require(validator.Results.Any((XWBPValidationResult result) => result.TypeError == XWBPValidationResult.ErrorType.DeadLoop), "OnStart flow cycle escaped dead-loop validation.");
		graph.Nodes.Remove(1);
		validator.Results.Clear();
		validator.ValidateEntryNode(graph);
		Require(validator.Results.Any((XWBPValidationResult result) => result.TypeError == XWBPValidationResult.ErrorType.MissingEntry), "Removing OnStart did not restore the missing-entry error.");
		return _failures.Count == failuresBefore;
	}

	private static void BuildSuccessfulGraph(XWBPGraphData graph)
	{
		graph.Clear();
		Add(graph, new XWBPNodeOnStart().CreateNodeData(), 1, new Vector2(80f, 100f));
		Add(graph, new XWBPNodeBranch().CreateNodeData(), 2, new Vector2(300f, 100f));
		Add(graph, new XWBPNodeGreater().CreateNodeData(), 3, new Vector2(300f, 330f));
		AddLiteral(graph, 4, 5L, new Vector2(80f, 330f));
		AddLiteral(graph, 5, 3L, new Vector2(80f, 450f));
		Add(graph, new XWBPNodeAddInt().CreateNodeData(), 6, new Vector2(560f, 300f));
		AddLiteral(graph, 7, 2L, new Vector2(380f, 520f));
		AddLiteral(graph, 8, 3L, new Vector2(560f, 520f));
		Add(graph, new XWBPNodePrint().CreateNodeData(), 9, new Vector2(790f, 80f));
		Add(graph, new XWBPNodePrint().CreateNodeData(), 10, new Vector2(790f, 240f));
		AddLiteral(graph, 11, "bad", new Vector2(590f, 640f));
		Connect(graph, 1, 0, 2, 0);
		Connect(graph, 3, 0, 2, 1);
		Connect(graph, 4, 0, 3, 0);
		Connect(graph, 5, 0, 3, 1);
		Connect(graph, 2, 0, 9, 0);
		Connect(graph, 6, 0, 9, 1);
		Connect(graph, 7, 0, 6, 0);
		Connect(graph, 8, 0, 6, 1);
		Connect(graph, 2, 1, 10, 0);
		Connect(graph, 11, 0, 10, 1);
	}

	private static XWBPGraphData BuildBudgetGraph()
	{
		XWBPGraphData xWBPGraphData = new XWBPGraphData
		{
			Name = "BudgetProbe"
		};
		Add(xWBPGraphData, new XWBPNodeOnStart().CreateNodeData(), 101, Vector2.Zero);
		XWBPNodeData xWBPNodeData = new XWBPNodeForLoop().CreateNodeData();
		xWBPNodeData.InputPorts[1].Value = Variant.From<long>(0L);
		xWBPNodeData.InputPorts[2].Value = Variant.From<long>(1000L);
		Add(xWBPGraphData, xWBPNodeData, 102, Vector2.Zero);
		Connect(xWBPGraphData, 101, 0, 102, 0);
		return xWBPGraphData;
	}

	private static XWBlueprintRuntimeContext BuildAdapterContext(string modRoot, IReadOnlyDictionary<string, Variant> selfProperties)
	{
		return new XWBlueprintRuntimeContext
		{
			ModProjectRoot = modRoot,
			SelfClassName = "ProbeSelf",
			SelfProperties = selfProperties,
			BlueprintVariables = new System.Collections.Generic.Dictionary<int, Variant> { [17] = Variant.From<long>(4L) }
		};
	}

	private static XWBPGraphData BuildLoadResourceGraph(string resourcePath)
	{
		XWBPGraphData xWBPGraphData = new XWBPGraphData
		{
			Name = "LoadResourceAdapterProbe"
		};
		Add(xWBPGraphData, new XWBPNodeOnStart().CreateNodeData(), 201, Vector2.Zero);
		Add(xWBPGraphData, XWBPNodeLoadResource.CreateForPath(resourcePath).CreateNodeData(), 202, Vector2.Zero);
		Add(xWBPGraphData, CreateGetPropertyNode("Resource", "resource_name", Variant.Type.String), 203, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodePrint().CreateNodeData(), 204, Vector2.Zero);
		Connect(xWBPGraphData, 201, 0, 204, 0);
		Connect(xWBPGraphData, 202, 0, 203, 0);
		Connect(xWBPGraphData, 203, 0, 204, 1);
		return xWBPGraphData;
	}

	private static XWBPGraphData BuildSelfAdapterGraph()
	{
		XWBPGraphData xWBPGraphData = new XWBPGraphData
		{
			Name = "SelfAdapterProbe"
		};
		Add(xWBPGraphData, new XWBPNodeOnStart().CreateNodeData(), 211, Vector2.Zero);
		Add(xWBPGraphData, CreateSetPropertyNode("ProbeSelf", "score", Variant.Type.Int), 212, Vector2.Zero);
		Add(xWBPGraphData, CreateCallMethodNode("ProbeSelf", "has_property", Variant.Type.Bool, ("name", Variant.Type.String)), 213, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodePrint().CreateNodeData(), 214, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodePrint().CreateNodeData(), 215, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodePrint().CreateNodeData(), 216, Vector2.Zero);
		Add(xWBPGraphData, CreateGetSelfNode("ProbeSelf"), 217, Vector2.Zero);
		AddLiteral(xWBPGraphData, 218, 9L, Vector2.Zero);
		AddLiteral(xWBPGraphData, 219, "score", Vector2.Zero);
		Add(xWBPGraphData, CreateGetPropertyNode("ProbeSelf", "score", Variant.Type.Int), 220, Vector2.Zero);
		XWBPNodeData xWBPNodeData = new XWBPNodeIsValid().CreateNodeData();
		xWBPNodeData.InputPorts[0].ClassName = "ProbeSelf";
		Add(xWBPGraphData, xWBPNodeData, 221, Vector2.Zero);
		Connect(xWBPGraphData, 211, 0, 212, 0);
		Connect(xWBPGraphData, 212, 0, 213, 0);
		Connect(xWBPGraphData, 213, 0, 214, 0);
		Connect(xWBPGraphData, 214, 0, 215, 0);
		Connect(xWBPGraphData, 215, 0, 216, 0);
		Connect(xWBPGraphData, 217, 0, 212, 1);
		Connect(xWBPGraphData, 218, 0, 212, 2);
		Connect(xWBPGraphData, 217, 0, 213, 1);
		Connect(xWBPGraphData, 219, 0, 213, 2);
		Connect(xWBPGraphData, 217, 0, 220, 0);
		Connect(xWBPGraphData, 220, 0, 214, 1);
		Connect(xWBPGraphData, 213, 1, 215, 1);
		Connect(xWBPGraphData, 217, 0, 221, 0);
		Connect(xWBPGraphData, 221, 0, 216, 1);
		return xWBPGraphData;
	}

	private static XWBPGraphData BuildDeniedMethodGraph()
	{
		XWBPGraphData xWBPGraphData = new XWBPGraphData
		{
			Name = "DeniedMethodProbe"
		};
		Add(xWBPGraphData, new XWBPNodeOnStart().CreateNodeData(), 231, Vector2.Zero);
		Add(xWBPGraphData, CreateCallMethodNode("ProbeSelf", "free", Variant.Type.Nil), 232, Vector2.Zero);
		Add(xWBPGraphData, CreateGetSelfNode("ProbeSelf"), 233, Vector2.Zero);
		Connect(xWBPGraphData, 231, 0, 232, 0);
		Connect(xWBPGraphData, 233, 0, 232, 1);
		return xWBPGraphData;
	}

	private static XWBPGraphData BuildSelfCollectionMutationGraph()
	{
		XWBPGraphData xWBPGraphData = new XWBPGraphData
		{
			Name = "SelfCollectionCopyProbe"
		};
		Add(xWBPGraphData, new XWBPNodeOnStart().CreateNodeData(), 241, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodeArrayAdd().CreateNodeData(), 242, Vector2.Zero);
		Add(xWBPGraphData, CreateGetSelfNode("ProbeSelf"), 243, Vector2.Zero);
		Add(xWBPGraphData, CreateGetPropertyNode("ProbeSelf", "bag", Variant.Type.NodePath), 244, Vector2.Zero);
		AddLiteral(xWBPGraphData, 245, 2L, Vector2.Zero);
		Connect(xWBPGraphData, 241, 0, 242, 0);
		Connect(xWBPGraphData, 243, 0, 244, 0);
		Connect(xWBPGraphData, 244, 0, 242, 1);
		Connect(xWBPGraphData, 245, 0, 242, 2);
		return xWBPGraphData;
	}

	private static XWBPNodeData CreateGetPropertyNode(string className, string name, Variant.Type type)
	{
		XWBPNodeGetProperty xWBPNodeGetProperty = new XWBPNodeGetProperty();
		xWBPNodeGetProperty.MethodType = XWBPNodeGetProperty.Type.Script;
		xWBPNodeGetProperty.PropertyData = CreatePropertyData(className, name, type);
		xWBPNodeGetProperty.BuildProperty();
		return xWBPNodeGetProperty.CreateNodeData();
	}

	private static XWBPNodeData CreateGetSelfNode(string className)
	{
		XWBPNodeData xWBPNodeData = new XWBPNodeGetSelf().CreateNodeData();
		xWBPNodeData.OutputPorts[0].ClassName = className;
		return xWBPNodeData;
	}

	private static XWBPNodeData CreateSetPropertyNode(string className, string name, Variant.Type type)
	{
		XWBPNodeSetProperty xWBPNodeSetProperty = new XWBPNodeSetProperty();
		xWBPNodeSetProperty.MethodType = XWBPNodeSetProperty.Type.Script;
		xWBPNodeSetProperty.PropertyData = CreatePropertyData(className, name, type);
		xWBPNodeSetProperty.BuildProperty();
		return xWBPNodeSetProperty.CreateNodeData();
	}

	private static Dictionary CreatePropertyData(string className, string name, Variant.Type type)
	{
		return new Dictionary
		{
			["base_class_name"] = className,
			["name"] = name,
			["type"] = (int)type,
			["is_static"] = false
		};
	}

	private static XWBPNodeData CreateCallMethodNode(string className, string methodName, Variant.Type returnType, params (string Name, Variant.Type Type)[] arguments)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		for (int i = 0; i < arguments.Length; i++)
		{
			var (text, type) = arguments[i];
			array.Add(new Dictionary
			{
				["name"] = text,
				["type"] = (int)type
			});
		}
		Dictionary methodData = new Dictionary
		{
			["base_class_name"] = className,
			["name"] = methodName,
			["is_static"] = false,
			["args"] = array,
			["return"] = new Dictionary { ["type"] = (int)returnType }
		};
		XWBPNodeCallMethod xWBPNodeCallMethod = new XWBPNodeCallMethod();
		xWBPNodeCallMethod.MethodType = XWBPNodeCallMethod.Type.Script;
		xWBPNodeCallMethod.MethodData = methodData;
		xWBPNodeCallMethod.BuildMethod();
		return xWBPNodeCallMethod.CreateNodeData();
	}

	private static XWBPGraphData BuildCollectionGraph()
	{
		XWBPGraphData xWBPGraphData = new XWBPGraphData
		{
			Name = "CollectionProbe"
		};
		Add(xWBPGraphData, new XWBPNodeOnStart().CreateNodeData(), 301, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodeArrayAdd().CreateNodeData(), 302, Vector2.Zero);
		XWBPNodeData xWBPNodeData = new XWBPNodeMakeArray().CreateNodeData();
		xWBPNodeData.InputPorts[0].Value = Variant.From<long>(1L);
		xWBPNodeData.InputPorts[1].Value = Variant.From<long>(2L);
		xWBPNodeData.InputPorts[2].Value = Variant.From<long>(3L);
		xWBPNodeData.InputPorts[3].Value = Variant.From<long>(4L);
		Add(xWBPGraphData, xWBPNodeData, 303, Vector2.Zero);
		AddLiteral(xWBPGraphData, 304, 5L, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodeDictSet().CreateNodeData(), 305, Vector2.Zero);
		XWBPNodeData xWBPNodeData2 = new XWBPNodeMakeDict().CreateNodeData();
		xWBPNodeData2.InputPorts[0].Value = Variant.From<string>("base");
		xWBPNodeData2.InputPorts[1].Value = Variant.From<long>(5L);
		xWBPNodeData2.InputPorts[2].Value = Variant.From<string>("name");
		xWBPNodeData2.InputPorts[3].Value = Variant.From<string>("pea");
		Add(xWBPGraphData, xWBPNodeData2, 306, Vector2.Zero);
		AddLiteral(xWBPGraphData, 307, "score", Vector2.Zero);
		AddLiteral(xWBPGraphData, 308, 9L, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodePrint().CreateNodeData(), 309, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodeArrayLength().CreateNodeData(), 310, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodePrint().CreateNodeData(), 311, Vector2.Zero);
		Add(xWBPGraphData, new XWBPNodeDictGet().CreateNodeData(), 312, Vector2.Zero);
		Connect(xWBPGraphData, 301, 0, 302, 0);
		Connect(xWBPGraphData, 303, 0, 302, 1);
		Connect(xWBPGraphData, 304, 0, 302, 2);
		Connect(xWBPGraphData, 302, 0, 305, 0);
		Connect(xWBPGraphData, 306, 0, 305, 1);
		Connect(xWBPGraphData, 307, 0, 305, 2);
		Connect(xWBPGraphData, 308, 0, 305, 3);
		Connect(xWBPGraphData, 305, 0, 309, 0);
		Connect(xWBPGraphData, 310, 0, 309, 1);
		Connect(xWBPGraphData, 303, 0, 310, 0);
		Connect(xWBPGraphData, 309, 0, 311, 0);
		Connect(xWBPGraphData, 312, 0, 311, 1);
		Connect(xWBPGraphData, 306, 0, 312, 0);
		Connect(xWBPGraphData, 307, 0, 312, 1);
		return xWBPGraphData;
	}

	private static void AddLiteral(XWBPGraphData graph, int id, Variant value, Vector2 position)
	{
		XWBPNodeData xWBPNodeData = new XWBPNodeLiteral().CreateNodeData();
		xWBPNodeData.OutputPorts[0].Value = value;
		Add(graph, xWBPNodeData, id, position);
	}

	private static void Add(XWBPGraphData graph, XWBPNodeData node, int id, Vector2 position)
	{
		node.Id = id;
		node.Position = position;
		graph.AddNodePreserveId(node);
	}

	private static void Connect(XWBPGraphData graph, int fromNode, int fromPort, int toNode, int toPort)
	{
		if (!graph.AddConnection(fromNode, fromPort, toNode, toPort))
		{
			throw new InvalidOperationException($"Could not connect {fromNode}:{fromPort} to {toNode}:{toPort}.");
		}
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (XWEditorInterface.Instance?.GetBlueprintEditor() is XWBPEditor xWBPEditor && GodotObject.IsInstanceValid(xWBPEditor) && xWBPEditor.IsInsideTree())
			{
				_editor = xWBPEditor;
				_graphEdit = xWBPEditor.GetGraphEditor();
				return GodotObject.IsInstanceValid(_graphEdit) && _graphEdit.IsInsideTree();
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> WaitForPreview(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (!_editor.SafePreviewRunning && _editor.LastSafePreviewResult != null)
			{
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private static XWBPGraphData FirstGraph(XWBPScriptData data)
	{
		if (data == null)
		{
			return null;
		}
		using (System.Collections.Generic.Dictionary<int, XWBPGraphData>.ValueCollection.Enumerator enumerator = data.Graphs.Values.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		return null;
	}

	private static Window FindAncestorWindow(Node node)
	{
		Node node2 = node;
		while (GodotObject.IsInstanceValid(node2))
		{
			if (node2 is Window result)
			{
				return result;
			}
			node2 = node2.GetParent();
		}
		return null;
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
			GD.PrintErr("[MOD_EDITOR_BLUEPRINT_SAFE_RUNTIME_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_BLUEPRINT_SAFE_RUNTIME_PROBE_FAILURE] " + failure);
		}
		if (!string.IsNullOrWhiteSpace(_sandboxRoot) && Directory.Exists(_sandboxRoot))
		{
			try
			{
				Directory.Delete(_sandboxRoot, recursive: true);
			}
			catch (IOException)
			{
			}
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildSuccessfulGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graph", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildBudgetGraph, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.BuildLoadResourceGraph, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSelfAdapterGraph, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.BuildDeniedMethodGraph, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.BuildSelfCollectionMutationGraph, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateGetPropertyNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateGetSelfNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSetPropertyNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePropertyData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCollectionGraph, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.AddLiteral, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graph", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Add, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graph", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Connect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graph", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "fromNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FirstGraph, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindAncestorWindow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Window"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BuildSuccessfulGraph && args.Count == 1)
		{
			BuildSuccessfulGraph(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildBudgetGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildBudgetGraph());
			return true;
		}
		if (method == MethodName.BuildLoadResourceGraph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildLoadResourceGraph(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildSelfAdapterGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildSelfAdapterGraph());
			return true;
		}
		if (method == MethodName.BuildDeniedMethodGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildDeniedMethodGraph());
			return true;
		}
		if (method == MethodName.BuildSelfCollectionMutationGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildSelfCollectionMutationGraph());
			return true;
		}
		if (method == MethodName.CreateGetPropertyNode && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(CreateGetPropertyNode(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateGetSelfNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(CreateGetSelfNode(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateSetPropertyNode && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(CreateSetPropertyNode(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2])));
			return true;
		}
		if (method == MethodName.CreatePropertyData && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreatePropertyData(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2])));
			return true;
		}
		if (method == MethodName.BuildCollectionGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildCollectionGraph());
			return true;
		}
		if (method == MethodName.AddLiteral && args.Count == 4)
		{
			AddLiteral(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.Add && args.Count == 4)
		{
			Add(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]), VariantUtils.ConvertTo<XWBPNodeData>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.Connect && args.Count == 5)
		{
			Connect(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.FirstGraph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(FirstGraph(VariantUtils.ConvertTo<XWBPScriptData>(in args[0])));
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BuildSuccessfulGraph && args.Count == 1)
		{
			BuildSuccessfulGraph(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildBudgetGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildBudgetGraph());
			return true;
		}
		if (method == MethodName.BuildLoadResourceGraph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildLoadResourceGraph(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildSelfAdapterGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildSelfAdapterGraph());
			return true;
		}
		if (method == MethodName.BuildDeniedMethodGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildDeniedMethodGraph());
			return true;
		}
		if (method == MethodName.BuildSelfCollectionMutationGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildSelfCollectionMutationGraph());
			return true;
		}
		if (method == MethodName.CreateGetPropertyNode && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(CreateGetPropertyNode(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateGetSelfNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(CreateGetSelfNode(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateSetPropertyNode && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(CreateSetPropertyNode(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2])));
			return true;
		}
		if (method == MethodName.CreatePropertyData && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreatePropertyData(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2])));
			return true;
		}
		if (method == MethodName.BuildCollectionGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(BuildCollectionGraph());
			return true;
		}
		if (method == MethodName.AddLiteral && args.Count == 4)
		{
			AddLiteral(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.Add && args.Count == 4)
		{
			Add(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]), VariantUtils.ConvertTo<XWBPNodeData>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.Connect && args.Count == 5)
		{
			Connect(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.FirstGraph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(FirstGraph(VariantUtils.ConvertTo<XWBPScriptData>(in args[0])));
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.BuildSuccessfulGraph)
		{
			return true;
		}
		if (method == MethodName.BuildBudgetGraph)
		{
			return true;
		}
		if (method == MethodName.BuildLoadResourceGraph)
		{
			return true;
		}
		if (method == MethodName.BuildSelfAdapterGraph)
		{
			return true;
		}
		if (method == MethodName.BuildDeniedMethodGraph)
		{
			return true;
		}
		if (method == MethodName.BuildSelfCollectionMutationGraph)
		{
			return true;
		}
		if (method == MethodName.CreateGetPropertyNode)
		{
			return true;
		}
		if (method == MethodName.CreateGetSelfNode)
		{
			return true;
		}
		if (method == MethodName.CreateSetPropertyNode)
		{
			return true;
		}
		if (method == MethodName.CreatePropertyData)
		{
			return true;
		}
		if (method == MethodName.BuildCollectionGraph)
		{
			return true;
		}
		if (method == MethodName.AddLiteral)
		{
			return true;
		}
		if (method == MethodName.Add)
		{
			return true;
		}
		if (method == MethodName.Connect)
		{
			return true;
		}
		if (method == MethodName.FirstGraph)
		{
			return true;
		}
		if (method == MethodName.FindAncestorWindow)
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
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XWBPEditor>(in value);
			return true;
		}
		if (name == PropertyName._graphEdit)
		{
			_graphEdit = VariantUtils.ConvertTo<XWBPGraphEdit>(in value);
			return true;
		}
		if (name == PropertyName._sandboxRoot)
		{
			_sandboxRoot = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editor)
		{
			value = VariantUtils.CreateFrom(in _editor);
			return true;
		}
		if (name == PropertyName._graphEdit)
		{
			value = VariantUtils.CreateFrom(in _graphEdit);
			return true;
		}
		if (name == PropertyName._sandboxRoot)
		{
			value = VariantUtils.CreateFrom(in _sandboxRoot);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._graphEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._sandboxRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._graphEdit, Variant.From(in _graphEdit));
		info.AddProperty(PropertyName._sandboxRoot, Variant.From(in _sandboxRoot));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XWBPEditor>();
		}
		if (info.TryGetProperty(PropertyName._graphEdit, out var value2))
		{
			_graphEdit = value2.As<XWBPGraphEdit>();
		}
		if (info.TryGetProperty(PropertyName._sandboxRoot, out var value3))
		{
			_sandboxRoot = value3.As<string>();
		}
	}
}
