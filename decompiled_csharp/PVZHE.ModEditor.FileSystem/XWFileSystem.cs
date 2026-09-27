using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/XWFileSystem.cs")]
public class XWFileSystem : RefCounted
{
	[Signal]
	public delegate void FilesystemChangedEventHandler();

	[Signal]
	public delegate void ScanStartedEventHandler();

	[Signal]
	public delegate void ScanCompletedEventHandler();

	[Signal]
	public delegate void ScanProgressEventHandler(int current, int total);

	[Signal]
	public delegate void FileChangedEventHandler(string filePath);

	[Signal]
	public delegate void DirectoryChangedEventHandler(string dirPath);

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName GetSingleton = "GetSingleton";

		public static readonly StringName SetProjectFolderPath = "SetProjectFolderPath";

		public static readonly StringName Scan = "Scan";

		public static readonly StringName ScanChanges = "ScanChanges";

		public static readonly StringName GetResourceTypeName = "GetResourceTypeName";

		public static readonly StringName GetFallbackTypeNameByExtension = "GetFallbackTypeNameByExtension";

		public static readonly StringName GetTypeNameFromTextResourceHeader = "GetTypeNameFromTextResourceHeader";

		public static readonly StringName GetQuotedHeaderAttribute = "GetQuotedHeaderAttribute";

		public static readonly StringName ScanDirectory = "ScanDirectory";

		public static readonly StringName UpdateDirectory = "UpdateDirectory";

		public static readonly StringName SortAll = "SortAll";

		public static readonly StringName SetSortMode = "SetSortMode";

		public static readonly StringName GetFilesystem = "GetFilesystem";

		public static readonly StringName GetFilesystemPath = "GetFilesystemPath";

		public static readonly StringName GetFileType = "GetFileType";

		public static readonly StringName UpdateFile = "UpdateFile";

		public static readonly StringName MakeDirRecursive = "MakeDirRecursive";

		public static readonly StringName CopyFile = "CopyFile";

		public static readonly StringName CopyDirectory = "CopyDirectory";

		public static readonly StringName MoveFile = "MoveFile";

		public static readonly StringName SyncCharacterPackageAfterRename = "SyncCharacterPackageAfterRename";

		public static readonly StringName RenameLegacyCharacterConfigIfExists = "RenameLegacyCharacterConfigIfExists";

		public static readonly StringName RenameCharacterPackageFileIfExists = "RenameCharacterPackageFileIfExists";

		public static readonly StringName ReplaceTextInCharacterPackage = "ReplaceTextInCharacterPackage";

		public static readonly StringName RegisterManifestPath = "RegisterManifestPath";

		public static readonly StringName RemoveFile = "RemoveFile";

		public static readonly StringName MovePathToTrash = "MovePathToTrash";

		public static readonly StringName IsPathInsideProject = "IsPathInsideProject";

		public static readonly StringName NormalizePath = "NormalizePath";

		public static readonly StringName AddFavorite = "AddFavorite";

		public static readonly StringName RemoveFavorite = "RemoveFavorite";

		public static readonly StringName IsFavorite = "IsFavorite";

		public static readonly StringName GetValidExtensions = "GetValidExtensions";

		public static readonly StringName ShowExtensionsToArray = "ShowExtensionsToArray";

		public static readonly StringName GenerateUniqueFolderName = "GenerateUniqueFolderName";

		public static readonly StringName GenerateUniqueFileName = "GenerateUniqueFileName";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName RootDirectory = "RootDirectory";

		public static readonly StringName ProjectFolderPath = "ProjectFolderPath";

		public static readonly StringName IsScanning = "IsScanning";

		public static readonly StringName SortMode = "SortMode";

		public static readonly StringName ScanTotal = "ScanTotal";

		public static readonly StringName ScanCurrent = "ScanCurrent";
	}

	public new class SignalName : RefCounted.SignalName
	{
		public static readonly StringName FilesystemChanged = "FilesystemChanged";

		public static readonly StringName ScanStarted = "ScanStarted";

		public static readonly StringName ScanCompleted = "ScanCompleted";

		public static readonly StringName ScanProgress = "ScanProgress";

		public static readonly StringName FileChanged = "FileChanged";

		public static readonly StringName DirectoryChanged = "DirectoryChanged";
	}

	private static string[] _showExtensionsArray;

	private FilesystemChangedEventHandler backing_FilesystemChanged;

	private ScanStartedEventHandler backing_ScanStarted;

	private ScanCompletedEventHandler backing_ScanCompleted;

	private ScanProgressEventHandler backing_ScanProgress;

	private FileChangedEventHandler backing_FileChanged;

	private DirectoryChangedEventHandler backing_DirectoryChanged;

	public static XWFileSystem Instance { get; private set; }

	public XWFileSystemDirectory RootDirectory { get; private set; }

	public string ProjectFolderPath { get; private set; } = "";

	public bool IsScanning { get; private set; }

	public XWFileSystemEnum.SortMode SortMode { get; private set; }

	public List<string> Favorites { get; } = new List<string>();

	public int ScanTotal { get; private set; }

	public int ScanCurrent { get; private set; }

	public event FilesystemChangedEventHandler FilesystemChanged
	{
		add
		{
			backing_FilesystemChanged = (FilesystemChangedEventHandler)Delegate.Combine(backing_FilesystemChanged, value);
		}
		remove
		{
			backing_FilesystemChanged = (FilesystemChangedEventHandler)Delegate.Remove(backing_FilesystemChanged, value);
		}
	}

	public event ScanStartedEventHandler ScanStarted
	{
		add
		{
			backing_ScanStarted = (ScanStartedEventHandler)Delegate.Combine(backing_ScanStarted, value);
		}
		remove
		{
			backing_ScanStarted = (ScanStartedEventHandler)Delegate.Remove(backing_ScanStarted, value);
		}
	}

	public event ScanCompletedEventHandler ScanCompleted
	{
		add
		{
			backing_ScanCompleted = (ScanCompletedEventHandler)Delegate.Combine(backing_ScanCompleted, value);
		}
		remove
		{
			backing_ScanCompleted = (ScanCompletedEventHandler)Delegate.Remove(backing_ScanCompleted, value);
		}
	}

	public event ScanProgressEventHandler ScanProgress
	{
		add
		{
			backing_ScanProgress = (ScanProgressEventHandler)Delegate.Combine(backing_ScanProgress, value);
		}
		remove
		{
			backing_ScanProgress = (ScanProgressEventHandler)Delegate.Remove(backing_ScanProgress, value);
		}
	}

	public event FileChangedEventHandler FileChanged
	{
		add
		{
			backing_FileChanged = (FileChangedEventHandler)Delegate.Combine(backing_FileChanged, value);
		}
		remove
		{
			backing_FileChanged = (FileChangedEventHandler)Delegate.Remove(backing_FileChanged, value);
		}
	}

	public event DirectoryChangedEventHandler DirectoryChanged
	{
		add
		{
			backing_DirectoryChanged = (DirectoryChangedEventHandler)Delegate.Combine(backing_DirectoryChanged, value);
		}
		remove
		{
			backing_DirectoryChanged = (DirectoryChangedEventHandler)Delegate.Remove(backing_DirectoryChanged, value);
		}
	}

	public XWFileSystem()
	{
		XWFileSystemExtensionRegistry.Init();
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public static XWFileSystem GetSingleton()
	{
		if (Instance == null)
		{
			Instance = new XWFileSystem();
		}
		return Instance;
	}

	public void SetProjectFolderPath(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			path = path.Replace('\\', '/');
			if (!path.EndsWith("/"))
			{
				path += "/";
			}
		}
		ProjectFolderPath = path;
		Scan();
	}

	public void Scan()
	{
		if (!string.IsNullOrEmpty(ProjectFolderPath))
		{
			XWFileSystemExtensionRegistry.Init();
			IsScanning = true;
			XWModManifestSyncService.SyncProject(ProjectFolderPath);
			ScanTotal = 0;
			ScanCurrent = 0;
			EmitSignal(SignalName.ScanStarted);
			RootDirectory = new XWFileSystemDirectory("res://", ProjectFolderPath);
			ScanDirectory(RootDirectory, ProjectFolderPath);
			SortAll();
			IsScanning = false;
			EmitSignal(SignalName.ScanCompleted);
			EmitSignal(SignalName.FilesystemChanged);
		}
	}

	public void ScanChanges()
	{
		if (RootDirectory == null)
		{
			Scan();
			return;
		}
		XWModManifestSyncService.SyncProject(ProjectFolderPath);
		UpdateDirectory(RootDirectory, ProjectFolderPath);
		SortAll();
		EmitSignal(SignalName.FilesystemChanged);
	}

	private StringName GetResourceTypeName(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		string text = path.GetExtension().ToLowerInvariant();
		string fallbackTypeNameByExtension = GetFallbackTypeNameByExtension(text);
		if (!string.IsNullOrEmpty(fallbackTypeNameByExtension) && text != "res" && text != "tres")
		{
			return fallbackTypeNameByExtension;
		}
		string typeNameFromTextResourceHeader = GetTypeNameFromTextResourceHeader(path, text);
		if (!string.IsNullOrEmpty(typeNameFromTextResourceHeader))
		{
			return typeNameFromTextResourceHeader;
		}
		return fallbackTypeNameByExtension;
	}

	private static string GetFallbackTypeNameByExtension(string extension)
	{
		switch (extension)
		{
		case "tscn":
		case "scn":
			return "PackedScene";
		case "cs":
			return "CSharpScript";
		case "jpeg":
		case "webp":
		case "png":
		case "jpg":
		case "svg":
			return "Texture2D";
		case "mp3":
		case "wav":
		case "ogg":
			return "AudioStream";
		case "json":
		case "csv":
		case "cfg":
		case "txt":
			return "TextFile";
		default:
			return "";
		}
	}

	private static string GetTypeNameFromTextResourceHeader(string path, string extension)
	{
		if (extension != "tres" && extension != "tscn")
		{
			return "";
		}
		if (!Godot.FileAccess.FileExists(path))
		{
			return "";
		}
		using Godot.FileAccess fileAccess = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
		if (fileAccess == null)
		{
			return "";
		}
		for (int i = 0; i < 8; i++)
		{
			if (fileAccess.EofReached())
			{
				break;
			}
			string text = fileAccess.GetLine().StripEdges();
			if (text.StartsWith("[gd_scene", StringComparison.Ordinal))
			{
				return "PackedScene";
			}
			if (text.StartsWith("[gd_resource", StringComparison.Ordinal))
			{
				return GetQuotedHeaderAttribute(text, "type");
			}
		}
		return "";
	}

	private static string GetQuotedHeaderAttribute(string line, string attributeName)
	{
		string text = attributeName + "=\"";
		int num = line.IndexOf(text, StringComparison.Ordinal);
		if (num < 0)
		{
			return "";
		}
		num += text.Length;
		int num2 = line.IndexOf('"', num);
		if (num2 <= num)
		{
			return "";
		}
		return line.Substring(num, num2 - num);
	}

	private void ScanDirectory(XWFileSystemDirectory pDir, string pPath)
	{
		using DirAccess dirAccess = DirAccess.Open(pPath);
		if (dirAccess == null)
		{
			return;
		}
		dirAccess.ListDirBegin();
		string next = dirAccess.GetNext();
		while (next != "")
		{
			if (dirAccess.CurrentIsDir())
			{
				string pPath2 = pPath + next + "/";
				XWFileSystemDirectory xWFileSystemDirectory = new XWFileSystemDirectory(next, pPath2);
				pDir.AddSubdir(xWFileSystemDirectory);
				ScanDirectory(xWFileSystemDirectory, pPath2);
			}
			else if (XWFileSystemExtensionRegistry.GetCanShowExtension(next.GetExtension()))
			{
				string text = pPath + next;
				XWFileInfo file = new XWFileInfo(next, text)
				{
					ModifiedTime = Godot.FileAccess.GetModifiedTime(text),
					Type = GetResourceTypeName(text)
				};
				pDir.AddFile(file);
			}
			next = dirAccess.GetNext();
			ScanCurrent++;
			if (ScanCurrent % 25 == 0)
			{
				EmitSignal(SignalName.ScanProgress, ScanCurrent, ScanTotal);
			}
		}
		dirAccess.ListDirEnd();
	}

	private void UpdateDirectory(XWFileSystemDirectory pDir, string pPath)
	{
		using DirAccess dirAccess = DirAccess.Open(pPath);
		if (dirAccess == null)
		{
			return;
		}
		Dictionary<string, XWFileSystemDirectory> dictionary = new Dictionary<string, XWFileSystemDirectory>();
		foreach (XWFileSystemDirectory subdir in pDir.Subdirs)
		{
			dictionary[subdir.Name] = subdir;
		}
		Dictionary<string, XWFileInfo> dictionary2 = new Dictionary<string, XWFileInfo>();
		foreach (XWFileInfo file2 in pDir.Files)
		{
			dictionary2[file2.Name] = file2;
		}
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		dirAccess.ListDirBegin();
		string next = dirAccess.GetNext();
		while (next != "")
		{
			if (dirAccess.CurrentIsDir())
			{
				if (!dictionary.ContainsKey(next))
				{
					string pPath2 = pPath + next + "/";
					XWFileSystemDirectory xWFileSystemDirectory = new XWFileSystemDirectory(next, pPath2);
					pDir.AddSubdir(xWFileSystemDirectory);
					ScanDirectory(xWFileSystemDirectory, pPath2);
				}
				else
				{
					UpdateDirectory(dictionary[next], pPath + next + "/");
				}
				list.Add(next);
			}
			else if (XWFileSystemExtensionRegistry.GetCanShowExtension(next.GetExtension()))
			{
				string text = pPath + next;
				if (!dictionary2.ContainsKey(next))
				{
					XWFileInfo file = new XWFileInfo(next, text)
					{
						ModifiedTime = Godot.FileAccess.GetModifiedTime(text),
						Type = GetResourceTypeName(text)
					};
					pDir.AddFile(file);
				}
				else
				{
					XWFileInfo xWFileInfo = dictionary2[next];
					ulong modifiedTime = Godot.FileAccess.GetModifiedTime(text);
					if (modifiedTime != xWFileInfo.ModifiedTime)
					{
						xWFileInfo.ModifiedTime = modifiedTime;
						xWFileInfo.Type = GetResourceTypeName(text);
						EmitSignal(SignalName.FileChanged, text);
					}
				}
				list2.Add(next);
			}
			next = dirAccess.GetNext();
		}
		dirAccess.ListDirEnd();
		for (int num = pDir.Subdirs.Count - 1; num >= 0; num--)
		{
			if (!list.Contains(pDir.Subdirs[num].Name))
			{
				pDir.RemoveSubdir(num);
			}
		}
		for (int num2 = pDir.Files.Count - 1; num2 >= 0; num2--)
		{
			if (!list2.Contains(pDir.Files[num2].Name))
			{
				pDir.RemoveFile(num2);
			}
		}
	}

	private void SortAll()
	{
		if (RootDirectory != null)
		{
			RootDirectory.SortFiles(SortMode);
			RootDirectory.SortSubdirs(SortMode);
		}
	}

	public void SetSortMode(XWFileSystemEnum.SortMode mode)
	{
		SortMode = mode;
		SortAll();
		EmitSignal(SignalName.FilesystemChanged);
	}

	public XWFileSystemDirectory GetFilesystem()
	{
		return RootDirectory;
	}

	public XWFileSystemDirectory GetFilesystemPath(string path)
	{
		return RootDirectory?.FindDirectoryInTree(path);
	}

	public StringName GetFileType(string filePath)
	{
		if (RootDirectory == null)
		{
			return "";
		}
		return RootDirectory.FindFileInTree(filePath)?.Type ?? ((StringName)"");
	}

	public void UpdateFile(string filePath)
	{
		if (RootDirectory != null)
		{
			XWFileInfo xWFileInfo = RootDirectory.FindFileInTree(filePath);
			if (xWFileInfo != null)
			{
				xWFileInfo.ModifiedTime = Godot.FileAccess.GetModifiedTime(filePath);
				xWFileInfo.Type = GetResourceTypeName(filePath);
				EmitSignal(SignalName.FileChanged, filePath);
			}
		}
	}

	public Error MakeDirRecursive(string path)
	{
		Error error = DirAccess.MakeDirRecursiveAbsolute(path);
		if (error == Error.Ok)
		{
			ScanChanges();
			EmitSignal(SignalName.DirectoryChanged, path);
		}
		return error;
	}

	public Error CopyFile(string from, string to)
	{
		using DirAccess dirAccess = DirAccess.Open(from.GetBaseDir());
		if (dirAccess == null)
		{
			return Error.CantOpen;
		}
		Error error = dirAccess.Copy(from, to);
		if (error == Error.Ok)
		{
			ScanChanges();
			EmitSignal(SignalName.FileChanged, to);
		}
		return error;
	}

	public Error CopyDirectory(string from, string to)
	{
		using DirAccess dirAccess = DirAccess.Open(from);
		if (dirAccess == null)
		{
			return Error.CantOpen;
		}
		DirAccess.MakeDirRecursiveAbsolute(to);
		dirAccess.ListDirBegin();
		string next = dirAccess.GetNext();
		while (next != "")
		{
			if (next == "." || next == "..")
			{
				next = dirAccess.GetNext();
				continue;
			}
			string text = from + "/" + next;
			string text2 = to + "/" + next;
			if (dirAccess.CurrentIsDir())
			{
				CopyDirectory(text + "/", text2 + "/");
			}
			else
			{
				DirAccess.CopyAbsolute(text, text2);
			}
			next = dirAccess.GetNext();
		}
		dirAccess.ListDirEnd();
		ScanChanges();
		return Error.Ok;
	}

	public Error MoveFile(string from, string to)
	{
		if (XWModProjectLayout.IsProtectedDirectory(from, ProjectFolderPath))
		{
			return Error.Unauthorized;
		}
		TryGetCharacterPackageRename(from, to, out var oldCharacterName, out var newCharacterName, out var renamedCharacterPackagePath);
		Error error = DirAccess.RenameAbsolute(from, to);
		if (error == Error.Ok)
		{
			XWModManifestSyncService.MovePath(ProjectFolderPath, from, to);
			if (!string.IsNullOrWhiteSpace(renamedCharacterPackagePath))
			{
				SyncCharacterPackageAfterRename(renamedCharacterPackagePath, oldCharacterName, newCharacterName);
			}
			ScanChanges();
			EmitSignal(SignalName.FileChanged, to);
		}
		return error;
	}

	private bool TryGetCharacterPackageRename(string from, string to, out string oldCharacterName, out string newCharacterName, out string renamedCharacterPackagePath)
	{
		oldCharacterName = "";
		newCharacterName = "";
		renamedCharacterPackagePath = "";
		if (string.IsNullOrWhiteSpace(ProjectFolderPath) || !Directory.Exists(from))
		{
			return false;
		}
		string path = XWModProjectLayout.ToProjectRelativePath(from, ProjectFolderPath);
		string path2 = XWModProjectLayout.ToProjectRelativePath(to, ProjectFolderPath);
		string[] array = NormalizePath(path).Split('/', StringSplitOptions.RemoveEmptyEntries);
		string[] array2 = NormalizePath(path2).Split('/', StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 4 || array2.Length != 4)
		{
			return false;
		}
		if (!string.Equals(array[0], "Resources", StringComparison.OrdinalIgnoreCase) || !string.Equals(array[1], "Characters", StringComparison.OrdinalIgnoreCase) || !string.Equals(array2[0], "Resources", StringComparison.OrdinalIgnoreCase) || !string.Equals(array2[1], "Characters", StringComparison.OrdinalIgnoreCase) || !string.Equals(array[2], array2[2], StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		oldCharacterName = array[3];
		newCharacterName = array2[3];
		if (string.Equals(oldCharacterName, newCharacterName, StringComparison.Ordinal))
		{
			return false;
		}
		renamedCharacterPackagePath = NormalizePath(to);
		return true;
	}

	private void SyncCharacterPackageAfterRename(string packagePath, string oldCharacterName, string newCharacterName)
	{
		if (!string.IsNullOrWhiteSpace(packagePath) && !string.IsNullOrWhiteSpace(oldCharacterName) && !string.IsNullOrWhiteSpace(newCharacterName) && Directory.Exists(packagePath))
		{
			RenameCharacterPackageFileIfExists(packagePath, "", oldCharacterName + ".tres", newCharacterName + ".tres");
			RenameCharacterPackageFileIfExists(packagePath, "", oldCharacterName + ".dat", newCharacterName + ".dat");
			RenameCharacterPackageFileIfExists(packagePath, "Scene", oldCharacterName + ".tscn", newCharacterName + ".tscn");
			RenameCharacterPackageFileIfExists(packagePath, "Script", oldCharacterName + ".cs", newCharacterName + ".cs");
			RenameCharacterPackageFileIfExists(packagePath, "Script", oldCharacterName + ".cs.uid", newCharacterName + ".cs.uid");
			RenameCharacterPackageFileIfExists(packagePath, "Config", oldCharacterName + "Config.tres", newCharacterName + "Config.tres");
			RenameLegacyCharacterConfigIfExists(packagePath, newCharacterName);
			RenameCharacterPackageFileIfExists(packagePath, "Sprite", oldCharacterName + ".tscn", newCharacterName + ".tscn");
			RenameCharacterPackageFileIfExists(packagePath, "Sprite", oldCharacterName + ".tres", newCharacterName + ".tres");
			RenameCharacterPackageFileIfExists(packagePath, "Sprite", oldCharacterName + ".dat", newCharacterName + ".dat");
			ReplaceTextInCharacterPackage(packagePath, oldCharacterName, newCharacterName);
		}
	}

	private void RenameLegacyCharacterConfigIfExists(string packagePath, string newCharacterName)
	{
		RenameCharacterPackageFileIfExists(packagePath, "Config", "Config.tres", newCharacterName + "Config.tres");
	}

	private void RenameCharacterPackageFileIfExists(string packagePath, string folderName, string oldFileName, string newFileName)
	{
		string path = NormalizePath(Path.Combine(packagePath, folderName));
		string text = NormalizePath(Path.Combine(path, oldFileName));
		string text2 = NormalizePath(Path.Combine(path, newFileName));
		if (File.Exists(text) && !File.Exists(text2) && DirAccess.RenameAbsolute(text, text2) == Error.Ok)
		{
			XWModManifestSyncService.MovePath(ProjectFolderPath, text, text2);
		}
	}

	private static void ReplaceTextInCharacterPackage(string packagePath, string oldCharacterName, string newCharacterName)
	{
		string[] files = Directory.GetFiles(packagePath, "*", SearchOption.AllDirectories);
		foreach (string path in files)
		{
			bool flag;
			switch (Path.GetExtension(path).ToLowerInvariant())
			{
			case ".tscn":
			case ".tres":
			case ".cs":
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
			if (flag)
			{
				string text = File.ReadAllText(path);
				if (text.Contains(oldCharacterName, StringComparison.Ordinal))
				{
					File.WriteAllText(path, text.Replace(oldCharacterName, newCharacterName, StringComparison.Ordinal));
				}
			}
		}
	}

	public void RegisterManifestPath(string path)
	{
		XWModManifestSyncService.RegisterPath(ProjectFolderPath, path);
	}

	public Error RemoveFile(string path)
	{
		if (XWModProjectLayout.IsProtectedDirectory(path, ProjectFolderPath))
		{
			return Error.Unauthorized;
		}
		List<string> companionFilesForDeletion = GetCompanionFilesForDeletion(path);
		Error error = MovePathToTrash(path);
		if (error == Error.Ok)
		{
			XWModManifestSyncService.RemovePath(ProjectFolderPath, path);
			foreach (string item in companionFilesForDeletion)
			{
				Error error2 = MovePathToTrash(item);
				if (error2 != Error.Ok)
				{
					GD.PrintErr($"[XWFileSystem] 删除配套资源失败: {item} ({error2})");
				}
				else
				{
					XWModManifestSyncService.RemovePath(ProjectFolderPath, item);
				}
			}
		}
		ScanChanges();
		return error;
	}

	private Error MovePathToTrash(string path)
	{
		return OS.MoveToTrash(ProjectSettings.GlobalizePath(path));
	}

	private List<string> GetCompanionFilesForDeletion(string path)
	{
		List<string> list = new List<string>();
		path = NormalizePath(path);
		if (!XWFileSystemCompanionResourcePolicy.TryResolveAdobeAnimateDatPath(path, out var datPath) || string.IsNullOrWhiteSpace(datPath) || !Godot.FileAccess.FileExists(datPath) || !IsPathInsideProject(datPath))
		{
			return list;
		}
		string text = NormalizePath(datPath);
		if (string.Equals(NormalizePath(text), NormalizePath(path), StringComparison.OrdinalIgnoreCase))
		{
			return list;
		}
		list.Add(text);
		return list;
	}

	private bool IsPathInsideProject(string path)
	{
		string text = NormalizePath(path);
		string text2 = NormalizePath(ProjectFolderPath);
		if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(text2))
		{
			return false;
		}
		if (!text2.EndsWith("/"))
		{
			text2 += "/";
		}
		return text.StartsWith(text2, StringComparison.OrdinalIgnoreCase);
	}

	private static string NormalizePath(string path)
	{
		return (path ?? "").Replace('\\', '/');
	}

	public void AddFavorite(string path)
	{
		if (!Favorites.Contains(path))
		{
			Favorites.Add(path);
			EmitSignal(SignalName.FilesystemChanged);
		}
	}

	public void RemoveFavorite(string path)
	{
		Favorites.Remove(path);
		EmitSignal(SignalName.FilesystemChanged);
	}

	public bool IsFavorite(string path)
	{
		return Favorites.Contains(path);
	}

	public string[] GetValidExtensions()
	{
		return _showExtensionsArray ?? (_showExtensionsArray = ShowExtensionsToArray());
	}

	private static string[] ShowExtensionsToArray()
	{
		return new List<string>(XWFileSystemExtensionRegistry.ShowExtension).ToArray();
	}

	public string GenerateUniqueFolderName(string path, string baseName, string exclude = "")
	{
		List<string> list = new List<string>(DirAccess.GetDirectoriesAt(path));
		if (exclude != "")
		{
			list.Remove(exclude);
		}
		if (!list.Contains(baseName))
		{
			return baseName;
		}
		int num = 1;
		string text = baseName + num;
		while (list.Contains(text))
		{
			num++;
			text = baseName + num;
		}
		return text;
	}

	public string GenerateUniqueFileName(string path, string baseName, string exclude = "")
	{
		List<string> list = new List<string>(DirAccess.GetFilesAt(path));
		if (exclude != "")
		{
			list.Remove(exclude);
		}
		if (!list.Contains(baseName))
		{
			return baseName;
		}
		int num = 1;
		string text = baseName.GetBaseName() + num + "." + baseName.GetExtension();
		while (list.Contains(text))
		{
			num++;
			text = baseName.GetBaseName() + num + "." + baseName.GetExtension();
		}
		return text;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(36)
		{
			new MethodInfo(MethodName.GetSingleton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.SetProjectFolderPath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Scan, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScanChanges, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetResourceTypeName, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFallbackTypeNameByExtension, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "extension", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeNameFromTextResourceHeader, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "extension", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetQuotedHeaderAttribute, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "attributeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScanDirectory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "pDir", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.String, "pPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateDirectory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "pDir", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.String, "pPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SortAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetSortMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFilesystem, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFilesystemPath, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFileType, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeDirRecursive, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyFile, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyDirectory, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveFile, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncCharacterPackageAfterRename, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packagePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "oldCharacterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "newCharacterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenameLegacyCharacterConfigIfExists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packagePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "newCharacterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenameCharacterPackageFileIfExists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packagePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "folderName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "oldFileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "newFileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceTextInCharacterPackage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packagePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "oldCharacterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "newCharacterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterManifestPath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveFile, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MovePathToTrash, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPathInsideProject, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFavorite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveFavorite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsFavorite, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetValidExtensions, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowExtensionsToArray, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GenerateUniqueFolderName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "baseName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "exclude", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateUniqueFileName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "baseName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "exclude", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetSingleton && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWFileSystem>(GetSingleton());
			return true;
		}
		if (method == MethodName.SetProjectFolderPath && args.Count == 1)
		{
			SetProjectFolderPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Scan && args.Count == 0)
		{
			Scan();
			ret = default;
			return true;
		}
		if (method == MethodName.ScanChanges && args.Count == 0)
		{
			ScanChanges();
			ret = default;
			return true;
		}
		if (method == MethodName.GetResourceTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(GetResourceTypeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFallbackTypeNameByExtension && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetFallbackTypeNameByExtension(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeNameFromTextResourceHeader && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetTypeNameFromTextResourceHeader(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetQuotedHeaderAttribute && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetQuotedHeaderAttribute(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ScanDirectory && args.Count == 2)
		{
			ScanDirectory(VariantUtils.ConvertTo<XWFileSystemDirectory>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDirectory && args.Count == 2)
		{
			UpdateDirectory(VariantUtils.ConvertTo<XWFileSystemDirectory>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SortAll && args.Count == 0)
		{
			SortAll();
			ret = default;
			return true;
		}
		if (method == MethodName.SetSortMode && args.Count == 1)
		{
			SetSortMode(VariantUtils.ConvertTo<XWFileSystemEnum.SortMode>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetFilesystem && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWFileSystemDirectory>(GetFilesystem());
			return true;
		}
		if (method == MethodName.GetFilesystemPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWFileSystemDirectory>(GetFilesystemPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFileType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(GetFileType(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateFile && args.Count == 1)
		{
			UpdateFile(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MakeDirRecursive && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Error>(MakeDirRecursive(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CopyFile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Error>(CopyFile(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CopyDirectory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Error>(CopyDirectory(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MoveFile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Error>(MoveFile(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SyncCharacterPackageAfterRename && args.Count == 3)
		{
			SyncCharacterPackageAfterRename(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenameLegacyCharacterConfigIfExists && args.Count == 2)
		{
			RenameLegacyCharacterConfigIfExists(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenameCharacterPackageFileIfExists && args.Count == 4)
		{
			RenameCharacterPackageFileIfExists(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceTextInCharacterPackage && args.Count == 3)
		{
			ReplaceTextInCharacterPackage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterManifestPath && args.Count == 1)
		{
			RegisterManifestPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Error>(RemoveFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MovePathToTrash && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Error>(MovePathToTrash(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsPathInsideProject && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPathInsideProject(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AddFavorite && args.Count == 1)
		{
			AddFavorite(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveFavorite && args.Count == 1)
		{
			RemoveFavorite(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsFavorite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFavorite(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetValidExtensions && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetValidExtensions());
			return true;
		}
		if (method == MethodName.ShowExtensionsToArray && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(ShowExtensionsToArray());
			return true;
		}
		if (method == MethodName.GenerateUniqueFolderName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(GenerateUniqueFolderName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.GenerateUniqueFileName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(GenerateUniqueFileName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetSingleton && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWFileSystem>(GetSingleton());
			return true;
		}
		if (method == MethodName.GetFallbackTypeNameByExtension && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetFallbackTypeNameByExtension(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeNameFromTextResourceHeader && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetTypeNameFromTextResourceHeader(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetQuotedHeaderAttribute && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetQuotedHeaderAttribute(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReplaceTextInCharacterPackage && args.Count == 3)
		{
			ReplaceTextInCharacterPackage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShowExtensionsToArray && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(ShowExtensionsToArray());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetSingleton)
		{
			return true;
		}
		if (method == MethodName.SetProjectFolderPath)
		{
			return true;
		}
		if (method == MethodName.Scan)
		{
			return true;
		}
		if (method == MethodName.ScanChanges)
		{
			return true;
		}
		if (method == MethodName.GetResourceTypeName)
		{
			return true;
		}
		if (method == MethodName.GetFallbackTypeNameByExtension)
		{
			return true;
		}
		if (method == MethodName.GetTypeNameFromTextResourceHeader)
		{
			return true;
		}
		if (method == MethodName.GetQuotedHeaderAttribute)
		{
			return true;
		}
		if (method == MethodName.ScanDirectory)
		{
			return true;
		}
		if (method == MethodName.UpdateDirectory)
		{
			return true;
		}
		if (method == MethodName.SortAll)
		{
			return true;
		}
		if (method == MethodName.SetSortMode)
		{
			return true;
		}
		if (method == MethodName.GetFilesystem)
		{
			return true;
		}
		if (method == MethodName.GetFilesystemPath)
		{
			return true;
		}
		if (method == MethodName.GetFileType)
		{
			return true;
		}
		if (method == MethodName.UpdateFile)
		{
			return true;
		}
		if (method == MethodName.MakeDirRecursive)
		{
			return true;
		}
		if (method == MethodName.CopyFile)
		{
			return true;
		}
		if (method == MethodName.CopyDirectory)
		{
			return true;
		}
		if (method == MethodName.MoveFile)
		{
			return true;
		}
		if (method == MethodName.SyncCharacterPackageAfterRename)
		{
			return true;
		}
		if (method == MethodName.RenameLegacyCharacterConfigIfExists)
		{
			return true;
		}
		if (method == MethodName.RenameCharacterPackageFileIfExists)
		{
			return true;
		}
		if (method == MethodName.ReplaceTextInCharacterPackage)
		{
			return true;
		}
		if (method == MethodName.RegisterManifestPath)
		{
			return true;
		}
		if (method == MethodName.RemoveFile)
		{
			return true;
		}
		if (method == MethodName.MovePathToTrash)
		{
			return true;
		}
		if (method == MethodName.IsPathInsideProject)
		{
			return true;
		}
		if (method == MethodName.NormalizePath)
		{
			return true;
		}
		if (method == MethodName.AddFavorite)
		{
			return true;
		}
		if (method == MethodName.RemoveFavorite)
		{
			return true;
		}
		if (method == MethodName.IsFavorite)
		{
			return true;
		}
		if (method == MethodName.GetValidExtensions)
		{
			return true;
		}
		if (method == MethodName.ShowExtensionsToArray)
		{
			return true;
		}
		if (method == MethodName.GenerateUniqueFolderName)
		{
			return true;
		}
		if (method == MethodName.GenerateUniqueFileName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.RootDirectory)
		{
			RootDirectory = VariantUtils.ConvertTo<XWFileSystemDirectory>(in value);
			return true;
		}
		if (name == PropertyName.ProjectFolderPath)
		{
			ProjectFolderPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.IsScanning)
		{
			IsScanning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.SortMode)
		{
			SortMode = VariantUtils.ConvertTo<XWFileSystemEnum.SortMode>(in value);
			return true;
		}
		if (name == PropertyName.ScanTotal)
		{
			ScanTotal = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ScanCurrent)
		{
			ScanCurrent = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.RootDirectory)
		{
			value = VariantUtils.CreateFrom<XWFileSystemDirectory>(RootDirectory);
			return true;
		}
		if (name == PropertyName.ProjectFolderPath)
		{
			value = VariantUtils.CreateFrom<string>(ProjectFolderPath);
			return true;
		}
		if (name == PropertyName.IsScanning)
		{
			value = VariantUtils.CreateFrom<bool>(IsScanning);
			return true;
		}
		if (name == PropertyName.SortMode)
		{
			value = VariantUtils.CreateFrom<XWFileSystemEnum.SortMode>(SortMode);
			return true;
		}
		int from;
		if (name == PropertyName.ScanTotal)
		{
			from = ScanTotal;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ScanCurrent)
		{
			from = ScanCurrent;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.RootDirectory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ProjectFolderPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsScanning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SortMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ScanTotal, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ScanCurrent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.RootDirectory, Variant.From<XWFileSystemDirectory>(RootDirectory));
		info.AddProperty(PropertyName.ProjectFolderPath, Variant.From<string>(ProjectFolderPath));
		info.AddProperty(PropertyName.IsScanning, Variant.From<bool>(IsScanning));
		info.AddProperty(PropertyName.SortMode, Variant.From<XWFileSystemEnum.SortMode>(SortMode));
		info.AddProperty(PropertyName.ScanTotal, Variant.From<int>(ScanTotal));
		info.AddProperty(PropertyName.ScanCurrent, Variant.From<int>(ScanCurrent));
		info.AddSignalEventDelegate(SignalName.FilesystemChanged, backing_FilesystemChanged);
		info.AddSignalEventDelegate(SignalName.ScanStarted, backing_ScanStarted);
		info.AddSignalEventDelegate(SignalName.ScanCompleted, backing_ScanCompleted);
		info.AddSignalEventDelegate(SignalName.ScanProgress, backing_ScanProgress);
		info.AddSignalEventDelegate(SignalName.FileChanged, backing_FileChanged);
		info.AddSignalEventDelegate(SignalName.DirectoryChanged, backing_DirectoryChanged);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.RootDirectory, out var value))
		{
			RootDirectory = value.As<XWFileSystemDirectory>();
		}
		if (info.TryGetProperty(PropertyName.ProjectFolderPath, out var value2))
		{
			ProjectFolderPath = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.IsScanning, out var value3))
		{
			IsScanning = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.SortMode, out var value4))
		{
			SortMode = value4.As<XWFileSystemEnum.SortMode>();
		}
		if (info.TryGetProperty(PropertyName.ScanTotal, out var value5))
		{
			ScanTotal = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ScanCurrent, out var value6))
		{
			ScanCurrent = value6.As<int>();
		}
		if (info.TryGetSignalEventDelegate<FilesystemChangedEventHandler>(SignalName.FilesystemChanged, out var value7))
		{
			backing_FilesystemChanged = value7;
		}
		if (info.TryGetSignalEventDelegate<ScanStartedEventHandler>(SignalName.ScanStarted, out var value8))
		{
			backing_ScanStarted = value8;
		}
		if (info.TryGetSignalEventDelegate<ScanCompletedEventHandler>(SignalName.ScanCompleted, out var value9))
		{
			backing_ScanCompleted = value9;
		}
		if (info.TryGetSignalEventDelegate<ScanProgressEventHandler>(SignalName.ScanProgress, out var value10))
		{
			backing_ScanProgress = value10;
		}
		if (info.TryGetSignalEventDelegate<FileChangedEventHandler>(SignalName.FileChanged, out var value11))
		{
			backing_FileChanged = value11;
		}
		if (info.TryGetSignalEventDelegate<DirectoryChangedEventHandler>(SignalName.DirectoryChanged, out var value12))
		{
			backing_DirectoryChanged = value12;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(SignalName.FilesystemChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ScanStarted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ScanCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ScanProgress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "current", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "total", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.FileChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.DirectoryChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dirPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalFilesystemChanged()
	{
		EmitSignal(SignalName.FilesystemChanged, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalScanStarted()
	{
		EmitSignal(SignalName.ScanStarted, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalScanCompleted()
	{
		EmitSignal(SignalName.ScanCompleted, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalScanProgress(int current, int total)
	{
		StringName scanProgress = SignalName.ScanProgress;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = current;
		buffer[1] = total;
		EmitSignal(scanProgress, buffer);
	}

	protected void EmitSignalFileChanged(string filePath)
	{
		EmitSignal(SignalName.FileChanged, new ReadOnlySpan<Variant>((Variant)filePath));
	}

	protected void EmitSignalDirectoryChanged(string dirPath)
	{
		EmitSignal(SignalName.DirectoryChanged, new ReadOnlySpan<Variant>((Variant)dirPath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.FilesystemChanged && args.Count == 0)
		{
			backing_FilesystemChanged?.Invoke();
		}
		else if (signal == SignalName.ScanStarted && args.Count == 0)
		{
			backing_ScanStarted?.Invoke();
		}
		else if (signal == SignalName.ScanCompleted && args.Count == 0)
		{
			backing_ScanCompleted?.Invoke();
		}
		else if (signal == SignalName.ScanProgress && args.Count == 2)
		{
			backing_ScanProgress?.Invoke(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
		}
		else if (signal == SignalName.FileChanged && args.Count == 1)
		{
			backing_FileChanged?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else if (signal == SignalName.DirectoryChanged && args.Count == 1)
		{
			backing_DirectoryChanged?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.FilesystemChanged)
		{
			return true;
		}
		if (signal == SignalName.ScanStarted)
		{
			return true;
		}
		if (signal == SignalName.ScanCompleted)
		{
			return true;
		}
		if (signal == SignalName.ScanProgress)
		{
			return true;
		}
		if (signal == SignalName.FileChanged)
		{
			return true;
		}
		if (signal == SignalName.DirectoryChanged)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
