using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DragMenu/DragMenu.cs")]
public class DragMenu : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName SetChildPos = "SetChildPos";

		public static readonly StringName SetPos = "SetPos";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName alive = "alive";

		public static readonly StringName currentIndex = "currentIndex";

		public static readonly StringName interval = "interval";

		public static readonly StringName alphaDecrease = "alphaDecrease";

		public static readonly StringName scaleBase = "scaleBase";

		public static readonly StringName scaleDecrease = "scaleDecrease";

		public static readonly StringName scaleMin = "scaleMin";

		public static readonly StringName _currentIndex = "_currentIndex";

		public static readonly StringName currentPos = "currentPos";

		public static readonly StringName mousePress = "mousePress";

		public static readonly StringName mouseSavePos = "mouseSavePos";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private int _currentIndex;

	public Vector2 currentPos = Vector2.Zero;

	public bool mousePress;

	public Vector2 mouseSavePos = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public bool alive { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public int currentIndex
	{
		get
		{
			return _currentIndex;
		}
		set
		{
			if (_currentIndex != value)
			{
				_currentIndex = value;
				CurrentIndexChanged?.Invoke(value);
			}
		}
	}

	[ExportGroup("Setting", "")]
	[Export(PropertyHint.None, "")]
	public double interval { get; set; } = 400.0;

	[Export(PropertyHint.None, "")]
	public double alphaDecrease { get; set; } = 0.75;

	[Export(PropertyHint.None, "")]
	public Vector2 scaleBase { get; set; } = Vector2.One * 1.5f;

	[Export(PropertyHint.None, "")]
	public double scaleDecrease { get; set; } = 0.5;

	[Export(PropertyHint.None, "")]
	public double scaleMin { get; set; }

	public event Action<int> CurrentIndexChanged;

	public override void _Ready()
	{
		ChildEnteredTree += SetChildPos;
	}

	public override void _Input(InputEvent _event)
	{
		if (alive)
		{
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
				currentPos.X += (mouseSavePos.X - GetViewport().GetMousePosition().X) * 2f;
				currentIndex = (int)Mathf.Clamp(Mathf.Round(currentPos.X / (float)interval), 0f, Mathf.Max(0, GetChildCount() - 1));
				mouseSavePos = GetViewport().GetMousePosition();
			}
			else
			{
				currentPos.X = (float)currentIndex * (float)interval;
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		for (int i = 0; i < GetChildCount(); i++)
		{
			Control child = GetChild<Control>(i);
			if (child is DragMenuSelectItem { button: var button } dragMenuSelectItem)
			{
				button.Disabled = (mousePress && currentIndex != i) || dragMenuSelectItem.@lock;
			}
			Vector2 to = ((!mousePress) ? new Vector2((float)((double)(i - currentIndex) * interval), 0f) : new Vector2((float)((double)i * interval - (double)currentPos.X), 0f));
			child.Position = child.Position.Lerp(to, (float)(5.0 * delta));
			float num = Mathf.Abs(child.Position.X) / (float)interval;
			child.Modulate = new Color(1f, 1f, 1f, Mathf.Clamp(1f - (float)alphaDecrease * num, 0f, 1f));
			child.Scale = (scaleBase - Vector2.One * (float)scaleDecrease * num).Clamp(Vector2.One * (float)scaleMin, scaleBase);
		}
	}

	public void SetChildPos(Node node)
	{
		node.Set(value: new Vector2((float)(GetChildCount() - 1) * (float)interval, 0f), property: "position");
	}

	public void SetPos(int index)
	{
		currentIndex = index;
		currentPos.X = (float)currentIndex * (float)interval;
		for (int i = 0; i < GetChildCount(); i++)
		{
			Control child = GetChild<Control>(i);
			Vector2 position = new Vector2((float)((double)(i - currentIndex) * interval), 0f);
			child.Position = position;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetChildPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetChildPos && args.Count == 1)
		{
			SetChildPos(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPos && args.Count == 1)
		{
			SetPos(VariantUtils.ConvertTo<int>(in args[0]));
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.SetChildPos)
		{
			return true;
		}
		if (method == MethodName.SetPos)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.alive)
		{
			alive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.currentIndex)
		{
			currentIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.interval)
		{
			interval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.alphaDecrease)
		{
			alphaDecrease = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.scaleBase)
		{
			scaleBase = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.scaleDecrease)
		{
			scaleDecrease = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.scaleMin)
		{
			scaleMin = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._currentIndex)
		{
			_currentIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentPos)
		{
			currentPos = VariantUtils.ConvertTo<Vector2>(in value);
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
		if (name == PropertyName.alive)
		{
			value = VariantUtils.CreateFrom<bool>(alive);
			return true;
		}
		if (name == PropertyName.currentIndex)
		{
			value = VariantUtils.CreateFrom<int>(currentIndex);
			return true;
		}
		double from;
		if (name == PropertyName.interval)
		{
			from = interval;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.alphaDecrease)
		{
			from = alphaDecrease;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.scaleBase)
		{
			value = VariantUtils.CreateFrom<Vector2>(scaleBase);
			return true;
		}
		if (name == PropertyName.scaleDecrease)
		{
			from = scaleDecrease;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.scaleMin)
		{
			from = scaleMin;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._currentIndex)
		{
			value = VariantUtils.CreateFrom(in _currentIndex);
			return true;
		}
		if (name == PropertyName.currentPos)
		{
			value = VariantUtils.CreateFrom(in currentPos);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.alive, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentIndex, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Setting", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.interval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.alphaDecrease, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.scaleBase, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.scaleDecrease, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.scaleMin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.currentPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mousePress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.mouseSavePos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.alive, Variant.From<bool>(alive));
		info.AddProperty(PropertyName.currentIndex, Variant.From<int>(currentIndex));
		info.AddProperty(PropertyName.interval, Variant.From<double>(interval));
		info.AddProperty(PropertyName.alphaDecrease, Variant.From<double>(alphaDecrease));
		info.AddProperty(PropertyName.scaleBase, Variant.From<Vector2>(scaleBase));
		info.AddProperty(PropertyName.scaleDecrease, Variant.From<double>(scaleDecrease));
		info.AddProperty(PropertyName.scaleMin, Variant.From<double>(scaleMin));
		info.AddProperty(PropertyName._currentIndex, Variant.From(in _currentIndex));
		info.AddProperty(PropertyName.currentPos, Variant.From(in currentPos));
		info.AddProperty(PropertyName.mousePress, Variant.From(in mousePress));
		info.AddProperty(PropertyName.mouseSavePos, Variant.From(in mouseSavePos));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.alive, out var value))
		{
			alive = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.currentIndex, out var value2))
		{
			currentIndex = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.interval, out var value3))
		{
			interval = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.alphaDecrease, out var value4))
		{
			alphaDecrease = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.scaleBase, out var value5))
		{
			scaleBase = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.scaleDecrease, out var value6))
		{
			scaleDecrease = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.scaleMin, out var value7))
		{
			scaleMin = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._currentIndex, out var value8))
		{
			_currentIndex = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentPos, out var value9))
		{
			currentPos = value9.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.mousePress, out var value10))
		{
			mousePress = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mouseSavePos, out var value11))
		{
			mouseSavePos = value11.As<Vector2>();
		}
	}
}
