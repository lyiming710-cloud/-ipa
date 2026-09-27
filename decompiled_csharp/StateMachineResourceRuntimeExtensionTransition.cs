using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class StateMachineResourceRuntimeExtensionTransition : StateMachineTransitionDefinition
{
	public new class MethodName : StateMachineTransitionDefinition.MethodName
	{
	}

	public new class PropertyName : StateMachineTransitionDefinition.PropertyName
	{
		public static readonly StringName ProbeTransitionValue = "ProbeTransitionValue";
	}

	public new class SignalName : StateMachineTransitionDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string ProbeTransitionValue { get; set; } = string.Empty;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ProbeTransitionValue)
		{
			ProbeTransitionValue = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ProbeTransitionValue)
		{
			value = VariantUtils.CreateFrom<string>(ProbeTransitionValue);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ProbeTransitionValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ProbeTransitionValue, Variant.From<string>(ProbeTransitionValue));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ProbeTransitionValue, out var value))
		{
			ProbeTransitionValue = value.As<string>();
		}
	}
}
