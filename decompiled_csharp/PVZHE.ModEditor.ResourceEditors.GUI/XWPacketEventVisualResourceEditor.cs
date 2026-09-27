using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketEventVisualResourceEditor.cs")]
public class XWPacketEventVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName RenderPacketEventWorkbench = "RenderPacketEventWorkbench";

		public static readonly StringName BindWorkbench = "BindWorkbench";

		public static readonly StringName BindEventProperties = "BindEventProperties";

		public static readonly StringName PopulateEventFields = "PopulateEventFields";

		public static readonly StringName MountLevelPacketConfigPicker = "MountLevelPacketConfigPicker";

		public static readonly StringName BindLevelPacketConfigFields = "BindLevelPacketConfigFields";

		public static readonly StringName OnVisualPropertyEdited = "OnVisualPropertyEdited";

		public static readonly StringName RefreshPreviewAfterPropertyChange = "RefreshPreviewAfterPropertyChange";

		public static readonly StringName RefreshPacketEventEditorFromHistory = "RefreshPacketEventEditorFromHistory";

		public static readonly StringName RebuildPacketEventEditor = "RebuildPacketEventEditor";

		public static readonly StringName DisposeEventBindings = "DisposeEventBindings";

		public static readonly StringName ApplyEventPreview = "ApplyEventPreview";

		public static readonly StringName UpdateSimulationPreview = "UpdateSimulationPreview";

		public static readonly StringName BuildPreviewDescription = "BuildPreviewDescription";

		public static readonly StringName CalculateResultCost = "CalculateResultCost";

		public static readonly StringName ConfigurePacketCard = "ConfigurePacketCard";

		public static readonly StringName SelectOptionById = "SelectOptionById";

		public static readonly StringName EmptyName = "EmptyName";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingEvent = "_editingEvent";

		public static readonly StringName _eventType = "_eventType";

		public static readonly StringName _formHint = "_formHint";

		public static readonly StringName _outcome = "_outcome";

		public static readonly StringName _currentName = "_currentName";

		public static readonly StringName _resultName = "_resultName";

		public static readonly StringName _deletionStamp = "_deletionStamp";

		public static readonly StringName _currentCardRoot = "_currentCardRoot";

		public static readonly StringName _resultCardRoot = "_resultCardRoot";

		public static readonly StringName _currentPacket = "_currentPacket";

		public static readonly StringName _resultPacket = "_resultPacket";

		public static readonly StringName _baseFields = "_baseFields";

		public static readonly StringName _changeCostFields = "_changeCostFields";

		public static readonly StringName _cooldownFields = "_cooldownFields";

		public static readonly StringName _changePacketFields = "_changePacketFields";

		public static readonly StringName _resourceNameEdit = "_resourceNameEdit";

		public static readonly StringName _localToSceneCheck = "_localToSceneCheck";

		public static readonly StringName _costMethod = "_costMethod";

		public static readonly StringName _costValue = "_costValue";

		public static readonly StringName _costMin = "_costMin";

		public static readonly StringName _costMax = "_costMax";

		public static readonly StringName _cooldownValue = "_cooldownValue";

		public static readonly StringName _packetName = "_packetName";

		public static readonly StringName _replaceCount = "_replaceCount";

		public static readonly StringName _initialCost = "_initialCost";

		public static readonly StringName _cooldownDuration = "_cooldownDuration";

		public static readonly StringName _simulatedUses = "_simulatedUses";

		public static readonly StringName _previewTween = "_previewTween";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string VisualEditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketEventVisualEditorLayout.tscn";

	private static PackedScene _visualEditorLayoutScene;

	private CardActionBehaviorDefinition _editingEvent;

	private XWVisualPropertyBinding _propertyBinding;

	private XWVisualPropertyBinding _levelPacketPropertyBinding;

	private Label _eventType;

	private Label _formHint;

	private Label _outcome;

	private Label _currentName;

	private Label _resultName;

	private Label _deletionStamp;

	private Control _currentCardRoot;

	private Control _resultCardRoot;

	private TowerDefenseInGamePacketShow _currentPacket;

	private TowerDefenseInGamePacketShow _resultPacket;

	private Control _baseFields;

	private Control _changeCostFields;

	private Control _cooldownFields;

	private Control _changePacketFields;

	private LineEdit _resourceNameEdit;

	private CheckButton _localToSceneCheck;

	private OptionButton _costMethod;

	private XWVisualSegmentedOption _costMethodVisualChoices;

	private SpinBox _costValue;

	private SpinBox _costMin;

	private SpinBox _costMax;

	private SpinBox _cooldownValue;

	private LineEdit _packetName;

	private SpinBox _replaceCount;

	private SpinBox _initialCost;

	private SpinBox _cooldownDuration;

	private SpinBox _simulatedUses;

	private Tween _previewTween;

	private bool _updatingControls;

	public override void _ExitTree()
	{
		DisposeEventBindings();
		if (GodotObject.IsInstanceValid(_previewTween))
		{
			_previewTween.Kill();
		}
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeEventBindings();
		if (CurrentResource is CardActionBehaviorDefinition packetEvent && CanvasGrid != null)
		{
			RenderPacketEventWorkbench(packetEvent);
		}
	}

	private void RenderPacketEventWorkbench(CardActionBehaviorDefinition packetEvent)
	{
		_editingEvent = packetEvent;
		CanvasGrid.Columns = 1;
		if (_visualEditorLayoutScene == null)
		{
			_visualEditorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketEventVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _visualEditorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(vBoxContainer))
		{
			CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
			BindWorkbench(vBoxContainer);
			PopulateEventFields();
			BindEventProperties(vBoxContainer, packetEvent);
			UpdateSimulationPreview(applied: false);
			AddSummaryRows();
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		Type type = resource?.GetType();
		if (!(type == typeof(CardActionBehaviorDefinition)) && !(type == typeof(CardActionBehaviorChangeCost)) && !(type == typeof(CardActionBehaviorSetCooldown)) && !(type == typeof(CardActionBehaviorChangePacket)) && !(type == typeof(CardActionBehaviorDelete)))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private void BindWorkbench(VBoxContainer root)
	{
		_eventType = root.GetNode<Label>("%EventType");
		_formHint = root.GetNode<Label>("%FormHint");
		_outcome = root.GetNode<Label>("%Outcome");
		_currentName = root.GetNode<Label>("%CurrentName");
		_resultName = root.GetNode<Label>("%ResultName");
		_deletionStamp = root.GetNode<Label>("%DeletionStamp");
		_currentCardRoot = root.GetNode<Control>("%CurrentCardRoot");
		_resultCardRoot = root.GetNode<Control>("%ResultCardRoot");
		_currentPacket = root.GetNode<TowerDefenseInGamePacketShow>("%CurrentPacket");
		_resultPacket = root.GetNode<TowerDefenseInGamePacketShow>("%ResultPacket");
		_baseFields = root.GetNode<Control>("%BaseFields");
		_changeCostFields = root.GetNode<Control>("%ChangeCostFields");
		_cooldownFields = root.GetNode<Control>("%CooldownFields");
		_changePacketFields = root.GetNode<Control>("%ChangePacketFields");
		_resourceNameEdit = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToSceneCheck = root.GetNode<CheckButton>("%LocalToSceneCheck");
		_costMethod = root.GetNode<OptionButton>("%CostMethod");
		_costMethodVisualChoices = new XWVisualSegmentedOption(_costMethod, root.GetNode<HFlowContainer>("%CostMethodVisualChoices"));
		_costMethodVisualChoices.Rebuild();
		_costValue = root.GetNode<SpinBox>("%CostValue");
		_costMin = root.GetNode<SpinBox>("%CostMin");
		_costMax = root.GetNode<SpinBox>("%CostMax");
		_cooldownValue = root.GetNode<SpinBox>("%CooldownValue");
		_packetName = root.GetNode<LineEdit>("%PacketName");
		_replaceCount = root.GetNode<SpinBox>("%ReplaceCount");
		_initialCost = root.GetNode<SpinBox>("%InitialCost");
		_cooldownDuration = root.GetNode<SpinBox>("%CooldownDuration");
		_simulatedUses = root.GetNode<SpinBox>("%SimulatedUses");
		_currentPacket.onlyDraw = true;
		_currentPacket.useCost = false;
		_resultPacket.onlyDraw = true;
		_resultPacket.useCost = false;
		root.GetNode<Button>("%TriggerButton").Pressed += ApplyEventPreview;
		root.GetNode<Button>("%ResetButton").Pressed += () =>
		{
			UpdateSimulationPreview(applied: false);
		};
		_initialCost.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview(applied: false);
		};
		_cooldownDuration.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview(applied: false);
		};
		_simulatedUses.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview(applied: false);
		};
	}

	private void BindEventProperties(VBoxContainer root, CardActionBehaviorDefinition packetEvent)
	{
		LineEdit resourceNameEdit = _resourceNameEdit;
		CheckButton localToSceneCheck = _localToSceneCheck;
		_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnVisualPropertyEdited);
		_propertyBinding.BindText(resourceNameEdit, packetEvent, "resource_name", RefreshPreviewAfterPropertyChange, this, "RefreshPacketEventEditorFromHistory");
		_propertyBinding.BindToggle(localToSceneCheck, packetEvent, "resource_local_to_scene", RefreshPreviewAfterPropertyChange, this, "RefreshPacketEventEditorFromHistory");
		CardActionBehaviorChangeCost cardActionBehaviorChangeCost = packetEvent as CardActionBehaviorChangeCost;
		if (cardActionBehaviorChangeCost == null)
		{
			if (!(packetEvent is CardActionBehaviorSetCooldown resource))
			{
				if (packetEvent is CardActionBehaviorChangePacket cardActionBehaviorChangePacket)
				{
					_propertyBinding.BindNumber(_replaceCount, cardActionBehaviorChangePacket, "count", RefreshPreviewAfterPropertyChange, this, "RefreshPacketEventEditorFromHistory");
					MountLevelPacketConfigPicker(root.GetNode<HBoxContainer>("%LevelPacketConfigPickerHost"), cardActionBehaviorChangePacket);
					BindLevelPacketConfigFields(cardActionBehaviorChangePacket.levelPacketConfig);
				}
			}
			else
			{
				_propertyBinding.BindNumber(_cooldownValue, resource, "value", RefreshPreviewAfterPropertyChange, this, "RefreshPacketEventEditorFromHistory");
			}
			return;
		}
		_propertyBinding.BindNumber(_costValue, cardActionBehaviorChangeCost, "value", RefreshPreviewAfterPropertyChange, this, "RefreshPacketEventEditorFromHistory");
		_propertyBinding.BindNumber(_costMin, cardActionBehaviorChangeCost, "_min", RefreshPreviewAfterPropertyChange, this, "RefreshPacketEventEditorFromHistory");
		_propertyBinding.BindNumber(_costMax, cardActionBehaviorChangeCost, "_max", RefreshPreviewAfterPropertyChange, this, "RefreshPacketEventEditorFromHistory");
		_costMethod.ItemSelected += (long index) =>
		{
			if (!_updatingControls && _propertyBinding != null)
			{
				_propertyBinding.SetValue(cardActionBehaviorChangeCost, "method", _costMethod.GetItemId((int)index), "修改费用运算", this, "RefreshPacketEventEditorFromHistory");
			}
		};
	}

	private void PopulateEventFields()
	{
		if (!GodotObject.IsInstanceValid(_editingEvent) || !GodotObject.IsInstanceValid(_baseFields))
		{
			return;
		}
		_updatingControls = true;
		try
		{
			_eventType.Text = _editingEvent.GetType().Name;
			bool flag = _editingEvent is CardActionBehaviorChangeCost;
			bool flag2 = _editingEvent is CardActionBehaviorSetCooldown;
			bool flag3 = _editingEvent is CardActionBehaviorChangePacket;
			_baseFields.Visible = !flag && !flag2 && !flag3;
			_changeCostFields.Visible = flag;
			_cooldownFields.Visible = flag2;
			_changePacketFields.Visible = flag3;
			if (_editingEvent is CardActionBehaviorChangeCost cardActionBehaviorChangeCost)
			{
				SelectOptionById(_costMethod, (int)cardActionBehaviorChangeCost.method);
				_costMethodVisualChoices?.RefreshSelection();
				_costValue.Value = cardActionBehaviorChangeCost.value;
				_costMin.Value = cardActionBehaviorChangeCost._min;
				_costMax.Value = cardActionBehaviorChangeCost._max;
			}
			if (_editingEvent is CardActionBehaviorSetCooldown cardActionBehaviorSetCooldown)
			{
				_cooldownValue.Value = cardActionBehaviorSetCooldown.value;
			}
			if (_editingEvent is CardActionBehaviorChangePacket cardActionBehaviorChangePacket)
			{
				_packetName.Text = cardActionBehaviorChangePacket.levelPacketConfig?.packetName ?? "";
				_replaceCount.Value = Mathf.Max(1, cardActionBehaviorChangePacket.count);
			}
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void MountLevelPacketConfigPicker(HBoxContainer host, CardActionBehaviorChangePacket changePacket)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return;
		}
		XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
		xWResourcePicker.Name = "LevelPacketConfigPicker";
		xWResourcePicker.Setup("TowerDefenseLevelPacketConfig");
		xWResourcePicker.SetEditedResource(changePacket.levelPacketConfig);
		xWResourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		xWResourcePicker.ResourceChanged += (Resource resource) =>
		{
			if (!_updatingControls && _propertyBinding != null)
			{
				_propertyBinding.SetValue(changePacket, "levelPacketConfig", resource as TowerDefenseLevelPacketConfig, "更换替换卡牌配置", this, "RefreshPacketEventEditorFromHistory");
				RebuildPacketEventEditor();
			}
		};
		host.AddChild(xWResourcePicker, forceReadableName: false, InternalMode.Disabled);
	}

	private void BindLevelPacketConfigFields(TowerDefenseLevelPacketConfig levelPacketConfig)
	{
		_levelPacketPropertyBinding?.Dispose();
		_levelPacketPropertyBinding = null;
		_packetName.Editable = GodotObject.IsInstanceValid(levelPacketConfig);
		if (!GodotObject.IsInstanceValid(levelPacketConfig))
		{
			_packetName.Text = "";
			_packetName.PlaceholderText = "请先新建或选择替换卡牌配置";
		}
		else
		{
			_packetName.PlaceholderText = "例如 PlantPeashooter";
			_levelPacketPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnVisualPropertyEdited);
			_levelPacketPropertyBinding.BindText(_packetName, levelPacketConfig, "packetName", RefreshPreviewAfterPropertyChange, this, "RefreshPacketEventEditorFromHistory");
		}
	}

	private void OnVisualPropertyEdited(bool committed)
	{
		if (GodotObject.IsInstanceValid(_editingEvent) && _editingEvent == CurrentResource)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
				return;
			}
			MarkCurrentResourceDirty();
			_editingEvent.EmitChanged();
		}
	}

	private void RefreshPreviewAfterPropertyChange()
	{
		UpdateSimulationPreview(applied: false);
	}

	public void RefreshPacketEventEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()))
		{
			RebuildPacketEventEditor();
		}
	}

	private void RebuildPacketEventEditor()
	{
		if (CanvasGrid == null || !(CurrentResource is CardActionBehaviorDefinition packetEvent))
		{
			return;
		}
		DisposeEventBindings();
		if (GodotObject.IsInstanceValid(_previewTween))
		{
			_previewTween.Kill();
			_previewTween = null;
		}
		foreach (Node child in CanvasGrid.GetChildren())
		{
			CanvasGrid.RemoveChild(child);
			child.QueueFree();
		}
		RenderPacketEventWorkbench(packetEvent);
	}

	private void DisposeEventBindings()
	{
		_costMethodVisualChoices?.Dispose();
		_costMethodVisualChoices = null;
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		_levelPacketPropertyBinding?.Dispose();
		_levelPacketPropertyBinding = null;
		_editingEvent = null;
	}

	private void ApplyEventPreview()
	{
		UpdateSimulationPreview(applied: true);
		if (GodotObject.IsInstanceValid(_resultCardRoot))
		{
			if (GodotObject.IsInstanceValid(_previewTween))
			{
				_previewTween.Kill();
			}
			_resultCardRoot.Modulate = Colors.White;
			_resultCardRoot.Scale = Vector2.One * 0.78f;
			_previewTween = CreateTween();
			_previewTween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
			_previewTween.TweenProperty(_resultCardRoot, "scale", Vector2.One * 1.08f, 0.22);
			_previewTween.TweenProperty(_resultCardRoot, "scale", Vector2.One, 0.14);
			if (_editingEvent is CardActionBehaviorDelete)
			{
				_previewTween.TweenProperty(_resultCardRoot, "modulate:a", 0.08f, 0.35);
			}
		}
	}

	private void UpdateSimulationPreview(bool applied)
	{
		if (!GodotObject.IsInstanceValid(_editingEvent) || !GodotObject.IsInstanceValid(_currentPacket))
		{
			return;
		}
		if (GodotObject.IsInstanceValid(_previewTween))
		{
			_previewTween.Kill();
			_previewTween = null;
		}
		long num = Math.Max(0, Mathf.RoundToInt(_initialCost.Value));
		double num2 = Math.Max(0.1, _cooldownDuration.Value);
		int num3 = Math.Max(1, Mathf.RoundToInt(_simulatedUses.Value));
		ConfigurePacketCard(_currentPacket, num, num2, cooling: false, 0.0);
		ConfigurePacketCard(_resultPacket, num, num2, cooling: false, 0.0);
		_currentName.Text = "当前卡牌";
		_resultName.Text = "结果卡牌";
		_resultCardRoot.Visible = true;
		_resultCardRoot.Modulate = Colors.White;
		_resultCardRoot.Scale = Vector2.One;
		_deletionStamp.Visible = false;
		if (!applied)
		{
			_outcome.Text = BuildPreviewDescription(num, num2, num3);
			_formHint.Text = "调整事件参数与模拟状态后触发；预演只计算并更新这两张真实游戏卡片。";
			return;
		}
		CardActionBehaviorDefinition editingEvent = _editingEvent;
		if (!(editingEvent is CardActionBehaviorChangeCost cardActionBehaviorChangeCost))
		{
			if (!(editingEvent is CardActionBehaviorSetCooldown cardActionBehaviorSetCooldown))
			{
				if (!(editingEvent is CardActionBehaviorChangePacket cardActionBehaviorChangePacket))
				{
					if (!(editingEvent is CardActionBehaviorDelete))
					{
						if (editingEvent != null && _editingEvent.GetType() == typeof(CardActionBehaviorDefinition))
						{
							_resultPacket.select = true;
							_resultPacket.ColorSet();
							_outcome.Text = "基础卡牌事件没有额外业务字段；资源属性可在上方直接编辑。";
						}
						else
						{
							_resultPacket.select = true;
							_resultPacket.ColorSet();
							_outcome.Text = "自定义卡牌事件已触发视觉提示；实际 Mod 字段由右侧检查器编辑。";
						}
					}
					else
					{
						_deletionStamp.Visible = true;
						_resultName.Text = "卡牌已删除";
						_outcome.Text = "该卡牌会从当前种子栏 packetList 移除并释放。";
					}
				}
				else
				{
					int num4 = Math.Max(1, cardActionBehaviorChangePacket.count);
					bool flag = num3 >= num4;
					string text = EmptyName(cardActionBehaviorChangePacket.levelPacketConfig?.packetName, "未配置替换卡牌");
					_resultName.Text = (flag ? text : $"原卡牌（{num3}/{num4}）");
					_resultPacket.select = flag;
					_resultPacket.ColorSet();
					_outcome.Text = (flag ? $"第 {num3} 次使用达到阈值，卡牌替换为 {text}。" : $"还需使用 {num4 - num3} 次，当前卡牌保持不变。");
				}
			}
			else
			{
				double remaining = Mathf.Clamp(cardActionBehaviorSetCooldown.value, 0.0, num2);
				ConfigurePacketCard(_resultPacket, num, num2, cooling: true, remaining);
				_resultName.Text = $"冷却计时 {cardActionBehaviorSetCooldown.value:0.##} 秒";
				_outcome.Text = $"卡片进入冷却，当前计时器设置为 {cardActionBehaviorSetCooldown.value:0.##} 秒（预览按总冷却 {num2:0.##} 秒裁切）。";
			}
		}
		else
		{
			long num5 = CalculateResultCost(num, cardActionBehaviorChangeCost);
			ConfigurePacketCard(_resultPacket, num5, num2, cooling: false, 0.0);
			_resultName.Text = $"费用 {num} → {num5}";
			_outcome.Text = $"卡牌费用已按 {cardActionBehaviorChangeCost.method} 运算，并应用最低/最高费用限制。";
		}
	}

	private string BuildPreviewDescription(long initialCost, double totalCooldown, int simulatedUses)
	{
		CardActionBehaviorDefinition editingEvent = _editingEvent;
		if (!(editingEvent is CardActionBehaviorChangeCost condition))
		{
			if (!(editingEvent is CardActionBehaviorSetCooldown cardActionBehaviorSetCooldown))
			{
				if (!(editingEvent is CardActionBehaviorChangePacket cardActionBehaviorChangePacket))
				{
					if (editingEvent is CardActionBehaviorDelete)
					{
						return "触发后卡牌会从种子栏消失。";
					}
					return "这是基础或 Mod 自定义卡牌事件；点击触发查看安全视觉提示。";
				}
				return $"第 {Math.Max(1, cardActionBehaviorChangePacket.count)} 次使用后替换为 {EmptyName(cardActionBehaviorChangePacket.levelPacketConfig?.packetName, "未配置卡牌")}；当前模拟第 {simulatedUses} 次。";
			}
			return $"将当前冷却计时设置为 {cardActionBehaviorSetCooldown.value:0.##} 秒；模拟总冷却 {totalCooldown:0.##} 秒。";
		}
		return $"预计费用：{initialCost} → {CalculateResultCost(initialCost, condition)}；点击触发查看卡片变化。";
	}

	private static long CalculateResultCost(long initialCost, CardActionBehaviorChangeCost condition)
	{
		long num = ((condition.method != CardActionBehaviorChangeCost.METHOD.MULTIPLY) ? (initialCost + (long)condition.value) : ((long)Mathf.Floor((double)initialCost * condition.value)));
		long num2 = num;
		if (condition._min != -1 && num2 < condition._min)
		{
			num2 = condition._min;
		}
		if (condition._max != -1 && num2 > condition._max)
		{
			num2 = condition._max;
		}
		return num2;
	}

	private static void ConfigurePacketCard(TowerDefenseInGamePacketShow packet, long cost, double totalCooldown, bool cooling, double remaining)
	{
		if (GodotObject.IsInstanceValid(packet))
		{
			packet.onlyDraw = true;
			packet.useCost = false;
			packet.baseItemCost = cost;
			packet.itemCost = cost;
			packet.alive = !cooling;
			packet.@lock = false;
			packet.openShadow = cooling;
			packet.select = false;
			packet.coldDownOpen = cooling;
			packet.coldDown = totalCooldown;
			packet.coldDownTimer = remaining;
			if (GodotObject.IsInstanceValid(packet.coldDownProgressBar))
			{
				packet.coldDownProgressBar.MaxValue = totalCooldown;
				packet.coldDownProgressBar.Value = remaining;
				packet.coldDownProgressBar.Visible = cooling;
			}
			packet.ColorSet();
		}
	}

	private static void SelectOptionById(OptionButton option, int id)
	{
		for (int i = 0; i < option.ItemCount; i++)
		{
			if (option.GetItemId(i) == id)
			{
				option.Select(i);
				break;
			}
		}
	}

	private static string EmptyName(string value, string fallback)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return fallback;
	}

	private void AddSummaryRows()
	{
		AddItemIfMissing(PreviewList, "卡牌事件 -> " + _editingEvent.GetType().Name);
		AddItemIfMissing(TimelineList, "种子栏触发前 -> 事件计算 -> 触发后卡片");
		AddItemIfMissing(GraphList, "卡牌事件 -> 费用 / 冷却 / 替换 / 删除（不调用 Execute）");
	}

	private static void AddItemIfMissing(ItemList list, string text)
	{
		if (!GodotObject.IsInstanceValid(list) || string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		for (int i = 0; i < list.ItemCount; i++)
		{
			if (list.GetItemText(i) == text)
			{
				return;
			}
		}
		list.AddItem(text);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderPacketEventWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindEventProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packetEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateEventFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MountLevelPacketConfigPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "changePacket", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindLevelPacketConfigFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "levelPacketConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPreviewAfterPropertyChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPacketEventEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildPacketEventEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeEventBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyEventPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSimulationPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "applied", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPreviewDescription, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "initialCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "totalCooldown", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "simulatedUses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CalculateResultCost, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "initialCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigurePacketCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "totalCooldown", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "cooling", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "remaining", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectOptionById, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmptyName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddItemIfMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RenderPacketEventWorkbench && args.Count == 1)
		{
			RenderPacketEventWorkbench(VariantUtils.ConvertTo<CardActionBehaviorDefinition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindWorkbench && args.Count == 1)
		{
			BindWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindEventProperties && args.Count == 2)
		{
			BindEventProperties(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<CardActionBehaviorDefinition>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateEventFields && args.Count == 0)
		{
			PopulateEventFields();
			ret = default;
			return true;
		}
		if (method == MethodName.MountLevelPacketConfigPicker && args.Count == 2)
		{
			MountLevelPacketConfigPicker(VariantUtils.ConvertTo<HBoxContainer>(in args[0]), VariantUtils.ConvertTo<CardActionBehaviorChangePacket>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindLevelPacketConfigFields && args.Count == 1)
		{
			BindLevelPacketConfigFields(VariantUtils.ConvertTo<TowerDefenseLevelPacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualPropertyEdited && args.Count == 1)
		{
			OnVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPreviewAfterPropertyChange && args.Count == 0)
		{
			RefreshPreviewAfterPropertyChange();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketEventEditorFromHistory && args.Count == 0)
		{
			RefreshPacketEventEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildPacketEventEditor && args.Count == 0)
		{
			RebuildPacketEventEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeEventBindings && args.Count == 0)
		{
			DisposeEventBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyEventPreview && args.Count == 0)
		{
			ApplyEventPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSimulationPreview && args.Count == 1)
		{
			UpdateSimulationPreview(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildPreviewDescription && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(BuildPreviewDescription(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.CalculateResultCost && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<long>(CalculateResultCost(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<CardActionBehaviorChangeCost>(in args[1])));
			return true;
		}
		if (method == MethodName.ConfigurePacketCard && args.Count == 5)
		{
			ConfigurePacketCard(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectOptionById && args.Count == 2)
		{
			SelectOptionById(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmptyName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 0)
		{
			AddSummaryRows();
			ret = default;
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CalculateResultCost && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<long>(CalculateResultCost(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<CardActionBehaviorChangeCost>(in args[1])));
			return true;
		}
		if (method == MethodName.ConfigurePacketCard && args.Count == 5)
		{
			ConfigurePacketCard(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectOptionById && args.Count == 2)
		{
			SelectOptionById(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmptyName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.RenderPacketEventWorkbench)
		{
			return true;
		}
		if (method == MethodName.BindWorkbench)
		{
			return true;
		}
		if (method == MethodName.BindEventProperties)
		{
			return true;
		}
		if (method == MethodName.PopulateEventFields)
		{
			return true;
		}
		if (method == MethodName.MountLevelPacketConfigPicker)
		{
			return true;
		}
		if (method == MethodName.BindLevelPacketConfigFields)
		{
			return true;
		}
		if (method == MethodName.OnVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshPreviewAfterPropertyChange)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketEventEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.RebuildPacketEventEditor)
		{
			return true;
		}
		if (method == MethodName.DisposeEventBindings)
		{
			return true;
		}
		if (method == MethodName.ApplyEventPreview)
		{
			return true;
		}
		if (method == MethodName.UpdateSimulationPreview)
		{
			return true;
		}
		if (method == MethodName.BuildPreviewDescription)
		{
			return true;
		}
		if (method == MethodName.CalculateResultCost)
		{
			return true;
		}
		if (method == MethodName.ConfigurePacketCard)
		{
			return true;
		}
		if (method == MethodName.SelectOptionById)
		{
			return true;
		}
		if (method == MethodName.EmptyName)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.AddItemIfMissing)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editingEvent)
		{
			_editingEvent = VariantUtils.ConvertTo<CardActionBehaviorDefinition>(in value);
			return true;
		}
		if (name == PropertyName._eventType)
		{
			_eventType = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._formHint)
		{
			_formHint = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._outcome)
		{
			_outcome = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._currentName)
		{
			_currentName = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._resultName)
		{
			_resultName = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._deletionStamp)
		{
			_deletionStamp = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._currentCardRoot)
		{
			_currentCardRoot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._resultCardRoot)
		{
			_resultCardRoot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._currentPacket)
		{
			_currentPacket = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName._resultPacket)
		{
			_resultPacket = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName._baseFields)
		{
			_baseFields = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._changeCostFields)
		{
			_changeCostFields = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._cooldownFields)
		{
			_cooldownFields = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._changePacketFields)
		{
			_changePacketFields = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._resourceNameEdit)
		{
			_resourceNameEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._localToSceneCheck)
		{
			_localToSceneCheck = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._costMethod)
		{
			_costMethod = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._costValue)
		{
			_costValue = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._costMin)
		{
			_costMin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._costMax)
		{
			_costMax = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._cooldownValue)
		{
			_cooldownValue = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._packetName)
		{
			_packetName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._replaceCount)
		{
			_replaceCount = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._initialCost)
		{
			_initialCost = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._cooldownDuration)
		{
			_cooldownDuration = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._simulatedUses)
		{
			_simulatedUses = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._previewTween)
		{
			_previewTween = VariantUtils.ConvertTo<Tween>(in value);
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
		if (name == PropertyName._editingEvent)
		{
			value = VariantUtils.CreateFrom(in _editingEvent);
			return true;
		}
		if (name == PropertyName._eventType)
		{
			value = VariantUtils.CreateFrom(in _eventType);
			return true;
		}
		if (name == PropertyName._formHint)
		{
			value = VariantUtils.CreateFrom(in _formHint);
			return true;
		}
		if (name == PropertyName._outcome)
		{
			value = VariantUtils.CreateFrom(in _outcome);
			return true;
		}
		if (name == PropertyName._currentName)
		{
			value = VariantUtils.CreateFrom(in _currentName);
			return true;
		}
		if (name == PropertyName._resultName)
		{
			value = VariantUtils.CreateFrom(in _resultName);
			return true;
		}
		if (name == PropertyName._deletionStamp)
		{
			value = VariantUtils.CreateFrom(in _deletionStamp);
			return true;
		}
		if (name == PropertyName._currentCardRoot)
		{
			value = VariantUtils.CreateFrom(in _currentCardRoot);
			return true;
		}
		if (name == PropertyName._resultCardRoot)
		{
			value = VariantUtils.CreateFrom(in _resultCardRoot);
			return true;
		}
		if (name == PropertyName._currentPacket)
		{
			value = VariantUtils.CreateFrom(in _currentPacket);
			return true;
		}
		if (name == PropertyName._resultPacket)
		{
			value = VariantUtils.CreateFrom(in _resultPacket);
			return true;
		}
		if (name == PropertyName._baseFields)
		{
			value = VariantUtils.CreateFrom(in _baseFields);
			return true;
		}
		if (name == PropertyName._changeCostFields)
		{
			value = VariantUtils.CreateFrom(in _changeCostFields);
			return true;
		}
		if (name == PropertyName._cooldownFields)
		{
			value = VariantUtils.CreateFrom(in _cooldownFields);
			return true;
		}
		if (name == PropertyName._changePacketFields)
		{
			value = VariantUtils.CreateFrom(in _changePacketFields);
			return true;
		}
		if (name == PropertyName._resourceNameEdit)
		{
			value = VariantUtils.CreateFrom(in _resourceNameEdit);
			return true;
		}
		if (name == PropertyName._localToSceneCheck)
		{
			value = VariantUtils.CreateFrom(in _localToSceneCheck);
			return true;
		}
		if (name == PropertyName._costMethod)
		{
			value = VariantUtils.CreateFrom(in _costMethod);
			return true;
		}
		if (name == PropertyName._costValue)
		{
			value = VariantUtils.CreateFrom(in _costValue);
			return true;
		}
		if (name == PropertyName._costMin)
		{
			value = VariantUtils.CreateFrom(in _costMin);
			return true;
		}
		if (name == PropertyName._costMax)
		{
			value = VariantUtils.CreateFrom(in _costMax);
			return true;
		}
		if (name == PropertyName._cooldownValue)
		{
			value = VariantUtils.CreateFrom(in _cooldownValue);
			return true;
		}
		if (name == PropertyName._packetName)
		{
			value = VariantUtils.CreateFrom(in _packetName);
			return true;
		}
		if (name == PropertyName._replaceCount)
		{
			value = VariantUtils.CreateFrom(in _replaceCount);
			return true;
		}
		if (name == PropertyName._initialCost)
		{
			value = VariantUtils.CreateFrom(in _initialCost);
			return true;
		}
		if (name == PropertyName._cooldownDuration)
		{
			value = VariantUtils.CreateFrom(in _cooldownDuration);
			return true;
		}
		if (name == PropertyName._simulatedUses)
		{
			value = VariantUtils.CreateFrom(in _simulatedUses);
			return true;
		}
		if (name == PropertyName._previewTween)
		{
			value = VariantUtils.CreateFrom(in _previewTween);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._editingEvent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._formHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._outcome, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._currentName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._deletionStamp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._currentCardRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultCardRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._currentPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._baseFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._changeCostFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cooldownFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._changePacketFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceNameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToSceneCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._costMethod, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._costValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._costMin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._costMax, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cooldownValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._replaceCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._initialCost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cooldownDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulatedUses, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingEvent, Variant.From(in _editingEvent));
		info.AddProperty(PropertyName._eventType, Variant.From(in _eventType));
		info.AddProperty(PropertyName._formHint, Variant.From(in _formHint));
		info.AddProperty(PropertyName._outcome, Variant.From(in _outcome));
		info.AddProperty(PropertyName._currentName, Variant.From(in _currentName));
		info.AddProperty(PropertyName._resultName, Variant.From(in _resultName));
		info.AddProperty(PropertyName._deletionStamp, Variant.From(in _deletionStamp));
		info.AddProperty(PropertyName._currentCardRoot, Variant.From(in _currentCardRoot));
		info.AddProperty(PropertyName._resultCardRoot, Variant.From(in _resultCardRoot));
		info.AddProperty(PropertyName._currentPacket, Variant.From(in _currentPacket));
		info.AddProperty(PropertyName._resultPacket, Variant.From(in _resultPacket));
		info.AddProperty(PropertyName._baseFields, Variant.From(in _baseFields));
		info.AddProperty(PropertyName._changeCostFields, Variant.From(in _changeCostFields));
		info.AddProperty(PropertyName._cooldownFields, Variant.From(in _cooldownFields));
		info.AddProperty(PropertyName._changePacketFields, Variant.From(in _changePacketFields));
		info.AddProperty(PropertyName._resourceNameEdit, Variant.From(in _resourceNameEdit));
		info.AddProperty(PropertyName._localToSceneCheck, Variant.From(in _localToSceneCheck));
		info.AddProperty(PropertyName._costMethod, Variant.From(in _costMethod));
		info.AddProperty(PropertyName._costValue, Variant.From(in _costValue));
		info.AddProperty(PropertyName._costMin, Variant.From(in _costMin));
		info.AddProperty(PropertyName._costMax, Variant.From(in _costMax));
		info.AddProperty(PropertyName._cooldownValue, Variant.From(in _cooldownValue));
		info.AddProperty(PropertyName._packetName, Variant.From(in _packetName));
		info.AddProperty(PropertyName._replaceCount, Variant.From(in _replaceCount));
		info.AddProperty(PropertyName._initialCost, Variant.From(in _initialCost));
		info.AddProperty(PropertyName._cooldownDuration, Variant.From(in _cooldownDuration));
		info.AddProperty(PropertyName._simulatedUses, Variant.From(in _simulatedUses));
		info.AddProperty(PropertyName._previewTween, Variant.From(in _previewTween));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingEvent, out var value))
		{
			_editingEvent = value.As<CardActionBehaviorDefinition>();
		}
		if (info.TryGetProperty(PropertyName._eventType, out var value2))
		{
			_eventType = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._formHint, out var value3))
		{
			_formHint = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._outcome, out var value4))
		{
			_outcome = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._currentName, out var value5))
		{
			_currentName = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resultName, out var value6))
		{
			_resultName = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._deletionStamp, out var value7))
		{
			_deletionStamp = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._currentCardRoot, out var value8))
		{
			_currentCardRoot = value8.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._resultCardRoot, out var value9))
		{
			_resultCardRoot = value9.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._currentPacket, out var value10))
		{
			_currentPacket = value10.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._resultPacket, out var value11))
		{
			_resultPacket = value11.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._baseFields, out var value12))
		{
			_baseFields = value12.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._changeCostFields, out var value13))
		{
			_changeCostFields = value13.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._cooldownFields, out var value14))
		{
			_cooldownFields = value14.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._changePacketFields, out var value15))
		{
			_changePacketFields = value15.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._resourceNameEdit, out var value16))
		{
			_resourceNameEdit = value16.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToSceneCheck, out var value17))
		{
			_localToSceneCheck = value17.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._costMethod, out var value18))
		{
			_costMethod = value18.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._costValue, out var value19))
		{
			_costValue = value19.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._costMin, out var value20))
		{
			_costMin = value20.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._costMax, out var value21))
		{
			_costMax = value21.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._cooldownValue, out var value22))
		{
			_cooldownValue = value22.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._packetName, out var value23))
		{
			_packetName = value23.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._replaceCount, out var value24))
		{
			_replaceCount = value24.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._initialCost, out var value25))
		{
			_initialCost = value25.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._cooldownDuration, out var value26))
		{
			_cooldownDuration = value26.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._simulatedUses, out var value27))
		{
			_simulatedUses = value27.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._previewTween, out var value28))
		{
			_previewTween = value28.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value29))
		{
			_updatingControls = value29.As<bool>();
		}
	}
}
