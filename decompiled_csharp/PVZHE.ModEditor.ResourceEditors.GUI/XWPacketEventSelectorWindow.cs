using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketEventSelectorWindow.cs")]
public class XWPacketEventSelectorWindow : Window
{
	private sealed class EventChoice
	{
		public string DisplayName { get; init; } = "";

		public string EventName { get; init; } = "";

		public string ClassName { get; init; } = "";

		public string SourceLabel { get; init; } = "";

		public string AssemblyName { get; init; } = "";

		public string DetailText { get; init; } = "";

		public Type RuntimeType { get; init; }

		public bool CanSelect { get; init; } = true;

		public bool IsAction { get; init; }
	}

	public new class MethodName : Window.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName EnsureNodeReferences = "EnsureNodeReferences";

		public static readonly StringName ConnectSignals = "ConnectSignals";

		public static readonly StringName DisconnectSignals = "DisconnectSignals";

		public static readonly StringName OnSearchChanged = "OnSearchChanged";

		public static readonly StringName OnSearchSubmitted = "OnSearchSubmitted";

		public static readonly StringName RebuildFilteredRows = "RebuildFilteredRows";

		public static readonly StringName ClearPreview = "ClearPreview";

		public static readonly StringName ConfirmSelection = "ConfirmSelection";

		public static readonly StringName CloseSelector = "CloseSelector";

		public static readonly StringName FocusSearch = "FocusSearch";

		public static readonly StringName GetBehaviorParameterName = "GetBehaviorParameterName";

		public static readonly StringName HumanizeBehaviorTypeName = "HumanizeBehaviorTypeName";

		public static readonly StringName HumanizeIdentifier = "HumanizeIdentifier";
	}

	public new class PropertyName : Window.PropertyName
	{
		public static readonly StringName _searchEdit = "_searchEdit";

		public static readonly StringName _choiceList = "_choiceList";

		public static readonly StringName _emptyState = "_emptyState";

		public static readonly StringName _titleLabel = "_titleLabel";

		public static readonly StringName _subtitleLabel = "_subtitleLabel";

		public static readonly StringName _previewGlyph = "_previewGlyph";

		public static readonly StringName _eventNameLabel = "_eventNameLabel";

		public static readonly StringName _sourceBadge = "_sourceBadge";

		public static readonly StringName _classNameLabel = "_classNameLabel";

		public static readonly StringName _assemblyLabel = "_assemblyLabel";

		public static readonly StringName _detailLabel = "_detailLabel";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _cancelButton = "_cancelButton";

		public static readonly StringName _confirmButton = "_confirmButton";

		public static readonly StringName _signalsConnected = "_signalsConnected";

		public static readonly StringName _suppressSearchSignal = "_suppressSearchSignal";
	}

	public new class SignalName : Window.SignalName
	{
	}

	private const string SelectorRowScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCompactPacketEventSelectorRow.tscn";

	private static PackedScene _selectorRowScene;

	private static readonly HashSet<string> BehaviorIdentityProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "BehaviorTypeId", "DefinitionId", "InstanceId", "SchemaVersion", "InitiallyEnabled" };

	private static readonly Dictionary<string, string> BuiltInBehaviorNames = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		["CardActionBehaviorChangeCost"] = "修改卡牌费用",
		["CardActionBehaviorChangePacket"] = "替换卡牌",
		["CardActionBehaviorCreditSun"] = "获得阳光",
		["CardActionBehaviorDelete"] = "移除卡牌",
		["CardActionBehaviorSetCooldown"] = "设置卡牌冷却",
		["TowerDefensePacketChangeCost"] = "卡牌费用规则"
	};

	private static readonly Dictionary<string, string> BehaviorParameterNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		["triggerFlags"] = "触发时机",
		["method"] = "运算方式",
		["value"] = "数值",
		["_min"] = "最低值",
		["_max"] = "最高值",
		["levelPacketConfig"] = "目标卡牌",
		["packetConfig"] = "卡牌配置",
		["count"] = "触发次数",
		["amount"] = "阳光数量",
		["amontDictionary"] = "各卡牌类型数值",
		["key"] = "规则标识",
		["lockCost"] = "锁定费用",
		["skip"] = "停止后续规则"
	};

	private readonly List<EventChoice> _allChoices = new List<EventChoice>();

	private LineEdit _searchEdit;

	private VBoxContainer _choiceList;

	private Label _emptyState;

	private Label _titleLabel;

	private Label _subtitleLabel;

	private Label _previewGlyph;

	private Label _eventNameLabel;

	private Label _sourceBadge;

	private Label _classNameLabel;

	private Label _assemblyLabel;

	private Label _detailLabel;

	private Label _statusLabel;

	private Button _cancelButton;

	private Button _confirmButton;

	private EventChoice _selectedChoice;

	private Action<Type> _selectionCallback;

	private bool _signalsConnected;

	private bool _suppressSearchSignal;

	public override void _Ready()
	{
		EnsureNodeReferences();
		ConnectSignals();
	}

	public override void _ExitTree()
	{
		DisconnectSignals();
		_selectionCallback = null;
		base._ExitTree();
	}

	public void ShowFor(string title, string subtitle, Action<Type> onSelected)
	{
		ShowFor(title, subtitle, typeof(CardActionBehaviorDefinition), onSelected);
	}

	public void ShowFor(string title, string subtitle, Type targetBaseType, Action<Type> onSelected)
	{
		EnsureNodeReferences();
		ConnectSignals();
		Title = title;
		_titleLabel.Text = title;
		_subtitleLabel.Text = subtitle;
		_selectionCallback = onSelected;
		_allChoices.Clear();
		_allChoices.AddRange(CollectEventChoices(targetBaseType));
		_suppressSearchSignal = true;
		_searchEdit.Text = "";
		_suppressSearchSignal = false;
		RebuildFilteredRows("");
		PopupCenteredClamped(new Vector2I(960, 640), 0.9f);
		CallDeferred("FocusSearch");
	}

	private void EnsureNodeReferences()
	{
		if (_searchEdit == null)
		{
			_searchEdit = GetNode<LineEdit>("%SearchEdit");
		}
		if (_choiceList == null)
		{
			_choiceList = GetNode<VBoxContainer>("%ChoiceList");
		}
		if (_emptyState == null)
		{
			_emptyState = GetNode<Label>("%EmptyState");
		}
		if (_titleLabel == null)
		{
			_titleLabel = GetNode<Label>("%TitleLabel");
		}
		if (_subtitleLabel == null)
		{
			_subtitleLabel = GetNode<Label>("%SubtitleLabel");
		}
		if (_previewGlyph == null)
		{
			_previewGlyph = GetNode<Label>("%PreviewGlyph");
		}
		if (_eventNameLabel == null)
		{
			_eventNameLabel = GetNode<Label>("%EventNameLabel");
		}
		if (_sourceBadge == null)
		{
			_sourceBadge = GetNode<Label>("%SourceBadge");
		}
		if (_classNameLabel == null)
		{
			_classNameLabel = GetNode<Label>("%ClassNameLabel");
		}
		if (_assemblyLabel == null)
		{
			_assemblyLabel = GetNode<Label>("%AssemblyLabel");
		}
		if (_detailLabel == null)
		{
			_detailLabel = GetNode<Label>("%DetailLabel");
		}
		if (_statusLabel == null)
		{
			_statusLabel = GetNode<Label>("%StatusLabel");
		}
		if (_cancelButton == null)
		{
			_cancelButton = GetNode<Button>("%CancelButton");
		}
		if (_confirmButton == null)
		{
			_confirmButton = GetNode<Button>("%ConfirmButton");
		}
	}

	private void ConnectSignals()
	{
		if (!_signalsConnected)
		{
			_searchEdit.TextChanged += OnSearchChanged;
			_searchEdit.TextSubmitted += OnSearchSubmitted;
			_cancelButton.Pressed += CloseSelector;
			_confirmButton.Pressed += ConfirmSelection;
			CloseRequested += CloseSelector;
			_signalsConnected = true;
		}
	}

	private void DisconnectSignals()
	{
		if (_signalsConnected)
		{
			if (GodotObject.IsInstanceValid(_searchEdit))
			{
				_searchEdit.TextChanged -= OnSearchChanged;
				_searchEdit.TextSubmitted -= OnSearchSubmitted;
			}
			if (GodotObject.IsInstanceValid(_cancelButton))
			{
				_cancelButton.Pressed -= CloseSelector;
			}
			if (GodotObject.IsInstanceValid(_confirmButton))
			{
				_confirmButton.Pressed -= ConfirmSelection;
			}
			CloseRequested -= CloseSelector;
			_signalsConnected = false;
		}
	}

	private void OnSearchChanged(string query)
	{
		if (!_suppressSearchSignal)
		{
			RebuildFilteredRows(query);
		}
	}

	private void OnSearchSubmitted(string query)
	{
		if (GodotObject.IsInstanceValid(_confirmButton) && !_confirmButton.Disabled)
		{
			ConfirmSelection();
		}
	}

	private void RebuildFilteredRows(string query)
	{
		foreach (Node child in _choiceList.GetChildren())
		{
			_choiceList.RemoveChild(child);
			child.QueueFree();
		}
		if (_selectorRowScene == null)
		{
			_selectorRowScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCompactPacketEventSelectorRow.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		_selectedChoice = null;
		Control control = null;
		EventChoice eventChoice = null;
		int num = 0;
		string query2 = query?.Trim() ?? "";
		foreach (EventChoice choice in _allChoices)
		{
			if (!Matches(choice, query2))
			{
				continue;
			}
			Control row = _selectorRowScene?.Instantiate<Control>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(row))
			{
				continue;
			}
			row.GetNode<Label>("%EventGlyph").Text = GetChoiceGlyph(choice);
			row.GetNode<Label>("%EventName").Text = choice.DisplayName;
			row.GetNode<Label>("%ClassName").Text = choice.ClassName;
			row.GetNode<Label>("%SourceBadge").Text = choice.SourceLabel;
			Button node = row.GetNode<Button>("%SelectSurface");
			node.Disabled = !choice.CanSelect;
			node.TooltipText = (choice.CanSelect ? choice.DetailText : "该行为的无参数构造失败，已隔离，不能添加。");
			node.Pressed += () =>
			{
				SelectChoice(row, choice);
			};
			node.GuiInput += (InputEvent input) =>
			{
				OnRowGuiInput(input, row, choice);
			};
			_choiceList.AddChild(row, forceReadableName: false, InternalMode.Disabled);
			if (choice.CanSelect)
			{
				if (control == null)
				{
					control = row;
				}
				if (eventChoice == null)
				{
					eventChoice = choice;
				}
			}
			num++;
		}
		_emptyState.Visible = num == 0;
		_statusLabel.Text = ((num == 0) ? "没有匹配的行为；可按中文名称、类名、参数或来源搜索。" : $"已显示 {num} 个行为拼图 · 单击预览，双击添加");
		if (GodotObject.IsInstanceValid(control) && eventChoice != null)
		{
			SelectChoice(control, eventChoice);
		}
		else
		{
			ClearPreview();
		}
	}

	private void SelectChoice(Control selectedRow, EventChoice choice)
	{
		if (choice == null || !choice.CanSelect)
		{
			return;
		}
		_selectedChoice = choice;
		foreach (Node child in _choiceList.GetChildren())
		{
			if (child is Control control)
			{
				Button nodeOrNull = control.GetNodeOrNull<Button>("%SelectSurface");
				if (GodotObject.IsInstanceValid(nodeOrNull))
				{
					nodeOrNull.ButtonPressed = control == selectedRow;
				}
			}
		}
		_previewGlyph.Text = GetChoiceGlyph(choice);
		_eventNameLabel.Text = choice.DisplayName;
		_sourceBadge.Text = choice.SourceLabel;
		_classNameLabel.Text = "类名  " + choice.ClassName;
		_assemblyLabel.Text = "程序集  " + choice.AssemblyName;
		_detailLabel.Text = choice.DetailText;
		_confirmButton.Disabled = !choice.CanSelect;
	}

	private void OnRowGuiInput(InputEvent input, Control row, EventChoice choice)
	{
		if (input is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left && inputEventMouseButton.DoubleClick)
		{
			SelectChoice(row, choice);
			ConfirmSelection();
		}
	}

	private void ClearPreview()
	{
		_selectedChoice = null;
		_previewGlyph.Text = "◆";
		_eventNameLabel.Text = "没有可选择的行为";
		_sourceBadge.Text = "未选择";
		_classNameLabel.Text = "类名 —";
		_assemblyLabel.Text = "程序集 —";
		_detailLabel.Text = "调整搜索条件后，从左侧选择行为拼图。";
		_confirmButton.Disabled = true;
	}

	private void ConfirmSelection()
	{
		if (!(_selectedChoice?.RuntimeType == null) && _selectedChoice.CanSelect && !_confirmButton.Disabled)
		{
			Type runtimeType = _selectedChoice.RuntimeType;
			Action<Type> selectionCallback = _selectionCallback;
			_selectionCallback = null;
			Hide();
			selectionCallback?.Invoke(runtimeType);
		}
	}

	private void CloseSelector()
	{
		_selectionCallback = null;
		Hide();
	}

	private void FocusSearch()
	{
		if (Visible && GodotObject.IsInstanceValid(_searchEdit))
		{
			_searchEdit.GrabFocus();
		}
	}

	[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The in-game Mod editor discovers behavior classes from loaded game and Mod assemblies at runtime.")]
	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "The behavior catalog validates public parameterless constructors before previewing editor-only instances.")]
	private static List<EventChoice> CollectEventChoices()
	{
		return CollectEventChoices(typeof(CardActionBehaviorDefinition));
	}

	[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The in-game Mod editor discovers behavior classes from loaded game and Mod assemblies at runtime.")]
	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "The behavior catalog validates public parameterless constructors before previewing editor-only instances.")]
	private static List<EventChoice> CollectEventChoices(Type targetBaseType)
	{
		List<EventChoice> list = new List<EventChoice>();
		if (!IsSupportedBehaviorBaseType(targetBaseType))
		{
			GD.PushWarning("Behavior catalog rejected unsupported base type: " + (targetBaseType?.FullName ?? "<null>"));
			return list;
		}
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			foreach (Type loadableType in GetLoadableTypes(assemblies[i]))
			{
				if (!(loadableType == null) && !loadableType.IsAbstract && !loadableType.ContainsGenericParameters && !(loadableType == targetBaseType) && targetBaseType.IsAssignableFrom(loadableType) && !(loadableType.GetConstructor(Type.EmptyTypes) == null))
				{
					string item = loadableType.FullName ?? loadableType.Name;
					if (hashSet.Add(item))
					{
						list.Add(CreateChoice(loadableType));
					}
				}
			}
		}
		list.Sort((EventChoice left, EventChoice right) => string.Compare(left.DisplayName, right.DisplayName, StringComparison.OrdinalIgnoreCase));
		return list;
	}

	[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The selector intentionally scans loaded assemblies so Mod packet event classes need no hard-coded registry.")]
	private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
	{
		try
		{
			return assembly.GetTypes();
		}
		catch (ReflectionTypeLoadException ex)
		{
			List<Type> list = new List<Type>();
			Type[] types = ex.Types;
			foreach (Type type in types)
			{
				if (type != null)
				{
					list.Add(type);
				}
			}
			return list;
		}
		catch
		{
			return Array.Empty<Type>();
		}
	}

	private static bool IsSupportedBehaviorBaseType(Type targetBaseType)
	{
		if (targetBaseType != null)
		{
			return typeof(CardBehaviorDefinition).IsAssignableFrom(targetBaseType);
		}
		return false;
	}

	private static EventChoice CreateChoice(Type type)
	{
		bool flag = TryCreatePreviewInstance(type, out var preview);
		string behaviorName = GetBehaviorName(preview, type);
		string className = type.FullName ?? type.Name;
		string sourceLabel = ((type.Assembly == typeof(CardBehaviorDefinition).Assembly) ? "内置" : "Mod 脚本");
		string assemblyName = type.Assembly.GetName().Name ?? "未知程序集";
		bool isAction = typeof(CardActionBehaviorDefinition).IsAssignableFrom(type);
		string text = BuildParameterSummary(type);
		if (!flag)
		{
			text = "⚠ 构造预览失败，已安全隔离。" + text;
		}
		return new EventChoice
		{
			DisplayName = (string.Equals(behaviorName, type.Name, StringComparison.Ordinal) ? type.Name : (behaviorName + " · " + type.Name)),
			EventName = behaviorName,
			ClassName = className,
			SourceLabel = sourceLabel,
			AssemblyName = assemblyName,
			DetailText = text,
			RuntimeType = type,
			CanSelect = flag,
			IsAction = isAction
		};
	}

	[UnconditionalSuppressMessage("Trimming", "IL2067", Justification = "The selector creates editor-only previews from discovered runtime event types.")]
	private static bool TryCreatePreviewInstance(Type type, out CardBehaviorDefinition preview)
	{
		preview = null;
		try
		{
			preview = Activator.CreateInstance(type) as CardBehaviorDefinition;
			return GodotObject.IsInstanceValid(preview);
		}
		catch (Exception ex)
		{
			GD.PushWarning("Behavior catalog preview construction failed: " + type?.FullName + " " + ex.Message);
			return false;
		}
	}

	private static string GetBehaviorName(CardBehaviorDefinition behavior, Type type)
	{
		string text = type?.Name ?? "CardBehavior";
		if (BuiltInBehaviorNames.TryGetValue(text, out var value))
		{
			return value;
		}
		if (TryGetAuthoredDisplayName(behavior, type, out var displayName))
		{
			return displayName;
		}
		if (GodotObject.IsInstanceValid(behavior) && behavior.DefinitionId != null && !behavior.DefinitionId.IsEmpty)
		{
			return behavior.DefinitionId.ToString();
		}
		return HumanizeBehaviorTypeName(text);
	}

	[UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "The editor-only Mod behavior catalog deliberately reads optional public display-name fields from discovered C# behavior types.")]
	private static bool TryGetAuthoredDisplayName(CardBehaviorDefinition behavior, Type type, out string displayName)
	{
		displayName = "";
		if (!GodotObject.IsInstanceValid(behavior) || type == null)
		{
			return false;
		}
		string[] array = new string[4] { "eventName", "EventName", "displayName", "DisplayName" };
		foreach (string text in array)
		{
			try
			{
				object obj = type.GetField(text, BindingFlags.Instance | BindingFlags.Public)?.GetValue(behavior);
				if (obj == null)
				{
					System.Reflection.PropertyInfo property = type.GetProperty(text, BindingFlags.Instance | BindingFlags.Public);
					obj = (((object)property != null && property.GetIndexParameters().Length == 0) ? property.GetValue(behavior) : null);
				}
				string text2 = obj?.ToString()?.Trim() ?? "";
				if (!string.IsNullOrWhiteSpace(text2))
				{
					displayName = text2;
					return true;
				}
			}
			catch (Exception ex)
			{
				GD.PushWarning($"Behavior catalog display-name read failed: {type.FullName}.{text} {ex.Message}");
			}
		}
		if (!string.IsNullOrWhiteSpace(behavior.ResourceName))
		{
			displayName = behavior.ResourceName.Trim();
			return true;
		}
		return false;
	}

	[UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "The editor-only behavior catalog deliberately reads exported public authoring fields and properties from discovered Mod behavior types.")]
	private static string BuildParameterSummary(Type behaviorType)
	{
		try
		{
			List<string> list = new List<string>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			FieldInfo[] fields = behaviorType.GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.GetCustomAttribute<ExportAttribute>(inherit: true) != null && !BehaviorIdentityProperties.Contains(fieldInfo.Name) && hashSet.Add(fieldInfo.Name))
				{
					list.Add(GetBehaviorParameterName(fieldInfo.Name));
				}
			}
			System.Reflection.PropertyInfo[] properties = behaviorType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (System.Reflection.PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.GetCustomAttribute<ExportAttribute>(inherit: true) != null && !BehaviorIdentityProperties.Contains(propertyInfo.Name) && hashSet.Add(propertyInfo.Name))
				{
					list.Add(GetBehaviorParameterName(propertyInfo.Name));
				}
			}
			return (list.Count == 0) ? "该行为没有额外参数；添加后会作为一块执行拼图显示。" : ("可配置参数  " + string.Join(" · ", list));
		}
		catch (Exception ex)
		{
			GD.PushWarning("Behavior catalog parameter scan failed: " + behaviorType?.FullName + " " + ex.Message);
			return "参数摘要不可用；添加后仍可在可视化行为编辑器中配置。";
		}
	}

	private static string GetBehaviorParameterName(string name)
	{
		if (!BehaviorParameterNames.TryGetValue(name ?? "", out var value))
		{
			return HumanizeIdentifier(name);
		}
		return value;
	}

	private static string HumanizeBehaviorTypeName(string typeName)
	{
		string text = typeName ?? "CardBehavior";
		string[] array = new string[3] { "TowerDefensePacket", "CardActionBehavior", "CardBehavior" };
		foreach (string text2 in array)
		{
			if (text.StartsWith(text2, StringComparison.Ordinal) && text.Length > text2.Length)
			{
				string text3 = text;
				int length = text2.Length;
				text = text3.Substring(length, text3.Length - length);
				break;
			}
		}
		return HumanizeIdentifier(text);
	}

	private static string HumanizeIdentifier(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return "未命名行为";
		}
		StringBuilder stringBuilder = new StringBuilder(value.Length + 8);
		char c = '\0';
		string text = value.TrimStart('_');
		foreach (char c2 in text)
		{
			if (stringBuilder.Length > 0 && char.IsUpper(c2) && (char.IsLower(c) || char.IsDigit(c)))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append(c2);
			c = c2;
		}
		return stringBuilder.ToString();
	}

	private static string GetChoiceGlyph(EventChoice choice)
	{
		if (choice?.SourceLabel != "内置")
		{
			return "M";
		}
		if (!choice.IsAction)
		{
			return "◇";
		}
		return "◆";
	}

	private static bool Matches(EventChoice choice, string query)
	{
		if (string.IsNullOrWhiteSpace(query))
		{
			return true;
		}
		if (!choice.DisplayName.Contains(query, StringComparison.OrdinalIgnoreCase) && !choice.EventName.Contains(query, StringComparison.OrdinalIgnoreCase) && !choice.ClassName.Contains(query, StringComparison.OrdinalIgnoreCase) && !choice.SourceLabel.Contains(query, StringComparison.OrdinalIgnoreCase) && !choice.AssemblyName.Contains(query, StringComparison.OrdinalIgnoreCase))
		{
			return choice.DetailText.Contains(query, StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(15)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName._ExitTree, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.EnsureNodeReferences, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ConnectSignals, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.DisconnectSignals, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnSearchChanged, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnSearchSubmitted, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RebuildFilteredRows, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ClearPreview, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ConfirmSelection, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CloseSelector, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.FocusSearch, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.GetBehaviorParameterName, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.HumanizeBehaviorTypeName, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "typeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.HumanizeIdentifier, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureNodeReferences && args.Count == 0)
		{
			EnsureNodeReferences();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectSignals && args.Count == 0)
		{
			ConnectSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectSignals && args.Count == 0)
		{
			DisconnectSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSearchChanged && args.Count == 1)
		{
			OnSearchChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSearchSubmitted && args.Count == 1)
		{
			OnSearchSubmitted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildFilteredRows && args.Count == 1)
		{
			RebuildFilteredRows(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPreview && args.Count == 0)
		{
			ClearPreview();
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
		if (method == MethodName.FocusSearch && args.Count == 0)
		{
			FocusSearch();
			ret = default;
			return true;
		}
		if (method == MethodName.GetBehaviorParameterName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetBehaviorParameterName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HumanizeBehaviorTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(HumanizeBehaviorTypeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HumanizeIdentifier && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(HumanizeIdentifier(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetBehaviorParameterName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetBehaviorParameterName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HumanizeBehaviorTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(HumanizeBehaviorTypeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HumanizeIdentifier && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(HumanizeIdentifier(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.EnsureNodeReferences)
		{
			return true;
		}
		if (method == MethodName.ConnectSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectSignals)
		{
			return true;
		}
		if (method == MethodName.OnSearchChanged)
		{
			return true;
		}
		if (method == MethodName.OnSearchSubmitted)
		{
			return true;
		}
		if (method == MethodName.RebuildFilteredRows)
		{
			return true;
		}
		if (method == MethodName.ClearPreview)
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
		if (method == MethodName.FocusSearch)
		{
			return true;
		}
		if (method == MethodName.GetBehaviorParameterName)
		{
			return true;
		}
		if (method == MethodName.HumanizeBehaviorTypeName)
		{
			return true;
		}
		if (method == MethodName.HumanizeIdentifier)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._searchEdit)
		{
			_searchEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._choiceList)
		{
			_choiceList = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._emptyState)
		{
			_emptyState = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			_titleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._subtitleLabel)
		{
			_subtitleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewGlyph)
		{
			_previewGlyph = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._eventNameLabel)
		{
			_eventNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._sourceBadge)
		{
			_sourceBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._classNameLabel)
		{
			_classNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._assemblyLabel)
		{
			_assemblyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._detailLabel)
		{
			_detailLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._cancelButton)
		{
			_cancelButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._confirmButton)
		{
			_confirmButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._signalsConnected)
		{
			_signalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._suppressSearchSignal)
		{
			_suppressSearchSignal = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._searchEdit)
		{
			value = VariantUtils.CreateFrom(in _searchEdit);
			return true;
		}
		if (name == PropertyName._choiceList)
		{
			value = VariantUtils.CreateFrom(in _choiceList);
			return true;
		}
		if (name == PropertyName._emptyState)
		{
			value = VariantUtils.CreateFrom(in _emptyState);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			value = VariantUtils.CreateFrom(in _titleLabel);
			return true;
		}
		if (name == PropertyName._subtitleLabel)
		{
			value = VariantUtils.CreateFrom(in _subtitleLabel);
			return true;
		}
		if (name == PropertyName._previewGlyph)
		{
			value = VariantUtils.CreateFrom(in _previewGlyph);
			return true;
		}
		if (name == PropertyName._eventNameLabel)
		{
			value = VariantUtils.CreateFrom(in _eventNameLabel);
			return true;
		}
		if (name == PropertyName._sourceBadge)
		{
			value = VariantUtils.CreateFrom(in _sourceBadge);
			return true;
		}
		if (name == PropertyName._classNameLabel)
		{
			value = VariantUtils.CreateFrom(in _classNameLabel);
			return true;
		}
		if (name == PropertyName._assemblyLabel)
		{
			value = VariantUtils.CreateFrom(in _assemblyLabel);
			return true;
		}
		if (name == PropertyName._detailLabel)
		{
			value = VariantUtils.CreateFrom(in _detailLabel);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._cancelButton)
		{
			value = VariantUtils.CreateFrom(in _cancelButton);
			return true;
		}
		if (name == PropertyName._confirmButton)
		{
			value = VariantUtils.CreateFrom(in _confirmButton);
			return true;
		}
		if (name == PropertyName._signalsConnected)
		{
			value = VariantUtils.CreateFrom(in _signalsConnected);
			return true;
		}
		if (name == PropertyName._suppressSearchSignal)
		{
			value = VariantUtils.CreateFrom(in _suppressSearchSignal);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._searchEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._choiceList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._emptyState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._titleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._subtitleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previewGlyph, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._eventNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._sourceBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._classNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._assemblyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._detailLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._cancelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._confirmButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._signalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._suppressSearchSignal, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._searchEdit, Variant.From(in _searchEdit));
		info.AddProperty(PropertyName._choiceList, Variant.From(in _choiceList));
		info.AddProperty(PropertyName._emptyState, Variant.From(in _emptyState));
		info.AddProperty(PropertyName._titleLabel, Variant.From(in _titleLabel));
		info.AddProperty(PropertyName._subtitleLabel, Variant.From(in _subtitleLabel));
		info.AddProperty(PropertyName._previewGlyph, Variant.From(in _previewGlyph));
		info.AddProperty(PropertyName._eventNameLabel, Variant.From(in _eventNameLabel));
		info.AddProperty(PropertyName._sourceBadge, Variant.From(in _sourceBadge));
		info.AddProperty(PropertyName._classNameLabel, Variant.From(in _classNameLabel));
		info.AddProperty(PropertyName._assemblyLabel, Variant.From(in _assemblyLabel));
		info.AddProperty(PropertyName._detailLabel, Variant.From(in _detailLabel));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._cancelButton, Variant.From(in _cancelButton));
		info.AddProperty(PropertyName._confirmButton, Variant.From(in _confirmButton));
		info.AddProperty(PropertyName._signalsConnected, Variant.From(in _signalsConnected));
		info.AddProperty(PropertyName._suppressSearchSignal, Variant.From(in _suppressSearchSignal));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._searchEdit, out var value))
		{
			_searchEdit = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._choiceList, out var value2))
		{
			_choiceList = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._emptyState, out var value3))
		{
			_emptyState = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._titleLabel, out var value4))
		{
			_titleLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._subtitleLabel, out var value5))
		{
			_subtitleLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewGlyph, out var value6))
		{
			_previewGlyph = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._eventNameLabel, out var value7))
		{
			_eventNameLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._sourceBadge, out var value8))
		{
			_sourceBadge = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._classNameLabel, out var value9))
		{
			_classNameLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._assemblyLabel, out var value10))
		{
			_assemblyLabel = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._detailLabel, out var value11))
		{
			_detailLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value12))
		{
			_statusLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._cancelButton, out var value13))
		{
			_cancelButton = value13.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._confirmButton, out var value14))
		{
			_confirmButton = value14.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._signalsConnected, out var value15))
		{
			_signalsConnected = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._suppressSearchSignal, out var value16))
		{
			_suppressSearchSignal = value16.As<bool>();
		}
	}
}
