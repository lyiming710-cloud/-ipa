using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;

namespace PVZHE.ModEditor.PVZIntegration;

public class DatExtensionMethod : XWFileSystemExtensionMethod
{
	public new class MethodName : XWFileSystemExtensionMethod.MethodName
	{
		public new static readonly StringName GetIcon = "GetIcon";

		public new static readonly StringName Execute = "Execute";

		public static readonly StringName OpenDatAnimationResource = "OpenDatAnimationResource";

		public static readonly StringName FindSameBaseAnimationResourcePath = "FindSameBaseAnimationResourcePath";
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

	public override void Execute(XWFileSystemTreeItemData data)
	{
		OpenDatAnimationResource(data);
	}

	private static void OpenDatAnimationResource(XWFileSystemTreeItemData data)
	{
		string text = data?.Path?.Replace('\\', '/') ?? "";
		if (string.IsNullOrWhiteSpace(text) || !FileAccess.FileExists(text))
		{
			XWEditorInterface.Instance?.ShowToast("DAT 文件不存在");
			return;
		}
		string text2 = FindSameBaseAnimationResourcePath(text);
		if (string.IsNullOrWhiteSpace(text2) || !ResourceLoader.Exists(text2) || !TresExtensionMethod.TryOpenAdobeAnimateResourceWithoutLoading(text2))
		{
			AdobeAnimateData adobeAnimateData = new AdobeAnimateData
			{
				ResourceName = text.GetFile().GetBaseName()
			};
			adobeAnimateData.SetAnimeFileForModImport(text.GetFile());
			adobeAnimateData.SetEditorResolvedAnimeFilePath(text);
			XWEditorInterface.Instance?.EditResource(adobeAnimateData, XWResourceEditContext.ForRoot(adobeAnimateData, text, "animation_editor"));
		}
	}

	private static string FindSameBaseAnimationResourcePath(string datPath)
	{
		if (string.IsNullOrWhiteSpace(datPath))
		{
			return "";
		}
		string text = datPath.GetBaseDir().PathJoin(datPath.GetFile().GetBaseName() + ".tres");
		if (!FileAccess.FileExists(text))
		{
			return "";
		}
		return text;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.GetIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenDatAnimationResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindSameBaseAnimationResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "datPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OpenDatAnimationResource && args.Count == 1)
		{
			OpenDatAnimationResource(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindSameBaseAnimationResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FindSameBaseAnimationResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.OpenDatAnimationResource && args.Count == 1)
		{
			OpenDatAnimationResource(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindSameBaseAnimationResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FindSameBaseAnimationResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.OpenDatAnimationResource)
		{
			return true;
		}
		if (method == MethodName.FindSameBaseAnimationResourcePath)
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
