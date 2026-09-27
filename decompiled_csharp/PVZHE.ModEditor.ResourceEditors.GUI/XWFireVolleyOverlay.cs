using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

internal sealed class XWFireVolleyOverlay : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName Bind = "Bind";

		public static readonly StringName RebuildMarkerPositions = "RebuildMarkerPositions";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName FindMarker = "FindMarker";

		public static readonly StringName FindNearestCandidate = "FindNearestCandidate";

		public static readonly StringName DropMarkerAt = "DropMarkerAt";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName DrawTargetDummies = "DrawTargetDummies";

		public static readonly StringName DrawTrajectories = "DrawTrajectories";

		public static readonly StringName DrawMarkers = "DrawMarkers";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName RedrawRevision = "RedrawRevision";

		public static readonly StringName MarkerHandleCount = "MarkerHandleCount";

		public static readonly StringName TrajectoryCount = "TrajectoryCount";

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

	private FireComponentDefinition _definition;

	private Node _owner;

	private int _dragMarkerIndex = -1;

	public int RedrawRevision { get; private set; }

	public int MarkerHandleCount => _markerPositions.Count;

	public int TrajectoryCount => (_definition?.fireProjectileList?.Count).GetValueOrDefault();

	public event Action<int, NodePath> MarkerPathRequested;

	public XWFireVolleyOverlay()
	{
		SetProcess(enable: false);
		SetPhysicsProcess(enable: false);
		SetProcessInput(enable: false);
	}

	public void Bind(FireComponentDefinition definition, Node owner)
	{
		_definition = definition;
		_owner = ((GodotObject.IsInstanceValid(owner) && owner.IsInsideTree()) ? owner : null);
		RebuildMarkerPositions();
		RedrawRevision++;
		QueueRedraw();
	}

	private void RebuildMarkerPositions()
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
		int valueOrDefault = (_definition?.firePosMarkerPaths?.Count).GetValueOrDefault();
		for (int i = 0; i < valueOrDefault; i++)
		{
			Vector2 item = new Vector2(-24 + i * 28, -62f);
			if (GodotObject.IsInstanceValid(_owner))
			{
				Marker2D nodeOrNull = _owner.GetNodeOrNull<Marker2D>(_definition.firePosMarkerPaths[i]);
				if (GodotObject.IsInstanceValid(nodeOrNull) && nodeOrNull.IsInsideTree())
				{
					item = ToLocal(nodeOrNull.GlobalPosition);
				}
			}
			_markerPositions.Add(item);
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!Visible || !IsProcessingInput())
		{
			return;
		}
		if (inputEvent is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			Vector2 vector = ToLocal(inputEventMouseButton.Position);
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
					RebuildMarkerPositions();
				}
				QueueRedraw();
				GetViewport()?.SetInputAsHandled();
			}
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion && _dragMarkerIndex >= 0)
		{
			_markerPositions[_dragMarkerIndex] = ToLocal(inputEventMouseMotion.Position);
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

	private NodePath FindNearestCandidate(Vector2 local)
	{
		float num = 54f;
		NodePath result = null;
		foreach (var (marker2D, nodePath) in _markerCandidates)
		{
			if (GodotObject.IsInstanceValid(marker2D) && marker2D.IsInsideTree())
			{
				float num2 = ToLocal(marker2D.GlobalPosition).DistanceTo(local);
				if (num2 < num)
				{
					num = num2;
					result = nodePath;
				}
			}
		}
		return result;
	}

	public bool DropMarkerAt(int index, Vector2 localPosition)
	{
		if (index < 0 || index >= _markerPositions.Count)
		{
			return false;
		}
		NodePath nodePath = FindNearestCandidate(localPosition);
		if (nodePath == null || nodePath.IsEmpty)
		{
			return false;
		}
		_markerPositions[index] = localPosition;
		MarkerPathRequested?.Invoke(index, nodePath);
		QueueRedraw();
		return true;
	}

	public override void _Draw()
	{
		if (GodotObject.IsInstanceValid(_definition))
		{
			DrawTargetDummies();
			DrawTrajectories();
			DrawMarkers();
		}
	}

	private void DrawTargetDummies()
	{
		Vector2[] array = new Vector2[3]
		{
			new Vector2(165f, -18f),
			new Vector2(235f, -18f),
			new Vector2(305f, -18f)
		};
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = _definition.randomChoose || (_definition.catapultFirstFar ? (i == array.Length - 1) : (i == 0));
			Color color = (flag ? new Color("ffca67") : new Color("5d7588"));
			DrawCircle(array[i], 17f, new Color(color, 0.2f));
			DrawArc(array[i], 17f, 0f, (float)Math.PI * 2f, 24, color, flag ? 3f : 1.5f);
			DrawLine(array[i] + new Vector2(0f, 17f), array[i] + new Vector2(0f, 37f), color, 2f);
		}
		Font fallbackFont = ThemeDB.FallbackFont;
		XWFireVolleyOverlay xWFireVolleyOverlay = this;
		Font font = fallbackFont;
		Vector2 pos = new Vector2(145f, 40f);
		string text;
		if (_definition.randomChoose)
		{
			text = "随机候选";
		}
		else
		{
			text = (_definition.catapultFirstFar ? "最远目标" : "最近目标");
		}
		xWFireVolleyOverlay.DrawString(font, pos, text, HorizontalAlignment.Left, 180f, 14, new Color("e1c289"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
	}

	private void DrawTrajectories()
	{
		int num = _definition.fireProjectileList?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			FireComponentFireProjectileConfig fireComponentFireProjectileConfig = _definition.fireProjectileList[i];
			if (GodotObject.IsInstanceValid(fireComponentFireProjectileConfig))
			{
				Vector2 vector = ((fireComponentFireProjectileConfig.firePosId >= 0 && fireComponentFireProjectileConfig.firePosId < _markerPositions.Count) ? _markerPositions[fireComponentFireProjectileConfig.firePosId] : Vector2.Zero);
				float num2 = Mathf.Clamp(fireComponentFireProjectileConfig.speed * 0.55f, 70f, 330f);
				Vector2 vector2 = Vector2.FromAngle(Mathf.DegToRad(fireComponentFireProjectileConfig.dir));
				Vector2 vector3 = new Vector2(0f, fireComponentFireProjectileConfig.offsetLine * 56);
				Vector2 vector4 = vector + vector2 * num2 + vector3;
				Color color = Color.FromHsv(((float)i * 0.17f + 0.08f) % 1f, 0.62f, 1f);
				for (int j = 0; j < 12; j += 2)
				{
					float weight = (float)j / 12f;
					float weight2 = (float)(j + 1) / 12f;
					DrawLine(vector.Lerp(vector4, weight), vector.Lerp(vector4, weight2), color, 2.5f);
				}
				Vector2 vector5 = vector2.Orthogonal().Normalized();
				DrawLine(vector4, vector4 - vector2 * 15f + vector5 * 7f, color, 2.5f);
				DrawLine(vector4, vector4 - vector2 * 15f - vector5 * 7f, color, 2.5f);
				DrawCircle(vector.Lerp(vector4, 0.48f), 5f, color);
			}
		}
	}

	private void DrawMarkers()
	{
		Font fallbackFont = ThemeDB.FallbackFont;
		for (int i = 0; i < _markerPositions.Count; i++)
		{
			Vector2 vector = _markerPositions[i];
			Color color = ((i == _dragMarkerIndex) ? new Color("fff19a") : new Color("ff9f43"));
			DrawCircle(vector, 12f, new Color(color, 0.28f));
			DrawArc(vector, 12f, 0f, (float)Math.PI * 2f, 24, color, 3f);
			DrawLine(vector + new Vector2(-17f, 0f), vector + new Vector2(17f, 0f), color, 1f);
			DrawLine(vector + new Vector2(0f, -17f), vector + new Vector2(0f, 17f), color, 1f);
			DrawString(fallbackFont, vector + new Vector2(15f, -10f), $"枪口 {i}", HorizontalAlignment.Left, 70f, 13, color, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildMarkerPositions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindMarker, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindNearestCandidate, new PropertyInfo(Variant.Type.NodePath, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "local", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DropMarkerAt, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "localPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawTargetDummies, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawTrajectories, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawMarkers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Bind && args.Count == 2)
		{
			Bind(VariantUtils.ConvertTo<FireComponentDefinition>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildMarkerPositions && args.Count == 0)
		{
			RebuildMarkerPositions();
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
		if (method == MethodName.FindNearestCandidate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<NodePath>(FindNearestCandidate(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.DropMarkerAt && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(DropMarkerAt(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawTargetDummies && args.Count == 0)
		{
			DrawTargetDummies();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawTrajectories && args.Count == 0)
		{
			DrawTrajectories();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawMarkers && args.Count == 0)
		{
			DrawMarkers();
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
		if (method == MethodName.RebuildMarkerPositions)
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
		if (method == MethodName.FindNearestCandidate)
		{
			return true;
		}
		if (method == MethodName.DropMarkerAt)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.DrawTargetDummies)
		{
			return true;
		}
		if (method == MethodName.DrawTrajectories)
		{
			return true;
		}
		if (method == MethodName.DrawMarkers)
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
			_definition = VariantUtils.ConvertTo<FireComponentDefinition>(in value);
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
		if (name == PropertyName.RedrawRevision)
		{
			from = RedrawRevision;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MarkerHandleCount)
		{
			from = MarkerHandleCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TrajectoryCount)
		{
			from = TrajectoryCount;
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
			new PropertyInfo(Variant.Type.Int, PropertyName.RedrawRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.MarkerHandleCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TrajectoryCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
			_definition = value2.As<FireComponentDefinition>();
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
