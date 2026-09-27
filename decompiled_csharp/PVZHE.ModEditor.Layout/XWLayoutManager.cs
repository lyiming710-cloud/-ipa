using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.Layout;

[ScriptPath("res://addons/ModEditor/Layout/XWLayoutManager.cs")]
public class XWLayoutManager : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Initialize = "Initialize";

		public static readonly StringName InitializeExisting = "InitializeExisting";

		public static readonly StringName AdoptContainer = "AdoptContainer";

		public static readonly StringName BuildLayout = "BuildLayout";

		public static readonly StringName CreateContainer = "CreateContainer";

		public static readonly StringName ApplyExpandFill = "ApplyExpandFill";

		public static readonly StringName RefreshDockVisibility = "RefreshDockVisibility";

		public static readonly StringName UpdateDockVisibility = "UpdateDockVisibility";

		public static readonly StringName HasTabs = "HasTabs";

		public static readonly StringName UpdateParentVisibility = "UpdateParentVisibility";

		public static readonly StringName HasVisibleControlChild = "HasVisibleControlChild";

		public static readonly StringName AddPanel = "AddPanel";

		public static readonly StringName AddDockTo = "AddDockTo";

		public static readonly StringName ResolveDockPositionFor = "ResolveDockPositionFor";

		public static readonly StringName ResolveFallbackDockFor = "ResolveFallbackDockFor";

		public static readonly StringName IsCenterDock = "IsCenterDock";

		public static readonly StringName CanMoveDockTo = "CanMoveDockTo";

		public static readonly StringName CanPanelMoveToDock = "CanPanelMoveToDock";

		public static readonly StringName MoveDockTo = "MoveDockTo";

		public static readonly StringName RestoreDock = "RestoreDock";

		public static readonly StringName FocusPanel = "FocusPanel";

		public static readonly StringName CaptureActiveMainPanel = "CaptureActiveMainPanel";

		public static readonly StringName SetActiveMainPanel = "SetActiveMainPanel";

		public static readonly StringName ShowAncestorControls = "ShowAncestorControls";

		public static readonly StringName GetDock = "GetDock";

		public static readonly StringName AddControlToDock = "AddControlToDock";

		public static readonly StringName RemoveControlFromDock = "RemoveControlFromDock";

		public static readonly StringName AddMainScreenPlugin = "AddMainScreenPlugin";

		public static readonly StringName RemoveMainScreenPlugin = "RemoveMainScreenPlugin";

		public static readonly StringName ApplySavedLayout = "ApplySavedLayout";

		public static readonly StringName GetDockPosition = "GetDockPosition";

		public static readonly StringName AutoSaveLayout = "AutoSaveLayout";

		public static readonly StringName LoadLayout = "LoadLayout";

		public static readonly StringName EnsureLayoutDir = "EnsureLayoutDir";

		public static readonly StringName GetCurrentLayout = "GetCurrentLayout";

		public static readonly StringName GetContainer = "GetContainer";

		public static readonly StringName GetDockContainer = "GetDockContainer";

		public static readonly StringName GetRoot = "GetRoot";

		public static readonly StringName GetRegistry = "GetRegistry";

		public static readonly StringName GetPanelCurrentDock = "GetPanelCurrentDock";

		public static readonly StringName MovePanelToDock = "MovePanelToDock";

		public static readonly StringName GetPanelName = "GetPanelName";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName ActiveMainPanelKey = "ActiveMainPanelKey";

		public static readonly StringName _root = "_root";

		public static readonly StringName _panelRegistry = "_panelRegistry";

		public static readonly StringName _currentLayout = "_currentLayout";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private const string LayoutDir = "user://ModEditorLayouts";

	private const string CurrentLayoutPath = "user://ModEditorLayouts/current_layout.cfg";

	private static readonly int[] SideDockPositions = new int[8] { 4, 5, 6, 7, 8, 9, 10, 11 };

	private static readonly HashSet<string> AutoHideParentNames = new HashSet<string> { "LeftCol", "LeftColRight", "LeftPanel", "RightCol", "RightColRight", "RightPanel" };

	private Control _root;

	private readonly System.Collections.Generic.Dictionary<int, TabContainer> _containers = new System.Collections.Generic.Dictionary<int, TabContainer>();

	private readonly System.Collections.Generic.Dictionary<string, XWEditorDock> _docksByKey = new System.Collections.Generic.Dictionary<string, XWEditorDock>();

	private readonly List<XWWindowWrapper> _floatingWindows = new List<XWWindowWrapper>();

	private readonly HashSet<TabContainer> _observedTabContainers = new HashSet<TabContainer>();

	private XWPanelRegistry _panelRegistry;

	private XWLayoutData _currentLayout = new XWLayoutData();

	public static XWLayoutManager Instance { get; private set; }

	public string ActiveMainPanelKey { get; private set; } = "";

	public void Initialize(Control root, XWPanelRegistry panelRegistry)
	{
		Instance = this;
		_root = root;
		_panelRegistry = panelRegistry;
		BuildLayout();
		LoadLayout();
	}

	public void InitializeExisting(Control root, XWPanelRegistry panelRegistry)
	{
		Instance = this;
		_root = root;
		_panelRegistry = panelRegistry;
		LoadLayout();
	}

	public void AdoptContainer(int position, TabContainer container)
	{
		_containers[position] = container;
		ApplyExpandFill(container);
		if (_observedTabContainers.Add(container))
		{
			container.TabChanged += (long _) =>
			{
				CaptureActiveMainPanel(container);
			};
		}
		if (container is XWDockContainer xWDockContainer)
		{
			xWDockContainer.DockPosition = position;
		}
		UpdateDockVisibility();
	}

	private void BuildLayout()
	{
		foreach (Node child in _root.GetChildren())
		{
			if (child != null)
			{
				Node node = child;
				node.QueueFree();
			}
		}
		HSplitContainer hSplitContainer = new HSplitContainer
		{
			Name = "MainHSplit"
		};
		hSplitContainer.SetAnchorsPreset(Control.LayoutPreset.FullRect);
		ApplyExpandFill(hSplitContainer);
		HSplitContainer hSplitContainer2 = new HSplitContainer
		{
			Name = "LeftPanel"
		};
		hSplitContainer2.CustomMinimumSize = new Vector2(240f, 0f);
		ApplyExpandFill(hSplitContainer2);
		VSplitContainer vSplitContainer = new VSplitContainer
		{
			Name = "LeftCol"
		};
		ApplyExpandFill(vSplitContainer);
		XWDockContainer node2 = CreateContainer(4);
		XWDockContainer node3 = CreateContainer(5);
		vSplitContainer.AddChild(node2, forceReadableName: false, Node.InternalMode.Disabled);
		vSplitContainer.AddChild(node3, forceReadableName: false, Node.InternalMode.Disabled);
		VSplitContainer vSplitContainer2 = new VSplitContainer
		{
			Name = "LeftColRight"
		};
		ApplyExpandFill(vSplitContainer2);
		XWDockContainer node4 = CreateContainer(6);
		XWDockContainer node5 = CreateContainer(7);
		vSplitContainer2.AddChild(node4, forceReadableName: false, Node.InternalMode.Disabled);
		vSplitContainer2.AddChild(node5, forceReadableName: false, Node.InternalMode.Disabled);
		hSplitContainer2.AddChild(vSplitContainer, forceReadableName: false, Node.InternalMode.Disabled);
		hSplitContainer2.AddChild(vSplitContainer2, forceReadableName: false, Node.InternalMode.Disabled);
		XWDockContainer node6 = CreateContainer(2);
		HSplitContainer hSplitContainer3 = new HSplitContainer
		{
			Name = "RightPanel"
		};
		hSplitContainer3.CustomMinimumSize = new Vector2(280f, 0f);
		ApplyExpandFill(hSplitContainer3);
		VSplitContainer vSplitContainer3 = new VSplitContainer
		{
			Name = "RightCol"
		};
		ApplyExpandFill(vSplitContainer3);
		XWDockContainer node7 = CreateContainer(8);
		XWDockContainer node8 = CreateContainer(9);
		vSplitContainer3.AddChild(node7, forceReadableName: false, Node.InternalMode.Disabled);
		vSplitContainer3.AddChild(node8, forceReadableName: false, Node.InternalMode.Disabled);
		VSplitContainer vSplitContainer4 = new VSplitContainer
		{
			Name = "RightColRight"
		};
		ApplyExpandFill(vSplitContainer4);
		XWDockContainer node9 = CreateContainer(10);
		XWDockContainer node10 = CreateContainer(11);
		vSplitContainer4.AddChild(node9, forceReadableName: false, Node.InternalMode.Disabled);
		vSplitContainer4.AddChild(node10, forceReadableName: false, Node.InternalMode.Disabled);
		hSplitContainer3.AddChild(vSplitContainer3, forceReadableName: false, Node.InternalMode.Disabled);
		hSplitContainer3.AddChild(vSplitContainer4, forceReadableName: false, Node.InternalMode.Disabled);
		hSplitContainer.AddChild(hSplitContainer2, forceReadableName: false, Node.InternalMode.Disabled);
		hSplitContainer.AddChild(node6, forceReadableName: false, Node.InternalMode.Disabled);
		hSplitContainer.AddChild(hSplitContainer3, forceReadableName: false, Node.InternalMode.Disabled);
		VSplitContainer vSplitContainer5 = new VSplitContainer
		{
			Name = "OuterVSplit"
		};
		vSplitContainer5.SetAnchorsPreset(Control.LayoutPreset.FullRect);
		ApplyExpandFill(vSplitContainer5);
		vSplitContainer5.AddChild(hSplitContainer, forceReadableName: false, Node.InternalMode.Disabled);
		XWDockContainer xWDockContainer = CreateContainer(3);
		xWDockContainer.CustomMinimumSize = new Vector2(0f, 120f);
		vSplitContainer5.AddChild(xWDockContainer, forceReadableName: false, Node.InternalMode.Disabled);
		_root.AddChild(vSplitContainer5, forceReadableName: false, Node.InternalMode.Disabled);
		UpdateDockVisibility();
	}

	private XWDockContainer CreateContainer(int position)
	{
		XWDockContainer xWDockContainer = new XWDockContainer
		{
			Name = $"Dock_{position}",
			DockPosition = position,
			TabAlignment = TabBar.AlignmentMode.Left
		};
		xWDockContainer.SetAnchorsPreset(Control.LayoutPreset.FullRect);
		ApplyExpandFill(xWDockContainer);
		_containers[position] = xWDockContainer;
		return xWDockContainer;
	}

	private static void ApplyExpandFill(Control control)
	{
		if (control != null)
		{
			control.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
			control.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
		}
	}

	public void RefreshDockVisibility()
	{
		UpdateDockVisibility();
	}

	private void UpdateDockVisibility()
	{
		int[] sideDockPositions = SideDockPositions;
		foreach (int key in sideDockPositions)
		{
			if (_containers.TryGetValue(key, out var value) && GodotObject.IsInstanceValid(value))
			{
				value.Visible = HasTabs(value);
			}
		}
		sideDockPositions = SideDockPositions;
		foreach (int key2 in sideDockPositions)
		{
			if (_containers.TryGetValue(key2, out var value2) && GodotObject.IsInstanceValid(value2))
			{
				UpdateParentVisibility(value2);
			}
		}
	}

	private static bool HasTabs(TabContainer container)
	{
		for (int i = 0; i < container.GetTabCount(); i++)
		{
			Control tabControl = container.GetTabControl(i);
			if (tabControl != null && GodotObject.IsInstanceValid(tabControl) && !tabControl.IsQueuedForDeletion())
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateParentVisibility(Node node)
	{
		Node parent = node.GetParent();
		while (parent is Control control && control != _root)
		{
			string item = control.Name.ToString();
			if (AutoHideParentNames.Contains(item))
			{
				control.Visible = HasVisibleControlChild(control);
			}
			parent = control.GetParent();
		}
	}

	private static bool HasVisibleControlChild(Control control)
	{
		foreach (Node child in control.GetChildren())
		{
			if (child is Control control2 && GodotObject.IsInstanceValid(control2) && !control2.IsQueuedForDeletion() && control2.Visible)
			{
				return true;
			}
		}
		return false;
	}

	public void AddPanel(Control panel, string key, string title, int position)
	{
		XWEditorDock xWEditorDock2;
		if (panel is XWEditorDock xWEditorDock)
		{
			xWEditorDock2 = xWEditorDock;
		}
		else
		{
			xWEditorDock2 = new XWEditorDock
			{
				Title = title,
				LayoutKey = key
			};
			panel.Name = "Content";
			xWEditorDock2.AddChild(panel, forceReadableName: false, Node.InternalMode.Disabled);
		}
		panel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
		panel.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
		xWEditorDock2.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
		xWEditorDock2.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
		if (string.IsNullOrEmpty(xWEditorDock2.LayoutKey))
		{
			xWEditorDock2.LayoutKey = key;
		}
		if (string.IsNullOrEmpty(xWEditorDock2.Title))
		{
			xWEditorDock2.Title = title;
		}
		xWEditorDock2.DefaultSlot = _panelRegistry?.GetDefaultDock(key) ?? position;
		xWEditorDock2.IsMainScreen = _panelRegistry?.IsMainPanel(key) ?? xWEditorDock2.IsMainScreen;
		xWEditorDock2.Closable = _panelRegistry?.CanClose(key) ?? xWEditorDock2.Closable;
		XWEditorDock xWEditorDock3 = xWEditorDock2;
		if (xWEditorDock3.DockIcon == null)
		{
			xWEditorDock3.DockIcon = _panelRegistry?.GetIcon(key);
		}
		xWEditorDock2.Name = title;
		xWEditorDock2.SetMeta("_dock_panel_key", key);
		_docksByKey[key] = xWEditorDock2;
		AddDockTo(xWEditorDock2, position);
	}

	public void AddDockTo(XWEditorDock dock, int position)
	{
		int num = ResolveDockPositionFor(dock, position);
		if (_containers.TryGetValue(num, out var value))
		{
			if (dock.GetParent() is TabContainer tabContainer)
			{
				tabContainer.RemoveChild(dock);
			}
			if (!string.IsNullOrEmpty(dock.Title))
			{
				dock.Name = dock.Title;
			}
			if (value is XWDockContainer xWDockContainer)
			{
				xWDockContainer.AddDock(dock);
			}
			else
			{
				value.AddChild(dock, forceReadableName: false, Node.InternalMode.Disabled);
			}
			value.CurrentTab = value.GetTabCount() - 1;
			if (num == 3)
			{
				ShowAncestorControls(value);
			}
			UpdateDockVisibility();
		}
	}

	private static int ResolveDockPositionFor(XWEditorDock dock, int position)
	{
		if (dock != null && dock.IsMainScreen)
		{
			return 2;
		}
		int num = XWLayoutData.ResolveDockPosition(position);
		if (dock != null && num == 2)
		{
			return ResolveFallbackDockFor(dock);
		}
		return num;
	}

	private static int ResolveFallbackDockFor(XWEditorDock dock)
	{
		int num = XWLayoutData.ResolveDockPosition(dock?.DefaultSlot ?? 4);
		if (num == 2)
		{
			return 4;
		}
		return num;
	}

	private static bool IsCenterDock(int position)
	{
		return XWLayoutData.ResolveDockPosition(position) == 2;
	}

	public bool CanMoveDockTo(XWEditorDock dock, int position)
	{
		if (dock == null)
		{
			return false;
		}
		if (!dock.IsMainScreen)
		{
			return !IsCenterDock(position);
		}
		return IsCenterDock(position);
	}

	public bool CanPanelMoveToDock(string panelKey, int targetDock)
	{
		if (_docksByKey.TryGetValue(panelKey, out var value))
		{
			return CanMoveDockTo(value, targetDock);
		}
		return false;
	}

	public void MoveDockTo(XWEditorDock dock, int newPosition)
	{
		if (CanMoveDockTo(dock, newPosition))
		{
			AddDockTo(dock, newPosition);
			AutoSaveLayout();
		}
	}

	public void RestoreDock(XWEditorDock dock)
	{
		AddDockTo(dock, dock.DefaultSlot);
	}

	public void FocusPanel(string key)
	{
		if (!_docksByKey.TryGetValue(key, out var value))
		{
			return;
		}
		value.Visible = true;
		UpdateDockVisibility();
		if (value.GetParent() is TabContainer tabContainer)
		{
			ShowAncestorControls(tabContainer);
			int num = -1;
			for (int i = 0; i < tabContainer.GetTabCount(); i++)
			{
				if (tabContainer.GetTabControl(i) == value)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				tabContainer.CurrentTab = num;
			}
		}
		SetActiveMainPanel(key);
	}

	private void CaptureActiveMainPanel(TabContainer container)
	{
		if (GodotObject.IsInstanceValid(container) && container.CurrentTab >= 0 && container.CurrentTab < container.GetTabCount() && container.GetTabControl(container.CurrentTab) is XWEditorDock xWEditorDock)
		{
			SetActiveMainPanel(xWEditorDock.LayoutKey);
		}
	}

	private void SetActiveMainPanel(string key)
	{
		if (!string.IsNullOrWhiteSpace(key))
		{
			XWPanelRegistry panelRegistry = _panelRegistry;
			if (panelRegistry != null && panelRegistry.IsMainPanel(key))
			{
				ActiveMainPanelKey = key;
			}
		}
	}

	private void ShowAncestorControls(Control control)
	{
		Node node = control;
		while (node is Control control2 && control2 != _root)
		{
			control2.Visible = true;
			if (control2.GetParent() is TabContainer tabContainer)
			{
				for (int i = 0; i < tabContainer.GetTabCount(); i++)
				{
					if (tabContainer.GetTabControl(i) == control2)
					{
						tabContainer.CurrentTab = i;
						break;
					}
				}
			}
			node = control2.GetParent();
		}
	}

	public XWEditorDock GetDock(string key)
	{
		if (!_docksByKey.TryGetValue(key, out var value))
		{
			return null;
		}
		return value;
	}

	public void AddControlToDock(int dockSlot, Control control)
	{
		if (control is XWEditorDock dock)
		{
			AddDockTo(dock, dockSlot);
		}
	}

	public void RemoveControlFromDock(Control control)
	{
		if (control is XWEditorDock xWEditorDock && !string.IsNullOrEmpty(xWEditorDock.LayoutKey))
		{
			_docksByKey.Remove(xWEditorDock.LayoutKey);
		}
		if (control.GetParent() is TabContainer tabContainer)
		{
			tabContainer.RemoveChild(control);
			UpdateDockVisibility();
		}
	}

	public void AddMainScreenPlugin(XWEditorPlugin plugin)
	{
	}

	public void RemoveMainScreenPlugin(XWEditorPlugin plugin)
	{
	}

	public void ApplySavedLayout()
	{
		if (_currentLayout == null || _currentLayout.PanelDocks.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<string, int> item in new List<KeyValuePair<string, int>>(_currentLayout.PanelDocks))
		{
			if (_docksByKey.TryGetValue(item.Key, out var value))
			{
				int num = ResolveDockPositionFor(value, item.Value);
				if (GetDockPosition(value) != num)
				{
					AddDockTo(value, num);
				}
			}
		}
		UpdateDockVisibility();
	}

	public int GetDockPosition(XWEditorDock dock)
	{
		foreach (KeyValuePair<int, TabContainer> container in _containers)
		{
			for (int i = 0; i < container.Value.GetTabCount(); i++)
			{
				if (container.Value.GetTabControl(i) == dock)
				{
					return container.Key;
				}
			}
		}
		return -1;
	}

	public void AutoSaveLayout()
	{
		EnsureLayoutDir();
		_currentLayout.PanelDocks.Clear();
		foreach (KeyValuePair<int, TabContainer> container in _containers)
		{
			for (int i = 0; i < container.Value.GetTabCount(); i++)
			{
				if (container.Value.GetTabControl(i) is XWEditorDock xWEditorDock && !string.IsNullOrEmpty(xWEditorDock.LayoutKey))
				{
					_currentLayout.PanelDocks[xWEditorDock.LayoutKey] = container.Key;
				}
			}
		}
		ConfigFile configFile = new ConfigFile();
		configFile.SetValue("layout", "data", _currentLayout.ToJson());
		configFile.Save("user://ModEditorLayouts/current_layout.cfg");
	}

	private void LoadLayout()
	{
		ConfigFile configFile = new ConfigFile();
		if (configFile.Load("user://ModEditorLayouts/current_layout.cfg") == Error.Ok && configFile.HasSectionKey("layout", "data"))
		{
			Dictionary data = (Dictionary)configFile.GetValue("layout", "data");
			_currentLayout.FromJson(data);
		}
	}

	private void EnsureLayoutDir()
	{
		if (!DirAccess.DirExistsAbsolute("user://ModEditorLayouts"))
		{
			DirAccess.MakeDirAbsolute("user://ModEditorLayouts");
		}
	}

	public XWLayoutData GetCurrentLayout()
	{
		return _currentLayout;
	}

	public TabContainer GetContainer(int position)
	{
		if (!_containers.TryGetValue(position, out var value))
		{
			return null;
		}
		return value;
	}

	public XWDockContainer GetDockContainer(int position)
	{
		if (!_containers.TryGetValue(position, out var value))
		{
			return null;
		}
		return value as XWDockContainer;
	}

	public Control GetRoot()
	{
		return _root;
	}

	public XWPanelRegistry GetRegistry()
	{
		return _panelRegistry;
	}

	public int GetPanelCurrentDock(string panelKey)
	{
		if (!_docksByKey.TryGetValue(panelKey, out var value))
		{
			return 4;
		}
		return GetDockPosition(value);
	}

	public void MovePanelToDock(string panelKey, int targetDock)
	{
		if (_docksByKey.TryGetValue(panelKey, out var value) && CanMoveDockTo(value, targetDock))
		{
			int num = ResolveDockPositionFor(value, targetDock);
			if (GetDockPosition(value) != num)
			{
				MoveDockTo(value, num);
			}
		}
	}

	public string GetPanelName(string panelKey)
	{
		return _panelRegistry?.GetDisplayName(panelKey) ?? panelKey;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(42)
		{
			new MethodInfo(MethodName.Initialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "panelRegistry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeExisting, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "panelRegistry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.AdoptContainer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TabContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateContainer, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TabContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyExpandFill, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshDockVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateDockVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasTabs, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TabContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateParentVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasVisibleControlChild, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "panel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddDockTo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false),
				new PropertyInfo(Variant.Type.Int, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveDockPositionFor, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false),
				new PropertyInfo(Variant.Type.Int, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveFallbackDockFor, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsCenterDock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanMoveDockTo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false),
				new PropertyInfo(Variant.Type.Int, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanPanelMoveToDock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "panelKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetDock", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveDockTo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false),
				new PropertyInfo(Variant.Type.Int, "newPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreDock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.FocusPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureActiveMainPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TabContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetActiveMainPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowAncestorControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetDock, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddControlToDock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "dockSlot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveControlFromDock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddMainScreenPlugin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plugin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveMainScreenPlugin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plugin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySavedLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetDockPosition, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.AutoSaveLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureLayoutDir, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentLayout, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetContainer, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TabContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDockContainer, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TabContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetRoot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRegistry, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPanelCurrentDock, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "panelKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MovePanelToDock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "panelKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetDock", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPanelName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "panelKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Initialize && args.Count == 2)
		{
			Initialize(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<XWPanelRegistry>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeExisting && args.Count == 2)
		{
			InitializeExisting(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<XWPanelRegistry>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AdoptContainer && args.Count == 2)
		{
			AdoptContainer(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TabContainer>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildLayout && args.Count == 0)
		{
			BuildLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateContainer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWDockContainer>(CreateContainer(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyExpandFill && args.Count == 1)
		{
			ApplyExpandFill(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDockVisibility && args.Count == 0)
		{
			RefreshDockVisibility();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDockVisibility && args.Count == 0)
		{
			UpdateDockVisibility();
			ret = default;
			return true;
		}
		if (method == MethodName.HasTabs && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasTabs(VariantUtils.ConvertTo<TabContainer>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateParentVisibility && args.Count == 1)
		{
			UpdateParentVisibility(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasVisibleControlChild && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasVisibleControlChild(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.AddPanel && args.Count == 4)
		{
			AddPanel(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddDockTo && args.Count == 2)
		{
			AddDockTo(VariantUtils.ConvertTo<XWEditorDock>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDockPositionFor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveDockPositionFor(VariantUtils.ConvertTo<XWEditorDock>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolveFallbackDockFor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveFallbackDockFor(VariantUtils.ConvertTo<XWEditorDock>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCenterDock && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCenterDock(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CanMoveDockTo && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanMoveDockTo(VariantUtils.ConvertTo<XWEditorDock>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.CanPanelMoveToDock && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanPanelMoveToDock(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.MoveDockTo && args.Count == 2)
		{
			MoveDockTo(VariantUtils.ConvertTo<XWEditorDock>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreDock && args.Count == 1)
		{
			RestoreDock(VariantUtils.ConvertTo<XWEditorDock>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FocusPanel && args.Count == 1)
		{
			FocusPanel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureActiveMainPanel && args.Count == 1)
		{
			CaptureActiveMainPanel(VariantUtils.ConvertTo<TabContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetActiveMainPanel && args.Count == 1)
		{
			SetActiveMainPanel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowAncestorControls && args.Count == 1)
		{
			ShowAncestorControls(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDock && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWEditorDock>(GetDock(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AddControlToDock && args.Count == 2)
		{
			AddControlToDock(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveControlFromDock && args.Count == 1)
		{
			RemoveControlFromDock(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddMainScreenPlugin && args.Count == 1)
		{
			AddMainScreenPlugin(VariantUtils.ConvertTo<XWEditorPlugin>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveMainScreenPlugin && args.Count == 1)
		{
			RemoveMainScreenPlugin(VariantUtils.ConvertTo<XWEditorPlugin>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySavedLayout && args.Count == 0)
		{
			ApplySavedLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.GetDockPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetDockPosition(VariantUtils.ConvertTo<XWEditorDock>(in args[0])));
			return true;
		}
		if (method == MethodName.AutoSaveLayout && args.Count == 0)
		{
			AutoSaveLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadLayout && args.Count == 0)
		{
			LoadLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureLayoutDir && args.Count == 0)
		{
			EnsureLayoutDir();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentLayout && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWLayoutData>(GetCurrentLayout());
			return true;
		}
		if (method == MethodName.GetContainer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TabContainer>(GetContainer(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDockContainer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWDockContainer>(GetDockContainer(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(GetRoot());
			return true;
		}
		if (method == MethodName.GetRegistry && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWPanelRegistry>(GetRegistry());
			return true;
		}
		if (method == MethodName.GetPanelCurrentDock && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetPanelCurrentDock(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MovePanelToDock && args.Count == 2)
		{
			MovePanelToDock(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPanelName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPanelName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ApplyExpandFill && args.Count == 1)
		{
			ApplyExpandFill(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasTabs && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasTabs(VariantUtils.ConvertTo<TabContainer>(in args[0])));
			return true;
		}
		if (method == MethodName.HasVisibleControlChild && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasVisibleControlChild(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveDockPositionFor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveDockPositionFor(VariantUtils.ConvertTo<XWEditorDock>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolveFallbackDockFor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveFallbackDockFor(VariantUtils.ConvertTo<XWEditorDock>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCenterDock && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCenterDock(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Initialize)
		{
			return true;
		}
		if (method == MethodName.InitializeExisting)
		{
			return true;
		}
		if (method == MethodName.AdoptContainer)
		{
			return true;
		}
		if (method == MethodName.BuildLayout)
		{
			return true;
		}
		if (method == MethodName.CreateContainer)
		{
			return true;
		}
		if (method == MethodName.ApplyExpandFill)
		{
			return true;
		}
		if (method == MethodName.RefreshDockVisibility)
		{
			return true;
		}
		if (method == MethodName.UpdateDockVisibility)
		{
			return true;
		}
		if (method == MethodName.HasTabs)
		{
			return true;
		}
		if (method == MethodName.UpdateParentVisibility)
		{
			return true;
		}
		if (method == MethodName.HasVisibleControlChild)
		{
			return true;
		}
		if (method == MethodName.AddPanel)
		{
			return true;
		}
		if (method == MethodName.AddDockTo)
		{
			return true;
		}
		if (method == MethodName.ResolveDockPositionFor)
		{
			return true;
		}
		if (method == MethodName.ResolveFallbackDockFor)
		{
			return true;
		}
		if (method == MethodName.IsCenterDock)
		{
			return true;
		}
		if (method == MethodName.CanMoveDockTo)
		{
			return true;
		}
		if (method == MethodName.CanPanelMoveToDock)
		{
			return true;
		}
		if (method == MethodName.MoveDockTo)
		{
			return true;
		}
		if (method == MethodName.RestoreDock)
		{
			return true;
		}
		if (method == MethodName.FocusPanel)
		{
			return true;
		}
		if (method == MethodName.CaptureActiveMainPanel)
		{
			return true;
		}
		if (method == MethodName.SetActiveMainPanel)
		{
			return true;
		}
		if (method == MethodName.ShowAncestorControls)
		{
			return true;
		}
		if (method == MethodName.GetDock)
		{
			return true;
		}
		if (method == MethodName.AddControlToDock)
		{
			return true;
		}
		if (method == MethodName.RemoveControlFromDock)
		{
			return true;
		}
		if (method == MethodName.AddMainScreenPlugin)
		{
			return true;
		}
		if (method == MethodName.RemoveMainScreenPlugin)
		{
			return true;
		}
		if (method == MethodName.ApplySavedLayout)
		{
			return true;
		}
		if (method == MethodName.GetDockPosition)
		{
			return true;
		}
		if (method == MethodName.AutoSaveLayout)
		{
			return true;
		}
		if (method == MethodName.LoadLayout)
		{
			return true;
		}
		if (method == MethodName.EnsureLayoutDir)
		{
			return true;
		}
		if (method == MethodName.GetCurrentLayout)
		{
			return true;
		}
		if (method == MethodName.GetContainer)
		{
			return true;
		}
		if (method == MethodName.GetDockContainer)
		{
			return true;
		}
		if (method == MethodName.GetRoot)
		{
			return true;
		}
		if (method == MethodName.GetRegistry)
		{
			return true;
		}
		if (method == MethodName.GetPanelCurrentDock)
		{
			return true;
		}
		if (method == MethodName.MovePanelToDock)
		{
			return true;
		}
		if (method == MethodName.GetPanelName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ActiveMainPanelKey)
		{
			ActiveMainPanelKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._root)
		{
			_root = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._panelRegistry)
		{
			_panelRegistry = VariantUtils.ConvertTo<XWPanelRegistry>(in value);
			return true;
		}
		if (name == PropertyName._currentLayout)
		{
			_currentLayout = VariantUtils.ConvertTo<XWLayoutData>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ActiveMainPanelKey)
		{
			value = VariantUtils.CreateFrom<string>(ActiveMainPanelKey);
			return true;
		}
		if (name == PropertyName._root)
		{
			value = VariantUtils.CreateFrom(in _root);
			return true;
		}
		if (name == PropertyName._panelRegistry)
		{
			value = VariantUtils.CreateFrom(in _panelRegistry);
			return true;
		}
		if (name == PropertyName._currentLayout)
		{
			value = VariantUtils.CreateFrom(in _currentLayout);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._root, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._panelRegistry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._currentLayout, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ActiveMainPanelKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ActiveMainPanelKey, Variant.From<string>(ActiveMainPanelKey));
		info.AddProperty(PropertyName._root, Variant.From(in _root));
		info.AddProperty(PropertyName._panelRegistry, Variant.From(in _panelRegistry));
		info.AddProperty(PropertyName._currentLayout, Variant.From(in _currentLayout));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ActiveMainPanelKey, out var value))
		{
			ActiveMainPanelKey = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._root, out var value2))
		{
			_root = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._panelRegistry, out var value3))
		{
			_panelRegistry = value3.As<XWPanelRegistry>();
		}
		if (info.TryGetProperty(PropertyName._currentLayout, out var value4))
		{
			_currentLayout = value4.As<XWLayoutData>();
		}
	}
}
