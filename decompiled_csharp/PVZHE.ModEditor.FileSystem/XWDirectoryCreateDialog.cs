using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/GUI/Dialog/XWDirectoryCreateDialog.cs")]
public class XWDirectoryCreateDialog : ConfirmationDialog
{
	public new class MethodName : ConfirmationDialog.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PopupAt = "PopupAt";

		public static readonly StringName DoCreate = "DoCreate";
	}

	public new class PropertyName : ConfirmationDialog.PropertyName
	{
		public static readonly StringName _basePathLabel = "_basePathLabel";

		public static readonly StringName _dirPathEdit = "_dirPathEdit";

		public static readonly StringName _validationLabel = "_validationLabel";

		public static readonly StringName _basePath = "_basePath";
	}

	public new class SignalName : ConfirmationDialog.SignalName
	{
	}

	private Label _basePathLabel;

	private LineEdit _dirPathEdit;

	private Label _validationLabel;

	private string _basePath = "";

	public override void _Ready()
	{
		_basePathLabel = GetNode<Label>("%BasePathLabel");
		_dirPathEdit = GetNode<LineEdit>("%DirPathEdit");
		_validationLabel = GetNode<Label>("%ValidationLabel");
		_dirPathEdit.TextSubmitted += (string _) =>
		{
			DoCreate();
		};
		Confirmed += DoCreate;
	}

	public void PopupAt(string basePath)
	{
		_basePath = basePath;
		_basePathLabel.Text = basePath;
		_dirPathEdit.Text = "";
		_validationLabel.Hide();
		PopupCentered();
		_dirPathEdit.GrabFocus();
	}

	private void DoCreate()
	{
		string text = _dirPathEdit.Text.Trim();
		if (string.IsNullOrEmpty(text))
		{
			_validationLabel.Text = "名称不能为空";
			_validationLabel.Show();
			return;
		}
		if (text.Contains("/") || text.Contains("\\"))
		{
			_validationLabel.Text = "名称不能包含路径分隔符";
			_validationLabel.Show();
			return;
		}
		_validationLabel.Hide();
		string path = _basePath + text + "/";
		XWFileSystem.GetSingleton().MakeDirRecursive(path);
		Hide();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopupAt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "basePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.PopupAt && args.Count == 1)
		{
			PopupAt(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoCreate && args.Count == 0)
		{
			DoCreate();
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
		if (method == MethodName.PopupAt)
		{
			return true;
		}
		if (method == MethodName.DoCreate)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._basePathLabel)
		{
			_basePathLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._dirPathEdit)
		{
			_dirPathEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._validationLabel)
		{
			_validationLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._basePath)
		{
			_basePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._basePathLabel)
		{
			value = VariantUtils.CreateFrom(in _basePathLabel);
			return true;
		}
		if (name == PropertyName._dirPathEdit)
		{
			value = VariantUtils.CreateFrom(in _dirPathEdit);
			return true;
		}
		if (name == PropertyName._validationLabel)
		{
			value = VariantUtils.CreateFrom(in _validationLabel);
			return true;
		}
		if (name == PropertyName._basePath)
		{
			value = VariantUtils.CreateFrom(in _basePath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._basePathLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._dirPathEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._validationLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._basePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._basePathLabel, Variant.From(in _basePathLabel));
		info.AddProperty(PropertyName._dirPathEdit, Variant.From(in _dirPathEdit));
		info.AddProperty(PropertyName._validationLabel, Variant.From(in _validationLabel));
		info.AddProperty(PropertyName._basePath, Variant.From(in _basePath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._basePathLabel, out var value))
		{
			_basePathLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._dirPathEdit, out var value2))
		{
			_dirPathEdit = value2.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._validationLabel, out var value3))
		{
			_validationLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._basePath, out var value4))
		{
			_basePath = value4.As<string>();
		}
	}
}
