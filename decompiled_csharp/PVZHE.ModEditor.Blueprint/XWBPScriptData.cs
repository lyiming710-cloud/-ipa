using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/XWBPScriptData.cs")]
public class XWBPScriptData : RefCounted
{
	[Signal]
	public delegate void GraphRemoveEventHandler(int id);

	[Signal]
	public delegate void FunctionRemoveEventHandler(int id);

	[Signal]
	public delegate void VariableRemoveEventHandler(int id);

	[Signal]
	public delegate void SignalRemoveEventHandler(int id);

	[Signal]
	public delegate void ExtendsClassChangedEventHandler(StringName oldClass, StringName newClass);

	[Signal]
	public delegate void BlueprintChangedEventHandler();

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName AddGraph = "AddGraph";

		public static readonly StringName RemoveGraph = "RemoveGraph";

		public static readonly StringName AddFunction = "AddFunction";

		public static readonly StringName RemoveFunction = "RemoveFunction";

		public static readonly StringName AddVariable = "AddVariable";

		public static readonly StringName RemoveVariable = "RemoveVariable";

		public static readonly StringName AddSignal = "AddSignal";

		public static readonly StringName RemoveSignal = "RemoveSignal";

		public static readonly StringName GenerateUniqueGraphName = "GenerateUniqueGraphName";

		public static readonly StringName GenerateUniqueFunctionName = "GenerateUniqueFunctionName";

		public static readonly StringName GenerateUniqueVariableName = "GenerateUniqueVariableName";

		public static readonly StringName GenerateUniqueSignalName = "GenerateUniqueSignalName";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName ExtendsClass = "ExtendsClass";

		public static readonly StringName NextGraphId = "NextGraphId";

		public static readonly StringName NextFunctionId = "NextFunctionId";

		public static readonly StringName NextVariableId = "NextVariableId";

		public static readonly StringName NextSignalId = "NextSignalId";

		public static readonly StringName _extendsClass = "_extendsClass";
	}

	public new class SignalName : RefCounted.SignalName
	{
		public static readonly StringName GraphRemove = "GraphRemove";

		public static readonly StringName FunctionRemove = "FunctionRemove";

		public static readonly StringName VariableRemove = "VariableRemove";

		public static readonly StringName SignalRemove = "SignalRemove";

		public static readonly StringName ExtendsClassChanged = "ExtendsClassChanged";

		public static readonly StringName BlueprintChanged = "BlueprintChanged";
	}

	private StringName _extendsClass = "";

	private GraphRemoveEventHandler backing_GraphRemove;

	private FunctionRemoveEventHandler backing_FunctionRemove;

	private VariableRemoveEventHandler backing_VariableRemove;

	private SignalRemoveEventHandler backing_SignalRemove;

	private ExtendsClassChangedEventHandler backing_ExtendsClassChanged;

	private BlueprintChangedEventHandler backing_BlueprintChanged;

	public StringName ExtendsClass
	{
		get
		{
			return _extendsClass;
		}
		set
		{
			if (_extendsClass != value)
			{
				StringName extendsClass = _extendsClass;
				_extendsClass = value;
				EmitSignal(SignalName.ExtendsClassChanged, extendsClass, value);
				EmitSignal(SignalName.BlueprintChanged);
			}
		}
	}

	public Dictionary<int, XWBPGraphData> Graphs { get; set; } = new Dictionary<int, XWBPGraphData>();

	public Dictionary<int, XWBPFunctionData> Functions { get; set; } = new Dictionary<int, XWBPFunctionData>();

	public Dictionary<int, XWBPVariableData> Variables { get; set; } = new Dictionary<int, XWBPVariableData>();

	public Dictionary<int, XWBPSignalData> SignalDatas { get; set; } = new Dictionary<int, XWBPSignalData>();

	public int NextGraphId { get; set; } = 1;

	public int NextFunctionId { get; set; } = 1;

	public int NextVariableId { get; set; } = 1;

	public int NextSignalId { get; set; } = 1;

	public event GraphRemoveEventHandler GraphRemove
	{
		add
		{
			backing_GraphRemove = (GraphRemoveEventHandler)Delegate.Combine(backing_GraphRemove, value);
		}
		remove
		{
			backing_GraphRemove = (GraphRemoveEventHandler)Delegate.Remove(backing_GraphRemove, value);
		}
	}

	public event FunctionRemoveEventHandler FunctionRemove
	{
		add
		{
			backing_FunctionRemove = (FunctionRemoveEventHandler)Delegate.Combine(backing_FunctionRemove, value);
		}
		remove
		{
			backing_FunctionRemove = (FunctionRemoveEventHandler)Delegate.Remove(backing_FunctionRemove, value);
		}
	}

	public event VariableRemoveEventHandler VariableRemove
	{
		add
		{
			backing_VariableRemove = (VariableRemoveEventHandler)Delegate.Combine(backing_VariableRemove, value);
		}
		remove
		{
			backing_VariableRemove = (VariableRemoveEventHandler)Delegate.Remove(backing_VariableRemove, value);
		}
	}

	public event SignalRemoveEventHandler SignalRemove
	{
		add
		{
			backing_SignalRemove = (SignalRemoveEventHandler)Delegate.Combine(backing_SignalRemove, value);
		}
		remove
		{
			backing_SignalRemove = (SignalRemoveEventHandler)Delegate.Remove(backing_SignalRemove, value);
		}
	}

	public event ExtendsClassChangedEventHandler ExtendsClassChanged
	{
		add
		{
			backing_ExtendsClassChanged = (ExtendsClassChangedEventHandler)Delegate.Combine(backing_ExtendsClassChanged, value);
		}
		remove
		{
			backing_ExtendsClassChanged = (ExtendsClassChangedEventHandler)Delegate.Remove(backing_ExtendsClassChanged, value);
		}
	}

	public event BlueprintChangedEventHandler BlueprintChanged
	{
		add
		{
			backing_BlueprintChanged = (BlueprintChangedEventHandler)Delegate.Combine(backing_BlueprintChanged, value);
		}
		remove
		{
			backing_BlueprintChanged = (BlueprintChangedEventHandler)Delegate.Remove(backing_BlueprintChanged, value);
		}
	}

	public void AddGraph(XWBPGraphData graphData, int id = 0)
	{
		if (id == 0)
		{
			graphData.Id = NextGraphId;
			Graphs[NextGraphId] = graphData;
			NextGraphId++;
		}
		else
		{
			graphData.Id = id;
			Graphs[id] = graphData;
			if (id >= NextGraphId)
			{
				NextGraphId = id + 1;
			}
		}
		EmitSignal(SignalName.BlueprintChanged);
	}

	public void RemoveGraph(int id)
	{
		EmitSignal(SignalName.GraphRemove, id);
		if (Graphs.TryGetValue(id, out var value))
		{
			value.RemoveSelf();
		}
		Graphs.Remove(id);
		EmitSignal(SignalName.BlueprintChanged);
	}

	public void AddFunction(XWBPFunctionData functionData, int id = 0)
	{
		if (id == 0)
		{
			XWBPNodeData xWBPNodeData = functionData.Nodes[-100000];
			xWBPNodeData.SetMetaData("FunctionId", NextFunctionId);
			xWBPNodeData.SetMetaData("MethodType", 0);
			functionData.Id = NextFunctionId;
			Functions[NextFunctionId] = functionData;
			NextFunctionId++;
		}
		else
		{
			XWBPNodeData xWBPNodeData2 = functionData.Nodes[-100000];
			xWBPNodeData2.SetMetaData("FunctionId", id);
			xWBPNodeData2.SetMetaData("MethodType", 0);
			functionData.Id = id;
			Functions[id] = functionData;
			if (id >= NextFunctionId)
			{
				NextFunctionId = id + 1;
			}
		}
		EmitSignal(SignalName.BlueprintChanged);
	}

	public void RemoveFunction(int id)
	{
		EmitSignal(SignalName.FunctionRemove, id);
		if (Functions.TryGetValue(id, out var value))
		{
			value.RemoveSelf();
		}
		Functions.Remove(id);
		EmitSignal(SignalName.BlueprintChanged);
	}

	public void AddVariable(XWBPVariableData variableData, int id = 0)
	{
		if (id == 0)
		{
			variableData.Id = NextVariableId;
			Variables[NextVariableId] = variableData;
			NextVariableId++;
		}
		else
		{
			variableData.Id = id;
			Variables[id] = variableData;
			if (id >= NextVariableId)
			{
				NextVariableId = id + 1;
			}
		}
		EmitSignal(SignalName.BlueprintChanged);
	}

	public void RemoveVariable(int id)
	{
		EmitSignal(SignalName.VariableRemove, id);
		if (Variables.TryGetValue(id, out var value))
		{
			value.RemoveSelf();
		}
		Variables.Remove(id);
		EmitSignal(SignalName.BlueprintChanged);
	}

	public void AddSignal(XWBPSignalData signalData, int id = 0)
	{
		if (id == 0)
		{
			signalData.Id = NextSignalId;
			SignalDatas[NextSignalId] = signalData;
			NextSignalId++;
		}
		else
		{
			signalData.Id = id;
			SignalDatas[id] = signalData;
			if (id >= NextSignalId)
			{
				NextSignalId = id + 1;
			}
		}
		EmitSignal(SignalName.BlueprintChanged);
	}

	public void RemoveSignal(int id)
	{
		EmitSignal(SignalName.SignalRemove, id);
		if (SignalDatas.TryGetValue(id, out var value))
		{
			value.RemoveSelf();
		}
		SignalDatas.Remove(id);
		EmitSignal(SignalName.BlueprintChanged);
	}

	public string GenerateUniqueGraphName(string baseName = "新图表", string excludeName = "")
	{
		List<string> list = new List<string>();
		foreach (XWBPGraphData value in Graphs.Values)
		{
			list.Add(value.Name);
		}
		return GenerateUniqueName(baseName, list, excludeName);
	}

	public string GenerateUniqueFunctionName(string baseName = "新函数", string excludeName = "")
	{
		List<string> list = new List<string>();
		foreach (XWBPFunctionData value in Functions.Values)
		{
			list.Add(value.Name);
		}
		return GenerateUniqueName(baseName, list, excludeName);
	}

	public string GenerateUniqueVariableName(string baseName = "新变量", string excludeName = "")
	{
		List<string> list = new List<string>();
		foreach (XWBPVariableData value in Variables.Values)
		{
			list.Add(value.Name);
		}
		return GenerateUniqueName(baseName, list, excludeName);
	}

	public string GenerateUniqueSignalName(string baseName = "新信号", string excludeName = "")
	{
		List<string> list = new List<string>();
		foreach (XWBPSignalData value in SignalDatas.Values)
		{
			list.Add(value.Name);
		}
		return GenerateUniqueName(baseName, list, excludeName);
	}

	private static string GenerateUniqueName(string baseName, List<string> existingNames, string excludeName = "")
	{
		if (!string.IsNullOrEmpty(excludeName))
		{
			existingNames.Remove(excludeName);
		}
		if (!existingNames.Contains(baseName))
		{
			return baseName;
		}
		int num = 1;
		string text = baseName + num;
		while (existingNames.Contains(text))
		{
			num++;
			text = baseName + num;
		}
		return text;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.AddGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graphData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFunction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "functionData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveFunction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddVariable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "variableData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveVariable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddSignal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "signalData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSignal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateUniqueGraphName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "baseName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "excludeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateUniqueFunctionName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "baseName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "excludeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateUniqueVariableName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "baseName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "excludeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateUniqueSignalName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "baseName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "excludeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AddGraph && args.Count == 2)
		{
			AddGraph(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveGraph && args.Count == 1)
		{
			RemoveGraph(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddFunction && args.Count == 2)
		{
			AddFunction(VariantUtils.ConvertTo<XWBPFunctionData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveFunction && args.Count == 1)
		{
			RemoveFunction(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddVariable && args.Count == 2)
		{
			AddVariable(VariantUtils.ConvertTo<XWBPVariableData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveVariable && args.Count == 1)
		{
			RemoveVariable(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSignal && args.Count == 2)
		{
			AddSignal(VariantUtils.ConvertTo<XWBPSignalData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSignal && args.Count == 1)
		{
			RemoveSignal(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GenerateUniqueGraphName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GenerateUniqueGraphName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GenerateUniqueFunctionName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GenerateUniqueFunctionName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GenerateUniqueVariableName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GenerateUniqueVariableName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GenerateUniqueSignalName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GenerateUniqueSignalName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.AddGraph)
		{
			return true;
		}
		if (method == MethodName.RemoveGraph)
		{
			return true;
		}
		if (method == MethodName.AddFunction)
		{
			return true;
		}
		if (method == MethodName.RemoveFunction)
		{
			return true;
		}
		if (method == MethodName.AddVariable)
		{
			return true;
		}
		if (method == MethodName.RemoveVariable)
		{
			return true;
		}
		if (method == MethodName.AddSignal)
		{
			return true;
		}
		if (method == MethodName.RemoveSignal)
		{
			return true;
		}
		if (method == MethodName.GenerateUniqueGraphName)
		{
			return true;
		}
		if (method == MethodName.GenerateUniqueFunctionName)
		{
			return true;
		}
		if (method == MethodName.GenerateUniqueVariableName)
		{
			return true;
		}
		if (method == MethodName.GenerateUniqueSignalName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ExtendsClass)
		{
			ExtendsClass = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.NextGraphId)
		{
			NextGraphId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.NextFunctionId)
		{
			NextFunctionId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.NextVariableId)
		{
			NextVariableId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.NextSignalId)
		{
			NextSignalId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._extendsClass)
		{
			_extendsClass = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ExtendsClass)
		{
			value = VariantUtils.CreateFrom<StringName>(ExtendsClass);
			return true;
		}
		int from;
		if (name == PropertyName.NextGraphId)
		{
			from = NextGraphId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.NextFunctionId)
		{
			from = NextFunctionId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.NextVariableId)
		{
			from = NextVariableId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.NextSignalId)
		{
			from = NextSignalId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._extendsClass)
		{
			value = VariantUtils.CreateFrom(in _extendsClass);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName._extendsClass, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.ExtendsClass, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.NextGraphId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.NextFunctionId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.NextVariableId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.NextSignalId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ExtendsClass, Variant.From<StringName>(ExtendsClass));
		info.AddProperty(PropertyName.NextGraphId, Variant.From<int>(NextGraphId));
		info.AddProperty(PropertyName.NextFunctionId, Variant.From<int>(NextFunctionId));
		info.AddProperty(PropertyName.NextVariableId, Variant.From<int>(NextVariableId));
		info.AddProperty(PropertyName.NextSignalId, Variant.From<int>(NextSignalId));
		info.AddProperty(PropertyName._extendsClass, Variant.From(in _extendsClass));
		info.AddSignalEventDelegate(SignalName.GraphRemove, backing_GraphRemove);
		info.AddSignalEventDelegate(SignalName.FunctionRemove, backing_FunctionRemove);
		info.AddSignalEventDelegate(SignalName.VariableRemove, backing_VariableRemove);
		info.AddSignalEventDelegate(SignalName.SignalRemove, backing_SignalRemove);
		info.AddSignalEventDelegate(SignalName.ExtendsClassChanged, backing_ExtendsClassChanged);
		info.AddSignalEventDelegate(SignalName.BlueprintChanged, backing_BlueprintChanged);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ExtendsClass, out var value))
		{
			ExtendsClass = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.NextGraphId, out var value2))
		{
			NextGraphId = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.NextFunctionId, out var value3))
		{
			NextFunctionId = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.NextVariableId, out var value4))
		{
			NextVariableId = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.NextSignalId, out var value5))
		{
			NextSignalId = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._extendsClass, out var value6))
		{
			_extendsClass = value6.As<StringName>();
		}
		if (info.TryGetSignalEventDelegate<GraphRemoveEventHandler>(SignalName.GraphRemove, out var value7))
		{
			backing_GraphRemove = value7;
		}
		if (info.TryGetSignalEventDelegate<FunctionRemoveEventHandler>(SignalName.FunctionRemove, out var value8))
		{
			backing_FunctionRemove = value8;
		}
		if (info.TryGetSignalEventDelegate<VariableRemoveEventHandler>(SignalName.VariableRemove, out var value9))
		{
			backing_VariableRemove = value9;
		}
		if (info.TryGetSignalEventDelegate<SignalRemoveEventHandler>(SignalName.SignalRemove, out var value10))
		{
			backing_SignalRemove = value10;
		}
		if (info.TryGetSignalEventDelegate<ExtendsClassChangedEventHandler>(SignalName.ExtendsClassChanged, out var value11))
		{
			backing_ExtendsClassChanged = value11;
		}
		if (info.TryGetSignalEventDelegate<BlueprintChangedEventHandler>(SignalName.BlueprintChanged, out var value12))
		{
			backing_BlueprintChanged = value12;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(SignalName.GraphRemove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.FunctionRemove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.VariableRemove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.SignalRemove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.ExtendsClassChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "oldClass", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "newClass", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.BlueprintChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalGraphRemove(int id)
	{
		EmitSignal(SignalName.GraphRemove, new ReadOnlySpan<Variant>((Variant)id));
	}

	protected void EmitSignalFunctionRemove(int id)
	{
		EmitSignal(SignalName.FunctionRemove, new ReadOnlySpan<Variant>((Variant)id));
	}

	protected void EmitSignalVariableRemove(int id)
	{
		EmitSignal(SignalName.VariableRemove, new ReadOnlySpan<Variant>((Variant)id));
	}

	protected void EmitSignalSignalRemove(int id)
	{
		EmitSignal(SignalName.SignalRemove, new ReadOnlySpan<Variant>((Variant)id));
	}

	protected void EmitSignalExtendsClassChanged(StringName oldClass, StringName newClass)
	{
		StringName extendsClassChanged = SignalName.ExtendsClassChanged;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = oldClass;
		buffer[1] = newClass;
		EmitSignal(extendsClassChanged, buffer);
	}

	protected void EmitSignalBlueprintChanged()
	{
		EmitSignal(SignalName.BlueprintChanged, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.GraphRemove && args.Count == 1)
		{
			backing_GraphRemove?.Invoke(VariantUtils.ConvertTo<int>(in args[0]));
		}
		else if (signal == SignalName.FunctionRemove && args.Count == 1)
		{
			backing_FunctionRemove?.Invoke(VariantUtils.ConvertTo<int>(in args[0]));
		}
		else if (signal == SignalName.VariableRemove && args.Count == 1)
		{
			backing_VariableRemove?.Invoke(VariantUtils.ConvertTo<int>(in args[0]));
		}
		else if (signal == SignalName.SignalRemove && args.Count == 1)
		{
			backing_SignalRemove?.Invoke(VariantUtils.ConvertTo<int>(in args[0]));
		}
		else if (signal == SignalName.ExtendsClassChanged && args.Count == 2)
		{
			backing_ExtendsClassChanged?.Invoke(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
		}
		else if (signal == SignalName.BlueprintChanged && args.Count == 0)
		{
			backing_BlueprintChanged?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.GraphRemove)
		{
			return true;
		}
		if (signal == SignalName.FunctionRemove)
		{
			return true;
		}
		if (signal == SignalName.VariableRemove)
		{
			return true;
		}
		if (signal == SignalName.SignalRemove)
		{
			return true;
		}
		if (signal == SignalName.ExtendsClassChanged)
		{
			return true;
		}
		if (signal == SignalName.BlueprintChanged)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
