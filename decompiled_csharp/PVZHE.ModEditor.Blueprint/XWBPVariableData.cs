using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/Variable/XWBPVariableData.cs")]
public class XWBPVariableData : RefCounted
{
	[Signal]
	public delegate void ChangeEventHandler();

	[Signal]
	public delegate void RemoveEventHandler();

	[Signal]
	public delegate void RenameEventHandler(string name);

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName RemoveSelf = "RemoveSelf";

		public static readonly StringName RenameSelf = "RenameSelf";

		public static readonly StringName VariableSet = "VariableSet";

		public static readonly StringName Duplicate = "Duplicate";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Owner = "Owner";

		public static readonly StringName Id = "Id";

		public static readonly StringName Name = "Name";

		public static readonly StringName Type = "Type";

		public static readonly StringName ClassName = "ClassName";

		public static readonly StringName DefaultValue = "DefaultValue";
	}

	public new class SignalName : RefCounted.SignalName
	{
		public static readonly StringName Change = "Change";

		public static readonly StringName Remove = "Remove";

		public static readonly StringName Rename = "Rename";
	}

	private ChangeEventHandler backing_Change;

	private RemoveEventHandler backing_Remove;

	private RenameEventHandler backing_Rename;

	public XWBPScriptData Owner { get; set; }

	public int Id { get; set; }

	public string Name { get; set; }

	public Variant.Type Type { get; set; }

	public string ClassName { get; set; } = "";

	public Variant DefaultValue { get; set; }

	public event ChangeEventHandler Change
	{
		add
		{
			backing_Change = (ChangeEventHandler)Delegate.Combine(backing_Change, value);
		}
		remove
		{
			backing_Change = (ChangeEventHandler)Delegate.Remove(backing_Change, value);
		}
	}

	public event RemoveEventHandler Remove
	{
		add
		{
			backing_Remove = (RemoveEventHandler)Delegate.Combine(backing_Remove, value);
		}
		remove
		{
			backing_Remove = (RemoveEventHandler)Delegate.Remove(backing_Remove, value);
		}
	}

	public event RenameEventHandler Rename
	{
		add
		{
			backing_Rename = (RenameEventHandler)Delegate.Combine(backing_Rename, value);
		}
		remove
		{
			backing_Rename = (RenameEventHandler)Delegate.Remove(backing_Rename, value);
		}
	}

	public void RemoveSelf()
	{
		EmitSignal(SignalName.Remove);
	}

	public void RenameSelf(string newName)
	{
		Name = newName;
		EmitSignal(SignalName.Rename, newName);
	}

	public List<XWInspectorProperty> GetProperties()
	{
		return new List<XWInspectorProperty>
		{
			new XWInspectorProperty(this, "", default, "属性", "", PropertyHint.None, "", Callable.From(VariableSet))
		};
	}

	public void VariableSet()
	{
		EmitSignal(SignalName.Change);
	}

	public XWBPVariableData Duplicate()
	{
		XWBPVariableSerializeData xWBPVariableSerializeData = new XWBPVariableSerializeData();
		xWBPVariableSerializeData.Serialize(this);
		return xWBPVariableSerializeData.Deserialize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.RemoveSelf, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenameSelf, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.VariableSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Duplicate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RemoveSelf && args.Count == 0)
		{
			RemoveSelf();
			ret = default;
			return true;
		}
		if (method == MethodName.RenameSelf && args.Count == 1)
		{
			RenameSelf(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.VariableSet && args.Count == 0)
		{
			VariableSet();
			ret = default;
			return true;
		}
		if (method == MethodName.Duplicate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPVariableData>(Duplicate());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.RemoveSelf)
		{
			return true;
		}
		if (method == MethodName.RenameSelf)
		{
			return true;
		}
		if (method == MethodName.VariableSet)
		{
			return true;
		}
		if (method == MethodName.Duplicate)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Owner)
		{
			Owner = VariantUtils.ConvertTo<XWBPScriptData>(in value);
			return true;
		}
		if (name == PropertyName.Id)
		{
			Id = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Name)
		{
			Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Type)
		{
			Type = VariantUtils.ConvertTo<Variant.Type>(in value);
			return true;
		}
		if (name == PropertyName.ClassName)
		{
			ClassName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.DefaultValue)
		{
			DefaultValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Owner)
		{
			value = VariantUtils.CreateFrom<XWBPScriptData>(Owner);
			return true;
		}
		if (name == PropertyName.Id)
		{
			value = VariantUtils.CreateFrom<int>(Id);
			return true;
		}
		string from;
		if (name == PropertyName.Name)
		{
			from = Name;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Type)
		{
			value = VariantUtils.CreateFrom<Variant.Type>(Type);
			return true;
		}
		if (name == PropertyName.ClassName)
		{
			from = ClassName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DefaultValue)
		{
			value = VariantUtils.CreateFrom<Variant>(DefaultValue);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.Owner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.Id, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.Type, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ClassName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.DefaultValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Owner, Variant.From<XWBPScriptData>(Owner));
		info.AddProperty(PropertyName.Id, Variant.From<int>(Id));
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
		info.AddProperty(PropertyName.Type, Variant.From<Variant.Type>(Type));
		info.AddProperty(PropertyName.ClassName, Variant.From<string>(ClassName));
		info.AddProperty(PropertyName.DefaultValue, Variant.From<Variant>(DefaultValue));
		info.AddSignalEventDelegate(SignalName.Change, backing_Change);
		info.AddSignalEventDelegate(SignalName.Remove, backing_Remove);
		info.AddSignalEventDelegate(SignalName.Rename, backing_Rename);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Owner, out var value))
		{
			Owner = value.As<XWBPScriptData>();
		}
		if (info.TryGetProperty(PropertyName.Id, out var value2))
		{
			Id = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Name, out var value3))
		{
			Name = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Type, out var value4))
		{
			Type = value4.As<Variant.Type>();
		}
		if (info.TryGetProperty(PropertyName.ClassName, out var value5))
		{
			ClassName = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.DefaultValue, out var value6))
		{
			DefaultValue = value6.As<Variant>();
		}
		if (info.TryGetSignalEventDelegate<ChangeEventHandler>(SignalName.Change, out var value7))
		{
			backing_Change = value7;
		}
		if (info.TryGetSignalEventDelegate<RemoveEventHandler>(SignalName.Remove, out var value8))
		{
			backing_Remove = value8;
		}
		if (info.TryGetSignalEventDelegate<RenameEventHandler>(SignalName.Rename, out var value9))
		{
			backing_Rename = value9;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(SignalName.Change, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.Remove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.Rename, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalChange()
	{
		EmitSignal(SignalName.Change, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalRemove()
	{
		EmitSignal(SignalName.Remove, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalRename(string name)
	{
		EmitSignal(SignalName.Rename, new ReadOnlySpan<Variant>((Variant)name));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.Change && args.Count == 0)
		{
			backing_Change?.Invoke();
		}
		else if (signal == SignalName.Remove && args.Count == 0)
		{
			backing_Remove?.Invoke();
		}
		else if (signal == SignalName.Rename && args.Count == 1)
		{
			backing_Rename?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.Change)
		{
			return true;
		}
		if (signal == SignalName.Remove)
		{
			return true;
		}
		if (signal == SignalName.Rename)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
