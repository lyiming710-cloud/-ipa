using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/RiseComponent/RiseComponentDefinition.cs")]
public class RiseComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName randomDurationMin = "randomDurationMin";

		public static readonly StringName randomDurationMax = "randomDurationMax";

		public static readonly StringName effectDelay = "effectDelay";

		public static readonly StringName stateStartPhysicsFrames = "stateStartPhysicsFrames";

		public static readonly StringName landDiscardRevealOffset = "landDiscardRevealOffset";

		public static readonly StringName discardResetPosition = "discardResetPosition";

		public static readonly StringName discardShaderParameter = "discardShaderParameter";

		public static readonly StringName riseEase = "riseEase";

		public static readonly StringName riseTransition = "riseTransition";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Range, "0.01,10,0.01")]
	public float randomDurationMin = 0.4f;

	[Export(PropertyHint.Range, "0.01,10,0.01")]
	public float randomDurationMax = 0.6f;

	[Export(PropertyHint.Range, "0,5,0.01")]
	public float effectDelay = 0.1f;

	[Export(PropertyHint.Range, "0,8,1")]
	public int stateStartPhysicsFrames = 2;

	[Export(PropertyHint.Range, "0,64,1")]
	public float landDiscardRevealOffset = 24f;

	[Export(PropertyHint.None, "")]
	public float discardResetPosition = 10000f;

	[Export(PropertyHint.None, "")]
	public StringName discardShaderParameter = "discardDownPos";

	[Export(PropertyHint.None, "")]
	public Tween.EaseType riseEase = Tween.EaseType.Out;

	[Export(PropertyHint.None, "")]
	public Tween.TransitionType riseTransition = Tween.TransitionType.Cubic;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new RiseComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.randomDurationMin)
		{
			randomDurationMin = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.randomDurationMax)
		{
			randomDurationMax = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.effectDelay)
		{
			effectDelay = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.stateStartPhysicsFrames)
		{
			stateStartPhysicsFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.landDiscardRevealOffset)
		{
			landDiscardRevealOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardResetPosition)
		{
			discardResetPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardShaderParameter)
		{
			discardShaderParameter = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.riseEase)
		{
			riseEase = VariantUtils.ConvertTo<Tween.EaseType>(in value);
			return true;
		}
		if (name == PropertyName.riseTransition)
		{
			riseTransition = VariantUtils.ConvertTo<Tween.TransitionType>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.randomDurationMin)
		{
			value = VariantUtils.CreateFrom(in randomDurationMin);
			return true;
		}
		if (name == PropertyName.randomDurationMax)
		{
			value = VariantUtils.CreateFrom(in randomDurationMax);
			return true;
		}
		if (name == PropertyName.effectDelay)
		{
			value = VariantUtils.CreateFrom(in effectDelay);
			return true;
		}
		if (name == PropertyName.stateStartPhysicsFrames)
		{
			value = VariantUtils.CreateFrom(in stateStartPhysicsFrames);
			return true;
		}
		if (name == PropertyName.landDiscardRevealOffset)
		{
			value = VariantUtils.CreateFrom(in landDiscardRevealOffset);
			return true;
		}
		if (name == PropertyName.discardResetPosition)
		{
			value = VariantUtils.CreateFrom(in discardResetPosition);
			return true;
		}
		if (name == PropertyName.discardShaderParameter)
		{
			value = VariantUtils.CreateFrom(in discardShaderParameter);
			return true;
		}
		if (name == PropertyName.riseEase)
		{
			value = VariantUtils.CreateFrom(in riseEase);
			return true;
		}
		if (name == PropertyName.riseTransition)
		{
			value = VariantUtils.CreateFrom(in riseTransition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.randomDurationMin, PropertyHint.Range, "0.01,10,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.randomDurationMax, PropertyHint.Range, "0.01,10,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.effectDelay, PropertyHint.Range, "0,5,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.stateStartPhysicsFrames, PropertyHint.Range, "0,8,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.landDiscardRevealOffset, PropertyHint.Range, "0,64,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.discardResetPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.discardShaderParameter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.riseEase, PropertyHint.Enum, "In,Out,InOut,OutIn", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.riseTransition, PropertyHint.Enum, "Linear,Sine,Quint,Quart,Quad,Expo,Elastic,Cubic,Circ,Bounce,Back,Spring", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.randomDurationMin, Variant.From(in randomDurationMin));
		info.AddProperty(PropertyName.randomDurationMax, Variant.From(in randomDurationMax));
		info.AddProperty(PropertyName.effectDelay, Variant.From(in effectDelay));
		info.AddProperty(PropertyName.stateStartPhysicsFrames, Variant.From(in stateStartPhysicsFrames));
		info.AddProperty(PropertyName.landDiscardRevealOffset, Variant.From(in landDiscardRevealOffset));
		info.AddProperty(PropertyName.discardResetPosition, Variant.From(in discardResetPosition));
		info.AddProperty(PropertyName.discardShaderParameter, Variant.From(in discardShaderParameter));
		info.AddProperty(PropertyName.riseEase, Variant.From(in riseEase));
		info.AddProperty(PropertyName.riseTransition, Variant.From(in riseTransition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.randomDurationMin, out var value))
		{
			randomDurationMin = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName.randomDurationMax, out var value2))
		{
			randomDurationMax = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.effectDelay, out var value3))
		{
			effectDelay = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.stateStartPhysicsFrames, out var value4))
		{
			stateStartPhysicsFrames = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.landDiscardRevealOffset, out var value5))
		{
			landDiscardRevealOffset = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardResetPosition, out var value6))
		{
			discardResetPosition = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardShaderParameter, out var value7))
		{
			discardShaderParameter = value7.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.riseEase, out var value8))
		{
			riseEase = value8.As<Tween.EaseType>();
		}
		if (info.TryGetProperty(PropertyName.riseTransition, out var value9))
		{
			riseTransition = value9.As<Tween.TransitionType>();
		}
	}
}
