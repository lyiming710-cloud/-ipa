using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/Resource/Node/XWBPNodePortSerializeData.cs")]
public class XWBPNodePortSerializeData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Serialize = "Serialize";

		public static readonly StringName Deserialize = "Deserialize";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Name = "Name";

		public static readonly StringName Direction = "Direction";

		public static readonly StringName PortType = "PortType";

		public static readonly StringName ClassName = "ClassName";

		public static readonly StringName DefaultValue = "DefaultValue";

		public static readonly StringName Value = "Value";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string Name { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public int Direction { get; set; }

	[Export(PropertyHint.None, "")]
	public int PortType { get; set; }

	[Export(PropertyHint.None, "")]
	public StringName ClassName { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public Variant DefaultValue { get; set; }

	[Export(PropertyHint.None, "")]
	public Variant Value { get; set; }

	public void Serialize(XWBPNodePortData data)
	{
		if (data != null)
		{
			Name = data.Name;
			Direction = (int)data.PortDirection;
			PortType = (int)data.PortTypeValue;
			ClassName = data.ClassName;
			DefaultValue = data.DefaultValue;
			Value = data.Value;
		}
	}

	public XWBPNodePortData Deserialize()
	{
		XWBPNodePortData.Direction direction = ((Direction == 1) ? XWBPNodePortData.Direction.Output : XWBPNodePortData.Direction.Input);
		return new XWBPNodePortData(Name ?? "", direction, (XWBPNodePortData.PortType)PortType, ClassName, DefaultValue)
		{
			Value = Value
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
			Serialize(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Deserialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPNodePortData>(Deserialize());
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
		if (name == PropertyName.Name)
		{
			Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Direction)
		{
			Direction = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.PortType)
		{
			PortType = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName.Value)
		{
			Value = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Name)
		{
			value = VariantUtils.CreateFrom<string>(Name);
			return true;
		}
		int from;
		if (name == PropertyName.Direction)
		{
			from = Direction;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PortType)
		{
			from = PortType;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ClassName)
		{
			value = VariantUtils.CreateFrom<StringName>(ClassName);
			return true;
		}
		Variant from2;
		if (name == PropertyName.DefaultValue)
		{
			from2 = DefaultValue;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.Value)
		{
			from2 = Value;
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
			new PropertyInfo(Variant.Type.String, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.Direction, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.PortType, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.ClassName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, PropertyName.DefaultValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable | PropertyUsageFlags.NilIsVariant, exported: true),
			new PropertyInfo(Variant.Type.Nil, PropertyName.Value, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable | PropertyUsageFlags.NilIsVariant, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
		info.AddProperty(PropertyName.Direction, Variant.From<int>(Direction));
		info.AddProperty(PropertyName.PortType, Variant.From<int>(PortType));
		info.AddProperty(PropertyName.ClassName, Variant.From<StringName>(ClassName));
		info.AddProperty(PropertyName.DefaultValue, Variant.From<Variant>(DefaultValue));
		info.AddProperty(PropertyName.Value, Variant.From<Variant>(Value));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Name, out var value))
		{
			Name = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Direction, out var value2))
		{
			Direction = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.PortType, out var value3))
		{
			PortType = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ClassName, out var value4))
		{
			ClassName = value4.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.DefaultValue, out var value5))
		{
			DefaultValue = value5.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.Value, out var value6))
		{
			Value = value6.As<Variant>();
		}
	}
}
