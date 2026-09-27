using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/NodeTypes/Method/XWBPNodeCallMethod.cs")]
public class XWBPNodeCallMethod : XWBPNodeType
{
	public enum Type
	{
		Bp,
		Script,
		Virtual,
		Super,
		SignalEvent,
		SignalEmit
	}

	public new class MethodName : XWBPNodeType.MethodName
	{
		public new static readonly StringName CreateNodeData = "CreateNodeData";

		public static readonly StringName WriteNodeMetaData = "WriteNodeMetaData";

		public new static readonly StringName LoadNodeData = "LoadNodeData";

		public static readonly StringName ToPortType = "ToPortType";

		public static readonly StringName BuildFunction = "BuildFunction";

		public static readonly StringName BuildMethod = "BuildMethod";

		public new static readonly StringName GraphNodeBuildRefresh = "GraphNodeBuildRefresh";

		public static readonly StringName BuildSignalEvent = "BuildSignalEvent";

		public static readonly StringName BuildSignalEmit = "BuildSignalEmit";

		public static readonly StringName RefreshScriptPresentation = "RefreshScriptPresentation";

		public static readonly StringName GetCsMemberName = "GetCsMemberName";

		public static readonly StringName GetCsClassName = "GetCsClassName";

		public static readonly StringName IsStaticMember = "IsStaticMember";

		public static readonly StringName IsSelfMember = "IsSelfMember";

		public static readonly StringName HasValueOutput = "HasValueOutput";

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

	public XWBPNodeCallMethod()
	{
		TypeId = "__XWBPGraphNode_CallMethod";
		NodeTypeEnumValue = NodeTypeEnum.Function;
		Category = "方法";
		DisplayName = "调用方法";
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
			return;
		}
		if (MethodData == null)
		{
			Dictionary dictionary = (MethodData = new Dictionary());
		}
		nodeData.SetMetaData("MethodData", MethodData);
	}

	public override void LoadNodeData(XWBPNodeData nodeData)
	{
		MethodType = (Type)nodeData.GetMetaData("MethodType").AsInt32();
		if (MethodType == Type.Bp)
		{
			FunctionId = nodeData.GetMetaData("FunctionId").AsInt32();
			return;
		}
		MethodData = nodeData.GetMetaData("MethodData").As<Dictionary>();
		RefreshScriptPresentation();
	}

	public static XWBPNodePortData.PortType ToPortType(int variantType)
	{
		if (variantType >= 100)
		{
			return (XWBPNodePortData.PortType)variantType;
		}
		if (variantType >= 0 && variantType <= 29)
		{
			return (XWBPNodePortData.PortType)variantType;
		}
		return XWBPNodePortData.PortType.Any;
	}

	public void BuildFunction(XWBPFunctionData function)
	{
		ClearAllPort();
		MethodData = new Dictionary();
		AddFlowInput("");
		AddFlowOutput("");
		foreach (XWBPNodePortData input in function.Inputs)
		{
			AddInput(input.Name, input.PortTypeValue, input.ClassName, input.DefaultValue);
		}
		foreach (XWBPNodePortData output in function.Outputs)
		{
			AddOutput(output.Name, output.PortTypeValue, output.ClassName, output.DefaultValue);
		}
	}

	public void BuildMethod()
	{
		ClearAllPort();
		Godot.Collections.Array array = (MethodData.TryGetValue("args", out var value) ? value.As<Godot.Collections.Array>() : new Godot.Collections.Array());
		int num = (MethodData.TryGetValue("return", out var value2) ? (value2.As<Dictionary>().TryGetValue("type", out var value3) ? value3.AsInt32() : 0) : 0);
		AddFlowInput("");
		AddFlowOutput("");
		bool flag = MethodData.TryGetValue("is_static", out var value4) && value4.AsBool();
		bool flag2 = IsSelfMember(MethodData);
		if (MethodType != Type.Super && !flag && !flag2)
		{
			StringName stringName = (MethodData.TryGetValue("base_class_name", out var value5) ? value5.AsStringName() : ((StringName)""));
			AddInput(stringName, XWBPNodePortData.PortType.Object, stringName);
		}
		foreach (Variant item in array)
		{
			Dictionary dictionary = item.As<Dictionary>();
			string portName = (dictionary.TryGetValue("name", out var value6) ? value6.AsString() : "参数");
			int num2 = (dictionary.TryGetValue("type", out var value7) ? value7.AsInt32() : 0);
			XWBPNodePortData.PortType portType = ToPortType(num2);
			StringName className = "";
			if (num2 == 24)
			{
				className = (dictionary.TryGetValue("class_name", out var value8) ? value8.AsStringName() : ((StringName)""));
			}
			AddInput(portName, portType, className);
		}
		if (num != 0)
		{
			XWBPNodePortData.PortType portType2 = ToPortType(num);
			StringName className2 = "";
			if (num == 24)
			{
				string text;
				if (MethodData.TryGetValue("return", out var value9))
				{
					text = (value9.As<Dictionary>().TryGetValue("class_name", out var value10) ? ((string?)value10.AsStringName()) : "");
				}
				else
				{
					text = "";
				}
				className2 = text;
			}
			AddOutput("返回值", portType2, className2);
		}
		RefreshScriptPresentation();
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
		else if (MethodType == Type.SignalEvent)
		{
			BuildSignalEvent();
		}
		else if (MethodType == Type.SignalEmit)
		{
			BuildSignalEmit();
		}
		else
		{
			BuildMethod();
		}
		BuildNodeData(xWBPGraphNode.NodeData);
		WriteNodeMetaData(xWBPGraphNode.NodeData);
	}

	public void BuildSignalEvent()
	{
		ClearAllPort();
		Godot.Collections.Array array = (MethodData.TryGetValue("args", out var value) ? value.As<Godot.Collections.Array>() : new Godot.Collections.Array());
		AddFlowOutput();
		foreach (Variant item in array)
		{
			Dictionary dictionary = item.As<Dictionary>();
			string portName = (dictionary.TryGetValue("name", out var value2) ? value2.AsString() : "参数");
			int num = (dictionary.TryGetValue("type", out var value3) ? value3.AsInt32() : 0);
			XWBPNodePortData.PortType portType = ToPortType(num);
			StringName className = "";
			if (num == 24)
			{
				className = (dictionary.TryGetValue("class_name", out var value4) ? value4.AsStringName() : ((StringName)""));
			}
			AddOutput(portName, portType, className);
		}
		RefreshScriptPresentation();
	}

	public void BuildSignalEmit()
	{
		ClearAllPort();
		Godot.Collections.Array array = (MethodData.TryGetValue("args", out var value) ? value.As<Godot.Collections.Array>() : new Godot.Collections.Array());
		AddFlowInput("");
		AddFlowOutput("");
		foreach (Variant item in array)
		{
			Dictionary dictionary = item.As<Dictionary>();
			string portName = (dictionary.TryGetValue("name", out var value2) ? value2.AsString() : "参数");
			int num = (dictionary.TryGetValue("type", out var value3) ? value3.AsInt32() : 0);
			XWBPNodePortData.PortType portType = ToPortType(num);
			StringName className = "";
			if (num == 24)
			{
				className = (dictionary.TryGetValue("class_name", out var value4) ? value4.AsStringName() : ((StringName)""));
			}
			AddInput(portName, portType, className);
		}
		RefreshScriptPresentation();
	}

	private void RefreshScriptPresentation()
	{
		if (MethodType != Type.Bp && MethodData != null)
		{
			string text = (MethodData.TryGetValue("cs_name", out var value) ? value.AsString() : "");
			if (string.IsNullOrWhiteSpace(text))
			{
				text = (MethodData.TryGetValue("name", out var value2) ? value2.AsString() : "方法");
			}
			string text2 = (MethodData.TryGetValue("base_class_name", out var value3) ? value3.AsString() : "");
			if (text2.Contains(".", StringComparison.Ordinal))
			{
				text2 = text2.Substring(text2.LastIndexOf('.') + 1);
			}
			SetCatalogPresentation(MethodType switch
			{
				Type.SignalEvent => "事件 " + text, 
				Type.SignalEmit => "发射 " + text, 
				Type.Super => "Super " + text, 
				_ => string.IsNullOrWhiteSpace(text2) ? (text + "()") : (text2 + "." + text + "()"), 
			}, string.IsNullOrWhiteSpace(text2) ? text : (text2 + "." + text));
		}
	}

	private static string GetCsMemberName(Dictionary methodData, string fallbackName, XWBPCodeGenerator generator)
	{
		if (methodData.TryGetValue("cs_name", out var value))
		{
			string text = value.AsString();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		return generator.ToCsMethodName(fallbackName);
	}

	private static string GetCsClassName(Dictionary methodData)
	{
		if (methodData.TryGetValue("cs_qualified_class_name", out var value))
		{
			string text = value.AsString();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		if (methodData.TryGetValue("cs_class_name", out var value2))
		{
			string text2 = value2.AsString();
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
		}
		if (!methodData.TryGetValue("base_class_name", out var value3))
		{
			return "";
		}
		return value3.AsString();
	}

	private static bool IsStaticMember(Dictionary methodData)
	{
		if (methodData.TryGetValue("is_static", out var value))
		{
			return value.AsBool();
		}
		return false;
	}

	private static bool IsSelfMember(Dictionary methodData)
	{
		if (methodData.TryGetValue("xw_self_member", out var value))
		{
			return value.AsBool();
		}
		return false;
	}

	private static bool HasValueOutput(XWBPNodeData node)
	{
		foreach (XWBPNodePortData outputPort in node.OutputPorts)
		{
			if (outputPort.PortTypeValue != XWBPNodePortData.PortType.Flow)
			{
				return true;
			}
		}
		return false;
	}

	public override void GenerateCode(XWBPCodeGenerator generator, XWBPNodeData node, XWBPFunctionData function)
	{
		int num = node.GetMetaData("MethodType").AsInt32();
		List<string> list = new List<string>();
		switch (num)
		{
		case 0:
		{
			int key = node.GetMetaData("FunctionId").AsInt32();
			if (!generator.ScriptData.Functions.TryGetValue(key, out var value12))
			{
				break;
			}
			string text3 = generator.ToCsMethodName(value12.Name);
			foreach (XWBPNodePortData input in value12.Inputs)
			{
				list.Add(generator.GetInputValue(node, input.Name, function));
			}
			if (value12.Outputs.Count > 0)
			{
				string value13 = $"var_{node.Id}";
				generator.NodeVariables[node.Id] = value13;
				string value14 = text3 + "(" + string.Join(", ", list) + ")";
				generator.AddLine($"var {value13}_result = {value14};");
			}
			else
			{
				generator.AddLine(text3 + "(" + string.Join(", ", list) + ");");
			}
			break;
		}
		case 5:
		{
			Dictionary dictionary4 = node.GetMetaData("MethodData").As<Dictionary>();
			string snakeName = (dictionary4.TryGetValue("name", out var value9) ? value9.AsStringName().ToString() : "");
			Variant value10;
			foreach (Variant item in dictionary4.TryGetValue("args", out value10) ? value10.As<Godot.Collections.Array>() : new Godot.Collections.Array())
			{
				string portName3 = (item.As<Dictionary>().TryGetValue("name", out var value11) ? value11.AsString() : "arg");
				list.Add(generator.GetInputValue(node, portName3, function));
			}
			string text2 = generator.ToPascalCase(snakeName);
			if (list.Count == 0)
			{
				generator.AddLine("EmitSignal(SignalName." + text2 + ");");
				break;
			}
			generator.AddLine($"EmitSignal(SignalName.{text2}, {string.Join(", ", list)});");
			break;
		}
		default:
		{
			Dictionary dictionary = node.GetMetaData("MethodData").As<Dictionary>();
			string text = (dictionary.TryGetValue("name", out var value) ? value.AsStringName().ToString() : "");
			Godot.Collections.Array array = (dictionary.TryGetValue("args", out var value2) ? value2.As<Godot.Collections.Array>() : new Godot.Collections.Array());
			string csMemberName = GetCsMemberName(dictionary, text, generator);
			switch (num)
			{
			case 3:
				if (!generator.ShouldCallSuper(text))
				{
					generator.AddLine("// Warning: Parent class does not implement " + text);
					break;
				}
				foreach (Variant item2 in array)
				{
					string name = (item2.As<Dictionary>().TryGetValue("name", out var value8) ? value8.AsString() : "arg");
					list.Add(generator.ToValidIdentifier(name));
				}
				if (list.Count == 0)
				{
					generator.AddLine("base." + csMemberName + "();");
					break;
				}
				generator.AddLine($"base.{csMemberName}({string.Join(", ", list)});");
				break;
			case 2:
				foreach (Variant item3 in array)
				{
					Dictionary dictionary3 = item3.As<Dictionary>();
					string portName2 = (dictionary3.TryGetValue("name", out var value7) ? value7.AsString() : "arg");
					string inputValue2 = generator.GetInputValue(node, portName2, function);
					list.Add(generator.ConvertToCSharpMemberValue(inputValue2, dictionary3));
				}
				generator.AddLine($"base.{csMemberName}({string.Join(", ", list)});");
				break;
			case 1:
			{
				StringName stringName = (dictionary.TryGetValue("base_class_name", out var value3) ? value3.AsStringName() : ((StringName)""));
				string value4;
				if (IsStaticMember(dictionary))
				{
					value4 = GetCsClassName(dictionary);
				}
				else
				{
					value4 = (IsSelfMember(dictionary) ? "this" : generator.GetInputValue(node, stringName, function));
				}
				foreach (Variant item4 in array)
				{
					Dictionary dictionary2 = item4.As<Dictionary>();
					string portName = (dictionary2.TryGetValue("name", out var value5) ? value5.AsString() : "arg");
					string inputValue = generator.GetInputValue(node, portName, function);
					list.Add(generator.ConvertToCSharpMemberValue(inputValue, dictionary2));
				}
				if (HasValueOutput(node))
				{
					string value6 = $"var_{node.Id}";
					generator.NodeVariables[node.Id] = value6;
					generator.AddLine($"var {value6}_result = {value4}.{csMemberName}({string.Join(", ", list)});");
				}
				else
				{
					generator.AddLine($"{value4}.{csMemberName}({string.Join(", ", list)});");
				}
				break;
			}
			}
			break;
		}
		}
		XWBPNodeData nextNode = generator.GetNextNode(node, 0, function);
		if (nextNode != null)
		{
			generator.GenerateNodeCode(nextNode, function);
		}
	}

	public override void GenerateCodeFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, XWBPGraphData graph)
	{
		int num = node.GetMetaData("MethodType").AsInt32();
		List<string> list = new List<string>();
		switch (num)
		{
		case 0:
		{
			int key = node.GetMetaData("FunctionId").AsInt32();
			if (!generator.ScriptData.Functions.TryGetValue(key, out var value12))
			{
				break;
			}
			string text3 = generator.ToCsMethodName(value12.Name);
			foreach (XWBPNodePortData input in value12.Inputs)
			{
				list.Add(generator.GetInputValueFromGraph(node, input.Name, graph));
			}
			if (value12.Outputs.Count > 0)
			{
				string value13 = $"var_{node.Id}";
				generator.NodeVariables[node.Id] = value13;
				string value14 = text3 + "(" + string.Join(", ", list) + ")";
				generator.AddLine($"var {value13}_result = {value14};");
			}
			else
			{
				generator.AddLine(text3 + "(" + string.Join(", ", list) + ");");
			}
			break;
		}
		case 5:
		{
			Dictionary dictionary4 = node.GetMetaData("MethodData").As<Dictionary>();
			string snakeName = (dictionary4.TryGetValue("name", out var value9) ? value9.AsStringName().ToString() : "");
			Variant value10;
			foreach (Variant item in dictionary4.TryGetValue("args", out value10) ? value10.As<Godot.Collections.Array>() : new Godot.Collections.Array())
			{
				string portName3 = (item.As<Dictionary>().TryGetValue("name", out var value11) ? value11.AsString() : "arg");
				list.Add(generator.GetInputValueFromGraph(node, portName3, graph));
			}
			string text2 = generator.ToPascalCase(snakeName);
			if (list.Count == 0)
			{
				generator.AddLine("EmitSignal(SignalName." + text2 + ");");
				break;
			}
			generator.AddLine($"EmitSignal(SignalName.{text2}, {string.Join(", ", list)});");
			break;
		}
		default:
		{
			Dictionary dictionary = node.GetMetaData("MethodData").As<Dictionary>();
			string text = (dictionary.TryGetValue("name", out var value) ? value.AsStringName().ToString() : "");
			Godot.Collections.Array array = (dictionary.TryGetValue("args", out var value2) ? value2.As<Godot.Collections.Array>() : new Godot.Collections.Array());
			string csMemberName = GetCsMemberName(dictionary, text, generator);
			switch (num)
			{
			case 3:
				if (!generator.ShouldCallSuper(text))
				{
					generator.AddLine("// Warning: Parent class does not implement " + text);
					break;
				}
				foreach (Variant item2 in array)
				{
					string name = (item2.As<Dictionary>().TryGetValue("name", out var value8) ? value8.AsString() : "arg");
					list.Add(generator.ToValidIdentifier(name));
				}
				if (list.Count == 0)
				{
					generator.AddLine("base." + csMemberName + "();");
					break;
				}
				generator.AddLine($"base.{csMemberName}({string.Join(", ", list)});");
				break;
			case 2:
				foreach (Variant item3 in array)
				{
					Dictionary dictionary3 = item3.As<Dictionary>();
					string portName2 = (dictionary3.TryGetValue("name", out var value7) ? value7.AsString() : "arg");
					string inputValueFromGraph2 = generator.GetInputValueFromGraph(node, portName2, graph);
					list.Add(generator.ConvertToCSharpMemberValue(inputValueFromGraph2, dictionary3));
				}
				generator.AddLine($"base.{csMemberName}({string.Join(", ", list)});");
				break;
			case 1:
			{
				StringName stringName = (dictionary.TryGetValue("base_class_name", out var value3) ? value3.AsStringName() : ((StringName)""));
				string value4;
				if (IsStaticMember(dictionary))
				{
					value4 = GetCsClassName(dictionary);
				}
				else
				{
					value4 = (IsSelfMember(dictionary) ? "this" : generator.GetInputValueFromGraph(node, stringName, graph));
				}
				foreach (Variant item4 in array)
				{
					Dictionary dictionary2 = item4.As<Dictionary>();
					string portName = (dictionary2.TryGetValue("name", out var value5) ? value5.AsString() : "arg");
					string inputValueFromGraph = generator.GetInputValueFromGraph(node, portName, graph);
					list.Add(generator.ConvertToCSharpMemberValue(inputValueFromGraph, dictionary2));
				}
				if (HasValueOutput(node))
				{
					string value6 = $"var_{node.Id}";
					generator.NodeVariables[node.Id] = value6;
					generator.AddLine($"var {value6}_result = {value4}.{csMemberName}({string.Join(", ", list)});");
				}
				else
				{
					generator.AddLine($"{value4}.{csMemberName}({string.Join(", ", list)});");
				}
				break;
			}
			}
			break;
		}
		case 4:
			break;
		}
		XWBPNodeData nextNodeFromGraph = generator.GetNextNodeFromGraph(node, 0, graph);
		if (nextNodeFromGraph != null)
		{
			generator.GenerateNodeCodeFromGraph(nextNodeFromGraph, graph);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
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
			new MethodInfo(MethodName.ToPortType, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "variantType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildFunction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "function", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildMethod, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GraphNodeBuildRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graphNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSignalEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildSignalEmit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshScriptPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCsMemberName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "methodData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fallbackName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "generator", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCsClassName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "methodData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsStaticMember, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "methodData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSelfMember, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "methodData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasValueOutput, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
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
		if (method == MethodName.ToPortType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodePortData.PortType>(ToPortType(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildFunction && args.Count == 1)
		{
			BuildFunction(VariantUtils.ConvertTo<XWBPFunctionData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildMethod && args.Count == 0)
		{
			BuildMethod();
			ret = default;
			return true;
		}
		if (method == MethodName.GraphNodeBuildRefresh && args.Count == 1)
		{
			GraphNodeBuildRefresh(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSignalEvent && args.Count == 0)
		{
			BuildSignalEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSignalEmit && args.Count == 0)
		{
			BuildSignalEmit();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshScriptPresentation && args.Count == 0)
		{
			RefreshScriptPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCsMemberName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(GetCsMemberName(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[2])));
			return true;
		}
		if (method == MethodName.GetCsClassName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCsClassName(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.IsStaticMember && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsStaticMember(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSelfMember && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSelfMember(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.HasValueOutput && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasValueOutput(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
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
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ToPortType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodePortData.PortType>(ToPortType(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCsMemberName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(GetCsMemberName(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[2])));
			return true;
		}
		if (method == MethodName.GetCsClassName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCsClassName(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.IsStaticMember && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsStaticMember(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSelfMember && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSelfMember(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.HasValueOutput && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasValueOutput(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
			return true;
		}
		ret = default;
		return false;
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
		if (method == MethodName.ToPortType)
		{
			return true;
		}
		if (method == MethodName.BuildFunction)
		{
			return true;
		}
		if (method == MethodName.BuildMethod)
		{
			return true;
		}
		if (method == MethodName.GraphNodeBuildRefresh)
		{
			return true;
		}
		if (method == MethodName.BuildSignalEvent)
		{
			return true;
		}
		if (method == MethodName.BuildSignalEmit)
		{
			return true;
		}
		if (method == MethodName.RefreshScriptPresentation)
		{
			return true;
		}
		if (method == MethodName.GetCsMemberName)
		{
			return true;
		}
		if (method == MethodName.GetCsClassName)
		{
			return true;
		}
		if (method == MethodName.IsStaticMember)
		{
			return true;
		}
		if (method == MethodName.IsSelfMember)
		{
			return true;
		}
		if (method == MethodName.HasValueOutput)
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
