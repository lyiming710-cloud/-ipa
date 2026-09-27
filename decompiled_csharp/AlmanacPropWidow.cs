using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/Almanac/AlmanacProp/AlmanacPropWidow.cs")]
public class AlmanacPropWidow : Control
{
	public delegate void PressedShovelEventHandler(ShovelConfig config);

	public delegate void PressedMowerEventHandler(MowerConfig config);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitShovel = "InitShovel";

		public static readonly StringName InitMower = "InitMower";

		public static readonly StringName Pressed = "Pressed";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName spriteNode = "spriteNode";

		public static readonly StringName type = "type";

		public static readonly StringName shovelConfig = "shovelConfig";

		public static readonly StringName mowerConfig = "mowerConfig";

		public static readonly StringName sprite = "sprite";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public Control spriteNode;

	public string type = "Shovel";

	public ShovelConfig shovelConfig;

	public MowerConfig mowerConfig;

	public Sprite2D sprite;

	public event PressedShovelEventHandler OnPressedShovel;

	public event PressedMowerEventHandler OnPressedMower;

	public override void _Ready()
	{
		spriteNode = GetNode<Control>("%SpriteNode");
		GetNode<BaseButton>("SpriteBrightButton/Transform/Button").Pressed += Pressed;
	}

	public void InitShovel(ShovelConfig config)
	{
		type = "Shovel";
		shovelConfig = config;
		sprite = new Sprite2D();
		sprite.Texture = shovelConfig.texture;
		sprite.LightMask = 0;
		sprite.Scale = Vector2.One * 0.8f;
		spriteNode.AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
	}

	public void InitMower(MowerConfig config)
	{
		type = "Mower";
		mowerConfig = config;
		sprite = new Sprite2D();
		sprite.Texture = mowerConfig.texture;
		sprite.LightMask = 0;
		sprite.Scale = Vector2.One * 0.5f;
		spriteNode.AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
	}

	public void Pressed()
	{
		AudioManager.Instance.AudioPlay("PacketPick");
		string text = type;
		if (!(text == "Shovel"))
		{
			if (text == "Mower")
			{
				OnPressedMower?.Invoke(mowerConfig);
			}
		}
		else
		{
			OnPressedShovel?.Invoke(shovelConfig);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitShovel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.InitMower, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.InitShovel && args.Count == 1)
		{
			InitShovel(VariantUtils.ConvertTo<ShovelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitMower && args.Count == 1)
		{
			InitMower(VariantUtils.ConvertTo<MowerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Pressed && args.Count == 0)
		{
			Pressed();
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
		if (method == MethodName.InitShovel)
		{
			return true;
		}
		if (method == MethodName.InitMower)
		{
			return true;
		}
		if (method == MethodName.Pressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.spriteNode)
		{
			spriteNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.type)
		{
			type = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.shovelConfig)
		{
			shovelConfig = VariantUtils.ConvertTo<ShovelConfig>(in value);
			return true;
		}
		if (name == PropertyName.mowerConfig)
		{
			mowerConfig = VariantUtils.ConvertTo<MowerConfig>(in value);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.spriteNode)
		{
			value = VariantUtils.CreateFrom(in spriteNode);
			return true;
		}
		if (name == PropertyName.type)
		{
			value = VariantUtils.CreateFrom(in type);
			return true;
		}
		if (name == PropertyName.shovelConfig)
		{
			value = VariantUtils.CreateFrom(in shovelConfig);
			return true;
		}
		if (name == PropertyName.mowerConfig)
		{
			value = VariantUtils.CreateFrom(in mowerConfig);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom(in sprite);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.spriteNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.type, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.shovelConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mowerConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.spriteNode, Variant.From(in spriteNode));
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName.shovelConfig, Variant.From(in shovelConfig));
		info.AddProperty(PropertyName.mowerConfig, Variant.From(in mowerConfig));
		info.AddProperty(PropertyName.sprite, Variant.From(in sprite));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.spriteNode, out var value))
		{
			spriteNode = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.type, out var value2))
		{
			type = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.shovelConfig, out var value3))
		{
			shovelConfig = value3.As<ShovelConfig>();
		}
		if (info.TryGetProperty(PropertyName.mowerConfig, out var value4))
		{
			mowerConfig = value4.As<MowerConfig>();
		}
		if (info.TryGetProperty(PropertyName.sprite, out var value5))
		{
			sprite = value5.As<Sprite2D>();
		}
	}
}
