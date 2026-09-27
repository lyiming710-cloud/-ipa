using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/PhonkComponent/PhonkComponentDefinition.cs")]
public class PhonkComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName stiffness = "stiffness";

		public static readonly StringName damping = "damping";

		public static readonly StringName rotStiffness = "rotStiffness";

		public static readonly StringName rotDamping = "rotDamping";

		public static readonly StringName maxScaleDeform = "maxScaleDeform";

		public static readonly StringName maxRotDeform = "maxRotDeform";

		public static readonly StringName hurtIntensity = "hurtIntensity";

		public static readonly StringName dieIntensity = "dieIntensity";

		public static readonly StringName attackIntensity = "attackIntensity";

		public static readonly StringName produceIntensity = "produceIntensity";

		public static readonly StringName plantIntensity = "plantIntensity";

		public static readonly StringName hurtCooldown = "hurtCooldown";

		public static readonly StringName maxSimulationStep = "maxSimulationStep";

		public static readonly StringName maxSimulationSubsteps = "maxSimulationSubsteps";

		public static readonly StringName transformResponse = "transformResponse";

		public static readonly StringName scaleSleepThreshold = "scaleSleepThreshold";

		public static readonly StringName velocitySleepThreshold = "velocitySleepThreshold";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Spring", "")]
	[Export(PropertyHint.Range, "0,5000,1")]
	public float stiffness { get; set; } = 800f;

	[Export(PropertyHint.Range, "0,100,0.1")]
	public float damping { get; set; } = 3.5f;

	[Export(PropertyHint.Range, "0,5000,1")]
	public float rotStiffness { get; set; } = 1400f;

	[Export(PropertyHint.Range, "0,100,0.1")]
	public float rotDamping { get; set; } = 5f;

	[Export(PropertyHint.Range, "0,2,0.01")]
	public float maxScaleDeform { get; set; } = 0.6f;

	[Export(PropertyHint.Range, "0,2,0.01")]
	public float maxRotDeform { get; set; } = 0.5f;

	[ExportGroup("Impulses", "")]
	[Export(PropertyHint.None, "")]
	public float hurtIntensity { get; set; } = 1.2f;

	[Export(PropertyHint.None, "")]
	public float dieIntensity { get; set; } = 1.8f;

	[Export(PropertyHint.None, "")]
	public float attackIntensity { get; set; } = 0.6f;

	[Export(PropertyHint.None, "")]
	public float produceIntensity { get; set; } = 0.5f;

	[Export(PropertyHint.None, "")]
	public float plantIntensity { get; set; } = 0.8f;

	[Export(PropertyHint.Range, "0,5,0.01")]
	public float hurtCooldown { get; set; } = 0.5f;

	[ExportGroup("Simulation", "")]
	[Export(PropertyHint.Range, "0.001,0.1,0.001")]
	public float maxSimulationStep { get; set; } = 1f / 60f;

	[Export(PropertyHint.Range, "1,16,1")]
	public int maxSimulationSubsteps { get; set; } = 4;

	[Export(PropertyHint.Range, "0,100,0.1")]
	public float transformResponse { get; set; } = 25f;

	[Export(PropertyHint.None, "")]
	public float scaleSleepThreshold { get; set; } = 0.001f;

	[Export(PropertyHint.None, "")]
	public float velocitySleepThreshold { get; set; } = 0.01f;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new PhonkComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.stiffness)
		{
			stiffness = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.damping)
		{
			damping = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.rotStiffness)
		{
			rotStiffness = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.rotDamping)
		{
			rotDamping = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.maxScaleDeform)
		{
			maxScaleDeform = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.maxRotDeform)
		{
			maxRotDeform = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.hurtIntensity)
		{
			hurtIntensity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.dieIntensity)
		{
			dieIntensity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.attackIntensity)
		{
			attackIntensity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.produceIntensity)
		{
			produceIntensity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.plantIntensity)
		{
			plantIntensity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.hurtCooldown)
		{
			hurtCooldown = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.maxSimulationStep)
		{
			maxSimulationStep = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.maxSimulationSubsteps)
		{
			maxSimulationSubsteps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.transformResponse)
		{
			transformResponse = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.scaleSleepThreshold)
		{
			scaleSleepThreshold = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.velocitySleepThreshold)
		{
			velocitySleepThreshold = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		float from;
		if (name == PropertyName.stiffness)
		{
			from = stiffness;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.damping)
		{
			from = damping;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.rotStiffness)
		{
			from = rotStiffness;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.rotDamping)
		{
			from = rotDamping;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.maxScaleDeform)
		{
			from = maxScaleDeform;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.maxRotDeform)
		{
			from = maxRotDeform;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.hurtIntensity)
		{
			from = hurtIntensity;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dieIntensity)
		{
			from = dieIntensity;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.attackIntensity)
		{
			from = attackIntensity;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.produceIntensity)
		{
			from = produceIntensity;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.plantIntensity)
		{
			from = plantIntensity;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.hurtCooldown)
		{
			from = hurtCooldown;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.maxSimulationStep)
		{
			from = maxSimulationStep;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.maxSimulationSubsteps)
		{
			value = VariantUtils.CreateFrom<int>(maxSimulationSubsteps);
			return true;
		}
		if (name == PropertyName.transformResponse)
		{
			from = transformResponse;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.scaleSleepThreshold)
		{
			from = scaleSleepThreshold;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.velocitySleepThreshold)
		{
			from = velocitySleepThreshold;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Spring", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.stiffness, PropertyHint.Range, "0,5000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.damping, PropertyHint.Range, "0,100,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rotStiffness, PropertyHint.Range, "0,5000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rotDamping, PropertyHint.Range, "0,100,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.maxScaleDeform, PropertyHint.Range, "0,2,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.maxRotDeform, PropertyHint.Range, "0,2,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Impulses", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hurtIntensity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dieIntensity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackIntensity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.produceIntensity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.plantIntensity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hurtCooldown, PropertyHint.Range, "0,5,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Simulation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.maxSimulationStep, PropertyHint.Range, "0.001,0.1,0.001", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxSimulationSubsteps, PropertyHint.Range, "1,16,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.transformResponse, PropertyHint.Range, "0,100,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.scaleSleepThreshold, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.velocitySleepThreshold, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.stiffness, Variant.From<float>(stiffness));
		info.AddProperty(PropertyName.damping, Variant.From<float>(damping));
		info.AddProperty(PropertyName.rotStiffness, Variant.From<float>(rotStiffness));
		info.AddProperty(PropertyName.rotDamping, Variant.From<float>(rotDamping));
		info.AddProperty(PropertyName.maxScaleDeform, Variant.From<float>(maxScaleDeform));
		info.AddProperty(PropertyName.maxRotDeform, Variant.From<float>(maxRotDeform));
		info.AddProperty(PropertyName.hurtIntensity, Variant.From<float>(hurtIntensity));
		info.AddProperty(PropertyName.dieIntensity, Variant.From<float>(dieIntensity));
		info.AddProperty(PropertyName.attackIntensity, Variant.From<float>(attackIntensity));
		info.AddProperty(PropertyName.produceIntensity, Variant.From<float>(produceIntensity));
		info.AddProperty(PropertyName.plantIntensity, Variant.From<float>(plantIntensity));
		info.AddProperty(PropertyName.hurtCooldown, Variant.From<float>(hurtCooldown));
		info.AddProperty(PropertyName.maxSimulationStep, Variant.From<float>(maxSimulationStep));
		info.AddProperty(PropertyName.maxSimulationSubsteps, Variant.From<int>(maxSimulationSubsteps));
		info.AddProperty(PropertyName.transformResponse, Variant.From<float>(transformResponse));
		info.AddProperty(PropertyName.scaleSleepThreshold, Variant.From<float>(scaleSleepThreshold));
		info.AddProperty(PropertyName.velocitySleepThreshold, Variant.From<float>(velocitySleepThreshold));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.stiffness, out var value))
		{
			stiffness = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName.damping, out var value2))
		{
			damping = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.rotStiffness, out var value3))
		{
			rotStiffness = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.rotDamping, out var value4))
		{
			rotDamping = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.maxScaleDeform, out var value5))
		{
			maxScaleDeform = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.maxRotDeform, out var value6))
		{
			maxRotDeform = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.hurtIntensity, out var value7))
		{
			hurtIntensity = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.dieIntensity, out var value8))
		{
			dieIntensity = value8.As<float>();
		}
		if (info.TryGetProperty(PropertyName.attackIntensity, out var value9))
		{
			attackIntensity = value9.As<float>();
		}
		if (info.TryGetProperty(PropertyName.produceIntensity, out var value10))
		{
			produceIntensity = value10.As<float>();
		}
		if (info.TryGetProperty(PropertyName.plantIntensity, out var value11))
		{
			plantIntensity = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName.hurtCooldown, out var value12))
		{
			hurtCooldown = value12.As<float>();
		}
		if (info.TryGetProperty(PropertyName.maxSimulationStep, out var value13))
		{
			maxSimulationStep = value13.As<float>();
		}
		if (info.TryGetProperty(PropertyName.maxSimulationSubsteps, out var value14))
		{
			maxSimulationSubsteps = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName.transformResponse, out var value15))
		{
			transformResponse = value15.As<float>();
		}
		if (info.TryGetProperty(PropertyName.scaleSleepThreshold, out var value16))
		{
			scaleSleepThreshold = value16.As<float>();
		}
		if (info.TryGetProperty(PropertyName.velocitySleepThreshold, out var value17))
		{
			velocitySleepThreshold = value17.As<float>();
		}
	}
}
