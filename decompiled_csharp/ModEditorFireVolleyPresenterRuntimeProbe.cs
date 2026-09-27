using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ResourceEditors;
using PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://Tests/ModEditorFireVolleyPresenterRuntimeProbe.cs")]
public class ModEditorFireVolleyPresenterRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ProbeRuntimeSemantics = "ProbeRuntimeSemantics";

		public static readonly StringName HasGenericPropertyControlOutsidePresenter = "HasGenericPropertyControlOutsidePresenter";

		public static readonly StringName CountGenericSpecializedControls = "CountGenericSpecializedControls";

		public static readonly StringName IsDescendantOf = "IsDescendantOf";

		public static readonly StringName HasAll = "HasAll";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _history = "_history";

		public static readonly StringName _editor = "_editor";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResourcePath = "user://ModEditorFireVolleyPresenterProbe/FireVolleyPresenter.tres";

	private readonly List<string> _failures = new List<string>();

	private XWUndoRedoManager _history;

	private XWCharacterComponentVisualResourceEditor _editor;

	public override async void _Ready()
	{
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
			Require(GodotObject.IsInstanceValid(modEditorManager), "无法创建 ModEditorManager。");
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
			bool f3 = await WaitForInterface(900);
			Require(f3, "F3 未初始化真实 Mod 编辑器界面。");
			if (!f3)
			{
				Finish();
				return;
			}
			string path = ProjectSettings.GlobalizePath("user://ModEditorFireVolleyPresenterProbe");
			if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
			}
			Directory.CreateDirectory(path);
			FireComponentFireProjectileConfig trajectory = new FireComponentFireProjectileConfig
			{
				checkProjectileId = 0,
				firePosId = 0,
				speed = 300f,
				dir = -8f,
				offsetLine = 1,
				fireNumSkip = -1
			};
			FireComponentDefinition definition = new FireComponentDefinition
			{
				ComponentTypeId = "FireComponent",
				DefinitionId = "probe.fire.volley",
				InstanceId = "probe.fire.volley.instance",
				fireEventName = "fire",
				fireIntervalBase = 1.5f,
				fireInterval = 1.4f,
				fireIntervalOffset = 0.1f,
				fireNum = 3,
				fireNumAtOnce = false,
				checkUse = true,
				checkAllLine = false,
				checkHeight = true,
				canTargetGargantuar = false,
				checkGravestone = true
			};
			definition.firePosMarkerPaths.Add(new NodePath("Muzzle"));
			definition.fireProjectileList.Add(trajectory);
			Require(ResourceSaver.Save(definition, "user://ModEditorFireVolleyPresenterProbe/FireVolleyPresenter.tres", ResourceSaver.SaverFlags.None) == Error.Ok, "Fire 探针资源保存失败。");
			definition = ResourceLoader.Load<FireComponentDefinition>("user://ModEditorFireVolleyPresenterProbe/FireVolleyPresenter.tres", "", ResourceLoader.CacheMode.Ignore);
			Require(GodotObject.IsInstanceValid(definition), "Fire 探针资源无法忽略缓存重载。");
			Node inspectorSentinel = new Node
			{
				Name = "FireInspectorSentinel"
			};
			AddChild(inspectorSentinel, forceReadableName: false, InternalMode.Disabled);
			XWInspector inspector = XWEditorInterface.Instance.GetInspector() as XWInspector;
			inspector?.EditObject(inspectorSentinel);
			await WaitFrames(2);
			bool route = XWResourceEditorRegistry.TryGetEditor(definition, "user://ModEditorFireVolleyPresenterProbe/FireVolleyPresenter.tres", out var descriptor) && descriptor.Category == "CharacterComponent" && descriptor.DockKey == "character_component_editor";
			Require(route, "Fire 定义未路由到角色组件编辑器。");
			XWEditorInterface.Instance.EditResource(definition, XWResourceEditContext.ForRoot(definition, "user://ModEditorFireVolleyPresenterProbe/FireVolleyPresenter.tres", "character_component_editor"));
			XWEditorInterface.Instance.FocusPanel("character_component_editor");
			bool flag = await WaitForComponentEditor(definition, 900);
			Require(flag, "角色组件编辑器未挂载 FireVolleyPresenter。");
			if (!flag)
			{
				Finish();
				return;
			}
			_editor.SetWorkbenchPage(0);
			await WaitFrames(3);
			XWFireVolleyPresenter presenter = _editor.FireVolleyPresenter;
			PanelContainer root = Find<PanelContainer>(_editor, "FireVolleyPresenter");
			Node2D node2D = Find<Node2D>(_editor, "PreviewWorld");
			Node2D node2D2 = Find<Node2D>(_editor, "FireVolleyPreviewOverlay");
			bool gamePosition = GodotObject.IsInstanceValid(presenter?.Root) && root == presenter.Root && GodotObject.IsInstanceValid(node2D) && node2D2?.GetParent() == node2D && IsDescendantOf(root, Find<Node>(_editor, "ComponentPropertyHost")) && _editor.IsPreviewViewportRendering && presenter.OverlayVisible && presenter.OverlayInputActive;
			Require(gamePosition, "Fire 游戏位置面板或 PreviewWorld overlay 未正确挂载。");
			Node2D node2D3 = new Node2D
			{
				Name = "FireMarkerDropOwner",
				ProcessMode = ProcessModeEnum.Disabled
			};
			Marker2D node = new Marker2D
			{
				Name = "Muzzle",
				Position = new Vector2(0f, -60f)
			};
			Marker2D muzzleAlt = new Marker2D
			{
				Name = "MuzzleAlt",
				Position = new Vector2(96f, -60f)
			};
			node2D3.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			node2D3.AddChild(muzzleAlt, forceReadableName: false, InternalMode.Disabled);
			node2D.AddChild(node2D3, forceReadableName: false, InternalMode.Disabled);
			presenter.BindPreviewCharacter(node2D3);
			await WaitFrames(2);
			_history.ClearHistory();
			bool markerDropped = presenter.DropMarkerHandleAt(0, presenter.Overlay.ToLocal(muzzleAlt.GlobalPosition));
			await WaitFrames(2);
			bool markerApplied = markerDropped && definition.firePosMarkerPaths[0].ToString() == "MuzzleAlt" && _history.HasUndo();
			bool markerUndone = _history.Undo();
			await WaitFrames(2);
			markerUndone &= definition.firePosMarkerPaths[0].ToString() == "Muzzle" && presenter.MarkerHandleCount == 1;
			bool markerRedone = _history.Redo();
			await WaitFrames(2);
			markerRedone &= definition.firePosMarkerPaths[0].ToString() == "MuzzleAlt";
			bool markerDragContract = ((presenter.MarkerHandleCount == 1) & markerApplied & markerUndone & markerRedone) && HasAll(_editor, "FireMarkerPanel", "FireMarkerRow_0", "FireMarkerPath_0", "FireAddMarkerButton");
			bool targetCards = HasAll(_editor, "FireChoice_same-line", "FireChoice_all-line", "FireChoice_nearest", "FireChoice_farthest", "FireChoice_random", "FireCheckHeightCard", "FireGargantuarCard", "FireGravestoneCard");
			bool timeline = HasAll(_editor, "FireVolleyTimeline", "FireChoice_repeated", "FireChoice_at-once", "FireVolleyCount", "FireVolleyInterval", "FireVolleyIntervalOffset", "FireEventNameEdit");
			bool trajectoryCards = presenter.TrajectoryCount == 1 && HasAll(_editor, "FireProjectileCard_0", "FireProjectileMuzzle_0", "FireProjectileSpeed_0", "FireProjectileDirection_0", "FireProjectileLaneOffset_0", "FireProjectileEvent_0");
			Require(markerDragContract & targetCards & timeline & trajectoryCards, "Fire 枪口、目标卡、时间轴或弹道卡不完整。");
			bool remainingDynamic = HasGenericPropertyControlOutsidePresenter(_editor, root, "checkRayResources") && HasGenericPropertyControlOutsidePresenter(_editor, root, "fireAudioName") && HasGenericPropertyControlOutsidePresenter(_editor, root, "offsetLineTweenDuration");
			bool noDuplicates = XWFireVolleyPresenter.SpecializedProperties.Count == 19 && CountGenericSpecializedControls(_editor, root) == 0;
			Require(remainingDynamic & noDuplicates, "Fire 剩余 metadata 字段缺失或专用字段被通用表单重复渲染。");
			SpinBox speed = Find<SpinBox>(_editor, "FireProjectileSpeed_0");
			ulong rootId = presenter.Root.GetInstanceId();
			ulong speedId = speed?.GetInstanceId() ?? 0;
			int fullBefore = _editor.FullSurfaceRefreshCount;
			int runtimeBuildBefore = _editor.RuntimePreviewBuildCount;
			int previewBefore = presenter.PreviewRevision;
			Stopwatch refreshWatch = Stopwatch.StartNew();
			if (GodotObject.IsInstanceValid(speed))
			{
				speed.EmitSignal(Control.SignalName.FocusEntered);
				for (int i = 310; i <= 390; i += 10)
				{
					speed.Value = i;
					speed.EmitSignal(Godot.Range.SignalName.ValueChanged, speed.Value);
				}
			}
			refreshWatch.Stop();
			await WaitFrames(2);
			int num;
			if (presenter.Root.GetInstanceId() == rootId)
			{
				SpinBox spinBox = Find<SpinBox>(_editor, "FireProjectileSpeed_0");
				if (spinBox != null && spinBox.GetInstanceId() == speedId && _editor.FullSurfaceRefreshCount == fullBefore)
				{
					num = ((presenter.PreviewRevision > previewBefore) ? 1 : 0);
					goto IL_0e1b;
				}
			}
			num = 0;
			goto IL_0e1b;
			IL_0e1b:
			bool inPlaceRefresh = (byte)num != 0;
			bool previewNonBlocking = inPlaceRefresh && _editor.RuntimePreviewBuildCount == runtimeBuildBefore && refreshWatch.ElapsedMilliseconds < 1000;
			speed?.EmitSignal(Control.SignalName.FocusExited);
			await WaitFrames(3);
			Require(inPlaceRefresh & previewNonBlocking, "连续弹道编辑重建了控件/整面板/运行时，或同步刷新发生阻塞。");
			_history.ClearHistory();
			speed = Find<SpinBox>(_editor, "FireProjectileSpeed_0");
			speedId = speed?.GetInstanceId() ?? 0;
			if (GodotObject.IsInstanceValid(speed))
			{
				speed.EmitSignal(Control.SignalName.FocusEntered);
				speed.Value = 455.0;
				speed.EmitSignal(Godot.Range.SignalName.ValueChanged, speed.Value);
				speed.EmitSignal(Control.SignalName.FocusExited);
			}
			await WaitFrames(3);
			trajectory = definition.fireProjectileList[0];
			bool applied = Mathf.IsEqualApprox(trajectory.speed, 455f) && _history.HasUndo();
			bool undone = _history.Undo();
			await WaitFrames(3);
			bool flag2 = undone;
			int num2;
			if (Mathf.IsEqualApprox(trajectory.speed, 390f))
			{
				SpinBox spinBox2 = Find<SpinBox>(_editor, "FireProjectileSpeed_0");
				num2 = ((spinBox2 != null && spinBox2.GetInstanceId() == speedId) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			undone = (byte)((flag2 ? 1u : 0u) & (uint)num2) != 0;
			bool redone = _history.Redo();
			await WaitFrames(3);
			bool flag3 = redone;
			int num3;
			if (Mathf.IsEqualApprox(trajectory.speed, 455f))
			{
				SpinBox spinBox3 = Find<SpinBox>(_editor, "FireProjectileSpeed_0");
				num3 = ((spinBox3 != null && spinBox3.GetInstanceId() == speedId) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
			redone = (byte)((flag3 ? 1u : 0u) & (uint)num3) != 0;
			bool undoRedo = applied & undone & redone;
			Require(undoRedo, "弹道速度未通过原位 Undo/Redo 往返。");
			_history.ClearHistory();
			Find<Button>(_editor, "FireChoice_random")?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(3);
			bool priorityApplied = definition.randomChoose && !definition.catapultFirstFar && _history.HasUndo();
			bool priorityUndone = _history.Undo();
			await WaitFrames(3);
			priorityUndone &= !definition.randomChoose && !definition.catapultFirstFar && !_history.HasUndo();
			bool priorityRedone = _history.Redo();
			await WaitFrames(3);
			priorityRedone &= definition.randomChoose && !definition.catapultFirstFar;
			bool atomicPriority = priorityApplied & priorityUndone & priorityRedone;
			Require(atomicPriority, "目标优先卡没有形成单一原子 Undo。");
			bool runtimeSemantics = ProbeRuntimeSemantics();
			Require(runtimeSemantics, "airFirst、跨行配置或 HUGE 巨人阈值运行时契约失败。");
			bool saveReload = _editor.SaveActiveResource();
			await WaitFrames(6);
			bool oldPresenterReleased = presenter.Root == null;
			FireComponentDefinition fireComponentDefinition = ResourceLoader.Load<FireComponentDefinition>("user://ModEditorFireVolleyPresenterProbe/FireVolleyPresenter.tres", "", ResourceLoader.CacheMode.Ignore);
			saveReload &= GodotObject.IsInstanceValid(fireComponentDefinition) && fireComponentDefinition.firePosMarkerPaths.Count == 1 && fireComponentDefinition.firePosMarkerPaths[0].ToString() == "MuzzleAlt" && fireComponentDefinition.randomChoose && fireComponentDefinition.fireProjectileList.Count == 1 && Mathf.IsEqualApprox(fireComponentDefinition.fireProjectileList[0].speed, 455f);
			Require(saveReload & oldPresenterReleased, "Fire 编辑未通过保存/忽略缓存重载，或旧 presenter 未释放。");
			PanelContainer panelContainer = _editor.FindChild("InspectorPanel", recursive: true, owned: false) as PanelContainer;
			bool inspectorUntouched = GodotObject.IsInstanceValid(panelContainer) && !panelContainer.Visible && _editor.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null && inspector?.CurrentObject == inspectorSentinel;
			Require(inspectorUntouched, "Fire presenter 暴露或替换了 Inspector。");
			presenter = _editor.FireVolleyPresenter;
			XWEditorInterface.Instance.FocusPanel("bp_editor");
			await WaitFrames(4);
			bool flag4 = GodotObject.IsInstanceValid(presenter?.Root) && !_editor.IsVisibleInTree() && _editor.ProcessMode == ProcessModeEnum.Disabled && presenter.IsProcessIdle;
			Require(flag4, "隐藏后的 Fire presenter 仍在处理输入或帧。");
			GD.Print($"[MOD_EDITOR_FIRE_VOLLEY_PRESENTER_PROBE] f3={f3} route={route} gamePosition={gamePosition} markerDrag={markerDragContract} targetCards={targetCards} timeline={timeline} trajectoryCards={trajectoryCards} remainingDynamic={remainingDynamic} noDuplicates={noDuplicates} inPlaceRefresh={inPlaceRefresh} undoRedo={undoRedo} atomicPriority={atomicPriority} runtimeSemantics={runtimeSemantics} saveReload={saveReload} oldPresenterReleased={oldPresenterReleased} inspectorUntouched={inspectorUntouched} hiddenStopped={flag4} previewNonBlocking={previewNonBlocking} failures={_failures.Count}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private static bool ProbeRuntimeSemantics()
	{
		int num = 1;
		int num2 = 2;
		int idleFireCheckPriorityPassCount = FireComponent.GetIdleFireCheckPriorityPassCount(airFirst: false, checkUse: true);
		int idleFireCheckPriorityPassCount2 = FireComponent.GetIdleFireCheckPriorityPassCount(airFirst: true, checkUse: true);
		int num3 = SelectFirstEligible(new int[2] { num, num2 }, new bool[2] { true, true }, idleFireCheckPriorityPassCount);
		int num4 = SelectFirstEligible(new int[2] { num, num2 }, new bool[2] { true, true }, idleFireCheckPriorityPassCount2);
		int collisionFlags = FireComponent.ResolveEffectiveCheckCollisionFlags(-1, num);
		bool flag = !FireComponent.IsIdleFireCheckInPriorityPass(collisionFlags, 0, idleFireCheckPriorityPassCount2) && FireComponent.IsIdleFireCheckInPriorityPass(collisionFlags, 1, idleFireCheckPriorityPassCount2);
		Vector2 vector = new Vector2(400f, 220f);
		FireComponent.ResolveOffsetLineRoute(vector, 1, 2, 3, 56f, 0.73f, 41f, out var routedPosition, out var yOffsetTarget, out var yOffsetDuration);
		FireComponent.ResolveOffsetLineRoute(vector, 1, 3, 3, 56f, 0.73f, 41f, out var routedPosition2, out var yOffsetTarget2, out var yOffsetDuration2);
		bool flag2 = routedPosition.IsEqualApprox(vector) && Mathf.IsEqualApprox(yOffsetTarget, 56f) && Mathf.IsEqualApprox(yOffsetDuration, 0.73f) && routedPosition2.IsEqualApprox(new Vector2(359f, 220f)) && Mathf.IsZeroApprox(yOffsetTarget2) && Mathf.IsZeroApprox(yOffsetDuration2);
		bool flag3 = FireComponent.IsGargantuarPhysiqueBlocked(canTargetGargantuar: false, TowerDefenseEnum.ZOMBIE_PHYSIQUE.HUGE) && !FireComponent.IsGargantuarPhysiqueBlocked(canTargetGargantuar: true, TowerDefenseEnum.ZOMBIE_PHYSIQUE.HUGE) && !FireComponent.IsGargantuarPhysiqueBlocked(canTargetGargantuar: false, TowerDefenseEnum.ZOMBIE_PHYSIQUE.MID);
		return (idleFireCheckPriorityPassCount == 1 && idleFireCheckPriorityPassCount2 == 2 && num3 == 0 && num4 == 1) & flag & flag2 & flag3;
	}

	private static int SelectFirstEligible(IReadOnlyList<int> flags, IReadOnlyList<bool> eligible, int passes)
	{
		for (int i = 0; i < passes; i++)
		{
			for (int j = 0; j < flags.Count; j++)
			{
				if (eligible[j] && FireComponent.IsIdleFireCheckInPriorityPass(flags[j], i, passes))
				{
					return j;
				}
			}
		}
		return -1;
	}

	private async Task<bool> WaitForInterface(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			XWEditorInterface instance = XWEditorInterface.Instance;
			Control control = instance?.GetEditorPanel();
			Node instance2 = control?.FindChild("LoadingOverlay", recursive: true, owned: false);
			if (GodotObject.IsInstanceValid(control) && !GodotObject.IsInstanceValid(instance2) && instance.GetInspector() is XWInspector)
			{
				(control.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control)?.Hide();
				_history = instance.GetUndoRedoManager();
				return GodotObject.IsInstanceValid(_history);
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> WaitForComponentEditor(FireComponentDefinition definition, int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			_editor = XWEditorInterface.Instance?.GetResourceEditor("character_component_editor") as XWCharacterComponentVisualResourceEditor;
			if (GodotObject.IsInstanceValid(_editor) && _editor.SelectedDefinition == definition && GodotObject.IsInstanceValid(_editor.FireVolleyPresenter?.Root))
			{
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private static bool HasGenericPropertyControlOutsidePresenter(Node root, Control presenterRoot, string property)
	{
		foreach (Node item in root.FindChildren("*", "", recursive: true, owned: false))
		{
			if (item is Control control && !presenterRoot.IsAncestorOf(control) && (control.TooltipText == property || control.TooltipText.StartsWith(property + " ·", StringComparison.Ordinal)))
			{
				return true;
			}
		}
		return false;
	}

	private static int CountGenericSpecializedControls(Node root, Control presenterRoot)
	{
		int num = 0;
		foreach (string specializedProperty in XWFireVolleyPresenter.SpecializedProperties)
		{
			foreach (Node item in root.FindChildren("*", "", recursive: true, owned: false))
			{
				if (item is Control control && !presenterRoot.IsAncestorOf(control) && (control.TooltipText == specializedProperty || control.TooltipText.StartsWith(specializedProperty + " ·", StringComparison.Ordinal) || item.Name == (StringName)("EnumSource_" + specializedProperty)))
				{
					num++;
				}
			}
		}
		return num;
	}

	private static bool IsDescendantOf(Node child, Node expectedAncestor)
	{
		if (GodotObject.IsInstanceValid(child) && GodotObject.IsInstanceValid(expectedAncestor))
		{
			return expectedAncestor.IsAncestorOf(child);
		}
		return false;
	}

	private static bool HasAll(Node root, params string[] names)
	{
		foreach (string pattern in names)
		{
			if (!GodotObject.IsInstanceValid(root?.FindChild(pattern, recursive: true, owned: false)))
			{
				return false;
			}
		}
		return true;
	}

	private static T Find<T>(Node root, string name) where T : Node
	{
		return root?.FindChild(name, recursive: true, owned: false) as T;
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
			GD.PrintErr("[MOD_EDITOR_FIRE_VOLLEY_PRESENTER_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_FIRE_VOLLEY_PRESENTER_FAILURE] " + failure);
		}
		string path = ProjectSettings.GlobalizePath("user://ModEditorFireVolleyPresenterProbe");
		if (Directory.Exists(path))
		{
			try
			{
				Directory.Delete(path, recursive: true);
			}
			catch
			{
			}
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProbeRuntimeSemantics, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.HasGenericPropertyControlOutsidePresenter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "presenterRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountGenericSpecializedControls, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "presenterRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsDescendantOf, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "expectedAncestor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasAll, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "names", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ProbeRuntimeSemantics && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ProbeRuntimeSemantics());
			return true;
		}
		if (method == MethodName.HasGenericPropertyControlOutsidePresenter && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(HasGenericPropertyControlOutsidePresenter(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.CountGenericSpecializedControls && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountGenericSpecializedControls(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.IsDescendantOf && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDescendantOf(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.HasAll && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasAll(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
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
		if (method == MethodName.ProbeRuntimeSemantics && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ProbeRuntimeSemantics());
			return true;
		}
		if (method == MethodName.HasGenericPropertyControlOutsidePresenter && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(HasGenericPropertyControlOutsidePresenter(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.CountGenericSpecializedControls && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountGenericSpecializedControls(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.IsDescendantOf && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDescendantOf(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.HasAll && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasAll(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
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
		if (method == MethodName.ProbeRuntimeSemantics)
		{
			return true;
		}
		if (method == MethodName.HasGenericPropertyControlOutsidePresenter)
		{
			return true;
		}
		if (method == MethodName.CountGenericSpecializedControls)
		{
			return true;
		}
		if (method == MethodName.IsDescendantOf)
		{
			return true;
		}
		if (method == MethodName.HasAll)
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
		if (name == PropertyName._history)
		{
			_history = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XWCharacterComponentVisualResourceEditor>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._history)
		{
			value = VariantUtils.CreateFrom(in _history);
			return true;
		}
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
			new PropertyInfo(Variant.Type.Object, PropertyName._history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._history, Variant.From(in _history));
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._history, out var value))
		{
			_history = value.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._editor, out var value2))
		{
			_editor = value2.As<XWCharacterComponentVisualResourceEditor>();
		}
	}
}
