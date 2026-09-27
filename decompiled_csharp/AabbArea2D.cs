using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Core/TowerDefenseManager/SubSystem/AabbArea2D.cs")]
public class AabbArea2D : Node2D, IAabbCollisionPreview2D, IAabbAreaQuery2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName DrawAreaShape = "DrawAreaShape";

		public static readonly StringName DrawRectangleShape = "DrawRectangleShape";

		public static readonly StringName DrawCircleShape = "DrawCircleShape";

		public static readonly StringName DrawSegmentShape = "DrawSegmentShape";

		public static readonly StringName ShapePoint = "ShapePoint";

		public static readonly StringName DrawClosedPolyline = "DrawClosedPolyline";

		public static readonly StringName SetEnabled = "SetEnabled";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName SetCollisionPreviewDraw = "SetCollisionPreviewDraw";

		public static readonly StringName DrawResourceRectangle = "DrawResourceRectangle";

		public static readonly StringName DrawResourceSegment = "DrawResourceSegment";

		public static readonly StringName DrawResourceConcave = "DrawResourceConcave";

		public static readonly StringName DrawResourceCapsule = "DrawResourceCapsule";

		public static readonly StringName DrawResourceShape = "DrawResourceShape";

		public static readonly StringName DrawResourceGeometry = "DrawResourceGeometry";

		public static readonly StringName DrawResourceEllipse = "DrawResourceEllipse";

		public static readonly StringName GetResourceGeometryHash = "GetResourceGeometryHash";

		public static readonly StringName DrawResourcePolygon = "DrawResourcePolygon";

		public static readonly StringName DrawFilledAndClosed = "DrawFilledAndClosed";

		public static readonly StringName SetRuntimeRectangleSize = "SetRuntimeRectangleSize";

		public static readonly StringName ClearRuntimeRectangleSize = "ClearRuntimeRectangleSize";

		public static readonly StringName SetRuntimeRectangleWidth = "SetRuntimeRectangleWidth";

		public static readonly StringName SetRuntimeSegmentLengthX = "SetRuntimeSegmentLengthX";

		public static readonly StringName SetRuntimeSegmentEnd = "SetRuntimeSegmentEnd";

		public static readonly StringName ClearRuntimeSegmentEnd = "ClearRuntimeSegmentEnd";

		public static readonly StringName SetRuntimeShapeLocalOrigin = "SetRuntimeShapeLocalOrigin";

		public static readonly StringName SetRuntimeShapeLocalOriginY = "SetRuntimeShapeLocalOriginY";

		public static readonly StringName SetRuntimeShapeLocalTransform = "SetRuntimeShapeLocalTransform";

		public static readonly StringName ClearRuntimeShapeLocalTransform = "ClearRuntimeShapeLocalTransform";

		public static readonly StringName DrawWorldRect = "DrawWorldRect";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName ShapeResources = "ShapeResources";

		public static readonly StringName Enabled = "Enabled";

		public static readonly StringName collision_layer = "collision_layer";

		public static readonly StringName collision_mask = "collision_mask";

		public static readonly StringName monitoring = "monitoring";

		public static readonly StringName monitorable = "monitorable";

		public static readonly StringName input_pickable = "input_pickable";

		public static readonly StringName priority = "priority";

		public static readonly StringName editorDraw = "editorDraw";

		public static readonly StringName editorShapeColor = "editorShapeColor";

		public static readonly StringName editorAabbColor = "editorAabbColor";

		public static readonly StringName editorLineWidth = "editorLineWidth";

		public static readonly StringName CollisionLayer = "CollisionLayer";

		public static readonly StringName CollisionMask = "CollisionMask";

		public static readonly StringName Monitoring = "Monitoring";

		public static readonly StringName Monitorable = "Monitorable";

		public static readonly StringName InputPickable = "InputPickable";

		public static readonly StringName Priority = "Priority";

		public static readonly StringName RegistryOwner = "RegistryOwner";

		public static readonly StringName WorldRect = "WorldRect";

		public static readonly StringName _collisionPreviewDraw = "_collisionPreviewDraw";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private bool _collisionPreviewDraw;

	private System.Collections.Generic.Dictionary<int, Vector2> _runtimeRectangleSizes;

	private System.Collections.Generic.Dictionary<int, Vector2> _runtimeSegmentEnds;

	private System.Collections.Generic.Dictionary<int, Transform2D> _runtimeShapeLocalTransforms;

	[Export(PropertyHint.None, "")]
	public Array<AabbShape2DResource> ShapeResources { get; set; } = new Array<AabbShape2DResource>();

	[Export(PropertyHint.None, "")]
	public bool Enabled { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public uint collision_layer { get; set; } = 1u;

	[Export(PropertyHint.None, "")]
	public uint collision_mask { get; set; } = 1u;

	[Export(PropertyHint.None, "")]
	public bool monitoring { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool monitorable { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool input_pickable { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public float priority { get; set; }

	[ExportGroup("Editor Debug", "")]
	[Export(PropertyHint.None, "")]
	public bool editorDraw { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Color editorShapeColor { get; set; } = new Color(0.1f, 0.75f, 1f, 0.75f);

	[Export(PropertyHint.None, "")]
	public Color editorAabbColor { get; set; } = new Color(1f, 0.7f, 0.1f, 0.65f);

	[Export(PropertyHint.None, "")]
	public float editorLineWidth { get; set; } = 2f;

	public uint CollisionLayer
	{
		get
		{
			return collision_layer;
		}
		set
		{
			collision_layer = value;
		}
	}

	public uint CollisionMask
	{
		get
		{
			return collision_mask;
		}
		set
		{
			collision_mask = value;
		}
	}

	public bool Monitoring
	{
		get
		{
			return monitoring;
		}
		set
		{
			monitoring = value;
		}
	}

	public bool Monitorable
	{
		get
		{
			return monitorable;
		}
		set
		{
			monitorable = value;
		}
	}

	public bool InputPickable
	{
		get
		{
			return input_pickable;
		}
		set
		{
			input_pickable = value;
		}
	}

	public float Priority
	{
		get
		{
			return priority;
		}
		set
		{
			priority = value;
		}
	}

	public Node RegistryOwner => this;

	public Rect2 WorldRect => AabbShapeUtil.ComputeAreaWorldRect(this);

	private void DrawAreaShape(CollisionShape2D collisionShape)
	{
		if (!GodotObject.IsInstanceValid(collisionShape) || collisionShape.Disabled || collisionShape.Shape == null)
		{
			return;
		}
		Shape2D shape = collisionShape.Shape;
		if (!(shape is RectangleShape2D rectangle))
		{
			if (!(shape is CircleShape2D circle))
			{
				if (shape is SegmentShape2D segment)
				{
					DrawSegmentShape(collisionShape, segment);
				}
			}
			else
			{
				DrawCircleShape(collisionShape, circle);
			}
		}
		else
		{
			DrawRectangleShape(collisionShape, rectangle);
		}
	}

	private void DrawRectangleShape(CollisionShape2D collisionShape, RectangleShape2D rectangle)
	{
		Vector2 vector = rectangle.Size * 0.5f;
		DrawClosedPolyline(ShapePoint(collisionShape, new Vector2(0f - vector.X, 0f - vector.Y)), ShapePoint(collisionShape, new Vector2(vector.X, 0f - vector.Y)), ShapePoint(collisionShape, new Vector2(vector.X, vector.Y)), ShapePoint(collisionShape, new Vector2(0f - vector.X, vector.Y)), editorShapeColor);
	}

	private void DrawCircleShape(CollisionShape2D collisionShape, CircleShape2D circle)
	{
		Vector2[] array = new Vector2[33];
		for (int i = 0; i <= 32; i++)
		{
			float angle = (float)Math.PI * 2f * (float)i / 32f;
			array[i] = ShapePoint(collisionShape, Vector2.FromAngle(angle) * circle.Radius);
		}
		DrawPolyline(array, editorShapeColor, editorLineWidth);
	}

	private void DrawSegmentShape(CollisionShape2D collisionShape, SegmentShape2D segment)
	{
		DrawLine(ShapePoint(collisionShape, segment.A), ShapePoint(collisionShape, segment.B), editorShapeColor, editorLineWidth);
	}

	private Vector2 ShapePoint(CollisionShape2D collisionShape, Vector2 point)
	{
		return ToLocal(collisionShape.ToGlobal(point));
	}

	private void DrawClosedPolyline(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color)
	{
		DrawPolyline(new Vector2[5] { a, b, c, d, a }, color, editorLineWidth);
	}

	public void SetEnabled(bool enabled)
	{
		Enabled = enabled;
		QueueRedraw();
	}

	public bool TryGetWorldRect(out Rect2 rect)
	{
		if (!Enabled)
		{
			rect = default;
			return false;
		}
		return AabbShapeUtil.TryComputeAreaWorldRect(this, out rect);
	}

	public override void _Ready()
	{
		SetProcess(Engine.IsEditorHint() || _collisionPreviewDraw);
	}

	public override void _EnterTree()
	{
		AabbAreaLayerRegistry.Register(this);
	}

	public override void _ExitTree()
	{
		AabbAreaLayerRegistry.Unregister(this);
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint() || _collisionPreviewDraw)
		{
			QueueRedraw();
		}
	}

	public override void _Draw()
	{
		if ((!Engine.IsEditorHint() && !_collisionPreviewDraw) || !editorDraw)
		{
			return;
		}
		if (ShapeResources != null && ShapeResources.Count > 0)
		{
			for (int i = 0; i < ShapeResources.Count; i++)
			{
				DrawResourceShape(i, ShapeResources[i]);
			}
		}
		else
		{
			foreach (Node child in GetChildren())
			{
				if (child is CollisionShape2D collisionShape)
				{
					DrawAreaShape(collisionShape);
				}
			}
		}
		DrawWorldRect();
	}

	public void SetCollisionPreviewDraw(bool enabled)
	{
		_collisionPreviewDraw = enabled;
		if (IsInsideTree())
		{
			SetProcess(Engine.IsEditorHint() | enabled);
		}
		QueueRedraw();
	}

	private void DrawResourceRectangle(int shapeIndex, AabbShape2DResource resource, Transform2D localTransform, RectangleShape2D rectangle, float width)
	{
		Vector2 vector = (TryGetRuntimeRectangleSize(shapeIndex, out var size) ? size : rectangle.Size) * 0.5f;
		DrawResourcePolygon(resource, localTransform, new Vector2[4]
		{
			new Vector2(0f - vector.X, 0f - vector.Y),
			new Vector2(vector.X, 0f - vector.Y),
			new Vector2(vector.X, vector.Y),
			new Vector2(0f - vector.X, vector.Y)
		}, width);
	}

	private void DrawResourceSegment(int shapeIndex, AabbShape2DResource resource, Transform2D localTransform, SegmentShape2D segment, float width)
	{
		Vector2 vector = (TryGetRuntimeSegmentEnd(shapeIndex, out var endpoint) ? endpoint : segment.B);
		DrawLine(localTransform * segment.A, localTransform * vector, resource.DebugOutlineColor, width);
	}

	private void DrawResourceConcave(AabbShape2DResource resource, Transform2D localTransform, ConcavePolygonShape2D concave, float width)
	{
		for (int i = 0; i + 1 < concave.Segments.Length; i += 2)
		{
			DrawLine(localTransform * concave.Segments[i], localTransform * concave.Segments[i + 1], resource.DebugOutlineColor, width);
		}
	}

	private void DrawResourceCapsule(AabbShape2DResource resource, Transform2D localTransform, CapsuleShape2D capsule, float width)
	{
		float num = Mathf.Max(0f, capsule.Height * 0.5f - capsule.Radius);
		Vector2[] array = new Vector2[50];
		for (int i = 0; i <= 24; i++)
		{
			float angle = (float)Math.PI + (float)Math.PI * (float)i / 24f;
			array[i] = localTransform * (new Vector2(0f, 0f - num) + Vector2.FromAngle(angle) * capsule.Radius);
		}
		for (int j = 0; j <= 24; j++)
		{
			float angle2 = (float)Math.PI * (float)j / 24f;
			array[25 + j] = localTransform * (new Vector2(0f, num) + Vector2.FromAngle(angle2) * capsule.Radius);
		}
		DrawFilledAndClosed(array, resource.DebugFillColor, resource.DebugOutlineColor, width);
	}

	private void DrawResourceShape(int shapeIndex, AabbShape2DResource resource)
	{
		if (GodotObject.IsInstanceValid(resource) && resource.Enabled && resource.DebugDraw && GodotObject.IsInstanceValid(resource.Geometry))
		{
			float width = Mathf.Max(0.5f, resource.DebugLineWidth);
			if (TryGetRuntimeShapeLocalTransform(shapeIndex, out var localTransform))
			{
				DrawResourceGeometry(shapeIndex, resource, localTransform, width);
			}
		}
	}

	private void DrawResourceGeometry(int shapeIndex, AabbShape2DResource resource, Transform2D localTransform, float width)
	{
		Shape2D geometry = resource.Geometry;
		if (!(geometry is RectangleShape2D rectangle))
		{
			if (!(geometry is CircleShape2D circleShape2D))
			{
				if (!(geometry is SegmentShape2D segment))
				{
					if (!(geometry is CapsuleShape2D capsule))
					{
						if (!(geometry is SeparationRayShape2D separationRayShape2D))
						{
							if (!(geometry is ConvexPolygonShape2D convexPolygonShape2D))
							{
								if (geometry is ConcavePolygonShape2D concave)
								{
									DrawResourceConcave(resource, localTransform, concave, width);
								}
							}
							else if (convexPolygonShape2D.Points.Length > 1)
							{
								DrawResourcePolygon(resource, localTransform, convexPolygonShape2D.Points, width);
							}
						}
						else
						{
							DrawLine(localTransform.Origin, localTransform * new Vector2(0f, separationRayShape2D.Length), resource.DebugOutlineColor, width);
						}
					}
					else
					{
						DrawResourceCapsule(resource, localTransform, capsule, width);
					}
				}
				else
				{
					DrawResourceSegment(shapeIndex, resource, localTransform, segment, width);
				}
			}
			else
			{
				DrawResourceEllipse(resource, localTransform, circleShape2D.Radius, circleShape2D.Radius, width);
			}
		}
		else
		{
			DrawResourceRectangle(shapeIndex, resource, localTransform, rectangle, width);
		}
	}

	private void DrawResourceEllipse(AabbShape2DResource resource, Transform2D localTransform, float radiusX, float radiusY, float width)
	{
		Vector2[] array = new Vector2[48];
		for (int i = 0; i < 48; i++)
		{
			float s = (float)Math.PI * 2f * (float)i / 48f;
			array[i] = localTransform * new Vector2(Mathf.Cos(s) * radiusX, Mathf.Sin(s) * radiusY);
		}
		DrawFilledAndClosed(array, resource.DebugFillColor, resource.DebugOutlineColor, width);
	}

	public bool TryComputeResourceWorldRect(out Rect2 rect)
	{
		rect = default;
		if (!Enabled || ShapeResources == null || ShapeResources.Count == 0)
		{
			return false;
		}
		bool hasRect = false;
		for (int i = 0; i < ShapeResources.Count; i++)
		{
			MergeResourceWorldRect(i, ref rect, ref hasRect);
		}
		return hasRect;
	}

	public int GetResourceGeometryHash()
	{
		if (ShapeResources == null || ShapeResources.Count == 0)
		{
			return 0;
		}
		HashCode hash = default;
		for (int i = 0; i < ShapeResources.Count; i++)
		{
			AddResourceGeometryHash(i, ref hash);
		}
		return hash.ToHashCode();
	}

	private void AddResourceGeometryHash(int index, ref HashCode hash)
	{
		AabbShape2DResource aabbShape2DResource = ShapeResources[index];
		hash.Add(GodotObject.IsInstanceValid(aabbShape2DResource) ? aabbShape2DResource.GetGeometryHash() : 0);
		AddRuntimeSegmentHash(index, ref hash);
		AddRuntimeRectangleHash(index, ref hash);
		AddRuntimeTransformHash(index, ref hash);
	}

	private void AddRuntimeSegmentHash(int index, ref HashCode hash)
	{
		bool flag = TryGetRuntimeSegmentEnd(index, out var endpoint);
		hash.Add(flag);
		if (flag)
		{
			hash.Add(endpoint);
		}
	}

	private void AddRuntimeRectangleHash(int index, ref HashCode hash)
	{
		bool flag = TryGetRuntimeRectangleSize(index, out var size);
		hash.Add(flag);
		if (flag)
		{
			hash.Add(size);
		}
	}

	private void AddRuntimeTransformHash(int index, ref HashCode hash)
	{
		Transform2D value = default;
		bool flag = _runtimeShapeLocalTransforms != null && _runtimeShapeLocalTransforms.TryGetValue(index, out value);
		hash.Add(flag);
		if (flag)
		{
			hash.Add(value);
		}
	}

	private void MergeResourceWorldRect(int index, ref Rect2 rect, ref bool hasRect)
	{
		AabbShape2DResource aabbShape2DResource = ShapeResources[index];
		if (GodotObject.IsInstanceValid(aabbShape2DResource) && TryComputeResourceShapeWorldRect(index, aabbShape2DResource, out var shapeRect))
		{
			rect = (hasRect ? AabbShapeUtil.Union(rect, shapeRect) : shapeRect);
			hasRect = true;
		}
	}

	private bool TryComputeResourceShapeWorldRect(int index, AabbShape2DResource resource, out Rect2 shapeRect)
	{
		shapeRect = default;
		if (!resource.Enabled || !GodotObject.IsInstanceValid(resource.Geometry) || !TryGetRuntimeShapeLocalTransform(index, out var localTransform))
		{
			return false;
		}
		Transform2D worldTransform = GlobalTransform * localTransform;
		return TryComputeResourceGeometryWorldRect(index, resource, worldTransform, out shapeRect);
	}

	private bool TryComputeResourceGeometryWorldRect(int index, AabbShape2DResource resource, Transform2D worldTransform, out Rect2 shapeRect)
	{
		if (resource.Geometry is RectangleShape2D && TryGetRuntimeRectangleSize(index, out var size))
		{
			shapeRect = AabbShapeUtil.ComputeRectangleWorldRect(worldTransform, size);
			return true;
		}
		if (resource.Geometry is SegmentShape2D segmentShape2D && TryGetRuntimeSegmentEnd(index, out var endpoint))
		{
			return AabbShapeUtil.TryComputeSegmentWorldRect(worldTransform, segmentShape2D.A, endpoint, out shapeRect);
		}
		return AabbShapeUtil.TryComputeShapeWorldRect(resource.Geometry, worldTransform, out shapeRect);
	}

	private void DrawResourcePolygon(AabbShape2DResource resource, Transform2D localTransform, Vector2[] source, float width)
	{
		Vector2[] array = new Vector2[source.Length];
		for (int i = 0; i < source.Length; i++)
		{
			array[i] = localTransform * source[i];
		}
		DrawFilledAndClosed(array, resource.DebugFillColor, resource.DebugOutlineColor, width);
	}

	private void DrawFilledAndClosed(Vector2[] points, Color fill, Color outline, float width)
	{
		if (points.Length >= 2)
		{
			if (points.Length >= 3 && fill.A > 0f)
			{
				DrawColoredPolygon(points, fill);
			}
			Vector2[] array = new Vector2[points.Length + 1];
			System.Array.Copy(points, array, points.Length);
			array[^1] = points[0];
			DrawPolyline(array, outline, width, antialiased: true);
		}
	}

	public bool SetRuntimeRectangleSize(int shapeIndex, Vector2 size)
	{
		if (!float.IsFinite(size.X) || !float.IsFinite(size.Y) || size.X < 0f || size.Y < 0f || !TryGetRectangleResource(shapeIndex, out var _))
		{
			return false;
		}
		if (_runtimeRectangleSizes == null)
		{
			_runtimeRectangleSizes = new System.Collections.Generic.Dictionary<int, Vector2>();
		}
		_runtimeRectangleSizes[shapeIndex] = size;
		QueueRedraw();
		return true;
	}

	public bool TryGetRuntimeRectangleSize(int shapeIndex, out Vector2 size)
	{
		if (_runtimeRectangleSizes != null && _runtimeRectangleSizes.TryGetValue(shapeIndex, out size))
		{
			return true;
		}
		size = default;
		return false;
	}

	public bool ClearRuntimeRectangleSize(int shapeIndex)
	{
		if (_runtimeRectangleSizes == null || !_runtimeRectangleSizes.Remove(shapeIndex))
		{
			return false;
		}
		if (_runtimeRectangleSizes.Count == 0)
		{
			_runtimeRectangleSizes = null;
		}
		QueueRedraw();
		return true;
	}

	public bool SetRuntimeRectangleWidth(int shapeIndex, float width)
	{
		if (!float.IsFinite(width) || width < 0f || !TryGetEffectiveRectangleSize(shapeIndex, out var size))
		{
			return false;
		}
		size.X = width;
		return SetRuntimeRectangleSize(shapeIndex, size);
	}

	private bool TryGetEffectiveRectangleSize(int shapeIndex, out Vector2 size)
	{
		if (TryGetRuntimeRectangleSize(shapeIndex, out size))
		{
			return true;
		}
		if (TryGetRectangleResource(shapeIndex, out var rectangle))
		{
			size = rectangle.Size;
			return true;
		}
		size = default;
		return false;
	}

	public bool SetRuntimeSegmentLengthX(int shapeIndex, float length)
	{
		if (!TryGetSegmentResource(shapeIndex, out var segment))
		{
			return false;
		}
		return SetRuntimeSegmentEnd(shapeIndex, new Vector2((float)Mathf.Sign(segment.B.X) * Mathf.Max(0f, length), segment.B.Y));
	}

	public bool SetRuntimeSegmentEnd(int shapeIndex, Vector2 endpoint)
	{
		if (!TryGetSegmentResource(shapeIndex, out var _))
		{
			return false;
		}
		if (_runtimeSegmentEnds == null)
		{
			_runtimeSegmentEnds = new System.Collections.Generic.Dictionary<int, Vector2>();
		}
		_runtimeSegmentEnds[shapeIndex] = endpoint;
		QueueRedraw();
		return true;
	}

	public bool TryGetRuntimeSegmentEnd(int shapeIndex, out Vector2 endpoint)
	{
		if (_runtimeSegmentEnds != null && _runtimeSegmentEnds.TryGetValue(shapeIndex, out endpoint))
		{
			return true;
		}
		endpoint = default;
		return false;
	}

	public bool ClearRuntimeSegmentEnd(int shapeIndex)
	{
		if (_runtimeSegmentEnds == null || !_runtimeSegmentEnds.Remove(shapeIndex))
		{
			return false;
		}
		if (_runtimeSegmentEnds.Count == 0)
		{
			_runtimeSegmentEnds = null;
		}
		QueueRedraw();
		return true;
	}

	public bool SetRuntimeShapeLocalOrigin(int shapeIndex, Vector2 localOrigin)
	{
		if (!TryGetRuntimeShapeLocalTransform(shapeIndex, out var localTransform))
		{
			return false;
		}
		localTransform.Origin = localOrigin;
		return SetRuntimeShapeLocalTransform(shapeIndex, localTransform);
	}

	public bool SetRuntimeShapeLocalOriginY(int shapeIndex, float localY)
	{
		if (!float.IsFinite(localY) || !TryGetRuntimeShapeLocalTransform(shapeIndex, out var localTransform))
		{
			return false;
		}
		localTransform.Origin = new Vector2(localTransform.Origin.X, localY);
		return SetRuntimeShapeLocalTransform(shapeIndex, localTransform);
	}

	public bool SetRuntimeShapeLocalTransform(int shapeIndex, Transform2D localTransform)
	{
		if (!TryGetShapeResource(shapeIndex, out var _))
		{
			return false;
		}
		if (_runtimeShapeLocalTransforms == null)
		{
			_runtimeShapeLocalTransforms = new System.Collections.Generic.Dictionary<int, Transform2D>();
		}
		_runtimeShapeLocalTransforms[shapeIndex] = localTransform;
		QueueRedraw();
		return true;
	}

	public bool TryGetRuntimeShapeLocalTransform(int shapeIndex, out Transform2D localTransform)
	{
		if (!TryGetShapeResource(shapeIndex, out var resource))
		{
			localTransform = Transform2D.Identity;
			return false;
		}
		if (_runtimeShapeLocalTransforms != null && _runtimeShapeLocalTransforms.TryGetValue(shapeIndex, out localTransform))
		{
			return true;
		}
		localTransform = resource.LocalTransform;
		return true;
	}

	public bool ClearRuntimeShapeLocalTransform(int shapeIndex)
	{
		if (_runtimeShapeLocalTransforms == null || !_runtimeShapeLocalTransforms.Remove(shapeIndex))
		{
			return false;
		}
		if (_runtimeShapeLocalTransforms.Count == 0)
		{
			_runtimeShapeLocalTransforms = null;
		}
		QueueRedraw();
		return true;
	}

	private bool TryGetShapeResource(int shapeIndex, out AabbShape2DResource resource)
	{
		resource = null;
		if (ShapeResources == null || shapeIndex < 0 || shapeIndex >= ShapeResources.Count)
		{
			return false;
		}
		resource = ShapeResources[shapeIndex];
		if (GodotObject.IsInstanceValid(resource))
		{
			return GodotObject.IsInstanceValid(resource.Geometry);
		}
		return false;
	}

	private bool TryGetRectangleResource(int shapeIndex, out RectangleShape2D rectangle)
	{
		rectangle = null;
		if (!TryGetShapeResource(shapeIndex, out var resource) || !(resource.Geometry is RectangleShape2D rectangleShape2D))
		{
			return false;
		}
		rectangle = rectangleShape2D;
		return true;
	}

	private bool TryGetSegmentResource(int shapeIndex, out SegmentShape2D segment)
	{
		segment = null;
		if (!TryGetShapeResource(shapeIndex, out var resource) || !(resource.Geometry is SegmentShape2D segmentShape2D))
		{
			return false;
		}
		segment = segmentShape2D;
		return true;
	}

	private void DrawWorldRect()
	{
		if (!AabbShapeUtil.TryComputeAreaWorldRect(this, out var rect))
		{
			rect = WorldRect;
		}
		if (!(rect.Size == Vector2.Zero))
		{
			Vector2 position = rect.Position;
			Vector2 vector = rect.Position + rect.Size;
			DrawClosedPolyline(ToLocal(new Vector2(position.X, position.Y)), ToLocal(new Vector2(vector.X, position.Y)), ToLocal(new Vector2(vector.X, vector.Y)), ToLocal(new Vector2(position.X, vector.Y)), editorAabbColor);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(34)
		{
			new MethodInfo(MethodName.DrawAreaShape, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collisionShape", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CollisionShape2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawRectangleShape, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collisionShape", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CollisionShape2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "rectangle", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RectangleShape2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawCircleShape, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collisionShape", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CollisionShape2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "circle", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CircleShape2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawSegmentShape, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collisionShape", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CollisionShape2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "segment", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SegmentShape2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShapePoint, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collisionShape", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CollisionShape2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "point", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawClosedPolyline, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "a", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "b", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "c", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "d", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetEnabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCollisionPreviewDraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawResourceRectangle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "localTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "rectangle", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RectangleShape2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawResourceSegment, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "localTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "segment", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SegmentShape2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawResourceConcave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "localTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "concave", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ConcavePolygonShape2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawResourceCapsule, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "localTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "capsule", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CapsuleShape2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawResourceShape, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawResourceGeometry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "localTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawResourceEllipse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "localTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "radiusX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "radiusY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetResourceGeometryHash, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawResourcePolygon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "localTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedVector2Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawFilledAndClosed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedVector2Array, "points", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "fill", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "outline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeRectangleSize, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearRuntimeRectangleSize, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeRectangleWidth, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeSegmentLengthX, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "length", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeSegmentEnd, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "endpoint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearRuntimeSegmentEnd, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeShapeLocalOrigin, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "localOrigin", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeShapeLocalOriginY, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "localY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeShapeLocalTransform, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "localTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearRuntimeShapeLocalTransform, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "shapeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawWorldRect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.DrawAreaShape && args.Count == 1)
		{
			DrawAreaShape(VariantUtils.ConvertTo<CollisionShape2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawRectangleShape && args.Count == 2)
		{
			DrawRectangleShape(VariantUtils.ConvertTo<CollisionShape2D>(in args[0]), VariantUtils.ConvertTo<RectangleShape2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawCircleShape && args.Count == 2)
		{
			DrawCircleShape(VariantUtils.ConvertTo<CollisionShape2D>(in args[0]), VariantUtils.ConvertTo<CircleShape2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawSegmentShape && args.Count == 2)
		{
			DrawSegmentShape(VariantUtils.ConvertTo<CollisionShape2D>(in args[0]), VariantUtils.ConvertTo<SegmentShape2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShapePoint && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ShapePoint(VariantUtils.ConvertTo<CollisionShape2D>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.DrawClosedPolyline && args.Count == 5)
		{
			DrawClosedPolyline(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<Color>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetEnabled && args.Count == 1)
		{
			SetEnabled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
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
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCollisionPreviewDraw && args.Count == 1)
		{
			SetCollisionPreviewDraw(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawResourceRectangle && args.Count == 5)
		{
			DrawResourceRectangle(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<AabbShape2DResource>(in args[1]), VariantUtils.ConvertTo<Transform2D>(in args[2]), VariantUtils.ConvertTo<RectangleShape2D>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawResourceSegment && args.Count == 5)
		{
			DrawResourceSegment(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<AabbShape2DResource>(in args[1]), VariantUtils.ConvertTo<Transform2D>(in args[2]), VariantUtils.ConvertTo<SegmentShape2D>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawResourceConcave && args.Count == 4)
		{
			DrawResourceConcave(VariantUtils.ConvertTo<AabbShape2DResource>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1]), VariantUtils.ConvertTo<ConcavePolygonShape2D>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawResourceCapsule && args.Count == 4)
		{
			DrawResourceCapsule(VariantUtils.ConvertTo<AabbShape2DResource>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1]), VariantUtils.ConvertTo<CapsuleShape2D>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawResourceShape && args.Count == 2)
		{
			DrawResourceShape(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<AabbShape2DResource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawResourceGeometry && args.Count == 4)
		{
			DrawResourceGeometry(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<AabbShape2DResource>(in args[1]), VariantUtils.ConvertTo<Transform2D>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawResourceEllipse && args.Count == 5)
		{
			DrawResourceEllipse(VariantUtils.ConvertTo<AabbShape2DResource>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetResourceGeometryHash && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetResourceGeometryHash());
			return true;
		}
		if (method == MethodName.DrawResourcePolygon && args.Count == 4)
		{
			DrawResourcePolygon(VariantUtils.ConvertTo<AabbShape2DResource>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1]), VariantUtils.ConvertTo<Vector2[]>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawFilledAndClosed && args.Count == 4)
		{
			DrawFilledAndClosed(VariantUtils.ConvertTo<Vector2[]>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRuntimeRectangleSize && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetRuntimeRectangleSize(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.ClearRuntimeRectangleSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ClearRuntimeRectangleSize(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetRuntimeRectangleWidth && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetRuntimeRectangleWidth(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.SetRuntimeSegmentLengthX && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetRuntimeSegmentLengthX(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.SetRuntimeSegmentEnd && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetRuntimeSegmentEnd(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.ClearRuntimeSegmentEnd && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ClearRuntimeSegmentEnd(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetRuntimeShapeLocalOrigin && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetRuntimeShapeLocalOrigin(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.SetRuntimeShapeLocalOriginY && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetRuntimeShapeLocalOriginY(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.SetRuntimeShapeLocalTransform && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetRuntimeShapeLocalTransform(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1])));
			return true;
		}
		if (method == MethodName.ClearRuntimeShapeLocalTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ClearRuntimeShapeLocalTransform(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.DrawWorldRect && args.Count == 0)
		{
			DrawWorldRect();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.DrawAreaShape)
		{
			return true;
		}
		if (method == MethodName.DrawRectangleShape)
		{
			return true;
		}
		if (method == MethodName.DrawCircleShape)
		{
			return true;
		}
		if (method == MethodName.DrawSegmentShape)
		{
			return true;
		}
		if (method == MethodName.ShapePoint)
		{
			return true;
		}
		if (method == MethodName.DrawClosedPolyline)
		{
			return true;
		}
		if (method == MethodName.SetEnabled)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._EnterTree)
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
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.SetCollisionPreviewDraw)
		{
			return true;
		}
		if (method == MethodName.DrawResourceRectangle)
		{
			return true;
		}
		if (method == MethodName.DrawResourceSegment)
		{
			return true;
		}
		if (method == MethodName.DrawResourceConcave)
		{
			return true;
		}
		if (method == MethodName.DrawResourceCapsule)
		{
			return true;
		}
		if (method == MethodName.DrawResourceShape)
		{
			return true;
		}
		if (method == MethodName.DrawResourceGeometry)
		{
			return true;
		}
		if (method == MethodName.DrawResourceEllipse)
		{
			return true;
		}
		if (method == MethodName.GetResourceGeometryHash)
		{
			return true;
		}
		if (method == MethodName.DrawResourcePolygon)
		{
			return true;
		}
		if (method == MethodName.DrawFilledAndClosed)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeRectangleSize)
		{
			return true;
		}
		if (method == MethodName.ClearRuntimeRectangleSize)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeRectangleWidth)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeSegmentLengthX)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeSegmentEnd)
		{
			return true;
		}
		if (method == MethodName.ClearRuntimeSegmentEnd)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeShapeLocalOrigin)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeShapeLocalOriginY)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeShapeLocalTransform)
		{
			return true;
		}
		if (method == MethodName.ClearRuntimeShapeLocalTransform)
		{
			return true;
		}
		if (method == MethodName.DrawWorldRect)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ShapeResources)
		{
			ShapeResources = VariantUtils.ConvertToArray<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.Enabled)
		{
			Enabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.collision_layer)
		{
			collision_layer = VariantUtils.ConvertTo<uint>(in value);
			return true;
		}
		if (name == PropertyName.collision_mask)
		{
			collision_mask = VariantUtils.ConvertTo<uint>(in value);
			return true;
		}
		if (name == PropertyName.monitoring)
		{
			monitoring = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.monitorable)
		{
			monitorable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.input_pickable)
		{
			input_pickable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.priority)
		{
			priority = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.editorDraw)
		{
			editorDraw = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.editorShapeColor)
		{
			editorShapeColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.editorAabbColor)
		{
			editorAabbColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.editorLineWidth)
		{
			editorLineWidth = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.CollisionLayer)
		{
			CollisionLayer = VariantUtils.ConvertTo<uint>(in value);
			return true;
		}
		if (name == PropertyName.CollisionMask)
		{
			CollisionMask = VariantUtils.ConvertTo<uint>(in value);
			return true;
		}
		if (name == PropertyName.Monitoring)
		{
			Monitoring = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Monitorable)
		{
			Monitorable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.InputPickable)
		{
			InputPickable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Priority)
		{
			Priority = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._collisionPreviewDraw)
		{
			_collisionPreviewDraw = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ShapeResources)
		{
			value = VariantUtils.CreateFromArray(ShapeResources);
			return true;
		}
		bool from;
		if (name == PropertyName.Enabled)
		{
			from = Enabled;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		uint from2;
		if (name == PropertyName.collision_layer)
		{
			from2 = collision_layer;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.collision_mask)
		{
			from2 = collision_mask;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.monitoring)
		{
			from = monitoring;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.monitorable)
		{
			from = monitorable;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.input_pickable)
		{
			from = input_pickable;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		float from3;
		if (name == PropertyName.priority)
		{
			from3 = priority;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.editorDraw)
		{
			from = editorDraw;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Color from4;
		if (name == PropertyName.editorShapeColor)
		{
			from4 = editorShapeColor;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.editorAabbColor)
		{
			from4 = editorAabbColor;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.editorLineWidth)
		{
			from3 = editorLineWidth;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CollisionLayer)
		{
			from2 = CollisionLayer;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CollisionMask)
		{
			from2 = CollisionMask;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.Monitoring)
		{
			from = Monitoring;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Monitorable)
		{
			from = Monitorable;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.InputPickable)
		{
			from = InputPickable;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Priority)
		{
			from3 = Priority;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.RegistryOwner)
		{
			value = VariantUtils.CreateFrom<Node>(RegistryOwner);
			return true;
		}
		if (name == PropertyName.WorldRect)
		{
			value = VariantUtils.CreateFrom<Rect2>(WorldRect);
			return true;
		}
		if (name == PropertyName._collisionPreviewDraw)
		{
			value = VariantUtils.CreateFrom(in _collisionPreviewDraw);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._collisionPreviewDraw, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.ShapeResources, PropertyHint.TypeString, "24/17:AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Enabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collision_layer, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collision_mask, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.monitoring, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.monitorable, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.input_pickable, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.priority, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Editor Debug", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.editorDraw, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.editorShapeColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.editorAabbColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.editorLineWidth, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionMask, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Monitoring, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Monitorable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.InputPickable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.Priority, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.RegistryOwner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.WorldRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ShapeResources, Variant.CreateFrom(ShapeResources));
		info.AddProperty(PropertyName.Enabled, Variant.From<bool>(Enabled));
		info.AddProperty(PropertyName.collision_layer, Variant.From<uint>(collision_layer));
		info.AddProperty(PropertyName.collision_mask, Variant.From<uint>(collision_mask));
		info.AddProperty(PropertyName.monitoring, Variant.From<bool>(monitoring));
		info.AddProperty(PropertyName.monitorable, Variant.From<bool>(monitorable));
		info.AddProperty(PropertyName.input_pickable, Variant.From<bool>(input_pickable));
		info.AddProperty(PropertyName.priority, Variant.From<float>(priority));
		info.AddProperty(PropertyName.editorDraw, Variant.From<bool>(editorDraw));
		info.AddProperty(PropertyName.editorShapeColor, Variant.From<Color>(editorShapeColor));
		info.AddProperty(PropertyName.editorAabbColor, Variant.From<Color>(editorAabbColor));
		info.AddProperty(PropertyName.editorLineWidth, Variant.From<float>(editorLineWidth));
		info.AddProperty(PropertyName.CollisionLayer, Variant.From<uint>(CollisionLayer));
		info.AddProperty(PropertyName.CollisionMask, Variant.From<uint>(CollisionMask));
		info.AddProperty(PropertyName.Monitoring, Variant.From<bool>(Monitoring));
		info.AddProperty(PropertyName.Monitorable, Variant.From<bool>(Monitorable));
		info.AddProperty(PropertyName.InputPickable, Variant.From<bool>(InputPickable));
		info.AddProperty(PropertyName.Priority, Variant.From<float>(Priority));
		info.AddProperty(PropertyName._collisionPreviewDraw, Variant.From(in _collisionPreviewDraw));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ShapeResources, out var value))
		{
			ShapeResources = value.AsGodotArray<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.Enabled, out var value2))
		{
			Enabled = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.collision_layer, out var value3))
		{
			collision_layer = value3.As<uint>();
		}
		if (info.TryGetProperty(PropertyName.collision_mask, out var value4))
		{
			collision_mask = value4.As<uint>();
		}
		if (info.TryGetProperty(PropertyName.monitoring, out var value5))
		{
			monitoring = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.monitorable, out var value6))
		{
			monitorable = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.input_pickable, out var value7))
		{
			input_pickable = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.priority, out var value8))
		{
			priority = value8.As<float>();
		}
		if (info.TryGetProperty(PropertyName.editorDraw, out var value9))
		{
			editorDraw = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.editorShapeColor, out var value10))
		{
			editorShapeColor = value10.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.editorAabbColor, out var value11))
		{
			editorAabbColor = value11.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.editorLineWidth, out var value12))
		{
			editorLineWidth = value12.As<float>();
		}
		if (info.TryGetProperty(PropertyName.CollisionLayer, out var value13))
		{
			CollisionLayer = value13.As<uint>();
		}
		if (info.TryGetProperty(PropertyName.CollisionMask, out var value14))
		{
			CollisionMask = value14.As<uint>();
		}
		if (info.TryGetProperty(PropertyName.Monitoring, out var value15))
		{
			Monitoring = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Monitorable, out var value16))
		{
			Monitorable = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.InputPickable, out var value17))
		{
			InputPickable = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Priority, out var value18))
		{
			Priority = value18.As<float>();
		}
		if (info.TryGetProperty(PropertyName._collisionPreviewDraw, out var value19))
		{
			_collisionPreviewDraw = value19.As<bool>();
		}
	}
}
