using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/NodeTypes/Entry/XWBPNodeMethodEntry.cs")]
public class XWBPNodeMethodEntry : XWBPNodeType
{
	public enum Type
	{
		Bp,
		Script,
		Virtual
	}

	public new class MethodName : XWBPNodeType.MethodName
	{
		public new static readonly StringName CreateNodeData = "CreateNodeData";

		public static readonly StringName WriteNodeMetaData = "WriteNodeMetaData";

		public new static readonly StringName LoadNodeData = "LoadNodeData";

		public static readonly StringName BuildFunction = "BuildFunction";

		public static readonly StringName BuildVirtualMethod = "BuildVirtualMethod";

		public new static readonly StringName GraphNodeBuildRefresh = "GraphNodeBuildRefresh";

		public new static readonly StringName GenerateCode = "GenerateCode";

		public new static readonly StringName GenerateCodeFromGraph = "GenerateCodeFromGraph";
	}

	public new class PropertyName : XWBPNodeType.PropertyName
	{
		public static readonly StringName FunctionId = "FunctionId";

		public static readonly StringName MethodType = "MethodType";

		public static readonly StringName MethodData = "MethodData";
	}

	public new class SignalName : XWBPNodeType.SignalName
	{
	}

	public int FunctionId { get; set; }

	public Type MethodType { get; set; }

	public Dictionary MethodData { get; set; } = new Dictionary();

	public XWBPNodeMethodEntry()
	{
		TypeId = "__XWBPGraphNode_MethodEntry";
		NodeTypeEnumValue = NodeTypeEnum.FunctionEntry;
		Category = "入口";
		DisplayName = "方法入口";
		Color = new Color(0.651f, 0.22f, 0.958f);
	}

	public override XWBPNodeData CreateNodeData()
	{
		XWBPNodeData xWBPNodeData = base.CreateNodeData();
		WriteNodeMetaData(xWBPNodeData);
		return xWBPNodeData;
	}

	private void WriteNodeMetaData(XWBPNodeData nodeData)
	{
		if (nodeData == null)
		{
			return;
		}
		nodeData.SetMetaData("MethodType", (int)MethodType);
		if (MethodType == Type.Bp)
		{
			nodeData.SetMetaData("FunctionId", FunctionId);
		}
		else if (MethodType == Type.Virtual)
		{
			if (MethodData == null)
			{
				Dictionary dictionary = (MethodData = new Dictionary());
			}
			nodeData.SetMetaData("MethodData", MethodData);
		}
	}

	public override void LoadNodeData(XWBPNodeData nodeData)
	{
		MethodType = (Type)nodeData.GetMetaData("MethodType").AsInt32();
		if (MethodType == Type.Bp)
		{
			FunctionId = nodeData.GetMetaData("FunctionId").AsInt32();
		}
		else if (MethodType == Type.Virtual)
		{
			MethodData = nodeData.GetMetaData("MethodData").As<Dictionary>();
		}
	}

	public void BuildFunction(XWBPFunctionData function)
	{
		ClearAllPort();
		AddFlowOutput("");
		foreach (XWBPNodePortData input in function.Inputs)
		{
			AddOutput(input.Name, input.PortTypeValue, input.ClassName, input.DefaultValue);
		}
	}

	public void BuildVirtualMethod()
	{
		ClearAllPort();
		AddFlowOutput("");
		Variant value;
		foreach (Variant item in MethodData.TryGetValue("args", out value) ? value.As<Array>() : new Array())
		{
			Dictionary dictionary = item.As<Dictionary>();
			string portName = (dictionary.TryGetValue("name", out var value2) ? value2.AsString() : "参数");
			int num = (dictionary.TryGetValue("type", out var value3) ? value3.AsInt32() : 0);
			XWBPNodePortData.PortType portType = XWBPNodeCallMethod.ToPortType(num);
			StringName className = "";
			if (num == 24)
			{
				className = (dictionary.TryGetValue("class_name", out var value4) ? value4.AsStringName() : ((StringName)""));
			}
			AddOutput(portName, portType, className);
		}
	}

	public override void GraphNodeBuildRefresh(GodotObject graphNode)
	{
		if (!(graphNode is XWBPGraphNode { NodeData: not null } xWBPGraphNode))
		{
			return;
		}
		if (MethodType == Type.Bp)
		{
			XWBPEditor editor = xWBPGraphNode.Editor;
			if (editor?.BpScriptData?.Functions != null && editor.BpScriptData.Functions.TryGetValue(FunctionId, out var value))
			{
				BuildFunction(value);
			}
		}
		else if (MethodType == Type.Virtual)
		{
			BuildVirtualMethod();
		}
		BuildNodeData(xWBPGraphNode.NodeData);
		WriteNodeMetaData(xWBPGraphNode.NodeData);
	}

	public override void GenerateCode(XWBPCodeGenerator generator, XWBPNodeData node, XWBPFunctionData function)
	{
		XWBPNodeData nextNode = generator.GetNextNode(node, 0, function);
		if (nextNode != null)
		{
			generator.GenerateNodeCode(nextNode, function);
		}
	}

	public override void GenerateCodeFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, XWBPGraphData graph)
	{
		XWBPNodeData nextNodeFromGraph = generator.GetNextNodeFromGraph(node, 0, graph);
		if (nextNodeFromGraph != null)
		{
			generator.GenerateNodeCodeFromGraph(nextNodeFromGraph, graph);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.CreateNodeData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WriteNodeMetaData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadNodeData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildFunction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "function", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildVirtualMethod, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GraphNodeBuildRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graphNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateCode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "generator", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "function", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateCodeFromGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "generator", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "graph", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateNodeData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(CreateNodeData());
			return true;
		}
		if (method == MethodName.WriteNodeMetaData && args.Count == 1)
		{
			WriteNodeMetaData(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadNodeData && args.Count == 1)
		{
			LoadNodeData(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildFunction && args.Count == 1)
		{
			BuildFunction(VariantUtils.ConvertTo<XWBPFunctionData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildVirtualMethod && args.Count == 0)
		{
			BuildVirtualMethod();
			ret = default;
			return true;
		}
		if (method == MethodName.GraphNodeBuildRefresh && args.Count == 1)
		{
			GraphNodeBuildRefresh(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GenerateCode && args.Count == 3)
		{
			GenerateCode(VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[0]), VariantUtils.ConvertTo<XWBPNodeData>(in args[1]), VariantUtils.ConvertTo<XWBPFunctionData>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GenerateCodeFromGraph && args.Count == 3)
		{
			GenerateCodeFromGraph(VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[0]), VariantUtils.ConvertTo<XWBPNodeData>(in args[1]), VariantUtils.ConvertTo<XWBPGraphData>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.CreateNodeData)
		{
			return true;
		}
		if (method == MethodName.WriteNodeMetaData)
		{
			return true;
		}
		if (method == MethodName.LoadNodeData)
		{
			return true;
		}
		if (method == MethodName.BuildFunction)
		{
			return true;
		}
		if (method == MethodName.BuildVirtualMethod)
		{
			return true;
		}
		if (method == MethodName.GraphNodeBuildRefresh)
		{
			return true;
		}
		if (method == MethodName.GenerateCode)
		{
			return true;
		}
		if (method == MethodName.GenerateCodeFromGraph)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.FunctionId)
		{
			FunctionId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.MethodType)
		{
			MethodType = VariantUtils.ConvertTo<Type>(in value);
			return true;
		}
		if (name == PropertyName.MethodData)
		{
			MethodData = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.FunctionId)
		{
			value = VariantUtils.CreateFrom<int>(FunctionId);
			return true;
		}
		if (name == PropertyName.MethodType)
		{
			value = VariantUtils.CreateFrom<Type>(MethodType);
			return true;
		}
		if (name == PropertyName.MethodData)
		{
			value = VariantUtils.CreateFrom<Dictionary>(MethodData);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.FunctionId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.MethodType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.MethodData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.FunctionId, Variant.From<int>(FunctionId));
		info.AddProperty(PropertyName.MethodType, Variant.From<Type>(MethodType));
		info.AddProperty(PropertyName.MethodData, Variant.From<Dictionary>(MethodData));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.FunctionId, out var value))
		{
			FunctionId = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.MethodType, out var value2))
		{
			MethodType = value2.As<Type>();
		}
		if (info.TryGetProperty(PropertyName.MethodData, out var value3))
		{
			MethodData = value3.As<Dictionary>();
		}
	}
}
