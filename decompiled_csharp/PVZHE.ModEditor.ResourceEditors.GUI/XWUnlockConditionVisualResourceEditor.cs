using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWUnlockConditionVisualResourceEditor.cs")]
public class XWUnlockConditionVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName IsBuiltInUnlockConditionWithCompleteVisualCoverage = "IsBuiltInUnlockConditionWithCompleteVisualCoverage";

		public static readonly StringName DisposePropertyBinding = "DisposePropertyBinding";

		public static readonly StringName BindWorkbench = "BindWorkbench";

		public static readonly StringName BindPropertyControls = "BindPropertyControls";

		public static readonly StringName PopulateControls = "PopulateControls";

		public static readonly StringName OnUnlockConditionVisualPropertyEdited = "OnUnlockConditionVisualPropertyEdited";

		public static readonly StringName RefreshUnlockConditionEditorFromHistory = "RefreshUnlockConditionEditorFromHistory";

		public static readonly StringName UpdatePreviewState = "UpdatePreviewState";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName GetResourceDisplayName = "GetResourceDisplayName";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingCondition = "_editingCondition";

		public static readonly StringName _baseFields = "_baseFields";

		public static readonly StringName _levelFinishFields = "_levelFinishFields";

		public static readonly StringName _survivalFields = "_survivalFields";

		public static readonly StringName _packetFields = "_packetFields";

		public static readonly StringName _packetBankFields = "_packetBankFields";

		public static readonly StringName _resourceName = "_resourceName";

		public static readonly StringName _localToScene = "_localToScene";

		public static readonly StringName _levelFinishKey = "_levelFinishKey";

		public static readonly StringName _survivalLevelKey = "_survivalLevelKey";

		public static readonly StringName _round = "_round";

		public static readonly StringName _packetName = "_packetName";

		public static readonly StringName _bankName = "_bankName";

		public static readonly StringName _category = "_category";

		public static readonly StringName _count = "_count";

		public static readonly StringName _simulateComplete = "_simulateComplete";

		public static readonly StringName _simulateProgressLabel = "_simulateProgressLabel";

		public static readonly StringName _simulateProgress = "_simulateProgress";

		public static readonly StringName _progress = "_progress";

		public static readonly StringName _simulationHint = "_simulationHint";

		public new static readonly StringName _title = "_title";

		public static readonly StringName _type = "_type";

		public static readonly StringName _cardFrame = "_cardFrame";

		public static readonly StringName _targetLabel = "_targetLabel";

		public static readonly StringName _lockIcon = "_lockIcon";

		public static readonly StringName _unlockIcon = "_unlockIcon";

		public static readonly StringName _stateLabel = "_stateLabel";

		public static readonly StringName _requirement = "_requirement";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string WorkbenchScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWUnlockConditionWorkbench.tscn";

	private static PackedScene _workbenchScene;

	private UnlockConditionBaseConfig _editingCondition;

	private XWVisualPropertyBinding _propertyBinding;

	private VBoxContainer _baseFields;

	private VBoxContainer _levelFinishFields;

	private VBoxContainer _survivalFields;

	private VBoxContainer _packetFields;

	private VBoxContainer _packetBankFields;

	private LineEdit _resourceName;

	private CheckButton _localToScene;

	private LineEdit _levelFinishKey;

	private LineEdit _survivalLevelKey;

	private SpinBox _round;

	private LineEdit _packetName;

	private LineEdit _bankName;

	private LineEdit _category;

	private SpinBox _count;

	private CheckBox _simulateComplete;

	private Label _simulateProgressLabel;

	private SpinBox _simulateProgress;

	private ProgressBar _progress;

	private Label _simulationHint;

	private Label _title;

	private Label _type;

	private PanelContainer _cardFrame;

	private Label _targetLabel;

	private TextureRect _lockIcon;

	private TextureRect _unlockIcon;

	private Label _stateLabel;

	private Label _requirement;

	private bool _updatingControls;

	public override void _ExitTree()
	{
		DisposePropertyBinding();
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposePropertyBinding();
		if (CurrentResource is UnlockConditionBaseConfig editingCondition && CanvasGrid != null)
		{
			_editingCondition = editingCondition;
			CanvasGrid.Columns = 1;
			if (_workbenchScene == null)
			{
				_workbenchScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWUnlockConditionWorkbench.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _workbenchScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindWorkbench(vBoxContainer);
				PopulateControls();
				BindPropertyControls();
				UpdatePreviewState();
				AddSummaryRows();
			}
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!IsBuiltInUnlockConditionWithCompleteVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool IsBuiltInUnlockConditionWithCompleteVisualCoverage(Resource resource)
	{
		Type type = resource?.GetType();
		if (!(type == typeof(UnlockConditionBaseConfig)) && !(type == typeof(UnlockConditionLevelFinishConfig)) && !(type == typeof(UnlockConditionLevelSurvivalRoundConfig)) && !(type == typeof(UnlockConditionPacketUnlockConfig)))
		{
			return type == typeof(UnlockConditionPacketBankCategoryPacketUnlockNumConfig);
		}
		return true;
	}

	private void DisposePropertyBinding(bool clearEditingCondition = true)
	{
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		if (clearEditingCondition)
		{
			_editingCondition = null;
		}
	}

	private void BindWorkbench(VBoxContainer root)
	{
		_baseFields = root.GetNode<VBoxContainer>("%BaseFields");
		_levelFinishFields = root.GetNode<VBoxContainer>("%LevelFinishFields");
		_survivalFields = root.GetNode<VBoxContainer>("%SurvivalFields");
		_packetFields = root.GetNode<VBoxContainer>("%PacketFields");
		_packetBankFields = root.GetNode<VBoxContainer>("%PacketBankFields");
		_resourceName = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToScene = root.GetNode<CheckButton>("%LocalToSceneCheck");
		_levelFinishKey = root.GetNode<LineEdit>("%LevelFinishKey");
		_survivalLevelKey = root.GetNode<LineEdit>("%SurvivalLevelKey");
		_round = root.GetNode<SpinBox>("%Round");
		_packetName = root.GetNode<LineEdit>("%PacketName");
		_bankName = root.GetNode<LineEdit>("%BankName");
		_category = root.GetNode<LineEdit>("%Category");
		_count = root.GetNode<SpinBox>("%Count");
		_simulateComplete = root.GetNode<CheckBox>("%SimulateComplete");
		_simulateProgressLabel = root.GetNode<Label>("%SimulateProgressLabel");
		_simulateProgress = root.GetNode<SpinBox>("%SimulateProgress");
		_progress = root.GetNode<ProgressBar>("%Progress");
		_simulationHint = root.GetNode<Label>("%SimulationHint");
		_title = root.GetNode<Label>("%Title");
		_type = root.GetNode<Label>("%Type");
		_cardFrame = root.GetNode<PanelContainer>("%CardFrame");
		_targetLabel = root.GetNode<Label>("%TargetLabel");
		_lockIcon = root.GetNode<TextureRect>("%LockIcon");
		_unlockIcon = root.GetNode<TextureRect>("%UnlockIcon");
		_stateLabel = root.GetNode<Label>("%StateLabel");
		_requirement = root.GetNode<Label>("%Requirement");
		_simulateComplete.Toggled += (bool _) =>
		{
			if (!_updatingControls)
			{
				UpdatePreviewState();
			}
		};
		_simulateProgress.ValueChanged += (double _) =>
		{
			if (!_updatingControls)
			{
				UpdatePreviewState();
			}
		};
	}

	private void BindPropertyControls()
	{
		DisposePropertyBinding(clearEditingCondition: false);
		if (GodotObject.IsInstanceValid(_editingCondition))
		{
			_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnUnlockConditionVisualPropertyEdited);
			_propertyBinding.BindText(_resourceName, _editingCondition, "resource_name", UpdatePreviewState, this, "RefreshUnlockConditionEditorFromHistory");
			_propertyBinding.BindToggle(_localToScene, _editingCondition, "resource_local_to_scene", UpdatePreviewState, this, "RefreshUnlockConditionEditorFromHistory");
			if (_editingCondition is UnlockConditionLevelFinishConfig resource)
			{
				_propertyBinding.BindText(_levelFinishKey, resource, "levelSaveKey", UpdatePreviewState, this, "RefreshUnlockConditionEditorFromHistory");
			}
			if (_editingCondition is UnlockConditionLevelSurvivalRoundConfig resource2)
			{
				_propertyBinding.BindText(_survivalLevelKey, resource2, "levelSaveKey", UpdatePreviewState, this, "RefreshUnlockConditionEditorFromHistory");
				_propertyBinding.BindNumber(_round, resource2, "roundNum", UpdatePreviewState, this, "RefreshUnlockConditionEditorFromHistory");
			}
			if (_editingCondition is UnlockConditionPacketUnlockConfig resource3)
			{
				_propertyBinding.BindText(_packetName, resource3, "packetName", UpdatePreviewState, this, "RefreshUnlockConditionEditorFromHistory");
			}
			if (_editingCondition is UnlockConditionPacketBankCategoryPacketUnlockNumConfig resource4)
			{
				_propertyBinding.BindText(_bankName, resource4, "packetBankName", UpdatePreviewState, this, "RefreshUnlockConditionEditorFromHistory");
				_propertyBinding.BindText(_category, resource4, "category", UpdatePreviewState, this, "RefreshUnlockConditionEditorFromHistory");
				_propertyBinding.BindNumber(_count, resource4, "num", UpdatePreviewState, this, "RefreshUnlockConditionEditorFromHistory");
			}
		}
	}

	private void PopulateControls()
	{
		if (!GodotObject.IsInstanceValid(_editingCondition) || !GodotObject.IsInstanceValid(_baseFields))
		{
			return;
		}
		_updatingControls = true;
		try
		{
			_baseFields.Visible = _editingCondition.GetType() == typeof(UnlockConditionBaseConfig);
			_levelFinishFields.Visible = _editingCondition is UnlockConditionLevelFinishConfig;
			_survivalFields.Visible = _editingCondition is UnlockConditionLevelSurvivalRoundConfig;
			_packetFields.Visible = _editingCondition is UnlockConditionPacketUnlockConfig;
			_packetBankFields.Visible = _editingCondition is UnlockConditionPacketBankCategoryPacketUnlockNumConfig;
			_simulateComplete.ButtonPressed = false;
			_simulateProgress.Value = 0.0;
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void OnUnlockConditionVisualPropertyEdited(bool committed)
	{
		if (CurrentResource is UnlockConditionBaseConfig unlockConditionBaseConfig && unlockConditionBaseConfig == _editingCondition)
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

	public void RefreshUnlockConditionEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()) && CurrentResource is UnlockConditionBaseConfig unlockConditionBaseConfig && unlockConditionBaseConfig == _editingCondition)
		{
			BindPropertyControls();
			PopulateControls();
			UpdatePreviewState();
		}
	}

	private void UpdatePreviewState()
	{
		if (!GodotObject.IsInstanceValid(_editingCondition) || !GodotObject.IsInstanceValid(_requirement))
		{
			return;
		}
		int num = 0;
		int num2 = 1;
		UnlockConditionBaseConfig editingCondition = _editingCondition;
		string text;
		string text4;
		string text5;
		bool flag;
		bool flag2;
		if (!(editingCondition is UnlockConditionLevelFinishConfig unlockConditionLevelFinishConfig))
		{
			if (!(editingCondition is UnlockConditionLevelSurvivalRoundConfig unlockConditionLevelSurvivalRoundConfig))
			{
				if (!(editingCondition is UnlockConditionPacketUnlockConfig unlockConditionPacketUnlockConfig))
				{
					if (editingCondition is UnlockConditionPacketBankCategoryPacketUnlockNumConfig unlockConditionPacketBankCategoryPacketUnlockNumConfig)
					{
						text = "卡牌库数量解锁";
						string text2 = EmptyToPlaceholder(unlockConditionPacketBankCategoryPacketUnlockNumConfig.packetBankName, "卡牌库");
						string text3 = EmptyToPlaceholder(unlockConditionPacketBankCategoryPacketUnlockNumConfig.category, "分类");
						text4 = text2 + " / " + text3;
						num2 = Math.Max(1, unlockConditionPacketBankCategoryPacketUnlockNumConfig.num);
						num = Mathf.RoundToInt(_simulateProgress.Value);
						text5 = $"在 {text4} 中至少解锁 {num2} 张卡牌";
						flag = num >= num2;
						flag2 = true;
					}
					else
					{
						text = "基础解锁条件";
						text4 = "未指定内容";
						text5 = "基础条件不会自动解锁，请换用一个具体条件类型";
						flag = false;
						flag2 = false;
					}
				}
				else
				{
					text = "卡牌前置解锁";
					text4 = EmptyToPlaceholder(unlockConditionPacketUnlockConfig.packetName, "卡牌 saveKey");
					text5 = "先解锁卡牌：" + text4;
					flag = _simulateComplete.ButtonPressed;
					num = (flag ? 1 : 0);
					flag2 = false;
				}
			}
			else
			{
				text = "生存轮数解锁";
				text4 = EmptyToPlaceholder(unlockConditionLevelSurvivalRoundConfig.levelSaveKey, "生存关卡存档键");
				num2 = Math.Max(1, unlockConditionLevelSurvivalRoundConfig.roundNum);
				num = Mathf.RoundToInt(_simulateProgress.Value);
				text5 = $"在 {text4} 完成 {num2} 轮";
				flag = num >= num2;
				flag2 = true;
			}
		}
		else
		{
			text = "通关解锁";
			text4 = EmptyToPlaceholder(unlockConditionLevelFinishConfig.levelSaveKey, "关卡存档键");
			text5 = ((unlockConditionLevelFinishConfig.levelSaveKey == "Unlock") ? "该内容在游戏中始终解锁" : ("通关关卡：" + text4));
			flag = unlockConditionLevelFinishConfig.levelSaveKey == "Unlock" || _simulateComplete.ButtonPressed;
			num = (flag ? 1 : 0);
			flag2 = false;
		}
		_type.Text = text;
		_title.Text = "游戏解锁条件 · " + GetResourceDisplayName();
		_targetLabel.Text = text4;
		_requirement.Text = text5;
		_stateLabel.Text = (flag ? "游戏中已解锁" : "游戏中仍锁定");
		_stateLabel.Modulate = (flag ? new Color(0.62f, 1f, 0.42f) : new Color(1f, 0.72f, 0.42f));
		_lockIcon.Visible = !flag;
		_unlockIcon.Visible = flag;
		_cardFrame.Modulate = (flag ? Colors.White : new Color(0.62f, 0.62f, 0.62f));
		_simulateComplete.Visible = !flag2 && _editingCondition.GetType() != typeof(UnlockConditionBaseConfig);
		_simulateProgressLabel.Visible = flag2;
		_simulateProgress.Visible = flag2;
		_progress.Visible = _editingCondition.GetType() != typeof(UnlockConditionBaseConfig);
		_progress.MaxValue = Math.Max(1, num2);
		_progress.Value = Mathf.Clamp(num, 0, num2);
		_simulationHint.Text = (flag2 ? $"模拟进度：{num} / {num2}。这只改变预览，不会修改游戏存档。" : "切换前置条件状态可预览锁定/解锁结果；不会修改游戏存档。");
	}

	private void AddSummaryRows()
	{
		if (PreviewList != null)
		{
			PreviewList.AddItem("资源类型 -> " + _editingCondition.GetType().Name);
			PreviewList.AddItem("编辑区 -> 游戏锁定卡片上的解锁要求");
			PreviewList.AddItem("模拟状态 -> 仅用于预览，不读取或写入主游戏存档");
		}
	}

	private string GetResourceDisplayName()
	{
		if (!string.IsNullOrWhiteSpace(_editingCondition.ResourceName))
		{
			return _editingCondition.ResourceName;
		}
		if (!string.IsNullOrWhiteSpace(CurrentResourcePath))
		{
			string text = CurrentResourcePath.Replace('\\', '/');
			int num = text.LastIndexOf('/');
			if (num < 0)
			{
				return text;
			}
			string text2 = text;
			int num2 = num + 1;
			return text2.Substring(num2, text2.Length - num2);
		}
		return _editingCondition.GetType().Name;
	}

	private static string EmptyToPlaceholder(string value, string placeholder)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return "<" + placeholder + ">";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsBuiltInUnlockConditionWithCompleteVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposePropertyBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "clearEditingCondition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindPropertyControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnUnlockConditionVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshUnlockConditionEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePreviewState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetResourceDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmptyToPlaceholder, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "placeholder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IsBuiltInUnlockConditionWithCompleteVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBuiltInUnlockConditionWithCompleteVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.DisposePropertyBinding && args.Count == 1)
		{
			DisposePropertyBinding(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindWorkbench && args.Count == 1)
		{
			BindWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindPropertyControls && args.Count == 0)
		{
			BindPropertyControls();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateControls && args.Count == 0)
		{
			PopulateControls();
			ret = default;
			return true;
		}
		if (method == MethodName.OnUnlockConditionVisualPropertyEdited && args.Count == 1)
		{
			OnUnlockConditionVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshUnlockConditionEditorFromHistory && args.Count == 0)
		{
			RefreshUnlockConditionEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreviewState && args.Count == 0)
		{
			UpdatePreviewState();
			ret = default;
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 0)
		{
			AddSummaryRows();
			ret = default;
			return true;
		}
		if (method == MethodName.GetResourceDisplayName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourceDisplayName());
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyToPlaceholder(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsBuiltInUnlockConditionWithCompleteVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBuiltInUnlockConditionWithCompleteVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyToPlaceholder(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.IsBuiltInUnlockConditionWithCompleteVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.DisposePropertyBinding)
		{
			return true;
		}
		if (method == MethodName.BindWorkbench)
		{
			return true;
		}
		if (method == MethodName.BindPropertyControls)
		{
			return true;
		}
		if (method == MethodName.PopulateControls)
		{
			return true;
		}
		if (method == MethodName.OnUnlockConditionVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshUnlockConditionEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.UpdatePreviewState)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.GetResourceDisplayName)
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
		if (name == PropertyName._editingCondition)
		{
			_editingCondition = VariantUtils.ConvertTo<UnlockConditionBaseConfig>(in value);
			return true;
		}
		if (name == PropertyName._baseFields)
		{
			_baseFields = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._levelFinishFields)
		{
			_levelFinishFields = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._survivalFields)
		{
			_survivalFields = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetFields)
		{
			_packetFields = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetBankFields)
		{
			_packetBankFields = VariantUtils.ConvertTo<VBoxContainer>(in value);
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
		if (name == PropertyName._levelFinishKey)
		{
			_levelFinishKey = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._survivalLevelKey)
		{
			_survivalLevelKey = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._round)
		{
			_round = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._packetName)
		{
			_packetName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._bankName)
		{
			_bankName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._category)
		{
			_category = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._count)
		{
			_count = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._simulateComplete)
		{
			_simulateComplete = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._simulateProgressLabel)
		{
			_simulateProgressLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._simulateProgress)
		{
			_simulateProgress = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._progress)
		{
			_progress = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._simulationHint)
		{
			_simulationHint = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._title)
		{
			_title = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._type)
		{
			_type = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._cardFrame)
		{
			_cardFrame = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._targetLabel)
		{
			_targetLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._lockIcon)
		{
			_lockIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._unlockIcon)
		{
			_unlockIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._stateLabel)
		{
			_stateLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._requirement)
		{
			_requirement = VariantUtils.ConvertTo<Label>(in value);
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
		if (name == PropertyName._editingCondition)
		{
			value = VariantUtils.CreateFrom(in _editingCondition);
			return true;
		}
		if (name == PropertyName._baseFields)
		{
			value = VariantUtils.CreateFrom(in _baseFields);
			return true;
		}
		if (name == PropertyName._levelFinishFields)
		{
			value = VariantUtils.CreateFrom(in _levelFinishFields);
			return true;
		}
		if (name == PropertyName._survivalFields)
		{
			value = VariantUtils.CreateFrom(in _survivalFields);
			return true;
		}
		if (name == PropertyName._packetFields)
		{
			value = VariantUtils.CreateFrom(in _packetFields);
			return true;
		}
		if (name == PropertyName._packetBankFields)
		{
			value = VariantUtils.CreateFrom(in _packetBankFields);
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
		if (name == PropertyName._levelFinishKey)
		{
			value = VariantUtils.CreateFrom(in _levelFinishKey);
			return true;
		}
		if (name == PropertyName._survivalLevelKey)
		{
			value = VariantUtils.CreateFrom(in _survivalLevelKey);
			return true;
		}
		if (name == PropertyName._round)
		{
			value = VariantUtils.CreateFrom(in _round);
			return true;
		}
		if (name == PropertyName._packetName)
		{
			value = VariantUtils.CreateFrom(in _packetName);
			return true;
		}
		if (name == PropertyName._bankName)
		{
			value = VariantUtils.CreateFrom(in _bankName);
			return true;
		}
		if (name == PropertyName._category)
		{
			value = VariantUtils.CreateFrom(in _category);
			return true;
		}
		if (name == PropertyName._count)
		{
			value = VariantUtils.CreateFrom(in _count);
			return true;
		}
		if (name == PropertyName._simulateComplete)
		{
			value = VariantUtils.CreateFrom(in _simulateComplete);
			return true;
		}
		if (name == PropertyName._simulateProgressLabel)
		{
			value = VariantUtils.CreateFrom(in _simulateProgressLabel);
			return true;
		}
		if (name == PropertyName._simulateProgress)
		{
			value = VariantUtils.CreateFrom(in _simulateProgress);
			return true;
		}
		if (name == PropertyName._progress)
		{
			value = VariantUtils.CreateFrom(in _progress);
			return true;
		}
		if (name == PropertyName._simulationHint)
		{
			value = VariantUtils.CreateFrom(in _simulationHint);
			return true;
		}
		if (name == PropertyName._title)
		{
			value = VariantUtils.CreateFrom(in _title);
			return true;
		}
		if (name == PropertyName._type)
		{
			value = VariantUtils.CreateFrom(in _type);
			return true;
		}
		if (name == PropertyName._cardFrame)
		{
			value = VariantUtils.CreateFrom(in _cardFrame);
			return true;
		}
		if (name == PropertyName._targetLabel)
		{
			value = VariantUtils.CreateFrom(in _targetLabel);
			return true;
		}
		if (name == PropertyName._lockIcon)
		{
			value = VariantUtils.CreateFrom(in _lockIcon);
			return true;
		}
		if (name == PropertyName._unlockIcon)
		{
			value = VariantUtils.CreateFrom(in _unlockIcon);
			return true;
		}
		if (name == PropertyName._stateLabel)
		{
			value = VariantUtils.CreateFrom(in _stateLabel);
			return true;
		}
		if (name == PropertyName._requirement)
		{
			value = VariantUtils.CreateFrom(in _requirement);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._editingCondition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._baseFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelFinishFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._survivalFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetBankFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelFinishKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._survivalLevelKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._round, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bankName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._category, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._count, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulateComplete, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulateProgressLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulateProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._progress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulationHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._title, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._type, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cardFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lockIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stateLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._requirement, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingCondition, Variant.From(in _editingCondition));
		info.AddProperty(PropertyName._baseFields, Variant.From(in _baseFields));
		info.AddProperty(PropertyName._levelFinishFields, Variant.From(in _levelFinishFields));
		info.AddProperty(PropertyName._survivalFields, Variant.From(in _survivalFields));
		info.AddProperty(PropertyName._packetFields, Variant.From(in _packetFields));
		info.AddProperty(PropertyName._packetBankFields, Variant.From(in _packetBankFields));
		info.AddProperty(PropertyName._resourceName, Variant.From(in _resourceName));
		info.AddProperty(PropertyName._localToScene, Variant.From(in _localToScene));
		info.AddProperty(PropertyName._levelFinishKey, Variant.From(in _levelFinishKey));
		info.AddProperty(PropertyName._survivalLevelKey, Variant.From(in _survivalLevelKey));
		info.AddProperty(PropertyName._round, Variant.From(in _round));
		info.AddProperty(PropertyName._packetName, Variant.From(in _packetName));
		info.AddProperty(PropertyName._bankName, Variant.From(in _bankName));
		info.AddProperty(PropertyName._category, Variant.From(in _category));
		info.AddProperty(PropertyName._count, Variant.From(in _count));
		info.AddProperty(PropertyName._simulateComplete, Variant.From(in _simulateComplete));
		info.AddProperty(PropertyName._simulateProgressLabel, Variant.From(in _simulateProgressLabel));
		info.AddProperty(PropertyName._simulateProgress, Variant.From(in _simulateProgress));
		info.AddProperty(PropertyName._progress, Variant.From(in _progress));
		info.AddProperty(PropertyName._simulationHint, Variant.From(in _simulationHint));
		info.AddProperty(PropertyName._title, Variant.From(in _title));
		info.AddProperty(PropertyName._type, Variant.From(in _type));
		info.AddProperty(PropertyName._cardFrame, Variant.From(in _cardFrame));
		info.AddProperty(PropertyName._targetLabel, Variant.From(in _targetLabel));
		info.AddProperty(PropertyName._lockIcon, Variant.From(in _lockIcon));
		info.AddProperty(PropertyName._unlockIcon, Variant.From(in _unlockIcon));
		info.AddProperty(PropertyName._stateLabel, Variant.From(in _stateLabel));
		info.AddProperty(PropertyName._requirement, Variant.From(in _requirement));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingCondition, out var value))
		{
			_editingCondition = value.As<UnlockConditionBaseConfig>();
		}
		if (info.TryGetProperty(PropertyName._baseFields, out var value2))
		{
			_baseFields = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._levelFinishFields, out var value3))
		{
			_levelFinishFields = value3.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._survivalFields, out var value4))
		{
			_survivalFields = value4.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetFields, out var value5))
		{
			_packetFields = value5.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetBankFields, out var value6))
		{
			_packetBankFields = value6.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._resourceName, out var value7))
		{
			_resourceName = value7.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToScene, out var value8))
		{
			_localToScene = value8.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._levelFinishKey, out var value9))
		{
			_levelFinishKey = value9.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._survivalLevelKey, out var value10))
		{
			_survivalLevelKey = value10.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._round, out var value11))
		{
			_round = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._packetName, out var value12))
		{
			_packetName = value12.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._bankName, out var value13))
		{
			_bankName = value13.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._category, out var value14))
		{
			_category = value14.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._count, out var value15))
		{
			_count = value15.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._simulateComplete, out var value16))
		{
			_simulateComplete = value16.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._simulateProgressLabel, out var value17))
		{
			_simulateProgressLabel = value17.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._simulateProgress, out var value18))
		{
			_simulateProgress = value18.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._progress, out var value19))
		{
			_progress = value19.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._simulationHint, out var value20))
		{
			_simulationHint = value20.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._title, out var value21))
		{
			_title = value21.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._type, out var value22))
		{
			_type = value22.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._cardFrame, out var value23))
		{
			_cardFrame = value23.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._targetLabel, out var value24))
		{
			_targetLabel = value24.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._lockIcon, out var value25))
		{
			_lockIcon = value25.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._unlockIcon, out var value26))
		{
			_unlockIcon = value26.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._stateLabel, out var value27))
		{
			_stateLabel = value27.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._requirement, out var value28))
		{
			_requirement = value28.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value29))
		{
			_updatingControls = value29.As<bool>();
		}
	}
}
