using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/Extension/Resource/XWFileSystemExtensionVisualResourceMethod.cs")]
public class XWFileSystemExtensionVisualResourceMethod : XWFileSystemExtensionMethod
{
	public new class MethodName : XWFileSystemExtensionMethod.MethodName
	{
		public new static readonly StringName GetIcon = "GetIcon";

		public new static readonly StringName Execute = "Execute";

		public static readonly StringName LoadExternalAudio = "LoadExternalAudio";
	}

	public new class PropertyName : XWFileSystemExtensionMethod.PropertyName
	{
	}

	public new class SignalName : XWFileSystemExtensionMethod.SignalName
	{
	}

	public override Texture2D GetIcon(XWFileSystemTreeItemData data)
	{
		if (data != null && data.Data.VariantType == Variant.Type.Object && data.Data.As<GodotObject>() is Resource resource)
		{
			return XWClassRegistry.Instance.GetClassIcon(resource.GetClass());
		}
		string text;
		switch (data?.GetExtension() ?? "")
		{
		case "wav":
		case "ogg":
		case "mp3":
		case "flac":
			text = "AudioStream";
			break;
		case "ttf":
		case "otf":
		case "woff":
		case "woff2":
			text = "Font";
			break;
		case "gdshader":
			text = "Shader";
			break;
		case "glb":
		case "fbx":
		case "dae":
		case "gltf":
		case "blend":
			text = "PackedScene";
			break;
		case "obj":
			text = "Mesh";
			break;
		case "translation":
		case "po":
			text = "Translation";
			break;
		case "ogv":
			text = "VideoStream";
			break;
		default:
			text = "Resource";
			break;
		}
		string className = text;
		return XWClassRegistry.Instance.GetClassIcon(className);
	}

	public override void Execute(XWFileSystemTreeItemData data)
	{
		if (data != null && !string.IsNullOrWhiteSpace(data.Path))
		{
			Resource resource = ((data.Data.VariantType == Variant.Type.Object) ? (data.Data.As<GodotObject>() as Resource) : null);
			if (!GodotObject.IsInstanceValid(resource) && ResourceLoader.Exists(data.Path))
			{
				resource = ResourceLoader.Load<Resource>(data.Path, null, ResourceLoader.CacheMode.Reuse);
			}
			if (!GodotObject.IsInstanceValid(resource))
			{
				resource = LoadExternalAudio(data.Path);
			}
			if (!GodotObject.IsInstanceValid(resource))
			{
				XWEditorInterface.Instance?.ShowToast("无法加载资源: " + data.Path);
			}
			else
			{
				XWEditorInterface.Instance?.EditResource(resource, XWResourceEditContext.ForRoot(resource, data.Path, "resource_editor"));
			}
		}
	}

	private static Resource LoadExternalAudio(string path)
	{
		if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
		{
			return null;
		}
		if (!XWModExternalMediaLoader.TryLoadAudio(path, Path.GetExtension(path), out var audio, out var _))
		{
			return null;
		}
		return audio;
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
			new MethodInfo(MethodName.LoadExternalAudio, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.LoadExternalAudio && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(LoadExternalAudio(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadExternalAudio && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(LoadExternalAudio(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.LoadExternalAudio)
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
