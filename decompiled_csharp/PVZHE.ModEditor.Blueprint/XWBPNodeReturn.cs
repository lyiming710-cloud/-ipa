using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/NodeTypes/Flow/XWBPNodeReturn.cs")]
public class XWBPNodeReturn : XWBPNodeType
{
	public new class MethodName : XWBPNodeType.MethodName
	{
		public static readonly StringName BuildReturnPorts = "BuildReturnPorts";

		public new static readonly StringName GenerateCode = "GenerateCode";

		public new static readonly StringName GenerateCodeFromGraph = "GenerateCodeFromGraph";
	}

	public new class PropertyName : XWBPNodeType.PropertyName
	{
	}

	public new class SignalName : XWBPNodeType.SignalName
	{
	}

	public XWBPNodeReturn()
	{
		TypeId = "__XWBPGraphNode_Return";
		Category = "流程控制";
		DisplayName = "返回";
		Description = "提前返回函数";
		Color = new Color(0.8f, 0.2f, 0.2f);
		AddFlowInput();
	}

	public void BuildReturnPorts(XWBPFunctionData functionData)
	{
		ClearAllPort();
		AddFlowInput();
		foreach (XWBPNodePortData output in functionData.Outputs)
		{
			AddInput(output.Name, output.PortTypeValue, output.ClassName, output.DefaultValue);
		}
	}

	public override void GenerateCode(XWBPCodeGenerator generator, XWBPNodeData node, XWBPFunctionData function)
	{
		List<XWBPNodePortData> list = new List<XWBPNodePortData>();
		foreach (XWBPNodePortData inputPort in node.InputPorts)
		{
			if (inputPort.PortTypeValue != XWBPNodePortData.PortType.Flow)
			{
				list.Add(inputPort);
			}
		}
		if (list.Count == 0)
		{
			generator.AddLine("return;");
			return;
		}
		if (list.Count == 1)
		{
			string inputValue = generator.GetInputValue(node, list[0].Name, function);
			generator.AddLine("return " + inputValue + ";");
			return;
		}
		List<string> list2 = new List<string>();
		foreach (XWBPNodePortData item in list)
		{
			list2.Add(generator.GetInputValue(node, item.Name, function));
		}
		generator.AddLine("return new object[] { " + string.Join(", ", list2) + " };");
	}

	public override void GenerateCodeFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, XWBPGraphData graph)
	{
		List<XWBPNodePortData> list = new List<XWBPNodePortData>();
		foreach (XWBPNodePortData inputPort in node.InputPorts)
		{
			if (inputPort.PortTypeValue != XWBPNodePortData.PortType.Flow)
			{
				list.Add(inputPort);
			}
		}
		if (list.Count == 0)
		{
			generator.AddLine("return;");
			return;
		}
		if (list.Count == 1)
		{
			string inputValueFromGraph = generator.GetInputValueFromGraph(node, list[0].Name, graph);
			generator.AddLine("return " + inputValueFromGraph + ";");
			return;
		}
		List<string> list2 = new List<string>();
		foreach (XWBPNodePortData item in list)
		{
			list2.Add(generator.GetInputValueFromGraph(node, item.Name, graph));
		}
		generator.AddLine("return new object[] { " + string.Join(", ", list2) + " };");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.BuildReturnPorts, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "functionData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
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
		if (method == MethodName.BuildReturnPorts && args.Count == 1)
		{
			BuildReturnPorts(VariantUtils.ConvertTo<XWBPFunctionData>(in args[0]));
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
		if (method == MethodName.BuildReturnPorts)
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
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
