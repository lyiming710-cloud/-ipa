using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/NodeTypes/Flow/XWBPNodeContinue.cs")]
public class XWBPNodeContinue : XWBPNodeType
{
	public new class MethodName : XWBPNodeType.MethodName
	{
		public new static readonly StringName GenerateCode = "GenerateCode";

		public new static readonly StringName GenerateCodeFromGraph = "GenerateCodeFromGraph";
	}

	public new class PropertyName : XWBPNodeType.PropertyName
	{
	}

	public new class SignalName : XWBPNodeType.SignalName
	{
	}

	public XWBPNodeContinue()
	{
		TypeId = "__XWBPGraphNode_Continue";
		Category = "流程控制";
		DisplayName = "继续循环";
		Description = "跳过本次循环剩余部分";
		Color = new Color(0.8f, 0.4f, 0.2f);
		AddFlowInput();
	}

	public override void GenerateCode(XWBPCodeGenerator generator, XWBPNodeData node, XWBPFunctionData function)
	{
		generator.AddLine("continue;");
	}

	public override void GenerateCodeFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, XWBPGraphData graph)
	{
		generator.AddLine("continue;");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
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
