using System;
using System.Collections;
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

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileVisualResourceEditor.cs")]
public class XWProjectileVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName OnEmbeddedInspectorPropertyChanged = "OnEmbeddedInspectorPropertyChanged";

		public static readonly StringName RenderProjectileEditor = "RenderProjectileEditor";

		public static readonly StringName IsProjectilePipelineResource = "IsProjectilePipelineResource";

		public static readonly StringName RenderProjectileFirePipeline = "RenderProjectileFirePipeline";

		public static readonly StringName BindPipelineEditorFields = "BindPipelineEditorFields";

		public static readonly StringName BindFireComponentFields = "BindFireComponentFields";

		public static readonly StringName BindFireComponentCheckFields = "BindFireComponentCheckFields";

		public static readonly StringName BindFireProjectileFields = "BindFireProjectileFields";

		public static readonly StringName BindFireProjectileSingleFields = "BindFireProjectileSingleFields";

		public static readonly StringName BindFireProjectileWeightFields = "BindFireProjectileWeightFields";

		public static readonly StringName BindFireProjectileWeightItemFields = "BindFireProjectileWeightItemFields";

		public static readonly StringName BuildFireCollisionFlagGrid = "BuildFireCollisionFlagGrid";

		public static readonly StringName RefreshFireProjectilePreview = "RefreshFireProjectilePreview";

		public static readonly StringName BindCreateDataCoreFields = "BindCreateDataCoreFields";

		public static readonly StringName BindProjectileBehaviorFields = "BindProjectileBehaviorFields";

		public static readonly StringName OpenCreateDataProjectilePicker = "OpenCreateDataProjectilePicker";

		public static readonly StringName OpenBehaviorPreviewProjectilePicker = "OpenBehaviorPreviewProjectilePicker";

		public static readonly StringName EnsureProjectilePicker = "EnsureProjectilePicker";

		public static readonly StringName OpenProjectileObjectCatalog = "OpenProjectileObjectCatalog";

		public static readonly StringName ResolveProjectileObjectIcon = "ResolveProjectileObjectIcon";

		public static readonly StringName MountProjectileSegmentedOption = "MountProjectileSegmentedOption";

		public static readonly StringName DisposeProjectileVisualChoices = "DisposeProjectileVisualChoices";

		public static readonly StringName BindPipelineGamePreview = "BindPipelineGamePreview";

		public static readonly StringName ResolvePipelineCreateData = "ResolvePipelineCreateData";

		public static readonly StringName ResolveWeightPreviewData = "ResolveWeightPreviewData";

		public static readonly StringName RefreshPipelineCreateDataPreview = "RefreshPipelineCreateDataPreview";

		public static readonly StringName LoadProjectileRegistryResource = "LoadProjectileRegistryResource";

		public static readonly StringName BuildPreviewConfigFromData = "BuildPreviewConfigFromData";

		public static readonly StringName RebuildPipelineProjectilePreview = "RebuildPipelineProjectilePreview";

		public static readonly StringName ApplyPipelineProjectileScale = "ApplyPipelineProjectileScale";

		public static readonly StringName GetPipelineProjectilePreviewScale = "GetPipelineProjectilePreviewScale";

		public static readonly StringName RebuildPipelineTargetPreview = "RebuildPipelineTargetPreview";

		public static readonly StringName RebuildPipelineTrajectory = "RebuildPipelineTrajectory";

		public static readonly StringName EvaluatePipelinePosition = "EvaluatePipelinePosition";

		public static readonly StringName FindSinBehavior = "FindSinBehavior";

		public static readonly StringName GetProjectilePreviewKey = "GetProjectilePreviewKey";

		public static readonly StringName ConfigurePipelineNode = "ConfigurePipelineNode";

		public static readonly StringName PopulatePipelineBranches = "PopulatePipelineBranches";

		public static readonly StringName AddPipelineBranchCard = "AddPipelineBranchCard";

		public static readonly StringName PopulateEditablePipelineWeightBranches = "PopulateEditablePipelineWeightBranches";

		public static readonly StringName ConfigurePipelineWeightBranchCard = "ConfigurePipelineWeightBranchCard";

		public static readonly StringName RefreshPipelineWeightProbabilities = "RefreshPipelineWeightProbabilities";

		public static readonly StringName RefreshPipelineWeightList = "RefreshPipelineWeightList";

		public static readonly StringName SelectPipelineWeightBranch = "SelectPipelineWeightBranch";

		public static readonly StringName OpenPipelineWeightItem = "OpenPipelineWeightItem";

		public static readonly StringName AddPipelineWeightItem = "AddPipelineWeightItem";

		public static readonly StringName DuplicatePipelineWeightItem = "DuplicatePipelineWeightItem";

		public static readonly StringName RemovePipelineWeightItem = "RemovePipelineWeightItem";

		public static readonly StringName MovePipelineWeightItem = "MovePipelineWeightItem";

		public static readonly StringName ReplacePipelineWeightItems = "ReplacePipelineWeightItems";

		public static readonly StringName RebuildPipelineWeightSurfaces = "RebuildPipelineWeightSurfaces";

		public static readonly StringName RefreshPipelineBranchCards = "RefreshPipelineBranchCards";

		public static readonly StringName OpenFirePipelineNestedResource = "OpenFirePipelineNestedResource";

		public static readonly StringName GetPipelineTitle = "GetPipelineTitle";

		public static readonly StringName GetPipelineParameterHint = "GetPipelineParameterHint";

		public static readonly StringName GetPipelineStatus = "GetPipelineStatus";

		public static readonly StringName GetShooterNodeDetail = "GetShooterNodeDetail";

		public static readonly StringName GetCheckNodeDetail = "GetCheckNodeDetail";

		public static readonly StringName GetPoolNodeDetail = "GetPoolNodeDetail";

		public static readonly StringName GetCreateNodeDetail = "GetCreateNodeDetail";

		public static readonly StringName GetBehaviorNodeDetail = "GetBehaviorNodeDetail";

		public static readonly StringName DescribeCreateData = "DescribeCreateData";

		public static readonly StringName DescribeProjectileResource = "DescribeProjectileResource";

		public static readonly StringName BindProjectileScenePreview = "BindProjectileScenePreview";

		public static readonly StringName BindProjectileCoreFields = "BindProjectileCoreFields";

		public static readonly StringName BindProjectileHitFields = "BindProjectileHitFields";

		public static readonly StringName EnsureProjectilePropertyBinding = "EnsureProjectilePropertyBinding";

		public static readonly StringName CommitProjectileProperty = "CommitProjectileProperty";

		public static readonly StringName OpenProjectileNestedResource = "OpenProjectileNestedResource";

		public static readonly StringName RebuildProjectileScenePreview = "RebuildProjectileScenePreview";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName GetPipelinePreviewDuration = "GetPipelinePreviewDuration";

		public static readonly StringName PlayProjectilePreview = "PlayProjectilePreview";

		public static readonly StringName PlayPipelinePreview = "PlayPipelinePreview";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public static readonly StringName ApplyProjectileRuntimeProcessMode = "ApplyProjectileRuntimeProcessMode";

		public static readonly StringName UpdatePipelinePlaybackVisual = "UpdatePipelinePlaybackVisual";

		public static readonly StringName UpdateProjectilePlaybackVisual = "UpdateProjectilePlaybackVisual";

		public static readonly StringName RebuildProjectileTargetPreview = "RebuildProjectileTargetPreview";

		public static readonly StringName ShowProjectileImpactPreview = "ShowProjectileImpactPreview";

		public static readonly StringName ClearProjectileImpactPreview = "ClearProjectileImpactPreview";

		public static readonly StringName FindRuntimeProjectile = "FindRuntimeProjectile";

		public static readonly StringName FindRuntimeCharacter = "FindRuntimeCharacter";

		public static readonly StringName UpdateProjectileSummary = "UpdateProjectileSummary";

		public static readonly StringName RefreshProjectileLists = "RefreshProjectileLists";

		public static readonly StringName BindProjectileChangePanel = "BindProjectileChangePanel";

		public static readonly StringName RefreshProjectileChangeList = "RefreshProjectileChangeList";

		public static readonly StringName OpenProjectileChangeItem = "OpenProjectileChangeItem";

		public static readonly StringName HasProjectileProperty = "HasProjectileProperty";

		public static readonly StringName ReadProjectileString = "ReadProjectileString";

		public static readonly StringName ReadProjectileDouble = "ReadProjectileDouble";

		public static readonly StringName ReadProjectileInt = "ReadProjectileInt";

		public static readonly StringName ReadProjectileBool = "ReadProjectileBool";

		public static readonly StringName ReadProjectileVector2 = "ReadProjectileVector2";

		public static readonly StringName ReadProjectilePackedScene = "ReadProjectilePackedScene";

		public static readonly StringName ReadProjectileVariant = "ReadProjectileVariant";

		public static readonly StringName FormatResource = "FormatResource";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";

		public static readonly StringName FormatVector = "FormatVector";

		public static readonly StringName FormatBool = "FormatBool";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingProjectile = "_editingProjectile";

		public static readonly StringName _projectileViewport = "_projectileViewport";

		public static readonly StringName _projectilePreviewRoot = "_projectilePreviewRoot";

		public static readonly StringName _projectileTargetRoot = "_projectileTargetRoot";

		public static readonly StringName _projectileImpactRoot = "_projectileImpactRoot";

		public static readonly StringName _projectileTimelineSlider = "_projectileTimelineSlider";

		public static readonly StringName _projectilePlayButton = "_projectilePlayButton";

		public static readonly StringName _projectilePlaybackStatusLabel = "_projectilePlaybackStatusLabel";

		public static readonly StringName _projectilePreviewPlaying = "_projectilePreviewPlaying";

		public static readonly StringName _projectilePreviewProgress = "_projectilePreviewProgress";

		public static readonly StringName _projectileSceneInstance = "_projectileSceneInstance";

		public static readonly StringName _projectileNameLabel = "_projectileNameLabel";

		public static readonly StringName _sceneLabel = "_sceneLabel";

		public static readonly StringName _damageLabel = "_damageLabel";

		public static readonly StringName _flagLabel = "_flagLabel";

		public static readonly StringName _rangeLabel = "_rangeLabel";

		public static readonly StringName _hitTargetEventList = "_hitTargetEventList";

		public static readonly StringName _hitCharacterEventList = "_hitCharacterEventList";

		public static readonly StringName _hitGroundEventList = "_hitGroundEventList";

		public static readonly StringName _behaviorsList = "_behaviorsList";

		public static readonly StringName _projectileChangeList = "_projectileChangeList";

		public static readonly StringName _projectileChangeHintLabel = "_projectileChangeHintLabel";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _pipelineProjectileRoot = "_pipelineProjectileRoot";

		public static readonly StringName _pipelineTargetRoot = "_pipelineTargetRoot";

		public static readonly StringName _pipelineTrajectoryLine = "_pipelineTrajectoryLine";

		public static readonly StringName _pipelineTimelineSlider = "_pipelineTimelineSlider";

		public static readonly StringName _pipelinePlayButton = "_pipelinePlayButton";

		public static readonly StringName _pipelinePreviewStatus = "_pipelinePreviewStatus";

		public static readonly StringName _pipelineStatusLabel = "_pipelineStatusLabel";

		public static readonly StringName _pipelineProjectileInstance = "_pipelineProjectileInstance";

		public static readonly StringName _pipelineResolvedScene = "_pipelineResolvedScene";

		public static readonly StringName _pipelinePreviewConfig = "_pipelinePreviewConfig";

		public static readonly StringName _pipelineCreateData = "_pipelineCreateData";

		public static readonly StringName _pipelineBehavior = "_pipelineBehavior";

		public static readonly StringName _pipelinePreviewProgress = "_pipelinePreviewProgress";

		public static readonly StringName _pipelinePreviewPlaying = "_pipelinePreviewPlaying";

		public static readonly StringName _pipelinePreviewProjectileKey = "_pipelinePreviewProjectileKey";

		public static readonly StringName _projectilePicker = "_projectilePicker";

		public static readonly StringName _pipelineBranchHost = "_pipelineBranchHost";

		public static readonly StringName _pipelineWeightList = "_pipelineWeightList";

		public static readonly StringName _pipelineWeightResource = "_pipelineWeightResource";

		public static readonly StringName _selectedPipelineWeightIndex = "_selectedPipelineWeightIndex";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string EditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileVisualEditorLayout.tscn";

	private const string CoreFieldsScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileCoreFields.tscn";

	private const string HitFieldsScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileHitFields.tscn";

	private const string FirePipelineScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileFirePipelineLayout.tscn";

	private const string PipelineBranchScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectilePipelineBranchCard.tscn";

	private const string BehaviorFieldsScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileBehaviorFields.tscn";

	private const string FireComponentFieldsScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileFireComponentFields.tscn";

	private const string ProjectileRegistryPath = "res://Registry/Projectile/ProjectileRegistry.json";

	private static PackedScene _editorLayoutScene;

	private static PackedScene _coreFieldsScene;

	private static PackedScene _hitFieldsScene;

	private static PackedScene _firePipelineScene;

	private static PackedScene _pipelineBranchScene;

	private static PackedScene _behaviorFieldsScene;

	private static PackedScene _fireComponentFieldsScene;

	private static readonly HashSet<string> ProjectileInspectorEditableProperties = new HashSet<string>
	{
		"resource_name", "resource_local_to_scene", "name", "skinName", "baseDamage", "size", "scale", "projectileObject", "projectileScene", "splatSceneType",
		"splatAudio", "splatScene", "hitEffect", "damageFlags", "fireMethodFlags", "collisionFlags", "catapultHeight", "trackSearchInterval", "rotateFollowVelocity", "hitBody",
		"isFire", "rangeType", "useRange", "rangeSize", "hitPesontage", "penetrateNum", "penetrateOverBack", "backOutGround", "backDuration", "hitTargetEventList",
		"hitCharacterEventList", "hitGroundEventList", "behaviors", "Flag/Damage", "Flag/FireMethod", "Flag/Collision", "Catapult/Height", "Penetrate/Num", "Penetrate/OverBack", "Back/OutOfGround",
		"Back/Duration"
	};

	private Resource _editingProjectile;

	private SubViewport _projectileViewport;

	private Node2D _projectilePreviewRoot;

	private Node2D _projectileTargetRoot;

	private Node2D _projectileImpactRoot;

	private HSlider _projectileTimelineSlider;

	private Button _projectilePlayButton;

	private Label _projectilePlaybackStatusLabel;

	private bool _projectilePreviewPlaying;

	private double _projectilePreviewProgress;

	private Node _projectileSceneInstance;

	private Label _projectileNameLabel;

	private Label _sceneLabel;

	private Label _damageLabel;

	private Label _flagLabel;

	private Label _rangeLabel;

	private ItemList _hitTargetEventList;

	private ItemList _hitCharacterEventList;

	private ItemList _hitGroundEventList;

	private ItemList _behaviorsList;

	private ItemList _projectileChangeList;

	private Label _projectileChangeHintLabel;

	private bool _updatingControls;

	private XWVisualPropertyBinding _projectilePropertyBinding;

	private Node2D _pipelineProjectileRoot;

	private Node2D _pipelineTargetRoot;

	private Line2D _pipelineTrajectoryLine;

	private HSlider _pipelineTimelineSlider;

	private Button _pipelinePlayButton;

	private Label _pipelinePreviewStatus;

	private Label _pipelineStatusLabel;

	private Node _pipelineProjectileInstance;

	private PackedScene _pipelineResolvedScene;

	private TowerDefenseProjectileConfig _pipelinePreviewConfig;

	private TowerDefenseProjectileCreateData _pipelineCreateData;

	private ProjectileBehaviorDefinition _pipelineBehavior;

	private double _pipelinePreviewProgress;

	private bool _pipelinePreviewPlaying;

	private string _pipelinePreviewProjectileKey = "";

	private XWGameplayResourcePickerWindow _projectilePicker;

	private readonly List<XWVisualSegmentedOption> _projectileSegmentedOptions = new List<XWVisualSegmentedOption>();

	private HBoxContainer _pipelineBranchHost;

	private ItemList _pipelineWeightList;

	private FireComponentProjectileWeight _pipelineWeightResource;

	private int _selectedPipelineWeightIndex = -1;

	public override void _Ready()
	{
		base._Ready();
		EnableVisibilityGatedProcessing();
	}

	public override void _ExitTree()
	{
		DisposeProjectileVisualChoices();
		_pipelinePreviewPlaying = false;
		_projectilePropertyBinding?.Dispose();
		_projectilePropertyBinding = null;
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeProjectileVisualChoices();
		_projectilePropertyBinding?.Dispose();
		_projectilePropertyBinding = null;
		_projectilePreviewPlaying = false;
		_pipelinePreviewPlaying = false;
		_pipelineProjectileRoot = null;
		_pipelineTargetRoot = null;
		_pipelineTrajectoryLine = null;
		_pipelineTimelineSlider = null;
		_pipelinePlayButton = null;
		_pipelinePreviewStatus = null;
		_pipelineStatusLabel = null;
		_pipelineProjectileInstance = null;
		_pipelineResolvedScene = null;
		_pipelinePreviewConfig = null;
		_pipelineCreateData = null;
		_pipelineBehavior = null;
		_pipelineBranchHost = null;
		_pipelineWeightList = null;
		_pipelineWeightResource = null;
		_selectedPipelineWeightIndex = -1;
		RequestVisibilityGatedProcessing(requested: false);
		Resource currentResource = CurrentResource;
		if ((currentResource is TowerDefenseProjectileConfig || currentResource is TowerDefenseProjectileData) ? true : false)
		{
			RenderProjectileEditor(_editingProjectile = CurrentResource);
		}
		else if (IsProjectilePipelineResource(CurrentResource))
		{
			_editingProjectile = CurrentResource;
			RenderProjectileFirePipeline(CurrentResource);
		}
		else
		{
			_editingProjectile = null;
		}
	}

	protected override HashSet<string> GetEmbeddedInspectorAllowedProperties(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if ((resource is TowerDefenseProjectileConfig || resource is TowerDefenseProjectileData) ? true : false)
		{
			return ProjectileInspectorEditableProperties;
		}
		return base.GetEmbeddedInspectorAllowedProperties(resource, path, descriptor);
	}

	protected override void OnEmbeddedInspectorPropertyChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		if ((obj is TowerDefenseProjectileConfig || obj is TowerDefenseProjectileData) ? true : false)
		{
			Resource projectile = (_editingProjectile = obj as Resource);
			UpdateProjectileSummary();
			RefreshProjectileLists(projectile);
			UpdateProjectilePlaybackVisual();
		}
	}

	private void RenderProjectileEditor(Resource projectile)
	{
		if (CanvasGrid == null)
		{
			return;
		}
		CanvasGrid.Columns = 1;
		if (_editorLayoutScene == null)
		{
			_editorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _editorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(vBoxContainer))
		{
			CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
			BindProjectileScenePreview(vBoxContainer);
			if (_coreFieldsScene == null)
			{
				_coreFieldsScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileCoreFields.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			if (_hitFieldsScene == null)
			{
				_hitFieldsScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileHitFields.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			Control control = _coreFieldsScene?.Instantiate<Control>(PackedScene.GenEditState.Disabled);
			Control control2 = _hitFieldsScene?.Instantiate<Control>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(control))
			{
				vBoxContainer.GetNode<VBoxContainer>("%CoreHost").AddChild(control, forceReadableName: false, InternalMode.Disabled);
				BindProjectileCoreFields(control, projectile);
			}
			if (GodotObject.IsInstanceValid(control2))
			{
				vBoxContainer.GetNode<VBoxContainer>("%HitHost").AddChild(control2, forceReadableName: false, InternalMode.Disabled);
				BindProjectileHitFields(control2, projectile);
			}
			BindProjectileChangePanel(vBoxContainer, projectile);
		}
	}

	private static bool IsProjectilePipelineResource(Resource resource)
	{
		if (resource is TowerDefenseProjectileCreateData || resource is ProjectileBehaviorDefinition || resource is FireComponentCheckConfig || resource is FireComponentFireProjectileConfig || resource is FireComponentProjectileResource || resource is FireComponentProjectileWeightItem)
		{
			return true;
		}
		return false;
	}

	private void RenderProjectileFirePipeline(Resource resource)
	{
		if (CanvasGrid == null || !GodotObject.IsInstanceValid(resource))
		{
			return;
		}
		CanvasGrid.Columns = 1;
		if (_firePipelineScene == null)
		{
			_firePipelineScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileFirePipelineLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		if (_pipelineBranchScene == null)
		{
			_pipelineBranchScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectilePipelineBranchCard.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _firePipelineScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(vBoxContainer))
		{
			CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
			EnsureProjectilePropertyBinding();
			_pipelinePreviewProjectileKey = ((!string.IsNullOrWhiteSpace(CurrentEditContext?.PreviewKey)) ? CurrentEditContext.PreviewKey : GetProjectilePreviewKey(resource));
			if (string.IsNullOrWhiteSpace(_pipelinePreviewProjectileKey))
			{
				_pipelinePreviewProjectileKey = "PeaDefault";
			}
			vBoxContainer.GetNode<Label>("%ResourceTypeLabel").Text = resource.GetType().Name;
			vBoxContainer.GetNode<Label>("%Title").Text = GetPipelineTitle(resource);
			vBoxContainer.GetNode<Label>("%ParameterHint").Text = GetPipelineParameterHint(resource);
			vBoxContainer.GetNode<Label>("%PipelineStatusLabel").Text = GetPipelineStatus(resource);
			ConfigurePipelineNode(vBoxContainer.GetNode<PanelContainer>("%ShooterNode"), resource is FireComponentFireProjectileConfig, GetShooterNodeDetail(resource));
			ConfigurePipelineNode(vBoxContainer.GetNode<PanelContainer>("%CheckNode"), resource is FireComponentCheckConfig, GetCheckNodeDetail(resource));
			PanelContainer node = vBoxContainer.GetNode<PanelContainer>("%PoolNode");
			bool active = ((resource is FireComponentProjectileResource || resource is FireComponentProjectileWeightItem || resource is FireComponentCheckConfig) ? true : false);
			ConfigurePipelineNode(node, active, GetPoolNodeDetail(resource));
			PanelContainer node2 = vBoxContainer.GetNode<PanelContainer>("%CreateNode");
			active = ((resource is TowerDefenseProjectileCreateData || resource is FireComponentProjectileResource || resource is FireComponentProjectileWeightItem) ? true : false);
			ConfigurePipelineNode(node2, active, GetCreateNodeDetail(resource));
			PanelContainer node3 = vBoxContainer.GetNode<PanelContainer>("%BehaviorNode");
			active = ((resource is ProjectileBehaviorDefinition || resource is TowerDefenseProjectileCreateData) ? true : false);
			ConfigurePipelineNode(node3, active, GetBehaviorNodeDetail(resource));
			ConfigurePipelineNode(vBoxContainer.GetNode<PanelContainer>("%TargetNode"), resource is TowerDefenseProjectileCreateData, "命中事件 · 安全只读预览");
			_pipelineBranchHost = vBoxContainer.GetNode<HBoxContainer>("%BranchHost");
			PopulatePipelineBranches(_pipelineBranchHost, resource);
			BindPipelineGamePreview(vBoxContainer, resource);
			BindPipelineEditorFields(vBoxContainer.GetNode<VBoxContainer>("%ParameterHost"), resource);
		}
	}

	private void BindPipelineEditorFields(VBoxContainer parameterHost, Resource resource)
	{
		if (!GodotObject.IsInstanceValid(parameterHost))
		{
			return;
		}
		if (resource is TowerDefenseProjectileCreateData towerDefenseProjectileCreateData)
		{
			if (_coreFieldsScene == null)
			{
				_coreFieldsScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileCoreFields.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			if (_hitFieldsScene == null)
			{
				_hitFieldsScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileHitFields.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			Control control = _coreFieldsScene?.Instantiate<Control>(PackedScene.GenEditState.Disabled);
			Control control2 = _hitFieldsScene?.Instantiate<Control>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(control))
			{
				parameterHost.AddChild(control, forceReadableName: false, InternalMode.Disabled);
				BindCreateDataCoreFields(control, towerDefenseProjectileCreateData);
			}
			if (GodotObject.IsInstanceValid(control2))
			{
				parameterHost.AddChild(control2, forceReadableName: false, InternalMode.Disabled);
				BindProjectileHitFields(control2, towerDefenseProjectileCreateData);
			}
		}
		else if (resource is ProjectileBehaviorDefinition behavior)
		{
			if (_behaviorFieldsScene == null)
			{
				_behaviorFieldsScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileBehaviorFields.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			PanelContainer panelContainer = _behaviorFieldsScene?.Instantiate<PanelContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(panelContainer))
			{
				parameterHost.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
				BindProjectileBehaviorFields(panelContainer, behavior);
			}
		}
		else
		{
			if (_fireComponentFieldsScene == null)
			{
				_fireComponentFieldsScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWProjectileFireComponentFields.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			PanelContainer panelContainer2 = _fireComponentFieldsScene?.Instantiate<PanelContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(panelContainer2))
			{
				parameterHost.AddChild(panelContainer2, forceReadableName: false, InternalMode.Disabled);
				BindFireComponentFields(panelContainer2, resource);
			}
		}
	}

	private void BindFireComponentFields(PanelContainer root, Resource resource)
	{
		EnsureProjectilePropertyBinding();
		root.GetNode<Control>("%CheckSection").Visible = resource is FireComponentCheckConfig;
		root.GetNode<Control>("%FireSection").Visible = resource is FireComponentFireProjectileConfig;
		root.GetNode<Control>("%SingleSection").Visible = resource is FireComponentProjectileSingle;
		root.GetNode<Control>("%WeightSection").Visible = resource is FireComponentProjectileWeight;
		root.GetNode<Control>("%WeightItemSection").Visible = resource is FireComponentProjectileWeightItem;
		if (!(resource is FireComponentCheckConfig check))
		{
			if (!(resource is FireComponentFireProjectileConfig fire))
			{
				if (!(resource is FireComponentProjectileSingle single))
				{
					if (!(resource is FireComponentProjectileWeight weight))
					{
						if (resource is FireComponentProjectileWeightItem item)
						{
							BindFireProjectileWeightItemFields(root, item);
						}
					}
					else
					{
						BindFireProjectileWeightFields(root, weight);
					}
				}
				else
				{
					BindFireProjectileSingleFields(root, single);
				}
			}
			else
			{
				BindFireProjectileFields(root, fire);
			}
		}
		else
		{
			BindFireComponentCheckFields(root, check);
		}
	}

	private void BindFireComponentCheckFields(PanelContainer root, FireComponentCheckConfig check)
	{
		CheckButton node = root.GetNode<CheckButton>("%UseParentCollisionCheck");
		GridContainer flagsGrid = root.GetNode<GridContainer>("%CollisionFlagsGrid");
		Label flagsLabel = root.GetNode<Label>("%CollisionFlagsLabel");
		node.ButtonPressed = check.useParentCollision;
		RefreshCollisionVisibility();
		node.Toggled += (bool value) =>
		{
			_projectilePropertyBinding.SetValue(check, "useParentCollision", value, "修改碰撞阵营继承");
			check.NotifyPropertyListChanged();
			RefreshCollisionVisibility();
		};
		BuildFireCollisionFlagGrid(flagsGrid, check);
		Button open = root.GetNode<Button>("%OpenCheckProjectileButton");
		XWResourcePicker node2 = root.GetNode<XWResourcePicker>("%CheckProjectilePicker");
		BindFireProjectilePicker(node2, check.projectile, (FireComponentProjectileResource resource) =>
		{
			_projectilePropertyBinding.SetValue(check, "projectile", resource, "修改检测子弹池");
			open.Disabled = !GodotObject.IsInstanceValid(resource);
			RefreshPipelineBranchCards();
			RefreshPipelineCreateDataPreview();
		});
		open.Disabled = !GodotObject.IsInstanceValid(check.projectile);
		open.Pressed += () =>
		{
			OpenFirePipelineNestedResource(check.projectile, check, "projectile", -1);
		};
		node2.ResourceSelected += (Resource resource) =>
		{
			OpenFirePipelineNestedResource(resource, check, "projectile", -1);
		};
		void RefreshCollisionVisibility()
		{
			flagsGrid.Visible = !check.useParentCollision;
			flagsLabel.Visible = !check.useParentCollision;
		}
	}

	private void BindFireProjectileFields(PanelContainer root, FireComponentFireProjectileConfig fire)
	{
		_projectilePropertyBinding.BindNumber(root.GetNode<SpinBox>("%CheckProjectileIdSpinBox"), fire, "checkProjectileId", RefreshFireProjectilePreview);
		_projectilePropertyBinding.BindNumber(root.GetNode<SpinBox>("%FirePosIdSpinBox"), fire, "firePosId", RefreshFireProjectilePreview);
		_projectilePropertyBinding.BindNumber(root.GetNode<SpinBox>("%SpeedSpinBox"), fire, "speed", RefreshFireProjectilePreview);
		_projectilePropertyBinding.BindNumber(root.GetNode<SpinBox>("%DirectionSpinBox"), fire, "dir", RefreshFireProjectilePreview);
		_projectilePropertyBinding.BindNumber(root.GetNode<SpinBox>("%OffsetLineSpinBox"), fire, "offsetLine", RefreshFireProjectilePreview);
		_projectilePropertyBinding.BindNumber(root.GetNode<SpinBox>("%FireNumSkipSpinBox"), fire, "fireNumSkip", RefreshFireProjectilePreview);
		_projectilePropertyBinding.BindText(root.GetNode<LineEdit>("%FireEventNeedLineEdit"), fire, "fireEventNeed", RefreshFireProjectilePreview);
		CheckButton node = root.GetNode<CheckButton>("%ProjectileFlipCheck");
		node.ButtonPressed = fire.projectileFlip;
		node.Toggled += (bool value) =>
		{
			_projectilePropertyBinding.SetValue(fire, "projectileFlip", value, "修改子弹翻转");
			RefreshFireProjectilePreview();
		};
	}

	private void BindFireProjectileSingleFields(PanelContainer root, FireComponentProjectileSingle single)
	{
		Button open = root.GetNode<Button>("%OpenSingleProjectileButton");
		XWResourcePicker node = root.GetNode<XWResourcePicker>("%SingleProjectileDataPicker");
		BindCreateDataPicker(node, single.projectileData, (TowerDefenseProjectileCreateData resource) =>
		{
			_projectilePropertyBinding.SetValue(single, "projectileData", resource, "修改单发子弹数据");
			open.Disabled = !GodotObject.IsInstanceValid(resource);
			RefreshPipelineBranchCards();
			RefreshPipelineCreateDataPreview();
		});
		open.Disabled = !GodotObject.IsInstanceValid(single.projectileData);
		open.Pressed += () =>
		{
			OpenFirePipelineNestedResource(single.projectileData, single, "projectileData", -1);
		};
		node.ResourceSelected += (Resource resource) =>
		{
			OpenFirePipelineNestedResource(resource, single, "projectileData", -1);
		};
	}

	private void BindFireProjectileWeightFields(PanelContainer root, FireComponentProjectileWeight weight)
	{
		_pipelineWeightResource = weight;
		CheckButton node = root.GetNode<CheckButton>("%AverageWeightCheck");
		node.ButtonPressed = weight.averageWeight;
		node.Toggled += (bool value) =>
		{
			_projectilePropertyBinding.SetValue(weight, "averageWeight", value, "修改权重模式");
			RefreshPipelineWeightProbabilities();
		};
		Button openDefault = root.GetNode<Button>("%OpenDefaultProjectileButton");
		XWResourcePicker node2 = root.GetNode<XWResourcePicker>("%WeightDefaultProjectileDataPicker");
		BindCreateDataPicker(node2, weight.projectileData, (TowerDefenseProjectileCreateData resource) =>
		{
			_projectilePropertyBinding.SetValue(weight, "projectileData", resource, "修改默认子弹数据");
			openDefault.Disabled = !GodotObject.IsInstanceValid(resource);
			if (weight.projectileWeight == null || weight.projectileWeight.Count == 0)
			{
				RefreshPipelineBranchCards();
			}
			RefreshPipelineCreateDataPreview();
		});
		openDefault.Disabled = !GodotObject.IsInstanceValid(weight.projectileData);
		openDefault.Pressed += () =>
		{
			OpenFirePipelineNestedResource(weight.projectileData, weight, "projectileData", -1);
		};
		node2.ResourceSelected += (Resource resource) =>
		{
			OpenFirePipelineNestedResource(resource, weight, "projectileData", -1);
		};
		_pipelineWeightList = root.GetNode<ItemList>("%WeightList");
		_pipelineWeightList.ItemSelected += (long index) =>
		{
			SelectPipelineWeightBranch((int)index);
		};
		_pipelineWeightList.ItemActivated += (long index) =>
		{
			OpenPipelineWeightItem((int)index);
		};
		root.GetNode<Button>("%AddWeightButton").Pressed += AddPipelineWeightItem;
		root.GetNode<Button>("%DuplicateWeightButton").Pressed += DuplicatePipelineWeightItem;
		root.GetNode<Button>("%RemoveWeightButton").Pressed += RemovePipelineWeightItem;
		root.GetNode<Button>("%MoveWeightUpButton").Pressed += () =>
		{
			MovePipelineWeightItem(-1);
		};
		root.GetNode<Button>("%MoveWeightDownButton").Pressed += () =>
		{
			MovePipelineWeightItem(1);
		};
		RefreshPipelineWeightList();
	}

	private void BindFireProjectileWeightItemFields(PanelContainer root, FireComponentProjectileWeightItem item)
	{
		Button open = root.GetNode<Button>("%OpenWeightItemProjectileButton");
		XWResourcePicker node = root.GetNode<XWResourcePicker>("%WeightItemProjectilePicker");
		BindFireProjectilePicker(node, item.projectileResource, (FireComponentProjectileResource resource) =>
		{
			_projectilePropertyBinding.SetValue(item, "projectileResource", resource, "修改权重分支子弹池");
			open.Disabled = !GodotObject.IsInstanceValid(resource);
			RefreshPipelineBranchCards();
			RefreshPipelineCreateDataPreview();
		});
		open.Disabled = !GodotObject.IsInstanceValid(item.projectileResource);
		open.Pressed += () =>
		{
			OpenFirePipelineNestedResource(item.projectileResource, item, "projectileResource", -1);
		};
		node.ResourceSelected += (Resource resource) =>
		{
			OpenFirePipelineNestedResource(resource, item, "projectileResource", -1);
		};
		_projectilePropertyBinding.BindNumber(root.GetNode<SpinBox>("%WeightSpinBox"), item, "weight", RefreshPipelineWeightProbabilities);
	}

	private static void BindFireProjectilePicker(XWResourcePicker picker, FireComponentProjectileResource value, Action<FireComponentProjectileResource> changed)
	{
		picker.Setup("FireComponentProjectileResource");
		picker.SetEditedResource(value);
		picker.ResourceChanged += (Resource resource) =>
		{
			changed?.Invoke(resource as FireComponentProjectileResource);
		};
	}

	private static void BindCreateDataPicker(XWResourcePicker picker, TowerDefenseProjectileCreateData value, Action<TowerDefenseProjectileCreateData> changed)
	{
		picker.Setup("TowerDefenseProjectileCreateData");
		picker.SetEditedResource(value);
		picker.ResourceChanged += (Resource resource) =>
		{
			changed?.Invoke(resource as TowerDefenseProjectileCreateData);
		};
	}

	private void BuildFireCollisionFlagGrid(GridContainer grid, FireComponentCheckConfig check)
	{
		TowerDefenseEnum.CHARACTER_COLLISION_FLAGS[] values = Enum.GetValues<TowerDefenseEnum.CHARACTER_COLLISION_FLAGS>();
		for (int i = 0; i < values.Length; i++)
		{
			TowerDefenseEnum.CHARACTER_COLLISION_FLAGS cHARACTER_COLLISION_FLAGS = values[i];
			int flag = Convert.ToInt32(cHARACTER_COLLISION_FLAGS);
			CheckButton checkButton = new CheckButton
			{
				Text = cHARACTER_COLLISION_FLAGS.ToString(),
				ButtonPressed = ((check.collisionFlags & flag) == flag),
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			checkButton.Toggled += (bool pressed) =>
			{
				int collisionFlags = check.collisionFlags;
				collisionFlags = (pressed ? (collisionFlags | flag) : (collisionFlags & ~flag));
				_projectilePropertyBinding.SetValue(check, "collisionFlags", collisionFlags, "修改检测碰撞阵营");
			};
			grid.AddChild(checkButton, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void RefreshFireProjectilePreview()
	{
		RefreshPipelineCreateDataPreview();
		if (CurrentResource is FireComponentFireProjectileConfig fireComponentFireProjectileConfig && GodotObject.IsInstanceValid(_pipelineStatusLabel))
		{
			_pipelineStatusLabel.Text = $"确定性预览：发射点 {fireComponentFireProjectileConfig.firePosId}，速度 {fireComponentFireProjectileConfig.speed:0.##} px/s，方向 {fireComponentFireProjectileConfig.dir:0.##}°，行偏移 {fireComponentFireProjectileConfig.offsetLine}";
		}
	}

	private void BindCreateDataCoreFields(Control root, TowerDefenseProjectileCreateData createData)
	{
		EnsureProjectilePropertyBinding();
		Button node = root.GetNode<Button>("%SelectProjectileButton");
		node.Visible = true;
		node.Pressed += () =>
		{
			OpenCreateDataProjectilePicker(createData, root);
		};
		LineEdit node2 = root.GetNode<LineEdit>("%ProjectileNameLineEdit");
		node2.Text = createData.projectileName.ToString();
		node2.Editable = false;
		node2.TooltipText = "使用右上角“从图鉴选择”以图像方式选择子弹";
		root.GetNode<Control>("%NameRow").Visible = true;
		BindTextIfPresent(root, "%SkinRow", root.GetNode<LineEdit>("%SkinNameLineEdit"), createData, "skinName", RefreshPipelineCreateDataPreview);
		BindNumberIfPresent(root, "%DamageRow", root.GetNode<SpinBox>("%BaseDamageSpinBox"), createData, "baseDamage", RefreshPipelineCreateDataPreview);
		BindVector2IfPresent(root, "%SizeRow", root.GetNode<SpinBox>("%SizeXSpinBox"), root.GetNode<SpinBox>("%SizeYSpinBox"), createData, "size", RefreshPipelineCreateDataPreview);
		BindVector2IfPresent(root, "%ScaleRow", root.GetNode<SpinBox>("%ScaleXSpinBox"), root.GetNode<SpinBox>("%ScaleYSpinBox"), createData, "scale", RefreshPipelineCreateDataPreview);
		root.GetNode<Control>("%ObjectRow").Visible = false;
		root.GetNode<Control>("%ProjectileSceneRow").Visible = false;
		root.GetNode<Control>("%SplatTypeRow").Visible = false;
		root.GetNode<Control>("%SplatAudioRow").Visible = false;
		root.GetNode<Control>("%SplatSceneRow").Visible = false;
		root.GetNode<Control>("%HitEffectRow").Visible = false;
		_damageLabel = root.GetNode<Label>("%ProjectileDamageSummary");
		_damageLabel.Text = $"覆盖伤害：{createData.baseDamage:0.##} · 尺寸 {FormatVector(createData.size)} · 缩放 {FormatVector(createData.scale)}";
	}

	private void BindProjectileBehaviorFields(PanelContainer root, ProjectileBehaviorDefinition behavior)
	{
		EnsureProjectilePropertyBinding();
		root.GetNode<Label>("%PreviewProjectileLabel").Text = "预览子弹：" + _pipelinePreviewProjectileKey;
		root.GetNode<Button>("%SelectPreviewProjectileButton").Pressed += () =>
		{
			OpenBehaviorPreviewProjectilePicker(root);
		};
		SpinBox node = root.GetNode<SpinBox>("%StrengthSpinBox");
		SpinBox node2 = root.GetNode<SpinBox>("%SpeedSpinBox");
		Control node3 = root.GetNode<Control>("%StrengthRow");
		if (root.GetNode<Control>("%SpeedRow").Visible = (node3.Visible = behavior is ProjectileBehaviorYMoveSin))
		{
			_projectilePropertyBinding.BindNumber(node, behavior, "Strength", RefreshPipelineCreateDataPreview);
			_projectilePropertyBinding.BindNumber(node2, behavior, "Speed", RefreshPipelineCreateDataPreview);
			root.GetNode<Label>("%BehaviorWarning").Text = "已识别正弦轨迹：只读取 Strength / Speed 绘制曲线，不调用旧式逐子弹回调。";
		}
		else
		{
			root.GetNode<Label>("%BehaviorWarning").Text = behavior.GetType().Name + " 尚无专用轨迹适配器；不会执行行为脚本。";
		}
	}

	private void OpenCreateDataProjectilePicker(TowerDefenseProjectileCreateData createData, Control coreFields)
	{
		EnsureProjectilePicker();
		_projectilePicker?.Open(XWGameplayResourceKind.Projectile, createData.projectileName.ToString(), (XWGameplayResourceChoice choice) =>
		{
			if (!(choice == null) && !string.IsNullOrWhiteSpace(choice.Key))
			{
				EnsureProjectilePropertyBinding();
				_projectilePropertyBinding.SetValue(createData, "projectileName", Variant.From<StringName>(new StringName(choice.Key)), "选择预览子弹");
				createData.InvalidateConfigCache();
				_pipelinePreviewProjectileKey = choice.Key;
				if (GodotObject.IsInstanceValid(coreFields))
				{
					coreFields.GetNode<LineEdit>("%ProjectileNameLineEdit").Text = choice.Key;
				}
				RefreshPipelineCreateDataPreview();
			}
		});
	}

	private void OpenBehaviorPreviewProjectilePicker(PanelContainer behaviorFields)
	{
		EnsureProjectilePicker();
		_projectilePicker?.Open(XWGameplayResourceKind.Projectile, _pipelinePreviewProjectileKey, (XWGameplayResourceChoice choice) =>
		{
			if (!(choice == null) && !string.IsNullOrWhiteSpace(choice.Key))
			{
				_pipelinePreviewProjectileKey = choice.Key;
				if (GodotObject.IsInstanceValid(behaviorFields))
				{
					behaviorFields.GetNode<Label>("%PreviewProjectileLabel").Text = "预览子弹：" + choice.Key;
				}
				RefreshPipelineCreateDataPreview();
			}
		});
	}

	private void EnsureProjectilePicker()
	{
		if (!GodotObject.IsInstanceValid(_projectilePicker))
		{
			_projectilePicker = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_projectilePicker))
			{
				AddChild(_projectilePicker, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void OpenProjectileObjectCatalog(OptionButton option, Button catalogButton)
	{
		EnsureProjectilePicker();
		List<XWGameplayResourceChoice> list = new List<XWGameplayResourceChoice>();
		ObjectManagerConfig.OBJECT[] values = Enum.GetValues<ObjectManagerConfig.OBJECT>();
		for (int i = 0; i < values.Length; i++)
		{
			ObjectManagerConfig.OBJECT oBJECT = values[i];
			int num = (int)oBJECT;
			list.Add(new XWGameplayResourceChoice(num.ToString(), oBJECT.ToString(), $"OBJECT ID {(int)oBJECT}", ResourceLoader.Load<Texture2D>(ResolveProjectileObjectIcon(oBJECT), null, ResourceLoader.CacheMode.Reuse), XWGameplayResourceKind.Resource, IsModResource: false));
		}
		_projectilePicker?.OpenChoices("对象池图鉴", "搜索对象池名称或 ID；隐藏选项保留原始枚举语义", option.GetSelectedId().ToString(), list, (XWGameplayResourceChoice choice) =>
		{
			if (int.TryParse(choice?.Key, out var result))
			{
				int itemIndex = option.GetItemIndex(result);
				if (itemIndex >= 0)
				{
					option.Select(itemIndex);
					catalogButton.Text = $"对象池图鉴 · {(ObjectManagerConfig.OBJECT)result}";
					option.EmitSignal(OptionButton.SignalName.ItemSelected, itemIndex);
				}
			}
		});
	}

	private static string ResolveProjectileObjectIcon(ObjectManagerConfig.OBJECT value)
	{
		string text = value.ToString();
		if (text.Contains("PROJECTILE", StringComparison.OrdinalIgnoreCase))
		{
			return "res://addons/ModEditor/Icons/ResourceProjectile.svg";
		}
		if (text.Contains("COIN", StringComparison.OrdinalIgnoreCase) || text.Contains("SUN", StringComparison.OrdinalIgnoreCase))
		{
			return "res://addons/ModEditor/Icons/ResourceCollectable.svg";
		}
		return "res://addons/ModEditor/Icons/ResourceCard.svg";
	}

	private void MountProjectileSegmentedOption(OptionButton option, HFlowContainer host)
	{
		if (!GodotObject.IsInstanceValid(option) || !GodotObject.IsInstanceValid(host) || option.ItemCount < 2 || option.ItemCount > 8)
		{
			if (GodotObject.IsInstanceValid(option))
			{
				option.Visible = true;
			}
		}
		else
		{
			XWVisualSegmentedOption xWVisualSegmentedOption = new XWVisualSegmentedOption(option, host);
			xWVisualSegmentedOption.Rebuild();
			_projectileSegmentedOptions.Add(xWVisualSegmentedOption);
		}
	}

	private void DisposeProjectileVisualChoices()
	{
		foreach (XWVisualSegmentedOption projectileSegmentedOption in _projectileSegmentedOptions)
		{
			projectileSegmentedOption?.Dispose();
		}
		_projectileSegmentedOptions.Clear();
	}

	private void BindPipelineGamePreview(VBoxContainer root, Resource resource)
	{
		_pipelineProjectileRoot = root.GetNode<Node2D>("%ProjectileRoot");
		_pipelineTargetRoot = root.GetNode<Node2D>("%TargetRoot");
		_pipelineTrajectoryLine = root.GetNode<Line2D>("%TrajectoryLine");
		_pipelineTimelineSlider = root.GetNode<HSlider>("%PipelineTimelineSlider");
		_pipelinePlayButton = root.GetNode<Button>("%PipelinePlayButton");
		_pipelinePreviewStatus = root.GetNode<Label>("%PipelinePreviewStatus");
		_pipelineStatusLabel = root.GetNode<Label>("%PipelineStatusLabel");
		_pipelineCreateData = ResolvePipelineCreateData(resource);
		_pipelineBehavior = (resource as ProjectileBehaviorDefinition) ?? FindSinBehavior(_pipelineCreateData);
		_pipelinePreviewProgress = 0.0;
		_pipelinePlayButton.Pressed += PlayPipelinePreview;
		_pipelineTimelineSlider.ValueChanged += (double value) =>
		{
			_pipelinePreviewProgress = value;
			_pipelinePreviewPlaying = false;
			UpdatePipelinePlaybackVisual();
			RequestVisibilityGatedProcessing(requested: false);
		};
		RebuildPipelineTargetPreview();
		RefreshPipelineCreateDataPreview();
		RefreshVisibilityGatedProcessing();
	}

	private TowerDefenseProjectileCreateData ResolvePipelineCreateData(Resource resource)
	{
		if (!(resource is TowerDefenseProjectileCreateData result))
		{
			if (!(resource is FireComponentProjectileSingle { projectileData: var projectileData }))
			{
				if (!(resource is FireComponentProjectileWeight weight))
				{
					if (!(resource is FireComponentProjectileWeightItem fireComponentProjectileWeightItem))
					{
						if (!(resource is FireComponentCheckConfig fireComponentCheckConfig))
						{
							if (!(resource is ProjectileBehaviorDefinition))
							{
								if (resource is FireComponentFireProjectileConfig)
								{
									return new TowerDefenseProjectileCreateData(new StringName(_pipelinePreviewProjectileKey));
								}
								return null;
							}
							return new TowerDefenseProjectileCreateData(new StringName(_pipelinePreviewProjectileKey));
						}
						return ResolvePipelineCreateData(fireComponentCheckConfig.projectile);
					}
					return ResolvePipelineCreateData(fireComponentProjectileWeightItem.projectileResource);
				}
				return ResolveWeightPreviewData(weight);
			}
			return projectileData;
		}
		return result;
	}

	private TowerDefenseProjectileCreateData ResolveWeightPreviewData(FireComponentProjectileWeight weight)
	{
		if (!GodotObject.IsInstanceValid(weight))
		{
			return null;
		}
		if (weight == _pipelineWeightResource && CurrentResource == weight && _selectedPipelineWeightIndex >= 0 && weight.projectileWeight != null && _selectedPipelineWeightIndex < weight.projectileWeight.Count)
		{
			FireComponentProjectileWeightItem fireComponentProjectileWeightItem = weight.projectileWeight[_selectedPipelineWeightIndex];
			if (GodotObject.IsInstanceValid(fireComponentProjectileWeightItem) && GodotObject.IsInstanceValid(fireComponentProjectileWeightItem.projectileResource))
			{
				return ResolvePipelineCreateData(fireComponentProjectileWeightItem.projectileResource);
			}
		}
		return weight.projectileData;
	}

	private void RefreshPipelineCreateDataPreview()
	{
		if (GodotObject.IsInstanceValid(_pipelineProjectileRoot))
		{
			Resource currentResource = CurrentResource;
			if ((currentResource is ProjectileBehaviorDefinition || currentResource is FireComponentFireProjectileConfig) ? true : false)
			{
				_pipelineCreateData = new TowerDefenseProjectileCreateData(new StringName(_pipelinePreviewProjectileKey));
			}
			else
			{
				_pipelineCreateData = ResolvePipelineCreateData(CurrentResource);
			}
			_pipelineBehavior = (CurrentResource as ProjectileBehaviorDefinition) ?? FindSinBehavior(_pipelineCreateData);
			_pipelinePreviewConfig = ResolveCreateDataPreviewConfig(_pipelineCreateData, out var status);
			RebuildPipelineProjectilePreview();
			RebuildPipelineTrajectory();
			UpdatePipelinePlaybackVisual();
			if (GodotObject.IsInstanceValid(_pipelineStatusLabel))
			{
				_pipelineStatusLabel.Text = status;
			}
		}
	}

	private static TowerDefenseProjectileConfig ResolveCreateDataPreviewConfig(TowerDefenseProjectileCreateData createData, out string status)
	{
		status = "安全预览：未配置子弹，不执行游戏逻辑";
		if (!GodotObject.IsInstanceValid(createData) || createData.projectileName.IsEmpty)
		{
			return null;
		}
		TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = createData.Duplicate(deep: true) as TowerDefenseProjectileCreateData;
		if (!GodotObject.IsInstanceValid(towerDefenseProjectileCreateData))
		{
			return null;
		}
		towerDefenseProjectileCreateData.InvalidateConfigCache();
		string text = towerDefenseProjectileCreateData.projectileName.ToString();
		Resource resource = LoadProjectileRegistryResource(text);
		TowerDefenseProjectileConfig towerDefenseProjectileConfig2;
		if (resource is TowerDefenseProjectileConfig towerDefenseProjectileConfig)
		{
			towerDefenseProjectileConfig2 = towerDefenseProjectileConfig.Duplicate(deep: true) as TowerDefenseProjectileConfig;
		}
		else
		{
			towerDefenseProjectileConfig2 = ((!(resource is TowerDefenseProjectileData data)) ? null : BuildPreviewConfigFromData(data));
		}
		TowerDefenseProjectileConfig towerDefenseProjectileConfig3 = towerDefenseProjectileConfig2;
		if (!GodotObject.IsInstanceValid(towerDefenseProjectileConfig3))
		{
			status = "安全预览：找不到子弹 " + text + "，仅显示缺失提示";
			return null;
		}
		towerDefenseProjectileConfig3.name = (string.IsNullOrWhiteSpace(towerDefenseProjectileConfig3.name) ? text : towerDefenseProjectileConfig3.name);
		towerDefenseProjectileConfig3.skinName = towerDefenseProjectileCreateData.skinName;
		towerDefenseProjectileCreateData.ApplyOverride(towerDefenseProjectileConfig3);
		status = "安全预览：" + text + " 已从复制体解析；不会回写继承字段或执行方法/事件";
		return towerDefenseProjectileConfig3;
	}

	private static Resource LoadProjectileRegistryResource(string projectileKey)
	{
		if (string.IsNullOrWhiteSpace(projectileKey) || !ResourceLoader.Exists("res://Registry/Projectile/ProjectileRegistry.json"))
		{
			return null;
		}
		Json json = ResourceLoader.Load<Json>("res://Registry/Projectile/ProjectileRegistry.json", "", ResourceLoader.CacheMode.Ignore);
		if (!GodotObject.IsInstanceValid(json) || json.Data.VariantType != Variant.Type.Dictionary)
		{
			return null;
		}
		Dictionary dictionary = (Dictionary)json.Data.AsGodotDictionary().GetValueOrDefault("Projectiles", new Dictionary());
		string text = "";
		foreach (Variant key in dictionary.Keys)
		{
			if (string.Equals(key.AsString(), projectileKey, StringComparison.OrdinalIgnoreCase))
			{
				text = dictionary[key].AsString();
				break;
			}
		}
		if (string.IsNullOrWhiteSpace(text) || !ResourceLoader.Exists(text))
		{
			return null;
		}
		return ResourceLoader.Load<Resource>(text, "", ResourceLoader.CacheMode.Reuse);
	}

	private static TowerDefenseProjectileConfig BuildPreviewConfigFromData(TowerDefenseProjectileData data)
	{
		if (!GodotObject.IsInstanceValid(data))
		{
			return null;
		}
		return new TowerDefenseProjectileConfig
		{
			name = data.name,
			baseDamage = data.baseDamage,
			size = data.size,
			scale = data.scale,
			projectileScene = data.projectileScene,
			splatAudio = data.splatAudio,
			splatScene = data.splatScene,
			hitEffect = data.hitEffect,
			hitTargetEventList = data.hitTargetEventList,
			hitCharacterEventList = data.hitCharacterEventList,
			hitGroundEventList = data.hitGroundEventList,
			blockHurt = data.blockHurt,
			rotateFollowVelocity = data.rotateFollowVelocity,
			rotateScale = data.rotateScale,
			hitBody = data.hitBody,
			rangeType = data.rangeType,
			useRange = data.useRange,
			rangeSize = data.rangeSize,
			hitPesontage = data.hitPesontage,
			damageFlags = (data.isFire ? 4 : 2)
		};
	}

	private void RebuildPipelineProjectilePreview()
	{
		if (!GodotObject.IsInstanceValid(_pipelineProjectileRoot))
		{
			return;
		}
		PackedScene packedScene = _pipelinePreviewConfig?.projectileScene;
		if (GodotObject.IsInstanceValid(_pipelineProjectileInstance) && packedScene == _pipelineResolvedScene)
		{
			ApplyPipelineProjectileScale(_pipelineProjectileInstance, GetPipelineProjectilePreviewScale());
			return;
		}
		foreach (Node child in _pipelineProjectileRoot.GetChildren())
		{
			child.QueueFree();
		}
		_pipelineProjectileInstance = null;
		_pipelineResolvedScene = packedScene;
		if (GodotObject.IsInstanceValid(packedScene))
		{
			try
			{
				Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
				node.Name = "PipelineProjectilePreview";
				TowerDefenseProjectile towerDefenseProjectile = FindRuntimeProjectile(node);
				if (towerDefenseProjectile != null)
				{
					towerDefenseProjectile.suppressGameplay = true;
				}
				ApplyPipelineProjectileScale(node, GetPipelineProjectilePreviewScale());
				_pipelineProjectileRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
				_pipelineProjectileInstance = node;
			}
			catch (Exception ex)
			{
				GD.PushWarning("Pipeline projectile preview failed: " + ex.Message);
			}
		}
		if (!GodotObject.IsInstanceValid(_pipelineProjectileInstance))
		{
			Polygon2D polygon2D = new Polygon2D();
			polygon2D.Name = "MissingProjectilePreview";
			polygon2D.Polygon = new Vector2[4]
			{
				new Vector2(-12f, -12f),
				new Vector2(12f, -12f),
				new Vector2(12f, 12f),
				new Vector2(-12f, 12f)
			};
			polygon2D.Color = new Color(0.95f, 0.18f, 0.18f, 0.9f);
			Polygon2D polygon2D2 = polygon2D;
			_pipelineProjectileRoot.AddChild(polygon2D2, forceReadableName: false, InternalMode.Disabled);
			_pipelineProjectileInstance = polygon2D2;
		}
	}

	private static void ApplyPipelineProjectileScale(Node instance, Vector2 scale)
	{
		if (instance is Node2D node2D)
		{
			node2D.Scale = scale;
		}
		else if (instance is Control control)
		{
			control.Scale = scale;
		}
	}

	private Vector2 GetPipelineProjectilePreviewScale()
	{
		Vector2 result = _pipelinePreviewConfig?.scale ?? Vector2.One;
		if (CurrentResource is FireComponentFireProjectileConfig { projectileFlip: not false })
		{
			result.X = 0f - Mathf.Abs(result.X);
		}
		return result;
	}

	private void RebuildPipelineTargetPreview()
	{
		if (!GodotObject.IsInstanceValid(_pipelineTargetRoot))
		{
			return;
		}
		foreach (Node child in _pipelineTargetRoot.GetChildren())
		{
			child.QueueFree();
		}
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Puzzle/Target/Scene/TowerDefenseZombieTarget.tscn", null, ResourceLoader.CacheMode.Reuse);
		if (GodotObject.IsInstanceValid(packedScene))
		{
			Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			TowerDefenseCharacter towerDefenseCharacter = FindRuntimeCharacter(node);
			if (towerDefenseCharacter != null)
			{
				towerDefenseCharacter.inGame = false;
				towerDefenseCharacter.editorPreviewMode = true;
			}
			_pipelineTargetRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void RebuildPipelineTrajectory()
	{
		if (GodotObject.IsInstanceValid(_pipelineTrajectoryLine))
		{
			Vector2[] array = new Vector2[41];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = EvaluatePipelinePosition((float)i / (float)(array.Length - 1));
			}
			_pipelineTrajectoryLine.Points = array;
		}
	}

	private Vector2 EvaluatePipelinePosition(float progress)
	{
		Vector2 vector = new Vector2(108f, 164f);
		Vector2 to = new Vector2(602f, 148f);
		if (CurrentResource is FireComponentFireProjectileConfig fireComponentFireProjectileConfig)
		{
			vector.Y += (float)fireComponentFireProjectileConfig.offsetLine * 42f;
			Vector2 vector2 = new Vector2(494f, 0f).Rotated(Mathf.DegToRad(fireComponentFireProjectileConfig.dir));
			to = vector + vector2;
		}
		int num = _pipelinePreviewConfig?.fireMethodFlags ?? _pipelineCreateData?.fireMethodFlags ?? 1;
		bool flag = (num & 8) != 0;
		float num2 = progress;
		if (flag)
		{
			num2 = ((progress <= 0.55f) ? (progress / 0.55f) : (1f - (progress - 0.55f) / 0.45f));
		}
		Vector2 result = vector.Lerp(to, Mathf.Clamp(num2, 0f, 1f));
		if ((num & 2) != 0)
		{
			float num3 = Mathf.Clamp((float)(_pipelinePreviewConfig?.catapultHeight ?? _pipelineCreateData?.catapultHeight ?? 300.0), 30f, 420f);
			result.Y -= Mathf.Sin(num2 * (float)Math.PI) * num3 * 0.34f;
		}
		if (_pipelineBehavior is ProjectileBehaviorYMoveSin projectileBehaviorYMoveSin)
		{
			result.Y += Mathf.Sin(progress * (float)projectileBehaviorYMoveSin.Speed * 1.2f) * (float)projectileBehaviorYMoveSin.Strength;
		}
		return result;
	}

	private static ProjectileBehaviorDefinition FindSinBehavior(TowerDefenseProjectileCreateData createData)
	{
		if (!GodotObject.IsInstanceValid(createData) || createData.behaviors == null)
		{
			return null;
		}
		foreach (ProjectileBehaviorDefinition behavior in createData.behaviors)
		{
			if (behavior is ProjectileBehaviorYMoveSin)
			{
				return behavior;
			}
		}
		return null;
	}

	private static string GetProjectilePreviewKey(Resource resource)
	{
		if (!(resource is TowerDefenseProjectileCreateData towerDefenseProjectileCreateData))
		{
			if (!(resource is TowerDefenseProjectileConfig { name: var name }))
			{
				if (!(resource is TowerDefenseProjectileData { name: var name2 }))
				{
					if (!(resource is FireComponentProjectileSingle { projectileData: var projectileData }))
					{
						if (!(resource is FireComponentProjectileWeight { projectileData: var projectileData2 }))
						{
							if (!(resource is FireComponentProjectileWeightItem fireComponentProjectileWeightItem))
							{
								if (resource is FireComponentCheckConfig fireComponentCheckConfig)
								{
									return GetProjectilePreviewKey(fireComponentCheckConfig.projectile);
								}
								return "";
							}
							return GetProjectilePreviewKey(fireComponentProjectileWeightItem.projectileResource);
						}
						return projectileData2?.projectileName.ToString() ?? "";
					}
					return projectileData?.projectileName.ToString() ?? "";
				}
				return name2;
			}
			return name;
		}
		return towerDefenseProjectileCreateData.projectileName.ToString();
	}

	private static void ConfigurePipelineNode(PanelContainer node, bool active, string detail)
	{
		if (GodotObject.IsInstanceValid(node))
		{
			node.Modulate = (active ? Colors.White : new Color(0.46f, 0.5f, 0.52f, 0.72f));
			Label nodeOrNull = node.GetNodeOrNull<Label>("Layout/Detail");
			if (GodotObject.IsInstanceValid(nodeOrNull) && !string.IsNullOrWhiteSpace(detail))
			{
				nodeOrNull.Text = detail;
			}
		}
	}

	private void PopulatePipelineBranches(HBoxContainer branchHost, Resource resource)
	{
		if (GodotObject.IsInstanceValid(branchHost))
		{
			if (resource is FireComponentProjectileWeight weight)
			{
				PopulateEditablePipelineWeightBranches(branchHost, weight);
				return;
			}
			string text;
			if (resource is TowerDefenseProjectileCreateData data)
			{
				text = DescribeCreateData(data);
			}
			else if (resource is FireComponentProjectileSingle fireComponentProjectileSingle)
			{
				text = DescribeCreateData(fireComponentProjectileSingle.projectileData);
			}
			else if (resource is FireComponentProjectileWeightItem fireComponentProjectileWeightItem)
			{
				text = DescribeProjectileResource(fireComponentProjectileWeightItem.projectileResource);
			}
			else
			{
				text = ((!(resource is FireComponentCheckConfig fireComponentCheckConfig)) ? resource.GetType().Name : DescribeProjectileResource(fireComponentCheckConfig.projectile));
			}
			string text2 = text;
			string warning = ((text2 == "未配置子弹") ? "需要选择子弹资源" : "确定性预览，不执行随机或伤害");
			AddPipelineBranchCard(branchHost, text2, "概率：100%", warning);
		}
	}

	private void AddPipelineBranchCard(HBoxContainer branchHost, string projectileName, string probability, string warning)
	{
		PanelContainer panelContainer = _pipelineBranchScene?.Instantiate<PanelContainer>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(panelContainer))
		{
			panelContainer.GetNode<Label>("%ProjectileName").Text = (string.IsNullOrWhiteSpace(projectileName) ? "未配置子弹" : projectileName);
			panelContainer.GetNode<Label>("%Probability").Text = probability ?? "概率：--";
			Label node = panelContainer.GetNode<Label>("%Warning");
			node.Text = warning ?? "";
			node.Visible = !string.IsNullOrWhiteSpace(warning);
			branchHost.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void PopulateEditablePipelineWeightBranches(HBoxContainer branchHost, FireComponentProjectileWeight weight)
	{
		_pipelineWeightResource = weight;
		Array<FireComponentProjectileWeightItem> projectileWeight = weight.projectileWeight;
		if (projectileWeight == null || projectileWeight.Count == 0)
		{
			AddPipelineBranchCard(branchHost, DescribeCreateData(weight.projectileData), "概率：100%", "权重池为空，使用默认子弹");
			return;
		}
		int num = 0;
		double num2 = 0.0;
		for (int i = 0; i < projectileWeight.Count; i++)
		{
			FireComponentProjectileWeightItem fireComponentProjectileWeightItem = projectileWeight[i];
			if (GodotObject.IsInstanceValid(fireComponentProjectileWeightItem) && GodotObject.IsInstanceValid(fireComponentProjectileWeightItem.projectileResource))
			{
				num++;
				num2 += Math.Max(0.0, fireComponentProjectileWeightItem.weight);
			}
		}
		for (int j = 0; j < projectileWeight.Count; j++)
		{
			FireComponentProjectileWeightItem fireComponentProjectileWeightItem2 = projectileWeight[j];
			bool flag = GodotObject.IsInstanceValid(fireComponentProjectileWeightItem2) && GodotObject.IsInstanceValid(fireComponentProjectileWeightItem2.projectileResource);
			double value;
			if (flag)
			{
				if (!weight.averageWeight && !(num2 <= 0.0))
				{
					value = Math.Max(0.0, fireComponentProjectileWeightItem2.weight) / num2;
				}
				else
				{
					value = ((num > 0) ? (1.0 / (double)num) : 0.0);
				}
			}
			else
			{
				value = 0.0;
			}
			AddPipelineBranchCard(branchHost, flag ? DescribeProjectileResource(fireComponentProjectileWeightItem2.projectileResource) : "未配置子弹", $"概率：{value:P1}", flag ? $"权重 {fireComponentProjectileWeightItem2.weight:0.##}" : "缺少子弹引用");
			ConfigurePipelineWeightBranchCard(branchHost.GetChild<PanelContainer>(branchHost.GetChildCount() - 1), weight, fireComponentProjectileWeightItem2, j);
		}
	}

	private void ConfigurePipelineWeightBranchCard(PanelContainer card, FireComponentProjectileWeight weight, FireComponentProjectileWeightItem item, int index)
	{
		if (GodotObject.IsInstanceValid(card))
		{
			card.SetMeta("weight_index", index);
			SpinBox node = card.GetNode<SpinBox>("%WeightSpinBox");
			Button node2 = card.GetNode<Button>("%SelectButton");
			Button node3 = card.GetNode<Button>("%OpenButton");
			node.Visible = GodotObject.IsInstanceValid(item);
			node2.Visible = true;
			node3.Visible = true;
			node3.Disabled = !GodotObject.IsInstanceValid(item);
			node2.Pressed += () =>
			{
				SelectPipelineWeightBranch(index);
			};
			node3.Pressed += () =>
			{
				OpenFirePipelineNestedResource(item, weight, "projectileWeight", index);
			};
			if (GodotObject.IsInstanceValid(item))
			{
				_projectilePropertyBinding.BindNumber(node, item, "weight", RefreshPipelineWeightProbabilities);
			}
		}
	}

	private void RefreshPipelineWeightProbabilities()
	{
		FireComponentProjectileWeight pipelineWeightResource = _pipelineWeightResource;
		if (!GodotObject.IsInstanceValid(pipelineWeightResource) || pipelineWeightResource.projectileWeight == null)
		{
			return;
		}
		int num = 0;
		double num2 = 0.0;
		for (int i = 0; i < pipelineWeightResource.projectileWeight.Count; i++)
		{
			FireComponentProjectileWeightItem fireComponentProjectileWeightItem = pipelineWeightResource.projectileWeight[i];
			if (GodotObject.IsInstanceValid(fireComponentProjectileWeightItem) && GodotObject.IsInstanceValid(fireComponentProjectileWeightItem.projectileResource))
			{
				num++;
				num2 += Math.Max(0.0, fireComponentProjectileWeightItem.weight);
			}
		}
		if (GodotObject.IsInstanceValid(_pipelineBranchHost))
		{
			foreach (Node child in _pipelineBranchHost.GetChildren())
			{
				if (!(child is PanelContainer panelContainer) || !panelContainer.HasMeta("weight_index"))
				{
					continue;
				}
				int num3 = panelContainer.GetMeta("weight_index").AsInt32();
				if (num3 >= 0 && num3 < pipelineWeightResource.projectileWeight.Count)
				{
					FireComponentProjectileWeightItem fireComponentProjectileWeightItem2 = pipelineWeightResource.projectileWeight[num3];
					bool flag = GodotObject.IsInstanceValid(fireComponentProjectileWeightItem2) && GodotObject.IsInstanceValid(fireComponentProjectileWeightItem2.projectileResource);
					double value;
					if (flag)
					{
						if (!pipelineWeightResource.averageWeight && !(num2 <= 0.0))
						{
							value = Math.Max(0.0, fireComponentProjectileWeightItem2.weight) / num2;
						}
						else
						{
							value = ((num > 0) ? (1.0 / (double)num) : 0.0);
						}
					}
					else
					{
						value = 0.0;
					}
					panelContainer.GetNode<Label>("%ProjectileName").Text = (flag ? DescribeProjectileResource(fireComponentProjectileWeightItem2.projectileResource) : "未配置子弹");
					panelContainer.GetNode<Label>("%Probability").Text = $"概率：{value:P1}";
					Label node = panelContainer.GetNode<Label>("%Warning");
					node.Text = (flag ? $"权重 {fireComponentProjectileWeightItem2.weight:0.##}" : "缺少子弹引用");
					node.Visible = true;
					if (GodotObject.IsInstanceValid(fireComponentProjectileWeightItem2))
					{
						panelContainer.GetNode<SpinBox>("%WeightSpinBox").SetValueNoSignal(fireComponentProjectileWeightItem2.weight);
					}
				}
			}
		}
		RefreshPipelineWeightList();
	}

	private void RefreshPipelineWeightList()
	{
		if (!GodotObject.IsInstanceValid(_pipelineWeightList) || !GodotObject.IsInstanceValid(_pipelineWeightResource))
		{
			return;
		}
		_pipelineWeightList.Clear();
		Array<FireComponentProjectileWeightItem> projectileWeight = _pipelineWeightResource.projectileWeight;
		if (projectileWeight == null || projectileWeight.Count == 0)
		{
			_pipelineWeightList.AddItem("暂无权重分支；预览使用默认子弹");
			_selectedPipelineWeightIndex = -1;
			return;
		}
		for (int i = 0; i < projectileWeight.Count; i++)
		{
			FireComponentProjectileWeightItem from = projectileWeight[i];
			string value = (GodotObject.IsInstanceValid(from) ? DescribeProjectileResource(from.projectileResource) : "缺失条目");
			double value2 = (GodotObject.IsInstanceValid(from) ? ((double)from.weight) : 0.0);
			int idx = _pipelineWeightList.AddItem($"{i + 1}. {value}    权重 {value2:0.##}");
			_pipelineWeightList.SetItemMetadata(idx, Variant.From(in from));
		}
		if (_selectedPipelineWeightIndex >= projectileWeight.Count)
		{
			_selectedPipelineWeightIndex = projectileWeight.Count - 1;
		}
		if (_selectedPipelineWeightIndex >= 0)
		{
			_pipelineWeightList.Select(_selectedPipelineWeightIndex);
		}
	}

	private void SelectPipelineWeightBranch(int index)
	{
		if (GodotObject.IsInstanceValid(_pipelineWeightResource) && _pipelineWeightResource.projectileWeight != null && index >= 0 && index < _pipelineWeightResource.projectileWeight.Count)
		{
			_selectedPipelineWeightIndex = index;
			if (GodotObject.IsInstanceValid(_pipelineWeightList))
			{
				_pipelineWeightList.Select(index);
			}
			RefreshPipelineCreateDataPreview();
		}
	}

	private void OpenPipelineWeightItem(int index)
	{
		if (GodotObject.IsInstanceValid(_pipelineWeightResource) && _pipelineWeightResource.projectileWeight != null && index >= 0 && index < _pipelineWeightResource.projectileWeight.Count)
		{
			OpenFirePipelineNestedResource(_pipelineWeightResource.projectileWeight[index], _pipelineWeightResource, "projectileWeight", index);
		}
	}

	private void AddPipelineWeightItem()
	{
		FireComponentProjectileWeight pipelineWeightResource = _pipelineWeightResource;
		if (GodotObject.IsInstanceValid(pipelineWeightResource))
		{
			Array<FireComponentProjectileWeightItem> array = ((pipelineWeightResource.projectileWeight == null) ? new Array<FireComponentProjectileWeightItem>() : new Array<FireComponentProjectileWeightItem>(pipelineWeightResource.projectileWeight));
			array.Add(new FireComponentProjectileWeightItem
			{
				ResourceLocalToScene = true,
				weight = 1f
			});
			_selectedPipelineWeightIndex = array.Count - 1;
			ReplacePipelineWeightItems(pipelineWeightResource, array, "新增权重分支");
		}
	}

	private void DuplicatePipelineWeightItem()
	{
		FireComponentProjectileWeight pipelineWeightResource = _pipelineWeightResource;
		if (TryGetSelectedPipelineWeightItem(out var item))
		{
			FireComponentProjectileWeightItem fireComponentProjectileWeightItem = (item.Duplicate(deep: true) as FireComponentProjectileWeightItem) ?? new FireComponentProjectileWeightItem
			{
				projectileResource = item.projectileResource,
				weight = item.weight
			};
			fireComponentProjectileWeightItem.ResourceLocalToScene = true;
			Array<FireComponentProjectileWeightItem> array = new Array<FireComponentProjectileWeightItem>(pipelineWeightResource.projectileWeight);
			_selectedPipelineWeightIndex = Mathf.Clamp(_selectedPipelineWeightIndex + 1, 0, array.Count);
			array.Insert(_selectedPipelineWeightIndex, fireComponentProjectileWeightItem);
			ReplacePipelineWeightItems(pipelineWeightResource, array, "复制权重分支");
		}
	}

	private void RemovePipelineWeightItem()
	{
		FireComponentProjectileWeight pipelineWeightResource = _pipelineWeightResource;
		if (TryGetSelectedPipelineWeightItem(out var _))
		{
			Array<FireComponentProjectileWeightItem> array = new Array<FireComponentProjectileWeightItem>(pipelineWeightResource.projectileWeight);
			array.RemoveAt(_selectedPipelineWeightIndex);
			_selectedPipelineWeightIndex = ((array.Count == 0) ? (-1) : Mathf.Min(_selectedPipelineWeightIndex, array.Count - 1));
			ReplacePipelineWeightItems(pipelineWeightResource, array, "删除权重分支");
		}
	}

	private void MovePipelineWeightItem(int direction)
	{
		FireComponentProjectileWeight pipelineWeightResource = _pipelineWeightResource;
		if (TryGetSelectedPipelineWeightItem(out var item))
		{
			int num = _selectedPipelineWeightIndex + Math.Sign(direction);
			if (num >= 0 && num < pipelineWeightResource.projectileWeight.Count)
			{
				Array<FireComponentProjectileWeightItem> array = new Array<FireComponentProjectileWeightItem>(pipelineWeightResource.projectileWeight);
				array.RemoveAt(_selectedPipelineWeightIndex);
				array.Insert(num, item);
				_selectedPipelineWeightIndex = num;
				ReplacePipelineWeightItems(pipelineWeightResource, array, (direction < 0) ? "上移权重分支" : "下移权重分支");
			}
		}
	}

	private bool TryGetSelectedPipelineWeightItem(out FireComponentProjectileWeightItem item)
	{
		item = null;
		FireComponentProjectileWeight pipelineWeightResource = _pipelineWeightResource;
		if (!GodotObject.IsInstanceValid(pipelineWeightResource) || pipelineWeightResource.projectileWeight == null || _selectedPipelineWeightIndex < 0 || _selectedPipelineWeightIndex >= pipelineWeightResource.projectileWeight.Count)
		{
			return false;
		}
		item = pipelineWeightResource.projectileWeight[_selectedPipelineWeightIndex];
		return GodotObject.IsInstanceValid(item);
	}

	private void ReplacePipelineWeightItems(FireComponentProjectileWeight weight, Array<FireComponentProjectileWeightItem> next, string actionName)
	{
		_projectilePropertyBinding.SetValue(weight, "projectileWeight", next, actionName);
		weight.NotifyPropertyListChanged();
		RebuildPipelineWeightSurfaces(weight);
	}

	private void RebuildPipelineWeightSurfaces(FireComponentProjectileWeight weight)
	{
		RefreshPipelineBranchCards(weight);
		RefreshPipelineWeightList();
		RefreshPipelineCreateDataPreview();
	}

	private void RefreshPipelineBranchCards(Resource resource = null)
	{
		if (!GodotObject.IsInstanceValid(_pipelineBranchHost))
		{
			return;
		}
		foreach (Node child in _pipelineBranchHost.GetChildren())
		{
			_pipelineBranchHost.RemoveChild(child);
			child.QueueFree();
		}
		PopulatePipelineBranches(_pipelineBranchHost, resource ?? CurrentResource);
	}

	private void OpenFirePipelineNestedResource(Resource child, Resource owner, string propertyName, int index)
	{
		if (GodotObject.IsInstanceValid(child) && GodotObject.IsInstanceValid(owner))
		{
			string text = ((owner == CurrentResource) ? CurrentResourcePath : owner.ResourcePath);
			if (string.IsNullOrWhiteSpace(text))
			{
				text = CurrentEditContext?.OwnerPath ?? CurrentResourcePath;
			}
			XWResourceEditContext context = XWResourceEditContext.ForProperty(child, owner, child.ResourcePath, text, propertyName, index, "projectile_editor", CurrentEditContext?.IsBuiltInSource ?? XWResourceEditContext.IsBuiltInPath(text), GetProjectilePreviewKey(owner));
			XWEditorInterface.Instance?.EditResource(child, context);
		}
	}

	private static string GetPipelineTitle(Resource resource)
	{
		if (!(resource is TowerDefenseProjectileCreateData))
		{
			if (!(resource is ProjectileBehaviorDefinition))
			{
				if (!(resource is FireComponentCheckConfig))
				{
					if (!(resource is FireComponentFireProjectileConfig))
					{
						if (!(resource is FireComponentProjectileWeight))
						{
							if (!(resource is FireComponentProjectileSingle))
							{
								if (!(resource is FireComponentProjectileWeightItem))
								{
									if (resource is FireComponentProjectileResource)
									{
										return "FireComponent 子弹池";
									}
									return "子弹发射蓝图";
								}
								return "子弹权重分支";
							}
							return "单发子弹池";
						}
						return "权重子弹池 · 概率分支";
					}
					return "发射参数 · 发射点与速度";
				}
				return "发射检测 · 目标与子弹池";
			}
			return "子弹轨迹方法 · 曲线预览";
		}
		return "子弹创建数据 · 发射预览";
	}

	private static string GetPipelineParameterHint(Resource resource)
	{
		if (!(resource is FireComponentFireProjectileConfig fireComponentFireProjectileConfig))
		{
			if (!(resource is FireComponentCheckConfig fireComponentCheckConfig))
			{
				if (!(resource is FireComponentProjectileWeight fireComponentProjectileWeight))
				{
					if (!(resource is FireComponentProjectileWeightItem fireComponentProjectileWeightItem))
					{
						if (!(resource is ProjectileBehaviorDefinition projectileBehaviorDefinition))
						{
							if (resource is TowerDefenseProjectileCreateData data)
							{
								return "子弹 Key：" + DescribeCreateData(data) + "。后续参数直接在游戏画面节点上编辑。";
							}
							return "在画面节点上编辑发射参数；完整字段仍可在下方检查器中访问。";
						}
						return "行为类型：" + projectileBehaviorDefinition.GetType().Name + "。预览只读取配置，不执行 BulletField Kernel。";
					}
					return $"当前分支权重：{fireComponentProjectileWeightItem.weight:0.##}";
				}
				return fireComponentProjectileWeight.averageWeight ? "当前为等概率分支。" : "分支宽度和概率由非负权重决定。";
			}
			return fireComponentCheckConfig.useParentCollision ? "继承角色碰撞阵营；子弹池决定可发射资源。" : $"自定义碰撞 flags：{fireComponentCheckConfig.collisionFlags}";
		}
		return $"发射点 {fireComponentFireProjectileConfig.firePosId} · 速度 {fireComponentFireProjectileConfig.speed:0.##} · 方向 {fireComponentFireProjectileConfig.dir:0.##}° · 行偏移 {fireComponentFireProjectileConfig.offsetLine}";
	}

	private static string GetPipelineStatus(Resource resource)
	{
		if (resource is ProjectileBehaviorDefinition projectileBehaviorDefinition)
		{
			if (!(projectileBehaviorDefinition is ProjectileBehaviorYMoveSin))
			{
				return "安全预览：未知方法只显示类型，不执行方法代码";
			}
			return "安全预览：已识别正弦轨迹适配器，不执行方法代码";
		}
		return "安全预览：不会执行真实 FireComponent、命中事件、伤害或随机选择";
	}

	private static string GetShooterNodeDetail(Resource resource)
	{
		if (!(resource is FireComponentFireProjectileConfig fireComponentFireProjectileConfig))
		{
			return "角色 / 发射点";
		}
		return $"点 {fireComponentFireProjectileConfig.firePosId} · {fireComponentFireProjectileConfig.speed:0.#} px/s";
	}

	private static string GetCheckNodeDetail(Resource resource)
	{
		if (!(resource is FireComponentCheckConfig fireComponentCheckConfig))
		{
			return "碰撞阵营";
		}
		if (!fireComponentCheckConfig.useParentCollision)
		{
			return $"flags {fireComponentCheckConfig.collisionFlags}";
		}
		return "继承碰撞阵营";
	}

	private static string GetPoolNodeDetail(Resource resource)
	{
		if (!(resource is FireComponentProjectileWeight fireComponentProjectileWeight))
		{
			if (!(resource is FireComponentProjectileSingle))
			{
				if (!(resource is FireComponentProjectileWeightItem fireComponentProjectileWeightItem))
				{
					if (resource is FireComponentCheckConfig)
					{
						return "检测使用的子弹池";
					}
					return "单发 / 权重";
				}
				return $"分支权重 {fireComponentProjectileWeightItem.weight:0.##}";
			}
			return "单一子弹";
		}
		return fireComponentProjectileWeight.averageWeight ? "等概率权重池" : $"权重分支 {fireComponentProjectileWeight.projectileWeight?.Count ?? 0}";
	}

	private static string GetCreateNodeDetail(Resource resource)
	{
		if (!(resource is TowerDefenseProjectileCreateData data))
		{
			if (!(resource is FireComponentProjectileSingle fireComponentProjectileSingle))
			{
				if (!(resource is FireComponentProjectileWeight fireComponentProjectileWeight))
				{
					if (resource is FireComponentProjectileWeightItem fireComponentProjectileWeightItem)
					{
						return DescribeProjectileResource(fireComponentProjectileWeightItem.projectileResource);
					}
					return "Key / 皮肤 / 伤害";
				}
				return DescribeCreateData(fireComponentProjectileWeight.projectileData);
			}
			return DescribeCreateData(fireComponentProjectileSingle.projectileData);
		}
		return DescribeCreateData(data);
	}

	private static string GetBehaviorNodeDetail(Resource resource)
	{
		if (!(resource is ProjectileBehaviorDefinition projectileBehaviorDefinition))
		{
			if (resource is TowerDefenseProjectileCreateData towerDefenseProjectileCreateData)
			{
				return $"行为 {towerDefenseProjectileCreateData.behaviors?.Count ?? 0}";
			}
			return "直线 / 抛物线 / 正弦";
		}
		return projectileBehaviorDefinition.GetType().Name;
	}

	private static string DescribeCreateData(TowerDefenseProjectileCreateData data)
	{
		if (!GodotObject.IsInstanceValid(data) || data.projectileName.IsEmpty)
		{
			return "未配置子弹";
		}
		return data.projectileName.ToString();
	}

	private static string DescribeProjectileResource(FireComponentProjectileResource resource)
	{
		if (!(resource is FireComponentProjectileSingle fireComponentProjectileSingle))
		{
			if (!(resource is FireComponentProjectileWeight fireComponentProjectileWeight))
			{
				if (resource == null)
				{
					return "未配置子弹";
				}
				return resource.GetType().Name;
			}
			return DescribeCreateData(fireComponentProjectileWeight.projectileData);
		}
		return DescribeCreateData(fireComponentProjectileSingle.projectileData);
	}

	private void BindProjectileScenePreview(VBoxContainer root)
	{
		_projectileNameLabel = root.GetNode<Label>("%ProjectileNameLabel");
		_sceneLabel = root.GetNode<Label>("%SceneLabel");
		_projectileViewport = root.GetNode<SubViewport>("%ProjectileViewport");
		_projectilePreviewRoot = root.GetNode<Node2D>("%PreviewRoot");
		_projectileTargetRoot = root.GetNode<Node2D>("%TargetRoot");
		_projectileImpactRoot = root.GetNode<Node2D>("%ImpactRoot");
		_projectileTimelineSlider = root.GetNode<HSlider>("%TimelineSlider");
		_projectilePlayButton = root.GetNode<Button>("%PlayButton");
		_projectilePlaybackStatusLabel = root.GetNode<Label>("%PlaybackStatusLabel");
		_projectilePlayButton.Pressed += PlayProjectilePreview;
		_projectileTimelineSlider.ValueChanged += (double value) =>
		{
			_projectilePreviewProgress = value;
			_projectilePreviewPlaying = false;
			RequestVisibilityGatedProcessing(requested: false);
			UpdateProjectilePlaybackVisual();
		};
		RebuildProjectileTargetPreview();
		RebuildProjectileScenePreview();
		UpdateProjectileSummary();
		RefreshVisibilityGatedProcessing();
	}

	private void BindProjectileCoreFields(Control root, Resource projectile)
	{
		EnsureProjectilePropertyBinding();
		LineEdit node = root.GetNode<LineEdit>("%ProjectileNameLineEdit");
		LineEdit node2 = root.GetNode<LineEdit>("%SkinNameLineEdit");
		SpinBox node3 = root.GetNode<SpinBox>("%BaseDamageSpinBox");
		SpinBox node4 = root.GetNode<SpinBox>("%SizeXSpinBox");
		SpinBox node5 = root.GetNode<SpinBox>("%SizeYSpinBox");
		SpinBox node6 = root.GetNode<SpinBox>("%ScaleXSpinBox");
		SpinBox node7 = root.GetNode<SpinBox>("%ScaleYSpinBox");
		BindTextIfPresent(root, "%NameRow", node, projectile, "name", UpdateProjectileSummary);
		BindTextIfPresent(root, "%SkinRow", node2, projectile, "skinName", UpdateProjectileSummary);
		BindNumberIfPresent(root, "%DamageRow", node3, projectile, "baseDamage", UpdateProjectileSummary);
		BindVector2IfPresent(root, "%SizeRow", node4, node5, projectile, "size", RebuildProjectileScenePreview);
		BindVector2IfPresent(root, "%ScaleRow", node6, node7, projectile, "scale", RebuildProjectileScenePreview);
		OptionButton projectileObject = root.GetNode<OptionButton>("%ProjectileObjectOption");
		if (root.GetNode<Control>("%ObjectRow").Visible = HasProjectileProperty(projectile, "projectileObject"))
		{
			projectileObject.Clear();
			int num = ReadProjectileInt(projectile, "projectileObject");
			ObjectManagerConfig.OBJECT[] values = Enum.GetValues<ObjectManagerConfig.OBJECT>();
			for (int i = 0; i < values.Length; i++)
			{
				ObjectManagerConfig.OBJECT oBJECT = values[i];
				projectileObject.AddItem(oBJECT.ToString(), (int)oBJECT);
				if (oBJECT == (ObjectManagerConfig.OBJECT)num)
				{
					projectileObject.Select(projectileObject.ItemCount - 1);
				}
			}
			projectileObject.ItemSelected += (long index) =>
			{
				if (!_updatingControls)
				{
					CommitProjectileProperty("projectileObject", Variant.From<int>(projectileObject.GetItemId((int)index)), refreshPropertyList: true);
				}
			};
			Button objectCatalogButton = root.GetNode<Button>("%ProjectileObjectCatalogButton");
			objectCatalogButton.Text = $"对象池图鉴 · {(ObjectManagerConfig.OBJECT)num}";
			objectCatalogButton.Pressed += () =>
			{
				OpenProjectileObjectCatalog(projectileObject, objectCatalogButton);
			};
		}
		BindResourcePickerIfPresent(root, "%ProjectileSceneRow", "%ProjectileScenePicker", projectile, "projectileScene", RebuildProjectileScenePreview);
		BindStringOptionIfPresent(root, "%SplatTypeRow", "%SplatSceneTypeOption", projectile, "splatSceneType", new string[2] { "Particles", "Sprite" }, UpdateProjectileSummary);
		MountProjectileSegmentedOption(root.GetNode<OptionButton>("%SplatSceneTypeOption"), root.GetNode<HFlowContainer>("%SplatSceneTypeSegments"));
		BindTextIfPresent(root, "%SplatAudioRow", root.GetNode<LineEdit>("%SplatAudioLineEdit"), projectile, "splatAudio", UpdateProjectileSummary);
		BindResourcePickerIfPresent(root, "%SplatSceneRow", "%SplatScenePicker", projectile, "splatScene", UpdateProjectilePlaybackVisual);
		BindResourcePickerIfPresent(root, "%HitEffectRow", "%HitEffectPicker", projectile, "hitEffect", UpdateProjectilePlaybackVisual);
		BindProjectileResourceCatalogButton(root, "%ProjectileSceneCatalogButton", "%ProjectileScenePicker", projectile, "projectileScene", RebuildProjectileScenePreview);
		BindProjectileResourceCatalogButton(root, "%SplatSceneCatalogButton", "%SplatScenePicker", projectile, "splatScene", UpdateProjectilePlaybackVisual);
		BindProjectileResourceCatalogButton(root, "%HitEffectCatalogButton", "%HitEffectPicker", projectile, "hitEffect", UpdateProjectilePlaybackVisual);
		_damageLabel = root.GetNode<Label>("%ProjectileDamageSummary");
		UpdateProjectileSummary();
	}

	private void BindProjectileHitFields(Control root, Resource projectile)
	{
		EnsureProjectilePropertyBinding();
		bool flag = BuildProjectileFlagGrid<TowerDefenseEnum.PROJECTILE_DAMAGE_FLAG>(root.GetNode<GridContainer>("%DamageFlagsGrid"), projectile, "damageFlags");
		bool flag2 = BuildProjectileFlagGrid<TowerDefenseEnum.PROJECTILE_FIRE_METHOD_FLAG>(root.GetNode<GridContainer>("%FireMethodFlagsGrid"), projectile, "fireMethodFlags");
		bool flag3 = BuildProjectileFlagGrid<TowerDefenseEnum.CHARACTER_COLLISION_FLAGS>(root.GetNode<GridContainer>("%CollisionFlagsGrid"), projectile, "collisionFlags");
		root.GetNode<Control>("%FlagsSection").Visible = flag | flag2 | flag3;
		BindNumberIfPresent(root, "%CatapultHeightRow", root.GetNode<SpinBox>("%CatapultHeightSpinBox"), projectile, "catapultHeight", UpdateProjectilePlaybackVisual);
		BindNumberIfPresent(root, "%TrackIntervalRow", root.GetNode<SpinBox>("%TrackIntervalSpinBox"), projectile, "trackSearchInterval", UpdateProjectileSummary);
		BindToggleIfPresent(root.GetNode<BaseButton>("%RotateFollowVelocityCheck"), projectile, "rotateFollowVelocity", UpdateProjectilePlaybackVisual);
		BindToggleIfPresent(root.GetNode<BaseButton>("%HitBodyCheck"), projectile, "hitBody", UpdateProjectileSummary);
		BindToggleIfPresent(root.GetNode<BaseButton>("%IsFireCheck"), projectile, "isFire", UpdateProjectileSummary);
		BindStringOptionIfPresent(root, "%RangeTypeRow", "%RangeTypeOption", projectile, "rangeType", new string[2] { "Default", "Bomb" }, UpdateProjectileSummary);
		MountProjectileSegmentedOption(root.GetNode<OptionButton>("%RangeTypeOption"), root.GetNode<HFlowContainer>("%RangeTypeSegments"));
		BindToggleIfPresent(root.GetNode<BaseButton>("%UseRangeCheck"), projectile, "useRange", UpdateProjectileSummary);
		BindVector2IfPresent(root, "%RangeSizeRow", root.GetNode<SpinBox>("%RangeSizeXSpinBox"), root.GetNode<SpinBox>("%RangeSizeYSpinBox"), projectile, "rangeSize", UpdateProjectileSummary);
		BindNumberIfPresent(root, "%HitPercentageRow", root.GetNode<SpinBox>("%HitPercentageSpinBox"), projectile, "hitPesontage", UpdateProjectileSummary);
		bool visible = HasProjectileProperty(projectile, "penetrateNum") || HasProjectileProperty(projectile, "penetrateOverBack") || HasProjectileProperty(projectile, "backOutGround") || HasProjectileProperty(projectile, "backDuration");
		root.GetNode<Control>("%PenetrateSection").Visible = visible;
		BindNumberIfPresent(root, "%PenetrateNumRow", root.GetNode<SpinBox>("%PenetrateNumSpinBox"), projectile, "penetrateNum", UpdateProjectileSummary);
		BindToggleIfPresent(root.GetNode<BaseButton>("%PenetrateOverBackCheck"), projectile, "penetrateOverBack", UpdateProjectileSummary);
		BindToggleIfPresent(root.GetNode<BaseButton>("%BackOutGroundCheck"), projectile, "backOutGround", UpdateProjectileSummary);
		BindNumberIfPresent(root, "%BackDurationRow", root.GetNode<SpinBox>("%BackDurationSpinBox"), projectile, "backDuration", UpdateProjectilePlaybackVisual);
		_flagLabel = root.GetNode<Label>("%ProjectileFlagSummary");
		_rangeLabel = root.GetNode<Label>("%ProjectileRangeSummary");
		_hitTargetEventList = root.GetNode<ItemList>("%HitTargetEventList");
		_hitCharacterEventList = root.GetNode<ItemList>("%HitCharacterEventList");
		_hitGroundEventList = root.GetNode<ItemList>("%HitGroundEventList");
		_behaviorsList = root.GetNode<ItemList>("%BehaviorsList");
		root.GetNode<Control>("%BehaviorsBox").Visible = HasProjectileProperty(projectile, "behaviors");
		_hitTargetEventList.ItemActivated += (long index) =>
		{
			OpenProjectileNestedResource(_hitTargetEventList, "hitTargetEventList", (int)index);
		};
		_hitCharacterEventList.ItemActivated += (long index) =>
		{
			OpenProjectileNestedResource(_hitCharacterEventList, "hitCharacterEventList", (int)index);
		};
		_hitGroundEventList.ItemActivated += (long index) =>
		{
			OpenProjectileNestedResource(_hitGroundEventList, "hitGroundEventList", (int)index);
		};
		_behaviorsList.ItemActivated += (long index) =>
		{
			OpenProjectileNestedResource(_behaviorsList, "behaviors", (int)index);
		};
		RefreshProjectileLists(projectile);
		UpdateProjectileSummary();
	}

	private void EnsureProjectilePropertyBinding()
	{
		if (_projectilePropertyBinding == null)
		{
			_projectilePropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), (bool _) =>
			{
				NotifyCurrentResourceEdited();
			});
		}
	}

	private void BindTextIfPresent(Control root, NodePath rowPath, LineEdit control, Resource projectile, StringName property, Action refresh)
	{
		bool flag = HasProjectileProperty(projectile, property.ToString());
		root.GetNode<Control>(rowPath).Visible = flag;
		if (flag)
		{
			_projectilePropertyBinding.BindVariantText(control, null, projectile, property, refresh);
		}
	}

	private void BindNumberIfPresent(Control root, NodePath rowPath, Godot.Range control, Resource projectile, StringName property, Action refresh)
	{
		bool flag = HasProjectileProperty(projectile, property.ToString());
		root.GetNode<Control>(rowPath).Visible = flag;
		if (flag)
		{
			_projectilePropertyBinding.BindNumber(control, projectile, property, refresh);
		}
	}

	private void BindVector2IfPresent(Control root, NodePath rowPath, SpinBox xControl, SpinBox yControl, Resource projectile, StringName property, Action refresh)
	{
		bool flag = HasProjectileProperty(projectile, property.ToString());
		root.GetNode<Control>(rowPath).Visible = flag;
		if (flag)
		{
			_projectilePropertyBinding.BindVector2Range(xControl, yControl, projectile, property, refresh);
		}
	}

	private void BindToggleIfPresent(BaseButton control, Resource projectile, StringName property, Action refresh)
	{
		if (control.Visible = HasProjectileProperty(projectile, property.ToString()))
		{
			_projectilePropertyBinding.BindToggle(control, projectile, property, refresh);
		}
	}

	private void BindResourcePickerIfPresent(Control root, NodePath rowPath, NodePath pickerPath, Resource projectile, StringName property, Action refresh)
	{
		bool flag = HasProjectileProperty(projectile, property.ToString());
		root.GetNode<Control>(rowPath).Visible = flag;
		if (flag)
		{
			XWResourcePicker node = root.GetNode<XWResourcePicker>(pickerPath);
			node.Setup("PackedScene");
			node.SetEditedResource(ReadProjectilePackedScene(projectile, property.ToString()));
			node.ResourceChanged += (Resource resource) =>
			{
				CommitProjectileProperty(property, Variant.From<PackedScene>(resource as PackedScene), refreshPropertyList: false);
				refresh?.Invoke();
			};
		}
	}

	private void BindProjectileResourceCatalogButton(Control root, NodePath buttonPath, NodePath pickerPath, Resource projectile, StringName property, Action refresh)
	{
		Button node = root.GetNode<Button>(buttonPath);
		XWResourcePicker surfacePicker = root.GetNode<XWResourcePicker>(pickerPath);
		if (!(node.Visible = HasProjectileProperty(projectile, property.ToString())))
		{
			return;
		}
		node.Pressed += () =>
		{
			EnsureProjectilePicker();
			PackedScene packedScene = ReadProjectilePackedScene(projectile, property.ToString());
			_projectilePicker?.OpenResourceLibrary("PackedScene", property.ToString(), packedScene?.ResourcePath ?? string.Empty, XWFileSystem.GetSingleton()?.ProjectFolderPath ?? string.Empty, new string[1] { "PackedScene" }, new string[3] { "Scenes", "Resources", "Asset" }, "res://addons/ModEditor/Icons/PackedScene.svg", (XWGameplayResourceChoice choice) =>
			{
				PackedScene from = ResourceLoader.Load<PackedScene>(choice?.ResourcePath ?? string.Empty, null, ResourceLoader.CacheMode.Reuse);
				surfacePicker.SetEditedResource(from);
				CommitProjectileProperty(property, Variant.From(in from), refreshPropertyList: false);
				refresh?.Invoke();
			});
		};
	}

	private void BindStringOptionIfPresent(Control root, NodePath rowPath, NodePath optionPath, Resource projectile, StringName property, IReadOnlyList<string> options, Action refresh)
	{
		bool flag = HasProjectileProperty(projectile, property.ToString());
		root.GetNode<Control>(rowPath).Visible = flag;
		if (!flag)
		{
			return;
		}
		OptionButton option = root.GetNode<OptionButton>(optionPath);
		string b = ReadProjectileString(projectile, property.ToString());
		option.Clear();
		for (int i = 0; i < options.Count; i++)
		{
			option.AddItem(options[i], i);
			if (string.Equals(options[i], b, StringComparison.OrdinalIgnoreCase))
			{
				option.Select(i);
			}
		}
		option.ItemSelected += (long index) =>
		{
			if (!_updatingControls)
			{
				CommitProjectileProperty(property, Variant.From<string>(option.GetItemText((int)index)), refreshPropertyList: false);
				refresh?.Invoke();
			}
		};
	}

	private bool BuildProjectileFlagGrid<TEnum>(GridContainer grid, Resource projectile, StringName property) where TEnum : struct, Enum
	{
		if (!HasProjectileProperty(projectile, property.ToString()))
		{
			grid.Visible = false;
			return false;
		}
		grid.Visible = true;
		int num = ReadProjectileInt(projectile, property.ToString());
		TEnum[] values = Enum.GetValues<TEnum>();
		for (int i = 0; i < values.Length; i++)
		{
			TEnum val = values[i];
			int flag = Convert.ToInt32(val);
			if (flag != 0)
			{
				CheckButton checkButton = new CheckButton
				{
					Text = val.ToString(),
					ButtonPressed = ((num & flag) == flag),
					TooltipText = $"{property}: {flag}",
					SizeFlagsHorizontal = SizeFlags.ExpandFill
				};
				checkButton.Toggled += (bool pressed) =>
				{
					int num2 = ReadProjectileInt(projectile, property.ToString());
					num2 = (pressed ? (num2 | flag) : (num2 & ~flag));
					CommitProjectileProperty(property, Variant.From(in num2), refreshPropertyList: true);
					UpdateProjectileSummary();
					UpdateProjectilePlaybackVisual();
				};
				grid.AddChild(checkButton, forceReadableName: false, InternalMode.Disabled);
			}
		}
		return true;
	}

	private void CommitProjectileProperty(StringName property, Variant value, bool refreshPropertyList)
	{
		if (GodotObject.IsInstanceValid(_editingProjectile) && HasProjectileProperty(_editingProjectile, property.ToString()))
		{
			EnsureProjectilePropertyBinding();
			_projectilePropertyBinding.SetValue(_editingProjectile, property, value, $"修改子弹 {property}");
			if (refreshPropertyList)
			{
				_editingProjectile.NotifyPropertyListChanged();
			}
		}
	}

	private void OpenProjectileNestedResource(ItemList list, string propertyName, int index)
	{
		if (GodotObject.IsInstanceValid(list) && index >= 0 && index < list.ItemCount)
		{
			Variant itemMetadata = list.GetItemMetadata(index);
			if (itemMetadata.VariantType == Variant.Type.Object && itemMetadata.AsGodotObject() is Resource resource && GodotObject.IsInstanceValid(resource))
			{
				XWResourceEditContext context = XWResourceEditContext.ForProperty(resource, _editingProjectile, resource.ResourcePath, CurrentResourcePath, propertyName, index, "projectile_editor", CurrentEditContext?.IsBuiltInSource ?? false, GetProjectilePreviewKey(_editingProjectile));
				XWEditorInterface.Instance?.EditResource(resource, context);
			}
		}
	}

	private void RebuildProjectileScenePreview()
	{
		if (!GodotObject.IsInstanceValid(_projectilePreviewRoot) || _editingProjectile == null)
		{
			return;
		}
		foreach (Node child in _projectilePreviewRoot.GetChildren())
		{
			child.QueueFree();
		}
		_projectileSceneInstance = null;
		Node node = null;
		PackedScene packedScene = ReadProjectilePackedScene(_editingProjectile, "projectileScene");
		if (GodotObject.IsInstanceValid(packedScene))
		{
			try
			{
				node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			}
			catch (Exception ex)
			{
				GD.PushWarning("Projectile editor scene preview failed: " + ex.Message);
			}
		}
		if (node != null)
		{
			node.Name = "ConfiguredProjectileScenePreview";
			TowerDefenseProjectile towerDefenseProjectile = FindRuntimeProjectile(node);
			if (towerDefenseProjectile != null)
			{
				towerDefenseProjectile.suppressGameplay = true;
			}
			Vector2 scale = ReadProjectileVector2(_editingProjectile, "scale", Vector2.One);
			if (node is Node2D node2D)
			{
				node2D.Scale = scale;
			}
			else if (node is Control control)
			{
				control.Scale = scale;
			}
			_projectilePreviewRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			_projectileSceneInstance = node;
			UpdateProjectilePlaybackVisual();
		}
		else
		{
			Vector2 vector = ReadProjectileVector2(_editingProjectile, "size", new Vector2(28f, 28f));
			ColorRect colorRect = new ColorRect
			{
				Name = "ProjectileFallbackShape",
				Color = new Color(0.45f, 0.7f, 1f, 0.85f),
				CustomMinimumSize = vector
			};
			colorRect.Position = -vector * 0.5f;
			_projectilePreviewRoot.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
			_projectileSceneInstance = colorRect;
			UpdateProjectilePlaybackVisual();
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (_projectilePreviewPlaying && !GodotObject.IsInstanceValid(_projectileSceneInstance))
		{
			_projectilePreviewPlaying = false;
		}
		if (_projectilePreviewPlaying)
		{
			_projectilePreviewProgress = Mathf.Clamp(_projectilePreviewProgress + delta / 1.35, 0.0, 1.0);
			if (GodotObject.IsInstanceValid(_projectileTimelineSlider))
			{
				_projectileTimelineSlider.SetValueNoSignal(_projectilePreviewProgress);
			}
			UpdateProjectilePlaybackVisual();
			if (_projectilePreviewProgress >= 1.0)
			{
				_projectilePreviewPlaying = false;
			}
		}
		if (_pipelinePreviewPlaying && !GodotObject.IsInstanceValid(_pipelineProjectileInstance))
		{
			_pipelinePreviewPlaying = false;
		}
		if (_pipelinePreviewPlaying)
		{
			_pipelinePreviewProgress = Mathf.Clamp(_pipelinePreviewProgress + delta / GetPipelinePreviewDuration(), 0.0, 1.0);
			if (GodotObject.IsInstanceValid(_pipelineTimelineSlider))
			{
				_pipelineTimelineSlider.SetValueNoSignal(_pipelinePreviewProgress);
			}
			UpdatePipelinePlaybackVisual();
			if (_pipelinePreviewProgress >= 1.0)
			{
				_pipelinePreviewPlaying = false;
			}
		}
		if (!_projectilePreviewPlaying && !_pipelinePreviewPlaying)
		{
			RequestVisibilityGatedProcessing(requested: false);
		}
	}

	private double GetPipelinePreviewDuration()
	{
		if (!(CurrentResource is FireComponentFireProjectileConfig fireComponentFireProjectileConfig))
		{
			return 1.35;
		}
		double num = Math.Max(1.0, Math.Abs(fireComponentFireProjectileConfig.speed));
		return Math.Clamp(494.0 / num, 0.25, 4.0);
	}

	private void PlayProjectilePreview()
	{
		ClearProjectileImpactPreview();
		_projectilePreviewProgress = 0.0;
		_projectilePreviewPlaying = GodotObject.IsInstanceValid(_projectileSceneInstance);
		if (GodotObject.IsInstanceValid(_projectileTimelineSlider))
		{
			_projectileTimelineSlider.SetValueNoSignal(0.0);
		}
		UpdateProjectilePlaybackVisual();
		if (_projectilePreviewPlaying)
		{
			RequestVisibilityGatedProcessing(requested: true);
		}
		else
		{
			RequestVisibilityGatedProcessing(requested: false);
		}
	}

	private void PlayPipelinePreview()
	{
		_pipelinePreviewProgress = 0.0;
		_pipelinePreviewPlaying = GodotObject.IsInstanceValid(_pipelineProjectileInstance);
		if (GodotObject.IsInstanceValid(_pipelineTimelineSlider))
		{
			_pipelineTimelineSlider.SetValueNoSignal(0.0);
		}
		UpdatePipelinePlaybackVisual();
		RequestVisibilityGatedProcessing(_pipelinePreviewPlaying || _projectilePreviewPlaying);
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		ApplyProjectileRuntimeProcessMode(visible);
	}

	private void ApplyProjectileRuntimeProcessMode(bool visible)
	{
		ProcessModeEnum processMode = (ProcessModeEnum)(visible ? 0 : 4);
		if (GodotObject.IsInstanceValid(_projectilePreviewRoot))
		{
			_projectilePreviewRoot.ProcessMode = processMode;
		}
		if (GodotObject.IsInstanceValid(_projectileTargetRoot))
		{
			_projectileTargetRoot.ProcessMode = processMode;
		}
		if (GodotObject.IsInstanceValid(_projectileImpactRoot))
		{
			_projectileImpactRoot.ProcessMode = processMode;
		}
		if (GodotObject.IsInstanceValid(_pipelineProjectileRoot))
		{
			_pipelineProjectileRoot.ProcessMode = processMode;
		}
		if (GodotObject.IsInstanceValid(_pipelineTargetRoot))
		{
			_pipelineTargetRoot.ProcessMode = processMode;
		}
	}

	private void UpdatePipelinePlaybackVisual()
	{
		if (!GodotObject.IsInstanceValid(_pipelineProjectileRoot))
		{
			return;
		}
		float num = (float)Mathf.Clamp(_pipelinePreviewProgress, 0.0, 1.0);
		Vector2 position = EvaluatePipelinePosition(num);
		_pipelineProjectileRoot.Position = position;
		TowerDefenseProjectileConfig pipelinePreviewConfig = _pipelinePreviewConfig;
		if (pipelinePreviewConfig != null && pipelinePreviewConfig.rotateFollowVelocity && _pipelineProjectileInstance is Node2D node2D)
		{
			Vector2 vector = EvaluatePipelinePosition(Mathf.Min(num + 0.01f, 1f));
			if (!position.IsEqualApprox(vector))
			{
				node2D.Rotation = position.DirectionTo(vector).Angle();
			}
		}
		if (GodotObject.IsInstanceValid(_pipelinePreviewStatus))
		{
			Label pipelinePreviewStatus = _pipelinePreviewStatus;
			string text;
			if (GodotObject.IsInstanceValid(_pipelinePreviewConfig))
			{
				if (num >= 1f)
				{
					text = "轨迹预览完成";
				}
				else
				{
					text = (_pipelinePreviewPlaying ? "安全飞行预览中" : $"时间 {num:P0}");
				}
			}
			else
			{
				text = "缺失子弹配置";
			}
			pipelinePreviewStatus.Text = text;
		}
	}

	private void UpdateProjectilePlaybackVisual()
	{
		if (!GodotObject.IsInstanceValid(_projectilePreviewRoot))
		{
			return;
		}
		float num = (float)Mathf.Clamp(_projectilePreviewProgress, 0.0, 1.0);
		Vector2 vector = new Vector2(120f, 218f);
		Vector2 to = new Vector2(552f, 190f);
		Vector2 position = vector.Lerp(to, num);
		bool flag = (ReadProjectileInt(_editingProjectile, "fireMethodFlags", 1) & 2) != 0;
		if (flag)
		{
			float num2 = Mathf.Clamp((float)ReadProjectileDouble(_editingProjectile, "catapultHeight", 300.0), 30f, 420f);
			position.Y -= Mathf.Sin(num * (float)Math.PI) * num2 * 0.42f;
		}
		_projectilePreviewRoot.Position = position;
		if (ReadProjectileBool(_editingProjectile, "rotateFollowVelocity") && GodotObject.IsInstanceValid(_projectileSceneInstance))
		{
			Vector2 to2 = vector.Lerp(to, Mathf.Min(num + 0.01f, 1f));
			if (flag)
			{
				to2.Y -= Mathf.Sin(Mathf.Min(num + 0.01f, 1f) * (float)Math.PI) * Mathf.Clamp((float)ReadProjectileDouble(_editingProjectile, "catapultHeight", 300.0), 30f, 420f) * 0.42f;
			}
			if (_projectileSceneInstance is Node2D node2D)
			{
				node2D.Rotation = position.DirectionTo(to2).Angle();
			}
		}
		if (GodotObject.IsInstanceValid(_projectilePlaybackStatusLabel))
		{
			Label projectilePlaybackStatusLabel = _projectilePlaybackStatusLabel;
			string text;
			if (num >= 1f)
			{
				text = "已命中目标";
			}
			else
			{
				text = (_projectilePreviewPlaying ? "飞行中" : $"时间 {num:P0}");
			}
			projectilePlaybackStatusLabel.Text = text;
		}
		if (num >= 1f)
		{
			ShowProjectileImpactPreview();
		}
		else
		{
			ClearProjectileImpactPreview();
		}
	}

	private void RebuildProjectileTargetPreview()
	{
		if (!GodotObject.IsInstanceValid(_projectileTargetRoot))
		{
			return;
		}
		foreach (Node child in _projectileTargetRoot.GetChildren())
		{
			child.QueueFree();
		}
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Puzzle/Target/Scene/TowerDefenseZombieTarget.tscn", null, ResourceLoader.CacheMode.Reuse);
		if (GodotObject.IsInstanceValid(packedScene))
		{
			Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			TowerDefenseCharacter towerDefenseCharacter = FindRuntimeCharacter(node);
			if (towerDefenseCharacter != null)
			{
				towerDefenseCharacter.inGame = false;
				towerDefenseCharacter.editorPreviewMode = true;
			}
			_projectileTargetRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void ShowProjectileImpactPreview()
	{
		if (!GodotObject.IsInstanceValid(_projectileImpactRoot) || _projectileImpactRoot.GetChildCount() > 0)
		{
			return;
		}
		PackedScene packedScene = ReadProjectilePackedScene(_editingProjectile, "splatScene") ?? ReadProjectilePackedScene(_editingProjectile, "hitEffect");
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			return;
		}
		try
		{
			_projectileImpactRoot.AddChild(packedScene.Instantiate(PackedScene.GenEditState.Disabled), forceReadableName: false, InternalMode.Disabled);
		}
		catch (Exception ex)
		{
			GD.PushWarning("Projectile impact preview failed: " + ex.Message);
		}
	}

	private void ClearProjectileImpactPreview()
	{
		if (!GodotObject.IsInstanceValid(_projectileImpactRoot))
		{
			return;
		}
		foreach (Node child in _projectileImpactRoot.GetChildren())
		{
			child.QueueFree();
		}
	}

	private static TowerDefenseProjectile FindRuntimeProjectile(Node node)
	{
		if (node is TowerDefenseProjectile result)
		{
			return result;
		}
		if (!GodotObject.IsInstanceValid(node))
		{
			return null;
		}
		foreach (Node child in node.GetChildren())
		{
			TowerDefenseProjectile towerDefenseProjectile = FindRuntimeProjectile(child);
			if (GodotObject.IsInstanceValid(towerDefenseProjectile))
			{
				return towerDefenseProjectile;
			}
		}
		return null;
	}

	private static TowerDefenseCharacter FindRuntimeCharacter(Node node)
	{
		if (node is TowerDefenseCharacter result)
		{
			return result;
		}
		if (!GodotObject.IsInstanceValid(node))
		{
			return null;
		}
		foreach (Node child in node.GetChildren())
		{
			TowerDefenseCharacter towerDefenseCharacter = FindRuntimeCharacter(child);
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				return towerDefenseCharacter;
			}
		}
		return null;
	}

	private void UpdateProjectileSummary()
	{
		if (_editingProjectile == null)
		{
			return;
		}
		_updatingControls = true;
		try
		{
			if (GodotObject.IsInstanceValid(_projectileNameLabel))
			{
				_projectileNameLabel.Text = "子弹: " + EmptyToPlaceholder(ReadProjectileString(_editingProjectile, "name"));
			}
			if (GodotObject.IsInstanceValid(_sceneLabel))
			{
				_sceneLabel.Text = "projectileScene: " + FormatResource(ReadProjectilePackedScene(_editingProjectile, "projectileScene"));
			}
			if (GodotObject.IsInstanceValid(_projectileChangeHintLabel))
			{
				_projectileChangeHintLabel.Text = "当前子弹 key: " + EmptyToPlaceholder(ReadProjectileString(_editingProjectile, "name")) + "。双击下面的 ProjectileChange 资源可打开链路编辑器。";
			}
			if (GodotObject.IsInstanceValid(_damageLabel))
			{
				_damageLabel.Text = $"伤害: {ReadProjectileDouble(_editingProjectile, "baseDamage", 20.0):0.##}, size={FormatVector(ReadProjectileVector2(_editingProjectile, "size", new Vector2(28f, 28f)))}, scale={FormatVector(ReadProjectileVector2(_editingProjectile, "scale", Vector2.One))}";
			}
			if (GodotObject.IsInstanceValid(_flagLabel))
			{
				_flagLabel.Text = (HasProjectileProperty(_editingProjectile, "damageFlags") ? $"damageFlags={ReadProjectileInt(_editingProjectile, "damageFlags")}, fireMethodFlags={ReadProjectileInt(_editingProjectile, "fireMethodFlags")}, collisionFlags={ReadProjectileInt(_editingProjectile, "collisionFlags")}" : ("isFire=" + FormatBool(ReadProjectileBool(_editingProjectile, "isFire"))));
			}
			if (GodotObject.IsInstanceValid(_rangeLabel))
			{
				_rangeLabel.Text = $"rangeType={ReadProjectileString(_editingProjectile, "rangeType", "Default")}, useRange={FormatBool(ReadProjectileBool(_editingProjectile, "useRange"))}, rangeSize={FormatVector(ReadProjectileVector2(_editingProjectile, "rangeSize", new Vector2(0.5f, 0.5f)))}";
			}
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void RefreshProjectileLists(Resource projectile)
	{
		PopulateList(_hitTargetEventList, ReadProjectileEnumerable(projectile, "hitTargetEventList"));
		PopulateList(_hitCharacterEventList, ReadProjectileEnumerable(projectile, "hitCharacterEventList"));
		PopulateList(_hitGroundEventList, ReadProjectileEnumerable(projectile, "hitGroundEventList"));
		PopulateList(_behaviorsList, ReadProjectileEnumerable(projectile, "behaviors"));
	}

	private void BindProjectileChangePanel(VBoxContainer root, Resource projectile)
	{
		root.GetNode<Button>("%RefreshButton").Pressed += RefreshProjectileChangeList;
		_projectileChangeHintLabel = root.GetNode<Label>("%ChangeHint");
		_projectileChangeHintLabel.Text = "当前子弹 key: " + EmptyToPlaceholder(ReadProjectileString(projectile, "name")) + "。双击下面的 ProjectileChange 资源可打开链路编辑器。";
		_projectileChangeList = root.GetNode<ItemList>("%ProjectileChangeList");
		_projectileChangeList.ItemActivated += OpenProjectileChangeItem;
		RefreshProjectileChangeList();
	}

	private void RefreshProjectileChangeList()
	{
		if (!GodotObject.IsInstanceValid(_projectileChangeList))
		{
			return;
		}
		_projectileChangeList.Clear();
		foreach (string item in CollectProjectileChangeResources())
		{
			string from = item;
			int idx = _projectileChangeList.AddItem(from.GetFile().GetBaseName());
			_projectileChangeList.SetItemTooltip(idx, from);
			_projectileChangeList.SetItemMetadata(idx, Variant.From(in from));
		}
		if (_projectileChangeList.ItemCount == 0)
		{
			_projectileChangeList.AddItem("未找到 Resources/ProjectileChanges 中的子弹变化资源");
		}
	}

	private void OpenProjectileChangeItem(long index)
	{
		if (!GodotObject.IsInstanceValid(_projectileChangeList) || index < 0 || index >= _projectileChangeList.ItemCount)
		{
			return;
		}
		Variant itemMetadata = _projectileChangeList.GetItemMetadata((int)index);
		if (itemMetadata.VariantType == Variant.Type.String)
		{
			string text = itemMetadata.AsString();
			if (!string.IsNullOrWhiteSpace(text))
			{
				XWResourceEditorRegistry.TryOpenPath(text);
			}
		}
	}

	private static List<string> CollectProjectileChangeResources()
	{
		List<string> list = new List<string>();
		string text = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
		if (string.IsNullOrWhiteSpace(text))
		{
			return list;
		}
		string path = Path.Combine(text, "Resources", "ProjectileChanges");
		if (!Directory.Exists(path))
		{
			return list;
		}
		foreach (string item in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
		{
			string text2 = Path.GetExtension(item).ToLowerInvariant();
			if ((text2 == ".tres" || text2 == ".res") ? true : false)
			{
				list.Add(item.Replace('\\', '/'));
			}
		}
		list.Sort(StringComparer.OrdinalIgnoreCase);
		return list;
	}

	private static void PopulateList(ItemList list, IEnumerable items)
	{
		if (!GodotObject.IsInstanceValid(list))
		{
			return;
		}
		list.Clear();
		if (items == null)
		{
			list.AddItem("未配置");
			return;
		}
		int num = 0;
		foreach (object item in items)
		{
			int idx = list.AddItem($"{num}: {FormatObject(item)}");
			Resource from = item as Resource;
			if (item is Variant variant && variant.VariantType == Variant.Type.Object && variant.AsGodotObject() is Resource resource)
			{
				from = resource;
			}
			if (GodotObject.IsInstanceValid(from))
			{
				list.SetItemMetadata(idx, Variant.From(in from));
			}
			num++;
		}
		if (num == 0)
		{
			list.AddItem("未配置");
		}
	}

	private static bool HasProjectileProperty(Resource projectile, string propertyName)
	{
		if (!GodotObject.IsInstanceValid(projectile) || string.IsNullOrWhiteSpace(propertyName))
		{
			return false;
		}
		foreach (Dictionary property in projectile.GetPropertyList())
		{
			if (property.ContainsKey("name") && property["name"].AsString() == propertyName)
			{
				return true;
			}
		}
		return false;
	}

	private static string ReadProjectileString(Resource projectile, string propertyName, string defaultValue = "")
	{
		Variant variant = ReadProjectileVariant(projectile, propertyName);
		if (variant.VariantType != Variant.Type.Nil)
		{
			return variant.AsString();
		}
		return defaultValue;
	}

	private static double ReadProjectileDouble(Resource projectile, string propertyName, double defaultValue = 0.0)
	{
		Variant variant = ReadProjectileVariant(projectile, propertyName);
		Variant.Type variantType = variant.VariantType;
		if ((ulong)(variantType - 2) > 1uL || 1 == 0)
		{
			return defaultValue;
		}
		return variant.AsDouble();
	}

	private static int ReadProjectileInt(Resource projectile, string propertyName, int defaultValue = 0)
	{
		Variant variant = ReadProjectileVariant(projectile, propertyName);
		Variant.Type variantType = variant.VariantType;
		if ((ulong)(variantType - 2) > 1uL || 1 == 0)
		{
			return defaultValue;
		}
		return variant.AsInt32();
	}

	private static bool ReadProjectileBool(Resource projectile, string propertyName, bool defaultValue = false)
	{
		Variant variant = ReadProjectileVariant(projectile, propertyName);
		if (variant.VariantType != Variant.Type.Bool)
		{
			return defaultValue;
		}
		return variant.AsBool();
	}

	private static Vector2 ReadProjectileVector2(Resource projectile, string propertyName, Vector2 defaultValue)
	{
		Variant variant = ReadProjectileVariant(projectile, propertyName);
		if (variant.VariantType != Variant.Type.Vector2)
		{
			return defaultValue;
		}
		return variant.AsVector2();
	}

	private static PackedScene ReadProjectilePackedScene(Resource projectile, string propertyName)
	{
		Variant variant = ReadProjectileVariant(projectile, propertyName);
		if (variant.VariantType != Variant.Type.Object)
		{
			return null;
		}
		return variant.AsGodotObject() as PackedScene;
	}

	private static IEnumerable ReadProjectileEnumerable(Resource projectile, string propertyName)
	{
		Variant variant = ReadProjectileVariant(projectile, propertyName);
		if (variant.VariantType == Variant.Type.Array)
		{
			return variant.AsGodotArray();
		}
		if (variant.VariantType == Variant.Type.Object)
		{
			return variant.AsGodotObject() as IEnumerable;
		}
		return null;
	}

	private static Variant ReadProjectileVariant(Resource projectile, string propertyName)
	{
		if (!GodotObject.IsInstanceValid(projectile) || string.IsNullOrWhiteSpace(propertyName) || !HasProjectileProperty(projectile, propertyName))
		{
			return default;
		}
		try
		{
			return projectile.Get(propertyName);
		}
		catch
		{
			return default;
		}
	}

	private static string FormatObject(object value)
	{
		if (value is Variant variant)
		{
			if (variant.VariantType != Variant.Type.Object)
			{
				return variant.ToString();
			}
			value = variant.AsGodotObject();
		}
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

	private static string FormatVector(Vector2 value)
	{
		return $"({value.X:0.##}, {value.Y:0.##})";
	}

	private static string FormatBool(bool value)
	{
		if (!value)
		{
			return "关";
		}
		return "开";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(102)
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
			new MethodInfo(MethodName.RenderProjectileEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsProjectilePipelineResource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderProjectileFirePipeline, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindPipelineEditorFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parameterHost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindFireComponentFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindFireComponentCheckFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "check", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindFireProjectileFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "fire", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindFireProjectileSingleFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "single", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindFireProjectileWeightFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "weight", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindFireProjectileWeightItemFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildFireCollisionFlagGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "grid", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GridContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "check", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshFireProjectilePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindCreateDataCoreFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "createData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindProjectileBehaviorFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "behavior", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenCreateDataProjectilePicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "createData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "coreFields", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenBehaviorPreviewProjectilePicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "behaviorFields", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureProjectilePicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenProjectileObjectCatalog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Object, "catalogButton", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveProjectileObjectIcon, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MountProjectileSegmentedOption, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeProjectileVisualChoices, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindPipelineGamePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePipelineCreateData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveWeightPreviewData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "weight", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPipelineCreateDataPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadProjectileRegistryResource, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectileKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPreviewConfigFromData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildPipelineProjectilePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPipelineProjectileScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "scale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPipelineProjectilePreviewScale, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildPipelineTargetPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildPipelineTrajectory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EvaluatePipelinePosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "progress", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindSinBehavior, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "createData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectilePreviewKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigurePipelineNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "detail", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopulatePipelineBranches, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "branchHost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddPipelineBranchCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "branchHost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "probability", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "warning", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateEditablePipelineWeightBranches, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "branchHost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "weight", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigurePipelineWeightBranchCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "card", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "weight", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPipelineWeightProbabilities, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPipelineWeightList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectPipelineWeightBranch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenPipelineWeightItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddPipelineWeightItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DuplicatePipelineWeightItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemovePipelineWeightItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MovePipelineWeightItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplacePipelineWeightItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "weight", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildPipelineWeightSurfaces, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "weight", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPipelineBranchCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenFirePipelineNestedResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPipelineTitle, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetPipelineParameterHint, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetPipelineStatus, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetShooterNodeDetail, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCheckNodeDetail, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetPoolNodeDetail, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCreateNodeDetail, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetBehaviorNodeDetail, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DescribeCreateData, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DescribeProjectileResource, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindProjectileScenePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindProjectileCoreFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindProjectileHitFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureProjectilePropertyBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitProjectileProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Bool, "refreshPropertyList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenProjectileNestedResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildProjectileScenePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPipelinePreviewDuration, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayProjectilePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayPipelinePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyProjectileRuntimeProcessMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePipelinePlaybackVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateProjectilePlaybackVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildProjectileTargetPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowProjectileImpactPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearProjectileImpactPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindRuntimeProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindRuntimeCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateProjectileSummary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshProjectileLists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindProjectileChangePanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshProjectileChangeList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenProjectileChangeItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasProjectileProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadProjectileString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadProjectileDouble, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadProjectileInt, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadProjectileBool, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadProjectileVector2, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadProjectilePackedScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadProjectileVariant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatResource, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EmptyToPlaceholder, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVector, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatBool, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RenderProjectileEditor && args.Count == 1)
		{
			RenderProjectileEditor(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsProjectilePipelineResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProjectilePipelineResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.RenderProjectileFirePipeline && args.Count == 1)
		{
			RenderProjectileFirePipeline(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindPipelineEditorFields && args.Count == 2)
		{
			BindPipelineEditorFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindFireComponentFields && args.Count == 2)
		{
			BindFireComponentFields(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindFireComponentCheckFields && args.Count == 2)
		{
			BindFireComponentCheckFields(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<FireComponentCheckConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindFireProjectileFields && args.Count == 2)
		{
			BindFireProjectileFields(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<FireComponentFireProjectileConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindFireProjectileSingleFields && args.Count == 2)
		{
			BindFireProjectileSingleFields(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<FireComponentProjectileSingle>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindFireProjectileWeightFields && args.Count == 2)
		{
			BindFireProjectileWeightFields(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<FireComponentProjectileWeight>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindFireProjectileWeightItemFields && args.Count == 2)
		{
			BindFireProjectileWeightItemFields(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<FireComponentProjectileWeightItem>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildFireCollisionFlagGrid && args.Count == 2)
		{
			BuildFireCollisionFlagGrid(VariantUtils.ConvertTo<GridContainer>(in args[0]), VariantUtils.ConvertTo<FireComponentCheckConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFireProjectilePreview && args.Count == 0)
		{
			RefreshFireProjectilePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.BindCreateDataCoreFields && args.Count == 2)
		{
			BindCreateDataCoreFields(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindProjectileBehaviorFields && args.Count == 2)
		{
			BindProjectileBehaviorFields(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<ProjectileBehaviorDefinition>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenCreateDataProjectilePicker && args.Count == 2)
		{
			OpenCreateDataProjectilePicker(VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenBehaviorPreviewProjectilePicker && args.Count == 1)
		{
			OpenBehaviorPreviewProjectilePicker(VariantUtils.ConvertTo<PanelContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureProjectilePicker && args.Count == 0)
		{
			EnsureProjectilePicker();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenProjectileObjectCatalog && args.Count == 2)
		{
			OpenProjectileObjectCatalog(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<Button>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveProjectileObjectIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveProjectileObjectIcon(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.MountProjectileSegmentedOption && args.Count == 2)
		{
			MountProjectileSegmentedOption(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<HFlowContainer>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeProjectileVisualChoices && args.Count == 0)
		{
			DisposeProjectileVisualChoices();
			ret = default;
			return true;
		}
		if (method == MethodName.BindPipelineGamePreview && args.Count == 2)
		{
			BindPipelineGamePreview(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePipelineCreateData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileCreateData>(ResolvePipelineCreateData(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveWeightPreviewData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileCreateData>(ResolveWeightPreviewData(VariantUtils.ConvertTo<FireComponentProjectileWeight>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshPipelineCreateDataPreview && args.Count == 0)
		{
			RefreshPipelineCreateDataPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadProjectileRegistryResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(LoadProjectileRegistryResource(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPreviewConfigFromData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileConfig>(BuildPreviewConfigFromData(VariantUtils.ConvertTo<TowerDefenseProjectileData>(in args[0])));
			return true;
		}
		if (method == MethodName.RebuildPipelineProjectilePreview && args.Count == 0)
		{
			RebuildPipelineProjectilePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPipelineProjectileScale && args.Count == 2)
		{
			ApplyPipelineProjectileScale(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPipelineProjectilePreviewScale && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetPipelineProjectilePreviewScale());
			return true;
		}
		if (method == MethodName.RebuildPipelineTargetPreview && args.Count == 0)
		{
			RebuildPipelineTargetPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildPipelineTrajectory && args.Count == 0)
		{
			RebuildPipelineTrajectory();
			ret = default;
			return true;
		}
		if (method == MethodName.EvaluatePipelinePosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(EvaluatePipelinePosition(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.FindSinBehavior && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ProjectileBehaviorDefinition>(FindSinBehavior(VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetProjectilePreviewKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetProjectilePreviewKey(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.ConfigurePipelineNode && args.Count == 3)
		{
			ConfigurePipelineNode(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulatePipelineBranches && args.Count == 2)
		{
			PopulatePipelineBranches(VariantUtils.ConvertTo<HBoxContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddPipelineBranchCard && args.Count == 4)
		{
			AddPipelineBranchCard(VariantUtils.ConvertTo<HBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateEditablePipelineWeightBranches && args.Count == 2)
		{
			PopulateEditablePipelineWeightBranches(VariantUtils.ConvertTo<HBoxContainer>(in args[0]), VariantUtils.ConvertTo<FireComponentProjectileWeight>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigurePipelineWeightBranchCard && args.Count == 4)
		{
			ConfigurePipelineWeightBranchCard(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<FireComponentProjectileWeight>(in args[1]), VariantUtils.ConvertTo<FireComponentProjectileWeightItem>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPipelineWeightProbabilities && args.Count == 0)
		{
			RefreshPipelineWeightProbabilities();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPipelineWeightList && args.Count == 0)
		{
			RefreshPipelineWeightList();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectPipelineWeightBranch && args.Count == 1)
		{
			SelectPipelineWeightBranch(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenPipelineWeightItem && args.Count == 1)
		{
			OpenPipelineWeightItem(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddPipelineWeightItem && args.Count == 0)
		{
			AddPipelineWeightItem();
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicatePipelineWeightItem && args.Count == 0)
		{
			DuplicatePipelineWeightItem();
			ret = default;
			return true;
		}
		if (method == MethodName.RemovePipelineWeightItem && args.Count == 0)
		{
			RemovePipelineWeightItem();
			ret = default;
			return true;
		}
		if (method == MethodName.MovePipelineWeightItem && args.Count == 1)
		{
			MovePipelineWeightItem(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplacePipelineWeightItems && args.Count == 3)
		{
			ReplacePipelineWeightItems(VariantUtils.ConvertTo<FireComponentProjectileWeight>(in args[0]), VariantUtils.ConvertToArray<FireComponentProjectileWeightItem>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildPipelineWeightSurfaces && args.Count == 1)
		{
			RebuildPipelineWeightSurfaces(VariantUtils.ConvertTo<FireComponentProjectileWeight>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPipelineBranchCards && args.Count == 1)
		{
			RefreshPipelineBranchCards(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenFirePipelineNestedResource && args.Count == 4)
		{
			OpenFirePipelineNestedResource(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPipelineTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPipelineTitle(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPipelineParameterHint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPipelineParameterHint(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPipelineStatus && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPipelineStatus(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetShooterNodeDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetShooterNodeDetail(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCheckNodeDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCheckNodeDetail(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPoolNodeDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPoolNodeDetail(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCreateNodeDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCreateNodeDetail(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetBehaviorNodeDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetBehaviorNodeDetail(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.DescribeCreateData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DescribeCreateData(VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in args[0])));
			return true;
		}
		if (method == MethodName.DescribeProjectileResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DescribeProjectileResource(VariantUtils.ConvertTo<FireComponentProjectileResource>(in args[0])));
			return true;
		}
		if (method == MethodName.BindProjectileScenePreview && args.Count == 1)
		{
			BindProjectileScenePreview(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindProjectileCoreFields && args.Count == 2)
		{
			BindProjectileCoreFields(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindProjectileHitFields && args.Count == 2)
		{
			BindProjectileHitFields(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureProjectilePropertyBinding && args.Count == 0)
		{
			EnsureProjectilePropertyBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.CommitProjectileProperty && args.Count == 3)
		{
			CommitProjectileProperty(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenProjectileNestedResource && args.Count == 3)
		{
			OpenProjectileNestedResource(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildProjectileScenePreview && args.Count == 0)
		{
			RebuildProjectileScenePreview();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPipelinePreviewDuration && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetPipelinePreviewDuration());
			return true;
		}
		if (method == MethodName.PlayProjectilePreview && args.Count == 0)
		{
			PlayProjectilePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayPipelinePreview && args.Count == 0)
		{
			PlayPipelinePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged && args.Count == 1)
		{
			OnVisualEditorVisibilityChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyProjectileRuntimeProcessMode && args.Count == 1)
		{
			ApplyProjectileRuntimeProcessMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePipelinePlaybackVisual && args.Count == 0)
		{
			UpdatePipelinePlaybackVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateProjectilePlaybackVisual && args.Count == 0)
		{
			UpdateProjectilePlaybackVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildProjectileTargetPreview && args.Count == 0)
		{
			RebuildProjectileTargetPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowProjectileImpactPreview && args.Count == 0)
		{
			ShowProjectileImpactPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearProjectileImpactPreview && args.Count == 0)
		{
			ClearProjectileImpactPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.FindRuntimeProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectile>(FindRuntimeProjectile(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindRuntimeCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindRuntimeCharacter(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateProjectileSummary && args.Count == 0)
		{
			UpdateProjectileSummary();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshProjectileLists && args.Count == 1)
		{
			RefreshProjectileLists(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindProjectileChangePanel && args.Count == 2)
		{
			BindProjectileChangePanel(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshProjectileChangeList && args.Count == 0)
		{
			RefreshProjectileChangeList();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenProjectileChangeItem && args.Count == 1)
		{
			OpenProjectileChangeItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasProjectileProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProjectileProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadProjectileString && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(ReadProjectileString(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadProjectileDouble && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadProjectileDouble(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadProjectileInt && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<int>(ReadProjectileInt(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadProjectileBool && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadProjectileBool(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadProjectileVector2 && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ReadProjectileVector2(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadProjectilePackedScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(ReadProjectilePackedScene(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadProjectileVariant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadProjectileVariant(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.FormatVector && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVector(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatBool && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatBool(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsProjectilePipelineResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProjectilePipelineResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveProjectileObjectIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveProjectileObjectIcon(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadProjectileRegistryResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(LoadProjectileRegistryResource(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildPreviewConfigFromData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileConfig>(BuildPreviewConfigFromData(VariantUtils.ConvertTo<TowerDefenseProjectileData>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyPipelineProjectileScale && args.Count == 2)
		{
			ApplyPipelineProjectileScale(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindSinBehavior && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ProjectileBehaviorDefinition>(FindSinBehavior(VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetProjectilePreviewKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetProjectilePreviewKey(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.ConfigurePipelineNode && args.Count == 3)
		{
			ConfigurePipelineNode(VariantUtils.ConvertTo<PanelContainer>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPipelineTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPipelineTitle(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPipelineParameterHint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPipelineParameterHint(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPipelineStatus && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPipelineStatus(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetShooterNodeDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetShooterNodeDetail(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCheckNodeDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCheckNodeDetail(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPoolNodeDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPoolNodeDetail(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCreateNodeDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCreateNodeDetail(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetBehaviorNodeDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetBehaviorNodeDetail(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.DescribeCreateData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DescribeCreateData(VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in args[0])));
			return true;
		}
		if (method == MethodName.DescribeProjectileResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DescribeProjectileResource(VariantUtils.ConvertTo<FireComponentProjectileResource>(in args[0])));
			return true;
		}
		if (method == MethodName.FindRuntimeProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectile>(FindRuntimeProjectile(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindRuntimeCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindRuntimeCharacter(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.HasProjectileProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProjectileProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadProjectileString && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(ReadProjectileString(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadProjectileDouble && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadProjectileDouble(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadProjectileInt && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<int>(ReadProjectileInt(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadProjectileBool && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadProjectileBool(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadProjectileVector2 && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ReadProjectileVector2(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadProjectilePackedScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(ReadProjectilePackedScene(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadProjectileVariant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadProjectileVariant(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.FormatVector && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVector(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatBool && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatBool(VariantUtils.ConvertTo<bool>(in args[0])));
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
		if (method == MethodName.RenderProjectileEditor)
		{
			return true;
		}
		if (method == MethodName.IsProjectilePipelineResource)
		{
			return true;
		}
		if (method == MethodName.RenderProjectileFirePipeline)
		{
			return true;
		}
		if (method == MethodName.BindPipelineEditorFields)
		{
			return true;
		}
		if (method == MethodName.BindFireComponentFields)
		{
			return true;
		}
		if (method == MethodName.BindFireComponentCheckFields)
		{
			return true;
		}
		if (method == MethodName.BindFireProjectileFields)
		{
			return true;
		}
		if (method == MethodName.BindFireProjectileSingleFields)
		{
			return true;
		}
		if (method == MethodName.BindFireProjectileWeightFields)
		{
			return true;
		}
		if (method == MethodName.BindFireProjectileWeightItemFields)
		{
			return true;
		}
		if (method == MethodName.BuildFireCollisionFlagGrid)
		{
			return true;
		}
		if (method == MethodName.RefreshFireProjectilePreview)
		{
			return true;
		}
		if (method == MethodName.BindCreateDataCoreFields)
		{
			return true;
		}
		if (method == MethodName.BindProjectileBehaviorFields)
		{
			return true;
		}
		if (method == MethodName.OpenCreateDataProjectilePicker)
		{
			return true;
		}
		if (method == MethodName.OpenBehaviorPreviewProjectilePicker)
		{
			return true;
		}
		if (method == MethodName.EnsureProjectilePicker)
		{
			return true;
		}
		if (method == MethodName.OpenProjectileObjectCatalog)
		{
			return true;
		}
		if (method == MethodName.ResolveProjectileObjectIcon)
		{
			return true;
		}
		if (method == MethodName.MountProjectileSegmentedOption)
		{
			return true;
		}
		if (method == MethodName.DisposeProjectileVisualChoices)
		{
			return true;
		}
		if (method == MethodName.BindPipelineGamePreview)
		{
			return true;
		}
		if (method == MethodName.ResolvePipelineCreateData)
		{
			return true;
		}
		if (method == MethodName.ResolveWeightPreviewData)
		{
			return true;
		}
		if (method == MethodName.RefreshPipelineCreateDataPreview)
		{
			return true;
		}
		if (method == MethodName.LoadProjectileRegistryResource)
		{
			return true;
		}
		if (method == MethodName.BuildPreviewConfigFromData)
		{
			return true;
		}
		if (method == MethodName.RebuildPipelineProjectilePreview)
		{
			return true;
		}
		if (method == MethodName.ApplyPipelineProjectileScale)
		{
			return true;
		}
		if (method == MethodName.GetPipelineProjectilePreviewScale)
		{
			return true;
		}
		if (method == MethodName.RebuildPipelineTargetPreview)
		{
			return true;
		}
		if (method == MethodName.RebuildPipelineTrajectory)
		{
			return true;
		}
		if (method == MethodName.EvaluatePipelinePosition)
		{
			return true;
		}
		if (method == MethodName.FindSinBehavior)
		{
			return true;
		}
		if (method == MethodName.GetProjectilePreviewKey)
		{
			return true;
		}
		if (method == MethodName.ConfigurePipelineNode)
		{
			return true;
		}
		if (method == MethodName.PopulatePipelineBranches)
		{
			return true;
		}
		if (method == MethodName.AddPipelineBranchCard)
		{
			return true;
		}
		if (method == MethodName.PopulateEditablePipelineWeightBranches)
		{
			return true;
		}
		if (method == MethodName.ConfigurePipelineWeightBranchCard)
		{
			return true;
		}
		if (method == MethodName.RefreshPipelineWeightProbabilities)
		{
			return true;
		}
		if (method == MethodName.RefreshPipelineWeightList)
		{
			return true;
		}
		if (method == MethodName.SelectPipelineWeightBranch)
		{
			return true;
		}
		if (method == MethodName.OpenPipelineWeightItem)
		{
			return true;
		}
		if (method == MethodName.AddPipelineWeightItem)
		{
			return true;
		}
		if (method == MethodName.DuplicatePipelineWeightItem)
		{
			return true;
		}
		if (method == MethodName.RemovePipelineWeightItem)
		{
			return true;
		}
		if (method == MethodName.MovePipelineWeightItem)
		{
			return true;
		}
		if (method == MethodName.ReplacePipelineWeightItems)
		{
			return true;
		}
		if (method == MethodName.RebuildPipelineWeightSurfaces)
		{
			return true;
		}
		if (method == MethodName.RefreshPipelineBranchCards)
		{
			return true;
		}
		if (method == MethodName.OpenFirePipelineNestedResource)
		{
			return true;
		}
		if (method == MethodName.GetPipelineTitle)
		{
			return true;
		}
		if (method == MethodName.GetPipelineParameterHint)
		{
			return true;
		}
		if (method == MethodName.GetPipelineStatus)
		{
			return true;
		}
		if (method == MethodName.GetShooterNodeDetail)
		{
			return true;
		}
		if (method == MethodName.GetCheckNodeDetail)
		{
			return true;
		}
		if (method == MethodName.GetPoolNodeDetail)
		{
			return true;
		}
		if (method == MethodName.GetCreateNodeDetail)
		{
			return true;
		}
		if (method == MethodName.GetBehaviorNodeDetail)
		{
			return true;
		}
		if (method == MethodName.DescribeCreateData)
		{
			return true;
		}
		if (method == MethodName.DescribeProjectileResource)
		{
			return true;
		}
		if (method == MethodName.BindProjectileScenePreview)
		{
			return true;
		}
		if (method == MethodName.BindProjectileCoreFields)
		{
			return true;
		}
		if (method == MethodName.BindProjectileHitFields)
		{
			return true;
		}
		if (method == MethodName.EnsureProjectilePropertyBinding)
		{
			return true;
		}
		if (method == MethodName.CommitProjectileProperty)
		{
			return true;
		}
		if (method == MethodName.OpenProjectileNestedResource)
		{
			return true;
		}
		if (method == MethodName.RebuildProjectileScenePreview)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.GetPipelinePreviewDuration)
		{
			return true;
		}
		if (method == MethodName.PlayProjectilePreview)
		{
			return true;
		}
		if (method == MethodName.PlayPipelinePreview)
		{
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.ApplyProjectileRuntimeProcessMode)
		{
			return true;
		}
		if (method == MethodName.UpdatePipelinePlaybackVisual)
		{
			return true;
		}
		if (method == MethodName.UpdateProjectilePlaybackVisual)
		{
			return true;
		}
		if (method == MethodName.RebuildProjectileTargetPreview)
		{
			return true;
		}
		if (method == MethodName.ShowProjectileImpactPreview)
		{
			return true;
		}
		if (method == MethodName.ClearProjectileImpactPreview)
		{
			return true;
		}
		if (method == MethodName.FindRuntimeProjectile)
		{
			return true;
		}
		if (method == MethodName.FindRuntimeCharacter)
		{
			return true;
		}
		if (method == MethodName.UpdateProjectileSummary)
		{
			return true;
		}
		if (method == MethodName.RefreshProjectileLists)
		{
			return true;
		}
		if (method == MethodName.BindProjectileChangePanel)
		{
			return true;
		}
		if (method == MethodName.RefreshProjectileChangeList)
		{
			return true;
		}
		if (method == MethodName.OpenProjectileChangeItem)
		{
			return true;
		}
		if (method == MethodName.HasProjectileProperty)
		{
			return true;
		}
		if (method == MethodName.ReadProjectileString)
		{
			return true;
		}
		if (method == MethodName.ReadProjectileDouble)
		{
			return true;
		}
		if (method == MethodName.ReadProjectileInt)
		{
			return true;
		}
		if (method == MethodName.ReadProjectileBool)
		{
			return true;
		}
		if (method == MethodName.ReadProjectileVector2)
		{
			return true;
		}
		if (method == MethodName.ReadProjectilePackedScene)
		{
			return true;
		}
		if (method == MethodName.ReadProjectileVariant)
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
		if (method == MethodName.FormatVector)
		{
			return true;
		}
		if (method == MethodName.FormatBool)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editingProjectile)
		{
			_editingProjectile = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._projectileViewport)
		{
			_projectileViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._projectilePreviewRoot)
		{
			_projectilePreviewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._projectileTargetRoot)
		{
			_projectileTargetRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._projectileImpactRoot)
		{
			_projectileImpactRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._projectileTimelineSlider)
		{
			_projectileTimelineSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._projectilePlayButton)
		{
			_projectilePlayButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._projectilePlaybackStatusLabel)
		{
			_projectilePlaybackStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._projectilePreviewPlaying)
		{
			_projectilePreviewPlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._projectilePreviewProgress)
		{
			_projectilePreviewProgress = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._projectileSceneInstance)
		{
			_projectileSceneInstance = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._projectileNameLabel)
		{
			_projectileNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._sceneLabel)
		{
			_sceneLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._damageLabel)
		{
			_damageLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._flagLabel)
		{
			_flagLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._rangeLabel)
		{
			_rangeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._hitTargetEventList)
		{
			_hitTargetEventList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._hitCharacterEventList)
		{
			_hitCharacterEventList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._hitGroundEventList)
		{
			_hitGroundEventList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._behaviorsList)
		{
			_behaviorsList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._projectileChangeList)
		{
			_projectileChangeList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._projectileChangeHintLabel)
		{
			_projectileChangeHintLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pipelineProjectileRoot)
		{
			_pipelineProjectileRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._pipelineTargetRoot)
		{
			_pipelineTargetRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._pipelineTrajectoryLine)
		{
			_pipelineTrajectoryLine = VariantUtils.ConvertTo<Line2D>(in value);
			return true;
		}
		if (name == PropertyName._pipelineTimelineSlider)
		{
			_pipelineTimelineSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._pipelinePlayButton)
		{
			_pipelinePlayButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._pipelinePreviewStatus)
		{
			_pipelinePreviewStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pipelineStatusLabel)
		{
			_pipelineStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pipelineProjectileInstance)
		{
			_pipelineProjectileInstance = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._pipelineResolvedScene)
		{
			_pipelineResolvedScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName._pipelinePreviewConfig)
		{
			_pipelinePreviewConfig = VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in value);
			return true;
		}
		if (name == PropertyName._pipelineCreateData)
		{
			_pipelineCreateData = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName._pipelineBehavior)
		{
			_pipelineBehavior = VariantUtils.ConvertTo<ProjectileBehaviorDefinition>(in value);
			return true;
		}
		if (name == PropertyName._pipelinePreviewProgress)
		{
			_pipelinePreviewProgress = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pipelinePreviewPlaying)
		{
			_pipelinePreviewPlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pipelinePreviewProjectileKey)
		{
			_pipelinePreviewProjectileKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._projectilePicker)
		{
			_projectilePicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._pipelineBranchHost)
		{
			_pipelineBranchHost = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._pipelineWeightList)
		{
			_pipelineWeightList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._pipelineWeightResource)
		{
			_pipelineWeightResource = VariantUtils.ConvertTo<FireComponentProjectileWeight>(in value);
			return true;
		}
		if (name == PropertyName._selectedPipelineWeightIndex)
		{
			_selectedPipelineWeightIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editingProjectile)
		{
			value = VariantUtils.CreateFrom(in _editingProjectile);
			return true;
		}
		if (name == PropertyName._projectileViewport)
		{
			value = VariantUtils.CreateFrom(in _projectileViewport);
			return true;
		}
		if (name == PropertyName._projectilePreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _projectilePreviewRoot);
			return true;
		}
		if (name == PropertyName._projectileTargetRoot)
		{
			value = VariantUtils.CreateFrom(in _projectileTargetRoot);
			return true;
		}
		if (name == PropertyName._projectileImpactRoot)
		{
			value = VariantUtils.CreateFrom(in _projectileImpactRoot);
			return true;
		}
		if (name == PropertyName._projectileTimelineSlider)
		{
			value = VariantUtils.CreateFrom(in _projectileTimelineSlider);
			return true;
		}
		if (name == PropertyName._projectilePlayButton)
		{
			value = VariantUtils.CreateFrom(in _projectilePlayButton);
			return true;
		}
		if (name == PropertyName._projectilePlaybackStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _projectilePlaybackStatusLabel);
			return true;
		}
		if (name == PropertyName._projectilePreviewPlaying)
		{
			value = VariantUtils.CreateFrom(in _projectilePreviewPlaying);
			return true;
		}
		if (name == PropertyName._projectilePreviewProgress)
		{
			value = VariantUtils.CreateFrom(in _projectilePreviewProgress);
			return true;
		}
		if (name == PropertyName._projectileSceneInstance)
		{
			value = VariantUtils.CreateFrom(in _projectileSceneInstance);
			return true;
		}
		if (name == PropertyName._projectileNameLabel)
		{
			value = VariantUtils.CreateFrom(in _projectileNameLabel);
			return true;
		}
		if (name == PropertyName._sceneLabel)
		{
			value = VariantUtils.CreateFrom(in _sceneLabel);
			return true;
		}
		if (name == PropertyName._damageLabel)
		{
			value = VariantUtils.CreateFrom(in _damageLabel);
			return true;
		}
		if (name == PropertyName._flagLabel)
		{
			value = VariantUtils.CreateFrom(in _flagLabel);
			return true;
		}
		if (name == PropertyName._rangeLabel)
		{
			value = VariantUtils.CreateFrom(in _rangeLabel);
			return true;
		}
		if (name == PropertyName._hitTargetEventList)
		{
			value = VariantUtils.CreateFrom(in _hitTargetEventList);
			return true;
		}
		if (name == PropertyName._hitCharacterEventList)
		{
			value = VariantUtils.CreateFrom(in _hitCharacterEventList);
			return true;
		}
		if (name == PropertyName._hitGroundEventList)
		{
			value = VariantUtils.CreateFrom(in _hitGroundEventList);
			return true;
		}
		if (name == PropertyName._behaviorsList)
		{
			value = VariantUtils.CreateFrom(in _behaviorsList);
			return true;
		}
		if (name == PropertyName._projectileChangeList)
		{
			value = VariantUtils.CreateFrom(in _projectileChangeList);
			return true;
		}
		if (name == PropertyName._projectileChangeHintLabel)
		{
			value = VariantUtils.CreateFrom(in _projectileChangeHintLabel);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._pipelineProjectileRoot)
		{
			value = VariantUtils.CreateFrom(in _pipelineProjectileRoot);
			return true;
		}
		if (name == PropertyName._pipelineTargetRoot)
		{
			value = VariantUtils.CreateFrom(in _pipelineTargetRoot);
			return true;
		}
		if (name == PropertyName._pipelineTrajectoryLine)
		{
			value = VariantUtils.CreateFrom(in _pipelineTrajectoryLine);
			return true;
		}
		if (name == PropertyName._pipelineTimelineSlider)
		{
			value = VariantUtils.CreateFrom(in _pipelineTimelineSlider);
			return true;
		}
		if (name == PropertyName._pipelinePlayButton)
		{
			value = VariantUtils.CreateFrom(in _pipelinePlayButton);
			return true;
		}
		if (name == PropertyName._pipelinePreviewStatus)
		{
			value = VariantUtils.CreateFrom(in _pipelinePreviewStatus);
			return true;
		}
		if (name == PropertyName._pipelineStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _pipelineStatusLabel);
			return true;
		}
		if (name == PropertyName._pipelineProjectileInstance)
		{
			value = VariantUtils.CreateFrom(in _pipelineProjectileInstance);
			return true;
		}
		if (name == PropertyName._pipelineResolvedScene)
		{
			value = VariantUtils.CreateFrom(in _pipelineResolvedScene);
			return true;
		}
		if (name == PropertyName._pipelinePreviewConfig)
		{
			value = VariantUtils.CreateFrom(in _pipelinePreviewConfig);
			return true;
		}
		if (name == PropertyName._pipelineCreateData)
		{
			value = VariantUtils.CreateFrom(in _pipelineCreateData);
			return true;
		}
		if (name == PropertyName._pipelineBehavior)
		{
			value = VariantUtils.CreateFrom(in _pipelineBehavior);
			return true;
		}
		if (name == PropertyName._pipelinePreviewProgress)
		{
			value = VariantUtils.CreateFrom(in _pipelinePreviewProgress);
			return true;
		}
		if (name == PropertyName._pipelinePreviewPlaying)
		{
			value = VariantUtils.CreateFrom(in _pipelinePreviewPlaying);
			return true;
		}
		if (name == PropertyName._pipelinePreviewProjectileKey)
		{
			value = VariantUtils.CreateFrom(in _pipelinePreviewProjectileKey);
			return true;
		}
		if (name == PropertyName._projectilePicker)
		{
			value = VariantUtils.CreateFrom(in _projectilePicker);
			return true;
		}
		if (name == PropertyName._pipelineBranchHost)
		{
			value = VariantUtils.CreateFrom(in _pipelineBranchHost);
			return true;
		}
		if (name == PropertyName._pipelineWeightList)
		{
			value = VariantUtils.CreateFrom(in _pipelineWeightList);
			return true;
		}
		if (name == PropertyName._pipelineWeightResource)
		{
			value = VariantUtils.CreateFrom(in _pipelineWeightResource);
			return true;
		}
		if (name == PropertyName._selectedPipelineWeightIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedPipelineWeightIndex);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingProjectile, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectilePreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileTargetRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileImpactRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileTimelineSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectilePlayButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectilePlaybackStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._projectilePreviewPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._projectilePreviewProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileSceneInstance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sceneLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._damageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._flagLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rangeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hitTargetEventList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hitCharacterEventList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hitGroundEventList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._behaviorsList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileChangeList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileChangeHintLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineProjectileRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineTargetRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineTrajectoryLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineTimelineSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelinePlayButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelinePreviewStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineProjectileInstance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineResolvedScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelinePreviewConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineCreateData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineBehavior, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pipelinePreviewProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pipelinePreviewPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pipelinePreviewProjectileKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectilePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineBranchHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineWeightList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pipelineWeightResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedPipelineWeightIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingProjectile, Variant.From(in _editingProjectile));
		info.AddProperty(PropertyName._projectileViewport, Variant.From(in _projectileViewport));
		info.AddProperty(PropertyName._projectilePreviewRoot, Variant.From(in _projectilePreviewRoot));
		info.AddProperty(PropertyName._projectileTargetRoot, Variant.From(in _projectileTargetRoot));
		info.AddProperty(PropertyName._projectileImpactRoot, Variant.From(in _projectileImpactRoot));
		info.AddProperty(PropertyName._projectileTimelineSlider, Variant.From(in _projectileTimelineSlider));
		info.AddProperty(PropertyName._projectilePlayButton, Variant.From(in _projectilePlayButton));
		info.AddProperty(PropertyName._projectilePlaybackStatusLabel, Variant.From(in _projectilePlaybackStatusLabel));
		info.AddProperty(PropertyName._projectilePreviewPlaying, Variant.From(in _projectilePreviewPlaying));
		info.AddProperty(PropertyName._projectilePreviewProgress, Variant.From(in _projectilePreviewProgress));
		info.AddProperty(PropertyName._projectileSceneInstance, Variant.From(in _projectileSceneInstance));
		info.AddProperty(PropertyName._projectileNameLabel, Variant.From(in _projectileNameLabel));
		info.AddProperty(PropertyName._sceneLabel, Variant.From(in _sceneLabel));
		info.AddProperty(PropertyName._damageLabel, Variant.From(in _damageLabel));
		info.AddProperty(PropertyName._flagLabel, Variant.From(in _flagLabel));
		info.AddProperty(PropertyName._rangeLabel, Variant.From(in _rangeLabel));
		info.AddProperty(PropertyName._hitTargetEventList, Variant.From(in _hitTargetEventList));
		info.AddProperty(PropertyName._hitCharacterEventList, Variant.From(in _hitCharacterEventList));
		info.AddProperty(PropertyName._hitGroundEventList, Variant.From(in _hitGroundEventList));
		info.AddProperty(PropertyName._behaviorsList, Variant.From(in _behaviorsList));
		info.AddProperty(PropertyName._projectileChangeList, Variant.From(in _projectileChangeList));
		info.AddProperty(PropertyName._projectileChangeHintLabel, Variant.From(in _projectileChangeHintLabel));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._pipelineProjectileRoot, Variant.From(in _pipelineProjectileRoot));
		info.AddProperty(PropertyName._pipelineTargetRoot, Variant.From(in _pipelineTargetRoot));
		info.AddProperty(PropertyName._pipelineTrajectoryLine, Variant.From(in _pipelineTrajectoryLine));
		info.AddProperty(PropertyName._pipelineTimelineSlider, Variant.From(in _pipelineTimelineSlider));
		info.AddProperty(PropertyName._pipelinePlayButton, Variant.From(in _pipelinePlayButton));
		info.AddProperty(PropertyName._pipelinePreviewStatus, Variant.From(in _pipelinePreviewStatus));
		info.AddProperty(PropertyName._pipelineStatusLabel, Variant.From(in _pipelineStatusLabel));
		info.AddProperty(PropertyName._pipelineProjectileInstance, Variant.From(in _pipelineProjectileInstance));
		info.AddProperty(PropertyName._pipelineResolvedScene, Variant.From(in _pipelineResolvedScene));
		info.AddProperty(PropertyName._pipelinePreviewConfig, Variant.From(in _pipelinePreviewConfig));
		info.AddProperty(PropertyName._pipelineCreateData, Variant.From(in _pipelineCreateData));
		info.AddProperty(PropertyName._pipelineBehavior, Variant.From(in _pipelineBehavior));
		info.AddProperty(PropertyName._pipelinePreviewProgress, Variant.From(in _pipelinePreviewProgress));
		info.AddProperty(PropertyName._pipelinePreviewPlaying, Variant.From(in _pipelinePreviewPlaying));
		info.AddProperty(PropertyName._pipelinePreviewProjectileKey, Variant.From(in _pipelinePreviewProjectileKey));
		info.AddProperty(PropertyName._projectilePicker, Variant.From(in _projectilePicker));
		info.AddProperty(PropertyName._pipelineBranchHost, Variant.From(in _pipelineBranchHost));
		info.AddProperty(PropertyName._pipelineWeightList, Variant.From(in _pipelineWeightList));
		info.AddProperty(PropertyName._pipelineWeightResource, Variant.From(in _pipelineWeightResource));
		info.AddProperty(PropertyName._selectedPipelineWeightIndex, Variant.From(in _selectedPipelineWeightIndex));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingProjectile, out var value))
		{
			_editingProjectile = value.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._projectileViewport, out var value2))
		{
			_projectileViewport = value2.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._projectilePreviewRoot, out var value3))
		{
			_projectilePreviewRoot = value3.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._projectileTargetRoot, out var value4))
		{
			_projectileTargetRoot = value4.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._projectileImpactRoot, out var value5))
		{
			_projectileImpactRoot = value5.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._projectileTimelineSlider, out var value6))
		{
			_projectileTimelineSlider = value6.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._projectilePlayButton, out var value7))
		{
			_projectilePlayButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._projectilePlaybackStatusLabel, out var value8))
		{
			_projectilePlaybackStatusLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._projectilePreviewPlaying, out var value9))
		{
			_projectilePreviewPlaying = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._projectilePreviewProgress, out var value10))
		{
			_projectilePreviewProgress = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName._projectileSceneInstance, out var value11))
		{
			_projectileSceneInstance = value11.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._projectileNameLabel, out var value12))
		{
			_projectileNameLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._sceneLabel, out var value13))
		{
			_sceneLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._damageLabel, out var value14))
		{
			_damageLabel = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._flagLabel, out var value15))
		{
			_flagLabel = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._rangeLabel, out var value16))
		{
			_rangeLabel = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._hitTargetEventList, out var value17))
		{
			_hitTargetEventList = value17.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._hitCharacterEventList, out var value18))
		{
			_hitCharacterEventList = value18.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._hitGroundEventList, out var value19))
		{
			_hitGroundEventList = value19.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._behaviorsList, out var value20))
		{
			_behaviorsList = value20.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._projectileChangeList, out var value21))
		{
			_projectileChangeList = value21.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._projectileChangeHintLabel, out var value22))
		{
			_projectileChangeHintLabel = value22.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value23))
		{
			_updatingControls = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pipelineProjectileRoot, out var value24))
		{
			_pipelineProjectileRoot = value24.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._pipelineTargetRoot, out var value25))
		{
			_pipelineTargetRoot = value25.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._pipelineTrajectoryLine, out var value26))
		{
			_pipelineTrajectoryLine = value26.As<Line2D>();
		}
		if (info.TryGetProperty(PropertyName._pipelineTimelineSlider, out var value27))
		{
			_pipelineTimelineSlider = value27.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._pipelinePlayButton, out var value28))
		{
			_pipelinePlayButton = value28.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pipelinePreviewStatus, out var value29))
		{
			_pipelinePreviewStatus = value29.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pipelineStatusLabel, out var value30))
		{
			_pipelineStatusLabel = value30.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pipelineProjectileInstance, out var value31))
		{
			_pipelineProjectileInstance = value31.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._pipelineResolvedScene, out var value32))
		{
			_pipelineResolvedScene = value32.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName._pipelinePreviewConfig, out var value33))
		{
			_pipelinePreviewConfig = value33.As<TowerDefenseProjectileConfig>();
		}
		if (info.TryGetProperty(PropertyName._pipelineCreateData, out var value34))
		{
			_pipelineCreateData = value34.As<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName._pipelineBehavior, out var value35))
		{
			_pipelineBehavior = value35.As<ProjectileBehaviorDefinition>();
		}
		if (info.TryGetProperty(PropertyName._pipelinePreviewProgress, out var value36))
		{
			_pipelinePreviewProgress = value36.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pipelinePreviewPlaying, out var value37))
		{
			_pipelinePreviewPlaying = value37.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pipelinePreviewProjectileKey, out var value38))
		{
			_pipelinePreviewProjectileKey = value38.As<string>();
		}
		if (info.TryGetProperty(PropertyName._projectilePicker, out var value39))
		{
			_projectilePicker = value39.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._pipelineBranchHost, out var value40))
		{
			_pipelineBranchHost = value40.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._pipelineWeightList, out var value41))
		{
			_pipelineWeightList = value41.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._pipelineWeightResource, out var value42))
		{
			_pipelineWeightResource = value42.As<FireComponentProjectileWeight>();
		}
		if (info.TryGetProperty(PropertyName._selectedPipelineWeightIndex, out var value43))
		{
			_selectedPipelineWeightIndex = value43.As<int>();
		}
	}
}
