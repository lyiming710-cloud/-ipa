using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://Tests/ModEditor2DResourceDropRuntimeProbe.cs")]
public class ModEditor2DResourceDropRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private const string TexturePath = "res://addons/ModEditor/Icons/Object.svg";

	private const string ScenePath = "res://Tests/ModEditor2DResourceDropRuntimeProbeDroppedScene.tscn";

	private const string Scene3DPath = "res://Tests/ModEditor2DResourceDropRuntimeProbe3DScene.tscn";

	private const string AudioPath = "res://Asset/Audio/Sfx/Zombie/ZombieEnteringWater.ogg";

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private string _savedProbePath = "";

	public override async void _Ready()
	{
		_ = 9;
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
			_savedProbePath = $"res://Tests/.mod_editor_2d_resource_drop_{Guid.NewGuid():N}.tscn";
			Node2D node2D = new Node2D
			{
				Name = "ResourceDropProbeRoot"
			};
			PackedScene packedScene = new PackedScene
			{
				ResourceName = "ResourceDropProbe"
			};
			Require(packedScene.Pack(node2D) == Error.Ok, "Could not pack the resource-drop probe scene.");
			node2D.Free();
			Require(ResourceSaver.Save(packedScene, _savedProbePath, ResourceSaver.SaverFlags.None) == Error.Ok, "Could not save the temporary resource-drop probe scene.");
			editor.LoadPackedSceneFromPath(_savedProbePath);
			await WaitFrames(10);
			XW2DViewport viewport = editor.FindChild("Viewport2D", recursive: true, owned: false) as XW2DViewport;
			Require(GodotObject.IsInstanceValid(viewport), "The real 2D viewport was not created.");
			if (!GodotObject.IsInstanceValid(viewport))
			{
				Finish();
				return;
			}
			XWUndoRedoManager history = XWEditorInterface.Instance?.GetUndoRedoManager();
			Require(GodotObject.IsInstanceValid(history), "The global ModEditor undo manager is missing.");
			history?.ClearHistory();
			history?.SetCurrentHistoryType(1);
			XWInspector inspector = FindNodeOfType<XWInspector>(GetTree().Root);
			GodotObject inspectedBeforeDrop = inspector?.CurrentObject;
			Dictionary dictionary = new Dictionary();
			Variant key = "type";
			dictionary[key] = "files";
			Variant key2 = "files";
			dictionary[key2] = new string[3] { "res://addons/ModEditor/Icons/Object.svg", "res://Tests/ModEditor2DResourceDropRuntimeProbeDroppedScene.tscn", "res://Asset/Audio/Sfx/Zombie/ZombieEnteringWater.ogg" };
			Dictionary from = dictionary;
			Vector2 atPosition = viewport.Size * 0.5f + new Vector2(96f, 48f);
			bool accepted = viewport._CanDropData(atPosition, Variant.From(in from));
			viewport._DropData(atPosition, Variant.From(in from));
			await WaitFrames(6);
			Node editedRoot = editor.CurrentSceneInstance;
			Sprite2D textureNode = editedRoot?.FindChild("Object", recursive: false, owned: false) as Sprite2D;
			Node2D sceneNode = editedRoot?.FindChild("ModEditor2DResourceDropRuntimeProbeDroppedScene", recursive: false, owned: false) as Node2D;
			AudioStreamPlayer2D audioNode = editedRoot?.FindChild("ZombieEnteringWater", recursive: false, owned: false) as AudioStreamPlayer2D;
			bool created = GodotObject.IsInstanceValid(textureNode) && GodotObject.IsInstanceValid(sceneNode) && GodotObject.IsInstanceValid(audioNode);
			bool positioned = created && textureNode.Position.IsEqualApprox(new Vector2(96f, 48f)) && sceneNode.Position.IsEqualApprox(new Vector2(120f, 72f)) && audioNode.Position.IsEqualApprox(new Vector2(144f, 96f));
			bool singleHistory = history != null && history.HasUndo() && history.GetCurrentActionName() == "拖入 3 个资源到 2D 画布";
			XWSceneTreeDock xWSceneTreeDock = XWEditorInterface.Instance?.GetSceneTreeDock();
			bool treeSynced = created && GodotObject.IsInstanceValid(xWSceneTreeDock) && xWSceneTreeDock.GetSelectedNode() == audioNode;
			bool inspectorUntouched = GodotObject.IsInstanceValid(inspector) && inspector.CurrentObject == inspectedBeforeDrop && inspector.CurrentObject != audioNode;
			Require(accepted, "The viewport rejected supported texture, scene, and audio resource drag data.");
			Require(created, "Dropping texture, scene, and audio resources did not create all expected 2D nodes.");
			Require(positioned, "Dropped resource nodes were not placed at the canvas drop position with visual cascading.");
			Require(singleHistory, "A multi-resource canvas drop did not create one Scene undo action.");
			Require(treeSynced, "The scene tree did not highlight the last node created by the canvas drop.");
			Require(inspectorUntouched, "The canvas resource drop redirected the raw Inspector.");
			bool undoCalled = history?.Undo() ?? false;
			await WaitFrames(4);
			bool undo = undoCalled && textureNode.GetParent() == null && sceneNode.GetParent() == null && audioNode.GetParent() == null;
			bool redoCalled = history?.Redo() ?? false;
			await WaitFrames(4);
			bool redo = redoCalled && textureNode.GetParent() == editedRoot && sceneNode.GetParent() == editedRoot && audioNode.GetParent() == editedRoot && textureNode.Position.IsEqualApprox(new Vector2(96f, 48f));
			Require(undo, "Undo did not detach every node from the multi-resource canvas drop.");
			Require(redo, "Redo did not restore the same dropped nodes and canvas positions.");
			bool saved = editor.SaveCurrentScene();
			await WaitFrames(4);
			Node node = ResourceLoader.Load<PackedScene>(_savedProbePath, "", ResourceLoader.CacheMode.Ignore)?.Instantiate(PackedScene.GenEditState.Disabled);
			bool reload = GodotObject.IsInstanceValid(node) && node.FindChild("Object", recursive: false, owned: false) is Sprite2D && node.FindChild("ModEditor2DResourceDropRuntimeProbeDroppedScene", recursive: false, owned: false) is Node2D && node.FindChild("ZombieEnteringWater", recursive: false, owned: false) is AudioStreamPlayer2D;
			node?.Free();
			Require(saved, "The 2D editor did not save the resource-drop result.");
			Require(reload, "The saved resource-drop result did not survive a cache-ignored scene reload.");
			Dictionary unsupportedData = new Dictionary
			{
				["type"] = "files",
				["files"] = new string[1] { "res://project.godot" }
			};
			bool rejectsUnsupported = !viewport._CanDropData(viewport.Size * 0.5f, Variant.From(in unsupportedData));
			Require(rejectsUnsupported, "The 2D viewport accepts unsupported text/config files as visual nodes.");
			int childCountBefore3D = editedRoot.GetChildCount();
			dictionary = new Dictionary();
			key = "type";
			dictionary[key] = "files";
			key2 = "files";
			dictionary[key2] = new string[1] { "res://Tests/ModEditor2DResourceDropRuntimeProbe3DScene.tscn" };
			Dictionary from2 = dictionary;
			bool rejects3D = !viewport._CanDropData(viewport.Size * 0.5f, Variant.From(in from2));
			viewport._DropData(viewport.Size * 0.5f, Variant.From(in from2));
			await WaitFrames(2);
			rejects3D = rejects3D && editedRoot.GetChildCount() == childCountBefore3D;
			Require(rejects3D, "The 2D viewport accepted or created a node for a Node3D-rooted scene.");
			bool cleanupUndoCalled = history?.Undo() ?? false;
			await WaitFrames(2);
			dictionary = new Dictionary();
			key2 = "type";
			dictionary[key2] = "files";
			key = "files";
			dictionary[key] = new string[1] { "res://addons/ModEditor/Icons/Object.svg" };
			Dictionary from3 = dictionary;
			viewport._DropData(viewport.Size * 0.5f, Variant.From(in from3));
			await WaitFrames(3);
			int num;
			if (cleanupUndoCalled && !GodotObject.IsInstanceValid(textureNode) && !GodotObject.IsInstanceValid(sceneNode) && !GodotObject.IsInstanceValid(audioNode))
			{
				if (history != null && !history.HasRedo())
				{
					num = ((editedRoot.FindChild("Object", recursive: false, owned: false) is Sprite2D) ? 1 : 0);
					goto IL_0d99;
				}
			}
			num = 0;
			goto IL_0d99;
			IL_0d99:
			bool flag = (byte)num != 0;
			Require(flag, "Starting a new canvas drop after undo did not release the discarded resource-drop redo batch.");
			Sprite2D sprite2D = editedRoot.FindChild("Object", recursive: false, owned: false) as Sprite2D;
			bool flag2 = viewport.GetTrackedDroppedResourceBatchCount() == 1;
			history?.ClearHistory(1);
			viewport._CanDropData(viewport.Size * 0.5f, Variant.From(in unsupportedData));
			int num2;
			if (flag2 && viewport.GetTrackedDroppedResourceBatchCount() == 0 && GodotObject.IsInstanceValid(sprite2D) && sprite2D.GetParent() == editedRoot)
			{
				if (history != null && !history.HasUndo())
				{
					num2 = ((history != null && !history.HasRedo()) ? 1 : 0);
					goto IL_0e67;
				}
			}
			num2 = 0;
			goto IL_0e67;
			IL_0e67:
			bool flag3 = (byte)num2 != 0;
			Require(flag3, "Clearing Scene history did not release tracked drop batches while preserving applied nodes.");
			GD.Print($"[MOD_EDITOR_2D_RESOURCE_DROP_PROBE] window={window} accepted={accepted} created={created} positioned={positioned} singleHistory={singleHistory} treeSynced={treeSynced} inspectorUntouched={inspectorUntouched} undo={undo} redo={redo} saved={saved} reload={reload} rejectsUnsupported={rejectsUnsupported} rejects3D={rejects3D} historyRelease={flag} clearHistoryRelease={flag3} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
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
			GD.PrintErr("[MOD_EDITOR_2D_RESOURCE_DROP_PROBE_FAILURE] " + message);
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
			GD.PrintErr("[MOD_EDITOR_2D_RESOURCE_DROP_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
