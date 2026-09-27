using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/NodeTypes/Variable/XWBPNodeLocalVariable.cs")]
public class XWBPNodeLocalVariable : XWBPNodeType
{
	public new class MethodName : XWBPNodeType.MethodName
	{
		public new static readonly StringName CreateNodeData = "CreateNodeData";

		public new static readonly StringName LoadNodeData = "LoadNodeData";

		public new static readonly StringName GetOutputValue = "GetOutputValue";

		public new static readonly StringName GetOutputValueFromGraph = "GetOutputValueFromGraph";

		public new static readonly StringName GenerateCode = "GenerateCode";

		public new static readonly StringName GenerateCodeFromGraph = "GenerateCodeFromGraph";
	}

	public new class PropertyName : XWBPNodeType.PropertyName
	{
		public static readonly StringName VarName = "VarName";

		public static readonly StringName VarType = "VarType";
	}

	public new class SignalName : XWBPNodeType.SignalName
	{
	}

	public StringName VarName { get; set; } = "局部变量";

	public int VarType { get; set; }

	public XWBPNodeLocalVariable()
	{
		TypeId = "__XWBPGraphNode_LocalVariable";
		Category = "变量";
		DisplayName = "局部变量";
		Description = "声明并获取局部变量";
		Color = new Color(0.4f, 0.6f, 0.3f);
		AddInput("值", XWBPNodePortData.PortType.Any);
		AddOutput("值", XWBPNodePortData.PortType.Any);
	}

	public override XWBPNodeData CreateNodeData()
	{
		XWBPNodeData xWBPNodeData = base.CreateNodeData();
		xWBPNodeData.SetMetaData("VarName", VarName);
		xWBPNodeData.SetMetaData("VarType", VarType);
		return xWBPNodeData;
	}

	public override void LoadNodeData(XWBPNodeData nodeData)
	{
		VarName = nodeData.GetMetaData("VarName").AsStringName();
		VarType = nodeData.GetMetaData("VarType").AsInt32();
	}

	public override string GetOutputValue(XWBPCodeGenerator generator, XWBPNodeData node, string portName, XWBPFunctionData function)
	{
		if (portName == "值")
		{
			string text = $"local_{node.Id}";
			generator.NodeVariables[node.Id] = text;
			string inputValue = generator.GetInputValue(node, "值", function);
			generator.AddLine($"var {text} = {inputValue};");
			return text;
		}
		return "null";
	}

	public override string GetOutputValueFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, string portName, XWBPGraphData graph)
	{
		if (portName == "值")
		{
			string text = $"local_{node.Id}";
			generator.NodeVariables[node.Id] = text;
			string inputValueFromGraph = generator.GetInputValueFromGraph(node, "值", graph);
			generator.AddLine($"var {text} = {inputValueFromGraph};");
			return text;
		}
		return "null";
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
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.CreateNodeData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadNodeData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetOutputValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "generator", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "function", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetOutputValueFromGraph, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "generator", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "graph", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
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
		if (method == MethodName.LoadNodeData && args.Count == 1)
		{
			LoadNodeData(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetOutputValue && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<string>(GetOutputValue(VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[0]), VariantUtils.ConvertTo<XWBPNodeData>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<XWBPFunctionData>(in args[3])));
			return true;
		}
		if (method == MethodName.GetOutputValueFromGraph && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<string>(GetOutputValueFromGraph(VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[0]), VariantUtils.ConvertTo<XWBPNodeData>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<XWBPGraphData>(in args[3])));
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
		if (method == MethodName.LoadNodeData)
		{
			return true;
		}
		if (method == MethodName.GetOutputValue)
		{
			return true;
		}
		if (method == MethodName.GetOutputValueFromGraph)
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
		if (name == PropertyName.VarName)
		{
			VarName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.VarType)
		{
			VarType = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.VarName)
		{
			value = VariantUtils.CreateFrom<StringName>(VarName);
			return true;
		}
		if (name == PropertyName.VarType)
		{
			value = VariantUtils.CreateFrom<int>(VarType);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.VarName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VarType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.VarName, Variant.From<StringName>(VarName));
		info.AddProperty(PropertyName.VarType, Variant.From<int>(VarType));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.VarName, out var value))
		{
			VarName = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.VarType, out var value2))
		{
			VarType = value2.As<int>();
		}
	}
}
