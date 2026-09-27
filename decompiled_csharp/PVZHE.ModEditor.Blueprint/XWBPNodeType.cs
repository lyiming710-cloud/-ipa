using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/Node/XWBPNodeType.cs")]
public abstract class XWBPNodeType : RefCounted
{
	public enum NodeTypeEnum
	{
		General,
		Entry,
		Function,
		FunctionEntry,
		PropertyGet,
		PropertySet
	}

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName SetCatalogPresentation = "SetCatalogPresentation";

		public static readonly StringName CreateNodeData = "CreateNodeData";

		public static readonly StringName BuildNodeData = "BuildNodeData";

		public static readonly StringName ClearAllPort = "ClearAllPort";

		public static readonly StringName ClearInput = "ClearInput";

		public static readonly StringName AddInput = "AddInput";

		public static readonly StringName ClearOutput = "ClearOutput";

		public static readonly StringName AddOutput = "AddOutput";

		public static readonly StringName AddFlowInput = "AddFlowInput";

		public static readonly StringName AddFlowOutput = "AddFlowOutput";

		public static readonly StringName LoadNodeData = "LoadNodeData";

		public static readonly StringName GraphNodeBuildRefresh = "GraphNodeBuildRefresh";

		public static readonly StringName GraphNodeInit = "GraphNodeInit";

		public static readonly StringName GenerateCode = "GenerateCode";

		public static readonly StringName GenerateCodeFromGraph = "GenerateCodeFromGraph";

		public static readonly StringName GetOutputValue = "GetOutputValue";

		public static readonly StringName GetOutputValueFromGraph = "GetOutputValueFromGraph";

		public static readonly StringName HasFlowOutput = "HasFlowOutput";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName TypeId = "TypeId";

		public static readonly StringName NodeTypeEnumValue = "NodeTypeEnumValue";

		public static readonly StringName Category = "Category";

		public static readonly StringName DisplayName = "DisplayName";

		public static readonly StringName Description = "Description";

		public static readonly StringName Color = "Color";

		public static readonly StringName Editor = "Editor";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public StringName TypeId { get; protected set; } = "";

	public NodeTypeEnum NodeTypeEnumValue { get; protected set; }

	public string Category { get; protected set; } = "";

	public string DisplayName { get; protected set; } = "";

	public string Description { get; protected set; } = "";

	public Color Color { get; protected set; } = new Color(0.04f, 0.2f, 0.4f);

	public List<XWBPNodePortData> InputPorts { get; protected set; } = new List<XWBPNodePortData>();

	public List<XWBPNodePortData> OutputPorts { get; protected set; } = new List<XWBPNodePortData>();

	public GodotObject Editor { get; set; }

	public void SetCatalogPresentation(string displayName, string description)
	{
		if (!string.IsNullOrWhiteSpace(displayName))
		{
			DisplayName = displayName;
		}
		Description = description ?? "";
	}

	public virtual XWBPNodeData CreateNodeData()
	{
		XWBPNodeData xWBPNodeData = new XWBPNodeData
		{
			TypeId = TypeId
		};
		foreach (XWBPNodePortData inputPort in InputPorts)
		{
			if (inputPort != null)
			{
				xWBPNodeData.InputPorts.Add(new XWBPNodePortData(inputPort.Name, inputPort.PortDirection, inputPort.PortTypeValue, inputPort.ClassName, inputPort.DefaultValue));
			}
		}
		foreach (XWBPNodePortData outputPort in OutputPorts)
		{
			if (outputPort != null)
			{
				xWBPNodeData.OutputPorts.Add(new XWBPNodePortData(outputPort.Name, outputPort.PortDirection, outputPort.PortTypeValue, outputPort.ClassName, outputPort.DefaultValue));
			}
		}
		xWBPNodeData.RebuildPortMap();
		return xWBPNodeData;
	}

	public virtual void BuildNodeData(XWBPNodeData nodeData)
	{
		if (nodeData == null)
		{
			return;
		}
		if (nodeData.MetaData == null)
		{
			Dictionary dictionary = (nodeData.MetaData = new Dictionary());
		}
		nodeData.InputPorts.Clear();
		nodeData.OutputPorts.Clear();
		nodeData.MetaData.Clear();
		nodeData.TypeId = TypeId;
		foreach (XWBPNodePortData inputPort in InputPorts)
		{
			if (inputPort != null)
			{
				nodeData.InputPorts.Add(new XWBPNodePortData(inputPort.Name, inputPort.PortDirection, inputPort.PortTypeValue, inputPort.ClassName, inputPort.DefaultValue));
			}
		}
		foreach (XWBPNodePortData outputPort in OutputPorts)
		{
			if (outputPort != null)
			{
				nodeData.OutputPorts.Add(new XWBPNodePortData(outputPort.Name, outputPort.PortDirection, outputPort.PortTypeValue, outputPort.ClassName, outputPort.DefaultValue));
			}
		}
		nodeData.RebuildPortMap();
	}

	public void ClearAllPort()
	{
		ClearInput();
		ClearOutput();
	}

	public void ClearInput()
	{
		InputPorts.Clear();
	}

	public void AddInput(string portName, XWBPNodePortData.PortType portType, StringName className = null, Variant defaultValue = default(Variant))
	{
		InputPorts.Add(new XWBPNodePortData(portName, XWBPNodePortData.Direction.Input, portType, className, defaultValue));
	}

	public void ClearOutput()
	{
		OutputPorts.Clear();
	}

	public void AddOutput(string portName, XWBPNodePortData.PortType portType, StringName className = null, Variant defaultValue = default(Variant))
	{
		OutputPorts.Add(new XWBPNodePortData(portName, XWBPNodePortData.Direction.Output, portType, className, defaultValue));
	}

	public void AddFlowInput(string portName = "执行")
	{
		AddInput(portName, XWBPNodePortData.PortType.Flow);
	}

	public void AddFlowOutput(string portName = "执行")
	{
		AddOutput(portName, XWBPNodePortData.PortType.Flow);
	}

	public virtual void LoadNodeData(XWBPNodeData nodeData)
	{
	}

	public virtual void GraphNodeBuildRefresh(GodotObject graphNode)
	{
	}

	public virtual void GraphNodeInit(GodotObject graphNode)
	{
	}

	public virtual void GenerateCode(XWBPCodeGenerator generator, XWBPNodeData node, XWBPFunctionData function)
	{
		if (!HasFlowOutput(node))
		{
			return;
		}
		throw new InvalidOperationException($"Blueprint node type '{TypeId}' does not implement C# code generation.");
	}

	public virtual void GenerateCodeFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, XWBPGraphData graph)
	{
		if (!HasFlowOutput(node))
		{
			return;
		}
		throw new InvalidOperationException($"Blueprint node type '{TypeId}' does not implement C# graph code generation.");
	}

	public virtual string GetOutputValue(XWBPCodeGenerator generator, XWBPNodeData node, string portName, XWBPFunctionData function)
	{
		if (generator == null || node == null)
		{
			return "null";
		}
		if (generator.NodeVariables.TryGetValue(node.Id, out var value))
		{
			return value + "_result";
		}
		XWBPNodePortData xWBPNodePortData = node.FindOutputPortByName(portName);
		if (xWBPNodePortData == null)
		{
			return "null";
		}
		return generator.GetDefaultValueString(xWBPNodePortData.Value, (int)xWBPNodePortData.PortTypeValue);
	}

	public virtual string GetOutputValueFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, string portName, XWBPGraphData graph)
	{
		if (generator == null || node == null)
		{
			return "null";
		}
		if (generator.NodeVariables.TryGetValue(node.Id, out var value))
		{
			return value + "_result";
		}
		XWBPNodePortData xWBPNodePortData = node.FindOutputPortByName(portName);
		if (xWBPNodePortData == null)
		{
			return "null";
		}
		return generator.GetDefaultValueString(xWBPNodePortData.Value, (int)xWBPNodePortData.PortTypeValue);
	}

	private static bool HasFlowOutput(XWBPNodeData node)
	{
		if (node == null)
		{
			return false;
		}
		foreach (XWBPNodePortData outputPort in node.OutputPorts)
		{
			if (outputPort != null && outputPort.PortTypeValue == XWBPNodePortData.PortType.Flow)
			{
				return true;
			}
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName.SetCatalogPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "displayName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "description", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateNodeData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildNodeData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearAllPort, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "portType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearOutput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddOutput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "portType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFlowInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFlowOutput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadNodeData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GraphNodeBuildRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graphNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.GraphNodeInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
			new MethodInfo(MethodName.HasFlowOutput, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetCatalogPresentation && args.Count == 2)
		{
			SetCatalogPresentation(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateNodeData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(CreateNodeData());
			return true;
		}
		if (method == MethodName.BuildNodeData && args.Count == 1)
		{
			BuildNodeData(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearAllPort && args.Count == 0)
		{
			ClearAllPort();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearInput && args.Count == 0)
		{
			ClearInput();
			ret = default;
			return true;
		}
		if (method == MethodName.AddInput && args.Count == 4)
		{
			AddInput(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<XWBPNodePortData.PortType>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearOutput && args.Count == 0)
		{
			ClearOutput();
			ret = default;
			return true;
		}
		if (method == MethodName.AddOutput && args.Count == 4)
		{
			AddOutput(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<XWBPNodePortData.PortType>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddFlowInput && args.Count == 1)
		{
			AddFlowInput(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddFlowOutput && args.Count == 1)
		{
			AddFlowOutput(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadNodeData && args.Count == 1)
		{
			LoadNodeData(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GraphNodeBuildRefresh && args.Count == 1)
		{
			GraphNodeBuildRefresh(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GraphNodeInit && args.Count == 1)
		{
			GraphNodeInit(VariantUtils.ConvertTo<GodotObject>(in args[0]));
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
		if (method == MethodName.HasFlowOutput && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFlowOutput(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasFlowOutput && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFlowOutput(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SetCatalogPresentation)
		{
			return true;
		}
		if (method == MethodName.CreateNodeData)
		{
			return true;
		}
		if (method == MethodName.BuildNodeData)
		{
			return true;
		}
		if (method == MethodName.ClearAllPort)
		{
			return true;
		}
		if (method == MethodName.ClearInput)
		{
			return true;
		}
		if (method == MethodName.AddInput)
		{
			return true;
		}
		if (method == MethodName.ClearOutput)
		{
			return true;
		}
		if (method == MethodName.AddOutput)
		{
			return true;
		}
		if (method == MethodName.AddFlowInput)
		{
			return true;
		}
		if (method == MethodName.AddFlowOutput)
		{
			return true;
		}
		if (method == MethodName.LoadNodeData)
		{
			return true;
		}
		if (method == MethodName.GraphNodeBuildRefresh)
		{
			return true;
		}
		if (method == MethodName.GraphNodeInit)
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
		if (method == MethodName.GetOutputValue)
		{
			return true;
		}
		if (method == MethodName.GetOutputValueFromGraph)
		{
			return true;
		}
		if (method == MethodName.HasFlowOutput)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.TypeId)
		{
			TypeId = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.NodeTypeEnumValue)
		{
			NodeTypeEnumValue = VariantUtils.ConvertTo<NodeTypeEnum>(in value);
			return true;
		}
		if (name == PropertyName.Category)
		{
			Category = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.DisplayName)
		{
			DisplayName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Description)
		{
			Description = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Color)
		{
			Color = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.Editor)
		{
			Editor = VariantUtils.ConvertTo<GodotObject>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.TypeId)
		{
			value = VariantUtils.CreateFrom<StringName>(TypeId);
			return true;
		}
		if (name == PropertyName.NodeTypeEnumValue)
		{
			value = VariantUtils.CreateFrom<NodeTypeEnum>(NodeTypeEnumValue);
			return true;
		}
		string from;
		if (name == PropertyName.Category)
		{
			from = Category;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DisplayName)
		{
			from = DisplayName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Description)
		{
			from = Description;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Color)
		{
			value = VariantUtils.CreateFrom<Color>(Color);
			return true;
		}
		if (name == PropertyName.Editor)
		{
			value = VariantUtils.CreateFrom<GodotObject>(Editor);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.TypeId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.NodeTypeEnumValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Category, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.DisplayName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Description, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.Color, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.TypeId, Variant.From<StringName>(TypeId));
		info.AddProperty(PropertyName.NodeTypeEnumValue, Variant.From<NodeTypeEnum>(NodeTypeEnumValue));
		info.AddProperty(PropertyName.Category, Variant.From<string>(Category));
		info.AddProperty(PropertyName.DisplayName, Variant.From<string>(DisplayName));
		info.AddProperty(PropertyName.Description, Variant.From<string>(Description));
		info.AddProperty(PropertyName.Color, Variant.From<Color>(Color));
		info.AddProperty(PropertyName.Editor, Variant.From<GodotObject>(Editor));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.TypeId, out var value))
		{
			TypeId = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.NodeTypeEnumValue, out var value2))
		{
			NodeTypeEnumValue = value2.As<NodeTypeEnum>();
		}
		if (info.TryGetProperty(PropertyName.Category, out var value3))
		{
			Category = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.DisplayName, out var value4))
		{
			DisplayName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Description, out var value5))
		{
			Description = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Color, out var value6))
		{
			Color = value6.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.Editor, out var value7))
		{
			Editor = value7.As<GodotObject>();
		}
	}
}
