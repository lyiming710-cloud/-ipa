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

[ScriptPath("res://Tests/ModEditorAttackContactPresenterRuntimeProbe.cs")]
public class ModEditorAttackContactPresenterRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private const string ResourcePath = "user://ModEditorAttackContactPresenterProbe/AttackContactPresenter.tres";

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
			string path = ProjectSettings.GlobalizePath("user://ModEditorAttackContactPresenterProbe");
			if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
			}
			Directory.CreateDirectory(path);
			AttackComponentDefinition definition = new AttackComponentDefinition
			{
				ComponentTypeId = "AttackComponent",
				DefinitionId = "probe.attack.contact",
				InstanceId = "probe.attack.contact.instance",
				attackType = "Eat",
				attackAnimeClips = "Attack",
				attackEventName = "attack",
				attackAnimeTimeScale = 1.0,
				attackIntervalBase = 1.5,
				attackInterval = 1.5,
				attackIntervalOffset = 0.1,
				checkIntreval = 5,
				checkGrid = true,
				checkLine = false,
				useCheckAreaGridColumn = false,
				checkEachShape = true,
				checkGravestone = true
			};
			Require(ResourceSaver.Save(definition, "user://ModEditorAttackContactPresenterProbe/AttackContactPresenter.tres", ResourceSaver.SaverFlags.None) == Error.Ok, "Attack 探针资源保存失败。");
			definition = ResourceLoader.Load<AttackComponentDefinition>("user://ModEditorAttackContactPresenterProbe/AttackContactPresenter.tres", "", ResourceLoader.CacheMode.Ignore);
			Require(GodotObject.IsInstanceValid(definition), "Attack 探针资源无法忽略缓存重载。");
			Node inspectorSentinel = new Node
			{
				Name = "AttackInspectorSentinel"
			};
			AddChild(inspectorSentinel, forceReadableName: false, InternalMode.Disabled);
			XWInspector inspector = XWEditorInterface.Instance.GetInspector() as XWInspector;
			inspector?.EditObject(inspectorSentinel);
			await WaitFrames(2);
			bool route = XWResourceEditorRegistry.TryGetEditor(definition, "user://ModEditorAttackContactPresenterProbe/AttackContactPresenter.tres", out var descriptor) && descriptor.Category == "CharacterComponent" && descriptor.DockKey == "character_component_editor";
			Require(route, "Attack 定义未路由到角色组件编辑器。");
			XWEditorInterface.Instance.EditResource(definition, XWResourceEditContext.ForRoot(definition, "user://ModEditorAttackContactPresenterProbe/AttackContactPresenter.tres", "character_component_editor"));
			XWEditorInterface.Instance.FocusPanel("character_component_editor");
			bool flag = await WaitForComponentEditor(definition, 900);
			Require(flag, "角色组件编辑器未挂载 AttackContactPresenter。");
			if (!flag)
			{
				Finish();
				return;
			}
			_editor.SetWorkbenchPage(0);
			await WaitFrames(3);
			XWAttackContactPresenter presenter = _editor.AttackContactPresenter;
			PanelContainer panelContainer = Find<PanelContainer>(_editor, "AttackContactPresenter");
			Node2D node2D = Find<Node2D>(_editor, "PreviewWorld");
			Node2D node2D2 = Find<Node2D>(_editor, "AttackContactPreviewOverlay");
			bool gamePosition = GodotObject.IsInstanceValid(presenter?.Root) && panelContainer == presenter.Root && GodotObject.IsInstanceValid(node2D) && node2D2?.GetParent() == node2D && IsDescendantOf(panelContainer, Find<Node>(_editor, "ComponentPropertyHost")) && _editor.IsPreviewViewportRendering && presenter.OverlayVisible && presenter.OverlayInputActive;
			Require(gamePosition, "Attack 游戏位置面板或 PreviewWorld overlay 未正确挂载。");
			bool targetCards = presenter.TargetCardCount == 6 && presenter.TargetDummyCount == 6 && HasAll(_editor, "AttackChoice_geometry-0", "AttackChoice_geometry-1", "AttackChoice_geometry-2", "AttackChoice_geometry-3", "AttackChoice_geometry-4", "AttackChoice_geometry-5", "AttackTallToggle", "AttackVaseToggle", "AttackBowlingToggle", "AttackGravestoneToggle", "AttackAllToggle", "AttackLadderToggle");
			bool timeline = HasAll(_editor, "AttackHitTimeline", "AttackAnimationClip", "AttackEventName", "AttackAnimationTimeScale", "AttackIntervalBase", "AttackInterval", "AttackIntervalOffset", "AttackCompletionClipRows");
			bool collisionReuse = _editor.CurrentRuntimePreview is AttackComponent && presenter.OverlayRedrawRevision > 0;
			Require(targetCards & timeline & collisionReuse, "Attack 目标卡、时间轴或真实碰撞预览绑定不完整。");
			bool remainingDynamic = HasGenericPropertyControlOutsidePresenter(_editor, panelContainer, "checkShapeResources") && HasGenericPropertyControlOutsidePresenter(_editor, panelContainer, "eventList") && HasGenericPropertyControlOutsidePresenter(_editor, panelContainer, "eatAudio");
			bool noDuplicates = XWAttackContactPresenter.SpecializedProperties.Count == 21 && CountGenericSpecializedControls(_editor, panelContainer) == 0;
			Require(remainingDynamic & noDuplicates, "Attack 剩余 metadata 字段缺失或专用字段被通用表单重复渲染。");
			SpinBox interval = Find<SpinBox>(_editor, "AttackInterval");
			ulong rootId = presenter.Root.GetInstanceId();
			ulong intervalId = interval?.GetInstanceId() ?? 0;
			int fullBefore = _editor.FullSurfaceRefreshCount;
			int runtimeBuildBefore = _editor.RuntimePreviewBuildCount;
			int previewBefore = presenter.PreviewRevision;
			Stopwatch refreshWatch = Stopwatch.StartNew();
			if (GodotObject.IsInstanceValid(interval))
			{
				interval.EmitSignal(Control.SignalName.FocusEntered);
				for (double num = 1.6; num <= 2.3; num += 0.1)
				{
					interval.Value = num;
					interval.EmitSignal(Godot.Range.SignalName.ValueChanged, interval.Value);
				}
			}
			refreshWatch.Stop();
			await WaitFrames(2);
			int num2;
			if (presenter.Root.GetInstanceId() == rootId)
			{
				SpinBox spinBox = Find<SpinBox>(_editor, "AttackInterval");
				if (spinBox != null && spinBox.GetInstanceId() == intervalId && _editor.FullSurfaceRefreshCount == fullBefore)
				{
					num2 = ((presenter.PreviewRevision > previewBefore) ? 1 : 0);
					goto IL_0a4b;
				}
			}
			num2 = 0;
			goto IL_0a4b;
			IL_0a4b:
			bool inPlaceRefresh = (byte)num2 != 0;
			bool previewNonBlocking = inPlaceRefresh && _editor.RuntimePreviewBuildCount == runtimeBuildBefore && refreshWatch.ElapsedMilliseconds < 1000;
			interval?.EmitSignal(Control.SignalName.FocusExited);
			await WaitFrames(3);
			Require(inPlaceRefresh & previewNonBlocking, "连续攻击间隔编辑重建了控件/整面板/运行时，或同步刷新阻塞。");
			_history.ClearHistory();
			interval = Find<SpinBox>(_editor, "AttackInterval");
			if (GodotObject.IsInstanceValid(interval))
			{
				interval.EmitSignal(Control.SignalName.FocusEntered);
				interval.Value = 3.25;
				interval.EmitSignal(Godot.Range.SignalName.ValueChanged, interval.Value);
				interval.EmitSignal(Control.SignalName.FocusExited);
			}
			await WaitFrames(3);
			bool applied = Mathf.IsEqualApprox((float)definition.attackInterval, 3.25f) && _history.HasUndo();
			bool undone = _history.Undo();
			await WaitFrames(3);
			bool flag2 = undone;
			int num3;
			if (!Mathf.IsEqualApprox((float)definition.attackInterval, 3.25f))
			{
				SpinBox spinBox2 = Find<SpinBox>(_editor, "AttackInterval");
				num3 = ((spinBox2 != null && spinBox2.GetInstanceId() == intervalId) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
			undone = (byte)((flag2 ? 1u : 0u) & (uint)num3) != 0;
			bool redone = _history.Redo();
			await WaitFrames(3);
			bool flag3 = redone;
			int num4;
			if (Mathf.IsEqualApprox((float)definition.attackInterval, 3.25f))
			{
				SpinBox spinBox3 = Find<SpinBox>(_editor, "AttackInterval");
				num4 = ((spinBox3 != null && spinBox3.GetInstanceId() == intervalId) ? 1 : 0);
			}
			else
			{
				num4 = 0;
			}
			redone = (byte)((flag3 ? 1u : 0u) & (uint)num4) != 0;
			bool undoRedo = applied & undone & redone;
			Require(undoRedo, "攻击间隔未通过原位 Undo/Redo 往返。");
			_history.ClearHistory();
			Find<Button>(_editor, "AttackChoice_geometry-5")?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(3);
			bool targetApplied = definition.checkLine && definition.checkGrid && definition.useCheckAreaGridColumn && _history.HasUndo();
			bool targetUndone = _history.Undo();
			await WaitFrames(3);
			targetUndone &= !definition.checkLine && definition.checkGrid && !definition.useCheckAreaGridColumn && !_history.HasUndo();
			bool targetRedone = _history.Redo();
			await WaitFrames(3);
			targetRedone &= definition.checkLine && definition.checkGrid && definition.useCheckAreaGridColumn;
			bool atomicTarget = targetApplied & targetUndone & targetRedone;
			Require(atomicTarget, "六种目标空间组合没有形成单一原子 Undo。");
			_history.ClearHistory();
			string structureBefore = presenter.StructureToken;
			int completionBefore = presenter.CompletionRowCount;
			Find<Button>(_editor, "AttackAddCompletionClipButton")?.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(3);
			bool structureApplied = presenter.CompletionRowCount == completionBefore + 1 && presenter.StructureToken != structureBefore && _editor.FullSurfaceRefreshCount == fullBefore && _history.HasUndo();
			bool structureUndone = _history.Undo();
			await WaitFrames(3);
			structureUndone &= presenter.CompletionRowCount == completionBefore && presenter.Root.GetInstanceId() == rootId;
			bool structureRedone = _history.Redo();
			await WaitFrames(3);
			structureRedone &= presenter.CompletionRowCount == completionBefore + 1;
			bool structureRefresh = structureApplied & structureUndone & structureRedone;
			Require(structureRefresh, "完成片段数组没有按结构 token 局部刷新。");
			bool runtimeSemantics = _editor.CurrentRuntimePreview is AttackComponent { checkEachShape: not false } attackComponent && !attackComponent.IsReleased;
			Require(runtimeSemantics, "checkEachShape 没有从 Definition 进入真实 Attack runtime。");
			bool saveReload = _editor.SaveActiveResource();
			await WaitFrames(6);
			bool oldPresenterReleased = presenter.Root == null;
			AttackComponentDefinition attackComponentDefinition = ResourceLoader.Load<AttackComponentDefinition>("user://ModEditorAttackContactPresenterProbe/AttackContactPresenter.tres", "", ResourceLoader.CacheMode.Ignore);
			saveReload &= GodotObject.IsInstanceValid(attackComponentDefinition) && attackComponentDefinition.checkEachShape && attackComponentDefinition.checkLine && attackComponentDefinition.checkGrid && attackComponentDefinition.useCheckAreaGridColumn && Mathf.IsEqualApprox((float)attackComponentDefinition.attackInterval, 3.25f) && attackComponentDefinition.attackAnimeClipsArray.Count == 2;
			Require(saveReload & oldPresenterReleased, "Attack 编辑未通过保存/忽略缓存重载，或旧 presenter 未释放。");
			PanelContainer panelContainer2 = _editor.FindChild("InspectorPanel", recursive: true, owned: false) as PanelContainer;
			bool inspectorUntouched = GodotObject.IsInstanceValid(panelContainer2) && !panelContainer2.Visible && _editor.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null && inspector?.CurrentObject == inspectorSentinel;
			Require(inspectorUntouched, "Attack presenter 暴露或替换了 Inspector。");
			presenter = _editor.AttackContactPresenter;
			XWEditorInterface.Instance.FocusPanel("bp_editor");
			await WaitFrames(4);
			bool flag4 = GodotObject.IsInstanceValid(presenter?.Root) && !_editor.IsVisibleInTree() && _editor.ProcessMode == ProcessModeEnum.Disabled && presenter.IsProcessIdle;
			Require(flag4, "隐藏后的 Attack presenter 仍在处理输入或帧。");
			GD.Print($"[MOD_EDITOR_ATTACK_CONTACT_PRESENTER_PROBE] f3={f3} route={route} gamePosition={gamePosition} targetCards={targetCards} timeline={timeline} collisionReuse={collisionReuse} remainingDynamic={remainingDynamic} noDuplicates={noDuplicates} inPlaceRefresh={inPlaceRefresh} undoRedo={undoRedo} atomicTarget={atomicTarget} structureRefresh={structureRefresh} runtimeSemantics={runtimeSemantics} saveReload={saveReload} oldPresenterReleased={oldPresenterReleased} inspectorUntouched={inspectorUntouched} hiddenStopped={flag4} previewNonBlocking={previewNonBlocking} failures={_failures.Count}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
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

	private async Task<bool> WaitForComponentEditor(AttackComponentDefinition definition, int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			_editor = XWEditorInterface.Instance?.GetResourceEditor("character_component_editor") as XWCharacterComponentVisualResourceEditor;
			if (GodotObject.IsInstanceValid(_editor) && _editor.SelectedDefinition == definition && GodotObject.IsInstanceValid(_editor.AttackContactPresenter?.Root))
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
		foreach (string specializedProperty in XWAttackContactPresenter.SpecializedProperties)
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
			GD.PrintErr("[MOD_EDITOR_ATTACK_CONTACT_PRESENTER_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_ATTACK_CONTACT_PRESENTER_FAILURE] " + failure);
		}
		string path = ProjectSettings.GlobalizePath("user://ModEditorAttackContactPresenterProbe");
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
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
