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
using PVZHE.ModEditor.ModSystem;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMowerVisualResourceEditor.cs")]
public class XWMowerVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName DisposeMowerEditorBindings = "DisposeMowerEditorBindings";

		public static readonly StringName RenderMowerEditor = "RenderMowerEditor";

		public static readonly StringName BindMowerPreviewPanel = "BindMowerPreviewPanel";

		public static readonly StringName BindMowerScenePreview = "BindMowerScenePreview";

		public static readonly StringName RebuildMowerScenePreview = "RebuildMowerScenePreview";

		public static readonly StringName ApplyMowerScenePreviewTransform = "ApplyMowerScenePreviewTransform";

		public static readonly StringName SetMowerScenePreviewRunning = "SetMowerScenePreviewRunning";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public static readonly StringName ApplyMowerRuntimeProcessMode = "ApplyMowerRuntimeProcessMode";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName StartMowerGamePreview = "StartMowerGamePreview";

		public static readonly StringName RebuildMowerTargets = "RebuildMowerTargets";

		public static readonly StringName UpdateMowerTargetHits = "UpdateMowerTargetHits";

		public static readonly StringName BindMowerDirectEditControls = "BindMowerDirectEditControls";

		public static readonly StringName BindMowerCollectionEditors = "BindMowerCollectionEditors";

		public static readonly StringName SetMowerResource = "SetMowerResource";

		public static readonly StringName SelectMowerUnlockCondition = "SelectMowerUnlockCondition";

		public static readonly StringName SelectMowerEvent = "SelectMowerEvent";

		public static readonly StringName AddMowerUnlockCondition = "AddMowerUnlockCondition";

		public static readonly StringName ReplaceSelectedMowerUnlockCondition = "ReplaceSelectedMowerUnlockCondition";

		public static readonly StringName RemoveSelectedMowerUnlockCondition = "RemoveSelectedMowerUnlockCondition";

		public static readonly StringName OpenSelectedMowerUnlockCondition = "OpenSelectedMowerUnlockCondition";

		public static readonly StringName MoveSelectedMowerUnlockCondition = "MoveSelectedMowerUnlockCondition";

		public static readonly StringName AddMowerEvent = "AddMowerEvent";

		public static readonly StringName ReplaceSelectedMowerEvent = "ReplaceSelectedMowerEvent";

		public static readonly StringName RemoveSelectedMowerEvent = "RemoveSelectedMowerEvent";

		public static readonly StringName OpenSelectedMowerEvent = "OpenSelectedMowerEvent";

		public static readonly StringName MoveSelectedMowerEvent = "MoveSelectedMowerEvent";

		public static readonly StringName ReplaceMowerUnlockConditions = "ReplaceMowerUnlockConditions";

		public static readonly StringName ReplaceMowerEvents = "ReplaceMowerEvents";

		public static readonly StringName OnMowerPropertyEdited = "OnMowerPropertyEdited";

		public static readonly StringName ScheduleMowerSave = "ScheduleMowerSave";

		public static readonly StringName RefreshMowerEditorFromHistory = "RefreshMowerEditorFromHistory";

		public static readonly StringName RebuildMowerEditor = "RebuildMowerEditor";

		public static readonly StringName SavePendingMowerResource = "SavePendingMowerResource";

		public static readonly StringName SaveMowerResource = "SaveMowerResource";

		public static readonly StringName UpdateMowerPreview = "UpdateMowerPreview";

		public static readonly StringName RefreshMowerLists = "RefreshMowerLists";

		public static readonly StringName SyncMowerCollectionSelections = "SyncMowerCollectionSelections";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName BuildListSummary = "BuildListSummary";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";

		public static readonly StringName FormatResource = "FormatResource";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingMower = "_editingMower";

		public static readonly StringName _texturePreview = "_texturePreview";

		public static readonly StringName _mowerPreviewViewport = "_mowerPreviewViewport";

		public static readonly StringName _mowerPreviewRoot = "_mowerPreviewRoot";

		public static readonly StringName _mowerTargetRoot = "_mowerTargetRoot";

		public static readonly StringName _mowerRunProgress = "_mowerRunProgress";

		public static readonly StringName _mowerSceneStatusLabel = "_mowerSceneStatusLabel";

		public static readonly StringName _mowerSceneRunButton = "_mowerSceneRunButton";

		public static readonly StringName _mowerScenePreviewScale = "_mowerScenePreviewScale";

		public static readonly StringName _mowerScenePreviewRunning = "_mowerScenePreviewRunning";

		public static readonly StringName _titleLabel = "_titleLabel";

		public static readonly StringName _spriteLabel = "_spriteLabel";

		public static readonly StringName _textSummaryLabel = "_textSummaryLabel";

		public static readonly StringName _unlockSummaryLabel = "_unlockSummaryLabel";

		public static readonly StringName _unlockList = "_unlockList";

		public static readonly StringName _eventList = "_eventList";

		public static readonly StringName _unlockPicker = "_unlockPicker";

		public static readonly StringName _eventPicker = "_eventPicker";

		public static readonly StringName _selectedUnlockIndex = "_selectedUnlockIndex";

		public static readonly StringName _selectedEventIndex = "_selectedEventIndex";

		public static readonly StringName _mowerSaveTimer = "_mowerSaveTimer";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string VisualEditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMowerVisualEditorLayout.tscn";

	private static PackedScene _visualEditorLayoutScene;

	private MowerConfig _editingMower;

	private XWVisualPropertyBinding _mowerPropertyBinding;

	private TextureRect _texturePreview;

	private SubViewport _mowerPreviewViewport;

	private Node2D _mowerPreviewRoot;

	private Node2D _mowerTargetRoot;

	private readonly List<Node2D> _mowerTargets = new List<Node2D>();

	private double _mowerRunProgress;

	private Label _mowerSceneStatusLabel;

	private Button _mowerSceneRunButton;

	private float _mowerScenePreviewScale = 1f;

	private bool _mowerScenePreviewRunning;

	private Label _titleLabel;

	private Label _spriteLabel;

	private Label _textSummaryLabel;

	private Label _unlockSummaryLabel;

	private ItemList _unlockList;

	private ItemList _eventList;

	private XWResourcePicker _unlockPicker;

	private XWResourcePicker _eventPicker;

	private int _selectedUnlockIndex = -1;

	private int _selectedEventIndex = -1;

	private Timer _mowerSaveTimer;

	public override void _Ready()
	{
		base._Ready();
		_mowerSaveTimer = new Timer
		{
			WaitTime = 0.35,
			OneShot = true
		};
		_mowerSaveTimer.Timeout += SavePendingMowerResource;
		AddChild(_mowerSaveTimer, forceReadableName: false, InternalMode.Disabled);
		EnableVisibilityGatedProcessing();
	}

	public override void _ExitTree()
	{
		DisposeMowerEditorBindings();
		if (GodotObject.IsInstanceValid(_mowerSaveTimer))
		{
			_mowerSaveTimer.Stop();
		}
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		SetMowerScenePreviewRunning(running: false);
		DisposeMowerEditorBindings();
		if (CurrentResource is MowerConfig mowerConfig)
		{
			_editingMower = mowerConfig;
			RenderMowerEditor(mowerConfig);
		}
	}

	private void DisposeMowerEditorBindings()
	{
		_mowerPropertyBinding?.Dispose();
		_mowerPropertyBinding = null;
		_editingMower = null;
		_unlockPicker = null;
		_eventPicker = null;
		_mowerPreviewViewport = null;
		_mowerPreviewRoot = null;
		_mowerTargetRoot = null;
		_mowerTargets.Clear();
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (resource?.GetType() != typeof(MowerConfig))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private void RenderMowerEditor(MowerConfig mower)
	{
		if (CanvasGrid != null)
		{
			CanvasGrid.Columns = 1;
			if (_visualEditorLayoutScene == null)
			{
				_visualEditorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMowerVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _visualEditorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindMowerPreviewPanel(vBoxContainer, mower);
				BindMowerDirectEditControls(vBoxContainer, mower);
				BindMowerCollectionEditors(vBoxContainer, mower);
				AddSummaryRows(mower);
			}
		}
	}

	private void BindMowerPreviewPanel(VBoxContainer root, MowerConfig mower)
	{
		_texturePreview = root.GetNode<TextureRect>("%TexturePreview");
		_titleLabel = root.GetNode<Label>("%TitleLabel");
		_spriteLabel = root.GetNode<Label>("%SpriteLabel");
		_textSummaryLabel = root.GetNode<Label>("%TextSummaryLabel");
		_unlockSummaryLabel = root.GetNode<Label>("%UnlockSummaryLabel");
		BindMowerScenePreview(root.GetNode<Control>("%ScenePreview"), mower);
		UpdateMowerPreview();
	}

	private void BindMowerScenePreview(Control frame, MowerConfig mower)
	{
		_mowerSceneStatusLabel = frame.GetNode<Label>("Layout/Toolbar/Status");
		_mowerSceneRunButton = frame.GetNode<Button>("Layout/Toolbar/RunButton");
		_mowerPreviewViewport = frame.GetNode<SubViewport>("Layout/ViewportContainer/Viewport");
		_mowerPreviewRoot = frame.GetNode<Node2D>("Layout/ViewportContainer/Viewport/PreviewRoot");
		_mowerTargetRoot = frame.GetNode<Node2D>("%TargetRoot");
		frame.GetNode<Button>("Layout/Toolbar/ReloadButton").Pressed += RebuildMowerScenePreview;
		_mowerSceneRunButton.Pressed += StartMowerGamePreview;
		HSlider node = frame.GetNode<HSlider>("Layout/ZoomSlider");
		node.Value = _mowerScenePreviewScale;
		node.ValueChanged += (double value) =>
		{
			_mowerScenePreviewScale = (float)value;
			ApplyMowerScenePreviewTransform();
		};
		RebuildMowerTargets();
		RebuildMowerScenePreview();
	}

	private void RebuildMowerScenePreview()
	{
		if (!GodotObject.IsInstanceValid(_mowerPreviewRoot) || _editingMower == null)
		{
			return;
		}
		foreach (Node child in _mowerPreviewRoot.GetChildren())
		{
			child.QueueFree();
		}
		Node node = null;
		if (GodotObject.IsInstanceValid(_editingMower.sprite))
		{
			try
			{
				node = _editingMower.sprite.Instantiate(PackedScene.GenEditState.Disabled);
				node.ProcessMode = (ProcessModeEnum)(_mowerScenePreviewRunning ? 0 : 4);
			}
			catch (Exception ex)
			{
				GD.PushWarning("Mower editor scene preview failed: " + ex.Message);
				if (GodotObject.IsInstanceValid(_mowerSceneStatusLabel))
				{
					_mowerSceneStatusLabel.Text = "场景实例化失败: " + ex.Message;
				}
			}
		}
		if (node != null)
		{
			node.Name = "ConfiguredMowerScenePreview";
			_mowerPreviewRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			if (GodotObject.IsInstanceValid(_mowerSceneStatusLabel))
			{
				_mowerSceneStatusLabel.Text = "场景实例: " + FormatResource(_editingMower.sprite);
			}
		}
		else if (GodotObject.IsInstanceValid(_editingMower.texture))
		{
			_mowerPreviewRoot.AddChild(new Sprite2D
			{
				Name = "MowerTextureFallback",
				Texture = _editingMower.texture
			}, forceReadableName: false, InternalMode.Disabled);
			if (GodotObject.IsInstanceValid(_mowerSceneStatusLabel))
			{
				_mowerSceneStatusLabel.Text = "未配置 sprite，使用 texture 预览";
			}
		}
		else if (GodotObject.IsInstanceValid(_mowerSceneStatusLabel))
		{
			_mowerSceneStatusLabel.Text = "未配置可预览的 sprite 或 texture";
		}
		ApplyMowerScenePreviewTransform();
		SetMowerScenePreviewRunning(_mowerScenePreviewRunning);
	}

	private void ApplyMowerScenePreviewTransform()
	{
		if (GodotObject.IsInstanceValid(_mowerPreviewRoot))
		{
			_mowerPreviewRoot.Position = new Vector2(65f + 570f * (float)_mowerRunProgress, 205f);
			_mowerPreviewRoot.Scale = Vector2.One * _mowerScenePreviewScale;
		}
	}

	private void SetMowerScenePreviewRunning(bool running)
	{
		_mowerScenePreviewRunning = running;
		RequestVisibilityGatedProcessing(running);
		ApplyMowerRuntimeProcessMode(IsVisibleInTree());
		if (GodotObject.IsInstanceValid(_mowerSceneRunButton))
		{
			_mowerSceneRunButton.Text = (running ? "运行中…" : "重新启动");
		}
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		ApplyMowerRuntimeProcessMode(visible);
	}

	private void ApplyMowerRuntimeProcessMode(bool visible)
	{
		ProcessModeEnum processMode = (ProcessModeEnum)((visible && _mowerScenePreviewRunning) ? 0 : 4);
		if (GodotObject.IsInstanceValid(_mowerPreviewRoot))
		{
			_mowerPreviewRoot.ProcessMode = processMode;
		}
		if (GodotObject.IsInstanceValid(_mowerTargetRoot))
		{
			_mowerTargetRoot.ProcessMode = processMode;
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (_mowerScenePreviewRunning)
		{
			_mowerRunProgress = Mathf.Clamp(_mowerRunProgress + delta / 2.25, 0.0, 1.0);
			ApplyMowerScenePreviewTransform();
			UpdateMowerTargetHits();
			if (_mowerRunProgress >= 1.0)
			{
				SetMowerScenePreviewRunning(running: false);
			}
		}
	}

	private void StartMowerGamePreview()
	{
		_mowerRunProgress = 0.0;
		RebuildMowerTargets();
		ApplyMowerScenePreviewTransform();
		SetMowerScenePreviewRunning(running: true);
	}

	private void RebuildMowerTargets()
	{
		if (!GodotObject.IsInstanceValid(_mowerTargetRoot))
		{
			return;
		}
		foreach (Node child in _mowerTargetRoot.GetChildren())
		{
			child.QueueFree();
		}
		_mowerTargets.Clear();
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Puzzle/Target/Scene/TowerDefenseZombieTarget.tscn", null, ResourceLoader.CacheMode.Reuse);
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			return;
		}
		float[] array = new float[3] { 260f, 410f, 550f };
		foreach (float x in array)
		{
			if (packedScene.Instantiate(PackedScene.GenEditState.Disabled) is TowerDefenseCharacter towerDefenseCharacter)
			{
				towerDefenseCharacter.inGame = false;
				towerDefenseCharacter.editorPreviewMode = true;
				towerDefenseCharacter.Position = new Vector2(x, 205f);
				_mowerTargetRoot.AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
				_mowerTargets.Add(towerDefenseCharacter);
			}
		}
	}

	private void UpdateMowerTargetHits()
	{
		float num = 65f + 570f * (float)_mowerRunProgress;
		foreach (Node2D mowerTarget in _mowerTargets)
		{
			if (GodotObject.IsInstanceValid(mowerTarget) && !(num < mowerTarget.Position.X - 28f))
			{
				float num2 = Mathf.Clamp((num - mowerTarget.Position.X) / 100f, 0f, 1f);
				mowerTarget.Rotation = (0f - num2) * 1.2f;
				mowerTarget.Position = new Vector2(mowerTarget.Position.X + num2 * 3f, 205f - num2 * 145f);
				mowerTarget.Modulate = new Color(1f, 1f, 1f, 1f - num2);
			}
		}
	}

	private void BindMowerDirectEditControls(VBoxContainer root, MowerConfig mower)
	{
		_mowerPropertyBinding?.Dispose();
		_mowerPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnMowerPropertyEdited);
		_mowerPropertyBinding.BindText(root.GetNode<LineEdit>("%ResourceNameEdit"), mower, "resource_name", UpdateMowerPreview, this, "RefreshMowerEditorFromHistory");
		_mowerPropertyBinding.BindToggle(root.GetNode<CheckButton>("%LocalToSceneCheck"), mower, "resource_local_to_scene", UpdateMowerPreview, this, "RefreshMowerEditorFromHistory");
		_mowerPropertyBinding.BindText(root.GetNode<LineEdit>("%SaveKeyLineEdit"), mower, "saveKey", UpdateMowerPreview, this, "RefreshMowerEditorFromHistory");
		_mowerPropertyBinding.BindText(root.GetNode<LineEdit>("%NameLineEdit"), mower, "name", UpdateMowerPreview, this, "RefreshMowerEditorFromHistory");
		_mowerPropertyBinding.BindText(root.GetNode<TextEdit>("%DescribeTextEdit"), mower, "describe", UpdateMowerPreview, this, "RefreshMowerEditorFromHistory");
		_mowerPropertyBinding.BindText(root.GetNode<TextEdit>("%HandbookDescribeTextEdit"), mower, "handbookDescribe", UpdateMowerPreview, this, "RefreshMowerEditorFromHistory");
		_mowerPropertyBinding.BindText(root.GetNode<TextEdit>("%HandbookStoryTextEdit"), mower, "handbookStory", UpdateMowerPreview, this, "RefreshMowerEditorFromHistory");
		MountMowerResourcePicker(root.GetNode<HBoxContainer>("%TexturePickerHost"), "TexturePicker", "Texture2D", mower.texture, (Resource resource) =>
		{
			SetMowerResource("texture", resource, "更换小推车贴图");
		});
		MountMowerResourcePicker(root.GetNode<HBoxContainer>("%SpritePickerHost"), "SpritePicker", "PackedScene", mower.sprite, (Resource resource) =>
		{
			SetMowerResource("sprite", resource, "更换小推车场景");
		});
	}

	private void BindMowerCollectionEditors(VBoxContainer root, MowerConfig mower)
	{
		_unlockList = root.GetNode<ItemList>("%UnlockList");
		_unlockList.ItemSelected += (long index) =>
		{
			SelectMowerUnlockCondition((int)index);
		};
		_unlockList.ItemActivated += (long index) =>
		{
			OpenMowerNestedResource(mower.unlockCheckList, "unlockCheckList", (int)index);
		};
		_unlockPicker = MountMowerResourcePicker(root.GetNode<HBoxContainer>("%UnlockPickerHost"), "UnlockConditionPicker", "UnlockConditionBaseConfig", null, null);
		root.GetNode<Button>("%UnlockAddButton").Pressed += AddMowerUnlockCondition;
		root.GetNode<Button>("%UnlockReplaceButton").Pressed += ReplaceSelectedMowerUnlockCondition;
		root.GetNode<Button>("%UnlockRemoveButton").Pressed += RemoveSelectedMowerUnlockCondition;
		root.GetNode<Button>("%UnlockOpenButton").Pressed += OpenSelectedMowerUnlockCondition;
		root.GetNode<Button>("%UnlockMoveUpButton").Pressed += () =>
		{
			MoveSelectedMowerUnlockCondition(-1);
		};
		root.GetNode<Button>("%UnlockMoveDownButton").Pressed += () =>
		{
			MoveSelectedMowerUnlockCondition(1);
		};
		_eventList = root.GetNode<ItemList>("%EventList");
		_eventList.ItemSelected += (long index) =>
		{
			SelectMowerEvent((int)index);
		};
		_eventList.ItemActivated += (long index) =>
		{
			OpenMowerNestedResource(mower.eventList, "eventList", (int)index);
		};
		_eventPicker = MountMowerResourcePicker(root.GetNode<HBoxContainer>("%EventPickerHost"), "MowerEventPicker", "MowerEventConfig", null, null);
		root.GetNode<Button>("%EventAddButton").Pressed += AddMowerEvent;
		root.GetNode<Button>("%EventReplaceButton").Pressed += ReplaceSelectedMowerEvent;
		root.GetNode<Button>("%EventRemoveButton").Pressed += RemoveSelectedMowerEvent;
		root.GetNode<Button>("%EventOpenButton").Pressed += OpenSelectedMowerEvent;
		root.GetNode<Button>("%EventMoveUpButton").Pressed += () =>
		{
			MoveSelectedMowerEvent(-1);
		};
		root.GetNode<Button>("%EventMoveDownButton").Pressed += () =>
		{
			MoveSelectedMowerEvent(1);
		};
		RefreshMowerLists(mower);
	}

	private static XWResourcePicker MountMowerResourcePicker(HBoxContainer host, string nodeName, string baseType, Resource value, Action<Resource> changed)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return null;
		}
		XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
		xWResourcePicker.Name = nodeName;
		xWResourcePicker.Setup(baseType);
		xWResourcePicker.SetEditedResource(value);
		xWResourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		if (changed != null)
		{
			xWResourcePicker.ResourceChanged += (Resource resource) =>
			{
				changed(resource);
			};
		}
		host.AddChild(xWResourcePicker, forceReadableName: false, InternalMode.Disabled);
		return xWResourcePicker;
	}

	private void SetMowerResource(string propertyName, Resource resource, string actionName)
	{
		if (_mowerPropertyBinding != null && CurrentResource is MowerConfig mowerConfig && mowerConfig == _editingMower)
		{
			Variant value = ((propertyName == "texture") ? Variant.From<Texture2D>(resource as Texture2D) : Variant.From<PackedScene>(resource as PackedScene));
			_mowerPropertyBinding.SetValue(mowerConfig, propertyName, value, actionName, this, "RefreshMowerEditorFromHistory");
			mowerConfig.NotifyPropertyListChanged();
			UpdateMowerPreview();
			RebuildMowerScenePreview();
		}
	}

	private void OpenMowerNestedResource<[MustBeVariant] T>(Array<T> array, string propertyName, int index) where T : Resource
	{
		if (array != null && index >= 0 && index < array.Count)
		{
			Resource resource = array[index];
			if (resource != null && GodotObject.IsInstanceValid(resource))
			{
				XWResourceEditContext context = XWResourceEditContext.ForProperty(resource, CurrentResource, resource.ResourcePath, CurrentResourcePath, propertyName, index, "mower_editor", CurrentEditContext?.IsBuiltInSource ?? XWResourceEditContext.IsBuiltInPath(CurrentResourcePath));
				XWEditorInterface.Instance?.EditResource(resource, context);
			}
		}
	}

	private void SelectMowerUnlockCondition(int index)
	{
		if (_editingMower?.unlockCheckList != null && index >= 0 && index < _editingMower.unlockCheckList.Count)
		{
			_selectedUnlockIndex = index;
			_unlockPicker?.SetEditedResource(_editingMower.unlockCheckList[index]);
		}
	}

	private void SelectMowerEvent(int index)
	{
		if (_editingMower?.eventList != null && index >= 0 && index < _editingMower.eventList.Count)
		{
			_selectedEventIndex = index;
			_eventPicker?.SetEditedResource(_editingMower.eventList[index]);
		}
	}

	private void AddMowerUnlockCondition()
	{
		if (CurrentResource is MowerConfig mowerConfig && _unlockPicker?.EditedResource is UnlockConditionBaseConfig unlockConditionBaseConfig && GodotObject.IsInstanceValid(unlockConditionBaseConfig))
		{
			Array<UnlockConditionBaseConfig> array = ((mowerConfig.unlockCheckList == null) ? new Array<UnlockConditionBaseConfig>() : new Array<UnlockConditionBaseConfig>(mowerConfig.unlockCheckList));
			array.Add(unlockConditionBaseConfig);
			_selectedUnlockIndex = array.Count - 1;
			ReplaceMowerUnlockConditions(array, "添加小推车解锁条件");
		}
	}

	private void ReplaceSelectedMowerUnlockCondition()
	{
		if (CurrentResource is MowerConfig mowerConfig && _unlockPicker?.EditedResource is UnlockConditionBaseConfig unlockConditionBaseConfig && GodotObject.IsInstanceValid(unlockConditionBaseConfig) && mowerConfig.unlockCheckList != null && _selectedUnlockIndex >= 0 && _selectedUnlockIndex < mowerConfig.unlockCheckList.Count)
		{
			Array<UnlockConditionBaseConfig> array = new Array<UnlockConditionBaseConfig>(mowerConfig.unlockCheckList);
			array[_selectedUnlockIndex] = unlockConditionBaseConfig;
			ReplaceMowerUnlockConditions(array, "替换小推车解锁条件");
		}
	}

	private void RemoveSelectedMowerUnlockCondition()
	{
		if (_editingMower?.unlockCheckList != null && _selectedUnlockIndex >= 0 && _selectedUnlockIndex < _editingMower.unlockCheckList.Count)
		{
			Array<UnlockConditionBaseConfig> array = new Array<UnlockConditionBaseConfig>(_editingMower.unlockCheckList);
			array.RemoveAt(_selectedUnlockIndex);
			_selectedUnlockIndex = ((array.Count == 0) ? (-1) : Math.Min(_selectedUnlockIndex, array.Count - 1));
			ReplaceMowerUnlockConditions(array, "移除小推车解锁条件");
		}
	}

	private void OpenSelectedMowerUnlockCondition()
	{
		OpenMowerNestedResource(_editingMower?.unlockCheckList, "unlockCheckList", _selectedUnlockIndex);
	}

	private void MoveSelectedMowerUnlockCondition(int direction)
	{
		if (_editingMower?.unlockCheckList != null && _selectedUnlockIndex >= 0 && _selectedUnlockIndex < _editingMower.unlockCheckList.Count)
		{
			int num = _selectedUnlockIndex + Math.Sign(direction);
			if (num >= 0 && num < _editingMower.unlockCheckList.Count)
			{
				Array<UnlockConditionBaseConfig> array = new Array<UnlockConditionBaseConfig>(_editingMower.unlockCheckList);
				UnlockConditionBaseConfig value = array[_selectedUnlockIndex];
				array[_selectedUnlockIndex] = array[num];
				array[num] = value;
				_selectedUnlockIndex = num;
				ReplaceMowerUnlockConditions(array, (direction < 0) ? "上移小推车解锁条件" : "下移小推车解锁条件");
			}
		}
	}

	private void AddMowerEvent()
	{
		if (CurrentResource is MowerConfig mowerConfig && _eventPicker?.EditedResource is MowerEventConfig mowerEventConfig && GodotObject.IsInstanceValid(mowerEventConfig))
		{
			Array<MowerEventConfig> array = ((mowerConfig.eventList == null) ? new Array<MowerEventConfig>() : new Array<MowerEventConfig>(mowerConfig.eventList));
			array.Add(mowerEventConfig);
			_selectedEventIndex = array.Count - 1;
			ReplaceMowerEvents(array, "添加小推车事件");
		}
	}

	private void ReplaceSelectedMowerEvent()
	{
		if (CurrentResource is MowerConfig mowerConfig && _eventPicker?.EditedResource is MowerEventConfig mowerEventConfig && GodotObject.IsInstanceValid(mowerEventConfig) && mowerConfig.eventList != null && _selectedEventIndex >= 0 && _selectedEventIndex < mowerConfig.eventList.Count)
		{
			Array<MowerEventConfig> array = new Array<MowerEventConfig>(mowerConfig.eventList);
			array[_selectedEventIndex] = mowerEventConfig;
			ReplaceMowerEvents(array, "替换小推车事件");
		}
	}

	private void RemoveSelectedMowerEvent()
	{
		if (_editingMower?.eventList != null && _selectedEventIndex >= 0 && _selectedEventIndex < _editingMower.eventList.Count)
		{
			Array<MowerEventConfig> array = new Array<MowerEventConfig>(_editingMower.eventList);
			array.RemoveAt(_selectedEventIndex);
			_selectedEventIndex = ((array.Count == 0) ? (-1) : Math.Min(_selectedEventIndex, array.Count - 1));
			ReplaceMowerEvents(array, "移除小推车事件");
		}
	}

	private void OpenSelectedMowerEvent()
	{
		OpenMowerNestedResource(_editingMower?.eventList, "eventList", _selectedEventIndex);
	}

	private void MoveSelectedMowerEvent(int direction)
	{
		if (_editingMower?.eventList != null && _selectedEventIndex >= 0 && _selectedEventIndex < _editingMower.eventList.Count)
		{
			int num = _selectedEventIndex + Math.Sign(direction);
			if (num >= 0 && num < _editingMower.eventList.Count)
			{
				Array<MowerEventConfig> array = new Array<MowerEventConfig>(_editingMower.eventList);
				MowerEventConfig value = array[_selectedEventIndex];
				array[_selectedEventIndex] = array[num];
				array[num] = value;
				_selectedEventIndex = num;
				ReplaceMowerEvents(array, (direction < 0) ? "上移小推车事件" : "下移小推车事件");
			}
		}
	}

	private void ReplaceMowerUnlockConditions(Array<UnlockConditionBaseConfig> next, string actionName)
	{
		if (_mowerPropertyBinding != null && CurrentResource is MowerConfig mowerConfig)
		{
			_mowerPropertyBinding.SetValue(mowerConfig, "unlockCheckList", next, actionName, this, "RefreshMowerEditorFromHistory");
			mowerConfig.NotifyPropertyListChanged();
			RefreshMowerLists(mowerConfig);
			UpdateMowerPreview();
		}
	}

	private void ReplaceMowerEvents(Array<MowerEventConfig> next, string actionName)
	{
		if (_mowerPropertyBinding != null && CurrentResource is MowerConfig mowerConfig)
		{
			_mowerPropertyBinding.SetValue(mowerConfig, "eventList", next, actionName, this, "RefreshMowerEditorFromHistory");
			mowerConfig.NotifyPropertyListChanged();
			RefreshMowerLists(mowerConfig);
			UpdateMowerPreview();
		}
	}

	private void OnMowerPropertyEdited(bool committed)
	{
		if (CurrentResource is MowerConfig mowerConfig && mowerConfig == _editingMower)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
			}
			else
			{
				MarkCurrentResourceDirty();
				mowerConfig.EmitChanged();
			}
			ScheduleMowerSave(mowerConfig);
		}
	}

	private void ScheduleMowerSave(MowerConfig mower)
	{
		if (GodotObject.IsInstanceValid(mower))
		{
			_editingMower = mower;
			if (GodotObject.IsInstanceValid(_mowerSaveTimer))
			{
				_mowerSaveTimer.Start();
			}
		}
	}

	public void RefreshMowerEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()) && CurrentResource is MowerConfig mowerConfig)
		{
			_editingMower = mowerConfig;
			ScheduleMowerSave(mowerConfig);
			RebuildMowerEditor();
		}
	}

	private void RebuildMowerEditor()
	{
		if (CanvasGrid == null || !GodotObject.IsInstanceValid(_editingMower))
		{
			return;
		}
		SetMowerScenePreviewRunning(running: false);
		_mowerPropertyBinding?.Dispose();
		_mowerPropertyBinding = null;
		foreach (Node child in CanvasGrid.GetChildren())
		{
			CanvasGrid.RemoveChild(child);
			child.QueueFree();
		}
		RenderMowerEditor(_editingMower);
	}

	private void SavePendingMowerResource()
	{
		if (GodotObject.IsInstanceValid(_editingMower))
		{
			SaveMowerResource(_editingMower);
		}
	}

	private void SaveMowerResource(MowerConfig mower)
	{
		string currentResourcePath = CurrentResourcePath;
		if (string.IsNullOrWhiteSpace(currentResourcePath) || !GodotObject.IsInstanceValid(mower))
		{
			return;
		}
		Error error = ResourceSaver.Save(mower, currentResourcePath, ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			GD.PushWarning($"Mower editor save failed: {error} {currentResourcePath}");
		}
		else
		{
			string text = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
			if (!string.IsNullOrWhiteSpace(text))
			{
				XWModManifestSyncService.RegisterPath(text, currentResourcePath);
			}
		}
	}

	private void UpdateMowerPreview()
	{
		if (_editingMower != null)
		{
			if (GodotObject.IsInstanceValid(_texturePreview))
			{
				_texturePreview.Texture = _editingMower.texture;
			}
			if (GodotObject.IsInstanceValid(_titleLabel))
			{
				_titleLabel.Text = "小推车: " + EmptyToPlaceholder(_editingMower.saveKey);
			}
			if (GodotObject.IsInstanceValid(_spriteLabel))
			{
				_spriteLabel.Text = "sprite: " + FormatResource(_editingMower.sprite);
			}
			if (GodotObject.IsInstanceValid(_textSummaryLabel))
			{
				_textSummaryLabel.Text = "文本: " + EmptyToPlaceholder(_editingMower.name) + " / " + EmptyToPlaceholder(_editingMower.describe);
			}
			if (GodotObject.IsInstanceValid(_unlockSummaryLabel))
			{
				_unlockSummaryLabel.Text = "解锁/事件: " + BuildListSummary(_editingMower);
			}
		}
	}

	private void RefreshMowerLists(MowerConfig mower)
	{
		PopulateResourceList(_unlockList, mower.unlockCheckList);
		PopulateResourceList(_eventList, mower.eventList);
		SyncMowerCollectionSelections(mower);
	}

	private void SyncMowerCollectionSelections(MowerConfig mower)
	{
		int num = mower.unlockCheckList?.Count ?? 0;
		_selectedUnlockIndex = ((num == 0) ? (-1) : Math.Clamp((_selectedUnlockIndex >= 0) ? _selectedUnlockIndex : 0, 0, num - 1));
		if (_selectedUnlockIndex >= 0)
		{
			_unlockList?.Select(_selectedUnlockIndex);
			_unlockPicker?.SetEditedResource(mower.unlockCheckList[_selectedUnlockIndex]);
		}
		else
		{
			_unlockPicker?.SetEditedResource(null);
		}
		int num2 = mower.eventList?.Count ?? 0;
		_selectedEventIndex = ((num2 == 0) ? (-1) : Math.Clamp((_selectedEventIndex >= 0) ? _selectedEventIndex : 0, 0, num2 - 1));
		if (_selectedEventIndex >= 0)
		{
			_eventList?.Select(_selectedEventIndex);
			_eventPicker?.SetEditedResource(mower.eventList[_selectedEventIndex]);
		}
		else
		{
			_eventPicker?.SetEditedResource(null);
		}
	}

	private void AddSummaryRows(MowerConfig mower)
	{
		AddItemIfMissing(PreviewList, "小推车 saveKey -> " + EmptyToPlaceholder(mower.saveKey));
		AddItemIfMissing(PreviewList, "texture -> " + FormatResource(mower.texture));
		AddItemIfMissing(PreviewList, "sprite -> " + FormatResource(mower.sprite));
		AddItemIfMissing(TimelineList, $"unlockCheckList -> {mower.unlockCheckList?.Count ?? 0}");
		AddItemIfMissing(TimelineList, $"eventList -> {mower.eventList?.Count ?? 0}");
		AddItemIfMissing(GraphList, "小推车 -> texture / sprite -> unlockCheckList -> eventList");
		AddItemIfMissing(ReferenceList, "texture -> " + FormatResource(mower.texture));
		AddItemIfMissing(ReferenceList, "sprite -> " + FormatResource(mower.sprite));
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
			list.AddItem("未配置");
			return;
		}
		for (int i = 0; i < array.Count; i++)
		{
			list.AddItem($"{i}: {FormatObject(array[i])}");
		}
	}

	private static string BuildListSummary(MowerConfig mower)
	{
		return $"unlockCheckList={mower.unlockCheckList?.Count ?? 0}, eventList={mower.eventList?.Count ?? 0}";
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
		return new List<MethodInfo>(46)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeMowerEditorBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderMowerEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindMowerPreviewPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindMowerScenePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildMowerScenePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyMowerScenePreviewTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetMowerScenePreviewRunning, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "running", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMowerRuntimeProcessMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartMowerGamePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildMowerTargets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateMowerTargetHits, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindMowerDirectEditControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindMowerCollectionEditors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetMowerResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectMowerUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectMowerEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddMowerUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReplaceSelectedMowerUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedMowerUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenSelectedMowerUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedMowerUnlockCondition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddMowerEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReplaceSelectedMowerEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedMowerEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenSelectedMowerEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedMowerEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceMowerUnlockConditions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceMowerEvents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnMowerPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScheduleMowerSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshMowerEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildMowerEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SavePendingMowerResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveMowerResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateMowerPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshMowerLists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncMowerCollectionSelections, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildListSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.DisposeMowerEditorBindings && args.Count == 0)
		{
			DisposeMowerEditorBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderMowerEditor && args.Count == 1)
		{
			RenderMowerEditor(VariantUtils.ConvertTo<MowerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindMowerPreviewPanel && args.Count == 2)
		{
			BindMowerPreviewPanel(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<MowerConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindMowerScenePreview && args.Count == 2)
		{
			BindMowerScenePreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<MowerConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildMowerScenePreview && args.Count == 0)
		{
			RebuildMowerScenePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMowerScenePreviewTransform && args.Count == 0)
		{
			ApplyMowerScenePreviewTransform();
			ret = default;
			return true;
		}
		if (method == MethodName.SetMowerScenePreviewRunning && args.Count == 1)
		{
			SetMowerScenePreviewRunning(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged && args.Count == 1)
		{
			OnVisualEditorVisibilityChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMowerRuntimeProcessMode && args.Count == 1)
		{
			ApplyMowerRuntimeProcessMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartMowerGamePreview && args.Count == 0)
		{
			StartMowerGamePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildMowerTargets && args.Count == 0)
		{
			RebuildMowerTargets();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMowerTargetHits && args.Count == 0)
		{
			UpdateMowerTargetHits();
			ret = default;
			return true;
		}
		if (method == MethodName.BindMowerDirectEditControls && args.Count == 2)
		{
			BindMowerDirectEditControls(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<MowerConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindMowerCollectionEditors && args.Count == 2)
		{
			BindMowerCollectionEditors(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<MowerConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMowerResource && args.Count == 3)
		{
			SetMowerResource(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectMowerUnlockCondition && args.Count == 1)
		{
			SelectMowerUnlockCondition(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectMowerEvent && args.Count == 1)
		{
			SelectMowerEvent(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddMowerUnlockCondition && args.Count == 0)
		{
			AddMowerUnlockCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceSelectedMowerUnlockCondition && args.Count == 0)
		{
			ReplaceSelectedMowerUnlockCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedMowerUnlockCondition && args.Count == 0)
		{
			RemoveSelectedMowerUnlockCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenSelectedMowerUnlockCondition && args.Count == 0)
		{
			OpenSelectedMowerUnlockCondition();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedMowerUnlockCondition && args.Count == 1)
		{
			MoveSelectedMowerUnlockCondition(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddMowerEvent && args.Count == 0)
		{
			AddMowerEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceSelectedMowerEvent && args.Count == 0)
		{
			ReplaceSelectedMowerEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedMowerEvent && args.Count == 0)
		{
			RemoveSelectedMowerEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenSelectedMowerEvent && args.Count == 0)
		{
			OpenSelectedMowerEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedMowerEvent && args.Count == 1)
		{
			MoveSelectedMowerEvent(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceMowerUnlockConditions && args.Count == 2)
		{
			ReplaceMowerUnlockConditions(VariantUtils.ConvertToArray<UnlockConditionBaseConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceMowerEvents && args.Count == 2)
		{
			ReplaceMowerEvents(VariantUtils.ConvertToArray<MowerEventConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnMowerPropertyEdited && args.Count == 1)
		{
			OnMowerPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleMowerSave && args.Count == 1)
		{
			ScheduleMowerSave(VariantUtils.ConvertTo<MowerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMowerEditorFromHistory && args.Count == 0)
		{
			RefreshMowerEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildMowerEditor && args.Count == 0)
		{
			RebuildMowerEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.SavePendingMowerResource && args.Count == 0)
		{
			SavePendingMowerResource();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveMowerResource && args.Count == 1)
		{
			SaveMowerResource(VariantUtils.ConvertTo<MowerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMowerPreview && args.Count == 0)
		{
			UpdateMowerPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMowerLists && args.Count == 1)
		{
			RefreshMowerLists(VariantUtils.ConvertTo<MowerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncMowerCollectionSelections && args.Count == 1)
		{
			SyncMowerCollectionSelections(VariantUtils.ConvertTo<MowerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 1)
		{
			AddSummaryRows(VariantUtils.ConvertTo<MowerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildListSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildListSummary(VariantUtils.ConvertTo<MowerConfig>(in args[0])));
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
		if (method == MethodName.BuildListSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildListSummary(VariantUtils.ConvertTo<MowerConfig>(in args[0])));
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.DisposeMowerEditorBindings)
		{
			return true;
		}
		if (method == MethodName.RenderMowerEditor)
		{
			return true;
		}
		if (method == MethodName.BindMowerPreviewPanel)
		{
			return true;
		}
		if (method == MethodName.BindMowerScenePreview)
		{
			return true;
		}
		if (method == MethodName.RebuildMowerScenePreview)
		{
			return true;
		}
		if (method == MethodName.ApplyMowerScenePreviewTransform)
		{
			return true;
		}
		if (method == MethodName.SetMowerScenePreviewRunning)
		{
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.ApplyMowerRuntimeProcessMode)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.StartMowerGamePreview)
		{
			return true;
		}
		if (method == MethodName.RebuildMowerTargets)
		{
			return true;
		}
		if (method == MethodName.UpdateMowerTargetHits)
		{
			return true;
		}
		if (method == MethodName.BindMowerDirectEditControls)
		{
			return true;
		}
		if (method == MethodName.BindMowerCollectionEditors)
		{
			return true;
		}
		if (method == MethodName.SetMowerResource)
		{
			return true;
		}
		if (method == MethodName.SelectMowerUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.SelectMowerEvent)
		{
			return true;
		}
		if (method == MethodName.AddMowerUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.ReplaceSelectedMowerUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedMowerUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.OpenSelectedMowerUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedMowerUnlockCondition)
		{
			return true;
		}
		if (method == MethodName.AddMowerEvent)
		{
			return true;
		}
		if (method == MethodName.ReplaceSelectedMowerEvent)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedMowerEvent)
		{
			return true;
		}
		if (method == MethodName.OpenSelectedMowerEvent)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedMowerEvent)
		{
			return true;
		}
		if (method == MethodName.ReplaceMowerUnlockConditions)
		{
			return true;
		}
		if (method == MethodName.ReplaceMowerEvents)
		{
			return true;
		}
		if (method == MethodName.OnMowerPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.ScheduleMowerSave)
		{
			return true;
		}
		if (method == MethodName.RefreshMowerEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.RebuildMowerEditor)
		{
			return true;
		}
		if (method == MethodName.SavePendingMowerResource)
		{
			return true;
		}
		if (method == MethodName.SaveMowerResource)
		{
			return true;
		}
		if (method == MethodName.UpdateMowerPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshMowerLists)
		{
			return true;
		}
		if (method == MethodName.SyncMowerCollectionSelections)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
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
		if (name == PropertyName._editingMower)
		{
			_editingMower = VariantUtils.ConvertTo<MowerConfig>(in value);
			return true;
		}
		if (name == PropertyName._texturePreview)
		{
			_texturePreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._mowerPreviewViewport)
		{
			_mowerPreviewViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._mowerPreviewRoot)
		{
			_mowerPreviewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._mowerTargetRoot)
		{
			_mowerTargetRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._mowerRunProgress)
		{
			_mowerRunProgress = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._mowerSceneStatusLabel)
		{
			_mowerSceneStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._mowerSceneRunButton)
		{
			_mowerSceneRunButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._mowerScenePreviewScale)
		{
			_mowerScenePreviewScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._mowerScenePreviewRunning)
		{
			_mowerScenePreviewRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			_titleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._spriteLabel)
		{
			_spriteLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._textSummaryLabel)
		{
			_textSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._unlockSummaryLabel)
		{
			_unlockSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
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
		if (name == PropertyName._mowerSaveTimer)
		{
			_mowerSaveTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editingMower)
		{
			value = VariantUtils.CreateFrom(in _editingMower);
			return true;
		}
		if (name == PropertyName._texturePreview)
		{
			value = VariantUtils.CreateFrom(in _texturePreview);
			return true;
		}
		if (name == PropertyName._mowerPreviewViewport)
		{
			value = VariantUtils.CreateFrom(in _mowerPreviewViewport);
			return true;
		}
		if (name == PropertyName._mowerPreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _mowerPreviewRoot);
			return true;
		}
		if (name == PropertyName._mowerTargetRoot)
		{
			value = VariantUtils.CreateFrom(in _mowerTargetRoot);
			return true;
		}
		if (name == PropertyName._mowerRunProgress)
		{
			value = VariantUtils.CreateFrom(in _mowerRunProgress);
			return true;
		}
		if (name == PropertyName._mowerSceneStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _mowerSceneStatusLabel);
			return true;
		}
		if (name == PropertyName._mowerSceneRunButton)
		{
			value = VariantUtils.CreateFrom(in _mowerSceneRunButton);
			return true;
		}
		if (name == PropertyName._mowerScenePreviewScale)
		{
			value = VariantUtils.CreateFrom(in _mowerScenePreviewScale);
			return true;
		}
		if (name == PropertyName._mowerScenePreviewRunning)
		{
			value = VariantUtils.CreateFrom(in _mowerScenePreviewRunning);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			value = VariantUtils.CreateFrom(in _titleLabel);
			return true;
		}
		if (name == PropertyName._spriteLabel)
		{
			value = VariantUtils.CreateFrom(in _spriteLabel);
			return true;
		}
		if (name == PropertyName._textSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _textSummaryLabel);
			return true;
		}
		if (name == PropertyName._unlockSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _unlockSummaryLabel);
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
		if (name == PropertyName._mowerSaveTimer)
		{
			value = VariantUtils.CreateFrom(in _mowerSaveTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingMower, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texturePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mowerPreviewViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mowerPreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mowerTargetRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._mowerRunProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mowerSceneStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mowerSceneRunButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._mowerScenePreviewScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mowerScenePreviewRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._titleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spriteLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._textSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedUnlockIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedEventIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mowerSaveTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingMower, Variant.From(in _editingMower));
		info.AddProperty(PropertyName._texturePreview, Variant.From(in _texturePreview));
		info.AddProperty(PropertyName._mowerPreviewViewport, Variant.From(in _mowerPreviewViewport));
		info.AddProperty(PropertyName._mowerPreviewRoot, Variant.From(in _mowerPreviewRoot));
		info.AddProperty(PropertyName._mowerTargetRoot, Variant.From(in _mowerTargetRoot));
		info.AddProperty(PropertyName._mowerRunProgress, Variant.From(in _mowerRunProgress));
		info.AddProperty(PropertyName._mowerSceneStatusLabel, Variant.From(in _mowerSceneStatusLabel));
		info.AddProperty(PropertyName._mowerSceneRunButton, Variant.From(in _mowerSceneRunButton));
		info.AddProperty(PropertyName._mowerScenePreviewScale, Variant.From(in _mowerScenePreviewScale));
		info.AddProperty(PropertyName._mowerScenePreviewRunning, Variant.From(in _mowerScenePreviewRunning));
		info.AddProperty(PropertyName._titleLabel, Variant.From(in _titleLabel));
		info.AddProperty(PropertyName._spriteLabel, Variant.From(in _spriteLabel));
		info.AddProperty(PropertyName._textSummaryLabel, Variant.From(in _textSummaryLabel));
		info.AddProperty(PropertyName._unlockSummaryLabel, Variant.From(in _unlockSummaryLabel));
		info.AddProperty(PropertyName._unlockList, Variant.From(in _unlockList));
		info.AddProperty(PropertyName._eventList, Variant.From(in _eventList));
		info.AddProperty(PropertyName._unlockPicker, Variant.From(in _unlockPicker));
		info.AddProperty(PropertyName._eventPicker, Variant.From(in _eventPicker));
		info.AddProperty(PropertyName._selectedUnlockIndex, Variant.From(in _selectedUnlockIndex));
		info.AddProperty(PropertyName._selectedEventIndex, Variant.From(in _selectedEventIndex));
		info.AddProperty(PropertyName._mowerSaveTimer, Variant.From(in _mowerSaveTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingMower, out var value))
		{
			_editingMower = value.As<MowerConfig>();
		}
		if (info.TryGetProperty(PropertyName._texturePreview, out var value2))
		{
			_texturePreview = value2.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._mowerPreviewViewport, out var value3))
		{
			_mowerPreviewViewport = value3.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._mowerPreviewRoot, out var value4))
		{
			_mowerPreviewRoot = value4.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._mowerTargetRoot, out var value5))
		{
			_mowerTargetRoot = value5.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._mowerRunProgress, out var value6))
		{
			_mowerRunProgress = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._mowerSceneStatusLabel, out var value7))
		{
			_mowerSceneStatusLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._mowerSceneRunButton, out var value8))
		{
			_mowerSceneRunButton = value8.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._mowerScenePreviewScale, out var value9))
		{
			_mowerScenePreviewScale = value9.As<float>();
		}
		if (info.TryGetProperty(PropertyName._mowerScenePreviewRunning, out var value10))
		{
			_mowerScenePreviewRunning = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._titleLabel, out var value11))
		{
			_titleLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._spriteLabel, out var value12))
		{
			_spriteLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._textSummaryLabel, out var value13))
		{
			_textSummaryLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._unlockSummaryLabel, out var value14))
		{
			_unlockSummaryLabel = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._unlockList, out var value15))
		{
			_unlockList = value15.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._eventList, out var value16))
		{
			_eventList = value16.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._unlockPicker, out var value17))
		{
			_unlockPicker = value17.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._eventPicker, out var value18))
		{
			_eventPicker = value18.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._selectedUnlockIndex, out var value19))
		{
			_selectedUnlockIndex = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedEventIndex, out var value20))
		{
			_selectedEventIndex = value20.As<int>();
		}
		if (info.TryGetProperty(PropertyName._mowerSaveTimer, out var value21))
		{
			_mowerSaveTimer = value21.As<Timer>();
		}
	}
}
