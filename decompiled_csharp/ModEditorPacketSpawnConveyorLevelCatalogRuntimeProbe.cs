using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.ResourceEditors;
using PVZHE.ModEditor.ResourceEditors.GUI;
using PVZHE.ModEditor.Tools;

[ScriptPath("res://Tests/ModEditorPacketSpawnConveyorLevelCatalogRuntimeProbe.cs")]
public class ModEditorPacketSpawnConveyorLevelCatalogRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName HasActions = "HasActions";

		public static readonly StringName CreateResources = "CreateResources";

		public static readonly StringName ValidateRoutes = "ValidateRoutes";

		public static readonly StringName RoutesTo = "RoutesTo";

		public static readonly StringName EditText = "EditText";

		public static readonly StringName InspectorIsHidden = "InspectorIsHidden";

		public static readonly StringName FindButtonByText = "FindButtonByText";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _history = "_history";

		public static readonly StringName _packetEditor = "_packetEditor";

		public static readonly StringName _conveyorEditor = "_conveyorEditor";

		public static readonly StringName _levelEditor = "_levelEditor";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly System.Collections.Generic.Dictionary<string, string> _paths = new System.Collections.Generic.Dictionary<string, string>(StringComparer.Ordinal);

	private XWUndoRedoManager _history;

	private XWPacketSpawnEntryVisualResourceEditor _packetEditor;

	private XWConveyorEventVisualResourceEditor _conveyorEditor;

	private XWLevelCatalogVisualResourceEditor _levelEditor;

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
			bool f3 = await WaitForEditors(900);
			Require(f3, "F3 did not initialize PacketSpawnEntry/ConveyorEvent/LevelCatalog editors.");
			if (!f3)
			{
				Finish();
				return;
			}
			string text = ProjectSettings.GlobalizePath("user://PacketConveyorLevelCatalogProbe");
			if (Directory.Exists(text))
			{
				Directory.Delete(text, recursive: true);
			}
			XWModProjectLayout.EnsureProjectLayout(text);
			bool actions = HasActions(text);
			bool created = CreateResources(text);
			bool routes = ValidateRoutes();
			Require(actions, "One or more standard directories lack their strong create action.");
			Require(created, "One or more strong templates failed concrete type/default validation.");
			Require(routes, "One or more resources did not route to its dedicated editor.");
			Node sentinel = new Node
			{
				Name = "LongTailInspectorSentinel"
			};
			AddChild(sentinel, forceReadableName: false, InternalMode.Disabled);
			XWEditorInterface.Instance.InspectObject(sentinel);
			bool packetDirect = await ProbePacketSurface();
			(bool, bool, bool) tuple = await ProbeConveyorSurface();
			bool conveyorDirect = tuple.Item1;
			bool conveyorUndoRedo = tuple.Item2;
			bool conveyorSave = tuple.Item3;
			(bool, bool, bool, bool) tuple2 = await ProbeLevelSurface();
			bool item = tuple2.Item1;
			bool item2 = tuple2.Item2;
			bool item3 = tuple2.Item3;
			bool item4 = tuple2.Item4;
			bool flag = !(XWEditorInterface.Instance.GetInspector() is XWInspector xWInspector) || xWInspector.CurrentObject == sentinel;
			Require(flag, "Dedicated editors replaced the global Inspector object.");
			GD.Print($"[MOD_EDITOR_PACKET_CONVEYOR_LEVEL_CATALOG_PROBE] f3={f3} actions={actions} created={created} routes={routes} packetDirect={packetDirect} conveyorDirect={conveyorDirect} conveyorUndoRedo={conveyorUndoRedo} conveyorSave={conveyorSave} levelDirect={item} levelUndoRedo={item2} levelSave={item3} levelRuntime={item4} inspectorUntouched={flag} failures={_failures.Count}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private bool HasActions(string root)
	{
		(string, string)[] array = new (string, string)[5]
		{
			("Resources/PacketSpawnEntries/Level", "new-level-packet-entry"),
			("Resources/PacketSpawnEntries/Conveyor", "new-conveyor-packet-entry"),
			("Resources/PacketSpawnEntries/Rain", "new-rain-packet-entry"),
			("Resources/ConveyorEvents", "new-conveyor-add-packet-event"),
			("Resources/LevelCatalogs", "new-level-catalog")
		};
		for (int i = 0; i < array.Length; i++)
		{
			(string, string) tuple = array[i];
			string item = tuple.Item1;
			string item2 = tuple.Item2;
			string directoryPath = Path.Combine(root, item.Replace('/', Path.DirectorySeparatorChar));
			bool flag = false;
			foreach (XWResourceCreateRoute.CreateAction item3 in XWResourceCreateRoute.GetActionsForDirectory(directoryPath, root))
			{
				flag |= item3.Id == item2;
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	private bool CreateResources(string root)
	{
		(string, string, string, Type)[] array = new (string, string, string, Type)[5]
		{
			("new-level-packet-entry", "Resources/PacketSpawnEntries/Level", "ProbeLevelPacket", typeof(TowerDefenseLevelPacketConfig)),
			("new-conveyor-packet-entry", "Resources/PacketSpawnEntries/Conveyor", "ProbeConveyorPacket", typeof(TowerDefenseConveyorPacketConfig)),
			("new-rain-packet-entry", "Resources/PacketSpawnEntries/Rain", "ProbeRainPacket", typeof(TowerDefenseRainModePacketConfig)),
			("new-conveyor-add-packet-event", "Resources/ConveyorEvents", "ProbeConveyorEvent", typeof(TowerDefenseConveyorEventAddPacket)),
			("new-level-catalog", "Resources/LevelCatalogs", "AdventureProbe", typeof(LevelCatalogConfig))
		};
		bool flag = true;
		(string, string, string, Type)[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			(string, string, string, Type) tuple = array2[i];
			string item = tuple.Item1;
			string item2 = tuple.Item2;
			string item3 = tuple.Item3;
			Type item4 = tuple.Item4;
			string directoryPath = Path.Combine(root, item2.Replace('/', Path.DirectorySeparatorChar));
			XWTemplateLibrary.TemplateCreateResult templateCreateResult = XWResourceCreateRoute.CreateFromAction(item, directoryPath, item3);
			string text = LocalPath(templateCreateResult);
			_paths[item] = text;
			Resource resource = Load<Resource>(text);
			bool flag2 = templateCreateResult.Success && GodotObject.IsInstanceValid(resource) && resource.GetType() == item4;
			flag &= flag2;
			Require(flag2, $"Template failed: {item}; success={templateCreateResult.Success}; error={templateCreateResult.Error}; type={resource?.GetType().Name}");
		}
		TowerDefenseConveyorEventAddPacket towerDefenseConveyorEventAddPacket = Load<TowerDefenseConveyorEventAddPacket>(_paths["new-conveyor-add-packet-event"]);
		LevelCatalogConfig levelCatalogConfig = Load<LevelCatalogConfig>(_paths["new-level-catalog"]);
		flag &= GodotObject.IsInstanceValid(towerDefenseConveyorEventAddPacket?.packet) && towerDefenseConveyorEventAddPacket.packet.weight == 10;
		return flag & (GodotObject.IsInstanceValid(levelCatalogConfig) && levelCatalogConfig.catalogKey == "AdventureProbe" && levelCatalogConfig.chapterList.Count == 1 && levelCatalogConfig.chapterList[0].levelList.Count == 1);
	}

	private bool ValidateRoutes()
	{
		if (RoutesTo(Load<Resource>(_paths["new-level-packet-entry"]), _paths["new-level-packet-entry"], "PacketSpawnEntry", "packet_spawn_entry_editor") && RoutesTo(Load<Resource>(_paths["new-conveyor-packet-entry"]), _paths["new-conveyor-packet-entry"], "PacketSpawnEntry", "packet_spawn_entry_editor") && RoutesTo(Load<Resource>(_paths["new-rain-packet-entry"]), _paths["new-rain-packet-entry"], "PacketSpawnEntry", "packet_spawn_entry_editor") && RoutesTo(Load<Resource>(_paths["new-conveyor-add-packet-event"]), _paths["new-conveyor-add-packet-event"], "ConveyorEvent", "conveyor_event_editor"))
		{
			return RoutesTo(Load<Resource>(_paths["new-level-catalog"]), _paths["new-level-catalog"], "LevelCatalog", "level_catalog_editor");
		}
		return false;
	}

	private async Task<bool> ProbePacketSurface()
	{
		Resource res = Load<Resource>(_paths["new-conveyor-packet-entry"]);
		XWEditorInterface.Instance.EditResource(res);
		XWEditorInterface.Instance.FocusPanel("packet_spawn_entry_editor");
		await WaitFrames(7);
		bool flag = GodotObject.IsInstanceValid(Find<LineEdit>(_packetEditor, "ResourceNameEdit")) && GodotObject.IsInstanceValid(Find<HSlider>(_packetEditor, "WeightSlider")) && InspectorIsHidden(_packetEditor);
		Require(flag, "PacketSpawnEntry did not expose its inspector-free direct workbench.");
		return flag;
	}

	private async Task<(bool Direct, bool UndoRedo, bool Save)> ProbeConveyorSurface()
	{
		string path = _paths["new-conveyor-add-packet-event"];
		TowerDefenseConveyorEventAddPacket conveyor = Load<TowerDefenseConveyorEventAddPacket>(path);
		XWEditorInterface.Instance.EditResource(conveyor);
		XWEditorInterface.Instance.FocusPanel("conveyor_event_editor");
		await WaitFrames(7);
		LineEdit lineEdit = Find<LineEdit>(_conveyorEditor, "ConveyorPacketNameEdit");
		SpinBox instance = Find<SpinBox>(_conveyorEditor, "ConveyorPacketWeightSpin");
		bool direct = GodotObject.IsInstanceValid(lineEdit) && GodotObject.IsInstanceValid(instance) && InspectorIsHidden(_conveyorEditor);
		Require(direct, "ConveyorEvent did not expose nested packet fields without Inspector.");
		if (!direct)
		{
			return (Direct: false, UndoRedo: false, Save: false);
		}
		_history.ClearHistory();
		EditText(lineEdit, "Sunflower");
		await WaitFrames(3);
		bool applied = conveyor.packet.name == "Sunflower" && _history.HasUndo();
		bool undone = _history.Undo();
		await WaitFrames(4);
		undone &= conveyor.packet.name == "NewPacket";
		bool redone = _history.Redo();
		await WaitFrames(4);
		redone &= conveyor.packet.name == "Sunflower";
		bool undoRedo = applied & undone & redone;
		Require(undoRedo, "ConveyorEvent direct nested edit did not round-trip through Undo/Redo.");
		Button button = FindButtonByText(_conveyorEditor, "保存");
		Require(GodotObject.IsInstanceValid(button), "ConveyorEvent toolbar has no Save button.");
		button?.EmitSignal(BaseButton.SignalName.Pressed);
		await WaitFrames(6);
		bool flag = Load<TowerDefenseConveyorEventAddPacket>(path)?.packet?.name == "Sunflower";
		Require(flag, "ConveyorEvent edit did not survive cache-ignore Save/Reload.");
		return (Direct: direct, UndoRedo: undoRedo, Save: flag);
	}

	private async Task<(bool Direct, bool UndoRedo, bool Save, bool Runtime)> ProbeLevelSurface()
	{
		string path = _paths["new-level-catalog"];
		LevelCatalogConfig catalog = Load<LevelCatalogConfig>(path);
		XWEditorInterface.Instance.EditResource(catalog);
		XWEditorInterface.Instance.FocusPanel("level_catalog_editor");
		await WaitFrames(8);
		LineEdit lineEdit = Find<LineEdit>(_levelEditor, "CatalogKeyEdit");
		LineEdit instance = Find<LineEdit>(_levelEditor, "ChapterNameEdit");
		LineEdit instance2 = Find<LineEdit>(_levelEditor, "LevelSaveKeyEdit");
		bool direct = GodotObject.IsInstanceValid(lineEdit) && GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(instance2) && InspectorIsHidden(_levelEditor);
		Require(direct, "LevelCatalog did not expose catalog/chapter/level fields without Inspector.");
		if (!direct)
		{
			return (Direct: false, UndoRedo: false, Save: false, Runtime: false);
		}
		_history.ClearHistory();
		EditText(lineEdit, "AdventureVisual");
		await WaitFrames(3);
		bool applied = catalog.catalogKey == "AdventureVisual" && _history.HasUndo();
		bool undone = _history.Undo();
		await WaitFrames(4);
		undone &= catalog.catalogKey == "AdventureProbe";
		bool redone = _history.Redo();
		await WaitFrames(4);
		redone &= catalog.catalogKey == "AdventureVisual";
		Button button = FindButtonByText(_levelEditor, "＋关卡");
		int before = catalog.chapterList[0].levelList.Count;
		button?.EmitSignal(BaseButton.SignalName.Pressed);
		await WaitFrames(4);
		bool arrayApplied = catalog.chapterList[0].levelList.Count == before + 1 && _history.HasUndo();
		bool arrayUndone = _history.Undo();
		await WaitFrames(4);
		arrayUndone &= catalog.chapterList[0].levelList.Count == before;
		bool arrayRedone = _history.Redo();
		await WaitFrames(4);
		arrayRedone &= catalog.chapterList[0].levelList.Count == before + 1;
		bool undoRedo = applied & undone & redone & arrayApplied & arrayUndone & arrayRedone;
		Require(undoRedo, "LevelCatalog scalar/typed-array edits did not round-trip through Undo/Redo.");
		Button button2 = FindButtonByText(_levelEditor, "保存");
		Require(GodotObject.IsInstanceValid(button2), "LevelCatalog toolbar has no Save button.");
		button2?.EmitSignal(BaseButton.SignalName.Pressed);
		await WaitFrames(7);
		LevelCatalogConfig levelCatalogConfig = Load<LevelCatalogConfig>(path);
		bool flag = levelCatalogConfig?.catalogKey == "AdventureVisual" && levelCatalogConfig.chapterList[0].levelList.Count == before + 1;
		Require(flag, "LevelCatalog edit did not survive cache-ignore Save/Reload.");
		Dictionary dictionary = levelCatalogConfig.ToRuntimeDictionary();
		bool flag2 = dictionary.ContainsKey("Chapter") && dictionary["Chapter"].AsGodotArray().Count == 1 && dictionary["Chapter"].AsGodotArray()[0].AsGodotDictionary().ContainsKey("Level") && ModLoader.InferRuntimeEntry("Resources/LevelCatalogs/AdventureVisual.tres", out var category, out var key) && category == "Level" && key == "AdventureVisual";
		Require(flag2, "LevelCatalog did not convert to the ResourceManager.LEVELS dictionary contract.");
		return (Direct: direct, UndoRedo: undoRedo, Save: flag, Runtime: flag2);
	}

	private async Task<bool> WaitForEditors(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			XWEditorInterface instance = XWEditorInterface.Instance;
			if (instance != null && instance.GetResourceEditor("packet_spawn_entry_editor") is XWPacketSpawnEntryVisualResourceEditor packetEditor && instance.GetResourceEditor("conveyor_event_editor") is XWConveyorEventVisualResourceEditor conveyorEditor && instance.GetResourceEditor("level_catalog_editor") is XWLevelCatalogVisualResourceEditor levelEditor)
			{
				_packetEditor = packetEditor;
				_conveyorEditor = conveyorEditor;
				_levelEditor = levelEditor;
				_history = instance.GetUndoRedoManager();
				(instance.GetEditorPanel()?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control)?.Hide();
				return GodotObject.IsInstanceValid(_history);
			}
			await WaitFrames(1);
		}
		XWEditorInterface instance2 = XWEditorInterface.Instance;
		string value = instance2?.GetResourceEditor("packet_spawn_entry_editor")?.GetType().Name ?? "null";
		string value2 = instance2?.GetResourceEditor("conveyor_event_editor")?.GetType().Name ?? "null";
		string value3 = instance2?.GetResourceEditor("level_catalog_editor")?.GetType().Name ?? "null";
		GD.Print($"[MOD_EDITOR_PACKET_CONVEYOR_LEVEL_CATALOG_PROBE_DIAGNOSTIC] interface={instance2 != null} packet={value} conveyor={value2} level={value3}");
		return false;
	}

	private static bool RoutesTo(Resource resource, string path, string category, string dock)
	{
		if (GodotObject.IsInstanceValid(resource) && XWResourceEditorRegistry.TryGetEditor(resource, path, out var descriptor) && descriptor.Category == category)
		{
			return descriptor.DockKey == dock;
		}
		return false;
	}

	private static void EditText(LineEdit edit, string text)
	{
		edit.EmitSignal(Control.SignalName.FocusEntered);
		edit.Text = text;
		edit.EmitSignal(LineEdit.SignalName.TextChanged, text);
		edit.EmitSignal(Control.SignalName.FocusExited);
	}

	private static T Load<T>(string path) where T : Resource
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return ResourceLoader.Load<T>(path, "", ResourceLoader.CacheMode.Ignore);
		}
		return null;
	}

	private static string LocalPath(XWTemplateLibrary.TemplateCreateResult result)
	{
		if (!result.Success || string.IsNullOrWhiteSpace(result.CreatedPath))
		{
			return "";
		}
		return ProjectSettings.LocalizePath(result.CreatedPath).Replace('\\', '/');
	}

	private static bool InspectorIsHidden(XWGenericVisualResourceEditor editor)
	{
		PanelContainer panelContainer = editor?.FindChild("InspectorPanel", recursive: true, owned: false) as PanelContainer;
		if (GodotObject.IsInstanceValid(panelContainer) && !panelContainer.Visible)
		{
			return editor.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null;
		}
		return false;
	}

	private static Button FindButtonByText(Node root, string text)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		foreach (Node item in root.FindChildren("*", "Button", recursive: true, owned: false))
		{
			if (item is Button button && button.Text == text)
			{
				return button;
			}
		}
		return null;
	}

	private static T Find<T>(Node root, string name) where T : Node
	{
		return root?.FindChild(name, recursive: true, owned: false) as T;
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
			GD.PrintErr("[MOD_EDITOR_PACKET_CONVEYOR_LEVEL_CATALOG_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_PACKET_CONVEYOR_LEVEL_CATALOG_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasActions, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "root", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateResources, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "root", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ValidateRoutes, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RoutesTo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EditText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "edit", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("LineEdit"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InspectorIsHidden, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindButtonByText, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.HasActions && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasActions(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateResources && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CreateResources(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ValidateRoutes && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ValidateRoutes());
			return true;
		}
		if (method == MethodName.RoutesTo && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(RoutesTo(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.EditText && args.Count == 2)
		{
			EditText(VariantUtils.ConvertTo<LineEdit>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InspectorIsHidden && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(InspectorIsHidden(VariantUtils.ConvertTo<XWGenericVisualResourceEditor>(in args[0])));
			return true;
		}
		if (method == MethodName.FindButtonByText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(FindButtonByText(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.RoutesTo && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(RoutesTo(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.EditText && args.Count == 2)
		{
			EditText(VariantUtils.ConvertTo<LineEdit>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InspectorIsHidden && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(InspectorIsHidden(VariantUtils.ConvertTo<XWGenericVisualResourceEditor>(in args[0])));
			return true;
		}
		if (method == MethodName.FindButtonByText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(FindButtonByText(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.HasActions)
		{
			return true;
		}
		if (method == MethodName.CreateResources)
		{
			return true;
		}
		if (method == MethodName.ValidateRoutes)
		{
			return true;
		}
		if (method == MethodName.RoutesTo)
		{
			return true;
		}
		if (method == MethodName.EditText)
		{
			return true;
		}
		if (method == MethodName.InspectorIsHidden)
		{
			return true;
		}
		if (method == MethodName.FindButtonByText)
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
		if (name == PropertyName._history)
		{
			_history = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName._packetEditor)
		{
			_packetEditor = VariantUtils.ConvertTo<XWPacketSpawnEntryVisualResourceEditor>(in value);
			return true;
		}
		if (name == PropertyName._conveyorEditor)
		{
			_conveyorEditor = VariantUtils.ConvertTo<XWConveyorEventVisualResourceEditor>(in value);
			return true;
		}
		if (name == PropertyName._levelEditor)
		{
			_levelEditor = VariantUtils.ConvertTo<XWLevelCatalogVisualResourceEditor>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._history)
		{
			value = VariantUtils.CreateFrom(in _history);
			return true;
		}
		if (name == PropertyName._packetEditor)
		{
			value = VariantUtils.CreateFrom(in _packetEditor);
			return true;
		}
		if (name == PropertyName._conveyorEditor)
		{
			value = VariantUtils.CreateFrom(in _conveyorEditor);
			return true;
		}
		if (name == PropertyName._levelEditor)
		{
			value = VariantUtils.CreateFrom(in _levelEditor);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._conveyorEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._history, Variant.From(in _history));
		info.AddProperty(PropertyName._packetEditor, Variant.From(in _packetEditor));
		info.AddProperty(PropertyName._conveyorEditor, Variant.From(in _conveyorEditor));
		info.AddProperty(PropertyName._levelEditor, Variant.From(in _levelEditor));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._history, out var value))
		{
			_history = value.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._packetEditor, out var value2))
		{
			_packetEditor = value2.As<XWPacketSpawnEntryVisualResourceEditor>();
		}
		if (info.TryGetProperty(PropertyName._conveyorEditor, out var value3))
		{
			_conveyorEditor = value3.As<XWConveyorEventVisualResourceEditor>();
		}
		if (info.TryGetProperty(PropertyName._levelEditor, out var value4))
		{
			_levelEditor = value4.As<XWLevelCatalogVisualResourceEditor>();
		}
	}
}
