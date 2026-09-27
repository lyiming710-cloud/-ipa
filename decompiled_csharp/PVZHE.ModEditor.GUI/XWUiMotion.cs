using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWUiMotion.cs")]
public class XWUiMotion : Node
{
	public enum MotionRole
	{
		NavigationButton,
		Card,
		Panel
	}

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName ConfigurePolicy = "ConfigurePolicy";

		public static readonly StringName SetReducedMotion = "SetReducedMotion";

		public static readonly StringName SetLowPerformanceMode = "SetLowPerformanceMode";

		public static readonly StringName BindButton = "BindButton";

		public static readonly StringName BindPanel = "BindPanel";

		public static readonly StringName FindBinding = "FindBinding";

		public static readonly StringName Initialize = "Initialize";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName SyncButtonState = "SyncButtonState";

		public static readonly StringName PlayPanelReveal = "PlayPanelReveal";

		public static readonly StringName SuspendForHiddenHost = "SuspendForHiddenHost";

		public static readonly StringName ResumeVisibleHost = "ResumeVisibleHost";

		public static readonly StringName OnVisibilityChanged = "OnVisibilityChanged";

		public static readonly StringName OnMotionPolicyChanged = "OnMotionPolicyChanged";

		public static readonly StringName UpdatePivot = "UpdatePivot";

		public static readonly StringName OnMouseEntered = "OnMouseEntered";

		public static readonly StringName OnMouseExited = "OnMouseExited";

		public static readonly StringName OnButtonDown = "OnButtonDown";

		public static readonly StringName OnButtonUp = "OnButtonUp";

		public static readonly StringName OnButtonToggled = "OnButtonToggled";

		public static readonly StringName ApplyButtonState = "ApplyButtonState";

		public static readonly StringName AnimateButtonTo = "AnimateButtonTo";

		public static readonly StringName CanAnimate = "CanAnimate";

		public static readonly StringName StopTween = "StopTween";

		public static readonly StringName OnTweenFinished = "OnTweenFinished";

		public static readonly StringName RestoreVisualState = "RestoreVisualState";

		public static readonly StringName RestorePanelState = "RestorePanelState";

		public static readonly StringName FindHostWindow = "FindHostWindow";

		public static readonly StringName WithAlpha = "WithAlpha";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName Role = "Role";

		public static readonly StringName IsAnimating = "IsAnimating";

		public static readonly StringName IsSuspended = "IsSuspended";

		public static readonly StringName AnimationStartCount = "AnimationStartCount";

		public static readonly StringName HiddenCancelCount = "HiddenCancelCount";

		public static readonly StringName ImmediateStateCount = "ImmediateStateCount";

		public static readonly StringName _target = "_target";

		public static readonly StringName _button = "_button";

		public static readonly StringName _hostWindow = "_hostWindow";

		public static readonly StringName _editorManager = "_editorManager";

		public static readonly StringName _role = "_role";

		public static readonly StringName _activeTween = "_activeTween";

		public static readonly StringName _restScale = "_restScale";

		public static readonly StringName _restPosition = "_restPosition";

		public static readonly StringName _restSelfModulate = "_restSelfModulate";

		public static readonly StringName _pointerInside = "_pointerInside";

		public static readonly StringName _initialized = "_initialized";

		public static readonly StringName _suspended = "_suspended";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string BindingNodeName = "__XWUiMotion";

	private static bool _reducedMotion;

	private static bool _lowPerformanceMode;

	private Control _target;

	private BaseButton _button;

	private Window _hostWindow;

	private ModEditorManager _editorManager;

	private MotionRole _role;

	private Tween _activeTween;

	private Vector2 _restScale = Vector2.One;

	private Vector2 _restPosition;

	private Color _restSelfModulate = Colors.White;

	private bool _pointerInside;

	private bool _initialized;

	private bool _suspended;

	public static bool ReducedMotion => _reducedMotion;

	public static bool LowPerformanceMode => _lowPerformanceMode;

	public static bool MotionAllowed
	{
		get
		{
			if (!_reducedMotion)
			{
				return !_lowPerformanceMode;
			}
			return false;
		}
	}

	public MotionRole Role => _role;

	public bool IsAnimating => GodotObject.IsInstanceValid(_activeTween);

	public bool IsSuspended => _suspended;

	public int AnimationStartCount { get; private set; }

	public int HiddenCancelCount { get; private set; }

	public int ImmediateStateCount { get; private set; }

	private static event Action MotionPolicyChanged;

	public static void ConfigurePolicy(bool reducedMotion, bool lowPerformanceMode)
	{
		if (_reducedMotion != reducedMotion || _lowPerformanceMode != lowPerformanceMode)
		{
			_reducedMotion = reducedMotion;
			_lowPerformanceMode = lowPerformanceMode;
			MotionPolicyChanged?.Invoke();
		}
	}

	public static void SetReducedMotion(bool reducedMotion)
	{
		ConfigurePolicy(reducedMotion, _lowPerformanceMode);
	}

	public static void SetLowPerformanceMode(bool lowPerformanceMode)
	{
		ConfigurePolicy(_reducedMotion, lowPerformanceMode);
	}

	public static XWUiMotion BindButton(BaseButton button, MotionRole role = MotionRole.NavigationButton)
	{
		if (!GodotObject.IsInstanceValid(button))
		{
			return null;
		}
		XWUiMotion xWUiMotion = FindBinding(button);
		if (xWUiMotion != null)
		{
			return xWUiMotion;
		}
		XWUiMotion xWUiMotion2 = new XWUiMotion
		{
			Name = "__XWUiMotion"
		};
		xWUiMotion2.Initialize(button, role);
		button.AddChild(xWUiMotion2, forceReadableName: false, InternalMode.Disabled);
		return xWUiMotion2;
	}

	public static XWUiMotion BindPanel(Control panel)
	{
		if (!GodotObject.IsInstanceValid(panel))
		{
			return null;
		}
		XWUiMotion xWUiMotion = FindBinding(panel);
		if (xWUiMotion != null)
		{
			return xWUiMotion;
		}
		XWUiMotion xWUiMotion2 = new XWUiMotion
		{
			Name = "__XWUiMotion"
		};
		xWUiMotion2.Initialize(panel, MotionRole.Panel);
		panel.AddChild(xWUiMotion2, forceReadableName: false, InternalMode.Disabled);
		return xWUiMotion2;
	}

	public static XWUiMotion FindBinding(Control target)
	{
		return target?.GetNodeOrNull<XWUiMotion>("__XWUiMotion");
	}

	private void Initialize(Control target, MotionRole role)
	{
		_target = target;
		_button = target as BaseButton;
		_role = role;
		_restScale = target.Scale;
		_restPosition = target.Position;
		_restSelfModulate = target.SelfModulate;
		_initialized = true;
	}

	public override void _Ready()
	{
		if (_initialized && GodotObject.IsInstanceValid(_target))
		{
			_target.PivotOffset = _target.Size * 0.5f;
			_target.Resized += UpdatePivot;
			_target.VisibilityChanged += OnVisibilityChanged;
			MotionPolicyChanged += OnMotionPolicyChanged;
			_hostWindow = FindHostWindow(_target);
			if (GodotObject.IsInstanceValid(_hostWindow))
			{
				_hostWindow.VisibilityChanged += OnVisibilityChanged;
			}
			_editorManager = ModEditorManager.Instance;
			if (GodotObject.IsInstanceValid(_editorManager))
			{
				_editorManager.EditorOpened += ResumeVisibleHost;
				_editorManager.EditorClosed += SuspendForHiddenHost;
			}
			if (GodotObject.IsInstanceValid(_button))
			{
				_button.MouseEntered += OnMouseEntered;
				_button.MouseExited += OnMouseExited;
				_button.ButtonDown += OnButtonDown;
				_button.ButtonUp += OnButtonUp;
				_button.Toggled += OnButtonToggled;
				ApplyButtonState(animate: false);
			}
			OnVisibilityChanged();
		}
	}

	public override void _ExitTree()
	{
		StopTween(restore: false);
		MotionPolicyChanged -= OnMotionPolicyChanged;
		if (GodotObject.IsInstanceValid(_target))
		{
			_target.Resized -= UpdatePivot;
			_target.VisibilityChanged -= OnVisibilityChanged;
		}
		if (GodotObject.IsInstanceValid(_hostWindow))
		{
			_hostWindow.VisibilityChanged -= OnVisibilityChanged;
		}
		if (GodotObject.IsInstanceValid(_editorManager))
		{
			_editorManager.EditorOpened -= ResumeVisibleHost;
			_editorManager.EditorClosed -= SuspendForHiddenHost;
		}
		if (GodotObject.IsInstanceValid(_button))
		{
			_button.MouseEntered -= OnMouseEntered;
			_button.MouseExited -= OnMouseExited;
			_button.ButtonDown -= OnButtonDown;
			_button.ButtonUp -= OnButtonUp;
			_button.Toggled -= OnButtonToggled;
		}
		base._ExitTree();
	}

	public void SyncButtonState(bool animate = true)
	{
		if (GodotObject.IsInstanceValid(_button))
		{
			ApplyButtonState(animate);
		}
	}

	public void PlayPanelReveal(float slidePixels = 10f)
	{
		if (_role != MotionRole.Panel || !CanAnimate())
		{
			RestorePanelState();
			ImmediateStateCount++;
			return;
		}
		bool flag = GodotObject.IsInstanceValid(_activeTween);
		StopTween(restore: false);
		if (flag)
		{
			RestorePanelState();
		}
		else
		{
			_restPosition = _target.Position;
		}
		_target.Position = _restPosition + new Vector2(0f, Mathf.Max(0f, slidePixels));
		_target.SelfModulate = WithAlpha(_restSelfModulate, 0.28f);
		Tween tween = (_activeTween = _target.CreateTween());
		AnimationStartCount++;
		tween.SetParallel();
		tween.TweenProperty(_target, "position", _restPosition, 0.16).SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.Out);
		tween.TweenProperty(_target, "self_modulate", _restSelfModulate, 0.14).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		tween.Finished += OnTweenFinished;
	}

	public void SuspendForHiddenHost()
	{
		if (!_suspended)
		{
			_suspended = true;
			if (GodotObject.IsInstanceValid(_activeTween))
			{
				HiddenCancelCount++;
				StopTween(restore: false);
			}
			RestoreVisualState();
		}
	}

	private void ResumeVisibleHost()
	{
		_suspended = false;
		RestoreVisualState();
	}

	private void OnVisibilityChanged()
	{
		if (GodotObject.IsInstanceValid(_target) && _target.IsVisibleInTree() && (!GodotObject.IsInstanceValid(_hostWindow) || _hostWindow.Visible))
		{
			ResumeVisibleHost();
		}
		else
		{
			SuspendForHiddenHost();
		}
	}

	private void OnMotionPolicyChanged()
	{
		StopTween(restore: false);
		RestoreVisualState();
		if (GodotObject.IsInstanceValid(_button))
		{
			ApplyButtonState(animate: false);
		}
	}

	private void UpdatePivot()
	{
		if (GodotObject.IsInstanceValid(_target))
		{
			_target.PivotOffset = _target.Size * 0.5f;
		}
	}

	private void OnMouseEntered()
	{
		_pointerInside = true;
		ApplyButtonState(animate: true);
	}

	private void OnMouseExited()
	{
		_pointerInside = false;
		ApplyButtonState(animate: true);
	}

	private void OnButtonDown()
	{
		if (GodotObject.IsInstanceValid(_button) && !_button.Disabled)
		{
			AnimateButtonTo(_restScale * 0.965f, new Color(1f, 0.91f, 0.7f, _restSelfModulate.A), 0.07);
		}
	}

	private void OnButtonUp()
	{
		ApplyButtonState(animate: true, rebound: true);
	}

	private void OnButtonToggled(bool pressed)
	{
		ApplyButtonState(animate: true, pressed);
	}

	private void ApplyButtonState(bool animate, bool rebound = false)
	{
		if (!GodotObject.IsInstanceValid(_button))
		{
			return;
		}
		int num;
		float num2;
		if (_button.ToggleMode)
		{
			num = (_button.ButtonPressed ? 1 : 0);
			if (num != 0)
			{
				num2 = 1.026f;
				goto IL_0057;
			}
		}
		else
		{
			num = 0;
		}
		if (_pointerInside)
		{
			num2 = ((_role == MotionRole.Card) ? 1.026f : 1.018f);
		}
		else
		{
			num2 = 1f;
		}
		goto IL_0057;
		IL_0057:
		float num3 = num2;
		Color color;
		if (num != 0)
		{
			color = new Color(1f, 0.94f, 0.72f, _restSelfModulate.A);
		}
		else
		{
			color = (_pointerInside ? new Color(1f, 1f, 0.92f, _restSelfModulate.A) : _restSelfModulate);
		}
		double duration = (rebound ? 0.15 : 0.11);
		if (!animate || !CanAnimate())
		{
			StopTween(restore: false);
			_target.Scale = _restScale;
			_target.SelfModulate = color;
			ImmediateStateCount++;
		}
		else
		{
			AnimateButtonTo(_restScale * num3, color, duration, rebound);
		}
	}

	private void AnimateButtonTo(Vector2 scale, Color tint, double duration, bool rebound = false)
	{
		if (!CanAnimate())
		{
			_target.Scale = _restScale;
			_target.SelfModulate = tint;
			ImmediateStateCount++;
			return;
		}
		StopTween(restore: false);
		Tween tween = (_activeTween = _target.CreateTween());
		AnimationStartCount++;
		tween.SetParallel();
		tween.TweenProperty(_target, "scale", scale, duration).SetTrans((Tween.TransitionType)(rebound ? 10 : 4)).SetEase(Tween.EaseType.Out);
		tween.TweenProperty(_target, "self_modulate", tint, duration).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
		tween.Finished += OnTweenFinished;
	}

	private bool CanAnimate()
	{
		if (MotionAllowed && !_suspended && GodotObject.IsInstanceValid(_target) && _target.IsVisibleInTree())
		{
			if (GodotObject.IsInstanceValid(_hostWindow))
			{
				return _hostWindow.Visible;
			}
			return true;
		}
		return false;
	}

	private void StopTween(bool restore)
	{
		if (GodotObject.IsInstanceValid(_activeTween))
		{
			_activeTween.Kill();
		}
		_activeTween = null;
		if (restore)
		{
			RestoreVisualState();
		}
	}

	private void OnTweenFinished()
	{
		_activeTween = null;
	}

	private void RestoreVisualState()
	{
		if (GodotObject.IsInstanceValid(_target))
		{
			if (_role == MotionRole.Panel)
			{
				RestorePanelState();
				return;
			}
			_target.Scale = _restScale;
			_target.SelfModulate = _restSelfModulate;
		}
	}

	private void RestorePanelState()
	{
		if (GodotObject.IsInstanceValid(_target))
		{
			_target.Position = _restPosition;
			_target.Scale = _restScale;
			_target.SelfModulate = _restSelfModulate;
		}
	}

	private static Window FindHostWindow(Node node)
	{
		Node node2 = node;
		while (GodotObject.IsInstanceValid(node2))
		{
			if (node2 is Window result)
			{
				return result;
			}
			node2 = node2.GetParent();
		}
		return null;
	}

	private static Color WithAlpha(Color color, float alphaMultiplier)
	{
		return new Color(color.R, color.G, color.B, color.A * alphaMultiplier);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(30)
		{
			new MethodInfo(MethodName.ConfigurePolicy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "reducedMotion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "lowPerformanceMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetReducedMotion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "reducedMotion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetLowPerformanceMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "lowPerformanceMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("BaseButton"), exported: false),
				new PropertyInfo(Variant.Type.Int, "role", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "panel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindBinding, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.Initialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Int, "role", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncButtonState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "animate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayPanelReveal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "slidePixels", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SuspendForHiddenHost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResumeVisibleHost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMotionPolicyChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePivot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMouseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMouseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnButtonDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnButtonUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnButtonToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyButtonState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "animate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "rebound", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimateButtonTo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "scale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "tint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "rebound", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanAnimate, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "restore", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTweenFinished, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreVisualState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestorePanelState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindHostWindow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Window"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.WithAlpha, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "alphaMultiplier", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConfigurePolicy && args.Count == 2)
		{
			ConfigurePolicy(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetReducedMotion && args.Count == 1)
		{
			SetReducedMotion(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLowPerformanceMode && args.Count == 1)
		{
			SetLowPerformanceMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindButton && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWUiMotion>(BindButton(VariantUtils.ConvertTo<BaseButton>(in args[0]), VariantUtils.ConvertTo<MotionRole>(in args[1])));
			return true;
		}
		if (method == MethodName.BindPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWUiMotion>(BindPanel(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.FindBinding && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWUiMotion>(FindBinding(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.Initialize && args.Count == 2)
		{
			Initialize(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<MotionRole>(in args[1]));
			ret = default;
			return true;
		}
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
		if (method == MethodName.SyncButtonState && args.Count == 1)
		{
			SyncButtonState(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayPanelReveal && args.Count == 1)
		{
			PlayPanelReveal(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SuspendForHiddenHost && args.Count == 0)
		{
			SuspendForHiddenHost();
			ret = default;
			return true;
		}
		if (method == MethodName.ResumeVisibleHost && args.Count == 0)
		{
			ResumeVisibleHost();
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisibilityChanged && args.Count == 0)
		{
			OnVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMotionPolicyChanged && args.Count == 0)
		{
			OnMotionPolicyChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePivot && args.Count == 0)
		{
			UpdatePivot();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMouseEntered && args.Count == 0)
		{
			OnMouseEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMouseExited && args.Count == 0)
		{
			OnMouseExited();
			ret = default;
			return true;
		}
		if (method == MethodName.OnButtonDown && args.Count == 0)
		{
			OnButtonDown();
			ret = default;
			return true;
		}
		if (method == MethodName.OnButtonUp && args.Count == 0)
		{
			OnButtonUp();
			ret = default;
			return true;
		}
		if (method == MethodName.OnButtonToggled && args.Count == 1)
		{
			OnButtonToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyButtonState && args.Count == 2)
		{
			ApplyButtonState(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimateButtonTo && args.Count == 4)
		{
			AnimateButtonTo(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanAnimate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanAnimate());
			return true;
		}
		if (method == MethodName.StopTween && args.Count == 1)
		{
			StopTween(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTweenFinished && args.Count == 0)
		{
			OnTweenFinished();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreVisualState && args.Count == 0)
		{
			RestoreVisualState();
			ret = default;
			return true;
		}
		if (method == MethodName.RestorePanelState && args.Count == 0)
		{
			RestorePanelState();
			ret = default;
			return true;
		}
		if (method == MethodName.FindHostWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindHostWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.WithAlpha && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Color>(WithAlpha(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConfigurePolicy && args.Count == 2)
		{
			ConfigurePolicy(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetReducedMotion && args.Count == 1)
		{
			SetReducedMotion(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLowPerformanceMode && args.Count == 1)
		{
			SetLowPerformanceMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindButton && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWUiMotion>(BindButton(VariantUtils.ConvertTo<BaseButton>(in args[0]), VariantUtils.ConvertTo<MotionRole>(in args[1])));
			return true;
		}
		if (method == MethodName.BindPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWUiMotion>(BindPanel(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.FindBinding && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWUiMotion>(FindBinding(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.FindHostWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindHostWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.WithAlpha && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Color>(WithAlpha(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ConfigurePolicy)
		{
			return true;
		}
		if (method == MethodName.SetReducedMotion)
		{
			return true;
		}
		if (method == MethodName.SetLowPerformanceMode)
		{
			return true;
		}
		if (method == MethodName.BindButton)
		{
			return true;
		}
		if (method == MethodName.BindPanel)
		{
			return true;
		}
		if (method == MethodName.FindBinding)
		{
			return true;
		}
		if (method == MethodName.Initialize)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.SyncButtonState)
		{
			return true;
		}
		if (method == MethodName.PlayPanelReveal)
		{
			return true;
		}
		if (method == MethodName.SuspendForHiddenHost)
		{
			return true;
		}
		if (method == MethodName.ResumeVisibleHost)
		{
			return true;
		}
		if (method == MethodName.OnVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.OnMotionPolicyChanged)
		{
			return true;
		}
		if (method == MethodName.UpdatePivot)
		{
			return true;
		}
		if (method == MethodName.OnMouseEntered)
		{
			return true;
		}
		if (method == MethodName.OnMouseExited)
		{
			return true;
		}
		if (method == MethodName.OnButtonDown)
		{
			return true;
		}
		if (method == MethodName.OnButtonUp)
		{
			return true;
		}
		if (method == MethodName.OnButtonToggled)
		{
			return true;
		}
		if (method == MethodName.ApplyButtonState)
		{
			return true;
		}
		if (method == MethodName.AnimateButtonTo)
		{
			return true;
		}
		if (method == MethodName.CanAnimate)
		{
			return true;
		}
		if (method == MethodName.StopTween)
		{
			return true;
		}
		if (method == MethodName.OnTweenFinished)
		{
			return true;
		}
		if (method == MethodName.RestoreVisualState)
		{
			return true;
		}
		if (method == MethodName.RestorePanelState)
		{
			return true;
		}
		if (method == MethodName.FindHostWindow)
		{
			return true;
		}
		if (method == MethodName.WithAlpha)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.AnimationStartCount)
		{
			AnimationStartCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.HiddenCancelCount)
		{
			HiddenCancelCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ImmediateStateCount)
		{
			ImmediateStateCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._target)
		{
			_target = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._button)
		{
			_button = VariantUtils.ConvertTo<BaseButton>(in value);
			return true;
		}
		if (name == PropertyName._hostWindow)
		{
			_hostWindow = VariantUtils.ConvertTo<Window>(in value);
			return true;
		}
		if (name == PropertyName._editorManager)
		{
			_editorManager = VariantUtils.ConvertTo<ModEditorManager>(in value);
			return true;
		}
		if (name == PropertyName._role)
		{
			_role = VariantUtils.ConvertTo<MotionRole>(in value);
			return true;
		}
		if (name == PropertyName._activeTween)
		{
			_activeTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._restScale)
		{
			_restScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._restPosition)
		{
			_restPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._restSelfModulate)
		{
			_restSelfModulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._pointerInside)
		{
			_pointerInside = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._initialized)
		{
			_initialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._suspended)
		{
			_suspended = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Role)
		{
			value = VariantUtils.CreateFrom<MotionRole>(Role);
			return true;
		}
		bool from;
		if (name == PropertyName.IsAnimating)
		{
			from = IsAnimating;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsSuspended)
		{
			from = IsSuspended;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.AnimationStartCount)
		{
			from2 = AnimationStartCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.HiddenCancelCount)
		{
			from2 = HiddenCancelCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ImmediateStateCount)
		{
			from2 = ImmediateStateCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._target)
		{
			value = VariantUtils.CreateFrom(in _target);
			return true;
		}
		if (name == PropertyName._button)
		{
			value = VariantUtils.CreateFrom(in _button);
			return true;
		}
		if (name == PropertyName._hostWindow)
		{
			value = VariantUtils.CreateFrom(in _hostWindow);
			return true;
		}
		if (name == PropertyName._editorManager)
		{
			value = VariantUtils.CreateFrom(in _editorManager);
			return true;
		}
		if (name == PropertyName._role)
		{
			value = VariantUtils.CreateFrom(in _role);
			return true;
		}
		if (name == PropertyName._activeTween)
		{
			value = VariantUtils.CreateFrom(in _activeTween);
			return true;
		}
		if (name == PropertyName._restScale)
		{
			value = VariantUtils.CreateFrom(in _restScale);
			return true;
		}
		if (name == PropertyName._restPosition)
		{
			value = VariantUtils.CreateFrom(in _restPosition);
			return true;
		}
		if (name == PropertyName._restSelfModulate)
		{
			value = VariantUtils.CreateFrom(in _restSelfModulate);
			return true;
		}
		if (name == PropertyName._pointerInside)
		{
			value = VariantUtils.CreateFrom(in _pointerInside);
			return true;
		}
		if (name == PropertyName._initialized)
		{
			value = VariantUtils.CreateFrom(in _initialized);
			return true;
		}
		if (name == PropertyName._suspended)
		{
			value = VariantUtils.CreateFrom(in _suspended);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._target, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._button, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hostWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._role, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._activeTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._restScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._restPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._restSelfModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pointerInside, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._initialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._suspended, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.Role, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsAnimating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsSuspended, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AnimationStartCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.HiddenCancelCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ImmediateStateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.AnimationStartCount, Variant.From<int>(AnimationStartCount));
		info.AddProperty(PropertyName.HiddenCancelCount, Variant.From<int>(HiddenCancelCount));
		info.AddProperty(PropertyName.ImmediateStateCount, Variant.From<int>(ImmediateStateCount));
		info.AddProperty(PropertyName._target, Variant.From(in _target));
		info.AddProperty(PropertyName._button, Variant.From(in _button));
		info.AddProperty(PropertyName._hostWindow, Variant.From(in _hostWindow));
		info.AddProperty(PropertyName._editorManager, Variant.From(in _editorManager));
		info.AddProperty(PropertyName._role, Variant.From(in _role));
		info.AddProperty(PropertyName._activeTween, Variant.From(in _activeTween));
		info.AddProperty(PropertyName._restScale, Variant.From(in _restScale));
		info.AddProperty(PropertyName._restPosition, Variant.From(in _restPosition));
		info.AddProperty(PropertyName._restSelfModulate, Variant.From(in _restSelfModulate));
		info.AddProperty(PropertyName._pointerInside, Variant.From(in _pointerInside));
		info.AddProperty(PropertyName._initialized, Variant.From(in _initialized));
		info.AddProperty(PropertyName._suspended, Variant.From(in _suspended));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.AnimationStartCount, out var value))
		{
			AnimationStartCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.HiddenCancelCount, out var value2))
		{
			HiddenCancelCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ImmediateStateCount, out var value3))
		{
			ImmediateStateCount = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._target, out var value4))
		{
			_target = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._button, out var value5))
		{
			_button = value5.As<BaseButton>();
		}
		if (info.TryGetProperty(PropertyName._hostWindow, out var value6))
		{
			_hostWindow = value6.As<Window>();
		}
		if (info.TryGetProperty(PropertyName._editorManager, out var value7))
		{
			_editorManager = value7.As<ModEditorManager>();
		}
		if (info.TryGetProperty(PropertyName._role, out var value8))
		{
			_role = value8.As<MotionRole>();
		}
		if (info.TryGetProperty(PropertyName._activeTween, out var value9))
		{
			_activeTween = value9.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._restScale, out var value10))
		{
			_restScale = value10.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._restPosition, out var value11))
		{
			_restPosition = value11.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._restSelfModulate, out var value12))
		{
			_restSelfModulate = value12.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._pointerInside, out var value13))
		{
			_pointerInside = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._initialized, out var value14))
		{
			_initialized = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._suspended, out var value15))
		{
			_suspended = value15.As<bool>();
		}
	}
}
