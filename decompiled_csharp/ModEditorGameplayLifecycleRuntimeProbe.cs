using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ResourceEditors;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

[ScriptPath("res://Tests/ModEditorGameplayLifecycleRuntimeProbe.cs")]
public class ModEditorGameplayLifecycleRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateFixture = "CreateFixture";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _editor = "_editor";

		public static readonly StringName _workbench = "_workbench";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string DraftPath = "user://mod_editor_gameplay_lifecycle_probe.tres";

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWGameplayLogicVisualResourceEditor _editor;

	private XWGameplayLifecycleWorkbench _workbench;

	private XWVisualEditorDescriptor _descriptor;

	public override async void _Ready()
	{
		bool stagePriority = false;
		try
		{
			TowerDefenseLevelWaveConfig wave = CreateFixture();
			Require(ResourceSaver.Save(wave, "user://mod_editor_gameplay_lifecycle_probe.tres", ResourceSaver.SaverFlags.None) == Error.Ok, "Could not save the isolated Wave fixture.");
			wave = ResourceLoader.Load<TowerDefenseLevelWaveConfig>("user://mod_editor_gameplay_lifecycle_probe.tres", "", ResourceLoader.CacheMode.Ignore);
			Require(GodotObject.IsInstanceValid(wave), "Isolated Wave fixture did not reload.");
			if (!GodotObject.IsInstanceValid(wave))
			{
				Finish();
				return;
			}
			TowerDefenseManager managerBefore = TowerDefenseManager.Instance;
			TowerDefenseControlNew controlBefore = TowerDefenseManager.CurrentControl;
			double runTimeBefore = (GodotObject.IsInstanceValid(managerBefore) ? managerBefore.runGameTime : 0.0);
			int managerNodesBefore = CountNodes<TowerDefenseManager>(GetTree().Root);
			int deathRecordsBefore = TowerDefenseManager.DeathRecordCount;
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
			bool f3 = await WaitForEditor(900);
			Require(f3, "F3 did not initialize the GameplayLogic editor within 900 frames.");
			if (!f3)
			{
				Finish();
				return;
			}
			bool flag = await EnterEditorSurface(900);
			Require(flag, "F3 editor did not finish loading its main editing surface.");
			if (!flag)
			{
				Finish();
				return;
			}
			Node inspectorSentinel = new Node
			{
				Name = "GameplayLifecycleInspectorSentinel"
			};
			AddChild(inspectorSentinel, forceReadableName: false, InternalMode.Disabled);
			XWEditorInterface.Instance.InspectObject(inspectorSentinel);
			bool registry = XWResourceEditorRegistry.TryGetEditor(wave, "user://mod_editor_gameplay_lifecycle_probe.tres", out _descriptor) && _descriptor.Category == "GameplayLogic" && XWResourceEditorRegistry.TryOpen(wave, "user://mod_editor_gameplay_lifecycle_probe.tres", XWResourceEditContext.ForRoot(wave, "user://mod_editor_gameplay_lifecycle_probe.tres", _descriptor.DockKey));
			Require(registry, "Wave fixture did not enter GameplayLogic through XWResourceEditorRegistry.");
			if (!registry)
			{
				Finish();
				return;
			}
			XWEditorInterface.Instance.FocusPanel(_descriptor.DockKey);
			await WaitFrames(8);
			bool mounted = await WaitForWorkbench(900);
			Require(mounted, "Scene-authored lifecycle workbench did not mount through the registry route.");
			if (!mounted)
			{
				Finish();
				return;
			}
			TabContainer gameplayPages = Find<TabContainer>(_editor, "GameplayPages");
			Control gameplaySurface = Find<Control>(_editor, "GameplayWorkbench");
			Control resourceShelf = Find<Control>(_editor, "ResourceShelfHost");
			Control stageHost = Find<Control>(_editor, "StageHost");
			Control timelineHost = Find<Control>(_editor, "TimelineHost");
			Control directProperties = Find<Control>(_editor, "AdvancedInspectorHost");
			SubViewport stageViewport = Find<SubViewport>(_editor, "StageViewport");
			Window editorWindow = FindAncestorWindow(_editor);
			if (GodotObject.IsInstanceValid(editorWindow))
			{
				editorWindow.MinSize = Vector2I.Zero;
				editorWindow.Size = new Vector2I(820, 720);
				await WaitFrames(12);
			}
			bool responsive = GodotObject.IsInstanceValid(editorWindow) && editorWindow.Size.X <= 840 && GodotObject.IsInstanceValid(gameplaySurface) && gameplaySurface.GetCombinedMinimumSize().X <= 380f && gameplaySurface.Size.X < 650f && GodotObject.IsInstanceValid(gameplayPages) && gameplayPages.GetTabCount() == 3;
			Require(responsive, $"Gameplay workbench did not contract to the narrow editor: window={editorWindow?.Size.X ?? (-1)}; surface={gameplaySurface?.Size.X ?? (-1f)}; minimum={gameplaySurface?.GetCombinedMinimumSize().X ?? (-1f)}.");
			bool shelfPage = false;
			bool battlePage = false;
			bool propertyPage = false;
			bool shelfStageGated = false;
			bool propertyStageGated = false;
			if (GodotObject.IsInstanceValid(gameplayPages))
			{
				gameplayPages.CurrentTab = 0;
				await WaitFrames(3);
				shelfPage = GodotObject.IsInstanceValid(resourceShelf) && resourceShelf.IsVisibleInTree() && !stageHost.IsVisibleInTree();
				shelfStageGated = GodotObject.IsInstanceValid(stageViewport) && stageViewport.RenderTargetUpdateMode == SubViewport.UpdateMode.Disabled;
				gameplayPages.CurrentTab = 1;
				await WaitFrames(3);
				battlePage = stageHost.IsVisibleInTree() && timelineHost.IsVisibleInTree();
				stagePriority = battlePage && stageHost.Size.X >= gameplayPages.Size.X - 24f && timelineHost.Size.X >= gameplayPages.Size.X - 24f && GodotObject.IsInstanceValid(stageViewport) && stageViewport.RenderTargetUpdateMode == SubViewport.UpdateMode.Always;
				gameplayPages.CurrentTab = 2;
				await WaitFrames(3);
				propertyPage = GodotObject.IsInstanceValid(directProperties) && directProperties.IsVisibleInTree() && !stageHost.IsVisibleInTree();
				propertyStageGated = GodotObject.IsInstanceValid(stageViewport) && stageViewport.RenderTargetUpdateMode == SubViewport.UpdateMode.Disabled;
				gameplayPages.CurrentTab = 1;
				await WaitFrames(3);
			}
			bool pagesReachable = shelfPage & battlePage & propertyPage;
			bool stageGated = shelfStageGated & propertyStageGated;
			Require(pagesReachable, $"Responsive Gameplay pages are not all reachable: shelf={shelfPage}; battle={battlePage}; properties={propertyPage}.");
			Require(stagePriority, $"Battle page does not give Stage/Timeline full width or safe viewport gating: pages={gameplayPages?.Size.X ?? (-1f)}; stage={stageHost?.Size.X ?? (-1f)}; timeline={timelineHost?.Size.X ?? (-1f)}; updateMode={stageViewport?.RenderTargetUpdateMode}.");
			Require(stageGated, $"Stage SubViewport kept rendering behind a hidden page: shelfGated={shelfStageGated}; propertiesGated={propertyStageGated}.");
			Button button = Find<Button>(_workbench, "LifecycleReadyButton");
			Button battleButton = Find<Button>(_workbench, "LifecycleBattleButton");
			Button settleButton = Find<Button>(_workbench, "LifecycleSettleButton");
			Button playPauseButton = Find<Button>(_workbench, "PlayPauseButton");
			Button stepButton = Find<Button>(_workbench, "StepButton");
			Button resetButton = Find<Button>(_workbench, "ResetButton");
			HSlider scrubber = Find<HSlider>(_workbench, "LifecycleScrubber");
			Control control = Find<Control>(_workbench, "LifecycleTimelineCanvas");
			VBoxContainer historyRoot = Find<VBoxContainer>(_workbench, "LifecycleHistoryRoot");
			bool condition = GodotObject.IsInstanceValid(button) && GodotObject.IsInstanceValid(battleButton) && GodotObject.IsInstanceValid(settleButton) && GodotObject.IsInstanceValid(playPauseButton) && GodotObject.IsInstanceValid(stepButton) && GodotObject.IsInstanceValid(resetButton) && GodotObject.IsInstanceValid(scrubber) && GodotObject.IsInstanceValid(control) && control.IsVisibleInTree() && GodotObject.IsInstanceValid(historyRoot);
			Require(condition, "Lifecycle scene is missing one or more fixed visual controls.");
			button?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			bool ready = _workbench.PhaseName == "Ready" && Math.Abs(_workbench.PreviewTime) < 0.001;
			battleButton?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			bool battle = _workbench.PhaseName == "Battle" && _workbench.PreviewTime > 0.0;
			settleButton?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			bool flag2 = _workbench.PhaseName == "Settle" && _workbench.PreviewTime < scrubber.MaxValue;
			bool phases = ready & battle & flag2;
			Require(phases, $"Lifecycle phase buttons failed: ready={ready}; battle={battle}; settle={flag2}.");
			resetButton?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			bool reset = _workbench.PhaseName == "Ready" && Math.Abs(_workbench.PreviewTime) < 0.001;
			Require(reset, "Reset did not return the sandbox to Ready at 00:00.");
			stepButton?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			bool step = _workbench.PreviewTime > 0.0 && !_workbench.IsPlaying;
			Require(step, "Single-step did not jump to the next configuration event.");
			double previewTime = _workbench.PreviewTime;
			playPauseButton?.EmitSignal(BaseButton.SignalName.Pressed);
			bool started = _workbench.IsPlaying;
			bool advanced = await WaitForTimeAdvance(previewTime, 600);
			playPauseButton?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			bool playPause = (started & advanced) && !_workbench.IsPlaying;
			Require(playPause, $"Play/Pause did not advance and stop cleanly: started={started}; advanced={advanced}.");
			double scrubTarget = (scrubber.Value = Math.Min(scrubber.MaxValue - 0.25, Math.Max(0.25, scrubber.MaxValue * 0.55)));
			await WaitFrames(3);
			bool flag3 = Math.Abs(_workbench.PreviewTime - scrubTarget) <= scrubber.Step * 0.51 + 0.001;
			if (flag3)
			{
				string phaseName = _workbench.PhaseName;
				bool flag4 = ((phaseName == "Battle" || phaseName == "Settle") ? true : false);
				flag3 = flag4;
			}
			bool scrub = flag3;
			Require(scrub, $"Timeline scrubber did not seek the lifecycle projection: target={scrubTarget:0.00}; actual={_workbench.PreviewTime:0.00}.");
			bool history = historyRoot.GetChildCount() >= 3;
			Require(history, $"Lifecycle event history is empty or incomplete: rows={historyRoot.GetChildCount()}.");
			playPauseButton?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(1);
			double beforeHide = _workbench.PreviewTime;
			gameplayPages.CurrentTab = 0;
			await WaitFrames(12);
			bool hiddenStopped = !_workbench.IsPlaying && Math.Abs(_workbench.PreviewTime - beforeHide) < 0.001 && stageViewport.RenderTargetUpdateMode == SubViewport.UpdateMode.Disabled;
			gameplayPages.CurrentTab = 1;
			await WaitFrames(2);
			Require(hiddenStopped, "Lifecycle timer kept running after the Battle tab became hidden.");
			PanelContainer panelContainer = _editor.FindChild("InspectorPanel", recursive: true, owned: false) as PanelContainer;
			XWInspector xWInspector = XWEditorInterface.Instance.GetInspector() as XWInspector;
			bool flag5 = GodotObject.IsInstanceValid(panelContainer) && !panelContainer.Visible && _editor.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null && (xWInspector == null || xWInspector.CurrentObject == inspectorSentinel);
			Require(flag5, "Gameplay lifecycle workbench exposed or touched the raw Inspector.");
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			double num2 = (GodotObject.IsInstanceValid(instance) ? instance.runGameTime : 0.0);
			bool flag6 = managerBefore == instance && controlBefore == currentControl && managerNodesBefore == CountNodes<TowerDefenseManager>(GetTree().Root) && deathRecordsBefore == TowerDefenseManager.DeathRecordCount && Math.Abs(num2 - runTimeBefore) < 0.0001 && wave.spawn.Count == 2 && wave.spawn[0].num == 2 && wave.spawn[1].line == 4;
			Require(flag6, "Lifecycle sandbox created or mutated real battle state/configuration.");
			GD.Print($"[MOD_EDITOR_GAMEPLAY_LIFECYCLE_PROBE] f3={f3} registry={registry} mounted={mounted} responsive={responsive} pagesReachable={pagesReachable} stagePriority={stagePriority} stageGated={stageGated} phases={phases} playPause={playPause} step={step} reset={reset} scrub={scrub} hiddenStopped={hiddenStopped} history={history} inspectorHidden={flag5} sideEffectFree={flag6} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private static TowerDefenseLevelWaveConfig CreateFixture()
	{
		return new TowerDefenseLevelWaveConfig
		{
			ResourceName = "Gameplay Lifecycle Probe",
			spawn = 
			{
				new TowerDefenseLevelSpawnConfig
				{
					zombie = "",
					line = 1,
					num = 2
				},
				new TowerDefenseLevelSpawnConfig
				{
					zombie = "",
					line = 4,
					num = 3
				}
			},
			dynamicPlantfood = { 2, 5 }
		};
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (XWResourceEditorRegistry.TryGetEditor(new TowerDefenseLevelWaveConfig(), "user://mod_editor_gameplay_lifecycle_probe.tres", out var descriptor) && XWEditorInterface.Instance?.GetResourceEditor(descriptor.DockKey) is XWGameplayLogicVisualResourceEditor xWGameplayLogicVisualResourceEditor && GodotObject.IsInstanceValid(xWGameplayLogicVisualResourceEditor))
			{
				_editor = xWGameplayLogicVisualResourceEditor;
				_descriptor = descriptor;
				(XWEditorInterface.Instance.GetEditorPanel()?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control)?.Hide();
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> WaitForWorkbench(int maxFrames)
	{
		int visibleFrames = 0;
		for (int frame = 0; frame < maxFrames; frame++)
		{
			_workbench = _editor?.FindChild("TimelineHost", recursive: true, owned: false) as XWGameplayLifecycleWorkbench;
			if (GodotObject.IsInstanceValid(_workbench) && _workbench.IsVisibleInTree() && _workbench.FindChild("LifecycleTimelineCanvas", recursive: true, owned: false) != null)
			{
				int num = visibleFrames + 1;
				visibleFrames = num;
				if (num >= 8)
				{
					return true;
				}
			}
			else
			{
				visibleFrames = 0;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> EnterEditorSurface(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			Control control = XWEditorInterface.Instance?.GetEditorPanel();
			Node instance = control?.FindChild("LoadingOverlay", recursive: true, owned: false);
			Control control2 = control?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control;
			if (GodotObject.IsInstanceValid(control) && !GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(control2))
			{
				control2.Hide();
				await WaitFrames(3);
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> WaitForTimeAdvance(double start, int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (!GodotObject.IsInstanceValid(_workbench) || !_workbench.IsPlaying)
			{
				return false;
			}
			if (_workbench.PreviewTime > start + 0.001)
			{
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private static int CountNodes<T>(Node root) where T : Node
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return 0;
		}
		int num = ((root is T) ? 1 : 0);
		foreach (Node child in root.GetChildren())
		{
			num += CountNodes<T>(child);
		}
		return num;
	}

	private static T Find<T>(Node root, string name) where T : Node
	{
		return root?.FindChild(name, recursive: true, owned: false) as T;
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
			GD.PrintErr("[MOD_EDITOR_GAMEPLAY_LIFECYCLE_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_GAMEPLAY_LIFECYCLE_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateFixture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
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
		if (method == MethodName.CreateFixture && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelWaveConfig>(CreateFixture());
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
		if (method == MethodName.CreateFixture && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelWaveConfig>(CreateFixture());
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
		if (method == MethodName.CreateFixture)
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
			_editor = VariantUtils.ConvertTo<XWGameplayLogicVisualResourceEditor>(in value);
			return true;
		}
		if (name == PropertyName._workbench)
		{
			_workbench = VariantUtils.ConvertTo<XWGameplayLifecycleWorkbench>(in value);
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
		if (name == PropertyName._workbench)
		{
			value = VariantUtils.CreateFrom(in _workbench);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._workbench, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._workbench, Variant.From(in _workbench));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XWGameplayLogicVisualResourceEditor>();
		}
		if (info.TryGetProperty(PropertyName._workbench, out var value2))
		{
			_workbench = value2.As<XWGameplayLifecycleWorkbench>();
		}
	}
}
