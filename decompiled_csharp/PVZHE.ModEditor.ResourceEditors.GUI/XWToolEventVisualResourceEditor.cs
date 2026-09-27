using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWToolEventVisualResourceEditor.cs")]
public class XWToolEventVisualResourceEditor : XWGenericVisualResourceEditor
{
	private enum PreviewPhase
	{
		Idle,
		ToolDown,
		Effect,
		Recover
	}

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName IsBuiltInToolEventWithCompleteVisualCoverage = "IsBuiltInToolEventWithCompleteVisualCoverage";

		public static readonly StringName BindWorkbench = "BindWorkbench";

		public static readonly StringName BindDirectEditControls = "BindDirectEditControls";

		public static readonly StringName PopulateEventFields = "PopulateEventFields";

		public static readonly StringName OnToolEventPropertyEdited = "OnToolEventPropertyEdited";

		public static readonly StringName RefreshDirectEditPreview = "RefreshDirectEditPreview";

		public static readonly StringName RefreshToolEventFromHistory = "RefreshToolEventFromHistory";

		public static readonly StringName DisposeToolEventBinding = "DisposeToolEventBinding";

		public static readonly StringName UpdateSimulationPreview = "UpdateSimulationPreview";

		public static readonly StringName BuildOutcome = "BuildOutcome";

		public static readonly StringName BuildCaptainOutcome = "BuildCaptainOutcome";

		public static readonly StringName GetSpawnCount = "GetSpawnCount";

		public static readonly StringName EmptyName = "EmptyName";

		public static readonly StringName StartToolEffectPreview = "StartToolEffectPreview";

		public static readonly StringName UpdateToolDownPhase = "UpdateToolDownPhase";

		public static readonly StringName UpdateEffectPhase = "UpdateEffectPhase";

		public static readonly StringName UpdateRecoverPhase = "UpdateRecoverPhase";

		public static readonly StringName ApplyEffectVisuals = "ApplyEffectVisuals";

		public static readonly StringName ShouldFadeTarget = "ShouldFadeTarget";

		public static readonly StringName ResetEffectVisuals = "ResetEffectVisuals";

		public static readonly StringName UpdatePreviewProcessingVisibility = "UpdatePreviewProcessingVisibility";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingEvent = "_editingEvent";

		public static readonly StringName _eventType = "_eventType";

		public static readonly StringName _outcome = "_outcome";

		public static readonly StringName _formHint = "_formHint";

		public static readonly StringName _baseFields = "_baseFields";

		public static readonly StringName _resourceName = "_resourceName";

		public static readonly StringName _localToScene = "_localToScene";

		public static readonly StringName _createSunFields = "_createSunFields";

		public static readonly StringName _recycleFields = "_recycleFields";

		public static readonly StringName _jalapenoFields = "_jalapenoFields";

		public static readonly StringName _projectileFields = "_projectileFields";

		public static readonly StringName _packetFields = "_packetFields";

		public static readonly StringName _sunNum = "_sunNum";

		public static readonly StringName _recyclePercentage = "_recyclePercentage";

		public static readonly StringName _recycleDestroy = "_recycleDestroy";

		public static readonly StringName _jalapenoDestroy = "_jalapenoDestroy";

		public static readonly StringName _projectileEveryNum = "_projectileEveryNum";

		public static readonly StringName _projectileName = "_projectileName";

		public static readonly StringName _projectileDamage = "_projectileDamage";

		public static readonly StringName _packetName = "_packetName";

		public static readonly StringName _packetEveryNum = "_packetEveryNum";

		public static readonly StringName _targetCost = "_targetCost";

		public static readonly StringName _targetHealth = "_targetHealth";

		public static readonly StringName _healthValue = "_healthValue";

		public static readonly StringName _izmMode = "_izmMode";

		public static readonly StringName _targetRoot = "_targetRoot";

		public static readonly StringName _targetPlant = "_targetPlant";

		public static readonly StringName _targetHealthBar = "_targetHealthBar";

		public static readonly StringName _toolRoot = "_toolRoot";

		public static readonly StringName _shovelTool = "_shovelTool";

		public static readonly StringName _mowerTool = "_mowerTool";

		public static readonly StringName _sunEffect = "_sunEffect";

		public static readonly StringName _projectileEffect = "_projectileEffect";

		public static readonly StringName _packetEffect = "_packetEffect";

		public static readonly StringName _fireEffect = "_fireEffect";

		public static readonly StringName _pumpkinEffect = "_pumpkinEffect";

		public static readonly StringName _effectLabel = "_effectLabel";

		public static readonly StringName _previewPhase = "_previewPhase";

		public static readonly StringName _previewElapsed = "_previewElapsed";

		public static readonly StringName _effectApplied = "_effectApplied";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string VisualEditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWToolEventVisualEditorLayout.tscn";

	private static PackedScene _visualEditorLayoutScene;

	private Resource _editingEvent;

	private Label _eventType;

	private Label _outcome;

	private Label _formHint;

	private Control _baseFields;

	private LineEdit _resourceName;

	private CheckButton _localToScene;

	private Control _createSunFields;

	private Control _recycleFields;

	private Control _jalapenoFields;

	private Control _projectileFields;

	private Control _packetFields;

	private SpinBox _sunNum;

	private SpinBox _recyclePercentage;

	private CheckBox _recycleDestroy;

	private CheckBox _jalapenoDestroy;

	private SpinBox _projectileEveryNum;

	private LineEdit _projectileName;

	private SpinBox _projectileDamage;

	private LineEdit _packetName;

	private SpinBox _packetEveryNum;

	private SpinBox _targetCost;

	private HSlider _targetHealth;

	private Label _healthValue;

	private CheckBox _izmMode;

	private Node2D _targetRoot;

	private Node _targetPlant;

	private ProgressBar _targetHealthBar;

	private Node2D _toolRoot;

	private TextureRect _shovelTool;

	private TextureRect _mowerTool;

	private TextureRect _sunEffect;

	private TextureRect _projectileEffect;

	private TextureRect _packetEffect;

	private ColorRect _fireEffect;

	private PanelContainer _pumpkinEffect;

	private Label _effectLabel;

	private XWVisualPropertyBinding _propertyBinding;

	private PreviewPhase _previewPhase;

	private double _previewElapsed;

	private bool _effectApplied;

	public override void _Ready()
	{
		SetProcess(enable: false);
		VisibilityChanged += UpdatePreviewProcessingVisibility;
		base._Ready();
	}

	public override void _ExitTree()
	{
		DisposeToolEventBinding();
		base._ExitTree();
	}

	public override void _Process(double delta)
	{
		if (!GodotObject.IsInstanceValid(_toolRoot) || !IsVisibleInTree())
		{
			SetProcess(enable: false);
			return;
		}
		_previewElapsed += delta;
		switch (_previewPhase)
		{
		case PreviewPhase.ToolDown:
			UpdateToolDownPhase();
			break;
		case PreviewPhase.Effect:
			UpdateEffectPhase();
			break;
		case PreviewPhase.Recover:
			UpdateRecoverPhase();
			break;
		default:
			SetProcess(enable: false);
			break;
		}
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeToolEventBinding();
		if ((CurrentResource is MowerEventConfig || CurrentResource is ShovelEventConfig) && CanvasGrid != null)
		{
			_editingEvent = CurrentResource;
			CanvasGrid.Columns = 1;
			if (_visualEditorLayoutScene == null)
			{
				_visualEditorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWToolEventVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _visualEditorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindWorkbench(vBoxContainer);
				PopulateEventFields();
				BindDirectEditControls(_editingEvent);
				UpdateSimulationPreview();
				AddSummaryRows();
				UpdatePreviewProcessingVisibility();
			}
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!IsBuiltInToolEventWithCompleteVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool IsBuiltInToolEventWithCompleteVisualCoverage(Resource resource)
	{
		Type type = resource?.GetType();
		if (!(type == typeof(MowerEventConfig)) && !(type == typeof(MowerEventCreateSunConfig)) && !(type == typeof(ShovelEventConfig)) && !(type == typeof(ShovelEventCaptainShovelConfig)) && !(type == typeof(ShovelEventCreateProjectileConfig)) && !(type == typeof(ShovelEventDestroyConfig)) && !(type == typeof(ShovelEventJalapenoConfig)) && !(type == typeof(ShovelEventPacketSpawnConfig)) && !(type == typeof(ShovelEventPumpkinShovelConfig)) && !(type == typeof(ShovelEventRecycleConfig)))
		{
			return type == typeof(ShovelEventSkeletonShovelConfig);
		}
		return true;
	}

	private void BindWorkbench(VBoxContainer root)
	{
		_eventType = root.GetNode<Label>("%EventType");
		_outcome = root.GetNode<Label>("%Outcome");
		_formHint = root.GetNode<Label>("%FormHint");
		_baseFields = root.GetNode<Control>("%BaseFields");
		_resourceName = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToScene = root.GetNode<CheckButton>("%LocalToSceneCheck");
		_createSunFields = root.GetNode<Control>("%CreateSunFields");
		_recycleFields = root.GetNode<Control>("%RecycleFields");
		_jalapenoFields = root.GetNode<Control>("%JalapenoFields");
		_projectileFields = root.GetNode<Control>("%ProjectileFields");
		_packetFields = root.GetNode<Control>("%PacketFields");
		_sunNum = root.GetNode<SpinBox>("%SunNum");
		_recyclePercentage = root.GetNode<SpinBox>("%RecyclePercentage");
		_recycleDestroy = root.GetNode<CheckBox>("%RecycleDestroy");
		_jalapenoDestroy = root.GetNode<CheckBox>("%JalapenoDestroy");
		_projectileEveryNum = root.GetNode<SpinBox>("%ProjectileEveryNum");
		_projectileName = root.GetNode<LineEdit>("%ProjectileName");
		_projectileDamage = root.GetNode<SpinBox>("%ProjectileDamage");
		_packetName = root.GetNode<LineEdit>("%PacketName");
		_packetEveryNum = root.GetNode<SpinBox>("%PacketEveryNum");
		_targetCost = root.GetNode<SpinBox>("%TargetCost");
		_targetHealth = root.GetNode<HSlider>("%TargetHealth");
		_healthValue = root.GetNode<Label>("%HealthValue");
		_izmMode = root.GetNode<CheckBox>("%IzmMode");
		_targetRoot = root.GetNode<Node2D>("%TargetRoot");
		_targetPlant = root.GetNode<Node>("%TargetPlant");
		_targetHealthBar = root.GetNode<ProgressBar>("%TargetHealthBar");
		_toolRoot = root.GetNode<Node2D>("%ToolRoot");
		_shovelTool = root.GetNode<TextureRect>("%ShovelTool");
		_mowerTool = root.GetNode<TextureRect>("%MowerTool");
		_sunEffect = root.GetNode<TextureRect>("%SunEffect");
		_projectileEffect = root.GetNode<TextureRect>("%ProjectileEffect");
		_packetEffect = root.GetNode<TextureRect>("%PacketEffect");
		_fireEffect = root.GetNode<ColorRect>("%FireEffect");
		_pumpkinEffect = root.GetNode<PanelContainer>("%PumpkinEffect");
		_effectLabel = root.GetNode<Label>("%EffectLabel");
		root.GetNode<Button>("%ApplyButton").Pressed += StartToolEffectPreview;
		_targetCost.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview();
		};
		_targetHealth.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview();
		};
		_izmMode.Toggled += (bool _) =>
		{
			UpdateSimulationPreview();
		};
	}

	private void BindDirectEditControls(Resource toolEvent)
	{
		if (GodotObject.IsInstanceValid(toolEvent))
		{
			_propertyBinding?.Dispose();
			_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnToolEventPropertyEdited);
			_propertyBinding.BindText(_resourceName, toolEvent, "resource_name", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
			_propertyBinding.BindToggle(_localToScene, toolEvent, "resource_local_to_scene", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
			if (toolEvent is MowerEventCreateSunConfig resource)
			{
				_propertyBinding.BindNumber(_sunNum, resource, "num", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
			}
			if (toolEvent is ShovelEventRecycleConfig resource2)
			{
				_propertyBinding.BindNumber(_recyclePercentage, resource2, "percentage", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
				_propertyBinding.BindToggle(_recycleDestroy, resource2, "destroy", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
			}
			if (toolEvent is ShovelEventJalapenoConfig resource3)
			{
				_propertyBinding.BindToggle(_jalapenoDestroy, resource3, "destroy", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
			}
			if (toolEvent is ShovelEventCreateProjectileConfig resource4)
			{
				_propertyBinding.BindNumber(_projectileEveryNum, resource4, "everyNum", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
				_propertyBinding.BindText(_projectileName, resource4, "projecileName", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
				_propertyBinding.BindNumber(_projectileDamage, resource4, "projectilebaseDamage", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
			}
			if (toolEvent is ShovelEventPacketSpawnConfig resource5)
			{
				_propertyBinding.BindText(_packetName, resource5, "packetName", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
				_propertyBinding.BindNumber(_packetEveryNum, resource5, "everyNum", RefreshDirectEditPreview, this, "RefreshToolEventFromHistory");
			}
		}
	}

	private void PopulateEventFields()
	{
		if (GodotObject.IsInstanceValid(_editingEvent) && GodotObject.IsInstanceValid(_baseFields))
		{
			_resourceName.Text = _editingEvent.ResourceName ?? "";
			_localToScene.ButtonPressed = _editingEvent.Get("resource_local_to_scene").AsBool();
			Resource editingEvent = _editingEvent;
			bool flag = ((editingEvent is MowerEventCreateSunConfig || editingEvent is ShovelEventRecycleConfig || editingEvent is ShovelEventJalapenoConfig || editingEvent is ShovelEventCreateProjectileConfig || editingEvent is ShovelEventPacketSpawnConfig) ? true : false);
			bool flag2 = flag;
			_baseFields.Visible = !flag2;
			_createSunFields.Visible = _editingEvent is MowerEventCreateSunConfig;
			_recycleFields.Visible = _editingEvent is ShovelEventRecycleConfig;
			_jalapenoFields.Visible = _editingEvent is ShovelEventJalapenoConfig;
			_projectileFields.Visible = _editingEvent is ShovelEventCreateProjectileConfig;
			_packetFields.Visible = _editingEvent is ShovelEventPacketSpawnConfig;
			if (_editingEvent is MowerEventCreateSunConfig mowerEventCreateSunConfig)
			{
				_sunNum.Value = mowerEventCreateSunConfig.num;
			}
			if (_editingEvent is ShovelEventRecycleConfig shovelEventRecycleConfig)
			{
				_recyclePercentage.Value = shovelEventRecycleConfig.percentage;
				_recycleDestroy.ButtonPressed = shovelEventRecycleConfig.destroy;
			}
			if (_editingEvent is ShovelEventJalapenoConfig shovelEventJalapenoConfig)
			{
				_jalapenoDestroy.ButtonPressed = shovelEventJalapenoConfig.destroy;
			}
			if (_editingEvent is ShovelEventCreateProjectileConfig shovelEventCreateProjectileConfig)
			{
				_projectileEveryNum.Value = Mathf.Max(1, shovelEventCreateProjectileConfig.everyNum);
				_projectileName.Text = shovelEventCreateProjectileConfig.projecileName ?? "";
				_projectileDamage.Value = shovelEventCreateProjectileConfig.projectilebaseDamage;
			}
			if (_editingEvent is ShovelEventPacketSpawnConfig shovelEventPacketSpawnConfig)
			{
				_packetName.Text = shovelEventPacketSpawnConfig.packetName ?? "";
				_packetEveryNum.Value = Mathf.Max(1, shovelEventPacketSpawnConfig.everyNum);
			}
		}
	}

	private void OnToolEventPropertyEdited(bool committed)
	{
		if ((CurrentResource is MowerEventConfig || CurrentResource is ShovelEventConfig) && CurrentResource == _editingEvent)
		{
			Resource currentResource = CurrentResource;
			if (committed)
			{
				NotifyCurrentResourceEdited();
				return;
			}
			MarkCurrentResourceDirty();
			currentResource.EmitChanged();
		}
	}

	private void RefreshDirectEditPreview()
	{
		UpdateSimulationPreview();
	}

	public void RefreshToolEventFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()) && (CurrentResource is MowerEventConfig || CurrentResource is ShovelEventConfig) && CurrentResource == _editingEvent)
		{
			_propertyBinding?.Dispose();
			_propertyBinding = null;
			PopulateEventFields();
			BindDirectEditControls(_editingEvent);
			UpdateSimulationPreview();
		}
	}

	private void DisposeToolEventBinding()
	{
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		_editingEvent = null;
	}

	private void UpdateSimulationPreview()
	{
		if (GodotObject.IsInstanceValid(_editingEvent) && GodotObject.IsInstanceValid(_outcome))
		{
			ResetEffectVisuals();
			int cost = Mathf.Max(0, Mathf.RoundToInt(_targetCost.Value));
			int num = Mathf.Clamp(Mathf.RoundToInt(_targetHealth.Value), 0, 100);
			bool buttonPressed = _izmMode.ButtonPressed;
			_targetRoot.Visible = true;
			_targetRoot.Modulate = Colors.White;
			_targetRoot.Scale = Vector2.One;
			_targetHealthBar.Value = num;
			_healthValue.Text = $"{num}%";
			bool flag = _editingEvent.Get("resource_local_to_scene").AsBool();
			string text = (string.IsNullOrWhiteSpace(_editingEvent.ResourceName) ? _editingEvent.GetType().Name : (_editingEvent.ResourceName + " · " + _editingEvent.GetType().Name));
			_eventType.Text = text + " · " + (flag ? "场景本地" : "可复用");
			bool flag2 = _editingEvent is MowerEventConfig;
			_shovelTool.Visible = !flag2;
			_mowerTool.Visible = flag2;
			_formHint.Text = (flag2 ? "在真实草坪与角色旁预演小推车触发结果；模拟不调用 Execute。" : "在真实草坪与角色旁预演铲除结果；目标费用、生命和模式仅用于推演。") + (flag ? " 当前资源仅随所属场景保存。" : " 当前资源可被多个场景复用。");
			_outcome.Text = BuildOutcome(cost, num, buttonPressed);
		}
	}

	private string BuildOutcome(int cost, int health, bool izm)
	{
		Resource editingEvent = _editingEvent;
		if (!(editingEvent is MowerEventCreateSunConfig mowerEventCreateSunConfig))
		{
			if (!(editingEvent is MowerEventConfig))
			{
				if (!(editingEvent is ShovelEventDestroyConfig))
				{
					if (!(editingEvent is ShovelEventRecycleConfig shovelEventRecycleConfig))
					{
						if (!(editingEvent is ShovelEventJalapenoConfig shovelEventJalapenoConfig))
						{
							if (!(editingEvent is ShovelEventCreateProjectileConfig shovelEventCreateProjectileConfig))
							{
								if (!(editingEvent is ShovelEventPacketSpawnConfig shovelEventPacketSpawnConfig))
								{
									if (!(editingEvent is ShovelEventPumpkinShovelConfig))
									{
										if (!(editingEvent is ShovelEventSkeletonShovelConfig))
										{
											if (!(editingEvent is ShovelEventCaptainShovelConfig))
											{
												if (editingEvent is ShovelEventConfig)
												{
													return "该铲子事件没有额外参数；点击演示查看固定工具动作。";
												}
												return "当前工具事件没有可推演的结果。";
											}
											return BuildCaptainOutcome(cost);
										}
										return (cost >= 50) ? "费用达到 50：复活最近死亡的植物并转为魅惑单位。" : "费用不足 50：不会触发复活。";
									}
									return $"把目标当前生命（模拟 {health}%）转移给南瓜保护层；若目标本身是南瓜则按剩余生命回收。";
								}
								if (shovelEventPacketSpawnConfig.packetName == "ItemMagnetWave")
								{
									return (cost >= Math.Max(1, shovelEventPacketSpawnConfig.everyNum)) ? $"生成 1 个 ItemMagnetWave，并将 drawNum 设为 {GetSpawnCount(cost, shovelEventPacketSpawnConfig.everyNum)}。" : $"费用不足 {Math.Max(1, shovelEventPacketSpawnConfig.everyNum)}，不会生成 ItemMagnetWave。";
								}
								ShovelEventPacketSpawnConfig shovelEventPacketSpawnConfig2 = shovelEventPacketSpawnConfig;
								return $"在当前格生成 {GetSpawnCount(cost, shovelEventPacketSpawnConfig2.everyNum)} 张 {EmptyName(shovelEventPacketSpawnConfig2.packetName, "未命名卡牌")}。";
							}
							return $"生成 {GetSpawnCount(cost, shovelEventCreateProjectileConfig.everyNum)} 枚 {EmptyName(shovelEventCreateProjectileConfig.projecileName, "未命名子弹")}，每枚基础伤害 {shovelEventCreateProjectileConfig.projectilebaseDamage:0.##}。";
						}
						return $"以目标费用 {cost} 作为伤害生成整行火焰{(shovelEventJalapenoConfig.destroy ? "，随后移除目标。" : "，目标保留。")}";
					}
					return $"返还约 {Mathf.RoundToInt((double)cost * shovelEventRecycleConfig.percentage)} 点费用（{cost} × {shovelEventRecycleConfig.percentage:0.##}）{(shovelEventRecycleConfig.destroy ? "，随后移除目标。" : "，目标继续留在场上。")}";
				}
				return "铲下后立即移除目标，不返还费用。";
			}
			return "该小推车事件没有额外参数；触发时保持基础事件行为。";
		}
		return $"小推车触发后，在目标位置生成 {mowerEventCreateSunConfig.num} 点{(izm ? "脑子" : "普通")}阳光。";
	}

	private static string BuildCaptainOutcome(int cost)
	{
		if (cost < 500)
		{
			int value = Mathf.FloorToInt((double)cost / 100.0);
			return $"生成 {value} 枚银币子弹，并在相邻行空降 {value} 名船员僵尸。";
		}
		return $"生成 {Mathf.FloorToInt((double)cost / 300.0)} 枚金币子弹，并在相邻行空降 {Mathf.FloorToInt((double)cost / 500.0)} 名船长僵尸。";
	}

	private static int GetSpawnCount(int cost, int everyNum)
	{
		return Mathf.FloorToInt((double)cost / (double)Math.Max(1, everyNum));
	}

	private static string EmptyName(string value, string fallback)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return fallback;
	}

	private void StartToolEffectPreview()
	{
		if (GodotObject.IsInstanceValid(_toolRoot))
		{
			UpdateSimulationPreview();
			_previewPhase = PreviewPhase.ToolDown;
			_previewElapsed = 0.0;
			_effectApplied = false;
			_toolRoot.Position = new Vector2(350f, 90f);
			_toolRoot.Rotation = 0f;
			_toolRoot.Scale = Vector2.One;
			_toolRoot.Modulate = Colors.White;
			SetProcess(IsVisibleInTree());
		}
	}

	private void UpdateToolDownPhase()
	{
		float num = Mathf.Clamp((float)(_previewElapsed / 0.36), 0f, 1f);
		float weight = num * num * (3f - 2f * num);
		_toolRoot.Position = new Vector2(350f, 90f).Lerp(new Vector2(480f, 275f), weight);
		_toolRoot.Rotation = ((_editingEvent is ShovelEventConfig) ? Mathf.Lerp(0f, -0.48f, weight) : 0f);
		_toolRoot.Scale = Vector2.One * Mathf.Lerp(1f, 1.12f, Mathf.Sin(num * (float)Math.PI));
		if (!(num < 1f))
		{
			_previewPhase = PreviewPhase.Effect;
			_previewElapsed = 0.0;
		}
	}

	private void UpdateEffectPhase()
	{
		if (!_effectApplied)
		{
			_effectApplied = true;
			ApplyEffectVisuals();
		}
		float num = Mathf.Clamp((float)(_previewElapsed / 0.78), 0f, 1f);
		float num2 = 0.88f + Mathf.Sin(num * (float)Math.PI) * 0.28f;
		if (_sunEffect.Visible)
		{
			_sunEffect.Position = new Vector2(588f, 188f).Lerp(new Vector2(630f, 95f), num);
			_sunEffect.Scale = Vector2.One * num2;
		}
		if (_projectileEffect.Visible)
		{
			_projectileEffect.Position = new Vector2(560f, 292f).Lerp(new Vector2(835f, 292f), num);
		}
		if (_packetEffect.Visible)
		{
			_packetEffect.Scale = Vector2.One * num2;
		}
		if (_fireEffect.Visible)
		{
			_fireEffect.Modulate = new Color(1f, 1f, 1f, 0.45f + Mathf.Sin(num * (float)Math.PI * 6f) * 0.35f);
		}
		if (_pumpkinEffect.Visible)
		{
			_pumpkinEffect.Scale = Vector2.One * num2;
		}
		if (ShouldFadeTarget())
		{
			_targetRoot.Modulate = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0.22f, num));
		}
		if (!(num < 1f))
		{
			_previewPhase = PreviewPhase.Recover;
			_previewElapsed = 0.0;
		}
	}

	private void UpdateRecoverPhase()
	{
		float num = Mathf.Clamp((float)(_previewElapsed / 0.36), 0f, 1f);
		float weight = num * num * (3f - 2f * num);
		_toolRoot.Position = new Vector2(480f, 275f).Lerp(new Vector2(350f, 90f), weight);
		_toolRoot.Rotation = Mathf.Lerp((_editingEvent is ShovelEventConfig) ? (-0.48f) : 0f, 0f, weight);
		_toolRoot.Scale = Vector2.One;
		if (!(num < 1f))
		{
			_previewPhase = PreviewPhase.Idle;
			SetProcess(enable: false);
		}
	}

	private void ApplyEffectVisuals()
	{
		int num = Mathf.Max(0, Mathf.RoundToInt(_targetCost.Value));
		Resource editingEvent = _editingEvent;
		if (!(editingEvent is MowerEventCreateSunConfig))
		{
			if (!(editingEvent is ShovelEventJalapenoConfig))
			{
				if (!(editingEvent is ShovelEventCreateProjectileConfig) && !(editingEvent is ShovelEventCaptainShovelConfig))
				{
					if (!(editingEvent is ShovelEventPacketSpawnConfig))
					{
						if (!(editingEvent is ShovelEventPumpkinShovelConfig))
						{
							if (!(editingEvent is ShovelEventRecycleConfig))
							{
								if (!(editingEvent is ShovelEventSkeletonShovelConfig))
								{
									if (editingEvent is ShovelEventDestroyConfig)
									{
										_effectLabel.Text = "目标被铲除";
										_effectLabel.Visible = true;
									}
									else
									{
										_effectLabel.Text = "工具事件触发";
										_effectLabel.Visible = true;
									}
								}
								else
								{
									_effectLabel.Text = ((num >= 50) ? "复活最近植物" : "费用不足");
									_effectLabel.Visible = true;
								}
							}
							else
							{
								_effectLabel.Text = "费用返还";
								_effectLabel.Visible = true;
							}
						}
						else
						{
							_pumpkinEffect.Visible = true;
						}
					}
					else
					{
						_packetEffect.Visible = true;
					}
				}
				else
				{
					_projectileEffect.Visible = true;
				}
			}
			else
			{
				_fireEffect.Visible = true;
			}
		}
		else
		{
			_sunEffect.Visible = true;
		}
	}

	private bool ShouldFadeTarget()
	{
		if (!(_editingEvent is ShovelEventDestroyConfig) && !(_editingEvent is ShovelEventPumpkinShovelConfig) && !(_editingEvent is ShovelEventRecycleConfig { destroy: not false }))
		{
			if (_editingEvent is ShovelEventJalapenoConfig shovelEventJalapenoConfig)
			{
				return shovelEventJalapenoConfig.destroy;
			}
			return false;
		}
		return true;
	}

	private void ResetEffectVisuals()
	{
		_previewPhase = PreviewPhase.Idle;
		_previewElapsed = 0.0;
		_effectApplied = false;
		SetProcess(enable: false);
		if (GodotObject.IsInstanceValid(_toolRoot))
		{
			_toolRoot.Position = new Vector2(350f, 90f);
			_toolRoot.Rotation = 0f;
			_toolRoot.Scale = Vector2.One;
			_toolRoot.Modulate = Colors.White;
		}
		if (GodotObject.IsInstanceValid(_sunEffect))
		{
			_sunEffect.Visible = false;
			_sunEffect.Position = new Vector2(588f, 188f);
			_sunEffect.Scale = Vector2.One;
			_sunEffect.Modulate = Colors.White;
		}
		if (GodotObject.IsInstanceValid(_projectileEffect))
		{
			_projectileEffect.Visible = false;
			_projectileEffect.Position = new Vector2(560f, 292f);
			_projectileEffect.Scale = Vector2.One;
			_projectileEffect.Modulate = Colors.White;
		}
		if (GodotObject.IsInstanceValid(_packetEffect))
		{
			_packetEffect.Visible = false;
			_packetEffect.Scale = Vector2.One;
			_packetEffect.Modulate = Colors.White;
		}
		if (GodotObject.IsInstanceValid(_fireEffect))
		{
			_fireEffect.Visible = false;
			_fireEffect.Modulate = Colors.White;
		}
		if (GodotObject.IsInstanceValid(_pumpkinEffect))
		{
			_pumpkinEffect.Visible = false;
			_pumpkinEffect.Scale = Vector2.One;
			_pumpkinEffect.Modulate = Colors.White;
		}
		if (GodotObject.IsInstanceValid(_effectLabel))
		{
			_effectLabel.Visible = false;
			_effectLabel.Modulate = Colors.White;
		}
	}

	private void UpdatePreviewProcessingVisibility()
	{
		bool flag = IsVisibleInTree();
		if (GodotObject.IsInstanceValid(_targetPlant))
		{
			_targetPlant.ProcessMode = (ProcessModeEnum)(flag ? 3 : 4);
		}
		if (!flag)
		{
			SetProcess(enable: false);
		}
		else if (_previewPhase != PreviewPhase.Idle)
		{
			SetProcess(enable: true);
		}
	}

	private void AddSummaryRows()
	{
		AddItemIfMissing(PreviewList, "工具事件 -> " + _editingEvent.GetType().Name);
		AddItemIfMissing(TimelineList, "点击“演示工具效果” -> 工具落下 -> 游戏效果 -> 回位");
		AddItemIfMissing(GraphList, "工具配置 -> 模拟目标 -> 只读结果推演（不调用 Execute）");
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
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsBuiltInToolEventWithCompleteVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindDirectEditControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "toolEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateEventFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnToolEventPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshDirectEditPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshToolEventFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeToolEventBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSimulationPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildOutcome, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "health", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "izm", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCaptainOutcome, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSpawnCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "everyNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmptyName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartToolEffectPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateToolDownPhase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateEffectPhase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateRecoverPhase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyEffectVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldFadeTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetEffectVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePreviewProcessingVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsBuiltInToolEventWithCompleteVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBuiltInToolEventWithCompleteVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BindWorkbench && args.Count == 1)
		{
			BindWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindDirectEditControls && args.Count == 1)
		{
			BindDirectEditControls(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateEventFields && args.Count == 0)
		{
			PopulateEventFields();
			ret = default;
			return true;
		}
		if (method == MethodName.OnToolEventPropertyEdited && args.Count == 1)
		{
			OnToolEventPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDirectEditPreview && args.Count == 0)
		{
			RefreshDirectEditPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshToolEventFromHistory && args.Count == 0)
		{
			RefreshToolEventFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeToolEventBinding && args.Count == 0)
		{
			DisposeToolEventBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSimulationPreview && args.Count == 0)
		{
			UpdateSimulationPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildOutcome && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(BuildOutcome(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.BuildCaptainOutcome && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCaptainOutcome(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSpawnCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetSpawnCount(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.EmptyName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.StartToolEffectPreview && args.Count == 0)
		{
			StartToolEffectPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateToolDownPhase && args.Count == 0)
		{
			UpdateToolDownPhase();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateEffectPhase && args.Count == 0)
		{
			UpdateEffectPhase();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRecoverPhase && args.Count == 0)
		{
			UpdateRecoverPhase();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyEffectVisuals && args.Count == 0)
		{
			ApplyEffectVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldFadeTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldFadeTarget());
			return true;
		}
		if (method == MethodName.ResetEffectVisuals && args.Count == 0)
		{
			ResetEffectVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreviewProcessingVisibility && args.Count == 0)
		{
			UpdatePreviewProcessingVisibility();
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
		if (method == MethodName.IsBuiltInToolEventWithCompleteVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBuiltInToolEventWithCompleteVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCaptainOutcome && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCaptainOutcome(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSpawnCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetSpawnCount(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.IsBuiltInToolEventWithCompleteVisualCoverage)
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
		if (method == MethodName.PopulateEventFields)
		{
			return true;
		}
		if (method == MethodName.OnToolEventPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshDirectEditPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshToolEventFromHistory)
		{
			return true;
		}
		if (method == MethodName.DisposeToolEventBinding)
		{
			return true;
		}
		if (method == MethodName.UpdateSimulationPreview)
		{
			return true;
		}
		if (method == MethodName.BuildOutcome)
		{
			return true;
		}
		if (method == MethodName.BuildCaptainOutcome)
		{
			return true;
		}
		if (method == MethodName.GetSpawnCount)
		{
			return true;
		}
		if (method == MethodName.EmptyName)
		{
			return true;
		}
		if (method == MethodName.StartToolEffectPreview)
		{
			return true;
		}
		if (method == MethodName.UpdateToolDownPhase)
		{
			return true;
		}
		if (method == MethodName.UpdateEffectPhase)
		{
			return true;
		}
		if (method == MethodName.UpdateRecoverPhase)
		{
			return true;
		}
		if (method == MethodName.ApplyEffectVisuals)
		{
			return true;
		}
		if (method == MethodName.ShouldFadeTarget)
		{
			return true;
		}
		if (method == MethodName.ResetEffectVisuals)
		{
			return true;
		}
		if (method == MethodName.UpdatePreviewProcessingVisibility)
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
			_editingEvent = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._eventType)
		{
			_eventType = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._outcome)
		{
			_outcome = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._formHint)
		{
			_formHint = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._baseFields)
		{
			_baseFields = VariantUtils.ConvertTo<Control>(in value);
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
		if (name == PropertyName._createSunFields)
		{
			_createSunFields = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._recycleFields)
		{
			_recycleFields = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._jalapenoFields)
		{
			_jalapenoFields = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._projectileFields)
		{
			_projectileFields = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._packetFields)
		{
			_packetFields = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._sunNum)
		{
			_sunNum = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._recyclePercentage)
		{
			_recyclePercentage = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._recycleDestroy)
		{
			_recycleDestroy = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._jalapenoDestroy)
		{
			_jalapenoDestroy = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._projectileEveryNum)
		{
			_projectileEveryNum = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			_projectileName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._projectileDamage)
		{
			_projectileDamage = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._packetName)
		{
			_packetName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._packetEveryNum)
		{
			_packetEveryNum = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._targetCost)
		{
			_targetCost = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._targetHealth)
		{
			_targetHealth = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._healthValue)
		{
			_healthValue = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._izmMode)
		{
			_izmMode = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._targetRoot)
		{
			_targetRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._targetPlant)
		{
			_targetPlant = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._targetHealthBar)
		{
			_targetHealthBar = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._toolRoot)
		{
			_toolRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._shovelTool)
		{
			_shovelTool = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._mowerTool)
		{
			_mowerTool = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._sunEffect)
		{
			_sunEffect = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._projectileEffect)
		{
			_projectileEffect = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._packetEffect)
		{
			_packetEffect = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._fireEffect)
		{
			_fireEffect = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName._pumpkinEffect)
		{
			_pumpkinEffect = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._effectLabel)
		{
			_effectLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewPhase)
		{
			_previewPhase = VariantUtils.ConvertTo<PreviewPhase>(in value);
			return true;
		}
		if (name == PropertyName._previewElapsed)
		{
			_previewElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._effectApplied)
		{
			_effectApplied = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._outcome)
		{
			value = VariantUtils.CreateFrom(in _outcome);
			return true;
		}
		if (name == PropertyName._formHint)
		{
			value = VariantUtils.CreateFrom(in _formHint);
			return true;
		}
		if (name == PropertyName._baseFields)
		{
			value = VariantUtils.CreateFrom(in _baseFields);
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
		if (name == PropertyName._createSunFields)
		{
			value = VariantUtils.CreateFrom(in _createSunFields);
			return true;
		}
		if (name == PropertyName._recycleFields)
		{
			value = VariantUtils.CreateFrom(in _recycleFields);
			return true;
		}
		if (name == PropertyName._jalapenoFields)
		{
			value = VariantUtils.CreateFrom(in _jalapenoFields);
			return true;
		}
		if (name == PropertyName._projectileFields)
		{
			value = VariantUtils.CreateFrom(in _projectileFields);
			return true;
		}
		if (name == PropertyName._packetFields)
		{
			value = VariantUtils.CreateFrom(in _packetFields);
			return true;
		}
		if (name == PropertyName._sunNum)
		{
			value = VariantUtils.CreateFrom(in _sunNum);
			return true;
		}
		if (name == PropertyName._recyclePercentage)
		{
			value = VariantUtils.CreateFrom(in _recyclePercentage);
			return true;
		}
		if (name == PropertyName._recycleDestroy)
		{
			value = VariantUtils.CreateFrom(in _recycleDestroy);
			return true;
		}
		if (name == PropertyName._jalapenoDestroy)
		{
			value = VariantUtils.CreateFrom(in _jalapenoDestroy);
			return true;
		}
		if (name == PropertyName._projectileEveryNum)
		{
			value = VariantUtils.CreateFrom(in _projectileEveryNum);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			value = VariantUtils.CreateFrom(in _projectileName);
			return true;
		}
		if (name == PropertyName._projectileDamage)
		{
			value = VariantUtils.CreateFrom(in _projectileDamage);
			return true;
		}
		if (name == PropertyName._packetName)
		{
			value = VariantUtils.CreateFrom(in _packetName);
			return true;
		}
		if (name == PropertyName._packetEveryNum)
		{
			value = VariantUtils.CreateFrom(in _packetEveryNum);
			return true;
		}
		if (name == PropertyName._targetCost)
		{
			value = VariantUtils.CreateFrom(in _targetCost);
			return true;
		}
		if (name == PropertyName._targetHealth)
		{
			value = VariantUtils.CreateFrom(in _targetHealth);
			return true;
		}
		if (name == PropertyName._healthValue)
		{
			value = VariantUtils.CreateFrom(in _healthValue);
			return true;
		}
		if (name == PropertyName._izmMode)
		{
			value = VariantUtils.CreateFrom(in _izmMode);
			return true;
		}
		if (name == PropertyName._targetRoot)
		{
			value = VariantUtils.CreateFrom(in _targetRoot);
			return true;
		}
		if (name == PropertyName._targetPlant)
		{
			value = VariantUtils.CreateFrom(in _targetPlant);
			return true;
		}
		if (name == PropertyName._targetHealthBar)
		{
			value = VariantUtils.CreateFrom(in _targetHealthBar);
			return true;
		}
		if (name == PropertyName._toolRoot)
		{
			value = VariantUtils.CreateFrom(in _toolRoot);
			return true;
		}
		if (name == PropertyName._shovelTool)
		{
			value = VariantUtils.CreateFrom(in _shovelTool);
			return true;
		}
		if (name == PropertyName._mowerTool)
		{
			value = VariantUtils.CreateFrom(in _mowerTool);
			return true;
		}
		if (name == PropertyName._sunEffect)
		{
			value = VariantUtils.CreateFrom(in _sunEffect);
			return true;
		}
		if (name == PropertyName._projectileEffect)
		{
			value = VariantUtils.CreateFrom(in _projectileEffect);
			return true;
		}
		if (name == PropertyName._packetEffect)
		{
			value = VariantUtils.CreateFrom(in _packetEffect);
			return true;
		}
		if (name == PropertyName._fireEffect)
		{
			value = VariantUtils.CreateFrom(in _fireEffect);
			return true;
		}
		if (name == PropertyName._pumpkinEffect)
		{
			value = VariantUtils.CreateFrom(in _pumpkinEffect);
			return true;
		}
		if (name == PropertyName._effectLabel)
		{
			value = VariantUtils.CreateFrom(in _effectLabel);
			return true;
		}
		if (name == PropertyName._previewPhase)
		{
			value = VariantUtils.CreateFrom(in _previewPhase);
			return true;
		}
		if (name == PropertyName._previewElapsed)
		{
			value = VariantUtils.CreateFrom(in _previewElapsed);
			return true;
		}
		if (name == PropertyName._effectApplied)
		{
			value = VariantUtils.CreateFrom(in _effectApplied);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._outcome, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._formHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._baseFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._createSunFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._recycleFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jalapenoFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._recyclePercentage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._recycleDestroy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jalapenoDestroy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileEveryNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileDamage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetEveryNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetCost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetHealth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._healthValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._izmMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetHealthBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._toolRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shovelTool, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mowerTool, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectileEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fireEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pumpkinEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._effectLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._previewPhase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._previewElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._effectApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingEvent, Variant.From(in _editingEvent));
		info.AddProperty(PropertyName._eventType, Variant.From(in _eventType));
		info.AddProperty(PropertyName._outcome, Variant.From(in _outcome));
		info.AddProperty(PropertyName._formHint, Variant.From(in _formHint));
		info.AddProperty(PropertyName._baseFields, Variant.From(in _baseFields));
		info.AddProperty(PropertyName._resourceName, Variant.From(in _resourceName));
		info.AddProperty(PropertyName._localToScene, Variant.From(in _localToScene));
		info.AddProperty(PropertyName._createSunFields, Variant.From(in _createSunFields));
		info.AddProperty(PropertyName._recycleFields, Variant.From(in _recycleFields));
		info.AddProperty(PropertyName._jalapenoFields, Variant.From(in _jalapenoFields));
		info.AddProperty(PropertyName._projectileFields, Variant.From(in _projectileFields));
		info.AddProperty(PropertyName._packetFields, Variant.From(in _packetFields));
		info.AddProperty(PropertyName._sunNum, Variant.From(in _sunNum));
		info.AddProperty(PropertyName._recyclePercentage, Variant.From(in _recyclePercentage));
		info.AddProperty(PropertyName._recycleDestroy, Variant.From(in _recycleDestroy));
		info.AddProperty(PropertyName._jalapenoDestroy, Variant.From(in _jalapenoDestroy));
		info.AddProperty(PropertyName._projectileEveryNum, Variant.From(in _projectileEveryNum));
		info.AddProperty(PropertyName._projectileName, Variant.From(in _projectileName));
		info.AddProperty(PropertyName._projectileDamage, Variant.From(in _projectileDamage));
		info.AddProperty(PropertyName._packetName, Variant.From(in _packetName));
		info.AddProperty(PropertyName._packetEveryNum, Variant.From(in _packetEveryNum));
		info.AddProperty(PropertyName._targetCost, Variant.From(in _targetCost));
		info.AddProperty(PropertyName._targetHealth, Variant.From(in _targetHealth));
		info.AddProperty(PropertyName._healthValue, Variant.From(in _healthValue));
		info.AddProperty(PropertyName._izmMode, Variant.From(in _izmMode));
		info.AddProperty(PropertyName._targetRoot, Variant.From(in _targetRoot));
		info.AddProperty(PropertyName._targetPlant, Variant.From(in _targetPlant));
		info.AddProperty(PropertyName._targetHealthBar, Variant.From(in _targetHealthBar));
		info.AddProperty(PropertyName._toolRoot, Variant.From(in _toolRoot));
		info.AddProperty(PropertyName._shovelTool, Variant.From(in _shovelTool));
		info.AddProperty(PropertyName._mowerTool, Variant.From(in _mowerTool));
		info.AddProperty(PropertyName._sunEffect, Variant.From(in _sunEffect));
		info.AddProperty(PropertyName._projectileEffect, Variant.From(in _projectileEffect));
		info.AddProperty(PropertyName._packetEffect, Variant.From(in _packetEffect));
		info.AddProperty(PropertyName._fireEffect, Variant.From(in _fireEffect));
		info.AddProperty(PropertyName._pumpkinEffect, Variant.From(in _pumpkinEffect));
		info.AddProperty(PropertyName._effectLabel, Variant.From(in _effectLabel));
		info.AddProperty(PropertyName._previewPhase, Variant.From(in _previewPhase));
		info.AddProperty(PropertyName._previewElapsed, Variant.From(in _previewElapsed));
		info.AddProperty(PropertyName._effectApplied, Variant.From(in _effectApplied));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingEvent, out var value))
		{
			_editingEvent = value.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._eventType, out var value2))
		{
			_eventType = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._outcome, out var value3))
		{
			_outcome = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._formHint, out var value4))
		{
			_formHint = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._baseFields, out var value5))
		{
			_baseFields = value5.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._resourceName, out var value6))
		{
			_resourceName = value6.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToScene, out var value7))
		{
			_localToScene = value7.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._createSunFields, out var value8))
		{
			_createSunFields = value8.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._recycleFields, out var value9))
		{
			_recycleFields = value9.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._jalapenoFields, out var value10))
		{
			_jalapenoFields = value10.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._projectileFields, out var value11))
		{
			_projectileFields = value11.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._packetFields, out var value12))
		{
			_packetFields = value12.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._sunNum, out var value13))
		{
			_sunNum = value13.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._recyclePercentage, out var value14))
		{
			_recyclePercentage = value14.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._recycleDestroy, out var value15))
		{
			_recycleDestroy = value15.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._jalapenoDestroy, out var value16))
		{
			_jalapenoDestroy = value16.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._projectileEveryNum, out var value17))
		{
			_projectileEveryNum = value17.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._projectileName, out var value18))
		{
			_projectileName = value18.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._projectileDamage, out var value19))
		{
			_projectileDamage = value19.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._packetName, out var value20))
		{
			_packetName = value20.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._packetEveryNum, out var value21))
		{
			_packetEveryNum = value21.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._targetCost, out var value22))
		{
			_targetCost = value22.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._targetHealth, out var value23))
		{
			_targetHealth = value23.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._healthValue, out var value24))
		{
			_healthValue = value24.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._izmMode, out var value25))
		{
			_izmMode = value25.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._targetRoot, out var value26))
		{
			_targetRoot = value26.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._targetPlant, out var value27))
		{
			_targetPlant = value27.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._targetHealthBar, out var value28))
		{
			_targetHealthBar = value28.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._toolRoot, out var value29))
		{
			_toolRoot = value29.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._shovelTool, out var value30))
		{
			_shovelTool = value30.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._mowerTool, out var value31))
		{
			_mowerTool = value31.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._sunEffect, out var value32))
		{
			_sunEffect = value32.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._projectileEffect, out var value33))
		{
			_projectileEffect = value33.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._packetEffect, out var value34))
		{
			_packetEffect = value34.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._fireEffect, out var value35))
		{
			_fireEffect = value35.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName._pumpkinEffect, out var value36))
		{
			_pumpkinEffect = value36.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._effectLabel, out var value37))
		{
			_effectLabel = value37.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewPhase, out var value38))
		{
			_previewPhase = value38.As<PreviewPhase>();
		}
		if (info.TryGetProperty(PropertyName._previewElapsed, out var value39))
		{
			_previewElapsed = value39.As<double>();
		}
		if (info.TryGetProperty(PropertyName._effectApplied, out var value40))
		{
			_effectApplied = value40.As<bool>();
		}
	}
}
