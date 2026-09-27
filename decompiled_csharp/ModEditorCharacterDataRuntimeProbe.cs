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

[ScriptPath("res://Tests/ModEditorCharacterDataRuntimeProbe.cs")]
public class ModEditorCharacterDataRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindLineEditByText = "FindLineEditByText";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName SimulateTextSession = "SimulateTextSession";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _editor = "_editor";

		public static readonly StringName _history = "_history";

		public static readonly StringName _metadataUndoRedo = "_metadataUndoRedo";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWGenericVisualResourceEditor _editor;

	private XWUndoRedoManager _history;

	private bool _metadataUndoRedo;

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
			Require(GodotObject.IsInstanceValid(modEditorManager), "ModEditorManager scene could not be instantiated.");
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
			Require(flag, "F3 did not initialize the real CharacterData editor within 900 frames.");
			if (!flag)
			{
				Finish();
				return;
			}
			bool window = FindAncestorWindow(_editor) != null;
			Require(window, "CharacterData editor is not mounted under the F3 ModEditor window.");
			bool inspectorHidden = await ProbeInspectorAndMetadata();
			bool metadataUndoRedo = _metadataUndoRedo;
			(bool, bool) tuple = await ProbeStandaloneDirectEdit();
			bool standaloneDirect = tuple.Item1;
			bool continuousMerged = tuple.Item2;
			bool value = await ProbeConeArmorStages();
			GD.Print($"[MOD_EDITOR_CHARACTER_DATA_PROBE] window={window} inspectorHidden={inspectorHidden} metadataUndoRedo={metadataUndoRedo} standaloneDirect={standaloneDirect} conePreserved={value} continuousMerged={continuousMerged} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private async Task<bool> ProbeInspectorAndMetadata()
	{
		CharacterArmorData armorData = new CharacterArmorData
		{
			ResourceName = "CharacterDataMetadataBefore",
			ResourceLocalToScene = false
		};
		await OpenResource(armorData);
		VBoxContainer vBoxContainer = FindControl<VBoxContainer>("EmbeddedInspectorHost");
		PanelContainer panelContainer = _editor.FindChild("InspectorPanel", recursive: true, owned: false) as PanelContainer;
		bool inspectorHidden = GodotObject.IsInstanceValid(panelContainer) && !panelContainer.Visible && GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() == 0 && _editor.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null;
		Require(inspectorHidden, "CharacterData workspace exposed or instantiated an embedded Inspector.");
		LineEdit lineEdit = FindControl<LineEdit>("ResourceNameEdit");
		Require(GodotObject.IsInstanceValid(lineEdit), "ResourceNameEdit is missing from CharacterData visual metadata.");
		if (!GodotObject.IsInstanceValid(lineEdit))
		{
			return inspectorHidden;
		}
		_history.ClearHistory();
		string before = armorData.ResourceName;
		SimulateTextSession(lineEdit, "CharacterDataMetadataAfter");
		await WaitFrames(2);
		bool applied = armorData.ResourceName == "CharacterDataMetadataAfter" && _history.HasUndo();
		bool undone = _history.Undo();
		await WaitFrames(2);
		undone = undone && armorData.ResourceName == before;
		bool redone = _history.Redo();
		await WaitFrames(2);
		redone = redone && armorData.ResourceName == "CharacterDataMetadataAfter";
		GD.Print($"[MOD_EDITOR_CHARACTER_DATA_PROBE_DETAIL] metadataApplied={applied} metadataUndone={undone} metadataRedone={redone} metadataValue={armorData.ResourceName} historyVersion={_history.GetVersion()} hasUndo={_history.HasUndo()} hasRedo={_history.HasRedo()}");
		_metadataUndoRedo = applied & undone & redone;
		Require(_metadataUndoRedo, "CharacterData resource_name did not round-trip through global undo/redo.");
		return inspectorHidden;
	}

	private async Task<(bool standaloneDirect, bool continuousMerged)> ProbeStandaloneDirectEdit()
	{
		CharacterCustomConfig custom = new CharacterCustomConfig
		{
			ResourceName = "StandaloneCustomResource",
			customName = "probe-custom-before"
		};
		await OpenResource(custom);
		VBoxContainer vBoxContainer = FindControl<VBoxContainer>("StandaloneEditorHost");
		LineEdit customName = FindLineEditByText("probe-custom-before");
		bool directSurface = GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() > 0 && GodotObject.IsInstanceValid(customName);
		Require(directSurface, "Standalone CharacterCustomConfig has no populated direct-edit surface.");
		if (!directSurface)
		{
			return (standaloneDirect: false, continuousMerged: false);
		}
		_history.ClearHistory();
		int beforeVersion = _history.GetVersion();
		customName.EmitSignal(Control.SignalName.FocusEntered);
		string[] array = new string[3] { "probe-custom-a", "probe-custom-ab", "probe-custom-final" };
		for (int i = 0; i < array.Length; i++)
		{
			string text = (customName.Text = array[i]);
			customName.EmitSignal(LineEdit.SignalName.TextChanged, text);
		}
		customName.EmitSignal(LineEdit.SignalName.TextSubmitted, "probe-custom-final");
		customName.EmitSignal(Control.SignalName.FocusExited);
		await WaitFrames(3);
		bool flag = GodotObject.IsInstanceValid(customName) && customName == FindLineEditByText("probe-custom-final");
		bool flag2 = _history.GetVersion() - beforeVersion == 1 && _history.HasUndo();
		bool continuousMerged = (custom.customName == "probe-custom-final") & flag2 & flag;
		GD.Print($"[MOD_EDITOR_CHARACTER_DATA_PROBE_DETAIL] customValue={custom.customName} sameControl={flag} oneHistoryAction={flag2} versionDelta={_history.GetVersion() - beforeVersion} hasUndo={_history.HasUndo()}");
		Require(continuousMerged, "Continuous CharacterCustomConfig input rebuilt the editor or produced more than one history action.");
		bool undo = _history.Undo();
		await WaitFrames(2);
		undo = undo && custom.customName == "probe-custom-before";
		bool redo = _history.Redo();
		await WaitFrames(2);
		redo = redo && custom.customName == "probe-custom-final";
		GD.Print($"[MOD_EDITOR_CHARACTER_DATA_PROBE_DETAIL] customUndo={undo} customRedo={redo} customFinal={custom.customName} hasUndo={_history.HasUndo()} hasRedo={_history.HasRedo()}");
		bool flag3 = directSurface & undo & redo;
		Require(flag3, "Standalone CharacterCustomConfig direct edit did not undo/redo.");
		return (standaloneDirect: flag3, continuousMerged: continuousMerged);
	}

	private async Task<bool> ProbeConeArmorStages()
	{
		TowerDefenseArmorTypeData cone = new TowerDefenseArmorTypeData
		{
			ResourceName = "ConeProbe",
			armorName = "Cone",
			stagePersontage = new Array<double> { 0.72, 0.33 },
			stageAnimeTexturePaths = new Array<string> { "res://probe/cone0.png", "res://probe/cone1.png", "res://probe/cone2.png" }
		};
		await OpenResource(cone);
		await WaitFrames(3);
		VBoxContainer vBoxContainer = FindControl<VBoxContainer>("StandaloneEditorHost");
		bool flag = GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() > 0 && cone.stagePersontage.Count == 2 && Math.Abs(cone.stagePersontage[0] - 0.72) < 0.0001 && Math.Abs(cone.stagePersontage[1] - 0.33) < 0.0001 && cone.stageAnimeTexturePaths.Count == 3 && cone.stageAnimeTexturePaths[0] == "res://probe/cone0.png" && cone.stageAnimeTexturePaths[1] == "res://probe/cone1.png" && cone.stageAnimeTexturePaths[2] == "res://probe/cone2.png";
		Require(flag, "Opening Cone armor changed its two thresholds or three stage textures.");
		return flag;
	}

	private async Task OpenResource(Resource resource)
	{
		XWEditorInterface.Instance.EditResource(resource);
		await WaitFrames(4);
		Control resourceEditor = XWEditorInterface.Instance.GetResourceEditor("character_data_editor");
		Require(resourceEditor == _editor, "CharacterData routing switched away from the registered editor.");
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			XWEditorInterface instance = XWEditorInterface.Instance;
			if (instance != null && instance.GetResourceEditor("character_data_editor") is XWGenericVisualResourceEditor xWGenericVisualResourceEditor && GodotObject.IsInstanceValid(xWGenericVisualResourceEditor))
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

	private LineEdit FindLineEditByText(string text)
	{
		if (!GodotObject.IsInstanceValid(_editor))
		{
			return null;
		}
		foreach (Node item in _editor.FindChildren("*", "LineEdit", recursive: true, owned: false))
		{
			if (item is LineEdit lineEdit && lineEdit.Text == text)
			{
				return lineEdit;
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

	private static void SimulateTextSession(LineEdit control, string value)
	{
		control.EmitSignal(Control.SignalName.FocusEntered);
		control.Text = value;
		control.EmitSignal(LineEdit.SignalName.TextChanged, value);
		control.EmitSignal(LineEdit.SignalName.TextSubmitted, value);
		control.EmitSignal(Control.SignalName.FocusExited);
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
			GD.PrintErr("[MOD_EDITOR_CHARACTER_DATA_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		if (_failures.Count > 0)
		{
			foreach (string failure in _failures)
			{
				GD.PrintErr("[MOD_EDITOR_CHARACTER_DATA_PROBE_FAILURE] " + failure);
			}
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindLineEditByText, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("LineEdit"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindAncestorWindow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Window"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SimulateTextSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("LineEdit"), exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.FindLineEditByText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<LineEdit>(FindLineEditByText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SimulateTextSession && args.Count == 2)
		{
			SimulateTextSession(VariantUtils.ConvertTo<LineEdit>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
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
		if (method == MethodName.SimulateTextSession && args.Count == 2)
		{
			SimulateTextSession(VariantUtils.ConvertTo<LineEdit>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.FindLineEditByText)
		{
			return true;
		}
		if (method == MethodName.FindAncestorWindow)
		{
			return true;
		}
		if (method == MethodName.SimulateTextSession)
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
		if (name == PropertyName._metadataUndoRedo)
		{
			_metadataUndoRedo = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._metadataUndoRedo)
		{
			value = VariantUtils.CreateFrom(in _metadataUndoRedo);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._metadataUndoRedo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._history, Variant.From(in _history));
		info.AddProperty(PropertyName._metadataUndoRedo, Variant.From(in _metadataUndoRedo));
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
		if (info.TryGetProperty(PropertyName._metadataUndoRedo, out var value3))
		{
			_metadataUndoRedo = value3.As<bool>();
		}
	}
}
