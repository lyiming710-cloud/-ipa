using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/RecycleComponent/RecycleComponentDefinition.cs")]
public class RecycleComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName defaultPercentage = "defaultPercentage";

		public static readonly StringName horizontalVelocityMin = "horizontalVelocityMin";

		public static readonly StringName horizontalVelocityMax = "horizontalVelocityMax";

		public static readonly StringName verticalVelocity = "verticalVelocity";

		public static readonly StringName sunGravity = "sunGravity";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Range, "0,10,0.05")]
	public float defaultPercentage = 0.2f;

	[Export(PropertyHint.None, "")]
	public float horizontalVelocityMin = -50f;

	[Export(PropertyHint.None, "")]
	public float horizontalVelocityMax = 50f;

	[Export(PropertyHint.None, "")]
	public float verticalVelocity = -400f;

	[Export(PropertyHint.None, "")]
	public float sunGravity = 980f;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new RecycleComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.defaultPercentage)
		{
			defaultPercentage = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.horizontalVelocityMin)
		{
			horizontalVelocityMin = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.horizontalVelocityMax)
		{
			horizontalVelocityMax = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.verticalVelocity)
		{
			verticalVelocity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.sunGravity)
		{
			sunGravity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.defaultPercentage)
		{
			value = VariantUtils.CreateFrom(in defaultPercentage);
			return true;
		}
		if (name == PropertyName.horizontalVelocityMin)
		{
			value = VariantUtils.CreateFrom(in horizontalVelocityMin);
			return true;
		}
		if (name == PropertyName.horizontalVelocityMax)
		{
			value = VariantUtils.CreateFrom(in horizontalVelocityMax);
			return true;
		}
		if (name == PropertyName.verticalVelocity)
		{
			value = VariantUtils.CreateFrom(in verticalVelocity);
			return true;
		}
		if (name == PropertyName.sunGravity)
		{
			value = VariantUtils.CreateFrom(in sunGravity);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.defaultPercentage, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.horizontalVelocityMin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.horizontalVelocityMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.verticalVelocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.sunGravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.defaultPercentage, Variant.From(in defaultPercentage));
		info.AddProperty(PropertyName.horizontalVelocityMin, Variant.From(in horizontalVelocityMin));
		info.AddProperty(PropertyName.horizontalVelocityMax, Variant.From(in horizontalVelocityMax));
		info.AddProperty(PropertyName.verticalVelocity, Variant.From(in verticalVelocity));
		info.AddProperty(PropertyName.sunGravity, Variant.From(in sunGravity));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.defaultPercentage, out var value))
		{
			defaultPercentage = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName.horizontalVelocityMin, out var value2))
		{
			horizontalVelocityMin = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.horizontalVelocityMax, out var value3))
		{
			horizontalVelocityMax = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.verticalVelocity, out var value4))
		{
			verticalVelocity = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.sunGravity, out var value5))
		{
			sunGravity = value5.As<float>();
		}
	}
}
