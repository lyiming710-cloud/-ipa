using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/GUI/Button/NinePatchButtonBase/NinePatchButtonBase.cs")]
public class NinePatchButtonBase : MarginContainer
{
	public delegate void PressedEventHandler();

	public delegate void MouseEnteredEventHandler();

	public delegate void MouseExitedEventHandler();

	public new class MethodName : MarginContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnReady = "OnReady";

		public static readonly StringName _Pressed = "_Pressed";

		public static readonly StringName _MouseEntered = "_MouseEntered";

		public static readonly StringName _MouseExited = "_MouseExited";

		public static readonly StringName ButtonDown = "ButtonDown";

		public static readonly StringName ButtonUp = "ButtonUp";
	}

	public new class PropertyName : MarginContainer.PropertyName
	{
		public static readonly StringName disable = "disable";

		public static readonly StringName text = "text";

		public static readonly StringName normalTexture = "normalTexture";

		public static readonly StringName pressedTexture = "pressedTexture";

		public static readonly StringName hoverTexture = "hoverTexture";

		public static readonly StringName ninePatchTexture = "ninePatchTexture";

		public static readonly StringName labelText = "labelText";

		public static readonly StringName _disable = "_disable";

		public static readonly StringName _text = "_text";

		public static readonly StringName _normalTexture = "_normalTexture";

		public static readonly StringName _pressedTexture = "_pressedTexture";

		public static readonly StringName _hoverTexture = "_hoverTexture";

		public static readonly StringName downSfx = "downSfx";

		public static readonly StringName upSfx = "upSfx";
	}

	public new class SignalName : MarginContainer.SignalName
	{
	}

	public NinePatchRect ninePatchTexture;

	public Label labelText;

	private bool _disable;

	private string _text = "";

	private Texture2D _normalTexture;

	private Texture2D _pressedTexture;

	private Texture2D _hoverTexture;

	[Export(PropertyHint.None, "")]
	public string downSfx = "ButtonClickPress";

	[Export(PropertyHint.None, "")]
	public string upSfx = "ButtonClickRelease";

	[Export(PropertyHint.None, "")]
	public bool disable
	{
		get
		{
			return _disable;
		}
		set
		{
			_disable = value;
			if (_disable)
			{
				ninePatchTexture?.SetDeferred("texture", normalTexture);
			}
		}
	}

	[Export(PropertyHint.MultilineText, "")]
	public string text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = value;
			if (labelText != null)
			{
				labelText.Text = value;
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public Texture2D normalTexture
	{
		get
		{
			return _normalTexture;
		}
		set
		{
			_normalTexture = value;
			if (ninePatchTexture != null)
			{
				ninePatchTexture.Texture = value;
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public Texture2D pressedTexture
	{
		get
		{
			return _pressedTexture;
		}
		set
		{
			_pressedTexture = value;
		}
	}

	[Export(PropertyHint.None, "")]
	public Texture2D hoverTexture
	{
		get
		{
			return _hoverTexture;
		}
		set
		{
			_hoverTexture = value;
		}
	}

	public event PressedEventHandler OnPressed;

	public event MouseEnteredEventHandler OnMouseEntered;

	public event MouseExitedEventHandler OnMouseExited;

	public override void _Ready()
	{
		ninePatchTexture = GetNode<NinePatchRect>("%NinePatchTexture");
		labelText = GetNode<Label>("%LabelText");
		labelText.Text = text;
		ninePatchTexture.Texture = normalTexture;
		TextureButton node = GetNode<TextureButton>("%TextureButton");
		node.Pressed += _Pressed;
		node.ButtonDown += ButtonDown;
		node.ButtonUp += ButtonUp;
		node.MouseEntered += _MouseEntered;
		node.MouseExited += _MouseExited;
		OnReady();
	}

	public virtual void OnReady()
	{
	}

	public void _Pressed()
	{
		if (!disable)
		{
			ninePatchTexture.Texture = pressedTexture;
			OnPressed?.Invoke();
		}
	}

	public void _MouseEntered()
	{
		if (!disable)
		{
			ninePatchTexture.Texture = hoverTexture;
			OnMouseEntered?.Invoke();
		}
	}

	public void _MouseExited()
	{
		if (!disable)
		{
			ninePatchTexture.Texture = normalTexture;
			OnMouseExited?.Invoke();
		}
	}

	public void ButtonDown()
	{
		if (!disable && downSfx != "")
		{
			AudioManager.Instance.AudioPlay(downSfx);
		}
	}

	public void ButtonUp()
	{
		if (!disable && upSfx != "")
		{
			AudioManager.Instance.AudioPlay(upSfx);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._MouseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._MouseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ButtonDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ButtonUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnReady && args.Count == 0)
		{
			OnReady();
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
		if (method == MethodName.ButtonDown && args.Count == 0)
		{
			ButtonDown();
			ret = default;
			return true;
		}
		if (method == MethodName.ButtonUp && args.Count == 0)
		{
			ButtonUp();
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
		if (method == MethodName.OnReady)
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
		if (method == MethodName.ButtonDown)
		{
			return true;
		}
		if (method == MethodName.ButtonUp)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.disable)
		{
			disable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.text)
		{
			text = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.normalTexture)
		{
			normalTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.pressedTexture)
		{
			pressedTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.hoverTexture)
		{
			hoverTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.ninePatchTexture)
		{
			ninePatchTexture = VariantUtils.ConvertTo<NinePatchRect>(in value);
			return true;
		}
		if (name == PropertyName.labelText)
		{
			labelText = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._disable)
		{
			_disable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._text)
		{
			_text = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._normalTexture)
		{
			_normalTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._pressedTexture)
		{
			_pressedTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._hoverTexture)
		{
			_hoverTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.downSfx)
		{
			downSfx = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.upSfx)
		{
			upSfx = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.disable)
		{
			value = VariantUtils.CreateFrom<bool>(disable);
			return true;
		}
		if (name == PropertyName.text)
		{
			value = VariantUtils.CreateFrom<string>(text);
			return true;
		}
		Texture2D from;
		if (name == PropertyName.normalTexture)
		{
			from = normalTexture;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.pressedTexture)
		{
			from = pressedTexture;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.hoverTexture)
		{
			from = hoverTexture;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ninePatchTexture)
		{
			value = VariantUtils.CreateFrom(in ninePatchTexture);
			return true;
		}
		if (name == PropertyName.labelText)
		{
			value = VariantUtils.CreateFrom(in labelText);
			return true;
		}
		if (name == PropertyName._disable)
		{
			value = VariantUtils.CreateFrom(in _disable);
			return true;
		}
		if (name == PropertyName._text)
		{
			value = VariantUtils.CreateFrom(in _text);
			return true;
		}
		if (name == PropertyName._normalTexture)
		{
			value = VariantUtils.CreateFrom(in _normalTexture);
			return true;
		}
		if (name == PropertyName._pressedTexture)
		{
			value = VariantUtils.CreateFrom(in _pressedTexture);
			return true;
		}
		if (name == PropertyName._hoverTexture)
		{
			value = VariantUtils.CreateFrom(in _hoverTexture);
			return true;
		}
		if (name == PropertyName.downSfx)
		{
			value = VariantUtils.CreateFrom(in downSfx);
			return true;
		}
		if (name == PropertyName.upSfx)
		{
			value = VariantUtils.CreateFrom(in upSfx);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.ninePatchTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.labelText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._disable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.disable, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._text, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.text, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._normalTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.normalTexture, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._pressedTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pressedTexture, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._hoverTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.hoverTexture, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.downSfx, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.upSfx, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.disable, Variant.From<bool>(disable));
		info.AddProperty(PropertyName.text, Variant.From<string>(text));
		info.AddProperty(PropertyName.normalTexture, Variant.From<Texture2D>(normalTexture));
		info.AddProperty(PropertyName.pressedTexture, Variant.From<Texture2D>(pressedTexture));
		info.AddProperty(PropertyName.hoverTexture, Variant.From<Texture2D>(hoverTexture));
		info.AddProperty(PropertyName.ninePatchTexture, Variant.From(in ninePatchTexture));
		info.AddProperty(PropertyName.labelText, Variant.From(in labelText));
		info.AddProperty(PropertyName._disable, Variant.From(in _disable));
		info.AddProperty(PropertyName._text, Variant.From(in _text));
		info.AddProperty(PropertyName._normalTexture, Variant.From(in _normalTexture));
		info.AddProperty(PropertyName._pressedTexture, Variant.From(in _pressedTexture));
		info.AddProperty(PropertyName._hoverTexture, Variant.From(in _hoverTexture));
		info.AddProperty(PropertyName.downSfx, Variant.From(in downSfx));
		info.AddProperty(PropertyName.upSfx, Variant.From(in upSfx));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.disable, out var value))
		{
			disable = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.text, out var value2))
		{
			text = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.normalTexture, out var value3))
		{
			normalTexture = value3.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.pressedTexture, out var value4))
		{
			pressedTexture = value4.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.hoverTexture, out var value5))
		{
			hoverTexture = value5.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.ninePatchTexture, out var value6))
		{
			ninePatchTexture = value6.As<NinePatchRect>();
		}
		if (info.TryGetProperty(PropertyName.labelText, out var value7))
		{
			labelText = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._disable, out var value8))
		{
			_disable = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._text, out var value9))
		{
			_text = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName._normalTexture, out var value10))
		{
			_normalTexture = value10.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._pressedTexture, out var value11))
		{
			_pressedTexture = value11.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._hoverTexture, out var value12))
		{
			_hoverTexture = value12.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.downSfx, out var value13))
		{
			downSfx = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName.upSfx, out var value14))
		{
			upSfx = value14.As<string>();
		}
	}
}
