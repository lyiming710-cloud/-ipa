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

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWShovelVisualResourceEditor.cs")]
public class XWShovelVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName DisposeShovelBinding = "DisposeShovelBinding";

		public static readonly StringName BindShovelWorkbench = "BindShovelWorkbench";

		public static readonly StringName CreateResourcePicker = "CreateResourcePicker";

		public static readonly StringName BindShovelPropertyControls = "BindShovelPropertyControls";

		public static readonly StringName OnShovelVisualPropertyEdited = "OnShovelVisualPropertyEdited";

		public static readonly StringName RefreshShovelEditorFromHistory = "RefreshShovelEditorFromHistory";

		public static readonly StringName PopulateShovelControls = "PopulateShovelControls";

		public static readonly StringName OnTexturePicked = "OnTexturePicked";

		public static readonly StringName AddShovelableName = "AddShovelableName";

		public static readonly StringName RemoveSelectedShovelableName = "RemoveSelectedShovelableName";

		public static readonly StringName SelectShovelableName = "SelectShovelableName";

		public static readonly StringName SelectUnlockCondition = "SelectUnlockCondition";

		public static readonly StringName AddUnlockCondition = "AddUnlockCondition";

		public static readonly StringName ReplaceSelectedUnlockCondition = "ReplaceSelectedUnlockCondition";

		public static readonly StringName RemoveSelectedUnlockCondition = "RemoveSelectedUnlockCondition";

		public static readonly StringName SelectShovelEvent = "SelectShovelEvent";

		public static readonly StringName AddShovelEvent = "AddShovelEvent";

		public static readonly StringName ReplaceSelectedShovelEvent = "ReplaceSelectedShovelEvent";

		public static readonly StringName RemoveSelectedShovelEvent = "RemoveSelectedShovelEvent";

		public static readonly StringName RefreshAfterListEdit = "RefreshAfterListEdit";

		public static readonly StringName BindShovelTargetPreview = "BindShovelTargetPreview";

		public static readonly StringName RefreshShovelTargetPreview = "RefreshShovelTargetPreview";

		public static readonly StringName RebuildShovelGameTarget = "RebuildShovelGameTarget";

		public static readonly StringName UseShovelOnPreviewTarget = "UseShovelOnPreviewTarget";

		public static readonly StringName UpdateShovelPreview = "UpdateShovelPreview";

		public static readonly StringName RefreshShovelLists = "RefreshShovelLists";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName CloneStringArray = "CloneStringArray";

		public static readonly StringName CloneUnlockArray = "CloneUnlockArray";

		public static readonly StringName CloneEventArray = "CloneEventArray";

		public static readonly StringName RestoreSelection = "RestoreSelection";

		public static readonly StringName PopulateStringList = "PopulateStringList";

		public static readonly StringName BuildListSummary = "BuildListSummary";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";

		public static readonly StringName FormatResource = "FormatResource";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingShovel = "_editingShovel";

		public static readonly StringName _texturePreview = "_texturePreview";

		public static readonly StringName _titleLabel = "_titleLabel";

		public static readonly StringName _textSummaryLabel = "_textSummaryLabel";

		public static readonly StringName _listSummaryLabel = "_listSummaryLabel";

		public static readonly StringName _textureResourceLabel = "_textureResourceLabel";

		public static readonly StringName _resourceName = "_resourceName";

		public static readonly StringName _localToScene = "_localToScene";

		public static readonly StringName _saveKey = "_saveKey";

		public static readonly StringName _name = "_name";

		public static readonly StringName _describe = "_describe";

		public static readonly StringName _handbookDescribe = "_handbookDescribe";

		public static readonly StringName _handbookStory = "_handbookStory";

		public static readonly StringName _texturePicker = "_texturePicker";

		public static readonly StringName _unlockPicker = "_unlockPicker";

		public static readonly StringName _eventPicker = "_eventPicker";

		public static readonly StringName _unlockList = "_unlockList";

		public static readonly StringName _eventList = "_eventList";

		public static readonly StringName _shovelableList = "_shovelableList";

		public static readonly StringName _shovelableNameEdit = "_shovelableNameEdit";

		public static readonly StringName _targetTypePreviewOption = "_targetTypePreviewOption";

		public static readonly StringName _targetNamePreviewEdit = "_targetNamePreviewEdit";

		public static readonly StringName _targetHypnosesPreviewCheck = "_targetHypnosesPreviewCheck";

		public static readonly StringName _targetHologramPreviewCheck = "_targetHologramPreviewCheck";

		public static readonly StringName _targetPreviewResultLabel = "_targetPreviewResultLabel";

		public static readonly StringName _shovelTargetRoot = "_shovelTargetRoot";

		public static readonly StringName _shovelPreviewAllowed = "_shovelPreviewAllowed";

		public static readonly StringName _selectedShovelableIndex = "_selectedShovelableIndex";

		public static readonly StringName _selectedUnlockIndex = "_selectedUnlockIndex";

		public static readonly StringName _selectedEventIndex = "_selectedEventIndex";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string VisualEditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWShovelVisualEditorLayout.tscn";

	private static PackedScene _visualEditorLayoutScene;

	private ShovelConfig _editingShovel;

	private XWVisualPropertyBinding _propertyBinding;

	private TextureRect _texturePreview;

	private Label _titleLabel;

	private Label _textSummaryLabel;

	private Label _listSummaryLabel;

	private Label _textureResourceLabel;

	private LineEdit _resourceName;

	private CheckButton _localToScene;

	private LineEdit _saveKey;

	private LineEdit _name;

	private TextEdit _describe;

	private TextEdit _handbookDescribe;

	private TextEdit _handbookStory;

	private XWResourcePicker _texturePicker;

	private XWResourcePicker _unlockPicker;

	private XWResourcePicker _eventPicker;

	private ItemList _unlockList;

	private ItemList _eventList;

	private ItemList _shovelableList;

	private LineEdit _shovelableNameEdit;

	private OptionButton _targetTypePreviewOption;

	private XWVisualSegmentedOption _targetTypeVisualChoices;

	private LineEdit _targetNamePreviewEdit;

	private CheckBox _targetHypnosesPreviewCheck;

	private CheckBox _targetHologramPreviewCheck;

	private Label _targetPreviewResultLabel;

	private Node2D _shovelTargetRoot;

	private bool _shovelPreviewAllowed;

	private int _selectedShovelableIndex = -1;

	private int _selectedUnlockIndex = -1;

	private int _selectedEventIndex = -1;

	private bool _updatingControls;

	public override void _ExitTree()
	{
		DisposeShovelBinding();
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeShovelBinding();
		if (CurrentResource is ShovelConfig shovelConfig && CanvasGrid != null)
		{
			_editingShovel = shovelConfig;
			CanvasGrid.Columns = 1;
			if (_visualEditorLayoutScene == null)
			{
				_visualEditorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWShovelVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _visualEditorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindShovelWorkbench(vBoxContainer);
				BindShovelPropertyControls(shovelConfig);
				PopulateShovelControls();
				UpdateShovelPreview();
				AddSummaryRows(shovelConfig);
			}
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (resource?.GetType() != typeof(ShovelConfig))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private void DisposeShovelBinding()
	{
		_targetTypeVisualChoices?.Dispose();
		_targetTypeVisualChoices = null;
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		_editingShovel = null;
	}

	private void BindShovelWorkbench(VBoxContainer root)
	{
		_texturePreview = root.GetNode<TextureRect>("%TexturePreview");
		_titleLabel = root.GetNode<Label>("%TitleLabel");
		_textSummaryLabel = root.GetNode<Label>("%TextSummaryLabel");
		_listSummaryLabel = root.GetNode<Label>("%ListSummaryLabel");
		_textureResourceLabel = root.GetNode<Label>("%TextureResourceLabel");
		_resourceName = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToScene = root.GetNode<CheckButton>("%LocalToSceneCheck");
		_saveKey = root.GetNode<LineEdit>("%SaveKeyLineEdit");
		_name = root.GetNode<LineEdit>("%NameLineEdit");
		_describe = root.GetNode<TextEdit>("%DescribeTextEdit");
		_handbookDescribe = root.GetNode<TextEdit>("%HandbookDescribeTextEdit");
		_handbookStory = root.GetNode<TextEdit>("%HandbookStoryTextEdit");
		_unlockList = root.GetNode<ItemList>("%UnlockList");
		_eventList = root.GetNode<ItemList>("%EventList");
		_shovelableList = root.GetNode<ItemList>("%ShovelableList");
		_shovelableNameEdit = root.GetNode<LineEdit>("%ShovelableNameLineEdit");
		_texturePicker = CreateResourcePicker(root.GetNode<VBoxContainer>("%TexturePickerHost"), "Texture2D");
		_unlockPicker = CreateResourcePicker(root.GetNode<VBoxContainer>("%UnlockPickerHost"), "UnlockConditionBaseConfig");
		_eventPicker = CreateResourcePicker(root.GetNode<VBoxContainer>("%EventPickerHost"), "ShovelEventConfig");
		_texturePicker.ResourceChanged += OnTexturePicked;
		_unlockList.ItemSelected += (long index) =>
		{
			SelectUnlockCondition((int)index);
		};
		_unlockList.ItemActivated += (long index) =>
		{
			OpenShovelNestedResource(_editingShovel?.unlockCheckList, "unlockCheckList", (int)index);
		};
		_eventList.ItemSelected += (long index) =>
		{
			SelectShovelEvent((int)index);
		};
		_eventList.ItemActivated += (long index) =>
		{
			OpenShovelNestedResource(_editingShovel?.eventList, "eventList", (int)index);
		};
		_shovelableList.ItemSelected += (long index) =>
		{
			SelectShovelableName((int)index);
		};
		root.GetNode<Button>("%AddShovelableButton").Pressed += AddShovelableName;
		root.GetNode<Button>("%RemoveShovelableButton").Pressed += RemoveSelectedShovelableName;
		root.GetNode<Button>("%AddUnlockButton").Pressed += AddUnlockCondition;
		root.GetNode<Button>("%ReplaceUnlockButton").Pressed += ReplaceSelectedUnlockCondition;
		root.GetNode<Button>("%RemoveUnlockButton").Pressed += RemoveSelectedUnlockCondition;
		root.GetNode<Button>("%OpenUnlockButton").Pressed += () =>
		{
			OpenShovelNestedResource(_editingShovel?.unlockCheckList, "unlockCheckList", _selectedUnlockIndex);
		};
		root.GetNode<Button>("%AddEventButton").Pressed += AddShovelEvent;
		root.GetNode<Button>("%ReplaceEventButton").Pressed += ReplaceSelectedShovelEvent;
		root.GetNode<Button>("%RemoveEventButton").Pressed += RemoveSelectedShovelEvent;
		root.GetNode<Button>("%OpenEventButton").Pressed += () =>
		{
			OpenShovelNestedResource(_editingShovel?.eventList, "eventList", _selectedEventIndex);
		};
		BindShovelTargetPreview(root.GetNode<Control>("%TargetPreview"));
	}

	private static XWResourcePicker CreateResourcePicker(VBoxContainer host, string baseType)
	{
		XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
		xWResourcePicker.Setup(baseType);
		xWResourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		host.AddChild(xWResourcePicker, forceReadableName: false, InternalMode.Disabled);
		return xWResourcePicker;
	}

	private void BindShovelPropertyControls(ShovelConfig shovel)
	{
		_propertyBinding?.Dispose();
		_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnShovelVisualPropertyEdited);
		_propertyBinding.BindText(_resourceName, shovel, "resource_name", UpdateShovelPreview, this, "RefreshShovelEditorFromHistory");
		_propertyBinding.BindToggle(_localToScene, shovel, "resource_local_to_scene", UpdateShovelPreview, this, "RefreshShovelEditorFromHistory");
		_propertyBinding.BindText(_saveKey, shovel, "saveKey", UpdateShovelPreview, this, "RefreshShovelEditorFromHistory");
		_propertyBinding.BindText(_name, shovel, "name", UpdateShovelPreview, this, "RefreshShovelEditorFromHistory");
		_propertyBinding.BindText(_describe, shovel, "describe", UpdateShovelPreview, this, "RefreshShovelEditorFromHistory");
		_propertyBinding.BindText(_handbookDescribe, shovel, "handbookDescribe", UpdateShovelPreview, this, "RefreshShovelEditorFromHistory");
		_propertyBinding.BindText(_handbookStory, shovel, "handbookStory", UpdateShovelPreview, this, "RefreshShovelEditorFromHistory");
	}

	private void OnShovelVisualPropertyEdited(bool committed)
	{
		if (CurrentResource is ShovelConfig shovelConfig && shovelConfig == _editingShovel)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
				return;
			}
			MarkCurrentResourceDirty();
			shovelConfig.EmitChanged();
		}
	}

	public void RefreshShovelEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()) && CurrentResource is ShovelConfig shovelConfig && shovelConfig == _editingShovel)
		{
			BindShovelPropertyControls(shovelConfig);
			PopulateShovelControls();
			UpdateShovelPreview();
		}
	}

	private void PopulateShovelControls()
	{
		if (!GodotObject.IsInstanceValid(_editingShovel))
		{
			return;
		}
		_updatingControls = true;
		try
		{
			_texturePicker?.SetEditedResource(_editingShovel.texture);
			RefreshShovelLists(_editingShovel);
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void OnTexturePicked(Resource resource)
	{
		if (!_updatingControls && _propertyBinding != null && GodotObject.IsInstanceValid(_editingShovel))
		{
			_propertyBinding.SetValue(_editingShovel, "texture", Variant.From(in resource), "更换铲子图像", this, "RefreshShovelEditorFromHistory");
			UpdateShovelPreview();
		}
	}

	private void AddShovelableName()
	{
		if (!GodotObject.IsInstanceValid(_editingShovel) || _propertyBinding == null)
		{
			return;
		}
		string text = _shovelableNameEdit?.Text.StripEdges() ?? "";
		if (!string.IsNullOrWhiteSpace(text))
		{
			Array<string> array = CloneStringArray(_editingShovel.shovelableNames);
			if (array.Contains(text))
			{
				_selectedShovelableIndex = array.IndexOf(text);
				RefreshShovelLists(_editingShovel);
				return;
			}
			array.Add(text);
			_selectedShovelableIndex = array.Count - 1;
			_propertyBinding.SetValue(_editingShovel, "shovelableNames", array, "添加可铲目标", this, "RefreshShovelEditorFromHistory");
			RefreshAfterListEdit();
		}
	}

	private void RemoveSelectedShovelableName()
	{
		if (GodotObject.IsInstanceValid(_editingShovel) && _propertyBinding != null && _editingShovel.shovelableNames != null && _selectedShovelableIndex >= 0 && _selectedShovelableIndex < _editingShovel.shovelableNames.Count)
		{
			Array<string> array = CloneStringArray(_editingShovel.shovelableNames);
			array.RemoveAt(_selectedShovelableIndex);
			_selectedShovelableIndex = Math.Min(_selectedShovelableIndex, array.Count - 1);
			_propertyBinding.SetValue(_editingShovel, "shovelableNames", array, "删除可铲目标", this, "RefreshShovelEditorFromHistory");
			RefreshAfterListEdit();
		}
	}

	private void SelectShovelableName(int index)
	{
		_selectedShovelableIndex = index;
		if (_editingShovel?.shovelableNames != null && index >= 0 && index < _editingShovel.shovelableNames.Count)
		{
			_shovelableNameEdit.Text = _editingShovel.shovelableNames[index];
		}
	}

	private void SelectUnlockCondition(int index)
	{
		_selectedUnlockIndex = index;
		if (_editingShovel?.unlockCheckList != null && index >= 0 && index < _editingShovel.unlockCheckList.Count)
		{
			_unlockPicker.SetEditedResource(_editingShovel.unlockCheckList[index]);
		}
	}

	private void AddUnlockCondition()
	{
		if (_unlockPicker?.GetEditedResource() is UnlockConditionBaseConfig item && GodotObject.IsInstanceValid(_editingShovel) && _propertyBinding != null)
		{
			Array<UnlockConditionBaseConfig> array = CloneUnlockArray(_editingShovel.unlockCheckList);
			array.Add(item);
			_selectedUnlockIndex = array.Count - 1;
			_propertyBinding.SetValue(_editingShovel, "unlockCheckList", array, "添加铲子解锁条件", this, "RefreshShovelEditorFromHistory");
			RefreshAfterListEdit();
		}
	}

	private void ReplaceSelectedUnlockCondition()
	{
		if (_unlockPicker?.GetEditedResource() is UnlockConditionBaseConfig value && GodotObject.IsInstanceValid(_editingShovel) && _propertyBinding != null && _editingShovel.unlockCheckList != null && _selectedUnlockIndex >= 0 && _selectedUnlockIndex < _editingShovel.unlockCheckList.Count)
		{
			Array<UnlockConditionBaseConfig> array = CloneUnlockArray(_editingShovel.unlockCheckList);
			array[_selectedUnlockIndex] = value;
			_propertyBinding.SetValue(_editingShovel, "unlockCheckList", array, "替换铲子解锁条件", this, "RefreshShovelEditorFromHistory");
			RefreshAfterListEdit();
		}
	}

	private void RemoveSelectedUnlockCondition()
	{
		if (GodotObject.IsInstanceValid(_editingShovel) && _propertyBinding != null && _editingShovel.unlockCheckList != null && _selectedUnlockIndex >= 0 && _selectedUnlockIndex < _editingShovel.unlockCheckList.Count)
		{
			Array<UnlockConditionBaseConfig> array = CloneUnlockArray(_editingShovel.unlockCheckList);
			array.RemoveAt(_selectedUnlockIndex);
			_selectedUnlockIndex = Math.Min(_selectedUnlockIndex, array.Count - 1);
			_propertyBinding.SetValue(_editingShovel, "unlockCheckList", array, "删除铲子解锁条件", this, "RefreshShovelEditorFromHistory");
			RefreshAfterListEdit();
		}
	}

	private void SelectShovelEvent(int index)
	{
		_selectedEventIndex = index;
		if (_editingShovel?.eventList != null && index >= 0 && index < _editingShovel.eventList.Count)
		{
			_eventPicker.SetEditedResource(_editingShovel.eventList[index]);
		}
	}

	private void AddShovelEvent()
	{
		if (_eventPicker?.GetEditedResource() is ShovelEventConfig item && GodotObject.IsInstanceValid(_editingShovel) && _propertyBinding != null)
		{
			Array<ShovelEventConfig> array = CloneEventArray(_editingShovel.eventList);
			array.Add(item);
			_selectedEventIndex = array.Count - 1;
			_propertyBinding.SetValue(_editingShovel, "eventList", array, "添加铲子事件", this, "RefreshShovelEditorFromHistory");
			RefreshAfterListEdit();
		}
	}

	private void ReplaceSelectedShovelEvent()
	{
		if (_eventPicker?.GetEditedResource() is ShovelEventConfig value && GodotObject.IsInstanceValid(_editingShovel) && _propertyBinding != null && _editingShovel.eventList != null && _selectedEventIndex >= 0 && _selectedEventIndex < _editingShovel.eventList.Count)
		{
			Array<ShovelEventConfig> array = CloneEventArray(_editingShovel.eventList);
			array[_selectedEventIndex] = value;
			_propertyBinding.SetValue(_editingShovel, "eventList", array, "替换铲子事件", this, "RefreshShovelEditorFromHistory");
			RefreshAfterListEdit();
		}
	}

	private void RemoveSelectedShovelEvent()
	{
		if (GodotObject.IsInstanceValid(_editingShovel) && _propertyBinding != null && _editingShovel.eventList != null && _selectedEventIndex >= 0 && _selectedEventIndex < _editingShovel.eventList.Count)
		{
			Array<ShovelEventConfig> array = CloneEventArray(_editingShovel.eventList);
			array.RemoveAt(_selectedEventIndex);
			_selectedEventIndex = Math.Min(_selectedEventIndex, array.Count - 1);
			_propertyBinding.SetValue(_editingShovel, "eventList", array, "删除铲子事件", this, "RefreshShovelEditorFromHistory");
			RefreshAfterListEdit();
		}
	}

	private void RefreshAfterListEdit()
	{
		RefreshShovelLists(_editingShovel);
		UpdateShovelPreview();
		RebuildShovelGameTarget();
	}

	private void OpenShovelNestedResource<[MustBeVariant] T>(Array<T> array, string propertyName, int index) where T : Resource
	{
		if (array != null && index >= 0 && index < array.Count)
		{
			Resource resource = array[index];
			if (resource != null && GodotObject.IsInstanceValid(resource))
			{
				XWResourceEditContext context = XWResourceEditContext.ForProperty(resource, CurrentResource, resource.ResourcePath, CurrentResourcePath, propertyName, index, "shovel_editor", CurrentEditContext?.IsBuiltInSource ?? XWResourceEditContext.IsBuiltInPath(CurrentResourcePath));
				XWEditorInterface.Instance?.EditResource(resource, context);
			}
		}
	}

	private void BindShovelTargetPreview(Control panel)
	{
		_targetTypePreviewOption = panel.GetNode<OptionButton>("Layout/Inputs/TargetType");
		_targetTypeVisualChoices = new XWVisualSegmentedOption(_targetTypePreviewOption, panel.GetNode<HFlowContainer>("%TargetTypeVisualChoices"));
		_targetTypeVisualChoices.Rebuild();
		_targetNamePreviewEdit = panel.GetNode<LineEdit>("Layout/Inputs/TargetName");
		_targetHypnosesPreviewCheck = panel.GetNode<CheckBox>("Layout/Inputs/Hypnoses");
		_targetHologramPreviewCheck = panel.GetNode<CheckBox>("Layout/Inputs/Hologram");
		_targetPreviewResultLabel = panel.GetNode<Label>("Layout/Result");
		_shovelTargetRoot = panel.GetNode<Node2D>("%TargetRoot");
		panel.GetNode<Button>("Layout/UseShovelButton").Pressed += UseShovelOnPreviewTarget;
		_targetTypePreviewOption.ItemSelected += (long _) =>
		{
			RefreshShovelTargetPreview();
			RebuildShovelGameTarget();
		};
		_targetNamePreviewEdit.TextChanged += (string _) =>
		{
			RefreshShovelTargetPreview();
			RebuildShovelGameTarget();
		};
		_targetHypnosesPreviewCheck.Toggled += (bool _) =>
		{
			RefreshShovelTargetPreview();
			RebuildShovelGameTarget();
		};
		_targetHologramPreviewCheck.Toggled += (bool _) =>
		{
			RefreshShovelTargetPreview();
			RebuildShovelGameTarget();
		};
		RefreshShovelTargetPreview();
		RebuildShovelGameTarget();
	}

	private void RefreshShovelTargetPreview()
	{
		if (GodotObject.IsInstanceValid(_targetPreviewResultLabel) && _editingShovel != null)
		{
			int num = (GodotObject.IsInstanceValid(_targetTypePreviewOption) ? _targetTypePreviewOption.Selected : 0);
			string text = (GodotObject.IsInstanceValid(_targetNamePreviewEdit) ? _targetNamePreviewEdit.Text.StripEdges() : "");
			bool flag = GodotObject.IsInstanceValid(_targetHypnosesPreviewCheck) && _targetHypnosesPreviewCheck.ButtonPressed;
			bool flag2 = GodotObject.IsInstanceValid(_targetHologramPreviewCheck) && _targetHologramPreviewCheck.ButtonPressed;
			bool flag3;
			string text2;
			switch (num)
			{
			case 1:
				flag3 = !flag && !flag2;
				text2 = (flag3 ? "道具护盾默认可铲" : "魅惑或全息护盾不可铲");
				break;
			case 2:
				flag3 = false;
				text2 = "普通道具在正式关卡中不可铲";
				break;
			case 3:
				flag3 = !string.IsNullOrWhiteSpace(text) && _editingShovel.shovelableNames != null && _editingShovel.shovelableNames.Contains(text);
				text2 = (flag3 ? "名称命中 shovelableNames" : "墓碑或弹坑名称未命中 shovelableNames");
				break;
			default:
				flag3 = !flag && !flag2;
				text2 = (flag3 ? "普通角色默认可铲" : "魅惑或全息角色不可铲");
				break;
			}
			_targetPreviewResultLabel.Text = (flag3 ? ("✓ 可铲 · " + text2) : ("✗ 不可铲 · " + text2));
			_targetPreviewResultLabel.Modulate = (flag3 ? new Color(0.48f, 0.92f, 0.55f) : new Color(1f, 0.52f, 0.44f));
			_shovelPreviewAllowed = flag3;
		}
	}

	private void RebuildShovelGameTarget()
	{
		if (!GodotObject.IsInstanceValid(_shovelTargetRoot))
		{
			return;
		}
		foreach (Node child in _shovelTargetRoot.GetChildren())
		{
			child.QueueFree();
		}
		PackedScene packedScene = ResourceLoader.Load<PackedScene>((!GodotObject.IsInstanceValid(_targetTypePreviewOption) || _targetTypePreviewOption.Selected == 0) ? "res://Asset/Anime/Character/Plant/Chapter0/Blover/Scene/TowerDefensePlantBlover.tscn" : "res://Asset/Anime/Character/Zombie/Puzzle/Target/Scene/TowerDefenseZombieTarget.tscn", null, ResourceLoader.CacheMode.Reuse);
		if (GodotObject.IsInstanceValid(packedScene))
		{
			Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			if (node is TowerDefenseCharacter towerDefenseCharacter)
			{
				towerDefenseCharacter.inGame = false;
				towerDefenseCharacter.editorPreviewMode = true;
				towerDefenseCharacter.Modulate = (_shovelPreviewAllowed ? Colors.White : new Color(1f, 0.55f, 0.55f));
			}
			_shovelTargetRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void UseShovelOnPreviewTarget()
	{
		if (GodotObject.IsInstanceValid(_shovelTargetRoot) && _shovelTargetRoot.GetChildCount() != 0 && _shovelTargetRoot.GetChild(0) is CanvasItem canvasItem)
		{
			Tween tween = canvasItem.CreateTween();
			if (_shovelPreviewAllowed)
			{
				tween.SetParallel();
				tween.TweenProperty(canvasItem, "position:y", -120f, 0.35);
				tween.TweenProperty(canvasItem, "rotation", -1.4f, 0.35);
				tween.TweenProperty(canvasItem, "modulate:a", 0f, 0.35);
				tween.SetParallel(parallel: false);
				tween.TweenCallback(Callable.From(RebuildShovelGameTarget));
			}
			else
			{
				tween.TweenProperty(canvasItem, "position:x", -12f, 0.06);
				tween.TweenProperty(canvasItem, "position:x", 12f, 0.06);
				tween.TweenProperty(canvasItem, "position:x", 0f, 0.06);
			}
		}
	}

	private void UpdateShovelPreview()
	{
		if (_editingShovel != null)
		{
			if (GodotObject.IsInstanceValid(_texturePreview))
			{
				_texturePreview.Texture = _editingShovel.texture;
			}
			if (GodotObject.IsInstanceValid(_titleLabel))
			{
				_titleLabel.Text = "铲子：" + EmptyToPlaceholder(_editingShovel.saveKey);
			}
			if (GodotObject.IsInstanceValid(_textSummaryLabel))
			{
				_textSummaryLabel.Text = "文本：" + EmptyToPlaceholder(_editingShovel.name) + " / " + EmptyToPlaceholder(_editingShovel.describe);
			}
			if (GodotObject.IsInstanceValid(_listSummaryLabel))
			{
				_listSummaryLabel.Text = "可铲 / 事件：" + BuildListSummary(_editingShovel);
			}
			if (GodotObject.IsInstanceValid(_textureResourceLabel))
			{
				_textureResourceLabel.Text = "texture：" + FormatResource(_editingShovel.texture);
			}
			RefreshShovelTargetPreview();
		}
	}

	private void RefreshShovelLists(ShovelConfig shovel)
	{
		PopulateStringList(_shovelableList, shovel.shovelableNames);
		PopulateResourceList(_unlockList, shovel.unlockCheckList);
		PopulateResourceList(_eventList, shovel.eventList);
		RestoreSelection(_shovelableList, _selectedShovelableIndex, shovel.shovelableNames?.Count ?? 0);
		RestoreSelection(_unlockList, _selectedUnlockIndex, shovel.unlockCheckList?.Count ?? 0);
		RestoreSelection(_eventList, _selectedEventIndex, shovel.eventList?.Count ?? 0);
	}

	private void AddSummaryRows(ShovelConfig shovel)
	{
		AddItemIfMissing(PreviewList, "铲子 saveKey → " + EmptyToPlaceholder(shovel.saveKey));
		AddItemIfMissing(PreviewList, "texture → " + FormatResource(shovel.texture));
		AddItemIfMissing(PreviewList, $"shovelableNames → {shovel.shovelableNames?.Count ?? 0}");
		AddItemIfMissing(TimelineList, $"unlockCheckList → {shovel.unlockCheckList?.Count ?? 0}");
		AddItemIfMissing(TimelineList, $"eventList → {shovel.eventList?.Count ?? 0}");
		AddItemIfMissing(GraphList, "铲子 → texture → shovelableNames → unlockCheckList → eventList");
		AddItemIfMissing(ReferenceList, "texture → " + FormatResource(shovel.texture));
	}

	private static Array<string> CloneStringArray(Array<string> source)
	{
		Array<string> array = new Array<string>();
		if (source != null)
		{
			foreach (string item in source)
			{
				array.Add(item);
			}
		}
		return array;
	}

	private static Array<UnlockConditionBaseConfig> CloneUnlockArray(Array<UnlockConditionBaseConfig> source)
	{
		Array<UnlockConditionBaseConfig> array = new Array<UnlockConditionBaseConfig>();
		if (source != null)
		{
			foreach (UnlockConditionBaseConfig item in source)
			{
				array.Add(item);
			}
		}
		return array;
	}

	private static Array<ShovelEventConfig> CloneEventArray(Array<ShovelEventConfig> source)
	{
		Array<ShovelEventConfig> array = new Array<ShovelEventConfig>();
		if (source != null)
		{
			foreach (ShovelEventConfig item in source)
			{
				array.Add(item);
			}
		}
		return array;
	}

	private static void RestoreSelection(ItemList list, int index, int count)
	{
		if (GodotObject.IsInstanceValid(list) && index >= 0 && index < count)
		{
			list.Select(index);
		}
	}

	private static void PopulateStringList(ItemList list, Array<string> array)
	{
		if (!GodotObject.IsInstanceValid(list))
		{
			return;
		}
		list.Clear();
		if (array == null || array.Count == 0)
		{
			list.AddItem("尚未配置");
			return;
		}
		for (int i = 0; i < array.Count; i++)
		{
			list.AddItem($"{i}: {array[i]}");
		}
	}

	private static void PopulateResourceList<[MustBeVariant] T>(ItemList list, Array<T> array)
	{
		if (!GodotObject.IsInstanceValid(list))
		{
			return;
		}
		list.Clear();
		if (array == null || array.Count == 0)
		{
			list.AddItem("尚未配置");
			return;
		}
		for (int i = 0; i < array.Count; i++)
		{
			list.AddItem($"{i}: {FormatObject(array[i])}");
		}
	}

	private static string BuildListSummary(ShovelConfig shovel)
	{
		return $"shovelableNames={shovel.shovelableNames?.Count ?? 0}, unlockCheckList={shovel.unlockCheckList?.Count ?? 0}, eventList={shovel.eventList?.Count ?? 0}";
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

	private static string FormatObject(object value)
	{
		if (value is Resource resource)
		{
			return FormatResource(resource);
		}
		if (!(value is GodotObject godotObject))
		{
			return value?.ToString() ?? "空";
		}
		return godotObject.GetType().Name;
	}

	private static string FormatResource(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "未配置";
		}
		if (!string.IsNullOrWhiteSpace(resource.ResourcePath))
		{
			return resource.ResourcePath.GetFile();
		}
		if (!string.IsNullOrWhiteSpace(resource.ResourceName))
		{
			return resource.ResourceName;
		}
		return resource.GetType().Name;
	}

	private static string EmptyToPlaceholder(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return "未配置";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(37)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeShovelBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindShovelWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateResourcePicker, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "baseType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindShovelPropertyControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shovel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnShovelVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshShovelEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateShovelControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTexturePicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddShovelableName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedShovelableName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectShovelableName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReplaceSelectedUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectShovelEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddShovelEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReplaceSelectedShovelEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedShovelEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshAfterListEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindShovelTargetPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "panel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshShovelTargetPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildShovelGameTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UseShovelOnPreviewTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateShovelPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshShovelLists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shovel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shovel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CloneStringArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneUnlockArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneEventArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateStringList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.Array, "array", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildListSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shovel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddItemIfMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatResource, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EmptyToPlaceholder, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.DisposeShovelBinding && args.Count == 0)
		{
			DisposeShovelBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.BindShovelWorkbench && args.Count == 1)
		{
			BindShovelWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateResourcePicker && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWResourcePicker>(CreateResourcePicker(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BindShovelPropertyControls && args.Count == 1)
		{
			BindShovelPropertyControls(VariantUtils.ConvertTo<ShovelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnShovelVisualPropertyEdited && args.Count == 1)
		{
			OnShovelVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshShovelEditorFromHistory && args.Count == 0)
		{
			RefreshShovelEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateShovelControls && args.Count == 0)
		{
			PopulateShovelControls();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTexturePicked && args.Count == 1)
		{
			OnTexturePicked(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddShovelableName && args.Count == 0)
		{
			AddShovelableName();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedShovelableName && args.Count == 0)
		{
			RemoveSelectedShovelableName();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectShovelableName && args.Count == 1)
		{
			SelectShovelableName(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectUnlockCondition && args.Count == 1)
		{
			SelectUnlockCondition(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddUnlockCondition && args.Count == 0)
		{
			AddUnlockCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceSelectedUnlockCondition && args.Count == 0)
		{
			ReplaceSelectedUnlockCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedUnlockCondition && args.Count == 0)
		{
			RemoveSelectedUnlockCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectShovelEvent && args.Count == 1)
		{
			SelectShovelEvent(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddShovelEvent && args.Count == 0)
		{
			AddShovelEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceSelectedShovelEvent && args.Count == 0)
		{
			ReplaceSelectedShovelEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedShovelEvent && args.Count == 0)
		{
			RemoveSelectedShovelEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAfterListEdit && args.Count == 0)
		{
			RefreshAfterListEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.BindShovelTargetPreview && args.Count == 1)
		{
			BindShovelTargetPreview(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshShovelTargetPreview && args.Count == 0)
		{
			RefreshShovelTargetPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildShovelGameTarget && args.Count == 0)
		{
			RebuildShovelGameTarget();
			ret = default;
			return true;
		}
		if (method == MethodName.UseShovelOnPreviewTarget && args.Count == 0)
		{
			UseShovelOnPreviewTarget();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateShovelPreview && args.Count == 0)
		{
			UpdateShovelPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshShovelLists && args.Count == 1)
		{
			RefreshShovelLists(VariantUtils.ConvertTo<ShovelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 1)
		{
			AddSummaryRows(VariantUtils.ConvertTo<ShovelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloneStringArray && args.Count == 1)
		{
			Array<string> array = CloneStringArray(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CloneUnlockArray && args.Count == 1)
		{
			Array<UnlockConditionBaseConfig> array2 = CloneUnlockArray(VariantUtils.ConvertToArray<UnlockConditionBaseConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.CloneEventArray && args.Count == 1)
		{
			Array<ShovelEventConfig> array3 = CloneEventArray(VariantUtils.ConvertToArray<ShovelEventConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.RestoreSelection && args.Count == 3)
		{
			RestoreSelection(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateStringList && args.Count == 2)
		{
			PopulateStringList(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertToArray<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildListSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildListSummary(VariantUtils.ConvertTo<ShovelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
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
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateResourcePicker && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWResourcePicker>(CreateResourcePicker(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CloneStringArray && args.Count == 1)
		{
			Array<string> array = CloneStringArray(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CloneUnlockArray && args.Count == 1)
		{
			Array<UnlockConditionBaseConfig> array2 = CloneUnlockArray(VariantUtils.ConvertToArray<UnlockConditionBaseConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.CloneEventArray && args.Count == 1)
		{
			Array<ShovelEventConfig> array3 = CloneEventArray(VariantUtils.ConvertToArray<ShovelEventConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.RestoreSelection && args.Count == 3)
		{
			RestoreSelection(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateStringList && args.Count == 2)
		{
			PopulateStringList(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertToArray<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildListSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildListSummary(VariantUtils.ConvertTo<ShovelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.DisposeShovelBinding)
		{
			return true;
		}
		if (method == MethodName.BindShovelWorkbench)
		{
			return true;
		}
		if (method == MethodName.CreateResourcePicker)
		{
			return true;
		}
		if (method == MethodName.BindShovelPropertyControls)
		{
			return true;
		}
		if (method == MethodName.OnShovelVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshShovelEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.PopulateShovelControls)
		{
			return true;
		}
		if (method == MethodName.OnTexturePicked)
		{
			return true;
		}
		if (method == MethodName.AddShovelableName)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedShovelableName)
		{
			return true;
		}
		if (method == MethodName.SelectShovelableName)
		{
			return true;
		}
		if (method == MethodName.SelectUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.AddUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.ReplaceSelectedUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.SelectShovelEvent)
		{
			return true;
		}
		if (method == MethodName.AddShovelEvent)
		{
			return true;
		}
		if (method == MethodName.ReplaceSelectedShovelEvent)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedShovelEvent)
		{
			return true;
		}
		if (method == MethodName.RefreshAfterListEdit)
		{
			return true;
		}
		if (method == MethodName.BindShovelTargetPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshShovelTargetPreview)
		{
			return true;
		}
		if (method == MethodName.RebuildShovelGameTarget)
		{
			return true;
		}
		if (method == MethodName.UseShovelOnPreviewTarget)
		{
			return true;
		}
		if (method == MethodName.UpdateShovelPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshShovelLists)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.CloneStringArray)
		{
			return true;
		}
		if (method == MethodName.CloneUnlockArray)
		{
			return true;
		}
		if (method == MethodName.CloneEventArray)
		{
			return true;
		}
		if (method == MethodName.RestoreSelection)
		{
			return true;
		}
		if (method == MethodName.PopulateStringList)
		{
			return true;
		}
		if (method == MethodName.BuildListSummary)
		{
			return true;
		}
		if (method == MethodName.AddItemIfMissing)
		{
			return true;
		}
		if (method == MethodName.FormatResource)
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
		if (name == PropertyName._editingShovel)
		{
			_editingShovel = VariantUtils.ConvertTo<ShovelConfig>(in value);
			return true;
		}
		if (name == PropertyName._texturePreview)
		{
			_texturePreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			_titleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._textSummaryLabel)
		{
			_textSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._listSummaryLabel)
		{
			_listSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._textureResourceLabel)
		{
			_textureResourceLabel = VariantUtils.ConvertTo<Label>(in value);
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
		if (name == PropertyName._saveKey)
		{
			_saveKey = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._name)
		{
			_name = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._describe)
		{
			_describe = VariantUtils.ConvertTo<TextEdit>(in value);
			return true;
		}
		if (name == PropertyName._handbookDescribe)
		{
			_handbookDescribe = VariantUtils.ConvertTo<TextEdit>(in value);
			return true;
		}
		if (name == PropertyName._handbookStory)
		{
			_handbookStory = VariantUtils.ConvertTo<TextEdit>(in value);
			return true;
		}
		if (name == PropertyName._texturePicker)
		{
			_texturePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._unlockPicker)
		{
			_unlockPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._eventPicker)
		{
			_eventPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._unlockList)
		{
			_unlockList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._eventList)
		{
			_eventList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._shovelableList)
		{
			_shovelableList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._shovelableNameEdit)
		{
			_shovelableNameEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._targetTypePreviewOption)
		{
			_targetTypePreviewOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._targetNamePreviewEdit)
		{
			_targetNamePreviewEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._targetHypnosesPreviewCheck)
		{
			_targetHypnosesPreviewCheck = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._targetHologramPreviewCheck)
		{
			_targetHologramPreviewCheck = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._targetPreviewResultLabel)
		{
			_targetPreviewResultLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._shovelTargetRoot)
		{
			_shovelTargetRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._shovelPreviewAllowed)
		{
			_shovelPreviewAllowed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._selectedShovelableIndex)
		{
			_selectedShovelableIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedUnlockIndex)
		{
			_selectedUnlockIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedEventIndex)
		{
			_selectedEventIndex = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._editingShovel)
		{
			value = VariantUtils.CreateFrom(in _editingShovel);
			return true;
		}
		if (name == PropertyName._texturePreview)
		{
			value = VariantUtils.CreateFrom(in _texturePreview);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			value = VariantUtils.CreateFrom(in _titleLabel);
			return true;
		}
		if (name == PropertyName._textSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _textSummaryLabel);
			return true;
		}
		if (name == PropertyName._listSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _listSummaryLabel);
			return true;
		}
		if (name == PropertyName._textureResourceLabel)
		{
			value = VariantUtils.CreateFrom(in _textureResourceLabel);
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
		if (name == PropertyName._saveKey)
		{
			value = VariantUtils.CreateFrom(in _saveKey);
			return true;
		}
		if (name == PropertyName._name)
		{
			value = VariantUtils.CreateFrom(in _name);
			return true;
		}
		if (name == PropertyName._describe)
		{
			value = VariantUtils.CreateFrom(in _describe);
			return true;
		}
		if (name == PropertyName._handbookDescribe)
		{
			value = VariantUtils.CreateFrom(in _handbookDescribe);
			return true;
		}
		if (name == PropertyName._handbookStory)
		{
			value = VariantUtils.CreateFrom(in _handbookStory);
			return true;
		}
		if (name == PropertyName._texturePicker)
		{
			value = VariantUtils.CreateFrom(in _texturePicker);
			return true;
		}
		if (name == PropertyName._unlockPicker)
		{
			value = VariantUtils.CreateFrom(in _unlockPicker);
			return true;
		}
		if (name == PropertyName._eventPicker)
		{
			value = VariantUtils.CreateFrom(in _eventPicker);
			return true;
		}
		if (name == PropertyName._unlockList)
		{
			value = VariantUtils.CreateFrom(in _unlockList);
			return true;
		}
		if (name == PropertyName._eventList)
		{
			value = VariantUtils.CreateFrom(in _eventList);
			return true;
		}
		if (name == PropertyName._shovelableList)
		{
			value = VariantUtils.CreateFrom(in _shovelableList);
			return true;
		}
		if (name == PropertyName._shovelableNameEdit)
		{
			value = VariantUtils.CreateFrom(in _shovelableNameEdit);
			return true;
		}
		if (name == PropertyName._targetTypePreviewOption)
		{
			value = VariantUtils.CreateFrom(in _targetTypePreviewOption);
			return true;
		}
		if (name == PropertyName._targetNamePreviewEdit)
		{
			value = VariantUtils.CreateFrom(in _targetNamePreviewEdit);
			return true;
		}
		if (name == PropertyName._targetHypnosesPreviewCheck)
		{
			value = VariantUtils.CreateFrom(in _targetHypnosesPreviewCheck);
			return true;
		}
		if (name == PropertyName._targetHologramPreviewCheck)
		{
			value = VariantUtils.CreateFrom(in _targetHologramPreviewCheck);
			return true;
		}
		if (name == PropertyName._targetPreviewResultLabel)
		{
			value = VariantUtils.CreateFrom(in _targetPreviewResultLabel);
			return true;
		}
		if (name == PropertyName._shovelTargetRoot)
		{
			value = VariantUtils.CreateFrom(in _shovelTargetRoot);
			return true;
		}
		if (name == PropertyName._shovelPreviewAllowed)
		{
			value = VariantUtils.CreateFrom(in _shovelPreviewAllowed);
			return true;
		}
		if (name == PropertyName._selectedShovelableIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedShovelableIndex);
			return true;
		}
		if (name == PropertyName._selectedUnlockIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedUnlockIndex);
			return true;
		}
		if (name == PropertyName._selectedEventIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedEventIndex);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._editingShovel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texturePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._titleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._textSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._listSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._textureResourceLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._describe, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._handbookDescribe, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._handbookStory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texturePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shovelableList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shovelableNameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetTypePreviewOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetNamePreviewEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetHypnosesPreviewCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetHologramPreviewCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetPreviewResultLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shovelTargetRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._shovelPreviewAllowed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedShovelableIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedUnlockIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedEventIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingShovel, Variant.From(in _editingShovel));
		info.AddProperty(PropertyName._texturePreview, Variant.From(in _texturePreview));
		info.AddProperty(PropertyName._titleLabel, Variant.From(in _titleLabel));
		info.AddProperty(PropertyName._textSummaryLabel, Variant.From(in _textSummaryLabel));
		info.AddProperty(PropertyName._listSummaryLabel, Variant.From(in _listSummaryLabel));
		info.AddProperty(PropertyName._textureResourceLabel, Variant.From(in _textureResourceLabel));
		info.AddProperty(PropertyName._resourceName, Variant.From(in _resourceName));
		info.AddProperty(PropertyName._localToScene, Variant.From(in _localToScene));
		info.AddProperty(PropertyName._saveKey, Variant.From(in _saveKey));
		info.AddProperty(PropertyName._name, Variant.From(in _name));
		info.AddProperty(PropertyName._describe, Variant.From(in _describe));
		info.AddProperty(PropertyName._handbookDescribe, Variant.From(in _handbookDescribe));
		info.AddProperty(PropertyName._handbookStory, Variant.From(in _handbookStory));
		info.AddProperty(PropertyName._texturePicker, Variant.From(in _texturePicker));
		info.AddProperty(PropertyName._unlockPicker, Variant.From(in _unlockPicker));
		info.AddProperty(PropertyName._eventPicker, Variant.From(in _eventPicker));
		info.AddProperty(PropertyName._unlockList, Variant.From(in _unlockList));
		info.AddProperty(PropertyName._eventList, Variant.From(in _eventList));
		info.AddProperty(PropertyName._shovelableList, Variant.From(in _shovelableList));
		info.AddProperty(PropertyName._shovelableNameEdit, Variant.From(in _shovelableNameEdit));
		info.AddProperty(PropertyName._targetTypePreviewOption, Variant.From(in _targetTypePreviewOption));
		info.AddProperty(PropertyName._targetNamePreviewEdit, Variant.From(in _targetNamePreviewEdit));
		info.AddProperty(PropertyName._targetHypnosesPreviewCheck, Variant.From(in _targetHypnosesPreviewCheck));
		info.AddProperty(PropertyName._targetHologramPreviewCheck, Variant.From(in _targetHologramPreviewCheck));
		info.AddProperty(PropertyName._targetPreviewResultLabel, Variant.From(in _targetPreviewResultLabel));
		info.AddProperty(PropertyName._shovelTargetRoot, Variant.From(in _shovelTargetRoot));
		info.AddProperty(PropertyName._shovelPreviewAllowed, Variant.From(in _shovelPreviewAllowed));
		info.AddProperty(PropertyName._selectedShovelableIndex, Variant.From(in _selectedShovelableIndex));
		info.AddProperty(PropertyName._selectedUnlockIndex, Variant.From(in _selectedUnlockIndex));
		info.AddProperty(PropertyName._selectedEventIndex, Variant.From(in _selectedEventIndex));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingShovel, out var value))
		{
			_editingShovel = value.As<ShovelConfig>();
		}
		if (info.TryGetProperty(PropertyName._texturePreview, out var value2))
		{
			_texturePreview = value2.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._titleLabel, out var value3))
		{
			_titleLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._textSummaryLabel, out var value4))
		{
			_textSummaryLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._listSummaryLabel, out var value5))
		{
			_listSummaryLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._textureResourceLabel, out var value6))
		{
			_textureResourceLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resourceName, out var value7))
		{
			_resourceName = value7.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToScene, out var value8))
		{
			_localToScene = value8.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._saveKey, out var value9))
		{
			_saveKey = value9.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._name, out var value10))
		{
			_name = value10.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._describe, out var value11))
		{
			_describe = value11.As<TextEdit>();
		}
		if (info.TryGetProperty(PropertyName._handbookDescribe, out var value12))
		{
			_handbookDescribe = value12.As<TextEdit>();
		}
		if (info.TryGetProperty(PropertyName._handbookStory, out var value13))
		{
			_handbookStory = value13.As<TextEdit>();
		}
		if (info.TryGetProperty(PropertyName._texturePicker, out var value14))
		{
			_texturePicker = value14.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._unlockPicker, out var value15))
		{
			_unlockPicker = value15.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._eventPicker, out var value16))
		{
			_eventPicker = value16.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._unlockList, out var value17))
		{
			_unlockList = value17.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._eventList, out var value18))
		{
			_eventList = value18.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._shovelableList, out var value19))
		{
			_shovelableList = value19.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._shovelableNameEdit, out var value20))
		{
			_shovelableNameEdit = value20.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._targetTypePreviewOption, out var value21))
		{
			_targetTypePreviewOption = value21.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._targetNamePreviewEdit, out var value22))
		{
			_targetNamePreviewEdit = value22.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._targetHypnosesPreviewCheck, out var value23))
		{
			_targetHypnosesPreviewCheck = value23.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._targetHologramPreviewCheck, out var value24))
		{
			_targetHologramPreviewCheck = value24.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._targetPreviewResultLabel, out var value25))
		{
			_targetPreviewResultLabel = value25.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._shovelTargetRoot, out var value26))
		{
			_shovelTargetRoot = value26.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._shovelPreviewAllowed, out var value27))
		{
			_shovelPreviewAllowed = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._selectedShovelableIndex, out var value28))
		{
			_selectedShovelableIndex = value28.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedUnlockIndex, out var value29))
		{
			_selectedUnlockIndex = value29.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedEventIndex, out var value30))
		{
			_selectedEventIndex = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value31))
		{
			_updatingControls = value31.As<bool>();
		}
	}
}
