using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://Tests/ModEditorTutorialWorkbenchRuntimeProbe.cs")]
public class ModEditorTutorialWorkbenchRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindCustomSurface = "FindCustomSurface";

		public static readonly StringName FindSpinBox = "FindSpinBox";

		public static readonly StringName FindButtonByText = "FindButtonByText";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _editor = "_editor";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string DraftPath = "user://mod_editor_tutorial_workbench_probe.tres";

	private const string StandaloneStepPath = "user://mod_editor_tutorial_step_probe.tres";

	private const string StandaloneConditionPath = "user://mod_editor_tutorial_condition_probe.tres";

	private const string EditorDockKey = "tutorial_editor";

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWTextFlowVisualResourceEditor _editor;

	public override async void _Ready()
	{
		_ = 35;
		try
		{
			TutorialConfig draft = new TutorialConfig
			{
				ResourceName = "ModEditorTutorialWorkbenchProbe",
				saveKey = "ProbeTutorial"
			};
			Error error = ResourceSaver.Save(draft, "user://mod_editor_tutorial_workbench_probe.tres", ResourceSaver.SaverFlags.None);
			Require(error == Error.Ok, $"Could not create isolated tutorial draft: {error}.");
			if (error != Error.Ok)
			{
				Finish();
				return;
			}
			draft = ResourceLoader.Load<TutorialConfig>("user://mod_editor_tutorial_workbench_probe.tres", "", ResourceLoader.CacheMode.Replace);
			Require(GodotObject.IsInstanceValid(draft) && draft.step.Count == 0, "Empty tutorial draft did not reload.");
			if (!GodotObject.IsInstanceValid(draft))
			{
				Finish();
				return;
			}
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
			Require(flag, "F3 did not initialize the tutorial editor within 900 frames.");
			if (!flag)
			{
				Finish();
				return;
			}
			bool flag2 = await EnterEditorSurface(900);
			Require(flag2, "F3 editor did not finish loading its main editing surface.");
			if (!flag2)
			{
				Finish();
				return;
			}
			XWEditorInterface.Instance.EditResource(draft, XWResourceEditContext.ForRoot(draft, "user://mod_editor_tutorial_workbench_probe.tres", "tutorial_editor"));
			XWEditorInterface.Instance.FocusPanel("tutorial_editor");
			XWTutorialInlineEditor inline = await WaitForInlineEditor(900);
			Require(GodotObject.IsInstanceValid(inline), "Tutorial game-surface editor did not mount.");
			if (!GodotObject.IsInstanceValid(inline))
			{
				Finish();
				return;
			}
			VBoxContainer vBoxContainer = _editor.FindChild("EmbeddedInspectorHost", recursive: true, owned: false) as VBoxContainer;
			Control control = _editor.FindChild("DirectPropertySurface", recursive: true, owned: false) as Control;
			bool inspectorHidden = GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() == 0 && (!GodotObject.IsInstanceValid(control) || !control.Visible);
			Require(inspectorHidden, "Tutorial workbench restored a raw or duplicate generic Inspector surface.");
			XWUndoRedoManager undoRedo = XWEditorInterface.Instance.GetUndoRedoManager();
			undoRedo.ClearHistory();
			Json jsonSource = new Json();
			Error jsonParse = jsonSource.Parse("{\"SaveKey\":\"ProbeJsonTutorial\",\"Step\":[{\"BroadCast\":{\"Text\":\"JSON 教程步骤\",\"Time\":2.5},\"Condition\":[]}]}");
			inline.JsonSourcePicker?.SetEditedResource(jsonSource);
			inline.JsonSourcePicker?.EmitSignal(XWResourcePicker.SignalName.ResourceChanged, jsonSource);
			await WaitFrames(3);
			bool jsonApplied = jsonParse == Error.Ok && draft.data == jsonSource && draft.saveKey == "ProbeJsonTutorial" && draft.step.Count == 1 && draft.step[0].broadCastConfig?.broadCastString == "JSON 教程步骤";
			bool undoJson = undoRedo.Undo();
			await WaitFrames(2);
			bool jsonUndone = undoJson && draft.data == null && draft.saveKey == "ProbeTutorial" && draft.step.Count == 0;
			bool redoJson = undoRedo.Redo();
			await WaitFrames(2);
			bool jsonRedone = redoJson && draft.data == jsonSource && draft.step.Count == 1;
			bool restoreEmpty = undoRedo.Undo();
			await WaitFrames(2);
			bool jsonSourceWorkflow = (jsonApplied & jsonUndone & jsonRedone & restoreEmpty) && draft.step.Count == 0;
			Require(jsonSourceWorkflow, "Tutorial JSON source replacement did not preserve root snapshots through Undo/Redo.");
			undoRedo.ClearHistory();
			bool addStep = inline.AddStep() && draft.step.Count == 1 && inline.StepCount == 1;
			bool undoAdd = undoRedo.Undo();
			await WaitFrames(2);
			bool undoAddPassed = undoAdd && draft.step.Count == 0 && inline.StepCount == 0;
			bool redoAdd = undoRedo.Redo();
			await WaitFrames(2);
			bool redoAddPassed = redoAdd && draft.step.Count == 1 && inline.StepCount == 1;
			Require(addStep & undoAddPassed & redoAddPassed, "Empty tutorial add-step did not survive global Undo/Redo.");
			LineEdit saveKey = inline.FindChild("SaveKeyEdit", recursive: true, owned: false) as LineEdit;
			if (GodotObject.IsInstanceValid(saveKey))
			{
				saveKey.GrabFocus();
				saveKey.Text = "ProbeTutorialEdited";
				await WaitFrames(1);
				saveKey.ReleaseFocus();
				await WaitFrames(2);
			}
			bool inlineRootText = GodotObject.IsInstanceValid(saveKey) && draft.saveKey == "ProbeTutorialEdited";
			Require(inlineRootText, "Tutorial saveKey was not editable in the game-surface header.");
			TutorialStepConfig first = draft.step[0];
			inline.SetBroadcastText("第一步：点击草坪");
			bool broadcastEdited = first.broadCastConfig?.broadCastString == "第一步：点击草坪";
			bool undoBroadcast = undoRedo.Undo();
			await WaitFrames(2);
			bool broadcastRestored = first.broadCastConfig?.broadCastString != "第一步：点击草坪";
			bool redoBroadcast = undoRedo.Redo();
			await WaitFrames(2);
			bool broadcastRedone = first.broadCastConfig?.broadCastString == "第一步：点击草坪";
			Require(broadcastEdited & undoBroadcast & broadcastRestored & redoBroadcast & broadcastRedone, "Inline broadcast text did not participate in global Undo/Redo.");
			bool duplicated = inline.DuplicateStep();
			await WaitFrames(2);
			TutorialStepConfig second = ((draft.step.Count > 1) ? draft.step[1] : null);
			inline.SetBroadcastText("第二步：收集阳光");
			bool moved = inline.MoveStepUp();
			await WaitFrames(2);
			bool movePassed = moved && draft.step.Count == 2 && draft.step[0] == second && inline.CurrentStepIndex == 0 && second?.broadCastConfig?.broadCastString == "第二步：收集阳光";
			bool undoMove = undoRedo.Undo();
			await WaitFrames(2);
			bool undoMovePassed = undoMove && draft.step[0] == first && inline.CurrentStepIndex == 1;
			bool redoMove = undoRedo.Redo();
			await WaitFrames(2);
			bool redoMovePassed = redoMove && draft.step[0] == second && inline.CurrentStepIndex == 0;
			bool removed = inline.RemoveStep();
			await WaitFrames(2);
			bool removePassed = removed && draft.step.Count == 1;
			bool undoRemove = undoRedo.Undo();
			await WaitFrames(2);
			bool flag3 = undoRemove && draft.step.Count == 2 && draft.step[0] == second;
			bool stepWorkflow = duplicated & movePassed & undoMovePassed & redoMovePassed & removePassed & flag3;
			Require(stepWorkflow, "Step duplicate/move/remove workflow did not preserve selection and Undo state.");
			OptionButton conditionType = inline.FindChild("ConditionType", recursive: true, owned: false) as OptionButton;
			conditionType?.Select(0);
			inline.AddCondition();
			await WaitFrames(2);
			TutorialStepConfig active = draft.step[0];
			TutorialConditionCheckCharaterNum character = ((active.conditionList.Count > 0) ? (active.conditionList[0] as TutorialConditionCheckCharaterNum) : null);
			inline.SetCharacterName(character, "Peashooter");
			inline.SetConditionTarget(character, 3.0);
			conditionType?.Select(1);
			inline.AddCondition();
			await WaitFrames(2);
			TutorialConditionCheckSunCollect sun = ((active.conditionList.Count > 1) ? (active.conditionList[1] as TutorialConditionCheckSunCollect) : null);
			inline.SetConditionTarget(sun, 75.0);
			bool movedCondition = inline.MoveCondition(sun, -1);
			bool duplicatedCondition = inline.DuplicateCondition(character);
			await WaitFrames(2);
			bool conditionCards = (movedCondition & duplicatedCondition) && active.conditionList.Count == 3 && active.conditionList[0] == sun && active.conditionList[2] is TutorialConditionCheckCharaterNum;
			inline.RemoveCondition(active.conditionList[2]);
			await WaitFrames(2);
			Require(conditionCards && active.conditionList.Count == 2, "Built-in condition cards did not add, edit, duplicate, move and remove correctly.");
			TutorialProbeCustomCondition customSource = new TutorialProbeCustomCondition
			{
				ResourceName = "ProbeCustomCondition",
				customValue = 7,
				playerText = "自定义玩家提示",
				accentColor = new Color(0.3f, 0.7f, 0.2f)
			};
			inline.CustomConditionPicker?.SetEditedResource(customSource);
			bool customAdded = inline.AddSelectedCondition();
			await WaitFrames(3);
			TutorialProbeCustomCondition custom = ((active.conditionList.Count > 2) ? (active.conditionList[2] as TutorialProbeCustomCondition) : null);
			XWDirectPropertySurface xWDirectPropertySurface = FindCustomSurface(inline);
			bool customCoverage = customAdded && GodotObject.IsInstanceValid(custom) && custom != customSource && GodotObject.IsInstanceValid(xWDirectPropertySurface) && xWDirectPropertySurface.MissingPropertyCount == 0 && xWDirectPropertySurface.EditablePropertyNames.Contains("customValue") && xWDirectPropertySurface.EditablePropertyNames.Contains("playerText") && xWDirectPropertySurface.EditablePropertyNames.Contains("accentColor") && GodotObject.IsInstanceValid(xWDirectPropertySurface.FindChild("Direct_customValue", recursive: true, owned: false));
			Require(customCoverage, "Custom TutorialConditionConfig exports were not mounted as direct main-surface controls.");
			bool undoCustomAdd = undoRedo.Undo();
			await WaitFrames(2);
			bool customRemovedByUndo = undoCustomAdd && active.conditionList.Count == 2;
			bool redoCustomAdd = undoRedo.Redo();
			await WaitFrames(3);
			custom = ((active.conditionList.Count > 2) ? (active.conditionList[2] as TutorialProbeCustomCondition) : null);
			xWDirectPropertySurface = FindCustomSurface(inline);
			bool customRestoredByRedo = redoCustomAdd && GodotObject.IsInstanceValid(custom) && GodotObject.IsInstanceValid(xWDirectPropertySurface);
			Require(customRemovedByUndo & customRestoredByRedo, "Custom condition collection change did not survive Undo/Redo.");
			SpinBox customValueControl = FindSpinBox(xWDirectPropertySurface?.FindChild("Direct_customValue", recursive: true, owned: false));
			if (GodotObject.IsInstanceValid(customValueControl))
			{
				customValueControl.Value = 42.0;
			}
			await WaitFrames(2);
			int num;
			if (GodotObject.IsInstanceValid(customValueControl))
			{
				TutorialProbeCustomCondition tutorialProbeCustomCondition = custom;
				num = ((tutorialProbeCustomCondition != null && tutorialProbeCustomCondition.customValue == 42) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool customEdited = (byte)num != 0;
			bool undoCustomValue = undoRedo.Undo();
			await WaitFrames(2);
			int num2;
			if (undoCustomValue)
			{
				TutorialProbeCustomCondition tutorialProbeCustomCondition2 = custom;
				num2 = ((tutorialProbeCustomCondition2 != null && tutorialProbeCustomCondition2.customValue == 7) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			bool customValueRestored = (byte)num2 != 0;
			bool redoCustomValue = undoRedo.Redo();
			await WaitFrames(2);
			int num3;
			if (redoCustomValue)
			{
				TutorialProbeCustomCondition tutorialProbeCustomCondition3 = custom;
				num3 = ((tutorialProbeCustomCondition3 != null && tutorialProbeCustomCondition3.customValue == 42) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
			bool customValueRedone = (byte)num3 != 0;
			Require(customEdited & customValueRestored & customValueRedone, "Custom condition direct control did not participate in global Undo/Redo.");
			bool runtimeBoundary = !ModLoader.InferRuntimeEntry("Resources/Tutorials/Conditions/ProbeCondition.tres", out var category, out var key) && !ModLoader.InferRuntimeEntry("Resources/Tutorials/Steps/ProbeStep.tres", out key, out category) && ModLoader.InferRuntimeEntry("Resources/Tutorials/ProbeTutorial.tres", out var category2, out category) && category2 == "Tutorial";
			Require(runtimeBoundary, "Standalone tutorial building blocks still pollute the runtime Tutorial dictionary.");
			Button button = FindButtonByText(_editor, "保存");
			Require(GodotObject.IsInstanceValid(button), "Tutorial resource toolbar save button is unavailable.");
			if (GodotObject.IsInstanceValid(button))
			{
				button.EmitSignal(BaseButton.SignalName.Pressed);
				await WaitFrames(8);
			}
			TutorialConfig tutorialConfig = ResourceLoader.Load<TutorialConfig>("user://mod_editor_tutorial_workbench_probe.tres", "", ResourceLoader.CacheMode.Ignore);
			TutorialProbeCustomCondition tutorialProbeCustomCondition4 = ((GodotObject.IsInstanceValid(tutorialConfig) && tutorialConfig.step.Count == 2 && tutorialConfig.step[0].conditionList.Count == 3) ? (tutorialConfig.step[0].conditionList[2] as TutorialProbeCustomCondition) : null);
			bool saveReload = GodotObject.IsInstanceValid(tutorialConfig) && tutorialConfig.saveKey == "ProbeTutorialEdited" && tutorialConfig.step.Count == 2 && tutorialConfig.step[0].broadCastConfig?.broadCastString == "第二步：收集阳光" && tutorialConfig.step[0].conditionList[0] is TutorialConditionCheckSunCollect { num: 75 } && GodotObject.IsInstanceValid(tutorialProbeCustomCondition4) && tutorialProbeCustomCondition4.customValue == 42 && tutorialProbeCustomCondition4.playerText == "自定义玩家提示";
			Require(saveReload, "Tutorial edits did not survive toolbar save and uncached reload.");
			TutorialStepConfig resource = new TutorialStepConfig
			{
				ResourceName = "StandaloneTutorialStepProbe",
				broadCastUse = true,
				broadCastConfig = new BroadCastConfig
				{
					ResourceLocalToScene = true,
					broadCastString = "独立步骤原文",
					broadCastTime = -1.0
				}
			};
			Error standaloneStepSave = ResourceSaver.Save(resource, "user://mod_editor_tutorial_step_probe.tres", ResourceSaver.SaverFlags.None);
			resource = ResourceLoader.Load<TutorialStepConfig>("user://mod_editor_tutorial_step_probe.tres", "", ResourceLoader.CacheMode.Replace);
			XWEditorInterface.Instance.EditResource(resource, XWResourceEditContext.ForRoot(resource, "user://mod_editor_tutorial_step_probe.tres", "tutorial_editor"));
			XWEditorInterface.Instance.FocusPanel("tutorial_editor");
			XWTutorialInlineEditor xWTutorialInlineEditor = await WaitForInlineEditor(900, (XWTutorialInlineEditor candidate) => !candidate.CanManageStepCollection && !candidate.IsStandaloneCondition);
			bool standaloneStepMounted = standaloneStepSave == Error.Ok && GodotObject.IsInstanceValid(xWTutorialInlineEditor);
			xWTutorialInlineEditor?.SetBroadcastText("独立步骤已保存");
			FindButtonByText(_editor, "保存")?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(8);
			TutorialStepConfig tutorialStepConfig = ResourceLoader.Load<TutorialStepConfig>("user://mod_editor_tutorial_step_probe.tres", "", ResourceLoader.CacheMode.Ignore);
			bool standaloneStepReloaded = GodotObject.IsInstanceValid(tutorialStepConfig) && tutorialStepConfig.broadCastConfig?.broadCastString == "独立步骤已保存";
			TutorialProbeCustomCondition resource2 = new TutorialProbeCustomCondition
			{
				ResourceName = "StandaloneTutorialConditionProbe",
				customValue = 5,
				playerText = "独立条件原文"
			};
			Error standaloneConditionSave = ResourceSaver.Save(resource2, "user://mod_editor_tutorial_condition_probe.tres", ResourceSaver.SaverFlags.None);
			resource2 = ResourceLoader.Load<TutorialProbeCustomCondition>("user://mod_editor_tutorial_condition_probe.tres", "", ResourceLoader.CacheMode.Replace);
			XWEditorInterface.Instance.EditResource(resource2, XWResourceEditContext.ForRoot(resource2, "user://mod_editor_tutorial_condition_probe.tres", "tutorial_editor"));
			XWEditorInterface.Instance.FocusPanel("tutorial_editor");
			XWTutorialInlineEditor xWTutorialInlineEditor2 = await WaitForInlineEditor(900, (XWTutorialInlineEditor candidate) => candidate.IsStandaloneCondition);
			XWDirectPropertySurface xWDirectPropertySurface2 = FindCustomSurface(xWTutorialInlineEditor2);
			SpinBox spinBox = FindSpinBox(xWDirectPropertySurface2?.FindChild("Direct_customValue", recursive: true, owned: false));
			bool standaloneConditionMounted = GodotObject.IsInstanceValid(xWTutorialInlineEditor2) && GodotObject.IsInstanceValid(xWDirectPropertySurface2) && GodotObject.IsInstanceValid(spinBox);
			if (GodotObject.IsInstanceValid(spinBox))
			{
				spinBox.Value = 19.0;
			}
			await WaitFrames(2);
			FindButtonByText(_editor, "保存")?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(8);
			TutorialProbeCustomCondition tutorialProbeCustomCondition5 = ResourceLoader.Load<TutorialProbeCustomCondition>("user://mod_editor_tutorial_condition_probe.tres", "", ResourceLoader.CacheMode.Ignore);
			TutorialConfig tutorialConfig2 = ResourceLoader.Load<TutorialConfig>("user://mod_editor_tutorial_workbench_probe.tres", "", ResourceLoader.CacheMode.Ignore);
			bool flag4 = (((standaloneStepMounted & standaloneStepReloaded) && standaloneConditionSave == Error.Ok) & standaloneConditionMounted) && GodotObject.IsInstanceValid(tutorialProbeCustomCondition5) && tutorialProbeCustomCondition5.customValue == 19 && GodotObject.IsInstanceValid(tutorialConfig2) && tutorialConfig2.step.Count == 2 && tutorialConfig2.step[0].broadCastConfig?.broadCastString == "第二步：收集阳光";
			Require(flag4, "Standalone step/condition adapters did not save their real resource targets safely.");
			bool value = FindAncestorWindow(_editor) != null;
			GD.Print($"[MOD_EDITOR_TUTORIAL_WORKBENCH_PROBE] window={value} jsonSource={jsonSourceWorkflow} emptyAdd={addStep & undoAddPassed & redoAddPassed} rootText={inlineRootText} broadcastUndo={broadcastEdited & broadcastRestored & broadcastRedone} steps={stepWorkflow} conditions={conditionCards} customCoverage={customCoverage} customUndo={customRemovedByUndo & customRestoredByRedo & customValueRestored & customValueRedone} runtimeBoundary={runtimeBoundary} saveReload={saveReload} standalone={flag4} inspectorHidden={inspectorHidden} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			if (XWEditorInterface.Instance?.GetResourceEditor("tutorial_editor") is XWTextFlowVisualResourceEditor xWTextFlowVisualResourceEditor && GodotObject.IsInstanceValid(xWTextFlowVisualResourceEditor))
			{
				_editor = xWTextFlowVisualResourceEditor;
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> EnterEditorSurface(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			Control control = XWEditorInterface.Instance?.GetEditorPanel();
			Node instance = control?.FindChild("LoadingOverlay", recursive: true, owned: false);
			Control control2 = control?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control;
			if (GodotObject.IsInstanceValid(control) && !GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(control2))
			{
				control2.Hide();
				XWEditorInterface.Instance.FocusPanel("tutorial_editor");
				await WaitFrames(3);
				return _editor.IsVisibleInTree();
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<XWTutorialInlineEditor> WaitForInlineEditor(int maxFrames, Func<XWTutorialInlineEditor, bool> predicate = null)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			XWTutorialInlineEditor xWTutorialInlineEditor = _editor?.FindChild("TutorialInlineEditor", recursive: true, owned: false) as XWTutorialInlineEditor;
			if (GodotObject.IsInstanceValid(xWTutorialInlineEditor) && xWTutorialInlineEditor.IsVisibleInTree() && GodotObject.IsInstanceValid(xWTutorialInlineEditor.FindChild("AddStepButton", recursive: true, owned: false)) && (predicate == null || predicate(xWTutorialInlineEditor)))
			{
				return xWTutorialInlineEditor;
			}
			await WaitFrames(1);
		}
		return null;
	}

	private static XWDirectPropertySurface FindCustomSurface(Node root)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		if (root is XWDirectPropertySurface result && root.Name.ToString().StartsWith("CustomConditionProperties_"))
		{
			return result;
		}
		foreach (Node child in root.GetChildren())
		{
			XWDirectPropertySurface xWDirectPropertySurface = FindCustomSurface(child);
			if (GodotObject.IsInstanceValid(xWDirectPropertySurface))
			{
				return xWDirectPropertySurface;
			}
		}
		return null;
	}

	private static SpinBox FindSpinBox(Node root)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		if (root is SpinBox result)
		{
			return result;
		}
		foreach (Node child in root.GetChildren())
		{
			SpinBox spinBox = FindSpinBox(child);
			if (GodotObject.IsInstanceValid(spinBox))
			{
				return spinBox;
			}
		}
		return null;
	}

	private static Button FindButtonByText(Node root, string text)
	{
		foreach (Node item in root.FindChildren("*", "Button", recursive: true, owned: false))
		{
			if (item is Button button && button.Text == text)
			{
				return button;
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
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
			GD.PrintErr("[MOD_EDITOR_TUTORIAL_WORKBENCH_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_TUTORIAL_WORKBENCH_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindCustomSurface, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindSpinBox, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindButtonByText, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.FindCustomSurface && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWDirectPropertySurface>(FindCustomSurface(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindSpinBox && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(FindSpinBox(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindButtonByText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(FindButtonByText(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.FindCustomSurface && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWDirectPropertySurface>(FindCustomSurface(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindSpinBox && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(FindSpinBox(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindButtonByText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(FindButtonByText(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.FindCustomSurface)
		{
			return true;
		}
		if (method == MethodName.FindSpinBox)
		{
			return true;
		}
		if (method == MethodName.FindButtonByText)
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
			_editor = VariantUtils.ConvertTo<XWTextFlowVisualResourceEditor>(in value);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XWTextFlowVisualResourceEditor>();
		}
	}
}
