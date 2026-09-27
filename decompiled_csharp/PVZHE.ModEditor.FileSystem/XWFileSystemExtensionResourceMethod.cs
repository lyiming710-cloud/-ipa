using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/RefCounted/Extension/XWFileSystemExtensionResourceMethod.cs")]
public class XWFileSystemExtensionResourceMethod : XWFileSystemExtensionMethod
{
	public new class MethodName : XWFileSystemExtensionMethod.MethodName
	{
		public new static readonly StringName GetIcon = "GetIcon";
	}

	public new class PropertyName : XWFileSystemExtensionMethod.PropertyName
	{
	}

	public new class SignalName : XWFileSystemExtensionMethod.SignalName
	{
	}

	public override Texture2D GetIcon(XWFileSystemTreeItemData data)
	{
		return XWFileSystemExtensionMethod.LoadIcon("res://addons/ModEditor/Icons/Object.svg");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.GetIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetIcon)
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
