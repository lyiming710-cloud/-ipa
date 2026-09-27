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
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;
using PVZHE.ModEditor.Tools;

[ScriptPath("res://Tests/ModEditorGameplayLogicResourceEntryRuntimeProbe.cs")]
public class ModEditorGameplayLogicResourceEntryRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName IsPlayableDefault = "IsPlayableDefault";

		public static readonly StringName InspectorIsHidden = "InspectorIsHidden";

		public static readonly StringName FindMenuItem = "FindMenuItem";

		public static readonly StringName FindTreeItemByPath = "FindTreeItemByPath";

		public static readonly StringName ExpandAncestors = "ExpandAncestors";

		public static readonly StringName Normalize = "Normalize";

		public static readonly StringName SamePath = "SamePath";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _probeRoot = "_probeRoot";

		public static readonly StringName _waveDirectory = "_waveDirectory";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private string _probeRoot = "";

	private string _waveDirectory = "";

	public override async void _Ready()
	{
		bool f3 = false;
		bool rightClick = false;
		bool toolbar = false;
		bool iconMenus = false;
		bool lazyOpen = false;
		bool managerCreated = false;
		bool managerOpened = false;
		bool managerPlayable = false;
		bool waveCreated = false;
		bool waveOpened = false;
		bool spawnCreated = false;
		bool spawnOpened = false;
		bool gridCreated = false;
		bool gridOpened = false;
		bool directVisual = false;
		bool undoRedo = false;
		bool saveReload = false;
		bool singleCreate = false;
		bool inspectorUntouched = false;
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
			f3 = await WaitForEditor(900);
			Require(f3, "F3 did not initialize the Mod editor and file-system panel.");
			if (!f3)
			{
				Finish();
				return;
			}
			XWFileSystemPanel panel = XWFileSystemPanel.Instance;
			(XWEditorInterface.Instance.GetEditorPanel()?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control)?.Hide();
			_probeRoot = ProjectSettings.GlobalizePath("user://ModEditorGameplayLogicResourceEntryProbe");
			if (Directory.Exists(_probeRoot))
			{
				Directory.Delete(_probeRoot, recursive: true);
			}
			XWModProjectLayout.EnsureProjectLayout(_probeRoot);
			panel.NavigateToProject(_probeRoot);
			await WaitFrames(10);
			Node inspectorSentinel = new Node
			{
				Name = "GameplayLogicEntryInspectorSentinel"
			};
			AddChild(inspectorSentinel, forceReadableName: false, InternalMode.Disabled);
			XWEditorInterface.Instance.InspectObject(inspectorSentinel);
			_waveDirectory = Normalize(Path.Combine(_probeRoot, "Resources", "GameplayLogic", "Waves"));
			string[] labels = new string[4] { "新建完整波次管理器", "新建波次模板", "新建线路生成项", "新建格子生成项" };
			XWFileSystemTree tree = panel.GetNode<XWFileSystemTree>("%FileTree");
			TreeItem directoryItem = FindTreeItemByPath(tree.GetRoot(), _waveDirectory);
			Require(directoryItem != null, "GameplayLogic/Waves directory is missing from the visual project tree.");
			if (directoryItem != null)
			{
				ExpandAncestors(directoryItem);
				tree.DeselectAll();
				directoryItem.Select(0);
				tree.ScrollToItem(directoryItem);
				await WaitFrames(2);
				Rect2 itemAreaRect = tree.GetItemAreaRect(directoryItem, 0);
				tree.EmitSignal(Tree.SignalName.ItemMouseSelected, itemAreaRect.Position + itemAreaRect.Size * 0.5f, 2L);
				await WaitFrames(2);
				PopupMenu node = panel.GetNode<PopupMenu>("%TreePopupMenu");
				rightClick = HasAllEntriesWithIcons(node, labels);
				Require(rightClick, "Wave directory right-click menu is missing one or more visual create entries/icons.");
				node.Hide();
			}
			panel.NavigateToPath(_waveDirectory + "/");
			await WaitFrames(4);
			MenuButton createButton = panel.GetNode<MenuButton>("%ButtonCreate");
			PopupMenu createMenu = createButton.GetPopup();
			createMenu.EmitSignal(Window.SignalName.AboutToPopup);
			await WaitFrames(2);
			toolbar = HasAllEntriesWithIcons(createMenu, labels);
			iconMenus = (rightClick & toolbar) && GodotObject.IsInstanceValid(createButton.Icon);
			Require(toolbar, "Top create menu does not expose all four wave resources.");
			Require(iconMenus, "Wave resource creation is still text-only in at least one menu.");
			lazyOpen = XWEditorInterface.Instance.TryGetLoadedResourceEditor("gameplay_logic_editor") == null;
			Require(lazyOpen, "GameplayLogic editor was eagerly created before a wave resource opened.");
			string managerPath = Normalize(Path.Combine(_waveDirectory, "EntryWaveManager.tres"));
			int num = FindMenuItem(createMenu, labels[0]);
			if (num >= 0)
			{
				createMenu.EmitSignal(PopupMenu.SignalName.IdPressed, createMenu.GetItemId(num));
				ConfirmationDialog confirmationDialog = await WaitForNameDialog(panel, 90);
				Require(GodotObject.IsInstanceValid(confirmationDialog), "Wave manager action did not open its visual name dialog.");
				if (GodotObject.IsInstanceValid(confirmationDialog))
				{
					LineEdit node2 = confirmationDialog.GetNode<LineEdit>("%NameEdit");
					node2.Text = "EntryWaveManager";
					node2.EmitSignal(LineEdit.SignalName.TextSubmitted, node2.Text);
					confirmationDialog.EmitSignal(AcceptDialog.SignalName.Confirmed);
				}
			}
			managerOpened = await WaitForActive<TowerDefenseLevelWaveManagerConfig>(managerPath, 300);
			managerCreated = File.Exists(managerPath);
			Require(managerCreated, "Toolbar creation did not write the WaveManager resource.");
			Require(managerOpened, "Created WaveManager did not automatically open in GameplayLogic.");
			TowerDefenseLevelWaveManagerConfig manager = Load<TowerDefenseLevelWaveManagerConfig>(managerPath);
			managerPlayable = IsPlayableDefault(manager);
			Require(managerPlayable, "WaveManager template is not a playable default graph.");
			XWTemplateLibrary.TemplateCreateResult templateCreateResult = XWResourceCreateRoute.CreateFromAction("new-wave", _waveDirectory, "EntryWave");
			string text = Normalize(templateCreateResult.CreatedPath);
			TowerDefenseLevelWaveConfig towerDefenseLevelWaveConfig = Load<TowerDefenseLevelWaveConfig>(text);
			waveCreated = templateCreateResult.Success && GodotObject.IsInstanceValid(towerDefenseLevelWaveConfig) && towerDefenseLevelWaveConfig.dynamicPlantfood.Count == 7 && towerDefenseLevelWaveConfig.spawn.Count == 1 && towerDefenseLevelWaveConfig.spawn[0].zombie == "ZombieNormal" && GodotObject.IsInstanceValid(towerDefenseLevelWaveConfig.dynamic);
			Require(waveCreated, "Typed Wave template failed: " + templateCreateResult.Error);
			if (waveCreated)
			{
				XWResourceEditorRegistry.TryOpen(towerDefenseLevelWaveConfig, text);
			}
			bool flag = waveCreated;
			if (flag)
			{
				flag = await WaitForActive<TowerDefenseLevelWaveConfig>(text, 180);
			}
			waveOpened = flag;
			Require(waveOpened, "Typed Wave did not route to GameplayLogic.");
			XWTemplateLibrary.TemplateCreateResult templateCreateResult2 = XWResourceCreateRoute.CreateFromAction("new-wave-spawn", _waveDirectory, "EntrySpawn");
			string spawnPath = Normalize(templateCreateResult2.CreatedPath);
			TowerDefenseLevelSpawnConfig spawn = Load<TowerDefenseLevelSpawnConfig>(spawnPath);
			spawnCreated = templateCreateResult2.Success && GodotObject.IsInstanceValid(spawn) && spawn.zombie == "ZombieNormal" && spawn.line == -1 && spawn.num == 1;
			Require(spawnCreated, "Typed lane Spawn template failed: " + templateCreateResult2.Error);
			if (spawnCreated)
			{
				XWResourceEditorRegistry.TryOpen(spawn, spawnPath);
			}
			flag = spawnCreated;
			if (flag)
			{
				flag = await WaitForActive<TowerDefenseLevelSpawnConfig>(spawnPath, 180);
			}
			spawnOpened = flag;
			Require(spawnOpened, "Typed lane Spawn did not route to GameplayLogic.");
			XWGameplayLogicVisualResourceEditor editor = XWEditorInterface.Instance.TryGetLoadedResourceEditor("gameplay_logic_editor") as XWGameplayLogicVisualResourceEditor;
			SpinBox spinBox = FindFirst<SpinBox>(editor?.FindChild("ShelfRoot", recursive: true, owned: false) as Control);
			directVisual = spawnOpened && GodotObject.IsInstanceValid(spinBox) && InspectorIsHidden(editor);
			Require(directVisual, "Lane Spawn did not expose direct visual controls or hid them behind an Inspector.");
			if (GodotObject.IsInstanceValid(spinBox))
			{
				LineEdit lineEdit = spinBox.GetLineEdit();
				lineEdit.EmitSignal(Control.SignalName.FocusEntered);
				spinBox.SetValueNoSignal(4.0);
				spinBox.EmitSignal(Godot.Range.SignalName.ValueChanged, 4.0);
				lineEdit.EmitSignal(Control.SignalName.FocusExited);
				await WaitFrames(3);
				bool flag2 = spawn.num == 4;
				XWUndoRedoManager undoRedoManager = XWEditorInterface.Instance.GetUndoRedoManager();
				bool flag3 = (undoRedoManager?.Undo() ?? false) && spawn.num == 1;
				bool flag4 = (undoRedoManager?.Redo() ?? false) && spawn.num == 4;
				undoRedo = flag2 & flag3 & flag4;
				Require(undoRedo, "Direct spawn-count control did not preserve Undo/Redo.");
			}
			bool flag5 = editor?.SaveActiveResource() ?? false;
			TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig = Load<TowerDefenseLevelSpawnConfig>(spawnPath);
			saveReload = flag5 && GodotObject.IsInstanceValid(towerDefenseLevelSpawnConfig) && towerDefenseLevelSpawnConfig.num == 4;
			Require(saveReload, "Visual spawn edit did not survive save and cache-ignoring reload.");
			XWTemplateLibrary.TemplateCreateResult templateCreateResult3 = XWResourceCreateRoute.CreateFromAction("new-grid-wave-spawn", _waveDirectory, "EntryGridSpawn");
			string text2 = Normalize(templateCreateResult3.CreatedPath);
			TowerDefenseLevelGridSpawnConfig towerDefenseLevelGridSpawnConfig = Load<TowerDefenseLevelGridSpawnConfig>(text2);
			gridCreated = templateCreateResult3.Success && GodotObject.IsInstanceValid(towerDefenseLevelGridSpawnConfig) && towerDefenseLevelGridSpawnConfig.gridPos == Vector2I.Zero;
			Require(gridCreated, "Typed grid Spawn template failed: " + templateCreateResult3.Error);
			if (gridCreated)
			{
				XWResourceEditorRegistry.TryOpen(towerDefenseLevelGridSpawnConfig, text2);
			}
			flag = gridCreated;
			if (flag)
			{
				flag = await WaitForActive<TowerDefenseLevelGridSpawnConfig>(text2, 180);
			}
			gridOpened = flag;
			Require(gridOpened, "Typed grid Spawn did not route to GameplayLogic.");
			singleCreate = Directory.GetFiles(_waveDirectory, "EntryWaveManager*.tres").Length == 1 && Directory.GetFiles(_waveDirectory, "EntryWave*.tres").Length == 2 && Directory.GetFiles(_waveDirectory, "EntrySpawn*.tres").Length == 1 && Directory.GetFiles(_waveDirectory, "EntryGridSpawn*.tres").Length == 1;
			Require(singleCreate, "Enter+Confirm submitted the manager name more than once, or a typed resource is missing.");
			inspectorUntouched = (!(XWEditorInterface.Instance.GetInspector() is XWInspector xWInspector) || xWInspector.CurrentObject == inspectorSentinel) && InspectorIsHidden(editor) && editor?.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null;
			Require(inspectorUntouched, "GameplayLogic entry replaced or exposed the raw Inspector.");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish(f3, rightClick, toolbar, iconMenus, lazyOpen, managerCreated, managerOpened, managerPlayable, waveCreated, waveOpened, spawnCreated, spawnOpened, gridCreated, gridOpened, directVisual, undoRedo, saveReload, singleCreate, inspectorUntouched);
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			Control control = XWEditorInterface.Instance?.GetEditorPanel();
			Node instance = control?.FindChild("LoadingOverlay", recursive: true, owned: false);
			if (XWFileSystemPanel.Instance != null && GodotObject.IsInstanceValid(control) && !GodotObject.IsInstanceValid(instance))
			{
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> WaitForActive<T>(string expectedPath, int maxFrames) where T : Resource
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (XWEditorInterface.Instance?.TryGetLoadedResourceEditor("gameplay_logic_editor") is XWGameplayLogicVisualResourceEditor xWGameplayLogicVisualResourceEditor && xWGameplayLogicVisualResourceEditor.ActiveResource is T && SamePath(xWGameplayLogicVisualResourceEditor.ActiveResourcePath, expectedPath))
			{
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<ConfirmationDialog> WaitForNameDialog(Node parent, int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			ConfirmationDialog confirmationDialog = parent.FindChild("NameInputDialog", recursive: true, owned: false) as ConfirmationDialog;
			if (GodotObject.IsInstanceValid(confirmationDialog))
			{
				return confirmationDialog;
			}
			await WaitFrames(1);
		}
		return null;
	}

	private static bool IsPlayableDefault(TowerDefenseLevelWaveManagerConfig manager)
	{
		if (GodotObject.IsInstanceValid(manager))
		{
			Array<TowerDefenseLevelDynamicConfig> dynamic = manager.dynamic;
			if (dynamic != null && dynamic.Count == 7)
			{
				Array<TowerDefenseLevelWaveConfig> wave = manager.wave;
				if (wave != null && wave.Count == 1)
				{
					foreach (TowerDefenseLevelDynamicConfig item in manager.dynamic)
					{
						if (!GodotObject.IsInstanceValid(item))
						{
							return false;
						}
					}
					TowerDefenseLevelWaveConfig towerDefenseLevelWaveConfig = manager.wave[0];
					if (GodotObject.IsInstanceValid(towerDefenseLevelWaveConfig))
					{
						Array<int> dynamicPlantfood = towerDefenseLevelWaveConfig.dynamicPlantfood;
						if (dynamicPlantfood != null && dynamicPlantfood.Count == 7 && GodotObject.IsInstanceValid(towerDefenseLevelWaveConfig.dynamic))
						{
							Array<TowerDefenseLevelSpawnConfig> spawn = towerDefenseLevelWaveConfig.spawn;
							if (spawn != null && spawn.Count == 1 && GodotObject.IsInstanceValid(towerDefenseLevelWaveConfig.spawn[0]) && towerDefenseLevelWaveConfig.spawn[0].zombie == "ZombieNormal" && towerDefenseLevelWaveConfig.spawn[0].num == 1 && towerDefenseLevelWaveConfig.spawn[0].line == -1 && manager.flagZombieUse && manager.flagZombie == "ZombieFlag" && manager.flagWaveInterval == 10 && Math.Abs(manager.spawnFrameBudgetMilliseconds - 6.0) < 0.001)
							{
								return manager.spawnMaxCharactersPerFrame == 8;
							}
						}
					}
					return false;
				}
			}
		}
		return false;
	}

	private static bool InspectorIsHidden(XWGameplayLogicVisualResourceEditor editor)
	{
		PanelContainer panelContainer = editor?.FindChild("InspectorPanel", recursive: true, owned: false) as PanelContainer;
		if (GodotObject.IsInstanceValid(panelContainer))
		{
			return !panelContainer.Visible;
		}
		return false;
	}

	private static bool HasAllEntriesWithIcons(PopupMenu menu, IEnumerable<string> labels)
	{
		if (!GodotObject.IsInstanceValid(menu))
		{
			return false;
		}
		foreach (string label in labels)
		{
			int num = FindMenuItem(menu, label);
			if (num < 0 || !GodotObject.IsInstanceValid(menu.GetItemIcon(num)))
			{
				return false;
			}
		}
		return true;
	}

	private static int FindMenuItem(PopupMenu menu, string text)
	{
		if (!GodotObject.IsInstanceValid(menu))
		{
			return -1;
		}
		for (int i = 0; i < menu.ItemCount; i++)
		{
			if (menu.GetItemText(i).Contains(text, StringComparison.Ordinal))
			{
				return i;
			}
		}
		return -1;
	}

	private static TreeItem FindTreeItemByPath(TreeItem item, string expectedPath)
	{
		if (item == null)
		{
			return null;
		}
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Object)
		{
			XWFileSystemTreeItemData xWFileSystemTreeItemData = metadata.As<XWFileSystemTreeItemData>();
			if (xWFileSystemTreeItemData != null && SamePath(xWFileSystemTreeItemData.Path, expectedPath))
			{
				return item;
			}
		}
		for (TreeItem treeItem = item.GetFirstChild(); treeItem != null; treeItem = treeItem.GetNext())
		{
			TreeItem treeItem2 = FindTreeItemByPath(treeItem, expectedPath);
			if (treeItem2 != null)
			{
				return treeItem2;
			}
		}
		return null;
	}

	private static void ExpandAncestors(TreeItem item)
	{
		for (TreeItem treeItem = item; treeItem != null; treeItem = treeItem.GetParent())
		{
			treeItem.SetCollapsed(enable: false);
		}
	}

	private static T FindFirst<T>(Node root) where T : Node
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
			T val = FindFirst<T>(child);
			if (GodotObject.IsInstanceValid(val))
			{
				return val;
			}
		}
		return null;
	}

	private static T Load<T>(string path) where T : Resource
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		return ResourceLoader.Load<T>(ProjectSettings.LocalizePath(Normalize(path)).Replace('\\', '/'), "", ResourceLoader.CacheMode.Ignore);
	}

	private static string Normalize(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			path = ProjectSettings.GlobalizePath(path);
		}
		return Path.GetFullPath(path).Replace('\\', '/').TrimEnd('/');
	}

	private static bool SamePath(string left, string right)
	{
		return string.Equals(Normalize(left), Normalize(right), StringComparison.OrdinalIgnoreCase);
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
			GD.PrintErr("[MOD_EDITOR_GAMEPLAY_LOGIC_ENTRY_PROBE_FAILURE] " + message);
		}
	}

	private void Finish(bool f3 = false, bool rightClick = false, bool toolbar = false, bool iconMenus = false, bool lazyOpen = false, bool managerCreated = false, bool managerOpened = false, bool managerPlayable = false, bool waveCreated = false, bool waveOpened = false, bool spawnCreated = false, bool spawnOpened = false, bool gridCreated = false, bool gridOpened = false, bool directVisual = false, bool undoRedo = false, bool saveReload = false, bool singleCreate = false, bool inspectorUntouched = false)
	{
		GD.Print($"[MOD_EDITOR_GAMEPLAY_LOGIC_ENTRY_PROBE] f3={f3} rightClick={rightClick} toolbar={toolbar} iconMenus={iconMenus} lazyOpen={lazyOpen} managerCreated={managerCreated} managerOpened={managerOpened} managerPlayable={managerPlayable} waveCreated={waveCreated} waveOpened={waveOpened} spawnCreated={spawnCreated} spawnOpened={spawnOpened} gridCreated={gridCreated} gridOpened={gridOpened} directVisual={directVisual} undoRedo={undoRedo} saveReload={saveReload} singleCreate={singleCreate} inspectorUntouched={inspectorUntouched} failures={_failures.Count}");
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_GAMEPLAY_LOGIC_ENTRY_PROBE_FAILURE] " + failure);
		}
		if (_failures.Count == 0 && System.Environment.GetEnvironmentVariable("MOD_EDITOR_VISIBLE_VALIDATION") == "1")
		{
			string path = Normalize(Path.Combine(_waveDirectory, "EntryWaveManager.tres"));
			TowerDefenseLevelWaveManagerConfig towerDefenseLevelWaveManagerConfig = Load<TowerDefenseLevelWaveManagerConfig>(path);
			if (GodotObject.IsInstanceValid(towerDefenseLevelWaveManagerConfig))
			{
				XWResourceEditorRegistry.TryOpen(towerDefenseLevelWaveManagerConfig, path);
			}
			GD.Print("[MOD_EDITOR_GAMEPLAY_LOGIC_ENTRY_PROBE] visibleHold=True");
			GetTree().CreateTimer(90.0).Timeout += () =>
			{
				GetTree().Quit();
			};
		}
		else
		{
			GetTree().Quit((_failures.Count != 0) ? 1 : 0);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsPlayableDefault, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.InspectorIsHidden, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindMenuItem, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "menu", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindTreeItemByPath, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.String, "expectedPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExpandAncestors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.Normalize, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SamePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "f3", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "rightClick", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "toolbar", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "iconMenus", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "lazyOpen", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "managerCreated", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "managerOpened", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "managerPlayable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "waveCreated", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "waveOpened", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "spawnCreated", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "spawnOpened", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "gridCreated", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "gridOpened", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "directVisual", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "undoRedo", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "saveReload", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "singleCreate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "inspectorUntouched", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IsPlayableDefault && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPlayableDefault(VariantUtils.ConvertTo<TowerDefenseLevelWaveManagerConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.InspectorIsHidden && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(InspectorIsHidden(VariantUtils.ConvertTo<XWGameplayLogicVisualResourceEditor>(in args[0])));
			return true;
		}
		if (method == MethodName.FindMenuItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindMenuItem(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindTreeItemByPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TreeItem>(FindTreeItemByPath(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ExpandAncestors && args.Count == 1)
		{
			ExpandAncestors(VariantUtils.ConvertTo<TreeItem>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Normalize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(Normalize(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SamePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SamePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 19)
		{
			Finish(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]), VariantUtils.ConvertTo<bool>(in args[8]), VariantUtils.ConvertTo<bool>(in args[9]), VariantUtils.ConvertTo<bool>(in args[10]), VariantUtils.ConvertTo<bool>(in args[11]), VariantUtils.ConvertTo<bool>(in args[12]), VariantUtils.ConvertTo<bool>(in args[13]), VariantUtils.ConvertTo<bool>(in args[14]), VariantUtils.ConvertTo<bool>(in args[15]), VariantUtils.ConvertTo<bool>(in args[16]), VariantUtils.ConvertTo<bool>(in args[17]), VariantUtils.ConvertTo<bool>(in args[18]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsPlayableDefault && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPlayableDefault(VariantUtils.ConvertTo<TowerDefenseLevelWaveManagerConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.InspectorIsHidden && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(InspectorIsHidden(VariantUtils.ConvertTo<XWGameplayLogicVisualResourceEditor>(in args[0])));
			return true;
		}
		if (method == MethodName.FindMenuItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindMenuItem(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindTreeItemByPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TreeItem>(FindTreeItemByPath(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ExpandAncestors && args.Count == 1)
		{
			ExpandAncestors(VariantUtils.ConvertTo<TreeItem>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Normalize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(Normalize(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SamePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SamePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.IsPlayableDefault)
		{
			return true;
		}
		if (method == MethodName.InspectorIsHidden)
		{
			return true;
		}
		if (method == MethodName.FindMenuItem)
		{
			return true;
		}
		if (method == MethodName.FindTreeItemByPath)
		{
			return true;
		}
		if (method == MethodName.ExpandAncestors)
		{
			return true;
		}
		if (method == MethodName.Normalize)
		{
			return true;
		}
		if (method == MethodName.SamePath)
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
		if (name == PropertyName._probeRoot)
		{
			_probeRoot = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._waveDirectory)
		{
			_waveDirectory = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._probeRoot)
		{
			value = VariantUtils.CreateFrom(in _probeRoot);
			return true;
		}
		if (name == PropertyName._waveDirectory)
		{
			value = VariantUtils.CreateFrom(in _waveDirectory);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._probeRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._waveDirectory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._probeRoot, Variant.From(in _probeRoot));
		info.AddProperty(PropertyName._waveDirectory, Variant.From(in _waveDirectory));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._probeRoot, out var value))
		{
			_probeRoot = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._waveDirectory, out var value2))
		{
			_waveDirectory = value2.As<string>();
		}
	}
}
