using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/GUI/ScriptItemTree/Graph/XWBPEditorGraphTree.cs")]
public class XWBPEditorGraphTree : XWBPScriptItemTreeBase
{
	public new class MethodName : XWBPScriptItemTreeBase.MethodName
	{
		public static readonly StringName BuildVirtualMethodItems = "BuildVirtualMethodItems";

		public static readonly StringName RefreshVirtualMethods = "RefreshVirtualMethods";

		public new static readonly StringName CreateMenu = "CreateMenu";

		public new static readonly StringName OnItemMouseSelected = "OnItemMouseSelected";

		public new static readonly StringName OnItemActivated = "OnItemActivated";

		public new static readonly StringName OnItemEdited = "OnItemEdited";

		public new static readonly StringName OnMenuIdPressed = "OnMenuIdPressed";

		public static readonly StringName Open = "Open";

		public static readonly StringName Rename = "Rename";

		public static readonly StringName Remove = "Remove";
	}

	public new class PropertyName : XWBPScriptItemTreeBase.PropertyName
	{
	}

	public new class SignalName : XWBPScriptItemTreeBase.SignalName
	{
	}

	private const string MethodEntryTypeId = "__XWBPGraphNode_MethodEntry";

	private static Texture2D _filesystemIcon;

	private static Texture2D _memberMethodIcon;

	private static Texture2D FilesystemIcon => _filesystemIcon ?? (_filesystemIcon = XWBPScriptItemTreeBase.LoadScriptItemIcon("Filesystem.svg"));

	private static Texture2D MemberMethodIcon => _memberMethodIcon ?? (_memberMethodIcon = XWBPScriptItemTreeBase.LoadScriptItemIcon("MemberMethod.svg"));

	public void Init(IEnumerable<XWBPGraphData> graphDataList)
	{
		Clear();
		_root = CreateItem();
		foreach (XWBPGraphData graphData in graphDataList)
		{
			TreeItem treeItem = _root.CreateChild();
			treeItem.SetText(0, graphData.Name);
			treeItem.SetIcon(0, FilesystemIcon);
			treeItem.SetMetadata(0, graphData);
			BuildVirtualMethodItems(treeItem, graphData);
		}
		UpdateMinimumHeight();
	}

	private void BuildVirtualMethodItems(TreeItem parentItem, XWBPGraphData graphData)
	{
		foreach (KeyValuePair<int, XWBPNodeData> node in graphData.Nodes)
		{
			XWBPNodeData value = node.Value;
			if (!(value.TypeId.ToString() != "__XWBPGraphNode_MethodEntry") && (int)value.GetMetaData("MethodType").AsInt64() == 2)
			{
				string text = value.GetMetaData("MethodData").AsGodotDictionary().GetValueOrDefault("name", Variant.From<string>(""))
					.AsString();
				TreeItem treeItem = parentItem.CreateChild();
				treeItem.SetText(0, text);
				treeItem.SetIcon(0, MemberMethodIcon);
				treeItem.SetMetadata(0, new Dictionary
				{
					{ "graphData", graphData },
					{ "nodeId", value.Id }
				});
			}
		}
	}

	public void RefreshVirtualMethods(XWBPGraphData graphData)
	{
		if (_root == null)
		{
			return;
		}
		for (int i = 0; i < _root.GetChildCount(); i++)
		{
			TreeItem child = _root.GetChild(i);
			Variant metadata = child.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() == graphData)
			{
				while (child.GetChildCount() > 0)
				{
					child.GetChild(0).Free();
				}
				BuildVirtualMethodItems(child, graphData);
				break;
			}
		}
	}

	protected override void CreateMenu()
	{
		base.CreateMenu();
		AddScriptItemMenuItem("打开", 1, "Load.svg");
		AddScriptItemMenuItem("重命名", 10, "Rename.svg");
		AddScriptItemMenuItem("移除", 20, "Remove.svg");
	}

	protected override void OnItemMouseSelected(Vector2 mousePosition, long mouseButtonIndex)
	{
		TreeItem selected = GetSelected();
		if (mouseButtonIndex == 1 && selected != null)
		{
			Variant metadata = selected.GetMetadata(0);
			GodotObject godotObject = null;
			if (metadata.VariantType == Variant.Type.Object)
			{
				godotObject = metadata.As<GodotObject>();
			}
			else if (metadata.VariantType == Variant.Type.Dictionary)
			{
				Dictionary dictionary = metadata.AsGodotDictionary();
				XWBPGraphData xWBPGraphData = dictionary.GetValueOrDefault("graphData").As<XWBPGraphData>();
				int nodeId = dictionary.GetValueOrDefault("nodeId", -1).AsInt32();
				godotObject = (GodotObject)(((object)xWBPGraphData?.GetNode(nodeId)) ?? ((object)xWBPGraphData));
			}
			if (GodotObject.IsInstanceValid(godotObject))
			{
				Editor?.EditBlueprintObject(godotObject);
			}
		}
		else if (mouseButtonIndex == 2)
		{
			selected = SelectItemAtPosition(mousePosition);
			if (selected != null)
			{
				bool disabled = selected.GetMetadata(0).VariantType == Variant.Type.Dictionary;
				int itemIndex = _menu.GetItemIndex(20);
				_menu.SetItemDisabled(itemIndex, disabled);
				PopupMenuAtPosition(mousePosition);
			}
		}
	}

	protected override void OnItemActivated()
	{
		Open();
	}

	protected override void OnItemEdited()
	{
		TreeItem edited = GetEdited();
		if (edited != null)
		{
			string text = edited.GetText(0);
			Variant metadata = edited.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPGraphData graph)
			{
				Editor?.RenameGraphWithUndo(graph, text);
			}
		}
	}

	protected override void OnMenuIdPressed(long id)
	{
		switch (id)
		{
		case 1L:
			Open();
			break;
		case 10L:
			Rename();
			break;
		case 20L:
			Remove();
			break;
		}
	}

	private void Open()
	{
		TreeItem selected = GetSelected();
		if (selected == null)
		{
			return;
		}
		Variant metadata = selected.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPGraphData graphData)
		{
			Editor?.GetGraphEditor()?.Init(graphData);
		}
		else if (metadata.VariantType == Variant.Type.Dictionary)
		{
			Dictionary dictionary = metadata.AsGodotDictionary();
			XWBPGraphData xWBPGraphData = dictionary["graphData"].As<XWBPGraphData>();
			int num = (int)dictionary["nodeId"].AsInt64();
			if (xWBPGraphData != null && num >= 0)
			{
				XWBPGraphEdit xWBPGraphEdit = Editor?.GetGraphEditor();
				xWBPGraphEdit?.Init(xWBPGraphData);
				xWBPGraphEdit?.FocusNode(num);
			}
		}
		DeselectAll();
	}

	private void Rename()
	{
		TreeItem selected = GetSelected();
		if (selected != null)
		{
			Variant metadata = selected.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPGraphData)
			{
				EditSelected(forceEdit: true);
			}
		}
	}

	private void Remove()
	{
		TreeItem selected = GetSelected();
		if (selected != null)
		{
			Variant metadata = selected.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPGraphData graph)
			{
				Editor?.RemoveGraphWithUndo(graph);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName.BuildVirtualMethodItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parentItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Object, "graphData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshVirtualMethods, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graphData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemMouseSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "mousePosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "mouseButtonIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnItemActivated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMenuIdPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Open, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Rename, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Remove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BuildVirtualMethodItems && args.Count == 2)
		{
			BuildVirtualMethodItems(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<XWBPGraphData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshVirtualMethods && args.Count == 1)
		{
			RefreshVirtualMethods(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateMenu && args.Count == 0)
		{
			CreateMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemMouseSelected && args.Count == 2)
		{
			OnItemMouseSelected(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemActivated && args.Count == 0)
		{
			OnItemActivated();
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemEdited && args.Count == 0)
		{
			OnItemEdited();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMenuIdPressed && args.Count == 1)
		{
			OnMenuIdPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Open && args.Count == 0)
		{
			Open();
			ret = default;
			return true;
		}
		if (method == MethodName.Rename && args.Count == 0)
		{
			Rename();
			ret = default;
			return true;
		}
		if (method == MethodName.Remove && args.Count == 0)
		{
			Remove();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.BuildVirtualMethodItems)
		{
			return true;
		}
		if (method == MethodName.RefreshVirtualMethods)
		{
			return true;
		}
		if (method == MethodName.CreateMenu)
		{
			return true;
		}
		if (method == MethodName.OnItemMouseSelected)
		{
			return true;
		}
		if (method == MethodName.OnItemActivated)
		{
			return true;
		}
		if (method == MethodName.OnItemEdited)
		{
			return true;
		}
		if (method == MethodName.OnMenuIdPressed)
		{
			return true;
		}
		if (method == MethodName.Open)
		{
			return true;
		}
		if (method == MethodName.Rename)
		{
			return true;
		}
		if (method == MethodName.Remove)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
