using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/DialogBox/DialogBoxGeneral/DialogBoxRenameUser/DialogBoxRenameUser.cs")]
public class DialogBoxRenameUser : DialogPopup
{
	public new class MethodName : DialogPopup.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ReadyButtonPressed = "ReadyButtonPressed";

		public static readonly StringName CancelButtonPressed = "CancelButtonPressed";
	}

	public new class PropertyName : DialogPopup.PropertyName
	{
		public static readonly StringName changeUser = "changeUser";

		public static readonly StringName nameLineEdit = "nameLineEdit";

		public static readonly StringName readyButton = "readyButton";

		public static readonly StringName cancelButton = "cancelButton";

		public static readonly StringName _changeUser = "_changeUser";
	}

	public new class SignalName : DialogPopup.SignalName
	{
	}

	public LineEdit nameLineEdit;

	public MainButton readyButton;

	public MainButton cancelButton;

	private string _changeUser = "";

	public string changeUser
	{
		get
		{
			return _changeUser;
		}
		set
		{
			_changeUser = value;
			if (nameLineEdit != null)
			{
				nameLineEdit.Text = value;
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		nameLineEdit = GetNode<LineEdit>("%NameLineEdit");
		readyButton = GetNode<MainButton>("%ReadyButton");
		cancelButton = GetNode<MainButton>("%CancelButton");
		GetNode<BaseButton>("%ReadyButton").Pressed += ReadyButtonPressed;
		GetNode<BaseButton>("%CancelButton").Pressed += CancelButtonPressed;
		nameLineEdit.Text = changeUser;
	}

	public void ReadyButtonPressed()
	{
		string text = nameLineEdit.Text;
		if (text.Length > 0 && !GameSaveManager.Instance.HasUser(text))
		{
			GameSaveManager.Instance.RenameUser(changeUser, text);
			GameSaveManager.Instance.SetUserCurrent(text);
			CloseDialog();
		}
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
			new MethodInfo(MethodName.ReadyButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ReadyButtonPressed && args.Count == 0)
		{
			ReadyButtonPressed();
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
		if (method == MethodName.ReadyButtonPressed)
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
		if (name == PropertyName.changeUser)
		{
			changeUser = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.nameLineEdit)
		{
			nameLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName.readyButton)
		{
			readyButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.cancelButton)
		{
			cancelButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._changeUser)
		{
			_changeUser = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.changeUser)
		{
			value = VariantUtils.CreateFrom<string>(changeUser);
			return true;
		}
		if (name == PropertyName.nameLineEdit)
		{
			value = VariantUtils.CreateFrom(in nameLineEdit);
			return true;
		}
		if (name == PropertyName.readyButton)
		{
			value = VariantUtils.CreateFrom(in readyButton);
			return true;
		}
		if (name == PropertyName.cancelButton)
		{
			value = VariantUtils.CreateFrom(in cancelButton);
			return true;
		}
		if (name == PropertyName._changeUser)
		{
			value = VariantUtils.CreateFrom(in _changeUser);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.nameLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.readyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cancelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._changeUser, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.changeUser, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.changeUser, Variant.From<string>(changeUser));
		info.AddProperty(PropertyName.nameLineEdit, Variant.From(in nameLineEdit));
		info.AddProperty(PropertyName.readyButton, Variant.From(in readyButton));
		info.AddProperty(PropertyName.cancelButton, Variant.From(in cancelButton));
		info.AddProperty(PropertyName._changeUser, Variant.From(in _changeUser));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.changeUser, out var value))
		{
			changeUser = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.nameLineEdit, out var value2))
		{
			nameLineEdit = value2.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName.readyButton, out var value3))
		{
			readyButton = value3.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.cancelButton, out var value4))
		{
			cancelButton = value4.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._changeUser, out var value5))
		{
			_changeUser = value5.As<string>();
		}
	}
}
