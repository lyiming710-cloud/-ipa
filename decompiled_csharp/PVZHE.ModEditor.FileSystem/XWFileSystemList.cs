using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/GUI/List/XWFileSystemList.cs")]
public class XWFileSystemList : ItemList
{
	[Signal]
	public delegate void ItemActivatedFileEventHandler(XWFileSystemTreeItemData data);

	[Signal]
	public delegate void ItemActivatedFolderEventHandler(string path);

	public new class MethodName : ItemList.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetDisplayMode = "SetDisplayMode";

		public static readonly StringName UpdateIconDisplay = "UpdateIconDisplay";

		public static readonly StringName GetCurrentIconSize = "GetCurrentIconSize";

		public static readonly StringName DisplayDirectory = "DisplayDirectory";

		public static readonly StringName SelectPath = "SelectPath";

		public new static readonly StringName _GetDragData = "_GetDragData";

		public new static readonly StringName _CanDropData = "_CanDropData";

		public new static readonly StringName _DropData = "_DropData";

		public static readonly StringName Search = "Search";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName MatchesFilters = "MatchesFilters";

		public static readonly StringName MatchesFolderFilters = "MatchesFolderFilters";

		public static readonly StringName ApplyFolderIconColor = "ApplyFolderIconColor";

		public static readonly StringName IsProtectedDirectory = "IsProtectedDirectory";

		public static readonly StringName OnItemActivated = "OnItemActivated";

		public static readonly StringName GetDropTargetDirectory = "GetDropTargetDirectory";

		public static readonly StringName GetDropTargetDirectoryAtGlobalPosition = "GetDropTargetDirectoryAtGlobalPosition";

		public static readonly StringName CanMoveDropData = "CanMoveDropData";

		public static readonly StringName MoveDraggedFilesToDirectory = "MoveDraggedFilesToDirectory";

		public static readonly StringName CanMovePathToDirectory = "CanMovePathToDirectory";

		public static readonly StringName MakeUniqueMoveTarget = "MakeUniqueMoveTarget";

		public static readonly StringName NormalizeDirectoryPath = "NormalizeDirectoryPath";

		public static readonly StringName NormalizePath = "NormalizePath";
	}

	public new class PropertyName : ItemList.PropertyName
	{
		public static readonly StringName _currentDir = "_currentDir";

		public static readonly StringName _displayMode = "_displayMode";

		public static readonly StringName _searchText = "_searchText";
	}

	public new class SignalName : ItemList.SignalName
	{
		public static readonly StringName ItemActivatedFile = "ItemActivatedFile";

		public static readonly StringName ItemActivatedFolder = "ItemActivatedFolder";
	}

	private XWFileSystemDirectory _currentDir;

	private XWFileSystemEnum.FileListDisplayMode _displayMode = XWFileSystemEnum.FileListDisplayMode.List;

	private string _searchText = "";

	private const int ListIconSize = 16;

	private const int ThumbnailIconSize = 72;

	private static readonly Color ProtectedFolderIconColor = new Color(0.42f, 0.65f, 1f);

	private static readonly Color NormalFolderIconColor = Colors.White;

	private ItemActivatedFileEventHandler backing_ItemActivatedFile;

	private ItemActivatedFolderEventHandler backing_ItemActivatedFolder;

	public event ItemActivatedFileEventHandler ItemActivatedFile
	{
		add
		{
			backing_ItemActivatedFile = (ItemActivatedFileEventHandler)Delegate.Combine(backing_ItemActivatedFile, value);
		}
		remove
		{
			backing_ItemActivatedFile = (ItemActivatedFileEventHandler)Delegate.Remove(backing_ItemActivatedFile, value);
		}
	}

	public event ItemActivatedFolderEventHandler ItemActivatedFolder
	{
		add
		{
			backing_ItemActivatedFolder = (ItemActivatedFolderEventHandler)Delegate.Combine(backing_ItemActivatedFolder, value);
		}
		remove
		{
			backing_ItemActivatedFolder = (ItemActivatedFolderEventHandler)Delegate.Remove(backing_ItemActivatedFolder, value);
		}
	}

	public override void _Ready()
	{
		ItemActivated += OnItemActivated;
		UpdateIconDisplay();
	}

	public void SetDisplayMode(XWFileSystemEnum.FileListDisplayMode mode)
	{
		_displayMode = mode;
		UpdateIconDisplay();
		Refresh();
	}

	private void UpdateIconDisplay()
	{
		SetIconMode((IconModeEnum)((_displayMode == XWFileSystemEnum.FileListDisplayMode.Thumbnails) ? 0 : 1));
		int currentIconSize = GetCurrentIconSize();
		FixedIconSize = new Vector2I(currentIconSize, currentIconSize);
	}

	private int GetCurrentIconSize()
	{
		if (_displayMode != XWFileSystemEnum.FileListDisplayMode.Thumbnails)
		{
			return 16;
		}
		return 72;
	}

	public void DisplayDirectory(XWFileSystemDirectory dir)
	{
		_currentDir = dir;
		Refresh();
	}

	public bool SelectPath(string path)
	{
		path = NormalizePath(path);
		if (string.IsNullOrEmpty(path))
		{
			return false;
		}
		DeselectAll();
		for (int i = 0; i < ItemCount; i++)
		{
			XWFileSystemTreeItemData xWFileSystemTreeItemData = GetItemMetadata(i).As<XWFileSystemTreeItemData>();
			if (xWFileSystemTreeItemData != null && !(NormalizePath(xWFileSystemTreeItemData.Path) != path))
			{
				Select(i);
				EnsureCurrentIsVisible();
				return true;
			}
		}
		return false;
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		List<string> dragPaths = GetDragPaths(atPosition);
		if (dragPaths.Count == 0)
		{
			return default;
		}
		Dictionary from = BuildFileDragData(dragPaths);
		SetDragPreview(CreateDragPreview(dragPaths));
		return Variant.From(in from);
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		string dropTargetDirectory = GetDropTargetDirectory(atPosition);
		return CanMoveDropData(data, dropTargetDirectory);
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		string dropTargetDirectory = GetDropTargetDirectory(atPosition);
		MoveDraggedFilesToDirectory(data, dropTargetDirectory);
	}

	public void Search(string query)
	{
		_searchText = query ?? "";
		Refresh();
	}

	public void Refresh()
	{
		Clear();
		if (_currentDir == null)
		{
			return;
		}
		foreach (XWFileSystemDirectory subdir in _currentDir.Subdirs)
		{
			XWFileSystemDirectory from = subdir;
			if (MatchesFolderFilters(from))
			{
				string projectFolderPath = XWFileSystem.GetSingleton().ProjectFolderPath;
				string displayName = XWModProjectLayout.GetDisplayName(XWModProjectLayout.ToProjectRelativePath(from.Path, projectFolderPath));
				int num = AddItem(displayName);
				SetItemMetadata(num, Variant.From<XWFileSystemTreeItemData>(new XWFileSystemTreeItemData(from.Path, Variant.From(in from), pIsFolder: true)));
				Texture2D texture2D = XWClassRegistry.Instance?.GetUIIcon("Folder");
				if (texture2D != null)
				{
					SetItemIcon(num, texture2D);
				}
				ApplyFolderIconColor(num, from.Path);
			}
		}
		foreach (XWFileInfo file in _currentDir.Files)
		{
			XWFileInfo from2 = file;
			if (MatchesFilters(from2))
			{
				int idx = AddItem(from2.Name);
				XWFileSystemTreeItemData from3 = new XWFileSystemTreeItemData(from2.Path, Variant.From(in from2), pIsFolder: false)
				{
					FileType = from2.Type,
					ModifiedTime = (long)from2.ModifiedTime
				};
				SetItemMetadata(idx, Variant.From(in from3));
				Texture2D icon = XWFileSystemExtensionRegistry.GetIcon(from3, GetCurrentIconSize());
				if (icon != null)
				{
					SetItemIcon(idx, icon);
				}
			}
		}
	}

	private bool MatchesFilters(XWFileInfo file)
	{
		if (file == null)
		{
			return false;
		}
		if (XWFileSystemCompanionResourcePolicy.ShouldDisplayAsOwnerChildOnly(file.Path))
		{
			return false;
		}
		if (!string.IsNullOrWhiteSpace(_searchText) && !file.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase) && !file.Path.Contains(_searchText, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return true;
	}

	private bool MatchesFolderFilters(XWFileSystemDirectory dir)
	{
		if (dir == null)
		{
			return false;
		}
		if (!string.IsNullOrWhiteSpace(_searchText) && !dir.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase) && !dir.Path.Contains(_searchText, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return true;
	}

	private void ApplyFolderIconColor(int itemIndex, string folderPath)
	{
		SetItemIconModulate(itemIndex, IsProtectedDirectory(folderPath) ? ProtectedFolderIconColor : NormalFolderIconColor);
	}

	private static bool IsProtectedDirectory(string folderPath)
	{
		return XWModProjectLayout.IsProtectedDirectory(folderPath, XWFileSystem.GetSingleton().ProjectFolderPath);
	}

	private void OnItemActivated(long index)
	{
		XWFileSystemTreeItemData xWFileSystemTreeItemData = GetItemMetadata((int)index).As<XWFileSystemTreeItemData>();
		if (xWFileSystemTreeItemData != null)
		{
			if (xWFileSystemTreeItemData.IsFolder)
			{
				EmitSignal(SignalName.ItemActivatedFolder, xWFileSystemTreeItemData.Path);
			}
			else
			{
				EmitSignal(SignalName.ItemActivatedFile, xWFileSystemTreeItemData);
			}
		}
	}

	private List<string> GetDragPaths(Vector2 atPosition)
	{
		List<string> list = new List<string>();
		int itemAtPosition = GetItemAtPosition(atPosition, exact: true);
		if (itemAtPosition >= 0 && !IsSelected(itemAtPosition))
		{
			AddItemPath(list, itemAtPosition);
			return list;
		}
		int[] selectedItems = GetSelectedItems();
		foreach (int index in selectedItems)
		{
			AddItemPath(list, index);
		}
		if (list.Count == 0 && itemAtPosition >= 0)
		{
			AddItemPath(list, itemAtPosition);
		}
		return list;
	}

	private void AddItemPath(List<string> paths, int index)
	{
		if (index >= 0 && index < ItemCount)
		{
			XWFileSystemTreeItemData xWFileSystemTreeItemData = GetItemMetadata(index).As<XWFileSystemTreeItemData>();
			if (xWFileSystemTreeItemData != null && !string.IsNullOrWhiteSpace(xWFileSystemTreeItemData.Path))
			{
				paths.Add(xWFileSystemTreeItemData.Path);
			}
		}
	}

	private static Dictionary BuildFileDragData(List<string> paths)
	{
		string[] array = paths.ToArray();
		bool flag = false;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (DirAccess.DirExistsAbsolute(array2[i]))
			{
				flag = true;
				break;
			}
		}
		return new Dictionary
		{
			["type"] = "files",
			["files"] = array,
			["paths"] = array,
			["path"] = ((array.Length != 0) ? array[0] : ""),
			["has_dirs"] = flag
		};
	}

	private static Control CreateDragPreview(List<string> paths)
	{
		string text = ((paths.Count == 1) ? paths[0].GetFile() : $"{paths.Count} 个项目");
		return new Label
		{
			Text = text
		};
	}

	private string GetDropTargetDirectory(Vector2 atPosition)
	{
		int itemAtPosition = GetItemAtPosition(atPosition, exact: true);
		if (itemAtPosition >= 0 && itemAtPosition < ItemCount)
		{
			XWFileSystemTreeItemData xWFileSystemTreeItemData = GetItemMetadata(itemAtPosition).As<XWFileSystemTreeItemData>();
			if (xWFileSystemTreeItemData != null && xWFileSystemTreeItemData.IsFolder)
			{
				return NormalizeDirectoryPath(xWFileSystemTreeItemData.Path);
			}
		}
		return NormalizeDirectoryPath(_currentDir?.Path ?? "");
	}

	public string GetDropTargetDirectoryAtGlobalPosition(Vector2 globalPosition)
	{
		Vector2 atPosition = GetGlobalTransform().AffineInverse() * globalPosition;
		return GetDropTargetDirectory(atPosition);
	}

	private static bool CanMoveDropData(Variant data, string targetDirectory)
	{
		return XWFileSystemDropHelper.CanDropFilesToDirectory(data, targetDirectory);
	}

	private static void MoveDraggedFilesToDirectory(Variant data, string targetDirectory)
	{
		XWFileSystemDropHelper.DropFilesToDirectory(data, targetDirectory);
	}

	private static List<string> ExtractDropPaths(Variant data)
	{
		List<string> list = new List<string>();
		if (data.VariantType != Variant.Type.Dictionary)
		{
			return list;
		}
		Dictionary dictionary = data.As<Dictionary>();
		if (dictionary.ContainsKey("files"))
		{
			string[] array = dictionary["files"].AsStringArray();
			foreach (string path in array)
			{
				AddDropPath(list, path);
			}
		}
		else if (dictionary.ContainsKey("paths"))
		{
			string[] array = dictionary["paths"].AsStringArray();
			foreach (string path2 in array)
			{
				AddDropPath(list, path2);
			}
		}
		else if (dictionary.ContainsKey("path"))
		{
			AddDropPath(list, dictionary["path"].AsString());
		}
		return list;
	}

	private static void AddDropPath(List<string> paths, string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			paths.Add(NormalizePath(path));
		}
	}

	private static bool CanMovePathToDirectory(string path, string targetDirectory)
	{
		string text = NormalizePath(path);
		targetDirectory = NormalizeDirectoryPath(targetDirectory);
		if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(targetDirectory))
		{
			return false;
		}
		if (!FileAccess.FileExists(text) && !DirAccess.DirExistsAbsolute(text))
		{
			return false;
		}
		if ((DirAccess.DirExistsAbsolute(text) ? NormalizeDirectoryPath(text) : NormalizeDirectoryPath(text.GetBaseDir())) == targetDirectory)
		{
			return false;
		}
		if (DirAccess.DirExistsAbsolute(text) && targetDirectory.StartsWith(NormalizeDirectoryPath(text), StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return true;
	}

	private static string MakeUniqueMoveTarget(XWFileSystem fs, string sourcePath, string targetPath)
	{
		string text = NormalizeDirectoryPath(targetPath.GetBaseDir());
		string file = targetPath.GetFile();
		if (DirAccess.DirExistsAbsolute(sourcePath))
		{
			if (!DirAccess.DirExistsAbsolute(targetPath) && !FileAccess.FileExists(targetPath))
			{
				return targetPath;
			}
			return text.PathJoin(fs.GenerateUniqueFolderName(text, file));
		}
		if (!FileAccess.FileExists(targetPath) && !DirAccess.DirExistsAbsolute(targetPath))
		{
			return targetPath;
		}
		return text.PathJoin(fs.GenerateUniqueFileName(text, file));
	}

	private static string NormalizeDirectoryPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		path = NormalizePath(path);
		if (!path.EndsWith("/"))
		{
			return path + "/";
		}
		return path;
	}

	private static string NormalizePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		return path.Replace('\\', '/').TrimEnd('/');
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(24)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetDisplayMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateIconDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentIconSize, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisplayDirectory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dir", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SelectPath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GetDragData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CanDropData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._DropData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Search, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MatchesFilters, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "file", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.MatchesFolderFilters, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dir", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyFolderIconColor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "itemIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "folderPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsProtectedDirectory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "folderPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnItemActivated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDropTargetDirectory, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDropTargetDirectoryAtGlobalPosition, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanMoveDropData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.String, "targetDirectory", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveDraggedFilesToDirectory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.String, "targetDirectory", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanMovePathToDirectory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "targetDirectory", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeUniqueMoveTarget, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fs", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.String, "sourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "targetPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeDirectoryPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SetDisplayMode && args.Count == 1)
		{
			SetDisplayMode(VariantUtils.ConvertTo<XWFileSystemEnum.FileListDisplayMode>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateIconDisplay && args.Count == 0)
		{
			UpdateIconDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentIconSize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCurrentIconSize());
			return true;
		}
		if (method == MethodName.DisplayDirectory && args.Count == 1)
		{
			DisplayDirectory(VariantUtils.ConvertTo<XWFileSystemDirectory>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SelectPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName._GetDragData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_GetDragData(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName._CanDropData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_CanDropData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._DropData && args.Count == 2)
		{
			_DropData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Search && args.Count == 1)
		{
			Search(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.MatchesFilters && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MatchesFilters(VariantUtils.ConvertTo<XWFileInfo>(in args[0])));
			return true;
		}
		if (method == MethodName.MatchesFolderFilters && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MatchesFolderFilters(VariantUtils.ConvertTo<XWFileSystemDirectory>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyFolderIconColor && args.Count == 2)
		{
			ApplyFolderIconColor(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsProtectedDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProtectedDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnItemActivated && args.Count == 1)
		{
			OnItemActivated(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDropTargetDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDropTargetDirectory(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDropTargetDirectoryAtGlobalPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDropTargetDirectoryAtGlobalPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.CanMoveDropData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanMoveDropData(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MoveDraggedFilesToDirectory && args.Count == 2)
		{
			MoveDraggedFilesToDirectory(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanMovePathToDirectory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanMovePathToDirectory(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeUniqueMoveTarget && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(MakeUniqueMoveTarget(VariantUtils.ConvertTo<XWFileSystem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.NormalizeDirectoryPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeDirectoryPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsProtectedDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProtectedDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanMoveDropData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanMoveDropData(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MoveDraggedFilesToDirectory && args.Count == 2)
		{
			MoveDraggedFilesToDirectory(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanMovePathToDirectory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanMovePathToDirectory(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeUniqueMoveTarget && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(MakeUniqueMoveTarget(VariantUtils.ConvertTo<XWFileSystem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.NormalizeDirectoryPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeDirectoryPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.SetDisplayMode)
		{
			return true;
		}
		if (method == MethodName.UpdateIconDisplay)
		{
			return true;
		}
		if (method == MethodName.GetCurrentIconSize)
		{
			return true;
		}
		if (method == MethodName.DisplayDirectory)
		{
			return true;
		}
		if (method == MethodName.SelectPath)
		{
			return true;
		}
		if (method == MethodName._GetDragData)
		{
			return true;
		}
		if (method == MethodName._CanDropData)
		{
			return true;
		}
		if (method == MethodName._DropData)
		{
			return true;
		}
		if (method == MethodName.Search)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.MatchesFilters)
		{
			return true;
		}
		if (method == MethodName.MatchesFolderFilters)
		{
			return true;
		}
		if (method == MethodName.ApplyFolderIconColor)
		{
			return true;
		}
		if (method == MethodName.IsProtectedDirectory)
		{
			return true;
		}
		if (method == MethodName.OnItemActivated)
		{
			return true;
		}
		if (method == MethodName.GetDropTargetDirectory)
		{
			return true;
		}
		if (method == MethodName.GetDropTargetDirectoryAtGlobalPosition)
		{
			return true;
		}
		if (method == MethodName.CanMoveDropData)
		{
			return true;
		}
		if (method == MethodName.MoveDraggedFilesToDirectory)
		{
			return true;
		}
		if (method == MethodName.CanMovePathToDirectory)
		{
			return true;
		}
		if (method == MethodName.MakeUniqueMoveTarget)
		{
			return true;
		}
		if (method == MethodName.NormalizeDirectoryPath)
		{
			return true;
		}
		if (method == MethodName.NormalizePath)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._currentDir)
		{
			_currentDir = VariantUtils.ConvertTo<XWFileSystemDirectory>(in value);
			return true;
		}
		if (name == PropertyName._displayMode)
		{
			_displayMode = VariantUtils.ConvertTo<XWFileSystemEnum.FileListDisplayMode>(in value);
			return true;
		}
		if (name == PropertyName._searchText)
		{
			_searchText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._currentDir)
		{
			value = VariantUtils.CreateFrom(in _currentDir);
			return true;
		}
		if (name == PropertyName._displayMode)
		{
			value = VariantUtils.CreateFrom(in _displayMode);
			return true;
		}
		if (name == PropertyName._searchText)
		{
			value = VariantUtils.CreateFrom(in _searchText);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._currentDir, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._displayMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._searchText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._currentDir, Variant.From(in _currentDir));
		info.AddProperty(PropertyName._displayMode, Variant.From(in _displayMode));
		info.AddProperty(PropertyName._searchText, Variant.From(in _searchText));
		info.AddSignalEventDelegate(SignalName.ItemActivatedFile, backing_ItemActivatedFile);
		info.AddSignalEventDelegate(SignalName.ItemActivatedFolder, backing_ItemActivatedFolder);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._currentDir, out var value))
		{
			_currentDir = value.As<XWFileSystemDirectory>();
		}
		if (info.TryGetProperty(PropertyName._displayMode, out var value2))
		{
			_displayMode = value2.As<XWFileSystemEnum.FileListDisplayMode>();
		}
		if (info.TryGetProperty(PropertyName._searchText, out var value3))
		{
			_searchText = value3.As<string>();
		}
		if (info.TryGetSignalEventDelegate<ItemActivatedFileEventHandler>(SignalName.ItemActivatedFile, out var value4))
		{
			backing_ItemActivatedFile = value4;
		}
		if (info.TryGetSignalEventDelegate<ItemActivatedFolderEventHandler>(SignalName.ItemActivatedFolder, out var value5))
		{
			backing_ItemActivatedFolder = value5;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(SignalName.ItemActivatedFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(SignalName.ItemActivatedFolder, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalItemActivatedFile(XWFileSystemTreeItemData data)
	{
		EmitSignal(SignalName.ItemActivatedFile, new ReadOnlySpan<Variant>((Variant)data));
	}

	protected void EmitSignalItemActivatedFolder(string path)
	{
		EmitSignal(SignalName.ItemActivatedFolder, new ReadOnlySpan<Variant>((Variant)path));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ItemActivatedFile && args.Count == 1)
		{
			backing_ItemActivatedFile?.Invoke(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
		}
		else if (signal == SignalName.ItemActivatedFolder && args.Count == 1)
		{
			backing_ItemActivatedFolder?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ItemActivatedFile)
		{
			return true;
		}
		if (signal == SignalName.ItemActivatedFolder)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
