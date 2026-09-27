using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Registry.Class;
using PVZHE.ModEditor.ResourceEditors;
using PVZHE.ModEditor.ScriptEditor;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/Extension/Script/XWFileSystemExtensionScriptMethod.cs")]
public class XWFileSystemExtensionScriptMethod : XWFileSystemExtensionMethod
{
	public new class MethodName : XWFileSystemExtensionMethod.MethodName
	{
		public new static readonly StringName GetIcon = "GetIcon";

		public new static readonly StringName Execute = "Execute";
	}

	public new class PropertyName : XWFileSystemExtensionMethod.PropertyName
	{
	}

	public new class SignalName : XWFileSystemExtensionMethod.SignalName
	{
	}

	public override Texture2D GetIcon(XWFileSystemTreeItemData data)
	{
		if ((data?.GetExtension() ?? "") == "cs")
		{
			return XWClassRegistry.Instance.GetUIIcon("Script");
		}
		return XWClassRegistry.Instance.GetUIIcon("File");
	}

	public override void Execute(XWFileSystemTreeItemData data)
	{
		if (data == null || string.IsNullOrEmpty(data.Path))
		{
			return;
		}
		if (!XWScriptEditor.IsSupportedScriptPath(data.Path))
		{
			if (!XWResourceEditorRegistry.TryOpenPath(data.Path))
			{
				XWEditorInterface.Instance?.ShowToast("脚本编辑器仅支持 C#；该文件没有可用的可视化编辑入口。", 2);
			}
			return;
		}
		XWScriptEditor xWScriptEditor = XWEditorInterface.Instance?.GetScriptEditor();
		if (xWScriptEditor == null || !GodotObject.IsInstanceValid(xWScriptEditor))
		{
			XWEditorInterface.Instance?.ShowToast("脚本编辑器未就绪");
			return;
		}
		xWScriptEditor.OpenFile(data.Path);
		XWEditorInterface.Instance?.FocusPanel("script_editor");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.GetIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
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
