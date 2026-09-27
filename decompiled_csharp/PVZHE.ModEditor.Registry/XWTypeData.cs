using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Registry;

[ScriptPath("res://addons/ModEditor/Registry/Type/XWTypeData.cs")]
public class XWTypeData : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Type = "Type";

		public static readonly StringName Name = "Name";

		public static readonly StringName DefaultValue = "DefaultValue";

		public static readonly StringName DefaultValueString = "DefaultValueString";

		public static readonly StringName Color = "Color";

		public static readonly StringName Editor = "Editor";

		public static readonly StringName Icon = "Icon";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public Variant.Type Type { get; set; }

	public string Name { get; set; }

	public Variant DefaultValue { get; set; }

	public string DefaultValueString { get; set; }

	public Color Color { get; set; }

	public PackedScene Editor { get; set; }

	public Texture2D Icon { get; set; }

	public XWTypeData()
	{
	}

	public XWTypeData(Variant.Type type, string name, Variant defaultValue, string defaultValueString, Color color, PackedScene editor, Texture2D icon = null)
	{
		Type = type;
		Name = name;
		DefaultValue = defaultValue;
		DefaultValueString = defaultValueString;
		Color = color;
		Editor = editor;
		Icon = icon;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Type)
		{
			Type = VariantUtils.ConvertTo<Variant.Type>(in value);
			return true;
		}
		if (name == PropertyName.Name)
		{
			Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.DefaultValue)
		{
			DefaultValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.DefaultValueString)
		{
			DefaultValueString = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Color)
		{
			Color = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.Editor)
		{
			Editor = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.Icon)
		{
			Icon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Type)
		{
			value = VariantUtils.CreateFrom<Variant.Type>(Type);
			return true;
		}
		string from;
		if (name == PropertyName.Name)
		{
			from = Name;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DefaultValue)
		{
			value = VariantUtils.CreateFrom<Variant>(DefaultValue);
			return true;
		}
		if (name == PropertyName.DefaultValueString)
		{
			from = DefaultValueString;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Color)
		{
			value = VariantUtils.CreateFrom<Color>(Color);
			return true;
		}
		if (name == PropertyName.Editor)
		{
			value = VariantUtils.CreateFrom<PackedScene>(Editor);
			return true;
		}
		if (name == PropertyName.Icon)
		{
			value = VariantUtils.CreateFrom<Texture2D>(Icon);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.Type, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.DefaultValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.DefaultValueString, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.Color, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Icon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Type, Variant.From<Variant.Type>(Type));
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
		info.AddProperty(PropertyName.DefaultValue, Variant.From<Variant>(DefaultValue));
		info.AddProperty(PropertyName.DefaultValueString, Variant.From<string>(DefaultValueString));
		info.AddProperty(PropertyName.Color, Variant.From<Color>(Color));
		info.AddProperty(PropertyName.Editor, Variant.From<PackedScene>(Editor));
		info.AddProperty(PropertyName.Icon, Variant.From<Texture2D>(Icon));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Type, out var value))
		{
			Type = value.As<Variant.Type>();
		}
		if (info.TryGetProperty(PropertyName.Name, out var value2))
		{
			Name = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.DefaultValue, out var value3))
		{
			DefaultValue = value3.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.DefaultValueString, out var value4))
		{
			DefaultValueString = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Color, out var value5))
		{
			Color = value5.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.Editor, out var value6))
		{
			Editor = value6.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.Icon, out var value7))
		{
			Icon = value7.As<Texture2D>();
		}
	}
}
