using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/NodeTypes/Flow/XWBPNodeMatch.cs")]
public class XWBPNodeMatch : XWBPNodeType
{
	public new class MethodName : XWBPNodeType.MethodName
	{
		public new static readonly StringName GenerateCode = "GenerateCode";

		public new static readonly StringName GenerateCodeFromGraph = "GenerateCodeFromGraph";

		public static readonly StringName EmitCases = "EmitCases";
	}

	public new class PropertyName : XWBPNodeType.PropertyName
	{
	}

	public new class SignalName : XWBPNodeType.SignalName
	{
	}

	public XWBPNodeMatch()
	{
		TypeId = "__XWBPGraphNode_Match";
		Category = "流程控制";
		DisplayName = "匹配";
		Description = "根据值匹配执行不同分支";
		Color = new Color(0.2f, 0.4f, 0.6f);
		AddFlowInput();
		AddInput("值", XWBPNodePortData.PortType.Any);
		AddFlowOutput("默认");
	}

	public override void GenerateCode(XWBPCodeGenerator generator, XWBPNodeData node, XWBPFunctionData function)
	{
		string inputValue = generator.GetInputValue(node, "值", function);
		generator.AddLine("switch (" + inputValue + ")");
		generator.AddLine("{");
		generator.IndentLevel++;
		EmitCases(generator, node, function, isGraph: false);
		generator.AddLine("default:");
		generator.IndentLevel++;
		XWBPNodeData nextNode = generator.GetNextNode(node, 0, function);
		if (nextNode != null)
		{
			generator.GenerateNodeCode(nextNode, function);
		}
		generator.AddLine("break;");
		generator.IndentLevel--;
		generator.IndentLevel--;
		generator.AddLine("}");
	}

	public override void GenerateCodeFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, XWBPGraphData graph)
	{
		string inputValueFromGraph = generator.GetInputValueFromGraph(node, "值", graph);
		generator.AddLine("switch (" + inputValueFromGraph + ")");
		generator.AddLine("{");
		generator.IndentLevel++;
		EmitCases(generator, node, graph, isGraph: true);
		generator.AddLine("default:");
		generator.IndentLevel++;
		XWBPNodeData nextNodeFromGraph = generator.GetNextNodeFromGraph(node, 0, graph);
		if (nextNodeFromGraph != null)
		{
			generator.GenerateNodeCodeFromGraph(nextNodeFromGraph, graph);
		}
		generator.AddLine("break;");
		generator.IndentLevel--;
		generator.IndentLevel--;
		generator.AddLine("}");
	}

	private void EmitCases(XWBPCodeGenerator generator, XWBPNodeData node, GodotObject context, bool isGraph)
	{
		Variant metaData = node.GetMetaData("Cases");
		if (metaData.VariantType != Variant.Type.Array)
		{
			return;
		}
		Array array = metaData.As<Array>();
		for (int i = 0; i < array.Count; i++)
		{
			if (array[i].VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary = array[i].As<Dictionary>();
			if (!dictionary.TryGetValue("value", out var value))
			{
				continue;
			}
			string text = ((value.VariantType == Variant.Type.String) ? ("\"" + value.AsString() + "\"") : value.AsString());
			int outputIndex = (dictionary.TryGetValue("portIndex", out var value2) ? value2.AsInt32() : (i + 1));
			generator.AddLine("case " + text + ":");
			generator.IndentLevel++;
			if (isGraph)
			{
				XWBPNodeData nextNodeFromGraph = generator.GetNextNodeFromGraph(node, outputIndex, (XWBPGraphData)context);
				if (nextNodeFromGraph != null)
				{
					generator.GenerateNodeCodeFromGraph(nextNodeFromGraph, (XWBPGraphData)context);
				}
			}
			else
			{
				XWBPNodeData nextNode = generator.GetNextNode(node, outputIndex, (XWBPFunctionData)context);
				if (nextNode != null)
				{
					generator.GenerateNodeCode(nextNode, (XWBPFunctionData)context);
				}
			}
			generator.AddLine("break;");
			generator.IndentLevel--;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
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
			new MethodInfo(MethodName.EmitCases, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "generator", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "context", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "isGraph", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.EmitCases && args.Count == 4)
		{
			EmitCases(VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[0]), VariantUtils.ConvertTo<XWBPNodeData>(in args[1]), VariantUtils.ConvertTo<GodotObject>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
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
		if (method == MethodName.EmitCases)
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
