using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ScriptEditor;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/Resource/XWBPScript.cs")]
public class XWBPScript : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Create = "Create";

		public static readonly StringName Serialize = "Serialize";

		public static readonly StringName Deserialize = "Deserialize";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName GenerateCode = "GenerateCode";

		public static readonly StringName SaveCode = "SaveCode";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName ExtendsClass = "ExtendsClass";

		public static readonly StringName Graphs = "Graphs";

		public static readonly StringName Functions = "Functions";

		public static readonly StringName Variables = "Variables";

		public static readonly StringName Signals = "Signals";

		public static readonly StringName NextGraphId = "NextGraphId";

		public static readonly StringName NextFunctionId = "NextFunctionId";

		public static readonly StringName NextVariableId = "NextVariableId";

		public static readonly StringName NextSignalId = "NextSignalId";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StringName ExtendsClass { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public Array<XWBPGraphSerializeData> Graphs { get; set; } = new Array<XWBPGraphSerializeData>();

	[Export(PropertyHint.None, "")]
	public Array<XWBPFunctionSerializeData> Functions { get; set; } = new Array<XWBPFunctionSerializeData>();

	[Export(PropertyHint.None, "")]
	public Array<XWBPVariableSerializeData> Variables { get; set; } = new Array<XWBPVariableSerializeData>();

	[Export(PropertyHint.None, "")]
	public Array<XWBPSignalSerializeData> Signals { get; set; } = new Array<XWBPSignalSerializeData>();

	[Export(PropertyHint.None, "")]
	public int NextGraphId { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public int NextFunctionId { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public int NextVariableId { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public int NextSignalId { get; set; } = 1;

	public static XWBPScript Create()
	{
		XWBPScript xWBPScript = new XWBPScript();
		XWBPGraphSerializeData item = new XWBPGraphSerializeData
		{
			Id = 1,
			Lock = true
		};
		xWBPScript.Graphs.Add(item);
		xWBPScript.NextGraphId = 2;
		return xWBPScript;
	}

	public void Serialize(XWBPScriptData data)
	{
		Clear();
		ExtendsClass = data.ExtendsClass;
		foreach (XWBPGraphData value in data.Graphs.Values)
		{
			XWBPGraphSerializeData xWBPGraphSerializeData = new XWBPGraphSerializeData();
			xWBPGraphSerializeData.Serialize(value);
			Graphs.Add(xWBPGraphSerializeData);
		}
		foreach (XWBPFunctionData value2 in data.Functions.Values)
		{
			XWBPFunctionSerializeData xWBPFunctionSerializeData = new XWBPFunctionSerializeData();
			xWBPFunctionSerializeData.Serialize(value2);
			Functions.Add(xWBPFunctionSerializeData);
		}
		foreach (XWBPVariableData value3 in data.Variables.Values)
		{
			XWBPVariableSerializeData xWBPVariableSerializeData = new XWBPVariableSerializeData();
			xWBPVariableSerializeData.Serialize(value3);
			Variables.Add(xWBPVariableSerializeData);
		}
		foreach (XWBPSignalData value4 in data.SignalDatas.Values)
		{
			XWBPSignalSerializeData xWBPSignalSerializeData = new XWBPSignalSerializeData();
			xWBPSignalSerializeData.Serialize(value4);
			Signals.Add(xWBPSignalSerializeData);
		}
		NextGraphId = data.NextGraphId;
		NextFunctionId = data.NextFunctionId;
		NextVariableId = data.NextVariableId;
		NextSignalId = data.NextSignalId;
	}

	public XWBPScriptData Deserialize()
	{
		XWBPScriptData xWBPScriptData = new XWBPScriptData
		{
			ExtendsClass = ExtendsClass,
			NextGraphId = NextGraphId,
			NextFunctionId = NextFunctionId,
			NextVariableId = NextVariableId,
			NextSignalId = NextSignalId
		};
		foreach (XWBPGraphSerializeData graph in Graphs)
		{
			XWBPGraphData xWBPGraphData = graph.Deserialize();
			xWBPGraphData.Owner = xWBPScriptData;
			xWBPScriptData.Graphs[xWBPGraphData.Id] = xWBPGraphData;
		}
		foreach (XWBPFunctionSerializeData function in Functions)
		{
			XWBPFunctionData xWBPFunctionData = (XWBPFunctionData)function.Deserialize();
			xWBPFunctionData.Owner = xWBPScriptData;
			xWBPScriptData.Functions[xWBPFunctionData.Id] = xWBPFunctionData;
		}
		foreach (XWBPVariableSerializeData variable in Variables)
		{
			XWBPVariableData xWBPVariableData = variable.Deserialize();
			xWBPVariableData.Owner = xWBPScriptData;
			xWBPScriptData.Variables[xWBPVariableData.Id] = xWBPVariableData;
		}
		foreach (XWBPSignalSerializeData signal in Signals)
		{
			XWBPSignalData xWBPSignalData = signal.Deserialize();
			xWBPSignalData.Owner = xWBPScriptData;
			xWBPScriptData.SignalDatas[xWBPSignalData.Id] = xWBPSignalData;
		}
		return xWBPScriptData;
	}

	public void Clear()
	{
		NextGraphId = 1;
		NextFunctionId = 1;
		NextVariableId = 1;
		NextSignalId = 1;
		Graphs.Clear();
		Functions.Clear();
		Variables.Clear();
		Signals.Clear();
	}

	public string GenerateCode()
	{
		return new XWBPCodeGenerator(Deserialize())
		{
			ClassName = XWBPCodeGenerator.BuildGeneratedClassName(ResourcePath),
			BlueprintSourcePath = ResourcePath
		}.Generate();
	}

	public Error SaveCode(string path)
	{
		if (string.IsNullOrWhiteSpace(ResourcePath))
		{
			return Error.InvalidParameter;
		}
		string text = ResourcePath.GetBaseDir().PathJoin(ResourcePath.GetFile().GetBaseName() + ".generated.cs");
		if (!string.Equals((path ?? "").Replace('\\', '/'), text.Replace('\\', '/'), StringComparison.OrdinalIgnoreCase))
		{
			return Error.InvalidParameter;
		}
		string text2 = XWBlueprintGeneratedCSharpPolicy.AttachMetadata(GenerateCode(), ResourcePath, text);
		FileAccess fileAccess = FileAccess.Open(path, FileAccess.ModeFlags.Write);
		if (!GodotObject.IsInstanceValid(fileAccess))
		{
			return Error.CantOpen;
		}
		fileAccess.StoreString(text2);
		fileAccess.Close();
		return Error.Ok;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Serialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Deserialize, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GenerateCode, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveCode, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPScript>(Create());
			return true;
		}
		if (method == MethodName.Serialize && args.Count == 1)
		{
			Serialize(VariantUtils.ConvertTo<XWBPScriptData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Deserialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPScriptData>(Deserialize());
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.GenerateCode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GenerateCode());
			return true;
		}
		if (method == MethodName.SaveCode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Error>(SaveCode(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPScript>(Create());
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
		if (method == MethodName.GenerateCode)
		{
			return true;
		}
		if (method == MethodName.SaveCode)
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
		if (name == PropertyName.Graphs)
		{
			Graphs = VariantUtils.ConvertToArray<XWBPGraphSerializeData>(in value);
			return true;
		}
		if (name == PropertyName.Functions)
		{
			Functions = VariantUtils.ConvertToArray<XWBPFunctionSerializeData>(in value);
			return true;
		}
		if (name == PropertyName.Variables)
		{
			Variables = VariantUtils.ConvertToArray<XWBPVariableSerializeData>(in value);
			return true;
		}
		if (name == PropertyName.Signals)
		{
			Signals = VariantUtils.ConvertToArray<XWBPSignalSerializeData>(in value);
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
		if (name == PropertyName.Graphs)
		{
			value = VariantUtils.CreateFromArray(Graphs);
			return true;
		}
		if (name == PropertyName.Functions)
		{
			value = VariantUtils.CreateFromArray(Functions);
			return true;
		}
		if (name == PropertyName.Variables)
		{
			value = VariantUtils.CreateFromArray(Variables);
			return true;
		}
		if (name == PropertyName.Signals)
		{
			value = VariantUtils.CreateFromArray(Signals);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.ExtendsClass, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Graphs, PropertyHint.TypeString, "24/17:XWBPGraphSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Functions, PropertyHint.TypeString, "24/17:XWBPFunctionSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Variables, PropertyHint.TypeString, "24/17:XWBPVariableSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Signals, PropertyHint.TypeString, "24/17:XWBPSignalSerializeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.NextGraphId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.NextFunctionId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.NextVariableId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.NextSignalId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ExtendsClass, Variant.From<StringName>(ExtendsClass));
		info.AddProperty(PropertyName.Graphs, Variant.CreateFrom(Graphs));
		info.AddProperty(PropertyName.Functions, Variant.CreateFrom(Functions));
		info.AddProperty(PropertyName.Variables, Variant.CreateFrom(Variables));
		info.AddProperty(PropertyName.Signals, Variant.CreateFrom(Signals));
		info.AddProperty(PropertyName.NextGraphId, Variant.From<int>(NextGraphId));
		info.AddProperty(PropertyName.NextFunctionId, Variant.From<int>(NextFunctionId));
		info.AddProperty(PropertyName.NextVariableId, Variant.From<int>(NextVariableId));
		info.AddProperty(PropertyName.NextSignalId, Variant.From<int>(NextSignalId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ExtendsClass, out var value))
		{
			ExtendsClass = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.Graphs, out var value2))
		{
			Graphs = value2.AsGodotArray<XWBPGraphSerializeData>();
		}
		if (info.TryGetProperty(PropertyName.Functions, out var value3))
		{
			Functions = value3.AsGodotArray<XWBPFunctionSerializeData>();
		}
		if (info.TryGetProperty(PropertyName.Variables, out var value4))
		{
			Variables = value4.AsGodotArray<XWBPVariableSerializeData>();
		}
		if (info.TryGetProperty(PropertyName.Signals, out var value5))
		{
			Signals = value5.AsGodotArray<XWBPSignalSerializeData>();
		}
		if (info.TryGetProperty(PropertyName.NextGraphId, out var value6))
		{
			NextGraphId = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.NextFunctionId, out var value7))
		{
			NextFunctionId = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.NextVariableId, out var value8))
		{
			NextVariableId = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.NextSignalId, out var value9))
		{
			NextSignalId = value9.As<int>();
		}
	}
}
