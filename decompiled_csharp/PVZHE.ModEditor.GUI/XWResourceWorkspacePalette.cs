using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWResourceWorkspacePalette.cs")]
public class XWResourceWorkspacePalette : Window
{
	private sealed class PaletteEntry
	{
		public string Key { get; init; } = "";

		public string Label { get; init; } = "";

		public string Group { get; init; } = "系统";

		public Texture2D Icon { get; init; }

		public Button Card { get; set; }

		public XWUiMotion Motion { get; set; }
	}

	public new class MethodName : Window.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName AddWorkspace = "AddWorkspace";

		public static readonly StringName OpenPalette = "OpenPalette";

		public static readonly StringName TrySelectWorkspace = "TrySelectWorkspace";

		public static readonly StringName BuildGroupButtons = "BuildGroupButtons";

		public static readonly StringName SelectGroup = "SelectGroup";

		public static readonly StringName PlayPaletteReveal = "PlayPaletteReveal";

		public static readonly StringName RefreshCards = "RefreshCards";

		public static readonly StringName HidePalette = "HidePalette";

		public static readonly StringName ResolveGroup = "ResolveGroup";

		public static readonly StringName ContainsAny = "ContainsAny";

		public static readonly StringName Sanitize = "Sanitize";
	}

	public new class PropertyName : Window.PropertyName
	{
		public static readonly StringName EntryCount = "EntryCount";

		public static readonly StringName VisibleEntryCount = "VisibleEntryCount";

		public static readonly StringName MotionBindingCount = "MotionBindingCount";

		public static readonly StringName SurfaceMotion = "SurfaceMotion";

		public static readonly StringName _searchEdit = "_searchEdit";

		public static readonly StringName _groupFlow = "_groupFlow";

		public static readonly StringName _cardGrid = "_cardGrid";

		public static readonly StringName _resultLabel = "_resultLabel";

		public static readonly StringName _emptyLabel = "_emptyLabel";

		public static readonly StringName _surfaceMotion = "_surfaceMotion";

		public static readonly StringName _activeGroup = "_activeGroup";
	}

	public new class SignalName : Window.SignalName
	{
	}

	private static readonly string[] GroupOrder = new string[6] { "全部", "角色战斗", "关卡玩法", "卡牌经济", "视听表现", "系统工具" };

	private readonly List<PaletteEntry> _entries = new List<PaletteEntry>();

	private readonly Dictionary<string, Button> _groupButtons = new Dictionary<string, Button>(StringComparer.Ordinal);

	private LineEdit _searchEdit;

	private HFlowContainer _groupFlow;

	private GridContainer _cardGrid;

	private Label _resultLabel;

	private Label _emptyLabel;

	private XWUiMotion _surfaceMotion;

	private string _activeGroup = "全部";

	public int EntryCount => _entries.Count;

	public int VisibleEntryCount { get; private set; }

	public int MotionBindingCount { get; private set; }

	public XWUiMotion SurfaceMotion => _surfaceMotion;

	public event Action<string> WorkspaceSelected;

	public event Action PaletteDismissed;

	public override void _Ready()
	{
		_searchEdit = GetNode<LineEdit>("%SearchEdit");
		_groupFlow = GetNode<HFlowContainer>("%GroupFlow");
		_cardGrid = GetNode<GridContainer>("%CardGrid");
		_resultLabel = GetNode<Label>("%ResultLabel");
		_emptyLabel = GetNode<Label>("%EmptyLabel");
		_surfaceMotion = XWUiMotion.BindPanel(GetNodeOrNull<PanelContainer>("%Surface"));
		CloseRequested += HidePalette;
		VisibilityChanged += () =>
		{
			if (!Visible)
			{
				PaletteDismissed?.Invoke();
			}
		};
		_searchEdit.TextChanged += (string _) =>
		{
			RefreshCards();
		};
		BuildGroupButtons();
		RefreshCards();
	}

	public void AddWorkspace(string key, string label, Texture2D icon)
	{
		if (!string.IsNullOrWhiteSpace(key) && !_entries.Exists((PaletteEntry entry) => entry.Key == key))
		{
			PaletteEntry paletteEntry = new PaletteEntry
			{
				Key = key.Trim(),
				Label = (string.IsNullOrWhiteSpace(label) ? "资源" : label.Trim()),
				Group = ResolveGroup(label),
				Icon = icon
			};
			_entries.Add(paletteEntry);
			if (GodotObject.IsInstanceValid(_cardGrid))
			{
				CreateCard(paletteEntry);
				RefreshCards();
			}
		}
	}

	public void OpenPalette()
	{
		if (GodotObject.IsInstanceValid(_searchEdit))
		{
			if (Visible)
			{
				GrabFocus();
				_searchEdit.GrabFocus();
				return;
			}
			_searchEdit.Text = "";
			SelectGroup("全部");
			PopupCenteredClamped(new Vector2I(900, 640), 0.9f);
			GrabFocus();
			_searchEdit.GrabFocus();
			CallDeferred("PlayPaletteReveal");
		}
	}

	public bool TrySelectWorkspace(string key)
	{
		PaletteEntry paletteEntry = _entries.Find((PaletteEntry candidate) => candidate.Key == key);
		if (paletteEntry == null)
		{
			return false;
		}
		WorkspaceSelected?.Invoke(paletteEntry.Key);
		Hide();
		return true;
	}

	private void BuildGroupButtons()
	{
		string[] groupOrder = GroupOrder;
		foreach (string group in groupOrder)
		{
			Button button = new Button
			{
				Name = "Group_" + group,
				Text = group,
				ToggleMode = true,
				ButtonPressed = (group == _activeGroup),
				CustomMinimumSize = new Vector2(92f, 32f),
				ThemeTypeVariation = "GameNavCard",
				TooltipText = "只显示" + group + "资源工作台"
			};
			button.Pressed += () =>
			{
				SelectGroup(group);
			};
			_groupButtons[group] = button;
			_groupFlow.AddChild(button, forceReadableName: false, InternalMode.Disabled);
			if (XWUiMotion.BindButton(button) != null)
			{
				MotionBindingCount++;
			}
		}
	}

	private void SelectGroup(string group)
	{
		_activeGroup = (_groupButtons.ContainsKey(group) ? group : "全部");
		foreach (var (text2, button2) in _groupButtons)
		{
			button2.SetPressedNoSignal(text2 == _activeGroup);
			XWUiMotion.FindBinding(button2)?.SyncButtonState();
		}
		RefreshCards();
	}

	private void CreateCard(PaletteEntry entry)
	{
		Button button = new Button
		{
			Name = "ResourceWorkspace_" + Sanitize(entry.Key),
			Text = entry.Label,
			Icon = entry.Icon,
			ExpandIcon = false,
			Alignment = HorizontalAlignment.Left,
			CustomMinimumSize = new Vector2(190f, 70f),
			SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
			TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis,
			TooltipText = $"{entry.Label}\n{entry.Group} · {entry.Key}\n点击进入可视资源工作台",
			ThemeTypeVariation = "GameNavCard"
		};
		button.Pressed += () =>
		{
			WorkspaceSelected?.Invoke(entry.Key);
			Hide();
		};
		entry.Card = button;
		_cardGrid.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		entry.Motion = XWUiMotion.BindButton(button, XWUiMotion.MotionRole.Card);
		if (entry.Motion != null)
		{
			MotionBindingCount++;
		}
	}

	private void PlayPaletteReveal()
	{
		_surfaceMotion?.PlayPanelReveal(12f);
	}

	private void RefreshCards()
	{
		if (!GodotObject.IsInstanceValid(_cardGrid))
		{
			return;
		}
		string value = _searchEdit?.Text?.Trim() ?? "";
		int num = 0;
		foreach (PaletteEntry entry in _entries)
		{
			bool num2 = _activeGroup == "全部" || entry.Group == _activeGroup;
			bool flag = string.IsNullOrWhiteSpace(value) || entry.Label.Contains(value, StringComparison.OrdinalIgnoreCase) || entry.Key.Contains(value, StringComparison.OrdinalIgnoreCase) || entry.Group.Contains(value, StringComparison.OrdinalIgnoreCase);
			bool flag2 = num2 & flag;
			if (GodotObject.IsInstanceValid(entry.Card))
			{
				entry.Card.Visible = flag2;
			}
			if (flag2)
			{
				num++;
			}
		}
		VisibleEntryCount = num;
		if (GodotObject.IsInstanceValid(_resultLabel))
		{
			_resultLabel.Text = $"{num} / {_entries.Count} 个可视工作台";
		}
		if (GodotObject.IsInstanceValid(_emptyLabel))
		{
			_emptyLabel.Visible = num == 0;
		}
	}

	private void HidePalette()
	{
		Hide();
	}

	private static string ResolveGroup(string label)
	{
		string value = label ?? "";
		if (ContainsAny(value, "角色", "战斗", "投射", "BUFF", "掉落", "收集", "小推车", "铲子", "组件"))
		{
			return "角色战斗";
		}
		if (ContainsAny(value, "关卡", "地图", "生存", "事件", "波次", "解锁", "传送", "玩法", "格子"))
		{
			return "关卡玩法";
		}
		if (ContainsAny(value, "卡片", "卡牌", "费用", "商店", "卡库", "牌库"))
		{
			return "卡牌经济";
		}
		if (ContainsAny(value, "动画", "音频", "BGM", "对话", "界面", "外观", "拼图"))
		{
			return "视听表现";
		}
		return "系统工具";
	}

	private static bool ContainsAny(string value, params string[] tokens)
	{
		foreach (string value2 in tokens)
		{
			if (value.Contains(value2, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	private static string Sanitize(string value)
	{
		return (value ?? "").Replace('/', '_').Replace(':', '_').Replace('.', '_')
			.Replace('@', '_');
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddWorkspace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenPalette, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrySelectWorkspace, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildGroupButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectGroup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "group", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayPaletteReveal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HidePalette, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveGroup, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ContainsAny, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "tokens", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Sanitize, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.AddWorkspace && args.Count == 3)
		{
			AddWorkspace(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Texture2D>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenPalette && args.Count == 0)
		{
			OpenPalette();
			ret = default;
			return true;
		}
		if (method == MethodName.TrySelectWorkspace && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TrySelectWorkspace(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildGroupButtons && args.Count == 0)
		{
			BuildGroupButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectGroup && args.Count == 1)
		{
			SelectGroup(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayPaletteReveal && args.Count == 0)
		{
			PlayPaletteReveal();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCards && args.Count == 0)
		{
			RefreshCards();
			ret = default;
			return true;
		}
		if (method == MethodName.HidePalette && args.Count == 0)
		{
			HidePalette();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveGroup && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveGroup(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ContainsAny && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsAny(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.Sanitize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(Sanitize(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveGroup && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveGroup(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ContainsAny && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsAny(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.Sanitize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(Sanitize(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.AddWorkspace)
		{
			return true;
		}
		if (method == MethodName.OpenPalette)
		{
			return true;
		}
		if (method == MethodName.TrySelectWorkspace)
		{
			return true;
		}
		if (method == MethodName.BuildGroupButtons)
		{
			return true;
		}
		if (method == MethodName.SelectGroup)
		{
			return true;
		}
		if (method == MethodName.PlayPaletteReveal)
		{
			return true;
		}
		if (method == MethodName.RefreshCards)
		{
			return true;
		}
		if (method == MethodName.HidePalette)
		{
			return true;
		}
		if (method == MethodName.ResolveGroup)
		{
			return true;
		}
		if (method == MethodName.ContainsAny)
		{
			return true;
		}
		if (method == MethodName.Sanitize)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.VisibleEntryCount)
		{
			VisibleEntryCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.MotionBindingCount)
		{
			MotionBindingCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._searchEdit)
		{
			_searchEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._groupFlow)
		{
			_groupFlow = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._cardGrid)
		{
			_cardGrid = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName._resultLabel)
		{
			_resultLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._emptyLabel)
		{
			_emptyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._surfaceMotion)
		{
			_surfaceMotion = VariantUtils.ConvertTo<XWUiMotion>(in value);
			return true;
		}
		if (name == PropertyName._activeGroup)
		{
			_activeGroup = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.EntryCount)
		{
			from = EntryCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.VisibleEntryCount)
		{
			from = VisibleEntryCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MotionBindingCount)
		{
			from = MotionBindingCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SurfaceMotion)
		{
			value = VariantUtils.CreateFrom<XWUiMotion>(SurfaceMotion);
			return true;
		}
		if (name == PropertyName._searchEdit)
		{
			value = VariantUtils.CreateFrom(in _searchEdit);
			return true;
		}
		if (name == PropertyName._groupFlow)
		{
			value = VariantUtils.CreateFrom(in _groupFlow);
			return true;
		}
		if (name == PropertyName._cardGrid)
		{
			value = VariantUtils.CreateFrom(in _cardGrid);
			return true;
		}
		if (name == PropertyName._resultLabel)
		{
			value = VariantUtils.CreateFrom(in _resultLabel);
			return true;
		}
		if (name == PropertyName._emptyLabel)
		{
			value = VariantUtils.CreateFrom(in _emptyLabel);
			return true;
		}
		if (name == PropertyName._surfaceMotion)
		{
			value = VariantUtils.CreateFrom(in _surfaceMotion);
			return true;
		}
		if (name == PropertyName._activeGroup)
		{
			value = VariantUtils.CreateFrom(in _activeGroup);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._searchEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._groupFlow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._surfaceMotion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._activeGroup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.EntryCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisibleEntryCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.MotionBindingCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.SurfaceMotion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.VisibleEntryCount, Variant.From<int>(VisibleEntryCount));
		info.AddProperty(PropertyName.MotionBindingCount, Variant.From<int>(MotionBindingCount));
		info.AddProperty(PropertyName._searchEdit, Variant.From(in _searchEdit));
		info.AddProperty(PropertyName._groupFlow, Variant.From(in _groupFlow));
		info.AddProperty(PropertyName._cardGrid, Variant.From(in _cardGrid));
		info.AddProperty(PropertyName._resultLabel, Variant.From(in _resultLabel));
		info.AddProperty(PropertyName._emptyLabel, Variant.From(in _emptyLabel));
		info.AddProperty(PropertyName._surfaceMotion, Variant.From(in _surfaceMotion));
		info.AddProperty(PropertyName._activeGroup, Variant.From(in _activeGroup));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.VisibleEntryCount, out var value))
		{
			VisibleEntryCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.MotionBindingCount, out var value2))
		{
			MotionBindingCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._searchEdit, out var value3))
		{
			_searchEdit = value3.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._groupFlow, out var value4))
		{
			_groupFlow = value4.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._cardGrid, out var value5))
		{
			_cardGrid = value5.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName._resultLabel, out var value6))
		{
			_resultLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._emptyLabel, out var value7))
		{
			_emptyLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._surfaceMotion, out var value8))
		{
			_surfaceMotion = value8.As<XWUiMotion>();
		}
		if (info.TryGetProperty(PropertyName._activeGroup, out var value9))
		{
			_activeGroup = value9.As<string>();
		}
	}
}
