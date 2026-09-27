using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/DialogBox/DialogBoxGeneral/DialogBoxInput/DialogBoxInput.cs")]
public class DialogBoxInput : DialogPopup
{
	public delegate void ConfirmButtonPressedEventHandler();

	public new class MethodName : DialogPopup.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ConfirmButtonPressed = "ConfirmButtonPressed";

		public static readonly StringName CancelButtonPressed = "CancelButtonPressed";
	}

	public new class PropertyName : DialogPopup.PropertyName
	{
		public static readonly StringName titleText = "titleText";

		public static readonly StringName inputMaxLength = "inputMaxLength";

		public static readonly StringName inputDefaultText = "inputDefaultText";

		public static readonly StringName inputPlaceholder = "inputPlaceholder";

		public static readonly StringName inputText = "inputText";

		public static readonly StringName titleLabel = "titleLabel";

		public static readonly StringName inputLineEdit = "inputLineEdit";

		public static readonly StringName confirmButton = "confirmButton";

		public static readonly StringName cancelButton = "cancelButton";

		public static readonly StringName _titleText = "_titleText";

		public static readonly StringName _inputMaxLength = "_inputMaxLength";

		public static readonly StringName _inputDefaultText = "_inputDefaultText";

		public static readonly StringName _inputPlaceholder = "_inputPlaceholder";
	}

	public new class SignalName : DialogPopup.SignalName
	{
	}

	public RichTextLabel titleLabel;

	public LineEdit inputLineEdit;

	public MainButton confirmButton;

	public MainButton cancelButton;

	private string _titleText = "";

	private int _inputMaxLength = 10;

	private string _inputDefaultText = "";

	private string _inputPlaceholder = "";

	public string titleText
	{
		get
		{
			return _titleText;
		}
		set
		{
			_titleText = value;
			if (IsNodeReady())
			{
				titleLabel.Text = value;
			}
		}
	}

	public int inputMaxLength
	{
		get
		{
			return _inputMaxLength;
		}
		set
		{
			_inputMaxLength = value;
			if (IsNodeReady())
			{
				inputLineEdit.MaxLength = value;
			}
		}
	}

	public string inputDefaultText
	{
		get
		{
			return _inputDefaultText;
		}
		set
		{
			_inputDefaultText = value;
			if (IsNodeReady())
			{
				inputLineEdit.Text = value;
			}
		}
	}

	public string inputPlaceholder
	{
		get
		{
			return _inputPlaceholder;
		}
		set
		{
			_inputPlaceholder = value;
			if (IsNodeReady())
			{
				inputLineEdit.PlaceholderText = value;
			}
		}
	}

	public string inputText => inputLineEdit.Text;

	public event ConfirmButtonPressedEventHandler OnConfirmButtonPressed;

	public override void _Ready()
	{
		base._Ready();
		titleLabel = GetNode<RichTextLabel>("%TitleLabel");
		inputLineEdit = GetNode<LineEdit>("%InputLineEdit");
		confirmButton = GetNode<MainButton>("%ConfirmButton");
		cancelButton = GetNode<MainButton>("%CancelButton");
		GetNode<BaseButton>("%ConfirmButton").Pressed += ConfirmButtonPressed;
		GetNode<BaseButton>("%CancelButton").Pressed += CancelButtonPressed;
		titleLabel.Text = titleText;
		inputLineEdit.MaxLength = inputMaxLength;
		inputLineEdit.Text = inputDefaultText;
		inputLineEdit.PlaceholderText = inputPlaceholder;
	}

	public void ConfirmButtonPressed()
	{
		OnConfirmButtonPressed?.Invoke();
		CloseDialog();
	}

	public void CancelButtonPressed()
	{
		CloseDialog();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfirmButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ConfirmButtonPressed && args.Count == 0)
		{
			ConfirmButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelButtonPressed && args.Count == 0)
		{
			CancelButtonPressed();
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
		if (method == MethodName.ConfirmButtonPressed)
		{
			return true;
		}
		if (method == MethodName.CancelButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.titleText)
		{
			titleText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.inputMaxLength)
		{
			inputMaxLength = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.inputDefaultText)
		{
			inputDefaultText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.inputPlaceholder)
		{
			inputPlaceholder = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.titleLabel)
		{
			titleLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.inputLineEdit)
		{
			inputLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName.confirmButton)
		{
			confirmButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.cancelButton)
		{
			cancelButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._titleText)
		{
			_titleText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._inputMaxLength)
		{
			_inputMaxLength = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._inputDefaultText)
		{
			_inputDefaultText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._inputPlaceholder)
		{
			_inputPlaceholder = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.titleText)
		{
			from = titleText;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.inputMaxLength)
		{
			value = VariantUtils.CreateFrom<int>(inputMaxLength);
			return true;
		}
		if (name == PropertyName.inputDefaultText)
		{
			from = inputDefaultText;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.inputPlaceholder)
		{
			from = inputPlaceholder;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.inputText)
		{
			from = inputText;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.titleLabel)
		{
			value = VariantUtils.CreateFrom(in titleLabel);
			return true;
		}
		if (name == PropertyName.inputLineEdit)
		{
			value = VariantUtils.CreateFrom(in inputLineEdit);
			return true;
		}
		if (name == PropertyName.confirmButton)
		{
			value = VariantUtils.CreateFrom(in confirmButton);
			return true;
		}
		if (name == PropertyName.cancelButton)
		{
			value = VariantUtils.CreateFrom(in cancelButton);
			return true;
		}
		if (name == PropertyName._titleText)
		{
			value = VariantUtils.CreateFrom(in _titleText);
			return true;
		}
		if (name == PropertyName._inputMaxLength)
		{
			value = VariantUtils.CreateFrom(in _inputMaxLength);
			return true;
		}
		if (name == PropertyName._inputDefaultText)
		{
			value = VariantUtils.CreateFrom(in _inputDefaultText);
			return true;
		}
		if (name == PropertyName._inputPlaceholder)
		{
			value = VariantUtils.CreateFrom(in _inputPlaceholder);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.titleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.inputLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.confirmButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cancelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._titleText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.titleText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._inputMaxLength, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.inputMaxLength, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._inputDefaultText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.inputDefaultText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._inputPlaceholder, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.inputPlaceholder, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.inputText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.titleText, Variant.From<string>(titleText));
		info.AddProperty(PropertyName.inputMaxLength, Variant.From<int>(inputMaxLength));
		info.AddProperty(PropertyName.inputDefaultText, Variant.From<string>(inputDefaultText));
		info.AddProperty(PropertyName.inputPlaceholder, Variant.From<string>(inputPlaceholder));
		info.AddProperty(PropertyName.titleLabel, Variant.From(in titleLabel));
		info.AddProperty(PropertyName.inputLineEdit, Variant.From(in inputLineEdit));
		info.AddProperty(PropertyName.confirmButton, Variant.From(in confirmButton));
		info.AddProperty(PropertyName.cancelButton, Variant.From(in cancelButton));
		info.AddProperty(PropertyName._titleText, Variant.From(in _titleText));
		info.AddProperty(PropertyName._inputMaxLength, Variant.From(in _inputMaxLength));
		info.AddProperty(PropertyName._inputDefaultText, Variant.From(in _inputDefaultText));
		info.AddProperty(PropertyName._inputPlaceholder, Variant.From(in _inputPlaceholder));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.titleText, out var value))
		{
			titleText = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.inputMaxLength, out var value2))
		{
			inputMaxLength = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.inputDefaultText, out var value3))
		{
			inputDefaultText = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.inputPlaceholder, out var value4))
		{
			inputPlaceholder = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.titleLabel, out var value5))
		{
			titleLabel = value5.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.inputLineEdit, out var value6))
		{
			inputLineEdit = value6.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName.confirmButton, out var value7))
		{
			confirmButton = value7.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.cancelButton, out var value8))
		{
			cancelButton = value8.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._titleText, out var value9))
		{
			_titleText = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName._inputMaxLength, out var value10))
		{
			_inputMaxLength = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._inputDefaultText, out var value11))
		{
			_inputDefaultText = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName._inputPlaceholder, out var value12))
		{
			_inputPlaceholder = value12.As<string>();
		}
	}
}
