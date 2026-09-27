using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/Signal/XWBPSignalData.cs")]
public class XWBPSignalData : RefCounted
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

		public static readonly StringName AddInput = "AddInput";

		public static readonly StringName RemoveInput = "RemoveInput";

		public static readonly StringName GetInputCount = "GetInputCount";

		public static readonly StringName InputsSet = "InputsSet";

		public static readonly StringName EnsureUniquePortNames = "EnsureUniquePortNames";

		public static readonly StringName Duplicate = "Duplicate";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Owner = "Owner";

		public static readonly StringName Id = "Id";

		public static readonly StringName Name = "Name";
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

	public string Name { get; set; } = "新信号";

	public List<XWBPNodePortData> Inputs { get; set; } = new List<XWBPNodePortData>();

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

	public void AddInput(string inputName, int inputType, StringName inputClassName = null)
	{
		XWBPNodePortData item = new XWBPNodePortData(inputName, XWBPNodePortData.Direction.Input, (XWBPNodePortData.PortType)inputType, inputClassName, default);
		Inputs.Add(item);
		EnsureUniquePortNames();
		EmitSignal(SignalName.Change);
	}

	public void RemoveInput(int index)
	{
		if (index >= 0 && index < Inputs.Count)
		{
			Inputs.RemoveAt(index);
			EmitSignal(SignalName.Change);
		}
	}

	public int GetInputCount()
	{
		return Inputs.Count;
	}

	public List<XWInspectorProperty> GetProperties()
	{
		return new List<XWInspectorProperty>
		{
			new XWInspectorProperty(this, "inputs", default, "信号", "", PropertyHint.None, "", Callable.From(InputsSet))
		};
	}

	public void InputsSet()
	{
		foreach (XWBPNodePortData input in Inputs)
		{
			input.PortDirection = XWBPNodePortData.Direction.Input;
		}
		EnsureUniquePortNames();
		EmitSignal(SignalName.Change);
	}

	public void EnsureUniquePortNames()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < Inputs.Count; i++)
		{
			XWBPNodePortData xWBPNodePortData = Inputs[i];
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

	public XWBPSignalData Duplicate()
	{
		XWBPSignalData xWBPSignalData = new XWBPSignalData
		{
			Name = Name
		};
		foreach (XWBPNodePortData input in Inputs)
		{
			XWBPNodePortData item = new XWBPNodePortData(input.Name, input.PortDirection, input.PortTypeValue, input.ClassName, input.DefaultValue);
			xWBPSignalData.Inputs.Add(item);
		}
		return xWBPSignalData;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.RemoveSelf, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenameSelf, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "inputName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "inputType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "inputClassName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetInputCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InputsSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureUniquePortNames, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.AddInput && args.Count == 3)
		{
			AddInput(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveInput && args.Count == 1)
		{
			RemoveInput(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetInputCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetInputCount());
			return true;
		}
		if (method == MethodName.InputsSet && args.Count == 0)
		{
			InputsSet();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureUniquePortNames && args.Count == 0)
		{
			EnsureUniquePortNames();
			ret = default;
			return true;
		}
		if (method == MethodName.Duplicate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPSignalData>(Duplicate());
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
		if (method == MethodName.AddInput)
		{
			return true;
		}
		if (method == MethodName.RemoveInput)
		{
			return true;
		}
		if (method == MethodName.GetInputCount)
		{
			return true;
		}
		if (method == MethodName.InputsSet)
		{
			return true;
		}
		if (method == MethodName.EnsureUniquePortNames)
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
		if (name == PropertyName.Name)
		{
			value = VariantUtils.CreateFrom<string>(Name);
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
			new PropertyInfo(Variant.Type.String, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Owner, Variant.From<XWBPScriptData>(Owner));
		info.AddProperty(PropertyName.Id, Variant.From<int>(Id));
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
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
		if (info.TryGetSignalEventDelegate<ChangeEventHandler>(SignalName.Change, out var value4))
		{
			backing_Change = value4;
		}
		if (info.TryGetSignalEventDelegate<RemoveEventHandler>(SignalName.Remove, out var value5))
		{
			backing_Remove = value5;
		}
		if (info.TryGetSignalEventDelegate<RenameEventHandler>(SignalName.Rename, out var value6))
		{
			backing_Rename = value6;
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
