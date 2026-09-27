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

[ScriptPath("res://Tests/ModEditorExplosionStagePresenterRuntimeProbe.cs")]
public class ModEditorExplosionStagePresenterRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName DragOverlay = "DragOverlay";

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

	private const string ResourcePath = "user://ModEditorExplosionStagePresenterProbe/ExplosionStagePresenter.tres";

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
			string path = ProjectSettings.GlobalizePath("user://ModEditorExplosionStagePresenterProbe");
			if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
			}
			Directory.CreateDirectory(path);
			ExplodeComponentDefinition definition = new ExplodeComponentDefinition
			{
				ComponentTypeId = "ExplodeComponent",
				DefinitionId = "probe.explosion.stage",
				InstanceId = "probe.explosion.stage.instance",
				explodeMethod = "Range",
				explodeUse = true,
				explodeRange = new Vector2(2f, 1f),
				scaleExplodeShapeToMapGrid = true,
				explodeAudio = "Doom",
				cameraShakeUse = true,
				cameraShakeOffset = new Vector2(8f, 5f),
				cameraShakeForce = 0.4f,
				cameraShakeInterval = 0.05f,
				cameraShakeTime = 1,
				screenColorBlinkUse = true,
				screenColorBlinkColor = new Color(1f, 0.35f, 0.2f, 0.6f),
				screenColorBlinkDuration = 0.4f,
				craterCreateUse = true,
				craterCreatePacketName = "Crater"
			};
			Require(ResourceSaver.Save(definition, "user://ModEditorExplosionStagePresenterProbe/ExplosionStagePresenter.tres", ResourceSaver.SaverFlags.None) == Error.Ok, "Explosion 探针资源保存失败。");
			definition = ResourceLoader.Load<ExplodeComponentDefinition>("user://ModEditorExplosionStagePresenterProbe/ExplosionStagePresenter.tres", "", ResourceLoader.CacheMode.Ignore);
			Require(GodotObject.IsInstanceValid(definition), "Explosion 探针资源无法忽略缓存重载。");
			Node inspectorSentinel = new Node
			{
				Name = "ExplosionInspectorSentinel"
			};
			AddChild(inspectorSentinel, forceReadableName: false, InternalMode.Disabled);
			XWInspector inspector = XWEditorInterface.Instance.GetInspector() as XWInspector;
			inspector?.EditObject(inspectorSentinel);
			await WaitFrames(2);
			bool route = XWResourceEditorRegistry.TryGetEditor(definition, "user://ModEditorExplosionStagePresenterProbe/ExplosionStagePresenter.tres", out var descriptor) && descriptor.Category == "CharacterComponent" && descriptor.DockKey == "character_component_editor";
			Require(route, "Explode 定义未路由到角色组件编辑器。");
			XWEditorInterface.Instance.EditResource(definition, XWResourceEditContext.ForRoot(definition, "user://ModEditorExplosionStagePresenterProbe/ExplosionStagePresenter.tres", "character_component_editor"));
			XWEditorInterface.Instance.FocusPanel("character_component_editor");
			bool flag = await WaitForComponentEditor(definition, 900);
			Require(flag, "角色组件编辑器未挂载 ExplosionStagePresenter。");
			if (!flag)
			{
				Finish();
				return;
			}
			_editor.SetWorkbenchPage(0);
			await WaitFrames(3);
			XWExplosionStagePresenter presenter = _editor.ExplosionStagePresenter;
			PanelContainer root = Find<PanelContainer>(_editor, "ExplosionStagePresenter");
			Node2D node2D = Find<Node2D>(_editor, "PreviewWorld");
			XWExplosionStageOverlay overlay = Find<XWExplosionStageOverlay>(_editor, "ExplosionStageOverlay");
			bool gamePosition = GodotObject.IsInstanceValid(presenter?.Root) && root == presenter.Root && GodotObject.IsInstanceValid(node2D) && overlay?.GetParent() == node2D && IsDescendantOf(root, Find<Node>(_editor, "ComponentPropertyHost")) && _editor.IsPreviewViewportRendering && presenter.OverlayVisible && presenter.OverlayInputActive;
			Require(gamePosition, "Explosion 游戏位置面板或 PreviewWorld overlay 未正确挂载。");
			bool methodCards = presenter.MethodCardCount == 5 && HasAll(_editor, "ExplosionMethod_Range", "ExplosionMethod_Line", "ExplosionMethod_Row", "ExplosionMethod_Cross", "ExplosionMethod_Slash", "ExplosionStageTimeline", "ExplosionOffsetRows");
			Require(methodCards, "Explosion 五种方法图形卡或时间轴不完整。");
			Node2D stageOwner = new Node2D
			{
				Name = "ExplosionStageOwner",
				Position = new Vector2(120f, 80f),
				ProcessMode = ProcessModeEnum.Disabled
			};
			node2D.AddChild(stageOwner, forceReadableName: false, InternalMode.Disabled);
			presenter.BindPreviewCharacter(stageOwner);
			overlay.QueueRedraw();
			await WaitFrames(3);
			Vector2 vector = overlay.ToLocal(stageOwner.GlobalPosition);
			bool rangeGeometry = overlay.LastDrawnMethod == "Range" && overlay.LastRangeRect.Size.IsEqualApprox(new Vector2(320f, 196f)) && overlay.LastRangeRect.GetCenter().IsEqualApprox(vector) && overlay.LastEffectPosition.IsEqualApprox(vector - new Vector2(0f, 30f));
			Require(rangeGeometry, "Range 网格框或 Effect -30Y 锚点没有按 runtime 语义绘制。");
			bool remainingDynamic = HasGenericPropertyControlOutsidePresenter(_editor, root, "explodeEvent") && HasGenericPropertyControlOutsidePresenter(_editor, root, "explodeEffect") && HasGenericPropertyControlOutsidePresenter(_editor, root, "explodeShape");
			bool noDuplicates = XWExplosionStagePresenter.SpecializedProperties.Count == 30 && CountGenericSpecializedControls(_editor, root) == 0;
			Require(remainingDynamic & noDuplicates, "Explosion 剩余 metadata 字段缺失或专用字段被通用表单重复渲染。");
			bool validation = presenter.ValidationText.Contains("Range 已启用但 explodeEvent 为空", StringComparison.Ordinal) && !XWExplosionStagePresenter.IsSupportedMethod("range") && XWExplosionStagePresenter.IsSupportedMethod("Range");
			Require(validation, "Range 空事件或未知方法的严格校验未生效。");
			SpinBox rangeX = Find<SpinBox>(_editor, "ExplosionRangeX");
			ulong rootId = presenter.Root.GetInstanceId();
			ulong rangeId = rangeX?.GetInstanceId() ?? 0;
			int fullBefore = _editor.FullSurfaceRefreshCount;
			int runtimeBuildBefore = _editor.RuntimePreviewBuildCount;
			int previewBefore = presenter.PreviewRevision;
			Stopwatch refreshWatch = Stopwatch.StartNew();
			if (GodotObject.IsInstanceValid(rangeX))
			{
				rangeX.EmitSignal(Control.SignalName.FocusEntered);
				double[] array = new double[5] { 2.2, 2.4, 2.6, 2.8, 3.0 };
				for (int i = 0; i < array.Length; i++)
				{
					double num = (rangeX.Value = array[i]);
					rangeX.EmitSignal(Godot.Range.SignalName.ValueChanged, num);
				}
			}
			refreshWatch.Stop();
			await WaitFrames(2);
			int num3;
			if (presenter.Root.GetInstanceId() == rootId)
			{
				SpinBox spinBox = Find<SpinBox>(_editor, "ExplosionRangeX");
				if (spinBox != null && spinBox.GetInstanceId() == rangeId && _editor.FullSurfaceRefreshCount == fullBefore)
				{
					num3 = ((presenter.PreviewRevision > previewBefore) ? 1 : 0);
					goto IL_0bbf;
				}
			}
			num3 = 0;
			goto IL_0bbf;
			IL_0bbf:
			bool inPlaceRefresh = (byte)num3 != 0;
			bool previewNonBlocking = inPlaceRefresh && _editor.RuntimePreviewBuildCount == runtimeBuildBefore && refreshWatch.ElapsedMilliseconds < 1000;
			rangeX?.EmitSignal(Control.SignalName.FocusExited);
			await WaitFrames(3);
			Require(inPlaceRefresh & previewNonBlocking, "连续 Range 编辑重建了控件/整面板/运行时，或同步刷新发生阻塞。");
			_history.ClearHistory();
			rangeX = Find<SpinBox>(_editor, "ExplosionRangeX");
			rangeId = rangeX?.GetInstanceId() ?? 0;
			if (GodotObject.IsInstanceValid(rangeX))
			{
				rangeX.EmitSignal(Control.SignalName.FocusEntered);
				rangeX.Value = 3.5;
				rangeX.EmitSignal(Godot.Range.SignalName.ValueChanged, 3.5);
				rangeX.EmitSignal(Control.SignalName.FocusExited);
			}
			await WaitFrames(3);
			bool applied = Mathf.IsEqualApprox(definition.explodeRange.X, 3.5f) && _history.HasUndo();
			bool undone = _history.Undo();
			await WaitFrames(3);
			bool flag2 = undone;
			int num4;
			if (Mathf.IsEqualApprox(definition.explodeRange.X, 3f))
			{
				SpinBox spinBox2 = Find<SpinBox>(_editor, "ExplosionRangeX");
				num4 = ((spinBox2 != null && spinBox2.GetInstanceId() == rangeId) ? 1 : 0);
			}
			else
			{
				num4 = 0;
			}
			undone = (byte)((flag2 ? 1u : 0u) & (uint)num4) != 0;
			bool redone = _history.Redo();
			await WaitFrames(3);
			bool flag3 = redone;
			int num5;
			if (Mathf.IsEqualApprox(definition.explodeRange.X, 3.5f))
			{
				SpinBox spinBox3 = Find<SpinBox>(_editor, "ExplosionRangeX");
				num5 = ((spinBox3 != null && spinBox3.GetInstanceId() == rangeId) ? 1 : 0);
			}
			else
			{
				num5 = 0;
			}
			redone = (byte)((flag3 ? 1u : 0u) & (uint)num5) != 0;
			bool undoRedo = applied & undone & redone;
			Require(undoRedo, "Range 标量未通过原位 Undo/Redo 往返。");
			_history.ClearHistory();
			Find<Button>(_editor, "ExplosionMethod_Line")?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(3);
			bool geometryApplied = definition.explodeMethod == "Line" && definition.explodeJalaOffset.Count == 1 && definition.explodeJalaOffset[0] == 0 && _history.HasUndo();
			bool geometryUndone = _history.Undo();
			await WaitFrames(3);
			geometryUndone &= definition.explodeMethod == "Range" && definition.explodeJalaOffset.Count == 0 && !_history.HasUndo();
			bool geometryRedone = _history.Redo();
			await WaitFrames(3);
			bool flag4 = geometryRedone;
			int num6;
			if (definition.explodeMethod == "Line" && definition.explodeJalaOffset.Count == 1)
			{
				XWExplosionStageOverlay xWExplosionStageOverlay = Find<XWExplosionStageOverlay>(_editor, "ExplosionStageOverlay");
				num6 = ((xWExplosionStageOverlay != null && xWExplosionStageOverlay.LastDrawnOffsetCount == 1) ? 1 : 0);
			}
			else
			{
				num6 = 0;
			}
			geometryRedone = (byte)((flag4 ? 1u : 0u) & (uint)num6) != 0;
			bool atomicGeometry = geometryApplied & geometryUndone & geometryRedone;
			Require(atomicGeometry, "爆炸方法与必要 offset 没有形成单个原子 Undo。");
			_history.ClearHistory();
			overlay.QueueRedraw();
			await WaitFrames(3);
			int dragFullBefore = _editor.FullSurfaceRefreshCount;
			int dragRuntimeBefore = _editor.RuntimePreviewBuildCount;
			ulong dragRootId = presenter.Root.GetInstanceId();
			Vector2 offsetHandlePosition = overlay.GetOffsetHandlePosition(0);
			Vector2 toLocal = offsetHandlePosition + new Vector2(0f, 196f);
			DragOverlay(overlay, offsetHandlePosition, toLocal);
			await WaitFrames(3);
			bool offsetApplied = definition.explodeJalaOffset.Count == 1 && definition.explodeJalaOffset[0] == 2 && _history.HasUndo();
			bool offsetUndone = _history.Undo();
			await WaitFrames(3);
			offsetUndone &= definition.explodeJalaOffset[0] == 0 && !_history.HasUndo();
			bool offsetRedone = _history.Redo();
			await WaitFrames(3);
			offsetRedone &= definition.explodeJalaOffset[0] == 2;
			definition.scaleExplodeShapeToMapGrid = false;
			_history.ClearHistory();
			Find<Button>(_editor, "ExplosionMethod_Range")?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(3);
			bool rangeMethodApplied = definition.explodeMethod == "Range" && !definition.scaleExplodeShapeToMapGrid && _history.HasUndo();
			bool rangeMethodUndone = _history.Undo();
			await WaitFrames(3);
			rangeMethodUndone &= definition.explodeMethod == "Line" && !definition.scaleExplodeShapeToMapGrid && !_history.HasUndo();
			bool rangeMethodRedone = _history.Redo();
			await WaitFrames(3);
			rangeMethodRedone &= definition.explodeMethod == "Range" && !definition.scaleExplodeShapeToMapGrid;
			bool authorityPreserved = rangeMethodApplied & rangeMethodUndone & rangeMethodRedone;
			_history.ClearHistory();
			overlay.QueueRedraw();
			await WaitFrames(3);
			Vector2 lastRangeHandlePosition = overlay.LastRangeHandlePosition;
			Vector2 toLocal2 = overlay.LastRangeRect.GetCenter() + new Vector2(320f, 196f);
			DragOverlay(overlay, lastRangeHandlePosition, toLocal2);
			await WaitFrames(3);
			bool rangeDragApplied = definition.explodeRange.IsEqualApprox(new Vector2(4f, 2f)) && _history.HasUndo();
			bool rangeDragUndone = _history.Undo();
			await WaitFrames(3);
			rangeDragUndone &= definition.explodeRange.IsEqualApprox(new Vector2(3.5f, 1f)) && !_history.HasUndo();
			bool rangeDragRedone = _history.Redo();
			await WaitFrames(3);
			rangeDragRedone &= definition.explodeRange.IsEqualApprox(new Vector2(4f, 2f));
			bool directDrag = (offsetApplied & offsetUndone & offsetRedone & rangeDragApplied & rangeDragUndone & rangeDragRedone) && presenter.Root.GetInstanceId() == dragRootId && _editor.FullSurfaceRefreshCount == dragFullBefore && _editor.RuntimePreviewBuildCount == dragRuntimeBefore;
			Require(directDrag, "Explosion Overlay 鼠标拖拽未吸附、未形成单次 Undo，或重建了 Root/运行时。");
			Require(authorityPreserved, "切换 Range 不应改写独立的 explodeShape/网格范围权威开关。");
			bool saveReload = _editor.SaveActiveResource();
			await WaitFrames(6);
			bool oldPresenterReleased = presenter.Root == null;
			ExplodeComponentDefinition explodeComponentDefinition = ResourceLoader.Load<ExplodeComponentDefinition>("user://ModEditorExplosionStagePresenterProbe/ExplosionStagePresenter.tres", "", ResourceLoader.CacheMode.Ignore);
			saveReload &= GodotObject.IsInstanceValid(explodeComponentDefinition) && explodeComponentDefinition.explodeMethod == "Range" && explodeComponentDefinition.explodeJalaOffset.Count == 1 && explodeComponentDefinition.explodeJalaOffset[0] == 2 && explodeComponentDefinition.explodeRange.IsEqualApprox(new Vector2(4f, 2f)) && !explodeComponentDefinition.scaleExplodeShapeToMapGrid;
			Require(saveReload & oldPresenterReleased, "Explosion 编辑未通过保存/忽略缓存重载，或旧 presenter 未释放。");
			PanelContainer panelContainer = _editor.FindChild("InspectorPanel", recursive: true, owned: false) as PanelContainer;
			bool inspectorUntouched = GodotObject.IsInstanceValid(panelContainer) && !panelContainer.Visible && _editor.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null && inspector?.CurrentObject == inspectorSentinel;
			Require(inspectorUntouched, "Explosion presenter 暴露或替换了 Inspector。");
			presenter = _editor.ExplosionStagePresenter;
			XWEditorInterface.Instance.FocusPanel("bp_editor");
			await WaitFrames(4);
			bool flag5 = GodotObject.IsInstanceValid(presenter?.Root) && !_editor.IsVisibleInTree() && _editor.ProcessMode == ProcessModeEnum.Disabled && presenter.IsProcessIdle;
			Require(flag5, "隐藏后的 Explosion presenter 仍在处理输入或帧。");
			GD.Print($"[MOD_EDITOR_EXPLOSION_STAGE_PRESENTER_PROBE] f3={f3} route={route} gamePosition={gamePosition} methodCards={methodCards} rangeGeometry={rangeGeometry} remainingDynamic={remainingDynamic} noDuplicates={noDuplicates} inPlaceRefresh={inPlaceRefresh} undoRedo={undoRedo} atomicGeometry={atomicGeometry} directDrag={directDrag} authorityPreserved={authorityPreserved} validation={validation} saveReload={saveReload} oldPresenterReleased={oldPresenterReleased} inspectorUntouched={inspectorUntouched} hiddenStopped={flag5} previewNonBlocking={previewNonBlocking} failures={_failures.Count}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private static void DragOverlay(XWExplosionStageOverlay overlay, Vector2 fromLocal, Vector2 toLocal)
	{
		Viewport viewport = overlay.GetViewport();
		Vector2 vector = overlay.GetGlobalTransformWithCanvas() * fromLocal;
		Vector2 vector2 = overlay.GetGlobalTransformWithCanvas() * toLocal;
		viewport.PushInput(new InputEventMouseButton
		{
			Position = vector,
			GlobalPosition = vector,
			ButtonIndex = MouseButton.Left,
			ButtonMask = MouseButtonMask.Left,
			Pressed = true
		}, inLocalCoords: true);
		viewport.PushInput(new InputEventMouseMotion
		{
			Position = vector2,
			GlobalPosition = vector2,
			Relative = vector2 - vector,
			ButtonMask = MouseButtonMask.Left
		}, inLocalCoords: true);
		viewport.PushInput(new InputEventMouseButton
		{
			Position = vector2,
			GlobalPosition = vector2,
			ButtonIndex = MouseButton.Left,
			ButtonMask = (MouseButtonMask)0L,
			Pressed = false
		}, inLocalCoords: true);
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

	private async Task<bool> WaitForComponentEditor(ExplodeComponentDefinition definition, int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			_editor = XWEditorInterface.Instance?.GetResourceEditor("character_component_editor") as XWCharacterComponentVisualResourceEditor;
			if (GodotObject.IsInstanceValid(_editor) && _editor.SelectedDefinition == definition && GodotObject.IsInstanceValid(_editor.ExplosionStagePresenter?.Root))
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
		foreach (string specializedProperty in XWExplosionStagePresenter.SpecializedProperties)
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
			GD.PrintErr("[MOD_EDITOR_EXPLOSION_STAGE_PRESENTER_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_EXPLOSION_STAGE_PRESENTER_FAILURE] " + failure);
		}
		string path = ProjectSettings.GlobalizePath("user://ModEditorExplosionStagePresenterProbe");
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
			new MethodInfo(MethodName.DragOverlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "overlay", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "fromLocal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "toLocal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.DragOverlay && args.Count == 3)
		{
			DragOverlay(VariantUtils.ConvertTo<XWExplosionStageOverlay>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
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
		if (method == MethodName.DragOverlay && args.Count == 3)
		{
			DragOverlay(VariantUtils.ConvertTo<XWExplosionStageOverlay>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
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
		if (method == MethodName.DragOverlay)
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
