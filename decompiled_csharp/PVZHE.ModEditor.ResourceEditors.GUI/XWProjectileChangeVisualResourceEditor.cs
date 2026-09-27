using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileChangeVisualResourceEditor.cs")]
public class XWProjectileChangeVisualResourceEditor : XWGenericVisualResourceEditor
{
	private enum ProjectileChangeEditMode
	{
		None,
		Chain,
		Single
	}

	private enum ProjectileSelectorTarget
	{
		SourceProjectile,
		TargetProjectile
	}

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName OnEmbeddedInspectorPropertyChanged = "OnEmbeddedInspectorPropertyChanged";

		public static readonly StringName RenderProjectileChangeEditor = "RenderProjectileChangeEditor";

		public static readonly StringName TryBindEditingResource = "TryBindEditingResource";

		public static readonly StringName GetEntryCount = "GetEntryCount";

		public static readonly StringName GetEditingTarget = "GetEditingTarget";

		public static readonly StringName BindRuntimeChangePreview = "BindRuntimeChangePreview";

		public static readonly StringName HandleRuntimePreviewInput = "HandleRuntimePreviewInput";

		public static readonly StringName SetRuntimePreviewZoom = "SetRuntimePreviewZoom";

		public static readonly StringName ResetRuntimePreviewView = "ResetRuntimePreviewView";

		public static readonly StringName ApplyRuntimePreviewView = "ApplyRuntimePreviewView";

		public static readonly StringName AdvanceRuntimeChangePreview = "AdvanceRuntimeChangePreview";

		public static readonly StringName RebuildRuntimeChangePreview = "RebuildRuntimeChangePreview";

		public static readonly StringName IsRuntimePreviewRequestCurrent = "IsRuntimePreviewRequestCurrent";

		public static readonly StringName AddRuntimeProjectileScene = "AddRuntimeProjectileScene";

		public static readonly StringName SetRuntimeChangePreviewRunning = "SetRuntimeChangePreviewRunning";

		public static readonly StringName OnProjectileChangeVisibilityChanged = "OnProjectileChangeVisibilityChanged";

		public static readonly StringName SetRuntimePreviewActive = "SetRuntimePreviewActive";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName UpdateRuntimeChangePlayback = "UpdateRuntimeChangePlayback";

		public static readonly StringName SetRuntimeProjectileVisual = "SetRuntimeProjectileVisual";

		public static readonly StringName BindEditorLayout = "BindEditorLayout";

		public static readonly StringName ApplyEditModeLayout = "ApplyEditModeLayout";

		public static readonly StringName RenderSelectedEntryEditor = "RenderSelectedEntryEditor";

		public static readonly StringName AddChangeEntry = "AddChangeEntry";

		public static readonly StringName RemoveSelectedChangeEntry = "RemoveSelectedChangeEntry";

		public static readonly StringName MoveSelectedChangeEntry = "MoveSelectedChangeEntry";

		public static readonly StringName ReplaceChangeList = "ReplaceChangeList";

		public static readonly StringName OnSelectedChangeEntryChanged = "OnSelectedChangeEntryChanged";

		public static readonly StringName RefreshSelectedEntryVisuals = "RefreshSelectedEntryVisuals";

		public static readonly StringName SetEntryProperty = "SetEntryProperty";

		public static readonly StringName ShowProjectileSelectorWindow = "ShowProjectileSelectorWindow";

		public static readonly StringName EnsureProjectileSelectorWindow = "EnsureProjectileSelectorWindow";

		public static readonly StringName ShowAudioSelectorWindow = "ShowAudioSelectorWindow";

		public static readonly StringName EnsureAudioSelectorWindow = "EnsureAudioSelectorWindow";

		public static readonly StringName ClearAudioSelection = "ClearAudioSelection";

		public static readonly StringName RefreshChangeChain = "RefreshChangeChain";

		public static readonly StringName DrawChangeChain = "DrawChangeChain";

		public static readonly StringName EnsureChangeList = "EnsureChangeList";

		public static readonly StringName GetSelectedEntry = "GetSelectedEntry";

		public static readonly StringName ClampSelectedIndex = "ClampSelectedIndex";

		public static readonly StringName UpdateToolbarState = "UpdateToolbarState";

		public static readonly StringName UpdateSelectedEntryLabel = "UpdateSelectedEntryLabel";

		public static readonly StringName QueueChangeCanvasRedraw = "QueueChangeCanvasRedraw";

		public static readonly StringName BuildSummary = "BuildSummary";

		public static readonly StringName DisposeProjectileChangeEditor = "DisposeProjectileChangeEditor";

		public static readonly StringName QueueNodeForDeletion = "QueueNodeForDeletion";

		public static readonly StringName BuildSelectedEntryPreview = "BuildSelectedEntryPreview";

		public static readonly StringName FormatEntryTitle = "FormatEntryTitle";

		public static readonly StringName FormatProjectileData = "FormatProjectileData";

		public static readonly StringName FormatProjectileConfig = "FormatProjectileConfig";

		public static readonly StringName ReadEntryProjectileName = "ReadEntryProjectileName";

		public static readonly StringName ReadProjectileConfigKey = "ReadProjectileConfigKey";

		public static readonly StringName FormatResource = "FormatResource";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName IsChainMode = "IsChainMode";

		public static readonly StringName _editMode = "_editMode";

		public static readonly StringName _editingChange = "_editingChange";

		public static readonly StringName _editingSingle = "_editingSingle";

		public static readonly StringName _selectedIndex = "_selectedIndex";

		public static readonly StringName _chainPanel = "_chainPanel";

		public static readonly StringName _chainList = "_chainList";

		public static readonly StringName _chainCanvas = "_chainCanvas";

		public static readonly StringName _itemEditorHost = "_itemEditorHost";

		public static readonly StringName _modeBadge = "_modeBadge";

		public static readonly StringName _summaryLabel = "_summaryLabel";

		public static readonly StringName _selectedLabel = "_selectedLabel";

		public static readonly StringName _emptyState = "_emptyState";

		public static readonly StringName _emptyStateLabel = "_emptyStateLabel";

		public static readonly StringName _sourceField = "_sourceField";

		public static readonly StringName _targetField = "_targetField";

		public static readonly StringName _audioField = "_audioField";

		public static readonly StringName _entryPreviewLabel = "_entryPreviewLabel";

		public static readonly StringName _runtimePreviewViewport = "_runtimePreviewViewport";

		public static readonly StringName _runtimePreviewRoot = "_runtimePreviewRoot";

		public static readonly StringName _runtimePreviewStatus = "_runtimePreviewStatus";

		public static readonly StringName _runtimePreviewRunButton = "_runtimePreviewRunButton";

		public static readonly StringName _runtimePreviewTimeline = "_runtimePreviewTimeline";

		public static readonly StringName _runtimePreviewInput = "_runtimePreviewInput";

		public static readonly StringName _runtimeZoomLabel = "_runtimeZoomLabel";

		public static readonly StringName _runtimeSourceProjectile = "_runtimeSourceProjectile";

		public static readonly StringName _runtimeTargetProjectile = "_runtimeTargetProjectile";

		public static readonly StringName _runtimePreviewProgress = "_runtimePreviewProgress";

		public static readonly StringName _runtimePreviewRunning = "_runtimePreviewRunning";

		public static readonly StringName _runtimePreviewActive = "_runtimePreviewActive";

		public static readonly StringName _runtimePreviewRequestVersion = "_runtimePreviewRequestVersion";

		public static readonly StringName _runtimePreviewBasePosition = "_runtimePreviewBasePosition";

		public static readonly StringName _runtimePreviewPan = "_runtimePreviewPan";

		public static readonly StringName _runtimePreviewZoom = "_runtimePreviewZoom";

		public static readonly StringName _runtimePreviewDragging = "_runtimePreviewDragging";

		public static readonly StringName _projectileDataPicker = "_projectileDataPicker";

		public static readonly StringName _projectileConfigPicker = "_projectileConfigPicker";

		public static readonly StringName _sourceProjectileKeyEdit = "_sourceProjectileKeyEdit";

		public static readonly StringName _changeAudioEdit = "_changeAudioEdit";

		public static readonly StringName _removeButton = "_removeButton";

		public static readonly StringName _moveUpButton = "_moveUpButton";

		public static readonly StringName _moveDownButton = "_moveDownButton";

		public static readonly StringName _runtimePreviousButton = "_runtimePreviousButton";

		public static readonly StringName _runtimeNextButton = "_runtimeNextButton";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _projectileSelectorTarget = "_projectileSelectorTarget";

		public static readonly StringName _projectileSelectorWindow = "_projectileSelectorWindow";

		public static readonly StringName _audioSelectorWindow = "_audioSelectorWindow";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string EditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileChangeEditorLayout.tscn";

	private static PackedScene _editorLayoutScene;

	private static readonly HashSet<string> InspectorEditableProperties = new HashSet<string> { "resource_name", "resource_local_to_scene", "changeList" };

	private static readonly HashSet<string> SingleInspectorEditableProperties = new HashSet<string> { "resource_name", "resource_local_to_scene", "projectileData", "projectileConfig", "changeAudio" };

	private ProjectileChangeEditMode _editMode;

	private ChangeProjectileConfig _editingChange;

	private ChangeProjectileSingleConfig _editingSingle;

	private XWVisualPropertyBinding _propertyBinding;

	private int _selectedIndex = -1;

	private Control _chainPanel;

	private ItemList _chainList;

	private Control _chainCanvas;

	private VBoxContainer _itemEditorHost;

	private Label _modeBadge;

	private Label _summaryLabel;

	private Label _selectedLabel;

	private Control _emptyState;

	private Label _emptyStateLabel;

	private Control _sourceField;

	private Control _targetField;

	private Control _audioField;

	private Label _entryPreviewLabel;

	private SubViewport _runtimePreviewViewport;

	private Node2D _runtimePreviewRoot;

	private Label _runtimePreviewStatus;

	private Button _runtimePreviewRunButton;

	private HSlider _runtimePreviewTimeline;

	private Control _runtimePreviewInput;

	private Label _runtimeZoomLabel;

	private Node _runtimeSourceProjectile;

	private Node _runtimeTargetProjectile;

	private double _runtimePreviewProgress;

	private bool _runtimePreviewRunning;

	private bool _runtimePreviewActive;

	private int _runtimePreviewRequestVersion;

	private Vector2 _runtimePreviewBasePosition;

	private Vector2 _runtimePreviewPan;

	private float _runtimePreviewZoom = 1f;

	private bool _runtimePreviewDragging;

	private XWResourcePicker _projectileDataPicker;

	private XWResourcePicker _projectileConfigPicker;

	private LineEdit _sourceProjectileKeyEdit;

	private LineEdit _changeAudioEdit;

	private Button _removeButton;

	private Button _moveUpButton;

	private Button _moveDownButton;

	private Button _runtimePreviousButton;

	private Button _runtimeNextButton;

	private bool _updatingControls;

	private ProjectileSelectorTarget _projectileSelectorTarget;

	private XWGameplayResourcePickerWindow _projectileSelectorWindow;

	private XWGameplayResourcePickerWindow _audioSelectorWindow;

	private bool IsChainMode => _editMode == ProjectileChangeEditMode.Chain;

	public override void _Ready()
	{
		SetProcess(enable: false);
		VisibilityChanged += OnProjectileChangeVisibilityChanged;
		base._Ready();
		OnProjectileChangeVisibilityChanged();
	}

	public override void _ExitTree()
	{
		VisibilityChanged -= OnProjectileChangeVisibilityChanged;
		DisposeProjectileChangeEditor();
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeProjectileChangeEditor();
		if (CanvasGrid != null && TryBindEditingResource(CurrentResource))
		{
			_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), (bool _) =>
			{
				NotifyCurrentResourceEdited();
				RefreshSelectedEntryVisuals();
			});
			RenderProjectileChangeEditor();
		}
	}

	protected override HashSet<string> GetEmbeddedInspectorAllowedProperties(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!(resource is ChangeProjectileConfig))
		{
			if (resource is ChangeProjectileSingleConfig)
			{
				return SingleInspectorEditableProperties;
			}
			return base.GetEmbeddedInspectorAllowedProperties(resource, path, descriptor);
		}
		return InspectorEditableProperties;
	}

	protected override void OnEmbeddedInspectorPropertyChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		if (obj is Resource resource && TryBindEditingResource(resource))
		{
			ClampSelectedIndex();
			RefreshChangeChain();
			RenderSelectedEntryEditor();
			QueueChangeCanvasRedraw();
		}
	}

	private void RenderProjectileChangeEditor()
	{
		if (CanvasGrid != null)
		{
			ClampSelectedIndex();
			CanvasGrid.Columns = 1;
			if (_editorLayoutScene == null)
			{
				_editorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileChangeEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _editorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindEditorLayout(vBoxContainer);
				ApplyEditModeLayout();
				RefreshChangeChain();
				RenderSelectedEntryEditor();
			}
		}
	}

	private bool TryBindEditingResource(Resource resource)
	{
		if (!(resource is ChangeProjectileConfig changeProjectileConfig))
		{
			if (resource is ChangeProjectileSingleConfig editingSingle)
			{
				_editMode = ProjectileChangeEditMode.Single;
				_editingChange = null;
				_editingSingle = editingSingle;
				_selectedIndex = 0;
				return true;
			}
			_editMode = ProjectileChangeEditMode.None;
			_editingChange = null;
			_editingSingle = null;
			_selectedIndex = -1;
			return false;
		}
		_editMode = ProjectileChangeEditMode.Chain;
		_editingChange = changeProjectileConfig;
		_editingSingle = null;
		EnsureChangeList(changeProjectileConfig);
		_selectedIndex = ((changeProjectileConfig.changeList.Count > 0) ? Mathf.Clamp(_selectedIndex, 0, changeProjectileConfig.changeList.Count - 1) : (-1));
		return true;
	}

	private int GetEntryCount()
	{
		if (!IsChainMode)
		{
			return GodotObject.IsInstanceValid(_editingSingle) ? 1 : 0;
		}
		return (_editingChange?.changeList?.Count).GetValueOrDefault();
	}

	private Resource GetEditingTarget()
	{
		if (_editMode == ProjectileChangeEditMode.Single)
		{
			return _editingSingle;
		}
		return GetSelectedEntry();
	}

	private void BindRuntimeChangePreview(Control panel)
	{
		_runtimePreviewViewport = panel.GetNode<SubViewport>("Layout/PreviewStack/ViewportContainer/Viewport");
		_runtimePreviewRoot = panel.GetNode<Node2D>("Layout/PreviewStack/ViewportContainer/Viewport/PreviewRoot");
		_runtimePreviewStatus = panel.GetNode<Label>("Layout/Status");
		_runtimePreviewRunButton = panel.GetNode<Button>("Layout/Toolbar/RunButton");
		_runtimePreviewTimeline = panel.GetNode<HSlider>("Layout/Toolbar/TimelineSlider");
		_runtimePreviewInput = panel.GetNode<Control>("Layout/PreviewStack/PreviewInput");
		_runtimeZoomLabel = panel.GetNode<Label>("Layout/Toolbar/ZoomLabel");
		_runtimePreviousButton = panel.GetNode<Button>("Layout/Toolbar/PreviousButton");
		_runtimeNextButton = panel.GetNode<Button>("Layout/Toolbar/NextButton");
		_runtimePreviousButton.Pressed += () =>
		{
			AdvanceRuntimeChangePreview(-1);
		};
		_runtimeNextButton.Pressed += () =>
		{
			AdvanceRuntimeChangePreview(1);
		};
		_runtimePreviewRunButton.Pressed += () =>
		{
			SetRuntimeChangePreviewRunning(!_runtimePreviewRunning);
		};
		_runtimePreviewTimeline.ValueChanged += (double value) =>
		{
			_runtimePreviewProgress = value;
			_runtimePreviewRunning = false;
			UpdateRuntimeChangePlayback();
		};
		_runtimePreviewInput.GuiInput += HandleRuntimePreviewInput;
		panel.GetNode<Button>("Layout/Toolbar/ResetViewButton").Pressed += ResetRuntimePreviewView;
		_runtimePreviewBasePosition = _runtimePreviewRoot.Position;
		ResetRuntimePreviewView();
		RebuildRuntimeChangePreview();
	}

	private void HandleRuntimePreviewInput(InputEvent inputEvent)
	{
		bool flag = false;
		if (inputEvent is InputEventMouseButton inputEventMouseButton)
		{
			if (inputEventMouseButton.ButtonIndex == MouseButton.Middle || inputEventMouseButton.ButtonIndex == MouseButton.Left)
			{
				_runtimePreviewDragging = inputEventMouseButton.Pressed;
				flag = true;
			}
			else if (inputEventMouseButton.Pressed && inputEventMouseButton.ButtonIndex == MouseButton.WheelUp)
			{
				SetRuntimePreviewZoom(_runtimePreviewZoom * 1.12f, inputEventMouseButton.Position);
				flag = true;
			}
			else if (inputEventMouseButton.Pressed && inputEventMouseButton.ButtonIndex == MouseButton.WheelDown)
			{
				SetRuntimePreviewZoom(_runtimePreviewZoom / 1.12f, inputEventMouseButton.Position);
				flag = true;
			}
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _runtimePreviewDragging)
		{
			_runtimePreviewPan += inputEventMouseMotion.Relative;
			ApplyRuntimePreviewView();
			flag = true;
		}
		if (flag && GodotObject.IsInstanceValid(_runtimePreviewInput))
		{
			_runtimePreviewInput.AcceptEvent();
		}
	}

	private void SetRuntimePreviewZoom(float zoom, Vector2 pointer)
	{
		float runtimePreviewZoom = _runtimePreviewZoom;
		Vector2 vector = _runtimePreviewBasePosition + _runtimePreviewPan;
		Vector2 vector2 = ((runtimePreviewZoom > 0f) ? ((pointer - vector) / runtimePreviewZoom) : Vector2.Zero);
		_runtimePreviewZoom = zoom;
		_runtimePreviewZoom = Mathf.Clamp(_runtimePreviewZoom, 0.35f, 3f);
		if (!Mathf.IsEqualApprox(runtimePreviewZoom, _runtimePreviewZoom))
		{
			_runtimePreviewPan = pointer - vector2 * _runtimePreviewZoom - _runtimePreviewBasePosition;
		}
		ApplyRuntimePreviewView();
	}

	private void ResetRuntimePreviewView()
	{
		_runtimePreviewPan = Vector2.Zero;
		_runtimePreviewZoom = 1f;
		_runtimePreviewDragging = false;
		ApplyRuntimePreviewView();
	}

	private void ApplyRuntimePreviewView()
	{
		if (GodotObject.IsInstanceValid(_runtimePreviewRoot))
		{
			_runtimePreviewRoot.Position = _runtimePreviewBasePosition + _runtimePreviewPan;
			_runtimePreviewRoot.Scale = Vector2.One * _runtimePreviewZoom;
		}
		if (GodotObject.IsInstanceValid(_runtimeZoomLabel))
		{
			_runtimeZoomLabel.Text = $"{Mathf.RoundToInt(_runtimePreviewZoom * 100f)}%";
		}
	}

	private void AdvanceRuntimeChangePreview(int delta)
	{
		if (IsChainMode)
		{
			int entryCount = GetEntryCount();
			if (entryCount > 0)
			{
				_selectedIndex = Mathf.Clamp(_selectedIndex + delta, 0, entryCount - 1);
				RefreshChangeChain();
				RenderSelectedEntryEditor();
				QueueChangeCanvasRedraw();
			}
		}
	}

	private void RebuildRuntimeChangePreview()
	{
		int requestVersion = ++_runtimePreviewRequestVersion;
		Resource currentResource = CurrentResource;
		Node2D runtimePreviewRoot = _runtimePreviewRoot;
		if (!GodotObject.IsInstanceValid(runtimePreviewRoot))
		{
			return;
		}
		foreach (Node child in runtimePreviewRoot.GetChildren())
		{
			child.QueueFree();
		}
		_runtimeSourceProjectile = null;
		_runtimeTargetProjectile = null;
		ChangeProjectileSingleConfig selectedEntry = GetSelectedEntry();
		if (!GodotObject.IsInstanceValid(selectedEntry))
		{
			if (GodotObject.IsInstanceValid(_runtimePreviewStatus))
			{
				_runtimePreviewStatus.Text = "未选择子弹变化项";
			}
			return;
		}
		TowerDefenseProjectileConfig towerDefenseProjectileConfig = selectedEntry.projectileData?.BuildConfig();
		string text = ReadEntryProjectileName(selectedEntry);
		if (!GodotObject.IsInstanceValid(towerDefenseProjectileConfig) && ResourceManager.Instance != null && !string.IsNullOrWhiteSpace(text))
		{
			towerDefenseProjectileConfig = TowerDefenseManager.GetProjectileConfig(text);
		}
		if (!IsRuntimePreviewRequestCurrent(requestVersion, currentResource, runtimePreviewRoot))
		{
			return;
		}
		_runtimeSourceProjectile = AddRuntimeProjectileScene(towerDefenseProjectileConfig?.projectileScene, Vector2.Zero, "SourceProjectilePreview", new Color(0.35f, 0.68f, 1f), runtimePreviewRoot);
		if (!IsRuntimePreviewRequestCurrent(requestVersion, currentResource, runtimePreviewRoot))
		{
			QueueNodeForDeletion(_runtimeSourceProjectile);
			_runtimeSourceProjectile = null;
			return;
		}
		_runtimeTargetProjectile = AddRuntimeProjectileScene(selectedEntry.projectileConfig?.projectileScene, Vector2.Zero, "TargetProjectilePreview", new Color(1f, 0.6f, 0.3f), runtimePreviewRoot);
		_runtimePreviewProgress = 0.0;
		_runtimePreviewTimeline?.SetValueNoSignal(0.0);
		SetRuntimeChangePreviewRunning(_runtimePreviewRunning);
		if (GodotObject.IsInstanceValid(_runtimePreviewStatus))
		{
			_runtimePreviewStatus.Text = (IsChainMode ? $"变化项 {_selectedIndex + 1}/{GetEntryCount()} · {FormatProjectileData(selectedEntry)} → {FormatProjectileConfig(selectedEntry)} · 音效 {EmptyToPlaceholder(selectedEntry.changeAudio)}" : $"单次变化 · {FormatProjectileData(selectedEntry)} → {FormatProjectileConfig(selectedEntry)} · 音效 {EmptyToPlaceholder(selectedEntry.changeAudio)}");
		}
		UpdateRuntimeChangePlayback();
	}

	private bool IsRuntimePreviewRequestCurrent(int requestVersion, Resource requestedResource, Node2D requestedRoot)
	{
		if (requestVersion != _runtimePreviewRequestVersion)
		{
			return false;
		}
		if (requestedResource == CurrentResource && requestedRoot == _runtimePreviewRoot && GodotObject.IsInstanceValid(requestedRoot))
		{
			return !requestedRoot.IsQueuedForDeletion();
		}
		return false;
	}

	private Node AddRuntimeProjectileScene(PackedScene scene, Vector2 position, string nodeName, Color fallbackColor, Node2D previewRoot)
	{
		if (!GodotObject.IsInstanceValid(previewRoot) || previewRoot.IsQueuedForDeletion())
		{
			return null;
		}
		Node node = null;
		if (GodotObject.IsInstanceValid(scene))
		{
			try
			{
				node = scene.Instantiate(PackedScene.GenEditState.Disabled);
				node.Name = nodeName;
				node.ProcessMode = (ProcessModeEnum)(_runtimePreviewActive ? 0 : 4);
				if (node is Node2D node2D)
				{
					node2D.Position = position;
				}
				else if (node is Control control)
				{
					control.Position = position;
				}
				previewRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			}
			catch (Exception ex)
			{
				GD.PushWarning("Projectile change runtime preview failed: " + nodeName + " " + ex.Message);
			}
		}
		if (node == null)
		{
			ColorRect colorRect = new ColorRect
			{
				Name = nodeName + "Fallback",
				Color = fallbackColor,
				Position = position - new Vector2(22f, 22f),
				Size = new Vector2(44f, 44f),
				MouseFilter = MouseFilterEnum.Ignore
			};
			previewRoot.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
			node = colorRect;
		}
		return node;
	}

	private void SetRuntimeChangePreviewRunning(bool running)
	{
		_runtimePreviewRunning = running;
		if (running && _runtimePreviewProgress >= 1.0)
		{
			_runtimePreviewProgress = 0.0;
		}
		if (GodotObject.IsInstanceValid(_runtimePreviewRunButton))
		{
			_runtimePreviewRunButton.Text = (running ? "暂停变化" : "播放变化");
		}
		SetRuntimePreviewActive(IsVisibleInTree());
		UpdateRuntimeChangePlayback();
	}

	private void OnProjectileChangeVisibilityChanged()
	{
		SetRuntimePreviewActive(IsVisibleInTree());
	}

	private void SetRuntimePreviewActive(bool active)
	{
		_runtimePreviewActive = active;
		SetProcess(active && _runtimePreviewRunning);
		if (GodotObject.IsInstanceValid(_runtimeSourceProjectile))
		{
			_runtimeSourceProjectile.ProcessMode = (ProcessModeEnum)(active ? 0 : 4);
		}
		if (GodotObject.IsInstanceValid(_runtimeTargetProjectile))
		{
			_runtimeTargetProjectile.ProcessMode = (ProcessModeEnum)(active ? 0 : 4);
		}
		if (!active)
		{
			_runtimePreviewDragging = false;
			_projectileSelectorWindow?.Dismiss();
			_audioSelectorWindow?.Dismiss();
		}
		else
		{
			UpdateRuntimeChangePlayback();
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (_runtimePreviewActive && _runtimePreviewRunning)
		{
			_runtimePreviewProgress = Mathf.Clamp(_runtimePreviewProgress + delta / 1.6, 0.0, 1.0);
			_runtimePreviewTimeline?.SetValueNoSignal(_runtimePreviewProgress);
			UpdateRuntimeChangePlayback();
			if (_runtimePreviewProgress >= 1.0)
			{
				SetRuntimeChangePreviewRunning(running: false);
			}
		}
	}

	private void UpdateRuntimeChangePlayback()
	{
		float num = (float)Mathf.Clamp(_runtimePreviewProgress, 0.0, 1.0);
		bool flag = num >= 0.5f;
		float weight = (flag ? ((num - 0.5f) * 2f) : (num * 2f));
		Vector2 position = (flag ? new Vector2(0f, 0f).Lerp(new Vector2(250f, -12f), weight) : new Vector2(-250f, 12f).Lerp(Vector2.Zero, weight));
		SetRuntimeProjectileVisual(_runtimeSourceProjectile, !flag, position);
		SetRuntimeProjectileVisual(_runtimeTargetProjectile, flag, position);
		if (GodotObject.IsInstanceValid(_runtimePreviewStatus) && GetEntryCount() > 0)
		{
			_runtimePreviewStatus.Text = (IsChainMode ? $"变化项 {_selectedIndex + 1}/{GetEntryCount()} · {(flag ? "目标子弹飞行" : "来源子弹飞行")} · {num:P0}" : $"单次变化 · {(flag ? "目标子弹飞行" : "来源子弹飞行")} · {num:P0}");
		}
	}

	private void SetRuntimeProjectileVisual(Node node, bool visible, Vector2 position)
	{
		if (GodotObject.IsInstanceValid(node))
		{
			if (node is CanvasItem canvasItem)
			{
				canvasItem.Visible = visible;
			}
			if (node is Node2D node2D)
			{
				node2D.Position = position;
			}
			else if (node is Control control)
			{
				control.Position = position;
			}
			node.ProcessMode = (ProcessModeEnum)((_runtimePreviewActive & visible) ? 0 : 4);
		}
	}

	private void BindEditorLayout(VBoxContainer root)
	{
		_modeBadge = root.GetNode<Label>("%ModeBadge");
		_summaryLabel = root.GetNode<Label>("%SummaryLabel");
		_summaryLabel.Text = BuildSummary();
		BindRuntimeChangePreview(root.GetNode<Control>("%RuntimePreview"));
		root.GetNode<Button>("%AddButton").Pressed += AddChangeEntry;
		_removeButton = root.GetNode<Button>("%RemoveButton");
		_moveUpButton = root.GetNode<Button>("%MoveUpButton");
		_moveDownButton = root.GetNode<Button>("%MoveDownButton");
		_removeButton.Pressed += RemoveSelectedChangeEntry;
		_moveUpButton.Pressed += () =>
		{
			MoveSelectedChangeEntry(-1);
		};
		_moveDownButton.Pressed += () =>
		{
			MoveSelectedChangeEntry(1);
		};
		_chainList = root.GetNode<ItemList>("%ChainList");
		_chainList.ItemSelected += (long index) =>
		{
			OnSelectedChangeEntryChanged((int)index);
		};
		Control chainCanvas = root.GetNode<Control>("%ChainCanvas");
		_chainCanvas = chainCanvas;
		chainCanvas.Draw += () =>
		{
			DrawChangeChain(chainCanvas);
		};
		_chainPanel = root.GetNode<Control>("%ChainPanel");
		_itemEditorHost = root.GetNode<VBoxContainer>("%ItemEditorHost");
		_selectedLabel = root.GetNode<Label>("%SelectedLabel");
		_emptyState = root.GetNode<Control>("%EmptyState");
		_emptyStateLabel = _emptyState.GetNode<Label>("EmptyStateLabel");
		_sourceField = root.GetNode<Control>("%SourceField");
		_targetField = root.GetNode<Control>("%TargetField");
		_audioField = root.GetNode<Control>("%AudioField");
		_entryPreviewLabel = root.GetNode<Label>("%EntryPreviewLabel");
		_sourceProjectileKeyEdit = root.GetNode<LineEdit>("%SourceProjectileKeyEdit");
		_changeAudioEdit = root.GetNode<LineEdit>("%ChangeAudioLineEdit");
		_projectileDataPicker = root.GetNode<XWResourcePicker>("%ProjectileDataPicker");
		_projectileDataPicker.Setup("TowerDefenseProjectileCreateData");
		_projectileDataPicker.ResourceChanged += (Resource resource) =>
		{
			SetEntryProperty("projectileData", Variant.From(in resource), "修改来源子弹数据");
		};
		_projectileConfigPicker = root.GetNode<XWResourcePicker>("%ProjectileConfigPicker");
		_projectileConfigPicker.Setup("TowerDefenseProjectileConfig");
		_projectileConfigPicker.ResourceChanged += (Resource resource) =>
		{
			SetEntryProperty("projectileConfig", Variant.From(in resource), "修改目标子弹");
		};
		root.GetNode<Button>("%ChooseSourceButton").Pressed += () =>
		{
			ShowProjectileSelectorWindow(ProjectileSelectorTarget.SourceProjectile);
		};
		root.GetNode<Button>("%ChooseTargetButton").Pressed += () =>
		{
			ShowProjectileSelectorWindow(ProjectileSelectorTarget.TargetProjectile);
		};
		root.GetNode<Button>("%ChooseAudioButton").Pressed += ShowAudioSelectorWindow;
		root.GetNode<Button>("%ClearAudioButton").Pressed += ClearAudioSelection;
	}

	private void ApplyEditModeLayout()
	{
		if (GodotObject.IsInstanceValid(_chainPanel))
		{
			_chainPanel.Visible = IsChainMode;
		}
		if (GodotObject.IsInstanceValid(_runtimePreviousButton))
		{
			_runtimePreviousButton.Visible = IsChainMode;
		}
		if (GodotObject.IsInstanceValid(_runtimeNextButton))
		{
			_runtimeNextButton.Visible = IsChainMode;
		}
		if (GodotObject.IsInstanceValid(_modeBadge))
		{
			_modeBadge.Text = (IsChainMode ? "连续变化链" : "单次子弹变化");
		}
		if (GodotObject.IsInstanceValid(_summaryLabel))
		{
			_summaryLabel.Text = BuildSummary();
		}
	}

	private void RenderSelectedEntryEditor()
	{
		if (!GodotObject.IsInstanceValid(_itemEditorHost) || !GodotObject.IsInstanceValid(_emptyState))
		{
			return;
		}
		ChangeProjectileSingleConfig selectedEntry = GetSelectedEntry();
		bool flag = GodotObject.IsInstanceValid(selectedEntry);
		_emptyState.Visible = !flag;
		_sourceField.Visible = flag;
		_targetField.Visible = flag;
		_audioField.Visible = flag;
		_entryPreviewLabel.Visible = flag;
		_selectedLabel.Visible = flag;
		if (!flag)
		{
			_emptyStateLabel.Text = (IsChainMode ? "还没有子弹变化项。使用左侧的添加按钮建立一段变化。" : "单条子弹变化资源不可用。");
			_projectileDataPicker.SetEditedResource(null);
			_projectileConfigPicker.SetEditedResource(null);
			RebuildRuntimeChangePreview();
			return;
		}
		_updatingControls = true;
		try
		{
			_selectedLabel.Text = (IsChainMode ? $"变化项 {_selectedIndex + 1}: {FormatEntryTitle(selectedEntry)}" : ("单次变化: " + FormatEntryTitle(selectedEntry)));
			_sourceProjectileKeyEdit.Text = ReadEntryProjectileName(selectedEntry);
			_projectileDataPicker.SetEditedResource(selectedEntry.projectileData);
			_projectileConfigPicker.SetEditedResource(selectedEntry.projectileConfig);
			_changeAudioEdit.Text = selectedEntry.changeAudio ?? "";
			_entryPreviewLabel.Text = BuildSelectedEntryPreview(selectedEntry);
		}
		finally
		{
			_updatingControls = false;
		}
		RebuildRuntimeChangePreview();
	}

	private void AddChangeEntry()
	{
		if (IsChainMode && _editingChange != null && _propertyBinding != null)
		{
			EnsureChangeList(_editingChange);
			ChangeProjectileSingleConfig item = new ChangeProjectileSingleConfig
			{
				ResourceName = $"ProjectileChange_{_editingChange.changeList.Count + 1}"
			};
			Array<ChangeProjectileSingleConfig> array = new Array<ChangeProjectileSingleConfig>(_editingChange.changeList);
			array.Add(item);
			ReplaceChangeList(array, "添加子弹变化", array.Count - 1);
		}
	}

	private void RemoveSelectedChangeEntry()
	{
		if (IsChainMode && _editingChange?.changeList != null && _selectedIndex >= 0 && _selectedIndex < _editingChange.changeList.Count)
		{
			Array<ChangeProjectileSingleConfig> array = new Array<ChangeProjectileSingleConfig>(_editingChange.changeList);
			array.RemoveAt(_selectedIndex);
			ReplaceChangeList(array, "删除子弹变化", Mathf.Min(_selectedIndex, array.Count - 1));
		}
	}

	private void MoveSelectedChangeEntry(int direction)
	{
		if (IsChainMode && _editingChange?.changeList != null && _selectedIndex >= 0)
		{
			int num = _selectedIndex + Math.Sign(direction);
			if (num >= 0 && num < _editingChange.changeList.Count)
			{
				Array<ChangeProjectileSingleConfig> array = new Array<ChangeProjectileSingleConfig>(_editingChange.changeList);
				ChangeProjectileSingleConfig item = array[_selectedIndex];
				array.RemoveAt(_selectedIndex);
				array.Insert(num, item);
				ReplaceChangeList(array, (direction < 0) ? "上移子弹变化" : "下移子弹变化", num);
			}
		}
	}

	private void ReplaceChangeList(Array<ChangeProjectileSingleConfig> next, string actionName, int nextIndex)
	{
		if (IsChainMode && GodotObject.IsInstanceValid(_editingChange) && _propertyBinding != null && next != null)
		{
			_selectedIndex = ((next.Count == 0) ? (-1) : Mathf.Clamp(nextIndex, 0, next.Count - 1));
			_propertyBinding.SetValue(_editingChange, "changeList", next, actionName);
			RefreshSelectedEntryVisuals();
		}
	}

	private void OnSelectedChangeEntryChanged(int index)
	{
		if (IsChainMode)
		{
			_selectedIndex = index;
			ClampSelectedIndex();
			RefreshChangeChain();
			RenderSelectedEntryEditor();
			QueueChangeCanvasRedraw();
		}
	}

	private void RefreshSelectedEntryVisuals()
	{
		RefreshChangeChain();
		RenderSelectedEntryEditor();
		QueueChangeCanvasRedraw();
	}

	private void SetEntryProperty(StringName property, Variant value, string actionName)
	{
		if (!_updatingControls && _propertyBinding != null)
		{
			ChangeProjectileSingleConfig selectedEntry = GetSelectedEntry();
			if (GodotObject.IsInstanceValid(selectedEntry) && !property.IsEmpty)
			{
				_propertyBinding.SetValue(selectedEntry, property, value, actionName);
			}
		}
	}

	private void ShowProjectileSelectorWindow(ProjectileSelectorTarget target)
	{
		_projectileSelectorTarget = target;
		EnsureProjectileSelectorWindow();
		if (GodotObject.IsInstanceValid(_projectileSelectorWindow))
		{
			ChangeProjectileSingleConfig selectedEntry = GetSelectedEntry();
			bool flag = target == ProjectileSelectorTarget.SourceProjectile;
			string currentPath = ((!flag) ? (selectedEntry?.projectileConfig?.ResourcePath ?? "") : (selectedEntry?.projectileData?.ResourcePath ?? ""));
			string projectPath = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
			string[] resourceClassNames = ((!flag) ? new string[1] { "TowerDefenseProjectileConfig" } : new string[2] { "TowerDefenseProjectileConfig", "TowerDefenseProjectileCreateData" });
			_projectileSelectorWindow.OpenResourceLibrary("Projectile", flag ? "来源子弹" : "目标子弹", currentPath, projectPath, resourceClassNames, new string[2] { "Asset/Config/Projectile", "Registry/Projectile/Config" }, "res://addons/ModEditor/Icons/ResourceProjectile.svg", SelectProjectileChoice);
		}
	}

	private void EnsureProjectileSelectorWindow()
	{
		if (!GodotObject.IsInstanceValid(_projectileSelectorWindow))
		{
			_projectileSelectorWindow = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_projectileSelectorWindow))
			{
				AddChild(_projectileSelectorWindow, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void SelectProjectileChoice(XWGameplayResourceChoice choice)
	{
		if (choice == null || string.IsNullOrWhiteSpace(choice.ResourcePath) || !GodotObject.IsInstanceValid(GetSelectedEntry()))
		{
			return;
		}
		Resource resource = (ResourceLoader.Exists(choice.ResourcePath) ? ResourceLoader.Load<Resource>(choice.ResourcePath, null, ResourceLoader.CacheMode.Reuse) : null);
		if (!GodotObject.IsInstanceValid(resource))
		{
			XWEditorInterface.Instance?.ShowToast("无法加载子弹资源: " + choice.ResourcePath);
		}
		else if (_projectileSelectorTarget == ProjectileSelectorTarget.SourceProjectile)
		{
			TowerDefenseProjectileCreateData from = resource as TowerDefenseProjectileCreateData;
			string text = from?.projectileName.ToString() ?? "";
			if (resource is TowerDefenseProjectileConfig config)
			{
				text = ReadProjectileConfigKey(config);
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				text = Path.GetFileNameWithoutExtension(choice.ResourcePath);
			}
			if (!GodotObject.IsInstanceValid(from))
			{
				from = new TowerDefenseProjectileCreateData
				{
					projectileName = new StringName(text),
					ResourceName = text + "CreateData"
				};
			}
			SetEntryProperty("projectileData", Variant.From(in from), "选择来源子弹 " + text);
		}
		else
		{
			TowerDefenseProjectileConfig from2 = resource as TowerDefenseProjectileConfig;
			if (from2 == null)
			{
				XWEditorInterface.Instance?.ShowToast("目标子弹资源类型不匹配: " + choice.ResourcePath);
				return;
			}
			string text2 = ReadProjectileConfigKey(from2);
			SetEntryProperty("projectileConfig", Variant.From(in from2), "选择目标子弹 " + text2);
		}
	}

	private void ShowAudioSelectorWindow()
	{
		EnsureAudioSelectorWindow();
		if (GodotObject.IsInstanceValid(_audioSelectorWindow))
		{
			_audioSelectorWindow.Open(XWGameplayResourceKind.Audio, GetSelectedEntry()?.changeAudio ?? "", SelectProjectileAudioChoice, (XWGameplayResourceChoice choice) => choice.Kind == XWGameplayResourceKind.Audio, lockKind: true, "变化音效图鉴");
		}
	}

	private void EnsureAudioSelectorWindow()
	{
		if (!GodotObject.IsInstanceValid(_audioSelectorWindow))
		{
			_audioSelectorWindow = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_audioSelectorWindow))
			{
				AddChild(_audioSelectorWindow, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void SelectProjectileAudioChoice(XWGameplayResourceChoice choice)
	{
		if (!(choice == null) && choice.Kind == XWGameplayResourceKind.Audio)
		{
			SetEntryProperty("changeAudio", Variant.From<string>(choice.Key), "选择音效 " + choice.Key);
		}
	}

	private void ClearAudioSelection()
	{
		SetEntryProperty("changeAudio", Variant.From<string>(""), "清除变化音效");
	}

	private void RefreshChangeChain()
	{
		ClampSelectedIndex();
		if (GodotObject.IsInstanceValid(_summaryLabel))
		{
			_summaryLabel.Text = BuildSummary();
		}
		if (!IsChainMode || _editingChange == null)
		{
			UpdateToolbarState();
			return;
		}
		EnsureChangeList(_editingChange);
		if (!GodotObject.IsInstanceValid(_chainList))
		{
			return;
		}
		_updatingControls = true;
		try
		{
			_chainList.Clear();
			for (int i = 0; i < _editingChange.changeList.Count; i++)
			{
				ChangeProjectileSingleConfig entry = _editingChange.changeList[i];
				_chainList.AddItem($"{i + 1}. {FormatEntryTitle(entry)}");
			}
			if (_editingChange.changeList.Count == 0)
			{
				_chainList.AddItem("未配置 changeList");
			}
			else if (_selectedIndex >= 0 && _selectedIndex < _chainList.ItemCount)
			{
				_chainList.Select(_selectedIndex);
			}
		}
		finally
		{
			_updatingControls = false;
		}
		UpdateToolbarState();
	}

	private void DrawChangeChain(Control canvas)
	{
		if (!IsChainMode || !GodotObject.IsInstanceValid(canvas) || _editingChange?.changeList == null)
		{
			return;
		}
		Rect2 rect = new Rect2(Vector2.Zero, canvas.Size);
		canvas.DrawRect(rect, new Color(0.055f, 0.06f, 0.07f));
		int count = _editingChange.changeList.Count;
		if (count == 0)
		{
			canvas.DrawString(GetThemeDefaultFont(), new Vector2(18f, 42f), "暂无子弹变化项", HorizontalAlignment.Left, -1f, 14, new Color(0.72f, 0.74f, 0.78f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			return;
		}
		float num = Mathf.Clamp((canvas.Size.X - 48f) / (float)Mathf.Max(count, 1) - 14f, 76f, 150f);
		float num2 = 52f;
		float num3 = Mathf.Max(24f, canvas.Size.Y * 0.5f - num2 * 0.5f);
		float num4 = 18f;
		for (int i = 0; i < count; i++)
		{
			ChangeProjectileSingleConfig entry = _editingChange.changeList[i];
			Rect2 rect2 = new Rect2(new Vector2(num4, num3), new Vector2(num, num2));
			bool flag = i == _selectedIndex;
			Color color = (flag ? new Color(0.18f, 0.34f, 0.58f) : new Color(0.16f, 0.17f, 0.18f));
			Color color2 = (flag ? new Color(0.45f, 0.72f, 1f) : new Color(0.31f, 0.33f, 0.36f));
			canvas.DrawRect(rect2, color);
			canvas.DrawRect(rect2, color2, filled: false, 2f);
			canvas.DrawString(GetThemeDefaultFont(), rect2.Position + new Vector2(8f, 21f), $"{i + 1}. {FormatProjectileData(entry)}", HorizontalAlignment.Left, num - 12f, 12, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			canvas.DrawString(GetThemeDefaultFont(), rect2.Position + new Vector2(8f, 40f), FormatProjectileConfig(entry), HorizontalAlignment.Left, num - 12f, 12, new Color(0.78f, 0.82f, 0.88f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			float num5 = num4 + num + 14f;
			if (i < count - 1)
			{
				Vector2 vector = new Vector2(num4 + num, num3 + num2 * 0.5f);
				Vector2 vector2 = new Vector2(num5, num3 + num2 * 0.5f);
				canvas.DrawLine(vector, vector2, new Color(0.56f, 0.66f, 0.78f), 2f);
				canvas.DrawLine(vector2 - new Vector2(8f, 5f), vector2, new Color(0.56f, 0.66f, 0.78f), 2f);
				canvas.DrawLine(vector2 - new Vector2(8f, -5f), vector2, new Color(0.56f, 0.66f, 0.78f), 2f);
			}
			num4 = num5;
		}
	}

	private void EnsureChangeList(ChangeProjectileConfig projectileChange)
	{
		if (projectileChange.changeList == null)
		{
			projectileChange.changeList = new Array<ChangeProjectileSingleConfig>();
		}
	}

	private ChangeProjectileSingleConfig GetSelectedEntry()
	{
		if (_editMode == ProjectileChangeEditMode.Single)
		{
			return _editingSingle;
		}
		if (_editingChange?.changeList == null || _selectedIndex < 0 || _selectedIndex >= _editingChange.changeList.Count)
		{
			return null;
		}
		return _editingChange.changeList[_selectedIndex];
	}

	private void ClampSelectedIndex()
	{
		if (_editMode == ProjectileChangeEditMode.Single)
		{
			_selectedIndex = ((!GodotObject.IsInstanceValid(_editingSingle)) ? (-1) : 0);
		}
		else if (_editingChange?.changeList == null || _editingChange.changeList.Count == 0)
		{
			_selectedIndex = -1;
		}
		else
		{
			_selectedIndex = Mathf.Clamp(_selectedIndex, 0, _editingChange.changeList.Count - 1);
		}
	}

	private void UpdateToolbarState()
	{
		bool flag = IsChainMode && _editingChange?.changeList != null && _selectedIndex >= 0 && _selectedIndex < _editingChange.changeList.Count;
		if (GodotObject.IsInstanceValid(_removeButton))
		{
			_removeButton.Disabled = !flag || !IsChainMode;
		}
		if (GodotObject.IsInstanceValid(_moveUpButton))
		{
			_moveUpButton.Disabled = !flag || !IsChainMode || _selectedIndex <= 0;
		}
		if (GodotObject.IsInstanceValid(_moveDownButton))
		{
			_moveDownButton.Disabled = !flag || !IsChainMode || _editingChange == null || _selectedIndex >= _editingChange.changeList.Count - 1;
		}
	}

	private void UpdateSelectedEntryLabel()
	{
		if (GodotObject.IsInstanceValid(_selectedLabel))
		{
			_selectedLabel.Text = (IsChainMode ? $"变化项 {_selectedIndex + 1}: {FormatEntryTitle(GetSelectedEntry())}" : ("单次变化: " + FormatEntryTitle(GetSelectedEntry())));
		}
	}

	private void QueueChangeCanvasRedraw()
	{
		if (GodotObject.IsInstanceValid(_chainCanvas))
		{
			_chainCanvas.QueueRedraw();
		}
	}

	private string BuildSummary()
	{
		if (!IsChainMode)
		{
			return "单次子弹变化: 在游戏草坪中预览来源与目标";
		}
		return $"子弹变化链路: {GetEntryCount()} 条变化";
	}

	private void DisposeProjectileChangeEditor()
	{
		_runtimePreviewRequestVersion++;
		_runtimePreviewRunning = false;
		_runtimePreviewActive = false;
		SetProcess(enable: false);
		_projectileSelectorWindow?.Dismiss();
		_audioSelectorWindow?.Dismiss();
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		QueueNodeForDeletion(_runtimeSourceProjectile);
		QueueNodeForDeletion(_runtimeTargetProjectile);
		QueueNodeForDeletion(_projectileSelectorWindow);
		QueueNodeForDeletion(_audioSelectorWindow);
		_editMode = ProjectileChangeEditMode.None;
		_editingChange = null;
		_editingSingle = null;
		_selectedIndex = -1;
		_chainPanel = null;
		_chainList = null;
		_chainCanvas = null;
		_itemEditorHost = null;
		_modeBadge = null;
		_summaryLabel = null;
		_selectedLabel = null;
		_emptyState = null;
		_emptyStateLabel = null;
		_sourceField = null;
		_targetField = null;
		_audioField = null;
		_entryPreviewLabel = null;
		_runtimePreviewViewport = null;
		_runtimePreviewRoot = null;
		_runtimePreviewStatus = null;
		_runtimePreviewRunButton = null;
		_runtimePreviewTimeline = null;
		_runtimePreviewInput = null;
		_runtimeZoomLabel = null;
		_runtimePreviousButton = null;
		_runtimeNextButton = null;
		_runtimeSourceProjectile = null;
		_runtimeTargetProjectile = null;
		_runtimePreviewProgress = 0.0;
		_runtimePreviewBasePosition = Vector2.Zero;
		_runtimePreviewPan = Vector2.Zero;
		_runtimePreviewZoom = 1f;
		_runtimePreviewDragging = false;
		_projectileDataPicker = null;
		_projectileConfigPicker = null;
		_sourceProjectileKeyEdit = null;
		_changeAudioEdit = null;
		_projectileSelectorWindow = null;
		_projectileSelectorTarget = ProjectileSelectorTarget.SourceProjectile;
		_audioSelectorWindow = null;
		_removeButton = null;
		_moveUpButton = null;
		_moveDownButton = null;
		_updatingControls = false;
	}

	private static void QueueNodeForDeletion(Node node)
	{
		if (GodotObject.IsInstanceValid(node) && !node.IsQueuedForDeletion())
		{
			node.ProcessMode = ProcessModeEnum.Disabled;
			if (node is Window window)
			{
				window.Hide();
			}
			node.QueueFree();
		}
	}

	private static string BuildSelectedEntryPreview(ChangeProjectileSingleConfig entry)
	{
		return $"来源: {FormatProjectileData(entry)}\n目标: {FormatProjectileConfig(entry)}\n音效: {EmptyToPlaceholder(entry?.changeAudio)}";
	}

	private static string FormatEntryTitle(ChangeProjectileSingleConfig entry)
	{
		if (!GodotObject.IsInstanceValid(entry))
		{
			return "未配置";
		}
		return FormatProjectileData(entry) + " -> " + FormatProjectileConfig(entry);
	}

	private static string FormatProjectileData(ChangeProjectileSingleConfig entry)
	{
		string text = ReadEntryProjectileName(entry);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return "未配置来源";
	}

	private static string FormatProjectileConfig(ChangeProjectileSingleConfig entry)
	{
		if (!GodotObject.IsInstanceValid(entry?.projectileConfig))
		{
			return "未配置目标";
		}
		string text = ReadProjectileConfigKey(entry.projectileConfig);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return FormatResource(entry.projectileConfig);
	}

	private static string ReadEntryProjectileName(ChangeProjectileSingleConfig entry)
	{
		if (!GodotObject.IsInstanceValid(entry))
		{
			return "";
		}
		if (GodotObject.IsInstanceValid(entry.projectileData))
		{
			return entry.projectileData.projectileName.ToString();
		}
		Variant variant = entry.Get("projectileName");
		if (variant.VariantType == Variant.Type.String || variant.VariantType == Variant.Type.StringName)
		{
			return variant.AsString();
		}
		return "";
	}

	private static string ReadProjectileConfigKey(TowerDefenseProjectileConfig config)
	{
		if (!GodotObject.IsInstanceValid(config))
		{
			return "";
		}
		if (!string.IsNullOrWhiteSpace(config.name))
		{
			return config.name;
		}
		return config.ResourceName;
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
		return new List<MethodInfo>(56)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEmbeddedInspectorPropertyChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.RenderProjectileChangeEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryBindEditingResource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetEntryCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEditingTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindRuntimeChangePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "panel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleRuntimePreviewInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimePreviewZoom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "zoom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pointer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetRuntimePreviewView, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyRuntimePreviewView, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdvanceRuntimeChangePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildRuntimeChangePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsRuntimePreviewRequestCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "requestVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "requestedResource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "requestedRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddRuntimeProjectileScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "fallbackColor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "previewRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeChangePreviewRunning, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "running", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnProjectileChangeVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetRuntimePreviewActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateRuntimeChangePlayback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetRuntimeProjectileVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindEditorLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyEditModeLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderSelectedEntryEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddChangeEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedChangeEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedChangeEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceChangeList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "nextIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSelectedChangeEntryChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshSelectedEntryVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEntryProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowProjectileSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureProjectileSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowAudioSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureAudioSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearAudioSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshChangeChain, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawChangeChain, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvas", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureChangeList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectileChange", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedEntry, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClampSelectedIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateToolbarState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSelectedEntryLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueChangeCanvasRedraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeProjectileChangeEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueNodeForDeletion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSelectedEntryPreview, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "entry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatEntryTitle, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "entry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatProjectileData, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "entry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatProjectileConfig, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "entry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadEntryProjectileName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "entry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadProjectileConfigKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.OnEmbeddedInspectorPropertyChanged && args.Count == 4)
		{
			OnEmbeddedInspectorPropertyChanged(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderProjectileChangeEditor && args.Count == 0)
		{
			RenderProjectileChangeEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.TryBindEditingResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryBindEditingResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetEntryCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEntryCount());
			return true;
		}
		if (method == MethodName.GetEditingTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Resource>(GetEditingTarget());
			return true;
		}
		if (method == MethodName.BindRuntimeChangePreview && args.Count == 1)
		{
			BindRuntimeChangePreview(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleRuntimePreviewInput && args.Count == 1)
		{
			HandleRuntimePreviewInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRuntimePreviewZoom && args.Count == 2)
		{
			SetRuntimePreviewZoom(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetRuntimePreviewView && args.Count == 0)
		{
			ResetRuntimePreviewView();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRuntimePreviewView && args.Count == 0)
		{
			ApplyRuntimePreviewView();
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceRuntimeChangePreview && args.Count == 1)
		{
			AdvanceRuntimeChangePreview(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildRuntimeChangePreview && args.Count == 0)
		{
			RebuildRuntimeChangePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.IsRuntimePreviewRequestCurrent && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRuntimePreviewRequestCurrent(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<Node2D>(in args[2])));
			return true;
		}
		if (method == MethodName.AddRuntimeProjectileScene && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<Node>(AddRuntimeProjectileScene(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]), VariantUtils.ConvertTo<Node2D>(in args[4])));
			return true;
		}
		if (method == MethodName.SetRuntimeChangePreviewRunning && args.Count == 1)
		{
			SetRuntimeChangePreviewRunning(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnProjectileChangeVisibilityChanged && args.Count == 0)
		{
			OnProjectileChangeVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.SetRuntimePreviewActive && args.Count == 1)
		{
			SetRuntimePreviewActive(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRuntimeChangePlayback && args.Count == 0)
		{
			UpdateRuntimeChangePlayback();
			ret = default;
			return true;
		}
		if (method == MethodName.SetRuntimeProjectileVisual && args.Count == 3)
		{
			SetRuntimeProjectileVisual(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindEditorLayout && args.Count == 1)
		{
			BindEditorLayout(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyEditModeLayout && args.Count == 0)
		{
			ApplyEditModeLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderSelectedEntryEditor && args.Count == 0)
		{
			RenderSelectedEntryEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.AddChangeEntry && args.Count == 0)
		{
			AddChangeEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedChangeEntry && args.Count == 0)
		{
			RemoveSelectedChangeEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedChangeEntry && args.Count == 1)
		{
			MoveSelectedChangeEntry(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceChangeList && args.Count == 3)
		{
			ReplaceChangeList(VariantUtils.ConvertToArray<ChangeProjectileSingleConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSelectedChangeEntryChanged && args.Count == 1)
		{
			OnSelectedChangeEntryChanged(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSelectedEntryVisuals && args.Count == 0)
		{
			RefreshSelectedEntryVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.SetEntryProperty && args.Count == 3)
		{
			SetEntryProperty(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowProjectileSelectorWindow && args.Count == 1)
		{
			ShowProjectileSelectorWindow(VariantUtils.ConvertTo<ProjectileSelectorTarget>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureProjectileSelectorWindow && args.Count == 0)
		{
			EnsureProjectileSelectorWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowAudioSelectorWindow && args.Count == 0)
		{
			ShowAudioSelectorWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureAudioSelectorWindow && args.Count == 0)
		{
			EnsureAudioSelectorWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearAudioSelection && args.Count == 0)
		{
			ClearAudioSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshChangeChain && args.Count == 0)
		{
			RefreshChangeChain();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawChangeChain && args.Count == 1)
		{
			DrawChangeChain(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureChangeList && args.Count == 1)
		{
			EnsureChangeList(VariantUtils.ConvertTo<ChangeProjectileConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedEntry && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ChangeProjectileSingleConfig>(GetSelectedEntry());
			return true;
		}
		if (method == MethodName.ClampSelectedIndex && args.Count == 0)
		{
			ClampSelectedIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateToolbarState && args.Count == 0)
		{
			UpdateToolbarState();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSelectedEntryLabel && args.Count == 0)
		{
			UpdateSelectedEntryLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueChangeCanvasRedraw && args.Count == 0)
		{
			QueueChangeCanvasRedraw();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSummary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSummary());
			return true;
		}
		if (method == MethodName.DisposeProjectileChangeEditor && args.Count == 0)
		{
			DisposeProjectileChangeEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueNodeForDeletion && args.Count == 1)
		{
			QueueNodeForDeletion(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSelectedEntryPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSelectedEntryPreview(VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatEntryTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatEntryTitle(VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatProjectileData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatProjectileData(VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatProjectileConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatProjectileConfig(VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadEntryProjectileName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadEntryProjectileName(VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadProjectileConfigKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadProjectileConfigKey(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0])));
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
		if (method == MethodName.QueueNodeForDeletion && args.Count == 1)
		{
			QueueNodeForDeletion(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSelectedEntryPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSelectedEntryPreview(VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatEntryTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatEntryTitle(VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatProjectileData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatProjectileData(VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatProjectileConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatProjectileConfig(VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadEntryProjectileName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadEntryProjectileName(VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadProjectileConfigKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadProjectileConfigKey(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0])));
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
		if (method == MethodName.OnEmbeddedInspectorPropertyChanged)
		{
			return true;
		}
		if (method == MethodName.RenderProjectileChangeEditor)
		{
			return true;
		}
		if (method == MethodName.TryBindEditingResource)
		{
			return true;
		}
		if (method == MethodName.GetEntryCount)
		{
			return true;
		}
		if (method == MethodName.GetEditingTarget)
		{
			return true;
		}
		if (method == MethodName.BindRuntimeChangePreview)
		{
			return true;
		}
		if (method == MethodName.HandleRuntimePreviewInput)
		{
			return true;
		}
		if (method == MethodName.SetRuntimePreviewZoom)
		{
			return true;
		}
		if (method == MethodName.ResetRuntimePreviewView)
		{
			return true;
		}
		if (method == MethodName.ApplyRuntimePreviewView)
		{
			return true;
		}
		if (method == MethodName.AdvanceRuntimeChangePreview)
		{
			return true;
		}
		if (method == MethodName.RebuildRuntimeChangePreview)
		{
			return true;
		}
		if (method == MethodName.IsRuntimePreviewRequestCurrent)
		{
			return true;
		}
		if (method == MethodName.AddRuntimeProjectileScene)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeChangePreviewRunning)
		{
			return true;
		}
		if (method == MethodName.OnProjectileChangeVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.SetRuntimePreviewActive)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.UpdateRuntimeChangePlayback)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeProjectileVisual)
		{
			return true;
		}
		if (method == MethodName.BindEditorLayout)
		{
			return true;
		}
		if (method == MethodName.ApplyEditModeLayout)
		{
			return true;
		}
		if (method == MethodName.RenderSelectedEntryEditor)
		{
			return true;
		}
		if (method == MethodName.AddChangeEntry)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedChangeEntry)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedChangeEntry)
		{
			return true;
		}
		if (method == MethodName.ReplaceChangeList)
		{
			return true;
		}
		if (method == MethodName.OnSelectedChangeEntryChanged)
		{
			return true;
		}
		if (method == MethodName.RefreshSelectedEntryVisuals)
		{
			return true;
		}
		if (method == MethodName.SetEntryProperty)
		{
			return true;
		}
		if (method == MethodName.ShowProjectileSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.EnsureProjectileSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.ShowAudioSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.EnsureAudioSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.ClearAudioSelection)
		{
			return true;
		}
		if (method == MethodName.RefreshChangeChain)
		{
			return true;
		}
		if (method == MethodName.DrawChangeChain)
		{
			return true;
		}
		if (method == MethodName.EnsureChangeList)
		{
			return true;
		}
		if (method == MethodName.GetSelectedEntry)
		{
			return true;
		}
		if (method == MethodName.ClampSelectedIndex)
		{
			return true;
		}
		if (method == MethodName.UpdateToolbarState)
		{
			return true;
		}
		if (method == MethodName.UpdateSelectedEntryLabel)
		{
			return true;
		}
		if (method == MethodName.QueueChangeCanvasRedraw)
		{
			return true;
		}
		if (method == MethodName.BuildSummary)
		{
			return true;
		}
		if (method == MethodName.DisposeProjectileChangeEditor)
		{
			return true;
		}
		if (method == MethodName.QueueNodeForDeletion)
		{
			return true;
		}
		if (method == MethodName.BuildSelectedEntryPreview)
		{
			return true;
		}
		if (method == MethodName.FormatEntryTitle)
		{
			return true;
		}
		if (method == MethodName.FormatProjectileData)
		{
			return true;
		}
		if (method == MethodName.FormatProjectileConfig)
		{
			return true;
		}
		if (method == MethodName.ReadEntryProjectileName)
		{
			return true;
		}
		if (method == MethodName.ReadProjectileConfigKey)
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
		if (name == PropertyName._editMode)
		{
			_editMode = VariantUtils.ConvertTo<ProjectileChangeEditMode>(in value);
			return true;
		}
		if (name == PropertyName._editingChange)
		{
			_editingChange = VariantUtils.ConvertTo<ChangeProjectileConfig>(in value);
			return true;
		}
		if (name == PropertyName._editingSingle)
		{
			_editingSingle = VariantUtils.ConvertTo<ChangeProjectileSingleConfig>(in value);
			return true;
		}
		if (name == PropertyName._selectedIndex)
		{
			_selectedIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._chainPanel)
		{
			_chainPanel = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._chainList)
		{
			_chainList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._chainCanvas)
		{
			_chainCanvas = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._itemEditorHost)
		{
			_itemEditorHost = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._modeBadge)
		{
			_modeBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			_summaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._selectedLabel)
		{
			_selectedLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._emptyState)
		{
			_emptyState = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._emptyStateLabel)
		{
			_emptyStateLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._sourceField)
		{
			_sourceField = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._targetField)
		{
			_targetField = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._audioField)
		{
			_audioField = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._entryPreviewLabel)
		{
			_entryPreviewLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewViewport)
		{
			_runtimePreviewViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewRoot)
		{
			_runtimePreviewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewStatus)
		{
			_runtimePreviewStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewRunButton)
		{
			_runtimePreviewRunButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewTimeline)
		{
			_runtimePreviewTimeline = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewInput)
		{
			_runtimePreviewInput = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._runtimeZoomLabel)
		{
			_runtimeZoomLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._runtimeSourceProjectile)
		{
			_runtimeSourceProjectile = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._runtimeTargetProjectile)
		{
			_runtimeTargetProjectile = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewProgress)
		{
			_runtimePreviewProgress = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewRunning)
		{
			_runtimePreviewRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewActive)
		{
			_runtimePreviewActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewRequestVersion)
		{
			_runtimePreviewRequestVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewBasePosition)
		{
			_runtimePreviewBasePosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewPan)
		{
			_runtimePreviewPan = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewZoom)
		{
			_runtimePreviewZoom = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviewDragging)
		{
			_runtimePreviewDragging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._projectileDataPicker)
		{
			_projectileDataPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._projectileConfigPicker)
		{
			_projectileConfigPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._sourceProjectileKeyEdit)
		{
			_sourceProjectileKeyEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._changeAudioEdit)
		{
			_changeAudioEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._removeButton)
		{
			_removeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveUpButton)
		{
			_moveUpButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveDownButton)
		{
			_moveDownButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._runtimePreviousButton)
		{
			_runtimePreviousButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._runtimeNextButton)
		{
			_runtimeNextButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._projectileSelectorTarget)
		{
			_projectileSelectorTarget = VariantUtils.ConvertTo<ProjectileSelectorTarget>(in value);
			return true;
		}
		if (name == PropertyName._projectileSelectorWindow)
		{
			_projectileSelectorWindow = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._audioSelectorWindow)
		{
			_audioSelectorWindow = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.IsChainMode)
		{
			value = VariantUtils.CreateFrom<bool>(IsChainMode);
			return true;
		}
		if (name == PropertyName._editMode)
		{
			value = VariantUtils.CreateFrom(in _editMode);
			return true;
		}
		if (name == PropertyName._editingChange)
		{
			value = VariantUtils.CreateFrom(in _editingChange);
			return true;
		}
		if (name == PropertyName._editingSingle)
		{
			value = VariantUtils.CreateFrom(in _editingSingle);
			return true;
		}
		if (name == PropertyName._selectedIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedIndex);
			return true;
		}
		if (name == PropertyName._chainPanel)
		{
			value = VariantUtils.CreateFrom(in _chainPanel);
			return true;
		}
		if (name == PropertyName._chainList)
		{
			value = VariantUtils.CreateFrom(in _chainList);
			return true;
		}
		if (name == PropertyName._chainCanvas)
		{
			value = VariantUtils.CreateFrom(in _chainCanvas);
			return true;
		}
		if (name == PropertyName._itemEditorHost)
		{
			value = VariantUtils.CreateFrom(in _itemEditorHost);
			return true;
		}
		if (name == PropertyName._modeBadge)
		{
			value = VariantUtils.CreateFrom(in _modeBadge);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			value = VariantUtils.CreateFrom(in _summaryLabel);
			return true;
		}
		if (name == PropertyName._selectedLabel)
		{
			value = VariantUtils.CreateFrom(in _selectedLabel);
			return true;
		}
		if (name == PropertyName._emptyState)
		{
			value = VariantUtils.CreateFrom(in _emptyState);
			return true;
		}
		if (name == PropertyName._emptyStateLabel)
		{
			value = VariantUtils.CreateFrom(in _emptyStateLabel);
			return true;
		}
		if (name == PropertyName._sourceField)
		{
			value = VariantUtils.CreateFrom(in _sourceField);
			return true;
		}
		if (name == PropertyName._targetField)
		{
			value = VariantUtils.CreateFrom(in _targetField);
			return true;
		}
		if (name == PropertyName._audioField)
		{
			value = VariantUtils.CreateFrom(in _audioField);
			return true;
		}
		if (name == PropertyName._entryPreviewLabel)
		{
			value = VariantUtils.CreateFrom(in _entryPreviewLabel);
			return true;
		}
		if (name == PropertyName._runtimePreviewViewport)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewViewport);
			return true;
		}
		if (name == PropertyName._runtimePreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewRoot);
			return true;
		}
		if (name == PropertyName._runtimePreviewStatus)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewStatus);
			return true;
		}
		if (name == PropertyName._runtimePreviewRunButton)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewRunButton);
			return true;
		}
		if (name == PropertyName._runtimePreviewTimeline)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewTimeline);
			return true;
		}
		if (name == PropertyName._runtimePreviewInput)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewInput);
			return true;
		}
		if (name == PropertyName._runtimeZoomLabel)
		{
			value = VariantUtils.CreateFrom(in _runtimeZoomLabel);
			return true;
		}
		if (name == PropertyName._runtimeSourceProjectile)
		{
			value = VariantUtils.CreateFrom(in _runtimeSourceProjectile);
			return true;
		}
		if (name == PropertyName._runtimeTargetProjectile)
		{
			value = VariantUtils.CreateFrom(in _runtimeTargetProjectile);
			return true;
		}
		if (name == PropertyName._runtimePreviewProgress)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewProgress);
			return true;
		}
		if (name == PropertyName._runtimePreviewRunning)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewRunning);
			return true;
		}
		if (name == PropertyName._runtimePreviewActive)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewActive);
			return true;
		}
		if (name == PropertyName._runtimePreviewRequestVersion)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewRequestVersion);
			return true;
		}
		if (name == PropertyName._runtimePreviewBasePosition)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewBasePosition);
			return true;
		}
		if (name == PropertyName._runtimePreviewPan)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewPan);
			return true;
		}
		if (name == PropertyName._runtimePreviewZoom)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewZoom);
			return true;
		}
		if (name == PropertyName._runtimePreviewDragging)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviewDragging);
			return true;
		}
		if (name == PropertyName._projectileDataPicker)
		{
			value = VariantUtils.CreateFrom(in _projectileDataPicker);
			return true;
		}
		if (name == PropertyName._projectileConfigPicker)
		{
			value = VariantUtils.CreateFrom(in _projectileConfigPicker);
			return true;
		}
		if (name == PropertyName._sourceProjectileKeyEdit)
		{
			value = VariantUtils.CreateFrom(in _sourceProjectileKeyEdit);
			return true;
		}
		if (name == PropertyName._changeAudioEdit)
		{
			value = VariantUtils.CreateFrom(in _changeAudioEdit);
			return true;
		}
		if (name == PropertyName._removeButton)
		{
			value = VariantUtils.CreateFrom(in _removeButton);
			return true;
		}
		if (name == PropertyName._moveUpButton)
		{
			value = VariantUtils.CreateFrom(in _moveUpButton);
			return true;
		}
		if (name == PropertyName._moveDownButton)
		{
			value = VariantUtils.CreateFrom(in _moveDownButton);
			return true;
		}
		if (name == PropertyName._runtimePreviousButton)
		{
			value = VariantUtils.CreateFrom(in _runtimePreviousButton);
			return true;
		}
		if (name == PropertyName._runtimeNextButton)
		{
			value = VariantUtils.CreateFrom(in _runtimeNextButton);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._projectileSelectorTarget)
		{
			value = VariantUtils.CreateFrom(in _projectileSelectorTarget);
			return true;
		}
		if (name == PropertyName._projectileSelectorWindow)
		{
			value = VariantUtils.CreateFrom(in _projectileSelectorWindow);
			return true;
		}
		if (name == PropertyName._audioSelectorWindow)
		{
			value = VariantUtils.CreateFrom(in _audioSelectorWindow);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._editMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingChange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingSingle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._chainPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._chainList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._chainCanvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._itemEditorHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._modeBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._summaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyStateLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sourceField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._entryPreviewLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimePreviewViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimePreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimePreviewStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimePreviewRunButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimePreviewTimeline, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimePreviewInput, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeZoomLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeSourceProjectile, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeTargetProjectile, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._runtimePreviewProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimePreviewRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimePreviewActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimePreviewRequestVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._runtimePreviewBasePosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._runtimePreviewPan, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._runtimePreviewZoom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimePreviewDragging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileDataPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileConfigPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sourceProjectileKeyEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._changeAudioEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._removeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveUpButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveDownButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimePreviousButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeNextButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._projectileSelectorTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileSelectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioSelectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsChainMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editMode, Variant.From(in _editMode));
		info.AddProperty(PropertyName._editingChange, Variant.From(in _editingChange));
		info.AddProperty(PropertyName._editingSingle, Variant.From(in _editingSingle));
		info.AddProperty(PropertyName._selectedIndex, Variant.From(in _selectedIndex));
		info.AddProperty(PropertyName._chainPanel, Variant.From(in _chainPanel));
		info.AddProperty(PropertyName._chainList, Variant.From(in _chainList));
		info.AddProperty(PropertyName._chainCanvas, Variant.From(in _chainCanvas));
		info.AddProperty(PropertyName._itemEditorHost, Variant.From(in _itemEditorHost));
		info.AddProperty(PropertyName._modeBadge, Variant.From(in _modeBadge));
		info.AddProperty(PropertyName._summaryLabel, Variant.From(in _summaryLabel));
		info.AddProperty(PropertyName._selectedLabel, Variant.From(in _selectedLabel));
		info.AddProperty(PropertyName._emptyState, Variant.From(in _emptyState));
		info.AddProperty(PropertyName._emptyStateLabel, Variant.From(in _emptyStateLabel));
		info.AddProperty(PropertyName._sourceField, Variant.From(in _sourceField));
		info.AddProperty(PropertyName._targetField, Variant.From(in _targetField));
		info.AddProperty(PropertyName._audioField, Variant.From(in _audioField));
		info.AddProperty(PropertyName._entryPreviewLabel, Variant.From(in _entryPreviewLabel));
		info.AddProperty(PropertyName._runtimePreviewViewport, Variant.From(in _runtimePreviewViewport));
		info.AddProperty(PropertyName._runtimePreviewRoot, Variant.From(in _runtimePreviewRoot));
		info.AddProperty(PropertyName._runtimePreviewStatus, Variant.From(in _runtimePreviewStatus));
		info.AddProperty(PropertyName._runtimePreviewRunButton, Variant.From(in _runtimePreviewRunButton));
		info.AddProperty(PropertyName._runtimePreviewTimeline, Variant.From(in _runtimePreviewTimeline));
		info.AddProperty(PropertyName._runtimePreviewInput, Variant.From(in _runtimePreviewInput));
		info.AddProperty(PropertyName._runtimeZoomLabel, Variant.From(in _runtimeZoomLabel));
		info.AddProperty(PropertyName._runtimeSourceProjectile, Variant.From(in _runtimeSourceProjectile));
		info.AddProperty(PropertyName._runtimeTargetProjectile, Variant.From(in _runtimeTargetProjectile));
		info.AddProperty(PropertyName._runtimePreviewProgress, Variant.From(in _runtimePreviewProgress));
		info.AddProperty(PropertyName._runtimePreviewRunning, Variant.From(in _runtimePreviewRunning));
		info.AddProperty(PropertyName._runtimePreviewActive, Variant.From(in _runtimePreviewActive));
		info.AddProperty(PropertyName._runtimePreviewRequestVersion, Variant.From(in _runtimePreviewRequestVersion));
		info.AddProperty(PropertyName._runtimePreviewBasePosition, Variant.From(in _runtimePreviewBasePosition));
		info.AddProperty(PropertyName._runtimePreviewPan, Variant.From(in _runtimePreviewPan));
		info.AddProperty(PropertyName._runtimePreviewZoom, Variant.From(in _runtimePreviewZoom));
		info.AddProperty(PropertyName._runtimePreviewDragging, Variant.From(in _runtimePreviewDragging));
		info.AddProperty(PropertyName._projectileDataPicker, Variant.From(in _projectileDataPicker));
		info.AddProperty(PropertyName._projectileConfigPicker, Variant.From(in _projectileConfigPicker));
		info.AddProperty(PropertyName._sourceProjectileKeyEdit, Variant.From(in _sourceProjectileKeyEdit));
		info.AddProperty(PropertyName._changeAudioEdit, Variant.From(in _changeAudioEdit));
		info.AddProperty(PropertyName._removeButton, Variant.From(in _removeButton));
		info.AddProperty(PropertyName._moveUpButton, Variant.From(in _moveUpButton));
		info.AddProperty(PropertyName._moveDownButton, Variant.From(in _moveDownButton));
		info.AddProperty(PropertyName._runtimePreviousButton, Variant.From(in _runtimePreviousButton));
		info.AddProperty(PropertyName._runtimeNextButton, Variant.From(in _runtimeNextButton));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._projectileSelectorTarget, Variant.From(in _projectileSelectorTarget));
		info.AddProperty(PropertyName._projectileSelectorWindow, Variant.From(in _projectileSelectorWindow));
		info.AddProperty(PropertyName._audioSelectorWindow, Variant.From(in _audioSelectorWindow));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editMode, out var value))
		{
			_editMode = value.As<ProjectileChangeEditMode>();
		}
		if (info.TryGetProperty(PropertyName._editingChange, out var value2))
		{
			_editingChange = value2.As<ChangeProjectileConfig>();
		}
		if (info.TryGetProperty(PropertyName._editingSingle, out var value3))
		{
			_editingSingle = value3.As<ChangeProjectileSingleConfig>();
		}
		if (info.TryGetProperty(PropertyName._selectedIndex, out var value4))
		{
			_selectedIndex = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._chainPanel, out var value5))
		{
			_chainPanel = value5.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._chainList, out var value6))
		{
			_chainList = value6.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._chainCanvas, out var value7))
		{
			_chainCanvas = value7.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._itemEditorHost, out var value8))
		{
			_itemEditorHost = value8.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._modeBadge, out var value9))
		{
			_modeBadge = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._summaryLabel, out var value10))
		{
			_summaryLabel = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._selectedLabel, out var value11))
		{
			_selectedLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._emptyState, out var value12))
		{
			_emptyState = value12.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._emptyStateLabel, out var value13))
		{
			_emptyStateLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._sourceField, out var value14))
		{
			_sourceField = value14.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._targetField, out var value15))
		{
			_targetField = value15.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._audioField, out var value16))
		{
			_audioField = value16.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._entryPreviewLabel, out var value17))
		{
			_entryPreviewLabel = value17.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewViewport, out var value18))
		{
			_runtimePreviewViewport = value18.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewRoot, out var value19))
		{
			_runtimePreviewRoot = value19.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewStatus, out var value20))
		{
			_runtimePreviewStatus = value20.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewRunButton, out var value21))
		{
			_runtimePreviewRunButton = value21.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewTimeline, out var value22))
		{
			_runtimePreviewTimeline = value22.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewInput, out var value23))
		{
			_runtimePreviewInput = value23.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._runtimeZoomLabel, out var value24))
		{
			_runtimeZoomLabel = value24.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._runtimeSourceProjectile, out var value25))
		{
			_runtimeSourceProjectile = value25.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._runtimeTargetProjectile, out var value26))
		{
			_runtimeTargetProjectile = value26.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewProgress, out var value27))
		{
			_runtimePreviewProgress = value27.As<double>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewRunning, out var value28))
		{
			_runtimePreviewRunning = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewActive, out var value29))
		{
			_runtimePreviewActive = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewRequestVersion, out var value30))
		{
			_runtimePreviewRequestVersion = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewBasePosition, out var value31))
		{
			_runtimePreviewBasePosition = value31.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewPan, out var value32))
		{
			_runtimePreviewPan = value32.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewZoom, out var value33))
		{
			_runtimePreviewZoom = value33.As<float>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviewDragging, out var value34))
		{
			_runtimePreviewDragging = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._projectileDataPicker, out var value35))
		{
			_projectileDataPicker = value35.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._projectileConfigPicker, out var value36))
		{
			_projectileConfigPicker = value36.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._sourceProjectileKeyEdit, out var value37))
		{
			_sourceProjectileKeyEdit = value37.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._changeAudioEdit, out var value38))
		{
			_changeAudioEdit = value38.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._removeButton, out var value39))
		{
			_removeButton = value39.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveUpButton, out var value40))
		{
			_moveUpButton = value40.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveDownButton, out var value41))
		{
			_moveDownButton = value41.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._runtimePreviousButton, out var value42))
		{
			_runtimePreviousButton = value42.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._runtimeNextButton, out var value43))
		{
			_runtimeNextButton = value43.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value44))
		{
			_updatingControls = value44.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._projectileSelectorTarget, out var value45))
		{
			_projectileSelectorTarget = value45.As<ProjectileSelectorTarget>();
		}
		if (info.TryGetProperty(PropertyName._projectileSelectorWindow, out var value46))
		{
			_projectileSelectorWindow = value46.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._audioSelectorWindow, out var value47))
		{
			_audioSelectorWindow = value47.As<XWGameplayResourcePickerWindow>();
		}
	}
}
