using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Core/TowerDefenseManager/SubSystem/AabbProbe2D.cs")]
public class AabbProbe2D : Node2D, IAabbCollisionPreview2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName SetCollisionPreviewDraw = "SetCollisionPreviewDraw";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName DrawProbeShape = "DrawProbeShape";

		public static readonly StringName DrawProbeRectangle = "DrawProbeRectangle";

		public static readonly StringName DrawProbeCircle = "DrawProbeCircle";

		public static readonly StringName DrawProbeWorldRect = "DrawProbeWorldRect";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName Enabled = "Enabled";

		public static readonly StringName Shape = "Shape";

		public static readonly StringName Size = "Size";

		public static readonly StringName Radius = "Radius";

		public static readonly StringName SegmentA = "SegmentA";

		public static readonly StringName SegmentB = "SegmentB";

		public static readonly StringName WorldRect = "WorldRect";

		public static readonly StringName editorDraw = "editorDraw";

		public static readonly StringName editorShapeColor = "editorShapeColor";

		public static readonly StringName editorAabbColor = "editorAabbColor";

		public static readonly StringName editorLineWidth = "editorLineWidth";

		public static readonly StringName _collisionPreviewDraw = "_collisionPreviewDraw";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private bool _collisionPreviewDraw;

	[Export(PropertyHint.None, "")]
	public bool Enabled { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public AabbProbeShape2D Shape { get; set; }

	[Export(PropertyHint.None, "")]
	public Vector2 Size { get; set; } = new Vector2(80f, 80f);

	[Export(PropertyHint.None, "")]
	public float Radius { get; set; } = 40f;

	[Export(PropertyHint.None, "")]
	public Vector2 SegmentA { get; set; } = new Vector2(-40f, 0f);

	[Export(PropertyHint.None, "")]
	public Vector2 SegmentB { get; set; } = new Vector2(40f, 0f);

	public Rect2 WorldRect
	{
		get
		{
			if (!Enabled)
			{
				return AabbShapeUtil.RectFromCenter(GlobalPosition, Vector2.Zero);
			}
			return AabbShapeUtil.ComputeProbeWorldRect(this);
		}
	}

	[ExportGroup("Editor Debug", "")]
	[Export(PropertyHint.None, "")]
	public bool editorDraw { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Color editorShapeColor { get; set; } = new Color(0.1f, 0.75f, 1f, 0.75f);

	[Export(PropertyHint.None, "")]
	public Color editorAabbColor { get; set; } = new Color(1f, 0.7f, 0.1f, 0.65f);

	[Export(PropertyHint.None, "")]
	public float editorLineWidth { get; set; } = 2f;

	public override void _Ready()
	{
		SetProcess(Engine.IsEditorHint() || _collisionPreviewDraw);
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint() || _collisionPreviewDraw)
		{
			QueueRedraw();
		}
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

	public override void _Draw()
	{
		if ((Engine.IsEditorHint() || _collisionPreviewDraw) && editorDraw)
		{
			DrawProbeShape();
			DrawProbeWorldRect();
		}
	}

	private void DrawProbeShape()
	{
		switch (Shape)
		{
		case AabbProbeShape2D.Circle:
			DrawProbeCircle();
			break;
		case AabbProbeShape2D.Segment:
			DrawLine(SegmentA, SegmentB, editorShapeColor, editorLineWidth);
			break;
		default:
			DrawProbeRectangle();
			break;
		}
	}

	private void DrawProbeRectangle()
	{
		Vector2 vector = Size * 0.5f;
		DrawPolyline(new Vector2[5]
		{
			new Vector2(0f - vector.X, 0f - vector.Y),
			new Vector2(vector.X, 0f - vector.Y),
			new Vector2(vector.X, vector.Y),
			new Vector2(0f - vector.X, vector.Y),
			new Vector2(0f - vector.X, 0f - vector.Y)
		}, editorShapeColor, editorLineWidth);
	}

	private void DrawProbeCircle()
	{
		Vector2[] array = new Vector2[33];
		for (int i = 0; i <= 32; i++)
		{
			float angle = (float)Math.PI * 2f * (float)i / 32f;
			array[i] = Vector2.FromAngle(angle) * Radius;
		}
		DrawPolyline(array, editorShapeColor, editorLineWidth);
	}

	private void DrawProbeWorldRect()
	{
		Rect2 worldRect = WorldRect;
		if (!(worldRect.Size == Vector2.Zero))
		{
			Vector2 position = worldRect.Position;
			Vector2 vector = worldRect.Position + worldRect.Size;
			DrawPolyline(new Vector2[5]
			{
				ToLocal(new Vector2(position.X, position.Y)),
				ToLocal(new Vector2(vector.X, position.Y)),
				ToLocal(new Vector2(vector.X, vector.Y)),
				ToLocal(new Vector2(position.X, vector.Y)),
				ToLocal(new Vector2(position.X, position.Y))
			}, editorAabbColor, editorLineWidth);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCollisionPreviewDraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawProbeShape, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawProbeRectangle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawProbeCircle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawProbeWorldRect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCollisionPreviewDraw && args.Count == 1)
		{
			SetCollisionPreviewDraw(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawProbeShape && args.Count == 0)
		{
			DrawProbeShape();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawProbeRectangle && args.Count == 0)
		{
			DrawProbeRectangle();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawProbeCircle && args.Count == 0)
		{
			DrawProbeCircle();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawProbeWorldRect && args.Count == 0)
		{
			DrawProbeWorldRect();
			ret = default;
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.SetCollisionPreviewDraw)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.DrawProbeShape)
		{
			return true;
		}
		if (method == MethodName.DrawProbeRectangle)
		{
			return true;
		}
		if (method == MethodName.DrawProbeCircle)
		{
			return true;
		}
		if (method == MethodName.DrawProbeWorldRect)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Enabled)
		{
			Enabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Shape)
		{
			Shape = VariantUtils.ConvertTo<AabbProbeShape2D>(in value);
			return true;
		}
		if (name == PropertyName.Size)
		{
			Size = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.Radius)
		{
			Radius = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.SegmentA)
		{
			SegmentA = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.SegmentB)
		{
			SegmentB = VariantUtils.ConvertTo<Vector2>(in value);
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
		bool from;
		if (name == PropertyName.Enabled)
		{
			from = Enabled;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Shape)
		{
			value = VariantUtils.CreateFrom<AabbProbeShape2D>(Shape);
			return true;
		}
		Vector2 from2;
		if (name == PropertyName.Size)
		{
			from2 = Size;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		float from3;
		if (name == PropertyName.Radius)
		{
			from3 = Radius;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.SegmentA)
		{
			from2 = SegmentA;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SegmentB)
		{
			from2 = SegmentB;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.WorldRect)
		{
			value = VariantUtils.CreateFrom<Rect2>(WorldRect);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.Enabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.Shape, PropertyHint.Enum, "Rectangle,Circle,Segment", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Size, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.Radius, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.SegmentA, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.SegmentB, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.WorldRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, "Editor Debug", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.editorDraw, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.editorShapeColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.editorAabbColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.editorLineWidth, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Enabled, Variant.From<bool>(Enabled));
		info.AddProperty(PropertyName.Shape, Variant.From<AabbProbeShape2D>(Shape));
		info.AddProperty(PropertyName.Size, Variant.From<Vector2>(Size));
		info.AddProperty(PropertyName.Radius, Variant.From<float>(Radius));
		info.AddProperty(PropertyName.SegmentA, Variant.From<Vector2>(SegmentA));
		info.AddProperty(PropertyName.SegmentB, Variant.From<Vector2>(SegmentB));
		info.AddProperty(PropertyName.editorDraw, Variant.From<bool>(editorDraw));
		info.AddProperty(PropertyName.editorShapeColor, Variant.From<Color>(editorShapeColor));
		info.AddProperty(PropertyName.editorAabbColor, Variant.From<Color>(editorAabbColor));
		info.AddProperty(PropertyName.editorLineWidth, Variant.From<float>(editorLineWidth));
		info.AddProperty(PropertyName._collisionPreviewDraw, Variant.From(in _collisionPreviewDraw));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Enabled, out var value))
		{
			Enabled = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Shape, out var value2))
		{
			Shape = value2.As<AabbProbeShape2D>();
		}
		if (info.TryGetProperty(PropertyName.Size, out var value3))
		{
			Size = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.Radius, out var value4))
		{
			Radius = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.SegmentA, out var value5))
		{
			SegmentA = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.SegmentB, out var value6))
		{
			SegmentB = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.editorDraw, out var value7))
		{
			editorDraw = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.editorShapeColor, out var value8))
		{
			editorShapeColor = value8.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.editorAabbColor, out var value9))
		{
			editorAabbColor = value9.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.editorLineWidth, out var value10))
		{
			editorLineWidth = value10.As<float>();
		}
		if (info.TryGetProperty(PropertyName._collisionPreviewDraw, out var value11))
		{
			_collisionPreviewDraw = value11.As<bool>();
		}
	}
}
