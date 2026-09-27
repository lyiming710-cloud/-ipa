using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class StateMachineProbeDerivedTransition : StateMachineTransitionDefinition
{
	public new class MethodName : StateMachineTransitionDefinition.MethodName
	{
	}

	public new class PropertyName : StateMachineTransitionDefinition.PropertyName
	{
		public static readonly StringName ModWeight = "ModWeight";
	}

	public new class SignalName : StateMachineTransitionDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int ModWeight { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ModWeight)
		{
			ModWeight = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ModWeight)
		{
			value = VariantUtils.CreateFrom<int>(ModWeight);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.ModWeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ModWeight, Variant.From<int>(ModWeight));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ModWeight, out var value))
		{
			ModWeight = value.As<int>();
		}
	}
}
