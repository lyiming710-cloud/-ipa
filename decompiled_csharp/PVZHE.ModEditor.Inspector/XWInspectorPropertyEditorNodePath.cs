using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/NodePath/XWInspectorPropertyEditorNodePath.cs")]
public class XWInspectorPropertyEditorNodePath : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName OnTextChanged = "OnTextChanged";

		public static readonly StringName CommitTextNodePath = "CommitTextNodePath";

		public static readonly StringName ShowNodeSelectorWindow = "ShowNodeSelectorWindow";

		public static readonly StringName EnsureNodeSelectorWindow = "EnsureNodeSelectorWindow";

		public static readonly StringName BuildNodeSelectorTree = "BuildNodeSelectorTree";

		public static readonly StringName AppendNodeTreeItem = "AppendNodeTreeItem";

		public static readonly StringName FilterNodePathTree = "FilterNodePathTree";

		public static readonly StringName SetVisibleRecursive = "SetVisibleRecursive";

		public static readonly StringName MatchesFilter = "MatchesFilter";

		public static readonly StringName UpdateSelectedNodePathPreview = "UpdateSelectedNodePathPreview";

		public static readonly StringName CommitSelectedNodePath = "CommitSelectedNodePath";

		public static readonly StringName ClearNodePath = "ClearNodePath";

		public static readonly StringName BuildRelativeNodePath = "BuildRelativeNodePath";

		public static readonly StringName ResolvePathBaseNode = "ResolvePathBaseNode";

		public static readonly StringName ResolveNodeSelectorRoot = "ResolveNodeSelectorRoot";

		public static readonly StringName IsNodeInSameTree = "IsNodeInSameTree";

		public static readonly StringName IsNodeValid = "IsNodeValid";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _lineEdit = "_lineEdit";

		public static readonly StringName _selectButton = "_selectButton";

		public static readonly StringName _nodePathSelectorWindow = "_nodePathSelectorWindow";

		public static readonly StringName _nodePathFilter = "_nodePathFilter";

		public static readonly StringName _nodePathTree = "_nodePathTree";

		public static readonly StringName _nodePathPreviewLabel = "_nodePathPreviewLabel";

		public static readonly StringName _confirmButton = "_confirmButton";

		public static readonly StringName _clearButton = "_clearButton";

		public static readonly StringName _cancelButton = "_cancelButton";

		public static readonly StringName _selectorRoot = "_selectorRoot";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string EditorScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/NodePath/XWInspectorPropertyEditorNodePath.tscn";

	private const string NodePathSelectorWindowScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/NodePath/XWNodePathSelectorWindow.tscn";

	private static readonly Texture2D SelectNodeIcon = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ClassIcon/CopyNodePath.svg", null, ResourceLoader.CacheMode.Reuse));

	private static PackedScene _nodePathSelectorWindowScene;

	private static PackedScene _editorScene;

	private LineEdit _lineEdit;

	private Button _selectButton;

	private Window _nodePathSelectorWindow;

	private LineEdit _nodePathFilter;

	private Tree _nodePathTree;

	private Label _nodePathPreviewLabel;

	private Button _confirmButton;

	private Button _clearButton;

	private Button _cancelButton;

	private Node _selectorRoot;

	private readonly System.Collections.Generic.Dictionary<TreeItem, Node> _treeItemToNode = new System.Collections.Generic.Dictionary<TreeItem, Node>();

	public static XWInspectorPropertyEditorNodePath Create()
	{
		if (_editorScene == null)
		{
			_editorScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/NodePath/XWInspectorPropertyEditorNodePath.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		return _editorScene?.Instantiate<XWInspectorPropertyEditorNodePath>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_lineEdit = GetNode<LineEdit>("%LineEdit");
		_selectButton = GetNode<Button>("%SelectNodePathButton");
		_lineEdit.TextChanged += OnTextChanged;
		_lineEdit.TextSubmitted += CommitTextNodePath;
		_lineEdit.FocusExited += () =>
		{
			CommitTextNodePath(_lineEdit.Text);
		};
		_selectButton.Pressed += ShowNodeSelectorWindow;
		_selectButton.Icon = SelectNodeIcon;
		_selectButton.TooltipText = "选择节点";
		_selectButton.CustomMinimumSize = new Vector2(30f, 28f);
		_selectButton.Flat = true;
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
		return new NodePath(_lineEdit.Text);
	}

	private void OnTextChanged(string text)
	{
		ValueChange(Variant.From<NodePath>(new NodePath(text)));
	}

	private void CommitTextNodePath(string text)
	{
		_lineEdit.Text = text ?? "";
		ValueChange(Variant.From<NodePath>(new NodePath(_lineEdit.Text)));
	}

	private void ShowNodeSelectorWindow()
	{
		if (EnsureNodeSelectorWindow())
		{
			_selectorRoot = ResolveNodeSelectorRoot();
			if (!IsNodeValid(_selectorRoot))
			{
				XWEditorInterface.Instance?.ShowToast("当前没有可选择的场景节点");
				return;
			}
			_nodePathFilter.Text = "";
			BuildNodeSelectorTree();
			_nodePathSelectorWindow.PopupCentered();
			_nodePathFilter.GrabFocus();
		}
	}

	private bool EnsureNodeSelectorWindow()
	{
		if (GodotObject.IsInstanceValid(_nodePathSelectorWindow))
		{
			return true;
		}
		if (_nodePathSelectorWindowScene == null)
		{
			_nodePathSelectorWindowScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/NodePath/XWNodePathSelectorWindow.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		_nodePathSelectorWindow = _nodePathSelectorWindowScene?.Instantiate<Window>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(_nodePathSelectorWindow))
		{
			XWEditorInterface.Instance?.ShowToast("无法加载节点路径选择窗口");
			return false;
		}
		_nodePathFilter = _nodePathSelectorWindow.GetNode<LineEdit>("%NodePathSelectorFilter");
		_nodePathTree = _nodePathSelectorWindow.GetNode<Tree>("%NodePathSelectorTree");
		_nodePathPreviewLabel = _nodePathSelectorWindow.GetNode<Label>("%NodePathPreviewLabel");
		_clearButton = _nodePathSelectorWindow.GetNode<Button>("%ClearNodePath");
		_cancelButton = _nodePathSelectorWindow.GetNode<Button>("%CancelNodePathSelection");
		_confirmButton = _nodePathSelectorWindow.GetNode<Button>("%ConfirmNodePathSelection");
		_nodePathSelectorWindow.CloseRequested += () =>
		{
			_nodePathSelectorWindow.Hide();
		};
		_nodePathFilter.TextChanged += FilterNodePathTree;
		_nodePathTree.ItemSelected += UpdateSelectedNodePathPreview;
		_nodePathTree.ItemActivated += CommitSelectedNodePath;
		_clearButton.Pressed += ClearNodePath;
		_cancelButton.Pressed += () =>
		{
			_nodePathSelectorWindow.Hide();
		};
		_confirmButton.Pressed += CommitSelectedNodePath;
		AddChild(_nodePathSelectorWindow, forceReadableName: false, InternalMode.Disabled);
		return true;
	}

	private void BuildNodeSelectorTree()
	{
		if (GodotObject.IsInstanceValid(_nodePathTree))
		{
			_nodePathTree.Clear();
			_treeItemToNode.Clear();
			if (IsNodeValid(_selectorRoot))
			{
				TreeItem treeItem = _nodePathTree.CreateItem();
				AppendNodeTreeItem(treeItem, _selectorRoot);
				treeItem.Collapsed = false;
				treeItem.Select(0);
				UpdateSelectedNodePathPreview();
			}
		}
	}

	private void AppendNodeTreeItem(TreeItem item, Node node)
	{
		if (!GodotObject.IsInstanceValid(item) || !IsNodeValid(node))
		{
			return;
		}
		item.SetText(0, node.Name);
		item.SetIcon(0, XWClassRegistry.Instance.GetClassIcon(node.GetClass()));
		item.SetTooltipText(0, node.GetPath().ToString());
		item.SetSelectable(0, selectable: true);
		_treeItemToNode[item] = node;
		foreach (Node child in node.GetChildren())
		{
			if (child != null)
			{
				Node node2 = child;
				TreeItem item2 = item.CreateChild();
				AppendNodeTreeItem(item2, node2);
			}
		}
	}

	private void FilterNodePathTree(string query)
	{
		if (!GodotObject.IsInstanceValid(_nodePathTree))
		{
			return;
		}
		TreeItem root = _nodePathTree.GetRoot();
		if (GodotObject.IsInstanceValid(root))
		{
			string text = (query ?? "").Trim().ToLowerInvariant();
			SetVisibleRecursive(root, text);
			root.Visible = true;
			if (string.IsNullOrEmpty(text))
			{
				root.Collapsed = false;
			}
		}
	}

	private bool SetVisibleRecursive(TreeItem item, string query)
	{
		if (!GodotObject.IsInstanceValid(item))
		{
			return false;
		}
		bool flag = string.IsNullOrEmpty(query) || MatchesFilter(item, query);
		bool flag2 = false;
		TreeItem treeItem = item.GetFirstChild();
		while (GodotObject.IsInstanceValid(treeItem))
		{
			flag2 |= SetVisibleRecursive(treeItem, query);
			treeItem = treeItem.GetNext();
		}
		bool result = (item.Visible = flag | flag2);
		if (!string.IsNullOrEmpty(query) & flag2)
		{
			item.Collapsed = false;
		}
		return result;
	}

	private bool MatchesFilter(TreeItem item, string query)
	{
		if (!GodotObject.IsInstanceValid(item) || string.IsNullOrEmpty(query))
		{
			return true;
		}
		if ((item.GetText(0) ?? "").ToLowerInvariant().Contains(query))
		{
			return true;
		}
		if (_treeItemToNode.TryGetValue(item, out var value) && IsNodeValid(value))
		{
			return value.GetPath().ToString().ToLowerInvariant()
				.Contains(query);
		}
		return false;
	}

	private void UpdateSelectedNodePathPreview()
	{
		if (GodotObject.IsInstanceValid(_nodePathPreviewLabel) && GodotObject.IsInstanceValid(_nodePathTree))
		{
			TreeItem selected = _nodePathTree.GetSelected();
			if (!GodotObject.IsInstanceValid(selected) || !_treeItemToNode.TryGetValue(selected, out var value) || !IsNodeValid(value))
			{
				_nodePathPreviewLabel.Text = "";
			}
			else
			{
				_nodePathPreviewLabel.Text = BuildRelativeNodePath(value);
			}
		}
	}

	private void CommitSelectedNodePath()
	{
		if (GodotObject.IsInstanceValid(_nodePathTree))
		{
			TreeItem selected = _nodePathTree.GetSelected();
			if (GodotObject.IsInstanceValid(selected) && _treeItemToNode.TryGetValue(selected, out var value) && IsNodeValid(value))
			{
				string text = BuildRelativeNodePath(value);
				_lineEdit.Text = text;
				ValueChange(Variant.From<NodePath>(new NodePath(text)));
				XWEditorInterface.Instance?.SetSelectedNode(value);
				_nodePathSelectorWindow?.Hide();
			}
		}
	}

	private void ClearNodePath()
	{
		_lineEdit.Text = "";
		ValueChange(Variant.From<NodePath>(new NodePath("")));
		_nodePathSelectorWindow?.Hide();
	}

	private string BuildRelativeNodePath(Node target)
	{
		if (!IsNodeValid(target))
		{
			return "";
		}
		Node node = ResolvePathBaseNode();
		if (IsNodeValid(node) && IsNodeInSameTree(node, target))
		{
			if (node == target)
			{
				return ".";
			}
			return node.GetPathTo(target).ToString();
		}
		if (IsNodeValid(_selectorRoot) && IsNodeInSameTree(_selectorRoot, target))
		{
			if (_selectorRoot == target)
			{
				return ".";
			}
			return _selectorRoot.GetPathTo(target).ToString();
		}
		return target.GetPath().ToString();
	}

	private Node ResolvePathBaseNode()
	{
		if (GodotObject.IsInstanceValid(Property) && Property.Object is Node node && IsNodeValid(node))
		{
			return node;
		}
		Array<Node> array = XWEditorInterface.Instance?.GetSelectedNodes();
		if (array != null && array.Count > 0)
		{
			Node node2 = array[0];
			if (IsNodeValid(node2))
			{
				return node2;
			}
		}
		return XWEditorInterface.Instance?.GetEditedSceneRoot();
	}

	private Node ResolveNodeSelectorRoot()
	{
		Node node = XWEditorInterface.Instance?.GetEditedSceneRoot();
		if (IsNodeValid(node))
		{
			return node;
		}
		Node node2 = ResolvePathBaseNode();
		if (!IsNodeValid(node2))
		{
			return null;
		}
		while (IsNodeValid(node2.GetParent()))
		{
			node2 = node2.GetParent();
		}
		return node2;
	}

	private static bool IsNodeInSameTree(Node a, Node b)
	{
		if (!IsNodeValid(a) || !IsNodeValid(b))
		{
			return false;
		}
		return a.GetTree() == b.GetTree();
	}

	private static bool IsNodeValid(Node node)
	{
		return GodotObject.IsInstanceValid(node);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTextChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitTextNodePath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowNodeSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureNodeSelectorWindow, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildNodeSelectorTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AppendNodeTreeItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FilterNodePathTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetVisibleRecursive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MatchesFilter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateSelectedNodePathPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitSelectedNodePath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearNodePath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildRelativeNodePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePathBaseNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveNodeSelectorRoot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsNodeInSameTree, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "a", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "b", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsNodeValid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorNodePath>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
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
		if (method == MethodName.OnTextChanged && args.Count == 1)
		{
			OnTextChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitTextNodePath && args.Count == 1)
		{
			CommitTextNodePath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowNodeSelectorWindow && args.Count == 0)
		{
			ShowNodeSelectorWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureNodeSelectorWindow && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureNodeSelectorWindow());
			return true;
		}
		if (method == MethodName.BuildNodeSelectorTree && args.Count == 0)
		{
			BuildNodeSelectorTree();
			ret = default;
			return true;
		}
		if (method == MethodName.AppendNodeTreeItem && args.Count == 2)
		{
			AppendNodeTreeItem(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FilterNodePathTree && args.Count == 1)
		{
			FilterNodePathTree(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetVisibleRecursive && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetVisibleRecursive(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MatchesFilter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(MatchesFilter(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.UpdateSelectedNodePathPreview && args.Count == 0)
		{
			UpdateSelectedNodePathPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.CommitSelectedNodePath && args.Count == 0)
		{
			CommitSelectedNodePath();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearNodePath && args.Count == 0)
		{
			ClearNodePath();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRelativeNodePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildRelativeNodePath(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolvePathBaseNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(ResolvePathBaseNode());
			return true;
		}
		if (method == MethodName.ResolveNodeSelectorRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(ResolveNodeSelectorRoot());
			return true;
		}
		if (method == MethodName.IsNodeInSameTree && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNodeInSameTree(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.IsNodeValid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNodeValid(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorNodePath>(Create());
			return true;
		}
		if (method == MethodName.IsNodeInSameTree && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNodeInSameTree(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.IsNodeValid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNodeValid(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.OnTextChanged)
		{
			return true;
		}
		if (method == MethodName.CommitTextNodePath)
		{
			return true;
		}
		if (method == MethodName.ShowNodeSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.EnsureNodeSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.BuildNodeSelectorTree)
		{
			return true;
		}
		if (method == MethodName.AppendNodeTreeItem)
		{
			return true;
		}
		if (method == MethodName.FilterNodePathTree)
		{
			return true;
		}
		if (method == MethodName.SetVisibleRecursive)
		{
			return true;
		}
		if (method == MethodName.MatchesFilter)
		{
			return true;
		}
		if (method == MethodName.UpdateSelectedNodePathPreview)
		{
			return true;
		}
		if (method == MethodName.CommitSelectedNodePath)
		{
			return true;
		}
		if (method == MethodName.ClearNodePath)
		{
			return true;
		}
		if (method == MethodName.BuildRelativeNodePath)
		{
			return true;
		}
		if (method == MethodName.ResolvePathBaseNode)
		{
			return true;
		}
		if (method == MethodName.ResolveNodeSelectorRoot)
		{
			return true;
		}
		if (method == MethodName.IsNodeInSameTree)
		{
			return true;
		}
		if (method == MethodName.IsNodeValid)
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
		if (name == PropertyName._selectButton)
		{
			_selectButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._nodePathSelectorWindow)
		{
			_nodePathSelectorWindow = VariantUtils.ConvertTo<Window>(in value);
			return true;
		}
		if (name == PropertyName._nodePathFilter)
		{
			_nodePathFilter = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._nodePathTree)
		{
			_nodePathTree = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._nodePathPreviewLabel)
		{
			_nodePathPreviewLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._confirmButton)
		{
			_confirmButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._clearButton)
		{
			_clearButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._cancelButton)
		{
			_cancelButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._selectorRoot)
		{
			_selectorRoot = VariantUtils.ConvertTo<Node>(in value);
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
		if (name == PropertyName._selectButton)
		{
			value = VariantUtils.CreateFrom(in _selectButton);
			return true;
		}
		if (name == PropertyName._nodePathSelectorWindow)
		{
			value = VariantUtils.CreateFrom(in _nodePathSelectorWindow);
			return true;
		}
		if (name == PropertyName._nodePathFilter)
		{
			value = VariantUtils.CreateFrom(in _nodePathFilter);
			return true;
		}
		if (name == PropertyName._nodePathTree)
		{
			value = VariantUtils.CreateFrom(in _nodePathTree);
			return true;
		}
		if (name == PropertyName._nodePathPreviewLabel)
		{
			value = VariantUtils.CreateFrom(in _nodePathPreviewLabel);
			return true;
		}
		if (name == PropertyName._confirmButton)
		{
			value = VariantUtils.CreateFrom(in _confirmButton);
			return true;
		}
		if (name == PropertyName._clearButton)
		{
			value = VariantUtils.CreateFrom(in _clearButton);
			return true;
		}
		if (name == PropertyName._cancelButton)
		{
			value = VariantUtils.CreateFrom(in _cancelButton);
			return true;
		}
		if (name == PropertyName._selectorRoot)
		{
			value = VariantUtils.CreateFrom(in _selectorRoot);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._selectButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nodePathSelectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nodePathFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nodePathTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nodePathPreviewLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._confirmButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clearButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cancelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectorRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._lineEdit, Variant.From(in _lineEdit));
		info.AddProperty(PropertyName._selectButton, Variant.From(in _selectButton));
		info.AddProperty(PropertyName._nodePathSelectorWindow, Variant.From(in _nodePathSelectorWindow));
		info.AddProperty(PropertyName._nodePathFilter, Variant.From(in _nodePathFilter));
		info.AddProperty(PropertyName._nodePathTree, Variant.From(in _nodePathTree));
		info.AddProperty(PropertyName._nodePathPreviewLabel, Variant.From(in _nodePathPreviewLabel));
		info.AddProperty(PropertyName._confirmButton, Variant.From(in _confirmButton));
		info.AddProperty(PropertyName._clearButton, Variant.From(in _clearButton));
		info.AddProperty(PropertyName._cancelButton, Variant.From(in _cancelButton));
		info.AddProperty(PropertyName._selectorRoot, Variant.From(in _selectorRoot));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._lineEdit, out var value))
		{
			_lineEdit = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._selectButton, out var value2))
		{
			_selectButton = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nodePathSelectorWindow, out var value3))
		{
			_nodePathSelectorWindow = value3.As<Window>();
		}
		if (info.TryGetProperty(PropertyName._nodePathFilter, out var value4))
		{
			_nodePathFilter = value4.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._nodePathTree, out var value5))
		{
			_nodePathTree = value5.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._nodePathPreviewLabel, out var value6))
		{
			_nodePathPreviewLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._confirmButton, out var value7))
		{
			_confirmButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._clearButton, out var value8))
		{
			_clearButton = value8.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._cancelButton, out var value9))
		{
			_cancelButton = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._selectorRoot, out var value10))
		{
			_selectorRoot = value10.As<Node>();
		}
	}
}
