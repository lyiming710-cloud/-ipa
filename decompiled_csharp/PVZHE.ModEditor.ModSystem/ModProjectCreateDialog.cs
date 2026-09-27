using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ModSystem;

[ScriptPath("res://addons/ModEditor/ModSystem/ModProjectCreateDialog.cs")]
public class ModProjectCreateDialog : ConfirmationDialog
{
	public new class MethodName : ConfirmationDialog.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnBrowsePressed = "OnBrowsePressed";

		public static readonly StringName OnDirSelected = "OnDirSelected";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : ConfirmationDialog.PropertyName
	{
		public static readonly StringName SelectedParentDir = "SelectedParentDir";

		public static readonly StringName ModName = "ModName";

		public static readonly StringName ModVersion = "ModVersion";

		public static readonly StringName ModAuthor = "ModAuthor";

		public static readonly StringName ModDescription = "ModDescription";

		public static readonly StringName _nameEdit = "_nameEdit";

		public static readonly StringName _versionEdit = "_versionEdit";

		public static readonly StringName _authorEdit = "_authorEdit";

		public static readonly StringName _descriptionEdit = "_descriptionEdit";

		public static readonly StringName _pathEdit = "_pathEdit";

		public static readonly StringName _browseButton = "_browseButton";

		public static readonly StringName _browseDialog = "_browseDialog";
	}

	public new class SignalName : ConfirmationDialog.SignalName
	{
	}

	private LineEdit _nameEdit;

	private LineEdit _versionEdit;

	private LineEdit _authorEdit;

	private TextEdit _descriptionEdit;

	private LineEdit _pathEdit;

	private Button _browseButton;

	private FileDialog _browseDialog;

	public string SelectedParentDir => _pathEdit?.Text ?? "";

	public string ModName => _nameEdit?.Text ?? "";

	public string ModVersion => _versionEdit?.Text ?? "1.0.0";

	public string ModAuthor => _authorEdit?.Text ?? "未知";

	public string ModDescription => _descriptionEdit?.Text ?? "";

	public override void _Ready()
	{
		_nameEdit = GetNode<LineEdit>("%NameEdit");
		_versionEdit = GetNode<LineEdit>("%VersionEdit");
		_authorEdit = GetNode<LineEdit>("%AuthorEdit");
		_descriptionEdit = GetNode<TextEdit>("%DescriptionEdit");
		_pathEdit = GetNode<LineEdit>("%PathEdit");
		_browseButton = GetNode<Button>("%BrowseButton");
		_browseDialog = GetNode<FileDialog>("%BrowseDialog");
		_browseButton.Pressed += OnBrowsePressed;
		_browseDialog.DirSelected += OnDirSelected;
		_pathEdit.Text = ProjectSettings.GlobalizePath("user://Mods/");
	}

	private void OnBrowsePressed()
	{
		string text = _pathEdit.Text;
		if (!string.IsNullOrEmpty(text) && DirAccess.DirExistsAbsolute(text))
		{
			_browseDialog.CurrentDir = text;
		}
		_browseDialog.PopupCentered();
	}

	private void OnDirSelected(string dir)
	{
		_pathEdit.Text = dir;
	}

	public override void _ExitTree()
	{
		if (_browseButton != null)
		{
			_browseButton.Pressed -= OnBrowsePressed;
		}
		if (_browseDialog != null)
		{
			_browseDialog.DirSelected -= OnDirSelected;
		}
		base._ExitTree();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnBrowsePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDirSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnBrowsePressed && args.Count == 0)
		{
			OnBrowsePressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDirSelected && args.Count == 1)
		{
			OnDirSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
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
		if (method == MethodName.OnBrowsePressed)
		{
			return true;
		}
		if (method == MethodName.OnDirSelected)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._nameEdit)
		{
			_nameEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._versionEdit)
		{
			_versionEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._authorEdit)
		{
			_authorEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._descriptionEdit)
		{
			_descriptionEdit = VariantUtils.ConvertTo<TextEdit>(in value);
			return true;
		}
		if (name == PropertyName._pathEdit)
		{
			_pathEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._browseButton)
		{
			_browseButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._browseDialog)
		{
			_browseDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.SelectedParentDir)
		{
			from = SelectedParentDir;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ModName)
		{
			from = ModName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ModVersion)
		{
			from = ModVersion;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ModAuthor)
		{
			from = ModAuthor;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ModDescription)
		{
			from = ModDescription;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._nameEdit)
		{
			value = VariantUtils.CreateFrom(in _nameEdit);
			return true;
		}
		if (name == PropertyName._versionEdit)
		{
			value = VariantUtils.CreateFrom(in _versionEdit);
			return true;
		}
		if (name == PropertyName._authorEdit)
		{
			value = VariantUtils.CreateFrom(in _authorEdit);
			return true;
		}
		if (name == PropertyName._descriptionEdit)
		{
			value = VariantUtils.CreateFrom(in _descriptionEdit);
			return true;
		}
		if (name == PropertyName._pathEdit)
		{
			value = VariantUtils.CreateFrom(in _pathEdit);
			return true;
		}
		if (name == PropertyName._browseButton)
		{
			value = VariantUtils.CreateFrom(in _browseButton);
			return true;
		}
		if (name == PropertyName._browseDialog)
		{
			value = VariantUtils.CreateFrom(in _browseDialog);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._nameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._versionEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._authorEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._descriptionEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pathEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._browseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._browseDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.SelectedParentDir, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ModName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ModVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ModAuthor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ModDescription, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._nameEdit, Variant.From(in _nameEdit));
		info.AddProperty(PropertyName._versionEdit, Variant.From(in _versionEdit));
		info.AddProperty(PropertyName._authorEdit, Variant.From(in _authorEdit));
		info.AddProperty(PropertyName._descriptionEdit, Variant.From(in _descriptionEdit));
		info.AddProperty(PropertyName._pathEdit, Variant.From(in _pathEdit));
		info.AddProperty(PropertyName._browseButton, Variant.From(in _browseButton));
		info.AddProperty(PropertyName._browseDialog, Variant.From(in _browseDialog));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._nameEdit, out var value))
		{
			_nameEdit = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._versionEdit, out var value2))
		{
			_versionEdit = value2.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._authorEdit, out var value3))
		{
			_authorEdit = value3.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._descriptionEdit, out var value4))
		{
			_descriptionEdit = value4.As<TextEdit>();
		}
		if (info.TryGetProperty(PropertyName._pathEdit, out var value5))
		{
			_pathEdit = value5.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._browseButton, out var value6))
		{
			_browseButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._browseDialog, out var value7))
		{
			_browseDialog = value7.As<FileDialog>();
		}
	}
}
