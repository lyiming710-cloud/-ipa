using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Test/ModEditorStateMachineProbeDerivedDefinition.cs")]
public class ModEditorStateMachineProbeDerivedDefinition : StateMachineDefinition
{
	public new class MethodName : StateMachineDefinition.MethodName
	{
	}

	public new class PropertyName : StateMachineDefinition.PropertyName
	{
		public static readonly StringName ModAuthorNote = "ModAuthorNote";
	}

	public new class SignalName : StateMachineDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string ModAuthorNote { get; set; } = "derived-before";

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ModAuthorNote)
		{
			ModAuthorNote = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ModAuthorNote)
		{
			value = VariantUtils.CreateFrom<string>(ModAuthorNote);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ModAuthorNote, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ModAuthorNote, Variant.From<string>(ModAuthorNote));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ModAuthorNote, out var value))
		{
			ModAuthorNote = value.As<string>();
		}
	}
}
