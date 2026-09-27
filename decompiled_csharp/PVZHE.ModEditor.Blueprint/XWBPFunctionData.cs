using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/XWBPFunctionData.cs")]
public class XWBPFunctionData : XWBPGraphData
{
	[Signal]
	public delegate void PortChangeEventHandler();

	public new class MethodName : XWBPGraphData.MethodName
	{
		public static readonly StringName Create = "Create";

		public static readonly StringName Duplicate = "Duplicate";

		public static readonly StringName InputsSet = "InputsSet";

		public static readonly StringName OutputsSet = "OutputsSet";

		public static readonly StringName AutoAddReturnNode = "AutoAddReturnNode";

		public static readonly StringName AutoRemoveReturnNodes = "AutoRemoveReturnNodes";

		public static readonly StringName PortChangeEmit = "PortChangeEmit";
	}

	public new class PropertyName : XWBPGraphData.PropertyName
	{
		public static readonly StringName FunctionNodeId = "FunctionNodeId";

		public static readonly StringName PreviousOutputCount = "PreviousOutputCount";

		public static readonly StringName StateMachineCallbackEnabled = "StateMachineCallbackEnabled";

		public static readonly StringName StateMachineCallbackLocalKey = "StateMachineCallbackLocalKey";

		public static readonly StringName StateMachineCallbackPhase = "StateMachineCallbackPhase";
	}

	public new class SignalName : XWBPGraphData.SignalName
	{
		public static readonly StringName PortChange = "PortChange";
	}

	public const int EntryNodeId = -100000;

	private PortChangeEventHandler backing_PortChange;

	public int FunctionNodeId { get; set; }

	public List<XWBPNodePortData> Inputs { get; set; } = new List<XWBPNodePortData>();

	public List<XWBPNodePortData> Outputs { get; set; } = new List<XWBPNodePortData>();

	public int PreviousOutputCount { get; set; }

	public bool StateMachineCallbackEnabled { get; set; }

	public string StateMachineCallbackLocalKey { get; set; } = "";

	public StateMachineCallbackPhase StateMachineCallbackPhase { get; set; }

	public event PortChangeEventHandler PortChange
	{
		add
		{
			backing_PortChange = (PortChangeEventHandler)Delegate.Combine(backing_PortChange, value);
		}
		remove
		{
			backing_PortChange = (PortChangeEventHandler)Delegate.Remove(backing_PortChange, value);
		}
	}

	public static XWBPFunctionData Create()
	{
		XWBPFunctionData xWBPFunctionData = new XWBPFunctionData();
		XWBPNodeMethodEntry xWBPNodeMethodEntry = new XWBPNodeMethodEntry
		{
			MethodType = XWBPNodeMethodEntry.Type.Bp
		};
		XWBPNodeData xWBPNodeData = xWBPNodeMethodEntry.CreateNodeData();
		xWBPNodeData.NodeType = xWBPNodeMethodEntry;
		xWBPNodeData.Id = -100000;
		xWBPNodeData.Lock = true;
		xWBPFunctionData.Nodes[-100000] = xWBPNodeData;
		return xWBPFunctionData;
	}

	public XWBPFunctionData Duplicate()
	{
		XWBPFunctionSerializeData xWBPFunctionSerializeData = new XWBPFunctionSerializeData();
		xWBPFunctionSerializeData.Serialize(this);
		return (XWBPFunctionData)xWBPFunctionSerializeData.Deserialize();
	}

	public override List<XWInspectorProperty> GetProperties()
	{
		List<XWInspectorProperty> properties = base.GetProperties();
		properties.Add(new XWInspectorProperty(this, "inputs", default, "函数", "", PropertyHint.None, "", Callable.From(InputsSet)));
		properties.Add(new XWInspectorProperty(this, "outputs", default, "函数", "", PropertyHint.None, "", Callable.From(OutputsSet)));
		return properties;
	}

	public void InputsSet()
	{
		foreach (XWBPNodePortData input in Inputs)
		{
			input.PortDirection = XWBPNodePortData.Direction.Input;
		}
		EnsureUniquePortNames(Inputs);
		PortChangeEmit();
	}

	public void OutputsSet()
	{
		foreach (XWBPNodePortData output in Outputs)
		{
			output.PortDirection = XWBPNodePortData.Direction.Output;
		}
		while (Outputs.Count > 1)
		{
			Outputs.RemoveAt(Outputs.Count - 1);
		}
		EnsureUniquePortNames(Outputs);
		if (PreviousOutputCount == 0 && Outputs.Count > 0)
		{
			AutoAddReturnNode();
		}
		else if (PreviousOutputCount > 0 && Outputs.Count == 0)
		{
			AutoRemoveReturnNodes();
		}
		PreviousOutputCount = Outputs.Count;
		PortChangeEmit();
	}

	public void AutoAddReturnNode()
	{
		bool flag = false;
		foreach (XWBPNodeData value2 in Nodes.Values)
		{
			if (value2.TypeId == (StringName)"__XWBPGraphNode_Return")
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			XWBPNodeData xWBPNodeData = (Nodes.TryGetValue(-100000, out var value) ? value : null);
			Vector2 position = new Vector2(400f, 0f);
			if (xWBPNodeData != null)
			{
				position = xWBPNodeData.Position + new Vector2(800f, 0f);
			}
			XWBPNodeData xWBPNodeData2 = new XWBPNodeReturn().CreateNodeData();
			xWBPNodeData2.Position = position;
			AddNode(xWBPNodeData2);
		}
	}

	public void AutoRemoveReturnNodes()
	{
		List<int> list = new List<int>();
		foreach (XWBPNodeData value in Nodes.Values)
		{
			if (value.TypeId == (StringName)"__XWBPGraphNode_Return")
			{
				list.Add(value.Id);
			}
		}
		foreach (int item in list)
		{
			RemoveNode(item);
		}
	}

	public static void EnsureUniquePortNames(List<XWBPNodePortData> ports)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < ports.Count; i++)
		{
			XWBPNodePortData xWBPNodePortData = ports[i];
			string name = xWBPNodePortData.Name;
			if (dictionary.TryGetValue(name, out var value))
			{
				string text = name + value;
				while (dictionary.ContainsKey(text))
				{
					value++;
					text = name + value;
				}
				xWBPNodePortData.Name = text;
				dictionary[name] = value + 1;
				dictionary[text] = 1;
			}
			else
			{
				dictionary[name] = 1;
			}
		}
	}

	public void PortChangeEmit()
	{
		EmitSignal(SignalName.PortChange);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Duplicate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InputsSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutputsSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AutoAddReturnNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AutoRemoveReturnNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PortChangeEmit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPFunctionData>(Create());
			return true;
		}
		if (method == MethodName.Duplicate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPFunctionData>(Duplicate());
			return true;
		}
		if (method == MethodName.InputsSet && args.Count == 0)
		{
			InputsSet();
			ret = default;
			return true;
		}
		if (method == MethodName.OutputsSet && args.Count == 0)
		{
			OutputsSet();
			ret = default;
			return true;
		}
		if (method == MethodName.AutoAddReturnNode && args.Count == 0)
		{
			AutoAddReturnNode();
			ret = default;
			return true;
		}
		if (method == MethodName.AutoRemoveReturnNodes && args.Count == 0)
		{
			AutoRemoveReturnNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.PortChangeEmit && args.Count == 0)
		{
			PortChangeEmit();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPFunctionData>(Create());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName.Duplicate)
		{
			return true;
		}
		if (method == MethodName.InputsSet)
		{
			return true;
		}
		if (method == MethodName.OutputsSet)
		{
			return true;
		}
		if (method == MethodName.AutoAddReturnNode)
		{
			return true;
		}
		if (method == MethodName.AutoRemoveReturnNodes)
		{
			return true;
		}
		if (method == MethodName.PortChangeEmit)
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
		if (name == PropertyName.PreviousOutputCount)
		{
			PreviousOutputCount = VariantUtils.ConvertTo<int>(in value);
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
		int from;
		if (name == PropertyName.FunctionNodeId)
		{
			from = FunctionNodeId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PreviousOutputCount)
		{
			from = PreviousOutputCount;
			value = VariantUtils.CreateFrom(in from);
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
			new PropertyInfo(Variant.Type.Int, PropertyName.FunctionNodeId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PreviousOutputCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.StateMachineCallbackEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.StateMachineCallbackLocalKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.StateMachineCallbackPhase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.FunctionNodeId, Variant.From<int>(FunctionNodeId));
		info.AddProperty(PropertyName.PreviousOutputCount, Variant.From<int>(PreviousOutputCount));
		info.AddProperty(PropertyName.StateMachineCallbackEnabled, Variant.From<bool>(StateMachineCallbackEnabled));
		info.AddProperty(PropertyName.StateMachineCallbackLocalKey, Variant.From<string>(StateMachineCallbackLocalKey));
		info.AddProperty(PropertyName.StateMachineCallbackPhase, Variant.From<StateMachineCallbackPhase>(StateMachineCallbackPhase));
		info.AddSignalEventDelegate(SignalName.PortChange, backing_PortChange);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.FunctionNodeId, out var value))
		{
			FunctionNodeId = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.PreviousOutputCount, out var value2))
		{
			PreviousOutputCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.StateMachineCallbackEnabled, out var value3))
		{
			StateMachineCallbackEnabled = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.StateMachineCallbackLocalKey, out var value4))
		{
			StateMachineCallbackLocalKey = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.StateMachineCallbackPhase, out var value5))
		{
			StateMachineCallbackPhase = value5.As<StateMachineCallbackPhase>();
		}
		if (info.TryGetSignalEventDelegate<PortChangeEventHandler>(SignalName.PortChange, out var value6))
		{
			backing_PortChange = value6;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.PortChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalPortChange()
	{
		EmitSignal(SignalName.PortChange, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.PortChange && args.Count == 0)
		{
			backing_PortChange?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.PortChange)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
