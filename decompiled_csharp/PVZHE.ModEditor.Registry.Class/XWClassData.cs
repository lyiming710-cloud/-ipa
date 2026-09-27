using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Registry.Class;

[ScriptPath("res://addons/ModEditor/Registry/Class/XWClassData.cs")]
public sealed class XWClassData : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName ClassName = "ClassName";

		public static readonly StringName Color = "Color";

		public static readonly StringName ScriptFile = "ScriptFile";

		public static readonly StringName Icon = "Icon";

		public static readonly StringName IsGodotClass = "IsGodotClass";

		public static readonly StringName IsGlobalClass = "IsGlobalClass";

		public static readonly StringName BaseClass = "BaseClass";

		public static readonly StringName ParentClassData = "ParentClassData";

		public static readonly StringName ScriptPath = "ScriptPath";

		public static readonly StringName IconPath = "IconPath";

		public static readonly StringName MethodNamesCache = "MethodNamesCache";

		public static readonly StringName MethodNamesCached = "MethodNamesCached";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public string ClassName { get; set; } = "";

	public Color Color { get; set; } = Colors.White;

	public Script ScriptFile { get; set; }

	public Texture2D Icon { get; set; }

	public bool IsGodotClass { get; set; }

	public bool IsGlobalClass { get; set; }

	public string BaseClass { get; set; } = "";

	public XWClassData ParentClassData { get; set; }

	public string ScriptPath { get; set; } = "";

	public string IconPath { get; set; } = "";

	public Dictionary MethodNamesCache { get; } = new Dictionary();

	public bool MethodNamesCached { get; set; }

	public XWClassData()
	{
	}

	public XWClassData(string className = "", Color color = default(Color), Script scriptFile = null, Texture2D icon = null)
	{
		ClassName = className;
		Color = ((color == default(Color)) ? Colors.White : color);
		ScriptFile = scriptFile;
		Icon = icon;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ClassName)
		{
			ClassName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Color)
		{
			Color = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.ScriptFile)
		{
			ScriptFile = VariantUtils.ConvertTo<Script>(in value);
			return true;
		}
		if (name == PropertyName.Icon)
		{
			Icon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.IsGodotClass)
		{
			IsGodotClass = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsGlobalClass)
		{
			IsGlobalClass = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.BaseClass)
		{
			BaseClass = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ParentClassData)
		{
			ParentClassData = VariantUtils.ConvertTo<XWClassData>(in value);
			return true;
		}
		if (name == PropertyName.ScriptPath)
		{
			ScriptPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.IconPath)
		{
			IconPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.MethodNamesCached)
		{
			MethodNamesCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.ClassName)
		{
			from = ClassName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Color)
		{
			value = VariantUtils.CreateFrom<Color>(Color);
			return true;
		}
		if (name == PropertyName.ScriptFile)
		{
			value = VariantUtils.CreateFrom<Script>(ScriptFile);
			return true;
		}
		if (name == PropertyName.Icon)
		{
			value = VariantUtils.CreateFrom<Texture2D>(Icon);
			return true;
		}
		bool from2;
		if (name == PropertyName.IsGodotClass)
		{
			from2 = IsGodotClass;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsGlobalClass)
		{
			from2 = IsGlobalClass;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.BaseClass)
		{
			from = BaseClass;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ParentClassData)
		{
			value = VariantUtils.CreateFrom<XWClassData>(ParentClassData);
			return true;
		}
		if (name == PropertyName.ScriptPath)
		{
			from = ScriptPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IconPath)
		{
			from = IconPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MethodNamesCache)
		{
			value = VariantUtils.CreateFrom<Dictionary>(MethodNamesCache);
			return true;
		}
		if (name == PropertyName.MethodNamesCached)
		{
			from2 = MethodNamesCached;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ClassName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.Color, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ScriptFile, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Icon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsGodotClass, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsGlobalClass, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.BaseClass, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ParentClassData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ScriptPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.IconPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.MethodNamesCache, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.MethodNamesCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ClassName, Variant.From<string>(ClassName));
		info.AddProperty(PropertyName.Color, Variant.From<Color>(Color));
		info.AddProperty(PropertyName.ScriptFile, Variant.From<Script>(ScriptFile));
		info.AddProperty(PropertyName.Icon, Variant.From<Texture2D>(Icon));
		info.AddProperty(PropertyName.IsGodotClass, Variant.From<bool>(IsGodotClass));
		info.AddProperty(PropertyName.IsGlobalClass, Variant.From<bool>(IsGlobalClass));
		info.AddProperty(PropertyName.BaseClass, Variant.From<string>(BaseClass));
		info.AddProperty(PropertyName.ParentClassData, Variant.From<XWClassData>(ParentClassData));
		info.AddProperty(PropertyName.ScriptPath, Variant.From<string>(ScriptPath));
		info.AddProperty(PropertyName.IconPath, Variant.From<string>(IconPath));
		info.AddProperty(PropertyName.MethodNamesCached, Variant.From<bool>(MethodNamesCached));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ClassName, out var value))
		{
			ClassName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Color, out var value2))
		{
			Color = value2.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.ScriptFile, out var value3))
		{
			ScriptFile = value3.As<Script>();
		}
		if (info.TryGetProperty(PropertyName.Icon, out var value4))
		{
			Icon = value4.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.IsGodotClass, out var value5))
		{
			IsGodotClass = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsGlobalClass, out var value6))
		{
			IsGlobalClass = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.BaseClass, out var value7))
		{
			BaseClass = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ParentClassData, out var value8))
		{
			ParentClassData = value8.As<XWClassData>();
		}
		if (info.TryGetProperty(PropertyName.ScriptPath, out var value9))
		{
			ScriptPath = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.IconPath, out var value10))
		{
			IconPath = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.MethodNamesCached, out var value11))
		{
			MethodNamesCached = value11.As<bool>();
		}
	}
}
