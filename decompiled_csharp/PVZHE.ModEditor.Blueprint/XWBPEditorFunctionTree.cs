using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Layout;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/GUI/ScriptItemTree/Function/XWBPEditorFunctionTree.cs")]
public class XWBPEditorFunctionTree : XWBPScriptItemTreeBase
{
	public new class MethodName : XWBPScriptItemTreeBase.MethodName
	{
		public new static readonly StringName CreateMenu = "CreateMenu";

		public new static readonly StringName OnItemMouseSelected = "OnItemMouseSelected";

		public new static readonly StringName OnItemActivated = "OnItemActivated";

		public new static readonly StringName OnItemEdited = "OnItemEdited";

		public new static readonly StringName _GetDragData = "_GetDragData";

		public new static readonly StringName OnMenuIdPressed = "OnMenuIdPressed";

		public static readonly StringName Open = "Open";

		public new static readonly StringName Duplicate = "Duplicate";

		public static readonly StringName Rename = "Rename";

		public static readonly StringName Remove = "Remove";
	}

	public new class PropertyName : XWBPScriptItemTreeBase.PropertyName
	{
	}

	public new class SignalName : XWBPScriptItemTreeBase.SignalName
	{
	}

	private static Texture2D _memberMethodIcon;

	private static Texture2D MemberMethodIcon => _memberMethodIcon ?? (_memberMethodIcon = XWBPScriptItemTreeBase.LoadScriptItemIcon("MemberMethod.svg"));

	public void Init(IEnumerable<XWBPFunctionData> functionDataList)
	{
		Clear();
		_root = CreateItem();
		foreach (XWBPFunctionData functionData in functionDataList)
		{
			TreeItem treeItem = _root.CreateChild();
			treeItem.SetText(0, functionData.Name);
			treeItem.SetIcon(0, MemberMethodIcon);
			treeItem.SetMetadata(0, functionData);
		}
		UpdateMinimumHeight();
	}

	protected override void CreateMenu()
	{
		base.CreateMenu();
		AddScriptItemMenuItem("打开", 1, "Load.svg");
		AddScriptItemMenuItem("复制", 5, "ActionCopy.svg");
		AddScriptItemMenuItem("重命名", 10, "Rename.svg");
		AddScriptItemMenuItem("移除", 20, "Remove.svg");
	}

	protected override void OnItemMouseSelected(Vector2 mousePosition, long mouseButtonIndex)
	{
		TreeItem selected = GetSelected();
		if (selected == null)
		{
			return;
		}
		Variant metadata = selected.GetMetadata(0);
		switch (mouseButtonIndex)
		{
		case 1L:
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPFunctionData target)
			{
				Editor?.EditBlueprintObject(target);
			}
			break;
		case 2L:
			PopMenu(mousePosition);
			break;
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
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPFunctionData function)
			{
				Editor?.RenameFunctionWithUndo(function, text);
			}
		}
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		return BuildDragDataForSelectedItem(atPosition, XWDragData.Type.BpFunction);
	}

	protected override void OnMenuIdPressed(long id)
	{
		switch (id)
		{
		case 1L:
			Open();
			break;
		case 5L:
			Duplicate();
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
		if (selected != null)
		{
			Variant metadata = selected.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPFunctionData graphData)
			{
				Editor?.GetGraphEditor()?.Init(graphData);
			}
			DeselectAll();
		}
	}

	private void Duplicate()
	{
		TreeItem selected = GetSelected();
		if (selected != null)
		{
			Variant metadata = selected.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPFunctionData function)
			{
				Editor?.DuplicateFunctionWithUndo(function);
			}
		}
	}

	private void Rename()
	{
		TreeItem selected = GetSelected();
		if (selected != null)
		{
			Variant metadata = selected.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPFunctionData)
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
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPFunctionData function)
			{
				Editor?.RemoveFunctionWithUndo(function);
			}
		}
	}

	private IEnumerable<XWBPFunctionData> GetAllFunctions()
	{
		if (Editor?.BpScriptData == null)
		{
			yield break;
		}
		foreach (KeyValuePair<int, XWBPFunctionData> function in Editor.BpScriptData.Functions)
		{
			yield return function.Value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName.CreateMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemMouseSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "mousePosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "mouseButtonIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnItemActivated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetDragData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnMenuIdPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Open, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Duplicate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Rename, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Remove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName._GetDragData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_GetDragData(VariantUtils.ConvertTo<Vector2>(in args[0])));
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
		if (method == MethodName.Duplicate && args.Count == 0)
		{
			Duplicate();
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
		if (method == MethodName._GetDragData)
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
		if (method == MethodName.Duplicate)
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
