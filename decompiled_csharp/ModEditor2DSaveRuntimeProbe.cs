using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://Tests/ModEditor2DSaveRuntimeProbe.cs")]
public class ModEditor2DSaveRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateNestedSceneFixture = "CreateNestedSceneFixture";

		public static readonly StringName CommitPosition = "CommitPosition";

		public static readonly StringName ReadSavedPosition = "ReadSavedPosition";

		public static readonly StringName SendKey = "SendKey";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";

		public static readonly StringName RemoveProbeFile = "RemoveProbeFile";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _nestedScenePath = "_nestedScenePath";

		public static readonly StringName _parentScenePath = "_parentScenePath";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private string _nestedScenePath = "";

	private string _parentScenePath = "";

	public override async void _Ready()
	{
		bool window = false;
		bool ctrlSaved = false;
		bool ctrlHistorySaved = false;
		bool menuSaved = false;
		bool menuHistorySaved = false;
		bool nestedBoundary = false;
		bool nestedOwner = false;
		bool nestedNotExpanded = false;
		bool insertedBoundary = false;
		bool insertedRedoBoundary = false;
		bool branchPacked = false;
		bool branchOwnersRestored = false;
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
			SendKey(Key.F3, ctrl: false);
			XW2DSceneEditor editor = await WaitForSceneEditor(900);
			window = FindAncestorWindow(editor) != null;
			Require(window, "2D scene editor is not mounted under the real F3 ModEditor window.");
			if (!GodotObject.IsInstanceValid(editor))
			{
				Finish();
				return;
			}
			await WaitFrames(60);
			string text = Guid.NewGuid().ToString("N");
			_nestedScenePath = "res://Tests/.mod_editor_2d_nested_" + text + ".tscn";
			_parentScenePath = "res://Tests/.mod_editor_2d_save_" + text + ".tscn";
			Require(CreateNestedSceneFixture(), "Could not create nested PackedScene save fixture.");
			editor.LoadPackedSceneFromPath(_parentScenePath);
			await WaitFrames(12);
			Node editedRoot = editor.CurrentSceneInstance;
			Node2D moveTarget = editedRoot?.GetNodeOrNull<Node2D>("MoveTarget");
			XWUndoRedoManager history = XWEditorInterface.Instance?.GetUndoRedoManager();
			int historyId = editor.GetCurrentSceneHistoryId();
			Require(GodotObject.IsInstanceValid(editedRoot), "Editable parent scene root is missing.");
			Require(GodotObject.IsInstanceValid(moveTarget), "Editable MoveTarget is missing.");
			Require(GodotObject.IsInstanceValid(history), "2D scene history manager is missing.");
			if (!GodotObject.IsInstanceValid(editedRoot) || !GodotObject.IsInstanceValid(moveTarget) || !GodotObject.IsInstanceValid(history))
			{
				Finish();
				return;
			}
			Vector2 ctrlPosition = new Vector2(128f, 64f);
			CommitPosition(history, historyId, moveTarget, ctrlPosition, "Ctrl+S probe move");
			bool ctrlDirtyBefore = history.IsHistoryUnsaved(historyId);
			FindNodeOfType<XW2DViewport>(editor)?.GrabFocus();
			SendKey(Key.S, ctrl: true);
			await WaitFrames(8);
			ctrlHistorySaved = ctrlDirtyBefore && !history.IsHistoryUnsaved(historyId);
			ctrlSaved = ReadSavedPosition(_parentScenePath).IsEqualApprox(ctrlPosition);
			Require(ctrlSaved, "Ctrl+S did not persist the active 2D scene position.");
			Require(ctrlHistorySaved, "Ctrl+S did not mark the current 2D scene history as saved.");
			Vector2 menuPosition = new Vector2(224f, 96f);
			CommitPosition(history, historyId, moveTarget, menuPosition, "File menu probe move");
			bool menuDirtyBefore = history.IsHistoryUnsaved(historyId);
			PopupMenu popupMenu = FindNodeNamed<PopupMenu>(FindAncestorWindow(editor), "文件");
			Require(GodotObject.IsInstanceValid(popupMenu), "The real ModEditor File menu is missing.");
			popupMenu?.EmitSignal(PopupMenu.SignalName.IdPressed, 2L);
			await WaitFrames(8);
			menuHistorySaved = menuDirtyBefore && !history.IsHistoryUnsaved(historyId);
			menuSaved = ReadSavedPosition(_parentScenePath).IsEqualApprox(menuPosition);
			Require(menuSaved, "Top-bar File > Save did not persist the active 2D scene position.");
			Require(menuHistorySaved, "Top-bar File > Save did not mark the current 2D scene history as saved.");
			string fileAsString = FileAccess.GetFileAsString(_parentScenePath);
			nestedNotExpanded = fileAsString.Contains(_nestedScenePath, StringComparison.Ordinal) && Regex.IsMatch(fileAsString, "\\[node\\s+name=\"NestedInstance\"[^\\]]*instance=ExtResource\\(") && !fileAsString.Contains("[node name=\"NestedVisual\" parent=\"NestedInstance", StringComparison.Ordinal);
			Node node = ResourceLoader.Load<PackedScene>(_parentScenePath, "", ResourceLoader.CacheMode.Ignore)?.Instantiate(PackedScene.GenEditState.Disabled);
			Node node2 = node?.GetNodeOrNull<Node>("NestedInstance");
			Node node3 = node2?.GetNodeOrNull<Node>("NestedVisual");
			nestedBoundary = GodotObject.IsInstanceValid(node2) && node2.SceneFilePath == _nestedScenePath && GodotObject.IsInstanceValid(node3);
			nestedOwner = nestedBoundary && node3.Owner == node2;
			node?.Free();
			Require(nestedBoundary, "Saving and reloading flattened or detached the nested PackedScene instance boundary.");
			Require(nestedOwner, "Saving and reloading rewrote the nested instance child's Owner to the parent scene root.");
			Require(nestedNotExpanded, "Saved parent scene text expanded the nested scene's internal node into the parent file.");
			XWSceneTreeDock xWSceneTreeDock = XWEditorInterface.Instance?.GetSceneTreeDock();
			Require(GodotObject.IsInstanceValid(xWSceneTreeDock), "The real 2D scene tree dock is unavailable.");
			xWSceneTreeDock?.SetScene(editedRoot);
			Node insertedInstance = xWSceneTreeDock?.InstantiateSceneWithHistory(_nestedScenePath);
			await WaitFrames(4);
			Node node4 = insertedInstance?.GetNodeOrNull<Node>("NestedVisual");
			insertedBoundary = GodotObject.IsInstanceValid(insertedInstance) && insertedInstance.SceneFilePath == _nestedScenePath && insertedInstance.Owner == editedRoot && GodotObject.IsInstanceValid(node4) && node4.Owner == insertedInstance;
			Require(insertedBoundary, "Scene-tree instantiation rewrote the nested PackedScene internal Owner graph.");
			bool insertionUndone = history.Undo();
			bool insertionRedone = history.Redo();
			await WaitFrames(4);
			node4 = insertedInstance?.GetNodeOrNull<Node>("NestedVisual");
			insertedRedoBoundary = (insertionUndone & insertionRedone) && insertedInstance?.GetParent() == editedRoot && insertedInstance.Owner == editedRoot && GodotObject.IsInstanceValid(node4) && node4.Owner == insertedInstance;
			Require(insertedRedoBoundary, "Undo/redo of a scene instance flattened or detached its internal Owner graph.");
			Node2D node2D = new Node2D
			{
				Name = "PackedBranch"
			};
			Node2D node2D2 = new Node2D
			{
				Name = "PackedBranchChild"
			};
			editedRoot.AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
			node2D.Owner = editedRoot;
			node2D.AddChild(node2D2, forceReadableName: false, InternalMode.Disabled);
			node2D2.Owner = editedRoot;
			Error error = XWSceneTreeDock.PackBranchPreservingOwners(node2D, out var packedScene);
			Node node5 = ((error == Error.Ok) ? packedScene.Instantiate(PackedScene.GenEditState.Disabled) : null);
			branchPacked = error == Error.Ok && GodotObject.IsInstanceValid(node5?.GetNodeOrNull<Node>("PackedBranchChild"));
			branchOwnersRestored = node2D.Owner == editedRoot && node2D2.Owner == editedRoot;
			node5?.Free();
			Require(branchPacked, "Save Branch as Scene omitted ordinary local descendants.");
			Require(branchOwnersRestored, "Save Branch as Scene did not restore the live scene Owner graph.");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		GD.Print($"[MOD_EDITOR_2D_SAVE_PROBE] window={window} ctrlSaved={ctrlSaved} ctrlHistorySaved={ctrlHistorySaved} menuSaved={menuSaved} menuHistorySaved={menuHistorySaved} nestedBoundary={nestedBoundary} nestedOwner={nestedOwner} nestedNotExpanded={nestedNotExpanded} insertedBoundary={insertedBoundary} insertedRedoBoundary={insertedRedoBoundary} branchPacked={branchPacked} branchOwnersRestored={branchOwnersRestored} failures={_failures.Count}");
		Finish();
	}

	private bool CreateNestedSceneFixture()
	{
		Node2D node2D = new Node2D
		{
			Name = "NestedTemplate"
		};
		ColorRect colorRect = new ColorRect
		{
			Name = "NestedVisual",
			Position = new Vector2(-16f, -16f),
			Size = new Vector2(32f, 32f),
			Color = new Color(0.25f, 0.8f, 0.45f),
			MouseFilter = Control.MouseFilterEnum.Ignore
		};
		node2D.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
		colorRect.Owner = node2D;
		PackedScene packedScene = new PackedScene();
		Error error = packedScene.Pack(node2D);
		node2D.Free();
		if (error != Error.Ok || ResourceSaver.Save(packedScene, _nestedScenePath, ResourceSaver.SaverFlags.None) != Error.Ok)
		{
			return false;
		}
		Node node = ResourceLoader.Load<PackedScene>(_nestedScenePath, "", ResourceLoader.CacheMode.Ignore)?.Instantiate(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(node))
		{
			return false;
		}
		node.Name = "NestedInstance";
		Node2D node2D2 = new Node2D
		{
			Name = "SaveProbeRoot"
		};
		node2D2.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		node.Owner = node2D2;
		Node2D node2D3 = new Node2D
		{
			Name = "MoveTarget",
			Position = new Vector2(12f, 8f)
		};
		node2D2.AddChild(node2D3, forceReadableName: false, InternalMode.Disabled);
		node2D3.Owner = node2D2;
		PackedScene packedScene2 = new PackedScene();
		Error error2 = packedScene2.Pack(node2D2);
		node2D2.Free();
		if (error2 == Error.Ok)
		{
			return ResourceSaver.Save(packedScene2, _parentScenePath, ResourceSaver.SaverFlags.None) == Error.Ok;
		}
		return false;
	}

	private static void CommitPosition(XWUndoRedoManager history, int historyId, Node2D target, Vector2 position, string actionName)
	{
		Vector2 from = target.Position;
		history.CreateAction(actionName, mergeMode: false, historyId);
		history.AddDoProperty(target, "position", Variant.From(in position));
		history.AddUndoProperty(target, "position", Variant.From(in from));
		history.CommitAction();
	}

	private static Vector2 ReadSavedPosition(string path)
	{
		Node node = ResourceLoader.Load<PackedScene>(path, "", ResourceLoader.CacheMode.Ignore)?.Instantiate(PackedScene.GenEditState.Disabled);
		Vector2 result = node?.GetNodeOrNull<Node2D>("MoveTarget")?.Position ?? new Vector2(1f / 0f, 1f / 0f);
		node?.Free();
		return result;
	}

	private static void SendKey(Key key, bool ctrl)
	{
		Input.ParseInputEvent(new InputEventKey
		{
			Keycode = key,
			PhysicalKeycode = key,
			CtrlPressed = ctrl,
			Pressed = true
		});
		Input.ParseInputEvent(new InputEventKey
		{
			Keycode = key,
			PhysicalKeycode = key,
			CtrlPressed = ctrl,
			Pressed = false
		});
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

	private static T FindNodeNamed<T>(Node root, string name) where T : Node
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		if (root is T result && root.Name == (StringName)name)
		{
			return result;
		}
		foreach (Node child in root.GetChildren())
		{
			T val = FindNodeNamed<T>(child, name);
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
			GD.PrintErr("[MOD_EDITOR_2D_SAVE_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		RemoveProbeFile(_parentScenePath);
		RemoveProbeFile(_nestedScenePath);
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_2D_SAVE_PROBE_FAILURE] " + failure);
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
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateNestedSceneFixture, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "history", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadSavedPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "ctrl", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateNestedSceneFixture && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CreateNestedSceneFixture());
			return true;
		}
		if (method == MethodName.CommitPosition && args.Count == 5)
		{
			CommitPosition(VariantUtils.ConvertTo<XWUndoRedoManager>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Node2D>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadSavedPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ReadSavedPosition(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SendKey && args.Count == 2)
		{
			SendKey(VariantUtils.ConvertTo<Key>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
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
		if (method == MethodName.CommitPosition && args.Count == 5)
		{
			CommitPosition(VariantUtils.ConvertTo<XWUndoRedoManager>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Node2D>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadSavedPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ReadSavedPosition(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SendKey && args.Count == 2)
		{
			SendKey(VariantUtils.ConvertTo<Key>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
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
		if (method == MethodName.CreateNestedSceneFixture)
		{
			return true;
		}
		if (method == MethodName.CommitPosition)
		{
			return true;
		}
		if (method == MethodName.ReadSavedPosition)
		{
			return true;
		}
		if (method == MethodName.SendKey)
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
		if (name == PropertyName._nestedScenePath)
		{
			_nestedScenePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._parentScenePath)
		{
			_parentScenePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._nestedScenePath)
		{
			value = VariantUtils.CreateFrom(in _nestedScenePath);
			return true;
		}
		if (name == PropertyName._parentScenePath)
		{
			value = VariantUtils.CreateFrom(in _parentScenePath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._nestedScenePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._parentScenePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._nestedScenePath, Variant.From(in _nestedScenePath));
		info.AddProperty(PropertyName._parentScenePath, Variant.From(in _parentScenePath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._nestedScenePath, out var value))
		{
			_nestedScenePath = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._parentScenePath, out var value2))
		{
			_parentScenePath = value2.As<string>();
		}
	}
}
