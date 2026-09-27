using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/GUI/Dialog/XWRemoveConfirmDialog.cs")]
public class XWRemoveConfirmDialog : ConfirmationDialog
{
	public new class MethodName : ConfirmationDialog.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PopupFor = "PopupFor";

		public static readonly StringName BuildFileTree = "BuildFileTree";

		public static readonly StringName BuildTree = "BuildTree";

		public static readonly StringName OnConfirmed = "OnConfirmed";
	}

	public new class PropertyName : ConfirmationDialog.PropertyName
	{
		public static readonly StringName _messageLabel = "_messageLabel";

		public static readonly StringName _fileTree = "_fileTree";

		public static readonly StringName _targetPath = "_targetPath";
	}

	public new class SignalName : ConfirmationDialog.SignalName
	{
	}

	private Label _messageLabel;

	private Tree _fileTree;

	private string _targetPath = "";

	public override void _Ready()
	{
		_messageLabel = GetNode<Label>("%MessageLabel");
		_fileTree = GetNode<Tree>("%FileTree");
		Confirmed += OnConfirmed;
	}

	public void PopupFor(string path)
	{
		if (XWModProjectLayout.IsProtectedDirectory(path, XWFileSystem.GetSingleton().ProjectFolderPath))
		{
			_targetPath = "";
			Hide();
			return;
		}
		_targetPath = path;
		_messageLabel.Text = "确认删除：\n" + path + "\n\n该操作会移到回收站。";
		BuildFileTree(path);
		PopupCentered();
	}

	private void BuildFileTree(string path)
	{
		_fileTree.Clear();
		TreeItem treeItem = _fileTree.CreateItem();
		treeItem.SetText(0, path);
		XWFileSystemDirectory filesystemPath = XWFileSystem.GetSingleton().GetFilesystemPath(path);
		if (filesystemPath != null)
		{
			BuildTree(treeItem, filesystemPath);
		}
	}

	private void BuildTree(TreeItem parent, XWFileSystemDirectory dir)
	{
		foreach (XWFileSystemDirectory subdir in dir.Subdirs)
		{
			TreeItem treeItem = _fileTree.CreateItem(parent);
			treeItem.SetText(0, subdir.Name);
			BuildTree(treeItem, subdir);
		}
		foreach (XWFileInfo file in dir.Files)
		{
			_fileTree.CreateItem(parent).SetText(0, file.Name);
		}
	}

	private void OnConfirmed()
	{
		if (!string.IsNullOrEmpty(_targetPath) && !XWModProjectLayout.IsProtectedDirectory(_targetPath, XWFileSystem.GetSingleton().ProjectFolderPath))
		{
			XWFileSystem.GetSingleton().RemoveFile(_targetPath);
			_targetPath = "";
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopupFor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildFileTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Object, "dir", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.PopupFor && args.Count == 1)
		{
			PopupFor(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildFileTree && args.Count == 1)
		{
			BuildFileTree(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTree && args.Count == 2)
		{
			BuildTree(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<XWFileSystemDirectory>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnConfirmed && args.Count == 0)
		{
			OnConfirmed();
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
		if (method == MethodName.PopupFor)
		{
			return true;
		}
		if (method == MethodName.BuildFileTree)
		{
			return true;
		}
		if (method == MethodName.BuildTree)
		{
			return true;
		}
		if (method == MethodName.OnConfirmed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._messageLabel)
		{
			_messageLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._fileTree)
		{
			_fileTree = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._targetPath)
		{
			_targetPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._messageLabel)
		{
			value = VariantUtils.CreateFrom(in _messageLabel);
			return true;
		}
		if (name == PropertyName._fileTree)
		{
			value = VariantUtils.CreateFrom(in _fileTree);
			return true;
		}
		if (name == PropertyName._targetPath)
		{
			value = VariantUtils.CreateFrom(in _targetPath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._messageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fileTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._targetPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._messageLabel, Variant.From(in _messageLabel));
		info.AddProperty(PropertyName._fileTree, Variant.From(in _fileTree));
		info.AddProperty(PropertyName._targetPath, Variant.From(in _targetPath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._messageLabel, out var value))
		{
			_messageLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._fileTree, out var value2))
		{
			_fileTree = value2.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._targetPath, out var value3))
		{
			_targetPath = value3.As<string>();
		}
	}
}
