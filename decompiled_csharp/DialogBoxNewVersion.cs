using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/DialogBox/DialogBoxGeneral/DialogBoxNewVersion/DialogBoxNewVersion.cs")]
public class DialogBoxNewVersion : DialogPopup
{
	public new class MethodName : DialogPopup.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName UpdateMessageDisplay = "UpdateMessageDisplay";

		public static readonly StringName AdjustDialogSize = "AdjustDialogSize";

		public static readonly StringName TrueButtonPressed = "TrueButtonPressed";

		public static readonly StringName FalseButtonPressed = "FalseButtonPressed";
	}

	public new class PropertyName : DialogPopup.PropertyName
	{
		public static readonly StringName message = "message";

		public static readonly StringName uri = "uri";

		public static readonly StringName _message = "_message";

		public static readonly StringName marginContainer2 = "marginContainer2";

		public static readonly StringName buttonContainer = "buttonContainer";

		public static readonly StringName messageLabel = "messageLabel";
	}

	public new class SignalName : DialogPopup.SignalName
	{
	}

	public string uri = "";

	private string _message = "";

	private MarginContainer marginContainer2;

	private HBoxContainer buttonContainer;

	private RichTextLabel messageLabel;

	private const float maxDialogHeight = 320f;

	public string message
	{
		get
		{
			return _message;
		}
		set
		{
			_message = value;
			UpdateMessageDisplay();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		marginContainer2 = GetNode<MarginContainer>("Layer/Control/MarginContainer2");
		buttonContainer = GetNode<HBoxContainer>("Layer/Control/ButtonContainer");
		messageLabel = GetNode<RichTextLabel>("%MessageLabel");
		GetNode<BaseButton>("%TrueButton").Pressed += TrueButtonPressed;
		GetNode<BaseButton>("%FalseButton").Pressed += FalseButtonPressed;
		UpdateMessageDisplay();
	}

	private void UpdateMessageDisplay()
	{
		if (messageLabel != null)
		{
			if (!string.IsNullOrEmpty(_message))
			{
				messageLabel.Text = "[center][font_size=16]更新内容：" + _message + "[/font_size][/center]";
				messageLabel.Visible = true;
			}
			else
			{
				messageLabel.Text = "";
				messageLabel.Visible = false;
			}
			CallDeferred("AdjustDialogSize");
		}
	}

	private void AdjustDialogSize()
	{
		if (marginContainer2 != null && buttonContainer != null)
		{
			float num = 25f;
			float num2 = ((buttonContainer.Size.Y > 0f) ? buttonContainer.Size.Y : 41f);
			float num3 = ((messageLabel != null && messageLabel.Visible) ? messageLabel.Size.Y : 0f);
			float num4 = 20f;
			float num5 = 80f;
			float num6 = 60f;
			float a = Mathf.Max((30f + num3 + num + num2 + num4 * 2f + num5 + num6) / 2f, 107f);
			a = Mathf.Min(a, 320f);
			float num7 = marginContainer2.Size.X / 2f;
			marginContainer2.OffsetTop = 0f - a;
			marginContainer2.OffsetBottom = a;
			marginContainer2.OffsetLeft = 0f - num7;
			marginContainer2.OffsetRight = num7;
			float num8 = a - num2 / 2f - num6;
			buttonContainer.OffsetTop = num8 - num2 / 2f;
			buttonContainer.OffsetBottom = num8 + num2 / 2f;
		}
	}

	public void TrueButtonPressed()
	{
		OS.ShellOpen(uri);
		CloseDialog();
	}

	public void FalseButtonPressed()
	{
		CloseDialog();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateMessageDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdjustDialogSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrueButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FalseButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.UpdateMessageDisplay && args.Count == 0)
		{
			UpdateMessageDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.AdjustDialogSize && args.Count == 0)
		{
			AdjustDialogSize();
			ret = default;
			return true;
		}
		if (method == MethodName.TrueButtonPressed && args.Count == 0)
		{
			TrueButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.FalseButtonPressed && args.Count == 0)
		{
			FalseButtonPressed();
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
		if (method == MethodName.UpdateMessageDisplay)
		{
			return true;
		}
		if (method == MethodName.AdjustDialogSize)
		{
			return true;
		}
		if (method == MethodName.TrueButtonPressed)
		{
			return true;
		}
		if (method == MethodName.FalseButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.message)
		{
			message = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.uri)
		{
			uri = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._message)
		{
			_message = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.marginContainer2)
		{
			marginContainer2 = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName.buttonContainer)
		{
			buttonContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.messageLabel)
		{
			messageLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.message)
		{
			value = VariantUtils.CreateFrom<string>(message);
			return true;
		}
		if (name == PropertyName.uri)
		{
			value = VariantUtils.CreateFrom(in uri);
			return true;
		}
		if (name == PropertyName._message)
		{
			value = VariantUtils.CreateFrom(in _message);
			return true;
		}
		if (name == PropertyName.marginContainer2)
		{
			value = VariantUtils.CreateFrom(in marginContainer2);
			return true;
		}
		if (name == PropertyName.buttonContainer)
		{
			value = VariantUtils.CreateFrom(in buttonContainer);
			return true;
		}
		if (name == PropertyName.messageLabel)
		{
			value = VariantUtils.CreateFrom(in messageLabel);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.uri, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._message, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.marginContainer2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.buttonContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.messageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.message, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.message, Variant.From<string>(message));
		info.AddProperty(PropertyName.uri, Variant.From(in uri));
		info.AddProperty(PropertyName._message, Variant.From(in _message));
		info.AddProperty(PropertyName.marginContainer2, Variant.From(in marginContainer2));
		info.AddProperty(PropertyName.buttonContainer, Variant.From(in buttonContainer));
		info.AddProperty(PropertyName.messageLabel, Variant.From(in messageLabel));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.message, out var value))
		{
			message = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.uri, out var value2))
		{
			uri = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName._message, out var value3))
		{
			_message = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.marginContainer2, out var value4))
		{
			marginContainer2 = value4.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName.buttonContainer, out var value5))
		{
			buttonContainer = value5.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.messageLabel, out var value6))
		{
			messageLabel = value6.As<RichTextLabel>();
		}
	}
}
