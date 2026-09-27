using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/Resource/Variable/XWBPVariableSerializeData.cs")]
public class XWBPVariableSerializeData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Serialize = "Serialize";

		public static readonly StringName Deserialize = "Deserialize";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Id = "Id";

		public static readonly StringName Name = "Name";

		public static readonly StringName Type = "Type";

		public static readonly StringName ClassName = "ClassName";

		public static readonly StringName DefaultValue = "DefaultValue";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int Id { get; set; }

	[Export(PropertyHint.None, "")]
	public string Name { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public int Type { get; set; }

	[Export(PropertyHint.None, "")]
	public StringName ClassName { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public Variant DefaultValue { get; set; }

	public void Serialize(XWBPVariableData data)
	{
		Id = data.Id;
		Name = data.Name;
		Type = (int)data.Type;
		ClassName = data.ClassName;
		DefaultValue = data.DefaultValue;
	}

	public XWBPVariableData Deserialize()
	{
		return new XWBPVariableData
		{
			Id = Id,
			Name = Name,
			Type = (Variant.Type)Type,
			ClassName = ClassName,
			DefaultValue = DefaultValue
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Serialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Deserialize, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Serialize && args.Count == 1)
		{
			Serialize(VariantUtils.ConvertTo<XWBPVariableData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Deserialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPVariableData>(Deserialize());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Serialize)
		{
			return true;
		}
		if (method == MethodName.Deserialize)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Id)
		{
			Id = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Name)
		{
			Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Type)
		{
			Type = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ClassName)
		{
			ClassName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.DefaultValue)
		{
			DefaultValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.Id)
		{
			from = Id;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Name)
		{
			value = VariantUtils.CreateFrom<string>(Name);
			return true;
		}
		if (name == PropertyName.Type)
		{
			from = Type;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ClassName)
		{
			value = VariantUtils.CreateFrom<StringName>(ClassName);
			return true;
		}
		if (name == PropertyName.DefaultValue)
		{
			value = VariantUtils.CreateFrom<Variant>(DefaultValue);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.Id, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.Type, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.ClassName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, PropertyName.DefaultValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable | PropertyUsageFlags.NilIsVariant, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Id, Variant.From<int>(Id));
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
		info.AddProperty(PropertyName.Type, Variant.From<int>(Type));
		info.AddProperty(PropertyName.ClassName, Variant.From<StringName>(ClassName));
		info.AddProperty(PropertyName.DefaultValue, Variant.From<Variant>(DefaultValue));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Id, out var value))
		{
			Id = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Name, out var value2))
		{
			Name = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Type, out var value3))
		{
			Type = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ClassName, out var value4))
		{
			ClassName = value4.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.DefaultValue, out var value5))
		{
			DefaultValue = value5.As<Variant>();
		}
	}
}
