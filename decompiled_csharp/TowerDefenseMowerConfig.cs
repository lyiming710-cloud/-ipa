using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Config/TowerDefenseMowerConfig.cs")]
public class TowerDefenseMowerConfig : TowerDefenseCharacterConfig
{
	public new class MethodName : TowerDefenseCharacterConfig.MethodName
	{
	}

	public new class PropertyName : TowerDefenseCharacterConfig.PropertyName
	{
		public static readonly StringName mowerConfig = "mowerConfig";
	}

	public new class SignalName : TowerDefenseCharacterConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public MowerConfig mowerConfig;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.mowerConfig)
		{
			mowerConfig = VariantUtils.ConvertTo<MowerConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mowerConfig)
		{
			value = VariantUtils.CreateFrom(in mowerConfig);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.mowerConfig, PropertyHint.ResourceType, "MowerConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mowerConfig, Variant.From(in mowerConfig));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mowerConfig, out var value))
		{
			mowerConfig = value.As<MowerConfig>();
		}
	}
}
