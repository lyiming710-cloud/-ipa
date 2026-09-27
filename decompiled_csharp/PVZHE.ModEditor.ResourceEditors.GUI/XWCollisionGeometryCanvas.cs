using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCollisionGeometryCanvas.cs")]
public class XWCollisionGeometryCanvas : Control
{
	public enum GeometryKind
	{
		HitBox,
		Shape,
		Ray
	}

	public new class MethodName : Control.MethodName
	{
		public static readonly StringName DisplayHitBox = "DisplayHitBox";

		public static readonly StringName DisplayShape = "DisplayShape";

		public static readonly StringName DisplayRay = "DisplayRay";

		public new static readonly StringName _Draw = "_Draw";

		public new static readonly StringName _GuiInput = "_GuiInput";

		public static readonly StringName DrawBackground = "DrawBackground";

		public static readonly StringName DrawGeometry = "DrawGeometry";

		public static readonly StringName GetShapeSize = "GetShapeSize";

		public static readonly StringName DrawHandle = "DrawHandle";

		public static readonly StringName GetOriginHandle = "GetOriginHandle";

		public static readonly StringName GetGeometryHandle = "GetGeometryHandle";

		public static readonly StringName ResetPreview = "ResetPreview";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _kind = "_kind";

		public static readonly StringName _transform = "_transform";

		public static readonly StringName _geometrySize = "_geometrySize";

		public static readonly StringName _rayTarget = "_rayTarget";

		public static readonly StringName _shape = "_shape";

		public static readonly StringName _fill = "_fill";

		public static readonly StringName _outline = "_outline";

		public static readonly StringName _lineWidth = "_lineWidth";

		public static readonly StringName _enabled = "_enabled";

		public static readonly StringName _dragOrigin = "_dragOrigin";

		public static readonly StringName _dragGeometry = "_dragGeometry";

		public static readonly StringName _previewOrigin = "_previewOrigin";

		public static readonly StringName _previewGeometry = "_previewGeometry";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const float Zoom = 1f;

	private const float HandleRadius = 8f;

	private GeometryKind _kind;

	private Transform2D _transform = Transform2D.Identity;

	private Vector2 _geometrySize = new Vector2(80f, 80f);

	private Vector2 _rayTarget = new Vector2(200f, 0f);

	private Shape2D _shape;

	private Color _fill = new Color(0.1f, 0.75f, 1f, 0.2f);

	private Color _outline = new Color(0.2f, 0.9f, 1f, 0.95f);

	private float _lineWidth = 2f;

	private bool _enabled = true;

	private bool _dragOrigin;

	private bool _dragGeometry;

	private Vector2 _previewOrigin;

	private Vector2 _previewGeometry;

	public Action<Vector2> OriginCommitted { get; set; }

	public Action<Vector2> GeometryCommitted { get; set; }

	public XWCollisionGeometryCanvas()
	{
		CustomMinimumSize = new Vector2(380f, 330f);
		MouseDefaultCursorShape = CursorShape.Cross;
		FocusMode = FocusModeEnum.Click;
		ClipContents = true;
	}

	public void DisplayHitBox(CharacterHitBoxDefinition resource)
	{
		_kind = GeometryKind.HitBox;
		_transform = resource?.LocalTransform ?? Transform2D.Identity;
		_geometrySize = resource?.Size ?? new Vector2(80f, 80f);
		_shape = null;
		_enabled = resource?.DefaultEnabled ?? false;
		_fill = resource?.DebugFillColor ?? new Color(0.1f, 0.75f, 1f, 0.2f);
		_outline = resource?.DebugOutlineColor ?? new Color(0.2f, 0.9f, 1f, 0.95f);
		_lineWidth = resource?.DebugLineWidth ?? 2f;
		ResetPreview();
	}

	public void DisplayShape(AabbShape2DResource resource)
	{
		_kind = GeometryKind.Shape;
		_transform = resource?.LocalTransform ?? Transform2D.Identity;
		_shape = resource?.Geometry;
		_geometrySize = GetShapeSize(_shape);
		_enabled = resource?.Enabled ?? false;
		_fill = resource?.DebugFillColor ?? new Color(0.1f, 0.75f, 1f, 0.2f);
		_outline = resource?.DebugOutlineColor ?? new Color(0.2f, 0.9f, 1f, 0.95f);
		_lineWidth = resource?.DebugLineWidth ?? 2f;
		ResetPreview();
	}

	public void DisplayRay(AabbRay2DResource resource)
	{
		_kind = GeometryKind.Ray;
		_transform = resource?.LocalTransform ?? Transform2D.Identity;
		_rayTarget = resource?.TargetPosition ?? new Vector2(200f, 0f);
		_enabled = resource?.Enabled ?? false;
		_fill = resource?.DebugColor ?? new Color(0.2f, 1f, 0.25f, 0.8f);
		_outline = _fill;
		_lineWidth = resource?.DebugLineWidth ?? 2f;
		_shape = null;
		ResetPreview();
	}

	public override void _Draw()
	{
		DrawBackground();
		Vector2 vector = Size * 0.5f;
		Transform2D transform = _transform;
		transform.Origin = _previewOrigin;
		Color fill = (_enabled ? _fill : new Color(0.55f, 0.24f, 0.2f, 0.16f));
		Color color = (_enabled ? _outline : new Color(0.95f, 0.36f, 0.28f, 0.85f));
		if (_kind == GeometryKind.Ray)
		{
			Vector2 vector2 = vector + transform.Origin * 1f;
			Vector2 vector3 = vector + transform * _previewGeometry * 1f;
			DrawLine(vector2, vector3, color, Mathf.Max(1f, _lineWidth), antialiased: true);
			Vector2 vector4 = (vector3 - vector2).Normalized();
			Vector2 vector5 = new Vector2(0f - vector4.Y, vector4.X);
			DrawColoredPolygon(new Vector2[3]
			{
				vector3,
				vector3 - vector4 * 16f + vector5 * 7f,
				vector3 - vector4 * 16f - vector5 * 7f
			}, color);
			DrawHandle(vector2, new Color(1f, 0.68f, 0.18f));
			DrawHandle(vector3, new Color(0.2f, 0.92f, 1f));
		}
		else
		{
			Vector2 position = vector + transform.Origin * 1f;
			DrawSetTransform(position, transform.Rotation, transform.Scale * 1f);
			DrawGeometry(_previewGeometry, fill, color);
			DrawSetTransform(Vector2.Zero, 0f, Vector2.One);
			DrawHandle(position, new Color(1f, 0.68f, 0.18f));
			Vector2 position2 = vector + transform * (_previewGeometry * 0.5f) * 1f;
			DrawHandle(position2, new Color(0.2f, 0.92f, 1f));
		}
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			if (inputEventMouseButton.Pressed)
			{
				Vector2 originHandle = GetOriginHandle();
				Vector2 geometryHandle = GetGeometryHandle();
				_dragOrigin = inputEventMouseButton.Position.DistanceTo(originHandle) <= 13f;
				_dragGeometry = !_dragOrigin && inputEventMouseButton.Position.DistanceTo(geometryHandle) <= 16f;
				if (_dragOrigin || _dragGeometry)
				{
					AcceptEvent();
				}
			}
			else if (_dragOrigin || _dragGeometry)
			{
				if (_dragOrigin)
				{
					OriginCommitted?.Invoke(_previewOrigin);
				}
				else
				{
					GeometryCommitted?.Invoke(_previewGeometry);
				}
				_dragOrigin = false;
				_dragGeometry = false;
				AcceptEvent();
			}
		}
		else if (@event is InputEventMouseMotion inputEventMouseMotion && (_dragOrigin || _dragGeometry))
		{
			Vector2 vector = (inputEventMouseMotion.Position - Size * 0.5f) / 1f;
			if (_dragOrigin)
			{
				_previewOrigin = vector;
			}
			else
			{
				Transform2D transform = _transform;
				transform.Origin = _previewOrigin;
				Vector2 vector2 = transform.AffineInverse() * vector;
				_previewGeometry = ((_kind == GeometryKind.Ray) ? vector2 : new Vector2(Mathf.Max(1f, Mathf.Abs(vector2.X) * 2f), Mathf.Max(1f, Mathf.Abs(vector2.Y) * 2f)));
			}
			QueueRedraw();
			AcceptEvent();
		}
	}

	private void DrawBackground()
	{
		DrawRect(new Rect2(Vector2.Zero, Size), new Color(0.025f, 0.035f, 0.028f));
		Vector2 vector = Size * 0.5f;
		for (float num = vector.X % 32f; num < Size.X; num += 32f)
		{
			DrawLine(new Vector2(num, 0f), new Vector2(num, Size.Y), new Color(0.18f, 0.24f, 0.18f, 0.55f));
		}
		for (float num2 = vector.Y % 32f; num2 < Size.Y; num2 += 32f)
		{
			DrawLine(new Vector2(0f, num2), new Vector2(Size.X, num2), new Color(0.18f, 0.24f, 0.18f, 0.55f));
		}
		DrawLine(new Vector2(0f, vector.Y), new Vector2(Size.X, vector.Y), new Color(0.62f, 0.32f, 0.24f, 0.7f), 2f);
		DrawLine(new Vector2(vector.X, 0f), new Vector2(vector.X, Size.Y), new Color(0.3f, 0.62f, 0.3f, 0.7f), 2f);
	}

	private void DrawGeometry(Vector2 size, Color fill, Color outline)
	{
		if (_kind == GeometryKind.HitBox || _shape is RectangleShape2D || _shape == null)
		{
			Rect2 rect = new Rect2(-size * 0.5f, size);
			DrawRect(rect, fill);
			DrawRect(rect, outline, filled: false, Mathf.Max(1f, _lineWidth));
		}
		else if (_shape is CircleShape2D)
		{
			float radius = Mathf.Max(0.5f, size.X * 0.5f);
			DrawCircle(Vector2.Zero, radius, fill);
			DrawArc(Vector2.Zero, radius, 0f, (float)Math.PI * 2f, 48, outline, Mathf.Max(1f, _lineWidth), antialiased: true);
		}
		else if (_shape is CapsuleShape2D)
		{
			float num = Mathf.Max(0.5f, size.X * 0.5f);
			float num2 = Mathf.Max(0f, size.Y - num * 2f);
			DrawRect(new Rect2(0f - num, (0f - num2) * 0.5f, num * 2f, num2), fill);
			DrawCircle(new Vector2(0f, (0f - num2) * 0.5f), num, fill);
			DrawCircle(new Vector2(0f, num2 * 0.5f), num, fill);
			DrawArc(new Vector2(0f, (0f - num2) * 0.5f), num, (float)Math.PI, (float)Math.PI * 2f, 24, outline, _lineWidth, antialiased: true);
			DrawArc(new Vector2(0f, num2 * 0.5f), num, 0f, (float)Math.PI, 24, outline, _lineWidth, antialiased: true);
			DrawLine(new Vector2(0f - num, (0f - num2) * 0.5f), new Vector2(0f - num, num2 * 0.5f), outline, _lineWidth);
			DrawLine(new Vector2(num, (0f - num2) * 0.5f), new Vector2(num, num2 * 0.5f), outline, _lineWidth);
		}
		else
		{
			Rect2 rect2 = new Rect2(-size * 0.5f, size);
			DrawRect(rect2, fill);
			DrawRect(rect2, outline, filled: false, Mathf.Max(1f, _lineWidth));
		}
	}

	private static Vector2 GetShapeSize(Shape2D shape)
	{
		if (!(shape is RectangleShape2D { Size: var size }))
		{
			if (!(shape is CircleShape2D circleShape2D))
			{
				if (!(shape is CapsuleShape2D capsuleShape2D))
				{
					if (shape is SegmentShape2D segmentShape2D)
					{
						return (segmentShape2D.B - segmentShape2D.A).Abs();
					}
					return new Vector2(80f, 80f);
				}
				return new Vector2(capsuleShape2D.Radius * 2f, capsuleShape2D.Height);
			}
			return Vector2.One * circleShape2D.Radius * 2f;
		}
		return size;
	}

	private void DrawHandle(Vector2 position, Color color)
	{
		DrawCircle(position, 10f, new Color(0f, 0f, 0f, 0.7f));
		DrawCircle(position, 8f, color);
	}

	private Vector2 GetOriginHandle()
	{
		return Size * 0.5f + _previewOrigin * 1f;
	}

	private Vector2 GetGeometryHandle()
	{
		Transform2D transform = _transform;
		transform.Origin = _previewOrigin;
		return Size * 0.5f + transform * ((_kind == GeometryKind.Ray) ? _previewGeometry : (_previewGeometry * 0.5f)) * 1f;
	}

	private void ResetPreview()
	{
		_previewOrigin = _transform.Origin;
		_previewGeometry = ((_kind == GeometryKind.Ray) ? _rayTarget : _geometrySize);
		QueueRedraw();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.DisplayHitBox, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisplayShape, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisplayRay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawBackground, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawGeometry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "fill", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "outline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetShapeSize, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shape", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Shape2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawHandle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetOriginHandle, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGeometryHandle, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.DisplayHitBox && args.Count == 1)
		{
			DisplayHitBox(VariantUtils.ConvertTo<CharacterHitBoxDefinition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisplayShape && args.Count == 1)
		{
			DisplayShape(VariantUtils.ConvertTo<AabbShape2DResource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisplayRay && args.Count == 1)
		{
			DisplayRay(VariantUtils.ConvertTo<AabbRay2DResource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName._GuiInput && args.Count == 1)
		{
			_GuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawBackground && args.Count == 0)
		{
			DrawBackground();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawGeometry && args.Count == 3)
		{
			DrawGeometry(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetShapeSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetShapeSize(VariantUtils.ConvertTo<Shape2D>(in args[0])));
			return true;
		}
		if (method == MethodName.DrawHandle && args.Count == 2)
		{
			DrawHandle(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetOriginHandle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetOriginHandle());
			return true;
		}
		if (method == MethodName.GetGeometryHandle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetGeometryHandle());
			return true;
		}
		if (method == MethodName.ResetPreview && args.Count == 0)
		{
			ResetPreview();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetShapeSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetShapeSize(VariantUtils.ConvertTo<Shape2D>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.DisplayHitBox)
		{
			return true;
		}
		if (method == MethodName.DisplayShape)
		{
			return true;
		}
		if (method == MethodName.DisplayRay)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName._GuiInput)
		{
			return true;
		}
		if (method == MethodName.DrawBackground)
		{
			return true;
		}
		if (method == MethodName.DrawGeometry)
		{
			return true;
		}
		if (method == MethodName.GetShapeSize)
		{
			return true;
		}
		if (method == MethodName.DrawHandle)
		{
			return true;
		}
		if (method == MethodName.GetOriginHandle)
		{
			return true;
		}
		if (method == MethodName.GetGeometryHandle)
		{
			return true;
		}
		if (method == MethodName.ResetPreview)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._kind)
		{
			_kind = VariantUtils.ConvertTo<GeometryKind>(in value);
			return true;
		}
		if (name == PropertyName._transform)
		{
			_transform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._geometrySize)
		{
			_geometrySize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._rayTarget)
		{
			_rayTarget = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._shape)
		{
			_shape = VariantUtils.ConvertTo<Shape2D>(in value);
			return true;
		}
		if (name == PropertyName._fill)
		{
			_fill = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._outline)
		{
			_outline = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._lineWidth)
		{
			_lineWidth = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._enabled)
		{
			_enabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragOrigin)
		{
			_dragOrigin = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragGeometry)
		{
			_dragGeometry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._previewOrigin)
		{
			_previewOrigin = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._previewGeometry)
		{
			_previewGeometry = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._kind)
		{
			value = VariantUtils.CreateFrom(in _kind);
			return true;
		}
		if (name == PropertyName._transform)
		{
			value = VariantUtils.CreateFrom(in _transform);
			return true;
		}
		if (name == PropertyName._geometrySize)
		{
			value = VariantUtils.CreateFrom(in _geometrySize);
			return true;
		}
		if (name == PropertyName._rayTarget)
		{
			value = VariantUtils.CreateFrom(in _rayTarget);
			return true;
		}
		if (name == PropertyName._shape)
		{
			value = VariantUtils.CreateFrom(in _shape);
			return true;
		}
		if (name == PropertyName._fill)
		{
			value = VariantUtils.CreateFrom(in _fill);
			return true;
		}
		if (name == PropertyName._outline)
		{
			value = VariantUtils.CreateFrom(in _outline);
			return true;
		}
		if (name == PropertyName._lineWidth)
		{
			value = VariantUtils.CreateFrom(in _lineWidth);
			return true;
		}
		if (name == PropertyName._enabled)
		{
			value = VariantUtils.CreateFrom(in _enabled);
			return true;
		}
		if (name == PropertyName._dragOrigin)
		{
			value = VariantUtils.CreateFrom(in _dragOrigin);
			return true;
		}
		if (name == PropertyName._dragGeometry)
		{
			value = VariantUtils.CreateFrom(in _dragGeometry);
			return true;
		}
		if (name == PropertyName._previewOrigin)
		{
			value = VariantUtils.CreateFrom(in _previewOrigin);
			return true;
		}
		if (name == PropertyName._previewGeometry)
		{
			value = VariantUtils.CreateFrom(in _previewGeometry);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._kind, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._transform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._geometrySize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._rayTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shape, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._fill, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._outline, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._lineWidth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._enabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dragOrigin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dragGeometry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previewOrigin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previewGeometry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._kind, Variant.From(in _kind));
		info.AddProperty(PropertyName._transform, Variant.From(in _transform));
		info.AddProperty(PropertyName._geometrySize, Variant.From(in _geometrySize));
		info.AddProperty(PropertyName._rayTarget, Variant.From(in _rayTarget));
		info.AddProperty(PropertyName._shape, Variant.From(in _shape));
		info.AddProperty(PropertyName._fill, Variant.From(in _fill));
		info.AddProperty(PropertyName._outline, Variant.From(in _outline));
		info.AddProperty(PropertyName._lineWidth, Variant.From(in _lineWidth));
		info.AddProperty(PropertyName._enabled, Variant.From(in _enabled));
		info.AddProperty(PropertyName._dragOrigin, Variant.From(in _dragOrigin));
		info.AddProperty(PropertyName._dragGeometry, Variant.From(in _dragGeometry));
		info.AddProperty(PropertyName._previewOrigin, Variant.From(in _previewOrigin));
		info.AddProperty(PropertyName._previewGeometry, Variant.From(in _previewGeometry));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._kind, out var value))
		{
			_kind = value.As<GeometryKind>();
		}
		if (info.TryGetProperty(PropertyName._transform, out var value2))
		{
			_transform = value2.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._geometrySize, out var value3))
		{
			_geometrySize = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._rayTarget, out var value4))
		{
			_rayTarget = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._shape, out var value5))
		{
			_shape = value5.As<Shape2D>();
		}
		if (info.TryGetProperty(PropertyName._fill, out var value6))
		{
			_fill = value6.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._outline, out var value7))
		{
			_outline = value7.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._lineWidth, out var value8))
		{
			_lineWidth = value8.As<float>();
		}
		if (info.TryGetProperty(PropertyName._enabled, out var value9))
		{
			_enabled = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragOrigin, out var value10))
		{
			_dragOrigin = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragGeometry, out var value11))
		{
			_dragGeometry = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._previewOrigin, out var value12))
		{
			_previewOrigin = value12.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._previewGeometry, out var value13))
		{
			_previewGeometry = value13.As<Vector2>();
		}
	}
}
