using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/StateMachine/XWStateGuardVisualResourceEditor.cs")]
public class XWStateGuardVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName BindScene = "BindScene";

		public static readonly StringName RebuildEditor = "RebuildEditor";

		public static readonly StringName BuildReturnToOwnerButton = "BuildReturnToOwnerButton";

		public static readonly StringName BuildRuntimeGuardFields = "BuildRuntimeGuardFields";

		public static readonly StringName BuildRuntimeKindSelector = "BuildRuntimeKindSelector";

		public static readonly StringName SetRuntimeGuardKind = "SetRuntimeGuardKind";

		public static readonly StringName BuildRuntimeCallbackFields = "BuildRuntimeCallbackFields";

		public static readonly StringName SetRuntimeGuardCallbackKey = "SetRuntimeGuardCallbackKey";

		public static readonly StringName BuildRuntimeCompositeFields = "BuildRuntimeCompositeFields";

		public static readonly StringName BuildExpressionFields = "BuildExpressionFields";

		public static readonly StringName BuildStateActiveFields = "BuildStateActiveFields";

		public static readonly StringName BuildCompositeFields = "BuildCompositeFields";

		public static readonly StringName BuildNotFields = "BuildNotFields";

		public static readonly StringName BuildAddButtons = "BuildAddButtons";

		public static readonly StringName BuildRuntimeAddButtons = "BuildRuntimeAddButtons";

		public static readonly StringName AddRuntimeGuardCreationButton = "AddRuntimeGuardCreationButton";

		public static readonly StringName RefreshComposition = "RefreshComposition";

		public static readonly StringName CreateChildCard = "CreateChildCard";

		public static readonly StringName CreateRuntimeChildCard = "CreateRuntimeChildCard";

		public static readonly StringName AddRuntimeCompositeChild = "AddRuntimeCompositeChild";

		public static readonly StringName RemoveRuntimeCompositeChild = "RemoveRuntimeCompositeChild";

		public static readonly StringName MoveRuntimeCompositeChild = "MoveRuntimeCompositeChild";

		public static readonly StringName OpenRuntimeCompositeChild = "OpenRuntimeCompositeChild";

		public static readonly StringName RemoveCompositeChild = "RemoveCompositeChild";

		public static readonly StringName MoveCompositeChild = "MoveCompositeChild";

		public static readonly StringName OpenCompositeChild = "OpenCompositeChild";

		public static readonly StringName AddValueTypeButton = "AddValueTypeButton";

		public static readonly StringName BuildActualValueEditor = "BuildActualValueEditor";

		public static readonly StringName OnGuardPropertyEdited = "OnGuardPropertyEdited";

		public static readonly StringName RefreshGuardEditorFromHistory = "RefreshGuardEditorFromHistory";

		public static readonly StringName UpdatePreview = "UpdatePreview";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName DisposeBinding = "DisposeBinding";

		public static readonly StringName AddSectionTitle = "AddSectionTitle";

		public static readonly StringName AddNotice = "AddNotice";

		public static readonly StringName AddFieldLabel = "AddFieldLabel";

		public static readonly StringName AddLineEdit = "AddLineEdit";

		public static readonly StringName AddFlow = "AddFlow";

		public static readonly StringName CreateChoiceButton = "CreateChoiceButton";

		public static readonly StringName CreateNumber = "CreateNumber";

		public static readonly StringName CreateToolbarButton = "CreateToolbarButton";

		public static readonly StringName CreateEmptyCompositionLabel = "CreateEmptyCompositionLabel";

		public static readonly StringName GetGuardIcon = "GetGuardIcon";

		public static readonly StringName IsSupportedGuard = "IsSupportedGuard";

		public static readonly StringName IsComposite = "IsComposite";

		public static readonly StringName GetCompositeArray = "GetCompositeArray";

		public static readonly StringName GetCompositeCount = "GetCompositeCount";

		public static readonly StringName NormalizeValueType = "NormalizeValueType";

		public static readonly StringName GetGuardTypeLabel = "GetGuardTypeLabel";

		public static readonly StringName GetRuntimeGuardTypeLabel = "GetRuntimeGuardTypeLabel";

		public static readonly StringName IsRuntimeCompositeKind = "IsRuntimeCompositeKind";

		public static readonly StringName ClearChildren = "ClearChildren";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName EditingGuard = "EditingGuard";

		public static readonly StringName GuardPreviewCanvas = "GuardPreviewCanvas";

		public static readonly StringName CompositionItemCount = "CompositionItemCount";

		public static readonly StringName SaveGuardButton = "SaveGuardButton";

		public static readonly StringName _editingGuard = "_editingGuard";

		public static readonly StringName _fieldsHost = "_fieldsHost";

		public static readonly StringName _compositionList = "_compositionList";

		public static readonly StringName _addConditionFlow = "_addConditionFlow";

		public static readonly StringName _compositionCard = "_compositionCard";

		public static readonly StringName _previewCanvas = "_previewCanvas";

		public static readonly StringName _actualEditorHost = "_actualEditorHost";

		public static readonly StringName _resultBadge = "_resultBadge";

		public static readonly StringName _typeBadge = "_typeBadge";

		public static readonly StringName _resourceName = "_resourceName";

		public static readonly StringName _localToScene = "_localToScene";

		public static readonly StringName _saveGuardButton = "_saveGuardButton";

		public static readonly StringName _actualValue = "_actualValue";

		public static readonly StringName _rebuilding = "_rebuilding";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string WorkbenchScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/StateMachine/XWStateGuardWorkbench.tscn";

	private static PackedScene _workbenchScene;

	private static readonly (StateMachineComparisonOperator Value, string Glyph, string Hint)[] Operators = new (StateMachineComparisonOperator, string, string)[6]
	{
		(StateMachineComparisonOperator.Equal, "＝", "等于"),
		(StateMachineComparisonOperator.NotEqual, "≠", "不等于"),
		(StateMachineComparisonOperator.Less, "＜", "小于"),
		(StateMachineComparisonOperator.LessOrEqual, "≤", "小于等于"),
		(StateMachineComparisonOperator.Greater, "＞", "大于"),
		(StateMachineComparisonOperator.GreaterOrEqual, "≥", "大于等于")
	};

	private Resource _editingGuard;

	private XWVisualPropertyBinding _propertyBinding;

	private VBoxContainer _fieldsHost;

	private VBoxContainer _compositionList;

	private HFlowContainer _addConditionFlow;

	private PanelContainer _compositionCard;

	private XWStateGuardPreviewCanvas _previewCanvas;

	private HBoxContainer _actualEditorHost;

	private Label _resultBadge;

	private Label _typeBadge;

	private LineEdit _resourceName;

	private CheckButton _localToScene;

	private Button _saveGuardButton;

	private Variant _actualValue;

	private bool _rebuilding;

	private readonly System.Collections.Generic.Dictionary<ulong, XWResourceEditContext> _guardNavigationContexts = new System.Collections.Generic.Dictionary<ulong, XWResourceEditContext>();

	public Resource EditingGuard => _editingGuard;

	public XWStateGuardPreviewCanvas GuardPreviewCanvas => _previewCanvas;

	public int CompositionItemCount => GetCompositeCount(_editingGuard);

	public Button SaveGuardButton => _saveGuardButton;

	public override void _ExitTree()
	{
		DisposeBinding();
		base._ExitTree();
	}

	protected override bool ShouldBindInlineTextSurface(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		return false;
	}

	protected override bool ShouldBindDirectPropertySurface(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		return false;
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		return false;
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeBinding();
		if (IsSupportedGuard(CurrentResource) && CanvasGrid != null)
		{
			_editingGuard = CurrentResource;
			CanvasGrid.Columns = 1;
			if (_workbenchScene == null)
			{
				_workbenchScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/StateMachine/XWStateGuardWorkbench.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _workbenchScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindScene(vBoxContainer);
				RebuildEditor();
				AddSummaryRows();
			}
		}
	}

	private void BindScene(VBoxContainer root)
	{
		_fieldsHost = root.GetNode<VBoxContainer>("%FieldsHost");
		_compositionList = root.GetNode<VBoxContainer>("%CompositionList");
		_addConditionFlow = root.GetNode<HFlowContainer>("%AddConditionFlow");
		_compositionCard = root.GetNode<PanelContainer>("%CompositionCard");
		_previewCanvas = root.GetNode<XWStateGuardPreviewCanvas>("%PreviewCanvas");
		_actualEditorHost = root.GetNode<HBoxContainer>("%ActualEditorHost");
		_resultBadge = root.GetNode<Label>("%ResultBadge");
		_typeBadge = root.GetNode<Label>("%TypeBadge");
		_resourceName = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToScene = root.GetNode<CheckButton>("%LocalToSceneCheck");
		_saveGuardButton = root.GetNode<Button>("%SaveGuardButton");
		_saveGuardButton.Pressed += SaveCurrentResource;
	}

	private void RebuildEditor()
	{
		if (!GodotObject.IsInstanceValid(_editingGuard) || !GodotObject.IsInstanceValid(_fieldsHost))
		{
			return;
		}
		if (CurrentEditContext != null)
		{
			_guardNavigationContexts[_editingGuard.GetInstanceId()] = CurrentEditContext;
		}
		_rebuilding = true;
		try
		{
			DisposeBinding(clearGuard: false);
			ClearChildren(_fieldsHost);
			ClearChildren(_actualEditorHost);
			ClearChildren(_compositionList);
			ClearChildren(_addConditionFlow);
			_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnGuardPropertyEdited);
			_propertyBinding.BindText(_resourceName, _editingGuard, "resource_name", UpdatePreview, this, "RefreshGuardEditorFromHistory");
			_propertyBinding.BindToggle(_localToScene, _editingGuard, "resource_local_to_scene", UpdatePreview, this, "RefreshGuardEditorFromHistory");
			BuildReturnToOwnerButton();
			_typeBadge.Text = GetGuardTypeLabel(_editingGuard);
			Resource editingGuard = _editingGuard;
			if (!(editingGuard is StateMachineGuardDefinition stateMachineGuardDefinition))
			{
				if (!(editingGuard is ExpressionGuard guard))
				{
					if (!(editingGuard is StateIsActiveGuard guard2))
					{
						if (!(editingGuard is AllOfGuard allOfGuard))
						{
							if (!(editingGuard is AnyOfGuard anyOfGuard))
							{
								if (editingGuard is NotGuard guard3)
								{
									BuildNotFields(guard3);
								}
							}
							else
							{
								BuildCompositeFields(anyOfGuard, anyOfGuard.guards, "任一满足 · OR");
							}
						}
						else
						{
							BuildCompositeFields(allOfGuard, allOfGuard.guards, "全部满足 · AND");
						}
					}
					else
					{
						BuildStateActiveFields(guard2);
					}
				}
				else
				{
					BuildExpressionFields(guard);
				}
			}
			else
			{
				BuildRuntimeKindSelector(stateMachineGuardDefinition);
				if (stateMachineGuardDefinition.Kind == StateMachineGuardKind.ExpressionProperty)
				{
					BuildRuntimeGuardFields(stateMachineGuardDefinition);
				}
				else if (stateMachineGuardDefinition.Kind == StateMachineGuardKind.Callback)
				{
					BuildRuntimeCallbackFields(stateMachineGuardDefinition);
				}
				else
				{
					BuildRuntimeCompositeFields(stateMachineGuardDefinition);
				}
			}
			BuildActualValueEditor();
			RefreshComposition();
			UpdatePreview();
		}
		finally
		{
			_rebuilding = false;
		}
	}

	private void BuildReturnToOwnerButton()
	{
		Resource owner = CurrentEditContext?.OwnerResource;
		if (GodotObject.IsInstanceValid(owner) && owner != _editingGuard)
		{
			Button button = new Button
			{
				Name = "ReturnToParentGuard",
				Text = "返回上级条件",
				TooltipText = "返回包含当前条件的上级拼图",
				CustomMinimumSize = new Vector2(0f, 34f)
			};
			button.Pressed += () =>
			{
				XWResourceEditContext context = (_guardNavigationContexts.TryGetValue(owner.GetInstanceId(), out var value) ? value : XWResourceEditContext.ForNestedResourceView(owner, owner.ResourcePath, "state_guard_editor", CurrentEditContext?.IsBuiltInSource ?? false, CurrentEditContext?.PersistenceRootResource, CurrentEditContext?.PersistenceRootPath));
				XWEditorInterface.Instance?.EditResource(owner, context);
			};
			_fieldsHost.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void BuildRuntimeGuardFields(StateMachineGuardDefinition guard)
	{
		AddSectionTitle("属性比较条件");
		LineEdit lineEdit = AddLineEdit("监听属性", "例如：health、has_target、wave");
		lineEdit.Name = "ComparedPropertyEdit";
		_propertyBinding.BindText(lineEdit, guard, "ComparedProperty", UpdatePreview, this, "RefreshGuardEditorFromHistory");
		AddFieldLabel("比较方式");
		HFlowContainer hFlowContainer = AddFlow("OperatorSegments");
		ButtonGroup buttonGroup = new ButtonGroup
		{
			AllowUnpress = false
		};
		(StateMachineComparisonOperator, string, string)[] operators = Operators;
		for (int i = 0; i < operators.Length; i++)
		{
			(StateMachineComparisonOperator, string, string) tuple = operators[i];
			StateMachineComparisonOperator value = tuple.Item1;
			string item = tuple.Item2;
			string hint = tuple.Item3;
			Button button = CreateChoiceButton($"Operator{value}", item + "  " + hint, hint, "res://addons/ModEditor/Icons/FlowPort.svg", buttonGroup);
			button.SetPressedNoSignal(guard.Operator == value);
			button.Pressed += () =>
			{
				XWVisualPropertyBinding propertyBinding = _propertyBinding;
				StateMachineGuardDefinition resource = guard;
				StringName property = "Operator";
				int from = (int)value;
				propertyBinding.SetValue(resource, property, Variant.From(in from), "条件比较改为" + hint, this, "RefreshGuardEditorFromHistory");
				UpdatePreview();
			};
			hFlowContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
		AddFieldLabel("期望值类型");
		HFlowContainer host = AddFlow("ExpectedTypeSegments");
		ButtonGroup buttonGroup2 = new ButtonGroup
		{
			AllowUnpress = false
		};
		Variant.Type type = NormalizeValueType(guard.ExpectedValue.VariantType);
		AddValueTypeButton(host, buttonGroup2, "文本", Variant.Type.String, type, "res://addons/ModEditor/Icons/TextFile.svg");
		AddValueTypeButton(host, buttonGroup2, "数值", Variant.Type.Float, type, "res://addons/ModEditor/Icons/ResourceCollectable.svg");
		AddValueTypeButton(host, buttonGroup2, "开关", Variant.Type.Bool, type, "res://addons/ModEditor/Icons/Unlock.svg");
		AddFieldLabel("期望值");
		switch (type)
		{
		case Variant.Type.Bool:
		{
			CheckButton checkButton = new CheckButton
			{
				Name = "ExpectedBool",
				Text = "条件期望为开启",
				ButtonPressed = guard.ExpectedValue.AsBool()
			};
			_fieldsHost.AddChild(checkButton, forceReadableName: false, InternalMode.Disabled);
			_propertyBinding.BindToggle(checkButton, guard, "ExpectedValue", UpdatePreview, this, "RefreshGuardEditorFromHistory");
			break;
		}
		case Variant.Type.Float:
		{
			SpinBox spinBox = CreateNumber("ExpectedNumber", -1000000.0, 1000000.0, 0.1);
			_fieldsHost.AddChild(spinBox, forceReadableName: false, InternalMode.Disabled);
			_propertyBinding.BindNumber(spinBox, guard, "ExpectedValue", UpdatePreview, this, "RefreshGuardEditorFromHistory");
			break;
		}
		default:
		{
			LineEdit lineEdit2 = new LineEdit
			{
				Name = "ExpectedText",
				PlaceholderText = "与运行时属性比较的文本",
				CustomMinimumSize = new Vector2(0f, 36f)
			};
			_fieldsHost.AddChild(lineEdit2, forceReadableName: false, InternalMode.Disabled);
			_propertyBinding.BindVariantText(lineEdit2, null, guard, "ExpectedValue", UpdatePreview, this, "RefreshGuardEditorFromHistory");
			break;
		}
		}
		CheckButton checkButton2 = new CheckButton
		{
			Name = "NegateResult",
			Text = "反转最终结果",
			TooltipText = "开启后，满足变为不满足，不满足变为满足。"
		};
		_fieldsHost.AddChild(checkButton2, forceReadableName: false, InternalMode.Disabled);
		_propertyBinding.BindToggle(checkButton2, guard, "Negate", UpdatePreview, this, "RefreshGuardEditorFromHistory");
	}

	private void BuildRuntimeKindSelector(StateMachineGuardDefinition guard)
	{
		AddFieldLabel("条件拼图类型");
		HFlowContainer hFlowContainer = AddFlow("RuntimeGuardKindFlow");
		ButtonGroup buttonGroup = new ButtonGroup
		{
			AllowUnpress = false
		};
		StateMachineGuardKind[] array = new StateMachineGuardKind[5]
		{
			StateMachineGuardKind.ExpressionProperty,
			StateMachineGuardKind.All,
			StateMachineGuardKind.Any,
			StateMachineGuardKind.Not,
			StateMachineGuardKind.Callback
		};
		for (int i = 0; i < array.Length; i++)
		{
			StateMachineGuardKind kind = array[i];
			Button button = CreateChoiceButton("RuntimeKind" + kind, GetRuntimeGuardTypeLabel(kind), "切换条件拼图类型", "res://addons/ModEditor/Icons/FlowPort.svg", buttonGroup);
			button.SetPressedNoSignal(guard.Kind == kind);
			button.Pressed += () =>
			{
				SetRuntimeGuardKind(guard, kind);
			};
			hFlowContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void SetRuntimeGuardKind(StateMachineGuardDefinition guard, StateMachineGuardKind kind)
	{
		if (guard.Kind != kind)
		{
			StateMachineGuardKind kind2 = guard.Kind;
			Array<Resource> from = new Array<Resource>(guard.Children);
			Array<Resource> from2 = new Array<Resource>(guard.Children);
			if (!IsRuntimeCompositeKind(kind))
			{
				from2.Clear();
			}
			else if (kind == StateMachineGuardKind.Not && from2.Count > 1)
			{
				Resource item = from2[0];
				from2.Clear();
				from2.Add(item);
			}
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager == null)
			{
				guard.Kind = kind;
				guard.Children = from2;
			}
			else
			{
				xWUndoRedoManager.CreateAction("切换条件拼图类型");
				StringName property = "Kind";
				int from3 = (int)kind;
				xWUndoRedoManager.AddDoProperty(guard, property, Variant.From(in from3));
				xWUndoRedoManager.AddDoProperty(guard, "Children", Variant.From(in from2));
				StringName property2 = "Kind";
				from3 = (int)kind2;
				xWUndoRedoManager.AddUndoProperty(guard, property2, Variant.From(in from3));
				xWUndoRedoManager.AddUndoProperty(guard, "Children", Variant.From(in from));
				xWUndoRedoManager.AddDoMethod(this, "RefreshGuardEditorFromHistory");
				xWUndoRedoManager.AddUndoMethod(this, "RefreshGuardEditorFromHistory");
				xWUndoRedoManager.CommitAction();
			}
			OnGuardPropertyEdited(committed: true);
			RebuildEditor();
		}
	}

	private void BuildRuntimeCallbackFields(StateMachineGuardDefinition guard)
	{
		AddSectionTitle("条件动作拼图");
		AddNotice("选择已加载的 C# 或蓝图布尔条件动作。运行时会把状态转移上下文交给动作，并使用返回结果决定是否允许切换。");
		StateMachineCallbackRegistrationResult stateMachineCallbackRegistrationResult = StateMachineCallbackRegistry.Shared.EnsureBuiltinAssembly(typeof(XWStateGuardVisualResourceEditor).Assembly);
		if (!stateMachineCallbackRegistrationResult.Success)
		{
			AddNotice("内置 C# 条件回调目录加载失败：" + stateMachineCallbackRegistrationResult.Error);
		}
		string text = guard.CallbackKey.ToString();
		StateMachineCallbackCatalogEntry[] array = StateMachineCallbackRegistry.Shared.GetCatalogSnapshot().Entries ?? System.Array.Empty<StateMachineCallbackCatalogEntry>();
		AddFieldLabel("已注册条件动作");
		HFlowContainer hFlowContainer = AddFlow("RuntimeGuardCallbackCatalog");
		int num = 0;
		bool flag = string.IsNullOrWhiteSpace(text);
		ButtonGroup buttonGroup = new ButtonGroup
		{
			AllowUnpress = false
		};
		for (int i = 0; i < array.Length; i++)
		{
			StateMachineCallbackCatalogEntry stateMachineCallbackCatalogEntry = array[i];
			if ((stateMachineCallbackCatalogEntry.Phases & StateMachineCallbackPhaseFlags.Guard) == 0)
			{
				continue;
			}
			num++;
			flag |= string.Equals(stateMachineCallbackCatalogEntry.Key, text, StringComparison.Ordinal);
			string text2 = stateMachineCallbackCatalogEntry.SourceKind switch
			{
				StateMachineCallbackSourceKind.Blueprint => "\ud83e\udde9 蓝图", 
				StateMachineCallbackSourceKind.Mixed => "◆ 混合", 
				_ => "C#", 
			};
			string text3 = (string.IsNullOrWhiteSpace(stateMachineCallbackCatalogEntry.DisplayName) ? stateMachineCallbackCatalogEntry.Key : stateMachineCallbackCatalogEntry.DisplayName);
			VBoxContainer vBoxContainer = new VBoxContainer
			{
				Name = "RuntimeGuardCallbackTile" + i,
				CustomMinimumSize = new Vector2(178f, 0f)
			};
			Button button = CreateChoiceButton("RuntimeGuardCallback" + i, text3 + "\n" + text2, (stateMachineCallbackCatalogEntry.OwnerId == "builtin") ? ("游戏内置条件动作：" + stateMachineCallbackCatalogEntry.Key) : ("Mod " + stateMachineCallbackCatalogEntry.OwnerId + " 注册的条件动作：" + stateMachineCallbackCatalogEntry.Key), "res://addons/ModEditor/Icons/FlowPort.svg", buttonGroup);
			button.CustomMinimumSize = new Vector2(178f, 54f);
			button.SetPressedNoSignal(string.Equals(stateMachineCallbackCatalogEntry.Key, text, StringComparison.Ordinal));
			string callbackKey = stateMachineCallbackCatalogEntry.Key;
			button.Pressed += () =>
			{
				SetRuntimeGuardCallbackKey(guard, callbackKey);
			};
			vBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
			if (stateMachineCallbackCatalogEntry.SourceKind == StateMachineCallbackSourceKind.Blueprint && !string.IsNullOrWhiteSpace(stateMachineCallbackCatalogEntry.SourcePath))
			{
				Button button2 = new Button
				{
					Name = "RuntimeGuardOpenBlueprint" + i,
					Text = "打开条件蓝图",
					TooltipText = stateMachineCallbackCatalogEntry.SourcePath
				};
				StateMachineCallbackCatalogEntry capturedEntry = stateMachineCallbackCatalogEntry;
				button2.Pressed += () =>
				{
					XWStateMachineVisualResourceEditor.OpenCallbackSource(capturedEntry, StateMachineCallbackPhase.Guard);
				};
				vBoxContainer.AddChild(button2, forceReadableName: false, InternalMode.Disabled);
			}
			hFlowContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		}
		if (num == 0)
		{
			hFlowContainer.AddChild(new Label
			{
				Text = "尚无已注册的条件动作。可以先填写随 Mod 程序集加载的完整键。",
				AutowrapMode = TextServer.AutowrapMode.WordSmart,
				Modulate = new Color("d3ad62")
			}, forceReadableName: false, InternalMode.Disabled);
		}
		else if (!flag)
		{
			AddNotice("当前完整键尚未注册；运行前必须编译并加载提供该条件动作的 Mod。");
		}
		LineEdit lineEdit = AddLineEdit("高级：完整回调键", "builtin/key 或 mod/mod-id/key");
		lineEdit.Name = "RuntimeGuardCallbackKeyEdit";
		lineEdit.TooltipText = "手动填写完整条件动作键；适用于编辑时尚未加载的 C# 或蓝图动作";
		_propertyBinding.BindText(lineEdit, guard, "CallbackKey", UpdatePreview, this, "RefreshGuardEditorFromHistory");
		CheckButton checkButton = new CheckButton
		{
			Name = "NegateCallbackResult",
			Text = "反转条件动作结果",
			TooltipText = "开启后，条件动作返回 true 会阻止转移，返回 false 会允许转移。"
		};
		_fieldsHost.AddChild(checkButton, forceReadableName: false, InternalMode.Disabled);
		_propertyBinding.BindToggle(checkButton, guard, "Negate", UpdatePreview, this, "RefreshGuardEditorFromHistory");
	}

	private void SetRuntimeGuardCallbackKey(StateMachineGuardDefinition guard, string callbackKey)
	{
		callbackKey = (callbackKey ?? string.Empty).Trim();
		if (!string.Equals(guard.CallbackKey.ToString(), callbackKey, StringComparison.Ordinal))
		{
			_propertyBinding.SetValue(guard, "CallbackKey", Variant.From(in callbackKey), "选择条件动作", this, "RefreshGuardEditorFromHistory");
			MarkCurrentResourceDirty();
			RebuildEditor();
		}
	}

	private void BuildRuntimeCompositeFields(StateMachineGuardDefinition guard)
	{
		AddSectionTitle(guard.Kind switch
		{
			StateMachineGuardKind.All => "全部满足 · AND", 
			StateMachineGuardKind.Any => "任一满足 · OR", 
			_ => "结果取反 · NOT", 
		});
		AddNotice("子条件以可视卡片组合；修改会写回当前状态机资源并进入 Undo/Redo 历史。");
		_compositionCard.Visible = true;
		BuildRuntimeAddButtons();
	}

	private void BuildExpressionFields(ExpressionGuard guard)
	{
		AddSectionTitle("场景状态图表达式");
		AddNotice("用于旧场景 StateChart。表达式可读取状态图属性，返回值必须是布尔值。");
		TextEdit textEdit = new TextEdit
		{
			Name = "ExpressionEdit",
			CustomMinimumSize = new Vector2(0f, 150f),
			PlaceholderText = "例如：health <= 0 or wave >= 10",
			WrapMode = TextEdit.LineWrappingMode.Boundary
		};
		_fieldsHost.AddChild(textEdit, forceReadableName: false, InternalMode.Disabled);
		_propertyBinding.BindText(textEdit, guard, "expression", UpdatePreview, this, "RefreshGuardEditorFromHistory");
	}

	private void BuildStateActiveFields(StateIsActiveGuard guard)
	{
		AddSectionTitle("场景状态激活条件");
		AddNotice("填写相对于 Transition 节点的目标状态路径；状态进入或退出时自动重新判断。");
		LineEdit lineEdit = AddLineEdit("目标状态路径", "../StateChart/Root/Playing");
		lineEdit.Name = "StatePathEdit";
		_propertyBinding.BindText(lineEdit, guard, "state", UpdatePreview, this, "RefreshGuardEditorFromHistory");
	}

	private void BuildCompositeFields(Resource owner, Array<Guard> guards, string label)
	{
		AddSectionTitle(label);
		AddNotice($"当前连接 {guards?.Count ?? 0} 个子条件。下方拼图栏可以新增、排序、删除或进入子条件。");
		_compositionCard.Visible = true;
		BuildAddButtons();
	}

	private void BuildNotFields(NotGuard guard)
	{
		AddSectionTitle("取反条件 · NOT");
		AddNotice(GodotObject.IsInstanceValid(guard.guard) ? "已连接一个子条件，运行时会反转其结果。" : "尚未连接子条件；空的取反条件在旧状态图中会返回真。");
		_compositionCard.Visible = true;
		BuildAddButtons();
	}

	private void BuildAddButtons()
	{
		AddGuardCreationButton("＋ 表达式", typeof(ExpressionGuard), "res://addons/godot_state_charts/expression_guard.svg");
		AddGuardCreationButton("＋ 状态激活", typeof(StateIsActiveGuard), "res://addons/godot_state_charts/state_is_active_guard.svg");
		AddGuardCreationButton("＋ 全部满足", typeof(AllOfGuard), "res://addons/godot_state_charts/all_of_guard.svg");
		AddGuardCreationButton("＋ 任一满足", typeof(AnyOfGuard), "res://addons/godot_state_charts/any_of_guard.svg");
		AddGuardCreationButton("＋ 取反", typeof(NotGuard), "res://addons/godot_state_charts/not_guard.svg");
	}

	private void BuildRuntimeAddButtons()
	{
		AddRuntimeGuardCreationButton("属性比较", StateMachineGuardKind.ExpressionProperty);
		AddRuntimeGuardCreationButton("C# 回调条件", StateMachineGuardKind.Callback);
		AddRuntimeGuardCreationButton("全部满足 AND", StateMachineGuardKind.All);
		AddRuntimeGuardCreationButton("任一满足 OR", StateMachineGuardKind.Any);
		AddRuntimeGuardCreationButton("结果取反 NOT", StateMachineGuardKind.Not);
	}

	private void AddRuntimeGuardCreationButton(string text, StateMachineGuardKind kind)
	{
		Button button = new Button
		{
			Name = "AddRuntime" + kind,
			Text = "＋ " + text,
			CustomMinimumSize = new Vector2(132f, 38f)
		};
		button.Pressed += () =>
		{
			AddRuntimeCompositeChild(kind);
		};
		_addConditionFlow.AddChild(button, forceReadableName: false, InternalMode.Disabled);
	}

	private void AddGuardCreationButton(string text, Type type, string iconPath)
	{
		Button button = new Button
		{
			Name = "Add" + type.Name,
			Text = text,
			Icon = ResourceLoader.Load<Texture2D>(iconPath, null, ResourceLoader.CacheMode.Reuse),
			CustomMinimumSize = new Vector2(132f, 38f),
			TooltipText = "创建内联 " + GetGuardTypeLabel(type) + " 子条件"
		};
		button.AddThemeConstantOverride("icon_max_width", 22);
		button.Pressed += () =>
		{
			AddCompositeChild(type);
		};
		_addConditionFlow.AddChild(button, forceReadableName: false, InternalMode.Disabled);
	}

	private void RefreshComposition()
	{
		if (!GodotObject.IsInstanceValid(_compositionCard))
		{
			return;
		}
		bool flag = IsComposite(_editingGuard);
		_compositionCard.Visible = flag;
		if (!flag || !GodotObject.IsInstanceValid(_compositionList))
		{
			return;
		}
		ClearChildren(_compositionList);
		if (_editingGuard is StateMachineGuardDefinition stateMachineGuardDefinition && IsRuntimeCompositeKind(stateMachineGuardDefinition.Kind))
		{
			for (int i = 0; i < stateMachineGuardDefinition.Children.Count; i++)
			{
				if (stateMachineGuardDefinition.Children[i] is StateMachineGuardDefinition stateMachineGuardDefinition2 && GodotObject.IsInstanceValid(stateMachineGuardDefinition2))
				{
					_compositionList.AddChild(CreateRuntimeChildCard(stateMachineGuardDefinition2, i, stateMachineGuardDefinition.Children.Count), forceReadableName: false, InternalMode.Disabled);
				}
			}
			if (stateMachineGuardDefinition.Children.Count == 0)
			{
				_compositionList.AddChild(CreateEmptyCompositionLabel(), forceReadableName: false, InternalMode.Disabled);
			}
			return;
		}
		if (_editingGuard is NotGuard notGuard)
		{
			if (GodotObject.IsInstanceValid(notGuard.guard))
			{
				_compositionList.AddChild(CreateChildCard(notGuard.guard, 0, 1), forceReadableName: false, InternalMode.Disabled);
			}
			else
			{
				_compositionList.AddChild(CreateEmptyCompositionLabel(), forceReadableName: false, InternalMode.Disabled);
			}
			return;
		}
		Array<Guard> compositeArray = GetCompositeArray(_editingGuard);
		if (compositeArray == null || compositeArray.Count == 0)
		{
			_compositionList.AddChild(CreateEmptyCompositionLabel(), forceReadableName: false, InternalMode.Disabled);
			return;
		}
		for (int j = 0; j < compositeArray.Count; j++)
		{
			if (GodotObject.IsInstanceValid(compositeArray[j]))
			{
				_compositionList.AddChild(CreateChildCard(compositeArray[j], j, compositeArray.Count), forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private Control CreateChildCard(Guard child, int index, int count)
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.Name = $"GuardChild{index}";
		panelContainer.TooltipText = "组合条件中的子条件资源";
		panelContainer.AddThemeStyleboxOverride("panel", new StyleBoxFlat
		{
			BgColor = new Color("172116"),
			BorderColor = new Color("455d31"),
			BorderWidthLeft = 1,
			BorderWidthTop = 1,
			BorderWidthRight = 1,
			BorderWidthBottom = 1,
			CornerRadiusTopLeft = 7,
			CornerRadiusTopRight = 7,
			CornerRadiusBottomLeft = 7,
			CornerRadiusBottomRight = 7,
			ContentMarginLeft = 10f,
			ContentMarginTop = 7f,
			ContentMarginRight = 10f,
			ContentMarginBottom = 7f
		});
		HFlowContainer hFlowContainer = new HFlowContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		panelContainer.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
		TextureRect node = new TextureRect
		{
			CustomMinimumSize = new Vector2(28f, 28f),
			Texture = GetGuardIcon(child),
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
		};
		hFlowContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		hFlowContainer.AddChild(new Label
		{
			Text = $"{index + 1}. {GetGuardTypeLabel(child)}",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		Button button = CreateToolbarButton("编辑", "进入子条件专用可视面板");
		button.Pressed += () =>
		{
			OpenCompositeChild(child, index);
		};
		hFlowContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		if (!(_editingGuard is NotGuard))
		{
			Button button2 = CreateToolbarButton("↑", "向前移动");
			button2.Disabled = index <= 0;
			button2.Pressed += () =>
			{
				MoveCompositeChild(index, -1);
			};
			hFlowContainer.AddChild(button2, forceReadableName: false, InternalMode.Disabled);
			Button button3 = CreateToolbarButton("↓", "向后移动");
			button3.Disabled = index >= count - 1;
			button3.Pressed += () =>
			{
				MoveCompositeChild(index, 1);
			};
			hFlowContainer.AddChild(button3, forceReadableName: false, InternalMode.Disabled);
		}
		Button button4 = CreateToolbarButton("删除", "从组合条件移除");
		button4.Pressed += () =>
		{
			RemoveCompositeChild(index);
		};
		hFlowContainer.AddChild(button4, forceReadableName: false, InternalMode.Disabled);
		return panelContainer;
	}

	private Control CreateRuntimeChildCard(StateMachineGuardDefinition child, int index, int count)
	{
		HFlowContainer hFlowContainer = new HFlowContainer
		{
			Name = $"RuntimeGuardChild{index}",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hFlowContainer.AddChild(new Label
		{
			Text = $"{index + 1}. {GetRuntimeGuardTypeLabel(child)}",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		Button button = CreateToolbarButton("编辑", "进入子条件拼图");
		button.Name = "RuntimeGuardEditChild" + index;
		button.Pressed += () =>
		{
			OpenRuntimeCompositeChild(child, index);
		};
		hFlowContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		if (_editingGuard is StateMachineGuardDefinition { Kind: not StateMachineGuardKind.Not })
		{
			Button button2 = CreateToolbarButton("↑", "向前移动");
			button2.Disabled = index == 0;
			button2.Pressed += () =>
			{
				MoveRuntimeCompositeChild(index, -1);
			};
			hFlowContainer.AddChild(button2, forceReadableName: false, InternalMode.Disabled);
			Button button3 = CreateToolbarButton("↓", "向后移动");
			button3.Disabled = index >= count - 1;
			button3.Pressed += () =>
			{
				MoveRuntimeCompositeChild(index, 1);
			};
			hFlowContainer.AddChild(button3, forceReadableName: false, InternalMode.Disabled);
		}
		Button button4 = CreateToolbarButton("删除", "移除子条件");
		button4.Name = "RuntimeGuardRemoveChild" + index;
		button4.Pressed += () =>
		{
			RemoveRuntimeCompositeChild(index);
		};
		hFlowContainer.AddChild(button4, forceReadableName: false, InternalMode.Disabled);
		return hFlowContainer;
	}

	private void AddRuntimeCompositeChild(StateMachineGuardKind kind)
	{
		if (_editingGuard is StateMachineGuardDefinition stateMachineGuardDefinition && IsRuntimeCompositeKind(stateMachineGuardDefinition.Kind))
		{
			Array<Resource> from = new Array<Resource>(stateMachineGuardDefinition.Children);
			if (stateMachineGuardDefinition.Kind == StateMachineGuardKind.Not)
			{
				from.Clear();
			}
			from.Add(new StateMachineGuardDefinition
			{
				Kind = kind,
				ResourceName = GetRuntimeGuardTypeLabel(kind)
			});
			_propertyBinding.SetValue(stateMachineGuardDefinition, "Children", Variant.From(in from), "添加条件拼图", this, "RefreshGuardEditorFromHistory");
			MarkCurrentResourceDirty();
			RefreshComposition();
			UpdatePreview();
		}
	}

	private void RemoveRuntimeCompositeChild(int index)
	{
		if (_editingGuard is StateMachineGuardDefinition stateMachineGuardDefinition && index >= 0 && index < stateMachineGuardDefinition.Children.Count)
		{
			Array<Resource> from = new Array<Resource>(stateMachineGuardDefinition.Children);
			from.RemoveAt(index);
			_propertyBinding.SetValue(stateMachineGuardDefinition, "Children", Variant.From(in from), "移除条件拼图", this, "RefreshGuardEditorFromHistory");
			MarkCurrentResourceDirty();
			RefreshComposition();
			UpdatePreview();
		}
	}

	private void MoveRuntimeCompositeChild(int index, int direction)
	{
		if (_editingGuard is StateMachineGuardDefinition stateMachineGuardDefinition)
		{
			int num = index + direction;
			if (index >= 0 && num >= 0 && index < stateMachineGuardDefinition.Children.Count && num < stateMachineGuardDefinition.Children.Count)
			{
				Array<Resource> from = new Array<Resource>(stateMachineGuardDefinition.Children);
				Resource item = from[index];
				from.RemoveAt(index);
				from.Insert(num, item);
				_propertyBinding.SetValue(stateMachineGuardDefinition, "Children", Variant.From(in from), "调整条件顺序", this, "RefreshGuardEditorFromHistory");
				MarkCurrentResourceDirty();
				RefreshComposition();
				UpdatePreview();
			}
		}
	}

	private void OpenRuntimeCompositeChild(StateMachineGuardDefinition child, int index)
	{
		XWResourceEditContext xWResourceEditContext = XWResourceEditContext.ForProperty(child, _editingGuard, child.ResourcePath, CurrentResourcePath, "Children", index, "state_guard_editor", CurrentEditContext?.IsBuiltInSource ?? false, "", CurrentEditContext?.PersistenceRootResource, CurrentEditContext?.PersistenceRootPath);
		_guardNavigationContexts[child.GetInstanceId()] = xWResourceEditContext;
		XWEditorInterface.Instance?.EditResource(child, xWResourceEditContext);
	}

	private void AddCompositeChild(Type type)
	{
		Guard from;
		if (type == typeof(ExpressionGuard))
		{
			from = new ExpressionGuard();
		}
		else if (type == typeof(StateIsActiveGuard))
		{
			from = new StateIsActiveGuard();
		}
		else if (type == typeof(AllOfGuard))
		{
			from = new AllOfGuard();
		}
		else if (type == typeof(AnyOfGuard))
		{
			from = new AnyOfGuard();
		}
		else
		{
			from = ((type == typeof(NotGuard)) ? new NotGuard() : null);
		}
		if (!GodotObject.IsInstanceValid(from))
		{
			return;
		}
		from.ResourceName = GetGuardTypeLabel(type);
		if (_editingGuard is NotGuard resource)
		{
			_propertyBinding.SetValue(resource, "guard", Variant.From(in from), "连接取反子条件", this, "RefreshGuardEditorFromHistory");
		}
		else
		{
			Array<Guard> compositeArray = GetCompositeArray(_editingGuard);
			if (compositeArray != null)
			{
				Array<Guard> from2 = new Array<Guard>(compositeArray);
				from2.Add(from);
				_propertyBinding.SetValue(_editingGuard, "guards", Variant.From(in from2), "添加组合子条件", this, "RefreshGuardEditorFromHistory");
			}
		}
		MarkCurrentResourceDirty();
		RefreshComposition();
		UpdatePreview();
	}

	private void RemoveCompositeChild(int index)
	{
		if (_editingGuard is NotGuard resource)
		{
			_propertyBinding.SetValue(resource, "guard", default, "移除取反子条件", this, "RefreshGuardEditorFromHistory");
		}
		else
		{
			Array<Guard> compositeArray = GetCompositeArray(_editingGuard);
			if (compositeArray != null && index >= 0 && index < compositeArray.Count)
			{
				Array<Guard> from = new Array<Guard>(compositeArray);
				from.RemoveAt(index);
				_propertyBinding.SetValue(_editingGuard, "guards", Variant.From(in from), "删除组合子条件", this, "RefreshGuardEditorFromHistory");
			}
		}
		MarkCurrentResourceDirty();
		RefreshComposition();
		UpdatePreview();
	}

	private void MoveCompositeChild(int index, int direction)
	{
		Array<Guard> compositeArray = GetCompositeArray(_editingGuard);
		int num = index + direction;
		if (compositeArray != null && index >= 0 && index < compositeArray.Count && num >= 0 && num < compositeArray.Count)
		{
			Array<Guard> from = new Array<Guard>(compositeArray);
			Guard item = from[index];
			from.RemoveAt(index);
			from.Insert(num, item);
			_propertyBinding.SetValue(_editingGuard, "guards", Variant.From(in from), "调整组合条件顺序", this, "RefreshGuardEditorFromHistory");
			MarkCurrentResourceDirty();
			RefreshComposition();
			UpdatePreview();
		}
	}

	private void OpenCompositeChild(Guard child, int index)
	{
		if (GodotObject.IsInstanceValid(child))
		{
			string propertyPath = ((_editingGuard is NotGuard) ? "guard" : "guards");
			XWResourceEditContext xWResourceEditContext = XWResourceEditContext.ForProperty(child, _editingGuard, child.ResourcePath, CurrentResourcePath, propertyPath, (_editingGuard is NotGuard) ? (-1) : index, "state_guard_editor", CurrentEditContext?.IsBuiltInSource ?? false, "", CurrentEditContext?.PersistenceRootResource, CurrentEditContext?.PersistenceRootPath);
			_guardNavigationContexts[child.GetInstanceId()] = xWResourceEditContext;
			XWEditorInterface.Instance?.EditResource(child, xWResourceEditContext);
		}
	}

	private void AddValueTypeButton(HFlowContainer host, ButtonGroup group, string text, Variant.Type type, Variant.Type current, string iconPath)
	{
		Button button = CreateChoiceButton("ExpectedType" + type, text, "把期望值改为" + text, iconPath, group);
		button.SetPressedNoSignal(current == type);
		button.Pressed += () =>
		{
			if (_editingGuard is StateMachineGuardDefinition resource)
			{
				Variant variant = type switch
				{
					Variant.Type.Bool => Variant.From<bool>(false), 
					Variant.Type.Float => Variant.From<double>(0.0), 
					_ => Variant.From(in string.Empty), 
				};
				_propertyBinding.SetValue(resource, "ExpectedValue", variant, "期望值类型改为" + text, this, "RefreshGuardEditorFromHistory");
				_actualValue = variant;
				RebuildEditor();
			}
		};
		host.AddChild(button, forceReadableName: false, InternalMode.Disabled);
	}

	private void BuildActualValueEditor()
	{
		if (!(_editingGuard is StateMachineGuardDefinition stateMachineGuardDefinition))
		{
			_actualEditorHost.AddChild(new Label
			{
				Text = "需在实际 StateChart 场景中判断",
				AutowrapMode = TextServer.AutowrapMode.WordSmart
			}, forceReadableName: false, InternalMode.Disabled);
			return;
		}
		if (stateMachineGuardDefinition.Kind == StateMachineGuardKind.Callback)
		{
			_actualEditorHost.AddChild(new Label
			{
				Text = "C# 回调需要实际宿主与转移上下文，编辑器仅校验注册状态。",
				AutowrapMode = TextServer.AutowrapMode.WordSmart
			}, forceReadableName: false, InternalMode.Disabled);
			return;
		}
		Variant.Type type = NormalizeValueType(stateMachineGuardDefinition.ExpectedValue.VariantType);
		if (_actualValue.VariantType != type)
		{
			_actualValue = type switch
			{
				Variant.Type.Bool => Variant.From<bool>(false), 
				Variant.Type.Float => Variant.From<double>(0.0), 
				_ => Variant.From(in string.Empty), 
			};
		}
		switch (type)
		{
		case Variant.Type.Bool:
		{
			CheckButton checkButton = new CheckButton
			{
				Name = "ActualBool",
				Text = "当前为开启",
				ButtonPressed = _actualValue.AsBool()
			};
			checkButton.Toggled += (bool value) =>
			{
				_actualValue = Variant.From(in value);
				UpdatePreview();
			};
			_actualEditorHost.AddChild(checkButton, forceReadableName: false, InternalMode.Disabled);
			return;
		}
		case Variant.Type.Float:
		{
			SpinBox spinBox = CreateNumber("ActualNumber", -1000000.0, 1000000.0, 0.1);
			spinBox.Value = _actualValue.AsDouble();
			spinBox.ValueChanged += (double value) =>
			{
				_actualValue = Variant.From(in value);
				UpdatePreview();
			};
			_actualEditorHost.AddChild(spinBox, forceReadableName: false, InternalMode.Disabled);
			return;
		}
		}
		LineEdit lineEdit = new LineEdit
		{
			Name = "ActualText",
			Text = _actualValue.AsString(),
			PlaceholderText = "输入模拟值",
			CustomMinimumSize = new Vector2(160f, 36f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		lineEdit.TextChanged += (string value) =>
		{
			_actualValue = Variant.From(in value);
			UpdatePreview();
		};
		_actualEditorHost.AddChild(lineEdit, forceReadableName: false, InternalMode.Disabled);
	}

	private void OnGuardPropertyEdited(bool committed)
	{
		if (GodotObject.IsInstanceValid(_editingGuard) && CurrentResource == _editingGuard)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
			}
			else
			{
				MarkCurrentResourceDirty();
				CurrentResource.EmitChanged();
			}
			UpdatePreview();
		}
	}

	public void RefreshGuardEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()))
		{
			RebuildEditor();
		}
	}

	private void UpdatePreview()
	{
		if (!_rebuilding && GodotObject.IsInstanceValid(_previewCanvas))
		{
			_previewCanvas.Bind(_editingGuard, _actualValue);
			Label resultBadge = _resultBadge;
			bool? previewResult = _previewCanvas.PreviewResult;
			string text;
			if (previewResult.HasValue)
			{
				text = ((previewResult != true) ? "× 当前阻止切换" : "✓ 当前允许切换");
			}
			else
			{
				text = "运行时场景判断";
			}
			resultBadge.Text = text;
			Label resultBadge2 = _resultBadge;
			previewResult = _previewCanvas.PreviewResult;
			Color modulate;
			if (previewResult.HasValue)
			{
				modulate = ((previewResult != true) ? new Color("f18a65") : new Color("9ce564"));
			}
			else
			{
				modulate = new Color("cbd9b8");
			}
			resultBadge2.Modulate = modulate;
		}
	}

	private void AddSummaryRows()
	{
		if (PreviewList != null)
		{
			PreviewList.AddItem("条件类型 -> " + GetGuardTypeLabel(_editingGuard));
			PreviewList.AddItem("编辑方式 -> 主面板拼图与实时条件链路");
			PreviewList.AddItem("性能 -> 隐藏时停止处理，输入变化时才重绘");
			PreviewList.AddItem("保存 -> 工具栏保存写回当前资源或所属资源");
		}
	}

	private void DisposeBinding(bool clearGuard = true)
	{
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		if (clearGuard)
		{
			_editingGuard = null;
		}
	}

	private void AddSectionTitle(string text)
	{
		_fieldsHost.AddChild(new Label
		{
			Text = text,
			ThemeTypeVariation = "HeaderSmall"
		}, forceReadableName: false, InternalMode.Disabled);
	}

	private void AddNotice(string text)
	{
		_fieldsHost.AddChild(new Label
		{
			Text = text,
			AutowrapMode = TextServer.AutowrapMode.WordSmart,
			Modulate = new Color("aebd9b")
		}, forceReadableName: false, InternalMode.Disabled);
	}

	private void AddFieldLabel(string text)
	{
		_fieldsHost.AddChild(new Label
		{
			Text = text
		}, forceReadableName: false, InternalMode.Disabled);
	}

	private LineEdit AddLineEdit(string label, string placeholder)
	{
		AddFieldLabel(label);
		LineEdit lineEdit = new LineEdit
		{
			Name = label.Replace(" ", string.Empty) + "Edit",
			PlaceholderText = placeholder,
			CustomMinimumSize = new Vector2(0f, 36f)
		};
		_fieldsHost.AddChild(lineEdit, forceReadableName: false, InternalMode.Disabled);
		return lineEdit;
	}

	private HFlowContainer AddFlow(string name)
	{
		HFlowContainer hFlowContainer = new HFlowContainer
		{
			Name = name,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hFlowContainer.AddThemeConstantOverride("h_separation", 6);
		hFlowContainer.AddThemeConstantOverride("v_separation", 6);
		_fieldsHost.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
		return hFlowContainer;
	}

	private static Button CreateChoiceButton(string name, string text, string tooltip, string iconPath, ButtonGroup group)
	{
		Button button = new Button();
		button.Name = name;
		button.Text = text;
		button.TooltipText = tooltip;
		button.ToggleMode = true;
		button.ButtonGroup = group;
		button.Icon = ResourceLoader.Load<Texture2D>(iconPath, null, ResourceLoader.CacheMode.Reuse);
		button.CustomMinimumSize = new Vector2(106f, 38f);
		button.AddThemeConstantOverride("icon_max_width", 20);
		return button;
	}

	private static SpinBox CreateNumber(string name, double minimum, double maximum, double step)
	{
		return new SpinBox
		{
			Name = name,
			MinValue = minimum,
			MaxValue = maximum,
			Step = step,
			AllowGreater = true,
			AllowLesser = true,
			CustomMinimumSize = new Vector2(160f, 36f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
	}

	private static Button CreateToolbarButton(string text, string tooltip)
	{
		return new Button
		{
			Text = text,
			TooltipText = tooltip,
			CustomMinimumSize = new Vector2((text.Length <= 1) ? 38 : 62, 34f)
		};
	}

	private static Label CreateEmptyCompositionLabel()
	{
		return new Label
		{
			Text = "尚未连接子条件，请从上方图标按钮添加。",
			AutowrapMode = TextServer.AutowrapMode.WordSmart,
			Modulate = new Color("9fab91")
		};
	}

	private static Texture2D GetGuardIcon(Resource guard)
	{
		string path;
		if (guard is ExpressionGuard)
		{
			path = "res://addons/godot_state_charts/expression_guard.svg";
		}
		else if (guard is StateIsActiveGuard)
		{
			path = "res://addons/godot_state_charts/state_is_active_guard.svg";
		}
		else if (guard is AllOfGuard)
		{
			path = "res://addons/godot_state_charts/all_of_guard.svg";
		}
		else if (guard is AnyOfGuard)
		{
			path = "res://addons/godot_state_charts/any_of_guard.svg";
		}
		else
		{
			path = ((!(guard is NotGuard)) ? "res://addons/godot_state_charts/guard.svg" : "res://addons/godot_state_charts/not_guard.svg");
		}
		return ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse);
	}

	private static bool IsSupportedGuard(Resource resource)
	{
		if (resource is StateMachineGuardDefinition || resource is ExpressionGuard || resource is StateIsActiveGuard || resource is AllOfGuard || resource is AnyOfGuard || resource is NotGuard)
		{
			return true;
		}
		return false;
	}

	private static bool IsComposite(Resource resource)
	{
		if ((!(resource is AllOfGuard) && !(resource is AnyOfGuard) && !(resource is NotGuard)) || 1 == 0)
		{
			if (resource is StateMachineGuardDefinition stateMachineGuardDefinition)
			{
				return IsRuntimeCompositeKind(stateMachineGuardDefinition.Kind);
			}
			return false;
		}
		return true;
	}

	private static Array<Guard> GetCompositeArray(Resource resource)
	{
		if (!(resource is AllOfGuard { guards: var guards }))
		{
			if (!(resource is AnyOfGuard { guards: var guards2 }))
			{
				return null;
			}
			return guards2;
		}
		return guards;
	}

	private static int GetCompositeCount(Resource resource)
	{
		if (!(resource is StateMachineGuardDefinition stateMachineGuardDefinition))
		{
			if (resource is AllOfGuard { guards: var guards })
			{
				return guards?.Count ?? 0;
			}
			if (resource is AnyOfGuard { guards: var guards2 })
			{
				return guards2?.Count ?? 0;
			}
			if (resource is NotGuard notGuard)
			{
				return GodotObject.IsInstanceValid(notGuard.guard) ? 1 : 0;
			}
		}
		else if (IsRuntimeCompositeKind(stateMachineGuardDefinition.Kind))
		{
			return stateMachineGuardDefinition.Children?.Count ?? 0;
		}
		return 0;
	}

	private static Variant.Type NormalizeValueType(Variant.Type type)
	{
		switch (type)
		{
		case Variant.Type.Bool:
			return Variant.Type.Bool;
		case Variant.Type.Int:
		case Variant.Type.Float:
			return Variant.Type.Float;
		default:
			return Variant.Type.String;
		}
	}

	private static string GetGuardTypeLabel(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "未知条件";
		}
		if (resource is StateMachineGuardDefinition guard)
		{
			return "资源状态机 · " + GetRuntimeGuardTypeLabel(guard);
		}
		return GetGuardTypeLabel(resource.GetType());
	}

	private static string GetGuardTypeLabel(Type type)
	{
		if (type == typeof(StateMachineGuardDefinition))
		{
			return "资源状态机条件拼图";
		}
		if (type == typeof(ExpressionGuard))
		{
			return "表达式条件";
		}
		if (type == typeof(StateIsActiveGuard))
		{
			return "状态激活条件";
		}
		if (type == typeof(AllOfGuard))
		{
			return "全部满足组合";
		}
		if (type == typeof(AnyOfGuard))
		{
			return "任一满足组合";
		}
		if (type == typeof(NotGuard))
		{
			return "结果取反组合";
		}
		return type?.Name ?? "未知条件";
	}

	private static string GetRuntimeGuardTypeLabel(StateMachineGuardDefinition guard)
	{
		return GetRuntimeGuardTypeLabel(guard?.Kind ?? StateMachineGuardKind.ExpressionProperty);
	}

	private static string GetRuntimeGuardTypeLabel(StateMachineGuardKind kind)
	{
		return kind switch
		{
			StateMachineGuardKind.All => "全部满足 AND", 
			StateMachineGuardKind.Any => "任一满足 OR", 
			StateMachineGuardKind.Not => "结果取反 NOT", 
			StateMachineGuardKind.Callback => "C# 回调条件", 
			_ => "属性比较", 
		};
	}

	private static bool IsRuntimeCompositeKind(StateMachineGuardKind kind)
	{
		if ((uint)(kind - 1) <= 2u)
		{
			return true;
		}
		return false;
	}

	private static void ClearChildren(Node parent)
	{
		if (!GodotObject.IsInstanceValid(parent))
		{
			return;
		}
		foreach (Node child in parent.GetChildren())
		{
			parent.RemoveChild(child);
			child.QueueFree();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(53)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildReturnToOwnerButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildRuntimeGuardFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildRuntimeKindSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeGuardKind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildRuntimeCallbackFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeGuardCallbackKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "callbackKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildRuntimeCompositeFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildExpressionFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildStateActiveFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCompositeFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Array, "guards", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildNotFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildAddButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildRuntimeAddButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddRuntimeGuardCreationButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshComposition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateChildCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateRuntimeChildCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddRuntimeCompositeChild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveRuntimeCompositeChild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveRuntimeCompositeChild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenRuntimeCompositeChild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveCompositeChild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveCompositeChild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenCompositeChild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddValueTypeButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "group", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ButtonGroup"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "current", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildActualValueEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGuardPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshGuardEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "clearGuard", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddSectionTitle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddNotice, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFieldLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddLineEdit, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("LineEdit"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "placeholder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFlow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateChoiceButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "group", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ButtonGroup"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateNumber, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateToolbarButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEmptyCompositionLabel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetGuardIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsSupportedGuard, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsComposite, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCompositeArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCompositeCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeValueType, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetGuardTypeLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetRuntimeGuardTypeLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsRuntimeCompositeKind, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearChildren, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.BindScene && args.Count == 1)
		{
			BindScene(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildEditor && args.Count == 0)
		{
			RebuildEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildReturnToOwnerButton && args.Count == 0)
		{
			BuildReturnToOwnerButton();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRuntimeGuardFields && args.Count == 1)
		{
			BuildRuntimeGuardFields(VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRuntimeKindSelector && args.Count == 1)
		{
			BuildRuntimeKindSelector(VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRuntimeGuardKind && args.Count == 2)
		{
			SetRuntimeGuardKind(VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[0]), VariantUtils.ConvertTo<StateMachineGuardKind>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRuntimeCallbackFields && args.Count == 1)
		{
			BuildRuntimeCallbackFields(VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRuntimeGuardCallbackKey && args.Count == 2)
		{
			SetRuntimeGuardCallbackKey(VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRuntimeCompositeFields && args.Count == 1)
		{
			BuildRuntimeCompositeFields(VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildExpressionFields && args.Count == 1)
		{
			BuildExpressionFields(VariantUtils.ConvertTo<ExpressionGuard>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildStateActiveFields && args.Count == 1)
		{
			BuildStateActiveFields(VariantUtils.ConvertTo<StateIsActiveGuard>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildCompositeFields && args.Count == 3)
		{
			BuildCompositeFields(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertToArray<Guard>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildNotFields && args.Count == 1)
		{
			BuildNotFields(VariantUtils.ConvertTo<NotGuard>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildAddButtons && args.Count == 0)
		{
			BuildAddButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRuntimeAddButtons && args.Count == 0)
		{
			BuildRuntimeAddButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.AddRuntimeGuardCreationButton && args.Count == 2)
		{
			AddRuntimeGuardCreationButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StateMachineGuardKind>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshComposition && args.Count == 0)
		{
			RefreshComposition();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateChildCard && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateChildCard(VariantUtils.ConvertTo<Guard>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateRuntimeChildCard && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateRuntimeChildCard(VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.AddRuntimeCompositeChild && args.Count == 1)
		{
			AddRuntimeCompositeChild(VariantUtils.ConvertTo<StateMachineGuardKind>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveRuntimeCompositeChild && args.Count == 1)
		{
			RemoveRuntimeCompositeChild(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveRuntimeCompositeChild && args.Count == 2)
		{
			MoveRuntimeCompositeChild(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenRuntimeCompositeChild && args.Count == 2)
		{
			OpenRuntimeCompositeChild(VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveCompositeChild && args.Count == 1)
		{
			RemoveCompositeChild(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveCompositeChild && args.Count == 2)
		{
			MoveCompositeChild(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenCompositeChild && args.Count == 2)
		{
			OpenCompositeChild(VariantUtils.ConvertTo<Guard>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddValueTypeButton && args.Count == 6)
		{
			AddValueTypeButton(VariantUtils.ConvertTo<HFlowContainer>(in args[0]), VariantUtils.ConvertTo<ButtonGroup>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Variant.Type>(in args[3]), VariantUtils.ConvertTo<Variant.Type>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildActualValueEditor && args.Count == 0)
		{
			BuildActualValueEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGuardPropertyEdited && args.Count == 1)
		{
			OnGuardPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshGuardEditorFromHistory && args.Count == 0)
		{
			RefreshGuardEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreview && args.Count == 0)
		{
			UpdatePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 0)
		{
			AddSummaryRows();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeBinding && args.Count == 1)
		{
			DisposeBinding(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSectionTitle && args.Count == 1)
		{
			AddSectionTitle(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddNotice && args.Count == 1)
		{
			AddNotice(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddFieldLabel && args.Count == 1)
		{
			AddFieldLabel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddLineEdit && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<LineEdit>(AddLineEdit(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AddFlow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<HFlowContainer>(AddFlow(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateChoiceButton && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<Button>(CreateChoiceButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<ButtonGroup>(in args[4])));
			return true;
		}
		if (method == MethodName.CreateNumber && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateNumber(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
			return true;
		}
		if (method == MethodName.CreateToolbarButton && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(CreateToolbarButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateEmptyCompositionLabel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateEmptyCompositionLabel());
			return true;
		}
		if (method == MethodName.GetGuardIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetGuardIcon(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSupportedGuard && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSupportedGuard(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.IsComposite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsComposite(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCompositeArray && args.Count == 1)
		{
			Array<Guard> compositeArray = GetCompositeArray(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = VariantUtils.CreateFromArray(compositeArray);
			return true;
		}
		if (method == MethodName.GetCompositeCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetCompositeCount(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeValueType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant.Type>(NormalizeValueType(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetGuardTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetGuardTypeLabel(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetRuntimeGuardTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetRuntimeGuardTypeLabel(VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[0])));
			return true;
		}
		if (method == MethodName.IsRuntimeCompositeKind && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRuntimeCompositeKind(VariantUtils.ConvertTo<StateMachineGuardKind>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearChildren && args.Count == 1)
		{
			ClearChildren(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateChoiceButton && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<Button>(CreateChoiceButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<ButtonGroup>(in args[4])));
			return true;
		}
		if (method == MethodName.CreateNumber && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(CreateNumber(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
			return true;
		}
		if (method == MethodName.CreateToolbarButton && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(CreateToolbarButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateEmptyCompositionLabel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateEmptyCompositionLabel());
			return true;
		}
		if (method == MethodName.GetGuardIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetGuardIcon(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSupportedGuard && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSupportedGuard(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.IsComposite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsComposite(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCompositeArray && args.Count == 1)
		{
			Array<Guard> compositeArray = GetCompositeArray(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = VariantUtils.CreateFromArray(compositeArray);
			return true;
		}
		if (method == MethodName.GetCompositeCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetCompositeCount(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeValueType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant.Type>(NormalizeValueType(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetGuardTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetGuardTypeLabel(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetRuntimeGuardTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetRuntimeGuardTypeLabel(VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[0])));
			return true;
		}
		if (method == MethodName.IsRuntimeCompositeKind && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRuntimeCompositeKind(VariantUtils.ConvertTo<StateMachineGuardKind>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearChildren && args.Count == 1)
		{
			ClearChildren(VariantUtils.ConvertTo<Node>(in args[0]));
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
		if (method == MethodName.BindScene)
		{
			return true;
		}
		if (method == MethodName.RebuildEditor)
		{
			return true;
		}
		if (method == MethodName.BuildReturnToOwnerButton)
		{
			return true;
		}
		if (method == MethodName.BuildRuntimeGuardFields)
		{
			return true;
		}
		if (method == MethodName.BuildRuntimeKindSelector)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeGuardKind)
		{
			return true;
		}
		if (method == MethodName.BuildRuntimeCallbackFields)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeGuardCallbackKey)
		{
			return true;
		}
		if (method == MethodName.BuildRuntimeCompositeFields)
		{
			return true;
		}
		if (method == MethodName.BuildExpressionFields)
		{
			return true;
		}
		if (method == MethodName.BuildStateActiveFields)
		{
			return true;
		}
		if (method == MethodName.BuildCompositeFields)
		{
			return true;
		}
		if (method == MethodName.BuildNotFields)
		{
			return true;
		}
		if (method == MethodName.BuildAddButtons)
		{
			return true;
		}
		if (method == MethodName.BuildRuntimeAddButtons)
		{
			return true;
		}
		if (method == MethodName.AddRuntimeGuardCreationButton)
		{
			return true;
		}
		if (method == MethodName.RefreshComposition)
		{
			return true;
		}
		if (method == MethodName.CreateChildCard)
		{
			return true;
		}
		if (method == MethodName.CreateRuntimeChildCard)
		{
			return true;
		}
		if (method == MethodName.AddRuntimeCompositeChild)
		{
			return true;
		}
		if (method == MethodName.RemoveRuntimeCompositeChild)
		{
			return true;
		}
		if (method == MethodName.MoveRuntimeCompositeChild)
		{
			return true;
		}
		if (method == MethodName.OpenRuntimeCompositeChild)
		{
			return true;
		}
		if (method == MethodName.RemoveCompositeChild)
		{
			return true;
		}
		if (method == MethodName.MoveCompositeChild)
		{
			return true;
		}
		if (method == MethodName.OpenCompositeChild)
		{
			return true;
		}
		if (method == MethodName.AddValueTypeButton)
		{
			return true;
		}
		if (method == MethodName.BuildActualValueEditor)
		{
			return true;
		}
		if (method == MethodName.OnGuardPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshGuardEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.UpdatePreview)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.DisposeBinding)
		{
			return true;
		}
		if (method == MethodName.AddSectionTitle)
		{
			return true;
		}
		if (method == MethodName.AddNotice)
		{
			return true;
		}
		if (method == MethodName.AddFieldLabel)
		{
			return true;
		}
		if (method == MethodName.AddLineEdit)
		{
			return true;
		}
		if (method == MethodName.AddFlow)
		{
			return true;
		}
		if (method == MethodName.CreateChoiceButton)
		{
			return true;
		}
		if (method == MethodName.CreateNumber)
		{
			return true;
		}
		if (method == MethodName.CreateToolbarButton)
		{
			return true;
		}
		if (method == MethodName.CreateEmptyCompositionLabel)
		{
			return true;
		}
		if (method == MethodName.GetGuardIcon)
		{
			return true;
		}
		if (method == MethodName.IsSupportedGuard)
		{
			return true;
		}
		if (method == MethodName.IsComposite)
		{
			return true;
		}
		if (method == MethodName.GetCompositeArray)
		{
			return true;
		}
		if (method == MethodName.GetCompositeCount)
		{
			return true;
		}
		if (method == MethodName.NormalizeValueType)
		{
			return true;
		}
		if (method == MethodName.GetGuardTypeLabel)
		{
			return true;
		}
		if (method == MethodName.GetRuntimeGuardTypeLabel)
		{
			return true;
		}
		if (method == MethodName.IsRuntimeCompositeKind)
		{
			return true;
		}
		if (method == MethodName.ClearChildren)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editingGuard)
		{
			_editingGuard = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._fieldsHost)
		{
			_fieldsHost = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._compositionList)
		{
			_compositionList = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._addConditionFlow)
		{
			_addConditionFlow = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._compositionCard)
		{
			_compositionCard = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._previewCanvas)
		{
			_previewCanvas = VariantUtils.ConvertTo<XWStateGuardPreviewCanvas>(in value);
			return true;
		}
		if (name == PropertyName._actualEditorHost)
		{
			_actualEditorHost = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._resultBadge)
		{
			_resultBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._typeBadge)
		{
			_typeBadge = VariantUtils.ConvertTo<Label>(in value);
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
		if (name == PropertyName._saveGuardButton)
		{
			_saveGuardButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._actualValue)
		{
			_actualValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName._rebuilding)
		{
			_rebuilding = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.EditingGuard)
		{
			value = VariantUtils.CreateFrom<Resource>(EditingGuard);
			return true;
		}
		if (name == PropertyName.GuardPreviewCanvas)
		{
			value = VariantUtils.CreateFrom<XWStateGuardPreviewCanvas>(GuardPreviewCanvas);
			return true;
		}
		if (name == PropertyName.CompositionItemCount)
		{
			value = VariantUtils.CreateFrom<int>(CompositionItemCount);
			return true;
		}
		if (name == PropertyName.SaveGuardButton)
		{
			value = VariantUtils.CreateFrom<Button>(SaveGuardButton);
			return true;
		}
		if (name == PropertyName._editingGuard)
		{
			value = VariantUtils.CreateFrom(in _editingGuard);
			return true;
		}
		if (name == PropertyName._fieldsHost)
		{
			value = VariantUtils.CreateFrom(in _fieldsHost);
			return true;
		}
		if (name == PropertyName._compositionList)
		{
			value = VariantUtils.CreateFrom(in _compositionList);
			return true;
		}
		if (name == PropertyName._addConditionFlow)
		{
			value = VariantUtils.CreateFrom(in _addConditionFlow);
			return true;
		}
		if (name == PropertyName._compositionCard)
		{
			value = VariantUtils.CreateFrom(in _compositionCard);
			return true;
		}
		if (name == PropertyName._previewCanvas)
		{
			value = VariantUtils.CreateFrom(in _previewCanvas);
			return true;
		}
		if (name == PropertyName._actualEditorHost)
		{
			value = VariantUtils.CreateFrom(in _actualEditorHost);
			return true;
		}
		if (name == PropertyName._resultBadge)
		{
			value = VariantUtils.CreateFrom(in _resultBadge);
			return true;
		}
		if (name == PropertyName._typeBadge)
		{
			value = VariantUtils.CreateFrom(in _typeBadge);
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
		if (name == PropertyName._saveGuardButton)
		{
			value = VariantUtils.CreateFrom(in _saveGuardButton);
			return true;
		}
		if (name == PropertyName._actualValue)
		{
			value = VariantUtils.CreateFrom(in _actualValue);
			return true;
		}
		if (name == PropertyName._rebuilding)
		{
			value = VariantUtils.CreateFrom(in _rebuilding);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingGuard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fieldsHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._compositionList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addConditionFlow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._compositionCard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewCanvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._actualEditorHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveGuardButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName._actualValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._rebuilding, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.EditingGuard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.GuardPreviewCanvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CompositionItemCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.SaveGuardButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingGuard, Variant.From(in _editingGuard));
		info.AddProperty(PropertyName._fieldsHost, Variant.From(in _fieldsHost));
		info.AddProperty(PropertyName._compositionList, Variant.From(in _compositionList));
		info.AddProperty(PropertyName._addConditionFlow, Variant.From(in _addConditionFlow));
		info.AddProperty(PropertyName._compositionCard, Variant.From(in _compositionCard));
		info.AddProperty(PropertyName._previewCanvas, Variant.From(in _previewCanvas));
		info.AddProperty(PropertyName._actualEditorHost, Variant.From(in _actualEditorHost));
		info.AddProperty(PropertyName._resultBadge, Variant.From(in _resultBadge));
		info.AddProperty(PropertyName._typeBadge, Variant.From(in _typeBadge));
		info.AddProperty(PropertyName._resourceName, Variant.From(in _resourceName));
		info.AddProperty(PropertyName._localToScene, Variant.From(in _localToScene));
		info.AddProperty(PropertyName._saveGuardButton, Variant.From(in _saveGuardButton));
		info.AddProperty(PropertyName._actualValue, Variant.From(in _actualValue));
		info.AddProperty(PropertyName._rebuilding, Variant.From(in _rebuilding));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingGuard, out var value))
		{
			_editingGuard = value.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._fieldsHost, out var value2))
		{
			_fieldsHost = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._compositionList, out var value3))
		{
			_compositionList = value3.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._addConditionFlow, out var value4))
		{
			_addConditionFlow = value4.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._compositionCard, out var value5))
		{
			_compositionCard = value5.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._previewCanvas, out var value6))
		{
			_previewCanvas = value6.As<XWStateGuardPreviewCanvas>();
		}
		if (info.TryGetProperty(PropertyName._actualEditorHost, out var value7))
		{
			_actualEditorHost = value7.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._resultBadge, out var value8))
		{
			_resultBadge = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._typeBadge, out var value9))
		{
			_typeBadge = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resourceName, out var value10))
		{
			_resourceName = value10.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToScene, out var value11))
		{
			_localToScene = value11.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._saveGuardButton, out var value12))
		{
			_saveGuardButton = value12.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._actualValue, out var value13))
		{
			_actualValue = value13.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName._rebuilding, out var value14))
		{
			_rebuilding = value14.As<bool>();
		}
	}
}
