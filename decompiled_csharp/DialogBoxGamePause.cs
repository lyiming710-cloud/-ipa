using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/DialogBox/DialogBoxGeneral/DialogBoxGamePause/DialogBoxGamePause.cs")]
public class DialogBoxGamePause : DialogPopup
{
	public new class MethodName : DialogPopup.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName TrueButtonPressed = "TrueButtonPressed";

		public new static readonly StringName _Input = "_Input";
	}

	public new class PropertyName : DialogPopup.PropertyName
	{
		public static readonly StringName spriteNode = "spriteNode";

		public static readonly StringName sprite = "sprite";
	}

	public new class SignalName : DialogPopup.SignalName
	{
	}

	public Control spriteNode;

	public AdobeAnimateSprite sprite;

	public override void _Ready()
	{
		base._Ready();
		spriteNode = GetNode<Control>("%SpriteNode");
		GetNode<BaseButton>("%TrueButton").Pressed += TrueButtonPressed;
		string randomSpriteName = ResourceManager.Instance.GetRandomSpriteName();
		PackedScene characterSprite = ResourceManager.Instance.GetCharacterSprite(randomSpriteName);
		if (characterSprite != null)
		{
			sprite = characterSprite.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			sprite.LightMask = 0;
			spriteNode.AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public void TrueButtonPressed()
	{
		CloseDialog();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!IsQueuedForDeletion() && inputEvent.IsActionPressed("Pause"))
		{
			GetViewport().SetInputAsHandled();
			CloseDialog();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrueButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
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
		if (method == MethodName.TrueButtonPressed && args.Count == 0)
		{
			TrueButtonPressed();
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
		if (method == MethodName.TrueButtonPressed)
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
		if (name == PropertyName.spriteNode)
		{
			spriteNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
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
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom(in sprite);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.spriteNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.spriteNode, Variant.From(in spriteNode));
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
		if (info.TryGetProperty(PropertyName.sprite, out var value2))
		{
			sprite = value2.As<AdobeAnimateSprite>();
		}
	}
}
