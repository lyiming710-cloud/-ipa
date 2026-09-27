using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Layout;

[ScriptPath("res://addons/ModEditor/Layout/XWDockContainer.cs")]
public class XWDockContainer : TabContainer
{
	public new class MethodName : TabContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnTabBarInput = "OnTabBarInput";

		public static readonly StringName ShouldHideBottomPanelOnTabClick = "ShouldHideBottomPanelOnTabClick";

		public static readonly StringName HideBottomPanelParent = "HideBottomPanelParent";

		public static readonly StringName CreateDragPreview = "CreateDragPreview";

		public static readonly StringName ReorderTab = "ReorderTab";

		public static readonly StringName GetDropTabIndex = "GetDropTabIndex";

		public static readonly StringName MoveTabControl = "MoveTabControl";

		public static readonly StringName IdentifyPanel = "IdentifyPanel";

		public static readonly StringName UpdateContextMenuState = "UpdateContextMenuState";

		public static readonly StringName SetMoveItemDisabled = "SetMoveItemDisabled";

		public static readonly StringName OnContextMenuItem = "OnContextMenuItem";

		public static readonly StringName AddDock = "AddDock";

		public static readonly StringName RemoveDock = "RemoveDock";
	}

	public new class PropertyName : TabContainer.PropertyName
	{
		public static readonly StringName DockPosition = "DockPosition";

		public static readonly StringName _isDraggingTab = "_isDraggingTab";

		public static readonly StringName _dragStartPos = "_dragStartPos";

		public static readonly StringName _dragTabIdx = "_dragTabIdx";

		public static readonly StringName _pressedTabIdx = "_pressedTabIdx";

		public static readonly StringName _pressedTabWasCurrent = "_pressedTabWasCurrent";

		public static readonly StringName _tabContextMenu = "_tabContextMenu";

		public static readonly StringName _contextTabIndex = "_contextTabIndex";
	}

	public new class SignalName : TabContainer.SignalName
	{
	}

	private const float DragThreshold = 12f;

	private const int FloatWindowId = 100;

	private const int CloseId = 101;

	private static readonly int[] MoveDockIds = new int[10] { 4, 5, 6, 7, 2, 8, 9, 10, 11, 3 };

	private bool _isDraggingTab;

	private Vector2 _dragStartPos = Vector2.Zero;

	private int _dragTabIdx = -1;

	private int _pressedTabIdx = -1;

	private bool _pressedTabWasCurrent;

	private PopupMenu _tabContextMenu;

	private int _contextTabIndex = -1;

	public int DockPosition { get; set; }

	public override void _Ready()
	{
		TabBar tabBar = GetTabBar();
		if (tabBar != null)
		{
			tabBar.GuiInput += OnTabBarInput;
		}
		_tabContextMenu = GetNode<PopupMenu>("%TabContextMenu");
		_tabContextMenu.IdPressed += OnContextMenuItem;
	}

	private void OnTabBarInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton inputEventMouseButton)
		{
			if (inputEventMouseButton.ButtonIndex == MouseButton.Left)
			{
				if (inputEventMouseButton.Pressed)
				{
					int num = (_pressedTabIdx = GetTabBar()?.GetTabIdxAtPoint(inputEventMouseButton.Position) ?? (-1));
					_pressedTabWasCurrent = num >= 0 && num == CurrentTab;
					if (num >= 0)
					{
						_isDraggingTab = true;
						_dragStartPos = inputEventMouseButton.Position;
						_dragTabIdx = num;
					}
					return;
				}
				int releaseTabIdx = GetTabBar()?.GetTabIdxAtPoint(inputEventMouseButton.Position) ?? (-1);
				if (ShouldHideBottomPanelOnTabClick(releaseTabIdx, inputEventMouseButton.Position))
				{
					HideBottomPanelParent();
					GetViewport()?.SetInputAsHandled();
				}
				_isDraggingTab = false;
				_dragTabIdx = -1;
				_pressedTabIdx = -1;
				_pressedTabWasCurrent = false;
			}
			else if (inputEventMouseButton.ButtonIndex == MouseButton.Right && inputEventMouseButton.Pressed)
			{
				int num2 = GetTabBar()?.GetTabIdxAtPoint(inputEventMouseButton.Position) ?? (-1);
				if (num2 >= 0)
				{
					_contextTabIndex = num2;
					UpdateContextMenuState(GetTabControl(_contextTabIndex) as XWEditorDock);
					_tabContextMenu.Position = DisplayServer.MouseGetPosition();
					_tabContextMenu.ResetSize();
					_tabContextMenu.Popup();
				}
			}
		}
		else
		{
			if (!(@event is InputEventMouseMotion inputEventMouseMotion) || !_isDraggingTab || _dragTabIdx < 0 || !(_dragStartPos.DistanceTo(inputEventMouseMotion.Position) > 12f))
			{
				return;
			}
			Control tabControl = GetTabControl(_dragTabIdx);
			if (tabControl != null)
			{
				string text = IdentifyPanel(tabControl);
				if (text != "")
				{
					XWDragData xWDragData = new XWDragData(text, XWDragData.Type.DockPanel);
					Label preview = CreateDragPreview(text);
					ForceDrag(xWDragData, preview);
				}
			}
			_isDraggingTab = false;
			_dragTabIdx = -1;
		}
	}

	private bool ShouldHideBottomPanelOnTabClick(int releaseTabIdx, Vector2 releasePos)
	{
		if (DockPosition == 3 && _pressedTabWasCurrent && _pressedTabIdx >= 0 && releaseTabIdx == _pressedTabIdx)
		{
			return _dragStartPos.DistanceTo(releasePos) <= 12f;
		}
		return false;
	}

	private void HideBottomPanelParent()
	{
		for (Node parent = GetParent(); parent is Control control; parent = control.GetParent())
		{
			if (control.Name == (StringName)"BottomPanel" || control.GetType().Name == "XWBottomPanel")
			{
				control.Visible = false;
				return;
			}
		}
		Visible = false;
	}

	private static Label CreateDragPreview(string panelKey)
	{
		string text = panelKey;
		if (XWLayoutManager.Instance != null)
		{
			text = XWLayoutManager.Instance.GetPanelName(panelKey);
		}
		Label label = new Label();
		label.Text = "  " + text + "  ";
		StyleBoxFlat styleBoxFlat = new StyleBoxFlat();
		styleBoxFlat.BgColor = new Color(0.04f, 0.54f, 1f, 0.7f);
		styleBoxFlat.SetCornerRadiusAll(4);
		styleBoxFlat.SetContentMarginAll(8f);
		styleBoxFlat.BorderColor = new Color(0.06f, 0.58f, 1f, 0.8f);
		styleBoxFlat.BorderWidthLeft = 1;
		styleBoxFlat.BorderWidthTop = 1;
		styleBoxFlat.BorderWidthRight = 1;
		styleBoxFlat.BorderWidthBottom = 1;
		styleBoxFlat.ShadowColor = new Color(0.04f, 0.54f, 1f, 0.35f);
		styleBoxFlat.ShadowSize = 16;
		styleBoxFlat.ShadowOffset = new Vector2(0f, 3f);
		label.AddThemeStyleboxOverride("normal", styleBoxFlat);
		label.Scale = new Vector2(0.8f, 0.8f);
		label.PivotOffset = label.Size / 2f;
		Tween tween = label.CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Back);
		tween.TweenProperty(label, "scale", new Vector2(1f, 1f), 0.2);
		return label;
	}

	public void ReorderTab(string panelKey, Vector2 mouseGlobalPos)
	{
		Control control = null;
		for (int i = 0; i < GetTabCount(); i++)
		{
			Control tabControl = GetTabControl(i);
			if (tabControl != null && tabControl.HasMeta("_dock_panel_key") && (string)tabControl.GetMeta("_dock_panel_key") == panelKey)
			{
				control = tabControl;
				break;
			}
		}
		if (control == null)
		{
			return;
		}
		int tabIdxFromControl = GetTabIdxFromControl(control);
		if (tabIdxFromControl < 0)
		{
			return;
		}
		TabBar tabBar = GetTabBar();
		if (tabBar == null)
		{
			return;
		}
		int dropTabIndex = GetDropTabIndex(tabBar, mouseGlobalPos);
		if (dropTabIndex >= 0 && dropTabIndex != tabIdxFromControl)
		{
			MoveTabControl(control, dropTabIndex);
			string text = "";
			if (XWLayoutManager.Instance != null)
			{
				text = XWLayoutManager.Instance.GetPanelName(panelKey);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = panelKey;
			}
			int tabIdxFromControl2 = GetTabIdxFromControl(control);
			if (tabIdxFromControl2 >= 0)
			{
				SetTabTitle(tabIdxFromControl2, text);
				CurrentTab = tabIdxFromControl2;
			}
		}
	}

	private int GetDropTabIndex(TabBar tabBar, Vector2 mouseGlobalPos)
	{
		Vector2 point = mouseGlobalPos - tabBar.GetGlobalRect().Position;
		point.Y = Mathf.Max(1f, tabBar.Size.Y * 0.5f);
		int tabIdxAtPoint = tabBar.GetTabIdxAtPoint(point);
		if (tabIdxAtPoint >= 0)
		{
			return tabIdxAtPoint;
		}
		int tabCount = GetTabCount();
		if (tabCount <= 0)
		{
			return -1;
		}
		if (!(point.X <= 0f))
		{
			return tabCount - 1;
		}
		return 0;
	}

	private void MoveTabControl(Control panel, int tabIndex)
	{
		Control tabControl = GetTabControl(tabIndex);
		if (tabControl != null && tabControl != panel)
		{
			MoveChild(panel, tabControl.GetIndex());
		}
	}

	private static string IdentifyPanel(Control panel)
	{
		if (panel != null && panel.HasMeta("_dock_panel_key"))
		{
			return (string)panel.GetMeta("_dock_panel_key");
		}
		return "";
	}

	private void UpdateContextMenuState(XWEditorDock dock)
	{
		int[] moveDockIds = MoveDockIds;
		foreach (int num in moveDockIds)
		{
			SetMoveItemDisabled(num, dock, num);
		}
	}

	private void SetMoveItemDisabled(int itemId, XWEditorDock dock, int targetDock)
	{
		int itemIndex = _tabContextMenu.GetItemIndex(itemId);
		if (itemIndex >= 0)
		{
			bool flag = XWLayoutManager.Instance?.CanMoveDockTo(dock, targetDock) ?? false;
			_tabContextMenu.SetItemDisabled(itemIndex, !flag);
		}
	}

	private void OnContextMenuItem(long id)
	{
		if (_contextTabIndex >= 0 && GetTabControl(_contextTabIndex) is XWEditorDock xWEditorDock)
		{
			switch (id)
			{
			case 100L:
				xWEditorDock.MakeFloating();
				break;
			case 101L:
				xWEditorDock.Close();
				XWLayoutManager.Instance?.AutoSaveLayout();
				break;
			default:
				XWLayoutManager.Instance?.MoveDockTo(xWEditorDock, (int)id);
				break;
			}
			_contextTabIndex = -1;
		}
	}

	public void AddDock(XWEditorDock dock)
	{
		AddChild(dock, forceReadableName: false, InternalMode.Disabled);
		int tabIdx = (CurrentTab = GetTabCount() - 1);
		Texture2D texture2D = dock.DockIcon;
		if (texture2D == null && !string.IsNullOrEmpty(dock.LayoutKey))
		{
			texture2D = XWPanelRegistry.Instance?.GetIcon(dock.LayoutKey);
		}
		if (texture2D != null)
		{
			SetTabIcon(tabIdx, texture2D);
		}
	}

	public void RemoveDock(XWEditorDock dock)
	{
		RemoveChild(dock);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTabBarInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldHideBottomPanelOnTabClick, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "releaseTabIdx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "releasePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HideBottomPanelParent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateDragPreview, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "panelKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReorderTab, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "panelKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mouseGlobalPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDropTabIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tabBar", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TabBar"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mouseGlobalPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveTabControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "panel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Int, "tabIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdentifyPanel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "panel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateContextMenuState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetMoveItemDisabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "itemId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false),
				new PropertyInfo(Variant.Type.Int, "targetDock", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnContextMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddDock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveDock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false)
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
		if (method == MethodName.OnTabBarInput && args.Count == 1)
		{
			OnTabBarInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldHideBottomPanelOnTabClick && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldHideBottomPanelOnTabClick(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.HideBottomPanelParent && args.Count == 0)
		{
			HideBottomPanelParent();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateDragPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateDragPreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReorderTab && args.Count == 2)
		{
			ReorderTab(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDropTabIndex && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetDropTabIndex(VariantUtils.ConvertTo<TabBar>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.MoveTabControl && args.Count == 2)
		{
			MoveTabControl(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdentifyPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(IdentifyPanel(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateContextMenuState && args.Count == 1)
		{
			UpdateContextMenuState(VariantUtils.ConvertTo<XWEditorDock>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMoveItemDisabled && args.Count == 3)
		{
			SetMoveItemDisabled(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<XWEditorDock>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnContextMenuItem && args.Count == 1)
		{
			OnContextMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddDock && args.Count == 1)
		{
			AddDock(VariantUtils.ConvertTo<XWEditorDock>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveDock && args.Count == 1)
		{
			RemoveDock(VariantUtils.ConvertTo<XWEditorDock>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateDragPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateDragPreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IdentifyPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(IdentifyPanel(VariantUtils.ConvertTo<Control>(in args[0])));
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
		if (method == MethodName.OnTabBarInput)
		{
			return true;
		}
		if (method == MethodName.ShouldHideBottomPanelOnTabClick)
		{
			return true;
		}
		if (method == MethodName.HideBottomPanelParent)
		{
			return true;
		}
		if (method == MethodName.CreateDragPreview)
		{
			return true;
		}
		if (method == MethodName.ReorderTab)
		{
			return true;
		}
		if (method == MethodName.GetDropTabIndex)
		{
			return true;
		}
		if (method == MethodName.MoveTabControl)
		{
			return true;
		}
		if (method == MethodName.IdentifyPanel)
		{
			return true;
		}
		if (method == MethodName.UpdateContextMenuState)
		{
			return true;
		}
		if (method == MethodName.SetMoveItemDisabled)
		{
			return true;
		}
		if (method == MethodName.OnContextMenuItem)
		{
			return true;
		}
		if (method == MethodName.AddDock)
		{
			return true;
		}
		if (method == MethodName.RemoveDock)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.DockPosition)
		{
			DockPosition = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._isDraggingTab)
		{
			_isDraggingTab = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragStartPos)
		{
			_dragStartPos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._dragTabIdx)
		{
			_dragTabIdx = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pressedTabIdx)
		{
			_pressedTabIdx = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pressedTabWasCurrent)
		{
			_pressedTabWasCurrent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._tabContextMenu)
		{
			_tabContextMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._contextTabIndex)
		{
			_contextTabIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.DockPosition)
		{
			value = VariantUtils.CreateFrom<int>(DockPosition);
			return true;
		}
		if (name == PropertyName._isDraggingTab)
		{
			value = VariantUtils.CreateFrom(in _isDraggingTab);
			return true;
		}
		if (name == PropertyName._dragStartPos)
		{
			value = VariantUtils.CreateFrom(in _dragStartPos);
			return true;
		}
		if (name == PropertyName._dragTabIdx)
		{
			value = VariantUtils.CreateFrom(in _dragTabIdx);
			return true;
		}
		if (name == PropertyName._pressedTabIdx)
		{
			value = VariantUtils.CreateFrom(in _pressedTabIdx);
			return true;
		}
		if (name == PropertyName._pressedTabWasCurrent)
		{
			value = VariantUtils.CreateFrom(in _pressedTabWasCurrent);
			return true;
		}
		if (name == PropertyName._tabContextMenu)
		{
			value = VariantUtils.CreateFrom(in _tabContextMenu);
			return true;
		}
		if (name == PropertyName._contextTabIndex)
		{
			value = VariantUtils.CreateFrom(in _contextTabIndex);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.DockPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isDraggingTab, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._dragStartPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._dragTabIdx, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pressedTabIdx, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pressedTabWasCurrent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._tabContextMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._contextTabIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.DockPosition, Variant.From<int>(DockPosition));
		info.AddProperty(PropertyName._isDraggingTab, Variant.From(in _isDraggingTab));
		info.AddProperty(PropertyName._dragStartPos, Variant.From(in _dragStartPos));
		info.AddProperty(PropertyName._dragTabIdx, Variant.From(in _dragTabIdx));
		info.AddProperty(PropertyName._pressedTabIdx, Variant.From(in _pressedTabIdx));
		info.AddProperty(PropertyName._pressedTabWasCurrent, Variant.From(in _pressedTabWasCurrent));
		info.AddProperty(PropertyName._tabContextMenu, Variant.From(in _tabContextMenu));
		info.AddProperty(PropertyName._contextTabIndex, Variant.From(in _contextTabIndex));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.DockPosition, out var value))
		{
			DockPosition = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._isDraggingTab, out var value2))
		{
			_isDraggingTab = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragStartPos, out var value3))
		{
			_dragStartPos = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._dragTabIdx, out var value4))
		{
			_dragTabIdx = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pressedTabIdx, out var value5))
		{
			_pressedTabIdx = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pressedTabWasCurrent, out var value6))
		{
			_pressedTabWasCurrent = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._tabContextMenu, out var value7))
		{
			_tabContextMenu = value7.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._contextTabIndex, out var value8))
		{
			_contextTabIndex = value8.As<int>();
		}
	}
}
