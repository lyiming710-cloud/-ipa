using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

internal sealed class XWSlotPositionOverlay : Control
{
	[Signal]
	public delegate void MarkerPathRequestedEventHandler(NodePath path);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName DrawOccupantGhost = "DrawOccupantGhost";

		public new static readonly StringName DrawEllipse = "DrawEllipse";

		public static readonly StringName FindNearestMarker = "FindNearestMarker";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName MarkerHandlePosition = "MarkerHandlePosition";

		public static readonly StringName _definition = "_definition";

		public static readonly StringName _owner = "_owner";

		public static readonly StringName _marker = "_marker";

		public static readonly StringName _dragging = "_dragging";

		public static readonly StringName _dragPosition = "_dragPosition";
	}

	public new class SignalName : Control.SignalName
	{
		public static readonly StringName MarkerPathRequested = "MarkerPathRequested";
	}

	private SlotComponentDefinition _definition;

	private Node _owner;

	private SlotComponent _runtime;

	private Marker2D _marker;

	private bool _dragging;

	private Vector2 _dragPosition;

	private MarkerPathRequestedEventHandler backing_MarkerPathRequested;

	public Vector2 MarkerHandlePosition { get; private set; }

	public event MarkerPathRequestedEventHandler MarkerPathRequested
	{
		add
		{
			backing_MarkerPathRequested = (MarkerPathRequestedEventHandler)Delegate.Combine(backing_MarkerPathRequested, value);
		}
		remove
		{
			backing_MarkerPathRequested = (MarkerPathRequestedEventHandler)Delegate.Remove(backing_MarkerPathRequested, value);
		}
	}

	public override void _Ready()
	{
		SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect, LayoutPresetMode.Minsize);
		MouseFilter = MouseFilterEnum.Ignore;
		SetProcess(enable: false);
		SetPhysicsProcess(enable: false);
	}

	public void Bind(SlotComponentDefinition definition, Node owner, SlotComponent runtime, Marker2D marker)
	{
		_definition = definition;
		_owner = owner;
		_runtime = runtime;
		_marker = marker;
		if (GodotObject.IsInstanceValid(marker))
		{
			Transform2D transform2D = GetGlobalTransformWithCanvas().AffineInverse();
			MarkerHandlePosition = transform2D * marker.GetGlobalTransformWithCanvas().Origin;
			_dragPosition = MarkerHandlePosition;
		}
		else
		{
			MarkerHandlePosition = Vector2.Zero;
			_dragPosition = Vector2.Zero;
			_dragging = false;
		}
		QueueRedraw();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!Visible || !GodotObject.IsInstanceValid(_marker))
		{
			return;
		}
		Transform2D transform2D = GetGlobalTransformWithCanvas().AffineInverse();
		if (inputEvent is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			Vector2 vector = transform2D * inputEventMouseButton.Position;
			if (inputEventMouseButton.Pressed)
			{
				_dragging = vector.DistanceTo(MarkerHandlePosition) <= 22f;
				if (_dragging)
				{
					_dragPosition = vector;
					GetViewport()?.SetInputAsHandled();
				}
			}
			else if (_dragging)
			{
				_dragging = false;
				_dragPosition = vector;
				Marker2D marker2D = FindNearestMarker(vector);
				if (GodotObject.IsInstanceValid(marker2D) && GodotObject.IsInstanceValid(_owner))
				{
					EmitSignal(SignalName.MarkerPathRequested, _owner.GetPathTo(marker2D));
				}
				QueueRedraw();
				GetViewport()?.SetInputAsHandled();
			}
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _dragging)
		{
			_dragPosition = transform2D * inputEventMouseMotion.Position;
			QueueRedraw();
			GetViewport()?.SetInputAsHandled();
		}
	}

	public override void _Draw()
	{
		if (GodotObject.IsInstanceValid(_marker))
		{
			Vector2 vector = (_dragging ? _dragPosition : MarkerHandlePosition);
			Color color = new Color("64cef1");
			DrawLine(new Vector2(10f, vector.Y), new Vector2(Math.Max(20f, Size.X - 10f), vector.Y), color, 2f);
			DrawCircle(vector, 13f, new Color("1e6680"));
			DrawCircle(vector, 8f, new Color("8de7ff"));
			DrawLine(vector, vector + new Vector2(0f, 44f), color, 2f);
			DrawOccupantGhost(vector + new Vector2(-42f, -24f), slot: true);
			DrawOccupantGhost(vector + new Vector2(42f, -24f), slot: false);
			Font fallbackFont = ThemeDB.FallbackFont;
			DrawString(fallbackFont, vector + new Vector2(-78f, 62f), "同一目标高度 · 两个独立绑定层", HorizontalAlignment.Left, -1f, 13, new Color("bcecff"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}
	}

	private void DrawOccupantGhost(Vector2 center, bool slot)
	{
		bool flag = (slot ? GodotObject.IsInstanceValid(_runtime?.slotCharacter) : GodotObject.IsInstanceValid(_runtime?.surroundCharacter));
		Color c = (slot ? new Color("55b7e5") : new Color("9a82dc"));
		Rect2 rect = new Rect2(center - new Vector2(24f, 32f), new Vector2(48f, 54f));
		DrawRect(rect, new Color(c, flag ? 0.78f : 0.3f));
		DrawCircle(center - new Vector2(0f, 38f), 15f, new Color(c, flag ? 0.92f : 0.42f));
		SlotComponentDefinition definition = _definition;
		if (definition != null && !definition.hideShadow)
		{
			DrawEllipse(center + new Vector2(0f, 26f), new Vector2(24f, 7f), new Color(0f, 0f, 0f, 0.45f));
		}
		Font fallbackFont = ThemeDB.FallbackFont;
		DrawString(fallbackFont, center + new Vector2(-25f, 45f), slot ? "槽内" : "环绕", HorizontalAlignment.Left, -1f, 12, new Color("e5f6ff"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
	}

	private void DrawEllipse(Vector2 center, Vector2 radius, Color color)
	{
		Vector2[] array = new Vector2[25];
		for (int i = 0; i < array.Length; i++)
		{
			float s = (float)Math.PI * 2f * (float)i / (float)(array.Length - 1);
			array[i] = center + new Vector2(Mathf.Cos(s) * radius.X, Mathf.Sin(s) * radius.Y);
		}
		DrawPolyline(array, color, 2f);
	}

	private Marker2D FindNearestMarker(Vector2 local)
	{
		Marker2D result = null;
		float num = 70f;
		foreach (Marker2D item in EnumerateMarkers(_owner))
		{
			Vector2 to = GetGlobalTransformWithCanvas().AffineInverse() * item.GetGlobalTransformWithCanvas().Origin;
			float num2 = local.DistanceTo(to);
			if (num2 < num)
			{
				num = num2;
				result = item;
			}
		}
		return result;
	}

	private static IEnumerable<Marker2D> EnumerateMarkers(Node root)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			yield break;
		}
		if (root is Marker2D marker2D)
		{
			yield return marker2D;
		}
		foreach (Node child in root.GetChildren())
		{
			foreach (Marker2D item in EnumerateMarkers(child))
			{
				yield return item;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawOccupantGhost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "center", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawEllipse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "center", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "radius", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindNearestMarker, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Marker2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "local", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawOccupantGhost && args.Count == 2)
		{
			DrawOccupantGhost(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawEllipse && args.Count == 3)
		{
			DrawEllipse(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindNearestMarker && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Marker2D>(FindNearestMarker(VariantUtils.ConvertTo<Vector2>(in args[0])));
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
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.DrawOccupantGhost)
		{
			return true;
		}
		if (method == MethodName.DrawEllipse)
		{
			return true;
		}
		if (method == MethodName.FindNearestMarker)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.MarkerHandlePosition)
		{
			MarkerHandlePosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._definition)
		{
			_definition = VariantUtils.ConvertTo<SlotComponentDefinition>(in value);
			return true;
		}
		if (name == PropertyName._owner)
		{
			_owner = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._marker)
		{
			_marker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName._dragging)
		{
			_dragging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragPosition)
		{
			_dragPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.MarkerHandlePosition)
		{
			value = VariantUtils.CreateFrom<Vector2>(MarkerHandlePosition);
			return true;
		}
		if (name == PropertyName._definition)
		{
			value = VariantUtils.CreateFrom(in _definition);
			return true;
		}
		if (name == PropertyName._owner)
		{
			value = VariantUtils.CreateFrom(in _owner);
			return true;
		}
		if (name == PropertyName._marker)
		{
			value = VariantUtils.CreateFrom(in _marker);
			return true;
		}
		if (name == PropertyName._dragging)
		{
			value = VariantUtils.CreateFrom(in _dragging);
			return true;
		}
		if (name == PropertyName._dragPosition)
		{
			value = VariantUtils.CreateFrom(in _dragPosition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._owner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._marker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dragging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._dragPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.MarkerHandlePosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.MarkerHandlePosition, Variant.From<Vector2>(MarkerHandlePosition));
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
		info.AddProperty(PropertyName._owner, Variant.From(in _owner));
		info.AddProperty(PropertyName._marker, Variant.From(in _marker));
		info.AddProperty(PropertyName._dragging, Variant.From(in _dragging));
		info.AddProperty(PropertyName._dragPosition, Variant.From(in _dragPosition));
		info.AddSignalEventDelegate(SignalName.MarkerPathRequested, backing_MarkerPathRequested);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.MarkerHandlePosition, out var value))
		{
			MarkerHandlePosition = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._definition, out var value2))
		{
			_definition = value2.As<SlotComponentDefinition>();
		}
		if (info.TryGetProperty(PropertyName._owner, out var value3))
		{
			_owner = value3.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._marker, out var value4))
		{
			_marker = value4.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName._dragging, out var value5))
		{
			_dragging = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragPosition, out var value6))
		{
			_dragPosition = value6.As<Vector2>();
		}
		if (info.TryGetSignalEventDelegate<MarkerPathRequestedEventHandler>(SignalName.MarkerPathRequested, out var value7))
		{
			backing_MarkerPathRequested = value7;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.MarkerPathRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.NodePath, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	private void EmitSignalMarkerPathRequested(NodePath path)
	{
		EmitSignal(SignalName.MarkerPathRequested, new ReadOnlySpan<Variant>((Variant)path));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.MarkerPathRequested && args.Count == 1)
		{
			backing_MarkerPathRequested?.Invoke(VariantUtils.ConvertTo<NodePath>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.MarkerPathRequested)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
