using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Blueprint;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWBlueprintCreateDialog.cs")]
public class XWBlueprintCreateDialog : ConfirmationDialog
{
	[Signal]
	public delegate void BlueprintCreatedEventHandler(string blueprintPath);

	public new class MethodName : ConfirmationDialog.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Config = "Config";

		public static readonly StringName ApplyPendingConfig = "ApplyPendingConfig";

		public static readonly StringName OnParentSearchPressed = "OnParentSearchPressed";

		public static readonly StringName SelectParentPreset = "SelectParentPreset";

		public static readonly StringName OnBrowsePathPressed = "OnBrowsePathPressed";

		public static readonly StringName OnPathSelected = "OnPathSelected";

		public static readonly StringName UpdateDialogState = "UpdateDialogState";

		public static readonly StringName UpdateParentPresetSelection = "UpdateParentPresetSelection";

		public static readonly StringName OnConfirmed = "OnConfirmed";

		public static readonly StringName TryCreate = "TryCreate";

		public static readonly StringName ShowError = "ShowError";

		public static readonly StringName NormalizePath = "NormalizePath";

		public static readonly StringName IsValidFileName = "IsValidFileName";
	}

	public new class PropertyName : ConfirmationDialog.PropertyName
	{
		public static readonly StringName _parentName = "_parentName";

		public static readonly StringName _filePath = "_filePath";

		public static readonly StringName _messageLabel = "_messageLabel";

		public static readonly StringName _parentSearchButton = "_parentSearchButton";

		public static readonly StringName _pathButton = "_pathButton";

		public static readonly StringName _fileDialog = "_fileDialog";

		public static readonly StringName _nodeParentCard = "_nodeParentCard";

		public static readonly StringName _node2DParentCard = "_node2DParentCard";

		public static readonly StringName _controlParentCard = "_controlParentCard";

		public static readonly StringName _resourceParentCard = "_resourceParentCard";

		public static readonly StringName _pendingParentClass = "_pendingParentClass";

		public static readonly StringName _pendingPath = "_pendingPath";

		public static readonly StringName _isPathValid = "_isPathValid";
	}

	public new class SignalName : ConfirmationDialog.SignalName
	{
		public static readonly StringName BlueprintCreated = "BlueprintCreated";
	}

	private const string ScenePath = "res://addons/ModEditor/GUI/XWBlueprintCreateDialog.tscn";

	private LineEdit _parentName;

	private LineEdit _filePath;

	private Label _messageLabel;

	private Button _parentSearchButton;

	private Button _pathButton;

	private FileDialog _fileDialog;

	private Button _nodeParentCard;

	private Button _node2DParentCard;

	private Button _controlParentCard;

	private Button _resourceParentCard;

	private string _pendingParentClass = "Node";

	private string _pendingPath = "";

	private bool _isPathValid;

	private BlueprintCreatedEventHandler backing_BlueprintCreated;

	public event BlueprintCreatedEventHandler BlueprintCreated
	{
		add
		{
			backing_BlueprintCreated = (BlueprintCreatedEventHandler)Delegate.Combine(backing_BlueprintCreated, value);
		}
		remove
		{
			backing_BlueprintCreated = (BlueprintCreatedEventHandler)Delegate.Remove(backing_BlueprintCreated, value);
		}
	}

	public static XWBlueprintCreateDialog Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/GUI/XWBlueprintCreateDialog.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWBlueprintCreateDialog>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		_parentName = GetNode<LineEdit>("%ParentName");
		_filePath = GetNode<LineEdit>("%FilePath");
		_messageLabel = GetNode<Label>("%MsgLabel");
		_parentSearchButton = GetNode<Button>("%ParentSearchBtn");
		_pathButton = GetNode<Button>("%PathBtn");
		_fileDialog = GetNode<FileDialog>("%PathDialog");
		_nodeParentCard = GetNode<Button>("%NodeParentCard");
		_node2DParentCard = GetNode<Button>("%Node2DParentCard");
		_controlParentCard = GetNode<Button>("%ControlParentCard");
		_resourceParentCard = GetNode<Button>("%ResourceParentCard");
		_fileDialog.Title = "选择蓝图保存路径";
		_fileDialog.FileMode = FileDialog.FileModeEnum.SaveFile;
		_fileDialog.Access = FileDialog.AccessEnum.Filesystem;
		_fileDialog.ClearFilters();
		_fileDialog.AddFilter("*.tres", "蓝图资源");
		Confirmed += OnConfirmed;
		CloseRequested += QueueFree;
		_parentName.TextChanged += (string _) =>
		{
			UpdateDialogState();
		};
		_parentName.TextSubmitted += (string _) =>
		{
			TryCreate();
		};
		_parentSearchButton.Pressed += OnParentSearchPressed;
		_filePath.TextChanged += (string _) =>
		{
			UpdateDialogState();
		};
		_filePath.TextSubmitted += (string _) =>
		{
			TryCreate();
		};
		_pathButton.Pressed += OnBrowsePathPressed;
		_fileDialog.FileSelected += OnPathSelected;
		_nodeParentCard.Pressed += () =>
		{
			SelectParentPreset("Node");
		};
		_node2DParentCard.Pressed += () =>
		{
			SelectParentPreset("Node2D");
		};
		_controlParentCard.Pressed += () =>
		{
			SelectParentPreset("Control");
		};
		_resourceParentCard.Pressed += () =>
		{
			SelectParentPreset("Resource");
		};
		ApplyPendingConfig();
		UpdateDialogState();
	}

	public void Config(string parentClass, string path)
	{
		_pendingParentClass = (string.IsNullOrWhiteSpace(parentClass) ? "Node" : parentClass.Trim());
		_pendingPath = path ?? "";
		if (_parentName != null)
		{
			ApplyPendingConfig();
			UpdateDialogState();
		}
	}

	private void ApplyPendingConfig()
	{
		_parentName.Text = _pendingParentClass;
		_filePath.Text = _pendingPath;
		_parentName.Deselect();
		_filePath.Deselect();
	}

	private void OnParentSearchPressed()
	{
		XWWindowExtendsClassSelector xWWindowExtendsClassSelector = XWWindowExtendsClassSelector.Create();
		if (!GodotObject.IsInstanceValid(xWWindowExtendsClassSelector))
		{
			XWEditorInterface.Instance?.ShowToast("类型选择器未就绪");
			return;
		}
		xWWindowExtendsClassSelector.Title = "选择蓝图父类";
		xWWindowExtendsClassSelector.OkButtonText = "继承";
		xWWindowExtendsClassSelector.ClassSelected += (StringName typeName) =>
		{
			_parentName.Text = typeName.ToString();
			UpdateDialogState();
		};
		AddChild(xWWindowExtendsClassSelector, forceReadableName: false, InternalMode.Disabled);
		xWWindowExtendsClassSelector.PopupCentered();
	}

	private void SelectParentPreset(string parentClass)
	{
		_parentName.Text = parentClass;
		UpdateDialogState();
	}

	private void OnBrowsePathPressed()
	{
		string text = NormalizePath(_filePath.Text);
		if (!string.IsNullOrEmpty(text))
		{
			_fileDialog.CurrentPath = text;
		}
		_fileDialog.PopupCentered();
	}

	private void OnPathSelected(string path)
	{
		_filePath.Text = path;
		UpdateDialogState();
	}

	private void UpdateDialogState()
	{
		string text = Validate(out _isPathValid);
		bool flag = !string.IsNullOrWhiteSpace(_parentName.Text);
		GetOkButton().Disabled = !flag || !_isPathValid;
		_messageLabel.Text = text;
		_messageLabel.Visible = !string.IsNullOrEmpty(text);
		UpdateParentPresetSelection();
	}

	private void UpdateParentPresetSelection()
	{
		if (GodotObject.IsInstanceValid(_nodeParentCard))
		{
			string a = _parentName.Text.Trim();
			_nodeParentCard.ButtonPressed = string.Equals(a, "Node", StringComparison.Ordinal);
			_node2DParentCard.ButtonPressed = string.Equals(a, "Node2D", StringComparison.Ordinal);
			_controlParentCard.ButtonPressed = string.Equals(a, "Control", StringComparison.Ordinal);
			_resourceParentCard.ButtonPressed = string.Equals(a, "Resource", StringComparison.Ordinal);
		}
	}

	private string Validate(out bool validPath)
	{
		validPath = false;
		if (string.IsNullOrWhiteSpace(_parentName?.Text))
		{
			return "请输入继承的父类。";
		}
		string text = NormalizePath(_filePath?.Text ?? "");
		if (string.IsNullOrWhiteSpace(text))
		{
			return "请输入蓝图文件路径。";
		}
		string file = text.GetFile();
		if (string.IsNullOrWhiteSpace(file))
		{
			return "路径缺少文件名。";
		}
		string baseName = file.GetBaseName();
		if (string.IsNullOrWhiteSpace(baseName) || !IsValidFileName(baseName))
		{
			return "文件名无效。";
		}
		validPath = true;
		return "";
	}

	private void OnConfirmed()
	{
		TryCreate();
	}

	private void TryCreate()
	{
		UpdateDialogState();
		if (_isPathValid && !string.IsNullOrWhiteSpace(_parentName.Text))
		{
			string text = NormalizePath(_filePath.Text);
			XWBlueprintCreationService.Result result = XWBlueprintCreationService.Create(text, _parentName.Text.Trim(), text.GetFile().GetBaseName());
			if (!result.Success)
			{
				ShowError(result.Error);
				return;
			}
			EmitSignal(SignalName.BlueprintCreated, result.CreatedPath);
			QueueFree();
		}
	}

	private void ShowError(string message)
	{
		_messageLabel.Text = message;
		_messageLabel.Visible = true;
		XWEditorInterface.Instance?.ShowToast(message);
		PopupCentered();
	}

	private static string NormalizePath(string path)
	{
		path = (path ?? "").Trim().Replace('\\', '/');
		if (string.IsNullOrEmpty(path))
		{
			return "";
		}
		if (!path.EndsWith(".tres", StringComparison.OrdinalIgnoreCase))
		{
			path += ".tres";
		}
		return path;
	}

	private static bool IsValidFileName(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		foreach (char c in value)
		{
			bool flag = c < ' ';
			if (!flag)
			{
				bool flag2;
				switch (c)
				{
				case '"':
				case '*':
				case '/':
				case ':':
				case '<':
				case '>':
				case '?':
				case '\\':
				case '|':
					flag2 = true;
					break;
				default:
					flag2 = false;
					break;
				}
				flag = flag2;
			}
			if (flag)
			{
				return false;
			}
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ConfirmationDialog"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Config, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "parentClass", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPendingConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnParentSearchPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectParentPreset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "parentClass", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnBrowsePathPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPathSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateDialogState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateParentPresetSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowError, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsValidFileName, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBlueprintCreateDialog>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.Config && args.Count == 2)
		{
			Config(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingConfig && args.Count == 0)
		{
			ApplyPendingConfig();
			ret = default;
			return true;
		}
		if (method == MethodName.OnParentSearchPressed && args.Count == 0)
		{
			OnParentSearchPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectParentPreset && args.Count == 1)
		{
			SelectParentPreset(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnBrowsePathPressed && args.Count == 0)
		{
			OnBrowsePathPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPathSelected && args.Count == 1)
		{
			OnPathSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDialogState && args.Count == 0)
		{
			UpdateDialogState();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateParentPresetSelection && args.Count == 0)
		{
			UpdateParentPresetSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.OnConfirmed && args.Count == 0)
		{
			OnConfirmed();
			ret = default;
			return true;
		}
		if (method == MethodName.TryCreate && args.Count == 0)
		{
			TryCreate();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowError && args.Count == 1)
		{
			ShowError(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsValidFileName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidFileName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBlueprintCreateDialog>(Create());
			return true;
		}
		if (method == MethodName.NormalizePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsValidFileName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidFileName(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.Config)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingConfig)
		{
			return true;
		}
		if (method == MethodName.OnParentSearchPressed)
		{
			return true;
		}
		if (method == MethodName.SelectParentPreset)
		{
			return true;
		}
		if (method == MethodName.OnBrowsePathPressed)
		{
			return true;
		}
		if (method == MethodName.OnPathSelected)
		{
			return true;
		}
		if (method == MethodName.UpdateDialogState)
		{
			return true;
		}
		if (method == MethodName.UpdateParentPresetSelection)
		{
			return true;
		}
		if (method == MethodName.OnConfirmed)
		{
			return true;
		}
		if (method == MethodName.TryCreate)
		{
			return true;
		}
		if (method == MethodName.ShowError)
		{
			return true;
		}
		if (method == MethodName.NormalizePath)
		{
			return true;
		}
		if (method == MethodName.IsValidFileName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._parentName)
		{
			_parentName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._filePath)
		{
			_filePath = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._messageLabel)
		{
			_messageLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._parentSearchButton)
		{
			_parentSearchButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._pathButton)
		{
			_pathButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._fileDialog)
		{
			_fileDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._nodeParentCard)
		{
			_nodeParentCard = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._node2DParentCard)
		{
			_node2DParentCard = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._controlParentCard)
		{
			_controlParentCard = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._resourceParentCard)
		{
			_resourceParentCard = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._pendingParentClass)
		{
			_pendingParentClass = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pendingPath)
		{
			_pendingPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._isPathValid)
		{
			_isPathValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._parentName)
		{
			value = VariantUtils.CreateFrom(in _parentName);
			return true;
		}
		if (name == PropertyName._filePath)
		{
			value = VariantUtils.CreateFrom(in _filePath);
			return true;
		}
		if (name == PropertyName._messageLabel)
		{
			value = VariantUtils.CreateFrom(in _messageLabel);
			return true;
		}
		if (name == PropertyName._parentSearchButton)
		{
			value = VariantUtils.CreateFrom(in _parentSearchButton);
			return true;
		}
		if (name == PropertyName._pathButton)
		{
			value = VariantUtils.CreateFrom(in _pathButton);
			return true;
		}
		if (name == PropertyName._fileDialog)
		{
			value = VariantUtils.CreateFrom(in _fileDialog);
			return true;
		}
		if (name == PropertyName._nodeParentCard)
		{
			value = VariantUtils.CreateFrom(in _nodeParentCard);
			return true;
		}
		if (name == PropertyName._node2DParentCard)
		{
			value = VariantUtils.CreateFrom(in _node2DParentCard);
			return true;
		}
		if (name == PropertyName._controlParentCard)
		{
			value = VariantUtils.CreateFrom(in _controlParentCard);
			return true;
		}
		if (name == PropertyName._resourceParentCard)
		{
			value = VariantUtils.CreateFrom(in _resourceParentCard);
			return true;
		}
		if (name == PropertyName._pendingParentClass)
		{
			value = VariantUtils.CreateFrom(in _pendingParentClass);
			return true;
		}
		if (name == PropertyName._pendingPath)
		{
			value = VariantUtils.CreateFrom(in _pendingPath);
			return true;
		}
		if (name == PropertyName._isPathValid)
		{
			value = VariantUtils.CreateFrom(in _isPathValid);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._parentName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._filePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._messageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._parentSearchButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pathButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fileDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nodeParentCard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._node2DParentCard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._controlParentCard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceParentCard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingParentClass, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isPathValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._parentName, Variant.From(in _parentName));
		info.AddProperty(PropertyName._filePath, Variant.From(in _filePath));
		info.AddProperty(PropertyName._messageLabel, Variant.From(in _messageLabel));
		info.AddProperty(PropertyName._parentSearchButton, Variant.From(in _parentSearchButton));
		info.AddProperty(PropertyName._pathButton, Variant.From(in _pathButton));
		info.AddProperty(PropertyName._fileDialog, Variant.From(in _fileDialog));
		info.AddProperty(PropertyName._nodeParentCard, Variant.From(in _nodeParentCard));
		info.AddProperty(PropertyName._node2DParentCard, Variant.From(in _node2DParentCard));
		info.AddProperty(PropertyName._controlParentCard, Variant.From(in _controlParentCard));
		info.AddProperty(PropertyName._resourceParentCard, Variant.From(in _resourceParentCard));
		info.AddProperty(PropertyName._pendingParentClass, Variant.From(in _pendingParentClass));
		info.AddProperty(PropertyName._pendingPath, Variant.From(in _pendingPath));
		info.AddProperty(PropertyName._isPathValid, Variant.From(in _isPathValid));
		info.AddSignalEventDelegate(SignalName.BlueprintCreated, backing_BlueprintCreated);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._parentName, out var value))
		{
			_parentName = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._filePath, out var value2))
		{
			_filePath = value2.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._messageLabel, out var value3))
		{
			_messageLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._parentSearchButton, out var value4))
		{
			_parentSearchButton = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pathButton, out var value5))
		{
			_pathButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._fileDialog, out var value6))
		{
			_fileDialog = value6.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._nodeParentCard, out var value7))
		{
			_nodeParentCard = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._node2DParentCard, out var value8))
		{
			_node2DParentCard = value8.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._controlParentCard, out var value9))
		{
			_controlParentCard = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._resourceParentCard, out var value10))
		{
			_resourceParentCard = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pendingParentClass, out var value11))
		{
			_pendingParentClass = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingPath, out var value12))
		{
			_pendingPath = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName._isPathValid, out var value13))
		{
			_isPathValid = value13.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<BlueprintCreatedEventHandler>(SignalName.BlueprintCreated, out var value14))
		{
			backing_BlueprintCreated = value14;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.BlueprintCreated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "blueprintPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalBlueprintCreated(string blueprintPath)
	{
		EmitSignal(SignalName.BlueprintCreated, new ReadOnlySpan<Variant>((Variant)blueprintPath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.BlueprintCreated && args.Count == 1)
		{
			backing_BlueprintCreated?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.BlueprintCreated)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
