using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/NodeTypes/Utilities/XWBPNodeLiteral.cs")]
public class XWBPNodeLiteral : XWBPNodeType
{
	public new class MethodName : XWBPNodeType.MethodName
	{
		public new static readonly StringName GenerateCode = "GenerateCode";

		public new static readonly StringName GenerateCodeFromGraph = "GenerateCodeFromGraph";

		public new static readonly StringName GetOutputValue = "GetOutputValue";

		public new static readonly StringName GetOutputValueFromGraph = "GetOutputValueFromGraph";
	}

	public new class PropertyName : XWBPNodeType.PropertyName
	{
	}

	public new class SignalName : XWBPNodeType.SignalName
	{
	}

	public XWBPNodeLiteral()
	{
		TypeId = "__XWBPGraphNode_Literal";
		Category = "实用工具";
		DisplayName = "字面量";
		Description = "输出常量值";
		Color = new Color(0.3f, 0.5f, 0.3f);
		AddOutput("值", XWBPNodePortData.PortType.Any);
	}

	public override void GenerateCode(XWBPCodeGenerator generator, XWBPNodeData node, XWBPFunctionData function)
	{
	}

	public override void GenerateCodeFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, XWBPGraphData graph)
	{
	}

	public override string GetOutputValue(XWBPCodeGenerator generator, XWBPNodeData node, string portName, XWBPFunctionData function)
	{
		if (portName == "值")
		{
			XWBPNodePortData xWBPNodePortData = node.FindOutputPortByName("值");
			if (xWBPNodePortData != null)
			{
				return generator.GetDefaultValueString(xWBPNodePortData.Value, (int)xWBPNodePortData.PortTypeValue);
			}
		}
		return "null";
	}

	public override string GetOutputValueFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, string portName, XWBPGraphData graph)
	{
		if (portName == "值")
		{
			XWBPNodePortData xWBPNodePortData = node.FindOutputPortByName("值");
			if (xWBPNodePortData != null)
			{
				return generator.GetDefaultValueString(xWBPNodePortData.Value, (int)xWBPNodePortData.PortTypeValue);
			}
		}
		return "null";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
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
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GenerateCode)
		{
			return true;
		}
		if (method == MethodName.GenerateCodeFromGraph)
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
