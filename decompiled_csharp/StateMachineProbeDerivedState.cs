using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class StateMachineProbeDerivedState : StateMachineStateDefinition
{
	public new class MethodName : StateMachineStateDefinition.MethodName
	{
	}

	public new class PropertyName : StateMachineStateDefinition.PropertyName
	{
		public static readonly StringName ModPayload = "ModPayload";
	}

	public new class SignalName : StateMachineStateDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string ModPayload { get; set; } = string.Empty;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ModPayload)
		{
			ModPayload = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ModPayload)
		{
			value = VariantUtils.CreateFrom<string>(ModPayload);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ModPayload, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ModPayload, Variant.From<string>(ModPayload));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ModPayload, out var value))
		{
			ModPayload = value.As<string>();
		}
	}
}
