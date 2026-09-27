using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/Resource/Signal/XWBPSignalSerializeData.cs")]
public class XWBPSignalSerializeData : Resource
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

		public static readonly StringName Inputs = "Inputs";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int Id { get; set; }

	[Export(PropertyHint.None, "")]
	public string Name { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public Array<XWBPNodePortSerializeData> Inputs { get; set; } = new Array<XWBPNodePortSerializeData>();

	public void Serialize(XWBPSignalData data)
	{
		Id = data.Id;
		Name = data.Name;
		Inputs.Clear();
		foreach (XWBPNodePortData input in data.Inputs)
		{
			XWBPNodePortSerializeData xWBPNodePortSerializeData = new XWBPNodePortSerializeData();
			xWBPNodePortSerializeData.Serialize(input);
			Inputs.Add(xWBPNodePortSerializeData);
		}
	}

	public XWBPSignalData Deserialize()
	{
		XWBPSignalData xWBPSignalData = new XWBPSignalData
		{
			Id = Id,
			Name = Name
		};
		foreach (XWBPNodePortSerializeData input in Inputs)
		{
			xWBPSignalData.Inputs.Add(input.Deserialize());
		}
		return xWBPSignalData;
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
			Serialize(VariantUtils.ConvertTo<XWBPSignalData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Deserialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPSignalData>(Deserialize());
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
		if (name == PropertyName.Inputs)
		{
			Inputs = VariantUtils.ConvertToArray<XWBPNodePortSerializeData>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Id)
		{
			value = VariantUtils.CreateFrom<int>(Id);
			return true;
		}
		if (name == PropertyName.Name)
		{
			value = VariantUtils.CreateFrom<string>(Name);
			return true;
		}
		if (name == PropertyName.Inputs)
		{
			value = VariantUtils.CreateFromArray(Inputs);
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
			new PropertyInfo(Variant.Type.Array, PropertyName.Inputs, PropertyHint.TypeString, "24/17:XWBPNodePortSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Id, Variant.From<int>(Id));
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
		info.AddProperty(PropertyName.Inputs, Variant.CreateFrom(Inputs));
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
		if (info.TryGetProperty(PropertyName.Inputs, out var value3))
		{
			Inputs = value3.AsGodotArray<XWBPNodePortSerializeData>();
		}
	}
}
