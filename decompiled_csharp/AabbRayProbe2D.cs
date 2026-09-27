using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Core/TowerDefenseManager/SubSystem/AabbRayProbe2D.cs")]
public class AabbRayProbe2D : Node2D, IAabbCollisionPreview2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName SetCollisionPreviewDraw = "SetCollisionPreviewDraw";

		public static readonly StringName DrawArrowHead = "DrawArrowHead";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName targetPosition = "targetPosition";

		public static readonly StringName editorDraw = "editorDraw";

		public static readonly StringName editorRayColor = "editorRayColor";

		public static readonly StringName editorLineWidth = "editorLineWidth";

		public static readonly StringName editorEndpointRadius = "editorEndpointRadius";

		public static readonly StringName editorArrowSize = "editorArrowSize";

		public static readonly StringName _targetPosition = "_targetPosition";

		public static readonly StringName _collisionPreviewDraw = "_collisionPreviewDraw";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private Vector2 _targetPosition = new Vector2(2000f, 0f);

	private bool _collisionPreviewDraw;

	[Export(PropertyHint.None, "")]
	public Vector2 targetPosition
	{
		get
		{
			return _targetPosition;
		}
		set
		{
			_targetPosition = value;
			QueueRedraw();
		}
	}

	[ExportGroup("Editor Debug", "")]
	[Export(PropertyHint.None, "")]
	public bool editorDraw { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Color editorRayColor { get; set; } = new Color(0.2f, 1f, 0.25f, 0.8f);

	[Export(PropertyHint.None, "")]
	public float editorLineWidth { get; set; } = 2f;

	[Export(PropertyHint.None, "")]
	public float editorEndpointRadius { get; set; } = 5f;

	[Export(PropertyHint.None, "")]
	public float editorArrowSize { get; set; } = 14f;

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

	public override void _Draw()
	{
		if ((Engine.IsEditorHint() || _collisionPreviewDraw) && editorDraw)
		{
			DrawLine(Vector2.Zero, targetPosition, editorRayColor, editorLineWidth);
			DrawCircle(Vector2.Zero, editorEndpointRadius, editorRayColor);
			DrawCircle(targetPosition, editorEndpointRadius, editorRayColor);
			DrawArrowHead();
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

	private void DrawArrowHead()
	{
		Vector2 vector = targetPosition;
		if (!(vector.LengthSquared() < 1E-06f))
		{
			Vector2 vector2 = vector.Normalized();
			Vector2 vector3 = vector2.Orthogonal();
			Vector2 vector4 = targetPosition - vector2 * editorArrowSize;
			DrawPolygon(new Vector2[3]
			{
				targetPosition,
				vector4 + vector3 * editorArrowSize * 0.45f,
				vector4 - vector3 * editorArrowSize * 0.45f
			}, new Color[1] { editorRayColor });
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCollisionPreviewDraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawArrowHead, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.DrawArrowHead && args.Count == 0)
		{
			DrawArrowHead();
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
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.SetCollisionPreviewDraw)
		{
			return true;
		}
		if (method == MethodName.DrawArrowHead)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.targetPosition)
		{
			targetPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.editorDraw)
		{
			editorDraw = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.editorRayColor)
		{
			editorRayColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.editorLineWidth)
		{
			editorLineWidth = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.editorEndpointRadius)
		{
			editorEndpointRadius = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.editorArrowSize)
		{
			editorArrowSize = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._targetPosition)
		{
			_targetPosition = VariantUtils.ConvertTo<Vector2>(in value);
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
		if (name == PropertyName.targetPosition)
		{
			value = VariantUtils.CreateFrom<Vector2>(targetPosition);
			return true;
		}
		if (name == PropertyName.editorDraw)
		{
			value = VariantUtils.CreateFrom<bool>(editorDraw);
			return true;
		}
		if (name == PropertyName.editorRayColor)
		{
			value = VariantUtils.CreateFrom<Color>(editorRayColor);
			return true;
		}
		float from;
		if (name == PropertyName.editorLineWidth)
		{
			from = editorLineWidth;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.editorEndpointRadius)
		{
			from = editorEndpointRadius;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.editorArrowSize)
		{
			from = editorArrowSize;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._targetPosition)
		{
			value = VariantUtils.CreateFrom(in _targetPosition);
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
			new PropertyInfo(Variant.Type.Vector2, PropertyName._targetPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._collisionPreviewDraw, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.targetPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Editor Debug", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.editorDraw, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.editorRayColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.editorLineWidth, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.editorEndpointRadius, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.editorArrowSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.targetPosition, Variant.From<Vector2>(targetPosition));
		info.AddProperty(PropertyName.editorDraw, Variant.From<bool>(editorDraw));
		info.AddProperty(PropertyName.editorRayColor, Variant.From<Color>(editorRayColor));
		info.AddProperty(PropertyName.editorLineWidth, Variant.From<float>(editorLineWidth));
		info.AddProperty(PropertyName.editorEndpointRadius, Variant.From<float>(editorEndpointRadius));
		info.AddProperty(PropertyName.editorArrowSize, Variant.From<float>(editorArrowSize));
		info.AddProperty(PropertyName._targetPosition, Variant.From(in _targetPosition));
		info.AddProperty(PropertyName._collisionPreviewDraw, Variant.From(in _collisionPreviewDraw));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.targetPosition, out var value))
		{
			targetPosition = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.editorDraw, out var value2))
		{
			editorDraw = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.editorRayColor, out var value3))
		{
			editorRayColor = value3.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.editorLineWidth, out var value4))
		{
			editorLineWidth = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.editorEndpointRadius, out var value5))
		{
			editorEndpointRadius = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.editorArrowSize, out var value6))
		{
			editorArrowSize = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName._targetPosition, out var value7))
		{
			_targetPosition = value7.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._collisionPreviewDraw, out var value8))
		{
			_collisionPreviewDraw = value8.As<bool>();
		}
	}
}
