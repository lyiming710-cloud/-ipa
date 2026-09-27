using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Script/Camera/CameraMovingControl.cs")]
public class CameraMovingControl : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Input = "_Input";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName WINDOW_WIDTH = "WINDOW_WIDTH";

		public static readonly StringName WINDOW_HEIGHT = "WINDOW_HEIGHT";

		public static readonly StringName alive = "alive";

		public static readonly StringName camera = "camera";

		public static readonly StringName cameraZoomMin = "cameraZoomMin";

		public static readonly StringName cameraZoomMax = "cameraZoomMax";

		public static readonly StringName edgeMarkUL = "edgeMarkUL";

		public static readonly StringName edgeMarkDR = "edgeMarkDR";

		public static readonly StringName mousePress = "mousePress";

		public static readonly StringName mouseSavePos = "mouseSavePos";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private int WINDOW_WIDTH = (int)ProjectSettings.GetSetting("display/window/size/viewport_width");

	private int WINDOW_HEIGHT = (int)ProjectSettings.GetSetting("display/window/size/viewport_height");

	[Export(PropertyHint.None, "")]
	public bool alive = true;

	[Export(PropertyHint.None, "")]
	public Camera2D camera;

	[Export(PropertyHint.None, "")]
	public Vector2 cameraZoomMin = new Vector2(1.5f, 1.5f);

	[Export(PropertyHint.None, "")]
	public Vector2 cameraZoomMax = new Vector2(1f, 1f);

	[Export(PropertyHint.None, "")]
	public Marker2D edgeMarkUL;

	[Export(PropertyHint.None, "")]
	public Marker2D edgeMarkDR;

	public bool mousePress;

	public Vector2 mouseSavePos = Vector2.Zero;

	public override void _Ready()
	{
	}

	public override void _Input(InputEvent _event)
	{
		if (alive)
		{
			if (Input.IsActionJustReleased("RollUp"))
			{
				camera.Zoom += Vector2.One * 0.05f;
			}
			if (Input.IsActionJustReleased("RollDown"))
			{
				camera.Zoom -= Vector2.One * 0.05f;
			}
			if (Input.IsActionJustPressed("Press"))
			{
				mousePress = true;
				mouseSavePos = GetViewport().GetMousePosition();
			}
			if (Input.IsActionJustReleased("Press"))
			{
				mousePress = false;
			}
			if (mousePress)
			{
				camera.GlobalPosition += mouseSavePos - GetViewport().GetMousePosition();
				mouseSavePos = GetViewport().GetMousePosition();
			}
			camera.Zoom = new Vector2(Mathf.Clamp(camera.Zoom.X, cameraZoomMax.X, cameraZoomMin.X), Mathf.Clamp(camera.Zoom.Y, cameraZoomMax.Y, cameraZoomMin.Y));
			camera.GlobalPosition = new Vector2(Mathf.Clamp(camera.GlobalPosition.X, edgeMarkUL.GlobalPosition.X, edgeMarkDR.GlobalPosition.X - (float)WINDOW_WIDTH / camera.Zoom.X), Mathf.Clamp(camera.GlobalPosition.Y, edgeMarkUL.GlobalPosition.Y, edgeMarkDR.GlobalPosition.Y - (float)WINDOW_HEIGHT / camera.Zoom.Y));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.WINDOW_WIDTH)
		{
			WINDOW_WIDTH = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.WINDOW_HEIGHT)
		{
			WINDOW_HEIGHT = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.alive)
		{
			alive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.camera)
		{
			camera = VariantUtils.ConvertTo<Camera2D>(in value);
			return true;
		}
		if (name == PropertyName.cameraZoomMin)
		{
			cameraZoomMin = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.cameraZoomMax)
		{
			cameraZoomMax = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.edgeMarkUL)
		{
			edgeMarkUL = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.edgeMarkDR)
		{
			edgeMarkDR = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.mousePress)
		{
			mousePress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mouseSavePos)
		{
			mouseSavePos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.WINDOW_WIDTH)
		{
			value = VariantUtils.CreateFrom(in WINDOW_WIDTH);
			return true;
		}
		if (name == PropertyName.WINDOW_HEIGHT)
		{
			value = VariantUtils.CreateFrom(in WINDOW_HEIGHT);
			return true;
		}
		if (name == PropertyName.alive)
		{
			value = VariantUtils.CreateFrom(in alive);
			return true;
		}
		if (name == PropertyName.camera)
		{
			value = VariantUtils.CreateFrom(in camera);
			return true;
		}
		if (name == PropertyName.cameraZoomMin)
		{
			value = VariantUtils.CreateFrom(in cameraZoomMin);
			return true;
		}
		if (name == PropertyName.cameraZoomMax)
		{
			value = VariantUtils.CreateFrom(in cameraZoomMax);
			return true;
		}
		if (name == PropertyName.edgeMarkUL)
		{
			value = VariantUtils.CreateFrom(in edgeMarkUL);
			return true;
		}
		if (name == PropertyName.edgeMarkDR)
		{
			value = VariantUtils.CreateFrom(in edgeMarkDR);
			return true;
		}
		if (name == PropertyName.mousePress)
		{
			value = VariantUtils.CreateFrom(in mousePress);
			return true;
		}
		if (name == PropertyName.mouseSavePos)
		{
			value = VariantUtils.CreateFrom(in mouseSavePos);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.WINDOW_WIDTH, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.WINDOW_HEIGHT, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.alive, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.camera, PropertyHint.NodeType, "Camera2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.cameraZoomMin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.cameraZoomMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.edgeMarkUL, PropertyHint.NodeType, "Marker2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.edgeMarkDR, PropertyHint.NodeType, "Marker2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mousePress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.mouseSavePos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.WINDOW_WIDTH, Variant.From(in WINDOW_WIDTH));
		info.AddProperty(PropertyName.WINDOW_HEIGHT, Variant.From(in WINDOW_HEIGHT));
		info.AddProperty(PropertyName.alive, Variant.From(in alive));
		info.AddProperty(PropertyName.camera, Variant.From(in camera));
		info.AddProperty(PropertyName.cameraZoomMin, Variant.From(in cameraZoomMin));
		info.AddProperty(PropertyName.cameraZoomMax, Variant.From(in cameraZoomMax));
		info.AddProperty(PropertyName.edgeMarkUL, Variant.From(in edgeMarkUL));
		info.AddProperty(PropertyName.edgeMarkDR, Variant.From(in edgeMarkDR));
		info.AddProperty(PropertyName.mousePress, Variant.From(in mousePress));
		info.AddProperty(PropertyName.mouseSavePos, Variant.From(in mouseSavePos));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.WINDOW_WIDTH, out var value))
		{
			WINDOW_WIDTH = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.WINDOW_HEIGHT, out var value2))
		{
			WINDOW_HEIGHT = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.alive, out var value3))
		{
			alive = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.camera, out var value4))
		{
			camera = value4.As<Camera2D>();
		}
		if (info.TryGetProperty(PropertyName.cameraZoomMin, out var value5))
		{
			cameraZoomMin = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.cameraZoomMax, out var value6))
		{
			cameraZoomMax = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.edgeMarkUL, out var value7))
		{
			edgeMarkUL = value7.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.edgeMarkDR, out var value8))
		{
			edgeMarkDR = value8.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.mousePress, out var value9))
		{
			mousePress = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mouseSavePos, out var value10))
		{
			mouseSavePos = value10.As<Vector2>();
		}
	}
}
