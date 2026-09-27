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

[ScriptPath("res://Tests/ModEditor2DPointHistoryRuntimeProbe.cs")]
public class ModEditor2DPointHistoryRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BuildSourceScene = "BuildSourceScene";

		public static readonly StringName ViewAt = "ViewAt";

		public static readonly StringName SelectNodeForCanvas = "SelectNodeForCanvas";

		public static readonly StringName ClickLeft = "ClickLeft";

		public static readonly StringName PressLeft = "PressLeft";

		public static readonly StringName MoveLeft = "MoveLeft";

		public static readonly StringName DragLeft = "DragLeft";

		public static readonly StringName ClickRight = "ClickRight";

		public static readonly StringName EmitCanvasInput = "EmitCanvasInput";

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
		_ = 25;
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
			_savedProbePath = $"res://Tests/.mod_editor_2d_point_history_{Guid.NewGuid():N}.tscn";
			Node2D node2D = BuildSourceScene();
			PackedScene packedScene = new PackedScene
			{
				ResourceName = "PointHistoryProbe"
			};
			Require(packedScene.Pack(node2D) == Error.Ok, "Could not pack the point-history probe scene.");
			node2D.Free();
			Require(ResourceSaver.Save(packedScene, _savedProbePath, ResourceSaver.SaverFlags.None) == Error.Ok, "Could not save the temporary point-history probe scene.");
			editor.LoadPackedSceneFromPath(_savedProbePath);
			await WaitFrames(10);
			XW2DViewport viewport = editor.FindChild("Viewport2D", recursive: true, owned: false) as XW2DViewport;
			XWSceneTreeDock dock = XWEditorInterface.Instance?.GetSceneTreeDock();
			XWUndoRedoManager history = XWEditorInterface.Instance?.GetUndoRedoManager();
			XWInspector inspector = FindNodeOfType<XWInspector>(GetTree().Root);
			Node editedRoot = editor.CurrentSceneInstance;
			Path2D path = editedRoot?.FindChild("Path", recursive: false, owned: false) as Path2D;
			Polygon2D polygon = editedRoot?.FindChild("Polygon", recursive: false, owned: false) as Polygon2D;
			Require(GodotObject.IsInstanceValid(viewport), "The real 2D viewport is missing.");
			Require(GodotObject.IsInstanceValid(dock), "The real scene-tree dock is missing.");
			Require(GodotObject.IsInstanceValid(history), "The Scene undo manager is missing.");
			Require(GodotObject.IsInstanceValid(path), "The editable Path2D is missing.");
			Require(GodotObject.IsInstanceValid(polygon), "The editable Polygon2D is missing.");
			if (!GodotObject.IsInstanceValid(viewport) || !GodotObject.IsInstanceValid(dock) || !GodotObject.IsInstanceValid(history) || !GodotObject.IsInstanceValid(path) || !GodotObject.IsInstanceValid(polygon))
			{
				Finish();
				return;
			}
			viewport.SetZoom(1f);
			viewport.CenterAt(Vector2.Zero);
			history.ClearHistory();
			GodotObject inspectorBeforePointEdits = inspector?.CurrentObject;
			SelectNodeForCanvas(dock, path);
			viewport.SetToolMode(XW2DViewport.ToolMode.Path);
			Vector2 pathAdded = new Vector2(80f, -48f);
			ClickLeft(viewport, ViewAt(viewport, pathAdded));
			await WaitFrames(3);
			bool pathAddApplied = path.Curve.PointCount == 3 && path.Curve.GetPointPosition(2).IsEqualApprox(pathAdded) && history.GetCurrentActionName() == "添加 2D 控制点";
			bool pathAddUndoCalled = history.Undo();
			await WaitFrames(2);
			bool pathAddUndo = pathAddUndoCalled && path.Curve.PointCount == 2;
			bool pathAddRedoCalled = history.Redo();
			await WaitFrames(2);
			bool flag = pathAddRedoCalled && path.Curve.PointCount == 3 && path.Curve.GetPointPosition(2).IsEqualApprox(pathAdded) && viewport.GetSelectedEditablePointIndex() == 2;
			bool pathAdd = pathAddApplied & pathAddUndo & flag;
			Require(pathAdd, "Path2D canvas add did not apply, undo, and redo one point.");
			Vector2 pathMoved = new Vector2(104f, -8f);
			DragLeft(viewport, ViewAt(viewport, pathAdded), ViewAt(viewport, pathMoved));
			await WaitFrames(3);
			bool pathMoveApplied = path.Curve.GetPointPosition(2).IsEqualApprox(pathMoved) && history.GetCurrentActionName() == "移动 2D 控制点";
			bool pathMoveUndoCalled = history.Undo();
			await WaitFrames(2);
			bool pathMoveUndo = pathMoveUndoCalled && path.Curve.GetPointPosition(2).IsEqualApprox(pathAdded);
			bool pathMoveRedoCalled = history.Redo();
			await WaitFrames(2);
			bool flag2 = pathMoveRedoCalled && path.Curve.GetPointPosition(2).IsEqualApprox(pathMoved);
			bool pathMove = pathMoveApplied & pathMoveUndo & flag2;
			Require(pathMove, "Path2D canvas point drag did not apply, undo, and redo one move.");
			ClickRight(viewport, ViewAt(viewport, pathMoved));
			await WaitFrames(3);
			bool pathDeleteApplied = path.Curve.PointCount == 2 && history.GetCurrentActionName() == "删除 2D 控制点";
			bool pathDeleteUndoCalled = history.Undo();
			await WaitFrames(2);
			bool pathDeleteUndo = pathDeleteUndoCalled && path.Curve.PointCount == 3 && path.Curve.GetPointPosition(2).IsEqualApprox(pathMoved) && viewport.GetSelectedEditablePointIndex() == 2;
			bool pathDeleteRedoCalled = history.Redo();
			await WaitFrames(2);
			bool pathDeleteRedo = pathDeleteRedoCalled && path.Curve.PointCount == 2;
			bool pathDeleteRestoreCalled = history.Undo();
			await WaitFrames(2);
			bool flag3 = pathDeleteRestoreCalled && path.Curve.PointCount == 3;
			bool pathDelete = pathDeleteApplied & pathDeleteUndo & pathDeleteRedo & flag3;
			Require(pathDelete, "Path2D right-click delete did not preserve point state through undo/redo.");
			Vector2 cancelBefore = path.Curve.GetPointPosition(0);
			Vector2 world = cancelBefore + new Vector2(30f, 22f);
			PressLeft(viewport, ViewAt(viewport, cancelBefore));
			MoveLeft(viewport, ViewAt(viewport, cancelBefore), ViewAt(viewport, world));
			EmitCanvasInput(viewport, new InputEventKey
			{
				Keycode = Key.Escape,
				Pressed = true
			});
			await WaitFrames(2);
			bool cancelRestore = path.Curve.GetPointPosition(0).IsEqualApprox(cancelBefore) && history.HasRedo();
			Require(cancelRestore, "Escaping an active point drag did not restore the pre-drag state without history.");
			SelectNodeForCanvas(dock, polygon);
			viewport.SetToolMode(XW2DViewport.ToolMode.Polygon);
			Vector2 polygonAdded = new Vector2(0f, 128f);
			ClickLeft(viewport, ViewAt(viewport, polygonAdded));
			await WaitFrames(3);
			bool polygonAddApplied = polygon.Polygon.Length == 4 && polygon.Polygon[3].IsEqualApprox(polygonAdded) && history.GetCurrentActionName() == "添加 2D 控制点";
			bool polygonAddUndoCalled = history.Undo();
			await WaitFrames(2);
			bool polygonAddUndo = polygonAddUndoCalled && polygon.Polygon.Length == 3;
			bool polygonAddRedoCalled = history.Redo();
			await WaitFrames(2);
			bool flag4 = polygonAddRedoCalled && polygon.Polygon.Length == 4 && polygon.Polygon[3].IsEqualApprox(polygonAdded);
			bool polygonAdd = polygonAddApplied & polygonAddUndo & flag4;
			Require(polygonAdd, "Polygon2D canvas add did not apply, undo, and redo one point.");
			Vector2 polygonMoved = new Vector2(24f, 112f);
			DragLeft(viewport, ViewAt(viewport, polygonAdded), ViewAt(viewport, polygonMoved));
			await WaitFrames(3);
			bool polygonMoveApplied = polygon.Polygon[3].IsEqualApprox(polygonMoved) && history.GetCurrentActionName() == "移动 2D 控制点";
			bool polygonMoveUndoCalled = history.Undo();
			await WaitFrames(2);
			bool polygonMoveUndo = polygonMoveUndoCalled && polygon.Polygon[3].IsEqualApprox(polygonAdded);
			bool polygonMoveRedoCalled = history.Redo();
			await WaitFrames(2);
			bool flag5 = polygonMoveRedoCalled && polygon.Polygon[3].IsEqualApprox(polygonMoved);
			bool polygonMove = polygonMoveApplied & polygonMoveUndo & flag5;
			Require(polygonMove, "Polygon2D canvas point drag did not apply, undo, and redo one move.");
			EmitCanvasInput(viewport, new InputEventKey
			{
				Keycode = Key.Delete,
				Pressed = true
			});
			await WaitFrames(3);
			bool polygonDeleteApplied = polygon.Polygon.Length == 3 && history.GetCurrentActionName() == "删除 2D 控制点";
			bool polygonDeleteUndoCalled = history.Undo();
			await WaitFrames(2);
			bool polygonDeleteUndo = polygonDeleteUndoCalled && polygon.Polygon.Length == 4 && polygon.Polygon[3].IsEqualApprox(polygonMoved) && viewport.GetSelectedEditablePointIndex() == 3;
			bool polygonDeleteRedoCalled = history.Redo();
			await WaitFrames(2);
			bool polygonDeleteRedo = polygonDeleteRedoCalled && polygon.Polygon.Length == 3;
			bool polygonDeleteRestoreCalled = history.Undo();
			await WaitFrames(2);
			bool flag6 = polygonDeleteRestoreCalled && polygon.Polygon.Length == 4;
			bool polygonDelete = polygonDeleteApplied & polygonDeleteUndo & polygonDeleteRedo & flag6;
			Require(polygonDelete, "Polygon2D keyboard delete did not preserve point state through undo/redo.");
			bool treeSynced = dock.GetSelectedNode() == polygon;
			bool canvasSelection = XWEditorInterface.Instance.GetEditorSelection().IsSelected(polygon);
			bool inspectorUntouched = GodotObject.IsInstanceValid(inspector) && inspector.CurrentObject == inspectorBeforePointEdits && inspector.CurrentObject != path && inspector.CurrentObject != polygon;
			Require(treeSynced, "Point history did not keep the scene-tree highlight synchronized.");
			Require(canvasSelection, "Point history did not keep the 2D canvas node selection synchronized.");
			Require(inspectorUntouched, "Point history redirected the raw Inspector.");
			bool saved = editor.SaveCurrentScene();
			await WaitFrames(3);
			Node node = ResourceLoader.Load<PackedScene>(_savedProbePath, "", ResourceLoader.CacheMode.Ignore)?.Instantiate(PackedScene.GenEditState.Disabled);
			Path2D path2D = node?.FindChild("Path", recursive: false, owned: false) as Path2D;
			Polygon2D polygon2D = node?.FindChild("Polygon", recursive: false, owned: false) as Polygon2D;
			int num;
			if (GodotObject.IsInstanceValid(path2D))
			{
				Curve2D curve = path2D.Curve;
				if (curve != null && curve.PointCount == 3 && path2D.Curve.GetPointPosition(2).IsEqualApprox(pathMoved) && GodotObject.IsInstanceValid(polygon2D) && polygon2D.Polygon.Length == 4)
				{
					num = (polygon2D.Polygon[3].IsEqualApprox(polygonMoved) ? 1 : 0);
					goto IL_1714;
				}
			}
			num = 0;
			goto IL_1714;
			IL_1714:
			bool flag7 = (byte)num != 0;
			node?.Free();
			Require(saved, "The 2D point-history result was not saved.");
			Require(flag7, "Path2D/Polygon2D point history did not survive cache-ignored reload.");
			history.ClearHistory(1);
			bool flag8 = !history.HasUndo() && !history.HasRedo();
			Require(flag8, "Scene history did not release point-state actions after ClearHistory.");
			GD.Print($"[MOD_EDITOR_2D_POINT_HISTORY_PROBE] window={window} pathAdd={pathAdd} pathMove={pathMove} pathDelete={pathDelete} polygonAdd={polygonAdd} polygonMove={polygonMove} polygonDelete={polygonDelete} cancelRestore={cancelRestore} treeSynced={treeSynced} canvasSelection={canvasSelection} inspectorUntouched={inspectorUntouched} saved={saved} reload={flag7} historyRelease={flag8} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
			viewport.ClearSceneInstance();
			editedRoot.Free();
			await WaitFrames(2);
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
			Name = "PointHistoryRoot"
		};
		Path2D path2D = new Path2D
		{
			Name = "Path",
			Curve = new Curve2D()
		};
		path2D.Curve.AddPoint(new Vector2(-80f, -48f));
		path2D.Curve.AddPoint(new Vector2(0f, -48f));
		node2D.AddChild(path2D, forceReadableName: false, InternalMode.Disabled);
		path2D.Owner = node2D;
		Polygon2D polygon2D = new Polygon2D();
		polygon2D.Name = "Polygon";
		polygon2D.Polygon = new Vector2[3]
		{
			new Vector2(-64f, 40f),
			new Vector2(0f, 88f),
			new Vector2(64f, 40f)
		};
		polygon2D.Color = new Color(0.22f, 0.72f, 0.42f, 0.72f);
		Polygon2D polygon2D2 = polygon2D;
		node2D.AddChild(polygon2D2, forceReadableName: false, InternalMode.Disabled);
		polygon2D2.Owner = node2D;
		return node2D;
	}

	private static Vector2 ViewAt(XW2DViewport viewport, Vector2 world)
	{
		return viewport.Size * 0.5f + world;
	}

	private static void SelectNodeForCanvas(XWSceneTreeDock dock, Node node)
	{
		dock.SelectNode(node, emitSignal: false);
		XWEditorInterface.Instance.SetSelectedNode(node);
	}

	private static void ClickLeft(XW2DViewport viewport, Vector2 position)
	{
		PressLeft(viewport, position);
		EmitCanvasInput(viewport, new InputEventMouseButton
		{
			Position = position,
			ButtonIndex = MouseButton.Left,
			ButtonMask = (MouseButtonMask)0L,
			Pressed = false
		});
	}

	private static void PressLeft(XW2DViewport viewport, Vector2 position)
	{
		EmitCanvasInput(viewport, new InputEventMouseButton
		{
			Position = position,
			ButtonIndex = MouseButton.Left,
			ButtonMask = MouseButtonMask.Left,
			Pressed = true
		});
	}

	private static void MoveLeft(XW2DViewport viewport, Vector2 from, Vector2 to)
	{
		EmitCanvasInput(viewport, new InputEventMouseMotion
		{
			Position = to,
			Relative = to - from,
			ButtonMask = MouseButtonMask.Left
		});
	}

	private static void DragLeft(XW2DViewport viewport, Vector2 from, Vector2 to)
	{
		PressLeft(viewport, from);
		MoveLeft(viewport, from, to);
		EmitCanvasInput(viewport, new InputEventMouseButton
		{
			Position = to,
			ButtonIndex = MouseButton.Left,
			ButtonMask = (MouseButtonMask)0L,
			Pressed = false
		});
	}

	private static void ClickRight(XW2DViewport viewport, Vector2 position)
	{
		EmitCanvasInput(viewport, new InputEventMouseButton
		{
			Position = position,
			ButtonIndex = MouseButton.Right,
			ButtonMask = MouseButtonMask.Right,
			Pressed = true
		});
	}

	private static void EmitCanvasInput(XW2DViewport viewport, InputEvent inputEvent)
	{
		viewport.EmitSignal(Control.SignalName.GuiInput, inputEvent);
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
			GD.PrintErr("[MOD_EDITOR_2D_POINT_HISTORY_PROBE_FAILURE] " + message);
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
			GD.PrintErr("[MOD_EDITOR_2D_POINT_HISTORY_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildSourceScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ViewAt, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "viewport", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "world", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectNodeForCanvas, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClickLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "viewport", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PressLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "viewport", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "viewport", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DragLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "viewport", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClickRight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "viewport", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitCanvasInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "viewport", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
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
		if (method == MethodName.ViewAt && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ViewAt(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.SelectNodeForCanvas && args.Count == 2)
		{
			SelectNodeForCanvas(VariantUtils.ConvertTo<XWSceneTreeDock>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClickLeft && args.Count == 2)
		{
			ClickLeft(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PressLeft && args.Count == 2)
		{
			PressLeft(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveLeft && args.Count == 3)
		{
			MoveLeft(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DragLeft && args.Count == 3)
		{
			DragLeft(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClickRight && args.Count == 2)
		{
			ClickRight(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitCanvasInput && args.Count == 2)
		{
			EmitCanvasInput(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<InputEvent>(in args[1]));
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
		if (method == MethodName.ViewAt && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ViewAt(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.SelectNodeForCanvas && args.Count == 2)
		{
			SelectNodeForCanvas(VariantUtils.ConvertTo<XWSceneTreeDock>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClickLeft && args.Count == 2)
		{
			ClickLeft(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PressLeft && args.Count == 2)
		{
			PressLeft(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveLeft && args.Count == 3)
		{
			MoveLeft(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DragLeft && args.Count == 3)
		{
			DragLeft(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClickRight && args.Count == 2)
		{
			ClickRight(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitCanvasInput && args.Count == 2)
		{
			EmitCanvasInput(VariantUtils.ConvertTo<XW2DViewport>(in args[0]), VariantUtils.ConvertTo<InputEvent>(in args[1]));
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
		if (method == MethodName.ViewAt)
		{
			return true;
		}
		if (method == MethodName.SelectNodeForCanvas)
		{
			return true;
		}
		if (method == MethodName.ClickLeft)
		{
			return true;
		}
		if (method == MethodName.PressLeft)
		{
			return true;
		}
		if (method == MethodName.MoveLeft)
		{
			return true;
		}
		if (method == MethodName.DragLeft)
		{
			return true;
		}
		if (method == MethodName.ClickRight)
		{
			return true;
		}
		if (method == MethodName.EmitCanvasInput)
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
