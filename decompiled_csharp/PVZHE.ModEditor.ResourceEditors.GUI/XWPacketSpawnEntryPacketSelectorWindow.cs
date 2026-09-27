using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketSpawnEntryPacketSelectorWindow.cs")]
public class XWPacketSpawnEntryPacketSelectorWindow : Window
{
	[Signal]
	public delegate void PacketSelectedEventHandler(string packetKey);

	private sealed class PacketSnapshot
	{
		public string Key { get; init; } = "";

		public string DisplayName { get; init; } = "";

		public string CostText { get; init; } = "";
	}

	public new class MethodName : Window.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ShowFor = "ShowFor";

		public static readonly StringName CancelPendingRequests = "CancelPendingRequests";

		public static readonly StringName OnFilterChanged = "OnFilterChanged";

		public static readonly StringName RebuildFilteredItems = "RebuildFilteredItems";

		public static readonly StringName RefreshVisibleCardPreviews = "RefreshVisibleCardPreviews";

		public static readonly StringName HydrateCardPreview = "HydrateCardPreview";

		public static readonly StringName ScheduleHydrationRetry = "ScheduleHydrationRetry";

		public static readonly StringName RetryHydrateCardPreview = "RetryHydrateCardPreview";

		public static readonly StringName ReleaseCardPreview = "ReleaseCardPreview";

		public static readonly StringName ConfirmSelection = "ConfirmSelection";

		public static readonly StringName CloseSelector = "CloseSelector";

		public static readonly StringName OnVisibilityChanged = "OnVisibilityChanged";

		public static readonly StringName OnWindowSizeChanged = "OnWindowSizeChanged";

		public static readonly StringName OnScrollValueChanged = "OnScrollValueChanged";

		public static readonly StringName UpdateSelectionDescription = "UpdateSelectionDescription";
	}

	public new class PropertyName : Window.PropertyName
	{
		public static readonly StringName _filter = "_filter";

		public static readonly StringName _packetScroll = "_packetScroll";

		public static readonly StringName _packetGrid = "_packetGrid";

		public static readonly StringName _emptyState = "_emptyState";

		public static readonly StringName _description = "_description";

		public static readonly StringName _cancelButton = "_cancelButton";

		public static readonly StringName _selectButton = "_selectButton";

		public static readonly StringName _verticalScrollBar = "_verticalScrollBar";

		public static readonly StringName _selectedPacketKey = "_selectedPacketKey";

		public static readonly StringName _requestVersion = "_requestVersion";

		public static readonly StringName _suppressFilterSignal = "_suppressFilterSignal";
	}

	public new class SignalName : Window.SignalName
	{
		public static readonly StringName PacketSelected = "PacketSelected";
	}

	private const string SelectorCardScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCompactPacketSelectorRow.tscn";

	private const string PacketKeyMetadata = "packet_key";

	private const string RetryCountMetadata = "packet_retry_count";

	private static PackedScene _selectorCardScene;

	private readonly List<PacketSnapshot> _packetSnapshots = new List<PacketSnapshot>();

	private LineEdit _filter;

	private ScrollContainer _packetScroll;

	private GridContainer _packetGrid;

	private Label _emptyState;

	private Label _description;

	private Button _cancelButton;

	private Button _selectButton;

	private ScrollBar _verticalScrollBar;

	private string _selectedPacketKey = "";

	private int _requestVersion;

	private bool _suppressFilterSignal;

	private PacketSelectedEventHandler backing_PacketSelected;

	public event PacketSelectedEventHandler PacketSelected
	{
		add
		{
			backing_PacketSelected = (PacketSelectedEventHandler)Delegate.Combine(backing_PacketSelected, value);
		}
		remove
		{
			backing_PacketSelected = (PacketSelectedEventHandler)Delegate.Remove(backing_PacketSelected, value);
		}
	}

	public override void _Ready()
	{
		_filter = GetNode<LineEdit>("%Filter");
		_packetScroll = GetNode<ScrollContainer>("%PacketScroll");
		_packetGrid = GetNode<GridContainer>("%PacketGrid");
		_emptyState = GetNode<Label>("%EmptyState");
		_description = GetNode<Label>("%Description");
		_cancelButton = GetNode<Button>("%CancelButton");
		_selectButton = GetNode<Button>("%SelectButton");
		_verticalScrollBar = _packetScroll.GetVScrollBar();
		_filter.TextChanged += OnFilterChanged;
		_cancelButton.Pressed += CloseSelector;
		_selectButton.Pressed += ConfirmSelection;
		CloseRequested += CloseSelector;
		VisibilityChanged += OnVisibilityChanged;
		SizeChanged += OnWindowSizeChanged;
		if (GodotObject.IsInstanceValid(_verticalScrollBar))
		{
			_verticalScrollBar.ValueChanged += OnScrollValueChanged;
		}
	}

	public override void _ExitTree()
	{
		CancelPendingRequests();
		if (GodotObject.IsInstanceValid(_filter))
		{
			_filter.TextChanged -= OnFilterChanged;
		}
		if (GodotObject.IsInstanceValid(_cancelButton))
		{
			_cancelButton.Pressed -= CloseSelector;
		}
		if (GodotObject.IsInstanceValid(_selectButton))
		{
			_selectButton.Pressed -= ConfirmSelection;
		}
		CloseRequested -= CloseSelector;
		VisibilityChanged -= OnVisibilityChanged;
		SizeChanged -= OnWindowSizeChanged;
		if (GodotObject.IsInstanceValid(_verticalScrollBar))
		{
			_verticalScrollBar.ValueChanged -= OnScrollValueChanged;
		}
		base._ExitTree();
	}

	public void ShowFor(string selectedPacketKey)
	{
		_requestVersion++;
		_selectedPacketKey = selectedPacketKey?.Trim() ?? "";
		_packetSnapshots.Clear();
		try
		{
			ResourceManager instance = ResourceManager.Instance;
			if (instance != null && instance.AreFullGameplayResourcesReady)
			{
				foreach (string item in (from key in ResourceManager.Instance.GetPacketNames().Concat(ResourceManager.Instance.TOWERDEFENSE_PACKETS.Keys)
					where !string.IsNullOrWhiteSpace(key)
					select key).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy((string key) => key, StringComparer.OrdinalIgnoreCase))
				{
					TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(item);
					string displayName = ((GodotObject.IsInstanceValid(packetConfigReadOnly) && !string.IsNullOrWhiteSpace(packetConfigReadOnly.name)) ? packetConfigReadOnly.name : item);
					string costText = (GodotObject.IsInstanceValid(packetConfigReadOnly) ? packetConfigReadOnly.GetCost().ToString() : "?");
					_packetSnapshots.Add(new PacketSnapshot
					{
						Key = item,
						DisplayName = displayName,
						CostText = costText
					});
				}
			}
		}
		catch (Exception ex)
		{
			_description.Text = "卡牌注册表暂不可用：" + ex.Message;
		}
		_suppressFilterSignal = true;
		_filter.Text = "";
		_suppressFilterSignal = false;
		RebuildFilteredItems("");
		PopupCenteredClamped(new Vector2I(960, 680), 0.9f);
		CallDeferred("RefreshVisibleCardPreviews");
	}

	public void CancelPendingRequests()
	{
		_requestVersion++;
		if (!GodotObject.IsInstanceValid(_packetGrid))
		{
			return;
		}
		foreach (Node child in _packetGrid.GetChildren())
		{
			if (child is Control card)
			{
				ReleaseCardPreview(card);
			}
		}
	}

	private void OnFilterChanged(string query)
	{
		if (!_suppressFilterSignal)
		{
			_requestVersion++;
			RebuildFilteredItems(query);
		}
	}

	private void RebuildFilteredItems(string query)
	{
		if (!GodotObject.IsInstanceValid(_packetGrid))
		{
			return;
		}
		foreach (Node child in _packetGrid.GetChildren())
		{
			_packetGrid.RemoveChild(child);
			child.QueueFree();
		}
		if (_selectorCardScene == null)
		{
			_selectorCardScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCompactPacketSelectorRow.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		string normalized = query?.Trim() ?? "";
		int num = 0;
		foreach (PacketSnapshot packet in _packetSnapshots)
		{
			if (!MatchesQuery(packet, normalized))
			{
				continue;
			}
			Control card = _selectorCardScene?.Instantiate<Control>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(card))
			{
				card.SetMeta("packet_key", packet.Key);
				card.SetMeta("packet_retry_count", 0);
				card.GetNode<Label>("%PacketName").Text = packet.DisplayName;
				card.GetNode<Label>("%PacketCost").Text = "阳光 " + packet.CostText;
				Button node = card.GetNode<Button>("%SelectSurface");
				node.ButtonPressed = string.Equals(packet.Key, _selectedPacketKey, StringComparison.OrdinalIgnoreCase);
				node.Pressed += () =>
				{
					SelectCard(card, packet);
				};
				node.GuiInput += (InputEvent input) =>
				{
					OnCardGuiInput(input, card, packet);
				};
				_packetGrid.AddChild(card, forceReadableName: false, InternalMode.Disabled);
				num++;
			}
		}
		_emptyState.Visible = num == 0;
		_selectButton.Disabled = num == 0 || string.IsNullOrWhiteSpace(_selectedPacketKey) || !_packetSnapshots.Any((PacketSnapshot packetSnapshot) => string.Equals(packetSnapshot.Key, _selectedPacketKey, StringComparison.OrdinalIgnoreCase) && MatchesQuery(packetSnapshot, normalized));
		if (num == 0)
		{
			_description.Text = "没有符合条件的卡牌；请按名称、saveKey 或费用过滤。";
		}
		else
		{
			UpdateSelectionDescription();
		}
		CallDeferred("RefreshVisibleCardPreviews");
	}

	private void RefreshVisibleCardPreviews()
	{
		if (!Visible || !GodotObject.IsInstanceValid(_packetScroll) || !GodotObject.IsInstanceValid(_packetGrid))
		{
			return;
		}
		Rect2 b = _packetScroll.GetGlobalRect().Grow(12f);
		int requestVersion = _requestVersion;
		foreach (Node child in _packetGrid.GetChildren())
		{
			if (child is Control control)
			{
				if (!control.GetGlobalRect().Intersects(b))
				{
					ReleaseCardPreview(control);
				}
				else if (control.GetNode<Control>("%PreviewHost").GetChildCount() == 0)
				{
					control.GetNode<Label>("%LoadingBadge").Visible = true;
					HydrateCardPreview(control, control.GetMeta("packet_key").AsString(), requestVersion);
				}
			}
		}
	}

	private void HydrateCardPreview(Control card, string packetKey, int requestVersion)
	{
		if (requestVersion != _requestVersion || !GodotObject.IsInstanceValid(card) || !card.IsInsideTree())
		{
			return;
		}
		TowerDefensePacketConfig towerDefensePacketConfig = null;
		try
		{
			towerDefensePacketConfig = TowerDefenseManager.GetPacketConfigReadOnly(packetKey)?.Duplicate(deep: true) as TowerDefensePacketConfig;
		}
		catch (Exception)
		{
		}
		if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
		{
			ScheduleHydrationRetry(card, packetKey, requestVersion);
			return;
		}
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = null;
		try
		{
			towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShowWithConfig();
			if (!GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				ScheduleHydrationRetry(card, packetKey, requestVersion);
				return;
			}
			if (requestVersion != _requestVersion || !GodotObject.IsInstanceValid(card) || !card.IsInsideTree())
			{
				towerDefenseInGamePacketShow.QueueFree();
				return;
			}
			Control node = card.GetNode<Control>("%PreviewHost");
			towerDefenseInGamePacketShow.Name = "SelectorPacketPreview";
			towerDefenseInGamePacketShow.onlyDraw = true;
			towerDefenseInGamePacketShow.setPcLayout = true;
			towerDefenseInGamePacketShow.showLove = false;
			towerDefenseInGamePacketShow.select = false;
			towerDefenseInGamePacketShow.alive = true;
			towerDefenseInGamePacketShow.@lock = false;
			towerDefenseInGamePacketShow.MouseFilter = Control.MouseFilterEnum.Ignore;
			towerDefenseInGamePacketShow.Position = new Vector2(16f, 18f);
			towerDefenseInGamePacketShow.Scale = new Vector2(0.23f, 0.23f);
			node.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
			towerDefenseInGamePacketShow.Init(towerDefensePacketConfig);
			towerDefenseInGamePacketShow.onlyDraw = true;
			towerDefenseInGamePacketShow.RefreshPreview();
			card.GetNode<Label>("%LoadingBadge").Visible = false;
		}
		catch (Exception)
		{
			if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				towerDefenseInGamePacketShow.GetParent()?.RemoveChild(towerDefenseInGamePacketShow);
				towerDefenseInGamePacketShow.QueueFree();
			}
			ScheduleHydrationRetry(card, packetKey, requestVersion);
		}
	}

	private void ScheduleHydrationRetry(Control card, string packetKey, int requestVersion)
	{
		if (requestVersion == _requestVersion && GodotObject.IsInstanceValid(card))
		{
			int num = card.GetMeta("packet_retry_count", 0).AsInt32();
			if (num >= 3)
			{
				card.GetNode<Label>("%LoadingBadge").Text = "预览暂不可用";
				return;
			}
			card.SetMeta("packet_retry_count", num + 1);
			CallDeferred("RetryHydrateCardPreview", card.GetInstanceId(), packetKey, requestVersion);
		}
	}

	private void RetryHydrateCardPreview(ulong cardInstanceId, string packetKey, int requestVersion)
	{
		Control control = GodotObject.InstanceFromId(cardInstanceId) as Control;
		if (requestVersion == _requestVersion && GodotObject.IsInstanceValid(control) && control.IsInsideTree())
		{
			HydrateCardPreview(control, packetKey, requestVersion);
		}
	}

	private void ReleaseCardPreview(Control card)
	{
		if (!GodotObject.IsInstanceValid(card))
		{
			return;
		}
		Control nodeOrNull = card.GetNodeOrNull<Control>("%PreviewHost");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			foreach (Node child in nodeOrNull.GetChildren())
			{
				if (child is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow)
				{
					towerDefenseInGamePacketShow.ClearEventHandlers();
				}
				nodeOrNull.RemoveChild(child);
				child.QueueFree();
			}
		}
		Label nodeOrNull2 = card.GetNodeOrNull<Label>("%LoadingBadge");
		if (GodotObject.IsInstanceValid(nodeOrNull2))
		{
			nodeOrNull2.Text = "加载卡面…";
			nodeOrNull2.Visible = false;
		}
		card.SetMeta("packet_retry_count", 0);
	}

	private void SelectCard(Control card, PacketSnapshot packet)
	{
		_selectedPacketKey = packet.Key;
		foreach (Node child in _packetGrid.GetChildren())
		{
			if (child is Control control)
			{
				control.GetNode<Button>("%SelectSurface").ButtonPressed = control == card;
			}
		}
		_selectButton.Disabled = false;
		UpdateSelectionDescription();
	}

	private void OnCardGuiInput(InputEvent input, Control card, PacketSnapshot packet)
	{
		if (input is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left && inputEventMouseButton.DoubleClick)
		{
			SelectCard(card, packet);
			ConfirmSelection();
		}
	}

	private void ConfirmSelection()
	{
		if (!string.IsNullOrWhiteSpace(_selectedPacketKey) && !_selectButton.Disabled)
		{
			string selectedPacketKey = _selectedPacketKey;
			CancelPendingRequests();
			Hide();
			EmitSignal(SignalName.PacketSelected, selectedPacketKey);
		}
	}

	private void CloseSelector()
	{
		CancelPendingRequests();
		Hide();
	}

	private void OnVisibilityChanged()
	{
		if (Visible)
		{
			CallDeferred("RefreshVisibleCardPreviews");
		}
		else
		{
			CancelPendingRequests();
		}
	}

	private void OnWindowSizeChanged()
	{
		CallDeferred("RefreshVisibleCardPreviews");
	}

	private void OnScrollValueChanged(double value)
	{
		CallDeferred("RefreshVisibleCardPreviews");
	}

	private void UpdateSelectionDescription()
	{
		PacketSnapshot packetSnapshot = _packetSnapshots.FirstOrDefault((PacketSnapshot packet) => string.Equals(packet.Key, _selectedPacketKey, StringComparison.OrdinalIgnoreCase));
		_description.Text = ((packetSnapshot == null) ? "单击卡面选择；双击可直接确认。" : $"{packetSnapshot.DisplayName} · saveKey: {packetSnapshot.Key} · 阳光: {packetSnapshot.CostText}");
	}

	private static bool MatchesQuery(PacketSnapshot packet, string query)
	{
		if (string.IsNullOrWhiteSpace(query))
		{
			return true;
		}
		if (!packet.Key.Contains(query, StringComparison.OrdinalIgnoreCase) && !packet.DisplayName.Contains(query, StringComparison.OrdinalIgnoreCase))
		{
			return packet.CostText.Contains(query, StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowFor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "selectedPacketKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelPendingRequests, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnFilterChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildFilteredItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshVisibleCardPreviews, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HydrateCardPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "card", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "packetKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "requestVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScheduleHydrationRetry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "card", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "packetKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "requestVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RetryHydrateCardPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "cardInstanceId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "packetKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "requestVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseCardPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "card", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfirmSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloseSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnWindowSizeChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnScrollValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateSelectionDescription, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowFor && args.Count == 1)
		{
			ShowFor(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CancelPendingRequests && args.Count == 0)
		{
			CancelPendingRequests();
			ret = default;
			return true;
		}
		if (method == MethodName.OnFilterChanged && args.Count == 1)
		{
			OnFilterChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildFilteredItems && args.Count == 1)
		{
			RebuildFilteredItems(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshVisibleCardPreviews && args.Count == 0)
		{
			RefreshVisibleCardPreviews();
			ret = default;
			return true;
		}
		if (method == MethodName.HydrateCardPreview && args.Count == 3)
		{
			HydrateCardPreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleHydrationRetry && args.Count == 3)
		{
			ScheduleHydrationRetry(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RetryHydrateCardPreview && args.Count == 3)
		{
			RetryHydrateCardPreview(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseCardPreview && args.Count == 1)
		{
			ReleaseCardPreview(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfirmSelection && args.Count == 0)
		{
			ConfirmSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.CloseSelector && args.Count == 0)
		{
			CloseSelector();
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisibilityChanged && args.Count == 0)
		{
			OnVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnWindowSizeChanged && args.Count == 0)
		{
			OnWindowSizeChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnScrollValueChanged && args.Count == 1)
		{
			OnScrollValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSelectionDescription && args.Count == 0)
		{
			UpdateSelectionDescription();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ShowFor)
		{
			return true;
		}
		if (method == MethodName.CancelPendingRequests)
		{
			return true;
		}
		if (method == MethodName.OnFilterChanged)
		{
			return true;
		}
		if (method == MethodName.RebuildFilteredItems)
		{
			return true;
		}
		if (method == MethodName.RefreshVisibleCardPreviews)
		{
			return true;
		}
		if (method == MethodName.HydrateCardPreview)
		{
			return true;
		}
		if (method == MethodName.ScheduleHydrationRetry)
		{
			return true;
		}
		if (method == MethodName.RetryHydrateCardPreview)
		{
			return true;
		}
		if (method == MethodName.ReleaseCardPreview)
		{
			return true;
		}
		if (method == MethodName.ConfirmSelection)
		{
			return true;
		}
		if (method == MethodName.CloseSelector)
		{
			return true;
		}
		if (method == MethodName.OnVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.OnWindowSizeChanged)
		{
			return true;
		}
		if (method == MethodName.OnScrollValueChanged)
		{
			return true;
		}
		if (method == MethodName.UpdateSelectionDescription)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._filter)
		{
			_filter = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._packetScroll)
		{
			_packetScroll = VariantUtils.ConvertTo<ScrollContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetGrid)
		{
			_packetGrid = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName._emptyState)
		{
			_emptyState = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._description)
		{
			_description = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._cancelButton)
		{
			_cancelButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._selectButton)
		{
			_selectButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._verticalScrollBar)
		{
			_verticalScrollBar = VariantUtils.ConvertTo<ScrollBar>(in value);
			return true;
		}
		if (name == PropertyName._selectedPacketKey)
		{
			_selectedPacketKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._requestVersion)
		{
			_requestVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._suppressFilterSignal)
		{
			_suppressFilterSignal = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._filter)
		{
			value = VariantUtils.CreateFrom(in _filter);
			return true;
		}
		if (name == PropertyName._packetScroll)
		{
			value = VariantUtils.CreateFrom(in _packetScroll);
			return true;
		}
		if (name == PropertyName._packetGrid)
		{
			value = VariantUtils.CreateFrom(in _packetGrid);
			return true;
		}
		if (name == PropertyName._emptyState)
		{
			value = VariantUtils.CreateFrom(in _emptyState);
			return true;
		}
		if (name == PropertyName._description)
		{
			value = VariantUtils.CreateFrom(in _description);
			return true;
		}
		if (name == PropertyName._cancelButton)
		{
			value = VariantUtils.CreateFrom(in _cancelButton);
			return true;
		}
		if (name == PropertyName._selectButton)
		{
			value = VariantUtils.CreateFrom(in _selectButton);
			return true;
		}
		if (name == PropertyName._verticalScrollBar)
		{
			value = VariantUtils.CreateFrom(in _verticalScrollBar);
			return true;
		}
		if (name == PropertyName._selectedPacketKey)
		{
			value = VariantUtils.CreateFrom(in _selectedPacketKey);
			return true;
		}
		if (name == PropertyName._requestVersion)
		{
			value = VariantUtils.CreateFrom(in _requestVersion);
			return true;
		}
		if (name == PropertyName._suppressFilterSignal)
		{
			value = VariantUtils.CreateFrom(in _suppressFilterSignal);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._filter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetScroll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._description, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cancelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._verticalScrollBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._selectedPacketKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._requestVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._suppressFilterSignal, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._filter, Variant.From(in _filter));
		info.AddProperty(PropertyName._packetScroll, Variant.From(in _packetScroll));
		info.AddProperty(PropertyName._packetGrid, Variant.From(in _packetGrid));
		info.AddProperty(PropertyName._emptyState, Variant.From(in _emptyState));
		info.AddProperty(PropertyName._description, Variant.From(in _description));
		info.AddProperty(PropertyName._cancelButton, Variant.From(in _cancelButton));
		info.AddProperty(PropertyName._selectButton, Variant.From(in _selectButton));
		info.AddProperty(PropertyName._verticalScrollBar, Variant.From(in _verticalScrollBar));
		info.AddProperty(PropertyName._selectedPacketKey, Variant.From(in _selectedPacketKey));
		info.AddProperty(PropertyName._requestVersion, Variant.From(in _requestVersion));
		info.AddProperty(PropertyName._suppressFilterSignal, Variant.From(in _suppressFilterSignal));
		info.AddSignalEventDelegate(SignalName.PacketSelected, backing_PacketSelected);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._filter, out var value))
		{
			_filter = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._packetScroll, out var value2))
		{
			_packetScroll = value2.As<ScrollContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetGrid, out var value3))
		{
			_packetGrid = value3.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName._emptyState, out var value4))
		{
			_emptyState = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._description, out var value5))
		{
			_description = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._cancelButton, out var value6))
		{
			_cancelButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._selectButton, out var value7))
		{
			_selectButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._verticalScrollBar, out var value8))
		{
			_verticalScrollBar = value8.As<ScrollBar>();
		}
		if (info.TryGetProperty(PropertyName._selectedPacketKey, out var value9))
		{
			_selectedPacketKey = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName._requestVersion, out var value10))
		{
			_requestVersion = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._suppressFilterSignal, out var value11))
		{
			_suppressFilterSignal = value11.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<PacketSelectedEventHandler>(SignalName.PacketSelected, out var value12))
		{
			backing_PacketSelected = value12;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.PacketSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalPacketSelected(string packetKey)
	{
		EmitSignal(SignalName.PacketSelected, new ReadOnlySpan<Variant>((Variant)packetKey));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.PacketSelected && args.Count == 1)
		{
			backing_PacketSelected?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.PacketSelected)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
