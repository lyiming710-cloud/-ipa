using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/GUI/Button/SpriteBrightButton/SpriteBrightButton.cs")]
public class SpriteBrightButton : TextureRect
{
	public delegate void PressedEventHandler();

	public delegate void MouseEnteredEventHandler();

	public delegate void MouseExitedEventHandler();

	public new class MethodName : TextureRect.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RefreshButtonRect = "RefreshButtonRect";

		public static readonly StringName _Pressed = "_Pressed";

		public static readonly StringName _MouseEntered = "_MouseEntered";

		public static readonly StringName _MouseExited = "_MouseExited";
	}

	public new class PropertyName : TextureRect.PropertyName
	{
		public static readonly StringName autoSize = "autoSize";

		public static readonly StringName button = "button";

		public static readonly StringName _autoSize = "_autoSize";

		public static readonly StringName disabled = "disabled";
	}

	public new class SignalName : TextureRect.SignalName
	{
	}

	public Button button;

	private bool _autoSize;

	[Export(PropertyHint.None, "")]
	public bool disabled;

	[Export(PropertyHint.None, "")]
	public bool autoSize
	{
		get
		{
			return _autoSize;
		}
		set
		{
			if (_autoSize != value)
			{
				_autoSize = value;
				RefreshButtonRect();
			}
		}
	}

	public event PressedEventHandler OnPressed;

	public event MouseEnteredEventHandler OnMouseEntered;

	public event MouseExitedEventHandler OnMouseExited;

	public override void _Ready()
	{
		button = GetNode<Button>("%Button");
		button.MouseEntered += _MouseEntered;
		button.MouseExited += _MouseExited;
		button.Pressed += _Pressed;
		Material = (Material)Material.Duplicate();
		Resized += RefreshButtonRect;
		RefreshButtonRect();
	}

	private void RefreshButtonRect()
	{
		if (_autoSize && GodotObject.IsInstanceValid(button))
		{
			button.Position = Vector2.Zero;
			button.Size = Size;
		}
	}

	public void _Pressed()
	{
		if (!disabled)
		{
			OnPressed?.Invoke();
			AudioManager.Instance.AudioPlay("ButtonPress");
		}
	}

	public void _MouseEntered()
	{
		OnMouseEntered?.Invoke();
		if (!disabled)
		{
			((ShaderMaterial)Material).SetShaderParameter("brightStrength", 0.3);
		}
	}

	public void _MouseExited()
	{
		OnMouseExited?.Invoke();
		if (!disabled)
		{
			((ShaderMaterial)Material).SetShaderParameter("brightStrength", 0.0);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshButtonRect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._MouseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._MouseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.RefreshButtonRect && args.Count == 0)
		{
			RefreshButtonRect();
			ret = default;
			return true;
		}
		if (method == MethodName._Pressed && args.Count == 0)
		{
			_Pressed();
			ret = default;
			return true;
		}
		if (method == MethodName._MouseEntered && args.Count == 0)
		{
			_MouseEntered();
			ret = default;
			return true;
		}
		if (method == MethodName._MouseExited && args.Count == 0)
		{
			_MouseExited();
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
		if (method == MethodName.RefreshButtonRect)
		{
			return true;
		}
		if (method == MethodName._Pressed)
		{
			return true;
		}
		if (method == MethodName._MouseEntered)
		{
			return true;
		}
		if (method == MethodName._MouseExited)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.autoSize)
		{
			autoSize = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.button)
		{
			button = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._autoSize)
		{
			_autoSize = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.disabled)
		{
			disabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.autoSize)
		{
			value = VariantUtils.CreateFrom<bool>(autoSize);
			return true;
		}
		if (name == PropertyName.button)
		{
			value = VariantUtils.CreateFrom(in button);
			return true;
		}
		if (name == PropertyName._autoSize)
		{
			value = VariantUtils.CreateFrom(in _autoSize);
			return true;
		}
		if (name == PropertyName.disabled)
		{
			value = VariantUtils.CreateFrom(in disabled);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.button, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._autoSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.autoSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.disabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.autoSize, Variant.From<bool>(autoSize));
		info.AddProperty(PropertyName.button, Variant.From(in button));
		info.AddProperty(PropertyName._autoSize, Variant.From(in _autoSize));
		info.AddProperty(PropertyName.disabled, Variant.From(in disabled));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.autoSize, out var value))
		{
			autoSize = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.button, out var value2))
		{
			button = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._autoSize, out var value3))
		{
			_autoSize = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.disabled, out var value4))
		{
			disabled = value4.As<bool>();
		}
	}
}
