using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWConveyorEventVisualResourceEditor.cs")]
public class XWConveyorEventVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName BuildWorkbench = "BuildWorkbench";

		public static readonly StringName BindEvent = "BindEvent";

		public static readonly StringName BuildPacketFields = "BuildPacketFields";

		public static readonly StringName OnPacketResourceChanged = "OnPacketResourceChanged";

		public static readonly StringName CreateDefaultPacket = "CreateDefaultPacket";

		public static readonly StringName OpenPacketEditor = "OpenPacketEditor";

		public static readonly StringName RefreshFromHistory = "RefreshFromHistory";

		public static readonly StringName QueueRefreshFromHistory = "QueueRefreshFromHistory";

		public static readonly StringName RefreshPreview = "RefreshPreview";

		public static readonly StringName OnEdited = "OnEdited";

		public static readonly StringName DisposeBindings = "DisposeBindings";

		public static readonly StringName Panel = "Panel";

		public static readonly StringName Section = "Section";

		public static readonly StringName StageBadge = "StageBadge";

		public static readonly StringName Field = "Field";

		public static readonly StringName Number = "Number";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _event = "_event";

		public static readonly StringName _packetPicker = "_packetPicker";

		public static readonly StringName _previewPacket = "_previewPacket";

		public static readonly StringName _previewRules = "_previewRules";

		public static readonly StringName _weightBar = "_weightBar";

		public static readonly StringName _packetFields = "_packetFields";

		public static readonly StringName _refreshQueued = "_refreshQueued";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private TowerDefenseConveyorEventAddPacket _event;

	private XWVisualPropertyBinding _eventBinding;

	private XWVisualPropertyBinding _packetBinding;

	private XWResourcePicker _packetPicker;

	private Label _previewPacket;

	private Label _previewRules;

	private ProgressBar _weightBar;

	private VBoxContainer _packetFields;

	private bool _refreshQueued;

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeBindings();
		if (CurrentResource is TowerDefenseConveyorEventAddPacket towerDefenseConveyorEventAddPacket && CanvasGrid != null)
		{
			_event = towerDefenseConveyorEventAddPacket;
			CanvasGrid.Columns = 1;
			CanvasGrid.AddChild(BuildWorkbench(), forceReadableName: false, InternalMode.Disabled);
			BindEvent();
			RefreshFromHistory();
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (resource?.GetType() != typeof(TowerDefenseConveyorEventAddPacket))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	public override void _ExitTree()
	{
		DisposeBindings();
		base._ExitTree();
	}

	private Control BuildWorkbench()
	{
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		vBoxContainer.AddThemeConstantOverride("separation", 12);
		PanelContainer panelContainer = Panel("传送带波次 · 加入卡牌", new Color(0.08f, 0.19f, 0.12f));
		VBoxContainer vBoxContainer2 = new VBoxContainer();
		vBoxContainer2.AddChild(new Label
		{
			Text = "\ud83c\udf0a  大波次触发",
			ThemeTypeVariation = "HeaderLarge"
		}, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(new Label
		{
			Text = "事件执行后，配置的卡牌会成为传送带候选卡。",
			Modulate = new Color(0.75f, 0.88f, 0.76f)
		}, forceReadableName: false, InternalMode.Disabled);
		panelContainer.AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		PanelContainer panelContainer2 = Panel("", new Color(0.045f, 0.1f, 0.075f));
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddThemeConstantOverride("separation", 18);
		hBoxContainer.AddChild(StageBadge("旗帜波次", new Color(0.64f, 0.2f, 0.15f)), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(new Label
		{
			Text = "➜",
			VerticalAlignment = VerticalAlignment.Center,
			ThemeTypeVariation = "HeaderLarge"
		}, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer3 = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_previewPacket = new Label
		{
			Text = "未配置卡牌",
			ThemeTypeVariation = "HeaderMedium"
		};
		_previewRules = new Label
		{
			AutowrapMode = TextServer.AutowrapMode.WordSmart
		};
		_weightBar = new ProgressBar
		{
			MinValue = 0.0,
			MaxValue = 100.0,
			ShowPercentage = false,
			CustomMinimumSize = new Vector2(0f, 16f)
		};
		vBoxContainer3.AddChild(_previewPacket, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer3.AddChild(_weightBar, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer3.AddChild(_previewRules, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(vBoxContainer3, forceReadableName: false, InternalMode.Disabled);
		panelContainer2.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(panelContainer2, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer4 = Section("事件资源");
		LineEdit lineEdit = new LineEdit
		{
			PlaceholderText = "资源名称"
		};
		lineEdit.Name = "ResourceNameEdit";
		CheckButton checkButton = new CheckButton
		{
			Text = "仅场景本地"
		};
		checkButton.Name = "LocalToSceneCheck";
		vBoxContainer4.AddChild(Field("资源名", lineEdit), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer4.AddChild(checkButton, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(vBoxContainer4, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer5 = Section("候选卡牌（直接编辑）");
		_packetPicker = XWResourcePicker.Create();
		_packetPicker.Setup("TowerDefenseConveyorPacketConfig");
		_packetPicker.ResourceChanged += OnPacketResourceChanged;
		_packetPicker.ResourceSelected += OpenPacketEditor;
		vBoxContainer5.AddChild(Field("卡牌条目", _packetPicker), forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer2 = new HBoxContainer();
		Button button = new Button
		{
			Text = "＋ 新建默认条目"
		};
		Button button2 = new Button
		{
			Text = "\ud83c\udfb4 打开完整卡牌条目"
		};
		button.Pressed += CreateDefaultPacket;
		button2.Pressed += () =>
		{
			OpenPacketEditor(_event?.packet);
		};
		hBoxContainer2.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer2.AddChild(button2, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer5.AddChild(hBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		_packetFields = new VBoxContainer();
		_packetFields.AddThemeConstantOverride("separation", 7);
		vBoxContainer5.AddChild(_packetFields, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(vBoxContainer5, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private void BindEvent()
	{
		_eventBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnEdited);
		_eventBinding.BindText(FindChild("ResourceNameEdit", recursive: true, owned: false) as LineEdit, _event, "resource_name", QueueRefreshFromHistory, this, "RefreshFromHistory");
		_eventBinding.BindToggle(FindChild("LocalToSceneCheck", recursive: true, owned: false) as CheckButton, _event, "resource_local_to_scene", QueueRefreshFromHistory, this, "RefreshFromHistory");
		_packetPicker.SetEditedResource(_event.packet);
		BuildPacketFields();
	}

	private void BuildPacketFields()
	{
		_packetBinding?.Dispose();
		_packetBinding = null;
		foreach (Node child in _packetFields.GetChildren())
		{
			child.QueueFree();
		}
		if (!GodotObject.IsInstanceValid(_event?.packet))
		{
			_packetFields.AddChild(new Label
			{
				Text = "先选择或新建卡牌条目。",
				Modulate = Colors.Gold
			}, forceReadableName: false, InternalMode.Disabled);
			return;
		}
		_packetBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnEdited);
		LineEdit control = new LineEdit
		{
			Name = "ConveyorPacketNameEdit"
		};
		SpinBox spinBox = Number(0.0, 100000.0, 1.0);
		spinBox.Name = "ConveyorPacketWeightSpin";
		SpinBox control2 = Number(-1.0, 999.0, 1.0);
		SpinBox control3 = Number(-1.0, 999.0, 1.0);
		SpinBox control4 = Number(0.0, 100.0, 0.05);
		SpinBox control5 = Number(0.0, 100.0, 0.05);
		_packetFields.AddChild(Field("卡牌键", control), forceReadableName: false, InternalMode.Disabled);
		_packetFields.AddChild(Field("抽取权重", spinBox), forceReadableName: false, InternalMode.Disabled);
		_packetFields.AddChild(Field("场上最少数量", control2), forceReadableName: false, InternalMode.Disabled);
		_packetFields.AddChild(Field("场上最多数量", control3), forceReadableName: false, InternalMode.Disabled);
		_packetFields.AddChild(Field("最少数量倍率", control4), forceReadableName: false, InternalMode.Disabled);
		_packetFields.AddChild(Field("最多数量倍率", control5), forceReadableName: false, InternalMode.Disabled);
		_packetBinding.BindText(control, _event.packet, "name", RefreshPreview, this, "RefreshFromHistory");
		_packetBinding.BindNumber(spinBox, _event.packet, "weight", RefreshPreview, this, "RefreshFromHistory");
		_packetBinding.BindNumber(control2, _event.packet, "minNum", RefreshPreview, this, "RefreshFromHistory");
		_packetBinding.BindNumber(control3, _event.packet, "maxNum", RefreshPreview, this, "RefreshFromHistory");
		_packetBinding.BindNumber(control4, _event.packet, "minMagnification", RefreshPreview, this, "RefreshFromHistory");
		_packetBinding.BindNumber(control5, _event.packet, "maxMagnification", RefreshPreview, this, "RefreshFromHistory");
	}

	private void OnPacketResourceChanged(Resource resource)
	{
		_eventBinding?.SetValue(_event, "packet", resource as TowerDefenseConveyorPacketConfig, "更换传送带候选卡牌", this, "RefreshFromHistory");
		RefreshFromHistory();
	}

	private void CreateDefaultPacket()
	{
		OnPacketResourceChanged(new TowerDefenseConveyorPacketConfig
		{
			name = "NewPacket",
			weight = 10
		});
	}

	private void OpenPacketEditor(Resource resource)
	{
		if (GodotObject.IsInstanceValid(resource))
		{
			XWEditorInterface.Instance?.EditResource(resource, XWResourceEditContext.ForProperty(resource, _event, resource.ResourcePath, CurrentResourcePath, "packet", -1, "packet_spawn_entry_editor", CurrentEditContext?.IsBuiltInSource ?? XWResourceEditContext.IsBuiltInPath(CurrentResourcePath)));
		}
	}

	public void RefreshFromHistory()
	{
		_refreshQueued = false;
		if (GodotObject.IsInstanceValid(_event))
		{
			_packetPicker?.SetEditedResource(_event.packet);
			BuildPacketFields();
			RefreshPreview();
		}
	}

	private void QueueRefreshFromHistory()
	{
		if (!_refreshQueued)
		{
			_refreshQueued = true;
			CallDeferred("RefreshFromHistory");
		}
	}

	private void RefreshPreview()
	{
		TowerDefenseConveyorPacketConfig towerDefenseConveyorPacketConfig = _event?.packet;
		bool flag = GodotObject.IsInstanceValid(towerDefenseConveyorPacketConfig);
		_previewPacket.Text = ((flag && !string.IsNullOrWhiteSpace(towerDefenseConveyorPacketConfig.name)) ? ("\ud83c\udfb4  " + towerDefenseConveyorPacketConfig.name) : "⚠ 未配置卡牌键");
		_weightBar.Value = (flag ? Mathf.Clamp(towerDefenseConveyorPacketConfig.weight, 0, 100) : 0);
		_previewRules.Text = (flag ? $"权重 {towerDefenseConveyorPacketConfig.weight}\u3000数量 {towerDefenseConveyorPacketConfig.minNum}～{towerDefenseConveyorPacketConfig.maxNum}\u3000倍率 {towerDefenseConveyorPacketConfig.minMagnification:0.##}～{towerDefenseConveyorPacketConfig.maxMagnification:0.##}" : "事件不会向传送带加入有效候选卡。");
	}

	private void OnEdited(bool committed)
	{
		NotifyCurrentResourceEdited();
		RefreshPreview();
	}

	private void DisposeBindings()
	{
		_eventBinding?.Dispose();
		_packetBinding?.Dispose();
		_eventBinding = null;
		_packetBinding = null;
		_event = null;
	}

	private static PanelContainer Panel(string tooltip, Color color)
	{
		PanelContainer panelContainer = new PanelContainer
		{
			TooltipText = tooltip
		};
		StyleBoxFlat styleBoxFlat = new StyleBoxFlat
		{
			BgColor = color,
			CornerRadiusTopLeft = 9,
			CornerRadiusTopRight = 9,
			CornerRadiusBottomLeft = 9,
			CornerRadiusBottomRight = 9
		};
		float contentMarginLeft = (styleBoxFlat.ContentMarginRight = 14f);
		styleBoxFlat.ContentMarginLeft = contentMarginLeft;
		contentMarginLeft = (styleBoxFlat.ContentMarginBottom = 12f);
		styleBoxFlat.ContentMarginTop = contentMarginLeft;
		panelContainer.AddThemeStyleboxOverride("panel", styleBoxFlat);
		return panelContainer;
	}

	private static VBoxContainer Section(string title)
	{
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.AddThemeConstantOverride("separation", 7);
		vBoxContainer.AddChild(new Label
		{
			Text = title,
			ThemeTypeVariation = "HeaderMedium"
		}, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private static Control StageBadge(string text, Color color)
	{
		PanelContainer panelContainer = Panel("", color);
		panelContainer.CustomMinimumSize = new Vector2(150f, 72f);
		panelContainer.AddChild(new Label
		{
			Text = text,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		}, forceReadableName: false, InternalMode.Disabled);
		return panelContainer;
	}

	private static HBoxContainer Field(string label, Control control)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddChild(new Label
		{
			Text = label,
			CustomMinimumSize = new Vector2(150f, 0f)
		}, forceReadableName: false, InternalMode.Disabled);
		control.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddChild(control, forceReadableName: false, InternalMode.Disabled);
		return hBoxContainer;
	}

	private static SpinBox Number(double min, double max, double step)
	{
		return new SpinBox
		{
			MinValue = min,
			MaxValue = max,
			Step = step,
			AllowGreater = true,
			AllowLesser = true
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildWorkbench, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildPacketFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPacketResourceChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateDefaultPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenPacketEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueRefreshFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Panel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Section, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StageBadge, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Field, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.Number, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "min", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "max", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BuildWorkbench && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(BuildWorkbench());
			return true;
		}
		if (method == MethodName.BindEvent && args.Count == 0)
		{
			BindEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildPacketFields && args.Count == 0)
		{
			BuildPacketFields();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPacketResourceChanged && args.Count == 1)
		{
			OnPacketResourceChanged(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateDefaultPacket && args.Count == 0)
		{
			CreateDefaultPacket();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenPacketEditor && args.Count == 1)
		{
			OpenPacketEditor(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFromHistory && args.Count == 0)
		{
			RefreshFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueRefreshFromHistory && args.Count == 0)
		{
			QueueRefreshFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPreview && args.Count == 0)
		{
			RefreshPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnEdited && args.Count == 1)
		{
			OnEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeBindings && args.Count == 0)
		{
			DisposeBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.Panel && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(Panel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.Section && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(Section(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.StageBadge && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(StageBadge(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.Field && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(Field(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.Number && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(Number(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Panel && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(Panel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.Section && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(Section(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.StageBadge && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(StageBadge(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.Field && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(Field(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.Number && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(Number(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
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
		if (method == MethodName.BuildWorkbench)
		{
			return true;
		}
		if (method == MethodName.BindEvent)
		{
			return true;
		}
		if (method == MethodName.BuildPacketFields)
		{
			return true;
		}
		if (method == MethodName.OnPacketResourceChanged)
		{
			return true;
		}
		if (method == MethodName.CreateDefaultPacket)
		{
			return true;
		}
		if (method == MethodName.OpenPacketEditor)
		{
			return true;
		}
		if (method == MethodName.RefreshFromHistory)
		{
			return true;
		}
		if (method == MethodName.QueueRefreshFromHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshPreview)
		{
			return true;
		}
		if (method == MethodName.OnEdited)
		{
			return true;
		}
		if (method == MethodName.DisposeBindings)
		{
			return true;
		}
		if (method == MethodName.Panel)
		{
			return true;
		}
		if (method == MethodName.Section)
		{
			return true;
		}
		if (method == MethodName.StageBadge)
		{
			return true;
		}
		if (method == MethodName.Field)
		{
			return true;
		}
		if (method == MethodName.Number)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._event)
		{
			_event = VariantUtils.ConvertTo<TowerDefenseConveyorEventAddPacket>(in value);
			return true;
		}
		if (name == PropertyName._packetPicker)
		{
			_packetPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._previewPacket)
		{
			_previewPacket = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewRules)
		{
			_previewRules = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._weightBar)
		{
			_weightBar = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._packetFields)
		{
			_packetFields = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._refreshQueued)
		{
			_refreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._event)
		{
			value = VariantUtils.CreateFrom(in _event);
			return true;
		}
		if (name == PropertyName._packetPicker)
		{
			value = VariantUtils.CreateFrom(in _packetPicker);
			return true;
		}
		if (name == PropertyName._previewPacket)
		{
			value = VariantUtils.CreateFrom(in _previewPacket);
			return true;
		}
		if (name == PropertyName._previewRules)
		{
			value = VariantUtils.CreateFrom(in _previewRules);
			return true;
		}
		if (name == PropertyName._weightBar)
		{
			value = VariantUtils.CreateFrom(in _weightBar);
			return true;
		}
		if (name == PropertyName._packetFields)
		{
			value = VariantUtils.CreateFrom(in _packetFields);
			return true;
		}
		if (name == PropertyName._refreshQueued)
		{
			value = VariantUtils.CreateFrom(in _refreshQueued);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._event, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewRules, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._refreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._event, Variant.From(in _event));
		info.AddProperty(PropertyName._packetPicker, Variant.From(in _packetPicker));
		info.AddProperty(PropertyName._previewPacket, Variant.From(in _previewPacket));
		info.AddProperty(PropertyName._previewRules, Variant.From(in _previewRules));
		info.AddProperty(PropertyName._weightBar, Variant.From(in _weightBar));
		info.AddProperty(PropertyName._packetFields, Variant.From(in _packetFields));
		info.AddProperty(PropertyName._refreshQueued, Variant.From(in _refreshQueued));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._event, out var value))
		{
			_event = value.As<TowerDefenseConveyorEventAddPacket>();
		}
		if (info.TryGetProperty(PropertyName._packetPicker, out var value2))
		{
			_packetPicker = value2.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._previewPacket, out var value3))
		{
			_previewPacket = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewRules, out var value4))
		{
			_previewRules = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._weightBar, out var value5))
		{
			_weightBar = value5.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._packetFields, out var value6))
		{
			_packetFields = value6.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._refreshQueued, out var value7))
		{
			_refreshQueued = value7.As<bool>();
		}
	}
}
