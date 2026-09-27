using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/Node/XWBPNodeData.cs")]
public class XWBPNodeData : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName RebuildPortMap = "RebuildPortMap";

		public static readonly StringName GetInputPort = "GetInputPort";

		public static readonly StringName GetOutputPort = "GetOutputPort";

		public static readonly StringName FindInputPortByName = "FindInputPortByName";

		public static readonly StringName FindOutputPortByName = "FindOutputPortByName";

		public static readonly StringName SetInputValue = "SetInputValue";

		public static readonly StringName GetInputValue = "GetInputValue";

		public static readonly StringName SetOutputValue = "SetOutputValue";

		public static readonly StringName GetOutputValue = "GetOutputValue";

		public static readonly StringName GetOutputValueByIndex = "GetOutputValueByIndex";

		public static readonly StringName SetOutputValueByIndex = "SetOutputValueByIndex";

		public static readonly StringName SetMetaData = "SetMetaData";

		public static readonly StringName GetMetaData = "GetMetaData";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Id = "Id";

		public static readonly StringName Lock = "Lock";

		public static readonly StringName TypeId = "TypeId";

		public static readonly StringName Position = "Position";

		public static readonly StringName Size = "Size";

		public static readonly StringName MetaData = "MetaData";

		public static readonly StringName NodeType = "NodeType";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private readonly System.Collections.Generic.Dictionary<string, XWBPNodePortData> _inputPortMap = new System.Collections.Generic.Dictionary<string, XWBPNodePortData>();

	private readonly System.Collections.Generic.Dictionary<string, XWBPNodePortData> _outputPortMap = new System.Collections.Generic.Dictionary<string, XWBPNodePortData>();

	public int Id { get; set; }

	public bool Lock { get; set; }

	public StringName TypeId { get; set; } = "";

	public Vector2 Position { get; set; } = Vector2.Zero;

	public Vector2 Size { get; set; } = Vector2.Zero;

	public List<XWBPNodePortData> InputPorts { get; set; } = new List<XWBPNodePortData>();

	public List<XWBPNodePortData> OutputPorts { get; set; } = new List<XWBPNodePortData>();

	public Dictionary MetaData { get; set; } = new Dictionary();

	public XWBPNodeType NodeType { get; set; }

	public XWBPNodeData()
	{
	}

	public XWBPNodeData(int id, StringName typeId)
	{
		Id = id;
		TypeId = typeId;
	}

	public void RebuildPortMap()
	{
		_inputPortMap.Clear();
		foreach (XWBPNodePortData inputPort in InputPorts)
		{
			if (inputPort != null)
			{
				_inputPortMap[inputPort.Name] = inputPort;
			}
		}
		_outputPortMap.Clear();
		foreach (XWBPNodePortData outputPort in OutputPorts)
		{
			if (outputPort != null)
			{
				_outputPortMap[outputPort.Name] = outputPort;
			}
		}
	}

	public XWBPNodePortData GetInputPort(int index)
	{
		if (index >= 0 && index < InputPorts.Count)
		{
			return InputPorts[index];
		}
		return null;
	}

	public XWBPNodePortData GetOutputPort(int index)
	{
		if (index >= 0 && index < OutputPorts.Count)
		{
			return OutputPorts[index];
		}
		return null;
	}

	public XWBPNodePortData FindInputPortByName(string portName)
	{
		if (_inputPortMap.TryGetValue(portName, out var value))
		{
			return value;
		}
		foreach (XWBPNodePortData inputPort in InputPorts)
		{
			if (inputPort.Name == portName)
			{
				_inputPortMap[portName] = inputPort;
				return inputPort;
			}
		}
		return null;
	}

	public XWBPNodePortData FindOutputPortByName(string portName)
	{
		if (_outputPortMap.TryGetValue(portName, out var value))
		{
			return value;
		}
		foreach (XWBPNodePortData outputPort in OutputPorts)
		{
			if (outputPort.Name == portName)
			{
				_outputPortMap[portName] = outputPort;
				return outputPort;
			}
		}
		return null;
	}

	public void SetInputValue(string portName, Variant val)
	{
		XWBPNodePortData xWBPNodePortData = FindInputPortByName(portName);
		if (xWBPNodePortData != null)
		{
			xWBPNodePortData.Value = val;
		}
	}

	public Variant GetInputValue(string portName)
	{
		return FindInputPortByName(portName)?.Value ?? default(Variant);
	}

	public void SetOutputValue(string portName, Variant val)
	{
		XWBPNodePortData xWBPNodePortData = FindOutputPortByName(portName);
		if (xWBPNodePortData != null)
		{
			xWBPNodePortData.Value = val;
		}
	}

	public Variant GetOutputValue(string portName)
	{
		return FindOutputPortByName(portName)?.Value ?? default(Variant);
	}

	public Variant GetOutputValueByIndex(int index)
	{
		return GetOutputPort(index)?.Value ?? default(Variant);
	}

	public void SetOutputValueByIndex(int index, Variant val)
	{
		XWBPNodePortData outputPort = GetOutputPort(index);
		if (outputPort != null)
		{
			outputPort.Value = val;
		}
	}

	public void SetMetaData(StringName key, Variant value)
	{
		if (MetaData == null)
		{
			Dictionary dictionary = (MetaData = new Dictionary());
		}
		MetaData[key] = value;
	}

	public Variant GetMetaData(StringName key)
	{
		if (MetaData == null || !MetaData.TryGetValue(key, out var value))
		{
			return default;
		}
		return value;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.RebuildPortMap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetInputPort, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetOutputPort, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindInputPortByName, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindOutputPortByName, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetInputValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "val", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetInputValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetOutputValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "val", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetOutputValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetOutputValueByIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetOutputValueByIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "val", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMetaData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMetaData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RebuildPortMap && args.Count == 0)
		{
			RebuildPortMap();
			ret = default;
			return true;
		}
		if (method == MethodName.GetInputPort && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodePortData>(GetInputPort(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetOutputPort && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodePortData>(GetOutputPort(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.FindInputPortByName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodePortData>(FindInputPortByName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindOutputPortByName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodePortData>(FindOutputPortByName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetInputValue && args.Count == 2)
		{
			SetInputValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetInputValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetInputValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetOutputValue && args.Count == 2)
		{
			SetOutputValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetOutputValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetOutputValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetOutputValueByIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetOutputValueByIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetOutputValueByIndex && args.Count == 2)
		{
			SetOutputValueByIndex(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMetaData && args.Count == 2)
		{
			SetMetaData(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMetaData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetMetaData(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.RebuildPortMap)
		{
			return true;
		}
		if (method == MethodName.GetInputPort)
		{
			return true;
		}
		if (method == MethodName.GetOutputPort)
		{
			return true;
		}
		if (method == MethodName.FindInputPortByName)
		{
			return true;
		}
		if (method == MethodName.FindOutputPortByName)
		{
			return true;
		}
		if (method == MethodName.SetInputValue)
		{
			return true;
		}
		if (method == MethodName.GetInputValue)
		{
			return true;
		}
		if (method == MethodName.SetOutputValue)
		{
			return true;
		}
		if (method == MethodName.GetOutputValue)
		{
			return true;
		}
		if (method == MethodName.GetOutputValueByIndex)
		{
			return true;
		}
		if (method == MethodName.SetOutputValueByIndex)
		{
			return true;
		}
		if (method == MethodName.SetMetaData)
		{
			return true;
		}
		if (method == MethodName.GetMetaData)
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
		if (name == PropertyName.MetaData)
		{
			MetaData = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.NodeType)
		{
			NodeType = VariantUtils.ConvertTo<XWBPNodeType>(in value);
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
		if (name == PropertyName.MetaData)
		{
			value = VariantUtils.CreateFrom<Dictionary>(MetaData);
			return true;
		}
		if (name == PropertyName.NodeType)
		{
			value = VariantUtils.CreateFrom<XWBPNodeType>(NodeType);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.Id, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Lock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.TypeId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Position, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Size, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.MetaData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.NodeType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
		info.AddProperty(PropertyName.MetaData, Variant.From<Dictionary>(MetaData));
		info.AddProperty(PropertyName.NodeType, Variant.From<XWBPNodeType>(NodeType));
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
		if (info.TryGetProperty(PropertyName.MetaData, out var value6))
		{
			MetaData = value6.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.NodeType, out var value7))
		{
			NodeType = value7.As<XWBPNodeType>();
		}
	}
}
