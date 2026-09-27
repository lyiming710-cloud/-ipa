using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketSpawnEntryRangeControl.cs")]
public class XWPacketSpawnEntryRangeControl : Control
{
	[Signal]
	public delegate void DragStartedEventHandler(bool minimumHandle);

	[Signal]
	public delegate void ValuesPreviewedEventHandler(double minimum, double maximum);

	[Signal]
	public delegate void ValuesCommittedEventHandler(double minimum, double maximum);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName Configure = "Configure";

		public static readonly StringName SetTitle = "SetTitle";

		public static readonly StringName SetValues = "SetValues";

		public static readonly StringName OnRangeTrackGuiInput = "OnRangeTrackGuiInput";

		public static readonly StringName PreviewPointer = "PreviewPointer";

		public static readonly StringName IsMinimumHandleCloser = "IsMinimumHandleCloser";

		public static readonly StringName OnUnlimitedMinimumToggled = "OnUnlimitedMinimumToggled";

		public static readonly StringName OnUnlimitedMaximumToggled = "OnUnlimitedMaximumToggled";

		public static readonly StringName UpdateVisuals = "UpdateVisuals";

		public static readonly StringName PositionHandle = "PositionHandle";

		public static readonly StringName NormalizeEndpoint = "NormalizeEndpoint";

		public static readonly StringName Quantize = "Quantize";

		public static readonly StringName GetNormalizedValue = "GetNormalizedValue";

		public static readonly StringName IsUnlimited = "IsUnlimited";

		public static readonly StringName FormatValue = "FormatValue";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _title = "_title";

		public static readonly StringName _rangeTrack = "_rangeTrack";

		public static readonly StringName _minimumHandle = "_minimumHandle";

		public static readonly StringName _maximumHandle = "_maximumHandle";

		public static readonly StringName _minimumValue = "_minimumValue";

		public static readonly StringName _maximumValue = "_maximumValue";

		public static readonly StringName _unlimitedControls = "_unlimitedControls";

		public static readonly StringName _unlimitedMinimum = "_unlimitedMinimum";

		public static readonly StringName _unlimitedMaximum = "_unlimitedMaximum";

		public static readonly StringName _minimumLimit = "_minimumLimit";

		public static readonly StringName _maximumLimit = "_maximumLimit";

		public static readonly StringName _finiteMinimumLimit = "_finiteMinimumLimit";

		public static readonly StringName _step = "_step";

		public static readonly StringName _minimum = "_minimum";

		public static readonly StringName _maximum = "_maximum";

		public static readonly StringName _lastFiniteMinimum = "_lastFiniteMinimum";

		public static readonly StringName _lastFiniteMaximum = "_lastFiniteMaximum";

		public static readonly StringName _allowUnlimited = "_allowUnlimited";

		public static readonly StringName _dragging = "_dragging";

		public static readonly StringName _draggingMinimum = "_draggingMinimum";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : Control.SignalName
	{
		public static readonly StringName DragStarted = "DragStarted";

		public static readonly StringName ValuesPreviewed = "ValuesPreviewed";

		public static readonly StringName ValuesCommitted = "ValuesCommitted";
	}

	private Label _title;

	private Control _rangeTrack;

	private Control _minimumHandle;

	private Control _maximumHandle;

	private Label _minimumValue;

	private Label _maximumValue;

	private HBoxContainer _unlimitedControls;

	private CheckButton _unlimitedMinimum;

	private CheckButton _unlimitedMaximum;

	private double _minimumLimit;

	private double _maximumLimit = 1.0;

	private double _finiteMinimumLimit;

	private double _step = 1.0;

	private double _minimum;

	private double _maximum = 1.0;

	private double _lastFiniteMinimum;

	private double _lastFiniteMaximum = 1.0;

	private bool _allowUnlimited;

	private bool _dragging;

	private bool _draggingMinimum;

	private bool _updatingControls;

	private DragStartedEventHandler backing_DragStarted;

	private ValuesPreviewedEventHandler backing_ValuesPreviewed;

	private ValuesCommittedEventHandler backing_ValuesCommitted;

	public event DragStartedEventHandler DragStarted
	{
		add
		{
			backing_DragStarted = (DragStartedEventHandler)Delegate.Combine(backing_DragStarted, value);
		}
		remove
		{
			backing_DragStarted = (DragStartedEventHandler)Delegate.Remove(backing_DragStarted, value);
		}
	}

	public event ValuesPreviewedEventHandler ValuesPreviewed
	{
		add
		{
			backing_ValuesPreviewed = (ValuesPreviewedEventHandler)Delegate.Combine(backing_ValuesPreviewed, value);
		}
		remove
		{
			backing_ValuesPreviewed = (ValuesPreviewedEventHandler)Delegate.Remove(backing_ValuesPreviewed, value);
		}
	}

	public event ValuesCommittedEventHandler ValuesCommitted
	{
		add
		{
			backing_ValuesCommitted = (ValuesCommittedEventHandler)Delegate.Combine(backing_ValuesCommitted, value);
		}
		remove
		{
			backing_ValuesCommitted = (ValuesCommittedEventHandler)Delegate.Remove(backing_ValuesCommitted, value);
		}
	}

	public override void _Ready()
	{
		_title = GetNode<Label>("%TitleLabel");
		_rangeTrack = GetNode<Control>("%RangeTrack");
		_minimumHandle = GetNode<Control>("%MinimumHandle");
		_maximumHandle = GetNode<Control>("%MaximumHandle");
		_minimumValue = GetNode<Label>("%MinimumValue");
		_maximumValue = GetNode<Label>("%MaximumValue");
		_unlimitedControls = GetNode<HBoxContainer>("%UnlimitedControls");
		_unlimitedMinimum = GetNode<CheckButton>("%UnlimitedMinimum");
		_unlimitedMaximum = GetNode<CheckButton>("%UnlimitedMaximum");
		_rangeTrack.GuiInput += OnRangeTrackGuiInput;
		_rangeTrack.Resized += UpdateVisuals;
		_unlimitedMinimum.Toggled += OnUnlimitedMinimumToggled;
		_unlimitedMaximum.Toggled += OnUnlimitedMaximumToggled;
		UpdateVisuals();
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_rangeTrack))
		{
			_rangeTrack.GuiInput -= OnRangeTrackGuiInput;
			_rangeTrack.Resized -= UpdateVisuals;
		}
		if (GodotObject.IsInstanceValid(_unlimitedMinimum))
		{
			_unlimitedMinimum.Toggled -= OnUnlimitedMinimumToggled;
		}
		if (GodotObject.IsInstanceValid(_unlimitedMaximum))
		{
			_unlimitedMaximum.Toggled -= OnUnlimitedMaximumToggled;
		}
		base._ExitTree();
	}

	public override void _Draw()
	{
		if (GodotObject.IsInstanceValid(_rangeTrack))
		{
			Rect2 globalRect = _rangeTrack.GetGlobalRect();
			Vector2 vector = GetGlobalTransformWithCanvas().AffineInverse() * globalRect.Position;
			float y = vector.Y + globalRect.Size.Y * 0.5f;
			float num = vector.X + 8f;
			float num2 = vector.X + Math.Max(8f, globalRect.Size.X - 8f);
			float x = num + (num2 - num) * (float)GetNormalizedValue(_minimum, minimumEndpoint: true);
			float x2 = num + (num2 - num) * (float)GetNormalizedValue(_maximum, minimumEndpoint: false);
			DrawLine(new Vector2(num, y), new Vector2(num2, y), new Color(0.17f, 0.22f, 0.12f), 7f, antialiased: true);
			DrawLine(new Vector2(x, y), new Vector2(x2, y), new Color(0.66f, 0.86f, 0.26f), 7f, antialiased: true);
		}
	}

	public void Configure(double minimumLimit, double maximumLimit, double step, bool allowUnlimited)
	{
		_minimumLimit = minimumLimit;
		_maximumLimit = Math.Max(minimumLimit, maximumLimit);
		_step = Math.Max(0.0001, step);
		_allowUnlimited = allowUnlimited;
		_finiteMinimumLimit = (allowUnlimited ? Math.Max(0.0, minimumLimit) : minimumLimit);
		if (GodotObject.IsInstanceValid(_unlimitedControls))
		{
			_unlimitedControls.Visible = allowUnlimited;
		}
		SetValues(_minimum, _maximum);
	}

	public void SetTitle(string title)
	{
		if (GodotObject.IsInstanceValid(_title))
		{
			_title.Text = title ?? "区间";
		}
	}

	public void SetValues(double minimum, double maximum)
	{
		_minimum = NormalizeEndpoint(minimum);
		_maximum = NormalizeEndpoint(maximum);
		ConstrainFiniteEndpoints(ref _minimum, ref _maximum, movedMinimum: true);
		if (!IsUnlimited(_minimum))
		{
			_lastFiniteMinimum = _minimum;
		}
		if (!IsUnlimited(_maximum))
		{
			_lastFiniteMaximum = _maximum;
		}
		UpdateVisuals();
	}

	private void OnRangeTrackGuiInput(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			if (inputEventMouseButton.Pressed)
			{
				_draggingMinimum = IsMinimumHandleCloser(inputEventMouseButton.Position.X);
				_dragging = true;
				EmitSignal(SignalName.DragStarted, _draggingMinimum);
				PreviewPointer(inputEventMouseButton.Position.X);
				_rangeTrack.AcceptEvent();
			}
			else if (_dragging)
			{
				PreviewPointer(inputEventMouseButton.Position.X);
				_dragging = false;
				EmitSignal(SignalName.ValuesCommitted, _minimum, _maximum);
				_rangeTrack.AcceptEvent();
			}
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _dragging)
		{
			PreviewPointer(inputEventMouseMotion.Position.X);
			_rangeTrack.AcceptEvent();
		}
	}

	private void PreviewPointer(float pointerX)
	{
		float num = Math.Max(1f, _rangeTrack.Size.X - 16f);
		double num2 = Math.Clamp((pointerX - 8f) / num, 0f, 1f);
		double num3 = Quantize(_finiteMinimumLimit + num2 * (_maximumLimit - _finiteMinimumLimit));
		if (_draggingMinimum)
		{
			_minimum = num3;
			_lastFiniteMinimum = num3;
		}
		else
		{
			_maximum = num3;
			_lastFiniteMaximum = num3;
		}
		ConstrainFiniteEndpoints(ref _minimum, ref _maximum, _draggingMinimum);
		UpdateVisuals();
		EmitSignal(SignalName.ValuesPreviewed, _minimum, _maximum);
	}

	private bool IsMinimumHandleCloser(float pointerX)
	{
		float num = Math.Max(1f, _rangeTrack.Size.X - 16f);
		float num2 = 8f + num * (float)GetNormalizedValue(_minimum, minimumEndpoint: true);
		float num3 = 8f + num * (float)GetNormalizedValue(_maximum, minimumEndpoint: false);
		return Math.Abs(pointerX - num2) <= Math.Abs(pointerX - num3);
	}

	private void OnUnlimitedMinimumToggled(bool enabled)
	{
		if (!_updatingControls && _allowUnlimited)
		{
			EmitSignal(SignalName.DragStarted, true);
			_minimum = (enabled ? (-1.0) : NormalizeEndpoint(_lastFiniteMinimum));
			ConstrainFiniteEndpoints(ref _minimum, ref _maximum, movedMinimum: true);
			UpdateVisuals();
			EmitSignal(SignalName.ValuesPreviewed, _minimum, _maximum);
			EmitSignal(SignalName.ValuesCommitted, _minimum, _maximum);
		}
	}

	private void OnUnlimitedMaximumToggled(bool enabled)
	{
		if (!_updatingControls && _allowUnlimited)
		{
			EmitSignal(SignalName.DragStarted, false);
			_maximum = (enabled ? (-1.0) : NormalizeEndpoint(_lastFiniteMaximum));
			ConstrainFiniteEndpoints(ref _minimum, ref _maximum, movedMinimum: false);
			UpdateVisuals();
			EmitSignal(SignalName.ValuesPreviewed, _minimum, _maximum);
			EmitSignal(SignalName.ValuesCommitted, _minimum, _maximum);
		}
	}

	private void UpdateVisuals()
	{
		if (GodotObject.IsInstanceValid(_rangeTrack))
		{
			_updatingControls = true;
			if (GodotObject.IsInstanceValid(_unlimitedMinimum))
			{
				_unlimitedMinimum.ButtonPressed = IsUnlimited(_minimum);
			}
			if (GodotObject.IsInstanceValid(_unlimitedMaximum))
			{
				_unlimitedMaximum.ButtonPressed = IsUnlimited(_maximum);
			}
			_updatingControls = false;
			if (GodotObject.IsInstanceValid(_minimumValue))
			{
				_minimumValue.Text = FormatValue(_minimum);
			}
			if (GodotObject.IsInstanceValid(_maximumValue))
			{
				_maximumValue.Text = FormatValue(_maximum);
			}
			float num = Math.Max(1f, _rangeTrack.Size.X - 16f);
			PositionHandle(_minimumHandle, 8f + num * (float)GetNormalizedValue(_minimum, minimumEndpoint: true));
			PositionHandle(_maximumHandle, 8f + num * (float)GetNormalizedValue(_maximum, minimumEndpoint: false));
			QueueRedraw();
		}
	}

	private void PositionHandle(Control handle, float centerX)
	{
		if (GodotObject.IsInstanceValid(handle))
		{
			handle.Position = new Vector2(centerX - handle.Size.X * 0.5f, (_rangeTrack.Size.Y - handle.Size.Y) * 0.5f);
		}
	}

	private double NormalizeEndpoint(double value)
	{
		if (IsUnlimited(value))
		{
			return -1.0;
		}
		return Quantize(Math.Clamp(value, _finiteMinimumLimit, _maximumLimit));
	}

	private double Quantize(double value)
	{
		double num = Math.Round((value - _finiteMinimumLimit) / _step);
		return Math.Clamp(_finiteMinimumLimit + num * _step, _finiteMinimumLimit, _maximumLimit);
	}

	private void ConstrainFiniteEndpoints(ref double minimum, ref double maximum, bool movedMinimum)
	{
		if (!IsUnlimited(minimum) && !IsUnlimited(maximum) && !(minimum <= maximum))
		{
			if (movedMinimum)
			{
				minimum = maximum;
			}
			else
			{
				maximum = minimum;
			}
		}
	}

	private double GetNormalizedValue(double value, bool minimumEndpoint)
	{
		if (IsUnlimited(value))
		{
			return (!minimumEndpoint) ? 1 : 0;
		}
		double num = Math.Max(0.0001, _maximumLimit - _finiteMinimumLimit);
		return Math.Clamp((value - _finiteMinimumLimit) / num, 0.0, 1.0);
	}

	private bool IsUnlimited(double value)
	{
		if (_allowUnlimited)
		{
			return value < 0.0;
		}
		return false;
	}

	private string FormatValue(double value)
	{
		if (IsUnlimited(value))
		{
			return "无限制";
		}
		if (!(Math.Abs(value - Math.Round(value)) < 0.0001))
		{
			return value.ToString("0.##");
		}
		return Math.Round(value).ToString("0");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Configure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimumLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximumLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "allowUnlimited", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetTitle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetValues, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnRangeTrackGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.PreviewPointer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "pointerX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMinimumHandleCloser, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "pointerX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnUnlimitedMinimumToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnUnlimitedMaximumToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PositionHandle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "handle", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Float, "centerX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeEndpoint, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Quantize, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNormalizedValue, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "minimumEndpoint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsUnlimited, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Configure && args.Count == 4)
		{
			Configure(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTitle && args.Count == 1)
		{
			SetTitle(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetValues && args.Count == 2)
		{
			SetValues(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnRangeTrackGuiInput && args.Count == 1)
		{
			OnRangeTrackGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewPointer && args.Count == 1)
		{
			PreviewPointer(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsMinimumHandleCloser && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMinimumHandleCloser(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.OnUnlimitedMinimumToggled && args.Count == 1)
		{
			OnUnlimitedMinimumToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnUnlimitedMaximumToggled && args.Count == 1)
		{
			OnUnlimitedMaximumToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateVisuals && args.Count == 0)
		{
			UpdateVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.PositionHandle && args.Count == 2)
		{
			PositionHandle(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeEndpoint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(NormalizeEndpoint(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.Quantize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(Quantize(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.GetNormalizedValue && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(GetNormalizedValue(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.IsUnlimited && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsUnlimited(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatValue(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
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
		if (method == MethodName.Configure)
		{
			return true;
		}
		if (method == MethodName.SetTitle)
		{
			return true;
		}
		if (method == MethodName.SetValues)
		{
			return true;
		}
		if (method == MethodName.OnRangeTrackGuiInput)
		{
			return true;
		}
		if (method == MethodName.PreviewPointer)
		{
			return true;
		}
		if (method == MethodName.IsMinimumHandleCloser)
		{
			return true;
		}
		if (method == MethodName.OnUnlimitedMinimumToggled)
		{
			return true;
		}
		if (method == MethodName.OnUnlimitedMaximumToggled)
		{
			return true;
		}
		if (method == MethodName.UpdateVisuals)
		{
			return true;
		}
		if (method == MethodName.PositionHandle)
		{
			return true;
		}
		if (method == MethodName.NormalizeEndpoint)
		{
			return true;
		}
		if (method == MethodName.Quantize)
		{
			return true;
		}
		if (method == MethodName.GetNormalizedValue)
		{
			return true;
		}
		if (method == MethodName.IsUnlimited)
		{
			return true;
		}
		if (method == MethodName.FormatValue)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._title)
		{
			_title = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._rangeTrack)
		{
			_rangeTrack = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._minimumHandle)
		{
			_minimumHandle = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._maximumHandle)
		{
			_maximumHandle = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._minimumValue)
		{
			_minimumValue = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._maximumValue)
		{
			_maximumValue = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._unlimitedControls)
		{
			_unlimitedControls = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._unlimitedMinimum)
		{
			_unlimitedMinimum = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._unlimitedMaximum)
		{
			_unlimitedMaximum = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._minimumLimit)
		{
			_minimumLimit = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._maximumLimit)
		{
			_maximumLimit = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._finiteMinimumLimit)
		{
			_finiteMinimumLimit = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._step)
		{
			_step = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._minimum)
		{
			_minimum = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._maximum)
		{
			_maximum = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._lastFiniteMinimum)
		{
			_lastFiniteMinimum = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._lastFiniteMaximum)
		{
			_lastFiniteMaximum = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._allowUnlimited)
		{
			_allowUnlimited = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragging)
		{
			_dragging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._draggingMinimum)
		{
			_draggingMinimum = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._title)
		{
			value = VariantUtils.CreateFrom(in _title);
			return true;
		}
		if (name == PropertyName._rangeTrack)
		{
			value = VariantUtils.CreateFrom(in _rangeTrack);
			return true;
		}
		if (name == PropertyName._minimumHandle)
		{
			value = VariantUtils.CreateFrom(in _minimumHandle);
			return true;
		}
		if (name == PropertyName._maximumHandle)
		{
			value = VariantUtils.CreateFrom(in _maximumHandle);
			return true;
		}
		if (name == PropertyName._minimumValue)
		{
			value = VariantUtils.CreateFrom(in _minimumValue);
			return true;
		}
		if (name == PropertyName._maximumValue)
		{
			value = VariantUtils.CreateFrom(in _maximumValue);
			return true;
		}
		if (name == PropertyName._unlimitedControls)
		{
			value = VariantUtils.CreateFrom(in _unlimitedControls);
			return true;
		}
		if (name == PropertyName._unlimitedMinimum)
		{
			value = VariantUtils.CreateFrom(in _unlimitedMinimum);
			return true;
		}
		if (name == PropertyName._unlimitedMaximum)
		{
			value = VariantUtils.CreateFrom(in _unlimitedMaximum);
			return true;
		}
		if (name == PropertyName._minimumLimit)
		{
			value = VariantUtils.CreateFrom(in _minimumLimit);
			return true;
		}
		if (name == PropertyName._maximumLimit)
		{
			value = VariantUtils.CreateFrom(in _maximumLimit);
			return true;
		}
		if (name == PropertyName._finiteMinimumLimit)
		{
			value = VariantUtils.CreateFrom(in _finiteMinimumLimit);
			return true;
		}
		if (name == PropertyName._step)
		{
			value = VariantUtils.CreateFrom(in _step);
			return true;
		}
		if (name == PropertyName._minimum)
		{
			value = VariantUtils.CreateFrom(in _minimum);
			return true;
		}
		if (name == PropertyName._maximum)
		{
			value = VariantUtils.CreateFrom(in _maximum);
			return true;
		}
		if (name == PropertyName._lastFiniteMinimum)
		{
			value = VariantUtils.CreateFrom(in _lastFiniteMinimum);
			return true;
		}
		if (name == PropertyName._lastFiniteMaximum)
		{
			value = VariantUtils.CreateFrom(in _lastFiniteMaximum);
			return true;
		}
		if (name == PropertyName._allowUnlimited)
		{
			value = VariantUtils.CreateFrom(in _allowUnlimited);
			return true;
		}
		if (name == PropertyName._dragging)
		{
			value = VariantUtils.CreateFrom(in _dragging);
			return true;
		}
		if (name == PropertyName._draggingMinimum)
		{
			value = VariantUtils.CreateFrom(in _draggingMinimum);
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
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._title, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rangeTrack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._minimumHandle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._maximumHandle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._minimumValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._maximumValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlimitedControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlimitedMinimum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlimitedMaximum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._minimumLimit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._maximumLimit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._finiteMinimumLimit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._step, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._minimum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._maximum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._lastFiniteMinimum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._lastFiniteMaximum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._allowUnlimited, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dragging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._draggingMinimum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._title, Variant.From(in _title));
		info.AddProperty(PropertyName._rangeTrack, Variant.From(in _rangeTrack));
		info.AddProperty(PropertyName._minimumHandle, Variant.From(in _minimumHandle));
		info.AddProperty(PropertyName._maximumHandle, Variant.From(in _maximumHandle));
		info.AddProperty(PropertyName._minimumValue, Variant.From(in _minimumValue));
		info.AddProperty(PropertyName._maximumValue, Variant.From(in _maximumValue));
		info.AddProperty(PropertyName._unlimitedControls, Variant.From(in _unlimitedControls));
		info.AddProperty(PropertyName._unlimitedMinimum, Variant.From(in _unlimitedMinimum));
		info.AddProperty(PropertyName._unlimitedMaximum, Variant.From(in _unlimitedMaximum));
		info.AddProperty(PropertyName._minimumLimit, Variant.From(in _minimumLimit));
		info.AddProperty(PropertyName._maximumLimit, Variant.From(in _maximumLimit));
		info.AddProperty(PropertyName._finiteMinimumLimit, Variant.From(in _finiteMinimumLimit));
		info.AddProperty(PropertyName._step, Variant.From(in _step));
		info.AddProperty(PropertyName._minimum, Variant.From(in _minimum));
		info.AddProperty(PropertyName._maximum, Variant.From(in _maximum));
		info.AddProperty(PropertyName._lastFiniteMinimum, Variant.From(in _lastFiniteMinimum));
		info.AddProperty(PropertyName._lastFiniteMaximum, Variant.From(in _lastFiniteMaximum));
		info.AddProperty(PropertyName._allowUnlimited, Variant.From(in _allowUnlimited));
		info.AddProperty(PropertyName._dragging, Variant.From(in _dragging));
		info.AddProperty(PropertyName._draggingMinimum, Variant.From(in _draggingMinimum));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddSignalEventDelegate(SignalName.DragStarted, backing_DragStarted);
		info.AddSignalEventDelegate(SignalName.ValuesPreviewed, backing_ValuesPreviewed);
		info.AddSignalEventDelegate(SignalName.ValuesCommitted, backing_ValuesCommitted);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._title, out var value))
		{
			_title = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._rangeTrack, out var value2))
		{
			_rangeTrack = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._minimumHandle, out var value3))
		{
			_minimumHandle = value3.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._maximumHandle, out var value4))
		{
			_maximumHandle = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._minimumValue, out var value5))
		{
			_minimumValue = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._maximumValue, out var value6))
		{
			_maximumValue = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._unlimitedControls, out var value7))
		{
			_unlimitedControls = value7.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._unlimitedMinimum, out var value8))
		{
			_unlimitedMinimum = value8.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._unlimitedMaximum, out var value9))
		{
			_unlimitedMaximum = value9.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._minimumLimit, out var value10))
		{
			_minimumLimit = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName._maximumLimit, out var value11))
		{
			_maximumLimit = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName._finiteMinimumLimit, out var value12))
		{
			_finiteMinimumLimit = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName._step, out var value13))
		{
			_step = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName._minimum, out var value14))
		{
			_minimum = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName._maximum, out var value15))
		{
			_maximum = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName._lastFiniteMinimum, out var value16))
		{
			_lastFiniteMinimum = value16.As<double>();
		}
		if (info.TryGetProperty(PropertyName._lastFiniteMaximum, out var value17))
		{
			_lastFiniteMaximum = value17.As<double>();
		}
		if (info.TryGetProperty(PropertyName._allowUnlimited, out var value18))
		{
			_allowUnlimited = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragging, out var value19))
		{
			_dragging = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._draggingMinimum, out var value20))
		{
			_draggingMinimum = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value21))
		{
			_updatingControls = value21.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<DragStartedEventHandler>(SignalName.DragStarted, out var value22))
		{
			backing_DragStarted = value22;
		}
		if (info.TryGetSignalEventDelegate<ValuesPreviewedEventHandler>(SignalName.ValuesPreviewed, out var value23))
		{
			backing_ValuesPreviewed = value23;
		}
		if (info.TryGetSignalEventDelegate<ValuesCommittedEventHandler>(SignalName.ValuesCommitted, out var value24))
		{
			backing_ValuesCommitted = value24;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(SignalName.DragStarted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "minimumHandle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.ValuesPreviewed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.ValuesCommitted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalDragStarted(bool minimumHandle)
	{
		EmitSignal(SignalName.DragStarted, new ReadOnlySpan<Variant>((Variant)minimumHandle));
	}

	protected void EmitSignalValuesPreviewed(double minimum, double maximum)
	{
		StringName valuesPreviewed = SignalName.ValuesPreviewed;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = minimum;
		buffer[1] = maximum;
		EmitSignal(valuesPreviewed, buffer);
	}

	protected void EmitSignalValuesCommitted(double minimum, double maximum)
	{
		StringName valuesCommitted = SignalName.ValuesCommitted;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = minimum;
		buffer[1] = maximum;
		EmitSignal(valuesCommitted, buffer);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.DragStarted && args.Count == 1)
		{
			backing_DragStarted?.Invoke(VariantUtils.ConvertTo<bool>(in args[0]));
		}
		else if (signal == SignalName.ValuesPreviewed && args.Count == 2)
		{
			backing_ValuesPreviewed?.Invoke(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
		}
		else if (signal == SignalName.ValuesCommitted && args.Count == 2)
		{
			backing_ValuesCommitted?.Invoke(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.DragStarted)
		{
			return true;
		}
		if (signal == SignalName.ValuesPreviewed)
		{
			return true;
		}
		if (signal == SignalName.ValuesCommitted)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
