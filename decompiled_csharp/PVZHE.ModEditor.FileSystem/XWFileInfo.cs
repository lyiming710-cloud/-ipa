using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/RefCounted/XWFileInfo.cs")]
public class XWFileInfo : RefCounted
{
	private class XWFileInfoComparer : IComparer<XWFileInfo>
	{
		private readonly Func<XWFileInfo, XWFileInfo, bool> _less;

		public XWFileInfoComparer(XWFileSystemEnum.SortMode mode)
		{
			_less = GetSortCallable(mode);
		}

		public int Compare(XWFileInfo a, XWFileInfo b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return 1;
			}
			if (b == null)
			{
				return -1;
			}
			if (_less(a, b))
			{
				return -1;
			}
			if (_less(b, a))
			{
				return 1;
			}
			return 0;
		}
	}

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName NaturalCompare = "NaturalCompare";

		public static readonly StringName SortByNameAsc = "SortByNameAsc";

		public static readonly StringName SortByNameDesc = "SortByNameDesc";

		public static readonly StringName SortByTypeAsc = "SortByTypeAsc";

		public static readonly StringName SortByTypeDesc = "SortByTypeDesc";

		public static readonly StringName SortByModifiedTimeAsc = "SortByModifiedTimeAsc";

		public static readonly StringName SortByModifiedTimeDesc = "SortByModifiedTimeDesc";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Name = "Name";

		public static readonly StringName Path = "Path";

		public static readonly StringName Type = "Type";

		public static readonly StringName IconPath = "IconPath";

		public static readonly StringName ModifiedTime = "ModifiedTime";

		public static readonly StringName ImportBroken = "ImportBroken";

		public static readonly StringName Sources = "Sources";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public string Name { get; set; } = "";

	public string Path { get; set; } = "";

	public StringName Type { get; set; } = "";

	public string IconPath { get; set; } = "";

	public ulong ModifiedTime { get; set; }

	public bool ImportBroken { get; set; }

	public string[] Sources { get; set; } = Array.Empty<string>();

	public XWFileInfo()
	{
	}

	public XWFileInfo(string pName, string pPath)
	{
		Name = pName;
		Path = pPath;
	}

	private static int NaturalCompare(string a, string b)
	{
		return string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
	}

	public static bool SortByNameAsc(XWFileInfo a, XWFileInfo b)
	{
		return NaturalCompare(a.Name, b.Name) < 0;
	}

	public static bool SortByNameDesc(XWFileInfo a, XWFileInfo b)
	{
		return NaturalCompare(a.Name, b.Name) > 0;
	}

	public static bool SortByTypeAsc(XWFileInfo a, XWFileInfo b)
	{
		string text = a.Name.GetExtension().ToLower();
		string text2 = b.Name.GetExtension().ToLower();
		if (text == text2)
		{
			return NaturalCompare(a.Name, b.Name) < 0;
		}
		return string.Compare(text, text2, StringComparison.OrdinalIgnoreCase) < 0;
	}

	public static bool SortByTypeDesc(XWFileInfo a, XWFileInfo b)
	{
		string text = a.Name.GetExtension().ToLower();
		string text2 = b.Name.GetExtension().ToLower();
		if (text == text2)
		{
			return NaturalCompare(a.Name, b.Name) < 0;
		}
		return string.Compare(text, text2, StringComparison.OrdinalIgnoreCase) > 0;
	}

	public static bool SortByModifiedTimeAsc(XWFileInfo a, XWFileInfo b)
	{
		if (a.ModifiedTime == b.ModifiedTime)
		{
			return NaturalCompare(a.Name, b.Name) < 0;
		}
		return a.ModifiedTime < b.ModifiedTime;
	}

	public static bool SortByModifiedTimeDesc(XWFileInfo a, XWFileInfo b)
	{
		if (a.ModifiedTime == b.ModifiedTime)
		{
			return NaturalCompare(a.Name, b.Name) < 0;
		}
		return a.ModifiedTime > b.ModifiedTime;
	}

	public static Func<XWFileInfo, XWFileInfo, bool> GetSortCallable(XWFileSystemEnum.SortMode sortMode)
	{
		return sortMode switch
		{
			XWFileSystemEnum.SortMode.NameAsc => (Func<XWFileInfo, XWFileInfo, bool>)SortByNameAsc, 
			XWFileSystemEnum.SortMode.NameDesc => SortByNameDesc, 
			XWFileSystemEnum.SortMode.TypeAsc => SortByTypeAsc, 
			XWFileSystemEnum.SortMode.TypeDesc => SortByTypeDesc, 
			XWFileSystemEnum.SortMode.ModifiedTimeAsc => SortByModifiedTimeAsc, 
			XWFileSystemEnum.SortMode.ModifiedTimeDesc => SortByModifiedTimeDesc, 
			_ => SortByNameAsc, 
		};
	}

	public static void SortList(List<XWFileInfo> list, XWFileSystemEnum.SortMode sortMode)
	{
		XWFileInfoComparer comparer = new XWFileInfoComparer(sortMode);
		list.Sort(comparer);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.NaturalCompare, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "a", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "b", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SortByNameAsc, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "a", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "b", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SortByNameDesc, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "a", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "b", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SortByTypeAsc, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "a", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "b", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SortByTypeDesc, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "a", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "b", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SortByModifiedTimeAsc, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "a", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "b", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SortByModifiedTimeDesc, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "a", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "b", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.NaturalCompare && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(NaturalCompare(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByNameAsc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByNameAsc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByNameDesc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByNameDesc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByTypeAsc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByTypeAsc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByTypeDesc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByTypeDesc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByModifiedTimeAsc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByModifiedTimeAsc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByModifiedTimeDesc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByModifiedTimeDesc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.NaturalCompare && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(NaturalCompare(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByNameAsc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByNameAsc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByNameDesc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByNameDesc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByTypeAsc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByTypeAsc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByTypeDesc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByTypeDesc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByModifiedTimeAsc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByModifiedTimeAsc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		if (method == MethodName.SortByModifiedTimeDesc && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SortByModifiedTimeDesc(VariantUtils.ConvertTo<XWFileInfo>(in args[0]), VariantUtils.ConvertTo<XWFileInfo>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.NaturalCompare)
		{
			return true;
		}
		if (method == MethodName.SortByNameAsc)
		{
			return true;
		}
		if (method == MethodName.SortByNameDesc)
		{
			return true;
		}
		if (method == MethodName.SortByTypeAsc)
		{
			return true;
		}
		if (method == MethodName.SortByTypeDesc)
		{
			return true;
		}
		if (method == MethodName.SortByModifiedTimeAsc)
		{
			return true;
		}
		if (method == MethodName.SortByModifiedTimeDesc)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Name)
		{
			Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Path)
		{
			Path = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Type)
		{
			Type = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.IconPath)
		{
			IconPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ModifiedTime)
		{
			ModifiedTime = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.ImportBroken)
		{
			ImportBroken = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Sources)
		{
			Sources = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.Name)
		{
			from = Name;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Path)
		{
			from = Path;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Type)
		{
			value = VariantUtils.CreateFrom<StringName>(Type);
			return true;
		}
		if (name == PropertyName.IconPath)
		{
			from = IconPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ModifiedTime)
		{
			value = VariantUtils.CreateFrom<ulong>(ModifiedTime);
			return true;
		}
		if (name == PropertyName.ImportBroken)
		{
			value = VariantUtils.CreateFrom<bool>(ImportBroken);
			return true;
		}
		if (name == PropertyName.Sources)
		{
			value = VariantUtils.CreateFrom<string[]>(Sources);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Path, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.Type, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.IconPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ModifiedTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ImportBroken, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName.Sources, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
		info.AddProperty(PropertyName.Path, Variant.From<string>(Path));
		info.AddProperty(PropertyName.Type, Variant.From<StringName>(Type));
		info.AddProperty(PropertyName.IconPath, Variant.From<string>(IconPath));
		info.AddProperty(PropertyName.ModifiedTime, Variant.From<ulong>(ModifiedTime));
		info.AddProperty(PropertyName.ImportBroken, Variant.From<bool>(ImportBroken));
		info.AddProperty(PropertyName.Sources, Variant.From<string[]>(Sources));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Name, out var value))
		{
			Name = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Path, out var value2))
		{
			Path = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Type, out var value3))
		{
			Type = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.IconPath, out var value4))
		{
			IconPath = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ModifiedTime, out var value5))
		{
			ModifiedTime = value5.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.ImportBroken, out var value6))
		{
			ImportBroken = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Sources, out var value7))
		{
			Sources = value7.As<string[]>();
		}
	}
}
