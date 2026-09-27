using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ImpFlightComponent/ImpFlightComponentDefinition.cs")]
public class ImpFlightComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName flyAudio = "flyAudio";

		public static readonly StringName flyAnimationSpeedScale = "flyAnimationSpeedScale";

		public static readonly StringName animationBlend = "animationBlend";

		public static readonly StringName flyEvent = "flyEvent";

		public static readonly StringName landEvent = "landEvent";

		public static readonly StringName idleEvent = "idleEvent";

		public static readonly StringName flyStateName = "flyStateName";

		public static readonly StringName suppressCollisionInFlight = "suppressCollisionInFlight";

		public static readonly StringName restoreCollisionOnDisable = "restoreCollisionOnDisable";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string flyAudio = "Imp";

	[Export(PropertyHint.Range, "0,4,0.05")]
	public float flyAnimationSpeedScale = 0.8f;

	[Export(PropertyHint.Range, "0,1,0.01")]
	public double animationBlend = 0.2;

	[Export(PropertyHint.None, "")]
	public StringName flyEvent = "ToFly";

	[Export(PropertyHint.None, "")]
	public StringName landEvent = "ToLand";

	[Export(PropertyHint.None, "")]
	public StringName idleEvent = "ToIdle";

	[Export(PropertyHint.None, "")]
	public StringName flyStateName = "Fly";

	[Export(PropertyHint.None, "")]
	public bool suppressCollisionInFlight = true;

	[Export(PropertyHint.None, "")]
	public bool restoreCollisionOnDisable = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ImpFlightComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.flyAudio)
		{
			flyAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.flyAnimationSpeedScale)
		{
			flyAnimationSpeedScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.animationBlend)
		{
			animationBlend = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.flyEvent)
		{
			flyEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.landEvent)
		{
			landEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleEvent)
		{
			idleEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.flyStateName)
		{
			flyStateName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.suppressCollisionInFlight)
		{
			suppressCollisionInFlight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.restoreCollisionOnDisable)
		{
			restoreCollisionOnDisable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.flyAudio)
		{
			value = VariantUtils.CreateFrom(in flyAudio);
			return true;
		}
		if (name == PropertyName.flyAnimationSpeedScale)
		{
			value = VariantUtils.CreateFrom(in flyAnimationSpeedScale);
			return true;
		}
		if (name == PropertyName.animationBlend)
		{
			value = VariantUtils.CreateFrom(in animationBlend);
			return true;
		}
		if (name == PropertyName.flyEvent)
		{
			value = VariantUtils.CreateFrom(in flyEvent);
			return true;
		}
		if (name == PropertyName.landEvent)
		{
			value = VariantUtils.CreateFrom(in landEvent);
			return true;
		}
		if (name == PropertyName.idleEvent)
		{
			value = VariantUtils.CreateFrom(in idleEvent);
			return true;
		}
		if (name == PropertyName.flyStateName)
		{
			value = VariantUtils.CreateFrom(in flyStateName);
			return true;
		}
		if (name == PropertyName.suppressCollisionInFlight)
		{
			value = VariantUtils.CreateFrom(in suppressCollisionInFlight);
			return true;
		}
		if (name == PropertyName.restoreCollisionOnDisable)
		{
			value = VariantUtils.CreateFrom(in restoreCollisionOnDisable);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.flyAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.flyAnimationSpeedScale, PropertyHint.Range, "0,4,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.animationBlend, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.flyEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.landEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.flyStateName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.suppressCollisionInFlight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.restoreCollisionOnDisable, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.flyAudio, Variant.From(in flyAudio));
		info.AddProperty(PropertyName.flyAnimationSpeedScale, Variant.From(in flyAnimationSpeedScale));
		info.AddProperty(PropertyName.animationBlend, Variant.From(in animationBlend));
		info.AddProperty(PropertyName.flyEvent, Variant.From(in flyEvent));
		info.AddProperty(PropertyName.landEvent, Variant.From(in landEvent));
		info.AddProperty(PropertyName.idleEvent, Variant.From(in idleEvent));
		info.AddProperty(PropertyName.flyStateName, Variant.From(in flyStateName));
		info.AddProperty(PropertyName.suppressCollisionInFlight, Variant.From(in suppressCollisionInFlight));
		info.AddProperty(PropertyName.restoreCollisionOnDisable, Variant.From(in restoreCollisionOnDisable));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.flyAudio, out var value))
		{
			flyAudio = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.flyAnimationSpeedScale, out var value2))
		{
			flyAnimationSpeedScale = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.animationBlend, out var value3))
		{
			animationBlend = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.flyEvent, out var value4))
		{
			flyEvent = value4.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.landEvent, out var value5))
		{
			landEvent = value5.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleEvent, out var value6))
		{
			idleEvent = value6.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.flyStateName, out var value7))
		{
			flyStateName = value7.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.suppressCollisionInFlight, out var value8))
		{
			suppressCollisionInFlight = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.restoreCollisionOnDisable, out var value9))
		{
			restoreCollisionOnDisable = value9.As<bool>();
		}
	}
}
