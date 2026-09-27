using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/XWBPGraphData.cs")]
public class XWBPGraphData : RefCounted
{
	[Signal]
	public delegate void RemoveEventHandler();

	[Signal]
	public delegate void RenameEventHandler(string name);

	[Signal]
	public delegate void GraphChangeEventHandler();

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName AddNode = "AddNode";

		public static readonly StringName AddNodePreserveId = "AddNodePreserveId";

		public static readonly StringName RemoveNode = "RemoveNode";

		public static readonly StringName GetNode = "GetNode";

		public static readonly StringName AddConnection = "AddConnection";

		public static readonly StringName AddConnectionIndex = "AddConnectionIndex";

		public static readonly StringName RemoveNodeConnection = "RemoveNodeConnection";

		public static readonly StringName RemoveConnection = "RemoveConnection";

		public static readonly StringName RebuildConnectionIndex = "RebuildConnectionIndex";

		public static readonly StringName ContainsConnection = "ContainsConnection";

		public static readonly StringName CanConnect = "CanConnect";

		public static readonly StringName IsConnectionStructurallyValid = "IsConnectionStructurallyValid";

		public static readonly StringName SanitizeConnections = "SanitizeConnections";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName FindEntryNode = "FindEntryNode";

		public static readonly StringName HasVirtualMethodEntry = "HasVirtualMethodEntry";

		public static readonly StringName RemoveSelf = "RemoveSelf";

		public static readonly StringName RenameSelf = "RenameSelf";

		public static readonly StringName NameSet = "NameSet";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Owner = "Owner";

		public static readonly StringName Id = "Id";

		public static readonly StringName Name = "Name";

		public static readonly StringName NextNodeId = "NextNodeId";

		public static readonly StringName Lock = "Lock";
	}

	public new class SignalName : RefCounted.SignalName
	{
		public static readonly StringName Remove = "Remove";

		public static readonly StringName Rename = "Rename";

		public static readonly StringName GraphChange = "GraphChange";
	}

	private readonly System.Collections.Generic.Dictionary<int, List<XWBPNodeConnectionData>> _connectionsFrom = new System.Collections.Generic.Dictionary<int, List<XWBPNodeConnectionData>>();

	private readonly System.Collections.Generic.Dictionary<int, List<XWBPNodeConnectionData>> _connectionsTo = new System.Collections.Generic.Dictionary<int, List<XWBPNodeConnectionData>>();

	private RemoveEventHandler backing_Remove;

	private RenameEventHandler backing_Rename;

	private GraphChangeEventHandler backing_GraphChange;

	public XWBPScriptData Owner { get; set; }

	public int Id { get; set; }

	public string Name { get; set; } = "新图表";

	public System.Collections.Generic.Dictionary<int, XWBPNodeData> Nodes { get; set; } = new System.Collections.Generic.Dictionary<int, XWBPNodeData>();

	public List<XWBPNodeConnectionData> Connections { get; set; } = new List<XWBPNodeConnectionData>();

	public int NextNodeId { get; set; } = 1;

	public bool Lock { get; set; }

	public event RemoveEventHandler Remove
	{
		add
		{
			backing_Remove = (RemoveEventHandler)Delegate.Combine(backing_Remove, value);
		}
		remove
		{
			backing_Remove = (RemoveEventHandler)Delegate.Remove(backing_Remove, value);
		}
	}

	public event RenameEventHandler Rename
	{
		add
		{
			backing_Rename = (RenameEventHandler)Delegate.Combine(backing_Rename, value);
		}
		remove
		{
			backing_Rename = (RenameEventHandler)Delegate.Remove(backing_Rename, value);
		}
	}

	public event GraphChangeEventHandler GraphChange
	{
		add
		{
			backing_GraphChange = (GraphChangeEventHandler)Delegate.Combine(backing_GraphChange, value);
		}
		remove
		{
			backing_GraphChange = (GraphChangeEventHandler)Delegate.Remove(backing_GraphChange, value);
		}
	}

	public void AddNode(XWBPNodeData nodeData)
	{
		if (nodeData != null)
		{
			if (NextNodeId < 1)
			{
				NextNodeId = 1;
			}
			while (Nodes.ContainsKey(NextNodeId))
			{
				NextNodeId++;
			}
			nodeData.Id = NextNodeId;
			NextNodeId++;
			nodeData.RebuildPortMap();
			Nodes[nodeData.Id] = nodeData;
			EmitSignal(SignalName.GraphChange);
		}
	}

	public void AddNodePreserveId(XWBPNodeData nodeData)
	{
		if (nodeData == null)
		{
			return;
		}
		if (nodeData.Id <= 0 || Nodes.ContainsKey(nodeData.Id))
		{
			AddNode(nodeData);
			return;
		}
		nodeData.RebuildPortMap();
		Nodes[nodeData.Id] = nodeData;
		if (NextNodeId <= nodeData.Id)
		{
			NextNodeId = nodeData.Id + 1;
		}
		EmitSignal(SignalName.GraphChange);
	}

	public void RemoveNode(int nodeId)
	{
		int count = Connections.Count;
		RemoveNodeConnection(nodeId);
		if (Nodes.Remove(nodeId) || count != Connections.Count)
		{
			EmitSignal(SignalName.GraphChange);
		}
	}

	public XWBPNodeData GetNode(int nodeId)
	{
		if (!Nodes.TryGetValue(nodeId, out var value))
		{
			return null;
		}
		return value;
	}

	public bool AddConnection(int fromNodeId, int fromPortIndex, int toNodeId, int toPortIndex)
	{
		if (!CanConnect(fromNodeId, fromPortIndex, toNodeId, toPortIndex))
		{
			return false;
		}
		XWBPNodeConnectionData xWBPNodeConnectionData = new XWBPNodeConnectionData(fromNodeId, fromPortIndex, toNodeId, toPortIndex);
		Connections.Add(xWBPNodeConnectionData);
		AddConnectionIndex(xWBPNodeConnectionData);
		EmitSignal(SignalName.GraphChange);
		return true;
	}

	private void AddConnectionIndex(XWBPNodeConnectionData connection)
	{
		if (connection != null)
		{
			if (!_connectionsFrom.TryGetValue(connection.FromNodeId, out var value))
			{
				value = new List<XWBPNodeConnectionData>();
				_connectionsFrom[connection.FromNodeId] = value;
			}
			value.Add(connection);
			if (!_connectionsTo.TryGetValue(connection.ToNodeId, out var value2))
			{
				value2 = new List<XWBPNodeConnectionData>();
				_connectionsTo[connection.ToNodeId] = value2;
			}
			value2.Add(connection);
		}
	}

	public void RemoveNodeConnection(int nodeId)
	{
		Connections = Connections.Where((XWBPNodeConnectionData c) => c != null && c.FromNodeId != nodeId && c.ToNodeId != nodeId).ToList();
		RebuildConnectionIndex();
	}

	public void RemoveConnection(int fromNodeId, int fromPortIndex, int toNodeId, int toPortIndex)
	{
		XWBPNodeConnectionData target = new XWBPNodeConnectionData(fromNodeId, fromPortIndex, toNodeId, toPortIndex);
		Connections = Connections.Where((XWBPNodeConnectionData c) => c != null && !c.Equals(target)).ToList();
		RebuildConnectionIndex();
		EmitSignal(SignalName.GraphChange);
	}

	public void RebuildConnectionIndex()
	{
		_connectionsFrom.Clear();
		_connectionsTo.Clear();
		foreach (XWBPNodeConnectionData connection in Connections)
		{
			if (connection != null)
			{
				AddConnectionIndex(connection);
			}
		}
	}

	private bool ContainsConnection(XWBPNodeConnectionData target)
	{
		if (target == null)
		{
			return false;
		}
		foreach (XWBPNodeConnectionData connection in Connections)
		{
			if (connection != null && connection.Equals(target))
			{
				return true;
			}
		}
		return false;
	}

	public List<XWBPNodeConnectionData> GetConnectionsToNode(int nodeId)
	{
		if (_connectionsTo.TryGetValue(nodeId, out var value))
		{
			return new List<XWBPNodeConnectionData>(value);
		}
		return new List<XWBPNodeConnectionData>();
	}

	public List<XWBPNodeConnectionData> GetConnectionsFromNode(int nodeId)
	{
		if (_connectionsFrom.TryGetValue(nodeId, out var value))
		{
			return new List<XWBPNodeConnectionData>(value);
		}
		return new List<XWBPNodeConnectionData>();
	}

	public List<XWBPNodeConnectionData> GetInputConnections(int nodeId, int portIndex)
	{
		List<XWBPNodeConnectionData> list = new List<XWBPNodeConnectionData>();
		if (_connectionsTo.TryGetValue(nodeId, out var value))
		{
			foreach (XWBPNodeConnectionData item in value)
			{
				if (item != null && item.ToPortIndex == portIndex)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	public List<XWBPNodeConnectionData> GetOutputConnections(int nodeId, int portIndex)
	{
		List<XWBPNodeConnectionData> list = new List<XWBPNodeConnectionData>();
		if (_connectionsFrom.TryGetValue(nodeId, out var value))
		{
			foreach (XWBPNodeConnectionData item in value)
			{
				if (item != null && item.FromPortIndex == portIndex)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	public bool CanConnect(int fromNodeId, int fromPortIndex, int toNodeId, int toPortIndex)
	{
		XWBPNodeConnectionData xWBPNodeConnectionData = new XWBPNodeConnectionData(fromNodeId, fromPortIndex, toNodeId, toPortIndex);
		if (ContainsConnection(xWBPNodeConnectionData))
		{
			return false;
		}
		return IsConnectionStructurallyValid(xWBPNodeConnectionData);
	}

	public bool IsConnectionStructurallyValid(XWBPNodeConnectionData connection, bool ignoreOccupiedPorts = false)
	{
		if (connection == null)
		{
			return false;
		}
		if (connection.FromNodeId == connection.ToNodeId)
		{
			return false;
		}
		XWBPNodeData node = GetNode(connection.FromNodeId);
		XWBPNodeData node2 = GetNode(connection.ToNodeId);
		if (node == null || node2 == null)
		{
			return false;
		}
		XWBPNodePortData outputPort = node.GetOutputPort(connection.FromPortIndex);
		XWBPNodePortData inputPort = node2.GetInputPort(connection.ToPortIndex);
		if (outputPort == null || inputPort == null)
		{
			return false;
		}
		if (outputPort.PortDirection != XWBPNodePortData.Direction.Output || inputPort.PortDirection != XWBPNodePortData.Direction.Input)
		{
			return false;
		}
		if (!outputPort.CanConnectTo(inputPort))
		{
			return false;
		}
		if (ignoreOccupiedPorts)
		{
			return true;
		}
		if (GetInputConnections(connection.ToNodeId, connection.ToPortIndex).Count > 0)
		{
			return false;
		}
		if (outputPort.PortTypeValue == XWBPNodePortData.PortType.Flow && GetOutputConnections(connection.FromNodeId, connection.FromPortIndex).Count > 0)
		{
			return false;
		}
		return true;
	}

	public void SanitizeConnections()
	{
		List<XWBPNodeConnectionData> connections = Connections;
		Connections = new List<XWBPNodeConnectionData>();
		_connectionsFrom.Clear();
		_connectionsTo.Clear();
		foreach (XWBPNodeConnectionData item in connections)
		{
			if (IsConnectionStructurallyValid(item) && !ContainsConnection(item))
			{
				Connections.Add(item);
				AddConnectionIndex(item);
			}
		}
	}

	public void Clear()
	{
		Nodes.Clear();
		Connections.Clear();
		_connectionsFrom.Clear();
		_connectionsTo.Clear();
		NextNodeId = 1;
		EmitSignal(SignalName.GraphChange);
	}

	public XWBPNodeData FindEntryNode()
	{
		foreach (XWBPNodeData value in Nodes.Values)
		{
			if (value != null && value.TypeId == (StringName)"__XWBPGraphNode_Entry")
			{
				return value;
			}
		}
		return null;
	}

	public bool HasVirtualMethodEntry(StringName methodName, int excludeNodeId = -1)
	{
		foreach (XWBPNodeData value2 in Nodes.Values)
		{
			if (value2 != null && value2.Id != excludeNodeId && value2.TypeId == (StringName)"__XWBPGraphNode_MethodEntry" && value2.GetMetaData("MethodType").AsInt32() == 2 && value2.GetMetaData("MethodData").As<Dictionary>().TryGetValue("name", out var value) && value.AsStringName() == methodName)
			{
				return true;
			}
		}
		return false;
	}

	public void RemoveSelf()
	{
		EmitSignal(SignalName.Remove);
	}

	public void RenameSelf(string newName)
	{
		Name = newName;
		EmitSignal(SignalName.Rename, newName);
	}

	public void NameSet()
	{
		EmitSignal(SignalName.Rename, Name);
	}

	public virtual List<XWInspectorProperty> GetProperties()
	{
		return new List<XWInspectorProperty>();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName.AddNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddNodePreserveId, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddConnection, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "fromNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddConnectionIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "connection", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveNodeConnection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveConnection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "fromNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildConnectionIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ContainsConnection, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanConnect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "fromNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsConnectionStructurallyValid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "connection", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "ignoreOccupiedPorts", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SanitizeConnections, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindEntryNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasVirtualMethodEntry, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "methodName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "excludeNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelf, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenameSelf, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NameSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AddNode && args.Count == 1)
		{
			AddNode(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddNodePreserveId && args.Count == 1)
		{
			AddNodePreserveId(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveNode && args.Count == 1)
		{
			RemoveNode(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(GetNode(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.AddConnection && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(AddConnection(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		if (method == MethodName.AddConnectionIndex && args.Count == 1)
		{
			AddConnectionIndex(VariantUtils.ConvertTo<XWBPNodeConnectionData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveNodeConnection && args.Count == 1)
		{
			RemoveNodeConnection(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveConnection && args.Count == 4)
		{
			RemoveConnection(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildConnectionIndex && args.Count == 0)
		{
			RebuildConnectionIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.ContainsConnection && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsConnection(VariantUtils.ConvertTo<XWBPNodeConnectionData>(in args[0])));
			return true;
		}
		if (method == MethodName.CanConnect && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(CanConnect(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		if (method == MethodName.IsConnectionStructurallyValid && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsConnectionStructurallyValid(VariantUtils.ConvertTo<XWBPNodeConnectionData>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.SanitizeConnections && args.Count == 0)
		{
			SanitizeConnections();
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.FindEntryNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(FindEntryNode());
			return true;
		}
		if (method == MethodName.HasVirtualMethodEntry && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasVirtualMethodEntry(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.RemoveSelf && args.Count == 0)
		{
			RemoveSelf();
			ret = default;
			return true;
		}
		if (method == MethodName.RenameSelf && args.Count == 1)
		{
			RenameSelf(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NameSet && args.Count == 0)
		{
			NameSet();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.AddNode)
		{
			return true;
		}
		if (method == MethodName.AddNodePreserveId)
		{
			return true;
		}
		if (method == MethodName.RemoveNode)
		{
			return true;
		}
		if (method == MethodName.GetNode)
		{
			return true;
		}
		if (method == MethodName.AddConnection)
		{
			return true;
		}
		if (method == MethodName.AddConnectionIndex)
		{
			return true;
		}
		if (method == MethodName.RemoveNodeConnection)
		{
			return true;
		}
		if (method == MethodName.RemoveConnection)
		{
			return true;
		}
		if (method == MethodName.RebuildConnectionIndex)
		{
			return true;
		}
		if (method == MethodName.ContainsConnection)
		{
			return true;
		}
		if (method == MethodName.CanConnect)
		{
			return true;
		}
		if (method == MethodName.IsConnectionStructurallyValid)
		{
			return true;
		}
		if (method == MethodName.SanitizeConnections)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.FindEntryNode)
		{
			return true;
		}
		if (method == MethodName.HasVirtualMethodEntry)
		{
			return true;
		}
		if (method == MethodName.RemoveSelf)
		{
			return true;
		}
		if (method == MethodName.RenameSelf)
		{
			return true;
		}
		if (method == MethodName.NameSet)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Owner)
		{
			Owner = VariantUtils.ConvertTo<XWBPScriptData>(in value);
			return true;
		}
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
		if (name == PropertyName.Owner)
		{
			value = VariantUtils.CreateFrom<XWBPScriptData>(Owner);
			return true;
		}
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
			new PropertyInfo(Variant.Type.Object, PropertyName.Owner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.Id, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.NextNodeId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Lock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Owner, Variant.From<XWBPScriptData>(Owner));
		info.AddProperty(PropertyName.Id, Variant.From<int>(Id));
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
		info.AddProperty(PropertyName.NextNodeId, Variant.From<int>(NextNodeId));
		info.AddProperty(PropertyName.Lock, Variant.From<bool>(Lock));
		info.AddSignalEventDelegate(SignalName.Remove, backing_Remove);
		info.AddSignalEventDelegate(SignalName.Rename, backing_Rename);
		info.AddSignalEventDelegate(SignalName.GraphChange, backing_GraphChange);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Owner, out var value))
		{
			Owner = value.As<XWBPScriptData>();
		}
		if (info.TryGetProperty(PropertyName.Id, out var value2))
		{
			Id = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Name, out var value3))
		{
			Name = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.NextNodeId, out var value4))
		{
			NextNodeId = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Lock, out var value5))
		{
			Lock = value5.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<RemoveEventHandler>(SignalName.Remove, out var value6))
		{
			backing_Remove = value6;
		}
		if (info.TryGetSignalEventDelegate<RenameEventHandler>(SignalName.Rename, out var value7))
		{
			backing_Rename = value7;
		}
		if (info.TryGetSignalEventDelegate<GraphChangeEventHandler>(SignalName.GraphChange, out var value8))
		{
			backing_GraphChange = value8;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(SignalName.Remove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.Rename, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.GraphChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalRemove()
	{
		EmitSignal(SignalName.Remove, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalRename(string name)
	{
		EmitSignal(SignalName.Rename, new ReadOnlySpan<Variant>((Variant)name));
	}

	protected void EmitSignalGraphChange()
	{
		EmitSignal(SignalName.GraphChange, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.Remove && args.Count == 0)
		{
			backing_Remove?.Invoke();
		}
		else if (signal == SignalName.Rename && args.Count == 1)
		{
			backing_Rename?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else if (signal == SignalName.GraphChange && args.Count == 0)
		{
			backing_GraphChange?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.Remove)
		{
			return true;
		}
		if (signal == SignalName.Rename)
		{
			return true;
		}
		if (signal == SignalName.GraphChange)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
