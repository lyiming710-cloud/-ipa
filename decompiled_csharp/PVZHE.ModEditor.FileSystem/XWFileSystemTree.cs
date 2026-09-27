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

[ScriptPath("res://addons/ModEditor/FileSystem/GUI/Tree/XWFileSystemTree.cs")]
public class XWFileSystemTree : Tree
{
	[Signal]
	public delegate void ItemActivatedFileEventHandler(XWFileSystemTreeItemData data);

	[Signal]
	public delegate void ItemActivatedFolderEventHandler(string path);

	[Signal]
	public delegate void FavoriteActivatedEventHandler(string path);

	[Signal]
	public delegate void InlineRenameSubmittedEventHandler(string oldPath, string newName);

	private sealed class SubResourceEntry
	{
		public string DisplayName { get; set; } = "";

		public string PropertyPath { get; set; } = "";

		public Resource Resource { get; set; }
	}

	private sealed class CompanionResourceEntry
	{
		public string Path { get; set; } = "";

		public string DisplayName { get; set; } = "";

		public string Kind { get; set; } = "";
	}

	public new class MethodName : Tree.MethodName
	{
		public static readonly StringName IsFavoritesRoot = "IsFavoritesRoot";

		public static readonly StringName IsFavoritesItem = "IsFavoritesItem";

		public static readonly StringName GetFavoritePath = "GetFavoritePath";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Init = "Init";

		public static readonly StringName Search = "Search";

		public static readonly StringName BeginInlineRenamePath = "BeginInlineRenamePath";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName OnInlineRenameEdited = "OnInlineRenameEdited";

		public static readonly StringName FindItemByPath = "FindItemByPath";

		public static readonly StringName BuildFavoritesItems = "BuildFavoritesItems";

		public static readonly StringName BuildTree = "BuildTree";

		public static readonly StringName BuildCompanionResourceTree = "BuildCompanionResourceTree";

		public static readonly StringName GetCharacterConfigPath = "GetCharacterConfigPath";

		public static readonly StringName GetCharacterPackageNameFromScenePath = "GetCharacterPackageNameFromScenePath";

		public static readonly StringName GetCharacterPackageDirectory = "GetCharacterPackageDirectory";

		public static readonly StringName GetLegacyCharacterPackageDirectory = "GetLegacyCharacterPackageDirectory";

		public static readonly StringName ResolveCompanionPath = "ResolveCompanionPath";

		public static readonly StringName GetCompanionResourceIcon = "GetCompanionResourceIcon";

		public static readonly StringName BuildSubResourceTree = "BuildSubResourceTree";

		public static readonly StringName LoadResourceForSubResourceTree = "LoadResourceForSubResourceTree";

		public static readonly StringName CanSafelyLoadForSubResourceTree = "CanSafelyLoadForSubResourceTree";

		public static readonly StringName ShouldSkipExternalResourceForSubResourceTree = "ShouldSkipExternalResourceForSubResourceTree";

		public static readonly StringName ShouldSkipExternalSceneForSubResourceTree = "ShouldSkipExternalSceneForSubResourceTree";

		public static readonly StringName HasUnsafeExternalSceneDependencies = "HasUnsafeExternalSceneDependencies";

		public static readonly StringName HasMissingTextResourceDependencies = "HasMissingTextResourceDependencies";

		public static readonly StringName IsSceneResourcePath = "IsSceneResourcePath";

		public static readonly StringName IsAbsoluteFilePath = "IsAbsoluteFilePath";

		public static readonly StringName CollapsePath = "CollapsePath";

		public static readonly StringName IsAdobeAnimateDataResourceText = "IsAdobeAnimateDataResourceText";

		public static readonly StringName ShouldSkipSubResourceProperty = "ShouldSkipSubResourceProperty";

		public static readonly StringName FormatSubResourceName = "FormatSubResourceName";

		public static readonly StringName FormatVariantKey = "FormatVariantKey";

		public static readonly StringName GetSubResourceIcon = "GetSubResourceIcon";

		public static readonly StringName GetResourceClassName = "GetResourceClassName";

		public static readonly StringName OnItemActivated = "OnItemActivated";

		public new static readonly StringName _GetDragData = "_GetDragData";

		public new static readonly StringName _CanDropData = "_CanDropData";

		public new static readonly StringName _DropData = "_DropData";

		public static readonly StringName GetDropTargetDirectory = "GetDropTargetDirectory";

		public static readonly StringName GetDropTargetDirectoryAtGlobalPosition = "GetDropTargetDirectoryAtGlobalPosition";

		public static readonly StringName CanMoveDropData = "CanMoveDropData";

		public static readonly StringName MoveDraggedFilesToDirectory = "MoveDraggedFilesToDirectory";

		public static readonly StringName CanMovePathToDirectory = "CanMovePathToDirectory";

		public static readonly StringName MakeUniqueMoveTarget = "MakeUniqueMoveTarget";

		public static readonly StringName NormalizeDirectoryPath = "NormalizeDirectoryPath";

		public static readonly StringName NormalizePath = "NormalizePath";

		public static readonly StringName ApplyFilter = "ApplyFilter";

		public static readonly StringName CanFilterTreeItem = "CanFilterTreeItem";

		public static readonly StringName GetFolderIcon = "GetFolderIcon";

		public static readonly StringName ApplyFolderIconColor = "ApplyFolderIconColor";

		public static readonly StringName IsProtectedDirectory = "IsProtectedDirectory";

		public static readonly StringName GetFileIcon = "GetFileIcon";

		public static readonly StringName GetFavoritesIcon = "GetFavoritesIcon";
	}

	public new class PropertyName : Tree.PropertyName
	{
		public static readonly StringName FavoritesItem = "FavoritesItem";

		public static readonly StringName _root = "_root";

		public static readonly StringName _favoritesItem = "_favoritesItem";

		public static readonly StringName _resTreeItem = "_resTreeItem";

		public static readonly StringName _inlineRenameItem = "_inlineRenameItem";

		public static readonly StringName _inlineRenamePath = "_inlineRenamePath";

		public static readonly StringName _inlineRenameOriginalText = "_inlineRenameOriginalText";

		public static readonly StringName _projectFolderPath = "_projectFolderPath";

		public static readonly StringName _filterText = "_filterText";
	}

	public new class SignalName : Tree.SignalName
	{
		public static readonly StringName ItemActivatedFile = "ItemActivatedFile";

		public static readonly StringName ItemActivatedFolder = "ItemActivatedFolder";

		public static readonly StringName FavoriteActivated = "FavoriteActivated";

		public static readonly StringName InlineRenameSubmitted = "InlineRenameSubmitted";
	}

	private TreeItem _root;

	private TreeItem _favoritesItem;

	private TreeItem _resTreeItem;

	private TreeItem _inlineRenameItem;

	private string _inlineRenamePath = "";

	private string _inlineRenameOriginalText = "";

	private string _projectFolderPath = "";

	private string _filterText = "";

	private const int MaxSubResourceDepth = 8;

	private static readonly Color ProtectedFolderIconColor = new Color(0.42f, 0.65f, 1f);

	private static readonly Color NormalFolderIconColor = Colors.White;

	private static readonly string[] CompanionExtensions = new string[12]
	{
		"dat", "json", "cfg", "txt", "png", "jpg", "jpeg", "webp", "svg", "wav",
		"ogg", "mp3"
	};

	private ItemActivatedFileEventHandler backing_ItemActivatedFile;

	private ItemActivatedFolderEventHandler backing_ItemActivatedFolder;

	private FavoriteActivatedEventHandler backing_FavoriteActivated;

	private InlineRenameSubmittedEventHandler backing_InlineRenameSubmitted;

	public TreeItem FavoritesItem => _favoritesItem;

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

	public event FavoriteActivatedEventHandler FavoriteActivated
	{
		add
		{
			backing_FavoriteActivated = (FavoriteActivatedEventHandler)Delegate.Combine(backing_FavoriteActivated, value);
		}
		remove
		{
			backing_FavoriteActivated = (FavoriteActivatedEventHandler)Delegate.Remove(backing_FavoriteActivated, value);
		}
	}

	public event InlineRenameSubmittedEventHandler InlineRenameSubmitted
	{
		add
		{
			backing_InlineRenameSubmitted = (InlineRenameSubmittedEventHandler)Delegate.Combine(backing_InlineRenameSubmitted, value);
		}
		remove
		{
			backing_InlineRenameSubmitted = (InlineRenameSubmittedEventHandler)Delegate.Remove(backing_InlineRenameSubmitted, value);
		}
	}

	public bool IsFavoritesRoot(TreeItem item)
	{
		if (item != null)
		{
			return item == _favoritesItem;
		}
		return false;
	}

	public bool IsFavoritesItem(TreeItem item)
	{
		if (item == null || _favoritesItem == null)
		{
			return false;
		}
		return item.GetParent() == _favoritesItem;
	}

	public string GetFavoritePath(TreeItem item)
	{
		if (!IsFavoritesItem(item))
		{
			return "";
		}
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType != Variant.Type.String)
		{
			return "";
		}
		return metadata.AsString();
	}

	public override void _Ready()
	{
		ItemActivated += OnItemActivated;
		ItemEdited += OnInlineRenameEdited;
		SetSelectMode(SelectModeEnum.Multi);
		SetAllowRmbSelect(allow: true);
		SetHideRoot(enable: true);
	}

	public override void _ExitTree()
	{
		if (XWFileSystem.Instance != null)
		{
			XWFileSystem.Instance.FilesystemChanged -= Refresh;
		}
		base._ExitTree();
	}

	public void Init(string path)
	{
		_projectFolderPath = path;
		XWFileSystem singleton = XWFileSystem.GetSingleton();
		singleton.FilesystemChanged -= Refresh;
		singleton.FilesystemChanged += Refresh;
		singleton.SetProjectFolderPath(path);
	}

	public void Search(string query)
	{
		_filterText = query;
		Refresh();
	}

	public bool BeginInlineRenamePath(string path)
	{
		path = NormalizePath(path);
		if (string.IsNullOrWhiteSpace(path) || XWModProjectLayout.IsProtectedDirectory(path, _projectFolderPath))
		{
			return false;
		}
		TreeItem treeItem = FindItemByPath(_root, path);
		if (treeItem == null || IsFavoritesRoot(treeItem) || IsFavoritesItem(treeItem))
		{
			return false;
		}
		XWFileSystemTreeItemData xWFileSystemTreeItemData = treeItem.GetMetadata(0).As<XWFileSystemTreeItemData>();
		if (xWFileSystemTreeItemData == null || xWFileSystemTreeItemData.IsSubResource)
		{
			return false;
		}
		_inlineRenameItem?.SetEditable(0, enabled: false);
		_inlineRenameItem = treeItem;
		_inlineRenamePath = xWFileSystemTreeItemData.Path;
		_inlineRenameOriginalText = treeItem.GetText(0);
		DeselectAll();
		treeItem.Select(0);
		ScrollToItem(treeItem);
		treeItem.SetEditable(0, enabled: true);
		EditSelected(forceEdit: true);
		return true;
	}

	public void Refresh()
	{
		List<string> uncollapsedPaths = GetUncollapsedPaths();
		Clear();
		XWFileSystem singleton = XWFileSystem.GetSingleton();
		XWFileSystemDirectory from = singleton.GetFilesystem();
		if (from != null)
		{
			_root = CreateItem();
			_favoritesItem = CreateItem(_root);
			_favoritesItem.SetText(0, "收藏夹:");
			_favoritesItem.SetIcon(0, GetFavoritesIcon());
			_favoritesItem.SetIconMaxWidth(0, 16);
			BuildFavoritesItems(singleton);
			_resTreeItem = CreateItem(_root);
			_resTreeItem.SetText(0, "res://");
			XWFileSystemTreeItemData from2 = new XWFileSystemTreeItemData(from.Path, Variant.From(in from), pIsFolder: true);
			_resTreeItem.SetMetadata(0, Variant.From(in from2));
			_resTreeItem.SetIcon(0, GetFolderIcon());
			_resTreeItem.SetIconMaxWidth(0, 16);
			ApplyFolderIconColor(_resTreeItem, from.Path);
			BuildTree(_resTreeItem, from);
			RestoreCollapsedPaths(uncollapsedPaths);
			if (!string.IsNullOrEmpty(_filterText))
			{
				ApplyFilter();
			}
		}
	}

	private void OnInlineRenameEdited()
	{
		TreeItem edited = GetEdited();
		if (_inlineRenameItem != null && edited == _inlineRenameItem)
		{
			string inlineRenamePath = _inlineRenamePath;
			string text = edited.GetText(0).StripEdges();
			edited.SetEditable(0, enabled: false);
			edited.SetText(0, _inlineRenameOriginalText);
			_inlineRenameItem = null;
			_inlineRenamePath = "";
			_inlineRenameOriginalText = "";
			EmitSignal(SignalName.InlineRenameSubmitted, inlineRenamePath, text);
		}
	}

	private static TreeItem FindItemByPath(TreeItem item, string path)
	{
		if (item == null || string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Object)
		{
			XWFileSystemTreeItemData xWFileSystemTreeItemData = metadata.As<XWFileSystemTreeItemData>();
			if (xWFileSystemTreeItemData != null && string.Equals(NormalizePath(xWFileSystemTreeItemData.Path), path, StringComparison.OrdinalIgnoreCase))
			{
				return item;
			}
		}
		for (TreeItem treeItem = item.GetFirstChild(); treeItem != null; treeItem = treeItem.GetNext())
		{
			TreeItem treeItem2 = FindItemByPath(treeItem, path);
			if (treeItem2 != null)
			{
				return treeItem2;
			}
		}
		return null;
	}

	private void BuildFavoritesItems(XWFileSystem fs)
	{
		foreach (string favorite in fs.Favorites)
		{
			if (!string.IsNullOrEmpty(favorite))
			{
				string text;
				Texture2D texture;
				if (favorite.EndsWith("/"))
				{
					string file = favorite.Substr(0, favorite.Length - 1).GetFile();
					text = (string.IsNullOrEmpty(file) ? "/" : file);
					texture = GetFolderIcon();
				}
				else
				{
					text = favorite.GetFile();
					texture = GetFileIcon();
				}
				TreeItem treeItem = CreateItem(_favoritesItem);
				treeItem.SetText(0, text);
				treeItem.SetIcon(0, texture);
				treeItem.SetIconMaxWidth(0, 16);
				if (favorite.EndsWith("/"))
				{
					ApplyFolderIconColor(treeItem, favorite);
				}
				treeItem.SetTooltipText(0, favorite);
				treeItem.SetMetadata(0, favorite);
				treeItem.SetCustomColor(0, new Color(1f, 0.85f, 0.4f));
			}
		}
	}

	private void BuildTree(TreeItem parentItem, XWFileSystemDirectory dir)
	{
		foreach (XWFileSystemDirectory subdir in dir.Subdirs)
		{
			XWFileSystemDirectory from = subdir;
			TreeItem treeItem = CreateItem(parentItem);
			treeItem.SetText(0, XWModProjectLayout.GetDisplayName(XWModProjectLayout.ToProjectRelativePath(from.Path, _projectFolderPath)));
			treeItem.SetMetadata(0, Variant.From<XWFileSystemTreeItemData>(new XWFileSystemTreeItemData(from.Path, Variant.From(in from), pIsFolder: true)));
			treeItem.SetIcon(0, GetFolderIcon());
			treeItem.SetIconMaxWidth(0, 16);
			ApplyFolderIconColor(treeItem, from.Path);
			BuildTree(treeItem, from);
		}
		foreach (XWFileInfo file in dir.Files)
		{
			XWFileInfo from2 = file;
			if (!XWFileSystemCompanionResourcePolicy.ShouldDisplayAsOwnerChildOnly(from2.Path))
			{
				TreeItem treeItem2 = CreateItem(parentItem);
				treeItem2.SetText(0, from2.Name);
				XWFileSystemTreeItemData from3 = new XWFileSystemTreeItemData(from2.Path, Variant.From(in from2), pIsFolder: false)
				{
					FileType = from2.Type,
					ModifiedTime = (long)from2.ModifiedTime
				};
				treeItem2.SetMetadata(0, Variant.From(in from3));
				Texture2D icon = XWFileSystemExtensionRegistry.GetIcon(from3);
				treeItem2.SetIcon(0, icon ?? GetFileIcon());
				treeItem2.SetIconMaxWidth(0, 16);
				Resource resource = LoadResourceForSubResourceTree(from3.Path);
				BuildSubResourceTree(treeItem2, from3, resource);
				BuildCompanionResourceTree(treeItem2, from3, resource);
			}
		}
	}

	private void BuildCompanionResourceTree(TreeItem parentItem, XWFileSystemTreeItemData ownerData, Resource resource)
	{
		if (parentItem == null || ownerData == null || ownerData.IsFolder || ownerData.IsSubResource || ownerData.IsCompanionResource)
		{
			return;
		}
		foreach (CompanionResourceEntry item in CollectCompanionResources(ownerData.Path, resource))
		{
			XWFileSystemTreeItemData from = new XWFileSystemTreeItemData(item.Path, Variant.From<string>(item.Path), pIsFolder: false)
			{
				IsCompanionResource = true,
				CompanionOwnerPath = ownerData.Path,
				CompanionKind = item.Kind,
				CompanionDisplayName = item.DisplayName
			};
			TreeItem treeItem = CreateItem(parentItem);
			treeItem.SetText(0, from.GetDisplayName());
			treeItem.SetTooltipText(0, ownerData.Path + "\n" + item.Path);
			treeItem.SetMetadata(0, Variant.From(in from));
			treeItem.SetIcon(0, GetCompanionResourceIcon(from));
			treeItem.SetIconMaxWidth(0, 16);
			treeItem.SetCustomColor(0, new Color(0.88f, 0.78f, 0.55f));
		}
	}

	private List<CompanionResourceEntry> CollectCompanionResources(string ownerPath, Resource resource)
	{
		List<CompanionResourceEntry> result = new List<CompanionResourceEntry>();
		HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		CollectAdobeAnimateCompanionResources(result, seen, ownerPath);
		CollectPropertyCompanionResources(result, seen, ownerPath, resource);
		CollectSameBaseCompanionResources(result, seen, ownerPath);
		CollectCharacterPackageCompanionResources(result, seen, ownerPath);
		return result;
	}

	private void CollectAdobeAnimateCompanionResources(List<CompanionResourceEntry> result, HashSet<string> seen, string ownerPath)
	{
		if (XWFileSystemCompanionResourcePolicy.TryResolveAdobeAnimateDatPath(ownerPath, out var datPath))
		{
			AddCompanionResource(result, seen, ownerPath, datPath, "animeFile");
		}
	}

	private void CollectPropertyCompanionResources(List<CompanionResourceEntry> result, HashSet<string> seen, string ownerPath, Resource resource)
	{
		if (resource == null)
		{
			return;
		}
		foreach (Dictionary property in resource.GetPropertyList())
		{
			if (property.ContainsKey("name"))
			{
				string text = property["name"].AsString();
				if (!string.IsNullOrWhiteSpace(text))
				{
					CollectVariantCompanionResources(result, seen, ownerPath, resource.Get(text), text);
				}
			}
		}
	}

	private void CollectVariantCompanionResources(List<CompanionResourceEntry> result, HashSet<string> seen, string ownerPath, Variant value, string propertyPath)
	{
		switch (value.VariantType)
		{
		case Variant.Type.String:
		case Variant.Type.StringName:
		{
			string text = ResolveCompanionPath(ownerPath, value.AsString());
			if (!string.IsNullOrWhiteSpace(text))
			{
				AddCompanionResource(result, seen, ownerPath, text, propertyPath);
			}
			break;
		}
		case Variant.Type.Array:
		{
			Godot.Collections.Array array = value.As<Godot.Collections.Array>();
			for (int i = 0; i < array.Count; i++)
			{
				CollectVariantCompanionResources(result, seen, ownerPath, array[i], $"{propertyPath}[{i}]");
			}
			break;
		}
		case Variant.Type.Dictionary:
		{
			Dictionary dictionary = value.As<Dictionary>();
			{
				foreach (Variant key in dictionary.Keys)
				{
					CollectVariantCompanionResources(result, seen, ownerPath, dictionary[key], propertyPath + "[" + FormatVariantKey(key) + "]");
				}
				break;
			}
		}
		}
	}

	private void CollectSameBaseCompanionResources(List<CompanionResourceEntry> result, HashSet<string> seen, string ownerPath)
	{
		if (string.IsNullOrWhiteSpace(ownerPath))
		{
			return;
		}
		string baseDir = ownerPath.GetBaseDir();
		string baseName = ownerPath.GetFile().GetBaseName();
		string[] companionExtensions = CompanionExtensions;
		foreach (string text in companionExtensions)
		{
			string text2 = baseDir.PathJoin(baseName + "." + text);
			if (!string.Equals(text2, ownerPath, StringComparison.OrdinalIgnoreCase) && FileAccess.FileExists(text2))
			{
				AddCompanionResource(result, seen, ownerPath, text2, text);
			}
		}
	}

	private void CollectCharacterPackageCompanionResources(List<CompanionResourceEntry> result, HashSet<string> seen, string ownerPath)
	{
		string characterPackageDirectory = GetCharacterPackageDirectory(ownerPath);
		if (!string.IsNullOrWhiteSpace(characterPackageDirectory))
		{
			string characterConfigPath = GetCharacterConfigPath(characterPackageDirectory, ownerPath);
			if (FileAccess.FileExists(characterConfigPath))
			{
				AddCompanionResource(result, seen, ownerPath, characterConfigPath, "配置");
			}
			AddCompanionDirectoryResources(result, seen, ownerPath, characterPackageDirectory, "角色子资源");
		}
	}

	private static string GetCharacterConfigPath(string packageDir, string ownerPath)
	{
		if (string.IsNullOrWhiteSpace(packageDir))
		{
			return "";
		}
		string instance = packageDir.PathJoin("Config");
		string characterPackageNameFromScenePath = GetCharacterPackageNameFromScenePath(ownerPath);
		if (!string.IsNullOrWhiteSpace(characterPackageNameFromScenePath))
		{
			string text = instance.PathJoin(characterPackageNameFromScenePath + "Config.tres");
			if (FileAccess.FileExists(text))
			{
				return text;
			}
		}
		string text2 = instance.PathJoin("Config.tres");
		if (FileAccess.FileExists(text2))
		{
			return text2;
		}
		string text3 = packageDir.PathJoin("Config.tres");
		if (!FileAccess.FileExists(text3))
		{
			return "";
		}
		return text3;
	}

	private static string GetCharacterPackageNameFromScenePath(string ownerPath)
	{
		if (string.IsNullOrWhiteSpace(ownerPath))
		{
			return "";
		}
		string text = ownerPath.Replace('\\', '/').TrimEnd('/');
		string baseName = text.GetFile().GetBaseName();
		if (!string.IsNullOrWhiteSpace(baseName))
		{
			return baseName;
		}
		string characterPackageDirectory = GetCharacterPackageDirectory(text);
		if (!string.IsNullOrWhiteSpace(characterPackageDirectory))
		{
			return characterPackageDirectory.GetFile();
		}
		return "";
	}

	private void AddCompanionDirectoryResources(List<CompanionResourceEntry> result, HashSet<string> seen, string ownerPath, string directoryPath, string kind)
	{
		DirAccess dirAccess = DirAccess.Open(directoryPath);
		if (dirAccess == null)
		{
			return;
		}
		string[] directories = dirAccess.GetDirectories();
		foreach (string text in directories)
		{
			if (!text.StartsWith("."))
			{
				AddCompanionDirectoryResources(result, seen, ownerPath, directoryPath.PathJoin(text), text);
			}
		}
		directories = dirAccess.GetFiles();
		foreach (string text2 in directories)
		{
			if (!text2.StartsWith(".") && !text2.EndsWith(".import"))
			{
				string text3 = directoryPath.PathJoin(text2);
				string baseDir = text3.Replace('\\', '/').Replace(GetCharacterPackageDirectory(ownerPath).TrimSuffix("/") + "/", "").GetBaseDir();
				AddCompanionResource(result, seen, ownerPath, text3, string.IsNullOrWhiteSpace(baseDir) ? kind : baseDir);
			}
		}
	}

	private static string GetCharacterPackageDirectory(string ownerPath)
	{
		if (string.IsNullOrWhiteSpace(ownerPath))
		{
			return "";
		}
		string text = ownerPath.Replace('\\', '/');
		string text2 = text.GetExtension().ToLowerInvariant();
		if ((!(text2 == "tscn") && !(text2 == "scn")) || 1 == 0)
		{
			return "";
		}
		if (!text.Contains("/Resources/Characters/", StringComparison.OrdinalIgnoreCase) && !text.StartsWith("Resources/Characters/", StringComparison.OrdinalIgnoreCase))
		{
			return "";
		}
		string baseDir = text.GetBaseDir();
		string baseDir2 = baseDir.GetBaseDir();
		if (string.Equals(baseDir, baseDir2.PathJoin("Scene"), StringComparison.OrdinalIgnoreCase) && DirAccess.DirExistsAbsolute(baseDir2))
		{
			return baseDir2;
		}
		return GetLegacyCharacterPackageDirectory(text);
	}

	private static string GetLegacyCharacterPackageDirectory(string normalizedOwnerPath)
	{
		string text = normalizedOwnerPath.GetBaseDir().PathJoin(normalizedOwnerPath.GetFile().GetBaseName());
		if (!DirAccess.DirExistsAbsolute(text))
		{
			return "";
		}
		return text;
	}

	private static string ResolveCompanionPath(string ownerPath, string rawPath)
	{
		if (string.IsNullOrWhiteSpace(rawPath) || rawPath.StartsWith("uid://", StringComparison.OrdinalIgnoreCase))
		{
			return "";
		}
		string text = rawPath.Replace('\\', '/');
		if (FileAccess.FileExists(text))
		{
			return text;
		}
		if (text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || text.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			return "";
		}
		string text2 = ownerPath.GetBaseDir().PathJoin(text);
		if (!FileAccess.FileExists(text2))
		{
			return "";
		}
		return text2;
	}

	private static void AddCompanionResource(List<CompanionResourceEntry> result, HashSet<string> seen, string ownerPath, string companionPath, string kind)
	{
		companionPath = companionPath.Replace('\\', '/');
		if (!string.IsNullOrWhiteSpace(companionPath) && !string.Equals(companionPath, ownerPath, StringComparison.OrdinalIgnoreCase) && seen.Add(companionPath))
		{
			string text = ((kind == "animeFile") ? "动画数据" : kind);
			result.Add(new CompanionResourceEntry
			{
				Path = companionPath,
				Kind = text,
				DisplayName = text + ": " + companionPath.GetFile()
			});
		}
	}

	private Texture2D GetCompanionResourceIcon(XWFileSystemTreeItemData data)
	{
		return XWFileSystemExtensionRegistry.GetIcon(data) ?? XWFileSystemExtensionRegistry.GetIconByExtension(data.GetExtension()) ?? XWClassRegistry.Instance.GetUIIcon("File") ?? GetFileIcon();
	}

	private void BuildSubResourceTree(TreeItem parentItem, XWFileSystemTreeItemData ownerData, Resource resource)
	{
		if (parentItem != null && ownerData != null && !ownerData.IsFolder && !ownerData.IsSubResource && resource != null)
		{
			HashSet<Resource> visited = new HashSet<Resource> { resource };
			BuildSubResourceTree(parentItem, ownerData.Path, resource, "", visited, 0);
		}
	}

	private void BuildSubResourceTree(TreeItem parentItem, string ownerPath, Resource resource, string parentPropertyPath, HashSet<Resource> visited, int depth)
	{
		if (parentItem == null || resource == null || depth >= 8)
		{
			return;
		}
		foreach (SubResourceEntry item in CollectSubResources(resource, ownerPath, parentPropertyPath, visited))
		{
			TreeItem treeItem = CreateItem(parentItem);
			XWFileSystemTreeItemData from = new XWFileSystemTreeItemData(ownerPath, Variant.From<Resource>(item.Resource), pIsFolder: false)
			{
				IsSubResource = true,
				SubResourceOwnerPath = ownerPath,
				SubResourcePropertyPath = item.PropertyPath,
				SubResourceDisplayName = item.DisplayName,
				FileType = GetResourceClassName(item.Resource)
			};
			treeItem.SetText(0, from.GetDisplayName());
			treeItem.SetTooltipText(0, ownerPath + "\n" + item.PropertyPath);
			treeItem.SetMetadata(0, Variant.From(in from));
			treeItem.SetIcon(0, GetSubResourceIcon(item.Resource));
			treeItem.SetIconMaxWidth(0, 16);
			treeItem.SetCustomColor(0, new Color(0.72f, 0.82f, 0.95f));
			BuildSubResourceTree(treeItem, ownerPath, item.Resource, item.PropertyPath, visited, depth + 1);
		}
	}

	private static Resource LoadResourceForSubResourceTree(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		bool flag;
		switch (path.GetExtension().ToLowerInvariant())
		{
		case "tres":
		case "res":
		case "tscn":
		case "scn":
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag)
		{
			return null;
		}
		if (!CanSafelyLoadForSubResourceTree(path))
		{
			return null;
		}
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return ResourceLoader.Load<Resource>(path, null, ResourceLoader.CacheMode.Reuse);
	}

	private static bool CanSafelyLoadForSubResourceTree(string path)
	{
		if (ShouldSkipExternalResourceForSubResourceTree(path))
		{
			return false;
		}
		if (IsAdobeAnimateDataResourceText(path))
		{
			return false;
		}
		if (HasMissingTextResourceDependencies(path))
		{
			return false;
		}
		if (HasUnsafeExternalSceneDependencies(path))
		{
			return false;
		}
		return true;
	}

	private static bool ShouldSkipExternalResourceForSubResourceTree(string path)
	{
		string text = path?.Replace('\\', '/') ?? "";
		bool flag;
		switch (text.GetExtension().ToLowerInvariant())
		{
		case "tres":
		case "res":
		case "tscn":
		case "scn":
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag)
		{
			return false;
		}
		return IsAbsoluteFilePath(text);
	}

	private static bool ShouldSkipExternalSceneForSubResourceTree(string path)
	{
		if (!IsSceneResourcePath(path))
		{
			return false;
		}
		return ShouldSkipExternalResourceForSubResourceTree(path);
	}

	private static bool HasUnsafeExternalSceneDependencies(string path)
	{
		bool flag;
		switch (path.GetExtension().ToLowerInvariant())
		{
		case "tres":
		case "res":
		case "tscn":
		case "scn":
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag)
		{
			return false;
		}
		if (!FileAccess.FileExists(path))
		{
			return false;
		}
		string[] array = FileAccess.GetFileAsString(path).Split('\n');
		foreach (string text in array)
		{
			if (text.Contains("[ext_resource", StringComparison.Ordinal) && TryReadQuotedAttribute(text, "path", out var value) && TryResolveTextResourceDependencyPath(path, value, out var resolvedPath) && IsSceneResourcePath(resolvedPath) && IsAbsoluteFilePath(resolvedPath.Replace('\\', '/')))
			{
				GD.PushWarning($"File system tree skipped subresource scan for {path}: external scene dependency {value} ({resolvedPath}).");
				return true;
			}
		}
		return false;
	}

	private static bool HasMissingTextResourceDependencies(string path)
	{
		bool flag;
		switch (path.GetExtension().ToLowerInvariant())
		{
		case "tres":
		case "res":
		case "tscn":
		case "scn":
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag)
		{
			return false;
		}
		if (!FileAccess.FileExists(path))
		{
			return false;
		}
		string[] array = FileAccess.GetFileAsString(path).Split('\n');
		foreach (string text in array)
		{
			if (text.Contains("[ext_resource", StringComparison.Ordinal) && TryReadQuotedAttribute(text, "path", out var value) && !TryResolveTextResourceDependencyPath(path, value, out var resolvedPath))
			{
				GD.PushWarning($"File system tree skipped subresource scan for {path}: missing ext_resource {value} ({resolvedPath}).");
				return true;
			}
		}
		return false;
	}

	private static bool IsSceneResourcePath(string path)
	{
		string text = path.GetExtension().ToLowerInvariant();
		if (text == "tscn" || text == "scn")
		{
			return true;
		}
		return false;
	}

	private static bool TryResolveTextResourceDependencyPath(string ownerPath, string dependencyPath, out string resolvedPath)
	{
		resolvedPath = "";
		if (string.IsNullOrWhiteSpace(dependencyPath))
		{
			return true;
		}
		string text = dependencyPath.Replace('\\', '/');
		if (text.StartsWith("uid://", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (FileAccess.FileExists(text))
		{
			resolvedPath = text;
			return true;
		}
		if (text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || text.StartsWith("user://", StringComparison.OrdinalIgnoreCase) || IsAbsoluteFilePath(text))
		{
			resolvedPath = text;
			return false;
		}
		resolvedPath = CollapsePath(ownerPath.GetBaseDir().PathJoin(text));
		return FileAccess.FileExists(resolvedPath);
	}

	private static bool TryReadQuotedAttribute(string text, string attributeName, out string value)
	{
		value = "";
		string text2 = attributeName + "=\"";
		int num = text.IndexOf(text2, StringComparison.Ordinal);
		if (num < 0)
		{
			return false;
		}
		num += text2.Length;
		int num2 = text.IndexOf('"', num);
		if (num2 < 0)
		{
			return false;
		}
		value = text.Substring(num, num2 - num);
		return true;
	}

	private static bool IsAbsoluteFilePath(string path)
	{
		if (!path.StartsWith("/", StringComparison.Ordinal))
		{
			if (path.Length >= 3 && path[1] == ':')
			{
				return path[2] == '/';
			}
			return false;
		}
		return true;
	}

	private static string CollapsePath(string path)
	{
		path = NormalizePath(path);
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		string text = "";
		string text2 = path;
		if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase))
		{
			text = path.Substring(0, 6);
			string text3 = path;
			text2 = text3.Substring(6, text3.Length - 6);
		}
		else if (path.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			text = path.Substring(0, 7);
			string text3 = path;
			text2 = text3.Substring(7, text3.Length - 7);
		}
		else if (path.Length >= 3 && path[1] == ':' && path[2] == '/')
		{
			text = path.Substring(0, 3);
			string text3 = path;
			text2 = text3.Substring(3, text3.Length - 3);
		}
		else if (path.StartsWith("/", StringComparison.Ordinal))
		{
			text = "/";
			text2 = path.TrimStart('/');
		}
		List<string> list = new List<string>();
		string[] array = text2.Split('/', StringSplitOptions.RemoveEmptyEntries);
		foreach (string text4 in array)
		{
			if (text4 == ".")
			{
				continue;
			}
			if (text4 == "..")
			{
				if (list.Count > 0)
				{
					list.RemoveAt(list.Count - 1);
				}
			}
			else
			{
				list.Add(text4);
			}
		}
		return text + string.Join("/", list);
	}

	private static bool IsAdobeAnimateDataResourceText(string path)
	{
		string text = path.GetExtension().ToLowerInvariant();
		if ((!(text == "tres") && !(text == "res")) || 1 == 0)
		{
			return false;
		}
		if (!FileAccess.FileExists(path))
		{
			return false;
		}
		string fileAsString = FileAccess.GetFileAsString(path);
		if (!fileAsString.Contains("script_class=\"AdobeAnimateData\"", StringComparison.Ordinal) && !fileAsString.Contains("AdobeAnimateData.cs", StringComparison.Ordinal) && !fileAsString.Contains("[gd_resource type=\"AdobeAnimateData\"", StringComparison.Ordinal))
		{
			if (fileAsString.Contains("[resource]\nscript = ExtResource", StringComparison.Ordinal))
			{
				return fileAsString.Contains("AdobeAnimateData", StringComparison.Ordinal);
			}
			return false;
		}
		return true;
	}

	private List<SubResourceEntry> CollectSubResources(Resource resource, string ownerPath, string parentPropertyPath, HashSet<Resource> visited)
	{
		List<SubResourceEntry> result = new List<SubResourceEntry>();
		if (resource == null)
		{
			return result;
		}
		foreach (Dictionary property in resource.GetPropertyList())
		{
			if (property.ContainsKey("name"))
			{
				string text = property["name"].AsString();
				if (!string.IsNullOrWhiteSpace(text) && !ShouldSkipSubResourceProperty(text))
				{
					Variant value = resource.Get(text);
					string propertyPath = (string.IsNullOrEmpty(parentPropertyPath) ? text : (parentPropertyPath + "." + text));
					CollectSubResources(result, value, ownerPath, propertyPath, visited);
				}
			}
		}
		return result;
	}

	private void CollectSubResources(List<SubResourceEntry> result, Variant value, string ownerPath, string propertyPath, HashSet<Resource> visited)
	{
		Variant.Type variantType = value.VariantType;
		Variant.Type num = variantType - 24;
		if ((ulong)num > 4uL)
		{
			return;
		}
		switch ((int)num)
		{
		case 0:
			if (value.As<GodotObject>() is Resource resource && ShouldShowSubResource(ownerPath, resource, visited))
			{
				visited.Add(resource);
				result.Add(new SubResourceEntry
				{
					DisplayName = FormatSubResourceName(propertyPath, resource),
					PropertyPath = propertyPath,
					Resource = resource
				});
			}
			break;
		case 4:
		{
			Godot.Collections.Array array = value.As<Godot.Collections.Array>();
			for (int i = 0; i < array.Count; i++)
			{
				CollectSubResources(result, array[i], ownerPath, $"{propertyPath}[{i}]", visited);
			}
			break;
		}
		case 3:
		{
			Dictionary dictionary = value.As<Dictionary>();
			{
				foreach (Variant key in dictionary.Keys)
				{
					CollectSubResources(result, dictionary[key], ownerPath, propertyPath + "[" + FormatVariantKey(key) + "]", visited);
				}
				break;
			}
		}
		case 1:
		case 2:
			break;
		}
	}

	private static bool ShouldSkipSubResourceProperty(string propertyName)
	{
		if (!(propertyName == "script"))
		{
			return propertyName.StartsWith("resource_", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	private static bool ShouldShowSubResource(string ownerPath, Resource resource, HashSet<Resource> visited)
	{
		if (resource == null || visited.Contains(resource))
		{
			return false;
		}
		string text = (resource.ResourcePath ?? "").Replace('\\', '/');
		if (string.IsNullOrWhiteSpace(text))
		{
			return true;
		}
		return text.Contains("::");
	}

	private static string FormatSubResourceName(string propertyPath, Resource resource)
	{
		string resourceClassName = GetResourceClassName(resource);
		string text = resource?.ResourceName ?? "";
		string text2 = (string.IsNullOrWhiteSpace(text) ? resourceClassName : (text + " : " + resourceClassName));
		return propertyPath + " (" + text2 + ")";
	}

	private static string FormatVariantKey(Variant key)
	{
		string text = ((key.VariantType == Variant.Type.String) ? key.AsString() : key.ToString());
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return "key";
	}

	private Texture2D GetSubResourceIcon(Resource resource)
	{
		string resourceClassName = GetResourceClassName(resource);
		return XWClassRegistry.Instance.GetUIIcon(resourceClassName) ?? XWClassRegistry.Instance.GetUIIcon("Resource") ?? XWClassRegistry.Instance.GetUIIcon("ResourcePreloader") ?? GetFileIcon();
	}

	private static string GetResourceClassName(Resource resource)
	{
		if (resource == null)
		{
			return "";
		}
		string text = resource.GetClass();
		string name = resource.GetType().Name;
		if (string.IsNullOrEmpty(name) || !(name != text))
		{
			return text;
		}
		return name;
	}

	private void OnItemActivated()
	{
		TreeItem selected = GetSelected();
		if (selected == null)
		{
			return;
		}
		Variant metadata = selected.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Nil)
		{
			return;
		}
		if (metadata.VariantType == Variant.Type.String)
		{
			string text = metadata.AsString();
			if (!string.IsNullOrEmpty(text))
			{
				EmitSignal(SignalName.FavoriteActivated, text);
			}
			return;
		}
		XWFileSystemTreeItemData xWFileSystemTreeItemData = metadata.As<XWFileSystemTreeItemData>();
		if (xWFileSystemTreeItemData != null)
		{
			if (xWFileSystemTreeItemData.IsFolder)
			{
				EmitSignal(SignalName.ItemActivatedFolder, xWFileSystemTreeItemData.Path);
				selected.Collapsed = !selected.Collapsed;
			}
			else
			{
				EmitSignal(SignalName.ItemActivatedFile, xWFileSystemTreeItemData);
			}
		}
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		TreeItem itemAtPosition = GetItemAtPosition(atPosition);
		if (itemAtPosition == null || IsFavoritesRoot(itemAtPosition))
		{
			return default;
		}
		List<string> dragPaths = GetDragPaths(itemAtPosition);
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

	private List<string> GetDragPaths(TreeItem originItem)
	{
		List<string> list = new List<string>();
		if (originItem == null)
		{
			return list;
		}
		if (!originItem.IsSelected(0))
		{
			AddTreeItemPath(list, originItem);
			return list;
		}
		for (TreeItem nextSelected = GetNextSelected(null); nextSelected != null; nextSelected = GetNextSelected(nextSelected))
		{
			AddTreeItemPath(list, nextSelected);
		}
		if (list.Count == 0)
		{
			AddTreeItemPath(list, originItem);
		}
		return list;
	}

	private void AddTreeItemPath(List<string> paths, TreeItem item)
	{
		if (item == null || IsFavoritesRoot(item))
		{
			return;
		}
		if (IsFavoritesItem(item))
		{
			string favoritePath = GetFavoritePath(item);
			if (!string.IsNullOrWhiteSpace(favoritePath))
			{
				paths.Add(favoritePath);
			}
			return;
		}
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType != Variant.Type.Nil)
		{
			XWFileSystemTreeItemData xWFileSystemTreeItemData = metadata.As<XWFileSystemTreeItemData>();
			if (xWFileSystemTreeItemData != null && !xWFileSystemTreeItemData.IsSubResource && !string.IsNullOrWhiteSpace(xWFileSystemTreeItemData.Path))
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
		foreach (string text in array2)
		{
			if (text.EndsWith("/") || DirAccess.DirExistsAbsolute(text))
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
		string text = ((paths.Count == 1) ? paths[0].TrimEnd('/').GetFile() : $"{paths.Count} 个项目");
		return new Label
		{
			Text = text
		};
	}

	private string GetDropTargetDirectory(Vector2 atPosition)
	{
		TreeItem itemAtPosition = GetItemAtPosition(atPosition);
		if (itemAtPosition == null || IsFavoritesRoot(itemAtPosition))
		{
			return "";
		}
		if (IsFavoritesItem(itemAtPosition))
		{
			string favoritePath = GetFavoritePath(itemAtPosition);
			if (!DirAccess.DirExistsAbsolute(favoritePath))
			{
				return "";
			}
			return NormalizeDirectoryPath(favoritePath);
		}
		Variant metadata = itemAtPosition.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Nil)
		{
			return "";
		}
		XWFileSystemTreeItemData xWFileSystemTreeItemData = metadata.As<XWFileSystemTreeItemData>();
		if (xWFileSystemTreeItemData == null || xWFileSystemTreeItemData.IsSubResource || xWFileSystemTreeItemData.IsCompanionResource || !xWFileSystemTreeItemData.IsFolder)
		{
			return "";
		}
		return NormalizeDirectoryPath(xWFileSystemTreeItemData.Path);
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

	private void ApplyFilter()
	{
		if (_root == null)
		{
			return;
		}
		string query = _filterText.ToLower();
		for (TreeItem treeItem = _root.GetFirstChild(); treeItem != null; treeItem = treeItem.GetNextInTree())
		{
			if (treeItem.Visible = CanFilterTreeItem(query, treeItem))
			{
				for (TreeItem treeItem2 = treeItem; treeItem2 != null; treeItem2 = treeItem2.GetParent())
				{
					treeItem2.Visible = true;
				}
				treeItem.UncollapseTree();
			}
		}
	}

	private static bool CanFilterTreeItem(string query, TreeItem treeItem)
	{
		string text = treeItem.GetText(0).ToLower();
		string[] array = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		foreach (string value in array)
		{
			if (!text.Contains(value))
			{
				return false;
			}
		}
		return true;
	}

	private List<string> GetUncollapsedPaths()
	{
		List<string> list = new List<string>();
		if (_root == null)
		{
			return list;
		}
		for (TreeItem treeItem = _root.GetFirstChild(); treeItem != null; treeItem = treeItem.GetNext())
		{
			if (treeItem != _favoritesItem)
			{
				CollectUncollapsed(treeItem, list);
			}
		}
		return list;
	}

	private void CollectUncollapsed(TreeItem item, List<string> paths)
	{
		if (item == null)
		{
			return;
		}
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType != Variant.Type.Nil)
		{
			XWFileSystemTreeItemData xWFileSystemTreeItemData = metadata.As<XWFileSystemTreeItemData>();
			if (xWFileSystemTreeItemData != null && xWFileSystemTreeItemData.IsFolder && !item.IsCollapsed())
			{
				paths.Add(xWFileSystemTreeItemData.Path);
			}
		}
		for (TreeItem treeItem = item.GetFirstChild(); treeItem != null; treeItem = treeItem.GetNext())
		{
			CollectUncollapsed(treeItem, paths);
		}
	}

	private void RestoreCollapsedPaths(List<string> paths)
	{
		if (_root == null)
		{
			return;
		}
		for (TreeItem treeItem = _root.GetFirstChild(); treeItem != null; treeItem = treeItem.GetNext())
		{
			if (treeItem != _favoritesItem)
			{
				RestoreCollapsedInChildren(treeItem, paths);
			}
		}
	}

	private void RestoreCollapsedInChildren(TreeItem item, List<string> paths)
	{
		while (item != null)
		{
			Variant metadata = item.GetMetadata(0);
			if (metadata.VariantType != Variant.Type.Nil)
			{
				XWFileSystemTreeItemData xWFileSystemTreeItemData = metadata.As<XWFileSystemTreeItemData>();
				if (xWFileSystemTreeItemData != null && xWFileSystemTreeItemData.IsFolder && !paths.Contains(xWFileSystemTreeItemData.Path))
				{
					item.SetCollapsed(enable: true);
				}
			}
			RestoreCollapsedInChildren(item.GetFirstChild(), paths);
			item = item.GetNext();
		}
	}

	private Texture2D GetFolderIcon()
	{
		return XWClassRegistry.Instance.GetUIIcon("Folder");
	}

	private void ApplyFolderIconColor(TreeItem item, string folderPath)
	{
		item?.SetIconModulate(0, IsProtectedDirectory(folderPath) ? ProtectedFolderIconColor : NormalFolderIconColor);
	}

	private bool IsProtectedDirectory(string folderPath)
	{
		string projectPath = (string.IsNullOrEmpty(_projectFolderPath) ? XWFileSystem.GetSingleton().ProjectFolderPath : _projectFolderPath);
		return XWModProjectLayout.IsProtectedDirectory(folderPath, projectPath);
	}

	private Texture2D GetFileIcon()
	{
		return XWClassRegistry.Instance.GetUIIcon("Filesystem");
	}

	private Texture2D GetFavoritesIcon()
	{
		return XWClassRegistry.Instance.GetUIIcon("Favorites");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(55)
		{
			new MethodInfo(MethodName.IsFavoritesRoot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsFavoritesItem, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetFavoritePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Search, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginInlineRenamePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnInlineRenameEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindItemByPath, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildFavoritesItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fs", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parentItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Object, "dir", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCompanionResourceTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parentItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Object, "ownerData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterConfigPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packageDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "ownerPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterPackageNameFromScenePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "ownerPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCharacterPackageDirectory, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "ownerPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLegacyCharacterPackageDirectory, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "normalizedOwnerPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveCompanionPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "ownerPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "rawPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCompanionResourceIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSubResourceTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parentItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Object, "ownerData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadResourceForSubResourceTree, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanSafelyLoadForSubResourceTree, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldSkipExternalResourceForSubResourceTree, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldSkipExternalSceneForSubResourceTree, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasUnsafeExternalSceneDependencies, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasMissingTextResourceDependencies, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSceneResourcePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsAbsoluteFilePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CollapsePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsAdobeAnimateDataResourceText, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldSkipSubResourceProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatSubResourceName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVariantKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "key", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSubResourceIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetResourceClassName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnItemActivated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			}, null),
			new MethodInfo(MethodName.ApplyFilter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanFilterTreeItem, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "treeItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetFolderIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyFolderIconColor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.String, "folderPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsProtectedDirectory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "folderPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFileIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFavoritesIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsFavoritesRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFavoritesRoot(VariantUtils.ConvertTo<TreeItem>(in args[0])));
			return true;
		}
		if (method == MethodName.IsFavoritesItem && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFavoritesItem(VariantUtils.ConvertTo<TreeItem>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFavoritePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetFavoritePath(VariantUtils.ConvertTo<TreeItem>(in args[0])));
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Search && args.Count == 1)
		{
			Search(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginInlineRenamePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(BeginInlineRenamePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.OnInlineRenameEdited && args.Count == 0)
		{
			OnInlineRenameEdited();
			ret = default;
			return true;
		}
		if (method == MethodName.FindItemByPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TreeItem>(FindItemByPath(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildFavoritesItems && args.Count == 1)
		{
			BuildFavoritesItems(VariantUtils.ConvertTo<XWFileSystem>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTree && args.Count == 2)
		{
			BuildTree(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<XWFileSystemDirectory>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildCompanionResourceTree && args.Count == 3)
		{
			BuildCompanionResourceTree(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[1]), VariantUtils.ConvertTo<Resource>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCharacterConfigPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterConfigPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCharacterPackageNameFromScenePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterPackageNameFromScenePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterPackageDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterPackageDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLegacyCharacterPackageDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetLegacyCharacterPackageDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveCompanionPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveCompanionPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCompanionResourceIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetCompanionResourceIcon(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildSubResourceTree && args.Count == 3)
		{
			BuildSubResourceTree(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[1]), VariantUtils.ConvertTo<Resource>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadResourceForSubResourceTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(LoadResourceForSubResourceTree(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanSafelyLoadForSubResourceTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSafelyLoadForSubResourceTree(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipExternalResourceForSubResourceTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipExternalResourceForSubResourceTree(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipExternalSceneForSubResourceTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipExternalSceneForSubResourceTree(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasUnsafeExternalSceneDependencies && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUnsafeExternalSceneDependencies(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasMissingTextResourceDependencies && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasMissingTextResourceDependencies(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSceneResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSceneResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAbsoluteFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAbsoluteFilePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CollapsePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CollapsePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAdobeAnimateDataResourceText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAdobeAnimateDataResourceText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipSubResourceProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipSubResourceProperty(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatSubResourceName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatSubResourceName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.FormatVariantKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariantKey(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSubResourceIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetSubResourceIcon(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetResourceClassName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourceClassName(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.OnItemActivated && args.Count == 0)
		{
			OnItemActivated();
			ret = default;
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
		if (method == MethodName.ApplyFilter && args.Count == 0)
		{
			ApplyFilter();
			ret = default;
			return true;
		}
		if (method == MethodName.CanFilterTreeItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanFilterTreeItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TreeItem>(in args[1])));
			return true;
		}
		if (method == MethodName.GetFolderIcon && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetFolderIcon());
			return true;
		}
		if (method == MethodName.ApplyFolderIconColor && args.Count == 2)
		{
			ApplyFolderIconColor(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsProtectedDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProtectedDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFileIcon && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetFileIcon());
			return true;
		}
		if (method == MethodName.GetFavoritesIcon && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetFavoritesIcon());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindItemByPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TreeItem>(FindItemByPath(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCharacterConfigPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterConfigPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCharacterPackageNameFromScenePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterPackageNameFromScenePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCharacterPackageDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCharacterPackageDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLegacyCharacterPackageDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetLegacyCharacterPackageDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveCompanionPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveCompanionPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.LoadResourceForSubResourceTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(LoadResourceForSubResourceTree(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanSafelyLoadForSubResourceTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSafelyLoadForSubResourceTree(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipExternalResourceForSubResourceTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipExternalResourceForSubResourceTree(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipExternalSceneForSubResourceTree && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipExternalSceneForSubResourceTree(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasUnsafeExternalSceneDependencies && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUnsafeExternalSceneDependencies(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasMissingTextResourceDependencies && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasMissingTextResourceDependencies(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSceneResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSceneResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAbsoluteFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAbsoluteFilePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CollapsePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CollapsePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAdobeAnimateDataResourceText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAdobeAnimateDataResourceText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipSubResourceProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipSubResourceProperty(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatSubResourceName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatSubResourceName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.FormatVariantKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariantKey(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.GetResourceClassName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourceClassName(VariantUtils.ConvertTo<Resource>(in args[0])));
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
		if (method == MethodName.CanFilterTreeItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanFilterTreeItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TreeItem>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.IsFavoritesRoot)
		{
			return true;
		}
		if (method == MethodName.IsFavoritesItem)
		{
			return true;
		}
		if (method == MethodName.GetFavoritePath)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Search)
		{
			return true;
		}
		if (method == MethodName.BeginInlineRenamePath)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.OnInlineRenameEdited)
		{
			return true;
		}
		if (method == MethodName.FindItemByPath)
		{
			return true;
		}
		if (method == MethodName.BuildFavoritesItems)
		{
			return true;
		}
		if (method == MethodName.BuildTree)
		{
			return true;
		}
		if (method == MethodName.BuildCompanionResourceTree)
		{
			return true;
		}
		if (method == MethodName.GetCharacterConfigPath)
		{
			return true;
		}
		if (method == MethodName.GetCharacterPackageNameFromScenePath)
		{
			return true;
		}
		if (method == MethodName.GetCharacterPackageDirectory)
		{
			return true;
		}
		if (method == MethodName.GetLegacyCharacterPackageDirectory)
		{
			return true;
		}
		if (method == MethodName.ResolveCompanionPath)
		{
			return true;
		}
		if (method == MethodName.GetCompanionResourceIcon)
		{
			return true;
		}
		if (method == MethodName.BuildSubResourceTree)
		{
			return true;
		}
		if (method == MethodName.LoadResourceForSubResourceTree)
		{
			return true;
		}
		if (method == MethodName.CanSafelyLoadForSubResourceTree)
		{
			return true;
		}
		if (method == MethodName.ShouldSkipExternalResourceForSubResourceTree)
		{
			return true;
		}
		if (method == MethodName.ShouldSkipExternalSceneForSubResourceTree)
		{
			return true;
		}
		if (method == MethodName.HasUnsafeExternalSceneDependencies)
		{
			return true;
		}
		if (method == MethodName.HasMissingTextResourceDependencies)
		{
			return true;
		}
		if (method == MethodName.IsSceneResourcePath)
		{
			return true;
		}
		if (method == MethodName.IsAbsoluteFilePath)
		{
			return true;
		}
		if (method == MethodName.CollapsePath)
		{
			return true;
		}
		if (method == MethodName.IsAdobeAnimateDataResourceText)
		{
			return true;
		}
		if (method == MethodName.ShouldSkipSubResourceProperty)
		{
			return true;
		}
		if (method == MethodName.FormatSubResourceName)
		{
			return true;
		}
		if (method == MethodName.FormatVariantKey)
		{
			return true;
		}
		if (method == MethodName.GetSubResourceIcon)
		{
			return true;
		}
		if (method == MethodName.GetResourceClassName)
		{
			return true;
		}
		if (method == MethodName.OnItemActivated)
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
		if (method == MethodName.ApplyFilter)
		{
			return true;
		}
		if (method == MethodName.CanFilterTreeItem)
		{
			return true;
		}
		if (method == MethodName.GetFolderIcon)
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
		if (method == MethodName.GetFileIcon)
		{
			return true;
		}
		if (method == MethodName.GetFavoritesIcon)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._root)
		{
			_root = VariantUtils.ConvertTo<TreeItem>(in value);
			return true;
		}
		if (name == PropertyName._favoritesItem)
		{
			_favoritesItem = VariantUtils.ConvertTo<TreeItem>(in value);
			return true;
		}
		if (name == PropertyName._resTreeItem)
		{
			_resTreeItem = VariantUtils.ConvertTo<TreeItem>(in value);
			return true;
		}
		if (name == PropertyName._inlineRenameItem)
		{
			_inlineRenameItem = VariantUtils.ConvertTo<TreeItem>(in value);
			return true;
		}
		if (name == PropertyName._inlineRenamePath)
		{
			_inlineRenamePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._inlineRenameOriginalText)
		{
			_inlineRenameOriginalText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._projectFolderPath)
		{
			_projectFolderPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._filterText)
		{
			_filterText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.FavoritesItem)
		{
			value = VariantUtils.CreateFrom<TreeItem>(FavoritesItem);
			return true;
		}
		if (name == PropertyName._root)
		{
			value = VariantUtils.CreateFrom(in _root);
			return true;
		}
		if (name == PropertyName._favoritesItem)
		{
			value = VariantUtils.CreateFrom(in _favoritesItem);
			return true;
		}
		if (name == PropertyName._resTreeItem)
		{
			value = VariantUtils.CreateFrom(in _resTreeItem);
			return true;
		}
		if (name == PropertyName._inlineRenameItem)
		{
			value = VariantUtils.CreateFrom(in _inlineRenameItem);
			return true;
		}
		if (name == PropertyName._inlineRenamePath)
		{
			value = VariantUtils.CreateFrom(in _inlineRenamePath);
			return true;
		}
		if (name == PropertyName._inlineRenameOriginalText)
		{
			value = VariantUtils.CreateFrom(in _inlineRenameOriginalText);
			return true;
		}
		if (name == PropertyName._projectFolderPath)
		{
			value = VariantUtils.CreateFrom(in _projectFolderPath);
			return true;
		}
		if (name == PropertyName._filterText)
		{
			value = VariantUtils.CreateFrom(in _filterText);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._root, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._favoritesItem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resTreeItem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineRenameItem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._inlineRenamePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._inlineRenameOriginalText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._projectFolderPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._filterText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.FavoritesItem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._root, Variant.From(in _root));
		info.AddProperty(PropertyName._favoritesItem, Variant.From(in _favoritesItem));
		info.AddProperty(PropertyName._resTreeItem, Variant.From(in _resTreeItem));
		info.AddProperty(PropertyName._inlineRenameItem, Variant.From(in _inlineRenameItem));
		info.AddProperty(PropertyName._inlineRenamePath, Variant.From(in _inlineRenamePath));
		info.AddProperty(PropertyName._inlineRenameOriginalText, Variant.From(in _inlineRenameOriginalText));
		info.AddProperty(PropertyName._projectFolderPath, Variant.From(in _projectFolderPath));
		info.AddProperty(PropertyName._filterText, Variant.From(in _filterText));
		info.AddSignalEventDelegate(SignalName.ItemActivatedFile, backing_ItemActivatedFile);
		info.AddSignalEventDelegate(SignalName.ItemActivatedFolder, backing_ItemActivatedFolder);
		info.AddSignalEventDelegate(SignalName.FavoriteActivated, backing_FavoriteActivated);
		info.AddSignalEventDelegate(SignalName.InlineRenameSubmitted, backing_InlineRenameSubmitted);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._root, out var value))
		{
			_root = value.As<TreeItem>();
		}
		if (info.TryGetProperty(PropertyName._favoritesItem, out var value2))
		{
			_favoritesItem = value2.As<TreeItem>();
		}
		if (info.TryGetProperty(PropertyName._resTreeItem, out var value3))
		{
			_resTreeItem = value3.As<TreeItem>();
		}
		if (info.TryGetProperty(PropertyName._inlineRenameItem, out var value4))
		{
			_inlineRenameItem = value4.As<TreeItem>();
		}
		if (info.TryGetProperty(PropertyName._inlineRenamePath, out var value5))
		{
			_inlineRenamePath = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName._inlineRenameOriginalText, out var value6))
		{
			_inlineRenameOriginalText = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName._projectFolderPath, out var value7))
		{
			_projectFolderPath = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName._filterText, out var value8))
		{
			_filterText = value8.As<string>();
		}
		if (info.TryGetSignalEventDelegate<ItemActivatedFileEventHandler>(SignalName.ItemActivatedFile, out var value9))
		{
			backing_ItemActivatedFile = value9;
		}
		if (info.TryGetSignalEventDelegate<ItemActivatedFolderEventHandler>(SignalName.ItemActivatedFolder, out var value10))
		{
			backing_ItemActivatedFolder = value10;
		}
		if (info.TryGetSignalEventDelegate<FavoriteActivatedEventHandler>(SignalName.FavoriteActivated, out var value11))
		{
			backing_FavoriteActivated = value11;
		}
		if (info.TryGetSignalEventDelegate<InlineRenameSubmittedEventHandler>(SignalName.InlineRenameSubmitted, out var value12))
		{
			backing_InlineRenameSubmitted = value12;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(SignalName.ItemActivatedFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(SignalName.ItemActivatedFolder, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.FavoriteActivated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.InlineRenameSubmitted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "oldPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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

	protected void EmitSignalFavoriteActivated(string path)
	{
		EmitSignal(SignalName.FavoriteActivated, new ReadOnlySpan<Variant>((Variant)path));
	}

	protected void EmitSignalInlineRenameSubmitted(string oldPath, string newName)
	{
		StringName inlineRenameSubmitted = SignalName.InlineRenameSubmitted;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = oldPath;
		buffer[1] = newName;
		EmitSignal(inlineRenameSubmitted, buffer);
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
		else if (signal == SignalName.FavoriteActivated && args.Count == 1)
		{
			backing_FavoriteActivated?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else if (signal == SignalName.InlineRenameSubmitted && args.Count == 2)
		{
			backing_InlineRenameSubmitted?.Invoke(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (signal == SignalName.FavoriteActivated)
		{
			return true;
		}
		if (signal == SignalName.InlineRenameSubmitted)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
