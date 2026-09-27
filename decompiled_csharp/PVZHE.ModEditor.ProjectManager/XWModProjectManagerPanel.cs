using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

namespace PVZHE.ModEditor.ProjectManager;

[ScriptPath("res://addons/ModEditor/ProjectManager/GUI/XWModProjectManagerPanel.cs")]
public class XWModProjectManagerPanel : PanelContainer
{
	[Signal]
	public delegate void ProjectOpenRequestedEventHandler(string projectFilePath);

	[Signal]
	public delegate void NewProjectRequestedEventHandler();

	[Signal]
	public delegate void OpenProjectRequestedEventHandler();

	private sealed class ProjectEntry
	{
		public string ProjectFilePath = "";

		public string Name = "";

		public string Version = "";

		public string Author = "";

		public string Description = "";

		public string ProjectDirectory = "";

		public bool Exists;
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ShowStartScreen = "ShowStartScreen";

		public static readonly StringName AddRecentProject = "AddRecentProject";

		public static readonly StringName SetupProjectList = "SetupProjectList";

		public static readonly StringName ConnectSignals = "ConnectSignals";

		public static readonly StringName LoadRecentProjects = "LoadRecentProjects";

		public static readonly StringName SaveRecentProjects = "SaveRecentProjects";

		public static readonly StringName RefreshProjectList = "RefreshProjectList";

		public static readonly StringName OnProjectItemSelected = "OnProjectItemSelected";

		public static readonly StringName SelectFirstProject = "SelectFirstProject";

		public static readonly StringName SelectProject = "SelectProject";

		public static readonly StringName OpenSelectedProject = "OpenSelectedProject";

		public static readonly StringName RemoveMissingProjectRecords = "RemoveMissingProjectRecords";

		public static readonly StringName NormalizeProjectFilePath = "NormalizeProjectFilePath";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName _projectList = "_projectList";

		public static readonly StringName _projectNameLabel = "_projectNameLabel";

		public static readonly StringName _projectPathLabel = "_projectPathLabel";

		public static readonly StringName _projectMetaLabel = "_projectMetaLabel";

		public static readonly StringName _projectDescriptionLabel = "_projectDescriptionLabel";

		public static readonly StringName _emptyHintLabel = "_emptyHintLabel";

		public static readonly StringName _newProjectButton = "_newProjectButton";

		public static readonly StringName _openProjectButton = "_openProjectButton";

		public static readonly StringName _refreshProjectsButton = "_refreshProjectsButton";

		public static readonly StringName _openSelectedProjectButton = "_openSelectedProjectButton";

		public static readonly StringName _removeMissingButton = "_removeMissingButton";

		public static readonly StringName _selectedProjectFilePath = "_selectedProjectFilePath";
	}

	public new class SignalName : PanelContainer.SignalName
	{
		public static readonly StringName ProjectOpenRequested = "ProjectOpenRequested";

		public static readonly StringName NewProjectRequested = "NewProjectRequested";

		public static readonly StringName OpenProjectRequested = "OpenProjectRequested";
	}

	private const string RecentProjectsConfigPath = "user://mod_editor_recent_projects.cfg";

	private const int MaxRecentProjects = 24;

	private Tree _projectList;

	private Label _projectNameLabel;

	private Label _projectPathLabel;

	private Label _projectMetaLabel;

	private RichTextLabel _projectDescriptionLabel;

	private Label _emptyHintLabel;

	private Button _newProjectButton;

	private Button _openProjectButton;

	private Button _refreshProjectsButton;

	private Button _openSelectedProjectButton;

	private Button _removeMissingButton;

	private readonly List<string> _recentProjectFiles = new List<string>();

	private readonly List<ProjectEntry> _projectEntries = new List<ProjectEntry>();

	private string _selectedProjectFilePath = "";

	private ProjectOpenRequestedEventHandler backing_ProjectOpenRequested;

	private NewProjectRequestedEventHandler backing_NewProjectRequested;

	private OpenProjectRequestedEventHandler backing_OpenProjectRequested;

	public event ProjectOpenRequestedEventHandler ProjectOpenRequested
	{
		add
		{
			backing_ProjectOpenRequested = (ProjectOpenRequestedEventHandler)Delegate.Combine(backing_ProjectOpenRequested, value);
		}
		remove
		{
			backing_ProjectOpenRequested = (ProjectOpenRequestedEventHandler)Delegate.Remove(backing_ProjectOpenRequested, value);
		}
	}

	public event NewProjectRequestedEventHandler NewProjectRequested
	{
		add
		{
			backing_NewProjectRequested = (NewProjectRequestedEventHandler)Delegate.Combine(backing_NewProjectRequested, value);
		}
		remove
		{
			backing_NewProjectRequested = (NewProjectRequestedEventHandler)Delegate.Remove(backing_NewProjectRequested, value);
		}
	}

	public event OpenProjectRequestedEventHandler OpenProjectRequested
	{
		add
		{
			backing_OpenProjectRequested = (OpenProjectRequestedEventHandler)Delegate.Combine(backing_OpenProjectRequested, value);
		}
		remove
		{
			backing_OpenProjectRequested = (OpenProjectRequestedEventHandler)Delegate.Remove(backing_OpenProjectRequested, value);
		}
	}

	public override void _Ready()
	{
		_projectList = GetNode<Tree>("%ProjectList");
		_projectNameLabel = GetNode<Label>("%ProjectNameLabel");
		_projectPathLabel = GetNode<Label>("%ProjectPathLabel");
		_projectMetaLabel = GetNode<Label>("%ProjectMetaLabel");
		_projectDescriptionLabel = GetNode<RichTextLabel>("%ProjectDescriptionLabel");
		_emptyHintLabel = GetNode<Label>("%EmptyHintLabel");
		_newProjectButton = GetNode<Button>("%NewProjectButton");
		_openProjectButton = GetNode<Button>("%OpenProjectButton");
		_refreshProjectsButton = GetNode<Button>("%RefreshProjectsButton");
		_openSelectedProjectButton = GetNode<Button>("%OpenSelectedProjectButton");
		_removeMissingButton = GetNode<Button>("%RemoveMissingButton");
		SetupProjectList();
		ConnectSignals();
		LoadRecentProjects();
		RefreshProjectList();
	}

	public void ShowStartScreen()
	{
		Visible = true;
		LoadRecentProjects();
		RefreshProjectList();
	}

	public void AddRecentProject(ModProject project)
	{
		if (project != null)
		{
			AddRecentProject(project.ProjectFilePath);
		}
	}

	public void AddRecentProject(string projectFilePath)
	{
		string normalized = NormalizeProjectFilePath(projectFilePath);
		if (!string.IsNullOrWhiteSpace(normalized))
		{
			_recentProjectFiles.RemoveAll((string path) => string.Equals(path, normalized, StringComparison.OrdinalIgnoreCase));
			_recentProjectFiles.Insert(0, normalized);
			if (_recentProjectFiles.Count > 24)
			{
				_recentProjectFiles.RemoveRange(24, _recentProjectFiles.Count - 24);
			}
			SaveRecentProjects();
			RefreshProjectList();
			SelectProject(normalized);
		}
	}

	private void SetupProjectList()
	{
		_projectList.Columns = 3;
		_projectList.ColumnTitlesVisible = true;
		_projectList.HideRoot = true;
		_projectList.SelectMode = Tree.SelectModeEnum.Row;
		_projectList.SetColumnTitle(0, "工程");
		_projectList.SetColumnTitle(1, "版本");
		_projectList.SetColumnTitle(2, "路径");
		_projectList.SetColumnExpand(0, expand: true);
		_projectList.SetColumnExpand(1, expand: false);
		_projectList.SetColumnCustomMinimumWidth(1, 92);
		_projectList.SetColumnExpand(2, expand: true);
	}

	private void ConnectSignals()
	{
		_projectList.ItemSelected += OnProjectItemSelected;
		_projectList.ItemActivated += OpenSelectedProject;
		_newProjectButton.Pressed += () =>
		{
			EmitSignal(SignalName.NewProjectRequested);
		};
		_openProjectButton.Pressed += () =>
		{
			EmitSignal(SignalName.OpenProjectRequested);
		};
		_refreshProjectsButton.Pressed += RefreshProjectList;
		_openSelectedProjectButton.Pressed += OpenSelectedProject;
		_removeMissingButton.Pressed += RemoveMissingProjectRecords;
	}

	private void LoadRecentProjects()
	{
		_recentProjectFiles.Clear();
		ConfigFile configFile = new ConfigFile();
		if (configFile.Load("user://mod_editor_recent_projects.cfg") != Error.Ok)
		{
			return;
		}
		int num = (int)configFile.GetValue("projects", "count", 0);
		for (int i = 0; i < num; i++)
		{
			string path = NormalizeProjectFilePath(configFile.GetValue("projects", $"path_{i}", "").AsString());
			if (!string.IsNullOrWhiteSpace(path) && !_recentProjectFiles.Exists((string existing) => string.Equals(existing, path, StringComparison.OrdinalIgnoreCase)))
			{
				_recentProjectFiles.Add(path);
			}
		}
	}

	private void SaveRecentProjects()
	{
		ConfigFile configFile = new ConfigFile();
		configFile.SetValue("projects", "count", _recentProjectFiles.Count);
		for (int i = 0; i < _recentProjectFiles.Count; i++)
		{
			configFile.SetValue("projects", $"path_{i}", _recentProjectFiles[i]);
		}
		configFile.Save("user://mod_editor_recent_projects.cfg");
	}

	private void RefreshProjectList()
	{
		_projectEntries.Clear();
		foreach (string recentProjectFile in _recentProjectFiles)
		{
			_projectEntries.Add(ReadProjectEntry(recentProjectFile));
		}
		_projectList.Clear();
		TreeItem parent = _projectList.CreateItem();
		foreach (ProjectEntry projectEntry in _projectEntries)
		{
			TreeItem treeItem = _projectList.CreateItem(parent);
			treeItem.SetText(0, projectEntry.Exists ? projectEntry.Name : (projectEntry.Name + " (缺失)"));
			treeItem.SetText(1, projectEntry.Version);
			treeItem.SetText(2, projectEntry.ProjectDirectory);
			treeItem.SetTooltipText(0, projectEntry.ProjectFilePath);
			treeItem.SetMetadata(0, projectEntry.ProjectFilePath);
			if (!projectEntry.Exists)
			{
				Color color = new Color(1f, 0.45f, 0.38f, 0.92f);
				treeItem.SetCustomColor(0, color);
				treeItem.SetCustomColor(1, color);
				treeItem.SetCustomColor(2, color);
			}
		}
		bool flag = _projectEntries.Count > 0;
		_emptyHintLabel.Visible = !flag;
		if (!flag)
		{
			SelectProject("");
		}
		else if (!string.IsNullOrWhiteSpace(_selectedProjectFilePath))
		{
			SelectProject(_selectedProjectFilePath);
		}
		else
		{
			SelectFirstProject();
		}
	}

	private void OnProjectItemSelected()
	{
		SelectProject(_projectList.GetSelected()?.GetMetadata(0).AsString() ?? "");
	}

	private void SelectFirstProject()
	{
		TreeItem treeItem = _projectList.GetRoot()?.GetFirstChild();
		if (treeItem == null)
		{
			SelectProject("");
			return;
		}
		treeItem.Select(0);
		SelectProject(treeItem.GetMetadata(0).AsString());
	}

	private void SelectProject(string projectFilePath)
	{
		_selectedProjectFilePath = NormalizeProjectFilePath(projectFilePath);
		ProjectEntry projectEntry = _projectEntries.Find((ProjectEntry item) => string.Equals(item.ProjectFilePath, _selectedProjectFilePath, StringComparison.OrdinalIgnoreCase));
		bool flag = projectEntry != null;
		bool flag2 = flag && projectEntry.Exists;
		_openSelectedProjectButton.Disabled = !flag2;
		_removeMissingButton.Disabled = !_projectEntries.Exists((ProjectEntry item) => !item.Exists);
		if (!flag)
		{
			_projectNameLabel.Text = "未选择工程";
			_projectPathLabel.Text = "";
			_projectMetaLabel.Text = "";
			_projectDescriptionLabel.Text = "从左侧新建或打开一个 Mod 工程。";
		}
		else
		{
			_projectNameLabel.Text = projectEntry.Name;
			_projectPathLabel.Text = projectEntry.ProjectDirectory;
			_projectMetaLabel.Text = (projectEntry.Exists ? ("版本 " + projectEntry.Version + "    作者 " + projectEntry.Author) : "工程文件不存在");
			_projectDescriptionLabel.Text = (string.IsNullOrWhiteSpace(projectEntry.Description) ? "没有描述。" : projectEntry.Description);
		}
	}

	private void OpenSelectedProject()
	{
		ProjectEntry projectEntry = _projectEntries.Find((ProjectEntry item) => string.Equals(item.ProjectFilePath, _selectedProjectFilePath, StringComparison.OrdinalIgnoreCase));
		if (projectEntry != null && projectEntry.Exists)
		{
			EmitSignal(SignalName.ProjectOpenRequested, projectEntry.ProjectFilePath);
		}
	}

	private void RemoveMissingProjectRecords()
	{
		_recentProjectFiles.RemoveAll((string path) => !File.Exists(path));
		SaveRecentProjects();
		RefreshProjectList();
	}

	private static ProjectEntry ReadProjectEntry(string projectFilePath)
	{
		string text = NormalizeProjectFilePath(projectFilePath);
		ProjectEntry projectEntry = new ProjectEntry
		{
			ProjectFilePath = text,
			ProjectDirectory = (string.IsNullOrWhiteSpace(text) ? "" : (Path.GetDirectoryName(text) ?? "")),
			Name = (string.IsNullOrWhiteSpace(text) ? "未知工程" : Path.GetFileNameWithoutExtension(text)),
			Version = "-",
			Author = "-",
			Description = "",
			Exists = File.Exists(text)
		};
		if (!projectEntry.Exists)
		{
			return projectEntry;
		}
		try
		{
			ModProject modProject = JsonSerializer.Deserialize(File.ReadAllText(text), XWModJsonContext.Default.ModProject);
			if (modProject == null)
			{
				return projectEntry;
			}
			projectEntry.Name = (string.IsNullOrWhiteSpace(modProject.Name) ? projectEntry.Name : modProject.Name);
			projectEntry.Version = (string.IsNullOrWhiteSpace(modProject.Version) ? "-" : modProject.Version);
			projectEntry.Author = (string.IsNullOrWhiteSpace(modProject.Author) ? "-" : modProject.Author);
			projectEntry.Description = modProject.Description ?? "";
		}
		catch (Exception ex)
		{
			projectEntry.Description = "读取工程信息失败: " + ex.Message;
		}
		return projectEntry;
	}

	private static string NormalizeProjectFilePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		string text = path.Trim();
		if (text.StartsWith("user://") || text.StartsWith("res://"))
		{
			text = ProjectSettings.GlobalizePath(text);
		}
		try
		{
			text = Path.GetFullPath(text);
		}
		catch
		{
			return text.Replace('\\', '/');
		}
		return text.Replace('\\', '/');
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowStartScreen, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddRecentProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectFilePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetupProjectList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadRecentProjects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveRecentProjects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshProjectList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnProjectItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectFirstProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectFilePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenSelectedProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveMissingProjectRecords, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeProjectFilePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName.ShowStartScreen && args.Count == 0)
		{
			ShowStartScreen();
			ret = default;
			return true;
		}
		if (method == MethodName.AddRecentProject && args.Count == 1)
		{
			AddRecentProject(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetupProjectList && args.Count == 0)
		{
			SetupProjectList();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectSignals && args.Count == 0)
		{
			ConnectSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadRecentProjects && args.Count == 0)
		{
			LoadRecentProjects();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveRecentProjects && args.Count == 0)
		{
			SaveRecentProjects();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshProjectList && args.Count == 0)
		{
			RefreshProjectList();
			ret = default;
			return true;
		}
		if (method == MethodName.OnProjectItemSelected && args.Count == 0)
		{
			OnProjectItemSelected();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectFirstProject && args.Count == 0)
		{
			SelectFirstProject();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectProject && args.Count == 1)
		{
			SelectProject(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenSelectedProject && args.Count == 0)
		{
			OpenSelectedProject();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveMissingProjectRecords && args.Count == 0)
		{
			RemoveMissingProjectRecords();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeProjectFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeProjectFilePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.NormalizeProjectFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeProjectFilePath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.ShowStartScreen)
		{
			return true;
		}
		if (method == MethodName.AddRecentProject)
		{
			return true;
		}
		if (method == MethodName.SetupProjectList)
		{
			return true;
		}
		if (method == MethodName.ConnectSignals)
		{
			return true;
		}
		if (method == MethodName.LoadRecentProjects)
		{
			return true;
		}
		if (method == MethodName.SaveRecentProjects)
		{
			return true;
		}
		if (method == MethodName.RefreshProjectList)
		{
			return true;
		}
		if (method == MethodName.OnProjectItemSelected)
		{
			return true;
		}
		if (method == MethodName.SelectFirstProject)
		{
			return true;
		}
		if (method == MethodName.SelectProject)
		{
			return true;
		}
		if (method == MethodName.OpenSelectedProject)
		{
			return true;
		}
		if (method == MethodName.RemoveMissingProjectRecords)
		{
			return true;
		}
		if (method == MethodName.NormalizeProjectFilePath)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._projectList)
		{
			_projectList = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._projectNameLabel)
		{
			_projectNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._projectPathLabel)
		{
			_projectPathLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._projectMetaLabel)
		{
			_projectMetaLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._projectDescriptionLabel)
		{
			_projectDescriptionLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._emptyHintLabel)
		{
			_emptyHintLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._newProjectButton)
		{
			_newProjectButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._openProjectButton)
		{
			_openProjectButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._refreshProjectsButton)
		{
			_refreshProjectsButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._openSelectedProjectButton)
		{
			_openSelectedProjectButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._removeMissingButton)
		{
			_removeMissingButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._selectedProjectFilePath)
		{
			_selectedProjectFilePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._projectList)
		{
			value = VariantUtils.CreateFrom(in _projectList);
			return true;
		}
		if (name == PropertyName._projectNameLabel)
		{
			value = VariantUtils.CreateFrom(in _projectNameLabel);
			return true;
		}
		if (name == PropertyName._projectPathLabel)
		{
			value = VariantUtils.CreateFrom(in _projectPathLabel);
			return true;
		}
		if (name == PropertyName._projectMetaLabel)
		{
			value = VariantUtils.CreateFrom(in _projectMetaLabel);
			return true;
		}
		if (name == PropertyName._projectDescriptionLabel)
		{
			value = VariantUtils.CreateFrom(in _projectDescriptionLabel);
			return true;
		}
		if (name == PropertyName._emptyHintLabel)
		{
			value = VariantUtils.CreateFrom(in _emptyHintLabel);
			return true;
		}
		if (name == PropertyName._newProjectButton)
		{
			value = VariantUtils.CreateFrom(in _newProjectButton);
			return true;
		}
		if (name == PropertyName._openProjectButton)
		{
			value = VariantUtils.CreateFrom(in _openProjectButton);
			return true;
		}
		if (name == PropertyName._refreshProjectsButton)
		{
			value = VariantUtils.CreateFrom(in _refreshProjectsButton);
			return true;
		}
		if (name == PropertyName._openSelectedProjectButton)
		{
			value = VariantUtils.CreateFrom(in _openSelectedProjectButton);
			return true;
		}
		if (name == PropertyName._removeMissingButton)
		{
			value = VariantUtils.CreateFrom(in _removeMissingButton);
			return true;
		}
		if (name == PropertyName._selectedProjectFilePath)
		{
			value = VariantUtils.CreateFrom(in _selectedProjectFilePath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._projectList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectPathLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectMetaLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectDescriptionLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyHintLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._newProjectButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openProjectButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._refreshProjectsButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openSelectedProjectButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._removeMissingButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._selectedProjectFilePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._projectList, Variant.From(in _projectList));
		info.AddProperty(PropertyName._projectNameLabel, Variant.From(in _projectNameLabel));
		info.AddProperty(PropertyName._projectPathLabel, Variant.From(in _projectPathLabel));
		info.AddProperty(PropertyName._projectMetaLabel, Variant.From(in _projectMetaLabel));
		info.AddProperty(PropertyName._projectDescriptionLabel, Variant.From(in _projectDescriptionLabel));
		info.AddProperty(PropertyName._emptyHintLabel, Variant.From(in _emptyHintLabel));
		info.AddProperty(PropertyName._newProjectButton, Variant.From(in _newProjectButton));
		info.AddProperty(PropertyName._openProjectButton, Variant.From(in _openProjectButton));
		info.AddProperty(PropertyName._refreshProjectsButton, Variant.From(in _refreshProjectsButton));
		info.AddProperty(PropertyName._openSelectedProjectButton, Variant.From(in _openSelectedProjectButton));
		info.AddProperty(PropertyName._removeMissingButton, Variant.From(in _removeMissingButton));
		info.AddProperty(PropertyName._selectedProjectFilePath, Variant.From(in _selectedProjectFilePath));
		info.AddSignalEventDelegate(SignalName.ProjectOpenRequested, backing_ProjectOpenRequested);
		info.AddSignalEventDelegate(SignalName.NewProjectRequested, backing_NewProjectRequested);
		info.AddSignalEventDelegate(SignalName.OpenProjectRequested, backing_OpenProjectRequested);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._projectList, out var value))
		{
			_projectList = value.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._projectNameLabel, out var value2))
		{
			_projectNameLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._projectPathLabel, out var value3))
		{
			_projectPathLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._projectMetaLabel, out var value4))
		{
			_projectMetaLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._projectDescriptionLabel, out var value5))
		{
			_projectDescriptionLabel = value5.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._emptyHintLabel, out var value6))
		{
			_emptyHintLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._newProjectButton, out var value7))
		{
			_newProjectButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._openProjectButton, out var value8))
		{
			_openProjectButton = value8.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._refreshProjectsButton, out var value9))
		{
			_refreshProjectsButton = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._openSelectedProjectButton, out var value10))
		{
			_openSelectedProjectButton = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._removeMissingButton, out var value11))
		{
			_removeMissingButton = value11.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._selectedProjectFilePath, out var value12))
		{
			_selectedProjectFilePath = value12.As<string>();
		}
		if (info.TryGetSignalEventDelegate<ProjectOpenRequestedEventHandler>(SignalName.ProjectOpenRequested, out var value13))
		{
			backing_ProjectOpenRequested = value13;
		}
		if (info.TryGetSignalEventDelegate<NewProjectRequestedEventHandler>(SignalName.NewProjectRequested, out var value14))
		{
			backing_NewProjectRequested = value14;
		}
		if (info.TryGetSignalEventDelegate<OpenProjectRequestedEventHandler>(SignalName.OpenProjectRequested, out var value15))
		{
			backing_OpenProjectRequested = value15;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(SignalName.ProjectOpenRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectFilePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.NewProjectRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.OpenProjectRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalProjectOpenRequested(string projectFilePath)
	{
		EmitSignal(SignalName.ProjectOpenRequested, new ReadOnlySpan<Variant>((Variant)projectFilePath));
	}

	protected void EmitSignalNewProjectRequested()
	{
		EmitSignal(SignalName.NewProjectRequested, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalOpenProjectRequested()
	{
		EmitSignal(SignalName.OpenProjectRequested, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ProjectOpenRequested && args.Count == 1)
		{
			backing_ProjectOpenRequested?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else if (signal == SignalName.NewProjectRequested && args.Count == 0)
		{
			backing_NewProjectRequested?.Invoke();
		}
		else if (signal == SignalName.OpenProjectRequested && args.Count == 0)
		{
			backing_OpenProjectRequested?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ProjectOpenRequested)
		{
			return true;
		}
		if (signal == SignalName.NewProjectRequested)
		{
			return true;
		}
		if (signal == SignalName.OpenProjectRequested)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
