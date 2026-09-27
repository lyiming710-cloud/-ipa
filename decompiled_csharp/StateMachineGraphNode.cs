using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://addons/godot_state_charts/VisualEditor/StateMachineGraphNode.cs")]
public class StateMachineGraphNode : GraphNode
{
	public new class MethodName : GraphNode.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetSimulationState = "SetSimulationState";

		public static readonly StringName Bind = "Bind";

		public static readonly StringName BindHierarchy = "BindHierarchy";

		public static readonly StringName BuildPuzzleTile = "BuildPuzzleTile";

		public static readonly StringName RefreshVisuals = "RefreshVisuals";

		public static readonly StringName GetIndependentActionCount = "GetIndependentActionCount";

		public static readonly StringName RefreshHierarchyVisuals = "RefreshHierarchyVisuals";

		public static readonly StringName RefreshSimulationVisuals = "RefreshSimulationVisuals";
	}

	public new class PropertyName : GraphNode.PropertyName
	{
		public static readonly StringName State = "State";

		public static readonly StringName StableId = "StableId";

		public static readonly StringName IsInherited = "IsInherited";

		public static readonly StringName IsReadOnly = "IsReadOnly";

		public static readonly StringName IsSimulationActive = "IsSimulationActive";

		public static readonly StringName IsSimulationPending = "IsSimulationPending";

		public static readonly StringName HierarchyChildCount = "HierarchyChildCount";

		public static readonly StringName IsHierarchyCollapsed = "IsHierarchyCollapsed";

		public static readonly StringName IsInitialChild = "IsInitialChild";

		public static readonly StringName IsHierarchyRoot = "IsHierarchyRoot";

		public static readonly StringName CanSetAsInitial = "CanSetAsInitial";

		public static readonly StringName ParentStableId = "ParentStableId";

		public static readonly StringName _kindBadge = "_kindBadge";

		public static readonly StringName _identityLabel = "_identityLabel";

		public static readonly StringName _callbackLabel = "_callbackLabel";

		public static readonly StringName _hierarchyBadge = "_hierarchyBadge";

		public static readonly StringName _initialChildBadge = "_initialChildBadge";

		public static readonly StringName _rootStateBadge = "_rootStateBadge";

		public static readonly StringName _lockLabel = "_lockLabel";

		public static readonly StringName _simulationBadge = "_simulationBadge";

		public static readonly StringName _inspectButton = "_inspectButton";

		public static readonly StringName _initialButton = "_initialButton";

		public static readonly StringName _collapseButton = "_collapseButton";

		public static readonly StringName _baseColor = "_baseColor";

		public static readonly StringName _simulationActive = "_simulationActive";

		public static readonly StringName _simulationPending = "_simulationPending";
	}

	public new class SignalName : GraphNode.SignalName
	{
	}

	private Label _kindBadge;

	private Label _identityLabel;

	private Label _callbackLabel;

	private Label _hierarchyBadge;

	private Label _initialChildBadge;

	private Label _rootStateBadge;

	private Label _lockLabel;

	private Label _simulationBadge;

	private Button _inspectButton;

	private Button _initialButton;

	private Button _collapseButton;

	private Color _baseColor = Colors.White;

	private bool _simulationActive;

	private bool _simulationPending;

	public StateMachineStateDefinition State { get; private set; }

	public string StableId => State?.StableId ?? string.Empty;

	public bool IsInherited { get; private set; }

	public bool IsReadOnly { get; private set; }

	public bool IsSimulationActive => _simulationActive;

	public bool IsSimulationPending => _simulationPending;

	public int HierarchyChildCount { get; private set; }

	public bool IsHierarchyCollapsed { get; private set; }

	public bool IsInitialChild { get; private set; }

	public bool IsHierarchyRoot { get; private set; }

	public bool CanSetAsInitial { get; private set; }

	public string ParentStableId { get; private set; } = string.Empty;

	public event Action<StateMachineGraphNode> InspectRequested;

	public event Action<StateMachineGraphNode> InitialStateRequested;

	public event Action<StateMachineGraphNode, bool> CollapseRequested;

	public override void _Ready()
	{
		BuildPuzzleTile();
		RefreshVisuals();
	}

	public void SetSimulationState(bool active, bool pending)
	{
		if (_simulationActive != active || _simulationPending != pending)
		{
			_simulationActive = active;
			_simulationPending = pending;
			RefreshSimulationVisuals();
		}
	}

	public void Bind(StateMachineStateDefinition state, bool isInherited, bool isReadOnly)
	{
		State = state;
		IsInherited = isInherited;
		IsReadOnly = isReadOnly;
		Name = $"State_{GetInstanceId()}";
		RefreshVisuals();
	}

	public void BindHierarchy(string parentStableId, string parentDisplayName, int childCount, bool isInitialChild, bool isHierarchyRoot, bool canSetAsInitial, bool collapsed)
	{
		ParentStableId = parentStableId ?? string.Empty;
		HierarchyChildCount = Math.Max(0, childCount);
		IsInitialChild = isInitialChild;
		IsHierarchyRoot = isHierarchyRoot;
		CanSetAsInitial = canSetAsInitial;
		IsHierarchyCollapsed = collapsed && HierarchyChildCount > 0;
		RefreshHierarchyVisuals(parentDisplayName);
	}

	private void BuildPuzzleTile()
	{
		if (GetNodeOrNull<Control>("PuzzleTile") == null)
		{
			VBoxContainer vBoxContainer = new VBoxContainer
			{
				Name = "PuzzleTile",
				CustomMinimumSize = new Vector2(236f, 136f),
				MouseFilter = MouseFilterEnum.Pass
			};
			AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
			HBoxContainer hBoxContainer = new HBoxContainer
			{
				Name = "HeadingRow",
				MouseFilter = MouseFilterEnum.Pass
			};
			vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
			_kindBadge = new Label
			{
				Name = "KindBadge",
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				MouseFilter = MouseFilterEnum.Ignore
			};
			hBoxContainer.AddChild(_kindBadge, forceReadableName: false, InternalMode.Disabled);
			_simulationBadge = new Label
			{
				Name = "SimulationBadge",
				Visible = false,
				TooltipText = "状态机沙盒中的实时状态",
				MouseFilter = MouseFilterEnum.Ignore
			};
			hBoxContainer.AddChild(_simulationBadge, forceReadableName: false, InternalMode.Disabled);
			_lockLabel = new Label
			{
				Name = "InheritanceBadge",
				TooltipText = "继承节点为只读；可在基定义中修改",
				MouseFilter = MouseFilterEnum.Ignore
			};
			hBoxContainer.AddChild(_lockLabel, forceReadableName: false, InternalMode.Disabled);
			_identityLabel = new Label
			{
				Name = "StableIdLabel",
				AutowrapMode = TextServer.AutowrapMode.WordSmart,
				MouseFilter = MouseFilterEnum.Ignore
			};
			vBoxContainer.AddChild(_identityLabel, forceReadableName: false, InternalMode.Disabled);
			HBoxContainer hBoxContainer2 = new HBoxContainer
			{
				Name = "HierarchyRow",
				MouseFilter = MouseFilterEnum.Pass
			};
			vBoxContainer.AddChild(hBoxContainer2, forceReadableName: false, InternalMode.Disabled);
			_collapseButton = new Button
			{
				Name = "CollapseSubtreeButton",
				CustomMinimumSize = new Vector2(38f, 28f),
				FocusMode = FocusModeEnum.None,
				Visible = false,
				MouseFilter = MouseFilterEnum.Stop,
				Icon = ResourceLoader.Load<Texture2D>("res://addons/godot_state_charts/toggle_sidebar.svg", null, ResourceLoader.CacheMode.Reuse)
			};
			_collapseButton.Pressed += () =>
			{
				CollapseRequested?.Invoke(this, !IsHierarchyCollapsed);
			};
			hBoxContainer2.AddChild(_collapseButton, forceReadableName: false, InternalMode.Disabled);
			_hierarchyBadge = new Label
			{
				Name = "HierarchyBadge",
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				Modulate = new Color(0.72f, 0.82f, 0.96f),
				MouseFilter = MouseFilterEnum.Ignore
			};
			hBoxContainer2.AddChild(_hierarchyBadge, forceReadableName: false, InternalMode.Disabled);
			_initialChildBadge = new Label
			{
				Name = "InitialChildBadge",
				Text = "★ 入口",
				TooltipText = "父复合状态进入时首先激活这个子状态",
				Visible = false,
				Modulate = new Color(1f, 0.78f, 0.28f),
				MouseFilter = MouseFilterEnum.Ignore
			};
			hBoxContainer2.AddChild(_initialChildBadge, forceReadableName: false, InternalMode.Disabled);
			_rootStateBadge = new Label
			{
				Name = "RootStateBadge",
				Text = "♛ 根",
				TooltipText = "状态机层级根状态",
				Visible = false,
				Modulate = new Color(0.56f, 0.92f, 1f),
				MouseFilter = MouseFilterEnum.Ignore
			};
			hBoxContainer2.AddChild(_rootStateBadge, forceReadableName: false, InternalMode.Disabled);
			_callbackLabel = new Label
			{
				Name = "CallbackLabel",
				Modulate = new Color(0.76f, 0.82f, 0.9f),
				MouseFilter = MouseFilterEnum.Ignore
			};
			vBoxContainer.AddChild(_callbackLabel, forceReadableName: false, InternalMode.Disabled);
			HBoxContainer hBoxContainer3 = new HBoxContainer
			{
				Name = "ActionRow",
				MouseFilter = MouseFilterEnum.Pass
			};
			vBoxContainer.AddChild(hBoxContainer3, forceReadableName: false, InternalMode.Disabled);
			_inspectButton = new Button
			{
				Name = "InspectButton",
				Text = "配置拼图",
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				MouseFilter = MouseFilterEnum.Stop
			};
			_inspectButton.Pressed += () =>
			{
				InspectRequested?.Invoke(this);
			};
			hBoxContainer3.AddChild(_inspectButton, forceReadableName: false, InternalMode.Disabled);
			_initialButton = new Button
			{
				Name = "InitialButton",
				Text = "设为入口",
				TooltipText = "设为父复合状态的初始子状态",
				MouseFilter = MouseFilterEnum.Stop
			};
			_initialButton.Pressed += () =>
			{
				InitialStateRequested?.Invoke(this);
			};
			hBoxContainer3.AddChild(_initialButton, forceReadableName: false, InternalMode.Disabled);
			SetSlot(0, enableLeftPort: true, 0, new Color(0.28f, 0.78f, 1f), enableRightPort: true, 0, new Color(1f, 0.66f, 0.24f));
		}
	}

	private void RefreshVisuals()
	{
		if (_kindBadge != null)
		{
			StateMachineStateKind stateMachineStateKind = State?.Kind ?? StateMachineStateKind.Atomic;
			Title = ((State == null || State.DisplayName.IsEmpty) ? "未命名状态" : State.DisplayName.ToString());
			Label kindBadge = _kindBadge;
			kindBadge.Text = stateMachineStateKind switch
			{
				StateMachineStateKind.Atomic => "◆ 原子状态", 
				StateMachineStateKind.Compound => "▣ 复合状态", 
				StateMachineStateKind.Parallel => "▥ 并行状态", 
				StateMachineStateKind.History => "◴ 历史状态", 
				_ => stateMachineStateKind.ToString(), 
			};
			_identityLabel.Text = (string.IsNullOrWhiteSpace(StableId) ? "ID 尚未生成" : StableId);
			int independentActionCount = GetIndependentActionCount(State);
			if (independentActionCount > 0)
			{
				_callbackLabel.Text = $"◆ 生命周期拼图 {independentActionCount}/4";
			}
			else
			{
				string text = State?.CallbackKey.ToString() ?? string.Empty;
				Label callbackLabel = _callbackLabel;
				string text2;
				if (string.IsNullOrWhiteSpace(text))
				{
					text2 = "生命周期动作：无";
				}
				else
				{
					text2 = (StateMachineCallbackKey.UsesExecutablePrefix(text) ? ("生命周期动作：" + text) : ("旧版性能标签：" + text));
				}
				callbackLabel.Text = text2;
			}
			Label lockLabel = _lockLabel;
			string text3;
			if (IsInherited)
			{
				text3 = "\ud83d\udd12 继承";
			}
			else
			{
				text3 = (IsReadOnly ? "\ud83d\udd12 只读" : "可编辑");
			}
			lockLabel.Text = text3;
			_inspectButton.Disabled = State == null;
			_initialButton.Disabled = IsReadOnly || State == null || string.IsNullOrWhiteSpace(State.ParentId);
			Color color = stateMachineStateKind switch
			{
				StateMachineStateKind.Atomic => new Color(0.16f, 0.58f, 0.84f), 
				StateMachineStateKind.Compound => new Color(0.48f, 0.34f, 0.82f), 
				StateMachineStateKind.Parallel => new Color(0.18f, 0.7f, 0.52f), 
				StateMachineStateKind.History => new Color(0.84f, 0.54f, 0.18f), 
				_ => new Color(0.35f, 0.4f, 0.5f), 
			};
			_baseColor = (IsInherited ? color.Darkened(0.38f) : color);
			RefreshHierarchyVisuals();
			RefreshSimulationVisuals();
		}
	}

	private static int GetIndependentActionCount(StateMachineStateDefinition state)
	{
		if (state == null)
		{
			return 0;
		}
		int num = 0;
		if (!state.EnterCallbackKey.IsEmpty)
		{
			num++;
		}
		if (!state.ExitCallbackKey.IsEmpty)
		{
			num++;
		}
		if (!state.ProcessCallbackKey.IsEmpty)
		{
			num++;
		}
		if (!state.PhysicsProcessCallbackKey.IsEmpty)
		{
			num++;
		}
		return num;
	}

	private void RefreshHierarchyVisuals(string parentDisplayName = "")
	{
		if (_hierarchyBadge != null && _collapseButton != null && _initialChildBadge != null && _rootStateBadge != null)
		{
			StateMachineStateKind stateMachineStateKind = State?.Kind ?? StateMachineStateKind.Atomic;
			bool flag = (uint)(stateMachineStateKind - 1) <= 1u;
			bool flag2 = flag && HierarchyChildCount > 0;
			_collapseButton.Visible = flag2;
			_collapseButton.Text = (IsHierarchyCollapsed ? "▸" : "▾");
			_collapseButton.TooltipText = (IsHierarchyCollapsed ? $"展开 {HierarchyChildCount} 个直属子状态" : $"折叠并收纳 {HierarchyChildCount} 个直属子状态及其后代");
			_initialChildBadge.Visible = IsInitialChild;
			_rootStateBadge.Visible = IsHierarchyRoot;
			if (_initialButton != null)
			{
				_initialButton.Visible = CanSetAsInitial;
				_initialButton.Disabled = !CanSetAsInitial;
			}
			string text = (string.IsNullOrWhiteSpace(parentDisplayName) ? ParentStableId : parentDisplayName);
			if (flag2)
			{
				_hierarchyBadge.Text = (IsHierarchyCollapsed ? $"◫ {HierarchyChildCount} 子状态已收纳" : $"◩ {HierarchyChildCount} 子状态");
			}
			else if (!string.IsNullOrWhiteSpace(text))
			{
				_hierarchyBadge.Text = "↳ " + text;
			}
			else
			{
				_hierarchyBadge.Text = "◇ 层级根";
			}
		}
	}

	private void RefreshSimulationVisuals()
	{
		if (_simulationBadge != null)
		{
			_simulationBadge.Visible = _simulationActive || _simulationPending;
			Label simulationBadge = _simulationBadge;
			string text;
			if (_simulationPending)
			{
				text = "⏱ 等待";
			}
			else
			{
				text = (_simulationActive ? "● 激活" : string.Empty);
			}
			simulationBadge.Text = text;
			_simulationBadge.Modulate = (_simulationPending ? new Color(1f, 0.78f, 0.28f) : new Color(0.42f, 1f, 0.58f));
			StateMachineGraphNode stateMachineGraphNode = this;
			Color selfModulate;
			if (_simulationPending)
			{
				selfModulate = _baseColor.Lerp(new Color(1f, 0.72f, 0.18f), 0.46f);
			}
			else if (_simulationActive)
			{
				selfModulate = _baseColor.Lightened(0.28f);
			}
			else
			{
				selfModulate = (IsHierarchyCollapsed ? _baseColor.Lerp(new Color(0.16f, 0.22f, 0.34f), 0.14f) : _baseColor);
			}
			stateMachineGraphNode.SelfModulate = selfModulate;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetSimulationState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "pending", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "state", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "isInherited", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isReadOnly", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindHierarchy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "parentStableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "parentDisplayName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "childCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isInitialChild", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isHierarchyRoot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canSetAsInitial", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "collapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPuzzleTile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetIndependentActionCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "state", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshHierarchyVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "parentDisplayName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshSimulationVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetSimulationState && args.Count == 2)
		{
			SetSimulationState(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Bind && args.Count == 3)
		{
			Bind(VariantUtils.ConvertTo<StateMachineStateDefinition>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindHierarchy && args.Count == 7)
		{
			BindHierarchy(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildPuzzleTile && args.Count == 0)
		{
			BuildPuzzleTile();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshVisuals && args.Count == 0)
		{
			RefreshVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.GetIndependentActionCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetIndependentActionCount(VariantUtils.ConvertTo<StateMachineStateDefinition>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshHierarchyVisuals && args.Count == 1)
		{
			RefreshHierarchyVisuals(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSimulationVisuals && args.Count == 0)
		{
			RefreshSimulationVisuals();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetIndependentActionCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetIndependentActionCount(VariantUtils.ConvertTo<StateMachineStateDefinition>(in args[0])));
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
		if (method == MethodName.SetSimulationState)
		{
			return true;
		}
		if (method == MethodName.Bind)
		{
			return true;
		}
		if (method == MethodName.BindHierarchy)
		{
			return true;
		}
		if (method == MethodName.BuildPuzzleTile)
		{
			return true;
		}
		if (method == MethodName.RefreshVisuals)
		{
			return true;
		}
		if (method == MethodName.GetIndependentActionCount)
		{
			return true;
		}
		if (method == MethodName.RefreshHierarchyVisuals)
		{
			return true;
		}
		if (method == MethodName.RefreshSimulationVisuals)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.State)
		{
			State = VariantUtils.ConvertTo<StateMachineStateDefinition>(in value);
			return true;
		}
		if (name == PropertyName.IsInherited)
		{
			IsInherited = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsReadOnly)
		{
			IsReadOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.HierarchyChildCount)
		{
			HierarchyChildCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.IsHierarchyCollapsed)
		{
			IsHierarchyCollapsed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsInitialChild)
		{
			IsInitialChild = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsHierarchyRoot)
		{
			IsHierarchyRoot = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.CanSetAsInitial)
		{
			CanSetAsInitial = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ParentStableId)
		{
			ParentStableId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._kindBadge)
		{
			_kindBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._identityLabel)
		{
			_identityLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._callbackLabel)
		{
			_callbackLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._hierarchyBadge)
		{
			_hierarchyBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._initialChildBadge)
		{
			_initialChildBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._rootStateBadge)
		{
			_rootStateBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._lockLabel)
		{
			_lockLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._simulationBadge)
		{
			_simulationBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._inspectButton)
		{
			_inspectButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._initialButton)
		{
			_initialButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._collapseButton)
		{
			_collapseButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._baseColor)
		{
			_baseColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._simulationActive)
		{
			_simulationActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._simulationPending)
		{
			_simulationPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.State)
		{
			value = VariantUtils.CreateFrom<StateMachineStateDefinition>(State);
			return true;
		}
		string from;
		if (name == PropertyName.StableId)
		{
			from = StableId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.IsInherited)
		{
			from2 = IsInherited;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsReadOnly)
		{
			from2 = IsReadOnly;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsSimulationActive)
		{
			from2 = IsSimulationActive;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsSimulationPending)
		{
			from2 = IsSimulationPending;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.HierarchyChildCount)
		{
			value = VariantUtils.CreateFrom<int>(HierarchyChildCount);
			return true;
		}
		if (name == PropertyName.IsHierarchyCollapsed)
		{
			from2 = IsHierarchyCollapsed;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsInitialChild)
		{
			from2 = IsInitialChild;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsHierarchyRoot)
		{
			from2 = IsHierarchyRoot;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CanSetAsInitial)
		{
			from2 = CanSetAsInitial;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ParentStableId)
		{
			from = ParentStableId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._kindBadge)
		{
			value = VariantUtils.CreateFrom(in _kindBadge);
			return true;
		}
		if (name == PropertyName._identityLabel)
		{
			value = VariantUtils.CreateFrom(in _identityLabel);
			return true;
		}
		if (name == PropertyName._callbackLabel)
		{
			value = VariantUtils.CreateFrom(in _callbackLabel);
			return true;
		}
		if (name == PropertyName._hierarchyBadge)
		{
			value = VariantUtils.CreateFrom(in _hierarchyBadge);
			return true;
		}
		if (name == PropertyName._initialChildBadge)
		{
			value = VariantUtils.CreateFrom(in _initialChildBadge);
			return true;
		}
		if (name == PropertyName._rootStateBadge)
		{
			value = VariantUtils.CreateFrom(in _rootStateBadge);
			return true;
		}
		if (name == PropertyName._lockLabel)
		{
			value = VariantUtils.CreateFrom(in _lockLabel);
			return true;
		}
		if (name == PropertyName._simulationBadge)
		{
			value = VariantUtils.CreateFrom(in _simulationBadge);
			return true;
		}
		if (name == PropertyName._inspectButton)
		{
			value = VariantUtils.CreateFrom(in _inspectButton);
			return true;
		}
		if (name == PropertyName._initialButton)
		{
			value = VariantUtils.CreateFrom(in _initialButton);
			return true;
		}
		if (name == PropertyName._collapseButton)
		{
			value = VariantUtils.CreateFrom(in _collapseButton);
			return true;
		}
		if (name == PropertyName._baseColor)
		{
			value = VariantUtils.CreateFrom(in _baseColor);
			return true;
		}
		if (name == PropertyName._simulationActive)
		{
			value = VariantUtils.CreateFrom(in _simulationActive);
			return true;
		}
		if (name == PropertyName._simulationPending)
		{
			value = VariantUtils.CreateFrom(in _simulationPending);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.State, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.StableId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsInherited, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsReadOnly, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsSimulationActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsSimulationPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.HierarchyChildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHierarchyCollapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsInitialChild, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHierarchyRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanSetAsInitial, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ParentStableId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._kindBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._identityLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._callbackLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hierarchyBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._initialChildBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rootStateBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lockLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulationBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inspectButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._initialButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._collapseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._baseColor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._simulationActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._simulationPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.State, Variant.From<StateMachineStateDefinition>(State));
		info.AddProperty(PropertyName.IsInherited, Variant.From<bool>(IsInherited));
		info.AddProperty(PropertyName.IsReadOnly, Variant.From<bool>(IsReadOnly));
		info.AddProperty(PropertyName.HierarchyChildCount, Variant.From<int>(HierarchyChildCount));
		info.AddProperty(PropertyName.IsHierarchyCollapsed, Variant.From<bool>(IsHierarchyCollapsed));
		info.AddProperty(PropertyName.IsInitialChild, Variant.From<bool>(IsInitialChild));
		info.AddProperty(PropertyName.IsHierarchyRoot, Variant.From<bool>(IsHierarchyRoot));
		info.AddProperty(PropertyName.CanSetAsInitial, Variant.From<bool>(CanSetAsInitial));
		info.AddProperty(PropertyName.ParentStableId, Variant.From<string>(ParentStableId));
		info.AddProperty(PropertyName._kindBadge, Variant.From(in _kindBadge));
		info.AddProperty(PropertyName._identityLabel, Variant.From(in _identityLabel));
		info.AddProperty(PropertyName._callbackLabel, Variant.From(in _callbackLabel));
		info.AddProperty(PropertyName._hierarchyBadge, Variant.From(in _hierarchyBadge));
		info.AddProperty(PropertyName._initialChildBadge, Variant.From(in _initialChildBadge));
		info.AddProperty(PropertyName._rootStateBadge, Variant.From(in _rootStateBadge));
		info.AddProperty(PropertyName._lockLabel, Variant.From(in _lockLabel));
		info.AddProperty(PropertyName._simulationBadge, Variant.From(in _simulationBadge));
		info.AddProperty(PropertyName._inspectButton, Variant.From(in _inspectButton));
		info.AddProperty(PropertyName._initialButton, Variant.From(in _initialButton));
		info.AddProperty(PropertyName._collapseButton, Variant.From(in _collapseButton));
		info.AddProperty(PropertyName._baseColor, Variant.From(in _baseColor));
		info.AddProperty(PropertyName._simulationActive, Variant.From(in _simulationActive));
		info.AddProperty(PropertyName._simulationPending, Variant.From(in _simulationPending));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.State, out var value))
		{
			State = value.As<StateMachineStateDefinition>();
		}
		if (info.TryGetProperty(PropertyName.IsInherited, out var value2))
		{
			IsInherited = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsReadOnly, out var value3))
		{
			IsReadOnly = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.HierarchyChildCount, out var value4))
		{
			HierarchyChildCount = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.IsHierarchyCollapsed, out var value5))
		{
			IsHierarchyCollapsed = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsInitialChild, out var value6))
		{
			IsInitialChild = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsHierarchyRoot, out var value7))
		{
			IsHierarchyRoot = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.CanSetAsInitial, out var value8))
		{
			CanSetAsInitial = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ParentStableId, out var value9))
		{
			ParentStableId = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName._kindBadge, out var value10))
		{
			_kindBadge = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._identityLabel, out var value11))
		{
			_identityLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._callbackLabel, out var value12))
		{
			_callbackLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._hierarchyBadge, out var value13))
		{
			_hierarchyBadge = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._initialChildBadge, out var value14))
		{
			_initialChildBadge = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._rootStateBadge, out var value15))
		{
			_rootStateBadge = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._lockLabel, out var value16))
		{
			_lockLabel = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._simulationBadge, out var value17))
		{
			_simulationBadge = value17.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._inspectButton, out var value18))
		{
			_inspectButton = value18.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._initialButton, out var value19))
		{
			_initialButton = value19.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._collapseButton, out var value20))
		{
			_collapseButton = value20.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._baseColor, out var value21))
		{
			_baseColor = value21.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._simulationActive, out var value22))
		{
			_simulationActive = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._simulationPending, out var value23))
		{
			_simulationPending = value23.As<bool>();
		}
	}
}
