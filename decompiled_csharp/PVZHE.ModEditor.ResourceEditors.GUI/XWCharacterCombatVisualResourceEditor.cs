using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterCombatVisualResourceEditor.cs")]
public class XWCharacterCombatVisualResourceEditor : XWGenericVisualResourceEditor
{
	private readonly record struct DamageProfile(double BaseDamage, double BodyScale, double ArmorScale, int DamageFlags, int CollisionFlags, string SourceLabel);

	private readonly record struct BuffProfile(string DisplayName, double Duration, Color Tint, string Description);

	private readonly record struct ArrayResourceChoice(string Label, Func<Resource> Factory);

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName HasCompleteCharacterCombatVisualCoverage = "HasCompleteCharacterCombatVisualCoverage";

		public static readonly StringName CanDirectlyEditProperty = "CanDirectlyEditProperty";

		public static readonly StringName BindWorkbench = "BindWorkbench";

		public static readonly StringName BuildTargetPreview = "BuildTargetPreview";

		public static readonly StringName BuildVisualPropertyRows = "BuildVisualPropertyRows";

		public static readonly StringName CreateVisualPropertyEditor = "CreateVisualPropertyEditor";

		public static readonly StringName CreateNumberEditor = "CreateNumberEditor";

		public static readonly StringName CreateTextEditor = "CreateTextEditor";

		public static readonly StringName CreateVector2Editor = "CreateVector2Editor";

		public static readonly StringName CreateVectorComponent = "CreateVectorComponent";

		public static readonly StringName CreateVector4IEditor = "CreateVector4IEditor";

		public static readonly StringName CreateIntegerVectorComponent = "CreateIntegerVectorComponent";

		public static readonly StringName AddVectorComponent = "AddVectorComponent";

		public static readonly StringName CreateEventArrayEditor = "CreateEventArrayEditor";

		public static readonly StringName AddArrayResource = "AddArrayResource";

		public static readonly StringName MoveArrayResource = "MoveArrayResource";

		public static readonly StringName RemoveArrayResource = "RemoveArrayResource";

		public static readonly StringName OpenArrayResource = "OpenArrayResource";

		public static readonly StringName CommitArrayProperty = "CommitArrayProperty";

		public static readonly StringName GetCurrentArrayProperty = "GetCurrentArrayProperty";

		public static readonly StringName CloneArrayForHistory = "CloneArrayForHistory";

		public static readonly StringName RefreshArrayEditor = "RefreshArrayEditor";

		public static readonly StringName GetSelectedArrayIndex = "GetSelectedArrayIndex";

		public static readonly StringName FormatArrayResource = "FormatArrayResource";

		public static readonly StringName CreateIntEnumEditor = "CreateIntEnumEditor";

		public static readonly StringName CreateStringEnumEditor = "CreateStringEnumEditor";

		public static readonly StringName WrapSmallSegmentedOption = "WrapSmallSegmentedOption";

		public static readonly StringName MountSmallSegmentedOption = "MountSmallSegmentedOption";

		public static readonly StringName EnsureVisualChoicePicker = "EnsureVisualChoicePicker";

		public static readonly StringName DisposeVisualChoices = "DisposeVisualChoices";

		public static readonly StringName CreateFlagsEditor = "CreateFlagsEditor";

		public static readonly StringName CreateResourceEditor = "CreateResourceEditor";

		public static readonly StringName CreateReadOnlySummary = "CreateReadOnlySummary";

		public static readonly StringName SetVisualProperty = "SetVisualProperty";

		public static readonly StringName OnVisualPropertyEdited = "OnVisualPropertyEdited";

		public static readonly StringName QueueCharacterCombatEditorRefreshFromHistory = "QueueCharacterCombatEditorRefreshFromHistory";

		public static readonly StringName RefreshCharacterCombatEditorFromHistory = "RefreshCharacterCombatEditorFromHistory";

		public static readonly StringName SimulateCurrentResource = "SimulateCurrentResource";

		public static readonly StringName ResetSimulation = "ResetSimulation";

		public static readonly StringName RefreshSimulationSummary = "RefreshSimulationSummary";

		public static readonly StringName SimulateBuff = "SimulateBuff";

		public static readonly StringName ResetBuffVisual = "ResetBuffVisual";

		public static readonly StringName CompleteBuffPreview = "CompleteBuffPreview";

		public static readonly StringName SetTargetTint = "SetTargetTint";

		public static readonly StringName SimulateCharacterEvent = "SimulateCharacterEvent";

		public static readonly StringName AnimateTargetShift = "AnimateTargetShift";

		public static readonly StringName AnimateTargetDestroy = "AnimateTargetDestroy";

		public static readonly StringName ShowEventEffect = "ShowEventEffect";

		public static readonly StringName ResetEventVisual = "ResetEventVisual";

		public static readonly StringName SetTargetOpacity = "SetTargetOpacity";

		public static readonly StringName FindFirstCanvasItem = "FindFirstCanvasItem";

		public static readonly StringName UpdateTargetHud = "UpdateTargetHud";

		public static readonly StringName ShowSimulationMessage = "ShowSimulationMessage";

		public static readonly StringName ReadDoubleProperty = "ReadDoubleProperty";

		public static readonly StringName FormatBuffDuration = "FormatBuffDuration";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName BuildResourceTypeLabel = "BuildResourceTypeLabel";

		public static readonly StringName BuildPropertyLabel = "BuildPropertyLabel";

		public static readonly StringName BuildPropertyNodeSuffix = "BuildPropertyNodeSuffix";

		public static readonly StringName BuildEventDisplayName = "BuildEventDisplayName";

		public static readonly StringName EmptyValue = "EmptyValue";

		public static readonly StringName SplitHintItems = "SplitHintItems";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _previewRoot = "_previewRoot";

		public static readonly StringName _previewFallback = "_previewFallback";

		public static readonly StringName _resourceTypeLabel = "_resourceTypeLabel";

		public static readonly StringName _targetMode = "_targetMode";

		public static readonly StringName _targetHealth = "_targetHealth";

		public static readonly StringName _targetName = "_targetName";

		public static readonly StringName _healthValue = "_healthValue";

		public static readonly StringName _damageNumber = "_damageNumber";

		public static readonly StringName _simulationText = "_simulationText";

		public static readonly StringName _eventEffect = "_eventEffect";

		public static readonly StringName _stateBadge = "_stateBadge";

		public static readonly StringName _stateTimer = "_stateTimer";

		public static readonly StringName _propertyRows = "_propertyRows";

		public static readonly StringName _propertyCount = "_propertyCount";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _damageTween = "_damageTween";

		public static readonly StringName _buffTween = "_buffTween";

		public static readonly StringName _eventTween = "_eventTween";

		public static readonly StringName _effectTween = "_effectTween";

		public static readonly StringName _targetSpriteInstance = "_targetSpriteInstance";

		public static readonly StringName _simulateButton = "_simulateButton";

		public static readonly StringName _currentHealth = "_currentHealth";

		public static readonly StringName _updatingVisualProperty = "_updatingVisualProperty";

		public static readonly StringName _combatRefreshQueued = "_combatRefreshQueued";

		public static readonly StringName _visualChoicePicker = "_visualChoicePicker";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string WorkbenchScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterCombatWorkbench.tscn";

	private const string PropertyRowScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterCombatPropertyRow.tscn";

	private const string ArrayEditorScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterCombatArrayEditor.tscn";

	private const string DefaultTargetSpriteUid = "uid://vp5mpqwb0n8b";

	private const int MaxVisualProperties = 48;

	private const int MaxVisualArrayItems = 64;

	private const double TargetMaxHealth = 1000.0;

	private static PackedScene _workbenchScene;

	private static PackedScene _propertyRowScene;

	private static PackedScene _arrayEditorScene;

	private Node2D _previewRoot;

	private Label _previewFallback;

	private Label _resourceTypeLabel;

	private OptionButton _targetMode;

	private ProgressBar _targetHealth;

	private Label _targetName;

	private Label _healthValue;

	private Label _damageNumber;

	private Label _simulationText;

	private Label _eventEffect;

	private Label _stateBadge;

	private ProgressBar _stateTimer;

	private VBoxContainer _propertyRows;

	private Label _propertyCount;

	private Label _statusLabel;

	private Tween _damageTween;

	private Tween _buffTween;

	private Tween _eventTween;

	private Tween _effectTween;

	private Node _targetSpriteInstance;

	private Button _simulateButton;

	private double _currentHealth = 1000.0;

	private bool _updatingVisualProperty;

	private bool _combatRefreshQueued;

	private XWVisualPropertyBinding _propertyBinding;

	private readonly List<XWVisualSegmentedOption> _segmentedOptions = new List<XWVisualSegmentedOption>();

	private XWGameplayResourcePickerWindow _visualChoicePicker;

	public override void _Ready()
	{
		base._Ready();
		SetProcess(enable: false);
	}

	public override void _ExitTree()
	{
		DisposeVisualChoices();
		_propertyBinding?.Dispose();
		_damageTween?.Kill();
		_buffTween?.Kill();
		_eventTween?.Kill();
		_effectTween?.Kill();
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		if (GodotObject.IsInstanceValid(CurrentResource) && CanvasGrid != null)
		{
			DisposeVisualChoices();
			_propertyBinding?.Dispose();
			_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnVisualPropertyEdited);
			CanvasGrid.Columns = 1;
			if (_workbenchScene == null)
			{
				_workbenchScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterCombatWorkbench.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _workbenchScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindWorkbench(vBoxContainer);
				BuildTargetPreview();
				BuildVisualPropertyRows(CurrentResource);
				ResetSimulation();
				AddSummaryRows(CurrentResource);
			}
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompleteCharacterCombatVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompleteCharacterCombatVisualCoverage(Resource resource)
	{
		if ((!(resource is AttackConfig) && !(resource is TowerDefenseCharacterBuffConfig) && !(resource is TowerDefenseCharacterEventBase) && !(resource is TowerDefenseCharacterEventLuckyDrawItem)) || 1 == 0)
		{
			return false;
		}
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		int num = 0;
		foreach (Dictionary property in resource.GetPropertyList())
		{
			if (TryReadEditableProperty(property, out var name, out var type, out var _, out var hintString) && hashSet.Add(name))
			{
				num++;
				if (num > 48 || !CanDirectlyEditProperty(type, name, hintString))
				{
					return false;
				}
			}
		}
		return true;
	}

	private static bool CanDirectlyEditProperty(Variant.Type type, string name, string hintString)
	{
		switch (type)
		{
		case Variant.Type.Bool:
		case Variant.Type.Int:
		case Variant.Type.Float:
		case Variant.Type.String:
		case Variant.Type.Vector2:
		case Variant.Type.Vector4I:
		case Variant.Type.StringName:
		case Variant.Type.NodePath:
		case Variant.Type.Object:
			return true;
		case Variant.Type.Array:
			return BuildArrayResourceChoices(name, hintString).Count > 0;
		default:
			return false;
		}
	}

	private void BindWorkbench(VBoxContainer workbench)
	{
		_previewRoot = workbench.GetNode<Node2D>("%PreviewRoot");
		_previewFallback = workbench.GetNode<Label>("%PreviewFallback");
		_resourceTypeLabel = workbench.GetNode<Label>("%ResourceType");
		_targetMode = workbench.GetNode<OptionButton>("%TargetMode");
		_targetHealth = workbench.GetNode<ProgressBar>("%TargetHealth");
		_targetName = workbench.GetNode<Label>("%TargetName");
		_healthValue = workbench.GetNode<Label>("%HealthValue");
		_damageNumber = workbench.GetNode<Label>("%DamageNumber");
		_simulationText = workbench.GetNode<Label>("%SimulationText");
		_eventEffect = workbench.GetNode<Label>("%EventEffect");
		_stateBadge = workbench.GetNode<Label>("%StateBadge");
		_stateTimer = workbench.GetNode<ProgressBar>("%StateTimer");
		_propertyRows = workbench.GetNode<VBoxContainer>("%PropertyRows");
		_propertyCount = workbench.GetNode<Label>("%PropertyCount");
		_statusLabel = workbench.GetNode<Label>("%Status");
		_resourceTypeLabel.Text = BuildResourceTypeLabel(CurrentResource);
		bool flag = CurrentResource is TowerDefenseCharacterBuffConfig;
		Resource currentResource = CurrentResource;
		bool flag2 = ((currentResource is TowerDefenseCharacterEventBase || currentResource is TowerDefenseCharacterEventLuckyDrawItem) ? true : false);
		bool flag3 = flag2;
		bool flag4 = TryBuildDamageProfile(CurrentResource, out var _);
		_targetMode.AddItem("身体目标");
		_targetMode.AddItem("盾牌 / 护甲");
		_targetMode.Visible = flag4;
		_targetMode.ItemSelected += (long _) =>
		{
			ResetSimulation();
		};
		HFlowContainer node = workbench.GetNode<HFlowContainer>("%TargetModeSegments");
		node.Visible = flag4;
		if (flag4)
		{
			MountSmallSegmentedOption(_targetMode, node);
		}
		workbench.GetNode<Button>("%ResetButton").Pressed += ResetSimulation;
		_simulateButton = workbench.GetNode<Button>("%SimulateButton");
		Button simulateButton = _simulateButton;
		string text;
		if (flag)
		{
			text = "▶ 应用 Buff";
		}
		else
		{
			text = ((flag3 && !flag4) ? "▶ 触发事件预览" : "▶ 模拟命中");
		}
		simulateButton.Text = text;
		_simulateButton.Pressed += SimulateCurrentResource;
	}

	private void BuildTargetPreview()
	{
		if (!GodotObject.IsInstanceValid(_previewRoot))
		{
			return;
		}
		foreach (Node child in _previewRoot.GetChildren())
		{
			child.QueueFree();
		}
		_targetSpriteInstance = null;
		PackedScene packedScene = ResourceManager.Instance?.GetCharacterSprite("ZombieNormal") ?? ResourceLoader.Load<PackedScene>("uid://vp5mpqwb0n8b", null, ResourceLoader.CacheMode.Reuse);
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			_previewFallback.Visible = true;
			_previewFallback.Text = "无法加载 ZombieNormal 游戏动画，伤害数值模拟仍可使用。";
			return;
		}
		try
		{
			_targetSpriteInstance = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			XWRuntimePreviewSceneAdapter.PrepareAnimationSprites(_targetSpriteInstance);
			_previewRoot.AddChild(_targetSpriteInstance, forceReadableName: false, InternalMode.Disabled);
			if (_targetSpriteInstance is Node2D node2D)
			{
				node2D.Position = Vector2.Zero;
				node2D.Scale = new Vector2(0.72f, 0.72f);
			}
			_previewFallback.Visible = false;
		}
		catch (Exception ex)
		{
			_previewFallback.Visible = true;
			_previewFallback.Text = "角色动画预览失败：" + ex.Message;
		}
	}

	private void BuildVisualPropertyRows(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(_propertyRows))
		{
			return;
		}
		foreach (Node child in _propertyRows.GetChildren())
		{
			child.QueueFree();
		}
		if (_propertyRowScene == null)
		{
			_propertyRowScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterCombatPropertyRow.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		int num = 0;
		foreach (Dictionary property in resource.GetPropertyList())
		{
			if (num >= 48 || !TryReadEditableProperty(property, out var name, out var type, out var hint, out var hintString) || !hashSet.Add(name) || !TryGetProperty(resource, name, out var value))
			{
				continue;
			}
			PanelContainer panelContainer = _propertyRowScene?.Instantiate<PanelContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(panelContainer))
			{
				Control control = CreateVisualPropertyEditor(name, type, hint, hintString, value);
				if (GodotObject.IsInstanceValid(control))
				{
					string text = BuildPropertyNodeSuffix(name);
					panelContainer.Name = "CombatRow_" + text;
					control.Name = "CombatProperty_" + text;
					_propertyRows.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
					panelContainer.GetNode<Label>("%PropertyName").Text = BuildPropertyLabel(name);
					panelContainer.GetNode<Label>("%PropertyName").TooltipText = name;
					panelContainer.GetNode<HBoxContainer>("%ValueHost").AddChild(control, forceReadableName: false, InternalMode.Disabled);
					num++;
				}
			}
		}
		_propertyCount.Text = $"{num} 个可视属性";
	}

	private Control CreateVisualPropertyEditor(string name, Variant.Type type, PropertyHint hint, string hintString, Variant value)
	{
		Variant.Type num = type - 1;
		if ((ulong)num <= 4uL)
		{
			switch ((int)num)
			{
			case 0:
			{
				CheckButton check = new CheckButton
				{
					Text = (value.AsBool() ? "启用" : "关闭"),
					ButtonPressed = value.AsBool()
				};
				check.Toggled += (bool enabled) =>
				{
					check.Text = (enabled ? "启用" : "关闭");
					SetVisualProperty(name, Variant.From(in enabled));
				};
				return check;
			}
			case 1:
				goto IL_00d1;
			case 2:
				return CreateNumberEditor(name, value.AsDouble(), integer: false);
			case 3:
				goto IL_0132;
			case 4:
				return CreateVector2Editor(name, value.AsVector2());
			}
		}
		if (type != Variant.Type.Vector4I)
		{
			Variant.Type num2 = type - 21;
			if ((ulong)num2 > 7uL)
			{
				goto IL_0242;
			}
			switch ((int)num2)
			{
			case 0:
			case 1:
				break;
			case 3:
				return CreateResourceEditor(name, value.AsGodotObject() as Resource, hintString);
			case 7:
				return CreateEventArrayEditor(name, value.AsGodotArray(), hintString) ?? CreateReadOnlySummary($"Array({value.AsGodotArray().Count}) · 扩展类型需要补充专用可视条目卡");
			case 6:
				return CreateReadOnlySummary($"Dictionary({value.AsGodotDictionary().Count}) · 扩展类型需要补充专用可视键值卡");
			default:
				goto IL_0242;
			}
			goto IL_0132;
		}
		return CreateVector4IEditor(name, value.AsVector4I());
		IL_0132:
		if (hint != PropertyHint.Enum)
		{
			return CreateTextEditor(name, value.AsString(), type);
		}
		return CreateStringEnumEditor(name, value.AsString(), hintString);
		IL_00d1:
		return hint switch
		{
			PropertyHint.Flags => CreateFlagsEditor(name, value.AsInt32(), hintString), 
			PropertyHint.Enum => CreateIntEnumEditor(name, value.AsInt32(), hintString), 
			_ => CreateNumberEditor(name, value.AsInt64(), integer: true), 
		};
		IL_0242:
		return CreateReadOnlySummary(value.ToString());
	}

	private Control CreateNumberEditor(string name, double value, bool integer)
	{
		SpinBox spinBox = new SpinBox();
		spinBox.MinValue = -1000000000.0;
		spinBox.MaxValue = 1000000000.0;
		spinBox.AllowGreater = true;
		spinBox.AllowLesser = true;
		SpinBox spinBox2 = spinBox;
		double step;
		if (integer)
		{
			step = 1.0;
		}
		else
		{
			step = ((Math.Abs(value) <= 1.0) ? 0.01 : 0.1);
		}
		spinBox2.Step = step;
		spinBox.Value = value;
		spinBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		spinBox.ValueChanged += (double next) =>
		{
			SetVisualProperty(name, integer ? Variant.From<long>((long)Math.Round(next)) : Variant.From(in next));
		};
		return spinBox;
	}

	private Control CreateTextEditor(string name, string value, Variant.Type type)
	{
		LineEdit edit = new LineEdit
		{
			Text = (value ?? ""),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		edit.TextSubmitted += Apply;
		edit.FocusExited += () =>
		{
			Apply(edit.Text);
		};
		return edit;
		void Apply(string text)
		{
			Variant value2 = type switch
			{
				Variant.Type.StringName => Variant.From<StringName>(new StringName(text)), 
				Variant.Type.NodePath => Variant.From<NodePath>(new NodePath(text)), 
				_ => Variant.From(in text), 
			};
			SetVisualProperty(name, value2);
		}
	}

	private Control CreateVector2Editor(string name, Vector2 value)
	{
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		SpinBox x = CreateVectorComponent("X", value.X);
		SpinBox y = CreateVectorComponent("Y", value.Y);
		hBoxContainer.AddChild(new Label
		{
			Text = "X",
			VerticalAlignment = VerticalAlignment.Center
		}, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(x, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(new Label
		{
			Text = "Y",
			VerticalAlignment = VerticalAlignment.Center
		}, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(y, forceReadableName: false, InternalMode.Disabled);
		x.ValueChanged += (double next) =>
		{
			SetVisualProperty(name, Variant.From<Vector2>(new Vector2((float)next, (float)y.Value)));
		};
		y.ValueChanged += (double next) =>
		{
			SetVisualProperty(name, Variant.From<Vector2>(new Vector2((float)x.Value, (float)next)));
		};
		return hBoxContainer;
	}

	private static SpinBox CreateVectorComponent(string name, double value)
	{
		return new SpinBox
		{
			Name = name,
			MinValue = -1000000.0,
			MaxValue = 1000000.0,
			AllowGreater = true,
			AllowLesser = true,
			Step = 0.1,
			Value = value,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
	}

	private Control CreateVector4IEditor(string name, Vector4I value)
	{
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		SpinBox x = CreateIntegerVectorComponent("X", value.X);
		SpinBox y = CreateIntegerVectorComponent("Y", value.Y);
		SpinBox z = CreateIntegerVectorComponent("Z", value.Z);
		SpinBox w = CreateIntegerVectorComponent("W", value.W);
		AddVectorComponent(hBoxContainer, "X", x);
		AddVectorComponent(hBoxContainer, "Y", y);
		AddVectorComponent(hBoxContainer, "Z", z);
		AddVectorComponent(hBoxContainer, "W", w);
		x.ValueChanged += (double next) =>
		{
			SetVisualProperty(name, Variant.From<Vector4I>(new Vector4I((int)next, (int)y.Value, (int)z.Value, (int)w.Value)));
		};
		y.ValueChanged += (double next) =>
		{
			SetVisualProperty(name, Variant.From<Vector4I>(new Vector4I((int)x.Value, (int)next, (int)z.Value, (int)w.Value)));
		};
		z.ValueChanged += (double next) =>
		{
			SetVisualProperty(name, Variant.From<Vector4I>(new Vector4I((int)x.Value, (int)y.Value, (int)next, (int)w.Value)));
		};
		w.ValueChanged += (double next) =>
		{
			SetVisualProperty(name, Variant.From<Vector4I>(new Vector4I((int)x.Value, (int)y.Value, (int)z.Value, (int)next)));
		};
		return hBoxContainer;
	}

	private static SpinBox CreateIntegerVectorComponent(string name, int value)
	{
		return new SpinBox
		{
			Name = name,
			MinValue = -1000000.0,
			MaxValue = 1000000.0,
			AllowGreater = true,
			AllowLesser = true,
			Step = 1.0,
			Value = value,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
	}

	private static void AddVectorComponent(HBoxContainer row, string label, SpinBox editor)
	{
		row.AddChild(new Label
		{
			Text = label,
			VerticalAlignment = VerticalAlignment.Center
		}, forceReadableName: false, InternalMode.Disabled);
		row.AddChild(editor, forceReadableName: false, InternalMode.Disabled);
	}

	private Control CreateEventArrayEditor(string propertyName, Godot.Collections.Array array, string hintString)
	{
		List<ArrayResourceChoice> choices = BuildArrayResourceChoices(propertyName, hintString);
		if (choices.Count == 0)
		{
			return null;
		}
		if (_arrayEditorScene == null)
		{
			_arrayEditorScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCharacterCombatArrayEditor.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _arrayEditorScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(vBoxContainer))
		{
			return null;
		}
		Label summary = vBoxContainer.GetNode<Label>("%Summary");
		OptionButton addType = vBoxContainer.GetNode<OptionButton>("%AddType");
		Label selectedType = vBoxContainer.GetNode<Label>("%SelectedAddTypeLabel");
		ItemList items = vBoxContainer.GetNode<ItemList>("%ArrayItems");
		for (int i = 0; i < choices.Count; i++)
		{
			addType.AddItem(choices[i].Label, i);
		}
		if (choices.Count > 0)
		{
			selectedType.Text = choices[0].Label;
		}
		vBoxContainer.GetNode<Button>("%AddTypeCatalogButton").Pressed += () =>
		{
			OpenArrayTypeCatalog(addType, selectedType, choices);
		};
		vBoxContainer.GetNode<Button>("%AddButton").Pressed += () =>
		{
			int index = Mathf.Clamp(addType.Selected, 0, choices.Count - 1);
			AddArrayResource(propertyName, choices[index].Factory(), items, summary);
		};
		vBoxContainer.GetNode<Button>("%MoveUpButton").Pressed += () =>
		{
			MoveArrayResource(propertyName, items, summary, -1);
		};
		vBoxContainer.GetNode<Button>("%MoveDownButton").Pressed += () =>
		{
			MoveArrayResource(propertyName, items, summary, 1);
		};
		vBoxContainer.GetNode<Button>("%RemoveButton").Pressed += () =>
		{
			RemoveArrayResource(propertyName, items, summary);
		};
		vBoxContainer.GetNode<Button>("%OpenButton").Pressed += () =>
		{
			OpenArrayResource(propertyName, items);
		};
		items.ItemActivated += (long _) =>
		{
			OpenArrayResource(propertyName, items);
		};
		RefreshArrayEditor(array, items, summary);
		return vBoxContainer;
	}

	private void AddArrayResource(string propertyName, Resource resource, ItemList items, Label summary)
	{
		Godot.Collections.Array currentArrayProperty = GetCurrentArrayProperty(propertyName);
		if (GodotObject.IsInstanceValid(resource) && currentArrayProperty != null && currentArrayProperty.Count < 64)
		{
			Godot.Collections.Array array = CloneArrayForHistory(currentArrayProperty);
			array.Add(Variant.From(in resource));
			CommitArrayProperty(propertyName, array);
			RefreshArrayEditor(array, items, summary);
			items.Select(array.Count - 1);
		}
	}

	private void MoveArrayResource(string propertyName, ItemList items, Label summary, int direction)
	{
		Godot.Collections.Array currentArrayProperty = GetCurrentArrayProperty(propertyName);
		int selectedArrayIndex = GetSelectedArrayIndex(items, currentArrayProperty);
		int num = selectedArrayIndex + direction;
		if (selectedArrayIndex >= 0 && num >= 0 && num < currentArrayProperty.Count)
		{
			Godot.Collections.Array array = CloneArrayForHistory(currentArrayProperty);
			Variant item = array[selectedArrayIndex];
			array.RemoveAt(selectedArrayIndex);
			array.Insert(num, item);
			CommitArrayProperty(propertyName, array);
			RefreshArrayEditor(array, items, summary);
			items.Select(num);
		}
	}

	private void RemoveArrayResource(string propertyName, ItemList items, Label summary)
	{
		Godot.Collections.Array currentArrayProperty = GetCurrentArrayProperty(propertyName);
		int selectedArrayIndex = GetSelectedArrayIndex(items, currentArrayProperty);
		if (selectedArrayIndex >= 0)
		{
			Godot.Collections.Array array = CloneArrayForHistory(currentArrayProperty);
			array.RemoveAt(selectedArrayIndex);
			CommitArrayProperty(propertyName, array);
			RefreshArrayEditor(array, items, summary);
			if (array.Count > 0)
			{
				items.Select(Math.Min(selectedArrayIndex, array.Count - 1));
			}
		}
	}

	private void OpenArrayResource(string propertyName, ItemList items)
	{
		Godot.Collections.Array currentArrayProperty = GetCurrentArrayProperty(propertyName);
		int selectedArrayIndex = GetSelectedArrayIndex(items, currentArrayProperty);
		if (selectedArrayIndex >= 0 && currentArrayProperty[selectedArrayIndex].AsGodotObject() is Resource resource && GodotObject.IsInstanceValid(resource))
		{
			XWResourceEditContext context = XWResourceEditContext.ForProperty(resource, CurrentResource, resource.ResourcePath, CurrentResourcePath, propertyName, selectedArrayIndex, "character_combat_editor", CurrentEditContext?.IsBuiltInSource ?? XWResourceEditContext.IsBuiltInPath(CurrentResourcePath));
			XWEditorInterface.Instance?.EditResource(resource, context);
		}
	}

	private void CommitArrayProperty(string propertyName, Godot.Collections.Array array)
	{
		if (GodotObject.IsInstanceValid(CurrentResource))
		{
			_propertyBinding?.SetValue(CurrentResource, propertyName, Variant.From(in array), "修改 " + propertyName, this, "QueueCharacterCombatEditorRefreshFromHistory");
		}
	}

	private Godot.Collections.Array GetCurrentArrayProperty(string propertyName)
	{
		if (!GodotObject.IsInstanceValid(CurrentResource) || string.IsNullOrWhiteSpace(propertyName))
		{
			return new Godot.Collections.Array();
		}
		Variant variant = CurrentResource.Get(propertyName);
		if (variant.VariantType != Variant.Type.Array)
		{
			return new Godot.Collections.Array();
		}
		return variant.AsGodotArray();
	}

	private static Godot.Collections.Array CloneArrayForHistory(Godot.Collections.Array source)
	{
		return source?.Duplicate(deep: true) ?? new Godot.Collections.Array();
	}

	private static void RefreshArrayEditor(Godot.Collections.Array array, ItemList items, Label summary)
	{
		items.Clear();
		int num = Math.Min(array?.Count ?? 0, 64);
		for (int i = 0; i < num; i++)
		{
			Resource resource = array[i].AsGodotObject() as Resource;
			items.AddItem($"{i + 1}. {FormatArrayResource(resource)}");
		}
		summary.Text = $"{array?.Count ?? 0} 个子资源" + (((array?.Count ?? 0) >= 64) ? " · 已达可视上限" : "");
	}

	private static int GetSelectedArrayIndex(ItemList items, Godot.Collections.Array array)
	{
		int[] selectedItems = items.GetSelectedItems();
		if (selectedItems.Length != 0 && selectedItems[0] >= 0 && selectedItems[0] < (array?.Count ?? 0))
		{
			return selectedItems[0];
		}
		return -1;
	}

	private static string FormatArrayResource(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "空子资源";
		}
		if (!string.IsNullOrWhiteSpace(resource.ResourceName))
		{
			return resource.ResourceName + " · " + resource.GetType().Name;
		}
		return resource.GetType().Name;
	}

	private Control CreateIntEnumEditor(string name, int value, string hintString)
	{
		OptionButton option = new OptionButton
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		string[] array = SplitHintItems(hintString);
		for (int i = 0; i < array.Length; i++)
		{
			option.AddItem(array[i], i);
			if (i == value)
			{
				option.Select(i);
			}
		}
		option.ItemSelected += (long index) =>
		{
			SetVisualProperty(name, Variant.From<long>((long)option.GetItemId((int)index)));
		};
		if (array.Length >= 2 && array.Length <= 8)
		{
			return WrapSmallSegmentedOption(option);
		}
		return CreateLargeChoiceButton(array, option);
	}

	private Control CreateStringEnumEditor(string name, string value, string hintString)
	{
		OptionButton option = new OptionButton
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		string[] array = SplitHintItems(hintString);
		for (int i = 0; i < array.Length; i++)
		{
			option.AddItem(array[i], i);
			if (string.Equals(array[i], value, StringComparison.OrdinalIgnoreCase))
			{
				option.Select(i);
			}
		}
		option.ItemSelected += (long index) =>
		{
			SetVisualProperty(name, Variant.From<string>(option.GetItemText((int)index)));
		};
		if (array.Length >= 2 && array.Length <= 8)
		{
			return WrapSmallSegmentedOption(option);
		}
		return CreateLargeChoiceButton(array, option);
	}

	private Control WrapSmallSegmentedOption(OptionButton option)
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		option.Visible = false;
		vBoxContainer.AddChild(option, forceReadableName: false, InternalMode.Disabled);
		HFlowContainer hFlowContainer = new HFlowContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
		MountSmallSegmentedOption(option, hFlowContainer);
		return vBoxContainer;
	}

	private Control CreateLargeChoiceButton(IReadOnlyList<string> items, OptionButton option)
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		option.Visible = false;
		vBoxContainer.AddChild(option, forceReadableName: false, InternalMode.Disabled);
		Button button = new Button
		{
			Text = ((items.Count == 0) ? "类型图鉴" : ("图鉴 · " + items[Mathf.Clamp(option.Selected, 0, items.Count - 1)])),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			TooltipText = "打开可搜索类型图鉴"
		};
		button.Pressed += () =>
		{
			EnsureVisualChoicePicker();
			_visualChoicePicker?.OpenChoices("类型图鉴", "搜索并选择配置枚举", option.Selected.ToString(), BuildTextChoices(items), (XWGameplayResourceChoice choice) =>
			{
				if (int.TryParse(choice?.Key, out var result) && result >= 0 && result < option.ItemCount)
				{
					option.Select(result);
					button.Text = "图鉴 · " + items[result];
					option.EmitSignal(OptionButton.SignalName.ItemSelected, result);
				}
			});
		};
		vBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private void MountSmallSegmentedOption(OptionButton option, HFlowContainer host)
	{
		if (GodotObject.IsInstanceValid(option) && GodotObject.IsInstanceValid(host))
		{
			XWVisualSegmentedOption xWVisualSegmentedOption = new XWVisualSegmentedOption(option, host);
			xWVisualSegmentedOption.Rebuild();
			_segmentedOptions.Add(xWVisualSegmentedOption);
		}
	}

	private void OpenArrayTypeCatalog(OptionButton option, Label selectedLabel, IReadOnlyList<ArrayResourceChoice> choices)
	{
		EnsureVisualChoicePicker();
		List<string> list = new List<string>(choices.Count);
		foreach (ArrayResourceChoice choice in choices)
		{
			list.Add(choice.Label);
		}
		_visualChoicePicker?.OpenChoices("子资源类型图鉴", "搜索事件、Buff 与战斗子资源类型", option.Selected.ToString(), BuildTextChoices(list), (XWGameplayResourceChoice choice) =>
		{
			if (int.TryParse(choice?.Key, out var result) && result >= 0 && result < choices.Count)
			{
				option.Select(result);
				selectedLabel.Text = choices[result].Label;
			}
		});
	}

	private static List<XWGameplayResourceChoice> BuildTextChoices(IReadOnlyList<string> labels)
	{
		List<XWGameplayResourceChoice> list = new List<XWGameplayResourceChoice>(labels?.Count ?? 0);
		if (labels == null)
		{
			return list;
		}
		for (int i = 0; i < labels.Count; i++)
		{
			list.Add(new XWGameplayResourceChoice(i.ToString(), labels[i], string.Empty, ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ResourceCard.svg", null, ResourceLoader.CacheMode.Reuse), XWGameplayResourceKind.Resource, IsModResource: false));
		}
		return list;
	}

	private void EnsureVisualChoicePicker()
	{
		if (!GodotObject.IsInstanceValid(_visualChoicePicker))
		{
			_visualChoicePicker = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_visualChoicePicker))
			{
				AddChild(_visualChoicePicker, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void DisposeVisualChoices()
	{
		foreach (XWVisualSegmentedOption segmentedOption in _segmentedOptions)
		{
			segmentedOption?.Dispose();
		}
		_segmentedOptions.Clear();
	}

	private Control CreateFlagsEditor(string name, int value, string hintString)
	{
		MenuButton menu = new MenuButton
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		string[] flags = SplitHintItems(hintString);
		for (int i = 0; i < flags.Length; i++)
		{
			menu.GetPopup().AddCheckItem(flags[i], i, Key.None);
			menu.GetPopup().SetItemChecked(i, (value & (1 << i)) != 0);
		}
		UpdateFlagsText(menu, value, flags);
		menu.GetPopup().IdPressed += (long id) =>
		{
			int num = Mathf.Clamp((int)id, 0, Math.Max(0, flags.Length - 1));
			value ^= 1 << num;
			menu.GetPopup().SetItemChecked(num, (value & (1 << num)) != 0);
			UpdateFlagsText(menu, value, flags);
			SetVisualProperty(name, Variant.From<long>((long)value));
		};
		return menu;
	}

	private Control CreateResourceEditor(string name, Resource value, string hintString)
	{
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		XWResourcePicker picker = XWResourcePicker.Create();
		string className = (string.IsNullOrWhiteSpace(hintString) ? "Resource" : hintString);
		picker.Setup(className);
		picker.SetEditedResource(value);
		picker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		picker.ResourceChanged += (Resource resource) =>
		{
			SetVisualProperty(name, Variant.From(in resource));
		};
		hBoxContainer.AddChild(picker, forceReadableName: false, InternalMode.Disabled);
		Button button = new Button
		{
			Text = "图鉴",
			TooltipText = "搜索游戏与当前 Mod 资源",
			CustomMinimumSize = new Vector2(72f, 34f)
		};
		button.Pressed += () =>
		{
			EnsureVisualChoicePicker();
			Resource obj = (GodotObject.IsInstanceValid(CurrentResource) ? (CurrentResource.Get(name).AsGodotObject() as Resource) : null);
			_visualChoicePicker?.OpenResourceLibrary(className, BuildPropertyLabel(name), obj?.ResourcePath ?? string.Empty, XWFileSystem.GetSingleton()?.ProjectFolderPath ?? string.Empty, new string[1] { className }, new string[2] { "Resources", "Asset" }, "res://addons/ModEditor/Icons/ResourceCard.svg", (XWGameplayResourceChoice choice) =>
			{
				Resource from = ResourceLoader.Load<Resource>(choice?.ResourcePath ?? string.Empty, null, ResourceLoader.CacheMode.Reuse);
				picker.SetEditedResource(from);
				SetVisualProperty(name, Variant.From(in from));
			});
		};
		hBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		return hBoxContainer;
	}

	private static Control CreateReadOnlySummary(string text)
	{
		return new Label
		{
			Text = text,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			AutowrapMode = TextServer.AutowrapMode.WordSmart
		};
	}

	private void SetVisualProperty(string name, Variant value)
	{
		if (_updatingVisualProperty || !GodotObject.IsInstanceValid(CurrentResource) || string.IsNullOrWhiteSpace(name))
		{
			return;
		}
		try
		{
			_updatingVisualProperty = true;
			_propertyBinding?.SetValue(CurrentResource, name, value, "修改 " + name, this, "QueueCharacterCombatEditorRefreshFromHistory");
		}
		catch (Exception ex)
		{
			GD.PushWarning("Character combat visual property update failed: " + name + " " + ex.Message);
		}
		finally
		{
			_updatingVisualProperty = false;
		}
	}

	private void OnVisualPropertyEdited(bool committed)
	{
		NotifyCurrentResourceEdited();
		RefreshSimulationSummary();
	}

	public void QueueCharacterCombatEditorRefreshFromHistory()
	{
		if (!_combatRefreshQueued)
		{
			_combatRefreshQueued = true;
			CallDeferred("RefreshCharacterCombatEditorFromHistory");
		}
	}

	public void RefreshCharacterCombatEditorFromHistory()
	{
		_combatRefreshQueued = false;
		if (GodotObject.IsInstanceValid(CurrentResource))
		{
			BuildVisualPropertyRows(CurrentResource);
			RefreshSimulationSummary();
		}
	}

	private void SimulateCurrentResource()
	{
		if (CurrentResource is TowerDefenseCharacterBuffConfig buff)
		{
			SimulateBuff(buff);
			return;
		}
		if (CurrentResource is TowerDefenseCharacterEventLuckyDrawItem towerDefenseCharacterEventLuckyDrawItem)
		{
			if (GodotObject.IsInstanceValid(towerDefenseCharacterEventLuckyDrawItem._event))
			{
				SimulateCharacterEvent(towerDefenseCharacterEventLuckyDrawItem._event, $"权重 {towerDefenseCharacterEventLuckyDrawItem.weight:0.###}");
			}
			else
			{
				ShowEventEffect($"空奖池项 · 权重 {towerDefenseCharacterEventLuckyDrawItem.weight:0.###}", new Color(0.75f, 0.75f, 0.75f), flyToTarget: false);
			}
			return;
		}
		if (CurrentResource is TowerDefenseCharacterEventBase towerDefenseCharacterEventBase && !TryBuildDamageProfile(towerDefenseCharacterEventBase, out var _))
		{
			SimulateCharacterEvent(towerDefenseCharacterEventBase);
			return;
		}
		if (!TryBuildDamageProfile(CurrentResource, out var profile2))
		{
			ShowSimulationMessage("该资源不是直接伤害类型，暂时没有伤害数值可模拟。", "—", hit: false);
			return;
		}
		bool flag = _targetMode.Selected == 1;
		bool flag2 = (flag ? ((profile2.DamageFlags & 1) != 0) : ((profile2.DamageFlags & 2) != 0));
		bool flag3 = (profile2.CollisionFlags & 1) != 0;
		if (!flag2 || !flag3)
		{
			string text = ((!flag2) ? "伤害标志未包含当前目标" : "碰撞标志未包含地面角色");
			ShowSimulationMessage("MISS · " + text, "MISS", hit: false);
			return;
		}
		double num = (flag ? profile2.ArmorScale : profile2.BodyScale);
		double num2 = Math.Max(0.0, profile2.BaseDamage * num);
		_currentHealth = Math.Max(0.0, _currentHealth - num2);
		UpdateTargetHud();
		ShowSimulationMessage($"{profile2.SourceLabel} · {profile2.BaseDamage:0.##} × {num:0.###} = {num2:0.##}", $"-{num2:0.##}", hit: true);
	}

	private void ResetSimulation()
	{
		ResetBuffVisual();
		ResetEventVisual();
		_currentHealth = 1000.0;
		if (GodotObject.IsInstanceValid(_damageNumber))
		{
			_damageNumber.Visible = false;
		}
		UpdateTargetHud();
		RefreshSimulationSummary();
	}

	private void RefreshSimulationSummary()
	{
		if (GodotObject.IsInstanceValid(_simulationText))
		{
			DamageProfile profile2;
			DamageProfile profile3;
			if (CurrentResource is TowerDefenseCharacterBuffConfig buff && TryBuildBuffProfile(buff, out var profile))
			{
				_simulationText.Text = "待命 · " + profile.DisplayName + " · " + FormatBuffDuration(profile.Duration);
				_statusLabel.Text = profile.Description + " · 隔离模拟，不调用 Buff 生命周期";
			}
			else if (CurrentResource is TowerDefenseCharacterEventBase resource && !TryBuildDamageProfile(resource, out profile2))
			{
				_simulationText.Text = "待命 · " + BuildEventDisplayName(resource) + " · 点击按钮触发隔离预览";
				_statusLabel.Text = "事件预览只解释和动画化配置，不调用真实战斗方法。";
			}
			else if (CurrentResource is TowerDefenseCharacterEventLuckyDrawItem towerDefenseCharacterEventLuckyDrawItem)
			{
				_simulationText.Text = $"待命 · 奖池权重 {towerDefenseCharacterEventLuckyDrawItem.weight:0.###} · {BuildEventDisplayName(towerDefenseCharacterEventLuckyDrawItem._event)}";
				_statusLabel.Text = "奖池项预览不会运行真实随机抽取。";
			}
			else if (TryBuildDamageProfile(CurrentResource, out profile3))
			{
				OptionButton targetMode = _targetMode;
				double num = ((targetMode != null && targetMode.Selected == 1) ? profile3.ArmorScale : profile3.BodyScale);
				_simulationText.Text = $"待命 · {profile3.SourceLabel} · 预计伤害 {profile3.BaseDamage * num:0.##}";
				_statusLabel.Text = $"伤害标志 0x{profile3.DamageFlags:X} · 碰撞标志 0x{profile3.CollisionFlags:X} · 隔离模拟，不执行真实事件";
			}
			else
			{
				_simulationText.Text = "该资源没有直接伤害数值。";
				_statusLabel.Text = "隔离预览不会执行真实战斗事件或访问战斗管理器。";
			}
		}
	}

	private void SimulateBuff(TowerDefenseCharacterBuffConfig buff)
	{
		if (TryBuildBuffProfile(buff, out var profile))
		{
			_buffTween?.Kill();
			_damageNumber.Visible = false;
			SetTargetTint(_targetSpriteInstance, profile.Tint);
			_stateBadge.Text = "状态：" + profile.DisplayName;
			_stateBadge.Modulate = profile.Tint.Lightened(0.25f);
			_simulationText.Text = $"已应用 {profile.DisplayName} · {profile.Description} · {FormatBuffDuration(profile.Duration)}";
			if (profile.Duration < 0.0)
			{
				_stateTimer.Visible = true;
				_stateTimer.MaxValue = 1.0;
				_stateTimer.Value = 1.0;
				return;
			}
			double num = Math.Max(0.01, profile.Duration);
			double duration = Mathf.Clamp(profile.Duration, 0.8, 5.0);
			_stateTimer.Visible = true;
			_stateTimer.MaxValue = num;
			_stateTimer.Value = num;
			_buffTween = CreateTween();
			_buffTween.TweenProperty(_stateTimer, "value", 0.0, duration).SetTrans(Tween.TransitionType.Linear);
			_buffTween.TweenCallback(Callable.From(CompleteBuffPreview));
		}
	}

	private void ResetBuffVisual()
	{
		_buffTween?.Kill();
		_buffTween = null;
		CompleteBuffPreview();
	}

	private void CompleteBuffPreview()
	{
		SetTargetTint(_targetSpriteInstance, Colors.White);
		if (GodotObject.IsInstanceValid(_stateBadge))
		{
			_stateBadge.Text = "状态：正常";
			_stateBadge.Modulate = Colors.White;
		}
		if (GodotObject.IsInstanceValid(_stateTimer))
		{
			_stateTimer.Visible = false;
		}
	}

	private static void SetTargetTint(Node node, Color tint)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is CanvasItem canvasItem)
		{
			canvasItem.Modulate = tint;
			return;
		}
		foreach (Node child in node.GetChildren())
		{
			SetTargetTint(child, tint);
		}
	}

	private void SimulateCharacterEvent(TowerDefenseCharacterEventBase characterEvent, string prefix = "")
	{
		string text = (string.IsNullOrWhiteSpace(prefix) ? "" : (prefix + " · "));
		if (!(characterEvent is TowerDefenseCharacterEventAddBuff towerDefenseCharacterEventAddBuff))
		{
			if (!(characterEvent is TowerDefenseCharacterEventForzen towerDefenseCharacterEventForzen))
			{
				if (!(characterEvent is TowerDefenseCharacterEventIceSpeedDown towerDefenseCharacterEventIceSpeedDown))
				{
					if (!(characterEvent is TowerDefenseCharacterEventHypnoses towerDefenseCharacterEventHypnoses))
					{
						if (!(characterEvent is TowerDefenseCharacterEventConditionRandom towerDefenseCharacterEventConditionRandom))
						{
							if (!(characterEvent is TowerDefenseCharacterEventConditionHitpointBelow towerDefenseCharacterEventConditionHitpointBelow))
							{
								if (!(characterEvent is TowerDefenseCharacterEventHitBack towerDefenseCharacterEventHitBack))
								{
									if (!(characterEvent is TowerDefenseCharacterEventGarlic))
									{
										if (!(characterEvent is TowerDefenseCharacterEventDestroy))
										{
											if (!(characterEvent is TowerDefenseCharacterEventPurify) && !(characterEvent is TowerDefenseCharacterEventWakeUp) && !(characterEvent is TowerDefenseCharacterEventArmorClear))
											{
												if (!(characterEvent is TowerDefenseCharacterEventCreateProjectile towerDefenseCharacterEventCreateProjectile))
												{
													if (!(characterEvent is TowerDefenseCharacterEventProjectileCreate towerDefenseCharacterEventProjectileCreate))
													{
														if (!(characterEvent is TowerDefenseCharacterEventExplodeProjectileFromMetaData towerDefenseCharacterEventExplodeProjectileFromMetaData))
														{
															if (!(characterEvent is TowerDefenseCharacterEventSnowBallSpawn))
															{
																if (!(characterEvent is TowerDefenseCharacterEventSunCreate towerDefenseCharacterEventSunCreate))
																{
																	if (!(characterEvent is TowerDefenseCharacterEventCoinCreate towerDefenseCharacterEventCoinCreate))
																	{
																		if (!(characterEvent is TowerDefenseCharacterEventYBCreate towerDefenseCharacterEventYBCreate))
																		{
																			if (!(characterEvent is TowerDefenseCharacterEventGoldShardCreate towerDefenseCharacterEventGoldShardCreate))
																			{
																				if (!(characterEvent is TowerDefenseCharacterEventPacketCreate towerDefenseCharacterEventPacketCreate))
																				{
																					if (!(characterEvent is TowerDefenseCharacterEventPacketSpawn towerDefenseCharacterEventPacketSpawn))
																					{
																						if (!(characterEvent is TowerDefenseCharacterEventCreateAddPacket towerDefenseCharacterEventCreateAddPacket))
																						{
																							if (!(characterEvent is TowerDefenseCharacterEventDieSpawn towerDefenseCharacterEventDieSpawn))
																							{
																								if (!(characterEvent is TowerDefenseCharacterEventCraterCreate))
																								{
																									if (characterEvent is TowerDefenseCharacterEventCreateEffect)
																									{
																										ShowEventEffect(text + "创建游戏特效", new Color(0.55f, 0.88f, 1f), flyToTarget: false);
																									}
																									else
																									{
																										ShowEventEffect(text + BuildEventDisplayName(characterEvent), new Color(0.9f, 0.86f, 0.62f), flyToTarget: false);
																									}
																								}
																								else
																								{
																									ShowEventEffect(text + "创建弹坑", new Color(0.45f, 0.3f, 0.2f), flyToTarget: false);
																								}
																							}
																							else
																							{
																								ShowEventEffect($"{text}死亡生成 {EmptyValue(towerDefenseCharacterEventDieSpawn.packetName)} · {towerDefenseCharacterEventDieSpawn.percentage:P0}", new Color(0.65f, 0.88f, 0.5f), flyToTarget: false);
																							}
																						}
																						else
																						{
																							ShowEventEffect(text + "加入卡片 " + EmptyValue(towerDefenseCharacterEventCreateAddPacket.packetName), new Color(0.55f, 0.9f, 0.45f), flyToTarget: false);
																						}
																					}
																					else
																					{
																						ShowEventEffect($"{text}生成角色 {EmptyValue(towerDefenseCharacterEventPacketSpawn.packetName)} · {towerDefenseCharacterEventPacketSpawn.percentage:P0}", new Color(0.55f, 0.9f, 0.45f), flyToTarget: false);
																					}
																				}
																				else
																				{
																					ShowEventEffect(text + "创建卡片 " + EmptyValue(towerDefenseCharacterEventPacketCreate.packetName), new Color(0.55f, 0.9f, 0.45f), flyToTarget: false);
																				}
																			}
																			else
																			{
																				ShowEventEffect($"{text}金色碎片 ×{towerDefenseCharacterEventGoldShardCreate.num}", new Color(1f, 0.72f, 0.12f), flyToTarget: false);
																			}
																		}
																		else
																		{
																			ShowEventEffect($"{text}银币 +{towerDefenseCharacterEventYBCreate.num:0.##}", new Color(0.8f, 0.88f, 0.95f), flyToTarget: false);
																		}
																	}
																	else
																	{
																		ShowEventEffect($"{text}金币 +{towerDefenseCharacterEventCoinCreate.num:0.##}", new Color(1f, 0.75f, 0.15f), flyToTarget: false);
																	}
																}
																else
																{
																	ShowEventEffect($"{text}阳光 +{towerDefenseCharacterEventSunCreate.num:0.##}", new Color(1f, 0.88f, 0.2f), flyToTarget: false);
																}
															}
															else
															{
																ShowEventEffect(text + "雪球投射物", new Color(0.75f, 0.92f, 1f), flyToTarget: true);
															}
														}
														else
														{
															ShowEventEffect($"{text}元数据投射物 · 速度 {towerDefenseCharacterEventExplodeProjectileFromMetaData.speed:0.##}", new Color(1f, 0.56f, 0.25f), flyToTarget: true);
														}
													}
													else
													{
														ShowEventEffect($"{text}散射投射物 ×{towerDefenseCharacterEventProjectileCreate.createNum}", new Color(0.42f, 0.95f, 0.38f), flyToTarget: true);
													}
												}
												else
												{
													ShowEventEffect($"{text}投射物飞行 · 速度 {towerDefenseCharacterEventCreateProjectile.speed:0.##}", new Color(0.42f, 0.95f, 0.38f), flyToTarget: true);
												}
											}
											else
											{
												ResetBuffVisual();
												ShowEventEffect(text + "清除角色状态", new Color(0.55f, 1f, 0.82f), flyToTarget: false);
											}
										}
										else
										{
											AnimateTargetDestroy(text + "销毁目标");
										}
									}
									else
									{
										AnimateTargetShift(90f, text + "大蒜换行");
									}
								}
								else
								{
									AnimateTargetShift((float)(Math.Max(1.0, towerDefenseCharacterEventHitBack.length) * 70.0), $"{text}击退 {towerDefenseCharacterEventHitBack.length:0.##} 格");
								}
							}
							else
							{
								ShowEventEffect($"{text}生命低于 {towerDefenseCharacterEventConditionHitpointBelow.percentage:P0} · {towerDefenseCharacterEventConditionHitpointBelow.eventList?.Count ?? 0} 个后续事件", new Color(1f, 0.55f, 0.35f), flyToTarget: false);
							}
						}
						else
						{
							ShowEventEffect($"{text}随机门 {towerDefenseCharacterEventConditionRandom.percentage:P0} · {towerDefenseCharacterEventConditionRandom.eventList?.Count ?? 0} 个后续事件", new Color(0.8f, 0.62f, 1f), flyToTarget: false);
						}
					}
					else
					{
						SimulateBuff(new TowerDefenseCharacterBuffHypnoses
						{
							time = towerDefenseCharacterEventHypnoses.time
						});
					}
				}
				else
				{
					SimulateBuff(new TowerDefenseCharacterBuffIceSpeedDown
					{
						time = towerDefenseCharacterEventIceSpeedDown.time
					});
				}
			}
			else
			{
				SimulateBuff(new TowerDefenseCharacterBuffFrozen
				{
					time = towerDefenseCharacterEventForzen.time,
					iceSpeedDownTime = towerDefenseCharacterEventForzen.iceSpeedDownTime
				});
			}
			return;
		}
		foreach (TowerDefenseCharacterBuffConfig buff in towerDefenseCharacterEventAddBuff.buffList)
		{
			if (GodotObject.IsInstanceValid(buff))
			{
				SimulateBuff(buff);
				_simulationText.Text = text + _simulationText.Text;
				return;
			}
		}
		ShowEventEffect(text + "Buff 列表为空", new Color(0.7f, 0.7f, 0.7f), flyToTarget: false);
	}

	private void AnimateTargetShift(float distance, string description)
	{
		ShowEventEffect(description, new Color(0.9f, 0.82f, 0.35f), flyToTarget: false);
		if (_targetSpriteInstance is Node2D node2D)
		{
			_eventTween?.Kill();
			node2D.Position = Vector2.Zero;
			_eventTween = CreateTween();
			_eventTween.TweenProperty(node2D, "position:x", distance, 0.35).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
			_eventTween.TweenInterval(0.25);
			_eventTween.TweenProperty(node2D, "position:x", 0.0, 0.35).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.InOut);
		}
	}

	private void AnimateTargetDestroy(string description)
	{
		ShowEventEffect(description, new Color(1f, 0.42f, 0.25f), flyToTarget: false);
		_eventTween?.Kill();
		SetTargetOpacity(_targetSpriteInstance, 1f);
		CanvasItem canvasItem = FindFirstCanvasItem(_targetSpriteInstance);
		if (GodotObject.IsInstanceValid(canvasItem))
		{
			_eventTween = CreateTween();
			_eventTween.TweenProperty(canvasItem, "modulate:a", 0.0, 0.55);
		}
	}

	private void ShowEventEffect(string text, Color color, bool flyToTarget)
	{
		if (!GodotObject.IsInstanceValid(_eventEffect))
		{
			return;
		}
		_effectTween?.Kill();
		_eventEffect.Visible = true;
		_eventEffect.Text = text;
		_eventEffect.Modulate = color;
		_eventEffect.Position = (flyToTarget ? new Vector2(80f, 205f) : new Vector2(265f, 205f));
		_simulationText.Text = text;
		_effectTween = CreateTween();
		_effectTween.SetParallel();
		_effectTween.TweenProperty(_eventEffect, "position:x", flyToTarget ? 470.0 : 265.0, 0.6).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		_effectTween.TweenProperty(_eventEffect, "modulate:a", 0.0, 0.9).SetDelay(0.45);
		_effectTween.Chain().TweenCallback(Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(_eventEffect))
			{
				_eventEffect.Visible = false;
			}
		}));
	}

	private void ResetEventVisual()
	{
		_eventTween?.Kill();
		_effectTween?.Kill();
		_eventTween = null;
		_effectTween = null;
		if (GodotObject.IsInstanceValid(_eventEffect))
		{
			_eventEffect.Visible = false;
		}
		if (_targetSpriteInstance is Node2D node2D)
		{
			node2D.Position = Vector2.Zero;
		}
		SetTargetOpacity(_targetSpriteInstance, 1f);
	}

	private static void SetTargetOpacity(Node node, float opacity)
	{
		CanvasItem canvasItem = FindFirstCanvasItem(node);
		if (GodotObject.IsInstanceValid(canvasItem))
		{
			Color modulate = canvasItem.Modulate;
			modulate.A = opacity;
			canvasItem.Modulate = modulate;
		}
	}

	private static CanvasItem FindFirstCanvasItem(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return null;
		}
		if (node is CanvasItem result)
		{
			return result;
		}
		foreach (Node child in node.GetChildren())
		{
			CanvasItem canvasItem = FindFirstCanvasItem(child);
			if (GodotObject.IsInstanceValid(canvasItem))
			{
				return canvasItem;
			}
		}
		return null;
	}

	private void UpdateTargetHud()
	{
		if (GodotObject.IsInstanceValid(_targetHealth))
		{
			_targetHealth.MaxValue = 1000.0;
			_targetHealth.Value = _currentHealth;
			OptionButton targetMode = _targetMode;
			bool flag = targetMode != null && targetMode.Selected == 1;
			_targetName.Text = (flag ? "普通僵尸 · 盾牌 / 护甲" : "普通僵尸 · 身体");
			_healthValue.Text = $"{_currentHealth:0.##} / {1000.0:0}";
		}
	}

	private void ShowSimulationMessage(string description, string damageText, bool hit)
	{
		_simulationText.Text = description;
		_damageTween?.Kill();
		_damageNumber.Visible = true;
		_damageNumber.Text = damageText;
		_damageNumber.Modulate = (hit ? Colors.White : new Color(0.75f, 0.85f, 1f));
		_damageNumber.Position = new Vector2(430f, 105f);
		_damageTween = CreateTween();
		_damageTween.SetParallel();
		_damageTween.TweenProperty(_damageNumber, "position:y", 62.0, 0.55).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		_damageTween.TweenProperty(_damageNumber, "modulate:a", 0.0, 0.8).SetDelay(0.2);
		_damageTween.Chain().TweenCallback(Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(_damageNumber))
			{
				_damageNumber.Visible = false;
			}
		}));
	}

	private static bool TryBuildDamageProfile(Resource resource, out DamageProfile profile)
	{
		profile = default;
		if (!(resource is AttackConfig attack))
		{
			if (!(resource is TowerDefenseCharacterEventHurtWithConfig towerDefenseCharacterEventHurtWithConfig))
			{
				if (resource is TowerDefenseCharacterEventHurt towerDefenseCharacterEventHurt)
				{
					profile = new DamageProfile(towerDefenseCharacterEventHurt.num, 1.0, 1.0, towerDefenseCharacterEventHurt.damageFlags, towerDefenseCharacterEventHurt.collisionFlags, "直接伤害事件");
					return true;
				}
				if (resource is TowerDefenseCharacterEventExplodeHurt towerDefenseCharacterEventExplodeHurt)
				{
					profile = DefaultEventDamage(towerDefenseCharacterEventExplodeHurt.num, towerDefenseCharacterEventExplodeHurt.type + " 爆炸");
					return true;
				}
				if (resource is TowerDefenseCharacterEventSmashHurt towerDefenseCharacterEventSmashHurt)
				{
					profile = DefaultEventDamage(towerDefenseCharacterEventSmashHurt.num, "砸击伤害");
					return true;
				}
				if (resource is TowerDefenseCharacterEventBowlingHurt towerDefenseCharacterEventBowlingHurt)
				{
					profile = DefaultEventDamage(towerDefenseCharacterEventBowlingHurt.num, "保龄球伤害 · " + towerDefenseCharacterEventBowlingHurt.dir);
					return true;
				}
				if (resource is TowerDefenseCharacterEventJala towerDefenseCharacterEventJala)
				{
					profile = DefaultEventDamage(towerDefenseCharacterEventJala.num, "辣椒全行伤害");
					return true;
				}
			}
			else if (GodotObject.IsInstanceValid(towerDefenseCharacterEventHurtWithConfig.AttackConfig))
			{
				profile = FromAttackConfig(towerDefenseCharacterEventHurtWithConfig.AttackConfig, "配置伤害事件");
				return true;
			}
			return false;
		}
		profile = FromAttackConfig(attack, "AttackConfig");
		return true;
	}

	private static bool TryBuildBuffProfile(TowerDefenseCharacterBuffConfig buff, out BuffProfile profile)
	{
		profile = default;
		if (!GodotObject.IsInstanceValid(buff))
		{
			return false;
		}
		double duration = ReadDoubleProperty(buff, "time", -1.0);
		BuffProfile buffProfile;
		if (buff is TowerDefenseCharacterBuffFrozen)
		{
			buffProfile = new BuffProfile("冻结", duration, new Color(0.55f, 0.82f, 1f), "冻结目标并在结束后附加减速");
		}
		else if (buff is TowerDefenseCharacterBuffIceSpeedDown)
		{
			buffProfile = new BuffProfile("冰冻减速", duration, new Color(0.62f, 0.88f, 1f), "降低目标行动与动画速度");
		}
		else if (buff is TowerDefenseCharacterBuffButter)
		{
			buffProfile = new BuffProfile("黄油定身", duration, new Color(1f, 0.84f, 0.3f), "使目标暂时无法行动");
		}
		else if (buff is TowerDefenseCharacterBuffBurn)
		{
			buffProfile = new BuffProfile("燃烧", duration, new Color(1f, 0.48f, 0.2f), "持续造成火焰伤害");
		}
		else if (buff is TowerDefenseCharacterBuffPoisoning)
		{
			buffProfile = new BuffProfile("中毒", duration, new Color(0.52f, 0.9f, 0.32f), "持续累积毒素伤害");
		}
		else if (buff is TowerDefenseCharacterBuffHypnoses)
		{
			buffProfile = new BuffProfile("魅惑", duration, new Color(1f, 0.48f, 0.86f), "改变角色阵营及后续联动");
		}
		else if (buff is TowerDefenseCharacterBuffSleep)
		{
			buffProfile = new BuffProfile("睡眠", duration, new Color(0.62f, 0.65f, 0.82f), "暂停角色主动行为");
		}
		else if (buff is TowerDefenseCharacterBuffDizziness)
		{
			buffProfile = new BuffProfile("眩晕", duration, new Color(0.78f, 0.6f, 1f), "短时间打断角色行动");
		}
		else if (buff is TowerDefenseCharacterBuffRedHeat)
		{
			buffProfile = new BuffProfile("红温", duration, new Color(1f, 0.28f, 0.2f), "进入高温强化状态");
		}
		else if (buff is TowerDefenseCharacterBuffFluorescence)
		{
			buffProfile = new BuffProfile("荧光", duration, new Color(0.35f, 1f, 0.9f), "附加荧光标记");
		}
		else if (buff is TowerDefenseCharacterBuffSquid)
		{
			buffProfile = new BuffProfile("墨汁", duration, new Color(0.38f, 0.45f, 0.62f), "墨汁遮挡与限制效果");
		}
		else if (buff is TowerDefenseCharacterBuffCherry)
		{
			buffProfile = new BuffProfile("樱桃冲击", duration, new Color(1f, 0.36f, 0.38f), "爆炸冲击后的短时状态");
		}
		else
		{
			buffProfile = ((!(buff is TowerDefenseCharacterBuffCoffee)) ? new BuffProfile(string.IsNullOrWhiteSpace(buff.key) ? buff.GetType().Name.Replace("TowerDefenseCharacterBuff", "") : buff.key, duration, new Color(0.78f, 0.92f, 0.72f), $"refresh={buff.refresh}, canFliter={buff.canFliter}") : new BuffProfile("咖啡加速", duration, new Color(0.76f, 0.52f, 0.3f), "提高角色时间流速"));
		}
		profile = buffProfile;
		return true;
	}

	private static double ReadDoubleProperty(Resource resource, string name, double fallback)
	{
		bool flag = TryGetProperty(resource, name, out var value);
		if (flag)
		{
			Variant.Type variantType = value.VariantType;
			bool flag2 = (((ulong)(variantType - 2) <= 1uL) ? true : false);
			flag = flag2;
		}
		if (!flag)
		{
			return fallback;
		}
		return value.AsDouble();
	}

	private static string FormatBuffDuration(double duration)
	{
		if (!(duration < 0.0))
		{
			return $"持续 {duration:0.##} 秒（预览加速）";
		}
		return "永久 / 条件解除";
	}

	private static DamageProfile FromAttackConfig(AttackConfig attack, string label)
	{
		return new DamageProfile(attack.num, attack.attackScale, attack.armorAttackScale, attack.damageFlags, attack.collisionFlags, label);
	}

	private static DamageProfile DefaultEventDamage(double damage, string label)
	{
		return new DamageProfile(damage, 1.0, 1.0, 3, 1, label);
	}

	private static bool TryReadEditableProperty(Dictionary property, out string name, out Variant.Type type, out PropertyHint hint, out string hintString)
	{
		name = (property.ContainsKey("name") ? property["name"].AsString() : "");
		type = (Variant.Type)(property.ContainsKey("type") ? property["type"].AsInt32() : 0);
		hint = (PropertyHint)(property.ContainsKey("hint") ? property["hint"].AsInt32() : 0);
		hintString = (property.ContainsKey("hint_string") ? property["hint_string"].AsString() : "");
		if (((property.ContainsKey("usage") ? property["usage"].AsInt64() : 0) & 4) != 0 && !string.IsNullOrWhiteSpace(name))
		{
			return name != "script";
		}
		return false;
	}

	private static bool TryGetProperty(Resource resource, string name, out Variant value)
	{
		value = default;
		try
		{
			value = resource.Get(name);
			return true;
		}
		catch
		{
			return false;
		}
	}

	private void AddSummaryRows(Resource resource)
	{
		PreviewList?.AddItem("游戏靶场 -> " + resource.GetType().Name);
		if (resource is TowerDefenseCharacterBuffConfig buff && TryBuildBuffProfile(buff, out var profile))
		{
			TimelineList?.AddItem("Buff: " + profile.DisplayName + " · " + FormatBuffDuration(profile.Duration));
			GraphList?.AddItem("Buff 资源 -> 角色状态 -> 持续时间 -> 结束恢复");
			ReferenceList?.AddItem("Buff 预览靶子 -> ZombieNormal 游戏动画");
			return;
		}
		if (resource is TowerDefenseCharacterEventBase resource2)
		{
			TimelineList?.AddItem("角色事件: " + BuildEventDisplayName(resource2));
			GraphList?.AddItem("角色事件 -> 触发条件 -> 隔离效果预览 -> 子事件链");
			ReferenceList?.AddItem("事件目标 -> ZombieNormal 游戏动画");
		}
		else if (resource is TowerDefenseCharacterEventLuckyDrawItem towerDefenseCharacterEventLuckyDrawItem)
		{
			TimelineList?.AddItem($"奖池权重: {towerDefenseCharacterEventLuckyDrawItem.weight:0.###}");
			GraphList?.AddItem("奖池项 -> 权重 -> 子事件");
		}
		if (TryBuildDamageProfile(resource, out var profile2))
		{
			TimelineList?.AddItem($"基础伤害: {profile2.BaseDamage:0.###}");
			TimelineList?.AddItem($"身体 / 护甲倍率: {profile2.BodyScale:0.###} / {profile2.ArmorScale:0.###}");
			GraphList?.AddItem("攻击资源 -> 伤害标志 -> 碰撞标志 -> 身体/护甲结算");
		}
		ReferenceList?.AddItem("角色靶子 -> ZombieNormal 游戏动画");
	}

	private static string BuildResourceTypeLabel(Resource resource)
	{
		if (!(resource is TowerDefenseCharacterBuffConfig))
		{
			if (!(resource is AttackConfig))
			{
				if (!(resource is TowerDefenseCharacterEventExplodeHurt))
				{
					if (!(resource is TowerDefenseCharacterEventSmashHurt))
					{
						if (!(resource is TowerDefenseCharacterEventBowlingHurt))
						{
							if (!(resource is TowerDefenseCharacterEventHurtWithConfig))
							{
								if (!(resource is TowerDefenseCharacterEventHurt))
								{
									if (!(resource is TowerDefenseCharacterEventLuckyDrawItem))
									{
										if (resource is TowerDefenseCharacterEventBase)
										{
											return "角色事件";
										}
										return resource?.GetType().Name ?? "战斗资源";
									}
									return "事件奖池权重项";
								}
								return "直接伤害事件";
							}
							return "配置伤害事件";
						}
						return "保龄球伤害事件";
					}
					return "砸击伤害事件";
				}
				return "爆炸伤害事件";
			}
			return "攻击配置";
		}
		return "角色 Buff";
	}

	private static string BuildPropertyLabel(string name)
	{
		switch (name)
		{
		case "num":
			return "基础伤害  num";
		case "attackScale":
			return "身体伤害倍率  attackScale";
		case "armorAttackScale":
			return "护甲伤害倍率  armorAttackScale";
		case "Flag/Damage":
		case "damageFlags":
			return "伤害命中标志";
		case "Flag/Collision":
		case "collisionFlags":
			return "角色碰撞标志";
		case "playSplatAudio":
			return "播放命中音效";
		case "burns":
			return "附带燃烧效果";
		case "dir":
			return "击退方向";
		case "type":
			return "爆炸类型";
		case "AttackConfig":
			return "内嵌攻击配置";
		case "_event":
			return "奖池子事件";
		case "weight":
			return "奖池权重";
		default:
			return name;
		}
	}

	private static string BuildPropertyNodeSuffix(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return "Property";
		}
		char[] array = name.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (!char.IsLetterOrDigit(array[i]) && array[i] != '_')
			{
				array[i] = '_';
			}
		}
		return new string(array);
	}

	private static List<ArrayResourceChoice> BuildArrayResourceChoices(string propertyName, string hintString)
	{
		List<ArrayResourceChoice> list = new List<ArrayResourceChoice>();
		string text = hintString + " " + propertyName;
		if (text.Contains("TowerDefenseCharacterEventLuckyDrawItem", StringComparison.OrdinalIgnoreCase))
		{
			list.Add(new ArrayResourceChoice("奖池权重项", () => new TowerDefenseCharacterEventLuckyDrawItem()));
			return list;
		}
		if (text.Contains("TowerDefenseCharacterBuffConfig", StringComparison.OrdinalIgnoreCase) || text.Contains("buffList", StringComparison.OrdinalIgnoreCase))
		{
			list.Add(new ArrayResourceChoice("冻结", () => new TowerDefenseCharacterBuffFrozen()));
			list.Add(new ArrayResourceChoice("燃烧", () => new TowerDefenseCharacterBuffBurn()));
			list.Add(new ArrayResourceChoice("冰冻减速", () => new TowerDefenseCharacterBuffIceSpeedDown()));
			list.Add(new ArrayResourceChoice("中毒", () => new TowerDefenseCharacterBuffPoisoning()));
			list.Add(new ArrayResourceChoice("魅惑", () => new TowerDefenseCharacterBuffHypnoses()));
			list.Add(new ArrayResourceChoice("黄油", () => new TowerDefenseCharacterBuffButter()));
			return list;
		}
		if (!text.Contains("Event", StringComparison.OrdinalIgnoreCase))
		{
			return list;
		}
		list.Add(new ArrayResourceChoice("直接伤害", () => new TowerDefenseCharacterEventHurt()));
		list.Add(new ArrayResourceChoice("配置伤害", () => new TowerDefenseCharacterEventHurtWithConfig()));
		list.Add(new ArrayResourceChoice("添加 Buff", () => new TowerDefenseCharacterEventAddBuff()));
		list.Add(new ArrayResourceChoice("冻结", () => new TowerDefenseCharacterEventForzen()));
		list.Add(new ArrayResourceChoice("冰冻减速", () => new TowerDefenseCharacterEventIceSpeedDown()));
		list.Add(new ArrayResourceChoice("魅惑", () => new TowerDefenseCharacterEventHypnoses()));
		list.Add(new ArrayResourceChoice("击退", () => new TowerDefenseCharacterEventHitBack()));
		list.Add(new ArrayResourceChoice("创建投射物", () => new TowerDefenseCharacterEventCreateProjectile()));
		list.Add(new ArrayResourceChoice("创建阳光", () => new TowerDefenseCharacterEventSunCreate()));
		list.Add(new ArrayResourceChoice("生成角色", () => new TowerDefenseCharacterEventPacketSpawn()));
		list.Add(new ArrayResourceChoice("销毁", () => new TowerDefenseCharacterEventDestroy()));
		list.Add(new ArrayResourceChoice("净化", () => new TowerDefenseCharacterEventPurify()));
		list.Add(new ArrayResourceChoice("随机条件", () => new TowerDefenseCharacterEventConditionRandom()));
		list.Add(new ArrayResourceChoice("生命条件", () => new TowerDefenseCharacterEventConditionHitpointBelow()));
		return list;
	}

	private static string BuildEventDisplayName(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "空事件";
		}
		string text = resource.GetType().Name.Replace("TowerDefenseCharacterEvent", "");
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return resource.GetType().Name;
	}

	private static string EmptyValue(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return "未配置";
	}

	private static string[] SplitHintItems(string hintString)
	{
		if (string.IsNullOrWhiteSpace(hintString))
		{
			return System.Array.Empty<string>();
		}
		string[] array = hintString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		for (int i = 0; i < array.Length; i++)
		{
			int num = array[i].IndexOf(':');
			if (num > 0)
			{
				array[i] = array[i].Substring(0, num);
			}
		}
		return array;
	}

	private static void UpdateFlagsText(MenuButton menu, int value, IReadOnlyList<string> flags)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < flags.Count; i++)
		{
			if ((value & (1 << i)) != 0)
			{
				list.Add(flags[i]);
			}
		}
		menu.Text = ((list.Count == 0) ? "未选择" : string.Join(" + ", list));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(64)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCompleteCharacterCombatVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanDirectlyEditProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "workbench", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildTargetPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildVisualPropertyRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateVisualPropertyEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateNumberEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "integer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateTextEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateVector2Editor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateVectorComponent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateVector4IEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector4I, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateIntegerVectorComponent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddVectorComponent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "row", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEventArrayEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "array", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddArrayResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "items", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.Object, "summary", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false)
			}, null),
			new MethodInfo(MethodName.MoveArrayResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "items", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.Object, "summary", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveArrayResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "items", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.Object, "summary", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenArrayResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "items", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false)
			}, null),
			new MethodInfo(MethodName.CommitArrayProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "array", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentArrayProperty, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneArrayForHistory, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshArrayEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "array", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "items", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.Object, "summary", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedArrayIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "items", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.Array, "array", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatArrayResource, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateIntEnumEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateStringEnumEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WrapSmallSegmentedOption, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false)
			}, null),
			new MethodInfo(MethodName.MountSmallSegmentedOption, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureVisualChoicePicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeVisualChoices, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateFlagsEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateResourceEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "value", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateReadOnlySummary, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetVisualProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.OnVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueCharacterCombatEditorRefreshFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCharacterCombatEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SimulateCurrentResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetSimulation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshSimulationSummary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SimulateBuff, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "buff", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResetBuffVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteBuffPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetTargetTint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Color, "tint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SimulateCharacterEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "characterEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "prefix", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimateTargetShift, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "distance", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "description", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimateTargetDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "description", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowEventEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "flyToTarget", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetEventVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetTargetOpacity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Float, "opacity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindFirstCanvasItem, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CanvasItem"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateTargetHud, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowSimulationMessage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "description", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "damageText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadDoubleProperty, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatBuffDuration, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildResourceTypeLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPropertyLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPropertyNodeSuffix, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildEventDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EmptyValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SplitHintItems, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.HasCompleteCharacterCombatVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteCharacterCombatVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.CanDirectlyEditProperty && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(CanDirectlyEditProperty(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.BindWorkbench && args.Count == 1)
		{
			BindWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTargetPreview && args.Count == 0)
		{
			BuildTargetPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildVisualPropertyRows && args.Count == 1)
		{
			BuildVisualPropertyRows(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateVisualPropertyEditor && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateVisualPropertyEditor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant.Type>(in args[1]), VariantUtils.ConvertTo<PropertyHint>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4])));
			return true;
		}
		if (method == MethodName.CreateNumberEditor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateNumberEditor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateTextEditor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateTextEditor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateVector2Editor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateVector2Editor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateVectorComponent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateVectorComponent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateVector4IEditor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateVector4IEditor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector4I>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateIntegerVectorComponent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateIntegerVectorComponent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.AddVectorComponent && args.Count == 3)
		{
			AddVectorComponent(VariantUtils.ConvertTo<HBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateEventArrayEditor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateEventArrayEditor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.AddArrayResource && args.Count == 4)
		{
			AddArrayResource(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<ItemList>(in args[2]), VariantUtils.ConvertTo<Label>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveArrayResource && args.Count == 4)
		{
			MoveArrayResource(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<ItemList>(in args[1]), VariantUtils.ConvertTo<Label>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveArrayResource && args.Count == 3)
		{
			RemoveArrayResource(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<ItemList>(in args[1]), VariantUtils.ConvertTo<Label>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenArrayResource && args.Count == 2)
		{
			OpenArrayResource(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<ItemList>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitArrayProperty && args.Count == 2)
		{
			CommitArrayProperty(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentArrayProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCurrentArrayProperty(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CloneArrayForHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(CloneArrayForHistory(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshArrayEditor && args.Count == 3)
		{
			RefreshArrayEditor(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<ItemList>(in args[1]), VariantUtils.ConvertTo<Label>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedArrayIndex && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetSelectedArrayIndex(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[1])));
			return true;
		}
		if (method == MethodName.FormatArrayResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatArrayResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateIntEnumEditor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateIntEnumEditor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateStringEnumEditor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateStringEnumEditor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.WrapSmallSegmentedOption && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(WrapSmallSegmentedOption(VariantUtils.ConvertTo<OptionButton>(in args[0])));
			return true;
		}
		if (method == MethodName.MountSmallSegmentedOption && args.Count == 2)
		{
			MountSmallSegmentedOption(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<HFlowContainer>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureVisualChoicePicker && args.Count == 0)
		{
			EnsureVisualChoicePicker();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeVisualChoices && args.Count == 0)
		{
			DisposeVisualChoices();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateFlagsEditor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateFlagsEditor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateResourceEditor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateResourceEditor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateReadOnlySummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateReadOnlySummary(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetVisualProperty && args.Count == 2)
		{
			SetVisualProperty(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualPropertyEdited && args.Count == 1)
		{
			OnVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueCharacterCombatEditorRefreshFromHistory && args.Count == 0)
		{
			QueueCharacterCombatEditorRefreshFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCharacterCombatEditorFromHistory && args.Count == 0)
		{
			RefreshCharacterCombatEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.SimulateCurrentResource && args.Count == 0)
		{
			SimulateCurrentResource();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetSimulation && args.Count == 0)
		{
			ResetSimulation();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSimulationSummary && args.Count == 0)
		{
			RefreshSimulationSummary();
			ret = default;
			return true;
		}
		if (method == MethodName.SimulateBuff && args.Count == 1)
		{
			SimulateBuff(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetBuffVisual && args.Count == 0)
		{
			ResetBuffVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteBuffPreview && args.Count == 0)
		{
			CompleteBuffPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.SetTargetTint && args.Count == 2)
		{
			SetTargetTint(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SimulateCharacterEvent && args.Count == 2)
		{
			SimulateCharacterEvent(VariantUtils.ConvertTo<TowerDefenseCharacterEventBase>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimateTargetShift && args.Count == 2)
		{
			AnimateTargetShift(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimateTargetDestroy && args.Count == 1)
		{
			AnimateTargetDestroy(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowEventEffect && args.Count == 3)
		{
			ShowEventEffect(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetEventVisual && args.Count == 0)
		{
			ResetEventVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.SetTargetOpacity && args.Count == 2)
		{
			SetTargetOpacity(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindFirstCanvasItem && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CanvasItem>(FindFirstCanvasItem(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateTargetHud && args.Count == 0)
		{
			UpdateTargetHud();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowSimulationMessage && args.Count == 3)
		{
			ShowSimulationMessage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadDoubleProperty && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadDoubleProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.FormatBuffDuration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatBuffDuration(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 1)
		{
			AddSummaryRows(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildResourceTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildResourceTypeLabel(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPropertyLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPropertyLabel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPropertyNodeSuffix && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPropertyNodeSuffix(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildEventDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildEventDisplayName(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.EmptyValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SplitHintItems && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(SplitHintItems(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasCompleteCharacterCombatVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteCharacterCombatVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.CanDirectlyEditProperty && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(CanDirectlyEditProperty(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateVectorComponent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateVectorComponent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateIntegerVectorComponent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateIntegerVectorComponent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.AddVectorComponent && args.Count == 3)
		{
			AddVectorComponent(VariantUtils.ConvertTo<HBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloneArrayForHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(CloneArrayForHistory(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshArrayEditor && args.Count == 3)
		{
			RefreshArrayEditor(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<ItemList>(in args[1]), VariantUtils.ConvertTo<Label>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedArrayIndex && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetSelectedArrayIndex(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[1])));
			return true;
		}
		if (method == MethodName.FormatArrayResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatArrayResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateReadOnlySummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateReadOnlySummary(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetTargetTint && args.Count == 2)
		{
			SetTargetTint(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTargetOpacity && args.Count == 2)
		{
			SetTargetOpacity(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindFirstCanvasItem && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CanvasItem>(FindFirstCanvasItem(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadDoubleProperty && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadDoubleProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.FormatBuffDuration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatBuffDuration(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildResourceTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildResourceTypeLabel(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPropertyLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPropertyLabel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPropertyNodeSuffix && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPropertyNodeSuffix(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildEventDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildEventDisplayName(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.EmptyValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SplitHintItems && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(SplitHintItems(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.HasCompleteCharacterCombatVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.CanDirectlyEditProperty)
		{
			return true;
		}
		if (method == MethodName.BindWorkbench)
		{
			return true;
		}
		if (method == MethodName.BuildTargetPreview)
		{
			return true;
		}
		if (method == MethodName.BuildVisualPropertyRows)
		{
			return true;
		}
		if (method == MethodName.CreateVisualPropertyEditor)
		{
			return true;
		}
		if (method == MethodName.CreateNumberEditor)
		{
			return true;
		}
		if (method == MethodName.CreateTextEditor)
		{
			return true;
		}
		if (method == MethodName.CreateVector2Editor)
		{
			return true;
		}
		if (method == MethodName.CreateVectorComponent)
		{
			return true;
		}
		if (method == MethodName.CreateVector4IEditor)
		{
			return true;
		}
		if (method == MethodName.CreateIntegerVectorComponent)
		{
			return true;
		}
		if (method == MethodName.AddVectorComponent)
		{
			return true;
		}
		if (method == MethodName.CreateEventArrayEditor)
		{
			return true;
		}
		if (method == MethodName.AddArrayResource)
		{
			return true;
		}
		if (method == MethodName.MoveArrayResource)
		{
			return true;
		}
		if (method == MethodName.RemoveArrayResource)
		{
			return true;
		}
		if (method == MethodName.OpenArrayResource)
		{
			return true;
		}
		if (method == MethodName.CommitArrayProperty)
		{
			return true;
		}
		if (method == MethodName.GetCurrentArrayProperty)
		{
			return true;
		}
		if (method == MethodName.CloneArrayForHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshArrayEditor)
		{
			return true;
		}
		if (method == MethodName.GetSelectedArrayIndex)
		{
			return true;
		}
		if (method == MethodName.FormatArrayResource)
		{
			return true;
		}
		if (method == MethodName.CreateIntEnumEditor)
		{
			return true;
		}
		if (method == MethodName.CreateStringEnumEditor)
		{
			return true;
		}
		if (method == MethodName.WrapSmallSegmentedOption)
		{
			return true;
		}
		if (method == MethodName.MountSmallSegmentedOption)
		{
			return true;
		}
		if (method == MethodName.EnsureVisualChoicePicker)
		{
			return true;
		}
		if (method == MethodName.DisposeVisualChoices)
		{
			return true;
		}
		if (method == MethodName.CreateFlagsEditor)
		{
			return true;
		}
		if (method == MethodName.CreateResourceEditor)
		{
			return true;
		}
		if (method == MethodName.CreateReadOnlySummary)
		{
			return true;
		}
		if (method == MethodName.SetVisualProperty)
		{
			return true;
		}
		if (method == MethodName.OnVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.QueueCharacterCombatEditorRefreshFromHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshCharacterCombatEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.SimulateCurrentResource)
		{
			return true;
		}
		if (method == MethodName.ResetSimulation)
		{
			return true;
		}
		if (method == MethodName.RefreshSimulationSummary)
		{
			return true;
		}
		if (method == MethodName.SimulateBuff)
		{
			return true;
		}
		if (method == MethodName.ResetBuffVisual)
		{
			return true;
		}
		if (method == MethodName.CompleteBuffPreview)
		{
			return true;
		}
		if (method == MethodName.SetTargetTint)
		{
			return true;
		}
		if (method == MethodName.SimulateCharacterEvent)
		{
			return true;
		}
		if (method == MethodName.AnimateTargetShift)
		{
			return true;
		}
		if (method == MethodName.AnimateTargetDestroy)
		{
			return true;
		}
		if (method == MethodName.ShowEventEffect)
		{
			return true;
		}
		if (method == MethodName.ResetEventVisual)
		{
			return true;
		}
		if (method == MethodName.SetTargetOpacity)
		{
			return true;
		}
		if (method == MethodName.FindFirstCanvasItem)
		{
			return true;
		}
		if (method == MethodName.UpdateTargetHud)
		{
			return true;
		}
		if (method == MethodName.ShowSimulationMessage)
		{
			return true;
		}
		if (method == MethodName.ReadDoubleProperty)
		{
			return true;
		}
		if (method == MethodName.FormatBuffDuration)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.BuildResourceTypeLabel)
		{
			return true;
		}
		if (method == MethodName.BuildPropertyLabel)
		{
			return true;
		}
		if (method == MethodName.BuildPropertyNodeSuffix)
		{
			return true;
		}
		if (method == MethodName.BuildEventDisplayName)
		{
			return true;
		}
		if (method == MethodName.EmptyValue)
		{
			return true;
		}
		if (method == MethodName.SplitHintItems)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._previewRoot)
		{
			_previewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._previewFallback)
		{
			_previewFallback = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._resourceTypeLabel)
		{
			_resourceTypeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._targetMode)
		{
			_targetMode = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._targetHealth)
		{
			_targetHealth = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._targetName)
		{
			_targetName = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._healthValue)
		{
			_healthValue = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._damageNumber)
		{
			_damageNumber = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._simulationText)
		{
			_simulationText = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._eventEffect)
		{
			_eventEffect = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._stateBadge)
		{
			_stateBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._stateTimer)
		{
			_stateTimer = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._propertyRows)
		{
			_propertyRows = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._propertyCount)
		{
			_propertyCount = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._damageTween)
		{
			_damageTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._buffTween)
		{
			_buffTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._eventTween)
		{
			_eventTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._effectTween)
		{
			_effectTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._targetSpriteInstance)
		{
			_targetSpriteInstance = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._simulateButton)
		{
			_simulateButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._currentHealth)
		{
			_currentHealth = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._updatingVisualProperty)
		{
			_updatingVisualProperty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._combatRefreshQueued)
		{
			_combatRefreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._visualChoicePicker)
		{
			_visualChoicePicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._previewRoot)
		{
			value = VariantUtils.CreateFrom(in _previewRoot);
			return true;
		}
		if (name == PropertyName._previewFallback)
		{
			value = VariantUtils.CreateFrom(in _previewFallback);
			return true;
		}
		if (name == PropertyName._resourceTypeLabel)
		{
			value = VariantUtils.CreateFrom(in _resourceTypeLabel);
			return true;
		}
		if (name == PropertyName._targetMode)
		{
			value = VariantUtils.CreateFrom(in _targetMode);
			return true;
		}
		if (name == PropertyName._targetHealth)
		{
			value = VariantUtils.CreateFrom(in _targetHealth);
			return true;
		}
		if (name == PropertyName._targetName)
		{
			value = VariantUtils.CreateFrom(in _targetName);
			return true;
		}
		if (name == PropertyName._healthValue)
		{
			value = VariantUtils.CreateFrom(in _healthValue);
			return true;
		}
		if (name == PropertyName._damageNumber)
		{
			value = VariantUtils.CreateFrom(in _damageNumber);
			return true;
		}
		if (name == PropertyName._simulationText)
		{
			value = VariantUtils.CreateFrom(in _simulationText);
			return true;
		}
		if (name == PropertyName._eventEffect)
		{
			value = VariantUtils.CreateFrom(in _eventEffect);
			return true;
		}
		if (name == PropertyName._stateBadge)
		{
			value = VariantUtils.CreateFrom(in _stateBadge);
			return true;
		}
		if (name == PropertyName._stateTimer)
		{
			value = VariantUtils.CreateFrom(in _stateTimer);
			return true;
		}
		if (name == PropertyName._propertyRows)
		{
			value = VariantUtils.CreateFrom(in _propertyRows);
			return true;
		}
		if (name == PropertyName._propertyCount)
		{
			value = VariantUtils.CreateFrom(in _propertyCount);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._damageTween)
		{
			value = VariantUtils.CreateFrom(in _damageTween);
			return true;
		}
		if (name == PropertyName._buffTween)
		{
			value = VariantUtils.CreateFrom(in _buffTween);
			return true;
		}
		if (name == PropertyName._eventTween)
		{
			value = VariantUtils.CreateFrom(in _eventTween);
			return true;
		}
		if (name == PropertyName._effectTween)
		{
			value = VariantUtils.CreateFrom(in _effectTween);
			return true;
		}
		if (name == PropertyName._targetSpriteInstance)
		{
			value = VariantUtils.CreateFrom(in _targetSpriteInstance);
			return true;
		}
		if (name == PropertyName._simulateButton)
		{
			value = VariantUtils.CreateFrom(in _simulateButton);
			return true;
		}
		if (name == PropertyName._currentHealth)
		{
			value = VariantUtils.CreateFrom(in _currentHealth);
			return true;
		}
		if (name == PropertyName._updatingVisualProperty)
		{
			value = VariantUtils.CreateFrom(in _updatingVisualProperty);
			return true;
		}
		if (name == PropertyName._combatRefreshQueued)
		{
			value = VariantUtils.CreateFrom(in _combatRefreshQueued);
			return true;
		}
		if (name == PropertyName._visualChoicePicker)
		{
			value = VariantUtils.CreateFrom(in _visualChoicePicker);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._previewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewFallback, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceTypeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetHealth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._healthValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._damageNumber, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulationText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stateBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stateTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._propertyRows, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._propertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._damageTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._buffTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._effectTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetSpriteInstance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulateButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._currentHealth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingVisualProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._combatRefreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._visualChoicePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._previewRoot, Variant.From(in _previewRoot));
		info.AddProperty(PropertyName._previewFallback, Variant.From(in _previewFallback));
		info.AddProperty(PropertyName._resourceTypeLabel, Variant.From(in _resourceTypeLabel));
		info.AddProperty(PropertyName._targetMode, Variant.From(in _targetMode));
		info.AddProperty(PropertyName._targetHealth, Variant.From(in _targetHealth));
		info.AddProperty(PropertyName._targetName, Variant.From(in _targetName));
		info.AddProperty(PropertyName._healthValue, Variant.From(in _healthValue));
		info.AddProperty(PropertyName._damageNumber, Variant.From(in _damageNumber));
		info.AddProperty(PropertyName._simulationText, Variant.From(in _simulationText));
		info.AddProperty(PropertyName._eventEffect, Variant.From(in _eventEffect));
		info.AddProperty(PropertyName._stateBadge, Variant.From(in _stateBadge));
		info.AddProperty(PropertyName._stateTimer, Variant.From(in _stateTimer));
		info.AddProperty(PropertyName._propertyRows, Variant.From(in _propertyRows));
		info.AddProperty(PropertyName._propertyCount, Variant.From(in _propertyCount));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._damageTween, Variant.From(in _damageTween));
		info.AddProperty(PropertyName._buffTween, Variant.From(in _buffTween));
		info.AddProperty(PropertyName._eventTween, Variant.From(in _eventTween));
		info.AddProperty(PropertyName._effectTween, Variant.From(in _effectTween));
		info.AddProperty(PropertyName._targetSpriteInstance, Variant.From(in _targetSpriteInstance));
		info.AddProperty(PropertyName._simulateButton, Variant.From(in _simulateButton));
		info.AddProperty(PropertyName._currentHealth, Variant.From(in _currentHealth));
		info.AddProperty(PropertyName._updatingVisualProperty, Variant.From(in _updatingVisualProperty));
		info.AddProperty(PropertyName._combatRefreshQueued, Variant.From(in _combatRefreshQueued));
		info.AddProperty(PropertyName._visualChoicePicker, Variant.From(in _visualChoicePicker));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._previewRoot, out var value))
		{
			_previewRoot = value.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._previewFallback, out var value2))
		{
			_previewFallback = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resourceTypeLabel, out var value3))
		{
			_resourceTypeLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._targetMode, out var value4))
		{
			_targetMode = value4.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._targetHealth, out var value5))
		{
			_targetHealth = value5.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._targetName, out var value6))
		{
			_targetName = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._healthValue, out var value7))
		{
			_healthValue = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._damageNumber, out var value8))
		{
			_damageNumber = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._simulationText, out var value9))
		{
			_simulationText = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._eventEffect, out var value10))
		{
			_eventEffect = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._stateBadge, out var value11))
		{
			_stateBadge = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._stateTimer, out var value12))
		{
			_stateTimer = value12.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._propertyRows, out var value13))
		{
			_propertyRows = value13.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._propertyCount, out var value14))
		{
			_propertyCount = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value15))
		{
			_statusLabel = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._damageTween, out var value16))
		{
			_damageTween = value16.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._buffTween, out var value17))
		{
			_buffTween = value17.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._eventTween, out var value18))
		{
			_eventTween = value18.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._effectTween, out var value19))
		{
			_effectTween = value19.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._targetSpriteInstance, out var value20))
		{
			_targetSpriteInstance = value20.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._simulateButton, out var value21))
		{
			_simulateButton = value21.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._currentHealth, out var value22))
		{
			_currentHealth = value22.As<double>();
		}
		if (info.TryGetProperty(PropertyName._updatingVisualProperty, out var value23))
		{
			_updatingVisualProperty = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._combatRefreshQueued, out var value24))
		{
			_combatRefreshQueued = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._visualChoicePicker, out var value25))
		{
			_visualChoicePicker = value25.As<XWGameplayResourcePickerWindow>();
		}
	}
}
