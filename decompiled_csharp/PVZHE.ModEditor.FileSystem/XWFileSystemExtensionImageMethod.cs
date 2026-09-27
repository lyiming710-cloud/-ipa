using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/Extension/XWFileSystemExtensionImageMethod.cs")]
public class XWFileSystemExtensionImageMethod : XWFileSystemExtensionMethod
{
	public new class MethodName : XWFileSystemExtensionMethod.MethodName
	{
		public new static readonly StringName GetIcon = "GetIcon";

		public new static readonly StringName Execute = "Execute";

		public static readonly StringName LoadTexture = "LoadTexture";

		public static readonly StringName LoadTextureFromFile = "LoadTextureFromFile";

		public static readonly StringName LoadSvgImage = "LoadSvgImage";

		public static readonly StringName CreateThumbnail = "CreateThumbnail";

		public static readonly StringName GetLoadablePath = "GetLoadablePath";
	}

	public new class PropertyName : XWFileSystemExtensionMethod.PropertyName
	{
	}

	public new class SignalName : XWFileSystemExtensionMethod.SignalName
	{
	}

	private const int DefaultIconSize = 16;

	public override Texture2D GetIcon(XWFileSystemTreeItemData data)
	{
		return GetIcon(data, 16);
	}

	public override Texture2D GetIcon(XWFileSystemTreeItemData data, int targetSize)
	{
		Texture2D texture2D = LoadTexture(data);
		if (!XWTextureSafety.CanPreview(texture2D))
		{
			return XWClassRegistry.Instance.GetUIIcon("Filesystem");
		}
		Image image = texture2D.GetImage();
		if (image == null || image.GetWidth() <= 0 || image.GetHeight() <= 0)
		{
			return XWClassRegistry.Instance.GetUIIcon("Filesystem");
		}
		return CreateThumbnail(image, targetSize);
	}

	public override void Execute(XWFileSystemTreeItemData data)
	{
		Texture2D texture2D = LoadTexture(data);
		if (XWTextureSafety.CanPreview(texture2D))
		{
			XWEditorInterface.Instance?.EditResource(texture2D, XWResourceEditContext.ForRoot(texture2D, data?.Path ?? texture2D.ResourcePath, "resource_editor"));
		}
	}

	private static Texture2D LoadTexture(XWFileSystemTreeItemData data)
	{
		if (data == null)
		{
			return null;
		}
		if (data.Data.VariantType == Variant.Type.Object && data.Data.As<GodotObject>() is Texture2D result)
		{
			return result;
		}
		if (!string.IsNullOrEmpty(data.Path) && ResourceLoader.Exists(data.Path))
		{
			return ResourceLoader.Load<Texture2D>(data.Path, null, ResourceLoader.CacheMode.Reuse);
		}
		if (!string.IsNullOrEmpty(data.Path) && FileAccess.FileExists(GetLoadablePath(data.Path)))
		{
			return LoadTextureFromFile(data.Path);
		}
		return null;
	}

	private static Texture2D LoadTextureFromFile(string path)
	{
		string loadablePath = GetLoadablePath(path);
		Image image = new Image();
		if (((path.GetExtension().ToLowerInvariant() == "svg") ? LoadSvgImage(loadablePath, image) : image.Load(loadablePath)) != Error.Ok || image.GetWidth() <= 0 || image.GetHeight() <= 0)
		{
			return null;
		}
		return ImageTexture.CreateFromImage(image);
	}

	private static Error LoadSvgImage(string path, Image image)
	{
		byte[] fileAsBytes = FileAccess.GetFileAsBytes(path);
		if (fileAsBytes.Length != 0)
		{
			return image.LoadSvgFromBuffer(fileAsBytes);
		}
		return Error.FileCantRead;
	}

	private static Texture2D CreateThumbnail(Image image, int targetSize)
	{
		targetSize = Mathf.Max(1, targetSize);
		float num = Mathf.Min((float)targetSize / (float)image.GetWidth(), (float)targetSize / (float)image.GetHeight());
		int width = Mathf.Max(1, Mathf.FloorToInt((float)image.GetWidth() * num));
		int height = Mathf.Max(1, Mathf.FloorToInt((float)image.GetHeight() * num));
		image.Resize(width, height, Image.Interpolation.Bilinear);
		return ImageTexture.CreateFromImage(image);
	}

	private static string GetLoadablePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		if (path.StartsWith("res://") || path.StartsWith("user://"))
		{
			return ProjectSettings.GlobalizePath(path);
		}
		return path.Replace('\\', '/');
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.GetIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "targetSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadTextureFromFile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadSvgImage, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateThumbnail, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Int, "targetSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLoadablePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName.GetIcon && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetIcon(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.Execute && args.Count == 1)
		{
			Execute(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadTexture(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadTextureFromFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadTextureFromFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadSvgImage && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Error>(LoadSvgImage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateThumbnail && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(CreateThumbnail(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetLoadablePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetLoadablePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadTexture(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadTextureFromFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadTextureFromFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadSvgImage && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Error>(LoadSvgImage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateThumbnail && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(CreateThumbnail(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetLoadablePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetLoadablePath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.LoadTexture)
		{
			return true;
		}
		if (method == MethodName.LoadTextureFromFile)
		{
			return true;
		}
		if (method == MethodName.LoadSvgImage)
		{
			return true;
		}
		if (method == MethodName.CreateThumbnail)
		{
			return true;
		}
		if (method == MethodName.GetLoadablePath)
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
