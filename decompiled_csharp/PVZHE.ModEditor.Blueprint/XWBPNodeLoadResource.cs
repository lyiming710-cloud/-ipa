using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.ScriptEditor;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/NodeTypes/Utilities/XWBPNodeLoadResource.cs")]
public class XWBPNodeLoadResource : XWBPNodeType
{
	public new class MethodName : XWBPNodeType.MethodName
	{
		public static readonly StringName CreateForPath = "CreateForPath";

		public new static readonly StringName CreateNodeData = "CreateNodeData";

		public new static readonly StringName LoadNodeData = "LoadNodeData";

		public new static readonly StringName GraphNodeBuildRefresh = "GraphNodeBuildRefresh";

		public new static readonly StringName GenerateCode = "GenerateCode";

		public new static readonly StringName GenerateCodeFromGraph = "GenerateCodeFromGraph";

		public new static readonly StringName GetOutputValue = "GetOutputValue";

		public new static readonly StringName GetOutputValueFromGraph = "GetOutputValueFromGraph";

		public static readonly StringName NormalizeResourcePath = "NormalizeResourcePath";

		public static readonly StringName EscapeStringLiteral = "EscapeStringLiteral";

		public static readonly StringName ApplyResourceMetadata = "ApplyResourceMetadata";

		public static readonly StringName RebuildResourceOutput = "RebuildResourceOutput";

		public static readonly StringName BuildLoadExpression = "BuildLoadExpression";

		public static readonly StringName GetResourcePath = "GetResourcePath";

		public static readonly StringName GetResourceClass = "GetResourceClass";

		public static readonly StringName ResolveResourceClass = "ResolveResourceClass";

		public static readonly StringName NormalizeResourceClass = "NormalizeResourceClass";

		public static readonly StringName IsSafeTypeName = "IsSafeTypeName";
	}

	public new class PropertyName : XWBPNodeType.PropertyName
	{
		public static readonly StringName ResourcePath = "ResourcePath";

		public static readonly StringName ResourceClass = "ResourceClass";
	}

	public new class SignalName : XWBPNodeType.SignalName
	{
	}

	private const string OutputPortName = "资源";

	private const string MetaResourcePath = "ResourcePath";

	private const string MetaResourceClass = "ResourceClass";

	public string ResourcePath { get; set; } = "";

	public string ResourceClass { get; set; } = "Resource";

	public XWBPNodeLoadResource()
	{
		TypeId = "__XWBPGraphNode_LoadResource";
		Category = "实用工具";
		DisplayName = "加载资源";
		Description = "输出拖入蓝图图表的资源。";
		Color = new Color(0.36f, 0.46f, 0.62f);
		AddOutput("资源", XWBPNodePortData.PortType.Object, "Resource");
	}

	public static XWBPNodeLoadResource CreateForPath(string resourcePath, Resource resource = null)
	{
		string text = NormalizeResourcePath(resourcePath);
		if (XWCSharpOnlyPolicy.IsUnsupportedScriptSourcePath(text) || !XWCSharpOnlyPolicy.IsSupportedModScriptResource(resource))
		{
			return null;
		}
		return new XWBPNodeLoadResource
		{
			ResourcePath = text,
			ResourceClass = ResolveResourceClass(text, resource)
		};
	}

	public override XWBPNodeData CreateNodeData()
	{
		XWBPNodeData xWBPNodeData = base.CreateNodeData();
		ApplyResourceMetadata(xWBPNodeData, ResourcePath, ResourceClass);
		RebuildResourceOutput(xWBPNodeData, ResourceClass);
		return xWBPNodeData;
	}

	public override void LoadNodeData(XWBPNodeData nodeData)
	{
		if (nodeData != null)
		{
			ResourcePath = GetResourcePath(nodeData);
			ResourceClass = GetResourceClass(nodeData);
			RebuildResourceOutput(nodeData, ResourceClass);
		}
	}

	public override void GraphNodeBuildRefresh(GodotObject graphNode)
	{
		if (graphNode is XWBPGraphNode { NodeData: not null } xWBPGraphNode)
		{
			string resourcePath = GetResourcePath(xWBPGraphNode.NodeData);
			string resourceClass = GetResourceClass(xWBPGraphNode.NodeData);
			RebuildResourceOutput(xWBPGraphNode.NodeData, resourceClass);
			ApplyResourceMetadata(xWBPGraphNode.NodeData, resourcePath, resourceClass);
			if (!string.IsNullOrWhiteSpace(resourcePath))
			{
				xWBPGraphNode.Title = "加载 " + resourcePath.GetFile();
			}
		}
	}

	public override void GenerateCode(XWBPCodeGenerator generator, XWBPNodeData node, XWBPFunctionData function)
	{
	}

	public override void GenerateCodeFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, XWBPGraphData graph)
	{
	}

	public override string GetOutputValue(XWBPCodeGenerator generator, XWBPNodeData node, string portName, XWBPFunctionData function)
	{
		if (!(portName == "资源"))
		{
			return "null";
		}
		return BuildLoadExpression(generator, node);
	}

	public override string GetOutputValueFromGraph(XWBPCodeGenerator generator, XWBPNodeData node, string portName, XWBPGraphData graph)
	{
		if (!(portName == "资源"))
		{
			return "null";
		}
		return BuildLoadExpression(generator, node);
	}

	public static string NormalizeResourcePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		string text = path.Replace('\\', '/').Trim();
		if (text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || text.StartsWith("uid://", StringComparison.OrdinalIgnoreCase))
		{
			return text;
		}
		string text2 = ProjectSettings.GlobalizePath("res://").Replace('\\', '/').TrimEnd('/');
		if (!string.IsNullOrWhiteSpace(text2) && text.StartsWith(text2, StringComparison.OrdinalIgnoreCase))
		{
			string text3 = text;
			int length = text2.Length;
			string text4 = text3.Substring(length, text3.Length - length).TrimStart('/');
			return "res://" + text4;
		}
		return text;
	}

	public static string EscapeStringLiteral(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return "";
		}
		return value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r")
			.Replace("\n", "\\n")
			.Replace("\t", "\\t");
	}

	private static void ApplyResourceMetadata(XWBPNodeData nodeData, string resourcePath, string resourceClass)
	{
		if (nodeData != null)
		{
			nodeData.SetMetaData("ResourcePath", NormalizeResourcePath(resourcePath));
			nodeData.SetMetaData("ResourceClass", NormalizeResourceClass(resourceClass));
		}
	}

	private static void RebuildResourceOutput(XWBPNodeData nodeData, string resourceClass)
	{
		if (nodeData != null)
		{
			nodeData.InputPorts.Clear();
			nodeData.OutputPorts.Clear();
			nodeData.OutputPorts.Add(new XWBPNodePortData("资源", XWBPNodePortData.Direction.Output, XWBPNodePortData.PortType.Object, NormalizeResourceClass(resourceClass), default));
			nodeData.RebuildPortMap();
		}
	}

	private static string BuildLoadExpression(XWBPCodeGenerator generator, XWBPNodeData node)
	{
		string resourcePath = GetResourcePath(node);
		if (string.IsNullOrWhiteSpace(resourcePath))
		{
			return "null";
		}
		if (XWCSharpOnlyPolicy.IsUnsupportedScriptSourcePath(resourcePath))
		{
			return "null";
		}
		string resourceClass = GetResourceClass(node);
		string value = ((generator != null) ? generator.ToCsClassName(resourceClass) : resourceClass);
		if (!IsSafeTypeName(value))
		{
			value = "Resource";
		}
		return $"GD.Load<{value}>(\"{EscapeStringLiteral(resourcePath)}\")";
	}

	private static string GetResourcePath(XWBPNodeData nodeData)
	{
		if (nodeData == null)
		{
			return "";
		}
		return NormalizeResourcePath(nodeData.GetMetaData("ResourcePath").AsString());
	}

	private static string GetResourceClass(XWBPNodeData nodeData)
	{
		if (nodeData == null)
		{
			return "Resource";
		}
		return NormalizeResourceClass(nodeData.GetMetaData("ResourceClass").AsString());
	}

	private static string ResolveResourceClass(string resourcePath, Resource resource)
	{
		string resourceClass;
		switch ((resourcePath ?? "").GetExtension().ToLowerInvariant())
		{
		case "tscn":
		case "scn":
			resourceClass = "PackedScene";
			break;
		case "jpeg":
		case "webp":
		case "svg":
		case "png":
		case "jpg":
		case "bmp":
		case "tga":
			resourceClass = "Texture2D";
			break;
		case "wav":
		case "ogg":
		case "mp3":
			resourceClass = "AudioStream";
			break;
		case "woff":
		case "font":
		case "ttf":
		case "otf":
		case "fnt":
		case "woff2":
			resourceClass = "Font";
			break;
		case "cs":
			resourceClass = "Script";
			break;
		default:
			resourceClass = resource?.GetClass() ?? "Resource";
			break;
		}
		return NormalizeResourceClass(resourceClass);
	}

	private static string NormalizeResourceClass(string resourceClass)
	{
		if (string.IsNullOrWhiteSpace(resourceClass))
		{
			return "Resource";
		}
		if (!IsSafeTypeName(resourceClass))
		{
			return "Resource";
		}
		return resourceClass;
	}

	private static bool IsSafeTypeName(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		string[] array = value.Split('.');
		foreach (string text in array)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
			if (!char.IsLetter(text[0]) && text[0] != '_')
			{
				return false;
			}
			for (int j = 1; j < text.Length; j++)
			{
				char c = text[j];
				if (!char.IsLetterOrDigit(c) && c != '_')
				{
					return false;
				}
			}
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName.CreateForPath, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateNodeData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadNodeData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
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
			new MethodInfo(MethodName.NormalizeResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EscapeStringLiteral, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyResourceMetadata, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "resourceClass", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildResourceOutput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.String, "resourceClass", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildLoadExpression, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "generator", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetResourceClass, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveResourceClass, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeResourceClass, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourceClass", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSafeTypeName, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateForPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeLoadResource>(CreateForPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
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
		if (method == MethodName.NormalizeResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.EscapeStringLiteral && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EscapeStringLiteral(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyResourceMetadata && args.Count == 3)
		{
			ApplyResourceMetadata(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildResourceOutput && args.Count == 2)
		{
			RebuildResourceOutput(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildLoadExpression && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildLoadExpression(VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[0]), VariantUtils.ConvertTo<XWBPNodeData>(in args[1])));
			return true;
		}
		if (method == MethodName.GetResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourcePath(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetResourceClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourceClass(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveResourceClass && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveResourceClass(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeResourceClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResourceClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSafeTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSafeTypeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateForPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeLoadResource>(CreateForPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.EscapeStringLiteral && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EscapeStringLiteral(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyResourceMetadata && args.Count == 3)
		{
			ApplyResourceMetadata(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildResourceOutput && args.Count == 2)
		{
			RebuildResourceOutput(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildLoadExpression && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildLoadExpression(VariantUtils.ConvertTo<XWBPCodeGenerator>(in args[0]), VariantUtils.ConvertTo<XWBPNodeData>(in args[1])));
			return true;
		}
		if (method == MethodName.GetResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourcePath(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetResourceClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourceClass(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveResourceClass && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveResourceClass(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeResourceClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResourceClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSafeTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSafeTypeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.CreateForPath)
		{
			return true;
		}
		if (method == MethodName.CreateNodeData)
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
		if (method == MethodName.NormalizeResourcePath)
		{
			return true;
		}
		if (method == MethodName.EscapeStringLiteral)
		{
			return true;
		}
		if (method == MethodName.ApplyResourceMetadata)
		{
			return true;
		}
		if (method == MethodName.RebuildResourceOutput)
		{
			return true;
		}
		if (method == MethodName.BuildLoadExpression)
		{
			return true;
		}
		if (method == MethodName.GetResourcePath)
		{
			return true;
		}
		if (method == MethodName.GetResourceClass)
		{
			return true;
		}
		if (method == MethodName.ResolveResourceClass)
		{
			return true;
		}
		if (method == MethodName.NormalizeResourceClass)
		{
			return true;
		}
		if (method == MethodName.IsSafeTypeName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ResourcePath)
		{
			ResourcePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ResourceClass)
		{
			ResourceClass = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.ResourcePath)
		{
			from = ResourcePath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ResourceClass)
		{
			from = ResourceClass;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ResourcePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ResourceClass, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ResourcePath, Variant.From<string>(ResourcePath));
		info.AddProperty(PropertyName.ResourceClass, Variant.From<string>(ResourceClass));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ResourcePath, out var value))
		{
			ResourcePath = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ResourceClass, out var value2))
		{
			ResourceClass = value2.As<string>();
		}
	}
}
