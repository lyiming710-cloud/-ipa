using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DragMenu/Select/DragMenuSelectItem.cs")]
public class DragMenuSelectItem : Control
{
	public delegate void SelectEventHandler(int index);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnSpriteTextureChanged = "OnSpriteTextureChanged";

		public static readonly StringName OnButtonPressed = "OnButtonPressed";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName textureDisplaySize = "textureDisplaySize";

		public static readonly StringName buttonNode = "buttonNode";

		public static readonly StringName graphics = "graphics";

		public static readonly StringName sprite = "sprite";

		public static readonly StringName button = "button";

		public static readonly StringName index = "index";

		public static readonly StringName @lock = "lock";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public Control graphics;

	public Sprite2D sprite;

	public Button button;

	public int index = -1;

	public bool @lock;

	[Export(PropertyHint.None, "")]
	public Vector2 textureDisplaySize { get; set; } = Vector2.Zero;

	public Button buttonNode => button;

	public event SelectEventHandler OnSelect;

	public override void _Ready()
	{
		graphics = GetNode<Control>("%Graphics");
		sprite = GetNode<Sprite2D>("%Sprite");
		button = GetNode<Button>("%Button");
		sprite.TextureChanged += OnSpriteTextureChanged;
		button.Pressed += OnButtonPressed;
	}

	public void OnSpriteTextureChanged()
	{
		if (GodotObject.IsInstanceValid(sprite.Texture))
		{
			Vector2 size = sprite.Texture.GetSize();
			Vector2 vector = ((textureDisplaySize.X > 0f && textureDisplaySize.Y > 0f) ? textureDisplaySize : size);
			sprite.Scale = vector / size;
			button.Size = vector;
			button.Position = -button.Size / 2f;
		}
	}

	public void OnButtonPressed()
	{
		OnSelect?.Invoke(index);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSpriteTextureChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnSpriteTextureChanged && args.Count == 0)
		{
			OnSpriteTextureChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnButtonPressed && args.Count == 0)
		{
			OnButtonPressed();
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
		if (method == MethodName.OnSpriteTextureChanged)
		{
			return true;
		}
		if (method == MethodName.OnButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.textureDisplaySize)
		{
			textureDisplaySize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.graphics)
		{
			graphics = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.button)
		{
			button = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.index)
		{
			index = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.@lock)
		{
			@lock = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.textureDisplaySize)
		{
			value = VariantUtils.CreateFrom<Vector2>(textureDisplaySize);
			return true;
		}
		if (name == PropertyName.buttonNode)
		{
			value = VariantUtils.CreateFrom<Button>(buttonNode);
			return true;
		}
		if (name == PropertyName.graphics)
		{
			value = VariantUtils.CreateFrom(in graphics);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom(in sprite);
			return true;
		}
		if (name == PropertyName.button)
		{
			value = VariantUtils.CreateFrom(in button);
			return true;
		}
		if (name == PropertyName.index)
		{
			value = VariantUtils.CreateFrom(in index);
			return true;
		}
		if (name == PropertyName.@lock)
		{
			value = VariantUtils.CreateFrom(in @lock);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.graphics, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.button, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.textureDisplaySize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.index, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.@lock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.buttonNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.textureDisplaySize, Variant.From<Vector2>(textureDisplaySize));
		info.AddProperty(PropertyName.graphics, Variant.From(in graphics));
		info.AddProperty(PropertyName.sprite, Variant.From(in sprite));
		info.AddProperty(PropertyName.button, Variant.From(in button));
		info.AddProperty(PropertyName.index, Variant.From(in index));
		info.AddProperty(PropertyName.@lock, Variant.From(in @lock));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.textureDisplaySize, out var value))
		{
			textureDisplaySize = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.graphics, out var value2))
		{
			graphics = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.sprite, out var value3))
		{
			sprite = value3.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.button, out var value4))
		{
			button = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.index, out var value5))
		{
			index = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.@lock, out var value6))
		{
			@lock = value6.As<bool>();
		}
	}
}
