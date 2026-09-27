using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/EnvironmentAnimeComponent/EnvironmentAnimeComponentDefinition.cs")]
public class EnvironmentAnimeComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName dayAnimation = "dayAnimation";

		public static readonly StringName nightAnimation = "nightAnimation";

		public static readonly StringName waterDayAnimation = "waterDayAnimation";

		public static readonly StringName waterNightAnimation = "waterNightAnimation";

		public static readonly StringName bobFrequency = "bobFrequency";

		public static readonly StringName bobAmplitude = "bobAmplitude";

		public static readonly StringName bobBaseY = "bobBaseY";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StringName dayAnimation = "Day";

	[Export(PropertyHint.None, "")]
	public StringName nightAnimation = "Night";

	[Export(PropertyHint.None, "")]
	public StringName waterDayAnimation = "Water";

	[Export(PropertyHint.None, "")]
	public StringName waterNightAnimation = "WaterNight";

	[Export(PropertyHint.Range, "0,20,0.1")]
	public float bobFrequency = 2f;

	[Export(PropertyHint.Range, "0,50,0.1")]
	public float bobAmplitude = 2f;

	[Export(PropertyHint.None, "")]
	public float bobBaseY;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new EnvironmentAnimeComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dayAnimation)
		{
			dayAnimation = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.nightAnimation)
		{
			nightAnimation = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.waterDayAnimation)
		{
			waterDayAnimation = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.waterNightAnimation)
		{
			waterNightAnimation = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.bobFrequency)
		{
			bobFrequency = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.bobAmplitude)
		{
			bobAmplitude = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.bobBaseY)
		{
			bobBaseY = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.dayAnimation)
		{
			value = VariantUtils.CreateFrom(in dayAnimation);
			return true;
		}
		if (name == PropertyName.nightAnimation)
		{
			value = VariantUtils.CreateFrom(in nightAnimation);
			return true;
		}
		if (name == PropertyName.waterDayAnimation)
		{
			value = VariantUtils.CreateFrom(in waterDayAnimation);
			return true;
		}
		if (name == PropertyName.waterNightAnimation)
		{
			value = VariantUtils.CreateFrom(in waterNightAnimation);
			return true;
		}
		if (name == PropertyName.bobFrequency)
		{
			value = VariantUtils.CreateFrom(in bobFrequency);
			return true;
		}
		if (name == PropertyName.bobAmplitude)
		{
			value = VariantUtils.CreateFrom(in bobAmplitude);
			return true;
		}
		if (name == PropertyName.bobBaseY)
		{
			value = VariantUtils.CreateFrom(in bobBaseY);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.dayAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.nightAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.waterDayAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.waterNightAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.bobFrequency, PropertyHint.Range, "0,20,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.bobAmplitude, PropertyHint.Range, "0,50,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.bobBaseY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dayAnimation, Variant.From(in dayAnimation));
		info.AddProperty(PropertyName.nightAnimation, Variant.From(in nightAnimation));
		info.AddProperty(PropertyName.waterDayAnimation, Variant.From(in waterDayAnimation));
		info.AddProperty(PropertyName.waterNightAnimation, Variant.From(in waterNightAnimation));
		info.AddProperty(PropertyName.bobFrequency, Variant.From(in bobFrequency));
		info.AddProperty(PropertyName.bobAmplitude, Variant.From(in bobAmplitude));
		info.AddProperty(PropertyName.bobBaseY, Variant.From(in bobBaseY));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dayAnimation, out var value))
		{
			dayAnimation = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.nightAnimation, out var value2))
		{
			nightAnimation = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.waterDayAnimation, out var value3))
		{
			waterDayAnimation = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.waterNightAnimation, out var value4))
		{
			waterNightAnimation = value4.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.bobFrequency, out var value5))
		{
			bobFrequency = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.bobAmplitude, out var value6))
		{
			bobAmplitude = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.bobBaseY, out var value7))
		{
			bobBaseY = value7.As<float>();
		}
	}
}
