using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWTutorialPreviewWindow.cs")]
public class XWTutorialPreviewWindow : Window
{
	public new class MethodName : Window.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Preview = "Preview";

		public static readonly StringName RefreshConfig = "RefreshConfig";

		public static readonly StringName BindInterface = "BindInterface";

		public static readonly StringName RefreshConfigView = "RefreshConfigView";

		public static readonly StringName ShowCurrentStep = "ShowCurrentStep";

		public static readonly StringName CompleteCurrentStep = "CompleteCurrentStep";

		public static readonly StringName PreviousStep = "PreviousStep";

		public static readonly StringName NextStep = "NextStep";

		public static readonly StringName ToggleAutoPlay = "ToggleAutoPlay";

		public static readonly StringName ScheduleAuto = "ScheduleAuto";

		public static readonly StringName StopAutoPlay = "StopAutoPlay";

		public static readonly StringName ClosePreview = "ClosePreview";

		public static readonly StringName DescribeCondition = "DescribeCondition";

		public static readonly StringName PreviewText = "PreviewText";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : Window.PropertyName
	{
		public static readonly StringName _config = "_config";

		public static readonly StringName _stepIndex = "_stepIndex";

		public static readonly StringName _autoPlaying = "_autoPlaying";

		public static readonly StringName _stepList = "_stepList";

		public static readonly StringName _titleLabel = "_titleLabel";

		public static readonly StringName _broadcastLabel = "_broadcastLabel";

		public static readonly StringName _broadcastManager = "_broadcastManager";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _conditionRows = "_conditionRows";

		public static readonly StringName _previousButton = "_previousButton";

		public static readonly StringName _nextButton = "_nextButton";

		public static readonly StringName _autoButton = "_autoButton";

		public static readonly StringName _autoTimer = "_autoTimer";

		public static readonly StringName _updatingSelection = "_updatingSelection";
	}

	public new class SignalName : Window.SignalName
	{
	}

	private TutorialConfig _config;

	private int _stepIndex;

	private bool _autoPlaying;

	private ItemList _stepList;

	private Label _titleLabel;

	private Label _broadcastLabel;

	private BroadCastManager _broadcastManager;

	private Label _statusLabel;

	private VBoxContainer _conditionRows;

	private Button _previousButton;

	private Button _nextButton;

	private Button _autoButton;

	private Timer _autoTimer;

	private bool _updatingSelection;

	public override void _Ready()
	{
		CloseRequested += ClosePreview;
		BindInterface();
		RefreshConfigView();
	}

	public void Preview(TutorialConfig config)
	{
		_config = config;
		_stepIndex = 0;
		if (IsNodeReady())
		{
			RefreshConfigView();
			PopupCenteredClamped(new Vector2I(960, 640), 0.9f);
			GrabFocus();
		}
	}

	public void RefreshConfig(TutorialConfig config)
	{
		_config = config;
		if (IsNodeReady())
		{
			RefreshConfigView();
		}
	}

	private void BindInterface()
	{
		_titleLabel = GetNode<Label>("Layout/Title");
		_stepList = GetNode<ItemList>("Layout/Split/StepList");
		_broadcastManager = GetNode<BroadCastManager>("%RuntimeBroadcast");
		_broadcastLabel = _broadcastManager.GetNode<Label>("%BroadCastLabel");
		_conditionRows = GetNode<VBoxContainer>("Layout/Split/Right/ConditionScroll/ConditionRows");
		_previousButton = GetNode<Button>("Layout/Split/Right/Controls/PreviousButton");
		Button node = GetNode<Button>("Layout/Split/Right/Controls/CompleteButton");
		_autoButton = GetNode<Button>("Layout/Split/Right/Controls/AutoButton");
		_nextButton = GetNode<Button>("Layout/Split/Right/Controls/NextButton");
		_statusLabel = GetNode<Label>("Layout/Split/Right/Status");
		_autoTimer = GetNode<Timer>("AutoTimer");
		_stepList.ItemSelected += (long index) =>
		{
			if (!_updatingSelection)
			{
				StopAutoPlay();
				_stepIndex = (int)index;
				ShowCurrentStep();
			}
		};
		_previousButton.Pressed += PreviousStep;
		node.Pressed += CompleteCurrentStep;
		_autoButton.Pressed += ToggleAutoPlay;
		_nextButton.Pressed += NextStep;
		_autoTimer.Timeout += NextStep;
	}

	private void RefreshConfigView()
	{
		_stepList.Clear();
		int valueOrDefault = (_config?.step?.Count).GetValueOrDefault();
		for (int i = 0; i < valueOrDefault; i++)
		{
			TutorialStepConfig tutorialStepConfig = _config.step[i];
			string value = ((GodotObject.IsInstanceValid(tutorialStepConfig) && tutorialStepConfig.broadCastUse) ? PreviewText(tutorialStepConfig.broadCastConfig?.broadCastString) : "无广播");
			_stepList.AddItem($"步骤 {i + 1}  ·  {value}");
		}
		_titleLabel.Text = "教程流程运行预览  ·  " + EmptyToPlaceholder(_config?.saveKey);
		_stepIndex = ((valueOrDefault != 0) ? Mathf.Clamp(_stepIndex, 0, valueOrDefault - 1) : 0);
		ShowCurrentStep();
	}

	private void ShowCurrentStep()
	{
		_broadcastManager?.BraodCastClear();
		foreach (Node child in _conditionRows.GetChildren())
		{
			child.QueueFree();
		}
		int valueOrDefault = (_config?.step?.Count).GetValueOrDefault();
		if (valueOrDefault == 0)
		{
			_broadcastLabel.Text = "尚未添加教程步骤";
			_statusLabel.Text = "0 / 0";
			_previousButton.Disabled = true;
			_nextButton.Disabled = true;
			return;
		}
		_stepIndex = Mathf.Clamp(_stepIndex, 0, valueOrDefault - 1);
		TutorialStepConfig tutorialStepConfig = _config.step[_stepIndex];
		bool flag = GodotObject.IsInstanceValid(tutorialStepConfig);
		if (flag && tutorialStepConfig.broadCastUse && GodotObject.IsInstanceValid(tutorialStepConfig.broadCastConfig))
		{
			_broadcastManager?.BroadCastAdd(tutorialStepConfig.broadCastConfig);
		}
		else
		{
			_broadcastLabel.Text = "（该步骤无广播文本）";
		}
		if (flag && tutorialStepConfig.conditionList != null)
		{
			for (int i = 0; i < tutorialStepConfig.conditionList.Count; i++)
			{
				TutorialConditionConfig condition = tutorialStepConfig.conditionList[i];
				_conditionRows.AddChild(new Label
				{
					Text = $"{i + 1}. {DescribeCondition(condition)}",
					AutowrapMode = TextServer.AutowrapMode.WordSmart
				}, forceReadableName: false, InternalMode.Disabled);
			}
		}
		if (_conditionRows.GetChildCount() == 0)
		{
			_conditionRows.AddChild(new Label
			{
				Text = "无条件：该步骤可直接完成"
			}, forceReadableName: false, InternalMode.Disabled);
		}
		_updatingSelection = true;
		_stepList.Select(_stepIndex);
		_stepList.EnsureCurrentIsVisible();
		_updatingSelection = false;
		_previousButton.Disabled = _stepIndex <= 0;
		_nextButton.Disabled = _stepIndex >= valueOrDefault - 1;
		double num = ((flag && tutorialStepConfig.broadCastUse && GodotObject.IsInstanceValid(tutorialStepConfig.broadCastConfig)) ? tutorialStepConfig.broadCastConfig.broadCastTime : (-1.0));
		_statusLabel.Text = $"{_stepIndex + 1} / {valueOrDefault}    广播时长: {((num < 0.0) ? "手动继续" : $"{num:0.##} 秒")}    条件: {(tutorialStepConfig?.conditionList?.Count).GetValueOrDefault()}";
		ScheduleAuto(num);
	}

	private void CompleteCurrentStep()
	{
		NextStep();
	}

	private void PreviousStep()
	{
		StopAutoPlay();
		if (_stepIndex > 0)
		{
			_stepIndex--;
			ShowCurrentStep();
		}
	}

	private void NextStep()
	{
		int valueOrDefault = (_config?.step?.Count).GetValueOrDefault();
		if (_stepIndex + 1 >= valueOrDefault)
		{
			StopAutoPlay();
			_statusLabel.Text = $"教程预览完成，共 {valueOrDefault} 个步骤";
		}
		else
		{
			_stepIndex++;
			ShowCurrentStep();
		}
	}

	private void ToggleAutoPlay()
	{
		if (_autoPlaying)
		{
			StopAutoPlay();
			return;
		}
		_autoPlaying = true;
		_stepIndex = 0;
		_autoButton.Text = "暂停播放";
		ShowCurrentStep();
	}

	private void ScheduleAuto(double configuredDuration)
	{
		if (_autoPlaying)
		{
			_autoTimer.Start((configuredDuration > 0.0) ? configuredDuration : 2.5);
		}
	}

	private void StopAutoPlay()
	{
		_autoPlaying = false;
		_autoTimer.Stop();
		_autoButton.Text = "自动播放";
	}

	private void ClosePreview()
	{
		StopAutoPlay();
		Hide();
	}

	private static string DescribeCondition(TutorialConditionConfig condition)
	{
		if (!(condition is TutorialConditionCheckSunCollect tutorialConditionCheckSunCollect))
		{
			if (!(condition is TutorialConditionCheckCharaterNum tutorialConditionCheckCharaterNum))
			{
				if (condition == null)
				{
					return "空条件";
				}
				return condition.GetType().Name;
			}
			return $"角色 {EmptyToPlaceholder(tutorialConditionCheckCharaterNum.characterName)} 数量 {tutorialConditionCheckCharaterNum.method} {tutorialConditionCheckCharaterNum.num}";
		}
		return $"收集阳光 ≥ {tutorialConditionCheckSunCollect.num}";
	}

	private static string PreviewText(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return "空文本";
		}
		string text2 = text.Replace('\n', ' ').Replace('\r', ' ');
		if (text2.Length <= 24)
		{
			return text2;
		}
		return text2.Substring(0, 24) + "…";
	}

	private static string EmptyToPlaceholder(string text)
	{
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return "未设置";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Preview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindInterface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshConfigView, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowCurrentStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteCurrentStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviousStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleAutoPlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleAuto, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "configuredDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StopAutoPlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClosePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DescribeCondition, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PreviewText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmptyToPlaceholder, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Preview && args.Count == 1)
		{
			Preview(VariantUtils.ConvertTo<TutorialConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshConfig && args.Count == 1)
		{
			RefreshConfig(VariantUtils.ConvertTo<TutorialConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindInterface && args.Count == 0)
		{
			BindInterface();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshConfigView && args.Count == 0)
		{
			RefreshConfigView();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCurrentStep && args.Count == 0)
		{
			ShowCurrentStep();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteCurrentStep && args.Count == 0)
		{
			CompleteCurrentStep();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviousStep && args.Count == 0)
		{
			PreviousStep();
			ret = default;
			return true;
		}
		if (method == MethodName.NextStep && args.Count == 0)
		{
			NextStep();
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleAutoPlay && args.Count == 0)
		{
			ToggleAutoPlay();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleAuto && args.Count == 1)
		{
			ScheduleAuto(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StopAutoPlay && args.Count == 0)
		{
			StopAutoPlay();
			ret = default;
			return true;
		}
		if (method == MethodName.ClosePreview && args.Count == 0)
		{
			ClosePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.DescribeCondition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DescribeCondition(VariantUtils.ConvertTo<TutorialConditionConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.PreviewText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(PreviewText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyToPlaceholder(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.DescribeCondition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DescribeCondition(VariantUtils.ConvertTo<TutorialConditionConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.PreviewText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(PreviewText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyToPlaceholder(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.Preview)
		{
			return true;
		}
		if (method == MethodName.RefreshConfig)
		{
			return true;
		}
		if (method == MethodName.BindInterface)
		{
			return true;
		}
		if (method == MethodName.RefreshConfigView)
		{
			return true;
		}
		if (method == MethodName.ShowCurrentStep)
		{
			return true;
		}
		if (method == MethodName.CompleteCurrentStep)
		{
			return true;
		}
		if (method == MethodName.PreviousStep)
		{
			return true;
		}
		if (method == MethodName.NextStep)
		{
			return true;
		}
		if (method == MethodName.ToggleAutoPlay)
		{
			return true;
		}
		if (method == MethodName.ScheduleAuto)
		{
			return true;
		}
		if (method == MethodName.StopAutoPlay)
		{
			return true;
		}
		if (method == MethodName.ClosePreview)
		{
			return true;
		}
		if (method == MethodName.DescribeCondition)
		{
			return true;
		}
		if (method == MethodName.PreviewText)
		{
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder)
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
		if (name == PropertyName._stepIndex)
		{
			_stepIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._autoPlaying)
		{
			_autoPlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._stepList)
		{
			_stepList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			_titleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._broadcastLabel)
		{
			_broadcastLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._broadcastManager)
		{
			_broadcastManager = VariantUtils.ConvertTo<BroadCastManager>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._conditionRows)
		{
			_conditionRows = VariantUtils.ConvertTo<VBoxContainer>(in value);
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
		if (name == PropertyName._autoButton)
		{
			_autoButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._autoTimer)
		{
			_autoTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._updatingSelection)
		{
			_updatingSelection = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._config)
		{
			value = VariantUtils.CreateFrom(in _config);
			return true;
		}
		if (name == PropertyName._stepIndex)
		{
			value = VariantUtils.CreateFrom(in _stepIndex);
			return true;
		}
		if (name == PropertyName._autoPlaying)
		{
			value = VariantUtils.CreateFrom(in _autoPlaying);
			return true;
		}
		if (name == PropertyName._stepList)
		{
			value = VariantUtils.CreateFrom(in _stepList);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			value = VariantUtils.CreateFrom(in _titleLabel);
			return true;
		}
		if (name == PropertyName._broadcastLabel)
		{
			value = VariantUtils.CreateFrom(in _broadcastLabel);
			return true;
		}
		if (name == PropertyName._broadcastManager)
		{
			value = VariantUtils.CreateFrom(in _broadcastManager);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._conditionRows)
		{
			value = VariantUtils.CreateFrom(in _conditionRows);
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
		if (name == PropertyName._autoButton)
		{
			value = VariantUtils.CreateFrom(in _autoButton);
			return true;
		}
		if (name == PropertyName._autoTimer)
		{
			value = VariantUtils.CreateFrom(in _autoTimer);
			return true;
		}
		if (name == PropertyName._updatingSelection)
		{
			value = VariantUtils.CreateFrom(in _updatingSelection);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._stepIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._autoPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stepList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._titleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._broadcastLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._broadcastManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._conditionRows, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._autoButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._autoTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingSelection, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._config, Variant.From(in _config));
		info.AddProperty(PropertyName._stepIndex, Variant.From(in _stepIndex));
		info.AddProperty(PropertyName._autoPlaying, Variant.From(in _autoPlaying));
		info.AddProperty(PropertyName._stepList, Variant.From(in _stepList));
		info.AddProperty(PropertyName._titleLabel, Variant.From(in _titleLabel));
		info.AddProperty(PropertyName._broadcastLabel, Variant.From(in _broadcastLabel));
		info.AddProperty(PropertyName._broadcastManager, Variant.From(in _broadcastManager));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._conditionRows, Variant.From(in _conditionRows));
		info.AddProperty(PropertyName._previousButton, Variant.From(in _previousButton));
		info.AddProperty(PropertyName._nextButton, Variant.From(in _nextButton));
		info.AddProperty(PropertyName._autoButton, Variant.From(in _autoButton));
		info.AddProperty(PropertyName._autoTimer, Variant.From(in _autoTimer));
		info.AddProperty(PropertyName._updatingSelection, Variant.From(in _updatingSelection));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._config, out var value))
		{
			_config = value.As<TutorialConfig>();
		}
		if (info.TryGetProperty(PropertyName._stepIndex, out var value2))
		{
			_stepIndex = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._autoPlaying, out var value3))
		{
			_autoPlaying = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._stepList, out var value4))
		{
			_stepList = value4.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._titleLabel, out var value5))
		{
			_titleLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._broadcastLabel, out var value6))
		{
			_broadcastLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._broadcastManager, out var value7))
		{
			_broadcastManager = value7.As<BroadCastManager>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value8))
		{
			_statusLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._conditionRows, out var value9))
		{
			_conditionRows = value9.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._previousButton, out var value10))
		{
			_previousButton = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nextButton, out var value11))
		{
			_nextButton = value11.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._autoButton, out var value12))
		{
			_autoButton = value12.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._autoTimer, out var value13))
		{
			_autoTimer = value13.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._updatingSelection, out var value14))
		{
			_updatingSelection = value14.As<bool>();
		}
	}
}
