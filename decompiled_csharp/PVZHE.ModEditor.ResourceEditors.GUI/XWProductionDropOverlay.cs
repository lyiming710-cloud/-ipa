using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

internal sealed class XWProductionDropOverlay : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName Bind = "Bind";

		public static readonly StringName RebuildPositions = "RebuildPositions";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName FindMarker = "FindMarker";

		public static readonly StringName DropMarkerAt = "DropMarkerAt";

		public static readonly StringName GetMarkerHandlePosition = "GetMarkerHandlePosition";

		public static readonly StringName FindNearestMarker = "FindNearestMarker";

		public new static readonly StringName _Draw = "_Draw";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName MarkerPinCount = "MarkerPinCount";

		public static readonly StringName RedrawRevision = "RedrawRevision";

		public static readonly StringName _definition = "_definition";

		public static readonly StringName _owner = "_owner";

		public static readonly StringName _dragMarkerIndex = "_dragMarkerIndex";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const float MarkerHitRadius = 18f;

	private readonly List<Vector2> _markerPositions = new List<Vector2>();

	private readonly List<(Marker2D Marker, NodePath Path)> _markerCandidates = new List<(Marker2D, NodePath)>();

	private ProduceComponentDefinition _definition;

	private Node _owner;

	private int _dragMarkerIndex = -1;

	public int MarkerPinCount => _markerPositions.Count;

	public int RedrawRevision { get; private set; }

	public event Action<int, NodePath> MarkerPathRequested;

	public XWProductionDropOverlay()
	{
		SetProcess(enable: false);
		SetPhysicsProcess(enable: false);
		SetProcessInput(enable: false);
	}

	public void Bind(ProduceComponentDefinition definition, Node owner)
	{
		_definition = definition;
		_owner = ((GodotObject.IsInstanceValid(owner) && owner.IsInsideTree()) ? owner : null);
		RebuildPositions();
		RedrawRevision++;
		QueueRedraw();
	}

	private void RebuildPositions()
	{
		_markerPositions.Clear();
		_markerCandidates.Clear();
		if (GodotObject.IsInstanceValid(_owner))
		{
			foreach (Node item2 in _owner.FindChildren("*", "Marker2D", recursive: true, owned: false))
			{
				if (item2 is Marker2D marker2D && marker2D.IsInsideTree())
				{
					_markerCandidates.Add((marker2D, _owner.GetPathTo(marker2D)));
				}
			}
		}
		int valueOrDefault = (_definition?.markerPaths?.Count).GetValueOrDefault();
		for (int i = 0; i < valueOrDefault; i++)
		{
			Vector2 item = new Vector2(-24 + i * 28, -54f);
			if (GodotObject.IsInstanceValid(_owner))
			{
				Marker2D nodeOrNull = _owner.GetNodeOrNull<Marker2D>(_definition.markerPaths[i]);
				if (GodotObject.IsInstanceValid(nodeOrNull) && nodeOrNull.IsInsideTree())
				{
					item = ToLocal(nodeOrNull.GlobalPosition);
				}
				else if (_owner is Node2D node2D)
				{
					item = ToLocal(node2D.GlobalPosition) + new Vector2(i * 22, -32f);
				}
			}
			_markerPositions.Add(item);
		}
		if (valueOrDefault == 0 && _owner is Node2D node2D2 && GodotObject.IsInstanceValid(node2D2))
		{
			_markerPositions.Add(ToLocal(node2D2.GlobalPosition));
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!Visible || !IsProcessingInput() || (_definition?.markerPaths?.Count).GetValueOrDefault() == 0)
		{
			return;
		}
		if (inputEvent is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			Vector2 vector = GetGlobalTransformWithCanvas().AffineInverse() * inputEventMouseButton.Position;
			if (inputEventMouseButton.Pressed)
			{
				_dragMarkerIndex = FindMarker(vector);
				if (_dragMarkerIndex >= 0)
				{
					GetViewport()?.SetInputAsHandled();
				}
			}
			else if (_dragMarkerIndex >= 0)
			{
				int dragMarkerIndex = _dragMarkerIndex;
				_dragMarkerIndex = -1;
				if (!DropMarkerAt(dragMarkerIndex, vector))
				{
					RebuildPositions();
				}
				QueueRedraw();
				GetViewport()?.SetInputAsHandled();
			}
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _dragMarkerIndex >= 0)
		{
			_markerPositions[_dragMarkerIndex] = GetGlobalTransformWithCanvas().AffineInverse() * inputEventMouseMotion.Position;
			QueueRedraw();
			GetViewport()?.SetInputAsHandled();
		}
	}

	private int FindMarker(Vector2 position)
	{
		for (int num = _markerPositions.Count - 1; num >= 0; num--)
		{
			if (_markerPositions[num].DistanceTo(position) <= 18f)
			{
				return num;
			}
		}
		return -1;
	}

	public bool DropMarkerAt(int index, Vector2 localPosition)
	{
		if (index < 0 || index >= (_definition?.markerPaths?.Count).GetValueOrDefault())
		{
			return false;
		}
		NodePath nodePath = FindNearestMarker(localPosition);
		if (nodePath == null || nodePath.IsEmpty)
		{
			return false;
		}
		_markerPositions[index] = localPosition;
		MarkerPathRequested?.Invoke(index, nodePath);
		QueueRedraw();
		return true;
	}

	public Vector2 GetMarkerHandlePosition(int index)
	{
		if (index < 0 || index >= _markerPositions.Count)
		{
			return Vector2.Zero;
		}
		return _markerPositions[index];
	}

	private NodePath FindNearestMarker(Vector2 localPosition)
	{
		float num = 54f;
		NodePath result = null;
		foreach (var (marker2D, nodePath) in _markerCandidates)
		{
			if (GodotObject.IsInstanceValid(marker2D) && marker2D.IsInsideTree())
			{
				float num2 = ToLocal(marker2D.GlobalPosition).DistanceTo(localPosition);
				if (num2 < num)
				{
					num = num2;
					result = nodePath;
				}
			}
		}
		return result;
	}

	public override void _Draw()
	{
		if (GodotObject.IsInstanceValid(_definition))
		{
			Font fallbackFont = ThemeDB.FallbackFont;
			int num = Math.Max(0, _definition.num);
			int num2 = Math.Max(1, _markerPositions.Count);
			int value = num * num2;
			for (int i = 0; i < _markerPositions.Count; i++)
			{
				Vector2 vector = _markerPositions[i];
				DrawCircle(vector, 13f, new Color(0.95f, 0.78f, 0.2f, 0.28f));
				DrawCircle(vector, 7f, new Color("ffe45c"));
				DrawLine(vector + new Vector2(0f, 8f), vector + new Vector2(0f, 24f), new Color("ffe45c"), 2f);
				DrawString(fallbackFont, vector + new Vector2(12f, -10f), $"{i + 1} × {num}", HorizontalAlignment.Left, -1f, 13, new Color("fff2a8"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			}
			Vector2 pos = ((_markerPositions.Count > 0) ? (_markerPositions[0] + new Vector2(-30f, 48f)) : new Vector2(-45f, 40f));
			string a = _definition.produceType?.Trim() ?? string.Empty;
			string text;
			if (_definition.onlyEmit)
			{
				text = $"每轮事件总量 {value}";
			}
			else if (string.Equals(a, "Packet", StringComparison.OrdinalIgnoreCase))
			{
				text = $"每轮最多 {num2} 张";
			}
			else
			{
				text = ((string.Equals(a, "Coin", StringComparison.OrdinalIgnoreCase) && _definition.coinRandom) ? $"每轮 {num2} 枚随机金币" : $"每轮总量 {value}");
			}
			DrawString(fallbackFont, pos, text, HorizontalAlignment.Left, -1f, 14, new Color("ffe28a"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildPositions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindMarker, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DropMarkerAt, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "localPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMarkerHandlePosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindNearestMarker, new PropertyInfo(Variant.Type.NodePath, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "localPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Bind && args.Count == 2)
		{
			Bind(VariantUtils.ConvertTo<ProduceComponentDefinition>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildPositions && args.Count == 0)
		{
			RebuildPositions();
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindMarker && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindMarker(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.DropMarkerAt && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(DropMarkerAt(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.GetMarkerHandlePosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMarkerHandlePosition(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.FindNearestMarker && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<NodePath>(FindNearestMarker(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Bind)
		{
			return true;
		}
		if (method == MethodName.RebuildPositions)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.FindMarker)
		{
			return true;
		}
		if (method == MethodName.DropMarkerAt)
		{
			return true;
		}
		if (method == MethodName.GetMarkerHandlePosition)
		{
			return true;
		}
		if (method == MethodName.FindNearestMarker)
		{
			return true;
		}
		if (method == MethodName._Draw)
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
		if (name == PropertyName._definition)
		{
			_definition = VariantUtils.ConvertTo<ProduceComponentDefinition>(in value);
			return true;
		}
		if (name == PropertyName._owner)
		{
			_owner = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._dragMarkerIndex)
		{
			_dragMarkerIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.MarkerPinCount)
		{
			from = MarkerPinCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RedrawRevision)
		{
			from = RedrawRevision;
			value = VariantUtils.CreateFrom(in from);
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
		if (name == PropertyName._dragMarkerIndex)
		{
			value = VariantUtils.CreateFrom(in _dragMarkerIndex);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._dragMarkerIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.MarkerPinCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RedrawRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.RedrawRevision, Variant.From<int>(RedrawRevision));
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
		info.AddProperty(PropertyName._owner, Variant.From(in _owner));
		info.AddProperty(PropertyName._dragMarkerIndex, Variant.From(in _dragMarkerIndex));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.RedrawRevision, out var value))
		{
			RedrawRevision = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._definition, out var value2))
		{
			_definition = value2.As<ProduceComponentDefinition>();
		}
		if (info.TryGetProperty(PropertyName._owner, out var value3))
		{
			_owner = value3.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._dragMarkerIndex, out var value4))
		{
			_dragMarkerIndex = value4.As<int>();
		}
	}
}
