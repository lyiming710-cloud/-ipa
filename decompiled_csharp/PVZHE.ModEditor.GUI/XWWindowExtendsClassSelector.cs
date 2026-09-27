using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Blueprint;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Registry.BP;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/Window/Selector/ExtendsClass/XWWindowExtendsClassSelector.cs")]
public class XWWindowExtendsClassSelector : XWWindowSelector
{
	[Signal]
	public delegate void ClassSelectedEventHandler(StringName className);

	public new class MethodName : XWWindowSelector.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName OnCloseRequested = "OnCloseRequested";

		public new static readonly StringName OnConfirmed = "OnConfirmed";

		public new static readonly StringName BuildTree = "BuildTree";

		public static readonly StringName BuildProjectClassTree = "BuildProjectClassTree";

		public static readonly StringName BeginProjectIndexWarmup = "BeginProjectIndexWarmup";

		public new static readonly StringName OnItemSelected = "OnItemSelected";

		public new static readonly StringName ShowDescription = "ShowDescription";

		public new static readonly StringName CanFilterTreeItem = "CanFilterTreeItem";
	}

	public new class PropertyName : XWWindowSelector.PropertyName
	{
		public static readonly StringName ContextEditor = "ContextEditor";

		public static readonly StringName _selectedData = "_selectedData";

		public static readonly StringName _selectedProjectClassName = "_selectedProjectClassName";

		public static readonly StringName _catalogProjectRoot = "_catalogProjectRoot";

		public static readonly StringName _catalogProjectContextVersion = "_catalogProjectContextVersion";

		public static readonly StringName _catalogScriptData = "_catalogScriptData";

		public static readonly StringName _projectIndexRequested = "_projectIndexRequested";
	}

	public new class SignalName : XWWindowSelector.SignalName
	{
		public static readonly StringName ClassSelected = "ClassSelected";
	}

	private const string ScenePath = "res://addons/ModEditor/Window/Selector/ExtendsClass/XWWindowExtendsClassSelector.tscn";

	private XWClassData _selectedData;

	private string _selectedProjectClassName = "";

	private string _catalogProjectRoot = "";

	private int _catalogProjectContextVersion;

	private XWBPScriptData _catalogScriptData;

	private bool _projectIndexRequested;

	private ClassSelectedEventHandler backing_ClassSelected;

	public XWBPEditor ContextEditor { get; set; }

	public event ClassSelectedEventHandler ClassSelected
	{
		add
		{
			backing_ClassSelected = (ClassSelectedEventHandler)Delegate.Combine(backing_ClassSelected, value);
		}
		remove
		{
			backing_ClassSelected = (ClassSelectedEventHandler)Delegate.Remove(backing_ClassSelected, value);
		}
	}

	public static XWWindowExtendsClassSelector Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Window/Selector/ExtendsClass/XWWindowExtendsClassSelector.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWWindowExtendsClassSelector>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		FavoritesFilePath = "user://xweditor_extends_class_favorites.cfg";
		RecentFilePath = "user://xweditor_extends_class_recent.cfg";
		_catalogProjectRoot = XWBPCSharpMemberRegistry.Instance.ActiveProjectRoot;
		_catalogProjectContextVersion = XWBPCSharpMemberRegistry.Instance.ProjectContextVersion;
		_catalogScriptData = ContextEditor?.BpScriptData;
		base._Ready();
		Title = "选择继承类";
		OkButtonText = "继承";
		GetOkButton().Disabled = true;
		if (GodotObject.IsInstanceValid(_searchLineEdit))
		{
			_searchLineEdit.PlaceholderText = "搜索类";
			_searchLineEdit.GrabFocus();
		}
		BeginProjectIndexWarmup();
	}

	protected override void OnCloseRequested()
	{
		QueueFree();
	}

	protected override void OnConfirmed()
	{
		XWBPCSharpMemberRegistry instance = XWBPCSharpMemberRegistry.Instance;
		if (!string.Equals(_catalogProjectRoot, instance.ActiveProjectRoot, StringComparison.OrdinalIgnoreCase) || _catalogProjectContextVersion != instance.ProjectContextVersion)
		{
			XWEditorInterface.Instance?.ShowToast("Mod 或 C# API 已变化，旧父类选择已取消，请重新打开类型目录。", 2);
			QueueFree();
			return;
		}
		if (GodotObject.IsInstanceValid(ContextEditor) && _catalogScriptData != ContextEditor.BpScriptData)
		{
			XWEditorInterface.Instance?.ShowToast("当前蓝图文档已变化，旧父类选择已取消，请在当前蓝图重新打开类型目录。", 2);
			QueueFree();
			return;
		}
		string text = (GodotObject.IsInstanceValid(_selectedData) ? _selectedData.ClassName : _selectedProjectClassName);
		if (!GetOkButton().Disabled && !string.IsNullOrWhiteSpace(text))
		{
			AddToRecent(text);
			EmitSignal(SignalName.ClassSelected, new StringName(text));
			QueueFree();
		}
	}

	protected override void BuildTree()
	{
		base.BuildTree();
		List<string> currentClassList = new List<string>(XWClassRegistry.Instance.GetAllClass());
		string text = (XWClassRegistry.Instance.HasClass("Object") ? "Object" : "GodotObject");
		if (XWClassRegistry.Instance.HasClass(text))
		{
			XWClassData classData = XWClassRegistry.Instance.GetClassData(text);
			TreeItem treeItem = _tree.CreateItem(Root);
			treeItem.Collapsed = false;
			treeItem.SetText(0, text);
			treeItem.SetIcon(0, XWClassRegistry.Instance.GetClassIcon(text));
			treeItem.SetMetadata(0, classData);
			BuildClassTree(currentClassList, text, treeItem);
			BuildProjectClassTree();
		}
	}

	private void BuildProjectClassTree()
	{
		XWBPCSharpMemberRegistry instance = XWBPCSharpMemberRegistry.Instance;
		if (string.IsNullOrWhiteSpace(instance.ActiveProjectRoot))
		{
			return;
		}
		if (!instance.IsProjectScanReady || !instance.IsHostScanReady)
		{
			TreeItem treeItem = _tree.CreateItem(Root);
			treeItem.Collapsed = false;
			treeItem.SetText(0, "当前 Mod C# API");
			treeItem.SetSelectable(0, selectable: false);
			treeItem.SetIcon(0, XWClassRegistry.Instance.GetUIIcon("Script"));
			TreeItem treeItem2 = _tree.CreateItem(treeItem);
			treeItem2.SetText(0, "正在后台索引可继承的 C# 类…");
			treeItem2.SetSelectable(0, selectable: false);
			BeginProjectIndexWarmup();
			return;
		}
		IReadOnlyList<XWBPCSharpMemberRegistry.CSharpClassDescriptor> projectClassDescriptors = instance.GetProjectClassDescriptors();
		if (projectClassDescriptors.Count == 0)
		{
			return;
		}
		TreeItem treeItem3 = _tree.CreateItem(Root);
		treeItem3.Collapsed = false;
		treeItem3.SetText(0, "当前 Mod C# API");
		treeItem3.SetSelectable(0, selectable: false);
		treeItem3.SetIcon(0, XWClassRegistry.Instance.GetUIIcon("Script"));
		foreach (XWBPCSharpMemberRegistry.CSharpClassDescriptor item in projectClassDescriptors)
		{
			Dictionary dictionary = new Dictionary
			{
				["xw_mod_csharp_class"] = true,
				["class_name"] = (string.IsNullOrWhiteSpace(item.QualifiedName) ? item.Name : item.QualifiedName),
				["display_name"] = item.Name,
				["qualified_name"] = item.QualifiedName,
				["base_class"] = item.BaseClass,
				["script_path"] = item.ScriptPath
			};
			TreeItem treeItem4 = _tree.CreateItem(treeItem3);
			treeItem4.Collapsed = true;
			treeItem4.SetText(0, item.CanInherit ? item.Name : (item.Name + " · 仅 API"));
			treeItem4.SetTooltipText(0, item.CanInherit ? (item.QualifiedName + "\n" + item.ScriptPath) : $"{item.QualifiedName}\n{item.ScriptPath}\n{item.InheritanceBlockReason}");
			treeItem4.SetIcon(0, XWClassRegistry.Instance.GetClassIcon(item.BaseClass) ?? XWClassRegistry.Instance.GetUIIcon("Script"));
			treeItem4.SetSelectable(0, item.CanInherit);
			if (item.CanInherit)
			{
				treeItem4.SetMetadata(0, dictionary);
			}
		}
	}

	private async void BeginProjectIndexWarmup()
	{
		XWBPCSharpMemberRegistry registry = XWBPCSharpMemberRegistry.Instance;
		if (_projectIndexRequested || (registry.IsProjectScanReady && registry.IsHostScanReady) || string.IsNullOrWhiteSpace(registry.ActiveProjectRoot))
		{
			return;
		}
		string rootSnapshot = registry.ActiveProjectRoot;
		int versionSnapshot = registry.ProjectContextVersion;
		XWBPScriptData scriptSnapshot = ContextEditor?.BpScriptData;
		_projectIndexRequested = true;
		bool ready = false;
		try
		{
			ready = await registry.EnsureProjectScannedAsync();
		}
		catch (Exception ex)
		{
			GD.PushWarning("当前 Mod C# 继承类型后台索引失败：" + ex.Message);
		}
		finally
		{
			_projectIndexRequested = false;
		}
		if (ready && GodotObject.IsInstanceValid(this) && IsInsideTree() && string.Equals(rootSnapshot, registry.ActiveProjectRoot, StringComparison.OrdinalIgnoreCase) && versionSnapshot == registry.ProjectContextVersion && scriptSnapshot == ContextEditor?.BpScriptData)
		{
			string query = (GodotObject.IsInstanceValid(_searchLineEdit) ? _searchLineEdit.Text : "");
			_selectedData = null;
			_selectedProjectClassName = "";
			BuildTree();
			FilterTree(query);
			GetOkButton().Disabled = true;
			_nameLabel.Text = "";
			_describeLabel.Text = "";
		}
	}

	private void BuildClassTree(List<string> currentClassList, string currentClass, TreeItem parentItem)
	{
		List<string> list = new List<string>(XWClassRegistry.Instance.GetInheritersFromClass(currentClass));
		list.Sort(StringComparer.OrdinalIgnoreCase);
		foreach (string item in list)
		{
			if (currentClassList.Contains(item) && !(XWClassRegistry.Instance.GetParentClass(item) != currentClass))
			{
				XWClassData classData = XWClassRegistry.Instance.GetClassData(item);
				if (GodotObject.IsInstanceValid(classData))
				{
					currentClassList.Remove(item);
					TreeItem treeItem = _tree.CreateItem(parentItem);
					treeItem.Collapsed = true;
					treeItem.SetText(0, item);
					treeItem.SetIcon(0, XWClassRegistry.Instance.GetClassIcon(item));
					treeItem.SetMetadata(0, classData);
					BuildClassTree(currentClassList, item, treeItem);
				}
			}
		}
	}

	protected override void OnItemSelected()
	{
		TreeItem selected = _tree.GetSelected();
		if (GodotObject.IsInstanceValid(selected))
		{
			Variant metadata = selected.GetMetadata(0);
			string className;
			string qualifiedName;
			string baseClass;
			string scriptPath;
			if (metadata.VariantType == Variant.Type.Object && metadata.As<GodotObject>() is XWClassData selectedData)
			{
				_selectedData = selectedData;
				_selectedProjectClassName = "";
				ShowDescription(selected);
				GetOkButton().Disabled = false;
			}
			else if (TryReadProjectClass(metadata, out className, out qualifiedName, out baseClass, out scriptPath))
			{
				_selectedData = null;
				_selectedProjectClassName = className;
				string text = (className.Contains('.') ? className.Substring(className.LastIndexOf('.') + 1) : className);
				_nameLabel.Text = text + "\n";
				_describeLabel.Text = $"[color=gray]类型:[/color] 当前 Mod C# 类\n[color=gray]完整名:[/color] {qualifiedName}\n[color=gray]基类:[/color] {baseClass}\n[color=gray]脚本:[/color] {scriptPath}";
				GetOkButton().Disabled = false;
			}
			else
			{
				_selectedData = null;
				_selectedProjectClassName = "";
				GetOkButton().Disabled = true;
			}
		}
	}

	private void ShowDescription(TreeItem item)
	{
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType != Variant.Type.Object || !(metadata.As<GodotObject>() is XWClassData xWClassData))
		{
			_nameLabel.Text = "";
			_describeLabel.Text = "";
			return;
		}
		string text;
		if (xWClassData.IsGodotClass)
		{
			text = "Godot 内置类";
		}
		else
		{
			text = (xWClassData.IsGlobalClass ? "全局脚本类" : "脚本类");
		}
		string parentClass = XWClassRegistry.Instance.GetParentClass(xWClassData.ClassName);
		_nameLabel.Text = xWClassData.ClassName + "\n";
		_describeLabel.Text = "[color=gray]类型:[/color] " + text + "\n[color=gray]基类:[/color] " + parentClass;
		if (!string.IsNullOrEmpty(xWClassData.ScriptPath))
		{
			RichTextLabel describeLabel = _describeLabel;
			describeLabel.Text = describeLabel.Text + "\n[color=gray]脚本:[/color] " + xWClassData.ScriptPath;
		}
	}

	protected override bool CanFilterTreeItem(string lowerQuery, TreeItem treeItem)
	{
		string text = treeItem.GetText(0);
		if (!string.IsNullOrEmpty(text) && text.ToLower().Contains(lowerQuery))
		{
			return true;
		}
		Variant metadata = treeItem.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Object && metadata.As<GodotObject>() is XWClassData xWClassData)
		{
			if (!xWClassData.ClassName.ToLower().Contains(lowerQuery) && !xWClassData.BaseClass.ToLower().Contains(lowerQuery))
			{
				return xWClassData.ScriptPath.ToLower().Contains(lowerQuery);
			}
			return true;
		}
		if (TryReadProjectClass(metadata, out var className, out var qualifiedName, out var baseClass, out var scriptPath))
		{
			if (!className.ToLowerInvariant().Contains(lowerQuery) && !qualifiedName.ToLowerInvariant().Contains(lowerQuery) && !baseClass.ToLowerInvariant().Contains(lowerQuery))
			{
				return scriptPath.ToLowerInvariant().Contains(lowerQuery);
			}
			return true;
		}
		return false;
	}

	private static bool TryReadProjectClass(Variant metadata, out string className, out string qualifiedName, out string baseClass, out string scriptPath)
	{
		className = "";
		qualifiedName = "";
		baseClass = "";
		scriptPath = "";
		if (metadata.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		Dictionary dictionary = metadata.As<Dictionary>();
		if (!dictionary.TryGetValue("xw_mod_csharp_class", out var value) || !value.AsBool())
		{
			return false;
		}
		className = (dictionary.TryGetValue("class_name", out var value2) ? value2.AsString() : "");
		qualifiedName = (dictionary.TryGetValue("qualified_name", out var value3) ? value3.AsString() : className);
		baseClass = (dictionary.TryGetValue("base_class", out var value4) ? value4.AsString() : "");
		scriptPath = (dictionary.TryGetValue("script_path", out var value5) ? value5.AsString() : "");
		return !string.IsNullOrWhiteSpace(className);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ConfirmationDialog"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCloseRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildProjectClassTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginProjectIndexWarmup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowDescription, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanFilterTreeItem, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "lowerQuery", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "treeItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWWindowExtendsClassSelector>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCloseRequested && args.Count == 0)
		{
			OnCloseRequested();
			ret = default;
			return true;
		}
		if (method == MethodName.OnConfirmed && args.Count == 0)
		{
			OnConfirmed();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTree && args.Count == 0)
		{
			BuildTree();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildProjectClassTree && args.Count == 0)
		{
			BuildProjectClassTree();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginProjectIndexWarmup && args.Count == 0)
		{
			BeginProjectIndexWarmup();
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemSelected && args.Count == 0)
		{
			OnItemSelected();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowDescription && args.Count == 1)
		{
			ShowDescription(VariantUtils.ConvertTo<TreeItem>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanFilterTreeItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanFilterTreeItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TreeItem>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWWindowExtendsClassSelector>(Create());
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
		if (method == MethodName.OnCloseRequested)
		{
			return true;
		}
		if (method == MethodName.OnConfirmed)
		{
			return true;
		}
		if (method == MethodName.BuildTree)
		{
			return true;
		}
		if (method == MethodName.BuildProjectClassTree)
		{
			return true;
		}
		if (method == MethodName.BeginProjectIndexWarmup)
		{
			return true;
		}
		if (method == MethodName.OnItemSelected)
		{
			return true;
		}
		if (method == MethodName.ShowDescription)
		{
			return true;
		}
		if (method == MethodName.CanFilterTreeItem)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ContextEditor)
		{
			ContextEditor = VariantUtils.ConvertTo<XWBPEditor>(in value);
			return true;
		}
		if (name == PropertyName._selectedData)
		{
			_selectedData = VariantUtils.ConvertTo<XWClassData>(in value);
			return true;
		}
		if (name == PropertyName._selectedProjectClassName)
		{
			_selectedProjectClassName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._catalogProjectRoot)
		{
			_catalogProjectRoot = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._catalogProjectContextVersion)
		{
			_catalogProjectContextVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._catalogScriptData)
		{
			_catalogScriptData = VariantUtils.ConvertTo<XWBPScriptData>(in value);
			return true;
		}
		if (name == PropertyName._projectIndexRequested)
		{
			_projectIndexRequested = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ContextEditor)
		{
			value = VariantUtils.CreateFrom<XWBPEditor>(ContextEditor);
			return true;
		}
		if (name == PropertyName._selectedData)
		{
			value = VariantUtils.CreateFrom(in _selectedData);
			return true;
		}
		if (name == PropertyName._selectedProjectClassName)
		{
			value = VariantUtils.CreateFrom(in _selectedProjectClassName);
			return true;
		}
		if (name == PropertyName._catalogProjectRoot)
		{
			value = VariantUtils.CreateFrom(in _catalogProjectRoot);
			return true;
		}
		if (name == PropertyName._catalogProjectContextVersion)
		{
			value = VariantUtils.CreateFrom(in _catalogProjectContextVersion);
			return true;
		}
		if (name == PropertyName._catalogScriptData)
		{
			value = VariantUtils.CreateFrom(in _catalogScriptData);
			return true;
		}
		if (name == PropertyName._projectIndexRequested)
		{
			value = VariantUtils.CreateFrom(in _projectIndexRequested);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._selectedProjectClassName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._catalogProjectRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._catalogProjectContextVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._catalogScriptData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._projectIndexRequested, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ContextEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ContextEditor, Variant.From<XWBPEditor>(ContextEditor));
		info.AddProperty(PropertyName._selectedData, Variant.From(in _selectedData));
		info.AddProperty(PropertyName._selectedProjectClassName, Variant.From(in _selectedProjectClassName));
		info.AddProperty(PropertyName._catalogProjectRoot, Variant.From(in _catalogProjectRoot));
		info.AddProperty(PropertyName._catalogProjectContextVersion, Variant.From(in _catalogProjectContextVersion));
		info.AddProperty(PropertyName._catalogScriptData, Variant.From(in _catalogScriptData));
		info.AddProperty(PropertyName._projectIndexRequested, Variant.From(in _projectIndexRequested));
		info.AddSignalEventDelegate(SignalName.ClassSelected, backing_ClassSelected);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ContextEditor, out var value))
		{
			ContextEditor = value.As<XWBPEditor>();
		}
		if (info.TryGetProperty(PropertyName._selectedData, out var value2))
		{
			_selectedData = value2.As<XWClassData>();
		}
		if (info.TryGetProperty(PropertyName._selectedProjectClassName, out var value3))
		{
			_selectedProjectClassName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._catalogProjectRoot, out var value4))
		{
			_catalogProjectRoot = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName._catalogProjectContextVersion, out var value5))
		{
			_catalogProjectContextVersion = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._catalogScriptData, out var value6))
		{
			_catalogScriptData = value6.As<XWBPScriptData>();
		}
		if (info.TryGetProperty(PropertyName._projectIndexRequested, out var value7))
		{
			_projectIndexRequested = value7.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<ClassSelectedEventHandler>(SignalName.ClassSelected, out var value8))
		{
			backing_ClassSelected = value8;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.ClassSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalClassSelected(StringName className)
	{
		EmitSignal(SignalName.ClassSelected, new ReadOnlySpan<Variant>((Variant)className));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ClassSelected && args.Count == 1)
		{
			backing_ClassSelected?.Invoke(VariantUtils.ConvertTo<StringName>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ClassSelected)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
