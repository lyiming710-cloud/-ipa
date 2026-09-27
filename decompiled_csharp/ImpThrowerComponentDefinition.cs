using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ImpThrowerComponent/ImpThrowerComponentDefinition.cs")]
public class ImpThrowerComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName impVerticalSpeed = "impVerticalSpeed";

		public static readonly StringName landingGridMin = "landingGridMin";

		public static readonly StringName landingGridMax = "landingGridMax";

		public static readonly StringName throwEase = "throwEase";

		public static readonly StringName throwTransition = "throwTransition";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double impVerticalSpeed { get; set; } = -60.0;

	[Export(PropertyHint.None, "")]
	public int landingGridMin { get; set; } = 3;

	[Export(PropertyHint.None, "")]
	public int landingGridMax { get; set; } = 5;

	[Export(PropertyHint.None, "")]
	public Tween.EaseType throwEase { get; set; } = Tween.EaseType.Out;

	[Export(PropertyHint.None, "")]
	public Tween.TransitionType throwTransition { get; set; } = Tween.TransitionType.Quad;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ImpThrowerComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.impVerticalSpeed)
		{
			impVerticalSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.landingGridMin)
		{
			landingGridMin = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.landingGridMax)
		{
			landingGridMax = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.throwEase)
		{
			throwEase = VariantUtils.ConvertTo<Tween.EaseType>(in value);
			return true;
		}
		if (name == PropertyName.throwTransition)
		{
			throwTransition = VariantUtils.ConvertTo<Tween.TransitionType>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.impVerticalSpeed)
		{
			value = VariantUtils.CreateFrom<double>(impVerticalSpeed);
			return true;
		}
		int from;
		if (name == PropertyName.landingGridMin)
		{
			from = landingGridMin;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.landingGridMax)
		{
			from = landingGridMax;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.throwEase)
		{
			value = VariantUtils.CreateFrom<Tween.EaseType>(throwEase);
			return true;
		}
		if (name == PropertyName.throwTransition)
		{
			value = VariantUtils.CreateFrom<Tween.TransitionType>(throwTransition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.impVerticalSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.landingGridMin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.landingGridMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.throwEase, PropertyHint.Enum, "In,Out,InOut,OutIn", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.throwTransition, PropertyHint.Enum, "Linear,Sine,Quint,Quart,Quad,Expo,Elastic,Cubic,Circ,Bounce,Back,Spring", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.impVerticalSpeed, Variant.From<double>(impVerticalSpeed));
		info.AddProperty(PropertyName.landingGridMin, Variant.From<int>(landingGridMin));
		info.AddProperty(PropertyName.landingGridMax, Variant.From<int>(landingGridMax));
		info.AddProperty(PropertyName.throwEase, Variant.From<Tween.EaseType>(throwEase));
		info.AddProperty(PropertyName.throwTransition, Variant.From<Tween.TransitionType>(throwTransition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.impVerticalSpeed, out var value))
		{
			impVerticalSpeed = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.landingGridMin, out var value2))
		{
			landingGridMin = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.landingGridMax, out var value3))
		{
			landingGridMax = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.throwEase, out var value4))
		{
			throwEase = value4.As<Tween.EaseType>();
		}
		if (info.TryGetProperty(PropertyName.throwTransition, out var value5))
		{
			throwTransition = value5.As<Tween.TransitionType>();
		}
	}
}
