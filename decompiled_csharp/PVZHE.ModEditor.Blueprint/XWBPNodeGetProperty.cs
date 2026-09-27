using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/NodeTypes/Variable/XWBPNodeGetProperty.cs")]
public class XWBPNodeGetProperty : XWBPNodeType
{
	public enum Type
	{
		Bp,
		Script
	}

	public new class MethodName : XWBPNodeType.MethodName
	{
		public new static readonly StringName CreateNodeData = "CreateNodeData";

		public static readonly StringName WriteNodeMetaData = "WriteNodeMetaData";

		public new static readonly StringName LoadNodeData = "LoadNodeData";

		public static readonly StringName BuildVariable = "BuildVariable";

		public static readonly StringName BuildProperty = "BuildProperty";

		public static readonly StringName RefreshScriptPresentation = "RefreshScriptPresentation";

		public new static readonly StringName GraphNodeBuildRefresh = "GraphNodeBuildRefresh";

		public static readonly StringName GetCsPropertyName = "GetCsPropertyName";

		public static readonly StringName GetCsClassName = "GetCsClassName";

		public static readonly StringName IsStaticMember = "IsStaticMember";

		public static readonly StringName IsSelfMember = "IsSelfMember";

		public new static readonly StringName GenerateCode = "GenerateCode";

		public new static readonly StringName GenerateCodeFromGraph = "GenerateCodeFromGraph";

		public new static readonly StringName GetOutputValue = "GetOutputValue";

		public new static readonly StringName GetOutputValueFromGraph = "GetOutputValueFromGraph";
	}

	public new class PropertyName : XWBPNodeType.PropertyName
	{
		public static readonly StringName VariableId = "VariableId";

		public static readonly StringName MethodType = "MethodType";

		public static readonly StringName PropertyData = "PropertyData";
	}

	public new class SignalName : XWBPNodeType.SignalName
	{
	}

	public int VariableId { get; set; }

	public Type MethodType { get; set; }

	public Dictionary PropertyData { get; set; } = new Dictionary();

	public XWBPNodeGetProperty()
	{
		TypeId = "__XWBPGraphNode_GetProperty";
		NodeTypeEnumValue = NodeTypeEnum.PropertyGet;
		Category = "变量";
		DisplayName = "获取属性";
		Color = new Color(0.4f, 0.6f, 0.3f);
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
			nodeData.SetMetaData("VariableId", VariableId);
			return;
		}
		if (PropertyData == null)
		{
			Dictionary dictionary = (PropertyData = new Dictionary());
		}
		nodeData.SetMetaData("PropertyData", PropertyData);
	}

	public override void LoadNodeData(XWBPNodeData nodeData)
	{
		MethodType = (Type)nodeData.GetMetaData("MethodType").AsInt32();
		if (MethodType == Type.Bp)
		{
			VariableId = nodeData.GetMetaData("VariableId").AsInt32();
			return;
		}
		PropertyData = nodeData.GetMetaData("PropertyData").As<Dictionary>();
		RefreshScriptPresentation();
	}

	public void BuildVariable(XWBPVariableData variable)
	{
		ClearAllPort();
		XWBPNodePortData.PortType portType = (XWBPNodePortData.PortType)variable.Type;
		StringName className = "";
		if (variable.Type == Variant.Type.Object)
		{
			className = variable.ClassName;
		}
		AddOutput("值", portType, className);
	}

	public void BuildProperty()
	{
		ClearAllPort();
		StringName stringName = (PropertyData.TryGetValue("base_class_name", out var value) ? value.AsStringName() : ((StringName)""));
		int num = (PropertyData.TryGetValue("type", out var value2) ? value2.AsInt32() : 0);
		if ((!PropertyData.TryGetValue("is_static", out var value3) || !value3.AsBool()) && !IsSelfMember(PropertyData))
		{
			AddInput(stringName, XWBPNodePortData.PortType.Object, stringName);
		}
		XWBPNodePortData.PortType portType = (XWBPNodePortData.PortType)num;
		StringName className = "";
		if (num == 24)
		{
			className = (PropertyData.TryGetValue("class_name", out var value4) ? value4.AsStringName() : ((StringName)""));
		}
		AddOutput("值", portType, className);
		RefreshScriptPresentation();
	}

	private void RefreshScriptPresentation()
	{
		if (MethodType == Type.Script && PropertyData != null)
		{
			string text = (PropertyData.TryGetValue("cs_name", out var value) ? value.AsString() : "");
			if (string.IsNullOrWhiteSpace(text))
			{
				text = (PropertyData.TryGetValue("name", out var value2) ? value2.AsString() : "属性");
			}
			string text2 = (PropertyData.TryGetValue("base_class_name", out var value3) ? value3.AsString() : "");
			if (text2.Contains(".", StringComparison.Ordinal))
			{
				text2 = text2.Substring(text2.LastIndexOf('.') + 1);
			}
			SetCatalogPresentation(string.IsNullOrWhiteSpace(text2) ? ("获取 " + text) : ("获取 " + text2 + "." + text), ("读取 " + text2 + "." + text).Trim('.'));
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
			if (editor?.BpScriptData?.Variables != null && editor.BpScriptData.Variables.TryGetValue(VariableId, out var value))
			{
				BuildVariable(value);
			}
		}
		else
		{
			BuildProperty();
		}
		BuildNodeData(xWBPGraphNode.NodeData);
		WriteNodeMetaData(xWBPGraphNode.NodeData);
	}

	private static string GetCsPropertyName(Dictionary propertyData, string fallbackName, XWBPCodeGenerator generator)
	{
		if (propertyData.TryGetValue("cs_name", out var value))
		{
			string text = value.AsString();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		return generator.ToPascalCase(fallbackName);
	}

	private static string GetCsClassName(Dictionary propertyData)
	{
		if (propertyData.TryGetValue("cs_qualified_class_name", out var value))
		{
			string text = value.AsString();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		if (propertyData.TryGetValue("cs_class_name", out var value2))
		{
			string text2 = value2.AsString();
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
		}
		if (!propertyData.TryGetValue("base_class_name", out var value3))
		{
			return "";
		}
		return value3.AsString();
	}

	private static bool IsStaticMember(Dictionary propertyData)
	{
		if (propertyData.TryGetValue("is_static", out var value))
		{
			return value.AsBool();
		}
		return false;
	}

	private static bool IsSelfMember(Dictionary propertyData)
	{
		if (propertyData.TryGetValue("xw_self_member", out var value))
		{
			return value.AsBool();
		}
		return false;
	}

	public override void GenerateCode(XWBPCodeGenerator generator, XWBPNodeData node, XWBPFunctionData function)
	{
		if (node.GetMetaData("MethodType").AsInt32() == 0)
		{
			int key = node.GetMetaData("VariableId").AsInt32();
			if (generator.ScriptData.Variables.TryGetValue(key, out var value))
			{
				string value2 = $"var_{node.Id}";
				generator.NodeVariables[node.Id] = value2;
				string value3 = generator.ToPascalCase(value.Name);
				generator.AddLine($"var {value2}_value = {value3};");
			}
		}
		else
		{
			Dictionary dictionary = node.GetMetaData("PropertyData").As<Dictionary>();
			string fallbackName = (dictionary.TryGetValue("name", out var value4) ? value4.AsString() : "");
			StringName stringName = (dictionary.TryGetValue("base_class_name", out var value5) ? value5.AsStringName() : ((StringName)""));
			string value6;
			if (IsStaticMember(dictionary))
			{
				value6 = GetCsClassName(dictionary);
			}
			else
			{
				value6 = (IsSelfMember(dictionary) ? "this" : generator.GetInputValue(node, stringName, function));
			}
			string value7 = $"var_{node.Id}";
			generator.NodeVariables[node.Id] = value7;
			generator.AddLine($"var {value7}_value = {value6}.{GetCsPropertyName(dictionary, fallbackName, generator)};");
		}
	}

	public override void GenerateCodeFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, XWBPGraphData graph)
	{
		if (node.GetMetaData("MethodType").AsInt32() == 0)
		{
			int key = node.GetMetaData("VariableId").AsInt32();
			if (generator.ScriptData.Variables.TryGetValue(key, out var value))
			{
				string value2 = $"var_{node.Id}";
				generator.NodeVariables[node.Id] = value2;
				string value3 = generator.ToPascalCase(value.Name);
				generator.AddLine($"var {value2}_value = {value3};");
			}
		}
		else
		{
			Dictionary dictionary = node.GetMetaData("PropertyData").As<Dictionary>();
			string fallbackName = (dictionary.TryGetValue("name", out var value4) ? value4.AsString() : "");
			StringName stringName = (dictionary.TryGetValue("base_class_name", out var value5) ? value5.AsStringName() : ((StringName)""));
			string value6;
			if (IsStaticMember(dictionary))
			{
				value6 = GetCsClassName(dictionary);
			}
			else
			{
				value6 = (IsSelfMember(dictionary) ? "this" : generator.GetInputValueFromGraph(node, stringName, graph));
			}
			string value7 = $"var_{node.Id}";
			generator.NodeVariables[node.Id] = value7;
			generator.AddLine($"var {value7}_value = {value6}.{GetCsPropertyName(dictionary, fallbackName, generator)};");
		}
	}

	public override string GetOutputValue(XWBPCodeGenerator generator, XWBPNodeData node, string portName, XWBPFunctionData function)
	{
		if (generator.NodeVariables.TryGetValue(node.Id, out var value))
		{
			return value + "_value";
		}
		return "null";
	}

	public override string GetOutputValueFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, string portName, XWBPGraphData graph)
	{
		if (generator.NodeVariables.TryGetValue(node.Id, out var value))
		{
			return value + "_value";
		}
		return "null";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
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
			new MethodInfo(MethodName.BuildVariable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "variable", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshScriptPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GraphNodeBuildRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graphNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCsPropertyName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "propertyData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fallbackName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "generator", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCsClassName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "propertyData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsStaticMember, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "propertyData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSelfMember, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "propertyData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BuildVariable && args.Count == 1)
		{
			BuildVariable(VariantUtils.ConvertTo<XWBPVariableData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildProperty && args.Count == 0)
		{
			BuildProperty();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshScriptPresentation && args.Count == 0)
		{
			RefreshScriptPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.GraphNodeBuildRefresh && args.Count == 1)
		{
			GraphNodeBuildRefresh(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCsPropertyName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(GetCsPropertyName(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[2])));
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
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetCsPropertyName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(GetCsPropertyName(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[2])));
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
		if (method == MethodName.BuildVariable)
		{
			return true;
		}
		if (method == MethodName.BuildProperty)
		{
			return true;
		}
		if (method == MethodName.RefreshScriptPresentation)
		{
			return true;
		}
		if (method == MethodName.GraphNodeBuildRefresh)
		{
			return true;
		}
		if (method == MethodName.GetCsPropertyName)
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
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.VariableId)
		{
			VariableId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.MethodType)
		{
			MethodType = VariantUtils.ConvertTo<Type>(in value);
			return true;
		}
		if (name == PropertyName.PropertyData)
		{
			PropertyData = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.VariableId)
		{
			value = VariantUtils.CreateFrom<int>(VariableId);
			return true;
		}
		if (name == PropertyName.MethodType)
		{
			value = VariantUtils.CreateFrom<Type>(MethodType);
			return true;
		}
		if (name == PropertyName.PropertyData)
		{
			value = VariantUtils.CreateFrom<Dictionary>(PropertyData);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.VariableId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.MethodType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.PropertyData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.VariableId, Variant.From<int>(VariableId));
		info.AddProperty(PropertyName.MethodType, Variant.From<Type>(MethodType));
		info.AddProperty(PropertyName.PropertyData, Variant.From<Dictionary>(PropertyData));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.VariableId, out var value))
		{
			VariableId = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.MethodType, out var value2))
		{
			MethodType = value2.As<Type>();
		}
		if (info.TryGetProperty(PropertyName.PropertyData, out var value3))
		{
			PropertyData = value3.As<Dictionary>();
		}
	}
}
