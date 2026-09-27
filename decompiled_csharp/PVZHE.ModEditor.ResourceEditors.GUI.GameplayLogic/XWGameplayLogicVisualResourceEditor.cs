using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/GameplayLogic/XWGameplayLogicVisualResourceEditor.cs")]
public class XWGameplayLogicVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName BindLayoutNodes = "BindLayoutNodes";

		public static readonly StringName ConnectResponsiveSignals = "ConnectResponsiveSignals";

		public static readonly StringName DisconnectResponsiveSignals = "DisconnectResponsiveSignals";

		public static readonly StringName OnGameplayPageChanged = "OnGameplayPageChanged";

		public static readonly StringName OnGameplayEditorVisibilityChanged = "OnGameplayEditorVisibilityChanged";

		public static readonly StringName UpdateResponsivePreviewProcessing = "UpdateResponsivePreviewProcessing";

		public static readonly StringName UpdateContextLabels = "UpdateContextLabels";

		public static readonly StringName ClearDynamicRoots = "ClearDynamicRoots";

		public static readonly StringName ClearDynamicChildren = "ClearDynamicChildren";

		public static readonly StringName UnmountPresenter = "UnmountPresenter";

		public static readonly StringName ResetPreviewSafety = "ResetPreviewSafety";

		public static readonly StringName DisposePreviewSafety = "DisposePreviewSafety";

		public static readonly StringName EnsureResourcePicker = "EnsureResourcePicker";

		public static readonly StringName ResetPropertyBinding = "ResetPropertyBinding";

		public static readonly StringName DisposePropertyBinding = "DisposePropertyBinding";

		public static readonly StringName OnVisualPropertyEdited = "OnVisualPropertyEdited";

		public static readonly StringName ResetEmbeddedResourceService = "ResetEmbeddedResourceService";

		public static readonly StringName OnEmbeddedResourceEdited = "OnEmbeddedResourceEdited";

		public new static readonly StringName OnCurrentResourceSaved = "OnCurrentResourceSaved";

		public new static readonly StringName OnCurrentResourceDraftRecovered = "OnCurrentResourceDraftRecovered";

		public static readonly StringName RefreshOwnerPanel = "RefreshOwnerPanel";

		public static readonly StringName JumpToOwnerResource = "JumpToOwnerResource";

		public static readonly StringName ShowPresenterError = "ShowPresenterError";

		public static readonly StringName HidePresenterError = "HidePresenterError";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _resourcePicker = "_resourcePicker";

		public static readonly StringName _resourceShelfHost = "_resourceShelfHost";

		public static readonly StringName _stageHost = "_stageHost";

		public static readonly StringName _timelineHost = "_timelineHost";

		public static readonly StringName _lifecycleWorkbench = "_lifecycleWorkbench";

		public static readonly StringName _gameplayPages = "_gameplayPages";

		public static readonly StringName _responsiveSignalsConnected = "_responsiveSignalsConnected";

		public static readonly StringName _advancedInspectorHost = "_advancedInspectorHost";

		public static readonly StringName _errorBarHost = "_errorBarHost";

		public static readonly StringName _errorMessageLabel = "_errorMessageLabel";

		public static readonly StringName _ownerPathLabel = "_ownerPathLabel";

		public static readonly StringName _saveStateLabel = "_saveStateLabel";

		public static readonly StringName _stageViewport = "_stageViewport";

		public static readonly StringName _stageRoot = "_stageRoot";

		public static readonly StringName _overlayRoot = "_overlayRoot";

		public static readonly StringName _hudRoot = "_hudRoot";

		public static readonly StringName _timelineRoot = "_timelineRoot";

		public static readonly StringName _shelfRoot = "_shelfRoot";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private readonly XWGameplayLogicPresenterRegistry _presenterRegistry = new XWGameplayLogicPresenterRegistry();

	private IXWGameplayLogicPresenter _activePresenter;

	private XWVisualPropertyBinding _propertyBinding;

	private XWEmbeddedResourceEditService _embeddedResourceEditService;

	private XWGameplayResourcePickerWindow _resourcePicker;

	private XWGameplayLogicPreviewSafety _previewSafety;

	private Control _resourceShelfHost;

	private Control _stageHost;

	private Control _timelineHost;

	private XWGameplayLifecycleWorkbench _lifecycleWorkbench;

	private TabContainer _gameplayPages;

	private bool _responsiveSignalsConnected;

	private Control _advancedInspectorHost;

	private Control _errorBarHost;

	private Label _errorMessageLabel;

	private Button _ownerPathLabel;

	private Label _saveStateLabel;

	private Action _ownerPathPressed;

	private SubViewport _stageViewport;

	private Node2D _stageRoot;

	private Control _overlayRoot;

	private Control _hudRoot;

	private Control _timelineRoot;

	private Control _shelfRoot;

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		BindLayoutNodes();
		EnsureResourcePicker();
		UnmountPresenter();
		ResetPropertyBinding();
		ResetEmbeddedResourceService();
		ClearDynamicRoots();
		ResetPreviewSafety();
		UpdateContextLabels();
		IReadOnlyList<IXWGameplayLogicPresenter> readOnlyList = _presenterRegistry.Find(CurrentResource);
		if (readOnlyList.Count != 1)
		{
			string message = ((readOnlyList.Count == 0) ? ((CurrentResource?.GetType().Name ?? "资源") + " 尚缺专用游戏画面呈现器。") : $"找到 {readOnlyList.Count} 个呈现器，请修正唯一匹配规则。");
			ShowPresenterError(message);
			return;
		}
		HidePresenterError();
		_activePresenter = readOnlyList[0];
		_activePresenter.Mount(new XWGameplayLogicPresentationContext
		{
			Resource = CurrentResource,
			EditContext = CurrentEditContext,
			PropertyBinding = _propertyBinding,
			EmbeddedResourceEditService = _embeddedResourceEditService,
			ResourcePicker = _resourcePicker,
			PreviewSafety = _previewSafety,
			StageViewport = _stageViewport,
			StageRoot = _stageRoot,
			OverlayRoot = _overlayRoot,
			HudRoot = _hudRoot,
			TimelineRoot = _timelineRoot,
			ShelfRoot = _shelfRoot,
			AdvancedInspectorHost = _advancedInspectorHost,
			ErrorBar = _errorBarHost
		});
		_activePresenter.Refresh();
		_lifecycleWorkbench?.Bind(CurrentResource);
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_ownerPathLabel) && _ownerPathPressed != null)
		{
			_ownerPathLabel.Pressed -= _ownerPathPressed;
		}
		DisconnectResponsiveSignals();
		UnmountPresenter();
		DisposePreviewSafety();
		DisposePropertyBinding();
		base._ExitTree();
	}

	private void BindLayoutNodes()
	{
		if (_resourceShelfHost == null)
		{
			_resourceShelfHost = GetNodeOrNull<Control>("%ResourceShelfHost");
		}
		if (_stageHost == null)
		{
			_stageHost = GetNodeOrNull<Control>("%StageHost");
		}
		if (_timelineHost == null)
		{
			_timelineHost = GetNodeOrNull<Control>("%TimelineHost");
		}
		if (_lifecycleWorkbench == null)
		{
			_lifecycleWorkbench = _timelineHost as XWGameplayLifecycleWorkbench;
		}
		if (_gameplayPages == null)
		{
			_gameplayPages = GetNodeOrNull<TabContainer>("%GameplayPages");
		}
		if (_advancedInspectorHost == null)
		{
			_advancedInspectorHost = GetNodeOrNull<Control>("%AdvancedInspectorHost");
		}
		if (_errorBarHost == null)
		{
			_errorBarHost = GetNodeOrNull<Control>("%ErrorBarHost");
		}
		if (_errorMessageLabel == null)
		{
			_errorMessageLabel = _errorBarHost?.GetNodeOrNull<Label>("%ErrorMessageLabel");
		}
		if (_ownerPathLabel == null)
		{
			_ownerPathLabel = GetNodeOrNull<Button>("%OwnerPathLabel");
		}
		if (GodotObject.IsInstanceValid(_ownerPathLabel) && _ownerPathPressed == null)
		{
			_ownerPathPressed = JumpToOwnerResource;
			_ownerPathLabel.Pressed += _ownerPathPressed;
		}
		if (_saveStateLabel == null)
		{
			_saveStateLabel = GetNodeOrNull<Label>("%SaveStateLabel");
		}
		if (_stageViewport == null)
		{
			_stageViewport = _stageHost?.GetNodeOrNull<SubViewport>("%StageViewport");
		}
		if (_stageRoot == null)
		{
			_stageRoot = _stageHost?.GetNodeOrNull<Node2D>("%StageRoot");
		}
		if (_overlayRoot == null)
		{
			_overlayRoot = _stageHost?.GetNodeOrNull<Control>("%OverlayRoot");
		}
		if (_hudRoot == null)
		{
			_hudRoot = _stageHost?.GetNodeOrNull<Control>("%HudRoot");
		}
		if (_timelineRoot == null)
		{
			_timelineRoot = _timelineHost?.GetNodeOrNull<Control>("%TimelineRoot");
		}
		if (_shelfRoot == null)
		{
			_shelfRoot = _resourceShelfHost?.GetNodeOrNull<Control>("%ShelfRoot");
		}
		ConnectResponsiveSignals();
		UpdateResponsivePreviewProcessing();
	}

	private void ConnectResponsiveSignals()
	{
		if (!_responsiveSignalsConnected && GodotObject.IsInstanceValid(_gameplayPages))
		{
			_gameplayPages.TabChanged += OnGameplayPageChanged;
			VisibilityChanged += OnGameplayEditorVisibilityChanged;
			_responsiveSignalsConnected = true;
		}
	}

	private void DisconnectResponsiveSignals()
	{
		if (_responsiveSignalsConnected)
		{
			if (GodotObject.IsInstanceValid(_gameplayPages))
			{
				_gameplayPages.TabChanged -= OnGameplayPageChanged;
			}
			VisibilityChanged -= OnGameplayEditorVisibilityChanged;
			_responsiveSignalsConnected = false;
			if (GodotObject.IsInstanceValid(_stageViewport))
			{
				_stageViewport.RenderTargetUpdateMode = SubViewport.UpdateMode.Disabled;
			}
		}
	}

	private void OnGameplayPageChanged(long _)
	{
		UpdateResponsivePreviewProcessing();
	}

	private void OnGameplayEditorVisibilityChanged()
	{
		UpdateResponsivePreviewProcessing();
	}

	private void UpdateResponsivePreviewProcessing()
	{
		if (GodotObject.IsInstanceValid(_stageViewport))
		{
			bool flag = IsVisibleInTree() && GodotObject.IsInstanceValid(_gameplayPages) && _gameplayPages.CurrentTab == 1;
			_stageViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode)(flag ? 4 : 0);
		}
	}

	private void UpdateContextLabels()
	{
		if (GodotObject.IsInstanceValid(_ownerPathLabel))
		{
			string text = CurrentEditContext?.OwnerPath ?? "";
			string value = CurrentEditContext?.PropertyPath ?? "";
			string value2 = (((CurrentEditContext?.ArrayIndex ?? (-1)) >= 0) ? $"[{CurrentEditContext.ArrayIndex}]" : "");
			_ownerPathLabel.Text = (string.IsNullOrWhiteSpace(value) ? ("根资源 · " + text) : $"拥有者: {text}  ·  {value}{value2}");
			_ownerPathLabel.Disabled = CurrentEditContext?.OwnerResource == null || CurrentEditContext.OwnerResource == CurrentResource;
			_ownerPathLabel.TooltipText = (_ownerPathLabel.Disabled ? "当前已经是根资源" : "返回拥有者资源并定位原属性/数组项");
		}
		if (GodotObject.IsInstanceValid(_saveStateLabel))
		{
			Label saveStateLabel = _saveStateLabel;
			XWResourceEditContext currentEditContext = CurrentEditContext;
			saveStateLabel.Text = ((currentEditContext != null && currentEditContext.IsBuiltInSource) ? "内置资源 · 保存时复制到当前 Mod" : "Mod 资源 · 可撤销 / 可自动保存");
		}
	}

	private void ClearDynamicRoots()
	{
		ClearDynamicChildren(_stageRoot);
		ClearDynamicChildren(_overlayRoot);
		ClearDynamicChildren(_hudRoot);
		ClearDynamicChildren(_timelineRoot);
		ClearDynamicChildren(_shelfRoot);
	}

	private static void ClearDynamicChildren(Node root)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return;
		}
		foreach (Node child in root.GetChildren())
		{
			child.QueueFree();
		}
	}

	private void UnmountPresenter()
	{
		_resourcePicker?.Dismiss();
		_lifecycleWorkbench?.Unbind();
		if (_activePresenter != null)
		{
			_activePresenter.Unmount();
			_activePresenter = null;
		}
		DisposePreviewSafety();
	}

	private void ResetPreviewSafety()
	{
		DisposePreviewSafety();
		_previewSafety = new XWGameplayLogicPreviewSafety();
	}

	private void DisposePreviewSafety()
	{
		_previewSafety?.Dispose();
		_previewSafety = null;
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

	private void ResetPropertyBinding()
	{
		DisposePropertyBinding();
		_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnVisualPropertyEdited);
	}

	private void DisposePropertyBinding()
	{
		_propertyBinding?.Dispose();
		_propertyBinding = null;
	}

	private void OnVisualPropertyEdited(bool committed)
	{
		if (GodotObject.IsInstanceValid(CurrentResource))
		{
			_lifecycleWorkbench?.Bind(CurrentResource);
			if (committed)
			{
				NotifyCurrentResourceEdited();
				RefreshOwnerPanel(markSaved: false);
			}
			else
			{
				MarkCurrentResourceDirty();
				CurrentResource.EmitChanged();
			}
			if (GodotObject.IsInstanceValid(_saveStateLabel))
			{
				_saveStateLabel.Text = (committed ? "未保存 · 已加入撤销记录" : "未保存 · 正在预览修改");
			}
		}
	}

	private void ResetEmbeddedResourceService()
	{
		_embeddedResourceEditService = new XWEmbeddedResourceEditService(XWEditorInterface.Instance?.GetUndoRedoManager(), OnEmbeddedResourceEdited);
	}

	private void OnEmbeddedResourceEdited()
	{
		_lifecycleWorkbench?.Bind(CurrentResource);
		NotifyCurrentResourceEdited(refreshVisuals: true);
		RefreshOwnerPanel(markSaved: false);
		if (GodotObject.IsInstanceValid(_saveStateLabel))
		{
			_saveStateLabel.Text = "未保存 · 资源组合已修改";
		}
	}

	protected override void OnCurrentResourceSaved()
	{
		string text = CurrentResource?.ResourcePath ?? "";
		bool markSaved = string.IsNullOrWhiteSpace(text) || text.Contains("::", StringComparison.Ordinal);
		RefreshOwnerPanel(markSaved);
	}

	protected override void OnCurrentResourceDraftRecovered()
	{
		RefreshOwnerPanel(markSaved: false);
	}

	private void RefreshOwnerPanel(bool markSaved)
	{
		Resource resource = CurrentEditContext?.OwnerResource;
		if (GodotObject.IsInstanceValid(resource) && resource != CurrentResource)
		{
			resource.EmitChanged();
			string ownerPath = CurrentEditContext.OwnerPath;
			if (XWResourceEditorRegistry.TryGetEditor(resource, ownerPath, out var descriptor) && XWEditorInterface.Instance?.TryGetLoadedResourceEditor(descriptor.DockKey) is XWGenericVisualResourceEditor xWGenericVisualResourceEditor)
			{
				xWGenericVisualResourceEditor.RefreshResourceIfOpen(resource, markSaved);
			}
		}
	}

	private void JumpToOwnerResource()
	{
		Resource resource = CurrentEditContext?.OwnerResource;
		if (GodotObject.IsInstanceValid(resource) && resource != CurrentResource)
		{
			string ownerPath = CurrentEditContext.OwnerPath;
			XWResourceEditContext context = XWResourceEditContext.ForProperty(resource, resource, ownerPath, ownerPath, CurrentEditContext.PropertyPath, CurrentEditContext.ArrayIndex, "gameplay_owner", CurrentEditContext.IsBuiltInSource);
			XWEditorInterface.Instance?.EditResource(resource, context);
		}
	}

	private void ShowPresenterError(string message)
	{
		if (GodotObject.IsInstanceValid(_errorBarHost))
		{
			_errorBarHost.Visible = true;
		}
		if (GodotObject.IsInstanceValid(_errorMessageLabel))
		{
			_errorMessageLabel.Text = message;
		}
	}

	private void HidePresenterError()
	{
		if (GodotObject.IsInstanceValid(_errorBarHost))
		{
			_errorBarHost.Visible = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(25)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindLayoutNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectResponsiveSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectResponsiveSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGameplayPageChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnGameplayEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateResponsivePreviewProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateContextLabels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearDynamicRoots, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearDynamicChildren, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.UnmountPresenter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetPreviewSafety, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposePreviewSafety, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureResourcePicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetPropertyBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposePropertyBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetEmbeddedResourceService, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEmbeddedResourceEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCurrentResourceSaved, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCurrentResourceDraftRecovered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshOwnerPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "markSaved", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.JumpToOwnerResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowPresenterError, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HidePresenterError, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BindLayoutNodes && args.Count == 0)
		{
			BindLayoutNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectResponsiveSignals && args.Count == 0)
		{
			ConnectResponsiveSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectResponsiveSignals && args.Count == 0)
		{
			DisconnectResponsiveSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGameplayPageChanged && args.Count == 1)
		{
			OnGameplayPageChanged(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnGameplayEditorVisibilityChanged && args.Count == 0)
		{
			OnGameplayEditorVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateResponsivePreviewProcessing && args.Count == 0)
		{
			UpdateResponsivePreviewProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateContextLabels && args.Count == 0)
		{
			UpdateContextLabels();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearDynamicRoots && args.Count == 0)
		{
			ClearDynamicRoots();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearDynamicChildren && args.Count == 1)
		{
			ClearDynamicChildren(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnmountPresenter && args.Count == 0)
		{
			UnmountPresenter();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetPreviewSafety && args.Count == 0)
		{
			ResetPreviewSafety();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposePreviewSafety && args.Count == 0)
		{
			DisposePreviewSafety();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureResourcePicker && args.Count == 0)
		{
			EnsureResourcePicker();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetPropertyBinding && args.Count == 0)
		{
			ResetPropertyBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposePropertyBinding && args.Count == 0)
		{
			DisposePropertyBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualPropertyEdited && args.Count == 1)
		{
			OnVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetEmbeddedResourceService && args.Count == 0)
		{
			ResetEmbeddedResourceService();
			ret = default;
			return true;
		}
		if (method == MethodName.OnEmbeddedResourceEdited && args.Count == 0)
		{
			OnEmbeddedResourceEdited();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCurrentResourceSaved && args.Count == 0)
		{
			OnCurrentResourceSaved();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCurrentResourceDraftRecovered && args.Count == 0)
		{
			OnCurrentResourceDraftRecovered();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshOwnerPanel && args.Count == 1)
		{
			RefreshOwnerPanel(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.JumpToOwnerResource && args.Count == 0)
		{
			JumpToOwnerResource();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowPresenterError && args.Count == 1)
		{
			ShowPresenterError(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HidePresenterError && args.Count == 0)
		{
			HidePresenterError();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ClearDynamicChildren && args.Count == 1)
		{
			ClearDynamicChildren(VariantUtils.ConvertTo<Node>(in args[0]));
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
		if (method == MethodName.BindLayoutNodes)
		{
			return true;
		}
		if (method == MethodName.ConnectResponsiveSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectResponsiveSignals)
		{
			return true;
		}
		if (method == MethodName.OnGameplayPageChanged)
		{
			return true;
		}
		if (method == MethodName.OnGameplayEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.UpdateResponsivePreviewProcessing)
		{
			return true;
		}
		if (method == MethodName.UpdateContextLabels)
		{
			return true;
		}
		if (method == MethodName.ClearDynamicRoots)
		{
			return true;
		}
		if (method == MethodName.ClearDynamicChildren)
		{
			return true;
		}
		if (method == MethodName.UnmountPresenter)
		{
			return true;
		}
		if (method == MethodName.ResetPreviewSafety)
		{
			return true;
		}
		if (method == MethodName.DisposePreviewSafety)
		{
			return true;
		}
		if (method == MethodName.EnsureResourcePicker)
		{
			return true;
		}
		if (method == MethodName.ResetPropertyBinding)
		{
			return true;
		}
		if (method == MethodName.DisposePropertyBinding)
		{
			return true;
		}
		if (method == MethodName.OnVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.ResetEmbeddedResourceService)
		{
			return true;
		}
		if (method == MethodName.OnEmbeddedResourceEdited)
		{
			return true;
		}
		if (method == MethodName.OnCurrentResourceSaved)
		{
			return true;
		}
		if (method == MethodName.OnCurrentResourceDraftRecovered)
		{
			return true;
		}
		if (method == MethodName.RefreshOwnerPanel)
		{
			return true;
		}
		if (method == MethodName.JumpToOwnerResource)
		{
			return true;
		}
		if (method == MethodName.ShowPresenterError)
		{
			return true;
		}
		if (method == MethodName.HidePresenterError)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._resourcePicker)
		{
			_resourcePicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._resourceShelfHost)
		{
			_resourceShelfHost = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._stageHost)
		{
			_stageHost = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._timelineHost)
		{
			_timelineHost = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._lifecycleWorkbench)
		{
			_lifecycleWorkbench = VariantUtils.ConvertTo<XWGameplayLifecycleWorkbench>(in value);
			return true;
		}
		if (name == PropertyName._gameplayPages)
		{
			_gameplayPages = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._responsiveSignalsConnected)
		{
			_responsiveSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._advancedInspectorHost)
		{
			_advancedInspectorHost = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._errorBarHost)
		{
			_errorBarHost = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._errorMessageLabel)
		{
			_errorMessageLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._ownerPathLabel)
		{
			_ownerPathLabel = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._saveStateLabel)
		{
			_saveStateLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._stageViewport)
		{
			_stageViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._stageRoot)
		{
			_stageRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._overlayRoot)
		{
			_overlayRoot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._hudRoot)
		{
			_hudRoot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._timelineRoot)
		{
			_timelineRoot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._shelfRoot)
		{
			_shelfRoot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._resourcePicker)
		{
			value = VariantUtils.CreateFrom(in _resourcePicker);
			return true;
		}
		if (name == PropertyName._resourceShelfHost)
		{
			value = VariantUtils.CreateFrom(in _resourceShelfHost);
			return true;
		}
		if (name == PropertyName._stageHost)
		{
			value = VariantUtils.CreateFrom(in _stageHost);
			return true;
		}
		if (name == PropertyName._timelineHost)
		{
			value = VariantUtils.CreateFrom(in _timelineHost);
			return true;
		}
		if (name == PropertyName._lifecycleWorkbench)
		{
			value = VariantUtils.CreateFrom(in _lifecycleWorkbench);
			return true;
		}
		if (name == PropertyName._gameplayPages)
		{
			value = VariantUtils.CreateFrom(in _gameplayPages);
			return true;
		}
		if (name == PropertyName._responsiveSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _responsiveSignalsConnected);
			return true;
		}
		if (name == PropertyName._advancedInspectorHost)
		{
			value = VariantUtils.CreateFrom(in _advancedInspectorHost);
			return true;
		}
		if (name == PropertyName._errorBarHost)
		{
			value = VariantUtils.CreateFrom(in _errorBarHost);
			return true;
		}
		if (name == PropertyName._errorMessageLabel)
		{
			value = VariantUtils.CreateFrom(in _errorMessageLabel);
			return true;
		}
		if (name == PropertyName._ownerPathLabel)
		{
			value = VariantUtils.CreateFrom(in _ownerPathLabel);
			return true;
		}
		if (name == PropertyName._saveStateLabel)
		{
			value = VariantUtils.CreateFrom(in _saveStateLabel);
			return true;
		}
		if (name == PropertyName._stageViewport)
		{
			value = VariantUtils.CreateFrom(in _stageViewport);
			return true;
		}
		if (name == PropertyName._stageRoot)
		{
			value = VariantUtils.CreateFrom(in _stageRoot);
			return true;
		}
		if (name == PropertyName._overlayRoot)
		{
			value = VariantUtils.CreateFrom(in _overlayRoot);
			return true;
		}
		if (name == PropertyName._hudRoot)
		{
			value = VariantUtils.CreateFrom(in _hudRoot);
			return true;
		}
		if (name == PropertyName._timelineRoot)
		{
			value = VariantUtils.CreateFrom(in _timelineRoot);
			return true;
		}
		if (name == PropertyName._shelfRoot)
		{
			value = VariantUtils.CreateFrom(in _shelfRoot);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._resourcePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceShelfHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._timelineHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lifecycleWorkbench, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gameplayPages, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._responsiveSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._advancedInspectorHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._errorBarHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._errorMessageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ownerPathLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveStateLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overlayRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hudRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._timelineRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shelfRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._resourcePicker, Variant.From(in _resourcePicker));
		info.AddProperty(PropertyName._resourceShelfHost, Variant.From(in _resourceShelfHost));
		info.AddProperty(PropertyName._stageHost, Variant.From(in _stageHost));
		info.AddProperty(PropertyName._timelineHost, Variant.From(in _timelineHost));
		info.AddProperty(PropertyName._lifecycleWorkbench, Variant.From(in _lifecycleWorkbench));
		info.AddProperty(PropertyName._gameplayPages, Variant.From(in _gameplayPages));
		info.AddProperty(PropertyName._responsiveSignalsConnected, Variant.From(in _responsiveSignalsConnected));
		info.AddProperty(PropertyName._advancedInspectorHost, Variant.From(in _advancedInspectorHost));
		info.AddProperty(PropertyName._errorBarHost, Variant.From(in _errorBarHost));
		info.AddProperty(PropertyName._errorMessageLabel, Variant.From(in _errorMessageLabel));
		info.AddProperty(PropertyName._ownerPathLabel, Variant.From(in _ownerPathLabel));
		info.AddProperty(PropertyName._saveStateLabel, Variant.From(in _saveStateLabel));
		info.AddProperty(PropertyName._stageViewport, Variant.From(in _stageViewport));
		info.AddProperty(PropertyName._stageRoot, Variant.From(in _stageRoot));
		info.AddProperty(PropertyName._overlayRoot, Variant.From(in _overlayRoot));
		info.AddProperty(PropertyName._hudRoot, Variant.From(in _hudRoot));
		info.AddProperty(PropertyName._timelineRoot, Variant.From(in _timelineRoot));
		info.AddProperty(PropertyName._shelfRoot, Variant.From(in _shelfRoot));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._resourcePicker, out var value))
		{
			_resourcePicker = value.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._resourceShelfHost, out var value2))
		{
			_resourceShelfHost = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._stageHost, out var value3))
		{
			_stageHost = value3.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._timelineHost, out var value4))
		{
			_timelineHost = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._lifecycleWorkbench, out var value5))
		{
			_lifecycleWorkbench = value5.As<XWGameplayLifecycleWorkbench>();
		}
		if (info.TryGetProperty(PropertyName._gameplayPages, out var value6))
		{
			_gameplayPages = value6.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._responsiveSignalsConnected, out var value7))
		{
			_responsiveSignalsConnected = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._advancedInspectorHost, out var value8))
		{
			_advancedInspectorHost = value8.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._errorBarHost, out var value9))
		{
			_errorBarHost = value9.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._errorMessageLabel, out var value10))
		{
			_errorMessageLabel = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._ownerPathLabel, out var value11))
		{
			_ownerPathLabel = value11.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._saveStateLabel, out var value12))
		{
			_saveStateLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._stageViewport, out var value13))
		{
			_stageViewport = value13.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._stageRoot, out var value14))
		{
			_stageRoot = value14.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._overlayRoot, out var value15))
		{
			_overlayRoot = value15.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._hudRoot, out var value16))
		{
			_hudRoot = value16.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._timelineRoot, out var value17))
		{
			_timelineRoot = value17.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._shelfRoot, out var value18))
		{
			_shelfRoot = value18.As<Control>();
		}
	}
}
