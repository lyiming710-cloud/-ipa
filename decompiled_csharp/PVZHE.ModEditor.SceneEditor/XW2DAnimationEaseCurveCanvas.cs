using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/2D/Animation/XW2DAnimationEaseCurveCanvas.cs")]
public class XW2DAnimationEaseCurveCanvas : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName SetTransition = "SetTransition";

		public static readonly StringName SetEditable = "SetEditable";

		public static readonly StringName CancelPendingDrag = "CancelPendingDrag";

		public new static readonly StringName _GuiInput = "_GuiInput";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName BeginDrag = "BeginDrag";

		public static readonly StringName UpdateDrag = "UpdateDrag";

		public static readonly StringName FinishDrag = "FinishDrag";

		public static readonly StringName CancelDrag = "CancelDrag";

		public static readonly StringName QueueCurveRedraw = "QueueCurveRedraw";

		public static readonly StringName GetGraphRect = "GetGraphRect";

		public static readonly StringName GraphPoint = "GraphPoint";

		public static readonly StringName Evaluate = "Evaluate";

		public static readonly StringName NormalizeTransition = "NormalizeTransition";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName Transition = "Transition";

		public static readonly StringName Editable = "Editable";

		public static readonly StringName IsDragging = "IsDragging";

		public static readonly StringName DrawSampleCount = "DrawSampleCount";

		public static readonly StringName RedrawRevision = "RedrawRevision";

		public static readonly StringName _transition = "_transition";

		public static readonly StringName _dragStartTransition = "_dragStartTransition";

		public static readonly StringName _dragStartPosition = "_dragStartPosition";

		public static readonly StringName _editable = "_editable";

		public static readonly StringName _dragging = "_dragging";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const int CurveSampleCount = 48;

	private const float MinimumDragMagnitude = 0.125f;

	private const float MaximumTransitionMagnitude = 1000000f;

	private const float GraphPadding = 14f;

	private const float HandleSample = 0.64f;

	private float _transition = 1f;

	private float _dragStartTransition = 1f;

	private Vector2 _dragStartPosition;

	private bool _editable;

	private bool _dragging;

	public float Transition => _transition;

	public bool Editable => _editable;

	public bool IsDragging => _dragging;

	public int DrawSampleCount => 48;

	public int RedrawRevision { get; private set; }

	public event Action<float> TransitionPreviewChanged;

	public event Action<float, float> TransitionCommitRequested;

	public override void _Ready()
	{
		CustomMinimumSize = new Vector2(220f, 132f);
		MouseDefaultCursorShape = CursorShape.PointingHand;
		MouseFilter = MouseFilterEnum.Stop;
		FocusMode = FocusModeEnum.All;
		ClipContents = true;
		SetProcess(enable: false);
		QueueCurveRedraw();
	}

	public override void _Notification(int what)
	{
		base._Notification(what);
		if ((long)what == 40 && IsNodeReady())
		{
			QueueCurveRedraw();
		}
		else if ((long)what == 31 && !IsVisibleInTree())
		{
			CancelDrag();
		}
	}

	public void SetTransition(float transition)
	{
		float num = NormalizeTransition(transition);
		if (!Mathf.IsEqualApprox(_transition, num))
		{
			_transition = num;
			QueueCurveRedraw();
		}
	}

	public void SetEditable(bool editable)
	{
		if (_editable != editable)
		{
			_editable = editable;
			if (!editable)
			{
				CancelDrag();
			}
			MouseDefaultCursorShape = (CursorShape)(editable ? 2 : 8);
			QueueCurveRedraw();
		}
	}

	public void CancelPendingDrag()
	{
		CancelDrag();
	}

	public override void _GuiInput(InputEvent inputEvent)
	{
		if (!_editable)
		{
			return;
		}
		if (inputEvent is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			if (inputEventMouseButton.Pressed)
			{
				BeginDrag(inputEventMouseButton.Position);
			}
			else
			{
				FinishDrag();
			}
			AcceptEvent();
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _dragging)
		{
			UpdateDrag(inputEventMouseMotion.Position);
			AcceptEvent();
		}
	}

	public override void _Draw()
	{
		Rect2 graphRect = GetGraphRect();
		Color color = (_editable ? new Color("13241d") : new Color("151c19"));
		Color color2 = (_editable ? new Color("5d8f5b") : new Color("3e4b43"));
		DrawRect(new Rect2(Vector2.Zero, Size), color);
		DrawRect(new Rect2(Vector2.Zero, Size), color2, filled: false, 1.5f);
		for (int i = 0; i <= 4; i++)
		{
			float weight = (float)i / 4f;
			float x = Mathf.Lerp(graphRect.Position.X, graphRect.End.X, weight);
			float y = Mathf.Lerp(graphRect.End.Y, graphRect.Position.Y, weight);
			DrawLine(new Vector2(x, graphRect.Position.Y), new Vector2(x, graphRect.End.Y), new Color("38504666"), 1f);
			DrawLine(new Vector2(graphRect.Position.X, y), new Vector2(graphRect.End.X, y), new Color("38504666"), 1f);
		}
		DrawLine(new Vector2(graphRect.Position.X, graphRect.End.Y), new Vector2(graphRect.End.X, graphRect.Position.Y), new Color("6e827577"), 1f);
		Vector2[] array = new Vector2[49];
		for (int j = 0; j <= 48; j++)
		{
			float x2 = (float)j / 48f;
			float y2 = Evaluate(x2, _transition);
			array[j] = GraphPoint(graphRect, x2, y2);
		}
		DrawPolyline(array, _editable ? new Color("f1c94f") : new Color("718078"), 2.5f, antialiased: true);
		Vector2 position = GraphPoint(graphRect, 0.64f, Evaluate(0.64f, _transition));
		DrawCircle(position, _dragging ? 7.5f : 6f, _editable ? new Color("ffdd69") : new Color("68736d"));
		DrawCircle(position, _dragging ? 7.5f : 6f, Colors.White, filled: false, 1.5f, antialiased: true);
		Font themeDefaultFont = GetThemeDefaultFont();
		int fontSize = Math.Max(10, GetThemeDefaultFontSize() - 2);
		DrawString(themeDefaultFont, new Vector2(graphRect.Position.X, Size.Y - 3f), $"曲率 {_transition:0.###} · 拖动金色节点", HorizontalAlignment.Left, graphRect.Size.X, fontSize, _editable ? new Color("dcebd6") : new Color("7f8b84"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
	}

	private void BeginDrag(Vector2 position)
	{
		_dragging = true;
		_dragStartPosition = position;
		_dragStartTransition = _transition;
		GrabFocus();
		QueueCurveRedraw();
	}

	private void UpdateDrag(Vector2 position)
	{
		float num = Math.Max(48f, GetGraphRect().Size.Y);
		float y = (_dragStartPosition.Y - position.Y) / num * 3.2f;
		float num2 = Math.Clamp(Math.Clamp(Math.Abs(_dragStartTransition), 0.125f, 1000000f) * Mathf.Pow(2f, y), 1E-06f, 1000000f);
		float num3 = NormalizeTransition(((_dragStartTransition < 0f) ? (-1f) : 1f) * num2);
		if (!Mathf.IsEqualApprox(num3, _transition))
		{
			_transition = num3;
			TransitionPreviewChanged?.Invoke(_transition);
			QueueCurveRedraw();
		}
	}

	private void FinishDrag()
	{
		if (_dragging)
		{
			_dragging = false;
			float dragStartTransition = _dragStartTransition;
			float transition = _transition;
			QueueCurveRedraw();
			if (!Mathf.IsEqualApprox(dragStartTransition, transition))
			{
				TransitionCommitRequested?.Invoke(dragStartTransition, transition);
			}
		}
	}

	private void CancelDrag()
	{
		if (_dragging)
		{
			float transition = _transition;
			_dragging = false;
			_transition = _dragStartTransition;
			if (!Mathf.IsEqualApprox(transition, _transition))
			{
				TransitionPreviewChanged?.Invoke(_transition);
			}
			QueueCurveRedraw();
		}
	}

	private void QueueCurveRedraw()
	{
		RedrawRevision++;
		QueueRedraw();
	}

	private Rect2 GetGraphRect()
	{
		float width = Math.Max(1f, Size.X - 28f);
		float height = Math.Max(1f, Size.Y - 28f - 16f);
		return new Rect2(14f, 14f, width, height);
	}

	private static Vector2 GraphPoint(Rect2 graph, float x, float y)
	{
		return new Vector2(Mathf.Lerp(graph.Position.X, graph.End.X, Mathf.Clamp(x, 0f, 1f)), Mathf.Lerp(graph.End.Y, graph.Position.Y, Mathf.Clamp(y, 0f, 1f)));
	}

	private static float Evaluate(float x, float transition)
	{
		float num = Mathf.Ease(Mathf.Clamp(x, 0f, 1f), transition);
		if (!float.IsFinite(num))
		{
			return x;
		}
		return Mathf.Clamp(num, 0f, 1f);
	}

	private static float NormalizeTransition(float transition)
	{
		if (!float.IsFinite(transition))
		{
			return 1f;
		}
		float num = ((transition < 0f) ? (-1f) : 1f);
		float num2 = Math.Min(Math.Abs(transition), 1000000f);
		if (transition != 0f)
		{
			return num * num2;
		}
		return 0f;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetEditable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "editable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelPendingDrag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginDrag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateDrag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishDrag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelDrag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueCurveRedraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGraphRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GraphPoint, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "graph", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "x", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "y", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Evaluate, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "x", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeTransition, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTransition && args.Count == 1)
		{
			SetTransition(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetEditable && args.Count == 1)
		{
			SetEditable(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CancelPendingDrag && args.Count == 0)
		{
			CancelPendingDrag();
			ret = default;
			return true;
		}
		if (method == MethodName._GuiInput && args.Count == 1)
		{
			_GuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginDrag && args.Count == 1)
		{
			BeginDrag(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDrag && args.Count == 1)
		{
			UpdateDrag(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishDrag && args.Count == 0)
		{
			FinishDrag();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelDrag && args.Count == 0)
		{
			CancelDrag();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueCurveRedraw && args.Count == 0)
		{
			QueueCurveRedraw();
			ret = default;
			return true;
		}
		if (method == MethodName.GetGraphRect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetGraphRect());
			return true;
		}
		if (method == MethodName.GraphPoint && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GraphPoint(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.Evaluate && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(Evaluate(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeTransition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(NormalizeTransition(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GraphPoint && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GraphPoint(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.Evaluate && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(Evaluate(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeTransition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(NormalizeTransition(VariantUtils.ConvertTo<float>(in args[0])));
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
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.SetTransition)
		{
			return true;
		}
		if (method == MethodName.SetEditable)
		{
			return true;
		}
		if (method == MethodName.CancelPendingDrag)
		{
			return true;
		}
		if (method == MethodName._GuiInput)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.BeginDrag)
		{
			return true;
		}
		if (method == MethodName.UpdateDrag)
		{
			return true;
		}
		if (method == MethodName.FinishDrag)
		{
			return true;
		}
		if (method == MethodName.CancelDrag)
		{
			return true;
		}
		if (method == MethodName.QueueCurveRedraw)
		{
			return true;
		}
		if (method == MethodName.GetGraphRect)
		{
			return true;
		}
		if (method == MethodName.GraphPoint)
		{
			return true;
		}
		if (method == MethodName.Evaluate)
		{
			return true;
		}
		if (method == MethodName.NormalizeTransition)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.RedrawRevision)
		{
			RedrawRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._transition)
		{
			_transition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._dragStartTransition)
		{
			_dragStartTransition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._dragStartPosition)
		{
			_dragStartPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._editable)
		{
			_editable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragging)
		{
			_dragging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Transition)
		{
			value = VariantUtils.CreateFrom<float>(Transition);
			return true;
		}
		bool from;
		if (name == PropertyName.Editable)
		{
			from = Editable;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsDragging)
		{
			from = IsDragging;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.DrawSampleCount)
		{
			from2 = DrawSampleCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.RedrawRevision)
		{
			from2 = RedrawRevision;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._transition)
		{
			value = VariantUtils.CreateFrom(in _transition);
			return true;
		}
		if (name == PropertyName._dragStartTransition)
		{
			value = VariantUtils.CreateFrom(in _dragStartTransition);
			return true;
		}
		if (name == PropertyName._dragStartPosition)
		{
			value = VariantUtils.CreateFrom(in _dragStartPosition);
			return true;
		}
		if (name == PropertyName._editable)
		{
			value = VariantUtils.CreateFrom(in _editable);
			return true;
		}
		if (name == PropertyName._dragging)
		{
			value = VariantUtils.CreateFrom(in _dragging);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._transition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._dragStartTransition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._dragStartPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._editable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dragging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.Transition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Editable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsDragging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DrawSampleCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RedrawRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.RedrawRevision, Variant.From<int>(RedrawRevision));
		info.AddProperty(PropertyName._transition, Variant.From(in _transition));
		info.AddProperty(PropertyName._dragStartTransition, Variant.From(in _dragStartTransition));
		info.AddProperty(PropertyName._dragStartPosition, Variant.From(in _dragStartPosition));
		info.AddProperty(PropertyName._editable, Variant.From(in _editable));
		info.AddProperty(PropertyName._dragging, Variant.From(in _dragging));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.RedrawRevision, out var value))
		{
			RedrawRevision = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._transition, out var value2))
		{
			_transition = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName._dragStartTransition, out var value3))
		{
			_dragStartTransition = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName._dragStartPosition, out var value4))
		{
			_dragStartPosition = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._editable, out var value5))
		{
			_editable = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragging, out var value6))
		{
			_dragging = value6.As<bool>();
		}
	}
}
