using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/Resource/XWBPFunctionSerializeData.cs")]
public class XWBPFunctionSerializeData : XWBPGraphSerializeData
{
	public new class MethodName : XWBPGraphSerializeData.MethodName
	{
		public new static readonly StringName Serialize = "Serialize";

		public new static readonly StringName Deserialize = "Deserialize";

		public new static readonly StringName Clear = "Clear";
	}

	public new class PropertyName : XWBPGraphSerializeData.PropertyName
	{
		public static readonly StringName FunctionNodeId = "FunctionNodeId";

		public static readonly StringName Inputs = "Inputs";

		public static readonly StringName Outputs = "Outputs";

		public static readonly StringName StateMachineCallbackEnabled = "StateMachineCallbackEnabled";

		public static readonly StringName StateMachineCallbackLocalKey = "StateMachineCallbackLocalKey";

		public static readonly StringName StateMachineCallbackPhase = "StateMachineCallbackPhase";
	}

	public new class SignalName : XWBPGraphSerializeData.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int FunctionNodeId { get; set; }

	[Export(PropertyHint.None, "")]
	public Array<XWBPNodePortSerializeData> Inputs { get; set; } = new Array<XWBPNodePortSerializeData>();

	[Export(PropertyHint.None, "")]
	public Array<XWBPNodePortSerializeData> Outputs { get; set; } = new Array<XWBPNodePortSerializeData>();

	[Export(PropertyHint.None, "")]
	public bool StateMachineCallbackEnabled { get; set; }

	[Export(PropertyHint.None, "")]
	public string StateMachineCallbackLocalKey { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public StateMachineCallbackPhase StateMachineCallbackPhase { get; set; }

	public override void Serialize(XWBPGraphData data)
	{
		Clear();
		base.Serialize(data);
		if (!(data is XWBPFunctionData xWBPFunctionData))
		{
			return;
		}
		FunctionNodeId = xWBPFunctionData.FunctionNodeId;
		StateMachineCallbackEnabled = xWBPFunctionData.StateMachineCallbackEnabled;
		StateMachineCallbackLocalKey = xWBPFunctionData.StateMachineCallbackLocalKey ?? "";
		StateMachineCallbackPhase = xWBPFunctionData.StateMachineCallbackPhase;
		foreach (XWBPNodePortData input in xWBPFunctionData.Inputs)
		{
			if (input != null)
			{
				XWBPNodePortSerializeData xWBPNodePortSerializeData = new XWBPNodePortSerializeData();
				xWBPNodePortSerializeData.Serialize(input);
				Inputs.Add(xWBPNodePortSerializeData);
			}
		}
		foreach (XWBPNodePortData output in xWBPFunctionData.Outputs)
		{
			if (output != null)
			{
				XWBPNodePortSerializeData xWBPNodePortSerializeData2 = new XWBPNodePortSerializeData();
				xWBPNodePortSerializeData2.Serialize(output);
				Outputs.Add(xWBPNodePortSerializeData2);
			}
		}
	}

	public override XWBPGraphData Deserialize()
	{
		XWBPFunctionData xWBPFunctionData = new XWBPFunctionData
		{
			Id = Id,
			Name = Name,
			NextNodeId = NextNodeId,
			Lock = Lock,
			FunctionNodeId = FunctionNodeId,
			StateMachineCallbackEnabled = StateMachineCallbackEnabled,
			StateMachineCallbackLocalKey = (StateMachineCallbackLocalKey ?? ""),
			StateMachineCallbackPhase = StateMachineCallbackPhase
		};
		int num = 0;
		foreach (XWBPNodeSerializeData nodesDatum in NodesData)
		{
			if (nodesDatum == null)
			{
				continue;
			}
			XWBPNodeData xWBPNodeData = nodesDatum.Deserialize();
			if (xWBPNodeData != null && xWBPNodeData.Id != 0 && !xWBPFunctionData.Nodes.ContainsKey(xWBPNodeData.Id))
			{
				xWBPFunctionData.Nodes[xWBPNodeData.Id] = xWBPNodeData;
				if (xWBPNodeData.Id > num)
				{
					num = xWBPNodeData.Id;
				}
			}
		}
		foreach (XWBPNodeConnectionSerializeData connectionsDatum in ConnectionsData)
		{
			if (connectionsDatum != null)
			{
				XWBPNodeConnectionData xWBPNodeConnectionData = connectionsDatum.Deserialize();
				if (xWBPNodeConnectionData != null)
				{
					xWBPFunctionData.Connections.Add(xWBPNodeConnectionData);
				}
			}
		}
		if (xWBPFunctionData.NextNodeId <= num)
		{
			xWBPFunctionData.NextNodeId = num + 1;
		}
		if (xWBPFunctionData.NextNodeId < 1)
		{
			xWBPFunctionData.NextNodeId = 1;
		}
		xWBPFunctionData.SanitizeConnections();
		foreach (XWBPNodePortSerializeData input in Inputs)
		{
			if (input != null)
			{
				XWBPNodePortData xWBPNodePortData = input.Deserialize();
				if (xWBPNodePortData != null)
				{
					xWBPFunctionData.Inputs.Add(xWBPNodePortData);
				}
			}
		}
		foreach (XWBPNodePortSerializeData output in Outputs)
		{
			if (output != null)
			{
				XWBPNodePortData xWBPNodePortData2 = output.Deserialize();
				if (xWBPNodePortData2 != null)
				{
					xWBPFunctionData.Outputs.Add(xWBPNodePortData2);
				}
			}
		}
		return xWBPFunctionData;
	}

	public override void Clear()
	{
		base.Clear();
		FunctionNodeId = 0;
		StateMachineCallbackEnabled = false;
		StateMachineCallbackLocalKey = "";
		StateMachineCallbackPhase = StateMachineCallbackPhase.Enter;
		Inputs.Clear();
		Outputs.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Serialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Deserialize, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Serialize && args.Count == 1)
		{
			Serialize(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Deserialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(Deserialize());
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Serialize)
		{
			return true;
		}
		if (method == MethodName.Deserialize)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.FunctionNodeId)
		{
			FunctionNodeId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Inputs)
		{
			Inputs = VariantUtils.ConvertToArray<XWBPNodePortSerializeData>(in value);
			return true;
		}
		if (name == PropertyName.Outputs)
		{
			Outputs = VariantUtils.ConvertToArray<XWBPNodePortSerializeData>(in value);
			return true;
		}
		if (name == PropertyName.StateMachineCallbackEnabled)
		{
			StateMachineCallbackEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.StateMachineCallbackLocalKey)
		{
			StateMachineCallbackLocalKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.StateMachineCallbackPhase)
		{
			StateMachineCallbackPhase = VariantUtils.ConvertTo<StateMachineCallbackPhase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.FunctionNodeId)
		{
			value = VariantUtils.CreateFrom<int>(FunctionNodeId);
			return true;
		}
		if (name == PropertyName.Inputs)
		{
			value = VariantUtils.CreateFromArray(Inputs);
			return true;
		}
		if (name == PropertyName.Outputs)
		{
			value = VariantUtils.CreateFromArray(Outputs);
			return true;
		}
		if (name == PropertyName.StateMachineCallbackEnabled)
		{
			value = VariantUtils.CreateFrom<bool>(StateMachineCallbackEnabled);
			return true;
		}
		if (name == PropertyName.StateMachineCallbackLocalKey)
		{
			value = VariantUtils.CreateFrom<string>(StateMachineCallbackLocalKey);
			return true;
		}
		if (name == PropertyName.StateMachineCallbackPhase)
		{
			value = VariantUtils.CreateFrom<StateMachineCallbackPhase>(StateMachineCallbackPhase);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.FunctionNodeId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Inputs, PropertyHint.TypeString, "24/17:XWBPNodePortSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Outputs, PropertyHint.TypeString, "24/17:XWBPNodePortSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.StateMachineCallbackEnabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.StateMachineCallbackLocalKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.StateMachineCallbackPhase, PropertyHint.Enum, "Enter,Exit,Process,PhysicsProcess,Guard", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.FunctionNodeId, Variant.From<int>(FunctionNodeId));
		info.AddProperty(PropertyName.Inputs, Variant.CreateFrom(Inputs));
		info.AddProperty(PropertyName.Outputs, Variant.CreateFrom(Outputs));
		info.AddProperty(PropertyName.StateMachineCallbackEnabled, Variant.From<bool>(StateMachineCallbackEnabled));
		info.AddProperty(PropertyName.StateMachineCallbackLocalKey, Variant.From<string>(StateMachineCallbackLocalKey));
		info.AddProperty(PropertyName.StateMachineCallbackPhase, Variant.From<StateMachineCallbackPhase>(StateMachineCallbackPhase));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.FunctionNodeId, out var value))
		{
			FunctionNodeId = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Inputs, out var value2))
		{
			Inputs = value2.AsGodotArray<XWBPNodePortSerializeData>();
		}
		if (info.TryGetProperty(PropertyName.Outputs, out var value3))
		{
			Outputs = value3.AsGodotArray<XWBPNodePortSerializeData>();
		}
		if (info.TryGetProperty(PropertyName.StateMachineCallbackEnabled, out var value4))
		{
			StateMachineCallbackEnabled = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.StateMachineCallbackLocalKey, out var value5))
		{
			StateMachineCallbackLocalKey = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.StateMachineCallbackPhase, out var value6))
		{
			StateMachineCallbackPhase = value6.As<StateMachineCallbackPhase>();
		}
	}
}
