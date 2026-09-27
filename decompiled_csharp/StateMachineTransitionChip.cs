using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://addons/godot_state_charts/VisualEditor/StateMachineTransitionChip.cs")]
public class StateMachineTransitionChip : Button
{
	public new class MethodName : Button.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName SetSelectedTransition = "SetSelectedTransition";

		public static readonly StringName SetRuntimeState = "SetRuntimeState";

		public static readonly StringName OnPressed = "OnPressed";

		public static readonly StringName ConfigureStyleBox = "ConfigureStyleBox";

		public static readonly StringName RefreshVisualState = "RefreshVisualState";

		public static readonly StringName BuildBaseText = "BuildBaseText";

		public static readonly StringName BuildTooltip = "BuildTooltip";

		public static readonly StringName ResolveIconPath = "ResolveIconPath";

		public static readonly StringName ResolveTriggerColor = "ResolveTriggerColor";
	}

	public new class PropertyName : Button.PropertyName
	{
		public static readonly StringName StableId = "StableId";

		public static readonly StringName SourceStateId = "SourceStateId";

		public static readonly StringName TargetStateId = "TargetStateId";

		public static readonly StringName Priority = "Priority";

		public static readonly StringName DeclarationOrder = "DeclarationOrder";

		public static readonly StringName IsPending = "IsPending";

		public static readonly StringName IsRecentlyCompleted = "IsRecentlyCompleted";

		public static readonly StringName IsSelectedTransition = "IsSelectedTransition";

		public static readonly StringName PendingDelayRemaining = "PendingDelayRemaining";

		public static readonly StringName VisualUpdateCount = "VisualUpdateCount";

		public static readonly StringName _style = "_style";

		public static readonly StringName _triggerKind = "_triggerKind";

		public static readonly StringName _eventName = "_eventName";

		public static readonly StringName _delaySeconds = "_delaySeconds";

		public static readonly StringName _hasGuard = "_hasGuard";

		public static readonly StringName _isReadOnly = "_isReadOnly";

		public static readonly StringName _showPriority = "_showPriority";

		public static readonly StringName _sourceDisplayName = "_sourceDisplayName";

		public static readonly StringName _targetDisplayName = "_targetDisplayName";

		public static readonly StringName _baseText = "_baseText";

		public static readonly StringName _pendingTenths = "_pendingTenths";
	}

	public new class SignalName : Button.SignalName
	{
	}

	private const string EventIconPath = "res://addons/ModEditor/Icons/ClassIcon/KeyNext.svg";

	private const string AutomaticIconPath = "res://addons/ModEditor/Icons/ClassIcon/PlayStart.svg";

	private const string DelayIconPath = "res://addons/ModEditor/Icons/ClassIcon/Time.svg";

	private const float ChipWidth = 142f;

	private const float ChipHeight = 28f;

	private readonly StyleBoxFlat _style = new StyleBoxFlat();

	private StateMachineTriggerKind _triggerKind;

	private StringName _eventName;

	private double _delaySeconds;

	private bool _hasGuard;

	private bool _isReadOnly;

	private bool _showPriority;

	private string _sourceDisplayName = string.Empty;

	private string _targetDisplayName = string.Empty;

	private string _baseText = string.Empty;

	private int _pendingTenths = -1;

	public string StableId { get; private set; } = string.Empty;

	public string SourceStateId { get; private set; } = string.Empty;

	public string TargetStateId { get; private set; } = string.Empty;

	public int Priority { get; private set; }

	public int DeclarationOrder { get; private set; }

	public bool IsPending { get; private set; }

	public bool IsRecentlyCompleted { get; private set; }

	public bool IsSelectedTransition { get; private set; }

	public double PendingDelayRemaining { get; private set; }

	public int VisualUpdateCount { get; private set; }

	public event Action<string> InspectRequested;

	public override void _Ready()
	{
		Name = (string.IsNullOrWhiteSpace(StableId) ? "TransitionChip" : ("TransitionChip_" + StableId));
		CustomMinimumSize = new Vector2(142f, 28f);
		Size = CustomMinimumSize;
		FocusMode = FocusModeEnum.None;
		MouseFilter = MouseFilterEnum.Stop;
		TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
		ExpandIcon = true;
		Alignment = HorizontalAlignment.Left;
		ZIndex = 12;
		ZAsRelative = false;
		AddThemeFontSizeOverride("font_size", 12);
		AddThemeConstantOverride("icon_max_width", 16);
		AddThemeConstantOverride("h_separation", 5);
		ConfigureStyleBox();
		Pressed += OnPressed;
		RefreshVisualState(force: true);
	}

	public override void _ExitTree()
	{
		Pressed -= OnPressed;
	}

	public override void _Draw()
	{
		if (IsPending)
		{
			float num = ((_delaySeconds <= 0.0001) ? 1f : Mathf.Clamp((float)(1.0 - PendingDelayRemaining / _delaySeconds), 0.04f, 1f));
			DrawRect(new Rect2(6f, Size.Y - 3f, Mathf.Max(4f, (Size.X - 12f) * num), 2f), new Color(1f, 0.68f, 0.16f, 0.95f));
		}
	}

	public bool Bind(StateMachineGraphTransitionViewModel item, string sourceDisplayName, string targetDisplayName, bool showPriority)
	{
		StateMachineTransitionDefinition stateMachineTransitionDefinition = item?.Transition;
		if (stateMachineTransitionDefinition == null || string.IsNullOrWhiteSpace(stateMachineTransitionDefinition.StableId))
		{
			return false;
		}
		string text = sourceDisplayName ?? stateMachineTransitionDefinition.SourceStateId ?? string.Empty;
		string text2 = targetDisplayName ?? stateMachineTransitionDefinition.TargetStateId ?? string.Empty;
		bool num = !string.Equals(StableId, stateMachineTransitionDefinition.StableId, StringComparison.Ordinal) || !string.Equals(SourceStateId, stateMachineTransitionDefinition.SourceStateId, StringComparison.Ordinal) || !string.Equals(TargetStateId, stateMachineTransitionDefinition.TargetStateId, StringComparison.Ordinal) || _triggerKind != stateMachineTransitionDefinition.TriggerKind || _eventName != stateMachineTransitionDefinition.EventName || !Mathf.IsEqualApprox(_delaySeconds, stateMachineTransitionDefinition.DelaySeconds) || Priority != stateMachineTransitionDefinition.Priority || DeclarationOrder != stateMachineTransitionDefinition.DeclarationOrder || _hasGuard != (stateMachineTransitionDefinition.GuardDefinition != null) || _isReadOnly != item.IsReadOnly || _showPriority != showPriority || !string.Equals(_sourceDisplayName, text, StringComparison.Ordinal) || !string.Equals(_targetDisplayName, text2, StringComparison.Ordinal);
		StableId = stateMachineTransitionDefinition.StableId;
		SourceStateId = stateMachineTransitionDefinition.SourceStateId ?? string.Empty;
		TargetStateId = stateMachineTransitionDefinition.TargetStateId ?? string.Empty;
		_triggerKind = stateMachineTransitionDefinition.TriggerKind;
		_eventName = stateMachineTransitionDefinition.EventName;
		_delaySeconds = Math.Max(0.0, stateMachineTransitionDefinition.DelaySeconds);
		Priority = stateMachineTransitionDefinition.Priority;
		DeclarationOrder = stateMachineTransitionDefinition.DeclarationOrder;
		_hasGuard = stateMachineTransitionDefinition.GuardDefinition != null;
		_isReadOnly = item.IsReadOnly;
		_showPriority = showPriority;
		_sourceDisplayName = text;
		_targetDisplayName = text2;
		if (!num)
		{
			return false;
		}
		Name = "TransitionChip_" + StableId;
		Icon = ResourceLoader.Load<Texture2D>(ResolveIconPath(_triggerKind), null, ResourceLoader.CacheMode.Reuse);
		_baseText = BuildBaseText();
		TooltipText = BuildTooltip();
		RefreshVisualState(force: true);
		return true;
	}

	public void SetSelectedTransition(bool selected)
	{
		if (IsSelectedTransition != selected)
		{
			IsSelectedTransition = selected;
			RefreshVisualState(force: false);
		}
	}

	public void SetRuntimeState(bool pending, double remainingSeconds, bool recentlyCompleted)
	{
		double num = Math.Max(0.0, remainingSeconds);
		int num2 = (pending ? Mathf.CeilToInt(num * 10.0) : (-1));
		if (IsPending != pending || IsRecentlyCompleted != recentlyCompleted || _pendingTenths != num2)
		{
			IsPending = pending;
			IsRecentlyCompleted = recentlyCompleted;
			PendingDelayRemaining = num;
			_pendingTenths = num2;
			RefreshVisualState(force: false);
			QueueRedraw();
		}
	}

	private void OnPressed()
	{
		if (!string.IsNullOrWhiteSpace(StableId))
		{
			InspectRequested?.Invoke(StableId);
		}
	}

	private void ConfigureStyleBox()
	{
		_style.CornerRadiusTopLeft = 8;
		_style.CornerRadiusTopRight = 8;
		_style.CornerRadiusBottomLeft = 8;
		_style.CornerRadiusBottomRight = 8;
		_style.BorderWidthLeft = 1;
		_style.BorderWidthTop = 1;
		_style.BorderWidthRight = 1;
		_style.BorderWidthBottom = 1;
		_style.ContentMarginLeft = 8f;
		_style.ContentMarginRight = 8f;
		_style.ContentMarginTop = 3f;
		_style.ContentMarginBottom = 3f;
		_style.ShadowColor = new Color(0f, 0f, 0f, 0.34f);
		_style.ShadowSize = 4;
		_style.ShadowOffset = new Vector2(0f, 2f);
		AddThemeStyleboxOverride("normal", _style);
		AddThemeStyleboxOverride("hover", _style);
		AddThemeStyleboxOverride("pressed", _style);
		AddThemeStyleboxOverride("focus", _style);
	}

	private void RefreshVisualState(bool force)
	{
		if (IsInsideTree() || force)
		{
			Color bgColor;
			Color borderColor;
			Color color;
			if (IsPending)
			{
				bgColor = new Color(0.28f, 0.2f, 0.07f, 0.98f);
				borderColor = new Color(1f, 0.67f, 0.12f);
				color = new Color(1f, 0.89f, 0.6f);
			}
			else if (IsRecentlyCompleted)
			{
				bgColor = new Color(0.07f, 0.26f, 0.17f, 0.98f);
				borderColor = new Color(0.3f, 0.96f, 0.57f);
				color = new Color(0.72f, 1f, 0.82f);
			}
			else if (IsSelectedTransition)
			{
				bgColor = new Color(0.11f, 0.23f, 0.38f, 0.98f);
				borderColor = new Color(0.36f, 0.72f, 1f);
				color = new Color(0.8f, 0.93f, 1f);
			}
			else
			{
				bgColor = new Color(0.085f, 0.105f, 0.15f, 0.96f);
				borderColor = ResolveTriggerColor(_triggerKind);
				color = new Color(0.89f, 0.92f, 0.98f);
			}
			_style.BgColor = bgColor;
			_style.BorderColor = borderColor;
			AddThemeColorOverride("font_color", color);
			AddThemeColorOverride("font_hover_color", color.Lightened(0.08f));
			AddThemeColorOverride("font_pressed_color", color);
			AddThemeColorOverride("icon_normal_color", borderColor.Lightened(0.12f));
			AddThemeColorOverride("icon_hover_color", borderColor.Lightened(0.22f));
			SelfModulate = (_isReadOnly ? new Color(0.78f, 0.81f, 0.88f, 0.92f) : Colors.White);
			string text;
			if (IsPending && _pendingTenths >= 0)
			{
				text = $" · {PendingDelayRemaining:0.0}s";
			}
			else
			{
				text = (IsRecentlyCompleted ? " · ✓" : string.Empty);
			}
			Text = _baseText + text;
			VisualUpdateCount++;
		}
	}

	private string BuildBaseText()
	{
		string text = _triggerKind switch
		{
			StateMachineTriggerKind.Automatic => "自动", 
			StateMachineTriggerKind.Delay => $"{_delaySeconds:0.###} 秒", 
			_ => _eventName.IsEmpty ? "事件" : _eventName.ToString(), 
		};
		string text2 = (_hasGuard ? " ◆" : string.Empty);
		string text3 = ((_showPriority || Priority != 0) ? $"  P{Priority}" : string.Empty);
		return text + text2 + text3;
	}

	private string BuildTooltip()
	{
		string value = _triggerKind switch
		{
			StateMachineTriggerKind.Automatic => "自动转换", 
			StateMachineTriggerKind.Delay => $"延迟 {_delaySeconds:0.###} 秒", 
			_ => _eventName.IsEmpty ? "未命名事件" : $"事件 {_eventName}", 
		};
		string text = (_hasGuard ? "\n守卫：已配置" : "\n守卫：无");
		string text2 = (_isReadOnly ? "\n来源：继承只读" : "\n来源：当前资源");
		return $"{_sourceDisplayName} → {_targetDisplayName}\n{value} · 优先级 {Priority}" + text + $"\n声明顺序：{DeclarationOrder}\nStableId：{StableId}" + text2 + "\n点击直接编辑此转换";
	}

	private static string ResolveIconPath(StateMachineTriggerKind triggerKind)
	{
		return triggerKind switch
		{
			StateMachineTriggerKind.Automatic => "res://addons/ModEditor/Icons/ClassIcon/PlayStart.svg", 
			StateMachineTriggerKind.Delay => "res://addons/ModEditor/Icons/ClassIcon/Time.svg", 
			_ => "res://addons/ModEditor/Icons/ClassIcon/KeyNext.svg", 
		};
	}

	private static Color ResolveTriggerColor(StateMachineTriggerKind triggerKind)
	{
		return triggerKind switch
		{
			StateMachineTriggerKind.Automatic => new Color(0.37f, 0.86f, 0.64f), 
			StateMachineTriggerKind.Delay => new Color(0.94f, 0.62f, 0.24f), 
			_ => new Color(0.45f, 0.67f, 1f), 
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetSelectedTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pending", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "remainingSeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "recentlyCompleted", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureStyleBox, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshVisualState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "force", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildBaseText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildTooltip, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveIconPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "triggerKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveTriggerColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "triggerKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.SetSelectedTransition && args.Count == 1)
		{
			SetSelectedTransition(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRuntimeState && args.Count == 3)
		{
			SetRuntimeState(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPressed && args.Count == 0)
		{
			OnPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureStyleBox && args.Count == 0)
		{
			ConfigureStyleBox();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshVisualState && args.Count == 1)
		{
			RefreshVisualState(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildBaseText && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildBaseText());
			return true;
		}
		if (method == MethodName.BuildTooltip && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildTooltip());
			return true;
		}
		if (method == MethodName.ResolveIconPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveIconPath(VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveTriggerColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(ResolveTriggerColor(VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveIconPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveIconPath(VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveTriggerColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(ResolveTriggerColor(VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[0])));
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
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.SetSelectedTransition)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeState)
		{
			return true;
		}
		if (method == MethodName.OnPressed)
		{
			return true;
		}
		if (method == MethodName.ConfigureStyleBox)
		{
			return true;
		}
		if (method == MethodName.RefreshVisualState)
		{
			return true;
		}
		if (method == MethodName.BuildBaseText)
		{
			return true;
		}
		if (method == MethodName.BuildTooltip)
		{
			return true;
		}
		if (method == MethodName.ResolveIconPath)
		{
			return true;
		}
		if (method == MethodName.ResolveTriggerColor)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.StableId)
		{
			StableId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.SourceStateId)
		{
			SourceStateId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.TargetStateId)
		{
			TargetStateId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Priority)
		{
			Priority = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.DeclarationOrder)
		{
			DeclarationOrder = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.IsPending)
		{
			IsPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsRecentlyCompleted)
		{
			IsRecentlyCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsSelectedTransition)
		{
			IsSelectedTransition = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.PendingDelayRemaining)
		{
			PendingDelayRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.VisualUpdateCount)
		{
			VisualUpdateCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._triggerKind)
		{
			_triggerKind = VariantUtils.ConvertTo<StateMachineTriggerKind>(in value);
			return true;
		}
		if (name == PropertyName._eventName)
		{
			_eventName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._delaySeconds)
		{
			_delaySeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._hasGuard)
		{
			_hasGuard = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isReadOnly)
		{
			_isReadOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._showPriority)
		{
			_showPriority = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sourceDisplayName)
		{
			_sourceDisplayName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._targetDisplayName)
		{
			_targetDisplayName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._baseText)
		{
			_baseText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pendingTenths)
		{
			_pendingTenths = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.StableId)
		{
			from = StableId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SourceStateId)
		{
			from = SourceStateId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TargetStateId)
		{
			from = TargetStateId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.Priority)
		{
			from2 = Priority;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DeclarationOrder)
		{
			from2 = DeclarationOrder;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		bool from3;
		if (name == PropertyName.IsPending)
		{
			from3 = IsPending;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.IsRecentlyCompleted)
		{
			from3 = IsRecentlyCompleted;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.IsSelectedTransition)
		{
			from3 = IsSelectedTransition;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.PendingDelayRemaining)
		{
			value = VariantUtils.CreateFrom<double>(PendingDelayRemaining);
			return true;
		}
		if (name == PropertyName.VisualUpdateCount)
		{
			from2 = VisualUpdateCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._style)
		{
			value = VariantUtils.CreateFrom(in _style);
			return true;
		}
		if (name == PropertyName._triggerKind)
		{
			value = VariantUtils.CreateFrom(in _triggerKind);
			return true;
		}
		if (name == PropertyName._eventName)
		{
			value = VariantUtils.CreateFrom(in _eventName);
			return true;
		}
		if (name == PropertyName._delaySeconds)
		{
			value = VariantUtils.CreateFrom(in _delaySeconds);
			return true;
		}
		if (name == PropertyName._hasGuard)
		{
			value = VariantUtils.CreateFrom(in _hasGuard);
			return true;
		}
		if (name == PropertyName._isReadOnly)
		{
			value = VariantUtils.CreateFrom(in _isReadOnly);
			return true;
		}
		if (name == PropertyName._showPriority)
		{
			value = VariantUtils.CreateFrom(in _showPriority);
			return true;
		}
		if (name == PropertyName._sourceDisplayName)
		{
			value = VariantUtils.CreateFrom(in _sourceDisplayName);
			return true;
		}
		if (name == PropertyName._targetDisplayName)
		{
			value = VariantUtils.CreateFrom(in _targetDisplayName);
			return true;
		}
		if (name == PropertyName._baseText)
		{
			value = VariantUtils.CreateFrom(in _baseText);
			return true;
		}
		if (name == PropertyName._pendingTenths)
		{
			value = VariantUtils.CreateFrom(in _pendingTenths);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.StableId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.SourceStateId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.TargetStateId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.Priority, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DeclarationOrder, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsRecentlyCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsSelectedTransition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.PendingDelayRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisualUpdateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._style, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._triggerKind, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName._eventName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._delaySeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasGuard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isReadOnly, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._showPriority, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._sourceDisplayName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._targetDisplayName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._baseText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pendingTenths, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.StableId, Variant.From<string>(StableId));
		info.AddProperty(PropertyName.SourceStateId, Variant.From<string>(SourceStateId));
		info.AddProperty(PropertyName.TargetStateId, Variant.From<string>(TargetStateId));
		info.AddProperty(PropertyName.Priority, Variant.From<int>(Priority));
		info.AddProperty(PropertyName.DeclarationOrder, Variant.From<int>(DeclarationOrder));
		info.AddProperty(PropertyName.IsPending, Variant.From<bool>(IsPending));
		info.AddProperty(PropertyName.IsRecentlyCompleted, Variant.From<bool>(IsRecentlyCompleted));
		info.AddProperty(PropertyName.IsSelectedTransition, Variant.From<bool>(IsSelectedTransition));
		info.AddProperty(PropertyName.PendingDelayRemaining, Variant.From<double>(PendingDelayRemaining));
		info.AddProperty(PropertyName.VisualUpdateCount, Variant.From<int>(VisualUpdateCount));
		info.AddProperty(PropertyName._triggerKind, Variant.From(in _triggerKind));
		info.AddProperty(PropertyName._eventName, Variant.From(in _eventName));
		info.AddProperty(PropertyName._delaySeconds, Variant.From(in _delaySeconds));
		info.AddProperty(PropertyName._hasGuard, Variant.From(in _hasGuard));
		info.AddProperty(PropertyName._isReadOnly, Variant.From(in _isReadOnly));
		info.AddProperty(PropertyName._showPriority, Variant.From(in _showPriority));
		info.AddProperty(PropertyName._sourceDisplayName, Variant.From(in _sourceDisplayName));
		info.AddProperty(PropertyName._targetDisplayName, Variant.From(in _targetDisplayName));
		info.AddProperty(PropertyName._baseText, Variant.From(in _baseText));
		info.AddProperty(PropertyName._pendingTenths, Variant.From(in _pendingTenths));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.StableId, out var value))
		{
			StableId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.SourceStateId, out var value2))
		{
			SourceStateId = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.TargetStateId, out var value3))
		{
			TargetStateId = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Priority, out var value4))
		{
			Priority = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.DeclarationOrder, out var value5))
		{
			DeclarationOrder = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.IsPending, out var value6))
		{
			IsPending = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsRecentlyCompleted, out var value7))
		{
			IsRecentlyCompleted = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsSelectedTransition, out var value8))
		{
			IsSelectedTransition = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.PendingDelayRemaining, out var value9))
		{
			PendingDelayRemaining = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.VisualUpdateCount, out var value10))
		{
			VisualUpdateCount = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._triggerKind, out var value11))
		{
			_triggerKind = value11.As<StateMachineTriggerKind>();
		}
		if (info.TryGetProperty(PropertyName._eventName, out var value12))
		{
			_eventName = value12.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._delaySeconds, out var value13))
		{
			_delaySeconds = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hasGuard, out var value14))
		{
			_hasGuard = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isReadOnly, out var value15))
		{
			_isReadOnly = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._showPriority, out var value16))
		{
			_showPriority = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sourceDisplayName, out var value17))
		{
			_sourceDisplayName = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName._targetDisplayName, out var value18))
		{
			_targetDisplayName = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName._baseText, out var value19))
		{
			_baseText = value19.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingTenths, out var value20))
		{
			_pendingTenths = value20.As<int>();
		}
	}
}
