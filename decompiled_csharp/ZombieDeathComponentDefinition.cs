using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ZombieDeathComponent/ZombieDeathComponentDefinition.cs")]
public class ZombieDeathComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName dropFeatureName = "dropFeatureName";

		public static readonly StringName dropVelocityMinX = "dropVelocityMinX";

		public static readonly StringName dropVelocityMaxX = "dropVelocityMaxX";

		public static readonly StringName dropVelocityY = "dropVelocityY";

		public static readonly StringName dropGravity = "dropGravity";

		public static readonly StringName fadeDuration = "fadeDuration";

		public static readonly StringName waterSinkDuration = "waterSinkDuration";

		public static readonly StringName waterSinkGroundHeight = "waterSinkGroundHeight";

		public static readonly StringName animationBlend = "animationBlend";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string dropFeatureName = "Coins";

	[Export(PropertyHint.None, "")]
	public float dropVelocityMinX = -50f;

	[Export(PropertyHint.None, "")]
	public float dropVelocityMaxX = 50f;

	[Export(PropertyHint.None, "")]
	public float dropVelocityY = -400f;

	[Export(PropertyHint.None, "")]
	public float dropGravity = 980f;

	[Export(PropertyHint.Range, "0.01,5,0.01")]
	public double fadeDuration = 0.5;

	[Export(PropertyHint.Range, "0.01,5,0.01")]
	public double waterSinkDuration = 1.0;

	[Export(PropertyHint.None, "")]
	public double waterSinkGroundHeight = -100.0;

	[Export(PropertyHint.Range, "0,1,0.01")]
	public double animationBlend = 0.2;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ZombieDeathComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dropFeatureName)
		{
			dropFeatureName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.dropVelocityMinX)
		{
			dropVelocityMinX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.dropVelocityMaxX)
		{
			dropVelocityMaxX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.dropVelocityY)
		{
			dropVelocityY = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.dropGravity)
		{
			dropGravity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.fadeDuration)
		{
			fadeDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.waterSinkDuration)
		{
			waterSinkDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.waterSinkGroundHeight)
		{
			waterSinkGroundHeight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.animationBlend)
		{
			animationBlend = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.dropFeatureName)
		{
			value = VariantUtils.CreateFrom(in dropFeatureName);
			return true;
		}
		if (name == PropertyName.dropVelocityMinX)
		{
			value = VariantUtils.CreateFrom(in dropVelocityMinX);
			return true;
		}
		if (name == PropertyName.dropVelocityMaxX)
		{
			value = VariantUtils.CreateFrom(in dropVelocityMaxX);
			return true;
		}
		if (name == PropertyName.dropVelocityY)
		{
			value = VariantUtils.CreateFrom(in dropVelocityY);
			return true;
		}
		if (name == PropertyName.dropGravity)
		{
			value = VariantUtils.CreateFrom(in dropGravity);
			return true;
		}
		if (name == PropertyName.fadeDuration)
		{
			value = VariantUtils.CreateFrom(in fadeDuration);
			return true;
		}
		if (name == PropertyName.waterSinkDuration)
		{
			value = VariantUtils.CreateFrom(in waterSinkDuration);
			return true;
		}
		if (name == PropertyName.waterSinkGroundHeight)
		{
			value = VariantUtils.CreateFrom(in waterSinkGroundHeight);
			return true;
		}
		if (name == PropertyName.animationBlend)
		{
			value = VariantUtils.CreateFrom(in animationBlend);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.dropFeatureName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dropVelocityMinX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dropVelocityMaxX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dropVelocityY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dropGravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fadeDuration, PropertyHint.Range, "0.01,5,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.waterSinkDuration, PropertyHint.Range, "0.01,5,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.waterSinkGroundHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.animationBlend, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dropFeatureName, Variant.From(in dropFeatureName));
		info.AddProperty(PropertyName.dropVelocityMinX, Variant.From(in dropVelocityMinX));
		info.AddProperty(PropertyName.dropVelocityMaxX, Variant.From(in dropVelocityMaxX));
		info.AddProperty(PropertyName.dropVelocityY, Variant.From(in dropVelocityY));
		info.AddProperty(PropertyName.dropGravity, Variant.From(in dropGravity));
		info.AddProperty(PropertyName.fadeDuration, Variant.From(in fadeDuration));
		info.AddProperty(PropertyName.waterSinkDuration, Variant.From(in waterSinkDuration));
		info.AddProperty(PropertyName.waterSinkGroundHeight, Variant.From(in waterSinkGroundHeight));
		info.AddProperty(PropertyName.animationBlend, Variant.From(in animationBlend));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dropFeatureName, out var value))
		{
			dropFeatureName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.dropVelocityMinX, out var value2))
		{
			dropVelocityMinX = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.dropVelocityMaxX, out var value3))
		{
			dropVelocityMaxX = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.dropVelocityY, out var value4))
		{
			dropVelocityY = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.dropGravity, out var value5))
		{
			dropGravity = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.fadeDuration, out var value6))
		{
			fadeDuration = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.waterSinkDuration, out var value7))
		{
			waterSinkDuration = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.waterSinkGroundHeight, out var value8))
		{
			waterSinkGroundHeight = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.animationBlend, out var value9))
		{
			animationBlend = value9.As<double>();
		}
	}
}
