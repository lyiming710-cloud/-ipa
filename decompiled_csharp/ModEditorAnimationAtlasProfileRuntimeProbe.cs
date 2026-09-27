using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.ResourceEditors;
using PVZHE.ModEditor.ResourceEditors.GUI;
using PVZHE.ModEditor.Tools;

[ScriptPath("res://Tests/ModEditorAnimationAtlasProfileRuntimeProbe.cs")]
public class ModEditorAnimationAtlasProfileRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadProfile = "LoadProfile";

		public static readonly StringName FindPropertyEditor = "FindPropertyEditor";

		public static readonly StringName InspectorIsHidden = "InspectorIsHidden";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _projectRoot = "_projectRoot";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private string _projectRoot = "";

	public override async void _Ready()
	{
		bool action = false;
		bool undoRedo = false;
		bool hiddenStopped = false;
		bool resumed = false;
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
			bool f3 = await WaitForInterface(900);
			Require(f3, "F3 did not initialize the real ModEditor.");
			if (!f3)
			{
				Finish();
				return;
			}
			_projectRoot = ProjectSettings.GlobalizePath($"user://AnimationAtlasProfileProbe-{Guid.NewGuid():N}");
			XWModProjectLayout.EnsureProjectLayout(_projectRoot);
			string directoryPath = Path.Combine(_projectRoot, "Resources", "AnimationAtlasProfiles");
			foreach (XWResourceCreateRoute.CreateAction item in XWResourceCreateRoute.GetActionsForDirectory(directoryPath, _projectRoot))
			{
				action |= item.Id == "new-animation-atlas-profile";
			}
			Require(action, "Animation atlas profile directory has no strong create action.");
			XWTemplateLibrary.TemplateCreateResult templateCreateResult = XWResourceCreateRoute.CreateFromAction("new-animation-atlas-profile", directoryPath, "ProbeAtlas");
			string resourcePath = (templateCreateResult.Success ? ProjectSettings.LocalizePath(templateCreateResult.CreatedPath).Replace('\\', '/') : "");
			AdobeAnimateAtlasProfile profile = LoadProfile(resourcePath);
			bool created = templateCreateResult.Success && GodotObject.IsInstanceValid(profile) && profile.ProfileId == "ProbeAtlas" && profile.ManifestPath == "" && !profile.StartupOnly;
			Require(created, $"Animation atlas profile template failed: success={templateCreateResult.Success}; error={templateCreateResult.Error}");
			bool route = GodotObject.IsInstanceValid(profile) && XWResourceEditorRegistry.TryGetEditor(profile, resourcePath, out var descriptor) && descriptor.Category == "AnimationAtlas" && descriptor.DockKey == "animation_atlas_editor";
			Require(route, "Animation atlas profile did not route to animation_atlas_editor.");
			Node sentinel = new Node
			{
				Name = "AtlasProfileInspectorSentinel"
			};
			AddChild(sentinel, forceReadableName: false, InternalMode.Disabled);
			XWEditorInterface.Instance.InspectObject(sentinel);
			XWEditorInterface.Instance.EditResource(profile);
			XWEditorInterface.Instance.FocusPanel("animation_atlas_editor");
			XWGenericVisualResourceEditor editor = await WaitForEditor("animation_atlas_editor", 240);
			(XWEditorInterface.Instance.GetEditorPanel()?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control)?.Hide();
			XWEditorInterface.Instance.FocusPanel("animation_atlas_editor");
			await WaitFrames(8);
			XWDirectPropertySurface surface = editor?.FindChild("DirectPropertySurface", recursive: true, owned: false) as XWDirectPropertySurface;
			GD.Print($"[MOD_EDITOR_ANIMATION_ATLAS_PROFILE_DIAGNOSTIC] editor={editor?.GetType().Name ?? "null"} active={editor?.ActiveResource?.GetType().Name ?? "null"} editable={editor?.DirectEditablePropertyCount ?? (-1)} mounted={editor?.DirectMountedPropertyCount ?? (-1)} missing={editor?.DirectMissingPropertyCount ?? (-1)} names={string.Join(",", surface?.EditablePropertyNames ?? Array.Empty<string>())} visible={editor?.IsVisibleInTree() ?? false}");
			XWInlineTextSurface instance = editor?.FindChild("InlineTextSurface", recursive: true, owned: false) as XWInlineTextSurface;
			bool direct = GodotObject.IsInstanceValid(editor) && GodotObject.IsInstanceValid(surface) && GodotObject.IsInstanceValid(instance) && editor.DirectEditablePropertyCount >= 3 && editor.DirectMissingPropertyCount == 0 && editor.InlineTechnicalTextCount >= 2 && surface.EditablePropertyNames.Contains("ProfileId") && surface.EditablePropertyNames.Contains("ManifestPath") && surface.EditablePropertyNames.Contains("StartupOnly") && InspectorIsHidden(editor);
			Require(direct, "Animation atlas profile did not expose all properties on the main visual surface.");
			XWUndoRedoManager history = XWEditorInterface.Instance.GetUndoRedoManager();
			LineEdit lineEdit = editor?.FindChild("Inline_ProfileId", recursive: true, owned: false) as LineEdit;
			Require(GodotObject.IsInstanceValid(lineEdit), "ProfileId direct control was not found.");
			if (GodotObject.IsInstanceValid(lineEdit) && GodotObject.IsInstanceValid(history))
			{
				history.ClearHistory();
				lineEdit.EmitSignal(Control.SignalName.FocusEntered);
				lineEdit.Text = "ProbeAtlasEdited";
				lineEdit.EmitSignal(LineEdit.SignalName.TextSubmitted, "ProbeAtlasEdited");
				await WaitFrames(4);
				bool applied = profile.ProfileId == "ProbeAtlasEdited" && history.HasUndo();
				bool undone = history.Undo();
				await WaitFrames(4);
				undone &= profile.ProfileId == "ProbeAtlas";
				bool redone = history.Redo();
				await WaitFrames(4);
				redone &= profile.ProfileId == "ProbeAtlasEdited";
				undoRedo = applied & undone & redone;
				GD.Print($"[MOD_EDITOR_ANIMATION_ATLAS_PROFILE_EDIT_DIAGNOSTIC] applied={applied} undone={undone} redone={redone} value={profile.ProfileId}");
				Require(undoRedo, "ProfileId edit did not round-trip through shared Undo/Redo.");
			}
			CheckBox checkBox = FindFirst<CheckBox>(FindPropertyEditor(surface, "StartupOnly"));
			Require(GodotObject.IsInstanceValid(checkBox), "StartupOnly direct toggle was not found.");
			if (GodotObject.IsInstanceValid(checkBox))
			{
				checkBox.ButtonPressed = true;
				await WaitFrames(4);
			}
			bool saved = GodotObject.IsInstanceValid(editor) && editor.SaveActiveResource();
			await WaitFrames(8);
			AdobeAnimateAtlasProfile adobeAnimateAtlasProfile = LoadProfile(resourcePath);
			bool saveReload = saved && GodotObject.IsInstanceValid(adobeAnimateAtlasProfile) && adobeAnimateAtlasProfile.ProfileId == "ProbeAtlasEdited" && adobeAnimateAtlasProfile.StartupOnly;
			Require(saveReload, "Atlas profile direct edits did not survive Save/cache-ignore reload.");
			if (GodotObject.IsInstanceValid(editor))
			{
				editor.Hide();
				await WaitFrames(3);
				hiddenStopped = editor.ProcessMode == ProcessModeEnum.Disabled && !editor.IsProcessing();
				editor.Show();
				XWEditorInterface.Instance.FocusPanel("animation_atlas_editor");
				await WaitFrames(3);
				resumed = editor.ProcessMode != ProcessModeEnum.Disabled;
			}
			Require(hiddenStopped, "Hidden animation atlas editor still processes.");
			Require(resumed, "Animation atlas editor did not resume after becoming visible.");
			ulong before = Time.GetTicksMsec();
			await WaitFrames(6);
			bool flag = Time.GetTicksMsec() - before < 1000;
			Require(flag, "Animation atlas authoring blocked the main thread.");
			XWInspector xWInspector = XWEditorInterface.Instance.GetInspector() as XWInspector;
			bool flag2 = xWInspector == null || xWInspector.CurrentObject == sentinel;
			GD.Print($"[MOD_EDITOR_ANIMATION_ATLAS_PROFILE_INSPECTOR_DIAGNOSTIC] current={xWInspector?.CurrentObject?.GetType().Name ?? "null"} sentinelValid={GodotObject.IsInstanceValid(sentinel)}");
			Require(flag2, "Animation atlas editor replaced the global Inspector object.");
			GD.Print($"[MOD_EDITOR_ANIMATION_ATLAS_PROFILE_PROBE] f3={f3} action={action} created={created} route={route} direct={direct} undoRedo={undoRedo} saveReload={saveReload} hiddenStopped={hiddenStopped} resumed={resumed} responsive={flag} inspectorUntouched={flag2} failures={_failures.Count}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private static AdobeAnimateAtlasProfile LoadProfile(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return ResourceLoader.Load<AdobeAnimateAtlasProfile>(path, "", ResourceLoader.CacheMode.Ignore);
		}
		return null;
	}

	private static async Task<bool> WaitForInterface(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (GodotObject.IsInstanceValid(XWEditorInterface.Instance?.GetEditorPanel()) && GodotObject.IsInstanceValid(XWEditorInterface.Instance?.GetInspector()))
			{
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private static async Task<XWGenericVisualResourceEditor> WaitForEditor(string dockKey, int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (XWEditorInterface.Instance?.GetResourceEditor(dockKey) is XWGenericVisualResourceEditor xWGenericVisualResourceEditor && GodotObject.IsInstanceValid(xWGenericVisualResourceEditor.FindChild("DirectPropertySurface", recursive: true, owned: false)))
			{
				return xWGenericVisualResourceEditor;
			}
			await WaitFrames(1);
		}
		return null;
	}

	private static XWInspectorPropertyEditorBase FindPropertyEditor(Node root, string propertyName)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		if (root is XWInspectorPropertyEditorBase { Property: var property } xWInspectorPropertyEditorBase && property?.PropName.ToString() == propertyName)
		{
			return xWInspectorPropertyEditorBase;
		}
		foreach (Node child in root.GetChildren())
		{
			XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase2 = FindPropertyEditor(child, propertyName);
			if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase2))
			{
				return xWInspectorPropertyEditorBase2;
			}
		}
		return null;
	}

	private static T FindFirst<T>(Node root) where T : Node
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		foreach (Node item in root.FindChildren("*", typeof(T).Name, recursive: true, owned: false))
		{
			if (item is T result)
			{
				return result;
			}
		}
		return null;
	}

	private static bool InspectorIsHidden(Control editor)
	{
		Control control = editor?.FindChild("InspectorPanel", recursive: true, owned: false) as Control;
		Control control2 = editor?.FindChild("EmbeddedInspectorHost", recursive: true, owned: false) as Control;
		if (!GodotObject.IsInstanceValid(control) || !control.IsVisibleInTree())
		{
			if (GodotObject.IsInstanceValid(control2))
			{
				return !control2.IsVisibleInTree();
			}
			return true;
		}
		return false;
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
		}
	}

	private static async Task WaitFrames(int frames)
	{
		SceneTree tree = Engine.GetMainLoop() as SceneTree;
		for (int index = 0; index < frames; index++)
		{
			await tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PushError(failure);
		}
		if (!string.IsNullOrWhiteSpace(_projectRoot) && Directory.Exists(_projectRoot))
		{
			Directory.Delete(_projectRoot, recursive: true);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadProfile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindPropertyEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InspectorIsHidden, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
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
		if (method == MethodName.LoadProfile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateAtlasProfile>(LoadProfile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindPropertyEditor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(FindPropertyEditor(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.InspectorIsHidden && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(InspectorIsHidden(VariantUtils.ConvertTo<Control>(in args[0])));
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
		if (method == MethodName.LoadProfile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateAtlasProfile>(LoadProfile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindPropertyEditor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(FindPropertyEditor(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.InspectorIsHidden && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(InspectorIsHidden(VariantUtils.ConvertTo<Control>(in args[0])));
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
		if (method == MethodName.LoadProfile)
		{
			return true;
		}
		if (method == MethodName.FindPropertyEditor)
		{
			return true;
		}
		if (method == MethodName.InspectorIsHidden)
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
		if (name == PropertyName._projectRoot)
		{
			_projectRoot = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._projectRoot)
		{
			value = VariantUtils.CreateFrom(in _projectRoot);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._projectRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._projectRoot, Variant.From(in _projectRoot));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._projectRoot, out var value))
		{
			_projectRoot = value.As<string>();
		}
	}
}
