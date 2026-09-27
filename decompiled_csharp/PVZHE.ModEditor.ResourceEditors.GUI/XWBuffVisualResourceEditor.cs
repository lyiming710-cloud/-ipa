using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWBuffVisualResourceEditor.cs")]
public class XWBuffVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName BuildWorkbench = "BuildWorkbench";

		public static readonly StringName BuildStage = "BuildStage";

		public static readonly StringName BuildFields = "BuildFields";

		public static readonly StringName CreateBinding = "CreateBinding";

		public static readonly StringName SetDrawBand = "SetDrawBand";

		public static readonly StringName BindVector2 = "BindVector2";

		public static readonly StringName SetResourceProperty = "SetResourceProperty";

		public static readonly StringName OnBuffVisualPropertyEdited = "OnBuffVisualPropertyEdited";

		public static readonly StringName RefreshBuffVisualEditorFromHistory = "RefreshBuffVisualEditorFromHistory";

		public static readonly StringName UpdatePreview = "UpdatePreview";

		public static readonly StringName RebuildPreviewVisual = "RebuildPreviewVisual";

		public static readonly StringName UpdatePreviewTransform = "UpdatePreviewTransform";

		public static readonly StringName DrawStageGuides = "DrawStageGuides";

		public static readonly StringName OnStageGuiInput = "OnStageGuiInput";

		public static readonly StringName CreatePicker = "CreatePicker";

		public static readonly StringName CreateSpin = "CreateSpin";

		public static readonly StringName CreateDrawBandButton = "CreateDrawBandButton";

		public static readonly StringName CreateSectionTitle = "CreateSectionTitle";

		public static readonly StringName CreateLabeledRow = "CreateLabeledRow";

		public static readonly StringName CreateVectorRow = "CreateVectorRow";

		public static readonly StringName CreatePanel = "CreatePanel";

		public static readonly StringName SetVectorNoSignal = "SetVectorNoSignal";

		public static readonly StringName DisposeBinding = "DisposeBinding";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _definition = "_definition";

		public static readonly StringName _stage = "_stage";

		public static readonly StringName _previewAnchor = "_previewAnchor";

		public static readonly StringName _visualBand = "_visualBand";

		public static readonly StringName _previewVisual = "_previewVisual";

		public static readonly StringName _mountedTexture = "_mountedTexture";

		public static readonly StringName _mountedScene = "_mountedScene";

		public static readonly StringName _emptyPreview = "_emptyPreview";

		public static readonly StringName _status = "_status";

		public static readonly StringName _resourceName = "_resourceName";

		public static readonly StringName _buffKey = "_buffKey";

		public static readonly StringName _nodeName = "_nodeName";

		public static readonly StringName _enabled = "_enabled";

		public static readonly StringName _localToScene = "_localToScene";

		public static readonly StringName _zAsRelative = "_zAsRelative";

		public static readonly StringName _centered = "_centered";

		public static readonly StringName _texturePicker = "_texturePicker";

		public static readonly StringName _scenePicker = "_scenePicker";

		public static readonly StringName _positionX = "_positionX";

		public static readonly StringName _positionY = "_positionY";

		public static readonly StringName _scaleX = "_scaleX";

		public static readonly StringName _scaleY = "_scaleY";

		public static readonly StringName _rotation = "_rotation";

		public static readonly StringName _zIndex = "_zIndex";

		public static readonly StringName _offsetX = "_offsetX";

		public static readonly StringName _offsetY = "_offsetY";

		public static readonly StringName _behindAnimationBand = "_behindAnimationBand";

		public static readonly StringName _frontAnimationBand = "_frontAnimationBand";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _dragging = "_dragging";

		public static readonly StringName _dragStartMouse = "_dragStartMouse";

		public static readonly StringName _dragStartPosition = "_dragStartPosition";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private BuffVisualDefinition _definition;

	private XWVisualPropertyBinding _binding;

	private Control _stage;

	private Node2D _previewAnchor;

	private Node2D _visualBand;

	private Node _previewVisual;

	private Texture2D _mountedTexture;

	private PackedScene _mountedScene;

	private Label _emptyPreview;

	private Label _status;

	private LineEdit _resourceName;

	private LineEdit _buffKey;

	private LineEdit _nodeName;

	private CheckButton _enabled;

	private CheckButton _localToScene;

	private CheckButton _zAsRelative;

	private CheckButton _centered;

	private XWResourcePicker _texturePicker;

	private XWResourcePicker _scenePicker;

	private SpinBox _positionX;

	private SpinBox _positionY;

	private SpinBox _scaleX;

	private SpinBox _scaleY;

	private SpinBox _rotation;

	private SpinBox _zIndex;

	private SpinBox _offsetX;

	private SpinBox _offsetY;

	private Button _behindAnimationBand;

	private Button _frontAnimationBand;

	private bool _updatingControls;

	private bool _dragging;

	private Vector2 _dragStartMouse;

	private Vector2 _dragStartPosition;

	public override void _ExitTree()
	{
		DisposeBinding();
		base._ExitTree();
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!(resource is BuffVisualDefinition))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeBinding();
		if (CurrentResource is BuffVisualDefinition definition && CanvasGrid != null)
		{
			_definition = definition;
			CanvasGrid.Columns = 1;
			CanvasGrid.AddChild(BuildWorkbench(), forceReadableName: false, InternalMode.Disabled);
			if (_buffKey == null)
			{
				_buffKey = CanvasGrid.FindChild("BuffKeyEdit", recursive: true, owned: false) as LineEdit;
			}
			CreateBinding();
			RefreshBuffVisualEditorFromHistory();
		}
	}

	private Control BuildWorkbench()
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Name = "BuffVisualWorkbench",
			CustomMinimumSize = new Vector2(0f, 560f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 10);
		PanelContainer panelContainer = CreatePanel(new Color(0.045f, 0.075f, 0.052f), new Color(0.35f, 0.72f, 0.32f), 10);
		HFlowContainer hFlowContainer = new HFlowContainer();
		hFlowContainer.AddThemeConstantOverride("separation", 12);
		panelContainer.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
		Label label = new Label
		{
			Text = "BUFF 外观装配台",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		label.AddThemeFontSizeOverride("font_size", 20);
		hFlowContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		_status = new Label
		{
			Name = "BuffVisualStatus",
			Text = "等待资源"
		};
		_status.AddThemeColorOverride("font_color", new Color(0.77f, 0.95f, 0.57f));
		hFlowContainer.AddChild(_status, forceReadableName: false, InternalMode.Disabled);
		Button button = new Button
		{
			Name = "SaveBuffVisualButton",
			Text = "保存 BUFF 外观",
			CustomMinimumSize = new Vector2(142f, 36f)
		};
		button.Pressed += SaveCurrentResource;
		hFlowContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		HSplitContainer hSplitContainer = new HSplitContainer();
		hSplitContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hSplitContainer.SizeFlagsVertical = SizeFlags.ExpandFill;
		hSplitContainer.SplitOffsets = new int[1] { 470 };
		HSplitContainer hSplitContainer2 = hSplitContainer;
		vBoxContainer.AddChild(hSplitContainer2, forceReadableName: false, InternalMode.Disabled);
		hSplitContainer2.AddChild(BuildStage(), forceReadableName: false, InternalMode.Disabled);
		hSplitContainer2.AddChild(BuildFields(), forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private Control BuildStage()
	{
		PanelContainer panelContainer = CreatePanel(new Color(0.018f, 0.032f, 0.024f), new Color(0.22f, 0.47f, 0.28f), 8);
		panelContainer.CustomMinimumSize = new Vector2(420f, 460f);
		panelContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		panelContainer.SizeFlagsVertical = SizeFlags.ExpandFill;
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 8);
		panelContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		Label label = new Label
		{
			Text = "游戏内挂件预览 · 在画面中拖动图像即可调整 position",
			AutowrapMode = TextServer.AutowrapMode.WordSmart
		};
		label.AddThemeColorOverride("font_color", new Color(0.72f, 0.86f, 0.67f));
		vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		_stage = new PanelContainer
		{
			Name = "BuffVisualStage",
			CustomMinimumSize = new Vector2(380f, 390f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill,
			MouseFilter = MouseFilterEnum.Stop,
			ClipContents = true
		};
		StyleBoxFlat stylebox = new StyleBoxFlat
		{
			BgColor = new Color(0.02f, 0.045f, 0.032f),
			BorderColor = new Color(0.2f, 0.42f, 0.24f),
			BorderWidthLeft = 1,
			BorderWidthTop = 1,
			BorderWidthRight = 1,
			BorderWidthBottom = 1,
			CornerRadiusTopLeft = 8,
			CornerRadiusTopRight = 8,
			CornerRadiusBottomLeft = 8,
			CornerRadiusBottomRight = 8
		};
		_stage.AddThemeStyleboxOverride("panel", stylebox);
		_stage.Draw += DrawStageGuides;
		_stage.Resized += UpdatePreviewTransform;
		_stage.GuiInput += OnStageGuiInput;
		vBoxContainer.AddChild(_stage, forceReadableName: false, InternalMode.Disabled);
		_previewAnchor = new Node2D
		{
			Name = "BuffPreviewAnchor"
		};
		_stage.AddChild(_previewAnchor, forceReadableName: false, InternalMode.Disabled);
		Polygon2D polygon2D = new Polygon2D();
		polygon2D.Name = "CharacterSilhouette";
		polygon2D.Polygon = new Vector2[7]
		{
			new Vector2(-42f, 86f),
			new Vector2(-54f, 8f),
			new Vector2(-36f, -75f),
			new Vector2(0f, -105f),
			new Vector2(36f, -75f),
			new Vector2(54f, 8f),
			new Vector2(42f, 86f)
		};
		polygon2D.Color = new Color(0.16f, 0.25f, 0.18f, 0.9f);
		polygon2D.ZIndex = 0;
		Polygon2D node = polygon2D;
		_previewAnchor.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		_visualBand = new Node2D
		{
			Name = "BuffVisualBand",
			ZIndex = 1000
		};
		_previewAnchor.AddChild(_visualBand, forceReadableName: false, InternalMode.Disabled);
		_emptyPreview = new Label
		{
			Name = "BuffVisualEmptyPreview",
			Text = "选择贴图或 Sprite2D 场景后在这里直接预览",
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			MouseFilter = MouseFilterEnum.Ignore
		};
		_emptyPreview.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect, LayoutPresetMode.Minsize);
		_emptyPreview.AddThemeColorOverride("font_color", new Color(0.48f, 0.63f, 0.5f));
		_stage.AddChild(_emptyPreview, forceReadableName: false, InternalMode.Disabled);
		return panelContainer;
	}

	private Control BuildFields()
	{
		ScrollContainer scrollContainer = new ScrollContainer
		{
			CustomMinimumSize = new Vector2(300f, 460f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 8);
		scrollContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(CreateSectionTitle("资源与 BUFF 身份"), forceReadableName: false, InternalMode.Disabled);
		_resourceName = new LineEdit
		{
			Name = "BuffResourceName",
			PlaceholderText = "资源名"
		};
		vBoxContainer.AddChild(CreateLabeledRow("资源名", _resourceName), forceReadableName: false, InternalMode.Disabled);
		_buffKey = new LineEdit
		{
			Name = "BuffKeyEdit",
			PlaceholderText = "例如 Frozen / Butter"
		};
		vBoxContainer.AddChild(CreateLabeledRow("BUFF 键", _buffKey), forceReadableName: false, InternalMode.Disabled);
		_nodeName = new LineEdit
		{
			Name = "BuffNodeName",
			PlaceholderText = "运行时 Sprite2D 节点名"
		};
		vBoxContainer.AddChild(CreateLabeledRow("节点名", _nodeName), forceReadableName: false, InternalMode.Disabled);
		_enabled = new CheckButton
		{
			Name = "BuffEnabled",
			Text = "运行时启用这个外观"
		};
		vBoxContainer.AddChild(_enabled, forceReadableName: false, InternalMode.Disabled);
		_localToScene = new CheckButton
		{
			Name = "BuffLocalToScene",
			Text = "资源仅属于当前场景"
		};
		vBoxContainer.AddChild(_localToScene, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(CreateSectionTitle("画面素材"), forceReadableName: false, InternalMode.Disabled);
		_texturePicker = CreatePicker("BuffTexturePicker", "Texture2D");
		vBoxContainer.AddChild(CreateLabeledRow("贴图", _texturePicker), forceReadableName: false, InternalMode.Disabled);
		_scenePicker = CreatePicker("BuffScenePicker", "PackedScene");
		vBoxContainer.AddChild(CreateLabeledRow("Sprite2D 场景", _scenePicker), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(CreateSectionTitle("画面变换"), forceReadableName: false, InternalMode.Disabled);
		_positionX = CreateSpin("BuffPositionX", -4096.0, 4096.0, 1.0);
		_positionY = CreateSpin("BuffPositionY", -4096.0, 4096.0, 1.0);
		vBoxContainer.AddChild(CreateVectorRow("位置", _positionX, _positionY), forceReadableName: false, InternalMode.Disabled);
		_scaleX = CreateSpin("BuffScaleX", -100.0, 100.0, 0.05, 1.0);
		_scaleY = CreateSpin("BuffScaleY", -100.0, 100.0, 0.05, 1.0);
		vBoxContainer.AddChild(CreateVectorRow("缩放", _scaleX, _scaleY), forceReadableName: false, InternalMode.Disabled);
		_rotation = CreateSpin("BuffRotation", -62.831854820251465, 62.831854820251465, 0.01);
		vBoxContainer.AddChild(CreateLabeledRow("旋转（弧度）", _rotation), forceReadableName: false, InternalMode.Disabled);
		_offsetX = CreateSpin("BuffOffsetX", -4096.0, 4096.0, 1.0);
		_offsetY = CreateSpin("BuffOffsetY", -4096.0, 4096.0, 1.0);
		vBoxContainer.AddChild(CreateVectorRow("贴图偏移", _offsetX, _offsetY), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(CreateSectionTitle("绘制层级"), forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			Name = "BuffDrawBand",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		ButtonGroup buttonGroup = new ButtonGroup();
		_behindAnimationBand = CreateDrawBandButton("◀ 动画后方", "BuffBehindAnimationBand", buttonGroup);
		_frontAnimationBand = CreateDrawBandButton("动画前方 ▶", "BuffFrontAnimationBand", buttonGroup);
		hBoxContainer.AddChild(_behindAnimationBand, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(_frontAnimationBand, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(CreateLabeledRow("动画绘制带", hBoxContainer), forceReadableName: false, InternalMode.Disabled);
		_zIndex = CreateSpin("BuffZIndex", -4096.0, 4096.0, 1.0);
		vBoxContainer.AddChild(CreateLabeledRow("局部 Z", _zIndex), forceReadableName: false, InternalMode.Disabled);
		_zAsRelative = new CheckButton
		{
			Name = "BuffZAsRelative",
			Text = "Z 相对父节点"
		};
		vBoxContainer.AddChild(_zAsRelative, forceReadableName: false, InternalMode.Disabled);
		_centered = new CheckButton
		{
			Name = "BuffCentered",
			Text = "贴图以中心对齐"
		};
		vBoxContainer.AddChild(_centered, forceReadableName: false, InternalMode.Disabled);
		return scrollContainer;
	}

	private void CreateBinding()
	{
		_binding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnBuffVisualPropertyEdited);
		_binding.BindText(_resourceName, _definition, "resource_name", UpdatePreview, this, "RefreshBuffVisualEditorFromHistory");
		_binding.BindText(_buffKey, _definition, "buffKey", UpdatePreview, this, "RefreshBuffVisualEditorFromHistory");
		_binding.BindText(_nodeName, _definition, "nodeName", UpdatePreview, this, "RefreshBuffVisualEditorFromHistory");
		_binding.BindToggle(_enabled, _definition, "enabled", UpdatePreview, this, "RefreshBuffVisualEditorFromHistory");
		_binding.BindToggle(_localToScene, _definition, "resource_local_to_scene", UpdatePreview, this, "RefreshBuffVisualEditorFromHistory");
		_binding.BindToggle(_zAsRelative, _definition, "zAsRelative", UpdatePreview, this, "RefreshBuffVisualEditorFromHistory");
		_binding.BindToggle(_centered, _definition, "centered", UpdatePreview, this, "RefreshBuffVisualEditorFromHistory");
		_binding.BindNumber(_rotation, _definition, "rotation", UpdatePreview, this, "RefreshBuffVisualEditorFromHistory");
		_binding.BindNumber(_zIndex, _definition, "zIndex", UpdatePreview, this, "RefreshBuffVisualEditorFromHistory");
		BindVector2(_positionX, _positionY, "position", "调整 BUFF 外观位置");
		BindVector2(_scaleX, _scaleY, "scale", "调整 BUFF 外观缩放");
		BindVector2(_offsetX, _offsetY, "offset", "调整 BUFF 贴图偏移");
		_texturePicker.ResourceChanged += (Resource resource) =>
		{
			SetResourceProperty("texture", resource, "更换 BUFF 外观贴图");
		};
		_scenePicker.ResourceChanged += (Resource resource) =>
		{
			SetResourceProperty("scene", resource, "更换 BUFF 外观场景");
		};
		_behindAnimationBand.Pressed += () =>
		{
			SetDrawBand(AdobeAnimateExternalVisualDrawBand.BehindAnimation);
		};
		_frontAnimationBand.Pressed += () =>
		{
			SetDrawBand(AdobeAnimateExternalVisualDrawBand.InFrontOfAnimation);
		};
	}

	private void SetDrawBand(AdobeAnimateExternalVisualDrawBand value)
	{
		if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_definition))
		{
			_binding.SetValue(_definition, "drawBand", (int)value, "切换 BUFF 动画绘制带", this, "RefreshBuffVisualEditorFromHistory");
			UpdatePreview();
		}
	}

	private void BindVector2(SpinBox x, SpinBox y, StringName property, string actionName)
	{
		x.FocusEntered += Begin;
		y.FocusEntered += Begin;
		x.ValueChanged += (double _) =>
		{
			Preview();
		};
		y.ValueChanged += (double _) =>
		{
			Preview();
		};
		x.FocusExited += Commit;
		y.FocusExited += Commit;
		void Begin()
		{
			if (!_updatingControls)
			{
				_binding?.BeginEdit(_definition, property);
			}
		}
		void Commit()
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_definition))
			{
				_binding.CommitEdit(_definition, property, _definition.Get(property), actionName, this, "RefreshBuffVisualEditorFromHistory");
				UpdatePreview();
			}
		}
		void Preview()
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_definition))
			{
				_binding.PreviewValue(_definition, property, new Vector2((float)x.Value, (float)y.Value));
				UpdatePreview();
			}
		}
	}

	private void SetResourceProperty(StringName property, Resource resource, string actionName)
	{
		if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_definition))
		{
			_binding.SetValue(_definition, property, resource, actionName, this, "RefreshBuffVisualEditorFromHistory");
			RefreshBuffVisualEditorFromHistory();
		}
	}

	private void OnBuffVisualPropertyEdited(bool committed)
	{
		if (CurrentResource == _definition)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
				return;
			}
			MarkCurrentResourceDirty();
			_definition.EmitChanged();
		}
	}

	public void RefreshBuffVisualEditorFromHistory()
	{
		if (GodotObject.IsInstanceValid(_definition) && GodotObject.IsInstanceValid(_resourceName) && GodotObject.IsInstanceValid(_buffKey) && GodotObject.IsInstanceValid(_nodeName) && GodotObject.IsInstanceValid(_enabled) && GodotObject.IsInstanceValid(_localToScene) && GodotObject.IsInstanceValid(_zAsRelative) && GodotObject.IsInstanceValid(_centered) && GodotObject.IsInstanceValid(_texturePicker) && GodotObject.IsInstanceValid(_scenePicker) && GodotObject.IsInstanceValid(_positionX) && GodotObject.IsInstanceValid(_positionY) && GodotObject.IsInstanceValid(_scaleX) && GodotObject.IsInstanceValid(_scaleY) && GodotObject.IsInstanceValid(_offsetX) && GodotObject.IsInstanceValid(_offsetY) && GodotObject.IsInstanceValid(_rotation) && GodotObject.IsInstanceValid(_zIndex) && GodotObject.IsInstanceValid(_behindAnimationBand) && GodotObject.IsInstanceValid(_frontAnimationBand))
		{
			_updatingControls = true;
			try
			{
				_resourceName.Text = _definition.ResourceName;
				_buffKey.Text = _definition.buffKey?.ToString() ?? "";
				_nodeName.Text = _definition.nodeName?.ToString() ?? "";
				_enabled.SetPressedNoSignal(_definition.enabled);
				_localToScene.SetPressedNoSignal(_definition.ResourceLocalToScene);
				_zAsRelative.SetPressedNoSignal(_definition.zAsRelative);
				_centered.SetPressedNoSignal(_definition.centered);
				_texturePicker.SetEditedResource(_definition.texture);
				_scenePicker.SetEditedResource(_definition.scene);
				SetVectorNoSignal(_positionX, _positionY, _definition.position);
				SetVectorNoSignal(_scaleX, _scaleY, _definition.scale);
				SetVectorNoSignal(_offsetX, _offsetY, _definition.offset);
				_rotation.SetValueNoSignal(_definition.rotation);
				_zIndex.SetValueNoSignal(_definition.zIndex);
				_behindAnimationBand.SetPressedNoSignal(_definition.drawBand == AdobeAnimateExternalVisualDrawBand.BehindAnimation);
				_frontAnimationBand.SetPressedNoSignal(_definition.drawBand == AdobeAnimateExternalVisualDrawBand.InFrontOfAnimation);
			}
			finally
			{
				_updatingControls = false;
			}
			UpdatePreview();
		}
	}

	private void UpdatePreview()
	{
		if (!GodotObject.IsInstanceValid(_definition) || !GodotObject.IsInstanceValid(_visualBand))
		{
			return;
		}
		if (_mountedTexture != _definition.texture || _mountedScene != _definition.scene || !GodotObject.IsInstanceValid(_previewVisual))
		{
			RebuildPreviewVisual();
		}
		if (_previewVisual is Sprite2D sprite2D)
		{
			if (GodotObject.IsInstanceValid(_definition.texture))
			{
				sprite2D.Texture = _definition.texture;
			}
			sprite2D.Name = (string.IsNullOrEmpty(_definition.nodeName?.ToString()) ? new StringName("BuffVisualPreview") : _definition.nodeName);
			sprite2D.Position = _definition.position;
			sprite2D.Scale = _definition.scale;
			sprite2D.Rotation = _definition.rotation;
			sprite2D.ZIndex = _definition.zIndex;
			sprite2D.ZAsRelative = _definition.zAsRelative;
			sprite2D.Centered = _definition.centered;
			sprite2D.Offset = _definition.offset;
			sprite2D.Visible = _definition.enabled;
		}
		_visualBand.ZIndex = ((_definition.drawBand == AdobeAnimateExternalVisualDrawBand.BehindAnimation) ? (-1000) : 1000);
		_emptyPreview.Visible = !GodotObject.IsInstanceValid(_definition.texture) && !GodotObject.IsInstanceValid(_definition.scene);
		_status.Text = $"{_definition.buffKey} · {(_definition.enabled ? "启用" : "停用")} · {((_definition.drawBand == AdobeAnimateExternalVisualDrawBand.BehindAnimation) ? "动画后方" : "动画前方")}";
		UpdatePreviewTransform();
	}

	private void RebuildPreviewVisual()
	{
		if (GodotObject.IsInstanceValid(_previewVisual))
		{
			if (_previewVisual.GetParent() == _visualBand)
			{
				_visualBand.RemoveChild(_previewVisual);
			}
			_previewVisual.QueueFree();
			_previewVisual = null;
		}
		Sprite2D sprite2D = null;
		if (GodotObject.IsInstanceValid(_definition.scene))
		{
			Node node = _definition.scene.Instantiate(PackedScene.GenEditState.Disabled);
			if (node is Sprite2D sprite2D2)
			{
				sprite2D = sprite2D2;
			}
			else
			{
				node?.Free();
			}
		}
		if (sprite2D == null)
		{
			sprite2D = new Sprite2D();
		}
		if (GodotObject.IsInstanceValid(_definition.texture))
		{
			sprite2D.Texture = _definition.texture;
		}
		sprite2D.ProcessMode = ProcessModeEnum.Disabled;
		_previewVisual = sprite2D;
		_visualBand.AddChild(sprite2D, forceReadableName: false, InternalMode.Disabled);
		_mountedTexture = _definition.texture;
		_mountedScene = _definition.scene;
	}

	private void UpdatePreviewTransform()
	{
		if (GodotObject.IsInstanceValid(_stage) && GodotObject.IsInstanceValid(_previewAnchor))
		{
			_previewAnchor.Position = _stage.Size * 0.5f;
		}
		_stage?.QueueRedraw();
	}

	private void DrawStageGuides()
	{
		if (GodotObject.IsInstanceValid(_stage))
		{
			Vector2 position = _stage.Size * 0.5f;
			Color color = new Color(0.22f, 0.42f, 0.26f, 0.38f);
			_stage.DrawLine(new Vector2(0f, position.Y), new Vector2(_stage.Size.X, position.Y), color, 1f);
			_stage.DrawLine(new Vector2(position.X, 0f), new Vector2(position.X, _stage.Size.Y), color, 1f);
			_stage.DrawCircle(position, 4f, new Color(0.62f, 0.86f, 0.4f, 0.8f));
		}
	}

	private void OnStageGuiInput(InputEvent inputEvent)
	{
		if (!GodotObject.IsInstanceValid(_definition) || _binding == null)
		{
			return;
		}
		if (inputEvent is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			if (inputEventMouseButton.Pressed)
			{
				_dragging = true;
				_dragStartMouse = inputEventMouseButton.Position;
				_dragStartPosition = _definition.position;
				_binding.BeginEdit(_definition, "position");
			}
			else if (_dragging)
			{
				_dragging = false;
				_binding.CommitEdit(_definition, "position", _definition.position, "在预览画面拖动 BUFF 外观", this, "RefreshBuffVisualEditorFromHistory");
			}
			_stage.AcceptEvent();
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _dragging)
		{
			Vector2 vector = _dragStartPosition + inputEventMouseMotion.Position - _dragStartMouse;
			_binding.PreviewValue(_definition, "position", vector);
			_updatingControls = true;
			SetVectorNoSignal(_positionX, _positionY, vector);
			_updatingControls = false;
			UpdatePreview();
			_stage.AcceptEvent();
		}
	}

	private static XWResourcePicker CreatePicker(string name, string type)
	{
		XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
		xWResourcePicker.Name = name;
		xWResourcePicker.Setup(type);
		xWResourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		return xWResourcePicker;
	}

	private static SpinBox CreateSpin(string name, double minimum, double maximum, double step, double value = 0.0)
	{
		return new SpinBox
		{
			Name = name,
			MinValue = minimum,
			MaxValue = maximum,
			Step = step,
			Value = value,
			AllowGreater = true,
			AllowLesser = true,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
	}

	private static Button CreateDrawBandButton(string text, string name, ButtonGroup group)
	{
		return new Button
		{
			Name = name,
			Text = text,
			ToggleMode = true,
			ButtonGroup = group,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			TooltipText = "在游戏角色预览中切换前后绘制层"
		};
	}

	private static Control CreateSectionTitle(string text)
	{
		Label label = new Label();
		label.Text = text;
		label.AddThemeFontSizeOverride("font_size", 17);
		label.AddThemeColorOverride("font_color", new Color(0.77f, 0.93f, 0.58f));
		return label;
	}

	private static Control CreateLabeledRow(string text, Control field)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddThemeConstantOverride("separation", 8);
		hBoxContainer.AddChild(new Label
		{
			Text = text,
			CustomMinimumSize = new Vector2(108f, 0f)
		}, forceReadableName: false, InternalMode.Disabled);
		field.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddChild(field, forceReadableName: false, InternalMode.Disabled);
		return hBoxContainer;
	}

	private static Control CreateVectorRow(string text, SpinBox x, SpinBox y)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddThemeConstantOverride("separation", 6);
		hBoxContainer.AddChild(new Label
		{
			Text = text,
			CustomMinimumSize = new Vector2(108f, 0f)
		}, forceReadableName: false, InternalMode.Disabled);
		x.Prefix = "X ";
		y.Prefix = "Y ";
		x.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		y.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddChild(x, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(y, forceReadableName: false, InternalMode.Disabled);
		return hBoxContainer;
	}

	private static PanelContainer CreatePanel(Color background, Color border, int radius)
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.AddThemeStyleboxOverride(stylebox: new StyleBoxFlat
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
			ContentMarginLeft = 10f,
			ContentMarginTop = 10f,
			ContentMarginRight = 10f,
			ContentMarginBottom = 10f
		}, name: "panel");
		return panelContainer;
	}

	private static void SetVectorNoSignal(SpinBox x, SpinBox y, Vector2 value)
	{
		x.SetValueNoSignal(value.X);
		y.SetValueNoSignal(value.Y);
	}

	private void DisposeBinding()
	{
		_binding?.Dispose();
		_binding = null;
		_definition = null;
		_previewVisual = null;
		_mountedTexture = null;
		_mountedScene = null;
		_dragging = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(24)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildWorkbench, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildStage, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildFields, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetDrawBand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindVector2, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "x", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "y", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetResourceProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnBuffVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshBuffVisualEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildPreviewVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePreviewTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawStageGuides, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnStageGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePicker, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSpin, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateDrawBandButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "group", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ButtonGroup"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSectionTitle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateLabeledRow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "field", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateVectorRow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "x", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "y", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "background", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "border", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "radius", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetVectorNoSignal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "x", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "y", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BuildStage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(BuildStage());
			return true;
		}
		if (method == MethodName.BuildFields && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(BuildFields());
			return true;
		}
		if (method == MethodName.CreateBinding && args.Count == 0)
		{
			CreateBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.SetDrawBand && args.Count == 1)
		{
			SetDrawBand(VariantUtils.ConvertTo<AdobeAnimateExternalVisualDrawBand>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindVector2 && args.Count == 4)
		{
			BindVector2(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetResourceProperty && args.Count == 3)
		{
			SetResourceProperty(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnBuffVisualPropertyEdited && args.Count == 1)
		{
			OnBuffVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshBuffVisualEditorFromHistory && args.Count == 0)
		{
			RefreshBuffVisualEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreview && args.Count == 0)
		{
			UpdatePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildPreviewVisual && args.Count == 0)
		{
			RebuildPreviewVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreviewTransform && args.Count == 0)
		{
			UpdatePreviewTransform();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawStageGuides && args.Count == 0)
		{
			DrawStageGuides();
			ret = default;
			return true;
		}
		if (method == MethodName.OnStageGuiInput && args.Count == 1)
		{
			OnStageGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePicker && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWResourcePicker>(CreatePicker(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateSpin && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateSpin(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<double>(in args[4])));
			return true;
		}
		if (method == MethodName.CreateDrawBandButton && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Button>(CreateDrawBandButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<ButtonGroup>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateSectionTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateSectionTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateLabeledRow && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateLabeledRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateVectorRow && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateVectorRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2])));
			return true;
		}
		if (method == MethodName.CreatePanel && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(CreatePanel(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.SetVectorNoSignal && args.Count == 3)
		{
			SetVectorNoSignal(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeBinding && args.Count == 0)
		{
			DisposeBinding();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreatePicker && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWResourcePicker>(CreatePicker(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateSpin && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateSpin(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<double>(in args[4])));
			return true;
		}
		if (method == MethodName.CreateDrawBandButton && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Button>(CreateDrawBandButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<ButtonGroup>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateSectionTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateSectionTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateLabeledRow && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateLabeledRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateVectorRow && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateVectorRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2])));
			return true;
		}
		if (method == MethodName.CreatePanel && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(CreatePanel(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.SetVectorNoSignal && args.Count == 3)
		{
			SetVectorNoSignal(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
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
		if (method == MethodName.BuildStage)
		{
			return true;
		}
		if (method == MethodName.BuildFields)
		{
			return true;
		}
		if (method == MethodName.CreateBinding)
		{
			return true;
		}
		if (method == MethodName.SetDrawBand)
		{
			return true;
		}
		if (method == MethodName.BindVector2)
		{
			return true;
		}
		if (method == MethodName.SetResourceProperty)
		{
			return true;
		}
		if (method == MethodName.OnBuffVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshBuffVisualEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.UpdatePreview)
		{
			return true;
		}
		if (method == MethodName.RebuildPreviewVisual)
		{
			return true;
		}
		if (method == MethodName.UpdatePreviewTransform)
		{
			return true;
		}
		if (method == MethodName.DrawStageGuides)
		{
			return true;
		}
		if (method == MethodName.OnStageGuiInput)
		{
			return true;
		}
		if (method == MethodName.CreatePicker)
		{
			return true;
		}
		if (method == MethodName.CreateSpin)
		{
			return true;
		}
		if (method == MethodName.CreateDrawBandButton)
		{
			return true;
		}
		if (method == MethodName.CreateSectionTitle)
		{
			return true;
		}
		if (method == MethodName.CreateLabeledRow)
		{
			return true;
		}
		if (method == MethodName.CreateVectorRow)
		{
			return true;
		}
		if (method == MethodName.CreatePanel)
		{
			return true;
		}
		if (method == MethodName.SetVectorNoSignal)
		{
			return true;
		}
		if (method == MethodName.DisposeBinding)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._definition)
		{
			_definition = VariantUtils.ConvertTo<BuffVisualDefinition>(in value);
			return true;
		}
		if (name == PropertyName._stage)
		{
			_stage = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._previewAnchor)
		{
			_previewAnchor = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._visualBand)
		{
			_visualBand = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._previewVisual)
		{
			_previewVisual = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._mountedTexture)
		{
			_mountedTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._mountedScene)
		{
			_mountedScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName._emptyPreview)
		{
			_emptyPreview = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._status)
		{
			_status = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._resourceName)
		{
			_resourceName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._buffKey)
		{
			_buffKey = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._nodeName)
		{
			_nodeName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._enabled)
		{
			_enabled = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._localToScene)
		{
			_localToScene = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._zAsRelative)
		{
			_zAsRelative = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._centered)
		{
			_centered = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._texturePicker)
		{
			_texturePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._scenePicker)
		{
			_scenePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._positionX)
		{
			_positionX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._positionY)
		{
			_positionY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._scaleX)
		{
			_scaleX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._scaleY)
		{
			_scaleY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._rotation)
		{
			_rotation = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._zIndex)
		{
			_zIndex = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._offsetX)
		{
			_offsetX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._offsetY)
		{
			_offsetY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._behindAnimationBand)
		{
			_behindAnimationBand = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._frontAnimationBand)
		{
			_frontAnimationBand = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragging)
		{
			_dragging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragStartMouse)
		{
			_dragStartMouse = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._dragStartPosition)
		{
			_dragStartPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._definition)
		{
			value = VariantUtils.CreateFrom(in _definition);
			return true;
		}
		if (name == PropertyName._stage)
		{
			value = VariantUtils.CreateFrom(in _stage);
			return true;
		}
		if (name == PropertyName._previewAnchor)
		{
			value = VariantUtils.CreateFrom(in _previewAnchor);
			return true;
		}
		if (name == PropertyName._visualBand)
		{
			value = VariantUtils.CreateFrom(in _visualBand);
			return true;
		}
		if (name == PropertyName._previewVisual)
		{
			value = VariantUtils.CreateFrom(in _previewVisual);
			return true;
		}
		if (name == PropertyName._mountedTexture)
		{
			value = VariantUtils.CreateFrom(in _mountedTexture);
			return true;
		}
		if (name == PropertyName._mountedScene)
		{
			value = VariantUtils.CreateFrom(in _mountedScene);
			return true;
		}
		if (name == PropertyName._emptyPreview)
		{
			value = VariantUtils.CreateFrom(in _emptyPreview);
			return true;
		}
		if (name == PropertyName._status)
		{
			value = VariantUtils.CreateFrom(in _status);
			return true;
		}
		if (name == PropertyName._resourceName)
		{
			value = VariantUtils.CreateFrom(in _resourceName);
			return true;
		}
		if (name == PropertyName._buffKey)
		{
			value = VariantUtils.CreateFrom(in _buffKey);
			return true;
		}
		if (name == PropertyName._nodeName)
		{
			value = VariantUtils.CreateFrom(in _nodeName);
			return true;
		}
		if (name == PropertyName._enabled)
		{
			value = VariantUtils.CreateFrom(in _enabled);
			return true;
		}
		if (name == PropertyName._localToScene)
		{
			value = VariantUtils.CreateFrom(in _localToScene);
			return true;
		}
		if (name == PropertyName._zAsRelative)
		{
			value = VariantUtils.CreateFrom(in _zAsRelative);
			return true;
		}
		if (name == PropertyName._centered)
		{
			value = VariantUtils.CreateFrom(in _centered);
			return true;
		}
		if (name == PropertyName._texturePicker)
		{
			value = VariantUtils.CreateFrom(in _texturePicker);
			return true;
		}
		if (name == PropertyName._scenePicker)
		{
			value = VariantUtils.CreateFrom(in _scenePicker);
			return true;
		}
		if (name == PropertyName._positionX)
		{
			value = VariantUtils.CreateFrom(in _positionX);
			return true;
		}
		if (name == PropertyName._positionY)
		{
			value = VariantUtils.CreateFrom(in _positionY);
			return true;
		}
		if (name == PropertyName._scaleX)
		{
			value = VariantUtils.CreateFrom(in _scaleX);
			return true;
		}
		if (name == PropertyName._scaleY)
		{
			value = VariantUtils.CreateFrom(in _scaleY);
			return true;
		}
		if (name == PropertyName._rotation)
		{
			value = VariantUtils.CreateFrom(in _rotation);
			return true;
		}
		if (name == PropertyName._zIndex)
		{
			value = VariantUtils.CreateFrom(in _zIndex);
			return true;
		}
		if (name == PropertyName._offsetX)
		{
			value = VariantUtils.CreateFrom(in _offsetX);
			return true;
		}
		if (name == PropertyName._offsetY)
		{
			value = VariantUtils.CreateFrom(in _offsetY);
			return true;
		}
		if (name == PropertyName._behindAnimationBand)
		{
			value = VariantUtils.CreateFrom(in _behindAnimationBand);
			return true;
		}
		if (name == PropertyName._frontAnimationBand)
		{
			value = VariantUtils.CreateFrom(in _frontAnimationBand);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._dragging)
		{
			value = VariantUtils.CreateFrom(in _dragging);
			return true;
		}
		if (name == PropertyName._dragStartMouse)
		{
			value = VariantUtils.CreateFrom(in _dragStartMouse);
			return true;
		}
		if (name == PropertyName._dragStartPosition)
		{
			value = VariantUtils.CreateFrom(in _dragStartPosition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewAnchor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._visualBand, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewVisual, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mountedTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mountedScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._status, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._buffKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nodeName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._enabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zAsRelative, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._centered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texturePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scenePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._positionX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._positionY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scaleX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scaleY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rotation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._offsetX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._offsetY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._behindAnimationBand, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._frontAnimationBand, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dragging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._dragStartMouse, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._dragStartPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
		info.AddProperty(PropertyName._stage, Variant.From(in _stage));
		info.AddProperty(PropertyName._previewAnchor, Variant.From(in _previewAnchor));
		info.AddProperty(PropertyName._visualBand, Variant.From(in _visualBand));
		info.AddProperty(PropertyName._previewVisual, Variant.From(in _previewVisual));
		info.AddProperty(PropertyName._mountedTexture, Variant.From(in _mountedTexture));
		info.AddProperty(PropertyName._mountedScene, Variant.From(in _mountedScene));
		info.AddProperty(PropertyName._emptyPreview, Variant.From(in _emptyPreview));
		info.AddProperty(PropertyName._status, Variant.From(in _status));
		info.AddProperty(PropertyName._resourceName, Variant.From(in _resourceName));
		info.AddProperty(PropertyName._buffKey, Variant.From(in _buffKey));
		info.AddProperty(PropertyName._nodeName, Variant.From(in _nodeName));
		info.AddProperty(PropertyName._enabled, Variant.From(in _enabled));
		info.AddProperty(PropertyName._localToScene, Variant.From(in _localToScene));
		info.AddProperty(PropertyName._zAsRelative, Variant.From(in _zAsRelative));
		info.AddProperty(PropertyName._centered, Variant.From(in _centered));
		info.AddProperty(PropertyName._texturePicker, Variant.From(in _texturePicker));
		info.AddProperty(PropertyName._scenePicker, Variant.From(in _scenePicker));
		info.AddProperty(PropertyName._positionX, Variant.From(in _positionX));
		info.AddProperty(PropertyName._positionY, Variant.From(in _positionY));
		info.AddProperty(PropertyName._scaleX, Variant.From(in _scaleX));
		info.AddProperty(PropertyName._scaleY, Variant.From(in _scaleY));
		info.AddProperty(PropertyName._rotation, Variant.From(in _rotation));
		info.AddProperty(PropertyName._zIndex, Variant.From(in _zIndex));
		info.AddProperty(PropertyName._offsetX, Variant.From(in _offsetX));
		info.AddProperty(PropertyName._offsetY, Variant.From(in _offsetY));
		info.AddProperty(PropertyName._behindAnimationBand, Variant.From(in _behindAnimationBand));
		info.AddProperty(PropertyName._frontAnimationBand, Variant.From(in _frontAnimationBand));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._dragging, Variant.From(in _dragging));
		info.AddProperty(PropertyName._dragStartMouse, Variant.From(in _dragStartMouse));
		info.AddProperty(PropertyName._dragStartPosition, Variant.From(in _dragStartPosition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._definition, out var value))
		{
			_definition = value.As<BuffVisualDefinition>();
		}
		if (info.TryGetProperty(PropertyName._stage, out var value2))
		{
			_stage = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._previewAnchor, out var value3))
		{
			_previewAnchor = value3.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._visualBand, out var value4))
		{
			_visualBand = value4.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._previewVisual, out var value5))
		{
			_previewVisual = value5.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._mountedTexture, out var value6))
		{
			_mountedTexture = value6.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._mountedScene, out var value7))
		{
			_mountedScene = value7.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName._emptyPreview, out var value8))
		{
			_emptyPreview = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._status, out var value9))
		{
			_status = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resourceName, out var value10))
		{
			_resourceName = value10.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._buffKey, out var value11))
		{
			_buffKey = value11.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._nodeName, out var value12))
		{
			_nodeName = value12.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._enabled, out var value13))
		{
			_enabled = value13.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._localToScene, out var value14))
		{
			_localToScene = value14.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._zAsRelative, out var value15))
		{
			_zAsRelative = value15.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._centered, out var value16))
		{
			_centered = value16.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._texturePicker, out var value17))
		{
			_texturePicker = value17.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._scenePicker, out var value18))
		{
			_scenePicker = value18.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._positionX, out var value19))
		{
			_positionX = value19.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._positionY, out var value20))
		{
			_positionY = value20.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._scaleX, out var value21))
		{
			_scaleX = value21.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._scaleY, out var value22))
		{
			_scaleY = value22.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._rotation, out var value23))
		{
			_rotation = value23.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._zIndex, out var value24))
		{
			_zIndex = value24.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._offsetX, out var value25))
		{
			_offsetX = value25.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._offsetY, out var value26))
		{
			_offsetY = value26.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._behindAnimationBand, out var value27))
		{
			_behindAnimationBand = value27.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._frontAnimationBand, out var value28))
		{
			_frontAnimationBand = value28.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value29))
		{
			_updatingControls = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragging, out var value30))
		{
			_dragging = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragStartMouse, out var value31))
		{
			_dragStartMouse = value31.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._dragStartPosition, out var value32))
		{
			_dragStartPosition = value32.As<Vector2>();
		}
	}
}
