using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Path/XWInspectorPropertyEditorPath.cs")]
public class XWInspectorPropertyEditorPath : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName SetupFileDialog = "SetupFileDialog";

		public static readonly StringName OnTextChanged = "OnTextChanged";

		public static readonly StringName OnTextSubmitted = "OnTextSubmitted";

		public static readonly StringName OnFocusExited = "OnFocusExited";

		public static readonly StringName OnBrowsePressed = "OnBrowsePressed";

		public static readonly StringName OnFileSelected = "OnFileSelected";

		public static readonly StringName OnDirSelected = "OnDirSelected";

		public static readonly StringName CommitPath = "CommitPath";

		public static readonly StringName UpdateBrowseButtonIcon = "UpdateBrowseButtonIcon";

		public static readonly StringName IsFolderHint = "IsFolderHint";

		public static readonly StringName ParseFilters = "ParseFilters";

		public static readonly StringName GetDragData = "GetDragData";

		public static readonly StringName CanDropDataForwarded = "CanDropDataForwarded";

		public static readonly StringName DropDataForwarded = "DropDataForwarded";

		public static readonly StringName ExtractDropPath = "ExtractDropPath";

		public static readonly StringName ExtractFirstPath = "ExtractFirstPath";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _lineEdit = "_lineEdit";

		public static readonly StringName _browseButton = "_browseButton";

		public static readonly StringName _fileDialog = "_fileDialog";

		public static readonly StringName _hint = "_hint";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Path/XWInspectorPropertyEditorPath.tscn";

	private static readonly Texture2D IconFileBrowse = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ClassIcon/FileBrowse.svg", null, ResourceLoader.CacheMode.Reuse));

	private static readonly Texture2D IconFolderBrowse = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ClassIcon/FolderBrowse.svg", null, ResourceLoader.CacheMode.Reuse));

	private LineEdit _lineEdit;

	private Button _browseButton;

	private FileDialog _fileDialog;

	private PropertyHint _hint = PropertyHint.File;

	public static XWInspectorPropertyEditorPath Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Path/XWInspectorPropertyEditorPath.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorPath>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_lineEdit = GetNode<LineEdit>("%LineEdit");
		_browseButton = GetNode<Button>("%BrowseButton");
		_fileDialog = GetNode<FileDialog>("%FileDialog");
		_lineEdit.TextChanged += OnTextChanged;
		_lineEdit.TextSubmitted += OnTextSubmitted;
		_lineEdit.FocusExited += OnFocusExited;
		_lineEdit.SetDragForwarding(Callable.From<Vector2, Variant>(GetDragData), Callable.From<Vector2, Variant, bool>(CanDropDataForwarded), Callable.From<Vector2, Variant>(DropDataForwarded));
		_browseButton.Pressed += OnBrowsePressed;
		_fileDialog.FileSelected += OnFileSelected;
		_fileDialog.DirSelected += OnDirSelected;
		UpdateBrowseButtonIcon();
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		_hint = property.Hint;
		SetupFileDialog();
		base.SetEditProperty(property, field);
	}

	public override void UpdateValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Nil)
		{
			XWInspectorPropertyEditorBase.SetLineEditTextPreservingCaret(_lineEdit, propertyValue.AsString());
		}
	}

	public override Variant GetValue()
	{
		return _lineEdit.Text;
	}

	private void SetupFileDialog()
	{
		PropertyHint hint = _hint;
		if (hint <= PropertyHint.SaveFile)
		{
			PropertyHint num = hint - 13;
			if ((ulong)num <= 3uL)
			{
				switch ((int)num)
				{
				case 1:
					goto IL_0052;
				case 3:
					goto IL_0071;
				case 0:
					goto IL_0090;
				case 2:
					goto IL_00ac;
				}
			}
			if (hint != PropertyHint.SaveFile)
			{
				goto IL_0100;
			}
			_fileDialog.FileMode = FileDialog.FileModeEnum.SaveFile;
			_fileDialog.Access = FileDialog.AccessEnum.Resources;
		}
		else
		{
			if (hint != PropertyHint.GlobalSaveFile)
			{
				if (hint == PropertyHint.FilePath)
				{
					goto IL_0090;
				}
				goto IL_0100;
			}
			_fileDialog.FileMode = FileDialog.FileModeEnum.SaveFile;
			_fileDialog.Access = FileDialog.AccessEnum.Filesystem;
		}
		goto IL_011a;
		IL_00ac:
		_fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		_fileDialog.Access = FileDialog.AccessEnum.Filesystem;
		goto IL_011a;
		IL_011a:
		_fileDialog.Filters = System.Array.Empty<string>();
		if (GodotObject.IsInstanceValid(Property) && Property.HintString != "")
		{
			string[] array = ParseFilters(Property.HintString);
			if (array.Length != 0)
			{
				_fileDialog.Filters = array;
			}
		}
		UpdateBrowseButtonIcon();
		return;
		IL_0100:
		_fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		_fileDialog.Access = FileDialog.AccessEnum.Resources;
		goto IL_011a;
		IL_0052:
		_fileDialog.FileMode = FileDialog.FileModeEnum.OpenDir;
		_fileDialog.Access = FileDialog.AccessEnum.Resources;
		goto IL_011a;
		IL_0071:
		_fileDialog.FileMode = FileDialog.FileModeEnum.OpenDir;
		_fileDialog.Access = FileDialog.AccessEnum.Filesystem;
		goto IL_011a;
		IL_0090:
		_fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		_fileDialog.Access = FileDialog.AccessEnum.Resources;
		goto IL_011a;
	}

	private void OnTextChanged(string newText)
	{
		ValueChange(newText);
	}

	private void OnTextSubmitted(string newText)
	{
		CommitPath(newText);
	}

	private void OnFocusExited()
	{
		CommitPath(_lineEdit.Text);
	}

	private void OnBrowsePressed()
	{
		SetupFileDialog();
		if (_lineEdit.Text != "")
		{
			if (_hint == PropertyHint.Dir || _hint == PropertyHint.GlobalDir)
			{
				_fileDialog.CurrentDir = _lineEdit.Text;
			}
			else
			{
				_fileDialog.CurrentPath = _lineEdit.Text;
			}
		}
		_fileDialog.PopupCenteredClamped(new Vector2I(800, 600), 0.9f);
	}

	private void OnFileSelected(string path)
	{
		CommitPath(path);
	}

	private void OnDirSelected(string path)
	{
		CommitPath(path);
	}

	private void CommitPath(string path)
	{
		_lineEdit.Text = path;
		ValueChange(path);
	}

	private void UpdateBrowseButtonIcon()
	{
		if (GodotObject.IsInstanceValid(_browseButton))
		{
			bool flag = IsFolderHint(_hint);
			_browseButton.Icon = (flag ? IconFolderBrowse : IconFileBrowse);
			_browseButton.TooltipText = (flag ? "选择文件夹" : "选择文件");
			_browseButton.CustomMinimumSize = new Vector2(30f, 28f);
			_browseButton.Flat = true;
		}
	}

	private static bool IsFolderHint(PropertyHint hint)
	{
		if (hint != PropertyHint.Dir)
		{
			return hint == PropertyHint.GlobalDir;
		}
		return true;
	}

	private static string[] ParseFilters(string hintString)
	{
		if (string.IsNullOrWhiteSpace(hintString))
		{
			return System.Array.Empty<string>();
		}
		string[] array = hintString.Split(",");
		List<string> list = new List<string>();
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i].Trim();
			if (!string.IsNullOrWhiteSpace(text))
			{
				list.Add(text);
			}
		}
		return list.ToArray();
	}

	private Variant GetDragData(Vector2 atPosition)
	{
		return default;
	}

	private bool CanDropDataForwarded(Vector2 atPosition, Variant data)
	{
		string text = ExtractDropPath(data);
		if (string.IsNullOrWhiteSpace(text))
		{
			return false;
		}
		if (IsFolderHint(_hint))
		{
			if (!DirAccess.DirExistsAbsolute(text))
			{
				return text.EndsWith("/");
			}
			return true;
		}
		if (!FileAccess.FileExists(text) && !ResourceLoader.Exists(text))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(Property) || string.IsNullOrWhiteSpace(Property.HintString))
		{
			return true;
		}
		string extension = text.GetExtension();
		string[] array = ParseFilters(Property.HintString);
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i].Split(';')[0].Trim();
			if (text2 == "*.*" || text2 == "*")
			{
				return true;
			}
			if (text2.StartsWith("*.") && extension.Equals(text2.Substring(2), StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	private void DropDataForwarded(Vector2 atPosition, Variant data)
	{
		string text = ExtractDropPath(data);
		if (!string.IsNullOrWhiteSpace(text))
		{
			CommitPath(text);
		}
	}

	private static string ExtractDropPath(Variant data)
	{
		if (data.VariantType != Variant.Type.Dictionary)
		{
			return "";
		}
		Dictionary dictionary = data.As<Dictionary>();
		if (dictionary.ContainsKey("files"))
		{
			return ExtractFirstPath(dictionary["files"]);
		}
		if (dictionary.ContainsKey("paths"))
		{
			return ExtractFirstPath(dictionary["paths"]);
		}
		if (dictionary.ContainsKey("path"))
		{
			return dictionary["path"].AsString();
		}
		return "";
	}

	private static string ExtractFirstPath(Variant value)
	{
		if (value.VariantType == Variant.Type.PackedStringArray)
		{
			string[] array = value.AsStringArray();
			if (array.Length == 0)
			{
				return "";
			}
			return array[0];
		}
		if (value.VariantType == Variant.Type.Array)
		{
			Godot.Collections.Array array2 = value.As<Godot.Collections.Array>();
			if (array2.Count <= 0)
			{
				return "";
			}
			return array2[0].AsString();
		}
		return value.AsString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupFileDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTextChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTextSubmitted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnFocusExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnBrowsePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDirSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitPath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateBrowseButtonIcon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsFolderHint, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ParseFilters, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.ExtractDropPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ExtractFirstPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorPath>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.SetEditProperty && args.Count == 2)
		{
			SetEditProperty(VariantUtils.ConvertTo<XWInspectorProperty>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateValue && args.Count == 0)
		{
			UpdateValue();
			ret = default;
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetValue());
			return true;
		}
		if (method == MethodName.SetupFileDialog && args.Count == 0)
		{
			SetupFileDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTextChanged && args.Count == 1)
		{
			OnTextChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTextSubmitted && args.Count == 1)
		{
			OnTextSubmitted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnFocusExited && args.Count == 0)
		{
			OnFocusExited();
			ret = default;
			return true;
		}
		if (method == MethodName.OnBrowsePressed && args.Count == 0)
		{
			OnBrowsePressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnFileSelected && args.Count == 1)
		{
			OnFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDirSelected && args.Count == 1)
		{
			OnDirSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitPath && args.Count == 1)
		{
			CommitPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateBrowseButtonIcon && args.Count == 0)
		{
			UpdateBrowseButtonIcon();
			ret = default;
			return true;
		}
		if (method == MethodName.IsFolderHint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFolderHint(VariantUtils.ConvertTo<PropertyHint>(in args[0])));
			return true;
		}
		if (method == MethodName.ParseFilters && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(ParseFilters(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.ExtractDropPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ExtractDropPath(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.ExtractFirstPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ExtractFirstPath(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorPath>(Create());
			return true;
		}
		if (method == MethodName.IsFolderHint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFolderHint(VariantUtils.ConvertTo<PropertyHint>(in args[0])));
			return true;
		}
		if (method == MethodName.ParseFilters && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(ParseFilters(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ExtractDropPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ExtractDropPath(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.ExtractFirstPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ExtractFirstPath(VariantUtils.ConvertTo<Variant>(in args[0])));
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
		if (method == MethodName.SetEditProperty)
		{
			return true;
		}
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.SetupFileDialog)
		{
			return true;
		}
		if (method == MethodName.OnTextChanged)
		{
			return true;
		}
		if (method == MethodName.OnTextSubmitted)
		{
			return true;
		}
		if (method == MethodName.OnFocusExited)
		{
			return true;
		}
		if (method == MethodName.OnBrowsePressed)
		{
			return true;
		}
		if (method == MethodName.OnFileSelected)
		{
			return true;
		}
		if (method == MethodName.OnDirSelected)
		{
			return true;
		}
		if (method == MethodName.CommitPath)
		{
			return true;
		}
		if (method == MethodName.UpdateBrowseButtonIcon)
		{
			return true;
		}
		if (method == MethodName.IsFolderHint)
		{
			return true;
		}
		if (method == MethodName.ParseFilters)
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
		if (method == MethodName.ExtractDropPath)
		{
			return true;
		}
		if (method == MethodName.ExtractFirstPath)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._lineEdit)
		{
			_lineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._browseButton)
		{
			_browseButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._fileDialog)
		{
			_fileDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._hint)
		{
			_hint = VariantUtils.ConvertTo<PropertyHint>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._lineEdit)
		{
			value = VariantUtils.CreateFrom(in _lineEdit);
			return true;
		}
		if (name == PropertyName._browseButton)
		{
			value = VariantUtils.CreateFrom(in _browseButton);
			return true;
		}
		if (name == PropertyName._fileDialog)
		{
			value = VariantUtils.CreateFrom(in _fileDialog);
			return true;
		}
		if (name == PropertyName._hint)
		{
			value = VariantUtils.CreateFrom(in _hint);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._lineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._browseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fileDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._hint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._lineEdit, Variant.From(in _lineEdit));
		info.AddProperty(PropertyName._browseButton, Variant.From(in _browseButton));
		info.AddProperty(PropertyName._fileDialog, Variant.From(in _fileDialog));
		info.AddProperty(PropertyName._hint, Variant.From(in _hint));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._lineEdit, out var value))
		{
			_lineEdit = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._browseButton, out var value2))
		{
			_browseButton = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._fileDialog, out var value3))
		{
			_fileDialog = value3.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._hint, out var value4))
		{
			_hint = value4.As<PropertyHint>();
		}
	}
}
