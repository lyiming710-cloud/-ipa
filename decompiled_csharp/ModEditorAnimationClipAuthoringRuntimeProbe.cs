using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.AdobeAnimateEditor.Inspector;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://Tests/ModEditorAnimationClipAuthoringRuntimeProbe.cs")]
public class ModEditorAnimationClipAuthoringRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindButtonByText = "FindButtonByText";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _editor = "_editor";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string FixturePath = "res://Asset/Anime/Splat/Spike/SpikeSplat.tres";

	private const string DraftPath = "user://mod_editor_animation_clip_authoring_probe.tres";

	private const string ProbeClipName = "probe_visual_clip";

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWAnimationVisualResourceEditor _editor;

	public override async void _Ready()
	{
		_ = 14;
		try
		{
			AdobeAnimateData adobeAnimateData = ResourceLoader.Load<AdobeAnimateData>("res://Asset/Anime/Splat/Spike/SpikeSplat.tres", null, ResourceLoader.CacheMode.Reuse);
			Require(GodotObject.IsInstanceValid(adobeAnimateData) && adobeAnimateData.HasPackedRuntimeData(), "Real SpikeSplat animation fixture is unavailable.");
			if (!GodotObject.IsInstanceValid(adobeAnimateData) || !adobeAnimateData.HasPackedRuntimeData())
			{
				Finish();
				return;
			}
			AdobeAnimateData draft = adobeAnimateData.Duplicate(deep: true) as AdobeAnimateData;
			Require(GodotObject.IsInstanceValid(draft), "Could not duplicate the real animation fixture into an isolated editable resource.");
			if (!GodotObject.IsInstanceValid(draft))
			{
				Finish();
				return;
			}
			draft.ResourceName = "ModEditorClipAuthoringProbe";
			Error error = ResourceSaver.Save(draft, "user://mod_editor_animation_clip_authoring_probe.tres", ResourceSaver.SaverFlags.None);
			Require(error == Error.Ok, $"Could not create the isolated animation resource: {error}.");
			if (error != Error.Ok)
			{
				Finish();
				return;
			}
			draft = ResourceLoader.Load<AdobeAnimateData>("user://mod_editor_animation_clip_authoring_probe.tres", "", ResourceLoader.CacheMode.Replace);
			Require(GodotObject.IsInstanceValid(draft) && draft.HasPackedRuntimeData(), "Isolated animation resource did not reload with packed runtime data.");
			if (!GodotObject.IsInstanceValid(draft) || !draft.HasPackedRuntimeData())
			{
				Finish();
				return;
			}
			int[] frameOffsetsBefore = CloneArray(draft.frameOffsets);
			int[] frameCountsBefore = CloneArray(draft.frameCounts);
			int[] sliceKeysBefore = CloneArray(draft.sliceKeys);
			float[] sliceTransformsBefore = CloneArray(draft.sliceTransforms);
			float[] sliceAlphaBefore = CloneArray(draft.sliceAlpha);
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
			Require(flag, "F3 did not initialize the animation editor within 900 frames.");
			if (!flag)
			{
				Finish();
				return;
			}
			bool flag2 = await EnterEditorSurface(900);
			Require(flag2, "F3 editor did not finish loading its main editing surface.");
			if (!flag2)
			{
				Finish();
				return;
			}
			XWEditorInterface.Instance.EditResource(draft, XWResourceEditContext.ForRoot(draft, "user://mod_editor_animation_clip_authoring_probe.tres", "animation_editor"));
			XWEditorInterface.Instance.FocusPanel("animation_editor");
			AdobeAnimateInspectorPreview preview = await WaitForPreview(900);
			Require(GodotObject.IsInstanceValid(preview), "Animation authoring surface did not mount the real runtime preview.");
			if (!GodotObject.IsInstanceValid(preview))
			{
				Finish();
				return;
			}
			TabBar tabBar = _editor.FindChild("AnimationWorkbenchTabs", recursive: true, owned: false) as TabBar;
			if (GodotObject.IsInstanceValid(tabBar))
			{
				tabBar.CurrentTab = 4;
				await WaitFrames(3);
			}
			ItemList instance = _editor.FindChild("ClipList", recursive: true, owned: false) as ItemList;
			LineEdit clipName = _editor.FindChild("ClipNameEdit", recursive: true, owned: false) as LineEdit;
			SpinBox clipStart = _editor.FindChild("ClipStartSpinBox", recursive: true, owned: false) as SpinBox;
			SpinBox clipEnd = _editor.FindChild("ClipEndSpinBox", recursive: true, owned: false) as SpinBox;
			XWPacketSpawnEntryRangeControl rangeControl = _editor.FindChild("ClipRangeControl", recursive: true, owned: false) as XWPacketSpawnEntryRangeControl;
			Button button = _editor.FindChild("AddClipButton", recursive: true, owned: false) as Button;
			Button deleteButton = _editor.FindChild("DeleteClipButton", recursive: true, owned: false) as Button;
			Button shiftLaterButton = _editor.FindChild("ShiftLaterButton", recursive: true, owned: false) as Button;
			Label label = _editor.FindChild("PackedDataSafetyLabel", recursive: true, owned: false) as Label;
			VBoxContainer vBoxContainer = _editor.FindChild("EmbeddedInspectorHost", recursive: true, owned: false) as VBoxContainer;
			bool directVisual = GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(clipName) && GodotObject.IsInstanceValid(clipStart) && GodotObject.IsInstanceValid(clipEnd) && GodotObject.IsInstanceValid(rangeControl) && GodotObject.IsInstanceValid(button) && GodotObject.IsInstanceValid(deleteButton) && GodotObject.IsInstanceValid(shiftLaterButton) && GodotObject.IsInstanceValid(label) && label.IsVisibleInTree();
			bool inspectorHidden = GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() == 0;
			Require(directVisual, "Clip authoring controls are not directly visible in the animation main surface.");
			Require(inspectorHidden, "Clip authoring seized the raw embedded Inspector.");
			if (!directVisual)
			{
				Finish();
				return;
			}
			button.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			string createdName = _editor.SelectedAnimationClipName;
			bool creation = !string.IsNullOrWhiteSpace(createdName) && draft.HasClip(createdName) && draft.clips.Count >= 2;
			Require(creation, "Creating a Clip from the current real preview frame failed.");
			clipName.EmitSignal(Control.SignalName.FocusEntered);
			clipName.Text = "probe_visual_clip";
			clipName.EmitSignal(LineEdit.SignalName.TextSubmitted, "probe_visual_clip");
			await WaitFrames(2);
			bool rename = draft.HasClip("probe_visual_clip") && !draft.HasClip(createdName) && _editor.SelectedAnimationClipName == "probe_visual_clip";
			Require(rename, "Direct Clip rename did not update the resource and selection.");
			rangeControl.EmitSignal(XWPacketSpawnEntryRangeControl.SignalName.DragStarted, true);
			rangeControl.EmitSignal(XWPacketSpawnEntryRangeControl.SignalName.ValuesPreviewed, 1.0, 4.0);
			rangeControl.EmitSignal(XWPacketSpawnEntryRangeControl.SignalName.ValuesCommitted, 1.0, 4.0);
			await WaitFrames(2);
			bool visualRange = draft.GetClip("probe_visual_clip") == new Vector2I(1, 5);
			Require(visualRange, $"Dual-handle visual range did not update the Clip: {draft.GetClip("probe_visual_clip")}.");
			clipEnd.EmitSignal(Control.SignalName.FocusEntered);
			clipEnd.Value = 5.0;
			clipStart.Value = 2.0;
			clipEnd.EmitSignal(Control.SignalName.FocusExited);
			await WaitFrames(2);
			Vector2I clip = draft.GetClip("probe_visual_clip");
			bool range = clip == new Vector2I(2, 6);
			Require(range, $"Exact/visual Clip range did not preserve the end-exclusive runtime contract: {clip}.");
			AdobeAnimateSprite adobeAnimateSprite = preview.FindChild("InspectorAnimationSprite", recursive: true, owned: false) as AdobeAnimateSprite;
			bool previewSynced = GodotObject.IsInstanceValid(adobeAnimateSprite) && preview.SelectedClipName == "probe_visual_clip" && adobeAnimateSprite.clipRange == clip;
			Require(previewSynced, "Edited Clip range did not reach the real AdobeAnimateSprite preview.");
			shiftLaterButton.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			Vector2I clip2 = draft.GetClip("probe_visual_clip");
			bool move = clip2 == new Vector2I(3, 7);
			Require(move, $"Moving the entire Clip by one frame failed: {clip2}.");
			XWUndoRedoManager undoRedo = XWEditorInterface.Instance.GetUndoRedoManager();
			bool undoMove = undoRedo.Undo();
			await WaitFrames(2);
			bool undoRange = draft.GetClip("probe_visual_clip") == new Vector2I(2, 6);
			bool redoMove = undoRedo.Redo();
			await WaitFrames(2);
			bool flag3 = draft.GetClip("probe_visual_clip") == new Vector2I(3, 7);
			bool undoRedoPassed = undoMove & undoRange & redoMove & flag3;
			Require(undoRedoPassed, "Clip move did not survive global Undo/Redo.");
			deleteButton.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			bool deleted = !draft.HasClip("probe_visual_clip");
			bool undoDelete = undoRedo.Undo();
			await WaitFrames(2);
			bool deleteRecovery = (deleted & undoDelete) && draft.HasClip("probe_visual_clip") && draft.GetClip("probe_visual_clip") == new Vector2I(3, 7);
			Require(deleteRecovery, "Deleted Clip did not recover with its exact range through Undo.");
			Button button2 = FindButtonByText(_editor, "保存");
			Require(GodotObject.IsInstanceValid(button2) && !button2.Disabled, "Animation resource save action is unavailable.");
			if (GodotObject.IsInstanceValid(button2) && !button2.Disabled)
			{
				button2.EmitSignal(BaseButton.SignalName.Pressed);
				await WaitFrames(4);
			}
			AdobeAnimateData adobeAnimateData2 = ResourceLoader.Load<AdobeAnimateData>("user://mod_editor_animation_clip_authoring_probe.tres", "", ResourceLoader.CacheMode.Ignore);
			bool flag4 = GodotObject.IsInstanceValid(adobeAnimateData2) && adobeAnimateData2.HasClip("probe_visual_clip") && adobeAnimateData2.GetClip("probe_visual_clip") == new Vector2I(3, 7);
			Require(flag4, "Saved Clip creation/range did not survive an uncached disk reload.");
			bool flag5 = ArraysEqual(frameOffsetsBefore, draft.frameOffsets) && ArraysEqual(frameCountsBefore, draft.frameCounts) && ArraysEqual(sliceKeysBefore, draft.sliceKeys) && ArraysEqual(sliceTransformsBefore, draft.sliceTransforms) && ArraysEqual(sliceAlphaBefore, draft.sliceAlpha) && _editor.PackedFrameEditingSupported;
			Require(flag5, "Clip authoring mutated correlated packed keyframe arrays.");
			bool value = FindAncestorWindow(_editor) != null;
			GD.Print($"[MOD_EDITOR_ANIMATION_CLIP_AUTHORING_PROBE] window={value} directVisual={directVisual} inspectorHidden={inspectorHidden} creation={creation} rename={rename} visualRange={visualRange} range={range} previewSynced={previewSynced} move={move} undoRedo={undoRedoPassed} deleteRecovery={deleteRecovery} saveReload={flag4} packedFramesProtected={flag5} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			if (XWEditorInterface.Instance?.GetResourceEditor("animation_editor") is XWAnimationVisualResourceEditor xWAnimationVisualResourceEditor && GodotObject.IsInstanceValid(xWAnimationVisualResourceEditor))
			{
				_editor = xWAnimationVisualResourceEditor;
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> EnterEditorSurface(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			Control control = XWEditorInterface.Instance?.GetEditorPanel();
			Node instance = control?.FindChild("LoadingOverlay", recursive: true, owned: false);
			Control control2 = control?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control;
			if (GodotObject.IsInstanceValid(control) && !GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(control2))
			{
				control2.Hide();
				XWEditorInterface.Instance.FocusPanel("animation_editor");
				await WaitFrames(3);
				return _editor.IsVisibleInTree();
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<AdobeAnimateInspectorPreview> WaitForPreview(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			AdobeAnimateInspectorPreview adobeAnimateInspectorPreview = _editor?.FindChild("RuntimeInspectorPreview", recursive: true, owned: false) as AdobeAnimateInspectorPreview;
			AdobeAnimateSprite instance = adobeAnimateInspectorPreview?.FindChild("InspectorAnimationSprite", recursive: true, owned: false) as AdobeAnimateSprite;
			if (GodotObject.IsInstanceValid(adobeAnimateInspectorPreview) && GodotObject.IsInstanceValid(instance))
			{
				return adobeAnimateInspectorPreview;
			}
			await WaitFrames(1);
		}
		return null;
	}

	private static Button FindButtonByText(Node root, string text)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		if (root is Button button && button.Text.Contains(text, StringComparison.Ordinal))
		{
			return button;
		}
		foreach (Node child in root.GetChildren())
		{
			Button button2 = FindButtonByText(child, text);
			if (GodotObject.IsInstanceValid(button2))
			{
				return button2;
			}
		}
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

	private static T[] CloneArray<T>(T[] source)
	{
		if (source != null)
		{
			return (T[])source.Clone();
		}
		return Array.Empty<T>();
	}

	private static bool ArraysEqual<T>(T[] expected, T[] actual)
	{
		if (expected == null || actual == null || expected.Length != actual.Length)
		{
			return false;
		}
		EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
		for (int i = 0; i < expected.Length; i++)
		{
			if (!equalityComparer.Equals(expected[i], actual[i]))
			{
				return false;
			}
		}
		return true;
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
			GD.PrintErr("[MOD_EDITOR_ANIMATION_CLIP_AUTHORING_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_ANIMATION_CLIP_AUTHORING_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindButtonByText, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.FindButtonByText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(FindButtonByText(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.FindButtonByText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(FindButtonByText(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.FindButtonByText)
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
			_editor = VariantUtils.ConvertTo<XWAnimationVisualResourceEditor>(in value);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XWAnimationVisualResourceEditor>();
		}
	}
}
