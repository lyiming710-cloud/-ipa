using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ResourceEditors;
using PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://Tests/ModEditorCollisionGeometryRuntimeProbe.cs")]
public class ModEditorCollisionGeometryRuntimeProbe : Node
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
		public static readonly StringName _editor = "_editor";

		public static readonly StringName _history = "_history";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWGenericVisualResourceEditor _editor;

	private XWUndoRedoManager _history;

	public override async void _Ready()
	{
		_ = 4;
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
			bool flag = await WaitForEditor(900);
			Require(flag, "F3 did not initialize the collision geometry editor within 900 frames.");
			if (!flag)
			{
				Finish();
				return;
			}
			bool window = FindAncestorWindow(_editor) != null;
			Require(window, "CollisionGeometry editor is not mounted under the F3 ModEditor window.");
			(bool, bool, bool, bool, bool, bool) tuple = await ProbeHitBox();
			bool route = tuple.Item1;
			bool inspectorHidden = tuple.Item2;
			bool directCanvas = tuple.Item3;
			bool continuousMerged = tuple.Item4;
			bool originUndoRedo = tuple.Item5;
			bool saveRoundTrip = tuple.Item6;
			(bool, bool) tuple2 = await ProbeShape();
			bool shapeRoute = tuple2.Item1;
			bool shapeUndoRedo = tuple2.Item2;
			bool value = await ProbeRay();
			GD.Print($"[MOD_EDITOR_COLLISION_GEOMETRY_PROBE] window={window} route={route} inspectorHidden={inspectorHidden} directCanvas={directCanvas} continuousMerged={continuousMerged} originUndoRedo={originUndoRedo} saveRoundTrip={saveRoundTrip} shapeRoute={shapeRoute} shapeUndoRedo={shapeUndoRedo} rayRoute={value} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private async Task<(bool route, bool inspectorHidden, bool directCanvas, bool continuousMerged, bool originUndoRedo, bool saveRoundTrip)> ProbeHitBox()
	{
		string savePath = "user://ModEditorCollisionGeometryProbe.tres";
		CharacterHitBoxDefinition hitBox = new CharacterHitBoxDefinition
		{
			ResourceName = "ProbeHitBox",
			Size = new Vector2(80f, 60f),
			LocalTransform = new Transform2D(0f, new Vector2(4f, 6f))
		};
		Error error = ResourceSaver.Save(hitBox, savePath, ResourceSaver.SaverFlags.None);
		Require(error == Error.Ok, $"Could not create collision save probe resource: {error}.");
		hitBox.TakeOverPath(savePath);
		bool descriptorRoute = XWResourceEditorRegistry.TryGetEditor(hitBox, "res://Resource/TowerDefense/Collision/CharacterHitBoxes/Probe.tres", out var descriptor) && descriptor?.Category == "CollisionGeometry" && descriptor.DockKey == "collision_geometry_editor";
		await OpenResource(hitBox);
		bool route = descriptorRoute && XWEditorInterface.Instance.GetResourceEditor("collision_geometry_editor") == _editor;
		Require(route, "CharacterHitBoxDefinition did not route to collision_geometry_editor.");
		PanelContainer panelContainer = FindControl<PanelContainer>("InspectorPanel");
		VBoxContainer vBoxContainer = FindControl<VBoxContainer>("EmbeddedInspectorHost");
		bool inspectorHidden = GodotObject.IsInstanceValid(panelContainer) && !panelContainer.Visible && GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() == 0 && _editor.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null;
		Require(inspectorHidden, "CharacterHitBoxDefinition still materializes the raw InspectorPanel.");
		XWCollisionGeometryCanvas canvas = FindControl<XWCollisionGeometryCanvas>("CollisionGeometryCanvas");
		bool directCanvas = GodotObject.IsInstanceValid(canvas) && canvas.OriginCommitted != null && canvas.GeometryCommitted != null;
		Require(directCanvas, "Collision geometry drag canvas is missing or not wired.");
		SpinBox spinBox = FindControl<SpinBox>("CollisionOriginX");
		bool continuousMerged = false;
		if (GodotObject.IsInstanceValid(spinBox))
		{
			_history.ClearHistory();
			int beforeVersion = _history.GetVersion();
			spinBox.EmitSignal(Control.SignalName.FocusEntered);
			double[] array = new double[3] { 9.0, 17.0, 25.0 };
			foreach (double num in array)
			{
				spinBox.SetValueNoSignal(num);
				spinBox.EmitSignal(Godot.Range.SignalName.ValueChanged, num);
			}
			spinBox.EmitSignal(Control.SignalName.FocusExited);
			await WaitFrames(3);
			continuousMerged = Math.Abs(hitBox.LocalTransform.Origin.X - 25f) < 0.001f && _history.GetVersion() - beforeVersion == 1 && _history.HasUndo();
		}
		Require(continuousMerged, "Continuous SpinBox input produced multiple history actions or per-step commits.");
		bool originUndoRedo = false;
		if (directCanvas)
		{
			_history.ClearHistory();
			Vector2 before = hitBox.LocalTransform.Origin;
			Vector2 after = new Vector2(33f, -17f);
			canvas.OriginCommitted(after);
			await WaitFrames(3);
			bool applied = hitBox.LocalTransform.Origin.IsEqualApprox(after) && _history.HasUndo();
			bool undoCalled = _history.Undo();
			await WaitFrames(3);
			bool undone = undoCalled && hitBox.LocalTransform.Origin.IsEqualApprox(before);
			bool redoCalled = _history.Redo();
			await WaitFrames(3);
			bool flag = redoCalled && hitBox.LocalTransform.Origin.IsEqualApprox(after);
			originUndoRedo = applied & undone & flag;
		}
		Require(originUndoRedo, "Canvas origin drag did not round-trip through shared UndoRedo.");
		Button button = FindControl<Button>("SaveCollisionGeometryButton");
		bool flag2 = false;
		if (GodotObject.IsInstanceValid(button))
		{
			button.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(4);
			CharacterHitBoxDefinition characterHitBoxDefinition = ResourceLoader.Load<CharacterHitBoxDefinition>(savePath, "", ResourceLoader.CacheMode.Replace);
			flag2 = GodotObject.IsInstanceValid(characterHitBoxDefinition) && characterHitBoxDefinition.LocalTransform.Origin.IsEqualApprox(new Vector2(33f, -17f));
		}
		Require(flag2, "The direct collision save button did not persist the edited transform.");
		DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(savePath));
		return (route: route, inspectorHidden: inspectorHidden, directCanvas: directCanvas, continuousMerged: continuousMerged, originUndoRedo: originUndoRedo, saveRoundTrip: flag2);
	}

	private async Task<(bool shapeRoute, bool shapeUndoRedo)> ProbeShape()
	{
		RectangleShape2D rectangle = new RectangleShape2D
		{
			Size = new Vector2(64f, 40f)
		};
		AabbShape2DResource resource = new AabbShape2DResource
		{
			Geometry = rectangle
		};
		bool descriptorRoute = XWResourceEditorRegistry.TryGetEditor(resource, "", out var descriptor) && descriptor?.Category == "CollisionGeometry";
		await OpenResource(resource);
		XWCollisionGeometryCanvas xWCollisionGeometryCanvas = FindControl<XWCollisionGeometryCanvas>("CollisionGeometryCanvas");
		bool shapeRoute = descriptorRoute && GodotObject.IsInstanceValid(xWCollisionGeometryCanvas);
		Require(shapeRoute, "AabbShape2DResource did not route to the collision canvas.");
		bool flag = false;
		if (shapeRoute)
		{
			_history.ClearHistory();
			Vector2 before = rectangle.Size;
			Vector2 after = new Vector2(144f, 92f);
			xWCollisionGeometryCanvas.GeometryCommitted(after);
			await WaitFrames(3);
			bool applied = rectangle.Size.IsEqualApprox(after) && _history.HasUndo();
			bool undoCalled = _history.Undo();
			await WaitFrames(3);
			bool undone = undoCalled && rectangle.Size.IsEqualApprox(before);
			bool redoCalled = _history.Redo();
			await WaitFrames(3);
			bool flag2 = redoCalled && rectangle.Size.IsEqualApprox(after);
			flag = applied & undone & flag2;
		}
		Require(flag, "Rectangle canvas resize did not round-trip through shared UndoRedo.");
		return (shapeRoute: shapeRoute, shapeUndoRedo: flag);
	}

	private async Task<bool> ProbeRay()
	{
		AabbRay2DResource resource = new AabbRay2DResource
		{
			TargetPosition = new Vector2(260f, 12f)
		};
		bool descriptorRoute = XWResourceEditorRegistry.TryGetEditor(resource, "", out var descriptor) && descriptor?.Category == "CollisionGeometry";
		await OpenResource(resource);
		XWCollisionGeometryCanvas instance = FindControl<XWCollisionGeometryCanvas>("CollisionGeometryCanvas");
		PanelContainer panelContainer = FindControl<PanelContainer>("InspectorPanel");
		bool flag = descriptorRoute && GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(panelContainer) && !panelContainer.Visible;
		Require(flag, "AabbRay2DResource did not open on the inspector-free collision canvas.");
		return flag;
	}

	private async Task OpenResource(Resource resource)
	{
		XWEditorInterface.Instance.EditResource(resource);
		await WaitFrames(5);
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			XWEditorInterface instance = XWEditorInterface.Instance;
			if (instance != null && instance.GetResourceEditor("collision_geometry_editor") is XWGenericVisualResourceEditor xWGenericVisualResourceEditor && GodotObject.IsInstanceValid(xWGenericVisualResourceEditor))
			{
				_editor = xWGenericVisualResourceEditor;
				_history = instance.GetUndoRedoManager();
				return GodotObject.IsInstanceValid(_history);
			}
			await WaitFrames(1);
		}
		return false;
	}

	private T FindControl<T>(string name) where T : Node
	{
		return _editor?.FindChild(name, recursive: true, owned: false) as T;
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
			GD.PrintErr("[MOD_EDITOR_COLLISION_GEOMETRY_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		if (_failures.Count > 0)
		{
			foreach (string failure in _failures)
			{
				GD.PrintErr("[MOD_EDITOR_COLLISION_GEOMETRY_PROBE_FAILURE] " + failure);
			}
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
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XWGenericVisualResourceEditor>(in value);
			return true;
		}
		if (name == PropertyName._history)
		{
			_history = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
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
		if (name == PropertyName._history)
		{
			value = VariantUtils.CreateFrom(in _history);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._history, Variant.From(in _history));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XWGenericVisualResourceEditor>();
		}
		if (info.TryGetProperty(PropertyName._history, out var value2))
		{
			_history = value2.As<XWUndoRedoManager>();
		}
	}
}
