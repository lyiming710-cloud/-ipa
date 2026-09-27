using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.Tools;

namespace PVZHE.ModEditor.Localization.GUI;

[ScriptPath("res://addons/ModEditor/Localization/GUI/XWLocalizationEditorPanel.cs")]
public class XWLocalizationEditorPanel : PanelContainer
{
	public new class MethodName : PanelContainer.MethodName
	{
		public static readonly StringName SetupCsvFile = "SetupCsvFile";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ConfigureTree = "ConfigureTree";

		public static readonly StringName RefreshLocalizationTable = "RefreshLocalizationTable";

		public static readonly StringName SaveLocalizationTable = "SaveLocalizationTable";

		public static readonly StringName AddLocalizationKey = "AddLocalizationKey";

		public static readonly StringName RemoveLocalizationKey = "RemoveLocalizationKey";

		public static readonly StringName PopulateLocalizationTree = "PopulateLocalizationTree";

		public static readonly StringName EditLocalizationEntry = "EditLocalizationEntry";

		public static readonly StringName UpdateSummary = "UpdateSummary";

		public static readonly StringName GetSelectedKey = "GetSelectedKey";

		public static readonly StringName NormalizeLocalizationKey = "NormalizeLocalizationKey";

		public static readonly StringName GetCurrentProjectPath = "GetCurrentProjectPath";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName _refreshButton = "_refreshButton";

		public static readonly StringName _saveButton = "_saveButton";

		public static readonly StringName _addKeyButton = "_addKeyButton";

		public static readonly StringName _removeKeyButton = "_removeKeyButton";

		public static readonly StringName _addKeyLineEdit = "_addKeyLineEdit";

		public static readonly StringName _filterLineEdit = "_filterLineEdit";

		public static readonly StringName _localizationTree = "_localizationTree";

		public static readonly StringName _localizationSummary = "_localizationSummary";

		public static readonly StringName _projectPath = "_projectPath";

		public static readonly StringName _csvPath = "_csvPath";

		public static readonly StringName _dirty = "_dirty";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private Button _refreshButton;

	private Button _saveButton;

	private Button _addKeyButton;

	private Button _removeKeyButton;

	private LineEdit _addKeyLineEdit;

	private LineEdit _filterLineEdit;

	private Tree _localizationTree;

	private Label _localizationSummary;

	private XWLocalizationTable _localizationTable = new XWLocalizationTable();

	private readonly List<string> _visibleKeys = new List<string>();

	private string _projectPath = "";

	private string _csvPath = "";

	private bool _dirty;

	public void SetupCsvFile(string csvPath)
	{
		_csvPath = csvPath ?? "";
		if (IsInsideTree() && _localizationTree != null)
		{
			RefreshLocalizationTable();
		}
	}

	public override void _Ready()
	{
		_refreshButton = GetNode<Button>("%RefreshLocalizationTable");
		_saveButton = GetNode<Button>("%SaveLocalizationTable");
		_addKeyButton = GetNode<Button>("%AddLocalizationKey");
		_removeKeyButton = GetNode<Button>("%RemoveLocalizationKey");
		_addKeyLineEdit = GetNode<LineEdit>("%AddKeyLineEdit");
		_filterLineEdit = GetNode<LineEdit>("%FilterLineEdit");
		_localizationTree = GetNode<Tree>("%LocalizationTree");
		_localizationSummary = GetNode<Label>("%LocalizationSummary");
		ConfigureTree();
		_refreshButton.Pressed += RefreshLocalizationTable;
		_saveButton.Pressed += SaveLocalizationTable;
		_addKeyButton.Pressed += AddLocalizationKey;
		_removeKeyButton.Pressed += RemoveLocalizationKey;
		_filterLineEdit.TextChanged += (string _) =>
		{
			PopulateLocalizationTree();
		};
		_addKeyLineEdit.TextSubmitted += (string _) =>
		{
			AddLocalizationKey();
		};
		_localizationTree.ItemEdited += EditLocalizationEntry;
		RefreshLocalizationTable();
	}

	private void ConfigureTree()
	{
		_localizationTree.Columns = 4;
		_localizationTree.ColumnTitlesVisible = true;
		_localizationTree.HideRoot = true;
		_localizationTree.SetColumnTitle(0, "Key");
		_localizationTree.SetColumnTitle(1, "简体中文 zh_CN");
		_localizationTree.SetColumnTitle(2, "英语 en_US");
		_localizationTree.SetColumnTitle(3, "状态");
		_localizationTree.SetColumnExpand(0, expand: true);
		_localizationTree.SetColumnExpand(1, expand: true);
		_localizationTree.SetColumnExpand(2, expand: true);
		_localizationTree.SetColumnExpand(3, expand: false);
		_localizationTree.SetColumnCustomMinimumWidth(3, 72);
	}

	private void RefreshLocalizationTable()
	{
		_projectPath = GetCurrentProjectPath();
		if (!string.IsNullOrWhiteSpace(_csvPath))
		{
			_localizationTable = XWLocalizationTable.LoadFromCsvFile(_csvPath);
			if (!string.IsNullOrWhiteSpace(_projectPath))
			{
				_localizationTable.EnsureProjectRows(_projectPath);
			}
			_dirty = false;
			PopulateLocalizationTree();
		}
		else if (string.IsNullOrWhiteSpace(_projectPath))
		{
			_localizationTree.Clear();
			_localizationSummary.Text = "请先打开 Mod 工程。";
		}
		else
		{
			_localizationTable = XWLocalizationTable.LoadFromProject(_projectPath);
			_localizationTable.EnsureProjectRows(_projectPath);
			_dirty = false;
			PopulateLocalizationTree();
		}
	}

	private void SaveLocalizationTable()
	{
		_projectPath = GetCurrentProjectPath();
		if (!string.IsNullOrWhiteSpace(_csvPath))
		{
			_localizationTable.SaveToCsvFile(_csvPath, XWLocalizationTable.DefaultLocales);
			if (!string.IsNullOrWhiteSpace(_projectPath))
			{
				XWModManifestSyncService.RegisterPath(_projectPath, _csvPath);
			}
			int value = (string.IsNullOrWhiteSpace(_projectPath) ? null : XWModManifest.Load(Path.Combine(_projectPath, "mod.json"))).Translations?.Count ?? 0;
			_dirty = false;
			PopulateLocalizationTree();
			List<string> missingTranslations = _localizationTable.GetMissingTranslations(XWLocalizationTable.DefaultLocales);
			_localizationSummary.Text = $"已保存到 Localization/*.csv    Key: {_localizationTable.Entries.Count}    缺失: {missingTranslations.Count}    文件: {value}";
		}
		else if (string.IsNullOrWhiteSpace(_projectPath))
		{
			_localizationSummary.Text = "请先打开 Mod 工程。";
		}
		else
		{
			_localizationTable.SaveToProject(_projectPath, XWLocalizationTable.DefaultLocales);
			int value2 = XWModManifest.Load(Path.Combine(_projectPath, "mod.json")).Translations?.Count ?? 0;
			_dirty = false;
			PopulateLocalizationTree();
			List<string> missingTranslations2 = _localizationTable.GetMissingTranslations(XWLocalizationTable.DefaultLocales);
			_localizationSummary.Text = $"已保存到 Localization/*.csv    Key: {_localizationTable.Entries.Count}    缺失: {missingTranslations2.Count}    文件: {value2}";
		}
	}

	private void AddLocalizationKey()
	{
		string text = NormalizeLocalizationKey(_addKeyLineEdit.Text);
		if (string.IsNullOrWhiteSpace(text))
		{
			_localizationSummary.Text = "请输入有效的多语言 key。";
			return;
		}
		string[] defaultLocales = XWLocalizationTable.DefaultLocales;
		foreach (string text2 in defaultLocales)
		{
			if (!_localizationTable.Entries.TryGetValue(text, out var value) || !value.Values.ContainsKey(text2))
			{
				_localizationTable.Set(text, text2, "");
			}
		}
		_addKeyLineEdit.Text = "";
		_dirty = true;
		PopulateLocalizationTree(text);
	}

	private void RemoveLocalizationKey()
	{
		string selectedKey = GetSelectedKey();
		if (!string.IsNullOrWhiteSpace(selectedKey) && _localizationTable.Remove(selectedKey))
		{
			_dirty = true;
			PopulateLocalizationTree();
		}
	}

	private void PopulateLocalizationTree(string selectKey = "")
	{
		_visibleKeys.Clear();
		_localizationTree.Clear();
		TreeItem parent = _localizationTree.CreateItem();
		List<string> list = new List<string>(_localizationTable.Entries.Keys);
		list.Sort(StringComparer.OrdinalIgnoreCase);
		foreach (string item in list)
		{
			if (_localizationTable.Entries.TryGetValue(item, out var value) && MatchesFilter(value))
			{
				_visibleKeys.Add(item);
				TreeItem treeItem = _localizationTree.CreateItem(parent);
				treeItem.SetText(0, item);
				treeItem.SetText(1, value.Values.TryGetValue("zh_CN", out var value2) ? value2 : "");
				treeItem.SetText(2, value.Values.TryGetValue("en_US", out var value3) ? value3 : "");
				treeItem.SetText(3, IsLocalizationEntryMissing(value) ? "缺失" : "完整");
				treeItem.SetEditable(1, enabled: true);
				treeItem.SetEditable(2, enabled: true);
				treeItem.SetMetadata(0, item);
				treeItem.SetMetadata(1, item);
				treeItem.SetMetadata(2, item);
				if (IsLocalizationEntryMissing(value))
				{
					treeItem.SetCustomColor(3, new Color(1f, 0.62f, 0.25f));
				}
				if (!string.IsNullOrEmpty(selectKey) && item == selectKey)
				{
					treeItem.Select(0);
				}
			}
		}
		UpdateSummary();
	}

	private void EditLocalizationEntry()
	{
		TreeItem selected = _localizationTree.GetSelected();
		if (selected == null)
		{
			return;
		}
		int selectedColumn = _localizationTree.GetSelectedColumn();
		string text = selectedColumn switch
		{
			1 => "zh_CN", 
			2 => "en_US", 
			_ => "", 
		};
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		string text2 = selected.GetMetadata(0).AsString();
		if (string.IsNullOrWhiteSpace(text2))
		{
			return;
		}
		_localizationTable.Set(text2, text, selected.GetText(selectedColumn));
		_dirty = true;
		if (_localizationTable.Entries.TryGetValue(text2, out var value))
		{
			bool flag = IsLocalizationEntryMissing(value);
			selected.SetText(3, flag ? "缺失" : "完整");
			if (flag)
			{
				selected.SetCustomColor(3, new Color(1f, 0.62f, 0.25f));
			}
			else
			{
				selected.ClearCustomColor(3);
			}
		}
		UpdateSummary();
	}

	private void UpdateSummary()
	{
		List<string> missingTranslations = _localizationTable.GetMissingTranslations(XWLocalizationTable.DefaultLocales);
		string value = (_dirty ? "    未保存" : "");
		_localizationSummary.Text = $"Localization/*.csv    Key: {_localizationTable.Entries.Count}    显示: {_visibleKeys.Count}    缺失: {missingTranslations.Count}{value}";
	}

	private bool MatchesFilter(XWLocalizationTable.Entry entry)
	{
		string value = _filterLineEdit?.Text?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(value))
		{
			return true;
		}
		if (entry.Key.Contains(value, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		foreach (string value2 in entry.Values.Values)
		{
			if (!string.IsNullOrEmpty(value2) && value2.Contains(value, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	private string GetSelectedKey()
	{
		TreeItem selected = _localizationTree.GetSelected();
		if (selected == null)
		{
			return "";
		}
		return selected.GetMetadata(0).AsString();
	}

	private static bool IsLocalizationEntryMissing(XWLocalizationTable.Entry entry)
	{
		string[] defaultLocales = XWLocalizationTable.DefaultLocales;
		foreach (string key in defaultLocales)
		{
			if (!entry.Values.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
			{
				return true;
			}
		}
		return false;
	}

	private static string NormalizeLocalizationKey(string rawKey)
	{
		if (string.IsNullOrWhiteSpace(rawKey))
		{
			return "";
		}
		List<char> list = new List<char>(rawKey.Trim().Length);
		string text = rawKey.Trim();
		foreach (char c in text)
		{
			if (char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.')
			{
				list.Add(c);
			}
			else if (char.IsWhiteSpace(c) || c == '/' || c == '\\')
			{
				list.Add('.');
			}
		}
		return new string(list.ToArray()).Trim('.');
	}

	private static string GetCurrentProjectPath()
	{
		if (XWEditorInterface.Instance?.GetEditorPanel() is ModEditorPanel modEditorPanel)
		{
			return modEditorPanel.GetCurrentProject()?.ProjectPath ?? "";
		}
		return "";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.SetupCsvFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "csvPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshLocalizationTable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveLocalizationTable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddLocalizationKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveLocalizationKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateLocalizationTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "selectKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EditLocalizationEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSummary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeLocalizationKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "rawKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentProjectPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetupCsvFile && args.Count == 1)
		{
			SetupCsvFile(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureTree && args.Count == 0)
		{
			ConfigureTree();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshLocalizationTable && args.Count == 0)
		{
			RefreshLocalizationTable();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveLocalizationTable && args.Count == 0)
		{
			SaveLocalizationTable();
			ret = default;
			return true;
		}
		if (method == MethodName.AddLocalizationKey && args.Count == 0)
		{
			AddLocalizationKey();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveLocalizationKey && args.Count == 0)
		{
			RemoveLocalizationKey();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateLocalizationTree && args.Count == 1)
		{
			PopulateLocalizationTree(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EditLocalizationEntry && args.Count == 0)
		{
			EditLocalizationEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSummary && args.Count == 0)
		{
			UpdateSummary();
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetSelectedKey());
			return true;
		}
		if (method == MethodName.NormalizeLocalizationKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeLocalizationKey(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurrentProjectPath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentProjectPath());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.NormalizeLocalizationKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeLocalizationKey(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurrentProjectPath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentProjectPath());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SetupCsvFile)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.ConfigureTree)
		{
			return true;
		}
		if (method == MethodName.RefreshLocalizationTable)
		{
			return true;
		}
		if (method == MethodName.SaveLocalizationTable)
		{
			return true;
		}
		if (method == MethodName.AddLocalizationKey)
		{
			return true;
		}
		if (method == MethodName.RemoveLocalizationKey)
		{
			return true;
		}
		if (method == MethodName.PopulateLocalizationTree)
		{
			return true;
		}
		if (method == MethodName.EditLocalizationEntry)
		{
			return true;
		}
		if (method == MethodName.UpdateSummary)
		{
			return true;
		}
		if (method == MethodName.GetSelectedKey)
		{
			return true;
		}
		if (method == MethodName.NormalizeLocalizationKey)
		{
			return true;
		}
		if (method == MethodName.GetCurrentProjectPath)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._refreshButton)
		{
			_refreshButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._saveButton)
		{
			_saveButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._addKeyButton)
		{
			_addKeyButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._removeKeyButton)
		{
			_removeKeyButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._addKeyLineEdit)
		{
			_addKeyLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._filterLineEdit)
		{
			_filterLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._localizationTree)
		{
			_localizationTree = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._localizationSummary)
		{
			_localizationSummary = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._projectPath)
		{
			_projectPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._csvPath)
		{
			_csvPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._dirty)
		{
			_dirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._refreshButton)
		{
			value = VariantUtils.CreateFrom(in _refreshButton);
			return true;
		}
		if (name == PropertyName._saveButton)
		{
			value = VariantUtils.CreateFrom(in _saveButton);
			return true;
		}
		if (name == PropertyName._addKeyButton)
		{
			value = VariantUtils.CreateFrom(in _addKeyButton);
			return true;
		}
		if (name == PropertyName._removeKeyButton)
		{
			value = VariantUtils.CreateFrom(in _removeKeyButton);
			return true;
		}
		if (name == PropertyName._addKeyLineEdit)
		{
			value = VariantUtils.CreateFrom(in _addKeyLineEdit);
			return true;
		}
		if (name == PropertyName._filterLineEdit)
		{
			value = VariantUtils.CreateFrom(in _filterLineEdit);
			return true;
		}
		if (name == PropertyName._localizationTree)
		{
			value = VariantUtils.CreateFrom(in _localizationTree);
			return true;
		}
		if (name == PropertyName._localizationSummary)
		{
			value = VariantUtils.CreateFrom(in _localizationSummary);
			return true;
		}
		if (name == PropertyName._projectPath)
		{
			value = VariantUtils.CreateFrom(in _projectPath);
			return true;
		}
		if (name == PropertyName._csvPath)
		{
			value = VariantUtils.CreateFrom(in _csvPath);
			return true;
		}
		if (name == PropertyName._dirty)
		{
			value = VariantUtils.CreateFrom(in _dirty);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._refreshButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addKeyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._removeKeyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addKeyLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._filterLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localizationTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localizationSummary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._projectPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._csvPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._refreshButton, Variant.From(in _refreshButton));
		info.AddProperty(PropertyName._saveButton, Variant.From(in _saveButton));
		info.AddProperty(PropertyName._addKeyButton, Variant.From(in _addKeyButton));
		info.AddProperty(PropertyName._removeKeyButton, Variant.From(in _removeKeyButton));
		info.AddProperty(PropertyName._addKeyLineEdit, Variant.From(in _addKeyLineEdit));
		info.AddProperty(PropertyName._filterLineEdit, Variant.From(in _filterLineEdit));
		info.AddProperty(PropertyName._localizationTree, Variant.From(in _localizationTree));
		info.AddProperty(PropertyName._localizationSummary, Variant.From(in _localizationSummary));
		info.AddProperty(PropertyName._projectPath, Variant.From(in _projectPath));
		info.AddProperty(PropertyName._csvPath, Variant.From(in _csvPath));
		info.AddProperty(PropertyName._dirty, Variant.From(in _dirty));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._refreshButton, out var value))
		{
			_refreshButton = value.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._saveButton, out var value2))
		{
			_saveButton = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._addKeyButton, out var value3))
		{
			_addKeyButton = value3.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._removeKeyButton, out var value4))
		{
			_removeKeyButton = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._addKeyLineEdit, out var value5))
		{
			_addKeyLineEdit = value5.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._filterLineEdit, out var value6))
		{
			_filterLineEdit = value6.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localizationTree, out var value7))
		{
			_localizationTree = value7.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._localizationSummary, out var value8))
		{
			_localizationSummary = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._projectPath, out var value9))
		{
			_projectPath = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName._csvPath, out var value10))
		{
			_csvPath = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName._dirty, out var value11))
		{
			_dirty = value11.As<bool>();
		}
	}
}
