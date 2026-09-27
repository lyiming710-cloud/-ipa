using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/RefCounted/Tree/XWFileSystemTreeItemData.cs")]
public class XWFileSystemTreeItemData : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName GetExtension = "GetExtension";

		public static readonly StringName GetFileName = "GetFileName";

		public static readonly StringName GetDisplayName = "GetDisplayName";

		public static readonly StringName GetBaseDir = "GetBaseDir";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Data = "Data";

		public static readonly StringName Path = "Path";

		public static readonly StringName IsFolder = "IsFolder";

		public static readonly StringName IsSubResource = "IsSubResource";

		public static readonly StringName SubResourceOwnerPath = "SubResourceOwnerPath";

		public static readonly StringName SubResourcePropertyPath = "SubResourcePropertyPath";

		public static readonly StringName SubResourceDisplayName = "SubResourceDisplayName";

		public static readonly StringName IsCompanionResource = "IsCompanionResource";

		public static readonly StringName CompanionOwnerPath = "CompanionOwnerPath";

		public static readonly StringName CompanionKind = "CompanionKind";

		public static readonly StringName CompanionDisplayName = "CompanionDisplayName";

		public static readonly StringName ModifiedTime = "ModifiedTime";

		public static readonly StringName FileType = "FileType";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public Variant Data { get; set; }

	public string Path { get; set; } = "";

	public bool IsFolder { get; set; }

	public bool IsSubResource { get; set; }

	public string SubResourceOwnerPath { get; set; } = "";

	public string SubResourcePropertyPath { get; set; } = "";

	public string SubResourceDisplayName { get; set; } = "";

	public bool IsCompanionResource { get; set; }

	public string CompanionOwnerPath { get; set; } = "";

	public string CompanionKind { get; set; } = "";

	public string CompanionDisplayName { get; set; } = "";

	public long ModifiedTime { get; set; }

	public StringName FileType { get; set; } = "";

	public XWFileSystemTreeItemData()
	{
	}

	public XWFileSystemTreeItemData(string pPath, Variant pData, bool pIsFolder)
	{
		Path = pPath;
		Data = pData;
		IsFolder = pIsFolder;
	}

	public string GetExtension()
	{
		return Path.GetExtension().ToLower();
	}

	public string GetFileName()
	{
		return Path.GetFile();
	}

	public string GetDisplayName()
	{
		if (IsSubResource && !string.IsNullOrWhiteSpace(SubResourceDisplayName))
		{
			return SubResourceDisplayName;
		}
		if (IsCompanionResource && !string.IsNullOrWhiteSpace(CompanionDisplayName))
		{
			return CompanionDisplayName;
		}
		if (IsFolder)
		{
			return Path.TrimSuffix("/").GetFile();
		}
		return GetFileName();
	}

	public string GetBaseDir()
	{
		return Path.GetBaseDir();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.GetExtension, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFileName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetBaseDir, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetExtension && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetExtension());
			return true;
		}
		if (method == MethodName.GetFileName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetFileName());
			return true;
		}
		if (method == MethodName.GetDisplayName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetDisplayName());
			return true;
		}
		if (method == MethodName.GetBaseDir && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetBaseDir());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetExtension)
		{
			return true;
		}
		if (method == MethodName.GetFileName)
		{
			return true;
		}
		if (method == MethodName.GetDisplayName)
		{
			return true;
		}
		if (method == MethodName.GetBaseDir)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Data)
		{
			Data = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.Path)
		{
			Path = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.IsFolder)
		{
			IsFolder = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsSubResource)
		{
			IsSubResource = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.SubResourceOwnerPath)
		{
			SubResourceOwnerPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.SubResourcePropertyPath)
		{
			SubResourcePropertyPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.SubResourceDisplayName)
		{
			SubResourceDisplayName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.IsCompanionResource)
		{
			IsCompanionResource = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.CompanionOwnerPath)
		{
			CompanionOwnerPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.CompanionKind)
		{
			CompanionKind = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.CompanionDisplayName)
		{
			CompanionDisplayName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ModifiedTime)
		{
			ModifiedTime = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.FileType)
		{
			FileType = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Data)
		{
			value = VariantUtils.CreateFrom<Variant>(Data);
			return true;
		}
		string from;
		if (name == PropertyName.Path)
		{
			from = Path;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.IsFolder)
		{
			from2 = IsFolder;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsSubResource)
		{
			from2 = IsSubResource;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SubResourceOwnerPath)
		{
			from = SubResourceOwnerPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SubResourcePropertyPath)
		{
			from = SubResourcePropertyPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SubResourceDisplayName)
		{
			from = SubResourceDisplayName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsCompanionResource)
		{
			from2 = IsCompanionResource;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CompanionOwnerPath)
		{
			from = CompanionOwnerPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CompanionKind)
		{
			from = CompanionKind;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CompanionDisplayName)
		{
			from = CompanionDisplayName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ModifiedTime)
		{
			value = VariantUtils.CreateFrom<long>(ModifiedTime);
			return true;
		}
		if (name == PropertyName.FileType)
		{
			value = VariantUtils.CreateFrom<StringName>(FileType);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, PropertyName.Data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Path, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsFolder, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsSubResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.SubResourceOwnerPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.SubResourcePropertyPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.SubResourceDisplayName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCompanionResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.CompanionOwnerPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.CompanionKind, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.CompanionDisplayName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ModifiedTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.FileType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Data, Variant.From<Variant>(Data));
		info.AddProperty(PropertyName.Path, Variant.From<string>(Path));
		info.AddProperty(PropertyName.IsFolder, Variant.From<bool>(IsFolder));
		info.AddProperty(PropertyName.IsSubResource, Variant.From<bool>(IsSubResource));
		info.AddProperty(PropertyName.SubResourceOwnerPath, Variant.From<string>(SubResourceOwnerPath));
		info.AddProperty(PropertyName.SubResourcePropertyPath, Variant.From<string>(SubResourcePropertyPath));
		info.AddProperty(PropertyName.SubResourceDisplayName, Variant.From<string>(SubResourceDisplayName));
		info.AddProperty(PropertyName.IsCompanionResource, Variant.From<bool>(IsCompanionResource));
		info.AddProperty(PropertyName.CompanionOwnerPath, Variant.From<string>(CompanionOwnerPath));
		info.AddProperty(PropertyName.CompanionKind, Variant.From<string>(CompanionKind));
		info.AddProperty(PropertyName.CompanionDisplayName, Variant.From<string>(CompanionDisplayName));
		info.AddProperty(PropertyName.ModifiedTime, Variant.From<long>(ModifiedTime));
		info.AddProperty(PropertyName.FileType, Variant.From<StringName>(FileType));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Data, out var value))
		{
			Data = value.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.Path, out var value2))
		{
			Path = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.IsFolder, out var value3))
		{
			IsFolder = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsSubResource, out var value4))
		{
			IsSubResource = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.SubResourceOwnerPath, out var value5))
		{
			SubResourceOwnerPath = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.SubResourcePropertyPath, out var value6))
		{
			SubResourcePropertyPath = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.SubResourceDisplayName, out var value7))
		{
			SubResourceDisplayName = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.IsCompanionResource, out var value8))
		{
			IsCompanionResource = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.CompanionOwnerPath, out var value9))
		{
			CompanionOwnerPath = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.CompanionKind, out var value10))
		{
			CompanionKind = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.CompanionDisplayName, out var value11))
		{
			CompanionDisplayName = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ModifiedTime, out var value12))
		{
			ModifiedTime = value12.As<long>();
		}
		if (info.TryGetProperty(PropertyName.FileType, out var value13))
		{
			FileType = value13.As<StringName>();
		}
	}
}
