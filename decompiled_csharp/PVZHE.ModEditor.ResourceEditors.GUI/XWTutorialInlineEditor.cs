using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWTutorialInlineEditor.cs")]
public class XWTutorialInlineEditor : VBoxContainer
{
	[Signal]
	public delegate void TutorialEditedEventHandler();

	private enum TutorialEditScope
	{
		Tutorial,
		Step,
		Condition
	}

	private sealed class TutorialRootSnapshot
	{
		public Json Data;

		public string SaveKey = "";

		public Array<TutorialStepConfig> Steps = new Array<TutorialStepConfig>();

		public int SelectedIndex;
	}

	public new class MethodName : VBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName EditConfig = "EditConfig";

		public static readonly StringName SelectStep = "SelectStep";

		public static readonly StringName ShowPrevious = "ShowPrevious";

		public static readonly StringName ShowNext = "ShowNext";

		public static readonly StringName AddStep = "AddStep";

		public static readonly StringName DuplicateStep = "DuplicateStep";

		public static readonly StringName RemoveStep = "RemoveStep";

		public static readonly StringName MoveStepUp = "MoveStepUp";

		public static readonly StringName MoveStepDown = "MoveStepDown";

		public static readonly StringName MoveStep = "MoveStep";

		public static readonly StringName SetBroadcastUse = "SetBroadcastUse";

		public static readonly StringName SetBroadcastText = "SetBroadcastText";

		public static readonly StringName SetBroadcastDuration = "SetBroadcastDuration";

		public static readonly StringName SetCharacterName = "SetCharacterName";

		public static readonly StringName SetComparisonMethod = "SetComparisonMethod";

		public static readonly StringName SetConditionTarget = "SetConditionTarget";

		public static readonly StringName AddCondition = "AddCondition";

		public static readonly StringName AddSelectedCondition = "AddSelectedCondition";

		public static readonly StringName DuplicateCondition = "DuplicateCondition";

		public static readonly StringName MoveCondition = "MoveCondition";

		public static readonly StringName RemoveCondition = "RemoveCondition";

		public static readonly StringName RefreshFromConfig = "RefreshFromConfig";

		public static readonly StringName ShowCurrentStep = "ShowCurrentStep";

		public static readonly StringName RefreshConditionRows = "RefreshConditionRows";

		public static readonly StringName BindConditionRow = "BindConditionRow";

		public static readonly StringName ApplyJsonSource = "ApplyJsonSource";

		public static readonly StringName StoreRootSnapshot = "StoreRootSnapshot";

		public static readonly StringName ApplyTutorialRootSnapshot = "ApplyTutorialRootSnapshot";

		public static readonly StringName AddConditionResource = "AddConditionResource";

		public static readonly StringName ReplaceConditionList = "ReplaceConditionList";

		public static readonly StringName ReplaceStepList = "ReplaceStepList";

		public static readonly StringName RefreshTutorialCollectionFromHistory = "RefreshTutorialCollectionFromHistory";

		public static readonly StringName RefreshConditionCollectionFromHistory = "RefreshConditionCollectionFromHistory";

		public static readonly StringName RefreshTutorialPropertiesFromHistory = "RefreshTutorialPropertiesFromHistory";

		public static readonly StringName SetResourceProperty = "SetResourceProperty";

		public static readonly StringName UpdateActionAvailability = "UpdateActionAvailability";

		public static readonly StringName RefreshStatus = "RefreshStatus";

		public static readonly StringName DisposeConditionBindings = "DisposeConditionBindings";

		public static readonly StringName CreateDefaultBroadcast = "CreateDefaultBroadcast";

		public static readonly StringName SetButtonIcon = "SetButtonIcon";

		public static readonly StringName CanEditCondition = "CanEditCondition";

		public static readonly StringName GetCurrentStep = "GetCurrentStep";

		public static readonly StringName EmitTutorialEdited = "EmitTutorialEdited";

		public static readonly StringName RefreshStepOptionLabel = "RefreshStepOptionLabel";

		public static readonly StringName DisposeComparisonVisualChoices = "DisposeComparisonVisualChoices";

		public static readonly StringName SetVisualButtonsDisabled = "SetVisualButtonsDisabled";

		public static readonly StringName BuildScopeLabel = "BuildScopeLabel";

		public static readonly StringName BuildStepLabel = "BuildStepLabel";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName StepCount = "StepCount";

		public static readonly StringName CurrentStepIndex = "CurrentStepIndex";

		public static readonly StringName ConditionCount = "ConditionCount";

		public static readonly StringName CanManageStepCollection = "CanManageStepCollection";

		public static readonly StringName IsStandaloneCondition = "IsStandaloneCondition";

		public static readonly StringName JsonSourcePicker = "JsonSourcePicker";

		public static readonly StringName CustomConditionPicker = "CustomConditionPicker";

		public static readonly StringName _config = "_config";

		public static readonly StringName _editingRoot = "_editingRoot";

		public static readonly StringName _editScope = "_editScope";

		public static readonly StringName _stepIndex = "_stepIndex";

		public static readonly StringName _undoRedo = "_undoRedo";

		public static readonly StringName _runtimeBroadcast = "_runtimeBroadcast";

		public static readonly StringName _runtimeBroadcastBar = "_runtimeBroadcastBar";

		public static readonly StringName _runtimeBroadcastLabel = "_runtimeBroadcastLabel";

		public static readonly StringName _broadcastTextEditor = "_broadcastTextEditor";

		public static readonly StringName _broadcastUse = "_broadcastUse";

		public static readonly StringName _broadcastDuration = "_broadcastDuration";

		public static readonly StringName _stepOption = "_stepOption";

		public static readonly StringName _saveKeyEdit = "_saveKeyEdit";

		public static readonly StringName _jsonSourcePickerHost = "_jsonSourcePickerHost";

		public static readonly StringName _jsonSourcePicker = "_jsonSourcePicker";

		public static readonly StringName _previousButton = "_previousButton";

		public static readonly StringName _nextButton = "_nextButton";

		public static readonly StringName _addStepButton = "_addStepButton";

		public static readonly StringName _duplicateStepButton = "_duplicateStepButton";

		public static readonly StringName _removeStepButton = "_removeStepButton";

		public static readonly StringName _moveStepUpButton = "_moveStepUpButton";

		public static readonly StringName _moveStepDownButton = "_moveStepDownButton";

		public static readonly StringName _stepCountBadge = "_stepCountBadge";

		public static readonly StringName _conditionType = "_conditionType";

		public static readonly StringName _conditionTypeVisualHost = "_conditionTypeVisualHost";

		public static readonly StringName _addConditionButton = "_addConditionButton";

		public static readonly StringName _customConditionPickerHost = "_customConditionPickerHost";

		public static readonly StringName _customConditionPicker = "_customConditionPicker";

		public static readonly StringName _addSelectedConditionButton = "_addSelectedConditionButton";

		public static readonly StringName _conditionRows = "_conditionRows";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _nextRootSnapshotId = "_nextRootSnapshotId";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
		public static readonly StringName TutorialEdited = "TutorialEdited";
	}

	private static readonly string[] ComparisonMethods = new string[5] { ">", ">=", "==", "<", "<=" };

	private const int MaxTutorialSteps = 256;

	private const int MaxVisibleConditionRows = 128;

	private const string ConditionRowScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWTutorialConditionRow.tscn";

	private const string CharacterIconPath = "res://addons/ModEditor/Icons/ResourceCharacter.svg";

	private const string SunIconPath = "res://Asset/Texture/GUI/Shop/Item/SunCollect.png";

	private const string AddIconPath = "res://addons/ModEditor/Icons/Add.svg";

	private const string DuplicateIconPath = "res://addons/ModEditor/Icons/Duplicate.svg";

	private const string RemoveIconPath = "res://addons/ModEditor/Icons/Remove.svg";

	private const string MoveUpIconPath = "res://addons/ModEditor/Icons/MoveUp.svg";

	private const string MoveDownIconPath = "res://addons/ModEditor/Icons/MoveDown.svg";

	private static PackedScene _conditionRowScene;

	private static Texture2D _characterIcon;

	private static Texture2D _sunIcon;

	private TutorialConfig _config;

	private Resource _editingRoot;

	private TutorialEditScope _editScope;

	private int _stepIndex;

	private XWUndoRedoManager _undoRedo;

	private XWVisualPropertyBinding _rootBinding;

	private XWVisualPropertyBinding _stepBinding;

	private BroadCastManager _runtimeBroadcast;

	private Control _runtimeBroadcastBar;

	private Label _runtimeBroadcastLabel;

	private TextEdit _broadcastTextEditor;

	private CheckButton _broadcastUse;

	private SpinBox _broadcastDuration;

	private OptionButton _stepOption;

	private XWVisualOptionGallery _stepVisualGallery;

	private LineEdit _saveKeyEdit;

	private VBoxContainer _jsonSourcePickerHost;

	private XWResourcePicker _jsonSourcePicker;

	private Button _previousButton;

	private Button _nextButton;

	private Button _addStepButton;

	private Button _duplicateStepButton;

	private Button _removeStepButton;

	private Button _moveStepUpButton;

	private Button _moveStepDownButton;

	private Label _stepCountBadge;

	private OptionButton _conditionType;

	private HFlowContainer _conditionTypeVisualHost;

	private XWVisualSegmentedOption _conditionTypeVisualChoices;

	private Button _addConditionButton;

	private VBoxContainer _customConditionPickerHost;

	private XWResourcePicker _customConditionPicker;

	private Button _addSelectedConditionButton;

	private VBoxContainer _conditionRows;

	private Label _statusLabel;

	private readonly List<XWVisualSegmentedOption> _comparisonVisualChoices = new List<XWVisualSegmentedOption>();

	private readonly List<XWVisualPropertyBinding> _conditionBindings = new List<XWVisualPropertyBinding>();

	private readonly System.Collections.Generic.Dictionary<long, TutorialRootSnapshot> _rootSnapshots = new System.Collections.Generic.Dictionary<long, TutorialRootSnapshot>();

	private long _nextRootSnapshotId = 1L;

	private bool _updatingControls;

	private TutorialEditedEventHandler backing_TutorialEdited;

	public int StepCount => (_config?.step?.Count).GetValueOrDefault();

	public int CurrentStepIndex => _stepIndex;

	public int ConditionCount => (GetCurrentStep()?.conditionList?.Count).GetValueOrDefault();

	public bool CanManageStepCollection => _editScope == TutorialEditScope.Tutorial;

	public bool IsStandaloneCondition => _editScope == TutorialEditScope.Condition;

	public XWResourcePicker JsonSourcePicker => _jsonSourcePicker;

	public XWResourcePicker CustomConditionPicker => _customConditionPicker;

	public event TutorialEditedEventHandler TutorialEdited
	{
		add
		{
			backing_TutorialEdited = (TutorialEditedEventHandler)Delegate.Combine(backing_TutorialEdited, value);
		}
		remove
		{
			backing_TutorialEdited = (TutorialEditedEventHandler)Delegate.Remove(backing_TutorialEdited, value);
		}
	}

	public override void _Ready()
	{
		_undoRedo = XWEditorInterface.Instance?.GetUndoRedoManager();
		_runtimeBroadcast = GetNode<BroadCastManager>("%RuntimeBroadcast");
		_runtimeBroadcastBar = _runtimeBroadcast.GetNode<Control>("%Broad");
		_runtimeBroadcastLabel = _runtimeBroadcast.GetNode<Label>("%BroadCastLabel");
		_runtimeBroadcastLabel.Visible = false;
		_broadcastTextEditor = GetNode<TextEdit>("%BroadcastTextEditor");
		_broadcastUse = GetNode<CheckButton>("%BroadcastUse");
		_broadcastDuration = GetNode<SpinBox>("%BroadcastDuration");
		_stepOption = GetNode<OptionButton>("%StepOption");
		_saveKeyEdit = GetNode<LineEdit>("%SaveKeyEdit");
		_jsonSourcePickerHost = GetNode<VBoxContainer>("%JsonSourcePickerHost");
		_previousButton = GetNode<Button>("%PreviousButton");
		_nextButton = GetNode<Button>("%NextButton");
		_addStepButton = GetNode<Button>("%AddStepButton");
		_duplicateStepButton = GetNode<Button>("%DuplicateStepButton");
		_removeStepButton = GetNode<Button>("%RemoveStepButton");
		_moveStepUpButton = GetNode<Button>("%MoveStepUpButton");
		_moveStepDownButton = GetNode<Button>("%MoveStepDownButton");
		_stepCountBadge = GetNode<Label>("%StepCountBadge");
		_conditionType = GetNode<OptionButton>("%ConditionType");
		_conditionTypeVisualHost = GetNode<HFlowContainer>("%ConditionTypeVisualChoices");
		_addConditionButton = GetNode<Button>("%AddConditionButton");
		_customConditionPickerHost = GetNode<VBoxContainer>("%CustomConditionPickerHost");
		_addSelectedConditionButton = GetNode<Button>("%AddSelectedConditionButton");
		_conditionRows = GetNode<VBoxContainer>("%ConditionRows");
		_statusLabel = GetNode<Label>("%Status");
		_jsonSourcePicker = XWResourcePicker.Create();
		_jsonSourcePicker.Name = "TutorialJsonSourcePicker";
		_jsonSourcePicker.Setup("Json", allowClear: true, allowNew: false);
		_jsonSourcePickerHost.AddChild(_jsonSourcePicker, forceReadableName: false, InternalMode.Disabled);
		_jsonSourcePicker.ResourceChanged += ApplyJsonSource;
		_customConditionPicker = XWResourcePicker.Create();
		_customConditionPicker.Name = "TutorialCustomConditionPicker";
		_customConditionPicker.Setup("TutorialConditionConfig");
		_customConditionPickerHost.AddChild(_customConditionPicker, forceReadableName: false, InternalMode.Disabled);
		_customConditionPicker.ResourceChanged += (Resource _) =>
		{
			UpdateActionAvailability();
		};
		SetButtonIcon(_addStepButton, "res://addons/ModEditor/Icons/Add.svg", "新增教程步骤");
		SetButtonIcon(_duplicateStepButton, "res://addons/ModEditor/Icons/Duplicate.svg", "复制当前步骤");
		SetButtonIcon(_removeStepButton, "res://addons/ModEditor/Icons/Remove.svg", "移除当前步骤");
		SetButtonIcon(_moveStepUpButton, "res://addons/ModEditor/Icons/MoveUp.svg", "上移当前步骤");
		SetButtonIcon(_moveStepDownButton, "res://addons/ModEditor/Icons/MoveDown.svg", "下移当前步骤");
		SetButtonIcon(_addConditionButton, "res://addons/ModEditor/Icons/Add.svg", "添加所选内置条件");
		SetButtonIcon(_addSelectedConditionButton, "res://addons/ModEditor/Icons/Duplicate.svg", "把所选条件资源作为独立副本加入当前步骤");
		_conditionType.AddItem("角色数量");
		_conditionType.AddItem("收集阳光");
		_stepVisualGallery = new XWVisualOptionGallery(_stepOption, GetNode<HFlowContainer>("%StepVisualChoices"), "TutorialStepVisualCatalog", (int _) => ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/FlowPort.svg", null, ResourceLoader.CacheMode.Reuse));
		_conditionTypeVisualChoices = new XWVisualSegmentedOption(_conditionType, _conditionTypeVisualHost, (int index) => ResourceLoader.Load<Texture2D>((index == 0) ? "res://addons/ModEditor/Icons/ResourceCharacter.svg" : "res://Asset/Texture/GUI/Shop/Item/SunCollect.png", null, ResourceLoader.CacheMode.Reuse));
		_conditionTypeVisualChoices.Rebuild();
		_previousButton.Pressed += ShowPrevious;
		_nextButton.Pressed += ShowNext;
		_addStepButton.Pressed += () =>
		{
			AddStep();
		};
		_duplicateStepButton.Pressed += () =>
		{
			DuplicateStep();
		};
		_removeStepButton.Pressed += () =>
		{
			RemoveStep();
		};
		_moveStepUpButton.Pressed += () =>
		{
			MoveStep(-1);
		};
		_moveStepDownButton.Pressed += () =>
		{
			MoveStep(1);
		};
		_stepOption.ItemSelected += SelectStep;
		_broadcastUse.Toggled += SetBroadcastUse;
		_addConditionButton.Pressed += AddCondition;
		_addSelectedConditionButton.Pressed += () =>
		{
			AddSelectedCondition();
		};
		RefreshFromConfig();
	}

	public override void _ExitTree()
	{
		_stepVisualGallery?.Dispose();
		_stepVisualGallery = null;
		_conditionTypeVisualChoices?.Dispose();
		_conditionTypeVisualChoices = null;
		_rootBinding?.Dispose();
		_stepBinding?.Dispose();
		_rootBinding = null;
		_stepBinding = null;
		DisposeConditionBindings();
		_rootSnapshots.Clear();
		DisposeComparisonVisualChoices();
		base._ExitTree();
	}

	public void EditConfig(TutorialConfig config, Resource editingRoot)
	{
		_config = config;
		_editingRoot = editingRoot;
		TutorialEditScope editScope = ((editingRoot is TutorialStepConfig) ? TutorialEditScope.Step : ((editingRoot is TutorialConditionConfig) ? TutorialEditScope.Condition : TutorialEditScope.Tutorial));
		_editScope = editScope;
		_stepIndex = 0;
		if (IsNodeReady())
		{
			RefreshFromConfig();
		}
	}

	public void SelectStep(long index)
	{
		int valueOrDefault = (_config?.step?.Count).GetValueOrDefault();
		if (valueOrDefault != 0)
		{
			_stepIndex = Mathf.Clamp((int)index, 0, valueOrDefault - 1);
			ShowCurrentStep();
		}
	}

	public void ShowPrevious()
	{
		if (_stepIndex > 0)
		{
			SelectStep(_stepIndex - 1);
		}
	}

	public void ShowNext()
	{
		int valueOrDefault = (_config?.step?.Count).GetValueOrDefault();
		if (_stepIndex + 1 < valueOrDefault)
		{
			SelectStep(_stepIndex + 1);
		}
	}

	public bool AddStep()
	{
		if (_editScope != TutorialEditScope.Tutorial || !GodotObject.IsInstanceValid(_config))
		{
			return false;
		}
		Array<TutorialStepConfig> array = new Array<TutorialStepConfig>(_config.step ?? new Array<TutorialStepConfig>());
		TutorialStepConfig item = new TutorialStepConfig
		{
			ResourceLocalToScene = true,
			ResourceName = $"TutorialStep_{array.Count + 1}",
			broadCastUse = true,
			broadCastConfig = CreateDefaultBroadcast()
		};
		array.Add(item);
		int doIndex = array.Count - 1;
		ReplaceStepList(array, "新增教程步骤", doIndex, _stepIndex);
		return StepCount == array.Count;
	}

	public bool DuplicateStep()
	{
		TutorialStepConfig currentStep = GetCurrentStep();
		if (_editScope != TutorialEditScope.Tutorial || !GodotObject.IsInstanceValid(currentStep))
		{
			return false;
		}
		TutorialStepConfig tutorialStepConfig = currentStep.Duplicate(deep: true) as TutorialStepConfig;
		if (!GodotObject.IsInstanceValid(tutorialStepConfig))
		{
			return false;
		}
		tutorialStepConfig.ResourceLocalToScene = true;
		tutorialStepConfig.ResourceName = (string.IsNullOrWhiteSpace(currentStep.ResourceName) ? $"TutorialStep_{_stepIndex + 2}" : (currentStep.ResourceName + "_Copy"));
		Array<TutorialStepConfig> array = new Array<TutorialStepConfig>(_config.step);
		int stepIndex = _stepIndex;
		int num = Math.Min(_stepIndex + 1, array.Count);
		array.Insert(num, tutorialStepConfig);
		ReplaceStepList(array, "复制教程步骤", num, stepIndex);
		return StepCount == array.Count;
	}

	public bool RemoveStep()
	{
		if (_editScope != TutorialEditScope.Tutorial || !GodotObject.IsInstanceValid(GetCurrentStep()))
		{
			return false;
		}
		Array<TutorialStepConfig> array = new Array<TutorialStepConfig>(_config.step);
		int stepIndex = _stepIndex;
		array.RemoveAt(_stepIndex);
		int doIndex = ((array.Count != 0) ? Math.Min(_stepIndex, array.Count - 1) : 0);
		ReplaceStepList(array, "移除教程步骤", doIndex, stepIndex);
		return StepCount == array.Count;
	}

	public bool MoveStepUp()
	{
		return MoveStep(-1);
	}

	public bool MoveStepDown()
	{
		return MoveStep(1);
	}

	private bool MoveStep(int direction)
	{
		if (_editScope != TutorialEditScope.Tutorial || !GodotObject.IsInstanceValid(GetCurrentStep()))
		{
			return false;
		}
		int num = _stepIndex + Math.Sign(direction);
		if (num < 0 || num >= StepCount)
		{
			return false;
		}
		Array<TutorialStepConfig> array = new Array<TutorialStepConfig>(_config.step);
		TutorialStepConfig value = array[_stepIndex];
		array[_stepIndex] = array[num];
		array[num] = value;
		int stepIndex = _stepIndex;
		ReplaceStepList(array, (direction < 0) ? "上移教程步骤" : "下移教程步骤", num, stepIndex);
		return _stepIndex == num;
	}

	public void SetBroadcastUse(bool enabled)
	{
		if (_updatingControls || _editScope == TutorialEditScope.Condition)
		{
			return;
		}
		TutorialStepConfig currentStep = GetCurrentStep();
		if (GodotObject.IsInstanceValid(currentStep) && currentStep.broadCastUse != enabled)
		{
			BroadCastConfig broadCastConfig = currentStep.broadCastConfig;
			BroadCastConfig broadCastConfig2 = ((enabled && !GodotObject.IsInstanceValid(broadCastConfig)) ? CreateDefaultBroadcast() : broadCastConfig);
			if (_undoRedo == null)
			{
				currentStep.broadCastUse = enabled;
				currentStep.broadCastConfig = broadCastConfig2;
				ShowCurrentStep();
				EmitTutorialEdited(currentStep, broadCastConfig2);
				return;
			}
			_undoRedo.CreateAction(enabled ? "启用教程广播" : "关闭教程广播");
			_undoRedo.AddDoProperty(currentStep, "broadCastUse", enabled);
			_undoRedo.AddDoProperty(currentStep, "broadCastConfig", broadCastConfig2);
			_undoRedo.AddUndoProperty(currentStep, "broadCastUse", currentStep.broadCastUse);
			_undoRedo.AddUndoProperty(currentStep, "broadCastConfig", broadCastConfig);
			_undoRedo.AddDoMethod(this, "RefreshTutorialPropertiesFromHistory");
			_undoRedo.AddUndoMethod(this, "RefreshTutorialPropertiesFromHistory");
			_undoRedo.CommitAction();
			ShowCurrentStep();
			EmitTutorialEdited(currentStep, broadCastConfig2);
		}
	}

	public void SetBroadcastText(string value)
	{
		if (_updatingControls || _editScope == TutorialEditScope.Condition)
		{
			return;
		}
		TutorialStepConfig currentStep = GetCurrentStep();
		if (GodotObject.IsInstanceValid(currentStep))
		{
			TutorialStepConfig tutorialStepConfig = currentStep;
			if (tutorialStepConfig.broadCastConfig == null)
			{
				tutorialStepConfig.broadCastConfig = CreateDefaultBroadcast();
			}
			SetResourceProperty(currentStep.broadCastConfig, "broadCastString", value ?? "", "修改教程广播文本", currentStep);
		}
	}

	public void SetBroadcastDuration(double value)
	{
		if (_updatingControls || _editScope == TutorialEditScope.Condition)
		{
			return;
		}
		TutorialStepConfig currentStep = GetCurrentStep();
		if (GodotObject.IsInstanceValid(currentStep))
		{
			TutorialStepConfig tutorialStepConfig = currentStep;
			if (tutorialStepConfig.broadCastConfig == null)
			{
				tutorialStepConfig.broadCastConfig = CreateDefaultBroadcast();
			}
			SetResourceProperty(currentStep.broadCastConfig, "broadCastTime", value, "修改教程广播时长", currentStep);
		}
	}

	public void SetCharacterName(TutorialConditionCheckCharaterNum condition, string value)
	{
		if (!_updatingControls && CanEditCondition(condition))
		{
			SetResourceProperty(condition, "characterName", value ?? "", "修改教程目标角色", GetCurrentStep());
		}
	}

	public void SetComparisonMethod(TutorialConditionCheckCharaterNum condition, string value)
	{
		if (!_updatingControls && CanEditCondition(condition) && System.Array.IndexOf(ComparisonMethods, value) >= 0)
		{
			SetResourceProperty(condition, "method", value, "修改教程比较方式", GetCurrentStep());
		}
	}

	public void SetConditionTarget(TutorialConditionConfig condition, double value)
	{
		if (_updatingControls || !CanEditCondition(condition))
		{
			return;
		}
		int num = Math.Max(0, (int)Math.Round(value));
		if (!(condition is TutorialConditionCheckCharaterNum resource))
		{
			if (condition is TutorialConditionCheckSunCollect resource2)
			{
				SetResourceProperty(resource2, "num", num, "修改教程阳光目标", GetCurrentStep());
			}
		}
		else
		{
			SetResourceProperty(resource, "num", num, "修改教程角色数量", GetCurrentStep());
		}
	}

	public void AddCondition()
	{
		if (_editScope != TutorialEditScope.Condition)
		{
			TutorialStepConfig currentStep = GetCurrentStep();
			if (GodotObject.IsInstanceValid(currentStep) && (currentStep.conditionList?.Count ?? 0) < 128)
			{
				TutorialConditionConfig tutorialConditionConfig = ((_conditionType.Selected == 1) ? ((TutorialConditionConfig)new TutorialConditionCheckSunCollect()) : ((TutorialConditionConfig)new TutorialConditionCheckCharaterNum()));
				tutorialConditionConfig.ResourceLocalToScene = true;
				tutorialConditionConfig.ResourceName = $"Condition_{(currentStep.conditionList?.Count ?? 0) + 1}";
				AddConditionResource(tutorialConditionConfig, "添加内置教程条件");
			}
		}
	}

	public bool AddSelectedCondition()
	{
		if (_editScope == TutorialEditScope.Condition || !(_customConditionPicker?.EditedResource is TutorialConditionConfig tutorialConditionConfig))
		{
			return false;
		}
		TutorialConditionConfig tutorialConditionConfig2 = tutorialConditionConfig.Duplicate(deep: true) as TutorialConditionConfig;
		if (!GodotObject.IsInstanceValid(tutorialConditionConfig2))
		{
			return false;
		}
		tutorialConditionConfig2.ResourceLocalToScene = true;
		tutorialConditionConfig2.ResourceName = (string.IsNullOrWhiteSpace(tutorialConditionConfig.ResourceName) ? (tutorialConditionConfig.GetType().Name + "_Copy") : (tutorialConditionConfig.ResourceName + "_Copy"));
		return AddConditionResource(tutorialConditionConfig2, "添加自定义教程条件副本");
	}

	public bool DuplicateCondition(TutorialConditionConfig condition)
	{
		TutorialStepConfig currentStep = GetCurrentStep();
		if (_editScope == TutorialEditScope.Condition || !GodotObject.IsInstanceValid(currentStep) || !GodotObject.IsInstanceValid(condition) || currentStep.conditionList == null)
		{
			return false;
		}
		int num = currentStep.conditionList.IndexOf(condition);
		if (num < 0 || currentStep.conditionList.Count >= 128)
		{
			return false;
		}
		TutorialConditionConfig tutorialConditionConfig = condition.Duplicate(deep: true) as TutorialConditionConfig;
		if (!GodotObject.IsInstanceValid(tutorialConditionConfig))
		{
			return false;
		}
		tutorialConditionConfig.ResourceLocalToScene = true;
		tutorialConditionConfig.ResourceName = (string.IsNullOrWhiteSpace(condition.ResourceName) ? (condition.GetType().Name + "_Copy") : (condition.ResourceName + "_Copy"));
		Array<TutorialConditionConfig> array = new Array<TutorialConditionConfig>(currentStep.conditionList);
		array.Insert(num + 1, tutorialConditionConfig);
		ReplaceConditionList(currentStep, array, "复制教程条件");
		return currentStep.conditionList.Count == array.Count;
	}

	public bool MoveCondition(TutorialConditionConfig condition, int direction)
	{
		TutorialStepConfig currentStep = GetCurrentStep();
		if (_editScope == TutorialEditScope.Condition || !GodotObject.IsInstanceValid(currentStep) || !GodotObject.IsInstanceValid(condition) || currentStep.conditionList == null)
		{
			return false;
		}
		int num = currentStep.conditionList.IndexOf(condition);
		int num2 = num + Math.Sign(direction);
		if (num < 0 || num2 < 0 || num2 >= currentStep.conditionList.Count)
		{
			return false;
		}
		Array<TutorialConditionConfig> array = new Array<TutorialConditionConfig>(currentStep.conditionList);
		array[num] = array[num2];
		array[num2] = condition;
		ReplaceConditionList(currentStep, array, (direction < 0) ? "上移教程条件" : "下移教程条件");
		return true;
	}

	public void RemoveCondition(TutorialConditionConfig condition)
	{
		if (_editScope == TutorialEditScope.Condition || !GodotObject.IsInstanceValid(condition))
		{
			return;
		}
		TutorialStepConfig currentStep = GetCurrentStep();
		if (GodotObject.IsInstanceValid(currentStep) && currentStep.conditionList != null)
		{
			Array<TutorialConditionConfig> array = new Array<TutorialConditionConfig>(currentStep.conditionList);
			if (array.Remove(condition))
			{
				ReplaceConditionList(currentStep, array, "移除教程条件");
			}
		}
	}

	private void RefreshFromConfig()
	{
		if (!IsNodeReady())
		{
			return;
		}
		_rootBinding?.Dispose();
		_rootBinding = null;
		_updatingControls = true;
		_saveKeyEdit.Text = (GodotObject.IsInstanceValid(_config) ? (_config.saveKey ?? "") : "");
		_saveKeyEdit.Editable = _editScope == TutorialEditScope.Tutorial && GodotObject.IsInstanceValid(_config);
		_jsonSourcePickerHost.Visible = _editScope == TutorialEditScope.Tutorial;
		_jsonSourcePicker?.SetEditedResource((_editScope != TutorialEditScope.Tutorial) ? null : _config?.data);
		_stepOption.Clear();
		int valueOrDefault = (_config?.step?.Count).GetValueOrDefault();
		for (int i = 0; i < valueOrDefault; i++)
		{
			_stepOption.AddItem(BuildStepLabel(i, _config.step[i]));
		}
		_stepVisualGallery?.Rebuild();
		_updatingControls = false;
		if (_editScope == TutorialEditScope.Tutorial && GodotObject.IsInstanceValid(_config))
		{
			_rootBinding = new XWVisualPropertyBinding(_undoRedo, (bool _) =>
			{
				EmitTutorialEdited(_config);
			});
			_rootBinding.BindText(_saveKeyEdit, _config, "saveKey", RefreshStatus, this, "RefreshTutorialPropertiesFromHistory");
		}
		_stepIndex = ((valueOrDefault != 0) ? Mathf.Clamp(_stepIndex, 0, valueOrDefault - 1) : 0);
		ShowCurrentStep();
	}

	private void ShowCurrentStep()
	{
		_stepBinding?.Dispose();
		_stepBinding = null;
		TutorialStepConfig step = GetCurrentStep();
		int valueOrDefault = (_config?.step?.Count).GetValueOrDefault();
		bool flag = GodotObject.IsInstanceValid(step);
		_updatingControls = true;
		_stepOption.Disabled = valueOrDefault == 0;
		_previousButton.Disabled = !flag || _stepIndex <= 0;
		_nextButton.Disabled = !flag || _stepIndex >= valueOrDefault - 1;
		_stepCountBadge.Text = $"{valueOrDefault} 步";
		if (flag)
		{
			_stepOption.Select(_stepIndex);
		}
		_stepVisualGallery?.RefreshSelection();
		_stepVisualGallery?.SetDisabled(valueOrDefault == 0, "当前教程没有可选步骤");
		bool flag2 = flag && _editScope != TutorialEditScope.Condition;
		bool flag3 = flag && step.broadCastUse;
		_broadcastUse.ButtonPressed = flag3;
		_broadcastUse.Disabled = !flag2;
		_broadcastTextEditor.Text = ((flag && GodotObject.IsInstanceValid(step.broadCastConfig)) ? (step.broadCastConfig.broadCastString ?? "") : "");
		_broadcastTextEditor.Editable = flag2 & flag3;
		_broadcastDuration.Value = ((flag && GodotObject.IsInstanceValid(step.broadCastConfig)) ? step.broadCastConfig.broadCastTime : (-1.0));
		_broadcastDuration.Editable = flag2 & flag3;
		_runtimeBroadcastBar.Visible = flag;
		_runtimeBroadcastBar.Modulate = (flag3 ? Colors.White : new Color(1f, 1f, 1f, 0.45f));
		_broadcastTextEditor.Visible = flag;
		_conditionType.Disabled = !flag2;
		SetVisualButtonsDisabled(_conditionTypeVisualHost, !flag2);
		_addConditionButton.Disabled = !flag2 || (step?.conditionList?.Count).GetValueOrDefault() >= 128;
		_updatingControls = false;
		if (flag2 && GodotObject.IsInstanceValid(step.broadCastConfig))
		{
			_stepBinding = new XWVisualPropertyBinding(_undoRedo, (bool _) =>
			{
				EmitTutorialEdited(step, step.broadCastConfig);
			});
			_stepBinding.BindText(_broadcastTextEditor, step.broadCastConfig, "broadCastString", RefreshStepOptionLabel, this, "RefreshTutorialPropertiesFromHistory");
			_stepBinding.BindNumber(_broadcastDuration, step.broadCastConfig, "broadCastTime", null, this, "RefreshTutorialPropertiesFromHistory");
		}
		RefreshConditionRows();
		UpdateActionAvailability();
		RefreshStatus();
	}

	private void RefreshConditionRows()
	{
		if (!GodotObject.IsInstanceValid(_conditionRows))
		{
			return;
		}
		DisposeConditionBindings();
		DisposeComparisonVisualChoices();
		foreach (Node child in _conditionRows.GetChildren())
		{
			_conditionRows.RemoveChild(child);
			child.QueueFree();
		}
		TutorialStepConfig currentStep = GetCurrentStep();
		int valueOrDefault = (currentStep?.conditionList?.Count).GetValueOrDefault();
		if (valueOrDefault == 0)
		{
			_conditionRows.AddChild(new Label
			{
				Text = "无完成条件：该步骤可直接推进。",
				HorizontalAlignment = HorizontalAlignment.Center
			}, forceReadableName: false, InternalMode.Disabled);
			return;
		}
		if (_conditionRowScene == null)
		{
			_conditionRowScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWTutorialConditionRow.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		int num = Math.Min(valueOrDefault, 128);
		for (int i = 0; i < num; i++)
		{
			TutorialConditionConfig tutorialConditionConfig = currentStep.conditionList[i];
			if (GodotObject.IsInstanceValid(tutorialConditionConfig))
			{
				PanelContainer panelContainer = _conditionRowScene?.Instantiate<PanelContainer>(PackedScene.GenEditState.Disabled);
				if (GodotObject.IsInstanceValid(panelContainer))
				{
					_conditionRows.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
					BindConditionRow(panelContainer, tutorialConditionConfig, i);
				}
			}
		}
	}

	private void BindConditionRow(PanelContainer row, TutorialConditionConfig condition, int index)
	{
		TextureRect node = row.GetNode<TextureRect>("%Icon");
		Label node2 = row.GetNode<Label>("%TypeTitle");
		Label node3 = row.GetNode<Label>("%CharacterLabel");
		LineEdit node4 = row.GetNode<LineEdit>("%CharacterName");
		OptionButton node5 = row.GetNode<OptionButton>("%Comparison");
		HFlowContainer node6 = row.GetNode<HFlowContainer>("%ComparisonVisualChoices");
		Label node7 = row.GetNode<Label>("%TargetLabel");
		SpinBox node8 = row.GetNode<SpinBox>("%Target");
		Label node9 = row.GetNode<Label>("%UnitLabel");
		HBoxContainer node10 = row.GetNode<HBoxContainer>("%Values");
		VBoxContainer node11 = row.GetNode<VBoxContainer>("%CustomPropertyHost");
		Button node12 = row.GetNode<Button>("%MoveUpButton");
		Button node13 = row.GetNode<Button>("%MoveDownButton");
		Button node14 = row.GetNode<Button>("%DuplicateButton");
		Button node15 = row.GetNode<Button>("%RemoveButton");
		bool flag = CanEditCondition(condition);
		node5.Clear();
		string[] comparisonMethods = ComparisonMethods;
		foreach (string label in comparisonMethods)
		{
			node5.AddItem(label);
		}
		XWVisualSegmentedOption xWVisualSegmentedOption = new XWVisualSegmentedOption(node5, node6);
		xWVisualSegmentedOption.Rebuild();
		_comparisonVisualChoices.Add(xWVisualSegmentedOption);
		bool flag2 = _editScope != TutorialEditScope.Condition;
		node12.Icon = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/MoveUp.svg", null, ResourceLoader.CacheMode.Reuse);
		node13.Icon = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/MoveDown.svg", null, ResourceLoader.CacheMode.Reuse);
		node14.Icon = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Duplicate.svg", null, ResourceLoader.CacheMode.Reuse);
		node15.Icon = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Remove.svg", null, ResourceLoader.CacheMode.Reuse);
		node12.Disabled = !flag2 || index <= 0;
		node13.Disabled = !flag2 || index >= (GetCurrentStep()?.conditionList?.Count).GetValueOrDefault() - 1;
		node14.Disabled = !flag2 || (GetCurrentStep()?.conditionList?.Count).GetValueOrDefault() >= 128;
		node15.Disabled = !flag2;
		node15.TooltipText = (node15.Disabled ? "独立条件资源只能编辑自身，不能从预览包装步骤中移除" : "从当前步骤移除该条件");
		node12.Pressed += () =>
		{
			MoveCondition(condition, -1);
		};
		node13.Pressed += () =>
		{
			MoveCondition(condition, 1);
		};
		node14.Pressed += () =>
		{
			DuplicateCondition(condition);
		};
		node15.Pressed += () =>
		{
			RemoveCondition(condition);
		};
		TutorialConditionCheckCharaterNum tutorialConditionCheckCharaterNum = condition as TutorialConditionCheckCharaterNum;
		if (tutorialConditionCheckCharaterNum == null)
		{
			TutorialConditionCheckSunCollect tutorialConditionCheckSunCollect = condition as TutorialConditionCheckSunCollect;
			if (tutorialConditionCheckSunCollect != null)
			{
				if (_sunIcon == null)
				{
					_sunIcon = ResourceLoader.Load<Texture2D>("res://Asset/Texture/GUI/Shop/Item/SunCollect.png", null, ResourceLoader.CacheMode.Reuse);
				}
				node.Texture = _sunIcon;
				node2.Text = $"条件 {index + 1} · 收集阳光";
				node3.Visible = false;
				node4.Visible = false;
				node5.Visible = false;
				node6.Visible = false;
				node7.Text = "累计收集";
				node8.Value = tutorialConditionCheckSunCollect.num;
				node8.Editable = flag;
				node9.Text = "阳光";
				XWVisualPropertyBinding xWVisualPropertyBinding = new XWVisualPropertyBinding(_undoRedo, (bool _) =>
				{
					EmitTutorialEdited(GetCurrentStep(), tutorialConditionCheckSunCollect);
				});
				xWVisualPropertyBinding.BindNumber(node8, tutorialConditionCheckSunCollect, "num", null, this, "RefreshTutorialPropertiesFromHistory");
				_conditionBindings.Add(xWVisualPropertyBinding);
			}
			else
			{
				node2.Text = $"条件 {index + 1} · {condition.GetType().Name}";
				node10.Visible = false;
				node11.Visible = true;
				XWDirectPropertySurface xWDirectPropertySurface = new XWDirectPropertySurface
				{
					Name = $"CustomConditionProperties_{index + 1}",
					SizeFlagsHorizontal = SizeFlags.ExpandFill
				};
				xWDirectPropertySurface.PropertyEdited += (GodotObject _, StringName _, StringName _, Variant _) =>
				{
					EmitTutorialEdited(GetCurrentStep(), condition);
				};
				node11.AddChild(xWDirectPropertySurface, forceReadableName: false, InternalMode.Disabled);
				xWDirectPropertySurface.BindResource(condition, _undoRedo);
			}
		}
		else
		{
			if (_characterIcon == null)
			{
				_characterIcon = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ResourceCharacter.svg", null, ResourceLoader.CacheMode.Reuse);
			}
			node.Texture = _characterIcon;
			node2.Text = $"条件 {index + 1} · 场上角色数量";
			node4.Text = tutorialConditionCheckCharaterNum.characterName ?? "";
			node4.Editable = flag;
			int idx = Math.Max(0, System.Array.IndexOf(ComparisonMethods, tutorialConditionCheckCharaterNum.method));
			node5.Select(idx);
			xWVisualSegmentedOption.RefreshSelection();
			node5.Disabled = !flag;
			SetVisualButtonsDisabled(node6, !flag);
			node8.Value = tutorialConditionCheckCharaterNum.num;
			node8.Editable = flag;
			XWVisualPropertyBinding xWVisualPropertyBinding2 = new XWVisualPropertyBinding(_undoRedo, (bool _) =>
			{
				EmitTutorialEdited(GetCurrentStep(), tutorialConditionCheckCharaterNum);
			});
			xWVisualPropertyBinding2.BindText(node4, tutorialConditionCheckCharaterNum, "characterName", null, this, "RefreshTutorialPropertiesFromHistory");
			xWVisualPropertyBinding2.BindNumber(node8, tutorialConditionCheckCharaterNum, "num", null, this, "RefreshTutorialPropertiesFromHistory");
			_conditionBindings.Add(xWVisualPropertyBinding2);
			node5.ItemSelected += (long selected) =>
			{
				SetComparisonMethod(tutorialConditionCheckCharaterNum, ComparisonMethods[Mathf.Clamp((int)selected, 0, ComparisonMethods.Length - 1)]);
			};
		}
	}

	private void ApplyJsonSource(Resource resource)
	{
		if (_updatingControls || _editScope != TutorialEditScope.Tutorial || !GodotObject.IsInstanceValid(_config))
		{
			return;
		}
		Json json = resource as Json;
		if (_config.data != json)
		{
			TutorialConfig tutorialConfig = new TutorialConfig
			{
				data = json
			};
			long num = StoreRootSnapshot(_config.data, _config.saveKey, _config.step, _stepIndex);
			long num2 = StoreRootSnapshot(json, tutorialConfig.saveKey, tutorialConfig.step, 0);
			if (_undoRedo == null)
			{
				ApplyTutorialRootSnapshot(num2);
				return;
			}
			_undoRedo.CreateAction((json == null) ? "清除教程 JSON 来源" : "更换教程 JSON 来源");
			_undoRedo.AddDoMethod(this, "ApplyTutorialRootSnapshot", num2);
			_undoRedo.AddUndoMethod(this, "ApplyTutorialRootSnapshot", num);
			_undoRedo.CommitAction();
		}
	}

	private long StoreRootSnapshot(Json data, string saveKey, Array<TutorialStepConfig> steps, int selectedIndex)
	{
		long num = _nextRootSnapshotId++;
		_rootSnapshots[num] = new TutorialRootSnapshot
		{
			Data = data,
			SaveKey = (saveKey ?? ""),
			Steps = new Array<TutorialStepConfig>(steps ?? new Array<TutorialStepConfig>()),
			SelectedIndex = selectedIndex
		};
		return num;
	}

	public void ApplyTutorialRootSnapshot(long snapshotId)
	{
		if (GodotObject.IsInstanceValid(_config) && _rootSnapshots.TryGetValue(snapshotId, out var value))
		{
			_config.Set("data", value.Data);
			_config.Set("saveKey", value.SaveKey);
			_config.Set("step", new Array<TutorialStepConfig>(value.Steps));
			_stepIndex = value.SelectedIndex;
			RefreshFromConfig();
			EmitTutorialEdited(_config);
		}
	}

	private bool AddConditionResource(TutorialConditionConfig condition, string actionName)
	{
		TutorialStepConfig currentStep = GetCurrentStep();
		if (_editScope == TutorialEditScope.Condition || !GodotObject.IsInstanceValid(currentStep) || !GodotObject.IsInstanceValid(condition) || (currentStep.conditionList?.Count ?? 0) >= 128)
		{
			return false;
		}
		Array<TutorialConditionConfig> array = new Array<TutorialConditionConfig>(currentStep.conditionList ?? new Array<TutorialConditionConfig>());
		array.Add(condition);
		ReplaceConditionList(currentStep, array, actionName);
		return currentStep.conditionList.Count == array.Count;
	}

	private void ReplaceConditionList(TutorialStepConfig step, Array<TutorialConditionConfig> next, string actionName)
	{
		if (GodotObject.IsInstanceValid(step) && next != null)
		{
			Array<TutorialConditionConfig> from = new Array<TutorialConditionConfig>(step.conditionList ?? new Array<TutorialConditionConfig>());
			if (_undoRedo == null)
			{
				step.conditionList = next;
				RefreshConditionRows();
				UpdateActionAvailability();
				RefreshStatus();
				EmitTutorialEdited(step);
			}
			else
			{
				_undoRedo.CreateAction(actionName);
				_undoRedo.AddDoProperty(step, "conditionList", Variant.From(in next));
				_undoRedo.AddUndoProperty(step, "conditionList", Variant.From(in from));
				_undoRedo.AddDoMethod(this, "RefreshConditionCollectionFromHistory");
				_undoRedo.AddUndoMethod(this, "RefreshConditionCollectionFromHistory");
				_undoRedo.CommitAction();
			}
		}
	}

	private void ReplaceStepList(Array<TutorialStepConfig> next, string actionName, int doIndex, int undoIndex)
	{
		if (_editScope == TutorialEditScope.Tutorial && GodotObject.IsInstanceValid(_config) && next != null)
		{
			Array<TutorialStepConfig> from = new Array<TutorialStepConfig>(_config.step ?? new Array<TutorialStepConfig>());
			if (_undoRedo == null)
			{
				_config.step = next;
				RefreshTutorialCollectionFromHistory(doIndex);
				return;
			}
			_undoRedo.CreateAction(actionName);
			_undoRedo.AddDoProperty(_config, "step", Variant.From(in next));
			_undoRedo.AddUndoProperty(_config, "step", Variant.From(in from));
			_undoRedo.AddDoMethod(this, "RefreshTutorialCollectionFromHistory", doIndex);
			_undoRedo.AddUndoMethod(this, "RefreshTutorialCollectionFromHistory", undoIndex);
			_undoRedo.CommitAction();
		}
	}

	public void RefreshTutorialCollectionFromHistory(long selectedIndex)
	{
		_stepIndex = (int)selectedIndex;
		RefreshFromConfig();
		EmitTutorialEdited(_config);
	}

	public void RefreshConditionCollectionFromHistory()
	{
		RefreshConditionRows();
		UpdateActionAvailability();
		RefreshStatus();
		EmitTutorialEdited(GetCurrentStep());
	}

	public void RefreshTutorialPropertiesFromHistory()
	{
		if (_undoRedo != null && (_undoRedo.IsUndoing() || _undoRedo.IsRedoing()))
		{
			RefreshFromConfig();
			EmitTutorialEdited(GetCurrentStep());
		}
	}

	private void SetResourceProperty(Resource resource, StringName property, Variant value, string actionName, params Resource[] relatedResources)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return;
		}
		Variant value2 = resource.Get(property);
		if (!value2.Equals(value))
		{
			if (_undoRedo == null)
			{
				resource.Set(property, value);
			}
			else
			{
				_undoRedo.CreateAction(actionName);
				_undoRedo.AddDoProperty(resource, property, value);
				_undoRedo.AddUndoProperty(resource, property, value2);
				_undoRedo.AddDoMethod(this, "RefreshTutorialPropertiesFromHistory");
				_undoRedo.AddUndoMethod(this, "RefreshTutorialPropertiesFromHistory");
				_undoRedo.CommitAction();
			}
			List<Resource> list = new List<Resource> { resource };
			if (relatedResources != null)
			{
				list.AddRange(relatedResources);
			}
			EmitTutorialEdited(list.ToArray());
			RefreshStepOptionLabel();
			RefreshStatus();
		}
	}

	private void UpdateActionAvailability()
	{
		if (IsNodeReady())
		{
			bool flag = _editScope == TutorialEditScope.Tutorial && GodotObject.IsInstanceValid(_config);
			bool flag2 = GodotObject.IsInstanceValid(GetCurrentStep());
			_addStepButton.Disabled = !flag || StepCount >= 256;
			_duplicateStepButton.Disabled = !flag || !flag2 || StepCount >= 256;
			_removeStepButton.Disabled = !flag || !flag2;
			_moveStepUpButton.Disabled = !flag || !flag2 || _stepIndex <= 0;
			_moveStepDownButton.Disabled = !flag || !flag2 || _stepIndex >= StepCount - 1;
			bool flag3 = flag2 && _editScope != TutorialEditScope.Condition;
			bool flag4 = ConditionCount < 128;
			_customConditionPickerHost.Visible = _editScope != TutorialEditScope.Condition;
			_addSelectedConditionButton.Visible = _editScope != TutorialEditScope.Condition;
			_addSelectedConditionButton.Disabled = !flag3 || !flag4 || !(_customConditionPicker?.EditedResource is TutorialConditionConfig);
		}
	}

	private void RefreshStatus()
	{
		if (GodotObject.IsInstanceValid(_statusLabel))
		{
			TutorialStepConfig currentStep = GetCurrentStep();
			if (!GodotObject.IsInstanceValid(currentStep))
			{
				_statusLabel.Text = ((_editScope == TutorialEditScope.Tutorial) ? "当前教程还没有步骤，点击上方“＋”即可直接创建第一步。" : "当前独立资源预览没有可编辑步骤。");
				return;
			}
			_statusLabel.Text = $"步骤 {_stepIndex + 1} / {StepCount} · 广播 {(currentStep.broadCastUse ? "启用" : "关闭")} · 条件 {currentStep.conditionList?.Count ?? 0} 个 · {BuildScopeLabel()}";
		}
	}

	private void DisposeConditionBindings()
	{
		foreach (XWVisualPropertyBinding conditionBinding in _conditionBindings)
		{
			conditionBinding.Dispose();
		}
		_conditionBindings.Clear();
	}

	private static BroadCastConfig CreateDefaultBroadcast()
	{
		return new BroadCastConfig
		{
			ResourceLocalToScene = true,
			broadCastString = "点击这里编辑新的教程提示",
			broadCastTime = -1.0
		};
	}

	private static void SetButtonIcon(Button button, string path, string tooltip)
	{
		if (GodotObject.IsInstanceValid(button))
		{
			button.Icon = ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse);
			button.TooltipText = tooltip;
		}
	}

	private bool CanEditCondition(TutorialConditionConfig condition)
	{
		if (!GodotObject.IsInstanceValid(condition))
		{
			return false;
		}
		if (_editScope != TutorialEditScope.Condition)
		{
			return true;
		}
		return _editingRoot == condition;
	}

	private TutorialStepConfig GetCurrentStep()
	{
		int valueOrDefault = (_config?.step?.Count).GetValueOrDefault();
		if (valueOrDefault <= 0 || _stepIndex < 0 || _stepIndex >= valueOrDefault)
		{
			return null;
		}
		return _config.step[_stepIndex];
	}

	private void EmitTutorialEdited(params Resource[] changedResources)
	{
		foreach (Resource resource in changedResources)
		{
			if (GodotObject.IsInstanceValid(resource))
			{
				resource.EmitChanged();
			}
		}
		if (GodotObject.IsInstanceValid(_config))
		{
			_config.EmitChanged();
		}
		if (GodotObject.IsInstanceValid(_editingRoot) && _editingRoot != _config)
		{
			_editingRoot.EmitChanged();
		}
		EmitSignal(SignalName.TutorialEdited);
	}

	private void RefreshStepOptionLabel()
	{
		TutorialStepConfig currentStep = GetCurrentStep();
		if (GodotObject.IsInstanceValid(currentStep) && _stepIndex >= 0 && _stepIndex < _stepOption.ItemCount)
		{
			_stepOption.SetItemText(_stepIndex, BuildStepLabel(_stepIndex, currentStep));
		}
		_stepVisualGallery?.Rebuild();
	}

	private void DisposeComparisonVisualChoices()
	{
		foreach (XWVisualSegmentedOption comparisonVisualChoice in _comparisonVisualChoices)
		{
			comparisonVisualChoice.Dispose();
		}
		_comparisonVisualChoices.Clear();
	}

	private static void SetVisualButtonsDisabled(HFlowContainer host, bool disabled)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return;
		}
		foreach (Node child in host.GetChildren())
		{
			if (child is Button button)
			{
				button.Disabled = disabled;
			}
		}
	}

	private string BuildScopeLabel()
	{
		return _editScope switch
		{
			TutorialEditScope.Step => "独立步骤资源", 
			TutorialEditScope.Condition => "独立条件资源", 
			_ => "完整教程资源", 
		};
	}

	private static string BuildStepLabel(int index, TutorialStepConfig step)
	{
		if (GodotObject.IsInstanceValid(step))
		{
			string text = step.broadCastConfig?.broadCastString ?? "";
			text = text.Replace('\n', ' ').Replace('\r', ' ').StripEdges();
			if (text.Length > 22)
			{
				text = text.Substring(0, 22) + "…";
			}
			return $"步骤 {index + 1} · {(step.broadCastUse ? text : "无广播")}";
		}
		return $"步骤 {index + 1} · 空资源";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(50)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EditConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "editingRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SelectStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowPrevious, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowNext, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddStep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DuplicateStep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveStep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveStepUp, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveStepDown, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveStep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetBroadcastUse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetBroadcastText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetBroadcastDuration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCharacterName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetComparisonMethod, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetConditionTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddSelectedCondition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DuplicateCondition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.MoveCondition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshFromConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowCurrentStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshConditionRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindConditionRow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "row", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyJsonSource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.StoreRootSnapshot, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false),
				new PropertyInfo(Variant.Type.String, "saveKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "steps", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "selectedIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyTutorialRootSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "snapshotId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddConditionResource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceConditionList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "step", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceStepList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "doIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "undoIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshTutorialCollectionFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "selectedIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshConditionCollectionFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshTutorialPropertiesFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetResourceProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "relatedResources", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateActionAvailability, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeConditionBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateDefaultBroadcast, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.SetButtonIcon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanEditCondition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentStep, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitTutorialEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "changedResources", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshStepOptionLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeComparisonVisualChoices, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetVisualButtonsDisabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "disabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildScopeLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildStepLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "step", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.EditConfig && args.Count == 2)
		{
			EditConfig(VariantUtils.ConvertTo<TutorialConfig>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectStep && args.Count == 1)
		{
			SelectStep(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowPrevious && args.Count == 0)
		{
			ShowPrevious();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowNext && args.Count == 0)
		{
			ShowNext();
			ret = default;
			return true;
		}
		if (method == MethodName.AddStep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(AddStep());
			return true;
		}
		if (method == MethodName.DuplicateStep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(DuplicateStep());
			return true;
		}
		if (method == MethodName.RemoveStep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveStep());
			return true;
		}
		if (method == MethodName.MoveStepUp && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(MoveStepUp());
			return true;
		}
		if (method == MethodName.MoveStepDown && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(MoveStepDown());
			return true;
		}
		if (method == MethodName.MoveStep && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MoveStep(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetBroadcastUse && args.Count == 1)
		{
			SetBroadcastUse(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetBroadcastText && args.Count == 1)
		{
			SetBroadcastText(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetBroadcastDuration && args.Count == 1)
		{
			SetBroadcastDuration(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCharacterName && args.Count == 2)
		{
			SetCharacterName(VariantUtils.ConvertTo<TutorialConditionCheckCharaterNum>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetComparisonMethod && args.Count == 2)
		{
			SetComparisonMethod(VariantUtils.ConvertTo<TutorialConditionCheckCharaterNum>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetConditionTarget && args.Count == 2)
		{
			SetConditionTarget(VariantUtils.ConvertTo<TutorialConditionConfig>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddCondition && args.Count == 0)
		{
			AddCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.AddSelectedCondition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(AddSelectedCondition());
			return true;
		}
		if (method == MethodName.DuplicateCondition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(DuplicateCondition(VariantUtils.ConvertTo<TutorialConditionConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.MoveCondition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(MoveCondition(VariantUtils.ConvertTo<TutorialConditionConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.RemoveCondition && args.Count == 1)
		{
			RemoveCondition(VariantUtils.ConvertTo<TutorialConditionConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFromConfig && args.Count == 0)
		{
			RefreshFromConfig();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCurrentStep && args.Count == 0)
		{
			ShowCurrentStep();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshConditionRows && args.Count == 0)
		{
			RefreshConditionRows();
			ret = default;
			return true;
		}
		if (method == MethodName.BindConditionRow && args.Count == 3)
		{
			BindConditionRow(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<TutorialConditionConfig>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyJsonSource && args.Count == 1)
		{
			ApplyJsonSource(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StoreRootSnapshot && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<long>(StoreRootSnapshot(VariantUtils.ConvertTo<Json>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertToArray<TutorialStepConfig>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		if (method == MethodName.ApplyTutorialRootSnapshot && args.Count == 1)
		{
			ApplyTutorialRootSnapshot(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddConditionResource && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AddConditionResource(VariantUtils.ConvertTo<TutorialConditionConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReplaceConditionList && args.Count == 3)
		{
			ReplaceConditionList(VariantUtils.ConvertTo<TutorialStepConfig>(in args[0]), VariantUtils.ConvertToArray<TutorialConditionConfig>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceStepList && args.Count == 4)
		{
			ReplaceStepList(VariantUtils.ConvertToArray<TutorialStepConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshTutorialCollectionFromHistory && args.Count == 1)
		{
			RefreshTutorialCollectionFromHistory(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshConditionCollectionFromHistory && args.Count == 0)
		{
			RefreshConditionCollectionFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshTutorialPropertiesFromHistory && args.Count == 0)
		{
			RefreshTutorialPropertiesFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.SetResourceProperty && args.Count == 5)
		{
			SetResourceProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertToSystemArrayOfGodotObject<Resource>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateActionAvailability && args.Count == 0)
		{
			UpdateActionAvailability();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshStatus && args.Count == 0)
		{
			RefreshStatus();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeConditionBindings && args.Count == 0)
		{
			DisposeConditionBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateDefaultBroadcast && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<BroadCastConfig>(CreateDefaultBroadcast());
			return true;
		}
		if (method == MethodName.SetButtonIcon && args.Count == 3)
		{
			SetButtonIcon(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanEditCondition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanEditCondition(VariantUtils.ConvertTo<TutorialConditionConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurrentStep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TutorialStepConfig>(GetCurrentStep());
			return true;
		}
		if (method == MethodName.EmitTutorialEdited && args.Count == 1)
		{
			EmitTutorialEdited(VariantUtils.ConvertToSystemArrayOfGodotObject<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshStepOptionLabel && args.Count == 0)
		{
			RefreshStepOptionLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeComparisonVisualChoices && args.Count == 0)
		{
			DisposeComparisonVisualChoices();
			ret = default;
			return true;
		}
		if (method == MethodName.SetVisualButtonsDisabled && args.Count == 2)
		{
			SetVisualButtonsDisabled(VariantUtils.ConvertTo<HFlowContainer>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildScopeLabel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildScopeLabel());
			return true;
		}
		if (method == MethodName.BuildStepLabel && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildStepLabel(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TutorialStepConfig>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateDefaultBroadcast && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<BroadCastConfig>(CreateDefaultBroadcast());
			return true;
		}
		if (method == MethodName.SetButtonIcon && args.Count == 3)
		{
			SetButtonIcon(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetVisualButtonsDisabled && args.Count == 2)
		{
			SetVisualButtonsDisabled(VariantUtils.ConvertTo<HFlowContainer>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildStepLabel && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildStepLabel(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TutorialStepConfig>(in args[1])));
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
		if (method == MethodName.EditConfig)
		{
			return true;
		}
		if (method == MethodName.SelectStep)
		{
			return true;
		}
		if (method == MethodName.ShowPrevious)
		{
			return true;
		}
		if (method == MethodName.ShowNext)
		{
			return true;
		}
		if (method == MethodName.AddStep)
		{
			return true;
		}
		if (method == MethodName.DuplicateStep)
		{
			return true;
		}
		if (method == MethodName.RemoveStep)
		{
			return true;
		}
		if (method == MethodName.MoveStepUp)
		{
			return true;
		}
		if (method == MethodName.MoveStepDown)
		{
			return true;
		}
		if (method == MethodName.MoveStep)
		{
			return true;
		}
		if (method == MethodName.SetBroadcastUse)
		{
			return true;
		}
		if (method == MethodName.SetBroadcastText)
		{
			return true;
		}
		if (method == MethodName.SetBroadcastDuration)
		{
			return true;
		}
		if (method == MethodName.SetCharacterName)
		{
			return true;
		}
		if (method == MethodName.SetComparisonMethod)
		{
			return true;
		}
		if (method == MethodName.SetConditionTarget)
		{
			return true;
		}
		if (method == MethodName.AddCondition)
		{
			return true;
		}
		if (method == MethodName.AddSelectedCondition)
		{
			return true;
		}
		if (method == MethodName.DuplicateCondition)
		{
			return true;
		}
		if (method == MethodName.MoveCondition)
		{
			return true;
		}
		if (method == MethodName.RemoveCondition)
		{
			return true;
		}
		if (method == MethodName.RefreshFromConfig)
		{
			return true;
		}
		if (method == MethodName.ShowCurrentStep)
		{
			return true;
		}
		if (method == MethodName.RefreshConditionRows)
		{
			return true;
		}
		if (method == MethodName.BindConditionRow)
		{
			return true;
		}
		if (method == MethodName.ApplyJsonSource)
		{
			return true;
		}
		if (method == MethodName.StoreRootSnapshot)
		{
			return true;
		}
		if (method == MethodName.ApplyTutorialRootSnapshot)
		{
			return true;
		}
		if (method == MethodName.AddConditionResource)
		{
			return true;
		}
		if (method == MethodName.ReplaceConditionList)
		{
			return true;
		}
		if (method == MethodName.ReplaceStepList)
		{
			return true;
		}
		if (method == MethodName.RefreshTutorialCollectionFromHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshConditionCollectionFromHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshTutorialPropertiesFromHistory)
		{
			return true;
		}
		if (method == MethodName.SetResourceProperty)
		{
			return true;
		}
		if (method == MethodName.UpdateActionAvailability)
		{
			return true;
		}
		if (method == MethodName.RefreshStatus)
		{
			return true;
		}
		if (method == MethodName.DisposeConditionBindings)
		{
			return true;
		}
		if (method == MethodName.CreateDefaultBroadcast)
		{
			return true;
		}
		if (method == MethodName.SetButtonIcon)
		{
			return true;
		}
		if (method == MethodName.CanEditCondition)
		{
			return true;
		}
		if (method == MethodName.GetCurrentStep)
		{
			return true;
		}
		if (method == MethodName.EmitTutorialEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshStepOptionLabel)
		{
			return true;
		}
		if (method == MethodName.DisposeComparisonVisualChoices)
		{
			return true;
		}
		if (method == MethodName.SetVisualButtonsDisabled)
		{
			return true;
		}
		if (method == MethodName.BuildScopeLabel)
		{
			return true;
		}
		if (method == MethodName.BuildStepLabel)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._config)
		{
			_config = VariantUtils.ConvertTo<TutorialConfig>(in value);
			return true;
		}
		if (name == PropertyName._editingRoot)
		{
			_editingRoot = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._editScope)
		{
			_editScope = VariantUtils.ConvertTo<TutorialEditScope>(in value);
			return true;
		}
		if (name == PropertyName._stepIndex)
		{
			_stepIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._undoRedo)
		{
			_undoRedo = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName._runtimeBroadcast)
		{
			_runtimeBroadcast = VariantUtils.ConvertTo<BroadCastManager>(in value);
			return true;
		}
		if (name == PropertyName._runtimeBroadcastBar)
		{
			_runtimeBroadcastBar = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._runtimeBroadcastLabel)
		{
			_runtimeBroadcastLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._broadcastTextEditor)
		{
			_broadcastTextEditor = VariantUtils.ConvertTo<TextEdit>(in value);
			return true;
		}
		if (name == PropertyName._broadcastUse)
		{
			_broadcastUse = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._broadcastDuration)
		{
			_broadcastDuration = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._stepOption)
		{
			_stepOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._saveKeyEdit)
		{
			_saveKeyEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._jsonSourcePickerHost)
		{
			_jsonSourcePickerHost = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._jsonSourcePicker)
		{
			_jsonSourcePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._previousButton)
		{
			_previousButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._nextButton)
		{
			_nextButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._addStepButton)
		{
			_addStepButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._duplicateStepButton)
		{
			_duplicateStepButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._removeStepButton)
		{
			_removeStepButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveStepUpButton)
		{
			_moveStepUpButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveStepDownButton)
		{
			_moveStepDownButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stepCountBadge)
		{
			_stepCountBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._conditionType)
		{
			_conditionType = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._conditionTypeVisualHost)
		{
			_conditionTypeVisualHost = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._addConditionButton)
		{
			_addConditionButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._customConditionPickerHost)
		{
			_customConditionPickerHost = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._customConditionPicker)
		{
			_customConditionPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._addSelectedConditionButton)
		{
			_addSelectedConditionButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._conditionRows)
		{
			_conditionRows = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._nextRootSnapshotId)
		{
			_nextRootSnapshotId = VariantUtils.ConvertTo<long>(in value);
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
		int from;
		if (name == PropertyName.StepCount)
		{
			from = StepCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CurrentStepIndex)
		{
			from = CurrentStepIndex;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ConditionCount)
		{
			from = ConditionCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.CanManageStepCollection)
		{
			from2 = CanManageStepCollection;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsStandaloneCondition)
		{
			from2 = IsStandaloneCondition;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		XWResourcePicker from3;
		if (name == PropertyName.JsonSourcePicker)
		{
			from3 = JsonSourcePicker;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CustomConditionPicker)
		{
			from3 = CustomConditionPicker;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName._config)
		{
			value = VariantUtils.CreateFrom(in _config);
			return true;
		}
		if (name == PropertyName._editingRoot)
		{
			value = VariantUtils.CreateFrom(in _editingRoot);
			return true;
		}
		if (name == PropertyName._editScope)
		{
			value = VariantUtils.CreateFrom(in _editScope);
			return true;
		}
		if (name == PropertyName._stepIndex)
		{
			value = VariantUtils.CreateFrom(in _stepIndex);
			return true;
		}
		if (name == PropertyName._undoRedo)
		{
			value = VariantUtils.CreateFrom(in _undoRedo);
			return true;
		}
		if (name == PropertyName._runtimeBroadcast)
		{
			value = VariantUtils.CreateFrom(in _runtimeBroadcast);
			return true;
		}
		if (name == PropertyName._runtimeBroadcastBar)
		{
			value = VariantUtils.CreateFrom(in _runtimeBroadcastBar);
			return true;
		}
		if (name == PropertyName._runtimeBroadcastLabel)
		{
			value = VariantUtils.CreateFrom(in _runtimeBroadcastLabel);
			return true;
		}
		if (name == PropertyName._broadcastTextEditor)
		{
			value = VariantUtils.CreateFrom(in _broadcastTextEditor);
			return true;
		}
		if (name == PropertyName._broadcastUse)
		{
			value = VariantUtils.CreateFrom(in _broadcastUse);
			return true;
		}
		if (name == PropertyName._broadcastDuration)
		{
			value = VariantUtils.CreateFrom(in _broadcastDuration);
			return true;
		}
		if (name == PropertyName._stepOption)
		{
			value = VariantUtils.CreateFrom(in _stepOption);
			return true;
		}
		if (name == PropertyName._saveKeyEdit)
		{
			value = VariantUtils.CreateFrom(in _saveKeyEdit);
			return true;
		}
		if (name == PropertyName._jsonSourcePickerHost)
		{
			value = VariantUtils.CreateFrom(in _jsonSourcePickerHost);
			return true;
		}
		if (name == PropertyName._jsonSourcePicker)
		{
			value = VariantUtils.CreateFrom(in _jsonSourcePicker);
			return true;
		}
		if (name == PropertyName._previousButton)
		{
			value = VariantUtils.CreateFrom(in _previousButton);
			return true;
		}
		if (name == PropertyName._nextButton)
		{
			value = VariantUtils.CreateFrom(in _nextButton);
			return true;
		}
		if (name == PropertyName._addStepButton)
		{
			value = VariantUtils.CreateFrom(in _addStepButton);
			return true;
		}
		if (name == PropertyName._duplicateStepButton)
		{
			value = VariantUtils.CreateFrom(in _duplicateStepButton);
			return true;
		}
		if (name == PropertyName._removeStepButton)
		{
			value = VariantUtils.CreateFrom(in _removeStepButton);
			return true;
		}
		if (name == PropertyName._moveStepUpButton)
		{
			value = VariantUtils.CreateFrom(in _moveStepUpButton);
			return true;
		}
		if (name == PropertyName._moveStepDownButton)
		{
			value = VariantUtils.CreateFrom(in _moveStepDownButton);
			return true;
		}
		if (name == PropertyName._stepCountBadge)
		{
			value = VariantUtils.CreateFrom(in _stepCountBadge);
			return true;
		}
		if (name == PropertyName._conditionType)
		{
			value = VariantUtils.CreateFrom(in _conditionType);
			return true;
		}
		if (name == PropertyName._conditionTypeVisualHost)
		{
			value = VariantUtils.CreateFrom(in _conditionTypeVisualHost);
			return true;
		}
		if (name == PropertyName._addConditionButton)
		{
			value = VariantUtils.CreateFrom(in _addConditionButton);
			return true;
		}
		if (name == PropertyName._customConditionPickerHost)
		{
			value = VariantUtils.CreateFrom(in _customConditionPickerHost);
			return true;
		}
		if (name == PropertyName._customConditionPicker)
		{
			value = VariantUtils.CreateFrom(in _customConditionPicker);
			return true;
		}
		if (name == PropertyName._addSelectedConditionButton)
		{
			value = VariantUtils.CreateFrom(in _addSelectedConditionButton);
			return true;
		}
		if (name == PropertyName._conditionRows)
		{
			value = VariantUtils.CreateFrom(in _conditionRows);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._nextRootSnapshotId)
		{
			value = VariantUtils.CreateFrom(in _nextRootSnapshotId);
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
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._editScope, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._stepIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._undoRedo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeBroadcast, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeBroadcastBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeBroadcastLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._broadcastTextEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._broadcastUse, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._broadcastDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stepOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveKeyEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jsonSourcePickerHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jsonSourcePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addStepButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._duplicateStepButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._removeStepButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveStepUpButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveStepDownButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stepCountBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._conditionType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._conditionTypeVisualHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addConditionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._customConditionPickerHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._customConditionPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addSelectedConditionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._conditionRows, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextRootSnapshotId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.StepCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CurrentStepIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ConditionCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanManageStepCollection, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsStandaloneCondition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.JsonSourcePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CustomConditionPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._config, Variant.From(in _config));
		info.AddProperty(PropertyName._editingRoot, Variant.From(in _editingRoot));
		info.AddProperty(PropertyName._editScope, Variant.From(in _editScope));
		info.AddProperty(PropertyName._stepIndex, Variant.From(in _stepIndex));
		info.AddProperty(PropertyName._undoRedo, Variant.From(in _undoRedo));
		info.AddProperty(PropertyName._runtimeBroadcast, Variant.From(in _runtimeBroadcast));
		info.AddProperty(PropertyName._runtimeBroadcastBar, Variant.From(in _runtimeBroadcastBar));
		info.AddProperty(PropertyName._runtimeBroadcastLabel, Variant.From(in _runtimeBroadcastLabel));
		info.AddProperty(PropertyName._broadcastTextEditor, Variant.From(in _broadcastTextEditor));
		info.AddProperty(PropertyName._broadcastUse, Variant.From(in _broadcastUse));
		info.AddProperty(PropertyName._broadcastDuration, Variant.From(in _broadcastDuration));
		info.AddProperty(PropertyName._stepOption, Variant.From(in _stepOption));
		info.AddProperty(PropertyName._saveKeyEdit, Variant.From(in _saveKeyEdit));
		info.AddProperty(PropertyName._jsonSourcePickerHost, Variant.From(in _jsonSourcePickerHost));
		info.AddProperty(PropertyName._jsonSourcePicker, Variant.From(in _jsonSourcePicker));
		info.AddProperty(PropertyName._previousButton, Variant.From(in _previousButton));
		info.AddProperty(PropertyName._nextButton, Variant.From(in _nextButton));
		info.AddProperty(PropertyName._addStepButton, Variant.From(in _addStepButton));
		info.AddProperty(PropertyName._duplicateStepButton, Variant.From(in _duplicateStepButton));
		info.AddProperty(PropertyName._removeStepButton, Variant.From(in _removeStepButton));
		info.AddProperty(PropertyName._moveStepUpButton, Variant.From(in _moveStepUpButton));
		info.AddProperty(PropertyName._moveStepDownButton, Variant.From(in _moveStepDownButton));
		info.AddProperty(PropertyName._stepCountBadge, Variant.From(in _stepCountBadge));
		info.AddProperty(PropertyName._conditionType, Variant.From(in _conditionType));
		info.AddProperty(PropertyName._conditionTypeVisualHost, Variant.From(in _conditionTypeVisualHost));
		info.AddProperty(PropertyName._addConditionButton, Variant.From(in _addConditionButton));
		info.AddProperty(PropertyName._customConditionPickerHost, Variant.From(in _customConditionPickerHost));
		info.AddProperty(PropertyName._customConditionPicker, Variant.From(in _customConditionPicker));
		info.AddProperty(PropertyName._addSelectedConditionButton, Variant.From(in _addSelectedConditionButton));
		info.AddProperty(PropertyName._conditionRows, Variant.From(in _conditionRows));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._nextRootSnapshotId, Variant.From(in _nextRootSnapshotId));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddSignalEventDelegate(SignalName.TutorialEdited, backing_TutorialEdited);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._config, out var value))
		{
			_config = value.As<TutorialConfig>();
		}
		if (info.TryGetProperty(PropertyName._editingRoot, out var value2))
		{
			_editingRoot = value2.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._editScope, out var value3))
		{
			_editScope = value3.As<TutorialEditScope>();
		}
		if (info.TryGetProperty(PropertyName._stepIndex, out var value4))
		{
			_stepIndex = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._undoRedo, out var value5))
		{
			_undoRedo = value5.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._runtimeBroadcast, out var value6))
		{
			_runtimeBroadcast = value6.As<BroadCastManager>();
		}
		if (info.TryGetProperty(PropertyName._runtimeBroadcastBar, out var value7))
		{
			_runtimeBroadcastBar = value7.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._runtimeBroadcastLabel, out var value8))
		{
			_runtimeBroadcastLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._broadcastTextEditor, out var value9))
		{
			_broadcastTextEditor = value9.As<TextEdit>();
		}
		if (info.TryGetProperty(PropertyName._broadcastUse, out var value10))
		{
			_broadcastUse = value10.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._broadcastDuration, out var value11))
		{
			_broadcastDuration = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._stepOption, out var value12))
		{
			_stepOption = value12.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._saveKeyEdit, out var value13))
		{
			_saveKeyEdit = value13.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._jsonSourcePickerHost, out var value14))
		{
			_jsonSourcePickerHost = value14.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._jsonSourcePicker, out var value15))
		{
			_jsonSourcePicker = value15.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._previousButton, out var value16))
		{
			_previousButton = value16.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nextButton, out var value17))
		{
			_nextButton = value17.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._addStepButton, out var value18))
		{
			_addStepButton = value18.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._duplicateStepButton, out var value19))
		{
			_duplicateStepButton = value19.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._removeStepButton, out var value20))
		{
			_removeStepButton = value20.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveStepUpButton, out var value21))
		{
			_moveStepUpButton = value21.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveStepDownButton, out var value22))
		{
			_moveStepDownButton = value22.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stepCountBadge, out var value23))
		{
			_stepCountBadge = value23.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._conditionType, out var value24))
		{
			_conditionType = value24.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._conditionTypeVisualHost, out var value25))
		{
			_conditionTypeVisualHost = value25.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._addConditionButton, out var value26))
		{
			_addConditionButton = value26.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._customConditionPickerHost, out var value27))
		{
			_customConditionPickerHost = value27.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._customConditionPicker, out var value28))
		{
			_customConditionPicker = value28.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._addSelectedConditionButton, out var value29))
		{
			_addSelectedConditionButton = value29.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._conditionRows, out var value30))
		{
			_conditionRows = value30.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value31))
		{
			_statusLabel = value31.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._nextRootSnapshotId, out var value32))
		{
			_nextRootSnapshotId = value32.As<long>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value33))
		{
			_updatingControls = value33.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<TutorialEditedEventHandler>(SignalName.TutorialEdited, out var value34))
		{
			backing_TutorialEdited = value34;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.TutorialEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalTutorialEdited()
	{
		EmitSignal(SignalName.TutorialEdited, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.TutorialEdited && args.Count == 0)
		{
			backing_TutorialEdited?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.TutorialEdited)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
