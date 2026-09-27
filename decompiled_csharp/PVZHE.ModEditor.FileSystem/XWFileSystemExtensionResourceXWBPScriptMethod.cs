using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Blueprint;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/Extension/Resource/XWFileSystemExtensionResourceXWBPScriptMethod.cs")]
public class XWFileSystemExtensionResourceXWBPScriptMethod : XWFileSystemExtensionResourceMethod
{
	public new class MethodName : XWFileSystemExtensionResourceMethod.MethodName
	{
		public new static readonly StringName GetIcon = "GetIcon";

		public new static readonly StringName Execute = "Execute";

		public static readonly StringName LoadBlueprint = "LoadBlueprint";
	}

	public new class PropertyName : XWFileSystemExtensionResourceMethod.PropertyName
	{
	}

	public new class SignalName : XWFileSystemExtensionResourceMethod.SignalName
	{
	}

	public override Texture2D GetIcon(XWFileSystemTreeItemData data)
	{
		return XWClassRegistry.Instance.GetUIIcon("GraphEdit");
	}

	public override void Execute(XWFileSystemTreeItemData data)
	{
		XWBPScript xWBPScript = LoadBlueprint(data);
		if (xWBPScript == null || !GodotObject.IsInstanceValid(xWBPScript))
		{
			XWEditorInterface.Instance?.ShowToast("无法加载蓝图: " + data?.Path);
		}
		else if (XWEditorInterface.Instance?.GetBlueprintEditor() is XWBPEditor xWBPEditor)
		{
			xWBPEditor.Init(xWBPScript);
			XWEditorInterface.Instance?.FocusPanel("bp_editor");
		}
		else
		{
			XWEditorInterface.Instance?.ShowToast("蓝图编辑器未就绪");
		}
	}

	private static XWBPScript LoadBlueprint(XWFileSystemTreeItemData data)
	{
		if (data == null)
		{
			return null;
		}
		if (data.Data.VariantType == Variant.Type.Object && data.Data.As<GodotObject>() is XWBPScript result)
		{
			return result;
		}
		if (!string.IsNullOrEmpty(data.Path) && ResourceLoader.Exists(data.Path))
		{
			return ResourceLoader.Load<XWBPScript>(data.Path, null, ResourceLoader.CacheMode.Reuse);
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.GetIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadBlueprint, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetIcon(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0])));
			return true;
		}
		if (method == MethodName.Execute && args.Count == 1)
		{
			Execute(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadBlueprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPScript>(LoadBlueprint(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadBlueprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPScript>(LoadBlueprint(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetIcon)
		{
			return true;
		}
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.LoadBlueprint)
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
