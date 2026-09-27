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
using PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://Tests/ModEditor2DCreateHistoryRuntimeProbe.cs")]
public class ModEditor2DCreateHistoryRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BuildSourceScene = "BuildSourceScene";

		public static readonly StringName BuildInstanceScene = "BuildInstanceScene";

		public static readonly StringName SavePackedScene = "SavePackedScene";

		public static readonly StringName ReloadScene = "ReloadScene";

		public static readonly StringName SelectForStructureEdit = "SelectForStructureEdit";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";

		public static readonly StringName RemoveProbeFile = "RemoveProbeFile";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _scenePath = "_scenePath";

		public static readonly StringName _instancePath = "_instancePath";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private string _scenePath = "";

	private string _instancePath = "";

	public override async void _Ready()
	{
		_ = 23;
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
			XW2DSceneEditor editor = await WaitForSceneEditor(900);
			bool window = FindAncestorWindow(editor) != null;
			Require(window, "2D scene editor is not mounted under the F3 ModEditor window.");
			if (!GodotObject.IsInstanceValid(editor))
			{
				Finish();
				return;
			}
			string text = Guid.NewGuid().ToString("N");
			_scenePath = "res://Tests/.mod_editor_2d_create_history_" + text + ".tscn";
			_instancePath = "res://Tests/.mod_editor_2d_create_instance_" + text + ".tscn";
			Require(SavePackedScene(BuildSourceScene(), _scenePath), "Could not save the create-history source scene.");
			Require(SavePackedScene(BuildInstanceScene(), _instancePath), "Could not save the create-history instance fixture.");
			editor.LoadPackedSceneFromPath(_scenePath);
			await WaitFrames(10);
			Node oldRoot = editor.CurrentSceneInstance;
			XWSceneTreeDock dock = XWEditorInterface.Instance?.GetSceneTreeDock();
			XWSceneNodeTree tree = FindNodeOfType<XWSceneNodeTree>(dock);
			XWUndoRedoManager history = XWEditorInterface.Instance?.GetUndoRedoManager();
			XWInspector inspector = FindNodeOfType<XWInspector>(GetTree().Root);
			Require(GodotObject.IsInstanceValid(oldRoot), "The editable source root is missing.");
			Require(GodotObject.IsInstanceValid(dock), "The real scene-tree dock is missing.");
			Require(GodotObject.IsInstanceValid(tree), "The real scene-node tree is missing.");
			Require(GodotObject.IsInstanceValid(history), "The Scene undo manager is missing.");
			if (!GodotObject.IsInstanceValid(oldRoot) || !GodotObject.IsInstanceValid(dock) || !GodotObject.IsInstanceValid(tree) || !GodotObject.IsInstanceValid(history))
			{
				Finish();
				return;
			}
			history.ClearHistory();
			tree.PrepareForSceneHistoryBranchChange();
			GodotObject inspectorBeforeHistory = inspector?.CurrentObject;
			SelectForStructureEdit(dock, oldRoot);
			Node added = dock.AddNodeByClassWithHistory("Node2D", new Vector2(96f, 48f));
			await WaitFrames(2);
			bool addApplied = added is Node2D node2D && added.GetParent() == oldRoot && added.Owner == oldRoot && node2D.GlobalPosition.IsEqualApprox(new Vector2(96f, 48f)) && history.GetCurrentActionName() == "添加 2D 节点";
			bool addUndoCalled = history.Undo();
			await WaitFrames(2);
			bool addUndo = addUndoCalled && GodotObject.IsInstanceValid(added) && added.GetParent() == null && dock.GetSelectedNode() == oldRoot;
			bool addRedoCalled = history.Redo();
			await WaitFrames(2);
			bool flag = addRedoCalled && added.GetParent() == oldRoot && dock.GetSelectedNode() == added && ((Node2D)added).GlobalPosition.IsEqualApprox(new Vector2(96f, 48f));
			bool addNode = addApplied & addUndo & flag;
			Require(addNode, "Add Node did not preserve identity, owner, position, selection, and redo.");
			SelectForStructureEdit(dock, oldRoot);
			Node instantiated = dock.InstantiateSceneWithHistory(_instancePath);
			await WaitFrames(2);
			string instantiatedName = instantiated?.Name.ToString() ?? "";
			Node instantiatedLeaf = instantiated?.FindChild("InstanceLeaf", recursive: false, owned: false);
			bool instantiateApplied = GodotObject.IsInstanceValid(instantiated) && instantiated.GetParent() == oldRoot && instantiated.Owner == oldRoot && !string.IsNullOrEmpty(instantiated.SceneFilePath) && instantiatedLeaf?.Owner == instantiated && history.GetCurrentActionName() == "实例化 2D 场景";
			bool instantiateUndoCalled = history.Undo();
			await WaitFrames(2);
			bool instantiateUndo = instantiateUndoCalled && GodotObject.IsInstanceValid(instantiated) && instantiated.GetParent() == null;
			bool instantiateRedoCalled = history.Redo();
			await WaitFrames(2);
			bool flag2 = instantiateRedoCalled && instantiated.GetParent() == oldRoot && instantiatedLeaf?.Owner == instantiated && dock.GetSelectedNode() == instantiated;
			bool instantiate = instantiateApplied & instantiateUndo & flag2;
			Require(instantiate, "Instantiate Scene did not preserve identity, owners, selection, and redo.");
			Node componentManager = oldRoot.FindChild("ComponentManager", recursive: false, owned: false);
			SelectForStructureEdit(dock, componentManager);
			Node component = dock.AddComponentSceneWithHistory("res://Script/Component/TimerComponent/TimerComponent.tscn");
			await WaitFrames(3);
			bool componentApplied = component is ComponentBase && component.GetParent() == componentManager && component.Owner == oldRoot && history.GetCurrentActionName() == "添加 2D Component";
			bool componentUndoCalled = history.Undo();
			await WaitFrames(2);
			bool componentUndo = componentUndoCalled && GodotObject.IsInstanceValid(component) && component.GetParent() == null;
			bool componentRedoCalled = history.Redo();
			await WaitFrames(3);
			bool flag3 = componentRedoCalled && component.GetParent() == componentManager && dock.GetSelectedNode() == component;
			bool componentHistory = componentApplied & componentUndo & flag3;
			Require(componentHistory, "Add Component did not preserve identity, owner, selection, and redo.");
			bool directSelection = XWEditorInterface.Instance.GetEditorSelection().IsSelected(component);
			bool inspectorUntouched = GodotObject.IsInstanceValid(inspector) && inspector.CurrentObject == inspectorBeforeHistory && inspector.CurrentObject != added && inspector.CurrentObject != instantiated && inspector.CurrentObject != component;
			Require(directSelection, "Create history did not synchronize direct canvas selection.");
			Require(inspectorUntouched, "Create history redirected the raw Inspector.");
			bool createSaved = editor.SaveCurrentScene();
			await WaitFrames(3);
			Node node = ReloadScene(_scenePath);
			bool createReloaded = GodotObject.IsInstanceValid(node) && GodotObject.IsInstanceValid(node.FindChild(added.Name.ToString(), recursive: false, owned: false)) && GodotObject.IsInstanceValid(node.FindChild(instantiatedName, recursive: false, owned: false)) && GodotObject.IsInstanceValid(node.FindChild("TimerComponent", recursive: true, owned: false));
			node?.Free();
			Require(createSaved & createReloaded, "Created nodes, instance, and Component did not survive save/reload.");
			Node branch = oldRoot.FindChild("Branch", recursive: false, owned: false);
			Node oldParent = branch?.GetParent();
			int oldIndex = branch?.GetIndex() ?? (-1);
			SelectForStructureEdit(dock, branch);
			bool makeRootCalled = dock.MakeSelectedNodeRoot();
			await WaitFrames(3);
			bool makeRootApplied = makeRootCalled && editor.CurrentSceneInstance == branch && XWEditorInterface.Instance.GetEditedSceneRoot() == branch && tree.GetEditedSceneRoot() == branch && dock.GetSelectedNode() == branch && branch.Owner == null && oldRoot.GetParent() == null && history.GetCurrentActionName() == "设置 2D 场景根节点";
			bool rootSaved = editor.SaveCurrentScene();
			await WaitFrames(3);
			Node node2 = ReloadScene(_scenePath);
			bool rootReloaded = GodotObject.IsInstanceValid(node2) && node2.Name == (StringName)"Branch" && GodotObject.IsInstanceValid(node2.FindChild("BranchLeaf", recursive: false, owned: false));
			node2?.Free();
			bool makeRootUndoCalled = history.Undo();
			await WaitFrames(3);
			bool makeRootUndo = makeRootUndoCalled && editor.CurrentSceneInstance == oldRoot && tree.GetEditedSceneRoot() == oldRoot && branch.GetParent() == oldParent && branch.GetIndex() == oldIndex && branch.Owner == oldRoot && dock.GetSelectedNode() == oldRoot;
			bool makeRootRedoCalled = history.Redo();
			await WaitFrames(3);
			bool makeRootRedo = makeRootRedoCalled && editor.CurrentSceneInstance == branch && tree.GetEditedSceneRoot() == branch && dock.GetSelectedNode() == branch;
			bool makeRootRestoreCalled = history.Undo();
			await WaitFrames(3);
			bool flag4 = makeRootRestoreCalled && editor.CurrentSceneInstance == oldRoot && branch.GetParent() == oldParent && branch.GetIndex() == oldIndex;
			bool makeRoot = makeRootApplied & rootSaved & rootReloaded & makeRootUndo & makeRootRedo & flag4;
			Require(makeRoot, "Make Root did not synchronize tree/canvas/tab state, save the active root, and undo/redo.");
			bool restoredSaved = editor.SaveCurrentScene();
			await WaitFrames(3);
			Node node3 = ReloadScene(_scenePath);
			bool flag5 = GodotObject.IsInstanceValid(node3) && node3.Name == (StringName)"CreateHistoryRoot" && GodotObject.IsInstanceValid(node3.FindChild("Branch", recursive: false, owned: false));
			node3?.Free();
			Require(restoredSaved & flag5, "Undoing Make Root did not restore a saveable original root.");
			bool rootRecordBeforeBranch = tree.GetTrackedSceneRootRecordCount() == 1;
			SelectForStructureEdit(dock, branch);
			bool replacementRename = tree.RenameSelectedNode("BranchRenamed");
			await WaitFrames(2);
			bool rootRedoBranchRelease = (rootRecordBeforeBranch & replacementRename) && tree.GetTrackedSceneRootRecordCount() == 0 && !history.HasRedo() && GodotObject.IsInstanceValid(oldRoot) && branch.GetParent() == oldParent;
			Require(rootRedoBranchRelease, "A new action did not release the discarded Make Root redo record safely.");
			SelectForStructureEdit(dock, branch);
			Node rootDiscardCandidate = oldRoot;
			bool secondMakeRoot = dock.MakeSelectedNodeRoot();
			await WaitFrames(3);
			bool rootRecordBeforeClear = secondMakeRoot && tree.GetTrackedSceneRootRecordCount() == 1 && editor.CurrentSceneInstance == branch;
			history.ClearHistory(1);
			tree.PrepareForSceneHistoryBranchChange();
			await WaitFrames(2);
			bool rootClearRelease = rootRecordBeforeClear && tree.GetTrackedSceneRootRecordCount() == 0 && !GodotObject.IsInstanceValid(rootDiscardCandidate) && GodotObject.IsInstanceValid(branch) && editor.CurrentSceneInstance == branch;
			Require(rootClearRelease, "Clearing Scene history did not release the discarded old root while preserving the active root.");
			SelectForStructureEdit(dock, branch);
			Node discardedAdd = dock.AddNodeByClassWithHistory("Node2D");
			bool discardedAddUndo = history.Undo();
			await WaitFrames(2);
			Node node4 = branch.FindChild("BranchLeaf", recursive: false, owned: false);
			SelectForStructureEdit(dock, node4);
			bool branchRename = tree.RenameSelectedNode("BranchLeafRenamed");
			await WaitFrames(2);
			bool flag6 = (discardedAddUndo & branchRename) && !GodotObject.IsInstanceValid(discardedAdd) && !history.HasRedo();
			Require(flag6, "A new Scene action did not free an orphaned Add Node from the discarded redo branch.");
			GD.Print($"[MOD_EDITOR_2D_CREATE_HISTORY_PROBE] window={window} addNode={addNode} instantiate={instantiate} component={componentHistory} directSelection={directSelection} inspectorUntouched={inspectorUntouched} createSaved={createSaved} createReloaded={createReloaded} makeRoot={makeRoot} rootSaved={rootSaved} rootReloaded={rootReloaded} rootRedoBranchRelease={rootRedoBranchRelease} rootClearRelease={rootClearRelease} addRedoBranchRelease={flag6} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private static Node2D BuildSourceScene()
	{
		Node2D node2D = new Node2D
		{
			Name = "CreateHistoryRoot"
		};
		Node2D node2D2 = new Node2D
		{
			Name = "ComponentManager"
		};
		Node2D node2D3 = new Node2D
		{
			Name = "Branch",
			Position = new Vector2(24f, 12f)
		};
		ColorRect colorRect = new ColorRect
		{
			Name = "BranchLeaf",
			Position = new Vector2(-16f, -16f),
			Size = new Vector2(32f, 32f),
			Color = new Color(0.26f, 0.72f, 1f),
			MouseFilter = Control.MouseFilterEnum.Ignore
		};
		node2D.AddChild(node2D2, forceReadableName: false, InternalMode.Disabled);
		node2D.AddChild(node2D3, forceReadableName: false, InternalMode.Disabled);
		node2D3.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
		node2D2.Owner = node2D;
		node2D3.Owner = node2D;
		colorRect.Owner = node2D;
		return node2D;
	}

	private static Node2D BuildInstanceScene()
	{
		Node2D node2D = new Node2D
		{
			Name = "InstanceFixture"
		};
		ColorRect colorRect = new ColorRect
		{
			Name = "InstanceLeaf",
			Size = new Vector2(20f, 20f),
			Color = new Color(1f, 0.58f, 0.24f),
			MouseFilter = Control.MouseFilterEnum.Ignore
		};
		node2D.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
		colorRect.Owner = node2D;
		return node2D;
	}

	private static bool SavePackedScene(Node root, string path)
	{
		PackedScene packedScene = new PackedScene();
		Error error = packedScene.Pack(root);
		root.Free();
		if (error == Error.Ok)
		{
			return ResourceSaver.Save(packedScene, path, ResourceSaver.SaverFlags.None) == Error.Ok;
		}
		return false;
	}

	private static Node ReloadScene(string path)
	{
		return ResourceLoader.Load<PackedScene>(path, "", ResourceLoader.CacheMode.Ignore)?.Instantiate(PackedScene.GenEditState.Disabled);
	}

	private static void SelectForStructureEdit(XWSceneTreeDock dock, Node node)
	{
		dock.SelectNode(node, emitSignal: false);
		XWEditorInterface.Instance.SetSelectedNode(node);
	}

	private async Task<XW2DSceneEditor> WaitForSceneEditor(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			XW2DSceneEditor xW2DSceneEditor = XWEditorInterface.Instance?.Get2DSceneEditor();
			if (GodotObject.IsInstanceValid(xW2DSceneEditor) && xW2DSceneEditor.IsInsideTree())
			{
				return xW2DSceneEditor;
			}
			await WaitFrames(1);
		}
		Require(condition: false, $"F3 did not initialize the 2D scene editor within {maxFrames} frames.");
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

	private static T FindNodeOfType<T>(Node root) where T : Node
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		if (root is T result)
		{
			return result;
		}
		foreach (Node child in root.GetChildren())
		{
			T val = FindNodeOfType<T>(child);
			if (GodotObject.IsInstanceValid(val))
			{
				return val;
			}
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
			GD.PrintErr("[MOD_EDITOR_2D_CREATE_HISTORY_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		RemoveProbeFile(_scenePath);
		RemoveProbeFile(_instancePath);
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_2D_CREATE_HISTORY_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	private static void RemoveProbeFile(string path)
	{
		if (!string.IsNullOrWhiteSpace(path) && FileAccess.FileExists(path))
		{
			DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(path));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildSourceScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.BuildInstanceScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.SavePackedScene, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReloadScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectForStructureEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveProbeFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName.BuildSourceScene && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(BuildSourceScene());
			return true;
		}
		if (method == MethodName.BuildInstanceScene && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(BuildInstanceScene());
			return true;
		}
		if (method == MethodName.SavePackedScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SavePackedScene(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReloadScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(ReloadScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectForStructureEdit && args.Count == 2)
		{
			SelectForStructureEdit(VariantUtils.ConvertTo<XWSceneTreeDock>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
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
		if (method == MethodName.RemoveProbeFile && args.Count == 1)
		{
			RemoveProbeFile(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BuildSourceScene && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(BuildSourceScene());
			return true;
		}
		if (method == MethodName.BuildInstanceScene && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(BuildInstanceScene());
			return true;
		}
		if (method == MethodName.SavePackedScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SavePackedScene(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReloadScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(ReloadScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectForStructureEdit && args.Count == 2)
		{
			SelectForStructureEdit(VariantUtils.ConvertTo<XWSceneTreeDock>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveProbeFile && args.Count == 1)
		{
			RemoveProbeFile(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.BuildSourceScene)
		{
			return true;
		}
		if (method == MethodName.BuildInstanceScene)
		{
			return true;
		}
		if (method == MethodName.SavePackedScene)
		{
			return true;
		}
		if (method == MethodName.ReloadScene)
		{
			return true;
		}
		if (method == MethodName.SelectForStructureEdit)
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
		if (method == MethodName.RemoveProbeFile)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._scenePath)
		{
			_scenePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._instancePath)
		{
			_instancePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._scenePath)
		{
			value = VariantUtils.CreateFrom(in _scenePath);
			return true;
		}
		if (name == PropertyName._instancePath)
		{
			value = VariantUtils.CreateFrom(in _instancePath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._scenePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._instancePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._scenePath, Variant.From(in _scenePath));
		info.AddProperty(PropertyName._instancePath, Variant.From(in _instancePath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._scenePath, out var value))
		{
			_scenePath = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._instancePath, out var value2))
		{
			_instancePath = value2.As<string>();
		}
	}
}
