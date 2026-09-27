using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/Resource/Node/XWBPNodeSerializeData.cs")]
public class XWBPNodeSerializeData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Serialize = "Serialize";

		public static readonly StringName Deserialize = "Deserialize";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Id = "Id";

		public static readonly StringName Lock = "Lock";

		public static readonly StringName TypeId = "TypeId";

		public static readonly StringName Position = "Position";

		public static readonly StringName Size = "Size";

		public static readonly StringName InputPorts = "InputPorts";

		public static readonly StringName OutputPorts = "OutputPorts";

		public static readonly StringName MetaData = "MetaData";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int Id { get; set; }

	[Export(PropertyHint.None, "")]
	public bool Lock { get; set; }

	[Export(PropertyHint.None, "")]
	public StringName TypeId { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public Vector2 Position { get; set; } = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public Vector2 Size { get; set; } = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public Array<XWBPNodePortSerializeData> InputPorts { get; set; } = new Array<XWBPNodePortSerializeData>();

	[Export(PropertyHint.None, "")]
	public Array<XWBPNodePortSerializeData> OutputPorts { get; set; } = new Array<XWBPNodePortSerializeData>();

	[Export(PropertyHint.None, "")]
	public Dictionary MetaData { get; set; } = new Dictionary();

	public void Serialize(XWBPNodeData data)
	{
		InputPorts.Clear();
		OutputPorts.Clear();
		if (data == null)
		{
			MetaData = new Dictionary();
			return;
		}
		Id = data.Id;
		Lock = data.Lock;
		TypeId = data.TypeId;
		Position = data.Position;
		Size = data.Size;
		foreach (XWBPNodePortData inputPort in data.InputPorts)
		{
			if (inputPort != null)
			{
				XWBPNodePortSerializeData xWBPNodePortSerializeData = new XWBPNodePortSerializeData();
				xWBPNodePortSerializeData.Serialize(inputPort);
				InputPorts.Add(xWBPNodePortSerializeData);
			}
		}
		foreach (XWBPNodePortData outputPort in data.OutputPorts)
		{
			if (outputPort != null)
			{
				XWBPNodePortSerializeData xWBPNodePortSerializeData2 = new XWBPNodePortSerializeData();
				xWBPNodePortSerializeData2.Serialize(outputPort);
				OutputPorts.Add(xWBPNodePortSerializeData2);
			}
		}
		MetaData = ((data.MetaData != null) ? data.MetaData.Duplicate(deep: true) : new Dictionary());
	}

	public XWBPNodeData Deserialize()
	{
		XWBPNodeData xWBPNodeData = new XWBPNodeData(Id, TypeId)
		{
			Lock = Lock,
			Position = Position,
			Size = Size
		};
		foreach (XWBPNodePortSerializeData inputPort in InputPorts)
		{
			if (inputPort != null)
			{
				XWBPNodePortData xWBPNodePortData = inputPort.Deserialize();
				if (xWBPNodePortData != null)
				{
					xWBPNodeData.InputPorts.Add(xWBPNodePortData);
				}
			}
		}
		foreach (XWBPNodePortSerializeData outputPort in OutputPorts)
		{
			if (outputPort != null)
			{
				XWBPNodePortData xWBPNodePortData2 = outputPort.Deserialize();
				if (xWBPNodePortData2 != null)
				{
					xWBPNodeData.OutputPorts.Add(xWBPNodePortData2);
				}
			}
		}
		xWBPNodeData.MetaData = ((MetaData != null) ? MetaData.Duplicate(deep: true) : new Dictionary());
		xWBPNodeData.RebuildPortMap();
		return xWBPNodeData;
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
			Serialize(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Deserialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(Deserialize());
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
		if (name == PropertyName.Lock)
		{
			Lock = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.TypeId)
		{
			TypeId = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.Position)
		{
			Position = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.Size)
		{
			Size = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.InputPorts)
		{
			InputPorts = VariantUtils.ConvertToArray<XWBPNodePortSerializeData>(in value);
			return true;
		}
		if (name == PropertyName.OutputPorts)
		{
			OutputPorts = VariantUtils.ConvertToArray<XWBPNodePortSerializeData>(in value);
			return true;
		}
		if (name == PropertyName.MetaData)
		{
			MetaData = VariantUtils.ConvertTo<Dictionary>(in value);
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
		if (name == PropertyName.Lock)
		{
			value = VariantUtils.CreateFrom<bool>(Lock);
			return true;
		}
		if (name == PropertyName.TypeId)
		{
			value = VariantUtils.CreateFrom<StringName>(TypeId);
			return true;
		}
		Vector2 from;
		if (name == PropertyName.Position)
		{
			from = Position;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Size)
		{
			from = Size;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.InputPorts)
		{
			value = VariantUtils.CreateFromArray(InputPorts);
			return true;
		}
		if (name == PropertyName.OutputPorts)
		{
			value = VariantUtils.CreateFromArray(OutputPorts);
			return true;
		}
		if (name == PropertyName.MetaData)
		{
			value = VariantUtils.CreateFrom<Dictionary>(MetaData);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.Lock, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.TypeId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Position, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Size, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.InputPorts, PropertyHint.TypeString, "24/17:XWBPNodePortSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.OutputPorts, PropertyHint.TypeString, "24/17:XWBPNodePortSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.MetaData, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Id, Variant.From<int>(Id));
		info.AddProperty(PropertyName.Lock, Variant.From<bool>(Lock));
		info.AddProperty(PropertyName.TypeId, Variant.From<StringName>(TypeId));
		info.AddProperty(PropertyName.Position, Variant.From<Vector2>(Position));
		info.AddProperty(PropertyName.Size, Variant.From<Vector2>(Size));
		info.AddProperty(PropertyName.InputPorts, Variant.CreateFrom(InputPorts));
		info.AddProperty(PropertyName.OutputPorts, Variant.CreateFrom(OutputPorts));
		info.AddProperty(PropertyName.MetaData, Variant.From<Dictionary>(MetaData));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Id, out var value))
		{
			Id = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Lock, out var value2))
		{
			Lock = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.TypeId, out var value3))
		{
			TypeId = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.Position, out var value4))
		{
			Position = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.Size, out var value5))
		{
			Size = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.InputPorts, out var value6))
		{
			InputPorts = value6.AsGodotArray<XWBPNodePortSerializeData>();
		}
		if (info.TryGetProperty(PropertyName.OutputPorts, out var value7))
		{
			OutputPorts = value7.AsGodotArray<XWBPNodePortSerializeData>();
		}
		if (info.TryGetProperty(PropertyName.MetaData, out var value8))
		{
			MetaData = value8.As<Dictionary>();
		}
	}
}
