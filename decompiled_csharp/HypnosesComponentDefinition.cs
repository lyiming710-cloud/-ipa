using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/HypnosesComponent/HypnosesComponentDefinition.cs")]
public class HypnosesComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName playHypnosesAudio = "playHypnosesAudio";

		public static readonly StringName hypnosesAudio = "hypnosesAudio";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool playHypnosesAudio = true;

	[Export(PropertyHint.None, "")]
	public string hypnosesAudio = "Floop";

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new HypnosesComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.playHypnosesAudio)
		{
			playHypnosesAudio = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hypnosesAudio)
		{
			hypnosesAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.playHypnosesAudio)
		{
			value = VariantUtils.CreateFrom(in playHypnosesAudio);
			return true;
		}
		if (name == PropertyName.hypnosesAudio)
		{
			value = VariantUtils.CreateFrom(in hypnosesAudio);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.playHypnosesAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.hypnosesAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.playHypnosesAudio, Variant.From(in playHypnosesAudio));
		info.AddProperty(PropertyName.hypnosesAudio, Variant.From(in hypnosesAudio));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.playHypnosesAudio, out var value))
		{
			playHypnosesAudio = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hypnosesAudio, out var value2))
		{
			hypnosesAudio = value2.As<string>();
		}
	}
}
