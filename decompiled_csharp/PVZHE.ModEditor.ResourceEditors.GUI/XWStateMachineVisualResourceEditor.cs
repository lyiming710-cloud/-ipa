using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Blueprint;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/StateMachine/XWStateMachineVisualResourceEditor.cs")]
public class XWStateMachineVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConfigureDedicatedGraphInputHost = "ConfigureDedicatedGraphInputHost";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public new static readonly StringName UndoLastChange = "UndoLastChange";

		public new static readonly StringName RedoLastChange = "RedoLastChange";

		public static readonly StringName BuildWorkbenchTabs = "BuildWorkbenchTabs";

		public static readonly StringName OnExtensionPropertyEdited = "OnExtensionPropertyEdited";

		public static readonly StringName OnWorkbenchTabChanged = "OnWorkbenchTabChanged";

		public static readonly StringName UpdateGraphWorkbenchActive = "UpdateGraphWorkbenchActive";

		public static readonly StringName OnHistoryChanged = "OnHistoryChanged";

		public static readonly StringName RefreshAfterHistoryChange = "RefreshAfterHistoryChange";

		public static readonly StringName OnStateMachineDefinitionChanged = "OnStateMachineDefinitionChanged";

		public static readonly StringName OnStateMachineLayoutChanged = "OnStateMachineLayoutChanged";

		public static readonly StringName DisconnectSurface = "DisconnectSurface";

		public new static readonly StringName OnCurrentResourceSaved = "OnCurrentResourceSaved";

		public static readonly StringName OnGuardEditRequested = "OnGuardEditRequested";

		public static readonly StringName OnResourcePickerRequested = "OnResourcePickerRequested";

		public static readonly StringName OnCreateDefinitionRequested = "OnCreateDefinitionRequested";

		public static readonly StringName OnOpenDefinitionRequested = "OnOpenDefinitionRequested";

		public static readonly StringName EnsureResourcePicker = "EnsureResourcePicker";

		public static readonly StringName OpenDefinitionResource = "OpenDefinitionResource";

		public static readonly StringName ApplyPickedResource = "ApplyPickedResource";

		public static readonly StringName IsOwnerBoundToCurrentDefinition = "IsOwnerBoundToCurrentDefinition";

		public static readonly StringName LocalizeKnownResourcePath = "LocalizeKnownResourcePath";

		public static readonly StringName LoadStateMachineLayout = "LoadStateMachineLayout";

		public static readonly StringName SaveStateMachineLayout = "SaveStateMachineLayout";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName GraphSurface = "GraphSurface";

		public static readonly StringName ExtensionPropertySurface = "ExtensionPropertySurface";

		public static readonly StringName WorkbenchTabs = "WorkbenchTabs";

		public static readonly StringName ActiveResourcePicker = "ActiveResourcePicker";

		public static readonly StringName UsesDedicatedGraphInputHost = "UsesDedicatedGraphInputHost";

		public static readonly StringName _surface = "_surface";

		public static readonly StringName _undoAdapter = "_undoAdapter";

		public static readonly StringName _undoRedoManager = "_undoRedoManager";

		public static readonly StringName _editingDefinition = "_editingDefinition";

		public static readonly StringName _editingLayout = "_editingLayout";

		public static readonly StringName _editingResourcePath = "_editingResourcePath";

		public static readonly StringName _workbenchTabs = "_workbenchTabs";

		public static readonly StringName _extensionPropertySurface = "_extensionPropertySurface";

		public static readonly StringName _resourcePicker = "_resourcePicker";

		public static readonly StringName _surfaceBindingGeneration = "_surfaceBindingGeneration";

		public static readonly StringName _editorWorkbenchVisible = "_editorWorkbenchVisible";

		public static readonly StringName _historyRefreshQueued = "_historyRefreshQueued";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string SurfaceScenePath = "res://addons/godot_state_charts/VisualEditor/StateMachineGraphEditorSurface.tscn";

	private static PackedScene _surfaceScene;

	private StateMachineGraphEditorSurface _surface;

	private StateMachineGraphController _controller;

	private XWStateMachineUndoAdapter _undoAdapter;

	private XWUndoRedoManager _undoRedoManager;

	private StateMachineDefinition _editingDefinition;

	private StateMachineLayout _editingLayout;

	private string _editingResourcePath = string.Empty;

	private TabContainer _workbenchTabs;

	private XWDirectPropertySurface _extensionPropertySurface;

	private readonly StateMachineLayoutStore _layoutStore = new StateMachineLayoutStore();

	private XWGameplayResourcePickerWindow _resourcePicker;

	private int _surfaceBindingGeneration;

	private bool _editorWorkbenchVisible = true;

	private bool _historyRefreshQueued;

	private static readonly IReadOnlyCollection<string> GraphOwnedDefinitionProperties = new string[7] { "SchemaVersion", "DefinitionId", "BaseDefinition", "RootStateId", "States", "Transitions", "Aliases" };

	public StateMachineGraphEditorSurface GraphSurface => _surface;

	public XWDirectPropertySurface ExtensionPropertySurface => _extensionPropertySurface;

	public TabContainer WorkbenchTabs => _workbenchTabs;

	public XWGameplayResourcePickerWindow ActiveResourcePicker => _resourcePicker;

	public bool UsesDedicatedGraphInputHost
	{
		get
		{
			ScrollContainer nodeOrNull = GetNodeOrNull<ScrollContainer>("%MainPanel");
			if (nodeOrNull != null && nodeOrNull.HorizontalScrollMode == ScrollContainer.ScrollMode.Disabled)
			{
				return nodeOrNull.VerticalScrollMode == ScrollContainer.ScrollMode.Disabled;
			}
			return false;
		}
	}

	public override void _ExitTree()
	{
		DisconnectSurface();
		_resourcePicker?.Dismiss();
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisconnectSurface();
		if (CanvasGrid == null)
		{
			return;
		}
		ConfigureDedicatedGraphInputHost();
		_editingDefinition = CurrentResource as StateMachineDefinition;
		ref string editingResourcePath = ref _editingResourcePath;
		string text;
		if (_editingDefinition == null)
		{
			text = string.Empty;
		}
		else
		{
			text = (string.IsNullOrWhiteSpace(CurrentResourcePath) ? _editingDefinition.ResourcePath : CurrentResourcePath);
		}
		editingResourcePath = text;
		_editingLayout = ((_editingDefinition == null) ? new StateMachineLayout() : (LoadStateMachineLayout(_editingDefinition) ?? new StateMachineLayout()));
		_controller = new StateMachineGraphController(new GuidStateMachineStableIdProvider());
		_undoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager() ?? new XWUndoRedoManager();
		_undoAdapter = new XWStateMachineUndoAdapter(_undoRedoManager);
		_undoRedoManager.HistoryChanged += OnHistoryChanged;
		_controller.BindUndoAdapter(_undoAdapter);
		if (_surfaceScene == null)
		{
			_surfaceScene = ResourceLoader.Load<PackedScene>("res://addons/godot_state_charts/VisualEditor/StateMachineGraphEditorSurface.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		_surface = _surfaceScene?.Instantiate<StateMachineGraphEditorSurface>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(_surface))
		{
			GD.PushError("ModEditor state-machine graph surface could not be loaded: res://addons/godot_state_charts/VisualEditor/StateMachineGraphEditorSurface.tscn");
			return;
		}
		CanvasGrid.Columns = 1;
		_surface.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		_surface.SizeFlagsVertical = SizeFlags.ExpandFill;
		_surface.CustomMinimumSize = new Vector2(0f, 480f);
		BuildWorkbenchTabs();
		_surface.Bind(_controller, _undoAdapter);
		if (_editingDefinition != null)
		{
			_surface.LoadDefinition(_editingDefinition, _editingLayout);
		}
		UpdateGraphWorkbenchActive();
		_surface.DefinitionChanged += OnStateMachineDefinitionChanged;
		_surface.LayoutChanged += OnStateMachineLayoutChanged;
		_surface.ResourcePickerRequested += OnResourcePickerRequested;
		_surface.GuardEditRequested += OnGuardEditRequested;
		_surface.CallbackSourceOpenRequested += OnCallbackSourceOpenRequested;
		_surface.CreateDefinitionRequested += OnCreateDefinitionRequested;
		_surface.OpenDefinitionRequested += OnOpenDefinitionRequested;
	}

	private void ConfigureDedicatedGraphInputHost()
	{
		ScrollContainer nodeOrNull = GetNodeOrNull<ScrollContainer>("%MainPanel");
		if (nodeOrNull != null)
		{
			nodeOrNull.HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled;
			nodeOrNull.VerticalScrollMode = ScrollContainer.ScrollMode.Disabled;
		}
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		base.OnVisualEditorVisibilityChanged(visible);
		_editorWorkbenchVisible = visible;
		UpdateGraphWorkbenchActive();
		if (!visible)
		{
			_resourcePicker?.Dismiss();
		}
	}

	protected override void UndoLastChange()
	{
		_surface?.DetailsPanel?.FlushPendingNumericCommit();
		_surface?.FlushViewportState();
		XWStateMachineUndoAdapter undoAdapter = _undoAdapter;
		if (undoAdapter == null || !undoAdapter.CanUndo || _controller == null)
		{
			XWEditorInterface.Instance?.ShowToast("没有可撤销的状态机修改。");
		}
		else
		{
			_controller.Undo();
		}
	}

	protected override void RedoLastChange()
	{
		_surface?.DetailsPanel?.FlushPendingNumericCommit();
		_surface?.FlushViewportState();
		XWStateMachineUndoAdapter undoAdapter = _undoAdapter;
		if (undoAdapter == null || !undoAdapter.CanRedo || _controller == null)
		{
			XWEditorInterface.Instance?.ShowToast("没有可重做的状态机修改。");
		}
		else
		{
			_controller.Redo();
		}
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

	private void BuildWorkbenchTabs()
	{
		_workbenchTabs = new TabContainer
		{
			Name = "StateMachineWorkbenchTabs",
			CustomMinimumSize = new Vector2(0f, 480f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		CanvasGrid.AddChild(_workbenchTabs, forceReadableName: false, InternalMode.Disabled);
		MarginContainer marginContainer = new MarginContainer
		{
			Name = "状态图",
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		marginContainer.AddChild(_surface, forceReadableName: false, InternalMode.Disabled);
		_workbenchTabs.AddChild(marginContainer, forceReadableName: false, InternalMode.Disabled);
		ScrollContainer scrollContainer = new ScrollContainer
		{
			Name = "资源与CSharp扩展",
			HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
			VerticalScrollMode = ScrollContainer.ScrollMode.Auto,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		_extensionPropertySurface = new XWDirectPropertySurface
		{
			Name = "StateMachineExtensionPropertySurface",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_extensionPropertySurface.PropertyEdited += OnExtensionPropertyEdited;
		scrollContainer.AddChild(_extensionPropertySurface, forceReadableName: false, InternalMode.Disabled);
		_workbenchTabs.AddChild(scrollContainer, forceReadableName: false, InternalMode.Disabled);
		_workbenchTabs.TabChanged += OnWorkbenchTabChanged;
		_extensionPropertySurface.BindObject(_editingDefinition, _undoRedoManager, GraphOwnedDefinitionProperties, _undoAdapter?.HistoryScope ?? (-1));
	}

	private void OnExtensionPropertyEdited(GodotObject obj, StringName property, StringName field, Variant value)
	{
		if (obj == _editingDefinition)
		{
			NotifyCurrentResourceEdited();
		}
	}

	private void OnWorkbenchTabChanged(long tab)
	{
		if (tab != 0L)
		{
			_surface?.DetailsPanel?.FlushPendingNumericCommit();
			_surface?.FlushViewportState();
		}
		UpdateGraphWorkbenchActive();
	}

	private void UpdateGraphWorkbenchActive()
	{
		bool flag = _workbenchTabs == null || _workbenchTabs.CurrentTab == 0;
		_surface?.SetWorkbenchActive(_editorWorkbenchVisible & flag);
	}

	private void OnHistoryChanged(int historyScope)
	{
		if (_undoAdapter != null && historyScope == _undoAdapter.HistoryScope && !_historyRefreshQueued)
		{
			_historyRefreshQueued = true;
			CallDeferred("RefreshAfterHistoryChange", _surfaceBindingGeneration);
		}
	}

	private void RefreshAfterHistoryChange(int bindingGeneration)
	{
		if (bindingGeneration == _surfaceBindingGeneration)
		{
			_historyRefreshQueued = false;
			if (GodotObject.IsInstanceValid(_editingDefinition))
			{
				_extensionPropertySurface?.RefreshValues();
				NotifyCurrentResourceEdited();
			}
		}
	}

	private void OnStateMachineDefinitionChanged()
	{
		NotifyCurrentResourceEdited();
	}

	protected override bool CanPersistResource(Resource resource, out string failureReason)
	{
		if (!base.CanPersistResource(resource, out failureReason))
		{
			return false;
		}
		if (!(resource is StateMachineDefinition stateMachineDefinition))
		{
			return true;
		}
		StateMachineValidationResult validation;
		if (stateMachineDefinition == _editingDefinition && _surface?.GraphController != null)
		{
			if (!_surface.GraphController.HasCompositionError)
			{
				return true;
			}
			validation = _surface.GraphController.Diagnostics;
		}
		else if (StateMachineDefinitionComposer.TryValidateInheritanceChain(stateMachineDefinition, out validation))
		{
			return true;
		}
		StateMachineDiagnostic stateMachineDiagnostic = ((validation != null && validation.Diagnostics?.Count > 0) ? validation.Diagnostics[0] : null);
		failureReason = ((stateMachineDiagnostic == null) ? "继承合成失败，请更换或清除基定义后再保存。" : $"[{stateMachineDiagnostic.Code}] {stateMachineDiagnostic.Message} 请更换或清除基定义后再保存。");
		return false;
	}

	private void OnStateMachineLayoutChanged()
	{
		SaveStateMachineLayout(_editingResourcePath, _editingLayout);
	}

	private void DisconnectSurface()
	{
		_surfaceBindingGeneration++;
		_resourcePicker?.Dismiss();
		if (_surface != null)
		{
			_surface.FlushViewportState();
			_surface.SetWorkbenchActive(active: false);
			_surface.DefinitionChanged -= OnStateMachineDefinitionChanged;
			_surface.LayoutChanged -= OnStateMachineLayoutChanged;
			_surface.ResourcePickerRequested -= OnResourcePickerRequested;
			_surface.GuardEditRequested -= OnGuardEditRequested;
			_surface.CallbackSourceOpenRequested -= OnCallbackSourceOpenRequested;
			_surface.CreateDefinitionRequested -= OnCreateDefinitionRequested;
			_surface.OpenDefinitionRequested -= OnOpenDefinitionRequested;
		}
		if (_extensionPropertySurface != null)
		{
			_extensionPropertySurface.PropertyEdited -= OnExtensionPropertyEdited;
		}
		if (_workbenchTabs != null)
		{
			_workbenchTabs.TabChanged -= OnWorkbenchTabChanged;
		}
		if (_undoRedoManager != null)
		{
			_undoRedoManager.HistoryChanged -= OnHistoryChanged;
		}
		_controller?.Dispose();
		_undoAdapter?.Release();
		_surface = null;
		_extensionPropertySurface = null;
		_workbenchTabs = null;
		_controller = null;
		_undoAdapter = null;
		_undoRedoManager = null;
		_editingDefinition = null;
		_editingLayout = null;
		_editingResourcePath = string.Empty;
		_editorWorkbenchVisible = true;
		_historyRefreshQueued = false;
	}

	protected override void OnCurrentResourceSaved()
	{
		base.OnCurrentResourceSaved();
		if (_editingDefinition != null && CurrentResource == _editingDefinition)
		{
			_editingResourcePath = (string.IsNullOrWhiteSpace(CurrentResourcePath) ? _editingDefinition.ResourcePath : CurrentResourcePath);
			SaveStateMachineLayout(_editingResourcePath, _editingLayout);
		}
	}

	private void OnGuardEditRequested(Resource child, Resource owner, string property, int index)
	{
		if (GodotObject.IsInstanceValid(child) && GodotObject.IsInstanceValid(owner))
		{
			string text = (string.IsNullOrWhiteSpace(CurrentResourcePath) ? owner.ResourcePath : CurrentResourcePath);
			XWResourceEditContext context = XWResourceEditContext.ForProperty(child, owner, child.ResourcePath, text, property, index, "state_machine", CurrentEditContext?.IsBuiltInSource ?? XWResourceEditContext.IsBuiltInPath(text), "", _editingDefinition, text);
			XWEditorInterface.Instance?.EditResource(child, context);
		}
	}

	private void OnCallbackSourceOpenRequested(StateMachineCallbackCatalogEntry entry, StateMachineCallbackPhase phase)
	{
		OpenCallbackSource(entry, phase);
	}

	internal static void OpenCallbackSource(StateMachineCallbackCatalogEntry entry, StateMachineCallbackPhase phase)
	{
		if (entry.SourceKind != StateMachineCallbackSourceKind.Blueprint)
		{
			XWEditorInterface.Instance?.ShowToast("该动作来自 C#，请在 C# 脚本编辑器中打开源码。");
			return;
		}
		string projectRoot = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? string.Empty;
		if (!TryResolveBlueprintSourcePath(entry.SourcePath, projectRoot, out var resolvedPath, out var error))
		{
			XWEditorInterface.Instance?.ShowToast("无法打开蓝图动作：" + error, 2);
			return;
		}
		XWBPScript xWBPScript;
		try
		{
			xWBPScript = ResourceLoader.Load<XWBPScript>(resolvedPath, null, ResourceLoader.CacheMode.Reuse);
		}
		catch (Exception ex)
		{
			XWEditorInterface.Instance?.ShowToast("蓝图加载失败：" + ex.GetBaseException().Message, 2);
			return;
		}
		if (!GodotObject.IsInstanceValid(xWBPScript) || !(XWEditorInterface.Instance?.GetBlueprintEditor() is XWBPEditor xWBPEditor))
		{
			XWEditorInterface.Instance?.ShowToast("蓝图编辑器尚未就绪。", 2);
			return;
		}
		xWBPEditor.Init(xWBPScript);
		if (StateMachineCallbackKey.TryParse(entry.Key, out var _, out var localKey) && xWBPEditor.BpScriptData != null)
		{
			foreach (XWBPFunctionData value in xWBPEditor.BpScriptData.Functions.Values)
			{
				if (value != null && value.StateMachineCallbackEnabled && value.StateMachineCallbackPhase == phase && string.Equals(value.StateMachineCallbackLocalKey?.Trim(), localKey, StringComparison.Ordinal))
				{
					xWBPEditor.OpenGraph(value);
					break;
				}
			}
		}
		XWEditorInterface.Instance.FocusPanel("bp_editor");
	}

	private static bool TryResolveBlueprintSourcePath(string sourcePath, string projectRoot, out string resolvedPath, out string error)
	{
		resolvedPath = string.Empty;
		error = string.Empty;
		string text = (sourcePath ?? string.Empty).Replace('\\', '/').Trim();
		if (string.IsNullOrWhiteSpace(text))
		{
			error = "动作没有记录蓝图路径。请重新生成并编译蓝图。";
			return false;
		}
		if (text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) && ResourceLoader.Exists(text))
		{
			resolvedPath = text;
			return true;
		}
		try
		{
			string text2 = (Path.IsPathRooted(text) ? Path.GetFullPath(text) : Path.GetFullPath(Path.Combine(projectRoot ?? string.Empty, text)));
			if (File.Exists(text2))
			{
				resolvedPath = text2.Replace('\\', '/');
				return true;
			}
		}
		catch (Exception ex)
		{
			error = ex.GetBaseException().Message;
			return false;
		}
		error = "找不到“" + text + "”。";
		return false;
	}

	private void OnResourcePickerRequested(Resource owner, string property)
	{
		if (owner == null || string.IsNullOrWhiteSpace(property))
		{
			return;
		}
		EnsureResourcePicker();
		if (!GodotObject.IsInstanceValid(_resourcePicker))
		{
			return;
		}
		Resource resource = owner.Get(property).As<Resource>();
		bool flag = property == "BaseDefinition";
		int generation = _surfaceBindingGeneration;
		_resourcePicker.OpenResourceLibrary("StateMachine", "状态机资源", resource?.ResourcePath ?? string.Empty, XWFileSystem.GetSingleton()?.ProjectFolderPath ?? string.Empty, (!flag) ? new string[1] { "Resource" } : new string[1] { "StateMachineDefinition" }, new string[2] { "Resources/StateMachines", "/StateMachines/" }, "res://addons/ModEditor/Icons/GraphEdit.svg", (XWGameplayResourceChoice choice) =>
		{
			if (generation == _surfaceBindingGeneration && IsOwnerBoundToCurrentDefinition(owner))
			{
				ApplyPickedResource(owner, property, choice?.ResourcePath);
			}
		});
	}

	private void OnCreateDefinitionRequested()
	{
		string text = "root." + Guid.NewGuid().ToString("N");
		string text2 = "state." + Guid.NewGuid().ToString("N");
		XWResourceEditorRegistry.TryOpen(new StateMachineDefinition
		{
			DefinitionId = Guid.NewGuid().ToString("N"),
			ResourceName = "新状态机",
			RootStateId = text,
			States = 
			{
				new StateMachineStateDefinition
				{
					StableId = text,
					DisplayName = "状态机入口",
					Kind = StateMachineStateKind.Compound,
					InitialChildId = text2
				},
				new StateMachineStateDefinition
				{
					StableId = text2,
					DisplayName = "初始状态",
					Kind = StateMachineStateKind.Atomic,
					ParentId = text
				}
			}
		}, string.Empty);
	}

	private void OnOpenDefinitionRequested()
	{
		EnsureResourcePicker();
		_resourcePicker?.OpenResourceLibrary("StateMachine", "状态机资源", string.Empty, XWFileSystem.GetSingleton()?.ProjectFolderPath ?? string.Empty, new string[1] { "StateMachineDefinition" }, new string[2] { "Resources/StateMachines", "/StateMachines/" }, "res://addons/ModEditor/Icons/GraphEdit.svg", (XWGameplayResourceChoice choice) =>
		{
			OpenDefinitionResource(choice?.ResourcePath);
		});
	}

	private void EnsureResourcePicker()
	{
		if (!GodotObject.IsInstanceValid(_resourcePicker))
		{
			_resourcePicker = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_resourcePicker))
			{
				AddChild(_resourcePicker, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private static void OpenDefinitionResource(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			StateMachineDefinition stateMachineDefinition = LoadPickedResource<StateMachineDefinition>(path);
			if (stateMachineDefinition != null)
			{
				XWResourceEditorRegistry.TryOpen(stateMachineDefinition, path);
			}
		}
	}

	private void ApplyPickedResource(Resource owner, string property, string path)
	{
		if (owner != null && !string.IsNullOrWhiteSpace(property) && !string.IsNullOrWhiteSpace(path))
		{
			Resource resource = LoadPickedResource<Resource>(path);
			if (resource != null && (!(property == "BaseDefinition") || resource is StateMachineDefinition))
			{
				_surface?.ApplyPickedResource(owner, property, resource);
			}
		}
	}

	private bool IsOwnerBoundToCurrentDefinition(Resource owner)
	{
		if (!GodotObject.IsInstanceValid(owner) || !GodotObject.IsInstanceValid(_editingDefinition))
		{
			return false;
		}
		if (owner == _editingDefinition)
		{
			return true;
		}
		if (owner is StateMachineStateDefinition stateMachineStateDefinition && _editingDefinition.States != null)
		{
			foreach (StateMachineStateDefinition state in _editingDefinition.States)
			{
				if (state == stateMachineStateDefinition || (!string.IsNullOrWhiteSpace(stateMachineStateDefinition.StableId) && string.Equals(state?.StableId, stateMachineStateDefinition.StableId, StringComparison.Ordinal)))
				{
					return true;
				}
			}
		}
		if (owner is StateMachineTransitionDefinition stateMachineTransitionDefinition && _editingDefinition.Transitions != null)
		{
			foreach (StateMachineTransitionDefinition transition in _editingDefinition.Transitions)
			{
				if (transition == stateMachineTransitionDefinition || (!string.IsNullOrWhiteSpace(stateMachineTransitionDefinition.StableId) && string.Equals(transition?.StableId, stateMachineTransitionDefinition.StableId, StringComparison.Ordinal)))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static T LoadPickedResource<T>(string path) where T : Resource
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		string text = path.Replace('\\', '/').Trim();
		string text2 = LocalizeKnownResourcePath(text);
		T val = ResourceLoader.Load<T>(text2, "", ResourceLoader.CacheMode.Ignore);
		if (!GodotObject.IsInstanceValid(val) && !string.Equals(text2, text, StringComparison.OrdinalIgnoreCase))
		{
			val = ResourceLoader.Load<T>(text, "", ResourceLoader.CacheMode.Ignore);
		}
		return val;
	}

	private static string LocalizeKnownResourcePath(string path)
	{
		if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			return path;
		}
		try
		{
			string fullPath = Path.GetFullPath(path);
			string[] array = new string[2] { "user://", "res://" };
			foreach (string text in array)
			{
				string text2 = Path.GetFullPath(ProjectSettings.GlobalizePath(text)).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
				if (fullPath.Equals(text2, StringComparison.OrdinalIgnoreCase) || fullPath.StartsWith(text2 + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) || fullPath.StartsWith(text2 + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
				{
					string text3 = Path.GetRelativePath(text2, fullPath).Replace('\\', '/');
					return text + text3;
				}
			}
		}
		catch (Exception ex) when ((ex is ArgumentException || ex is NotSupportedException || ex is PathTooLongException) ? true : false)
		{
		}
		return path;
	}

	private StateMachineLayout LoadStateMachineLayout(StateMachineDefinition definition)
	{
		string text = ((!string.IsNullOrWhiteSpace(CurrentResourcePath)) ? CurrentResourcePath : definition?.ResourcePath);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return _layoutStore.Load(text);
		}
		return null;
	}

	private void SaveStateMachineLayout(string resourcePath, StateMachineLayout layout)
	{
		if (layout != null && !string.IsNullOrWhiteSpace(resourcePath))
		{
			Error error = _layoutStore.Save(resourcePath, layout);
			if (error != Error.Ok)
			{
				GD.PushWarning($"State-machine layout save failed: {resourcePath} ({error})");
				XWEditorInterface.Instance?.ShowToast($"状态机画布布局保存失败：{error}");
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(26)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureDedicatedGraphInputHost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UndoLastChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RedoLastChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildWorkbenchTabs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnExtensionPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.OnWorkbenchTabChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "tab", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateGraphWorkbenchActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnHistoryChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyScope", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAfterHistoryChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "bindingGeneration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnStateMachineDefinitionChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnStateMachineLayoutChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCurrentResourceSaved, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGuardEditRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnResourcePickerRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCreateDefinitionRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnOpenDefinitionRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureResourcePicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenDefinitionResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPickedResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsOwnerBoundToCurrentDefinition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.LocalizeKnownResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadStateMachineLayout, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SaveStateMachineLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "layout", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.ConfigureDedicatedGraphInputHost && args.Count == 0)
		{
			ConfigureDedicatedGraphInputHost();
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged && args.Count == 1)
		{
			OnVisualEditorVisibilityChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UndoLastChange && args.Count == 0)
		{
			UndoLastChange();
			ret = default;
			return true;
		}
		if (method == MethodName.RedoLastChange && args.Count == 0)
		{
			RedoLastChange();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildWorkbenchTabs && args.Count == 0)
		{
			BuildWorkbenchTabs();
			ret = default;
			return true;
		}
		if (method == MethodName.OnExtensionPropertyEdited && args.Count == 4)
		{
			OnExtensionPropertyEdited(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnWorkbenchTabChanged && args.Count == 1)
		{
			OnWorkbenchTabChanged(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateGraphWorkbenchActive && args.Count == 0)
		{
			UpdateGraphWorkbenchActive();
			ret = default;
			return true;
		}
		if (method == MethodName.OnHistoryChanged && args.Count == 1)
		{
			OnHistoryChanged(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAfterHistoryChange && args.Count == 1)
		{
			RefreshAfterHistoryChange(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnStateMachineDefinitionChanged && args.Count == 0)
		{
			OnStateMachineDefinitionChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnStateMachineLayoutChanged && args.Count == 0)
		{
			OnStateMachineLayoutChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectSurface && args.Count == 0)
		{
			DisconnectSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCurrentResourceSaved && args.Count == 0)
		{
			OnCurrentResourceSaved();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGuardEditRequested && args.Count == 4)
		{
			OnGuardEditRequested(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnResourcePickerRequested && args.Count == 2)
		{
			OnResourcePickerRequested(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCreateDefinitionRequested && args.Count == 0)
		{
			OnCreateDefinitionRequested();
			ret = default;
			return true;
		}
		if (method == MethodName.OnOpenDefinitionRequested && args.Count == 0)
		{
			OnOpenDefinitionRequested();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureResourcePicker && args.Count == 0)
		{
			EnsureResourcePicker();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenDefinitionResource && args.Count == 1)
		{
			OpenDefinitionResource(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPickedResource && args.Count == 3)
		{
			ApplyPickedResource(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsOwnerBoundToCurrentDefinition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsOwnerBoundToCurrentDefinition(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.LocalizeKnownResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(LocalizeKnownResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadStateMachineLayout && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineLayout>(LoadStateMachineLayout(VariantUtils.ConvertTo<StateMachineDefinition>(in args[0])));
			return true;
		}
		if (method == MethodName.SaveStateMachineLayout && args.Count == 2)
		{
			SaveStateMachineLayout(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StateMachineLayout>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.OpenDefinitionResource && args.Count == 1)
		{
			OpenDefinitionResource(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LocalizeKnownResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(LocalizeKnownResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.ConfigureDedicatedGraphInputHost)
		{
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.UndoLastChange)
		{
			return true;
		}
		if (method == MethodName.RedoLastChange)
		{
			return true;
		}
		if (method == MethodName.BuildWorkbenchTabs)
		{
			return true;
		}
		if (method == MethodName.OnExtensionPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.OnWorkbenchTabChanged)
		{
			return true;
		}
		if (method == MethodName.UpdateGraphWorkbenchActive)
		{
			return true;
		}
		if (method == MethodName.OnHistoryChanged)
		{
			return true;
		}
		if (method == MethodName.RefreshAfterHistoryChange)
		{
			return true;
		}
		if (method == MethodName.OnStateMachineDefinitionChanged)
		{
			return true;
		}
		if (method == MethodName.OnStateMachineLayoutChanged)
		{
			return true;
		}
		if (method == MethodName.DisconnectSurface)
		{
			return true;
		}
		if (method == MethodName.OnCurrentResourceSaved)
		{
			return true;
		}
		if (method == MethodName.OnGuardEditRequested)
		{
			return true;
		}
		if (method == MethodName.OnResourcePickerRequested)
		{
			return true;
		}
		if (method == MethodName.OnCreateDefinitionRequested)
		{
			return true;
		}
		if (method == MethodName.OnOpenDefinitionRequested)
		{
			return true;
		}
		if (method == MethodName.EnsureResourcePicker)
		{
			return true;
		}
		if (method == MethodName.OpenDefinitionResource)
		{
			return true;
		}
		if (method == MethodName.ApplyPickedResource)
		{
			return true;
		}
		if (method == MethodName.IsOwnerBoundToCurrentDefinition)
		{
			return true;
		}
		if (method == MethodName.LocalizeKnownResourcePath)
		{
			return true;
		}
		if (method == MethodName.LoadStateMachineLayout)
		{
			return true;
		}
		if (method == MethodName.SaveStateMachineLayout)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._surface)
		{
			_surface = VariantUtils.ConvertTo<StateMachineGraphEditorSurface>(in value);
			return true;
		}
		if (name == PropertyName._undoAdapter)
		{
			_undoAdapter = VariantUtils.ConvertTo<XWStateMachineUndoAdapter>(in value);
			return true;
		}
		if (name == PropertyName._undoRedoManager)
		{
			_undoRedoManager = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName._editingDefinition)
		{
			_editingDefinition = VariantUtils.ConvertTo<StateMachineDefinition>(in value);
			return true;
		}
		if (name == PropertyName._editingLayout)
		{
			_editingLayout = VariantUtils.ConvertTo<StateMachineLayout>(in value);
			return true;
		}
		if (name == PropertyName._editingResourcePath)
		{
			_editingResourcePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._workbenchTabs)
		{
			_workbenchTabs = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._extensionPropertySurface)
		{
			_extensionPropertySurface = VariantUtils.ConvertTo<XWDirectPropertySurface>(in value);
			return true;
		}
		if (name == PropertyName._resourcePicker)
		{
			_resourcePicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._surfaceBindingGeneration)
		{
			_surfaceBindingGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._editorWorkbenchVisible)
		{
			_editorWorkbenchVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._historyRefreshQueued)
		{
			_historyRefreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.GraphSurface)
		{
			value = VariantUtils.CreateFrom<StateMachineGraphEditorSurface>(GraphSurface);
			return true;
		}
		if (name == PropertyName.ExtensionPropertySurface)
		{
			value = VariantUtils.CreateFrom<XWDirectPropertySurface>(ExtensionPropertySurface);
			return true;
		}
		if (name == PropertyName.WorkbenchTabs)
		{
			value = VariantUtils.CreateFrom<TabContainer>(WorkbenchTabs);
			return true;
		}
		if (name == PropertyName.ActiveResourcePicker)
		{
			value = VariantUtils.CreateFrom<XWGameplayResourcePickerWindow>(ActiveResourcePicker);
			return true;
		}
		if (name == PropertyName.UsesDedicatedGraphInputHost)
		{
			value = VariantUtils.CreateFrom<bool>(UsesDedicatedGraphInputHost);
			return true;
		}
		if (name == PropertyName._surface)
		{
			value = VariantUtils.CreateFrom(in _surface);
			return true;
		}
		if (name == PropertyName._undoAdapter)
		{
			value = VariantUtils.CreateFrom(in _undoAdapter);
			return true;
		}
		if (name == PropertyName._undoRedoManager)
		{
			value = VariantUtils.CreateFrom(in _undoRedoManager);
			return true;
		}
		if (name == PropertyName._editingDefinition)
		{
			value = VariantUtils.CreateFrom(in _editingDefinition);
			return true;
		}
		if (name == PropertyName._editingLayout)
		{
			value = VariantUtils.CreateFrom(in _editingLayout);
			return true;
		}
		if (name == PropertyName._editingResourcePath)
		{
			value = VariantUtils.CreateFrom(in _editingResourcePath);
			return true;
		}
		if (name == PropertyName._workbenchTabs)
		{
			value = VariantUtils.CreateFrom(in _workbenchTabs);
			return true;
		}
		if (name == PropertyName._extensionPropertySurface)
		{
			value = VariantUtils.CreateFrom(in _extensionPropertySurface);
			return true;
		}
		if (name == PropertyName._resourcePicker)
		{
			value = VariantUtils.CreateFrom(in _resourcePicker);
			return true;
		}
		if (name == PropertyName._surfaceBindingGeneration)
		{
			value = VariantUtils.CreateFrom(in _surfaceBindingGeneration);
			return true;
		}
		if (name == PropertyName._editorWorkbenchVisible)
		{
			value = VariantUtils.CreateFrom(in _editorWorkbenchVisible);
			return true;
		}
		if (name == PropertyName._historyRefreshQueued)
		{
			value = VariantUtils.CreateFrom(in _historyRefreshQueued);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._surface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._undoAdapter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._undoRedoManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingDefinition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingLayout, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._editingResourcePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._workbenchTabs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._extensionPropertySurface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourcePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._surfaceBindingGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._editorWorkbenchVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._historyRefreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.GraphSurface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ExtensionPropertySurface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.WorkbenchTabs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ActiveResourcePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UsesDedicatedGraphInputHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._surface, Variant.From(in _surface));
		info.AddProperty(PropertyName._undoAdapter, Variant.From(in _undoAdapter));
		info.AddProperty(PropertyName._undoRedoManager, Variant.From(in _undoRedoManager));
		info.AddProperty(PropertyName._editingDefinition, Variant.From(in _editingDefinition));
		info.AddProperty(PropertyName._editingLayout, Variant.From(in _editingLayout));
		info.AddProperty(PropertyName._editingResourcePath, Variant.From(in _editingResourcePath));
		info.AddProperty(PropertyName._workbenchTabs, Variant.From(in _workbenchTabs));
		info.AddProperty(PropertyName._extensionPropertySurface, Variant.From(in _extensionPropertySurface));
		info.AddProperty(PropertyName._resourcePicker, Variant.From(in _resourcePicker));
		info.AddProperty(PropertyName._surfaceBindingGeneration, Variant.From(in _surfaceBindingGeneration));
		info.AddProperty(PropertyName._editorWorkbenchVisible, Variant.From(in _editorWorkbenchVisible));
		info.AddProperty(PropertyName._historyRefreshQueued, Variant.From(in _historyRefreshQueued));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._surface, out var value))
		{
			_surface = value.As<StateMachineGraphEditorSurface>();
		}
		if (info.TryGetProperty(PropertyName._undoAdapter, out var value2))
		{
			_undoAdapter = value2.As<XWStateMachineUndoAdapter>();
		}
		if (info.TryGetProperty(PropertyName._undoRedoManager, out var value3))
		{
			_undoRedoManager = value3.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._editingDefinition, out var value4))
		{
			_editingDefinition = value4.As<StateMachineDefinition>();
		}
		if (info.TryGetProperty(PropertyName._editingLayout, out var value5))
		{
			_editingLayout = value5.As<StateMachineLayout>();
		}
		if (info.TryGetProperty(PropertyName._editingResourcePath, out var value6))
		{
			_editingResourcePath = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName._workbenchTabs, out var value7))
		{
			_workbenchTabs = value7.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._extensionPropertySurface, out var value8))
		{
			_extensionPropertySurface = value8.As<XWDirectPropertySurface>();
		}
		if (info.TryGetProperty(PropertyName._resourcePicker, out var value9))
		{
			_resourcePicker = value9.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._surfaceBindingGeneration, out var value10))
		{
			_surfaceBindingGeneration = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._editorWorkbenchVisible, out var value11))
		{
			_editorWorkbenchVisible = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._historyRefreshQueued, out var value12))
		{
			_historyRefreshQueued = value12.As<bool>();
		}
	}
}
