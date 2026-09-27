using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCollisionGeometryVisualResourceEditor.cs")]
public class XWCollisionGeometryVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName IsCollisionResource = "IsCollisionResource";

		public static readonly StringName BuildWorkbench = "BuildWorkbench";

		public static readonly StringName BuildIdentityFields = "BuildIdentityFields";

		public static readonly StringName BuildTransformFields = "BuildTransformFields";

		public static readonly StringName BuildGeometryFields = "BuildGeometryFields";

		public static readonly StringName BuildDebugFields = "BuildDebugFields";

		public static readonly StringName CreateBinding = "CreateBinding";

		public static readonly StringName PopulateControls = "PopulateControls";

		public static readonly StringName RebuildGeometryFields = "RebuildGeometryFields";

		public static readonly StringName AddShapeGeometryFields = "AddShapeGeometryFields";

		public static readonly StringName AddVectorPropertyFields = "AddVectorPropertyFields";

		public static readonly StringName AddNumberPropertyField = "AddNumberPropertyField";

		public static readonly StringName BindVectorComponent = "BindVectorComponent";

		public static readonly StringName BindTransformControl = "BindTransformControl";

		public static readonly StringName BuildTransformFromControls = "BuildTransformFromControls";

		public static readonly StringName CommitCanvasOrigin = "CommitCanvasOrigin";

		public static readonly StringName CommitCanvasGeometry = "CommitCanvasGeometry";

		public static readonly StringName CommitCapsuleGeometry = "CommitCapsuleGeometry";

		public static readonly StringName ChangeShapeType = "ChangeShapeType";

		public static readonly StringName ChangeShapeResource = "ChangeShapeResource";

		public static readonly StringName BindColorControl = "BindColorControl";

		public static readonly StringName OnCollisionPropertyEdited = "OnCollisionPropertyEdited";

		public static readonly StringName RefreshCollisionEditorFromHistory = "RefreshCollisionEditorFromHistory";

		public static readonly StringName UpdateCollisionPreview = "UpdateCollisionPreview";

		public static readonly StringName GetLocalTransform = "GetLocalTransform";

		public static readonly StringName GetCurrentShapeSize = "GetCurrentShapeSize";

		public static readonly StringName GetColor = "GetColor";

		public static readonly StringName SelectShapeType = "SelectShapeType";

		public static readonly StringName LoadCollisionShapeIcon = "LoadCollisionShapeIcon";

		public static readonly StringName DisposeCollisionBinding = "DisposeCollisionBinding";

		public static readonly StringName CreateSectionTitle = "CreateSectionTitle";

		public static readonly StringName CreateLabeledRow = "CreateLabeledRow";

		public static readonly StringName CreateVectorRow = "CreateVectorRow";

		public static readonly StringName CreateSpin = "CreateSpin";

		public static readonly StringName CreatePanel = "CreatePanel";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingCollision = "_editingCollision";

		public static readonly StringName _canvas = "_canvas";

		public static readonly StringName _typeBadge = "_typeBadge";

		public static readonly StringName _geometrySummary = "_geometrySummary";

		public static readonly StringName _resourceName = "_resourceName";

		public static readonly StringName _localToScene = "_localToScene";

		public static readonly StringName _enabled = "_enabled";

		public static readonly StringName _monitorable = "_monitorable";

		public static readonly StringName _debugDraw = "_debugDraw";

		public static readonly StringName _fillColor = "_fillColor";

		public static readonly StringName _outlineColor = "_outlineColor";

		public static readonly StringName _lineWidth = "_lineWidth";

		public static readonly StringName _originX = "_originX";

		public static readonly StringName _originY = "_originY";

		public static readonly StringName _rotation = "_rotation";

		public static readonly StringName _scaleX = "_scaleX";

		public static readonly StringName _scaleY = "_scaleY";

		public static readonly StringName _shapeType = "_shapeType";

		public static readonly StringName _shapeTypeSegments = "_shapeTypeSegments";

		public static readonly StringName _shapePicker = "_shapePicker";

		public static readonly StringName _shapeFields = "_shapeFields";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private XWVisualPropertyBinding _binding;

	private Resource _editingCollision;

	private XWCollisionGeometryCanvas _canvas;

	private Label _typeBadge;

	private Label _geometrySummary;

	private LineEdit _resourceName;

	private CheckButton _localToScene;

	private CheckButton _enabled;

	private CheckButton _monitorable;

	private CheckButton _debugDraw;

	private ColorPickerButton _fillColor;

	private ColorPickerButton _outlineColor;

	private SpinBox _lineWidth;

	private SpinBox _originX;

	private SpinBox _originY;

	private SpinBox _rotation;

	private SpinBox _scaleX;

	private SpinBox _scaleY;

	private OptionButton _shapeType;

	private HFlowContainer _shapeTypeSegments;

	private XWVisualSegmentedOption _shapeTypeVisualChoice;

	private XWResourcePicker _shapePicker;

	private VBoxContainer _shapeFields;

	private bool _updatingControls;

	public override void _ExitTree()
	{
		DisposeCollisionBinding();
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeCollisionBinding();
		if (IsCollisionResource(CurrentResource) && CanvasGrid != null)
		{
			_editingCollision = CurrentResource;
			CanvasGrid.Columns = 1;
			Control node = BuildWorkbench();
			CanvasGrid.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			CreateBinding();
			PopulateControls();
			UpdateCollisionPreview();
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!IsCollisionResource(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool IsCollisionResource(Resource resource)
	{
		if (resource is CharacterHitBoxDefinition || resource is AabbShape2DResource || resource is AabbRay2DResource)
		{
			return true;
		}
		return false;
	}

	private Control BuildWorkbench()
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Name = "CollisionGeometryWorkbench",
			CustomMinimumSize = new Vector2(0f, 560f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 10);
		PanelContainer panelContainer = CreatePanel(new Color(0.055f, 0.075f, 0.052f), new Color(0.48f, 0.64f, 0.26f), 10);
		HFlowContainer hFlowContainer = new HFlowContainer();
		hFlowContainer.AddThemeConstantOverride("separation", 12);
		panelContainer.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
		hFlowContainer.AddChild(new Label
		{
			Text = "战场碰撞几何编辑器",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		_typeBadge = new Label
		{
			Text = "COLLISION"
		};
		_typeBadge.AddThemeColorOverride("font_color", new Color(0.85f, 0.96f, 0.55f));
		hFlowContainer.AddChild(_typeBadge, forceReadableName: false, InternalMode.Disabled);
		Button button = new Button
		{
			Name = "SaveCollisionGeometryButton",
			Text = "保存碰撞资源",
			CustomMinimumSize = new Vector2(132f, 34f)
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
		PanelContainer panelContainer2 = CreatePanel(new Color(0.025f, 0.035f, 0.028f), new Color(0.3f, 0.42f, 0.23f), 8);
		panelContainer2.CustomMinimumSize = new Vector2(420f, 460f);
		panelContainer2.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		panelContainer2.SizeFlagsVertical = SizeFlags.ExpandFill;
		VBoxContainer vBoxContainer2 = new VBoxContainer();
		vBoxContainer2.AddThemeConstantOverride("separation", 8);
		panelContainer2.AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		_geometrySummary = new Label
		{
			Text = "拖动橙色原点移动，拖动蓝色手柄调整范围",
			AutowrapMode = TextServer.AutowrapMode.WordSmart
		};
		_geometrySummary.AddThemeColorOverride("font_color", new Color(0.8f, 0.88f, 0.68f));
		vBoxContainer2.AddChild(_geometrySummary, forceReadableName: false, InternalMode.Disabled);
		_canvas = new XWCollisionGeometryCanvas
		{
			Name = "CollisionGeometryCanvas",
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		_canvas.OriginCommitted = CommitCanvasOrigin;
		_canvas.GeometryCommitted = CommitCanvasGeometry;
		vBoxContainer2.AddChild(_canvas, forceReadableName: false, InternalMode.Disabled);
		hSplitContainer2.AddChild(panelContainer2, forceReadableName: false, InternalMode.Disabled);
		ScrollContainer scrollContainer = new ScrollContainer
		{
			CustomMinimumSize = new Vector2(300f, 460f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		VBoxContainer vBoxContainer3 = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer3.AddThemeConstantOverride("separation", 8);
		scrollContainer.AddChild(vBoxContainer3, forceReadableName: false, InternalMode.Disabled);
		hSplitContainer2.AddChild(scrollContainer, forceReadableName: false, InternalMode.Disabled);
		BuildIdentityFields(vBoxContainer3);
		BuildTransformFields(vBoxContainer3);
		BuildGeometryFields(vBoxContainer3);
		BuildDebugFields(vBoxContainer3);
		return vBoxContainer;
	}

	private void BuildIdentityFields(VBoxContainer fields)
	{
		fields.AddChild(CreateSectionTitle("资源与运行状态"), forceReadableName: false, InternalMode.Disabled);
		_resourceName = new LineEdit
		{
			PlaceholderText = "用于资源库识别的名称"
		};
		fields.AddChild(CreateLabeledRow("资源名称", _resourceName), forceReadableName: false, InternalMode.Disabled);
		_localToScene = new CheckButton
		{
			Text = "仅属于当前场景"
		};
		fields.AddChild(_localToScene, forceReadableName: false, InternalMode.Disabled);
		_enabled = new CheckButton
		{
			Text = "运行时启用碰撞"
		};
		fields.AddChild(_enabled, forceReadableName: false, InternalMode.Disabled);
		_monitorable = new CheckButton
		{
			Text = "允许其他区域检测"
		};
		fields.AddChild(_monitorable, forceReadableName: false, InternalMode.Disabled);
	}

	private void BuildTransformFields(VBoxContainer fields)
	{
		fields.AddChild(CreateSectionTitle("画面位置与旋转"), forceReadableName: false, InternalMode.Disabled);
		_originX = CreateSpin(-10000.0, 10000.0, 0.5);
		_originY = CreateSpin(-10000.0, 10000.0, 0.5);
		_originX.Name = "CollisionOriginX";
		_originY.Name = "CollisionOriginY";
		fields.AddChild(CreateVectorRow("局部位置", _originX, _originY), forceReadableName: false, InternalMode.Disabled);
		_rotation = CreateSpin(-3600.0, 3600.0, 0.5);
		fields.AddChild(CreateLabeledRow("旋转角度", _rotation), forceReadableName: false, InternalMode.Disabled);
		_scaleX = CreateSpin(-100.0, 100.0, 0.05, 1.0);
		_scaleY = CreateSpin(-100.0, 100.0, 0.05, 1.0);
		fields.AddChild(CreateVectorRow("缩放", _scaleX, _scaleY), forceReadableName: false, InternalMode.Disabled);
	}

	private void BuildGeometryFields(VBoxContainer fields)
	{
		fields.AddChild(CreateSectionTitle("碰撞形状"), forceReadableName: false, InternalMode.Disabled);
		_shapeType = new OptionButton
		{
			Visible = false
		};
		_shapeType.AddItem("矩形", 0);
		_shapeType.AddItem("圆形", 1);
		_shapeType.AddItem("胶囊", 2);
		_shapeType.AddItem("线段", 3);
		_shapeType.AddItem("其他 Shape2D", 4);
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddChild(_shapeType, forceReadableName: false, InternalMode.Disabled);
		_shapeTypeSegments = new HFlowContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_shapeTypeSegments.AddThemeConstantOverride("h_separation", 6);
		_shapeTypeSegments.AddThemeConstantOverride("v_separation", 6);
		vBoxContainer.AddChild(_shapeTypeSegments, forceReadableName: false, InternalMode.Disabled);
		fields.AddChild(CreateLabeledRow("形状类型", vBoxContainer), forceReadableName: false, InternalMode.Disabled);
		_shapeTypeVisualChoice = new XWVisualSegmentedOption(_shapeType, _shapeTypeSegments, LoadCollisionShapeIcon);
		_shapeTypeVisualChoice.Rebuild();
		_shapePicker = XWResourcePicker.Create();
		_shapePicker.Setup("Shape2D");
		_shapePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		fields.AddChild(CreateLabeledRow("形状资源", _shapePicker), forceReadableName: false, InternalMode.Disabled);
		_shapeFields = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_shapeFields.AddThemeConstantOverride("separation", 6);
		fields.AddChild(_shapeFields, forceReadableName: false, InternalMode.Disabled);
	}

	private void BuildDebugFields(VBoxContainer fields)
	{
		fields.AddChild(CreateSectionTitle("调试画面"), forceReadableName: false, InternalMode.Disabled);
		_debugDraw = new CheckButton
		{
			Text = "在运行预览中显示碰撞范围"
		};
		fields.AddChild(_debugDraw, forceReadableName: false, InternalMode.Disabled);
		_fillColor = new ColorPickerButton
		{
			CustomMinimumSize = new Vector2(90f, 30f)
		};
		fields.AddChild(CreateLabeledRow("填充颜色", _fillColor), forceReadableName: false, InternalMode.Disabled);
		_outlineColor = new ColorPickerButton
		{
			CustomMinimumSize = new Vector2(90f, 30f)
		};
		fields.AddChild(CreateLabeledRow("轮廓颜色", _outlineColor), forceReadableName: false, InternalMode.Disabled);
		_lineWidth = CreateSpin(0.5, 20.0, 0.5, 2.0);
		fields.AddChild(CreateLabeledRow("线宽", _lineWidth), forceReadableName: false, InternalMode.Disabled);
	}

	private void CreateBinding()
	{
		_binding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnCollisionPropertyEdited);
		_binding.BindText(_resourceName, _editingCollision, "resource_name", UpdateCollisionPreview, this, "RefreshCollisionEditorFromHistory");
		_binding.BindToggle(_localToScene, _editingCollision, "resource_local_to_scene", UpdateCollisionPreview, this, "RefreshCollisionEditorFromHistory");
		if (_editingCollision is CharacterHitBoxDefinition resource)
		{
			_binding.BindToggle(_enabled, resource, "DefaultEnabled", UpdateCollisionPreview, this, "RefreshCollisionEditorFromHistory");
			_binding.BindToggle(_monitorable, resource, "DefaultMonitorable", UpdateCollisionPreview, this, "RefreshCollisionEditorFromHistory");
		}
		else
		{
			_binding.BindToggle(_enabled, _editingCollision, "Enabled", UpdateCollisionPreview, this, "RefreshCollisionEditorFromHistory");
			_monitorable.Visible = false;
		}
		_binding.BindToggle(_debugDraw, _editingCollision, "DebugDraw", UpdateCollisionPreview, this, "RefreshCollisionEditorFromHistory");
		_binding.BindNumber(_lineWidth, _editingCollision, "DebugLineWidth", UpdateCollisionPreview, this, "RefreshCollisionEditorFromHistory");
		BindTransformControl(_originX, "调整碰撞位置");
		BindTransformControl(_originY, "调整碰撞位置");
		BindTransformControl(_rotation, "旋转碰撞形状");
		BindTransformControl(_scaleX, "缩放碰撞形状");
		BindTransformControl(_scaleY, "缩放碰撞形状");
		BindColorControl(_fillColor, "DebugFillColor", "DebugColor", "修改碰撞填充颜色");
		BindColorControl(_outlineColor, "DebugOutlineColor", "DebugColor", "修改碰撞轮廓颜色");
		_shapeType.ItemSelected += ChangeShapeType;
		_shapePicker.ResourceChanged += ChangeShapeResource;
	}

	private void PopulateControls()
	{
		if (!GodotObject.IsInstanceValid(_editingCollision))
		{
			return;
		}
		_updatingControls = true;
		try
		{
			Transform2D localTransform = GetLocalTransform();
			_originX.Value = localTransform.Origin.X;
			_originY.Value = localTransform.Origin.Y;
			_rotation.Value = Mathf.RadToDeg(localTransform.Rotation);
			Vector2 scale = localTransform.Scale;
			_scaleX.Value = scale.X;
			_scaleY.Value = scale.Y;
			bool flag = _editingCollision is AabbRay2DResource;
			bool flag2 = _editingCollision is AabbShape2DResource;
			_shapeType.Visible = false;
			_shapeTypeSegments.Visible = flag2;
			_shapePicker.Visible = flag2;
			if (flag2)
			{
				AabbShape2DResource aabbShape2DResource = (AabbShape2DResource)_editingCollision;
				_shapePicker.SetEditedResource(aabbShape2DResource.Geometry);
				SelectShapeType(aabbShape2DResource.Geometry);
			}
			else
			{
				_shapePicker.SetEditedResource(null);
			}
			_fillColor.Color = GetColor("DebugFillColor", "DebugColor", new Color(0.1f, 0.75f, 1f, 0.2f));
			_outlineColor.Color = GetColor("DebugOutlineColor", "DebugColor", new Color(0.2f, 0.9f, 1f, 0.95f));
			_fillColor.Visible = !flag;
			RebuildGeometryFields();
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void RebuildGeometryFields()
	{
		foreach (Node child in _shapeFields.GetChildren())
		{
			_shapeFields.RemoveChild(child);
			child.QueueFree();
		}
		Resource editingCollision = _editingCollision;
		if (!(editingCollision is CharacterHitBoxDefinition characterHitBoxDefinition))
		{
			if (!(editingCollision is AabbRay2DResource aabbRay2DResource))
			{
				if (editingCollision is AabbShape2DResource aabbShape2DResource)
				{
					AddShapeGeometryFields(aabbShape2DResource.Geometry);
				}
			}
			else
			{
				AddVectorPropertyFields(_shapeFields, "射线终点", aabbRay2DResource, "TargetPosition", aabbRay2DResource.TargetPosition, -10000.0, 10000.0);
				AddNumberPropertyField(_shapeFields, "终点半径", aabbRay2DResource, "DebugEndpointRadius", aabbRay2DResource.DebugEndpointRadius, 1.0, 128.0);
				AddNumberPropertyField(_shapeFields, "箭头大小", aabbRay2DResource, "DebugArrowSize", aabbRay2DResource.DebugArrowSize, 1.0, 256.0);
			}
		}
		else
		{
			AddVectorPropertyFields(_shapeFields, "碰撞尺寸", characterHitBoxDefinition, "Size", characterHitBoxDefinition.Size, 1.0, 10000.0);
		}
	}

	private void AddShapeGeometryFields(Shape2D shape)
	{
		if (!(shape is RectangleShape2D rectangleShape2D))
		{
			if (!(shape is CircleShape2D circleShape2D))
			{
				if (!(shape is CapsuleShape2D capsuleShape2D))
				{
					if (shape is SegmentShape2D segmentShape2D)
					{
						AddVectorPropertyFields(_shapeFields, "线段起点", segmentShape2D, "a", segmentShape2D.A, -10000.0, 10000.0);
						AddVectorPropertyFields(_shapeFields, "线段终点", segmentShape2D, "b", segmentShape2D.B, -10000.0, 10000.0);
					}
					else
					{
						_shapeFields.AddChild(new Label
						{
							Text = ((shape == null) ? "请选择或创建一个 Shape2D" : "此形状使用画布预览和资源替换；可切换为上方四种可视形状。"),
							AutowrapMode = TextServer.AutowrapMode.WordSmart
						}, forceReadableName: false, InternalMode.Disabled);
					}
				}
				else
				{
					AddNumberPropertyField(_shapeFields, "胶囊半径", capsuleShape2D, "radius", capsuleShape2D.Radius, 0.5, 5000.0);
					AddNumberPropertyField(_shapeFields, "胶囊高度", capsuleShape2D, "height", capsuleShape2D.Height, 1.0, 10000.0);
				}
			}
			else
			{
				AddNumberPropertyField(_shapeFields, "圆形半径", circleShape2D, "radius", circleShape2D.Radius, 0.5, 5000.0);
			}
		}
		else
		{
			AddVectorPropertyFields(_shapeFields, "矩形尺寸", rectangleShape2D, "size", rectangleShape2D.Size, 1.0, 10000.0);
		}
	}

	private void AddVectorPropertyFields(VBoxContainer host, string label, Resource target, StringName property, Vector2 value, double minimum, double maximum)
	{
		SpinBox x = CreateSpin(minimum, maximum, 0.5, value.X);
		SpinBox y = CreateSpin(minimum, maximum, 0.5, value.Y);
		host.AddChild(CreateVectorRow(label, x, y), forceReadableName: false, InternalMode.Disabled);
		BindVectorComponent(target, property, x, y, "修改" + label);
	}

	private void AddNumberPropertyField(VBoxContainer host, string label, Resource target, StringName property, double value, double minimum, double maximum)
	{
		SpinBox spinBox = CreateSpin(minimum, maximum, 0.5, value);
		host.AddChild(CreateLabeledRow(label, spinBox), forceReadableName: false, InternalMode.Disabled);
		_binding.BindNumber(spinBox, target, property, UpdateCollisionPreview, this, "RefreshCollisionEditorFromHistory");
	}

	private void BindVectorComponent(Resource target, StringName property, SpinBox x, SpinBox y, string actionName)
	{
		Action value = () =>
		{
			_binding?.BeginEdit(target, property);
		};
		Godot.Range.ValueChangedEventHandler value2 = (double _) =>
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(target))
			{
				_binding.PreviewValue(target, property, new Vector2((float)x.Value, (float)y.Value));
				UpdateCollisionPreview();
			}
		};
		Action value3 = () =>
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(target))
			{
				_binding.CommitEdit(target, property, target.Get(property), actionName, this, "RefreshCollisionEditorFromHistory");
				UpdateCollisionPreview();
			}
		};
		x.FocusEntered += value;
		y.FocusEntered += value;
		x.ValueChanged += value2;
		y.ValueChanged += value2;
		x.FocusExited += value3;
		y.FocusExited += value3;
	}

	private void BindTransformControl(SpinBox control, string actionName)
	{
		control.FocusEntered += () =>
		{
			_binding?.BeginEdit(_editingCollision, "LocalTransform");
		};
		control.ValueChanged += (double _) =>
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_editingCollision))
			{
				_binding.PreviewValue(_editingCollision, "LocalTransform", BuildTransformFromControls());
				UpdateCollisionPreview();
			}
		};
		control.FocusExited += () =>
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_editingCollision))
			{
				_binding.CommitEdit(_editingCollision, "LocalTransform", _editingCollision.Get("LocalTransform"), actionName, this, "RefreshCollisionEditorFromHistory");
				UpdateCollisionPreview();
			}
		};
	}

	private Transform2D BuildTransformFromControls()
	{
		Transform2D localTransform = GetLocalTransform();
		return new Transform2D(Mathf.DegToRad((float)_rotation.Value), new Vector2((float)_scaleX.Value, (float)_scaleY.Value), localTransform.Skew, new Vector2((float)_originX.Value, (float)_originY.Value));
	}

	private void CommitCanvasOrigin(Vector2 origin)
	{
		if (_binding != null && GodotObject.IsInstanceValid(_editingCollision))
		{
			Transform2D localTransform = GetLocalTransform();
			localTransform.Origin = origin;
			_binding.SetValue(_editingCollision, "LocalTransform", localTransform, "拖动碰撞原点", this, "RefreshCollisionEditorFromHistory");
			PopulateControls();
			UpdateCollisionPreview();
		}
	}

	private void CommitCanvasGeometry(Vector2 value)
	{
		if (_binding == null || !GodotObject.IsInstanceValid(_editingCollision))
		{
			return;
		}
		Resource editingCollision = _editingCollision;
		if (!(editingCollision is CharacterHitBoxDefinition resource))
		{
			if (!(editingCollision is AabbRay2DResource resource2))
			{
				if (editingCollision is AabbShape2DResource { Geometry: var geometry })
				{
					if (!(geometry is RectangleShape2D resource3))
					{
						if (!(geometry is CircleShape2D resource4))
						{
							if (!(geometry is CapsuleShape2D capsule))
							{
								if (geometry is SegmentShape2D resource5)
								{
									_binding.SetValue(resource5, "b", value, "拖动调整线段终点", this, "RefreshCollisionEditorFromHistory");
								}
							}
							else
							{
								CommitCapsuleGeometry(capsule, value);
							}
						}
						else
						{
							_binding.SetValue(resource4, "radius", Mathf.Max(0.5f, value.X * 0.5f), "拖动调整圆形半径", this, "RefreshCollisionEditorFromHistory");
						}
					}
					else
					{
						_binding.SetValue(resource3, "size", value, "拖动调整矩形尺寸", this, "RefreshCollisionEditorFromHistory");
					}
				}
			}
			else
			{
				_binding.SetValue(resource2, "TargetPosition", value, "拖动调整射线终点", this, "RefreshCollisionEditorFromHistory");
			}
		}
		else
		{
			_binding.SetValue(resource, "Size", value, "拖动调整碰撞尺寸", this, "RefreshCollisionEditorFromHistory");
		}
		PopulateControls();
		UpdateCollisionPreview();
	}

	private void CommitCapsuleGeometry(CapsuleShape2D capsule, Vector2 value)
	{
		float num = Mathf.Max(0.5f, value.X * 0.5f);
		float num2 = Mathf.Max(num * 2f, value.Y);
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null)
		{
			capsule.Radius = num;
			capsule.Height = num2;
			OnCollisionPropertyEdited(committed: true);
			return;
		}
		xWUndoRedoManager.CreateAction("拖动调整胶囊尺寸");
		xWUndoRedoManager.AddDoProperty(capsule, "radius", num);
		xWUndoRedoManager.AddDoProperty(capsule, "height", num2);
		xWUndoRedoManager.AddUndoProperty(capsule, "radius", capsule.Radius);
		xWUndoRedoManager.AddUndoProperty(capsule, "height", capsule.Height);
		xWUndoRedoManager.AddDoMethod(this, "RefreshCollisionEditorFromHistory");
		xWUndoRedoManager.AddUndoMethod(this, "RefreshCollisionEditorFromHistory");
		xWUndoRedoManager.CommitAction();
		OnCollisionPropertyEdited(committed: true);
	}

	private void ChangeShapeType(long index)
	{
		if (!_updatingControls && _editingCollision is AabbShape2DResource aabbShape2DResource && _binding != null)
		{
			Shape2D shape2D = _shapeType.GetItemId((int)index) switch
			{
				0 => new RectangleShape2D
				{
					Size = GetCurrentShapeSize()
				}, 
				1 => new CircleShape2D
				{
					Radius = Mathf.Max(0.5f, GetCurrentShapeSize().X * 0.5f)
				}, 
				2 => new CapsuleShape2D
				{
					Radius = Mathf.Max(0.5f, GetCurrentShapeSize().X * 0.5f),
					Height = Mathf.Max(1f, GetCurrentShapeSize().Y)
				}, 
				3 => new SegmentShape2D
				{
					A = -GetCurrentShapeSize() * 0.5f,
					B = GetCurrentShapeSize() * 0.5f
				}, 
				_ => aabbShape2DResource.Geometry, 
			};
			if (shape2D != aabbShape2DResource.Geometry)
			{
				_binding.SetValue(aabbShape2DResource, "Geometry", shape2D, "切换碰撞形状", this, "RefreshCollisionEditorFromHistory");
				PopulateControls();
				UpdateCollisionPreview();
			}
		}
	}

	private void ChangeShapeResource(Resource resource)
	{
		if (!_updatingControls && _editingCollision is AabbShape2DResource resource2 && _binding != null && (resource == null || resource is Shape2D))
		{
			_binding.SetValue(resource2, "Geometry", resource, "替换碰撞形状资源", this, "RefreshCollisionEditorFromHistory");
			PopulateControls();
			UpdateCollisionPreview();
		}
	}

	private void BindColorControl(ColorPickerButton control, StringName primaryProperty, StringName rayProperty, string actionName)
	{
		control.Pressed += () =>
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_editingCollision))
			{
				_binding.BeginEdit(_editingCollision, ResolveProperty());
			}
		};
		control.ColorChanged += (Color color) =>
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_editingCollision))
			{
				_binding.PreviewValue(_editingCollision, ResolveProperty(), color);
				UpdateCollisionPreview();
			}
		};
		control.PopupClosed += () =>
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_editingCollision))
			{
				StringName property = ResolveProperty();
				_binding.CommitEdit(_editingCollision, property, _editingCollision.Get(property), actionName, this, "RefreshCollisionEditorFromHistory");
				UpdateCollisionPreview();
			}
		};
		StringName ResolveProperty()
		{
			if (!(_editingCollision is AabbRay2DResource))
			{
				return primaryProperty;
			}
			return rayProperty;
		}
	}

	private void OnCollisionPropertyEdited(bool committed)
	{
		if (IsCollisionResource(CurrentResource) && CurrentResource == _editingCollision)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
				return;
			}
			MarkCurrentResourceDirty();
			CurrentResource.EmitChanged();
		}
	}

	public void RefreshCollisionEditorFromHistory()
	{
		if (IsCollisionResource(CurrentResource) && CurrentResource == _editingCollision)
		{
			PopulateControls();
			UpdateCollisionPreview();
		}
	}

	private void UpdateCollisionPreview()
	{
		if (!GodotObject.IsInstanceValid(_canvas) || !GodotObject.IsInstanceValid(_editingCollision))
		{
			return;
		}
		Resource editingCollision = _editingCollision;
		if (!(editingCollision is CharacterHitBoxDefinition characterHitBoxDefinition))
		{
			if (!(editingCollision is AabbShape2DResource aabbShape2DResource))
			{
				if (editingCollision is AabbRay2DResource aabbRay2DResource)
				{
					_typeBadge.Text = "AABB CHECK RAY";
					_geometrySummary.Text = $"射线检测  {aabbRay2DResource.TargetPosition.X:0.#}, {aabbRay2DResource.TargetPosition.Y:0.#}  ·  拖动蓝色端点改变射线";
					_canvas.DisplayRay(aabbRay2DResource);
				}
			}
			else
			{
				_typeBadge.Text = "AABB AREA SHAPE";
				_geometrySummary.Text = "区域碰撞  " + (aabbShape2DResource.Geometry?.GetType().Name ?? "未选择形状") + "  ·  画布手柄可直接编辑";
				_canvas.DisplayShape(aabbShape2DResource);
			}
		}
		else
		{
			_typeBadge.Text = "CHARACTER HIT BOX";
			_geometrySummary.Text = $"角色主碰撞框  {characterHitBoxDefinition.Size.X:0.#} × {characterHitBoxDefinition.Size.Y:0.#}  ·  拖动橙色原点移动，拖动蓝色手柄改尺寸";
			_canvas.DisplayHitBox(characterHitBoxDefinition);
		}
	}

	private Transform2D GetLocalTransform()
	{
		Resource editingCollision = _editingCollision;
		if (!(editingCollision is CharacterHitBoxDefinition { LocalTransform: var localTransform }))
		{
			if (!(editingCollision is AabbShape2DResource { LocalTransform: var localTransform2 }))
			{
				if (!(editingCollision is AabbRay2DResource { LocalTransform: var localTransform3 }))
				{
					return Transform2D.Identity;
				}
				return localTransform3;
			}
			return localTransform2;
		}
		return localTransform;
	}

	private Vector2 GetCurrentShapeSize()
	{
		if (!(_editingCollision is AabbShape2DResource { Geometry: var geometry }))
		{
			return new Vector2(80f, 80f);
		}
		if (!(geometry is RectangleShape2D { Size: var size }))
		{
			if (!(geometry is CircleShape2D circleShape2D))
			{
				if (!(geometry is CapsuleShape2D capsuleShape2D))
				{
					if (geometry is SegmentShape2D segmentShape2D)
					{
						return (segmentShape2D.B - segmentShape2D.A).Abs();
					}
					return new Vector2(80f, 80f);
				}
				return new Vector2(capsuleShape2D.Radius * 2f, capsuleShape2D.Height);
			}
			return Vector2.One * circleShape2D.Radius * 2f;
		}
		return size;
	}

	private Color GetColor(StringName primaryProperty, StringName rayProperty, Color fallback)
	{
		if (!GodotObject.IsInstanceValid(_editingCollision))
		{
			return fallback;
		}
		StringName property = ((_editingCollision is AabbRay2DResource) ? rayProperty : primaryProperty);
		Variant variant = _editingCollision.Get(property);
		if (variant.VariantType != Variant.Type.Color)
		{
			return fallback;
		}
		return variant.AsColor();
	}

	private void SelectShapeType(Shape2D shape)
	{
		OptionButton shapeType = _shapeType;
		int idx = ((!(shape is RectangleShape2D)) ? ((shape is CircleShape2D) ? 1 : ((shape is CapsuleShape2D) ? 2 : ((!(shape is SegmentShape2D)) ? 4 : 3))) : 0);
		shapeType.Select(idx);
		_shapeTypeVisualChoice?.RefreshSelection();
	}

	private static Texture2D LoadCollisionShapeIcon(int index)
	{
		return ResourceLoader.Load<Texture2D>(index switch
		{
			0 => "res://addons/ModEditor/Icons/ClassIcon/RectangleShape2D.svg", 
			1 => "res://addons/ModEditor/Icons/ClassIcon/CircleShape2D.svg", 
			2 => "res://addons/ModEditor/Icons/ClassIcon/CapsuleShape2D.svg", 
			3 => "res://addons/ModEditor/Icons/ClassIcon/SegmentShape2D.svg", 
			_ => "res://addons/ModEditor/Icons/ClassIcon/CollisionShape2D.svg", 
		}, "", ResourceLoader.CacheMode.Reuse);
	}

	private void DisposeCollisionBinding()
	{
		_shapeTypeVisualChoice?.Dispose();
		_shapeTypeVisualChoice = null;
		_shapeTypeSegments = null;
		_shapeType = null;
		_binding?.Dispose();
		_binding = null;
		_editingCollision = null;
	}

	private static Label CreateSectionTitle(string text)
	{
		Label label = new Label();
		label.Text = text;
		label.AddThemeFontSizeOverride("font_size", 17);
		label.AddThemeColorOverride("font_color", new Color(0.86f, 0.94f, 0.67f));
		return label;
	}

	private static HBoxContainer CreateLabeledRow(string text, Control field)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
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

	private static HBoxContainer CreateVectorRow(string text, SpinBox x, SpinBox y)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddThemeConstantOverride("separation", 6);
		hBoxContainer.AddChild(new Label
		{
			Text = text,
			CustomMinimumSize = new Vector2(108f, 0f)
		}, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(new Label
		{
			Text = "X"
		}, forceReadableName: false, InternalMode.Disabled);
		x.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddChild(x, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(new Label
		{
			Text = "Y"
		}, forceReadableName: false, InternalMode.Disabled);
		y.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddChild(y, forceReadableName: false, InternalMode.Disabled);
		return hBoxContainer;
	}

	private static SpinBox CreateSpin(double minimum, double maximum, double step, double value = 0.0)
	{
		return new SpinBox
		{
			MinValue = minimum,
			MaxValue = maximum,
			Step = step,
			Value = value,
			AllowGreater = true,
			AllowLesser = true
		};
	}

	private static PanelContainer CreatePanel(Color background, Color border, int radius)
	{
		StyleBoxFlat stylebox = new StyleBoxFlat
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
			ContentMarginLeft = 12f,
			ContentMarginTop = 10f,
			ContentMarginRight = 12f,
			ContentMarginBottom = 10f
		};
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.AddThemeStyleboxOverride("panel", stylebox);
		return panelContainer;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(36)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCollisionResource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildWorkbench, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildIdentityFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fields", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildTransformFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fields", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildGeometryFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fields", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildDebugFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fields", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildGeometryFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddShapeGeometryFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shape", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Shape2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddVectorPropertyFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddNumberPropertyField, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindVectorComponent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "x", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "y", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindTransformControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildTransformFromControls, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitCanvasOrigin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "origin", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitCanvasGeometry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitCapsuleGeometry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "capsule", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CapsuleShape2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangeShapeType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangeShapeResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindColorControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ColorPickerButton"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "primaryProperty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "rayProperty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCollisionPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCollisionEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCollisionPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLocalTransform, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentShapeSize, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "primaryProperty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "rayProperty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectShapeType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shape", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Shape2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadCollisionShapeIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeCollisionBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSectionTitle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateLabeledRow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "field", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateVectorRow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "x", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "y", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSpin, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "background", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "border", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "radius", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IsCollisionResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCollisionResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildWorkbench && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(BuildWorkbench());
			return true;
		}
		if (method == MethodName.BuildIdentityFields && args.Count == 1)
		{
			BuildIdentityFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTransformFields && args.Count == 1)
		{
			BuildTransformFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildGeometryFields && args.Count == 1)
		{
			BuildGeometryFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildDebugFields && args.Count == 1)
		{
			BuildDebugFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateBinding && args.Count == 0)
		{
			CreateBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateControls && args.Count == 0)
		{
			PopulateControls();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildGeometryFields && args.Count == 0)
		{
			RebuildGeometryFields();
			ret = default;
			return true;
		}
		if (method == MethodName.AddShapeGeometryFields && args.Count == 1)
		{
			AddShapeGeometryFields(VariantUtils.ConvertTo<Shape2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddVectorPropertyFields && args.Count == 7)
		{
			AddVectorPropertyFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Resource>(in args[2]), VariantUtils.ConvertTo<StringName>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddNumberPropertyField && args.Count == 7)
		{
			AddNumberPropertyField(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Resource>(in args[2]), VariantUtils.ConvertTo<StringName>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindVectorComponent && args.Count == 5)
		{
			BindVectorComponent(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2]), VariantUtils.ConvertTo<SpinBox>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindTransformControl && args.Count == 2)
		{
			BindTransformControl(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTransformFromControls && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(BuildTransformFromControls());
			return true;
		}
		if (method == MethodName.CommitCanvasOrigin && args.Count == 1)
		{
			CommitCanvasOrigin(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitCanvasGeometry && args.Count == 1)
		{
			CommitCanvasGeometry(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitCapsuleGeometry && args.Count == 2)
		{
			CommitCapsuleGeometry(VariantUtils.ConvertTo<CapsuleShape2D>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeShapeType && args.Count == 1)
		{
			ChangeShapeType(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeShapeResource && args.Count == 1)
		{
			ChangeShapeResource(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindColorControl && args.Count == 4)
		{
			BindColorControl(VariantUtils.ConvertTo<ColorPickerButton>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCollisionPropertyEdited && args.Count == 1)
		{
			OnCollisionPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCollisionEditorFromHistory && args.Count == 0)
		{
			RefreshCollisionEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCollisionPreview && args.Count == 0)
		{
			UpdateCollisionPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.GetLocalTransform && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(GetLocalTransform());
			return true;
		}
		if (method == MethodName.GetCurrentShapeSize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCurrentShapeSize());
			return true;
		}
		if (method == MethodName.GetColor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Color>(GetColor(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.SelectShapeType && args.Count == 1)
		{
			SelectShapeType(VariantUtils.ConvertTo<Shape2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadCollisionShapeIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadCollisionShapeIcon(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.DisposeCollisionBinding && args.Count == 0)
		{
			DisposeCollisionBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSectionTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateSectionTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateLabeledRow && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(CreateLabeledRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateVectorRow && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(CreateVectorRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateSpin && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateSpin(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
			return true;
		}
		if (method == MethodName.CreatePanel && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(CreatePanel(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsCollisionResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCollisionResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadCollisionShapeIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadCollisionShapeIcon(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateSectionTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateSectionTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateLabeledRow && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(CreateLabeledRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateVectorRow && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(CreateVectorRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateSpin && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateSpin(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
			return true;
		}
		if (method == MethodName.CreatePanel && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(CreatePanel(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
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
		if (method == MethodName.IsCollisionResource)
		{
			return true;
		}
		if (method == MethodName.BuildWorkbench)
		{
			return true;
		}
		if (method == MethodName.BuildIdentityFields)
		{
			return true;
		}
		if (method == MethodName.BuildTransformFields)
		{
			return true;
		}
		if (method == MethodName.BuildGeometryFields)
		{
			return true;
		}
		if (method == MethodName.BuildDebugFields)
		{
			return true;
		}
		if (method == MethodName.CreateBinding)
		{
			return true;
		}
		if (method == MethodName.PopulateControls)
		{
			return true;
		}
		if (method == MethodName.RebuildGeometryFields)
		{
			return true;
		}
		if (method == MethodName.AddShapeGeometryFields)
		{
			return true;
		}
		if (method == MethodName.AddVectorPropertyFields)
		{
			return true;
		}
		if (method == MethodName.AddNumberPropertyField)
		{
			return true;
		}
		if (method == MethodName.BindVectorComponent)
		{
			return true;
		}
		if (method == MethodName.BindTransformControl)
		{
			return true;
		}
		if (method == MethodName.BuildTransformFromControls)
		{
			return true;
		}
		if (method == MethodName.CommitCanvasOrigin)
		{
			return true;
		}
		if (method == MethodName.CommitCanvasGeometry)
		{
			return true;
		}
		if (method == MethodName.CommitCapsuleGeometry)
		{
			return true;
		}
		if (method == MethodName.ChangeShapeType)
		{
			return true;
		}
		if (method == MethodName.ChangeShapeResource)
		{
			return true;
		}
		if (method == MethodName.BindColorControl)
		{
			return true;
		}
		if (method == MethodName.OnCollisionPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshCollisionEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.UpdateCollisionPreview)
		{
			return true;
		}
		if (method == MethodName.GetLocalTransform)
		{
			return true;
		}
		if (method == MethodName.GetCurrentShapeSize)
		{
			return true;
		}
		if (method == MethodName.GetColor)
		{
			return true;
		}
		if (method == MethodName.SelectShapeType)
		{
			return true;
		}
		if (method == MethodName.LoadCollisionShapeIcon)
		{
			return true;
		}
		if (method == MethodName.DisposeCollisionBinding)
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
		if (method == MethodName.CreateSpin)
		{
			return true;
		}
		if (method == MethodName.CreatePanel)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editingCollision)
		{
			_editingCollision = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._canvas)
		{
			_canvas = VariantUtils.ConvertTo<XWCollisionGeometryCanvas>(in value);
			return true;
		}
		if (name == PropertyName._typeBadge)
		{
			_typeBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._geometrySummary)
		{
			_geometrySummary = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._resourceName)
		{
			_resourceName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._localToScene)
		{
			_localToScene = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._enabled)
		{
			_enabled = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._monitorable)
		{
			_monitorable = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._debugDraw)
		{
			_debugDraw = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._fillColor)
		{
			_fillColor = VariantUtils.ConvertTo<ColorPickerButton>(in value);
			return true;
		}
		if (name == PropertyName._outlineColor)
		{
			_outlineColor = VariantUtils.ConvertTo<ColorPickerButton>(in value);
			return true;
		}
		if (name == PropertyName._lineWidth)
		{
			_lineWidth = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._originX)
		{
			_originX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._originY)
		{
			_originY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._rotation)
		{
			_rotation = VariantUtils.ConvertTo<SpinBox>(in value);
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
		if (name == PropertyName._shapeType)
		{
			_shapeType = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._shapeTypeSegments)
		{
			_shapeTypeSegments = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._shapePicker)
		{
			_shapePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._shapeFields)
		{
			_shapeFields = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editingCollision)
		{
			value = VariantUtils.CreateFrom(in _editingCollision);
			return true;
		}
		if (name == PropertyName._canvas)
		{
			value = VariantUtils.CreateFrom(in _canvas);
			return true;
		}
		if (name == PropertyName._typeBadge)
		{
			value = VariantUtils.CreateFrom(in _typeBadge);
			return true;
		}
		if (name == PropertyName._geometrySummary)
		{
			value = VariantUtils.CreateFrom(in _geometrySummary);
			return true;
		}
		if (name == PropertyName._resourceName)
		{
			value = VariantUtils.CreateFrom(in _resourceName);
			return true;
		}
		if (name == PropertyName._localToScene)
		{
			value = VariantUtils.CreateFrom(in _localToScene);
			return true;
		}
		if (name == PropertyName._enabled)
		{
			value = VariantUtils.CreateFrom(in _enabled);
			return true;
		}
		if (name == PropertyName._monitorable)
		{
			value = VariantUtils.CreateFrom(in _monitorable);
			return true;
		}
		if (name == PropertyName._debugDraw)
		{
			value = VariantUtils.CreateFrom(in _debugDraw);
			return true;
		}
		if (name == PropertyName._fillColor)
		{
			value = VariantUtils.CreateFrom(in _fillColor);
			return true;
		}
		if (name == PropertyName._outlineColor)
		{
			value = VariantUtils.CreateFrom(in _outlineColor);
			return true;
		}
		if (name == PropertyName._lineWidth)
		{
			value = VariantUtils.CreateFrom(in _lineWidth);
			return true;
		}
		if (name == PropertyName._originX)
		{
			value = VariantUtils.CreateFrom(in _originX);
			return true;
		}
		if (name == PropertyName._originY)
		{
			value = VariantUtils.CreateFrom(in _originY);
			return true;
		}
		if (name == PropertyName._rotation)
		{
			value = VariantUtils.CreateFrom(in _rotation);
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
		if (name == PropertyName._shapeType)
		{
			value = VariantUtils.CreateFrom(in _shapeType);
			return true;
		}
		if (name == PropertyName._shapeTypeSegments)
		{
			value = VariantUtils.CreateFrom(in _shapeTypeSegments);
			return true;
		}
		if (name == PropertyName._shapePicker)
		{
			value = VariantUtils.CreateFrom(in _shapePicker);
			return true;
		}
		if (name == PropertyName._shapeFields)
		{
			value = VariantUtils.CreateFrom(in _shapeFields);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingCollision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._canvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._geometrySummary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._enabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._monitorable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._debugDraw, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fillColor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._outlineColor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lineWidth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._originX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._originY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rotation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scaleX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scaleY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shapeType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shapeTypeSegments, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shapePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shapeFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingCollision, Variant.From(in _editingCollision));
		info.AddProperty(PropertyName._canvas, Variant.From(in _canvas));
		info.AddProperty(PropertyName._typeBadge, Variant.From(in _typeBadge));
		info.AddProperty(PropertyName._geometrySummary, Variant.From(in _geometrySummary));
		info.AddProperty(PropertyName._resourceName, Variant.From(in _resourceName));
		info.AddProperty(PropertyName._localToScene, Variant.From(in _localToScene));
		info.AddProperty(PropertyName._enabled, Variant.From(in _enabled));
		info.AddProperty(PropertyName._monitorable, Variant.From(in _monitorable));
		info.AddProperty(PropertyName._debugDraw, Variant.From(in _debugDraw));
		info.AddProperty(PropertyName._fillColor, Variant.From(in _fillColor));
		info.AddProperty(PropertyName._outlineColor, Variant.From(in _outlineColor));
		info.AddProperty(PropertyName._lineWidth, Variant.From(in _lineWidth));
		info.AddProperty(PropertyName._originX, Variant.From(in _originX));
		info.AddProperty(PropertyName._originY, Variant.From(in _originY));
		info.AddProperty(PropertyName._rotation, Variant.From(in _rotation));
		info.AddProperty(PropertyName._scaleX, Variant.From(in _scaleX));
		info.AddProperty(PropertyName._scaleY, Variant.From(in _scaleY));
		info.AddProperty(PropertyName._shapeType, Variant.From(in _shapeType));
		info.AddProperty(PropertyName._shapeTypeSegments, Variant.From(in _shapeTypeSegments));
		info.AddProperty(PropertyName._shapePicker, Variant.From(in _shapePicker));
		info.AddProperty(PropertyName._shapeFields, Variant.From(in _shapeFields));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingCollision, out var value))
		{
			_editingCollision = value.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._canvas, out var value2))
		{
			_canvas = value2.As<XWCollisionGeometryCanvas>();
		}
		if (info.TryGetProperty(PropertyName._typeBadge, out var value3))
		{
			_typeBadge = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._geometrySummary, out var value4))
		{
			_geometrySummary = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resourceName, out var value5))
		{
			_resourceName = value5.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToScene, out var value6))
		{
			_localToScene = value6.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._enabled, out var value7))
		{
			_enabled = value7.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._monitorable, out var value8))
		{
			_monitorable = value8.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._debugDraw, out var value9))
		{
			_debugDraw = value9.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._fillColor, out var value10))
		{
			_fillColor = value10.As<ColorPickerButton>();
		}
		if (info.TryGetProperty(PropertyName._outlineColor, out var value11))
		{
			_outlineColor = value11.As<ColorPickerButton>();
		}
		if (info.TryGetProperty(PropertyName._lineWidth, out var value12))
		{
			_lineWidth = value12.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._originX, out var value13))
		{
			_originX = value13.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._originY, out var value14))
		{
			_originY = value14.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._rotation, out var value15))
		{
			_rotation = value15.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._scaleX, out var value16))
		{
			_scaleX = value16.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._scaleY, out var value17))
		{
			_scaleY = value17.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._shapeType, out var value18))
		{
			_shapeType = value18.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._shapeTypeSegments, out var value19))
		{
			_shapeTypeSegments = value19.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._shapePicker, out var value20))
		{
			_shapePicker = value20.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._shapeFields, out var value21))
		{
			_shapeFields = value21.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value22))
		{
			_updatingControls = value22.As<bool>();
		}
	}
}
