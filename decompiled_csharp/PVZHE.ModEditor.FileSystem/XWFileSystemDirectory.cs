using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/RefCounted/XWFileSystemDirectory.cs")]
public class XWFileSystemDirectory : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName GetSubdirCount = "GetSubdirCount";

		public static readonly StringName GetSubdir = "GetSubdir";

		public static readonly StringName GetFileCount = "GetFileCount";

		public static readonly StringName GetFile = "GetFile";

		public static readonly StringName FindDirIndex = "FindDirIndex";

		public static readonly StringName FindFileIndex = "FindFileIndex";

		public static readonly StringName GetSubdirByName = "GetSubdirByName";

		public static readonly StringName GetFileByName = "GetFileByName";

		public static readonly StringName AddSubdir = "AddSubdir";

		public static readonly StringName RemoveSubdir = "RemoveSubdir";

		public static readonly StringName AddFile = "AddFile";

		public static readonly StringName RemoveFile = "RemoveFile";

		public static readonly StringName SortFiles = "SortFiles";

		public static readonly StringName SortSubdirs = "SortSubdirs";

		public static readonly StringName FindFileInTree = "FindFileInTree";

		public static readonly StringName FindDirectoryInTree = "FindDirectoryInTree";

		public static readonly StringName Clear = "Clear";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Name = "Name";

		public static readonly StringName Path = "Path";

		public static readonly StringName ModifiedTime = "ModifiedTime";

		public static readonly StringName Parent = "Parent";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public string Name { get; set; } = "";

	public string Path { get; set; } = "";

	public ulong ModifiedTime { get; set; }

	public XWFileSystemDirectory Parent { get; set; }

	public List<XWFileSystemDirectory> Subdirs { get; } = new List<XWFileSystemDirectory>();

	public List<XWFileInfo> Files { get; } = new List<XWFileInfo>();

	public XWFileSystemDirectory()
	{
	}

	public XWFileSystemDirectory(string pName, string pPath)
	{
		Name = pName;
		Path = pPath;
	}

	public int GetSubdirCount()
	{
		return Subdirs.Count;
	}

	public XWFileSystemDirectory GetSubdir(int idx)
	{
		if (idx < 0 || idx >= Subdirs.Count)
		{
			return null;
		}
		return Subdirs[idx];
	}

	public int GetFileCount()
	{
		return Files.Count;
	}

	public XWFileInfo GetFile(int idx)
	{
		if (idx < 0 || idx >= Files.Count)
		{
			return null;
		}
		return Files[idx];
	}

	public int FindDirIndex(string dirName)
	{
		for (int i = 0; i < Subdirs.Count; i++)
		{
			if (Subdirs[i].Name == dirName)
			{
				return i;
			}
		}
		return -1;
	}

	public int FindFileIndex(string fileName)
	{
		for (int i = 0; i < Files.Count; i++)
		{
			if (Files[i].Name == fileName)
			{
				return i;
			}
		}
		return -1;
	}

	public XWFileSystemDirectory GetSubdirByName(string name)
	{
		int num = FindDirIndex(name);
		if (num < 0)
		{
			return null;
		}
		return Subdirs[num];
	}

	public XWFileInfo GetFileByName(string name)
	{
		int num = FindFileIndex(name);
		if (num < 0)
		{
			return null;
		}
		return Files[num];
	}

	public void AddSubdir(XWFileSystemDirectory subdir)
	{
		subdir.Parent = this;
		Subdirs.Add(subdir);
	}

	public void RemoveSubdir(int idx)
	{
		if (idx >= 0 && idx < Subdirs.Count)
		{
			Subdirs.RemoveAt(idx);
		}
	}

	public void AddFile(XWFileInfo file)
	{
		Files.Add(file);
	}

	public void RemoveFile(int idx)
	{
		if (idx >= 0 && idx < Files.Count)
		{
			Files.RemoveAt(idx);
		}
	}

	public void SortFiles(XWFileSystemEnum.SortMode sortMode)
	{
		XWFileInfo.SortList(Files, sortMode);
		foreach (XWFileSystemDirectory subdir in Subdirs)
		{
			subdir.SortFiles(sortMode);
		}
	}

	public void SortSubdirs(XWFileSystemEnum.SortMode sortMode)
	{
		switch (sortMode)
		{
		case XWFileSystemEnum.SortMode.NameAsc:
			Subdirs.Sort((XWFileSystemDirectory a, XWFileSystemDirectory b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
			break;
		case XWFileSystemEnum.SortMode.NameDesc:
			Subdirs.Sort((XWFileSystemDirectory a, XWFileSystemDirectory b) => -string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
			break;
		default:
			Subdirs.Sort((XWFileSystemDirectory a, XWFileSystemDirectory b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
			break;
		}
		foreach (XWFileSystemDirectory subdir in Subdirs)
		{
			subdir.SortSubdirs(sortMode);
		}
	}

	public XWFileInfo FindFileInTree(string filePath)
	{
		foreach (XWFileInfo file in Files)
		{
			if (file.Path == filePath)
			{
				return file;
			}
		}
		foreach (XWFileSystemDirectory subdir in Subdirs)
		{
			XWFileInfo xWFileInfo = subdir.FindFileInTree(filePath);
			if (xWFileInfo != null)
			{
				return xWFileInfo;
			}
		}
		return null;
	}

	public XWFileSystemDirectory FindDirectoryInTree(string dirPath)
	{
		if (Path == dirPath)
		{
			return this;
		}
		foreach (XWFileSystemDirectory subdir in Subdirs)
		{
			XWFileSystemDirectory xWFileSystemDirectory = subdir.FindDirectoryInTree(dirPath);
			if (xWFileSystemDirectory != null)
			{
				return xWFileSystemDirectory;
			}
		}
		return null;
	}

	public List<string> GetAllFilePaths()
	{
		List<string> list = new List<string>();
		foreach (XWFileInfo file in Files)
		{
			list.Add(file.Path);
		}
		foreach (XWFileSystemDirectory subdir in Subdirs)
		{
			list.AddRange(subdir.GetAllFilePaths());
		}
		return list;
	}

	public List<string> GetAllDirPaths()
	{
		List<string> list = new List<string> { Path };
		foreach (XWFileSystemDirectory subdir in Subdirs)
		{
			list.AddRange(subdir.GetAllDirPaths());
		}
		return list;
	}

	public void Clear()
	{
		Subdirs.Clear();
		Files.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName.GetSubdirCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSubdir, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "idx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFileCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "idx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindDirIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dirName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindFileIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSubdirByName, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFileByName, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddSubdir, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "subdir", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSubdir, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "idx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "file", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "idx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SortFiles, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "sortMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SortSubdirs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "sortMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindFileInTree, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindDirectoryInTree, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dirPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetSubdirCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetSubdirCount());
			return true;
		}
		if (method == MethodName.GetSubdir && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWFileSystemDirectory>(GetSubdir(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFileCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetFileCount());
			return true;
		}
		if (method == MethodName.GetFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWFileInfo>(GetFile(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.FindDirIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindDirIndex(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindFileIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindFileIndex(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSubdirByName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWFileSystemDirectory>(GetSubdirByName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFileByName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWFileInfo>(GetFileByName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AddSubdir && args.Count == 1)
		{
			AddSubdir(VariantUtils.ConvertTo<XWFileSystemDirectory>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSubdir && args.Count == 1)
		{
			RemoveSubdir(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddFile && args.Count == 1)
		{
			AddFile(VariantUtils.ConvertTo<XWFileInfo>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveFile && args.Count == 1)
		{
			RemoveFile(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SortFiles && args.Count == 1)
		{
			SortFiles(VariantUtils.ConvertTo<XWFileSystemEnum.SortMode>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SortSubdirs && args.Count == 1)
		{
			SortSubdirs(VariantUtils.ConvertTo<XWFileSystemEnum.SortMode>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindFileInTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWFileInfo>(FindFileInTree(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindDirectoryInTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWFileSystemDirectory>(FindDirectoryInTree(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetSubdirCount)
		{
			return true;
		}
		if (method == MethodName.GetSubdir)
		{
			return true;
		}
		if (method == MethodName.GetFileCount)
		{
			return true;
		}
		if (method == MethodName.GetFile)
		{
			return true;
		}
		if (method == MethodName.FindDirIndex)
		{
			return true;
		}
		if (method == MethodName.FindFileIndex)
		{
			return true;
		}
		if (method == MethodName.GetSubdirByName)
		{
			return true;
		}
		if (method == MethodName.GetFileByName)
		{
			return true;
		}
		if (method == MethodName.AddSubdir)
		{
			return true;
		}
		if (method == MethodName.RemoveSubdir)
		{
			return true;
		}
		if (method == MethodName.AddFile)
		{
			return true;
		}
		if (method == MethodName.RemoveFile)
		{
			return true;
		}
		if (method == MethodName.SortFiles)
		{
			return true;
		}
		if (method == MethodName.SortSubdirs)
		{
			return true;
		}
		if (method == MethodName.FindFileInTree)
		{
			return true;
		}
		if (method == MethodName.FindDirectoryInTree)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Name)
		{
			Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Path)
		{
			Path = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ModifiedTime)
		{
			ModifiedTime = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.Parent)
		{
			Parent = VariantUtils.ConvertTo<XWFileSystemDirectory>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.Name)
		{
			from = Name;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Path)
		{
			from = Path;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ModifiedTime)
		{
			value = VariantUtils.CreateFrom<ulong>(ModifiedTime);
			return true;
		}
		if (name == PropertyName.Parent)
		{
			value = VariantUtils.CreateFrom<XWFileSystemDirectory>(Parent);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Path, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ModifiedTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Parent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
		info.AddProperty(PropertyName.Path, Variant.From<string>(Path));
		info.AddProperty(PropertyName.ModifiedTime, Variant.From<ulong>(ModifiedTime));
		info.AddProperty(PropertyName.Parent, Variant.From<XWFileSystemDirectory>(Parent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Name, out var value))
		{
			Name = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Path, out var value2))
		{
			Path = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ModifiedTime, out var value3))
		{
			ModifiedTime = value3.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.Parent, out var value4))
		{
			Parent = value4.As<XWFileSystemDirectory>();
		}
	}
}
