using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/NodeTypes/Signal/XWBPNodeEmitSignal.cs")]
public class XWBPNodeEmitSignal : XWBPNodeType
{
	public new class MethodName : XWBPNodeType.MethodName
	{
		public new static readonly StringName CreateNodeData = "CreateNodeData";

		public new static readonly StringName LoadNodeData = "LoadNodeData";

		public new static readonly StringName GraphNodeInit = "GraphNodeInit";

		public new static readonly StringName GraphNodeBuildRefresh = "GraphNodeBuildRefresh";

		public static readonly StringName BuildSignal = "BuildSignal";

		public new static readonly StringName GenerateCode = "GenerateCode";

		public new static readonly StringName GenerateCodeFromGraph = "GenerateCodeFromGraph";
	}

	public new class PropertyName : XWBPNodeType.PropertyName
	{
		public static readonly StringName SignalId = "SignalId";
	}

	public new class SignalName : XWBPNodeType.SignalName
	{
	}

	public int SignalId { get; set; } = -1;

	public XWBPNodeEmitSignal()
	{
		TypeId = "__XWBPGraphNode_EmitSignal";
		NodeTypeEnumValue = NodeTypeEnum.General;
		Category = "事件";
		DisplayName = "发射信号";
		Description = "发射蓝图定义的信号";
		Color = new Color(0.6f, 0.4f, 0.2f);
	}

	public override XWBPNodeData CreateNodeData()
	{
		XWBPNodeData xWBPNodeData = base.CreateNodeData();
		xWBPNodeData.SetMetaData("SignalId", SignalId);
		return xWBPNodeData;
	}

	public override void LoadNodeData(XWBPNodeData nodeData)
	{
		SignalId = nodeData.GetMetaData("SignalId").AsInt32();
	}

	public override void GraphNodeInit(GodotObject graphNode)
	{
		if (SignalId == -1)
		{
			return;
		}
		XWBPEditor xWBPEditor = (graphNode as XWBPGraphNode)?.Editor;
		if (xWBPEditor?.BpScriptData?.SignalDatas != null)
		{
			if (!xWBPEditor.BpScriptData.SignalDatas.TryGetValue(SignalId, out var value))
			{
				(graphNode as XWBPGraphNode)?.RemoveSelf();
			}
			else
			{
				(graphNode as XWBPGraphNode).Title = "发射 " + value.Name;
			}
		}
	}

	public override void GraphNodeBuildRefresh(GodotObject graphNode)
	{
		if (SignalId == -1)
		{
			return;
		}
		XWBPEditor xWBPEditor = (graphNode as XWBPGraphNode)?.Editor;
		if (xWBPEditor?.BpScriptData?.SignalDatas != null && xWBPEditor.BpScriptData.SignalDatas.TryGetValue(SignalId, out var value))
		{
			BuildSignal(value);
			XWBPNodeData xWBPNodeData = (graphNode as XWBPGraphNode)?.NodeData;
			if (xWBPNodeData != null)
			{
				BuildNodeData(xWBPNodeData);
				xWBPNodeData.SetMetaData("SignalId", SignalId);
			}
		}
	}

	public void BuildSignal(XWBPSignalData signalData)
	{
		ClearAllPort();
		AddFlowInput("");
		AddFlowOutput("");
		foreach (XWBPNodePortData input in signalData.Inputs)
		{
			AddInput(input.Name, input.PortTypeValue, input.ClassName, input.DefaultValue);
		}
	}

	public override void GenerateCode(XWBPCodeGenerator generator, XWBPNodeData node, XWBPFunctionData function)
	{
		int key = node.GetMetaData("SignalId").AsInt32();
		if (generator.ScriptData?.SignalDatas == null || !generator.ScriptData.SignalDatas.TryGetValue(key, out var value))
		{
			XWBPNodeData nextNode = generator.GetNextNode(node, 0, function);
			if (nextNode != null)
			{
				generator.GenerateNodeCode(nextNode, function);
			}
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (XWBPNodePortData input in value.Inputs)
		{
			if (!flag)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(generator.GetInputValue(node, input.Name, function));
			flag = false;
		}
		string text = generator.ToPascalCase(value.Name.ToString());
		if (stringBuilder.Length == 0)
		{
			generator.AddLine("EmitSignal(SignalName." + text + ");");
		}
		else
		{
			generator.AddLine($"EmitSignal(SignalName.{text}, {stringBuilder});");
		}
		XWBPNodeData nextNode2 = generator.GetNextNode(node, 0, function);
		if (nextNode2 != null)
		{
			generator.GenerateNodeCode(nextNode2, function);
		}
	}

	public override void GenerateCodeFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, XWBPGraphData graph)
	{
		int key = node.GetMetaData("SignalId").AsInt32();
		if (generator.ScriptData?.SignalDatas == null || !generator.ScriptData.SignalDatas.TryGetValue(key, out var value))
		{
			XWBPNodeData nextNodeFromGraph = generator.GetNextNodeFromGraph(node, 0, graph);
			if (nextNodeFromGraph != null)
			{
				generator.GenerateNodeCodeFromGraph(nextNodeFromGraph, graph);
			}
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (XWBPNodePortData input in value.Inputs)
		{
			if (!flag)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(generator.GetInputValueFromGraph(node, input.Name, graph));
			flag = false;
		}
		string text = generator.ToPascalCase(value.Name.ToString());
		if (stringBuilder.Length == 0)
		{
			generator.AddLine("EmitSignal(SignalName." + text + ");");
		}
		else
		{
			generator.AddLine($"EmitSignal(SignalName.{text}, {stringBuilder});");
		}
		XWBPNodeData nextNodeFromGraph2 = generator.GetNextNodeFromGraph(node, 0, graph);
		if (nextNodeFromGraph2 != null)
		{
			generator.GenerateNodeCodeFromGraph(nextNodeFromGraph2, graph);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.CreateNodeData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadNodeData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GraphNodeInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graphNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.GraphNodeBuildRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graphNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSignal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "signalData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
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
		if (method == MethodName.GraphNodeInit && args.Count == 1)
		{
			GraphNodeInit(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GraphNodeBuildRefresh && args.Count == 1)
		{
			GraphNodeBuildRefresh(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSignal && args.Count == 1)
		{
			BuildSignal(VariantUtils.ConvertTo<XWBPSignalData>(in args[0]));
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
		if (method == MethodName.LoadNodeData)
		{
			return true;
		}
		if (method == MethodName.GraphNodeInit)
		{
			return true;
		}
		if (method == MethodName.GraphNodeBuildRefresh)
		{
			return true;
		}
		if (method == MethodName.BuildSignal)
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
		if (name == PropertyName.SignalId)
		{
			SignalId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.SignalId)
		{
			value = VariantUtils.CreateFrom<int>(SignalId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.SignalId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.SignalId, Variant.From<int>(SignalId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.SignalId, out var value))
		{
			SignalId = value.As<int>();
		}
	}
}
