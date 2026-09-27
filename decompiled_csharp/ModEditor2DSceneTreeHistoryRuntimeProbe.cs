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

[ScriptPath("res://Tests/ModEditor2DSceneTreeHistoryRuntimeProbe.cs")]
public class ModEditor2DSceneTreeHistoryRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BuildSourceScene = "BuildSourceScene";

		public static readonly StringName AddVisual = "AddVisual";

		public static readonly StringName SelectForStructureEdit = "SelectForStructureEdit";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _savedProbePath = "_savedProbePath";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private string _savedProbePath = "";

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
			XW2DSceneEditor editor = await WaitForSceneEditor(900);
			bool window = FindAncestorWindow(editor) != null;
			Require(window, "2D scene editor is not mounted under the F3 ModEditor window.");
			if (!GodotObject.IsInstanceValid(editor))
			{
				Finish();
				return;
			}
			_savedProbePath = $"res://Tests/.mod_editor_2d_tree_history_{Guid.NewGuid():N}.tscn";
			Node2D node2D = BuildSourceScene();
			PackedScene packedScene = new PackedScene
			{
				ResourceName = "SceneTreeHistoryProbe"
			};
			Require(packedScene.Pack(node2D) == Error.Ok, "Could not pack the scene-tree history probe scene.");
			node2D.Free();
			Require(ResourceSaver.Save(packedScene, _savedProbePath, ResourceSaver.SaverFlags.None) == Error.Ok, "Could not save the temporary scene-tree history probe scene.");
			editor.LoadPackedSceneFromPath(_savedProbePath);
			await WaitFrames(10);
			Node editedRoot = editor.CurrentSceneInstance;
			XWSceneTreeDock dock = XWEditorInterface.Instance?.GetSceneTreeDock();
			XWSceneNodeTree tree = FindNodeOfType<XWSceneNodeTree>(dock);
			XWUndoRedoManager history = XWEditorInterface.Instance?.GetUndoRedoManager();
			XWInspector inspector = FindNodeOfType<XWInspector>(GetTree().Root);
			Require(GodotObject.IsInstanceValid(editedRoot), "The editable scene root is missing.");
			Require(GodotObject.IsInstanceValid(dock), "The real scene-tree dock is missing.");
			Require(GodotObject.IsInstanceValid(tree), "The real scene-node tree is missing.");
			Require(GodotObject.IsInstanceValid(history), "The Scene undo manager is missing.");
			if (!GodotObject.IsInstanceValid(editedRoot) || !GodotObject.IsInstanceValid(dock) || !GodotObject.IsInstanceValid(tree) || !GodotObject.IsInstanceValid(history))
			{
				Finish();
				return;
			}
			history.ClearHistory();
			Node nodeA = editedRoot.FindChild("A", recursive: false, owned: false);
			Node nodeB = editedRoot.FindChild("B", recursive: false, owned: false);
			Node nodeC = editedRoot.FindChild("C", recursive: false, owned: false);
			GodotObject inspectorBeforeHistory = inspector?.CurrentObject;
			SelectForStructureEdit(dock, nodeB);
			bool renameApplied = tree.RenameSelectedNode("Beta") && nodeB.Name == (StringName)"Beta" && history.GetCurrentActionName() == "重命名 2D 节点";
			bool renameUndoCalled = history.Undo();
			await WaitFrames(2);
			bool renameUndo = renameUndoCalled && nodeB.Name == (StringName)"B";
			bool renameRedoCalled = history.Redo();
			await WaitFrames(2);
			bool flag = renameRedoCalled && nodeB.Name == (StringName)"Beta";
			bool rename = renameApplied & renameUndo & flag;
			Require(rename, "Scene-history rename did not apply, undo, and redo the same node name.");
			SelectForStructureEdit(dock, nodeB);
			Node duplicate = dock.DuplicateSelectedNode();
			await WaitFrames(2);
			int duplicateIndex = duplicate?.GetIndex() ?? (-1);
			bool duplicateApplied = GodotObject.IsInstanceValid(duplicate) && duplicate.Name == (StringName)"Beta2" && duplicate.GetParent() == editedRoot && duplicateIndex == nodeB.GetIndex() + 1 && history.GetCurrentActionName() == "复制 2D 节点";
			bool duplicateUndoCalled = history.Undo();
			await WaitFrames(2);
			bool duplicateUndo = duplicateUndoCalled && GodotObject.IsInstanceValid(duplicate) && duplicate.GetParent() == null && dock.GetSelectedNode() == nodeB;
			bool duplicateRedoCalled = history.Redo();
			await WaitFrames(2);
			bool flag2 = duplicateRedoCalled && duplicate.GetParent() == editedRoot && duplicate.GetIndex() == duplicateIndex && dock.GetSelectedNode() == duplicate;
			bool duplicateHistory = duplicateApplied & duplicateUndo & flag2;
			Require(duplicateHistory, "Scene-history duplicate did not preserve identity, index, selection, and redo.");
			int originalBIndex = nodeB.GetIndex();
			SelectForStructureEdit(dock, nodeB);
			bool deleteApplied = dock.RemoveSelectedNode() && GodotObject.IsInstanceValid(nodeB) && nodeB.GetParent() == null && dock.GetSelectedNode() == editedRoot && history.GetCurrentActionName() == "删除 2D 节点";
			bool deleteUndoCalled = history.Undo();
			await WaitFrames(2);
			bool deleteUndo = deleteUndoCalled && nodeB.GetParent() == editedRoot && nodeB.GetIndex() == originalBIndex && dock.GetSelectedNode() == nodeB;
			bool deleteRedoCalled = history.Redo();
			await WaitFrames(2);
			bool deleteRedo = deleteRedoCalled && nodeB.GetParent() == null;
			bool deleteRestoreCalled = history.Undo();
			await WaitFrames(2);
			bool flag3 = deleteRestoreCalled && nodeB.GetParent() == editedRoot && nodeB.GetIndex() == originalBIndex;
			bool deleteHistory = deleteApplied & deleteUndo & deleteRedo & flag3;
			Require(deleteHistory, "Scene-history delete did not retain and restore the exact node and sibling index.");
			SelectForStructureEdit(dock, nodeB);
			int moveBefore = nodeB.GetIndex();
			bool moveApplied = dock.MoveSelectedNode(1) && nodeB.GetIndex() == moveBefore + 1 && history.GetCurrentActionName() == "调整 2D 节点顺序";
			bool redoBranchRelease = tree.GetTrackedStructuralNodeRecordCount() == 1 && GodotObject.IsInstanceValid(nodeB) && nodeB.GetParent() == editedRoot;
			bool moveUndoCalled = history.Undo();
			await WaitFrames(2);
			bool moveUndo = moveUndoCalled && nodeB.GetIndex() == moveBefore;
			bool moveRedoCalled = history.Redo();
			await WaitFrames(2);
			bool flag4 = moveRedoCalled && nodeB.GetIndex() == moveBefore + 1;
			bool move = moveApplied & moveUndo & flag4;
			Require(move, "Scene-history reorder did not restore and reapply the exact sibling order.");
			Require(redoBranchRelease, "Starting a reorder after undo did not release the discarded delete redo record.");
			bool treeSynced = dock.GetSelectedNode() == nodeB;
			bool canvasSelection = XWEditorInterface.Instance.GetEditorSelection().IsSelected(nodeB);
			bool inspectorUntouched = GodotObject.IsInstanceValid(inspector) && inspector.CurrentObject == inspectorBeforeHistory && inspector.CurrentObject != nodeB && inspector.CurrentObject != duplicate;
			Require(treeSynced, "Scene history did not keep the scene-tree highlight synchronized.");
			Require(canvasSelection, "Scene history did not keep the 2D canvas selection synchronized.");
			Require(inspectorUntouched, "Scene history redirected the raw Inspector during structure edits.");
			bool saved = editor.SaveCurrentScene();
			await WaitFrames(3);
			Node node = ResourceLoader.Load<PackedScene>(_savedProbePath, "", ResourceLoader.CacheMode.Ignore)?.Instantiate(PackedScene.GenEditState.Disabled);
			string[] array = new string[4] { "A", "Beta2", "Beta", "C" };
			bool reload = GodotObject.IsInstanceValid(node) && node.GetChildCount() == array.Length;
			if (reload)
			{
				for (int i = 0; i < array.Length; i++)
				{
					reload &= node.GetChild(i).Name == (StringName)array[i];
				}
			}
			node?.Free();
			Require(saved, "The scene-tree history result was not saved.");
			Require(reload, "The renamed, duplicated, restored, and reordered tree did not survive reload.");
			bool flag5 = tree.GetTrackedStructuralNodeRecordCount() == 1;
			history.ClearHistory(1);
			tree.PrepareForSceneHistoryBranchChange();
			bool clearHistoryRelease = flag5 && tree.GetTrackedStructuralNodeRecordCount() == 0 && GodotObject.IsInstanceValid(duplicate) && duplicate.GetParent() == editedRoot;
			Require(clearHistoryRelease, "Clearing Scene history did not release structure records while preserving applied duplicates.");
			SelectForStructureEdit(dock, nodeC);
			Node discardedDuplicate = dock.DuplicateSelectedNode();
			bool discardedUndoCalled = history.Undo();
			await WaitFrames(2);
			bool detachedBeforeDiscard = discardedUndoCalled && GodotObject.IsInstanceValid(discardedDuplicate) && discardedDuplicate.GetParent() == null;
			SelectForStructureEdit(dock, nodeA);
			bool replacementRename = tree.RenameSelectedNode("Alpha");
			await WaitFrames(2);
			bool flag6 = (detachedBeforeDiscard & replacementRename) && !GodotObject.IsInstanceValid(discardedDuplicate) && !history.HasRedo();
			Require(flag6, "A new structure action did not free the orphaned duplicate from the discarded redo branch.");
			GD.Print($"[MOD_EDITOR_2D_TREE_HISTORY_PROBE] window={window} rename={rename} duplicate={duplicateHistory} delete={deleteHistory} move={move} treeSynced={treeSynced} canvasSelection={canvasSelection} inspectorUntouched={inspectorUntouched} saved={saved} reload={reload} redoBranchRelease={redoBranchRelease} clearHistoryRelease={clearHistoryRelease} branchReferenceRelease={flag6} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
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
			Name = "TreeHistoryRoot"
		};
		AddVisual(node2D, "A", new Vector2(-72f, 0f), new Color(0.28f, 0.64f, 1f));
		AddVisual(node2D, "B", Vector2.Zero, new Color(0.35f, 0.85f, 0.42f));
		AddVisual(node2D, "C", new Vector2(72f, 0f), new Color(1f, 0.62f, 0.2f));
		return node2D;
	}

	private static void AddVisual(Node root, string name, Vector2 position, Color color)
	{
		ColorRect colorRect = new ColorRect
		{
			Name = name,
			Position = position - new Vector2(20f, 20f),
			Size = new Vector2(40f, 40f),
			Color = color,
			MouseFilter = Control.MouseFilterEnum.Ignore
		};
		root.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
		colorRect.Owner = root;
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
			GD.PrintErr("[MOD_EDITOR_2D_TREE_HISTORY_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		if (!string.IsNullOrWhiteSpace(_savedProbePath))
		{
			string path = ProjectSettings.GlobalizePath(_savedProbePath);
			if (FileAccess.FileExists(_savedProbePath))
			{
				DirAccess.RemoveAbsolute(path);
			}
		}
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_2D_TREE_HISTORY_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildSourceScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.AddVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BuildSourceScene && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(BuildSourceScene());
			return true;
		}
		if (method == MethodName.AddVisual && args.Count == 4)
		{
			AddVisual(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]));
			ret = default;
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
		if (method == MethodName.AddVisual && args.Count == 4)
		{
			AddVisual(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]));
			ret = default;
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
		if (method == MethodName.AddVisual)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._savedProbePath)
		{
			_savedProbePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._savedProbePath)
		{
			value = VariantUtils.CreateFrom(in _savedProbePath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._savedProbePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._savedProbePath, Variant.From(in _savedProbePath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._savedProbePath, out var value))
		{
			_savedProbePath = value.As<string>();
		}
	}
}
