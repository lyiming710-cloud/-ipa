using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.GUI;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/SceneNodeTree/NodeSelector/XWWindowNodeSelector.cs")]
public class XWWindowNodeSelector : XWWindowSelector
{
	[Signal]
	public delegate void NodeTypeSelectedEventHandler(string className);

	public new class MethodName : XWWindowSelector.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName OnCloseRequested = "OnCloseRequested";

		public new static readonly StringName OnConfirmed = "OnConfirmed";

		public new static readonly StringName BuildTree = "BuildTree";

		public new static readonly StringName OnItemSelected = "OnItemSelected";

		public new static readonly StringName CanFilterTreeItem = "CanFilterTreeItem";

		public static readonly StringName BuildNodeClassCache = "BuildNodeClassCache";

		public static readonly StringName BuildInheritanceTree = "BuildInheritanceTree";

		public static readonly StringName CreateNodeItem = "CreateNodeItem";

		public static readonly StringName BuildNodeMetadata = "BuildNodeMetadata";

		public static readonly StringName GetSelectedClassName = "GetSelectedClassName";

		public static readonly StringName ShowNodeDescription = "ShowNodeDescription";

		public static readonly StringName DictionaryStringContains = "DictionaryStringContains";

		public static readonly StringName IsDisplayableNodeClass = "IsDisplayableNodeClass";

		public static readonly StringName IsEditorOnlyClass = "IsEditorOnlyClass";

		public static readonly StringName BuildTooltip = "BuildTooltip";
	}

	public new class PropertyName : XWWindowSelector.PropertyName
	{
	}

	public new class SignalName : XWWindowSelector.SignalName
	{
		public static readonly StringName NodeTypeSelected = "NodeTypeSelected";
	}

	private const string ScenePath = "res://addons/ModEditor/SceneEditor/SceneNodeTree/NodeSelector/XWWindowNodeSelector.tscn";

	private const string MetaTypeId = "type_id";

	private const string MetaClassName = "class_name";

	private const string MetaScriptPath = "script_path";

	private const string MetaIsCustom = "is_custom";

	private static PackedScene _scene;

	private readonly System.Collections.Generic.Dictionary<string, XWClassData> _nodeClassData = new System.Collections.Generic.Dictionary<string, XWClassData>(StringComparer.Ordinal);

	private readonly HashSet<string> _creatableClasses = new HashSet<string>(StringComparer.Ordinal);

	private NodeTypeSelectedEventHandler backing_NodeTypeSelected;

	public event NodeTypeSelectedEventHandler NodeTypeSelected
	{
		add
		{
			backing_NodeTypeSelected = (NodeTypeSelectedEventHandler)Delegate.Combine(backing_NodeTypeSelected, value);
		}
		remove
		{
			backing_NodeTypeSelected = (NodeTypeSelectedEventHandler)Delegate.Remove(backing_NodeTypeSelected, value);
		}
	}

	public static XWWindowNodeSelector Create()
	{
		if (_scene == null)
		{
			_scene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/SceneEditor/SceneNodeTree/NodeSelector/XWWindowNodeSelector.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		if (!GodotObject.IsInstanceValid(_scene))
		{
			return null;
		}
		return _scene.Instantiate<XWWindowNodeSelector>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		FavoritesFilePath = "user://xweditor_node_selector_favorites.cfg";
		RecentFilePath = "user://xweditor_node_selector_recent.cfg";
		base._Ready();
		Title = "创建节点";
		OkButtonText = "创建";
		Button okButton = GetOkButton();
		if (GodotObject.IsInstanceValid(okButton))
		{
			okButton.Disabled = true;
		}
		if (GodotObject.IsInstanceValid(_searchLineEdit))
		{
			_searchLineEdit.PlaceholderText = "搜索节点类型";
			_searchLineEdit.GrabFocus();
		}
	}

	protected override void OnCloseRequested()
	{
		QueueFree();
	}

	protected override void OnConfirmed()
	{
		Button okButton = GetOkButton();
		if (!GodotObject.IsInstanceValid(okButton) || !okButton.Disabled)
		{
			string selectedClassName = GetSelectedClassName();
			if (!string.IsNullOrEmpty(selectedClassName))
			{
				AddToRecent(new StringName(selectedClassName));
				EmitSignal(SignalName.NodeTypeSelected, selectedClassName);
				QueueFree();
			}
		}
	}

	protected override void BuildTree()
	{
		_tree.Clear();
		Root = _tree.CreateItem();
		AllCategories.Clear();
		CurrentResult.Clear();
		BuildNodeClassCache();
		BuildInheritanceTree();
	}

	protected override void OnItemSelected()
	{
		string selectedClassName = GetSelectedClassName();
		Button okButton = GetOkButton();
		if (GodotObject.IsInstanceValid(okButton))
		{
			okButton.Disabled = string.IsNullOrEmpty(selectedClassName);
		}
		if (string.IsNullOrEmpty(selectedClassName))
		{
			_nameLabel.Text = "";
			_describeLabel.Text = "";
		}
		else
		{
			ShowNodeDescription(selectedClassName);
		}
	}

	protected override bool CanFilterTreeItem(string lowerQuery, TreeItem treeItem)
	{
		string text = treeItem.GetText(0);
		if (!string.IsNullOrEmpty(text) && text.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		Variant metadata = treeItem.GetMetadata(0);
		if (metadata.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		Dictionary dict = metadata.As<Dictionary>();
		if (!DictionaryStringContains(dict, "class_name", lowerQuery))
		{
			return DictionaryStringContains(dict, "script_path", lowerQuery);
		}
		return true;
	}

	private void BuildNodeClassCache()
	{
		_nodeClassData.Clear();
		_creatableClasses.Clear();
		XWClassRegistry instance = XWClassRegistry.Instance;
		foreach (XWClassData allClassDatum in instance.GetAllClassData())
		{
			if (IsDisplayableNodeClass(instance, allClassDatum))
			{
				_nodeClassData[allClassDatum.ClassName] = allClassDatum;
				if (instance.CanInstantiate(allClassDatum.ClassName))
				{
					_creatableClasses.Add(allClassDatum.ClassName);
				}
			}
		}
	}

	private void BuildInheritanceTree()
	{
		if (!_nodeClassData.ContainsKey("Node"))
		{
			return;
		}
		HashSet<string> visited = new HashSet<string>(StringComparer.Ordinal) { "Node" };
		TreeItem parentItem = CreateNodeItem(Root, "Node", collapsed: false);
		BuildClassChildren("Node", parentItem, visited);
		string[] array = _nodeClassData.Keys.Where((string item) => !visited.Contains(item)).OrderBy((string result) => result, StringComparer.OrdinalIgnoreCase).ToArray();
		if (array.Length != 0)
		{
			TreeItem treeItem = _tree.CreateItem(Root);
			treeItem.Collapsed = true;
			treeItem.SetText(0, "其它");
			string[] array2 = array;
			foreach (string className in array2)
			{
				CreateNodeItem(treeItem, className);
			}
		}
	}

	private void BuildClassChildren(string parentClass, TreeItem parentItem, HashSet<string> visited)
	{
		string[] array = _nodeClassData.Keys.Where((string className) => className != parentClass && XWClassRegistry.Instance.GetParentClass(className) == parentClass).OrderBy((string className) => className, StringComparer.OrdinalIgnoreCase).ToArray();
		foreach (string text in array)
		{
			if (visited.Add(text))
			{
				TreeItem parentItem2 = CreateNodeItem(parentItem, text);
				BuildClassChildren(text, parentItem2, visited);
			}
		}
	}

	private TreeItem CreateNodeItem(TreeItem parent, string className, bool collapsed = true)
	{
		if (!_nodeClassData.TryGetValue(className, out var value))
		{
			return null;
		}
		TreeItem treeItem = _tree.CreateItem(parent);
		treeItem.Collapsed = collapsed;
		treeItem.SetText(0, className);
		treeItem.SetTooltipText(0, BuildTooltip(value));
		if (_creatableClasses.Contains(className))
		{
			treeItem.SetMetadata(0, BuildNodeMetadata(value));
		}
		else
		{
			treeItem.SetCustomColor(0, new Color(0.62f, 0.66f, 0.72f));
		}
		Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(className);
		if (GodotObject.IsInstanceValid(classIcon))
		{
			treeItem.SetIcon(0, classIcon);
		}
		return treeItem;
	}

	private Dictionary BuildNodeMetadata(XWClassData data)
	{
		return new Dictionary
		{
			{
				"type_id",
				new StringName(data.ClassName)
			},
			{ "class_name", data.ClassName },
			{
				"script_path",
				data.ScriptPath ?? ""
			},
			{ "is_custom", data.IsGlobalClass }
		};
	}

	private string GetSelectedClassName()
	{
		TreeItem selected = _tree.GetSelected();
		if (!GodotObject.IsInstanceValid(selected))
		{
			return "";
		}
		Variant metadata = selected.GetMetadata(0);
		if (metadata.VariantType != Variant.Type.Dictionary)
		{
			return "";
		}
		Dictionary dictionary = metadata.As<Dictionary>();
		if (!dictionary.ContainsKey("class_name"))
		{
			return "";
		}
		return dictionary["class_name"].AsString();
	}

	private void ShowNodeDescription(string className)
	{
		if (_nodeClassData.TryGetValue(className, out var value))
		{
			string parentClass = XWClassRegistry.Instance.GetParentClass(className);
			_nameLabel.Text = className;
			_describeLabel.Text = "类型：" + (value.IsGlobalClass ? "自定义节点" : "内置节点") + "\n基类：" + (string.IsNullOrEmpty(parentClass) ? "Node" : parentClass);
			if (value.IsGlobalClass && !string.IsNullOrEmpty(value.ScriptPath))
			{
				RichTextLabel describeLabel = _describeLabel;
				describeLabel.Text = describeLabel.Text + "\n脚本：" + value.ScriptPath;
			}
		}
	}

	private static bool DictionaryStringContains(Dictionary dict, string key, string lowerQuery)
	{
		if (!dict.ContainsKey(key))
		{
			return false;
		}
		string text = dict[key].AsString();
		if (!string.IsNullOrEmpty(text))
		{
			return text.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static bool IsDisplayableNodeClass(XWClassRegistry registry, XWClassData data)
	{
		if (data == null || string.IsNullOrEmpty(data.ClassName))
		{
			return false;
		}
		string className = data.ClassName;
		if (!registry.IsClassInstanceOf(className, "Node"))
		{
			return false;
		}
		if (registry.IsClassInstanceOf(className, "Node3D"))
		{
			return false;
		}
		return !IsEditorOnlyClass(className);
	}

	private static bool IsEditorOnlyClass(string className)
	{
		if (!className.StartsWith("Editor", StringComparison.Ordinal) && !className.StartsWith("ScriptEditor", StringComparison.Ordinal))
		{
			return className.StartsWith("VisualShaderEditor", StringComparison.Ordinal);
		}
		return true;
	}

	private static string BuildTooltip(XWClassData data)
	{
		if (data.IsGlobalClass && !string.IsNullOrEmpty(data.ScriptPath))
		{
			return data.ClassName + "\n" + data.ScriptPath;
		}
		return data.ClassName;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ConfirmationDialog"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCloseRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanFilterTreeItem, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "lowerQuery", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "treeItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildNodeClassCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildInheritanceTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateNodeItem, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "collapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildNodeMetadata, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedClassName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowNodeDescription, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DictionaryStringContains, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dict", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "lowerQuery", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsDisplayableNodeClass, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "registry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsEditorOnlyClass, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildTooltip, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWWindowNodeSelector>(Create());
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
		if (method == MethodName.OnItemSelected && args.Count == 0)
		{
			OnItemSelected();
			ret = default;
			return true;
		}
		if (method == MethodName.CanFilterTreeItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanFilterTreeItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TreeItem>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildNodeClassCache && args.Count == 0)
		{
			BuildNodeClassCache();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildInheritanceTree && args.Count == 0)
		{
			BuildInheritanceTree();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateNodeItem && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TreeItem>(CreateNodeItem(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.BuildNodeMetadata && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(BuildNodeMetadata(VariantUtils.ConvertTo<XWClassData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSelectedClassName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetSelectedClassName());
			return true;
		}
		if (method == MethodName.ShowNodeDescription && args.Count == 1)
		{
			ShowNodeDescription(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DictionaryStringContains && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(DictionaryStringContains(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.IsDisplayableNodeClass && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDisplayableNodeClass(VariantUtils.ConvertTo<XWClassRegistry>(in args[0]), VariantUtils.ConvertTo<XWClassData>(in args[1])));
			return true;
		}
		if (method == MethodName.IsEditorOnlyClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEditorOnlyClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildTooltip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildTooltip(VariantUtils.ConvertTo<XWClassData>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWWindowNodeSelector>(Create());
			return true;
		}
		if (method == MethodName.DictionaryStringContains && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(DictionaryStringContains(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.IsDisplayableNodeClass && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDisplayableNodeClass(VariantUtils.ConvertTo<XWClassRegistry>(in args[0]), VariantUtils.ConvertTo<XWClassData>(in args[1])));
			return true;
		}
		if (method == MethodName.IsEditorOnlyClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEditorOnlyClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildTooltip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildTooltip(VariantUtils.ConvertTo<XWClassData>(in args[0])));
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
		if (method == MethodName.OnItemSelected)
		{
			return true;
		}
		if (method == MethodName.CanFilterTreeItem)
		{
			return true;
		}
		if (method == MethodName.BuildNodeClassCache)
		{
			return true;
		}
		if (method == MethodName.BuildInheritanceTree)
		{
			return true;
		}
		if (method == MethodName.CreateNodeItem)
		{
			return true;
		}
		if (method == MethodName.BuildNodeMetadata)
		{
			return true;
		}
		if (method == MethodName.GetSelectedClassName)
		{
			return true;
		}
		if (method == MethodName.ShowNodeDescription)
		{
			return true;
		}
		if (method == MethodName.DictionaryStringContains)
		{
			return true;
		}
		if (method == MethodName.IsDisplayableNodeClass)
		{
			return true;
		}
		if (method == MethodName.IsEditorOnlyClass)
		{
			return true;
		}
		if (method == MethodName.BuildTooltip)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddSignalEventDelegate(SignalName.NodeTypeSelected, backing_NodeTypeSelected);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetSignalEventDelegate<NodeTypeSelectedEventHandler>(SignalName.NodeTypeSelected, out var value))
		{
			backing_NodeTypeSelected = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.NodeTypeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalNodeTypeSelected(string className)
	{
		EmitSignal(SignalName.NodeTypeSelected, new ReadOnlySpan<Variant>((Variant)className));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.NodeTypeSelected && args.Count == 1)
		{
			backing_NodeTypeSelected?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.NodeTypeSelected)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
