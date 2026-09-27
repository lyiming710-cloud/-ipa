using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/GargantuarSmashComponent/GargantuarSmashComponentDefinition.cs")]
public class GargantuarSmashComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName smashAudio = "smashAudio";

		public static readonly StringName cameraShakeDuration = "cameraShakeDuration";

		public static readonly StringName cameraShakeInterval = "cameraShakeInterval";

		public static readonly StringName cameraShakeCount = "cameraShakeCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string smashAudio = "GargantuarThump";

	[Export(PropertyHint.Range, "0,10,0.05")]
	public double cameraShakeDuration = 2.0;

	[Export(PropertyHint.Range, "0.001,1,0.001")]
	public double cameraShakeInterval = 0.05;

	[Export(PropertyHint.Range, "0,32,1")]
	public int cameraShakeCount = 4;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new GargantuarSmashComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.smashAudio)
		{
			smashAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeDuration)
		{
			cameraShakeDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeInterval)
		{
			cameraShakeInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeCount)
		{
			cameraShakeCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.smashAudio)
		{
			value = VariantUtils.CreateFrom(in smashAudio);
			return true;
		}
		if (name == PropertyName.cameraShakeDuration)
		{
			value = VariantUtils.CreateFrom(in cameraShakeDuration);
			return true;
		}
		if (name == PropertyName.cameraShakeInterval)
		{
			value = VariantUtils.CreateFrom(in cameraShakeInterval);
			return true;
		}
		if (name == PropertyName.cameraShakeCount)
		{
			value = VariantUtils.CreateFrom(in cameraShakeCount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.smashAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraShakeDuration, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraShakeInterval, PropertyHint.Range, "0.001,1,0.001", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.cameraShakeCount, PropertyHint.Range, "0,32,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.smashAudio, Variant.From(in smashAudio));
		info.AddProperty(PropertyName.cameraShakeDuration, Variant.From(in cameraShakeDuration));
		info.AddProperty(PropertyName.cameraShakeInterval, Variant.From(in cameraShakeInterval));
		info.AddProperty(PropertyName.cameraShakeCount, Variant.From(in cameraShakeCount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.smashAudio, out var value))
		{
			smashAudio = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeDuration, out var value2))
		{
			cameraShakeDuration = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeInterval, out var value3))
		{
			cameraShakeInterval = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeCount, out var value4))
		{
			cameraShakeCount = value4.As<int>();
		}
	}
}
