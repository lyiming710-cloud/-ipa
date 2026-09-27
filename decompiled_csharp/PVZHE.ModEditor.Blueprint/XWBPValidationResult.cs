using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/Validator/XWBPValidationResult.cs")]
public class XWBPValidationResult : RefCounted
{
	public enum Severity
	{
		Error,
		Warning,
		Info
	}

	public enum ErrorType
	{
		DeadLoop,
		UnconnectedPort,
		MissingMethod,
		MissingVariable,
		MissingSignal,
		MissingEntry,
		OrphanNode,
		InvalidConnection,
		DuplicateEntry,
		DuplicateSignalName,
		DuplicateVariableName,
		DuplicateFunctionName,
		DuplicateParameterName,
		MissingReturn,
		UnsupportedNode,
		GeneratedCode
	}

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName SetNodeId = "SetNodeId";

		public static readonly StringName SetPortIndex = "SetPortIndex";

		public static readonly StringName SetGraphData = "SetGraphData";

		public static readonly StringName SetPosition = "SetPosition";

		public static readonly StringName GetSeverityText = "GetSeverityText";

		public static readonly StringName GetErrorTypeText = "GetErrorTypeText";

		public new static readonly StringName ToString = "ToString";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName ResultSeverity = "ResultSeverity";

		public static readonly StringName TypeError = "TypeError";

		public static readonly StringName Message = "Message";

		public static readonly StringName NodeId = "NodeId";

		public static readonly StringName PortIndex = "PortIndex";

		public static readonly StringName GraphData = "GraphData";

		public static readonly StringName Position = "Position";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public Severity ResultSeverity { get; set; }

	public ErrorType TypeError { get; set; }

	public string Message { get; set; } = "";

	public int NodeId { get; set; } = -1;

	public int PortIndex { get; set; } = -1;

	public XWBPGraphData GraphData { get; set; }

	public Vector2 Position { get; set; } = Vector2.Zero;

	public XWBPValidationResult()
	{
	}

	public XWBPValidationResult(ErrorType errorType, string message, Severity severity = Severity.Error)
	{
		TypeError = errorType;
		Message = message;
		ResultSeverity = severity;
	}

	public XWBPValidationResult SetNodeId(int nodeId)
	{
		NodeId = nodeId;
		return this;
	}

	public XWBPValidationResult SetPortIndex(int portIndex)
	{
		PortIndex = portIndex;
		return this;
	}

	public XWBPValidationResult SetGraphData(XWBPGraphData graphData)
	{
		GraphData = graphData;
		return this;
	}

	public XWBPValidationResult SetPosition(Vector2 position)
	{
		Position = position;
		return this;
	}

	public string GetSeverityText()
	{
		return ResultSeverity switch
		{
			Severity.Error => "错误", 
			Severity.Warning => "警告", 
			Severity.Info => "信息", 
			_ => "未知", 
		};
	}

	public string GetErrorTypeText()
	{
		return TypeError switch
		{
			ErrorType.DeadLoop => "死循环", 
			ErrorType.UnconnectedPort => "未连接端口", 
			ErrorType.MissingMethod => "方法丢失", 
			ErrorType.MissingVariable => "变量丢失", 
			ErrorType.MissingSignal => "信号丢失", 
			ErrorType.MissingEntry => "缺少入口", 
			ErrorType.OrphanNode => "孤立节点", 
			ErrorType.InvalidConnection => "无效连接", 
			ErrorType.DuplicateEntry => "重复入口", 
			ErrorType.DuplicateSignalName => "重复信号名", 
			ErrorType.DuplicateVariableName => "重复变量名", 
			ErrorType.DuplicateFunctionName => "重复函数名", 
			ErrorType.DuplicateParameterName => "重复参数名", 
			ErrorType.MissingReturn => "缺少返回节点", 
			ErrorType.UnsupportedNode => "不支持的节点", 
			ErrorType.GeneratedCode => "生成代码错误", 
			_ => "未知错误", 
		};
	}

	public override string ToString()
	{
		string text = $"[{GetSeverityText()}] {GetErrorTypeText()}: {Message}";
		if (NodeId >= 0)
		{
			text += $" (节点ID: {NodeId}";
			if (PortIndex >= 0)
			{
				text += $", 端口: {PortIndex}";
			}
			text += ")";
		}
		return text;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.SetNodeId, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPortIndex, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "portIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetGraphData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graphData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetPosition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSeverityText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetErrorTypeText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetNodeId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPValidationResult>(SetNodeId(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPortIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPValidationResult>(SetPortIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetGraphData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPValidationResult>(SetGraphData(VariantUtils.ConvertTo<XWBPGraphData>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPValidationResult>(SetPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSeverityText && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetSeverityText());
			return true;
		}
		if (method == MethodName.GetErrorTypeText && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetErrorTypeText());
			return true;
		}
		if (method == MethodName.ToString && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(ToString());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SetNodeId)
		{
			return true;
		}
		if (method == MethodName.SetPortIndex)
		{
			return true;
		}
		if (method == MethodName.SetGraphData)
		{
			return true;
		}
		if (method == MethodName.SetPosition)
		{
			return true;
		}
		if (method == MethodName.GetSeverityText)
		{
			return true;
		}
		if (method == MethodName.GetErrorTypeText)
		{
			return true;
		}
		if (method == MethodName.ToString)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ResultSeverity)
		{
			ResultSeverity = VariantUtils.ConvertTo<Severity>(in value);
			return true;
		}
		if (name == PropertyName.TypeError)
		{
			TypeError = VariantUtils.ConvertTo<ErrorType>(in value);
			return true;
		}
		if (name == PropertyName.Message)
		{
			Message = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.NodeId)
		{
			NodeId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.PortIndex)
		{
			PortIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.GraphData)
		{
			GraphData = VariantUtils.ConvertTo<XWBPGraphData>(in value);
			return true;
		}
		if (name == PropertyName.Position)
		{
			Position = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ResultSeverity)
		{
			value = VariantUtils.CreateFrom<Severity>(ResultSeverity);
			return true;
		}
		if (name == PropertyName.TypeError)
		{
			value = VariantUtils.CreateFrom<ErrorType>(TypeError);
			return true;
		}
		if (name == PropertyName.Message)
		{
			value = VariantUtils.CreateFrom<string>(Message);
			return true;
		}
		int from;
		if (name == PropertyName.NodeId)
		{
			from = NodeId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PortIndex)
		{
			from = PortIndex;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.GraphData)
		{
			value = VariantUtils.CreateFrom<XWBPGraphData>(GraphData);
			return true;
		}
		if (name == PropertyName.Position)
		{
			value = VariantUtils.CreateFrom<Vector2>(Position);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.ResultSeverity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TypeError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Message, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.NodeId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PortIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.GraphData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Position, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ResultSeverity, Variant.From<Severity>(ResultSeverity));
		info.AddProperty(PropertyName.TypeError, Variant.From<ErrorType>(TypeError));
		info.AddProperty(PropertyName.Message, Variant.From<string>(Message));
		info.AddProperty(PropertyName.NodeId, Variant.From<int>(NodeId));
		info.AddProperty(PropertyName.PortIndex, Variant.From<int>(PortIndex));
		info.AddProperty(PropertyName.GraphData, Variant.From<XWBPGraphData>(GraphData));
		info.AddProperty(PropertyName.Position, Variant.From<Vector2>(Position));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ResultSeverity, out var value))
		{
			ResultSeverity = value.As<Severity>();
		}
		if (info.TryGetProperty(PropertyName.TypeError, out var value2))
		{
			TypeError = value2.As<ErrorType>();
		}
		if (info.TryGetProperty(PropertyName.Message, out var value3))
		{
			Message = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.NodeId, out var value4))
		{
			NodeId = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.PortIndex, out var value5))
		{
			PortIndex = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.GraphData, out var value6))
		{
			GraphData = value6.As<XWBPGraphData>();
		}
		if (info.TryGetProperty(PropertyName.Position, out var value7))
		{
			Position = value7.As<Vector2>();
		}
	}
}
