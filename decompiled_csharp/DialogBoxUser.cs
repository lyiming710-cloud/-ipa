using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/DialogBoxGeneral/DialogBoxUser/DialogBoxUser.cs")]
public class DialogBoxUser : DialogPopup
{
	public new class MethodName : DialogPopup.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName RefreshUser = "RefreshUser";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName UserItemCreate = "UserItemCreate";

		public static readonly StringName UserChange = "UserChange";

		public static readonly StringName RenameButtonPressed = "RenameButtonPressed";

		public static readonly StringName RenameDialogClose = "RenameDialogClose";

		public static readonly StringName FinishButtonPressed = "FinishButtonPressed";

		public static readonly StringName DeleteButtonPressed = "DeleteButtonPressed";

		public static readonly StringName DeleteDialogClose = "DeleteDialogClose";

		public static readonly StringName CancelButtonPressed = "CancelButtonPressed";

		public static readonly StringName CreateUserButtonPressed = "CreateUserButtonPressed";

		public static readonly StringName CreateUserDialogClose = "CreateUserDialogClose";

		public static readonly StringName LogOutButtonPressed = "LogOutButtonPressed";

		public static readonly StringName BackButtonPressed = "BackButtonPressed";

		public static readonly StringName ExportButtonPressed = "ExportButtonPressed";

		public static readonly StringName SaveFileTo = "SaveFileTo";

		public static readonly StringName LoadButtonPressed = "LoadButtonPressed";

		public static readonly StringName FileOpen = "FileOpen";

		public static readonly StringName InternetExportButtoPressed = "InternetExportButtoPressed";

		public static readonly StringName CreateSharedSavePayload = "CreateSharedSavePayload";

		public static readonly StringName InternetLoadButtonPressed = "InternetLoadButtonPressed";

		public static readonly StringName OnGetSharedLevelSuccess = "OnGetSharedLevelSuccess";

		public static readonly StringName ShowImportNameInput = "ShowImportNameInput";

		public static readonly StringName ImportToNewUser = "ImportToNewUser";

		public static readonly StringName OnGetSharedLevelFailed = "OnGetSharedLevelFailed";

		public static readonly StringName OnShareLevelSuccess = "OnShareLevelSuccess";

		public static readonly StringName EnsureTempSaveDirectory = "EnsureTempSaveDirectory";

		public static readonly StringName EnsureSaveExtension = "EnsureSaveExtension";

		public static readonly StringName IsAndroidContentUri = "IsAndroidContentUri";

		public static readonly StringName CopyFileBytes = "CopyFileBytes";

		public static readonly StringName GetDefaultUserFromSave = "GetDefaultUserFromSave";

		public static readonly StringName ExtractSharedUserDictionary = "ExtractSharedUserDictionary";

		public static readonly StringName OnShareLevelFailed = "OnShareLevelFailed";
	}

	public new class PropertyName : DialogPopup.PropertyName
	{
		public static readonly StringName userContainer = "userContainer";

		public static readonly StringName renameButton = "renameButton";

		public static readonly StringName finishButton = "finishButton";

		public static readonly StringName deleteButton = "deleteButton";

		public static readonly StringName cancelButton = "cancelButton";

		public static readonly StringName createUserButton = "createUserButton";

		public static readonly StringName logOutButton = "logOutButton";

		public static readonly StringName backButton = "backButton";

		public static readonly StringName editUser = "editUser";

		public static readonly StringName pendingImportData = "pendingImportData";
	}

	public new class SignalName : DialogPopup.SignalName
	{
	}

	private const int MaxSharedSaveCompressedBytes = 33554432;

	private const int MaxSharedSaveDecompressedBytes = 33554432;

	private const ulong MaxLocalSaveBytes = 33554432uL;

	private const string ExportTempPath = "user://Csharp/export_save.res";

	private const string ImportTempPath = "user://Csharp/import_save.res";

	private static PackedScene _UserItemScene;

	private static ButtonGroup _UserButtonGroup;

	private VBoxContainer userContainer;

	private MainButton renameButton;

	private MainButton finishButton;

	private MainButton deleteButton;

	private MainButton cancelButton;

	private Button createUserButton;

	private MainButton logOutButton;

	private MainButton backButton;

	public string editUser = "";

	public Dictionary pendingImportData = new Dictionary();

	private static PackedScene userItemScene => _UserItemScene ?? (_UserItemScene = GD.Load<PackedScene>("uid://xo7n1bu85swf"));

	private static ButtonGroup userButtonGroup => _UserButtonGroup ?? (_UserButtonGroup = GD.Load<ButtonGroup>("uid://dge7tb6et4lw7"));

	public override void _Ready()
	{
		base._Ready();
		userContainer = GetNode<VBoxContainer>("%UserContainer");
		renameButton = GetNode<MainButton>("%RenameButton");
		finishButton = GetNode<MainButton>("%FinishButton");
		deleteButton = GetNode<MainButton>("%DeleteButton");
		cancelButton = GetNode<MainButton>("%CancelButton");
		createUserButton = GetNode<Button>("%CreateUserButton");
		logOutButton = GetNode<MainButton>("%LogOutButton");
		backButton = GetNode<MainButton>("%BackButton");
		GetNode<BaseButton>("%RenameButton").Pressed += RenameButtonPressed;
		GetNode<BaseButton>("%FinishButton").Pressed += FinishButtonPressed;
		GetNode<BaseButton>("%DeleteButton").Pressed += DeleteButtonPressed;
		GetNode<BaseButton>("%CancelButton").Pressed += CancelButtonPressed;
		GetNode<BaseButton>("%CreateUserButton").Pressed += CreateUserButtonPressed;
		GetNode<BaseButton>("%LogOutButton").Pressed += LogOutButtonPressed;
		GetNode<BaseButton>("%BackButton").Pressed += BackButtonPressed;
		GetNode<BaseButton>("Layer/Control/ExportButton").Pressed += ExportButtonPressed;
		GetNode<BaseButton>("Layer/Control/LoadButton").Pressed += LoadButtonPressed;
		GetNode<BaseButton>("Layer/Control/InternetExportButton").Pressed += InternetExportButtoPressed;
		GetNode<BaseButton>("Layer/Control/InternetLoadButton").Pressed += InternetLoadButtonPressed;
		RefreshUser();
		InternetServerManager.Instance.OnShareLevelSuccess += OnShareLevelSuccess;
		InternetServerManager.Instance.OnShareLevelFailed += OnShareLevelFailed;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (InternetServerManager.Instance != null)
		{
			InternetServerManager.Instance.OnShareLevelSuccess -= OnShareLevelSuccess;
			InternetServerManager.Instance.OnShareLevelFailed -= OnShareLevelFailed;
			InternetServerManager.Instance.OnGetSharedLevelSuccess -= OnGetSharedLevelSuccess;
			InternetServerManager.Instance.OnGetSharedLevelFailed -= OnGetSharedLevelFailed;
		}
	}

	public void RefreshUser()
	{
		foreach (Node child in userContainer.GetChildren())
		{
			child.QueueFree();
		}
		foreach (string user in GameSaveManager.Instance.GetUserList())
		{
			UserItemCreate(user);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		deleteButton.Disabled = GameSaveManager.Instance.GetUserList().Count <= 1;
	}

	public void UserItemCreate(string user)
	{
		UserItem userItem = (UserItem)userItemScene.Instantiate(PackedScene.GenEditState.Disabled);
		if (GameSaveManager.Instance.GetUserCurrent() == user)
		{
			editUser = user;
			userItem.ButtonPressed = true;
		}
		userItem.Text = user;
		userItem.OnChoose += UserChange;
		userItem.ButtonGroup = userButtonGroup;
		userContainer.AddChild(userItem, forceReadableName: false, InternalMode.Disabled);
	}

	public void UserChange(string user)
	{
		editUser = user;
	}

	public void RenameButtonPressed()
	{
		DialogBoxBase dialogBoxBase = DialogCreate("RenameUser");
		dialogBoxBase.Set("changeUser", editUser);
		dialogBoxBase.OnClose += RenameDialogClose;
	}

	public void RenameDialogClose()
	{
		RefreshUser();
	}

	public void FinishButtonPressed()
	{
		GameSaveManager.Instance.SetUserCurrent(editUser);
		GameSaveManager.Instance.Save();
		CloseDialog();
	}

	public void DeleteButtonPressed()
	{
		DialogBoxBase dialogBoxBase = DialogCreate("DeleteUser");
		dialogBoxBase.Set("deleteUser", editUser);
		dialogBoxBase.OnClose += DeleteDialogClose;
	}

	public void DeleteDialogClose()
	{
		if (!GameSaveManager.Instance.HasUser(editUser))
		{
			Array<Node> children = userContainer.GetChildren();
			((Button)children[0]).ButtonPressed = true;
			editUser = ((Button)children[0]).Text;
			RefreshUser();
		}
	}

	public void CancelButtonPressed()
	{
		GameSaveManager.Instance.SetUserCurrent(editUser);
		GameSaveManager.Instance.Save();
		CloseDialog();
	}

	public void CreateUserButtonPressed()
	{
		DialogCreate("NewUser").OnClose += CreateUserDialogClose;
	}

	public void CreateUserDialogClose()
	{
		RefreshUser();
		editUser = GameSaveManager.Instance.GetUserCurrent();
	}

	public void LogOutButtonPressed()
	{
		MultiPlayerManager.Instance.LogOut();
		CloseDialog();
	}

	public void BackButtonPressed()
	{
		SceneManager.Instance.ChangeScene("Loading");
		CloseDialog();
	}

	public void ExportButtonPressed()
	{
		DisplayServer.FileDialogShow("保存存档文件", "", "", showHidden: false, DisplayServer.FileDialogMode.SaveFile, new string[1] { "*.res" }, Callable.From((bool status, string[] selectedPaths, int selectedFilterIndex) =>
		{
			SaveFileTo(status, selectedPaths, selectedFilterIndex);
		}));
	}

	private void SaveFileTo(bool status, string[] selectedPaths, int selectedFilterIndex)
	{
		if (status && selectedPaths.Length != 0)
		{
			string toPath = EnsureSaveExtension(selectedPaths[0]);
			EnsureTempSaveDirectory();
			GameSaveManager.Instance.SyncCoinBankForExport();
			if (ResourceSaver.Save(GameSaveManager.Instance.config, "user://Csharp/export_save.res", ResourceSaver.SaverFlags.None) != Error.Ok || !CopyFileBytes("user://Csharp/export_save.res", toPath))
			{
				DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]导出失败[/font_size][/center]");
			}
			else
			{
				DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]导出成功[/font_size][/center]");
			}
		}
	}

	public void LoadButtonPressed()
	{
		DisplayServer.FileDialogShow("打开存档文件", "", "", showHidden: false, DisplayServer.FileDialogMode.OpenFile, new string[1] { "*.res" }, Callable.From((bool status, string[] selectedPaths, int selectedFilterIndex) =>
		{
			FileOpen(status, selectedPaths, selectedFilterIndex);
		}));
	}

	private void FileOpen(bool status, string[] selectedPaths, int selectedFilterIndex)
	{
		if (!status || selectedPaths.Length == 0)
		{
			return;
		}
		EnsureTempSaveDirectory();
		if (!CopyFileBytes(selectedPaths[0], "user://Csharp/import_save.res"))
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]无法读取该文件[/font_size][/center]");
			return;
		}
		Resource resource = ResourceLoader.Load("user://Csharp/import_save.res", "", ResourceLoader.CacheMode.Ignore);
		if (GameSaveManager.Instance.TryConvertSaveResource(resource, out var saveConfig))
		{
			DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("DialogBoxChoose");
			dialogBoxBase.Set("text", "[center][font_size=24]是否导入该存档？[/font_size][/center]");
			((DialogBoxChoose)dialogBoxBase).OnChooseTrue += () =>
			{
				if (!GameSaveManager.Instance.ReplaceMainSave(saveConfig))
				{
					DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]存档替换失败，原存档保持不变[/font_size][/center]");
				}
				else
				{
					editUser = GetDefaultUserFromSave(saveConfig);
					RefreshUser();
					GameSaveManager.Instance.SetUserCurrent(editUser);
				}
			};
		}
		else
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]该文件不是存档文件[/font_size][/center]");
		}
	}

	public void InternetExportButtoPressed()
	{
		if (editUser != "" && GameSaveManager.Instance.config.saveDictionary.ContainsKey(editUser))
		{
			DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("DialogBoxChoose");
			dialogBoxBase.Set("text", "[center][font_size=24]确定要分享该存档吗？[/font_size][/center]");
			((DialogBoxChoose)dialogBoxBase).OnChooseTrue += () =>
			{
				byte[] array = CreateSharedSavePayload(editUser);
				if (array.Length != 0)
				{
					InternetServerManager.Instance.ShareLevel(array);
				}
			};
		}
		else
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]无法获取当前用户数据[/font_size][/center]");
		}
	}

	internal byte[] CreateSharedSavePayload(string user)
	{
		GameSaveManager instance = GameSaveManager.Instance;
		if (!GodotObject.IsInstanceValid(instance) || string.IsNullOrEmpty(user))
		{
			return System.Array.Empty<byte>();
		}
		instance.SyncCoinBankForExport();
		if (instance.config == null || !instance.config.saveDictionary.ContainsKey(user))
		{
			return System.Array.Empty<byte>();
		}
		byte[] array = Json.Stringify(instance.config.saveDictionary[user].AsGodotDictionary()).ToUtf8Buffer();
		if (array.Length > 33554432)
		{
			return System.Array.Empty<byte>();
		}
		return array;
	}

	public void InternetLoadButtonPressed()
	{
		DialogBoxBase inputDialog = DialogManager.Instance.DialogCreate("DialogBoxInput");
		inputDialog.Set("titleText", "[center][font_size=24]输入分享码[/font_size][/center]");
		inputDialog.Set("inputMaxLength", 4);
		inputDialog.Set("inputDefaultText", "");
		inputDialog.Set("inputPlaceholder", "请输入4位分享码");
		((DialogBoxInput)inputDialog).OnConfirmButtonPressed += () =>
		{
			string instance = (string)inputDialog.Get("inputText");
			instance = instance.StripEdges();
			if (instance.Length == 4)
			{
				InternetServerManager.Instance.OnGetSharedLevelSuccess += OnGetSharedLevelSuccess;
				InternetServerManager.Instance.OnGetSharedLevelFailed += OnGetSharedLevelFailed;
				InternetServerManager.Instance.GetSharedFile(instance);
			}
			else
			{
				DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]请输入4位分享码[/font_size][/center]");
			}
		};
	}

	private void OnGetSharedLevelSuccess(byte[] data)
	{
		InternetServerManager.Instance.OnGetSharedLevelSuccess -= OnGetSharedLevelSuccess;
		InternetServerManager.Instance.OnGetSharedLevelFailed -= OnGetSharedLevelFailed;
		if (TryParseSharedSavePayload(data, out pendingImportData))
		{
			ShowImportNameInput();
		}
		else
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]数据格式错误[/font_size][/center]");
		}
	}

	private void ShowImportNameInput()
	{
		DialogBoxBase inputDialog = DialogManager.Instance.DialogCreate("DialogBoxInput");
		inputDialog.Set("titleText", "[center][font_size=24]导入存档[/font_size][/center]");
		inputDialog.Set("inputMaxLength", 20);
		inputDialog.Set("inputDefaultText", "");
		inputDialog.Set("inputPlaceholder", "请输入存档名称");
		((DialogBoxInput)inputDialog).OnConfirmButtonPressed += () =>
		{
			string instance = (string)inputDialog.Get("inputText");
			instance = instance.StripEdges();
			if (instance.Length > 0)
			{
				if (GameSaveManager.Instance.HasUser(instance))
				{
					DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]该用户名已存在[/font_size][/center]");
				}
				else
				{
					ImportToNewUser(instance);
				}
			}
			else
			{
				DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]请输入存档名称[/font_size][/center]");
			}
		};
	}

	private void ImportToNewUser(string userName)
	{
		GameSaveManager.Instance.config.userList.Add(userName);
		GameSaveManager.Instance.config.saveDictionary[userName] = pendingImportData.Duplicate(deep: true);
		GameSaveManager.Instance.Save();
		RefreshUser();
		pendingImportData.Clear();
		DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]导入成功[/font_size][/center]");
	}

	private void OnGetSharedLevelFailed(string message)
	{
		InternetServerManager.Instance.OnGetSharedLevelSuccess -= OnGetSharedLevelSuccess;
		InternetServerManager.Instance.OnGetSharedLevelFailed -= OnGetSharedLevelFailed;
		DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]" + message + "[/font_size][/center]");
	}

	private void OnShareLevelSuccess(string code, long expireAt, long expireSeconds)
	{
		Dictionary timeZoneFromSystem = Time.GetTimeZoneFromSystem();
		Dictionary datetimeDictFromUnixTime = Time.GetDatetimeDictFromUnixTime(expireAt + (long)timeZoneFromSystem["bias"] * 60);
		string value = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)(long)datetimeDictFromUnixTime["hour"], (int)(long)datetimeDictFromUnixTime["minute"], (int)(long)datetimeDictFromUnixTime["second"]);
		DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", $"[center][font_size=16]分享成功！\n分享码: {code}\n过期时间: {value}[/font_size][/center]");
	}

	private static void EnsureTempSaveDirectory()
	{
		if (!DirAccess.DirExistsAbsolute("user://Csharp"))
		{
			DirAccess.MakeDirRecursiveAbsolute("user://Csharp");
		}
	}

	private static string EnsureSaveExtension(string path)
	{
		if (IsAndroidContentUri(path) || path.GetExtension().ToLowerInvariant() == "res")
		{
			return path;
		}
		return path + ".res";
	}

	private static bool IsAndroidContentUri(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			return path.StartsWith("content://", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static bool CopyFileBytes(string fromPath, string toPath)
	{
		if (string.IsNullOrEmpty(fromPath) || string.IsNullOrEmpty(toPath))
		{
			return false;
		}
		using FileAccess fileAccess = FileAccess.Open(fromPath, FileAccess.ModeFlags.Read);
		if (fileAccess == null)
		{
			return false;
		}
		ulong length = fileAccess.GetLength();
		if (length == 0 || length > 33554432)
		{
			return false;
		}
		using FileAccess fileAccess2 = FileAccess.Open(toPath, FileAccess.ModeFlags.Write);
		if (fileAccess2 == null)
		{
			return false;
		}
		while (fileAccess.GetPosition() < length)
		{
			int num = (int)Math.Min(81920uL, length - fileAccess.GetPosition());
			byte[] buffer = fileAccess.GetBuffer(num);
			if (buffer.Length != num)
			{
				return false;
			}
			fileAccess2.StoreBuffer(buffer);
		}
		return true;
	}

	private static string GetDefaultUserFromSave(GameSaveConfigCSharp saveConfig)
	{
		if (saveConfig == null)
		{
			return "";
		}
		if (!string.IsNullOrEmpty(saveConfig.userCurrent) && saveConfig.userList.Contains(saveConfig.userCurrent))
		{
			return saveConfig.userCurrent;
		}
		if (saveConfig.userList.Count <= 0)
		{
			return "";
		}
		return saveConfig.userList[0];
	}

	private static bool TryParseSharedSavePayload(byte[] data, out Dictionary saveData)
	{
		saveData = new Dictionary();
		if (data == null || data.Length == 0 || data.Length > 33554432)
		{
			return false;
		}
		if (TryParseSaveJson(data.GetStringFromUtf8(), out saveData))
		{
			return true;
		}
		try
		{
			byte[] array = data.DecompressDynamic(33554432L, FileAccess.CompressionMode.GZip);
			if (array != null && array.Length != 0)
			{
				return TryParseSaveJson(array.GetStringFromUtf8(), out saveData);
			}
		}
		catch (Exception)
		{
		}
		return false;
	}

	private static bool TryParseSaveJson(string jsonString, out Dictionary saveData)
	{
		saveData = new Dictionary();
		if (string.IsNullOrWhiteSpace(jsonString))
		{
			return false;
		}
		Json json = new Json();
		if (json.Parse(jsonString) != Error.Ok || json.Data.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		Dictionary parsed = json.Data.AsGodotDictionary();
		saveData = ExtractSharedUserDictionary(parsed);
		return saveData.Count > 0;
	}

	private static Dictionary ExtractSharedUserDictionary(Dictionary parsed)
	{
		if (!parsed.ContainsKey("Dictionary"))
		{
			return parsed;
		}
		string text = parsed["Dictionary"].AsString();
		if (string.IsNullOrWhiteSpace(text))
		{
			return parsed;
		}
		Json json = new Json();
		if (json.Parse(text) != Error.Ok || json.Data.VariantType != Variant.Type.Dictionary)
		{
			return parsed;
		}
		Dictionary dictionary = json.Data.AsGodotDictionary();
		string text2 = (parsed.ContainsKey("Current") ? parsed["Current"].AsString() : "");
		if (!string.IsNullOrEmpty(text2) && dictionary.ContainsKey(text2))
		{
			return dictionary[text2].AsGodotDictionary();
		}
		using IEnumerator<Variant> enumerator = dictionary.Keys.GetEnumerator();
		if (enumerator.MoveNext())
		{
			Variant current = enumerator.Current;
			return dictionary[current].AsGodotDictionary();
		}
		return parsed;
	}

	private void OnShareLevelFailed(string message)
	{
		DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]分享失败: " + message + "[/font_size][/center]");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(35)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshUser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UserItemCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UserChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenameButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenameDialogClose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinishButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeleteButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeleteDialogClose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateUserButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateUserDialogClose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LogOutButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BackButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFileTo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "status", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "selectedPaths", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "selectedFilterIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FileOpen, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "status", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "selectedPaths", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "selectedFilterIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InternetExportButtoPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSharedSavePayload, new PropertyInfo(Variant.Type.PackedByteArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InternetLoadButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGetSharedLevelSuccess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedByteArray, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowImportNameInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportToNewUser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "userName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnGetSharedLevelFailed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnShareLevelSuccess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "code", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expireAt", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expireSeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureTempSaveDirectory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.EnsureSaveExtension, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsAndroidContentUri, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyFileBytes, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fromPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "toPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDefaultUserFromSave, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "saveConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExtractSharedUserDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "parsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnShareLevelFailed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshUser && args.Count == 0)
		{
			RefreshUser();
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UserItemCreate && args.Count == 1)
		{
			UserItemCreate(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UserChange && args.Count == 1)
		{
			UserChange(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenameButtonPressed && args.Count == 0)
		{
			RenameButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.RenameDialogClose && args.Count == 0)
		{
			RenameDialogClose();
			ret = default;
			return true;
		}
		if (method == MethodName.FinishButtonPressed && args.Count == 0)
		{
			FinishButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteButtonPressed && args.Count == 0)
		{
			DeleteButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteDialogClose && args.Count == 0)
		{
			DeleteDialogClose();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelButtonPressed && args.Count == 0)
		{
			CancelButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateUserButtonPressed && args.Count == 0)
		{
			CreateUserButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateUserDialogClose && args.Count == 0)
		{
			CreateUserDialogClose();
			ret = default;
			return true;
		}
		if (method == MethodName.LogOutButtonPressed && args.Count == 0)
		{
			LogOutButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.BackButtonPressed && args.Count == 0)
		{
			BackButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportButtonPressed && args.Count == 0)
		{
			ExportButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFileTo && args.Count == 3)
		{
			SaveFileTo(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadButtonPressed && args.Count == 0)
		{
			LoadButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.FileOpen && args.Count == 3)
		{
			FileOpen(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.InternetExportButtoPressed && args.Count == 0)
		{
			InternetExportButtoPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSharedSavePayload && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<byte[]>(CreateSharedSavePayload(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.InternetLoadButtonPressed && args.Count == 0)
		{
			InternetLoadButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGetSharedLevelSuccess && args.Count == 1)
		{
			OnGetSharedLevelSuccess(VariantUtils.ConvertTo<byte[]>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowImportNameInput && args.Count == 0)
		{
			ShowImportNameInput();
			ret = default;
			return true;
		}
		if (method == MethodName.ImportToNewUser && args.Count == 1)
		{
			ImportToNewUser(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnGetSharedLevelFailed && args.Count == 1)
		{
			OnGetSharedLevelFailed(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnShareLevelSuccess && args.Count == 3)
		{
			OnShareLevelSuccess(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<long>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureTempSaveDirectory && args.Count == 0)
		{
			EnsureTempSaveDirectory();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureSaveExtension && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EnsureSaveExtension(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAndroidContentUri && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAndroidContentUri(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CopyFileBytes && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CopyFileBytes(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetDefaultUserFromSave && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDefaultUserFromSave(VariantUtils.ConvertTo<GameSaveConfigCSharp>(in args[0])));
			return true;
		}
		if (method == MethodName.ExtractSharedUserDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExtractSharedUserDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.OnShareLevelFailed && args.Count == 1)
		{
			OnShareLevelFailed(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EnsureTempSaveDirectory && args.Count == 0)
		{
			EnsureTempSaveDirectory();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureSaveExtension && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EnsureSaveExtension(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAndroidContentUri && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAndroidContentUri(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CopyFileBytes && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CopyFileBytes(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetDefaultUserFromSave && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDefaultUserFromSave(VariantUtils.ConvertTo<GameSaveConfigCSharp>(in args[0])));
			return true;
		}
		if (method == MethodName.ExtractSharedUserDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExtractSharedUserDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.RefreshUser)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.UserItemCreate)
		{
			return true;
		}
		if (method == MethodName.UserChange)
		{
			return true;
		}
		if (method == MethodName.RenameButtonPressed)
		{
			return true;
		}
		if (method == MethodName.RenameDialogClose)
		{
			return true;
		}
		if (method == MethodName.FinishButtonPressed)
		{
			return true;
		}
		if (method == MethodName.DeleteButtonPressed)
		{
			return true;
		}
		if (method == MethodName.DeleteDialogClose)
		{
			return true;
		}
		if (method == MethodName.CancelButtonPressed)
		{
			return true;
		}
		if (method == MethodName.CreateUserButtonPressed)
		{
			return true;
		}
		if (method == MethodName.CreateUserDialogClose)
		{
			return true;
		}
		if (method == MethodName.LogOutButtonPressed)
		{
			return true;
		}
		if (method == MethodName.BackButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ExportButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SaveFileTo)
		{
			return true;
		}
		if (method == MethodName.LoadButtonPressed)
		{
			return true;
		}
		if (method == MethodName.FileOpen)
		{
			return true;
		}
		if (method == MethodName.InternetExportButtoPressed)
		{
			return true;
		}
		if (method == MethodName.CreateSharedSavePayload)
		{
			return true;
		}
		if (method == MethodName.InternetLoadButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnGetSharedLevelSuccess)
		{
			return true;
		}
		if (method == MethodName.ShowImportNameInput)
		{
			return true;
		}
		if (method == MethodName.ImportToNewUser)
		{
			return true;
		}
		if (method == MethodName.OnGetSharedLevelFailed)
		{
			return true;
		}
		if (method == MethodName.OnShareLevelSuccess)
		{
			return true;
		}
		if (method == MethodName.EnsureTempSaveDirectory)
		{
			return true;
		}
		if (method == MethodName.EnsureSaveExtension)
		{
			return true;
		}
		if (method == MethodName.IsAndroidContentUri)
		{
			return true;
		}
		if (method == MethodName.CopyFileBytes)
		{
			return true;
		}
		if (method == MethodName.GetDefaultUserFromSave)
		{
			return true;
		}
		if (method == MethodName.ExtractSharedUserDictionary)
		{
			return true;
		}
		if (method == MethodName.OnShareLevelFailed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.userContainer)
		{
			userContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.renameButton)
		{
			renameButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.finishButton)
		{
			finishButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.deleteButton)
		{
			deleteButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.cancelButton)
		{
			cancelButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.createUserButton)
		{
			createUserButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.logOutButton)
		{
			logOutButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.backButton)
		{
			backButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.editUser)
		{
			editUser = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.pendingImportData)
		{
			pendingImportData = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.userContainer)
		{
			value = VariantUtils.CreateFrom(in userContainer);
			return true;
		}
		if (name == PropertyName.renameButton)
		{
			value = VariantUtils.CreateFrom(in renameButton);
			return true;
		}
		if (name == PropertyName.finishButton)
		{
			value = VariantUtils.CreateFrom(in finishButton);
			return true;
		}
		if (name == PropertyName.deleteButton)
		{
			value = VariantUtils.CreateFrom(in deleteButton);
			return true;
		}
		if (name == PropertyName.cancelButton)
		{
			value = VariantUtils.CreateFrom(in cancelButton);
			return true;
		}
		if (name == PropertyName.createUserButton)
		{
			value = VariantUtils.CreateFrom(in createUserButton);
			return true;
		}
		if (name == PropertyName.logOutButton)
		{
			value = VariantUtils.CreateFrom(in logOutButton);
			return true;
		}
		if (name == PropertyName.backButton)
		{
			value = VariantUtils.CreateFrom(in backButton);
			return true;
		}
		if (name == PropertyName.editUser)
		{
			value = VariantUtils.CreateFrom(in editUser);
			return true;
		}
		if (name == PropertyName.pendingImportData)
		{
			value = VariantUtils.CreateFrom(in pendingImportData);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.userContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.renameButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.finishButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.deleteButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cancelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.createUserButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.logOutButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.backButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.editUser, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.pendingImportData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.userContainer, Variant.From(in userContainer));
		info.AddProperty(PropertyName.renameButton, Variant.From(in renameButton));
		info.AddProperty(PropertyName.finishButton, Variant.From(in finishButton));
		info.AddProperty(PropertyName.deleteButton, Variant.From(in deleteButton));
		info.AddProperty(PropertyName.cancelButton, Variant.From(in cancelButton));
		info.AddProperty(PropertyName.createUserButton, Variant.From(in createUserButton));
		info.AddProperty(PropertyName.logOutButton, Variant.From(in logOutButton));
		info.AddProperty(PropertyName.backButton, Variant.From(in backButton));
		info.AddProperty(PropertyName.editUser, Variant.From(in editUser));
		info.AddProperty(PropertyName.pendingImportData, Variant.From(in pendingImportData));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.userContainer, out var value))
		{
			userContainer = value.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.renameButton, out var value2))
		{
			renameButton = value2.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.finishButton, out var value3))
		{
			finishButton = value3.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.deleteButton, out var value4))
		{
			deleteButton = value4.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.cancelButton, out var value5))
		{
			cancelButton = value5.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.createUserButton, out var value6))
		{
			createUserButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.logOutButton, out var value7))
		{
			logOutButton = value7.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.backButton, out var value8))
		{
			backButton = value8.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.editUser, out var value9))
		{
			editUser = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.pendingImportData, out var value10))
		{
			pendingImportData = value10.As<Dictionary>();
		}
	}
}
