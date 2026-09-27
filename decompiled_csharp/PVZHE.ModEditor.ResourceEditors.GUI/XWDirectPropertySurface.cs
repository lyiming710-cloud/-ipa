using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.Registry;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/XWDirectPropertySurface.cs")]
public class XWDirectPropertySurface : PanelContainer
{
	private sealed class DirectRow
	{
		public string SearchText = "";

		public Control Control;
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName RefreshValues = "RefreshValues";

		public static readonly StringName RefreshEditorTree = "RefreshEditorTree";

		public static readonly StringName BuildChrome = "BuildChrome";

		public static readonly StringName Rebuild = "Rebuild";

		public static readonly StringName MountVisualEnumControl = "MountVisualEnumControl";

		public static readonly StringName DisposeVisualEnumControls = "DisposeVisualEnumControls";

		public static readonly StringName CreateGroupCard = "CreateGroupCard";

		public static readonly StringName OnEditorValueChanged = "OnEditorValueChanged";

		public static readonly StringName ApplySearch = "ApplySearch";

		public static readonly StringName HasVisiblePropertyEditor = "HasVisiblePropertyEditor";

		public static readonly StringName SetPropertyEditorVisibility = "SetPropertyEditorVisibility";

		public static readonly StringName UpdateCoverageLabel = "UpdateCoverageLabel";

		public static readonly StringName UpdateSurfaceTitle = "UpdateSurfaceTitle";

		public static readonly StringName FirstHintPart = "FirstHintPart";

		public static readonly StringName BuildPropertyLabel = "BuildPropertyLabel";

		public static readonly StringName FirstNonEmpty = "FirstNonEmpty";

		public static readonly StringName SanitizeNodeName = "SanitizeNodeName";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName EditablePropertyCount = "EditablePropertyCount";

		public static readonly StringName MountedPropertyCount = "MountedPropertyCount";

		public static readonly StringName MissingPropertyCount = "MissingPropertyCount";

		public static readonly StringName SpecializedPropertyCount = "SpecializedPropertyCount";

		public static readonly StringName DirectControlCount = "DirectControlCount";

		public static readonly StringName VisualizedEnumPropertyCount = "VisualizedEnumPropertyCount";

		public static readonly StringName SegmentedEnumPropertyCount = "SegmentedEnumPropertyCount";

		public static readonly StringName GalleryEnumPropertyCount = "GalleryEnumPropertyCount";

		public static readonly StringName VisibleRawEnumOptionCount = "VisibleRawEnumOptionCount";

		public static readonly StringName _object = "_object";

		public static readonly StringName _undoRedo = "_undoRedo";

		public static readonly StringName _root = "_root";

		public static readonly StringName _groupFlow = "_groupFlow";

		public static readonly StringName _search = "_search";

		public static readonly StringName _titleLabel = "_titleLabel";

		public static readonly StringName _coverageLabel = "_coverageLabel";

		public static readonly StringName _ready = "_ready";

		public static readonly StringName _includeScript = "_includeScript";

		public static readonly StringName _historyTypeOverride = "_historyTypeOverride";

		public static readonly StringName _schemaSignature = "_schemaSignature";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private GodotObject _object;

	private XWUndoRedoManager _undoRedo;

	private VBoxContainer _root;

	private HFlowContainer _groupFlow;

	private LineEdit _search;

	private Label _titleLabel;

	private Label _coverageLabel;

	private bool _ready;

	private bool _includeScript;

	private int _historyTypeOverride = -1;

	private string _schemaSignature = "";

	private readonly List<DirectRow> _rows = new List<DirectRow>();

	private readonly List<PanelContainer> _cards = new List<PanelContainer>();

	private readonly List<XWVisualSegmentedOption> _visualEnumSegments = new List<XWVisualSegmentedOption>();

	private readonly List<XWVisualOptionGallery> _visualEnumGalleries = new List<XWVisualOptionGallery>();

	private readonly List<OptionButton> _visualEnumSources = new List<OptionButton>();

	private readonly List<string> _editablePropertyNames = new List<string>();

	private readonly List<string> _missingPropertyNames = new List<string>();

	private readonly HashSet<string> _specializedPropertyNames = new HashSet<string>(StringComparer.Ordinal);

	public int EditablePropertyCount => _editablePropertyNames.Count;

	public int MountedPropertyCount => _editablePropertyNames.Count - _missingPropertyNames.Count;

	public int MissingPropertyCount => _missingPropertyNames.Count;

	public int SpecializedPropertyCount => _specializedPropertyNames.Count;

	public int DirectControlCount => _rows.Count;

	public int VisualizedEnumPropertyCount => _visualEnumSegments.Count + _visualEnumGalleries.Count;

	public int SegmentedEnumPropertyCount => _visualEnumSegments.Count;

	public int GalleryEnumPropertyCount => _visualEnumGalleries.Count;

	public int VisibleRawEnumOptionCount
	{
		get
		{
			int num = 0;
			foreach (OptionButton visualEnumSource in _visualEnumSources)
			{
				if (GodotObject.IsInstanceValid(visualEnumSource) && visualEnumSource.Visible)
				{
					num++;
				}
			}
			return num;
		}
	}

	public IReadOnlyList<string> EditablePropertyNames => _editablePropertyNames;

	public IReadOnlyList<string> MissingPropertyNames => _missingPropertyNames;

	public event Action<GodotObject, StringName, StringName, Variant> PropertyEdited;

	public override void _Ready()
	{
		BuildChrome();
		_ready = true;
		Rebuild();
	}

	public override void _ExitTree()
	{
		DisposeVisualEnumControls();
	}

	public void BindResource(Resource resource, XWUndoRedoManager undoRedo, IReadOnlyCollection<string> specializedPropertyNames = null)
	{
		BindObject(resource, undoRedo, specializedPropertyNames);
	}

	public void BindObject(GodotObject target, XWUndoRedoManager undoRedo, IReadOnlyCollection<string> specializedPropertyNames = null, int historyTypeOverride = -1, bool includeScript = false)
	{
		bool flag = GodotObject.IsInstanceValid(_object) && GodotObject.IsInstanceValid(target) && _object.GetInstanceId() == target.GetInstanceId();
		HashSet<string> hashSet = ((specializedPropertyNames == null) ? new HashSet<string>(StringComparer.Ordinal) : new HashSet<string>(specializedPropertyNames, StringComparer.Ordinal));
		string text = ComputeSchemaSignature(target, hashSet, includeScript);
		bool flag2 = _includeScript != includeScript || _historyTypeOverride != historyTypeOverride;
		_object = target;
		_undoRedo = undoRedo;
		_includeScript = includeScript;
		_historyTypeOverride = historyTypeOverride;
		_specializedPropertyNames.Clear();
		_specializedPropertyNames.UnionWith(hashSet);
		if (_ready)
		{
			if (flag && !flag2 && text == _schemaSignature)
			{
				RefreshValues();
			}
			else
			{
				Rebuild();
			}
		}
	}

	public void RefreshValues()
	{
		if (!GodotObject.IsInstanceValid(_groupFlow))
		{
			return;
		}
		foreach (Node child in _groupFlow.GetChildren())
		{
			RefreshEditorTree(child);
		}
		foreach (XWVisualSegmentedOption visualEnumSegment in _visualEnumSegments)
		{
			visualEnumSegment.RefreshSelection();
		}
		foreach (XWVisualOptionGallery visualEnumGallery in _visualEnumGalleries)
		{
			visualEnumGallery.RefreshSelection();
		}
	}

	private static void RefreshEditorTree(Node node)
	{
		if (node is XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase)
		{
			xWInspectorPropertyEditorBase.UpdateValueSafely();
		}
		foreach (Node child in node.GetChildren())
		{
			RefreshEditorTree(child);
		}
	}

	private void BuildChrome()
	{
		SizeFlagsHorizontal = SizeFlags.ExpandFill;
		StyleBoxFlat stylebox = new StyleBoxFlat
		{
			BgColor = new Color(0.035f, 0.055f, 0.041f, 0.98f),
			BorderColor = new Color(0.27f, 0.48f, 0.22f, 0.9f),
			BorderWidthLeft = 2,
			BorderWidthTop = 2,
			BorderWidthRight = 2,
			BorderWidthBottom = 2,
			CornerRadiusTopLeft = 10,
			CornerRadiusTopRight = 10,
			CornerRadiusBottomLeft = 10,
			CornerRadiusBottomRight = 10
		};
		AddThemeStyleboxOverride("panel", stylebox);
		MarginContainer marginContainer = new MarginContainer();
		marginContainer.AddThemeConstantOverride("margin_left", 14);
		marginContainer.AddThemeConstantOverride("margin_top", 12);
		marginContainer.AddThemeConstantOverride("margin_right", 14);
		marginContainer.AddThemeConstantOverride("margin_bottom", 14);
		AddChild(marginContainer, forceReadableName: false, InternalMode.Disabled);
		_root = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_root.AddThemeConstantOverride("separation", 10);
		marginContainer.AddChild(_root, forceReadableName: false, InternalMode.Disabled);
		HFlowContainer hFlowContainer = new HFlowContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hFlowContainer.AddThemeConstantOverride("separation", 10);
		_root.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
		_titleLabel = new Label
		{
			Text = "◆ 直接属性拼图",
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			TooltipText = "所有作者可编辑属性都直接挂在主画面；右侧原始 Inspector 不参与资源编辑。"
		};
		_titleLabel.AddThemeFontSizeOverride("font_size", 19);
		_titleLabel.AddThemeColorOverride("font_color", new Color(0.82f, 1f, 0.67f));
		hFlowContainer.AddChild(_titleLabel, forceReadableName: false, InternalMode.Disabled);
		_coverageLabel = new Label
		{
			Text = "0 / 0",
			TooltipText = "已挂载直接控件 / 可编辑属性"
		};
		_coverageLabel.AddThemeColorOverride("font_color", new Color(0.98f, 0.8f, 0.34f));
		hFlowContainer.AddChild(_coverageLabel, forceReadableName: false, InternalMode.Disabled);
		_search = new LineEdit
		{
			CustomMinimumSize = new Vector2(220f, 34f),
			PlaceholderText = "搜索属性卡牌…",
			ClearButtonEnabled = true
		};
		_search.TextChanged += ApplySearch;
		hFlowContainer.AddChild(_search, forceReadableName: false, InternalMode.Disabled);
		_groupFlow = new HFlowContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			Alignment = FlowContainer.AlignmentMode.Begin
		};
		_groupFlow.AddThemeConstantOverride("h_separation", 10);
		_groupFlow.AddThemeConstantOverride("v_separation", 10);
		_root.AddChild(_groupFlow, forceReadableName: false, InternalMode.Disabled);
	}

	private void Rebuild()
	{
		if (!GodotObject.IsInstanceValid(_groupFlow))
		{
			return;
		}
		DisposeVisualEnumControls();
		foreach (Node child in _groupFlow.GetChildren())
		{
			child.QueueFree();
		}
		_rows.Clear();
		_cards.Clear();
		_editablePropertyNames.Clear();
		_missingPropertyNames.Clear();
		_schemaSignature = ComputeSchemaSignature(_object, _specializedPropertyNames, _includeScript);
		UpdateSurfaceTitle();
		if (!GodotObject.IsInstanceValid(_object))
		{
			Visible = false;
			UpdateCoverageLabel();
			return;
		}
		Visible = true;
		System.Collections.Generic.Dictionary<string, VBoxContainer> dictionary = new System.Collections.Generic.Dictionary<string, VBoxContainer>(StringComparer.Ordinal);
		string text = "";
		string text2 = "";
		string text3 = "";
		string groupBase = "";
		string subgroupBase = "";
		foreach (Dictionary property2 in _object.GetPropertyList())
		{
			if (!property2.ContainsKey("name") || !property2.ContainsKey("usage"))
			{
				continue;
			}
			string text4 = property2["name"].AsString();
			PropertyUsageFlags propertyUsageFlags = (PropertyUsageFlags)property2["usage"].AsInt64();
			string hintString = (property2.ContainsKey("hint_string") ? property2["hint_string"].AsString() : "");
			if ((propertyUsageFlags & PropertyUsageFlags.Category) != PropertyUsageFlags.None)
			{
				bool flag = ((text4 == "Object" || text4 == "Resource") ? true : false);
				text = (flag ? "" : text4);
				text2 = (text3 = (groupBase = (subgroupBase = "")));
			}
			else if ((propertyUsageFlags & PropertyUsageFlags.Group) != PropertyUsageFlags.None)
			{
				text2 = text4;
				text3 = (subgroupBase = "");
				groupBase = FirstHintPart(hintString);
			}
			else if ((propertyUsageFlags & PropertyUsageFlags.Subgroup) != PropertyUsageFlags.None)
			{
				text3 = text4;
				subgroupBase = FirstHintPart(hintString);
			}
			else
			{
				if ((propertyUsageFlags & PropertyUsageFlags.Editor) == PropertyUsageFlags.None || (propertyUsageFlags & PropertyUsageFlags.ReadOnly) != PropertyUsageFlags.None || (text4 == "script" && !_includeScript) || text4.StartsWith("metadata/_", StringComparison.Ordinal) || text4.StartsWith("_", StringComparison.Ordinal))
				{
					continue;
				}
				_editablePropertyNames.Add(text4);
				if (_specializedPropertyNames.Contains(text4))
				{
					continue;
				}
				PropertyHint propertyHint = (PropertyHint)(property2.ContainsKey("hint") ? property2["hint"].AsInt32() : 0);
				XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase;
				try
				{
					xWInspectorPropertyEditorBase = XWTypeRegistry.Instance.GetEditorFromObjectProperty(_object, text4, (int)propertyHint, hintString);
				}
				catch
				{
					xWInspectorPropertyEditorBase = null;
				}
				if (!GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase))
				{
					_missingPropertyNames.Add(text4);
					continue;
				}
				string title = FirstNonEmpty(text3, text2, text, "基础属性");
				string key = $"{text}/{text2}/{text3}";
				if (!dictionary.TryGetValue(key, out var value))
				{
					value = (dictionary[key] = CreateGroupCard(title, text));
				}
				string text5 = BuildPropertyLabel(text4, text3, subgroupBase, text2, groupBase);
				XWInspectorProperty property = new XWInspectorProperty(_object, text4, default, text2, text3, propertyHint, hintString)
				{
					CategoryName = text,
					LabelOverride = text5,
					PropertyUsage = (long)propertyUsageFlags,
					ReadOnly = ((propertyUsageFlags & PropertyUsageFlags.ReadOnly) != 0),
					Description = (property2.ContainsKey("description") ? property2["description"].AsString() : "")
				};
				string text6 = SanitizeNodeName(text4);
				VBoxContainer vBoxContainer2 = new VBoxContainer
				{
					Name = "DirectRow_" + text6,
					SizeFlagsHorizontal = SizeFlags.ExpandFill
				};
				vBoxContainer2.AddThemeConstantOverride("separation", 4);
				value.AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
				xWInspectorPropertyEditorBase.Name = "Direct_" + text6;
				xWInspectorPropertyEditorBase.SizeFlagsHorizontal = SizeFlags.ExpandFill;
				xWInspectorPropertyEditorBase.UndoRedoManager = _undoRedo;
				xWInspectorPropertyEditorBase.HistoryTypeOverride = _historyTypeOverride;
				xWInspectorPropertyEditorBase.SetSplitRatio(0.34f);
				vBoxContainer2.AddChild(xWInspectorPropertyEditorBase, forceReadableName: false, InternalMode.Disabled);
				xWInspectorPropertyEditorBase.SetEditProperty(property);
				xWInspectorPropertyEditorBase.ValueChanged += OnEditorValueChanged;
				MountVisualEnumControl(xWInspectorPropertyEditorBase, vBoxContainer2, text6);
				_rows.Add(new DirectRow
				{
					Control = vBoxContainer2,
					SearchText = $"{text} {text2} {text3} {text4} {text5}".ToLowerInvariant()
				});
			}
		}
		UpdateCoverageLabel();
		ApplySearch(GodotObject.IsInstanceValid(_search) ? _search.Text : "");
	}

	private void MountVisualEnumControl(XWInspectorPropertyEditorBase editor, VBoxContainer rowHost, string safePropertyName)
	{
		if (!(editor is XWInspectorPropertyEditorEnum))
		{
			return;
		}
		OptionButton optionButton = editor.FindChild("OptionButton", recursive: true, owned: false) as OptionButton;
		if (GodotObject.IsInstanceValid(optionButton) && optionButton.ItemCount > 0)
		{
			HFlowContainer hFlowContainer = new HFlowContainer
			{
				Name = "DirectVisualEnum_" + safePropertyName,
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			hFlowContainer.AddThemeConstantOverride("h_separation", 6);
			hFlowContainer.AddThemeConstantOverride("v_separation", 6);
			Node parent = optionButton.GetParent();
			if (GodotObject.IsInstanceValid(parent))
			{
				parent.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
			}
			else
			{
				rowHost.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
			}
			_visualEnumSources.Add(optionButton);
			if (optionButton.ItemCount <= 6)
			{
				XWVisualSegmentedOption xWVisualSegmentedOption = new XWVisualSegmentedOption(optionButton, hFlowContainer);
				xWVisualSegmentedOption.Rebuild();
				_visualEnumSegments.Add(xWVisualSegmentedOption);
			}
			else
			{
				XWVisualOptionGallery xWVisualOptionGallery = new XWVisualOptionGallery(optionButton, hFlowContainer, "DirectEnumGallery_" + safePropertyName);
				xWVisualOptionGallery.Rebuild();
				_visualEnumGalleries.Add(xWVisualOptionGallery);
			}
		}
	}

	private void DisposeVisualEnumControls()
	{
		foreach (XWVisualSegmentedOption visualEnumSegment in _visualEnumSegments)
		{
			visualEnumSegment.Dispose();
		}
		foreach (XWVisualOptionGallery visualEnumGallery in _visualEnumGalleries)
		{
			visualEnumGallery.Dispose();
		}
		_visualEnumSegments.Clear();
		_visualEnumGalleries.Clear();
		_visualEnumSources.Clear();
	}

	private VBoxContainer CreateGroupCard(string title, string category)
	{
		PanelContainer panelContainer = new PanelContainer
		{
			CustomMinimumSize = new Vector2(430f, 0f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		StyleBoxFlat stylebox = new StyleBoxFlat
		{
			BgColor = new Color(0.055f, 0.075f, 0.059f, 0.96f),
			BorderColor = new Color(0.2f, 0.34f, 0.18f, 0.86f),
			BorderWidthLeft = 1,
			BorderWidthTop = 1,
			BorderWidthRight = 1,
			BorderWidthBottom = 1,
			CornerRadiusTopLeft = 7,
			CornerRadiusTopRight = 7,
			CornerRadiusBottomLeft = 7,
			CornerRadiusBottomRight = 7
		};
		panelContainer.AddThemeStyleboxOverride("panel", stylebox);
		panelContainer.TooltipText = (category + " " + title).ToLowerInvariant();
		_groupFlow.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		_cards.Add(panelContainer);
		MarginContainer marginContainer = new MarginContainer();
		marginContainer.AddThemeConstantOverride("margin_left", 9);
		marginContainer.AddThemeConstantOverride("margin_top", 8);
		marginContainer.AddThemeConstantOverride("margin_right", 9);
		marginContainer.AddThemeConstantOverride("margin_bottom", 9);
		panelContainer.AddChild(marginContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 3);
		marginContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		Label label = new Label
		{
			Text = "◆ " + title,
			TooltipText = category
		};
		label.AddThemeFontSizeOverride("font_size", 16);
		label.AddThemeColorOverride("font_color", new Color(0.92f, 0.78f, 0.34f));
		vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private void OnEditorValueChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		PropertyEdited?.Invoke(obj, property, field, value);
	}

	private void ApplySearch(string query)
	{
		string text = (query ?? "").Trim().ToLowerInvariant();
		foreach (DirectRow row in _rows)
		{
			if (GodotObject.IsInstanceValid(row.Control))
			{
				row.Control.Visible = text.Length == 0 || row.SearchText.Contains(text, StringComparison.Ordinal);
			}
		}
		foreach (PanelContainer card in _cards)
		{
			if (GodotObject.IsInstanceValid(card))
			{
				bool flag = text.Length == 0 || card.TooltipText.Contains(text, StringComparison.Ordinal);
				if (flag && text.Length > 0)
				{
					SetPropertyEditorVisibility(card, visible: true);
				}
				card.Visible = flag || HasVisiblePropertyEditor(card);
			}
		}
	}

	private static bool HasVisiblePropertyEditor(Node node)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is XWInspectorPropertyEditorBase { Visible: not false })
			{
				return true;
			}
			if (HasVisiblePropertyEditor(child))
			{
				return true;
			}
		}
		return false;
	}

	private static void SetPropertyEditorVisibility(Node node, bool visible)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase)
			{
				xWInspectorPropertyEditorBase.Visible = visible;
			}
			SetPropertyEditorVisibility(child, visible);
		}
	}

	private void UpdateCoverageLabel()
	{
		if (GodotObject.IsInstanceValid(_coverageLabel))
		{
			_coverageLabel.Text = ((SpecializedPropertyCount > 0) ? $"{MountedPropertyCount} / {EditablePropertyCount} · 专用 {SpecializedPropertyCount}" : $"{MountedPropertyCount} / {EditablePropertyCount}");
			Label coverageLabel = _coverageLabel;
			string tooltipText;
			if (MissingPropertyCount != 0)
			{
				tooltipText = "缺少直接控件：" + string.Join(", ", _missingPropertyNames);
			}
			else
			{
				tooltipText = ((SpecializedPropertyCount > 0) ? "当前资源全部作者属性均有主画面控件；专用就地控件不会重复出现在属性拼图" : "当前资源全部作者属性均有主画面直接控件");
			}
			coverageLabel.TooltipText = tooltipText;
			_coverageLabel.AddThemeColorOverride("font_color", (MissingPropertyCount == 0) ? new Color(0.52f, 0.92f, 0.42f) : new Color(1f, 0.38f, 0.26f));
		}
	}

	private void UpdateSurfaceTitle()
	{
		if (GodotObject.IsInstanceValid(_titleLabel))
		{
			if (_object is Node node && GodotObject.IsInstanceValid(node))
			{
				_titleLabel.Text = $"◆ {node.Name} · 节点属性拼图";
				_titleLabel.TooltipText = "节点全部作者属性直接位于 2D 主工作区；右侧原始 Inspector 保持原对象不变。";
			}
			else
			{
				_titleLabel.Text = "◆ 直接属性拼图";
				_titleLabel.TooltipText = "所有作者可编辑属性都直接挂在主画面；右侧原始 Inspector 不参与资源编辑。";
			}
		}
	}

	private static string FirstHintPart(string hintString)
	{
		if (string.IsNullOrWhiteSpace(hintString))
		{
			return "";
		}
		int num = hintString.IndexOf(',');
		if (num >= 0)
		{
			return hintString.Substring(0, num);
		}
		return hintString;
	}

	private static string BuildPropertyLabel(string propertyName, string subgroup, string subgroupBase, string group, string groupBase)
	{
		if (!string.IsNullOrEmpty(subgroup) && !string.IsNullOrEmpty(subgroupBase) && propertyName.StartsWith(subgroupBase, StringComparison.Ordinal))
		{
			string text = propertyName;
			int length = subgroupBase.Length;
			return text.Substring(length, text.Length - length).TrimStart('/', '_');
		}
		if (string.IsNullOrEmpty(subgroup) && !string.IsNullOrEmpty(group) && !string.IsNullOrEmpty(groupBase) && propertyName.StartsWith(groupBase, StringComparison.Ordinal))
		{
			string text = propertyName;
			int length = groupBase.Length;
			return text.Substring(length, text.Length - length).TrimStart('/', '_');
		}
		return propertyName;
	}

	private static string FirstNonEmpty(params string[] values)
	{
		foreach (string text in values)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
		}
		return "";
	}

	private static string SanitizeNodeName(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return "Property";
		}
		return value.Replace('/', '_').Replace(':', '_').Replace('@', '_')
			.Replace('.', '_');
	}

	private static string ComputeSchemaSignature(GodotObject target, IEnumerable<string> specializedPropertyNames, bool includeScript)
	{
		if (!GodotObject.IsInstanceValid(target))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder(target.GetType().FullName).Append("|script:").Append(includeScript ? '1' : '0');
		foreach (Dictionary property in target.GetPropertyList())
		{
			if (property.ContainsKey("name") && property.ContainsKey("usage"))
			{
				stringBuilder.Append('|').Append(property["name"].AsString()).Append(':')
					.Append(property["usage"].AsInt64())
					.Append(':')
					.Append(property.ContainsKey("hint") ? property["hint"].AsInt32() : 0)
					.Append(':')
					.Append(property.ContainsKey("hint_string") ? property["hint_string"].AsString() : "");
			}
		}
		List<string> list = new List<string>(specializedPropertyNames ?? System.Array.Empty<string>());
		list.Sort(StringComparer.Ordinal);
		foreach (string item in list)
		{
			stringBuilder.Append("|specialized:").Append(item);
		}
		return stringBuilder.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshValues, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshEditorTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildChrome, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Rebuild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MountVisualEnumControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "rowHost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "safePropertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeVisualEnumControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateGroupCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnEditorValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySearch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasVisiblePropertyEditor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetPropertyEditorVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCoverageLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSurfaceTitle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FirstHintPart, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPropertyLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subgroup", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subgroupBase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "group", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "groupBase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FirstNonEmpty, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedStringArray, "values", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SanitizeNodeName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshValues && args.Count == 0)
		{
			RefreshValues();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEditorTree && args.Count == 1)
		{
			RefreshEditorTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildChrome && args.Count == 0)
		{
			BuildChrome();
			ret = default;
			return true;
		}
		if (method == MethodName.Rebuild && args.Count == 0)
		{
			Rebuild();
			ret = default;
			return true;
		}
		if (method == MethodName.MountVisualEnumControl && args.Count == 3)
		{
			MountVisualEnumControl(VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[0]), VariantUtils.ConvertTo<VBoxContainer>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeVisualEnumControls && args.Count == 0)
		{
			DisposeVisualEnumControls();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateGroupCard && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(CreateGroupCard(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.OnEditorValueChanged && args.Count == 4)
		{
			OnEditorValueChanged(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySearch && args.Count == 1)
		{
			ApplySearch(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasVisiblePropertyEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasVisiblePropertyEditor(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPropertyEditorVisibility && args.Count == 2)
		{
			SetPropertyEditorVisibility(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCoverageLabel && args.Count == 0)
		{
			UpdateCoverageLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSurfaceTitle && args.Count == 0)
		{
			UpdateSurfaceTitle();
			ret = default;
			return true;
		}
		if (method == MethodName.FirstHintPart && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FirstHintPart(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPropertyLabel && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPropertyLabel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4])));
			return true;
		}
		if (method == MethodName.FirstNonEmpty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FirstNonEmpty(VariantUtils.ConvertTo<string[]>(in args[0])));
			return true;
		}
		if (method == MethodName.SanitizeNodeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(SanitizeNodeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RefreshEditorTree && args.Count == 1)
		{
			RefreshEditorTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasVisiblePropertyEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasVisiblePropertyEditor(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPropertyEditorVisibility && args.Count == 2)
		{
			SetPropertyEditorVisibility(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FirstHintPart && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FirstHintPart(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPropertyLabel && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPropertyLabel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4])));
			return true;
		}
		if (method == MethodName.FirstNonEmpty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FirstNonEmpty(VariantUtils.ConvertTo<string[]>(in args[0])));
			return true;
		}
		if (method == MethodName.SanitizeNodeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(SanitizeNodeName(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.RefreshValues)
		{
			return true;
		}
		if (method == MethodName.RefreshEditorTree)
		{
			return true;
		}
		if (method == MethodName.BuildChrome)
		{
			return true;
		}
		if (method == MethodName.Rebuild)
		{
			return true;
		}
		if (method == MethodName.MountVisualEnumControl)
		{
			return true;
		}
		if (method == MethodName.DisposeVisualEnumControls)
		{
			return true;
		}
		if (method == MethodName.CreateGroupCard)
		{
			return true;
		}
		if (method == MethodName.OnEditorValueChanged)
		{
			return true;
		}
		if (method == MethodName.ApplySearch)
		{
			return true;
		}
		if (method == MethodName.HasVisiblePropertyEditor)
		{
			return true;
		}
		if (method == MethodName.SetPropertyEditorVisibility)
		{
			return true;
		}
		if (method == MethodName.UpdateCoverageLabel)
		{
			return true;
		}
		if (method == MethodName.UpdateSurfaceTitle)
		{
			return true;
		}
		if (method == MethodName.FirstHintPart)
		{
			return true;
		}
		if (method == MethodName.BuildPropertyLabel)
		{
			return true;
		}
		if (method == MethodName.FirstNonEmpty)
		{
			return true;
		}
		if (method == MethodName.SanitizeNodeName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._object)
		{
			_object = VariantUtils.ConvertTo<GodotObject>(in value);
			return true;
		}
		if (name == PropertyName._undoRedo)
		{
			_undoRedo = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName._root)
		{
			_root = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._groupFlow)
		{
			_groupFlow = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._search)
		{
			_search = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			_titleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._coverageLabel)
		{
			_coverageLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._ready)
		{
			_ready = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._includeScript)
		{
			_includeScript = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._historyTypeOverride)
		{
			_historyTypeOverride = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._schemaSignature)
		{
			_schemaSignature = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.EditablePropertyCount)
		{
			from = EditablePropertyCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MountedPropertyCount)
		{
			from = MountedPropertyCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MissingPropertyCount)
		{
			from = MissingPropertyCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SpecializedPropertyCount)
		{
			from = SpecializedPropertyCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DirectControlCount)
		{
			from = DirectControlCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.VisualizedEnumPropertyCount)
		{
			from = VisualizedEnumPropertyCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SegmentedEnumPropertyCount)
		{
			from = SegmentedEnumPropertyCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.GalleryEnumPropertyCount)
		{
			from = GalleryEnumPropertyCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.VisibleRawEnumOptionCount)
		{
			from = VisibleRawEnumOptionCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._object)
		{
			value = VariantUtils.CreateFrom(in _object);
			return true;
		}
		if (name == PropertyName._undoRedo)
		{
			value = VariantUtils.CreateFrom(in _undoRedo);
			return true;
		}
		if (name == PropertyName._root)
		{
			value = VariantUtils.CreateFrom(in _root);
			return true;
		}
		if (name == PropertyName._groupFlow)
		{
			value = VariantUtils.CreateFrom(in _groupFlow);
			return true;
		}
		if (name == PropertyName._search)
		{
			value = VariantUtils.CreateFrom(in _search);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			value = VariantUtils.CreateFrom(in _titleLabel);
			return true;
		}
		if (name == PropertyName._coverageLabel)
		{
			value = VariantUtils.CreateFrom(in _coverageLabel);
			return true;
		}
		if (name == PropertyName._ready)
		{
			value = VariantUtils.CreateFrom(in _ready);
			return true;
		}
		if (name == PropertyName._includeScript)
		{
			value = VariantUtils.CreateFrom(in _includeScript);
			return true;
		}
		if (name == PropertyName._historyTypeOverride)
		{
			value = VariantUtils.CreateFrom(in _historyTypeOverride);
			return true;
		}
		if (name == PropertyName._schemaSignature)
		{
			value = VariantUtils.CreateFrom(in _schemaSignature);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._object, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._undoRedo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._root, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._groupFlow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._search, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._titleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._coverageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ready, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._includeScript, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._historyTypeOverride, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._schemaSignature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.EditablePropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.MountedPropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.MissingPropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SpecializedPropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DirectControlCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisualizedEnumPropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SegmentedEnumPropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.GalleryEnumPropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisibleRawEnumOptionCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._object, Variant.From(in _object));
		info.AddProperty(PropertyName._undoRedo, Variant.From(in _undoRedo));
		info.AddProperty(PropertyName._root, Variant.From(in _root));
		info.AddProperty(PropertyName._groupFlow, Variant.From(in _groupFlow));
		info.AddProperty(PropertyName._search, Variant.From(in _search));
		info.AddProperty(PropertyName._titleLabel, Variant.From(in _titleLabel));
		info.AddProperty(PropertyName._coverageLabel, Variant.From(in _coverageLabel));
		info.AddProperty(PropertyName._ready, Variant.From(in _ready));
		info.AddProperty(PropertyName._includeScript, Variant.From(in _includeScript));
		info.AddProperty(PropertyName._historyTypeOverride, Variant.From(in _historyTypeOverride));
		info.AddProperty(PropertyName._schemaSignature, Variant.From(in _schemaSignature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._object, out var value))
		{
			_object = value.As<GodotObject>();
		}
		if (info.TryGetProperty(PropertyName._undoRedo, out var value2))
		{
			_undoRedo = value2.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._root, out var value3))
		{
			_root = value3.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._groupFlow, out var value4))
		{
			_groupFlow = value4.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._search, out var value5))
		{
			_search = value5.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._titleLabel, out var value6))
		{
			_titleLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._coverageLabel, out var value7))
		{
			_coverageLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._ready, out var value8))
		{
			_ready = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._includeScript, out var value9))
		{
			_includeScript = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._historyTypeOverride, out var value10))
		{
			_historyTypeOverride = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._schemaSignature, out var value11))
		{
			_schemaSignature = value11.As<string>();
		}
	}
}
