using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketCostRuleVisualResourceEditor.cs")]
public class XWPacketCostRuleVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName HasCompletePacketCostRuleVisualCoverage = "HasCompletePacketCostRuleVisualCoverage";

		public static readonly StringName DisposePacketCostRuleBinding = "DisposePacketCostRuleBinding";

		public static readonly StringName BindWorkbench = "BindWorkbench";

		public static readonly StringName BindDirectEditControls = "BindDirectEditControls";

		public static readonly StringName BindConditionSignals = "BindConditionSignals";

		public static readonly StringName BeginSelectedAmountEdit = "BeginSelectedAmountEdit";

		public static readonly StringName PreviewSelectedAmount = "PreviewSelectedAmount";

		public static readonly StringName CommitSelectedAmountEdit = "CommitSelectedAmountEdit";

		public static readonly StringName OnPacketTypeSelected = "OnPacketTypeSelected";

		public static readonly StringName PopulateConditionFields = "PopulateConditionFields";

		public static readonly StringName PopulateSelectedAmount = "PopulateSelectedAmount";

		public static readonly StringName OnPacketCostRulePropertyEdited = "OnPacketCostRulePropertyEdited";

		public static readonly StringName RefreshDirectEditPreview = "RefreshDirectEditPreview";

		public static readonly StringName RefreshPacketCostRuleFromHistory = "RefreshPacketCostRuleFromHistory";

		public static readonly StringName ApplyCostRulePreview = "ApplyCostRulePreview";

		public static readonly StringName UpdateSimulationPreview = "UpdateSimulationPreview";

		public static readonly StringName BuildRuleStateText = "BuildRuleStateText";

		public static readonly StringName CalculateResultCost = "CalculateResultCost";

		public static readonly StringName ConfigurePacketCard = "ConfigurePacketCard";

		public static readonly StringName LoadPacketTexture = "LoadPacketTexture";

		public static readonly StringName GetPacketTexturePath = "GetPacketTexturePath";

		public static readonly StringName GetPacketTypeDisplayName = "GetPacketTypeDisplayName";

		public static readonly StringName SelectOptionByText = "SelectOptionByText";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingCondition = "_editingCondition";

		public static readonly StringName _selectedPacketType = "_selectedPacketType";

		public static readonly StringName _ruleKeyBadge = "_ruleKeyBadge";

		public static readonly StringName _ruleState = "_ruleState";

		public static readonly StringName _currentName = "_currentName";

		public static readonly StringName _resultName = "_resultName";

		public static readonly StringName _resultCardRoot = "_resultCardRoot";

		public static readonly StringName _currentPacket = "_currentPacket";

		public static readonly StringName _resultPacket = "_resultPacket";

		public static readonly StringName _initialCost = "_initialCost";

		public static readonly StringName _packetType = "_packetType";

		public static readonly StringName _canChangeCost = "_canChangeCost";

		public static readonly StringName _method = "_method";

		public static readonly StringName _amount = "_amount";

		public static readonly StringName _resourceName = "_resourceName";

		public static readonly StringName _localToScene = "_localToScene";

		public static readonly StringName _key = "_key";

		public static readonly StringName _lockCost = "_lockCost";

		public static readonly StringName _skip = "_skip";

		public static readonly StringName _previewTween = "_previewTween";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string VisualEditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketCostRuleVisualEditorLayout.tscn";

	private static PackedScene _visualEditorLayoutScene;

	private static readonly TowerDefenseEnum.PACKET_TYPE[] PacketTypes = new TowerDefenseEnum.PACKET_TYPE[9]
	{
		TowerDefenseEnum.PACKET_TYPE.WHITE,
		TowerDefenseEnum.PACKET_TYPE.GOLD,
		TowerDefenseEnum.PACKET_TYPE.DIAMOND,
		TowerDefenseEnum.PACKET_TYPE.COLOUR,
		TowerDefenseEnum.PACKET_TYPE.STAR,
		TowerDefenseEnum.PACKET_TYPE.ORIGINAL,
		TowerDefenseEnum.PACKET_TYPE.ZOMBIE,
		TowerDefenseEnum.PACKET_TYPE.COVER,
		TowerDefenseEnum.PACKET_TYPE.GRAY
	};

	private static readonly System.Collections.Generic.Dictionary<string, Texture2D> PacketTextureCache = new System.Collections.Generic.Dictionary<string, Texture2D>(StringComparer.Ordinal);

	private TowerDefensePacketChangeCost _editingCondition;

	private TowerDefenseEnum.PACKET_TYPE _selectedPacketType;

	private Label _ruleKeyBadge;

	private Label _ruleState;

	private Label _currentName;

	private Label _resultName;

	private Control _resultCardRoot;

	private TowerDefenseInGamePacketShow _currentPacket;

	private TowerDefenseInGamePacketShow _resultPacket;

	private SpinBox _initialCost;

	private OptionButton _packetType;

	private XWVisualOptionGallery _packetTypeVisualGallery;

	private CheckBox _canChangeCost;

	private OptionButton _method;

	private XWVisualSegmentedOption _methodVisualChoices;

	private SpinBox _amount;

	private LineEdit _resourceName;

	private CheckButton _localToScene;

	private LineEdit _key;

	private CheckBox _lockCost;

	private CheckBox _skip;

	private XWVisualPropertyBinding _propertyBinding;

	private Tween _previewTween;

	private bool _updatingControls;

	public override void _ExitTree()
	{
		DisposePacketCostRuleBinding();
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposePacketCostRuleBinding();
		if (CurrentResource is TowerDefensePacketChangeCost towerDefensePacketChangeCost && CanvasGrid != null)
		{
			_editingCondition = towerDefensePacketChangeCost;
			CanvasGrid.Columns = 1;
			if (_visualEditorLayoutScene == null)
			{
				_visualEditorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketCostRuleVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _visualEditorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindWorkbench(vBoxContainer, towerDefensePacketChangeCost);
				PopulateConditionFields();
				UpdateSimulationPreview(applied: false);
				AddSummaryRows();
			}
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompletePacketCostRuleVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompletePacketCostRuleVisualCoverage(Resource resource)
	{
		return resource?.GetType() == typeof(TowerDefensePacketChangeCost);
	}

	private void DisposePacketCostRuleBinding()
	{
		_packetTypeVisualGallery?.Dispose();
		_packetTypeVisualGallery = null;
		_methodVisualChoices?.Dispose();
		_methodVisualChoices = null;
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		_editingCondition = null;
	}

	private void BindWorkbench(VBoxContainer root, TowerDefensePacketChangeCost condition)
	{
		_ruleKeyBadge = root.GetNode<Label>("%RuleKeyBadge");
		_ruleState = root.GetNode<Label>("%RuleState");
		_currentName = root.GetNode<Label>("%CurrentName");
		_resultName = root.GetNode<Label>("%ResultName");
		_resultCardRoot = root.GetNode<Control>("%ResultCardRoot");
		_currentPacket = root.GetNode<TowerDefenseInGamePacketShow>("%CurrentPacket");
		_resultPacket = root.GetNode<TowerDefenseInGamePacketShow>("%ResultPacket");
		_initialCost = root.GetNode<SpinBox>("%InitialCost");
		_packetType = root.GetNode<OptionButton>("%PacketType");
		_canChangeCost = root.GetNode<CheckBox>("%CanChangeCost");
		_method = root.GetNode<OptionButton>("%Method");
		_amount = root.GetNode<SpinBox>("%Amount");
		_resourceName = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToScene = root.GetNode<CheckButton>("%LocalToSceneCheck");
		_key = root.GetNode<LineEdit>("%Key");
		_lockCost = root.GetNode<CheckBox>("%LockCost");
		_skip = root.GetNode<CheckBox>("%Skip");
		BindDirectEditControls(condition);
		TowerDefenseEnum.PACKET_TYPE[] packetTypes = PacketTypes;
		foreach (TowerDefenseEnum.PACKET_TYPE pACKET_TYPE in packetTypes)
		{
			_packetType.AddItem(GetPacketTypeDisplayName(pACKET_TYPE), (int)pACKET_TYPE);
		}
		_packetTypeVisualGallery = new XWVisualOptionGallery(_packetType, root.GetNode<HFlowContainer>("%PacketTypeVisualChoices"), "PacketTypeVisualCatalog", (int index) => LoadPacketTexture((TowerDefenseEnum.PACKET_TYPE)_packetType.GetItemId(index)));
		_packetTypeVisualGallery.Rebuild();
		_methodVisualChoices = new XWVisualSegmentedOption(_method, root.GetNode<HFlowContainer>("%MethodVisualChoices"));
		_methodVisualChoices.Rebuild();
		ConfigurePacketCard(_currentPacket, 100, _selectedPacketType);
		ConfigurePacketCard(_resultPacket, 100, _selectedPacketType);
		root.GetNode<Button>("%ApplyButton").Pressed += ApplyCostRulePreview;
		root.GetNode<Button>("%ResetButton").Pressed += () =>
		{
			UpdateSimulationPreview(applied: false);
		};
		_initialCost.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview(applied: false);
		};
		_canChangeCost.Toggled += (bool _) =>
		{
			UpdateSimulationPreview(applied: false);
		};
		_packetType.ItemSelected += OnPacketTypeSelected;
		BindConditionSignals();
	}

	private void BindDirectEditControls(TowerDefensePacketChangeCost condition)
	{
		_propertyBinding?.Dispose();
		_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnPacketCostRulePropertyEdited);
		_propertyBinding.BindText(_resourceName, condition, "resource_name", RefreshDirectEditPreview, this, "RefreshPacketCostRuleFromHistory");
		_propertyBinding.BindToggle(_localToScene, condition, "resource_local_to_scene", RefreshDirectEditPreview, this, "RefreshPacketCostRuleFromHistory");
		_propertyBinding.BindText(_key, condition, "key", RefreshDirectEditPreview, this, "RefreshPacketCostRuleFromHistory");
		_propertyBinding.BindToggle(_lockCost, condition, "lockCost", RefreshDirectEditPreview, this, "RefreshPacketCostRuleFromHistory");
		_propertyBinding.BindToggle(_skip, condition, "skip", RefreshDirectEditPreview, this, "RefreshPacketCostRuleFromHistory");
	}

	private void BindConditionSignals()
	{
		_method.ItemSelected += (long index) =>
		{
			if (!_updatingControls && _propertyBinding != null && GodotObject.IsInstanceValid(_editingCondition))
			{
				TowerDefensePacketChangeCost editingCondition = _editingCondition;
				_propertyBinding.SetValue(editingCondition, "method", _method.GetItemText((int)index), "修改费用修正方式", this, "RefreshPacketCostRuleFromHistory");
			}
		};
		_amount.FocusEntered += BeginSelectedAmountEdit;
		_amount.ValueChanged += PreviewSelectedAmount;
		_amount.FocusExited += CommitSelectedAmountEdit;
	}

	private void BeginSelectedAmountEdit()
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_editingCondition))
		{
			_propertyBinding.BeginEdit(_editingCondition, "amontDictionary");
		}
	}

	private void PreviewSelectedAmount(double value)
	{
		if (!_updatingControls && _propertyBinding != null && GodotObject.IsInstanceValid(_editingCondition))
		{
			Dictionary dictionary = ((_editingCondition.amontDictionary == null) ? new Dictionary() : _editingCondition.amontDictionary.Duplicate());
			dictionary[(int)_selectedPacketType] = Mathf.RoundToInt(value);
			_propertyBinding.PreviewValue(_editingCondition, "amontDictionary", dictionary);
		}
	}

	private void CommitSelectedAmountEdit()
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_editingCondition))
		{
			_propertyBinding.CommitEdit(_editingCondition, "amontDictionary", _editingCondition.amontDictionary, "修改" + GetPacketTypeDisplayName(_selectedPacketType) + "费用", this, "RefreshPacketCostRuleFromHistory");
		}
	}

	private void OnPacketTypeSelected(long index)
	{
		if (index >= 0 && index < _packetType.ItemCount)
		{
			_selectedPacketType = (TowerDefenseEnum.PACKET_TYPE)_packetType.GetItemId((int)index);
			PopulateSelectedAmount();
			UpdateSimulationPreview(applied: false);
		}
	}

	private void PopulateConditionFields()
	{
		if (!GodotObject.IsInstanceValid(_editingCondition) || !GodotObject.IsInstanceValid(_method))
		{
			return;
		}
		_updatingControls = true;
		try
		{
			SelectOptionByText(_method, _editingCondition.method);
			_methodVisualChoices?.RefreshSelection();
			PopulateSelectedAmount();
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void PopulateSelectedAmount()
	{
		if (!GodotObject.IsInstanceValid(_editingCondition) || !GodotObject.IsInstanceValid(_amount))
		{
			return;
		}
		bool updatingControls = _updatingControls;
		_updatingControls = true;
		try
		{
			int selectedPacketType = (int)_selectedPacketType;
			_amount.Value = (TryGetAmount(_editingCondition, selectedPacketType, out var amount) ? amount : 0);
		}
		finally
		{
			_updatingControls = updatingControls;
		}
	}

	private void OnPacketCostRulePropertyEdited(bool committed)
	{
		if (CurrentResource is TowerDefensePacketChangeCost towerDefensePacketChangeCost && towerDefensePacketChangeCost == _editingCondition)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
			}
			else
			{
				MarkCurrentResourceDirty();
				towerDefensePacketChangeCost.EmitChanged();
			}
			UpdateSimulationPreview(applied: false);
		}
	}

	private void RefreshDirectEditPreview()
	{
		UpdateSimulationPreview(applied: false);
	}

	public void RefreshPacketCostRuleFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()) && CurrentResource is TowerDefensePacketChangeCost towerDefensePacketChangeCost && towerDefensePacketChangeCost == _editingCondition)
		{
			BindDirectEditControls(towerDefensePacketChangeCost);
			PopulateConditionFields();
			UpdateSimulationPreview(applied: false);
		}
	}

	private void ApplyCostRulePreview()
	{
		UpdateSimulationPreview(applied: true);
		if (GodotObject.IsInstanceValid(_resultCardRoot))
		{
			if (GodotObject.IsInstanceValid(_previewTween))
			{
				_previewTween.Kill();
			}
			_resultCardRoot.Scale = Vector2.One * 0.78f;
			_previewTween = CreateTween();
			_previewTween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
			_previewTween.TweenProperty(_resultCardRoot, "scale", Vector2.One * 1.08f, 0.22);
			_previewTween.TweenProperty(_resultCardRoot, "scale", Vector2.One, 0.14);
		}
	}

	private void UpdateSimulationPreview(bool applied)
	{
		if (GodotObject.IsInstanceValid(_editingCondition) && GodotObject.IsInstanceValid(_currentPacket))
		{
			if (GodotObject.IsInstanceValid(_previewTween))
			{
				_previewTween.Kill();
				_previewTween = null;
			}
			int num = Mathf.RoundToInt(_initialCost.Value);
			bool buttonPressed = _canChangeCost.ButtonPressed;
			int num2 = (applied ? CalculateResultCost(num, _editingCondition, _selectedPacketType, buttonPressed) : num);
			ConfigurePacketCard(_currentPacket, num, _selectedPacketType);
			ConfigurePacketCard(_resultPacket, num2, _selectedPacketType);
			_currentName.Text = $"费用 {num}";
			_resultName.Text = (applied ? $"费用 {num} → {num2}" : $"费用 {num}");
			_ruleKeyBadge.Text = (string.IsNullOrWhiteSpace(_editingCondition.key) ? "未命名规则" : _editingCondition.key);
			_ruleState.Text = BuildRuleStateText(num, num2, buttonPressed, applied);
			_resultCardRoot.Scale = Vector2.One;
		}
	}

	private string BuildRuleStateText(int initialCost, int resultCost, bool canChangeCost, bool applied)
	{
		string value = (TryGetAmount(_editingCondition, (int)_selectedPacketType, out var amount) ? amount.ToString() : "未配置");
		string value2 = (_editingCondition.lockCost ? " · 应用后锁定后续增费" : "") + (_editingCondition.skip ? " · 应用后跳过剩余规则" : "");
		if (applied)
		{
			if (_editingCondition.method == "Increase" && !canChangeCost)
			{
				return $"卡牌已禁止增加费用，Increase 被拦截：{initialCost} → {resultCost}{value2}";
			}
			return $"{GetPacketTypeDisplayName(_selectedPacketType)} 已执行 {_editingCondition.method}：{initialCost} → {resultCost}{value2}";
		}
		return $"{GetPacketTypeDisplayName(_selectedPacketType)}：{_editingCondition.method} {value}；点击应用查看真实卡框费用变化{value2}";
	}

	private static int CalculateResultCost(int initialCost, TowerDefensePacketChangeCost condition, TowerDefenseEnum.PACKET_TYPE packetType, bool canChangeCost)
	{
		if (!TryGetAmount(condition, (int)packetType, out var amount))
		{
			return initialCost;
		}
		switch (condition.method)
		{
		case "Increase":
			if (canChangeCost)
			{
				return initialCost + amount;
			}
			break;
		case "Decrease":
			return initialCost - amount;
		case "Set":
			return amount;
		}
		return initialCost;
	}

	private static bool TryGetAmount(TowerDefensePacketChangeCost condition, int typeKey, out int amount)
	{
		amount = 0;
		if (condition?.amontDictionary == null || !condition.amontDictionary.ContainsKey(typeKey))
		{
			return false;
		}
		amount = condition.amontDictionary[typeKey].AsInt32();
		return true;
	}

	private static void ConfigurePacketCard(TowerDefenseInGamePacketShow packet, int cost, TowerDefenseEnum.PACKET_TYPE packetType)
	{
		if (GodotObject.IsInstanceValid(packet))
		{
			packet.ProcessMode = ProcessModeEnum.Disabled;
			packet.onlyDraw = true;
			packet.useCost = false;
			packet.baseItemCost = cost;
			packet.itemCost = cost;
			packet.alive = true;
			packet.@lock = false;
			packet.openShadow = false;
			packet.select = false;
			packet.coldDownOpen = false;
			if (GodotObject.IsInstanceValid(packet.coldDownProgressBar))
			{
				packet.coldDownProgressBar.Visible = false;
			}
			if (GodotObject.IsInstanceValid(packet.backgroundTexture))
			{
				packet.backgroundTexture.Texture = LoadPacketTexture(packetType);
			}
			packet.ColorSet();
		}
	}

	private static Texture2D LoadPacketTexture(TowerDefenseEnum.PACKET_TYPE packetType)
	{
		string packetTexturePath = GetPacketTexturePath(packetType);
		if (!PacketTextureCache.TryGetValue(packetTexturePath, out var value) || !GodotObject.IsInstanceValid(value))
		{
			value = ResourceLoader.Load<Texture2D>(packetTexturePath, null, ResourceLoader.CacheMode.Reuse);
			PacketTextureCache[packetTexturePath] = value;
		}
		return value;
	}

	private static string GetPacketTexturePath(TowerDefenseEnum.PACKET_TYPE packetType)
	{
		return "res://Asset/Texture/TowerDefense/Packet/PC/" + packetType switch
		{
			TowerDefenseEnum.PACKET_TYPE.GOLD => "PacketGold.png", 
			TowerDefenseEnum.PACKET_TYPE.DIAMOND => "PacketDiamond.png", 
			TowerDefenseEnum.PACKET_TYPE.COLOUR => "PacketColour.png", 
			TowerDefenseEnum.PACKET_TYPE.STAR => "PacketStar.png", 
			TowerDefenseEnum.PACKET_TYPE.ZOMBIE => "PacketZombie.png", 
			TowerDefenseEnum.PACKET_TYPE.COVER => "PacketCover.png", 
			TowerDefenseEnum.PACKET_TYPE.GRAY => "PacketGray.png", 
			_ => "PacketNormal.png", 
		};
	}

	private static string GetPacketTypeDisplayName(TowerDefenseEnum.PACKET_TYPE packetType)
	{
		return packetType switch
		{
			TowerDefenseEnum.PACKET_TYPE.WHITE => "普通白卡", 
			TowerDefenseEnum.PACKET_TYPE.GOLD => "金卡", 
			TowerDefenseEnum.PACKET_TYPE.DIAMOND => "钻石卡", 
			TowerDefenseEnum.PACKET_TYPE.COLOUR => "彩卡", 
			TowerDefenseEnum.PACKET_TYPE.STAR => "星卡", 
			TowerDefenseEnum.PACKET_TYPE.ORIGINAL => "原版卡", 
			TowerDefenseEnum.PACKET_TYPE.ZOMBIE => "僵尸卡", 
			TowerDefenseEnum.PACKET_TYPE.COVER => "覆盖卡", 
			TowerDefenseEnum.PACKET_TYPE.GRAY => "灰卡", 
			_ => packetType.ToString(), 
		};
	}

	private static void SelectOptionByText(OptionButton option, string text)
	{
		for (int i = 0; i < option.ItemCount; i++)
		{
			if (string.Equals(option.GetItemText(i), text, StringComparison.Ordinal))
			{
				option.Select(i);
				return;
			}
		}
		option.Select(0);
	}

	private void AddSummaryRows()
	{
		AddItemIfMissing(PreviewList, "真实种子栏卡框：费用修正前 → 修正后");
		AddItemIfMissing(TimelineList, "选择卡框 → 编辑该类型数值 → 安全演算");
		AddItemIfMissing(GraphList, "卡牌费用规则 → Increase / Decrease / Set（不调用 Execute）");
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
		return new List<MethodInfo>(26)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCompletePacketCostRuleVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposePacketCostRuleBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindDirectEditControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindConditionSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginSelectedAmountEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewSelectedAmount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitSelectedAmountEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPacketTypeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateConditionFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateSelectedAmount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPacketCostRulePropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshDirectEditPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPacketCostRuleFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyCostRulePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSimulationPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "applied", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildRuleStateText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "initialCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resultCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canChangeCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "applied", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CalculateResultCost, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "initialCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canChangeCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigurePacketCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadPacketTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketTexturePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketTypeDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectOptionByText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.HasCompletePacketCostRuleVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompletePacketCostRuleVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.DisposePacketCostRuleBinding && args.Count == 0)
		{
			DisposePacketCostRuleBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.BindWorkbench && args.Count == 2)
		{
			BindWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindDirectEditControls && args.Count == 1)
		{
			BindDirectEditControls(VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindConditionSignals && args.Count == 0)
		{
			BindConditionSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginSelectedAmountEdit && args.Count == 0)
		{
			BeginSelectedAmountEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewSelectedAmount && args.Count == 1)
		{
			PreviewSelectedAmount(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitSelectedAmountEdit && args.Count == 0)
		{
			CommitSelectedAmountEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPacketTypeSelected && args.Count == 1)
		{
			OnPacketTypeSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateConditionFields && args.Count == 0)
		{
			PopulateConditionFields();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateSelectedAmount && args.Count == 0)
		{
			PopulateSelectedAmount();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPacketCostRulePropertyEdited && args.Count == 1)
		{
			OnPacketCostRulePropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDirectEditPreview && args.Count == 0)
		{
			RefreshDirectEditPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketCostRuleFromHistory && args.Count == 0)
		{
			RefreshPacketCostRuleFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCostRulePreview && args.Count == 0)
		{
			ApplyCostRulePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSimulationPreview && args.Count == 1)
		{
			UpdateSimulationPreview(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRuleStateText && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<string>(BuildRuleStateText(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.CalculateResultCost && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<int>(CalculateResultCost(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.ConfigurePacketCard && args.Count == 3)
		{
			ConfigurePacketCard(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPacketTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadPacketTexture(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketTexturePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPacketTexturePath(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketTypeDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPacketTypeDisplayName(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectOptionByText && args.Count == 2)
		{
			SelectOptionByText(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
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
		if (method == MethodName.HasCompletePacketCostRuleVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompletePacketCostRuleVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.CalculateResultCost && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<int>(CalculateResultCost(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.ConfigurePacketCard && args.Count == 3)
		{
			ConfigurePacketCard(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPacketTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadPacketTexture(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketTexturePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPacketTexturePath(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketTypeDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPacketTypeDisplayName(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectOptionByText && args.Count == 2)
		{
			SelectOptionByText(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
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
		if (method == MethodName.HasCompletePacketCostRuleVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.DisposePacketCostRuleBinding)
		{
			return true;
		}
		if (method == MethodName.BindWorkbench)
		{
			return true;
		}
		if (method == MethodName.BindDirectEditControls)
		{
			return true;
		}
		if (method == MethodName.BindConditionSignals)
		{
			return true;
		}
		if (method == MethodName.BeginSelectedAmountEdit)
		{
			return true;
		}
		if (method == MethodName.PreviewSelectedAmount)
		{
			return true;
		}
		if (method == MethodName.CommitSelectedAmountEdit)
		{
			return true;
		}
		if (method == MethodName.OnPacketTypeSelected)
		{
			return true;
		}
		if (method == MethodName.PopulateConditionFields)
		{
			return true;
		}
		if (method == MethodName.PopulateSelectedAmount)
		{
			return true;
		}
		if (method == MethodName.OnPacketCostRulePropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshDirectEditPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketCostRuleFromHistory)
		{
			return true;
		}
		if (method == MethodName.ApplyCostRulePreview)
		{
			return true;
		}
		if (method == MethodName.UpdateSimulationPreview)
		{
			return true;
		}
		if (method == MethodName.BuildRuleStateText)
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
		if (method == MethodName.LoadPacketTexture)
		{
			return true;
		}
		if (method == MethodName.GetPacketTexturePath)
		{
			return true;
		}
		if (method == MethodName.GetPacketTypeDisplayName)
		{
			return true;
		}
		if (method == MethodName.SelectOptionByText)
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
		if (name == PropertyName._editingCondition)
		{
			_editingCondition = VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in value);
			return true;
		}
		if (name == PropertyName._selectedPacketType)
		{
			_selectedPacketType = VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in value);
			return true;
		}
		if (name == PropertyName._ruleKeyBadge)
		{
			_ruleKeyBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._ruleState)
		{
			_ruleState = VariantUtils.ConvertTo<Label>(in value);
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
		if (name == PropertyName._initialCost)
		{
			_initialCost = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._packetType)
		{
			_packetType = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._canChangeCost)
		{
			_canChangeCost = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._method)
		{
			_method = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._amount)
		{
			_amount = VariantUtils.ConvertTo<SpinBox>(in value);
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
		if (name == PropertyName._key)
		{
			_key = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._lockCost)
		{
			_lockCost = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._skip)
		{
			_skip = VariantUtils.ConvertTo<CheckBox>(in value);
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
		if (name == PropertyName._editingCondition)
		{
			value = VariantUtils.CreateFrom(in _editingCondition);
			return true;
		}
		if (name == PropertyName._selectedPacketType)
		{
			value = VariantUtils.CreateFrom(in _selectedPacketType);
			return true;
		}
		if (name == PropertyName._ruleKeyBadge)
		{
			value = VariantUtils.CreateFrom(in _ruleKeyBadge);
			return true;
		}
		if (name == PropertyName._ruleState)
		{
			value = VariantUtils.CreateFrom(in _ruleState);
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
		if (name == PropertyName._initialCost)
		{
			value = VariantUtils.CreateFrom(in _initialCost);
			return true;
		}
		if (name == PropertyName._packetType)
		{
			value = VariantUtils.CreateFrom(in _packetType);
			return true;
		}
		if (name == PropertyName._canChangeCost)
		{
			value = VariantUtils.CreateFrom(in _canChangeCost);
			return true;
		}
		if (name == PropertyName._method)
		{
			value = VariantUtils.CreateFrom(in _method);
			return true;
		}
		if (name == PropertyName._amount)
		{
			value = VariantUtils.CreateFrom(in _amount);
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
		if (name == PropertyName._key)
		{
			value = VariantUtils.CreateFrom(in _key);
			return true;
		}
		if (name == PropertyName._lockCost)
		{
			value = VariantUtils.CreateFrom(in _lockCost);
			return true;
		}
		if (name == PropertyName._skip)
		{
			value = VariantUtils.CreateFrom(in _skip);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._editingCondition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedPacketType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ruleKeyBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ruleState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._currentName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultCardRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._currentPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._initialCost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._canChangeCost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._method, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._amount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._key, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lockCost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._skip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingCondition, Variant.From(in _editingCondition));
		info.AddProperty(PropertyName._selectedPacketType, Variant.From(in _selectedPacketType));
		info.AddProperty(PropertyName._ruleKeyBadge, Variant.From(in _ruleKeyBadge));
		info.AddProperty(PropertyName._ruleState, Variant.From(in _ruleState));
		info.AddProperty(PropertyName._currentName, Variant.From(in _currentName));
		info.AddProperty(PropertyName._resultName, Variant.From(in _resultName));
		info.AddProperty(PropertyName._resultCardRoot, Variant.From(in _resultCardRoot));
		info.AddProperty(PropertyName._currentPacket, Variant.From(in _currentPacket));
		info.AddProperty(PropertyName._resultPacket, Variant.From(in _resultPacket));
		info.AddProperty(PropertyName._initialCost, Variant.From(in _initialCost));
		info.AddProperty(PropertyName._packetType, Variant.From(in _packetType));
		info.AddProperty(PropertyName._canChangeCost, Variant.From(in _canChangeCost));
		info.AddProperty(PropertyName._method, Variant.From(in _method));
		info.AddProperty(PropertyName._amount, Variant.From(in _amount));
		info.AddProperty(PropertyName._resourceName, Variant.From(in _resourceName));
		info.AddProperty(PropertyName._localToScene, Variant.From(in _localToScene));
		info.AddProperty(PropertyName._key, Variant.From(in _key));
		info.AddProperty(PropertyName._lockCost, Variant.From(in _lockCost));
		info.AddProperty(PropertyName._skip, Variant.From(in _skip));
		info.AddProperty(PropertyName._previewTween, Variant.From(in _previewTween));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingCondition, out var value))
		{
			_editingCondition = value.As<TowerDefensePacketChangeCost>();
		}
		if (info.TryGetProperty(PropertyName._selectedPacketType, out var value2))
		{
			_selectedPacketType = value2.As<TowerDefenseEnum.PACKET_TYPE>();
		}
		if (info.TryGetProperty(PropertyName._ruleKeyBadge, out var value3))
		{
			_ruleKeyBadge = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._ruleState, out var value4))
		{
			_ruleState = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._currentName, out var value5))
		{
			_currentName = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resultName, out var value6))
		{
			_resultName = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resultCardRoot, out var value7))
		{
			_resultCardRoot = value7.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._currentPacket, out var value8))
		{
			_currentPacket = value8.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._resultPacket, out var value9))
		{
			_resultPacket = value9.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._initialCost, out var value10))
		{
			_initialCost = value10.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._packetType, out var value11))
		{
			_packetType = value11.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._canChangeCost, out var value12))
		{
			_canChangeCost = value12.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._method, out var value13))
		{
			_method = value13.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._amount, out var value14))
		{
			_amount = value14.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._resourceName, out var value15))
		{
			_resourceName = value15.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToScene, out var value16))
		{
			_localToScene = value16.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._key, out var value17))
		{
			_key = value17.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._lockCost, out var value18))
		{
			_lockCost = value18.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._skip, out var value19))
		{
			_skip = value19.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._previewTween, out var value20))
		{
			_previewTween = value20.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value21))
		{
			_updatingControls = value21.As<bool>();
		}
	}
}
