using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class StateMachineResourceRuntimeExtensionState : StateMachineStateDefinition
{
	public new class MethodName : StateMachineStateDefinition.MethodName
	{
	}

	public new class PropertyName : StateMachineStateDefinition.PropertyName
	{
		public static readonly StringName ProbeStateValue = "ProbeStateValue";
	}

	public new class SignalName : StateMachineStateDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int ProbeStateValue { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ProbeStateValue)
		{
			ProbeStateValue = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ProbeStateValue)
		{
			value = VariantUtils.CreateFrom<int>(ProbeStateValue);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.ProbeStateValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ProbeStateValue, Variant.From<int>(ProbeStateValue));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ProbeStateValue, out var value))
		{
			ProbeStateValue = value.As<int>();
		}
	}
}
