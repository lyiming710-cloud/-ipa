using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/XWInlineTextSurface.cs")]
public class XWInlineTextSurface : PanelContainer
{
	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName BindResource = "BindResource";

		public static readonly StringName RefreshValues = "RefreshValues";

		public static readonly StringName BuildChrome = "BuildChrome";

		public static readonly StringName ClearRows = "ClearRows";

		public static readonly StringName CreatePanelStyle = "CreatePanelStyle";

		public static readonly StringName RoleGlyph = "RoleGlyph";

		public static readonly StringName TechnicalIcon = "TechnicalIcon";

		public static readonly StringName Sanitize = "Sanitize";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName SemanticTextCount = "SemanticTextCount";

		public static readonly StringName InlineCoveredCount = "InlineCoveredCount";

		public static readonly StringName TechnicalTextCount = "TechnicalTextCount";

		public static readonly StringName TechnicalCoveredCount = "TechnicalCoveredCount";

		public static readonly StringName MissingCount = "MissingCount";

		public static readonly StringName TechnicalMissingCount = "TechnicalMissingCount";

		public static readonly StringName _resource = "_resource";

		public static readonly StringName _undoRedo = "_undoRedo";

		public static readonly StringName _root = "_root";

		public static readonly StringName _synchronizingControls = "_synchronizingControls";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private Resource _resource;

	private XWUndoRedoManager _undoRedo;

	private XWVisualPropertyBinding _binding;

	private readonly List<XWInlineTextProperty> _properties = new List<XWInlineTextProperty>();

	private readonly Dictionary<string, Control> _controls = new Dictionary<string, Control>(StringComparer.Ordinal);

	private VBoxContainer _root;

	private bool _synchronizingControls;

	public int SemanticTextCount { get; private set; }

	public int InlineCoveredCount { get; private set; }

	public int TechnicalTextCount { get; private set; }

	public int TechnicalCoveredCount { get; private set; }

	public int MissingCount => SemanticTextCount - InlineCoveredCount;

	public int TechnicalMissingCount => TechnicalTextCount - TechnicalCoveredCount;

	public IReadOnlyList<XWInlineTextProperty> Properties => _properties;

	public event Action<bool> TextEdited;

	public event Action<XWInlineTextProperty> BindingPickerRequested;

	public override void _ExitTree()
	{
		_binding?.Dispose();
		_binding = null;
		base._ExitTree();
	}

	public void BindResource(Resource resource, string category, XWUndoRedoManager undoRedo)
	{
		_binding?.Dispose();
		_binding = new XWVisualPropertyBinding(undoRedo, (bool committed) =>
		{
			TextEdited?.Invoke(committed);
		});
		_undoRedo = undoRedo;
		_resource = resource;
		_properties.Clear();
		_controls.Clear();
		SemanticTextCount = 0;
		InlineCoveredCount = 0;
		TechnicalTextCount = 0;
		TechnicalCoveredCount = 0;
		if (!GodotObject.IsInstanceValid(_root))
		{
			BuildChrome();
		}
		ClearRows();
		if (!GodotObject.IsInstanceValid(resource))
		{
			Visible = false;
			return;
		}
		_properties.AddRange(XWInlineTextCoverageContract.Inspect(resource, category));
		Visible = _properties.Count > 0;
		if (Visible)
		{
			List<XWInlineTextProperty> list = _properties.FindAll((XWInlineTextProperty property) => property.IsSemantic);
			List<XWInlineTextProperty> list2 = _properties.FindAll((XWInlineTextProperty property) => property.IsTechnical);
			SemanticTextCount = list.Count;
			TechnicalTextCount = list2.Count;
			if (list.Count > 0)
			{
				BuildSemanticStage(list, category);
			}
			if (list2.Count > 0)
			{
				BuildTechnicalRail(list2);
			}
			InlineCoveredCount = list.FindAll((XWInlineTextProperty property) => _controls.ContainsKey(property.CoverageKey)).Count;
			TechnicalCoveredCount = list2.FindAll((XWInlineTextProperty property) => _controls.ContainsKey(property.CoverageKey)).Count;
		}
	}

	public void ApplyPickedBinding(XWInlineTextProperty property, string value)
	{
		if (!GodotObject.IsInstanceValid(_resource) || property == null || property.Owner != _resource)
		{
			return;
		}
		Variant value2 = ConvertText(property, value ?? "");
		_binding.SetValue(_resource, property.Property, value2, "选择 " + property.Label);
		if (!_controls.TryGetValue(property.CoverageKey, out var value3))
		{
			return;
		}
		_synchronizingControls = true;
		try
		{
			if (value3 is LineEdit lineEdit)
			{
				lineEdit.Text = value ?? "";
			}
			else if (value3 is XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase)
			{
				xWInspectorPropertyEditorBase.UpdateValueSafely();
			}
		}
		finally
		{
			_synchronizingControls = false;
		}
	}

	public void RefreshValues()
	{
		if (_synchronizingControls)
		{
			return;
		}
		_synchronizingControls = true;
		try
		{
			_binding?.RefreshBoundControls();
			foreach (XWInlineTextProperty property in _properties)
			{
				if (!_controls.TryGetValue(property.CoverageKey, out var value) || !GodotObject.IsInstanceValid(value))
				{
					continue;
				}
				if (value is XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase)
				{
					xWInspectorPropertyEditorBase.UpdateValueSafely();
					continue;
				}
				string text = ReadText(property);
				if (value is LineEdit lineEdit)
				{
					lineEdit.Text = text;
				}
				else if (value is TextEdit textEdit)
				{
					textEdit.Text = text;
				}
				else
				{
					if (!(value is OptionButton optionButton) || !property.IsTextEnum)
					{
						continue;
					}
					for (int i = 0; i < optionButton.ItemCount; i++)
					{
						if (string.Equals(optionButton.GetItemText(i), text, StringComparison.Ordinal))
						{
							optionButton.Select(i);
							break;
						}
					}
				}
			}
		}
		finally
		{
			_synchronizingControls = false;
		}
	}

	private void BuildChrome()
	{
		Name = "InlineTextSurface";
		SizeFlagsHorizontal = SizeFlags.ExpandFill;
		CustomMinimumSize = new Vector2(520f, 0f);
		AddThemeStyleboxOverride("panel", CreatePanelStyle(new Color(0.025f, 0.045f, 0.035f, 0.99f), new Color(0.32f, 0.58f, 0.22f, 0.95f), 10));
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
		_root.AddThemeConstantOverride("separation", 12);
		marginContainer.AddChild(_root, forceReadableName: false, InternalMode.Disabled);
	}

	private void ClearRows()
	{
		if (!GodotObject.IsInstanceValid(_root))
		{
			return;
		}
		foreach (Node child in _root.GetChildren())
		{
			_root.RemoveChild(child);
			child.QueueFree();
		}
	}

	private void BuildSemanticStage(IReadOnlyList<XWInlineTextProperty> properties, string category)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddThemeConstantOverride("separation", 8);
		TextureRect node = new TextureRect
		{
			Texture = XWClassRegistry.Instance.GetUIIcon("Control"),
			CustomMinimumSize = new Vector2(28f, 28f),
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
		};
		hBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		Label label = new Label
		{
			Text = "游戏文字就地编辑 · " + category,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			TooltipText = "输入框位于标题、对白、提示和按钮实际出现的视觉区域。"
		};
		label.AddThemeFontSizeOverride("font_size", 18);
		label.AddThemeColorOverride("font_color", new Color(0.86f, 1f, 0.68f));
		hBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		Label node2 = new Label
		{
			Text = $"{properties.Count} 处",
			Modulate = new Color(0.98f, 0.79f, 0.29f)
		};
		hBoxContainer.AddChild(node2, forceReadableName: false, InternalMode.Disabled);
		_root.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Name = "GameTextStage",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 9);
		_root.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		foreach (XWInlineTextProperty property in properties)
		{
			Control control = ((!property.IsTextEnum) ? (property.Role switch
			{
				XWInlineTextRole.Title => CreateTitleEditor(property), 
				XWInlineTextRole.Dialogue => CreateBodyEditor(property, dialogue: true), 
				XWInlineTextRole.Description => CreateBodyEditor(property, dialogue: false), 
				XWInlineTextRole.HudMessage => CreateHudEditor(property), 
				XWInlineTextRole.ButtonLabel => CreateButtonEditor(property), 
				_ => null, 
			}) : CreateSemanticEnumEditor(property));
			Control control2 = control;
			if (GodotObject.IsInstanceValid(control2))
			{
				vBoxContainer.AddChild(control2, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private Control CreateSemanticEnumEditor(XWInlineTextProperty property)
	{
		(PanelContainer Panel, VBoxContainer Body) tuple = CreateRoleCard(property, new Color(0.1f, 0.17f, 0.08f), new Color(0.58f, 0.84f, 0.3f));
		tuple.Body.AddChild(CreateTextEnumEditor(property), forceReadableName: false, InternalMode.Disabled);
		return tuple.Panel;
	}

	private Control CreateTitleEditor(XWInlineTextProperty property)
	{
		(PanelContainer, VBoxContainer) tuple = CreateRoleCard(property, new Color(0.13f, 0.24f, 0.09f), new Color(0.61f, 0.83f, 0.28f));
		LineEdit lineEdit = new LineEdit
		{
			Name = "Inline_" + Sanitize(property.Property.ToString()),
			PlaceholderText = "输入游戏画面标题…",
			Alignment = HorizontalAlignment.Center,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			CustomMinimumSize = new Vector2(0f, 44f)
		};
		lineEdit.AddThemeFontSizeOverride("font_size", 22);
		AddRoleEditor(tuple.Item2, property, lineEdit);
		return tuple.Item1;
	}

	private Control CreateBodyEditor(XWInlineTextProperty property, bool dialogue)
	{
		(PanelContainer, VBoxContainer) tuple = CreateRoleCard(property, dialogue ? new Color(0.08f, 0.12f, 0.17f) : new Color(0.1f, 0.095f, 0.055f), dialogue ? new Color(0.36f, 0.68f, 0.94f) : new Color(0.88f, 0.65f, 0.24f));
		HBoxContainer hBoxContainer = new HBoxContainer();
		if (dialogue)
		{
			Label label = new Label
			{
				Text = "NPC",
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				CustomMinimumSize = new Vector2(64f, 64f)
			};
			label.AddThemeStyleboxOverride("normal", CreatePanelStyle(new Color(0.1f, 0.2f, 0.27f), new Color(0.31f, 0.62f, 0.83f), 32));
			hBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		}
		TextEdit textEdit = new TextEdit
		{
			Name = "Inline_" + Sanitize(property.Property.ToString()),
			PlaceholderText = (dialogue ? "在游戏对白气泡中输入…" : "在说明卡片中输入…"),
			CustomMinimumSize = new Vector2(0f, dialogue ? 92 : 76),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			WrapMode = TextEdit.LineWrappingMode.Boundary
		};
		hBoxContainer.AddChild(textEdit, forceReadableName: false, InternalMode.Disabled);
		AddRoleEditor(tuple.Item2, property, hBoxContainer, textEdit);
		return tuple.Item1;
	}

	private Control CreateHudEditor(XWInlineTextProperty property)
	{
		(PanelContainer, VBoxContainer) tuple = CreateRoleCard(property, new Color(0.08f, 0.13f, 0.09f), new Color(0.38f, 0.82f, 0.43f));
		LineEdit host = new LineEdit
		{
			Name = "Inline_" + Sanitize(property.Property.ToString()),
			PlaceholderText = "游戏 HUD 提示…",
			Alignment = HorizontalAlignment.Center,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		AddRoleEditor(tuple.Item2, property, host);
		return tuple.Item1;
	}

	private Control CreateButtonEditor(XWInlineTextProperty property)
	{
		(PanelContainer, VBoxContainer) tuple = CreateRoleCard(property, new Color(0.1f, 0.16f, 0.07f), new Color(0.52f, 0.82f, 0.27f));
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			Alignment = BoxContainer.AlignmentMode.Center
		};
		PanelContainer panelContainer = new PanelContainer
		{
			CustomMinimumSize = new Vector2(260f, 48f)
		};
		panelContainer.AddThemeStyleboxOverride("panel", CreatePanelStyle(new Color(0.18f, 0.34f, 0.1f), new Color(0.66f, 0.91f, 0.31f), 8));
		LineEdit lineEdit = new LineEdit
		{
			Name = "Inline_" + Sanitize(property.Property.ToString()),
			PlaceholderText = "按钮文字…",
			Alignment = HorizontalAlignment.Center,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		panelContainer.AddChild(lineEdit, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		AddRoleEditor(tuple.Item2, property, hBoxContainer, lineEdit);
		return tuple.Item1;
	}

	private (PanelContainer Panel, VBoxContainer Body) CreateRoleCard(XWInlineTextProperty property, Color background, Color border)
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		panelContainer.AddThemeStyleboxOverride("panel", CreatePanelStyle(background, border, 7));
		MarginContainer marginContainer = new MarginContainer();
		marginContainer.AddThemeConstantOverride("margin_left", 10);
		marginContainer.AddThemeConstantOverride("margin_top", 8);
		marginContainer.AddThemeConstantOverride("margin_right", 10);
		marginContainer.AddThemeConstantOverride("margin_bottom", 9);
		panelContainer.AddChild(marginContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer = new VBoxContainer();
		marginContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		Label label = new Label
		{
			Text = RoleGlyph(property.Role) + "  " + property.Label,
			TooltipText = property.CoverageKey + " · " + property.ClassificationReason
		};
		label.AddThemeColorOverride("font_color", border.Lightened(0.18f));
		vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		return (Panel: panelContainer, Body: vBoxContainer);
	}

	private void AddRoleEditor(VBoxContainer body, XWInlineTextProperty property, Control host, Control editor = null)
	{
		if (editor == null)
		{
			editor = host;
		}
		body.AddChild(host, forceReadableName: false, InternalMode.Disabled);
		BindEditor(property, editor);
	}

	private void BuildTechnicalRail(IReadOnlyList<XWInlineTextProperty> properties)
	{
		_root.AddChild(new HSeparator(), forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer();
		TextureRect node = new TextureRect
		{
			Texture = XWClassRegistry.Instance.GetUIIcon("AssetLib"),
			CustomMinimumSize = new Vector2(24f, 24f),
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
		};
		hBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(new Label
		{
			Text = $"资源绑定轨道 · {properties.Count}",
			TooltipText = "技术键和路径不计入玩家文字；使用图鉴选择器绑定，也可直接修正。",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		_root.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		GridContainer gridContainer = new GridContainer
		{
			Columns = 2,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		gridContainer.AddThemeConstantOverride("h_separation", 8);
		gridContainer.AddThemeConstantOverride("v_separation", 6);
		_root.AddChild(gridContainer, forceReadableName: false, InternalMode.Disabled);
		foreach (XWInlineTextProperty property in properties)
		{
			gridContainer.AddChild(CreateTechnicalBindingCard(property), forceReadableName: false, InternalMode.Disabled);
		}
	}

	private Control CreateTechnicalBindingCard(XWInlineTextProperty property)
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.CustomMinimumSize = new Vector2(330f, 0f);
		panelContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		panelContainer.AddThemeStyleboxOverride("panel", CreatePanelStyle(new Color(0.055f, 0.065f, 0.08f), new Color(0.25f, 0.34f, 0.44f), 6));
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddThemeConstantOverride("separation", 6);
		panelContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		TextureRect node = new TextureRect
		{
			Texture = XWClassRegistry.Instance.GetUIIcon(TechnicalIcon(property.Role)),
			CustomMinimumSize = new Vector2(26f, 26f),
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
			TooltipText = property.ClassificationReason
		};
		hBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hBoxContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(new Label
		{
			Text = property.Label,
			TooltipText = property.CoverageKey
		}, forceReadableName: false, InternalMode.Disabled);
		if (property.IsTextEnum)
		{
			OptionButton node2 = CreateTextEnumEditor(property);
			vBoxContainer.AddChild(node2, forceReadableName: false, InternalMode.Disabled);
			return panelContainer;
		}
		if (property.IsNodePath)
		{
			XWInspectorPropertyEditorBase node3 = CreateNodePathEditor(property);
			vBoxContainer.AddChild(node3, forceReadableName: false, InternalMode.Disabled);
			return panelContainer;
		}
		LineEdit lineEdit = new LineEdit
		{
			Name = "Inline_" + Sanitize(property.Property.ToString()),
			PlaceholderText = ((property.Role == XWInlineTextRole.TechnicalPath) ? "选择资源路径…" : "输入游戏资源 Key…"),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddChild(lineEdit, forceReadableName: false, InternalMode.Disabled);
		BindEditor(property, lineEdit);
		if (XWInlineTextCoverageContract.TryResolveBindingBaseType(property, out var baseType))
		{
			Button button = new Button
			{
				Name = "Pick_" + Sanitize(property.Property.ToString()),
				Text = "图鉴",
				Icon = XWClassRegistry.Instance.GetUIIcon("AssetLib"),
				TooltipText = "打开 " + baseType + " 图鉴并绑定此字段"
			};
			button.Pressed += () =>
			{
				BindingPickerRequested?.Invoke(property);
			};
			hBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
		return panelContainer;
	}

	private OptionButton CreateTextEnumEditor(XWInlineTextProperty property)
	{
		OptionButton options = new OptionButton
		{
			Name = "InlineEnum_" + Sanitize(property.Property.ToString()),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			TooltipText = "从资源声明的可用值中选择"
		};
		string b = ReadText(property);
		int num = -1;
		string[] array = (property.HintString ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (!string.IsNullOrWhiteSpace(text))
			{
				options.AddItem(text);
				if (string.Equals(text, b, StringComparison.Ordinal))
				{
					num = options.ItemCount - 1;
				}
			}
		}
		if (num >= 0)
		{
			options.Select(num);
		}
		options.ItemSelected += (long index) =>
		{
			if (!_synchronizingControls && index >= 0 && index < options.ItemCount)
			{
				string itemText = options.GetItemText((int)index);
				_binding.SetValue(_resource, property.Property, ConvertText(property, itemText), "选择 " + property.Label);
			}
		};
		_controls[property.CoverageKey] = options;
		return options;
	}

	private XWInspectorPropertyEditorBase CreateNodePathEditor(XWInlineTextProperty property)
	{
		XWInspectorPropertyEditorNodePath editor = XWInspectorPropertyEditorNodePath.Create();
		if (!GodotObject.IsInstanceValid(editor))
		{
			throw new InvalidOperationException("NodePath selector is unavailable for " + property.CoverageKey);
		}
		editor.Name = "InlineNodePath_" + Sanitize(property.Property.ToString());
		editor.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		editor.PropertyNameLock = true;
		editor.UndoRedoManager = _undoRedo;
		editor.ValueChanged += (GodotObject _, StringName _, StringName _, Variant _) =>
		{
			TextEdited?.Invoke(obj: true);
		};
		XWInspectorProperty metadata = new XWInspectorProperty(_resource, property.Property, default, null, null, property.Hint, property.HintString)
		{
			LabelOverride = property.Label,
			PropertyUsage = 4L
		};
		editor.Ready += () =>
		{
			editor.SetEditProperty(metadata);
		};
		_controls[property.CoverageKey] = editor;
		return editor;
	}

	private void BindEditor(XWInlineTextProperty property, Control editor)
	{
		_controls[property.CoverageKey] = editor;
		LineEdit line = editor as LineEdit;
		if (line != null)
		{
			line.Text = ReadText(property);
			bool submitted = false;
			line.FocusEntered += () =>
			{
				if (!_synchronizingControls)
				{
					submitted = false;
					_binding.BeginEdit(_resource, property.Property);
				}
			};
			line.TextChanged += (string value) =>
			{
				if (!_synchronizingControls)
				{
					submitted = false;
					_binding.PreviewValue(_resource, property.Property, ConvertText(property, value));
				}
			};
			line.TextSubmitted += (string value) =>
			{
				if (!_synchronizingControls)
				{
					_binding.CommitEdit(_resource, property.Property, ConvertText(property, value), "修改 " + property.Label);
					submitted = true;
				}
			};
			line.FocusExited += () =>
			{
				if (!_synchronizingControls)
				{
					if (submitted)
					{
						submitted = false;
					}
					else
					{
						_binding.CommitEdit(_resource, property.Property, ConvertText(property, line.Text), "修改 " + property.Label);
					}
				}
			};
			return;
		}
		TextEdit text = editor as TextEdit;
		if (text == null)
		{
			return;
		}
		text.Text = ReadText(property);
		text.FocusEntered += () =>
		{
			if (!_synchronizingControls)
			{
				_binding.BeginEdit(_resource, property.Property);
			}
		};
		text.TextChanged += () =>
		{
			if (!_synchronizingControls)
			{
				_binding.PreviewValue(_resource, property.Property, ConvertText(property, text.Text));
			}
		};
		text.FocusExited += () =>
		{
			if (!_synchronizingControls)
			{
				_binding.CommitEdit(_resource, property.Property, ConvertText(property, text.Text), "修改 " + property.Label);
			}
		};
	}

	private string ReadText(XWInlineTextProperty property)
	{
		try
		{
			return _resource.Get(property.Property).AsString();
		}
		catch
		{
			return "";
		}
	}

	private static Variant ConvertText(XWInlineTextProperty property, string value)
	{
		return property.VariantType switch
		{
			Variant.Type.StringName => Variant.From<StringName>(new StringName(value)), 
			Variant.Type.NodePath => Variant.From<NodePath>(new NodePath(value)), 
			_ => Variant.From(in value), 
		};
	}

	private static StyleBoxFlat CreatePanelStyle(Color background, Color border, int radius)
	{
		return new StyleBoxFlat
		{
			BgColor = background,
			BorderColor = border,
			BorderWidthLeft = 1,
			BorderWidthTop = 1,
			BorderWidthRight = 1,
			BorderWidthBottom = 1,
			CornerRadiusTopLeft = radius,
			CornerRadiusTopRight = radius,
			CornerRadiusBottomLeft = radius,
			CornerRadiusBottomRight = radius,
			ContentMarginLeft = 8f,
			ContentMarginTop = 6f,
			ContentMarginRight = 8f,
			ContentMarginBottom = 7f
		};
	}

	private static string RoleGlyph(XWInlineTextRole role)
	{
		return role switch
		{
			XWInlineTextRole.Title => "★", 
			XWInlineTextRole.Dialogue => "●", 
			XWInlineTextRole.Description => "▤", 
			XWInlineTextRole.HudMessage => "⚑", 
			XWInlineTextRole.ButtonLabel => "◆", 
			_ => "◇", 
		};
	}

	private static StringName TechnicalIcon(XWInlineTextRole role)
	{
		return role switch
		{
			XWInlineTextRole.TechnicalPath => new StringName("Folder"), 
			XWInlineTextRole.TechnicalBinding => new StringName("Instance"), 
			_ => new StringName("Key"), 
		};
	}

	private static string Sanitize(string value)
	{
		return (value ?? "Property").Replace('/', '_').Replace(':', '_').Replace('.', '_')
			.Replace('@', '_');
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "undoRedo", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshValues, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildChrome, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreatePanelStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "background", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "border", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "radius", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RoleGlyph, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "role", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TechnicalIcon, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "role", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.BindResource && args.Count == 3)
		{
			BindResource(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<XWUndoRedoManager>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshValues && args.Count == 0)
		{
			RefreshValues();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildChrome && args.Count == 0)
		{
			BuildChrome();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearRows && args.Count == 0)
		{
			ClearRows();
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePanelStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreatePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.RoleGlyph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(RoleGlyph(VariantUtils.ConvertTo<XWInlineTextRole>(in args[0])));
			return true;
		}
		if (method == MethodName.TechnicalIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(TechnicalIcon(VariantUtils.ConvertTo<XWInlineTextRole>(in args[0])));
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
		if (method == MethodName.CreatePanelStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreatePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.RoleGlyph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(RoleGlyph(VariantUtils.ConvertTo<XWInlineTextRole>(in args[0])));
			return true;
		}
		if (method == MethodName.TechnicalIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(TechnicalIcon(VariantUtils.ConvertTo<XWInlineTextRole>(in args[0])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BindResource)
		{
			return true;
		}
		if (method == MethodName.RefreshValues)
		{
			return true;
		}
		if (method == MethodName.BuildChrome)
		{
			return true;
		}
		if (method == MethodName.ClearRows)
		{
			return true;
		}
		if (method == MethodName.CreatePanelStyle)
		{
			return true;
		}
		if (method == MethodName.RoleGlyph)
		{
			return true;
		}
		if (method == MethodName.TechnicalIcon)
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
		if (name == PropertyName.SemanticTextCount)
		{
			SemanticTextCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.InlineCoveredCount)
		{
			InlineCoveredCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.TechnicalTextCount)
		{
			TechnicalTextCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.TechnicalCoveredCount)
		{
			TechnicalCoveredCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._resource)
		{
			_resource = VariantUtils.ConvertTo<Resource>(in value);
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
		if (name == PropertyName._synchronizingControls)
		{
			_synchronizingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.SemanticTextCount)
		{
			from = SemanticTextCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.InlineCoveredCount)
		{
			from = InlineCoveredCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TechnicalTextCount)
		{
			from = TechnicalTextCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TechnicalCoveredCount)
		{
			from = TechnicalCoveredCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MissingCount)
		{
			from = MissingCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TechnicalMissingCount)
		{
			from = TechnicalMissingCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._resource)
		{
			value = VariantUtils.CreateFrom(in _resource);
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
		if (name == PropertyName._synchronizingControls)
		{
			value = VariantUtils.CreateFrom(in _synchronizingControls);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._resource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._undoRedo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._root, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._synchronizingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SemanticTextCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.InlineCoveredCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TechnicalTextCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TechnicalCoveredCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.MissingCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TechnicalMissingCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.SemanticTextCount, Variant.From<int>(SemanticTextCount));
		info.AddProperty(PropertyName.InlineCoveredCount, Variant.From<int>(InlineCoveredCount));
		info.AddProperty(PropertyName.TechnicalTextCount, Variant.From<int>(TechnicalTextCount));
		info.AddProperty(PropertyName.TechnicalCoveredCount, Variant.From<int>(TechnicalCoveredCount));
		info.AddProperty(PropertyName._resource, Variant.From(in _resource));
		info.AddProperty(PropertyName._undoRedo, Variant.From(in _undoRedo));
		info.AddProperty(PropertyName._root, Variant.From(in _root));
		info.AddProperty(PropertyName._synchronizingControls, Variant.From(in _synchronizingControls));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.SemanticTextCount, out var value))
		{
			SemanticTextCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.InlineCoveredCount, out var value2))
		{
			InlineCoveredCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.TechnicalTextCount, out var value3))
		{
			TechnicalTextCount = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.TechnicalCoveredCount, out var value4))
		{
			TechnicalCoveredCount = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._resource, out var value5))
		{
			_resource = value5.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._undoRedo, out var value6))
		{
			_undoRedo = value6.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._root, out var value7))
		{
			_root = value7.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._synchronizingControls, out var value8))
		{
			_synchronizingControls = value8.As<bool>();
		}
	}
}
