using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.Registry.Class;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;
using PVZHE.ModEditor.ScriptEditor;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Resource/XWResourcePicker.cs")]
public class XWResourcePicker : HBoxContainer
{
	[Signal]
	public delegate void ResourceChangedEventHandler(Resource resource);

	[Signal]
	public delegate void ResourceSelectedEventHandler(Resource resource);

	public new class MethodName : HBoxContainer.MethodName
	{
		public static readonly StringName Create = "Create";

		public static readonly StringName EnsureIconsLoaded = "EnsureIconsLoaded";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Setup = "Setup";

		public static readonly StringName SetEditedResource = "SetEditedResource";

		public static readonly StringName GetEditedResource = "GetEditedResource";

		public static readonly StringName UpdateDisplay = "UpdateDisplay";

		public static readonly StringName ApplyTexturePreviewLayout = "ApplyTexturePreviewLayout";

		public static readonly StringName ClearTexturePreview = "ClearTexturePreview";

		public static readonly StringName OnAssignButtonPressed = "OnAssignButtonPressed";

		public static readonly StringName OnAssignButtonGuiInput = "OnAssignButtonGuiInput";

		public static readonly StringName ShowAdvancedResourceMenu = "ShowAdvancedResourceMenu";

		public static readonly StringName AddNewResourceItems = "AddNewResourceItems";

		public static readonly StringName OnMenuItemPressed = "OnMenuItemPressed";

		public static readonly StringName OnQuickLoadButtonPressed = "OnQuickLoadButtonPressed";

		public static readonly StringName OnMakeUniqueButtonPressed = "OnMakeUniqueButtonPressed";

		public static readonly StringName OnEditButtonPressed = "OnEditButtonPressed";

		public static readonly StringName LoadResource = "LoadResource";

		public static readonly StringName QuickLoadResource = "QuickLoadResource";

		public static readonly StringName OpenVisualResourceLibrary = "OpenVisualResourceLibrary";

		public static readonly StringName EnsureResourceLibraryPicker = "EnsureResourceLibraryPicker";

		public static readonly StringName OnVisualResourceChosen = "OnVisualResourceChosen";

		public static readonly StringName OnResourceFileSelected = "OnResourceFileSelected";

		public static readonly StringName LoadResourceFromPath = "LoadResourceFromPath";

		public static readonly StringName LoadExternalTexture = "LoadExternalTexture";

		public static readonly StringName LoadSvgImage = "LoadSvgImage";

		public static readonly StringName IsImagePath = "IsImagePath";

		public static readonly StringName GetLoadablePath = "GetLoadablePath";

		public static readonly StringName NewResource = "NewResource";

		public static readonly StringName CreateResourceInstance = "CreateResourceInstance";

		public static readonly StringName ClearResource = "ClearResource";

		public static readonly StringName CopyResourcePath = "CopyResourcePath";

		public static readonly StringName PasteResourceFromClipboard = "PasteResourceFromClipboard";

		public static readonly StringName MakeUnique = "MakeUnique";

		public static readonly StringName MakeBuiltIn = "MakeBuiltIn";

		public static readonly StringName SaveResource = "SaveResource";

		public static readonly StringName SaveAsResource = "SaveAsResource";

		public static readonly StringName OnSaveResourcePathSelected = "OnSaveResourcePathSelected";

		public static readonly StringName ShowInFileSystem = "ShowInFileSystem";

		public static readonly StringName UpdateEditedResource = "UpdateEditedResource";

		public static readonly StringName GetDragData = "GetDragData";

		public static readonly StringName CanDropDataForwarded = "CanDropDataForwarded";

		public static readonly StringName DropDataForwarded = "DropDataForwarded";

		public new static readonly StringName _CanDropData = "_CanDropData";

		public new static readonly StringName _DropData = "_DropData";

		public static readonly StringName IsDropValidAndRedraw = "IsDropValidAndRedraw";

		public static readonly StringName DropResourceData = "DropResourceData";

		public static readonly StringName IsDropValid = "IsDropValid";

		public static readonly StringName IsTypeValid = "IsTypeValid";

		public static readonly StringName IsDropResourcePathValid = "IsDropResourcePathValid";

		public static readonly StringName IsResourceTypeNameValid = "IsResourceTypeNameValid";

		public static readonly StringName GetFallbackResourceType = "GetFallbackResourceType";

		public static readonly StringName GetResourceTypeNameFromPath = "GetResourceTypeNameFromPath";

		public static readonly StringName GetTypeNameFromTextResourceHeader = "GetTypeNameFromTextResourceHeader";

		public static readonly StringName GetQuotedHeaderAttribute = "GetQuotedHeaderAttribute";

		public static readonly StringName GetResourceFromDropData = "GetResourceFromDropData";

		public static readonly StringName OnAssignButtonDraw = "OnAssignButtonDraw";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName Notify = "Notify";
	}

	public new class PropertyName : HBoxContainer.PropertyName
	{
		public static readonly StringName BaseType = "BaseType";

		public static readonly StringName EditedResource = "EditedResource";

		public static readonly StringName AllowClear = "AllowClear";

		public static readonly StringName AllowNew = "AllowNew";

		public static readonly StringName _makeUniqueButton = "_makeUniqueButton";

		public static readonly StringName _assignButton = "_assignButton";

		public static readonly StringName _quickLoadButton = "_quickLoadButton";

		public static readonly StringName _editButton = "_editButton";

		public static readonly StringName _menuPopup = "_menuPopup";

		public static readonly StringName _previewRect = "_previewRect";

		public static readonly StringName _loadResourceDialog = "_loadResourceDialog";

		public static readonly StringName _quickLoadResourceDialog = "_quickLoadResourceDialog";

		public static readonly StringName _saveResourceAsDialog = "_saveResourceAsDialog";

		public static readonly StringName _resourceLibraryPicker = "_resourceLibraryPicker";

		public static readonly StringName _hoveringValidDrop = "_hoveringValidDrop";

		public static readonly StringName _pendingDisplayUpdate = "_pendingDisplayUpdate";
	}

	public new class SignalName : HBoxContainer.SignalName
	{
		public static readonly StringName ResourceChanged = "ResourceChanged";

		public static readonly StringName ResourceSelected = "ResourceSelected";
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Resource/XWResourcePicker.tscn";

	private static readonly Vector2 PickerMinimumSize = new Vector2(0f, 28f);

	private static readonly Vector2 AssignButtonMinimumSize = new Vector2(0f, 28f);

	private static readonly Vector2 TexturePreviewMinimumSize = new Vector2(0f, 32f);

	private const int MenuInspect = 0;

	private const int MenuLoad = 100;

	private const int MenuQuickLoad = 101;

	private const int MenuCopyPath = 200;

	private const int MenuPastePath = 201;

	private const int MenuClear = 300;

	private const int MenuMakeUnique = 400;

	private const int MenuMakeBuiltIn = 401;

	private const int MenuSave = 500;

	private const int MenuSaveAs = 501;

	private const int MenuShowInFilesystem = 600;

	private const int MenuNewResourceStart = 1000;

	private static Texture2D _iconInstance;

	private static Texture2D _iconLoad;

	private static Texture2D _iconClear;

	private static Texture2D _iconDuplicate;

	private static Texture2D _iconSave;

	private static Texture2D _iconSearch;

	private static Texture2D _iconShowInFilesystem;

	private static Texture2D _iconObject;

	private static Texture2D _iconActionCopy;

	private static Texture2D _iconActionPaste;

	private static Texture2D _iconTreeArrow;

	private Button _makeUniqueButton;

	private Button _assignButton;

	private Button _quickLoadButton;

	private Button _editButton;

	private PopupMenu _menuPopup;

	private TextureRect _previewRect;

	private FileDialog _loadResourceDialog;

	private FileDialog _quickLoadResourceDialog;

	private FileDialog _saveResourceAsDialog;

	private XWGameplayResourcePickerWindow _resourceLibraryPicker;

	private bool _hoveringValidDrop;

	private bool _pendingDisplayUpdate;

	private ResourceChangedEventHandler backing_ResourceChanged;

	private ResourceSelectedEventHandler backing_ResourceSelected;

	public string BaseType { get; private set; } = "Resource";

	public Resource EditedResource { get; private set; }

	public bool AllowClear { get; private set; } = true;

	public bool AllowNew { get; private set; } = true;

	public event ResourceChangedEventHandler ResourceChanged
	{
		add
		{
			backing_ResourceChanged = (ResourceChangedEventHandler)Delegate.Combine(backing_ResourceChanged, value);
		}
		remove
		{
			backing_ResourceChanged = (ResourceChangedEventHandler)Delegate.Remove(backing_ResourceChanged, value);
		}
	}

	public event ResourceSelectedEventHandler ResourceSelected
	{
		add
		{
			backing_ResourceSelected = (ResourceSelectedEventHandler)Delegate.Combine(backing_ResourceSelected, value);
		}
		remove
		{
			backing_ResourceSelected = (ResourceSelectedEventHandler)Delegate.Remove(backing_ResourceSelected, value);
		}
	}

	public static XWResourcePicker Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Resource/XWResourcePicker.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWResourcePicker>(PackedScene.GenEditState.Disabled);
	}

	private static void EnsureIconsLoaded()
	{
		if (!GodotObject.IsInstanceValid(_iconInstance))
		{
			_iconInstance = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Instance.svg", null, ResourceLoader.CacheMode.Reuse));
			_iconLoad = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Load.svg", null, ResourceLoader.CacheMode.Reuse));
			_iconClear = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Clear.svg", null, ResourceLoader.CacheMode.Reuse));
			_iconDuplicate = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Duplicate.svg", null, ResourceLoader.CacheMode.Reuse));
			_iconSave = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Save.svg", null, ResourceLoader.CacheMode.Reuse));
			_iconSearch = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Search.svg", null, ResourceLoader.CacheMode.Reuse));
			_iconShowInFilesystem = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ShowInFileSystem.svg", null, ResourceLoader.CacheMode.Reuse));
			_iconObject = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Object.svg", null, ResourceLoader.CacheMode.Reuse));
			_iconActionCopy = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ActionCopy.svg", null, ResourceLoader.CacheMode.Reuse));
			_iconActionPaste = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ActionPaste.svg", null, ResourceLoader.CacheMode.Reuse));
			_iconTreeArrow = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/GuiTreeArrowDown.svg", null, ResourceLoader.CacheMode.Reuse));
		}
	}

	public override void _Ready()
	{
		EnsureIconsLoaded();
		_makeUniqueButton = GetNode<Button>("%MakeUniqueButton");
		_assignButton = GetNode<Button>("%AssignButton");
		_quickLoadButton = GetNode<Button>("%QuickLoadButton");
		_editButton = GetNode<Button>("%EditButton");
		_menuPopup = GetNode<PopupMenu>("%MenuPopup");
		_previewRect = GetNode<TextureRect>("%PreviewRect");
		_loadResourceDialog = GetNode<FileDialog>("%LoadResourceDialog");
		_quickLoadResourceDialog = GetNode<FileDialog>("%QuickLoadResourceDialog");
		_saveResourceAsDialog = GetNode<FileDialog>("%SaveResourceAsDialog");
		_makeUniqueButton.Icon = _iconInstance;
		_makeUniqueButton.TooltipText = "设为唯一资源";
		_makeUniqueButton.Pressed += OnMakeUniqueButtonPressed;
		_assignButton.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
		_assignButton.Pressed += OnAssignButtonPressed;
		_assignButton.GuiInput += OnAssignButtonGuiInput;
		_assignButton.Draw += OnAssignButtonDraw;
		_assignButton.SetDragForwarding(Callable.From<Vector2, Variant>(GetDragData), Callable.From<Vector2, Variant, bool>(CanDropDataForwarded), Callable.From<Vector2, Variant>(DropDataForwarded));
		_quickLoadButton.Icon = _iconLoad;
		_quickLoadButton.TooltipText = "快速加载资源";
		_quickLoadButton.Pressed += OnQuickLoadButtonPressed;
		_editButton.Icon = _iconTreeArrow;
		_editButton.TooltipText = "检查资源";
		_editButton.Pressed += OnEditButtonPressed;
		_menuPopup.IdPressed += OnMenuItemPressed;
		_loadResourceDialog.FileSelected += OnResourceFileSelected;
		_quickLoadResourceDialog.FileSelected += OnResourceFileSelected;
		_saveResourceAsDialog.FileSelected += OnSaveResourcePathSelected;
		if (_pendingDisplayUpdate)
		{
			_pendingDisplayUpdate = false;
			UpdateDisplay();
		}
	}

	public void Setup(string baseType, bool allowClear = true, bool allowNew = true)
	{
		BaseType = (string.IsNullOrWhiteSpace(baseType) ? "Resource" : baseType);
		AllowClear = allowClear;
		AllowNew = allowNew;
		UpdateDisplay();
	}

	public void SetEditedResource(Resource resource)
	{
		if (EditedResource != resource)
		{
			EditedResource = resource;
			UpdateDisplay();
		}
	}

	public Resource GetEditedResource()
	{
		return EditedResource;
	}

	private void UpdateDisplay()
	{
		if (!IsNodeReady())
		{
			_pendingDisplayUpdate = true;
		}
		else if (GodotObject.IsInstanceValid(EditedResource))
		{
			string text = EditedResource.GetClass();
			Variant script = EditedResource.GetScript();
			string text2 = text;
			GodotObject godotObject = script.As<GodotObject>();
			if (GodotObject.IsInstanceValid(godotObject) && godotObject is Script script2)
			{
				string text3 = script2.GetGlobalName();
				if (!string.IsNullOrEmpty(text3))
				{
					text2 = text3 + " (" + text + ")";
				}
			}
			string resourcePath = EditedResource.ResourcePath;
			if (!string.IsNullOrEmpty(resourcePath))
			{
				text2 = resourcePath.GetFile();
			}
			_assignButton.Text = "  " + text2;
			_assignButton.TooltipText = "类型: " + text + "\n路径: " + (string.IsNullOrEmpty(resourcePath) ? "<内置>" : resourcePath);
			Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(text);
			_assignButton.Icon = (GodotObject.IsInstanceValid(classIcon) ? classIcon : _iconObject);
			if (EditedResource is Texture2D texture && XWTextureSafety.CanPreview(texture))
			{
				ApplyTexturePreviewLayout(texture);
			}
			else
			{
				ClearTexturePreview();
			}
			_makeUniqueButton.Visible = true;
			_quickLoadButton.Visible = true;
			_editButton.Visible = true;
		}
		else
		{
			_assignButton.Text = "  <空 " + BaseType + ">";
			_assignButton.Icon = _iconObject;
			_assignButton.TooltipText = "";
			ClearTexturePreview();
			_makeUniqueButton.Visible = false;
			_quickLoadButton.Visible = false;
			_editButton.Visible = false;
		}
	}

	private void ApplyTexturePreviewLayout(Texture2D texture)
	{
		if (GodotObject.IsInstanceValid(_assignButton) && GodotObject.IsInstanceValid(_previewRect))
		{
			CustomMinimumSize = TexturePreviewMinimumSize;
			_assignButton.CustomMinimumSize = TexturePreviewMinimumSize;
			_assignButton.Text = "";
			_assignButton.Icon = null;
			_previewRect.CustomMinimumSize = TexturePreviewMinimumSize;
			_previewRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
			_previewRect.Texture = texture;
			_previewRect.Visible = true;
		}
	}

	private void ClearTexturePreview()
	{
		CustomMinimumSize = PickerMinimumSize;
		if (GodotObject.IsInstanceValid(_assignButton))
		{
			_assignButton.CustomMinimumSize = AssignButtonMinimumSize;
		}
		if (GodotObject.IsInstanceValid(_previewRect))
		{
			_previewRect.Texture = null;
			_previewRect.Visible = false;
		}
	}

	private void OnAssignButtonPressed()
	{
		if (!OpenVisualResourceLibrary())
		{
			ShowAdvancedResourceMenu();
		}
	}

	private void OnAssignButtonGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Right && inputEventMouseButton.Pressed)
		{
			ShowAdvancedResourceMenu();
			_assignButton.AcceptEvent();
		}
	}

	private void ShowAdvancedResourceMenu()
	{
		_menuPopup.Clear();
		if (GodotObject.IsInstanceValid(EditedResource))
		{
			_menuPopup.AddIconItem(_iconSearch, "检查", 0, Key.None);
			_menuPopup.AddSeparator();
		}
		if (AllowNew)
		{
			AddNewResourceItems();
		}
		_menuPopup.AddIconItem(_iconLoad, "加载...", 100, Key.None);
		if (GodotObject.IsInstanceValid(EditedResource))
		{
			_menuPopup.AddIconItem(_iconLoad, "快速加载...", 101, Key.None);
			_menuPopup.AddSeparator();
			_menuPopup.AddIconItem(_iconActionCopy, "复制资源路径", 200, Key.None);
			_menuPopup.AddIconItem(_iconActionPaste, "粘贴资源路径", 201, Key.None);
			if (AllowClear)
			{
				_menuPopup.AddSeparator();
				_menuPopup.AddIconItem(_iconClear, "清除", 300, Key.None);
			}
			_menuPopup.AddSeparator();
			_menuPopup.AddIconItem(_iconDuplicate, "设为唯一", 400, Key.None);
			_menuPopup.AddIconItem(_iconInstance, "设为内置", 401, Key.None);
			_menuPopup.AddIconItem(_iconSave, "保存", 500, Key.None);
			_menuPopup.AddIconItem(_iconSave, "另存为...", 501, Key.None);
			_menuPopup.AddSeparator();
			_menuPopup.AddIconItem(_iconShowInFilesystem, "在文件系统中显示", 600, Key.None);
		}
		_menuPopup.ResetSize();
		_menuPopup.Position = new Vector2I((int)_assignButton.GlobalPosition.X, (int)(_assignButton.GlobalPosition.Y + _assignButton.Size.Y));
		_menuPopup.Popup();
	}

	private void AddNewResourceItems()
	{
		List<string> creatableResourceTypes = GetCreatableResourceTypes();
		if (creatableResourceTypes.Count != 0)
		{
			_menuPopup.AddSeparator();
			for (int i = 0; i < creatableResourceTypes.Count; i++)
			{
				string text = creatableResourceTypes[i];
				Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(text);
				Texture2D texture = (GodotObject.IsInstanceValid(classIcon) ? classIcon : _iconObject);
				_menuPopup.AddIconItem(texture, "新建 " + text, 1000 + i, Key.None);
			}
		}
	}

	private void OnMenuItemPressed(long id)
	{
		switch (id)
		{
		case 0L:
			EmitSignal(SignalName.ResourceSelected, EditedResource);
			return;
		case 100L:
			LoadResource();
			return;
		case 101L:
			QuickLoadResource();
			return;
		case 200L:
			CopyResourcePath();
			return;
		case 201L:
			PasteResourceFromClipboard();
			return;
		case 300L:
			ClearResource();
			return;
		case 400L:
			MakeUnique();
			return;
		case 401L:
			MakeBuiltIn();
			return;
		case 500L:
			SaveResource();
			return;
		case 501L:
			SaveAsResource();
			return;
		case 600L:
			ShowInFileSystem();
			return;
		}
		if (id >= 1000)
		{
			NewResource((int)id - 1000);
		}
	}

	private void OnQuickLoadButtonPressed()
	{
		QuickLoadResource();
	}

	private void OnMakeUniqueButtonPressed()
	{
		MakeUnique();
	}

	private void OnEditButtonPressed()
	{
		if (GodotObject.IsInstanceValid(EditedResource))
		{
			EmitSignal(SignalName.ResourceSelected, EditedResource);
		}
	}

	private void LoadResource()
	{
		if (!OpenVisualResourceLibrary())
		{
			_loadResourceDialog.PopupCentered();
		}
	}

	private void QuickLoadResource()
	{
		if (!OpenVisualResourceLibrary())
		{
			_quickLoadResourceDialog.PopupCentered();
		}
	}

	private bool OpenVisualResourceLibrary()
	{
		EnsureResourceLibraryPicker();
		if (!GodotObject.IsInstanceValid(_resourceLibraryPicker))
		{
			return false;
		}
		XWResourcePickerLibraryProfile xWResourcePickerLibraryProfile = XWResourcePickerLibraryProfileResolver.Resolve(BaseType);
		string currentPath = (GodotObject.IsInstanceValid(EditedResource) ? EditedResource.ResourcePath : "");
		string projectPath = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
		_resourceLibraryPicker.OpenResourceLibrary(xWResourcePickerLibraryProfile.Category, xWResourcePickerLibraryProfile.DisplayName, currentPath, projectPath, xWResourcePickerLibraryProfile.ClassNames, xWResourcePickerLibraryProfile.BuiltInPathMarkers, xWResourcePickerLibraryProfile.IconPath, (XWGameplayResourceChoice choice) =>
		{
			OnVisualResourceChosen(choice.ResourcePath);
		});
		return true;
	}

	private void EnsureResourceLibraryPicker()
	{
		if (!GodotObject.IsInstanceValid(_resourceLibraryPicker))
		{
			_resourceLibraryPicker = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_resourceLibraryPicker))
			{
				AddChild(_resourceLibraryPicker, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void OnVisualResourceChosen(string path)
	{
		OnResourceFileSelected(path);
	}

	private void OnResourceFileSelected(string path)
	{
		Resource resource = LoadResourceFromPath(path);
		if (GodotObject.IsInstanceValid(resource))
		{
			UpdateEditedResource(resource);
		}
	}

	private Resource LoadResourceFromPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		if (XWCSharpOnlyPolicy.IsUnsupportedScriptSourcePath(path))
		{
			Notify("Mod 脚本仅支持 C#，不能加载其他脚本源文件。");
			return null;
		}
		Resource resource = null;
		if (IsImagePath(path))
		{
			resource = LoadExternalTexture(path);
			if (GodotObject.IsInstanceValid(resource) && !IsTypeValid(resource))
			{
				resource = null;
			}
		}
		if (!GodotObject.IsInstanceValid(resource) && ResourceLoader.Exists(path))
		{
			try
			{
				resource = ResourceLoader.Load<Resource>(path, null, ResourceLoader.CacheMode.Reuse);
			}
			catch (Exception ex)
			{
				GD.PushWarning($"XWResourcePicker: load resource failed: {path} ({ex.Message})");
			}
		}
		if (resource == null)
		{
			resource = LoadExternalTexture(path);
		}
		if (!GodotObject.IsInstanceValid(resource))
		{
			Notify("无法加载资源: " + path);
			return null;
		}
		if (!IsTypeValid(resource))
		{
			Notify("资源类型不匹配: 需要 " + BaseType + "，实际为 " + resource.GetClass());
			return null;
		}
		return resource;
	}

	private Texture2D LoadExternalTexture(string path)
	{
		if (!IsImagePath(path))
		{
			return null;
		}
		string loadablePath = GetLoadablePath(path);
		if (!FileAccess.FileExists(loadablePath))
		{
			return null;
		}
		Image image = new Image();
		if (((path.GetExtension().ToLowerInvariant() == "svg") ? LoadSvgImage(loadablePath, image) : image.Load(loadablePath)) != Error.Ok || image.GetWidth() <= 0 || image.GetHeight() <= 0)
		{
			return null;
		}
		ImageTexture imageTexture = ImageTexture.CreateFromImage(image);
		imageTexture.ResourcePath = path;
		return imageTexture;
	}

	private static Error LoadSvgImage(string path, Image image)
	{
		byte[] fileAsBytes = FileAccess.GetFileAsBytes(path);
		if (fileAsBytes.Length != 0)
		{
			return image.LoadSvgFromBuffer(fileAsBytes);
		}
		return Error.FileCantRead;
	}

	private static bool IsImagePath(string path)
	{
		switch (path.GetExtension().ToLowerInvariant())
		{
		case "png":
		case "jpg":
		case "jpeg":
		case "webp":
		case "svg":
			return true;
		default:
			return false;
		}
	}

	private static string GetLoadablePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		if (path.StartsWith("res://") || path.StartsWith("user://"))
		{
			return ProjectSettings.GlobalizePath(path);
		}
		return path.Replace('\\', '/');
	}

	private void NewResource(int typeIndex)
	{
		List<string> creatableResourceTypes = GetCreatableResourceTypes();
		if (typeIndex >= 0 && typeIndex < creatableResourceTypes.Count)
		{
			Resource resource = CreateResourceInstance(creatableResourceTypes[typeIndex]);
			if (GodotObject.IsInstanceValid(resource))
			{
				UpdateEditedResource(resource);
			}
		}
	}

	private Resource CreateResourceInstance(string typeName)
	{
		if (string.IsNullOrWhiteSpace(typeName))
		{
			return null;
		}
		if (XWClassRegistry.Instance.Instantiate(typeName).AsGodotObject() is Resource result)
		{
			return result;
		}
		GD.PushWarning("XWResourcePicker: 无法创建资源类型 " + typeName);
		return null;
	}

	private List<string> GetCreatableResourceTypes()
	{
		List<string> list = new List<string>();
		foreach (string item in XWClassRegistry.Instance.GetInheritersFromClass(BaseType))
		{
			string text = item.ToString();
			if (XWTextureSafety.CanCreateResourceType(text) && XWClassRegistry.Instance.CanInstantiate(text) && !list.Contains(text))
			{
				list.Add(text);
			}
		}
		if (XWTextureSafety.CanCreateResourceType(BaseType) && XWClassRegistry.Instance.CanInstantiate(BaseType) && !list.Contains(BaseType))
		{
			list.Insert(0, BaseType);
		}
		return list;
	}

	private void ClearResource()
	{
		UpdateEditedResource(null);
	}

	private void CopyResourcePath()
	{
		if (GodotObject.IsInstanceValid(EditedResource))
		{
			string resourcePath = EditedResource.ResourcePath;
			if (string.IsNullOrWhiteSpace(resourcePath))
			{
				Notify("内置资源没有文件路径。");
				return;
			}
			DisplayServer.ClipboardSet(resourcePath);
			Notify("已复制资源路径: " + resourcePath);
		}
	}

	private void PasteResourceFromClipboard()
	{
		string text = DisplayServer.ClipboardGet().StripEdges();
		if (!string.IsNullOrEmpty(text))
		{
			Resource resource = LoadResourceFromPath(text);
			if (GodotObject.IsInstanceValid(resource))
			{
				UpdateEditedResource(resource);
			}
		}
	}

	private void MakeUnique()
	{
		if (GodotObject.IsInstanceValid(EditedResource))
		{
			Resource resource = EditedResource.Duplicate();
			if (GodotObject.IsInstanceValid(resource))
			{
				resource.ResourcePath = "";
				UpdateEditedResource(resource);
			}
		}
	}

	private void MakeBuiltIn()
	{
		if (GodotObject.IsInstanceValid(EditedResource))
		{
			EditedResource.ResourcePath = "";
			UpdateEditedResource(EditedResource);
		}
	}

	private void SaveResource()
	{
		if (GodotObject.IsInstanceValid(EditedResource))
		{
			string resourcePath = EditedResource.ResourcePath;
			if (string.IsNullOrEmpty(resourcePath))
			{
				SaveAsResource();
				return;
			}
			Error error = ResourceSaver.Save(EditedResource, resourcePath, ResourceSaver.SaverFlags.None);
			Notify((error == Error.Ok) ? ("已保存: " + resourcePath) : $"保存失败: {error}");
		}
	}

	private void SaveAsResource()
	{
		if (GodotObject.IsInstanceValid(EditedResource))
		{
			_saveResourceAsDialog.CurrentFile = EditedResource.ResourcePath.GetFile();
			_saveResourceAsDialog.PopupCentered();
		}
	}

	private void OnSaveResourcePathSelected(string path)
	{
		Error error = ResourceSaver.Save(EditedResource, path, ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			Notify($"保存失败: {error}");
		}
		else
		{
			EditedResource.ResourcePath = path;
			UpdateDisplay();
			EmitSignal(SignalName.ResourceChanged, EditedResource);
			Notify("已保存: " + path);
		}
	}

	private void ShowInFileSystem()
	{
		if (!GodotObject.IsInstanceValid(EditedResource))
		{
			return;
		}
		string resourcePath = EditedResource.ResourcePath;
		if (string.IsNullOrEmpty(resourcePath))
		{
			Notify("内置资源没有文件系统位置。");
			return;
		}
		Control control = XWEditorInterface.Instance?.GetFileSystemPanel();
		if (GodotObject.IsInstanceValid(control) && control.HasMethod("NavigateToPath"))
		{
			control.Call("NavigateToPath", resourcePath);
			XWEditorInterface.Instance?.FocusPanel("file_system");
		}
	}

	private void UpdateEditedResource(Resource resource)
	{
		if (GodotObject.IsInstanceValid(resource) && !IsTypeValid(resource))
		{
			Notify("资源类型不匹配: 需要 " + BaseType + "，实际为 " + resource.GetClass());
			return;
		}
		SetEditedResource(resource);
		EmitSignal(SignalName.ResourceChanged, resource);
	}

	private Variant GetDragData(Vector2 atPosition)
	{
		return default;
	}

	private bool CanDropDataForwarded(Vector2 atPosition, Variant data)
	{
		return IsDropValidAndRedraw(data);
	}

	private void DropDataForwarded(Vector2 atPosition, Variant data)
	{
		DropResourceData(data);
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return IsDropValidAndRedraw(data);
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		DropResourceData(data);
	}

	private bool IsDropValidAndRedraw(Variant data)
	{
		_hoveringValidDrop = IsDropValid(data);
		_assignButton.QueueRedraw();
		return _hoveringValidDrop;
	}

	private void DropResourceData(Variant data)
	{
		Resource resourceFromDropData = GetResourceFromDropData(data, loadFiles: true);
		if (GodotObject.IsInstanceValid(resourceFromDropData))
		{
			UpdateEditedResource(resourceFromDropData);
		}
	}

	private bool IsDropValid(Variant data)
	{
		Resource resourceFromDropData = GetResourceFromDropData(data, loadFiles: false);
		if (GodotObject.IsInstanceValid(resourceFromDropData))
		{
			return IsTypeValid(resourceFromDropData);
		}
		if (TryGetDropResourcePath(data, out var path))
		{
			return IsDropResourcePathValid(path);
		}
		return false;
	}

	private bool IsTypeValid(Resource res)
	{
		if (!GodotObject.IsInstanceValid(res))
		{
			return AllowClear;
		}
		if (!XWCSharpOnlyPolicy.IsSupportedModScriptResource(res))
		{
			return false;
		}
		if (string.IsNullOrEmpty(BaseType) || BaseType == "Resource")
		{
			return true;
		}
		if (res.IsClass(BaseType))
		{
			return true;
		}
		if (ClassDB.ClassExists(res.GetClass()) && ClassDB.ClassExists(BaseType) && ClassDB.IsParentClass(res.GetClass(), BaseType))
		{
			return true;
		}
		Script script = res.GetScript().As<Script>();
		if (GodotObject.IsInstanceValid(script) && script.GetGlobalName() == (StringName)BaseType)
		{
			return true;
		}
		return false;
	}

	private bool IsDropResourcePathValid(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		if (XWCSharpOnlyPolicy.IsUnsupportedScriptSourcePath(path))
		{
			return false;
		}
		if (IsImagePath(path) && FileAccess.FileExists(GetLoadablePath(path)))
		{
			return IsResourceTypeNameValid("ImageTexture");
		}
		return IsResourceTypeNameValid(GetResourceTypeNameFromPath(path));
	}

	private bool IsResourceTypeNameValid(string typeName)
	{
		if (string.IsNullOrWhiteSpace(typeName))
		{
			return false;
		}
		if (string.IsNullOrEmpty(BaseType) || BaseType == "Resource")
		{
			return true;
		}
		if (typeName == BaseType)
		{
			return true;
		}
		if (ClassDB.ClassExists(typeName) && ClassDB.ClassExists(BaseType) && ClassDB.IsParentClass(typeName, BaseType))
		{
			return true;
		}
		return XWClassRegistry.Instance.IsClassInstanceOf(typeName, BaseType);
	}

	private static string GetFallbackResourceType(string path)
	{
		switch (path.GetExtension().ToLowerInvariant())
		{
		case "tscn":
		case "scn":
			return "PackedScene";
		case "cs":
			return "CSharpScript";
		case "jpeg":
		case "webp":
		case "svg":
		case "png":
		case "jpg":
			return "Texture2D";
		case "wav":
		case "ogg":
		case "mp3":
			return "AudioStream";
		default:
			return "";
		}
	}

	private static string GetResourceTypeNameFromPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		string extension = path.GetExtension().ToLowerInvariant();
		string typeNameFromTextResourceHeader = GetTypeNameFromTextResourceHeader(path, extension);
		if (!string.IsNullOrEmpty(typeNameFromTextResourceHeader))
		{
			return typeNameFromTextResourceHeader;
		}
		return GetFallbackResourceType(path);
	}

	private static string GetTypeNameFromTextResourceHeader(string path, string extension)
	{
		if (extension != "tres" && extension != "tscn")
		{
			return "";
		}
		if (!FileAccess.FileExists(path))
		{
			return "";
		}
		using FileAccess fileAccess = FileAccess.Open(path, FileAccess.ModeFlags.Read);
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
				string quotedHeaderAttribute = GetQuotedHeaderAttribute(text, "script_class");
				if (!string.IsNullOrEmpty(quotedHeaderAttribute))
				{
					return quotedHeaderAttribute;
				}
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

	private bool TryGetDropResourcePath(Variant data, out string path)
	{
		path = "";
		if (data.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		Dictionary dictionary = data.As<Dictionary>();
		if (!dictionary.ContainsKey("files"))
		{
			return false;
		}
		string[] array = dictionary["files"].AsStringArray();
		if (array.Length == 0 || string.IsNullOrWhiteSpace(array[0]))
		{
			return false;
		}
		path = array[0];
		return true;
	}

	private Resource GetResourceFromDropData(Variant data, bool loadFiles)
	{
		if (data.VariantType != Variant.Type.Dictionary)
		{
			return null;
		}
		Dictionary dictionary = data.As<Dictionary>();
		if (dictionary.ContainsKey("resource"))
		{
			return dictionary["resource"].As<Resource>();
		}
		if (loadFiles && TryGetDropResourcePath(data, out var path))
		{
			return LoadResourceFromPath(path);
		}
		return null;
	}

	private void OnAssignButtonDraw()
	{
		if (_hoveringValidDrop)
		{
			Rect2 rect = new Rect2(Vector2.Zero, _assignButton.Size);
			Color themeColor = GetThemeColor("accent_color", "Editor");
			_assignButton.DrawRect(rect, themeColor, filled: false, 2f);
		}
	}

	public override void _Notification(int what)
	{
		if ((long)what == 22)
		{
			_hoveringValidDrop = false;
			if (GodotObject.IsInstanceValid(_assignButton))
			{
				_assignButton.QueueRedraw();
			}
		}
	}

	private static void Notify(string message)
	{
		XWEditorInterface.Instance?.ShowToast(message);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(59)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.EnsureIconsLoaded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Setup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "baseType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "allowClear", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "allowNew", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetEditedResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetEditedResource, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyTexturePreviewLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearTexturePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAssignButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAssignButtonGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowAdvancedResourceMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddNewResourceItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMenuItemPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnQuickLoadButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMakeUniqueButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEditButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QuickLoadResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenVisualResourceLibrary, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureResourceLibraryPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualResourceChosen, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnResourceFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadResourceFromPath, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadExternalTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadSvgImage, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsImagePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLoadablePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NewResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "typeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateResourceInstance, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "typeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CopyResourcePath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PasteResourceFromClipboard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MakeUnique, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MakeBuiltIn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveAsResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSaveResourcePathSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowInFileSystem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateEditedResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetDragData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanDropDataForwarded, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.DropDataForwarded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
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
			new MethodInfo(MethodName.IsDropValidAndRedraw, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.DropResourceData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.IsDropValid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.IsTypeValid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "res", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsDropResourcePathValid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsResourceTypeNameValid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "typeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFallbackResourceType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetResourceTypeNameFromPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.GetResourceFromDropData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Bool, "loadFiles", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAssignButtonDraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Notify, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWResourcePicker>(Create());
			return true;
		}
		if (method == MethodName.EnsureIconsLoaded && args.Count == 0)
		{
			EnsureIconsLoaded();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.Setup && args.Count == 3)
		{
			Setup(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetEditedResource && args.Count == 1)
		{
			SetEditedResource(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetEditedResource && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Resource>(GetEditedResource());
			return true;
		}
		if (method == MethodName.UpdateDisplay && args.Count == 0)
		{
			UpdateDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyTexturePreviewLayout && args.Count == 1)
		{
			ApplyTexturePreviewLayout(VariantUtils.ConvertTo<Texture2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearTexturePreview && args.Count == 0)
		{
			ClearTexturePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAssignButtonPressed && args.Count == 0)
		{
			OnAssignButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAssignButtonGuiInput && args.Count == 1)
		{
			OnAssignButtonGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowAdvancedResourceMenu && args.Count == 0)
		{
			ShowAdvancedResourceMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.AddNewResourceItems && args.Count == 0)
		{
			AddNewResourceItems();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMenuItemPressed && args.Count == 1)
		{
			OnMenuItemPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnQuickLoadButtonPressed && args.Count == 0)
		{
			OnQuickLoadButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMakeUniqueButtonPressed && args.Count == 0)
		{
			OnMakeUniqueButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnEditButtonPressed && args.Count == 0)
		{
			OnEditButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadResource && args.Count == 0)
		{
			LoadResource();
			ret = default;
			return true;
		}
		if (method == MethodName.QuickLoadResource && args.Count == 0)
		{
			QuickLoadResource();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenVisualResourceLibrary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(OpenVisualResourceLibrary());
			return true;
		}
		if (method == MethodName.EnsureResourceLibraryPicker && args.Count == 0)
		{
			EnsureResourceLibraryPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualResourceChosen && args.Count == 1)
		{
			OnVisualResourceChosen(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnResourceFileSelected && args.Count == 1)
		{
			OnResourceFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadResourceFromPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(LoadResourceFromPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadExternalTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadExternalTexture(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadSvgImage && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Error>(LoadSvgImage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
			return true;
		}
		if (method == MethodName.IsImagePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsImagePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLoadablePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetLoadablePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NewResource && args.Count == 1)
		{
			NewResource(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateResourceInstance && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(CreateResourceInstance(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearResource && args.Count == 0)
		{
			ClearResource();
			ret = default;
			return true;
		}
		if (method == MethodName.CopyResourcePath && args.Count == 0)
		{
			CopyResourcePath();
			ret = default;
			return true;
		}
		if (method == MethodName.PasteResourceFromClipboard && args.Count == 0)
		{
			PasteResourceFromClipboard();
			ret = default;
			return true;
		}
		if (method == MethodName.MakeUnique && args.Count == 0)
		{
			MakeUnique();
			ret = default;
			return true;
		}
		if (method == MethodName.MakeBuiltIn && args.Count == 0)
		{
			MakeBuiltIn();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveResource && args.Count == 0)
		{
			SaveResource();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveAsResource && args.Count == 0)
		{
			SaveAsResource();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSaveResourcePathSelected && args.Count == 1)
		{
			OnSaveResourcePathSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowInFileSystem && args.Count == 0)
		{
			ShowInFileSystem();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateEditedResource && args.Count == 1)
		{
			UpdateEditedResource(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDragData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetDragData(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.CanDropDataForwarded && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanDropDataForwarded(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.DropDataForwarded && args.Count == 2)
		{
			DropDataForwarded(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
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
		if (method == MethodName.IsDropValidAndRedraw && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDropValidAndRedraw(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.DropResourceData && args.Count == 1)
		{
			DropResourceData(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsDropValid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDropValid(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.IsTypeValid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTypeValid(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.IsDropResourcePathValid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDropResourcePathValid(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsResourceTypeNameValid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsResourceTypeNameValid(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFallbackResourceType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetFallbackResourceType(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetResourceTypeNameFromPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourceTypeNameFromPath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.GetResourceFromDropData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Resource>(GetResourceFromDropData(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.OnAssignButtonDraw && args.Count == 0)
		{
			OnAssignButtonDraw();
			ret = default;
			return true;
		}
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Notify && args.Count == 1)
		{
			Notify(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWResourcePicker>(Create());
			return true;
		}
		if (method == MethodName.EnsureIconsLoaded && args.Count == 0)
		{
			EnsureIconsLoaded();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadSvgImage && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Error>(LoadSvgImage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
			return true;
		}
		if (method == MethodName.IsImagePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsImagePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLoadablePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetLoadablePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFallbackResourceType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetFallbackResourceType(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetResourceTypeNameFromPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourceTypeNameFromPath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.Notify && args.Count == 1)
		{
			Notify(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName.EnsureIconsLoaded)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.Setup)
		{
			return true;
		}
		if (method == MethodName.SetEditedResource)
		{
			return true;
		}
		if (method == MethodName.GetEditedResource)
		{
			return true;
		}
		if (method == MethodName.UpdateDisplay)
		{
			return true;
		}
		if (method == MethodName.ApplyTexturePreviewLayout)
		{
			return true;
		}
		if (method == MethodName.ClearTexturePreview)
		{
			return true;
		}
		if (method == MethodName.OnAssignButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnAssignButtonGuiInput)
		{
			return true;
		}
		if (method == MethodName.ShowAdvancedResourceMenu)
		{
			return true;
		}
		if (method == MethodName.AddNewResourceItems)
		{
			return true;
		}
		if (method == MethodName.OnMenuItemPressed)
		{
			return true;
		}
		if (method == MethodName.OnQuickLoadButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnMakeUniqueButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnEditButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LoadResource)
		{
			return true;
		}
		if (method == MethodName.QuickLoadResource)
		{
			return true;
		}
		if (method == MethodName.OpenVisualResourceLibrary)
		{
			return true;
		}
		if (method == MethodName.EnsureResourceLibraryPicker)
		{
			return true;
		}
		if (method == MethodName.OnVisualResourceChosen)
		{
			return true;
		}
		if (method == MethodName.OnResourceFileSelected)
		{
			return true;
		}
		if (method == MethodName.LoadResourceFromPath)
		{
			return true;
		}
		if (method == MethodName.LoadExternalTexture)
		{
			return true;
		}
		if (method == MethodName.LoadSvgImage)
		{
			return true;
		}
		if (method == MethodName.IsImagePath)
		{
			return true;
		}
		if (method == MethodName.GetLoadablePath)
		{
			return true;
		}
		if (method == MethodName.NewResource)
		{
			return true;
		}
		if (method == MethodName.CreateResourceInstance)
		{
			return true;
		}
		if (method == MethodName.ClearResource)
		{
			return true;
		}
		if (method == MethodName.CopyResourcePath)
		{
			return true;
		}
		if (method == MethodName.PasteResourceFromClipboard)
		{
			return true;
		}
		if (method == MethodName.MakeUnique)
		{
			return true;
		}
		if (method == MethodName.MakeBuiltIn)
		{
			return true;
		}
		if (method == MethodName.SaveResource)
		{
			return true;
		}
		if (method == MethodName.SaveAsResource)
		{
			return true;
		}
		if (method == MethodName.OnSaveResourcePathSelected)
		{
			return true;
		}
		if (method == MethodName.ShowInFileSystem)
		{
			return true;
		}
		if (method == MethodName.UpdateEditedResource)
		{
			return true;
		}
		if (method == MethodName.GetDragData)
		{
			return true;
		}
		if (method == MethodName.CanDropDataForwarded)
		{
			return true;
		}
		if (method == MethodName.DropDataForwarded)
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
		if (method == MethodName.IsDropValidAndRedraw)
		{
			return true;
		}
		if (method == MethodName.DropResourceData)
		{
			return true;
		}
		if (method == MethodName.IsDropValid)
		{
			return true;
		}
		if (method == MethodName.IsTypeValid)
		{
			return true;
		}
		if (method == MethodName.IsDropResourcePathValid)
		{
			return true;
		}
		if (method == MethodName.IsResourceTypeNameValid)
		{
			return true;
		}
		if (method == MethodName.GetFallbackResourceType)
		{
			return true;
		}
		if (method == MethodName.GetResourceTypeNameFromPath)
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
		if (method == MethodName.GetResourceFromDropData)
		{
			return true;
		}
		if (method == MethodName.OnAssignButtonDraw)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.Notify)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.BaseType)
		{
			BaseType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.EditedResource)
		{
			EditedResource = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName.AllowClear)
		{
			AllowClear = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.AllowNew)
		{
			AllowNew = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._makeUniqueButton)
		{
			_makeUniqueButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._assignButton)
		{
			_assignButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._quickLoadButton)
		{
			_quickLoadButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._editButton)
		{
			_editButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._menuPopup)
		{
			_menuPopup = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._previewRect)
		{
			_previewRect = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._loadResourceDialog)
		{
			_loadResourceDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._quickLoadResourceDialog)
		{
			_quickLoadResourceDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._saveResourceAsDialog)
		{
			_saveResourceAsDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._resourceLibraryPicker)
		{
			_resourceLibraryPicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._hoveringValidDrop)
		{
			_hoveringValidDrop = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingDisplayUpdate)
		{
			_pendingDisplayUpdate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.BaseType)
		{
			value = VariantUtils.CreateFrom<string>(BaseType);
			return true;
		}
		if (name == PropertyName.EditedResource)
		{
			value = VariantUtils.CreateFrom<Resource>(EditedResource);
			return true;
		}
		bool from;
		if (name == PropertyName.AllowClear)
		{
			from = AllowClear;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AllowNew)
		{
			from = AllowNew;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._makeUniqueButton)
		{
			value = VariantUtils.CreateFrom(in _makeUniqueButton);
			return true;
		}
		if (name == PropertyName._assignButton)
		{
			value = VariantUtils.CreateFrom(in _assignButton);
			return true;
		}
		if (name == PropertyName._quickLoadButton)
		{
			value = VariantUtils.CreateFrom(in _quickLoadButton);
			return true;
		}
		if (name == PropertyName._editButton)
		{
			value = VariantUtils.CreateFrom(in _editButton);
			return true;
		}
		if (name == PropertyName._menuPopup)
		{
			value = VariantUtils.CreateFrom(in _menuPopup);
			return true;
		}
		if (name == PropertyName._previewRect)
		{
			value = VariantUtils.CreateFrom(in _previewRect);
			return true;
		}
		if (name == PropertyName._loadResourceDialog)
		{
			value = VariantUtils.CreateFrom(in _loadResourceDialog);
			return true;
		}
		if (name == PropertyName._quickLoadResourceDialog)
		{
			value = VariantUtils.CreateFrom(in _quickLoadResourceDialog);
			return true;
		}
		if (name == PropertyName._saveResourceAsDialog)
		{
			value = VariantUtils.CreateFrom(in _saveResourceAsDialog);
			return true;
		}
		if (name == PropertyName._resourceLibraryPicker)
		{
			value = VariantUtils.CreateFrom(in _resourceLibraryPicker);
			return true;
		}
		if (name == PropertyName._hoveringValidDrop)
		{
			value = VariantUtils.CreateFrom(in _hoveringValidDrop);
			return true;
		}
		if (name == PropertyName._pendingDisplayUpdate)
		{
			value = VariantUtils.CreateFrom(in _pendingDisplayUpdate);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._makeUniqueButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._assignButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._quickLoadButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._menuPopup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadResourceDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._quickLoadResourceDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveResourceAsDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceLibraryPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.BaseType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.EditedResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AllowClear, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AllowNew, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hoveringValidDrop, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingDisplayUpdate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.BaseType, Variant.From<string>(BaseType));
		info.AddProperty(PropertyName.EditedResource, Variant.From<Resource>(EditedResource));
		info.AddProperty(PropertyName.AllowClear, Variant.From<bool>(AllowClear));
		info.AddProperty(PropertyName.AllowNew, Variant.From<bool>(AllowNew));
		info.AddProperty(PropertyName._makeUniqueButton, Variant.From(in _makeUniqueButton));
		info.AddProperty(PropertyName._assignButton, Variant.From(in _assignButton));
		info.AddProperty(PropertyName._quickLoadButton, Variant.From(in _quickLoadButton));
		info.AddProperty(PropertyName._editButton, Variant.From(in _editButton));
		info.AddProperty(PropertyName._menuPopup, Variant.From(in _menuPopup));
		info.AddProperty(PropertyName._previewRect, Variant.From(in _previewRect));
		info.AddProperty(PropertyName._loadResourceDialog, Variant.From(in _loadResourceDialog));
		info.AddProperty(PropertyName._quickLoadResourceDialog, Variant.From(in _quickLoadResourceDialog));
		info.AddProperty(PropertyName._saveResourceAsDialog, Variant.From(in _saveResourceAsDialog));
		info.AddProperty(PropertyName._resourceLibraryPicker, Variant.From(in _resourceLibraryPicker));
		info.AddProperty(PropertyName._hoveringValidDrop, Variant.From(in _hoveringValidDrop));
		info.AddProperty(PropertyName._pendingDisplayUpdate, Variant.From(in _pendingDisplayUpdate));
		info.AddSignalEventDelegate(SignalName.ResourceChanged, backing_ResourceChanged);
		info.AddSignalEventDelegate(SignalName.ResourceSelected, backing_ResourceSelected);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.BaseType, out var value))
		{
			BaseType = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.EditedResource, out var value2))
		{
			EditedResource = value2.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName.AllowClear, out var value3))
		{
			AllowClear = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.AllowNew, out var value4))
		{
			AllowNew = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._makeUniqueButton, out var value5))
		{
			_makeUniqueButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._assignButton, out var value6))
		{
			_assignButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._quickLoadButton, out var value7))
		{
			_quickLoadButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._editButton, out var value8))
		{
			_editButton = value8.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._menuPopup, out var value9))
		{
			_menuPopup = value9.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._previewRect, out var value10))
		{
			_previewRect = value10.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._loadResourceDialog, out var value11))
		{
			_loadResourceDialog = value11.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._quickLoadResourceDialog, out var value12))
		{
			_quickLoadResourceDialog = value12.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._saveResourceAsDialog, out var value13))
		{
			_saveResourceAsDialog = value13.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._resourceLibraryPicker, out var value14))
		{
			_resourceLibraryPicker = value14.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._hoveringValidDrop, out var value15))
		{
			_hoveringValidDrop = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingDisplayUpdate, out var value16))
		{
			_pendingDisplayUpdate = value16.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<ResourceChangedEventHandler>(SignalName.ResourceChanged, out var value17))
		{
			backing_ResourceChanged = value17;
		}
		if (info.TryGetSignalEventDelegate<ResourceSelectedEventHandler>(SignalName.ResourceSelected, out var value18))
		{
			backing_ResourceSelected = value18;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(SignalName.ResourceChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(SignalName.ResourceSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	protected void EmitSignalResourceChanged(Resource resource)
	{
		EmitSignal(SignalName.ResourceChanged, new ReadOnlySpan<Variant>((Variant)resource));
	}

	protected void EmitSignalResourceSelected(Resource resource)
	{
		EmitSignal(SignalName.ResourceSelected, new ReadOnlySpan<Variant>((Variant)resource));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ResourceChanged && args.Count == 1)
		{
			backing_ResourceChanged?.Invoke(VariantUtils.ConvertTo<Resource>(in args[0]));
		}
		else if (signal == SignalName.ResourceSelected && args.Count == 1)
		{
			backing_ResourceSelected?.Invoke(VariantUtils.ConvertTo<Resource>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ResourceChanged)
		{
			return true;
		}
		if (signal == SignalName.ResourceSelected)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
