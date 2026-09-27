using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Layout;
using PVZHE.ModEditor.Registry;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/GUI/ScriptItemTree/Variable/XWBPEditorVariableTree.cs")]
public class XWBPEditorVariableTree : XWBPScriptItemTreeBase
{
	public new class MethodName : XWBPScriptItemTreeBase.MethodName
	{
		public static readonly StringName ApplyIcon = "ApplyIcon";

		public static readonly StringName OnVariableChanged = "OnVariableChanged";

		public new static readonly StringName CreateMenu = "CreateMenu";

		public new static readonly StringName OnItemMouseSelected = "OnItemMouseSelected";

		public new static readonly StringName OnItemEdited = "OnItemEdited";

		public new static readonly StringName _GetDragData = "_GetDragData";

		public new static readonly StringName OnMenuIdPressed = "OnMenuIdPressed";

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

	private List<XWBPVariableData> _variableDataList = new List<XWBPVariableData>();

	public void Init(IEnumerable<XWBPVariableData> variableDataList)
	{
		Clear();
		_variableDataList = new List<XWBPVariableData>(variableDataList);
		_root = CreateItem();
		foreach (XWBPVariableData variableData in _variableDataList)
		{
			TreeItem treeItem = _root.CreateChild();
			treeItem.SetText(0, variableData.Name);
			ApplyIcon(treeItem, variableData);
			treeItem.SetMetadata(0, variableData);
			variableData.Change += () =>
			{
				OnVariableChanged(treeItem, variableData);
			};
		}
		UpdateMinimumHeight();
	}

	private static void ApplyIcon(TreeItem treeItem, XWBPVariableData variableData)
	{
		if (treeItem != null && variableData != null)
		{
			Texture2D texture2D = null;
			if (variableData.Type == Variant.Type.Object && !string.IsNullOrWhiteSpace(variableData.ClassName))
			{
				texture2D = XWClassRegistry.Instance.GetClassIcon(variableData.ClassName);
			}
			if (texture2D == null && XWTypeRegistry.Instance.HasType(variableData.Type))
			{
				texture2D = XWTypeRegistry.Instance.GetTypeIcon(variableData.Type);
			}
			if (texture2D != null)
			{
				treeItem.SetIcon(0, texture2D);
			}
		}
	}

	private void OnVariableChanged(TreeItem treeItem, XWBPVariableData variableData)
	{
		if (GodotObject.IsInstanceValid(treeItem))
		{
			ApplyIcon(treeItem, variableData);
		}
	}

	protected override void CreateMenu()
	{
		base.CreateMenu();
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
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPVariableData target)
			{
				Editor?.EditBlueprintObject(target);
			}
			break;
		case 2L:
			PopMenu(mousePosition);
			break;
		}
	}

	protected override void OnItemEdited()
	{
		TreeItem edited = GetEdited();
		if (edited != null)
		{
			string text = edited.GetText(0);
			Variant metadata = edited.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPVariableData variable)
			{
				Editor?.RenameVariableWithUndo(variable, text);
			}
		}
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		return BuildDragDataForSelectedItem(atPosition, XWDragData.Type.BpVariable);
	}

	protected override void OnMenuIdPressed(long id)
	{
		switch (id)
		{
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

	private void Duplicate()
	{
		TreeItem selected = GetSelected();
		if (selected != null)
		{
			Variant metadata = selected.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPVariableData variable)
			{
				Editor?.DuplicateVariableWithUndo(variable);
			}
		}
	}

	private void Rename()
	{
		TreeItem selected = GetSelected();
		if (selected != null)
		{
			Variant metadata = selected.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPVariableData)
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
			if (metadata.VariantType == Variant.Type.Object && metadata.As<RefCounted>() is XWBPVariableData variable)
			{
				Editor?.RemoveVariableWithUndo(variable);
			}
		}
	}

	private IEnumerable<XWBPVariableData> GetAllVariables()
	{
		if (Editor?.BpScriptData == null)
		{
			yield break;
		}
		foreach (KeyValuePair<int, XWBPVariableData> variable in Editor.BpScriptData.Variables)
		{
			yield return variable.Value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName.ApplyIcon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "treeItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Object, "variableData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnVariableChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "treeItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Object, "variableData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemMouseSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "mousePosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "mouseButtonIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnItemEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetDragData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnMenuIdPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Duplicate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Rename, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Remove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ApplyIcon && args.Count == 2)
		{
			ApplyIcon(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<XWBPVariableData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnVariableChanged && args.Count == 2)
		{
			OnVariableChanged(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<XWBPVariableData>(in args[1]));
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
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ApplyIcon && args.Count == 2)
		{
			ApplyIcon(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<XWBPVariableData>(in args[1]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ApplyIcon)
		{
			return true;
		}
		if (method == MethodName.OnVariableChanged)
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
