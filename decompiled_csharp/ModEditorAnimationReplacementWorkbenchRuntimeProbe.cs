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
using PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://Tests/ModEditorAnimationReplacementWorkbenchRuntimeProbe.cs")]
public class ModEditorAnimationReplacementWorkbenchRuntimeProbe : Node
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
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string FixturePath = "res://Asset/Anime/Splat/Spike/SpikeSplat.tres";

	private const string DraftPath = "user://mod_editor_animation_replacement_workbench_probe.tres";

	private const string TextureAPath = "res://Asset/Texture/GUI/Almanac/AlmanacCloseButton.png";

	private const string TextureBPath = "res://Asset/Texture/GUI/Almanac/AlmanacCloseButtonHighlight.png";

	private const string TextureCPath = "res://Asset/Texture/GUI/Almanac/AlmanacIndexButton.png";

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWAnimationVisualResourceEditor _editor;

	public override async void _Ready()
	{
		_ = 14;
		try
		{
			AdobeAnimateData adobeAnimateData = ResourceLoader.Load<AdobeAnimateData>("res://Asset/Anime/Splat/Spike/SpikeSplat.tres", null, ResourceLoader.CacheMode.Reuse);
			Require(GodotObject.IsInstanceValid(adobeAnimateData) && adobeAnimateData.HasPackedRuntimeData(), "Real animation fixture is unavailable.");
			if (!GodotObject.IsInstanceValid(adobeAnimateData) || !adobeAnimateData.HasPackedRuntimeData())
			{
				Finish();
				return;
			}
			AdobeAnimateData draft = adobeAnimateData.Duplicate(deep: true) as AdobeAnimateData;
			Require(GodotObject.IsInstanceValid(draft), "Could not duplicate the animation fixture.");
			if (!GodotObject.IsInstanceValid(draft))
			{
				Finish();
				return;
			}
			List<(int Id, string Name)> media = GetMedia(draft);
			Require(media.Count >= 2, "The animation fixture does not expose two visual media entries.");
			if (media.Count < 2)
			{
				Finish();
				return;
			}
			draft.ResourceName = "ModEditorAnimationReplacementProbe";
			draft.replaceSlotDictionary = new Dictionary { [media[0].Name] = "legacy_slot_payload" };
			ResourceLoader.Load<Texture2D>("res://Asset/Texture/GUI/Almanac/AlmanacCloseButton.png", null, ResourceLoader.CacheMode.Reuse);
			ResourceLoader.Load<Texture2D>("res://Asset/Texture/GUI/Almanac/AlmanacCloseButtonHighlight.png", null, ResourceLoader.CacheMode.Reuse);
			draft.extraMediaReplaceTexturePaths = new Array<string> { "res://Asset/Texture/GUI/Almanac/AlmanacCloseButton.png", "res://Asset/Texture/GUI/Almanac/AlmanacCloseButtonHighlight.png" };
			Error error = ResourceSaver.Save(draft, "user://mod_editor_animation_replacement_workbench_probe.tres", ResourceSaver.SaverFlags.None);
			Require(error == Error.Ok, $"Could not create isolated animation draft: {error}.");
			if (error != Error.Ok)
			{
				Finish();
				return;
			}
			draft = ResourceLoader.Load<AdobeAnimateData>("user://mod_editor_animation_replacement_workbench_probe.tres", "", ResourceLoader.CacheMode.Replace);
			Require(GodotObject.IsInstanceValid(draft), "Isolated animation draft did not reload.");
			if (!GodotObject.IsInstanceValid(draft))
			{
				Finish();
				return;
			}
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
			XWEditorInterface.Instance.EditResource(draft, XWResourceEditContext.ForRoot(draft, "user://mod_editor_animation_replacement_workbench_probe.tres", "animation_editor"));
			XWEditorInterface.Instance.FocusPanel("animation_editor");
			TabBar tabBar = await WaitForAnimationTabs(900);
			if (GodotObject.IsInstanceValid(tabBar))
			{
				tabBar.CurrentTab = 5;
				await WaitFrames(3);
			}
			bool flag3 = await WaitForWorkbench(900);
			Require(flag3, "Animation replacement workbench did not mount.");
			if (!flag3)
			{
				Finish();
				return;
			}
			Control control = _editor.FindChild("ReplacementLibraryPanel", recursive: true, owned: false) as Control;
			HFlowContainer hFlowContainer = _editor.FindChild("ReplacementSlotCards", recursive: true, owned: false) as HFlowContainer;
			HFlowContainer hFlowContainer2 = _editor.FindChild("ExtraTextureCards", recursive: true, owned: false) as HFlowContainer;
			Button saveButton = _editor.FindChild("ReplacementSaveButton", recursive: true, owned: false) as Button;
			Button instance = _editor.FindChild("ReplacementReloadButton", recursive: true, owned: false) as Button;
			VBoxContainer vBoxContainer = _editor.FindChild("EmbeddedInspectorHost", recursive: true, owned: false) as VBoxContainer;
			bool visualSurface = GodotObject.IsInstanceValid(control) && control.IsVisibleInTree() && GodotObject.IsInstanceValid(hFlowContainer) && GodotObject.IsInstanceValid(hFlowContainer2) && hFlowContainer.GetChildCount() == 1 && hFlowContainer2.GetChildCount() == 2 && hFlowContainer.GetChild(0) is XWAnimationReplacementCard && hFlowContainer2.GetChild(0) is XWAnimationReplacementCard && GodotObject.IsInstanceValid(saveButton) && GodotObject.IsInstanceValid(instance);
			bool inspectorHidden = GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() == 0;
			Require(visualSurface, "Replacement slot/texture cards are not directly visible on the scene-authored main surface.");
			Require(inspectorHidden, "Animation replacement workbench restored the raw embedded Inspector.");
			bool addedSlot = _editor.AddAnimationReplacementSlot(media[1].Name);
			await WaitFrames(2);
			bool legacyPreserved = draft.replaceSlotDictionary[media[0].Name].AsString() == "legacy_slot_payload";
			bool addPassed = ((addedSlot && draft.replaceSlotDictionary.Count == 2 && draft.replaceSlotDictionary.ContainsKey(media[1].Name)) & legacyPreserved) && _editor.AnimationReplacementSlotCardCount == 2;
			Require(addPassed, "Adding a visual media slot did not preserve the opaque legacy slot value.");
			XWUndoRedoManager undoRedo = XWEditorInterface.Instance.GetUndoRedoManager();
			bool undoAdd = undoRedo.Undo();
			await WaitFrames(2);
			bool undoPassed = undoAdd && draft.replaceSlotDictionary.Count == 1 && !draft.replaceSlotDictionary.ContainsKey(media[1].Name) && _editor.AnimationReplacementSlotCardCount == 1;
			bool redoAdd = undoRedo.Redo();
			await WaitFrames(2);
			bool redoPassed = redoAdd && draft.replaceSlotDictionary.Count == 2 && _editor.AnimationReplacementSlotCardCount == 2;
			Require(undoPassed & redoPassed, "Replacement slot mutation did not survive global Undo/Redo.");
			Texture2D texture = ResourceLoader.Load<Texture2D>("res://Asset/Texture/GUI/Almanac/AlmanacIndexButton.png", null, ResourceLoader.CacheMode.Reuse);
			bool replacedTexture = _editor.ReplaceAnimationExtraTexture(0, texture);
			await WaitFrames(2);
			bool removedTexture = _editor.RemoveAnimationExtraTexture(1);
			await WaitFrames(2);
			bool textureMutation = (replacedTexture & removedTexture) && draft.extraMediaReplaceTexturePaths.Count == 1 && draft.extraMediaReplaceTexturePaths[0] == "res://Asset/Texture/GUI/Almanac/AlmanacIndexButton.png" && _editor.AnimationExtraTextureCardCount == 1;
			bool undoTextureRemove = undoRedo.Undo();
			await WaitFrames(2);
			bool textureUndo = undoTextureRemove && draft.extraMediaReplaceTexturePaths.Count == 2 && _editor.AnimationExtraTextureCardCount == 2;
			Require(textureMutation & textureUndo, "Extra replacement texture cards did not mutate and recover through Undo.");
			saveButton.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(4);
			AdobeAnimateData adobeAnimateData2 = ResourceLoader.Load<AdobeAnimateData>("user://mod_editor_animation_replacement_workbench_probe.tres", "", ResourceLoader.CacheMode.Ignore);
			bool saveReload = GodotObject.IsInstanceValid(adobeAnimateData2) && adobeAnimateData2.replaceSlotDictionary.Count == 2 && adobeAnimateData2.replaceSlotDictionary[media[0].Name].AsString() == "legacy_slot_payload" && adobeAnimateData2.extraMediaReplaceTexturePaths.Count == 2 && adobeAnimateData2.extraMediaReplaceTexturePaths[0] == "res://Asset/Texture/GUI/Almanac/AlmanacIndexButton.png";
			Require(saveReload, "Saved replacement slot/texture data did not survive an uncached disk reload.");
			bool unsavedRemoval = _editor.RemoveAnimationReplacementSlot(media[1].Name);
			await WaitFrames(2);
			bool changedBeforeReload = unsavedRemoval && _editor.AnimationReplacementSlotCardCount == 1;
			_editor.ReloadAnimationReplacementConfiguration();
			await WaitFrames(4);
			bool flag4 = _editor.AnimationReplacementSlotCardCount == 2 && _editor.AnimationExtraTextureCardCount == 2;
			Require(changedBeforeReload & flag4, "Disk reload did not restore the last saved replacement configuration.");
			bool value = FindAncestorWindow(_editor) != null;
			GD.Print($"[MOD_EDITOR_ANIMATION_REPLACEMENT_PROBE] window={value} visualSurface={visualSurface} inspectorHidden={inspectorHidden} add={addPassed} legacyPreserved={legacyPreserved} undoRedo={undoPassed & redoPassed} textureMutation={textureMutation} textureUndo={textureUndo} saveReload={saveReload} reloadRestored={flag4} mutations={_editor.AnimationReplacementMutationCount} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private static List<(int Id, string Name)> GetMedia(AdobeAnimateData animation)
	{
		List<(int, string)> list = new List<(int, string)>();
		foreach (Variant key in animation.mediaDictionary.Keys)
		{
			list.Add((animation.mediaDictionary[key].AsInt32(), key.AsString()));
		}
		list.Sort(((int, string) left, (int, string) right) => left.Item1.CompareTo(right.Item1));
		return list;
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

	private async Task<bool> WaitForWorkbench(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			Control control = _editor?.FindChild("ReplacementLibraryPanel", recursive: true, owned: false) as Control;
			if (GodotObject.IsInstanceValid(control) && control.IsVisibleInTree())
			{
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<TabBar> WaitForAnimationTabs(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			TabBar tabBar = _editor?.FindChild("AnimationWorkbenchTabs", recursive: true, owned: false) as TabBar;
			if (GodotObject.IsInstanceValid(tabBar))
			{
				return tabBar;
			}
			await WaitFrames(1);
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
			GD.PrintErr("[MOD_EDITOR_ANIMATION_REPLACEMENT_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_ANIMATION_REPLACEMENT_PROBE_FAILURE] " + failure);
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
