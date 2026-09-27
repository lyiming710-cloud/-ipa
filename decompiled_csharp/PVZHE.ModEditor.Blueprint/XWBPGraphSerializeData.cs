using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/Resource/XWBPGraphSerializeData.cs")]
public class XWBPGraphSerializeData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Serialize = "Serialize";

		public static readonly StringName Deserialize = "Deserialize";

		public static readonly StringName Clear = "Clear";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Id = "Id";

		public static readonly StringName Name = "Name";

		public static readonly StringName NextNodeId = "NextNodeId";

		public static readonly StringName NodesData = "NodesData";

		public static readonly StringName ConnectionsData = "ConnectionsData";

		public static readonly StringName Lock = "Lock";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int Id { get; set; }

	[Export(PropertyHint.None, "")]
	public string Name { get; set; } = "图表";

	[Export(PropertyHint.None, "")]
	public int NextNodeId { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public Array<XWBPNodeSerializeData> NodesData { get; set; } = new Array<XWBPNodeSerializeData>();

	[Export(PropertyHint.None, "")]
	public Array<XWBPNodeConnectionSerializeData> ConnectionsData { get; set; } = new Array<XWBPNodeConnectionSerializeData>();

	[Export(PropertyHint.None, "")]
	public bool Lock { get; set; }

	public virtual void Serialize(XWBPGraphData data)
	{
		Clear();
		if (data == null)
		{
			return;
		}
		Id = data.Id;
		Name = data.Name ?? "";
		NextNodeId = data.NextNodeId;
		foreach (XWBPNodeData value in data.Nodes.Values)
		{
			if (value != null)
			{
				XWBPNodeSerializeData xWBPNodeSerializeData = new XWBPNodeSerializeData();
				xWBPNodeSerializeData.Serialize(value);
				NodesData.Add(xWBPNodeSerializeData);
			}
		}
		foreach (XWBPNodeConnectionData connection in data.Connections)
		{
			if (connection != null)
			{
				XWBPNodeConnectionSerializeData xWBPNodeConnectionSerializeData = new XWBPNodeConnectionSerializeData();
				xWBPNodeConnectionSerializeData.Serialize(connection);
				ConnectionsData.Add(xWBPNodeConnectionSerializeData);
			}
		}
		Lock = data.Lock;
	}

	public virtual XWBPGraphData Deserialize()
	{
		XWBPGraphData xWBPGraphData = new XWBPGraphData
		{
			Id = Id,
			Name = Name,
			NextNodeId = NextNodeId,
			Lock = Lock
		};
		int num = 0;
		foreach (XWBPNodeSerializeData nodesDatum in NodesData)
		{
			if (nodesDatum == null)
			{
				continue;
			}
			XWBPNodeData xWBPNodeData = nodesDatum.Deserialize();
			if (xWBPNodeData != null && xWBPNodeData.Id != 0 && !xWBPGraphData.Nodes.ContainsKey(xWBPNodeData.Id))
			{
				xWBPGraphData.Nodes[xWBPNodeData.Id] = xWBPNodeData;
				if (xWBPNodeData.Id > num)
				{
					num = xWBPNodeData.Id;
				}
			}
		}
		foreach (XWBPNodeConnectionSerializeData connectionsDatum in ConnectionsData)
		{
			if (connectionsDatum != null)
			{
				XWBPNodeConnectionData xWBPNodeConnectionData = connectionsDatum.Deserialize();
				if (xWBPNodeConnectionData != null)
				{
					xWBPGraphData.Connections.Add(xWBPNodeConnectionData);
				}
			}
		}
		if (xWBPGraphData.NextNodeId <= num)
		{
			xWBPGraphData.NextNodeId = num + 1;
		}
		if (xWBPGraphData.NextNodeId < 1)
		{
			xWBPGraphData.NextNodeId = 1;
		}
		xWBPGraphData.SanitizeConnections();
		return xWBPGraphData;
	}

	public virtual void Clear()
	{
		NextNodeId = 1;
		NodesData.Clear();
		ConnectionsData.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Serialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Deserialize, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Serialize && args.Count == 1)
		{
			Serialize(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Deserialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(Deserialize());
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
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
		if (method == MethodName.Clear)
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
		if (name == PropertyName.NextNodeId)
		{
			NextNodeId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.NodesData)
		{
			NodesData = VariantUtils.ConvertToArray<XWBPNodeSerializeData>(in value);
			return true;
		}
		if (name == PropertyName.ConnectionsData)
		{
			ConnectionsData = VariantUtils.ConvertToArray<XWBPNodeConnectionSerializeData>(in value);
			return true;
		}
		if (name == PropertyName.Lock)
		{
			Lock = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.NextNodeId)
		{
			from = NextNodeId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.NodesData)
		{
			value = VariantUtils.CreateFromArray(NodesData);
			return true;
		}
		if (name == PropertyName.ConnectionsData)
		{
			value = VariantUtils.CreateFromArray(ConnectionsData);
			return true;
		}
		if (name == PropertyName.Lock)
		{
			value = VariantUtils.CreateFrom<bool>(Lock);
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
			new PropertyInfo(Variant.Type.Int, PropertyName.NextNodeId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.NodesData, PropertyHint.TypeString, "24/17:XWBPNodeSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.ConnectionsData, PropertyHint.TypeString, "24/17:XWBPNodeConnectionSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Lock, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Id, Variant.From<int>(Id));
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
		info.AddProperty(PropertyName.NextNodeId, Variant.From<int>(NextNodeId));
		info.AddProperty(PropertyName.NodesData, Variant.CreateFrom(NodesData));
		info.AddProperty(PropertyName.ConnectionsData, Variant.CreateFrom(ConnectionsData));
		info.AddProperty(PropertyName.Lock, Variant.From<bool>(Lock));
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
		if (info.TryGetProperty(PropertyName.NextNodeId, out var value3))
		{
			NextNodeId = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.NodesData, out var value4))
		{
			NodesData = value4.AsGodotArray<XWBPNodeSerializeData>();
		}
		if (info.TryGetProperty(PropertyName.ConnectionsData, out var value5))
		{
			ConnectionsData = value5.AsGodotArray<XWBPNodeConnectionSerializeData>();
		}
		if (info.TryGetProperty(PropertyName.Lock, out var value6))
		{
			Lock = value6.As<bool>();
		}
	}
}
